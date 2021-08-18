
Option Strict Off

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SPProgUtility.Mandanten

Imports SPLoNLASearch.ClsDataDetail
Imports SPProgUtility.ProgPath


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

	'// Query.GetSearchCommand
	Dim _strCommand As SqlCommand
	Public Property GetSearchCommand() As SqlCommand
		Get
			Return _strCommand
		End Get
		Set(ByVal value As SqlCommand)
			_strCommand = value
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

	' // LONr
	Dim _strLONr As String
	Public Property GetLONr() As String
		Get
			Return _strLONr
		End Get
		Set(ByVal value As String)
			_strLONr = value
		End Set
	End Property

	' // LP
	'Dim _strLP As String
	'Public Property GetLP() As String
	'  Get
	'    Return _strLP
	'  End Get
	'  Set(ByVal value As String)
	'    _strLP = value
	'  End Set
	'End Property

	' // Jahr 
	Dim _strJahr As String
	Public Property GetJahr() As String
		Get
			Return _strJahr
		End Get
		Set(ByVal value As String)
			_strJahr = value
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

	'// LogedUSSigneFile
	Dim _LogedUSSignFile As String
	Public Property LogedUSSigneFile() As String
		Get
			Return _LogedUSSignFile
		End Get
		Set(ByVal value As String)
			_LogedUSSignFile = value
		End Set
	End Property

	'// LogedUSSigne 
	Dim _LogedUSSigne As Byte()
	Public Property LogedUSSigne() As Byte()
		Get
			Return _LogedUSSigne
		End Get
		Set(ByVal value As Byte())
			_LogedUSSigne = value
		End Set
	End Property

#End Region

End Class

