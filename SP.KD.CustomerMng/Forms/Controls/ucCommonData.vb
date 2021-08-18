
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
Imports SP.Infrastructure

Imports SPProgUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.KD.KontaktMng
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable
Imports DevExpress.XtraBars.Navigation

Namespace UI

	''' <summary>
	''' Common data.
	''' </summary>
	Public Class ucCommonData


#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"

#End Region

#Region "Private Fields"
		''' <summary>
		''' Boolean flag indicating if drop down data has been loaded.
		''' </summary>
		Private m_HasDropDownDataBeenLoaded As Boolean = False

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		Private m_IsAuthorizedForCode_216 As Boolean = False
		Private m_IsAuthorizedForCode_222 As Boolean = False
		Private m_IsAuthorizedForCode_223 As Boolean = False
		Private m_IsAuthorizedForCode_224 As Boolean = False
		Private m_IsAuthorizedForCode_675 As Boolean = False
		Private m_IsAuthorizedForCode_200001 As Boolean = False

		Private m_NotificationUtilWebServiceUri As String

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()
			m_NotificationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI

			lueMandant.Properties.ShowHeader = False
			lueMandant.Properties.ShowFooter = False
			lueMandant.Properties.DropDownRows = 10

			lueCountry.Properties.ShowHeader = False
			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 50

			lueState1.Properties.ShowHeader = False
			lueState1.Properties.ShowFooter = False
			lueState1.Properties.DropDownRows = 10

			lueState2.Properties.ShowHeader = False
			lueState2.Properties.ShowFooter = False
			lueState2.Properties.DropDownRows = 10

			lueContactInfo.Properties.ShowHeader = False
			lueContactInfo.Properties.ShowFooter = False
			lueContactInfo.Properties.DropDownRows = 20

			lueAdvisor.Properties.DropDownRows = 20

			lueBusinessBranches.Properties.ShowHeader = False
			lueBusinessBranches.Properties.ShowFooter = False
			lueBusinessBranches.Properties.DropDownRows = 10

			lueLanguage.Properties.ShowHeader = False
			lueLanguage.Properties.ShowFooter = False
			lueLanguage.Properties.DropDownRows = 10

			m_Utility = New Utility

			AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisor.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueBusinessBranches.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler cbFProperty.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler gridLueSecondProperty.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLanguage.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueContactInfo.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueState1.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueState2.ButtonClick, AddressOf OnDropDown_ButtonClick

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
					success = success AndAlso LoadCustomerData(customerNumber)
				End If
			Else
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

			grpadresse.Text = m_Translate.GetSafeTranslationValue("Kunde: {0}")

			' ---Reset address data---
			txtCompany1.Text = String.Empty
			txtCompany1.Properties.MaxLength = 70

			txtCompany2.Text = String.Empty
			txtCompany2.Properties.MaxLength = 70

			txtCompany3.Text = String.Empty
			txtCompany3.Properties.MaxLength = 70

			txtPostOfficeBox.Text = String.Empty
			txtPostOfficeBox.Properties.MaxLength = 70

			txtStreet.Text = String.Empty
			txtStreet.Properties.MaxLength = 70

			txtLocation.Text = String.Empty
			txtLocation.Properties.MaxLength = 70

			' ---Reset communication data---
			txtTelephone.Text = String.Empty
			txtTelephone.Properties.MaxLength = 70

			txtTelefax.Text = String.Empty
			txtTelefax.Properties.MaxLength = 70

			txtHomepage.Text = String.Empty
			txtHomepage.Properties.MaxLength = 70

			txtEmail.Text = String.Empty
			txtEmail.Properties.MaxLength = 70

			txtXing.Text = String.Empty
			txtXing.Properties.MaxLength = 255

			txtFacebook.Text = String.Empty
			txtFacebook.Properties.MaxLength = 255

			txtNoUseComment.Text = String.Empty
			txtNoUseComment.Properties.MaxLength = 50

			txtComment.Text = String.Empty
			txtESComment.Text = String.Empty
			txtRPComment.Text = String.Empty
			txtInvoiceComment.Text = String.Empty
			txtPaymentComment.Text = String.Empty

			' ---Reset check boxes---
			chkNoUse.Checked = False

			' ---Reset drop downs, grids and lists---

			ResetMandantDropDown()

			ResetCountryDropDown()
			ResetPostcodeDropDown()

			ResetAdvisorDropDown()
			ResetBusinessBranchesDropDown()
			ResetFirstPropertyDropDown()
			ResetSecondPropertyDropDown()

			ResetLanguageDropDown()
			ResetContactInfoDataDropDown()
			ResetCustomerStates1DropDown()
			ResetCustomerStates2DropDown()

			lstCustomerAssignedBusinessBranches.DataSource = Nothing

			lueMandant.Enabled = m_IsAuthorizedForCode_200001
			lueAdvisor.Enabled = m_IsAuthorizedForCode_224
			cbFProperty.Enabled = m_IsAuthorizedForCode_222
			gridLueSecondProperty.Enabled = m_IsAuthorizedForCode_223

			lueBusinessBranches.Enabled = m_IsAuthorizedForCode_675
			lstCustomerAssignedBusinessBranches.Enabled = m_IsAuthorizedForCode_675

			chkNoUse.Enabled = m_IsAuthorizedForCode_216

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

			Return isValid

		End Function

		''' <summary>
		''' Merges the custmer master data.
		''' </summary>
		''' <param name="customerMasterData">The customer master data object where the data gets filled into.</param>
		Public Overrides Sub MergeCustomerMasterData(ByVal customerMasterData As CustomerMasterData)

			If Not (IsCustomerDataLoaded AndAlso m_CustomerNumber = customerMasterData.CustomerNumber) Then Return

			' Fill values
			customerMasterData.CustomerMandantNumber = lueMandant.EditValue
				customerMasterData.Company1 = txtCompany1.Text
				customerMasterData.Company2 = txtCompany2.Text
				customerMasterData.Company3 = txtCompany3.Text
				customerMasterData.PostOfficeBox = txtPostOfficeBox.Text
				customerMasterData.Street = txtStreet.Text
				customerMasterData.CountryCode = lueCountry.EditValue
				customerMasterData.Postcode = luePostcode.EditValue

				Dim geoData = PerformGeoDataWebservice(customerMasterData.CountryCode, customerMasterData.Postcode)
				If Not geoData Is Nothing Then
					customerMasterData.Latitude = geoData.Latitude
					customerMasterData.Longitude = geoData.Longitude
				Else
					m_Logger.LogWarning(String.Format("postcode could not be founded: {0} >>> {1}", customerMasterData.CountryCode, customerMasterData.Postcode))
					customerMasterData.Latitude = 0
					customerMasterData.Longitude = 0
				End If

				customerMasterData.Location = txtLocation.Text
				customerMasterData.Telephone = txtTelephone.Text
				customerMasterData.Telefax = txtTelefax.Text
				customerMasterData.Hompage = txtHomepage.Text
				customerMasterData.EMail = txtEmail.Text

				If txtTelefax.Properties.Buttons(0).Tag = 2 Then
					customerMasterData.Telefax_Mailing = True
				Else
					customerMasterData.Telefax_Mailing = False
				End If

				If txtEmail.Properties.Buttons(1).Tag = 2 Then
					customerMasterData.Email_Mailing = True
				Else
					customerMasterData.Email_Mailing = False
				End If


				customerMasterData.facebook = txtFacebook.Text
				customerMasterData.xing = txtXing.Text

				customerMasterData.KST = lueAdvisor.EditValue

				' First Property (color) 
				If (Not cbFProperty.SelectedItem Is Nothing) Then

					Dim comboboxItem = CType(cbFProperty.SelectedItem, ImageComboBoxItem)
					Dim value = CType(comboboxItem.Value, Decimal)
					customerMasterData.FirstProperty = value

				Else
					customerMasterData.FirstProperty = Nothing

				End If

				customerMasterData.Language = lueLanguage.EditValue
				customerMasterData.HowContact = lueContactInfo.EditValue
				customerMasterData.CustomerState1 = lueState1.EditValue
				customerMasterData.CustomerState2 = lueState2.EditValue

				customerMasterData.NoUse = chkNoUse.Checked
				customerMasterData.NoUseComment = txtNoUseComment.Text

				customerMasterData.Comment = txtComment.Text
				customerMasterData.Notice_Employment = txtESComment.Text
				customerMasterData.Notice_Report = txtRPComment.Text
				customerMasterData.Notice_Invoice = txtInvoiceComment.Text
				customerMasterData.Notice_Payment = txtPaymentComment.Text

				ChangeTabPaneHeaderForGivenText()

		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub

		''' <summary>
		''' Updates the solvency descision.
		''' </summary>
		Public Sub UpdateSolvencyDecision(ByVal decisionID As Integer?)

			If Not decisionID.HasValue Then
				btnSolvencyDecision.Visible = False
				btnSolvencyDecision.Image = Nothing

			Else

				Try

					btnSolvencyDecision.Visible = True
					Dim decision = CType(decisionID, DecisionResult)
					Select Case decision
						Case DecisionResult.LightGreen
							btnSolvencyDecision.Image = My.Resources.bullet_green_small
						Case DecisionResult.Green
							btnSolvencyDecision.Image = My.Resources.bullet_green_small
						Case DecisionResult.YellowGreen
							btnSolvencyDecision.Image = My.Resources.bullet_green_small
						Case DecisionResult.Yellow
							btnSolvencyDecision.Image = My.Resources.bullet_yellow_small
						Case DecisionResult.Orange
							btnSolvencyDecision.Image = My.Resources.bullet_yellow_small
						Case DecisionResult.Red
							btnSolvencyDecision.Image = My.Resources.bullet_red_small
						Case DecisionResult.DarkRed
							btnSolvencyDecision.Image = My.Resources.bullet_red_small
						Case Else
							btnSolvencyDecision.Visible = False
							btnSolvencyDecision.Image = Nothing
					End Select

				Catch ex As Exception
					m_Logger.LogError(ex.ToString())
				End Try
			End If

		End Sub

#End Region

#Region "Private Methods"


		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpadresse.Text = m_Translate.GetSafeTranslationValue(Me.grpadresse.Text)

			Me.lblfirma.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma.Text)
			Me.lblfirma2.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma2.Text)
			Me.lblfirma3.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma3.Text)
			Me.lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
			Me.lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)
			Me.lblland.Text = m_Translate.GetSafeTranslationValue(Me.lblland.Text)
			Me.lblplz.Text = m_Translate.GetSafeTranslationValue(Me.lblplz.Text)
			Me.lblort.Text = m_Translate.GetSafeTranslationValue(Me.lblort.Text)


			Me.grpkommunikation.Text = m_Translate.GetSafeTranslationValue(Me.grpkommunikation.Text)

			Me.lbltelefon.Text = m_Translate.GetSafeTranslationValue(Me.lbltelefon.Text, True)
			Me.lbltelefax.Text = m_Translate.GetSafeTranslationValue(Me.lbltelefax.Text, True)
			Me.lblhomepage.Text = m_Translate.GetSafeTranslationValue(Me.lblhomepage.Text, True)
			Me.lblemail.Text = m_Translate.GetSafeTranslationValue(Me.lblemail.Text, True)
			Me.lblxing.Text = m_Translate.GetSafeTranslationValue(Me.lblxing.Text, True)
			Me.lblfacebook.Text = m_Translate.GetSafeTranslationValue(Me.lblfacebook.Text, True)

			Me.grpmerkmale.Text = m_Translate.GetSafeTranslationValue(Me.grpmerkmale.Text)

			Me.lbl1eigenschaft.Text = m_Translate.GetSafeTranslationValue(Me.lbl1eigenschaft.Text, True)
			Me.lbl2eigenschaft.Text = m_Translate.GetSafeTranslationValue(Me.lbl2eigenschaft.Text, True)
			Me.lbl1status.Text = m_Translate.GetSafeTranslationValue(Me.lbl1status.Text, True)
			Me.lbl2status.Text = m_Translate.GetSafeTranslationValue(Me.lbl2status.Text, True)
			Me.lblkontakt.Text = m_Translate.GetSafeTranslationValue(Me.lblkontakt.Text, True)

			Me.lblmandant.Text = m_Translate.GetSafeTranslationValue(Me.lblmandant.Text)
			Me.lblberater.Text = m_Translate.GetSafeTranslationValue(Me.lblberater.Text)
			Me.lblZugrifffiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblZugrifffiliale.Text)
			Me.lblsprache.Text = m_Translate.GetSafeTranslationValue(Me.lblsprache.Text)

			Me.chkNoUse.Text = m_Translate.GetSafeTranslationValue(Me.chkNoUse.Text)

			Me.grpbemerkung.Text = m_Translate.GetSafeTranslationValue(Me.grpbemerkung.Text)

		End Sub

		''' <summary>
		''' Loads customer data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerData(ByVal customerNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadCustomerMasterData(customerNumber)
			success = success AndAlso LoadAssignedBusinessBranchsDataOfCustomer(customerNumber)

			m_CustomerNumber = IIf(success, customerNumber, Nothing)

			errorProvider.Clear()

			Return success
		End Function

		''' <summary>
		''' Loads user rights.
		''' </summary>
		Private Sub LoadUserRights()

			m_IsAuthorizedForCode_200001 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 200001, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_216 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 216, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_224 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 224, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_222 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 222, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_223 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 223, m_InitializationData.MDData.MDNr)

			m_IsAuthorizedForCode_675 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)

		End Sub

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadMandantDropDownData()

			Dim dropDownResult = LoadCountryDropDownData()
			dropDownResult = dropDownResult AndAlso LoadPostcodeDropDownData()
			dropDownResult = dropDownResult AndAlso LoadAdvisorDropDownData()
			dropDownResult = dropDownResult AndAlso LoadBusinessBranchesDropDown()
			dropDownResult = dropDownResult AndAlso LoadFirstPropertyDropwDownData()
			dropDownResult = dropDownResult AndAlso LoadSecondPropertyDropDownData()
			dropDownResult = dropDownResult AndAlso LoadLanguageDropDownData()
			dropDownResult = dropDownResult AndAlso LoadContactInfoDropDownData()
			dropDownResult = dropDownResult AndAlso LoadCustomerStates1DropDownData()
			dropDownResult = dropDownResult AndAlso LoadCustomerStates2DropDownData()

			Return success
		End Function


