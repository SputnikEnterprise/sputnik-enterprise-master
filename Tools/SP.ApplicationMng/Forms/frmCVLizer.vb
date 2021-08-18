
Imports System.Data
Imports DevExpress.XtraExport
Imports DevExpress.XtraGrid.Export

Imports TrxmlUtility.Xsd
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.CVLizer.DataObjects
Imports SP.DatabaseAccess.EMailJob
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SP.DatabaseAccess.ScanJob

Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.CommonXmlUtility

Imports System.Text
Imports System.Security.Cryptography
Imports System.IO
Imports SP.DatabaseAccess.CVLizer
Imports SP.ApplicationMng.CVLizer.UI
Imports SP.ApplicationMng.CVLizer.DataObject
Imports System.Threading
Imports SP.ApplicationMng.ChilKatUtility
Imports DevExpress.XtraTab
Imports DevExpress.XtraTab.ViewInfo
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid

Namespace CVLizer

	Public Class frmCVLizer


		Private Delegate Sub StartLoadingData()


#Region "private consts"

		Private Const webServiceCVLUri As String = "http://cvlizer.joinvision.com:80/cvlizer/exservicesoap"
		Private Const CVL_WEB_REQUEST_URL As String = "https://cvlizer.joinvision.com/cvlizer/rest/v1/extract/xml/"
		Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx" ' "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"
		Private Const DEFAULT_SPUTNIK_SCANDOC_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPScanJobUtility.asmx" ' "http://asmx.domain.com/wsSPS_services/SPScanJobUtility.asmx"
		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"

		Private Const JSON_AUTHENTICATION_TOKEN As String = "demotoken"

		Private Const TEMPORARY_FOLDER_ORIGINAL_PDF As String = "ORIGINAL_PDF"      ' Folder where the document is dropped before processing.
		Private Const TEMPORARY_FOLDER_SPLITTED_PDFS As String = "SPLITTED_PDFS"    ' The single pages from the original document are the working reports to import.
		Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
		Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"

		Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
		Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"
		Private Const PROGRAM_XML_SETTING_SCANNING As String = "Settings/Scanning"
		Private Const PROGRAM_XML_SETTING_EMAIL As String = "Settings/EMailSetting"
		Private Const PROGRAM_XML_SETTING_FTP As String = "Settings/FTPSetting"

#End Region

