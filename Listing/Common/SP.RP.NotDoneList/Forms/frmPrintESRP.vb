
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Deleted
Imports SP.DatabaseAccess.Deleted.DataObjects
Imports SP.DatabaseAccess.Deleted.DataObjects.DeletedData
Imports SP.DatabaseAccess.Deleted.DeletedDatabaseAccess

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports System.Threading

Public Class frmPrintESRP
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_DeletedDatabaseAccess As IDeletedDatabaseAccess

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


	Private _ClsFunc As New ClsDivFunc


	Public Shared frmMyLV As frmRPListSearch_LV


	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean


	Private connectionString As String

	Private m_GridSettingPath As String
	Private m_GVESSettingfilename As String

	Private m_xmlSettingRestoreSearchSetting As String
	Private m_xmlSettingSearchFilter As String

#End Region


#Region "private consts"


#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		connectionString = m_InitializationData.MDData.MDDbConn
		m_DeletedDatabaseAccess = New SP.DatabaseAccess.Deleted.DeletedDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		ResetMandantenDropDown()
		LoadMandantenDropDown()

	End Sub

#End Region

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
		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

		'    Return Not Data Is Nothing
	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName)

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)

		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		'Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region


#Region "Dropdown Funktionen 1. Seite..."

	Private Sub Cbo_KST1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST1.QueryPopUp
		If Me.Cbo_KST1.Properties.Items.Count = 0 Then ListREKst1(Me.Cbo_KST1)
	End Sub

	Private Sub Cbo_KST2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST2.QueryPopUp
		If Me.Cbo_KST2.Properties.Items.Count = 0 Then ListREKst2(Me.Cbo_KST2)
	End Sub

	Private Sub Cbo_Berater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		If Me.Cbo_Berater.Properties.Items.Count = 0 Then ListBerater(Me.Cbo_Berater)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		If Me.Cbo_Filiale.Properties.Items.Count = 0 Then ListRPFiliale(Me.Cbo_Filiale)
	End Sub

	Private Sub Cbo_Month_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.QueryPopUp
		If Me.Cbo_Month.Properties.Items.Count = 0 Then ListRPMonth(Cbo_Month)
	End Sub

	Private Sub Cbo_Year_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.QueryPopUp
		If Me.Cbo_Year.Properties.Items.Count = 0 Then ListRPYear(Cbo_Year)
	End Sub

