Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.EMailJob
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SP.DatabaseAccess.ScanJob
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports System.IO

Namespace ChilKatUtility

	Public Class EMailUtility


#Region "private consts"

		Private Const REPORT_SCAN_EMAIL_USER As String = "mailaddress"
		Private Const REPORT_SCAN_EMAIL_PASSWORD As String = "password"

		Private Const CHILKAT_COMPONENT_CODE As String = "yourserialnumber"



		Private Const DEFAULT_SPUTNIK_VACANCYDATA_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPVacancyData.asmx" ' "http://asmx.domain.com/wsSPS_services/SPVacancyData.asmx"
		Private Const DEFAULT_SPUTNIK_EXTERNAL_VACANCYDATA_UTIL_WEBSERVICE_URI As String = "wsSPS_services/externalservices/SPVacancyServices.asmx" ' "http://asmx.domain.com/wsSPS_services/externalservices/SPVacancyServices.asmx"

		' Hostpoint.ch
		Private Const OUR_EXCHANGE_SERVER As String = "smtpServer"
		Private Const CV_EMAIL_USER As String = "parsingmailaddress"
		Private Const CV_EMAIL_PASSWORD As String = "password"

#End Region


#Region "private fields"


		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Protected m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_EMailDatabaseAccess As IEMailJobDatabaseAccess
		Private m_ScanDatabaseAccess As IScanJobDatabaseAccess
		Private m_AppDatabaseAccess As IAppDatabaseAccess

		Private m_customerID As String

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connString_Application As String
		Private m_connString_Scan As String
		Private m_connString_EMail As String
		Private m_connString_Info As String

		Private m_VacancyUtilWebServiceUri As String
		Private m_VacancyExternalUtilWebServiceUri As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_ChilGrob As Chilkat.Global
		Private mailman As Chilkat.MailMan
		Private imap As Chilkat.Imap
		Private m_ftp2 As Chilkat.Ftp2
		Private m_h2t As Chilkat.HtmlToText
		Private Property m_SettingFile As ProgramSettings

		Private m_ReportMailbox As String

		Private m_CVSMTPServer As String
		Private m_CVMailbox As String
		Private m_CVUserName As String
		Private m_CVPassword As String
		Private m_CVPort As Integer
		Private m_CVSSL As Boolean

#End Region


#Region "public properties"

		Public Property CustomerID As String
		Public Property AssignedUserID As String
		Public Property CurrentMailBox As String
		Public Property m_PatternData As EMailPatternSettingData
		Public Property CurrentEMailData As EMailData
		Public Property CurrentEMailAttachmentData As List(Of EMailAttachment)

#End Region


#Region "constructor"

		Public Sub New(ByVal settingFile As ProgramSettings)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SettingFile = settingFile
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_connString_Application = m_SettingFile.ConnstringApplication
			m_connString_Scan = m_SettingFile.ConnstringScanjobs
			m_connString_Info = m_SettingFile.ConnstringSysteminfo
			m_connString_EMail = m_SettingFile.ConnstringEMail

			m_ReportMailbox = m_SettingFile.ReportMailbox
			m_CVMailbox = m_SettingFile.CVMailbox
			If String.IsNullOrWhiteSpace(m_ReportMailbox) Then m_ReportMailbox = "Inbox"
			If String.IsNullOrWhiteSpace(m_CVMailbox) Then m_CVMailbox = "Inbox"

			m_CVSMTPServer = m_SettingFile.SmtpServer
			m_CVUserName = m_SettingFile.SmtpUser
			m_CVPassword = m_SettingFile.SmtpPassword
			m_CVPort = m_SettingFile.SmtpPort
			m_CVSSL = m_SettingFile.SmtpUseTLS

			If String.IsNullOrWhiteSpace(m_CVSMTPServer) Then m_CVSMTPServer = OUR_EXCHANGE_SERVER
			If String.IsNullOrWhiteSpace(m_CVUserName) Then m_CVUserName = CV_EMAIL_USER
			If String.IsNullOrWhiteSpace(m_CVPassword) Then m_CVPassword = CV_EMAIL_PASSWORD
			If String.IsNullOrWhiteSpace(m_CVPort) Then m_CVPort = 993
			If String.IsNullOrWhiteSpace(m_CVSSL) Then m_CVSSL = True



			m_EMailDatabaseAccess = New EMailJobDatabaseAccess(m_connString_EMail, "DE")
			m_ScanDatabaseAccess = New ScanJobDatabaseAccess(m_connString_Scan, "DE")
			m_AppDatabaseAccess = New AppDatabaseAccess(m_connString_Application, "DE")

			Dim domainName = m_SettingFile.WebserviceDomain
			m_VacancyUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCYDATA_UTIL_WEBSERVICE_URI)
			m_VacancyExternalUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EXTERNAL_VACANCYDATA_UTIL_WEBSERVICE_URI)


		End Sub

