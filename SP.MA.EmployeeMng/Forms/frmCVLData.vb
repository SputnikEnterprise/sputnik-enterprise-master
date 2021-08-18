
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.MA.EmployeeMng.Settings


Namespace UI

	Public Class frmCVLData


#Region "Private Consts"

    Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx" ' "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

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
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		Private m_mandant As Mandant

		''' <summary>
		''' Boolan flag indicating if the form has been initialized.
		''' </summary>
		Private m_IsInitialized = False

		Private m_CustomerID As String
		Private m_CVLProfileID As Integer?
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

			' Bottom tabs
			m_ListOfTabControls.Add(ucApplicationData)
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

			m_BottomActiveTabPage = ucApplicationData

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			Dim domainName = m_InitializationData.MDData.WebserviceDomain

			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

			TranslateControls()


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

#End Region

#Region "Public Methods"

		''' <summary>
		''' Show the data of an employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadCVLData(ByVal employeeNumber As Integer, ByVal profileID As Integer?) As Boolean

			If Not m_IsInitialized Then
				Reset()
				m_IsInitialized = True
			End If

			CleanUp()

			m_SuppressUIEvents = True

			Dim success As Boolean = True
			m_CVLProfileID = profileID
			'success = success AndAlso m_TopActiveTabPage.Activate(employeeNumber)

			success = success AndAlso PerformCVLProfileDataWebservice(m_CVLProfileID)
			success = success AndAlso m_BottomActiveTabPage.Activate(employeeNumber)
			success = success AndAlso PrepareStatusAndNavigationBar(employeeNumber)

			m_EmployeeNumber = IIf(success, employeeNumber, Nothing)

			m_SuppressUIEvents = False

			Return success
		End Function

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

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
			'm_TopActiveTabPage.Deactivate()
			'm_TopActiveTabPage = ucDocumentManagement
			'XtraTabControl1.SelectedTabPage = xtabAllgemein

			' Bottom page
			m_BottomActiveTabPage.Deactivate()
			m_BottomActiveTabPage = ucApplicationData
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
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmCVLData_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmCVLData_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

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
		Private Sub OnxTabMoreInfo_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles xtabMoreinfo.SelectedPageChanging
			Dim IsDataValid As Boolean = True

			If m_SuppressUIEvents Then
				Return
			End If

			If (IsEmployeeDataLoaded) Then

				Dim page = e.Page

				If Not (m_BottomActiveTabPage Is Nothing) Then
					m_BottomActiveTabPage.Deactivate()
				End If

				If (Object.ReferenceEquals(page, xtabApplications)) Then
					IsDataValid = ucApplicationData.Activate(m_EmployeeNumber)
					m_BottomActiveTabPage = ucApplicationData

				ElseIf (Object.ReferenceEquals(page, xtabWork)) Then
					IsDataValid = ucCVLWorkData.ActivateCVLWork(m_CVLProfileID, m_WorkID)
					m_BottomActiveTabPage = ucCVLWorkData
				ElseIf (Object.ReferenceEquals(page, xtabEducation)) Then
					IsDataValid = ucCVLEducationData.ActivateCVLWork(m_CVLProfileID, m_EducationID)
					m_BottomActiveTabPage = ucCVLEducationData

				ElseIf (Object.ReferenceEquals(page, xtabAddInfo)) Then
					IsDataValid = ucCVLAdditionalData.ActivateCVLWork(m_CVLProfileID, m_AdditionalID)
					m_BottomActiveTabPage = ucCVLAdditionalData

				ElseIf (Object.ReferenceEquals(page, xtabPublication)) Then
					IsDataValid = ucCVLPublicationData.ActivateCVLWork(m_CVLProfileID, Nothing)
					m_BottomActiveTabPage = ucCVLPublicationData


				End If

			End If

		End Sub

		''' <summary>
		'''  Performs loading cvl work data.
		''' </summary>
		Private Function PerformCVLProfileDataWebservice(ByVal profileID As Integer?) As Boolean
			If profileID Is Nothing Then Return False

			Dim ws = New Internal.Automations.SPApplicationWebService.SPApplicationSoapClient
			ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
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

			bsiCreated.Caption = String.Format(" {0:f}", searchResult.CreatedOn)

			Return Not (searchResult Is Nothing)

		End Function

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

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_CVL_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_CVL_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_CVL_FORM_LOCATION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

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
					m_SettingsManager.WriteString(SettingKeys.SETTING_CVL_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_CVL_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_CVL_FORM_HEIGHT, Me.Height)

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

			For Each tabControl In m_ListOfTabControls
				tabControl.CleanUp()
			Next

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub

#End Region



	End Class

End Namespace
