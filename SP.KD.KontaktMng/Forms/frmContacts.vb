
Imports System.Reflection.Assembly

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports SP.KD.KontaktMng.Settings
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.ucListSelectPopup
Imports System.IO
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraBars
Imports SP.DatabaseAccess.Common
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Employee

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.TodoMng
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


''' <summary>
''' Contact management.
''' </summary>
Public Class frmContacts

	Public Delegate Sub ContactDataSavedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)
	Public Delegate Sub ContactDataDeletedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)

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
	Private m_CommonDataAccess As ICommonDatabaseAccess

	''' <summary>
	''' The customer data access object.
	''' </summary>
	Private m_CustomerDataAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' Used to copy contacts for employees (Kandidaten)
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
	''' Contains the customer number of the loaded customer.
	''' </summary>
	Private m_CustomerNumber As Integer

	''' <summary>
	''' Contains the responsible person record number.
	''' </summary>
	Private m_ResponsiblePersonRecordNumber As Integer?

	''' <summary>
	''' Record number of selected contact.
	''' </summary>
	Private m_CurrentContactRecordNumber As Integer?

	''' <summary>
	''' Responsible person assigned contact data.
	''' </summary>
	Private m_ResponsiblePersonContactData As ResponsiblePersonAssignedContactData

	''' <summary>
	''' Customer dependent employee contact data
	''' </summary>
	Private m_CustomerDependentEmployeeContactData As IEnumerable(Of CustomerDependentEmployeeContactData)

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

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False
	Private m_AllowedDelete As Boolean

	''' <summary>
	''' Employee popup column definitions.
	''' </summary>
	Private m_EmployeePopupColumns As New List(Of PopupColumDefintion)

	''' <summary>
	''' Popup to delete a pdf document.
	''' </summary>
	Private m_DeletePDFPopup As PopupMenu

	''' <summary>
	''' EmployeeNumbers to copy (user for contact  copy function).
	''' </summary>
	Private m_EmployeeNumbersToCopy As Integer()

	''' <summary>
	''' PDF image.
	''' </summary>
	Private m_ContactImage As Image

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

		m_CommonDataAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDataAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage) ' Used to copy contacts for employees (Kandidaten)
		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		' Column defintions for popups
		m_EmployeePopupColumns.Add(New PopupColumDefintion With {.Name = "Name", .Translation = "Name", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})
		m_EmployeePopupColumns.Add(New PopupColumDefintion With {.Name = "Address", .Translation = "Adresse", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})

		m_ContactImage = My.Resources.contat

		AddHandler gvContactData.RowCellClick, AddressOf OnGvContactData_RowCellClick

		AddHandler dateEditFrom.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueZHDName.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueContactType.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueVacancy.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePropose.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueES.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler chkTelephone.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
		AddHandler chkOffered.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
		AddHandler chkMailed.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
		AddHandler chkSMS.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged


		Dim communicationHub = MessageService.Instance.Hub
		communicationHub.Subscribe(Of EmployeeContactDataHasChanged)(AddressOf HandleEmployeeDataHasChangedMsg)

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
	''' Gets the selected customer dependent employee contact.
	''' </summary>
	''' <returns>The selected dependent  customer employee contact none is selected.</returns>
	Public ReadOnly Property SelectedCustomerDependentEmployeeContactData As CustomerDependentEmployeeContactData
		Get
			Dim grdView = TryCast(Me.gridDependentEmployeeContacts.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim depententContact = CType(grdView.GetRow(selectedRows(0)), CustomerDependentEmployeeContactData)
					Return depententContact
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
	''' Activates new contact data model.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responiblePersonNumber">The responsible person number (optional).</param>
	''' <param name="initalDataForNewContact">The inital data for new contact.</param>
	''' <param name="contactFilterSettings">The contact filter settings (optional).</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function ActivateNewContactDataMode(ByVal customerNumber As Integer, ByVal responiblePersonNumber? As Integer,
												 ByVal initalDataForNewContact As InitalDataForNewContact, ByVal contactFilterSettings As ContactFilterSettings) As Boolean

		Dim success As Boolean = PrepareForm(customerNumber, responiblePersonNumber, contactFilterSettings)

		PrepareForNew(initalDataForNewContact)

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))
		End If

		Return success
	End Function

	''' <summary>
	''' Loads the contact data of a customer or a responsible person.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responiblePersonNumber">The responsible person number (optional).</param>
	''' <param name="contactRecordNumber">The contact record number.</param>
	''' <param name="contactFilterSettings">The contact filter settings (optional).</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function LoadContactData(ByVal customerNumber As Integer, ByVal responiblePersonNumber? As Integer, ByVal contactRecordNumber As Integer, ByVal contactFilterSettings As ContactFilterSettings) As Boolean

		Dim success As Boolean = PrepareForm(customerNumber, responiblePersonNumber, contactFilterSettings)

		If contactRecordNumber Then
			success = success AndAlso LoadContactDetailData(customerNumber, responiblePersonNumber, contactRecordNumber)
			FocusContact(customerNumber, contactRecordNumber)
		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))
		End If

		Return success
	End Function

	''' <summary>
	''' Loads vacancy drop down data.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	Public Function LoadVacancyDropDownData(ByVal customerNumber As Integer) As Boolean
		Dim vacancyData = m_CustomerDataAccess.LoadVacancyData(customerNumber)

		lueVacancy.Properties.DataSource = vacancyData
		lueVacancy.Properties.Buttons(0).Enabled = vacancyData.Count > 0

		Return Not vacancyData Is Nothing
	End Function

	''' <summary>
	''' Loads propose drop down data.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	Public Function LoadProposeDropDownData(ByVal customerNumber As Integer) As Boolean
		Dim proposeData = m_CustomerDataAccess.LoadProposeData(customerNumber)

		luePropose.Properties.DataSource = proposeData
		luePropose.Properties.Buttons(0).Enabled = proposeData.Count > 0

		Return Not proposeData Is Nothing
	End Function

	''' <summary>
	''' Loads ES drop down data.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	Public Function LoadESDropDownData(ByVal customerNumber As Integer) As Boolean
		Dim esData = m_CustomerDataAccess.LoadESData(customerNumber)

		lueES.Properties.DataSource = esData
		lueES.Properties.Buttons(0).Enabled = esData.Count > 0

		Return Not esData Is Nothing
	End Function


#End Region


