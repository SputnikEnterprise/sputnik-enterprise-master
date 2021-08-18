
Imports System.ComponentModel

Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Initialization
Imports System.Threading.Tasks
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.Threading
Imports SP.Internal.Automations.SPNotificationWebService
Imports DevExpress.XtraEditors.Repository
Imports SP.Main.Notify.ScanJobs
Imports DevExpress.XtraGrid.Views.Base
Imports System.Management
Imports SP.DatabaseAccess.ScanJob.DataObjects
Imports System.IO
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.Internal.Automations
Imports SP.Main.Notify.WOSDataTransfer

Namespace UI


	Public Class frmNotify



		Private Delegate Sub StartLoadingData()


#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
		Private Const DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPScanJobUtility.asmx"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES As String = "MD_{0}/Sonstiges"

		Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_VACANCY_GUID As String = "MD_{0}/Export/Vak_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_PROPOSE_GUID As String = "MD_{0}/Export/sendproposeattachmenttowos"

		Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
		Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
		Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"

#End Region


#Region "Private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_UpdateDatabaseAccess As NotifyDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_alarm As Threading.Timer

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml
		Private m_ProgSettingsXml As SettingsXml


		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MasterMandantData As IEnumerable(Of MasterMandantData)
		Private m_AllowedMandantData As IEnumerable(Of MasterMandantData)


		Private m_MandantData As Mandant
		Private m_connectionString As String
		Private m_SonstigesSetting As String

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_NotificationUtilWebServiceUri As String

		''' <summary>
		''' Service Uri of Sputnik scan job util webservice.
		''' </summary>
		Private m_ScanjobUtilWebServiceUri As String

		Private m_firstNotifyID As Integer?
		Private m_PopupMenu As DevExpress.XtraBars.PopupMenu

		Private gridDataList As New BindingList(Of NotifyData)
		Private m_ExitApplication As Boolean
		Private m_Timer As System.Timers.Timer
		Private m_TimerReport As System.Timers.Timer
		Private m_ChangeownReportForFinishingFlag As Boolean

		Private frmScanReport As frmReportDropIn
		Private frmScanCV As frmCVDropIn
		Private m_ReportUtility As ReportJobUtilities

		Private m_OriginalCustomerID As String
		Private m_IsProcessing As Boolean


		Private m_CurrentMDNumber As Integer
		Private m_CurrentMDString As String
		Private m_CurrentMDGuid As String

		Private m_CommonConfigFolder As String
		Private m_SputnikFileServer As String

		Private m_currentImportResultData As List(Of ImportResult)
		Private m_currentImportApplicantData As List(Of ImportResult)

		Private m_scanObj As ScanJobsUtilities
		Private m_ApplicantObj As ApplicantJobUtilities
		Private m_WosObj As WOSDataTransfer.SendScanJobTOWOS

		Private m_AllowedCustomerDataSendToWOS As Boolean
		Private m_AllowedVacancyDataSendToWOS As Boolean
		Private m_AllowedEmployeeDataSendToWOS As Boolean
		Private m_AllowedProposeDataSendToWOS As Boolean

		Private m_StagingMDGuid As String

#End Region


#Region "public property"
		''' <summary>
		''' Gets or sets the preselection data.
		''' </summary>
		Public Property PreselectionData As PreselectionData

#End Region


#Region "private properties"

		''' <summary>
		''' Gets the selected master mandant.
		''' </summary>
		Private ReadOnly Property SelectedMasterMandantData As MasterMandantData
			Get
				Dim grdView = TryCast(grdMasterMandant.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), MasterMandantData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected notification.
		''' </summary>
		Private ReadOnly Property SelectedNotificationData As NotifyViewData
			Get
				Dim grdView = TryCast(grdNotification.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), NotifyViewData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


#Region "Contructor"

		Sub New(ByVal _setting As InitializeClass)

			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI

			m_SuppressUIEvents = True

			InitializeComponent()

			WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Text
			WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

			m_ExitApplication = False

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_ScanjobUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI)



			Reset()
			TranslateControls()
			m_ChangeownReportForFinishingFlag = False
			m_currentImportResultData = New List(Of ImportResult)
			m_currentImportApplicantData = New List(Of ImportResult)

			m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)
			Dim SettingFile = ReadSettingFile()

			m_StagingMDGuid = "testguid"
			Dim intervalPeriod As Decimal = SettingFile.Notificationintervalperiode
			Dim intervalPeriodForReport As Decimal = SettingFile.Notificationintervalperiodeforreport

			Dim intervalTime As Decimal = If(intervalPeriod = 0, 0, Math.Min(Math.Max(intervalPeriod, 1), 60) * 60000D)
			Dim intervalReportTime As Decimal = If(intervalPeriodForReport = 0, 0, Math.Max(Math.Max(intervalPeriodForReport, 1), 30) * 60000D)  ' 300000D 

			m_SputnikFileServer = SettingFile.FileServerPath

			m_Logger.LogDebug(String.Format("intervalPeriod: {0:f0} | intervalTime: {1:f0} | intervalPeriodForReport: {2:f0} | intervalReportTime: {3:f0}",
																				intervalPeriod, intervalTime, intervalPeriodForReport, intervalReportTime))

			txtFileServer.EditValue = m_SputnikFileServer

			m_MasterMandantData = New BindingList(Of MasterMandantData)
			m_AllowedMandantData = New BindingList(Of MasterMandantData)

			Dim connectionString As String = String.Empty
			If Not String.IsNullOrWhiteSpace(m_SputnikFileServer) Then
				m_UpdateDatabaseAccess = New NotifyDatabaseAccess(connectionString, Language.German, m_SputnikFileServer)

				Dim success = LoadMasterMandantData()
				grdMasterMandant.DataSource = m_MasterMandantData
			End If

			XtraTabControl1.SelectedTabPage = xtabMasterMandant

			If intervalTime > 0 Then
				m_Timer = New System.Timers.Timer
				m_Timer.Interval = intervalTime
				AddHandler m_Timer.Elapsed, AddressOf RunTimer
			End If


			m_SuppressUIEvents = False

		End Sub

		Private Function InitialAssignedMandantData() As Boolean
			Dim result As Boolean = True

			Dim xmlSettingFile = m_MandantData.GetSelectedMDDataXMLFilename(m_CurrentMDNumber, Now.Year)
			m_Logger.LogDebug(String.Format("m_CurrentMDNumber: {0} | xmlsettingfile: {1}", m_CurrentMDNumber, xmlSettingFile))

			Try

				If File.Exists(xmlSettingFile) Then
					m_InitializationData = CreateInitialData(m_CurrentMDNumber, 1)
					If m_InitializationData Is Nothing Then Throw New Exception(String.Format("md {0} data could not be founded!!!", m_CurrentMDNumber))

					m_connectionString = m_CurrentMDString
					m_CommonDatabaseAccess = New CommonDatabaseAccess(m_CurrentMDString, m_InitializationData.UserData.UserLanguage)
					m_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentMDString, m_InitializationData.UserData.UserLanguage)

					m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_CurrentMDNumber, Now.Year))
					m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES, m_CurrentMDNumber)

					m_AllowedCustomerDataSendToWOS = Not String.IsNullOrWhiteSpace(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr)))
					m_AllowedVacancyDataSendToWOS = Not String.IsNullOrWhiteSpace(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_VACANCY_GUID, m_InitializationData.MDData.MDNr)))
					m_AllowedEmployeeDataSendToWOS = Not String.IsNullOrWhiteSpace(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr)))
					m_AllowedProposeDataSendToWOS = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_PROPOSE_GUID, m_InitializationData.MDData.MDNr)), False)


					m_PopupMenu = New DevExpress.XtraBars.PopupMenu
					m_SuppressUIEvents = True

					LoadDropDownData()
					m_OriginalCustomerID = m_CurrentMDGuid
				Else
					m_OriginalCustomerID = String.Empty
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("m_CurrentMDNumber: {0} |  was with error!!!{1}", m_CurrentMDNumber, ex.ToString))
				result = False

			Finally
				m_SuppressUIEvents = False

			End Try


			Return result

		End Function


#End Region


