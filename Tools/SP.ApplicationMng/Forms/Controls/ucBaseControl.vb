Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Initialization
Imports SP.DatabaseAccess.Employee
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

'Namespace UI

Public Class ucBaseControl
	Inherits DevExpress.XtraEditors.XtraUserControl


#Region "private consts"

	Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
	Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
	Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
	Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"
	Private Const PROGRAM_XML_SETTING_EMAIL As String = "Settings/EMailSetting"
	Private Const PROGRAM_XML_SETTING_FTP As String = "Settings/FTPSetting"

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
	Protected m_ESDataAccess As IESDatabaseAccess

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager
	Protected m_ProgSettingsXml As SettingsXml

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
	Protected m_SuppressUIEvents As Boolean = False

	Protected m_CommonConfigFolder As String
	Protected m_ApplicationUtilWebServiceUri As String

#End Region

#Region "public consts"

	Public Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx" ' "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

#End Region


#Region "Public Properties"

	Public Property ProgSettingData As ProgramSettings


#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()
		m_UtilityUI = New UtilityUI

		If ProgSettingData Is Nothing Then
			m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)
			ProgSettingData = ReadSettingFile()
			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", ProgSettingData.WebserviceDomain, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
		End If

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

		m_ESDataAccess = New DatabaseAccess.ES.ESDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		TranslateControls()

	End Sub

	''' <summary>
	''' Loads data.
	''' </summary>
	''' <param name="esData">The es data.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Overridable Function LoadData(ByVal esData As ESMasterData) As Boolean
		Return True
	End Function

	''' <summary>
	''' Merges ES master data.
	''' </summary>
	''' <param name="esData">The es data.</param>
	Public Overridable Sub MergeESMasterData(ByVal esData As ESMasterData)
	End Sub

	''' <summary>
	''' Resets the control.
	''' </summary>
	Public Overridable Sub Reset()
		' Do not make this method abstract because the WinForms designer does not like that.
		'Throw New NotImplementedException("The methods must be overriden by subclass.")
	End Sub

	''' <summary>
	''' Validated data.
	''' </summary>
	Public Overridable Function ValidateData() As Boolean
		Return True
	End Function

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



#Region "Helpers"

	Private Function ReadSettingFile() As ProgramSettings
		Dim result As New ProgramSettings
		Dim pathSetting As String
		Dim eMailSetting As String
		Dim ftpSetting As String
		Dim dbConnSetting As String

		Dim settingFile = Path.Combine(m_CommonConfigFolder, PROGRAM_SETTING_FILE)
		m_Logger.LogDebug(String.Format("m_CommonConfigFolder: {0} | settingFile: {1}", m_CommonConfigFolder, settingFile))

		m_ProgSettingsXml = New SettingsXml(settingFile)
		pathSetting = PROGRAM_XML_SETTING_PATH
		eMailSetting = PROGRAM_XML_SETTING_EMAIL
		ftpSetting = PROGRAM_XML_SETTING_FTP
		dbConnSetting = PROGRAM_XML_SETTING_DBCONNECTIONS

		Dim webservicedomain = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/webservicedomain", pathSetting))
		Dim fileServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/spsenterprisefolder", pathSetting))
		Dim cvscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvscanfolder", pathSetting))
		Dim reportscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportscanfolder", pathSetting))
		Dim scanparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/scanparserstartprogram", pathSetting))
		Dim emailparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/emailparserstartprogram", pathSetting))

		Dim notificationintervalperiode = ParseToDouble(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiode", pathSetting)), 0)
		Dim notificationintervalperiodeforreport = ParseToDouble(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiodeforreport", pathSetting)), 0)
		Dim cvlfoldertowatch = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertowatch", pathSetting))
		Dim cvlfoldertoarchive = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertoarchive", pathSetting))
		Dim cvlxmlfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlxmlfolder", pathSetting))
		Dim temporaryfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/temporaryfolder", pathSetting))

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

		Dim reportEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailuser", eMailSetting))
		Dim reportEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailpassword", eMailSetting))
		'Dim cvEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailuser", eMailSetting))
		'Dim cvEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailpassword", eMailSetting))

		Dim ftpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpserver", ftpSetting))
		Dim ftpFolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpfolder", ftpSetting))
		Dim ftpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpuser", ftpSetting))
		Dim ftpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftppassword", ftpSetting))


		Dim connstring_application = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_application", dbConnSetting))
		Dim connstring_cvlizer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_cvlizer", dbConnSetting))
		Dim connstring_systeminfo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_systeminfo", dbConnSetting))
		Dim connstring_scanjobs = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_scanjobs", dbConnSetting))
		Dim connstring_email = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_email", dbConnSetting))
		Dim cvlparseasdemo As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlparseasdemo", pathSetting)), True)
		Dim asksendtocvlizer As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/asksendtocvlizer", pathSetting)), True)


		m_Logger.LogDebug(String.Format("fileServer: {0} | cvscanfolder: {1} | reportscanfolder: {2} | notificationintervalperiode: {3} | | notificationintervalperiodeforreport: {4}",
																			fileServer, cvscanfolder, reportscanfolder, notificationintervalperiode, notificationintervalperiodeforreport))

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

		result.SmtpNotificationServer = smtpNotificationServer
		result.SmtpNotificationUser = SmtpNotificationUser
		result.SmtpNotificationPassword = SmtpNotificationPassword
		result.SmtpNotificationPort = SmtpNotificationPort
		result.SmtpNotificationUseTLS = SmtpNotificationUseTLS

		result.SmtpServer = smtpServer
		result.SmtpUser = SmtpUser
		result.SmtpPassword = SmtpPassword
		result.SmtpPort = SmtpPort
		result.SmtpUseTLS = SmtpUseTLS

		result.ReportEmailUser = reportEmailUser
		result.ReportEmailPassword = reportEmailPassword
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
		result.CVLParseAsDemo = cvlparseasdemo
		result.AskSendToCVLizer = asksendtocvlizer

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


End Class

'End Namespace
