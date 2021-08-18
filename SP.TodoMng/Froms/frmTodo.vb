
Imports System.Reflection.Assembly
Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.TodoMng
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.TodoMng.Settings
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Vacancy
Imports SP.DatabaseAccess.Propose
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports SP.DatabaseAccess.Propose.DataObjects
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports System.IO
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraRichEdit.API.Native

''' <summary>
''' Todo Management.
''' </summary>
Public Class frmTodo

#Region "Private Fields"

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

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CustomerDataAccess As ICustomerDatabaseAccess
	Private m_VacancyDataAccess As IVacancyDatabaseAccess
	Private m_ProposeDataAccess As IProposeDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	Private m_IsInitialDataLoaded As Boolean = False

	''' <summary>
	''' Mandant data.
	''' </summary>
	Private m_Mandant As Mandant

	''' <summary>
	''' The actual todo data
	''' </summary>
	''' <remarks></remarks>
	Private m_todoData As TodoData
	Private m_ToDoUserData As IEnumerable(Of TodoUserData)

	''' <summary>
	''' The actual todo data
	''' </summary>
	''' <remarks></remarks>
	Private m_defaultTodoData As TodoData

	''' <summary>
	''' User filter setting.
	''' </summary>
	Private m_UserFilterSetting As String = String.Empty
	Private m_ModulInput As SourceInput


	Private GridSettingPath As String
	Private m_GVTODOSettingfilename As String


#End Region

#Region "Contructor"

	''' <summary>
	''' The consturctor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Mandant = New Mandant
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_Mandant.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDataAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_VacancyDataAccess = New VacancyDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ProposeDataAccess = New ProposeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		m_defaultTodoData = New TodoData()
		m_defaultTodoData.UserNumber = m_InitializationData.UserData.UserNr
		m_todoData = m_defaultTodoData.Clone()


		Try

			GridSettingPath = String.Format("{0}TODOMng\", m_Mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
			If Not Directory.Exists(GridSettingPath) Then Directory.CreateDirectory(GridSettingPath)
			If Not Directory.Exists(String.Format("{0}Properties\", GridSettingPath)) Then Directory.CreateDirectory(String.Format("{0}Properties\", GridSettingPath))

			m_GVTODOSettingfilename = String.Format("{0}Properties\TODOMng{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

			Reset()

			Dim settingOnlyImportant = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_FILTER_ONLY_IMPORTANT)
			Dim settingExcludeFinished = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_FILTER_EXCLUDE_FINISHED)
			m_UserFilterSetting = m_SettingsManager.ReadString(SettingKeys.SETTING_FILTER_CHECKED_USERS)
			m_UserFilterSetting = m_InitializationData.UserData.UserNr

			chkFilterOnlyImportant.Checked = settingOnlyImportant
			chkFilterExcludeFinished.Checked = settingExcludeFinished

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		TranslateControls()

		AddHandler cbbFilterUser.EditValueChanged, AddressOf FilterChanged
		AddHandler chkFilterOnlyImportant.EditValueChanged, AddressOf FilterChanged
		AddHandler chkFilterExcludeFinished.EditValueChanged, AddressOf FilterChanged

		AddHandler lueUSNr.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler dateEditScheduleBegins.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler dateEditScheduleEnds.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueModul.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler gvTODO.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvTODO.ColumnWidthChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvTODO.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler gvTODO.ColumnFilterChanged, AddressOf OngvColumnFilterChanged

	End Sub

#End Region


#Region "Public Properties"

	Public Property ToDoIDNumber As Integer?
	Public Property UserNumber As Integer
	Public Property Subject As String
	Public Property Body As String
	Public Property EmployeeNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property ResponsiblePersonRecordNumber As Integer?
	Public Property VacancyNumber As Integer?
	Public Property ProposeNumber As Integer?
	Public Property ESNumber As Integer?
	Public Property RPNumber As Integer?
	Public Property LMNumber As Integer?
	Public Property RENumber As Integer?
	Public Property ZENumber As Integer?


	''' <summary>
	''' Gets the first TODO view data.
	''' </summary>
	''' <returns>First TODO view data or nothing.</returns>
	Public ReadOnly Property FirstTodoViewDataInList As TodoListViewData
		Get
			If gvTODO.RowCount > 0 Then

				Dim rowHandle = gvTODO.GetVisibleRowHandle(0)
				Return CType(gvTODO.GetRow(rowHandle), TodoListViewData)
			Else
				Return Nothing
			End If

		End Get
	End Property

#End Region


#Region "Public Methods"

	Public Sub InitNewTodo(
		ByVal usnr As Integer, ByVal betreff As String, ByVal text As String,
		ByVal MANr As Integer?,
		ByVal KDNr As Integer?,
		ByVal zhdRecordNumber As Integer?,
		ByVal VakNr As Integer?,
		ByVal PNr As Integer?,
		ByVal ESNr As Integer?,
		ByVal RPNr As Integer?,
		ByVal LMNr As Integer?,
		ByVal RENr As Integer?,
		ByVal ZENr As Integer?)

		LoadData()

		'm_todoData = New TodoData() With {
		'	.UserNumber = UserNumber,
		'	.EmployeeNumber = EmployeeNumber,
		'	.CustomerNumber = CustomerNumber,
		'	.ResponsiblePersonRecordNumber = ResponsiblePersonRecordNumber,
		'	.VacancyNumber = VacancyNumber,
		'	.ProposeNumber = ProposeNumber,
		'	.ESNumber = ESNumber,
		'	.RPNumber = RPNumber,
		'	.LMNumber = LMNumber,
		'	.RENumber = RENumber,
		'	.ZENumber = ZENumber,
		'	.Subject = Subject,
		'	.Body = Body}

		'm_defaultTodoData = m_todoData.Clone()

	End Sub

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		' Init Filter
		success = success AndAlso LoadTodoUserDropDownData()

		' Init LookupEdits
		success = success AndAlso LoadUserDropDownData()
		success = success AndAlso LoadTodoListData()

		If ToDoIDNumber Is Nothing Then
			m_todoData = Nothing
			BlankTODOFields()

			lueModul.Visible = False
			lblModulName.Visible = False

			'success = success AndAlso PrepareForm()

		Else
			LoadTodoData(ToDoIDNumber)
			FocusTodoData(ToDoIDNumber)

		End If

		Return success
	End Function


#End Region

#Region "private properties"

	Private ReadOnly Property SelectedTODORecord As TodoData
		Get
			Dim gvData = TryCast(grdTODO.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim data = CType(gvData.GetRow(selectedRows(0)), TodoData)
					Return data
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedTODOUserRecord As TodoUserData
		Get
			Dim gvData = TryCast(grdDependentUsers.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim data = CType(gvData.GetRow(selectedRows(0)), TodoUserData)
					Return data
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region

#Region "Private Methods"

	''' <summary>
	'''  Translate controls.
	''' </summary>
	Private Sub TranslateControls()
		' Translate controls
		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
		btnNew.Text = m_Translate.GetSafeTranslationValue(Me.btnNew.Text)
		btnDelete.Text = m_Translate.GetSafeTranslationValue(Me.btnDelete.Text)

		lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text)
		lblBeginnt.Text = m_Translate.GetSafeTranslationValue(Me.lblBeginnt.Text)
		lblEndet.Text = m_Translate.GetSafeTranslationValue(Me.lblEndet.Text)
		chkRemember.Text = m_Translate.GetSafeTranslationValue(Me.chkRemember.Text)

		lblBetreff.Text = m_Translate.GetSafeTranslationValue(Me.lblBetreff.Text)
		lblBeschreibung.Text = m_Translate.GetSafeTranslationValue(Me.lblBeschreibung.Text)

		bsiAnzahlDatensaetzelbl.Caption = m_Translate.GetSafeTranslationValue(bsiAnzahlDatensaetzelbl.Caption)

		bsiErstelltlbl.Caption = m_Translate.GetSafeTranslationValue(bsiErstelltlbl.Caption)
		bsiGeaendertlbl.Caption = m_Translate.GetSafeTranslationValue(bsiGeaendertlbl.Caption)

	End Sub

	Private Function PrepareForm() As Boolean

		Dim success As Boolean = True

		If Not m_IsInitialDataLoaded Then
			m_IsInitialDataLoaded = True
		End If

		' Reset the form
		'Reset()

		'm_EmployeeNumber = EmployeeNumber
		If Not EmployeeNumber Is Nothing Then
			Dim employeeData = LoadEmployeeDropDownData(EmployeeNumber)
			lueModul.Properties.DataSource = employeeData

			lueModul.EditValue = EmployeeNumber
		End If

		If Not CustomerNumber Is Nothing Then
			Dim customerData = LoadCustomerDropDownData(CustomerNumber)
			lueModul.Properties.DataSource = customerData

			lueModul.EditValue = CustomerNumber

			If Not ResponsiblePersonRecordNumber Is Nothing Then
				Dim zhdData = LoadResponsiblePersonDropDownData(CustomerNumber, ResponsiblePersonRecordNumber)
				lueModul.Properties.DataSource = zhdData

				lueModul.EditValue = ResponsiblePersonRecordNumber
			End If
		End If

		If Not ProposeNumber Is Nothing Then
			Dim customerData = LoadProposeDropDownData(ProposeNumber)
			lueModul.Properties.DataSource = ProposeNumber
		End If


		'Dim initalYearFilter As Integer() = Nothing

		'If Not contactFilterSettings Is Nothing Then

		'	m_SuppressUIEvents = True
		'	chkTelephone.Checked = contactFilterSettings.ExcluePhone
		'	chkMailed.Checked = contactFilterSettings.ExclueMail
		'	chkOffered.Checked = contactFilterSettings.ExclueOffered
		'	chkSMS.Checked = contactFilterSettings.ExcludeSMS
		'	m_SuppressUIEvents = False

		'	initalYearFilter = contactFilterSettings.Years
		'End If

		'success = success AndAlso LoadYearsListData(EmployeeNumber, initalYearFilter)
		'success = success AndAlso FilterContactData(EmployeeNumber)

		Return success

	End Function

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		If (Not advisorPicture.Image Is Nothing) Then
			advisorPicture.Image.Dispose()
		End If
		chkFilterExcludeFinished.Visible = False
		chkFilterOnlyImportant.Visible = False

		EmployeeNumber = Nothing
		CustomerNumber = Nothing
		ResponsiblePersonRecordNumber = Nothing
		VacancyNumber = Nothing
		ProposeNumber = Nothing
		ESNumber = Nothing
		RPNumber = Nothing

		advisorPicture.Image = Nothing
		advisorPicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")
		advisorPicture.Properties.ShowMenu = False
		advisorPicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

		bsiRecCount.Caption = "0"
		bsiChangedOn.Caption = String.Empty
		bsiChangedOn.Caption = String.Empty

		rtfBody.Options.Export.Html.DefaultCharacterPropertiesExportToCss = False
		Dim fontName As Font = New Font("Tahoma", 8.25, FontStyle.Regular)
		rtfBody.Font = fontName

		txtSubject.Properties.MaxLength = 250

		' ---Reset drop downs, grids and lists---
		ResetModulDropDown()

		ResetTodoGrid()
		ResetDependentUsersGrid()
		ResetDependentUsersDropDown()

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the modul drop down.
	''' </summary>
	Private Sub ResetModulDropDown()
		lueModul.Properties.DisplayMember = "ModulViewdata"
		lueModul.Properties.ValueMember = "ModulNumber"

		Dim columns = lueModul.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("ModulViewdata", 0))

		lueModul.Properties.ShowHeader = False
		lueModul.Properties.ShowFooter = False
		lueModul.Properties.DropDownRows = 10
		lueModul.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueModul.Properties.SearchMode = SearchMode.AutoComplete
		lueModul.Properties.AutoSearchColumnIndex = 0

		lueModul.Properties.NullText = String.Empty
		lueModul.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the TODO grid.
	''' </summary>
	Private Sub ResetTodoGrid()

		' Reset the grid
		gvTODO.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvTODO.OptionsView.ShowGroupPanel = False
		gvTODO.OptionsView.ShowAutoFilterRow = True
		gvTODO.OptionsView.ShowIndicator = False

		gvTODO.Columns.Clear()

		Dim columnAppointmentDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAppointmentDate.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnAppointmentDate.Name = "ID"
		columnAppointmentDate.FieldName = "ID"
		columnAppointmentDate.Visible = False
		gvTODO.Columns.Add(columnAppointmentDate)

		Dim columnJobTitle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobTitle.Caption = m_Translate.GetSafeTranslationValue("Benutzer-Nr.")
		columnJobTitle.Name = "UserNumber"
		columnJobTitle.FieldName = "UserNumber"
		columnJobTitle.Visible = False
		gvTODO.Columns.Add(columnJobTitle)

		Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnCompany.Name = "EmployeeNumber"
		columnCompany.FieldName = "EmployeeNumber"
		columnCompany.Visible = False
		gvTODO.Columns.Add(columnCompany)

		Dim columnJobAppointmentState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobAppointmentState.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnJobAppointmentState.Name = "CustomerNumber"
		columnJobAppointmentState.FieldName = "CustomerNumber"
		columnJobAppointmentState.Visible = False
		gvTODO.Columns.Add(columnJobAppointmentState)

		Dim columnSchedulebegins As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSchedulebegins.Caption = m_Translate.GetSafeTranslationValue("Beginnt")
		columnSchedulebegins.Name = "Schedulebegins"
		columnSchedulebegins.FieldName = "Schedulebegins"
		columnSchedulebegins.AppearanceHeader.Options.UseTextOptions = True
		columnSchedulebegins.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnSchedulebegins.DisplayFormat.FormatString = "g"
		columnSchedulebegins.Visible = True
		columnSchedulebegins.Width = 200
		gvTODO.Columns.Add(columnSchedulebegins)

		Dim columnScheduleends As New DevExpress.XtraGrid.Columns.GridColumn()
		columnScheduleends.Caption = m_Translate.GetSafeTranslationValue("Endet am")
		columnScheduleends.Name = "Scheduleends"
		columnScheduleends.FieldName = "Scheduleends"
		columnScheduleends.AppearanceHeader.Options.UseTextOptions = True
		columnScheduleends.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnScheduleends.DisplayFormat.FormatString = "g"
		columnScheduleends.Visible = False
		gvTODO.Columns.Add(columnScheduleends)


		Dim columnSubject As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSubject.Caption = m_Translate.GetSafeTranslationValue("Betreff")
		columnSubject.Name = "Subject"
		columnSubject.FieldName = "Subject"
		columnSubject.Visible = True
		columnSubject.Width = 300
		gvTODO.Columns.Add(columnSubject)

		Dim columnImportant As New DevExpress.XtraGrid.Columns.GridColumn()
		columnImportant.Caption = m_Translate.GetSafeTranslationValue("Wichtig")
		columnImportant.Name = "IsImportant"
		columnImportant.FieldName = "IsImportant"
		columnImportant.Visible = True
		columnImportant.Width = 60
		gvTODO.Columns.Add(columnImportant)

		Dim columnIsDueNow As New DevExpress.XtraGrid.Columns.GridColumn()
		columnIsDueNow.Caption = m_Translate.GetSafeTranslationValue("Fällig")
		columnIsDueNow.Name = "IsDueNow"
		columnIsDueNow.FieldName = "IsDueNow"
		columnIsDueNow.Visible = True
		columnIsDueNow.Width = 60
		gvTODO.Columns.Add(columnIsDueNow)

		Dim columnIsSystemNotification As New DevExpress.XtraGrid.Columns.GridColumn()
		columnIsSystemNotification.Caption = m_Translate.GetSafeTranslationValue("System-Nachrichten")
		columnIsSystemNotification.Name = "IsSystemNotification"
		columnIsSystemNotification.FieldName = "IsSystemNotification"
		columnIsSystemNotification.Visible = True
		columnIsSystemNotification.Width = 60
		gvTODO.Columns.Add(columnIsSystemNotification)

		Dim columnCompleted As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompleted.Caption = m_Translate.GetSafeTranslationValue("Erledigt")
		columnCompleted.Name = "IsCompleted"
		columnCompleted.FieldName = "IsCompleted"
		columnCompleted.Visible = True
		columnCompleted.Width = 60
		gvTODO.Columns.Add(columnCompleted)


		RestoreGridLayoutFromXml()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		grdTODO.DataSource = Nothing
		m_SuppressUIEvents = suppressUIEventsState



	End Sub

	Private Sub ResetDependentUsersGrid()

		' Reset the grid
		gvDependentUsers.OptionsView.ShowIndicator = False

		gvDependentUsers.Columns.Clear()

		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		gvDependentUsers.Columns.Add(columnID)

		Dim columnUserNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUserNumber.Caption = m_Translate.GetSafeTranslationValue("Benutzer-Nr.")
		columnUserNumber.Name = "UserNumber"
		columnUserNumber.FieldName = "UserNumber"
		columnUserNumber.Visible = False
		gvDependentUsers.Columns.Add(columnUserNumber)

		Dim columnFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFullname.Caption = m_Translate.GetSafeTranslationValue("Benutzer")
		columnFullname.Name = "Fullname"
		columnFullname.FieldName = "Fullname"
		columnFullname.Visible = True
		gvDependentUsers.Columns.Add(columnFullname)

		Dim columnDone As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDone.Caption = m_Translate.GetSafeTranslationValue("Erledigt")
		columnDone.Name = "Done"
		columnDone.FieldName = "Done"
		columnDone.Visible = True
		gvDependentUsers.Columns.Add(columnDone)


		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		grdDependentUsers.DataSource = Nothing
		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub ResetDependentUsersDropDown()

		lueDependentUsers.Properties.DisplayMember = "Fullname"
		lueDependentUsers.Properties.ValueMember = "UsrNr"

		Dim columns = lueDependentUsers.Properties.Columns
		columns.Clear()
		'columns.Add(New LookUpColumnInfo("UsrNr", 0))
		columns.Add(New LookUpColumnInfo("Fullname", 0, "Name"))

		lueDependentUsers.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueDependentUsers.Properties.SearchMode = SearchMode.AutoComplete
		lueDependentUsers.Properties.AutoSearchColumnIndex = 1

		lueDependentUsers.Properties.NullText = String.Empty
		lueDependentUsers.EditValue = Nothing

	End Sub


	''' <summary>
	''' Loads the Todo list data
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function LoadTodoListData() As Boolean

		Dim items As String = cbbFilterUser.EditValue
		items = items.Replace(cbbFilterUser.Properties.SeparatorChar, ",")
		If String.IsNullOrWhiteSpace(items) Then items = String.Format("{0}", m_InitializationData.UserData.UserNr)

		Dim todoDataList = m_EmployeeDatabaseAccess.LoadTodoListDataBySearchCriteria(m_InitializationData.MDData.MDGuid, items, m_InitializationData.UserData.UserNr)  ', chkFilterOnlyImportant.Checked, chkFilterExcludeFinished.Checked)

		If (todoDataList Is Nothing) Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("TODO-Daten konnten nicht geladen werden."))

			Return False
		End If

		Dim listDataSource As BindingList(Of TodoListViewData) = New BindingList(Of TodoListViewData)

		' Convert the data to view data.
		For Each db In todoDataList

			Dim viewData = New TodoListViewData() With {
				.ID = db.ID,
				.UserNumber = db.UserNumber,
				.EmployeeNumber = db.EmployeeNumber,
				.CustomerNumber = db.CustomerNumber,
				.Subject = db.Subject,
				.IsImportant = db.IsImportant,
				.IsCompleted = db.IsCompleted,
				.SourceInput = db.TODOSourceEnum,
				.Schedulebegins = db.Schedulebegins,
				.Scheduleends = db.Scheduleends}

			listDataSource.Add(viewData)
		Next

		bsiRecCount.Caption = String.Format("{0}", todoDataList.Count)

		m_SuppressUIEvents = True
		grdTODO.DataSource = listDataSource
		m_SuppressUIEvents = False

		Return True
	End Function

	''' <summary>
	''' Create a empty new Todo data.
	''' </summary>
	''' <param name="todoID"></param>
	''' <remarks></remarks>
	Public Sub LoadTodoData(ByVal todoID As Integer?)

		If Not todoID Is Nothing Then
			m_todoData = m_EmployeeDatabaseAccess.LoadTodoData(m_InitializationData.MDData.MDGuid, todoID, m_InitializationData.UserData.UserNr)
		End If

		If m_todoData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("TODO-Daten konnten nicht geladen werden."))

			Return
		End If

		lueUSNr.EditValue = m_todoData.UserNumber
		txtSubject.EditValue = m_todoData.Subject
		rtfBody.HtmlText = m_todoData.Body

		chkImportant.Checked = m_todoData.IsImportant.GetValueOrDefault(False)
		chkFinished.Checked = m_todoData.IsCompleted.GetValueOrDefault(False)
		chkAllUsers.Checked = m_todoData.AllUsers.GetValueOrDefault(False)

		dateEditScheduleBegins.EditValue = m_todoData.Schedulebegins
		timeScheduleBegins.EditValue = m_todoData.Schedulebegins
		dateEditScheduleEnds.EditValue = m_todoData.Scheduleends
		timeScheduleEnds.EditValue = m_todoData.Scheduleends

		btnDelete.Enabled = m_todoData.TODOSourceEnum = TODOEnum.CREATED_MANUALLY

		m_todoData.ScheduleRememberIn = Nothing
		m_todoData.ScheduleRemember = Nothing

		LoadModulDropDownData()

		m_ToDoUserData = LoadToDoUsersData(m_todoData.ID)

		bsiCreatedOn.Caption = String.Format("{0:G}, {1}", m_todoData.CreatedOn, m_todoData.CreatedFrom)
		bsiChangedOn.Caption = String.Format("{0:G}, {1}", m_todoData.ChangedOn, m_todoData.ChangedFrom)

	End Sub

	Private Function LoadModulDropDownData() As Boolean
		Dim result As Boolean = True
		Dim modulNumber As Integer?
		Dim modulData As New List(Of LookupViewData)

		lblModulName.Visible = False
		lueModul.Visible = False

		If m_todoData.ProposeNumber.GetValueOrDefault(0) > 0 Then
			lblModulName.Text = m_Translate.GetSafeTranslationValue("Vorschlag")
			m_ModulInput = SourceInput.PROPOSE
			modulNumber = m_todoData.ProposeNumber
			modulData = LoadProposeDropDownData(modulNumber)

		ElseIf m_todoData.VacancyNumber.GetValueOrDefault(0) > 0 Then
			lblModulName.Text = m_Translate.GetSafeTranslationValue("Vakanz")
			m_ModulInput = SourceInput.VACANCY
			modulNumber = m_todoData.VacancyNumber
			modulData = LoadVacancyDropDownData(modulNumber)

		ElseIf m_todoData.EmployeeNumber.GetValueOrDefault(0) > 0 Then
			lblModulName.Text = m_Translate.GetSafeTranslationValue("Kandidat")
			m_ModulInput = SourceInput.EMPLOYEE
			modulNumber = m_todoData.EmployeeNumber
			modulData = LoadEmployeeDropDownData(modulNumber)

		ElseIf m_todoData.CustomerNumber.GetValueOrDefault(0) > 0 AndAlso m_todoData.ResponsiblePersonRecordNumber.GetValueOrDefault(0) = 0 Then
			lblModulName.Text = m_Translate.GetSafeTranslationValue("Kunde")
			m_ModulInput = SourceInput.CUSTOMER
			modulNumber = m_todoData.CustomerNumber
			modulData = LoadCustomerDropDownData(modulNumber)

		ElseIf m_todoData.ResponsiblePersonRecordNumber.GetValueOrDefault(0) > 0 Then
			lblModulName.Text = m_Translate.GetSafeTranslationValue("Zuständige Person")
			m_ModulInput = SourceInput.RESPONSIBLE_PERSON
			modulNumber = m_todoData.ResponsiblePersonRecordNumber
			modulData = LoadResponsiblePersonDropDownData(m_todoData.CustomerNumber, m_todoData.ResponsiblePersonRecordNumber)

		Else
			Return False

		End If

		lueModul.Properties.DataSource = modulData
		lueModul.EditValue = modulNumber

		lblModulName.Visible = modulNumber.GetValueOrDefault(0) > 0
		lueModul.Visible = modulNumber.GetValueOrDefault(0) > 0


		Return result
	End Function

	Private Function LoadToDoUsersData(ByVal todoID As Integer) As IEnumerable(Of TodoUserData)
		Dim result As IEnumerable(Of TodoUserData) = Nothing

		Try
			result = m_EmployeeDatabaseAccess.LoadTodoUserData(todoID)
			If result Is Nothing Then
				m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Benutzerangaben wurden gefunden."))

				Return Nothing
			End If

			' is user owner?
			If m_todoData.UserNumber <> m_InitializationData.UserData.UserNr Then
				Dim userAssignedlist = result.Where(Function(x) x.UserNumber = m_InitializationData.UserData.UserNr).ToList
				If Not userAssignedlist Is Nothing Then
					result = userAssignedlist
				End If
				btnDelete.Enabled = False
				btnDependentUsers.Enabled = False

			End If
			LockControls(m_todoData.UserNumber = m_InitializationData.UserData.UserNr)

			m_ToDoUserData = result
			grdDependentUsers.DataSource = m_ToDoUserData
			FocusTodoUsersData(m_todoData.UserNumber)

		Catch ex As Exception

		End Try

		Return result
	End Function

	Private Sub LockControls(ByVal lock As Boolean)

		lueUSNr.Enabled = lock
		lueModul.Enabled = lock
		txtSubject.Enabled = lock
		rtfBody.Enabled = lock
		chkImportant.Enabled = lock
		chkFinished.Enabled = m_todoData.UserNumber = m_InitializationData.UserData.UserNr OrElse m_todoData.AllUsers.GetValueOrDefault(False)

		chkAllUsers.Enabled = lock

		dateEditScheduleBegins.Enabled = lock
		timeScheduleBegins.Enabled = lock
		dateEditScheduleEnds.Enabled = lock
		timeScheduleEnds.Enabled = lock

		btnDelete.Enabled = lock
		btnDependentUsers.Enabled = lock

	End Sub

	''' <summary>
	''' Handles click on save button.
	''' </summary>
	Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
		Dim success As Boolean = True

		If ValidateContactInputData() Then
			success = success AndAlso SaveTodoData()

			If success Then
				LoadTodoUserDropDownData()
				LoadTodoListData()
				LoadToDoUsersData(m_todoData.ID)

				FocusTodoData(m_todoData.ID)
			End If

		End If

	End Sub

	Private Function ValidateContactInputData() As Boolean
		Return True
	End Function

	''' <summary>
	''' Saves a TodoData (Insert or Update)
	''' </summary>
	''' <remarks></remarks>
	Private Function SaveTodoData() As Boolean
		Dim success As Boolean = True
		Dim saveNewrecord As Boolean = m_todoData Is Nothing

		If saveNewrecord Then m_todoData = New TodoData
		If lueUSNr.EditValue Is Nothing Then
			m_todoData.UserNumber = m_InitializationData.UserData.UserNr
		Else
			m_todoData.UserNumber = ParseToIntOrNothing(lueUSNr.EditValue)
		End If

		m_todoData.TU_UserNumber = m_InitializationData.UserData.UserNr

		m_todoData.Subject = txtSubject.EditValue
		m_todoData.Body = rtfBody.HtmlText
		m_todoData.IsImportant = chkImportant.Checked
		m_todoData.IsCompleted = chkFinished.Checked
		m_todoData.AllUsers = chkAllUsers.Checked

		m_todoData.Schedulebegins = CombineDateAndTime(dateEditScheduleBegins.EditValue, timeScheduleBegins.EditValue)
		m_todoData.Scheduleends = CombineDateAndTime(dateEditScheduleEnds.EditValue, timeScheduleEnds.EditValue)

		m_todoData.ScheduleRememberIn = Nothing
		m_todoData.ScheduleRemember = Nothing

		If saveNewrecord Then
			m_todoData.EmployeeNumber = EmployeeNumber
			m_todoData.CustomerNumber = CustomerNumber
			m_todoData.ResponsiblePersonRecordNumber = ResponsiblePersonRecordNumber
			m_todoData.VacancyNumber = VacancyNumber
			m_todoData.ProposeNumber = ProposeNumber
			m_todoData.ESNumber = ESNumber
			m_todoData.RPNumber = RPNumber

			m_todoData.CreatedOn = DateTime.Now
			m_todoData.CreatedFrom = m_InitializationData.UserData.UserFullName
		Else
			m_todoData.ChangedOn = DateTime.Now
			m_todoData.ChangedFrom = m_InitializationData.UserData.UserFullName
		End If

		If saveNewrecord Then
			success = m_EmployeeDatabaseAccess.InsertTodoData(m_InitializationData.MDData.MDGuid, m_todoData)
		Else
			success = m_EmployeeDatabaseAccess.UpdateTodoData(m_InitializationData.MDData.MDGuid, m_todoData)
		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

		Else
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

			bsiCreatedOn.Caption = String.Format("{0:f}, {1}", m_todoData.CreatedOn, m_todoData.CreatedFrom)
			bsiChangedOn.Caption = String.Format("{0:f}, {1}", m_todoData.ChangedOn, m_todoData.ChangedFrom)
		End If

		Return success
	End Function

	''' <summary>
	''' Handles click on new button.
	''' </summary>
	Private Sub OnBtnNew_Click(sender As System.Object, e As System.EventArgs) Handles btnNew.Click

		BlankTODOFields()
		m_todoData = Nothing ' m_defaultTodoData.Clone()
		'LoadTodoData(Nothing)
	End Sub

	Private Sub BlankTODOFields()

		lueUSNr.EditValue = m_InitializationData.UserData.UserNr
		LoadUserImage()

		txtSubject.EditValue = String.Empty
		rtfBody.HtmlText = String.Empty

		chkImportant.Checked = True
		chkFinished.Checked = False
		chkAllUsers.Checked = False

		dateEditScheduleBegins.EditValue = Nothing
		timeScheduleBegins.EditValue = Nothing
		dateEditScheduleEnds.EditValue = Nothing
		timeScheduleEnds.EditValue = Nothing

		btnDelete.Enabled = False
		btnDependentUsers.Enabled = False

		grdDependentUsers.DataSource = Nothing
		bsiChangedOn.Caption = String.Empty
		bsiChangedOn.Caption = String.Empty

	End Sub

	''' <summary>
	''' Handles click on delete button.
	''' </summary>
	Private Sub OnBtnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
		Dim success As Boolean = True

		If m_todoData Is Nothing Then Return

		If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																									m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then Return

		success = success AndAlso m_EmployeeDatabaseAccess.DeleteTodo(m_todoData.ID)
		If success Then
			Dim msg = "Der ausgewählte Datensatz wurde erfolgreich gelöscht."
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg),
																	 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)

			Dim selectedRows = gvTODO.GetSelectedRows()

			LoadTodoUserDropDownData()

			LoadTodoListData()

			Dim firstToDoData = FirstTodoViewDataInList

			If Not firstToDoData Is Nothing Then LoadTodoData(firstToDoData.ID)

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Datensatz konnte nicht gelöscht werden."))

			Return
		End If

	End Sub

	Private Sub OngvDependentUsers_KeyDown(sender As Object, e As KeyEventArgs) Handles gvDependentUsers.KeyDown
		If (e.KeyCode = Keys.Delete) Then
			Dim success = DeleteTODOUserData()

			If success Then
				LoadToDoUsersData(m_todoData.ID)
			End If

		End If

	End Sub

	Private Function DeleteTODOUserData() As Boolean
		Dim success As Boolean = True
		Dim msg As String = "Sind Sie sicher dass Sie den Benutzer entfernen möchten?"

		success = success AndAlso m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), "Benutzer entfernen")
		If Not success Then Return False

		Dim data = SelectedTODOUserRecord()
		If data Is Nothing OrElse data.UserNumber = lueUSNr.EditValue Then
			msg = "Löschen eigener Benutzer ist nicht möglich."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg), "Benutzer entfernen")

			Return False
		End If
		success = success AndAlso m_EmployeeDatabaseAccess.DeleteTodoUserData(data.ID, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)

		Return success
	End Function

	Private Sub OnbtnDependentUsers_Click(sender As Object, e As EventArgs) Handles btnDependentUsers.Click

		pccUsers.SuspendLayout()
		Me.pccUsers.Manager = New DevExpress.XtraBars.BarManager
		pccUsers.ShowCloseButton = True
		pccUsers.ShowSizeGrip = True

		Dim userData = m_CustomerDataAccess.LoadUserData()
		lueDependentUsers.Properties.DataSource = userData

		pccUsers.ShowPopup(Cursor.Position)
		pccUsers.ResumeLayout()

	End Sub

	''' <summary>
	''' Loads Filer Todo User Data drop down data.
	''' </summary>
	Private Function LoadTodoUserDropDownData() As Boolean

		Dim userTodoData = m_EmployeeDatabaseAccess.LoadUserDataFromTODOList(m_InitializationData.MDData.MDGuid)

		cbbFilterUser.Properties.DataSource = userTodoData

		If Not String.IsNullOrEmpty(m_UserFilterSetting) Then

			Dim previousSuppressState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			cbbFilterUser.SetEditValue(m_UserFilterSetting)
			m_SuppressUIEvents = previousSuppressState

			m_UserFilterSetting = String.Empty
		End If

		Return Not userTodoData Is Nothing
	End Function

	''' <summary>
	''' Loads User (USNr) drop down data.
	''' </summary>
	Private Function LoadUserDropDownData() As Boolean

		Dim userData = m_CustomerDataAccess.LoadUserData()

		lueUSNr.Properties.DataSource = userData
		lueUSNr.Properties.ForceInitialize()

		Return Not userData Is Nothing
	End Function

	Private Sub OnbtnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub

	''' <summary>
	''' Loads Employee (MANr) drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData(ByVal employeeNumber As Integer?) As IEnumerable(Of LookupViewData)
		Dim result As New List(Of LookupViewData)

		If (employeeNumber Is Nothing) Then Return Nothing
		Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)

		If Not employeeData Is Nothing Then
			Dim data As LookupViewData = New LookupViewData
			data.ModulNumber = employeeData.EmployeeNumber
			data.ModulViewdata = employeeData.EmployeeFullname

			result.Add(data)
		End If

		Return result
	End Function

	''' <summary>
	''' Loads Employee (KDNr) drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData(ByVal customerNumber As Integer?) As IEnumerable(Of LookupViewData)
		Dim result As New List(Of LookupViewData)

		If (customerNumber Is Nothing) Then Return Nothing
		Dim customerData = m_CustomerDataAccess.LoadCustomerMasterData(customerNumber, String.Empty)
		If Not customerData Is Nothing Then
			Dim data As LookupViewData = New LookupViewData
			data.ModulNumber = customerData.CustomerNumber
			data.ModulViewdata = customerData.Company1

			result.Add(data)
		End If

		Return result
	End Function

	''' <summary>
	''' Loads Responsible Person (KDZHDNr) drop down data.
	''' </summary>
	Private Function LoadResponsiblePersonDropDownData(ByVal customerNumber As Integer?, ByVal zhdNumber As Integer?) As IEnumerable(Of LookupViewData)
		Dim result As New List(Of LookupViewData)

		If (customerNumber Is Nothing) Then Return Nothing
		Dim customerData = m_CustomerDataAccess.LoadResponsiblePersonMasterData(customerNumber, zhdNumber)
		If Not customerData Is Nothing Then
			Dim data As LookupViewData = New LookupViewData
			data.ModulNumber = customerData.RecordNumber
			data.ModulViewdata = customerData.ResponsiblePersonFullname

			result.Add(data)
		End If

		Return result
	End Function

	Private Function LoadVacancyDropDownData(ByVal vacancyNumber As Integer?) As IEnumerable(Of LookupViewData)
		Dim result As New List(Of LookupViewData)

		If (vacancyNumber Is Nothing) Then Return Nothing
		Dim vacancyData = m_VacancyDataAccess.LoadVacancyMasterData(m_InitializationData.MDData.MDNr, vacancyNumber)
		If Not vacancyData Is Nothing Then
			Dim data As LookupViewData = New LookupViewData
			data.ModulNumber = vacancyData.VakNr
			data.ModulViewdata = vacancyData.Bezeichnung

			result.Add(data)
		End If

		Return result
	End Function

	Private Function LoadProposeDropDownData(ByVal proposeNumber As Integer?) As IEnumerable(Of LookupViewData)
		Dim result As New List(Of LookupViewData)

		If (proposeNumber Is Nothing) Then Return Nothing
		Dim proposeData = m_ProposeDataAccess.LoadProposeMasterData(proposeNumber)
		If Not proposeData Is Nothing Then
			Dim data As LookupViewData = New LookupViewData
			data.ModulNumber = proposeData.ProposeNr
			data.ModulViewdata = proposeData.Bezeichnung

			result.Add(data)
		End If

		Return result
	End Function

	Private Sub LoadUserImage()
		Dim usNr = ParseToIntOrNothing(lueUSNr.EditValue)
		If (Not usNr Is Nothing) Then
			Dim userImage = m_EmployeeDatabaseAccess.LoadUserImageData(usNr)
			If ((Not userImage Is Nothing) AndAlso (Not userImage.UserImage Is Nothing) AndAlso (userImage.UserImage.Count > 0)) Then
				Dim memoryStream As New System.IO.MemoryStream(userImage.UserImage)
				advisorPicture.Image = Image.FromStream(memoryStream)
				Return
			End If
		End If

		advisorPicture.Image = Nothing
		advisorPicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")

	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnFrmTodo_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
			Dim setting_mainsplit_position = m_SettingsManager.ReadInteger(SettingKeys.SETTING_MAINSPLIT_POSITION)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			If setting_mainsplit_position > 0 Then sccMain.SplitterPosition = Math.Max(sccMain.SplitterPosition, setting_mainsplit_position)

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
	''' Handles the form disposed event.
	''' </summary>
	Private Sub OnFrmTodo_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
				m_SettingsManager.WriteBoolean(SettingKeys.SETTING_FILTER_ONLY_IMPORTANT, chkFilterOnlyImportant.Checked)
				m_SettingsManager.WriteBoolean(SettingKeys.SETTING_FILTER_EXCLUDE_FINISHED, chkFilterExcludeFinished.Checked)
				m_SettingsManager.WriteString(SettingKeys.SETTING_FILTER_CHECKED_USERS, cbbFilterUser.EditValue)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_MAINSPLIT_POSITION, sccMain.SplitterPosition)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try
	End Sub

	''' <summary>
	''' Keypreview for Modul-version
	''' </summary>
	Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 AndAlso m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	Private Sub OnlueDependentUsers_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueDependentUsers.ButtonClick

		If e.Button.Index = 1 Then

			Dim todoUserData = New TodoUserData With {.Customer_ID = m_InitializationData.MDData.MDGuid,
			.FK_ToDoID = m_todoData.ID,
			.UserNumber = lueDependentUsers.EditValue,
			.CreatedFrom = m_InitializationData.UserData.UserFullName}

			pccUsers.HidePopup()

			Dim success As Boolean = True
			success = success AndAlso m_EmployeeDatabaseAccess.InsertTodoUserData(m_InitializationData.MDData.MDGuid, todoUserData)
			m_ToDoUserData = LoadToDoUsersData(m_todoData.ID)
			FocusTodoUsersData(todoUserData.UserNumber)

		End If

	End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DateEdit Then
				Dim dateEdit As DateEdit = CType(sender, DateEdit)
				dateEdit.EditValue = Nothing
			End If

		ElseIf e.Button.Index = 2 Then
			OpenAssigendModul()

		End If

	End Sub

	Private Function OpenAssigendModul() As Boolean
		Dim result As Boolean = True

		Select Case m_ModulInput
			Case SourceInput.EMPLOYEE
				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, lueModul.EditValue)
				hub.Publish(openEmployeeMng)

			Case SourceInput.CUSTOMER
				Dim hub = MessageService.Instance.Hub
				Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, lueModul.EditValue)
				hub.Publish(openCustomerMng)

			Case SourceInput.RESPONSIBLE_PERSON
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenResponsiblePersonMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_todoData.CustomerNumber, lueModul.EditValue)
				hub.Publish(openMng)

			Case SourceInput.PROPOSE
				Dim hub = MessageService.Instance.Hub
				Dim openProposeMng As New OpenProposeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, lueModul.EditValue)
				hub.Publish(openProposeMng)

			Case SourceInput.VACANCY
				Dim hub = MessageService.Instance.Hub
				Dim openVacancyMng As New OpenVacancyMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, lueModul.EditValue)
				hub.Publish(openVacancyMng)

			Case Else
				Return False

		End Select

		Return result
	End Function

	''' <summary>
	''' Combines date and time.
	''' </summary>
	''' <param name="dateComponent">The date component.</param>
	''' <param name="timeComponent">The time component (date is ignored)</param>
	''' <returns>Combined date and time</returns>
	Private Function CombineDateAndTime(ByVal dateComponent As DateTime?, ByVal timeComponent As DateTime?) As DateTime?

		If Not dateComponent.HasValue Then
			Return Nothing
		End If

		If Not timeComponent.HasValue Then
			Return dateComponent.Value.Date
		End If

		Dim timeSpan As TimeSpan = timeComponent.Value - timeComponent.Value.Date
		Dim dateAndTime = dateComponent.Value.Date.Add(timeSpan)

		Return dateAndTime
	End Function

	Private Function ParseToIntOrNothing(ByVal number As String) As Integer?
		Dim result As Integer
		If (Not Integer.TryParse(number, result)) Then
			Return Nothing
		End If
		Return result
	End Function

	''' <summary>
	''' Focuses a todo.
	''' </summary>
	''' <param name="id">The id of the to do..</param>
	Private Sub FocusTodoData(ByVal id As Integer)

		If Not grdTODO.DataSource Is Nothing Then

			Dim todoData = CType(gvTODO.DataSource, BindingList(Of TodoListViewData))

			Dim index = todoData.ToList().FindIndex(Function(data) data.ID = id)

			Dim suppressState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			Dim rowHandle = gvTODO.GetRowHandle(index)
			gvTODO.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = suppressState
		End If

	End Sub

	Private Sub FocusTodoUsersData(ByVal userNr As Integer)

		If Not grdDependentUsers.DataSource Is Nothing Then

			Dim userData = CType(gvDependentUsers.DataSource, IEnumerable(Of TodoUserData))

			Dim index = userData.ToList().FindIndex(Function(data) data.UserNumber = userNr)

			Dim suppressState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim rowHandle = gvDependentUsers.GetRowHandle(index)
			gvDependentUsers.FocusedRowHandle = rowHandle

			m_SuppressUIEvents = suppressState
		End If

	End Sub

