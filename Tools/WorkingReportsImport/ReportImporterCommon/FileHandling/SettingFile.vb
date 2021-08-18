

Imports System.IO
Imports ReportImporterCommon.CommonXmlUtility
Imports ReportImporterCommon.Logging

Public Class SettingFile


#Region "Private consts"

	''' <summary>
	''' Maximal database text length.
	''' </summary>
	Public Const MAX_TEXT_LENGTH As Integer = 255

	Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
	Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
	Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
	Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"
	Private Const PROGRAM_XML_SETTING_EMAIL As String = "Settings/EMailSetting"
	Private Const PROGRAM_XML_SETTING_SCANNING As String = "Settings/Scanning"
	Private Const PROGRAM_XML_SETTING_FTP As String = "Settings/FTPSetting"

#End Region

	Private Shared m_logger As ILogger = New Logger()

	Private m_CommonConfigFolder As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml
	Private m_ProgSettingsXml As SettingsXml

	Private m_SettingFileName As String
	Private m_SettingFile As ProgramSettings

	Public Sub New()

		m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)

		'm_SettingFile = ReadSettingFile()

		'If m_SettingFile Is Nothing Then
		'	m_logger.LogError(String.Format("settingfile could not be readed!: {0}", m_SettingFileName))
		'End If

	End Sub

	Public ReadOnly Property SettingFileValues() As ProgramSettings
		Get
			Return m_SettingFile
		End Get
	End Property


	Public Function ReadSettingFile() As ProgramSettings
		Dim result As New ProgramSettings
		Dim pathSetting As String
		Dim eMailSetting As String
		Dim scanningSetting As String
		Dim ftpSetting As String
		Dim dbConnSetting As String

		m_SettingFileName = Path.Combine(m_CommonConfigFolder, PROGRAM_SETTING_FILE)
		m_logger.LogDebug(String.Format("m_CommonConfigFolder: {0} | settingFile: {1}", m_CommonConfigFolder, m_SettingFileName))
		If Not File.Exists(m_SettingFileName) Then
			MsgBox(String.Format("Die Datei konnte nicht geladen werden!{0}{1}", vbNewLine, m_SettingFileName))

			Return Nothing
		End If
		result.CurrentSettingFilename = m_SettingFileName
		m_ProgSettingsXml = New SettingsXml(m_SettingFileName)
		pathSetting = PROGRAM_XML_SETTING_PATH
		eMailSetting = PROGRAM_XML_SETTING_EMAIL
		scanningSetting = PROGRAM_XML_SETTING_SCANNING
		ftpSetting = PROGRAM_XML_SETTING_FTP
		dbConnSetting = PROGRAM_XML_SETTING_DBCONNECTIONS

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

		Dim smtpNotificationServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpserver", eMailSetting))
		Dim SmtpNotificationPort = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpport", eMailSetting))
		Dim SmtpNotificationUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpuser", eMailSetting))
		Dim SmtpNotificationPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpassword", eMailSetting))
		Dim SmtpNotificationUseTLS = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/Notification/smtpusetls", eMailSetting)), False)



		Dim notificationintervalperiode = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiode", pathSetting))), 0)
		Dim notificationintervalperiodeforreport = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiodeforreport", pathSetting))), 0)
		Dim cvlparseasdemo As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlparseasdemo", pathSetting)), True)
		Dim asksendtocvlizer As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/asksendtocvlizer", pathSetting)), True)


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
		Dim cvEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailuser", eMailSetting))
		Dim cvEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailpassword", eMailSetting))

		' scanning
		Dim scanFileFilter = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/FileFilter", scanningSetting))
		Dim scanDirectoryToListen = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/DirectoryToListen", scanningSetting))
		Dim processedScannedDocuments = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ProcessedScannedDocuments", scanningSetting))
		Dim softeksettingfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/softeksettingfolder", scanningSetting))
		Dim softekwaitduration = Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/softekwaitduration", scanningSetting)))
		Dim notifyOnDispose = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/NotifyOnDispose", scanningSetting)), True)
		Dim notifyEMailTo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/NotifyEMailTo", scanningSetting))
		m_logger.LogDebug(String.Format("scanFileFilter: {0} | scanDirectoryToListen: {1} | processedScannedDocuments: {2} | softeksettingfolder: {3} | notifyOnDispose: {4} | notifyEMailTo: {5}",
																			scanFileFilter, scanDirectoryToListen, processedScannedDocuments, softeksettingfolder, notifyOnDispose, notifyEMailTo))


		Dim ftpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpserver", ftpSetting))
		Dim ftpFolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpfolder", ftpSetting))
		Dim ftpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpuser", ftpSetting))
		Dim ftpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftppassword", ftpSetting))
		m_logger.LogDebug(String.Format("FTP is readed: {0}", m_SettingFileName))


		Dim connstring_application = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_application", dbConnSetting))
		Dim connstring_cvlizer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_cvlizer", dbConnSetting))
		Dim connstring_systeminfo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_systeminfo", dbConnSetting))
		Dim connstring_scanjobs = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_scanjobs", dbConnSetting))
		Dim connstring_email = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_email", dbConnSetting))
		Dim connstring_wos = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_wos", dbConnSetting))
		m_logger.LogDebug(String.Format("Connections is readed: {0}", m_SettingFileName))


		m_logger.LogDebug(String.Format("fileServer: {0} | cvscanfolder: {1} | reportscanfolder: {2} | notificationintervalperiode: {3} | notificationintervalperiodeforreport: {4}",
																			fileServer, cvscanfolder, reportscanfolder, notificationintervalperiode, notificationintervalperiodeforreport))
		m_logger.LogDebug(String.Format("smtpServer: {0} >>> {1} | SmtpUser: {2} >>> {3} | stagingEMailFrom: {4} | notifyEMailfrom: {5} | stagingEMailTo: {6}",
																		smtpServer, SmtpPort, SmtpUser, SmtpPassword, stagingEMailFrom, notifyEMailfrom, stagingEMailTo))

		Try
			result.FileServerPath = fileServer
			result.CVScanFolder = cvscanfolder
			result.ReportScanFolder = reportscanfolder
			result.TemporaryFolder = temporaryfolder
			result.SoftekSettingFolder = softeksettingfolder
			result.SoftekWaitDuration = softekwaitduration

			result.ScanParserStartProgram = If(Not File.Exists(scanparserstartprogram), String.Empty, scanparserstartprogram)
			result.EMailParserStartProgram = If(Not File.Exists(emailparserstartprogram), String.Empty, emailparserstartprogram)
			result.Notificationintervalperiode = notificationintervalperiode
			result.Notificationintervalperiodeforreport = notificationintervalperiodeforreport
			result.CVLFolderTOWatch = cvlfoldertowatch
			result.CVLFolderTOArchive = cvlfoldertoarchive
			result.CVLXMLFolder = cvlxmlfolder

			result.scanFileFilter = scanFileFilter
			result.scanDirectoryToListen = scanDirectoryToListen
			result.processedScannedDocuments = processedScannedDocuments
			result.notifyOnDisposeScanListing = notifyOnDispose
			result.notifyEMailToScanJob = notifyEMailTo

			result.SmtpNotificationServer = smtpNotificationServer
			result.SmtpNotificationUser = SmtpNotificationUser
			result.SmtpNotificationPassword = SmtpNotificationPassword
			result.SmtpNotificationPort = SmtpNotificationPort
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
			result.CVEmailUser = cvEmailUser
			result.CVEmailPassword = cvEmailPassword
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
			result.CVLParseAsDemo = cvlparseasdemo
			result.ParseEMailAttachment = parseemailattachment
			result.AskSendToCVLizer = asksendtocvlizer


		Catch ex As Exception
			m_logger.LogError(String.Format("{0} >>> {1}", m_SettingFileName, ex.ToString))

			Return Nothing
		End Try

		m_logger.LogDebug(String.Format("file is readed: {0}", m_SettingFileName))

		m_SettingFile = result

		Return result

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

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If
	End Function


End Class