#Region "public Methods"

		Public Function LoadNotify() As Boolean
			Dim success As Boolean = True

			m_IsProcessing = False
			Try
				If Not m_Timer Is Nothing Then m_Timer.Stop()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()

				While True
					LoadNotificationData()

					success = success AndAlso LoadAutomatedScanJobData()
					success = success AndAlso LoadAutomatedApplicationJobData()
					success = success AndAlso LoadAutomatedProposeJobData()

					If Not success Then
						m_Logger.LogWarning("process could not be finished!!!")

						Exit While
					End If

					Exit While
				End While

				If Not m_Timer Is Nothing Then m_Timer.Enabled = True
				If Not m_TimerReport Is Nothing Then m_TimerReport.Enabled = True

			Catch ex As Exception
				If Not m_Timer Is Nothing Then m_Timer.Enabled = True
				If Not m_TimerReport Is Nothing Then m_TimerReport.Enabled = True

			End Try


			Return success

		End Function

		Public Sub HideNotifyContexMenu()
			If Not m_PopupMenu Is Nothing Then
				m_PopupMenu.HidePopup()
			End If
		End Sub

		Public Function IsProcessBusy() As Boolean
			Return m_IsProcessing
		End Function

		Public Function CleanUpProgNotify() As Boolean
			Dim success As Boolean = False

			HideNotifyContexMenu()
			m_ExitApplication = True

			If Not m_Timer Is Nothing Then m_Timer.Stop()
			If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()

			If m_IsProcessing Then
				Application.DoEvents()

			Else
				Me.Close()
				success = True

			End If


		End Function


#End Region


#Region "Form Events"

		Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

			e.Cancel = Not m_ExitApplication
			HideNotifyContexMenu()

			If Not e.Cancel Then

				If Not frmScanReport Is Nothing Then frmScanReport.Close()
				If Not frmScanCV Is Nothing Then frmScanCV.Close()

				If Not m_Timer Is Nothing Then m_Timer.Stop()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()
				NotifyIcon1.Dispose()

			Else
				Me.WindowState = FormWindowState.Minimized
			End If

		End Sub

		Private Sub OnfrmNotify_Load(sender As Object, e As EventArgs) Handles Me.Load
			Me.WindowState = FormWindowState.Normal
		End Sub

		Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
			Dim notifyBody As String = "Mandant: {1}{0}Die Suche wird gestartet."
			notifyBody = String.Format(m_Translate.GetSafeTranslationValue(notifyBody), vbNewLine, m_InitializationData.MDData.MDName)

			If Me.WindowState = FormWindowState.Minimized Then
				NotifyIcon1.Visible = True
				NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
				NotifyIcon1.BalloonTipTitle = m_Translate.GetSafeTranslationValue("Sputnik Enterprise Suite: Mitteilungen")
				NotifyIcon1.BalloonTipText = notifyBody
				NotifyIcon1.ShowBalloonTip(50000)

				ShowInTaskbar = False

			End If

		End Sub

#End Region

		Private Sub OnbtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			m_ExitApplication = True
			Me.Close()
		End Sub

		Private Sub OngrpSearch_CustomButtonClick(sender As Object, e As EventArgs) Handles grpSearch.CustomButtonClick

			'#If Not DEBUG Then
			LoadNotificationData()
			LoadAutomatedScanJobData()
			LoadAutomatedApplicationJobData()
			'#End If
			LoadAutomatedProposeJobData()

		End Sub

		Private Sub OnbtnUpdateCountryData_Click(sender As Object, e As EventArgs) Handles btnUpdateCountryData.Click

			PerformCustomerDataForUpdateJobs()

			If m_AllowedMandantData Is Nothing Then Return

			Try
				If m_IsProcessing Then Return
				m_IsProcessing = True

				If Not m_Timer Is Nothing Then m_Timer.Stop()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()

				For Each md In m_AllowedMandantData

					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id
#If DEBUG Then
					m_CurrentMDGuid = m_StagingMDGuid
#End If

					m_Logger.LogDebug(String.Format("btnUpdateCountryData: m_CurrentMDNumber {0} | is looking for mandant data...", m_CurrentMDNumber))
					Dim initSuccess As Boolean = InitialAssignedMandantData()

					RefreshEmployeeCountryDataViaWebService()
					RefreshCustomerCountryDataViaWebService()

				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

				If Not m_Timer Is Nothing Then m_Timer.Start()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Start()

				m_IsProcessing = False

			End Try


		End Sub

		Private Sub OnbtnUpdateGeoData_Click(sender As Object, e As EventArgs) Handles btnUpdateGeoData.Click

			PerformCustomerDataForUpdateJobs()

			If m_AllowedMandantData Is Nothing Then Return

			Try
				If m_IsProcessing Then Return
				m_IsProcessing = True

				If Not m_Timer Is Nothing Then m_Timer.Stop()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()

				For Each md In m_AllowedMandantData

					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id
#If DEBUG Then
					m_CurrentMDGuid = m_StagingMDGuid
#End If

					m_Logger.LogDebug(String.Format("btnUpdateGeoData: m_CurrentMDNumber {0} | is looking for mandant data...", m_CurrentMDNumber))
					Dim initSuccess As Boolean = InitialAssignedMandantData()

					RefreshEmployeeGeoDataViaWebService()
					RefreshCustomerGeoDataViaWebService()

				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

				If Not m_Timer Is Nothing Then m_Timer.Start()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Start()

				m_IsProcessing = False

			End Try


		End Sub

		Private Sub RunTimer(sender As Object, e As EventArgs)
			LoadData()
		End Sub

		Private Sub RunReportTimer(sender As Object, e As EventArgs)
			Return
			'LoadReportData()
		End Sub

		Private Sub txt_MDGuidForVacancyCheck_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txt_MDGuidForVacancyCheck.ButtonClick

			If e.Button.Index = 0 Then
				If Not txt_MDGuidForVacancyCheck Is Nothing AndAlso Not String.IsNullOrWhiteSpace(txt_MDGuidForVacancyCheck.EditValue) Then
					LoadAutomatedVacancyJobData()
				End If
			End If

		End Sub

		Private Function LoadAutomatedScanJobData() As Boolean
			Dim success As Boolean = True
			grpSearch.Text = String.Format("Last search: {0}", Now)

			PerformCustomerDataForScanJobsWebserviceCall()

			If m_AllowedMandantData Is Nothing Then Return success

			Try

				For Each md In m_AllowedMandantData
					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id

					m_Logger.LogDebug(String.Format("LoadAutomatedScanJobData: m_CurrentMDNumber {0} | is looking for mandant data...", m_CurrentMDNumber))
					Dim initSuccess As Boolean = InitialAssignedMandantData()

					If initSuccess Then
						If Not String.IsNullOrWhiteSpace(m_OriginalCustomerID) Then
							m_Logger.LogDebug(String.Format("LoadAutomatedScanJobData: m_CurrentMDNumber {0} is looking for scans...", m_CurrentMDNumber))

							SearchScanlistViaWebService()

						Else
							m_Logger.LogDebug(String.Format("LoadAutomatedScanJobData: m_CurrentMDNumber {0} ===>>> m_OriginalCustomerID is empty!", m_CurrentMDNumber))

						End If
					End If

				Next

				If Not m_currentImportResultData Is Nothing AndAlso m_currentImportResultData.Count > 0 Then XtraTabControl1.SelectedTabPage = xtabDocScan

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

			End Try

			Return success

		End Function

		Private Function LoadAutomatedApplicationJobData() As Boolean
			Dim success As Boolean = True
			grpSearch.Text = String.Format("Last search: {0}", Now)

			PerformCustomerDataForApplicationJobsWebserviceCall()

			If m_AllowedMandantData Is Nothing Then Return success

			Try

				For Each md In m_AllowedMandantData
					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id

#If DEBUG Then
					m_CurrentMDGuid = m_StagingMDGuid
#End If

					m_Logger.LogDebug(String.Format("LoadAutomatedApplicationJobData: m_CurrentMDNumber {0} | is looking for mandant data...", m_CurrentMDNumber))
					Dim initSuccess As Boolean = InitialAssignedMandantData()

					If initSuccess Then
						If Not String.IsNullOrWhiteSpace(m_OriginalCustomerID) Then
							m_Logger.LogDebug(String.Format("LoadAutomatedApplicationJobData: m_CurrentMDNumber {0} is looking for applications...", m_CurrentMDNumber))

							SearchApplicantlistViaWebService()

						Else
							m_Logger.LogDebug(String.Format("LoadAutomatedApplicationJobData: m_CurrentMDNumber {0} ===>>> m_OriginalCustomerID is empty!", m_CurrentMDNumber))

						End If
					End If

				Next

				If Not m_currentImportApplicantData Is Nothing AndAlso m_currentImportApplicantData.Count > 0 Then XtraTabControl1.SelectedTabPage = xtabApplicant

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

			End Try

			Return success

		End Function


		Private Function LoadAutomatedProposeJobData() As Boolean
			Dim success As Boolean = True
			grpSearch.Text = String.Format("Last search: {0}", Now)

			If m_MasterMandantData Is Nothing OrElse m_MasterMandantData.Count = 0 Then Return success

			Try

				For Each md In m_MasterMandantData
					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id
