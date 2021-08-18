
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

Namespace UI

	''' <summary>
	''' Employee management.
	''' </summary>
	Public Class frmApplicant


#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


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

		Private Property m_PrintJobNr As String
		Private Property m_SQL4Print As String

		Private m_SaveButton As NavBarItem
		Private m_IsDataValid As Boolean = True

		''' <summary>
		''' Boolan flag indicating if the form has been initialized.
		''' </summary>
		Private m_IsInitialized = False

		'''' <summary>
		'''' WOS NavBar Item.
		'''' </summary>
		'Private m_Wos_P_Data As NavBarItem

		'Private m_PropertyForm As frmEmployeesProperties
		Private m_AllowedDesign As Boolean

		Private m_CustomerID As String
		Private m_WorkID As Integer?
		Private m_PersonalID As Integer?
		Private m_EducationID As Integer?
		Private m_AdditionalID As Integer?
		Private m_ObjectiveID As Integer?



#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				' Mandantendaten
				m_mandant = New Mandant
				m_path = New ClsProgPath
				m_Common = New CommonSetting

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_CustomerID = m_InitializationData.MDData.MDGuid

			' Top tabs
			m_ListOfTabControls.Add(ucCommonData)
			m_ListOfTabControls.Add(ucMediation)
			m_ListOfTabControls.Add(ucLanguagesAndProfessions)
			m_ListOfTabControls.Add(ucContactData)
			m_ListOfTabControls.Add(ucDocumentManagement)

			' Bottom tabs
			m_ListOfTabControls.Add(ucMonthlySalaryData)
			m_ListOfTabControls.Add(ucCVLWorkData)
			m_ListOfTabControls.Add(ucCVLEducationData)
			m_ListOfTabControls.Add(ucCVLAdditionalData)
			m_ListOfTabControls.Add(ucCVLPublicationData)
			xtabApplications.PageVisible = True
			xtabEducation.PageVisible = True
			xtabAddInfo.PageVisible = True

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfTabControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
			Next

			m_UCMediator = New UserControlFromMediator(Me, ucCommonData, Nothing, Nothing)
			m_TopActiveTabPage = ucCommonData
			m_BottomActiveTabPage = ucMonthlySalaryData

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 105, m_InitializationData.MDData.MDNr)
			'm_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)

			' Translate controls.
			TranslateControls()

			' Creates the navigation bar.
			CreateMyNavBar()

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Boolean flag indicating if employee data is loaded.
		''' </summary>
		Public ReadOnly Property IsEmployeeDataLoaded As Boolean
			Get
				Return m_EmployeeNumber.HasValue
			End Get

		End Property

		''' <summary>
		''' Gets the UC control mediator.
		''' </summary>
		''' <returns>The UC control mediator.</returns>
		Public ReadOnly Property UCMediator As UserControlFromMediator
			Get
				Return m_UCMediator
			End Get
		End Property

		''' <summary>
		''' Gets or sets data valid flag.
		''' </summary>
		''' <returns>Data valid flag</returns>
		Public Property IsDataValid As Boolean
			Get
				Return m_IsDataValid
			End Get
			Set(value As Boolean)

				m_IsDataValid = value

				If Not m_IsDataValid AndAlso Not m_SaveButton Is Nothing Then
					m_SaveButton.Enabled = False
				End If

			End Set
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Show the data of an employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

			If Not m_SaveButton Is Nothing Then
				m_SaveButton.Enabled = True
			End If

			If Not m_IsInitialized Then
				Reset()
				m_IsInitialized = True
			End If

			CleanUp()

			m_SuppressUIEvents = True

			Dim success As Boolean = True

			success = success AndAlso m_TopActiveTabPage.Activate(employeeNumber)

			success = success AndAlso PerformCVLProfileDataWebservice(ucCommonData.EmployeeCVLProfileID)
			success = success AndAlso m_BottomActiveTabPage.Activate(employeeNumber)
			success = success AndAlso PrepareStatusAndNavigationBar(employeeNumber)

			m_EmployeeNumber = IIf(success, employeeNumber, Nothing)

			IsDataValid = success

			m_SuppressUIEvents = False

			Return success
		End Function

#End Region


#Region "Private Properties"

		''' <summary>
		''' get datamatrix printername from userprofile
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetUserDataMaxtrixPrintername() As String
			Get
				Dim sp_utility As New SPProgUtility.MainUtilities.Utilities

				Dim strQuery As String = "//Report/matrixprintername"
				Dim strStyleName As String = sp_utility.GetXMLValueByQueryWithFilename(m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr), strQuery, String.Empty)

				Return strStyleName

			End Get
		End Property

		''' <summary>
		''' get datamatrix code from mandant
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetDataMaxtrixCodeString() As String
			Get
				Dim strQuery As String = String.Format("{0}/datamatrixcodestringforemployeelabel", FORM_XML_MAIN_KEY)
				Dim dataMatrixCode As String = m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), strQuery)
				If String.IsNullOrWhiteSpace(dataMatrixCode) Then dataMatrixCode = "MA_{0}_999"

				Return dataMatrixCode

			End Get
		End Property

#End Region


