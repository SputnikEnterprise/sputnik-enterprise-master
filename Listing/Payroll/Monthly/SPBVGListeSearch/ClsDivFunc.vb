
Imports System.Data.SqlClient

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath

Imports SPBVGListeSearch.ClsDataDetail
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.DatabaseAccess.PayrollMng

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.DatabaseAccess.Employee
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

#Region "Diverses"

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

	'// Parameter für LV 
	Dim _lvParam As ClsDivFunc.LVParameter
	Public Property GetSearchReader() As ClsDivFunc.LVParameter
		Get
			Return _lvParam
		End Get
		Set(ByVal value As ClsDivFunc.LVParameter)
			_lvParam = value
		End Set
	End Property

	Dim _sqlCmd As SqlCommand
	Public Property SqlCmd() As SqlCommand
		Get
			Return _sqlCmd
		End Get
		Set(ByVal value As SqlCommand)
			_sqlCmd = value
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

	Structure LVParameter
		Public SQLCommandText As String
		Public MDNr As Integer
		Public MANRListe As String
		Public JahrVon As String
		Public JahrBis As String
		Public MonatVon As String
		Public MonatBis As String
		Public TableTarget As String
		Public LOArtenList As String
		Public BetragNullAusblenden As Integer
		Public NurDenErstenES As Integer
	End Structure

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

	' // KrediNr
	Dim _strKrediNr As String
	Public Property GetKrediNr() As String
		Get
			Return _strKrediNr
		End Get
		Set(ByVal value As String)
			_strKrediNr = value
		End Set
	End Property

	' // ESNr
	Dim _strESNr As String
	Public Property GetESNr() As String
		Get
			Return _strESNr
		End Get
		Set(ByVal value As String)
			_strESNr = value
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

	'// KDNr
	Dim _strKDNr As String
	Public Property GetKDNr() As String
		Get
			Return _strKDNr
		End Get
		Set(ByVal value As String)
			_strKDNr = value
		End Set
	End Property

	'// Kundennamen
	Dim _strKDName As String
	Public Property GetKDName() As String
		Get
			Return _strKDName
		End Get
		Set(ByVal value As String)
			_strKDName = value
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

	'// GAV-Beruf
	Dim _strESGAVBeruf As String
	Public Property GetESGAVBeruf() As String
		Get
			Return _strESGAVBeruf
		End Get
		Set(ByVal value As String)
			_strESGAVBeruf = value
		End Set
	End Property

	'// Einsatz als
	Dim _strESEinsatzAls As String
	Public Property GetESEinsatzAls() As String
		Get
			Return _strESEinsatzAls
		End Get
		Set(ByVal value As String)
			_strESEinsatzAls = value
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

	'// Print.LLPrintInDiffColor
	Dim _LLPrintInDiffColor As Boolean
	Public Property LLPrintInDiffColor() As Boolean
		Get
			Return _LLPrintInDiffColor
		End Get
		Set(ByVal value As Boolean)
			_LLPrintInDiffColor = value
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


#Region "Public const"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"

#End Region

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_path As ClsProgPath
	Private m_common As CommonSetting
	Private m_utility As Utilities

	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_DateUtility As SP.Infrastructure.DateAndTimeCalculation.DateAndTimeUtily
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private m_EmployeeData As EmployeeMasterData
	Private m_EmployeeLOSetting As EmployeeLOSettingsData
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	Private m_PayrollSetting As String

	Private m_MandantData As MandantData

	Private m_SearchCriteria As New SearchCriteria


#Region "Contructor"

	Public Sub New(ByVal m_searchcriteria As SearchCriteria)

		m_md = New Mandant
		m_path = New ClsProgPath
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI
		m_DateUtility = New DateAndTimeUtily
		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)

		Me.m_SearchCriteria = m_searchcriteria
		m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitialData.MDData.MDNr)

		m_MandantData = m_PayrollDatabaseAccess.LoadMandantData(Now.Year, m_InitialData.MDData.MDNr)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)

	End Sub

#End Region


	Function IsBVGListForAXA() As Boolean
		Dim m_utility As New Utilities
		Dim result As Boolean
		m_md = New Mandant

		Dim sql As String = "Select Top 1 D.DocName, IsNull(MD.BVG_List, '') As BVG_List, IsNull(MD.BVG_List_Grouped, '') As BVG_List_Grouped "
		sql &= "From DokPrint D, Mandanten MD Where D.JobNr = @JobNr And MD.MDNr = @MDNr And MD.Jahr = @Year"

		Dim jobnrParameter As New SqlClient.SqlParameter("JobNr", m_SearchCriteria.jobnrforprint)
		Dim mdnrParameter As New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr)
		Dim yearParameter As New SqlClient.SqlParameter("Year", m_SearchCriteria.bisjahr)

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(jobnrParameter)
		listOfParams.Add(mdnrParameter)
		listOfParams.Add(yearParameter)

		Dim reader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then
				Dim bvgfieldname = "BVG_List" & If(m_SearchCriteria.jobnrforprint.Contains(".G"), "_Grouped", "")

				While reader.Read()

					If m_utility.SafeGetString(reader, bvgfieldname) <> String.Empty Then
						'If m_utility.SafeGetString(reader, "DocName") <> m_utility.SafeGetString(reader, bvgfieldname) And m_utility.SafeGetString(reader, bvgfieldname) <> String.Empty Then
						If m_utility.SafeGetString(reader, bvgfieldname).ToUpper.Contains("AXA") Then Return True

					Else
						If m_utility.SafeGetString(reader, "DocName").ToUpper.Contains("AXA") Then Return True

					End If

				End While

			End If

			'Return True

		Catch e As Exception
			result = False
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function



