
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPRPUmsatzTotal.ClsDataDetail
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Logging

Public Class ClsGetSQLString

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Private m_md As Mandant
	Private m_common As CommonSetting
	Private m_utility As Utilities
	Private m_path As ClsProgPath

	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_MandantXMLFile As String
	Private m_FibuSetting As String
	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_SearchCriteria As SearchCriteria


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING As String = "MD_{0}/BuchungsKonten"

#End Region


#Region "Constructor"

	Public Sub New(ByVal _searchCriteria As SearchCriteria)

		m_md = New Mandant
		m_common = New CommonSetting
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI
		m_path = New ClsProgPath

		m_SearchCriteria = _searchCriteria

		Try

			m_FibuSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING, m_InitialData.MDData.MDNr)
			m_MandantXMLFile = m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, If(m_SearchCriteria Is Nothing, Now.Year, m_SearchCriteria.FirstYear))
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUi.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

		End Try

	End Sub

#End Region


	Function LoadJournalDataWithCustomerForOutput(ByVal strFMonth As Integer, ByVal strLMonth As Integer, ByVal strYear As Integer) As Boolean
		Dim success As Boolean = True
		Dim sql As String

		sql = "Select * From #UmsatzJournal_New UmJ "

		success = success AndAlso InsertCustomerDataToFinalDb(sql)

		Return success
	End Function

	Function InsertCustomerDataToFinalDb(ByVal strQuery As String) As Boolean
		Dim success As Boolean = True
		Dim sSql As String

		Try
			sSql = String.Format("Begin Try Drop Table [UmsatzJournal_New_{0}] End Try Begin Catch End Catch; ", _ClsProgSetting.GetLogedUSGuid)
			sSql &= String.Format("Select * Into [UmsatzJournal_New_{0}] ", _ClsProgSetting.GetLogedUSGuid)
			sSql &= "From #UmsatzJournal_New UmJ "

			Dim cmd As SqlClient.SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			m_Logger.LogError(String.Format("customerNumber={0} | {1}", m_SearchCriteria.customerNumber, ex.ToString))

			Return False
		End Try

		Return success
	End Function

	''' <summary>
	''' Daten aufstellen für Ausdruck
	''' </summary>
	Function GetUJournalQueryForOutput(ByVal strFMonth As Integer, ByVal strLMonth As Integer, ByVal strYear As Integer) As String
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim strResult As String = "Select * From #UmsatzJournal_New UmJ "
		strResult += String.Format("Where UserNr = {0} ", iSelectedUSNr)

		Dim strQueryString As String = GetFilterName4Search(strFMonth, strLMonth, strYear, strResult)
		strResult = strQueryString & " "
		strResult += GetSortString()
		InsertDataToFinalDb(strResult, False)

		strResult = GetUJournalQueryForNormalOutput(strFMonth, strLMonth, strYear, False)
		strResult += GetSortString()

		Return strResult
	End Function

	Function GetUJournalQueryForGroupedOutput(ByVal strFMonth As Integer,
																						ByVal strLMonth As Integer,
																						ByVal strYear As Integer) As String
		Dim strResult As String = "Select "
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim strLAPflicht As String = GetLAAHVPflicht(strYear)
		Dim bNetto530 As Boolean = Not CBool(strLAPflicht.Split(CChar("#"))(0))
		Dim bNetto630 As Boolean = Not CBool(strLAPflicht.Split(CChar("#"))(1))
		Dim bNetto730 As Boolean = Not CBool(strLAPflicht.Split(CChar("#"))(2))

		Try
			strResult += "sum(KDTotal) as KDTotal, sum(KDTotalInd) As KDTotalInd, sum(KDTotalFest) As KDTotalFest, "
			strResult += "sum(KDTotalG) as KDTotalG, "
			strResult += "sum(KDTotalTempSKonto) As KDTotalTempSKonto, sum(KDTotalIndSKonto) As KDTotalIndSKonto, "
			strResult += "sum(KDTotalFSKonto) As KDTotalFSKonto, "

			strResult += "sum(KDVerlustA) as KDVerlustA, sum(KDVerlustF) as KDVerlustF, "
			strResult += "sum(KDVerlustInd) As KDVerlustInd, "
			strResult += "sum(KDGuA) as KDGuA, sum(KDGuF) as KDGuF , "
			strResult += "sum(KDGuInd) As KDGuInd, "
			strResult += "sum(KDRuA) as KDRuA, sum(KDRuF) as KDRuF, "
			strResult += "sum(KDRuInd) As KDRuInd, "

			strResult += "sum(KDTotalFOp) as KDTotalFOp, "

			strResult += "sum(Bruttolohn) as Bruttolohn, sum(AHVLohn) As AHVLohn, sum(AGBetrag) As AGBetrag, "
			strResult += "sum(FremdLeistung) As FremdLeistung, "
			strResult += "sum(FerBack) as Ferback, sum(FeierBack) As FeierBack, sum(LO13Back) As LO13Back, sum(TimeBack) As TimeBack, "
			strResult += "sum(FerAus) as FerAus, sum(FeierAus) As FeierAus, sum(LO13Aus) As LO13Aus, sum(TimeAus) As TimeAus, "

			strResult += "sum([AdminKosten]) as [AdminKosten], sum(_TempUmsatz) as _TempUmsatz, sum(_IndUmsatz) As _IndUmsatz, "
			strResult += "sum(_FestUmsatz) As _FestUmsatz, "
			strResult += "sum(_Lohnaufwand_1) as _Lohnaufwand_1, sum(_Lohnaufwand_2) As _Lohnaufwand_2, sum(AGProz) As AGProz, "
			strResult += "sum(AGBetrag_2) As AGBetrag_2, "
			strResult += "sum(_BGTemp) as _BGTemp, sum(_BGInd) As _BGInd, sum(_BGFest) As _BGFest, sum([_Marge]) As [_Marge], "

			strResult += "sum(F_TempUmsatz) as F_TempUmsatz, sum(F_IndUmsatz) As F_IndUmsatz, sum(F_FestUmsatz) As F_FestUmsatz, "
			strResult += "sum(F_Lohnaufwand_1) As F_Lohnaufwand_1, "
			strResult += "sum(F_Lohnaufwand_2) as F_Lohnaufwand_2, "

			strResult += "sum([_PAnteil_F_BGT]) as [_PAnteil_F_BGT], sum([_PAnteil_F_BGI]) As [_PAnteil_F_BGI], "
			strResult += "sum([_PAnteil_F_BGF]) As [_PAnteil_F_BGF], "

			strResult += "sum(_530) As _530, sum(_630) As _630, sum(_730) As _730, sum(_800) As _800, "

			'If bNetto530 Then
			'  strResult += "'1' As _530_, "
			'Else
			'  strResult += "'0' As _530_, "
			'End If
			'If bNetto630 Then
			'  strResult += "'1' As _630_, "
			'Else
			'  strResult += "'0' As _630_, "
			'End If
			'If bNetto730 Then
			'  strResult += "'1' As _730_, "
			'Else
			'  strResult += "'0' As _730_, "
			'End If
			'strResult += "'1' As _800_, "

			strResult &= "_530_, _630_, _730_, "

			strResult &= "F_BGTemp, F_BGInd, F_BGFest, "
			strResult &= "U_BGTemp, U_BGInd, U_BGFest, "

			strResult += String.Format("USFiliale From [UmsatzJournal_New_{0}] UmJ ", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
			strResult += String.Format("Where UserNr = {0} ", iSelectedUSNr)

			Dim strQueryString As String = GetFilterName4Search(strFMonth, strLMonth, strYear, strResult)
			strResult = strQueryString & " Group By USFiliale "
			strResult &= ", F_BGTemp, F_BGInd, F_BGFest, "
			strResult &= "U_BGTemp, U_BGInd, U_BGFest, "
			strResult &= "_530_, _630_, _730_ "

			strResult &= "Order By USFiliale, "
			strResult &= "F_BGTemp, F_BGInd, F_BGFest, "
			strResult &= "U_BGTemp, U_BGInd, U_BGFest "

			InsertDataToFinalDb(strResult, True)

			Dim sSql As String = String.Format("Begin Try Drop Table [UmsatzJournal_New_Grouped_{0}] End Try Begin Catch End Catch ", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
			Dim cmd As SqlClient.SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()

		Catch ex As Exception

		End Try

		Return strResult
	End Function

	Function GetUJournalQueryForNormalOutput(ByVal strFMonth As Integer,
																					 ByVal strLMonth As Integer,
																					 ByVal strYear As Integer,
																					 ByVal bGrouped As Boolean) As String
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim strResult As String = String.Format("Select * From [UmsatzJournal_New_{0}{1}] UmJ ",
																						CStr(IIf(bGrouped, "Grouped_", "")), _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
		strResult += String.Format("Where UserNr = {0} ", iSelectedUSNr)

		Return strResult
	End Function

	Function InsertDataToFinalDb(ByVal strQuery As String, ByVal bGrouped As Boolean) As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		'Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr

		Try
			sSql = String.Format("Begin Try Drop Table [UmsatzJournal_New_{0}{1}] End Try Begin Catch End Catch", CStr(IIf(bGrouped, "Grouped_", "")), _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
			Dim cmd As SqlClient.SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()
		Catch ex As Exception

		End Try

		Try
			sSql = String.Format("Select * Into [UmsatzJournal_New_{0}{1}] ", CStr(IIf(bGrouped, "Grouped_", "")), _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
			sSql &= "From #UmsatzJournal_New UmJ "
			sSql &= Mid(strQuery, strQuery.ToUpper.IndexOf(" Where".ToUpper) + 2, strQuery.Length)

			If Not bGrouped Then
				Dim cmd As SqlClient.SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
				cmd.ExecuteNonQuery()
			End If

		Catch ex As Exception

		End Try

		Return sSql
	End Function

#Region "Lohn-Query..."

	Function GetLOLQuery4Vars_1(ByVal strSelectedKst3Bez As String,
															 ByVal strFMonth As Integer,
															 ByVal strLMonth As Integer,
															 ByVal strVYear As Integer,
															 ByVal strBYear As Integer,
															 ByVal i4What As Integer) As String
		Dim sSql As String = String.Empty
		If i4What <> -6 Then
			sSql = "Select Isnull(Sum(LOL.M_Btr), 0) As m_btr From dbo.LOL "
			sSql &= "Left Join dbo.LA On LOL.LANr = LA.Lanr And LA.LAJahr = LOL.Jahr "
			sSql &= "Left Join dbo.Mitarbeiter MA On LOL.MANr = MA.MANr "
			sSql &= "Left Join dbo.Kunden KD On LOL.DestKDNr = KD.KDNr "

		Else
			sSql = "Select IsNull(Sum(RPL.KompBetrag)*(-1), 0) As m_btr From dbo.RPL "
			sSql &= "Left Join dbo.RP On RPL.RPNr = RP.RPNr "
			sSql &= "Left Join dbo.Mitarbeiter MA On RPL.MANr = MA.MANr "
			sSql &= "Left Join dbo.Kunden KD On RPL.KDNr = KD.KDNr "

		End If

		'End If
		'-1 LOL-Datenbank (Brutto-Lohn)
		'-2 LOL-Datenbank (AHV-Lohn)
		'-3 LOL-Datenbank (Fremdleistungen)

		Dim whereQuery As String = GetQueryParam4LOLDb(sSql, strSelectedKst3Bez, strFMonth, strLMonth, strVYear, strBYear, i4What).Trim
		If i4What <> -6 Then
			whereQuery = String.Format("LOL.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "And", ""), whereQuery)
		Else
			whereQuery = String.Format("RP.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "And", ""), whereQuery)
		End If
		sSql &= If(Not String.IsNullOrWhiteSpace(whereQuery), " Where ", String.Empty) & whereQuery & " "

		Return sSql
	End Function

	' LO-Datenbank (pro LO einen Datensatz!!!)
	Function GetSQLStringFromLODb(ByVal strSelectedKst3Bez As String,
															 ByVal strFMonth As Integer,
															 ByVal strLMonth As Integer,
															 ByVal strVYear As Integer,
															 ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select LOL.LONr From LOL " ', LOL.LANr, IsNull(sum(LOL.m_Btr), 0.00) As LABetrag 
		sSql &= "Left Join LA On LOL.LANr = LA.LANr And LA.LAJahr = LOL.Jahr "
		sSql &= "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
		sSql &= "Left Join Kunden KD On LOL.DestKDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4LOLDb(sSql, strSelectedKst3Bez, strFMonth, strLMonth, strVYear, strBYear, -20).Trim

		strQueryString = String.Format("LOL.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty)) & strQueryString & " "
		sSql &= "Group By LOL.LONr "
		sSql &= "Order By LOL.LONr"

		Return sSql
	End Function

	' LOL-Datenbank (pro LONr die AG.-Lohnarten suchen!!!)
	Function GetSQLString4AGDb(ByVal iLONr As Integer) As EachPayrollAGAnteilData
		Dim result As EachPayrollAGAnteilData = Nothing
		Dim Sql As String = "[Get AGAnteil For Db1 each Payroll]"

		Dim cmd = New System.Data.SqlClient.SqlCommand(Sql, ClsDataDetail.Conn)
		'Trace.WriteLine(iLONr)

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("LONr", iLONr))
		listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(m_SearchCriteria.employeeNumber, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(m_SearchCriteria.customerNumber, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(m_SearchCriteria.esNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = OpenReader(ClsDataDetail.Conn, Sql, listOfParams, CommandType.StoredProcedure)
		'Dim reader = cmd.ExecuteReader

		Try

			If Not reader Is Nothing Then

				If reader.Read Then
					result = New EachPayrollAGAnteilData

					result.AHVAmountEachEmployee = SafeGetDecimal(reader, "AHVAnteilEachEmployee", 0)
					result.AHVAnteil = SafeGetDecimal(reader, "AHVAnteil", 0)

					result.AGAmountEachEmployee = SafeGetDecimal(reader, "AGAnteilEachEmployee", 0)
					result.AGBVGAmountEachEmployee = SafeGetDecimal(reader, "AGBVGAmountEachEmployee", 0)
					result.AGAnteil = SafeGetDecimal(reader, "AGAnteil", 0)

				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace())
			result = Nothing

		End Try


		Return result

	End Function

	' LAPflichtige Daten pro Lohnabrechnung
	Function GetLOLKSTQuery(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer,
													 ByVal iLONr As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select IsNull(Sum(LOL.m_Btr), 0) As TotalBtr From LOL Left Join LA On "
		sSql += "LOL.LANr = LA.LANr And LA.LAJahr = LOL.Jahr "
		sSql &= "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
		sSql &= "Left Join Kunden KD On LOL.DestKDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4LOLDb(sSql, strSelectedKst3Bez, strFMonth, strLMonth, strVYear, strBYear, -21).Trim

		strQueryString = String.Format("LOL.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty)) & strQueryString & " "
		sSql &= CStr(IIf(strQueryString <> String.Empty, "And ", String.Empty)) & "LONr = " & iLONr

		Return sSql
	End Function

	' LAPflichtige Daten pro Lohnabrechnung; EO-Entschädigung
	Function GetLOLEOKstQuery(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer,
													 ByVal iLONr As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select IsNull(Sum(LOL.m_Btr), 0) As TotalBtr From LOL "
		sSql &= "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
		sSql &= "Left Join Kunden KD On LOL.DestKDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4LOLDb(sSql, strSelectedKst3Bez, strFMonth, strLMonth, strVYear, strBYear, -22).Trim

		strQueryString = String.Format("LOL.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty)) & strQueryString & " "
		sSql &= CStr(IIf(strQueryString <> String.Empty, "And ", String.Empty)) & "LONr = " & iLONr

		Return sSql
	End Function

#End Region


#Region "Debitoren-Query..."

	' OP-Datenbank (Rapportzeilen)
	Function GetRPLOPQuery4Vars(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty
		'RPL.RENr, 
		sSql &= "Select RPL.RPNr, RPL.RENr,  IsNull(Sum(RPL.K_Betrag), 0) As BetragTTotal from RPL Left join RP On RPL.RPNr = RP.RPNr "
		sSql &= "Left Join Kunden KD On RPL.KDNr = KD.KDNr "
		sSql &= "Left Join Mitarbeiter MA On RPL.MANr = MA.MANr "

		Dim strQueryString As String = GetQueryParam4OPDb(strSelectedKst3Bez, "TOP").Trim

		strQueryString = String.Format("RP.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty)) & strQueryString & " "
		sSql &= "Group By RPL.RPNr, RPL.RENr "
		sSql &= "Order By RPL.RPNr "

		Return sSql
	End Function

	' OP-Datenbank (Rechnungsdatenbank: Individuelle Rechnungen)
	Function GetIOPQuery4Vars(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select RE.RENr, IsNull(Sum(RE.BetragInk), 0) - IsNull(Sum(RE.MwSt1), 0) As BetragITotal From RE "
		sSql += "Left Join Kunden KD On RE.KDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4OPDb(strSelectedKst3Bez, "IOP").Trim

		strQueryString = String.Format("RE.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty))
		sSql &= strQueryString & " "
		sSql &= "Group By RE.RENr "
		sSql &= "Order By RE.RENr "


		Return sSql
	End Function

	' OP-Datenbank (Rechnungsdatenbank: Festanstellungen Art = 'F')
	Function GetFOPQuery4Vars(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select RE.RENr, IsNull(Sum(RE.BetragInk), 0) - IsNull(Sum(RE.MwSt1), 0) As BetragFTotal From RE "
		sSql += "Left Join Kunden KD On RE.KDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4OPDb(strSelectedKst3Bez, "FESTOP").Trim

		strQueryString = String.Format("RE.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty))
		sSql &= strQueryString & " "
		sSql &= "Group By RE.RENr "
		sSql &= "Order By RE.RENr "

		Return sSql
	End Function

	' OP-Datenbank (Rechnungsdatenbank: Gutschriften Art = 'G' (Achtung: Negativer Betrag!!!))
	Function GetGOPQuery4Vars(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select RE.RENr, (IsNull(Sum(RE.BetragEx), 0) + IsNull(Sum(RE.BetragOhne), 0)) As BetragGTotal From RE "
		sSql += "Left Join Kunden KD On RE.KDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4OPDb(strSelectedKst3Bez, "GOP").Trim

		strQueryString = String.Format("RE.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty))
		sSql &= strQueryString & " "
		sSql &= "Group By RE.RENr "
		sSql &= "Order By RE.RENr "

		Return sSql
	End Function

	' OP-Datenbank (Rechnungsdatenbank: Gutschriften und Rückvergütungen Art = 'G' (Achtung: Negativer Betrag!!!))
	Function GetGOPQuery4Vars(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer,
													 ByVal strArt As String,
													 ByVal strArt_2 As String) As String
		Dim sSql As String = String.Empty

		sSql = "Select RE.RENr, (IsNull(Sum(RE.BetragEx), 0) + IsNull(Sum(RE.BetragOhne), 0)) As BetragGTotal From RE "
		sSql += "Left Join Kunden KD On RE.KDNr = KD.KDNr "

		'End If

		Dim strQueryString As String = GetQueryParam4OPDb(strSelectedKst3Bez, String.Format("{0}OP{1}", strArt, strArt_2)).Trim

		strQueryString = String.Format("RE.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty))
		sSql &= strQueryString & " "
		sSql &= "Group By RE.RENr "
		sSql &= "Order By RE.RENr "


		Return sSql
	End Function

	' FremdOpdatenbank
	Function GetFremdOPQuery4Vars(ByVal strSelectedKst3Bez As String,
																ByVal strFMonth As Integer, ByVal strLMonth As Integer,
																ByVal strVYear As Integer, ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select FOp.FOpNr, IsNull(Sum(FOp.BetragEx), 0) As BetragFopTotal From FremdOp FOp "
		sSql += "Left Join Kunden KD On FOp.KDNr = KD.KDNr "
		sSql += "Left Join Mitarbeiter MA On FOp.MANr = MA.MANr "

		Dim strQueryString As String = GetQueryParam4OPDb(strSelectedKst3Bez, "FOP").Trim

		strQueryString = String.Format("FOP.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty))
		sSql &= strQueryString & " "
		sSql &= "Group By FOp.FOpNr "
		sSql &= "Order By FOp.FOpNr "

		'm_Logger.LogDebug(sSql)
		'Trace.WriteLine(sSql)

		Return sSql
	End Function

	' OP-Datenbank (Rapportzeilen)
	Function GetSKontoQuery4Vars(ByVal strSelectedKst3Bez As String,
													 ByVal strFMonth As Integer,
													 ByVal strLMonth As Integer,
													 ByVal strVYear As Integer,
													 ByVal strBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select ZE.ZENr, ZE.RENr, ZE.FKSoll, ZE.Betrag, RE.MwStProz From ZE "
		sSql &= "Left Join RE On ZE.RENr = RE.RENr "
		sSql &= "Left Join Kunden KD On ZE.KDNr = KD.KDNr "

		Dim strQueryString As String = GetQueryParam4SKontoDb("", strSelectedKst3Bez).Trim
		sSql &= CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty)) & strQueryString & " "


		Return sSql
	End Function

#End Region

#Region "Suchselektion (Whereklausel)..."

	Function GetQueryParam4IOPDb(ByVal sSQLQuery As String, ByVal strSelectedKst3Bez As String) As String
		Dim sSql As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = _ClsProgSetting.GetUSFiliale()
		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")

		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		'Dim jahrVon As String = "0"
		'Dim jahrBis As String = "9999"
		'If m_SearchCriteria.FirstYear <> "" Then	' .Cbo_VYear_1.Text <> "" Then
		Dim jahrVon = m_SearchCriteria.FirstYear    ' .Cbo_VYear_1.Text
		'End If
		'If m_SearchCriteria.LastYear <> "" Then	 ' .Cbo_BYear_1.Text <> "" Then
		Dim jahrBis = m_SearchCriteria.LastYear    ' .Cbo_BYear_1.Text
		'End If

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		'{4} = " And "
		sSql += String.Format("{4}((Year(RE.FAK_DAT) = {0} And Month(RE.FAK_DAT) >= {2} And ({0} <> {1} Or (Month(RE.FAK_DAT) >= {2} And Month(RE.FAK_DAT) <= {3}))) Or ",
													jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, strAndString)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text, strAndString)
		sSql += String.Format("(Year(RE.FAK_DAT) > {0} And Year(RE.FAK_DAT) < {1}) Or ", jahrVon, jahrBis)

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		sSql += String.Format("(Year(RE.FAK_DAT) = {1} And Month(RE.FAK_DAT) <= {3} And ({0} <> {1} Or (Month(RE.FAK_DAT) >= {2} And Month(RE.FAK_DAT) <= {3}))))", jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text)

		' FilterBez zuordnen
		FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}",
															 m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear,
															 m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)
		'.Cbo_VMonth_1.Text, .Cbo_VYear_1.Text, .Cbo_BMonth_1.Text, .Cbo_BYear_1.Text, vbLf)


		' 3. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "RE.Kst"
		Dim strMaskKst3Bez As String = ClsDataDetail.GetFormVars(5).ToString ' .Cbo_RPKst3.Text
		'If strMaskKst3Bez = String.Empty And strSelectedKst3Bez <> String.Empty Then
		strMaskKst3Bez = strSelectedKst3Bez
		'End If
		If UCase(strMaskKst3Bez) <> String.Empty Then
			sZusatzBez = strMaskKst3Bez
			FilterBez += "Berater wie (" & ClsDataDetail.GetKstFullName & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Nationalität --------------------------------------------------------------------------------------------------
		' ist nicht wichtig da der Umsastz vom Kunden her kommt...
		' Land -------------------------------------------------------------------------------------------------------
		' ist nicht wichtig da der Umsastz vom Kunden her kommt...

		' Branche -----------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "RE.KDBranche"
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "KD-Branche wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(7).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.PLZ"
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName
			FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez.Trim, ",") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			sSql += strAndString & strFieldName & " In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

		End If

		' Ort -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.Ort"
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Ort wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(9).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' MANr -------------------------------------------------------------------------------------------------------

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "RE.KDNr"
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez += "Kundennummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------


		ClsDataDetail.GetFilterBez = FilterBez


		Return sSql
	End Function

	Function GetQueryParam4FOPDb(ByVal sSQLQuery As String, ByVal strSelectedKst3Bez As String) As String
		Dim sSql As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = _ClsProgSetting.GetUSFiliale()
		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")

		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		'Dim jahrVon As String = "0"
		'Dim jahrBis As String = "9999"
		'If m_SearchCriteria.FirstYear <> "" Then	' .Cbo_VYear_1.Text <> "" Then
		'End If
		'If m_SearchCriteria.LastYear <> "" Then	 ' .Cbo_BYear_1.Text <> "" Then
		'End If
		Dim jahrVon = m_SearchCriteria.FirstYear    ' .Cbo_VYear_1.Text
		Dim jahrBis = m_SearchCriteria.LastYear    ' .Cbo_BYear_1.Text

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		'{4} = " And "
		sSql += String.Format("{4}((Year(FOP.KrediOn) = {0} And Month(FOP.KrediOn) >= {2} And ({0} <> {1} Or (Month(FOP.KrediOn) >= {2} And Month(FOP.KrediOn) <= {3}))) Or ", jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, strAndString)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text, strAndString)
		sSql += String.Format("(Year(FOP.KrediOn) > {0} And Year(FOP.KrediOn) < {1}) Or ", jahrVon, jahrBis)

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		sSql += String.Format("(Year(FOP.KrediOn) = {1} And Month(FOP.KrediOn) <= {3} And ({0} <> {1} Or (Month(FOP.KrediOn) >= {2} And Month(FOP.KrediOn) <= {3}))))", jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text)

		' FilterBez zuordnen
		FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}",
															 m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear,
															 m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)
		'.Cbo_VMonth_1.Text, .Cbo_VYear_1.Text, .Cbo_BMonth_1.Text, .Cbo_BYear_1.Text, vbLf)


		' 3. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "fOP.KST3"
		'Dim strMaskKst3Bez As String = .Cbo_RPKst3.Text
		'If strMaskKst3Bez = String.Empty And strSelectedKst3Bez <> String.Empty Then
		'  strMaskKst3Bez = strSelectedKst3Bez
		'End If
		Dim strMaskKst3Bez As String = strSelectedKst3Bez

		If UCase(strMaskKst3Bez) <> String.Empty Then
			sZusatzBez = strMaskKst3Bez
			FilterBez += "Berater wie (" & sZusatzBez & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Branche -----------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "fop.KDBranche"
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Branche wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(7).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.PLZ"
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName
			FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez.Trim, ",") > 0 Then
				sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			End If
			sSql += strAndString & strFieldName & " In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

		End If

		' Ort -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.Ort"
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Ort wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(9).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If


		' Nationalität --------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "MA.Nationality"
		If UCase(ClsDataDetail.GetFormVars(10).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(10).ToString.Trim
			sZusatzBez = Replace(GetLandCode(sZusatzBez), ", ", ",")
			FilterBez += "Nationalität wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			sSql += strAndString & strFieldName & " "
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += "Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += "In ('" & sZusatzBez & "')"
			End If
		End If

		' Land -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "MA.Land"
		If UCase(ClsDataDetail.GetFormVars(11).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(11).ToString.Trim
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Land wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & GetLandCode(sZusatzBez) & "')"
			End If
		End If


		' MANr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.employeeNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "FOP.MANr"
			sZusatzBez = m_SearchCriteria.employeeNumber
			FilterBez += "Kandidatennummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "FOP.KDNr"
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez += "Kundennummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "FOP.ESNr"
			sZusatzBez = m_SearchCriteria.esNumber
			FilterBez += "Einsatznummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If


		ClsDataDetail.GetFilterBez = FilterBez


		Return sSql
	End Function

	Function GetQueryParam4SKontoDb(ByVal sSQLQuery As String, ByVal strSelectedKst3Bez As String) As String
		Dim sSql As String = sSQLQuery
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = _ClsProgSetting.GetUSFiliale()
		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		Dim liKontoNr As List(Of Integer) = GetKontoNr()

		'0  strResult.Add(iSKAOPMwSt)
		'1  strResult.Add(iSKIOPMwSt)
		'2  strResult.Add(iSKFOPMwSt)
		'3  strResult.Add(iSKAOP)
		'4  strResult.Add(iSKIOP)
		'5  strResult.Add(iSKFOP)

		'6  strResult.Add(iErAOPMwSt)
		'7  strResult.Add(iErIOPMwSt)
		'8  strResult.Add(iErFOPMwSt)
		'9  strResult.Add(iErAOP)
		'10 strResult.Add(iErIOP)
		'11 strResult.Add(iErFOP)

		'12 strResult.Add(CInt(strSOPVerluste))

		Dim iSKAOPMwSt As Integer = liKontoNr(0)
		Dim iSKAOP As Integer = liKontoNr(3)
		Dim iSKIOPMwSt As Integer = liKontoNr(1)
		Dim iSKIOP As Integer = liKontoNr(4)
		Dim iSKFOPMwSt As Integer = liKontoNr(2)
		Dim iSKFOP As Integer = liKontoNr(5)

		' debitoren verluste
		Dim iVerAOPMwSt As Integer = liKontoNr(13) ' = (iVerAOPMwSt)
		Dim iVerIOPMwSt As Integer = liKontoNr(14)  'As Integer = (iVerIOPMwSt)
		Dim iVerFOPMwSt As Integer = liKontoNr(15) '= (iVerFOPMwSt)
		Dim iVerAOP As Integer = liKontoNr(16) '= (iVerAOP)
		Dim iVerIOP As Integer = liKontoNr(17) '= (iVerIOP)
		Dim iVerFOP As Integer = liKontoNr(18) '= (iVerFOP)

		' debitoren erlöse
		Dim iErAOPMwSt As Integer = liKontoNr(6)
		Dim iErAOP As Integer = liKontoNr(9)
		Dim iErIOPMwSt As Integer = liKontoNr(7)
		Dim iErIOP As Integer = liKontoNr(10)
		Dim iErFOPMwSt As Integer = liKontoNr(8)
		Dim iErFOP As Integer = liKontoNr(11)


		Dim iRuAOPMwSt As Integer = 0 ' liKontoNr(19) ' = (iRuAOPMwSt)
		Dim iRuIOPMwSt As Integer = 0 ' liKontoNr(20) '= (iRuIOPMwSt)
		Dim iRuFOPMwSt As Integer = 0 ' liKontoNr(21) '= (iRuFOPMwSt)
		Dim iRuAOP As Integer = 0 ' liKontoNr(22) '= (iRuAOP)
		Dim iRuIOP As Integer = 0 ' liKontoNr(23) '= (iRuIOP)
		Dim iRuFOP As Integer = 0 ' liKontoNr(24) '= (iRuFOP)

		Dim iGuAOPMwSt As Integer = 0 ' liKontoNr(25) '= (iGuAOPMwSt)
		Dim iGuIOPMwSt As Integer = 0 ' liKontoNr(26) '= (iGuIOPMwSt)
		Dim iGuFOPMwSt As Integer = 0 ' liKontoNr(27) ' = (iGuFOPMwSt)
		Dim iGuAOP As Integer = 0 ' liKontoNr(28) '= (iGuAOP)
		Dim iGuIOP As Integer = 0 ' liKontoNr(29) '= (iGuIOP)
		Dim iGuFOP As Integer = 0 ' liKontoNr(30) '= (iGuFOP)




		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")

		sSql &= "ZE.FKSoll In ("
		sSql &= iSKAOPMwSt & " " & CStr(IIf(iSKAOP = 0, "", ", " & iSKAOP)) & " "
		sSql &= CStr(IIf(iSKIOPMwSt = 0, "", ", " & iSKIOPMwSt)) & " " & CStr(IIf(iSKIOP = 0, "", ", " & iSKIOP)) & " "
		sSql &= CStr(IIf(iSKFOPMwSt = 0, "", ", " & iSKFOPMwSt)) & " " & CStr(IIf(iSKFOP = 0, "", ", " & iSKFOP)) & " "

		sSql &= CStr(IIf(iVerAOPMwSt = 0, "", ", " & iVerAOPMwSt)) & " " & CStr(IIf(iVerIOPMwSt = 0, "", ", " & iVerIOPMwSt)) & " "
		sSql &= CStr(IIf(iErIOPMwSt = 0, "", ", " & iErIOPMwSt)) & " " & CStr(IIf(iErIOP = 0, "", ", " & iErIOP)) & " "
		sSql &= CStr(IIf(iErFOPMwSt = 0, "", ", " & iErFOPMwSt)) & " " & CStr(IIf(iErFOP = 0, "", ", " & iErFOP)) & " "

		sSql &= CStr(IIf(iErAOPMwSt = 0, "", ", " & iErAOPMwSt)) & " " & CStr(IIf(iErAOP = 0, "", ", " & iErAOP)) & " "
		sSql &= CStr(IIf(iVerFOPMwSt = 0, "", ", " & iVerFOPMwSt)) & " " & CStr(IIf(iVerAOP = 0, "", ", " & iVerAOP)) & " "
		sSql &= CStr(IIf(iVerIOP = 0, "", ", " & iVerIOP)) & " " & CStr(IIf(iVerFOP = 0, "", ", " & iVerFOP)) & " "

		'sSql &= CStr(IIf(iRuAOPMwSt = 0, "", ", " & iRuAOPMwSt)) & " " & CStr(IIf(iRuIOPMwSt = 0, "", ", " & iRuIOPMwSt)) & " "
		'sSql &= CStr(IIf(iRuFOPMwSt = 0, "", ", " & iRuFOPMwSt)) & " " & CStr(IIf(iRuAOP = 0, "", ", " & iRuAOP)) & " "
		'sSql &= CStr(IIf(iRuIOP = 0, "", ", " & iRuIOP)) & " " & CStr(IIf(iRuFOP = 0, "", ", " & iRuFOP)) & " "
		'sSql &= CStr(IIf(iGuAOPMwSt = 0, "", ", " & iGuAOPMwSt)) & " " & CStr(IIf(iGuIOPMwSt = 0, "", ", " & iGuIOPMwSt)) & " "
		'sSql &= CStr(IIf(iGuFOPMwSt = 0, "", ", " & iGuFOPMwSt)) & " " & CStr(IIf(iGuAOP = 0, "", ", " & iGuAOP)) & " "
		'sSql &= CStr(IIf(iGuIOP = 0, "", ", " & iGuIOP)) & " " & CStr(IIf(iGuFOP = 0, "", ", " & iGuFOP)) & ") "

		sSql &= ") "

		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		'Dim jahrVon As String = "0"
		'Dim jahrBis As String = "9999"
		'If m_SearchCriteria.FirstYear <> "" Then	' .Cbo_VYear_1.Text <> "" Then
		Dim jahrVon = m_SearchCriteria.FirstYear    ' .Cbo_VYear_1.Text
		'End If
		'If m_SearchCriteria.LastYear <> "" Then	 ' .Cbo_BYear_1.Text <> "" Then
		Dim jahrBis = m_SearchCriteria.LastYear    ' .Cbo_BYear_1.Text
		'End If

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		'{4} = " And "
		sSql += String.Format("{4}((Year(ZE.V_Date) = {0} And Month(ZE.V_Date) >= {2} And ({0} <> {1} Or (Month(ZE.V_Date) >= {2} And Month(ZE.V_Date) <= {3}))) Or ", jahrVon, jahrBis,
													 m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, strAndString)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text, strAndString)
		sSql += String.Format("(Year(ZE.V_Date) > {0} And Year(ZE.V_Date) < {1}) Or ", jahrVon, jahrBis)

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		sSql += String.Format("(Year(ZE.V_Date) = {1} And Month(ZE.V_Date) <= {3} And ({0} <> {1} Or (Month(ZE.V_Date) >= {2} And Month(ZE.V_Date) <= {3}))))", jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text)

		' FilterBez zuordnen
		FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}",
															 m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear,
															 m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)
		'.Cbo_VMonth_1.Text, .Cbo_VYear_1.Text, .Cbo_BMonth_1.Text, .Cbo_BYear_1.Text, vbLf)


		' 3. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "RE.Kst"
		Dim strMaskKst3Bez As String = ClsDataDetail.GetFormVars(5).ToString ' .Cbo_RPKst3.Text
		'If strMaskKst3Bez = String.Empty And strSelectedKst3Bez <> String.Empty Then
		strMaskKst3Bez = strSelectedKst3Bez
		'End If
		If UCase(strMaskKst3Bez) <> String.Empty Then
			sZusatzBez = strMaskKst3Bez
			FilterBez += "Berater wie (" & ClsDataDetail.GetKstFullName & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Nationalität --------------------------------------------------------------------------------------------------
		' ist nicht wichtig da der Umsastz vom Kunden her kommt...
		' Land -------------------------------------------------------------------------------------------------------
		' ist nicht wichtig da der Umsastz vom Kunden her kommt...

		' Branche -----------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "RE.KDBranche"
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "KD-Branche wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(7).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.PLZ"
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName
			FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez.Trim, ",") > 0 Then
				sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			End If
			sSql += strAndString & strFieldName & " In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

		End If

		' Ort -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.Ort"
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Ort wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(9).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' MANr -------------------------------------------------------------------------------------------------------

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "ZE.KDNr"
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez += "Kundennummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------

		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql
	End Function

	Function GetQueryParam4LOLDb(ByVal sSQLQuery As String,
															 ByVal strSelectedKst3Bez As String,
															 ByVal strFMonth As Integer,
															 ByVal strLMonth As Integer,
															 ByVal strVYear As Integer,
															 ByVal strBYear As Integer,
															 ByVal i4What As Integer) As String
		Dim sSql As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		If i4What = -11 Then
			Trace.WriteLine(strSelectedKst3Bez)
		End If

		If i4What <> -6 Then sSql += "LOL.m_Btr <> 0 " Else sSql += "RPL.m_Betrag <> 0 "
		If i4What = -1 Then       ' Bruttolohn
			sSql += "And LA.DB1_Bruttopflichtig = 1 "

		ElseIf i4What = -2 Then   ' AHV-Lohn
			sSql += "And LA.DB1_AHVpflichtig = 1 "


		ElseIf i4What = -3 Then   ' Ferback
			sSql += String.Format("And LOL.LANr = {0} ", If(ClsDataDetail.IsFerienAsNetto_2, 8000.02, 602)) ' "And LOL.LANr = 602 "
		ElseIf i4What = -4 Then   ' Feierback
			sSql += String.Format("And LOL.LANr = {0} ", If(ClsDataDetail.IsFeiertagAsNetto_2, 8000.01, 502)) '502 "
		ElseIf i4What = -5 Then   ' 13. Lohnback
			sSql += String.Format("And LOL.LANr = {0} ", If(ClsDataDetail.Is13LohnAsNetto_2, 8000.03, 702)) ' "And LOL.LANr = 702 "
		ElseIf i4What = -6 Then   ' Gleitzeit Back
			sSql += "And (RPL.KompBetrag <> 0 Or RPL.KompBetrag Is Not Null) And RP.LONr <> 0 "


		ElseIf i4What = -7 Then   ' FerAus
			sSql += "And LOL.LANr = 660 "
		ElseIf i4What = -8 Then   ' FeierAus
			sSql += "And LOL.LANr = 560 "
		ElseIf i4What = -9 Then   ' 13. LohnAus
			sSql += "And LOL.LANr = 760 "
		ElseIf i4What = -10 Then   ' Gleitzeit Aus
			sSql += "And LOL.LANr = 800 "

		ElseIf i4What = -13 Then   ' Fremdleistungen
			sSql += "And (LA.Sum0Betrag = '36' or LA.Sum1Betrag = '36' or LA.Sum2Betrag = '36' or LA.Sum3Betrag = '36') "
			sSql += "And (LA.Sum0Betrag Is Not Null And LA.Sum1Betrag Is Not Null And LA.Sum2Betrag Is Not Null "
			sSql &= "And LA.Sum3Betrag Is Not Null) "


		ElseIf i4What = -20 Then   ' AG-Anteile; AHV-Basis und AG-Beitrag
			sSql += "And LA.DB1_AHVPflichtig = 1 "

		ElseIf i4What = -21 Then   ' AG-Anteile; AHV-pflichtige Lohnarten
			sSql += "And LA.DB1_AHVPflichtig = 1 "

		ElseIf i4What = -22 Then   ' AG-Anteile; EO-Entschädigung
			sSql += "And LOL.LANr In (2500) "

		End If


		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		Dim jahrVon = m_SearchCriteria.FirstYear
		Dim jahrBis = m_SearchCriteria.LastYear

		If i4What = -6 Then
			sSql &= strAndString & String.Format("((RP.Jahr = {0} And RP.Monat >= {1} And ", jahrVon, m_SearchCriteria.FirstMonth)
			sSql &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
			sSql &= String.Format("(RP.Monat >= {0} And RP.Monat <= {1}))) Or ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)

			sSql &= String.Format("(RP.Jahr > {0} And RP.Jahr < {1}) Or ", jahrVon, jahrBis)
			sSql &= String.Format("(RP.Jahr = {0} And RP.Monat <= {1} And ", jahrBis, m_SearchCriteria.LastMonth)
			sSql &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
			sSql &= String.Format("(RP.Monat >= {0} And RP.Monat <= {1})))) ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)

		Else
			sSql &= strAndString & String.Format("((LOL.Jahr = {0} And LOL.LP >= {1} And ", jahrVon, m_SearchCriteria.FirstMonth)
			sSql &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
			sSql &= String.Format("(LOL.LP >= {0} And LOL.LP <= {1}))) Or ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
			sSql &= String.Format("(LOL.Jahr > {0} And LOL.Jahr < {1}) Or ", jahrVon, jahrBis)
			sSql &= String.Format("(LOL.Jahr = {0} And LOL.LP <= {1} And ", jahrBis, m_SearchCriteria.LastMonth)
			sSql &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
			sSql &= String.Format("(LOL.LP >= {0} And LOL.LP <= {1})))) ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)

		End If

		' FilterBez zuordnen
		FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}", m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)


		' 3. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = CStr(IIf(i4What = -6, "RP.RPKST", "LOL.KST"))
		If UCase(strSelectedKst3Bez) <> String.Empty Then
			sZusatzBez = strSelectedKst3Bez
			FilterBez += "Berater wie (" & sZusatzBez & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Branche -----------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		' i4What = -6 heisst Kompensationszeiten von Gleitzeit, daher muss es vom RP nehmen
		strFieldName = CStr(IIf(i4What = -6, "RP.KDBranche", "LOL.ESBranche"))
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, "# ", "#")
			FilterBez += "Einsatzbranche wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(7).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.PLZ"
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName
			FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez.Trim, ",") > 0 Then
				sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			End If
			sSql += strAndString & strFieldName & " In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

		End If

		' Ort -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "KD.Ort"
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Ort wie (" & sZusatzBez & ") " & vbLf

			If InStr(ClsDataDetail.GetFormVars(9).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Nationalität --------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "MA.Nationality"
		If UCase(ClsDataDetail.GetFormVars(10).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(10).ToString.Trim
			sZusatzBez = Replace(GetLandCode(sZusatzBez), ", ", ",")
			FilterBez += "Nationalität wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			sSql += strAndString & strFieldName & " "
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += "Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += "In ('" & sZusatzBez & "')"
			End If
		End If

		' Land -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "MA.Land"
		If UCase(ClsDataDetail.GetFormVars(11).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(11).ToString.Trim
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += "Land wie (" & sZusatzBez & ") " & vbLf

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sSql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sSql += strAndString & strFieldName & " In ('" & GetLandCode(sZusatzBez) & "')"
			End If
		End If


		' MANr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.employeeNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "MA.MANr"
			sZusatzBez = m_SearchCriteria.employeeNumber
			FilterBez += "Kandidatennummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = "KD.KDNr"
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez += "Kundennummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
			strFieldName = CStr(IIf(i4What = -6, "RP.ESNr", "LOL.DestESNr"))
			sZusatzBez = m_SearchCriteria.esNumber
			FilterBez += "Einsatznummer wie (" & sZusatzBez & ") " & vbLf

			sSql += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If


		ClsDataDetail.GetFilterBez = FilterBez


		Return sSql
	End Function

	Function GetFilterName4Search(ByVal strFMonth As Integer, ByVal strLMonth As Integer, ByVal strYear As Integer, ByVal sSql As String) As String
		Dim FilterBez As String = String.Empty
		Dim sZusatzBez As String = String.Empty

		' Von / Bis-Monat 1 --------------------------------------------------------------------------------------
		Dim strAndString As String = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		Dim strFieldName As String = "UmJ.Monat"

		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		'Dim jahrVon As Integer = "0"
		'Dim jahrBis As Integer = "9999"
		'If m_SearchCriteria.FirstYear <> "" Then	' .Cbo_VYear_1.Text <> "" Then
		Dim jahrVon As Integer = m_SearchCriteria.FirstYear   ' .Cbo_VYear_1.Text
		'End If
		'If m_SearchCriteria.LastYear <> "" Then	 ' .Cbo_BYear_1.Text <> "" Then
		Dim jahrBis As Integer = m_SearchCriteria.LastYear    ' .Cbo_BYear_1.Text
		'End If

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		'{4} = " And "
		sSql += String.Format("{4}((UmJ.Jahr = {0} And UmJ.Monat >= {2} And ({0} <> {1} Or (UmJ.Monat >= {2} And UmJ.Monat <= {3}))) Or ", jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, strAndString)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text, strAndString)
		sSql += String.Format("(UmJ.Jahr > {0} And UmJ.Jahr < {1}) Or ", jahrVon, jahrBis)

		'{0} = Jahr-Von
		'{1} = Jahr-Bis
		'{2} = Monat-Von
		'{3} = Monat-Bis
		sSql += String.Format("(UmJ.Jahr = {1} And UmJ.Monat <= {3} And ({0} <> {1} Or (UmJ.Monat >= {2} And UmJ.Monat <= {3}))))",
													jahrVon, jahrBis,
													m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
		'.Cbo_VMonth_1.Text, .Cbo_BMonth_1.Text)

		' FilterBez zuordnen
		FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}",
															 m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear,
															 m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)
		'.Cbo_VMonth_1.Text, .Cbo_VYear_1.Text, .Cbo_BMonth_1.Text, .Cbo_BYear_1.Text, vbLf)


		' 3. KST -------------------------------------------------------------------------------------------------------
		Dim strName As String()
		Dim strMyName As String = String.Empty
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "UmJ.KST3_1"
		Dim strMaskKst3Bez As String = ClsDataDetail.GetFormVars(5).ToString ' .Cbo_RPKst3.Text
		If UCase(strMaskKst3Bez) <> String.Empty Then
			sZusatzBez = strMaskKst3Bez
			FilterBez += "Berater wie (" & ClsDataDetail.GetKstFullName & ") " & vbCrLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Filiale ------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		If UCase(ClsDataDetail.GetFormVars(6).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(6).ToString
			strFieldName = "UmJ.USFiliale"
			FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbCrLf
			sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"

		End If

		' ES_Branche -----------------------------------------------------------------------------------------------------
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			FilterBez += "Branche wie (" & sZusatzBez & ") " & vbCrLf
		End If

		' KD_Kanton -------------------------------------------------------------------------------------------------------
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim
			FilterBez += "Kanton wie (" & sZusatzBez.Trim & ") " & vbCrLf
		End If

		' KD_Ort -------------------------------------------------------------------------------------------------------
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim
			FilterBez += "Ort wie (" & sZusatzBez & ") " & vbCrLf
		End If

		' MA_Nationalität --------------------------------------------------------------------------------------------------
		If UCase(ClsDataDetail.GetFormVars(10).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(10).ToString.Trim
			FilterBez += "Nationalität wie (" & sZusatzBez & ") " & vbCrLf
		End If

		' MA_Land -------------------------------------------------------------------------------------------------------
		If UCase(ClsDataDetail.GetFormVars(11).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(11).ToString.Trim
			FilterBez += "Land wie (" & sZusatzBez & ") " & vbCrLf
		End If


		' MANr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.employeeNumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = m_SearchCriteria.employeeNumber
			FilterBez += String.Format("Kandidatennummer wie ({0}){1}", sZusatzBez, vbCrLf)
		End If

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez += String.Format("Kundennummer wie ({0}){1}", sZusatzBez, vbCrLf)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = m_SearchCriteria.esNumber
			FilterBez += String.Format("Einsatznummer wie ({0}){1}", sZusatzBez, vbCrLf)
		End If


		ClsDataDetail.GetFilterBez = FilterBez.Trim


		Return sSql
	End Function

	Function GetKontoNr() As List(Of Integer)
		Dim strResult As List(Of Integer) = FillMyLOiBetrag(31)   '(13)
		Dim strSOPVerluste As String = String.Empty

		'Dim _ClsReg As New SPProgUtility.ClsDivReg
		'Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()

		'Dim iSKAOPMwSt As Integer
		'Dim iSKAOP As Integer
		'Dim iSKIOPMwSt As Integer
		'Dim iSKIOP As Integer
		'Dim iSKFOPMwSt As Integer
		'Dim iSKFOP As Integer

		'Dim iErAOPMwSt As Integer
		'Dim iErAOP As Integer
		'Dim iErIOPMwSt As Integer
		'Dim iErIOP As Integer
		'Dim iErFOPMwSt As Integer
		'Dim iErFOP As Integer

		'Dim iVerAOPMwSt As Integer
		'Dim iVerAOP As Integer
		'Dim iVerIOPMwSt As Integer
		'Dim iVerIOP As Integer
		'Dim iVerFOPMwSt As Integer
		'Dim iVerFOP As Integer

		'Dim iRuAOPMwSt As Integer
		'Dim iRuAOP As Integer
		'Dim iRuIOPMwSt As Integer
		'Dim iRuIOP As Integer
		'Dim iRuFOPMwSt As Integer
		'Dim iRuFOP As Integer

		'Dim iGuAOPMwSt As Integer
		'Dim iGuAOP As Integer
		'Dim iGuIOPMwSt As Integer
		'Dim iGuIOP As Integer
		'Dim iGuFOPMwSt As Integer
		'Dim iGuFOP As Integer


		'Dim strQuery As String = String.Empty
		'Dim strBez As String = String.Empty


		Dim _1 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_1", m_FibuSetting)), 0)
		Dim _2 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_2", m_FibuSetting)), 0)

		Dim _3 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_3", m_FibuSetting)), 0)
		Dim _4 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_4", m_FibuSetting)), 0)
		Dim _5 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_5", m_FibuSetting)), 0)
		Dim _6 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_6", m_FibuSetting)), 0)
		Dim _7 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_7", m_FibuSetting)), 0)
		Dim _8 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_8", m_FibuSetting)), 0)
		Dim _9 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_9", m_FibuSetting)), 0)
		Dim _10 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_10", m_FibuSetting)), 0)
		Dim _11 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_11", m_FibuSetting)), 0)
		Dim _12 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_12", m_FibuSetting)), 0)
		Dim _13 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_13", m_FibuSetting)), 0)
		Dim _14 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_14", m_FibuSetting))
		Dim _15 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_15", m_FibuSetting)), 0)
		Dim _16 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_16", m_FibuSetting)), 0)
		Dim _17 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_17", m_FibuSetting)), 0)
		Dim _18 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_18", m_FibuSetting)), 0)
		Dim _19 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_19", m_FibuSetting)), 0)
		Dim _20 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_20", m_FibuSetting)), 0)
		Dim _21 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_21", m_FibuSetting)), 0)
		Dim _22 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_22", m_FibuSetting)), 0)
		Dim _23 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_23", m_FibuSetting)), 0)
		Dim _24 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_24", m_FibuSetting)), 0)

		Dim _25 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_25", m_FibuSetting)), 0)
		Dim _26 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_26", m_FibuSetting)), 0)
		Dim _27 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_27", m_FibuSetting)), 0)
		Dim _28 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_28", m_FibuSetting)), 0)
		Dim _35 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_35", m_FibuSetting)), 0)
		Dim _36 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_36", m_FibuSetting)), 0)

		Dim _33 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_33", m_FibuSetting)), 0)
		Dim _34 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_34", m_FibuSetting)), 0)

		Dim _29 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_29", m_FibuSetting)), 0)
		Dim _30 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_30", m_FibuSetting)), 0)
		Dim _31 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_31", m_FibuSetting)), 0)
		Dim _32 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_32", m_FibuSetting)), 0)
		Dim _37 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_37", m_FibuSetting)), 0)
		Dim _38 As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/_38", m_FibuSetting)), 0)




		' SKonto A Rechnungen -------------------------------------------------------------------------
		'strQuery = "//BuchungsKonten/_10"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iSKAOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iSKAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "10")))
		'End If
		strResult(0) = _10 ' (iSKAOPMwSt)

		' SKonto I Rechnungen
		'strQuery = "//BuchungsKonten/_11"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iSKIOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iSKIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "11")))
		'End If
		strResult(1) = _11 ' (iSKIOPMwSt)

		' SKonto F Rechnungen
		'strQuery = "//BuchungsKonten/_19"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iSKFOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iSKFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "19")))
		'End If
		strResult(2) = _19 ' (iSKFOPMwSt)

		' SKonto A Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_12"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iSKAOP = CInt(Val(strBez))
		'	'Else
		'	'	iSKAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "12")))
		'End If
		strResult(3) = _12 ' (iSKAOP)


		' SKonto I Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_13"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iSKIOP = CInt(Val(strBez))
		'	'Else
		'	'	iSKIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "13")))
		'End If
		strResult(4) = _13 ' (iSKIOP)


		' SKonto F Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_20"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iSKFOP = CInt(Val(strBez))
		'	'Else
		'	'	iSKFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "20")))
		'End If
		strResult(5) = _20 ' (iSKFOP)


		' Erlös A Rechnungen --------------------------------------------------------------------------
		'strQuery = "//BuchungsKonten/_2"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iErAOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iErAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "2")))
		'End If
		strResult(6) = _2 ' (iErAOPMwSt)

		' Erlös I Rechnungen 
		'strQuery = "//BuchungsKonten/_3"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iErIOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iErIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "3")))
		'End If
		strResult(7) = _3 ' (iErIOPMwSt)


		' Erlös F Rechnungen 
		'strQuery = "//BuchungsKonten/_17"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iErFOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iErFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "17")))
		'End If
		strResult(8) = _17 ' (iErFOPMwSt)


		' Erlös A Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_4"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iErAOP = CInt(Val(strBez))
		'	'Else
		'	'	iErAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "4")))
		'End If
		strResult(9) = _4 ' (iErAOP)

		' Erlös I Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_5"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iErIOP = CInt(Val(strBez))
		'	'Else
		'	'	iErIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "5")))
		'End If
		strResult(10) = _5 ' (iErIOP)

		' Erlös F Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_18"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iErFOP = CInt(Val(strBez))
		'	'Else
		'	'	iErFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "18")))
		'End If
		strResult(11) = _18 ' (iErFOP)


		' Sonstige Verluskonten
		' strQuery = "//BuchungsKonten/_14"
		' strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		' If strBez <> String.Empty Then
		'strSOPVerluste = (strBez)
		' Else
		'strSOPVerluste = _ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "14")
		' End If
		strResult(12) = 0
		'    strResult(12) = (CInt(Val(strSOPVerluste)))

		' ---------------------------------------------------------------------------------------------

		' Verlust A Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_21"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iVerAOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iVerAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "21")))
		'End If
		strResult(13) = _21 ' (iVerAOPMwSt)

		' Verlust I Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_25"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iVerIOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iVerIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "25")))
		'End If
		strResult(14) = _25 ' (iVerIOPMwSt)

		' Verlust F Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_29"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iVerFOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iVerFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "29")))
		'End If
		strResult(15) = _29 ' (iVerFOPMwSt)

		' Verlust A Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_22"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iVerAOP = CInt(Val(strBez))
		'	'Else
		'	'	iVerAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "22")))
		'End If
		strResult(16) = _22 ' (iVerAOP)

		' Verlust I Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_26"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iVerIOP = CInt(Val(strBez))
		'	'Else
		'	'	iVerIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "26")))
		'End If
		strResult(17) = _26 ' (iVerIOP)

		' Verlust F Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_30"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iVerFOP = CInt(Val(strBez))
		'	'Else
		'	'	iVerFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "30")))
		'End If
		strResult(18) = _30 ' (iVerFOP)


		' Vergütungen ----------------------------------------------------------------------------------------------
		' Vergütungen A Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_23"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iRuAOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iRuAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "23")))
		'End If
		strResult(19) = _23 ' iRuAOPMwSt '(iRuAOPMwSt)

		' Vergütungen I Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_27"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iRuIOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iRuIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "27")))
		'End If
		strResult(20) = _27 ' iRuIOPMwSt ' (iRuIOPMwSt)

		' Vergütungen F Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_31"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iRuFOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iRuFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "31")))
		'End If
		strResult(21) = _31 ' iRuFOPMwSt ' (iRuFOPMwSt)

		' Vergütungen A Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_24"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iRuAOP = CInt(Val(strBez))
		'	'Else
		'	'	iRuAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "24")))
		'End If
		strResult(22) = _24 ' iRuAOP ' (iRuAOP)

		' Vergütungen I Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_28"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iRuIOP = CInt(Val(strBez))
		'	'Else
		'	'	iRuIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "28")))
		'End If
		strResult(23) = _28 ' iRuIOP ' (iRuIOP)

		' Vergütungen F Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_32"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iRuFOP = CInt(Val(strBez))
		'	'Else
		'	'	iRuFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "32")))
		'End If
		strResult(24) = _32 ' iRuFOP ' (iRuFOP)


		' Gutschrift ----------------------------------------------------------------------------------------------
		' Gutschrift A Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_33"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iGuAOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iGuAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "33")))
		'End If
		strResult(25) = _33 ' iGuAOPMwSt ' (iGuAOPMwSt)

		' Gutschrift I Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_35"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iGuIOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iGuIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "35")))
		'End If
		strResult(26) = _35 ' iGuIOPMwSt ' (iGuIOPMwSt)

		' Gutschrift F Rechnungen MwSt.
		'strQuery = "//BuchungsKonten/_37"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iGuFOPMwSt = CInt(Val(strBez))
		'	'Else
		'	'	iGuFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "37")))
		'End If
		strResult(27) = _37 ' iGuFOPMwSt ' (iGuFOPMwSt)

		' Gutschrift A Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_34"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iGuAOP = CInt(Val(strBez))
		'	'Else
		'	'	iGuAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "34")))
		'End If
		strResult(28) = _34 ' iGuAOP ' (iGuAOP)

		' Gutschrift I Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_36"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iGuIOP = CInt(Val(strBez))
		'	'Else
		'	'	iGuIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "36")))
		'End If
		strResult(29) = _36 ' (iGuIOP)

		' Gutschrift F Rechnungen MwSt.-frei
		'strQuery = "//BuchungsKonten/_38"
		'strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
		'If strBez <> String.Empty Then
		'	iGuFOP = CInt(Val(strBez))
		'	'Else
		'	'	iGuFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "38")))
		'End If
		strResult(30) = _38 ' iGuFOP ' (iGuFOP)

		'm_Logger.LogDebug(strResult)

		Return strResult
	End Function

	Function FillMyLOiBetrag(ByVal iCount As Integer) As List(Of Integer)
		Dim loiBetrag As New List(Of Integer)

		If iCount = 0 Then iCount = 30
		For i As Integer = 0 To iCount - 1
			loiBetrag.Add(0)
			loiBetrag(i) = 0
		Next

		Return loiBetrag
	End Function

#End Region

#Region "Funktionen zur Suche nach Daten..."

	Function GetSQLStringFromRPDb(ByVal strSelectedKst3Bez As String,
																ByVal iMyFMonth As Integer,
																ByVal iMyLMonth As Integer,
																ByVal strMyVYear As Integer,
																ByVal strMyBYear As Integer) As String
		Dim sSql As String = String.Empty

		sSql = "Select RP.RPNr, RP.ESNr, RP.MANr, RP.Jahr, RP.Monat, RP.LONr, RP.RPKst, RP.RPKst1, RP.RPKst2, "
		sSql += "RPL.K_Anzahl, RPL.M_Anzahl, RPL.M_Betrag, RPL.LANr, RPL.KDNr, RPL.K_betrag, RPL.RENr, RPL.KD, "
		sSql += "KD.Firma1, KD.Strasse As KDStrasse, KD.PLZ As KDPLZ, KD.Ort As KDOrt, KD.Land As KDLand, "
		sSql += "MA.Nachname As MANachname, MA.Vorname As MAVorname, "

		sSql += "0 As MargenInProz "

		sSql += "From dbo.RP "
		sSql += "Left Join dbo.RPL On RP.RPNr = RPL.RPNr "
		sSql += "Left Join dbo.Kunden KD On RP.KDNr = KD.KDNr "
		sSql += "Left Join dbo.Mitarbeiter MA On RP.MANr = MA.MANr "
		sSql += "Left Join dbo.ESLohn On RP.ESNr = ESLohn.ESNr "

		Dim strQueryString As String = GetQueryParam4Db("", strSelectedKst3Bez)
		strQueryString = String.Format("RP.MDNr = {0} {1} {2}", m_InitialData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(strQueryString), "And", ""), strQueryString)
		sSql += CStr(IIf(strQueryString <> String.Empty, "Where ", String.Empty)) & strQueryString


		Return sSql
	End Function

	Function GetQueryParam4OPDb(ByVal strSelectedKst3Bez As String, ByVal str4What As String) As String
		Dim Sql As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		'Dim strUSFiliale As String = ""	' muss ändern... _ClsProgSetting.GetUSFiliale()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		If str4What.ToUpper = "TOP" Then
			Sql = "RPL.K_betrag <> 0 And RPL.KD = 1 "
			Sql = GetQueryParam4Db(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "IOP" Then
			Sql = "RE.Art In ('I') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "FESTOP" Then
			Sql = "RE.Art In ('F') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "GOP" Then
			Sql = "RE.Art In ('G') And RE.Art_2 In ('A') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "GOPF" Then
			Sql = "RE.Art In ('G') And RE.Art_2 In ('F') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "GOPI" Then
			Sql = "RE.Art In ('G') And RE.Art_2 In ('I') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "ROPA" Then
			Sql = "RE.Art In ('R') And RE.Art_2 In ('A') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "ROPF" Then
			Sql = "RE.Art In ('R') And RE.Art_2 In ('F') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)

		ElseIf str4What.ToUpper = "ROPI" Then
			Sql = "RE.Art In ('R') And RE.Art_2 In ('I') "
			Sql = GetQueryParam4IOPDb(Sql, strSelectedKst3Bez)


		Else
			Sql &= GetQueryParam4FOPDb(Sql, strSelectedKst3Bez)

		End If


		Return Sql
	End Function

	Function GetQueryParam4Db(ByVal sSQLQuery As String, ByVal strSelectedKst3Bez As String) As String
		Dim Sql As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty


		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		Dim jahrVon = m_SearchCriteria.FirstYear
		Dim jahrBis = m_SearchCriteria.LastYear

		Sql &= strAndString & String.Format("((RP.Jahr = {0} And RP.Monat >= {1} And ", jahrVon, m_SearchCriteria.FirstMonth)
		Sql &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
		Sql &= String.Format("(RP.Monat >= {0} And RP.Monat <= {1}))) Or ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
		Sql &= String.Format("(RP.Jahr > {0} And RP.Jahr < {1}) Or ", jahrVon, jahrBis)
		Sql &= String.Format("(RP.Jahr = {0} And RP.Monat <= {1} And ", jahrBis, m_SearchCriteria.LastMonth)
		Sql &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
		Sql &= String.Format("(RP.Monat >= {0} And RP.Monat <= {1})))) ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)

		' FilterBez zuordnen
		FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}",
															 m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear,
															 m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear,
															 vbLf)

		' 3. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		strFieldName = "RP.RPKst"
		If UCase(strSelectedKst3Bez) <> String.Empty Then
			sZusatzBez = strSelectedKst3Bez
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Berater wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				Sql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				Sql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Branche -----------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		strFieldName = "RP.KDBranche"
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Einsatzbranche wie ({0}){1}"), sZusatzBez, vbLf)

			If InStr(ClsDataDetail.GetFormVars(7).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				Sql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				Sql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		strFieldName = "KD.PLZ"
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Kanton wie ({0}){1}"), sZusatzBez, vbLf)

			If InStr(sZusatzBez.Trim, ",") > 0 Then
				sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			End If
			Sql += strAndString & strFieldName & " In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

		End If

		' Ort -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		strFieldName = "KD.Ort"
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Ort wie ({0}){1}"), sZusatzBez, vbLf)

			If InStr(ClsDataDetail.GetFormVars(9).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				Sql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				Sql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Nationalität --------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		strFieldName = "MA.Nationality"
		If UCase(ClsDataDetail.GetFormVars(10).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(10).ToString.Trim
			sZusatzBez = Replace(GetLandCode(sZusatzBez), ", ", ",")
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Nationalität wie ({0}){1}"), sZusatzBez, vbLf)

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			Sql += strAndString & strFieldName & " "
			If sZusatzBez.Contains("*") OrElse sZusatzBez.Contains("%") Then
				Sql += "Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				Sql += "In ('" & sZusatzBez & "')"
			End If
		End If

		' Land -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
		strFieldName = "MA.Land"
		If UCase(ClsDataDetail.GetFormVars(11).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(11).ToString.Trim
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Land wie ({0}){1}"), sZusatzBez, vbLf)

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				Sql += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				Sql += strAndString & strFieldName & " In ('" & GetLandCode(sZusatzBez) & "')"
			End If
		End If


		' MANr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.employeeNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
			strFieldName = "RPL.MANr"
			sZusatzBez = m_SearchCriteria.employeeNumber
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Kandidatennummer wie ({0}){1}"), sZusatzBez, vbLf)

			Sql += String.Format("{0}{1} In ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
			strFieldName = "RPL.KDNr"
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Kundennummer wie ({0}){1}"), sZusatzBez, vbLf)

			Sql += String.Format("{0}{1} In ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(Sql), String.Empty, " And ")
			strFieldName = "RPL.ESNr"
			sZusatzBez = m_SearchCriteria.esNumber
			FilterBez += String.Format(_ClsProgSetting.TranslateText("Einsatznummer wie ({0}){1}"), sZusatzBez, vbLf)

			Sql += String.Format("{0}{1} In ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If


		ClsDataDetail.GetFilterBez = FilterBez


		Return Sql
	End Function

	Function GetSortString() As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strMyName As String = String.Empty

		If ClsDataDetail.GetFormVars(0).ToString.ToUpper.Contains("Filiale".ToUpper) Then
			strSortBez = "Filiale und Disponenten"
			strMyName = "UmJ.USFiliale ASC, UmJ.Kst3Bez ASC"

		Else
			strSortBez = "Disponenten"
			strMyName = "UmJ.Kst3Bez ASC"

		End If
		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function


#End Region


	Public Function ListESData4DB1(ByVal vonmonat As Integer, ByVal bismonat As Integer, ByVal jahr As Integer, ByVal lastyear As Integer) As IEnumerable(Of DB1ESData)
		Dim result As List(Of DB1ESData) = Nothing

		Dim sql As String = String.Empty

		sql &= "[Get ESData For Db1 Listing]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("mdnr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("FirstYear", jahr))
		listOfParams.Add(New SqlClient.SqlParameter("LastMonth", bismonat))
		listOfParams.Add(New SqlClient.SqlParameter("FirstMonth", vonmonat))
		listOfParams.Add(New SqlClient.SqlParameter("LastYear", lastyear))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then
				result = New List(Of DB1ESData)

				While reader.Read
					Dim esData As New DB1ESData

					esData.ESNumber = SafeGetInteger(reader, "ESNr", 0)
					esData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					esData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)

					esData.customername = SafeGetString(reader, "Firma1")
					esData.employeefullname = SafeGetString(reader, "MAName")

					esData.es_ab = SafeGetDateTime(reader, "es_ab", Nothing)
					esData.es_ende = SafeGetDateTime(reader, "es_Ende", Nothing)

					result.Add(esData)

				End While

			End If


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return result

	End Function

	Public Function ListEmployeeData4DB1(ByVal vonmonat As Integer, ByVal bismonat As Integer, ByVal jahr As Integer, ByVal lastyear As Integer) As IEnumerable(Of DB1EmployeeData)
		Dim result As List(Of DB1EmployeeData) = Nothing

		Dim sql As String = String.Empty

		sql &= "[Get EmployeeData For Db1 Listing]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("mdnr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("FirstYear", jahr))
		listOfParams.Add(New SqlClient.SqlParameter("LastMonth", bismonat))
		listOfParams.Add(New SqlClient.SqlParameter("FirstMonth", vonmonat))
		listOfParams.Add(New SqlClient.SqlParameter("LastYear", lastyear))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then
				result = New List(Of DB1EmployeeData)

				While reader.Read
					Dim esData As New DB1EmployeeData

					esData.ESNumber = 0 ' SafeGetInteger(reader, "ESNr", 0)
					esData.CustomerNumber = 0 ' SafeGetInteger(reader, "KDNr", 0)
					esData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)

					esData.customername = String.Empty 'SafeGetString(reader, "Firma1")
					esData.employeefullname = SafeGetString(reader, "MAName")

					esData.es_ab = Nothing ' SafeGetDateTime(reader, "es_ab", Nothing)
					esData.es_ende = Nothing ' SafeGetDateTime(reader, "es_Ende", Nothing)

					result.Add(esData)

				End While

			End If


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return result

	End Function


	Public Function ListCustomerData4DB1(ByVal vonmonat As Integer, ByVal bismonat As Integer, ByVal jahr As Integer, ByVal lastyear As Integer) As IEnumerable(Of DB1CustomerData)
		Dim result As List(Of DB1CustomerData) = Nothing

		Dim sql As String = String.Empty

		sql &= "[Get CustomerData For Db1 Listing]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("mdnr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("FirstMonth", vonmonat))
		listOfParams.Add(New SqlClient.SqlParameter("LastMonth", bismonat))
		listOfParams.Add(New SqlClient.SqlParameter("FirstYear", jahr))
		listOfParams.Add(New SqlClient.SqlParameter("LastYear", lastyear))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then
				result = New List(Of DB1CustomerData)

				While reader.Read
					Dim esData As New DB1CustomerData

					esData.ESNumber = 0 ' SafeGetInteger(reader, "ESNr", 0)
					esData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					esData.EmployeeNumber = 0 'SafeGetInteger(reader, "MANr", 0)

					esData.customername = SafeGetString(reader, "Firma1")
					esData.employeefullname = String.Empty  ' SafeGetString(reader, "MAName")

					esData.es_ab = Nothing ' SafeGetDateTime(reader, "es_ab", Nothing)
					esData.es_ende = Nothing ' SafeGetDateTime(reader, "es_Ende", Nothing)

					result.Add(esData)

				End While

			End If


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return result

	End Function

	Public Function LoadCustomerData4CustomerJournalSearch(ByVal vonmonat As Integer, ByVal bismonat As Integer, ByVal jahr As Integer) As IEnumerable(Of DB1CustomerData)
		Dim result As List(Of DB1CustomerData) = Nothing

		Dim sql As String = String.Empty

		sql &= "[Get CustomerData For Db1 CustomerJournal Listing]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("mdnr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("FirstYear", jahr))
		listOfParams.Add(New SqlClient.SqlParameter("LastMonth", bismonat))
		listOfParams.Add(New SqlClient.SqlParameter("FirstMonth", vonmonat))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then
				result = New List(Of DB1CustomerData)

				While reader.Read
					Dim esData As New DB1CustomerData

					esData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)


					result.Add(esData)

				End While

			End If


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return result

	End Function




#Region "Helpers"

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
		Dim result As Integer
		If (Not Integer.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region


End Class