#If DEBUG Then
					m_CurrentMDGuid = m_StagingMDGuid
#End If

					Dim initSuccess As Boolean = InitialAssignedMandantData()
					If Not m_AllowedProposeDataSendToWOS Then Continue For

					If initSuccess Then
						If Not String.IsNullOrWhiteSpace(m_OriginalCustomerID) Then
							m_Logger.LogDebug(String.Format("LoadAutomatedProposeJobData: m_CurrentMDNumber {0} is looking for vacancy...", m_CurrentMDNumber))

							PerformProposeListWebserviceCallAsync()

						Else
							m_Logger.LogDebug(String.Format("LoadAutomatedProposeJobData: m_CurrentMDNumber {0} ===>>> m_OriginalCustomerID is empty!", m_CurrentMDNumber))

						End If
					End If

				Next

				If Not m_currentImportResultData Is Nothing AndAlso m_currentImportResultData.Count > 0 Then XtraTabControl1.SelectedTabPage = xtabDocScan

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

			End Try

			Return success

		End Function


		''' <summary>
		''' Search for employee country over web service.
		''' </summary>
		Private Sub RefreshEmployeeCountryDataViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			btnUpdateCountryData.Enabled = False

			Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingEmployeeCountryDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingEmployeeCountryDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs employee country asynchronous.
		''' </summary>
		Private Function PerformUpdatingEmployeeCountryDataWebserviceCallAsync() As Boolean

			Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
			Dim result = baseTableData.UpdateEmployeeCountryData()

			Return result

		End Function

		''' <summary>
		''' Finish employee country web service call.
		''' </summary>
		Private Sub FinishUpdatingEmployeeCountryDataWebserviceCallTask(ByVal t As Task(Of Boolean))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True
						Dim result = t.Result

						If result Then
							m_Logger.LogInfo(m_Translate.GetSafeTranslationValue("Ihre Kandidaten-Länder wurden erfolgreich aktualisiert."))

						Else
							m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kandidaten-Länder wurden nicht erfolgreich aktualisiert."))

						End If

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(t.Exception.ToString())
						m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kandidaten-Länder wurden nicht erfolgreich aktualisiert."))

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			btnUpdateCountryData.Enabled = True

		End Sub

		''' <summary>
		''' Search for customer country over web service.
		''' </summary>
		Private Sub RefreshCustomerCountryDataViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			btnUpdateCountryData.Enabled = False

			Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingCustomerCountryDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingCustomerCountryDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs customer country asynchronous.
		''' </summary>
		Private Function PerformUpdatingCustomerCountryDataWebserviceCallAsync() As Boolean

			Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
			Dim result = baseTableData.UpdateCustomerCountryData()

			Return result

		End Function

		''' <summary>
		''' Finish customer country web service call.
		''' </summary>
		Private Sub FinishUpdatingCustomerCountryDataWebserviceCallTask(ByVal t As Task(Of Boolean))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True
						Dim result = t.Result

						If result Then
							m_Logger.LogInfo(m_Translate.GetSafeTranslationValue("Ihre Kunden-Länder wurden erfolgreich aktualisiert."))

						Else
							m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kunden-Länder wurden nicht erfolgreich aktualisiert."))

						End If

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(t.Exception.ToString())
						m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kunden-Länder wurden nicht erfolgreich aktualisiert."))

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			btnUpdateCountryData.Enabled = True

		End Sub


#Region "geo data"

		''' <summary>
		''' Search for employee geo over web service.
		''' </summary>
		Private Sub RefreshEmployeeGeoDataViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			btnUpdateGeoData.Enabled = False

			Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingEmployeeGeoDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingEmployeeGeoDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs employee geo asynchronous.
		''' </summary>
		Private Function PerformUpdatingEmployeeGeoDataWebserviceCallAsync() As Boolean

			Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
			Dim result = baseTableData.UpdateEmployeeGeoData("CH")

			Return result

		End Function

		''' <summary>
		''' Finish employee geo web service call.
		''' </summary>
		Private Sub FinishUpdatingEmployeeGeoDataWebserviceCallTask(ByVal t As Task(Of Boolean))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True
						Dim result = t.Result

						If result Then
							m_Logger.LogInfo(m_Translate.GetSafeTranslationValue("Ihre Kandidaten Geo-Koordinaten wurden erfolgreich aktualisiert."))

						Else
							m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kandidaten Geo-Koordinaten wurden nicht erfolgreich aktualisiert."))

						End If

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(t.Exception.ToString())
						m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kandidaten Geo-Koordinaten wurden nicht erfolgreich aktualisiert."))

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			btnUpdateGeoData.Enabled = True

		End Sub

		''' <summary>
		''' Search for customer geo over web service.
		''' </summary>
		Private Sub RefreshCustomerGeoDataViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			btnUpdateGeoData.Enabled = False

			Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingCustomerGeoDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingCustomerGeoDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs customer geo asynchronous.
		''' </summary>
		Private Function PerformUpdatingCustomerGeoDataWebserviceCallAsync() As Boolean

			Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
			Dim result = baseTableData.UpdateCustomerGeoData("CH")

			Return result

		End Function

		''' <summary>
		''' Finish customer geo web service call.
		''' </summary>
		Private Sub FinishUpdatingCustomerGeoDataWebserviceCallTask(ByVal t As Task(Of Boolean))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True
						Dim result = t.Result

						If result Then
							m_Logger.LogInfo(m_Translate.GetSafeTranslationValue("Ihre Kunden Geo-Koordinaten wurden erfolgreich aktualisiert."))

						Else
							m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kunden Geo-Koordinaten wurden nicht erfolgreich aktualisiert."))

						End If

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(t.Exception.ToString())
						m_Logger.LogError(m_Translate.GetSafeTranslationValue("Ihre Kunden Geo-Koordinaten wurden nicht erfolgreich aktualisiert."))

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			btnUpdateGeoData.Enabled = True

		End Sub


#End Region


		Private Sub CreateMenuPopup()
			Dim barMgr As New DevExpress.XtraBars.BarManager
			Dim liMnu As New List(Of String) From {"Programmfenster öffnen#0",
																							 "Rapport Drop-In#2",
																							 "CV Drop-In#3",
																							 "Notify-Daten abrufen#4",
																							 "_Programm beenden#1"}

			Try
				For i As Integer = 0 To liMnu.Count - 1
					Dim myValue As String() = liMnu(i).Split(CChar("#"))

					If myValue(0).ToString <> String.Empty Then
						m_PopupMenu.Manager = barMgr

						Dim itm As New DevExpress.XtraBars.BarButtonItem

						itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString.Replace("_", ""))
						itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

						'm_PopupMenu.AddItem(itm)
						If myValue(0).StartsWith("_") Then m_PopupMenu.AddItem(itm).BeginGroup = True Else m_PopupMenu.AddItem(itm)
						AddHandler itm.ItemClick, AddressOf GetPopupMnu

					End If

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		Private Sub GetPopupMnu(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

			Dim mnuItem = CType(CShort(e.Item.Name), ContextMnuValue)
			Select Case mnuItem
				Case 0
					If m_InitializationData.UserData.UserNr <> 1 Then
						Dim msg = "Sie können das Hauptprogramm nicht öffnen."
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))

						Return
					End If
					ShowInTaskbar = True
					Me.WindowState = FormWindowState.Normal
					NotifyIcon1.Visible = False

				Case 1
					m_ExitApplication = True
					Me.Close()

				Case 2
					LoadReportDropIn()

				Case 3
					LoadCVDropIn()

				Case 4
					LoadData()

				Case 5
					'LoadApplicantJobData()


				Case Else
					m_Logger.LogError("menu data could not be loaded! " & m_InitializationData.MDData.MDDbConn)

			End Select

		End Sub

		Private Sub LoadReportDropIn()
			If frmScanReport Is Nothing OrElse frmScanReport.IsDisposed Then frmScanReport = New frmReportDropIn(m_InitializationData)

			frmScanReport.Show()
			frmScanReport.BringToFront()

		End Sub

		Private Sub LoadCVDropIn()
			If frmScanCV Is Nothing OrElse frmScanCV.IsDisposed Then frmScanCV = New frmCVDropIn(m_InitializationData)

			frmScanCV.Show()
			frmScanCV.BringToFront()

		End Sub





