Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraEditors

Namespace UI

  Public Class ucPageEmployeeAdditionalData1

#Region "Private Consts"
    Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
    Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
#End Region

#Region "Private Fields"

    ''' <summary>
    ''' The cls prog path.
    ''' </summary>
    Private m_ProgPath As ClsProgPath

    ''' <summary>
    ''' The mandant.
    ''' </summary>
    Private m_Mandant As Mandant

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      Try
        m_ProgPath = New ClsProgPath
        m_Mandant = New Mandant
      Catch ex As Exception
        m_Logger.LogError(ex.ToString)
      End Try

      AddHandler lueState1.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueState2.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueContactInfo.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueCountryQualification.ButtonClick, AddressOf OnDropDown_ButtonClick

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

      If m_IsFirstPageActivation Then
        success = success AndAlso LoadDropDownData()

        PreselectData()

        m_IsFirstPageActivation = False
      End If

      Return success
    End Function

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_IsFirstPageActivation = True

      chkDStellen.Checked = False
      chkESLock.Checked = False

      'Qualification
      txtQualification.Text = String.Empty
      txtQualification.Tag = Nothing
      txtQualification.Properties.MaxLength = 70

      '  Reset drop downs and lists

      ResetEmployeeStates1DropDown()
      ResetEmployeeStates2DropDown()
      ResetContactInfoDataDropDown()
      ResetQulificationCountryDropDown()

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean

      ErrorProvider.Clear()

      Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

      Dim mdNr = m_UCMediator.SelectedMandantAndAdvisorData.MandantData.MandantNumber

      Dim mustFstateBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
                                                                 String.Format("{0}/emplyoeefstateselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
      Dim mustSstateBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
                                                                 String.Format("{0}/emplyoeesstateselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
      Dim mustContactBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
                                                                 String.Format("{0}/emplyoeecontactselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
      Dim mustQualificationBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
                                                                       String.Format("{0}/emplyoeequalificationselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

      Dim isValid As Boolean = True

      If mustFstateBeSelected And Not lueState1.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueState1, ErrorProvider, String.IsNullOrEmpty(lueState1.EditValue), errorText)
      End If
      If mustSstateBeSelected And Not lueState2.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueState2, ErrorProvider, String.IsNullOrEmpty(lueState2.EditValue), errorText)
      End If
      If mustContactBeSelected And Not lueContactInfo.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueContactInfo, ErrorProvider, String.IsNullOrEmpty(lueContactInfo.EditValue), errorText)
      End If

      If mustQualificationBeSelected Then
        isValid = isValid And SetErrorIfInvalid(txtQualification, ErrorProvider, String.IsNullOrEmpty(txtQualification.Text), errorText)
        isValid = isValid And SetErrorIfInvalid(lueCountryQualification, ErrorProvider, lueCountryQualification.EditValue Is Nothing, errorText)
      End If

      Return isValid

    End Function

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected employee additional data1 data.
    ''' </summary>
    ''' <returns>Employee additional data1.</returns>
    Public ReadOnly Property SelectedEmployeeAdditionalData1 As InitAdditionalEmployeeData1
      Get

        Dim data As New InitAdditionalEmployeeData1 With {
          .DStellen = chkDStellen.Checked,
          .NoES = chkESLock.Checked,
          .KStat1 = lueState1.EditValue,
          .KStat2 = lueState2.EditValue,
          .KontaktHow = lueContactInfo.EditValue,
          .Profession = txtQualification.Text,
          .ProfessionCode = If(txtQualification.Tag Is Nothing, Nothing, Convert.ToInt32(txtQualification.Tag)),
          .QLand = lueCountryQualification.EditValue
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

      ' Group Eingenschaften und Merkmale
      Me.grpMerkmale.Text = m_Translate.GetSafeTranslationValue(Me.grpMerkmale.Text)

			Me.chkDStellen.Text = m_Translate.GetSafeTranslationValue(Me.chkDStellen.Text, True)
			Me.chkESLock.Text = m_Translate.GetSafeTranslationValue(Me.chkESLock.Text, True)

			Me.lbl1status.Text = m_Translate.GetSafeTranslationValue(Me.lbl1status.Text, True)
			Me.lbl2status.Text = m_Translate.GetSafeTranslationValue(Me.lbl2status.Text, True)
			Me.lblkontakt.Text = m_Translate.GetSafeTranslationValue(Me.lblkontakt.Text, True)

      ' Group Qualifikation
			Me.grpQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.grpQualifikation.Text, True)
			Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text, True)
			Me.lblHerkunftslandQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblHerkunftslandQualifikation.Text, True)

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
    ''' Resets the qualification country drop down.
    ''' </summary>
    Private Sub ResetQulificationCountryDropDown()

      lueCountryQualification.Properties.ShowHeader = False
      lueCountryQualification.Properties.ShowFooter = False
      lueCountryQualification.Properties.DropDownRows = 50
      lueCountryQualification.Properties.DisplayMember = "Name"
      lueCountryQualification.Properties.ValueMember = "Code"

      Dim columns = lueCountryQualification.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Land")))

      lueCountryQualification.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueCountryQualification.Properties.SearchMode = SearchMode.AutoComplete
      lueCountryQualification.Properties.AutoSearchColumnIndex = 0

      lueCountryQualification.Properties.NullText = String.Empty
      lueCountryQualification.EditValue = Nothing
    End Sub

    ''' <summary>
    ''' Loads the drop down data.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadDropDownData() As Boolean
      Dim success As Boolean = True

      success = success AndAlso LoadEmployeeStates1DropDownData()
      success = success AndAlso LoadEmployeeStates2DropDownData()
      success = success AndAlso LoadContactInfoDropDownData()
      success = success AndAlso LoadQualificationCountryDropDownData()

      Return success

    End Function

    ''' <summary>
    ''' Load employee states1 drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeStates1DropDownData() As Boolean
      Dim employeeStates1 = m_UCMediator.EmployeeDbAccess.LoadEmployeeStateData1()

      If (employeeStates1 Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Stati1 konnten nicht geladen werden."))
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
      Dim customerStates2 = m_UCMediator.EmployeeDbAccess.LoadEmployeeStateData2()

      If (customerStates2 Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Stati2 konnten nicht geladen werden."))
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
      Dim contactInfoData = m_UCMediator.EmployeeDbAccess.LoadEmployeeContactsInfo()

      If (contactInfoData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Kontaktarten konnten nicht geladen werden."))
      End If

      lueContactInfo.Properties.DataSource = contactInfoData
      lueContactInfo.Properties.ForceInitialize()

      Return Not contactInfoData Is Nothing
    End Function

    ''' <summary>
    ''' Loads the qualification country drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadQualificationCountryDropDownData() As Boolean
      Dim countryData = m_UCMediator.CommonDbAccess.LoadCountryData()

      If (countryData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten (Qualifikation) konnten nicht geladen werden."))
      End If

      lueCountryQualification.Properties.DataSource = countryData
      lueCountryQualification.Properties.ForceInitialize()

      Return Not countryData Is Nothing
    End Function

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
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim mdnr = m_UCMediator.SelectedMandantAndAdvisorData.MandantData.MandantNumber
			Dim xmlFilename As String = m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr)

			Dim employeedes As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(xmlFilename,String.Format("{0}/employeedes", FORM_XML_MAIN_KEY)), False)
			Dim employeenoes As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(xmlFilename,String.Format("{0}/employeenoes", FORM_XML_MAIN_KEY)), False)

			Dim emplyoeefstate As String = m_ProgPath.GetXMLNodeValue(xmlFilename, String.Format("{0}/emplyoeefstate", FORM_XML_MAIN_KEY))
			Dim employeesstate As String = m_ProgPath.GetXMLNodeValue(xmlFilename, String.Format("{0}/employeesstate", FORM_XML_MAIN_KEY))

			Dim employeecontact As String = m_ProgPath.GetXMLNodeValue(xmlFilename, String.Format("{0}/employeecontact", FORM_XML_MAIN_KEY))
			Dim employeeCountryQualification As String = m_ProgPath.GetXMLNodeValue(xmlFilename, String.Format("{0}/employeecountryqualification", FORM_XML_MAIN_KEY))

			chkDStellen.Checked = employeedes
			chkESLock.Checked = employeenoes

			' First state
			If Not String.IsNullOrEmpty(emplyoeefstate) Then
				lueState1.EditValue = emplyoeefstate
			End If

			' Second state
			If Not String.IsNullOrEmpty(employeesstate) Then
				lueState2.EditValue = employeesstate
			End If

			' Contact
			If Not String.IsNullOrEmpty(employeecontact) Then
				lueContactInfo.EditValue = employeecontact
			End If

			If Not String.IsNullOrEmpty(employeeCountryQualification) Then
				lueCountryQualification.EditValue = employeeCountryQualification
			End If

		End Sub

    ''' <summary>
    ''' Handles drop down button clicks.
    ''' </summary>
    Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

      Const ID_OF_DELETE_BUTTON As Int32 = 1

      ' If delete button has been clicked reset the drop down.
      If e.Button.Index = ID_OF_DELETE_BUTTON Then

        If TypeOf sender Is BaseEdit Then
          If CType(sender, BaseEdit).Properties.ReadOnly Then
            ' nothing
          Else
            CType(sender, BaseEdit).EditValue = Nothing
          End If
        End If

      End If
    End Sub

#End Region

  End Class

End Namespace
