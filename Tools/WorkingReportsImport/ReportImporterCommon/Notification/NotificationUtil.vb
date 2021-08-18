
Imports System.Net.Mail
Imports System.Net
Imports System.IO
Imports ReportImporterCommon.Logging

Namespace Notification


	Public Class Notifying


		Private Shared m_logger As ILogger = New Logger()

		Private m_SettingFile As ProgramSettings

		Public Sub New()

		End Sub

		Public Property SettingFileData As ProgramSettings


		Private ReadOnly Property DeliveryMethod As SmtpDeliveryMethod
			Get
				Dim value As New SmtpDeliveryMethod
				Return Nothing

				'Dim deliveryMethode As Integer? = Val(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/smtp-deliverymethode", m_MailingSetting)))
				'If deliveryMethode = 0 Then Return Nothing
				'If deliveryMethode = 1 Then Return SmtpDeliveryMethod.Network
				'If deliveryMethode = 2 Then Return SmtpDeliveryMethod.PickupDirectoryFromIis
				'If deliveryMethode = 3 Then Return SmtpDeliveryMethod.SpecifiedPickupDirectory


				Return value
			End Get
		End Property



		Public Function SendMailToWithExchange(ByVal strFrom As String, ByVal strTo As String, ByVal bccAddresses As String, ByVal strSubject As String, ByVal strBody As String, ByVal aAttachmentFile As List(Of String)) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
			Dim mailmsg As New System.Net.Mail.MailMessage
			Dim result As Boolean = True
			Dim strToAdresses = String.Format("{0}", strTo).Split(CChar(";")).ToList()
			Dim strbCCAdresses As String() = String.Format("{0}", bccAddresses).Split(CChar(";"))
			Dim smtpDelivery As SmtpDeliveryMethod = DeliveryMethod

			Dim m_EnableSSL As Boolean = SettingFileData.SmtpNotificationUseTLS
			Dim m_SmtpPort As Integer = If(String.IsNullOrWhiteSpace(SettingFileData.SmtpNotificationPort), 25, Val(SettingFileData.SmtpNotificationPort))
			Dim strEx_UserName As String = String.Empty
			Dim strEx_UserPW As String = String.Empty
			strEx_UserName = SettingFileData.SmtpNotificationUser
			strEx_UserPW = SettingFileData.SmtpNotificationPassword
			Dim strSmtp As String = SettingFileData.SmtpNotificationServer

			Try

#If DEBUG Then
				strSmtp = ""
				strEx_UserName = ""
				strEx_UserPW = ""
				m_SmtpPort = 587
				m_EnableSSL = True

				strFrom = ""
				strToAdresses = New List(Of String) From {""}
#End If
				m_logger.LogDebug(String.Format("Start Mailing:{0}SMTPServer: {1}{0}SMTP-Port: {2}{0}SSL: {3}{0}From: {4}{0}AN: {5}",
												vbNewLine, strSmtp, m_SmtpPort, m_EnableSSL, strFrom, strTo))

				With mailmsg
					.IsBodyHtml = True
					.To.Clear()
					.From = New MailAddress(strFrom)
					For Each toItem In strToAdresses
						If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
					Next
					For i As Integer = 0 To strbCCAdresses.Length - 1
						If strbCCAdresses(i).Trim <> String.Empty Then .Bcc.Add(New MailAddress(strbCCAdresses(i).Trim))
					Next

					.ReplyToList.Clear()
					.ReplyToList.Add(.From)

					.Subject = strSubject.Trim()
					.Body = strBody.Trim()

					.Priority = Net.Mail.MailPriority.High
					If Not aAttachmentFile Is Nothing AndAlso aAttachmentFile.Count > 0 Then
						For Each itm In aAttachmentFile
							If File.Exists(itm) Then
								.Attachments.Add(New System.Net.Mail.Attachment(itm))
							End If
						Next
					End If

				End With


				Try
					If Not String.IsNullOrWhiteSpace(strEx_UserName) Then
						Dim mailClient As New System.Net.Mail.SmtpClient(strSmtp, m_SmtpPort)
						mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
						mailClient.EnableSsl = m_EnableSSL
						If smtpDelivery = Nothing Then mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network Else mailClient.DeliveryMethod = smtpDelivery
						m_logger.LogInfo(String.Format("sending email with authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | strSmtp: {2} | smtpPort: {3}", m_EnableSSL, mailClient.DeliveryMethod.ToString, strSmtp, m_SmtpPort))

						mailClient.Send(mailmsg)

						mailClient.Dispose()

					Else

						obj.Port = m_SmtpPort
						obj.EnableSsl = m_EnableSSL
						If smtpDelivery = Nothing Then obj.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network Else obj.DeliveryMethod = smtpDelivery
						obj.Host = strSmtp

						m_logger.LogInfo(String.Format("sending email with NO authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | strSmtp: {2} | smtpPort: {3}", m_EnableSSL, obj.DeliveryMethod.ToString, strSmtp, m_SmtpPort))

						obj.Send(mailmsg)

					End If



					'If strEx_UserName <> String.Empty Then
					'	'strEx_UserName = SettingFileData.SmtpNotificationUser
					'	'strEx_UserPW = SettingFileData.SmtpNotificationPassword

					'	Dim mailClient As New System.Net.Mail.SmtpClient(strSmtp, m_SmtpPort)
					'	mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
					'	mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
					'	mailClient.EnableSsl = m_EnableSSL

					'	mailClient.Send(mailmsg)

					'Else
					'	obj.Host = strSmtp
					'	obj.Send(mailmsg)

					'End If

					result = True
					m_logger.LogDebug(String.Format("mail result: {0}", result))


				Catch ex As Exception
					m_logger.LogError(String.Format("mail send error: {0}", ex.ToString()))
					result = False

				Finally
					obj.Dispose()
					If Not mailmsg.Attachments Is Nothing Then mailmsg.Attachments.Dispose()
					mailmsg.Dispose()

				End Try
			Catch ex As Exception
				m_logger.LogError(String.Format("mail common error: {0}", ex.ToString()))
				result = False

			End Try

			Return result
		End Function

	End Class

End Namespace