#Region "notifing"

		Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
			ShowInTaskbar = True
			Me.WindowState = FormWindowState.Normal
			NotifyIcon1.Visible = False
		End Sub

		Private Sub OnNotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick
			'If e.Button = MouseButtons.Right AndAlso IsHandleCreated And sender Is NotifyIcon1 Then
			If Not m_PopupMenu Is Nothing AndAlso e.Button = MouseButtons.Right AndAlso sender Is NotifyIcon1 Then
				m_PopupMenu.ShowPopup(Control.MousePosition)
			End If
		End Sub


#End Region


		Private Sub InitGrid()


		End Sub


		Private Sub Reset()

			ResetMandantenDropDown()
			ResetAdvisorDropDown()

			ResetDetails()
			ResetMasterMandantGrid()
			ResetNotificationGrid()
			ResetScanJobGrid()
			ResetApplicantGrid()

		End Sub

		Private Sub ResetDetails()

			lblNotifyHeader.Text = String.Empty
			lblNotifyArt.Text = String.Empty
			reNotifyComments.HtmlText = Nothing

			lblCreatedOn.Text = String.Empty
			lblCheckedOn.Text = String.Empty

		End Sub

		''' <summary>
		'''  Translate controls
		''' </summary>
		Private Sub TranslateControls()

			Text = m_Translate.GetSafeTranslationValue(Text)

		End Sub




#Region "reseting grids and dropdowns"


		''' <summary>
		''' Resets master mandant grid.
		''' </summary>
		Private Sub ResetMasterMandantGrid()

			gvMasterMandant.OptionsView.ShowIndicator = False
			gvMasterMandant.OptionsView.ShowAutoFilterRow = True
			gvMasterMandant.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvMasterMandant.OptionsView.ShowFooter = False
			gvMasterMandant.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			Dim edit As RepositoryItemButtonEdit = New RepositoryItemButtonEdit
			edit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			grdMasterMandant.RepositoryItems.Add(edit)

			gvMasterMandant.Columns.Clear()


			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnID.OptionsColumn.AllowEdit = True
			columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = True
			columnID.Width = 50
			gvMasterMandant.Columns.Add(columnID)

			Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMDNr.OptionsColumn.AllowEdit = False
			columnMDNr.Caption = m_Translate.GetSafeTranslationValue("MDNr")
			columnMDNr.Name = "MDNr"
			columnMDNr.FieldName = "MDNr"
			columnMDNr.Visible = True
			columnMDNr.Width = 80
			columnMDNr.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnMDNr)

			Dim columnMDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMDName.OptionsColumn.AllowEdit = False
			columnMDName.Caption = m_Translate.GetSafeTranslationValue("MDName")
			columnMDName.Name = "MDName"
			columnMDName.FieldName = "MDName"
			columnMDName.Visible = True
			columnMDName.Width = 80
			columnMDName.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnMDName)

			Dim columnMDPath As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDPath.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMDPath.OptionsColumn.AllowEdit = False
			columnMDPath.Caption = m_Translate.GetSafeTranslationValue("MDPath")
			columnMDPath.Name = "MDPath"
			columnMDPath.FieldName = "MDPath"
			columnMDPath.Visible = True
			columnMDPath.Width = 80
			columnMDPath.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnMDPath)

			Dim columnDbName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDbName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDbName.OptionsColumn.AllowEdit = False
			columnDbName.Caption = m_Translate.GetSafeTranslationValue("DbName")
			columnDbName.Name = "DbName"
			columnDbName.FieldName = "DbName"
			columnDbName.Visible = True
			columnDbName.Width = 80
			columnDbName.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnDbName)

			Dim columnDbConnectionstr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDbConnectionstr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDbConnectionstr.OptionsColumn.AllowEdit = False
			columnDbConnectionstr.Caption = m_Translate.GetSafeTranslationValue("DbConnectionstr")
			columnDbConnectionstr.Name = "DbConnectionstr"
			columnDbConnectionstr.FieldName = "DbConnectionstr"
			columnDbConnectionstr.Visible = True
			columnDbConnectionstr.Width = 80
			columnDbConnectionstr.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnDbConnectionstr)

			Dim columnDbServerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDbServerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDbServerName.OptionsColumn.AllowEdit = False
			columnDbServerName.Caption = m_Translate.GetSafeTranslationValue("DbServerName")
			columnDbServerName.Name = "DbServerName"
			columnDbServerName.FieldName = "DbServerName"
			columnDbServerName.Visible = True
			columnDbServerName.Width = 80
			columnDbServerName.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnDbServerName)

			Dim columnCustomer_id As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_id.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_id.OptionsColumn.AllowEdit = False
			columnCustomer_id.Caption = m_Translate.GetSafeTranslationValue("Customer_id")
			columnCustomer_id.Name = "Customer_id"
			columnCustomer_id.FieldName = "Customer_id"
			columnCustomer_id.Visible = True
			columnCustomer_id.Width = 80
			columnCustomer_id.ColumnEdit = edit
			gvMasterMandant.Columns.Add(columnCustomer_id)

			Dim columnFileServerPath As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFileServerPath.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFileServerPath.OptionsColumn.AllowEdit = False
			columnFileServerPath.Caption = m_Translate.GetSafeTranslationValue("FileServerPath")
			columnFileServerPath.Name = "FileServerPath"
			columnFileServerPath.FieldName = "FileServerPath"
			columnFileServerPath.Visible = True
			columnFileServerPath.Width = 60
			gvMasterMandant.Columns.Add(columnFileServerPath)

			Dim columnFileServerRootPath As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFileServerRootPath.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFileServerRootPath.OptionsColumn.AllowEdit = False
			columnFileServerRootPath.Caption = m_Translate.GetSafeTranslationValue("FileServerRootPath")
			columnFileServerRootPath.Name = "FileServerRootPath"
			columnFileServerRootPath.FieldName = "FileServerRootPath"
			columnFileServerRootPath.Visible = True
			columnFileServerRootPath.Width = 60
			gvMasterMandant.Columns.Add(columnFileServerRootPath)


			grdMasterMandant.DataSource = Nothing

		End Sub


		''' <summary>
		''' Resets the Mandanten drop down.
		''' </summary>
		Private Sub ResetMandantenDropDown()

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantGuid"

			lueMandant.Properties.Columns.Clear()
			lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																							 .Width = 100,
																							 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

			lueMandant.Properties.ShowHeader = False
			lueMandant.Properties.ShowFooter = False

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

			lueAdvisor.Properties.DisplayMember = "FirstName_LastName"
			lueAdvisor.Properties.ValueMember = "UserNumber"

			Dim columns = lueAdvisor.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("KST", 0))
			columns.Add(New LookUpColumnInfo("LastName_FirstName", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

			lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
			lueAdvisor.Properties.AutoSearchColumnIndex = 1

			lueAdvisor.Properties.NullText = String.Empty
			lueAdvisor.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets Notification grid.
		''' </summary>
		Private Sub ResetNotificationGrid()

			gvNotification.OptionsView.ShowIndicator = False
			gvNotification.OptionsView.ShowAutoFilterRow = True
			gvNotification.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvNotification.OptionsView.ShowFooter = False
			gvNotification.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			Dim edit As RepositoryItemButtonEdit = New RepositoryItemButtonEdit
			edit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			grdNotification.RepositoryItems.Add(edit)

			gvNotification.Columns.Clear()


			Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSelectedRec.OptionsColumn.AllowEdit = True
			columnSelectedRec.Caption = " "
			columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnSelectedRec.Name = "Checked"
			columnSelectedRec.FieldName = "Checked"
			columnSelectedRec.Visible = True
			columnSelectedRec.Width = 10
			gvNotification.Columns.Add(columnSelectedRec)

			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			columnID.Width = 50
			gvNotification.Columns.Add(columnID)

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Mandant")
			columnCustomer_ID.Name = "Customer_ID"
			columnCustomer_ID.FieldName = "Customer_ID"
			columnCustomer_ID.Width = 60
			columnCustomer_ID.Visible = False
			gvNotification.Columns.Add(columnCustomer_ID)

			Dim columnNotifyHeader As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNotifyHeader.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNotifyHeader.OptionsColumn.AllowEdit = False
			columnNotifyHeader.Caption = m_Translate.GetSafeTranslationValue("Betreff")
			columnNotifyHeader.Name = "NotifyHeader"
			columnNotifyHeader.FieldName = "NotifyHeader"
			columnNotifyHeader.Visible = True
			columnNotifyHeader.Width = 50
			columnNotifyHeader.ColumnEdit = edit
			gvNotification.Columns.Add(columnNotifyHeader)

			Dim columnNotifyComments As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNotifyComments.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNotifyComments.OptionsColumn.AllowEdit = False
			columnNotifyComments.Caption = m_Translate.GetSafeTranslationValue("Info")
			columnNotifyComments.Name = "NotifyComments"
			columnNotifyComments.FieldName = "NotifyComments"
			columnNotifyComments.Visible = True
			columnNotifyComments.Width = 80
			columnNotifyComments.ColumnEdit = edit
			gvNotification.Columns.Add(columnNotifyComments)

			Dim columnWhoCreated_FullData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWhoCreated_FullData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWhoCreated_FullData.OptionsColumn.AllowEdit = False
			columnWhoCreated_FullData.Caption = m_Translate.GetSafeTranslationValue("Erstellt")
			columnWhoCreated_FullData.Name = "WhoCreated_FullData"
			columnWhoCreated_FullData.FieldName = "WhoCreated_FullData"
			columnWhoCreated_FullData.Visible = True
			columnWhoCreated_FullData.Width = 60
			gvNotification.Columns.Add(columnWhoCreated_FullData)

			Dim columnWhoChecked_FullData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWhoChecked_FullData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWhoChecked_FullData.OptionsColumn.AllowEdit = False
			columnWhoChecked_FullData.Caption = m_Translate.GetSafeTranslationValue("Gesehen")
			columnWhoChecked_FullData.Name = "WhoChecked_FullData"
			columnWhoChecked_FullData.FieldName = "WhoChecked_FullData"
			columnWhoChecked_FullData.Visible = True
			columnWhoChecked_FullData.Width = 60
			gvNotification.Columns.Add(columnWhoChecked_FullData)


			grdNotification.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets ScanJobs grid.
		''' </summary>
		Private Sub ResetScanJobGrid()

			gvScanJobs.OptionsView.ShowIndicator = False
			gvScanJobs.OptionsView.ShowAutoFilterRow = True
			gvScanJobs.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvScanJobs.OptionsView.ShowFooter = False
			gvScanJobs.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			Dim edit As RepositoryItemButtonEdit = New RepositoryItemButtonEdit
			edit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			grdScanJobs.RepositoryItems.Add(edit)

			gvScanJobs.Columns.Clear()


			Dim columnModulNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnModulNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnModulNumber.OptionsColumn.AllowEdit = False
			columnModulNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnModulNumber.Caption = m_Translate.GetSafeTranslationValue("ModulNumber")
			columnModulNumber.Name = "ModulNumber"
			columnModulNumber.FieldName = "ModulNumber"
			columnModulNumber.Visible = True
			columnModulNumber.Width = 50
			columnModulNumber.ColumnEdit = edit
			gvScanJobs.Columns.Add(columnModulNumber)

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Customer_ID")
			columnCustomer_ID.Name = "Customer_ID"
			columnCustomer_ID.FieldName = "Customer_ID"
			columnCustomer_ID.Visible = True
			columnCustomer_ID.Width = 50
			gvScanJobs.Columns.Add(columnCustomer_ID)

			Dim columnIsValid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsValid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsValid.OptionsColumn.AllowEdit = False
			columnIsValid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsValid.Caption = m_Translate.GetSafeTranslationValue("ModulName")
			columnIsValid.Name = "ModulName"
			columnIsValid.FieldName = "ModulName"
			columnIsValid.Visible = True
			columnIsValid.Width = 100
			'columnIsValid.ColumnEdit = edit
			gvScanJobs.Columns.Add(columnIsValid)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			columnCreatedOn.Width = 60
			gvScanJobs.Columns.Add(columnCreatedOn)


			grdScanJobs.DataSource = Nothing

		End Sub

		Private Sub ResetApplicantGrid()

			gvApplicant.OptionsView.ShowIndicator = False
			gvApplicant.OptionsView.ShowAutoFilterRow = True
			gvApplicant.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvApplicant.OptionsView.ShowFooter = False
			gvApplicant.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			Dim edit As RepositoryItemButtonEdit = New RepositoryItemButtonEdit
			edit.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			grdApplicant.RepositoryItems.Add(edit)

			gvApplicant.Columns.Clear()


			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = m_Translate.GetSafeTranslationValue("Customer_ID")
			columnCustomer_ID.Name = "Customer_ID"
			columnCustomer_ID.FieldName = "Customer_ID"
			columnCustomer_ID.Visible = True
			columnCustomer_ID.Width = 50
			gvApplicant.Columns.Add(columnCustomer_ID)

			Dim columnModulNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnModulNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnModulNumber.OptionsColumn.AllowEdit = False
			columnModulNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnModulNumber.Caption = m_Translate.GetSafeTranslationValue("Applicant Number")
			columnModulNumber.Name = "ModulNumber"
			columnModulNumber.FieldName = "ModulNumber"
			columnModulNumber.Visible = True
			columnModulNumber.Width = 50
			columnModulNumber.ColumnEdit = edit
			gvApplicant.Columns.Add(columnModulNumber)

			Dim columnIsValid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsValid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsValid.OptionsColumn.AllowEdit = False
			columnIsValid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsValid.Caption = m_Translate.GetSafeTranslationValue("Applicant")
			columnIsValid.Name = "ModulName"
			columnIsValid.FieldName = "ModulName"
			columnIsValid.Visible = True
			columnIsValid.Width = 100
			'columnIsValid.ColumnEdit = edit
			gvApplicant.Columns.Add(columnIsValid)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			columnCreatedOn.Width = 60
			gvApplicant.Columns.Add(columnCreatedOn)


			grdApplicant.DataSource = Nothing

		End Sub

#End Region


#Region "loading drop down data"

		''' <summary>
		''' Load Mandanten drop down
		''' </summary>
		''' <remarks></remarks>
		Private Function LoadMandantenDropDown() As Boolean
			Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

			If Data Is Nothing Then
				m_Logger.LogError("md data could not be founded! " & m_InitializationData.MDData.MDDbConn)
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				Return False
			End If
			lueMandant.Properties.DataSource = Data
			lueMandant.Properties.ForceInitialize()

			Return Not Data Is Nothing

		End Function

		''' <summary>
		''' Loads the advisor drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorDropDown() As Boolean

			Dim userDataList = m_CommonDatabaseAccess.LoadAdvisorData()

			Dim advisorViewDataList As New List(Of AdvisorViewData)

			If Not userDataList Is Nothing Then
				For Each userData In userDataList
					Dim advisorViewData As AdvisorViewData = New AdvisorViewData
					advisorViewData.KST = userData.KST
					advisorViewData.FristName = userData.Firstname
					advisorViewData.LastName = userData.Lastname
					advisorViewData.UserNumber = userData.UserNumber

					advisorViewDataList.Add(advisorViewData)
				Next
			Else
				m_Logger.LogError("user data could not be loaded! " & m_InitializationData.MDData.MDDbConn)
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
			End If

			lueAdvisor.Properties.DataSource = advisorViewDataList
			lueAdvisor.Properties.ForceInitialize()

			Return Not userDataList Is Nothing
		End Function


#End Region


		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

			If hasPreselectionData Then

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

				' ---Mandant---
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of SP.DatabaseAccess.Common.DataObjects.MandantData))

					If manantDataList.Any(Function(md) md.MandantGuid = PreselectionData.CustomerID) Then

						' Mandant is required
						lueMandant.EditValue = PreselectionData.CustomerID

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						m_SuppressUIEvents = supressUIEventState
						Return
					End If

				End If

				If Not lueMandant.Properties.DataSource Is Nothing Then
					lueAdvisor.EditValue = m_InitializationData.UserData.UserKST
				End If

				m_SuppressUIEvents = supressUIEventState

			Else
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of SP.DatabaseAccess.Common.DataObjects.MandantData))

					If manantDataList.Any(Function(md) md.MandantGuid = m_InitializationData.MDData.MDGuid) Then

						' Mandant is required
						lueMandant.EditValue = m_InitializationData.MDData.MDGuid

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						Return
					End If

				End If

			End If

		End Sub


#Region "private methodes"

		Sub OngvMasterMandant_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvMasterMandant.RowCellClick

			Dim column = e.Column
			Dim dataRow = gvMasterMandant.GetRow(e.RowHandle)
			If dataRow Is Nothing Then
				Return
			Else
				LoadAssignedMasterMandantDetails()

			End If

		End Sub

		Private Function LoadMasterMandantData() As Boolean
			Dim success As Boolean = True

			m_MasterMandantData = m_UpdateDatabaseAccess.LoadUpdateMandantData
			If m_MasterMandantData Is Nothing Then
				m_Logger.LogWarning("could not find any data from mandant table in dbselect database!")

				Return False

			End If


			Return (Not m_MasterMandantData Is Nothing)

		End Function

		''' <summary>
		''' Focuses a master mandant.
		''' </summary>
		Private Sub FocusMasterMandant(ByVal mdID As Integer?)

			If Not mdID.HasValue Then Return
			If Not grdMasterMandant.DataSource Is Nothing Then

				Dim mandantViewData = CType(gvMasterMandant.DataSource, BindingList(Of MasterMandantData))

				Dim index = mandantViewData.ToList().FindIndex(Function(data) data.ID = mdID)

				m_SuppressUIEvents = True
				Dim rowHandle = gvMasterMandant.GetRowHandle(index)
				gvMasterMandant.FocusedRowHandle = rowHandle

				LoadAssignedMasterMandantDetails()


				m_SuppressUIEvents = False
			End If

		End Sub

		Private Sub LoadAssignedMasterMandantDetails()

			Dim data = SelectedMasterMandantData
			If data Is Nothing Then
				m_Logger.LogError("master md data could not be loaded! " & m_InitializationData.MDData.MDDbConn)

				Return
			End If

			m_CurrentMDNumber = data.MDNr
			m_CurrentMDString = data.DbConnectionstr
			m_CurrentMDGuid = data.Customer_id

			InitialAssignedMandantData()

		End Sub


		Private Sub LoadData()

			Try

				If Me.InvokeRequired = True Then
					Me.Invoke(New StartLoadingData(AddressOf LoadData))
				Else

