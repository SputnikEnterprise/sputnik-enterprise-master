
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Initialization
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Namespace UI

  Public Class ucBaseControl
    Inherits DevExpress.XtraEditors.XtraUserControl

#Region "Protected Fields"

    ''' <summary>
    ''' The Initialization data.
    ''' </summary>
    Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

    ''' <summary>
    ''' Thre translation value helper.
    ''' </summary>
    Protected m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Protected m_AdvancePaymentDbAcccess As IAdvancePaymentDatabaseAccess

    ''' <summary>
    ''' The common database access.
    ''' </summary>
    Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

    ''' <summary>
    ''' The settings manager.
    ''' </summary>
    Protected m_SettingsManager As ISettingsManager

    ''' <summary>
    ''' The SPProgUtility object.
    ''' </summary>
    Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

    ''' <summary>
    ''' Boolean flag indicating if the inital control data has been loaded.
    ''' </summary>
    Protected m_IsIntialControlDataLoaded As Boolean

    ''' <summary>
    ''' UI Utility functions.
    ''' </summary>
    Protected m_UtilityUI As UtilityUI

    ''' <summary>
    ''' The logger.
    ''' </summary>
    Protected Shared m_Logger As ILogger = New Logger()

    ''' <summary>
    ''' Boolean flag indicating if form is initializing.
    ''' </summary>
    Protected m_SuppressUIEvents As Boolean = True

    ''' <summary>
    ''' Communication support between controls.
    ''' </summary>
    Protected m_UCMediator As UserControlFormMediator

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()
      m_SettingsManager = New SettingsManager
      m_UtilityUI = New UtilityUI
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Boolean flag indicating if the inital control data has been loaded.
    ''' </summary>
    Public Property IsIntialControlDataLoaded As Boolean
      Get
        Return m_IsIntialControlDataLoaded
      End Get

      Set(value As Boolean)
        m_IsIntialControlDataLoaded = value
      End Set

    End Property

    ''' <summary>
    ''' Gets or set the UC mediator.
    ''' </summary>
    Public Property UCMediator As UserControlFormMediator
      Get
        Return m_UCMediator
      End Get
      Set(value As UserControlFormMediator)
        m_UCMediator = value
      End Set
    End Property

#End Region

#Region "Protected Properties"

    ''' <summary>
    ''' Gets a boolean flag indiciating if the ui events should be suppressed.
    ''' </summary>
    Protected ReadOnly Property AreUIEventsSuppressed As Boolean
      Get
        Return m_SuppressUIEvents
      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Inits the control with configuration information.
    ''' </summary>
    '''<param name="initializationClass">The initialization class.</param>
    '''<param name="translationHelper">The translation helper.</param>
    Public Overridable Sub InitWithConfigurationData(ByVal initializationClass As InitializeClass, ByVal translationHelper As TranslateValuesHelper)

      m_InitializationData = initializationClass
      m_Translate = translationHelper

      m_AdvancePaymentDbAcccess = New DatabaseAccess.AdvancePaymentMng.AdvancePaymentDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
      m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

      TranslateControls()

    End Sub

    ''' <summary>
    ''' Loads data of the active advance payment.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Overridable Function LoadDataOfActiveAdvancePayment() As Boolean
      Return True
    End Function

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    Public Overridable Function Activate() As Boolean
      Return True
    End Function

    ''' <summary>
    ''' Deactivates the control.
    ''' </summary>
    Public Overridable Sub Deactivate()
    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overridable Sub Reset()
      ' Do not make this method abstract because the WinForms designer does not like that.
    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overridable Function ValidateData() As Boolean
      Return True
    End Function

    ''' <summary>
    ''' Saves additional data.
    ''' </summary>
    Public Overridable Function SaveAdditionalData() As Boolean
      Return True
    End Function

    ''' <summary>
    ''' Merges the ZG master data.
    ''' </summary>
    ''' <param name="zgMasterData">The ZG master data object where the data gets filled into.</param>
    Public Overridable Sub MergeCustomerMasterData(ByVal zgMasterData As ZGMasterData)
    End Sub

    ''' <summary>
    ''' Sets readonly state of controls.
    ''' </summary>
    ''' <param name="isReadonly">Boolean flag indicating if the controls should be readonly.</param>
    Public Overridable Sub SetReadonlyStateOfControls(ByVal isReadonly)
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overridable Sub CleanUp()
    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overridable Sub TranslateControls()
      ' Should be overriden by controls which need translation.
    End Sub

    ''' <summary>
    ''' Sets the valid state of a control.
    ''' </summary>
    ''' <param name="control">The control to validate.</param>
    ''' <param name="errorProvider">The error providor.</param>
    ''' <param name="invalid">Boolean flag if data is invalid.</param>
    ''' <param name="errorText">The error text.</param>
    ''' <returns>Valid flag</returns>
    Protected Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

      If (invalid) Then
        errorProvider.SetError(control, errorText)
      Else
        errorProvider.SetError(control, String.Empty)
      End If

      Return Not invalid

    End Function

    ''' <summary>
    ''' Sets the suppress UI events state.
    ''' </summary>
    ''' <param name="shouldEventsBeSuppressed">Boolean flag indicating the  UI events should be suppressed.</param>
    ''' <returns>Previous state of suppress events.</returns>
    ''' <remarks></remarks>
    Protected Function SetSuppressUIEventsState(ByVal shouldEventsBeSuppressed As Boolean)

      Dim orginalState = m_SuppressUIEvents
      m_SuppressUIEvents = shouldEventsBeSuppressed

      Return orginalState

    End Function

#End Region

    Private Sub InitializeComponent()
      Me.SuspendLayout()
      '
      'ucBaseControl
      '
      Me.Name = "ucBaseControl"
      Me.Size = New System.Drawing.Size(156, 154)
      Me.ResumeLayout(False)

    End Sub
  End Class

End Namespace
