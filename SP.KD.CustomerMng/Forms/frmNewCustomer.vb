Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports SP.Infrastructure
Imports SP.Infrastructure.UI


Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Settings
Imports SP.KD.CustomerMng.Settings
Imports SP.Infrastructure.Logging

Namespace UI

	Public Class frmNewCustomer

#Region "Private Fields"

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
		''' The data access object.
		''' </summary>
		Private m_DataAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		''' <remarks></remarks>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_md As Mandant
		Private m_common As CommonSetting

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="_setting">The initialization data.</param>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			m_md = New Mandant
			m_SettingsManager = New SettingsManager
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_common = New CommonSetting()

			m_DataAccess = New SP.DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_UtilityUI = New UtilityUI()
			m_Utility = New Utility()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			lueCountry.Properties.ShowHeader = False
			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 10

			luePostcode.Properties.ShowHeader = False
			luePostcode.Properties.ShowFooter = False
			luePostcode.Properties.DropDownRows = 10

			Me.gvExistingCustomers.OptionsView.ShowIndicator = False

			TranslateControls()

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Customer number of newly addes customer.
		''' </summary>
		Public Property NewlyAddedCustomerNumber As Integer?

#End Region

#Region "Private Methods"


		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)

			Me.lblFirma.Text = m_Translate.GetSafeTranslationValue(Me.lblFirma.Text)
			Me.lblStrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblStrasse.Text)
			Me.lblland.Text = m_Translate.GetSafeTranslationValue(Me.lblland.Text)
			Me.lblplz.Text = m_Translate.GetSafeTranslationValue(Me.lblplz.Text)
			Me.lblort.Text = m_Translate.GetSafeTranslationValue(Me.lblort.Text)

		End Sub

		Private Sub Onform_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_NEW_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_NEW_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_NEW_FORM_LOCATION)

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

		Private Sub SaveFromSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_NEW_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_NEW_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_NEW_FORM_HEIGHT, Me.Height)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Handles the form load event.
		''' </summary>
		Private Sub OnFrmNewCustomer_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

			ResetForm()

			AddHandler txtCompany1.TextChanged, AddressOf OnControl_LostFocus
			AddHandler txtStreet.TextChanged, AddressOf OnControl_LostFocus
			AddHandler txtLocation.TextChanged, AddressOf OnControl_LostFocus
			AddHandler lueCountry.TextChanged, AddressOf OnControl_LostFocus
			AddHandler luePostcode.TextChanged, AddressOf OnControl_LostFocus

		End Sub

		''' <summary>
		''' Resets the form.
		''' </summary>
		Private Sub ResetForm()

			txtCompany1.Text = String.Empty
			txtCompany1.Properties.MaxLength = 70

			txtStreet.Text = String.Empty
			txtStreet.Properties.MaxLength = 70

			txtLocation.Text = String.Empty
			txtLocation.Properties.MaxLength = 70

			ResetCountryDropDown()
			ResetPostcodeDropDown()
			ResetGridExistingCustomers()
		End Sub

		''' <summary>
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetCountryDropDown()

			lueCountry.Properties.DisplayMember = "Name"
			lueCountry.Properties.ValueMember = "Code"

			Dim columns = lueCountry.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Name", 0))

			lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCountry.Properties.SearchMode = SearchMode.AutoComplete
			lueCountry.Properties.AutoSearchColumnIndex = 0
			lueCountry.Properties.NullText = String.Empty
			lueCountry.EditValue = "CH"

			Dim countryData = m_CommonDatabaseAccess.LoadCountryData()
			lueCountry.Properties.DataSource = countryData
			lueCountry.Properties.ForceInitialize()
		End Sub

		''' <summary>
		''' Resets the postcode drop down.
		''' </summary>
		Private Sub ResetPostcodeDropDown()

			luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
			luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

			luePostcode.Properties.DisplayMember = "Postcode"
			luePostcode.Properties.ValueMember = "Postcode"

			Dim columns = luePostcode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Postcode", 0))
			columns.Add(New LookUpColumnInfo("Location", 0))

			luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePostcode.Properties.SearchMode = SearchMode.AutoComplete
			luePostcode.Properties.AutoSearchColumnIndex = 1
			luePostcode.Properties.NullText = String.Empty
			luePostcode.EditValue = Nothing

			Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()
			luePostcode.Properties.DataSource = postcodeData
			luePostcode.Properties.ForceInitialize()
		End Sub

		''' <summary>
		''' Resets the existing customers grid.
		''' </summary>
		Private Sub ResetGridExistingCustomers()

			' Reset the grid
			gvExistingCustomers.Columns.Clear()

			Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany.Name = "Company"
			columnCompany.FieldName = "Company"
			columnCompany.Visible = True
			gvExistingCustomers.Columns.Add(columnCompany)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "Address"
			columnAddress.FieldName = "Address"
			columnAddress.Visible = True
			gvExistingCustomers.Columns.Add(columnAddress)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "customeradvisor"
			columnAdvisor.FieldName = "customeradvisor"
			columnAdvisor.Visible = True
			gvExistingCustomers.Columns.Add(columnAdvisor)


			grdExistingCustomers.DataSource = Nothing

		End Sub

		''' <summary>
		''' Validates the input.
		''' </summary>
		Private Function ValidateInputData() As Boolean

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(txtCompany1, ErrorProvider1, String.IsNullOrEmpty(txtCompany1.Text), m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein."))
			isValid = isValid And SetErrorIfInvalid(txtStreet, ErrorProvider1, String.IsNullOrEmpty(txtStreet.Text), m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein."))
			isValid = isValid And SetErrorIfInvalid(lueCountry, ErrorProvider1, lueCountry.EditValue Is Nothing, m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein."))
			isValid = isValid And SetErrorIfInvalid(luePostcode, ErrorProvider1, luePostcode.EditValue Is Nothing, m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein."))
			isValid = isValid And SetErrorIfInvalid(txtLocation, ErrorProvider1, String.IsNullOrEmpty(txtLocation.Text), m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein."))

			Return isValid

		End Function

		''' <summary>
		''' Validates a control
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
		''' Reads the customer offset from the settings.
		''' </summary>
		''' <returns>Customer offset or zero if it could not be read.</returns>
		Private Function ReadCustomerOffsetFromSettings() As Integer

			Dim strQuery As String = "//StartNr/Kunden"
			Dim r = m_ClsProgSetting.GetUserProfileFile
			Dim customerNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
			Dim intVal As Integer

			If Integer.TryParse(customerNumberStartNumberSetting, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function

		''' <summary>
		''' Searches for existing customers.
		''' </summary>
		Private Sub SearchExistingCustomerData()

			Dim existingCustomerData As IEnumerable(Of ExistingCustomerSearchData) = m_DataAccess.LoadExistingCustomersBySearchCriteria(txtCompany1.Text, txtStreet.Text,
																																																																	luePostcode.EditValue, txtLocation.Text, String.Empty) 'lueCountry.EditValue)

			Dim existingCustomersGridData = (From customerData In existingCustomerData
											 Select New ExistingCustomerGridViewItem With
														  {.Company = customerData.Company,
															 .Address = String.Format("{0} {1}-{2}, {3}", customerData.Street, customerData.CountryCode, customerData.Postcode, customerData.Location),
															 .customerkst = customerData.customerKST,
															 .customeradvisor = customerData.customerAdvisor
														  }).ToList()


			Dim listDataSource As BindingList(Of ExistingCustomerGridViewItem) = New BindingList(Of ExistingCustomerGridViewItem)

			For Each customerGridData In existingCustomersGridData

				listDataSource.Add(customerGridData)

			Next

			grdExistingCustomers.DataSource = listDataSource

		End Sub

		''' <summary>
		''' Handles click on save button.
		''' </summary>
		Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

			If ValidateInputData() Then

				Dim customerNumberOffsetFromSettings As Integer = ReadCustomerOffsetFromSettings()
				Dim kst = m_InitializationData.UserData.UserKST
				Dim currencyvalue As String = "CHF"
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim xmlFilename As String = m_md.GetSelectedMDFormDataXMLFilename(m_md.GetDefaultMDNr)

				Dim invoiceremindercode As String = m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/invoiceremindercode", FORM_XML_MAIN_KEY))
				Dim PaymentCondition As String = m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/conditionalcash", FORM_XML_MAIN_KEY))
				Dim InvoiceOption As String = m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/invoicetype", FORM_XML_MAIN_KEY))
				Dim NoUse As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/customernotuse", FORM_XML_MAIN_KEY)), False)
				Dim CreditWarning As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/warnbycreditlimitexceeded", FORM_XML_MAIN_KEY)), False)
				Dim CreditLimit1 As Decimal? = m_Utility.ParseToDec(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/firstcreditlimitamount", FORM_XML_MAIN_KEY)), 0)
				Dim CreditLimit2 As Decimal? = m_Utility.ParseToDec(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/secondcreditlimitamount", FORM_XML_MAIN_KEY)), 0)
				Dim OneInvoicePerMail As String = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFilename, String.Format("{0}/oneinvoicepermail", FORM_XML_MAIN_KEY)), False)

				Dim initCustomerData = New NewCustomerInitData With {.CustomerNumberOffset = customerNumberOffsetFromSettings}
				initCustomerData.CustomerMandantNumber = m_InitializationData.MDData.MDNr
				initCustomerData.Company1 = txtCompany1.EditValue
				initCustomerData.Street = txtStreet.EditValue
				initCustomerData.CountryCode = lueCountry.EditValue
				initCustomerData.Postcode = luePostcode.EditValue
				initCustomerData.Location = txtLocation.Text
				initCustomerData.KST = m_InitializationData.UserData.UserKST
				initCustomerData.CurrencyCode = currencyvalue
				initCustomerData.ReminderCode = invoiceremindercode
				initCustomerData.PaymentCondition = PaymentCondition
				initCustomerData.InvoiceOption = InvoiceOption
				initCustomerData.NoUse = NoUse
				initCustomerData.CreditWarning = CreditWarning

				initCustomerData.OneInvoicePerMail = OneInvoicePerMail
				initCustomerData.CreditLimit1 = CreditLimit1
				initCustomerData.CreditLimit2 = CreditLimit2
				initCustomerData.KDBusinessBranch = m_InitializationData.UserData.UserFiliale
				initCustomerData.CreatedFrom = m_InitializationData.UserData.UserFullName
				initCustomerData.CreatedUserNumber = m_InitializationData.UserData.UserNr

				Dim success = m_DataAccess.AddNewCustomer(initCustomerData)


				NewlyAddedCustomerNumber = initCustomerData.CustomerNumber

				If success Then
					DialogResult = DialogResult.OK
					Close()
				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Kunde konnte nicht angelegt werden."))

					Return
				End If

			End If

		End Sub

		Private Sub Onform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
			SaveFromSettings()
		End Sub

		''' <summary>
		''' Handles click on close button.
		''' </summary>
		Private Sub OnBtnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
			DialogResult = DialogResult.Abort
			Close()
		End Sub

		''' <summary>
		''' Handles controls lost focus event.
		''' </summary>
		Private Sub OnControl_LostFocus(sender As System.Object, e As System.EventArgs)
			SearchExistingCustomerData()
		End Sub

		''' <summary>
		''' Handles drop down button click.
		''' </summary>
		Private Sub OnDropDown_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueCountry.Properties.ButtonClick, luePostcode.ButtonClick
			' If delete button has been clicked reset the drop down.
			If e.Button.Index = 1 Then

				If TypeOf sender Is LookUpEdit Then
					Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
					lookupEdit.EditValue = Nothing
				End If
			End If
		End Sub

		''' <summary>
		''' Handles change of postcode.
		''' </summary>
		Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

			Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)

			If Not postCodeData Is Nothing Then
				txtLocation.Text = postCodeData.Location
			End If

		End Sub

		''' <summary>
		''' Handles new value event on postcode(plz) lookup edit.
		''' </summary>
		Private Sub OnLuePostcode_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles luePostcode.ProcessNewValue

			If Not luePostcode.Properties.DataSource Is Nothing Then

				Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

				Dim newPostcode As New PostCodeData With {.Postcode = e.DisplayValue.ToString()}
				listOfPostcode.Add(newPostcode)

				e.Handled = True
			End If
		End Sub

#End Region

#Region "View helper classes"

		''' <summary>
		''' View data for existing customers.
		''' </summary>
		Private Class ExistingCustomerGridViewItem

			Public Property Company As String
			Public Property Address As String

			Public Property customerkst As String
			Public Property customeradvisor As String

		End Class

#End Region

	End Class

End Namespace