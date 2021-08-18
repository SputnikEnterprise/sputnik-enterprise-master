Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Namespace UI

  Public Class ucPageWelcome

#Region "Private Constants"

    Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
    Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' The mandant data.
    ''' </summary>
    Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData

    ''' <summary>
    ''' The advisor data.
    ''' </summary>
    Private m_SelectedAdvisorData As SP.DatabaseAccess.Common.DataObjects.AdvisorData

    ''' <summary>
    ''' The advisors.
    ''' </summary>
    Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

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

      Try
        m_ProgPath = New ClsProgPath
        m_Mandant = New Mandant
      Catch ex As Exception
        m_Logger.LogError(ex.ToString)
      End Try

      AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueAdvisor.ButtonClick, AddressOf OnDropDown_ButtonClick

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected mandant and advisor data.
    ''' </summary>
    ''' <returns>Mandant and advisor data.</returns>
    Public ReadOnly Property SelectedMandantAndAdvisorData As InitMandantAndAdvisorData
      Get

        Dim data As New InitMandantAndAdvisorData With {
          .MandantData = m_SelectedMandantData,
          .AdvisorData = m_SelectedAdvisorData
        }

        Return data
      End Get
    End Property

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
        success = success AndAlso LoadMandantDropDownData()

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

      '  Reset drop downs and lists

      ResetMandantDropDown()
      ResetAdvisorDropDown()

      errorProvider.Clear()

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean

      errorProvider.Clear()

      Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

      Dim isValid As Boolean = True

      isValid = isValid And SetErrorIfInvalid(lueMandant, errorProvider, m_SelectedMandantData Is Nothing, errorText)

      If isValid Then

        ' Advisor
        Dim mustAdvisorBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(lueMandant.EditValue),
                                                                  String.Format("{0}/emplyoeeadvisorselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

        If mustAdvisorBeSelected Then
          isValid = isValid And SetErrorIfInvalid(lueAdvisor, errorProvider, m_SelectedAdvisorData Is Nothing, errorText)
        End If

      End If

      Return isValid

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      ' Group mandant data.
      Me.grpMandant.Text = m_Translate.GetSafeTranslationValue(Me.grpMandant.Text)

      Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
      Me.lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text)

    End Sub

    ''' <summary>
    ''' Resets the Mandant drop down.
    ''' </summary>
    Private Sub ResetMandantDropDown()

      lueMandant.Properties.DisplayMember = "MandantName1"
      lueMandant.Properties.ValueMember = "MandantNumber"

      lueMandant.Properties.Columns.Clear()
      lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
                                             .Width = 100,
                                             .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

      lueMandant.Properties.ShowFooter = False
      lueMandant.Properties.DropDownRows = 10
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

      lueAdvisor.Properties.DisplayMember = "UserFullname"
      lueAdvisor.Properties.ValueMember = "KST"

      Dim columns = lueAdvisor.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("KST", 0))
      columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

      lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
      lueAdvisor.Properties.AutoSearchColumnIndex = 1

      lueAdvisor.Properties.NullText = String.Empty
      lueAdvisor.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Loads the mandant drop down data.
    ''' </summary>
    Private Function LoadMandantDropDownData() As Boolean
      Dim mandantData = m_UCMediator.CommonDbAccess.LoadCompaniesListData()

      If (mandantData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
      End If

      lueMandant.Properties.DataSource = mandantData
      lueMandant.Properties.ForceInitialize()

      Return mandantData IsNot Nothing
    End Function

    ''' <summary>
    ''' Loads the advisor drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadAdvisorDropDownData() As Boolean

      m_Advisors = m_UCMediator.CommonDbAccess.LoadAdvisorData()

      If m_Advisors Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
      End If

      lueAdvisor.Properties.DataSource = m_Advisors
      lueAdvisor.Properties.ForceInitialize()

      Return Not m_Advisors Is Nothing

    End Function

    ''' <summary>
    ''' Handles change of mandant.
    ''' </summary>
    Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

      If Not lueMandant.EditValue Is Nothing Then

        Dim mandantData = CType(lueMandant.GetSelectedDataRow(), MandantData)

        m_SelectedMandantData = mandantData
        m_UCMediator.HandleChangeOfMandant(m_SelectedMandantData.MandantNumber)

        LoadAdvisorDropDownData()

      Else
        m_SelectedMandantData = Nothing

        lueAdvisor.EditValue = Nothing
        lueAdvisor.Properties.DataSource = Nothing

      End If

    End Sub

    ''' <summary>
    ''' Handles change of advisor.
    ''' </summary>
    Private Sub OnLueAdvisor_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdvisor.EditValueChanged

      If m_SuppressUIEvents Then
        Return
      End If

      If Not lueAdvisor.EditValue Is Nothing AndAlso
        Not m_Advisors Is Nothing Then

        Dim advisorData = CType(lueAdvisor.GetSelectedDataRow(), AdvisorData)
        m_SelectedAdvisorData = advisorData
      Else
        m_SelectedAdvisorData = Nothing
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

    ''' <summary>
    ''' Preselects data.
    ''' </summary>
    Private Sub PreselectData()

      If Not PreselectionData Is Nothing Then

        Dim supressUIEventState = m_SuppressUIEvents
        m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

        ' ---Mandant---
        If Not lueMandant.Properties.DataSource Is Nothing Then

          Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

          If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

            ' Mandant is required
            lueMandant.EditValue = PreselectionData.MDNr

          Else
            m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Mandant {0} konnte nicht vorselektiert werden."), PreselectionData.MDNr))
            m_SuppressUIEvents = supressUIEventState
            Return
          End If

        End If

        If Not String.IsNullOrEmpty(PreselectionData.Advisor) Then
          SelectAdvisor(lueAdvisor, PreselectionData.Advisor)
        Else
          SelectDefaultAdvisor()
        End If
       
        m_SuppressUIEvents = supressUIEventState
      Else
        ' No preselection data -> use mandant form initialization object.

        ' ---Mandant---
        If Not lueMandant.Properties.DataSource Is Nothing Then

          Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

          If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

            ' Mandant is required
            lueMandant.EditValue = m_InitializationData.MDData.MDNr

          Else
            m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Mandant {0} konnte nicht vorselektiert werden."), m_InitializationData.MDData.MDNr))
            Return
          End If

        End If

        SelectDefaultAdvisor()

      End If

    End Sub

    ''' <summary>
    ''' Selects an advisor and add missing advisor
    ''' </summary>
    ''' <param name="lueAdvisor">The advisor lookup edit.</param>
    ''' <param name="advisorKST">The advisor Kst.</param>
    Private Sub SelectAdvisor(lueAdvisor As LookUpEdit, advisorKST As String)
      Dim advisor = (From a In m_Advisors Where a.KST = advisorKST).FirstOrDefault
      If advisor Is Nothing Then
        'Add missing advisor
        m_Advisors.Add(New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = advisorKST})
      End If
      lueAdvisor.EditValue = advisorKST
    End Sub

    ''' <summary>
    ''' Selects the default advisor.
    ''' </summary>
    Private Sub SelectDefaultAdvisor()

      ' Select advisor
      Dim selectadvisorkst As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(lueMandant.EditValue),
                                                                       String.Format("{0}/selectadvisorkst", FORM_XML_MAIN_KEY)), False)

      If selectadvisorkst Then
        SelectAdvisor(lueAdvisor, m_InitializationData.UserData.UserKST)
      End If

    End Sub

#End Region

  End Class

End Namespace
