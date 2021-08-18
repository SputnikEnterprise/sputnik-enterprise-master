
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports SP.ApplicationMng.CVLizer.DataObject
Imports SP.Infrastructure.Logging
Imports SPProgUtility.CommonXmlUtility
Imports System.Xml
Imports System.Xml.XPath

Public Class frmStartProgram


#Region "private consts"

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
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_CommonConfigFolder As String
	Private m_SputnikFileServer As String
	Private m_ScanParserProgram As String
	Private m_EMailParserProgram As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml
	Private m_ProgSettingsXml As SettingsXml

	Private m_SettingFileName As String
	Private m_SettingFile As ProgramSettings

#End Region


	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
		WindowsFormsSettings.AllowAutoFilterConditionChange = DefaultBoolean.False

		m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)

		'Dim test = ReadMunicipalitiesDataFromXMLFile()
		m_SettingFile = ReadSettingFile()
		If m_SettingFile Is Nothing Then End

		m_ScanParserProgram = m_SettingFile.ScanParserStartProgram
		m_EMailParserProgram = m_SettingFile.EMailParserStartProgram

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Dim ipAddress As String
		Dim osVersion As String
		Dim sputnikEnvironment As Boolean = False

		osVersion = My.Computer.Info.OSVersion
		ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(Function(a As IPAddress) Not a.IsIPv6LinkLocal AndAlso Not a.IsIPv6Multicast AndAlso Not a.IsIPv6SiteLocal).First().ToString()
		If ipAddress.Contains("ipaddress") OrElse ipAddress.Contains("ipaddress") Then
			sputnikEnvironment = True
		End If
		m_Logger.LogInfo(String.Format("your local IP: {0}", ipAddress))
#If Not DEBUG Then
		btnStartCVLizer.Enabled = sputnikEnvironment
		btnStartEMailParsing.Enabled = sputnikEnvironment
		btnReadXMLForCommunity.Enabled = sputnikEnvironment
		btnStartScan.Enabled = sputnikEnvironment
