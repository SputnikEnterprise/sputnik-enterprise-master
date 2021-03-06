
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects

Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Pdf
Imports SPProgUtility.CommonXmlUtility

Imports System.Text
Imports DevExpress.Skins
Imports DevExpress.UserSkins
Imports SPProgUtility
Imports System.Security.Cryptography
Imports System.IO
Imports SP.Infrastructure
Imports System.Collections.Specialized
Imports System.Net

Imports System.Threading.Tasks
Imports System.Threading
Imports SP.DatabaseAccess.Employee
Imports SPSSendMail.RichEditSendMail
Imports SP.Internal.Automations

Public Class ucApplicationDetail


#Region "private consts"

	Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx"

#End Region


#Region "Public Properties"

	'Public Property m_CurrentApplicationData As MainViewApplicationData
	Public Property CurrentApplicationID As Integer

#End Region


#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_AppDatabaseAccess As IAppDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	Private m_customerID As String
	Private m_EMailID As Integer?

	Private m_ApplicationUtilWebServiceUri As String
	Private m_CurrentApplicationData As MainViewApplicationData

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connString As String

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility
	Private m_advisor As AdvisorData

	Private m_mandant As Mandant


#End Region



#Region "constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		'm_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI
		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)

		m_CurrentApplicationData = New MainViewApplicationData

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
		WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

		m_connString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)
		m_AppDatabaseAccess = New AppDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)

		TranslateControls()
		Reset()

		m_SuppressUIEvents = False

	End Sub


#End Region


#Region "Public Methods"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		success = success AndAlso LoadCheckedAdvisorDropDownData()
		success = success AndAlso LoadAdvisorDropDownData()
		success = success AndAlso LoadBusinessbranchDropDownData()
		success = success AndAlso LoadApplicationData()

		m_SuppressUIEvents = suppressState

		Return success

	End Function

	Public Sub CleanUp()
		Dim success As Boolean = True


	End Sub


