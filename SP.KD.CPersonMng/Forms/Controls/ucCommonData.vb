
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraEditors.Popup

Namespace UI

	''' <summary>
	''' Common data.
	''' </summary>
	Public Class ucCommonData

#Region "Private Consts"

		Private Const DEFAULT_SALUTATION_CODE_MALE = "Herr"
		Private Const DEFAULT_SALUTATION_CODE_FEMALE = "Frau"
		Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY As String = "MD_{0}/Templates/customer-contact-body"

#End Region

#Region "Private Fields"

		Private m_SalutationData As IEnumerable(Of SalutationData)
		Private m_MandantSettingsXml As SettingsXml
		Private m_Mandant As Mandant


		Private m_xmlSettingCustomerContactBodySetting As String

#End Region

#Region "Constructor"

		Public Sub New()

			m_Mandant = New Mandant

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			'm_xmlSettingCustomerContactBodySetting = String.Format(MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY, m_InitializationData.MDData.MDNr)

			lueAdvisor.Properties.DropDownRows = 20

			lueContactInfo.Properties.ShowHeader = False
			lueContactInfo.Properties.ShowFooter = False
			lueContactInfo.Properties.DropDownRows = 10

			lueState1.Properties.ShowHeader = False
			lueState1.Properties.ShowFooter = False
			lueState1.Properties.DropDownRows = 10

			lueState2.Properties.ShowHeader = False
			lueState2.Properties.ShowFooter = False
			lueState2.Properties.DropDownRows = 10

			lueTermsAndConditions.Properties.ShowHeader = False
			lueTermsAndConditions.Properties.ShowFooter = False
			lueTermsAndConditions.Properties.DropDownRows = 10

			lueSalutation.Properties.ShowHeader = False
			lueSalutation.Properties.ShowFooter = False
			lueSalutation.Properties.DropDownRows = 10

			lueDepartment.Properties.ShowHeader = False
			lueDepartment.Properties.ShowFooter = False
			lueDepartment.Properties.DropDownRows = 20

			luePosition.Properties.ShowHeader = False
			luePosition.Properties.ShowFooter = False
			luePosition.Properties.DropDownRows = 20

			lueSalutationForm.Properties.ShowHeader = False
			lueSalutationForm.Properties.ShowFooter = False
			lueSalutationForm.Properties.DropDownRows = 20

			AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueSalutation.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueDepartment.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePosition.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueSalutationForm.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditBirthday.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisor.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueContactInfo.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueState1.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueState2.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueTermsAndConditions.ButtonClick, AddressOf OnDropDown_ButtonClick


			Dim communicationHub = MessageService.Instance.Hub
			communicationHub.Subscribe(Of RefreshResponsiblePersonAddress)(AddressOf HandleRefreshResponsiblePersonAddress)

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean
			m_SuppressUIEvents = True
			Dim success As Boolean = True

			m_xmlSettingCustomerContactBodySetting = String.Format(MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY, m_InitializationData.MDData.MDNr)
			m_MandantSettingsXml = New SettingsXml(m_Mandant.GetAllUserGridSettingXMLFilename(m_InitializationData.MDData.MDNr))

			If (Not IsIntialControlDataLoaded) Then
				success = success AndAlso LoadDropDownData()
				IsIntialControlDataLoaded = True
			End If

			If (recordNumber.HasValue) Then
				If (Not IsResponsiblePersonDataLoaded) Then
					success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)

				ElseIf Not customerNumber = m_CustomerNumber OrElse Not m_RecordNumber = recordNumber Then
					success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)

				End If

			Else
				Reset()
			End If
			m_SuppressUIEvents = False

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

			m_CustomerNumber = 0
			m_RecordNumber = Nothing

			' Address
			txtCompany1.Text = String.Empty
			txtCompany1.Properties.MaxLength = 70

			txtPostOfficeBox.Text = String.Empty
			txtPostOfficeBox.Properties.MaxLength = 70

			txtStreet.Text = String.Empty
			txtStreet.Properties.MaxLength = 70

			lueCountry.EditValue = Nothing

			m_SuppressUIEvents = True
			luePostcode.EditValue = Nothing
			m_SuppressUIEvents = False

			txtLocation.Text = String.Empty
			txtLocation.Properties.MaxLength = 70

			' Personal info
			lueSalutation.EditValue = Nothing
			txtLastname.Text = String.Empty
			txtLastname.Properties.MaxLength = 70

			txtFirstname.Text = String.Empty
			txtFirstname.Properties.MaxLength = 70

			lueDepartment.EditValue = Nothing
			luePosition.EditValue = Nothing
			lueSalutationForm.EditValue = Nothing
			dateEditBirthday.EditValue = Nothing
			memoEditInterests.Text = String.Empty
			memoEditInterests.Properties.MaxLength = 4000

			' Properties
			lueAdvisor.EditValue = Nothing
			lueContactInfo.EditValue = Nothing
			lueState1.EditValue = Nothing
			lueState2.EditValue = Nothing
			lueTermsAndConditions.EditValue = Nothing

			' Communcation
			txtTelephone.Text = String.Empty
			txtTelephone.Properties.MaxLength = 70

			txtTelefax.Text = String.Empty
			txtTelefax.Properties.MaxLength = 70
			txtTelefax.Properties.Buttons(0).Tag = 1
			txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.apply_16x16

			txtMobile.Text = String.Empty
			txtMobile.Properties.MaxLength = 70
			txtMobile.Properties.Buttons(2).Tag = 1
			txtMobile.Properties.Buttons(2).ImageOptions.Image = My.Resources.apply_16x16

			txtEmail.Text = String.Empty
			txtEmail.Properties.MaxLength = 70
			txtEmail.Properties.Buttons(1).Tag = 1
			txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.apply_16x16

			txtXing.Text = String.Empty
			txtXing.Properties.MaxLength = 255

			txtComment.Text = String.Empty
			txtComment.Properties.MaxLength = 4000


			txtFacebook.Text = String.Empty
			txtFacebook.Properties.MaxLength = 255
			txtLinkedIn.Text = String.Empty
			txtLinkedIn.Properties.MaxLength = 255

			txtFacebook.Visible = False
			txtLinkedIn.Location = txtFacebook.Location
			txtLinkedIn.Visible = True
			btnKommunikationArt.Text = "LinkedIn"
			BuildCommunicationDropDown()


			' ---Reset address data---

			ResetCountryDropDown()
			ResetPostcodeDropDown()
			ResetSalutationDropDown()
			ResetDepartmentDropDown()
			ResetPositionDropDown()
			ResetSalutationFormDropDown()
			ResetAdvisorDropDown()
			ResetContactInfoDropDown()
			ResetState1DropDown()
			ResetState2DropDown()
			ResetTermsAndConditionsDropDown()

			errorProvider.Clear()
		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(txtCompany1, errorProvider, String.IsNullOrEmpty(txtCompany1.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(txtStreet, errorProvider, String.IsNullOrEmpty(txtStreet.Text) AndAlso String.IsNullOrEmpty(txtPostOfficeBox.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueCountry, errorProvider, lueCountry.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(luePostcode, errorProvider, luePostcode.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(txtLocation, errorProvider, String.IsNullOrEmpty(txtLocation.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueSalutation, errorProvider, lueSalutation.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(txtLastname, errorProvider, String.IsNullOrEmpty(txtLastname.Text), errorText)

			Return isValid

		End Function

		''' <summary>
		''' Merges the responsible person master data.
		''' </summary>
		''' <param name="responsiblePersonMasterData">The responsible person master data object where the data gets filled into.</param>
		Public Overrides Sub MergeResponsiblePersonMasterData(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData, Optional forceMerge As Boolean = False)
			If ((IsResponsiblePersonDataLoaded AndAlso
					m_CustomerNumber = responsiblePersonMasterData.CustomerNumber AndAlso
					m_RecordNumber = responsiblePersonMasterData.RecordNumber) Or forceMerge) Then


				' Address
				responsiblePersonMasterData.Company1 = txtCompany1.Text
				responsiblePersonMasterData.PostOfficeBox = txtPostOfficeBox.Text
				responsiblePersonMasterData.Street = txtStreet.Text
				responsiblePersonMasterData.CountryCode = lueCountry.EditValue
				responsiblePersonMasterData.Postcode = luePostcode.EditValue
				responsiblePersonMasterData.Location = txtLocation.Text

				' Personal info
				responsiblePersonMasterData.Salutation = lueSalutation.EditValue
				responsiblePersonMasterData.Lastname = txtLastname.Text
				responsiblePersonMasterData.Firstname = txtFirstname.Text
				responsiblePersonMasterData.Department = lueDepartment.EditValue
				responsiblePersonMasterData.Position = luePosition.EditValue
				responsiblePersonMasterData.SalutationForm = lueSalutationForm.EditValue
				responsiblePersonMasterData.Birthdate = dateEditBirthday.EditValue
				responsiblePersonMasterData.Interests = memoEditInterests.Text

				' Properties
				responsiblePersonMasterData.Advisor = lueAdvisor.EditValue
				responsiblePersonMasterData.KDZHowKontakt = lueContactInfo.EditValue
				responsiblePersonMasterData.State1 = lueState1.EditValue
				responsiblePersonMasterData.State2 = lueState2.EditValue
				responsiblePersonMasterData.TermsAndConditions_WOS = lueTermsAndConditions.EditValue

				' Communication
				responsiblePersonMasterData.Telephone = txtTelephone.Text
				responsiblePersonMasterData.Telefax = txtTelefax.Text
				responsiblePersonMasterData.MobilePhone = txtMobile.Text
				responsiblePersonMasterData.Email = txtEmail.Text
				responsiblePersonMasterData.Facebook = txtFacebook.Text
				responsiblePersonMasterData.LinkedIn = txtLinkedIn.Text
				responsiblePersonMasterData.Xing = txtXing.Text

				'responsiblePersonMasterData.Telefax_Mailing = chkTelefaxMailing.Checked
				'responsiblePersonMasterData.SMS_Mailing = chkSMSMailing.Checked
				'responsiblePersonMasterData.Email_Mailing = chkEmailMailing.Checked


				If txtTelefax.Properties.Buttons(0).Tag = 2 Then
					responsiblePersonMasterData.Telefax_Mailing = True
				Else
					responsiblePersonMasterData.Telefax_Mailing = False
				End If

				If txtMobile.Properties.Buttons(2).Tag = 2 Then
					responsiblePersonMasterData.SMS_Mailing = True
				Else
					responsiblePersonMasterData.SMS_Mailing = False
				End If

				If txtEmail.Properties.Buttons(1).Tag = 2 Then
					responsiblePersonMasterData.Email_Mailing = True
				Else
					responsiblePersonMasterData.Email_Mailing = False
				End If





				' Comments
				responsiblePersonMasterData.KDZComments = txtComment.Text

			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub

		''' <summary>
		''' Prepares to enter new responible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function PrepareToEnterNewResponsiblePerson(ByVal customerNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = success AndAlso Activate(customerNumber, Nothing)

			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_InitializationData.UserData.UserFiliale)

			If (Not customerMasterData Is Nothing) Then

				grpAdresse.Text = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Adresse"), customerMasterData.CustomerNumber)
				grpPersonalien.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue("Personalien"))

				txtCompany1.Text = customerMasterData.Company1
				txtStreet.Text = customerMasterData.Street

				m_SuppressUIEvents = True

				' Add postcode if its not in list of known postcodes.
				Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

				If Not String.IsNullOrEmpty(customerMasterData.Postcode) AndAlso
					Not listOfPostcode.Any(Function(postcode) postcode.Postcode = customerMasterData.Postcode) Then
					Dim newPostcode As New PostCodeData With {.Postcode = customerMasterData.Postcode}
					listOfPostcode.Add(newPostcode)
				End If
				luePostcode.EditValue = customerMasterData.Postcode


				m_SuppressUIEvents = False
				lueCountry.EditValue = customerMasterData.CountryCode
				txtLocation.Text = customerMasterData.Location

				m_CustomerNumber = customerNumber
				m_RecordNumber = Nothing
			Else
				success = False
			End If

			LoadAdvisorDropDownData()

			Return success
		End Function

#End Region

#Region "Privte Methods"

		''' <summary>
		'''  Transalte controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.grpAdresse.Text = m_Translate.GetSafeTranslationValue(Me.grpAdresse.Text, True)
			Me.grpPersonalien.Text = m_Translate.GetSafeTranslationValue(Me.grpPersonalien.Text, True)
			Me.grpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.grpEigenschaften.Text, True)
			Me.grpkommunikation.Text = m_Translate.GetSafeTranslationValue(Me.grpkommunikation.Text, True)
			Me.grpbemerkung.Text = m_Translate.GetSafeTranslationValue(Me.grpbemerkung.Text)

			Me.lblfirma.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma.Text)
			Me.lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
			Me.lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)
			Me.lblland.Text = m_Translate.GetSafeTranslationValue(Me.lblland.Text)
			Me.lblplz.Text = m_Translate.GetSafeTranslationValue(Me.lblplz.Text)
			Me.lblort.Text = m_Translate.GetSafeTranslationValue(Me.lblort.Text)

			Me.lblanrede.Text = m_Translate.GetSafeTranslationValue(Me.lblanrede.Text)
			Me.lblnachname.Text = m_Translate.GetSafeTranslationValue(Me.lblnachname.Text)
			Me.lblVorname.Text = m_Translate.GetSafeTranslationValue(Me.lblVorname.Text, True)
			Me.lblabteilung.Text = m_Translate.GetSafeTranslationValue(Me.lblabteilung.Text, True)
			Me.lblposition.Text = m_Translate.GetSafeTranslationValue(Me.lblposition.Text)
			Me.lblanredeform.Text = m_Translate.GetSafeTranslationValue(Me.lblanredeform.Text)
			Me.lblgeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblgeburtsdatum.Text)
			Me.lblinteressen.Text = m_Translate.GetSafeTranslationValue(Me.lblinteressen.Text)

			Me.lbl1status.Text = m_Translate.GetSafeTranslationValue(Me.lbl1status.Text, True)
			Me.lbl2status.Text = m_Translate.GetSafeTranslationValue(Me.lbl2status.Text, True)
			Me.lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text, True)
			Me.lblkontakt.Text = m_Translate.GetSafeTranslationValue(Me.lblkontakt.Text, True)
			Me.lblAGBWOS.Text = m_Translate.GetSafeTranslationValue(Me.lblAGBWOS.Text, True)

			lblTelefon.Text = m_Translate.GetSafeTranslationValue(lblTelefon.Text)
			lbltelefax.Text = m_Translate.GetSafeTranslationValue(lbltelefax.Text)
			lblNatel.Text = m_Translate.GetSafeTranslationValue(lblNatel.Text)

		End Sub


		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadResponsiblePersonData(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadResponsiblePersonMasterData(customerNumber, recordNumber)

			m_CustomerNumber = IIf(success, customerNumber, 0)
			m_RecordNumber = IIf(success, recordNumber, Nothing)

			LoadAdvisorDropDownData()

			errorProvider.Clear()

			Return success
		End Function

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			Dim dropDownResult = LoadCountryDropDownData()
			dropDownResult = dropDownResult AndAlso LoadPostcodeDropDownData()
			dropDownResult = dropDownResult AndAlso LoadSalutationAndSalutationFormDropDownData()
			dropDownResult = dropDownResult AndAlso LoadDepartmentDropDownData()
			dropDownResult = dropDownResult AndAlso LoadPositionDropDownData()
			'dropDownResult = dropDownResult AndAlso LoadAdvisorDropDownData()
			dropDownResult = dropDownResult AndAlso LoadContactInfoDropDownData()
			dropDownResult = dropDownResult AndAlso LoadState1DropDownData()
			dropDownResult = dropDownResult AndAlso LoadState2DropDownData()
			dropDownResult = dropDownResult AndAlso LoadTermsAndConditionsDropDownData()

			Return success
		End Function

		''' <summary>
		''' Loads the country drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadCountryDropDownData() As Boolean
			Dim result As Boolean = True
			Dim countryData As IEnumerable(Of CVLBaseTableViewData) = Nothing

			Try
				Dim baseTable = New SPSBaseTables(m_InitializationData)
				baseTable.BaseTableName = "Country"
				countryData = baseTable.PerformCVLBaseTablelistWebserviceCall()

				If (countryData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
				End If

				lueCountry.Properties.DataSource = countryData
				lueCountry.Properties.ForceInitialize()

			Catch ex As Exception

			End Try

			'Dim countryData = m_CommonDatabaseAccess.LoadCountryData()
			Return Not countryData Is Nothing
		End Function

		''' <summary>
		''' Loads the salutation and salutation form drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadSalutationAndSalutationFormDropDownData() As Boolean
			m_SalutationData = m_CommonDatabaseAccess.LoadSalutationData()

			If (m_SalutationData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Anredeformen konnten nicht geladen werden."))
			End If

			lueSalutation.Properties.DataSource = m_SalutationData
			lueSalutation.Properties.ForceInitialize()

			lueSalutationForm.Properties.DataSource = m_SalutationData
			lueSalutationForm.Properties.ForceInitialize()

			Return Not m_SalutationData Is Nothing
		End Function

		''' <summary>
		''' Loads the postcode drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadPostcodeDropDownData() As Boolean
			Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

			If (postcodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
			End If

			luePostcode.Properties.DataSource = postcodeData
			luePostcode.Properties.ForceInitialize()

			Return Not postcodeData Is Nothing
		End Function

		''' <summary>
		''' Loads the department drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDepartmentDropDownData() As Boolean
			Dim departmentData = m_DataAccess.LoadDepartmentData()

			If (departmentData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Abteilungsdaten konnten nicht geladen werden."))
			End If

			lueDepartment.Properties.DataSource = departmentData
			lueDepartment.Properties.ForceInitialize()

			Return Not departmentData Is Nothing
		End Function

		''' <summary>
		''' Loads the position drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadPositionDropDownData() As Boolean
			Dim positionData = m_DataAccess.LoadPositionData()

			If (positionData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Positionsdaten konnten nicht geladen werden."))
			End If

			luePosition.Properties.DataSource = positionData
			luePosition.Properties.ForceInitialize()

			Return Not positionData Is Nothing
		End Function

		''' <summary>
		''' Loads the advisor drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadAdvisorDropDownData() As Boolean
			' m_RecordNumber
			'Dim advisorData = m_CommonDatabaseAccess.LoadAdvisorsData()

			Dim advisorData As IEnumerable(Of AdvisorData) '= m_CommonDatabaseAccess.LoadAdvisorData()
			If m_RecordNumber.GetValueOrDefault(0) = 0 Then
				advisorData = m_CommonDatabaseAccess.LoadActivatedAdvisorData()
			Else
				advisorData = m_CommonDatabaseAccess.LoadAllAdvisorsData()
			End If


			Dim advisorViewDataList As New List(Of AdvisorViewData)

			If Not advisorData Is Nothing Then
				For Each advisor In advisorData
					Dim advisorViewData As AdvisorViewData = New AdvisorViewData
					advisorViewData.KST = advisor.KST
					advisorViewData.FristName = advisor.Firstname
					advisorViewData.LastName = advisor.Lastname

					advisorViewDataList.Add(advisorViewData)
				Next
			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
			End If

			lueAdvisor.Properties.DataSource = advisorViewDataList
			lueAdvisor.Properties.ForceInitialize()

			Return Not advisorData Is Nothing
		End Function

		''' <summary>
		''' Loads the contact info drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadContactInfoDropDownData() As Boolean
			Dim contactInfoData = m_DataAccess.LoadResponsiblePersonContactInfoData()

			If (contactInfoData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))
			End If

			lueContactInfo.Properties.DataSource = contactInfoData
			lueContactInfo.Properties.ForceInitialize()

			Return Not contactInfoData Is Nothing
		End Function

		''' <summary>
		''' Loads the state1 drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadState1DropDownData() As Boolean
			Dim state1Data = m_DataAccess.LoadResponsiblePersonStateData1()

			If (state1Data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status(1)-Daten konnten nicht geladen werden."))
			End If

			lueState1.Properties.DataSource = state1Data
			lueState1.Properties.ForceInitialize()

			Return Not state1Data Is Nothing
		End Function

		''' <summary>
		''' Loads the state2 drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadState2DropDownData() As Boolean
			Dim state2Data = m_DataAccess.LoadResponsiblePersonStateData2()

			If (state2Data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status(2)-Daten konnten nicht geladen werden."))
			End If

			lueState2.Properties.DataSource = state2Data
			lueState2.Properties.ForceInitialize()

			Return Not state2Data Is Nothing
		End Function

		''' <summary>
		''' Loads the terms and conditions drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadTermsAndConditionsDropDownData() As Boolean
			Dim termsAndConditionsData = m_CommonDatabaseAccess.LoadTermsAndConditionsData()

			If (termsAndConditionsData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AGB-Daten konnten nicht geladen werden."))
			End If

			lueTermsAndConditions.Properties.DataSource = termsAndConditionsData
			lueTermsAndConditions.Properties.ForceInitialize()

			Return Not termsAndConditionsData Is Nothing
		End Function

		Private Sub BuildCommunicationDropDown()

			Dim itm As New DevExpress.XtraBars.BarButtonItem
			Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu

			BarManager1.Images = ImageCollection1
			popupMenu1.Manager = BarManager1
			btnKommunikationArt.DropDownControl = popupMenu1

			itm = New DevExpress.XtraBars.BarButtonItem With {.Name = "LINKEDIN", .Caption = "LinkedIn", .ImageIndex = 0}
			popupMenu1.AddItem(itm)
			AddHandler itm.ItemClick, AddressOf GetMnuItem4Communication


			itm = New DevExpress.XtraBars.BarButtonItem With {.Name = "FACEBOOK", .Caption = "Facebook", .ImageIndex = 1}
			popupMenu1.AddItem(itm).BeginGroup = True

			AddHandler itm.ItemClick, AddressOf GetMnuItem4Communication

		End Sub

		Private Sub GetMnuItem4Communication(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
			Dim strMnuName As String = e.Item.Name.ToLower

			txtFacebook.Visible = False
			txtLinkedIn.Visible = False

			Select Case strMnuName
				Case "LINKEDIN".ToLower
					btnKommunikationArt.ImageOptions.Image = ImageCollection1.Images(0)
					btnKommunikationArt.Text = "LinkedIn"
					txtLinkedIn.Visible = True
					txtLinkedIn.Location = txtFacebook.Location

				Case "FACEBOOK".ToLower
					btnKommunikationArt.ImageOptions.Image = ImageCollection1.Images(1)
					btnKommunikationArt.Text = "Facebook"
					txtFacebook.Visible = True


				Case Else
					Return

			End Select


		End Sub

		''' <summary>
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetCountryDropDown()

			lueCountry.Properties.DisplayMember = "Translated_Value"
			lueCountry.Properties.ValueMember = "Code"

			Dim columns = lueCountry.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Land")))

			lueCountry.Properties.DropDownRows = 10
			lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCountry.Properties.SearchMode = SearchMode.AutoComplete
			lueCountry.Properties.AutoSearchColumnIndex = 0

			lueCountry.Properties.NullText = String.Empty
			lueCountry.EditValue = Nothing

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
			columns.Add(New LookUpColumnInfo("Postcode", 0, m_Translate.GetSafeTranslationValue("PLZ")))
			columns.Add(New LookUpColumnInfo("Location", 0, m_Translate.GetSafeTranslationValue("Ort")))

			luePostcode.Properties.DropDownRows = 10
			luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePostcode.Properties.SearchMode = SearchMode.AutoComplete
			luePostcode.Properties.AutoSearchColumnIndex = 1
			luePostcode.Properties.NullText = String.Empty
			m_SuppressUIEvents = True
			luePostcode.EditValue = Nothing
			m_SuppressUIEvents = False
		End Sub

		''' <summary>
		''' Resets the salutation drop down.
		''' </summary>
		Private Sub ResetSalutationDropDown()

			lueSalutation.Properties.DisplayMember = "TranslatedSalutation"
			lueSalutation.Properties.ValueMember = "Salutation"

			Dim columns = lueSalutation.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedSalutation", 0))

			lueSalutation.Properties.DropDownRows = 10
			lueSalutation.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSalutation.Properties.SearchMode = SearchMode.AutoComplete
			lueSalutation.Properties.AutoSearchColumnIndex = 0
			lueSalutation.Properties.NullText = String.Empty
			lueSalutation.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the department drop down.
		''' </summary>
		Private Sub ResetDepartmentDropDown()

			lueDepartment.Properties.DisplayMember = "Description"
			lueDepartment.Properties.ValueMember = "Description"

			Dim columns = lueDepartment.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0))

			lueDepartment.Properties.DropDownRows = 10
			lueDepartment.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueDepartment.Properties.SearchMode = SearchMode.AutoComplete
			lueDepartment.Properties.AutoSearchColumnIndex = 0
			lueDepartment.Properties.NullText = String.Empty
			lueDepartment.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the position drop down.
		''' </summary>
		Private Sub ResetPositionDropDown()

			luePosition.Properties.DisplayMember = "Description"
			luePosition.Properties.ValueMember = "Description"

			Dim columns = luePosition.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0))

			luePosition.Properties.DropDownRows = 10
			luePosition.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePosition.Properties.SearchMode = SearchMode.AutoComplete
			luePosition.Properties.AutoSearchColumnIndex = 0
			luePosition.Properties.NullText = String.Empty
			luePosition.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the salutation form drop down.
		''' </summary>
		Private Sub ResetSalutationFormDropDown()

			lueSalutationForm.Properties.DisplayMember = "TranslatedLetterForm"
			lueSalutationForm.Properties.ValueMember = "LetterForm"

			Dim columns = lueSalutationForm.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedLetterForm", 0))

			lueSalutationForm.Properties.DropDownRows = 10
			lueSalutationForm.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSalutationForm.Properties.SearchMode = SearchMode.AutoComplete
			lueSalutationForm.Properties.AutoSearchColumnIndex = 0
			lueSalutationForm.Properties.NullText = String.Empty
			lueSalutationForm.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the advisors drop down.
		''' </summary>
		Private Sub ResetAdvisorDropDown()

			lueAdvisor.Properties.DisplayMember = "FirstName_LastName"
			lueAdvisor.Properties.ValueMember = "KST"

			Dim columns = lueAdvisor.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("KST", 0))
			columns.Add(New LookUpColumnInfo("LastName_FirstName", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

			lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
			lueAdvisor.Properties.AutoSearchColumnIndex = 1

			lueAdvisor.Properties.NullText = String.Empty
			lueAdvisor.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the contact info drop down.
		''' </summary>
		Private Sub ResetContactInfoDropDown()

			lueContactInfo.Properties.DisplayMember = "Description"
			lueContactInfo.Properties.ValueMember = "Description"

			Dim columns = lueContactInfo.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueContactInfo.Properties.DropDownRows = 10
			lueContactInfo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueContactInfo.Properties.SearchMode = SearchMode.AutoComplete
			lueContactInfo.Properties.AutoSearchColumnIndex = 0
			lueContactInfo.Properties.NullText = String.Empty
			lueContactInfo.EditValue = Nothing
		End Sub


		''' <summary>
		''' Resets state1 drop down.
		''' </summary>
		Public Sub ResetState1DropDown()

			lueState1.Properties.DisplayMember = "Description"
			lueState1.Properties.ValueMember = "Description"

			Dim columns = lueState1.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0))

			lueState1.Properties.DropDownRows = 10
			lueState1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueState1.Properties.SearchMode = SearchMode.AutoComplete
			lueState1.Properties.AutoSearchColumnIndex = 0

			lueState1.Properties.NullText = String.Empty
			lueState1.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets state2 drop down.
		''' </summary>
		Public Sub ResetState2DropDown()

			lueState2.Properties.DisplayMember = "Description"
			lueState2.Properties.ValueMember = "Description"

			Dim columns = lueState2.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0))

			lueState2.Properties.DropDownRows = 10
			lueState2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueState2.Properties.SearchMode = SearchMode.AutoComplete
			lueState2.Properties.AutoSearchColumnIndex = 0

			lueState2.Properties.NullText = String.Empty
			lueState2.EditValue = Nothing
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

			lueTermsAndConditions.Properties.DropDownRows = 10
			lueTermsAndConditions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTermsAndConditions.Properties.SearchMode = SearchMode.AutoComplete
			lueTermsAndConditions.Properties.AutoSearchColumnIndex = 0

			lueTermsAndConditions.Properties.NullText = String.Empty
			lueTermsAndConditions.EditValue = Nothing
		End Sub

		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number.</param>
		Private Function LoadResponsiblePersonMasterData(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As Boolean

			Dim responsiblePersonMasterData = m_DataAccess.LoadResponsiblePersonMasterData(customerNumber, recordNumber)

			If (responsiblePersonMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
				Return False
			End If

			SetAddressDataInUI(responsiblePersonMasterData)

			grpPersonalien.Text = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Personalien"), responsiblePersonMasterData.RecordNumber)

			' Personal info
			m_SuppressUIEvents = True
			lueSalutation.EditValue = responsiblePersonMasterData.Salutation
			lueSalutationForm.EditValue = responsiblePersonMasterData.SalutationForm
			m_SuppressUIEvents = False

			txtLastname.Text = responsiblePersonMasterData.Lastname
			txtFirstname.Text = responsiblePersonMasterData.Firstname
			lueDepartment.EditValue = responsiblePersonMasterData.Department
			luePosition.EditValue = responsiblePersonMasterData.Position
			dateEditBirthday.EditValue = responsiblePersonMasterData.Birthdate
			memoEditInterests.Text = responsiblePersonMasterData.Interests

			' Properties
			lueAdvisor.EditValue = responsiblePersonMasterData.Advisor
			lueContactInfo.EditValue = responsiblePersonMasterData.KDZHowKontakt
			lueState1.EditValue = responsiblePersonMasterData.State1
			lueState2.EditValue = responsiblePersonMasterData.State2
			lueTermsAndConditions.EditValue = responsiblePersonMasterData.TermsAndConditions_WOS

			' Communication
			txtTelephone.Text = responsiblePersonMasterData.Telephone
			txtTelefax.Text = responsiblePersonMasterData.Telefax
			txtMobile.Text = responsiblePersonMasterData.MobilePhone
			txtEmail.Text = responsiblePersonMasterData.Email


			If responsiblePersonMasterData.Telefax_Mailing Then
				txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.cancel_16x16
				txtTelefax.Properties.Buttons(0).Tag = 2
			Else
				txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.apply_16x16
				txtTelefax.Properties.Buttons(0).Tag = 1
			End If

			If responsiblePersonMasterData.SMS_Mailing Then
				txtMobile.Properties.Buttons(2).ImageOptions.Image = My.Resources.cancel_16x16
				txtMobile.Properties.Buttons(2).Tag = 2
			Else
				txtMobile.Properties.Buttons(2).ImageOptions.Image = My.Resources.apply_16x16
				txtMobile.Properties.Buttons(2).Tag = 1
			End If

			If responsiblePersonMasterData.Email_Mailing Then
				txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.cancel_16x16
				txtEmail.Properties.Buttons(1).Tag = 2
			Else
				txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.apply_16x16
				txtEmail.Properties.Buttons(1).Tag = 1
			End If





			txtFacebook.Text = responsiblePersonMasterData.Facebook
			txtLinkedIn.Text = responsiblePersonMasterData.LinkedIn
			txtXing.Text = responsiblePersonMasterData.Xing

			'chkTelefaxMailing.Checked = responsiblePersonMasterData.Telefax_Mailing
			'chkSMSMailing.Checked = responsiblePersonMasterData.SMS_Mailing
			'chkEmailMailing.Checked = responsiblePersonMasterData.Email_Mailing

			' Comments
			txtComment.Text = responsiblePersonMasterData.KDZComments

			Return True

		End Function

		''' <summary>
		''' Set the address data in UI.
		''' </summary>
		''' <param name="responsiblePersonMasterData">The rersponsible person data.</param>
		Private Sub SetAddressDataInUI(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData)

			grpAdresse.Text = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Adresse"), responsiblePersonMasterData.CustomerNumber)

			' Address
			txtCompany1.Text = responsiblePersonMasterData.Company1
			txtPostOfficeBox.Text = responsiblePersonMasterData.PostOfficeBox
			txtStreet.Text = responsiblePersonMasterData.Street
			lueCountry.EditValue = responsiblePersonMasterData.CountryCode
			m_SuppressUIEvents = True

			' Add postcode if its not in list of known postcodes.
			Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

			If Not String.IsNullOrEmpty(responsiblePersonMasterData.Postcode) AndAlso
				Not listOfPostcode.Any(Function(postcode) postcode.Postcode = responsiblePersonMasterData.Postcode) Then
				Dim newPostcode As New PostCodeData With {.Postcode = responsiblePersonMasterData.Postcode}
				listOfPostcode.Add(newPostcode)
			End If
			luePostcode.EditValue = responsiblePersonMasterData.Postcode

			m_SuppressUIEvents = False
			txtLocation.Text = responsiblePersonMasterData.Location

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
			End If
		End Sub

		''' <summary>
		''' Handles change of salutation.
		''' </summary>
		Private Sub OnLueSalutation_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueSalutation.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not m_SalutationData Is Nothing AndAlso
				Not lueSalutation.EditValue Is Nothing Then

				Dim salutationData = m_SalutationData.Where(Function(data) data.Salutation = lueSalutation.EditValue).FirstOrDefault()

				If Not salutationData Is Nothing Then
					m_SuppressUIEvents = True
					lueSalutationForm.EditValue = salutationData.LetterForm
					m_SuppressUIEvents = False
				End If

			End If

		End Sub

		''' <summary>
		''' Handles change of letter salutation.
		''' </summary>
		Private Sub OnLueSalutationForm_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueSalutationForm.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not m_SalutationData Is Nothing AndAlso
				Not lueSalutationForm.EditValue Is Nothing Then

				Dim salutationData = m_SalutationData.Where(Function(data) data.LetterForm = lueSalutationForm.EditValue).FirstOrDefault()

				If Not salutationData Is Nothing Then
					m_SuppressUIEvents = True
					lueSalutation.EditValue = salutationData.Salutation
					m_SuppressUIEvents = False
				End If

			End If

		End Sub

		''' <summary>
		''' Handles change of postcode.
		''' </summary>
		Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

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

		''' <summary>
		''' Handles click on open telephone button.
		''' </summary>
		Private Sub OntxtTelephone_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTelephone.ButtonClick
			'Const ModuleNumberResponsiblePersonManagement As Integer = 3

			If Not String.IsNullOrWhiteSpace(txtTelephone.Text) Then
				OpenTelephone(txtTelephone.Text) ', 0, m_CustomerNumber, m_RecordNumber, 0, ModuleNumberResponsiblePersonManagement, 0)
			End If
		End Sub

		Private Sub OntxtTelefax_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTelefax.ButtonClick

			Const ID_OF_SHOW_HIDE_BUTTON As Int32 = 0
			If e.Button.Index = ID_OF_SHOW_HIDE_BUTTON Then
				If txtTelefax.Properties.Buttons(0).Tag = 2 Then
					txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.apply_16x16
					txtTelefax.Properties.Buttons(0).Tag = 1
				Else
					txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.cancel_16x16
					txtTelefax.Properties.Buttons(0).Tag = 2
				End If

				Return
			End If

		End Sub

		''' <summary>
		''' Sends an sms message. 
		''' </summary>
		Private Sub OntxtMobile_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMobile.ButtonClick
			'TODO: should be change soon as ecall-sms finished!
			Dim MobileKind As Integer = 1

			Const ID_OF_SMS_BUTTON As Int32 = 0
			Const ID_OF_RUN_BUTTON As Int32 = 1
			Const ID_OF_SHOW_HIDE_BUTTON As Int32 = 2
			If e.Button.Index = ID_OF_SHOW_HIDE_BUTTON Then
				If txtMobile.Properties.Buttons(2).Tag = 2 Then
					txtMobile.Properties.Buttons(2).ImageOptions.Image = My.Resources.apply_16x16
					txtMobile.Properties.Buttons(2).Tag = 1
				Else
					txtMobile.Properties.Buttons(2).ImageOptions.Image = My.Resources.cancel_16x16
					txtMobile.Properties.Buttons(2).Tag = 2
				End If

				Return
			End If


			If String.IsNullOrWhiteSpace(txtMobile.Text) Or Not m_RecordNumber.HasValue Then Exit Sub
			If e.Button.Index = ID_OF_SMS_BUTTON Then
				OpeneCallSMS(m_InitializationData, txtMobile.Text, Nothing, MobileKind, m_CustomerNumber, m_RecordNumber)

			ElseIf e.Button.Index = ID_OF_RUN_BUTTON Then
				OpenTelephone(txtMobile.Text)

			End If

		End Sub

		''' <summary>
		''' Handles click on open email button.
		''' </summary>
		Private Sub OnTxtEmail_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtEmail.ButtonClick

			Const ID_OF_SHOW_HIDE_BUTTON As Int32 = 1
			If e.Button.Index = ID_OF_SHOW_HIDE_BUTTON Then
				If txtEmail.Properties.Buttons(1).Tag = 2 Then
					txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.apply_16x16
					txtEmail.Properties.Buttons(1).Tag = 1
				Else
					txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.cancel_16x16
					txtEmail.Properties.Buttons(1).Tag = 2
				End If

				Return
			End If

			If Not String.IsNullOrWhiteSpace(txtEmail.Text) Then
				m_UtilityUI.OpenEmail(txtEmail.Text)
				Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {
														 .MDData = m_InitializationData.MDData, .ProsonalizedData = m_InitializationData.ProsonalizedData,
														 .TranslationData = m_InitializationData.TranslationData, .UserData = m_InitializationData.UserData})

				obj.NewResponsiblePersonContact(m_CustomerNumber, txtEmail.EditValue,
															 String.Empty, m_RecordNumber,
															 Now, m_InitializationData.UserData.UserFullName, Nothing,
															 Now, m_InitializationData.UserData.UserFullName, Nothing, Nothing, "Einzelmail", 1, False, True,
															 False)

			End If

		End Sub

		''' <summary>
		''' Handles click on open xing button.
		''' </summary>
		Private Sub OnTxtXing_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtXing.ButtonClick
			If Not String.IsNullOrWhiteSpace(txtXing.Text) Then
				m_UtilityUI.OpenURL(txtXing.Text)
			End If
		End Sub

		''' <summary>
		''' Handles click on open facebook button.
		''' </summary>
		Private Sub OnTxtFacebook_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFacebook.ButtonClick
			If Not String.IsNullOrWhiteSpace(txtFacebook.Text) Then
				m_UtilityUI.OpenURL(txtFacebook.Text)
			End If
		End Sub

		''' <summary>
		''' Handles RefreshResponsiblePersonAddress message.
		''' </summary>
		''' <param name="msg">The  message.</param>
		Private Sub HandleRefreshResponsiblePersonAddress(ByVal msg As RefreshResponsiblePersonAddress)

			If Not IsResponsiblePersonDataLoaded Or Not (m_InitializationData.MDData.MDNr = msg.MDNr And
																									 CustomerNumber = msg.CustomerNumber) Then
				Return
			End If

			Dim responsiblePersonMasterData = m_DataAccess.LoadResponsiblePersonMasterData(CustomerNumber, RecordNumber)

			If (responsiblePersonMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Adressdaten konnten nicht aktualisiert werden."))
				Return
			End If

			SetAddressDataInUI(responsiblePersonMasterData)

		End Sub


		Private Sub OpenTelephone(ByVal number As String)

			Dim oMyProg As New SPSTapi.UI.frmCaller(m_InitializationData)

			oMyProg.LoadData(number)
			oMyProg.Show()
			oMyProg.BringToFront()

		End Sub

		Private Sub OpeneCallSMS(ByVal InitalData As SP.Infrastructure.Initialization.InitializeClass,
														 ByVal number As String, ByVal EmployeeNumber As Integer?, ByVal mobilekind As Integer,
														 ByVal CustomerNumber As Integer?,
														 ByVal ResponiblePersonNumber As Integer?)

			Dim continueSending As Boolean = True
			Dim sql As String = String.Empty

			Try
				Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = InitalData.MDData, .PersonalizedData = InitalData.ProsonalizedData, .TranslationData = InitalData.TranslationData, .UserData = InitalData.UserData}

				If txtMobile.Properties.Buttons(2).Tag = 2 Then
					Dim msg As String = "Achtung: Sie haben die Funktionalität für den SMS-Versand über Listing ausgeschaltet. Möchten Sie dennoch an die zuständige Person eine SMS-Nachricht senden?"
					continueSending = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("SMS-Versand"))

				End If
				If Not continueSending Then Return

				If CustomerNumber.HasValue Then
					sql = "SELECT KD.KDNr, z.RecNr As zhdrecnr, KD.Firma1, "
					sql &= "z.Anredeform AS Anredeform, z.Nachname, z.Vorname, "
					sql &= "z.Strasse, z.Land, z.PLZ, z.Ort, "
					'sql &= "( "
					'sql &= "Case z.zhd_SMS_Mailing "
					sql &= "z.Natel As Natel "
					sql &= "FROM Kunden KD "
					sql &= "Left Join KD_Zustaendig z On KD.KDNr = z.KDNr "
					sql &= "WHERE (z.Natel <> '' And z.Natel Is Not Null ) "
					sql &= "And KD.KDNr = {0} And z.RecNr = {1}"

					sql = String.Format(sql, CustomerNumber, ResponiblePersonNumber)

					Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, sql, SPS.Export.Listing.Utility.ReceiverType.Customer)
					frmSMS2eCall.LoadData()

					frmSMS2eCall.Show()
					frmSMS2eCall.BringToFront()

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub


#End Region

#Region "View helper classes"

		''' <summary>
		''' Advisor view data.
		''' </summary>
		Private Class AdvisorViewData

			Public Property KST As String
			Public Property FristName As String
			Public Property LastName As String

			Public ReadOnly Property LastName_FirstName As String
				Get
					Return String.Format("{0}, {1}", LastName, FristName)
				End Get
			End Property

			Public ReadOnly Property FirstName_LastName As String
				Get
					Return String.Format("{0} {1}", FristName, LastName)
				End Get
			End Property

		End Class

#End Region

	End Class

End Namespace
