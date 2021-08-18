
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports System.Net.Mail
Imports DevExpress.XtraEditors
Imports SP.KD.ReAdresse.UI
Imports DevExpress.XtraPivotGrid
Imports DevExpress.XtraGrid.Views.Grid

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports System.IO
Imports SP.Infrastructure


Namespace UI

	''' <summary>
	''' Account and sales data.
	''' </summary>
	Public Class ucAccountAndSales


#Region "Private Consts"

		Private Const USER_XML_SETTING_SPUTNIK_OPENINVOICE_CUSTOMER_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/customer/openinvoices/restorelayoutfromxml"
		Private Const USER_XML_SETTING_SPUTNIK_OPENINVOICE_CUSTOMER_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/customer/openinvoices/keepfilter"

#End Region

#Region "Private Fields"
		''' <summary>
		''' Check edit for active symbol.
		''' </summary>
		Private m_CheckEditActive As RepositoryItemCheckEdit

		''' <summary>
		''' The inovice address form.
		''' </summary>
		Private m_InvoiceAddressesForm As frmInvoiceAddress

		Private m_IsAuthorizedForCode_207 As Boolean = False
		Private m_IsAuthorizedForCode_208 As Boolean = False
		Private m_IsAuthorizedForCode_215 As Boolean = False


		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility
		Private m_Mandant As Mandant
		Private Property GridSettingPath As String
		Private UserGridSettingsXml As SettingsXml

		Private m_GVOpenInvoiceCustomerSettingfilename As String

		Private m_xmlSettingOpenInvoiceCustomerFilter As String
		Private m_xmlSettingRestoreOpenInvoiceCustomerSetting As String


#End Region



#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_Mandant = New Mandant
			m_Utility = New Utility

			lueCurrency.Properties.ShowHeader = False
			lueCurrency.Properties.ShowFooter = False
			lueCurrency.Properties.DropDownRows = 10

			lueInvoiceOptions.Properties.ShowHeader = False
			lueInvoiceOptions.Properties.ShowFooter = False
			lueInvoiceOptions.Properties.DropDownRows = 10

			lueOPShipment.Properties.ShowHeader = False
			lueOPShipment.Properties.ShowFooter = False
			lueOPShipment.Properties.DropDownRows = 10

			lueTermsAndConditions.Properties.ShowHeader = False
			lueTermsAndConditions.Properties.ShowFooter = False
			lueTermsAndConditions.Properties.DropDownRows = 10

			gvOpenDebitorInvoices.OptionsView.ShowIndicator = False
			gvInvoiceAddresses.OptionsView.ShowIndicator = False

			' Important symbol.
			m_CheckEditActive = CType(gridInvoiceAddresses.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditActive.PictureChecked = My.Resources.Completed
			m_CheckEditActive.PictureUnchecked = Nothing
			m_CheckEditActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			AddHandler lueOPShipment.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueTermsAndConditions.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCurrency.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueInvoiceType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueInvoiceOptions.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler gvOpenDebitorInvoices.ColumnFilterChanged, AddressOf OngvOpenInvoiceCustomerColumnPositionChanged
			AddHandler gvOpenDebitorInvoices.ColumnPositionChanged, AddressOf OngvOpenInvoiceCustomerColumnPositionChanged
			AddHandler gvOpenDebitorInvoices.ColumnWidthChanged, AddressOf OngvOpenInvoiceCustomerColumnPositionChanged

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)

			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			UserGridSettingsXml = New SettingsXml(m_Mandant.GetAllUserGridSettingXMLFilename(m_InitializationData.MDData.MDNr))

			LoadUserRights()
		End Sub

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function Activate(ByVal customerNumber As Integer?) As Boolean

			Dim success As Boolean = True

			If (Not IsIntialControlDataLoaded) Then
				success = success AndAlso LoadDropDownData()
				IsIntialControlDataLoaded = True
			End If

			If (customerNumber.HasValue) Then
				If (Not IsCustomerDataLoaded) Then
					success = success AndAlso LoadCustomerData(customerNumber)
				ElseIf Not customerNumber = m_CustomerNumber Then
					CleanUp()
					success = success AndAlso LoadCustomerData(customerNumber)
				End If
			Else
				CleanUp()
				Reset()
			End If

			Return success
		End Function

		''' <summary>
		''' Deactivates the control.
		''' </summary>
		Public Overrides Sub Deactivate()
			' Do nothing
		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_CustomerNumber = Nothing

			Try
				Dim mSettingpath = String.Format("{0}Customer\", m_Mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
				If Not Directory.Exists(mSettingpath) Then Directory.CreateDirectory(mSettingpath)

				m_GVOpenInvoiceCustomerSettingfilename = String.Format("{0}{1}{2}.xml", mSettingpath, gvOpenDebitorInvoices.Name, m_InitializationData.UserData.UserNr)

				m_xmlSettingRestoreOpenInvoiceCustomerSetting = String.Format(USER_XML_SETTING_SPUTNIK_OPENINVOICE_CUSTOMER_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr)
				m_xmlSettingOpenInvoiceCustomerFilter = String.Format(USER_XML_SETTING_SPUTNIK_OPENINVOICE_CUSTOMER_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr)

			Catch ex As Exception

			End Try


			' Resets textboxes, etc.

			spinNumberOfCopies.Value = 1
			chkMwSt.Checked = True
			chkHoursInNormalTime.Checked = False
			spinNumberOfCopies.Properties.MinValue = 0
			spinNumberOfCopies.Properties.MaxValue = 9

			txtValueAddedTaxNumber.Text = String.Empty
			txtValueAddedTaxNumber.Properties.MaxLength = 50

			txtAdminEmail.Text = String.Empty
			txtAdminEmail.Properties.MaxLength = 255
			lstAdminEmails.DataSource = Nothing
			chkPublicizeForWOS.Checked = True

			' Reset drop downs and grids

			ResetCountrOPShipmentDropDown()
			ResetTermsAndConditionsDropDown()
			ResetCurrencyDropDown()
			ResetInvoiceTypeDropDown()
			ResetInvoiceOptionDropDown()

			ResetInvoiceAddressGrid()
			ResetOpenDepitorPostGrid()
			ResetSalesVolumePivotGrid()

			grpumsatzzahlen.Visible = m_IsAuthorizedForCode_208
			chkMwSt.Enabled = m_IsAuthorizedForCode_215
			lueInvoiceType.Enabled = m_IsAuthorizedForCode_207

			' is wos allowed in mandant
			Dim bAllowedWOS As Boolean = m_Mandant.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year)

			lblagbwos.Visible = bAllowedWOS
			lueTermsAndConditions.Visible = bAllowedWOS

			lbladminemail.Visible = bAllowedWOS
			txtAdminEmail.Visible = bAllowedWOS
			lstAdminEmails.Visible = bAllowedWOS
			chkPublicizeForWOS.Visible = bAllowedWOS
			chkDoNotShowContractInWOS.Visible = bAllowedWOS

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			' Do nothing
			Return True
		End Function

		''' <summary>
		''' Merges the custmer master data.
		''' </summary>
		''' <param name="customerMasterData">The customer master data object where the data gets filled into.</param>
		Public Overrides Sub MergeCustomerMasterData(ByVal customerMasterData As CustomerMasterData)
			If (IsCustomerDataLoaded AndAlso
					m_CustomerNumber = customerMasterData.CustomerNumber) Then

				' Fill values

				customerMasterData.NumberOfCopies = spinNumberOfCopies.Value
				customerMasterData.mwstpflicht = chkMwSt.Checked
				customerMasterData.ShowHoursInNormal = chkHoursInNormalTime.Checked
				customerMasterData.OPShipment = lueOPShipment.EditValue
				customerMasterData.ValueAddedTaxNumber = txtValueAddedTaxNumber.Text
				customerMasterData.TermsAndConditions_WOS = lueTermsAndConditions.EditValue
				customerMasterData.sendToWOS = chkPublicizeForWOS.Checked AndAlso (lstAdminEmails.ItemCount > 0)
				customerMasterData.DoNotShowContractInWOS = chkDoNotShowContractInWOS.Checked
				customerMasterData.CurrencyCode = lueCurrency.EditValue
				customerMasterData.BillTypeCode = lueInvoiceType.EditValue
				customerMasterData.InvoiceOption = If(lueInvoiceOptions.EditValue Is Nothing, "", lueInvoiceOptions.EditValue)
			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()

			If Not m_InvoiceAddressesForm Is Nothing AndAlso
					Not m_InvoiceAddressesForm.IsDisposed Then

				Try
					m_InvoiceAddressesForm.Close()
					m_InvoiceAddressesForm.Dispose()
				Catch
					' Do nothing
				End Try

			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpoffenedebitoren.Text = m_Translate.GetSafeTranslationValue(Me.grpoffenedebitoren.Text)

			Me.grpumsatzzahlen.Text = m_Translate.GetSafeTranslationValue(Me.grpumsatzzahlen.Text)

			Me.chkMwSt.Text = m_Translate.GetSafeTranslationValue(Me.chkMwSt.Text)
			Me.chkHoursInNormalTime.Text = m_Translate.GetSafeTranslationValue(Me.chkHoursInNormalTime.Text)

			Me.grpfakturierung.Text = m_Translate.GetSafeTranslationValue(Me.grpfakturierung.Text)
			Me.lblanzahlkopien.Text = m_Translate.GetSafeTranslationValue(Me.lblanzahlkopien.Text)
			Me.lblwaehrung.Text = m_Translate.GetSafeTranslationValue(Me.lblwaehrung.Text)
			Me.lblfakturaart.Text = m_Translate.GetSafeTranslationValue(Me.lblfakturaart.Text)
			Me.lblerweitert.Text = m_Translate.GetSafeTranslationValue(Me.lblerweitert.Text)
			Me.lblmitrechnungsenden.Text = m_Translate.GetSafeTranslationValue(Me.lblmitrechnungsenden.Text)
			Me.lblmwstnr.Text = m_Translate.GetSafeTranslationValue(Me.lblmwstnr.Text)

			Me.lblagbwos.Text = m_Translate.GetSafeTranslationValue(Me.lblagbwos.Text)
			Me.lbladminemail.Text = m_Translate.GetSafeTranslationValue(Me.lbladminemail.Text)
			Me.chkPublicizeForWOS.Text = m_Translate.GetSafeTranslationValue(Me.chkPublicizeForWOS.Text)
			Me.chkDoNotShowContractInWOS.Text = m_Translate.GetSafeTranslationValue(Me.chkDoNotShowContractInWOS.Text)

		End Sub


		''' <summary>
		''' Loads customer data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		'''<returns>Boolean flag indicating succcess.</returns>
		Private Function LoadCustomerData(ByVal customerNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadCustomerMasterData(customerNumber)
			success = success AndAlso LoadAssignedCustomerInvoiceAddressData(customerNumber)
			success = success AndAlso LoadAssignedCustomerOpenDebitorInvoices(customerNumber)
			success = success AndAlso LoadAssignedCustomerEmailData(customerNumber)
			success = success AndAlso LoadSalesVolumneData(customerNumber)

			m_CustomerNumber = IIf(success, customerNumber, Nothing)

			Return success
		End Function

		''' <summary>
		''' Loads user rights.
		''' </summary>
		Private Sub LoadUserRights()
			m_IsAuthorizedForCode_207 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 207, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_208 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 208, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_215 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 215, m_InitializationData.MDData.MDNr)
		End Sub

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadDropDownData() As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadOPShipmentDropDownData()
			success = success AndAlso LoadTermsAndConditionsDropDownData()
			success = success AndAlso LoadCurrencyDropDownData()
			success = success AndAlso LoadInvoiceTypeDropDownData()
			success = success AndAlso LoadInvoiceOptionDropDownData()

			Return success
		End Function

		''' <summary>
		''' Loads the OP shipment drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadOPShipmentDropDownData() As Boolean
			Dim opShipmentData = m_DataAccess.LoadOPShipmentData()

			If (opShipmentData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("OPVersanddaten konnten nicht geladen werden."))
			End If

			lueOPShipment.Properties.DataSource = opShipmentData
			lueOPShipment.Properties.ForceInitialize()

			Return Not opShipmentData Is Nothing
		End Function

		''' <summary>
		''' Loads the terms and conditions drop down data.
		''' </summary>
		Private Function LoadTermsAndConditionsDropDownData()
			Dim termsAndConditionsData = m_CommonDatabaseAccess.LoadTermsAndConditionsData()

			If (termsAndConditionsData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AGB-Daten konnten nicht geladen werden."))
			End If

			lueTermsAndConditions.Properties.DataSource = termsAndConditionsData
			lueTermsAndConditions.Properties.ForceInitialize()

			Return Not termsAndConditionsData Is Nothing
		End Function

		''' <summary>
		''' Loads the currency drop down data.
		''' </summary>
		Private Function LoadCurrencyDropDownData() As Boolean
			Dim currencyData = m_CommonDatabaseAccess.LoadCurrencyData()

			If (currencyData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Währungsdaten konnten nicht geladen werden."))
			End If

			lueCurrency.Properties.DataSource = currencyData
			lueCurrency.Properties.ForceInitialize()

			Return Not currencyData Is Nothing
		End Function

		''' <summary>
		''' Loads the Invoice type drop down data.
		''' </summary>
		Private Function LoadInvoiceTypeDropDownData()
			Dim invoiceTypeData = m_DataAccess.LoadInvoiceTypeData()

			If (invoiceTypeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fakturatyp-Daten konnten nicht geladen werden."))
			End If

			lueInvoiceType.Properties.DataSource = invoiceTypeData
			lueInvoiceType.Properties.DropDownRows = Math.Min(20, invoiceTypeData.Count)

			lueInvoiceType.Properties().ForceInitialize()

			Return Not invoiceTypeData Is Nothing
		End Function

		''' <summary>
		''' Loads the invoice options drop down data.
		''' </summary>
		Private Function LoadInvoiceOptionDropDownData() As Boolean
			Dim invoiceOptionsData = m_DataAccess.LoadInvoiceOptionsData()

			If (invoiceOptionsData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fakturaoptions-Daten konnten nicht geladen werden."))
			End If

			lueInvoiceOptions.Properties.DataSource = invoiceOptionsData
			lueInvoiceOptions.Properties.ForceInitialize()

			Return Not invoiceOptionsData Is Nothing
		End Function

		''' <summary>
		''' Resets the OP shipment drop down.
		''' </summary>
		Private Sub ResetCountrOPShipmentDropDown()

			lueOPShipment.Properties.DisplayMember = "Description"
			lueOPShipment.Properties.ValueMember = "Description"

			Dim columns = lueOPShipment.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0))

			lueOPShipment.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueOPShipment.Properties.SearchMode = SearchMode.AutoComplete
			lueOPShipment.Properties.AutoSearchColumnIndex = 0
			lueOPShipment.Properties.DropDownRows = 10
			lueOPShipment.Properties.ShowFooter = False

			lueOPShipment.Properties.NullText = String.Empty
			lueOPShipment.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the terms and conditions drop down.
		''' </summary>
		Private Sub ResetTermsAndConditionsDropDown()

			lueTermsAndConditions.Properties.DisplayMember = "TranslatedTermsAndConditions"
			lueTermsAndConditions.Properties.ValueMember = "Description"

			Dim columns = lueTermsAndConditions.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedTermsAndConditions", 0))

			lueTermsAndConditions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTermsAndConditions.Properties.SearchMode = SearchMode.AutoComplete
			lueTermsAndConditions.Properties.AutoSearchColumnIndex = 0

			lueTermsAndConditions.Properties.NullText = String.Empty
			lueTermsAndConditions.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the curreny drop down.
		''' </summary>
		Private Sub ResetCurrencyDropDown()

			lueCurrency.Properties.DisplayMember = "Description"
			lueCurrency.Properties.ValueMember = "Code"

			Dim columns = lueCurrency.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Code", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueCurrency.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCurrency.Properties.SearchMode = SearchMode.AutoComplete
			lueCurrency.Properties.AutoSearchColumnIndex = 0

			lueCurrency.Properties.NullText = String.Empty
			lueCurrency.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the invoice type drop down.
		''' </summary>
		Private Sub ResetInvoiceTypeDropDown()

			lueInvoiceType.Properties.DisplayMember = "Description"
			lueInvoiceType.Properties.ValueMember = "Code"

			Dim columns = lueInvoiceType.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Code", 100, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("Description", 400, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueInvoiceType.Properties.SearchMode = SearchMode.AutoComplete
			lueInvoiceType.Properties.AutoSearchColumnIndex = 1

			lueInvoiceType.Properties.AutoHeight = False
			lueInvoiceType.Properties.ShowLines = True
			lueInvoiceType.Properties.ShowHeader = True

			lueInvoiceType.Properties.DropDownItemHeight = 20
			lueInvoiceType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
			lueInvoiceType.Properties.PopupWidthMode = PopupWidthMode.ContentWidth

			lueInvoiceType.Properties.NullText = String.Empty
			lueInvoiceType.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the invoice option drop down.
		''' </summary>
		Private Sub ResetInvoiceOptionDropDown()

			lueInvoiceOptions.Properties.DisplayMember = "Description"
			lueInvoiceOptions.Properties.ValueMember = "Description"

			Dim columns = lueInvoiceOptions.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueInvoiceOptions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueInvoiceOptions.Properties.SearchMode = SearchMode.AutoComplete
			lueInvoiceOptions.Properties.AutoSearchColumnIndex = 0

			lueInvoiceOptions.Properties.NullText = String.Empty
			lueInvoiceOptions.EditValue = Nothing

		End Sub


		''' <summary>
		''' Resets the invoice address grid.
		''' </summary>
		Private Sub ResetInvoiceAddressGrid()

			' Reset the grid
			gvInvoiceAddresses.Columns.Clear()

			Dim columnInvoiceCompany As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoiceCompany.Caption = m_Translate.GetSafeTranslationValue("Firmenname")
			columnInvoiceCompany.Name = "InvoiceCompany"
			columnInvoiceCompany.FieldName = "InvoiceCompany"
			columnInvoiceCompany.Visible = True
			gvInvoiceAddresses.Columns.Add(columnInvoiceCompany)

			Dim columnInvoiceAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoiceAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnInvoiceAddress.Name = "InvoiceAddress"
			columnInvoiceAddress.FieldName = "InvoiceAddress"
			columnInvoiceAddress.Visible = True
			gvInvoiceAddresses.Columns.Add(columnInvoiceAddress)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
			activeColumn.Name = "Active"
			activeColumn.FieldName = "Active"
			activeColumn.Visible = True
			activeColumn.ColumnEdit = m_CheckEditActive
			gvInvoiceAddresses.Columns.Add(activeColumn)

			gridInvoiceAddresses.DataSource = Nothing

		End Sub


		''' <summary>
		''' Resets the open debitor posts grid.
		''' </summary>
		Private Sub ResetOpenDepitorPostGrid()

			' Reset the grid
			gvOpenDebitorInvoices.Columns.Clear()

			Dim columnNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnNumber.Name = "InvoiceNumber"
			columnNumber.FieldName = "InvoiceNumber"
			columnNumber.Visible = True
			gvOpenDebitorInvoices.Columns.Add(columnNumber)

			Dim dateColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			dateColumn.Caption = m_Translate.GetSafeTranslationValue("Datum")
			dateColumn.Name = "InvoiceData"
			dateColumn.FieldName = "InvoiceData"
			dateColumn.Visible = True
			gvOpenDebitorInvoices.Columns.Add(dateColumn)

			Dim faelligColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			faelligColumn.Caption = m_Translate.GetSafeTranslationValue("Fällig")
			faelligColumn.Name = m_Translate.GetSafeTranslationValue("DueDate")
			faelligColumn.FieldName = "DueDate"
			faelligColumn.Visible = True
			gvOpenDebitorInvoices.Columns.Add(faelligColumn)

			Dim amountColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			amountColumn.Caption = m_Translate.GetSafeTranslationValue("Betrag exk.")
			amountColumn.Name = "AmountEx"
			amountColumn.FieldName = "AmountEx"
			amountColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			amountColumn.AppearanceHeader.Options.UseTextOptions = True
			amountColumn.Visible = True
			amountColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			amountColumn.DisplayFormat.FormatString = "N"
			gvOpenDebitorInvoices.Columns.Add(amountColumn)

			Dim openAmountColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			openAmountColumn.Caption = m_Translate.GetSafeTranslationValue("Offen")
			openAmountColumn.Name = "OpenAmount"
			openAmountColumn.FieldName = "OpenAmount"
			openAmountColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			openAmountColumn.AppearanceHeader.Options.UseTextOptions = True
			openAmountColumn.Visible = True
			openAmountColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			openAmountColumn.DisplayFormat.FormatString = "N"
			gvOpenDebitorInvoices.Columns.Add(openAmountColumn)

			Dim zFilialColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			zFilialColumn.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			zFilialColumn.Name = "zfiliale"
			zFilialColumn.FieldName = "zfiliale"
			zFilialColumn.Visible = False
			gvOpenDebitorInvoices.Columns.Add(zFilialColumn)

			RestoreGridLayoutFromXml(gvOpenDebitorInvoices.Name)

			gridOpenDebitorInvoices.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the sales volume pivot grid.
		''' </summary>
		Private Sub ResetSalesVolumePivotGrid()
			Dim fieldCategory As PivotGridField = New PivotGridField("Category", PivotArea.RowArea)
			fieldCategory.Width = 120

			Dim fieldYear As PivotGridField = New PivotGridField("Year", PivotArea.ColumnArea)
			fieldYear.SortOrder = PivotSortOrder.Descending

			Dim fieldAmount As PivotGridField = New PivotGridField("Amount", PivotArea.DataArea)
			'fieldAmount.Appearance.Value.Options.UseTextOptions = True 
			'fieldAmount.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			fieldAmount.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			fieldAmount.CellFormat.FormatString = "N2"

			pivotSalesVolume.Fields.Clear()
			pivotSalesVolume.Fields.Add(fieldCategory)
			pivotSalesVolume.Fields.Add(fieldYear)
			pivotSalesVolume.Fields.Add(fieldAmount)

			pivotSalesVolume.OptionsView.ShowFilterHeaders = False
			pivotSalesVolume.OptionsView.ShowColumnHeaders = False
			pivotSalesVolume.OptionsView.ShowRowHeaders = False
			pivotSalesVolume.OptionsView.ShowDataHeaders = False

		End Sub

		''' <summary>
		''' Loads customer master data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerMasterData(ByVal customerNumber As Integer) As Boolean

			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_ClsProgSetting.GetUSFiliale)

			If (customerMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
				Return False
			End If

			spinNumberOfCopies.Text = IIf(customerMasterData.NumberOfCopies Is Nothing, 1, customerMasterData.NumberOfCopies)
			chkMwSt.Checked = IIf(customerMasterData.mwstpflicht Is Nothing, 0, customerMasterData.mwstpflicht)
			chkHoursInNormalTime.Checked = IIf(customerMasterData.ShowHoursInNormal Is Nothing, False, customerMasterData.ShowHoursInNormal)
			lueOPShipment.EditValue = customerMasterData.OPShipment

			txtValueAddedTaxNumber.Text = customerMasterData.ValueAddedTaxNumber
			lueTermsAndConditions.EditValue = customerMasterData.TermsAndConditions_WOS
			chkPublicizeForWOS.Checked = IIf(customerMasterData.sendToWOS Is Nothing, 0, customerMasterData.sendToWOS)
			chkDoNotShowContractInWOS.Checked = IIf(customerMasterData.DoNotShowContractInWOS Is Nothing, 0, customerMasterData.DoNotShowContractInWOS)

			lueCurrency.EditValue = customerMasterData.CurrencyCode
			lueInvoiceOptions.EditValue = customerMasterData.InvoiceOption
			lueInvoiceType.EditValue = customerMasterData.BillTypeCode

			Return True
		End Function

		''' <summary>
		''' Loads assigned customer email data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedCustomerEmailData(ByVal customerNumber As Integer) As Boolean

			Dim emailData = m_DataAccess.LoadAssignedEmailsOfCustomer(customerNumber)

			If (emailData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Admin-Emailadressen konnten nicht geladen werden."))
				Return False
			End If

			lstAdminEmails.DisplayMember = "Email"
			lstAdminEmails.ValueMember = "ID"
			lstAdminEmails.DataSource = emailData

			Return True

		End Function

		''' <summary>
		''' Loads assigned customer invoice address data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedCustomerInvoiceAddressData(ByVal customerNumber As Integer) As Boolean

			Dim customerInvoiceAddressData = m_DataAccess.LoadAssignedInvoiceAddressDataOfCustomer(customerNumber)

			If (customerInvoiceAddressData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsadressen konnten nicht geladen werden."))
				Return False
			End If

			Dim listDataSource As BindingList(Of InvoiceAddressViewData) = New BindingList(Of InvoiceAddressViewData)

			' Convert the data to view data.
			For Each address In customerInvoiceAddressData

				Dim invoiceAddressViewData = New InvoiceAddressViewData() With {
								.CustomerNumber = address.CustomerNumber,
								.RecordNumber = address.RecordNumber,
								.Id = address.ID,
								.InvoiceCompany = address.InvoiceCompany,
								.InvoiceEMailAddress = address.InvoiceEMailAddress,
								.InvoiceSendAsZip = address.InvoiceSendAsZip,
								.InvoiceAddress = String.Format("{0} {1}-{2}, {3}",
																address.InvoiceStreet, address.InvoiceCountryCode, address.InvoicePostcode, address.InvoiceLocation),
																.Active = address.Active}

				listDataSource.Add(invoiceAddressViewData)
			Next

			gridInvoiceAddresses.DataSource = listDataSource

			Return True

		End Function

		''' <summary>
		''' Loads assigned open customer debitor invoices data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedCustomerOpenDebitorInvoices(ByVal customerNumber As Integer) As Boolean

			Dim openDebitorInvoices = m_DataAccess.LoadAssignedOpenDebitorInvoicesOfCustomer(customerNumber)

			If (openDebitorInvoices Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Offene Debitoren konnten nicht geladen werden."))
				Return False
			End If

			Dim listDataSource As BindingList(Of OpenDebitorViewData) = New BindingList(Of OpenDebitorViewData)


			If openDebitorInvoices.Count > 0 Then

				Dim sumAmount As Decimal = (From d In openDebitorInvoices Select d.AmountInk).Sum()
				Dim sumAmountEx As Decimal = (From d In openDebitorInvoices Select d.AmountEx).Sum()
				Dim sumOpenAmount As Decimal = (From d In openDebitorInvoices Select d.OpenAmount).Sum()

				Dim total = New OpenDebitorViewData() With {
					.InvoiceNumber = m_Translate.GetSafeTranslationValue("Total"),
					.AmountEx = sumAmountEx,
					.Amount = sumAmount,
					.OpenAmount = sumOpenAmount
					}

				listDataSource.Add(total)
			End If

			' Convert the data to view data.
			For Each openDebitorInvoice In openDebitorInvoices
				Dim openDebitorInvoiceViewData = New OpenDebitorViewData() With {
								.InvoiceNumber = openDebitorInvoice.InvoiceNumber,
								.InvoiceData = openDebitorInvoice.InvoiceDate,
								.DueDate = openDebitorInvoice.DueDate,
								.Amount = openDebitorInvoice.AmountInk,
								.AmountEx = openDebitorInvoice.AmountEx,
								.OpenAmount = openDebitorInvoice.OpenAmount,
								.zFiliale = openDebitorInvoice.zFiliale
						}

				listDataSource.Add(openDebitorInvoiceViewData)
			Next


			gridOpenDebitorInvoices.DataSource = listDataSource

			Return True

		End Function

		''' <summary>
		''' Handles double click on invoice.
		''' </summary>
		Private Sub OngvOpenDebitorInvoices_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvOpenDebitorInvoices.DoubleClick
			Dim selectedRows = gvOpenDebitorInvoices.GetSelectedRows()
			If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 14, m_InitializationData.MDData.MDNr) Then Return

			If (selectedRows.Count > 0) Then
				Dim invoiceData = CType(gvOpenDebitorInvoices.GetRow(selectedRows(0)), OpenDebitorViewData)
				Dim reNr = invoiceData.InvoiceNumber

				Dim hub = MessageService.Instance.Hub
				Dim openInvoiceMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, reNr)
				hub.Publish(openInvoiceMng)

			End If

		End Sub



		''' <summary>
		''' Loads customer sales data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadSalesVolumneData(ByVal customerNumber As Integer) As Boolean

			Dim salesVolumeData = m_DataAccess.LoadSalesVolumeDataOfCustomer(customerNumber)

			If (salesVolumeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Umsatzzahlen konnten nicht geladen werden."))
				Return False
			End If

			Dim ds As BindingList(Of SalesVolumeData) = New BindingList(Of SalesVolumeData)

			' Category muss übersetzt werden!
			For Each salesData In salesVolumeData
				salesData.Category = m_Translate.GetSafeTranslationValue(salesData.Category)
				ds.Add(salesData)
			Next

			pivotSalesVolume.DataSource = ds
			pivotSalesVolume.BestFit()
			Return True

		End Function

		''' <summary>
		''' Shows the invoice addresses form.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="invoiceAddressRecordNumber">The invoice address record number to select.</param>
		Private Sub ShowInvoiceAddressForm(ByVal customerNumber As Integer, ByVal invoiceAddressRecordNumber As Integer?)

			If m_InvoiceAddressesForm Is Nothing OrElse m_InvoiceAddressesForm.IsDisposed Then

				If Not m_InvoiceAddressesForm Is Nothing Then
					' First cleanup handlers of old form before new form is created.
					RemoveHandler m_InvoiceAddressesForm.FormClosed, AddressOf OnInvoiceAddressFormClosed
					RemoveHandler m_InvoiceAddressesForm.InvoiceAddressDataSaved, AddressOf OnInvoiceAddressFormInvoiceDataSaved
					RemoveHandler m_InvoiceAddressesForm.InvoiceAddressDataDeleted, AddressOf OnInvoiceAddressFormInvoiceDataDeleted
				End If

				m_InvoiceAddressesForm = New frmInvoiceAddress(m_InitializationData)
				AddHandler m_InvoiceAddressesForm.FormClosed, AddressOf OnInvoiceAddressFormClosed
				AddHandler m_InvoiceAddressesForm.InvoiceAddressDataSaved, AddressOf OnInvoiceAddressFormInvoiceDataSaved
				AddHandler m_InvoiceAddressesForm.InvoiceAddressDataDeleted, AddressOf OnInvoiceAddressFormInvoiceDataDeleted
			End If

			m_InvoiceAddressesForm.Show()
			m_InvoiceAddressesForm.LoadCustomerInvoiceAddresses(customerNumber, invoiceAddressRecordNumber)
			m_InvoiceAddressesForm.BringToFront()

		End Sub

		''' <summary>
		''' Handles value change of invoice type drow down.
		''' </summary>
		Private Sub OnLueInvoicelType_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueInvoiceType.EditValueChanged

			Dim success = True

			Dim invoiceTypeData As InvoiceTypeData = TryCast(lueInvoiceType.GetSelectedDataRow(), InvoiceTypeData)

			If Not invoiceTypeData Is Nothing AndAlso invoiceTypeData.Code = "M" Then
				lueInvoiceOptions.Enabled = True
			Else
				lueInvoiceOptions.Enabled = False
				lueInvoiceOptions.EditValue = Nothing
			End If

		End Sub

		''' <summary>
		''' Handles keydown even on admin email text box.
		''' </summary>
		Private Sub OnTxtAdminEmail_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAdminEmail.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			Dim success As Boolean = True

			If (e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrEmpty(txtAdminEmail.Text)) Then

				Try
					Dim email As New MailAddress(txtAdminEmail.Text)
				Catch ex As Exception
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Bitte geben Sie eine gültige Emailadresse ein"))
					Return
				End Try

				Dim assignedEmails = m_DataAccess.LoadAssignedEmailsOfCustomer(m_CustomerNumber)

				If Not assignedEmails.Any(Function(data) data.Email.ToLower().Trim() = txtAdminEmail.Text.ToLower().Trim) Then

					Dim customerEmail As New CustomerAssignedEmailData With {.CustomerNumber = m_CustomerNumber, .Email = txtAdminEmail.Text}
					success = m_DataAccess.AddCustomerEmailAssignment(customerEmail)

				End If

				If Not success Then
					m_UtilityUI.ShowErrorDialog("Email konnte nicht gespeichert werden")
				End If

				LoadAssignedCustomerEmailData(m_CustomerNumber)
				txtAdminEmail.Text = String.Empty

			End If

		End Sub

		''' <summary>
		''' Handles keydown even on list admin emails.
		''' </summary>
		Private Sub OnLstAdminEmails_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstAdminEmails.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			Dim success As Boolean = True

			If (e.KeyCode = Keys.Delete) Then

				Dim customerEmailData As CustomerAssignedEmailData = TryCast(lstAdminEmails.SelectedItem, CustomerAssignedEmailData)

				If (Not customerEmailData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerEmailAssignment(customerEmailData.ID) Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Email konnte nicht gelöscht werden."))
					End If

					LoadAssignedCustomerEmailData(m_CustomerNumber)
					If lstAdminEmails.ItemCount = 0 Then chkPublicizeForWOS.Checked = False

				End If

			End If
		End Sub

		''' <summary>
		''' Handles key down on invoice address grid.
		''' </summary>
		''' <remarks></remarks>
		Private Sub OnGridInvoiceAddresses_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridInvoiceAddresses.KeyDown
			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 209, m_InitializationData.MDData.MDNr) Then Return

				Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim invoiceAddressData = CType(grdView.GetRow(selectedRows(0)), InvoiceAddressViewData)

						If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																						m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
							Return
						End If

						Dim result = m_DataAccess.DeleteCustomerInvoiceAddressAssignment(invoiceAddressData.Id, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

						Select Case result
							Case DeleteCustomerInvoiceAddressAssignmentResult.CouldNotDeleteOnlyOneRecordLeft
								m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Es existiert nur ein Datensatz. Bitte erfassen Sie einen neuen Datensatz und dann löschen Sie die gewünschten Daten."),
																																						 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

							Case DeleteCustomerInvoiceAddressAssignmentResult.CouldNotDeleteBecauseOfExistingKST
								m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnungsadresse kann nich gelöscht werden, da sie bereits mit einer Kostenstelle verbunden ist."))

							Case DeleteCustomerInvoiceAddressAssignmentResult.ErrorWhileDelete
								m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Rechnungsadresse konnte nicht gelöscht werden."))

							Case DeleteCustomerInvoiceAddressAssignmentResult.Deleted
								LoadAssignedCustomerInvoiceAddressData(m_CustomerNumber)

						End Select

					End If
				End If
			End If

		End Sub

		''' <summary>
		''' Handles double click on invoice address.
		''' </summary>
		Private Sub OnInvoiceAddresses_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvInvoiceAddresses.DoubleClick
			Dim selectedRows = gvInvoiceAddresses.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim invoiceAddressData = CType(gvInvoiceAddresses.GetRow(selectedRows(0)), InvoiceAddressViewData)
				ShowInvoiceAddressForm(m_CustomerNumber, invoiceAddressData.RecordNumber)
			End If
		End Sub

		''' <summary>
		''' Handles close of invoice address form.
		''' </summary>
		Private Sub OnInvoiceAddressFormClosed(sender As System.Object, e As System.EventArgs)
			LoadAssignedCustomerInvoiceAddressData(m_CustomerNumber)

			Dim invoiceAddressForm = CType(sender, frmInvoiceAddress)

			If Not invoiceAddressForm.SelectedInvoiceAddressViewData Is Nothing Then
				FocusInvoiceAddress(m_CustomerNumber, invoiceAddressForm.SelectedInvoiceAddressViewData.RecordNumber)
			End If

		End Sub

		''' <summary>
		''' Handles invoice save event of invoice form.
		''' </summary>
		Private Sub OnInvoiceAddressFormInvoiceDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)

			LoadAssignedCustomerInvoiceAddressData(m_CustomerNumber)

			Dim invoiceAddressForm = CType(sender, frmInvoiceAddress)

			If Not invoiceAddressForm.SelectedInvoiceAddressViewData Is Nothing Then
				FocusInvoiceAddress(m_CustomerNumber, invoiceAddressForm.SelectedInvoiceAddressViewData.RecordNumber)
			End If

		End Sub

		''' <summary>
		''' Handles invoice delete event of invoice form.
		''' </summary>
		Private Sub OnInvoiceAddressFormInvoiceDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)

			LoadAssignedCustomerInvoiceAddressData(m_CustomerNumber)

		End Sub

		''' <summary>
		''' Handles click on new invoice address button.
		''' </summary>
		Private Sub OnBtnAddInvoiceAddress_Click(sender As System.Object, e As System.EventArgs) Handles btnAddInvoiceAddress.Click
			If IsCustomerDataLoaded Then
				ShowInvoiceAddressForm(m_CustomerNumber, Nothing)
			End If
		End Sub

		''' <summary>
		''' Handles row style event of open debitor invoices.
		''' </summary>
		Private Sub OnGvOpenDebitorInvoices_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvOpenDebitorInvoices.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim number As String = view.GetRowCellDisplayText(e.RowHandle, view.Columns("InvoiceNumber"))

				If Not String.IsNullOrEmpty(number) AndAlso number.ToLower() = "total" Then
					e.Appearance.Font = New Font(e.Appearance.Font, FontStyle.Bold)
				End If
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
				ElseIf TypeOf sender Is ComboBoxEdit Then
					Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
					comboboxEdit.EditValue = Nothing
				ElseIf TypeOf sender Is DateEdit Then
					Dim dateEdit As DateEdit = CType(sender, DateEdit)
					dateEdit.EditValue = Nothing
				End If
			End If
		End Sub

		''' <summary>
		''' Focuses a invoice address.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The responsible person record number.</param>
		Private Sub FocusInvoiceAddress(ByVal customerNumber As Integer, ByVal recordNumber As Integer)

			Dim listDataSource As BindingList(Of InvoiceAddressViewData) = gridInvoiceAddresses.DataSource

			Dim invoiceAddressViewData = listDataSource.Where(Function(data) data.CustomerNumber = customerNumber AndAlso data.RecordNumber = recordNumber).FirstOrDefault()

			If Not invoiceAddressViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(invoiceAddressViewData)
				Dim rowHandle = gvInvoiceAddresses.GetRowHandle(sourceIndex)
				gvInvoiceAddresses.FocusedRowHandle = rowHandle
			End If

		End Sub

#End Region


#Region "GridSettings"

		Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
			Dim keepFilter = False
			Dim restoreLayout = True

			Select Case GridName.ToLower
				Case gvOpenDebitorInvoices.Name.ToLower
					Try
						keepFilter = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingOpenInvoiceCustomerFilter), False)
						restoreLayout = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreOpenInvoiceCustomerSetting), True)
					Catch ex As Exception

					End Try

					If restoreLayout AndAlso File.Exists(m_GVOpenInvoiceCustomerSettingfilename) Then gvOpenDebitorInvoices.RestoreLayoutFromXml(m_GVOpenInvoiceCustomerSettingfilename)
					If restoreLayout AndAlso Not keepFilter Then gvOpenDebitorInvoices.ActiveFilterCriteria = Nothing


				Case Else

					Exit Sub


			End Select


		End Sub

		Private Sub OngvOpenInvoiceCustomerColumnPositionChanged(sender As Object, e As System.EventArgs)

			gvOpenDebitorInvoices.SaveLayoutToXml(m_GVOpenInvoiceCustomerSettingfilename)

		End Sub



#End Region


#Region "View helper classes"

		''' <summary>
		'''  Invoice address view data.
		''' </summary>
		Class InvoiceAddressViewData
			Public Property Id As Integer
			Public Property CustomerNumber As Integer
			Public Property RecordNumber As Integer
			Public Property InvoiceCompany As String
			Public Property InvoiceAddress As String
			Public Property InvoiceEMailAddress As String
			Public Property InvoiceSendAsZip As Boolean?
			Public Property Active As Boolean?
		End Class

		''' <summary>
		'''  Open debitor posts.
		''' </summary>
		Class OpenDebitorViewData
			Public Property InvoiceNumber As String
			Public Property InvoiceData As DateTime?
			Public Property DueDate As DateTime?
			Public Property AmountEx As Decimal?
			Public Property Amount As Decimal?
			Public Property OpenAmount As Decimal?
			Public Property zFiliale As String

		End Class

#End Region


	End Class
End Namespace