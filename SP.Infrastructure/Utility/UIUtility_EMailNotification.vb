
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports SP.Infrastructure.Logging
Imports Logger = SP.Infrastructure.Logging.Logger


Namespace UI

	Partial Class UtilityUI


		Private Const NOTIFICATION_EMAIL_TOADDRESS As String = "notification@domain.com"
		Private Const NOTIFICATION_EMAIL_FROMADDRESS As String = "info@domain.com"
		Private Const NOTIFICATION_EMAIL_SMTPSERVER As String = "smtpserver"
		Private Const NOTIFICATION_EMAIL_USERNAME = "username"
		Private Const NOTIFICATION_EMAIL_PASSWORD = "password"
		Private Const NOTIFICATION_EMAIL_SMTPPORT = 587
		Private Const NOTIFICATION_EMAIL_ENABLESSL = True


		Private m_Logger As ILogger = New Logger()


		''' <summary>
		''' send notifications with email.
		''' </summary>
		Public Function SendMailNotification(ByVal subject As String, ByVal body As String, ByVal additionalToAddress As String, ByVal attachmentFiles As List(Of String)) As Boolean

			Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
			Dim mailmsg As New System.Net.Mail.MailMessage
			Dim result As Boolean = True
			Dim m_EnableSSL As Boolean = NOTIFICATION_EMAIL_ENABLESSL
			Dim m_SmtpPort As Integer = NOTIFICATION_EMAIL_SMTPPORT
			Dim strSmtp As String = NOTIFICATION_EMAIL_SMTPSERVER
			Dim otherToAddress = additionalToAddress

			Dim strToAdresses As New List(Of String)
			Dim strFrom As String = NOTIFICATION_EMAIL_FROMADDRESS
			Dim bIsHtml = True

			If String.IsNullOrWhiteSpace(otherToAddress) Then
				strToAdresses = NOTIFICATION_EMAIL_TOADDRESS.Split(CChar(";")).ToList()
			Else
				otherToAddress = String.Format("{0};{1}", NOTIFICATION_EMAIL_TOADDRESS, otherToAddress)
				strToAdresses = otherToAddress.Split(CChar(";")).ToList()

			End If
			Try
				Dim strEx_UserName As String = String.Empty
				Dim strEx_UserPW As String = String.Empty
				strEx_UserName = NOTIFICATION_EMAIL_USERNAME
				strEx_UserPW = NOTIFICATION_EMAIL_PASSWORD

#If DEBUG Then
				strSmtp = "smtpserver"
				strEx_UserName = "username"
				strEx_UserPW = "password"
				m_SmtpPort = 587
				m_EnableSSL = True

				strFrom = "notification@domain.com"
				strToAdresses = New List(Of String) From {"user@domain.com"}
#End If

				With mailmsg
					.IsBodyHtml = bIsHtml
					.To.Clear()
					.From = New MailAddress(strFrom)
					For Each toItem In strToAdresses
						If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
					Next
					.ReplyToList.Clear()
					.ReplyToList.Add(.From)

					.Subject = subject.Trim()
					.Body = body.Trim()

					.Priority = Net.Mail.MailPriority.High
					If Not attachmentFiles Is Nothing AndAlso attachmentFiles.Count > 0 Then
						For Each itm In attachmentFiles
							If File.Exists(itm) Then .Attachments.Add(New System.Net.Mail.Attachment(itm))
						Next
					End If

				End With

				Dim mailClient As New System.Net.Mail.SmtpClient(strSmtp, m_SmtpPort)
				mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
				mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
				mailClient.EnableSsl = m_EnableSSL

				mailClient.Send(mailmsg)
				mailClient.Dispose()


			Catch ex As Exception
				m_Logger.LogError(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}",
														strSmtp, m_SmtpPort, m_EnableSSL, strToAdresses, strFrom, ex.ToString()))
				result = False

			Finally
				obj.Dispose()
				If Not mailmsg.Attachments Is Nothing Then mailmsg.Attachments.Dispose()
				mailmsg.Dispose()
			End Try


			Return result
		End Function


	End Class


End Namespace