Public Class ClsDbFunc
	Private Shared m_Logger As ILogger = New Logger()

	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_mandant As Mandant
	''' <summary>
	''' The cls prog path.
	''' </summary>
	Private m_path As ClsProgPath

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


	Private m_SearchCriteria As New SearchCriteria

	Public _SynchronizingThread As Threading.Thread
	Public _SynchronizingMain As System.ComponentModel.ISynchronizeInvoke

	Public _NotifyMainProgressDelegate As NotifyMainProgressDel
	Public Delegate Sub NotifyMainProgressDel(ByVal Message As String, ByVal PercentComplete As Integer)
	Public _NotifyMainAllowAbortDelegate As NotifyMainAllowAbortDel
	Public Delegate Sub NotifyMainAllowAbortDel(ByVal Abort As Boolean)


#Region "Private Consts"

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Lohnausweis_NLA"

#End Region




#Region "Constructor"

	Public Sub New(ByVal _search As SearchCriteria)

		m_mandant = New Mandant
		m_path = New ClsProgPath

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

		m_SearchCriteria = _search

	End Sub


#End Region




#Region "Private methods"

	Private Sub NotifyMainProgressBar(ByVal Message As String, ByVal Value As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			If Not _NotifyMainProgressDelegate Is Nothing Then
				Dim args(1) As Object
				args(0) = Message
				args(1) = Value
				_SynchronizingMain.Invoke(_NotifyMainProgressDelegate, args)
			End If

		Catch ex As ObjectDisposedException
			' Das Objekt wurde zerstört --> keine Fehlermeldung
		Catch ex As InvalidOperationException
			' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub NotifyMainAllowAbort(ByVal Abort As Boolean)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			If Not _NotifyMainAllowAbortDelegate Is Nothing Then
				Dim args(0) As Object
				args(0) = Abort
				_SynchronizingMain.Invoke(_NotifyMainAllowAbortDelegate, args)
			End If
		Catch ex As ObjectDisposedException
			' Das Objekt wurde zerstört --> keine Fehlermeldung
		Catch ex As InvalidOperationException
			' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try
	End Sub

	Dim _counter As Integer
	Dim _counterMax As Integer = 100

#End Region


#Region "Funktionen zur Suche nach Daten..."

	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetQuerySQLString() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0

		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		ClsDataDetail.SelectedDataTable.Clear()
		ClsDataDetail.SelectedDataTableFailed.Clear()
		Dim selItem As ClsDataDetail.SelectionItem


		Try

			'With frmTest
			Dim jahr As String = Date.Now.Year.ToString
			Dim manrList As String = ""
			Dim nameVon As String = ""
			Dim nameBis As String = ""
			Dim makanton As String = m_SearchCriteria.employeeCanton
			Dim maLand As String = m_SearchCriteria.employeeCountry
			Dim permission As String = m_SearchCriteria.employeePermission

			ClsDataDetail.SelectedContainer.Clear()
			ClsDataDetail.LLTablename = String.Format("_LoNLA_{0}", m_InitialData.UserData.UserNr)

			' JAHR
			If ClsDataDetail.Param.Jahr.Length > 0 Then
				jahr = ClsDataDetail.Param.Jahr
				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.Jahr
				selItem.Text = jahr
				ClsDataDetail.SelectedContainer.Add(selItem)
			End If

			If ClsDataDetail.Param.MANR.Length > 0 Then
				For Each manr As String In ClsDataDetail.Param.MANR.Split(CChar(","))
					manrList += String.Format("{0},", manr)
				Next
				manrList = manrList.Substring(0, manrList.Length - 1)

				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.MANR
				selItem.Text = manrList
				ClsDataDetail.SelectedContainer.Add(selItem)
			End If

			If ClsDataDetail.Param.NameVon.Length > 0 Then
				nameVon = ClsDataDetail.Param.NameVon

				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.NameVon
				selItem.Text = nameVon
				ClsDataDetail.SelectedContainer.Add(selItem)
			End If

			' NAME BIS
			If ClsDataDetail.Param.NameBis.Length > 0 Then
				nameBis = ClsDataDetail.Param.NameBis
				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.NameBis
				selItem.Text = nameBis
				ClsDataDetail.SelectedContainer.Add(selItem)
			End If


			' NEUER LOHNAUSWEIS
			Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
			cmd.CommandText = "[Create New Table For LoNLA With Mandant]"
			cmd.CommandType = CommandType.StoredProcedure
			Dim pjahr As SqlParameter = New SqlParameter("@jahr", SqlDbType.Int, 4)
			Dim pMANR As SqlParameter = New SqlParameter("@manrListe", SqlDbType.NVarChar, 4000)
			Dim pNameVon As SqlParameter = New SqlParameter("@nameVon", SqlDbType.NVarChar, 100)
			Dim pNameBis As SqlParameter = New SqlParameter("@nameBis", SqlDbType.NVarChar, 100)
			Dim ptblName As SqlParameter = New SqlParameter("@tblName", SqlDbType.NVarChar, 40)
			pjahr.Value = jahr
			pMANR.Value = manrList
			pNameVon.Value = nameVon
			pNameBis.Value = nameBis
			ptblName.Value = ClsDataDetail.LLTablename
			cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			cmd.Parameters.Add(pjahr)
			cmd.Parameters.Add(pMANR)
			cmd.Parameters.Add(pNameVon)
			cmd.Parameters.Add(pNameBis)
			cmd.Parameters.AddWithValue("@Kanton", makanton)
			cmd.Parameters.AddWithValue("@countryListe", maLand)
			cmd.Parameters.AddWithValue("@permissionListe", permission)

			cmd.Parameters.Add(ptblName)

			NotifyMainProgressBar(TranslateMyText("Die Daten werden aufbereitet"), 20)
			Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
			Dim dt As DataTable = New DataTable("NLA")
			AddHandler dt.RowChanged, New DataRowChangeEventHandler(AddressOf Row_Changed)

			' Es muss während der Übermittlung zur und von der Datenbank verhindert werden, dass der Benutzer die Suche unterbricht.
			NotifyMainAllowAbort(False)
			If da.Fill(dt) > 0 Then
				NotifyMainProgressBar(TranslateMyText("Aufbereitung der Daten ist abgeschlossen"), 40)
			End If

			Dim col As DataColumn = New DataColumn("Chk", GetType(String))
			dt.Columns.Add(col)
			For Each row As DataRow In dt.Rows
				row("Chk") = "1"
			Next

			' Lohnausweise schlagen fehl, wenn eine Zahl ausgewiesen und kein Text dazu geschrieben wird.
			' Diese werden aus der normalen Tabelle zur Tabelle der Fehlgeschlagenen verschoben.
			'--------------------------------------------------------------------------------------------------------------
			' Einträge aus der Mandantenverwaltung
			'Dim xmlMandantFile As String = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitialData.MDData.MDNr)
			'Dim str_2_3 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_2_3", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_3_0 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_3_0", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_4_0 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_4_0", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_7_0 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_7_0", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_13_1_2 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_13_1_2", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_13_2_3 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_13_2_3", FORM_XML_MAIN_KEY)), String.Empty)

			'Dim str_14_1_1 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_nebenleistung_1", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_14_1_2 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_nebenleistung_2", FORM_XML_MAIN_KEY)), String.Empty)

			'Dim str_15_1_1 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_bemerkung_1", FORM_XML_MAIN_KEY)), String.Empty)
			'Dim str_15_1_2 As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlMandantFile, String.Format("{0}/nla_bemerkung_2", FORM_XML_MAIN_KEY)), String.Empty)

			'm_Logger.LogInfo(String.Format("str_2_3: {0} | str_3_0: {1} | str_4_0: {2} | str_7_0: {3} | str_13_1_2: {4} | str_13_2_3: {5} | str_14_1_1: {6} | str_14_1_2: {7} | str_15_1_1: {8} | str_15_1_2: {9}",
			'															 str_2_3, str_3_0, str_4_0, str_7_0, str_13_1_2, str_13_2_3, str_14_1_1, str_14_1_2, str_15_1_1, str_15_1_2))

			'ClsDataDetail.NLA_2_3 = str_2_3
			'ClsDataDetail.NLA_3_0 = str_3_0
			'ClsDataDetail.NLA_4_0 = str_4_0
			'ClsDataDetail.NLA_7_0 = str_7_0
			'ClsDataDetail.NLA_13_1_2 = str_13_1_2
			'ClsDataDetail.NLA_13_2_3 = str_13_2_3

			'ClsDataDetail.NLA_Nebenleistung_1 = str_14_1_1
			'ClsDataDetail.NLA_Nebenleistung_2 = str_14_1_2
			'ClsDataDetail.NLA_Bemerkung_1 = str_15_1_1
			'ClsDataDetail.NLA_Bemerkung_2 = str_15_1_2

			'Dim dtFailed As DataTable = New DataTable("NLAFAILED")
			'dtFailed = dt.Clone()
			'Dim bIsAllOK As Boolean = True
			'Dim strFailedMesssage As String = String.Empty

			'Dim queryCommandClausel As String = String.Format("UPDATE {0} ", ClsDataDetail.LLTablename)
			'Dim querySetClausel As String = "{0} = '{1}'"
			'Dim cmdLohnText As New List(Of String)

			'Dim cmdLohn As SqlCommand = New SqlCommand()
			'conn.Open()
			'cmdLohn.Connection = conn
			'Dim nla As String = String.Empty

			'cmdLohnText.Add(String.Format(cmdLohnTextDefault, ClsDataDetail.LLTablename, "NLA_3_0", nla))


			'For z As Integer = 0 To dt.Rows.Count - 1
			'	' Wenn beim Kandidaten und in der Mandantenverwaltung kein Eintrag vorhanden ist, so kann der
			'	' Lohnausweis nicht ordnungsgemäss erstellt werden.

			'	If CDec(dt.Rows(z)("Z_2_3")) <> 0 And dt.Rows(z)("NLA_2_3").ToString = "" And str_2_3 = "" Then
			'		nla = GetLAData4LAWField(CInt(jahr), CInt(dt.Rows(z)("manr").ToString), "2_3")
			'		cmdLohnText.Add(String.Format(querySetClausel, "NLA_2_3", nla))

			'	End If
			'	'strFailedMesssage = "Begründung für Punkt: 2.3 fehlt."
			'	If CDec(dt.Rows(z)("Z_3_0")) <> 0 And dt.Rows(z)("NLA_3_0").ToString = "" And str_3_0 = "" Then
			'		nla = GetLAData4LAWField(CInt(jahr), CInt(dt.Rows(z)("manr").ToString), "3_0")
			'		m_Logger.LogInfo(String.Format("Z_3_0: {0}", nla))
			'		cmdLohnText.Add(String.Format(querySetClausel, "NLA_3_0", nla))

			'	End If
			'	'strFailedMesssage = "Begründung für Punkt: 3 fehlt."
			'	If CDec(dt.Rows(z)("Z_4_0")) <> 0 And dt.Rows(z)("NLA_4_0").ToString = "" And str_4_0 = "" Then
			'		nla = GetLAData4LAWField(CInt(jahr), CInt(dt.Rows(z)("manr").ToString), "4_0")
			'		m_Logger.LogInfo(String.Format("Z_4_0: {0}", nla))
			'		cmdLohnText.Add(String.Format(querySetClausel, "NLA_4_0", nla))

			'	End If
			'	'strFailedMesssage = "Begründung für Punkt: 4 fehlt."
			'	If CDec(dt.Rows(z)("Z_7_0")) <> 0 And dt.Rows(z)("NLA_7_0").ToString = "" And str_7_0 = "" Then
			'		nla = GetLAData4LAWField(CInt(jahr), CInt(dt.Rows(z)("manr").ToString), "7_0")
			'		cmdLohnText.Add(String.Format(querySetClausel, "NLA_7_0", nla))

			'	End If
			'	'strFailedMesssage = "Begründung für Punkt: 7 fehlt."
			'	If CDec(dt.Rows(z)("Z_13_1_2")) <> 0 And dt.Rows(z)("NLA_13_1_2").ToString = "" And str_13_1_2 = "" Then
			'		nla = GetLAData4LAWField(CInt(jahr), CInt(dt.Rows(z)("manr").ToString), "13_1_2")
			'		cmdLohnText.Add(String.Format(querySetClausel, "NLA_13_1_2", nla))

			'	End If
			'	'strFailedMesssage = "Begründung für Punkt: 13.1.2 fehlt."
			'	If CDec(dt.Rows(z)("Z_13_2_3")) <> 0 And dt.Rows(z)("NLA_13_2_3").ToString = "" And str_13_2_3 = "" Then
			'		nla = GetLAData4LAWField(CInt(jahr), CInt(dt.Rows(z)("manr").ToString), "13_2_3")
			'		cmdLohnText.Add(String.Format(querySetClausel, "NLA_13_2_3", nla))

			'	End If

			'	Dim querySetClauselValue As String = String.Empty
			'	If cmdLohnText.Count > 0 Then
			'		querySetClauselValue = String.Join(", ", cmdLohnText.ToList())
			'	End If
			'	Dim queryWhereClausel As String = String.Format(" WHERE MANR = {0} ", CInt(dt.Rows(z)("manr").ToString))

			'	m_Logger.LogInfo(String.Format("cmdLohnTextDefault: {0} >>> querySetClauselValue: {1} >>> queryWhereClausel: {2}", queryCommandClausel, querySetClauselValue, queryWhereClausel))


			'	If cmdLohnText.Count > 0 Then
			'		Dim queryString As String = String.Format("{0} SET {1} {2}", queryCommandClausel, querySetClauselValue, queryWhereClausel)
			'		cmdLohn.CommandText = queryString
			'		cmdLohn.ExecuteNonQuery()

			'		Trace.WriteLine(cmdLohn.CommandText)
			'		cmdLohnText.Clear()
			'		cmdLohn.CommandText = String.Empty
			'		nla = String.Empty
			'	End If

			'	If strFailedMesssage <> String.Empty Then
			'		dt.Rows(z).Item(48) = strFailedMesssage
			'		dtFailed.Rows.Add(dt.Rows(z).ItemArray)
			'		dt.Rows.Remove(dt.Rows(z))
			'		z = z - 1
			'		bIsAllOK = False
			'	Else
			'		bIsAllOK = True
			'	End If


			'	' Die For-Next verlässt sonst nicht die Schleife und rattert trotzdem alle durch...
			'	If z >= dt.Rows.Count - 1 Then
			'		Exit For
			'	End If
			'Next
			ClsDataDetail.SelectedDataTable = dt
			'ClsDataDetail.SelectedDataTableFailed = dtFailed

			sSql = String.Format("Select * From {0}", ClsDataDetail.LLTablename)

			NotifyMainProgressBar("Daten laden abgeschlossen", 60)


		Catch ex As ApplicationException
			' Der Thread wurde während der Erstellung der Tabelle unterbrochen --> Kein Fehler  
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		Finally
			conn.Close()
		End Try

		ClsDataDetail.txt_SQLQuery = sSql
		Return sSql
	End Function

	Private Sub Row_Changed(ByVal sender As Object, ByVal e As DataRowChangeEventArgs)
		Try
			' Die Übertragung mit der Datenbank ist abgeschlossen. Jetzt wird das DataTable gefüllt und kann unterbrochen werden.
			NotifyMainAllowAbort(True)

			_counter += 1
			If _counter > _counterMax Then
				_counterMax = _counterMax * 2
				_counter = 1
			End If
			NotifyMainProgressBar(TranslateMyText("Daten werden aufbereitet..."), ClsDataDetail.GetProzent(1, _counterMax, _counter))
		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
			NotifyMainProgressBar(TranslateMyText("Vorgang wurde abgebrochen"), 1)
		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try


	End Sub

	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strItems += lst.Items(i).ToString & "#@"
		Next

		Return Left(strItems, Len(strItems) - 2)
	End Function

#End Region


End Class

