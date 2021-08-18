
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SPMASearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

#Region "Diverses"

	Public Property mandantNumber As Integer()

	'// Get4What._strModul4What
	Dim _strModul4What As String
	Public Property Get4What() As String
		Get
			Return _strModul4What
		End Get
		Set(ByVal value As String)
			_strModul4What = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strQuery As String
	Public Property GetSearchQuery() As String
		Get
			Return _strQuery
		End Get
		Set(ByVal value As String)
			_strQuery = value
		End Set
	End Property

	'// LargerLV
	Dim _bLargerLV As Boolean
	Public Property GetLargerLV() As Boolean
		Get
			Return _bLargerLV
		End Get
		Set(ByVal value As Boolean)
			_bLargerLV = value
		End Set
	End Property


#End Region

#Region "Funktionen für LvClick in der Suchmaske..."

	'// Allgemeiner Zwischenspeicher
	Dim _strSelektion As String
	Public Property GetSelektion() As String
		Get
			Return _strSelektion
		End Get
		Set(ByVal value As String)
			_strSelektion = value
		End Set
	End Property

	' // ID
	Dim _strID As String
	Public Property GetID() As String
		Get
			Return _strID
		End Get
		Set(ByVal value As String)
			_strID = value
		End Set
	End Property

	'// MANr
	Dim _strMANr As String
	Public Property GetMANr() As String
		Get
			Return _strMANr
		End Get
		Set(ByVal value As String)
			_strMANr = value
		End Set
	End Property

	'// Kandidatenname
	Dim _strMAName As String
	Public Property GetMAName() As String
		Get
			Return _strMAName
		End Get
		Set(ByVal value As String)
			_strMAName = value
		End Set
	End Property

	'// Kandidatenvorname
	Dim _strMAVorname As String
	Public Property GetMAVorname() As String
		Get
			Return _strMAVorname
		End Get
		Set(ByVal value As String)
			_strMAVorname = value
		End Set
	End Property

	'// Kandidatenberuf
	Dim _strMABeruf As String
	Public Property GetMABeruf() As String
		Get
			Return _strMABeruf
		End Get
		Set(ByVal value As String)
			_strMABeruf = value
		End Set
	End Property

	'// QSTGemeinde
	Dim _strQSTGemeinde As String
	Public Property GetQSTGemeinde() As String
		Get
			Return _strQSTGemeinde
		End Get
		Set(ByVal value As String)
			_strQSTGemeinde = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strTelNr As String
	Public Property GetTelNr() As String
		Get
			Return _strTelNr
		End Get
		Set(ByVal value As String)
			_strTelNr = value
		End Set
	End Property

#End Region

#Region "LL_Properties"
	'// Print.LLDocName
	Dim _LLDocName As String
	Public Property LLDocName() As String
		Get
			Return _LLDocName
		End Get
		Set(ByVal value As String)
			_LLDocName = value
		End Set
	End Property

	'// Print.LLDocLabel
	Dim _LLDocLabel As String
	Public Property LLDocLabel() As String
		Get
			Return _LLDocLabel
		End Get
		Set(ByVal value As String)
			_LLDocLabel = value
		End Set
	End Property

	'// Print.LLFontDesent
	Dim _LLFontDesent As Integer
	Public Property LLFontDesent() As Integer
		Get
			Return _LLFontDesent
		End Get
		Set(ByVal value As Integer)
			_LLFontDesent = value
		End Set
	End Property

	'// Print.LLIncPrv
	Dim _LLIncPrv As Integer
	Public Property LLIncPrv() As Integer
		Get
			Return _LLIncPrv
		End Get
		Set(ByVal value As Integer)
			_LLIncPrv = value
		End Set
	End Property

	'// Print.LLParamCheck
	Dim _LLParamCheck As Integer
	Public Property LLParamCheck() As Integer
		Get
			Return _LLParamCheck
		End Get
		Set(ByVal value As Integer)
			_LLParamCheck = value
		End Set
	End Property

	'// Print.LLKonvertName
	Dim _LLKonvertName As Integer
	Public Property LLKonvertName() As Integer
		Get
			Return _LLKonvertName
		End Get
		Set(ByVal value As Integer)
			_LLKonvertName = value
		End Set
	End Property

	'// Print.LLZoomProz
	Dim _LLZoomProz As Integer
	Public Property LLZoomProz() As Integer
		Get
			Return _LLZoomProz
		End Get
		Set(ByVal value As Integer)
			_LLZoomProz = value
		End Set
	End Property

	'// Print.LLCopyCount
	Dim _LLCopyCount As Integer
	Public Property LLCopyCount() As Integer
		Get
			Return _LLCopyCount
		End Get
		Set(ByVal value As Integer)
			_LLCopyCount = value
		End Set
	End Property

	'// Print.LLExportedFilePath
	Dim _LLExportedFilePath As String
	Public Property LLExportedFilePath() As String
		Get
			Return _LLExportedFilePath
		End Get
		Set(ByVal value As String)
			_LLExportedFilePath = value
		End Set
	End Property

	'// Print.LLExportedFileName
	Dim _LLExportedFileName As String
	Public Property LLExportedFileName() As String
		Get
			Return _LLExportedFileName
		End Get
		Set(ByVal value As String)
			_LLExportedFileName = value
		End Set
	End Property

	'// Print.LLExportfilter
	Dim _LLExportfilter As String
	Public Property LLExportfilter() As String
		Get
			Return _LLExportfilter
		End Get
		Set(ByVal value As String)
			_LLExportfilter = value
		End Set
	End Property

	'// Print.LLExporterName
	Dim _LLExporterName As String
	Public Property LLExporterName() As String
		Get
			Return _LLExporterName
		End Get
		Set(ByVal value As String)
			_LLExporterName = value
		End Set
	End Property

	'// Print.LLExporterFileName
	Dim _LLExporterFileName As String
	Public Property LLExporterFileName() As String
		Get
			Return _LLExporterFileName
		End Get
		Set(ByVal value As String)
			_LLExporterFileName = value
		End Set
	End Property

#End Region

#Region "US Setting"

	'// USeMail (= eMail des Personalvermittlers)
	Dim _USeMail As String
	Public Property USeMail() As String
		Get
			Return _USeMail
		End Get
		Set(ByVal value As String)
			_USeMail = value
		End Set
	End Property

	'// USTelefon (= USTelefon des Personalvermittlers)
	Dim _USTelefon As String
	Public Property USTelefon() As String
		Get
			Return _USTelefon
		End Get
		Set(ByVal value As String)
			_USTelefon = value
		End Set
	End Property

	'// USTelefax (= USTelefax des Personalvermittlers)
	Dim _USTelefax As String
	Public Property USTelefax() As String
		Get
			Return _USTelefax
		End Get
		Set(ByVal value As String)
			_USTelefax = value
		End Set
	End Property

	'// USVorname (= USVorname des Personalvermittlers)
	Dim _USVorname As String
	Public Property USVorname() As String
		Get
			Return _USVorname
		End Get
		Set(ByVal value As String)
			_USVorname = value
		End Set
	End Property

	'// USAnrede (= USAnrede des Personalvermittlers)
	Dim _USAnrede As String
	Public Property USAnrede() As String
		Get
			Return _USAnrede
		End Get
		Set(ByVal value As String)
			_USAnrede = value
		End Set
	End Property

	'// USNachname (= USNachname des Personalvermittlers)
	Dim _USNachname As String
	Public Property USNachname() As String
		Get
			Return _USNachname
		End Get
		Set(ByVal value As String)
			_USNachname = value
		End Set
	End Property

	'// USMDName (= MDName des Personalvermittlers)
	Dim _USMDname As String
	Public Property USMDname() As String
		Get
			Return _USMDname
		End Get
		Set(ByVal value As String)
			_USMDname = value
		End Set
	End Property

	'// MDName2 (= MDName2 des Personalvermittlers)
	Dim _USMDname2 As String
	Public Property USMDname2() As String
		Get
			Return _USMDname2
		End Get
		Set(ByVal value As String)
			_USMDname2 = value
		End Set
	End Property

	'// MDName3 (= MDName3 des Personalvermittlers)
	Dim _USMDname3 As String
	Public Property USMDname3() As String
		Get
			Return _USMDname3
		End Get
		Set(ByVal value As String)
			_USMDname3 = value
		End Set
	End Property

	'// USMDPostfach (= MDPostfach des Personalvermittlers)
	Dim _USMDPostfach As String
	Public Property USMDPostfach() As String
		Get
			Return _USMDPostfach
		End Get
		Set(ByVal value As String)
			_USMDPostfach = value
		End Set
	End Property

	'// USMDStrasse (= MDstrasse des Personalvermittlers)
	Dim _USMDStrasse As String
	Public Property USMDStrasse() As String
		Get
			Return _USMDStrasse
		End Get
		Set(ByVal value As String)
			_USMDStrasse = value
		End Set
	End Property

	'// USMDOrt (= MDOrt des Personalvermittlers)
	Dim _USMDOrt As String
	Public Property USMDOrt() As String
		Get
			Return _USMDOrt
		End Get
		Set(ByVal value As String)
			_USMDOrt = value
		End Set
	End Property

	'// USMDPLZ (= MDPLZ des Personalvermittlers)
	Dim _USMDPlz As String
	Public Property USMDPlz() As String
		Get
			Return _USMDPlz
		End Get
		Set(ByVal value As String)
			_USMDPlz = value
		End Set
	End Property

	'// USMDLand (= MDLand des Personalvermittlers)
	Dim _USMDLand As String
	Public Property USMDLand() As String
		Get
			Return _USMDLand
		End Get
		Set(ByVal value As String)
			_USMDLand = value
		End Set
	End Property

	'// USMDTelefon (= MDTelefon des Personalvermittlers)
	Dim _USMDTelefon As String
	Public Property USMDTelefon() As String
		Get
			Return _USMDTelefon
		End Get
		Set(ByVal value As String)
			_USMDTelefon = value
		End Set
	End Property

	'// USMDTelefax (= MDTelefax des Personalvermittlers)
	Dim _USMDTelefax As String
	Public Property USMDTelefax() As String
		Get
			Return _USMDTelefax
		End Get
		Set(ByVal value As String)
			_USMDTelefax = value
		End Set
	End Property

	'// USMDeMail (= MDeMail des Personalvermittlers)
	Dim _USMDeMail As String
	Public Property USMDeMail() As String
		Get
			Return _USMDeMail
		End Get
		Set(ByVal value As String)
			_USMDeMail = value
		End Set
	End Property

	'// USMDHomepage (= MDHomepage des Personalvermittlers)
	Dim _USMDHomepage As String
	Public Property USMDHomepage() As String
		Get
			Return _USMDHomepage
		End Get
		Set(ByVal value As String)
			_USMDHomepage = value
		End Set
	End Property

#End Region

End Class

Public Class ClsDbFunc


#Region "private constants"

	Private Const sqlGebDat As String = "(DatePart(Day, MA.GebDat) BETWEEN {0} AND {1} And DatePart(Month, MA.GebDat) = {2}) "
	Private Const sqlGebDat_2 As String = "(DatePart(Day, MA.GebDat) BETWEEN {0} AND {1} And DatePart(Month, MA.GebDat) = {2} " &
		"OR DatePart(Day, MA.GebDat) BETWEEN 1 AND {3} And DatePart(Month, MA.GebDat) = {4}) "

#End Region

#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
	Private m_md As Mandant
	Private m_utility As Utilities

	Private m_utilityUI As UtilityUI
	Private m_DateTimeUi As DateAndTimeUtily

#End Region

	Public Property mandantNumber As New List(Of Integer)


#Region "Constructor"

	Sub New()

		m_utility = New Utilities
		m_md = New Mandant

		m_utilityUI = New UtilityUI
		m_DateTimeUi = New DateAndTimeUtily


	End Sub


#End Region

	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim result As List(Of MandantenData) = Nothing

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_utility.SafeGetString(reader, "MDName")
					recData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_md.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function