#Region "Private Methods"

		Private ReadOnly Property GetHwnd() As String
			Get
				Return CStr(Me.Handle)
			End Get
		End Property


		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
			Me.xtabVermittlung.Text = m_Translate.GetSafeTranslationValue(Me.xtabVermittlung.Text)
			Me.xtabSprachenUndBerufe.Text = m_Translate.GetSafeTranslationValue(Me.xtabSprachenUndBerufe.Text)
			Me.xtabKontakte.Text = m_Translate.GetSafeTranslationValue(Me.xtabKontakte.Text)
			Me.xtabDokumente.Text = m_Translate.GetSafeTranslationValue(Me.xtabDokumente.Text)

			Me.xtabApplications.Text = m_Translate.GetSafeTranslationValue(Me.xtabApplications.Text)
			Me.xtabEducation.Text = m_Translate.GetSafeTranslationValue(Me.xtabEducation.Text)
			Me.xtabAddInfo.Text = m_Translate.GetSafeTranslationValue(Me.xtabAddInfo.Text)
			Me.xtabWork.Text = m_Translate.GetSafeTranslationValue(Me.xtabWork.Text)

		End Sub

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			Dim supressState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			If (Not employeePicture.Image Is Nothing) Then
				employeePicture.Image.Dispose()
			End If

			employeePicture.Image = Nothing
			employeePicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")
			employeePicture.Properties.ShowMenu = False
			employeePicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

			' Reset all the child controls
			For Each tabControl In m_ListOfTabControls
				tabControl.Reset()
			Next

			' Top page
			m_TopActiveTabPage.Deactivate()
			m_TopActiveTabPage = ucCommonData
			XtraTabControl1.SelectedTabPage = xtabAllgemein

			' Bottom page
			m_BottomActiveTabPage.Deactivate()
			m_BottomActiveTabPage = ucMonthlySalaryData
			xtabMoreinfo.SelectedTabPage = xtabApplications

			m_SuppressUIEvents = False

			m_EmployeeNumber = Nothing

			m_SuppressUIEvents = supressState
		End Sub

		''' <summary>
		''' CleanUp the form
		''' </summary>
		Private Sub CleanUp()

			' Cleanup all the child controls
			For Each tabControl In m_ListOfTabControls
				tabControl.CleanUp()
			Next

		End Sub

		''' <summary>
		''' Saves employee data.
		''' </summary>
		Public Function SaveEmployeeData(ByVal showMsg As Boolean) As Boolean
			Dim success As Boolean = True

			If (IsEmployeeDataLoaded) Then

				' 1. Load the master, other (sonstiges) and contactComm data
				Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber)
				Dim employeeOtherData As EmployeeOtherData = m_EmployeeDatabaseAccess.LoadEmployeeOtherData(m_EmployeeNumber)
				Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_EmployeeNumber)
				Dim employeeLOSettingData As EmployeeLOSettingsData = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(m_EmployeeNumber)

				If employeeMasterData Is Nothing Or
					employeeOtherData Is Nothing Or
					employeeContactCommData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return False
				End If

				' 2. Ask all tabs to merge its data with the records just loaded.
				For Each tabControl In m_ListOfTabControls
					tabControl.MergeEmployeeMasterData(employeeMasterData)
					tabControl.MergeEmployeeOtherData(employeeOtherData)
					tabControl.MergeEmployeeContactCommData(employeeContactCommData)
				Next

				' 3. Update the data in the database
				employeeMasterData.ValidatePermissionWithTax = True
				employeeMasterData.ChangedOn = DateTime.Now
				employeeMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName

				employeeLOSettingData.PayrollSendAsZip = True


				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(employeeMasterData)
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeOtherData(employeeOtherData)
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeConactCommData(employeeContactCommData)
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeLOSettings(employeeLOSettingData)

				Dim message As String = String.Empty

				If (success) Then
					success = success AndAlso PerformUpdateAssignedRemoteApplicantWebservice(m_EmployeeNumber)

					message = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
					bsiChanged.Caption = String.Format(" {0:f}, {1}", employeeMasterData.ChangedOn, employeeMasterData.ChangedFrom)
					If showMsg Then
						m_UtilityUI.ShowOKDialog(message, m_Translate.GetSafeTranslationValue("Daten speichern"))
					End If

				Else
					message = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
					m_UtilityUI.ShowErrorDialog(message)
					success = False

				End If

			Else
				Dim Message = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
				m_UtilityUI.ShowErrorDialog(Message)

				success = False

			End If

			Return success

		End Function

		''' <summary>
		''' Saves employee data.
		''' </summary>
		Public Sub ChangeApplicantToEmployee()

			If (IsEmployeeDataLoaded AndAlso ValidateData()) Then
				If Not SaveEmployeeData(False) Then Return

				' 1. Load the master, other (sonstiges) and contactComm data
				Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber)
				If employeeMasterData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

				Dim existEmployees = ExistAssignedEmployeeData(employeeMasterData)

				If Not existEmployees Is Nothing Then

					If existEmployees.Count > 2 Then
						LoadAllFoundedEmployees(existEmployees)

						Return
					End If

					Dim msgMandant As String = String.Empty
					Dim msg As String
					Dim result As DialogResult = DialogResult.Cancel

					Dim assignedApplicant = existEmployees.Where(Function(x) x.EmployeeNumber <> employeeMasterData.EmployeeNumber).FirstOrDefault

					If Not assignedApplicant Is Nothing Then

						If assignedApplicant.MDNr <> employeeMasterData.MDNr Then
							m_Logger.LogWarning(String.Format("employee eists allready, but diffrent MD! existEmployee.EmployeeNumber: {0} >>> employeeMasterData.EmployeeNumber: {1}", assignedApplicant.EmployeeNumber, employeeMasterData.EmployeeNumber))

							msgMandant = "Achtung: Gleicher Datensatz aber unterschiedliche Mandanten!{0}{0}"
							msgMandant = m_Translate.GetSafeTranslationValue(msgMandant)

						Else
							m_Logger.LogWarning(String.Format("employee eists allready! existEmployee.EmployeeNumber: {0} >>> employeeMasterData.EmployeeNumber: {1}", assignedApplicant.EmployeeNumber, employeeMasterData.EmployeeNumber))
							msg = String.Empty

						End If

						msg = "Möglicherweise existiert bereits einen ähnlichen Datensatz mit den Angaben.{0}"
						msg &= "Es wurde nach Nach- /Vorname, EMail-Adresse und Geburtstag gesucht.{0}"
						msg &= "Möchten Sie den Datensatz als neuen Kandidaten übernehmen?{0}{0}"
						msg &= "Bestehende Datensatznummer: {1}{0}{0}"
						msg &= "Ja = Neuen Daten anlegen{0}"
						msg &= "Nein = Bewerbungen zum bestehende Daensatz zuweisen. (Dabei wird der geöffnete Datensatz gelöscht!){0}"
						msg &= "Abbrechen = Vorgang abbrechen"
						msg = m_Translate.GetSafeTranslationValue(msg)
						msg = msgMandant & msg

						msg = String.Format(msg, vbNewLine, assignedApplicant.EmployeeNumber)
						result = DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Duplikat gefunden", MessageBoxButtons.YesNoCancel)

						If result = DialogResult.No Then
							Dim updateSuccess As Boolean = UpdateExistingEmployeeWithApplicantData(employeeMasterData, assignedApplicant)
							If Not updateSuccess Then
								m_UtilityUI.ShowErrorDialog("Die Daten konnten nicht gespeichert werden. Bitte probieren Sie den Vorgang erneut.")

								Return
							Else
								m_UtilityUI.ShowInfoDialog("Der Vorgang wurde erfolgreich ausgeführt. Nun werden die Daten gelöscht.")
								If DeleteSelectedEmployee(False) = DeleteEmployeeResult.Deleted Then Me.CleanupAndHideForm()

								Return
							End If

						ElseIf result = DialogResult.Cancel Then
							Return
						End If

					End If
				End If

				employeeMasterData.ShowAsApplicant = Not employeeMasterData.ShowAsApplicant.GetValueOrDefault(False)
				employeeMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName

				Dim success = m_EmployeeDatabaseAccess.UpdateApplicantToEmployee(employeeMasterData)

				Dim message As String = String.Empty

				If (success) Then
					Dim title As String = m_Translate.GetSafeTranslationValue("Bewerber zum Kandidat")
					Dim description As String = m_Translate.GetSafeTranslationValue("Bewerber wird zum Kandidat")
					Dim contactType As String = "Information"

					success = success AndAlso SaveEmployeeContactData(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)

					message = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
					bsiChanged.Caption = String.Format(" {0:f}, {1}", employeeMasterData.ChangedOn, employeeMasterData.ChangedFrom)

					m_UtilityUI.ShowOKDialog(message, m_Translate.GetSafeTranslationValue("Bewerber als Kandidat speichern"))
				Else
					message = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
					m_UtilityUI.ShowErrorDialog(message)

				End If

				If success Then
					Dim hub = MessageService.Instance.Hub
					Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_EmployeeNumber)
					hub.Publish(openEmployeeMng)

					Me.Close()
				End If

			Else
				Dim Message = String.Format(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.{0}Möglicherweise sind pflichtfelder nicht ausgefüllt!"), vbNewLine)
				m_UtilityUI.ShowErrorDialog(Message)

			End If


		End Sub

		Private Function ExistAssignedEmployeeData(ByVal applicantData As EmployeeMasterData) As IEnumerable(Of ExistingEmployeeSearchData)
			Dim result = New List(Of ExistingEmployeeSearchData)
			Dim xmlFile As String = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)

			Dim searchforchangingapplicanttoemployee As String = m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/searchforchangingapplicanttoemployee", FORM_XML_MAIN_KEY))
			Dim lastname As String = String.Empty
			Dim firstname As String = String.Empty
			Dim email As String = String.Empty
			Dim birthdate As Date? = Nothing

			lastname = applicantData.Lastname
			firstname = applicantData.Firstname
			email = applicantData.Email
			birthdate = applicantData.Birthdate

			If Not String.IsNullOrWhiteSpace(searchforchangingapplicanttoemployee) Then
				Dim value As List(Of String) = searchforchangingapplicanttoemployee.Split(New Char() {";"c, ","c, "#"c, "|"c}).ToList

				If value.Count > 0 Then
					Dim searchforLastname As String = value.Where(Function(x) x = "lastname").FirstOrDefault
					Dim searchforFirstname As String = value.Where(Function(x) x = "firstname").FirstOrDefault
					Dim searchforMmail As String = value.Where(Function(x) x = "email").FirstOrDefault
					Dim searchforBirthdate As String = value.Where(Function(x) x = "birthdate").FirstOrDefault

					If Not String.IsNullOrWhiteSpace(searchforLastname) Then lastname = applicantData.Lastname
					If Not String.IsNullOrWhiteSpace(searchforFirstname) Then firstname = applicantData.Firstname
					If Not String.IsNullOrWhiteSpace(searchforMmail) Then email = applicantData.Email Else email = String.Empty
					If Not String.IsNullOrWhiteSpace(searchforBirthdate) Then birthdate = applicantData.Birthdate Else birthdate = Nothing

				End If

			End If

			Dim querySetting = New ExistingEmployeeSearchData With {.Lastname = lastname, .Firstname = firstname, .Email = email, .Birthdate = birthdate}
			Dim existingEmployeeData As IEnumerable(Of ExistingEmployeeSearchData) = m_EmployeeDatabaseAccess.LoadAssignedEmployeesBySearchCriteria(querySetting)

			result = existingEmployeeData


			Return result

		End Function

		Private Function LoadAllFoundedEmployees(ByVal data As IEnumerable(Of ExistingEmployeeSearchData)) As Boolean
			Dim result As Boolean = True
			Dim frm As New frmExistingEmployees(m_InitializationData)

			frm.ExistingEmployeeData = data
			frm.LoadData()

			frm.Show()
			frm.BringToFront()


			Return result
		End Function

		Private Function UpdateExistingEmployeeWithApplicantData(ByVal applicantData As EmployeeMasterData, ByVal existEmployee As ExistingEmployeeSearchData) As Boolean
			Dim success As Boolean = True

			' load existing employee which is not open now!
			Dim existingData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(existEmployee.EmployeeNumber, False)
			If existingData Is Nothing Then Return False

			existingData.ApplicantID = applicantData.ID
			existingData.CVLProfileID = applicantData.CVLProfileID

			success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(existingData)
			success = success AndAlso m_EmployeeDatabaseAccess.ChangeEployeeDataWithApplicantData(existingData.EmployeeNumber, applicantData.EmployeeNumber)
			success = success AndAlso PerformUpdateAssignedApplicantJobWebservice(applicantData.EmployeeNumber, existingData.EmployeeNumber)


			Return success

		End Function

		Private Function PerformUpdateAssignedApplicantJobWebservice(ByVal oldApplicantNumber As Integer, ByVal newApplicantNumber As Integer?) As Boolean

			Dim success As Boolean = True

#If DEBUG Then
			'Return True
			'm_ApplicationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPNotification.asmx"
#End If

			Dim webservice As New Internal.Automations.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			success = success AndAlso webservice.UpdateAssignedApplicantWithExistingEmployeeData(m_InitializationData.MDData.MDGuid, oldApplicantNumber, newApplicantNumber)


			Return success

		End Function

		Private Function PerformUpdateAssignedRemoteApplicantWebservice(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True

#If DEBUG Then
			'Return True
			'm_ApplicationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPNotification.asmx"
#End If

			Dim existingData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
			If existingData Is Nothing Then Return False

			Dim webservice As New Internal.Automations.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Dim applicantData = New Internal.Automations.SPApplicationWebService.ApplicantDataDTO()
			applicantData.APID = existingData.ApplicantID
			applicantData.Birthdate = existingData.Birthdate
			applicantData.Country = existingData.Country
			If existingData.CivilStatus Is Nothing Then
				applicantData.CivilStateLabel = String.Empty
			Else
				applicantData.CivilStateLabel = existingData.CivilStatus
			End If
			applicantData.CVLProfileID = existingData.CVLProfileID
			applicantData.EMail = existingData.Email
			applicantData.Firstname = existingData.Firstname
			If String.IsNullOrWhiteSpace(existingData.Gender) Then
				applicantData.Gender = String.Empty
			ElseIf existingData.Gender = "W" Then
				applicantData.Gender = "f"
			Else
				applicantData.Gender = "m"
			End If
			applicantData.Lastname = existingData.Lastname
			applicantData.Latitude = existingData.Latitude
			applicantData.Location = existingData.Location
			applicantData.Longitude = existingData.Longitude
			If existingData.MA_Canton Is Nothing Then
				applicantData.Canton = m_InitializationData.MDData.MDCanton
			Else
				applicantData.Canton = existingData.MA_Canton
			End If
			applicantData.Nationality = existingData.Nationality
			applicantData.Postcode = existingData.Postcode
			applicantData.Street = existingData.Street


			' Read data over webservice
			success = success AndAlso webservice.UpdateAssignedApplicantCVLDataWithEmployeeData(m_InitializationData.MDData.MDGuid, employeeNumber, existingData.ApplicantID, applicantData)
			If Not success Then
				m_Logger.LogWarning(String.Format("record could not be saved on remote table: employeeNumber: {0} | ApplicantID: {1} >>> CVLProfileID: {2}", employeeNumber, existingData.ApplicantID, existingData.CVLProfileID))
			End If

			Return success

		End Function

		''' <summary>
		''' Validates the data on the tabs.
		''' </summary>
		Private Function ValidateData() As Boolean

			Dim valid As Boolean = True
			For Each tabControl In m_ListOfTabControls

				' Only validate tabs with the correct employee number.
				If tabControl.EmployeeNumber = m_EmployeeNumber Then
					valid = valid AndAlso tabControl.ValidateData()
				Else
					' Skip
				End If

			Next

			Return valid

		End Function

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmEmployees_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmEmployees_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnFrmResponsiblePerson_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

			CleanupAndHideForm()

			e.Cancel = True

		End Sub


		''' <summary>
		''' Handles tab control selectioin changing
		''' </summary>
		Private Sub OnxtraTabControl_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles XtraTabControl1.SelectedPageChanging

			If m_SuppressUIEvents Then
				Return
			End If

			If (IsEmployeeDataLoaded) Then

				Dim page = e.Page

				If Not (m_TopActiveTabPage Is Nothing) Then
					m_TopActiveTabPage.Deactivate()
				End If

				If (Object.ReferenceEquals(page, xtabAllgemein)) Then
					IsDataValid = ucCommonData.Activate(m_EmployeeNumber)
					m_TopActiveTabPage = ucCommonData
				ElseIf (Object.ReferenceEquals(page, xtabVermittlung)) Then
					IsDataValid = ucMediation.Activate(m_EmployeeNumber)
					m_TopActiveTabPage = ucMediation
				ElseIf (Object.ReferenceEquals(page, xtabSprachenUndBerufe)) Then
					IsDataValid = ucLanguagesAndProfessions.Activate(m_EmployeeNumber)
					m_TopActiveTabPage = ucLanguagesAndProfessions
				ElseIf (Object.ReferenceEquals(page, xtabKontakte)) Then
					IsDataValid = ucContactData.Activate(m_EmployeeNumber)
					m_TopActiveTabPage = ucContactData
				ElseIf (Object.ReferenceEquals(page, xtabDokumente)) Then
					IsDataValid = ucDocumentManagement.Activate(m_EmployeeNumber)
					m_TopActiveTabPage = ucDocumentManagement
				End If

			End If

		End Sub

		''' <summary>
		''' Handles tab control selectioin changing
		''' </summary>
		Private Sub OnxTabMoreInfo_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles xtabMoreinfo.SelectedPageChanging

			If m_SuppressUIEvents Then
				Return
			End If

			If (IsEmployeeDataLoaded) Then

				Dim page = e.Page

				If Not (m_BottomActiveTabPage Is Nothing) Then
					m_BottomActiveTabPage.Deactivate()
				End If

				If (Object.ReferenceEquals(page, xtabApplications)) Then
					IsDataValid = ucMonthlySalaryData.Activate(m_EmployeeNumber)
					m_BottomActiveTabPage = ucMonthlySalaryData

				ElseIf (Object.ReferenceEquals(page, xtabWork)) Then
					IsDataValid = ucCVLWorkData.ActivateCVLWork(ucCommonData.EmployeeCVLProfileID, m_WorkID)
					m_BottomActiveTabPage = ucCVLWorkData
				ElseIf (Object.ReferenceEquals(page, xtabEducation)) Then
					IsDataValid = ucCVLEducationData.ActivateCVLWork(ucCommonData.EmployeeCVLProfileID, m_EducationID)
					m_BottomActiveTabPage = ucCVLEducationData

				ElseIf (Object.ReferenceEquals(page, xtabAddInfo)) Then
					IsDataValid = ucCVLAdditionalData.ActivateCVLWork(ucCommonData.EmployeeCVLProfileID, m_AdditionalID)
					m_BottomActiveTabPage = ucCVLAdditionalData
				ElseIf (Object.ReferenceEquals(page, xtabPublication)) Then
					IsDataValid = ucCVLPublicationData.ActivateCVLWork(ucCommonData.EmployeeCVLProfileID, Nothing)
					m_BottomActiveTabPage = ucCVLPublicationData


				End If

			End If

		End Sub

		''' <summary>
		''' Keypreview for Modul-version
		''' </summary>
		Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
			If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
				Dim strRAssembly As String = ""
				Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
				For Each a In AppDomain.CurrentDomain.GetAssemblies()
					strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
				Next
				strMsg = String.Format(strMsg, vbNewLine,
															 GetExecutingAssembly().FullName,
															 GetExecutingAssembly().Location,
															 strRAssembly)
				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If
		End Sub

		Private Sub employeePicture_Click(sender As Object, e As System.EventArgs) Handles employeePicture.Click
			If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 121, m_InitializationData.MDData.MDNr) Then
				OpenFormforChangeEmployeePhoto()
			End If
		End Sub

		''' <summary>
		''' Clickevent for Navbar.
		''' </summary>
		Private Sub OnnbMain_LinkClicked(ByVal sender As Object,
																 ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim bForDesign As Boolean = False
			Try
				Dim strLinkName As String = e.Link.ItemName
				Dim strLinkCaption As String = e.Link.Caption

				For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
					e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
				Next
				e.Link.Item.Appearance.ForeColor = Color.Orange

				Select Case strLinkName.ToLower
					'Case "New_Employee".ToLower
					'	ShowNewEmployeeFrom()
					Case "Save_Employee_Data".ToLower
						SaveEmployeeData(True)
					Case "Print_Employee_Data".ToLower
						GetMenuItems4Print()
					Case "Close_Employee_Form".ToLower
						Me.CleanupAndHideForm()
					Case "delete_Employee_Data".ToLower
						If DeleteSelectedEmployee(True) = DeleteEmployeeResult.Deleted Then Me.CleanupAndHideForm()

					Case "CreateTODO".ToLower
						ShowTodo()

					Case "ChangeToEmployee".ToLower()
						ChangeApplicantToEmployee()
					Case "OpenOKEMailNotification".ToLower()
						ShowOKEMailNotificationData()
					Case "OpenCancelEMailNotification".ToLower()
						ShowCancelEMailNotificationData()


					' TODO: contextmenu with application data!!!
					Case "ShowInterviews".ToLower()
						ShowJobInterviews()
					Case "ShowInterviews_".ToLower()
						ShowJobInterviews()



					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(ex.Message)

			Finally

			End Try

		End Sub

		''' <summary>
		'''  Performs loading cvl work data.
		''' </summary>
		Private Function PerformCVLProfileDataWebservice(ByVal profileID As Integer?) As Boolean

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim ws = New SPApplicationWebService.SPApplicationSoapClient
			ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			'Dim searchResult = ws.LoadAssignedCVLProfileViewData(m_CustomerID, profileID)
			Dim searchResult = ws.LoadAssignedCVLProfileViewData(String.Empty, profileID)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("profileData: could not be loaded from webservice! {0} | {1}", m_CustomerID, profileID))

				Return False
			End If

			m_WorkID = searchResult.WorkID
			m_PersonalID = searchResult.PersonalID
			m_EducationID = searchResult.EducationID
			m_AdditionalID = searchResult.AdditionalID
			m_ObjectiveID = searchResult.ObjectiveID

			If m_WorkID Is Nothing Then
				xtabWork.Enabled = False
				xtabEducation.Enabled = False
				xtabAddInfo.Enabled = False
				xtabPublication.Enabled = False
			End If

			Return Not (searchResult Is Nothing)

		End Function

		''' <summary>
		''' Shows a todo From.
		''' </summary>
		Private Sub ShowTodo()
			Dim frmTodo As New frmTodo(m_InitializationData)
			' optional init new todo
			Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
			Dim EmployeeNumber As Integer? = m_EmployeeNumber
			Dim CustomerNumber As Integer? = Nothing
			Dim ResponsiblePersonRecordNumber As Integer? = Nothing
			Dim VacancyNumber As Integer? = Nothing
			Dim ProposeNumber As Integer? = Nothing
			Dim ESNumber As Integer? = Nothing
			Dim RPNumber As Integer? = Nothing
			Dim LMNumber As Integer? = Nothing
			Dim RENumber As Integer? = Nothing
			Dim ZENumber As Integer? = Nothing
			Dim Subject As String = String.Empty
			Dim Body As String = ""

			frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
													VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

			frmTodo.Show()

		End Sub

		''' <summary>
		''' shows notification mail-data for employee
		''' </summary>
		Private Sub ShowOKEMailNotificationData()
			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Dim frmMail = New frmMailTpl(m_InitializationData)
			Try
				Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.ApplicantOKNotification, .EmployeeNumber = m_EmployeeNumber}

				frmMail.PreselectionData = preselectionSetting
				frmMail.LoadData()

				frmMail.Show()
				frmMail.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(String.Format("Mitteilungsdaten konnten nicht geladen werden.{0}{1}", vbNewLine, ex.ToString()))
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Mitteilungsdaten konnten nicht geladen werden.")), "OK-Notification")

			End Try

		End Sub

		''' <summary>
		''' shows notification mail-data for employee
		''' </summary>
		Private Sub ShowCancelEMailNotificationData()
			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If
			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)

			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerber-Stammdaten (Statuszeile/Navigation) konnten nicht geladen werden."))
				Return
			End If

			Dim applicationListData = m_EmployeeDatabaseAccess.LoadAssignedEmployeeApplications(m_InitializationData.MDData.MDGuid, m_EmployeeNumber, 0, False)
			If applicationListData Is Nothing OrElse applicationListData.Count = 0 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Bewerbungen."))
				Return
			End If

			If employeeMasterData.ApplicantLifecycle <> ApplicationLifecycelEnum.APPLICATIONREJECTED Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Achtung: Hiermit sagen Sie den Bewerber ab. So werden alle offenen Bewerbungen als 'Abgesagt' abgeschlossen! Sind Sie sicher?")
				Dim result = m_UtilityUI.ShowYesNoDialog(msg, "Bewerber absagen")
				If Not result Then Return

				employeeMasterData.ApplicantLifecycle = ApplicationLifecycelEnum.APPLICATIONREJECTED
				result = result AndAlso m_EmployeeDatabaseAccess.UpdateApplicantFlagData(employeeMasterData)
				If Not result Then Return

			End If

			Dim frmMail = New frmMailTpl(m_InitializationData)
			Try
				Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.ApplicantCancelNotification, .EmployeeNumber = m_EmployeeNumber, .ApplicationNumber = applicationListData(0).ID}

				frmMail.PreselectionData = preselectionSetting
				frmMail.LoadData()

				frmMail.Show()
				frmMail.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(String.Format("Mitteilungsdaten konnten nicht geladen werden.{0}{1}", vbNewLine, ex.ToString()))
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Mitteilungsdaten konnten nicht geladen werden.")), "Cancel-Notification")

			End Try

		End Sub

		''' <summary>
		''' Shows job interviews.
		''' </summary>
		Private Sub ShowJobInterviews()

			If IsEmployeeDataLoaded Then
				Dim frmInterview As New MA.VorstellungMng.frmJobInterview(m_InitializationData)
				frmInterview.Show()
				frmInterview.LoadJobInterviewData(m_EmployeeNumber)
			End If

		End Sub

		''' <summary>
		''' Creates Navigationbar
		''' </summary>
		Private Sub CreateMyNavBar()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Me.navMain.Items.Clear()
			Dim labelPrinterName = GetUserDataMaxtrixPrintername
			bbiDatamatrix.Caption = String.Format("DataMatrix-Code: {0}", labelPrinterName)
			bbiDatamatrix.Enabled = Not String.IsNullOrWhiteSpace(labelPrinterName)
			Try
				navMain.PaintStyleName = "SkinExplorerBarView"

				' Create a Local group.
				Dim groupDatei As NavBarGroup = New NavBarGroup(("Datei"))
				groupDatei.Name = "gNavDatei"

				m_SaveButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
				m_SaveButton.Name = "Save_Employee_Data"
				m_SaveButton.SmallImage = Me.ImageCollection1.Images(1)
				m_SaveButton.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 102, m_InitializationData.MDData.MDNr) AndAlso IsDataValid

				Dim Print_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
				Print_P_Data.Name = "Print_Employee_Data"
				Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)
				Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 104, m_InitializationData.MDData.MDNr)

				Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
				Close_P_Data.Name = "Close_Employee_Form"
				Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

				Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
				groupDelete.Name = "gNavDelete"
				groupDelete.Appearance.ForeColor = Color.Red

				Dim Delete_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
				Delete_P_Data.Name = "Delete_Employee_Data"
				Delete_P_Data.SmallImage = Me.ImageCollection1.Images(4)
				Delete_P_Data.Appearance.ForeColor = Color.Red
				Delete_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 103, m_InitializationData.MDData.MDNr)

				Dim groupExtra As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
				groupExtra.Name = "gNavExtra"

				Dim TODO_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("To-do erstellen"))
				TODO_P_Data.Name = "CreateTODO"
				TODO_P_Data.SmallImage = Me.ImageCollection1.Images(9)

				Dim groupMoreModule As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras Verwaltung"))
				groupMoreModule.Name = "gNavMoreModule"

				Dim ChangeToEmployee_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Als Kandidat definieren"))
				ChangeToEmployee_Data.Name = "changeToEmployee"
				ChangeToEmployee_Data.SmallImage = Me.ImageCollection1.Images(11)

				Dim Inverview_Data_ As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Bewerber absagen"))
				Inverview_Data_.Name = "OpenCancelEMailNotification"
				Inverview_Data_.SmallImage = Me.ImageCollection1.Images(11)

				Try
					navMain.BeginUpdate()

					navMain.Groups.Add(groupDatei)
					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 102, m_InitializationData.MDData.MDNr) Then
						groupDatei.ItemLinks.Add(m_SaveButton)
					End If
					groupDatei.ItemLinks.Add(Print_P_Data)
					groupDatei.ItemLinks.Add(Close_P_Data)

					groupDatei.Expanded = True

					navMain.Groups.Add(groupExtra)
					groupExtra.ItemLinks.Add(TODO_P_Data)

					navMain.Groups.Add(groupMoreModule)

					groupMoreModule.ItemLinks.Add(ChangeToEmployee_Data)
					groupMoreModule.ItemLinks.Add(Inverview_Data_)

					groupExtra.Expanded = True
					groupMoreModule.Expanded = True

					navMain.EndUpdate()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message),
																									 "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

		End Sub

		''' <summary>
		''' Prepares status and navigation bar.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success</returns>
		Private Function PrepareStatusAndNavigationBar(ByVal employeeNumber As Integer)
			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, True)

			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerber-Stammdaten (Statuszeile/Navigation) konnten nicht geladen werden."))
				Return False
			End If

			bsiCreated.Caption = String.Format(" {0:f}, {1}", employeeMasterData.CreatedOn, employeeMasterData.CreatedFrom)
			bsiChanged.Caption = String.Format(" {0:f}, {1}", employeeMasterData.ChangedOn, employeeMasterData.ChangedFrom)

			'm_Wos_P_Data.Enabled = employeeMasterData.WOSGuid <> String.Empty

			Try
				If Not employeeMasterData.MABild Is Nothing AndAlso employeeMasterData.MABild.Count > 0 Then
					Dim memoryStream As New System.IO.MemoryStream(employeeMasterData.MABild)
					employeePicture.Image = Image.FromStream(memoryStream)
				Else
					employeePicture.Image = My.Resources.o14_person_placeholder_96
				End If
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

			Return True
		End Function

		Private Sub OnbbiDataMatrix_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDatamatrix.ItemClick
			PrintEmployeeDataMatrixCode()
		End Sub

		''' <summary>
		''' Build contextmenu for print.
		''' </summary>
		Private Sub GetMenuItems4Print()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim mnuData = m_EmployeeDatabaseAccess.LoadContextMenu4PrintData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))
				Exit Sub
			End If

			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu.Manager = Me.BarManager1
			popupMenu.Manager.Images = Me.ImageCollection1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem
				Dim strMnuBez As String = m_Translate.GetSafeTranslationValue(mnuData(i).MnuCaption)

				itm.Caption = strMnuBez.Replace("-", "")
				itm.Name = mnuData(i).MnuName
				Dim bAsGroup As Boolean = strMnuBez.StartsWith("-")

				If bAsGroup Then
					popupMenu.AddItem(itm).BeginGroup = True
				Else
					popupMenu.AddItem(itm)
				End If

				If itm.Name = "1.0" Then
					AddHandler itm.ItemClick, AddressOf PrintDocs
				ElseIf itm.Name = "1.0.1" Then
					AddHandler itm.ItemClick, AddressOf PrintDocs

				End If

			Next

			' fill templates
			Dim mnuTemplatesData = m_EmployeeDatabaseAccess.LoadContextMenu4PrintTemplatesData
			If Not (mnuTemplatesData Is Nothing) Then
				For i As Integer = 0 To mnuTemplatesData.Count - 1
					itm = New DevExpress.XtraBars.BarButtonItem

					itm.Caption = m_Translate.GetSafeTranslationValue(mnuTemplatesData(i).MnuCaption)
					itm.Name = String.Format("{0}|{1}", mnuTemplatesData(i).MnuDocPath, mnuTemplatesData(i).MnuDocMacro)

					If i = 0 Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf PrintDocs
				Next
			End If

			' show contextmenu
			popupMenu.ShowPopup(New Point(Me.navMain.Width + Me.Left, Cursor.Position.Y))

		End Sub

		''' <summary>
		''' Prints documents.
		''' </summary>
		Private Sub PrintDocs(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim strMenuName As String() = e.Item.Name.Split("|")
			If strMenuName.Length = 2 Then
				' then office-templates...
				If Not strMenuName(0).Substring(1, 2) = ":\" And Not strMenuName(0).StartsWith("\\") Then
					strMenuName(0) = String.Format("{0}{1}", m_mandant.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr), strMenuName(0))
				End If
				If File.Exists(strMenuName(0)) Then
					Dim fi As New FileInfo(strMenuName(0))
					Dim newFilename As String = String.Format("{0}{1}", m_path.GetSpS2DeleteHomeFolder, fi.Name)
					Try
						File.Copy(strMenuName(0), newFilename, True)
					Catch ex As Exception
						m_Logger.LogError(String.Format(m_Translate.GetSafeTranslationValue("{0}.Datei konnte nicht kopiert werden. {1}"), strMethodeName, ex.ToString))
						newFilename = strMenuName(0)
					End Try
					Try
						Dim _clsBrowser As New ClassBrowserPath
						_clsBrowser.GetBrowserApplicationPath(strMenuName(0))
						Dim startInfo As New ProcessStartInfo

						Dim _reg As New SPProgUtility.ClsDivReg
						_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MANr", m_EmployeeNumber)
						_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MandantNumber", m_InitializationData.MDData.MDNr)

						startInfo.FileName = _clsBrowser.GetBrowserPath
						startInfo.Arguments = Chr(34) & newFilename & Chr(34) & If(strMenuName(1) <> String.Empty, " /m" & strMenuName(1), "")
						startInfo.UseShellExecute = False
						Process.Start(startInfo)

					Catch ex As Exception
						m_Logger.LogError(String.Format(m_Translate.GetSafeTranslationValue("{0}.Datei konnte nicht geöffnet werden. {1}"), strMethodeName, ex.Message))
						m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Datei konnte nicht geöffnet werden. {0}"), ex.Message))
					End Try
				End If

			Else
				Me.m_PrintJobNr = e.Item.Name
				PrintEmployeeTemplate()

			End If

		End Sub

		''' <summary>
		'''  Starts printdialog with List Label 18.
		''' </summary>
		Private Sub PrintEmployeeTemplate()

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)

			Dim strResult As String = "Success..."
			Dim _PrintSetting As New ClsLLMASearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
				.SelectedMDNr = employeeMasterData.MDNr,
				.SQL2Open = String.Empty,
				.JobNr2Print = Me.m_PrintJobNr,
				.ShowAsDesign = ShowDesign,
				.frmhwnd = GetHwnd,
				.liMANr2Print = New List(Of Integer)(New Integer() {Me.m_EmployeeNumber})}

			If m_PrintJobNr = "1.0.1" Then
				Dim _TplSetting As New ClsLLMATemplateSetting With {.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber})}
				Dim obj As New MATemplates.ClsPrintMATemplates(m_InitializationData)
				'strResult = obj.PrintMATemplatePDU1PDF(_TplSetting)

			Else
				Dim obj As New MAStammblatt.ClsPrintMAStammblatt(_PrintSetting)
				strResult = obj.PrintMAStammBlatt()

			End If


		End Sub

		Private Sub PrintEmployeeDataMatrixCode()

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If
			'If printLabel Then
			Dim printerName As String = GetUserDataMaxtrixPrintername()
			If String.IsNullOrWhiteSpace(printerName) Then
				Dim msg = "Achtung: Sie haben keinen Drucker definiert!"
				msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine)

				m_UtilityUI.ShowInfoDialog(msg)
				Return
			End If

			' DATAMATRIX_VALUE_PATTERN_REPORT As String = "^KD_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
			Dim dataMatrixCode As String = GetDataMaxtrixCodeString()
			SP.Infrastructure.BarcodeUtility.PrintBarcode(String.Format(dataMatrixCode, m_EmployeeNumber), printerName)

			'End If

		End Sub

		''' <summary>
		''' Shows the new employee form.
		''' </summary>
		Private Sub ShowNewEmployeeFrom()

			Dim frmNewEmployee As frmNewEmployee = New frmNewEmployee(m_InitializationData, Nothing)

			frmNewEmployee.Show()
			frmNewEmployee.BringToFront()

		End Sub

		''' <summary>
		''' Deletes a selected employee.
		''' </summary>
		Private Function DeleteSelectedEmployee(ByVal aksUser As Boolean) As DeleteEmployeeResult
			Dim result = DeleteEmployeeResult.Deleted

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))
				Return Nothing
			End If
			If aksUser Then
				If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																										m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
					Return DeleteEmployeeResult.ErrorWhileDelete
				End If
			End If


			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)

			result = m_EmployeeDatabaseAccess.DeleteEmployee(employeeMasterData.EmployeeNumber,
																											 ConstantValues.ModulName, String.Format("{0}, {1}",
																											 m_InitializationData.UserData.UserLName, m_InitializationData.UserData.UserFName),
																											 m_InitializationData.UserData.UserNr)


			Dim msg As String = String.Empty

			Select Case result
				Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingPropose
					msg = "Der ausgewählte Kandidat hat Vorschläge."

				Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingES
					msg = "Der ausgewählte Kandidat hat Einsätze."

				Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingRP
					msg = "Der ausgewählte Kandidat hat Rapporte."

				Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingZG
					msg = "Der ausgewählte Kandidat hat Vorschüsse."

				Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingLM
					msg = "Der ausgewählte Kandidat hat monatliche Lohnangaben."

				Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingLO
					msg = "Der ausgewählte Kandidat hat Lohnabrechnungen."

				Case DeleteEmployeeResult.ErrorWhileDelete
					msg = "Die Daten konnten nicht gelöscht werden."

				Case DeleteEmployeeResult.Deleted
					msg = "Der ausgewählte Kandidat wurde erfolgreich gelöscht."
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg),
																	 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)
					Return result

			End Select
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(String.Format(msg & "{0}Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen.", vbNewLine)),
															 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)


			Return result
		End Function

		''' <summary>
		''' Shows the employee photo module for employee.
		''' </summary>
		Private Sub OpenFormforChangeEmployeePhoto()

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Dim o2Open As New SpImageProcess.ClsMain_Net

			Try
				o2Open.ShowfrmMAPhoto(m_EmployeeNumber, String.Empty)
				PrepareStatusAndNavigationBar(m_EmployeeNumber)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das gewünschte Programm kann nicht ausgeführt werden."))

			End Try

		End Sub


		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
				Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
				If setting_form_mainsplitter > 0 Then Me.sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_form_mainsplitter)

				If Not String.IsNullOrEmpty(setting_form_location) Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Saves the form settings.
		''' </summary>
		Private Sub SaveFromSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccMain.SplitterPosition)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFromSettings()

			' Cleanup child panels.
			'If Not m_PropertyForm Is Nothing AndAlso Not m_PropertyForm.IsDisposed Then

			'	Try
			'		m_PropertyForm.Close()
			'		m_PropertyForm.Dispose()
			'	Catch
			'		' Do nothing
			'	End Try
			'End If

			For Each tabControl In m_ListOfTabControls
				tabControl.CleanUp()
			Next

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub

#End Region

#Region "employee contact table"

		Private Function SaveEmployeeContactData(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String) As Boolean
			Dim success As Boolean = True

			Dim currentContactRecordNumber As Integer = 0
			Dim currentDocumentID As Integer = 0
			Dim contactData As EmployeeContactData = Nothing
			Dim fileContent As Byte() = Nothing

			If not String.IsNullOrWhiteSpace(attachedFile) Then
				fileContent = m_Utility.LoadFileBytes(attachedFile)
			End If

			Dim dt = DateTime.Now
			contactData = New EmployeeContactData With {.EmployeeNumber = m_EmployeeNumber,
																																		 .CreatedOn = dt,
																																		 .CreatedFrom = m_InitializationData.UserData.UserFullName}

			contactData.EmployeeNumber = m_EmployeeNumber
			contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
			contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
			contactData.ContactPeriodString = title
			contactData.ContactsString = description
			contactData.ContactImportant = False
			contactData.ContactFinished = False
			contactData.VacancyNumber = Nothing
			contactData.ProposeNr = Nothing
			contactData.ESNr = Nothing
			contactData.CustomerNumber = Nothing

			contactData.ChangedFrom = m_InitializationData.UserData.UserFullName
			contactData.ChangedOn = dt
			contactData.UsNr = m_InitializationData.UserData.UserNr

			' Check if the document bytes must be saved.
			If Not (attachedFile Is Nothing) And success Then

				Dim contactDocument As SP.DatabaseAccess.Employee.DataObjects.ContactMng.ContactDoc = Nothing


				contactDocument = New SP.DatabaseAccess.Employee.DataObjects.ContactMng.ContactDoc() With {.CreatedOn = dt,
																									 .CreatedFrom = m_InitializationData.UserData.UserFullName,
																									 .FileBytes = fileContent,
																									 .FileExtension = Path.GetExtension(attachedFile)}
				success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

				If success Then
					currentDocumentID = contactDocument.ID
					contactData.KontaktDocID = currentDocumentID
				End If

			End If

			' Insert contact
			contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

			If success Then
				currentContactRecordNumber = contactData.RecordNumber
			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Kontaktdaten konnten nicht gespeichert werden."))
			Else


			End If


			Return success
		End Function

		''' <summary>
		''' Combines date and time.
		''' </summary>
		''' <param name="dateComponent">The date component.</param>
		''' <param name="timeComponent">The time component (date is ignored)</param>
		''' <returns>Combined date and time</returns>
		Private Function CombineDateAndTime(ByVal dateComponent As DateTime?, ByVal timeComponent As DateTime?) As DateTime?

			If Not dateComponent.HasValue Then
				Return Nothing
			End If

			If Not timeComponent.HasValue Then
				Return dateComponent.Value.Date
			End If

			Dim timeSpan As TimeSpan = timeComponent.Value - timeComponent.Value.Date
			Dim dateAndTime = dateComponent.Value.Date.Add(timeSpan)

			Return dateAndTime
		End Function

#End Region



	End Class

End Namespace