#Region "Private Methods"


	''' <summary>
	'''  trannslate controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
		Me.btnNewContact.Text = m_Translate.GetSafeTranslationValue(Me.btnNewContact.Text)
		Me.btnDeleteContact.Text = m_Translate.GetSafeTranslationValue(Me.btnDeleteContact.Text)
		Me.btnCopyContactForEmployees.Text = m_Translate.GetSafeTranslationValue(Me.btnCopyContactForEmployees.Text)
		Me.btnCreateTODO.Text = m_Translate.GetSafeTranslationValue(Me.btnCreateTODO.Text)

		Me.grpDetail.Text = m_Translate.GetSafeTranslationValue(Me.grpDetail.Text)
		Me.chkImportant.Text = m_Translate.GetSafeTranslationValue(Me.chkImportant.Text)
		Me.chkFinished.Text = m_Translate.GetSafeTranslationValue(Me.chkFinished.Text)

		Me.lbldatum.Text = m_Translate.GetSafeTranslationValue(Me.lbldatum.Text)
		Me.lblzhd.Text = m_Translate.GetSafeTranslationValue(Me.lblzhd.Text)
		Me.lblkategorie.Text = m_Translate.GetSafeTranslationValue(Me.lblkategorie.Text)
		Me.lblbezeichnung.Text = m_Translate.GetSafeTranslationValue(Me.lblbezeichnung.Text)
		Me.lblbeschreibung.Text = m_Translate.GetSafeTranslationValue(Me.lblbeschreibung.Text)
		Me.lbldatei.Text = m_Translate.GetSafeTranslationValue(Me.lbldatei.Text)
		Me.lblvakanz.Text = m_Translate.GetSafeTranslationValue(Me.lblvakanz.Text)
		Me.lblvorschlag.Text = m_Translate.GetSafeTranslationValue(Me.lblvorschlag.Text)
		Me.lbleinsatz.Text = m_Translate.GetSafeTranslationValue(Me.lbleinsatz.Text)
		Me.lblKopienKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblKopienKandidat.Text)
		Me.lblKopiertVonKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblKopiertVonKandidat.Text)
		Me.lblerstellt.Text = m_Translate.GetSafeTranslationValue(Me.lblerstellt.Text)
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
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responiblePersonNumber">The responsible person number.</param>
	''' <param name="contactFilterSettings">The conact filter settings.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function PrepareForm(ByVal customerNumber As Integer, ByVal responiblePersonNumber? As Integer, ByVal contactFilterSettings As ContactFilterSettings) As Boolean

		Dim success As Boolean = True

		If Not m_IsInitialDataLoaded OrElse
			Not m_CustomerNumber = customerNumber Then
			success = success AndAlso LoadCustomerNameData(customerNumber, responiblePersonNumber)
			success = success AndAlso LoadDropDown(customerNumber)
			m_IsInitialDataLoaded = True
		End If

		' Reset the form
		Reset()

		m_CustomerNumber = customerNumber
		m_ResponsiblePersonRecordNumber = responiblePersonNumber

		' If the responsible person number is provided then we view the records of a single person -> the responsible person can not be changed.
		If responiblePersonNumber Then
			lueZHDName.Enabled = False
		End If

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

		success = success AndAlso LoadYearsListData(customerNumber, responiblePersonNumber, initalYearFilter)
		success = success AndAlso FilterContactData(customerNumber, responiblePersonNumber)

		Return success

	End Function

	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	Private Function LoadDropDown(ByVal customerNumber As Integer) As Boolean

		Dim success = True
		success = success AndAlso LoadResponsiblePersonsDropDownData(customerNumber)
		success = success AndAlso LoadContactType1DropDownData()
		success = success AndAlso LoadVacancyDropDownData(customerNumber)
		success = success AndAlso LoadProposeDropDownData(customerNumber)
		success = success AndAlso LoadESDropDownData(customerNumber)

		Return success
	End Function

	''' <summary>
	''' Load responsible person drop down data.
	''' </summary>
	Private Function LoadResponsiblePersonsDropDownData(ByVal customerNumber As Integer) As Boolean
		Dim responsiblePersonData = m_CustomerDataAccess.LoadResponsiblePersonData(customerNumber)

		Dim responsiblePersonViewData = Nothing

		If Not responsiblePersonData Is Nothing Then

			responsiblePersonViewData = New List(Of ResponsiblePersonViewData)

			For Each person In responsiblePersonData
				responsiblePersonViewData.Add(New ResponsiblePersonViewData With {
																																					.Name = If(String.IsNullOrEmpty(person.Firstname), person.TranslatedSalutation & " " & person.Lastname,
																																																									person.TranslatedSalutation & " " & person.Firstname & " " & person.Lastname).Trim(),
																																					.ResponsiblePersonRecordNumber = person.RecordNumber,
																																					.ZState1 = person.ZState1,
																																					.ZState2 = person.ZState2
																																					 })
			Next

		End If

		lueZHDName.Properties.DataSource = responsiblePersonViewData


		Return Not responsiblePersonViewData Is Nothing
	End Function

	''' <summary>
	''' Load contact type1 drop down data.
	''' </summary>
	Private Function LoadContactType1DropDownData() As Boolean

		Dim contactType1Data = m_CommonDataAccess.LoadContactTypeData1()

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
		lueContactType.Properties.DropDownRows = Math.Min(contactType1Data.Count, 20)

		lueContactType.Properties.ForceInitialize()

		Return Not contactType1ViewData Is Nothing

	End Function


	''' <summary>
	''' Resets the from.
	''' </summary>
	Private Sub Reset()

		m_CustomerNumber = 0
		m_ResponsiblePersonRecordNumber = Nothing
		m_CurrentContactRecordNumber = Nothing
		m_CurrentFileBytes = Nothing
		m_CurrentFileExtension = String.Empty

		m_CurrentDocumentID = Nothing
		m_ResponsiblePersonContactData = Nothing
		m_CustomerDependentEmployeeContactData = Nothing

		lueZHDName.EditValue = Nothing
		lueContactType.EditValue = Nothing
		txtTitle.Text = String.Empty
		txtTitle.Properties.MaxLength = 1000

		txtDescription.Text = String.Empty
		txtDescription.Properties.MaxLength = 20000

		chkImportant.Checked = False
		chkFinished.Checked = False

		cboFilename.Text = String.Empty
		Me.cboFilename.Properties.Buttons(1).Enabled = False

		SwitchUICompentsForCopiedRecord(False)

		' ---Reset drop downs, grids and lists---

		ResetResponsiblePersonDropDown()
		ResetContactType1DropDown()
		ResetVacancyDropDown()
		ResetProposeDropDown()
		ResetESDropDown()

		ResetContactGrid()
		ResetDependentEmployeeContactGrid()

		TranslateControls()
		EnableOrDisableContentsPanelControls(True)
		btnSave.Enabled = True
		btnNewContact.Enabled = True

		m_AllowedDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 229, m_InitializationData.MDData.MDNr)
		btnDeleteContact.Enabled = m_AllowedDelete

		' Clear errors
		errorProviderContactManagement.Clear()
	End Sub

	''' <summary>
	''' Resets the responsible person drop down.
	''' </summary>
	Private Sub ResetResponsiblePersonDropDown()

		lueZHDName.Properties.DisplayMember = "Name"
		lueZHDName.Properties.ValueMember = "ResponsiblePersonRecordNumber"

		gvZHDName.OptionsView.ShowIndicator = False
		gvZHDName.OptionsView.ShowColumnHeaders = True
		gvZHDName.OptionsView.ShowFooter = False
		gvZHDName.OptionsView.ShowAutoFilterRow = True
		gvZHDName.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvZHDName.Columns.Clear()

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "ResponsiblePersonRecordNumber"
		columnRecordNumber.FieldName = "ResponsiblePersonRecordNumber"
		columnRecordNumber.Visible = False
		gvZHDName.Columns.Add(columnRecordNumber)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnName.Name = "Name"
		columnName.FieldName = "Name"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnName)

		Dim columnfstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfstate.Caption = m_Translate.GetSafeTranslationValue("KD1Status", True)
		columnfstate.Name = "fstate"
		columnfstate.FieldName = "fstate"
		columnfstate.Visible = False
		columnfstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnfstate)

		Dim columnsstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsstate.Caption = m_Translate.GetSafeTranslationValue("KD2Status", True)
		columnsstate.Name = "sstate"
		columnsstate.FieldName = "sstate"
		columnsstate.Visible = False
		columnsstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnsstate)

		Dim columnPosition As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPosition.Caption = m_Translate.GetSafeTranslationValue("Position")
		columnPosition.Name = "Position"
		columnPosition.FieldName = "Position"
		columnPosition.Visible = False
		columnPosition.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnPosition)

		Dim columnDepartment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDepartment.Caption = m_Translate.GetSafeTranslationValue("Abteilung")
		columnDepartment.Name = "Department"
		columnDepartment.FieldName = "Department"
		columnDepartment.Visible = False
		columnDepartment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnDepartment)

		Dim columnContact As New DevExpress.XtraGrid.Columns.GridColumn()
		columnContact.Caption = m_Translate.GetSafeTranslationValue("KDKontakt")
		columnContact.Name = "howcontact"
		columnContact.FieldName = "howcontact"
		columnContact.Visible = False
		columnContact.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHDName.Columns.Add(columnContact)


		lueZHDName.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueZHDName.Properties.NullText = String.Empty
		lueZHDName.EditValue = Nothing

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

		Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullname.Name = "EmployeeFullname"
		columnEmployeeFullname.FieldName = "EmployeeFullname"
		columnEmployeeFullname.Visible = True
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


	' ''' <summary>
	' ''' Resets the propose drop down.
	' ''' </summary>
	'Private Sub ResetProposeDropDown()
	'  luePropose.Properties.DisplayMember = "Description"
	'  luePropose.Properties.ValueMember = "ProposeNumber"

	'  Dim columns = luePropose.Properties.Columns
	'  columns.Clear()
	'columns.Add(New LookUpColumnInfo("EmployeeFullname", 0))
	'columns.Add(New LookUpColumnInfo("Description", 0))

	'columns.Add(New LookUpColumnInfo("P_State", 0))
	'columns.Add(New LookUpColumnInfo("CreatedOn", 0))
	'columns.Add(New LookUpColumnInfo("CreatedFrom", 0))

	'  luePropose.Properties.ShowHeader = False
	'  luePropose.Properties.ShowFooter = False

	'  luePropose.Properties.BestFitMode = BestFitMode.BestFitResizePopup
	'  luePropose.Properties.SearchMode = SearchMode.AutoComplete
	'  luePropose.Properties.AutoSearchColumnIndex = 0

	'  luePropose.Properties.NullText = String.Empty
	'  luePropose.EditValue = Nothing
	'End Sub

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

		Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullname.Name = "EmployeeFullname"
		columnEmployeeFullname.FieldName = "EmployeeFullname"
		columnEmployeeFullname.Visible = True
		columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvES.Columns.Add(columnEmployeeFullname)

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

	'Private Sub ResetESDropDown()
	'	lueES.Properties.DisplayMember = "ES_As"
	'	lueES.Properties.ValueMember = "ESNumber"

	'	Dim columns = lueES.Properties.Columns
	'	columns.Clear()
	'	columns.Add(New LookUpColumnInfo("EmployeeFullname", 0))
	'	columns.Add(New LookUpColumnInfo("ES_As", 0))

	'	Dim fromDateColumn = New LookUpColumnInfo("ES_FromDate", 0)
	'	fromDateColumn.FormatString = "d"
	'	fromDateColumn.FormatType = DevExpress.Utils.FormatType.Custom

	'	columns.Add(fromDateColumn)

	'	Dim toDateColumn = New LookUpColumnInfo("ES_ToDate", 0)
	'	toDateColumn.FormatString = "d"
	'	toDateColumn.FormatType = DevExpress.Utils.FormatType.Custom

	'	columns.Add(toDateColumn)

	'	lueES.Properties.ShowHeader = False
	'	lueES.Properties.ShowFooter = False

	'	lueES.Properties.BestFitMode = BestFitMode.BestFitResizePopup
	'	lueES.Properties.SearchMode = SearchMode.AutoComplete
	'	lueES.Properties.AutoSearchColumnIndex = 0

	'	lueES.Properties.NullText = String.Empty
	'	lueES.EditValue = Nothing
	'End Sub
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
	''' Resets the dependent employee contact grid()
	''' </summary>
	Private Sub ResetDependentEmployeeContactGrid()
		' Reset the grid
		gvDependentEmployeeContacts.Columns.Clear()
		gvDependentEmployeeContacts.OptionsView.ShowIndicator = False

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnName.Name = "Name"
		columnName.FieldName = "Name"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvDependentEmployeeContacts.Columns.Add(columnName)

		Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAddress.Caption = m_Translate.GetSafeTranslationValue("Addresse")
		columnAddress.Name = "Address"
		columnAddress.FieldName = "Address"
		columnAddress.Visible = True
		columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvDependentEmployeeContacts.Columns.Add(columnAddress)

		Dim contactSymbol As New DevExpress.XtraGrid.Columns.GridColumn()
		contactSymbol.Caption = " "
		contactSymbol.Name = "dependentEmployeeContactSymbol"
		contactSymbol.FieldName = "dependentEmployeeContactSymbol"
		contactSymbol.Visible = True
		contactSymbol.ColumnEdit = New RepositoryItemPictureEdit()
		contactSymbol.UnboundType = DevExpress.Data.UnboundColumnType.Object
		contactSymbol.Width = 20
		gvDependentEmployeeContacts.Columns.Add(contactSymbol)

	End Sub

	''' <summary>
	''' Loads the customer name data.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responsiblePersonNumber">The responsible person number.</param>
	''' <returns>Boolean flag indicating success</returns>
	Private Function LoadCustomerNameData(ByVal customerNumber As Integer, ByVal responsiblePersonNumber As Integer?) As Boolean

		Dim customer = m_CustomerDataAccess.LoadCustomerMasterData(customerNumber, m_InitializationData.UserData.UserFiliale)

		If (customer Is Nothing) Then
			Return False
		End If

		Dim strformCaption As String = m_Translate.GetSafeTranslationValue(Me.Text)
		If responsiblePersonNumber.GetValueOrDefault(0) > 0 Then
			Dim responsiblePerson = m_CustomerDataAccess.LoadResponsiblePersonMasterData(customerNumber, responsiblePersonNumber)
			If responsiblePerson Is Nothing Then
				Return False
			End If

			If String.IsNullOrEmpty(responsiblePerson.Firstname) Then
				Text = String.Format("{0}: [{1}, {2} {3}]", strformCaption, customer.Company1, responsiblePerson.TranslatedSalutation, responsiblePerson.Lastname)

			Else
				Text = String.Format("{0}: [{1}, {2} {3} {4}]", strformCaption, customer.Company1, responsiblePerson.TranslatedSalutation, responsiblePerson.Firstname, responsiblePerson.Lastname)
			End If

		Else
			Text = String.Format("{0}: [{1}]", strformCaption, customer.Company1)
		End If

		Return True
	End Function

	''' <summary>
	''' Loads the years drop down data.
	''' </summary>
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responsiblePersonNumber">The responsible person number.</param>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadYearsListData(ByVal customerNumber As Integer, ByVal responsiblePersonNumber As Integer?, Optional ByVal yearsToSelect As Integer() = Nothing) As Boolean
		Dim yearsData = m_CustomerDataAccess.LoadCustomerContactTotalDistinctYears(customerNumber, responsiblePersonNumber)

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
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responiblePersonNumber">The responsible person number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function FilterContactData(ByVal customerNumber As Integer, ByVal responiblePersonNumber As Integer?) As Boolean

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
		Dim contactData = m_CustomerDataAccess.LoadCustomerContactOverviewlDataBySearchCriteria(customerNumber, responiblePersonNumber, Not chkTelephone.Checked, Not chkOffered.Checked, Not chkMailed.Checked, Not chkSMS.Checked, yearsArray)

		If (contactData Is Nothing) Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))

			Return False
		End If

		Dim listDataSource As BindingList(Of ContactViewData) = New BindingList(Of ContactViewData)

		' Convert the data to view data.
		For Each p In contactData

			Dim cViewData = New ContactViewData() With {
			  .CustomerNumber = p.CustomerNumber,
			  .ResponsiblePersonNumber = p.ResponsiblePersonRecordNumber,
			  .ContactRecordNumber = p.RecNr,
			  .ContactDate = p.ContactDate,
			  .Person_Subject = p.PersonOrSubject,
			  .Description = p.Description,
			  .Important = p.IsImportant,
			  .Completed = p.IsCompleted,
			  .KST = p.Creator}

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
			LoadContactDetailData(selectedContact.CustomerNumber, selectedContact.ResponsiblePersonNumber, selectedContact.ContactRecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles double click on contcat grid.
	''' </summary>
	Private Sub OnContact_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridContactData.DoubleClick

		Dim selectedContact = SelectedContactViewData

		If Not selectedContact Is Nothing Then
			LoadContactDetailData(selectedContact.CustomerNumber, selectedContact.ResponsiblePersonNumber, selectedContact.ContactRecordNumber)
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
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="responsiblePersonNumber">The responsible person number.</param>
	''' <param name="contactNumber">The contact number.</param>
	Private Function LoadContactDetailData(ByVal customerNumber As Integer, ByVal responsiblePersonNumber As Integer?, ByVal contactNumber As Integer) As Boolean

		Dim success As Boolean = True

		' Clear errors
		errorProviderContactManagement.Clear()

		Dim contactResult = m_CustomerDataAccess.LoadAssignedContactDataOfResponsiblePerson(customerNumber, responsiblePersonNumber, contactNumber)
		m_ResponsiblePersonContactData = contactResult

		If Not contactResult Is Nothing Then

			dateEditFrom.EditValue = contactResult.ContactDate
			timeStart.EditValue = contactResult.ContactDate

			lueZHDName.EditValue = contactResult.ResponsiblePersonNumber
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

			lueVacancy.EditValue = contactResult.VacancyNumber
			luePropose.EditValue = contactResult.ProposeNr
			lueES.EditValue = contactResult.ESNr

			success = success AndAlso LoadCustomerDependentEmployeeContactData(contactResult.ID)

			m_CurrentFileBytes = Nothing
			m_CurrentFileExtension = String.Empty

			lblContactCreated.Text = String.Format("{0:f}, {1}", contactResult.CreatedOn, contactResult.CreatedFrom)
			lblContactChanged.Text = String.Format("{0:f}, {1}", contactResult.ChangedOn, contactResult.ChangedFrom)

			m_CurrentContactRecordNumber = contactResult.RecordNumber

			success = success AndAlso PrepareUIForCopiedRecords(contactResult)

		Else
			success = False
		End If

		Return success
	End Function


	''' <summary>
	''' Prepares the UI for copied records.
	''' </summary>
	''' <param name="contactResult">The contact result.</param>
	''' <returns>Boolean flag indicating success.</returns>>
	Private Function PrepareUIForCopiedRecords(ByVal contactResult As ResponsiblePersonAssignedContactData) As Boolean

		Dim success = True

		Dim isCopiedRecord = (contactResult.EmployeeContactRecID.HasValue)

		EnableOrDisableContentsPanelControls(Not isCopiedRecord)
		btnSave.Enabled = Not isCopiedRecord
		btnDeleteContact.Enabled = Not isCopiedRecord AndAlso m_AllowedDelete
		lblWarnOnCopy.Visible = isCopiedRecord

		If isCopiedRecord Then
			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(contactResult.EmployeeContactEmployeeNr, False)

			If Not employeeMasterData Is Nothing Then
				lblKopiertVonKandidatValue.Text = String.Format("{0} {1} ({2}), {3}-{4} {5}", employeeMasterData.Lastname, employeeMasterData.Firstname,
																	  employeeMasterData.EmployeeNumber, employeeMasterData.Country, employeeMasterData.Postcode, employeeMasterData.Street)

			End If

			success = (Not employeeMasterData Is Nothing)
		End If

		SwitchUICompentsForCopiedRecord(isCopiedRecord)

		Return success
	End Function

	''' <summary>
	''' Loads customer dependent employee contact data.
	''' </summary>
	''' <param name="kdRecID">The customer record id.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadCustomerDependentEmployeeContactData(ByVal kdRecID As Integer) As Boolean

		m_CustomerDependentEmployeeContactData = m_CustomerDataAccess.LoadCustomerDependentEmployeeContactData(kdRecID)

		gridDependentEmployeeContacts.DataSource = m_CustomerDependentEmployeeContactData
		m_EmployeeNumbersToCopy = m_CustomerDependentEmployeeContactData.Select(Function(data) data.EmployeeNumber).ToArray()

		Return Not m_CustomerDependentEmployeeContactData Is Nothing
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
		m_EmployeeNumbersToCopy = Nothing
		m_ResponsiblePersonContactData = Nothing
		m_CustomerDependentEmployeeContactData = Nothing

		dateEditFrom.EditValue = Nothing
		timeStart.EditValue = Nothing
		lueZHDName.EditValue = If(m_ResponsiblePersonRecordNumber.HasValue, m_ResponsiblePersonRecordNumber, Nothing)
		lueContactType.EditValue = Nothing
		txtTitle.Text = String.Empty
		txtDescription.Text = String.Empty
		chkImportant.Checked = False
		chkFinished.Checked = False
		cboFilename.Text = String.Empty
		lueVacancy.EditValue = Nothing
		luePropose.EditValue = Nothing
		lueES.EditValue = Nothing
		gridDependentEmployeeContacts.DataSource = Nothing

		Me.cboFilename.Properties.Buttons(1).Enabled = False

		EnableOrDisableContentsPanelControls(True)
		btnSave.Enabled = True
		btnNewContact.Enabled = True
		btnDeleteContact.Enabled = m_AllowedDelete
		lblWarnOnCopy.Visible = False

		lblContactCreated.Text = "-"
		lblContactChanged.Text = "-"

		SwitchUICompentsForCopiedRecord(False)

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

			' Check if the contact should be copied for a number of employees.
			If Not initalDataForNewContact.EmployeeCopyList Is Nothing Then

				m_EmployeeNumbersToCopy = initalDataForNewContact.EmployeeCopyList.ToArray()

				' The data must be saved in order to create a copy for the employees.
				SaveData()

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
		SaveData()
	End Sub

	''' <summary>
	''' Saves the data.
	''' </summary>
	Private Sub SaveData()

		If ValidateContactInputData() Then

			Dim contactData As ResponsiblePersonAssignedContactData = Nothing

			Dim dt = DateTime.Now
			If Not m_CurrentContactRecordNumber.HasValue Then
				contactData = New ResponsiblePersonAssignedContactData With {.CustomerNumber = m_CustomerNumber,
																		   .ResponsiblePersonNumber = m_ResponsiblePersonRecordNumber,
																		   .CreatedOn = dt,
																		   .CreatedFrom = m_ClsProgSetting.GetUserName()}
			Else

				contactData = m_CustomerDataAccess.LoadAssignedContactDataOfResponsiblePerson(m_CustomerNumber, m_ResponsiblePersonRecordNumber, m_CurrentContactRecordNumber)

				If contactData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

			End If

			contactData.CustomerNumber = m_CustomerNumber
			contactData.ContactDate = CombineDateAndTime(dateEditFrom.EditValue, timeStart.EditValue)
			contactData.ResponsiblePersonNumber = If(lueZHDName.EditValue Is Nothing, 0, lueZHDName.EditValue)
			contactData.ContactType1 = If(lueContactType.EditValue Is Nothing, 0, lueContactType.EditValue)
			contactData.ContactPeriodString = txtTitle.Text
			contactData.ContactsString = txtDescription.Text
			contactData.ContactImportant = chkImportant.Checked
			contactData.ContactFinished = chkFinished.Checked
			contactData.MANr = 0
			contactData.VacancyNumber = lueVacancy.EditValue
			contactData.ProposeNr = luePropose.EditValue
			contactData.ESNr = lueES.EditValue

			contactData.ChangedFrom = m_ClsProgSetting.GetUserName()
			contactData.ChangedOn = dt
			contactData.UsNr = m_InitializationData.UserData.UserNr

			Dim isNewContact = (contactData.ID = 0)

			Dim success As Boolean = True

			' Check if the document bytes must be saved.
			If Not (m_CurrentFileBytes Is Nothing) And success Then

				Dim contactDocument As ContactDoc = Nothing

				If Not m_CurrentDocumentID.HasValue Then

					contactDocument = New ContactDoc() With {.CreatedOn = dt,
																										 .CreatedFrom = m_ClsProgSetting.GetUserName(),
																										 .FileBytes = m_CurrentFileBytes,
																										 .FileExtension = m_CurrentFileExtension}
					success = success AndAlso m_CustomerDataAccess.AddContactDocument(contactDocument)

					If success Then
						m_CurrentDocumentID = contactDocument.ID
						contactData.KontaktDocID = m_CurrentDocumentID
					End If

				Else
					contactDocument = m_CustomerDataAccess.LoadContactDocumentData(m_CurrentDocumentID.Value, False)

					If Not contactDocument Is Nothing Then

						contactDocument.FileBytes = m_CurrentFileBytes
						contactDocument.FileExtension = m_CurrentFileExtension
						success = success AndAlso m_CustomerDataAccess.UpdateContactDocumentData(contactDocument, False)
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
			If isNewContact Then
				contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_CustomerDataAccess.AddResponsiblePersonContactAssignment(contactData)

				If success Then
					m_CurrentContactRecordNumber = contactData.RecordNumber
				End If

			Else
				contactData.ChangedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_CustomerDataAccess.UpdateResponsiblePersonAssignedContactData(contactData)
			End If


			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else

				' ---Copy contact---

				If Not m_EmployeeNumbersToCopy Is Nothing Then
					' The user has selected a list of employees -> copy the contact for all of them.
					success = success AndAlso CopyContactForEmployees(contactData, m_CurrentDocumentID, m_EmployeeNumbersToCopy)

					' Clear employee list.
					m_EmployeeNumbersToCopy = Nothing

				End If

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht kopiert werden."))
				End If

				' --end of copy contact ---

				' Update document dates.
				lblContactCreated.Text = String.Format("{0:f}, {1}", contactData.CreatedOn, contactData.CreatedFrom)
				lblContactChanged.Text = String.Format("{0:f}, {1}", contactData.ChangedOn, contactData.ChangedFrom)

				LoadCustomerDependentEmployeeContactData(contactData.ID)
				LoadYearsListData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)
				FilterContactData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)
				FocusContact(m_CustomerNumber, contactData.RecordNumber)

				RaiseEvent ContactDataSaved(Me, m_CustomerNumber, m_ResponsiblePersonRecordNumber, contactData.RecordNumber)

				' Notifiy system about changed contact
				Dim hub = MessageService.Instance.Hub
				Dim customerContactHasChangedMsg As New CustomerContactDataHasChanged(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CustomerNumber, m_CurrentContactRecordNumber)
				hub.Publish(customerContactHasChangedMsg)

			End If

		End If

	End Sub

	''' <summary>
	''' Copies the contact for employees.
	''' </summary>
	''' <param name="contactData">The contact data to copy.</param>
	''' <param name="employeeNumbers">The employee numbers.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function CopyContactForEmployees(ByVal contactData As ResponsiblePersonAssignedContactData,
											   ByVal contactDocumentID As Integer?,
											   ByVal employeeNumbers As Integer()) As Boolean

		Dim success As Boolean = True

		Dim dt = DateTime.Now

		' Load dependent employee contact list
		Dim dependentEmployeeContactList = m_CustomerDataAccess.LoadCustomerDependentEmployeeContactData(contactData.ID)

		' Copy contact for each employee.
		For Each employeeNumber In employeeNumbers

			' Create a shallow copy
			Dim copyOfContactData As DatabaseAccess.Employee.DataObjects.ContactMng.EmployeeContactData

			Dim employeeNr As Integer = employeeNumber
			Dim dependentEmployeeContact = dependentEmployeeContactList.Where(Function(data) data.EmployeeNumber = employeeNr).FirstOrDefault()

			If dependentEmployeeContact Is Nothing Then
				' Its a new contact
				copyOfContactData = New DatabaseAccess.Employee.DataObjects.ContactMng.EmployeeContactData
				copyOfContactData.ID = Nothing
				copyOfContactData.RecordNumber = Nothing
			Else
				' Update existing contact
				copyOfContactData = m_EmployeeDatabaseAccess.LoadEmployeeContact(employeeNumber, dependentEmployeeContact.EmployeeContactRecordNumber)

				If copyOfContactData Is Nothing Then
					Return False
				End If
			End If

			' Overwrite values
			copyOfContactData.EmployeeNumber = employeeNumber
			copyOfContactData.ContactsString = contactData.ContactsString
			copyOfContactData.ContactType1 = contactData.ContactType1
			copyOfContactData.ContactType2 = contactData.ContactType2
			copyOfContactData.ContactDate = contactData.ContactDate
			copyOfContactData.ContactPeriodString = contactData.ContactPeriodString
			copyOfContactData.ContactImportant = contactData.ContactImportant
			copyOfContactData.ContactFinished = contactData.ContactFinished
			copyOfContactData.CreatedOn = dt
			copyOfContactData.CreatedFrom = m_InitializationData.UserData.UserFullName
			copyOfContactData.ProposeNr = contactData.ProposeNr
			copyOfContactData.VacancyNumber = contactData.VacancyNumber
			copyOfContactData.OfNumber = contactData.OfNumber
			copyOfContactData.Mail_ID = contactData.Mail_ID
			copyOfContactData.TaskRecNr = contactData.TaskRecNr
			copyOfContactData.UsNr = contactData.UsNr
			copyOfContactData.ESNr = contactData.ESNr
			copyOfContactData.CustomerNumber = m_CustomerNumber
			copyOfContactData.CustomerContactRecId = contactData.ID
			copyOfContactData.KontaktDocID = contactData.KontaktDocID

			copyOfContactData.ChangedFrom = m_InitializationData.UserData.UserFullName
			copyOfContactData.ChangedOn = dt


			' Save the contact

			If copyOfContactData.ID > 0 Then
				copyOfContactData.ChangedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeContact(copyOfContactData)
			Else
				copyOfContactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(copyOfContactData)
			End If

			If Not success Then
				Exit For
			End If

		Next

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

			Dim recordToDelete = m_CustomerDataAccess.LoadAssignedContactDataOfResponsiblePerson(m_CustomerNumber, m_ResponsiblePersonRecordNumber, m_CurrentContactRecordNumber)

			If recordToDelete Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kontakt konnte nicht gelöscht werden."))
				Return
			End If

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																				m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then
				Dim success = m_CustomerDataAccess.DeleteResponsiblePersonContactAssignment(recordToDelete.ID)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kontakt konnte nicht gelöscht werden."))

					Return
				Else
					If recordToDelete.KontaktDocID.HasValue Then
						success = success AndAlso m_CustomerDataAccess.DeleteContactDocument(recordToDelete.KontaktDocID)

						If Not success Then
							m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das angehängte Dokument des Kontaktes konnte nicht gelöscht werden."))
						End If
					End If
				End If

				For Each dependentContact In m_CustomerDependentEmployeeContactData
					success = success AndAlso m_EmployeeDatabaseAccess.DeleteEmployeeContact(dependentContact.EmployeeContactID)
				Next

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ein oder mehrere verbundene Kandidatenkontakte konnten nicht gelöscht werden."))
				End If

				LoadYearsListData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)
				FilterContactData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)

				' Load the contact detail data of the first contact in the list.
				Dim firstContact = FirstContactInListOfContacts

				If Not firstContact Is Nothing Then
					LoadContactDetailData(m_CustomerNumber, m_ResponsiblePersonRecordNumber, firstContact.ContactRecordNumber)
				Else
					m_CurrentContactRecordNumber = Nothing
					m_ResponsiblePersonContactData = Nothing
					m_CustomerDependentEmployeeContactData = Nothing

					m_CurrentFileBytes = Nothing
					m_CurrentFileExtension = String.Empty

					m_CurrentDocumentID = Nothing
				End If

				m_SuppressUIEvents = True
				cboFilename.Text = String.Empty
				m_SuppressUIEvents = False

				RaiseEvent ContactDataDeleted(Me, m_CustomerNumber, m_ResponsiblePersonRecordNumber, recordToDelete.RecordNumber)

			End If

		End If

	End Sub

	''' <summary>
	''' Handles click on employee copy contact button.
	''' </summary>
	Private Sub OnBtnCopyContactForEmployees_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyContactForEmployees.Click

		If ValidateContactInputData() Then

			Dim frm As New frmCopyContact(m_InitializationData)

			If Not m_CustomerDependentEmployeeContactData Is Nothing Then
				Dim selectedEmployees() As Integer = m_CustomerDependentEmployeeContactData.Select(Function(data) data.EmployeeNumber).ToArray()
				frm.LoadData(selectedEmployees)
			Else
				frm.LoadData()
			End If
			If Not lueES.EditValue Is Nothing Then
				Dim data = TryCast(lueES.GetSelectedDataRow(), ESData)
				frm.FocusEmployee(data.EmployeeNumber)
			ElseIf Not luePropose.EditValue Is Nothing Then
				Dim data = TryCast(luePropose.GetSelectedDataRow(), ProposeData)
				frm.FocusEmployee(data.EmployeeNumber)

			End If
			frm.ShowDialog()

			If frm.DialogResult = DialogResult.OK Then

				Dim selectedEmplyoeeNumbers As Integer() = frm.SelectedEmployeeNumbers

				If Not selectedEmplyoeeNumbers Is Nothing Then
					m_EmployeeNumbersToCopy = selectedEmplyoeeNumbers
				End If

				SaveData()

			End If

		End If

	End Sub

	''' <summary>
	''' Handles change of filter checkbox.
	''' </summary>
	Private Sub OnFilterCheckBox_CheckedChanged(sender As System.Object, e As System.EventArgs)

		If m_SuppressUIEvents Then
			Return
		End If

		FilterContactData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)

		If (m_CurrentContactRecordNumber.HasValue) Then
			FocusContact(m_CustomerNumber, m_CurrentContactRecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles change of year checkbox.
	''' </summary>
	Private Sub OnLstYears_ItemCheck(sender As System.Object, e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs) Handles lstYears.ItemCheck

		If m_SuppressUIEvents Then
			Return
		End If

		FilterContactData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)

		If (m_CurrentContactRecordNumber.HasValue) Then
			FocusContact(m_CustomerNumber, m_CurrentContactRecordNumber)
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

			'			If fileInfo.Extension.ToLower() = ".pdf" OrElse fileInfo.Extension.ToLower() = ".msg" Then
			LoadDocumentBytesFormFileSystem(fileInfo.FullName)
			'End If

		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OnFrmContacts_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		e.Effect = DragDropEffects.Copy
		'If e.Data.GetDataPresent("FileGroupDescriptor") Then
		'	e.Effect = DragDropEffects.Copy
		'End If

		'  If Not files Is Nothing AndAlso files.Count > 0 Then
		'    Dim fileInfo As New FileInfo(files(0))

		'    If fileInfo.Extension.ToLower() = ".pdf" Then
		'      e.Effect = DragDropEffects.Copy
		'    Else
		'      e.Effect = DragDropEffects.None
		'    End If

		'  End If
	End Sub

	''' <summary>
	''' Handles double click on contact grid to open document
	''' </summary>
	Private Sub OnGvContactData_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) And m_CurrentDocumentID.HasValue Then
			OpenDocument()
		End If

	End Sub


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
	''' Handles key down on dependent employee contacts grid.
	''' </summary>
	Private Sub OnGridDependentEmployeeContacts_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridDependentEmployeeContacts.KeyDown

		If (e.KeyCode = Keys.Delete) Then

			Dim selectedDepenentContact = SelectedCustomerDependentEmployeeContactData

			If Not selectedDepenentContact Is Nothing Then

				If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie diesen Kandidatenkontakt wirklich löschen?"))) Then
					Dim success As Boolean = True

					success = success AndAlso m_EmployeeDatabaseAccess.DeleteEmployeeContact(selectedDepenentContact.EmployeeContactID)
					success = success AndAlso LoadCustomerDependentEmployeeContactData(selectedDepenentContact.CustomerContactID)
				End If

			End If

		End If

	End Sub

	''' <summary>
	''' Handles click on file path button.
	''' </summary>
	Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cboFilename.ButtonClick

		If e.Button.Index = 0 Then

			With OpenFileDialog1
				.Filter =
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
			Dim documentData = m_CustomerDataAccess.LoadContactDocumentData(m_CurrentDocumentID, True)

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
	''' Opens an employee contact.
	''' </summary>
	Private Sub OpenEmployeeContact(ByVal employeeNr As Integer, ByVal contatRecNr As Integer)

		' Send a request to open a customerMng form.
		Dim hub = MessageService.Instance.Hub
		Dim openCustomerContact As New OpenMAKontaktMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNr, contatRecNr)
		hub.Publish(openCustomerContact)
	End Sub

	''' <summary>
	''' Opens an employee.
	''' </summary>
	Private Sub OpenEmployee(ByVal employeeNumber As Integer)

		' Send a request to open a employeeMng form.
		Dim hub = MessageService.Instance.Hub
		Dim openCustomerMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNumber)
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

				If Not row Is Nothing AndAlso row.DocumentId.HasValue Then

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

		Dim success = m_CustomerDataAccess.DeleteContactDocument(documentID)

		If success Then
			FilterContactData(m_CustomerNumber, m_ResponsiblePersonRecordNumber)

			If m_CurrentContactRecordNumber.HasValue Then
				FocusContact(m_CustomerNumber, m_CurrentContactRecordNumber)
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
		Dim EmployeeNumber As Integer? = Nothing
		Dim CustomerNumber As Integer? = m_CustomerNumber
		Dim ResponsiblePersonRecordNumber As Integer? = m_ResponsiblePersonRecordNumber
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
	''' Handles click on open dependent employee contatact.
	''' </summary>
	Private Sub OnLblWarnOnCopy_Click(sender As Object, e As EventArgs) Handles lblWarnOnCopy.Click

		Dim contactResult = m_CustomerDataAccess.LoadAssignedContactDataOfResponsiblePerson(m_CustomerNumber, m_ResponsiblePersonRecordNumber, m_CurrentContactRecordNumber)

		If (Not contactResult Is Nothing AndAlso contactResult.EmployeeContactRecID.HasValue) Then

			OpenEmployeeContact(contactResult.EmployeeContactEmployeeNr, contactResult.EmplyoeeContactRecNr)

		End If

	End Sub

	''' <summary>
	''' Handles cell click on responsible person grid.
	''' </summary>
	Private Sub OnGvResponsiblePerson_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvDependentEmployeeContacts.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvDependentEmployeeContacts.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim row = CType(dataRow, CustomerDependentEmployeeContactData)

				Select Case column.Name
					Case "Name", "Address"
						OpenEmployee(row.EmployeeNumber)
					Case "dependentEmployeeContactSymbol"
						OpenEmployeeContact(row.EmployeeNumber, row.EmployeeContactRecordNumber)
					Case Else
						' Do nothing
				End Select

			End If

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

			Dim rowData = CType(gvVacancy.GetRow(e.RowHandle), VacancyData)

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

			Dim rowData = CType(gvPropose.GetRow(e.RowHandle), ProposeData)

			If Not rowData.P_State Is Nothing AndAlso (rowData.P_State.ToLower.Contains("absage") Or rowData.P_State.ToLower.Contains("abgeschlossen")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

	''' <summary>
	''' Handles unbound column data event of gvDependentEmployeeContacts.
	''' </summary>
	Private Sub OnGvDependentEmployeeContactss_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvDependentEmployeeContacts.CustomUnboundColumnData

		If e.Column.Name = "dependentEmployeeContactSymbol" Then
			If (e.IsGetData()) Then
				e.Value = m_ContactImage
			End If
		End If
	End Sub

	''' <summary>
	''' Handles employee contact change notification messages.
	''' </summary>
	''' <param name="msg">The employee contact notification message.</param>
	Private Sub HandleEmployeeDataHasChangedMsg(ByVal msg As EmployeeContactDataHasChanged)

		If Not m_ResponsiblePersonContactData Is Nothing AndAlso
		  m_InitializationData.MDData.MDNr = msg.MDNr AndAlso
		  m_ResponsiblePersonContactData.EmployeeContactEmployeeNr = msg.EmployeeNumber AndAlso
		  m_ResponsiblePersonContactData.EmplyoeeContactRecNr = msg.ContactRecNr Then

			' The partent contact data has changed. This means this contact data has also changed -> reload.
			LoadContactDetailData(m_CustomerNumber, m_ResponsiblePersonRecordNumber, m_CurrentContactRecordNumber)

		End If

	End Sub


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
	''' <param name="customerNumber">The customer number.</param>
	''' <param name="contactRecordNumber">The contact record number</param>
	Private Sub FocusContact(ByVal customerNumber As Integer, ByVal contactRecordNumber As Integer)

		If Not gridContactData.DataSource Is Nothing Then

			Dim contactViewData = CType(gvContactData.DataSource, BindingList(Of ContactViewData))

			Dim index = contactViewData.ToList().FindIndex(Function(data) data.CustomerNumber = customerNumber And data.ContactRecordNumber = contactRecordNumber)

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


	''' <summary>
	''' Shows or hieds the copied from employee label.
	''' </summary>
	Private Sub SwitchUICompentsForCopiedRecord(ByVal isCopied As Boolean)

		lblKopiertVonKandidat.Visible = isCopied
		lblKopiertVonKandidatValue.Visible = isCopied
		lblKopienKandidat.Visible = Not isCopied
		gridDependentEmployeeContacts.Visible = Not isCopied
		btnCopyContactForEmployees.Visible = Not isCopied

	End Sub

#End Region

#Region "View helper classes"

	''' <summary>
	'''  Contact view data.
	''' </summary>
	Class ContactViewData
		Public Property CustomerNumber As Integer
		Public Property ResponsiblePersonNumber As Integer?
		Public Property ContactRecordNumber As Integer?
		Public Property ContactDate As DateTime?
		Public Property Person_Subject As String
		Public Property Description As String
		Public Property Important As Boolean?
		Public Property Completed As Boolean?
		Public Property KST As String
		Public Property PDFImage As Image
		Public Property DocumentId As Integer?

	End Class


	''' <summary>
	''' Responsible person view data.
	''' </summary>
	Class ResponsiblePersonViewData

		Public Property Name As String
		Public Property ResponsiblePersonRecordNumber As Integer?

		Public Property ID As Integer
		Public Property CustomerNumber As Integer
		Public Property RecordNumber As Integer
		Public Property Position As String
		Public Property Department As String
		Public Property TrnslatedSalutation As String
		Public Property Firstname_Lastname As String
		Public Property Telephone As String
		Public Property Telefax As String
		Public Property MobilePhone As String
		Public Property Email As String
		Public Property TranslatedZHowKontakt As String
		Public Property TranslatedZState1 As String
		Public Property TranslatedZState2 As String
		Public Property ZState1 As String
		Public Property ZState2 As String

		Public ReadOnly Property Position_Department As String
			Get
				Return String.Format("{0} / {1}", Position, Department)
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

	Public Property title As String ' Bezeichnung
	Public Property description As String ' Beschreibung
	Public Property customerVacancyNumber As Integer?
	Public Property customerProposeNumber As Integer?
	Public Property customerESNumber As Integer?

	Public Property EmployeeCopyList As List(Of Integer)

End Class