#Region "private fields"

		''' <summary>
		'''  This is the main service thread.
		''' </summary>
		Private serviceThread As Thread

		''' <summary>
		''' This field stores whether the service is running.
		''' </summary>
		''' <remarks>
		''' It must be set to false to leave the main service thread, only when the service is stopped.
		''' </remarks>
		Private serviceStarted As Boolean

		''' <summary>
		''' This thread safe queue is filled with jobs from the file system watcher.
		''' </summary>
		''' <remarks>The working thread will dequeue the jobs and proccess them.</remarks>
		Private synchronizedQueueWrapper As Queue
		Private m_IsProcessRunning As Boolean

		''' <summary>
		'''  The filewatchter instance.
		''' </summary>
		''' <remarks></remarks>
		Private fileWatcher As FileWatcher

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_ApplicationUtilWebServiceUri As String
		Private m_ScanUtilWebServiceUri As String
		Private m_NotificationUtilWebServiceUri As String

		'''' <summary>
		'''' The Initialization data.
		'''' </summary>
		'Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Protected m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		'''' <summary>
		'''' The translation value helper.
		'''' </summary>
		'Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_AppDatabaseAccess As IAppDatabaseAccess
		Private m_EMailDatabaseAccess As IEMailJobDatabaseAccess
		Private m_ScanDatabaseAccess As IScanJobDatabaseAccess

		''' <summary>
		''' The cv data access object.
		''' </summary>
		Private m_CVLDatabaseAccess As ICVLizerDatabaseAccess


		'Private Const User As String = "username"  ' "username"
		'Private Const PW As String = "password" ' "eyfi6876"
		Private m_applicantDb As String
		Private m_customerID As String

		Private m_SelectedFile As String
		Private m_currentTrXMLID As Integer?
		Private m_XMLContent As String
		Private m_CVLizerXMLData As CVLizerXMLData

		Private m_CurrentFileExtension As String
		Private m_CVLFolderToWatch As String
		Private m_CVLXMLFolder As String
		Private m_WorkingReportpath As String
		Private m_TemporaryFolder As String

		Private m_WorkPhaseData As IEnumerable(Of WorkPhaseViewData)

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml
		Private m_ProgSettingsXml As SettingsXml

		''' <summary>
		''' connection string
		''' </summary>
		Private m_connStr_Application As String
		Private m_connStr_CVlizer As String
		Private m_connStr_Systeminfo As String
		Private m_connStr_Scanjobs As String
		Private m_connStr_Email As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath

		Private ucPersonal As CVLizer.UI.ucCVLPersonal
		Private ucWork As CVLizer.UI.ucCVLWork
		Private ucEducation As CVLizer.UI.ucCVLEducation
		Private ucPublication As CVLizer.UI.ucCVLPublication
		Private ucAdditional As CVLizer.UI.ucCVLAdditioinalInformation
		Private ucDocument As CVLizer.UI.ucCVLDocument

		Private m_watchfolder As FileSystemWatcher
		Private m_CVFileData As CVFileData
		Private m_LogData As List(Of EntryLOGData)

		Private m_CommonConfigFolder As String
		Private m_SettingFileName As String
		Private m_SettingFile As ProgramSettings

#End Region


#Region "constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			'm_InitializationData = CreateInitialData(0, 0)
			m_mandant = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_path = New SPProgUtility.ProgPath.ClsProgPath

			m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)
			m_SettingFile = ReadSettingFile()

			m_LogData = New List(Of EntryLOGData)
			m_LogData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Info", .Message = "LOG started..."})

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_connStr_Application = m_SettingFile.ConnstringApplication
			m_connStr_CVlizer = m_SettingFile.ConnstringCVLizer
			m_connStr_Systeminfo = m_SettingFile.ConnstringSysteminfo
			m_connStr_Scanjobs = m_SettingFile.ConnstringScanjobs
			m_connStr_Email = m_SettingFile.ConnstringEMail
			m_CVLFolderToWatch = m_SettingFile.CVLFolderTOWatch
			m_CVLXMLFolder = m_SettingFile.CVLXMLFolder

			Me.KeyPreview = True

			Dim domainName = m_SettingFile.WebserviceDomain
			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)
			m_ScanUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_SCANDOC_UTIL_WEBSERVICE_URI)
			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

			m_AppDatabaseAccess = New AppDatabaseAccess(m_connStr_Application, "DE")
			m_CVLDatabaseAccess = New CVLizerDatabaseAccess(m_connStr_CVlizer, "DE")
			m_EMailDatabaseAccess = New EMailJobDatabaseAccess(m_connStr_Email, "DE")
			m_ScanDatabaseAccess = New ScanJobDatabaseAccess(m_connStr_Scanjobs, "DE")

			If String.IsNullOrWhiteSpace(m_CVLFolderToWatch) OrElse Not Directory.Exists(m_CVLFolderToWatch) Then m_CVLFolderToWatch = Environment.SpecialFolder.MyDocuments
			If String.IsNullOrWhiteSpace(m_CVLXMLFolder) OrElse Not Directory.Exists(m_CVLXMLFolder) Then
				m_CVLXMLFolder = Environment.SpecialFolder.MyDocuments
			End If

			Reset()

#If DEBUG Then
			'm_customerID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If

			ucPersonal = New ucCVLPersonal() 'm_InitializationData)
			pnlPersonal.Controls.Add(ucPersonal)
			ucPersonal.Dock = DockStyle.Fill

			ucWork = New ucCVLWork() 'm_InitializationData)
			pnlWork.Controls.Add(ucWork)
			ucWork.Dock = DockStyle.Fill

			ucEducation = New ucCVLEducation() 'm_InitializationData)
			pnlEducation.Controls.Add(ucEducation)
			ucEducation.Dock = DockStyle.Fill

			ucPublication = New ucCVLPublication() 'm_InitializationData)
			pnlPublication.Controls.Add(ucPublication)
			'ucPublication.Dock = Windows.Forms.DockStyle.Fill
			ucPublication.Anchor = AnchorStyles.Bottom And AnchorStyles.Left + AnchorStyles.Right And AnchorStyles.Top

			ucAdditional = New ucCVLAdditioinalInformation() ' m_InitializationData)
			pnlAddtionalInformation.Controls.Add(ucAdditional)
			ucAdditional.Dock = DockStyle.Fill

			ucDocument = New ucCVLDocument() ' m_InitializationData)
			pnlDocument.Controls.Add(ucDocument)
			ucDocument.Dock = DockStyle.Fill


			'LoadFilesIntoLst()

			xtabCVLDetails.SelectedTabPage = xtabApplicant
			xtabCV.SelectedTabPage = xtabCVLData

			LoadCVLProfileData()
			Dim profileData = CType(grdProfile.DataSource, IEnumerable(Of ProfileLocalViewData))
			If Not profileData Is Nothing AndAlso profileData.Count > 0 Then
				FocusMainGrid(profileData(0).ProfileID)
			End If

			m_watchfolder = New System.IO.FileSystemWatcher()
#If Not DEBUG Then
			WatchAssignedFolder()
#End If

			AddHandler deDate.EditValueChanged, AddressOf deDate_EditValueChanged
			'AddHandler gvAdvisorLogin.CustomDrawRowFooter, AddressOf OngvAdvisorLogin_CustomDrawRowFooter
			'AddHandler gvAdvisorLogin.CustomDrawRowFooterCell, AddressOf OngvAdvisorLogin_CustomDrawRowFooterCell

		End Sub

#End Region


		Sub LoadDomains()

			Dim result As Boolean = False
			Dim m_customerID As String = "A64C930D-F7F7-49E5-B068-3D9ACF54B1BF"
			Dim obj = New SP.ApplicationMng.CVLizer.Import.CVLizer


			result = obj.LoadCVLDomains()


		End Sub

#Region "public methodes"


#End Region


#Region "Private Properties"

		'''' <summary>
		'''' Gets the selected file directory data.
		'''' </summary>
		'Private ReadOnly Property SelectedFileViewData As FileDirectoryData
		'	Get
		'		Dim grdView = TryCast(grdFiles.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		'		If Not (grdView Is Nothing) Then

		'			Dim selectedRows = grdView.GetSelectedRows()

		'			If (selectedRows.Count > 0) Then
		'				Dim viewData = CType(grdView.GetRow(selectedRows(0)), FileDirectoryData)
		'				Return viewData
		'			End If

		'		End If

		'		Return Nothing
		'	End Get

		'End Property

		''' <summary>
		''' Gets the selected profile data.
		''' </summary>
		Private ReadOnly Property SelectedProfileViewData As ProfileLocalViewData
			Get
				Dim grdView = TryCast(grdProfile.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), ProfileLocalViewData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region




#Region "Reset"

		Private Sub Reset()

			m_customerID = String.Empty
			deDate.EditValue = Now.Date

			ResetProfileGrid()
			ResetLOGGrid()
			ResetXMLFilesGrid()
			ResetDocScanGrid()
			ResetNotScanedFilesGrid()

			ResetMailNotificationGrid()
			ResetAdvisorLoginGrid()
			ResetAdvisorMontlyLoginGrid()

			tpAdvisorLogins.SelectedPage = tpDailylogin

		End Sub

		''' <summary>
		''' Resets cv Proflie grid.
		''' </summary>
		Private Sub ResetProfileGrid()

			gvProfile.OptionsView.ShowIndicator = False
			gvProfile.OptionsView.ShowAutoFilterRow = True
			gvProfile.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvProfile.OptionsView.ShowFooter = False
			gvProfile.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvProfile.Columns.Clear()


			Dim columnProfileID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnProfileID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnProfileID.OptionsColumn.AllowEdit = False
			columnProfileID.Caption = ("ProfileID")
			columnProfileID.Name = "ProfileID"
			columnProfileID.FieldName = "ProfileID"
			columnProfileID.Visible = True
			columnProfileID.Width = 10
			gvProfile.Columns.Add(columnProfileID)

			Dim columnPersonalID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPersonalID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPersonalID.OptionsColumn.AllowEdit = False
			columnPersonalID.Caption = ("PersonalID")
			columnPersonalID.Name = "PersonalID"
			columnPersonalID.FieldName = "PersonalID"
			columnPersonalID.Width = 10
			columnPersonalID.Visible = True
			gvProfile.Columns.Add(columnPersonalID)

			Dim columnWorkID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWorkID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWorkID.OptionsColumn.AllowEdit = False
			columnWorkID.Caption = ("WorkID")
			columnWorkID.Name = "WorkID"
			columnWorkID.FieldName = "WorkID"
			columnWorkID.Width = 10
			columnWorkID.Visible = False
			gvProfile.Columns.Add(columnWorkID)

			Dim columnEducationID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEducationID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEducationID.OptionsColumn.AllowEdit = False
			columnEducationID.Caption = ("EducationID")
			columnEducationID.Name = "EducationID"
			columnEducationID.FieldName = "EducationID"
			columnEducationID.Width = 10
			columnEducationID.Visible = False
			gvProfile.Columns.Add(columnEducationID)

			Dim columnAdditionalID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdditionalID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdditionalID.OptionsColumn.AllowEdit = False
			columnAdditionalID.Caption = ("AdditionalID")
			columnAdditionalID.Name = "AdditionalID"
			columnAdditionalID.FieldName = "AdditionalID"
			columnAdditionalID.Width = 5
			columnAdditionalID.Visible = False
			gvProfile.Columns.Add(columnAdditionalID)

			Dim columnObjectiveID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnObjectiveID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnObjectiveID.OptionsColumn.AllowEdit = False
			columnObjectiveID.Caption = ("ObjectiveID")
			columnObjectiveID.Name = "ObjectiveID"
			columnObjectiveID.FieldName = "ObjectiveID"
			columnObjectiveID.Width = 10
			columnObjectiveID.Visible = False
			gvProfile.Columns.Add(columnObjectiveID)

			Dim columnApplicationID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnApplicationID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnApplicationID.OptionsColumn.AllowEdit = False
			columnApplicationID.Caption = ("ApplicationID")
			columnApplicationID.Name = "ApplicationID"
			columnApplicationID.FieldName = "ApplicationID"
			columnApplicationID.Width = 10
			columnApplicationID.Visible = True
			gvProfile.Columns.Add(columnApplicationID)

			Dim columnApplicantID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnApplicantID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnApplicantID.OptionsColumn.AllowEdit = False
			columnApplicantID.Caption = ("ApplicantID")
			columnApplicantID.Name = "ApplicantID"
			columnApplicantID.FieldName = "ApplicantID"
			columnApplicantID.Width = 10
			columnApplicantID.Visible = True
			gvProfile.Columns.Add(columnApplicantID)

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("CustomerName")
			columnCustomer_ID.Name = "CustomerName"
			columnCustomer_ID.FieldName = "CustomerName"
			columnCustomer_ID.Visible = True
			columnCustomer_ID.Width = 60
			columnCustomer_ID.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			gvProfile.Columns.Add(columnCustomer_ID)

			Dim columnApplicationLabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnApplicationLabel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnApplicationLabel.OptionsColumn.AllowEdit = False
			columnApplicationLabel.Caption = ("ApplicationLabel")
			columnApplicationLabel.Name = "ApplicationLabel"
			columnApplicationLabel.FieldName = "ApplicationLabel"
			columnApplicationLabel.Visible = True
			columnApplicationLabel.Width = 50
			gvProfile.Columns.Add(columnApplicationLabel)

			Dim columnFullname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFullname.OptionsColumn.AllowEdit = False
			columnFullname.Caption = ("Fullname")
			columnFullname.Name = "Fullname"
			columnFullname.FieldName = "Fullname"
			columnFullname.Width = 50
			columnFullname.Visible = True
			gvProfile.Columns.Add(columnFullname)

			Dim columnApplicantAgeViewData As New DevExpress.XtraGrid.Columns.GridColumn()
			columnApplicantAgeViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnApplicantAgeViewData.OptionsColumn.AllowEdit = False
			columnApplicantAgeViewData.Caption = ("ApplicantAgeViewData")
			columnApplicantAgeViewData.Name = "ApplicantAgeViewData"
			columnApplicantAgeViewData.FieldName = "ApplicantAgeViewData"
			columnApplicantAgeViewData.Width = 50
			columnApplicantAgeViewData.Visible = True
			gvProfile.Columns.Add(columnApplicantAgeViewData)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.OptionsColumn.AllowEdit = False
			columnCreatedOn.Caption = ("CreatedOn")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "G"
			columnCreatedOn.Width = 40
			columnCreatedOn.Visible = True
			gvProfile.Columns.Add(columnCreatedOn)

			Dim item As New GridGroupSummaryItem() With {.FieldName = "CustomerName", .SummaryType = DevExpress.Data.SummaryItemType.Count}
			gvProfile.GroupSummary.Add(item)


			grdProfile.DataSource = Nothing

		End Sub



