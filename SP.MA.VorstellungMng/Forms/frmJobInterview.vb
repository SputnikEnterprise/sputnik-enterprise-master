
Imports System.Reflection.Assembly

Imports SP.MA.VorstellungMng.Settings
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings
Imports SP.DatabaseAccess.Employee

Imports SPProgUtility.Mandanten
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee.DataObjects.JobInterviewMng
Imports DevExpress.XtraEditors
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.TodoMng

''' <summary>
''' Job interview management.
''' </summary>
Public Class frmJobInterview

	Public Delegate Sub InterviewDataSavedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal RecordNumber As Integer)
	Public Delegate Sub InterviewDataDeletedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal RecordNumber As Integer)


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
  ''' The employee data access object.
  ''' </summary>
  Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

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
  ''' Contains the employee number.
  ''' </summary>
  Private m_EmployeeNumber As Integer

  ''' <summary>
  ''' The record number of selected interview.
  ''' </summary>
  Private m_CurrentJobInterviewRecordNumber As Integer?

  ''' <summary>
  ''' Boolean flag indicating if initial data has been loaded.
  ''' </summary>
  Private m_IsInitialDataLoaded As Boolean = False

  ''' <summary>
  ''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
  ''' </summary>
  Private m_SuppressUIEvents As Boolean = False

  ''' <summary>
  ''' Mandant data.
  ''' </summary>
  Private m_Mandant As Mandant

#End Region

#Region "Events"

	Public Event InterviewDataSaved As InterviewDataSavedHandler
	Public Event InterviewDataDeleted As InterviewDataDeletedHandler

#End Region