#Region "Funktionen für Template_1..."

	'// _HBeruf
	Dim _strHBeruf As String
	Public Property GetHBeruf() As String
		Get
			Return _strHBeruf
		End Get
		Set(ByVal value As String)
			_strHBeruf = value
		End Set
	End Property

	'// _sBeruf
	Dim _strSBeruf As String
	Public Property GetSBeruf() As String
		Get
			Return _strSBeruf
		End Get
		Set(ByVal value As String)
			_strSBeruf = value
		End Set
	End Property

	'// _Branche
	Dim _strBranche As String
	Public Property GetBranche() As String
		Get
			Return _strBranche
		End Get
		Set(ByVal value As String)
			_strBranche = value
		End Set
	End Property

	'// _Geschlecht
	Dim _strGeschlecht As String
	Public Property GetGeschlecht() As String
		Get
			Return _strGeschlecht
		End Get
		Set(ByVal value As String)
			_strGeschlecht = value
		End Set
	End Property

	'// im Einsatz
	Dim _bInES As Boolean
	Public Property GetInES() As Boolean
		Get
			Return _bInES
		End Get
		Set(ByVal value As Boolean)
			_bInES = value
		End Set
	End Property

	'// Kontakt
	Dim _strKontakt As String
	Public Property GetMAKontakt() As String
		Get
			Return _strKontakt
		End Get
		Set(ByVal value As String)
			_strKontakt = value
		End Set
	End Property

	'// Status
	Dim _strState As String
	Public Property GetMAState() As String
		Get
			Return _strState
		End Get
		Set(ByVal value As String)
			_strState = value
		End Set
	End Property

	Sub New(ByVal iVakNr As Integer, ByVal bHBeruf As Boolean, ByVal bSBeruf As Boolean,
					ByVal bBranche As Boolean, ByVal bGeschlecht As Boolean,
					ByVal bInES As Boolean,
					ByVal strMAKontakt As String, ByVal strMAState As String)

		GetHBeruf = If(bHBeruf, GetVakDb(iVakNr, "Vak_HBerufe", "Bezeichnung"), "")
		GetSBeruf = If(bSBeruf, GetVakDb(iVakNr, "Vak_sBerufe", "Bezeichnung"), "")
		GetBranche = If(bBranche, GetVakDb(iVakNr, "Vak_Branchen", "Bezeichnung"), "")
		GetGeschlecht = If(bGeschlecht, GetVakDb(iVakNr, "Vakanzen", "MASex"), "")
		GetInES = bInES

		GetMAKontakt = strMAKontakt
		GetMAState = strMAState

	End Sub


	Function GetStartQuery_Template_1() As String
		Dim sSql As String = String.Empty
		Dim sZusatzBez As String = String.Empty

		' Standard SQL-Query der Tabelle Mitarbeiter beim Fehlen der XML-Datei
		Dim aktivVom As String = Date.Parse(Now.ToString).ToString("d")
		Dim aktivBis As String = Date.Parse(Now.ToString).ToString("d")
		sSql = "Begin Try Drop Table #ESActiv End Try Begin Catch End Catch "

		sSql &= "Select ES.MANr Into #ESActiv From ES Where "
		sSql &= "((ES.ES_Ende >= '{0}' Or ES.ES_Ende Is Null) And "
		sSql &= "ES.ES_Ab <= '{1}') "
		sSql = String.Format(sSql, aktivVom, aktivBis)

		sSql &= "Begin Try Drop Table _MAVakTemplate_{0} End Try Begin Catch End Catch "
		sSql += "Select DISTINCT(MA.MANr), MA.ID as MAID, MA.KST, "
		sSql += "MA.Nachname as MANachname, MA.Vorname as MAVorname, "
		sSql += "MA.PLZ As MAPLZ, MA.Ort As MAOrt, "
		sSql += "MA.Land As MALand, MA.Telefon3 as MATelefon3, "
		sSql += "MA.Wohnt_Bei As MACo, MA.Postfach As MAPostfach, MA.Strasse As MAStrasse, MA.Telefon_G as MATelefonG, "
		sSql += "MA.Telefon_P as MATelefonP, MA.Natel As MANatel, IsNull(MA.MA_SMS_Mailing, 0) As MA_SMS_Mailing, IsNull(MA.MA_EMail_Mailing, 0) As MA_EMAil_Mailing, "
		sSql += "MA.GebDat as MAGebDat, MA.AHV_Nr as MAAHV_Nr, "
		sSql += "MA.Geschlecht as MAGeschlecht, MA.Beruf As MABeruf, MA.BerufCode As MABerufCode, "
		sSql += "MA_LOSetting.Currency as MACurrency, MA.Bewillig as MABewillig, "
		sSql += "MA.Bew_Bis as MABew_Bis, "
		sSql += "MA.Q_Steuer as MAQ_Steuer, "
		sSql += "(SELECT Top 1 IsNull(Notice_Common, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS MAV_Hinweis, "
		sSql += "(SELECT Top 1 IsNull(Notice_Employment, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_Employment, "
		sSql += "(SELECT Top 1 IsNull(Notice_Report, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_Report, "
		sSql += "(SELECT Top 1 IsNull(Notice_AdvancedPayment, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_AdvancedPayment, "
		sSql += "(SELECT Top 1 IsNull(Notice_Payroll, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_Payroll, "

		sSql += "MA.AHV_Nr_New as MAAHV_Nr_New, "
		sSql += "MA.Nationality as MANationality, MA.Zivilstand as MAZivilstand, "
		sSql += "MA.S_Kanton as MAS_Kanton, "
		sSql += "MA.ForeignCategory, MA.EmploymentType, MA.OtherEmploymentType, MA.TypeofStay, MA.CHPartner, MA.NoSpecialTax, "

		sSql += "MAKK.KStat1 as MAKStat1, MAKK.KStat2 as MAKStat2, "
		sSql += "MAKK.KontaktHow as MAKontaktHow, "
		sSql += "MA.eMail As MAeMail, MA_LOSetting.Zahlart as MAZahlungsart, "
		sSql += "MA.Send2WOS as MASend2WOS, "

		sSql += "MA_LL.Dateiname, "
		sSql &= "CONVERT(NVARCHAR(4000), MA_LL.Reserve1) AS Reserve1, " +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve2) AS Reserve2, CONVERT(NVARCHAR(4000), MA_LL.Reserve3) AS Reserve3, " +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve4) AS Reserve4, CONVERT(NVARCHAR(4000), MA_LL.Reserve5) AS Reserve5, " +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve6) AS Reserve6, CONVERT(NVARCHAR(4000), MA_LL.Reserve7) AS Reserve7, " +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve8) AS Reserve8, CONVERT(NVARCHAR(4000), MA_LL.Reserve9) AS Reserve9, " +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve10) AS Reserve10, CONVERT(NVARCHAR(4000), MA_LL.Reserve11) AS Reserve11," +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve12) AS Reserve12, CONVERT(NVARCHAR(4000), MA_LL.Reserve13) AS Reserve13, " +
							"CONVERT(NVARCHAR(4000), MA_LL.Reserve14) AS Reserve14, CONVERT(NVARCHAR(4000), MA_LL.Reserve15) AS Reserve15, "

		sSql += "MA_LL.Referenz1, MA_LL.Referenz2, MA_LL.Referenz3, "
		sSql += "MA_LL.Abteil1, MA_LL.Abteil2, MA_LL.Abteil3, "
		sSql += "MA_LL.Ort1, MA_LL.Ort2, MA_LL.Ort3, "
		sSql += "MA_LL.Ref1Firma, MA_LL.Ref2Firma, MA_LL.Ref3Firma, "
		sSql += "MA_LL.Ref1Telefon, MA_LL.Ref2Telefon, MA_LL.Ref3Telefon, "
		sSql += "MA_LL.Ref1Text, MA_LL.Ref2Text, MA_LL.Ref3Text, "

		sSql += "Benutzer.Vorname AS US_Vorname, Benutzer.Nachname AS US_Nachname, "
		sSql += "Benutzer.Anrede AS US_Anrede, Benutzer.Postfach AS US_Postfach, "
		sSql += "Benutzer.Strasse AS US_Strasse, Benutzer.Ort AS US_Ort, "
		sSql += "Benutzer.PLZ AS US_PLZ, Benutzer.Land AS US_Land, "
		sSql += "Benutzer.Telefon AS US_Telefon, Benutzer.Telefax AS US_Telefax, "
		sSql += "Benutzer.Natel AS US_Natel, Benutzer.eMail AS US_eMail, "
		sSql += "Benutzer.Homepage AS US_Homepage, Benutzer.Abteilung AS US_Abteilung, "
		sSql += "Benutzer.USFiliale AS US_USFiliale, IsNull((Select Top 1 (US.Nachname + ', ' + US.Vorname) As MABerater From Benutzer US Where US.Kst = MA.KST), '') As MABerater, "

		' Die Farbe der Hauptmaske für die Kandidaten übernehmen
		' Die SQL-String ist auf der Datenbank
		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmdFarbe As SqlCommand = New SqlCommand("SELECT CaseString FROM TBL_COLORS WHERE Modulname='MA0'", conn)
		conn.Open()
		sSql += cmdFarbe.ExecuteScalar().ToString
		conn.Close()

		sSql += "Into _MAVakTemplate_{0} "
		sSql += "From Mitarbeiter MA "
		sSql += "Left Join MASonstiges On MA.MANr = MASonstiges.MANr "
		sSql += "Left Join MAKontakt_Komm MAKK On MA.MANr = MAKK.MANr "
		sSql += "Left Join MA_LOSetting On MA.MANr = MA_LOSetting.MANr "
		'sSql += "Left Join MA_LOAusweis On MA.MANr = MA_LOAusweis.MANr "
		sSql += "Left Join MA_LL On MA.MANr = MA_LL.MANr "

		' MA_ES_ALs
		If GetSBeruf <> String.Empty Then
			sSql += " Left Join MA_ES_ALs On MA_ES_ALs.MANr = MA.MANr"
		End If
		' MA_BRANCHE
		If GetBranche <> String.Empty Then
			sSql += " Left Join MA_Branche On MA_Branche.MANr = MA.MANr"
		End If

		sSql += ", Benutzer "

		Return sSql
	End Function

	Function GetQuerySQLString_Template_1(ByVal sSQLQuery As String) As String
		Dim sSql As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = ClsDataDetail.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)


		' Berufe -------------------------------------------------------------------------------------------------
		If GetHBeruf <> String.Empty Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sZusatzBez = GetHBeruf.Replace("'", "''")
			sSql = strAndString & "MA.Beruf In ('"
			sSql += Replace(sZusatzBez, "#@", "','") & "')"

			FilterBez += "Qualifikation in (" & Replace(sZusatzBez, "#@", ", ") & ")" & vbLf
		End If

		' Sonstitge Qualifikation -----------------------------------------------------------------------------------------
		If GetSBeruf <> String.Empty Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sZusatzBez = GetSBeruf.Replace("'", "''").Trim
			sSql += strAndString & "MA_ES_ALs.BerufsText In ('"
			sSql += Replace(sZusatzBez, "#@", "','") & "')"
			FilterBez += "Sonstige Qualifikation in (" & Replace(sZusatzBez, "#@", ", ") & ")" & vbLf
		End If

		' Branchen --------------------------------------------------------------------------------------------------------
		If GetBranche <> String.Empty Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sZusatzBez = GetBranche.Replace("'", "''").Trim
			sSql += strAndString & "MA_Branche.Bezeichnung In ('"
			sSql += Replace(sZusatzBez, "#@", "','") & "')"
			FilterBez += "Kandidat in den Branchen (" & Replace(sZusatzBez, "#@", ", ") & ")" & vbLf
		End If

		' Geschlecht ---------------------------------------------------------------------------------------------
		If GetGeschlecht <> String.Empty Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sZusatzBez = GetGeschlecht.Replace("'", "''")
			FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geschlecht = {0}{1}"), sZusatzBez, vbLf)
			sZusatzBez = sZusatzBez.Replace(" ", "").Replace(",", "','")
			sSql += String.Format("{0}MA.Geschlecht in ('{1}')", strAndString, sZusatzBez)
		End If

		' MA-Kontakt ---------------------------------------------------------------------------------------------
		If GetMAKontakt <> String.Empty Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sZusatzBez = GetMAKontakt.Replace("'", "''")
			FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Kontakt = {0}{1}"), sZusatzBez, vbLf)

			Dim aBez As String() = sZusatzBez.Split(CChar("#"))
			sZusatzBez = String.Empty
			For i As Short = 0 To CShort(CShort(aBez.Length) - 1)
				sZusatzBez &= If(sZusatzBez = String.Empty, "", "', '") & aBez(i).ToUpper.Trim
			Next

			'      sZusatzBez = sZusatzBez.Replace(",", "','")
			sSql += String.Format("{0}MAKK.KontaktHow in ('{1}')", strAndString, sZusatzBez)
		End If

		' MA-Status ---------------------------------------------------------------------------------------------
		If GetMAState <> String.Empty Then
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Status = {0}{1}"), GetMAState, vbLf)
			sZusatzBez = GetMAState.Replace("'", "''")
			Dim aBez As String() = sZusatzBez.Split(CChar("#"))
			sZusatzBez = String.Empty
			For i As Short = 0 To CShort(CShort(aBez.Length) - 1)
				sZusatzBez &= If(sZusatzBez = String.Empty, "", "', '") & aBez(i).ToUpper.Trim
			Next

			'      sZusatzBez = sZusatzBez.Replace(",", "','")
			sSql += String.Format("{0}MAKK.KStat1 in ('{1}')", strAndString, sZusatzBez)
		End If

		' Im ES ---------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		If GetInES Then
			FilterBez += String.Format(m_xml.GetSafeTranslationValue("Nicht heute im Einsatz{0}"), vbLf)
			' Aktiv zwischen -----------------------------------------------------------------------------------------
			sSql &= String.Format("{0} MA.MANr Not In (Select MANr From #ESActiv)", strAndString)

		Else
			FilterBez += String.Format(m_xml.GetSafeTranslationValue("Heute im Einsatz{0}"), vbLf)
			' Aktiv zwischen -----------------------------------------------------------------------------------------
			sSql &= String.Format("{0} MA.MANr In (Select MANr From #ESActiv)", strAndString)
		End If

		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		sSql += String.Format("{0}Benutzer.USNr = {1}", strAndString, ClsDataDetail.UserData.UserNr)

		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql
	End Function

	Function GetSortString_Template_1(ByVal strMySortbez As String) As String
		Dim strSort As String = " Order by"
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Kandidatennummer
		'1 - Kandidatenname
		'2 - Kandidatenstrasse
		'3 - Kandidaten-Ort
		'4 - Kandidaten-PLZ
		'5 - Geburtsdatum (nach Datum)
		'6 - Geburtstage (nach Tage)
		strName = Regex.Split(strMySortbez, ",")
		strMyName = String.Empty
		For i As Integer = 0 To strName.Length - 1
			Select Case CInt(Val(Left(strName(i).ToString.Trim, 1)))
				Case 0          ' Nach Kandidatennummer
					If Left(strName(i).ToString.Trim, 1) <> "0" Then
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " " & strName(i).ToString.Trim
					Else
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Mitarbeiternummer"

					End If

				Case 1          ' Kandidatenname
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANachname"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"

				Case 2          ' Kandidatenstrasse
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAStrasse"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenplz"

				Case 3          ' Ort
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAOrt"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidaten-Ortschaft"
				Case 4          ' PLZ
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAPLZ"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidaten-Postleitzahl"

				Case 5          ' GebDat
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAGetDat"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Geburtsdatum"

				Case 6          ' Day(GebDat)
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Day(MAGebDat)"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Geburtstage"

				Case Else       ' Kandidatenname ??? Wird jedoch nie vorkommen
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANachname, MAOrt"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname und Kandidatenort"

			End Select
		Next i

		'Standard-Sortierung, falls leer
		If strMyName.Trim.Length = 0 Then
			strMyName = " MANachname, MAVorname"
			strSortBez = m_xml.GetSafeTranslationValue("Kandidatenname und Kandidatenvorname")
		End If
		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function