#If DEBUG Then
					'Return
#End If
					If m_IsProcessing Then Return
					m_IsProcessing = True

					If Not m_Timer Is Nothing Then m_Timer.Stop()
					If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()

					Dim success As Boolean = True

					'success = success AndAlso LoadNotificationData()
					'#If Not DEBUG Then
					'					Return
					'#End If
					success = success AndAlso LoadAutomatedScanJobData()
					success = success AndAlso LoadAutomatedApplicationJobData()
					success = success AndAlso LoadAutomatedProposeJobData()

					Try
						If Not m_InitializationData Is Nothing Then
							Dim headerText As String = "Sputnik Enterpreise: [Mitteilungen]"
							Dim detailText As String = "Mandant: {1}{0}Die Suche wurde gestartet."
							detailText = String.Format(m_Translate.GetSafeTranslationValue(detailText), vbNewLine, m_InitializationData.MDData.MDName)

							detailText = "Die Suche wurde beendet:"
							detailText &= "{1} Mitteilungen wurden aufgelistet.{0}"
							detailText &= "{2} Scan-Mitteilungen wurden aufgelistet.{0}"
							detailText = String.Format(m_Translate.GetSafeTranslationValue(detailText), vbNewLine, gvNotification.RowCount, gvScanJobs.RowCount)
							If gvNotification.RowCount > 0 Then
								Dim info As DevExpress.XtraBars.Alerter.AlertInfo = New DevExpress.XtraBars.Alerter.AlertInfo(headerText, detailText, navbarImageCollection.Images("information_32x32.png"))
								AlertControl1.ShowCloseButton = False
								AlertControl1.Show(Me, info)
							End If
						End If

					Catch ex As Exception

					End Try

					If Not m_Timer Is Nothing Then m_Timer.Start()
					If Not m_TimerReport Is Nothing Then m_TimerReport.Start()

					m_IsProcessing = False

				End If

			Catch ex As Exception
				m_IsProcessing = False

				If Not m_Timer Is Nothing Then m_Timer.Start()
				If Not m_TimerReport Is Nothing Then m_TimerReport.Start()

			End Try

		End Sub


		Private Sub LoadReportData()

			Try

				If Me.InvokeRequired = True Then
					Me.Invoke(New StartLoadingData(AddressOf LoadReportData))
				Else