#Region "Constructor"

  Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Try
      ' Mandantendaten
      m_Mandant = New Mandant
      m_InitializationData = _setting
      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
    End Try

    gvInterview.OptionsView.ShowIndicator = False
    m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
    m_SettingsManager = New SettingsManager
    m_UtilityUI = New UtilityUI
    m_Utility = New Utility

    AddHandler dateEditFrom.ButtonClick, AddressOf OnDropDown_ButtonClick
    AddHandler lueCustomerName.ButtonClick, AddressOf OnDropDown_ButtonClick
    AddHandler lueZHDName.ButtonClick, AddressOf OnDropDown_ButtonClick
    AddHandler lueState1.ButtonClick, AddressOf OnDropDown_ButtonClick
    AddHandler lueVacancy.ButtonClick, AddressOf OnDropDown_ButtonClick
    AddHandler luePropose.ButtonClick, AddressOf OnDropDown_ButtonClick

    Reset()

  End Sub

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' Gets or sets initial data for new job interview.
  ''' </summary>
  Public Property InitDataForNewInteview As InitalDataForJobInterview

  ''' <summary>
  ''' Gets the selected interview view data.
  ''' </summary>
  ''' <returns>The selected interview or nothing if none is selected.</returns>
  Public ReadOnly Property SelectedInterviewViewData As JobInterviewViewData
    Get
      Dim grdView = TryCast(gridInterview.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      If Not (grdView Is Nothing) Then

        Dim selectedRows = grdView.GetSelectedRows()

        If (selectedRows.Count > 0) Then
          Dim interview = CType(grdView.GetRow(selectedRows(0)), JobInterviewViewData)
          Return interview
        End If

      End If

      Return Nothing
    End Get

  End Property

  ''' <summary>
  ''' Gets the selected responsible person view data.
  ''' </summary>
  ''' <returns>The responsible person view data or nothing.</returns>
  Public ReadOnly Property SelectedResponsiblePersonViewData As ResponsiblePersonViewData
    Get

      If lueZHDName.EditValue Is Nothing Then
        Return Nothing
      End If

      If lueZHDName.Properties.DataSource Is Nothing Then
        Return Nothing
      End If

      Dim responsiblePersonList = CType(lueZHDName.Properties.DataSource, List(Of ResponsiblePersonViewData))

      Dim viewData = responsiblePersonList.Where(Function(data) data.RecordNumber = lueZHDName.EditValue).FirstOrDefault()

      Return viewData

    End Get
  End Property

  ''' <summary>
  ''' Gets the first interview in the list of interviews.
  ''' </summary>
  ''' <returns>First interview in list or nothing.</returns>
  Public ReadOnly Property FirsJobInterviewInListOJobInterviews As JobInterviewViewData
    Get
      If gvInterview.RowCount > 0 Then

        Dim rowHandle = gvInterview.GetVisibleRowHandle(0)
        Return CType(gvInterview.GetRow(rowHandle), JobInterviewViewData)
      Else
        Return Nothing
      End If

    End Get
  End Property

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Loads the job interview data of an employee.
  ''' </summary>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <param name="jobInterviewRecordNumber">The job interview record number to load.</param>
  ''' <returns>Boolean truth value indicating success.</returns>
  Public Function LoadJobInterviewData(ByVal employeeNumber As Integer, Optional ByVal jobInterviewRecordNumber As Integer? = Nothing) As Boolean

    Dim success As Boolean = True

    m_EmployeeNumber = employeeNumber

    ' Reset the form
    Reset()

    success = success AndAlso LoadEmployeeMasterData(employeeNumber)
    success = success AndAlso LoadEmployeeInterviewData(employeeNumber)

    If Not m_IsInitialDataLoaded Then
      success = success AndAlso LoadAppointmentStateDropDownData()
      success = success AndAlso LoadCustomerDropDownData()
      m_IsInitialDataLoaded = True
    End If

    If jobInterviewRecordNumber.HasValue Then
      FocusJobInterview(employeeNumber, jobInterviewRecordNumber)
      Dim selectedJobInterview = SelectedInterviewViewData

      If Not selectedJobInterview Is Nothing Then
        PresentInterviewDetailData(selectedJobInterview)
      Else
        ' If record could not be found -> load first job interview in the list.
        LoadFirstJobInterview()
      End If
    ElseIf Not InitDataForNewInteview Is Nothing Then
      PrepareForNew()
    Else
      LoadFirstJobInterview()
    End If

    Return success

  End Function
#End Region

#Region "Private Methods"

  ''' <summary>
  '''  Translate controls.
  ''' </summary>
  Private Sub TranslateControls()

    Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

    btnSave.Text = m_Translate.GetSafeTranslationValue(btnSave.Text)
    btnNewInterview.Text = m_Translate.GetSafeTranslationValue(btnNewInterview.Text)
    btnDeleteInterview.Text = m_Translate.GetSafeTranslationValue(btnDeleteInterview.Text)
    Me.btnCreateTODO.Text = m_Translate.GetSafeTranslationValue(Me.btnCreateTODO.Text)

    lblVorstellungals.Text = m_Translate.GetSafeTranslationValue(lblVorstellungals.Text)
    lbldatum.Text = m_Translate.GetSafeTranslationValue(lbldatum.Text)
    lblKunde.Text = m_Translate.GetSafeTranslationValue(lblKunde.Text)
    lblZHD.Text = m_Translate.GetSafeTranslationValue(lblZHD.Text)
    lblAdresse.Text = m_Translate.GetSafeTranslationValue(lblAdresse.Text)
    lblTelefon.Text = m_Translate.GetSafeTranslationValue(lblTelefon.Text)
    lblTelefax.Text = m_Translate.GetSafeTranslationValue(lblTelefax.Text)
    lblHomepage.Text = m_Translate.GetSafeTranslationValue(lblHomepage.Text)
    lblEMail.Text = m_Translate.GetSafeTranslationValue(lblEMail.Text)
    lblstatus.Text = m_Translate.GetSafeTranslationValue(lblstatus.Text)
    lblErgebnis.Text = m_Translate.GetSafeTranslationValue(lblErgebnis.Text)
    lblvakanz.Text = m_Translate.GetSafeTranslationValue(lblvakanz.Text)

    lblerstellt.Text = m_Translate.GetSafeTranslationValue(lblerstellt.Text)
    lblgaendert.Text = m_Translate.GetSafeTranslationValue(lblgaendert.Text)

  End Sub


  ''' <summary>
  ''' Resets the form.
  ''' </summary>
  Private Sub Reset()

    m_CurrentJobInterviewRecordNumber = Nothing

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True

    If (Not employeePicture.Image Is Nothing) Then
      employeePicture.Image.Dispose()
    End If

    employeePicture.Image = Nothing
    employeePicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")
    employeePicture.Properties.ShowMenu = False
    employeePicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

    txtInterviewAs.Text = String.Empty
    txtInterviewAs.Properties.MaxLength = 100

    dateEditFrom.EditValue = Nothing
    timeStart.EditValue = Nothing

    lueCustomerName.EditValue = Nothing
    lueZHDName.EditValue = Nothing

    txtAddress.Text = String.Empty
    txtAddress.Properties.MaxLength = 100

    txtTelefon.Text = String.Empty
    txtTelefax.Properties.MaxLength = 50

    txtTelefax.Text = String.Empty
    txtTelefax.Properties.MaxLength = 50

    txtHompage.Text = String.Empty
    txtHompage.Properties.MaxLength = 100

    txteMail.Text = String.Empty
    txteMail.Properties.MaxLength = 100

    lueState1.EditValue = Nothing

    txtResult.Text = String.Empty
    txtResult.Properties.MaxLength = 500

    lueVacancy.EditValue = Nothing
    luePropose.EditValue = Nothing

    lblInterviewCreated.Text = String.Empty
    lblInterviewChanged.Text = String.Empty

    ' ---Reset drop downs, grids and lists---
    ResetCustomerDropDown()
    ResetResponsiblePersonDropDown()
    ResetAppointmentStateDropDown()

    ResetVacancyDropDown()
    ResetProposeDropDown()

    ResetInterviewGrid()

    m_SuppressUIEvents = suppressUIEventsState

    TranslateControls()

    btnDeleteInterview.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 137)

  End Sub

  ''' <summary>
  ''' Resets the customer person drop down.
  ''' </summary>
  Private Sub ResetCustomerDropDown()

    lueCustomerName.Properties.DisplayMember = "Company"
		lueCustomerName.Properties.ValueMember = "CustomerNumber"

		gvCustomer.OptionsView.ShowIndicator = False
		gvCustomer.OptionsView.ShowColumnHeaders = True
		gvCustomer.OptionsView.ShowFooter = False

		gvCustomer.OptionsView.ShowAutoFilterRow = True
		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCustomer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerNumber.BestFit()
		gvCustomer.Columns.Add(columnCustomerNumber)

		Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany.Name = "Company"
		columnCompany.FieldName = "Company"
		columnCompany.Visible = True
		columnCompany.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCompany.MaxWidth = 200
		gvCustomer.Columns.Add(columnCompany)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnPostcodeAndLocation.BestFit()
		gvCustomer.Columns.Add(columnPostcodeAndLocation)

		lueCustomerName.Properties.BestFitMode = BestFitMode.BestFitResizePopup
    lueCustomerName.Properties.NullText = String.Empty
    lueCustomerName.EditValue = Nothing

  End Sub

  ''' <summary>
  ''' Resets the responsible person drop down.
  ''' </summary>
  Private Sub ResetResponsiblePersonDropDown()

    lueZHDName.Properties.DisplayMember = "SalutationLastnameFirstname"
    lueZHDName.Properties.ValueMember = "RecordNumber"

		gvZHDName.OptionsView.ShowIndicator = False
		gvZHDName.OptionsView.ShowColumnHeaders = True
		gvZHDName.OptionsView.ShowFooter = False
		gvZHDName.OptionsView.ShowAutoFilterRow = True
		gvZHDName.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvZHDName.Columns.Clear()

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "RecordNumber"
		columnRecordNumber.FieldName = "RecordNumber"
		columnRecordNumber.Visible = False
		gvZHDName.Columns.Add(columnRecordNumber)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnName.Name = "SalutationLastnameFirstname"
		columnName.FieldName = "SalutationLastnameFirstname"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnName)


		lueZHDName.Properties.NullText = String.Empty

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    lueZHDName.EditValue = Nothing
    m_SuppressUIEvents = suppressUIEventsState

  End Sub

  ''' <summary>
  ''' Resets the appointment state drop down.
  ''' </summary>
  Private Sub ResetAppointmentStateDropDown()

    lueState1.Properties.DisplayMember = "Description"
    lueState1.Properties.ValueMember = "Description"

    Dim columns = lueState1.Properties.Columns
    columns.Clear()
    columns.Add(New LookUpColumnInfo("Description", 0))

    lueState1.Properties.ShowHeader = False
    lueState1.Properties.ShowFooter = False
    lueState1.Properties.DropDownRows = 10
    lueState1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
    lueState1.Properties.SearchMode = SearchMode.AutoComplete
    lueState1.Properties.AutoSearchColumnIndex = 0

    lueState1.Properties.NullText = String.Empty
    lueState1.EditValue = Nothing

  End Sub

  ''' <summary>
  ''' Resets the vacancy drop down.
  ''' </summary>
  Private Sub ResetVacancyDropDown()
    lueVacancy.Properties.DisplayMember = "Description"
    lueVacancy.Properties.ValueMember = "VacancyNumber"

		gvVacancy.OptionsView.ShowIndicator = False
		gvVacancy.OptionsView.ShowColumnHeaders = True
		gvVacancy.OptionsView.ShowFooter = False

		gvVacancy.OptionsView.ShowAutoFilterRow = True
		gvVacancy.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvVacancy.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "VacancyNumber"
		columnCustomerNumber.FieldName = "VacancyNumber"
		columnCustomerNumber.Visible = True
		gvVacancy.Columns.Add(columnCustomerNumber)

		Dim columnVBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnVBezeichnung.Name = "Description"
		columnVBezeichnung.FieldName = "Description"
		columnVBezeichnung.Visible = True
		columnVBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvVacancy.Columns.Add(columnVBezeichnung)

		Dim columnVState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVState.Caption = m_Translate.GetSafeTranslationValue("Status")
		columnVState.Name = "VakState"
		columnVState.FieldName = "VakState"
		columnVState.Visible = True
		columnVState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvVacancy.Columns.Add(columnVState)

		Dim columnCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCreatedon.Name = "CreatedOn"
		columnCreatedon.FieldName = "CreatedOn"
		columnCreatedon.Visible = True
		gvVacancy.Columns.Add(columnCreatedon)

		lueVacancy.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueVacancy.Properties.NullText = String.Empty
    lueVacancy.EditValue = Nothing
  End Sub

  ''' <summary>
  ''' Resets the propose drop down.
  ''' </summary>
  Private Sub ResetProposeDropDown()
    luePropose.Properties.DisplayMember = "Description"
    luePropose.Properties.ValueMember = "ProposeNumber"

		gvPropose.OptionsView.ShowIndicator = False
		gvPropose.OptionsView.ShowColumnHeaders = True
		gvPropose.OptionsView.ShowFooter = False
		gvPropose.OptionsView.ShowAutoFilterRow = True
		gvPropose.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPropose.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "ProposeNumber"
		columnCustomerNumber.FieldName = "ProposeNumber"
		columnCustomerNumber.Visible = True
		gvPropose.Columns.Add(columnCustomerNumber)

		Dim columnVBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnVBezeichnung.Name = "Description"
		columnVBezeichnung.FieldName = "Description"
		columnVBezeichnung.Visible = True
		columnVBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnVBezeichnung.MaxWidth = 200
		gvPropose.Columns.Add(columnVBezeichnung)

		Dim columnVState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVState.Caption = m_Translate.GetSafeTranslationValue("Status")
		columnVState.Name = "P_State"
		columnVState.FieldName = "P_State"
		columnVState.Visible = True
		columnVState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvPropose.Columns.Add(columnVState)

		Dim columnCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCreatedon.Name = "CreatedOn"
		columnCreatedon.FieldName = "CreatedOn"
		columnCreatedon.Visible = True
		gvPropose.Columns.Add(columnCreatedon)

		luePropose.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePropose.Properties.NullText = String.Empty
    luePropose.EditValue = Nothing

	End Sub

  ''' <summary>
  ''' Resets the interview grid.
  ''' </summary>
  Private Sub ResetInterviewGrid()

    ' Reset the grid
    gvInterview.OptionsView.ShowIndicator = False

    gvInterview.Columns.Clear()

    Dim columnAppointmentDate As New DevExpress.XtraGrid.Columns.GridColumn()
    columnAppointmentDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
    columnAppointmentDate.Name = "AppointmentDate"
    columnAppointmentDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
    columnAppointmentDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm"
    columnAppointmentDate.FieldName = "AppointmentDate"
    columnAppointmentDate.Visible = True
    gvInterview.Columns.Add(columnAppointmentDate)

    Dim columnJobTitle As New DevExpress.XtraGrid.Columns.GridColumn()
    columnJobTitle.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
    columnJobTitle.Name = "JobTitle"
    columnJobTitle.FieldName = "JobTitle"
    columnJobTitle.Visible = True
    gvInterview.Columns.Add(columnJobTitle)

    Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
    columnCompany.Caption = m_Translate.GetSafeTranslationValue("Kunde")
    columnCompany.Name = "Company"
    columnCompany.FieldName = "Company"
    columnCompany.Visible = True
    gvInterview.Columns.Add(columnCompany)

    Dim columnJobAppointmentState As New DevExpress.XtraGrid.Columns.GridColumn()
    columnJobAppointmentState.Caption = m_Translate.GetSafeTranslationValue("Status")
    columnJobAppointmentState.Name = "JobAppointmentState"
    columnJobAppointmentState.FieldName = "JobAppointmentState"
    columnJobAppointmentState.Visible = True
    gvInterview.Columns.Add(columnJobAppointmentState)

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    gridInterview.DataSource = Nothing
    m_SuppressUIEvents = suppressUIEventsState

  End Sub

  ''' <summary>
  ''' Loads the first job interview.
  ''' </summary>
  Private Sub LoadFirstJobInterview()

    Dim firstJobInterview = FirsJobInterviewInListOJobInterviews

    If Not firstJobInterview Is Nothing Then
      PresentInterviewDetailData(firstJobInterview)
    Else
      PrepareForNew()
    End If
  End Sub

  ''' <summary>
  ''' Loads the employee master data.
  ''' </summary>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <returns>Boolean flag indicating success.</returns>
  Private Function LoadEmployeeMasterData(ByVal employeeNumber As Integer) As Boolean

    Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, True)

    If (employeeMasterData Is Nothing) Then
      Return False
    End If

    ' Set the title with lastname and firstname of employee.
    Text = String.Format(m_Translate.GetSafeTranslationValue("Vorstellungsgespräche von {0} {1}"), employeeMasterData.Lastname, employeeMasterData.Firstname)

    ' Load the picture
    Try
      If Not employeeMasterData.MABild Is Nothing AndAlso employeeMasterData.MABild.Count > 0 Then
        Dim memoryStream As New System.IO.MemoryStream(employeeMasterData.MABild)
        employeePicture.Image = Image.FromStream(memoryStream)
      End If
    Catch ex As Exception
      m_Logger.LogError(ex.ToString())
    End Try

    Return True
  End Function

  ''' <summary>
  ''' Loads employee interview data.
  ''' </summary>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <returns>Boolean value indicating success.</returns>
  Private Function LoadEmployeeInterviewData(ByVal employeeNumber As Integer)

    Dim employeeJobInterviews = m_EmployeeDatabaseAccess.LoadEmployeeJobInterviews(employeeNumber)

    If (employeeJobInterviews Is Nothing) Then
      Return False
    End If

    Dim listDataSource As BindingList(Of JobInterviewViewData) = New BindingList(Of JobInterviewViewData)

    ' Convert the data to view data.
    For Each interview In employeeJobInterviews

      Dim viewData = New JobInterviewViewData() With {
        .ID = interview.ID,
        .RecordNumber = interview.RecordNumber,
        .AppointmentDate = interview.AppointmentDate,
        .JobTitle = interview.JobTitle,
        .Company = interview.Company,
        .JobAppointmentState = interview.JobAppointmentState,
        .Location = interview.Location,
        .Telephone = interview.Telephone,
        .Telefax = interview.Telefax,
        .Homepage = interview.Homepage,
        .Email = interview.eMail,
        .Outcome = interview.Outcome,
        .VakNr = interview.VakNr,
        .ProposeNr = interview.ProposeNr,
        .CreatedOn = interview.CreatedOn,
        .CreatedFrom = interview.CreatedFrom,
        .ChangedOn = interview.ChangedOn,
        .ChangedFrom = interview.ChangedFrom,
        .CustomerNumber = interview.CustomerNumber,
        .ResponsiblePersonRecordNumber = interview.ResponsiblePersonNumber
      }

      listDataSource.Add(viewData)
    Next

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    gridInterview.DataSource = listDataSource
    m_SuppressUIEvents = suppressUIEventsState

    Return True

  End Function

  ''' <summary>
  ''' Loads customer drop down data.
  ''' </summary>
  Private Function LoadCustomerDropDownData() As Boolean

    Dim customerData = m_EmployeeDatabaseAccess.LoadCustomerDataForJobInterviewMng()

    If (customerData Is Nothing) Then
      Return False
    End If

    Dim customerViewData = New List(Of CustomerViewData)

    For Each customer In customerData
      customerViewData.Add(New CustomerViewData With {
                           .CustomerNumber = customer.CustomerNumber,
                           .Company = customer.Company,
                           .Postcode = customer.Postcode,
                           .Location = customer.Location
                           })
    Next

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    lueCustomerName.Properties.DataSource = customerViewData

		m_SuppressUIEvents = suppressUIEventsState

    Return True

  End Function

  ''' <summary>
  ''' Loads responsible person drop down data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <returns>Boolean flag indicating success.</returns>
  Private Function LoadResponsiblePersonDropDownData(ByVal customerNumber As Integer) As Boolean

    Dim responsiblePersonData = m_EmployeeDatabaseAccess.LoadResponsiblePersonDataForJobInterviewMng(customerNumber)

    If (responsiblePersonData Is Nothing) Then
      Return False
    End If

    Dim responsiblePersonViewData = New List(Of ResponsiblePersonViewData)

    For Each responsiblePerson In responsiblePersonData
			responsiblePersonViewData.Add(New ResponsiblePersonViewData With {
																		.ID = responsiblePerson.ID,
																		.RecordNumber = responsiblePerson.RecordNumber,
																		.TranslatedSalutation = responsiblePerson.TranslatedAnrede,
																		.Lastname = responsiblePerson.Lastname,
																		.Firstname = responsiblePerson.Firstname,
																		.Postcode = responsiblePerson.Postcode,
																		.Location = responsiblePerson.Location,
																		.Telephone = responsiblePerson.Telephone,
																		.Telefax = responsiblePerson.Telefax,
																		.Email = responsiblePerson.EMail,
																		.Homepage = responsiblePerson.Homepage,
																		.ZState1 = responsiblePerson.ZState1,
																		.ZState2 = responsiblePerson.ZState2
																	})
    Next

		Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    lueZHDName.EditValue = Nothing
    lueZHDName.Properties.DataSource = responsiblePersonViewData

		m_SuppressUIEvents = suppressUIEventsState

    Return True

  End Function

  ''' <summary>
  ''' Loads appointment state drop down data.
  ''' </summary>
  ''' <returns>Boolean value indicating success.</returns>
  Private Function LoadAppointmentStateDropDownData()

    Dim jobstateData = m_EmployeeDatabaseAccess.LoadJobAppointmentStateDataeForJobInterviewMng()

    If (jobstateData Is Nothing) Then
      Return False
    End If

    Dim responsiblePersonViewData = New List(Of AppointmentStateViewData)

    For Each state In jobstateData
      responsiblePersonViewData.Add(New AppointmentStateViewData With {
                                    .ID = state.ID,
                                    .Description = state.Description
                           })
    Next


    lueState1.Properties.DataSource = responsiblePersonViewData
    lueState1.Properties.ForceInitialize()

    Return True

  End Function

  ''' <summary>
  ''' Loads vacancy view data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <returns>Boolan value indicating success.</returns>
  Private Function LoadVacancyDropDownData(ByVal customerNumber As Integer) As Boolean

    Dim vacancyData = m_EmployeeDatabaseAccess.LoadVacancyDataForJobInterviewMng(customerNumber)

    If (vacancyData Is Nothing) Then
      Return False
    End If

    Dim vacancyViewData = New List(Of VacancyViewData)

		For Each vacancy In vacancyData
			vacancyViewData.Add(New VacancyViewData With {
																		.VacancyNumber = vacancy.VacancyNumber,
																		.Description = vacancy.Description,
																		.CreatedFrom = vacancy.CreatedFrom,
																		.CreatedOn = vacancy.CreatedOn,
																		.VakState = vacancy.VakState
																	})
		Next

    lueVacancy.Properties.DataSource = vacancyViewData

    Return True

  End Function

  ''' <summary>
  ''' Loads vacancy view data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <returns>Boolan value indicating success.</returns>
  Private Function LoadProposeDropDownData(ByVal customerNumber As Integer, ByVal employeeNumber As Integer) As Boolean

    Dim proposeData = m_EmployeeDatabaseAccess.LoadProposeDataForJobInterviewMng(customerNumber, employeeNumber)

    If (proposeData Is Nothing) Then
      Return False
    End If

    Dim proposeViewData = New List(Of ProposeViewData)

    For Each propose In proposeData
			proposeViewData.Add(New ProposeViewData With {
													.ProposeNumber = propose.ProposeNumber,
													.Description = propose.Description,
													.P_State = propose.P_State,
													.CreatedFrom = propose.CreatedFrom,
													.CreatedOn = propose.CreatedOn
												})
    Next

    luePropose.Properties.DataSource = proposeViewData

    Return True

  End Function

  ''' <summary>
  ''' Handles form load event.
  ''' </summary>
  Private Sub OnFrmJobInterview_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    Me.KeyPreview = True
    Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_Mandant.GetDefaultUSNr, String.Empty)
    If strStyleName <> String.Empty Then
      UserLookAndFeel.Default.SetSkinStyle(strStyleName)
    End If

    Try
      Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
      Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
      Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
      Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)

			If setting_form_height >= 100 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width >= 100 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			If setting_form_mainsplitter > 0 Then Me.sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_form_mainsplitter)

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
  Private Sub OnFrmJobInterview_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

    ' Save form location, width and height in setttings
    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccMain.SplitterPosition)
        m_SettingsManager.SaveSettings()
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())

    End Try

    ' Dispose employee picture
    If (Not employeePicture.Image Is Nothing) Then
      employeePicture.Image.Dispose()
    End If

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
      strMsg = String.Format(strMsg, vbNewLine, _
                             GetExecutingAssembly().FullName, _
                             GetExecutingAssembly().Location, _
                             strRAssembly)
      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End If
  End Sub

  ''' <summary>
  ''' Handles focus change of interview row.
  ''' </summary>
  Private Sub OnInterview_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvInterview.FocusedRowChanged

    If m_SuppressUIEvents Then
      Return
    End If

    Dim selectedInterview = SelectedInterviewViewData

    If Not selectedInterview Is Nothing Then
      PresentInterviewDetailData(selectedInterview)
    End If

  End Sub

	''' <summary>
	'''  Handles RowStyle event of gvZHDName grid view.
	''' </summary>
	Private Sub OngvZHDName_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvZHDName.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvZHDName.GetRow(e.RowHandle), ResponsiblePersonViewData)

			If Not rowData.IsZHDActiv.GetValueOrDefault(True) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

	''' <summary>
	'''  Handles RowStyle event of gvVacancy grid view.
	''' </summary>
	Private Sub OnGvVacancy_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvVacancy.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvVacancy.GetRow(e.RowHandle), VacancyViewData)

			If Not rowData.VakState Is Nothing AndAlso (rowData.VakState.ToLower.Contains("inaktiv") Or rowData.VakState.ToLower.Contains("nicht aktiv")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

	''' <summary>
	'''  Handles RowStyle event of gvPropose grid view.
	''' </summary>
	Private Sub OngvPropose_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvPropose.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvPropose.GetRow(e.RowHandle), ProposeViewData)

			If Not rowData.P_State Is Nothing AndAlso (rowData.P_State.ToLower.Contains("absage") Or rowData.P_State.ToLower.Contains("abgeschlossen")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

  ''' <summary>
  ''' Handles double click on grid.
  ''' </summary>
  Private Sub OnGridInterview_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridInterview.DoubleClick

    Dim selectedInterview = SelectedInterviewViewData

    If Not selectedInterview Is Nothing Then
      PresentInterviewDetailData(selectedInterview)
    End If

  End Sub

  ''' <summary>
  ''' Handles change of customer name lookup edit.
  ''' </summary>
  Private Sub OnLueCustomerName_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomerName.EditValueChanged

    If (m_SuppressUIEvents) Then
      Return
    End If

    If Not lueCustomerName.EditValue Is Nothing Then

      Dim customerNumber As Integer? = lueCustomerName.EditValue

      ' Load responsible persons, vancancy and propose data
      Dim success = LoadResponsiblePersonDropDownData(customerNumber)
      success = success AndAlso LoadVacancyDropDownData(customerNumber)
      success = success AndAlso LoadProposeDropDownData(customerNumber, m_EmployeeNumber)

      If Not success Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler beim Laden vom Daten (ZHD, Vananz, Vorschlag)."))
      End If
    Else
      m_SuppressUIEvents = True
      lueZHDName.EditValue = Nothing
      lueZHDName.Properties.DataSource = Nothing
      m_SuppressUIEvents = False

      lueVacancy.EditValue = Nothing
      lueVacancy.Properties.DataSource = Nothing
      luePropose.EditValue = Nothing
      luePropose.Properties.DataSource = Nothing
    End If

  End Sub

  ''' <summary>
  ''' Handles change of responsible peron lookup edit.
  ''' </summary>
  Private Sub OnLueZHDName_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueZHDName.EditValueChanged

    If (m_SuppressUIEvents) Then
      Return
    End If
		txtAddress.Text = Nothing
		txtTelefon.Text = Nothing
		txtTelefax.Text = Nothing
		txtHompage.Text = Nothing
		txteMail.Text = Nothing

    If Not lueZHDName.EditValue Is Nothing Then

      Dim responsiblePersonViewData = SelectedResponsiblePersonViewData

      If Not responsiblePersonViewData Is Nothing Then
        txtAddress.Text = responsiblePersonViewData.PostcodeLocation
        txtTelefon.Text = responsiblePersonViewData.Telephone
        txtTelefax.Text = responsiblePersonViewData.Telefax
        txtHompage.Text = responsiblePersonViewData.Homepage
        txteMail.Text = responsiblePersonViewData.Email
      End If

    End If

  End Sub

  ''' <summary>
  ''' Handles click on save button.
  ''' </summary>
  Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
		SaveData()
	End Sub

	''' <summary>
	''' Saves the data.
	''' </summary>
	Private Sub SaveData()

		If ValidateInterviewInputData() Then

			Dim interviewData As EmployeeJobAppointmentData = Nothing

			Dim dt = DateTime.Now
			If Not m_CurrentJobInterviewRecordNumber.HasValue Then
				interviewData = New EmployeeJobAppointmentData With {.EmployeeNumber = m_EmployeeNumber,
																							.CreatedOn = dt,
																							.CreatedFrom = m_ClsProgSetting.GetUserName()}
			Else

				Dim interviewList = m_EmployeeDatabaseAccess.LoadEmployeeJobInterviews(m_EmployeeNumber, m_CurrentJobInterviewRecordNumber)

				If interviewList Is Nothing OrElse Not interviewList.Count = 1 Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

				interviewData = interviewList(0)
			End If

			interviewData.JobTitle = txtInterviewAs.Text
			interviewData.AppointmentDate = CombineDateAndTime(dateEditFrom.EditValue, timeStart.EditValue)
			interviewData.CustomerNumber = lueCustomerName.EditValue
			interviewData.Company = lueCustomerName.Text
			interviewData.ResponsiblePersonNumber = lueZHDName.EditValue
			interviewData.Location = txtAddress.Text
			interviewData.Telephone = txtTelefon.Text
			interviewData.Telefax = txtTelefax.Text
			interviewData.Homepage = txtHompage.Text
			interviewData.eMail = txteMail.Text
			interviewData.JobAppointmentState = lueState1.EditValue
			interviewData.Outcome = txtResult.Text
			interviewData.VakNr = lueVacancy.EditValue
			interviewData.ProposeNr = luePropose.EditValue

			interviewData.ChangedFrom = m_ClsProgSetting.GetUserName()
			interviewData.ChangedOn = dt

			Dim success As Boolean = True

			' Insert or update interview
			If interviewData.ID = 0 Then
				success = m_EmployeeDatabaseAccess.AddEmployeeJobInterview(interviewData)
				m_CurrentJobInterviewRecordNumber = interviewData.RecordNumber
			Else
				success = m_EmployeeDatabaseAccess.UpdateEmployeeJobInterview(interviewData)
			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else

				lblInterviewCreated.Text = String.Format("{0:f}, {1}", interviewData.CreatedOn, interviewData.CreatedFrom)
				lblInterviewChanged.Text = String.Format("{0:f}, {1}", interviewData.ChangedOn, interviewData.ChangedFrom)

				LoadEmployeeInterviewData(m_EmployeeNumber)
				FocusJobInterview(m_EmployeeNumber, m_CurrentJobInterviewRecordNumber)

				RaiseEvent InterviewDataSaved(Me, m_EmployeeNumber, interviewData.RecordNumber)

			End If

		End If

	End Sub


  ''' <summary>
  ''' Handles click on new button.
  ''' </summary>
  Private Sub OnBtnNewInterview_Click(sender As System.Object, e As System.EventArgs) Handles btnNewInterview.Click
    PrepareForNew()
  End Sub

  ''' <summary>
  ''' Handles click on delete button.
  ''' </summary>
  Private Sub OnDeleteInterview_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteInterview.Click

    Dim interviewViewData = SelectedInterviewViewData

    If Not interviewViewData Is Nothing AndAlso m_CurrentJobInterviewRecordNumber.HasValue Then

      If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
                                      m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
        Return
      End If

      Dim result = m_EmployeeDatabaseAccess.DeleteEmployeeJobInterview(interviewViewData.ID)

      Select Case result
        Case DeleteEmployeeJobAppointmentResult.ErrorWhileDelete
          m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gelöscht werden."))
        Case DeleteEmployeeJobAppointmentResult.Deleted
          ' Reload interview data and select first entry.
          LoadJobInterviewData(m_EmployeeNumber)
          Dim firstinterview = FirsJobInterviewInListOJobInterviews

          If (firstinterview Is Nothing) Then
            PrepareForNew()
          Else
            PresentInterviewDetailData(firstinterview)
          End If

      End Select

			RaiseEvent InterviewDataDeleted(Me, m_EmployeeNumber, interviewViewData.RecordNumber)

    End If

  End Sub

  ''' <summary>
  ''' Presents job interview detail data.
  ''' </summary>
  ''' <param name="jobInterviewViewData">The job interview view data.</param>
  Private Sub PresentInterviewDetailData(ByVal jobInterviewViewData As JobInterviewViewData)

    If (jobInterviewViewData Is Nothing) Then
      Return
    End If

    m_CurrentJobInterviewRecordNumber = jobInterviewViewData.RecordNumber

    txtInterviewAs.Text = jobInterviewViewData.JobTitle
    dateEditFrom.EditValue = jobInterviewViewData.AppointmentDate
    timeStart.EditValue = jobInterviewViewData.AppointmentDate

    lueCustomerName.EditValue = jobInterviewViewData.CustomerNumber

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    lueZHDName.EditValue = jobInterviewViewData.ResponsiblePersonRecordNumber
    m_SuppressUIEvents = suppressUIEventsState

    txtAddress.Text = jobInterviewViewData.Location

    txtTelefon.Text = jobInterviewViewData.Telephone
    txtTelefax.Text = jobInterviewViewData.Telefax
    txtHompage.Text = jobInterviewViewData.Homepage
    txteMail.Text = jobInterviewViewData.Email

    lueState1.EditValue = jobInterviewViewData.JobAppointmentState

    txtResult.Text = jobInterviewViewData.Outcome

    lueVacancy.EditValue = jobInterviewViewData.VakNr
    luePropose.EditValue = jobInterviewViewData.ProposeNr

    lblInterviewCreated.Text = String.Format("{0:f}, {1}", jobInterviewViewData.CreatedOn, jobInterviewViewData.CreatedFrom)
    lblInterviewChanged.Text = String.Format("{0:f}, {1}", jobInterviewViewData.ChangedOn, jobInterviewViewData.ChangedFrom)

    ' Clear errors
    ClearErrors()

  End Sub


  ''' <summary>
  ''' Prepares for a new job interview.
  ''' </summary>
  Private Sub PrepareForNew()

    m_CurrentJobInterviewRecordNumber = Nothing

    txtInterviewAs.Text = String.Empty
    dateEditFrom.EditValue = Nothing
    timeStart.EditValue = Nothing

    lueCustomerName.EditValue = Nothing

    Dim suppressUIEventsState = m_SuppressUIEvents
    m_SuppressUIEvents = True
    lueZHDName.EditValue = Nothing
    m_SuppressUIEvents = suppressUIEventsState

    txtAddress.Text = String.Empty
    txtTelefon.Text = String.Empty
    txtTelefax.Text = String.Empty
    txtHompage.Text = String.Empty
    txteMail.Text = String.Empty
    lueState1.EditValue = Nothing
    txtResult.Text = String.Empty
    lueVacancy.EditValue = Nothing
    luePropose.EditValue = Nothing

    lblInterviewCreated.Text = String.Empty
    lblInterviewChanged.Text = String.Empty

    ApplyInitialDataForNewJobInteview()

    ' Clear errors
    ClearErrors()

  End Sub

  ''' <summary>
  ''' Applies initial data for new job interview.
  ''' </summary>
  Private Sub ApplyInitialDataForNewJobInteview()

    If Not InitDataForNewInteview Is Nothing Then

      ' Interview as
      If Not String.IsNullOrWhiteSpace(InitDataForNewInteview.InterviewAs) Then
        txtInterviewAs.Text = InitDataForNewInteview.InterviewAs
      End If

      ' Date and time
      If InitDataForNewInteview.InteviewDate.HasValue Then
        dateEditFrom.EditValue = InitDataForNewInteview.InteviewDate
        timeStart.EditValue = InitDataForNewInteview.InteviewDate
      End If

      If InitDataForNewInteview.CustomerNumber.HasValue Then

        lueCustomerName.EditValue = InitDataForNewInteview.CustomerNumber

        ' Responsible Person number
        If InitDataForNewInteview.ResponsiblePersonNumber.HasValue Then
          lueZHDName.EditValue = InitDataForNewInteview.ResponsiblePersonNumber.Value
        End If

        ' Vacancy Number
        If InitDataForNewInteview.VakNr.HasValue Then
          lueVacancy.EditValue = InitDataForNewInteview.VakNr
        End If

        ' Propose Number
        If InitDataForNewInteview.ProposeNr.HasValue Then
          luePropose.EditValue = InitDataForNewInteview.ProposeNr
        End If

      End If

      ' State
      If InitDataForNewInteview.IDState.HasValue Then

        If Not lueState1.Properties.DataSource Is Nothing Then
          Dim list = CType(lueState1.Properties.DataSource, List(Of AppointmentStateViewData))
          Dim stateEntry = list.Where(Function(data) data.ID = InitDataForNewInteview.IDState).FirstOrDefault()

          If Not stateEntry Is Nothing Then
            lueState1.EditValue = stateEntry.Description
          End If

        End If
      End If

      '  Result
      If Not String.IsNullOrWhiteSpace(InitDataForNewInteview.Result) Then
        txtResult.Text = InitDataForNewInteview.Result
      End If

      InitDataForNewInteview = Nothing
    End If

  End Sub

  ''' <summary>
  ''' Focuses a job interview.
  ''' </summary>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <param name="interviewRecordNumber">The job interview record number</param>
  Private Sub FocusJobInterview(ByVal employeeNumber As Integer, ByVal interviewRecordNumber As Integer)

    If Not gridInterview.DataSource Is Nothing Then

      Dim interViewData = CType(gvInterview.DataSource, BindingList(Of JobInterviewViewData))

      Dim index = interViewData.ToList().FindIndex(Function(data) data.RecordNumber = interviewRecordNumber)

      Dim suppressState = m_SuppressUIEvents
      m_SuppressUIEvents = True
      Dim rowHandle = gvInterview.GetRowHandle(index)
      gvInterview.FocusedRowHandle = rowHandle
      m_SuppressUIEvents = suppressState
    End If

  End Sub

  ''' <summary>
  ''' Validates interview data input data.
  ''' </summary>
  Private Function ValidateInterviewInputData() As Boolean

    Dim missingFieldText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

    Dim isValid As Boolean = True

    isValid = isValid And SetErrorIfInvalid(txtInterviewAs, errorProviderJobInterviewMng, String.IsNullOrEmpty(txtInterviewAs.Text), missingFieldText)
    isValid = isValid And SetErrorIfInvalid(dateEditFrom, errorProviderJobInterviewMng, dateEditFrom.EditValue Is Nothing, missingFieldText)

    Return isValid
  End Function

  ''' <summary>
  ''' Validates a control.
  ''' </summary>
  ''' <param name="control">The control to validate.</param>
  ''' <param name="errorProvider">The error providor.</param>
  ''' <param name="invalid">Boolean flag if data is invalid.</param>
  ''' <param name="errorText">The error text.</param>
  ''' <returns>Valid flag</returns>
  Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

    If (invalid) Then
      errorProvider.SetError(control, errorText)
    Else
      errorProvider.SetError(control, String.Empty)
    End If

    Return Not invalid

  End Function

  ''' <summary>
  ''' Clears the errors.
  ''' </summary>
  Private Sub ClearErrors()
    errorProviderJobInterviewMng.Clear()
  End Sub

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

  ''' <summary>
  ''' Handles drop down button clicks.
  ''' </summary>
  Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

    Const ID_OF_DELETE_BUTTON As Int32 = 1

    ' If delete button has been clicked reset the drop down.
    If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is GridLookUpEdit Then
				Dim lookupEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DateEdit Then
				Dim dateEdit As DateEdit = CType(sender, DateEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
  End Sub

  ''' <summary>
  ''' Opens TODO form.
  ''' </summary>
  Private Sub btnCreateTODO_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateTODO.Click
    Dim frmTodo As New frmTodo(m_InitializationData)
    ' optional init new todo
    Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
    Dim EmployeeNumber As Integer? = m_EmployeeNumber
    Dim CustomerNumber As Integer? = Nothing
    Dim ResponsiblePersonRecordNumber As Integer? = Nothing
    Dim VacancyNumber As Integer? = Nothing
    Dim ProposeNumber As Integer? = Nothing
    Dim ESNumber As Integer? = Nothing
    Dim RPNumber As Integer? = Nothing
    Dim LMNumber As Integer? = Nothing
    Dim RENumber As Integer? = Nothing
    Dim ZENumber As Integer? = Nothing
    Dim Subject As String = String.Empty
    Dim Body As String = ""

    frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber,
                        CustomerNumber, ResponsiblePersonRecordNumber,
                        VacancyNumber, ProposeNumber, ESNumber, RPNumber,
                        LMNumber, RENumber, ZENumber)

    frmTodo.Show()

  End Sub
