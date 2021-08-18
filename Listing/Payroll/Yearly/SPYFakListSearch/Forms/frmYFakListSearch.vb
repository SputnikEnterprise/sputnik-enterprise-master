
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.XtraEditors
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports System.Threading
Imports DevExpress.XtraEditors.Controls

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports DevExpress.LookAndFeel

Public Class frmYFakListSearch
	Inherits XtraForm

	Protected Shared m_Logger As ILogger = New Logger()

#Region "Private fields"

	Dim _ClsFunc As New ClsDivFunc
	Dim _ClsUserSec As New SPProgUtility.SPUserSec.ClsUserSec

	Dim _ClsReg As New SPProgUtility.ClsDivReg
	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Public Shared frmMyLV As frmListeSearch_LV
	Public Const frmMyLVName As String = "frmListeSearch_LV"

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private m_md As Mandant
	Protected m_Utility As SPProgUtility.MainUtilities.Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Constructor"


	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		ResetMandantenDropDown()
		LoadMandantenDropDown()

		PrintJobNr = GetJobNr

	End Sub


#End Region

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property

	Private ReadOnly Property GetJobNr() As String
		Get
			Return "12.6"

		End Get
	End Property


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
																					 .Width = 100,
																					 .Caption = "Mandant"})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim _ClsFunc As New ClsDbFunc(Nothing)
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

		'    Return Not Data Is Nothing
	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub


#End Region


#Region "Dropdown Funktionen 1. Seite..."


	Private Sub Cbo_Kanton_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Kanton.QueryPopUp
		Dim year As Integer? = Cbo_Year.EditValue
		If Not year.HasValue Then year = m_InitializationData.MDData.MDYear
		ListMAKanton(Me.Cbo_Kanton, year)
	End Sub

	Private Sub Cbo_Filiale_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Filiale.QueryPopUp
		Dim year As Integer? = Cbo_Year.EditValue
		If Not year.HasValue Then year = m_InitializationData.MDData.MDYear
		ListMAFiliale(Me.Cbo_Filiale, year)
	End Sub

	Private Sub Cbo_Year_QueryCloseUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Year.QueryCloseUp
		' Lohnarten neu suchen. Vielleicht gibt die eine oder andere nicht in ausgewähltem Jahr!
		GetInfoString4LA()
	End Sub

	Private Sub Cbo_Year_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Year.QueryPopUp
		ListLOJahr(Cbo_Year)
	End Sub