#End Region


#Region "public methods"

		Public Function CleanUp() As Boolean
			Dim success As Boolean = True

			success = success AndAlso DisconnectEMailHost()
			success = success AndAlso DisconnectFTPHost()


			Return success

		End Function

		Public Function DisconnectFTPHost() As Boolean
			Dim success As Boolean = True
			If m_ftp2 Is Nothing Then Return success

			If m_ftp2.IsConnected Then
				success = success AndAlso m_ftp2.Disconnect()
			End If

			Return success

		End Function

		Public Function DisconnectEMailHost() As Boolean
			Dim success As Boolean = True
			If mailman Is Nothing Then Return success

			If mailman.IsSmtpConnected Then
				success = success AndAlso mailman.CloseSmtpConnection()
			End If

			If imap Is Nothing Then Return success
			If imap.IsConnected Then
				success = success AndAlso imap.Disconnect()
			End If

			Return success

		End Function

		Public Function PrepareChilKatLogin() As Boolean
			Dim success As Boolean = True


			m_ChilGrob = New Chilkat.Global
			mailman = New Chilkat.MailMan
			imap = New Chilkat.Imap
			imap.VerboseLogging = True
			imap.KeepSessionLog = True

			m_ftp2 = New Chilkat.Ftp2
			m_h2t = New Chilkat.HtmlToText

			success = success AndAlso m_ChilGrob.UnlockBundle(CHILKAT_COMPONENT_CODE)
			If (success <> True) Then
				m_Logger.LogError(String.Format("failed to unlock compoents! {0}", m_ChilGrob.LastErrorText))
				Return False
			End If


			mailman.MailHost = m_CVSMTPServer 'smtpServer
			imap.Ssl = m_CVSSL 'True
			imap.Port = m_CVPort ' 993

			success = success AndAlso imap.Connect(m_CVSMTPServer)
			If (success <> True) Then
				m_Logger.LogError(String.Format("PrepareChilKatLogin: unable conect to imap host: {1}{0}{2}", vbNewLine, m_CVSMTPServer, imap.LastErrorText))
				Return False
			End If


			Return success

		End Function

		Public Function PrepareChilKatCVLogin() As Boolean
			Dim success As Boolean = True

			success = success AndAlso imap.Login(m_CVUserName, m_CVPassword)

			If (success <> True) Then
				m_Logger.LogError(String.Format("PrepareChilKatCVLogin: unable to login in imap: {1} > {2}{0}LastErrorHtml:{0}{3}{0}SessionLog:{0}{4}", vbNewLine, m_CVUserName, m_CVPassword, imap.LastErrorHtml, imap.SessionLog))
				Return False
			End If


			Return success

		End Function


		Public Function LoadIMAPMails() As IEnumerable(Of EMailData)
			Dim success As Boolean
			Dim result As List(Of EMailData) = Nothing

			' Select an IMAP mailbox
			success = LoadMailBoxData()
			If Not success Then Return Nothing
			'	m_Logger.LogError(String.Format("unable conect to imap.SelectMailbox('{0}')! {1}", mailBox, imap.LastErrorText))
			'	Return Nothing
			'End If

			Dim messageSet As Chilkat.MessageSet
			' We can choose to fetch UIDs or sequence numbers.
			Dim fetchUids As Boolean
			fetchUids = True
			' Get the message IDs of all the emails in the mailbox
			messageSet = imap.Search("ALL", fetchUids)
			If (messageSet Is Nothing) Then
				m_Logger.LogError(String.Format("unable conect to imap.Search('ALL', fetchUids)! {0}", imap.LastErrorText))
				Return Nothing
			End If

			' Fetch the emails into a bundle object:
			Dim bundle As Chilkat.EmailBundle
			bundle = imap.FetchBundle(messageSet)
			If (bundle Is Nothing) Then
				m_Logger.LogError(String.Format("unable conect to imap.FetchBundle(messageSet)! {0}", imap.LastErrorText))
				Return Nothing
			End If

			Dim email As Chilkat.Email
			result = New List(Of EMailData)
			For i As Integer = 0 To bundle.MessageCount - 1
				email = bundle.GetEmail(i)

				Dim data As New EMailData
				Dim attachmentData As List(Of EMailAttachment) = Nothing
				attachmentData = New List(Of EMailAttachment)

				data.EMailFrom = email.FromAddress
				data.EMailTo = email.GetToAddr(0)
				data.EMailSubject = email.Subject
				data.EMailBody = email.Body
				data.EMailUidl = email.GetImapUid
				data.EMailMime = email.GetMime
				data.EMailDate = email.LocalDate


				data.HasHtmlBody = email.HasHtmlBody
				If email.HasPlainTextBody Then
					data.EMailPlainTextBody = email.GetPlainTextBody
				Else
					If email.HasHtmlBody Then
						data.EMailPlainTextBody = ConvertHtmlToPlainText(email.Body)
					Else
						data.EMailPlainTextBody = email.Body
					End If
				End If

				Dim attachmentCount = email.NumAttachments
				For j As Integer = 0 To attachmentCount - 1
					Dim attachment = New EMailAttachment

					attachment.AttachmentSize = email.GetAttachmentData(j)
					attachment.AttachmentName = email.GetAttachmentFilename(j)

					If attachment.AttachmentSize.Length > 10 Then attachmentData.Add(attachment)

				Next
				data.EMailAttachment = attachmentData

				result.Add(data)

			Next
			m_Logger.LogInfo(String.Format("{0}: loading imap data finishing!", CurrentMailBox))


			Return result

		End Function

		Public Function DeleteAssignedIMAPEMail(ByVal assignedeMailUidl As String) As Boolean
			Dim success As Boolean = True

			Try
				Dim listOfData = LoadIMAPMails()
				Dim data = listOfData.Where(Function(x) x.EMailUidl = assignedeMailUidl).FirstOrDefault
				If data Is Nothing Then Return True

				imap.KeepSessionLog = True

				'  Select an IMAP mailbox
				success = success AndAlso LoadMailBoxData()
				If Not success Then Return False

				'  We can choose to fetch UIDs or sequence numbers.
				Dim fetchUids As Boolean = True
				''  Get the message IDs of all the emails in the mailbox
				Dim email As Chilkat.Email
				email = imap.FetchSingle(assignedeMailUidl, fetchUids)
				success = (Not email Is Nothing) AndAlso success AndAlso imap.SetMailFlag(email, "Deleted", 1)

				success = success AndAlso imap.ExpungeAndClose()
				If (success <> True) Then
					m_Logger.LogError(String.Format("unable conect to imap.ExpungeAndClose()! {0}", imap.LastErrorText))
					Return False
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try


			Return success

		End Function

		Public Function MoveAssignedIMAPEMail(ByVal assignedeMailUidl As String) As Boolean
			Dim success As Boolean = True
			Dim destFolder As String = "Inbox/Not defined"
			Dim fetchUids As Boolean = True

			Try
				Dim listOfData = LoadIMAPMails()
				Dim data = listOfData.Where(Function(x) x.EMailUidl = assignedeMailUidl).FirstOrDefault
				If data Is Nothing Then
					m_Logger.LogWarning(String.Format("unable to find email with EMailUidl: {0}. Maybe mail is allready deleted!", assignedeMailUidl))
					Return True
				End If

				imap.KeepSessionLog = True

				'  Select an IMAP mailbox
				success = success AndAlso LoadMailBoxData()
				If Not success Then Return False


				success = imap.Copy(assignedeMailUidl, True, destFolder)
				If (success <> True) Then
					m_Logger.LogError(String.Format("unable conect to imap.Copy()! {0}", imap.LastErrorText))
					Return False
				End If

				''  Get the message IDs of all the emails in the mailbox
				Dim email As Chilkat.Email
				email = imap.FetchSingle(assignedeMailUidl, fetchUids)
				success = (Not email Is Nothing) AndAlso success AndAlso imap.SetMailFlag(email, "Deleted", 1)

				success = success AndAlso imap.ExpungeAndClose()
				If (success <> True) Then
					m_Logger.LogError(String.Format("unable conect to imap.ExpungeAndClose()! {0}", imap.LastErrorText))
					Return False
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return success

		End Function


		Public Function SaveAssignedIMAPEMailToEML(ByVal assignedeMailUidl As String) As String
			Dim result As String = String.Empty

			'  Select an IMAP mailbox
			Dim success As Boolean = True

			Try
				result = Path.Combine(m_SettingFile.CVLFolderTOArchive, CustomerID, String.Format("{0}.eml", Guid.NewGuid().ToString()))
				If Not Directory.Exists(Path.Combine(m_SettingFile.CVLFolderTOArchive, CustomerID)) Then
					Directory.CreateDirectory(Path.Combine(m_SettingFile.CVLFolderTOArchive, CustomerID))
				End If

			Catch ex As Exception
				result = Path.Combine(Path.GetTempPath(), String.Format("{0}.eml", Guid.NewGuid().ToString()))

			End Try


			success = success AndAlso LoadMailBoxData()
			If Not success Then Return String.Empty

			' We can choose to fetch UIDs or sequence numbers.
			Dim fetchUids As Boolean = True
			''  Get the message IDs of all the emails in the mailbox
			Dim email As Chilkat.Email
			email = imap.FetchSingle(assignedeMailUidl, fetchUids)
			email.OverwriteExisting = True

			If (Not email Is Nothing) AndAlso success Then
				success = email.SaveEml(result)
			End If
			If Not success OrElse Not File.Exists(result) Then result = String.Empty


			Return result

		End Function


		Public Function ListIMAPMailboxes() As List(Of String)
			Dim result As List(Of String) = Nothing
			m_Logger.LogDebug("loading imap data!")


			Dim refName As String = ""

			Dim wildcardedMailbox As String = "*"

			Dim mboxes As Chilkat.Mailboxes = imap.ListMailboxes(refName, wildcardedMailbox)
			If (mboxes Is Nothing) Then
				m_Logger.LogError(imap.LastErrorText)
				Return Nothing
			End If

			result = New List(Of String)
			Dim i As Integer
			For i = 0 To mboxes.Count - 1
				result.Add(mboxes.GetName(i))
			Next

			Return result

		End Function

		Public Function ConvertHtmlToPlainText(ByVal htmlCode As String) As String
			Dim htmlAgilityText As String = ConvertToPlainText(htmlCode)

			Return htmlAgilityText

			Return m_h2t.ToText(htmlCode)
		End Function

		Function ConvertToPlainText(ByVal html As String) As String
			Dim htmlutilities As New HTMLUtiles.Utilities

			Dim plainText As String = html
			plainText = htmlutilities.ConvertToPlainText(html)

			Return plainText

		End Function


#End Region

		Private Function LoadMailBoxData() As Boolean
			Dim success As Boolean = True

			If String.IsNullOrWhiteSpace(CurrentMailBox) Then CurrentMailBox = "INBOX"

			If Not imap.IsConnected Then
				success = success AndAlso PrepareChilKatLogin()
				success = success AndAlso PrepareChilKatCVLogin()
			End If
			success = imap.SelectMailbox(CurrentMailBox)
			If Not success Then
				m_Logger.LogError(String.Format("unable conect to imap.SelectMailbox('{0}')! {1}", CurrentMailBox, imap.LastErrorText))
			End If


			Return success

		End Function



	End Class

End Namespace
