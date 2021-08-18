Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.ProgPath

Namespace UI

  Public Class ucPageEmployeeAdditionalData2

#Region "Private Consts"

    Private Const Code_NoQST As String = "0"
    Private Const PERMISSION_CODE_S = "S"
		Private Const PERMISSION_CODE_C = "C"
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

#End Region

#Region "Constructor"

    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

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
    End Sub

    ''' <summary>
    ''' Activates the page.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function ActivatePage() As Boolean

      Dim success As Boolean = True

      If (m_IsFirstPageActivation) Then

        LoadWebserviceUrl()
        success = success AndAlso LoadQSTAndChurchTaxCodeTranslationData()
        success = success AndAlso LoadDropDownData()

				'Dim basicdata = m_UCMediator.SelectedBasiscData

				'' Bewilligung (Take nationality and country code from other user control)
				'success = success AndAlso LoadPermissionDropDownData(basicdata.Nationality, basicdata.CountryCode, Nothing)

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

      '  Reset drop downs and lists
      ResetTaxCantonDropDown()
      ResetChurchTaxDropDown()
      ResetNumberOfChildrenDropDown()
      ResetCodeDropDown()
      ResetCommunityDropDown()
      ResetPermissionDropDown()

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

      Dim isValid As Boolean = True

      ' Check if web service call was ok
      isValid = isValid And SetErrorIfInvalid(lueTaxCanton, errorProvider, Not m_TaxInfoWebServiceCallOk, errotTextTaxInfoWebService)
    
      ' Check permission
      If mustPermissionBeSelected Then
        isValid = isValid And SetErrorIfInvalid(luePermission, errorProvider, luePermission.EditValue Is Nothing, errorText)
			End If

			If luePermission.EditValue = "S" Or luePermission.EditValue = "C" Or luePermission.EditValue = "K" Then
				mustPermissionToDateBeSelected = False
				mustBirthPlaceBeSelected = False
				mustCurchTaxBeSelected = False
				mustSCantonBeSelected = False

			Else

				If lueCode.EditValue Is Nothing Or lueCode.EditValue = "0" Then
					If mustTaxBeSelectedifnotCH Then isValid = isValid And SetErrorIfInvalid(lueCode, errorProvider, (lueCode.EditValue Is Nothing Or lueCode.EditValue = 0), errorText)
				Else
					mustCurchTaxBeSelected = True
				End If

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
			If mustCurchTaxBeSelected And Not lueChurchTax.Properties.DataSource Is Nothing Then

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
    ''' Handle change of country code.
    ''' </summary>
    Public Sub CountryCodeHasChanged()

      If m_IsFirstPageActivation Then
        ' Ignore information if page is not loaded yet.
        Return
      End If

			'Dim basicData = m_UCMediator.SelectedBasiscData

			'Dim selectedNationality = basicData.Nationality
			'Dim selectedCountry = basicData.CountryCode

			'LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
			'   ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

    ''' <summary>
    ''' Handle change of nationality.
    ''' </summary>
    Public Sub NationalityHasChanged()

      If m_IsFirstPageActivation Then
        ' Ignore information if page is not loaded yet.
        Return
      End If

			'Dim basicData = m_UCMediator.SelectedBasiscData

			'Dim selectedNationality = basicData.Nationality
			'   Dim selectedCountry = basicData.CountryCode

			'LoadPermissionDropDownData(selectedNationality, selectedCountry, luePermission.EditValue)
			'   ReLoadQSTCodeChurchTaxCodeAndNumberChildrenFromUIControlValues()

		End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected employee additional data2. data.
    ''' </summary>
    ''' <returns>Employee additional data1.</returns>
    Public ReadOnly Property SelectedEmployeeAdditionalData2 As InitAdditionalEmployeeData2
      Get

        Dim data As New InitAdditionalEmployeeData2 With {
          .Permission = luePermission.EditValue,
          .PermissionToDate = dateEditPermissionTo.EditValue,
          .BirthPlace = txtBirthPlace.Text,
          .S_Canton = lueTaxCanton.EditValue,
          .Residence = chkCertificateForResidenceReceived.Checked,
          .ANS_QST_Bis = dateEditAnsQSTBis.EditValue,
          .Q_Steuer = lueCode.EditValue,
          .ChurchTax = lueChurchTax.EditValue,
          .ChildsCount = lueNumberOfChildren.EditValue,
          .QSTCommunity = lueCommunity.EditValue
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
			Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)
			Me.lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBescheinigungGueltigBis.Text)
			Me.lblGemeinde.Text = m_Translate.GetSafeTranslationValue(Me.lblGemeinde.Text)
			Me.chkCHPartner.Text = m_Translate.GetSafeTranslationValue(Me.chkCHPartner.Text)

			' Quellensteuer
			Me.lblKantonFuerQuellensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblKantonFuerQuellensteuer.Text)
      Me.chkCertificateForResidenceReceived.Text = m_Translate.GetSafeTranslationValue(Me.chkCertificateForResidenceReceived.Text)
      Me.lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBescheinigungGueltigBis.Text)
      Me.lblCode.Text = m_Translate.GetSafeTranslationValue(Me.lblCode.Text)
      Me.lblKirchensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblKirchensteuer.Text)
      Me.lblAnzahlKinder.Text = m_Translate.GetSafeTranslationValue(Me.lblAnzahlKinder.Text)

      ' Group Bewilligung
			Me.grpBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.grpBewilligung.Text)
			Me.grpQST.Text = m_Translate.GetSafeTranslationValue(Me.grpQST.Text)
      Me.lblBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBis.Text)
      Me.lblHeimatort.Text = m_Translate.GetSafeTranslationValue(Me.lblHeimatort.Text)

    End Sub

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

      lueCommunity.Properties.DisplayMember = "CommunityName"
      lueCommunity.Properties.ValueMember = "CommunityName"

      Dim columns = lueCommunity.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("CommunityName", 0, String.Empty))

      lueCommunity.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueCommunity.Properties.SearchMode = SearchMode.AutoComplete
      lueCommunity.Properties.AutoSearchColumnIndex = 0
      lueCommunity.Properties.NullText = String.Empty
      lueCommunity.EditValue = Nothing
    End Sub

    ''' <summary>
    ''' Resets the permission state drop down.
    ''' </summary>
    Private Sub ResetPermissionDropDown()

      luePermission.Properties.DisplayMember = "TranslatedPermission"
      luePermission.Properties.ValueMember = "RecValue"

      Dim columns = luePermission.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("RecValue", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("TranslatedPermission", 0, m_Translate.GetSafeTranslationValue("Bewilligung")))

      luePermission.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      luePermission.Properties.SearchMode = SearchMode.AutoComplete
      luePermission.Properties.AutoSearchColumnIndex = 1
      luePermission.Properties.NullText = String.Empty
      luePermission.EditValue = Nothing
    End Sub

    ''' <summary>
    ''' Loads QST and church tax code translation data.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadQSTAndChurchTaxCodeTranslationData() As Boolean
      Dim success As Boolean = True

      m_QSTTranslationData = m_UCMediator.EmployeeDbAccess.LoadQSTData()
      m_ChurchTaxCodeTranslationData = m_UCMediator.EmployeeDbAccess.LoadChurchTaxCodeData()

      If m_QSTTranslationData Is Nothing Or
        m_ChurchTaxCodeTranslationData Is Nothing Then
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
      success = success AndAlso LoadCommunityDropDownData()

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

      Return Not cantonData Is Nothing
    End Function

    ''' <summary>
    ''' Loads the community drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
		Private Function LoadCommunityDropDownData() As Boolean
			Dim canton As String = String.Empty
			If Not lueTaxCanton.EditValue Is Nothing Then
				canton = lueTaxCanton.EditValue
			End If
			Dim communityData = m_UCMediator.EmployeeDbAccess.LoadEmployeeQSTCommunitiesWithCanton(canton)

			If (communityData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gemeindedaten konnten nicht geladen werden."))
			End If

			lueCommunity.Properties.DataSource = communityData
			lueCommunity.Properties.ForceInitialize()

			Return Not communityData Is Nothing
		End Function

    ''' <summary>
    ''' Loads permission drop down data.
    ''' </summary>
    '''<param name="nationality">The nationalilty code.</param>
    '''<param name="countryCode">The country code.</param>
    '''<param name="currentPermission">The current permission code.</param>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadPermissionDropDownData(ByVal nationality As String, ByVal countryCode As String, ByVal currentPermission As String) As Boolean
      Dim permissionData = m_UCMediator.CommonDbAccess.LoadPermissionData()

      If (permissionData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Bewilligungsdaten konnten nicht geladen werden."))
      End If

      ' If nationality = Switzerland and country is also switzerland -> show only switzerland permisssion
      If nationality = COUNTRY_CODE_CH And countryCode = COUNTRY_CODE_CH Then
        permissionData = permissionData.Where(Function(data) data.RecValue = PERMISSION_CODE_S).ToList()
      End If

      Dim supressUIState = m_SuppressUIEvents
      m_SuppressUIEvents = True

      luePermission.Properties.DataSource = permissionData
      luePermission.Properties.ForceInitialize()

      If nationality = COUNTRY_CODE_CH Then
        luePermission.EditValue = PERMISSION_CODE_S
      Else
        luePermission.EditValue = currentPermission
      End If

      m_SuppressUIEvents = supressUIState

      Return Not permissionData Is Nothing
    End Function

    ''' <summary>
    ''' Loads the web service url.
    ''' </summary>
    Private Sub LoadWebserviceUrl()

      Dim mdnr = m_UCMediator.SelectedMandantAndAdvisorData.MandantData.MandantNumber

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(mdnr, Now.Year))
				'm_TaxInfoServiceUrl = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVICE_URI, mdnr))

				'If String.IsNullOrWhiteSpace(m_TaxInfoServiceUrl) Then
				'	m_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
				'End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				'm_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
			End Try

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_TaxInfoServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI)

		End Sub

    ''' <summary>
    ''' Handles new value event on community (Gemeinde) lookup edit.
    ''' </summary>
    Private Sub OnLueCommunity_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueCommunity.ProcessNewValue

      If Not lueCommunity.Properties.DataSource Is Nothing Then

        Dim listOfCommunities = CType(lueCommunity.Properties.DataSource, List(Of EmployeeQSTCommunityData))

        Dim newCommunity As New EmployeeQSTCommunityData With {.CommunityName = e.DisplayValue.ToString()}
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
				LoadCommunityDropDownData()
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

				Dim ws = New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient

				ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxInfoServiceUrl)

				Dim result = ws.LoadTaxInfoData(m_InitializationData.MDData.MDGuid, canton, year)

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
                          ByVal qstTranslationData As IEnumerable(Of QSTData),
                          ByRef hasSettingsChanged As Boolean)

			If permission = PERMISSION_CODE_S AndAlso String.IsNullOrEmpty(currentQSTCode) Then
				' Replace empty string with Code_NoQST
				currentQSTCode = Code_NoQST
			Else
				currentQSTCode = Nothing
			End If

			Dim newListOfCodeViewData As List(Of QSTCodeViewData) = New List(Of QSTCodeViewData)

      Dim newQSTCode As String = String.Empty
      Dim newChurchTaxCode = String.Empty
      Dim newChildren As Short = 0

      ' Determine data of qst code field
      m_SalaryDataHelperFunctions.DetermineQSTCodeData(permission, countryCode, certificateForResidenceReceived, currentQSTCode, taxHelper, qstTranslationData,
                           newListOfCodeViewData, newQSTCode) ' Output

			If permission Is Nothing OrElse (permission <> PERMISSION_CODE_S AndAlso permission <> PERMISSION_CODE_C) Then
				newQSTCode = Nothing
			End If

			' Load church tax and number of children field
			LoadChurchTaxCodeAndNumberOfChildren(permission, newQSTCode, currentChurchTaxCode, currentChildren, taxHelper,
																					 newChurchTaxCode, newChildren)	' Output

			' Check if setings has changed
			hasSettingsChanged = ((Not newQSTCode = currentQSTCode) Or
														(Not newChurchTaxCode = currentChurchTaxCode) Or
														(Not newChildren = currentChildren))

			Dim supressUIStae = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Set QST code
			lueCode.Properties.DataSource = newListOfCodeViewData
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
      LoadNumberOfChildren(permission, currentQSTCode, newChurchTaxCode, currentChildren, taxHelper,
                           newChildren) ' Output

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

			'Dim selectedPostCode = m_UCMediator.SelectedBasiscData.PostCode

			'Dim sCanton As String = m_UCMediator.CommonDbAccess.LoadCantonByPostCode(selectedPostCode)

			'If String.IsNullOrEmpty(sCanton) Then
			'  sCanton = m_InitializationData.MDData.MDCanton
			'End If

			'If Not String.IsNullOrEmpty(sCanton) Then
			'  lueTaxCanton.EditValue = sCanton
			'End If

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