#End Region



	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmLohnkontiSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded(frmMyLVName, True)

		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnForm_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		Try
			If FormIsLoaded(frmMyLVName, False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	''' <summary>
	'''  trannslate controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)


	End Sub


	Private Sub OnFormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		TranslateControls()
		SetInitialFields()

		Dim strSort As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0").ToString

		If strSort = String.Empty Then strSort = "2 - Kandidatenname"

		GetInfoString4LA()

	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try
		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.Message))

		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Me.lueMandant.Visible = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

			Me.xtabSQLAbfrage.PageVisible = m_InitializationData.UserData.UserNr = 1

			bbiClear.Enabled = True
			bbiPrint.Enabled = False
			bbiExport.Enabled = False

			FillDefaultValues()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Mandantenauswahl anzeigen: {0}", ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

	End Sub

	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()

		Try
			' DATUM ---------------------------------
			' Dropdown Jahr muss vorbelegt sein.
			If Me.Cbo_Year.EditValue Is Nothing Then
				ListLOJahr(Me.Cbo_Year)
			End If
			Me.Cbo_Year.EditValue = If(Now.Month <= 3, Now.Year - 1, Now.Year)

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub


	Function GetInfoString4LA() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strValue As String = String.Empty
		Dim strSqlQuery As String = "Select LA.LANr, LA.LALOText From LA Where MDNr = @MDNr And LANr In ("
		strSqlQuery += "3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3900.1, 3901, 3901.1) And LA.LAJahr = @Jahr "
		strSqlQuery += "Order By LA.LANr"

		'strSqlQuery = String.Format(strSqlQuery, Math.Max(CInt(Me.Cbo_Year.Text), Now.Year))
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", Cbo_Year.EditValue)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			ClsDataDetail.GetFilterBez2 = String.Empty
			While reader.Read
				ClsDataDetail.GetFilterBez2 &= If(ClsDataDetail.GetFilterBez2 = String.Empty, "", ", ") & String.Format("{0}", Format(reader("LANr"), "f"))
				strValue &= String.Format("<b>{0}:</b> {1}</br>", Format(reader("LANr"), "f"), reader("LALOText"))

			End While
			Me.lbLstlInfo.Text = String.Format("<size=11><b>" & m_Translate.GetSafeTranslationValue("Info") & "</b><size=8.2></br>" & _
																				 m_Translate.GetSafeTranslationValue("Nach folgenden Lohnarten wird gesucht:</br>{0}"), _
																				 strValue)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return strValue

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strValue
	End Function

	' ''' <summary>
	' ''' Daten fürs Drucken bereit stellen.
	' ''' </summary>
	' ''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	' ''' <param name="bForExport">ob die Liste für Export ist.</param>
	' ''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	' ''' <remarks></remarks>
	' ''' 
	'Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'	Dim iKDNr As Integer = 0
	'	Dim iKDZNr As Integer = 0
	'	Dim bResult As Boolean = True
	'	Dim bWithKD As Boolean = True

	'	Dim sSql As String = ClsDataDetail.GetSQLQuery()
	'	If sSql = String.Empty Then
	'		m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"))
	'		Exit Sub
	'	End If

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'	Try
	'		Conn.Open()

	'		Dim rZGrec As SqlDataReader = cmd.ExecuteReader					 ' Offertendatenbank
	'		Try
	'			If Not rZGrec.HasRows Then
	'				cmd.Dispose()
	'				rZGrec.Close()
	'				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
	'				Exit Sub

	'			End If

	'		Catch ex As Exception
	'			m_UtilityUi.ShowErrorDialog(ex.ToString)
	'			Exit Sub

	'		End Try

	'		rZGrec.Read()
	'		If rZGrec.HasRows Then
	'			Me.SQL4Print = sSql
	'			Me.bPrintAsDesign = bForDesign
	'			Me.PrintJobNr = strJobInfo

	'			StartPrinting()
	'			'PrintListingThread = New Thread(AddressOf StartPrinting)
	'			'PrintListingThread.Name = "PrintingFakListing"
	'			'PrintListingThread.SetApartmentState(ApartmentState.STA)
	'			'PrintListingThread.Start()

	'		End If
	'		rZGrec.Close()

	'	Catch ex As Exception
	'		m_UtilityUi.ShowErrorDialog(ex.ToString)

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'End Sub

	Sub StartPrinting()

		Me.PrintJobNr = GetJobNr()
		Me.SQL4Print = Me.txt_SQLQuery.Text
		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLFAKSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
			.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																											 .SQL2Open = Me.SQL4Print,
																																											 .JobNr2Print = Me.PrintJobNr,
																																									 .frmhwnd = GetHwnd}
		Dim obj As New SPS.Listing.Print.Utility.FAKSearchListing.ClsPrintFAKSearchList(_Setting)
		obj.PrintFAKSearchList(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub

#Region "Funktionen zur Menüaufbau..."

	'Private Sub mnuDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDesign.Click

	'	GetData4Print(True, False, ClsDataDetail.GetJobNr4Print)

	'End Sub

	'Private Sub mnuListPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuListPrint.Click

	'	GetData4Print(False, False, ClsDataDetail.GetJobNr4Print)

	'End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		ClsDataDetail.GetFilterBez = String.Empty
		ClsDataDetail.GetFilterBez3 = String.Empty
		ClsDataDetail.GetFilterBez4 = String.Empty
		' Achtung: Darf nicht geleert werden wegen Lohnarten!!!
		'ClsDataDetail.GetFilterBez2 = String.Empty   


		Try

			m_SearchCriteria = GetSearchKrieteria()
			Me.txt_SQLQuery.Text = String.Empty
			If Not (Kontrolle()) Then Exit Sub

			ClsDataDetail.GetSQLQuery() = String.Empty
			ClsDataDetail.GetSQLSortString() = String.Empty
			ClsDataDetail.GetSQLSortString_2() = String.Empty

			FormIsLoaded(frmMyLVName, True)

			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			GetMyQueryString()


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = String.Empty

			If lueMandant.EditValue Is Nothing Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Sie haben keinen Mandanten ausgewählt.{0}"), vbNewLine)
			End If

			If Cbo_Year.EditValue Is Nothing Then
				msg &= String.Format(m_Translate.GetSafeTranslationValue("Sie haben kein Jahr ausgewählt.{0}"), vbNewLine)
			End If

			If Not String.IsNullOrWhiteSpace(msg) Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbNewLine, msg)
				m_UtilityUi.ShowErrorDialog(msg)

				Return False
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Return True
	End Function

	Function GetMyQueryString() As Boolean
		'Dim sSql1Query As String = String.Empty
		'Dim sSql2Query As String = String.Empty
		'Dim strSort As String = String.Empty
		'Dim strArtQuery As String = String.Empty
		'Dim _ClsDb As New ClsDbFunc


		'sSql1Query = _ClsDb.GetStartSQLString(Me)		 ' 1. String
		'sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)		' Where Klausel

		'If Trim(sSql2Query) <> String.Empty Then sSql1Query = String.Format("{0} Where {1}", sSql1Query, sSql2Query)

		'strSort = _ClsDb.GetSortString(Me)			' Sort Klausel
		'ClsDataDetail.GetSQLSortString = strSort
		'ClsDataDetail.GetSQLQuery = String.Format("{0} {1}", sSql1Query, strSort)

		'Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery
		' Die 1. Suchphase in der LOL-Datenbank wurde abgeschlossen. Nun müssen die Daten zusammengestellt werden.

		BackgroundWorker1.WorkerSupportsCancellation = True
		BackgroundWorker1.RunWorkerAsync()		' Multithreading starten
		'      _ClsDb.BuildRPDayDb(ClsDataDetail.GetSQLQuery) 'sSql1Query + sSql2Query & " Order By RP.RPNr")
		'BackgroundWorker1.CancelAsync()     ' Multithreading beenden

		'sSql1Query = _ClsDb.GetStartSQLString_2(Me)    ' 2. String für Drucken (die Whereklausel kommt nicht mehr.

		'ClsDataDetail.GetSQLQuery() = sSql1Query + strSort


		Return True
	End Function

#End Region

#Region "Key Ereignisse..."

	Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

		For Each Ctrl As Control In Ctrls
			AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
			'      AddHandler Ctrl.Click, AddressOf ClickEvents
		Next

	End Sub

	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)	' System.EventArgs)
		'   ToDo  Auswertung und Klick-Aktion ausführen
		'If sender Is TextBox1 Then

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		'End If
	End Sub

