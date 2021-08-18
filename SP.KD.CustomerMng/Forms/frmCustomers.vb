
Imports SPProgUtility

Imports System.Reflection.Assembly
Imports System.IO
Imports System.ComponentModel
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports System.IO.File
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraBars.Alerter
Imports DevExpress.XtraBars
Imports DevExpress.Utils.Menu
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Popup
Imports System.Threading
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Reflection
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors
Imports System.Text.RegularExpressions
Imports DevExpress.XtraNavBar
Imports SPS.Listing.Print.Utility
Imports SP.KD.CustomerMng.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Settings
Imports SP.KD.CPersonMng.UI
Imports SP.Infrastructure.Logging
Imports SPS.ExternalServices
Imports SPS.ExternalServices.DeltavistaWebService
Imports SP.Infrastructure
Imports System.Text
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility

Imports SP.TodoMng
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.ProgPath


Namespace UI

	''' <summary>
	''' Customer management form.
	''' </summary>
	Public Class frmCustomers



#Region "Private Consts"

		Private Const USER_XML_SETTING_SPUTNIK_RESPONSIBLEPERSON_CUSTOMER_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/customer/responsibleperson/restorelayoutfromxml"
		Private Const USER_XML_SETTING_SPUTNIK_RESPONSIBLEPERSON_CUSTOMER_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/customer/responsibleperson/keepfilter"

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
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' The active user control.
		''' </summary>
		Private m_ActiveUserControl As ucBaseControl

		''' <summary>
		''' List of tab controls.
		''' </summary>
		Private m_ListOfTabControls As New List(Of ucBaseControl)

		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_DataAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Protected m_SettingsManager As ISettingsManager

		''' <summary>
		''' Contains the customer number of the loaded customer.
		''' </summary>
		Private m_CustomerNumber As Integer?

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Protected m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Protected m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if UI events should be suppressed.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = True

		''' <summary>
		''' Responsible person form.
		''' </summary>
		Private m_ResponsiblePersonsForm As frmResponsiblePerson

		''' <summary>
		''' Boolan flag indicating if the form has been initialized.
		''' </summary>
		Private m_IsInitialized = False

		Private Property m_MetroForeColor As System.Drawing.Color
		Private Property m_MetroBorderColor As System.Drawing.Color

		Private Property m_PrintJobNr As String
		Private Property m_SQL4Print As String
		Private Property m_bPrintAsDesign As Boolean

		Private m_mandant As Mandant

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_path As ClsProgPath


		Private m_DeltavistaWSReferenceNumber As String
		Private m_DeltavistaWSUserName As String
		Private m_DeltavistaWSPassword As String
		Private m_DeltavistaWSServiceUrl As String

		'''' <summary>
		'''' WOS NavBar Item.
		'''' </summary>
		'Private m_Wos_P_Data As NavBarItem

		Private m_PropertyForm As frmCustomersProperties
		Private m_AllowedDesign As Boolean
		Private m_AllowedProfilMatcher As Boolean


		Private Property GridSettingPath As String
		Private UserGridSettingsXml As SettingsXml

		Private m_GVResponsiblepersonCustomerSettingfilename As String

		Private m_xmlSettingResponsiblepersonCustomerFilter As String
		Private m_xmlSettingRestoreResponsiblepersonCustomerSetting As String


#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			InitializeComponent()

			Try
				' Mandantendaten
				m_mandant = New Mandant
				m_path = New ClsProgPath
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_ActiveUserControl = ucCommonData

			m_ListOfTabControls.Add(ucCommonData)
			m_ListOfTabControls.Add(ucMediationAndRental)
			m_ListOfTabControls.Add(ucContactData)
			m_ListOfTabControls.Add(ucAccountAndSales)
			m_ListOfTabControls.Add(ucAdditionalInfo)
			m_ListOfTabControls.Add(ucDocumentManagement)

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfTabControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
			Next

			m_DataAccess = New SP.DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			Try
				Dim mSettingpath = String.Format("{0}Customer\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
				If Not Directory.Exists(mSettingpath) Then Directory.CreateDirectory(mSettingpath)

				m_GVResponsiblepersonCustomerSettingfilename = String.Format("{0}{1}{2}.xml", mSettingpath, gvResponsiblePersons.Name, m_InitializationData.UserData.UserNr)

				m_xmlSettingRestoreResponsiblepersonCustomerSetting = String.Format(USER_XML_SETTING_SPUTNIK_RESPONSIBLEPERSON_CUSTOMER_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr)
				m_xmlSettingResponsiblepersonCustomerFilter = String.Format(USER_XML_SETTING_SPUTNIK_RESPONSIBLEPERSON_CUSTOMER_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr)

			Catch ex As Exception

			End Try

			Try
				m_DeltavistaWSReferenceNumber = m_mandant.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, "").dvrefnr
				m_DeltavistaWSUserName = m_mandant.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, "").dvusername
				m_DeltavistaWSPassword = m_mandant.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, "").dvuserpw
				m_DeltavistaWSServiceUrl = m_mandant.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, "").dvurl

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try
			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 205, m_InitializationData.MDData.MDNr)
			m_AllowedProfilMatcher = m_mandant.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, "").PMSearch
			m_AllowedProfilMatcher = m_AllowedProfilMatcher AndAlso m_InitializationData.UserData.UserNr = 1 AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 679, m_InitializationData.MDData.MDNr)

			TranslateControls()

			' Creates the navigation bar.
			CreateMyNavBar()

			AddHandler gvResponsiblePersons.ColumnFilterChanged, AddressOf OngvResponsiblepersonCustomerColumnPositionChanged
			AddHandler gvResponsiblePersons.ColumnPositionChanged, AddressOf OngvResponsiblepersonCustomerColumnPositionChanged
			AddHandler gvResponsiblePersons.ColumnWidthChanged, AddressOf OngvResponsiblepersonCustomerColumnPositionChanged


		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Boolean flag indicating if customer data is loaded.
		''' </summary>
		Public ReadOnly Property IsCustomerDataLoaded As Boolean
			Get
				Return m_CustomerNumber.HasValue
			End Get

		End Property

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the selected responsible person view data.
		''' </summary>
		''' <returns>The selected responsible person view data or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedResponsiblePersonViewData As ResponsiblePersonViewData
			Get
				Dim grdView = TryCast(grdZHD.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim overviewData = CType(grdView.GetRow(selectedRows(0)), ResponsiblePersonViewData)
						Return overviewData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' get datamatrix printername from userprofile
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetUserDataMaxtrixPrintername() As String
			Get
				Dim sp_utility As New SPProgUtility.MainUtilities.Utilities

				Dim strQuery As String = "//Report/matrixprintername"
				Dim strStyleName As String = sp_utility.GetXMLValueByQueryWithFilename(m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr), strQuery, Nothing)

				Return strStyleName

			End Get
		End Property

		''' <summary>
		''' get datamatrix code from mandant
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetDataMaxtrixCodeString() As String
			Get
				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
				Dim strQuery As String = String.Format("{0}/datamatrixcodestringforcustomerlabel", FORM_XML_MAIN_KEY)
				Dim dataMatrixCode As String = m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), strQuery)
				If String.IsNullOrWhiteSpace(dataMatrixCode) OrElse dataMatrixCode.Length < 10 Then dataMatrixCode = "KD_{0}_999"

				Return dataMatrixCode

			End Get
		End Property


#End Region