#End Region

#Region "View helper classes"

	Enum SourceInput
		EMPLOYEE
		CUSTOMER
		RESPONSIBLE_PERSON
		VACANCY
		PROPOSE
		APPLICATION
		EMPLOYMENT
		REPORT
		PAYROLL
		INVOICE
	End Enum

	Class LookupViewData
		Public Property ModulNumber As Integer?
		Public Property ModulViewdata As String

	End Class


	''' <summary>
	'''  Todo view data.
	''' </summary>
	Class TodoListViewData
		Public Property ID As Integer
		Public Property UserNumber As Integer?
		Public Property TU_UserNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property Subject As String
		Public Property IsImportant As Boolean?
		Public Property IsCompleted As Boolean?
		Public Property SourceInput As Integer?
		Public Property Schedulebegins As DateTime?
		Public Property Scheduleends As DateTime?

		Public ReadOnly Property IsDueNow As Boolean?
			Get
				If Not IsCompleted.GetValueOrDefault(False) AndAlso (Schedulebegins.HasValue AndAlso Schedulebegins <= Now.Date) Then
					Return True
				Else
					Return False
				End If
			End Get

		End Property

		Public ReadOnly Property IsSystemNotification As Boolean?
			Get
				If SourceInput.GetValueOrDefault(0) <> 0 Then
					Return True
				Else
					Return False
				End If
			End Get

		End Property

	End Class


#End Region

	Private Sub lueUSNr_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueUSNr.EditValueChanged
		LoadUserImage()
	End Sub

	Private Sub FilterChanged(sender As System.Object, e As System.EventArgs) ' Handles cbbFilterUser.EditValueChanged, chkFilterImportant.EditValueChanged, chkFilterFinished.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		LoadTodoListData()
	End Sub

	Private Sub gvTODO_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvTODO.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedTodo = GetSelectedTodo()

		If Not selectedTodo Is Nothing Then
			LoadTodoData(selectedTodo.ID)
		End If

	End Sub

	''' <summary>
	''' Handles double click on gvTODO.
	''' </summary>
	Private Sub OnGvExistingInvoiceAddressses_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvTODO.DoubleClick
		Dim selectedTodo = GetSelectedTodo()

		If Not selectedTodo Is Nothing Then
			LoadTodoData(selectedTodo.ID)
		End If
	End Sub

	Private Function GetSelectedTodo() As TodoListViewData
		Dim grdView = TryCast(grdTODO.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		If Not (grdView Is Nothing) Then

			Dim selectedRows = grdView.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim contact = CType(grdView.GetRow(selectedRows(0)), TodoListViewData)
				Return contact
			End If

		End If

		Return Nothing
	End Function



#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			If File.Exists(m_GVTODOSettingfilename) Then gvTODO.RestoreLayoutFromXml(m_GVTODOSettingfilename)

			If restoreLayout AndAlso Not keepFilter Then gvTODO.ActiveFilterCriteria = Nothing

		Catch ex As Exception

		End Try

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)
		gvTODO.SaveLayoutToXml(m_GVTODOSettingfilename)
	End Sub


	Private Sub OngvColumnFilterChanged(sender As Object, e As System.EventArgs)

		bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		bsiRecCount.Caption = String.Format("{0}", gvTODO.RowCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

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

	Private Sub dateEditScheduleBegins_EditValueChanged(sender As Object, e As EventArgs) Handles dateEditScheduleBegins.EditValueChanged

	End Sub



#End Region


End Class


''' <summary>
''' TODO filter settings.
''' </summary>
Public Class TODOFilterSettings

#Region "Private Fields"

	Private m_years As List(Of Integer)

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()
		m_years = New List(Of Integer)
	End Sub


#End Region

#Region "Public properties"

	Public Property Advisor As String
	Public Property Todoimportant As Boolean
	Public Property TodoDone As Boolean

	Public ReadOnly Property Years
		Get
			Return m_years.ToArray()
		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Clears the years.
	''' </summary>
	Public Sub ClearYears()
		m_years.Clear()
	End Sub

	''' <summary>
	''' Adds a year.
	''' </summary>
	''' <param name="year">The year to add.</param>
	Public Sub AddYear(ByVal year As Integer)
		m_years.Add(year)
	End Sub

#End Region

End Class
