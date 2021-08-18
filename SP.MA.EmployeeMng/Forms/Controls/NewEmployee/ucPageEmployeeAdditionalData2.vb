
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.ProgPath
Imports SP.Internal.Automations.BaseTable
Imports SP.Internal.Automations
Imports System.ComponentModel

Namespace UI

	Public Class ucPageEmployeeAdditionalData2

#Region "Private Consts"

		Private Const Code_NoQST As String = "0"
		Private Const PERMISSION_CODE_S = ""
		Private Const PERMISSION_CODE_C = "C"
		Private Const COUNTRY_CODE_CH = "CH"

		Private Const MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicetaxinfoservices"
		Private Const DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI = "wsSPS_services/SPEmployeeTaxInfoService.asmx" ' "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx"
		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private Fields"

		Private m_TaxDataHelper As TaxDataHelper
		Private m_QSTTranslationData As IEnumerable(Of SP.Internal.Automations.TaxCodeData)
		Private m_ChurchTaxCodeTranslationData As IEnumerable(Of SP.Internal.Automations.TaxChurchCodeData)
		Private m_SalaryDataHelperFunctions As SalaryDataHelperFunctions
		Private m_TaxInfoWebServiceCallOk As Boolean = True

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' Tax info service URL.
		''' </summary>
		Private m_TaxInfoServiceUrl As String

		Private m_UserSec117 As Boolean
		Private m_UserSec118 As Boolean
		Private m_BaseTableUtil As SPSBaseTables

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			Try
				m_ProgPath = New ClsProgPath
				m_MandantData = New Mandant
				'm_BaseTableUtil = New SPSBaseTables(m_InitializationData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			m_SalaryDataHelperFunctions = New SalaryDataHelperFunctions()

			AddHandler luePermission.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueForeignCategory.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditPermissionTo.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler lueTaxCanton.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditAnsQSTBis.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueChurchTax.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueNumberOfChildren.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCommunity.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueEmploymentType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueOtherEmploymentType.ButtonClick, AddressOf OnDropDown_ButtonClick

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

		End Sub

		''' <summary>
		''' Activates the page.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function ActivatePage() As Boolean

			Dim success As Boolean = True

			If (m_IsFirstPageActivation) Then
				m_BaseTableUtil = New SPSBaseTables(m_InitializationData)

				'LoadWebserviceUrl()
				success = success AndAlso LoadQSTAndChurchTaxCodeTranslationData()
				success = success AndAlso LoadDropDownData()

				Dim basicdata = m_UCMediator.SelectedBasiscData

				' Bewilligung (Take nationality and country code from other user control)
				success = success AndAlso LoadPermissionDropDownData(basicdata.Nationality, basicdata.CountryCode, Nothing)

				PreselectData()

			End If

			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_IsFirstPageActivation = True

			m_TaxDataHelper = Nothing
			m_QSTTranslationData = Nothing
			m_ChurchTaxCodeTranslationData = Nothing
			m_TaxInfoWebServiceCallOk = True

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			chkCertificateForResidenceReceived.Checked = False
			dateEditAnsQSTBis.EditValue = Nothing
			dateEditPermissionTo.EditValue = Nothing

			txtBirthPlace.Text = String.Empty
			txtBirthPlace.Properties.MaxLength = 100
			chkCHPartner.Checked = False
			chkNoSpecialTax.Checked = False
			txtZemisNumber.Text = String.Empty

			'  Reset drop downs and lists
			ResetPermissionDropDown()
			ResetForeignCategoryDropDown()

			ResetTaxCantonDropDown()
			ResetCommunityDropDown()
			ResetCodeDropDown()
			ResetChurchTaxDropDown()
			ResetNumberOfChildrenDropDown()
			ResetEmploymentTypeDropDown()
			ResetOtherEmploymentTypeDropDown()

			ResetTypeofStayDropDown()


			m_UserSec117 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 117, m_InitializationData.MDData.MDNr)
			m_UserSec118 = m_UserSec117 AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 118, m_InitializationData.MDData.MDNr)
			If Not m_UserSec118 AndAlso m_UserSec117 Then m_UserSec117 = False

			lueCode.Enabled = m_UserSec117
			lueChurchTax.Enabled = m_UserSec117
			lueNumberOfChildren.Enabled = m_UserSec117

			luePermission.Enabled = m_UserSec118
			'dateEditPermissionTo.Enabled = m_UserSec118
			chkCHPartner.Enabled = m_UserSec118


			m_SuppressUIEvents = suppressUIEventsState

			errorProvider.Clear()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			errorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errotTextTaxInfoWebService = m_Translate.GetSafeTranslationValue("Der Webservice für die Steuerinformation konnte nicht angesprochen werden.")

			Dim mdNr = m_UCMediator.SelectedMandantAndAdvisorData.MandantData.MandantNumber

			Dim mustPermissionBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																				   String.Format("{0}/emplyoeepermitselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustPermissionToDateBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																			 String.Format("{0}/emplyoeepermitdateselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustBirthPlaceBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																	   String.Format("{0}/emplyoeehometownselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustTaxBeSelectedifnotCH As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																													 String.Format("{0}/emplyoeesteuerselectionifnotch", FORM_XML_REQUIREDFIEKDS_KEY)), True)
			Dim mustCurchTaxBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																													 String.Format("{0}/emplyoeekirchensteuerselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustSCantonBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
														   String.Format("{0}/emplyoeesteuercantonselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim taxcodeforcivilstatesingleperson As String = m_Utility.ParseToString(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
														   String.Format("{0}/taxcodeforcivilstatesingleperson", FORM_XML_MAIN_KEY)), "A0Y")
			Dim taxcodeforcivilstatemarriedperson As String = m_Utility.ParseToString(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
														   String.Format("{0}/taxcodeforcivilstatemarriedperson", FORM_XML_MAIN_KEY)), "C0Y")

			Dim isValid As Boolean = True
			Dim nationalityCode As String = m_UCMediator.SelectedBasiscData.Nationality
			Dim countryCode As String = m_UCMediator.SelectedBasiscData.CountryCode
			'Dim emptyPermission As Boolean = luePermission.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePermission.EditValue)
			Dim allowedPermissionToBeNothing = (luePermission.EditValue = PERMISSION_CODE_C OrElse luePermission.EditValue = "13" OrElse chkCHPartner.EditValue OrElse (nationalityCode = "CH" AndAlso countryCode = "CH")) AndAlso m_UserSec118

			If allowedPermissionToBeNothing Then
				mustPermissionBeSelected = False
				mustPermissionToDateBeSelected = False
				mustBirthPlaceBeSelected = False
			End If

			' Check if web service call was ok
			isValid = isValid And SetErrorIfInvalid(lueTaxCanton, errorProvider, Not m_TaxInfoWebServiceCallOk, errotTextTaxInfoWebService)


			' Check permission data
			If mustPermissionBeSelected AndAlso m_UserSec118 Then
				isValid = isValid And SetErrorIfInvalid(luePermission, errorProvider, String.IsNullOrWhiteSpace(luePermission.EditValue), errorText)
			End If

			' Check permissionTo date
			If mustPermissionToDateBeSelected Then ' AndAlso m_UserSec118 Then
				isValid = isValid And SetErrorIfInvalid(dateEditPermissionTo, errorProvider, dateEditPermissionTo.EditValue Is Nothing, errorText)
			End If

			' Check birthplace
			If mustBirthPlaceBeSelected Then
				isValid = isValid And SetErrorIfInvalid(txtBirthPlace, errorProvider, String.IsNullOrWhiteSpace(txtBirthPlace.Text), errorText)
			End If


			Dim requiredTAX As Boolean = True
			countryCode = countryCode.ToUpper

			If countryCode = "LI" Then
				requiredTAX = False

			ElseIf countryCode = "CH" Then
				'If allowedPermissionToBeNothing Then
				'	requiredTAX = Not chkCHPartner.EditValue
				'Else
				requiredTAX = Not allowedPermissionToBeNothing
				'End If

			ElseIf countryCode = "FR" Then
				If chkCertificateForResidenceReceived.Checked Then requiredTAX = False

			Else
				requiredTAX = True
			End If

			'If Not mustTaxBeSelectedifnotCH AndAlso Not (luePermission.EditValue = PERMISSION_CODE_C OrElse luePermission.EditValue = PERMISSION_CODE_S) Then requiredTAX = False

			mustCurchTaxBeSelected = requiredTAX
			mustSCantonBeSelected = requiredTAX

			If m_UserSec117 AndAlso requiredTAX Then
				isValid = isValid And SetErrorIfInvalid(lueCode, errorProvider, (lueCode.EditValue Is Nothing OrElse lueCode.EditValue = "0"), errorText)
			End If

			' Check church tax
			If mustCurchTaxBeSelected And Not lueChurchTax.Properties.DataSource Is Nothing Then
				Dim listOfChurchTaxCodes = CType(lueChurchTax.Properties.DataSource, List(Of ChurchViewData))

				isValid = isValid And SetErrorIfInvalid(lueChurchTax, errorProvider, lueChurchTax.EditValue Is Nothing AndAlso listOfChurchTaxCodes.Count > 0, errorText)
			End If

			' Check S_Canton (Steuerkanton)
			If mustSCantonBeSelected Then
				isValid = isValid And SetErrorIfInvalid(lueTaxCanton, errorProvider, lueTaxCanton.EditValue Is Nothing, errorText)
			End If

			Return isValid

		End Function


		''' <summary>
		''' Handle change of country code.
		''' </summary>
		Public Sub CountryCodeHasChanged()

			If m_IsFirstPageActivation Then
				' Ignore information if page is not loaded yet.
				Return
			End If

			Dim basicData = m_UCMediator.SelectedBasiscData

			Dim selectedNationality = basicData.Nationality
			Dim selectedCountry = basicData.CountryCode

			LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
			ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

		''' <summary>
		''' Handle change of nationality.
		''' </summary>
		Public Sub NationalityHasChanged()

			If m_IsFirstPageActivation Then
				' Ignore information if page is not loaded yet.
				Return
			End If

			Dim basicData = m_UCMediator.SelectedBasiscData

			Dim selectedNationality = basicData.Nationality
			Dim selectedCountry = basicData.CountryCode

			LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
			ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected employee additional data2. data.
		''' </summary>
		''' <returns>Employee additional data1.</returns>
		Public ReadOnly Property SelectedEmployeeAdditionalData2 As InitAdditionalEmployeeData2
			Get

				Dim bfsNumber As Integer = 0
				Dim communityLabel As String = String.Empty
				If Not lueCommunity.EditValue Is Nothing Then
					Dim qstCommunities = CType(lueCommunity.Properties.DataSource, BindingList(Of CommunityData))
					Dim commData = qstCommunities.Where(Function(x) x.BFSNumber = lueCommunity.EditValue).FirstOrDefault

					bfsNumber = commData.BFSNumber
					communityLabel = commData.Translated_Value
				End If

				Dim data As New InitAdditionalEmployeeData2 With {.Permission = luePermission.EditValue,
					.ForeignCategory = lueForeignCategory.EditValue,
					.PermissionToDate = dateEditPermissionTo.EditValue,
					.BirthPlace = txtBirthPlace.EditValue,
					.ZEMISNumber = txtZemisNumber.EditValue,
					.S_Canton = lueTaxCanton.EditValue,
					.Residence = chkCertificateForResidenceReceived.Checked,
					.ANS_QST_Bis = dateEditAnsQSTBis.EditValue,
					.Q_Steuer = lueCode.EditValue,
					.ChurchTax = lueChurchTax.EditValue,
					.ChildsCount = lueNumberOfChildren.EditValue,
					.QSTCommunity = lueCommunity.EditValue,
					.TaxCommunityCode = bfsNumber,
					.TaxCommunityLabel = communityLabel,
					.CHPartner = chkCHPartner.Checked,
					.NoSpecialTax = chkNoSpecialTax.Checked,
					.ValidatePermissionWithTax = True,
					.TypeofStay = lueTypeofStay.EditValue,
					.EmploymentType = lueEmploymentType.EditValue,
					.OtherEmploymentType = lueOtherEmploymentType.EditValue
				}

				Return data
			End Get
		End Property

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' bewilligung
			grpBewilligung.Text = m_Translate.GetSafeTranslationValue(grpBewilligung.Text)
			lblBewilligung.Text = m_Translate.GetSafeTranslationValue(lblBewilligung.Text)
			lblKategorie.Text = m_Translate.GetSafeTranslationValue(lblKategorie.Text)
			lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(lblBescheinigungGueltigBis.Text)
			chkCHPartner.Text = m_Translate.GetSafeTranslationValue(chkCHPartner.Text)
			chkNoSpecialTax.Text = m_Translate.GetSafeTranslationValue(chkNoSpecialTax.Text)
			lblBis.Text = m_Translate.GetSafeTranslationValue(lblBis.Text)
			lblHeimatort.Text = m_Translate.GetSafeTranslationValue(lblHeimatort.Text)

			' Quellensteuer
			grpQST.Text = m_Translate.GetSafeTranslationValue(grpQST.Text)
			lblKantonFuerQuellensteuer.Text = m_Translate.GetSafeTranslationValue(lblKantonFuerQuellensteuer.Text)
			lblGemeinde.Text = m_Translate.GetSafeTranslationValue(lblGemeinde.Text)
			lblTarifgruppe.Text = m_Translate.GetSafeTranslationValue(lblTarifgruppe.Text)
			lblKirchensteuer.Text = m_Translate.GetSafeTranslationValue(lblKirchensteuer.Text)
			lblAnzahlKinder.Text = m_Translate.GetSafeTranslationValue(lblAnzahlKinder.Text)
			lblBeschaeftigungsart.Text = m_Translate.GetSafeTranslationValue(lblBeschaeftigungsart.Text)
			lblWeitereBeschaeftigung.Text = m_Translate.GetSafeTranslationValue(lblWeitereBeschaeftigung.Text)

			grpBorderCrossers.Text = m_Translate.GetSafeTranslationValue(grpBorderCrossers.Text)
			chkCertificateForResidenceReceived.Text = m_Translate.GetSafeTranslationValue(chkCertificateForResidenceReceived.Text)
			lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(lblBescheinigungGueltigBis.Text)
			lblAufenthaltsart.Text = m_Translate.GetSafeTranslationValue(lblAufenthaltsart.Text)

		End Sub


#Region "reset"

		''' <summary>
		''' Resets the tax canton drop down.
		''' </summary>
		Private Sub ResetTaxCantonDropDown()

			lueTaxCanton.Properties.DisplayMember = "Description"
			lueTaxCanton.Properties.ValueMember = "GetField"

			Dim columns = lueTaxCanton.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Kanton")))

			lueTaxCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTaxCanton.Properties.SearchMode = SearchMode.AutoComplete
			lueTaxCanton.Properties.AutoSearchColumnIndex = 0
			lueTaxCanton.Properties.NullText = String.Empty
			lueTaxCanton.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the code drop down.
		''' </summary>
		Private Sub ResetCodeDropDown()

			lueCode.Properties.DisplayMember = "CodeAndTranslation"
			lueCode.Properties.ValueMember = "Code"

			Dim columns = lueCode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Code", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("Translation", 0, String.Empty))

			lueCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCode.Properties.SearchMode = SearchMode.AutoComplete
			lueCode.Properties.AutoSearchColumnIndex = 1
			lueCode.Properties.NullText = String.Empty
			lueCode.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the church tax drop down.
		''' </summary>
		Private Sub ResetChurchTaxDropDown()

			lueChurchTax.Properties.DisplayMember = "ChurchCodeAndTranslation"
			lueChurchTax.Properties.ValueMember = "ChurchTaxCode"

			Dim columns = lueChurchTax.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("ChurchTaxCode", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("ChurchCodeAndTranslation", 0, String.Empty))

			lueChurchTax.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueChurchTax.Properties.SearchMode = SearchMode.AutoComplete
			lueChurchTax.Properties.AutoSearchColumnIndex = 1
			lueChurchTax.Properties.NullText = String.Empty
			lueChurchTax.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the number of children drop down.
		''' </summary>
		Private Sub ResetNumberOfChildrenDropDown()

			lueNumberOfChildren.Properties.DisplayMember = "NumberOfChildren"
			lueNumberOfChildren.Properties.ValueMember = "NumberOfChildren"

			Dim columns = lueNumberOfChildren.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("NumberOfChildren", 0, String.Empty))

			lueNumberOfChildren.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueNumberOfChildren.Properties.SearchMode = SearchMode.AutoComplete
			lueNumberOfChildren.Properties.AutoSearchColumnIndex = 1
			lueNumberOfChildren.Properties.NullText = String.Empty
			lueNumberOfChildren.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the community drop down.
		''' </summary>
		Private Sub ResetCommunityDropDown()

			lueCommunity.Properties.SearchMode = SearchMode.OnlyInPopup
			lueCommunity.Properties.TextEditStyle = TextEditStyles.Standard

			lueCommunity.Properties.DisplayMember = "ViewData"
			lueCommunity.Properties.ValueMember = "BFSNumber"

			Dim columns = lueCommunity.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Gemeinde")))

			lueCommunity.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCommunity.Properties.SearchMode = SearchMode.AutoComplete
			lueCommunity.Properties.AutoSearchColumnIndex = 1
			lueCommunity.Properties.NullText = String.Empty
			lueCommunity.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the permission state drop down.
		''' </summary>
		Private Sub ResetPermissionDropDown()

			luePermission.Properties.DisplayMember = "Translated_Value"
			luePermission.Properties.ValueMember = "Rec_Value"

			Dim columns = luePermission.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Rec_Value", m_Translate.GetSafeTranslationValue("Code"), 100))
			columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Kategorie"), 500))

			luePermission.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePermission.Properties.SearchMode = SearchMode.AutoComplete
			luePermission.Properties.AutoSearchColumnIndex = 1
			luePermission.Properties.NullText = String.Empty

			luePermission.Properties.PopupWidth = 700
			luePermission.Properties.PopupSizeable = True

			luePermission.EditValue = Nothing
		End Sub

		Private Sub ResetForeignCategoryDropDown()

			lueForeignCategory.Properties.DisplayMember = "Translated_Value"
			lueForeignCategory.Properties.ValueMember = "Rec_Value"

			Dim columns = lueForeignCategory.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Rec_Value", m_Translate.GetSafeTranslationValue("Code"), 100))
			columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Kategorie"), 500))

			lueForeignCategory.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueForeignCategory.Properties.SearchMode = SearchMode.AutoComplete
			lueForeignCategory.Properties.AutoSearchColumnIndex = 1
			lueForeignCategory.Properties.NullText = String.Empty

			lueForeignCategory.Properties.PopupWidth = 700
			lueForeignCategory.Properties.PopupSizeable = True

			lueForeignCategory.EditValue = Nothing
		End Sub

		Private Sub ResetEmploymentTypeDropDown()

			lueEmploymentType.Properties.ShowHeader = False
			lueEmploymentType.Properties.ShowFooter = False
			lueEmploymentType.Properties.DisplayMember = "Translated_Value"
			lueEmploymentType.Properties.ValueMember = "Rec_Value"

			Dim columns = lueEmploymentType.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Beschäftigungsart")))

			lueEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
			lueEmploymentType.Properties.AutoSearchColumnIndex = 0

			lueEmploymentType.Properties.NullText = String.Empty
			lueEmploymentType.EditValue = Nothing

		End Sub

		Private Sub ResetOtherEmploymentTypeDropDown()

			lueOtherEmploymentType.Properties.ShowHeader = False
			lueOtherEmploymentType.Properties.ShowFooter = False
			lueOtherEmploymentType.Properties.DisplayMember = "Translated_Value"
			lueOtherEmploymentType.Properties.ValueMember = "Rec_Value"

			Dim columns = lueOtherEmploymentType.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Weitere Beschäftigungsart")))

			lueOtherEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueOtherEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
			lueOtherEmploymentType.Properties.AutoSearchColumnIndex = 0

			lueOtherEmploymentType.Properties.NullText = String.Empty
			lueOtherEmploymentType.EditValue = Nothing

		End Sub

		Private Sub ResetTypeofStayDropDown()

			lueTypeofStay.Properties.ShowHeader = False
			lueTypeofStay.Properties.ShowFooter = False
			lueTypeofStay.Properties.DisplayMember = "Translated_Value"
			lueTypeofStay.Properties.ValueMember = "Rec_Value"

			Dim columns = lueTypeofStay.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Aufgenthaltsart")))

			lueTypeofStay.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTypeofStay.Properties.SearchMode = SearchMode.AutoComplete
			lueTypeofStay.Properties.AutoSearchColumnIndex = 0

			lueTypeofStay.Properties.NullText = String.Empty
			lueTypeofStay.EditValue = Nothing

		End Sub

#End Region


		''' <summary>
		''' Loads QST and church tax code translation data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadQSTAndChurchTaxCodeTranslationData() As Boolean
			Dim success As Boolean = True

			'm_QSTTranslationData = m_UCMediator.EmployeeDbAccess.LoadQSTData()
			'm_ChurchTaxCodeTranslationData = m_UCMediator.EmployeeDbAccess.LoadChurchTaxCodeData()

			m_QSTTranslationData = m_BaseTableUtil.PerformTaxCodeDataOverWebService(m_InitializationData.UserData.UserLanguage)
			m_ChurchTaxCodeTranslationData = m_BaseTableUtil.PerformTaxChurchCodeDataOverWebService(m_InitializationData.UserData.UserLanguage) ' m_EmployeeDataAccess.LoadChurchTaxCodeData()

			If m_QSTTranslationData Is Nothing OrElse m_ChurchTaxCodeTranslationData Is Nothing Then
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadTaxCantonDropDownData()
			'success = success AndAlso LoadCommunityOverWebService()

			success = success AndAlso LoadEmploymentTypeDataOverWebService()
			success = success AndAlso LoadOtherEmploymentTypeDataOverWebService()

			success = success AndAlso LoadTypeOfStayDataOverWebService()

			Return success
		End Function

		''' <summary>
		''' Loads the tax canton drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadTaxCantonDropDownData() As Boolean
			Dim cantonData = m_UCMediator.CommonDbAccess.LoadCantonData()

			If (cantonData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantondaten (für Quellensteuer) konnten nicht geladen werden."))
			End If

			lueTaxCanton.Properties.DataSource = cantonData
			lueTaxCanton.Properties.ForceInitialize()
			lueTaxCanton.Properties.DropDownRows = Math.Min(30, cantonData.Count)

			Return Not cantonData Is Nothing
		End Function

		'Private Function LoadCommunityDropDownData() As Boolean
		'	Dim canton As String = String.Empty
		'	If Not lueTaxCanton.EditValue Is Nothing Then
		'		canton = lueTaxCanton.EditValue
		'	End If
		'	Dim communityData = m_UCMediator.EmployeeDbAccess.LoadEmployeeQSTCommunitiesWithCanton(canton)

		'	If (communityData Is Nothing) Then
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gemeindedaten konnten nicht geladen werden."))
		'	End If

		'	lueCommunity.Properties.DataSource = communityData
		'	lueCommunity.Properties.ForceInitialize()

		'	Return Not communityData Is Nothing
		'End Function

		''' <summary>
		''' Loads the community drop down data.
		''' </summary>
		Private Function LoadCommunityOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim canton = lueTaxCanton.EditValue
				If String.IsNullOrWhiteSpace(canton) Then canton = m_InitializationData.MDData.MDCanton

				Dim result = m_BaseTableUtil.PerformCommunityDataOverWebService(canton, language)

				lueCommunity.Properties.DataSource = result
				If Not result Is Nothing Then lueCommunity.Properties.DropDownRows = Math.Min(20, result.Count + 1)

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		''' <summary>
		''' Loads permission drop down data.
		''' </summary>
		'''<param name="nationality">The nationalilty code.</param>
		'''<param name="countryCode">The country code.</param>
		'''<param name="currentPermission">The current permission code.</param>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadPermissionDropDownData(ByVal nationality As String, ByVal countryCode As String, ByVal currentPermission As String) As Boolean
			'Dim permissionData = m_UCMediator.CommonDbAccess.LoadPermissionData()
			Dim language = m_InitializationData.UserData.UserLanguage
			Dim searchResult = m_BaseTableUtil.PerformPermissionDataOverWebService(language)
			Dim permissionData As New BindingList(Of SP.Internal.Automations.PermissionData)

			For Each result In searchResult
				Dim viewData = New SP.Internal.Automations.PermissionData With {.Rec_Value = result.Code, .Translated_Value = result.Translated_Value}
				If nationality = COUNTRY_CODE_CH AndAlso countryCode = COUNTRY_CODE_CH Then
					If viewData.Rec_Value = "13" Then permissionData.Add(viewData)
				Else
					permissionData.Add(viewData)
				End If
			Next

			If (permissionData Is Nothing) Then
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Bewilligungsdaten konnten nicht geladen werden."))
				m_Logger.LogWarning("no permission data was founded!")
				luePermission.Enabled = False
				lueForeignCategory.Enabled = False
			End If

			' If nationality = Switzerland and country is also switzerland -> show only switzerland permisssion
			'If nationality = COUNTRY_CODE_CH AndAlso countryCode = COUNTRY_CODE_CH Then
			'	Dim chPersmission = New BindingList(Of PermissionData)(permissionData.Where(Function(x) some, 13).ToList()) ' permissionData.Where(Function(data) data.Rec_Value = "13") '.ToList()
			'	If Not chPersmission Is Nothing Then
			'		permissionData = CType(chPersmission, BindingList(Of PermissionData))
			'	End If

			'End If


			Dim supressUIState = m_SuppressUIEvents
			m_SuppressUIEvents = True


			luePermission.Properties.DataSource = permissionData
			luePermission.Properties.ForceInitialize()

			If nationality = COUNTRY_CODE_CH Then
				luePermission.EditValue = Nothing ' PERMISSION_CODE_S
			Else
				luePermission.EditValue = currentPermission
			End If
			If Not permissionData Is Nothing Then luePermission.Properties.DropDownRows = permissionData.Count + 1
			LoadForeignCategoryDataOverWebService()

			m_SuppressUIEvents = supressUIState

			Return Not permissionData Is Nothing
		End Function

		Private Function LoadEmploymentTypeDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim result = m_BaseTableUtil.PerformEmploymentTypeDataOverWebService(language)
				lueEmploymentType.Properties.DataSource = result

				If Not result Is Nothing Then lueEmploymentType.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		Private Function LoadOtherEmploymentTypeDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim result = m_BaseTableUtil.PerformOtherEmploymentTypeDataOverWebService(language)
				lueOtherEmploymentType.Properties.DataSource = result

				If Not result Is Nothing Then lueOtherEmploymentType.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		Private Function LoadTypeOfStayDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim result = m_BaseTableUtil.PerformTypeOfStayDataOverWebService(language)
				lueTypeofStay.Properties.DataSource = result

				If Not result Is Nothing Then lueTypeofStay.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

		Private Function LoadForeignCategoryDataOverWebService() As Boolean
			Dim success As Boolean = True

			Try
				Dim language = m_InitializationData.UserData.UserLanguage
				Dim code = If(luePermission.EditValue Is Nothing, String.Empty, luePermission.EditValue)

				Dim result = m_BaseTableUtil.PerformForeignCategoryDataOverWebService(code, language)
				lueForeignCategory.Properties.DataSource = result

				If Not result Is Nothing Then lueForeignCategory.Properties.DropDownRows = result.Count + 1

				success = Not result Is Nothing


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function


		''' <summary>
		''' Handles new value event on community (Gemeinde) lookup edit.
		''' </summary>
		Private Sub OnLueCommunity_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueCommunity.ProcessNewValue

			If Not lueCommunity.Properties.DataSource Is Nothing Then

				Dim listOfCommunities = CType(lueCommunity.Properties.DataSource, BindingList(Of CommunityData))

				Dim newCommunity As New CommunityData With {.Translated_Value = e.DisplayValue.ToString()}
				listOfCommunities.Add(newCommunity)

				e.Handled = True
			End If
		End Sub

		''' <summary>
		'''  Handles edit value change of qst canton.
		''' </summary>
		Private Sub OnLueTaxCanton_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueTaxCanton.EditValueChanged
			If m_SuppressUIEvents Then
				Return
			End If

			If (LoadTaxInfoDataOverWebService(lueTaxCanton.EditValue, DateTime.Now.Year, m_InitializationData.UserData.UserLanguage)) Then
				ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()
				LoadCommunityOverWebService()
			End If

		End Sub

		''' <summary>
		'''  Handles edit value change of qst code.
		''' </summary>
		Private Sub lOnLueCode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCode.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadChurchTaxCodeAndNumberOfChildrenFromUIControlValues()

		End Sub

		''' <summary>
		'''  Handles edit value change of church tax code.
		''' </summary>
		Private Sub OnLueChurchTax_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueChurchTax.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadNumberOfChildrenFromUIControlValues()

		End Sub

		''' <summary>
		''' Handles edit value change of permission lookupedit.
		''' </summary>
		Private Sub OnLuePermission_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePermission.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			LoadForeignCategoryDataOverWebService()
			ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

		''' <summary>
		''' Handles certificate for residence received check box change event.
		''' </summary>
		Private Sub OnChkCertificateForResidenceReceived_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCertificateForResidenceReceived.CheckedChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

		''' <summary>
		''' Loads tax info data over webservice.
		''' </summary>
		''' <param name="canton">The canton.</param>
		''' <param name="year">The year.</param>
		''' <param name="language">The language.</param>
		Private Function LoadTaxInfoDataOverWebService(ByVal canton As String, ByVal year As Integer, ByVal language As String) As Boolean

			If String.IsNullOrEmpty(canton) Then
				canton = m_InitializationData.MDData.MDCanton
				m_Logger.LogWarning("S_Kanton is empty!")
			End If

			Dim success As Boolean = True

			Try
				'Dim ws = New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
				'ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxInfoServiceUrl)

				'Dim result = ws.LoadTaxInfoData(m_InitializationData.MDData.MDGuid, canton, year)
				'Dim result = m_BaseTableUtil.PerformTaxInfoDataOverWebService(m_InitializationData.MDData.MDGuid, canton, year)

				Dim mdGuid As String = "no md selected!"
				If String.IsNullOrWhiteSpace(m_InitializationData.MDData.MDGuid) Then mdGuid = "No md selected!" Else mdGuid = m_InitializationData.MDData.MDGuid
				Dim result = m_BaseTableUtil.PerformTaxInfoDataOverWebService(canton, year, language)

				If (result Is Nothing) Then
					HandleFailedWebServiceConnection()
					success = False
				Else
					m_TaxDataHelper = New TaxDataHelper(result, m_QSTTranslationData, m_ChurchTaxCodeTranslationData)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				HandleFailedWebServiceConnection()
				success = False
			End Try

			m_TaxInfoWebServiceCallOk = success

			Return success
		End Function

		''' <summary>
		''' Handles failed webservice connection.
		''' </summary>
		Private Sub HandleFailedWebServiceConnection()
			m_TaxDataHelper = Nothing

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Quellensteuer Daten konnten nicht über den WebService geladen werden."))
		End Sub

		''' <summary>
		''' Reloads QST, church tax code and number of children data from UI control values.
		''' </summary>
		Private Sub ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

			Dim hasSettingsChanged As Boolean = False ' not used

			LoadQSTCodeChurchCodeAndNumberChildren(
										 luePermission.EditValue,
										 m_UCMediator.SelectedBasiscData.CountryCode,
										 chkCertificateForResidenceReceived.Checked,
										 lueCode.EditValue,
										 lueChurchTax.EditValue,
										 lueNumberOfChildren.EditValue,
										 m_TaxDataHelper, m_QSTTranslationData, hasSettingsChanged)
		End Sub

		''' <summary>
		''' Reloads church tax code an number of children from u contol values.
		''' </summary>
		Private Sub ReLoadChurchTaxCodeAndNumberOfChildrenFromUIControlValues()

			Dim newChurchTaxCode = String.Empty ' not used
			Dim newChildren As Short = 0 ' not used
			LoadChurchTaxCodeAndNumberOfChildren(luePermission.EditValue, lueCode.EditValue, lueChurchTax.EditValue, lueNumberOfChildren.EditValue, m_TaxDataHelper, newChurchTaxCode, newChildren)

		End Sub

		''' <summary>
		''' Reloads number of children from u contol values.
		''' </summary>
		Private Sub ReLoadNumberOfChildrenFromUIControlValues()
			Dim newChildren As Short = 0
			LoadNumberOfChildren(luePermission.EditValue, lueCode.EditValue, lueChurchTax.EditValue, lueNumberOfChildren.EditValue, m_TaxDataHelper, newChildren)
		End Sub

		''' <summary>
		''' Loads QST code, church tax code and number of children data.
		''' </summary>
		''' <param name="permission">Permission code.</param>
		''' <param name="countryCode">The country code.</param>
		''' <param name="certificateForResidenceReceived">The certificate for residence received value.</param>
		''' <param name="currentQSTCode">The current QST code value.</param>
		''' <param name="currentChurchTaxCode">The current church tax code.</param>
		''' <param name="currentChildren">The current children code.</param>
		''' <param name="taxHelper">The tax helper.</param>
		''' <param name="qstTranslationData">The qst translation data.</param>
		''' <param name="hasSettingsChanged">Boolean flag indicating if settings have changed.</param>
		Private Sub LoadQSTCodeChurchCodeAndNumberChildren(
							  ByVal permission As String,
							  ByVal countryCode As String,
							  ByVal certificateForResidenceReceived As Boolean,
							  ByVal currentQSTCode As String,
							  ByVal currentChurchTaxCode As String,
							  ByVal currentChildren As Short,
							  ByVal taxHelper As TaxDataHelper,
								  ByVal qstTranslationData As IEnumerable(Of SP.Internal.Automations.TaxCodeData),
							  ByRef hasSettingsChanged As Boolean)

			If permission Is Nothing OrElse ((permission = PERMISSION_CODE_S OrElse permission = PERMISSION_CODE_C) OrElse String.IsNullOrEmpty(currentQSTCode)) Then
				' Replace empty string with Code_NoQST
				currentQSTCode = Code_NoQST
				'Else
				'	currentQSTCode = Nothing
			End If

			Dim newListOfCodeViewData As List(Of QSTCodeViewData) = New List(Of QSTCodeViewData)

			Dim newQSTCode As String = String.Empty
			Dim newChurchTaxCode = String.Empty
			Dim newChildren As Short = 0

			' Determine data of qst code field
			m_SalaryDataHelperFunctions.DetermineQSTCodeData(permission, countryCode, certificateForResidenceReceived, currentQSTCode, taxHelper, qstTranslationData,
							   newListOfCodeViewData, newQSTCode) ' Output

			If permission Is Nothing OrElse (permission <> PERMISSION_CODE_C) Then 'OrElse (permission <> PERMISSION_CODE_S AndAlso permission <> PERMISSION_CODE_C) Then
				'newQSTCode = Nothing
			End If

			' Load church tax and number of children field
			LoadChurchTaxCodeAndNumberOfChildren(permission, newQSTCode, currentChurchTaxCode, currentChildren, taxHelper, newChurchTaxCode, newChildren) ' Output

			' Check if setings has changed
			hasSettingsChanged = ((Not newQSTCode = currentQSTCode) Or
															(Not newChurchTaxCode = currentChurchTaxCode) Or
															(Not newChildren = currentChildren))

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set QST code
			lueCode.Properties.DataSource = newListOfCodeViewData
			If Not newListOfCodeViewData Is Nothing Then lueCode.Properties.DropDownRows = newListOfCodeViewData.Count + 1

			lueCode.Properties.ForceInitialize()
			lueCode.EditValue = newQSTCode

			m_SuppressUIEvents = supressUIStae

		End Sub

		''' <summary>
		''' Loads church tax code and number of children data.
		''' </summary>
		''' <param name="permission">The permission code.</param>
		''' <param name="currentQSTCode">The current QST code.</param>
		''' <param name="currentChurchTaxCode">The current churchtax code.</param>
		''' <param name="currentChildren">The current number of children.</param>
		''' <param name="taxHelper">The tax helper.</param>
		''' <param name="newChurchTaxCode">The new church tax code.</param>
		''' <param name="newChildren">The new number of children.</param>
		Private Sub LoadChurchTaxCodeAndNumberOfChildren(
							 ByVal permission As String,
							 ByVal currentQSTCode As String,
							 ByVal currentChurchTaxCode As String,
							 ByVal currentChildren As Short,
							 ByVal taxHelper As TaxDataHelper,
							 ByRef newChurchTaxCode As String,
							 ByRef newChildren As Short)

			If String.IsNullOrEmpty(currentQSTCode) Then
				' Replace empty string with Code_NoQST
				currentQSTCode = Code_NoQST
			End If

			Dim newListOfChurchTaxCode = New List(Of ChurchViewData)
			newChurchTaxCode = String.Empty

			' Determine data of church tax code
			m_SalaryDataHelperFunctions.DetermineChurchTaxCodeData(currentQSTCode, permission, currentChurchTaxCode, taxHelper,
									 newListOfChurchTaxCode, newChurchTaxCode) ' Output

			' Load data of number of children field
			LoadNumberOfChildren(permission, currentQSTCode, newChurchTaxCode, currentChildren, taxHelper, newChildren) ' Output

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set Church tax code
			lueChurchTax.Properties.DataSource = newListOfChurchTaxCode
			lueChurchTax.Properties.ForceInitialize()
			lueChurchTax.EditValue = newChurchTaxCode

			m_SuppressUIEvents = supressUIStae

		End Sub

		''' <summary>
		''' Load number of children data.
		''' </summary>
		''' <param name="permission">The permission code.</param>
		''' <param name="currentQSTCode">The current QST code.</param>
		''' <param name="currentCurchTaxCode">The current church tax code.</param>
		''' <param name="currentChildren">The current number of children.</param>
		''' <param name="taxHelper">The tax helper.</param>
		''' <param name="newChildren">The new number of children.</param>
		Private Sub LoadNumberOfChildren(
							 ByVal permission As String,
							 ByVal currentQSTCode As String,
							 ByVal currentCurchTaxCode As String,
							 ByVal currentChildren As Short,
							 ByVal taxHelper As TaxDataHelper,
							 ByRef newChildren As Short)

			Dim newListOfChildren As New List(Of NumberOfChildrenViewData)
			newChildren = 0

			m_SalaryDataHelperFunctions.DetermineNumberOfChildrenData(currentQSTCode, currentCurchTaxCode, permission, taxHelper, currentChildren, newListOfChildren, newChildren)

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set number of children 
			lueNumberOfChildren.Properties.DataSource = newListOfChildren
			lueNumberOfChildren.Properties.ForceInitialize()
			lueNumberOfChildren.EditValue = newChildren

			m_SuppressUIEvents = supressUIStae

		End Sub

		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim mdnr = m_UCMediator.SelectedMandantAndAdvisorData.MandantData.MandantNumber
			Dim xmlFilename As String = m_MandantData.GetSelectedMDFormDataXMLFilename(mdnr)

			Dim selectedPostCode = m_UCMediator.SelectedBasiscData.PostCode
			Dim selectedCountry = m_UCMediator.SelectedBasiscData.CountryCode
			Dim selectedNationality = m_UCMediator.SelectedBasiscData.Nationality
			Dim selectedCivilstate As String = m_UCMediator.SelectedBasiscData.CivilState

			Dim standardtaxcode As String = ""
			Dim childNumber As Integer = 0
			Dim churchTax As String = ""

			Dim sCanton As String = m_UCMediator.CommonDbAccess.LoadCantonByPostCode(selectedPostCode)
			If selectedCountry <> "CH" Then sCanton = String.Empty

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			If String.IsNullOrEmpty(sCanton) Then
				sCanton = m_InitializationData.MDData.MDCanton
			End If
			lueTaxCanton.EditValue = sCanton
			LoadCommunityOverWebService()

			Dim taxcodeforcivilstatesingleperson As String = m_Utility.ParseToString(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdnr),
														   String.Format("{0}/taxcodeforcivilstatesingleperson", FORM_XML_MAIN_KEY)), "A0Y")
			Dim taxcodeforcivilstatemarriedperson As String = m_Utility.ParseToString(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdnr),
														   String.Format("{0}/taxcodeforcivilstatemarriedperson", FORM_XML_MAIN_KEY)), "C0Y")

			Dim permissioncodeonnotswiss As String = m_Utility.ParseToString(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdnr),
														   String.Format("{0}/permissioncodeonnotswiss", FORM_XML_MAIN_KEY)), "10")

			LoadTaxInfoDataOverWebService(sCanton, Now.Year, m_InitializationData.UserData.UserLanguage)
			If selectedNationality <> "CH" Then
				luePermission.EditValue = permissioncodeonnotswiss
				'LoadQSTCodeChurchCodeAndNumberChildren(permissioncodeonnotswiss, selectedCountry, False, standardtaxcode, churchTax, childNumber, m_TaxDataHelper, m_QSTTranslationData, True)

			Else
				luePermission.EditValue = Nothing
				standardtaxcode = "0"
				childNumber = 0
				churchTax = String.Empty

			End If
			LoadQSTCodeChurchCodeAndNumberChildren(permissioncodeonnotswiss, selectedCountry, False, standardtaxcode, churchTax, childNumber, m_TaxDataHelper, m_QSTTranslationData, True)
			LoadForeignCategoryDataOverWebService()

			Dim emptyPermission As Boolean = luePermission.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePermission.EditValue)
			Dim requiredTAX As Boolean = True
			If selectedCountry = "LI" Then
				requiredTAX = False

			ElseIf selectedCountry = "CH" Then
				If Not (emptyPermission OrElse (luePermission.EditValue = "13")) Then
					requiredTAX = Not chkCHPartner.EditValue
				Else
					requiredTAX = False
				End If

			ElseIf selectedCountry = "FR" Then
				If chkCertificateForResidenceReceived.EditValue Then requiredTAX = False

			Else
				requiredTAX = True
			End If

			If requiredTAX Then

				If selectedCivilstate = "V" Then
					If Not String.IsNullOrWhiteSpace(taxcodeforcivilstatemarriedperson) AndAlso taxcodeforcivilstatemarriedperson.Length = 3 Then
						standardtaxcode = taxcodeforcivilstatemarriedperson.Substring(0, 1)
						childNumber = taxcodeforcivilstatemarriedperson.Substring(1, 1)
						churchTax = taxcodeforcivilstatemarriedperson.Substring(2, 1)
					End If
				Else
					If Not String.IsNullOrWhiteSpace(taxcodeforcivilstatesingleperson) AndAlso taxcodeforcivilstatesingleperson.Length = 3 Then
						standardtaxcode = taxcodeforcivilstatesingleperson.Substring(0, 1)
						childNumber = taxcodeforcivilstatesingleperson.Substring(1, 1)
						churchTax = taxcodeforcivilstatesingleperson.Substring(2, 1)
					End If
				End If
				LoadQSTCodeChurchCodeAndNumberChildren(permissioncodeonnotswiss, selectedCountry, False, standardtaxcode, churchTax, childNumber, m_TaxDataHelper, m_QSTTranslationData, True)

				lueCode.EditValue = standardtaxcode
				lueNumberOfChildren.EditValue = childNumber
				lueChurchTax.EditValue = churchTax
			End If

			m_SuppressUIEvents = supressUIStae


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

#End Region

	End Class

End Namespace
