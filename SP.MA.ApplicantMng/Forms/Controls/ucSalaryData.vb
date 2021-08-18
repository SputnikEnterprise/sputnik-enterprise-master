
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Namespace UI

	Public Class ucSalaryData

#Region "Private Consts"

		Private Const Code_NoQST As String = "0"
		Private Const PERMISSION_CODE_S = "S"
		Private Const COUNTRY_CODE_CH = "CH"

		Private Const MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicetaxinfoservices"
		Private Const DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI = "wsSPS_services/SPEmployeeTaxInfoService.asmx"
		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"

		Private m_TaxDataHelper As TaxDataHelper
		Private m_QSTTranslationData As IEnumerable(Of QSTData)
		Private m_ChurchTaxCodeTranslationData As IEnumerable(Of ChurchTaxCodeData)
		Private m_SalaryDataHelperFunctions As SalaryDataHelperFunctions

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

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			Try
				m_ProgPath = New ClsProgPath
				m_MandantData = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			m_SalaryDataHelperFunctions = New SalaryDataHelperFunctions()

			AddHandler lueTaxCanton.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditAnsQSTBis.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueChurchTax.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueNumberOfChildren.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCommunity.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePermission.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditPermissionTo.ButtonClick, AddressOf OnDropDown_ButtonClick

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

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				'm_TaxInfoServiceUrl = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

				'If String.IsNullOrWhiteSpace(m_TaxInfoServiceUrl) Then
				'  m_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
				'End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
			End Try

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_TaxInfoServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI)

		End Sub


		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			If (Not IsIntialControlDataLoaded) Then
				'success = success AndAlso LoadQSTAndChurchTaxCodeTranslationData()
				'success = success AndAlso LoadDropDownData()
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

			'chkCertificateForResidenceReceived.Checked = False
			'dateEditAnsQSTBis.EditValue = Nothing
			'dateEditPermissionTo.EditValue = Nothing

			'txtBirthPlace.Text = String.Empty
			'txtBirthPlace.Properties.MaxLength = 100

			'  Reset drop downs and lists
			'ResetTaxCantonDropDown()
			'ResetChurchTaxDropDown()
			'ResetNumberOfChildrenDropDown()
			'ResetCodeDropDown()
			'ResetCommunityDropDown()
			'ResetPermissionDropDown()

			'Dim userSec118 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 118, m_InitializationData.MDData.MDNr)
			'luePermission.Enabled = userSec118
			'dateEditPermissionTo.Enabled = userSec118

			'Dim userSec117 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 117, m_InitializationData.MDData.MDNr)
			'lueCode.Enabled = userSec117
			'lueChurchTax.Enabled = userSec117
			'lueNumberOfChildren.Enabled = userSec117

			m_SuppressUIEvents = suppressUIEventsState

			errorProvider.Clear()
		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			errorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			isValid = isValid AndAlso SetErrorIfInvalid(lueCode, errorProvider, lueCode.EditValue Is Nothing, errorText)


			Dim mdNr = m_InitializationData.MDData.MDNr

			Dim mustPermissionBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																		   String.Format("{0}/emplyoeepermitselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustPermissionToDateBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
																	 String.Format("{0}/emplyoeepermitdateselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustBirthPlaceBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
															   String.Format("{0}/emplyoeehometownselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustCurchTaxBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
														 String.Format("{0}/emplyoeekirchensteuerselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim mustSCantonBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantData.GetSelectedMDFormDataXMLFilename(mdNr),
												   String.Format("{0}/emplyoeesteuercantonselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)


			' Check permission
			If mustPermissionBeSelected Then
				isValid = isValid And SetErrorIfInvalid(luePermission, errorProvider, luePermission.EditValue Is Nothing, errorText)
			End If
			If luePermission.EditValue = "S" Then
				mustPermissionToDateBeSelected = False
				mustBirthPlaceBeSelected = False
			End If

			' Check permissionTo date
			If mustPermissionToDateBeSelected Then
				isValid = isValid And SetErrorIfInvalid(dateEditPermissionTo, errorProvider, dateEditPermissionTo.EditValue Is Nothing, errorText)
			End If

			' Check birthplace
			If mustBirthPlaceBeSelected Then
				isValid = isValid And SetErrorIfInvalid(txtBirthPlace, errorProvider, String.IsNullOrWhiteSpace(txtBirthPlace.Text), errorText)
			End If

			' Check church tax
			If Not lueChurchTax.Properties.DataSource Is Nothing Then

				Dim listOfChurchTaxCodes = CType(lueChurchTax.Properties.DataSource, List(Of ChurchViewData))

				isValid = isValid And SetErrorIfInvalid(lueChurchTax, errorProvider, lueChurchTax.EditValue Is Nothing And listOfChurchTaxCodes.Count > 0, errorText)
			End If

			' Check S_Canton (Steuerkanton)
			If mustSCantonBeSelected Then
				isValid = isValid And SetErrorIfInvalid(lueTaxCanton, errorProvider, lueTaxCanton.EditValue Is Nothing, errorText)
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
				employeeMasterData.S_Canton = lueTaxCanton.EditValue
				employeeMasterData.Residence = chkCertificateForResidenceReceived.Checked
				employeeMasterData.ANS_OST_Bis = dateEditAnsQSTBis.EditValue

				employeeMasterData.Q_Steuer = lueCode.EditValue
				employeeMasterData.ChurchTax = lueChurchTax.EditValue
				employeeMasterData.ChildsCount = lueNumberOfChildren.EditValue

				employeeMasterData.QSTCommunity = lueCommunity.EditValue

				employeeMasterData.Permission = luePermission.EditValue
				employeeMasterData.PermissionToDate = dateEditPermissionTo.EditValue
				employeeMasterData.BirthPlace = txtBirthPlace.Text
				employeeMasterData.CHPartner = chkCHPartner.EditValue
				employeeMasterData.ValidatePermissionWithTax = True
				employeeMasterData.NoSpecialTax = chkNoSpecialTax.EditValue

			End If

		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			' Do nothing
		End Sub

		''' <summary>
		''' Handle change of country code.
		''' </summary>
		''' <param name="employeeNumber">The emplyoee number.</param>
		Public Sub CountryCodeHasChanged(ByVal employeeNumber As Integer)

			If (m_EmployeeNumber = employeeNumber) Then

				Dim selectedNationality = m_UCMediator.GetNationalityFromUi(m_EmployeeNumber)
				Dim selectedCountry = m_UCMediator.GetCountryCodeFromUi(m_EmployeeNumber)

				'LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
				'ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()
			Else
				' Ignore this event because the employee number does not match with the requested employee number.
			End If

		End Sub

		''' <summary>
		''' Handle change of nationality.
		''' </summary>
		''' <param name="employeeNumber">The emplyoee number.</param>
		Public Sub NationalityHasChanged(ByVal employeeNumber As Integer)

			If (m_EmployeeNumber = employeeNumber) Then
				Dim selectedNationality = m_UCMediator.GetNationalityFromUi(m_EmployeeNumber)
				Dim selectedCountry = m_UCMediator.GetCountryCodeFromUi(m_EmployeeNumber)

				'LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
				'ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()
			Else
				' Ignore this event because the employee number does not match with the requested employee number.
			End If

		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Group Bewilligung
			Me.grpBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.grpBewilligung.Text)
			Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)
			Me.lblBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBis.Text)
			Me.lblHeimatort.Text = m_Translate.GetSafeTranslationValue(Me.lblHeimatort.Text)
			Me.chkCHPartner.Text = m_Translate.GetSafeTranslationValue(Me.chkCHPartner.Text)
			Me.chkNoSpecialTax.Text = m_Translate.GetSafeTranslationValue(Me.chkNoSpecialTax.Text)

			' Quellensteuer
			Me.grpQST.Text = m_Translate.GetSafeTranslationValue(Me.grpQST.Text)
			Me.lblKantonFuerQuellensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblKantonFuerQuellensteuer.Text)
			Me.chkCertificateForResidenceReceived.Text = m_Translate.GetSafeTranslationValue(Me.chkCertificateForResidenceReceived.Text)
			Me.lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBescheinigungGueltigBis.Text)
			Me.lblCode.Text = m_Translate.GetSafeTranslationValue(Me.lblCode.Text)
			Me.lblKirchensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblKirchensteuer.Text)
			Me.lblAnzahlKinder.Text = m_Translate.GetSafeTranslationValue(Me.lblAnzahlKinder.Text)
			Me.lblGemeinde.Text = m_Translate.GetSafeTranslationValue(Me.lblGemeinde.Text)

		End Sub

		'''' <summary>
		'''' Resets the tax canton drop down.
		'''' </summary>
		'Private Sub ResetTaxCantonDropDown()

		'  lueTaxCanton.Properties.DisplayMember = "Description"
		'  lueTaxCanton.Properties.ValueMember = "GetField"

		'  Dim columns = lueTaxCanton.Properties.Columns
		'  columns.Clear()
		'  columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
		'  columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Kanton")))

		'  lueTaxCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'  lueTaxCanton.Properties.SearchMode = SearchMode.AutoComplete
		'  lueTaxCanton.Properties.AutoSearchColumnIndex = 0
		'  lueTaxCanton.Properties.NullText = String.Empty
		'  lueTaxCanton.EditValue = Nothing
		'End Sub

		'''' <summary>
		'''' Resets the code drop down.
		'''' </summary>
		'Private Sub ResetCodeDropDown()

		'  lueCode.Properties.DisplayMember = "CodeAndTranslation"
		'  lueCode.Properties.ValueMember = "Code"

		'  Dim columns = lueCode.Properties.Columns
		'  columns.Clear()
		'  columns.Add(New LookUpColumnInfo("Code", 0, String.Empty))
		'  columns.Add(New LookUpColumnInfo("Translation", 0, String.Empty))

		'  lueCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'  lueCode.Properties.SearchMode = SearchMode.AutoComplete
		'  lueCode.Properties.AutoSearchColumnIndex = 1
		'  lueCode.Properties.NullText = String.Empty
		'  lueCode.EditValue = Nothing
		'End Sub

		'''' <summary>
		'''' Resets the church tax drop down.
		'''' </summary>
		'Private Sub ResetChurchTaxDropDown()

		'  lueChurchTax.Properties.DisplayMember = "ChurchCodeAndTranslation"
		'  lueChurchTax.Properties.ValueMember = "ChurchTaxCode"

		'  Dim columns = lueChurchTax.Properties.Columns
		'  columns.Clear()
		'  columns.Add(New LookUpColumnInfo("ChurchTaxCode", 0, String.Empty))
		'  columns.Add(New LookUpColumnInfo("ChurchCodeAndTranslation", 0, String.Empty))

		'  lueChurchTax.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'  lueChurchTax.Properties.SearchMode = SearchMode.AutoComplete
		'  lueChurchTax.Properties.AutoSearchColumnIndex = 1
		'  lueChurchTax.Properties.NullText = String.Empty
		'  lueChurchTax.EditValue = Nothing
		'End Sub

		'''' <summary>
		'''' Resets the number of children drop down.
		'''' </summary>
		'Private Sub ResetNumberOfChildrenDropDown()

		'  lueNumberOfChildren.Properties.DisplayMember = "NumberOfChildren"
		'  lueNumberOfChildren.Properties.ValueMember = "NumberOfChildren"

		'  Dim columns = lueNumberOfChildren.Properties.Columns
		'  columns.Clear()
		'  columns.Add(New LookUpColumnInfo("NumberOfChildren", 0, String.Empty))

		'  lueNumberOfChildren.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'  lueNumberOfChildren.Properties.SearchMode = SearchMode.AutoComplete
		'  lueNumberOfChildren.Properties.AutoSearchColumnIndex = 1
		'  lueNumberOfChildren.Properties.NullText = String.Empty
		'  lueNumberOfChildren.EditValue = Nothing
		'End Sub

		' ''' <summary>
		' ''' Resets the community drop down.
		' ''' </summary>
		' Private Sub ResetCommunityDropDown()

		'   lueCommunity.Properties.SearchMode = SearchMode.OnlyInPopup
		'   lueCommunity.Properties.TextEditStyle = TextEditStyles.Standard

		'   lueCommunity.Properties.DisplayMember = "CommunityName"
		'   lueCommunity.Properties.ValueMember = "CommunityName"

		'   Dim columns = lueCommunity.Properties.Columns
		'   columns.Clear()
		'   columns.Add(New LookUpColumnInfo("CommunityName", 0, String.Empty))

		'   lueCommunity.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'   lueCommunity.Properties.SearchMode = SearchMode.AutoComplete
		'   lueCommunity.Properties.AutoSearchColumnIndex = 0
		'   lueCommunity.Properties.NullText = String.Empty
		'   lueCommunity.EditValue = Nothing
		' End Sub

		' ''' <summary>
		' ''' Resets the permission state drop down.
		' ''' </summary>
		' Private Sub ResetPermissionDropDown()

		'   luePermission.Properties.DisplayMember = "TranslatedPermission"
		'   luePermission.Properties.ValueMember = "RecValue"

		'   Dim columns = luePermission.Properties.Columns
		'   columns.Clear()
		'   columns.Add(New LookUpColumnInfo("RecValue", 0, String.Empty))
		'columns.Add(New LookUpColumnInfo("TranslatedPermission", 0, m_Translate.GetSafeTranslationValue("Bewilligung")))

		'   luePermission.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'   luePermission.Properties.SearchMode = SearchMode.AutoComplete
		'   luePermission.Properties.AutoSearchColumnIndex = 1
		'   luePermission.Properties.NullText = String.Empty
		'   luePermission.EditValue = Nothing
		' End Sub


		'''' <summary>
		'''' Loads the drop down data.
		'''' </summary>
		'''' <returns>Boolean value indicating success.</returns>
		'Private Function LoadDropDownData() As Boolean
		'    Dim success As Boolean = True

		'    success = success AndAlso LoadTaxCantonDropDownData()
		'	'success = success AndAlso LoadCommunityDropDownData()

		'	Return success
		'  End Function

		'''' <summary>
		'''' Loads QST and church tax code translation data.
		'''' </summary>
		'''' <returns>Boolean value indicating success.</returns>
		'Private Function LoadQSTAndChurchTaxCodeTranslationData() As Boolean
		'  Dim success As Boolean = True

		'  m_QSTTranslationData = m_EmployeeDataAccess.LoadQSTData()
		'  m_ChurchTaxCodeTranslationData = m_EmployeeDataAccess.LoadChurchTaxCodeData()

		'  If m_QSTTranslationData Is Nothing Or
		'    m_ChurchTaxCodeTranslationData Is Nothing Then
		'    success = False
		'  End If

		'  Return success

		'End Function

		'''' <summary>
		'''' Loads the tax canton drop down data.
		'''' </summary>
		''''<returns>Boolean flag indicating success.</returns>
		'Private Function LoadTaxCantonDropDownData() As Boolean
		'  Dim cantonData = m_CommonDatabaseAccess.LoadCantonData()

		'  If (cantonData Is Nothing) Then
		'    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantondaten (für Quellensteuer) konnten nicht geladen werden."))
		'  End If

		'  lueTaxCanton.Properties.DataSource = cantonData
		'  lueTaxCanton.Properties.ForceInitialize()

		'  Return Not cantonData Is Nothing
		'End Function

		'  ''' <summary>
		'  ''' Loads the community drop down data.
		'  ''' </summary>
		'  '''<returns>Boolean flag indicating success.</returns>
		'Private Function LoadCommunityDropDownData() As Boolean
		'	Dim canton As String = String.Empty
		'	If lueTaxCanton.EditValue Is Nothing Then

		'	Else
		'		canton = lueTaxCanton.EditValue
		'	End If
		'	Dim communityData = m_EmployeeDataAccess.LoadEmployeeQSTCommunitiesWithCanton(canton)

		'	If (communityData Is Nothing) Then
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gemeindedaten konnten nicht geladen werden."))
		'	End If

		'	lueCommunity.Properties.DataSource = communityData
		'	lueCommunity.Properties.ForceInitialize()

		'	Return Not communityData Is Nothing
		'End Function

		'''' <summary>
		'''' Loads permission drop down data.
		'''' </summary>
		''''<param name="nationality">The nationalilty code.</param>
		''''<param name="countryCode">The country code.</param>
		''''<param name="currentPermission">The current permission code.</param>
		''''<returns>Boolean flag indicating success.</returns>
		'Private Function LoadPermissionDropDownData(ByVal nationality As String, ByVal countryCode As String, ByVal currentPermission As String) As Boolean
		'  Dim permissionData = m_CommonDatabaseAccess.LoadPermissionData()

		'  If (permissionData Is Nothing) Then
		'    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Bewilligungsdaten konnten nicht geladen werden."))
		'  End If

		'  ' If nationality = Switzerland and country is also switzerland -> show only switzerland permisssion
		'  If nationality = COUNTRY_CODE_CH And countryCode = COUNTRY_CODE_CH Then
		'    permissionData = permissionData.Where(Function(data) data.RecValue = PERMISSION_CODE_S).ToList()
		'  End If

		'  Dim supressUIState = m_SuppressUIEvents
		'  m_SuppressUIEvents = True

		'  luePermission.Properties.DataSource = permissionData
		'  luePermission.Properties.ForceInitialize()

		'  If nationality = COUNTRY_CODE_CH Then
		'    luePermission.EditValue = PERMISSION_CODE_S
		'  Else
		'    luePermission.EditValue = currentPermission
		'  End If

		'  m_SuppressUIEvents = supressUIState

		'  Return Not permissionData Is Nothing
		'End Function

		''' <summary>
		'''  Loads responsible person data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

			success = LoadEmployeeMasterData(employeeNumber)

			errorProvider.Clear()

			Return success
		End Function

		''' <summary>
		'''  Loads the employee master data..
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		Private Function LoadEmployeeMasterData(ByVal employeeNumber As Integer) As Boolean
			Dim success As Boolean = True

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(employeeNumber, False)

			If (employeeMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnangaben(1) konnten nicht geladen werden."))
				Return False
			End If

			m_SuppressUIEvents = suppressUIEventsState

			Return success

		End Function



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