#End Region

#Region "View helper classes"

  ''' <summary>
  ''' Job interview view data.
  ''' </summary>
  Class JobInterviewViewData

    Public Property ID As Integer
    Public Property RecordNumber As Integer?
    Public Property AppointmentDate As DateTime?
    Public Property JobTitle As String
    Public Property Company As String
    Public Property JobAppointmentState As String
    Public Property Location As String
    Public Property Telephone As String
    Public Property Telefax As String
    Public Property Homepage As String
    Public Property Email As String
    Public Property Outcome As String
    Public Property VakNr As Integer?
    Public Property ProposeNr As Integer?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property CustomerNumber As Integer?
    Public Property ResponsiblePersonRecordNumber As Integer?

  End Class

  ''' <summary>
  ''' Customer view data.
  ''' </summary>
  Class CustomerViewData
    Public Property CustomerNumber As Integer?
    Public Property Company As String
    Public Property Postcode As String
    Public Property Location As String

    ''' <summary>
    ''' Gets the post code and location.
    ''' </summary>
    Public ReadOnly Property PostcodeAndLocation
      Get
        Return String.Format("{0} {1}", Postcode, Location)
      End Get
    End Property
  End Class

  ''' <summary>
  ''' Responsible person view data.
  ''' </summary>
  Class ResponsiblePersonViewData
    Public Property ID As Integer
    Public Property RecordNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property TranslatedSalutation As String
    Public Property Lastname As String
    Public Property Firstname As String
    Public Property Postcode As String
    Public Property Location As String
    Public Property Telephone As String
    Public Property Telefax As String
    Public Property Email As String
    Public Property Homepage As String
		Public Property TranslatedZState1 As String
		Public Property TranslatedZState2 As String
		Public Property ZState1 As String
		Public Property ZState2 As String


    ''' <summary>
    ''' Gets the lastname and firstname.
    ''' </summary>
    Public ReadOnly Property LastnameFirstname
      Get
				Return String.Format("{0}, {1}", Lastname, Firstname)
      End Get
    End Property

    ''' <summary>
    ''' Gets the salutuation, lastname and firstname.
    ''' </summary>
    Public ReadOnly Property SalutationLastnameFirstname
      Get
				Return String.Format("{0} {1} {2}", TranslatedSalutation, Firstname, Lastname)
      End Get
    End Property

    ''' <summary>
    ''' Gets the  postcode and location.
    ''' </summary>
    Public ReadOnly Property PostcodeLocation
      Get
        Return String.Format("{0} {1}", Postcode, Location)
      End Get
    End Property

		Public ReadOnly Property IsZHDActiv As Boolean?
			Get
				Dim isZActiv As Boolean = True
				Dim state1 As String = If(String.IsNullOrWhiteSpace(ZState1), String.Empty, ZState1.ToLower)
				Dim state2 As String = If(String.IsNullOrWhiteSpace(ZState2), String.Empty, ZState2.ToLower)

				isZActiv = Not (state1.Contains("inaktiv") OrElse state1.Contains("mehr aktiv") OrElse state2.Contains("inaktiv") OrElse state2.Contains("mehr aktiv"))
				Return isZActiv
			End Get
		End Property

  End Class

  ''' <summary>
  ''' Appointment state view data.
  ''' </summary>
  Class AppointmentStateViewData
    Public Property ID As Integer
    Public Property Description As String
  End Class

  ''' <summary>
  ''' Vacancy view data.
  ''' </summary>
  Class VacancyViewData
    Public Property VacancyNumber As Integer?
    Public Property Description As String
		Public Property VakState As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

  ''' <summary>
  ''' Propose view data.
  ''' </summary>
  Class ProposeViewData
    Public Property ProposeNumber As Integer?
    Public Property Description As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property P_State As String

	End Class

#End Region

  ''' <summary>
  ''' Inital data for new job interview.
  ''' </summary>
  Public Class InitalDataForJobInterview


    Public Property InterviewAs As String
    Public Property InteviewDate As DateTime?
    Public Property CustomerNumber As Integer?
    Public Property ResponsiblePersonNumber As Integer?
    Public Property IDState As Integer?
    Public Property Result As String
    Public Property VakNr As Integer?
    Public Property ProposeNr As Integer?


  End Class

End Class