#End Region


#Region "form handle"

		Private Sub frmCVLizer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
			Me.OnStop()
		End Sub

#End Region


#Region "loading directories files"

		'Private Sub txtPath_ButtonClick(sender As Object, e As ButtonPressedEventArgs)
		'	Dim dialog As New FolderBrowserDialog()

		'	dialog.Description = ("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")
		'	dialog.ShowNewFolderButton = True
		'	dialog.SelectedPath = txtPath.EditValue

		'	If dialog.ShowDialog() = DialogResult.OK Then
		'		txtPath.EditValue = dialog.SelectedPath
		'		LoadFilesIntoLst()
		'	End If

		'End Sub

		'Private Sub txtPath_KeyUp(sender As Object, e As KeyEventArgs)
		'	If e.KeyCode = Keys.Enter Then
		'		LoadFilesIntoLst()
		'	End If
		'End Sub

		'Private Sub LoadFilesIntoLst()

		'	grdFiles.DataSource = Nothing
		'	Dim AllowedExtension As String = ".pdf"
		'	Dim gridDataList As New BindingList(Of FileDirectoryData)
		'	If Not Directory.Exists(txtPath.EditValue) Then Return

		'	Dim orderedFiles = New System.IO.DirectoryInfo(txtPath.EditValue).GetFiles(".", SearchOption.TopDirectoryOnly).OrderByDescending(Function(x) x.CreationTime)

		'	For Each file As System.IO.FileInfo In orderedFiles
		'		gridDataList.Add(New FileDirectoryData With {.Filename = file.Name, .FileDate = file.CreationTimeUtc}) ' Path.GetFileName(file), .FileDate = FileDateTime(file)})
		'	Next

		'	grdFiles.DataSource = gridDataList

		'End Sub

		'Sub OngvFiles_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
		'	Dim data = SelectedFileViewData
		'	If data Is Nothing Then
		'		SplashScreenManager.CloseForm(False)
		'		m_UtilityUI.ShowErrorDialog(("Verzeichnis-Dateien konnten nicht geladen werden."))
		'		Return
		'	End If

		'	m_SelectedFile = Path.Combine(txtPath.EditValue, data.Filename)
		'	DisplaySelectedDirectoryFileDetail()

		'	If e.Clicks = 2 Then
		'		If String.IsNullOrWhiteSpace(m_SelectedFile) Then Return
		'		m_Utility.OpenFileWithDefaultProgram(m_SelectedFile)
		'	End If

		'End Sub

		'Private Sub DisplaySelectedDirectoryFileDetail()

		'	Dim fileextension = Path.GetExtension(m_SelectedFile)

		'	Try
		'		If fileextension.ToLower = ".pdf" Then

		'		ElseIf fileextension.ToLower = ".doc" OrElse fileextension.ToLower = ".docx" Then
		'			rtfApplicantData.LoadDocument(m_SelectedFile)

		'		ElseIf fileextension.ToLower = ".rtf" Then
		'			rtfApplicantData.LoadDocument(m_SelectedFile, DevExpress.XtraRichEdit.DocumentFormat.Rtf)

		'		ElseIf fileextension.ToLower = ".txt" Then
		'			rtfApplicantData.LoadDocument(m_SelectedFile, DevExpress.XtraRichEdit.DocumentFormat.PlainText)

		'		ElseIf fileextension.ToLower = ".xml" Then
		'			rtfApplicantData.LoadDocument(m_SelectedFile, DevExpress.XtraRichEdit.DocumentFormat.PlainText)


		'		End If

		'	Catch ex As Exception

		'	End Try


		'End Sub

		Private Sub xtabCV_SelectedPageChanged(sender As Object, e As TabPageChangedEventArgs) Handles xtabCV.SelectedPageChanged
			LoadSelectedTabData()
		End Sub

#End Region