#End Region


#Region "Funktionen zur Suche nach Daten..."

	'Sub New(ByVal strMAKontakt As String, ByVal strMAState As String)

	'  GetMAKontakt = strMAKontakt
	'  GetMAState = strMAState

	'End Sub


	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	''' <param name="frmTest"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString(ByVal frmTest As frmMASearch) As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0

		With frmTest
			' Standard SQL-Query der Tabelle Mitarbeiter beim Fehlen der XML-Datei
			sSql = "Begin Try Drop Table _MATemplate_{0} End Try Begin Catch End Catch "
			sSql += "Select DISTINCT(MA.MANr), MA.ID as MAID, MA.KST, "
			sSql += "MA.Nachname as MANachname, MA.Vorname as MAVorname, "
			sSql += "MA.PLZ As MAPLZ, MA.Ort As MAOrt, "
			sSql += "MA.Land As MALand, MA.Telefon3 as MATelefon3, "
			sSql += "MA.Wohnt_Bei As MACo, MA.Postfach As MAPostfach, MA.Strasse As MAStrasse, MA.Telefon_G as MATelefonG, "
			sSql += "MA.Telefon_P as MATelefonP, MA.Natel As MANatel, IsNull(MA.MA_SMS_Mailing, 0) As MA_SMS_Mailing, IsNull(MA.MA_EMail_Mailing, 0) As MA_EMAil_Mailing, "
			sSql += "MA.GebDat as MAGebDat, MA.AHV_Nr as MAAHV_Nr, "
			sSql += "MA.Geschlecht as MAGeschlecht, MA.Beruf As MABeruf, MA.BerufCode As MABerufCode, "
			sSql += "MA_LOSetting.Currency as MACurrency, MA.Bewillig as MABewillig, "
			sSql += "MA.Bew_Bis as MABew_Bis, "
			sSql += "MA.Q_Steuer as MAQ_Steuer, "
			sSql += "(SELECT Top 1 IsNull(Notice_Common, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS MAV_Hinweis, "
			sSql += "(SELECT Top 1 IsNull(Notice_Employment, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_Employment, "
			sSql += "(SELECT Top 1 IsNull(Notice_Report, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_Report, "
			sSql += "(SELECT Top 1 IsNull(Notice_AdvancedPayment, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_AdvancedPayment, "
			sSql += "(SELECT Top 1 IsNull(Notice_Payroll, '') FROM dbo.tbl_MA_Notices Where tbl_MA_Notices.EmployeeNumber = MA.MANr) AS Notice_Payroll, "

			sSql += "MA.AHV_Nr_New as MAAHV_Nr_New, "
			sSql += "MA.Nationality as MANationality, MA.Zivilstand as MAZivilstand, "
			sSql += "MA.S_Kanton as MAS_Kanton, "
			sSql += "MA.ForeignCategory, MA.EmploymentType, MA.OtherEmploymentType, MA.TypeofStay, MA.CHPartner, MA.NoSpecialTax, "

			sSql += "MAKK.KStat1 as MAKStat1, MAKK.KStat2 as MAKStat2, "
			sSql += "MAKK.KontaktHow as MAKontaktHow, "
			sSql += "MA.eMail As MAeMail, MA_LOSetting.Zahlart as MAZahlungsart, "
			sSql += "MA.Send2WOS as MASend2WOS, MA.GebOrt, "

			sSql += "MA_LL.Dateiname, "
			sSql &= "CONVERT(NVARCHAR(4000), MA_LL.Reserve1) AS Reserve1, " +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve2) AS Reserve2, CONVERT(NVARCHAR(4000), MA_LL.Reserve3) AS Reserve3, " +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve4) AS Reserve4, CONVERT(NVARCHAR(4000), MA_LL.Reserve5) AS Reserve5, " +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve6) AS Reserve6, CONVERT(NVARCHAR(4000), MA_LL.Reserve7) AS Reserve7, " +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve8) AS Reserve8, CONVERT(NVARCHAR(4000), MA_LL.Reserve9) AS Reserve9, " +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve10) AS Reserve10, CONVERT(NVARCHAR(4000), MA_LL.Reserve11) AS Reserve11," +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve12) AS Reserve12, CONVERT(NVARCHAR(4000), MA_LL.Reserve13) AS Reserve13, " +
								"CONVERT(NVARCHAR(4000), MA_LL.Reserve14) AS Reserve14, CONVERT(NVARCHAR(4000), MA_LL.Reserve15) AS Reserve15, "

			sSql += "MA_LL.Referenz1, MA_LL.Referenz2, MA_LL.Referenz3, "
			sSql += "MA_LL.Abteil1, MA_LL.Abteil2, MA_LL.Abteil3, "
			sSql += "MA_LL.Ort1, MA_LL.Ort2, MA_LL.Ort3, "
			sSql += "MA_LL.Ref1Firma, MA_LL.Ref2Firma, MA_LL.Ref3Firma, "
			sSql += "MA_LL.Ref1Telefon, MA_LL.Ref2Telefon, MA_LL.Ref3Telefon, "
			sSql += "MA_LL.Ref1Text, MA_LL.Ref2Text, MA_LL.Ref3Text, "

			sSql += "Benutzer.Vorname AS US_Vorname, Benutzer.Nachname AS US_Nachname, "
			sSql += "Benutzer.Anrede AS US_Anrede, Benutzer.Postfach AS US_Postfach, "
			sSql += "Benutzer.Strasse AS US_Strasse, Benutzer.Ort AS US_Ort, "
			sSql += "Benutzer.PLZ AS US_PLZ, Benutzer.Land AS US_Land, "
			sSql += "Benutzer.Telefon AS US_Telefon, Benutzer.Telefax AS US_Telefax, "
			sSql += "Benutzer.Natel AS US_Natel, Benutzer.eMail AS US_eMail, "
			sSql += "Benutzer.Homepage AS US_Homepage, Benutzer.Abteilung AS US_Abteilung, "
			sSql += "Benutzer.USFiliale AS US_USFiliale, IsNull((Select Top 1 (US.Nachname + ', ' + US.Vorname) As MABerater From Benutzer US Where US.Kst = MA.KST), '') As MABerater, "

			' Die Farbe der Hauptmaske für die Kandidaten übernehmen
			' Die SQL-String ist auf der Datenbank
			Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
			Dim cmdFarbe As SqlCommand = New SqlCommand("SELECT CaseString FROM TBL_COLORS WHERE Modulname='MA0'", conn)
			conn.Open()
			sSql += cmdFarbe.ExecuteScalar().ToString
			conn.Close()

			sSql += "Into _MATemplate_{0} "
			sSql += "From Mitarbeiter MA "
			sSql += "Left Join MASonstiges On "
			sSql += "MA.MANr = MASonstiges.MANr "
			sSql += "Left Join MAKontakt_Komm MAKK On "
			sSql += "MA.MANr = MAKK.MANr "
			sSql += "Left Join MA_LOSetting On "
			sSql += "MA.MANr = MA_LOSetting.MANr "
			sSql += "Left Join MA_LL On "
			sSql += "MA.MANr = MA_LL.MANr "


			sSql += ", Benutzer "

		End With

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmMASearch) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = ClsDataDetail.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)
		Dim strFieldName As String = String.Empty
		Dim fieldValue As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim cv As ComboBoxItem
		Dim ExcludeValue As Boolean

		With frmTest

			' 1. Seite === Allgemeine ============================================================================================
			' ====================================================================================================================

			' Mitarbeiternummer MANr --------------------------------------------------------------------
			Dim manr1 As String = .txtMANr_1.Text.Replace("'", "").Replace("*", "%").Trim
			Dim manr2 As String = .txtMANr_2.Text.Replace("'", "").Replace("*", "%").Trim

			If manr1 = String.Empty And manr2 = String.Empty Then   ' do nothing
			ElseIf manr1.Contains("%") Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiternummer wie ({0}){1}"), manr1, vbLf)
				sSql += String.Format(" MA.MANr Like '{0}'", manr1)
				' Suche mehrere MANr getrennt durch Komma
			ElseIf InStr(manr1, ",") > 0 Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiternummer wie ({0}){1}"), manr1, vbLf)
				sSql += " MA.MANr In ("
				For Each manr In manr1.Split(CChar(","))
					sSql += String.Format("'{0}',", manr.Replace("*", "%"))
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				' Suche genau eine MANr
			ElseIf manr1 = manr2 Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiternummer = {0}{1}"), manr1, vbLf)
				sSql += String.Format(" MA.MANr = '{0}'", manr1)

			ElseIf manr1 <> "" And manr2 = "" Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiternummer ab {0}{1}"), manr1, vbLf)
				sSql += String.Format(" MA.MANr >= '{0}'", manr1)

			ElseIf manr1 = "" And manr2 <> "" Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiternummer bis {0}{1}"), manr2, vbLf)
				sSql += String.Format(" MA.MANr <= {0}", manr2)

			Else
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiternummer zwischen {0} und {1}{2}"), manr1, manr2, vbLf)
				sSql += String.Format(" MA.MANr Between {0} And {1}",
				 manr1.Trim, manr2)
			End If

			' Name Nachname ------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			Dim nachname1 = .txtMANachname_1.Text.Replace("'", "''").Replace("*", "%").Trim
			Dim nachname2 = .txtMANachname_2.Text.Replace("'", "''").Replace("*", "%").Trim

			' Keine Suche
			If nachname1 = "" And nachname2 = "" Then

				'Suche mit Sonderzeichen %
			ElseIf nachname1.Contains("%") Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitername wie ({0}){1}"), nachname1, vbLf)
				sSql += String.Format("{0}MA.Nachname Like '{1}'", strAndString, nachname1)

				'Suche nach genau einem Namen
			ElseIf UCase(nachname1) = UCase(nachname2) Then
				FilterBez += "Mitarbeitername = " & nachname2 & vbLf
				sSql += String.Format("{0}MA.Nachname like '{1}'", strAndString, nachname1.Trim)

				' Suche nach mehrere Namen getrennt durch Komma
			ElseIf nachname1.Contains(",") Then
				sSql += String.Format("{0}MA.Nachname in (", strAndString)
				For Each nname In nachname1.Split(CChar(","))
					sSql += String.Format("'{0}',", nname.Trim)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				FilterBez += "Mitarbeiter " & nachname1 & vbLf

				' Suche ab dem ersten Namen
			ElseIf nachname1 <> "" And nachname2 = "" Then
				FilterBez += "Mitarbeitername ab " & nachname1 & vbLf
				sSql += String.Format("{0}MA.Nachname >= '{1}'", strAndString, nachname1)

				' Suche bis zum zweiten Namen 
				' ACHTUNG: Damit auch bis und mit letzter Buchstabe selektiert wird,
				' muss 'zzz' als Workaround hinzugefügt werden.
			ElseIf nachname1 = "" And nachname2 <> "" Then
				FilterBez += "Mitarbeitername bis " & nachname2 & vbLf
				sSql += String.Format("{0}MA.Nachname <= '{1}zzz'", strAndString, nachname2)

				' Suche zwischen den ersten und zweiten Namen
				' ACHTUNG: (siehe oben)
			Else
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitername zwischen {0} und {1}{2}"), nachname1, nachname2, vbLf)
				sSql += String.Format("{0}MA.Nachname Between '{1}' And '{2}zzz'",
				strAndString, nachname1, nachname2)
			End If

			' Vorname ------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			Dim vorname1 = .txtMAVorname_1.Text.Replace("'", "''").Replace("*", "%").Trim
			Dim vorname2 = .txtMAVorname_2.Text.Replace("'", "''").Replace("*", "%").Trim

			'Keine Suche, wenn beide Felder leer
			If vorname1 = "" And vorname2 = "" Then

				' Suche mit Sonderzeichen %
			ElseIf vorname1.Contains("*") Or vorname1.Contains("%") Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitervorname wie ({0}){1}"), vorname1, vbLf)
				sSql += String.Format("{0} MA.Vorname Like '{1}'", strAndString, vorname1)

				' Suche nach genau einem Vornamen
			ElseIf UCase(vorname1) = UCase(vorname2) Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitervorname = {0}{1}"), vorname1, vbLf)
				sSql += String.Format("{0}MA.Vorname like '{1}'", strAndString, vorname1)

				' Suche nach mehrere Vornamen getrennt durch Komma
			ElseIf vorname1.Contains(",") Then
				sSql += String.Format("{0}MA.Vorname in (", strAndString)
				For Each vname In vorname1.Split(CChar(","))
					sSql += String.Format("'{0}',", vname.Trim)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"
				FilterBez += "Mitarbeitervorname " & vorname1 & vbLf

				' Suche ab dem ersten Namen
			ElseIf vorname1 <> "" And vorname2 = "" Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitervorname ab {0}{1}"), vorname1, vbLf)
				sSql += String.Format("{0}MA.Vorname >= '{1}'", strAndString, vorname1)

				' Suche bis zum zweiten Namen
				' ACHTUNG: Damit auch bis und mit letzter Buchstabe selektiert wird,
				' muss 'zzz' als Workaround hinzugefügt werden.
			ElseIf vorname1 = "" And vorname2 <> "" Then
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitervorname bis {0}{1}"), vorname2, vbLf)
				sSql += String.Format("{0}MA.Vorname <= '{1}zzz'", strAndString, vorname2)

				' Suche zwischen den ersten und zweiten Vornamen
				' ACHTUNG: (siehe oben)
			Else
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitervorname zwischen {0} und {1}{2}"), vorname1, vorname2, vbLf)
				sSql += String.Format("{0}MA.Vorname Between '{1}' And '{2}zzz'", strAndString, vorname1, vorname2)
			End If

			' Wohnt_bei -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtMACo.Text.Replace("'", "''").Replace("*", "%")
			' Suche nach einem oder mehrere Orte
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Co-Adresse ({0}){1}"), sZusatzBez, vbLf)
				sSql += String.Format("{0}(", strAndString)
				Dim strOrString As String = ""
				For Each CoBez As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("{0}MA.Wohnt_bei Like '{1}'", strOrString, CoBez.Trim)
					strOrString = " Or "
				Next
				sSql += ")"
			End If

			' Postfach -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtMAPostfach.Text.Replace("'", "''").Replace("*", "%")
			' Suche nach einem oder mehrere Orte
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Postfach-Adresse ({0}){1}"), sZusatzBez, vbLf)
				sSql += String.Format("{0}(", strAndString)
				Dim strOrString As String = ""
				For Each CoBez As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("{0}MA.Postfach Like '{1}'", strOrString, CoBez.Trim)
					strOrString = " Or "
				Next
				sSql += ")"
			End If

			' Strasse -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtMAStrasse.Text.Replace("'", "''").Replace("*", "%")
			' Suche nach einem oder mehrere Orte
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Strasse ({0}){1}"), sZusatzBez, vbLf)
				sSql += String.Format("{0}(", strAndString)
				Dim strOrString As String = ""
				For Each CoBez As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("{0}MA.Strasse Like '{1}'", strOrString, CoBez.Trim)
					strOrString = " Or "
				Next
				sSql += ")"
			End If

			' PLZ -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtMAPLZ_1.Text.Replace("'", "''").Replace("*", "%").Trim
			' Suche nach einer oder mehrere PLZ
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If sZusatzBez.IndexOf(CChar("-")) > 0 Then
					Dim plzVon As String = sZusatzBez.Split(CChar("-"))(0)
					Dim plzBis As String = sZusatzBez.Split(CChar("-"))(1)
					sSql += String.Format("{0}MA.PLZ Between '{1}' And '{2}'", strAndString, plzVon, plzBis)
					FilterBez += String.Format("Mitarbeiter mit PLZ zwischen {0} und {1}{2}", plzVon, plzBis, vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit PLZ ({0}){1}"), sZusatzBez, vbLf)
					sSql += String.Format("{0}(", strAndString)
					Dim strOrString As String = ""
					For Each plz As String In sZusatzBez.Split(CChar(","))
						sSql += String.Format("{0}MA.PLZ like '{1}'", strOrString, plz.Trim)
						strOrString = " Or "
					Next
					sSql += ")"
				End If
			End If

			' Ort -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtMAOrt_1.Text.Replace("'", "''").Replace("*", "%")
			' Suche nach einem oder mehrere Orte
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Ort ({0}){1}"), sZusatzBez, vbLf)
				sSql += String.Format("{0}(", strAndString)
				Dim strOrString As String = ""
				For Each ortBez As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("{0}MA.Ort Like '{1}'", strOrString, ortBez.Trim)
					strOrString = " Or "
				Next
				sSql += ")"
			End If

			' Kanton (Wohnsitz) --------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .Cbo_MAKanton.Text <> String.Empty Then
				sZusatzBez = .Cbo_MAKanton.Text.Trim.Replace("*", "%").Replace("'", "''").Trim
				Dim leereFelder As Boolean = False
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					If strMyName = "Leere Felder" Then
						sSql += strAndString & "((MA.MA_Kanton Is Null Or MA.MA_Kanton = '') Or "
						strMyName = strMyName.Replace("Leere Felder", "")
						leereFelder = True
					End If

				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				If InStr(sZusatzBez.Trim, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
				End If

				sSql += strAndString & "MA.PLZ In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"
				If leereFelder Then
					sSql += ")"
				End If

				FilterBez += "Kanton wie (" & .Cbo_MAKanton.Text & ") " & vbLf

			End If

			' Herkunftsland des Mitarbeiter --------------------------------------------------------------------------
			If Not .lueCountry.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueCountry.EditValue) Then
				ExcludeValue = .ExcludeSelectedLookupEditValue(.lueCountry)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueCountry.EditValue.Trim.Replace("'", "''").Replace(" ", "").Trim

				sSql += String.Format("{0}MA.Land {1} (", strAndString, If(ExcludeValue, "Not In", "In"))
				fieldValue = .lueCountry.EditValue
				sSql += String.Format("'{0}',", fieldValue)

				sSql = sSql.Remove(sSql.Length - 1, 1)
				sSql += ")"

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten-Land {1} ({2}){0}"), vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Nationalität -------------------------------------------------------------------------------------------
			If Not .lueNationality.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueNationality.EditValue) Then
				ExcludeValue = .ExcludeSelectedLookupEditValue(.lueNationality)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueNationality.EditValue.Trim.Replace(" ", "").Replace("'", "''")

				sSql += String.Format("{0}MA.Nationality {1} (", strAndString, If(ExcludeValue, "Not In", "In"))
				fieldValue = .lueNationality.EditValue
				sSql += String.Format("'{0}',", fieldValue)

				sSql = sSql.Remove(sSql.Length - 1, 1)
				sSql += ")"

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten-Nationalität {1} ({2}){0}"), vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' Geschlecht ---------------------------------------------------------------------------------------------
			If .Cbo_MAGeschlecht.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAGeschlecht.SelectedItem, ComboBoxItem)

				sZusatzBez = cv.Value.Replace("'", "''")
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geschlecht = {0}{1}"), cv.Text, vbLf)
				sZusatzBez = sZusatzBez.Replace(" ", "").Replace(",", "','")
				sSql += String.Format("{0}MA.Geschlecht in ('{1}')", strAndString, sZusatzBez)
			End If

			' Zivilstand ---------------------------------------------------------------------------------------------
			If Not .lueCivilstate.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueCivilstate.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim zv As String = .lueCivilstate.EditValue.Replace("'", "''").Replace("*", "%").Trim
				fieldValue = .lueCivilstate.EditValue

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Zivilstand = {0}{1}"), fieldValue, vbLf)
				sSql += String.Format("{0}MA.Zivilstand in ('{1}')", strAndString, fieldValue.Replace(" ", "").Replace(",", "','"))
			End If

			' Korrespondenzsprache -------------------------------------------------------------------------------------
			If .Cbo_MAKorrSprache.Text.Length > 0 Or
			 .Cbo_MAKorrSprache.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}MA.Sprache = '{1}'", strAndString,
				 .Cbo_MAKorrSprache.Text.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit der Sprache = {0}{1}"),
				.Cbo_MAKorrSprache.Text, vbLf)
			End If

			' Dauerstelle / Vermittelt  ------------------------------------------------------------------------------
			If .Cbo_MAVermittelt.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAVermittelt.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}(MAKK.DStellen = 0 Or MAKK.DStellen Is Null) ", strAndString)
				Else
					sSql += String.Format("{0}MAKK.DStellen = 1 ", strAndString)
				End If

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				 .lblMAVermittelt.Text, cv.Text, vbLf)
			End If

			' Nicht mehr einsetzen, gesperrt  -------------------------------------------------------------------------
			If .Cbo_MAGesperrt.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAGesperrt.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}(MAKK.NoES = 0 Or MAKK.NoES Is Null) ", strAndString)
				Else
					sSql += String.Format("{0}MAKK.NoES = 1 ", strAndString)
				End If

				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}",
				 .lblMAGesperrt.Text, cv.Text, vbLf)
			End If

			' Unterlagen über WOS zugänglich machen  -------------------------------------------------------------------
			If .Cbo_MAUnterlagenÜberWOS.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAUnterlagenÜberWOS.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}(MA.Send2WOS = 0 Or MA.Send2WOS Is Null) ", strAndString)
				Else
					sSql += String.Format("{0}MA.Send2WOS = 1 ", strAndString)
				End If

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				 .lblMAUnterlagenÜberWOS.Text, cv.Text, vbLf)
			End If

			' Bewilligung --------------------------------------------------------------------------------------------
			' Bewilligungs-Art
			If Not .luePermissionCode.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.luePermissionCode.EditValue) Then
				ExcludeValue = .ExcludeSelectedLookupEditValue(.luePermissionCode)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .luePermissionCode.EditValue.Trim.Replace("'", "''").Replace(" ", "").Trim

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next
				sSql += String.Format("{0}MA.Bewillig {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), strMyName.Trim.Replace(",", "','"))

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Bewilligung {1} ({2}){0}"), vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			If Not .lueForeignCategory.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueForeignCategory.EditValue

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next
				sSql += String.Format("{0}MA.ForeignCategory In ('{1}')", strAndString, strMyName.Trim.Replace(",", "','"))

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Bewilligung-Kategorie In ({1}){0}"), vbNewLine, sZusatzBez)

			End If

			If Not .lueCHPartner.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueCHPartner.EditValue
				sSql += String.Format("{0}IsNull(MA.CHPartner, 0) = {1}", strAndString, sZusatzBez)

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Eheschliessung oder Eingetragene Partnerschaft mit einer Person mit Schweizer Bürgerrecht: {1}{0}"), vbNewLine, If(sZusatzBez = "0", "Nein", "Ja"))
			End If

			If Not .lueNoSpecialTax.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueNoSpecialTax.EditValue
				sSql += String.Format("{0}IsNull(MA.NoSpecialTax, 0) = {1}", strAndString, sZusatzBez)

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Flüchtlinge mit Sonderabgabe: {1}{0}"), vbNewLine, If(sZusatzBez = "0", "Nein", "Ja"))
			End If

			If Not .lueTypeofStay.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .lueTypeofStay.EditValue

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next
				sSql += String.Format("{0}MA.TypeofStay In ('{1}')", strAndString, strMyName.Trim.Replace(",", "','"))

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Aufenthaltsart In ({1}){0}"), vbNewLine, sZusatzBez)

			End If

			If Not .lueEmploymentType.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				fieldValue = .lueEmploymentType.EditValue

				strMyName = String.Empty
				For Each bez As String In fieldValue.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next
				sSql += String.Format("{0}MA.EmploymentType In ('{1}')", strAndString, strMyName.Trim.Replace(",", "','"))

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Beschäftigungsart In ({1}){0}"), vbNewLine, fieldValue)

			End If

			If Not .lueOtherEmploymentType.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				fieldValue = .lueOtherEmploymentType.EditValue

				strMyName = String.Empty
				For Each bez As String In fieldValue.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next
				sSql += String.Format("{0}MA.OtherEmploymentType In ('{1}')", strAndString, strMyName.Trim.Replace(",", "','"))

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Weitere Beschäftigungsart In ({1}){0}"), vbNewLine, fieldValue)

			End If


			' Bewilligung gültig bis Monat
			If .Cbo_MABewBisMonat.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				' Explizit gültige Bewilligung im Monat
				sSql += String.Format("{0}Month(MA.Bew_Bis) = {1}", strAndString, .Cbo_MABewBisMonat.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Bewilligung gültig genau im Monat {0}{1}"),
				 .Cbo_MABewBisMonat.Text, vbLf)
			End If

			' Bewilligung gültig bis Jahr
			If .Cbo_MABewBisJahr.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				' Explizit gültige Bewilligung in den Jahren
				sSql += String.Format("{0}Year(MA.Bew_Bis) = {1}", strAndString, .Cbo_MABewBisJahr.Text)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Bewilligung gültig im Jahr {0}{1}"),
				 .Cbo_MABewBisJahr.Text, vbLf)
			End If


			' Heimatort ----------------------------------------------------------------------------------------------
			If Not .lueBirthPlace.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueBirthPlace.EditValue) Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim heimatort As String = .lueBirthPlace.EditValue.Replace("'", "''").Replace("*", "%").Trim
				If heimatort.Contains("%") Then
					sSql += String.Format("{0}MA.GebOrt Like '{1}'", strAndString, heimatort)
				ElseIf heimatort.Contains(",") Then
					sSql += String.Format("{0} MA.GebOrt In (", strAndString)
					For Each hort As String In heimatort.Split(CChar(","))
						sSql += String.Format("'{0}',", hort.Trim)
					Next
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ")"
				Else
					sSql += String.Format("{0}MA.GebOrt = '{1}'", strAndString, heimatort)
				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Heimatort = {0}{1}"), .lueBirthPlace.EditValue, vbLf)
			End If

			' Q-Steuer -----------------------------------------------------------------------------------------------
			If Not .lueQSTCode.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(.lueQSTCode.EditValue) Then
				ExcludeValue = .ExcludeSelectedLookupEditValue(.lueQSTCode)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				sSql += String.Format("{0}MA.Q_Steuer {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), .lueQSTCode.EditValue.Trim.Replace(",", "','"))

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Q-Steuer {1} ({2}){0}"), vbNewLine, If(ExcludeValue, "NICHT in", "in"), .lueQSTCode.EditValue)

			End If

			' S-Kanton (Steuerkanton) --------------------------------------------------------------------------------
			If .Cbo_MASKanton.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim leereFelder As Boolean = False
				If .Cbo_MASKanton.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MA.S_Kanton Is Null Or ", strAndString)
					leereFelder = True
				End If

				If Not leereFelder Then
					sSql += strAndString
				End If

				sSql += String.Format("MA.S_Kanton in ('{0}')",
				 .Cbo_MASKanton.Text.Trim.Replace("Leere Felder", "").Replace(" ", "") _
				 .Replace("'", "''").Replace("*", "%").Replace(",", "','"))

				If leereFelder Then
					sSql += ")"
				End If

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter in den S-Kanton {0}{1}"), .Cbo_MASKanton.Text, vbLf)

			End If

			' Ansässigkeit -------------------------------------------------------------------------------------------
			If Not .lueCertificateForResidenceReceived.EditValue Is Nothing Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				fieldValue = .lueCertificateForResidenceReceived.EditValue

				sSql += String.Format("{0}IsNull(MA.Ansaessigkeit, 0) = {1}", strAndString, fieldValue)
				FilterBez += String.Format("Mitarbeiter mit Ansaessigkeit = {0}{1}", fieldValue, vbNewLine)
			End If

			' Gemeinde -----------------------------------------------------------------------------------------------
			Dim communityCodes As String = String.Empty
			Dim communityData = .lueCommunity.Properties.GetItems.GetCheckedValues()
			Dim communityLabel = .lueCommunity.Text
			For Each itm In communityData
				If Not String.IsNullOrWhiteSpace(itm) Then communityCodes &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(communityCodes), "", ", "), itm)
			Next


			If Not String.IsNullOrWhiteSpace(communityCodes) Then
				'ExcludeValue = .ExcludeSelectedLSTValue(.btnHideGemeinde)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				'sZusatzBez = GetLstItems(.Lst_MAQSTGemeinde) '.Replace("'", "''")

				sSql += String.Format("{0}MA.TaxCommunityCode IN ({1}) ", strAndString, communityCodes)
				FilterBez += String.Format("QST-Gemeinden in ({1}){0}", vbNewLine, communityLabel)
			End If


			' 2. Seite === Betreuung =============================================================================================
			' ====================================================================================================================

			' Berater ---------------------------------------------------------------------------------------------------------
			If .Cbo_MABetreuer.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim kst As String = .Cbo_MABetreuer.Text.Replace(" ", "").Replace("'", "''").Replace(",", "','").Replace("*", "%").Trim
				If .Cbo_MABetreuer.Text.Length > 0 Then
					kst = DirectCast(.Cbo_MABetreuer.SelectedItem, ComboBoxItem).Value
				End If
				sSql += String.Format("{0}MA.KST = '{1}'", strAndString, kst)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit dem Betreuer = {0}{1}"),
				 .Cbo_MABetreuer.Text, vbLf)
			End If

			' Geschäftsstelle (Filiale) ---------------------------------------------------------------------------------------
			If .Cbo_MAGeschaeftsstellen.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim leereFelder As Boolean = False
				If .Cbo_MAGeschaeftsstellen.Text.Contains("Leere Felder") Then
					leereFelder = True
				End If
				sZusatzBez = .Cbo_MAGeschaeftsstellen.Text.Trim.Replace("Leere Felder", "").Replace("'", "''")
				sSql += String.Format("{0} MA.MANr In	(Select MANr From MA_Filiale Where ", strAndString)
				If leereFelder Then
					sSql += "("
				End If
				sSql += " MA_Filiale.Bezeichnung in ("
				For Each filiale In sZusatzBez.Split(CChar(","))
					sSql += String.Format("'{0}',", filiale.Replace("*", "%").Trim)
				Next
				sSql = sSql.Substring(0, sSql.Length - 1)
				sSql += ")"

				If leereFelder Then
					sSql += " Or MA_Filiale.Bezeichnung Is Null) "
				End If

				sSql += "Group By MANr)"

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter in den Filialen = {0}{1}"),
				 .Cbo_MAGeschaeftsstellen.Text, vbLf)
			End If

			' Kontakt ---------------------------------------------------------------------------------------------------------
			If .Cbo_MAKontakt.Text.Length > 0 Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_MAKontakt)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAKontakt.Text.Trim.Replace("'", "''")

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(.Cbo_MAKontakt.Properties.SeparatorChar))
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_MAKontakt.Properties.SeparatorChar, "")) & bez.ToString.Trim
				Next

				sSql += String.Format("{0}MAKK.KontaktHow {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), strMyName.Trim.Replace(.Cbo_MAKontakt.Properties.SeparatorChar, "','"))

				FilterBez += String.Format("Kandidatenkontakt {1} ({2}){0}", vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' 1. Status -------------------------------------------------------------------------------------------------------
			If .Cbo_MAStatus1.Text <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_MAStatus1)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAStatus1.Text.Trim.Replace("'", "''")

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(.Cbo_MAStatus1.Properties.SeparatorChar))
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_MAStatus1.Properties.SeparatorChar, "")) & bez.ToString.Trim
				Next

				sSql += String.Format("{0}MAKK.KStat1 {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), strMyName.Trim.Replace(.Cbo_MAStatus1.Properties.SeparatorChar, "','"))
				FilterBez += String.Format("Kandidatenstatus1 {1} ({2}){0}", vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' 2. Status -------------------------------------------------------------------------------------------------------
			If .Cbo_MAStatus2.Text <> String.Empty Then
				ExcludeValue = .ExcludeSelectedCheckedComboboxValue(.Cbo_MAStatus2)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAStatus2.Text.Trim.Replace("'", "''")

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(.Cbo_MAStatus2.Properties.SeparatorChar))
					strMyName += CStr(IIf(strMyName.Length > 0, .Cbo_MAStatus2.Properties.SeparatorChar, "")) & bez.ToString.Trim
				Next

				sSql += String.Format("{0}MAKK.KStat2 {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), strMyName.Trim.Replace(.Cbo_MAStatus2.Properties.SeparatorChar, "','"))
				FilterBez += String.Format("Kandidatenstatus2 {1} ({2}){0}", vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' Fahrzeug --------------------------------------------------------------------------------------------------------
			If .Cbo_MAFahrzeug.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim leereFelder As Boolean = False
				sZusatzBez = .Cbo_MAFahrzeug.Text.Trim.Replace(", ", ",").Replace("'", "''")
				If .Cbo_MAFahrzeug.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MASonstiges.Fahrzeug Is Null Or ", strAndString)
					leereFelder = True
				End If
				If Not leereFelder Then sSql += strAndString

				sSql += "MASonstiges.Fahrzeug In ("
				For Each strText As String In sZusatzBez.Split(CChar(","))
					sSql += String.Format("'{0}',", strText.Trim)
				Next
				sSql = sSql.Remove(sSql.Length - 1, 1)
				sSql += ")"
				If leereFelder Then sSql += ")"
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit den Fahrzeuge {0}{1}"), .Cbo_MAFahrzeug.Text, vbLf)
			End If

			' Führerschein ----------------------------------------------------------------------------------------------------
			If .Cbo_MAFuehrerausweis.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim leereFelder As Boolean = False
				If .Cbo_MAFuehrerausweis.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}((MASonstiges.F_Schein1 Is Null Or ", strAndString)
					sSql += "MASonstiges.F_Schein2 Is Null Or "
					sSql += "MASonstiges.F_Schein3 Is Null) Or "
					leereFelder = True
				End If
				If Not leereFelder Then sSql += strAndString
				sZusatzBez = .Cbo_MAFuehrerausweis.Text.Trim.Replace("Leere Felder", "").Replace(" ", "").Replace("'", "''").Replace(",", "','")
				sSql += String.Format("(MASonstiges.F_Schein1 in ('{0}') Or ", sZusatzBez)
				sSql += String.Format("MASonstiges.F_Schein2 in ('{0}') Or ", sZusatzBez)
				sSql += String.Format("MASonstiges.F_Schein3 in ('{0}'))", sZusatzBez)
				If leereFelder Then sSql += ")"

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit den Führerausweise {0}{1}"), .Cbo_MAFuehrerausweis.Text, vbLf)
			End If

			' 1.Reserve -------------------------------------------------------------------------------------------------------
			If .Cbo_MAReserve1.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim leereFelder As Boolean = False
				If .Cbo_MAReserve1.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MAKK.Res1 Is Null Or ", strAndString)
					leereFelder = True
				End If
				If Not leereFelder Then sSql += strAndString
				sSql += String.Format("MAKK.Res1 = '{0}'", .Cbo_MAReserve1.Text.Replace("'", "''").Replace("Leere Felder", "").Trim)
				If leereFelder Then sSql += ")"

				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}", .lblReserve1.Text, .Cbo_MAReserve1.Text, vbLf)
			End If
			' 2.Reserve -------------------------------------------------------------------------------------------------------
			If .Cbo_MAReserve2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim leereFelder As Boolean = False
				If .Cbo_MAReserve2.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MAKK.Res2 Is Null Or ", strAndString)
					leereFelder = True
				End If
				If Not leereFelder Then sSql += strAndString
				sSql += String.Format("MAKK.Res2 = '{0}'", .Cbo_MAReserve2.Text.Replace("'", "''").Replace("Leere Felder", "").Trim)
				If leereFelder Then sSql += ")"
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblReserve2.Text, .Cbo_MAReserve2.Text, vbLf)
			End If
			' 3.Reserve -------------------------------------------------------------------------------------------------------
			If .Cbo_MAReserve3.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim leereFelder As Boolean = False
				If .Cbo_MAReserve3.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MAKK.Res3 Is Null Or ", strAndString)
					leereFelder = True
				End If
				If Not leereFelder Then sSql += strAndString
				sSql += String.Format("MAKK.Res3 = '{0}'", .Cbo_MAReserve3.Text.Replace("'", "''").Replace("Leere Felder", "").Trim)
				If leereFelder Then sSql += ")"
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblReserve3.Text, .Cbo_MAReserve3.Text, vbLf)
			End If
			' 4.Reserve -------------------------------------------------------------------------------------------------------
			If .Cbo_MAReserve4.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim leereFelder As Boolean = False
				If .Cbo_MAReserve4.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MAKK.Res4 Is Null Or ", strAndString)
					leereFelder = True
				End If
				If Not leereFelder Then sSql += strAndString
				sSql += String.Format("MAKK.Res4 = '{0}'", .Cbo_MAReserve4.Text.Replace("'", "''").Replace("Leere Felder", "").Trim)
				If leereFelder Then sSql += ")"

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblReserve4.Text, .Cbo_MAReserve4.Text, vbLf)
			End If
			' 5.Reserve -------------------------------------------------------------------------------------------------------
			If .Cbo_MAReserve5.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim leereFelder As Boolean = False
				If .Cbo_MAReserve5.Text.Contains("Leere Felder") Then
					sSql += String.Format("{0}(MAKK.Res5 Is Null Or ", strAndString)
					leereFelder = True
				End If

				If Not leereFelder Then
					sSql += strAndString
				End If
				sSql += String.Format("MAKK.Res5 = '{0}'", .Cbo_MAReserve5.Text.Replace("'", "''").Replace("Leere Felder", "").Trim)
				If leereFelder Then sSql += ")"

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblReserve5.Text, .Cbo_MAReserve5.Text, vbLf)
			End If

			' Qualifikationsnachweis ------------------------------------------------------------------------------------------
			If .Cbo_MAQualNachweis.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAQualNachweis.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MAKK.GotDocs = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				 .lblQualNachweis.Text, cv.Text, vbLf)
			End If

			' Geburtsdatum Geboren am --------------------------------------------------------------------------------
			If .deGeb_1.Text <> "" Then If Year(CDate(.deGeb_1.Text)) = 1 Then .deGeb_1.Text = String.Empty
			If .deGeb_2.Text <> "" Then If Year(CDate(.deGeb_2.Text)) = 1 Then .deGeb_2.Text = String.Empty
			If .deGeb_1.Text.Length > 0 Or .deGeb_2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim gebDate1 As String = ""
				Dim gebDate2 As String = ""
				If IsDate(.deGeb_1.Text) Then
					gebDate1 = Date.Parse(.deGeb_1.Text).ToString("d")
				End If
				If IsDate(.deGeb_2.Text) Then
					gebDate2 = Date.Parse(.deGeb_2.Text).ToString("d")
				End If
				If gebDate1.Length > 0 And gebDate2.Length > 0 Then
					' Suche Geburtsdatum zwischen zwei Datum
					sSql += String.Format("{0}MA.GebDat Between Convert(DateTime, '{1} 00:00',104) And Convert(DateTime, '{2} 23:59', 104)",
											strAndString, gebDate1, gebDate2)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter geboren zwischen {0} und {1}{2}"), gebDate1, gebDate2, vbLf)
					' Suche ab erstes Geburtsdatum
				ElseIf gebDate1.Length > 0 Then
					sSql += String.Format("{0}MA.GebDat >= Convert(DateTime,'{1} 00:00',104)", strAndString, gebDate1)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter ab Geburtsdatum {0}{1}"), gebDate1, vbLf)
					' Suche bis zweiten Geburtsdatum
				ElseIf gebDate2.Length > 0 Then
					sSql += String.Format("{0}MA.GebDat <= Convert(DateTime,'{1} 23:59',104)", strAndString, gebDate2)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter bis Geburtsdatum {0}{1}"), gebDate2, vbLf)
				End If
			End If

			' Geboren im Monat ---------------------------------------------------------------------------------------
			If .Cbo_MAGeborenMonat.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = " "
				For Each monat As String In .Cbo_MAGeborenMonat.Text.Split(CChar(","))
					monat = monat.Replace("'", "").Trim
					If monat.Length > 0 And Val(monat) > 0 Then
						sZusatzBez += monat + ","
					End If
				Next
				sZusatzBez = sZusatzBez.Substring(0, sZusatzBez.Length - 1)
				If sZusatzBez.Length > 0 Then
					sSql += String.Format("{0}Month(MA.GebDat) in ({1})", strAndString, sZusatzBez.Trim)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter geboren im Monat {0}{1}"), .Cbo_MAGeborenMonat.Text, vbLf)
				End If
			End If

			' Geboren in der KW --------------------------------------------------------------------------------------
			If Val(.txtMAGebKW_1.Text) > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim kw1 As Integer = CInt(Val(.txtMAGebKW_1.Text))
				Dim firstAndLastDateOfWeek1 As New List(Of Date)

				firstAndLastDateOfWeek1 = m_DateTimeUi.GetMondayAndSundayOfWeek(Now.Year, Now.Month, CInt(kw1))
				Dim StartDay As Integer = firstAndLastDateOfWeek1(0).Day
				Dim EndDay As Integer = firstAndLastDateOfWeek1(1).Day

				Dim EndDayOfStartMonth As Integer = 31
				Dim StartMonth As Integer = firstAndLastDateOfWeek1(0).Month
				Dim EndMonth As Integer = firstAndLastDateOfWeek1(1).Month

				'Suche zwischen der ersten und zweiten Kalenderwochenangabe
				If kw1 > 0 Then

					If StartMonth = EndMonth Then
						sSql += strAndString & String.Format(sqlGebDat, StartDay, EndDay, StartMonth)

					Else
						sSql += strAndString & String.Format(sqlGebDat_2, StartDay, EndDayOfStartMonth, StartMonth, EndDay, EndMonth)

					End If

					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeitergeburtsdatum in der Woche {0}{1}"), kw1, vbLf)

				Else
					m_utilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Achtung: Die Wochenangaben scheinen fehlerhaft zu sein. Sie müssen VON-BIS Woche eingeben!"))

				End If
			End If

			' Alter --------------------------------------------------------------------------------------------------
			If Val(.txtMAAlter_1.Text) > 0 Or Val(.txtMAAlter_2.Text) > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				' Suche zwischen der ersten und zweiten Altersangabe
				Dim alter1 As Double = Val(.txtMAAlter_1.Text.Replace("'", "").Replace("*", "").Replace("%", "").Trim)
				Dim alter2 As Double = Val(.txtMAAlter_2.Text.Replace("'", "").Replace("*", "").Replace("%", "").Trim)
				If alter1 > 0 And alter2 > 0 Then
					' Suche genau mit einer Altersangabe
					If alter1 = alter2 Then
						sSql += String.Format("{0}(Year(GetDate()) - Year(MA.GebDat)) = {1} ", strAndString, alter1)
						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit dem Alter {0}{1}"), .txtMAAlter_1.Text, vbLf)

					Else
						sSql += String.Format("{0}(Year(GetDate()) - Year(MA.GebDat)) Between {1} And {2} ", strAndString, alter1, alter2)
						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter zwischen {0} und {1} Jahren{2}"), .txtMAAlter_1.Text, .txtMAAlter_2.Text, vbLf)

					End If

					' Suche ab der ersten Altersangabe
				ElseIf alter1 > 0 Then
					sSql += String.Format("{0}(Year(GetDate()) - Year(MA.GebDat)) >= {1} ", strAndString, alter1)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter ab dem Alter von {0} Jahren{1}"), .txtMAAlter_1.Text, vbLf)

					' Suche bis zur zweiten Altersangabe
				ElseIf alter2 > 0 Then
					sSql += String.Format("{0}(Year(GetDate()) - Year(MA.GebDat)) <= {1} ", strAndString, alter2)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter bis zum Alter von {0} Jahren{1}"), .txtMAAlter_2.Text, vbLf)

				End If

			End If

			' Geburtstag ---------------------------------------------------------------------------------------------
			If .Cbo_MAGeburtstag.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAGeburtstag.SelectedItem, ComboBoxItem)

				Dim firstAndLastDateOfWeek1 As New List(Of Date)

				Dim dayOfWeek As Integer = Integer.Parse(Date.Now.DayOfWeek.ToString("D"))
				Dim month As Integer = Date.Now.Month
				Dim year As Integer = Date.Now.Year
				Dim kw As Integer = DatePart(DateInterval.WeekOfYear, Date.Now, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)

				Dim gebDat1 As Date
				Dim gebDat2 As Date

				' Die Woche fängt mit Sonntag=0 an, somit muss eine Woche abgezogen werden,
				' falls am Sonntag die Abfrage gestartet wird.
				If dayOfWeek = 0 Then dayOfWeek = 7

				Dim weeknr As Integer = DatePart(DateInterval.WeekOfYear, Now.Date, FirstDayOfWeek.System, FirstWeekOfYear.System)
				Dim lastWeekNr As Integer = weeknr - 1
				Dim nextWeekNr As Integer = weeknr + 1

				If weeknr = 1 Then
					lastWeekNr = DatePart(DateInterval.WeekOfYear, New Date(Now.Year - 1, 12, 31), FirstDayOfWeek.System, FirstWeekOfYear.System)

				ElseIf weeknr >= 52 Then
					If DatePart(DateInterval.WeekOfYear, New Date(Now.Year, 12, 31), FirstDayOfWeek.System, FirstWeekOfYear.System) >= 52 Then
						nextWeekNr = 1
					End If

				End If

				Select Case cv.Value
					Case "HE" ' Heute
						sSql += String.Format("{0}Day(MA.GebDat) = '{1}' And Month(MA.GebDat) = '{2}'", strAndString, Date.Now.Day, Date.Now.Month)
						FilterBez += String.Format("Mitarbeiter-Geburtsttag von heute am {0}{1}", Date.Today.ToShortDateString(), vbLf)

					Case "LW" ' Letzte Woche
						gebDat1 = Date.Now.AddDays(-6 - dayOfWeek)
						gebDat2 = Date.Now.AddDays(0 - dayOfWeek)
						firstAndLastDateOfWeek1 = m_DateTimeUi.GetMondayAndSundayOfWeek(Now.Year, Now.Month, lastWeekNr)
						Dim StartDay As Integer = firstAndLastDateOfWeek1(0).Day
						Dim EndDay As Integer = firstAndLastDateOfWeek1(1).Day

						Dim EndDayOfStartMonth As Integer = 31
						Dim StartMonth As Integer = firstAndLastDateOfWeek1(0).Month
						Dim EndMonth As Integer = firstAndLastDateOfWeek1(1).Month

						If StartMonth = EndMonth Then
							sSql += strAndString & String.Format(sqlGebDat, StartDay, EndDay, StartMonth)

						Else
							sSql += strAndString & String.Format(sqlGebDat_2, StartDay, EndDayOfStartMonth, StartMonth, EndDay, EndMonth)

						End If

						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geburtstage der letzten Woche({1}){0}"), vbLf, lastWeekNr)

					Case "DW" ' Diese Woche
						gebDat1 = Date.Now.AddDays(1 - dayOfWeek)
						gebDat2 = Date.Now.AddDays(7 - dayOfWeek)
						firstAndLastDateOfWeek1 = m_DateTimeUi.GetMondayAndSundayOfWeek(Now.Year, Now.Month, weeknr)
						Dim StartDay As Integer = firstAndLastDateOfWeek1(0).Day
						Dim EndDay As Integer = firstAndLastDateOfWeek1(1).Day

						Dim EndDayOfStartMonth As Integer = 31
						Dim StartMonth As Integer = firstAndLastDateOfWeek1(0).Month
						Dim EndMonth As Integer = firstAndLastDateOfWeek1(1).Month

						If StartMonth = EndMonth Then
							sSql += strAndString & String.Format(sqlGebDat, StartDay, EndDay, StartMonth)

						Else
							sSql += strAndString & String.Format(sqlGebDat_2, StartDay, EndDayOfStartMonth, StartMonth, EndDay, EndMonth)

						End If

						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geburtstage dieser Woche({1}){0}"), vbLf, weeknr)

					Case "NW" ' Nächste Woche
						gebDat1 = Date.Now.AddDays(8 - dayOfWeek)
						gebDat2 = Date.Now.AddDays(14 - dayOfWeek)
						firstAndLastDateOfWeek1 = m_DateTimeUi.GetMondayAndSundayOfWeek(Now.Year, Now.Month, nextWeekNr)
						Dim StartDay As Integer = firstAndLastDateOfWeek1(0).Day
						Dim EndDay As Integer = firstAndLastDateOfWeek1(1).Day

						Dim EndDayOfStartMonth As Integer = 31
						Dim StartMonth As Integer = firstAndLastDateOfWeek1(0).Month
						Dim EndMonth As Integer = firstAndLastDateOfWeek1(1).Month

						If StartMonth = EndMonth Then
							sSql += strAndString & String.Format(sqlGebDat, StartDay, EndDay, StartMonth)

						Else
							sSql += strAndString & String.Format(sqlGebDat_2, StartDay, EndDayOfStartMonth, StartMonth, EndDay, EndMonth)

						End If
						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geburtstage der nächsten Woche({1}){0}"), vbLf, nextWeekNr)

					Case "LM" ' Letzter Monat
						sSql += String.Format("{0}Month(MA.GebDat) = {1}", strAndString, (Now.Month - 1))

						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geburtstage des letzten Monats{0}"), vbLf)

					Case "DM" ' Diesen Monat
						sSql += String.Format("{0}Month(MA.GebDat) = {1}", strAndString, Now.Month)
						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geburtstage dieses Monats{0}"), vbLf)
					Case "NM" ' Nächsten Monat
						sSql += String.Format("{0}Month(MA.GebDat) = {1}", strAndString, (Now.Month + 1))
						FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter-Geburtstage des nächsten Monats{0}"), vbLf)
				End Select

			End If


			' SMS Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_MASMS_NoMailing.Text) <> String.Empty Then
				strFieldName = "MA.MA_SMS_Mailing"
				sZusatzBez = .Cbo_MASMS_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("SMS-Mailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("SMS-Mailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1", strAndString, strFieldName)

				End If

			End If

			' Mailing Mailing ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_MAMail_NoMailing.Text) <> String.Empty Then
				strFieldName = "Ma.MA_EMAil_Mailing"
				sZusatzBez = .Cbo_MAMail_NoMailing.Text.Trim
				If InStr(UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += m_xml.GetSafeTranslationValue("E-Mailmailing darf gesendet werden") & vbLf
					sSql += String.Format("{0}({1} = 0 Or {1} Is Null)", strAndString, strFieldName)

				Else
					FilterBez += m_xml.GetSafeTranslationValue("E-Mailmailing darf nicht gesendet werden") & vbLf
					sSql += String.Format("{0}{1} = 1", strAndString, strFieldName)

				End If
			End If




			' AHV-Karte vollständig -------------------------------------------------------------------------------------------
			If .Cbo_MAAHVvollstaendig.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAAHVvollstaendig.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}Not Len(MA.AHV_Nr) > 11 ", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.AHV_Nr) > 11 ", strAndString)
				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				.lblAHVvollstaendig.Text, cv.Text, vbLf)
			End If

			' Neue AHV-Karte vollständig --------------------------------------------------------------------------------------
			If .Cbo_MANewAHVvollstaendig.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MANewAHVvollstaendig.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}Not Len(MA.AHV_Nr_New) = 16", strAndString)
				Else
					sSql += String.Format("{0}Len(MA.AHV_Nr_New) = 16", strAndString)
				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				.lblNewAHVvollstaendig.Text, cv.Text, vbLf)
			End If

			' Verfügbar -------------------------------------------------------------------------------------------------------
			If .deVerfueg_1.Text <> "" Then If Year(CDate(.deVerfueg_1.Text)) = 1 Then .deVerfueg_1.Text = String.Empty
			If .deVerfueg_2.Text <> "" Then If Year(CDate(.deVerfueg_2.Text)) = 1 Then .deVerfueg_2.Text = String.Empty
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .deVerfueg_1.Text <> String.Empty Or .deVerfueg_2.Text <> String.Empty Then
				Dim verfVon As String = ""
				Dim verfBis As String = ""
				If IsDate(.deVerfueg_1.Text) Then
					verfVon = Date.Parse(.deVerfueg_1.Text).ToString("d")
				End If
				If IsDate(.deVerfueg_2.Text) Then
					verfBis = Date.Parse(.deVerfueg_2.Text).ToString("d")
				End If

				' Suche zwischen zwei Datum
				If verfVon.Length > 0 And verfBis.Length > 0 Then
					sSql += String.Format("{0}(MAKK.ESEnde <= Convert(DateTime, '{1}', 104) ", strAndString, verfVon)
					sSql += "Or MAKK.ESEnde Is Null) And"
					sSql += String.Format("(MAKK.ESAb <= Convert(DateTime, '{0}', 104) Or MAKK.ESAb Is Null)", verfBis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter verfügbar zwischen {0} und {1}{2}"), verfVon, verfBis, vbLf)
					' Such ab erstes Daum
				ElseIf verfVon.Length > 0 Then
					sSql += String.Format("{0}(MAKK.ESAb >= Convert(DateTime, '{1}', 104) Or MAKK.ESAb Is Null)",
																strAndString, verfVon)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter verfügbar ab {0}{1}"), verfVon, vbLf)
					'Suche bis zweites Datum
				ElseIf verfBis.Length > 0 Then
					sSql += String.Format("{0}(MAKK.ESEnde <= Convert(DateTime, '{1}', 104) Or MAKK.ESEnde Is Null)",
								 strAndString, verfBis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter verfügbar bis {0}{1}"), verfBis, vbLf)
				End If
			End If

			' Erfasst am/bis --------------------------------------------------------------------------------------------------
			If .deErfasst_1.Text <> "" Then If Year(CDate(.deErfasst_1.Text)) = 1 Then .deErfasst_1.Text = String.Empty
			If .deErfasst_2.Text <> "" Then If Year(CDate(.deErfasst_2.Text)) = 1 Then .deErfasst_2.Text = String.Empty
			If .deErfasst_1.Text.Length > 0 Or .deErfasst_2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstAm As String = ""
				Dim erfasstBis As String = ""
				If IsDate(.deErfasst_1.Text) Then
					erfasstAm = Date.Parse(.deErfasst_1.Text).ToString("d")
				End If
				If IsDate(.deErfasst_2.Text) Then
					erfasstBis = Date.Parse(.deErfasst_2.Text).ToString("d")
				End If
				If erfasstAm.Length > 0 And erfasstBis.Length > 0 Then
					' Suche zwischen zwei Datum
					sSql += String.Format("{0}MA.CreatedOn Between Convert(DateTime, '{1} 00:00', 104) And Convert(DateTime, '{2} 23:59', 104)",
									 strAndString, erfasstAm, erfasstBis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter erfasst zwischen {0} und {1}{2}"), erfasstAm, erfasstBis, vbLf)
				ElseIf erfasstAm.Length > 0 Then
					' Suche ab erstes Datum
					sSql += String.Format("{0}MA.CreatedOn >= Convert(DateTime,'{1} 00:00',104)", strAndString, erfasstAm)
					FilterBez += String.Format("Mitarbeiter erfasst ab {0}{1}", erfasstAm, vbLf)
				ElseIf erfasstBis.Length > 0 Then
					' Suche bis zweites Datum
					sSql += String.Format("{0}MA.CreatedOn <= Convert(DateTime,'{1} 23:59',104)", strAndString, erfasstBis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter erfasst bis {0}{1}"), erfasstBis, vbLf)
				End If
			End If

			' Geändert am/bis --------------------------------------------------------------------------------------------------
			If .de_MAChange_1.Text <> "" Then If Year(CDate(.de_MAChange_1.Text)) = 1 Then .de_MAChange_1.Text = String.Empty
			If .de_MAChange_2.Text <> "" Then If Year(CDate(.de_MAChange_2.Text)) = 1 Then .de_MAChange_2.Text = String.Empty
			If .de_MAChange_1.Text.Length > 0 Or .de_MAChange_2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim erfasstAm As String = ""
				Dim erfasstBis As String = ""
				If IsDate(.de_MAChange_1.Text) Then
					erfasstAm = Date.Parse(.de_MAChange_1.Text).ToString("d")
				End If
				If IsDate(.de_MAChange_2.Text) Then
					erfasstBis = Date.Parse(.de_MAChange_2.Text).ToString("d")
				End If
				If erfasstAm.Length > 0 And erfasstBis.Length > 0 Then
					' Suche zwischen zwei Datum
					sSql += String.Format("{0}MA.ChangedOn Between Convert(DateTime, '{1} 00:00', 104) And Convert(DateTime, '{2} 23:59', 104)",
									 strAndString, erfasstAm, erfasstBis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter geändert zwischen {0} und {1}{2}"), erfasstAm, erfasstBis, vbLf)
				ElseIf erfasstAm.Length > 0 Then
					' Suche ab erstes Datum
					sSql += String.Format("{0}MA.ChangedOn >= Convert(DateTime,'{1} 00:00',104)", strAndString, erfasstAm)
					FilterBez += String.Format("Mitarbeiter geändert ab {0}{1}", erfasstAm, vbLf)
				ElseIf erfasstBis.Length > 0 Then
					' Suche bis zweites Datum
					sSql += String.Format("{0}MA.ChangedOn <= Convert(DateTime,'{1} 23:59',104)", strAndString, erfasstBis)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter geändert bis {0}{1}"), erfasstBis, vbLf)
				End If
			End If

			' Im Einsatz ------------------------------------------------------------------------------------------------------
			If .Cbo_MAImEinsatz.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAImEinsatz.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0} Not MA.MANr in (Select ES.MANr From ES Group By ES.MANr)", strAndString)
				Else
					sSql += String.Format("{0} MA.MANr in (Select ES.MANr From ES Group By ES.MANr)", strAndString)
				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				.lblImEinsatz.Text, cv.Text, vbLf)
			End If

			' Heute im Einsatz
			If .cboTodayinES.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cboTodayinES.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					' Heute nicht im ES!
					sSql &= String.Format("{0} MA.MANr Not IN (Select ES.MANr From ES Where ( ES.ES_Ende < Convert(Date, GetDate()) ) Group By ES.MANr) ", strAndString)

				Else
					' Heute im ES!
					sSql &= String.Format("{0} MA.MANr IN (Select ES.MANr From ES Where ( (ES.ES_Ende >= Convert(Date, GetDate()) Or ES.ES_Ende Is Null) And ES.ES_Ab <= Convert(Date, GetDate()) ) Group By ES.MANr) ", strAndString)

				End If

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter welche heute {0} Einsatz sind{1}"), If(cv.Value = "0", "nicht im", "im"), vbNewLine)
			End If



			' 3. Seite === Zusatztabellen ========================================================================================
			' ====================================================================================================================
			' Berufgruppe -------------------------------------------------------------------------------------------------
			If .lst_BerufGruppe.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideBerufGruppe)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.lst_BerufGruppe) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM [MA.Berufgruppe] WHERE BerufBez_DE In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Berufsgruppe {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Fachbereiche -------------------------------------------------------------------------------------------------
			If .lst_Fachbereich.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideFachbereich)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.lst_Fachbereich) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM [MA.Berufgruppe] WHERE FachBez_DE In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Fachbereich {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Berufe -------------------------------------------------------------------------------------------------
			'If .Lst_MABerufe.Items.Count > 0 Then
			If Not .Lst_MABerufe.DataSource Is Nothing Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMABerufe)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim oldData = CType(.Lst_MABerufe.DataSource, IEnumerable(Of EmployeeAssignedProfessionData))
				Dim berufCodeString = String.Empty
				sZusatzBez = String.Empty
				For Each itm In oldData
					sZusatzBez = If(String.IsNullOrWhiteSpace(sZusatzBez), "'" & itm.ProfessionText & "'", sZusatzBez & ",'" & itm.ProfessionText & "'")
					berufCodeString = If(String.IsNullOrWhiteSpace(berufCodeString), String.Format("{0}", itm.ProfessionCode), berufCodeString & "," & itm.ProfessionCode)

				Next

				sSql += String.Format("{0}(MA.Beruf {1} ({2}) Or MA.BerufCode {1} ({3}) )", strAndString, If(ExcludeValue, "Not In", "In"), sZusatzBez, berufCodeString)

				FilterBez += String.Format("Qualifikation {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Sonstitge Qualifikation -----------------------------------------------------------------------------------------
			If .Lst_MASonstQualifikation.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMASonstigeQualifikation)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MASonstQualifikation) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_ES_ALs WHERE BerufsText In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Sonstige Qualifikation {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Branchen --------------------------------------------------------------------------------------------------------
			If .Lst_MABranchen.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMABranchen)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MABranchen) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_Branche WHERE Bezeichnung In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Branchen {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Kommunikationsart -----------------------------------------------------------------------------------------------
			If .Lst_MAKommunikationsart.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMAKommunikationsart)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MAKommunikationsart) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_Kommunikation WHERE Bezeichnung In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Kommunikationsart {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Gewünschte Anstellungsart ---------------------------------------------------------------------------------------
			If .Lst_MAAnstellungsart.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMAAnstellung)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MAAnstellungsart) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_Anstellung WHERE Bezeichnung In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Anstellungsart {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Beurteilung -----------------------------------------------------------------------------------------------------
			If .Lst_MABeurteilung.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMABeurteilung)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MABeurteilung) ' .Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_Beurteilung WHERE Bezeichnung In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Beurteilung {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Mündliche Sprachen ----------------------------------------------------------------------------------------------
			If .Lst_MAMSprachen.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMAMSprachen)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MAMSprachen) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_MSprachen WHERE Bezeichnung In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Mündliche Sprachen {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If

			' Schriftlichen Sprachen ------------------------------------------------------------------------------------------
			If .Lst_MASSprachen.Items.Count > 0 Then
				ExcludeValue = .ExcludeSelectedLSTValue(.btnHideMASSprachen)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = GetLstItems(.Lst_MASSprachen) '.Replace("#@", "','").Replace("'", "''").Trim

				sSql &= String.Format("{0}{1} EXISTS(SELECT TOP 1 MANr FROM MA_SSprachen WHERE Bezeichnung In ({2}) AND MANr = MA.MANR)", strAndString, If(ExcludeValue, "NOT", ""), sZusatzBez)

				FilterBez += String.Format("Schriftliche Sprachen {2} ({1}){0}", vbNewLine, sZusatzBez, If(ExcludeValue, "NICHT in", "in"))
			End If



			' 4. Seite === Kontakt und Lohndaten ==============================================================================
			' =================================================================================================================

			' AHV-Karte abgegeben ---------------------------------------------------------------------------------------------
			If .Cbo_MAAHVabgegeben.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAAHVabgegeben.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MAKK.GetAHVKarte = {1}", strAndString, cv.Value)
				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}", .lblAHVKarteAbgegeben.Text, cv.Text, vbLf)
			End If

			' AHV-Karte retourniert -------------------------------------------------------------------------------------------
			If .Cbo_MAAHVretourniert.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAAHVretourniert.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MAKK.AHVKarteBacked = {1}", strAndString, cv.Value)
				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}",
				 .lblAHVKarteRetourniert.Text, cv.Text, vbLf)
			End If

			' Zwischenverdienst -----------------------------------------------------------------------------------------------
			If .Cbo_MAZwischenverdienst.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAZwischenverdienst.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MAKK.InZv = {1}", strAndString, cv.Value)
				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}", .lblZwischenverdienst.Text, cv.Text, vbLf)
			End If

			' ALK-Kassen -----------------------------------------------------------------------------------------------
			If .cboALKKasse.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cboALKKasse.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MAKK.ALKNumber = {1}", strAndString, cv.Value)
				FilterBez += String.Format("Mitarbeiter mit ALK-Nummer = {0}{1}", cv.Text, vbLf)
			End If

			' Rahmenarbeitsvertrag --------------------------------------------------------------------------------------------
			If .Cbo_MARahmenarbeitsvertrag.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MARahmenarbeitsvertrag.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MAKK.RahmenArbeit = {1}", strAndString, cv.Value)
				FilterBez += String.Format("Mitarbeiter mit {0} = {1}{2}", .lblRahmenArbeitsVer.Text, cv.Text, vbLf)
			End If

			' Arbeitspensum ---------------------------------------------------------------------------------------------------
			If .Cbo_MAArbeitspensum.Text.Length > 0 Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_MAArbeitspensum)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAArbeitspensum.Text

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next

				sSql += String.Format("{0}MAKK.Arbeitspensum {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), strMyName.Trim.Replace(",", "','"))
				FilterBez += String.Format("Kandidaten-Arbeitspensum {1} ({2}){0}", vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)

			End If

			' Kündigungsfristen -----------------------------------------------------------------------------------------------
			If .Cbo_MAKuendigungsfristen.Text <> String.Empty Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_MAKuendigungsfristen)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAKuendigungsfristen.Text

				strMyName = String.Empty
				For Each bez As String In sZusatzBez.Split(CChar(","))
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & bez.ToString.Trim
				Next

				sSql += String.Format("{0}MAKK.KundFristen {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), strMyName.Trim.Replace(",", "','"))
				FilterBez += String.Format("Kandidaten-Kündigungsfristen {1} ({2}){0}", vbNewLine, If(ExcludeValue, "NICHT in", "in"), sZusatzBez)
			End If

			' Im Web veröffentlichen ------------------------------------------------------------------------------------------
			If .Cbo_MAImWeb.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAImWeb.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}(MAKK.WebExport = 0 Or MAKK.WebExport Is Null)", strAndString)
				Else
					sSql += String.Format("{0}MAKK.WebExport = 1", strAndString)
				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblImWeb.Text, cv.Text, vbLf)
			End If

			' AGB für WOS -----------------------------------------------------------------------------------------------
			If .Cbo_MAAGBfürWOS.Text <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}MAKK.AGB_WOS = '{1}'", strAndString, .Cbo_MAAGBfürWOS.Text.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblMAAGBfürWOS.Text, .Cbo_MAAGBfürWOS.Text, vbLf)
			End If


			' TAB-PAGES =======================================================================================================
			' =================================================================================================================

			' AUSZAHLUNG
			' =================================================================================================================

			' Währung ---------------------------------------------------------------------------------------------------------
			If .Cbo_MAWaehrung.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sZusatzBez = .Cbo_MAWaehrung.Text

				sSql += String.Format("{0}MA_LOSetting.Currency = '{1}'", strAndString, sZusatzBez.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblWaehrung.Text, sZusatzBez, vbLf)
			End If

			' Rapporte ausdrucken ---------------------------------------------------------------------------------------------
			If .Cbo_MARapporteAusdrucken.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MARapporteAusdrucken.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					sSql += String.Format("{0}(MA_LOSetting.NoRPPrint = 0 Or MA_LOSetting.NoRPPrint Is Null) ", strAndString)
				Else
					sSql += String.Format("{0}MA_LOSetting.NoRPPrint = 1 ", strAndString)
				End If

				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblRapporteAusdrucken.Text, cv.Text, vbLf)
			End If

			' Lohn sperren ----------------------------------------------------------------------------------------------------
			If .Cbo_MALohnSperren.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MALohnSperren.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.NoLO = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblLohnSperren.Text, cv.Text, vbLf)
			End If

			' Vorschuss sperren -----------------------------------------------------------------------------------------------
			If .Cbo_MAVorschussSperren.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAVorschussSperren.SelectedItem, ComboBoxItem)
				If cv Is Nothing Then
					sSql += String.Format("{0}MA_LOSetting.NoZG = {1}", strAndString, cv.Value)
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
					 .lblVorschussSperren.Text, .Cbo_MAVorschussSperren.Text, vbLf)
				End If
			End If

			' Zahlungsart -----------------------------------------------------------------------------------------------------
			If .Cbo_MAZahlungsart.Text.Length > 0 Then
				ExcludeValue = .ExcludeSelectedComboboxValue(.Cbo_MAZahlungsart)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAZahlungsart.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.Zahlart {1} ('{2}')", strAndString, If(ExcludeValue, "Not In", "In"), cv.Value.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("{1} {2} {3}{0}"), vbNewLine, .lblZahlungsart.Text, If(ExcludeValue, "NICHT in", "in"), cv.Text)
			End If

			' Mandantennummer -------------------------------------------------------------------------------------------------------
			If ClsDataDetail.MDData.MultiMD = 1 Then

				Dim mdnumber As String = ConvListObject2String(Me.mandantNumber, ", ")
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Mandantennummer: ({1}){0}"), vbLf, mdnumber)

				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}MA.MDNr In ({1})", strAndString, mdnumber)
			End If

			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .lueAddressSource.EditValue = 1 Then ' Kandidat
				sSql += String.Format("{0}IsNull(MA.ShowAsApplicant, 0) = 0", strAndString)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Adress-Quelle = Kandidat{0}"), vbNewLine)

			ElseIf .lueAddressSource.EditValue = 2 Then ' Bewerber
				sSql += String.Format("{0}IsNull(MA.ShowAsApplicant, 0) = 1", strAndString)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Adress-Quelle = Bewerber{0}"), vbNewLine)

			Else
			End If


			' Unterschreitungslimite --------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .Cbo_MAUnterschreitungslimiteVorhanden.Text.Length > 0 Then

				' Alle Kandidaten, die Unterschreitungslimite haben oder nicht
				Dim sqlKreditlimite As String = ""
				sqlKreditlimite = "[List MANR Unterschreitungslimite]"
				cv = DirectCast(.Cbo_MAUnterschreitungslimiteVorhanden.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter ohne Unterschreitungslimite {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit Unterschreitungslimite {0}"), vbLf)
				End If
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim cmd As SqlCommand = New SqlCommand(sqlKreditlimite, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "

				End If

				connection.Close()

			End If

			' Unterschreitungslimite unterschritten ----------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .Cbo_MAUnterschreitungslimiteUnter.Text.Length > 0 Then

				' Alle Kandidaten, die eine Unterschreitungslimite haben, oder in der Mandantenverwaltung angegebene globale
				' Limite haben und diese überschritten ist oder nicht.
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKreditlimite As String = "[List MANR Unterschreitungslimite unterschritten]"
				cv = DirectCast(.Cbo_MAUnterschreitungslimiteUnter.SelectedItem, ComboBoxItem)

				If cv.Value = "0" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter Unterschreitungslimite nicht unterschritten {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter Unterschreitungslimite unterschritten {0}"), vbLf)
				End If
				Dim cmd As SqlCommand = New SqlCommand(sqlKreditlimite, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@unterschritten", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Unterschreitungslimite unterschritten {0}"), vbLf)

				End If

				connection.Close()

			End If


			' ABZÜGE
			' =================================================================================================================

			' AHV-Code --------------------------------------------------------------------------------------------------------
			If .Cbo_MAAHVCode.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAAHVCode.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.AHVCode = '{1}'", strAndString, cv.Value.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblAHVCode.Text, cv.Text, vbLf)
			End If

			If .cbo_AHVAnmeldung.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.cbo_AHVAnmeldung.SelectedItem, ComboBoxItem)

				If CInt(cv.Value) = 0 Then
					sSql += String.Format("{0}MA_LOSetting.AHVAnAm Is Null", strAndString)
				Else
					sSql += String.Format("{0}MA_LOSetting.AHVAnAm Is Not Null", strAndString)

				End If
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("AHV-Angemeldet = {1}{2}"), .lblAHVAnmeldung.Text, cv.Text, vbLf)

			End If

			' ALV-Code --------------------------------------------------------------------------------------------------------
			If .Cbo_MAALVCode.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAALVCode.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.ALVCode = '{1}'", strAndString, cv.Value.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblALVCode.Text, cv.Text, vbLf)
			End If

			' BVG-Code --------------------------------------------------------------------------------------------------------
			If .Cbo_MABVGCode.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MABVGCode.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.BVGCode = '{1}'", strAndString, cv.Value.Replace("'", "''").Trim)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblBVGCode.Text, cv.Text, vbLf)
			End If

			' KTG-Pflicht -----------------------------------------------------------------------------------------------------
			If .Cbo_MAKTGpflicht.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAKTGpflicht.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.KTGpflicht = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblKTGpflicht.Text, cv.Text, vbLf)
			End If

			' 2. SUVA ---------------------------------------------------------------------------------------------------------
			If .Cbo_MASuva2.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MASuva2.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.SecSuvaCode = '{1}'", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblSuva2.Text, cv.Text, vbLf)
			End If

			' Kinder vorhanden ------------------------------------------------------------------------------------------------
			If .Cbo_MAKinderVorhanden.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAKinderVorhanden.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.KI = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblKinderVorhanden.Text, cv.Text, vbLf)
			End If

			' RÜCKSTELLUNGEN / GUTHABEN
			' =================================================================================================================

			' Rückstellung Ferien ---------------------------------------------------------------------------------------------
			If .Cbo_MARueckFerien.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MARueckFerien.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.FerienBack = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblRueckFerien.Text, cv.Text, vbLf)
			End If

			' Rückstellung Feiertag -------------------------------------------------------------------------------------------
			If .Cbo_MARueckFeiertag.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MARueckFeiertag.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.FeierBack = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblRueckFeiertag.Text, cv.Text, vbLf)
			End If

			' Rückstellung 13.Lohn --------------------------------------------------------------------------------------------
			If .Cbo_MARueck13Lohn.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MARueck13Lohn.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.Lohn13Back = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"),
				 .lblRueck13Lohn.Text, .Cbo_MARueck13Lohn.Text, vbLf)
			End If

			' Gleitzeit -------------------------------------------------------------------------------------------------------
			If .Cbo_MAGleitzeit.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				cv = DirectCast(.Cbo_MAGleitzeit.SelectedItem, ComboBoxItem)

				sSql += String.Format("{0}MA_LOSetting.MAGleitzeit = {1}", strAndString, cv.Value)
				FilterBez += String.Format(m_xml.GetSafeTranslationValue("Mitarbeiter mit {0} = {1}{2}"), .lblGleitzeit.Text, cv.Text, vbLf)
			End If

			' SONSTIGES
			' =================================================================================================================

			' Bankverbindung --------------------------------------------------------------------------------------------------
			If .Cbo_MABankverbindung.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlBankverbindung As String

				' Alle Kandidaten, die Zahlungsart K und eine Bankverbindung haben oder nicht
				sqlBankverbindung = "[List MANR Bankverbindung]"
				cv = DirectCast(.Cbo_MABankverbindung.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Zahlungsart K und Bankverbindung {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Zahlungsart K ohne Bankverbindung {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlBankverbindung, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)
				cmd.Parameters.AddWithValue("@zahlungsart", "K")

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' Angaben über KiZu -----------------------------------------------------------------------------------------------
			If .Cbo_MAAngabenKiZu.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKiZu As String

				' Alle Kandidaten, die Kinderzulagenangaben gemacht haben oder nicht
				sqlKiZu = "[List MANR Angaben Kinderzulagen]"
				cv = DirectCast(.Cbo_MAAngabenKiZu.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Angaben über Kinderzulagen {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten ohne Angaben über Kinderzulagen {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlKiZu, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' Monatliche Lohnangaben ------------------------------------------------------------------------------------------
			If .Cbo_MAMonatlAngaben.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKiZu As String

				' Alle Kandidaten mit oder ohne monatlichen Lohnangaben
				sqlKiZu = "[List MANR Monatliche Lohnangaben]"
				cv = DirectCast(.Cbo_MAMonatlAngaben.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Monatliche Lohnangaben {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten ohne Monatliche Lohnangaben {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlKiZu, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' Lohnausweis -----------------------------------------------------------------------------------------------------
			If .Cbo_MALohnausweis.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKiZu As String

				' Alle Kandidaten mit oder ohne Lohnausweis-Angaben
				sqlKiZu = "[List MANR Lohnausweis]"
				cv = DirectCast(.Cbo_MALohnausweis.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Lohnausweisangaben {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten ohne Lohnausweisangaben {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlKiZu, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' ADRESSEN --------------------------------------------------------------------------------------------------------
			' Adresse Einsatzvertrag ------------------------------------------------------------------------------------------
			If .Cbo_MAAdresseES.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKiZu As String

				' Alle Kandidaten mit oder ohne Einsatzvertragsadresse
				sqlKiZu = "[List MANR ESAddress]"
				cv = DirectCast(.Cbo_MAAdresseES.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Einsatzvertragsadresse {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten ohne Einsatzvertragsadresse {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlKiZu, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If


			' Adresse Rapportversand ------------------------------------------------------------------------------------------
			If .Cbo_MAAdresseRap.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKiZu As String

				' Alle Kandidaten mit oder ohne Rapportsadresse
				sqlKiZu = "[List MANR RPAddress]"
				cv = DirectCast(.Cbo_MAAdresseRap.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Rapportsadresse {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten ohne Rapportsadresse {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlKiZu, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' Adresse Lohnabrechnung ------------------------------------------------------------------------------------------
			If .Cbo_MAAdresseLohn.Text.Length > 0 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				Dim connection As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
				Dim sqlKiZu As String

				' Alle Kandidaten mit oder ohne Lohnabrechnungsadresse
				sqlKiZu = "[List MANR LOAddress]"
				cv = DirectCast(.Cbo_MAAdresseLohn.SelectedItem, ComboBoxItem)

				If cv.Value = "1" Then
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten mit Lohnabrechnungsadresse {0}"), vbLf)
				Else
					FilterBez += String.Format(m_xml.GetSafeTranslationValue("Kandidaten ohne Lohnabrechnungsadresse {0}"), vbLf)
				End If

				Dim cmd As SqlCommand = New SqlCommand(sqlKiZu, connection)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@filiale", ClsDataDetail.UserData.UserFiliale)
				cmd.Parameters.AddWithValue("@mitOhne", cv.Value)

				connection.Open()

				Dim reader As SqlDataReader = cmd.ExecuteReader()
				If reader.HasRows Then
					sSql += String.Format("{0} MA.MANR In (", strAndString)
					While reader.Read
						sSql += String.Format("{0},", reader("MANR"))
					End While
					sSql = sSql.Remove(sSql.Length - 1, 1)
					sSql += ") "
				End If

				connection.Close()
			End If

			' Standard ========================================================================================================
			' =================================================================================================================
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}Benutzer.USNr = {1}", strAndString, ClsDataDetail.UserData.UserNr)

		End With
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("MASearchWhereQuery: {0}", sSql))

		Return sSql
	End Function

	Function GetSortString(ByVal strMySortbez As String) As String
		Dim strSort As String = " Order by"
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Kandidatennummer
		'1 - Kandidatenname
		'2 - Kandidatenstrasse
		'3 - Kandidaten-Ort
		'4 - Kandidaten-PLZ
		'5 - Geburtsdatum (nach Datum)
		'6 - Geburtstage (nach Tage)
		strName = Regex.Split(strMySortbez, ",")
		strMyName = String.Empty
		For i As Integer = 0 To strName.Length - 1
			Select Case CInt(Val(Left(strName(i).ToString.Trim, 1)))
				Case 0          ' Nach Kandidatennummer
					If Left(strName(i).ToString.Trim, 1) <> "0" Then
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " " & strName(i).ToString.Trim
					Else
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Mitarbeiternummer"

					End If
					strMyName = strMyName.Replace("MA.", "MA")

				Case 1          ' Kandidatenname
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANachname"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"

				Case 2          ' Kandidatenstrasse
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAStrasse"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenplz"

				Case 3          ' Ort
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAOrt"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidaten-Ortschaft"
				Case 4          ' PLZ
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAPLZ"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidaten-Postleitzahl"

				Case 5          ' GebDat
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MAGebDat"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Geburtsdatum"

				Case 6          ' Day(GebDat)
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Day(MAGebDat)"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Geburtstage"

				Case Else       ' Kandidatenname ??? Wird jedoch nie vorkommen
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MANachname, MAOrt"
					strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname und Kandidatenort"

			End Select
		Next i

		'Standard-Sortierung, falls leer
		If strMyName.Trim.Length = 0 Then
			strMyName = " MANachname, MAVorname"
			strSortBez = m_xml.GetSafeTranslationValue("Kandidatenname und Kandidatenvorname")
		End If
		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function

	Function GetLstItems(ByVal lst As DevExpress.XtraEditors.ListBoxControl) As String
		Dim LSTContent As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			LSTContent &= If(LSTContent.Trim.Length > 0, ",", "") & String.Format("'{0}'", lst.Items(i).ToString.Replace("'", "''"))
		Next

		Return LSTContent

	End Function


#End Region

	Sub GetJobNr4Print(ByVal sListArt As Short)
		Dim strModul2print As String = String.Empty

		'Bruttoumsatzliste Rapporte Total
		If sListArt = 0 Then strModul2print = "1.3" ' Druck für Kandidatenliste
		If sListArt = 1 Then strModul2print = "1.3.4" ' Liste der Kandidatenkontakte

		ClsDataDetail.GetModulToPrint = strModul2print
	End Sub

	Public Overloads Shared Function ConvListObject2String(ByVal lst As List(Of Integer), Optional ByVal Seperator As String = ", ") As String
		Dim str As New System.Text.StringBuilder
		For i As Integer = 0 To lst.Count - 1
			str.AppendFormat("{0}{1}", CInt(lst.Item(i)), If(i = lst.Count - 1, "", Seperator))
		Next
		Return str.ToString
	End Function

End Class

