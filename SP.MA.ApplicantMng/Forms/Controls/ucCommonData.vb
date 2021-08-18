Imports DevExpress.XtraEditors.Controls

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable


Namespace UI

	Public Class ucCommonData

#Region "Private Consts"

		Private Const DEFAULT_SALUTATION_CODE_MALE = "Herr"
		Private Const DEFAULT_SALUTATION_CODE_FEMALE = "Frau"

		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

#End Region

#Region "Private Fields"

		Private m_SalutationData As IEnumerable(Of SalutationData)

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		Private m_ExistsESToShow As Boolean
		Private m_ExistsProposeToShow As Boolean
		Private m_ESNumber As Integer?
		Private m_ProposeNumber As Integer?

		Private m_ApplicationUtilWebServiceUri As String

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()
			m_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI

			Try
				m_ProgPath = New ClsProgPath
				m_Mandant = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			AddHandler txtLastname.ButtonClick, AddressOf changeLastname_ButtonClick
			AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCanton.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditBirthday.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueGender.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueNationality.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCivilstate.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueState1.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueState2.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueContactInfo.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisor.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLanguage.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueSalutation.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLetterSalutation.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected country code.
		''' </summary>
		''' <returns>The selected country code.</returns>
		Public ReadOnly Property SelectedCountryCode As String
			Get
				Return lueCountry.EditValue
			End Get
		End Property

		''' <summary>
		''' Gets the selected nationality.
		''' </summary>
		''' <returns>The selected nationality.</returns>
		Public ReadOnly Property SelectedNationality As String
			Get
				Return lueNationality.EditValue
			End Get
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			If (Not IsIntialControlDataLoaded) Then
				success = success AndAlso LoadDropDownData()
				IsIntialControlDataLoaded = True
			End If

			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				success = success AndAlso LoadEmployeeData(employeeNumber)
				m_EmployeeNumber = IIf(success, employeeNumber, 0)
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

			m_EmployeeNumber = Nothing

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Address
			txtLastname.Text = String.Empty
			txtLastname.Properties.MaxLength = 255

			txtFirstname.Text = String.Empty
			txtFirstname.Properties.MaxLength = 255

			txtPostOfficeBox.Text = String.Empty
			txtPostOfficeBox.Properties.MaxLength = 70

			txtStreet.Text = String.Empty
			txtStreet.Properties.MaxLength = 70

			txtLocation.Text = String.Empty
			txtLocation.Properties.MaxLength = 70

			dateEditBirthday.EditValue = Nothing
			lblAge.Text = String.Empty

			' Communication
			txtTelephoneP.Text = String.Empty
			txtTelephoneP.Properties.MaxLength = 70

			txtMobilePhone.Text = String.Empty
			txtMobilePhone.Properties.MaxLength = 70

			txtEmail.Text = String.Empty
			txtEmail.Properties.MaxLength = 70

			txtXing.Text = String.Empty
			txtXing.Properties.MaxLength = 255

			txtFacebook.Text = String.Empty
			txtFacebook.Properties.MaxLength = 255
			txtLinkedIn.Text = String.Empty
			txtLinkedIn.Properties.MaxLength = 255

			txtFacebook.Visible = False
			txtLinkedIn.Location = txtFacebook.Location
			txtLinkedIn.Visible = True
			btnKommunikationArt.Text = "LinkedIn"
			BuildCommunicationDropDown()


			'Qualification
			txtQualification.Text = String.Empty
			txtQualification.Tag = Nothing
			txtQualification.Properties.MaxLength = 70

			' Comments
			txtComment.Text = String.Empty
			txtComment.Properties.MaxLength = 4000

			'  Reset drop downs and lists

			ResetCountryDropDown()
			ResetPostcodeDropDown()
			ResetCantonDropDown()
			ResetGenderDropDown()
			ResetNationalityDropDown()
			ResetCivilStateDropDown()
			ResetEmployeeStates1DropDown()
			ResetEmployeeStates2DropDown()
			ResetContactInfoDataDropDown()
			ResetMandantDropDown()
			ResetAdvisorDropDown()
			ResetBusinessBranchesDropDown()
			ResetLanguageDropDown()

			ResetSalutationDropDown()
			ResetSalutationFormDropDown()

			m_SuppressUIEvents = suppressUIEventsState

			' is wos allowed in mandant
			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim bAllowedWOS As Boolean = m_md.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year)
			Dim bAllowedChangedAdvisor As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 133, m_InitializationData.MDData.MDNr)
			Dim userSec124 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 124, m_InitializationData.MDData.MDNr)

			Dim userSec200000 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 200000, m_InitializationData.MDData.MDNr)
			Dim userSec675 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)

			lueMandant.Enabled = userSec200000
			lueAdvisor.Enabled = bAllowedChangedAdvisor
			txtQualification.Properties.Buttons(0).Enabled = userSec124

			lueBusinessBranches.Enabled = userSec675


			errorProvider.Clear()
		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			errorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(txtLastname, errorProvider, String.IsNullOrEmpty(txtLastname.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(txtFirstname, errorProvider, String.IsNullOrEmpty(txtFirstname.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(txtStreet, errorProvider, String.IsNullOrEmpty(txtStreet.Text) AndAlso String.IsNullOrEmpty(txtStreet.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueCountry, errorProvider, lueCountry.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueCountry.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(luePostcode, errorProvider, luePostcode.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(txtLocation, errorProvider, String.IsNullOrEmpty(txtLocation.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueLanguage, errorProvider, lueLanguage.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(dateEditBirthday, errorProvider, dateEditBirthday.EditValue Is Nothing OrElse Not DateTime.TryParse(dateEditBirthday.EditValue, Nothing), errorText)
			isValid = isValid And SetErrorIfInvalid(lueGender, errorProvider, lueGender.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(lueNationality, errorProvider, lueNationality.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueNationality.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueCivilstate, errorProvider, lueCivilstate.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueCivilstate.Text), errorText)

			Dim mdNr = m_InitializationData.MDData.MDNr

			' Advisor
			Dim mustAdvisorBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
																																String.Format("{0}/emplyoeeadvisorselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			If mustAdvisorBeSelected Then
				isValid = isValid And SetErrorIfInvalid(lueAdvisor, errorProvider, lueAdvisor.EditValue Is Nothing, errorText)
			End If

			' Qualification and qualification country
			Dim mustQualificationBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
																																			 String.Format("{0}/emplyoeequalificationselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			If mustQualificationBeSelected Then
				isValid = isValid And SetErrorIfInvalid(txtQualification, errorProvider, String.IsNullOrEmpty(txtQualification.Text), errorText)
			End If

			Return isValid

		End Function

		''' <summary>
		''' Merges the employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
		''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
		Public Overrides Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
			If ((IsEmployeeDataLoaded AndAlso
					m_EmployeeNumber = employeeMasterData.EmployeeNumber) Or forceMerge) Then

				' Address
				employeeMasterData.Lastname = txtLastname.Text
				employeeMasterData.Firstname = txtFirstname.Text
				employeeMasterData.PostOfficeBox = txtPostOfficeBox.Text
				employeeMasterData.Street = txtStreet.Text
				employeeMasterData.Country = lueCountry.EditValue
				employeeMasterData.Postcode = luePostcode.EditValue

				Dim geoData = PerformGeoDataWebservice(employeeMasterData.Country, employeeMasterData.Postcode)
				If Not geoData Is Nothing Then
					employeeMasterData.Latitude = geoData.Latitude
					employeeMasterData.Longitude = geoData.Longitude
				Else
					m_Logger.LogWarning(String.Format("postcode could not be founded: {0} >>> {1}", employeeMasterData.Country, employeeMasterData.Postcode))
					employeeMasterData.Latitude = 0
					employeeMasterData.Longitude = 0
				End If

				employeeMasterData.Location = txtLocation.Text
				employeeMasterData.MA_Canton = lueCanton.EditValue
				employeeMasterData.Birthdate = dateEditBirthday.EditValue
				employeeMasterData.Gender = lueGender.EditValue
				employeeMasterData.Nationality = lueNationality.EditValue
				employeeMasterData.CivilStatus = lueCivilstate.EditValue

				' Communication
				employeeMasterData.Telephone_P = txtTelephoneP.Text
				employeeMasterData.MobilePhone = txtMobilePhone.Text
				employeeMasterData.Email = txtEmail.Text


				If txtMobilePhone.Properties.Buttons(2).Tag = 2 Then
					employeeMasterData.MA_SMS_Mailing = True
				Else
					employeeMasterData.MA_SMS_Mailing = False
				End If

				If txtEmail.Properties.Buttons(1).Tag = 2 Then
					employeeMasterData.MA_EMail_Mailing = True
				Else
					employeeMasterData.MA_EMail_Mailing = False
				End If


				employeeMasterData.Xing = txtXing.Text
				employeeMasterData.Facebook = txtFacebook.Text
				employeeMasterData.LinkedIn = txtlinkedin.Text

				' Properties and Attributes
				employeeMasterData.MDNr = lueMandant.EditValue
				employeeMasterData.KST = lueAdvisor.EditValue

				employeeMasterData.Profession = txtQualification.Text
				employeeMasterData.ProfessionCode = If(txtQualification.Tag Is Nothing, Nothing, Convert.ToInt32(txtQualification.Tag))
				employeeMasterData.Language = lueLanguage.EditValue

				' Hints
				employeeMasterData.V_Hint = txtComment.Text

			End If
		End Sub

		''' <summary>
		'''  Merges the employee contact comm data.
		''' </summary>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		Public Overrides Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
			If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeContactCommData.EmployeeNumber) Then

				' Properties and Attributes
				employeeContactCommData.KStat1 = lueState1.EditValue
				employeeContactCommData.KStat2 = lueState2.EditValue
				employeeContactCommData.KontaktHow = lueContactInfo.EditValue
				employeeContactCommData.AnredeForm = lueSalutation.EditValue
				employeeContactCommData.BriefAnrede = lueLetterSalutation.EditValue

			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Group Adresse

			Me.grpadresse.Text = m_Translate.GetSafeTranslationValue(Me.grpadresse.Text)

			Me.lblNachname.Text = m_Translate.GetSafeTranslationValue(Me.lblNachname.Text)
			Me.lblVorname.Text = m_Translate.GetSafeTranslationValue(Me.lblVorname.Text)
			Me.lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
			Me.lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)
			Me.lblland.Text = m_Translate.GetSafeTranslationValue(Me.lblland.Text)
			Me.lblplz.Text = m_Translate.GetSafeTranslationValue(Me.lblplz.Text)
			Me.lblort.Text = m_Translate.GetSafeTranslationValue(Me.lblort.Text)
			Me.lblCanton.Text = m_Translate.GetSafeTranslationValue(Me.lblCanton.Text)
			Me.lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGeburtsdatum.Text)
			Me.lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(Me.lblGeschlecht.Text)
			Me.lblNationalitaet.Text = m_Translate.GetSafeTranslationValue(Me.lblNationalitaet.Text)
			Me.lblZivilstand.Text = m_Translate.GetSafeTranslationValue(Me.lblZivilstand.Text)

			' Group Communication

			Me.grpKommunikation.Text = m_Translate.GetSafeTranslationValue(Me.grpKommunikation.Text)

			Me.lblTelefon_P.Text = m_Translate.GetSafeTranslationValue(Me.lblTelefon_P.Text, True)
			Me.lblNatel.Text = m_Translate.GetSafeTranslationValue(Me.lblNatel.Text, True)
			Me.lblEmail.Text = m_Translate.GetSafeTranslationValue(Me.lblEmail.Text)
			Me.lblxing.Text = m_Translate.GetSafeTranslationValue(Me.lblxing.Text)

			' Group Properties and Attributes

			Me.grpmerkmale.Text = m_Translate.GetSafeTranslationValue(Me.grpmerkmale.Text)
			Me.lbl1status.Text = m_Translate.GetSafeTranslationValue(Me.lbl1status.Text, True)
			Me.lbl2status.Text = m_Translate.GetSafeTranslationValue(Me.lbl2status.Text, True)
			Me.lblkontakt.Text = m_Translate.GetSafeTranslationValue(Me.lblkontakt.Text, True)
			Me.lblmandant.Text = m_Translate.GetSafeTranslationValue(Me.lblmandant.Text)
			Me.lblberater.Text = m_Translate.GetSafeTranslationValue(Me.lblberater.Text)
			Me.lblZugrifffiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblZugrifffiliale.Text)
			Me.lblSprache.Text = m_Translate.GetSafeTranslationValue(Me.lblSprache.Text)

			' Group Anrede
			Me.grpAnrede.Text = m_Translate.GetSafeTranslationValue(Me.grpAnrede.Text)

			' Group Qualifikation
			Me.grpQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.grpQualifikation.Text)
			Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)

			' Group Hints
			Me.grpbemerkung.Text = m_Translate.GetSafeTranslationValue(Me.grpbemerkung.Text)

		End Sub

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

			lueCountry.Properties.ShowHeader = False
			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 20
			lueCountry.Properties.DisplayMember = "Translated_Value"
			lueCountry.Properties.ValueMember = "Code"

			Dim columns = lueCountry.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Land")))

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

			luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePostcode.Properties.SearchMode = SearchMode.AutoComplete
			luePostcode.Properties.AutoSearchColumnIndex = 1
			luePostcode.Properties.NullText = String.Empty
			luePostcode.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the canton drop down.
		''' </summary>
		Private Sub ResetCantonDropDown()

			lueCanton.Properties.DisplayMember = "Description"
			lueCanton.Properties.ValueMember = "GetField"

			Dim columns = lueCanton.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Kanton")))

			lueCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCanton.Properties.SearchMode = SearchMode.AutoComplete
			lueCanton.Properties.AutoSearchColumnIndex = 0
			lueCanton.Properties.NullText = String.Empty
			lueCanton.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the gender drop down.
		''' </summary>
		Private Sub ResetGenderDropDown()

			lueGender.Properties.ShowHeader = False
			lueGender.Properties.ShowFooter = False
			lueGender.Properties.DropDownRows = 10

			lueGender.Properties.DisplayMember = "TranslatedGender"
			lueGender.Properties.ValueMember = "RecValue"

			Dim columns = lueGender.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedGender", 0, ("Geschlecht")))

			lueGender.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueGender.Properties.SearchMode = SearchMode.AutoComplete
			lueGender.Properties.AutoSearchColumnIndex = 0

			lueGender.Properties.NullText = String.Empty
			lueGender.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the nationality drop down.
		''' </summary>
		Private Sub ResetNationalityDropDown()

			lueNationality.Properties.ShowHeader = False
			lueNationality.Properties.ShowFooter = False
			lueNationality.Properties.DropDownRows = 20
			lueNationality.Properties.DisplayMember = "Translated_Value"
			lueNationality.Properties.ValueMember = "Code"

			Dim columns = lueNationality.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Land")))

			lueNationality.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueNationality.Properties.SearchMode = SearchMode.AutoComplete
			lueNationality.Properties.AutoSearchColumnIndex = 0

			lueNationality.Properties.NullText = String.Empty
			lueNationality.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the civil state down.
		''' </summary>
		Private Sub ResetCivilStateDropDown()

			lueCivilstate.Properties.DisplayMember = "TranslatedCivilState"
			lueCivilstate.Properties.ValueMember = "GetField"

			Dim columns = lueCivilstate.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("TranslatedCivilState", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueCivilstate.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCivilstate.Properties.SearchMode = SearchMode.AutoComplete
			lueCivilstate.Properties.AutoSearchColumnIndex = 1
			lueCivilstate.Properties.NullText = String.Empty
			lueCivilstate.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets employee states1 drop down.
		''' </summary>
		Private Sub ResetEmployeeStates1DropDown()

			lueState1.Properties.DisplayMember = "TranslatedState"
			lueState1.Properties.ValueMember = "Description"

			Dim columns = lueState1.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedState", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueState1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueState1.Properties.SearchMode = SearchMode.AutoComplete
			lueState1.Properties.AutoSearchColumnIndex = 0

			lueState1.Properties.NullText = String.Empty
			lueState1.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets employee states2 drop down.
		''' </summary>
		Private Sub ResetEmployeeStates2DropDown()

			lueState2.Properties.DisplayMember = "TranslatedState"
			lueState2.Properties.ValueMember = "Description"

			Dim columns = lueState2.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedState", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueState2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueState2.Properties.SearchMode = SearchMode.AutoComplete
			lueState2.Properties.AutoSearchColumnIndex = 0

			lueState2.Properties.NullText = String.Empty
			lueState2.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets contact info drop down.
		''' </summary>
		Private Sub ResetContactInfoDataDropDown()
			lueContactInfo.Properties.DisplayMember = "TranslatedContactInfoText"
			lueContactInfo.Properties.ValueMember = "Description"

			Dim columns = lueContactInfo.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedContactInfoText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueContactInfo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueContactInfo.Properties.SearchMode = SearchMode.AutoComplete
			lueContactInfo.Properties.AutoSearchColumnIndex = 0

			lueContactInfo.Properties.NullText = String.Empty
			lueContactInfo.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			lueMandant.Properties.ShowHeader = False
			lueMandant.Properties.ShowFooter = False
			lueMandant.Properties.DropDownRows = 10

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"

			Dim columns = lueMandant.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

			lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMandant.Properties.SearchMode = SearchMode.AutoComplete
			lueMandant.Properties.AutoSearchColumnIndex = 0

			lueMandant.Properties.NullText = String.Empty
			lueMandant.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the advisors drop down.
		''' </summary>
		Private Sub ResetAdvisorDropDown()

			lueAdvisor.Properties.DropDownRows = 20

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
		''' Resets the business branches drop down.
		''' </summary>
		Private Sub ResetBusinessBranchesDropDown()

			lueBusinessBranches.Properties.DisplayMember = "Name"
			lueBusinessBranches.Properties.ValueMember = "ID"

			Dim columns = lueBusinessBranches.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueBusinessBranches.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueBusinessBranches.Properties.SearchMode = SearchMode.AutoComplete
			lueBusinessBranches.Properties.AutoSearchColumnIndex = 0

			lueBusinessBranches.Properties.NullText = String.Empty
			lueBusinessBranches.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the language drop down.
		''' </summary>
		Private Sub ResetLanguageDropDown()

			lueLanguage.Properties.ShowHeader = False
			lueLanguage.Properties.ShowFooter = False
			lueLanguage.Properties.DropDownRows = 10

			lueLanguage.Properties.DisplayMember = "TranslatedDescription"
			lueLanguage.Properties.ValueMember = "Description"

			Dim columns = lueLanguage.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedDescription", 0, m_Translate.GetSafeTranslationValue("Sprache")))

			lueLanguage.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueLanguage.Properties.SearchMode = SearchMode.AutoComplete
			lueLanguage.Properties.AutoSearchColumnIndex = 0

			lueLanguage.Properties.NullText = String.Empty
			lueLanguage.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the salutation drop down.
		''' </summary>
		Private Sub ResetSalutationDropDown()

			lueSalutation.Properties.DisplayMember = "TranslatedSalutation"
			lueSalutation.Properties.ValueMember = "Salutation"

			Dim columns = lueSalutation.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedSalutation", 0, ""))

			lueSalutation.Properties.DropDownRows = 10
			lueSalutation.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSalutation.Properties.SearchMode = SearchMode.AutoComplete
			lueSalutation.Properties.AutoSearchColumnIndex = 0
			lueSalutation.Properties.NullText = String.Empty
			lueSalutation.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the salutation form drop down.
		''' </summary>
		Private Sub ResetSalutationFormDropDown()

			lueLetterSalutation.Properties.DisplayMember = "TranslatedLetterForm"
			lueLetterSalutation.Properties.ValueMember = "LetterForm"

			Dim columns = lueLetterSalutation.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedLetterForm", 0, ""))

			lueLetterSalutation.Properties.DropDownRows = 10
			lueLetterSalutation.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueLetterSalutation.Properties.SearchMode = SearchMode.AutoComplete
			lueLetterSalutation.Properties.AutoSearchColumnIndex = 0
			lueLetterSalutation.Properties.NullText = String.Empty
			lueLetterSalutation.EditValue = Nothing
		End Sub

		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadEmployeeMasterData(employeeNumber)
			success = success AndAlso LoadEmployeeContactCommData(employeeNumber)

			errorProvider.Clear()

			Return success
		End Function


		''' <summary>
		'''  Loads the employee master data..
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		Private Function LoadEmployeeMasterData(ByVal employeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(employeeNumber, False)

			If (employeeMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
				Return False
			End If
			m_CVLProfileID = employeeMasterData.CVLProfileID

			' Address
			grpadresse.Text = String.Format(m_Translate.GetSafeTranslationValue("Bewerber: {0}"), employeeMasterData.EmployeeNumber)
			txtLastname.Text = employeeMasterData.Lastname
			txtFirstname.Text = employeeMasterData.Firstname
			txtPostOfficeBox.Text = employeeMasterData.PostOfficeBox
			txtStreet.Text = employeeMasterData.Street
			lueCountry.EditValue = employeeMasterData.Country

			' Add missing post code to drop down
			Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

			If Not String.IsNullOrEmpty(employeeMasterData.Postcode) AndAlso
				Not listOfPostcode.Any(Function(postcode) postcode.Postcode = employeeMasterData.Postcode) Then
				Dim newPostcode As New PostCodeData With {.Postcode = employeeMasterData.Postcode}
				listOfPostcode.Add(newPostcode)
			End If

			luePostcode.EditValue = employeeMasterData.Postcode

			txtLocation.Text = employeeMasterData.Location
			lueCanton.EditValue = employeeMasterData.MA_Canton
			dateEditBirthday.EditValue = employeeMasterData.Birthdate
			RecalculateAge()
			lueGender.EditValue = employeeMasterData.Gender
			lueNationality.EditValue = employeeMasterData.Nationality
			lueCivilstate.EditValue = employeeMasterData.CivilStatus

			' Communication
			txtTelephoneP.Text = employeeMasterData.Telephone_P
			txtMobilePhone.Text = employeeMasterData.MobilePhone
			txtEmail.Text = employeeMasterData.Email

			If (employeeMasterData.MA_SMS_Mailing.HasValue AndAlso employeeMasterData.MA_SMS_Mailing = True) Then
				txtMobilePhone.Properties.Buttons(2).ImageOptions.Image = My.Resources.cancel_16x16
				txtMobilePhone.Properties.Buttons(2).Tag = 2
			Else
				txtMobilePhone.Properties.Buttons(2).ImageOptions.Image = My.Resources.apply_16x16
				txtMobilePhone.Properties.Buttons(2).Tag = 1
			End If

			If (employeeMasterData.MA_EMail_Mailing.HasValue AndAlso employeeMasterData.MA_EMail_Mailing = True) Then
				txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.cancel_16x16
				txtEmail.Properties.Buttons(1).Tag = 2
			Else
				txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.apply_16x16
				txtEmail.Properties.Buttons(1).Tag = 1
			End If


			txtXing.Text = employeeMasterData.Xing
			txtFacebook.Text = employeeMasterData.Facebook
			txtlinkedin.Text = employeeMasterData.LinkedIn

			' Properties and attributes
			lueMandant.EditValue = employeeMasterData.MDNr
			lueAdvisor.EditValue = employeeMasterData.KST
			lueLanguage.EditValue = employeeMasterData.Language

			Select Case employeeMasterData.ApplicantLifecycle.GetValueOrDefault(0)
				Case 0
					lblApplicantLifecycle.Text = m_Translate.GetSafeTranslationValue("Als Bewerber")
				Case 1
					lblApplicantLifecycle.Text = m_Translate.GetSafeTranslationValue("Abgesagt")

				Case Else
					lblApplicantLifecycle.Text = String.Empty
					lbl1status.Text = String.Empty

			End Select

			' Qualification
			txtQualification.Text = employeeMasterData.Profession
			txtQualification.Tag = employeeMasterData.ProfessionCode

			' Comments
			txtComment.Text = employeeMasterData.V_Hint

			m_SuppressUIEvents = suppressUIEventsState

			Return True

		End Function

		''' <summary>
		''' Loads the employee contact comm data.
		''' </summary>
		''' <param name="enmployeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeContactCommData(ByVal enmployeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDataAccess.LoadEmployeeContactCommData(enmployeeNumber)

			If (employeeContactCommData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
				Return False
			End If

			' Properties and Attributes data
			lueState1.EditValue = employeeContactCommData.KStat1
			lueState2.EditValue = employeeContactCommData.KStat2
			lueContactInfo.EditValue = employeeContactCommData.KontaktHow
			lueSalutation.EditValue = employeeContactCommData.AnredeForm
			lueLetterSalutation.EditValue = employeeContactCommData.BriefAnrede

			m_SuppressUIEvents = suppressUIEventsState

			Return True
		End Function

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadCountryDropDownData()
			success = success AndAlso LoadPostcodeDropDownData()
			success = success AndAlso LoadCantonDropDownData()
			success = success AndAlso LoadGenderDropDownData()
			success = success AndAlso LoadNationalityDropDownData()
			success = success AndAlso LoadCivilStateDropDownData()
			success = success AndAlso LoadEmployeeStates1DropDownData()
			success = success AndAlso LoadEmployeeStates2DropDownData()
			success = success AndAlso LoadContactInfoDropDownData()
			success = success AndAlso LoadMandantDropDownData()
			success = success AndAlso LoadAdvisorDropDownData()
			success = success AndAlso LoadBusinessBranchesDropDown()
			success = success AndAlso LoadLanguageDropDownData()
			success = success AndAlso LoadSalutationAndSalutationFormDropDownData()

			Return success
		End Function

		''' <summary>
		''' Loads the country drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
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
		''' Loads the postcode drop downdata.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
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
		''' Loads the canton drop downdata.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadCantonDropDownData() As Boolean
			Dim cantonData = m_CommonDatabaseAccess.LoadCantonData()

			If (cantonData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantondaten konnten nicht geladen werden."))
			End If

			lueCanton.Properties.DataSource = cantonData
			lueCanton.Properties.ForceInitialize()

			Return Not cantonData Is Nothing
		End Function

		''' <summary>
		''' Loads gender drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadGenderDropDownData() As Boolean
			Dim genderData = m_CommonDatabaseAccess.LoadGenderData()

			If (genderData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Geschlechtsdaten konnten nicht geladen werden."))
			End If

			lueGender.Properties.DataSource = genderData
			lueGender.Properties.ForceInitialize()

			Return Not genderData Is Nothing
		End Function

		''' <summary>
		''' Loads nationality drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadNationalityDropDownData() As Boolean
			Dim result As Boolean = True
			Dim countryData As IEnumerable(Of CVLBaseTableViewData) = Nothing

			Try
				Dim baseTable = New SPSBaseTables(m_InitializationData)
				baseTable.BaseTableName = "Country"
				countryData = baseTable.PerformCVLBaseTablelistWebserviceCall()

				If (countryData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Nationalitätsdaten konnten nicht geladen werden."))
				End If

				lueNationality.Properties.DataSource = countryData
				lueNationality.Properties.ForceInitialize()

			Catch ex As Exception
				m_Logger.LogError(String.Format("lueNationality: {0}", ex.ToString))
			End Try
			'Dim countryData = m_CommonDatabaseAccess.LoadCountryData()

			'If (countryData Is Nothing) Then
			'	m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Nationalitätsdaten konnten nicht geladen werden."))
			'End If

			'lueNationality.Properties.DataSource = countryData
			'lueNationality.Properties.ForceInitialize()

			Return Not countryData Is Nothing
		End Function

		''' <summary>
		''' Loads civil state drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadCivilStateDropDownData() As Boolean
			Dim civilStateData = m_CommonDatabaseAccess.LoadCivilStateData()

			If (civilStateData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Zivilstandsdaten konnten nicht geladen werden."))
			End If

			lueCivilstate.Properties.DataSource = civilStateData
			lueCivilstate.Properties.ForceInitialize()

			Return Not civilStateData Is Nothing
		End Function

		''' <summary>
		''' Load employee states1 drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeStates1DropDownData() As Boolean
			Dim employeeStates1 = m_EmployeeDataAccess.LoadEmployeeStateData1()

			If (employeeStates1 Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare 1. Status konnten nicht geladen werden."))
			End If

			lueState1.Properties.DataSource = employeeStates1
			lueState1.Properties.ForceInitialize()

			Return Not employeeStates1 Is Nothing
		End Function

		''' <summary>
		''' Load employee states2 drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeStates2DropDownData() As Boolean
			Dim customerStates2 = m_EmployeeDataAccess.LoadEmployeeStateData2()

			If (customerStates2 Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare 2. Status konnten nicht geladen werden."))
			End If

			lueState2.Properties.DataSource = customerStates2
			lueState2.Properties.ForceInitialize()

			Return Not customerStates2 Is Nothing
		End Function

		''' <summary>
		''' Load contact info drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadContactInfoDropDownData() As Boolean
			Dim contactInfoData = m_EmployeeDataAccess.LoadEmployeeContactsInfo()

			If (contactInfoData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Kontaktarten konnten nicht geladen werden."))
			End If

			lueContactInfo.Properties.DataSource = contactInfoData
			lueContactInfo.Properties.ForceInitialize()

			Return Not contactInfoData Is Nothing
		End Function

		''' <summary>
		''' Loads the mandant down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return Not mandantData Is Nothing
		End Function

		''' <summary>
		''' Loads the advisor drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorDropDownData() As Boolean

			Dim userDataList = m_CommonDatabaseAccess.LoadAdvisorData()

			Dim advisorViewDataList As New List(Of AdvisorViewData)

			If Not userDataList Is Nothing Then
				For Each userData In userDataList
					Dim advisorViewData As AdvisorViewData = New AdvisorViewData
					advisorViewData.KST = userData.KST
					advisorViewData.FristName = userData.Firstname
					advisorViewData.LastName = userData.Lastname

					advisorViewDataList.Add(advisorViewData)
				Next
			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
			End If

			lueAdvisor.Properties.DataSource = advisorViewDataList
			lueAdvisor.Properties.ForceInitialize()

			Return Not userDataList Is Nothing
		End Function

		''' <summary>
		''' Loads the business branches drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadBusinessBranchesDropDown() As Boolean

			Dim availableBusinessBranches = m_CommonDatabaseAccess.LoadBusinessBranchsData()

			If (availableBusinessBranches Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Filialen konnten nicht geladen werden."))
			End If

			lueBusinessBranches.Properties.DataSource = availableBusinessBranches
			lueBusinessBranches.Properties.ForceInitialize()

			Return Not availableBusinessBranches Is Nothing
		End Function

		''' <summary>
		''' Loads language drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadLanguageDropDownData() As Boolean
			Dim languageData = m_CommonDatabaseAccess.LoadLanguageData()

			If (languageData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Sprachen konnten nicht geladen werden."))
			End If

			lueLanguage.Properties.DataSource = languageData
			lueLanguage.Properties.ForceInitialize()

			Return Not languageData Is Nothing
		End Function

		''' <summary>
		''' Loads the salutation and salutation form drop downdata.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadSalutationAndSalutationFormDropDownData() As Boolean
			m_SalutationData = m_CommonDatabaseAccess.LoadSalutationData()

			If (m_SalutationData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(m_Translate.GetSafeTranslationValue("Anredeformen konnten nicht geladen werden.")))
			End If

			lueSalutation.Properties.DataSource = m_SalutationData
			lueSalutation.Properties.ForceInitialize()

			lueLetterSalutation.Properties.DataSource = m_SalutationData
			lueLetterSalutation.Properties.ForceInitialize()

			Return Not m_SalutationData Is Nothing
		End Function

		''' <summary>
		''' Handles change of country code.
		''' </summary>
		Private Sub OnLueCountry_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCountry.EditValueChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			m_UCMediator.CountryCodeHasChanged(m_EmployeeNumber)

		End Sub

		''' <summary>
		''' Handles edit value change of postcode.
		''' </summary>
		Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If luePostcode.EditValue Is Nothing Then
				lueCanton.EditValue = Nothing
			Else

				Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)

				If Not postCodeData Is Nothing Then
					txtLocation.Text = postCodeData.Location
					lueCanton.EditValue = postCodeData.Canton
				End If

			End If
		End Sub

		''' <summary>
		''' Handles new value event on postcode(plz) lookup edit.
		''' </summary>
		Private Sub OnLuePostcode_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles luePostcode.ProcessNewValue

			If Not luePostcode.Properties.DataSource Is Nothing AndAlso Not String.IsNullOrWhiteSpace(e.DisplayValue.ToString()) Then

				Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

				Dim newPostcode As New PostCodeData With {.Postcode = e.DisplayValue.ToString()}
				listOfPostcode.Add(newPostcode)

				e.Handled = True
			End If
		End Sub

		''' <summary>
		''' Handles leave event of birthdate control.
		''' </summary>
		Private Sub OnDateEditBirthday_Leave(sender As System.Object, e As System.EventArgs) Handles dateEditBirthday.Leave
			RecalculateAge()
		End Sub

		''' <summary>
		''' Handles change of nationality.
		''' </summary>
		Private Sub OnLueNationality_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueNationality.EditValueChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			m_UCMediator.NationalityHasChanged(m_EmployeeNumber)

		End Sub

		''' <summary>
		''' Handles change of gender.
		''' </summary>
		Private Sub OnLueGender_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueGender.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not m_SalutationData Is Nothing AndAlso
				Not lueGender.EditValue Is Nothing Then

				Dim gender As String = lueGender.EditValue

				Select Case gender.ToUpper()
					Case "M"
						lueSalutation.EditValue = DEFAULT_SALUTATION_CODE_MALE
					Case "W"
						lueSalutation.EditValue = DEFAULT_SALUTATION_CODE_FEMALE
					Case Else
						' Do nothing

				End Select
			End If

		End Sub

		''' <summary>
		''' Handles click on open telephone button.
		''' </summary>
		Private Sub OntxtTelephone_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTelephoneP.ButtonClick
			Dim phonenumber As String = sender.text
			If Not m_EmployeeNumber.HasValue Then Exit Sub

			'Const ModuleNumberCustomerManagement As Integer = 1
			If Not String.IsNullOrWhiteSpace(phonenumber) Then
				OpenTelephone(phonenumber) ', m_EmployeeNumber, 0, 0, 0, ModuleNumberCustomerManagement, 0)
			End If
		End Sub

		''' <summary>
		''' Handles click on one of the open mobile telephone buttons.
		''' </summary>
		Private Sub OntxtMobile_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtMobilePhone.ButtonClick
			Dim phonenumber As String = sender.text
			Dim MobileKind As Integer = If(sender.name.ToString.ToLower.Contains("txtMobilePhone2".ToLower), 2, 1)

			Const ID_OF_SEND_SMS_BUTTON As Int32 = 0
			Const ID_OF_CALL_BUTTON As Int32 = 1
			Const ID_OF_SHOW_HIDE_BUTTON As Int32 = 2
			If e.Button.Index = ID_OF_SHOW_HIDE_BUTTON Then
				If txtMobilePhone.Properties.Buttons(2).Tag = 2 Then
					txtMobilePhone.Properties.Buttons(2).ImageOptions.Image = My.Resources.apply_16x16
					txtMobilePhone.Properties.Buttons(2).Tag = 1
				Else
					txtMobilePhone.Properties.Buttons(2).ImageOptions.Image = My.Resources.cancel_16x16
					txtMobilePhone.Properties.Buttons(2).Tag = 2
				End If

				Return
			End If

			If Not m_EmployeeNumber.HasValue Then Return
			If Not String.IsNullOrWhiteSpace(phonenumber) Then
				If e.Button.Index = ID_OF_SEND_SMS_BUTTON Then
					OpeneCallSMS(m_InitializationData, phonenumber, m_EmployeeNumber, MobileKind, Nothing, Nothing)

				ElseIf e.Button.Index = ID_OF_CALL_BUTTON Then
					OpenTelephone(phonenumber)

				End If
			End If

		End Sub

		''' <summary>
		''' Handles click on email button.
		''' </summary>
		Private Sub OnTxtEmail_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtEmail.ButtonClick
			If Not String.IsNullOrWhiteSpace(txtEmail.Text) Then
				Dim result As Boolean

				Const ID_OF_SHOW_HIDE_BUTTON As Int32 = 1

				' If delete button has been clicked reset the drop down.
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

				Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {
														 .MDData = m_InitializationData.MDData, .ProsonalizedData = m_InitializationData.ProsonalizedData,
														 .TranslationData = m_InitializationData.TranslationData, .UserData = m_InitializationData.UserData})

				m_UtilityUI.OpenEmail(txtEmail.Text)
				result = obj.NewEmployeeContact(m_EmployeeNumber, txtEmail.Text,
															 String.Empty,
															 "Einzelmail", 1, Now, False, True,
															 Now, m_InitializationData.UserData.UserFullName)
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

		Private Sub OnlabelESState_Click(sender As Object, e As EventArgs)
			m_ExistsESToShow = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 250, m_InitializationData.MDData.MDNr)

			If m_ExistsESToShow AndAlso m_ESNumber.GetValueOrDefault(0) > 0 Then
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_ESNumber)
				hub.Publish(openMng)
			End If

		End Sub

		Private Sub OnlabelProposeState_Click(sender As Object, e As EventArgs)
			m_ExistsProposeToShow = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 801, m_InitializationData.MDData.MDNr)

			If m_ExistsProposeToShow AndAlso m_ProposeNumber.GetValueOrDefault(0) > 0 Then
				Dim obj As New SPProposeUtility.ClsMain_Net(m_InitializationData)
				obj.ShowfrmProposal(m_ProposeNumber)
			End If

		End Sub

		''' <summary>
		''' Handles click on qualification button.
		''' </summary>
		Private Sub OnTxtQulification_ButtonClick(sender As System.Object, e As System.EventArgs) Handles txtQualification.ButtonClick
			' Show profession selection dialog.
			Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
			obj.SelectMultirecords = False
			Dim success = True

			success = success AndAlso obj.LoadQualificationData("M")
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedProfessionsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return

			' Tokenize the result string.
			' Result string has the following format <ProfessionCode>#<ProfessionDescription>
			Dim tokens As String() = selectedProfessionsString.Split("#")

			' It must be an even number of tokens -> otherwhise something is wrong
			If tokens.Count Mod 2 = 0 Then
				txtQualification.Tag = tokens(0)
				txtQualification.Text = tokens(1)
			End If

		End Sub

		''' <summary>
		''' Handles change of available business branch.
		''' </summary>
		Private Sub OnLueAvailableBusinessBranch_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueBusinessBranches.EditValueChanged

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			Dim businessBranchAdded = False

			Dim selectedBusinessBranchData As AvilableBusinessBranchData = TryCast(lueBusinessBranches.GetSelectedDataRow(), AvilableBusinessBranchData)

			If (Not selectedBusinessBranchData Is Nothing) Then

				' Load the already assigned business branches
				Dim customerBusinessBranchData = m_EmployeeDataAccess.LoadEmployeeBusinessBranches(m_EmployeeNumber)

				' Check if the business branch is already assigned.
				If (Not customerBusinessBranchData Is Nothing AndAlso
						Not customerBusinessBranchData.Any(Function(data) data.Description.ToLower().Trim() = selectedBusinessBranchData.Name.ToLower().Trim())) Then

					' Add to database
					Dim businessBranchAssignmentToInsert = New EmployeeBusinessBranchData With {.EmployeeNumber = m_EmployeeNumber, .MDNr = selectedBusinessBranchData.Code_1, .Description = selectedBusinessBranchData.Name}
					businessBranchAdded = m_EmployeeDataAccess.AddEmployeeBussinessBranch(businessBranchAssignmentToInsert)
				End If

			End If

		End Sub

		''' <summary>
		''' Handles change of salutation.
		''' </summary>
		Private Sub OnSalutation_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueSalutation.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not m_SalutationData Is Nothing AndAlso
				Not lueSalutation.EditValue Is Nothing Then

				Dim salutationData = m_SalutationData.Where(Function(data) data.Salutation = lueSalutation.EditValue).FirstOrDefault()

				If Not salutationData Is Nothing Then
					m_SuppressUIEvents = True
					lueLetterSalutation.EditValue = salutationData.LetterForm
					m_SuppressUIEvents = False
				End If

			End If

		End Sub

		''' <summary>
		''' Handles change of letter salutation.
		''' </summary>
		Private Sub OnLetterSalutation_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueLetterSalutation.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not m_SalutationData Is Nothing AndAlso
				Not lueLetterSalutation.EditValue Is Nothing Then

				Dim salutationData = m_SalutationData.Where(Function(data) data.LetterForm = lueLetterSalutation.EditValue).FirstOrDefault()

				If Not salutationData Is Nothing Then
					m_SuppressUIEvents = True
					lueSalutation.EditValue = salutationData.Salutation
					m_SuppressUIEvents = False
				End If

			End If

		End Sub

		''' <summary>
		''' Handles change value down button clicks.
		''' </summary>
		Private Sub changeLastname_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_CHANGE_BUTTON As Int32 = 0

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_CHANGE_BUTTON Then

				If TypeOf sender Is ComboBoxEdit Then
					Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
					If (employeeMasterData Is Nothing) Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
						Return
					End If
					Dim comboEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
					If comboEdit.EditValue = employeeMasterData.Firstname Then
						comboEdit.EditValue = employeeMasterData.Lastname
						txtFirstname.EditValue = employeeMasterData.Firstname
					Else
						comboEdit.EditValue = employeeMasterData.Firstname
						txtFirstname.EditValue = employeeMasterData.Lastname
					End If

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
				ElseIf TypeOf sender Is DateEdit Then
					Dim dateEdit As DateEdit = CType(sender, DateEdit)
					dateEdit.EditValue = Nothing
				End If
			End If
		End Sub

		''' <summary>
		''' Recalculates the age.
		''' </summary>
		Private Sub RecalculateAge()
			If dateEditBirthday.EditValue Is Nothing Then
				lblAge.Text = String.Empty
			Else
				lblAge.Text = GetAge(dateEditBirthday.EditValue)
			End If

		End Sub

		''' <summary>
		''' Gets the age in years.
		''' </summary>
		''' <param name="birthDate">The birthdate.</param>
		''' <returns>Age in years.</returns>
		Private Function GetAge(ByVal birthDate As DateTime)

			' Get year diff
			Dim years As Integer = DateTime.Now.Year - birthDate.Year

			birthDate = birthDate.AddYears(years)

			' Subtract another year if its a day before the the birth day
			If (DateTime.Today.CompareTo(birthDate) < 0) Then
				years = years - 1
			End If

			Return years

		End Function

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

			Dim sql As String = String.Empty

			Try
				Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = InitalData.MDData,
																																					 .PersonalizedData = InitalData.ProsonalizedData,
																																					 .TranslationData = InitalData.TranslationData,
																																					 .UserData = InitalData.UserData}

				If EmployeeNumber.HasValue Then
					sql = "Select MA.MANr, MA.Nachname As Nachname, MA.Vorname As Vorname, "
					sql &= "MA.Strasse, MA.Land, MA.PLZ, MA.Ort, "
					sql &= "( Case MA.MA_SMS_Mailing "
					sql &= "When 0 Then MA.Natel{0} Else '' End) As Natel, "
					sql &= "ma.geschlecht AS Geschlecht, "
					sql &= "mak.briefanrede AS Anredeform "
					sql &= "From Mitarbeiter MA "
					sql &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = ma.manr "
					sql &= "Where (MA.Natel{0} <> '' And MA.Natel{0} Is Not Null ) "
					sql &= "And MA.MANr = {1}"

					sql = String.Format(sql, If(mobilekind = 1, "", "2"), EmployeeNumber)

					Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, sql, SPS.Export.Listing.Utility.ReceiverType.Employee)
					frmSMS2eCall.LoadData()

					frmSMS2eCall.Show()
					frmSMS2eCall.BringToFront()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Private Function PerformGeoDataWebservice(ByVal countryCode As String, ByVal postCode As String) As GeoCoordinateDataViewData
			Dim result As New GeoCoordinateDataViewData

			Try
				Dim baseTable = New SPSBaseTables(m_InitializationData)
				result = baseTable.PerformGeoDataWebservice(countryCode, postCode)

			Catch ex As Exception
				m_Logger.LogError(String.Format("geo data could not be loaded from webservice! {0} | {1}", m_InitializationData.MDData.MDGuid, postCode))

				Return Nothing
			End Try

			Return result
		End Function

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

		Protected Overrides Sub Finalize()
			MyBase.Finalize()
		End Sub


	End Class

End Namespace