#End Region

	Sub TranslateControls()

		grpBewerbung.Text = m_Translate.GetSafeTranslationValue(grpBewerbung.Text)
		lblBemerkung.Text = m_Translate.GetSafeTranslationValue(lblBemerkung.Text)
		lblDatum.Text = m_Translate.GetSafeTranslationValue(lblDatum.Text)
		lblDurch.Text = m_Translate.GetSafeTranslationValue(lblDurch.Text)
		lblStatus.Text = m_Translate.GetSafeTranslationValue(lblStatus.Text)

		grpZuweisen.Text = m_Translate.GetSafeTranslationValue(grpZuweisen.Text)
		lblBeraterIn.Text = m_Translate.GetSafeTranslationValue(lblBeraterIn.Text)
		lblFiliale.Text = m_Translate.GetSafeTranslationValue(lblFiliale.Text)
		btnAssign.Text = m_Translate.GetSafeTranslationValue(btnAssign.Text)

		grpWeitereSchritte.Text = m_Translate.GetSafeTranslationValue(grpWeitereSchritte.Text)
		btnPropose.Text = m_Translate.GetSafeTranslationValue(btnPropose.Text)
		btnRejectWithoutMail.Text = m_Translate.GetSafeTranslationValue(btnRejectWithoutMail.Text)

	End Sub


	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		txtApplicationLabel.EditValue = Nothing
		txtComment.EditValue = Nothing
		deCreatedOn.EditValue = Nothing
		deChangedOn.EditValue = Nothing
		lueCheckedFrom.EditValue = Nothing
		LifecycleInfo.Text = Nothing

		lueAdvisor.EditValue = Nothing
		lueBusinessbranch.EditValue = Nothing

		txtComment.ReadOnly = True
		deCreatedOn.Enabled = False
		deChangedOn.Enabled = False
		lueCheckedFrom.Enabled = False

		ResetCheckedAdvisorDropDown()
		ResetAdvisorDropDown()
		ResetBusinessbranchDropDown()

		m_SuppressUIEvents = suppressState

	End Sub

	''' <summary>
	''' Resets the checkedfrom advisors drop down.
	''' </summary>
	Private Sub ResetCheckedAdvisorDropDown()

		lueCheckedFrom.Properties.ReadOnly = False
		lueCheckedFrom.Properties.DropDownRows = 20

		lueCheckedFrom.Properties.DisplayMember = "UserFullnameReversedWithoutComma"
		lueCheckedFrom.Properties.ValueMember = "UserFullnameReversedWithoutComma"

		Dim columns = lueCheckedFrom.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

		lueCheckedFrom.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCheckedFrom.Properties.SearchMode = SearchMode.AutoComplete
		lueCheckedFrom.Properties.AutoSearchColumnIndex = 1

		lueCheckedFrom.Properties.NullText = String.Empty
		lueCheckedFrom.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the advisors drop down.
	''' </summary>
	Private Sub ResetAdvisorDropDown()

		lueAdvisor.Properties.ReadOnly = False
		lueAdvisor.Properties.DropDownRows = 20

		lueAdvisor.Properties.DisplayMember = "UserFullnameReversedWithoutComma"
		lueAdvisor.Properties.ValueMember = "UserGuid"

		Dim columns = lueAdvisor.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

		lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
		lueAdvisor.Properties.AutoSearchColumnIndex = 1

		lueAdvisor.Properties.NullText = String.Empty
		lueAdvisor.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the Businessbranch drop down.
	''' </summary>
	Private Sub ResetBusinessbranchDropDown()

		lueBusinessbranch.Properties.DisplayMember = "MandantName1"
		lueBusinessbranch.Properties.ValueMember = "MandantNumber"

		lueBusinessbranch.Properties.Columns.Clear()
		lueBusinessbranch.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueBusinessbranch.Properties.ShowFooter = False
		lueBusinessbranch.Properties.DropDownRows = 10
		lueBusinessbranch.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBusinessbranch.Properties.SearchMode = SearchMode.AutoComplete
		lueBusinessbranch.Properties.AutoSearchColumnIndex = 0

		lueBusinessbranch.Properties.NullText = String.Empty
		lueBusinessbranch.EditValue = Nothing

	End Sub



	''' <summary>
	''' Loads the advisor drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadCheckedAdvisorDropDownData() As Boolean

		Dim advisors = m_CommonDatabaseAccess.LoadAllAdvisorsData()

		If advisors Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
		End If

		' Customer advisor
		lueCheckedFrom.Properties.DataSource = advisors
		lueCheckedFrom.Properties.ForceInitialize()

		Return Not advisors Is Nothing

	End Function

	''' <summary>
	''' Loads the advisor drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadAdvisorDropDownData() As Boolean

		Dim advisors = m_CommonDatabaseAccess.LoadAllAdvisorsData()

		If advisors Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
		End If

		' Employee advisor
		lueAdvisor.Properties.DataSource = advisors
		lueAdvisor.Properties.ForceInitialize()

		Return Not advisors Is Nothing

	End Function

	''' <summary>
	''' Loads the Businessbranch drop down data.
	''' </summary>
	Private Function LoadBusinessbranchDropDownData() As Boolean
		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueBusinessbranch.Properties.DataSource = mandantData
		lueBusinessbranch.Properties.ForceInitialize()

		Return mandantData IsNot Nothing
	End Function

	Private Function LoadApplicationData() As Boolean
		Dim success As Boolean = True

		m_CurrentApplicationData = m_AppDatabaseAccess.LoadAssignedApplicationDataForMainView(CurrentApplicationID)
		If m_CurrentApplicationData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerbung-Daten konnten nicht geladen werden."))
			Return False
		End If

		txtApplicationLabel.EditValue = m_CurrentApplicationData.ApplicationLabel
		txtComment.EditValue = m_CurrentApplicationData.Comment
		deCreatedOn.EditValue = Format(m_CurrentApplicationData.CreatedOn, "G")
		deChangedOn.EditValue = Format(m_CurrentApplicationData.CheckedOn, "G")
		lueCheckedFrom.EditValue = m_CurrentApplicationData.CheckedFrom
		LifecycleInfo.Text = m_Translate.GetSafeTranslationValue(m_CurrentApplicationData.ApplicationLifeCycelLabel)

		lueAdvisor.EditValue = m_CurrentApplicationData.Advisor
		Dim advisor = LoadAdvisorWithGuidData(m_CurrentApplicationData.Advisor)
		If Not advisor Is Nothing Then
			lueBusinessbranch.EditValue = advisor.UserMDNr
		End If

		Return success

	End Function


	''' <summary>
	''' Handles change of KST1.
	''' </summary>
	Private Sub OnlueAdvisor_EditValueChanged(sender As Object, e As EventArgs) Handles lueAdvisor.EditValueChanged

		If m_SuppressUIEvents OrElse lueAdvisor.EditValue Is Nothing Then
			Return
		End If

		Dim assignedAdvisor = LoadAdvisorWithGuidData(lueAdvisor.EditValue)
		If assignedAdvisor.Deactivated.GetValueOrDefault(False) Then
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Achtung: Der Benutzer kann nicht ermittelt werden. Möglicherweise wurde ber Benutzer gelöscht oder deaktiviert.<br>Bitte ändern Sie die Berater-Daten!"))
			lueAdvisor.EditValue = Nothing

			Return
		End If

		lueBusinessbranch.EditValue = assignedAdvisor.UserMDNr

	End Sub

	Private Function LoadAdvisorWithGuidData(ByVal advisorGuid As String) As AdvisorData

		m_advisor = m_CommonDatabaseAccess.LoadAdvisorDataforGivenGuid(advisorGuid)

		If m_advisor Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
			Return Nothing
		End If

		Return m_advisor

	End Function

	''' <summary>
	''' Handles click on open telephone button.
	''' </summary>
	Private Sub OntxtApplicationLabel_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtApplicationLabel.ButtonClick
		Dim phonenumber As String = sender.text
		If m_CurrentApplicationData Is Nothing Then Return

		m_CurrentApplicationData.ApplicationLabel = txtApplicationLabel.EditValue
		Dim success As Boolean = True
		success = success AndAlso m_AppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData(m_CurrentApplicationData)

		If success Then
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
			Return
		End If

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		LoadApplicationData()
		success = success AndAlso PerformUpdateAssignedApplicationJobWebservice(m_CurrentApplicationData.Customer_ID)

		m_SuppressUIEvents = suppressState

	End Sub

	Private Sub btnAssign_Click(sender As Object, e As EventArgs) Handles btnAssign.Click
		SaveApplicationData()
	End Sub

	Private Sub OnbtnPropose_Click(sender As Object, e As EventArgs) Handles btnPropose.Click

		If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 801) Then m_Logger.LogWarning("No rights...") : Exit Sub

		Try
			Dim pSetting As New SPProposeUtility.ClsProposeSetting With {.SelectedMANr = m_CurrentApplicationData.EmployeeID,
																								 .SelectedKDNr = m_CurrentApplicationData.Customernumber,
																								 .SelectedZHDNr = 0,
																								 .SelectedVakNr = m_CurrentApplicationData.VacancyNumber,
																								 .ApplicationNumber = m_CurrentApplicationData.ID,
																								 .SelectedProposeNr = Nothing}

			Dim frmPropose As New SPProposeUtility.frmPropose(m_InitializationData, pSetting)
			frmPropose.Show()
			frmPropose.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub OnbtnEmployee_Click(sender As Object, e As EventArgs) Handles btnEmployee.Click
		If Not ChangeApplicantToEmployee() Then Return
		Dim message As String = String.Empty

		message = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
		m_UtilityUI.ShowOKDialog(message, m_Translate.GetSafeTranslationValue("Bewerber als Kandidat speichern"))

	End Sub

	Private Sub btnInviteApplication_Click(sender As Object, e As EventArgs) Handles btnInviteApplication.Click
		ShowOKEMailNotificationData()
	End Sub

	Private Sub btnRejectWithoutMail_Click(sender As Object, e As EventArgs) Handles btnRejectWithoutMail.Click
		ShowCancelEMailNotificationData()
	End Sub

	Private Function ChangeApplicantToEmployee() As Boolean
		Dim success As Boolean = True
		' we can not know if all fields are ok!
		Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentApplicationData.EmployeeID)

		If employeeMasterData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
			Return False
		End If

		'success = success AndAlso m_UtilityUI.ShowYesNoDialog("Hiermit wird der Bewerber automatisch als Kandidat definiert. Sind Sie sicher?", "Bewerber als Kandidat übernehmen")
		'If Not success Then Return success

		employeeMasterData.ShowAsApplicant = If(m_InitializationData.UserData.UserNr = 1, Not employeeMasterData.ShowAsApplicant.GetValueOrDefault(False), False)
		employeeMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName

		success = success AndAlso m_EmployeeDatabaseAccess.UpdateApplicantToEmployee(employeeMasterData)

		Dim message As String = String.Empty

		If Not (success) Then
			message = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
			m_UtilityUI.ShowErrorDialog(message)

		End If


		Return success

	End Function

	Private Function SaveApplicationData() As Boolean
		Dim success As Boolean = True


		Dim assignedAdvisor = LoadAdvisorWithGuidData(lueAdvisor.EditValue)
		If assignedAdvisor.Deactivated.GetValueOrDefault(False) Then
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Achtung: Der Benutzer kann nicht ermittelt werden. Möglicherweise wurde ber Benutzer gelöscht oder deaktiviert.<br>Bitte ändern Sie die Berater-Daten!"))
			lueAdvisor.EditValue = Nothing

			Return False
		End If

		deCreatedOn.EditValue = Now.Date

		m_CurrentApplicationData.ApplicationLabel = txtApplicationLabel.EditValue
		m_CurrentApplicationData.Comment = txtComment.EditValue
		m_CurrentApplicationData.CheckedOn = deCreatedOn.EditValue
		m_CurrentApplicationData.CheckedFrom = m_InitializationData.UserData.UserFullName
		m_CurrentApplicationData.Advisor = assignedAdvisor.UserGuid
		m_CurrentApplicationData.BusinessBranch = lueBusinessbranch.Text
		m_CurrentApplicationData.ApplicationLifecycle = 3

		success = success AndAlso m_AppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData(m_CurrentApplicationData)
		success = success AndAlso PerformUpdateAssignedApplicationJobWebservice(m_CurrentApplicationData.Customer_ID)

		If Not (success) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Bewerbung konnte nicht zugewiesen werden!"))

			Return False
		End If

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		LoadApplicationData()


		m_SuppressUIEvents = suppressState
		m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihre Bewerbung konnte erfolgreich zugewiesen werden."))

		Return success

	End Function

	Private Function PerformUpdateAssignedApplicationJobWebservice(ByVal customerID As String) As Boolean

		Dim success As Boolean = True

