
Imports System.Reflection.Assembly

Imports SP.TodoMng
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel
Imports SP.MA.EmployeeMng.Settings
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
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports DevExpress.XtraBars
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors

Namespace UI

	''' <summary>
	''' Employee management.
	''' </summary>
	Public Class frmEmployees


#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx"

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

		'''' <summary>
		'''' Boolean flag indicating if initial data has been loaded.
		'''' </summary>
		'Private m_IsInitialDataLoaded As Boolean = False

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
		'Private Property m_SQL4Print As String

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
		Private m_CV_P_Data As NavBarItem

		Private m_AllowedWOS As Boolean
		Private m_PropertyForm As frmEmployeesProperties
		Private m_AllowedDesign As Boolean
		Private m_ApplicationUtilWebServiceUri As String


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
				'm_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI

				Dim domainName = m_InitializationData.MDData.WebserviceDomain
				m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)


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

			' Top tabs
			m_ListOfTabControls.Add(ucCommonData)
			m_ListOfTabControls.Add(ucMediation)
			m_ListOfTabControls.Add(ucLanguagesAndProfessions)
			m_ListOfTabControls.Add(ucContactData)
			m_ListOfTabControls.Add(ucDocumentManagement)

			' Bottom tabs
			m_ListOfTabControls.Add(ucSalaryData1)
			m_ListOfTabControls.Add(ucSalaryData2)
			m_ListOfTabControls.Add(ucMonthlySalaryData)
			m_ListOfTabControls.Add(ucBankData)

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfTabControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
			Next

			m_UCMediator = New UserControlFromMediator(Me,
																								ucCommonData,
																								ucSalaryData1,
																								ucSalaryData2,
																								ucBankData)
			m_TopActiveTabPage = ucCommonData
			m_BottomActiveTabPage = ucSalaryData1

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 105, m_InitializationData.MDData.MDNr)

			' Translate controls.
			TranslateControls()

			' Creates the navigation bar.
			CreateMyNavBar()

		End Sub