#If DEBUG Then
					'Return
#End If
					If m_IsProcessing Then Return
					m_IsProcessing = True
					If Not m_TimerReport Is Nothing Then m_TimerReport.Stop()

					LoadFinishingReportListData()

					If Not m_TimerReport Is Nothing Then m_TimerReport.Start()
					m_IsProcessing = False

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

#End Region


#Region "Grid for loading notifications"

		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadMandantenDropDown()
			success = success AndAlso LoadAdvisorDropDown()

			'If Not PreselectionData Is Nothing Then PreselectData()

		End Function

		Private Function LoadNotificationData() As Boolean
			Dim success As Boolean = True
			grpSearch.Text = String.Format("Last search: {0}", Now)

			If m_MasterMandantData Is Nothing OrElse m_MasterMandantData.Count = 0 Then Return success

			Try

				For Each md In m_MasterMandantData
					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id

#If DEBUG Then
					m_CurrentMDGuid = m_StagingMDGuid
#End If

					'If txt_MDGuidForVacancyCheck.EditValue <> m_CurrentMDGuid Then Continue For
					Dim initSuccess As Boolean = InitialAssignedMandantData()

					If initSuccess Then
						If Not String.IsNullOrWhiteSpace(m_OriginalCustomerID) Then
							m_Logger.LogDebug(String.Format("LoadNotificationData: m_CurrentMDNumber {0} is looking for vacancy...", m_CurrentMDNumber))

							SearchNotificationlistViaWebService()

						Else
							m_Logger.LogDebug(String.Format("LoadNotificationData: m_CurrentMDNumber {0} ===>>> m_OriginalCustomerID is empty!", m_CurrentMDNumber))

						End If
					End If

				Next

				If Not m_currentImportResultData Is Nothing AndAlso m_currentImportResultData.Count > 0 Then XtraTabControl1.SelectedTabPage = xtabDocScan

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

			End Try


			Return success

		End Function


		Private Sub LoadFinishingReportListData()
			SearchForFinishingReportist()
		End Sub

		''' <summary>
		'''  Performs customer data for scan jobs.
		''' </summary>
		Private Function PerformCustomerDataForScanJobsWebserviceCall() As Boolean

			Dim listDataSource As BindingList(Of MasterMandantData) = New BindingList(Of MasterMandantData)

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.GetCustomerDataForScanJoblist().ToList

			For Each itm In m_MasterMandantData
				Dim data = searchResult.Where(Function(x) x.CustomerID = itm.Customer_id).FirstOrDefault
				If Not data Is Nothing Then
					listDataSource.Add(itm)
				End If

			Next
			m_AllowedMandantData = listDataSource

			Return (Not listDataSource Is Nothing)

		End Function

		''' <summary>
		'''  Performs customer data for application jobs.
		''' </summary>
		Private Function PerformCustomerDataForApplicationJobsWebserviceCall() As Boolean

			Dim listDataSource As BindingList(Of MasterMandantData) = New BindingList(Of MasterMandantData)

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.GetCustomerDataForApplicationJoblist().ToList

			For Each itm In m_MasterMandantData
				Dim data = searchResult.Where(Function(x) x.CustomerID = itm.Customer_id).FirstOrDefault
				If Not data Is Nothing Then
					listDataSource.Add(itm)
				End If

			Next
			m_AllowedMandantData = listDataSource

			Return (Not listDataSource Is Nothing)

		End Function

		''' <summary>
		'''  Performs customer data for update data.
		''' </summary>
		Private Function PerformCustomerDataForUpdateJobs() As Boolean

			Dim listDataSource As BindingList(Of MasterMandantData) = New BindingList(Of MasterMandantData)

			For Each itm In m_MasterMandantData
				listDataSource.Add(itm)
			Next
			m_AllowedMandantData = listDataSource

			Return (Not listDataSource Is Nothing)

		End Function



		''' <summary>
		''' Search for notification over web service.
		''' </summary>
		Private Sub SearchNotificationlistViaWebService()

			If String.IsNullOrWhiteSpace(m_CurrentMDGuid) OrElse m_CurrentMDGuid Is Nothing Then Return
			Dim data = PerformNotificationlistWebserviceCall()

			If data Is Nothing OrElse data.Count = 0 Then Return
			CreateAutomatedTODOWithSystemMessage(data)

		End Sub

		''' <summary>
		'''  Performs notification check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformNotificationlistWebserviceCall() As BindingList(Of NotifyViewData)

			Dim listDataSource As BindingList(Of NotifyViewData) = New BindingList(Of NotifyViewData)
			Dim Customer_ID As String = m_CurrentMDGuid