#Region "fileWatcher"


		Public Sub WatchAssignedFolder()

			If String.IsNullOrWhiteSpace(m_CVLFolderToWatch) Then Return

			Dim queue As New Queue
			synchronizedQueueWrapper = Queue.Synchronized(queue)


			Dim FileListenerSettings As New ReportFileListenerSettings With {.bNotifyOnScan = False, .bNotifyOnDispose = False, .Folder2Watch = m_CVLFolderToWatch,
				.ConnStr4ScanDb = m_connStr_Application, .Folder4ProcessedScannedDocuments = "",
				.SendNotificationTo = "", .SmtpPort = "", .SmtpServer = "",
				.Folder4TemporaryFiles = "", .WorkingForWebService = True}

			fileWatcher = New FileWatcher()
			If Me.fileWatcher.Configure(m_CVLFolderToWatch, "*.*", FileListenerSettings, AddressOf HandleWorkingReport) Then

				' Create worker thread; this will invoke the WorkerFunction when we start it.
				' Since we use a separate worker thread, the main service thread will return quickly, telling Windows that service has started.
				Dim threadStart As New ThreadStart(AddressOf WorkerFunction)
				serviceThread = New Thread(threadStart)

				' Set flag to indicate worker thread is active
				Me.serviceStarted = True

				Dim directoryToListenInfo As New DirectoryInfo(m_CVLFolderToWatch)

				' Add existing pdf files to the job queue.
				For Each existingReport As FileInfo In directoryToListenInfo.GetFiles("*.*", System.IO.SearchOption.AllDirectories)
					synchronizedQueueWrapper.Enqueue(existingReport.FullName)
				Next

				' Start the thread
				serviceThread.Start()
			Else
				' The file watcher could not be configured, so by not starting any thread, the service will automatically be stopped.
				m_Logger.LogWarning("The file listener could not be started, since to a configuration error. The service will be stopped.")
				OnStop()
			End If

		End Sub

		''' <summary>
		''' Callback method for the file listener.
		''' </summary>
		''' <param name="reportPath">The path to the report file.</param>
		''' <remarks>As soon as a file is dropped in the folder, this callback method is called.</remarks>
		Private Sub HandleWorkingReport(ByVal reportPath As String)

			' Queue the job.
			Me.synchronizedQueueWrapper.Enqueue(reportPath)

		End Sub

		''' <summary>
		''' Insert Code to start the service.
		''' </summary>
		Sub OnStop()
			' Code to end service
			If serviceStarted Then
				serviceStarted = False
				serviceThread.Join(New TimeSpan(0, 0, 2))
			End If
			m_Logger.LogWarning("The file listener is going to be down. The service will be stopped.")

			m_watchfolder.Dispose()

		End Sub

		''' <summary>
		''' This is the main loop in the service.
		''' </summary>
		Private Sub WorkerFunction()
			Dim success As Boolean = True
			Dim proccedFileNumber As Integer = 0

			' Start an endless loop; loop will abort only when "serviceStarted" flag = false
			While serviceStarted

				If Me.synchronizedQueueWrapper.Count > 0 Then

					Dim cvFileName As String = synchronizedQueueWrapper.Dequeue

					Thread.Sleep(New TimeSpan(0, 0, 10))

					Dim cvFileInfo As New FileInfo(cvFileName)

					If m_IsProcessRunning Then
						Me.synchronizedQueueWrapper.Enqueue(cvFileName)
						m_Logger.LogInfo(String.Format("The file {0} is in used...", cvFileInfo.FullName))

						Continue While
					End If

					If Not IsFileAlreadyParsed(cvFileInfo.FullName) Then

						proccedFileNumber += 1
						m_Logger.LogInfo(String.Format("Jobnumer {0} with file {1} will now be processed...", proccedFileNumber, cvFileInfo.FullName))

						' The dictionary name of the report file is used as the mandant guid.
						Dim mandantGuid As String = cvFileInfo.Directory.Name
#If DEBUG Then
						mandantGuid = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If
						'Do something with the working report
						m_Logger.LogInfo(String.Format("Pass working report {0} to Converter!", cvFileName))

						m_customerID = mandantGuid
						success = StartParsingDropInFile(m_customerID, cvFileInfo.FullName)

						Dim msg As String = String.Empty
						Dim logData = New List(Of EntryLOGData)
						If success Then
							msg = String.Format("{0} >>> {1}: file was parsed...", m_customerID, cvFileInfo.FullName)
							m_Logger.LogInfo(msg)
						Else
							msg = String.Format("*** {0} >>> {1}: file could not parsed...", m_customerID, cvFileInfo.FullName)
							m_Logger.LogError(msg)
						End If

						Dim existData As List(Of EntryLOGData) = CType(grdLOG.DataSource, List(Of EntryLOGData))
						If existData Is Nothing Then
							existData = New List(Of EntryLOGData)
							existData = m_LogData
						End If
						existData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "CVDropIn", .Message = msg})
						grdLOG.DataSource = existData
						grdLOG.RefreshDataSource()


						m_Logger.LogInfo(String.Format("Jobnumer {0} for file {1} processed!", proccedFileNumber, cvFileName))

					Else
						m_Logger.LogInfo(String.Format("The file {0} is invalid and will not be processed.", cvFileInfo.FullName))
					End If

				End If

				If serviceStarted Then
					Thread.Sleep(New TimeSpan(0, 0, 1))
				End If

			End While

			' Time to end the thread
			Thread.CurrentThread.Abort()
		End Sub

		Private Function StartParsingDropInFile(ByVal customerID As String, ByVal fullFileName As String) As Boolean
			Dim success As Boolean = True
			Dim m_EMailUtility As EMailUtility

			m_EMailUtility = New EMailUtility(m_SettingFile)
			m_EMailUtility.m_PatternData = m_EMailDatabaseAccess.LoadEMailPatternParsingData(customerID)

			Dim parseResult As ParsResult = m_EMailUtility.ParsReceivedDropIn(customerID)
			If Not parseResult.ParseValue AndAlso parseResult.ParseMessage.ToLower = "not defined" Then
				' it was not from homepage!
				m_Logger.LogDebug(String.Format(">>> DropIn is not from homepage!"))
			End If

			If Not parseResult Is Nothing Then
				Dim cvFiles As New List(Of String)
				cvFiles.Add(fullFileName)
				If cvFiles.Count > 0 Then

					success = success AndAlso SendFilesToCVL(cvFiles, parseResult.ApplicantID, parseResult.ApplicationID)
					If success AndAlso Not m_CVLizerXMLData Is Nothing Then
						success = success AndAlso UpdateApplicantDataWithCVLData_(parseResult.ApplicantID, parseResult.ApplicationID, fullFileName)
					Else
						m_Logger.LogError(String.Format("SendFilesToCVL: file could not be send to cvlizer! {0} >>> {1}", m_customerID, fullFileName))

					End If

				End If
			End If

			Try
				Dim movingFiles As Boolean = MovePDFToProcessedScannedDocuments(fullFileName, m_SettingFile.CVLFolderTOArchive, customerID)
				If Not movingFiles AndAlso File.Exists(fullFileName) Then File.Delete(fullFileName)

				Dim result = LoadProfileDataOfXML()
				m_Logger.LogInfo(String.Format("file was parsed and data are loaded. {0}", m_customerID))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0} ->>> {1}", m_customerID, ex.ToString))

			End Try

			'success = success AndAlso DeleteSplittedFolder(workingFolder)


			Return success
		End Function

		Private Function SendFilesToCVL(ByVal cvFilename As List(Of String), ByVal applicantID As Integer?, ByVal applicationID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLizerXMLData = Nothing

			Try
				Dim cvlImport = New CVLizer.Import.CVLizer
				cvlImport.m_AppDatabaseAccess = m_AppDatabaseAccess
				cvlImport.m_CVLDatabaseAccess = m_CVLDatabaseAccess
				cvlImport.m_SettingFile = m_SettingFile
				cvlImport.Customer_ID = m_customerID
				cvlImport.ApplicantID = applicantID
				cvlImport.ApplicationID = applicationID

				Dim payableUser As New CustomerPayableUserData
				payableUser.CustomerID = m_customerID
				payableUser.AdvisorID = String.Empty
				payableUser.JobID = "CV-DropIn"
				payableUser.ServiceName = "CVLIZER_SCAN"
				payableUser.Advisorname = "DropIn-User"

				cvlImport.PayableUserData = payableUser

				result = result AndAlso cvlImport.ParseCVFileWithCVLizer(cvFilename)
				If result Then m_CVLizerXMLData = cvlImport.GetCVLProfileData

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return False

			End Try


			Return result

		End Function

		Private Function UpdateApplicantDataWithCVLData_(ByVal applicantID As Integer, ByVal applicationID As Integer, ByVal fullFileName As String) As Boolean
			Dim success As Boolean = True

			If m_CVLizerXMLData Is Nothing Then
				m_Logger.LogError("no cv content was founded!")

				Return False
			End If

			success = success AndAlso m_AppDatabaseAccess.UpdateApplicatantWithCVLData(m_CVLizerXMLData, applicantID, applicationID, EMailSettingData.PriorityModulEnum.NOTDEFINED)
			If Not success Then Return success

			Dim scanData = m_ScanDatabaseAccess.LoadDropInDataForApplication(m_CVLizerXMLData.Customer_ID, fullFileName)
			If Not scanData Is Nothing Then
				success = success AndAlso m_AppDatabaseAccess.UpdateApplicationWithScanDropInData(m_CVLizerXMLData.Customer_ID, applicationID, scanData)
			End If

			m_Logger.LogDebug("email data stored into db!")


			Return success

		End Function


#End Region


#Region "sending cv to parse"

		Private Sub btnSendToCVLizer_Click(sender As Object, e As EventArgs) Handles btnSendToCVLizer.Click
			Dim success As Boolean = True

			'success = success AndAlso ParseCVFileWithCVLizer(m_SelectedFile)
			'success = success AndAlso LoadProfileDataOfXML()

		End Sub

		Private Function IsFileAlreadyParsed(ByVal cvFilename As String) As Boolean
			Dim result As Boolean?

			m_CVFileData = New CVFileData

			Try

				Dim myFile As New FileInfo(cvFilename)
				Dim sizeInBytes As Long = myFile.Length
				If Path.GetExtension(cvFilename).ToLower = ".xml" Then Return False

				Dim bytes() = m_Utility.LoadFileBytes(cvFilename)
				Dim s As New SHA256Managed
				Dim hash() As Byte = s.ComputeHash(bytes)
				Dim hashValue = Convert.ToBase64String(hash)

				Dim myFileExtension = GetMimeFromBytes(bytes)

				Dim extension = (From kp As KeyValuePair(Of String, String) In MIMETypesDictionary
								 Where kp.Value = myFileExtension
								 Select kp.Key).ToList()

				m_CVFileData.FileContent = bytes
				m_CVFileData.FileName = cvFilename
				m_CVFileData.FileHash = hashValue
				m_CVFileData.FileExtension = Path.GetExtension(cvFilename)
				m_CVFileData.FileDate = File.GetCreationTimeUtc(cvFilename)
				m_CVFileData.Filesize = myFile.Length


				result = m_CVLDatabaseAccess.ExistsCVLFile(m_customerID, hashValue)

				If (result Is Nothing) Then
					SplashScreenManager.CloseForm(False)
					m_Logger.LogError(("Dokument-Daten konnten nicht überprüft werden."))

					Return False
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("file can not be checked for existing: {0} >>> {1}", cvFilename, ex.ToString))
				Return False
			End Try

			Return result

		End Function

		Private Function PerformSendingCVFileWithRESTService(ByVal cvFilename As String) As Boolean
			Dim success As Boolean = True

			If String.IsNullOrWhiteSpace(cvFilename) Then Return False

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihr Dokument{0}{1}{0}wird analysiert."), vbNewLine, cvFilename) & Space(100))
			SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")

			Try
				m_Logger.LogDebug(String.Format("uploading file for parsing: {0}", cvFilename))

				Dim bytes() As Byte = System.IO.File.ReadAllBytes(cvFilename)
				Dim file As String = Convert.ToBase64String(bytes)
				Dim data As JsonData = New JsonData
				data.model = "cvlizer_3_0"
				data.language = "de"
				data.filename = cvFilename
				data.data = file

				Dim json As String = (New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data))
				Dim jsonBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(json)
				Dim req As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(CVL_WEB_REQUEST_URL), System.Net.HttpWebRequest)
				req.Method = "POST"
				req.ContentType = "application/json"
				req.Headers.Add("Authorization", String.Format("Bearer {0}", JSON_AUTHENTICATION_TOKEN))
				req.ContentLength = jsonBytes.Length
				Dim post As System.IO.Stream = req.GetRequestStream
				post.Write(jsonBytes, 0, jsonBytes.Length)

				Dim result As String = Nothing
				Dim resp As System.Net.HttpWebResponse = CType(req.GetResponse, System.Net.HttpWebResponse)
				Dim reader As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream)
				result = reader.ReadToEnd
				reader.Close()
				m_Logger.LogDebug(String.Format("parsing was successfull..."))

				Dim tmpFilename = Path.GetTempFileName()
				Using sw As StreamWriter = New StreamWriter(tmpFilename, False, Encoding.UTF8)
					sw.Write(result)
					sw.Close()
				End Using
				m_Logger.LogDebug(String.Format("result file is created in: {0}", tmpFilename))

				Try
					Dim resultXMLFileName = Path.Combine(m_CVLXMLFolder, IO.Path.GetFileName((Path.ChangeExtension(cvFilename, "xml"))))

					If IO.File.Exists(resultXMLFileName) Then
						m_Logger.LogDebug(String.Format("deleting file: {0}", resultXMLFileName))
						IO.File.Delete(resultXMLFileName)
					End If
					IO.File.Move(tmpFilename, resultXMLFileName)

					' set cvfile to result file
					m_CVFileData.XMLFileName = resultXMLFileName
					m_SelectedFile = resultXMLFileName


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))

				End Try


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				success = False

			Finally
				SplashScreenManager.CloseForm(False)

			End Try


			Return success

		End Function


#End Region


#Region "parsing CVLizer-XML file"


		Private Sub OnbtnReadCVLizerXML_Click(sender As Object, e As EventArgs) Handles btnReadCVLizerXML.Click
			Dim success = True

			success = success AndAlso LoadCVFileFromXML(m_SelectedFile, False)
			If Not success Then Return

			LoadProfileDataOfXML()

		End Sub

		Private Sub OnbtnReadAndSaveCVLizerXML_Click(sender As Object, e As EventArgs) Handles btnReadAndSaveCVLizerXML.Click
			Dim success = True

			m_CVFileData = Nothing
			success = success AndAlso LoadCVFileFromXML(m_SelectedFile, True)
			If Not success Then Return

			LoadProfileDataOfXML()

		End Sub

		Private Sub OnbtnReadCVLizerProfile_Click(sender As Object, e As EventArgs) Handles btnReadCVLizerProfile.Click
			Dim success = True

			m_CVFileData = Nothing
			LoadProfileDataOfXML()

		End Sub

		Private Function LoadCVFileFromXML(ByVal cvFileName As String, ByVal SaveToDb As Boolean) As Boolean
			Dim success = True

			m_XMLContent = String.Empty
			If Path.GetExtension(cvFileName).ToLower <> ".xml" Then Return False

			If m_CVFileData Is Nothing Then
				m_CVFileData = New CVFileData
				m_CVFileData.XMLFileName = m_SelectedFile
			End If

			success = success AndAlso ParseAssignedXMLDetail(SaveToDb)


			Return success

		End Function

		Private Function ParseAssignedXMLDetail(ByVal SaveToDb As Boolean) As Boolean
			Dim success As Boolean = True
			Dim cvLizerUtility = New CVLizerLoader.CustomAreaType(m_customerID, m_CVFileData)

			If Not SaveToDb Then
				m_CVLizerXMLData = cvLizerUtility.ImportFromFile()
				success = success AndAlso Not (m_CVLizerXMLData Is Nothing)
			Else
				success = success AndAlso cvLizerUtility.PaseXMLFileAndAddToDatabase()

			End If


			Return success

		End Function


#End Region


		Private Function LoadProfileDataOfXML() As Boolean
			Dim result As Boolean = True

			If Me.InvokeRequired = True Then
				Me.Invoke(New StartLoadingData(AddressOf LoadProfileDataOfXML))
			Else

				result = result AndAlso LoadCVLProfileData()
			End If


			Return True

		End Function


		Private Function LoadCVLProfileData() As Boolean

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim profileDate As DateTime? = Nothing
			profileDate = deDate.EditValue

			If profileDate Is Nothing Then profileDate = Now.Date
			Try

				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")


				Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)
				m_Logger.LogInfo(String.Format("m_ApplicationUtilWebServiceUri: {0} ", m_ApplicationUtilWebServiceUri, m_customerID))

				' Read data over webservice
				Dim searchResult = webservice.LoadALLAssignedDayCVLProfileViewData(profileDate)

				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("LoadALLAssignedDayCVLProfileViewData could not be loaded from webservice! {0} | {1}", m_customerID, 0))

					Return False
				End If

				Dim gridData = (From person In searchResult
								Select New ProfileLocalViewData With {.ProfileID = person.ProfileID,
															.AdditionalID = person.AdditionalID,
															.ApplicationID = person.ApplicationID,
															.ApplicantID = person.ApplicantID,
															.ApplicationLabel = person.ApplicationLabel,
															.CreatedOn = person.CreatedOn,
															.DateOfBirth = person.DateOfBirth,
															.Customer_ID = person.Customer_ID,
															.CustomerName = person.CustomerName,
															.EducationID = person.EducationID,
															.GenderLabel = person.GenderLabel,
															.FirstName = person.FirstName,
															.LastName = person.LastName,
															.ObjectiveID = person.ObjectiveID,
															.PersonalID = person.PersonalID,
															.WorkID = person.WorkID
															}).ToList()

				Dim listDataSource As BindingList(Of ProfileLocalViewData) = New BindingList(Of ProfileLocalViewData)
				For Each p In gridData
					listDataSource.Add(p)
				Next

				grdProfile.DataSource = listDataSource
				bsiMainRecordCount.Caption = String.Format("Anzahl Datensätze: {0}", listDataSource.Count)

				PresentProfileData()


				Return Not listDataSource Is Nothing

			Catch ex As Exception
				m_Logger.LogError(String.Format("LoadCVLProfileData.LoadALLAssignedDayCVLProfileViewData could not be loaded from webservice! {1} | {2}{0}{3}", vbNewLine, m_customerID, profileDate, ex.ToString))

				Return Nothing
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Function

		Sub OngvgvProfile_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvProfile.RowCellClick
			PresentProfileData()
		End Sub

		Private Sub OngvProfile_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvProfile.FocusedRowChanged
			PresentProfileData()
		End Sub

		''' <summary>
		''' Focuses a grid view.
		''' </summary>
		Private Sub FocusMainGrid(ByVal profileID As Integer)

			If Not grdProfile.DataSource Is Nothing Then

				Dim documentViewData = CType(gvProfile.DataSource, IEnumerable(Of ProfileLocalViewData))

				Dim index = documentViewData.ToList().FindIndex(Function(data) data.ProfileID = profileID)

				Dim rowHandle = gvProfile.GetRowHandle(index)
				gvProfile.FocusedRowHandle = rowHandle

			End If

		End Sub

		Private Function PresentProfileData() As Boolean
			Dim result As Boolean = True

			Try
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(String.Format(("Ihre Daten werden abgerufen.")) & Space(100))
				SplashScreenManager.Default.SetWaitFormDescription(("Dies kann einige Sekunden dauern") & "...")

				ucPersonal.Visible = False
				ucWork.Visible = False
				ucEducation.Visible = False
				ucPublication.Visible = False
				ucAdditional.Visible = False
				ucDocument.Visible = False

				Dim data = SelectedProfileViewData
				If data Is Nothing Then
					SplashScreenManager.CloseForm(False)

					Return False
				End If

				ucPersonal.LoadAssignedPersonalData(data.Customer_ID, data.ProfileID, data.PersonalID)
				ucPersonal.Visible = True

				If data.PersonalID.GetValueOrDefault(0) = 0 Then Return True

				ucWork.LoadAssignedWorkData(data.ProfileID, data.WorkID)
				ucWork.Visible = True

				ucEducation.LoadAssignedEducationData(data.ProfileID, data.EducationID)
				ucEducation.Visible = True

				ucPublication.LoadAssignedEducationData(data.ProfileID)
				ucPublication.Visible = True

				ucAdditional.LoadAssignedAdditionalData(data.ProfileID, data.AdditionalID)
				ucAdditional.Visible = True

				ucDocument.LoadAssignedDocumentData(data.ProfileID)
				ucDocument.Visible = True

			Catch ex As Exception
				SplashScreenManager.CloseForm(False)
				m_Logger.LogError(ex.ToString)

				Return False
			Finally
				SplashScreenManager.CloseForm(False)

			End Try


			Return result

		End Function

		Private Sub LoadPersonalInformation()
			If m_CVLizerXMLData.PersonalInformation Is Nothing Then Return

			Dim listDataSource As BindingList(Of PersonalInformationData) = New BindingList(Of PersonalInformationData)
			listDataSource.Add(m_CVLizerXMLData.PersonalInformation)

			grdProfile.DataSource = listDataSource

		End Sub

		Private Sub LoadWorkInformation()
			If m_CVLizerXMLData.Work Is Nothing Then Return

			Dim listDataSource As BindingList(Of WorkPhaseData) = New BindingList(Of WorkPhaseData)
			For Each p In m_CVLizerXMLData.Work.WorkPhases
				listDataSource.Add(p)
			Next

			grdProfile.DataSource = listDataSource

		End Sub

		Private Sub LoadEducationInformation()
			If m_CVLizerXMLData.Education Is Nothing Then Return

			Dim listDataSource As BindingList(Of EducationPhaseData) = New BindingList(Of EducationPhaseData)
			For Each p In m_CVLizerXMLData.Education.EducationPhases
				listDataSource.Add(p)
			Next

			grdProfile.DataSource = listDataSource

		End Sub

		Private Sub LoadPublicationInformation()
			If m_CVLizerXMLData.Publication Is Nothing Then Return

			Dim listDataSource As BindingList(Of PublicationData) = New BindingList(Of PublicationData)
			For Each p In m_CVLizerXMLData.Publication
				listDataSource.Add(p)
			Next

			grdProfile.DataSource = listDataSource

		End Sub

		Private Sub LoadAdditionalInformation()
			If m_CVLizerXMLData.AdditionalInformation Is Nothing Then Return

			Dim listDataSource As BindingList(Of OtherInformationData) = New BindingList(Of OtherInformationData)
			For Each p In m_CVLizerXMLData.AdditionalInformation.Additionals
				'listDataSource.Add(p)
			Next

			grdProfile.DataSource = listDataSource

		End Sub