#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Die nötigen Parameter für die ListView werden zusammengestellt.
	''' </summary>
	''' <param name="frmTest"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetLVParameter(ByVal frmTest As frmBVGListeSearch) As ClsDivFunc.LVParameter
		Dim LVParam As ClsDivFunc.LVParameter = New ClsDivFunc.LVParameter
		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim cv As ComboValue
		Dim loarten As String = ""

		With frmTest

			' Default-Werte
			LVParam.SQLCommandText = "[Create New Table For BVGListe With Mandant]"
			Dim manrListe As String = .txt_MANR.Text
			Dim jahrVon As String = Date.Now.Year.ToString
			Dim jahrBis As String = Date.Now.Year.ToString
			Dim monatVon As String = "1"
			Dim monatBis As String = "12"
			'LVParam.TableTarget = String.Format("_BVGListe_{0}", ClsDataDetail.m_InitialData.UserData.usernr)
			LVParam.TableTarget = "_BVGListe_0"
			LVParam.BetragNullAusblenden = Convert.ToInt32(.Chk_BVGListeNullBetrag.Checked)

			If .Cbo_Lohnart.Text <> String.Empty Then
				If .Cbo_Lohnart.Text.Contains(",") Then
					loarten = .Cbo_Lohnart.Text
				Else
					If Not TryCast(.Cbo_Lohnart.SelectedItem, ComboValue) Is Nothing Then
						cv = DirectCast(.Cbo_Lohnart.SelectedItem, ComboValue)
						If Not cv Is Nothing Then
							loarten = cv.ComboValue
						End If

					Else
						For Each lonr As String In .Cbo_Lohnart.Text.Split(CChar(","))
							loarten += String.Format("{0},", lonr.Trim)
						Next
						loarten = loarten.Remove(loarten.Length - 1, 1) ' Letzten Komma entfernen

					End If
				End If
			End If

			' MONAT VON
			If .Cbo_MonatVon.Text.Length > 0 Then
				monatVon = .Cbo_MonatVon.Text
			End If
			' MONAT BIS
			If .Cbo_MonatBis.Text.Length > 0 Then
				monatBis = .Cbo_MonatBis.Text
			End If
			' JAHR VON
			If .Cbo_VonJahr.Text.Length > 0 Then
				jahrVon = .Cbo_VonJahr.Text
			End If
			' JAHR BIS
			If .Cbo_BisJahr.Text.Length > 0 Then
				jahrBis = .Cbo_BisJahr.Text
			End If



			' Parameter füllen
			LVParam.MDNr = ClsDataDetail.m_InitialData.MDData.MDNr
			LVParam.MANRListe = manrListe
			LVParam.MonatVon = monatVon
			LVParam.MonatBis = monatBis
			LVParam.JahrVon = jahrVon
			LVParam.JahrBis = jahrBis

			LVParam.MDNr = ClsDataDetail.m_InitialData.MDData.MDNr
			LVParam.MANRListe = manrListe
			LVParam.MonatVon = monatVon
			LVParam.MonatBis = monatBis
			LVParam.JahrVon = jahrVon
			LVParam.JahrBis = jahrBis


			If .Cbo_Lohnart.Text.Length > 0 Then
				LVParam.LOArtenList = loarten ' DirectCast(.Cbo_Lohnart.SelectedItem, ComboValue).ComboValue
			End If

			' TABELLENNAMEN FÜR DEN SELECT UND LL
			ClsDataDetail.LLTabellennamen = LVParam.TableTarget

			' FILTERBEDINGUNGEN für die Anzeige auf der Liste
			ClsDataDetail.GetFilterBez = ""
			If manrListe.Length > 0 Then
				ClsDataDetail.GetFilterBez += String.Format("Kandidaten-Nr.: {0}{1}", manrListe, vbLf)
			End If
			ClsDataDetail.GetFilterBez += String.Format("Vom {0} / {1} bis {2} / {3}{4}", monatVon, jahrVon, monatBis, jahrBis, vbLf)
			If .Cbo_Lohnart.Text.Length > 0 Then
				ClsDataDetail.GetFilterBez += String.Format("Lohnart(en): {0}{1}", .Cbo_Lohnart.Text, vbLf)
			End If
		End With

		Return LVParam
	End Function

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString() As String
		Dim Sql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		'Dim cv As ComboValue

		Try
			conn.Open()
		Catch ex As SqlException
			For Each Err As SqlError In ex.Errors
				m_UtilityUi.ShowErrorDialog(String.Format("Die Verbindung zur Datenbank kann nicht geöffnet werden.{0}{1}", vbNewLine, ex.Message))
			Next
			Throw ex
		End Try

		Dim bIsBVGListForAXA As Boolean = IsBVGListForAXA()
		'bIsBVGListForAXA = True

		Dim MDNr As Integer = ClsDataDetail.m_InitialData.MDData.MDNr
		Dim manrListe As String = m_SearchCriteria.manr
		Dim jahrVon As String = m_SearchCriteria.vonjahr ' Date.Now.Year.ToString
		Dim jahrBis As String = m_SearchCriteria.bisjahr ' Date.Now.Year.ToString
		Dim monatVon As String = m_SearchCriteria.vonmonat ' "1"
		Dim monatBis As String = m_SearchCriteria.bismonat ' "12"
		Dim tabellenName As String = "_BVGListe_{0}"

		If bIsBVGListForAXA Then
			tabellenName = String.Format(tabellenName, m_InitialData.UserData.UserGuid)
		Else
			tabellenName = String.Format(tabellenName, m_InitialData.UserData.UserNr)

		End If
		Dim loarten As String = m_SearchCriteria.lohnarten
		Dim betragNull As Integer = Convert.ToInt32(m_SearchCriteria.deletenull) ' .Chk_BVGListeNullBetrag.Checked)
		Dim nurErstenES As Integer = Convert.ToInt32(m_SearchCriteria.getfirstes) ' .Chk_BVGListeNurErstenES.Checked)

		' TABELLENNAMEN FÜR DEN SELECT UND LL
		ClsDataDetail.LLTabellennamen = tabellenName


		'sSql = String.Format("EXEC [Create New Table For BVGListe With Mandant] {0}, {1}, {2}, {3}, '{4}', '{5}', {6}, {7}, {8}" _
		'										 , MDNr, jahrVon, jahrBis, monatVon, monatBis, tabellenName, loarten, betragNull, nurErstenES)

		If bIsBVGListForAXA Then
			Dim result = UpdateOldCreatedLOrecordsForBVG()
			If Not result Then Return String.Empty

			Sql = "Select "
			Sql &= "sum(BVGDays) As BVGDays, "
			Sql &= "sum(BVGStd) As BVGStd, sum(AHVLohn) As AHVLohn, sum(M_BAS) As M_BAS, sum(M_BTR) As M_BTR, "
			Sql &= "(Select Top 1 min(BVGbegin_) From [{0}] Where MANr = bvg.MANr and (bvgein <> '' Or BVGEin Is Not Null)) As BVGEin, "
			Sql &= "(Select Top 1 max(BVGend_) From [{0}] Where MANr = bvg.MANr) As BVGAus, "

			Sql &= "DateDiff(Day, (Select Top 1 min(BVGBegin_) From [{0}] Where MANr = bvg.MANr and (bvgein <> '' Or BVGEin Is Not Null)), "
			Sql &= "	(Select Top 1 max(BVGEnd_) From [{0}] Where MANr = bvg.MANr) ) + 1  As BVGTAGE, "

			Sql &= "(Select Top 1 FirstESBegin From [{0}] Where MANr = bvg.MANr) As FirstESBegin, "

			Sql &= "(Select Top 1 MANR From [{0}] Where MANr = bvg.MANr) As MANR, "
			Sql &= "(Select Top 1 Nachname From [{0}] Where MANr = bvg.MANr) As Nachname, "
			Sql &= "(Select Top 1 Vorname From [{0}] Where MANr = bvg.MANr) As Vorname, "
			Sql &= "(Select Top 1 GebDat From [{0}] Where MANr = bvg.MANr) As GebDat, "
			Sql &= "(Select Top 1 AHV_Nr From [{0}] Where MANr = bvg.MANr) As AHV_Nr, "
			Sql &= "(Select Top 1 AHV_Nr_New From [{0}] Where MANr = bvg.MANr) As AHV_Nr_New, "
			Sql &= "(Select Top 1 Geschlecht From [{0}] Where MANr = bvg.MANr) As Geschlecht, "
			Sql &= "(Select Top 1 MAStrasse From [{0}] Where MANr = bvg.MANr) As MAStrasse, "
			Sql &= "(Select Top 1 MAPLZ From [{0}] Where MANr = bvg.MANr) As MAPLZ, "
			Sql &= "(Select Top 1 MAOrt From [{0}] Where MANr = bvg.MANr) As MAOrt, "
			Sql &= "(Select Top 1 MAPLZOrt From [{0}] Where MANr = bvg.MANr) As MAPLZOrt, "

			Sql &= "(Select Top 1 MALand From [{0}] Where MANr = bvg.MANr) As MALand, "
			Sql &= "(Select Top 1 Zivilstand From [{0}] Where MANr = bvg.MANr) As Zivilstand, "
			Sql &= "(Select Top 1 Sprache From [{0}] Where MANr = bvg.MANr) As Sprache, "
			Sql &= "(Select Top 1 Kinder From [{0}] Where MANr = bvg.MANr) As Kinder, "
			Sql &= "(Select Top 1 Bewillig From [{0}] Where MANr = bvg.MANr) As Bewillig, "
			Sql &= "(Select Top 1 MACo From [{0}] Where MANr = bvg.MANr) As MACo, "
			Sql &= "(Select Top 1 Postfach From [{0}] Where MANr = bvg.MANr) As Postfach, "
			Sql &= "(Select Top 1 Arbeitspensum From [{0}] Where MANr = bvg.MANr) As Arbeitspensum "

			Sql &= "From [{0}] bvg "
			Sql &= "Group by "
			Sql &= "MANR "
			Sql &= "ORDER BY Nachname, Vorname "

			Sql = String.Format(Sql, tabellenName)

		Else

			Sql = String.Format("SELECT * FROM [{0}] ORDER BY Nachname, Vorname, Jahr, Monat, LANR", tabellenName)

		End If


		Try

			' Haupttabelle mit Rohdaten aus der DB holen
			' SELECT
			Dim cmd As SqlCommand = New SqlCommand("", conn)
			cmd.CommandType = CommandType.Text
			Dim cmdTextHaupt As System.Text.StringBuilder = New System.Text.StringBuilder()

			Dim cmdCreateTable As SqlCommand = New SqlCommand(String.Format("BEGIN TRY DROP TABLE [{0}] END TRY BEGIN CATCH END CATCH ", ClsDataDetail.LLTabellennamen), conn)
			cmdCreateTable.ExecuteNonQuery()

			cmdTextHaupt.Append("SELECT 0 As Row, LOL.MANR, LOL.LANR, LA.LALOText, LOL.Jahr, LOL.LP As Monat, LOL.LONR, ")
			cmdTextHaupt.Append("LOL.M_ANZ, LOL.M_BAS, LOL.M_ANS, LOL.M_BTR, ")
			cmdTextHaupt.Append("Ma.Nachname, ")
			cmdTextHaupt.Append("Ma.Vorname, ")
			cmdTextHaupt.Append("@monatVon As MonatVon, ")
			cmdTextHaupt.Append("@monatBis As MonatBis, ")
			cmdTextHaupt.Append("@jahrVon As JahrVon, ")
			cmdTextHaupt.Append("@jahrBis As JahrBis, ")
			cmdTextHaupt.Append("Ma.GebDat, ")
			cmdTextHaupt.Append("Ma.AHV_Nr, ")
			cmdTextHaupt.Append("Ma.AHV_Nr_New, ")
			cmdTextHaupt.Append("Ma.Geschlecht, ")
			cmdTextHaupt.Append("Ma.Strasse As MAStrasse, ")
			cmdTextHaupt.Append("Ma.PLZ As MAPLZ, ")
			cmdTextHaupt.Append("Ma.Ort As MAOrt, ")
			cmdTextHaupt.Append("(MA.PLZ + ' ' + MA.Ort) As MAPLZOrt, ")
			cmdTextHaupt.Append("Ma.Land As MALand, ")
			cmdTextHaupt.Append("Ma.Zivilstand, ")
			cmdTextHaupt.Append("Ma.Sprache, ")
			cmdTextHaupt.Append("Ma.Kinder, ")
			cmdTextHaupt.Append("Ma.Bewillig, MA.Wohnt_Bei As MACo, MA.Postfach, MAK.Arbeitspensum, ")
			cmdTextHaupt.Append("Convert(Decimal(18,2),0) As BVGStd, ")
			cmdTextHaupt.Append("Convert(Decimal(18,2),0) As AHVLohn, ")
			cmdTextHaupt.Append("'' As FirstESBegin, ")

			cmdTextHaupt.Append("'' As BVGEin, ")
			cmdTextHaupt.Append("'' As BVGAus, ")

			cmdTextHaupt.Append("(SELECT MIN(LO.BVGBegin) FROM lo WHERE lo.mdnr = @MDNr AND lo.manr = lol.manr AND (lo.lp between @monatvon AND @monatbis  AND CONVERT(INT, lo.Jahr) between @jahrvon AND @jahrbis) AND lo.BVGBegin IS NOT null) BVGBegin_, ")
			cmdTextHaupt.Append("(SELECT max(LO.BVGEnd) FROM lo WHERE lo.mdnr = @MDNr AND lo.manr = lol.manr AND (lo.lp between @monatvon AND @monatbis  AND CONVERT(INT, lo.Jahr) between @jahrvon AND @jahrbis) AND lo.BVGEnd IS NOT null) BVGEnd_, ")
			cmdTextHaupt.Append("ISNULL(( SELECT Top 1 (L.M_BTR) FROM LOL L WHERE L.MDNr = @MDNr AND L.MANR = LOL.MANR AND L.LONr = LOL.LONr AND L.LANr = 7590.10), 0) BVGDays ")

			' FROM
			cmdTextHaupt.Append("FROM LOL ")
			cmdTextHaupt.Append("LEFT JOIN Mitarbeiter MA ON ")
			cmdTextHaupt.Append("MA.MANR = LOL.MANR ")
			cmdTextHaupt.Append("LEFT JOIN MAKontakt_Komm MAK ON MAK.MANr = MA.MANr ")
			cmdTextHaupt.Append("INNER JOIN LO ON ")
			cmdTextHaupt.Append("LO.MANR = LOL.MANR And ")
			cmdTextHaupt.Append("LO.LONR = LOL.LONR ")
			cmdTextHaupt.Append("LEFT JOIN LA ON ")
			cmdTextHaupt.Append("LA.LANR = LOL.LANR And ")
			cmdTextHaupt.Append("LA.LAJahr = LOL.Jahr ")

			' WHERE
			cmdTextHaupt.Append("WHERE LOL.MDNr = @MDNr And ")
			If manrListe.Length > 0 Then
				cmdTextHaupt.Append(String.Format("LOL.MANR In ({0}) And ", manrListe))
			End If
			If betragNull = 1 Then
				cmdTextHaupt.Append("LOL.M_BTR <> 0 And ")
			End If
			cmdTextHaupt.Append(String.Format("LOL.LANR IN ({0}) And ", loarten)) ' DirectCast(.Cbo_Lohnart.SelectedItem, ComboValue).ComboValue))
			cmdTextHaupt.Append("((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or ")
			cmdTextHaupt.Append("(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or ")
			cmdTextHaupt.Append("(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) ) ")
			cmdTextHaupt.Append("ORDER BY MA.Nachname, MA.Vorname, LOL.Jahr, LOL.LP, LOL.LANR ")

			cmd.CommandText = cmdTextHaupt.ToString
			cmd.Parameters.AddWithValue("@MDNr", MDNr)
			cmd.Parameters.AddWithValue("@monatVon", Int32.Parse(monatVon))
			cmd.Parameters.AddWithValue("@monatBis", Int32.Parse(monatBis))
			cmd.Parameters.AddWithValue("@jahrVon", Int32.Parse(jahrVon))
			cmd.Parameters.AddWithValue("@jahrBis", Int32.Parse(jahrBis))
			Dim daHaupt As SqlDataAdapter = New SqlDataAdapter(cmd)
			Dim dt As DataTable = New DataTable("Haupttabelle")


			' Eintritts- und Austrittsdatum (Einsatz-Beginn und -Ende)
			' Nur den ersten Einsatz im Jahr ist berücksichtigt.
			Dim cmdES As SqlCommand = New SqlCommand("", conn)
			Dim cmdTextESAb As String = ""
			Dim cmdTextESEnde As String = ""

			If bIsBVGListForAXA Then

			Else

				cmdTextESAb = "Select TOP 1 Convert(Datetime, LO.BVGBegin, 104) BVGBegin From LO Where LO.MDNr = @MDNr And "
				cmdTextESAb += "LO.MANR = @manr And "
				cmdTextESAb += "LO.LP Between Month(@datumVon) and Month(@datumBis) And Jahr = Year(@datumBis) "
				cmdTextESAb += "Order By LO.BVGBegin ASC "

				'cmdTextESEnde = "Select TOP 1 LO.BVGEnd From LO Where LO.MDNr = @MDNr And "
				'cmdTextESEnde += "LO.MANR = @manr And "
				'cmdTextESEnde += "LO.LP Between Month(@datumVon) and Month(@datumBis) And Jahr = Year(@datumBis) "
				'cmdTextESEnde += "Order By LO.BVGEnd ASC "




				'cmdTextESAb = "Select TOP 1 ES.ES_Ab From ES Where ES.MDNr = @MDNr And "
				'cmdTextESAb += "ES.MANR = @manr And "
				'cmdTextESAb += "((ES.ES_Ab <= @datumBis And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon)) Or "
				'cmdTextESAb += " (@nurErstenEinsatz = 1 And (Year(ES.ES_Ab) = Year(@datumVon) Or Year(ES.ES_Ende) = Year(@datumVon)))) "
				'cmdTextESAb += "Order By manr,ES.ES_Ab ASC "


				cmdTextESEnde = "Select Top 1 ES_Ende From ( "
				cmdTextESEnde += "Select IsNull(ES.ES_Ende, Convert(DateTime,'01.01.9999',104)) as ES_Ende From ES Where ES.MDNr = @MDNr And "
				cmdTextESEnde += "ES.MANR = @manr And "
				cmdTextESEnde += "ES.ES_Ab <= @datumBisPlus And (ES.ES_Ende Is Null Or ES.ES_Ende >= @datumVon) "
				cmdTextESEnde += ") as t "
				cmdTextESEnde += "Order By ES_Ende DESC "

			End If




			' Einsatz-Ende müssen um 1 Monat nach der gesuchten Zeitperiode gesucht werden.
			' Wenn der Einsatz unbefristet ist, so muss das Datum 01.01.9999 gesetzt werden, da ein Null-Wert 
			' der kleinste Wert hat und somit sich am falschen Ende der Liste befindet. (Sprich: ganz unten)
			' [Nicht 31.12.9999, da ein Monat addiert werden muss]

			Dim datumVon As DateTime
			Dim datumBis As DateTime
			Dim datumBisPlus As DateTime
			Dim pESMDNr As SqlParameter = New SqlParameter("@MDNr", SqlDbType.Int)
			Dim pESmanrHaupt As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)
			Dim pESdtVon As SqlParameter = New SqlParameter("@datumVon", SqlDbType.DateTime)
			Dim pESdtBis As SqlParameter = New SqlParameter("@datumBis", SqlDbType.DateTime)
			Dim pESdtBisPlus As SqlParameter = New SqlParameter("@datumBisPlus", SqlDbType.DateTime)
			Dim pNurErstenES As SqlParameter = New SqlParameter("@nurErstenEinsatz", SqlDbType.Bit)

			cmdES.Parameters.Add(pESMDNr)
			cmdES.Parameters.Add(pNurErstenES)
			cmdES.Parameters.Add(pESmanrHaupt)
			cmdES.Parameters.Add(pESdtVon)
			cmdES.Parameters.Add(pESdtBis)
			cmdES.Parameters.Add(pESdtBisPlus)
			Dim gefunden As Boolean = False
			Dim neuerKandidat As Boolean = True
			Dim zCounter As Integer = 0
			pESMDNr.Value = ClsDataDetail.m_InitialData.MDData.MDNr
			pNurErstenES.Value = nurErstenES

			' BVG-Abzug der letzten 3 Monate abchecken für Ermittlung des Eintrittsdatum
			' -1 bedeutet, dass es keine Lohnabrechnungen vorhanden sind
			' 0 bedeutet, dass es Lohnabrechnungen gibt, aber kein BVG-Abzug
			' > 0 bedeutet, dass Lohnabrechnungen mit BVG-Abzug vorhanden sind.
			Dim cmdTextBVGVor3Monate As String = ""
			Dim cmdBVGVor3Monate As New SqlCommand
			Dim pBVGVor3MonateManr As New SqlParameter
			Dim pBVGVor3MonateDtVon As New SqlParameter
			Dim cmdTextBVGNach3Monate As String = ""
			Dim cmdBVGNach3Monate As New SqlCommand
			Dim pBVGNach3MonateManr As New SqlParameter
			Dim pBVGNach3MonateDtBis As New SqlParameter

			If Not bIsBVGListForAXA Then ' IsBVGListForAXA() Then

				cmdTextBVGVor3Monate = "SELECT IsNull(Sum(BVGAbzug),-1) As BVGAbzug FROM ("
				cmdTextBVGVor3Monate += "SELECT "
				cmdTextBVGVor3Monate += "(SELECT Count(*) FROM LOL WHERE LOL.MDNr = @MDNr And "
				cmdTextBVGVor3Monate += " LOL.MANR=LO.MANR And "
				cmdTextBVGVor3Monate += " LOL.Jahr = LO.Jahr And "
				cmdTextBVGVor3Monate += " LOL.LP = LO.LP And "
				cmdTextBVGVor3Monate += " LOL.LANR In (7590, 7592, 7596) And "
				cmdTextBVGVor3Monate += " LOL.M_Btr <> 0) As BVGAbzug "
				cmdTextBVGVor3Monate += "FROM LO "
				cmdTextBVGVor3Monate += "WHERE LO.MDNr = @MDNr And "
				cmdTextBVGVor3Monate += "LO.MANR = @manr And "
				cmdTextBVGVor3Monate += "LO.Jahr = Year(@datumVon) And "
				cmdTextBVGVor3Monate += "(LO.Jahr = Year(DateAdd(Month,-1,@datumVon)) And LO.LP = Month(DateAdd(Month,-1,@datumVon)) Or "
				cmdTextBVGVor3Monate += " LO.Jahr = Year(DateAdd(Month,-2,@datumVon)) And LO.LP = Month(DateAdd(Month,-2,@datumVon)) Or "
				cmdTextBVGVor3Monate += " LO.Jahr = Year(DateAdd(Month,-3,@datumVon)) And LO.LP = Month(DateAdd(Month,-3,@datumVon))) "
				cmdTextBVGVor3Monate += ") As T"

				cmdBVGVor3Monate = New SqlCommand(cmdTextBVGVor3Monate, conn)
				cmdBVGVor3Monate.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
				pBVGVor3MonateManr = New SqlParameter("@manr", SqlDbType.Int)
				pBVGVor3MonateDtVon = New SqlParameter("@datumVon", SqlDbType.DateTime)

				cmdBVGVor3Monate.Parameters.Add(pBVGVor3MonateManr)
				cmdBVGVor3Monate.Parameters.Add(pBVGVor3MonateDtVon)


				' BVG-Abzug in den nächsten 3 Monate abchecken für Ermittlung des Austrittsdatum
				' -1 bedeutet, dass es keine Lohnabrechnungen vorhanden sind
				' 0 bedeutet, dass es Lohnabrechnungen gibt, aber kein BVG-Abzug
				' > 0 bedeutet, dass Lohnabrechnungen mit BVG-Abzug vorhanden sind.
				cmdTextBVGNach3Monate = "SELECT IsNull(Sum(BVGAbzug),-1) As BVGAbzug FROM ("
				cmdTextBVGNach3Monate += "SELECT "
				cmdTextBVGNach3Monate += "(SELECT Count(*) FROM LOL WHERE LOL.MDNr = @MDNr And "
				cmdTextBVGNach3Monate += " LOL.MANR=LO.MANR And "
				cmdTextBVGNach3Monate += " LOL.Jahr = LO.Jahr And "
				cmdTextBVGNach3Monate += " LOL.LP = LO.LP And "
				cmdTextBVGNach3Monate += " LOL.LANR In (7590, 7592, 7596) And "
				cmdTextBVGNach3Monate += " LOL.M_Btr <> 0) As BVGAbzug "
				cmdTextBVGNach3Monate += "FROM LO "
				cmdTextBVGNach3Monate += "WHERE LO.MDNr = @MDNr And "
				cmdTextBVGNach3Monate += "LO.MANR = @manr And "
				cmdTextBVGNach3Monate += "LO.Jahr= Year(@datumBis) And "
				cmdTextBVGNach3Monate += "(LO.Jahr = Year(DateAdd(Month,1,@datumBis)) And LO.LP = Month(DateAdd(Month,1,@datumBis)) Or"
				cmdTextBVGNach3Monate += " LO.Jahr = Year(DateAdd(Month,2,@datumBis)) And LO.LP = Month(DateAdd(Month,2,@datumBis)) Or"
				cmdTextBVGNach3Monate += " LO.Jahr = Year(DateAdd(Month,3,@datumBis)) And LO.LP = Month(DateAdd(Month,3,@datumBis)))"
				cmdTextBVGNach3Monate += ") As T"

				cmdBVGNach3Monate = New SqlCommand(cmdTextBVGNach3Monate, conn)

				cmdBVGNach3Monate.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
				pBVGNach3MonateManr = New SqlParameter("@manr", SqlDbType.Int)
				pBVGNach3MonateDtBis = New SqlParameter("@datumBis", SqlDbType.DateTime)

				cmdBVGNach3Monate.Parameters.Add(pBVGNach3MonateManr)
				cmdBVGNach3Monate.Parameters.Add(pBVGNach3MonateDtBis)

			End If

			' Test beginn
			' BVG-Stunden pro Monat
			Dim cmdTextStdAnz As String = "SELECT IsNull(Sum(StdAnz),0) As StdAnz FROM "
			cmdTextStdAnz += "(SELECT IsNull(LOL.M_Btr,0) As StdAnz "
			cmdTextStdAnz += " FROM LOL "
			cmdTextStdAnz += " WHERE LOL.MDNr = @MDNr And "
			cmdTextStdAnz += " LOL.LANr = 7520 And "
			cmdTextStdAnz += " LOL.LP = @monat And "
			cmdTextStdAnz += " LOL.Jahr = @jahr And "
			cmdTextStdAnz += " LOL.MANR = @manr "
			cmdTextStdAnz += ") As T"
			Dim cmdStdAnz As SqlCommand = New SqlCommand(cmdTextStdAnz, conn)
			cmdStdAnz.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			Dim pStdAnzJahr As SqlParameter = New SqlParameter("@jahr", SqlDbType.Int)
			Dim pStdAnzMonat As SqlParameter = New SqlParameter("@monat", SqlDbType.Int)
			Dim pStdAnzManr As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)
			cmdStdAnz.Parameters.Add(pStdAnzJahr)
			cmdStdAnz.Parameters.Add(pStdAnzMonat)
			cmdStdAnz.Parameters.Add(pStdAnzManr)
			Dim daStdAnz As SqlDataAdapter = New SqlDataAdapter(cmdStdAnz)

			' AVH-Lohn pro Monat
			Dim cmdTextAHVLohn As String = "SELECT IsNull(Sum(AHVLohn),0) As AHVLohn FROM "
			cmdTextAHVLohn += "(SELECT IsNull(LOL.M_Btr,0) As AHVLohn "
			cmdTextAHVLohn += " FROM LOL "
			cmdTextAHVLohn += " WHERE LOL.MDNr = @MDNr And "
			cmdTextAHVLohn += " LOL.LANr = 7110 And "
			cmdTextAHVLohn += " LOL.LP = @monat And "
			cmdTextAHVLohn += " LOL.Jahr = @jahr And "
			cmdTextAHVLohn += " LOL.MANR = @manr "
			cmdTextAHVLohn += ") As T"
			Dim cmdAHVLohn As SqlCommand = New SqlCommand(cmdTextAHVLohn, conn)
			cmdAHVLohn.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			Dim pAHVLohnJahr As SqlParameter = New SqlParameter("@jahr", SqlDbType.Int)
			Dim pAHVLohnMonat As SqlParameter = New SqlParameter("@monat", SqlDbType.Int)
			Dim pAHVLohnManr As SqlParameter = New SqlParameter("@manr", SqlDbType.Int)
			cmdAHVLohn.Parameters.Add(pAHVLohnJahr)
			cmdAHVLohn.Parameters.Add(pAHVLohnMonat)
			cmdAHVLohn.Parameters.Add(pAHVLohnManr)
			Dim daBVGLohn As SqlDataAdapter = New SqlDataAdapter(cmdAHVLohn)

			' Test Ende

			' M_ANZ, M_BAS, M_ANS und M_BTR pro Monat und Lohnart
			' Zuerst werden die nötigen Daten in der gesuchten Zeitperiode in einer Tabelle geholt,
			' anschliessend werden diese pro Kandidat summiert
			Dim cmdTextM_XTabelle As String = "SELECT LOL.MANR, LOL.M_ANZ, LOL.M_BAS, LOL.M_ANS, LOL.M_BTR, LOL.Jahr, LOL.LP, LOL.LANR "
			cmdTextM_XTabelle += "FROM dbo.LOL "
			cmdTextM_XTabelle += "WHERE LOL.MDNr = @MDNr And "
			cmdTextM_XTabelle += String.Format("LOL.LANR IN ({0}) And ", loarten)
			cmdTextM_XTabelle += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
			cmdTextM_XTabelle += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
			cmdTextM_XTabelle += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) ) "
			cmdTextM_XTabelle += "GROUP BY LOL.MANR, LOL.M_ANZ, LOL.M_BAS, LOL.M_ANS, LOL.M_BTR, LOL.Jahr, LOL.LP, LOL.LANR"
			Dim cmdM_XTabelle As SqlCommand = New SqlCommand(cmdTextM_XTabelle, conn)

			cmdM_XTabelle.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			cmdM_XTabelle.Parameters.AddWithValue("@jahrVon", jahrVon)
			cmdM_XTabelle.Parameters.AddWithValue("@jahrBis", jahrBis)
			cmdM_XTabelle.Parameters.AddWithValue("@monatVon", monatVon)
			cmdM_XTabelle.Parameters.AddWithValue("@monatBis", monatBis)
			Dim daM_XTabelle As SqlDataAdapter = New SqlDataAdapter(cmdM_XTabelle)
			Dim dtM_X As DataTable = New DataTable("M_XTabelle")
			'M_X-Daten holen...
			daM_XTabelle.Fill(dtM_X)
			' ... und den SELECT für die einzelnen Summen pro Kandidat und Monat werden in der Hauptschleife ermittelt.

			' Haupttabelle füllen
			Dim manr As String = ""
			Dim manrTemp As String = "" ' für nurErstenEinsatz-Routine
			Dim JahrMin As Integer = 1900
			Dim JahrMax As Integer = 2050
			Dim LPMin As Integer = 1
			Dim LPMax As Integer = 1
			Dim row As DataRow
			Dim absolutZeile As Integer = 0
			Dim alternierendeZeile As Boolean = True

			If daHaupt.Fill(dt) > 0 Then
				For i As Integer = 0 To dt.Rows.Count - 1
					row = dt.Rows(i)
					gefunden = False
					manr = row("MANR").ToString ' Kandidat-Manr festhalten


					' Neuen Kandidaten?
					If manr <> manrTemp Then
						manrTemp = manr
						' Wenn nur den ersten Einsatz als Eintritt markiert, pro Kandidat zurückstellen
						pNurErstenES.Value = nurErstenES
						' Alternierende Zeile
						alternierendeZeile = Not alternierendeZeile
					End If

					' EIN- UND AUSTRITT BVG-PFLICHT
					LPMin = Int32.Parse(row("Monat").ToString) ' Erster Monat ist Min
					JahrMin = Int32.Parse(row("Jahr").ToString) ' Erstes Jahr ist Min
					' Max herausfinden
					LPMax = Int32.Parse(row("Monat").ToString) ' Erster Monat könnte auch der letzte sein
					JahrMax = Int32.Parse(row("Jahr").ToString) ' Erstes Jahr könnte auch das letzte sein

					' Suche nach vorne: Nächster Datensatz überprüfen.
					zCounter = 0
					absolutZeile = 0
					While Not gefunden

						zCounter += 1
						If i + zCounter = dt.Rows.Count Then ' Es gibt keine Datensätze mehr
							zCounter -= 1 ' Pointer um eins zurück
							gefunden = True
						ElseIf manr <> dt.Rows(i + zCounter)("MANR").ToString Then ' Nächster Datensatz hat neuen Kandidat --> neuerKandidat
							zCounter -= 1 ' Pointer um eins zurück
							gefunden = True
							neuerKandidat = True
						ElseIf Int32.Parse(dt.Rows(i + zCounter)("Jahr").ToString) > JahrMax Then ' Nächstes Jahr
							If Int32.Parse(dt.Rows(i + zCounter)("Monat").ToString) > LPMax + 3 - 12 Then
								' Drei Monate werden übersprungen im nächsten Jahr --> Min/Max gefunden
								zCounter -= 1 ' Pointer um eins zurück
								gefunden = True
							End If
						ElseIf Int32.Parse(dt.Rows(i + zCounter)("Monat").ToString) > LPMax + 3 Then
							' Drei Monate werden übersprungen --> Min/Max gefunden
							zCounter -= 1 ' Pointer um eins zurück
							gefunden = True
						Else
							' Max vom nächsten Datensatz zuweisen
							LPMax = Int32.Parse(dt.Rows(i + zCounter)("Monat").ToString)
							JahrMax = Int32.Parse(dt.Rows(i + zCounter)("Jahr").ToString)
						End If

						'Alternierende Zeile
						dt.Rows(i + absolutZeile)("Row") = Convert.ToByte(alternierendeZeile)

						' BVG-Stunden pro Monat
						pStdAnzMonat.Value = dt.Rows(i + absolutZeile)("Monat")
						pStdAnzJahr.Value = dt.Rows(i + absolutZeile)("Jahr")
						pStdAnzManr.Value = manr
						dt.Rows(i + absolutZeile)("BVGStd") = cmdStdAnz.ExecuteScalar()

						' AHV-Lohn pro Monat
						pAHVLohnMonat.Value = dt.Rows(i + absolutZeile)("Monat")
						pAHVLohnJahr.Value = dt.Rows(i + absolutZeile)("Jahr")
						pAHVLohnManr.Value = manr


						dt.Rows(i + absolutZeile)("AHVLohn") = cmdAHVLohn.ExecuteScalar()

						absolutZeile += 1

					End While

					' EINTRITT UND AUSTRITT BVG-PFLICHT ========================================================================

					' EINTRITTSDATUM (EINSATZ-BEGINN) -----------------------------
					Dim esBeginObj As Object
					Dim esEndeObj As Object

					esBeginObj = dt.Rows(0)("bvgbegin_")
					esEndeObj = dt.Rows(0)("bvgEnd_")

					If bIsBVGListForAXA Then 'IsBVGListForAXA() Then

						esBeginObj = dt.Rows(0)("bvgbegin_")
						esEndeObj = dt.Rows(0)("bvgEnd_")
					Else

						' Wenn Min und Max eines Kunden gefunden, so die Einsatzdaten holen
						datumVon = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, JahrMin))
						datumBis = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, JahrMax)).AddMonths(1).AddDays(-1)
						'esBeginObj = m_DateUtility.MaxDate(dt.Rows(0)("bvgbegin_"), datumVon)
						'esEndeObj = m_DateUtility.MinDate(dt.Rows(0)("bvgEnd_"), datumBis)

						' Das DatumPlus (+ 4 Monate = 3 Monate im Vorraus + 1 )
						datumBisPlus = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, JahrMax)).AddMonths(4).AddDays(-1)

						' Parameter für Einsatz-Beginn und -Ende füllen
						pESmanrHaupt.Value = manr
						pESdtVon.Value = datumVon
						pESdtBis.Value = datumBis
						pESdtBisPlus.Value = datumBisPlus

						cmdES.CommandText = cmdTextESAb
						esBeginObj = cmdES.ExecuteScalar() ' Einsatzbeginn der gesuchten Zeitperiode

						' Lohnabrechnung ohne Einsatz während gesuchten Monats --> Das Von-Datum als Eintrittsdatum setzen
						If esBeginObj Is Nothing OrElse esBeginObj Is DBNull.Value Then
							esBeginObj = datumVon
						End If

						' Eintrittsdatum initialisieren
						Dim eintrittDt As DateTime = DirectCast(esBeginObj, DateTime)

						' Da nach Zeitperiode gesucht wird, so kann es sein, dass der erste Monat kein Einsatz hat, aber
						' die folgenden Monate doch. Der Einsatz beginnt in anderen Worte später als die Lohnabrechnungen.
						If Year(eintrittDt) > Int32.Parse(jahrVon) Or Year(eintrittDt) = Int32.Parse(jahrVon) And Month(eintrittDt) > LPMin Then
							' Das Von-Datum als Eintrittsdatum setzen
							eintrittDt = datumVon
						End If

						' BVG-Abzug in den letzten 3 Monaten davor prüfen
						pBVGVor3MonateManr.Value = manr
						pBVGVor3MonateDtVon.Value = datumVon
						esBeginObj = cmdBVGVor3Monate.ExecuteScalar()

						' Wenn der erste Einsatz im Jahr markiert
						If nurErstenES = 1 Then
							' Wenn der erste Einsatz nach Lohnabrechnungen anfängt, so ersten Monat der Zeitperiode
							If Year(eintrittDt) <> JahrMin And Month(eintrittDt) > LPMin Then
								eintrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, JahrMin))
							End If
							' ACHTUNG: Nur im ersten Lauf Checkbox "Nur der erste Einsatz im Jahr" berücksichtigen.
							pNurErstenES.Value = 0

						Else
							dt.Rows(i)("FirstESBegin") = eintrittDt.ToShortDateString

							' Es gibt in den letzten 3 Monaten mindestens eine Lohnabrechnung

							If CInt(esBeginObj) > -1 Then
								' ...mit BVG-Abzug
								If CInt(esBeginObj) > 0 Then
									eintrittDt = DateTime.Parse("01.01.1900") ' kein Eintrittsdatum
								Else ' ...ohne BVG-Abzug
									' Wenn kein Einsatz vorhanden, oder früher oder später anfängt, so Monats-Beginn-Datum (LPMin)
									If Year(eintrittDt) <> JahrMin Or Month(eintrittDt) <> LPMin Then
										eintrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, JahrMin))
									End If
								End If
							Else ' Es gibt in den letzten 3 Monate keine Lohnabrechnung
								' Eintrittsdatum angeben
								' Wenn kein Einsatz vorhanden, oder früher oder später anfängt, so Monats-Beginn-Datum (LPMin)
								If Year(eintrittDt).ToString = jahrVon And Month(eintrittDt) <> LPMin Then
									eintrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMin, jahrVon))
								End If

							End If
						End If

						If Year(eintrittDt) > 1900 Then
							dt.Rows(i)("BVGEin") = eintrittDt.ToShortDateString
						End If



						' AUSTRITTSDATUM (EINSATZ-ENDE) -----------------------
						'  unbefristeter Einsatz = 01.01.9999 (statt DbNull) [Nicht 31.12.9999 nehmen, da ein Monat addiert wird.]
						' Testfall



						cmdES.CommandText = cmdTextESEnde
						esEndeObj = cmdES.ExecuteScalar() ' Einsatz-Ende der gesuchten Zeitperiode

						'cmdES.CommandText = cmdTextESEnde
						'Dim esEndeObj As Object = cmdES.ExecuteScalar()	' Einsatz-Ende der gesuchten Zeitperiode

						' Gibt es im Nachmonat eine Lohnabrechnung 
						'     mit Quellensteuer-Abzug so kein Austrittsdatum
						'     ohne Quellensteuer-Abzug so Austrittsdatum
						' Gibt es im Nachmonat keine Lohnabrechnung aber der Einsatz geht weiter 
						'     mit Einsatz so kein Austrittsdatum
						'     ohne Einsatz so Austrittsdatum 
						' AUSNAHME bildet der Monat 12: Hier wird stets ein Austritt angegeben. Entweder
						'   das Einsatz-Ende-Datum, falls ein Einsatz dann endet, oder Monatsende, falls 
						'   weitere Einsätze im nächsten Jahr vorhanden sind.
						pBVGNach3MonateManr.Value = manr
						pBVGNach3MonateDtBis.Value = datumBis
						Dim BVGNachMonatObj As Object = cmdBVGNach3Monate.ExecuteScalar()

						' Austrittsdatum mit letzen Monat initialisieren
						Dim austrittDt As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, JahrMax)).AddMonths(1).AddDays(-1)
						' Wenn Einsatz-Ende-Datum vorhanden
						If Not esEndeObj Is Nothing Then
							Dim einsatzEndeDt As DateTime = DirectCast(esEndeObj, DateTime)
							' Das Einsatz-Ende-Datum nur nehmen, wenn im gleichen Monat wie die letzte bvgflichtige Lohnabrechnung endet.
							' Oder der Einsatz unbefristet ist.
							If einsatzEndeDt.Year = austrittDt.Year And einsatzEndeDt.Month = austrittDt.Month Or einsatzEndeDt.Year = 9999 Then
								austrittDt = DirectCast(esEndeObj, DateTime)
							End If
						End If

						' Lohnabrechnung in den nächsten 3 Monate vorhanden
						If CInt(BVGNachMonatObj) > -1 Then
							' ...mit BVG-Abzug
							If CInt(BVGNachMonatObj) > 0 Then
								austrittDt = DateTime.Parse("01.01.9999") ' Kein Austrittsdatum (gleich wie unbefristeter Einsatz)
							Else ' ...ohne BVG-Abzug
								' Wenn der Einsatz-Ende nicht im gleichen Monat endet, so Monatsende von LPMax nehmen
								If austrittDt.Year <> JahrMax Or austrittDt.Month <> LPMax Then
									austrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, JahrMax)).AddMonths(1).AddDays(-1)
								End If
							End If
						Else ' Keine Lohnabrechnung
							' Gibt es einen Einsatz
							If Not esEndeObj Is Nothing Then
								' Wenn der Einsatz-Ende vor dem letzten Monat endet, so Monatsende von LPMax nehmen
								If austrittDt.Year = JahrMax And austrittDt.Month < LPMax Or austrittDt.Year < JahrMax Then
									austrittDt = DateTime.Parse(String.Format("01.{0}.{1}", LPMax, JahrMax)).AddMonths(1).AddDays(-1)
								End If
								' Endet der Einsatz nach dem letzten BVG-Pflichtigen Monat, so kein Austrittsdatum
								Dim einsatzEndeDt As DateTime = DirectCast(esEndeObj, DateTime)
								If einsatzEndeDt > datumBis Then
									austrittDt = DateTime.Parse("01.01.9999") ' Kein Austrittsdatum (gleich wie unbefristeter Einsatz)
								End If
							End If
						End If

						If austrittDt.Year < 3000 Then
							dt.Rows(i + zCounter)("BVGAus") = austrittDt.ToShortDateString
						Else
							If bIsBVGListForAXA Then
								Dim endofbismonth = m_DateUtility.GetLastDayOfMonth(CInt(m_SearchCriteria.bisjahr), CInt(m_SearchCriteria.bismonat))
								dt.Rows(i + zCounter)("BVGAus") = endofbismonth.ToShortDateString
							End If

						End If

					End If

					' Um so viele Datensätze vorrücken, wie vorher Anzahl Male nach vorne gesucht wurde
					i += zCounter

				Next
				'End If

				ClsDataDetail.QSTListeDataTable = dt

				' Eine bestehende Tabelle auf der Datenbank löschen
				cmdCreateTable = New SqlCommand(String.Format("BEGIN TRY DROP TABLE [{0}] END TRY BEGIN CATCH END CATCH ", ClsDataDetail.LLTabellennamen), conn)
				cmdCreateTable.ExecuteNonQuery()

				' Die erstellte Tabelle auf die Datenbank erzeugen
				cmdCreateTable.CommandText = String.Format("CREATE TABLE [{0}] (", ClsDataDetail.LLTabellennamen)
				For Each col As DataColumn In dt.Columns
					If col.DataType.Name = "Decimal" Then
						cmdCreateTable.CommandText += String.Format(" {0} {1}(18,2),", col.ColumnName, col.DataType.Name)
					Else
						cmdCreateTable.CommandText += String.Format(" {0} {1},", col.ColumnName, col.DataType.Name)
					End If
				Next
				cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 1, 1) ' letztes Komma entfernen
				cmdCreateTable.CommandText += " )"
				cmdCreateTable.CommandText = cmdCreateTable.CommandText.Replace("Int32", "Int").Replace("Int16", "Int").Replace("String", "nvarchar(255)")
				cmdCreateTable.ExecuteNonQuery()

				' Die erzeugte Tabelle mit der erstellten Tabelle füllen
				cmdCreateTable.CommandText = String.Format("INSERT INTO [{0}] VALUES (", ClsDataDetail.LLTabellennamen)
				For Each col As DataColumn In dt.Columns
					Dim typeObj As Object = SqlDbType.Int
					Select Case col.DataType.Name.ToUpper
						Case "String".ToUpper
							typeObj = SqlDbType.NVarChar
						Case "DateTime".ToUpper
							typeObj = SqlDbType.DateTime
						Case "Decimal".ToUpper
							typeObj = SqlDbType.Decimal
					End Select
					' CommandText ergänzen
					cmdCreateTable.CommandText += String.Format("@{0}, ", col.ColumnName)
					'Parameter hinzufügen
					Dim p As SqlParameter = New SqlParameter(String.Format("@{0}", col.ColumnName), DirectCast(typeObj, SqlDbType))
					cmdCreateTable.Parameters.Add(p)

				Next
				cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 2, 2) ' letztes Komma entfernen
				cmdCreateTable.CommandText += ")"

				' Jeden Datensatz auf der Datenbank übertragen
				For Each rowToInsert As DataRow In dt.Rows
					' Parameter füllen
					For Each p As SqlParameter In cmdCreateTable.Parameters
						If p.SqlDbType.ToString.ToUpper = "NVARCHAR" Then
							p.Value = rowToInsert(p.ParameterName.Replace("@", "")).ToString
						Else
							p.Value = rowToInsert(p.ParameterName.Replace("@", ""))
						End If
						'Trace.WriteLine(String.Format("{0}: {1}", p.ParameterName, p.Value))

					Next
					' Zeile schreiben
					cmdCreateTable.ExecuteNonQuery()
				Next

				conn.Close()

				' FILTERBEDINGUNGEN für die Anzeige auf der Liste
				ClsDataDetail.GetFilterBez = ""
				If manrListe.Length > 0 Then
					ClsDataDetail.GetFilterBez += String.Format("Kandidaten-Nr.: {0}{1}", manrListe, vbLf)
				End If
				ClsDataDetail.GetFilterBez += String.Format("Vom {0} / {1} bis {2} / {3}{4}", monatVon, jahrVon, monatBis, jahrBis, vbLf)
				'If .Cbo_BVGListeLohnart.Text.Length > 0 Then
				'  ClsDataDetail.GetFilterBez += String.Format("Lohnart(en): {0}{1}", .Cbo_BVGListeLohnart.Text, vbLf)
				'End If

			End If

		Catch ex As Exception
			SplashScreenManager.CloseForm(False)
			m_UtilityUi.ShowErrorDialog(ex.ToString)
			m_Logger.LogError(ex.ToString)

			Sql = String.Empty

		End Try


		Return Sql
	End Function


