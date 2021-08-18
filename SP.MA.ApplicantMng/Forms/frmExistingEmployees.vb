Imports System.Reflection.Assembly

Imports SP.TodoMng
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel
Imports SP.MA.ApplicantMng.Settings
Imports System.Windows.Forms

Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPS.Listing.Print.Utility
Imports System.Threading
Imports System.IO
Imports DevExpress.XtraNavBar
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary

'Imports SPS.MA.Guthaben.ShowMAGuthabenForm.ClsShowForm
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPSSendMail.RichEditSendMail
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Applicant

Namespace UI


	Public Class frmExistingEmployees



#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_AppDatabaseAccess As IAppDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' Contains the employee number of the loaded employee data.
		''' </summary>
		Private m_EmployeeNumber As Integer?

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_ApplicationUtilWebServiceUri As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' The top active tab page.
		''' </summary>
		Private m_TopActiveTabPage As ucBaseControl

		''' <summary>
		''' The bottom active tab page.
		''' </summary>
		Private m_BottomActiveTabPage As ucBaseControl

		''' <summary>
		''' List of tab controls.
		''' </summary>
		Private m_ListOfTabControls As New List(Of ucBaseControl)

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' Communication support between controls.
		''' </summary>
		Protected m_UCMediator As UserControlFromMediator

		''' <summary>
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_mandant As Mandant
		Private m_path As ClsProgPath



		Private ucDetail As ucPageEmployeeBasicData

#End Region


#Region "Public Properties"

		Public Property ExistingEmployeeData As IEnumerable(Of ExistingEmployeeSearchData)

#End Region



#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_mandant = New Mandant
			m_path = New ClsProgPath
			m_Common = New CommonSetting
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			m_SettingsManager = New SettingsManager

			WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
			WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False


			ucDetail = New ucPageEmployeeBasicData(m_InitializationData)

			pnlDetail.Controls.Add(ucDetail)
			ucDetail.Dock = DockStyle.Fill
			pnlDetail.Dock = DockStyle.Fill

			TranslateControls()


		End Sub

#End Region


#Region "Public methods"
		Public Function LoadData() As Boolean
			Dim success As Boolean = True

			ucDetail.ExistingEmployeeData = ExistingEmployeeData
			ucDetail.LoadData()

			Return success
		End Function


#End Region

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
			btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

		End Sub

		Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Me.Close()
		End Sub

		Private Sub btnSaveApplicationsToAssignedEmployee_Click(sender As Object, e As EventArgs) Handles btnSaveApplicationsToAssignedEmployee.Click
			Dim data = ucDetail.GetAssigendEmployeeData
			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden ausgewählt."))

				Return
			End If
			Dim existingEmployeeNumber As Integer = data.EmployeeNumber

			'UpdateExistingEmployeeWithApplicantData()

		End Sub


		'Private Function UpdateExistingEmployeeWithApplicantData(ByVal applicantData As ApplicantData, ByVal existingEmployeeNumber As Integer) As Boolean
		'	Dim success As Boolean = True

		'	Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(existingEmployeeNumber)
		'	If employeeData Is Nothing Then Return False

		'	employeeData.CVLProfileID = applicantData.CVLProfileID
		'	employeeData.ApplicantID = applicantData.ID

		'	m_Logger.LogWarning(String.Format("employee was allready exists! Now updating employeemasterdata: existingEmployeeNumber: {0} | CVLProfileID: {1} >>> ID: {2}",
		'																	existingEmployeeNumber, applicantData.CVLProfileID, applicantData.ID))
		'	success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(employeeData)

		'	' create new contact record
		'	Dim title As String = String.Empty
		'	Dim description As String = String.Empty
		'	Dim contactType As String = String.Empty

		'	m_Logger.LogWarning(String.Format("Now inserting application data: {0} >>> ID: {1}", applicantData.CVLProfileID, applicantData.ID))

		'	success = success AndAlso ImportApplicationJobData(applicantData.ID)
		'	success = success AndAlso ImportDocumentJobData(applicantData.ID)
		'	success = success AndAlso ImportCVLDocumentData(applicantData.CVLProfileID)

		'	applicantData.ApplicantNumber = existingEmployeeNumber

		'	Try
		'		title = String.Format("{0}", m_Translate.GetSafeTranslationValue("Bewerbung-Eingang"))
		'		contactType = "Einzelmail"
		'		description = title
		'		If success AndAlso existingEmployeeNumber > 0 Then AddNewEmployeeContact(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)

		'	Catch ex As Exception
		'		m_Logger.LogError(String.Format("contact could not be imsertetd! applicantData.ID: {0} >>> existingEmployeeNumber: {1} | {2}", applicantData.ID, existingEmployeeNumber, ex.ToString))
		'	End Try


		'	Return success

		'End Function

		'Private Sub AddNewEmployeeContact(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String)

		'	If m_CurrentApplicationNumber.GetValueOrDefault(0) = 0 Then Return
		'	Dim currentContactRecordNumber As Integer = 0
		'	Dim currentDocumentID As Integer = 0
		'	Dim contactData As EmployeeContactData = Nothing
		'	Dim fileContent = m_Utility.LoadFileBytes(attachedFile)
		'	Dim advisorFullname As String = "System"
		'	Dim extension As String = ".msg"

		'	If fileContent Is Nothing Then
		'		Dim mailData = PerformLoadAssignedApplicationEMaliWebservice(m_CurrentApplicationNumber.GetValueOrDefault(0))
		'		If Not mailData Is Nothing AndAlso Not mailData.EMailContent Is Nothing Then
		'			fileContent = mailData.EMailContent
		'			extension = ".eml"
		'		End If
		'	End If
		'	Dim dt = DateTime.Now
		'	contactData = New EmployeeContactData With {.EmployeeNumber = m_currentApplicantNumber,
		'																																 .CreatedOn = dt,
		'																																 .CreatedFrom = advisorFullname}

		'	contactData.EmployeeNumber = m_currentApplicantNumber
		'	contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
		'	contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
		'	contactData.ContactPeriodString = title
		'	contactData.ContactsString = description
		'	contactData.ContactImportant = False
		'	contactData.ContactFinished = False
		'	contactData.VacancyNumber = m_VacancyNumber
		'	contactData.ProposeNr = Nothing
		'	contactData.ESNr = Nothing
		'	contactData.CustomerNumber = Nothing

		'	contactData.ChangedFrom = advisorFullname
		'	contactData.ChangedOn = dt
		'	contactData.UsNr = 1

		'	Dim success As Boolean = True

		'	' Check if the document bytes must be saved.
		'	If Not (fileContent Is Nothing) And success Then

		'		Dim contactDocument As ContactDoc = Nothing

		'		contactDocument = New ContactDoc() With {.CreatedOn = dt,
		'																								 .CreatedFrom = advisorFullname,
		'																								 .FileBytes = fileContent,
		'																								 .FileExtension = extension}
		'		success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

		'		If success Then
		'			currentDocumentID = contactDocument.ID
		'			contactData.KontaktDocID = currentDocumentID
		'		End If

		'	End If

		'	' Insert contact
		'	success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

		'	If success Then
		'		currentContactRecordNumber = contactData.RecordNumber
		'	End If

		'	If Not success Then
		'		m_Logger.LogError("add new contact was not successfull!")
		'	End If

		'End Sub


	End Class


End Namespace