Imports System.Reflection.Assembly
Imports System.IO
Imports System.ComponentModel
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports System.IO.File
'Imports System.Data
'Imports System.Data.SqlClient
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
Imports DevExpress.XtraEditors
Imports System.Text.RegularExpressions

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.Infrastructure.Initialization
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Applicant

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
		Protected m_EmployeeDataAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The data access object.
		''' </summary>
		Protected m_applicationDataAccess As IAppDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Protected m_SettingsManager As ISettingsManager

    Protected m_customerID As String

    ''' <summary>
    ''' Contains the employee number
    ''' </summary>
    Protected m_EmployeeNumber As Integer?

    ''' <summary>
    ''' cvlProfileID
    ''' </summary>
    Protected m_CVLProfileID As Integer?

    ''' <summary>
    ''' cvlWorkID
    ''' </summary>
    Protected m_CVLWorkID As Integer?

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
		''' Utility functions.
		''' </summary>
		Protected m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Protected Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Protected m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' Communication support between controls.
		''' </summary>
		Protected m_UCMediator As UserControlFromMediator

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()
			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility()
		End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the customer_id.
    ''' </summary>
    Public ReadOnly Property CustomerID As String
      Get
        Return m_customerID
      End Get
    End Property

    ''' <summary>
    ''' Gets the employee number.
    ''' </summary>
    ''' <returns>The employee number</returns>
    Public ReadOnly Property EmployeeNumber As Integer?
			Get
				Return m_EmployeeNumber
			End Get
		End Property

    ''' <summary>
    ''' Gets the employee cvlProfileID.
    ''' </summary>
    Public ReadOnly Property EmployeeCVLProfileID As Integer?
      Get
        Return m_CVLProfileID
      End Get
    End Property

    ''' <summary>
    ''' Boolean flag indicating if employee data is loaded.
    ''' </summary>
    Public ReadOnly Property IsEmployeeDataLoaded As Boolean
			Get
				Return m_EmployeeNumber.HasValue
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

		''' <summary>
		''' Gets or set the UC mediator.
		''' </summary>
		Public Property UCMediator As UserControlFromMediator
			Get
				Return m_UCMediator
			End Get
			Set(value As UserControlFromMediator)
				m_UCMediator = value
			End Set
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

			m_EmployeeDataAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_applicationDataAccess = New DatabaseAccess.Applicant.AppDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			TranslateControls()

		End Sub

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overridable Function Activate(ByVal employeeNumber As Integer) As Boolean
			' Do not make this method abstract because the WinForms designer does not like that.
			Throw New NotImplementedException("The methods must be overriden by subclass.")
		End Function

    Public Overridable Function ActivateCVLWork(ByVal cvlProfileID As Integer?, ByVal cvlWorkID As Integer?) As Boolean
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
		''' Merges the employee master data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
		''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
		Public Overridable Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
			' Do not make this method abstract because the WinForms designer does not like that.
			Throw New NotImplementedException("The methods must be overriden by subclass.")
		End Sub

		''' <summary>
		'''  Merges the employee contact comm data.
		''' </summary>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		Public Overridable Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
			' Do nothing
		End Sub

		''' <summary>
		'''  Merges the employee contact other data (MASonstiges).
		''' </summary>
		''' <param name="employeeOtherData">The employee other data.</param>
		Public Overridable Sub MergeEmployeeOtherData(ByVal employeeOtherData As EmployeeOtherData)
			' Do nothing
		End Sub

		''' <summary>
		'''  Merges the employee LOSetting data data (MA_LOSetting).
		''' </summary>
		''' <param name="employeeLOSettings">The employee LOSetting data.</param>
		Public Overridable Sub MergeEmployeeLOSettingsData(ByVal employeeLOSettings As EmployeeLOSettingsData)
			' Do nothing
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

#Region "Private Methods"

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

#End Region

	End Class

End Namespace