#End Region

	Private Sub frmYFakListSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded(frmMyLVName, False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	'Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
	'	GetMenuItems4Export(Me.btnExport)
	'End Sub

	'Private Sub btnExport_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnExport.DropDownItemClicked

	'	btnExport.DropDown.Close()
	'	Select Case UCase(e.ClickedItem.Name)
	'		Case UCase("TXT")
	'			Call RunKommaModul(ClsDataDetail.GetSQLQuery())

	'		Case UCase("XML")
	'			Call RunXMLModul(ClsDataDetail.GetSQLQuery())

	'	End Select

	'End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim bisjahr As String = Me.Cbo_Year.EditValue

		If Me.Cbo_Year.EditValue Is Nothing Then Me.Cbo_Year.EditValue = CStr(Year(Now))
		bisjahr = Cbo_Year.EditValue

		result.listname = m_Translate.GetSafeTranslationValue("Aufstellung über FAK-Abrechnung")
		result.mandantenname = lueMandant.Text

		result.Kanton = Cbo_Kanton.EditValue
		result.filiale = Cbo_Filiale.EditValue

		result.jahr = bisjahr

		Return result

	End Function


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		CheckForIllegalCrossThreadCalls = False
		bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		sSql1Query = _ClsDb.GetStartSQLString()		 ' 1. String
		sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)		' Where Klausel

		If Trim(sSql2Query) <> String.Empty Then sSql1Query = String.Format("{0} Where {1}", sSql1Query, sSql2Query)

		strSort = _ClsDb.GetSortString()			' Sort Klausel
		ClsDataDetail.GetSQLSortString = strSort
		ClsDataDetail.GetSQLQuery = String.Format("{0} {1}", sSql1Query, strSort)

		Me.txt_SQLQuery.Text = ClsDataDetail.GetSQLQuery

		_ClsDb.BuildYFakDb(ClsDataDetail.GetSQLQuery)
		sSql1Query = _ClsDb.GetStartSQLString_2()		 ' 2. String für Drucken (die Whereklausel kommt nicht mehr.
		strSort = _ClsDb.GetSortString_2()					 ' Sort Klausel
		ClsDataDetail.GetSQLSortString_2 = strSort

		ClsDataDetail.GetSQLQuery() = sSql1Query + ClsDataDetail.GetSQLSortString_2()

		Me.txt_SQLQuery.Text = ClsDataDetail.GetSQLQuery

		e.Result = True
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			m_UtilityUi.ShowErrorDialog(e.ToString)
		Else
			If e.Cancelled = True Then
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Vorgang wurde abgebrochen."))

			Else
				BackgroundWorker1.CancelAsync()

				' Daten auflisten...
				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(Me.txt_SQLQuery.Text, String.Empty)
					frmMyLV.Show()
					Me.Select()
				End If

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), _
																					 frmMyLV.RecCount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), _
																								frmMyLV.RecCount)

				' Die Buttons Drucken und Export aktivieren
				If frmMyLV.RecCount > 0 Then
					Me.bbiPrint.Enabled = True
					Me.bbiExport.Enabled = True

					'CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If

			End If
		End If
		Me.bbiSearch.Enabled = True

	End Sub