#Region "Public Methods"


		''' <summary>
		''' Show the data of a customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadCustomerData(ByVal customerNumber As Integer?) As Boolean

			If Not m_IsInitialized Then
				Reset()
				m_IsInitialized = True
			End If

			CleanUp()

			m_SuppressUIEvents = True

			Dim success As Boolean = True

			success = success AndAlso m_ActiveUserControl.Activate(customerNumber)

			If (customerNumber.HasValue) Then

				success = success AndAlso LoadResponsiblePersonData(customerNumber.Value)
				success = success AndAlso PrepareStatusAndNavigationBar(customerNumber.Value)

				m_CustomerNumber = IIf(success, customerNumber, Nothing)
			End If

			If Not customerNumber.HasValue Then
				ShowNewFormForCustomer(True)
			End If

			m_SuppressUIEvents = False

			Return success
		End Function


		''' <summary>
		''' Gets the currently seleced postcode data in UI.
		''' </summary>
		'''<param name="customerNumber">Customer number.</param>
		''' <returns>Currently selected postcode data.</returns>
		Public Function GetUISelectedPostCodeOfCustomer(ByVal customerNumber As Integer) As PostCodeData

			Dim postCodeData As PostCodeData = Nothing

			If IsCustomerDataLoaded AndAlso
				customerNumber = ucCommonData.CustomerNumber Then
				postCodeData = TryCast(ucCommonData.luePostcode.GetSelectedDataRow(), PostCodeData)
			End If

			Return postCodeData

		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.grpzustaendigepersonen.Text = m_Translate.GetSafeTranslationValue(Me.grpzustaendigepersonen.Text)

			Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
			Me.xtabBilling.Text = m_Translate.GetSafeTranslationValue(Me.xtabBilling.Text)
			Me.xtabContact.Text = m_Translate.GetSafeTranslationValue(Me.xtabContact.Text)
			Me.xtabDocMng.Text = m_Translate.GetSafeTranslationValue(Me.xtabDocMng.Text)
			Me.xtabOthers.Text = m_Translate.GetSafeTranslationValue(Me.xtabOthers.Text)
			Me.xtabVermittlung.Text = m_Translate.GetSafeTranslationValue(Me.xtabVermittlung.Text)

			Me.bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblErstellt.Caption)
			Me.bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblGeaendert.Caption)
			Me.bsiCreated.Caption = String.Empty
			Me.bsiChanged.Caption = String.Empty

			Me.grpzustaendigepersonen.Text = m_Translate.GetSafeTranslationValue(Me.grpzustaendigepersonen.Text)

		End Sub

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()

			m_SuppressUIEvents = True

			' Reset all the child controls
			For Each tabControl In m_ListOfTabControls
				tabControl.Reset()
			Next

			m_ActiveUserControl.Deactivate()
			m_ActiveUserControl = ucCommonData
			XtraTabControl1.SelectedTabPage = xtabAllgemein

			m_SuppressUIEvents = False

			ResetGridResponsiblePerson()

			m_CustomerNumber = Nothing

		End Sub

		''' <summary>
		''' Resets the responsible person grid.
		''' </summary>
		Private Sub ResetGridResponsiblePerson()

			gvResponsiblePersons.OptionsView.ShowIndicator = False
			gvResponsiblePersons.OptionsView.ShowAutoFilterRow = True
			gvResponsiblePersons.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			' Reset the grid
			gvResponsiblePersons.Columns.Clear()

			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnCustomerNumber.Name = "CustomerNumber"
			columnCustomerNumber.FieldName = "CustomerNumber"
			columnCustomerNumber.Visible = False
			gvResponsiblePersons.Columns.Add(columnCustomerNumber)

			Dim columnPositionDepartment As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPositionDepartment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPositionDepartment.Caption = m_Translate.GetSafeTranslationValue("Position/Abteilung")
			columnPositionDepartment.Name = "Position_Department"
			columnPositionDepartment.FieldName = "Position_Department"
			columnPositionDepartment.Visible = True
			gvResponsiblePersons.Columns.Add(columnPositionDepartment)

			Dim columnPosition As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPosition.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPosition.Caption = m_Translate.GetSafeTranslationValue("Position")
			columnPosition.Name = "Position"
			columnPosition.FieldName = "Position"
			columnPosition.Visible = False
			gvResponsiblePersons.Columns.Add(columnPosition)

			Dim columnDepartment As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDepartment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDepartment.Caption = m_Translate.GetSafeTranslationValue("Abteilung")
			columnDepartment.Name = "Department"
			columnDepartment.FieldName = "Department"
			columnDepartment.Visible = False
			gvResponsiblePersons.Columns.Add(columnDepartment)

			Dim columnTranslatedSalutation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTranslatedSalutation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTranslatedSalutation.Caption = m_Translate.GetSafeTranslationValue("Anrede")
			columnTranslatedSalutation.Name = "TrnslatedSalutation"
			columnTranslatedSalutation.FieldName = "TrnslatedSalutation"
			columnTranslatedSalutation.Visible = True
			gvResponsiblePersons.Columns.Add(columnTranslatedSalutation)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.Caption = m_Translate.GetSafeTranslationValue("Nach-/Vorname")
			columnName.Name = "Firstname_Lastname"
			columnName.FieldName = "Firstname_Lastname"
			columnName.Visible = True
			gvResponsiblePersons.Columns.Add(columnName)

			Dim columnTelephone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTelephone.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTelephone.Caption = m_Translate.GetSafeTranslationValue("Telefon")
			columnTelephone.Name = "Telephone"
			columnTelephone.FieldName = "Telephone"
			columnTelephone.Visible = True
			gvResponsiblePersons.Columns.Add(columnTelephone)

			Dim columnTelefax As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTelefax.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTelefax.Caption = m_Translate.GetSafeTranslationValue("Telefax")
			columnTelefax.Name = "Telefax"
			columnTelefax.FieldName = "Telefax"
			columnTelefax.Visible = True
			gvResponsiblePersons.Columns.Add(columnTelefax)

			Dim columnMobilePhone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMobilePhone.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMobilePhone.Caption = m_Translate.GetSafeTranslationValue("Natel")
			columnMobilePhone.Name = "MobilePhone"
			columnMobilePhone.FieldName = "MobilePhone"
			columnMobilePhone.Visible = True
			gvResponsiblePersons.Columns.Add(columnMobilePhone)

			Dim columnEmail As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmail.Caption = m_Translate.GetSafeTranslationValue("Email")
			columnEmail.Name = "Email"
			columnEmail.FieldName = "Email"
			columnEmail.Visible = True
			gvResponsiblePersons.Columns.Add(columnEmail)

			'Dim columnIsZHDActiv As New DevExpress.XtraGrid.Columns.GridColumn()
			'columnIsZHDActiv.Caption = m_Translate.GetSafeTranslationValue("Aktiv?")
			'columnIsZHDActiv.Name = "IsZHDActiv"
			'columnIsZHDActiv.FieldName = "IsZHDActiv"
			'columnIsZHDActiv.MaxWidth = 50
			'columnIsZHDActiv.Visible = True
			'gvResponsiblePersons.Columns.Add(columnIsZHDActiv)


			RestoreGridLayoutFromXml(gvResponsiblePersons.Name)

			grdZHD.DataSource = Nothing

		End Sub

		''' <summary>
		''' Loads responsible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadResponsiblePersonData(ByVal customerNumber As Integer) As Boolean

			Dim responsiblePersonData As IEnumerable(Of ResponsiblePersonData) = m_DataAccess.LoadResponsiblePersonData(customerNumber)

			If (responsiblePersonData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuständige Personen konnten nicht geladen werden."))
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In responsiblePersonData
					Select New ResponsiblePersonViewData With
								 {.ID = person.ID,
									.CustomerNumber = person.CustomerNumber,
									.RecordNumber = person.RecordNumber,
									.Position = person.Position,
									.Department = person.Department,
									.TrnslatedSalutation = person.TranslatedSalutation,
									.Firstname_Lastname = FormatResponsiblePersonName(person.Lastname, person.Firstname),
									.Telephone = person.Telephone,
									.Telefax = person.Telefax,
									.MobilePhone = person.MobilePhone,
									.Email = person.Email,
									.TranslatedZState1 = person.TranslatedZState1,
									.TranslatedZState2 = person.TranslatedZState2,
									.ZState1 = person.ZState1,
									.ZState2 = person.ZState2,
									.TranslatedZHowKontakt = person.TranslatedZHowKontakt}).ToList()

			Dim listDataSource As BindingList(Of ResponsiblePersonViewData) = New BindingList(Of ResponsiblePersonViewData)

			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdZHD.DataSource = listDataSource

			Return True

		End Function

		''' <summary>
		''' Formats the responsible person name (lastname and firstname)
		''' </summary>
		Private Function FormatResponsiblePersonName(ByVal lastname As String, ByVal firstname As String) As String

			If (String.IsNullOrEmpty(lastname) AndAlso
					String.IsNullOrEmpty(firstname)) Then
				Return "-"
			ElseIf String.IsNullOrEmpty(firstname) AndAlso
					Not String.IsNullOrEmpty(lastname) Then
				Return String.Format("{0}", lastname)
			Else
				Return String.Format("{0} {1}", lastname, firstname)
			End If

		End Function

		''' <summary>
		''' Saves Customerdata.
		''' </summary>
		Public Sub SaveCustomerData()

			If (IsCustomerDataLoaded AndAlso ValidateData()) Then

				Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(m_CustomerNumber, m_InitializationData.UserData.UserFiliale)

				If customerMasterData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

				Dim customerDataFromDatabase = customerMasterData.Clone()

				customerMasterData.ChangedOn = DateTime.Now
				customerMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName
				customerMasterData.ChangedUserNumber = m_InitializationData.UserData.UserNr

				ucCommonData.MergeCustomerMasterData(customerMasterData)
				ucMediationAndRental.MergeCustomerMasterData(customerMasterData)
				ucAccountAndSales.MergeCustomerMasterData(customerMasterData)
				ucAdditionalInfo.MergeCustomerMasterData(customerMasterData)

				' Update the customer
				Dim success = m_DataAccess.UpdateCustomerMasterData(customerMasterData)

				Dim message As String = String.Empty

				If AreCustomerAddressDataDifferent(customerMasterData, customerDataFromDatabase) Then

					If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie geänderten Adressdaten auch für die zuständigen Personen übernehmen?")) Then

						success = success AndAlso m_DataAccess.CopyCustomerAddressToResponsiblePersons(m_CustomerNumber)

						If success Then
							' Notifiy system to update responsible person address.
							Dim hub = MessageService.Instance.Hub
							Dim notifyRefreshResponsiblePersonAddress As New RefreshResponsiblePersonAddress(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CustomerNumber)
							hub.Publish(notifyRefreshResponsiblePersonAddress)
						End If

					End If

					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Achtung: Die geänderten Adressdaten werden in Fakturaadressen nicht automatisch geändert!"))
				End If

				If (success) Then
					bsiChanged.Caption = String.Format(" {0:f}, {1}", customerMasterData.ChangedOn, customerMasterData.ChangedFrom)
					m_UtilityUI.ShowInfoDialog((m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")), m_Translate.GetSafeTranslationValue("Daten speichern"))

				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
				End If

			End If
		End Sub


		''' <summary>
		''' Checks if two customer data records have different addresses.
		''' </summary>
		''' <param name="a">Customer1</param>
		''' <param name="b">Customer2</param>
		''' <returns>Boolean truth value.</returns>
		Private Function AreCustomerAddressDataDifferent(ByVal a As CustomerMasterData, ByVal b As CustomerMasterData) As Boolean

			Dim didAddressChange As Boolean = Not (a.Company1 = b.Company1 AndAlso
																				a.PostOfficeBox = b.PostOfficeBox AndAlso
																				a.Street = b.Street AndAlso
																				a.CountryCode = b.CountryCode AndAlso
																				a.Postcode = b.Postcode AndAlso
																				a.Location = b.Location)

			Return didAddressChange
		End Function

		''' <summary>
		''' Handles the form load event.
		''' </summary>
		Private Sub OnForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			' Init styles etc.

			Try
				Me.KeyPreview = True
				Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
				If strStyleName <> String.Empty Then
					UserLookAndFeel.Default.SetSkinStyle(strStyleName)
				End If
				Dim liColor As List(Of Color) = GetMetroColor("INFO")	 ' Color.White |  Color.Orange

				If liColor.Count < 1 Then liColor = New List(Of Color)(New Color() {Color.White, Color.Orange})
				Me.m_MetroForeColor = liColor(0)
				Me.m_MetroBorderColor = liColor(1)
				StyleManager.MetroColorGeneratorParameters = New MetroColorGeneratorParameters(Me.m_MetroForeColor, Me.m_MetroBorderColor)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

			End Try

		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmCustomers_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged
			If Visible Then
				' Load form settings 
				LoadFormSettings()
			End If
		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnFrmCustomers_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

			CleanupAndHideForm()

			e.Cancel = True

		End Sub

		''' <summary>
		''' Clickevent for Navbar.
		''' </summary>
		Private Sub OnnbMain_LinkClicked(ByVal sender As Object, _
																 ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim bForDesign As Boolean = False
			Try
				Trace.WriteLine(String.Format("{0} >>> {1}", e.Link.ItemName, e.Link.Caption))
				Dim strLinkName As String = e.Link.ItemName
				Dim strLinkCaption As String = e.Link.Caption

				For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
					e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
				Next
				e.Link.Item.Appearance.ForeColor = Color.Orange

				Select Case strLinkName.ToLower
					Case "New_Customer".ToLower
						ShowNewFormForCustomer(False)

					Case "Save_customer_Data".ToLower
						SaveCustomerData()

					Case "Print_customer_Data".ToLower
						GetMenuItems4Print()

					Case "Close_Customer_Form".ToLower
						CleanupAndHideForm()

					Case "delete_customer_Data".ToLower
						If DeleteSelectedCustomer() = DeleteCustomerAddressAssignmentResult.Deleted Then CleanupAndHideForm()

					Case "pm_Customer_Data".ToLower
						LoadProfilmatcherDataForAssignedCustomer()
					Case "customer_properties".ToLower
						ShowCustomerProperties()
					'Case "wos_Customer_Data".ToLower
					'	SendWOSLink()

					Case "P_MakePropose".ToLower

					Case "P_MakeES".ToLower

					Case "solvency_check".ToLower
						Dim info As New TaskDialogInfo
						info = CreateSolvencyCheckOptionDialog()
						Dim result As eTaskDialogResult = TaskDialog.Show(info)

						Select Case result
							Case eTaskDialogResult.Custom1
								RunQuickBusinessCheck()
							Case eTaskDialogResult.Custom2
								RunBusinessCheck()
							Case Else
								' Do nothing

						End Select

					Case "CreateTODO".ToLower
						Dim frmTodo As New frmTodo(m_InitializationData)
						' optional init new todo
						Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
						Dim EmployeeNumber As Integer? = Nothing
						Dim CustomerNumber As Integer? = m_CustomerNumber
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

						frmTodo.CustomerNumber = m_CustomerNumber
						frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber, VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

						frmTodo.Show()

					Case Else


				End Select
				'If Not Me.tbVorschlag.Visible And Not Me.pcEditor.Visible Then Me.tbVorschlag.Visible = True

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(ex.Message)

			Finally

			End Try

		End Sub

		''' <summary>
		''' Handles tab control selectioin changing
		''' </summary>
		Private Sub OnxtraTabControl_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles XtraTabControl1.SelectedPageChanging

			If m_SuppressUIEvents Then
				Return
			End If

			Dim page = e.Page

			If Not (m_ActiveUserControl Is Nothing) Then
				m_ActiveUserControl.Deactivate()
			End If

			If (Object.ReferenceEquals(page, xtabAllgemein)) Then
				ucCommonData.Activate(m_CustomerNumber)
				m_ActiveUserControl = ucCommonData
			ElseIf Object.ReferenceEquals(page, xtabVermittlung) Then
				ucMediationAndRental.Activate(m_CustomerNumber)
				m_ActiveUserControl = ucMediationAndRental
			ElseIf Object.ReferenceEquals(page, xtabContact) Then
				ucContactData.Activate(m_CustomerNumber)
				m_ActiveUserControl = ucContactData
			ElseIf Object.ReferenceEquals(page, xtabBilling) Then
				ucAccountAndSales.Activate(m_CustomerNumber)
				m_ActiveUserControl = ucAccountAndSales
			ElseIf Object.ReferenceEquals(page, xtabOthers) Then
				ucAdditionalInfo.Activate(m_CustomerNumber)
				m_ActiveUserControl = ucAdditionalInfo
			ElseIf Object.ReferenceEquals(page, xtabDocMng) Then
				ucDocumentManagement.Activate(m_CustomerNumber)
				m_ActiveUserControl = ucDocumentManagement
			End If

		End Sub

		''' <summary>
		''' Handles cell click on responsible person grid.
		''' </summary>
		Private Sub OnGvResponsiblePerson_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvResponsiblePersons.RowCellClick
			'Const ModuleResponsiblePersonNumber As Integer = 3

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvResponsiblePersons.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim viewData = CType(dataRow, ResponsiblePersonViewData)

					Select Case column.Name
						Case "Telephone"
							If Not String.IsNullOrEmpty(viewData.Telephone) Then
								ucCommonData.OpenTelephone(viewData.Telephone) ', 0, viewData.CustomerNumber, viewData.RecordNumber, 0, ModuleResponsiblePersonNumber, 0)
							Else
								ShowResponsiblePersonForm(m_CustomerNumber, viewData.RecordNumber)
							End If

						Case "MobilePhone"
							If Not String.IsNullOrEmpty(viewData.MobilePhone) Then
								ucCommonData.OpenTelephone(viewData.MobilePhone) ', 0, viewData.CustomerNumber, viewData.RecordNumber, 0, ModuleResponsiblePersonNumber, 0)
							Else
								ShowResponsiblePersonForm(m_CustomerNumber, viewData.RecordNumber)
							End If

						Case "Email"
							If Not String.IsNullOrEmpty(viewData.Email) Then
								m_UtilityUI.OpenEmail(viewData.Email)
								Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {.MDData = m_InitializationData.MDData,
																																							 .ProsonalizedData = m_InitializationData.ProsonalizedData,
																																							 .TranslationData = m_InitializationData.TranslationData,
																																							 .UserData = m_InitializationData.UserData})

								obj.NewResponsiblePersonContact(m_CustomerNumber, viewData.Email,
																			 String.Empty, viewData.RecordNumber,
																			 Now, m_InitializationData.UserData.UserFullName, Nothing,
																			 Now, m_InitializationData.UserData.UserFullName, Nothing, Nothing, "Einzelmail", 1, False, True,
																			 False)

							Else
								ShowResponsiblePersonForm(m_CustomerNumber, viewData.RecordNumber)
							End If
						Case Else
							ShowResponsiblePersonForm(m_CustomerNumber, viewData.RecordNumber)
					End Select

				End If

			End If

		End Sub

		''' <summary>
		''' Handles keydown on reponsible persons grid.
		''' </summary>
		Private Sub OnGridReponsiblePersons_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdZHD.KeyDown

			If (e.KeyCode = Keys.Delete) Then

				DeleteSelectedResponsiblePerson()

			End If
		End Sub

		''' <summary>
		'''  Handles RowStyle event of gvResponsiblePersons grid view.
		''' </summary>
		Private Sub OngvResponsiblePersons_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvResponsiblePersons.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvResponsiblePersons.GetRow(e.RowHandle), ResponsiblePersonViewData)

				If Not rowData.IsZHDActiv.GetValueOrDefault(True) Then
					' (rowData.ZState1.Contains("inaktiv") OrElse rowData.ZState1.Contains("nicht aktiv") OrElse rowData.ZState2.Contains("inaktiv") OrElse rowData.ZState2.Contains("mehr aktiv")) Then
					e.Appearance.BackColor = Color.LightGray
					e.Appearance.BackColor2 = Color.LightGray
				End If

			End If

		End Sub

		''' <summary>
		''' Deletes the selected responsible person.
		''' </summary>
		Private Sub DeleteSelectedResponsiblePerson()

			If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 233, m_InitializationData.MDData.MDNr) Then Exit Sub

			Dim selectedResponsiblePerson = SelectedResponsiblePersonViewData

			If selectedResponsiblePerson Is Nothing Then
				Return
			End If

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählte Person wirklich löschen?"), m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then

				Dim success = m_DataAccess.DeleteResponsiblePerson(selectedResponsiblePerson.ID, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die zuständige Person konnte nicht gelöscht werden."))
				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Achtung: Die Kontakteinträge der Zuständigen Person wurden auf Hauptkunde übernommen."))
				End If

				LoadResponsiblePersonData(m_CustomerNumber)
			End If
		End Sub


		''' <summary>
		''' Handles click on button new responsible person.
		''' </summary>
		Private Sub OnBtnAddResponsiblePerson_Click(sender As System.Object, e As System.EventArgs) Handles btnAddResponsiblePerson.Click
			If (IsCustomerDataLoaded) Then
				ShowResponsiblePersonForm(m_CustomerNumber, Nothing)
			End If
		End Sub

		''' <summary>
		''' Handles close of responsible person form.
		''' </summary>
		Private Sub OnResponsiblePersonFormClosed(sender As System.Object, e As System.EventArgs)
			LoadResponsiblePersonData(m_CustomerNumber)

			Dim responsiblePersonForm = CType(sender, frmResponsiblePerson)

			If Not responsiblePersonForm.SelectedResponsiblePersonOverViewData Is Nothing Then
				FocusResponsiblePerson(m_CustomerNumber, m_ResponsiblePersonsForm.SelectedResponsiblePersonOverViewData.RecordNumber)
			End If

		End Sub

		''' <summary>
		''' Handles save event of repsonbible person form.
		''' </summary>
		Private Sub OnResponsibleFormPersonDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)

			LoadResponsiblePersonData(m_CustomerNumber)

			Dim responsiblePersonForm = CType(sender, frmResponsiblePerson)

			If Not responsiblePersonForm.SelectedResponsiblePersonOverViewData Is Nothing Then
				FocusResponsiblePerson(m_CustomerNumber, responsiblePersonForm.SelectedResponsiblePersonOverViewData.RecordNumber)
			End If

		End Sub

		''' <summary>
		''' Handles delete event of repsonbible person form.
		''' </summary>
		Private Sub OnResponsibleFormPersonDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)

			LoadResponsiblePersonData(m_CustomerNumber)

		End Sub

		''' <summary>
		''' Focuses a responsible person.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The responsible person record number.</param>
		Private Sub FocusResponsiblePerson(ByVal customerNumber As Integer, ByVal recordNumber As Integer)

			Dim listDataSource As BindingList(Of ResponsiblePersonViewData) = grdZHD.DataSource

			Dim responsiblePersonViewData = listDataSource.Where(Function(data) data.CustomerNumber = customerNumber AndAlso data.RecordNumber = recordNumber).FirstOrDefault()

			If Not responsiblePersonViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(responsiblePersonViewData)
				Dim rowHandle = gvResponsiblePersons.GetRowHandle(sourceIndex)
				gvResponsiblePersons.FocusedRowHandle = rowHandle
			End If

		End Sub

		''' <summary>
		''' Shows the responsible person form.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
		Private Sub ShowResponsiblePersonForm(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?)

			If m_ResponsiblePersonsForm Is Nothing OrElse m_ResponsiblePersonsForm.IsDisposed Then

				If Not m_ResponsiblePersonsForm Is Nothing Then
					' First cleanup handlers of old form before new form is created.
					RemoveHandler m_ResponsiblePersonsForm.FormClosed, AddressOf OnResponsiblePersonFormClosed
					RemoveHandler m_ResponsiblePersonsForm.ResponsiblePersonDataSaved, AddressOf OnResponsibleFormPersonDataSaved
					RemoveHandler m_ResponsiblePersonsForm.ResponsiblePersonDataDeleted, AddressOf OnResponsibleFormPersonDataDeleted
				End If

				m_ResponsiblePersonsForm = New frmResponsiblePerson(m_InitializationData)
				AddHandler m_ResponsiblePersonsForm.FormClosed, AddressOf OnResponsiblePersonFormClosed
				AddHandler m_ResponsiblePersonsForm.ResponsiblePersonDataSaved, AddressOf OnResponsibleFormPersonDataSaved
				AddHandler m_ResponsiblePersonsForm.ResponsiblePersonDataDeleted, AddressOf OnResponsibleFormPersonDataDeleted
			End If

			m_ResponsiblePersonsForm.Show()
			m_ResponsiblePersonsForm.LoadResponsiblePersonData(customerNumber, responsiblePersonRecordNumber)

			m_ResponsiblePersonsForm.BringToFront()

		End Sub

		''' <summary>
		''' Validates the data on the tabs.
		''' </summary>
		Public Function ValidateData() As Boolean

			Dim valid As Boolean = True
			For Each tabControl In m_ListOfTabControls

				' Only validate tabs with the correct customer number.
				If tabControl.CustomerNumber = m_CustomerNumber Then
					valid = valid AndAlso tabControl.ValidateData()
				Else
					' Skip
				End If

			Next

			Return valid

		End Function

		''' <summary>
		''' Creates Navigationbar
		''' </summary>
		Private Sub CreateMyNavBar()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Me.navMain.Items.Clear()
			Dim labelPrinterName = GetUserDataMaxtrixPrintername
			bbiDatamatrix.Caption = String.Format("DataMatrix-Code: {0}", labelPrinterName)
			bbiDatamatrix.Enabled = Not String.IsNullOrWhiteSpace(labelPrinterName)
			Try
				navMain.PaintStyleName = "SkinExplorerBarView"

				' Create a Local group.
				Dim groupDatei As NavBarGroup = New NavBarGroup(("Datei"))
				groupDatei.Name = "gNavDatei"

				Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
				New_P.Name = "New_Customer"
				New_P.SmallImage = Me.ImageCollection1.Images(0)

				Dim Save_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
				Save_P_Data.Name = "Save_Customer_Data"
				Save_P_Data.SmallImage = Me.ImageCollection1.Images(1)
				Save_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 202, m_InitializationData.MDData.MDNr)

				Dim Print_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
				Print_P_Data.Name = "Print_Customer_Data"
				Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)
				Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 204, m_InitializationData.MDData.MDNr)

				Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
				Close_P_Data.Name = "Close_Customer_Form"
				Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

				Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
				groupDelete.Name = "gNavDelete"
				groupDelete.Appearance.ForeColor = Color.Red

				Dim Delete_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
				Delete_P_Data.Name = "Delete_Customer_Data"
				Delete_P_Data.SmallImage = Me.ImageCollection1.Images(4)
				Delete_P_Data.Appearance.ForeColor = Color.Red
				Delete_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 203, m_InitializationData.MDData.MDNr)

				Dim groupExtra As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
				groupExtra.Name = "gNavExtra"

				Dim TODO_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("To-do erstellen"))
				TODO_P_Data.Name = "CreateTODO"
				TODO_P_Data.SmallImage = Me.ImageCollection1.Images(8)

				Dim Property_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Abhängigkeiten anzeigen"))
				Property_P_Data.Name = "customer_properties"
				Property_P_Data.SmallImage = Me.ImageCollection1.Images(9)
				Property_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 217, m_InitializationData.MDData.MDNr)

				'm_Wos_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("WOS-Link senden"))
				'm_Wos_P_Data.Name = "wos_Customer_Data"
				'm_Wos_P_Data.SmallImage = Me.ImageCollection1.Images(5)

				Dim outlook_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten über Profilmatcher laden"))
				outlook_P_Data.Name = "pm_Customer_Data"
				outlook_P_Data.SmallImage = Me.ImageCollection1.Images(11)
				outlook_P_Data.Enabled = m_AllowedProfilMatcher

				Dim Solvency_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Bonität überprüfen"))
				Solvency_P_Data.Name = "solvency_Check"
				Solvency_P_Data.SmallImage = Me.ImageCollection1.Images(7)
				Solvency_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 234, m_InitializationData.MDData.MDNr)


				Try
					navMain.BeginUpdate()

					navMain.Groups.Add(groupDatei)
					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 202, m_InitializationData.MDData.MDNr) Then
						groupDatei.ItemLinks.Add(New_P)
						groupDatei.ItemLinks.Add(Save_P_Data)
					End If

					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 204, m_InitializationData.MDData.MDNr) Then groupDatei.ItemLinks.Add(Print_P_Data)
					groupDatei.ItemLinks.Add(Close_P_Data)
					groupDatei.Expanded = True

					navMain.Groups.Add(groupDelete)
					groupDelete.ItemLinks.Add(Delete_P_Data)
					groupDelete.Expanded = False

					navMain.Groups.Add(groupExtra)
					groupExtra.ItemLinks.Add(TODO_P_Data)
					groupExtra.ItemLinks.Add(Property_P_Data)

					'If m_mandant.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year) Then groupExtra.ItemLinks.Add(m_Wos_P_Data)

					'groupExtra.ItemLinks.Add(outlook_P_Data)
					groupExtra.ItemLinks.Add(Solvency_P_Data)
					groupExtra.Expanded = True

					navMain.EndUpdate()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))
			End Try

		End Sub

		''' <summary>
		''' Prepares status and navigation bar.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success</returns>
		''' <remarks></remarks>
		Private Function PrepareStatusAndNavigationBar(ByVal customerNumber As Integer)
			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_InitializationData.UserData.UserFiliale)

			If customerMasterData Is Nothing Then

				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundenstammdaten (Statuszeile/Navigation) konnten nicht geladen werden."))
				Return False
			End If

			bsiCreated.Caption = String.Format(" {0:f}, {1}", customerMasterData.CreatedOn, customerMasterData.CreatedFrom)
			bsiChanged.Caption = String.Format(" {0:f}, {1}", customerMasterData.ChangedOn, customerMasterData.ChangedFrom)

			'm_Wos_P_Data.Enabled = customerMasterData.WOSGuid <> String.Empty

			Return True
		End Function

		Private Sub OnbbiDataMatrix_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDatamatrix.ItemClick
			PrintCustomerDataMatrixCode()
		End Sub

		''' <summary>
		''' Build contextmenu for print.
		''' </summary>
		Private Sub GetMenuItems4Print()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim mnuData = m_DataAccess.LoadContextMenu4PrintData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))
				Exit Sub
			End If

			BarManager1.BeginUpdate()
			BarManager1.ForceInitialize()

			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu.Manager = Me.BarManager1
			popupMenu.Manager.Images = Me.ImageCollection1

			Dim itm As New DevExpress.XtraBars.BarButtonItem
			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem
				Dim strMnuBez As String = m_Translate.GetSafeTranslationValue(mnuData(i).MnuCaption)

				itm.Caption = strMnuBez
				itm.Name = mnuData(i).MnuName
				'Dim bAsGroup As Boolean = strMnuBez.StartsWith("-")



				If strMnuBez.StartsWith("_") OrElse strMnuBez.StartsWith("-") Then
					itm.Caption = m_Translate.GetSafeTranslationValue(strMnuBez.Remove(0, 1))
					popupMenu.AddItem(itm).BeginGroup = True
				Else
					itm.Caption = m_Translate.GetSafeTranslationValue(itm.Caption)
					popupMenu.AddItem(itm)
				End If




				'If bAsGroup Then
				'	popupMenu.AddItem(itm).BeginGroup = True
				'Else
				'	popupMenu.AddItem(itm)
				'End If

				AddHandler itm.ItemClick, AddressOf PrintDocs
			Next

			' fill templates
			Dim mnuTemplatesData = m_DataAccess.LoadContextMenu4PrintTemplatesData
			If Not (mnuTemplatesData Is Nothing) Then
				For i As Integer = 0 To mnuTemplatesData.Count - 1
					itm = New DevExpress.XtraBars.BarButtonItem

					'itm.Caption = m_Translate.GetSafeTranslationValue(mnuTemplatesData(i).MnuCaption)
					'itm.Name = String.Format("{0}|{1}", mnuTemplatesData(i).MnuDocPath, mnuTemplatesData(i).MnuDocMacro)

					'If i = 0 Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					Dim strMnuBez As String = m_Translate.GetSafeTranslationValue(mnuTemplatesData(i).MnuCaption)
					Dim bAsGroup As Boolean = strMnuBez.StartsWith("-") OrElse i = 0
					itm.Caption = strMnuBez.Replace("-", "")
					itm.Name = String.Format("{0}|{1}", mnuTemplatesData(i).MnuDocPath, mnuTemplatesData(i).MnuDocMacro)

					If bAsGroup Then
						popupMenu.AddItem(itm).BeginGroup = True
					Else
						popupMenu.AddItem(itm)
					End If

					AddHandler itm.ItemClick, AddressOf PrintDocs
				Next
			End If
			BarManager1.EndUpdate()

			' show contextmenu
			popupMenu.ShowPopup(New Point(Me.navMain.Width + Me.Left, Cursor.Position.Y))

		End Sub

		''' <summary>
		''' Prints documents.
		''' </summary>
		Private Sub PrintDocs(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim strMenuName As String() = e.Item.Name.Split("|")
			If strMenuName.Length = 2 Then
				' then office-templates...
				If Not strMenuName(0).Substring(1, 2) = ":\" And Not strMenuName(0).StartsWith("\\") Then
					strMenuName(0) = String.Format("{0}{1}", m_ClsProgSetting.GetMDTemplatePath, strMenuName(0))
				End If
				If File.Exists(strMenuName(0)) Then
					Dim fi As New FileInfo(strMenuName(0))
					Dim newFilename As String = String.Format("{0}{1}", m_ClsProgSetting.GetSpSFiles2DeletePath, fi.Name)
					Try
						File.Copy(strMenuName(0), newFilename, True)
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datei konnte nicht kopiert werden. {1}", strMethodeName, ex.ToString))
						newFilename = strMenuName(0)
					End Try
					Try
						Dim _clsBrowser As New ClassBrowserPath
						_clsBrowser.GetBrowserApplicationPath(strMenuName(0))
						Dim startInfo As New ProcessStartInfo

						Dim _reg As New SPProgUtility.ClsDivReg
						Dim selectedResponsiblePerson = SelectedResponsiblePersonViewData

						If selectedResponsiblePerson Is Nothing Then
							Return
						End If
						_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "KDZuNr", selectedResponsiblePerson.RecordNumber)
						_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "KDZHDNr", selectedResponsiblePerson.RecordNumber)
						_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "KDNr", m_CustomerNumber)
						_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MandantNumber", m_InitializationData.MDData.MDNr)

						startInfo.FileName = _clsBrowser.GetBrowserPath
						startInfo.Arguments = Chr(34) & newFilename & Chr(34) & If(strMenuName(1) <> String.Empty, " /m" & strMenuName(1), "")
						startInfo.UseShellExecute = False
						Process.Start(startInfo)

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datei konnte nicht geöffnet werden. {1}", strMethodeName, ex.Message))
						m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Datei konnte nicht geöffnet werden. {0}"), ex.Message))
					End Try
				End If

			Else
				Me.m_PrintJobNr = e.Item.Name
				PrintKDTemplate()

			End If

		End Sub

		''' <summary>
		'''  Starts printdialog with List Label 18.
		''' </summary>
		Private Sub PrintKDTemplate()

			If (Not IsCustomerDataLoaded) Then
				Return
			End If
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Dim strResult As String = "Success..."
			Dim _Setting As New ClsLLKDSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																										 .SQL2Open = String.Empty,
																																										 .JobNr2Print = Me.m_PrintJobNr,
																																										.ShowAsDesign = ShowDesign,
																																										.liKDNr2Print = New List(Of Integer)(New Integer() {Me.m_CustomerNumber})}
			Dim obj As New KDStammblatt.ClsPrintKDStammblatt(_Setting)
			strResult = obj.PrintKDStammBlatt()

		End Sub

		Private Sub PrintCustomerDataMatrixCode()

			If (Not IsCustomerDataLoaded) Then
				Return
			End If
			'If printLabel Then
			Dim printerName As String = GetUserDataMaxtrixPrintername()
			If String.IsNullOrWhiteSpace(printerName) Then
				Dim msg = "Achtung: Sie haben keinen Drucker definiert!"
				msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine)

				m_UtilityUI.ShowInfoDialog(msg)
			End If

			' DATAMATRIX_VALUE_PATTERN_REPORT As String = "^KD_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
			Dim dataMatrixCode As String = GetDataMaxtrixCodeString()
			SP.Infrastructure.BarcodeUtility.PrintBarcode(String.Format(dataMatrixCode, m_CustomerNumber), printerName)

			'End If

		End Sub

		''' <summary>
		''' Shows the new customer form.
		''' </summary>
		'''<param name="closeMainFormOnCancel">Boolean flag indicating if the main form should be closed on cancel click.</param>
		Private Sub ShowNewFormForCustomer(ByVal closeMainFormOnCancel As Boolean)

			Dim frmNewCustomer As New frmNewCustomer(m_InitializationData)
			frmNewCustomer.ShowDialog()

			If (frmNewCustomer.DialogResult = DialogResult.OK) Then

				' Checks if customer has been added.
				If (frmNewCustomer.NewlyAddedCustomerNumber.HasValue) Then

					m_CustomerNumber = frmNewCustomer.NewlyAddedCustomerNumber

					m_ActiveUserControl.Activate(frmNewCustomer.NewlyAddedCustomerNumber)

					LoadResponsiblePersonData(frmNewCustomer.NewlyAddedCustomerNumber)
					PrepareStatusAndNavigationBar(frmNewCustomer.NewlyAddedCustomerNumber)
				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kunde konnte nicht hinzugefügt werden."))
				End If

			ElseIf closeMainFormOnCancel Then
				CleanUp()
				CleanupAndHideForm()
			End If

		End Sub

		Private Function DeleteSelectedCustomer() As DeleteCustomerAddressAssignmentResult
			Dim result = DeleteCustomerAddressAssignmentResult.Deleted

			If (Not IsCustomerDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))
				Return Nothing
			End If
			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																													m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return DeleteCustomerAddressAssignmentResult.ErrorWhileDelete
			End If

			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(m_CustomerNumber, m_InitializationData.UserData.UserFiliale)

			result = m_DataAccess.DeleteCustomerAddressAssignment(customerMasterData.CustomerNumber,
																																				 ConstantValues.ModulName, String.Format("{0}, {1}",
																																																								 m_InitializationData.UserData.UserLName, m_InitializationData.UserData.UserFName),
																																																							 m_InitializationData.UserData.UserNr)

			If (IsCustomerDataLoaded) Then

				Select Case result
					Case DeleteCustomerAddressAssignmentResult.CouldNotDeleteBecauseOfExistingVac
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde hat Vakanzen. Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerAddressAssignmentResult.CouldNotDeleteBecauseOfExistingPropose
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde hat Vorschläge. Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerAddressAssignmentResult.CouldNotDeleteBecauseOfExistingES
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde hat Einsätze. Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerAddressAssignmentResult.CouldNotDeleteBecauseOfExistingRP
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde hat Rapporte. Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerAddressAssignmentResult.CouldNotDeleteBecauseOfExistingRE
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde hat Rechnungen. Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerAddressAssignmentResult.CouldNotDeleteBecauseOfExistingZE
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde hat Zahlungseingänge. Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerAddressAssignmentResult.ErrorWhileDelete
						m_UtilityUI.ShowErrorDialog("Die Daten konnten nicht gelöscht werden.")

					Case DeleteCustomerAddressAssignmentResult.Deleted
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Kunde wurde erfolgreich gelöscht."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)

				End Select
			End If


			Return result
		End Function

		''' <summary>
		''' shows customerproperties
		''' </summary>
		''' <remarks></remarks>
		Private Sub ShowCustomerProperties()

			If (Not IsCustomerDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Try
				If m_PropertyForm Is Nothing OrElse m_PropertyForm.IsDisposed Then
					m_PropertyForm = New frmCustomersProperties(m_InitializationData, m_Translate, m_CustomerNumber)
				End If
				m_PropertyForm.LoadData(m_CustomerNumber, "show_vacancies")
				m_PropertyForm.Show()
				m_PropertyForm.BringToFront()

			Catch e As Exception
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", e.ToString))

			End Try

		End Sub

		''' <summary>
		''' save customerdata to outlookcontact (Kontakt\Sputnik Kunden) folder
		''' </summary>
		''' <remarks></remarks>
		Private Sub LoadProfilmatcherDataForAssignedCustomer()
			Dim _setting = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, m_InitializationData.MDData, m_InitializationData.UserData)

			Try
				' Read data over webservice
				Dim frm = New SP.Internal.Automations.SPProfilMatcher.frmProfilmatcher(m_InitializationData)
				frm.PreselectionData = New SP.Internal.Automations.SPProfilMatcher.PreselectionData With {.MDNr = m_InitializationData.MDData.MDNr, .CustomerNumber = m_CustomerNumber}

				frm.PreselectData()
				frm.Show()
				frm.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		'''' <summary>
		'''' sends automated mail to customer admin email-address
		'''' </summary>
		'''' <remarks></remarks>
		'Private Sub SendWOSLink()

		'	If m_CustomerNumber Is Nothing Then Return
		'	'Dim msg = m_Translate.GetSafeTranslationValue("Ihr Kunde wird automatisch über die neuen Dokumenten-Uploads informiert. Möchten Sie trotzdem eine Benachrichtigung senden?")
		'	'If Not m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Online Dokumente")) Then Return


		'	'Dim notifyMail = New SPSSendMail.RichEditSendMail.SendWOSMailNotification(m_InitializationData)

		'	'Dim preselectionSetting As New SPSSendMail.RichEditSendMail.PreselectionMailData With {.MailType = SPSSendMail.RichEditSendMail.PreselectionMailData.MailTypeEnum.CustomerWOS, .CustomerNumber = m_CustomerNumber}
		'	'notifyMail.PreselectionData = preselectionSetting

		'	'Dim result = notifyMail.SendCustomerWOSNotification()

		'	'If result.Value Then
		'	'	m_UtilityUI.ShowInfoDialog("Die Nachricht mit Dokumentenlink wurde erfolgreich versendet.")
		'	'Else
		'	'	msg = m_Translate.GetSafeTranslationValue("Die Nachricht mit Dokumentenlink wurde nicht versendet.{0}{1}")
		'	'	m_UtilityUI.ShowErrorDialog(String.Format(msg, vbNewLine, result.ValueLable))
		'	'End If

		'End Sub

#Region "Solvency Check"


		''' <summary>
		''' Creates the solvency check option dialog.
		''' </summary>
		''' <returns>A task dialog info object.</returns>
		Private Function CreateSolvencyCheckOptionDialog() As TaskDialogInfo
			Dim btnQuickCheck As New DevComponents.DotNetBar.Command
			Dim strMsg As String = "Der gewünschte Dienst ist kostenpflichtig. Möchten Sie Quick-Check oder Business-Check ausführen?"
			Dim strFooter As String = "{0}{1}Letzte Überprüfung: {2}{3}"
			Dim btnBusinessCheck As New DevComponents.DotNetBar.Command

			strFooter = String.Format(m_Translate.GetSafeTranslationValue(strFooter), vbLf,
														 "<strong><font color=""#FF0000"">",
														 ucCommonData.btnSolvencyDecision.ToolTip,
														 "</font></strong>")
			If Not ucCommonData.btnSolvencyDecision.Visible Then strFooter = String.Empty

			AddHandler btnQuickCheck.Executed, Sub() TaskDialog.Close(eTaskDialogResult.Custom1)
			AddHandler btnBusinessCheck.Executed, Sub() TaskDialog.Close(eTaskDialogResult.Custom2)

			btnQuickCheck.Text = "Ich möchte <b>Quick-Check</b> ausführen. (7.- sFr.)"
			btnBusinessCheck.Text = "Ich möchte <b>Business-Check</b> ausführen. (24.- sFr.)"
			btnQuickCheck.Image = My.Resources.QuickCheck
			btnBusinessCheck.Image = My.Resources.BusinessCheck

			btnQuickCheck.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 234, m_InitializationData.MDData.MDNr)
			btnBusinessCheck.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 235, m_InitializationData.MDData.MDNr)

			Dim info As New TaskDialogInfo(m_Translate.GetSafeTranslationValue("Bonität überprüfen"),
															 CType(System.Enum.Parse(GetType(eTaskDialogIcon), "ShieldHelp"), eTaskDialogIcon),
																									 m_Translate.GetSafeTranslationValue("Bonitätsprüfung"),
																									 m_Translate.GetSafeTranslationValue(strMsg),
																									 eTaskDialogButton.Close,
																									 CType(System.Enum.Parse(GetType(eTaskDialogBackgroundColor),
																																					"Default"), eTaskDialogBackgroundColor),
																																			 Nothing,
																																			 New Command() {btnQuickCheck, btnBusinessCheck}, Nothing,
																																			strFooter, Nothing)
			Return info
		End Function

		''' <summary>
		''' Runs a quick business check.
		''' </summary>
		Private Sub RunQuickBusinessCheck()

			' TODO: Email senden oder Webservice aufrufen
			Dim strMD_Guid As String = m_ClsProgSetting.GetSelectedMDData(0)
			Dim strMD_Name As String = m_ClsProgSetting.GetSelectedMDData(1)
			Dim strUser_Name As String = m_ClsProgSetting.GetUserName
			Dim strCheck As String = "Quick-Check"
			Dim eMail As String = "info@domain.com"
			Dim smtp As String = "mail.domain.com"

			Try
				PerformSolvencCheck(CompanyReportType.QUICK_CHECK_BUSINESS)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Solvenzprüfung ist fehlgeschlagen."))
			End Try

		End Sub

		''' <summary>
		''' Runs a business check.
		''' </summary>
		Private Sub RunBusinessCheck()
			' TODO: Email senden oder Webservice aufrufen

			Try
				PerformSolvencCheck(CompanyReportType.CREDIT_CHECK_BUSINESS)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Solvenzprüfung ist fehlgeschlagen."))
			End Try
		End Sub

		''' <summary>
		''' Performs a solvency check.
		''' </summary>
		''' <param name="companyReportType">The report type.</param>
		Private Sub PerformSolvencCheck(ByVal companyReportType As CompanyReportType)

			Dim customerData As New CustomerMasterData()

			' Load current visible customerd data from customer panel.
			customerData.CustomerNumber = m_CustomerNumber
			ucCommonData.Activate(m_CustomerNumber)	' Make sure common data is loaded.
			ucCommonData.MergeCustomerMasterData(customerData)

			' At least there should be the company name present.
			If String.IsNullOrEmpty(customerData.Company1) Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Bitte geben Sie die Firmenbezeichnung ein."))
				Return
			End If

			' --- Create search address ---

			Dim companyAddress As New CompanyAddress()

			' Street and house number
			If Not String.IsNullOrEmpty(customerData.Street) Then

				Dim seperatedStreetAndHousenumber As Tuple(Of String, String) = m_Utility.SimpleSplitStreetAndHouseNumber(customerData.Street)

				If Not seperatedStreetAndHousenumber Is Nothing Then

					companyAddress.Street = seperatedStreetAndHousenumber.Item1
					companyAddress.HouseNumber = seperatedStreetAndHousenumber.Item2
				Else
					' Split did not work then use the street value of the custmer data.
					companyAddress.Street = customerData.Street
					companyAddress.HouseNumber = String.Empty
				End If
			End If

			companyAddress.CompanyName = customerData.Company1
			companyAddress.Zip = customerData.Postcode
			companyAddress.City = customerData.Location
			companyAddress.CountryCode = customerData.CountryCode

			' Telephone
			If Not String.IsNullOrEmpty(customerData.Telephone) Then
				companyAddress.AddContItem(ContactType.PHONE, customerData.Telephone)
			End If

			' Telefax
			If Not String.IsNullOrEmpty(customerData.Telefax) Then
				companyAddress.AddContItem(ContactType.FAX, customerData.Telefax)
			End If

			' Email
			If Not String.IsNullOrEmpty(customerData.EMail) Then
				companyAddress.AddContItem(ContactType.EMAIL, customerData.EMail)
			End If

			' Homepage
			If Not String.IsNullOrEmpty(customerData.Hompage) Then
				companyAddress.AddContItem(ContactType.WEB, customerData.Hompage)
			End If
			m_Logger.LogDebug(String.Format("m_DeltavistaWSReferenceNumber: {0} | CompanyName: {1} | Zip: {2} | City: {3} | CountryCode: {4} | Telefon: {5} | Telefax: {6} | EMail: {7}",
																			m_DeltavistaWSReferenceNumber, companyAddress.CompanyName, companyAddress.Zip, companyAddress.City, companyAddress.CountryCode, customerData.Telephone, customerData.Telefax, customerData.EMail))

			' --- Perform check ---
			Dim companyRequest As New SolvencyReportCompanyRequestData(m_DeltavistaWSReferenceNumber, companyAddress, companyReportType, DeltavistaWebService.TargetReportFormat.PDF)
			Dim frmPerformSolvencyCheck As New frmPerformSolvencyCheck(m_CustomerNumber, m_DeltavistaWSUserName, m_DeltavistaWSPassword, m_DeltavistaWSServiceUrl, companyRequest, m_InitializationData)
			frmPerformSolvencyCheck.StartPosition = FormStartPosition.CenterParent
			frmPerformSolvencyCheck.ShowDialog()
			Dim responseData = frmPerformSolvencyCheck.ResponseData

			HandleSolvencyCheckResponse(responseData, companyReportType)

		End Sub

		''' <summary>
		''' Handle the solvency check response.
		''' </summary>
		''' <param name="responseData">The response data.</param>
		''' <param name="companyReportType">The company report type.</param>
		Private Sub HandleSolvencyCheckResponse(ByVal responseData As TypeGetReportResponse, ByVal companyReportType As CompanyReportType)

			If responseData Is Nothing OrElse
					responseData.addressMatchResult Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Prüfung war nicht erfolgreich."))
				Return
			End If

			Select Case responseData.addressMatchResult.addressMatchResultType
				Case AddressMatchResultType.MATCH
					' There was an exact match -> save the result to the database.

					Dim creditInfo As CustomerAssignedCreditInfo = SaveSolvencyCheckResultsToDatabase(responseData, companyReportType)

					If Not creditInfo Is Nothing Then

						' Update solvency check symbol on common data page.
						' Common data of customer should already loaded because this is done before solvency check.
						ucCommonData.UpdateSolvencyDecision(creditInfo.DV_DecisionID)

						' Reload credit info on additional info tab

						If ucAdditionalInfo.CustomerNumber = m_CustomerNumber Then
							' Reload credit infos if ucAdditional data has already ben loaded with current customers data.
							ucAdditionalInfo.ReloadCreditInfos()
						End If

						' Show result form.
						Dim frmSolvencyCheckResult = New frmSolvencyResult(creditInfo.CustomerNumber, creditInfo.RecordNumber, m_DeltavistaWSReferenceNumber,
																															 m_DeltavistaWSUserName, m_DeltavistaWSPassword, m_DeltavistaWSServiceUrl, m_InitializationData)
						frmSolvencyCheckResult.StartPosition = FormStartPosition.CenterParent
						frmSolvencyCheckResult.ShowDialog()
					Else
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Prüfungsergebnis konnte nicht gespeichert werden."))
					End If
				Case AddressMatchResultType.CANDIDATES
					' There was not an exact match of the address -> let the user choose the correct address.

					' Show address choose dialog.
					Dim frmChooseAddress = New frmCompanyAddressSelectionSolvencyCheck(responseData.addressMatchResult.candidates, m_Translate)
					frmChooseAddress.StartPosition = FormStartPosition.CenterParent
					frmChooseAddress.ShowDialog()

					' Check if user made a selection
					If Not frmChooseAddress.ChoosedCompanyAddressIdentifier Is Nothing Then

						' Now the search will by done by the address identifier
						Dim identifier As Identifier = frmChooseAddress.ChoosedCompanyAddressIdentifier
						Dim companyAddress As New CompanyAddress() With {.AddressIdentifier = identifier}
						Dim companyRequest As New SolvencyReportCompanyRequestData(m_DeltavistaWSReferenceNumber, companyAddress, companyReportType, DeltavistaWebService.TargetReportFormat.PDF)
						Dim frmPerformSolvencyCheck As New frmPerformSolvencyCheck(m_CustomerNumber, m_DeltavistaWSUserName, m_DeltavistaWSPassword, m_DeltavistaWSServiceUrl, companyRequest, m_InitializationData)

						frmPerformSolvencyCheck.StartPosition = FormStartPosition.CenterParent
						frmPerformSolvencyCheck.ShowDialog()
						responseData = frmPerformSolvencyCheck.ResponseData

						' Handle the response 
						HandleSolvencyCheckResponse(responseData, companyReportType)
					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Prüfung wurde abgebrochen."))
					End If
				Case AddressMatchResultType.NO_MATCH
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Der Prüfungsdienst konnte die Addresse nicht finden. Bitte prüfen Sie die Adressdaten und versuchen Sie es dann erneut."))
			End Select

		End Sub

		''' <summary>
		''' Save the solvency check result to the database.
		''' </summary>
		''' <param name="responeData">The respone data.</param>
		''' <param name="companyReportType">Then company report type.</param>
		''' <returns>The saved customer credit info data.</returns>
		Private Function SaveSolvencyCheckResultsToDatabase(ByVal responeData As TypeGetReportResponse, ByVal companyReportType As CompanyReportType) As CustomerAssignedCreditInfo

			Dim customerCreditInfo As CustomerAssignedCreditInfo = Nothing

			If Not responeData Is Nothing Then

				Try
					Dim now = DateTime.Now

					customerCreditInfo = New CustomerAssignedCreditInfo
					customerCreditInfo.CustomerNumber = m_CustomerNumber
					customerCreditInfo.FromDate = now
					customerCreditInfo.Description = responeData.decisionMatrix.decisionText
					customerCreditInfo.ActiveRec = False
					customerCreditInfo.ToDate = Nothing
					customerCreditInfo.CreatedOn = now
					customerCreditInfo.CreatedFrom = m_ClsProgSetting.GetUserName
					customerCreditInfo.ChangedOn = now
					customerCreditInfo.ChangedFrom = m_ClsProgSetting.GetUserName
					customerCreditInfo.DV_ArchiveID = responeData.archivingId

					' the address identifier
					Dim addressIdentifer = (From identifier In responeData.addressMatchResult.foundAddress.identifiers Where identifier.identifierType = IdentifierType.ADDRESS_ID).FirstOrDefault()

					If Not addressIdentifer Is Nothing Then
						customerCreditInfo.DV_FoundedAddressID = addressIdentifer.identifierText
					Else
						customerCreditInfo.DV_FoundedAddressID = Nothing
						m_Logger.LogWarning(String.Format("Address identifier was not present in identifier collection. CustomerNumber (KDNr) = {0}", m_CustomerNumber))
					End If

					Dim decisioinResult As DecisionResult
					Select Case responeData.decisionMatrix.decision
						Case Decision.LIGHT_GREEN
							decisioinResult = DecisionResult.LightGreen
						Case Decision.GREEN
							decisioinResult = DecisionResult.Green
						Case Decision.YELLOW_GREEN
							decisioinResult = DecisionResult.YellowGreen
						Case Decision.YELLOW
							decisioinResult = DecisionResult.Yellow
						Case Decision.ORANGE
							decisioinResult = DecisionResult.Orange
						Case Decision.RED
							decisioinResult = DecisionResult.Red
						Case Decision.DARK_RED
							decisioinResult = DecisionResult.DarkRed
						Case Else
							Throw New Exception("Decision mapping not definded.")
					End Select

					customerCreditInfo.DV_DecisionID = CType(decisioinResult, Integer)

					customerCreditInfo.DV_DecisionText = responeData.decisionMatrix.decisionText
					customerCreditInfo.USNr = m_InitializationData.UserData.UserNr

					' The found addrss
					Dim fAdr = CType(responeData.addressMatchResult.foundAddress.address, CompanyAddressDescription)
					Dim foundedAddressBuffer As New StringBuilder
					foundedAddressBuffer.AppendLine(fAdr.companyName.Trim())
					foundedAddressBuffer.AppendLine((fAdr.location.street & " " & fAdr.location.houseNumber).Trim())
					foundedAddressBuffer.AppendLine((fAdr.location.country & "-" & fAdr.location.zip & ", " & fAdr.location.city).Trim())
					customerCreditInfo.DV_FoundedAddress = foundedAddressBuffer.ToString().Trim()

					' Type of check
					Select Case companyReportType
						Case SPS.ExternalServices.CompanyReportType.QUICK_CHECK_BUSINESS
							customerCreditInfo.DV_QueryType = CType(BusinessSolvencyCheckType.QuickBusinessCheck, Integer)
						Case SPS.ExternalServices.CompanyReportType.CREDIT_CHECK_BUSINESS
							customerCreditInfo.DV_QueryType = CType(BusinessSolvencyCheckType.BusinessCheck, Integer)
						Case Else
							Throw New Exception("Report type mapping not definded.")
					End Select

					' PDF bytes
					Dim pdfBytes As Byte() = Nothing
					pdfBytes = Convert.FromBase64String(responeData.report)
					customerCreditInfo.DV_PDFFile = pdfBytes

					Dim succes = m_DataAccess.AddCustomerCreditInfoAssignment(customerCreditInfo)

					If Not succes Then
						customerCreditInfo = Nothing
					End If

				Catch ex As Exception
					m_Logger.LogError(ex.ToString())
					customerCreditInfo = Nothing
				End Try

			Else
				customerCreditInfo = Nothing
			End If

			Return customerCreditInfo
		End Function

		''' <summary>
		''' Load forms settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
				Dim setting_form_sccMainPos = m_SettingsManager.ReadInteger(SettingKeys.SETTING_SCC_MAINPOS)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
				If setting_form_location <> String.Empty Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If
				If setting_form_sccMainPos > 0 Then Me.sccMain.SplitterPosition = Math.Max(setting_form_sccMainPos, 10)

			Catch ex As Exception
				m_Logger.LogError(String.Format("LoadFormSettings.Formsizing.{0}{1}", vbNewLine, ex.Message))

			End Try

		End Sub

		''' <summary>
		''' Saves the form settings.
		''' </summary>
		Private Sub SaveFormSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_SCC_MAINPOS, Me.sccMain.SplitterPosition)
					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub


#End Region


#Region "GridSettings"

		Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
			Dim keepFilter = False
			Dim restoreLayout = True

			Select Case GridName.ToLower
				Case gvResponsiblePersons.Name.ToLower
					Try
						keepFilter = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingResponsiblepersonCustomerFilter), False)
						restoreLayout = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreResponsiblepersonCustomerSetting), True)
					Catch ex As Exception

					End Try

					If restoreLayout AndAlso File.Exists(m_GVResponsiblepersonCustomerSettingfilename) Then gvResponsiblePersons.RestoreLayoutFromXml(m_GVResponsiblepersonCustomerSettingfilename)
					If restoreLayout AndAlso Not keepFilter Then gvResponsiblePersons.ActiveFilterCriteria = Nothing


				Case Else

					Exit Sub


			End Select


		End Sub

		Private Sub OngvResponsiblepersonCustomerColumnPositionChanged(sender As Object, e As System.EventArgs)

			gvResponsiblePersons.SaveLayoutToXml(m_GVResponsiblepersonCustomerSettingfilename)

		End Sub



#End Region


		''' <summary>
		''' CleanUp the form
		''' </summary>
		Private Sub CleanUp()

			If Not m_PropertyForm Is Nothing AndAlso Not m_PropertyForm.IsDisposed Then

				Try
					m_PropertyForm.Close()
					m_PropertyForm.Dispose()
				Catch
					' Do nothing
				End Try
			End If

			' Release responsible person form
			If Not m_ResponsiblePersonsForm Is Nothing AndAlso
					Not m_ResponsiblePersonsForm.IsDisposed Then

				Try
					m_ResponsiblePersonsForm.Close()
					m_ResponsiblePersonsForm.Dispose()
				Catch
					' Do nothing
				End Try

			End If

			' Cleanup all the child controls
			For Each tabControl In m_ListOfTabControls
				tabControl.CleanUp()
			Next

		End Sub

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFormSettings()

			CleanUp()

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub

#End Region


#Region "View helper classes"

		''' <summary>
		''' Responsible person view data.
		''' </summary>
		Private Class ResponsiblePersonViewData

			' ID
			Public Property ID As Integer

			' KDNr
			Public Property CustomerNumber As Integer

			' KDZNr
			Public Property RecordNumber As Integer

			'Position
			Public Property Position As String
			Public Property Department As String

			'Abteilung
			Public Property TrnslatedSalutation As String

			'Firstname_Lastname
			Public Property Firstname_Lastname As String

			'Vorname
			Public Property Telephone As String

			'Nachname 
			Public Property Telefax As String

			' Natel (Handy)
			Public Property MobilePhone As String

			' Email
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
					'AndAlso Not String.IsNullOrWhiteSpace(state1 & state2)
					Return isZActiv
				End Get
			End Property


		End Class


#End Region


	End Class

End Namespace