#End Region

		Public ReadOnly Property GetWOSTemplateJobNumber As List(Of String)
			Get
				Dim m_path As New ClsProgPath
				Dim m_md As New Mandant
				Dim selectedCategoryNumber = m_md.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Templates",
																													 "available_employee_wos_template_jobnr", String.Empty).Split(New Char() {";", ":", ","}, StringSplitOptions.RemoveEmptyEntries).ToList

				Return selectedCategoryNumber
			End Get
		End Property

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
				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
				Dim strQuery As String = String.Format("{0}/datamatrixcodestringforemployeelabel", FORM_XML_MAIN_KEY)
				Dim dataMatrixCode As String = m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), strQuery)
				If String.IsNullOrWhiteSpace(dataMatrixCode) OrElse dataMatrixCode.Length < 10 Then dataMatrixCode = "MA_{0}_999"

				Return dataMatrixCode

			End Get
		End Property

		Private ReadOnly Property GetDataMaxtrixCodeString(ByVal categorieNumber As Integer?) As String
			Get
				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
				Dim xmlQuery As String
				If categorieNumber.GetValueOrDefault(0) = 0 Then
					xmlQuery = String.Format("{0}/datamatrixcodestringforemployeelabel", FORM_XML_MAIN_KEY)
				Else
					xmlQuery = String.Format("{0}/datamatrixcodestringforemployeelabel_{1}", FORM_XML_MAIN_KEY, categorieNumber)
				End If

				Dim dataMatrixCode As String = m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), xmlQuery)
				If String.IsNullOrWhiteSpace(dataMatrixCode) OrElse dataMatrixCode.Length < 10 Then dataMatrixCode = "MA_{0}_{1}"


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

			Me.xtabQSTBew.Text = m_Translate.GetSafeTranslationValue(Me.xtabQSTBew.Text)
			Me.xtabLohnangaben.Text = m_Translate.GetSafeTranslationValue(Me.xtabLohnangaben.Text)
			Me.xtabMLohn.Text = m_Translate.GetSafeTranslationValue(Me.xtabMLohn.Text)
			Me.xtabBank.Text = m_Translate.GetSafeTranslationValue(Me.xtabBank.Text)

			Me.bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblErstellt.Caption)
			Me.bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblGeaendert.Caption)

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
			xtabMain.SelectedTabPage = xtabAllgemein

			' Bottom page
			m_BottomActiveTabPage.Deactivate()
			m_BottomActiveTabPage = ucSalaryData1
			xtabMoreinfo.SelectedTabPage = xtabQSTBew

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
		Public Sub SaveEmployeeData()
			Dim success As Boolean = True

			If (IsEmployeeDataLoaded AndAlso ValidateData()) Then

				' 1. Load the master, other (sonstiges) and contactComm data
				Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber)
				Dim employeeOtherData As EmployeeOtherData = m_EmployeeDatabaseAccess.LoadEmployeeOtherData(m_EmployeeNumber)
				Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_EmployeeNumber)
				Dim employeeLOSettingData As EmployeeLOSettingsData = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(m_EmployeeNumber)

				Dim oldWOSState As Boolean = employeeMasterData.Send2WOS.GetValueOrDefault(False)
				If employeeMasterData Is Nothing OrElse employeeOtherData Is Nothing OrElse employeeContactCommData Is Nothing OrElse employeeLOSettingData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden. Daten konnten nicht geladen werden."))
					m_Logger.LogError("master master data are nothing! employee can not be savee.")

					Return
				End If
				Dim employeeExistingMasterData = employeeMasterData.Clone()
				Dim employeeExistingLOSettingData = employeeLOSettingData.Clone()

				' 2. Ask all tabs to merge its data with the records just loaded.
				For Each tabControl In m_ListOfTabControls
					tabControl.MergeEmployeeMasterData(employeeMasterData)
					tabControl.MergeEmployeeOtherData(employeeOtherData)
					tabControl.MergeEmployeeContactCommData(employeeContactCommData)
					tabControl.MergeEmployeeLOSettingsData(employeeLOSettingData)
				Next

				' 3. Update the data in the database
				employeeMasterData.ChangedOn = DateTime.Now
				employeeMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName
				employeeMasterData.ChangedUserNumber = m_InitializationData.UserData.UserNr
				employeeMasterData.Send2WOS = tgsSendToWOS.EditValue
				employeeMasterData.SendDataWithEMail = tgsSendDataWithEMail.EditValue

				If m_AllowedWOS Then employeeContactCommData.WebExport = tgsExportWeb.EditValue

				Try
					success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(employeeMasterData)
					success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeOtherData(employeeOtherData)
					success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeConactCommData(employeeContactCommData)
					success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeLOSettings(employeeLOSettingData)

					success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeBackupHistory(employeeExistingMasterData, employeeExistingLOSettingData, employeeMasterData, employeeLOSettingData, m_InitializationData.UserData.UserNr)


					Dim message As String = String.Empty

					If (success) Then
						success = success AndAlso employeeMasterData.ApplicantID.GetValueOrDefault(0) > 0 AndAlso PerformUpdateAssignedRemoteApplicantWebservice(m_EmployeeNumber)

						message = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.{0}{1}")
						bsiChanged.Caption = String.Format(" {0:f}, {1}", employeeMasterData.ChangedOn, employeeMasterData.ChangedFrom)

						Dim TransferedToWeb = SendEmployeeDataToWebServer()
						If Not TransferedToWeb.Message.Contains("No License") Then
							Dim transfermsg As String = String.Empty
							Dim currentWOSState As Boolean = employeeMasterData.Send2WOS.GetValueOrDefault(False)

							If currentWOSState Then transfermsg = "Ihre Daten wurden erfolgreich übermittelt."
							If Not currentWOSState AndAlso oldWOSState Then transfermsg = "Ihre Daten wurden erfolgreich entfernt."
							If Not currentWOSState AndAlso Not oldWOSState Then transfermsg = ""

							transfermsg = m_Translate.GetSafeTranslationValue(transfermsg)
							If TransferedToWeb.Value Then
								message = String.Format(message, vbNewLine, transfermsg)
							Else
								transfermsg = String.Format("{0}<br>{1}", m_Translate.GetSafeTranslationValue("Ihre Daten konnten <b>nicht</b> erfolgreich übermittelt werden."), TransferedToWeb.Message.ToString())
								message = String.Format(message, vbNewLine, transfermsg)
							End If
						Else
							message = String.Format(message, String.Empty, String.Empty)
						End If

						If TransferedToWeb.Value Then
							m_UtilityUI.ShowOKDialog(Me, message, m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Information)
						Else
							m_UtilityUI.ShowErrorDialog(message, m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Exclamation)
						End If
					Else
						message = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
						m_UtilityUI.ShowErrorDialog(message)

					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("m_EmployeeNumber: {0} | {1}", m_EmployeeNumber, ex.ToString))

					Return
				End Try

			Else
				Dim Message = String.Format(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.{0}Möglicherweise sind <b>pflichtfelder nicht</b> ausgefüllt!"), vbNewLine)
				Dim caption = "Fehlende Informationen <b>(Pflichtfelder)</b>"
				m_UtilityUI.ShowErrorDialog(Message, caption)

			End If

		End Sub

		Private Function SendEmployeeDataToWebServer() As WOSSendResult
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True, .Message = String.Empty}
			If Not m_mandant.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year) Then Return New WOSSendResult With {.Value = True, .Message = "No License"}

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber)
			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_EmployeeNumber)

			Dim bExportToWeb = employeeContactCommData.WebExport.GetValueOrDefault(False)

			Dim transferObj As New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)

			If bExportToWeb Then
				m_PrintJobNr = String.Empty
				Dim employeeTemplate = CreateAvailableEmployeeTemplateForTransferToWOS()
				If employeeTemplate Is Nothing OrElse employeeTemplate.Count = 0 Then
					m_Logger.LogWarning(String.Format("no tamplate to transfer available employee: {0}", m_EmployeeNumber))
				End If
				Dim transferAvailableEmployeeResult = transferObj.TransferEmployeeDataToWOS(m_EmployeeNumber, employeeTemplate)
				result = transferAvailableEmployeeResult

			Else
				If Not String.IsNullOrWhiteSpace(employeeMasterData.Transfered_Guid) Then
					Dim transferResult = transferObj.DeleteTransferedEmployeeDataFromWOS(m_EmployeeNumber)
					result = transferResult
				End If

			End If

			Return result
		End Function

		Private Function PerformUpdateAssignedRemoteApplicantWebservice(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True


			Dim existingData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
			If existingData Is Nothing Then Return False
			If existingData.ApplicantID.GetValueOrDefault(0) > 0 Then Return True

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
		Private Sub OnfrmApplicant_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnfrmApplicant_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

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
		Private Sub OnxtraTabControl_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles xtabMain.SelectedPageChanging

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

				If (Object.ReferenceEquals(page, xtabQSTBew)) Then
					IsDataValid = ucSalaryData1.Activate(m_EmployeeNumber)
					m_BottomActiveTabPage = ucSalaryData1
				ElseIf (Object.ReferenceEquals(page, xtabLohnangaben)) Then
					IsDataValid = ucSalaryData2.Activate(m_EmployeeNumber)
					m_BottomActiveTabPage = ucSalaryData2
				ElseIf (Object.ReferenceEquals(page, xtabMLohn)) Then
					IsDataValid = ucMonthlySalaryData.Activate(m_EmployeeNumber)
					m_BottomActiveTabPage = ucMonthlySalaryData
				ElseIf (Object.ReferenceEquals(page, xtabBank)) Then
					IsDataValid = ucBankData.Activate(m_EmployeeNumber)
					m_BottomActiveTabPage = ucBankData
				End If

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
		Private Sub OnnbMain_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
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
					Case "New_Employee".ToLower
						ShowNewEmployeeFrom()
					Case "Save_Employee_Data".ToLower
						SaveEmployeeData()
					Case "Print_Employee_Data".ToLower
						GetMenuItems4Print()
					Case "Close_Employee_Form".ToLower
						Me.CleanupAndHideForm()
					Case "delete_Employee_Data".ToLower
						If DeleteSelectedEmployee() = DeleteEmployeeResult.Deleted Then Me.CleanupAndHideForm()

					Case "Employee_ShowCVDetails".ToLower
						ShowEmployeeCVLDetails()
					Case "Employee_versand".ToLower
						ShowEmployeeVersandFields()
					Case "Employee_beruffach".ToLower
						ShowEmployeeBerufFach()

					Case "employee_properties".ToLower
						ShowEmployeeProperties()
					Case "outlook_employee_Data".ToLower()
						SendEmployeedata2OutlookContact()
					'Case "wos_Employee_Data".ToLower()
					'	SendWOSLink()
					Case "CreateTODO".ToLower
						ShowTodo()
					Case "ShowGuthaben".ToLower()
						ShowGuthaben()
					Case "ShowNLAData".ToLower()
						ShowNLAData()
					Case "ShowKIData".ToLower()
						ShowKIData()
					Case "ShowMoreAddress".ToLower()
						ShowMoreAddresses()
					Case "ShowInterviews".ToLower()
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
		''' save employeedata to outlookcontact (Kontakt\Sputnik Kandidaten) folder
		''' </summary>
		''' <remarks></remarks>
		Private Sub SendEmployeedata2OutlookContact()
			Dim objItem As Object
			Dim AddNewrec As Integer
			Dim strDesFoldername As String
			Dim headerLabel As String
			Dim iRecCount As Integer

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Return
			End If

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterstammdaten konnten nicht geladen werden."))

				Return
			End If

			Try

				Dim ol As Object = CreateObject("Outlook.Application")
				Dim olns As Object = ol.GetNamespace("MAPI")
				Dim objFolder As Object = SetContactFolder(ol, olns, "Sputnik Kandidaten")

				If objFolder Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Outlook-Kontakt Ordner konnte nicht ermittelt werden."))

					Return
				End If

				strDesFoldername = objFolder.Name

				headerLabel = String.Format("{0} {1}", IIf(employeeMasterData.Gender.ToString.ToUpper = "M", "Herr", "Frau"), employeeMasterData.EmployeeFullname)

				' Existiert ein Dopplikat?
				For Each objItem In objFolder.Items
					If Val(UCase$(objItem.CustomerID)) = m_EmployeeNumber AndAlso UCase$(objItem.ReferredBy) = UCase$("MA") Then

						If AddNewrec > 0 Then Exit For
						m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("Möglicherweise ist der Datensatz bereits vorhanden.{0}{1}: {2}"),
																									 vbNewLine, m_EmployeeNumber, headerLabel), m_Translate.GetSafeTranslationValue("Dopplikat"))

						Return
					End If
				Next objItem

				Dim objNewContact As Object = objFolder.Items.Add
				With objNewContact
					.LastName = employeeMasterData.Lastname
					.FirstName = employeeMasterData.Firstname
					.FullName = headerLabel

					.HomeAddressState = employeeMasterData.S_Canton
					.HomeAddressPostOfficeBox = employeeMasterData.PostOfficeBox
					.HomeAddressCity = employeeMasterData.Location
					.HomeAddressStreet = employeeMasterData.Street
					.HomeAddressPostalCode = employeeMasterData.Postcode
					.HomeAddressCountry = employeeMasterData.Country

					If employeeMasterData.Birthdate.HasValue Then .Birthday = employeeMasterData.Birthdate

					.HomeTelephoneNumber = employeeMasterData.Telephone_P
					.MobileTelephoneNumber = employeeMasterData.MobilePhone
					.HomeFaxNumber = employeeMasterData.Telephone2
					.BusinessTelephoneNumber = employeeMasterData.Telephone_G
					.BusinessFaxNumber = employeeMasterData.Telephone3
					.Webpage = employeeMasterData.Homepage
					.eMail1Address = employeeMasterData.Email

					.Body = employeeMasterData.V_Hint
					.Profession = employeeMasterData.Profession
					.JobTitle = employeeMasterData.Profession
					.Language = employeeMasterData.Language
					.Categories = "Sputnik Enterprise"

					.CustomerID = m_EmployeeNumber
					.ReferredBy = "MA"

					.Save
					'.Display(True)
					iRecCount = iRecCount + 1

				End With

				m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in Kontakt\{0} gespeichert."), strDesFoldername))

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich exportiert werden.<br>{0}"), ex.ToString))
				m_Logger.LogError(ex.ToString)

				Return
			End Try

		End Sub

		Private Function SetContactFolder(ByVal ol As Object, ByVal olns As Object, ByVal strFoldername As String) As Object
			Dim objFolder As Object = olns.GetDefaultFolder(10)

			Try
				Dim fldrs As Object = objFolder.Folders

				For Each folder In fldrs
					If folder.name = strFoldername Then
						objFolder = objFolder.Folders(strFoldername)

						Return objFolder
					End If
				Next
				objFolder.Folders.Add(strFoldername)
				objFolder = objFolder.Folders(strFoldername)

			Catch ex As Exception
				Try
					objFolder = objFolder.Folders(strFoldername)
				Catch e As Exception
					Return Nothing
				End Try


			End Try


			Return objFolder

		End Function


		'''' <summary>
		'''' sends automated mail to employee email-address
		'''' </summary>
		'''' <remarks></remarks>
		'Private Sub SendWOSLink()

		'	If m_EmployeeNumber Is Nothing Then Return
		'	Dim msg = m_Translate.GetSafeTranslationValue("Ihr KandidatIn wird automatisch über die neuen Dokumenten-Uploads informiert. Möchten Sie trotzdem eine Benachrichtigung senden?")
		'	If Not m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Online Dokumente")) Then Return

		'	Dim notifyMail = New SPSSendMail.RichEditSendMail.SendWOSMailNotification(m_InitializationData)

		'	Dim preselectionSetting As New SPSSendMail.RichEditSendMail.PreselectionMailData With {.MailType = SPSSendMail.RichEditSendMail.MailTypeEnum.EmployeeWOS,
		'		.EmployeeNumber = m_EmployeeNumber}
		'	notifyMail.PreselectionData = preselectionSetting

		'	Dim result = notifyMail.SendEmployeeWOSNotification()

		'	If result.Value Then
		'		m_UtilityUI.ShowInfoDialog("Die Nachricht mit Dokumentenlink wurde erfolgreich versendet.")
		'	Else
		'		msg = m_Translate.GetSafeTranslationValue("Die Nachricht mit Dokumentenlink wurde nicht versendet.{0}{1}")
		'		m_UtilityUI.ShowErrorDialog(String.Format(msg, vbNewLine, result.ValueLable))
		'	End If

		'End Sub

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

			frmTodo.EmployeeNumber = m_EmployeeNumber
			frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
													VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

			frmTodo.Show()

		End Sub

		' ''' <summary>
		' ''' shows propertyform for selected employee
		' ''' </summary>
		' ''' <remarks></remarks>
		'Private Sub ShowCustomerProperties()
		'  Dim oMyProg As Object

		'  If (Not IsEmployeeDataLoaded) Then
		'    m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

		'    Exit Sub
		'  End If

		'  Try
		'    oMyProg = CreateObject("SPSModulsView.ClsMain")
		'    oMyProg.TranslateProg4Net("ShowMAExtras", m_EmployeeNumber)

		'  Catch e As Exception
		'    m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

		'  End Try

		'End Sub


		''' <summary>
		''' Shows Guthaben.
		''' </summary>
		Private Sub ShowGuthaben()

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If
			Try
				Dim frm = New SPS.MA.Guthaben.frmMAGuthaben(m_InitializationData)
				frm.EmployeeNumber = m_EmployeeNumber

				frm.LoadData()
				frm.Show()
				frm.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		End Sub

		''' <summary>
		''' Shows KIData.
		''' </summary>
		Private Sub ShowKIData()
			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If
			Dim o2Open As New SPMAAddress.frmMAKiAU(m_InitializationData)
			Try
				o2Open.LoadData(m_EmployeeNumber)
				o2Open.Show()
				o2Open.BringToFront()


			Catch e As Exception
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

			End Try

		End Sub

		''' <summary>
		''' shows nlafield-data for employee
		''' </summary>
		''' <remarks></remarks>
		Private Sub ShowNLAData()
			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If
			Dim o2Open As New SPMAAddress.frmNLAData(m_InitializationData)
			Try
				o2Open.LoadData(m_EmployeeNumber)
				o2Open.Show()
				o2Open.BringToFront()

			Catch e As Exception
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

			End Try

		End Sub

		''' <summary>
		''' Shows more addresses.
		''' </summary>
		Private Sub ShowMoreAddresses()
			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If
			'Dim o2Open As New SPMAAddress.ClsMain_Net(m_InitializationData)
			'o2Open.ShowfrmMAAddress(m_EmployeeNumber)

			Try
				Dim obj As New SPMAAddress.frmMAAddress(m_InitializationData)

				obj.EmployeeNumber = m_EmployeeNumber
				obj.LoadData()
				obj.Show()
				obj.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString)
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
			'bbiDatamatrix.ButtonStyle = BarButtonStyle.DropDown
			'			bbiDatamatrix.ActAsDropDown = False

			Try
			navMain.PaintStyleName = "SkinExplorerBarView"

				' Create a Local group.
				Dim groupDatei As NavBarGroup = New NavBarGroup(("Datei"))
				groupDatei.Name = "gNavDatei"

				Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
				New_P.Name = "New_Employee"
				New_P.SmallImage = Me.ImageCollection1.Images(0)

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

				m_CV_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Lebenslauf Details"))
				m_CV_P_Data.Name = "Employee_ShowCVDetails"
				m_CV_P_Data.SmallImage = Me.ImageCollection1.Images(17)
				m_CV_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 678, m_InitializationData.MDData.MDNr)

				Dim Versand_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten für Versand"))
				Versand_P_Data.Name = "Employee_versand"
				Versand_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 122, m_InitializationData.MDData.MDNr)

				Dim BerufFach_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Berufsgruppen und Fachbereiche"))
				BerufFach_P_Data.Name = "Employee_beruffach"
				BerufFach_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 124, m_InitializationData.MDData.MDNr)

				Dim Property_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Abhängigkeiten anzeigen"))
				Property_P_Data.Name = "Employee_properties"
				Property_P_Data.SmallImage = Me.ImageCollection1.Images(15)
				Property_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 128, m_InitializationData.MDData.MDNr)

				'm_Wos_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("WOS-Link senden"))
				'm_Wos_P_Data.Name = "wos_Employee_Data"
				'm_Wos_P_Data.SmallImage = Me.ImageCollection1.Images(6)

				Dim outlook_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Ins Outlook exportieren"))
				outlook_P_Data.Name = "outlook_employee_Data"
				outlook_P_Data.SmallImage = Me.ImageCollection1.Images(10)
				outlook_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 129, m_InitializationData.MDData.MDNr)


				Dim groupMoreModule As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras Verwaltung"))
				groupMoreModule.Name = "gNavMoreModule"

				Dim Guthaben_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Guthaben anzeigen"))
				Guthaben_Data.Name = "ShowGuthaben"
				Guthaben_Data.SmallImage = Me.ImageCollection1.Images(12)

				Dim Kinderangaben_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Angaben über Kinder"))
				Kinderangaben_Data.Name = "ShowKIData"
				Kinderangaben_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 112, m_InitializationData.MDData.MDNr)

				Dim NLAangaben_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Felder für Lohnausweis"))
				NLAangaben_Data.Name = "ShowNLAData"
				'NLAangaben_Data.SmallImage = Me.ImageCollection1.Images(9)

				Dim MoreAddresses_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Weitere Adressen"))
				MoreAddresses_Data.Name = "ShowMoreAddress"
				MoreAddresses_Data.SmallImage = Me.ImageCollection1.Images(13)
				MoreAddresses_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 123, m_InitializationData.MDData.MDNr)

				Dim Inverview_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorstellungen anzeigen"))
				Inverview_Data.Name = "ShowInterviews"
				Inverview_Data.SmallImage = Me.ImageCollection1.Images(11)

				Try
					navMain.BeginUpdate()

					navMain.Groups.Add(groupDatei)
					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 102, m_InitializationData.MDData.MDNr) Then
						groupDatei.ItemLinks.Add(New_P)
						groupDatei.ItemLinks.Add(m_SaveButton)
					End If
					groupDatei.ItemLinks.Add(Print_P_Data)
					groupDatei.ItemLinks.Add(Close_P_Data)

					groupDatei.Expanded = True

					navMain.Groups.Add(groupDelete)
					groupDelete.ItemLinks.Add(Delete_P_Data)
					groupDelete.Expanded = False

					navMain.Groups.Add(groupExtra)
					groupExtra.ItemLinks.Add(TODO_P_Data)
					groupExtra.ItemLinks.Add(Property_P_Data)

					groupExtra.ItemLinks.Add(outlook_P_Data)

					groupExtra.ItemLinks.Add(m_CV_P_Data)
					groupExtra.ItemLinks.Add(Versand_P_Data)
					groupExtra.ItemLinks.Add(BerufFach_P_Data)

					navMain.Groups.Add(groupMoreModule)

					groupMoreModule.ItemLinks.Add(Guthaben_Data)
					groupMoreModule.ItemLinks.Add(NLAangaben_Data)
					groupMoreModule.ItemLinks.Add(Kinderangaben_Data)
					groupMoreModule.ItemLinks.Add(MoreAddresses_Data)
					groupMoreModule.ItemLinks.Add(Inverview_Data)

					groupExtra.Expanded = True
					groupMoreModule.Expanded = True

					navMain.EndUpdate()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))

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
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterstammdaten (Statuszeile/Navigation) konnten nicht geladen werden."))
				Return False
			End If

			bsiCreated.Caption = String.Format(" {0:f}, {1}", employeeMasterData.CreatedOn, employeeMasterData.CreatedFrom)
			bsiChanged.Caption = String.Format(" {0:f}, {1}", employeeMasterData.ChangedOn, employeeMasterData.ChangedFrom)

			tgsSendToWOS.EditValue = employeeMasterData.Send2WOS.GetValueOrDefault(False)
			tgsSendDataWithEMail.EditValue = employeeMasterData.SendDataWithEMail
			m_AllowedWOS = m_mandant.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year)

			m_CV_P_Data.Enabled = employeeMasterData.CVLProfileID.GetValueOrDefault(0) > 0
			LoadSendingEmployeeAsAvailableEmployee(employeeNumber)

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
			LoadItemss4PrintDataMatrixCode()
			'PrintEmployeeDataMatrixCode()
		End Sub

		Private Sub LoadItemss4PrintDataMatrixCode()
			Dim categoryData = m_EmployeeDatabaseAccess.LoadEmployeeDocumentCategories()

			If (categoryData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Return
			End If

			Dim categoryViewData = Nothing

			categoryViewData = New List(Of CategoryVieData)
			categoryViewData.Add(New CategoryVieData With {.CategoryNumber = 0, .Description = m_Translate.GetSafeTranslationValue("Allgemeine Kategorie")})

			For Each category In categoryData

				Dim categoryDescription As String = String.Empty
				Select Case m_InitializationData.UserData.UserLanguage.ToLower().Trim()
					Case "d", "de"
						categoryDescription = category.DescriptionGerman
					Case "f", "fr"
						categoryDescription = category.DescriptionFrench
					Case "i", "it"
						categoryDescription = category.DescriptionItalian
					Case Else
						categoryDescription = category.DescriptionGerman
				End Select

				categoryViewData.Add(New CategoryVieData With {.CategoryNumber = category.CategoryNumber, .Description = categoryDescription})
			Next

			BarManager1.CloseMenus()
			BarManager1.ShowCloseButton = True

			BarManager1.BeginUpdate()
			BarManager1.ForceInitialize()

			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu.ClearLinks()
			popupMenu.Manager = BarManager1
			popupMenu.Manager.Images = Me.ImageCollection1

			Dim itm As New DevExpress.XtraBars.BarButtonItem
			For Each mnu As CategoryVieData In categoryViewData

				itm = New DevExpress.XtraBars.BarButtonItem
				Dim strMnuBez As String = If(mnu.CategoryNumber = 0, mnu.Description, mnu.ItemLabel)

				itm.Caption = strMnuBez
				itm.Name = mnu.CategoryNumber

				If popupMenu.ItemLinks.Count = 1 Then
					popupMenu.AddItem(itm).BeginGroup = True

				Else
					popupMenu.AddItem(itm)

				End If

				AddHandler itm.ItemClick, AddressOf PrintEmployeeDataMatrixCode_Staging

			Next

			BarManager1.EndUpdate()

			bbiDatamatrix.DropDownControl = popupMenu
			' show contextmenu
			'popupMenu.ShowPopup(New Point(Me.navMain.Width + Me.Left, Cursor.Position.Y))
			'popupMenu.ShowPopup(New Point(Cursor.Position.X - 10, Cursor.Position.Y - 10))

		End Sub

		Private Sub navMain_Click(sender As Object, e As EventArgs) Handles navMain.Click
			BarManager1.CloseMenus()
		End Sub

		Private Sub ucSalaryData1_MouseDown(sender As Object, e As MouseEventArgs) Handles ucSalaryData1.MouseDown, ucCommonData.MouseClick
			BarManager1.CloseMenus()
		End Sub

		Private Sub xtabMoreinfo_MouseClick(sender As Object, e As MouseEventArgs) Handles xtabMoreinfo.MouseClick, xtabMain.MouseClick
			BarManager1.CloseMenus()
		End Sub

		Private Sub LoadSendingEmployeeAsAvailableEmployee(ByVal employeeNumber As Integer)
			Dim sendData As Boolean = True
			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(employeeNumber)

			If (employeeContactCommData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))

				Return
			End If
			sendData = employeeContactCommData.WebExport.GetValueOrDefault(False)
			tgsExportWeb.EditValue = sendData
			tgsExportWeb.Enabled = m_AllowedWOS

		End Sub

		''' <summary>
		''' Build contextmenu for print.
		''' </summary>
		Private Sub GetMenuItems4Print()
			Dim mnuData = m_EmployeeDatabaseAccess.LoadContextMenu4PrintData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Return
			End If
			BarManager1.BeginUpdate()
			BarManager1.ForceInitialize()

			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu.Manager = Me.BarManager1
			popupMenu.Manager.Images = Me.ImageCollection1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem
				Dim strMnuBez As String = mnuData(i).MnuCaption

				itm.Caption = strMnuBez
				itm.Name = mnuData(i).MnuName

				If itm.Name.ToLower = "1.5".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 108, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "1.7".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 110, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "1.4".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 106, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "9.1".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554, m_InitializationData.MDData.MDNr) Then Continue For
				ElseIf itm.Name.ToLower = "1.0".ToLower Then
					If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 104, m_InitializationData.MDData.MDNr) Then Continue For
				End If


				If strMnuBez.StartsWith("_") OrElse strMnuBez.StartsWith("-") Then
					itm.Caption = m_Translate.GetSafeTranslationValue(strMnuBez.Remove(0, 1))
					popupMenu.AddItem(itm).BeginGroup = True
				Else
					itm.Caption = m_Translate.GetSafeTranslationValue(itm.Caption)
					popupMenu.AddItem(itm)
				End If

				If itm.Name = "1.0" Then
					AddHandler itm.ItemClick, AddressOf PrintEmployeeStammblatt

				ElseIf itm.Name = "1.4" Then
					AddHandler itm.ItemClick, AddressOf PrintSuvaStdListe4SelectedEmployee

				ElseIf itm.Name = "1.5" Then
					AddHandler itm.ItemClick, AddressOf PrintZV4SelectedEmployee

				ElseIf itm.Name = "1.7" Then
					AddHandler itm.ItemClick, AddressOf PrintARG4SelectedEmployee

				ElseIf itm.Name = "9.1" Then
					AddHandler itm.ItemClick, AddressOf PrintSalaryform4SelectedEmployee

				ElseIf itm.Name.Contains("AvailableEmployee.0.01") Then
					AddHandler itm.ItemClick, AddressOf PrintAvailableEmployeeTemplate

				ElseIf itm.Name.Contains("EmployeeTemplate.0.0") Then '  "EmployeeTemplate.0.01" Then
					AddHandler itm.ItemClick, AddressOf PrintAvailableEmployeeTemplate

				Else
					AddHandler itm.ItemClick, AddressOf PrintDocs

				End If

			Next

			' fill templates
			Dim mnuTemplatesData = m_EmployeeDatabaseAccess.LoadContextMenu4PrintTemplatesData
			If Not (mnuTemplatesData Is Nothing) Then
				For i As Integer = 0 To mnuTemplatesData.Count - 1
					itm = New DevExpress.XtraBars.BarButtonItem

					Dim strMnuBez As String = m_Translate.GetSafeTranslationValue(mnuTemplatesData(i).MnuCaption)
					Dim bAsGroup As Boolean = strMnuBez.StartsWith("-") OrElse i = 0
					itm.Caption = strMnuBez.Replace("-", "")
					itm.Name = String.Format("{0}|{1}", mnuTemplatesData(i).MnuDocPath, mnuTemplatesData(i).MnuDocMacro)

					If bAsGroup Then
						popupMenu.AddItem(itm).BeginGroup = True
					Else
						popupMenu.AddItem(itm)
					End If

					AddHandler itm.ItemClick, AddressOf PrintDocs
				Next
			End If
			BarManager1.EndUpdate()

			' show contextmenu
			popupMenu.ShowPopup(New Point(Me.navMain.Width + Me.Left, Cursor.Position.Y))

		End Sub

		''' <summary>
		''' Prints documents.
		''' </summary>
		Private Sub PrintDocs(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim tplFilename As String = String.Empty
			Dim _reg As New SPProgUtility.ClsDivReg
			Dim fileExtension As String = String.Empty
			Dim newFilename As String = String.Empty


			_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MANr", m_EmployeeNumber)
			_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "MandantNumber", m_InitializationData.MDData.MDNr)
			_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "CurrentRecordMandantPath", m_InitializationData.MDData.MDMainPath)
			_reg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "CurrentRecordMandantTemplatePath", m_InitializationData.MDData.MDTemplatePath)

			Dim strMenuName As String() = e.Item.Name.Split("|")
			If strMenuName.Length = 2 Then
				' then office-templates...

				If strMenuName(0) <> "automatedDoc" Then
					If Not strMenuName(0).Substring(1, 2) = ":\" And Not strMenuName(0).StartsWith("\\") Then
						tplFilename = String.Format("{0}{1}", m_mandant.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr), strMenuName(0))
					End If
					If Not File.Exists(tplFilename) Then
						m_Logger.LogError(String.Format("template could not be founded: {0}", tplFilename))
						m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Vorlage wurde nicht gefunden.<br>{0}"), tplFilename))

						Return
					End If

					Dim fi As New FileInfo(tplFilename)
					newFilename = String.Format("{0}{1}", m_path.GetSpS2DeleteHomeFolder, fi.Name)
					fileExtension = fi.Extension.ToUpper

					Try
						File.Copy(tplFilename, newFilename, True)
					Catch ex As Exception
						m_Logger.LogError(String.Format(m_Translate.GetSafeTranslationValue("Datei {0} konnte nicht in {1} kopiert werden. {2}"), tplFilename, newFilename, ex.ToString))
						newFilename = tplFilename

						Return
					End Try

				Else
					newFilename = Path.GetTempFileName()
					newFilename = Path.ChangeExtension(newFilename, "PDF")
					fileExtension = ".pdf"
				End If

				Try

					If fileExtension.ToUpper = ".DOC" OrElse fileExtension.ToUpper = ".DOCX" OrElse fileExtension.ToUpper = ".DOCM" Then
						LoadMWWordDocumentFiles(tplFilename, strMenuName(1), newFilename)

						'Dim _clsBrowser As New ClassBrowserPath
						'_clsBrowser.GetBrowserApplicationPath(tplFilename
						'Dim startInfo As New ProcessStartInfo

						'startInfo.FileName = _clsBrowser.GetBrowserPath
						'startInfo.Arguments = Chr(34) & newFilename & Chr(34) & If(strMenuName(1) <> String.Empty, " /m" & strMenuName(1), "")
						'startInfo.UseShellExecute = False
						'Process.Start(startInfo)

					ElseIf fileExtension.ToUpper = ".PDF" Then
						LoadPDFDocumentFiles(tplFilename, strMenuName(1), newFilename)

						'If strMenuName(1) = "1.0.1" Then
						'	Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newFilename,
						'.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber})}

						'	Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)
						'	Dim success = obj.PrintMATemplatePDU1PDF(_Setting)

						'	If Not success.Printresult Then
						'		m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newFilename))

						'		Return
						'	End If
						'ElseIf strMenuName(1) = "1.0.2" Then
						'	Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newFilename,
						'		.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber})}

						'	Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)
						'	Dim success = obj.PrintChilderFormularPDF(_Setting)

						'	If Not success.Printresult Then
						'		m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newFilename))

						'		Return
						'	End If

						'Else
						'	Process.Start(newFilename)

						'End If

					End If


				Catch ex As Exception
					m_Logger.LogError(String.Format(m_Translate.GetSafeTranslationValue("{0}.Datei konnte nicht geöffnet werden. {1}"), strMethodeName, ex.Message))
					m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Datei konnte nicht geöffnet werden. {0}"), ex.Message))
				End Try


			Else
				m_UtilityUI.ShowInfoDialog(String.Format("Die Methode wird nicht mehr unterstützt. Bitte kontaktieren Sie Ihrem Softwarelieferanten.{0}", e.Item.Name))

				'Me.m_PrintJobNr = e.Item.Name
				'PrintEmployeeTemplate()

			End If

		End Sub

		Private Sub LoadMWWordDocumentFiles(ByVal tplFilename As String, ByVal macroName As String, ByVal newDocFile As String)

			If Not File.Exists(tplFilename) Then Return
			Dim _clsBrowser As New ClassBrowserPath
			_clsBrowser.GetBrowserApplicationPath(tplFilename)
			Dim startInfo As New ProcessStartInfo

			startInfo.FileName = _clsBrowser.GetBrowserPath
			startInfo.Arguments = Chr(34) & newDocFile & Chr(34) & If(Not String.IsNullOrWhiteSpace(macroName), " /m" & macroName, "")
			startInfo.UseShellExecute = False

			Process.Start(startInfo)

		End Sub

		Private Sub LoadPDFDocumentFiles(ByVal tplFilename As String, ByVal jobNumber As String, ByVal newDocFile As String)

			If jobNumber = "1.0.1" Then
				If Not File.Exists(tplFilename) Then Return
				Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newDocFile,
							.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber})}

				Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)
				Dim success = obj.PrintMATemplatePDU1PDF(_Setting)

				If Not success.Printresult Then
					m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newDocFile))

					Return
				End If

			ElseIf jobNumber = "1.0.2" Then
				' EU_EFTA Forms
				Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newDocFile, .EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber})}
				Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)

				Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
				Dim fieldResult As String = String.Empty
				If ShowDesign Then
					fieldResult = obj.LoadPDFFieldDataWithDX(newDocFile)

				Else
					Dim result As PrintResult
					result = obj.PrintEU_EFTAFormularPDF(_Setting)

					If Not result.Printresult Then
						m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newDocFile))

						Return
					End If
				End If

			ElseIf jobNumber = "1.0.3" Then
				' TODO: Kinderzulagen
				Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMATemplateSetting With {.TemplateName = newDocFile, .EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber})}
				Dim obj As New SPS.Listing.Print.Utility.MATemplates.ClsPrintMATemplates(m_InitializationData)

				Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
				Dim fieldResult As String = String.Empty
				If ShowDesign Then
					fieldResult = obj.LoadPDFFieldDataWithDX(newDocFile)

				Else
					Dim result As PrintResult
					result = obj.PrintChilderFormularPDF(_Setting)

					If Not result.Printresult Then
						m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage konnte nicht geöffnet werden!{0}{1}"), vbNewLine, newDocFile))

						Return
					End If
				End If

			Else
				Process.Start(newDocFile)

			End If


		End Sub

		''' <summary>
		'''  Starts printdialog with List Label 18.
		''' </summary>
		Private Sub PrintEmployeeStammblatt(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			m_PrintJobNr = e.Item.Name

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
			Dim strResult As String = String.Empty
			Dim _PrintSetting As New ClsLLMASearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn, .SelectedMDNr = employeeMasterData.MDNr,
																															 .SQL2Open = String.Empty,
																															 .JobNr2Print = Me.m_PrintJobNr,
																															 .ShowAsDesign = ShowDesign,
																															 .frmhwnd = GetHwnd,
																															 .liMANr2Print = New List(Of Integer)(New Integer() {Me.m_EmployeeNumber})}

			Dim obj As New MAStammblatt.ClsPrintMAStammblatt(m_InitializationData)
			obj.PrintData = _PrintSetting

			strResult = obj.PrintMAStammBlatt()

		End Sub

		Private Sub PrintEmployeeDataMatrixCode_Staging(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
			If (Not IsEmployeeDataLoaded) Then Return

			Dim menuID As String = Val(e.Item.Name.ToString)

			Dim printerName As String = GetUserDataMaxtrixPrintername()
			If String.IsNullOrWhiteSpace(printerName) Then
				Dim msg = "Achtung: Sie haben keinen Drucker definiert!"
				msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine)

				m_UtilityUI.ShowInfoDialog(msg)
				Return
			End If

			' DATAMATRIX_VALUE_PATTERN_REPORT As String = "^KD_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"

			Dim dataMatrixCode As String = String.Empty
			Select Case menuID
				Case 0
					dataMatrixCode = GetDataMaxtrixCodeString(Nothing)


				Case Else
					dataMatrixCode = GetDataMaxtrixCodeString(menuID)


			End Select
			SP.Infrastructure.BarcodeUtility.PrintBarcode(String.Format(dataMatrixCode, m_EmployeeNumber, If(menuID = 0, 999, menuID)), printerName)

			Me.Focus()


		End Sub

		Private Sub PrintEmployeeDataMatrixCode()
			If (Not IsEmployeeDataLoaded) Then Return

			Dim printerName As String = GetUserDataMaxtrixPrintername()
			If String.IsNullOrWhiteSpace(printerName) Then
				Dim msg = "Achtung: Sie haben keinen Drucker definiert!"
				msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine)

				m_UtilityUI.ShowInfoDialog(msg)
				Return
			End If

			' DATAMATRIX_VALUE_PATTERN_REPORT As String = "^KD_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
			Dim dataMatrixCode As String = String.Empty
			dataMatrixCode = GetDataMaxtrixCodeString(Nothing)
			SP.Infrastructure.BarcodeUtility.PrintBarcode(String.Format(dataMatrixCode, m_EmployeeNumber), printerName)

			Me.Focus()

		End Sub

		''' <summary>
		''' startet das Threading zum öffnen der Einsatzverwaltung
		''' </summary>
		''' <remarks></remarks>
		Sub PrintSuvaStdListe4SelectedEmployee()

			Dim frm As New SP.Employee.SuvaSTDSearch.frmSUVAStd(m_InitializationData)

			Dim employeeNumbers As New List(Of Integer?) '(New Integer() {1352655})
			employeeNumbers.Add(m_EmployeeNumber)

			Dim preselectionSetting As New SP.Employee.SuvaSTDSearch.PreselectionData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumbers = employeeNumbers, .ListYear = Now.Year} ' New List(Of Integer?)(New Integer() {1352655})}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()



			'Dim t As Thread = New Thread(AddressOf OpenForm4SuvaStdListeWithThreading)

			't.IsBackground = True
			't.Name = "OpenForm4SuvaStdListeWithThreading"
			''t.SetApartmentState(ApartmentState.STA)
			't.Start()

		End Sub

		''' <summary>
		''' startet das Threading zum öffnen der Zwischenverdienstverwaltung
		''' </summary>
		''' <remarks></remarks>
		Sub PrintZV4SelectedEmployee()
			Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Try
				Dim frmZV = New SPS.MA.Guthaben.frmZV(m_InitializationData)

				Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionZVData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = m_EmployeeNumber}
				frmZV.PreselectionData = preselectionSetting

				frmZV.LoadData()
				frmZV.DisplayEmployeeData()

				frmZV.Show()
				frmZV.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

			End Try

			'Dim t As Thread = New Thread(AddressOf OpenZVFormWithThreading)

			't.IsBackground = True
			't.Name = "OpenZVFormWithThreading"
			''t.SetApartmentState(ApartmentState.STA)
			't.Start()

		End Sub


		''' <summary>
		''' startet das Threading zum öffnen der Arbeitgeberbescheinigung 
		''' </summary>
		''' <remarks></remarks>
		Sub PrintARG4SelectedEmployee()
			Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Try

				Dim frmARGB = New SPS.MA.Guthaben.frmARGB(m_InitializationData)

				Dim preselectionSetting As New SPS.MA.Guthaben.PreselectionARGBData With {.MDNr = m_InitializationData.MDData.MDNr, .EmployeeNumber = m_EmployeeNumber}
				frmARGB.PreselectionData = preselectionSetting

				frmARGB.LoadData()
				frmARGB.DisplayEmployeeData()

				frmARGB.Show()
				frmARGB.BringToFront()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

			End Try

			'Dim t As Thread = New Thread(AddressOf OpenARGFormWithThreading)

			't.IsBackground = True
			't.Name = "OpenARGFormWithThreading"
			''t.SetApartmentState(ApartmentState.STA)
			't.Start()

		End Sub


		''' <summary>
		''' prints Salaryform for selected employee
		''' </summary>
		''' <remarks></remarks>
		Private Sub PrintSalaryform4SelectedEmployee()

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			Dim strResult As String = "Success..."
			Dim liLONr As New List(Of Integer)
			Try
				Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.SelectedMANr = New List(Of Integer)(New Integer() {m_EmployeeNumber}),
																																	 .SelectedLONr = New List(Of Integer)(New Integer() {0}),
																																	 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
																																	 .SelectedYear = New List(Of Integer)(New Integer() {0}),
																																	 .SearchAutomatic = True,
																																	 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																	 .LogedUSNr = m_InitializationData.UserData.UserNr}

				'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(m_InitializationData, _settring)
				'obj.ShowfrmLO4Print()

				Dim obj As New SP.LO.PrintUtility.frmLOPrint(m_InitializationData)
				obj.LOSetting = _settring

				obj.Show()
				obj.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		End Sub

		Private Sub PrintAvailableEmployeeTemplate(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If
			m_PrintJobNr = e.Item.Name

			Dim result = CreateAvailableEmployeeTemplateForTransferToWOS()
			If Not result Is Nothing AndAlso result.Count > 0 Then
				Process.Start(result(0))
			End If

		End Sub

		Private Function CreateAvailableEmployeeTemplateForTransferToWOS() As List(Of String)
			Dim result As New List(Of String)
			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			Dim printJobNrs = GetWOSTemplateJobNumber
			Dim templateFile As List(Of String) = Nothing


			If Not String.IsNullOrWhiteSpace(m_PrintJobNr) Then
				printJobNrs.Clear()
				printJobNrs.Add(m_PrintJobNr)
			ElseIf (printJobNrs Is Nothing OrElse printJobNrs.Count = 0) AndAlso String.IsNullOrWhiteSpace(m_PrintJobNr) Then
				Return Nothing
			End If

			Dim obj As New SPS.Listing.Print.Utility.ClsLLMATemplatesPrint(m_InitializationData)
			templateFile = New List(Of String)

			For Each jobNr In printJobNrs
				Dim _PrintSetting As New ClsLLMATemplateSetting With {.EmployeeNumbers2Print = New List(Of Integer)(New Integer() {m_EmployeeNumber}), .FrmHwnd = 0, .TemplateJobNumber = jobNr, .ShowInDesign = ShowDesign, .CreateExportFile = True}

				obj.PrintSetting = _PrintSetting
				Dim success = obj.PrintAssignedEmployeeWOSTemplateData()

				obj.Dispose()
				If success AndAlso _PrintSetting.CreateExportFile AndAlso Not _PrintSetting.ExportedFiles Is Nothing Then templateFile.Add(_PrintSetting.ExportedFiles(0))
			Next
			If Not templateFile Is Nothing AndAlso templateFile.Count > 0 Then result = templateFile

			Return result
		End Function

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
		Private Function DeleteSelectedEmployee() As DeleteEmployeeResult
			Dim result = DeleteEmployeeResult.Deleted

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))
				Return Nothing
			End If
			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"), m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return DeleteEmployeeResult.ErrorWhileDelete
			End If

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
			result = m_EmployeeDatabaseAccess.DeleteEmployee(employeeMasterData.EmployeeNumber, ConstantValues.ModulName, m_InitializationData.UserData.UserFullNameWithComma, m_InitializationData.UserData.UserNr)


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

		'''' <summary>
		'''' Shows the terminplaner for user.
		'''' </summary>
		'Private Sub ShowAdvisorTerminplaner()
		'	Dim oMyProg As Object

		'	If (Not IsEmployeeDataLoaded) Then
		'		m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

		'		Exit Sub
		'	End If

		'	Try
		'		oMyProg = CreateObject("SPSModulsView.ClsMain")
		'		oMyProg.TranslateProg4Net("SPSTerminUtil.ClsMain", m_InitializationData.UserData.UserNr)

		'	Catch e As Exception
		'		m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Das gewünschte Programm kann nicht gestartet werden.")))

		'	End Try

		'End Sub

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
		''' Shows the employee CVL Details.
		''' </summary>
		Private Sub ShowEmployeeCVLDetails()

			If (Not IsEmployeeDataLoaded) OrElse ucCommonData.EmployeeCVLProfileID.GetValueOrDefault(0) = 0 Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Dim frmCVL As New frmCVLData(m_InitializationData)

			Try
				frmCVL.LoadCVLData(m_EmployeeNumber, ucCommonData.EmployeeCVLProfileID)
				frmCVL.Show()
				frmCVL.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das gewünschte Programm kann nicht ausgeführt werden."))

			End Try

		End Sub

		''' <summary>
		''' Shows the employee VersandFields module.
		''' </summary>
		Private Sub ShowEmployeeVersandFields()

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Try
				Dim obj As New SP.MA.MassenVersand.frmMAMassenversand(m_InitializationData)
				obj.EmployeeNumber = m_EmployeeNumber

				Dim success = obj.LoadData()
				If success Then
					obj.Show()
					obj.BringToFront()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das gewünschte Programm kann nicht ausgeführt werden."))

			End Try

		End Sub

		''' <summary>
		''' Shows the employee Berufsgruppen und Fachbereiche module.
		''' </summary>
		Private Sub ShowEmployeeBerufFach()

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Dim obj As New SP.MA.MassenVersand.ClsMain_Net

			Try
				obj.ShowfrmMABerufgruppe(m_EmployeeNumber)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das gewünschte Programm kann nicht ausgeführt werden."))

			End Try

		End Sub

		''' <summary>
		''' Shows the employee properties.
		''' </summary>
		Private Sub ShowEmployeeProperties()

			If (Not IsEmployeeDataLoaded) Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Exit Sub
			End If

			Try
				If m_PropertyForm Is Nothing OrElse m_PropertyForm.IsDisposed Then
					m_PropertyForm = New frmEmployeesProperties(m_InitializationData, m_Translate, m_EmployeeNumber, employeePicture.Image)
				End If
				m_PropertyForm.LoadData(m_EmployeeNumber, "show_propose", employeePicture.Image)
				m_PropertyForm.Show()
				m_PropertyForm.BringToFront()

			Catch e As Exception
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", e.ToString))

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
			If Not m_PropertyForm Is Nothing AndAlso Not m_PropertyForm.IsDisposed Then

				Try
					m_PropertyForm.Close()
					m_PropertyForm.Dispose()
				Catch
					' Do nothing
				End Try
			End If

			For Each tabControl In m_ListOfTabControls
				tabControl.CleanUp()
			Next

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub



#End Region


		<Serializable()>
		Class SampleRefType
			Public Val As EmployeeMasterData
		End Class

		<Serializable()>
		Private Class EmployeeMasterShallowCloneData
			Public someObject As EmployeeMasterData
			Function ShallowClone() As EmployeeMasterData
				Return CType(Me.MemberwiseClone(), EmployeeMasterData)
			End Function
		End Class

		<Serializable()>
		Private Class EmployeeLOSettingShallowCloneData
			Public someObject As SampleRefType
			Function ShallowClone() As EmployeeLOSettingShallowCloneData
				Return CType(Me.MemberwiseClone(), EmployeeLOSettingShallowCloneData)
			End Function
		End Class


		Class CategoryVieData

			Public Property CategoryNumber As Integer?
			Public Property Description As String

			Public ReadOnly Property ItemLabel As String
				Get
					Return String.Format("({0}) - {1}", CategoryNumber, Description)
				End Get
			End Property

		End Class


	End Class

End Namespace