#End Region





	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmZGSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmRPListSearch_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Try
			Me.Text = m_xml.GetSafeTranslationValue(Me.Text)

			Me.xtabAllgemein.Text = m_xml.GetSafeTranslationValue(Me.xtabAllgemein.Text)
			Me.xtabSQLAbfrage.Text = m_xml.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)

			Me.lblHeader1.Text = m_xml.GetSafeTranslationValue(Me.lblHeader1.Text)
			Me.lblHeader2.Text = m_xml.GetSafeTranslationValue(Me.lblHeader2.Text)
			Me.CmdClose.Text = m_xml.GetSafeTranslationValue(Me.CmdClose.Text)

			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiSearch.Caption = m_xml.GetSafeTranslationValue(Me.bbiSearch.Caption)
			Me.bbiClear.Caption = m_xml.GetSafeTranslationValue(Me.bbiClear.Caption)
			Me.bbiPrint.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrint.Caption)

			Me.lblMDName.Text = m_xml.GetSafeTranslationValue(Me.lblMDName.Text)
			Me.lblsortierung.Text = m_xml.GetSafeTranslationValue(Me.lblsortierung.Text)
			Me.lblJahr.Text = m_xml.GetSafeTranslationValue(Me.lblJahr.Text)
			Me.lblMonat.Text = m_xml.GetSafeTranslationValue(Me.lblMonat.Text)

			Me.lbl1KST.Text = m_xml.GetSafeTranslationValue(Me.lbl1KST.Text)
			Me.lbl2KST.Text = m_xml.GetSafeTranslationValue(Me.lbl2KST.Text)
			Me.lblBerater.Text = m_xml.GetSafeTranslationValue(Me.lblBerater.Text)

			Me.CheckBox1.Text = m_xml.GetSafeTranslationValue(Me.CheckBox1.Text)
			Me.CheckBox2.Text = m_xml.GetSafeTranslationValue(Me.CheckBox2.Text)
			Me.CheckBox3.Text = m_xml.GetSafeTranslationValue(Me.CheckBox3.Text)

			Me.lblFiliale.Text = m_xml.GetSafeTranslationValue(Me.lblFiliale.Text)
			Me.lblAktuellQery.Text = m_xml.GetSafeTranslationValue(Me.lblAktuellQery.Text)


		Catch ex As Exception
			Logger.Error(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim m_md As New Mandant
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSort As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & _
																									 "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0").ToString
		If strSort = String.Empty Then strSort = String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Kandidatenname"))
		Me.CboSort.Text = strSort

		Try
			Try
				Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
				Me.Invoke(UpdateDelegate)

			Catch ex As Exception
				Logger.Error(String.Format("{0}. Translation: {1}", strMethodeName, ex.Message))

			End Try

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
				If My.Settings.frm_Location <> String.Empty Then
					Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				Logger.Error(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			Logger.Error(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try
		SetBeraterValues()

		Try
			Me.lueMandant.EditValue = ClsDataDetail.ProgSettingData.SelectedMDNr
			Me.lueMandant.Visible = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

		Catch ex As Exception
			Logger.Error(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

		Me.Cbo_Year.Text = Now.Year
		Me.Cbo_Month.Text = If(Now.Day > 10, Now.Month, Now.Month - 1)

		Me.LblTimeValue.Visible = CBool(CInt(ClsDataDetail.ProgSettingData.LogedUSNr) = 1)

		If ClsDataDetail.UserData.UserNr <> 1 Then
			Me.xtabRPSearch.TabPages.Remove(Me.xtabSQLAbfrage)
		End If


	End Sub

	Sub SetBeraterValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_common As New CommonSetting

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(0, 672) Then
			Try
				Me.Cbo_Filiale.Enabled = False
				Me.Cbo_KST1.Enabled = False
				Me.Cbo_KST2.Enabled = False
				Dim strUSTitle As String = GetUSTitle(ClsDataDetail.ProgSettingData.LogedUSNr)

				Me.Cbo_Berater.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
				Me.Cbo_Filiale.Text = m_common.GetLogedUserFiliale
				ListBerater(Me.Cbo_Berater, Me.Cbo_Filiale.Text)


				For Each item As CheckedListBoxItem In Me.Cbo_Berater.Properties.Items
					Dim cv As ComboValue = DirectCast(item.Value, ComboValue)
					Dim strKst As String = cv.ComboValue.Trim
					Dim strUserName As String = cv.Text.Trim
					If strUserName.ToLower.Contains(String.Format("{0}, {1}", m_common.GetLogedUserLastName, m_common.GetLogedUserFirstName).ToLower) Then
						item.CheckState = CheckState.Checked
						Me.Cbo_Berater.Text = cv.Text
						Exit For
					End If

				Next

			Catch ex As Exception
				Logger.Error(String.Format("{0}.BenutzerTitel auflisten: {1}", strMethodeName, ex.Message))
			End Try

		End If

	End Sub

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True

		Dim sSql As String = ClsDataDetail.GetSQLQuery()
		If sSql = String.Empty Then
			MsgBox(m_xml.GetSafeTranslationValue("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "GetData4Print_0")
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader					 ' Offertendatenbank
			Try
				If Not rZGrec.HasRows Then
					cmd.Dispose()
					rZGrec.Close()

					MessageBox.Show(m_xml.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."), "GetData4Print_1", _
													MessageBoxButtons.OK, MessageBoxIcon.Information)
					Exit Sub
				End If

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print_2")
				Exit Sub

			End Try
			rZGrec.Read()
			If rZGrec.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				PrintListingThread = New Thread(AddressOf StartPrinting)
				PrintListingThread.Name = "PrintingESListing"
				PrintListingThread.SetApartmentState(ApartmentState.STA)
				PrintListingThread.Start()

			End If
			rZGrec.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetData4Print_3")

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

	End Sub

	Sub StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLRPNotFinishedPrintSetting With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring, _
																																											 .bAsDesign = Me.bPrintAsDesign, _
																																									 .SQL2Open = Me.SQL4Print, _
																																									 .JobNr2Print = Me.PrintJobNr, _
																																											 .ListSortBez = ClsDataDetail.GetSortBez, _
																																											 .ListFilterBez = New List(Of String)(New String() _
																																																														{ClsDataDetail.GetFilterBez, _
																																																														 ClsDataDetail.GetFilterBez2, _
																																																														 ClsDataDetail.GetFilterBez3, _
																																																														 ClsDataDetail.GetFilterBez4}),
																																											 .SelectedMDNr = ClsDataDetail.ProgSettingData.SelectedMDNr,
																																											 .LogedUSNr = ClsDataDetail.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.RPNotFinishedListing.ClsPrintRPNotFinishedList(_Setting)

		obj.PrintRPNotFinishedList()

	End Sub

#Region "Funktionen zur Menüaufbau..."


	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		Try
			_ClsFunc.GetRPNr = 0
			_ClsFunc.GetMANr = 0

			Me.txt_SQL_1.Text = String.Empty
			ClsDataDetail.GetSQLQuery() = String.Empty
			ClsDataDetail.GetSQLSortString() = String.Empty

			FormIsLoaded("frmRPListSearch_LV", True)
			Me.LblTimeValue.Text = String.Empty
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Nach Daten wird gesucht...")

			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()

		Catch ex As Exception
			MessageBox.Show(ex.Message, "bbisearch_Click")

		Finally

		End Try

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty
		Dim _ClsDb As New ClsDbFunc

		If ClsDataDetail.GetSQLQuery() = String.Empty Then
			Me.bbiSearch.Enabled = False

			_ClsDb.GetJobNr4Print(Me)
			sSql1Query = _ClsDb.GetStartSQLString()		 ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)		' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If

			ClsDataDetail.GetSQLQuery = sSql1Query + sSql2Query & " Order By RP.RPNr"
			strSort = _ClsDb.GetSortString(Me)			' Sort Klausel
			ClsDataDetail.GetSQLSortString = strSort

			Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery


			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.RunWorkerAsync()		' Multithreading starten

		End If

		Return True
	End Function

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		'    Dim cControl As Control
		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmRPListSearch_LV", True)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

		Me.bbiSearch.Enabled = True

		' Cbo leeren...
		Me.Cbo_Month.Properties.Items.Clear()
		Me.Cbo_Year.Properties.Items.Clear()


		For Each cControl As Control In Me.Controls
			If TypeOf (cControl) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = cControl
				cbo.Text = String.Empty
				cbo.Properties.Items.Clear()

			ElseIf TypeOf (cControl) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = cControl
				cbo.Text = String.Empty
				cbo.Properties.Items.Clear()

			ElseIf TypeOf (cControl) Is DevExpress.XtraEditors.MemoEdit Then
				Dim cbo As DevExpress.XtraEditors.MemoEdit = cControl
				cbo.Text = String.Empty
			End If

		Next

		Me.CboSort.Text = strText
		ClsDataDetail.GetSQLQuery() = String.Empty
		ClsDataDetail.GetSQLSortString() = String.Empty

	End Sub

#End Region



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

	Private Sub frmZGSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmRPListSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		Dim _ClsDb As New ClsDbFunc
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Stopwatch.Reset()
		Stopwatch.Start()

		CheckForIllegalCrossThreadCalls = False
		Me.LblTimeValue.Text = ""
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Nach Daten wird gesucht...")

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		_ClsDb.BuildRPDayDb(ClsDataDetail.GetSQLQuery) 'sSql1Query + sSql2Query & " Order By RP.RPNr")
		sSql1Query = _ClsDb.GetStartSQLString_2()		 ' 2. String für Drucken (die Whereklausel kommt nicht mehr.
		ClsDataDetail.GetSQLQuery() = sSql1Query + ClsDataDetail.GetSQLSortString()

		e.Result = True
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(e.Error.Message)
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				MessageBox.Show(m_xml.GetSafeTranslationValue("Aktion abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()

				If Not FormIsLoaded("frmRPListSearch_LV", True) Then
					frmMyLV = New frmRPListSearch_LV(ClsDataDetail.GetSQLQuery(), Me.Location.X, Me.Location.Y, Me.Height)

					frmMyLV.Show()
					Me.Select()
					Me.LblTimeValue.Text = String.Format(m_xml.GetSafeTranslationValue("Datenauflistung für {0} Einträge: in {1} ms"), _
																							 frmMyLV.gvRP.RowCount, _
																							 Stopwatch.ElapsedMilliseconds().ToString)

					Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), _
																						 frmMyLV.RecCount)
					frmMyLV.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), _
																									frmMyLV.RecCount)

				End If
				Me.bbiSearch.Enabled = True
				Me.txt_SQL_2.Text = ClsDataDetail.GetSQLQuery()

				If frmMyLV.gvRP.RowCount > 0 Then
					Me.bbiPrint.Enabled = True
					'Me.bbiExport.Enabled = True
					CreatePrintPopupMenu()
				End If

			End If
		End If

	End Sub

#End Region

	Private Sub CboSort_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0", Me.CboSort.Text)
	End Sub

	Private Sub bbiprint_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_xml.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub



	Private Sub CboSort_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(CboSort)
	End Sub




#Region "Contextmenü für Print und Export..."

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 614)
		Dim liMnu As New List(Of String) From {If(IsUserActionAllowed(0, 604), "Liste drucken#mnuRPListPrint", ""), _
																					 If(bShowDesign, "Entwurfansicht#PrintDesign", "")}

		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			'Me.bbiPrint.Visibility = BarItemVisibility.Always
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				bshowMnu = myValue(0).ToString <> String.Empty

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
				End If

			Next

		Catch ex As Exception
			Logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnuRPListPrint".ToUpper
				GetData4Print(False, False, ClsDataDetail.GetModulToPrint())

			Case "PrintDesign".ToUpper
				GetData4Print(True, False, ClsDataDetail.GetModulToPrint())


			Case Else
				Exit Sub

		End Select

	End Sub


#End Region


End Class