#End Region


	Function UpdateOldCreatedLOrecordsForBVG() As Boolean
		Dim result As Boolean = True
		Dim sql As String

		sql = "Select LO.LONr, LO.MANr, Convert(INT, LO.LP) LP, Convert(INT, LO.Jahr) Jahr, BVGBegin, BVGEnd From LO "
		sql &= "Where LO.MDNr = @MDNr "
		sql &= "AND dbo.DateTimeFromYearMonthDay(LO.Jahr, LO.LP, 1) >= dbo.DateTimeFromYearMonthDay(@year, @month, 1) "
		sql &= "AND dbo.DateTimeFromYearMonthDay(LO.Jahr, LO.LP, 1) <= dbo.DateTimeFromYearMonthDay(@yearbis, @monthbis + 1, 1)-1 "
		sql &= "AND LO.LONr NOT IN (SELECT TOP 1 LOL.LONR FROM LOL WHERE LOL.MDNr = LO.MDNr AND LOL.LONr = LO.LONr AND LOL.MANr = LO.MANr AND LOL.LANr IN (7590) AND M_BTR = 0 ) "
		sql &= "AND LO.LONr NOT IN (SELECT TOP 1 LOL.LONR FROM LOL WHERE LOL.MDNr = LO.MDNr AND LOL.LONr = LO.LONr AND LOL.MANr = LO.MANr AND LOL.LANr IN (7590.10) ) "
		sql &= "AND LO.MANr NOT IN (SELECT TOP 1 MAL.MANr FROM dbo.MA_LOSetting MAL WHERE MAL.MANr = LO.MANr AND MAL.BVGCode = '0') "

		sql &= "Order By LO.MANr, LO.Jahr, LO.LP"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@mdNr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("@month", Val(m_SearchCriteria.vonmonat)))
		listOfParams.Add(New SqlClient.SqlParameter("@monthbis", If(Val(m_SearchCriteria.bismonat) = 12, 0, Val(m_SearchCriteria.bismonat))))
		listOfParams.Add(New SqlClient.SqlParameter("@year", Val(m_SearchCriteria.vonjahr)))
		listOfParams.Add(New SqlClient.SqlParameter("@yearbis", If(Val(m_SearchCriteria.bismonat) = 12, Val(m_SearchCriteria.bisjahr) + 1, Val(m_SearchCriteria.bisjahr))))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		If (Not reader Is Nothing) Then

			While reader.Read
				Dim employeeNumber As Integer
				Dim payrollnumber As Integer
				Dim monat As Integer
				Dim jahr As Integer
				Dim bvgBegin As Date?
				Dim bvgEnd As Date?

				employeeNumber = m_utility.SafeGetInteger(reader, "MANr", 0)
				payrollnumber = m_utility.SafeGetInteger(reader, "LONr", 0)
				monat = m_utility.SafeGetInteger(reader, "LP", 0)
				jahr = m_utility.SafeGetInteger(reader, "jahr", 0)
				bvgBegin = m_utility.SafeGetDateTime(reader, "BVGBegin", Nothing)
				bvgEnd = m_utility.SafeGetDateTime(reader, "BVGEnd", Nothing)

				m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
				m_EmployeeLOSetting = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(employeeNumber)

				If employeeNumber = 1376110 Then
					Trace.WriteLine(employeeNumber)
				End If

				Get_ES_Std_Total_New(m_EmployeeData.EmployeeNumber, monat, jahr)

				If bvgBegin <> BVGBeginForLO OrElse bvgEnd <> BVGEndForLO Then
					Trace.WriteLine(String.Format("{10}Kandidat: {0} | Zeit: {1}/{2} >>> WorkTime: {3:f2} |BVGWorkTime: {4:f2} | BVGWorkDays: {5:f2} | BVG-Daten: {6:d} - {7:d} | Neue BVG-Daten: {8:d} - {9:d}",
																				m_EmployeeData.EmployeeNumber, monat, jahr, WorkTime, BVGWorkTime, BVGWorkDays,
																				bvgBegin, bvgEnd,
																				BVGBeginForLO, BVGEndForLO,
																				If(bvgBegin <> BVGBeginForLO OrElse bvgEnd <> BVGEndForLO, "********** ", "")
																				))
				End If

				result = result AndAlso AddBVGLAToLOL(payrollnumber)

				If Not result Then Return result

			End While

		End If


		Return result

	End Function


	Function AddBVGLAToLOL(ByVal payrollNumber As Integer) As Boolean
		Dim success As Boolean = True
		Dim sql As String

		sql = "Delete LOL Where LANr In (7950.10) "
		sql &= "And MDNr = @MDNr "
		sql &= "And Jahr = (Select Top 1 Jahr From LO Where LONr = @payrollNumber) "
		sql &= "And LP = (Select Top 1 LP From LO Where LONr = @payrollNumber) "
		sql &= "And LONr = (Select Top 1 LONr From LO Where LONr = @payrollNumber) "
		sql &= "And MANr = (Select Top 1 MANr From LO Where LONr = @payrollNumber); "

		sql &= "Insert Into LOL ("

		sql &= "LONr, "
		sql &= "MANr, "
		sql &= "LANr, "
		sql &= "LP, "
		sql &= "Jahr, "
		sql &= "Modulname, "
		sql &= "Currency, "
		sql &= "M_Anz, "
		sql &= "M_Bas, "
		sql &= "M_Ans, "
		sql &= "M_Btr, "
		sql &= "RPText, "
		sql &= "AGLA, "
		sql &= "S_Kanton, "
		sql &= "LMWithDTA, "
		sql &= "ZGGrund, "
		sql &= "BnkNr, "
		sql &= "MDNr"
		sql &= ") "

		sql &= "Values ("
		sql &= "(Select Top 1 LONr From LO Where LONr = @payrollNumber), "
		sql &= "(Select Top 1 MANr From LO Where LONr = @payrollNumber), "
		sql &= "7590.10, "
		sql &= "(Select Top 1 LP From LO Where LONr = @payrollNumber), "
		sql &= "(Select Top 1 Jahr From LO Where LONr = @payrollNumber), "
		sql &= "'A', "
		sql &= "'CHF', "
		sql &= "1, "
		sql &= "@M_Bas, "
		sql &= "100, "
		sql &= "@M_Btr, "
		sql &= "(Select Top 1 LALOText From LA Where LANr = 7590.10 And LAJahr = (Select Top 1 Jahr From LO Where LONr = @payrollNumber) ), "
		sql &= "0, "
		sql &= "'', "
		sql &= "0, "
		sql &= "'', "
		sql &= "0, "
		sql &= "@MDNr"
		sql &= "); "

		sql &= "Update LO Set BVGBegin = @BVGBegin, BVGEnd = @BVGEnd, BVGDateData = @BVGDateData Where LONr = @payrollNumber; "

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("mdNr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("payrollNumber", payrollNumber))
		listOfParams.Add(New SqlClient.SqlParameter("M_Bas", m_utility.ReplaceMissing(BVGWorkDays, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("M_Btr", m_utility.ReplaceMissing(BVGWorkDays, DBNull.Value)))

		listOfParams.Add(New SqlClient.SqlParameter("BVGBegin", m_utility.ReplaceMissing(BVGBeginForLO, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("BVGEnd", m_utility.ReplaceMissing(BVGEndForLO, DBNull.Value)))

		Dim bvgdate As String = String.Empty
		For Each itm In bvgDatesData
			bvgdate &= String.Format("{0:d} - {1:d}{2}", itm.Von, itm.Bis, vbNewLine)
		Next
		listOfParams.Add(New SqlClient.SqlParameter("BVGDateData", m_utility.ReplaceMissing(bvgdate, DBNull.Value)))

		success = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)


		Return success

	End Function

End Class






Partial Public Class ClsDbFunc


#Region "public properties"

	Public Property LoggingMessage As String
	Public Property WorkTime As Decimal
	Public Property BVGWorkTime As Decimal
	Public Property WorkDaysCurrentMonth As Integer
	Public Property BVGWorkDays As Integer
	Public Property BVGBeginForLO As Date?
	Public Property BVGEndForLO As Date?
	Public Property bvgDatesData As New List(Of BVGDayData)

	Public ReadOnly Property StartofCurrentPayrollMonth(ByVal lpMonth As Integer, ByVal lpYear As Integer) As Date
		Get
			Return CDate("01." & lpMonth & "." & lpYear)
		End Get
	End Property

	Public ReadOnly Property EndofCurrentPayrollMonth(ByVal lpMonth As Integer, ByVal lpYear As Integer) As Date
		Get
			Return CDate(DateAdd("m", 1, StartofCurrentPayrollMonth(lpMonth, lpYear).AddDays(-StartofCurrentPayrollMonth(lpMonth, lpYear).Day + 1))).AddDays(-1)
		End Get
	End Property

	Public ReadOnly Property StartofCurrentPayrollYear(ByVal lpYear As Integer) As Date
		Get
			Return CDate("01.01." & lpYear)
		End Get
	End Property

	Public ReadOnly Property EndeofCurrentPayrollYear(ByVal lpYear As Integer) As Date
		Get
			Return CDate("31.12." & lpYear)
		End Get
	End Property

#End Region



	Private Function Get_ES_Std_Total_New(ByVal employeeNumber As Integer, ByVal lpMonth As Integer, ByVal lpYear As Integer) As Decimal
		Dim bArbTage As Boolean

		LoggingMessage = String.Empty

		BVGBeginForLO = Nothing
		BVGEndForLO = Nothing
		BVGWorkDays = 0
		BVGWorkTime = 0
		WorkTime = 0
		WorkDaysCurrentMonth = 0
		bvgDatesData = New List(Of BVGDayData)

		Dim WorkedData = m_PayrollDatabaseAccess.LoadESDataForRPStdTotal(employeeNumber, m_InitialData.MDData.MDNr, lpMonth, lpYear)
		ThrowExceptionOnError(WorkedData Is Nothing, "Einsatzstunden Daten konnten nicht geladen werden.")
		WorkTime = WorkedData

		Dim calculatebvgwithesdays As Boolean = StrToBool(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, lpYear), String.Format("{0}/calculatebvgwithesdays", m_PayrollSetting)))
		bArbTage = calculatebvgwithesdays

		If bArbTage Then
			Get_ES_Std_Total_New = Get_ES_Std_Total_New_1(employeeNumber, lpMonth, lpYear)
		Else
			Get_ES_Std_Total_New = Get_ES_Std_Total_New_0(employeeNumber, lpMonth, lpYear)
		End If

	End Function

	Private Function Get_ES_Std_Total_New_0(ByVal employeeNumber As Integer, ByVal lpMonth As Integer, ByVal lpYear As Integer) As Decimal
		Dim iAnzAlteDauer As Integer
		Dim dateUtility As New DateAndTimeUtily

		Dim StartofMonth As Date = StartofCurrentPayrollMonth(lpMonth, lpYear)
		Dim EndofMonth As Date = EndofCurrentPayrollMonth(lpMonth, lpYear)
		Dim dOldRPVon As Date
		Dim dOldRPBis As Date
		Dim cBVGStd As Decimal
		Dim iBVGAfter As Integer
		Dim iESBreakWeek As Integer
		Dim bvgDates As New List(Of BVGDayData)

		Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, lpYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, lpYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
		m_EmployeeLOSetting = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(employeeNumber)

		bvginterval = "ww"
		iBVGAfter = Val(bvgintervaladd)
		If iBVGAfter = 0 Then iBVGAfter = 13 ' nach 13 Wochen

		iESBreakWeek = Math.Max(Val(m_MandantData.BVG_Aus1Woche), 2)
		Dim msg As String = String.Format("| BVGCode: {0}", m_EmployeeLOSetting.BVGCode)
		Dim bvgDayMessage As String = String.Empty
		msg &= String.Format("| bvginterval: {0} ", bvginterval)
		msg &= String.Format("| iBVGAfter: {0} ", iBVGAfter)

		LoggingMessage &= "|BVG-Data (Normal_0!): "
		LoggingMessage &= msg


		' Kandidat hat Priortät 1
		Select Case Val(m_EmployeeLOSetting.BVGCode)
			Case 0            ' 0; Kein Abzug
				cBVGStd = 0

			Case 1, 2, 3         ' 1; Ab 1. Tag
				cBVGStd = WorkTime
				bvgDates = GetBVGDaysDatainCurrentMonth(lpMonth, lpYear)

			Case Else                               ' 9; Ab 13 Wochen

				' Einsatz ist unbefristet: dann ist es ab dem ersten Tag BVG-pflichtig (spezialfall beachten!!!)
				' Einsatz geht länger als 3 Monate: ist ab 3. Monat BVG-pflichtig
				' Einsatz ist kürtzer als 3 Monate: ist NICHT BVG-pflichtig
				' Die Einsätze sind unterbrochen und der Unterbruch dauert länger als x Wochen:
				' BVG-pflicht fängt von 0 an.
				' Die Einsätze sind unterbrochen und der Unterbruch dauert kürzer als x Wochen:
				' BVG-pflicht geht weiter.
				' Der Mann arbeitet neu: Der BVG-pflicht fängt von 0 an
				cBVGStd = 0

				Dim rpData = m_PayrollDatabaseAccess.LoadRPDataForESStdTotalNew0Calculation(employeeNumber, m_InitialData.MDData.MDNr,
																																										DateAdd("m", -CInt(Math.Max(1, iESBreakWeek * 7 / 30)), StartofMonth),
																																										DateAdd("m", 1, StartofMonth))
				ThrowExceptionOnError(rpData Is Nothing, "RP Daten für ES_Std_Total_New_0 konnten nicht geladen werden.")

				If rpData.Count = 0 Then
					BVGWorkTime = Math.Min(cBVGStd, WorkTime)
					Return cBVGStd
				End If

				Dim rRPrec = rpData(0)

				' zum Ersten mal werden die Von und Bis Daten gespeichert...
				dOldRPVon = rRPrec.Von
				dOldRPBis = rRPrec.Bis
				iAnzAlteDauer = rRPrec.ESRPTage
				msg &= String.Format(" ¦ RP-Daten: {0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

				If rRPrec.Monat = lpMonth And Convert.ToInt32(rRPrec.Jahr) = lpYear Then
					' Es ist der erste Monat wo er arbeitet; dann ist es nichts!!!
					cBVGStd = 0

				Else

					For rpIndex As Integer = 1 To rpData.Count - 1

						rRPrec = rpData(rpIndex)
						msg &= String.Format("{0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

						' If Not rRPrec.EOF Then
						If dOldRPVon < rRPrec.Von Then
							If dOldRPBis >= rRPrec.Von Then
								If dOldRPBis <= rRPrec.Bis Then dOldRPBis = rRPrec.Bis

							ElseIf dOldRPBis < rRPrec.Von Then
								' Das Ende der letzte Rapport ist vor dem neuen Rapportbeginn: kann pflichtig werden
								If DateDiff(bvginterval, dOldRPBis, rRPrec.Von, vbUseSystemDayOfWeek, vbUseSystem) > iESBreakWeek Then
									' dann fängt alles von 0 an...
									dOldRPVon = rRPrec.Von
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer = Val(rRPrec.ESRPTage)

								Else

									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer += Val(rRPrec.ESRPTage)

								End If
							End If

						End If

						If DateDiff(bvginterval, dOldRPVon, dOldRPBis, vbUseSystemDayOfWeek, vbUseSystem) > iBVGAfter Then

							' es ist dann BVG-pflichtig!!!
							If rRPrec.Monat = lpMonth And rRPrec.Jahr = lpYear Then

								' Testphase...
								Dim dBVGBegin As Date
								'Dim dDiffDaysbisEndeMonat As Integer

								'dDiffDaysbisEndeMonat = Math.Max(0, iAnzAlteDauer - (iBVGAfter * 7))
								'dBVGBegin = DateAdd("d", dDiffDaysbisEndeMonat * (-1), dOldRPBis)
								' 2. Test
								dBVGBegin = DateAdd(DateInterval.Day, (iBVGAfter * 7), dOldRPVon)

								If BVGBeginForLO Is Nothing Then BVGBeginForLO = dBVGBegin
								If BVGEndForLO Is Nothing Then BVGEndForLO = rpData.Max(Function(data) data.Bis)

								bvgDates.Add(New BVGDayData With {.RPNr = rRPrec.RPNr, .Von = rRPrec.Von, .Bis = rRPrec.Bis,
																	 .DayCount = DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})
								If String.IsNullOrWhiteSpace(bvgDayMessage) Then bvgDayMessage = "(BVG-Tage) "
								bvgDayMessage &= String.Format("{0:d} - {1:d}: {2} | ", rRPrec.Von, rRPrec.Bis,
																		 DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1)

								'If Month(dBVGBegin) < Month(dOldRPBis) Then
								If dBVGBegin < dOldRPBis AndAlso Month(dBVGBegin) < Month(dOldRPBis) Then
									' BVGBeginn war bereits letzten Monat
									Dim BVGrpData = rpData.Where(Function(data) data.Monat = lpMonth)
									dBVGBegin = BVGrpData.Min(Function(data) data.Von)
									BVGBeginForLO = dBVGBegin

									cBVGStd = WorkTime
									bvgDates = GetBVGDaysDatainCurrentMonth(lpMonth, lpYear)

									Exit For

								End If
								' End der Testphase...

								cBVGStd += GetBVGStdFromRPL_0(rRPrec.RPNr, dOldRPVon, dOldRPBis)

							End If

						End If

					Next
				End If

		End Select

		If cBVGStd > 0 Then
			Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(employeeNumber, m_InitialData.MDData.MDNr,
																																												 StartofCurrentPayrollMonth(lpMonth, lpYear), EndofCurrentPayrollMonth(lpMonth, lpYear))
			ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")

			Dim firstESAb As DateTime? = ESDataRecList.Min(Function(data) data.ES_Ab)
			Dim lastESEnde As DateTime? = Nothing

			If ESDataRecList.Any(Function(data) Not data.ES_Ende.HasValue) Then
				lastESEnde = EndofMonth
			Else
				lastESEnde = ESDataRecList.Max(Function(data) data.ES_Ende)
			End If
			If Not BVGBeginForLO.HasValue Then BVGBeginForLO = firstESAb
			If Not BVGEndForLO.HasValue Then BVGEndForLO = lastESEnde

			BVGBeginForLO = dateUtility.MaxDate(firstESAb, dateUtility.MaxDate(BVGBeginForLO, StartofMonth))
			BVGEndForLO = dateUtility.MinDate(lastESEnde, dateUtility.MinDate(BVGEndForLO, EndofMonth))
			WorkDaysCurrentMonth = DateDiff(DateInterval.Day, CDate(BVGBeginForLO), CDate(BVGEndForLO), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1

			If bvgDates Is Nothing OrElse bvgDates.Count = 0 Then
				bvgDates.Add(New BVGDayData With {.RPNr = 0, .Von = BVGBeginForLO, .Bis = BVGEndForLO,
																					.DayCount = DateDiff(DateInterval.Day, CDate(BVGBeginForLO), CDate(BVGEndForLO), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})
			End If

		End If

		If Not String.IsNullOrWhiteSpace(msg) Then LoggingMessage &= msg & String.Format("Anzahl Tage: {0}", iAnzAlteDauer)
		If Not bvgDates Is Nothing AndAlso bvgDates.Count > 0 Then
			Dim bvgDayCount As Integer = Get_BVGDays_In_Month_New(lpMonth, lpYear, bvgDates)

			If Not String.IsNullOrWhiteSpace(msg) Then LoggingMessage &= bvgDayMessage & String.Format("Anzahl BVG-pflichtige Tage: {0}", bvgDayCount)
		End If

		BVGWorkTime = Math.Min(cBVGStd, WorkTime)
		' es sollte nur bis einer Maximum 180 Stunden pro Monat kommen
		BVGWorkTime = Math.Min(BVGWorkTime, Val(m_MandantData.BVG_Std) / 12)
		bvgDatesData = bvgDates

		Return Math.Min(cBVGStd, Val(m_MandantData.BVG_Std) / 12)

	End Function

	' Neue Funktionen für BVG-Abrechnung.
	' Achtung nicht 3 Monate mit Ketteneinsätzen sonder nach Einsatzdauer...
	Private Function Get_ES_Std_Total_New_1(ByVal employeeNumber As Integer, ByVal lpMonth As Integer, ByVal lpYear As Integer) As Decimal
		Dim iAnzAlteDauer As Integer
		Dim dateUtility As New DateAndTimeUtily

		Dim StartofMonth As Date = StartofCurrentPayrollMonth(lpMonth, lpYear)
		Dim EndofMonth As Date = EndofCurrentPayrollMonth(lpMonth, lpYear)
		Dim dOldRPVon As Date
		Dim dOldRPBis As Date

		Dim cBVGStd As Decimal = 0
		Dim iBVGAfter As Integer
		Dim iESBreakWeek As Integer

		Dim bvgDates As New List(Of BVGDayData)

		Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, lpYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, lpYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		bvginterval = "ww"
		iBVGAfter = Val(bvgintervaladd)

		If iBVGAfter = 0 Then iBVGAfter = 13
		' Achtung: Hier darf er nicht um eins substrahieren, da er hier pro Tag rechnet;
		' ab 91sten Tag ist der Kandidat BVG-pflicht.

		iESBreakWeek = Math.Max(Val(m_MandantData.BVG_Aus1Woche), 2)
		Dim msg As String = String.Format("| BVGCode: {0}", m_EmployeeLOSetting.BVGCode)
		Dim bvgDayMessage As String = String.Empty
		msg &= String.Format("| bvginterval: {0} ", bvginterval)
		msg &= String.Format("| iBVGAfter: {0} ", iBVGAfter)

		LoggingMessage &= "|BVG-Data (Berechnung pro Einsatztage!): "
		LoggingMessage &= msg
		msg = String.Empty

		' Kandidat hat Priortät 1
		Select Case Val(m_EmployeeLOSetting.BVGCode)
			Case 0            ' 0; Kein Abzug
				cBVGStd = 0

			Case 1, 2, 3         ' 1; Ab 1. Tag
				cBVGStd = WorkTime
				bvgDates = GetBVGDaysDatainCurrentMonth(lpMonth, lpYear)


			Case Else                               ' 9; Ab 13 Wochen

				Dim rpData = m_PayrollDatabaseAccess.LoadRPDataForESStdTotalNew1Calculation(m_EmployeeData.EmployeeNumber, m_InitialData.MDData.MDNr,
																																										DateAdd("m", -CInt(Math.Max(1, (iESBreakWeek * 7 / 30))), StartofMonth),
																																										DateAdd("m", 1, StartofMonth))
				ThrowExceptionOnError(rpData Is Nothing, "RP Daten für ES_Std_Total_New_1 konnten nicht geladen werden.")

				If rpData.Count = 0 Then
					BVGWorkTime = Math.Min(cBVGStd, WorkTime)
					Return cBVGStd
				End If

				Dim rRPrec = rpData(0)

				' zum Ersten mal werden die Von und Bis Daten gespeichert...
				dOldRPVon = rRPrec.Von
				dOldRPBis = rRPrec.Bis
				iAnzAlteDauer = rRPrec.ESRPTage
				msg &= String.Format(" ¦ RP-Daten: {0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

				If rRPrec.Monat = lpMonth And Convert.ToInt32(rRPrec.Jahr) = lpYear Then
					' Es ist der erste Monat wo er arbeitet; dann ist es nichts!!!
					cBVGStd = 0

				Else

					For rpIndex As Integer = 1 To rpData.Count - 1

						rRPrec = rpData(rpIndex)

						msg &= String.Format("{0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

						' If Not rRPrec.EOF Then
						If dOldRPVon < rRPrec.Von Then
							If dOldRPBis >= rRPrec.Von Then
								If dOldRPBis <= rRPrec.Bis Then dOldRPBis = rRPrec.Bis

							ElseIf dOldRPBis < rRPrec.Von Then
								' Das Ende der letzte Rapport ist vor dem neuen Rapportbeginn: kann pflichtig werden
								If DateDiff(bvginterval, dOldRPBis, rRPrec.Von, vbUseSystemDayOfWeek, vbUseSystem) + 1 > iESBreakWeek Then
									' dann fängt alles von 0 an...
									dOldRPVon = rRPrec.Von
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer = Val(rRPrec.ESRPTage)

								Else

									dOldRPVon = rRPrec.Von
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer = iAnzAlteDauer + Val(rRPrec.ESRPTage)
								End If
							End If

						End If

						If iAnzAlteDauer >= (iBVGAfter * 7) Then
							' es ist dann BVG-pflichtig!!!
							If rRPrec.Monat = lpMonth And rRPrec.Jahr = lpYear Then
								Dim dBVGBegin As Date
								Dim dDiffDaysbisEndeMonat As Integer

								dDiffDaysbisEndeMonat = Math.Max(0, iAnzAlteDauer - (iBVGAfter * 7))
								dBVGBegin = DateAdd("d", dDiffDaysbisEndeMonat * (-1), dOldRPBis)

								If BVGBeginForLO Is Nothing Then BVGBeginForLO = dBVGBegin
								If BVGEndForLO Is Nothing Then BVGEndForLO = rpData(rpData.Count - 1).Bis

								'If Month(dBVGBegin) < Month(dOldRPBis) Then
								If dBVGBegin < dOldRPBis AndAlso Month(dBVGBegin) < Month(dOldRPBis) Then
									' BVGBeginn war bereits letzten Monat
									Dim BVGrpData = rpData.Where(Function(data) data.Monat = lpMonth) 'And data.Jahr = LPjahr)
									dBVGBegin = BVGrpData.Min(Function(data) data.Von)
									BVGBeginForLO = dBVGBegin

									bvgDates.Add(New BVGDayData With {.RPNr = rRPrec.RPNr, .Von = rRPrec.Von, .Bis = rRPrec.Bis,
																									 .DayCount = DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})
									If String.IsNullOrWhiteSpace(bvgDayMessage) Then bvgDayMessage = "(BVG-Tage) "
									bvgDayMessage &= String.Format("{0:d} - {1:d}: {2} | ", rRPrec.Von, rRPrec.Bis,
																			 DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1)

									If rpIndex = rpData.Count - 1 Then
										cBVGStd = WorkTime
										bvgDates = GetBVGDaysDatainCurrentMonth(lpMonth, lpYear)

										Exit For
									End If

								End If

								cBVGStd += GetBVGStdFromRPL_1(rRPrec.RPNr, dBVGBegin, dOldRPBis)

							End If
						End If
						' End If

					Next

					If BVGBeginForLO.HasValue Then BVGEndForLO = dOldRPBis

				End If

		End Select


		If cBVGStd > 0 Then
			Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(m_EmployeeData.EmployeeNumber, m_InitialData.MDData.MDNr, StartofMonth, EndofMonth)
			ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")

			Dim firstESAb As DateTime? = ESDataRecList.Min(Function(data) data.ES_Ab)
			Dim lastESEnde As DateTime? = Nothing

			If ESDataRecList.Any(Function(data) Not data.ES_Ende.HasValue) Then
				lastESEnde = EndofMonth
			Else
				lastESEnde = ESDataRecList.Max(Function(data) data.ES_Ende)
			End If
			If Not BVGBeginForLO.HasValue Then BVGBeginForLO = firstESAb
			If Not BVGEndForLO.HasValue Then BVGEndForLO = lastESEnde

			BVGBeginForLO = dateUtility.MaxDate(firstESAb, dateUtility.MaxDate(BVGBeginForLO, StartofMonth))
			BVGEndForLO = dateUtility.MinDate(lastESEnde, dateUtility.MinDate(BVGEndForLO, EndofMonth))
			WorkDaysCurrentMonth = DateDiff(DateInterval.Day, CDate(BVGBeginForLO), CDate(BVGEndForLO), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1

			If bvgDates Is Nothing OrElse bvgDates.Count = 0 Then
				bvgDates.Add(New BVGDayData With {.RPNr = 0, .Von = BVGBeginForLO, .Bis = BVGEndForLO,
																					.DayCount = DateDiff(DateInterval.Day, CDate(BVGBeginForLO), CDate(BVGEndForLO), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})
			End If

		End If

		If Not String.IsNullOrWhiteSpace(msg) Then LoggingMessage &= msg & String.Format(" Anzahl Tage: {0} Tage", iAnzAlteDauer)
		If Not bvgDates Is Nothing AndAlso bvgDates.Count > 0 Then
			Dim bvgDayCount As Integer = Get_BVGDays_In_Month_New(lpMonth, lpYear, bvgDates)

			If Not String.IsNullOrWhiteSpace(msg) Then LoggingMessage &= bvgDayMessage & String.Format(" Anzahl BVG-pflichtige Tage: {0} Tage", bvgDayCount)
		End If

		BVGWorkTime = Math.Min(cBVGStd, WorkTime)
		' es sollte nur bis einer Maximum 180 Stunden pro Monat kommen
		BVGWorkTime = Math.Min(BVGWorkTime, Val(m_MandantData.BVG_Std) / 12)
		bvgDatesData = bvgDates

		Return BVGWorkTime

	End Function

	Private Function GetBVGDaysDatainCurrentMonth(ByVal lpMonth As Integer, ByVal lpYear As Integer) As List(Of BVGDayData)
		Dim bvgDates As New List(Of BVGDayData)
		Dim dateUtility As New DateAndTimeUtily

		Dim StartofMonth As Date = StartofCurrentPayrollMonth(lpMonth, lpYear)
		Dim EndofMonth As Date = EndofCurrentPayrollMonth(lpMonth, lpYear)

		Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(m_EmployeeData.EmployeeNumber, m_InitialData.MDData.MDNr, StartofMonth, EndofMonth)
		ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")
		If ESDataRecList.Count = 0 Then Return bvgDates

		Dim oldBegin As Date = dateUtility.MaxDate(ESDataRecList(0).ES_Ab, StartofMonth)
		Dim oldEnde As Date = dateUtility.MinDate(ESDataRecList(0).ES_Ende.GetValueOrDefault(EndofMonth), EndofMonth)

		Dim esBeginn As Date
		Dim esEnde As Date
		Dim i As Integer = 0

		For Each esdata In ESDataRecList

			esBeginn = dateUtility.MaxDate(esdata.ES_Ab, StartofMonth)
			esEnde = dateUtility.MinDate(esdata.ES_Ende.GetValueOrDefault(EndofMonth), EndofMonth)

			If i > 0 AndAlso esBeginn < oldEnde Then
				esBeginn = oldEnde
			End If

			If i > 0 AndAlso esEnde < oldEnde Then esEnde = oldEnde
			bvgDates.Add(New BVGDayData With {.RPNr = 0, .Von = esBeginn, .Bis = esEnde,
																				.DayCount = DateDiff(DateInterval.Day, CDate(esBeginn), CDate(esEnde), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})

			oldBegin = esBeginn
			oldEnde = esEnde

		Next

		Return bvgDates

	End Function

	Private Function GetBVGStdFromRPL_0(ByVal lRPNr As Integer, ByVal dRPStart As Date, ByVal dRPEnde As Date) As Decimal
		Dim dBVGStart As Date
		Dim i As Integer
		Dim strFieldBez As String = String.Empty
		Dim cTotalStd As Decimal = 0
		Dim iBVGAfter As Integer

		Dim dStartofMonth As Date = StartofCurrentPayrollMonth(Month(dRPStart), Year(dRPStart))
		Dim dEndofMonth As Date = EndofCurrentPayrollMonth(Month(dRPStart), Year(dRPStart))

		Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, Year(dRPStart)), String.Format("{0}/bvginterval", m_PayrollSetting))
		Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, Year(dRPStart)), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		bvginterval = "ww"
		iBVGAfter = Val(bvgintervaladd)

		'If strBVGInterval = "" Then strBVGInterval = "ww" ' Wochenweise
		If iBVGAfter = 0 Then iBVGAfter = 13 ' nach 13 Wochen
		'iBVGAfter = iBVGAfter - 1

		' Startdatum für BVG-Beginn...
		dBVGStart = DateAdd(bvginterval, iBVGAfter, dRPStart)
		If dBVGStart > dEndofMonth Then Return cTotalStd

		If Month(dBVGStart) = Month(dRPEnde) And Year(dBVGStart) = Year(dRPEnde) Then
			For i = (dBVGStart.Day) To (dRPEnde.Day)
				strFieldBez = strFieldBez & IIf(strFieldBez = "", "", " + ") & "ISNull(RPL_MA_Day.Tag" & i & ", 0.00)"
			Next i

		Else
			For i = 1 To (dRPEnde.Day)
				strFieldBez = strFieldBez & IIf(strFieldBez = "", "", " + ") & "ISNull(RPL_MA_Day.Tag" & i & ", 0.00)"
			Next i

		End If

		Dim sumStdTotal = m_PayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPL0Calculation(strFieldBez, Convert.ToInt32(lRPNr))

		If sumStdTotal.HasValue Then cTotalStd = sumStdTotal

		Return cTotalStd

	End Function

	Private Function GetBVGStdFromRPL_1(ByVal lRPNr As Integer, ByVal dRPStart As Date, ByVal dRPEnde As Date) As Decimal
		Dim i As Integer
		Dim strFieldBez As String = String.Empty
		Dim cTotalStd As Decimal = 0

		Dim dStartofMonth As Date = StartofCurrentPayrollMonth(Month(dRPStart), Year(dRPStart))
		Dim dEndofMonth As Date = EndofCurrentPayrollMonth(Month(dRPStart), Year(dRPStart))

		If dRPStart > dEndofMonth Then Return cTotalStd
		If Month(dRPStart) <> Month(dRPEnde) Then
			dRPStart = dStartofMonth
		End If

		For i = dRPStart.Day To dRPEnde.Day
			strFieldBez = strFieldBez & IIf(strFieldBez = "", "", " + ") & "ISNull(RPL_MA_Day.Tag" & i & ", 0.00)"
		Next
		If strFieldBez <> "" Then

			Dim sumStdTotal = m_PayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPL1Calculation(strFieldBez, Convert.ToInt32(lRPNr))

			If sumStdTotal.HasValue Then cTotalStd = sumStdTotal

		End If

		Return cTotalStd

	End Function


	Private Function Get_BVGDays_In_Month_New(ByVal lpMonth As Integer, ByVal lpYear As Integer, ByVal bvgDates As IEnumerable(Of BVGDayData)) As Integer
		Dim rpBegin As Date
		Dim rpEnde As Date
		Dim bvgDays As Integer
		Dim bvgBusinessDays As Integer
		Dim dateUtility As New DateAndTimeUtily
		Dim StartofMonth As Date
		Dim EndofMonth As Date

		If bvgDates Is Nothing OrElse bvgDates.Count = 0 Then Return 0

		Dim ESDataRecList = bvgDates
		Dim getbvgasbusinessdays As Boolean = m_utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitialData.MDData.MDNr, lpYear),
																																													String.Format("{0}/bvgasbusinessdays", m_PayrollSetting)), False)
		LoggingMessage &= String.Format(" ¦ getbvgasbusinessdays: {0} ¦ ", getbvgasbusinessdays)

		StartofMonth = StartofCurrentPayrollMonth(lpMonth, lpYear)
		EndofMonth = EndofCurrentPayrollMonth(lpMonth, lpYear)

		' Kandidat hat Priortät 1
		Select Case m_EmployeeLOSetting.BVGCode
			Case 0             ' Kein Abzug
				bvgDays = 0
				'Case 1, 2, 3				 ' Ab 1. Tag
				'	bvgDays = ESLPTage

			Case Else                               ' automatisch ab 13 Wochen
				Dim oldBegin As Date = ESDataRecList(0).Von
				Dim oldEnde As Date = dateUtility.MinDate(ESDataRecList(0).Bis, EndofMonth)
				Dim i As Integer = 0

				For Each ESDataRec In ESDataRecList

					rpBegin = ESDataRec.Von
					rpEnde = dateUtility.MinDate(ESDataRec.Bis, EndofMonth)

					If i > 0 AndAlso rpBegin <= oldEnde AndAlso rpEnde <= oldEnde Then

					Else

						If i > 0 AndAlso rpBegin < oldEnde Then
							rpBegin = oldEnde
						End If

						If i > 0 AndAlso rpEnde < oldEnde Then rpEnde = oldEnde

						bvgDays += DateDiff(DateInterval.Day, rpBegin, rpEnde, FirstDayOfWeek.System, FirstWeekOfYear.System) + 1
						bvgBusinessDays += Weekdays(rpBegin, rpEnde) '+ 1

						oldBegin = rpBegin
						oldEnde = rpEnde

					End If

					i += 1

				Next
				If getbvgasbusinessdays Then bvgDays = bvgBusinessDays

		End Select
		BVGWorkDays = bvgDays

		Return Math.Min(bvgDays, WorkDaysCurrentMonth)

	End Function

	Private Function Weekdays(ByVal startDate As Date, ByVal endDate As Date) As Integer
		Dim numWeekdays As Integer = 0
		Dim totalDays As Integer = 0
		Dim WeekendDays As Integer = 0

		totalDays = DateDiff(DateInterval.Day, startDate, endDate, FirstDayOfWeek.System, FirstWeekOfYear.System) + 1

		For i As Integer = 1 To totalDays

			If DatePart(DateInterval.Weekday, startDate) = 1 Then
				WeekendDays = WeekendDays + 1
			End If
			If DatePart(DateInterval.Weekday, startDate) = 7 Then
				WeekendDays = WeekendDays + 1
			End If
			startDate = DateAdd("d", 1, startDate)
		Next

		numWeekdays = totalDays - WeekendDays

		Return numWeekdays

	End Function

	Private Sub ThrowExceptionOnError(ByVal err As Boolean, ByVal errorText As String)
		If err Then
			Throw New Exception(errorText)
		End If

	End Sub

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function

End Class