#If DEBUG Then
			Customer_ID = m_CurrentMDGuid
#End If

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedNotificationForTODO(Customer_ID).ToList

			For Each result In searchResult

				Dim viewData = New NotifyViewData With {
						.ID = result.ID,
						.Customer_ID = result.CustomerID,
						.CheckedFrom = result.CheckedFrom,
						.CheckedOn = result.CheckedOn,
						.CreatedFrom = result.CreatedFrom,
						.CreatedOn = result.CreatedOn,
						.NotifyArt = result.NotifyArt,
						.NotifyComments = result.NotifyComments,
						.NotifyHeader = result.NotifyHeader,
						.Checked = Not (result.CheckedOn Is Nothing)
					}

				If Not m_firstNotifyID.HasValue Then m_firstNotifyID = result.ID
				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		Private Function CreateAutomatedTODOWithSystemMessage(ByVal data As BindingList(Of NotifyViewData)) As Boolean
			Dim result As Boolean = True
			Dim notifyObj As New SP.Internal.Automations.Notifying.Notifier(m_InitializationData)
			Dim notifyData As New SP.Internal.Automations.Notifying.NotifyData

			For Each itm In data
				notifyData = New SP.Internal.Automations.Notifying.NotifyData With {.ID = itm.ID}

				notifyData.NotifyComments = itm.NotifyComments
				notifyData.NotifyArt = itm.NotifyArt
				notifyData.NotifyHeader = itm.NotifyHeader
				notifyData.NotifyHeader = itm.NotifyHeader

				result = result AndAlso notifyObj.AddNewNotifierForSystemMessages(m_InitializationData.MDData.MDGuid, notifyData)

				If result Then PerformUpdateAssignedNotificationWebservice(itm)
			Next


			Return result

		End Function

		Private Function PerformUpdateAssignedNotificationWebservice(ByVal notifyData As NotifyViewData) As Boolean

			Dim success As Boolean = True


			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			Dim userNumber As Integer = 1
			Dim userName As String = "System"
			Dim userGuid As String = "userguid"

			If Not m_InitializationData.UserData Is Nothing Then
				userNumber = m_InitializationData.UserData.UserNr
				userName = m_InitializationData.UserData.UserFullName
				userGuid = m_InitializationData.UserData.UserGuid
			End If
			' Read data over webservice
			success = success AndAlso webservice.UpdateAssignedNotification(m_CurrentMDGuid, notifyData.ID, notifyData.Checked, userGuid, userName)


			Return success

		End Function

		''' <summary>
		''' Search for reports.
		''' </summary>
		Private Sub SearchForFinishingReportist()

			If String.IsNullOrWhiteSpace(m_CurrentMDGuid) OrElse m_CurrentMDGuid Is Nothing Then Return
			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			grdNotification.DataSource = Nothing

			Task(Of Boolean).Factory.StartNew(Function() PerformForFinishingReportlistCallAsync(),
																								CancellationToken.None,
																								TaskCreationOptions.None,
																								TaskScheduler.Default).ContinueWith(Sub(t) FinishForFinishingReportCallTask(t), CancellationToken.None,
																																										TaskContinuationOptions.None, uiSynchronizationContext)

		End Sub

		''' <summary>
		'''  Performs report check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformForFinishingReportlistCallAsync() As Boolean

			m_ReportUtility = New ReportJobUtilities(m_InitializationData)
			m_ReportUtility.ChangeOwnReports = m_ChangeownReportForFinishingFlag
			Dim success = m_ReportUtility.UpdateReportFinishFlag()


			Return success

		End Function

		''' <summary>
		''' Finish report check call.
		''' </summary>
		Private Sub FinishForFinishingReportCallTask(ByVal t As Task(Of Boolean))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(String.Format("report data could not be loaded! {0}", t.Exception.ToString()))
						'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapport-Daten konnten nicht aktuallisiert werden."))

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


		End Sub


		''' <summary>
		''' Search for scan list over web service.
		''' </summary>
		Private Sub SearchScanlistViaWebService()

			If String.IsNullOrWhiteSpace(m_CurrentMDGuid) OrElse m_CurrentMDGuid Is Nothing Then Return
			PerformScanlistWebserviceCallAsync()

		End Sub

		Private Function PerformScanlistWebserviceCallAsync() As Boolean
			Dim success As Boolean = True
			m_scanObj = New ScanJobsUtilities(m_InitializationData)

			m_scanObj.CustomerID = m_CurrentMDGuid

			success = success AndAlso m_scanObj.LoadNotCheckedScanJobData
			LoadScanImportResult(m_scanObj)


			Return success
		End Function

		Private Function LoadScanImportResult(ByVal scanObj As ScanJobsUtilities) As Boolean
			Dim result As Boolean = True

			Try
				Dim scanData As List(Of ScanJobData) = scanObj.ImportedScanJobData.ToList()
				Dim existsScanData As New List(Of ImportResult)
				existsScanData = m_currentImportResultData

				grdScanJobs.DataSource = Nothing

				scanData = scanData.OrderByDescending(Function(data) data.CreatedOn).ToList()

				For Each itm In scanData
					Dim overviewData = New ImportResult

					overviewData.ModulNumber = itm.RecordNumber
					overviewData.Customer_ID = itm.Customer_ID
					overviewData.ModulName = itm.ModulNumber.ToString
					overviewData.CreatedOn = itm.CreatedOn

					existsScanData.Add(overviewData)

				Next

				Trace.WriteLine(String.Format("Anzahl Datensätze: {0}", existsScanData.Count))
				m_currentImportResultData = existsScanData.OrderByDescending(Function(x) x.CreatedOn).ToList()
				grdScanJobs.DataSource = m_currentImportResultData


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				grdScanJobs.DataSource = Nothing

			End Try

		End Function


		''' <summary>
		''' Search for applicant list over web service.
		''' </summary>
		Private Sub SearchApplicantlistViaWebService()

			If String.IsNullOrWhiteSpace(m_CurrentMDGuid) OrElse m_CurrentMDGuid Is Nothing Then Return
			PerformApplicantlistWebserviceCallAsync()

		End Sub

		Private Function PerformApplicantlistWebserviceCallAsync() As Boolean
			Dim success As Boolean = True

			m_ApplicantObj = New ApplicantJobUtilities(m_InitializationData)
			m_ApplicantObj.CustomerID = m_CurrentMDGuid

			success = success AndAlso m_ApplicantObj.LoadNotCheckedApplicantJobData
			LoadApplicationImportResult(m_ApplicantObj)

		End Function

		Private Function LoadApplicationImportResult(ByVal applicantObj As ApplicantJobUtilities) As Boolean
			Dim result As Boolean = True

			Try
				Dim applicationData As List(Of ApplicantData) = applicantObj.ImportedApplicantData.ToList()
				If applicationData.Count = 0 Then Return True

				Dim existsApplicantData As New List(Of ImportResult)
				existsApplicantData = m_currentImportApplicantData ' m_currentImportResultData

				grdApplicant.DataSource = Nothing

				applicationData = applicationData.OrderByDescending(Function(data) data.CreatedOn).ToList()

				For Each itm In applicationData
					Dim overviewData = New ImportResult

					overviewData.ModulNumber = itm.ApplicantNumber
					overviewData.Customer_ID = itm.Customer_ID
					overviewData.ModulName = itm.ApplicantFullname
					overviewData.CreatedOn = itm.CreatedOn

					existsApplicantData.Add(overviewData)

				Next

				Trace.WriteLine(String.Format("Anzahl Datensätze: {0}", existsApplicantData.Count))
				m_currentImportApplicantData = existsApplicantData.OrderByDescending(Function(x) x.CreatedOn).ToList()
				grdApplicant.DataSource = m_currentImportApplicantData


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				grdApplicant.DataSource = Nothing
				result = False

			End Try


			Return result

		End Function

		Private Function PerformProposeListWebserviceCallAsync() As Boolean
			Dim success As Boolean = True
			m_WosObj = New SendScanJobTOWOS(m_InitializationData)

			success = success AndAlso m_WosObj.NotifyCustomerProposeResultDataFromWOS

		End Function