#Region "Helpers"

		Private Function ReadSettingFile() As ProgramSettings
			Dim result As New ProgramSettings
			Dim pathSetting As String
			Dim scanningSetting As String
			Dim eMailSetting As String
			Dim ftpSetting As String
			Dim dbConnSetting As String

			m_SettingFileName = Path.Combine(m_CommonConfigFolder, PROGRAM_SETTING_FILE)
			m_Logger.LogDebug(String.Format("m_CommonConfigFolder: {0} | settingFile: {1}", m_CommonConfigFolder, m_SettingFileName))
			If Not File.Exists(m_SettingFileName) Then
				MsgBox(String.Format("Die Datei konnte nicht geladen werden!{0}{1}", vbNewLine, m_SettingFileName))

				Return Nothing
			End If
			m_ProgSettingsXml = New SettingsXml(m_SettingFileName)
			pathSetting = PROGRAM_XML_SETTING_PATH
			scanningSetting = PROGRAM_XML_SETTING_SCANNING
			eMailSetting = PROGRAM_XML_SETTING_EMAIL
			ftpSetting = PROGRAM_XML_SETTING_FTP
			dbConnSetting = PROGRAM_XML_SETTING_DBCONNECTIONS

			Dim webservicedomain = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/webservicedomain", pathSetting))
			Dim fileServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/spsenterprisefolder", pathSetting))
			Dim cvscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvscanfolder", pathSetting))
			Dim reportscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportscanfolder", pathSetting))
			Dim scanparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/scanparserstartprogram", pathSetting))
			Dim emailparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/emailparserstartprogram", pathSetting))
			Dim parseemailattachment = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/parseemailattachment", pathSetting)), True)
			Dim cvlfoldertowatch = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertowatch", pathSetting))
			Dim cvlfoldertoarchive = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertoarchive", pathSetting))
			Dim cvlxmlfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlxmlfolder", pathSetting))
			Dim temporaryfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/temporaryfolder", pathSetting))

			Dim notificationintervalperiode = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiode", pathSetting))), 0)
			Dim notificationintervalperiodeforreport = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiodeforreport", pathSetting))), 0)
			Dim cvlparseasdemo As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlparseasdemo", pathSetting)), True)
			Dim asksendtocvlizer As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/asksendtocvlizer", pathSetting)), True)

			m_Logger.LogDebug(String.Format("Path is readed: {0}", m_SettingFileName))

			' scanning
			Dim scanFileFilter = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/FileFilter", scanningSetting))
			Dim scanDirectoryToListen = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/DirectoryToListen", scanningSetting))
			Dim processedScannedDocuments = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ProcessedScannedDocuments", scanningSetting))
			Dim softeksettingfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/softeksettingfolder", scanningSetting))
			Dim softekwaitduration = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/softekwaitduration", scanningSetting))
			Dim notifyOnDispose = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/NotifyOnDispose", scanningSetting)), True)
			Dim notifyEMailTo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/NotifyEMailTo", scanningSetting))
			m_Logger.LogDebug(String.Format("scanFileFilter: {0} | scanDirectoryToListen: {1} | processedScannedDocuments: {2} | softeksettingfolder: {3} | notifyOnDispose: {4} | notifyEMailTo: {5}",
																			scanFileFilter, scanDirectoryToListen, processedScannedDocuments, softeksettingfolder, notifyOnDispose, notifyEMailTo))


			Dim smtpNotificationServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpserver", eMailSetting))
			Dim SmtpNotificationPort = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpport", eMailSetting))
			Dim SmtpNotificationUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpuser", eMailSetting))
			Dim SmtpNotificationPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpassword", eMailSetting))
			Dim SmtpNotificationUseTLS = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpusetls", eMailSetting)), False)

			Dim smtpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpserver", eMailSetting))
			Dim SmtpPort = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpport", eMailSetting))
			Dim SmtpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpuser", eMailSetting))
			Dim SmtpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpassword", eMailSetting))
			Dim SmtpUseTLS = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpusetls", eMailSetting)), False)

			Dim stagingEMailFrom = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/stagingemailfrom", eMailSetting))
			Dim stagingEMailTo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/stagingemailto", eMailSetting))
			Dim notifyEMailfrom = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notifyemailfrom", eMailSetting))

			Dim reportmailbox = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportmailbox", eMailSetting))
			Dim reportEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailuser", eMailSetting))
			Dim reportEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailpassword", eMailSetting))
			Dim cvmailbox = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvmailbox", eMailSetting))
			'Dim cvEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailuser", eMailSetting))
			'Dim cvEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailpassword", eMailSetting))
			Dim deleteparsedemails = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/deleteparsedemails", eMailSetting)), False)

			m_Logger.LogDebug(String.Format("eMail is readed: {0}", m_SettingFileName))

			Dim ftpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpserver", ftpSetting))
			Dim ftpFolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpfolder", ftpSetting))
			Dim ftpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpuser", ftpSetting))
			Dim ftpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftppassword", ftpSetting))
			m_Logger.LogDebug(String.Format("FTP is readed: {0}", m_SettingFileName))


			Dim connstring_application = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_application", dbConnSetting))
			Dim connstring_cvlizer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_cvlizer", dbConnSetting))
			Dim connstring_systeminfo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_systeminfo", dbConnSetting))
			Dim connstring_scanjobs = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_scanjobs", dbConnSetting))
			Dim connstring_email = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_email", dbConnSetting))
			Dim connstring_wos = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_wos", dbConnSetting))
			Dim connstring_sppublicdata = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_sppublicdata", dbConnSetting))
			m_Logger.LogDebug(String.Format("Connections is readed: {0}", m_SettingFileName))


			m_Logger.LogDebug(String.Format("fileServer: {0} | cvscanfolder: {1} | reportscanfolder: {2} | notificationintervalperiode: {3} | notificationintervalperiodeforreport: {4}",
																			fileServer, cvscanfolder, reportscanfolder, notificationintervalperiode, notificationintervalperiodeforreport))
			m_Logger.LogDebug(String.Format("smtpServer: {0} >>> {1} | SmtpUser: {2} >>> {3} | stagingEMailFrom: {4} | stagingEMailTo: {5} | notifyEMailfrom: {6}",
																		smtpServer, SmtpPort, SmtpUser, SmtpPassword, stagingEMailFrom, stagingEMailTo, notifyEMailfrom))

			Try
				If String.IsNullOrWhiteSpace(webservicedomain) Then webservicedomain = "http://asmx.domain.com"
				result.WebserviceDomain = webservicedomain
				result.FileServerPath = fileServer
				result.CVScanFolder = cvscanfolder
				result.ReportScanFolder = reportscanfolder
				result.TemporaryFolder = temporaryfolder

				result.ScanParserStartProgram = If(Not File.Exists(scanparserstartprogram), String.Empty, scanparserstartprogram)
				result.EMailParserStartProgram = If(Not File.Exists(emailparserstartprogram), String.Empty, emailparserstartprogram)
				result.Notificationintervalperiode = notificationintervalperiode
				result.Notificationintervalperiodeforreport = notificationintervalperiodeforreport
				result.CVLFolderTOWatch = cvlfoldertowatch
				result.CVLFolderTOArchive = cvlfoldertoarchive
				result.CVLXMLFolder = cvlxmlfolder

				result.ScanFileFilter = scanFileFilter
				result.SoftekSettingFolder = softeksettingfolder
				result.SoftekSettingFolder = softeksettingfolder
				result.ScanDirectoryToListen = scanDirectoryToListen
				result.ProcessedScannedDocuments = processedScannedDocuments
				result.NotifyOnDisposeScanListing = notifyOnDispose
				result.NotifyEMailToScanJob = notifyEMailTo

				result.SmtpNotificationServer = smtpNotificationServer
				result.SmtpNotificationPort = SmtpNotificationPort
				result.SmtpNotificationUser = SmtpNotificationUser
				result.SmtpNotificationPassword = SmtpNotificationPassword
				result.SmtpNotificationUseTLS = SmtpNotificationUseTLS

				result.SmtpServer = smtpServer
				result.SmtpPort = SmtpPort
				result.SmtpUser = SmtpUser
				result.SmtpPassword = SmtpPassword
				result.StagingEMailFrom = stagingEMailFrom
				result.StagingEMailTo = stagingEMailTo
				result.NotifyEMailFrom = notifyEMailfrom
				result.SmtpUseTLS = SmtpUseTLS

				result.ReportMailbox = reportmailbox
				result.ReportEmailUser = reportEmailUser
				result.ReportEmailPassword = reportEmailPassword
				result.CVMailbox = cvmailbox
				'result.CVEmailUser = cvEmailUser
				'result.CVEmailPassword = cvEmailPassword
				result.FTPServer = ftpServer
				result.FTPFolder = ftpFolder
				result.FTPUser = ftpUser
				result.FTPPassword = ftpPassword

				result.ConnstringApplication = connstring_application
				result.ConnstringCVLizer = connstring_cvlizer
				result.ConnstringSysteminfo = connstring_systeminfo
				result.ConnstringScanjobs = connstring_scanjobs
				result.ConnstringEMail = connstring_email
				result.ConnstringWOS = connstring_wos
				result.ConnstringSPPublicData = connstring_sppublicdata
				result.CVLParseAsDemo = cvlparseasdemo
				result.ParseEMailAttachment = parseemailattachment
				result.AskSendToCVLizer = asksendtocvlizer
				result.DeleteParsedEMails = deleteparsedemails


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0} >>> {1}", m_SettingFileName, ex.ToString))

			End Try

			m_Logger.LogDebug(String.Format("file is readed: {0}", m_SettingFileName))

			Dim success = CreateTemporaryWorkingFolders(result)
			If Not success Then Return Nothing


			Return result

		End Function

		''' <summary>
		''' Creates the temporary working folders.
		''' </summary>
		Private Function CreateTemporaryWorkingFolders(ByVal settingFile As ProgramSettings) As Boolean

			Try
				If Not Directory.Exists(settingFile.CVLFolderTOWatch) Then Directory.CreateDirectory(settingFile.CVLFolderTOWatch)
				If Not Directory.Exists(settingFile.CVLFolderTOArchive) Then Directory.CreateDirectory(settingFile.CVLFolderTOArchive)
				If Not Directory.Exists(settingFile.CVLXMLFolder) Then Directory.CreateDirectory(settingFile.CVLXMLFolder)
				If Not Directory.Exists(settingFile.TemporaryFolder) Then Directory.CreateDirectory(settingFile.TemporaryFolder)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return False
			End Try

			Return True
		End Function


		''' <summary>
		''' Moves a pdf file to the processed scanned documents folder.
		''' </summary>
		''' <param name="originalPDFFilePath">The original pdf file path.</param>
		''' <param name="scannedDocumentsFolderPath">The scanned documents folder.</param>
		''' <param name="mandantGuid">The mandant guid.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function MovePDFToProcessedScannedDocuments(ByVal originalPDFFilePath As String, ByVal scannedDocumentsFolderPath As String, ByVal mandantGuid As String) As Boolean

			If Not Directory.Exists(scannedDocumentsFolderPath) Then
				m_Logger.LogWarning(String.Format("The configurated scanned documents folder {0} does not exist.", scannedDocumentsFolderPath))
				Return False
			End If

			Dim mandantSubFolderPath As String = Path.Combine(scannedDocumentsFolderPath, mandantGuid)
			Dim success As Boolean = True

			Try

				If Not Directory.Exists(mandantSubFolderPath) Then
					' Create the mandant sub directory if it does not exist.
					Directory.CreateDirectory(mandantSubFolderPath)
				End If

				Dim originalPDFFileInfo As New FileInfo(originalPDFFilePath)

				If (originalPDFFileInfo.Exists) Then
					' Copy the pdf to the mandant sub directory
					Dim newFilePath As String = MoveOrCopyFileToFolder(originalPDFFilePath, mandantSubFolderPath, False)

					If newFilePath Is Nothing Then
						success = False
					End If

				Else
					m_Logger.LogWarning(String.Format("The pdf file {0} does not exist.", originalPDFFilePath))
					success = False
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("MovePDFToProcessedScannedDocuments: {0}", ex.ToString))
				success = False
			End Try

			Return success

		End Function

		''' <summary>
		''' Moves the source pdf to the original pdf folder.
		''' </summary>
		''' <param name="filePath">The file full path.</param>
		''' <param name="destinationFolderPath">The destination folder full path.</param>
		''' <param name="doCopy">True for copying, false for moving.</param>
		''' <returns>Path to the moved or copied destination file or Nothing if this did not work.</returns>
		Private Function MoveOrCopyFileToFolder(ByVal filePath As String, ByVal destinationFolderPath As String, ByVal doCopy As Boolean) As String

			Dim newFilePath As String

			Try
				Dim fileInfo As FileInfo = New FileInfo(filePath)

				newFilePath = Path.Combine(destinationFolderPath, fileInfo.Name)

				Dim newFileInfo As New FileInfo(newFilePath)

				' Delete an existing file with the same name before move or copy operation.
				If (newFileInfo.Exists) Then
					newFileInfo.Delete()
				End If

				If (doCopy) Then
					' Copy the file.
					File.Copy(filePath, newFilePath, True)
				Else
					' Move the file.
					File.Move(filePath, newFilePath)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try

			Return newFilePath

		End Function

		Private Function DeleteSplittedFolder(ByVal workingFolderPath As String) As Boolean
			Dim success As Boolean = True

			Try
				If Directory.Exists(workingFolderPath) Then
					For Each _file As String In Directory.GetFiles(workingFolderPath)
						File.Delete(_file)
					Next
					For Each _folder As String In Directory.GetDirectories(workingFolderPath)

						DeleteSplittedFolder(_folder)
					Next
					System.IO.Directory.Delete(workingFolderPath, True)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Error: can not remove folder {0} | {1}.", workingFolderPath, ex.ToString))
				success = False

			End Try


			Return success

		End Function

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Private Function imageToByteArray(imageIn As System.Drawing.Image) As Byte()

			Dim ms As MemoryStream = New MemoryStream()
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)

			Return ms.ToArray()

		End Function

		''' <summary>
		''' Replaces a missing object with another object.
		''' </summary>
		Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

			If (obj Is Nothing) Then
				Return replacementObject
			Else
				Return obj
			End If

		End Function


		Private Function GetSafeStringFromXElement(ByVal xelment As XElement) As String

			If xelment Is Nothing Then
				Return String.Empty
			Else

				Return xelment.Value
			End If

		End Function

		Private Function FromBase64(ByVal sText As String) As String
			' Base64-String zunächst in ByteArray konvertieren
			Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

			' ByteArray in String umwandeln
			Return System.Text.Encoding.Default.GetString(nBytes)
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


#End Region



#Region "Helper class"

		Private Class FileDirectoryData
			Public Property Filename As String
			Public Property FileDate As DateTime?

		End Class

		Private Class ProfileLocalViewData
			Inherits Main.Notify.SPApplicationWebService.CVLizerProfileDataDTO

			Public ReadOnly Property Fullname As String
				Get
					Return String.Format("{0} {1} {2}", GenderLabel, FirstName, LastName)
				End Get
			End Property

			Public ReadOnly Property AgeViewData As Integer?
				Get
					Dim birthDate = DateOfBirth.GetValueOrDefault(Now)
					Dim years As Integer = DateTime.Now.Year - birthDate.Year

					birthDate = birthDate.AddYears(years)

					' Subtract another year if its a day before the the birth day
					If (DateTime.Today.CompareTo(birthDate) < 0) Then
						years = years - 1
					End If


					Return years
				End Get
			End Property

			Public ReadOnly Property ApplicantAgeViewData As String
				Get
					Dim birthDate = DateOfBirth.GetValueOrDefault(Now)
					Dim years As Integer = DateTime.Now.Year - birthDate.Year

					birthDate = birthDate.AddYears(years)

					' Subtract another year if its a day before the the birth day
					If (DateTime.Today.CompareTo(birthDate) < 0) Then
						years = years - 1
					End If


					Return String.Format("({0}) {1:dd.MM.yyyy}", years, DateOfBirth)
				End Get
			End Property

		End Class

		Private Sub LoadCVLDomains()
			Dim result As String = String.Empty
			Dim m_customerID As String = ""
			Dim obj = New SP.ApplicationMng.CVLizer.Import.CVLizer

			result = obj.LoadCVLDomains()
			Process.Start(result)

		End Sub

		Private Sub ShowRecivedXML()
			Dim data = SelectedProfileViewData

			If data Is Nothing Then
				SplashScreenManager.CloseForm(False)

				Return
			End If
			Dim EMailData = m_EMailDatabaseAccess.LoadAssigendApplicationEMailData(data.Customer_ID, data.ApplicationID, Nothing)
			If EMailData Is Nothing OrElse EMailData.ID.GetValueOrDefault(0) = 0 OrElse String.IsNullOrWhiteSpace(EMailData.EMLFilename) Then Return

			Dim filename As String = Path.GetFileName(EMailData.EMLFilename.ToLower)
			Dim xmlPath = Path.Combine(m_SettingFile.CVLXMLFolder, data.Customer_ID, filename.Replace(".eml", ".xml"))
			Process.Start("explorer.exe", "/select," & xmlPath)

		End Sub

		Private Sub ShowRecivedEMail()
			Dim data = SelectedProfileViewData

			If data Is Nothing Then
				SplashScreenManager.CloseForm(False)

				Return
			End If
			Dim EMailData = m_EMailDatabaseAccess.LoadAssigendApplicationEMailData(data.Customer_ID, data.ApplicationID, Nothing)
			If EMailData Is Nothing OrElse EMailData.ID.GetValueOrDefault(0) = 0 OrElse String.IsNullOrWhiteSpace(EMailData.EMLFilename) Then Return


			Dim eMailPath = Path.Combine(m_SettingFile.CVLFolderTOArchive, data.Customer_ID, EMailData.EMLFilename)
			Process.Start("explorer.exe", "/select," & eMailPath)

		End Sub

		Private Sub deDate_EditValueChanged(sender As Object, e As EventArgs)
			LoadSelectedTabData()
		End Sub

		Private Sub LoadSelectedTabData()

			xtabCVLDetails.Enabled = False
			xtabCV.CustomHeaderButtons(0).Enabled = False
			xtabCV.CustomHeaderButtons(1).Enabled = False
			xtabCV.CustomHeaderButtons(2).Enabled = False

			If xtabCV.SelectedTabPage Is xtabCVLData Then
				LoadCVLProfileData()
				xtabCVLDetails.Enabled = True

				xtabCV.CustomHeaderButtons(0).Enabled = True
				xtabCV.CustomHeaderButtons(1).Enabled = True
				xtabCV.CustomHeaderButtons(2).Enabled = True

			ElseIf xtabCV.SelectedTabPage Is xtabDocScan Then
				LoadDocScanData()
				LoadNotScanedFiles()

			ElseIf xtabCV.SelectedTabPage Is xtabMailNotificationAdvisorLogin Then
				LoadMailNotificationData()
				LoadAdvisorData()
				LoadAdvisorMonthlyData()

				xtabCV.CustomHeaderButtons(0).Enabled = True


			ElseIf xtabCV.SelectedTabPage Is xtabLOG Then
				LoadXMLFileData()

			Else
				Return


			End If

		End Sub

		Private Sub xtabCV_CustomHeaderButtonClick(sender As Object, e As CustomHeaderButtonEventArgs) Handles xtabCV.CustomHeaderButtonClick

			Select Case e.Button.Tag
				Case 1
					LoadSelectedTabData()

				Case 2
					If xtabCV.SelectedTabPage Is xtabMailNotificationAdvisorLogin Then
						SearchGivenEmailAddress
					ElseIf xtabCV.SelectedTabPage Is xtabCVLData Then
						ShowRecivedEMail()
					End If

				Case 3
					ShowRecivedXML()

				Case 4
					LoadCVLDomains()

				Case 5
					createXLSFile()


				Case Else
					Return

			End Select

		End Sub

		Private Sub SearchGivenEmailAddress()


		End Sub

		Private Sub CreateXLSFile()

			Dim filename = Path.GetRandomFileName
			filename = Path.ChangeExtension(filename, "xlsx")
			filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename)

			Dim advOptions As DevExpress.XtraPrinting.XlsxExportOptionsEx = New DevExpress.XtraPrinting.XlsxExportOptionsEx()
			advOptions.AllowSortingAndFiltering = DevExpress.Utils.DefaultBoolean.False
			advOptions.ShowColumnHeaders = DevExpress.Utils.DefaultBoolean.True
			advOptions.ShowGridLines = False
			advOptions.ApplyFormattingToEntireColumn = DevExpress.Utils.DefaultBoolean.True
			advOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False
			advOptions.ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.False

			If xtabCV.SelectedTabPage Is xtabDocScan Then
				'advOptions.SheetName = "Scan-Daten"
				'grdDocScan.ExportToXlsx(filename, advOptions)

				gvDocScan.OptionsPrint.ExpandAllGroups = True
				gvDocScan.OptionsPrint.ExpandAllDetails = True
				gvDocScan.ShowPrintPreview()

				Return


			ElseIf xtabCV.SelectedTabPage Is xtabMailNotificationAdvisorLogin Then
				If tpAdvisorLogins.SelectedPage Is tpDailylogin Then
					'advOptions.SheetName = "Login-Daten Täglich"
					'grdAdvisorLogin.ExportToXlsx(filename, advOptions)

					gvAdvisorLogin.OptionsPrint.ExpandAllGroups = True
					gvAdvisorLogin.OptionsPrint.ExpandAllDetails = True
					gvAdvisorLogin.ShowPrintPreview()

					Return

				Else
					'advOptions.ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.True
					'advOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.True
					'advOptions.SheetName = "Login-Daten Monatlich"
					'grdMonthlyLogins.ExportToXlsx(filename, advOptions)

					gvMontlyLogins.OptionsPrint.ExpandAllGroups = True
					gvMontlyLogins.OptionsPrint.ExpandAllDetails = True
					gvMontlyLogins.ShowPrintPreview()

					Return

				End If

			ElseIf xtabCV.SelectedTabPage Is xtabLOG Then
				'advOptions.ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.True
				'advOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.True
				'advOptions.SheetName = "CVL Parsings Monatlich"
				'grdCVLXMLFiles.ExportToXlsx(filename, advOptions)

				gvCVLXMLFiles.OptionsPrint.ExpandAllGroups = True
				gvCVLXMLFiles.OptionsPrint.ExpandAllDetails = True
				gvCVLXMLFiles.ShowPrintPreview()

				Return

			Else
					Return

			End If
			Process.Start("explorer.exe", "/select," & filename)
			Process.Start(filename)

		End Sub

		Private Sub OngvAdvisorLogin_CustomDrawRowFooter(sender As Object, e As RowObjectCustomDrawEventArgs)

			'e.Cache.FillRectangle(e.Cache.GetGradientBrush(e.Bounds, Color.Red, Color.Maroon, System.Drawing.Drawing2D.LinearGradientMode.Horizontal), e.Bounds)
			'Prevent default painting
			e.Handled = True

		End Sub

		Private Sub OngvAdvisorLogin_CustomDrawRowFooterCell(sender As Object, e As FooterCellCustomDrawEventArgs)

			'e.Bounds.Inflate(-5, -5)
			'e.Appearance.ForeColor = Color.Teal
			'e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			'e.Appearance.FontSizeDelta = 1
			'e.DefaultDraw()
			'e.Cache.DrawRectangle(e.Cache.GetPen(Color.DarkOliveGreen, 5), e.Bounds)
			'e.Handled = True

		End Sub

#End Region




	End Class


End Namespace




