
Imports System.Reflection.Assembly

Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.ComponentModel
Imports DevExpress.XtraEditors

Public Class frmLOLKontiSearch


#Region "Private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_ConnectionString As String

	Private strValueSeprator As String = "#@"

	'Private _ClsFunc As New ClsDivFunc

	Public Shared frmMyLV As frmListeSearch_LV
	Public Const frmMyLVName As String = "frmListeSearch_LV"

	Private PrintListingThread As Thread
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private m_md As Mandant
	Protected m_Utility As SPProgUtility.MainUtilities.Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria

	Private m_Years As List(Of IntegerValueViewWrapper)
	Private m_LohnKontiData As IEnumerable(Of ListingPayrollLohnkontiData)

#End Region


#Region "Constructor"


	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI


		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		'm_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		'm_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		'm_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)


		Reset()

		TranslateControls()
		SetInitialFields()

		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub


#End Region


#Region "private properties"

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property

	Private ReadOnly Property PrintJobNr As String
		Get
			Return "9.4"
		End Get
	End Property

#End Region


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadMandantenDropDown()
		If success Then PreselectsMandantYearAndMonth()

	End Function

#End Region



	Private Sub Reset()

		ResetMandantenDropDown()
		ResetYearDropDown()
		ResetMonthDropDown()

	End Sub


	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetYearDropDown()

		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the month drop down.
	''' </summary>
	Private Sub ResetMonthDropDown()

		lueMonth.Properties.DisplayMember = "Value"
		lueMonth.Properties.ValueMember = "Value"
		lueMonth.Properties.ShowHeader = False

		lueMonth.Properties.Columns.Clear()
		lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueMonth.Properties.ShowFooter = False
		lueMonth.Properties.DropDownRows = 13
		lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonth.Properties.SearchMode = SearchMode.AutoComplete
		lueMonth.Properties.AutoSearchColumnIndex = 0

		lueMonth.Properties.NullText = String.Empty
		lueMonth.EditValue = Nothing
	End Sub



	Private Function LoadMandantenDropDown() As Boolean
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()


		Return Not Data Is Nothing

	End Function

	''' <summary>
	''' Preselects mandant, year and month.
	''' </summary>
	Private Sub PreselectsMandantYearAndMonth()

		lueMandant.EditValue = m_InitializationData.MDData.MDNr
		PreselectYearAndMonth()

	End Sub

	''' <summary>
	''' Preslects year and month.
	''' </summary>
	Private Sub PreselectYearAndMonth()

		Dim isDateBefore10thDay = (DateTime.Now.Day <= 10)
		Dim today = DateTime.Now.Date
		Dim yearMonthToSelect As DateTime = New DateTime(today.Year, today.Month, 1)

		If isDateBefore10thDay Then
			' Take previous month if date is before 10th.
			yearMonthToSelect = yearMonthToSelect.AddMonths(-1)
		End If

		SelectYear(yearMonthToSelect.Year)

	End Sub

	''' <summary>
	''' Selects a year.
	''' </summary>
	''' <param name="year">The year to select.</param>
	Private Sub SelectYear(ByVal year As Integer)

		If m_Years Is Nothing Then
			m_Years = New List(Of IntegerValueViewWrapper)
			lueYear.Properties.DataSource = m_Years
			lueYear.Properties.ForceInitialize()
		End If

		If Not m_Years.Any(Function(data) data.Value = year) Then
			m_Years.Add(New IntegerValueViewWrapper With {.Value = year})
		End If

		lueYear.EditValue = year

	End Sub

	Private Function LoadMandantYearDropDownData() As Boolean

		Dim success As Boolean = True

		Dim mandantNumber = lueMandant.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not mandantNumber Is Nothing Then

			Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(mandantNumber)

			If (yearData Is Nothing) Then
				success = False
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
			End If

			If Not yearData Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For Each yearValue In yearData
					wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
				Next

			End If

		End If

		m_Years = wrappedValues

		lueYear.EditValue = Nothing
		lueYear.Properties.DataSource = m_Years
		lueYear.Properties.ForceInitialize()

		Return success
	End Function

	''' <summary>
	''' Loads the month drop down data.
	''' </summary>
	Private Function LoadMonthDropDownData() As Boolean

		Dim success As Boolean = True

		Dim mandantNumber = lueMandant.EditValue
		Dim year = lueYear.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not mandantNumber Is Nothing And
				 Not year Is Nothing Then

			Dim closedMonth = m_CommonDatabaseAccess.LoadPayrollMonthOfYear(year, mandantNumber)

			If (closedMonth Is Nothing) Then
				success = False
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verrechnete Monate konnten nicht geladen werden."))
			End If

			If Not closedMonth Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For i As Integer = 1 To 12

					If closedMonth.Contains(i) Then
						wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
					End If

				Next

			End If

		End If

		'm_Month = wrappedValues
		lueMonth.EditValue = Nothing
		lueMonth.Properties.DataSource = wrappedValues
		lueMonth.Properties.ForceInitialize()

		Return success
	End Function


	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

		If Not lueMandant.EditValue Is Nothing Then

			If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
				Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

				Dim clsMandant = m_md.GetSelectedMDData(lueMandant.EditValue)
				Dim logedUserData = m_md.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
				Dim personalizedData = m_InitializationData.ProsonalizedData
				Dim translate = m_InitializationData.TranslationData

				m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)
			End If

			Dim conStr = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

		End If

		LoadMandantYearDropDownData()
		PreselectYearAndMonth()

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub

	''' <summary>
	''' Handles edit change event of lueYear.
	''' </summary>
	Private Sub OnLueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged

		LoadMonthDropDownData()

	End Sub


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
				frmMyLV.TopMost = True
				frmMyLV.TopMost = False
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

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

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try

	End Sub



#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Try
			If lueYear.EditValue Is Nothing Then lueYear.EditValue = CStr(Year(Now))

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty

			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded(frmMyLVName, True)

			' Die Query-String aufbauen...
			GetMyQueryString()


		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim bisjahr As Integer = lueYear.EditValue

		If lueYear.EditValue Is Nothing Then lueYear.EditValue = CStr(Year(Now))
		bisjahr = lueYear.EditValue

		result.listname = m_Translate.GetSafeTranslationValue("Aufstellung über Lohnkonti")
		result.MandanteNname = lueMandant.Text
		result.MDNr = lueMandant.EditValue

		result.EmployeeNumberList = txt_MANr.EditValue
		result.FromYear = bisjahr
		result.FromMonth = lueMonth.EditValue

		Return result

	End Function


	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""

			If Me.txt_MANr.Text.Trim.Length > 0 Then
				For Each manr As String In Me.txt_MANr.Text.Split(CChar(","))
					If Not IsNumeric(manr) Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), manr, vbLf)
					ElseIf CInt(manr).ToString <> manr Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist ungültig.{1}"), manr, vbLf)
					End If
				Next
			End If

			If msg.Length > 0 Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbNewLine, msg)
				m_UtilityUi.ShowErrorDialog(msg)

				Return False
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		' Da ein threadübergreifender Zugriff nicht erlaubt ist, müssen Parameter übertragen werden
		ClsDataDetail.Param.MANR = Me.txt_MANr.Text
		ClsDataDetail.Param.Jahr = lueYear.EditValue

		Return True
	End Function


	Function GetMyQueryString() As Boolean

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)
			Return False

		End Try

		Return True

	End Function

#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		Dim success As Boolean = True

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim cForeColor As New System.Drawing.Color

		Me.bbiSearch.Enabled = False

		Try
			success = success AndAlso LoadQueryresult()
			txt_SQLQuery.EditValue = "[Load Payroll Data For LohnKonti]"

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_LohnKontiData = Nothing
			Me.bbiSearch.Enabled = True

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Try

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Fehler in Ihrer Anwendung.{0}{1}"), vbNewLine, e.Error.Message))
		Else
			If e.Cancelled = True Then
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Vorgang wurde abgebrochen."))

			Else
				BackgroundWorker1.CancelAsync()

				' Daten auflisten...
				If m_LohnKontiData Is Nothing Then Return

				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(m_InitializationData)
					frmMyLV.LohnKontiData = m_LohnKontiData
					frmMyLV.LoadFoundedSalaryList()

					frmMyLV.Show()
					frmMyLV.BringToFront()

				End If

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), m_LohnKontiData.Count)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), m_LohnKontiData.Count)

				Me.ResumeLayout()

			End If

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

		End If
		Me.bbiSearch.Enabled = True

	End Sub

#End Region

	Private Function LoadQueryresult() As Boolean
		Dim success = True

		Dim listOfData = m_ListingDatabaseAccess.LoadAnnualLohnkontiData(m_SearchCriteria.MDNr, m_SearchCriteria.FromYear, m_SearchCriteria.FromMonth, m_SearchCriteria.FromMonth, m_SearchCriteria.EmployeeNumberList)
		If listOfData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(Me, m_Translate.GetSafeTranslationValue("Die Lohnkonti-Daten konnten nicht geladen werden."))

			Return False
		End If
		m_LohnKontiData = listOfData


		Return Not listOfData Is Nothing

	End Function

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
	'		PrintListingThread = New Thread(AddressOf StartPrinting)
	'		PrintListingThread.Name = "PrintingList"
	'		PrintListingThread.SetApartmentState(ApartmentState.STA)
	'		PrintListingThread.Start()

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

	'	End Try

	'End Sub

	Sub StartPrinting()
		Dim strFilter As String = String.Empty

		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)
		Me.SQL4Print = Me.txt_SQLQuery.Text

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.MandanteNname) & vbNewLine
		strFilter &= If(m_SearchCriteria.FromYear > 0, String.Format("Jahr: {0}", m_SearchCriteria.FromYear), String.Empty)
		strFilter &= If(m_SearchCriteria.FromMonth.GetValueOrDefault(0) > 0, String.Format("Monat: {0}", m_SearchCriteria.FromYear), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.EmployeeNumberList), String.Format("{1}Kandidaten: {0}", m_SearchCriteria.EmployeeNumberList, vbNewLine), String.Empty)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLLohnKontiSearchPrintSetting With {.m_InitializationData = m_InitializationData, .LohnKontiData = m_LohnKontiData}

		_Setting.LohnKontiData = m_LohnKontiData
		_Setting.SQL2Open = Me.SQL4Print
		_Setting.PrintJobNumber = Me.PrintJobNr
		_Setting.frmhwnd = GetHwnd
		_Setting.ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})
		_Setting.ShowAsDesign = Me.bPrintAsDesign
		_Setting.SelectedYear = lueYear.EditValue
		_Setting.SelectedMonth = lueMonth.EditValue

		Dim obj As New SPS.Listing.Print.Utility.LohnKontiSearchListing.ClsPrintLohnKontiSearchList(m_InitializationData)
		obj.PrintData = _Setting

		obj.PrintLohnKontiSearchList()

	End Sub

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
				'Dim ExportThread As New Thread(AddressOf StartExportModul)
				'ExportThread.Name = "LOLOLKontiToCSV"
				'ExportThread.Start()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																			 .SQL2Open = String.Empty,
																																			 .ModulName = "LOLOLKontiToCSV"}
		If m_LohnKontiData Is Nothing Then Return
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		_Setting.ModulName = "LOLOLKontiToCSV"
		obj.ExportCSVFromLOKontiListing(m_LohnKontiData)

	End Sub


#End Region



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
		'LoadDropDownData()

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
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.xtabMain.Name).Controls
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


	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr.ButtonClick

		If e.Button.Index = 1 Then
			Dim cbo As ComboBoxEdit = CType(sender, ComboBoxEdit)
			cbo.EditValue = Nothing

			Return
		End If
		m_SearchCriteria.MDNr = lueMandant.EditValue
		m_SearchCriteria.FromYear = lueYear.EditValue
		m_SearchCriteria.FromMonth = lueMonth.EditValue

		Dim frmTest As New frmSearchRec(m_InitializationData, m_SearchCriteria)

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListeValue()
		Me.txt_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub onForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For i As Integer = 0 To GetExecutingAssembly.GetReferencedAssemblies.Count - 1
				strRAssembly &= String.Format("-->> {1}{0}", vbNewLine, GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim cbo As ComboBoxEdit = CType(sender, ComboBoxEdit)
				cbo.EditValue = Nothing
			End If

		End If

	End Sub





#Region "Helper Classes"


	''' <summary>
	''' Wraps an integer value.
	''' </summary>
	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class

	Private Sub txt_MANr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txt_MANr.SelectedIndexChanged

	End Sub


#End Region

End Class