#End Region

		Sub OngvNotification_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvNotification.RowCellClick

			ResetDetails()
			Dim column = e.Column
			Dim dataRow = gvNotification.GetRow(e.RowHandle)
			If dataRow Is Nothing Then
				Return
			Else
				LoadAssignedNotifyDetails()

			End If

		End Sub

		Private Sub OngvNotification_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gvNotification.RowUpdated
			Dim msg As String

			gvNotification.PostEditor()
			gvNotification.UpdateCurrentRow()

			grdNotification.RefreshDataSource()

			grdNotification.FocusedView.CloseEditor()
			Dim success = UpdateRecord(e.Row)

			If success Then
				msg = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
			Else
				msg = m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden.")
			End If

		End Sub

		Private Sub LoadAssignedNotifyDetails()

			Dim data = SelectedNotificationData
			If data Is Nothing Then
				m_Logger.LogError("notification data could not be founded! " & m_InitializationData.MDData.MDDbConn)

				Return
			End If

			lblNotifyHeader.Text = data.NotifyHeader
			lblNotifyArt.Text = data.NotifyArt.ToString
			reNotifyComments.HtmlText = data.NotifyComments
			lblCreatedOn.Text = data.WhoCreated_FullData
			lblCheckedOn.Text = data.WhoChecked_FullData

		End Sub

		Private Function UpdateRecord(ByVal rowobject As Object) As Boolean
			Dim success As Boolean = True
			Dim data = grdNotification.DataSource

			Dim SelectedData = CType(rowobject, NotifyViewData)
			If SelectedData.ID = 0 Then

			Else
				SelectedData.CheckedFrom = m_InitializationData.UserData.UserFullName
				success = PerformUpdateAssignedNotificationWebservice(SelectedData)

			End If


			Return success

		End Function

		''' <summary>
		''' Focuses a notification.
		''' </summary>
		Private Sub FocusNotification(ByVal notifyID As Integer?)

			If Not notifyID.HasValue Then Return
			If Not grdNotification.DataSource Is Nothing Then

				Dim notifyViewData = CType(gvNotification.DataSource, BindingList(Of NotifyViewData))

				Dim index = notifyViewData.ToList().FindIndex(Function(data) data.ID = notifyID)

				m_SuppressUIEvents = True
				Dim rowHandle = gvNotification.GetRowHandle(index)
				gvNotification.FocusedRowHandle = rowHandle

				LoadAssignedNotifyDetails()

				m_SuppressUIEvents = False
			End If

		End Sub

#Region "Helpers"

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			If String.IsNullOrWhiteSpace(clsMandant.MDName) Then Return Nothing

			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			If logedUserData Is Nothing Then Return Nothing

			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)
			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Private Function ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Private Function ParseToDouble(ByVal stringvalue As String, ByVal value As Double?) As Double
			Dim result As Double
			If (Not Double.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

		Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
			Dim result As Boolean
			If (Not Boolean.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function


		Private Function IsMainViewRunning() As Boolean
			Dim result As Boolean = True
			Dim searchApp As String = System.IO.Path.Combine(Environment.CurrentDirectory, "SPS.MainView.exe")
			Dim isApprunning = System.IO.File.Exists(searchApp) AndAlso IsProcessRunningUnderLogedUserName("SPS.MainView")


			Return isApprunning

		End Function

		Public Function IsProcessRunningUnderLogedUserName(ByVal ProcessName As String) As Boolean
			Dim selectQuery As SelectQuery = New SelectQuery("Win32_Process")
			selectQuery.Condition = "Name = '" & ProcessName & ".exe'"
			Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher(selectQuery)
			Dim y As System.Management.ManagementObjectCollection
			Dim serverName = System.Net.Dns.GetHostName
			Dim userName = Environment.UserName
			Dim strObject = "winmgmts://" & serverName

			m_Logger = New Logger

			y = searcher.Get
			For Each proc As ManagementObject In y
				Dim s(1) As String
				proc.InvokeMethod("GetOwner", CType(s, Object()))
				Dim n As String = proc("Name").ToString().ToUpper

				Trace.WriteLine(String.Format("{0}: \\{1}\{2}", n, s(1), s(0)))
				If n = String.Format("{0}.exe".ToUpper, ProcessName.ToString.ToUpper) AndAlso s(0) = userName Then

					Return True ' ("User: " & s(1) & "\\" & s(0))
				End If
			Next


			Return False

		End Function


#End Region


		''' <summary>
		''' notificaton search view data (tbl_Notify).
		''' </summary>
		Private Class NotifyViewData

			Public Property ID As Integer
			Public Property Customer_ID As String
			Public Property NotifyHeader As String
			Public Property NotifyComments As String
			Public Property NotifyArt As NotifyEnum
			Public Property CreatedOn As DateTime?
			Public Property CreatedFrom As String
			Public Property CheckedOn As DateTime?
			Public Property CheckedFrom As String
			Public Property Checked As Boolean

			Public Enum NotifyEnum

				COMMON
				DOCUMENTSCANNING
				APPLICATION

				EMPLOYEEWOS
				CUSTOMERWOS
				WOSMAIL

				STATIONUPDATE
				FTPUPDATE

				PVLCATEGORIES
				PVLVERSIONCHECK

				GAVFLDATA
				PVLADDRESS

				SYSTEMUPDATE
				SCANFILEINFO
				SCANERROR
			End Enum

			Public ReadOnly Property WhoCreated_FullData As String
				Get
					Return String.Format("{0}, {1}", CreatedOn, CreatedFrom)
				End Get
			End Property

			Public ReadOnly Property WhoChecked_FullData As String
				Get
					If CheckedOn.HasValue Then
						Return String.Format("{0}, {1}", CheckedOn, CheckedFrom)
					Else
						Return String.Empty
					End If
				End Get
			End Property


		End Class


		Private Class AdvisorViewData

			Public Property KST As String
			Public Property UserNumber As Integer
			Public Property FristName As String
			Public Property LastName As String

			Public ReadOnly Property LastName_FirstName As String
				Get
					Return String.Format("{0}, {1}", LastName, FristName)
				End Get
			End Property

			Public ReadOnly Property FirstName_LastName As String
				Get
					Return String.Format("{0} {1}", FristName, LastName)
				End Get
			End Property

		End Class


		Private Class ImportResult
			Public Property ModulNumber As String
			Public Property Customer_ID As String
			Public Property ModulName As String
			Public Property CreatedOn As DateTime?

		End Class

		''' <summary>
		''' form context menu.
		''' </summary>
		Private Enum ContextMnuValue

			OpenProgram
			ClosePropram
			ReportDropIn
			CVDropIn
			LoadNotifications

		End Enum

		Private Sub btnSaveSetting_Click(sender As Object, e As EventArgs)


		End Sub

		Private Function ReadSettingFile() As ProgramSettings
			Dim result As New ProgramSettings
			Dim pathSetting As String

			Dim settingFile = Path.Combine(m_CommonConfigFolder, PROGRAM_SETTING_FILE)
			m_Logger.LogDebug(String.Format("m_CommonConfigFolder: {0} | settingFile: {1}", m_CommonConfigFolder, settingFile))

			m_ProgSettingsXml = New SettingsXml(settingFile)
			pathSetting = String.Format(PROGRAM_XML_SETTING_PATH, m_InitializationData.MDData.MDNr)

			Dim fileServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/spsenterprisefolder", pathSetting))
			Dim cvscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvscanfolder", pathSetting))
			Dim reportscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportscanfolder", pathSetting))

			Dim notificationintervalperiode = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiode", pathSetting))), 0)
			Dim notificationintervalperiodeforreport = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiodeforreport", pathSetting))), 0)

			m_Logger.LogDebug(String.Format("fileServer: {0} | cvscanfolder: {1} | reportscanfolder: {2} | notificationintervalperiode: {3} | | notificationintervalperiodeforreport: {4}",
																				fileServer, cvscanfolder, reportscanfolder, notificationintervalperiode, notificationintervalperiodeforreport))

			result.FileServerPath = fileServer
			result.CVScanFolder = cvscanfolder
			result.ReportScanFolder = reportscanfolder

			result.Notificationintervalperiode = notificationintervalperiode
			result.Notificationintervalperiodeforreport = notificationintervalperiodeforreport


			Return result

		End Function

	End Class


	Public Class ProgramSettings
		Public Property FileServerPath As String
		Public Property CVScanFolder As String
		Public Property ReportScanFolder As String

		Public Property Notificationintervalperiode As Decimal
		Public Property Notificationintervalperiodeforreport As Decimal

	End Class

End Namespace
