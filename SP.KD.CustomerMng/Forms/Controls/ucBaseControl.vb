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
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Reflection
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors
Imports System.Text.RegularExpressions
Imports DevExpress.XtraNavBar
Imports SPS.Listing.Print.Utility
Imports SP.KD.CustomerMng.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common

Namespace UI

  ''' <summary>
  ''' Base user control.
  ''' </summary>
  Public Class ucBaseControl
    Inherits DevExpress.XtraEditors.XtraUserControl

#Region "Private Fields"
#End Region

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
    Protected m_DataAccess As ICustomerDatabaseAccess

    ''' <summary>
    ''' The common database access.
    ''' </summary>
    Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

    ''' <summary>
    ''' The settings manager.
    ''' </summary>
    Protected m_SettingsManager As ISettingsManager

    ''' <summary>
    ''' Contains the customer number of the loaded customer.
    ''' </summary>
    Protected m_CustomerNumber As Integer?

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
    ''' Gets the customer number.
    ''' </summary>
    ''' <returns>The customer number.</returns>
    Public ReadOnly Property CustomerNumber As Integer?
      Get
        Return m_CustomerNumber
      End Get
    End Property

    ''' <summary>
    ''' Boolean flag indicating if customer data is loaded.
    ''' </summary>
    Public ReadOnly Property IsCustomerDataLoaded As Boolean
      Get
        Return m_CustomerNumber.HasValue
      End Get

    End Property

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

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Inits the control with configuration information.
    ''' </summary>
    '''<param name="initializationClass">The initialization class.</param>
    '''<param name="translationHelper">The translation helper.</param>
    Public Overridable Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)

      m_InitializationData = initializationClass
      m_Translate = translationHelper

      m_DataAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
      m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

      TranslateControls()

    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Overridable Function Activate(ByVal customerNumber As Integer?) As Boolean
      ' Do not make this method abstract because the WinForms designer does not like that.
      Throw New NotImplementedException("The methods must be overriden by subclass.")
    End Function

    ''' <summary>
    ''' Deactivates the control.
    ''' </summary>
    Public Overridable Sub Deactivate()
      ' Do not make this method abstract because the WinForms designer does not like that.
      Throw New NotImplementedException("The methods must be overriden by subclass.")
    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overridable Sub Reset()
      ' Do not make this method abstract because the WinForms designer does not like that.
      Throw New NotImplementedException("The methods must be overriden by subclass.")
    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overridable Function ValidateData() As Boolean
      ' Do not make this function abstract because the WinForms designer does not like that.
      Throw New NotImplementedException("The function must be overriden by subclass.")
    End Function

    ''' <summary>
    ''' Merges the custmer master data.
    ''' </summary>
    ''' <param name="customerMasterData">The customer master data object where the data gets filled into.</param>
    Public Overridable Sub MergeCustomerMasterData(ByVal customerMasterData As CustomerMasterData)
      ' Do not make this method abstract because the WinForms designer does not like that.
      Throw New NotImplementedException("The methods must be overriden by subclass.")
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overridable Sub CleanUp()
      ' Do not make this method abstract because the WinForms designer does not like that.
      Throw New NotImplementedException("The methods must be overriden by subclass.")
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

#End Region

    Private Sub InitializeComponent()
      Me.SuspendLayout()
      '
      'ucBaseControl
      '
      Me.Appearance.BackColor = System.Drawing.Color.Transparent
      Me.Appearance.Options.UseBackColor = True
      Me.Name = "ucBaseControl"
      Me.Padding = New System.Windows.Forms.Padding(5)
      Me.ResumeLayout(False)

    End Sub
  End Class

End Namespace