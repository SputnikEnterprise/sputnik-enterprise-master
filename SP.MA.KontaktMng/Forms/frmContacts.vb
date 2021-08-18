
Imports System.Reflection.Assembly

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.ucListSelectPopup
Imports System.IO
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraBars
Imports SP.MA.KontaktMng.Settings
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports System.Runtime.Serialization

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SP.TodoMng
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports System.Text.RegularExpressions


''' <summary>
''' Contact management.
''' </summary>
Public Class frmContacts

	Public Delegate Sub ContactDataSavedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)
	Public Delegate Sub ContactDataDeletedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)

#Region "Private Consts"

	Private Const POPUP_DEFAULT_WIDTH As Integer = 420
	Private Const POPUP_DEFAULT_HEIGHT As Integer = 325

#End Region

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
	''' The customer database access.
	''' </summary>
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

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
	''' Record number of selected contact.
	''' </summary>
	Private m_CurrentContactRecordNumber As Integer?

	''' <summary>
	''' Current file bytes.
	''' </summary>
	Private m_CurrentFileBytes As Byte()
	Private m_CurrentFileExtension As String

	''' <summary>
	''' Current document id.
	''' </summary>
	Private m_CurrentDocumentID As Integer?

	''' <summary>
	''' Boolean flag indicating if initial data has been loaded.
	''' </summary>
	Private m_IsInitialDataLoaded As Boolean = False
	Private m_AllowedDelete As Boolean

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' Employee popup data.
	''' </summary>
	Private m_EmployeePopupData As IEnumerable(Of EmployeeViewData)

	''' <summary>
	''' Employee popup column definitions.
	''' </summary>
	Private m_EmployeePopupColumns As New List(Of PopupColumDefintion)

	''' <summary>
	''' Popup to delete a pdf document.
	''' </summary>
	Private m_DeletePDFPopup As PopupMenu

	''' <summary>
	''' The employee contact data.
	''' </summary>
	Private m_EmployeeContactData As EmployeeContactData

	''' <summary>
	''' Employee dependent customer contact data.
	''' </summary>
	Private m_DependentCustomerContactData As EmployeeDependentCustomerContactData

	Private m_md As Mandant

#End Region

#Region "Events"

	Public Event ContactDataSaved As ContactDataSavedHandler
	Public Event ContactDataDeleted As ContactDataDeletedHandler

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
		Try
			' Mandantendaten
			m_md = New Mandant
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		gvContactData.OptionsView.ShowIndicator = False

		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		' Column defintions for popups
		m_EmployeePopupColumns.Add(New PopupColumDefintion With {.Name = "Name", .Translation = "Name"})
		m_EmployeePopupColumns.Add(New PopupColumDefintion With {.Name = "Address", .Translation = "Adresse"})

		AddHandler gvContactData.RowCellClick, AddressOf OnGvContactData_RowCellClick

		AddHandler dateEditFrom.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueContactType.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCustomerName.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueVacancy.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePropose.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueES.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler chkTelephone.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
		AddHandler chkOffered.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
		AddHandler chkMailed.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
		AddHandler chkSMS.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged

		Dim communicationHub = MessageService.Instance.Hub
		communicationHub.Subscribe(Of CustomerContactDataHasChanged)(AddressOf HandleCustomerDataHasChangedMsg)

		Reset()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets the selected contact view data.
	''' </summary>
	''' <returns>The selected contact or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedContactViewData As ContactViewData
		Get
			Dim grdView = TryCast(gridContactData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), ContactViewData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the first contact in the list of contact
	''' </summary>
	''' <returns>First contact in list or nothing.</returns>
	Public ReadOnly Property FirstContactInListOfContacts As ContactViewData
		Get
			If gvContactData.RowCount > 0 Then

				Dim rowHandle = gvContactData.GetVisibleRowHandle(0)
				Return CType(gvContactData.GetRow(rowHandle), ContactViewData)
			Else
				Return Nothing
			End If

		End Get
	End Property


	''' <summary>
	''' Gets the current contact record number.
	''' </summary>
	''' <returns>The current contact record number.</returns>
	Public ReadOnly Property CurrentContactRecordNumber As Integer?
		Get
			Return m_CurrentContactRecordNumber
		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Activates new contact data mode.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="initalDataForNewContact">The inital data for new contact.</param>
	''' <param name="contactFilterSettings">The contact filter settings (optional).</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function ActivateNewContactDataMode(ByVal employeeNumber As Integer, ByVal initalDataForNewContact As InitalDataForNewContact, ByVal contactFilterSettings As ContactFilterSettings) As Boolean

		Dim success As Boolean = PrepareForm(employeeNumber, contactFilterSettings)

		PrepareForNew(initalDataForNewContact)

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))
		End If

		Return success
	End Function

	''' <summary>
	''' Loads the contact data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="contactRecordNumber">The contact record number.</param>
	''' <param name="contactFilterSettings">The contact filter settings (optional).</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function LoadContactData(ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer, ByVal contactFilterSettings As ContactFilterSettings) As Boolean

		Dim success As Boolean = PrepareForm(employeeNumber, contactFilterSettings)

		If contactRecordNumber Then
			success = success AndAlso LoadContactDetailData(employeeNumber, contactRecordNumber)
			FocusContact(employeeNumber, contactRecordNumber)
		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))
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

		Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
		Me.btnNewContact.Text = m_Translate.GetSafeTranslationValue(Me.btnNewContact.Text)
		Me.btnDeleteContact.Text = m_Translate.GetSafeTranslationValue(Me.btnDeleteContact.Text)
		Me.btnCopy.Text = m_Translate.GetSafeTranslationValue(Me.btnCopy.Text)
		Me.btnCreateTODO.Text = m_Translate.GetSafeTranslationValue(Me.btnCreateTODO.Text)

		Me.grpDetail.Text = m_Translate.GetSafeTranslationValue(Me.grpDetail.Text)
		Me.chkImportant.Text = m_Translate.GetSafeTranslationValue(Me.chkImportant.Text)
		Me.chkFinished.Text = m_Translate.GetSafeTranslationValue(Me.chkFinished.Text)

		Me.lblDatum.Text = m_Translate.GetSafeTranslationValue(Me.lblDatum.Text)
		Me.lblkategorie.Text = m_Translate.GetSafeTranslationValue(Me.lblkategorie.Text)
		Me.lbl.Text = m_Translate.GetSafeTranslationValue(Me.lbl.Text)
		Me.lblBeschreibung.Text = m_Translate.GetSafeTranslationValue(Me.lblBeschreibung.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)
		Me.lblKunde.Text = m_Translate.GetSafeTranslationValue(Me.lblKunde.Text)
		Me.lblVakanz.Text = m_Translate.GetSafeTranslationValue(Me.lblVakanz.Text)
		Me.lblVorschlag.Text = m_Translate.GetSafeTranslationValue(Me.lblVorschlag.Text)
		Me.lblEinsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatz.Text)
		Me.lblErstellt.Text = m_Translate.GetSafeTranslationValue(Me.lblErstellt.Text)
		Me.lblgeaendert.Text = m_Translate.GetSafeTranslationValue(Me.lblgeaendert.Text)

		Me.grpFilter.Text = m_Translate.GetSafeTranslationValue(Me.grpFilter.Text)
		Me.chkTelephone.Text = m_Translate.GetSafeTranslationValue(Me.chkTelephone.Text)
		Me.chkMailed.Text = m_Translate.GetSafeTranslationValue(Me.chkMailed.Text)
		Me.chkOffered.Text = m_Translate.GetSafeTranslationValue(Me.chkOffered.Text)
		Me.chkSMS.Text = m_Translate.GetSafeTranslationValue(Me.chkSMS.Text)

		Me.grpJahr.Text = m_Translate.GetSafeTranslationValue(Me.grpJahr.Text)


	End Sub

	''' <summary>
	''' Prepares the form for data presentation.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="contactFilterSettings">The conact filter settings.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function PrepareForm(ByVal employeeNumber As Integer, ByVal contactFilterSettings As ContactFilterSettings) As Boolean

		Dim success As Boolean = True

		If Not m_IsInitialDataLoaded OrElse
				Not m_EmployeeNumber = employeeNumber Then
			success = success AndAlso LoadEmployeeNameData(employeeNumber)
			success = success AndAlso LoadDropDown(employeeNumber)
			m_IsInitialDataLoaded = True
		End If

		' Reset the form
		Reset()

		m_EmployeeNumber = employeeNumber

		Dim initalYearFilter As Integer() = Nothing

		If Not contactFilterSettings Is Nothing Then

			m_SuppressUIEvents = True
			chkTelephone.Checked = contactFilterSettings.ExcluePhone
			chkMailed.Checked = contactFilterSettings.ExclueMail
			chkOffered.Checked = contactFilterSettings.ExclueOffered
			chkSMS.Checked = contactFilterSettings.ExcludeSMS
			m_SuppressUIEvents = False

			initalYearFilter = contactFilterSettings.Years
		End If

		success = success AndAlso LoadYearsListData(employeeNumber, initalYearFilter)
		success = success AndAlso FilterContactData(employeeNumber)

		Return success

	End Function

	''' <summary>
	''' Loads the employee name data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean flag indicating success</returns>
	Private Function LoadEmployeeNameData(ByVal employeeNumber As Integer) As Boolean

		Dim employee = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber)

		If (employee Is Nothing) Then
			Return False
		End If

		Dim salutation As String = String.Empty

		If String.IsNullOrEmpty(employee.Gender) Then
			salutation = "Frau/Herr"
		Else
			Select Case employee.Gender.Trim().ToUpper()
				Case "M"
					salutation = "Herr"
				Case "W"
					salutation = "Frau"

			End Select
		End If
		salutation = m_Translate.GetSafeTranslationValue(salutation)

		Dim strFormCaption As String = m_Translate.GetSafeTranslationValue(Me.Text)
		If String.IsNullOrEmpty(employee.Firstname) Then
			Text = String.Format("{0}: [{1} {2}]", strFormCaption, salutation, employee.Lastname)

		Else
			Text = String.Format("{0}: [{1} {2} {3}]", strFormCaption, salutation, employee.Firstname, employee.Lastname)
		End If

		Return True
	End Function

	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	Private Function LoadDropDown(ByVal employeeNumber As Integer) As Boolean

		Dim success = True
		success = success AndAlso LoadContactType1DropDownData()
		success = success AndAlso LoadCustomerDropDownData()
		success = success AndAlso LoadVacancyProposeAndEsDataBasedOnSelectedCustomer()

		Return success
	End Function

	''' <summary>
	''' Load contact type1 drop down data.
	''' </summary>
	Private Function LoadContactType1DropDownData() As Boolean

		Dim contactType1Data = m_CommonDatabaseAccess.LoadContactTypeData1()

		Dim contactType1ViewData = Nothing

		If Not contactType1Data Is Nothing Then

			contactType1ViewData = New List(Of ContactType1ViewData)

			Dim language = m_ClsProgSetting.GetUSLanguage().Trim().ToLower()
			Dim caption As String = String.Empty
			For Each contactType1 In contactType1Data

				Select Case language
					Case "d", "de"
						caption = contactType1.Caption_DE
					Case "f", "fr"
						caption = contactType1.Caption_FR
					Case "i", "it"
						caption = contactType1.Caption_IT
					Case "e", "en"
						caption = contactType1.Caption_EN
					Case Else
						caption = contactType1.Caption_DE
				End Select

				contactType1ViewData.Add(New ContactType1ViewData With {
																 .Caption = caption,
																 .Bez_ID = contactType1.Bez_ID})
			Next

		End If

		lueContactType.Properties.DataSource = contactType1ViewData
		lueContactType.Properties.DropDownRows = Math.Min(contactType1ViewData.Count, 20)

		lueContactType.Properties.ForceInitialize()

		Return Not contactType1ViewData Is Nothing

	End Function

	''' <summary>
	''' Loads customer drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim customerData = m_EmployeeDatabaseAccess.LoadCustomerDataForContactMng()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueCustomerName.Properties.DataSource = customerData
		m_SuppressUIEvents = suppressUIEventsState

		Return True

	End Function

	''' <summary>
	''' Loads vacancy drop down data.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	Private Function LoadVacancyDropDownData(ByVal customerNumber As Integer?) As Boolean
		Dim vacancyData = m_EmployeeDatabaseAccess.LoadVacancyDataForContactMng(customerNumber)

		lueVacancy.Properties.DataSource = vacancyData
		lueVacancy.Properties.Buttons(0).Enabled = vacancyData.Count > 0

		Return Not vacancyData Is Nothing
	End Function

	''' <summary>
	''' Loads propose drop down data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="customerNumber">The customer number.</param>
	Private Function LoadProposeDropDownData(ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?) As Boolean
		Dim proposeData = m_EmployeeDatabaseAccess.LoadProposeDataForContactMng(employeeNumber, customerNumber)

		luePropose.Properties.DataSource = proposeData
		luePropose.Properties.Buttons(0).Enabled = proposeData.Count > 0

		Return Not proposeData Is Nothing
	End Function

	''' <summary>
	''' Loads ES drop down data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="customerNumber">The customer number.</param>
	Private Function LoadESDropDownData(ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?) As Boolean
		Dim esData = m_EmployeeDatabaseAccess.LoadESDataForContactMng(employeeNumber, customerNumber)

		lueES.Properties.DataSource = esData
		lueES.Properties.Buttons(0).Enabled = esData.Count > 0

		Return Not esData Is Nothing
	End Function

	''' <summary>
	''' Resets the from.
	''' </summary>
	Private Sub Reset()

		m_EmployeeNumber = 0
		m_CurrentContactRecordNumber = Nothing

		m_CurrentFileBytes = Nothing
		m_CurrentFileExtension = String.Empty

		m_CurrentDocumentID = Nothing
		m_EmployeeContactData = Nothing
		m_DependentCustomerContactData = Nothing

		lueContactType.EditValue = Nothing

		txtTitle.Text = String.Empty
		txtTitle.Properties.MaxLength = 1000

		txtDescription.Text = String.Empty
		txtDescription.Properties.MaxLength = 20000

		chkImportant.Checked = False
		chkFinished.Checked = False

		cboFilename.Text = String.Empty
		Me.cboFilename.Properties.Buttons(1).Enabled = False


		' ---Reset drop downs, grids and lists---

		ResetCustomerDropDown()
		ResetContactType1DropDown()
		ResetVacancyDropDown()
		ResetProposeDropDown()
		ResetESDropDown()

		ResetContactGrid()

		TranslateControls()

		EnableOrDisableContentsPanelControls(True)
		btnSave.Enabled = True
		btnNewContact.Enabled = True

		m_AllowedDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 136, m_InitializationData.MDData.MDNr)
		btnDeleteContact.Enabled = m_AllowedDelete

		btnCopy.Visible = False
		lblWarnOnCopy.Visible = False

		' Clear errors
		errorProviderContactManagement.Clear()
	End Sub

	''' <summary>
	''' Resets the contact type 1 drop down.
	''' </summary>
	Private Sub ResetContactType1DropDown()

		lueContactType.Properties.DisplayMember = "Caption"
		lueContactType.Properties.ValueMember = "Bez_ID"

		Dim columns = lueContactType.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Caption", 0))

		lueContactType.Properties.ShowHeader = False
		lueContactType.Properties.ShowFooter = False

		lueContactType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueContactType.Properties.SearchMode = SearchMode.AutoComplete
		lueContactType.Properties.AutoSearchColumnIndex = 0

		lueContactType.Properties.NullText = String.Empty
		lueContactType.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the customer person drop down.
	''' </summary>
	Private Sub ResetCustomerDropDown()

		lueCustomerName.Properties.DisplayMember = "Company1"
		lueCustomerName.Properties.ValueMember = "CustomerNumber"

		gvCustomer.OptionsView.ShowIndicator = False
		gvCustomer.OptionsView.ShowColumnHeaders = True
		gvCustomer.OptionsView.ShowFooter = False

		gvCustomer.OptionsView.ShowAutoFilterRow = True
		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCustomer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerNumber.MaxWidth = 50
		gvCustomer.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnPostcodeAndLocation)

		lueCustomerName.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomerName.Properties.NullText = String.Empty
		lueCustomerName.EditValue = Nothing

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

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columncustomername.Name = "Customername"
		columncustomername.FieldName = "Customername"
		columncustomername.Visible = True
		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername.MaxWidth = 200
		gvVacancy.Columns.Add(columncustomername)

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

		Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnEmployeeFullname.Name = "Customername"
		columnEmployeeFullname.FieldName = "Customername"
		columnEmployeeFullname.Visible = False
		columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullname.MaxWidth = 200
		gvPropose.Columns.Add(columnEmployeeFullname)

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
	''' Resets the ES drop down.
	''' </summary>
	Private Sub ResetESDropDown()
		lueES.Properties.DisplayMember = "ESDataShowESAsWithDate"
		lueES.Properties.ValueMember = "ESNumber"

		gvES.OptionsView.ShowIndicator = False
		gvES.OptionsView.ShowColumnHeaders = True
		gvES.OptionsView.ShowFooter = False
		gvES.OptionsView.ShowAutoFilterRow = True
		gvES.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvES.Columns.Clear()

		Dim columnESNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnESNumber.Name = "ESNumber"
		columnESNumber.FieldName = "ESNumber"
		columnESNumber.Visible = True
		gvES.Columns.Add(columnESNumber)

		Dim columnCustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCustomername.Name = "customername"
		columnCustomername.FieldName = "customername"
		columnCustomername.Visible = False
		columnCustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvES.Columns.Add(columnCustomername)

		Dim columnES_As As New DevExpress.XtraGrid.Columns.GridColumn()
		columnES_As.Caption = m_Translate.GetSafeTranslationValue("Einsatz als")
		columnES_As.Name = "ES_As"
		columnES_As.FieldName = "ES_As"
		columnES_As.Visible = True
		columnES_As.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvES.Columns.Add(columnES_As)

		Dim columnVState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVState.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnVState.Name = "ESPeriode"
		columnVState.FieldName = "ESPeriode"
		columnVState.Visible = True
		columnVState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvES.Columns.Add(columnVState)


		lueES.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueES.Properties.NullText = String.Empty
		lueES.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the contact grid.
	''' </summary>
	Private Sub ResetContactGrid()

		' Reset the grid
		gvContactData.Columns.Clear()

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnDate.Name = "ContactDate"
		columnDate.FieldName = "ContactDate"
		columnDate.Visible = True
		columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gvContactData.Columns.Add(columnDate)

		Dim personSubjectColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		personSubjectColumn.Caption = m_Translate.GetSafeTranslationValue("Person / Betreff")
		personSubjectColumn.Name = "Person_Subject"
		personSubjectColumn.FieldName = "Person_Subject"
		personSubjectColumn.Visible = True
		gvContactData.Columns.Add(personSubjectColumn)

		Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
		docType.Caption = " "
		docType.Name = "docType"
		docType.FieldName = "docType"
		docType.Visible = True
		Dim picutureEdit As New RepositoryItemPictureEdit()
		picutureEdit.NullText = " "
		docType.ColumnEdit = picutureEdit
		docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
		docType.Width = 20
		gvContactData.Columns.Add(docType)

	End Sub

	''' <summary>
	''' Loads the years drop down data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadYearsListData(ByVal employeeNumber As Integer, Optional ByVal yearsToSelect As Integer() = Nothing) As Boolean
		Dim yearsData = m_EmployeeDatabaseAccess.LoadEmployeeContactDistinctYears(employeeNumber)

		If (yearsData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre konnten nicht geladen werden."))
		End If

		m_SuppressUIEvents = True

		Dim selectedYears As New List(Of Integer)

		If Not yearsToSelect Is Nothing Then
			selectedYears.AddRange(yearsToSelect)
		Else

			' Remember currently checked years
			If lstYears.Items.Count > 0 Then
				For Each item As CheckedListBoxItem In lstYears.CheckedItems
					selectedYears.Add(item.Value)
				Next
			End If
		End If

		' Load years from database.
		lstYears.Items.Clear()
		For Each yearEntry In yearsData
			lstYears.Items.Add(yearEntry)
		Next

		' Recheck previously selected years.
		If selectedYears.Count > 0 Then
			For Each iYear In selectedYears
				Dim index = lstYears.Items.IndexOf(iYear)

				If index > -1 Then
					lstYears.Items(index).CheckState = CheckState.Checked
				End If

			Next
		Else
			' Select current year.
			Dim index = lstYears.Items.IndexOf(Date.Now.Date.Year)
			lstYears.Items(index).CheckState = CheckState.Checked
		End If

		m_SuppressUIEvents = False

		Return Not yearsData Is Nothing
	End Function

	''' <summary>
	''' Loads contact data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function FilterContactData(ByVal employeeNumber As Integer) As Boolean

		' Convert check years to a integer array.
		Dim selectedYears = lstYears.CheckedItems
		Dim filterYears As New List(Of Integer)

		For Each yar In selectedYears
			filterYears.Add(CType(yar, CheckedListBoxItem).Value)
		Next

		' If no years are checked enter a impossible year (-> nothing will be found).
		If (filterYears.Count = 0) Then
			filterYears.Add(-1)
		End If

		Dim yearsArray = filterYears.ToArray()
		Dim contactData = m_EmployeeDatabaseAccess.LoadEmployeeContactOverviewDataBySearchCriteria(employeeNumber, Not chkTelephone.Checked, Not chkOffered.Checked, Not chkMailed.Checked, Not chkSMS.Checked, yearsArray)

		If (contactData Is Nothing) Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))

			Return False
		End If

		Dim listDataSource As BindingList(Of ContactViewData) = New BindingList(Of ContactViewData)

		' Convert the data to view data.
		For Each p In contactData

			Dim cViewData = New ContactViewData() With {
					.EmployeeNumber = p.EmployeeNumber,
					.ContactRecordNumber = p.RecNr,
					.ContactDate = p.ContactDate,
					.Person_Subject = p.PersonOrSubject,
					.Description = p.Description,
					.Important = p.IsImportant,
					.Completed = p.IsCompleted,
					.KST = p.CreatedFrom,
					.KDKontactRecID = p.KDKontactRecID}

			If p.DocumentID.HasValue Then
				cViewData.PDFImage = My.Resources.DocumentAttach
				cViewData.DocumentId = p.DocumentID
			End If

			listDataSource.Add(cViewData)
		Next

		m_SuppressUIEvents = True
		gridContactData.DataSource = listDataSource
		m_SuppressUIEvents = False

		Return True
	End Function


	''' <summary>
	''' Handles focus change of contact row.
	''' </summary>
	Private Sub OnContact_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvContactData.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedContact = SelectedContactViewData

		If Not selectedContact Is Nothing Then
			LoadContactDetailData(selectedContact.EmployeeNumber, selectedContact.ContactRecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles double click on contact.
	''' </summary>
	Private Sub OnContact_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridContactData.DoubleClick

		Dim selectedContact = SelectedContactViewData

		If Not selectedContact Is Nothing Then
			LoadContactDetailData(selectedContact.EmployeeNumber, selectedContact.ContactRecordNumber)
		End If
	End Sub

	''' <summary>
	'''  Handles RowStyle event of gvVacancy grid view.
	''' </summary>
	Private Sub OnGvVacancy_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvVacancy.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvVacancy.GetRow(e.RowHandle), VacancyDataForContactMng)

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

			Dim rowData = CType(gvPropose.GetRow(e.RowHandle), ProposeDataForContactMng)

			If Not rowData.P_State Is Nothing AndAlso (rowData.P_State.ToLower.Contains("absage") Or rowData.P_State.ToLower.Contains("abgeschlossen")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub


	''' <summary>
	''' Handles unbound column data event.
	''' </summary>
	Private Sub OnGvContacts_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvContactData.CustomUnboundColumnData

		If e.Column.Name = "docType" Then
			If (e.IsGetData()) Then
				e.Value = CType(e.Row, ContactViewData).PDFImage
			End If
		End If
	End Sub

	''' <summary>
	''' Loads contact detail data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="contactNumber">The contact number.</param>
	Private Function LoadContactDetailData(ByVal employeeNumber As Integer, ByVal contactNumber As Integer) As Boolean

		Dim success As Boolean = True

		' Clear errors
		errorProviderContactManagement.Clear()

		Dim contactResult = m_EmployeeDatabaseAccess.LoadEmployeeContact(employeeNumber, contactNumber)
		m_EmployeeContactData = contactResult

		If Not contactResult Is Nothing Then

			dateEditFrom.EditValue = contactResult.ContactDate
			timeStart.EditValue = contactResult.ContactDate

			lueContactType.EditValue = contactResult.ContactType1

			txtTitle.Text = contactResult.ContactPeriodString
			txtDescription.Text = contactResult.ContactsString

			chkImportant.Checked = If(contactResult.ContactImportant.HasValue, contactResult.ContactImportant, False)
			chkFinished.Checked = If(contactResult.ContactFinished.HasValue, contactResult.ContactFinished, False)

			m_CurrentDocumentID = contactResult.KontaktDocID
			Me.cboFilename.Properties.Buttons(1).Enabled = m_CurrentDocumentID.HasValue

			m_SuppressUIEvents = True
			cboFilename.Text = String.Empty
			m_SuppressUIEvents = False

			lueCustomerName.EditValue = contactResult.CustomerNumber
			lueVacancy.EditValue = contactResult.VacancyNumber
			luePropose.EditValue = contactResult.ProposeNr
			lueES.EditValue = contactResult.ESNr

			m_CurrentFileBytes = Nothing
			m_CurrentFileExtension = String.Empty

			lblContactCreated.Text = String.Format("{0:f}, {1}", contactResult.CreatedOn, contactResult.CreatedFrom)
			lblContactChanged.Text = String.Format("{0:f}, {1}", contactResult.ChangedOn, contactResult.ChangedFrom)

			m_CurrentContactRecordNumber = contactResult.RecordNumber

			Dim isCopiedRecord = (contactResult.CustomerContactRecId.HasValue)

			EnableOrDisableContentsPanelControls(Not isCopiedRecord)
			btnSave.Enabled = Not isCopiedRecord
			btnDeleteContact.Enabled = Not isCopiedRecord AndAlso m_AllowedDelete
			btnCopy.Visible = isCopiedRecord
			lblWarnOnCopy.Visible = isCopiedRecord

			' Load a dependent customer contact (if none exists-> nothing is returned)
			m_DependentCustomerContactData = m_EmployeeDatabaseAccess.LoadEmployeeDependentCustomerContactData(contactResult.ID)

		Else
			success = False
		End If

		Return success
	End Function

	''' <summary>
	''' Prepare form for new contact.
	''' </summary>
	''' <param name="initalDataForNewContact">The inital data for new contact.</param>
	Private Sub PrepareForNew(Optional ByVal initalDataForNewContact As InitalDataForNewContact = Nothing)

		m_CurrentFileBytes = Nothing
		m_CurrentFileExtension = String.Empty
		m_CurrentContactRecordNumber = Nothing
		m_CurrentDocumentID = Nothing
		m_EmployeeContactData = Nothing
		m_DependentCustomerContactData = Nothing

		dateEditFrom.EditValue = Nothing
		timeStart.EditValue = Nothing
		lueContactType.EditValue = Nothing
		txtTitle.Text = String.Empty
		txtDescription.Text = String.Empty
		chkImportant.Checked = False
		chkFinished.Checked = False
		cboFilename.Text = String.Empty

		lueCustomerName.EditValue = Nothing
		lueVacancy.EditValue = Nothing
		luePropose.EditValue = Nothing
		lueES.EditValue = Nothing

		Me.cboFilename.Properties.Buttons(1).Enabled = False
		EnableOrDisableContentsPanelControls(True)
		btnSave.Enabled = True
		btnNewContact.Enabled = True
		btnDeleteContact.Enabled = m_AllowedDelete
		btnCopy.Visible = False
		lblWarnOnCopy.Visible = False

		lblContactCreated.Text = "-"
		lblContactChanged.Text = "-"

		' Inital form data 
		If Not initalDataForNewContact Is Nothing Then

			' StartDateTime
			If initalDataForNewContact.StartDateTime.HasValue Then
				initalDataForNewContact.StartDateTime = initalDataForNewContact.StartDateTime.Value.AddSeconds(-initalDataForNewContact.StartDateTime.Value.Second)

				dateEditFrom.EditValue = initalDataForNewContact.StartDateTime
				timeStart.EditValue = initalDataForNewContact.StartDateTime
			End If

			' ContactTypeBezID
			If Not String.IsNullOrEmpty(initalDataForNewContact.ContactTypeBezID) Then
				lueContactType.EditValue = initalDataForNewContact.ContactTypeBezID
			End If

			' customerNumber
			lueCustomerName.EditValue = initalDataForNewContact.customerNumber

			' title
			txtTitle.Text = initalDataForNewContact.title

			'description
			txtDescription.Text = initalDataForNewContact.description

			' customerVacancyNumber
			If initalDataForNewContact.customerVacancyNumber.HasValue Then
				lueVacancy.EditValue = initalDataForNewContact.customerVacancyNumber
			End If

			' customerProposeNumber
			If initalDataForNewContact.customerProposeNumber.HasValue Then
				luePropose.EditValue = initalDataForNewContact.customerProposeNumber
			End If

			' customerESNumber
			If initalDataForNewContact.customerESNumber.HasValue Then
				lueES.EditValue = initalDataForNewContact.customerESNumber
			End If


		End If

		txtTitle.Focus()

		' Clear errors
		errorProviderContactManagement.Clear()

	End Sub

	''' <summary>
	''' Handles click on save button.
	''' </summary>
	Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

		If ValidateContactInputData() Then

			Dim contactData As EmployeeContactData = Nothing

			Dim dt = DateTime.Now
			If Not m_CurrentContactRecordNumber.HasValue Then
				contactData = New EmployeeContactData With {.EmployeeNumber = m_EmployeeNumber,
																																			 .CreatedOn = dt,
																																			 .CreatedFrom = m_ClsProgSetting.GetUserName()}
			Else

				contactData = m_EmployeeDatabaseAccess.LoadEmployeeContact(m_EmployeeNumber, m_CurrentContactRecordNumber)

				If contactData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

			End If

			If Not (ContinueInCaseOfChangedCustomer()) Then
				Return
			End If

			contactData.EmployeeNumber = m_EmployeeNumber
			contactData.ContactDate = CombineDateAndTime(dateEditFrom.EditValue, timeStart.EditValue)
			contactData.ContactType1 = If(lueContactType.EditValue Is Nothing, 0, lueContactType.EditValue)
			contactData.ContactPeriodString = txtTitle.Text
			contactData.ContactsString = txtDescription.Text
			contactData.ContactImportant = chkImportant.Checked
			contactData.ContactFinished = chkFinished.Checked
			contactData.VacancyNumber = lueVacancy.EditValue
			contactData.ProposeNr = luePropose.EditValue
			contactData.ESNr = lueES.EditValue
			contactData.CustomerNumber = lueCustomerName.EditValue

			contactData.ChangedFrom = m_ClsProgSetting.GetUserName()
			contactData.ChangedOn = dt
			contactData.UsNr = m_InitializationData.UserData.UserNr

			Dim success As Boolean = True

			' Check if the document bytes must be saved.
			If Not (m_CurrentFileBytes Is Nothing) And success Then

				Dim contactDocument As ContactDoc = Nothing

				If Not m_CurrentDocumentID.HasValue Then

					contactDocument = New ContactDoc() With {.CreatedOn = dt,
																									 .CreatedFrom = m_ClsProgSetting.GetUserName(),
																									 .FileBytes = m_CurrentFileBytes,
																									 .FileExtension = m_CurrentFileExtension}
					success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

					If success Then
						m_CurrentDocumentID = contactDocument.ID
						contactData.KontaktDocID = m_CurrentDocumentID
					End If

				Else
					contactDocument = m_EmployeeDatabaseAccess.LoadContactDocumentData(m_CurrentDocumentID.Value, False)

					If Not contactDocument Is Nothing Then

						contactDocument.FileBytes = m_CurrentFileBytes
						contactDocument.FileExtension = m_CurrentFileExtension
						success = success AndAlso m_EmployeeDatabaseAccess.UpdateContactDocumentData(contactDocument, False)
					Else
						success = False
					End If

				End If
			End If

			m_CurrentFileBytes = Nothing
			m_CurrentFileExtension = String.Empty

			m_SuppressUIEvents = True
			cboFilename.Text = String.Empty
			m_SuppressUIEvents = False

			' Insert or update contact
			If contactData.ID = 0 Then
				contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

				If success Then
					m_CurrentContactRecordNumber = contactData.RecordNumber
				End If

			Else
				contactData.ChangedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeContact(contactData)
			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else

				' ---Copy contact---

				If (contactData.CustomerNumber.HasValue) Then
					success = success AndAlso CopyContactForCustomer(contactData)
				Else

					' Customer is cleared in UI; Delete dependent record.
					If Not m_DependentCustomerContactData Is Nothing Then
						success = success AndAlso m_CustomerDatabaseAccess.DeleteResponsiblePersonContactAssignment(m_DependentCustomerContactData.KDKontaktID)
						m_DependentCustomerContactData = Nothing
					End If

				End If

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht kopiert werden."))
				End If


				' Update document dates.
				lblContactCreated.Text = String.Format("{0:f}, {1}", contactData.CreatedOn, contactData.CreatedFrom)
				lblContactChanged.Text = String.Format("{0:f}, {1}", contactData.ChangedOn, contactData.ChangedFrom)

				LoadYearsListData(m_EmployeeNumber)
				FilterContactData(m_EmployeeNumber)
				FocusContact(m_EmployeeNumber, contactData.RecordNumber)

				RaiseEvent ContactDataSaved(Me, m_EmployeeNumber, contactData.RecordNumber)

				' Notifiy system about changed contact
				Dim hub = MessageService.Instance.Hub
				Dim employeeContactHasChangedMsg As New EmployeeContactDataHasChanged(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_EmployeeNumber, m_CurrentContactRecordNumber)
				hub.Publish(employeeContactHasChangedMsg)

			End If

		End If
	End Sub

	''' <summary>
	''' Checks if the user wants to continue on changed customer.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function ContinueInCaseOfChangedCustomer() As Boolean

		If m_DependentCustomerContactData IsNot Nothing AndAlso
			 lueCustomerName.EditValue IsNot Nothing AndAlso
			 Not m_DependentCustomerContactData.KDnr = lueCustomerName.EditValue Then

			Dim customerOld = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_DependentCustomerContactData.KDnr, String.Empty)
			Dim customerNew = m_CustomerDatabaseAccess.LoadCustomerMasterData(lueCustomerName.EditValue, String.Empty)

			If customerOld IsNot Nothing AndAlso customerNew IsNot Nothing Then

				Dim messageText = m_Translate.GetSafeTranslationValue("Für diesen Kontakt existiert ein abhängiger Kundenkontakt.") + vbCrLf + vbCrLf +
													m_Translate.GetSafeTranslationValue("Wenn Sie fortfahren, wird der abhängige Kundenkontakt") + vbCrLf +
													String.Format(m_Translate.GetSafeTranslationValue("vom Kunden {0} zum Kunden {1} verschoben."), customerOld.Company1, customerNew.Company1) + vbCrLf + vbCrLf +
													m_Translate.GetSafeTranslationValue("Möchten Sie fortfahren?")

				If Not (m_UtilityUI.ShowYesNoDialog(messageText, m_Translate.GetSafeTranslationValue("Kontakt verschieben"))) Then
					Return False
				End If

			End If
		End If

		Return True
	End Function

	''' <summary>
	''' Copies the contact for customers.
	''' </summary>
	''' <param name="contactData">The contact data to copy.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function CopyContactForCustomer(ByVal contactData As EmployeeContactData) As Boolean

		Dim success As Boolean = True

		Dim dt = DateTime.Now

		Dim copyOfContactData As SP.DatabaseAccess.Customer.DataObjects.ResponsiblePersonAssignedContactData

		If m_DependentCustomerContactData Is Nothing Then
			copyOfContactData = New SP.DatabaseAccess.Customer.DataObjects.ResponsiblePersonAssignedContactData

			copyOfContactData.ID = Nothing
			copyOfContactData.RecordNumber = Nothing
			copyOfContactData.CreatedOn = dt
			copyOfContactData.CreatedFrom = m_InitializationData.UserData.UserFullName
			copyOfContactData.ResponsiblePersonNumber = 0

		Else
			' Update existing contact
			copyOfContactData = m_CustomerDatabaseAccess.LoadAssignedContactDataOfResponsiblePerson(m_DependentCustomerContactData.KDnr, Nothing, m_DependentCustomerContactData.KDKontaktRecNr)

			If copyOfContactData Is Nothing Then
				Return False
			End If
		End If

		copyOfContactData.CustomerNumber = contactData.CustomerNumber
		copyOfContactData.ResponsiblePersonNumber = 0

		copyOfContactData.ContactDate = contactData.ContactDate
		copyOfContactData.ContactsString = contactData.ContactsString
		copyOfContactData.Username = Nothing

		copyOfContactData.ContactType1 = contactData.ContactType1
		copyOfContactData.ContactType2 = contactData.ContactType2
		copyOfContactData.ContactPeriodString = contactData.ContactPeriodString
		copyOfContactData.ContactImportant = contactData.ContactImportant
		copyOfContactData.ContactFinished = contactData.ContactFinished
		copyOfContactData.MANr = 0 ' ???

		copyOfContactData.ProposeNr = contactData.ProposeNr
		copyOfContactData.VacancyNumber = contactData.VacancyNumber
		copyOfContactData.OfNumber = contactData.OfNumber
		copyOfContactData.Mail_ID = contactData.Mail_ID
		copyOfContactData.TaskRecNr = contactData.TaskRecNr
		copyOfContactData.UsNr = contactData.UsNr
		copyOfContactData.ESNr = contactData.ESNr
		copyOfContactData.EmployeeContactRecID = contactData.ID
		copyOfContactData.KontaktDocID = contactData.KontaktDocID

		copyOfContactData.ChangedFrom = m_InitializationData.UserData.UserFullName
		copyOfContactData.ChangedOn = dt

		' Save the contact

		If copyOfContactData.ID > 0 Then
			copyOfContactData.ChangedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.UpdateResponsiblePersonAssignedContactData(copyOfContactData)
		Else
			copyOfContactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(copyOfContactData)
		End If
		m_DependentCustomerContactData = m_EmployeeDatabaseAccess.LoadEmployeeDependentCustomerContactData(contactData.ID)

		Return success
	End Function

	''' <summary>
	''' Handles click on new contact button.
	''' </summary>
	Private Sub OnBtnNewContact_Click(sender As System.Object, e As System.EventArgs) Handles btnNewContact.Click
		Dim initalData As New InitalDataForNewContact With {.StartDateTime = DateTime.Now}
		PrepareForNew(initalData)
	End Sub

	''' <summary>
	''' Handles click on delete contact button.
	''' </summary>
	Private Sub OnBtnDeleteContact_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteContact.Click

		If Not m_CurrentContactRecordNumber Is Nothing Then

			Dim recordToDelete = m_EmployeeDatabaseAccess.LoadEmployeeContact(m_EmployeeNumber, m_CurrentContactRecordNumber)

			If recordToDelete Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kontakt konnte nicht gelöscht werden."))
				Return
			End If

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																			m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then
				Dim success = m_EmployeeDatabaseAccess.DeleteEmployeeContact(recordToDelete.ID)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kontakt konnte nicht gelöscht werden."))

					Return
				Else
					If recordToDelete.KontaktDocID.HasValue Then
						success = success AndAlso m_EmployeeDatabaseAccess.DeleteContactDocument(recordToDelete.KontaktDocID)

						If Not success Then
							m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das angehängte Dokument des Kontaktes konnte nicht gelöscht werden."))
						End If
					End If
				End If

				If Not m_DependentCustomerContactData Is Nothing Then
					success = success AndAlso m_CustomerDatabaseAccess.DeleteResponsiblePersonContactAssignment(m_DependentCustomerContactData.KDKontaktID)
					m_DependentCustomerContactData = Nothing
				End If

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der verbundene Kundenkontakt konnte nicht gelöscht werden."))
				End If

				LoadYearsListData(m_EmployeeNumber)
				FilterContactData(m_EmployeeNumber)

				' Load the contact detail data of the first contact in the list.
				Dim firstContact = FirstContactInListOfContacts

				If Not firstContact Is Nothing Then
					LoadContactDetailData(m_EmployeeNumber, firstContact.ContactRecordNumber)
				Else
					m_CurrentContactRecordNumber = Nothing
					m_EmployeeContactData = Nothing
					m_DependentCustomerContactData = Nothing

					m_CurrentFileBytes = Nothing
					m_CurrentFileExtension = String.Empty
					m_CurrentDocumentID = Nothing
				End If

				m_SuppressUIEvents = True
				cboFilename.Text = String.Empty
				m_SuppressUIEvents = False

				RaiseEvent ContactDataDeleted(Me, m_EmployeeNumber, recordToDelete.RecordNumber)

			End If

		End If

	End Sub

	''' <summary>
	''' Handles click on copy contact button.
	''' </summary>
	Private Sub OnBtnCopy_Click(sender As System.Object, e As System.EventArgs) Handles btnCopy.Click

		Dim contactData As EmployeeContactData = Nothing
		Dim dt = DateTime.Now
		contactData = New EmployeeContactData With {.EmployeeNumber = m_EmployeeNumber,
																																		 .CreatedOn = dt,
																																		 .CreatedFrom = m_ClsProgSetting.GetUserName()}

		contactData.EmployeeNumber = m_EmployeeNumber
		contactData.ContactDate = CombineDateAndTime(dateEditFrom.EditValue, timeStart.EditValue)
		contactData.ContactType1 = If(lueContactType.EditValue Is Nothing, 0, lueContactType.EditValue)
		contactData.ContactPeriodString = txtTitle.Text
		contactData.ContactsString = txtDescription.Text
		contactData.ContactImportant = chkImportant.Checked
		contactData.ContactFinished = chkFinished.Checked
		contactData.VacancyNumber = lueVacancy.EditValue
		contactData.ProposeNr = luePropose.EditValue
		contactData.ESNr = lueES.EditValue
		contactData.CustomerNumber = lueCustomerName.EditValue
		contactData.ChangedFrom = m_ClsProgSetting.GetUserName()
		contactData.ChangedOn = dt
		contactData.UsNr = m_InitializationData.UserData.UserNr

		Dim success As Boolean = True

		' Copy document
		If m_CurrentDocumentID.HasValue Then
			Dim contactDocument As ContactDoc = Nothing

			contactDocument = m_EmployeeDatabaseAccess.LoadContactDocumentData(m_CurrentDocumentID.Value, True)

			If Not contactDocument Is Nothing Then
				contactDocument.ID = 0
				success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

				If success Then
					contactData.KontaktDocID = contactDocument.ID
				End If
			Else
				success = False
			End If

		End If

		contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
		success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

		If success Then
			LoadContactData(m_EmployeeNumber, contactData.RecordNumber, Nothing)
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt konnte nicht kopiert werden."))
		End If

	End Sub

	''' <summary>
	''' Handles click on label warn on copy.
	''' </summary>
	Private Sub OnlblWarnOnCopy_Click(sender As Object, e As EventArgs) Handles lblWarnOnCopy.Click

		Dim contactResult = m_EmployeeDatabaseAccess.LoadEmployeeContact(m_EmployeeNumber, m_CurrentContactRecordNumber)

		If (Not contactResult Is Nothing AndAlso contactResult.CustomerContactRecId.HasValue) Then

			OpenCustomerContact(contactResult.CustomerContactKDNr, contactResult.CustomerContactRecNr)

		End If

	End Sub

	''' <summary>
	''' Handles change of filter checkbox.
	''' </summary>
	Private Sub OnFilterCheckBox_CheckedChanged(sender As System.Object, e As System.EventArgs)

		If m_SuppressUIEvents Then
			Return
		End If

		FilterContactData(m_EmployeeNumber)

		If (m_CurrentContactRecordNumber.HasValue) Then
			FocusContact(m_EmployeeNumber, m_CurrentContactRecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles change of year checkbox.
	''' </summary>
	Private Sub OnLstYears_ItemCheck(sender As System.Object, e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs) Handles lstYears.ItemCheck

		If m_SuppressUIEvents Then
			Return
		End If

		FilterContactData(m_EmployeeNumber)

		If (m_CurrentContactRecordNumber.HasValue) Then
			FocusContact(m_EmployeeNumber, m_CurrentContactRecordNumber)
		End If
	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnFrmContacts_Load(sender As System.Object, e As System.EventArgs) Handles Me.Load

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Me.AllowDrop = True

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
			Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
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
	Private Sub OnFrmContacts_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

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

	End Sub

	''' <summary>
	''' Keypreview for Modul-version
	''' </summary>
	Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
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
	''' Handles the form dragdrop event.
	''' </summary>
	Private Sub OnFrmContacts_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		If e.Data.GetDataPresent("FileGroupDescriptor") Then
			'supports a drop of a Outlook message 
			Dim m_Path As New SPProgUtility.ClsProgSettingPath
			Dim objOL As Object = Nothing
			objOL = CreateObject("Outlook.Application")
			Dim myobj As Object

			For i As Integer = 1 To objOL.ActiveExplorer.Selection.Count
				myobj = objOL.ActiveExplorer.Selection.Item(i)

				'hardcode a destination path for testing
				Dim strFilename As String = myobj.Subject
				Try
					lueContactType.EditValue = "Einzelmail"
					txtTitle.Text = strFilename
					txtDescription.Text = myobj.body
					dateEditFrom.EditValue = CType(Format(myobj.CreationTime, "d"), Date)
					timeStart.EditValue = CType(Format(myobj.CreationTime, "t"), DateTime)

				Catch ex As Exception

				End Try

				strFilename = System.Text.RegularExpressions.Regex.Replace(myobj.Subject, "[\\/:*?""<>|\r\n]", "", System.Text.RegularExpressions.RegexOptions.Singleline)
				strFilename &= ".msg"
				Dim strFile As String = IO.Path.Combine(m_Path.GetSpS2DeleteHomeFolder, strFilename)

				myobj.SaveAs(strFile)
				files = New String() {strFile}
			Next

		Else

		End If

		If Not files Is Nothing AndAlso files.Length > 0 Then
			Dim fileInfo As New FileInfo(files(0))

			'If fileInfo.Extension.ToLower() = ".pdf" OrElse fileInfo.Extension.ToLower() = ".msg" Then
			LoadDocumentBytesFormFileSystem(fileInfo.FullName)
			'End If

		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OnFrmContacts_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
		'Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		e.Effect = DragDropEffects.Copy
		'If e.Data.GetDataPresent("FileGroupDescriptor") Then
		'	e.Effect = DragDropEffects.Copy
		'End If

		'If Not files Is Nothing AndAlso files.Count > 0 Then
		'	Dim fileInfo As New FileInfo(files(0))

		'	If fileInfo.Extension.ToLower() = ".pdf" OrElse fileInfo.Extension.ToLower() = ".msg" Then
		'		e.Effect = DragDropEffects.Copy
		'	Else
		'		e.Effect = DragDropEffects.None
		'	End If

		'End If

	End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is BaseEdit Then
				If CType(sender, BaseEdit).Properties.ReadOnly Then
					' nothing
				Else
					CType(sender, BaseEdit).EditValue = Nothing
				End If
			End If

		End If
	End Sub

	''' <summary>
	''' Reads a popup size setting.
	''' </summary>
	''' <param name="settingKey">The settings key.</param>
	''' <returns>The size setting.</returns>
	Private Function ReadPopupSizeSetting(ByRef settingKey As String) As Size

		' Load width/height setting
		Dim popupSizeSetting As String = String.Empty
		Dim popupSize As Size
		popupSize.Width = POPUP_DEFAULT_WIDTH
		popupSize.Height = POPUP_DEFAULT_HEIGHT

		Try
			popupSizeSetting = m_SettingsManager.ReadString(settingKey)

			If Not String.IsNullOrEmpty(popupSizeSetting) Then
				Dim arrSize As String() = popupSizeSetting.Split(CChar(";"))
				popupSize.Width = arrSize(0)
				popupSize.Height = arrSize(1)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		Return popupSize
	End Function

	''' <summary>
	''' Handles double click on contact grid to open document
	''' </summary>
	Private Sub OnGvContactData_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) And m_CurrentDocumentID.HasValue Then
			OpenDocument()
		End If

	End Sub

	''' <summary>
	''' Handles click on file path button.
	''' Handles click on file open.
	''' </summary>
	Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cboFilename.ButtonClick

		If e.Button.Index = 0 Then

			With OpenFileDialog1
				.Filter = _
				"PDF-Dokumente (*.pdf)|*.pdf"
				.FilterIndex = 0
				.InitialDirectory = If(cboFilename.Text = String.Empty, m_ClsProgSetting.GetUserHomePath, cboFilename.Text)
				.Title = "Dokument öffnen"
				.FileName = String.Empty

				If .ShowDialog() = DialogResult.OK Then

					LoadDocumentBytesFormFileSystem(.FileName)

				End If
			End With

		ElseIf e.Button.Index = 1 Then

			OpenDocument()

		End If

	End Sub

	''' <summary>
	''' Loads document bytes from file system.
	''' </summary>
	''' <param name="filepath">The file path.</param>
	Private Sub LoadDocumentBytesFormFileSystem(ByVal filepath As String)

		cboFilename.Text = String.Empty
		m_CurrentFileBytes = m_Utility.LoadFileBytes(filepath)

		If m_CurrentFileBytes Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Datei konnte nicht geöffnet werden."))

		Else
			cboFilename.Text = filepath

			Dim fileInfo As New FileInfo(filepath)
			m_CurrentFileExtension = fileInfo.Extension
			If m_CurrentFileExtension.StartsWith(".") Then m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")
			Me.cboFilename.Properties.Buttons(1).Enabled = True
			Me.cboFilename.Properties.Buttons(1).Image = If(m_CurrentFileExtension = "msg", My.Resources.Mail2, My.Resources.pdf)
		End If

	End Sub

	''' <summary>
	''' Opens a document
	''' </summary>
	Private Sub OpenDocument()

		Dim bytes As Byte() = Nothing
		Dim extension As String = "pdf"

		If (Not String.IsNullOrEmpty(cboFilename.Text)) Then

			Try
				bytes = File.ReadAllBytes(cboFilename.Text)
				extension = If(cboFilename.Text.EndsWith(".msg"), "msg", extension)

			Catch
				' Do nothing
			End Try
		ElseIf m_CurrentDocumentID.HasValue Then
			Dim documentData = m_EmployeeDatabaseAccess.LoadContactDocumentData(m_CurrentDocumentID, True)

			If Not documentData Is Nothing Then
				bytes = documentData.FileBytes

				If Not documentData.FileExtension Is Nothing AndAlso Not String.IsNullOrWhiteSpace(documentData.FileExtension) Then
					extension = documentData.FileExtension
				End If

			End If

		End If

		Dim tempFileName = System.IO.Path.GetTempFileName()
		Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, extension)

		If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

			m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

		End If

	End Sub

	''' <summary>
	''' Opens a customer contact.
	''' </summary>
	Private Sub OpenCustomerContact(ByVal kdNr As Integer, ByVal contatRecNr As Integer)

		' Send a request to open a customer contact management form.
		Dim hub = MessageService.Instance.Hub
		Dim openCustomerContact As New OpenKDKontaktMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, kdNr, contatRecNr)
		hub.Publish(openCustomerContact)

	End Sub

	''' <summary>
	''' Opens a customer.
	''' </summary>
	Private Sub OpenCustomer(ByVal kdNr As Integer)
		' Send a request to open a customerMng form.
		Dim hub = MessageService.Instance.Hub
		Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, kdNr)
		hub.Publish(openCustomerMng)
	End Sub

	''' <summary>
	''' Handles text change of open file name combo box.
	''' </summary>
	Private Sub OnFilename_TextChanged(sender As System.Object, e As System.EventArgs) Handles cboFilename.TextChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If String.IsNullOrEmpty(cboFilename.Text) Then
			Me.cboFilename.Properties.Buttons(1).Enabled = False
		End If
	End Sub

	''' <summary>
	''' Handles mouse down on gvContactData.
	''' </summary>
	Private Sub OnGvContactData_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles gvContactData.MouseDown
		If e.Button = System.Windows.Forms.MouseButtons.Right Then
			Dim hi As GridHitInfo = gvContactData.CalcHitInfo(e.Location)
			Dim value As Object = "null"
			If (hi.InRowCell And hi.Column.Name = "docType") Then

				Dim row As ContactViewData = gvContactData.GetRow(hi.RowHandle)

				If Not row Is Nothing AndAlso row.DocumentId.HasValue AndAlso Not row.KDKontactRecID.HasValue Then

					value = gvContactData.GetRowCellValue(hi.RowHandle, hi.Column)

					If (m_DeletePDFPopup Is Nothing) Then
						m_DeletePDFPopup = New PopupMenu(BarManager1)
					End If

					m_DeletePDFPopup.ItemLinks.Clear()
					Dim deletePDFItem As BarButtonItem = New BarButtonItem(BarManager1, "Dokument löschen")
					deletePDFItem.Tag = row.DocumentId
					AddHandler deletePDFItem.ItemClick, AddressOf OnDeletePDFItem_Click
					m_DeletePDFPopup.AddItem(deletePDFItem)
					m_DeletePDFPopup.ShowPopup(Cursor.Position)

				End If

			End If
		End If

	End Sub

	''' <summary>
	''' Handles click on delete pdf item (Popup).
	''' </summary>
	Private Sub OnDeletePDFItem_Click(ByVal sender As Object, ByVal e As ItemClickEventArgs)

		Dim barItem As BarButtonItem = e.Item

		Dim documentID As Integer = barItem.Tag

		Dim success = m_EmployeeDatabaseAccess.DeleteContactDocument(documentID)

		If success Then
			FilterContactData(m_EmployeeNumber)

			If m_CurrentContactRecordNumber.HasValue Then
				FocusContact(m_EmployeeNumber, m_CurrentContactRecordNumber)
			End If

			Me.cboFilename.Properties.Buttons(1).Enabled = False
			m_CurrentDocumentID = Nothing
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))
		End If


	End Sub

	''' <summary>
	''' opens TODO form
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
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

	''' <summary>
	''' Handles click on customer name lookup edit.
	''' </summary>
	Private Sub OnLueCustomerName_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueCustomerName.ButtonClick

		If (e.Button.Index = 2) Then

			If Not m_DependentCustomerContactData Is Nothing Then
				OpenCustomerContact(m_DependentCustomerContactData.KDnr, m_DependentCustomerContactData.KDKontaktRecNr)
			End If

		ElseIf (e.Button.Index = 3) Then

			If Not lueCustomerName.EditValue Is Nothing Then
				OpenCustomer(lueCustomerName.EditValue)
			End If
		End If

	End Sub

	''' <summary>
	''' Handles edit value change of customer lookup.
	''' </summary>
	Private Sub OnLueCustomerName_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomerName.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim success = LoadVacancyProposeAndEsDataBasedOnSelectedCustomer()

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanz, Vorschlag oder Einsatz konnte nicht gefiltert werden."))
		End If

	End Sub

	''' <summary>
	''' Handles customer contact change notification messages.
	''' </summary>
	''' <param name="msg">The customer contact notification message.</param>
	Private Sub HandleCustomerDataHasChangedMsg(ByVal msg As CustomerContactDataHasChanged)

		If Not m_EmployeeContactData Is Nothing AndAlso
			m_InitializationData.MDData.MDNr = msg.MDNr AndAlso
			m_EmployeeContactData.CustomerContactKDNr = msg.CustomerNumber AndAlso
			m_EmployeeContactData.CustomerContactRecNr = msg.ContactRecNr Then

			' The partent contact data has changed. This means this contact data has also changed -> reload.
			LoadContactDetailData(m_EmployeeNumber, m_CurrentContactRecordNumber)

		End If

	End Sub

	''' <summary>
	''' Loads vacany, propose and ES data based on the selected customer.
	''' </summary>
	Private Function LoadVacancyProposeAndEsDataBasedOnSelectedCustomer() As Boolean

		Dim success As Boolean = True

		lueVacancy.EditValue = Nothing
		luePropose.EditValue = Nothing
		lueES.EditValue = Nothing

		success = success AndAlso LoadVacancyDropDownData(lueCustomerName.EditValue)
		success = success AndAlso LoadProposeDropDownData(m_EmployeeNumber, lueCustomerName.EditValue)
		success = success AndAlso LoadESDropDownData(m_EmployeeNumber, lueCustomerName.EditValue)

		Return success

	End Function

	''' <summary>
	''' Validates document input data.
	''' </summary>
	Private Function ValidateContactInputData() As Boolean

		' Clear errors
		errorProviderContactManagement.Clear()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(dateEditFrom, errorProviderContactManagement, dateEditFrom.EditValue Is Nothing, errorText)
		isValid = isValid And SetErrorIfInvalid(lueContactType, errorProviderContactManagement, lueContactType.EditValue Is Nothing, errorText)

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
	''' Focuses a document.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="contactRecordNumber">The contact record number</param>
	Private Sub FocusContact(ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)

		If Not gridContactData.DataSource Is Nothing Then

			Dim contactViewData = CType(gvContactData.DataSource, BindingList(Of ContactViewData))

			Dim index = contactViewData.ToList().FindIndex(Function(data) data.EmployeeNumber = employeeNumber And data.ContactRecordNumber = contactRecordNumber)

			m_SuppressUIEvents = True
			Dim rowHandle = gvContactData.GetRowHandle(index)
			gvContactData.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = False
		End If

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
	''' Enables or disables controls in contents panel. 
	''' </summary>
	Private Sub EnableOrDisableContentsPanelControls(ByVal areControlsEnabled As Boolean)

		For Each ctrl As Control In pnlContents.Controls
			ctrl.Enabled = areControlsEnabled
		Next

	End Sub

#End Region

#Region "View helper classes"

	''' <summary>
	'''  Contact view data.
	''' </summary>
	Class ContactViewData
		Public Property EmployeeNumber As Integer
		Public Property ContactRecordNumber As Integer?
		Public Property ContactDate As DateTime?
		Public Property Person_Subject As String
		Public Property Description As String
		Public Property Important As Boolean?
		Public Property Completed As Boolean?
		Public Property KST As String
		Public Property PDFImage As Image
		Public Property DocumentId As Integer?
		Public Property KDKontactRecID As Integer?

	End Class


	''' <summary>
	''' Responsible person view data.
	''' </summary>
	Class ResponsiblePersonViewData

		Public Property Name As String
		Public Property ResponsiblePersonRecordNumber As Integer?

	End Class

	''' <summary>
	''' Employee view data.
	''' </summary>
	Class EmployeeViewData

		Public Property EmployeeNumber As Integer
		Public Property Name As String
		Public Property Address As String

	End Class

	''' <summary>
	''' ContactType1 view data.
	''' </summary>
	Class ContactType1ViewData

		Public Property Caption As String
		Public Property Bez_ID As String

	End Class

#End Region

End Class

''' <summary>
''' Contat filter settings.
''' </summary>
Public Class ContactFilterSettings

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

	Public Property ExcluePhone As Boolean
	Public Property ExclueMail As Boolean
	Public Property ExclueOffered As Boolean
	Public Property ExcludeSMS As Boolean

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

''' <summary>
''' Inital data for new contact.
''' </summary>
Public Class InitalDataForNewContact

	Public Property StartDateTime As DateTime?
	Public Property ContactTypeBezID As String

	Public Property title As String	' Bezeichnung
	Public Property description As String	' Beschreibung
	Public Property customerNumber As Integer?
	Public Property customerVacancyNumber As Integer?
	Public Property customerProposeNumber As Integer?
	Public Property customerESNumber As Integer?

End Class