#End Region

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#PrintList", _
	'																				 "Entwurfsansicht#PrintDesign"}
	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))

	'			If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty
	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

	'				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub

	'Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Me.PrintJobNr = GetJobNr()
	'	Me.SQL4Print = Me.txt_SQLQuery.Text

	'	Try
	'		Select Case e.Item.Name.ToUpper
	'			Case "PrintList".ToUpper
	'				Me.bPrintAsDesign = False

	'			Case "printdesign".ToUpper
	'				Me.bPrintAsDesign = True

	'			Case Else
	'				Exit Sub

	'		End Select
	'		StartPrinting()
	'		'PrintListingThread = New Thread(AddressOf StartPrinting)
	'		'PrintListingThread.Name = "PrintingList"
	'		'PrintListingThread.SetApartmentState(ApartmentState.STA)
	'		'PrintListingThread.Start()

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

	'	End Try

	'End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiExport.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV"}
		Try
			bbiExport.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = myValue(1).ToString

					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.txt_SQLQuery.Text

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				StartExportModul()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "YFakListToCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromLOYFakListing(Me.txt_SQLQuery.Text)

	End Sub


#Region "zum Leeren..."

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		FormIsLoaded("frmListeSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()
		FillDefaultValues()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.XtraTabControl1.Name).Controls
				For Each con As Control In tabPg.Controls
					ResetControl(con)
				Next
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion ruft sich rekursiv auf.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = lueMandant.Name.ToLower Then Exit Sub

			' Rekursiver Aufruf
			' Sonst Control zurücksetzen
			If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
				tb.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = CType(con, DevExpress.XtraEditors.ComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(con, DevExpress.XtraEditors.CheckedComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

#End Region







	Private Sub frmLOAGSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine, _
														 GetExecutingAssembly().FullName, _
														 GetExecutingAssembly().Location, _
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub



	Private Sub Cbo_Kanton_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Kanton.SelectedIndexChanged

	End Sub
End Class