#If DEBUG Then
		'Return True
		'm_NotificationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPNotification.asmx"
#End If

		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		Dim data = New SPApplicationWebService.ApplicationDataDTO
		data.ID = m_CurrentApplicationData.ApplicationID
		data.ApplicationLabel = m_CurrentApplicationData.ApplicationLabel
		data.Advisor = m_CurrentApplicationData.Advisor
		data.ApplicationLifecycle = m_CurrentApplicationData.ApplicationLifecycle
		data.BusinessBranch = m_CurrentApplicationData.BusinessBranch
		data.Comment = m_CurrentApplicationData.Comment


		' Read data over webservice
		success = success AndAlso webservice.UpdateAssignedApplicationData(customerID, data)


		Return success

	End Function

	''' <summary>
	''' shows notification mail-data for employee
	''' </summary>
	Private Sub ShowOKEMailNotificationData()
		If (m_CurrentApplicationData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

			Return
		End If

		Dim frmMail = New frmMailTpl(m_InitializationData)
		Try
			Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.ApplicationOKNotification, .ApplicationNumber = m_CurrentApplicationData.ID, .EmployeeNumber = m_CurrentApplicationData.EmployeeID}

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
		If (m_CurrentApplicationData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

			Return
		End If
		Dim frmMail = New frmMailTpl(m_InitializationData)
		Try
			Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.ApplicationCancelNotification, .ApplicationNumber = m_CurrentApplicationData.ID, .EmployeeNumber = m_CurrentApplicationData.EmployeeID}

			frmMail.PreselectionData = preselectionSetting
			frmMail.LoadData()

			frmMail.Show()
			frmMail.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Mitteilungsdaten konnten nicht geladen werden.{0}{1}", vbNewLine, ex.ToString()))
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Mitteilungsdaten konnten nicht geladen werden.")), "Cancel-Notification")

		End Try

	End Sub

End Class
