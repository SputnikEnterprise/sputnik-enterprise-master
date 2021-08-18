
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SP.MA.ApplicantMng.Settings
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure.Initialization
Imports DevExpress.XtraSplashScreen
Imports SP.MA.ApplicantMng.UI
Imports DevExpress.XtraEditors
Imports SPS.MainView.EmployeeSettings
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors.Controls

Public Class frmEmployeeQuickSearch

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' Contains the employee number of the loaded employee data.
	''' </summary>
	Private m_EmployeeNumber As Integer?

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

	Private connectionString As String

	Private GridSettingPath As String
	Private m_GVMAQuickSearchSettingfilename As String

	Private Property m_EmployeeImage As Image

#End Region


#Region "private consts"

	Private Const MODUL_NAME As String = "EmployeeQuickSearch"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_SettingsManager = New SettingsZEManager

		InitializeComponent()


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		m_mandant = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		Try
			Dim strModulName As String = MODUL_NAME

			m_GVMAQuickSearchSettingfilename = String.Format("{0}Employee\QuickSearch\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

		Catch ex As Exception

		End Try

		' Translate controls.
		TranslateControls()
		Reset()


		AddHandler gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
		AddHandler gvQuery.FocusedRowChanged, AddressOf OngvQuery_FocusedRowChanged

		AddHandler gvMain.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvMain.ColumnWidthChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler sccHeaderInfo.SizeChanged, AddressOf SaveFormProperties

	End Sub


#End Region


#Region "Private Properties"

	''' <summary>
	''' Gets the selected employee data.
	''' </summary>
	Private ReadOnly Property SelectedViewData As ExistingEmployeeSearchData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(grdView.GetRow(selectedRows(0)), ExistingEmployeeSearchData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedQueryViewData As SearchQueryTemplateData
		Get
			Dim grdView = TryCast(grdQuery.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim selectedValue = CType(grdView.GetRow(selectedRows(0)), SearchQueryTemplateData)
					Return selectedValue
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region


#Region "Public Methods"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadQuickSearchEmployeeAndAplicantList()
		success = success AndAlso LoadQuickSearchQueryList()


		Return success

	End Function


#End Region


#Region "Private Methods"


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		bsiLblRecCount.Caption = m_Translate.GetSafeTranslationValue(bsiLblRecCount.Caption)
		bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)
		bbiSMS.Caption = m_Translate.GetSafeTranslationValue(bbiSMS.Caption)

	End Sub


#Region "Reset grids"


	Private Sub Reset()

		ResetProposeGrid()
		ResetQueryTemplateGrid()

	End Sub

	''' <summary>
	''' reset quicksearch grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetProposeGrid()

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsFind.AllowFindPanel = False
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True
		gvMain.OptionsFind.AlwaysVisible = False

		gvMain.Columns.Clear()


		Try

			Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnEmployeeNumber.Name = "EmployeeNumber"
			columnEmployeeNumber.FieldName = "EmployeeNumber"
			columnEmployeeNumber.Visible = False
			gvMain.Columns.Add(columnEmployeeNumber)

			Dim columnEmployeeFullnameWithComma As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullnameWithComma.Caption = m_Translate.GetSafeTranslationValue("Nach- /Vorname")
			columnEmployeeFullnameWithComma.Name = "EmployeeFullnameWithComma"
			columnEmployeeFullnameWithComma.FieldName = "EmployeeFullnameWithComma"
			columnEmployeeFullnameWithComma.Visible = True
			columnEmployeeFullnameWithComma.Width = 250
			gvMain.Columns.Add(columnEmployeeFullnameWithComma)

			Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnStreet.Name = "Street"
			columnStreet.FieldName = "Street"
			columnStreet.Visible = True
			columnStreet.Width = 150
			gvMain.Columns.Add(columnStreet)

			Dim columnEmployeeAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnEmployeeAddress.Name = "EmployeeAddress"
			columnEmployeeAddress.FieldName = "EmployeeAddress"
			columnEmployeeAddress.Visible = True
			columnEmployeeAddress.Width = 200
			gvMain.Columns.Add(columnEmployeeAddress)

			Dim columnBirthdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBirthdate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBirthdate.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
			columnBirthdate.Name = "Birthdate"
			columnBirthdate.FieldName = "Birthdate"
			columnBirthdate.Visible = True
			columnBirthdate.Width = 100
			gvMain.Columns.Add(columnBirthdate)

			Dim columnEmail As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmail.Caption = m_Translate.GetSafeTranslationValue("E-Mail")
			columnEmail.Name = "Email"
			columnEmail.FieldName = "Email"
			columnEmail.Visible = True
			columnEmail.Width = 150
			gvMain.Columns.Add(columnEmail)

			Dim columnMobilePhone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMobilePhone.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMobilePhone.Caption = m_Translate.GetSafeTranslationValue("Mobile")
			columnMobilePhone.Name = "MobilePhone"
			columnMobilePhone.FieldName = "MobilePhone"
			columnMobilePhone.Visible = True
			columnMobilePhone.Width = 150
			gvMain.Columns.Add(columnMobilePhone)

			Dim columnMABusinessBranch As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMABusinessBranch.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMABusinessBranch.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnMABusinessBranch.Name = "MABusinessBranch"
			columnMABusinessBranch.FieldName = "MABusinessBranch"
			columnMABusinessBranch.Visible = True
			columnMABusinessBranch.Width = 100
			gvMain.Columns.Add(columnMABusinessBranch)

			Dim columnemployeeAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnemployeeAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnemployeeAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnemployeeAdvisor.Name = "employeeAdvisor"
			columnemployeeAdvisor.FieldName = "employeeAdvisor"
			columnemployeeAdvisor.Visible = True
			columnemployeeAdvisor.Width = 100
			gvMain.Columns.Add(columnemployeeAdvisor)

			Dim columnShowAsApplicant As New DevExpress.XtraGrid.Columns.GridColumn()
			columnShowAsApplicant.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnShowAsApplicant.Caption = m_Translate.GetSafeTranslationValue("Bewerber?")
			columnShowAsApplicant.Name = "ShowAsApplicant"
			columnShowAsApplicant.FieldName = "ShowAsApplicant"
			columnShowAsApplicant.Visible = True
			columnShowAsApplicant.Width = 100
			gvMain.Columns.Add(columnShowAsApplicant)


			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		grdMain.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the query template grid.
	''' </summary>
	Private Sub ResetQueryTemplateGrid()

		gvQuery.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvQuery.OptionsFind.AllowFindPanel = False
		gvQuery.OptionsView.ShowGroupPanel = False
		gvQuery.OptionsView.ShowIndicator = False
		gvQuery.OptionsView.ShowAutoFilterRow = False
		gvQuery.OptionsFind.AlwaysVisible = False
		gvQuery.OptionsView.ShowColumnHeaders = False
		gvQuery.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False

		gvQuery.Columns.Clear()


		Try

			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			gvQuery.Columns.Add(columnID)

			Dim columnMenuLabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMenuLabel.Caption = String.Empty
			columnMenuLabel.Name = "TranslatedLabel"
			columnMenuLabel.FieldName = "TranslatedLabel"
			columnMenuLabel.Visible = True
			columnMenuLabel.Width = 250
			gvQuery.Columns.Add(columnMenuLabel)

			Dim columnQueryString As New DevExpress.XtraGrid.Columns.GridColumn()
			columnQueryString.Caption = m_Translate.GetSafeTranslationValue("QueryString")
			columnQueryString.Name = "QueryString"
			columnQueryString.FieldName = "QueryString"
			columnQueryString.Visible = False
			gvQuery.Columns.Add(columnQueryString)

			Dim columnQueryType As New DevExpress.XtraGrid.Columns.GridColumn()
			columnQueryType.Caption = m_Translate.GetSafeTranslationValue("QueryType")
			columnQueryType.Name = "QueryType"
			columnQueryType.FieldName = "QueryType"
			columnQueryType.Visible = False
			gvQuery.Columns.Add(columnQueryType)

			Dim columnShowMenuIn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnShowMenuIn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnShowMenuIn.Caption = m_Translate.GetSafeTranslationValue("ShowMenuIn")
			columnShowMenuIn.Name = "ShowMenuIn"
			columnShowMenuIn.FieldName = "ShowMenuIn"
			columnShowMenuIn.Visible = False
			gvQuery.Columns.Add(columnShowMenuIn)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		grdQuery.DataSource = Nothing

	End Sub


#End Region


	Private Function LoadQuickSearchEmployeeAndAplicantList() As Boolean

		Dim lastname As String = String.Empty
		Dim firstname As String = String.Empty
		Dim street As String = String.Empty
		Dim postcode As String = String.Empty
		Dim location As String = String.Empty
		Dim countryCode As String = String.Empty
		Dim listOfPropose As IEnumerable(Of ExistingEmployeeSearchData) = Nothing

		Try

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			listOfPropose = m_EmployeeDatabaseAccess.LoadExistingEmployeesANDApplicantData(ModulConstants.MDData.MDNr)
			If listOfPropose Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Kandidaten-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt."))
				Return False
			End If

			Dim reportGridData = (From report In listOfPropose
														Select New ExistingEmployeeSearchData With
															{.MDNr = report.MDNr,
															.EmployeeNumber = report.EmployeeNumber,
															.Firstname = report.Firstname,
															.Lastname = report.Lastname,
															.Street = report.Street,
															.Postcode = report.Postcode,
															.Location = report.Location,
															.MobilePhone = report.MobilePhone,
															.Email = report.Email,
															.Birthdate = report.Birthdate,
															.MABusinessBranch = report.MABusinessBranch,
															.ShowAsApplicant = report.ShowAsApplicant,
															.ApplicantID = report.ApplicantID,
															.ApplicantLifecycle = report.ApplicantLifecycle,
															.CountryCode = report.CountryCode,
															.Gender = report.Gender,
															.Telephone_P = report.Telephone_P
															}).ToList()

			Dim listDataSource As BindingList(Of ExistingEmployeeSearchData) = New BindingList(Of ExistingEmployeeSearchData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdMain.DataSource = listDataSource
			bsiRecCount.Caption = listOfPropose.Count

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		Return Not listOfPropose Is Nothing
	End Function

	Private Function LoadQuickSearchQueryList() As Boolean

		Dim listOfQueries As IEnumerable(Of SearchQueryTemplateData) = Nothing

		Try
			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			listOfQueries = m_CommonDatabaseAccess.LoadSearchQueryTemplateData(ModulConstants.MDData.MDNr, "MainView")
			If listOfQueries Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog("Fehler in der Suche nach Vorlagen für Such-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")

				Return False
			End If


			grdQuery.DataSource = listOfQueries

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		Return Not listOfQueries Is Nothing
	End Function


	Private Function LoadQuickSearchQueryResultList(ByVal queryType As Integer, ByVal queryString As String) As Boolean

		Dim lastname As String = String.Empty
		Dim firstname As String = String.Empty
		Dim street As String = String.Empty
		Dim postcode As String = String.Empty
		Dim location As String = String.Empty
		Dim countryCode As String = String.Empty
		Dim listOfPropose As IEnumerable(Of ExistingEmployeeSearchData) = Nothing

		Try

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			If queryType = 0 Then
				listOfPropose = m_EmployeeDatabaseAccess.LoadQuickSearchDataWithStoredProcedure(ModulConstants.MDData.MDNr, queryString)
			Else
			End If

			If listOfPropose Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog("Fehler in der Kandidaten-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim reportGridData = (From report In listOfPropose
														Select New ExistingEmployeeSearchData With
															{.MDNr = report.MDNr,
															.EmployeeNumber = report.EmployeeNumber,
															.Firstname = report.Firstname,
															.Lastname = report.Lastname,
															.Street = report.Street,
															.Postcode = report.Postcode,
															.Location = report.Location,
															.MobilePhone = report.MobilePhone,
															.Email = report.Email,
															.Birthdate = report.Birthdate,
															.MABusinessBranch = report.MABusinessBranch,
															.ShowAsApplicant = report.ShowAsApplicant,
															.ApplicantID = report.ApplicantID,
															.ApplicantLifecycle = report.ApplicantLifecycle,
															.CountryCode = report.CountryCode,
															.Gender = report.Gender,
															.Telephone_P = report.Telephone_P
															}).ToList()

			Dim listDataSource As BindingList(Of ExistingEmployeeSearchData) = New BindingList(Of ExistingEmployeeSearchData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdMain.DataSource = listDataSource
			bsiRecCount.Caption = listDataSource.Count

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		Return Not listOfPropose Is Nothing
	End Function


#Region "form handler"

	Private Sub frmFoundedReports_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		SaveFromSettings()
	End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrmFoundedReports_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_QUICKSEARCH_FORM_WIDTH)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_QUICKSEARCH_FORM_HEIGHT)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingMAKeys.SETTING_QUICKSEARCH_FORM_LOCATION)
			Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_QUICKSEARCH_SCC_MAIN_SPLITTERPOSION)

			If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Saves the form settings.
	''' </summary>
	Private Sub SaveFromSettings()

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingMAKeys.SETTING_QUICKSEARCH_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_QUICKSEARCH_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_QUICKSEARCH_FORM_HEIGHT, Me.Height)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	Sub SaveFormProperties()

		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_QUICKSEARCH_SCC_MAIN_SPLITTERPOSION, sccHeaderInfo.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub


#End Region


#Region "Open modules"

	Private Sub OpenSelectedEmployee(ByVal mandantnumber As Integer, ByVal employeenumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, employeenumber)
		hub.Publish(openMng)

	End Sub

	Private Sub OpenSelectedApplicant(ByVal mandantnumber As Integer, ByVal applicantNumber As Integer)

		Try
			If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 101, mandantnumber) Then Exit Sub

			Dim frm As frmApplicant = CType(ModulConstants.GetModuleCach.GetModuleForm(mandantnumber, m_InitializationData.UserData.UserNr, SP.ModuleCaching.ModuleName.ApplicantMng), frmApplicant)

			frm.LoadEmployeeData(applicantNumber)
			If frm.IsEmployeeDataLoaded Then
				frm.Show()
				frm.BringToFront()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Private Function OpeneCallSMS() As Boolean
		Dim result As Boolean = True
		Dim sql As String

		Try

			Dim data = SelectedQueryViewData

			If Not data Is Nothing Then
				sql = data.QueryString

			Else

				Return False
			End If

			Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = ModulConstants.MDData,
																																					 .PersonalizedData = ModulConstants.ProsonalizedData,
																																					 .TranslationData = ModulConstants.TranslationData,
																																					 .UserData = ModulConstants.UserData}

			Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, String.Empty, SPS.Export.Listing.Utility.ReceiverType.Employee)

			Dim employeeData As BindingList(Of ExistingEmployeeSearchData) = CType(grdMain.DataSource, IEnumerable(Of ExistingEmployeeSearchData))
			Dim employeeDataRet_2 = From test In employeeData Where test.SMS_Mailing = 0
															Select test

			'Dim employeeDataRet = employeeData.Where(Function(x) (x.SMS_Mailing = False)).ToList()
			If employeeDataRet_2 Is Nothing Then Return False

			Dim listDataSource As BindingList(Of ExistingEmployeeSearchData) = New BindingList(Of ExistingEmployeeSearchData)

			For Each p In employeeData
				listDataSource.Add(p)
			Next
			Dim employeeDataRet = listDataSource.Where(Function(x) (x.SMS_Mailing = False))


			frmSMS2eCall.QuickSearchData = listDataSource ' CType(employeeDataRet_2, BindingList(Of ExistingEmployeeSearchData))
			frmSMS2eCall.LoadData()

			frmSMS2eCall.Show()
			frmSMS2eCall.BringToFront()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return 0

	End Function