#Region "load lookupedit"


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
					m_Logger.LogWarning("country data could not be loaded from webserver.")

					Return True
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
		''' Loads the advisor drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorDropDownData() As Boolean

			Dim userDataList = m_CommonDatabaseAccess.LoadAllAdvisorsData()

			Dim advisorViewDataList As New List(Of AdvisorViewData)

			If Not userDataList Is Nothing Then
				For Each userData In userDataList
					Dim advisorViewData As AdvisorViewData = New AdvisorViewData
					advisorViewData.KST = userData.KST
					advisorViewData.FirstName = userData.Firstname
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
		''' Loads the frist property drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadFirstPropertyDropwDownData() As Boolean
			Const WHITE_COLOR_WIN32_CODE As Integer = 16777215

			Dim firstPropertyData = m_DataAccess.LoadFirstPropertyData()

			If (firstPropertyData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für 1. Eigenschaft konnten nicht geladen werden."))
			End If

			Dim listOfColors = New List(Of ComboboxColorValueViewData)

			If (Not firstPropertyData Is Nothing) Then

				' Convert the first property data to color values
				For Each fproperty In firstPropertyData

					Dim colorValue = New ComboboxColorValueViewData
					Dim rawColorValue As Decimal? = fproperty.FPropertyValue

					If Not rawColorValue.HasValue Then
						colorValue.ColorName = "Nicht definierte Farbe"
						colorValue.Color = Color.Transparent
					Else

						If String.IsNullOrEmpty(fproperty.Description) And
								Not rawColorValue = 0 Then

							' The color name is not in the database -> create the name from the Win32 color name.
							Dim strColorname = Regex.Split(ColorTranslator.FromWin32(rawColorValue).ToString(), " ")(1)
							strColorname = Microsoft.VisualBasic.Strings.Replace(strColorname, "[", "")
							strColorname = Microsoft.VisualBasic.Strings.Replace(strColorname, "]", "")
							colorValue.ColorName = strColorname
						Else
							colorValue.ColorName = fproperty.Description.Trim
						End If

						' Special case: Color 0 is transformed to white
						If (rawColorValue = 0) Then
							rawColorValue = WHITE_COLOR_WIN32_CODE
							colorValue.ColorName = "White"
						End If
						colorValue.RawValue = rawColorValue
						colorValue.Color = ColorTranslator.FromWin32(rawColorValue)
					End If

					listOfColors.Add(colorValue)

				Next

				FillImageComboxWithColorValues(cbFProperty, listOfColors)

			End If

			Return Not firstPropertyData Is Nothing
		End Function

		''' <summary>
		''' Loads the second property drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadSecondPropertyDropDownData() As Boolean
			Dim secondProeprtyData = m_DataAccess.LoadSecondPropertyData()

			If (secondProeprtyData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für 2. Eigenschaft konnten nicht geladen werden."))
			End If

			gridLueSecondProperty.Properties.DataSource = secondProeprtyData
			gridLueSecondProperty.ForceInitialize()

			Return Not secondProeprtyData Is Nothing
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
		''' Load contact info drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadContactInfoDropDownData() As Boolean
			Dim contactInfoData = m_DataAccess.LoadCustomerContactInfoData()

			If (contactInfoData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Kontaktarten konnten nicht geladen werden."))
			End If

			lueContactInfo.Properties.DataSource = contactInfoData
			lueContactInfo.Properties.ForceInitialize()

			Return Not contactInfoData Is Nothing
		End Function

		''' <summary>
		''' Load customer states1 drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerStates1DropDownData() As Boolean
			Dim availableCustomerStates1 = m_DataAccess.LoadCustomerStateData1()

			If (availableCustomerStates1 Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Stati1 konnten nicht geladen werden."))
			End If

			lueState1.Properties.DataSource = availableCustomerStates1
			lueState1.Properties.ForceInitialize()

			Return Not availableCustomerStates1 Is Nothing
		End Function

		''' <summary>
		''' Load customer states2 drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerStates2DropDownData() As Boolean
			Dim availableCustomerStates2 = m_DataAccess.LoadCustomerStateData2()

			If (availableCustomerStates2 Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Stati2 konnten nicht geladen werden."))
			End If

			lueState2.Properties.DataSource = availableCustomerStates2
			lueState2.Properties.ForceInitialize()

			Return Not availableCustomerStates2 Is Nothing
		End Function


#End Region



#Region "reset lookupedit"

		''' <summary>
		''' Resets the mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

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
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetCountryDropDown()

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
		''' Resets the first property drop down.
		''' </summary>
		Private Sub ResetFirstPropertyDropDown()
			cbFProperty.SelectedItem = Nothing
		End Sub

		''' <summary>
		''' Resets the second property drop down.
		''' </summary>
		Private Sub ResetSecondPropertyDropDown()
			gridLueSecondProperty.Properties.DisplayMember = "Description"
			gridLueSecondProperty.Properties.ValueMember = "IconIndex"
			gridLueSecondProperty.Properties.PopulateViewColumns()
			gridLueSecondProperty.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			gridLueSecondProperty.Properties.NullText = String.Empty

		End Sub

		''' <summary>
		''' Resets the advisors drop down.
		''' </summary>
		Private Sub ResetAdvisorDropDown()

			lueAdvisor.Properties.DisplayMember = "FirstName_LastName"
			lueAdvisor.Properties.ValueMember = "KST"

			Dim columns = lueAdvisor.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("KST", 0, m_Translate.GetSafeTranslationValue("Kostenstelle")))
			columns.Add(New LookUpColumnInfo("LastName_FirstName", 0, m_Translate.GetSafeTranslationValue("Name")))

			lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
			lueAdvisor.Properties.AutoSearchColumnIndex = 1

			lueAdvisor.Properties.NullText = String.Empty
			lueAdvisor.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the language drop down.
		''' </summary>
		Public Sub ResetLanguageDropDown()

			lueLanguage.Properties.DisplayMember = "Description"
			lueLanguage.Properties.ValueMember = "Description"

			Dim columns = lueLanguage.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Sprache")))

			lueLanguage.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueLanguage.Properties.SearchMode = SearchMode.AutoComplete
			lueLanguage.Properties.AutoSearchColumnIndex = 0

			lueLanguage.Properties.NullText = String.Empty
			lueLanguage.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets contact info drop down.
		''' </summary>
		Public Sub ResetContactInfoDataDropDown()
			lueContactInfo.Properties.DisplayMember = "Description"
			lueContactInfo.Properties.ValueMember = "Description"

			Dim columns = lueContactInfo.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueContactInfo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueContactInfo.Properties.SearchMode = SearchMode.AutoComplete
			lueContactInfo.Properties.AutoSearchColumnIndex = 0

			lueContactInfo.Properties.NullText = String.Empty
			lueContactInfo.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets available customer states1 drop down.
		''' </summary>
		Public Sub ResetCustomerStates1DropDown()

			lueState1.Properties.DisplayMember = "Description"
			lueState1.Properties.ValueMember = "Description"

			Dim columns = lueState1.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueState1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueState1.Properties.SearchMode = SearchMode.AutoComplete
			lueState1.Properties.AutoSearchColumnIndex = 0

			lueState1.Properties.NullText = String.Empty
			lueState1.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets available customer states2 drop down.
		''' </summary>
		Public Sub ResetCustomerStates2DropDown()

			lueState2.Properties.DisplayMember = "Description"
			lueState2.Properties.ValueMember = "Description"

			Dim columns = lueState2.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueState2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueState2.Properties.SearchMode = SearchMode.AutoComplete
			lueState2.Properties.AutoSearchColumnIndex = 0

			lueState2.Properties.NullText = String.Empty
			lueState2.EditValue = Nothing

		End Sub


#End Region



		''' <summary>
		''' Loads customer master data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerMasterData(ByVal customerNumber As Integer) As Boolean

			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_InitializationData.UserData.UserFiliale)
			' TODO: decisionID from KD_Kreditinfo
			'btnSolvencyDecision.Image = My.Resources.bullet_ball_glass_green

			If (customerMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
				Return False
			End If
			Dim allowedCustomerDetails As Boolean = True
			If Not String.IsNullOrWhiteSpace(m_InitializationData.UserData.UserFiliale) Then
				If Not String.IsNullOrWhiteSpace(customerMasterData.KDBusinessBranch) Then
					If Not customerMasterData.KDBusinessBranch.Contains(m_InitializationData.UserData.UserFiliale) Then
						m_Logger.LogWarning("userright not allowed to open customer details!")
						allowedCustomerDetails = False
					End If
				End If
			End If
			If Not allowedCustomerDetails Then Return False

			grpadresse.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde: {0}"), customerMasterData.CustomerNumber)

			txtCompany1.Text = customerMasterData.Company1
			txtCompany2.Text = customerMasterData.Company2
			txtCompany3.Text = customerMasterData.Company3
			txtStreet.Text = customerMasterData.Street
			txtPostOfficeBox.Text = customerMasterData.PostOfficeBox
			lueCountry.EditValue = customerMasterData.CountryCode

			Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

			If Not String.IsNullOrEmpty(customerMasterData.Postcode) AndAlso
				Not listOfPostcode.Any(Function(postcode) postcode.Postcode = customerMasterData.Postcode) Then
				Dim newPostcode As New PostCodeData With {.Postcode = customerMasterData.Postcode}
				listOfPostcode.Add(newPostcode)
			End If
			luePostcode.EditValue = customerMasterData.Postcode

			txtLocation.Text = customerMasterData.Location

			' Contactdata
			txtTelephone.Text = customerMasterData.Telephone
			txtTelefax.Text = customerMasterData.Telefax
			txtEmail.Text = customerMasterData.EMail

			If customerMasterData.Telefax_Mailing Then
				txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.cancel_16x16
				txtTelefax.Properties.Buttons(0).Tag = 2
			Else
				txtTelefax.Properties.Buttons(0).ImageOptions.Image = My.Resources.apply_16x16
				txtTelefax.Properties.Buttons(0).Tag = 1
			End If

			If customerMasterData.Email_Mailing Then
				txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.cancel_16x16
				txtEmail.Properties.Buttons(1).Tag = 2
			Else
				txtEmail.Properties.Buttons(1).ImageOptions.Image = My.Resources.apply_16x16
				txtEmail.Properties.Buttons(1).Tag = 1
			End If



			txtHomepage.Text = customerMasterData.Hompage
			txtFacebook.Text = customerMasterData.facebook
			txtXing.Text = customerMasterData.xing

			lueMandant.EditValue = customerMasterData.CustomerMandantNumber
			lueAdvisor.EditValue = customerMasterData.KST

			Dim item = cbFProperty.Properties.Items.GetItem(customerMasterData.FirstProperty)
			cbFProperty.SelectedItem = item

			lueLanguage.EditValue = customerMasterData.Language
			lueContactInfo.EditValue = customerMasterData.HowContact
			lueState1.EditValue = customerMasterData.CustomerState1
			lueState2.EditValue = customerMasterData.CustomerState2

			chkNoUse.Checked = customerMasterData.NoUse
			txtNoUseComment.Text = customerMasterData.NoUseComment
			txtComment.Text = customerMasterData.Comment
			txtESComment.Text = customerMasterData.Notice_Employment
			txtRPComment.Text = customerMasterData.Notice_Report
			txtInvoiceComment.Text = customerMasterData.Notice_Invoice
			txtPaymentComment.Text = customerMasterData.Notice_Payment

			UpdateSolvencyDecision(customerMasterData.SolvencyDecisionID)
			btnSolvencyDecision.ToolTip = customerMasterData.SolvencyInfo

			ChangeTabPaneHeaderForGivenText()

			Return True

		End Function

		Private Sub ChangeTabPaneHeaderForGivenText()

			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(2).Properties.Appearance.ForeColor = If(String.IsNullOrWhiteSpace(txtComment.EditValue), Color.Black, Color.Orange)
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(3).Properties.Appearance.ForeColor = If(String.IsNullOrWhiteSpace(txtESComment.EditValue), Color.Black, Color.Orange)
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(4).Properties.Appearance.ForeColor = If(String.IsNullOrWhiteSpace(txtRPComment.EditValue), Color.Black, Color.Orange)
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(5).Properties.Appearance.ForeColor = If(String.IsNullOrWhiteSpace(txtInvoiceComment.EditValue), Color.Black, Color.Orange)
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(6).Properties.Appearance.ForeColor = If(String.IsNullOrWhiteSpace(txtPaymentComment.EditValue), Color.Black, Color.Orange)

			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(2).Properties.Appearance.Font = New Font(tnpNotices.AppearanceButton.Normal.Font, If(String.IsNullOrWhiteSpace(txtComment.EditValue), FontStyle.Regular, FontStyle.Bold))
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(3).Properties.Appearance.Font = New Font(tnpNotices.AppearanceButton.Normal.Font, If(String.IsNullOrWhiteSpace(txtESComment.EditValue), FontStyle.Regular, FontStyle.Bold))
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(4).Properties.Appearance.Font = New Font(tnpNotices.AppearanceButton.Normal.Font, If(String.IsNullOrWhiteSpace(txtRPComment.EditValue), FontStyle.Regular, FontStyle.Bold))
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(5).Properties.Appearance.Font = New Font(tnpNotices.AppearanceButton.Normal.Font, If(String.IsNullOrWhiteSpace(txtInvoiceComment.EditValue), FontStyle.Regular, FontStyle.Bold))
			CType(tnpNotices, INavigationPane).ButtonsPanel.Buttons(6).Properties.Appearance.Font = New Font(tnpNotices.AppearanceButton.Normal.Font, If(String.IsNullOrWhiteSpace(txtPaymentComment.EditValue), FontStyle.Regular, FontStyle.Bold))

		End Sub

		''' <summary>
		''' Loads assigned customer business branches data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedBusinessBranchsDataOfCustomer(ByVal customerNumber As Integer) As Boolean

			Dim customerBusinessBranchData = m_DataAccess.LoadAssignedBusinessBranchsDataOfCustomer(customerNumber)

			If (customerBusinessBranchData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Kundenfilialen konnten nicht geladen werden"))
				Return False
			End If

			lstCustomerAssignedBusinessBranches.DisplayMember = "Name"
			lstCustomerAssignedBusinessBranches.ValueMember = "Name"
			lstCustomerAssignedBusinessBranches.DataSource = customerBusinessBranchData

			Return True

		End Function

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

		''' <summary>
		''' Handles click on open telephone button.
		''' </summary>
		Private Sub OntxtTelephone_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtTelephone.ButtonClick

			If Not String.IsNullOrWhiteSpace(txtTelephone.Text) Then
				OpenTelephone(txtTelephone.Text)
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
		''' Handles click on open home page button.
		''' </summary>
		Private Sub OntxtHomepage_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtHomepage.ButtonClick
			If Not String.IsNullOrWhiteSpace(txtHomepage.Text) Then
				m_UtilityUI.OpenURL(txtHomepage.Text)
			End If
		End Sub

		''' <summary>
		''' Handles click on open xing button.
		''' </summary>
		Private Sub OntxtXing_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtXing.ButtonClick
			If Not String.IsNullOrWhiteSpace(txtXing.Text) Then
				m_UtilityUI.OpenURL(txtXing.Text)
			End If
		End Sub

		''' <summary>
		''' Handles click on open facebook button.
		''' </summary>
		Private Sub OntxtFacebook_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFacebook.ButtonClick
			If Not String.IsNullOrWhiteSpace(txtFacebook.Text) Then
				m_UtilityUI.OpenURL(txtFacebook.Text)
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
				Dim result As Boolean
				m_UtilityUI.OpenEmail(txtEmail.Text)
				Dim obj As New SPSSendMail.ContactLogger(New SPSSendMail.InitializeClass With {
														 .MDData = m_InitializationData.MDData, .ProsonalizedData = m_InitializationData.ProsonalizedData,
														 .TranslationData = m_InitializationData.TranslationData, .UserData = m_InitializationData.UserData})

				result = obj.NewResponsiblePersonContact(m_CustomerNumber, txtEmail.Text,
															 String.Empty, Nothing,
															 Now, m_InitializationData.UserData.UserFullName, Nothing,
															 Now, m_InitializationData.UserData.UserFullName, Nothing, Nothing, "Einzelmail", 1, False, True,
															 False)

			End If
		End Sub

		''' <summary>
		''' Handles change of available business branch.
		''' </summary>
		Private Sub OnLueAvailableBusinessBranch_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueBusinessBranches.EditValueChanged

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			Dim businessBranchAdded = False

			Dim selectedBusinessBranchData As AvilableBusinessBranchData = TryCast(lueBusinessBranches.GetSelectedDataRow(), AvilableBusinessBranchData)

			If (Not selectedBusinessBranchData Is Nothing) Then

				' Load the already assigned business branches
				Dim customerBusinessBranchData = m_DataAccess.LoadAssignedBusinessBranchsDataOfCustomer(m_CustomerNumber)

				' Check if the business branch is already assigned.
				If (Not customerBusinessBranchData Is Nothing AndAlso
						Not customerBusinessBranchData.Any(Function(data) data.Name.ToLower().Trim() = selectedBusinessBranchData.Name.ToLower().Trim())) Then

					' Add to database
					Dim businessBranchAssignmentToInsert = New CustomerAssignedBusinessBranchData With {.CustomerNumber = m_CustomerNumber, .MDNr = selectedBusinessBranchData.Code_1, .Name = selectedBusinessBranchData.Name}
					businessBranchAdded = m_DataAccess.AddCustomerBussinessBranchAssignment(businessBranchAssignmentToInsert)
				End If

				' Reload business branches.
				LoadAssignedBusinessBranchsDataOfCustomer(m_CustomerNumber)

			End If

		End Sub

		''' <summary>
		''' Handles keydown event on customer business branches list.
		''' </summary>
		Private Sub OnLstCustomerBusinessBranches_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstCustomerAssignedBusinessBranches.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim selectedBusinessBranchData As CustomerAssignedBusinessBranchData = TryCast(lstCustomerAssignedBusinessBranches.SelectedItem, CustomerAssignedBusinessBranchData)

				If (Not selectedBusinessBranchData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerBusinessBranchDataAssignment(selectedBusinessBranchData.ID) Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Filiale konnte nicht gelöscht werden."))
					End If

					LoadAssignedBusinessBranchsDataOfCustomer(m_CustomerNumber)

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
				End If
			End If
		End Sub

		''' <summary>
		''' Handles click on solvency descision button.
		''' </summary>
		Private Sub OnBtnSolvencyDecision_Click(sender As System.Object, e As System.EventArgs) Handles btnSolvencyDecision.Click

			Dim lastestCustomerCreditInfoData = m_DataAccess.LoadLatestCustomerSolvencyCheckCreditInfo(m_CustomerNumber, True)

			If Not lastestCustomerCreditInfoData Is Nothing AndAlso
					 Not lastestCustomerCreditInfoData.DV_PDFFile Is Nothing Then

				Dim bytes() = lastestCustomerCreditInfoData.DV_PDFFile
				Dim tempFileName = System.IO.Path.GetTempFileName()
				Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

				If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
					m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

				End If

			End If

		End Sub

		''' <summary>
		''' Fills an image combobox with color values.s
		''' </summary>
		''' <param name="imageCombobox">The image combobox.</param>
		''' <param name="colors">The colors.</param>
		Private Sub FillImageComboxWithColorValues(ByVal imageCombobox As ImageComboBoxEdit, ByVal colors As IEnumerable(Of ComboboxColorValueViewData))

			imageCombobox.Properties.Items.Clear()

			' Add the ImageComboBoxItem items.
			' Description -> ColorName
			' Value -> RawValue
			For Each item In colors
				imageCombobox.Properties.Items.Add(New ImageComboBoxItem(item.ColorName, item.RawValue))
			Next

			' Now create a list of images from the raw color values.
			Dim imageList As New ImageList
			imageCombobox.Properties.SmallImages = imageList

			For Each item In imageCombobox.Properties.Items

				Dim width As Integer = 16
				Dim height As Integer = 16
				Dim bmp As New Bitmap(width, height)

				Using g = Graphics.FromImage(bmp)
					g.DrawRectangle(New Pen(Color.Black, 2), 0, 0, width, height)
					g.FillRectangle(New SolidBrush(ColorTranslator.FromWin32(CType(item.Value, Decimal))), 1, 1, width - 2, height - 2)

				End Using

				imageList.Images.Add(bmp)
				item.ImageIndex = imageList.Images.Count - 1

			Next

		End Sub

		Public Sub OpenTelephone(ByVal number As String)
			Dim oMyProg As New SPSTapi.UI.frmCaller(m_InitializationData)

			oMyProg.LoadData(number)
			oMyProg.Show()
			oMyProg.BringToFront()

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
		''' Combobox Color value view data.
		''' </summary>
		''' <remarks></remarks>
		Private Class ComboboxColorValueViewData
			Public Property ColorName As String
			Public Property RawValue As Decimal
			Public Color As System.Drawing.Color
		End Class

		''' <summary>
		''' Advisor view data.
		''' </summary>
		Private Class AdvisorViewData

			Public Property KST As String
			Public Property FirstName As String
			Public Property LastName As String

			Public ReadOnly Property LastName_FirstName As String
				Get
					Return String.Format("{0} {1}", LastName, FirstName)
				End Get
			End Property

			Public ReadOnly Property FirstName_LastName As String
				Get
					Return String.Format("{0} {1}", FirstName, LastName)
				End Get
			End Property

		End Class

#End Region


	End Class

End Namespace