#End If
		btnReadXMLForCommunity.Visible = sputnikEnvironment

		lblSettingfilename.Text = m_SettingFileName
		lblLocalIPAddress.Text = ipAddress


	End Sub

	Private Sub frmTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load


	End Sub

	Private Sub btnStartScan_Click(sender As Object, e As EventArgs) Handles btnStartScan.Click
		If Not String.IsNullOrWhiteSpace(m_ScanParserProgram) Then Process.Start(m_ScanParserProgram)
	End Sub

	Private Sub btnStartCVLizer_Click(sender As Object, e As EventArgs) Handles btnStartCVLizer.Click
		Dim frm = New CVLizer.frmCVLizer()

		frm.Show()
		frm.BringToFront()

	End Sub

	Private Sub btnStartEMailParsing_Click(sender As Object, e As EventArgs) Handles btnStartEMailParsing.Click
		If Not String.IsNullOrWhiteSpace(m_EMailParserProgram) Then
			Process.Start(m_EMailParserProgram)
		Else
			Try
				If m_SettingFile Is Nothing Then Return
				Dim frmEMail = New UI.frmCVWatcher(m_SettingFile)
				frmEMail.Show()
				frmEMail.BringToFront()

			Catch ex As Exception

			End Try

		End If

	End Sub

	Private Sub OnbtnReadXMLForCommunity_Click(sender As Object, e As EventArgs) Handles btnReadXMLForCommunity.Click
		Dim obj = New spPublicData(m_SettingFile, "")

		Dim result = obj.ReadMunicipalitiesDataFromXMLFile()
		If Not result Then
			m_Logger.LogError("result was not successfull!")
		End If

	End Sub


	Private Sub btnStartNotifier_Click(sender As Object, e As EventArgs) Handles btnStartNotifier.Click
		Dim m_init As SP.Infrastructure.Initialization.InitializeClass = CreateInitialData(0, 0)
		Dim frmNotifier = New SP.Main.Notify.UI.frmNotify(m_init)

		Dim success = frmNotifier.LoadNotify
		frmNotifier.Show()
		frmNotifier.BringToFront()

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

			result.scanFileFilter = scanFileFilter
			result.SoftekSettingFolder = softeksettingfolder
			result.ScanDirectoryToListen = scanDirectoryToListen
			result.processedScannedDocuments = processedScannedDocuments
			result.notifyOnDisposeScanListing = notifyOnDispose
			result.notifyEMailToScanJob = notifyEMailTo

			result.SmtpNotificationServer = smtpNotificationServer
			result.SmtpNotificationPort = SmtpNotificationPort
			result.SmtpNotificationUser = SmtpNotificationUser
			result.SmtpNotificationPassword = SmtpNotificationPassword
			result.SmtpNotificationUseTLS = SmtpNotificationUseTLS

			result.SmtpServer = smtpServer
			result.SmtpPort = SmtpPort
			result.SmtpUser = SmtpUser
			result.SmtpPassword = SmtpPassword
			result.SmtpUseTLS = SmtpUseTLS

			result.StagingEMailFrom = stagingEMailFrom
			result.StagingEMailTo = stagingEMailTo
			result.NotifyEMailFrom = notifyEMailfrom

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

	Private Function ReadMunicipalitiesDataFromXMLFile() As Boolean
		Dim i As Integer = 0
		m_SettingFileName = Path.Combine("<your path>\XML", "your file.xml")

		Dim searchCommunity As String = "Bronschhofen"

		Dim wcCanton As New List(Of CantonData)
		Dim wcDistrict As New List(Of DistrictData)
		Dim wcMunicipality As New List(Of MunicipalityData)


		Dim reader As XmlReader = XmlReader.Create(m_SettingFileName)
		Try

			i = 0
			While (Not reader.EOF)

				If reader.Name <> "canton" Then
					reader.ReadToFollowing("canton")
				End If

				If Not reader.EOF Then
					Dim xCanton As XElement = XElement.ReadFrom(reader)
					Dim ns As XNamespace = xCanton.GetDefaultNamespace()
					Dim canton As New CantonData

					If Not xCanton.Element(ns + "cantonId") Is Nothing Then canton.cantonId = CType(xCanton.Element(ns + "cantonId"), Integer)

					If Not xCanton.Element(ns + "cantonAbbreviation") Is Nothing Then canton.cantonAbbreviation = xCanton.Element(ns + "cantonAbbreviation")
					If Not xCanton.Element(ns + "cantonLongName") Is Nothing Then canton.cantonLongName = xCanton.Element(ns + "cantonLongName")
					If Not xCanton.Element(ns + "cantonDateOfChange") Is Nothing Then canton.cantonDateOfChange = CType(xCanton.Element(ns + "cantonDateOfChange"), DateTime)

					wcCanton.Add(canton)

				End If

			End While

			reader = XmlReader.Create(m_SettingFileName)
			While (Not reader.EOF)

				If reader.Name <> "district" Then
					reader.ReadToFollowing("district")
				End If

				If Not reader.EOF Then
					Dim xDistrict As XElement = XElement.ReadFrom(reader)
					Dim ns As XNamespace = xDistrict.GetDefaultNamespace()
					Dim district As New DistrictData

					If Not xDistrict.Element(ns + "districtHistId") Is Nothing Then district.districtHistId = CType(xDistrict.Element(ns + "districtHistId"), Integer)
					If Not xDistrict.Element(ns + "cantonId") Is Nothing Then district.cantonId = CType(xDistrict.Element(ns + "cantonId"), Integer)
					If Not xDistrict.Element(ns + "districtId") Is Nothing Then district.districtId = CType(xDistrict.Element(ns + "districtId"), Integer)
					If Not xDistrict.Element(ns + "districtEntryMode") Is Nothing Then district.districtEntryMode = CType(xDistrict.Element(ns + "districtEntryMode"), Integer)
					If Not xDistrict.Element(ns + "districtAdmissionNumber") Is Nothing Then district.districtAdmissionNumber = CType(xDistrict.Element(ns + "districtAdmissionNumber"), Integer)
					If Not xDistrict.Element(ns + "districtAdmissionMode") Is Nothing Then district.districtAdmissionMode = CType(xDistrict.Element(ns + "districtAdmissionMode"), Integer)

					If Not xDistrict.Element(ns + "districtLongName") Is Nothing Then district.districtLongName = xDistrict.Element(ns + "districtLongName")
					If Not xDistrict.Element(ns + "districtShortName") Is Nothing Then district.districtShortName = xDistrict.Element(ns + "districtShortName")

					If Not xDistrict.Element(ns + "districtAdmissionDate") Is Nothing Then district.districtAdmissionDate = CType(xDistrict.Element(ns + "districtAdmissionDate"), DateTime)
					If Not xDistrict.Element(ns + "districtDateOfChange") Is Nothing Then district.districtDateOfChange = CType(xDistrict.Element(ns + "districtDateOfChange"), DateTime)


					wcDistrict.Add(district)

				End If

			End While

			reader = XmlReader.Create(m_SettingFileName)
			While (Not reader.EOF)

				If reader.Name <> "municipality" Then
					reader.ReadToFollowing("municipality")
				End If

				If Not reader.EOF Then
					Dim xMunicipality As XElement = XElement.ReadFrom(reader)
					Dim ns As XNamespace = xMunicipality.GetDefaultNamespace()
					Dim wcproduction As New MunicipalityData

					If Not xMunicipality.Element(ns + "historyMunicipalityId") Is Nothing Then wcproduction.HistoricNumber = CType(xMunicipality.Element(ns + "historyMunicipalityId"), Integer)
					If Not xMunicipality.Element(ns + "districtHistId") Is Nothing Then wcproduction.BezirkNumber = CType(xMunicipality.Element(ns + "districtHistId"), Integer)
					If Not xMunicipality.Element(ns + "municipalityId") Is Nothing Then wcproduction.BFSNumber = CType(xMunicipality.Element(ns + "municipalityId"), Integer)
					If Not xMunicipality.Element(ns + "municipalityEntryMode") Is Nothing Then wcproduction.municipalityEntryMode = CType(xMunicipality.Element(ns + "municipalityEntryMode"), Integer)
					If Not xMunicipality.Element(ns + "municipalityStatus") Is Nothing Then wcproduction.municipalityStatus = CType(xMunicipality.Element(ns + "municipalityStatus"), Integer)

					If Not xMunicipality.Element(ns + "municipalityAdmissionNumber") Is Nothing Then wcproduction.municipalityAdmissionNumber = CType(xMunicipality.Element(ns + "municipalityAdmissionNumber"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAdmissionMode") Is Nothing Then wcproduction.municipalityAdmissionMode = CType(xMunicipality.Element(ns + "municipalityAdmissionMode"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAbolitionNumber") Is Nothing Then wcproduction.municipalityAbolitionNumber = CType(xMunicipality.Element(ns + "municipalityAbolitionNumber"), Integer)
					If Not xMunicipality.Element(ns + "municipalityAbolitionMode") Is Nothing Then wcproduction.municipalityAbolitionMode = CType(xMunicipality.Element(ns + "municipalityAbolitionMode"), Integer)

					If Not xMunicipality.Element(ns + "cantonAbbreviation") Is Nothing Then wcproduction.Canton = xMunicipality.Element(ns + "cantonAbbreviation")
					If Not xMunicipality.Element(ns + "municipalityShortName") Is Nothing Then wcproduction.Bez_DE = xMunicipality.Element(ns + "municipalityShortName")
					If Not xMunicipality.Element(ns + "municipalityLongName") Is Nothing Then wcproduction.municipalityLongName = xMunicipality.Element(ns + "municipalityLongName")

					If Not xMunicipality.Element(ns + "municipalityAdmissionDate") Is Nothing Then wcproduction.municipalityAdmissionDate = CType(xMunicipality.Element(ns + "municipalityAdmissionDate"), DateTime)
					If Not xMunicipality.Element(ns + "municipalityAbolitionDate") Is Nothing Then wcproduction.municipalityAbolitionDate = CType(xMunicipality.Element(ns + "municipalityAbolitionDate"), DateTime)
					If Not xMunicipality.Element(ns + "municipalityDateOfChange") Is Nothing Then wcproduction.municipalityDateOfChange = CType(xMunicipality.Element(ns + "municipalityDateOfChange"), DateTime)

					If wcproduction.Bez_DE.ToLower <> wcproduction.municipalityLongName.ToLower Then
					End If

					If wcproduction.Bez_DE.ToLower = searchCommunity.ToLower OrElse wcproduction.municipalityLongName.ToLower = searchCommunity.ToLower Then
						'Exit While
					End If

					wcMunicipality.Add(wcproduction)

				End If

			End While


		Catch ex As Exception

		End Try

	End Function

	Private Function ReadCommunitySettingFile() As Boolean
		Dim success As Boolean = True

		Dim result As New ProgramSettings
		Dim pathSetting As String
		Dim eMailSetting As String
		Dim ftpSetting As String
		Dim dbConnSetting As String

		m_SettingFileName = Path.Combine("<your path>", "your file.xml")

		If Not File.Exists(m_SettingFileName) Then
			MsgBox(String.Format("Die Datei konnte nicht geladen werden!{0}{1}", vbNewLine, m_SettingFileName))

			Return Nothing
		End If
		m_ProgSettingsXml = New SettingsXml(m_SettingFileName)

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

			result.StagingEMailFrom = stagingEMailFrom
			result.StagingEMailTo = stagingEMailTo
			result.NotifyEMailFrom = notifyEMailfrom

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
			result.CVLParseAsDemo = cvlparseasdemo
			result.ParseEMailAttachment = parseemailattachment
			result.AskSendToCVLizer = asksendtocvlizer
			result.DeleteParsedEMails = deleteparsedemails


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0} >>> {1}", m_SettingFileName, ex.ToString))

		End Try

		m_Logger.LogDebug(String.Format("file is readed: {0}", m_SettingFileName))

		success = success AndAlso CreateTemporaryWorkingFolders(result)
		If Not success Then Return Nothing


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

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#End Region

	''' <summary>
	''' Creates the temporary working folders.
	''' </summary>
	Private Function CreateTemporaryWorkingFolders(ByVal settingFile As ProgramSettings) As Boolean

		Try

			If Not String.IsNullOrWhiteSpace(settingFile.CVLFolderTOWatch) AndAlso Not Directory.Exists(settingFile.CVLFolderTOWatch) Then Directory.CreateDirectory(settingFile.CVLFolderTOWatch)
			If Not String.IsNullOrWhiteSpace(settingFile.CVLFolderTOArchive) AndAlso Not Directory.Exists(settingFile.CVLFolderTOArchive) Then Directory.CreateDirectory(settingFile.CVLFolderTOArchive)
			If Not String.IsNullOrWhiteSpace(settingFile.CVLXMLFolder) AndAlso Not Directory.Exists(settingFile.CVLXMLFolder) Then Directory.CreateDirectory(settingFile.CVLXMLFolder)
			If Not String.IsNullOrWhiteSpace(settingFile.TemporaryFolder) AndAlso Not Directory.Exists(settingFile.TemporaryFolder) Then Directory.CreateDirectory(settingFile.TemporaryFolder)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False
		End Try

		Return True
	End Function


	Private Class CantonData
		Public Property cantonId As Integer
		Public Property cantonAbbreviation As String
		Public Property cantonLongName As String
		Public Property cantonDateOfChange As DateTime?

	End Class

	Private Class DistrictData
		Public Property districtHistId As Integer
		Public Property cantonId As Integer
		Public Property districtId As Integer
		Public Property districtEntryMode As Integer
		Public Property districtAdmissionNumber As Integer

		Public Property districtAdmissionMode As Integer
		Public Property districtLongName As String
		Public Property districtShortName As String
		Public Property districtAdmissionDate As DateTime?
		Public Property districtDateOfChange As DateTime?

	End Class

	Private Class MunicipalityData
		Public Property HistoricNumber As Integer
		Public Property BezirkNumber As Integer
		Public Property BFSNumber As Integer
		Public Property municipalityEntryMode As Integer
		Public Property municipalityStatus As Integer
		Public Property municipalityAdmissionNumber As Integer

		Public Property municipalityAdmissionMode As Integer
		Public Property municipalityAbolitionNumber As Integer
		Public Property municipalityAbolitionMode As Integer
		Public Property Bez_DE As String
		Public Property municipalityLongName As String
		Public Property Canton As String
		Public Property municipalityAdmissionDate As DateTime?
		Public Property municipalityAbolitionDate As DateTime?
		Public Property municipalityDateOfChange As DateTime?

	End Class

	Private Sub lblSettingfilename_Click(sender As Object, e As EventArgs) Handles lblSettingfilename.Click
		Process.Start("explorer.exe", "/select," & lblSettingfilename.Text)
	End Sub

End Class

Public Class TestXMLReader1

	Private _document As XDocument

	Public Sub New(ByVal filename As String)
		_document = XDocument.Load(filename)
	End Sub

	Public Function XElements() As IEnumerable(Of XElement)
		Dim xRoot As XElement = _document.Element("districts")
		Return xRoot.Elements()
	End Function

End Class

Public Class TestXMLReader2

	Private _filePath As String

	Public Sub New(ByVal filename As String)
		_filePath = filename
	End Sub

	Public Iterator Function XElements() As IEnumerable(Of XElement)
		Using reader As XmlReader = XmlReader.Create(_filePath)
			reader.MoveToContent()

			While reader.Read()

				If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "child_node" Then
					Dim el As XElement = TryCast(XNode.ReadFrom(reader), XElement)
					If el IsNot Nothing Then Yield el
				End If
			End While
		End Using

	End Function

End Class

Public Class ProgramSettings
	Public Property WebserviceDomain As String
	Public Property FileServerPath As String
	Public Property CVScanFolder As String
	Public Property ReportScanFolder As String
	Public Property ScanParserStartProgram As String
	Public Property EMailParserStartProgram As String

	Public Property Notificationintervalperiode As Decimal
	Public Property Notificationintervalperiodeforreport As Decimal
	Public Property CVLFolderTOWatch As String
	Public Property CVLFolderTOArchive As String
	Public Property CVLXMLFolder As String
	Public Property TemporaryFolder As String

	Public Property SoftekSettingFolder As String
	Public Property ScanFileFilter As String
	Public Property ScanDirectoryToListen As String
	Public Property ProcessedScannedDocuments As String
	Public Property NotifyOnDisposeScanListing As Boolean
	Public Property NotifyEMailToScanJob As String

	Public Property SmtpNotificationServer As String
	Public Property SmtpNotificationPort As String
	Public Property SmtpNotificationUser As String
	Public Property SmtpNotificationPassword As String
	Public Property SmtpNotificationUseTLS As Boolean

	Public Property SmtpServer As String
	Public Property SmtpPort As String
	Public Property SmtpUser As String
	Public Property SmtpPassword As String
	Public Property SmtpUseTLS As Boolean

	Public Property StagingEMailFrom As String
	Public Property StagingEMailTo As String
	Public Property NotifyEMailFrom As String

	Public Property ReportMailbox As String
	Public Property ReportEmailUser As String
	Public Property ReportEmailPassword As String

	Public Property CVMailbox As String

	Public Property FTPServer As String
	Public Property FTPFolder As String
	Public Property FTPUser As String
	Public Property FTPPassword As String


	Public Property ConnstringApplication As String
	Public Property ConnstringCVLizer As String
	Public Property ConnstringSysteminfo As String
	Public Property ConnstringScanjobs As String
	Public Property ConnstringEMail As String
	Public Property ConnstringWOS As String
	Public Property ConnstringSPPublicData As String
	Public Property CVLParseAsDemo As Boolean
	Public Property ParseEMailAttachment As Boolean
	Public Property AskSendToCVLizer As Boolean
	Public Property DeleteParsedEMails As Boolean


End Class

Public Class CVLUsageData
	Public Property Customer_ID As String
	Public Property CustomerName As String
	Public Property TotalUsedAmount As Integer?

	Public Property UsedMonth As Integer?
	Public Property UsedYear As Integer?
	Public Property UsedAmount As Integer?

	Public ReadOnly Property MonthYear As String
		Get
			Return String.Format("{0:F0} - {1}", UsedMonth, UsedYear)
		End Get
	End Property

End Class

Public Class MonthlyAmount
	Public Property UsedMonth As Integer?
	Public Property UsedYear As Integer?
	Public Property UsedAmount As Integer?

End Class