#End Region

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			Dim data = SelectedViewData()

			If Not data Is Nothing Then

				Select Case data.ShowAsApplicant.GetValueOrDefault(False)
					Case True
						OpenSelectedApplicant(data.MDNr, data.EmployeeNumber)

					Case Else
						OpenSelectedEmployee(data.MDNr, data.EmployeeNumber)

				End Select

			End If

		End If

	End Sub

	Private Sub OngvQuery_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)

		Try
			Dim data = SelectedQueryViewData

			If Not data Is Nothing Then
				LoadQuickSearchQueryResultList(data.QueryType, data.QueryString)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(ex.ToString)

		End Try

	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		If gvMain.RowCount > 0 Then
			' Opens the Preview window. 
			grdMain.ShowPrintPreview()
		End If
	End Sub

	Private Sub OnbbiSMS_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSMS.ItemClick
		OpeneCallSMS()
	End Sub



#End Region





#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			If File.Exists(m_GVMAQuickSearchSettingfilename) Then gvMain.RestoreLayoutFromXml(m_GVMAQuickSearchSettingfilename)

			If restoreLayout AndAlso Not keepFilter Then gvMain.ActiveFilterCriteria = Nothing

		Catch ex As Exception

		End Try

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMAQuickSearchSettingfilename)

	End Sub


	Private Sub OngvEmployeeProperty_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvMain.RowCount)

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

#End Region


End Class
