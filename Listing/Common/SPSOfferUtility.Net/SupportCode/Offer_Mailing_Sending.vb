
Imports SP.Infrastructure.LOGWriter
Imports DevExpress.XtraGrid.Columns
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.ComponentModel
Imports System.IO
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Propose.DataObjects
Imports System.Net.Mail
Imports System.Net
Imports SP.Infrastructure

Partial Class OfferMessages

#Region "Private fields"

	Private m_EmailLanguageTemplateFolder As String
	Private m_EMailLanguageTemplateFilename As String
	Private m_CurrentOfferData As OffersMasterData

	Private m_SendFileType As SendFiletype
	Enum SendFiletype
		PresentationDoc
		EmployeeDoc
		BothDoc
	End Enum

#End Region


#Region "private properties"

	Private ReadOnly Property LoadAssingedEMailSubject As String
		Get
			Dim value As String
			If String.IsNullOrWhiteSpace(m_CurrentOfferData.OF_Res7) Then
				value = m_CurrentOfferData.OFLabel
			Else
				value = m_CurrentOfferData.OF_Res7
			End If

			Return value
		End Get

	End Property

	Private ReadOnly Property LoadAssingedEMailBody As String
		Get
			Dim value As String
			value = m_CurrentOfferData.OF_Res8

			Return value
		End Get

	End Property

#End Region

	Function LoadOffersDataForSendingMail() As Boolean
		Dim result As Boolean = True
		Dim eMailSubject As String = String.Empty
		Dim eMailBody As String = String.Empty
		Dim Of_Bezeichnung As String = String.Empty


		Dim data = m_ProposeDatabaseAccess.LoadAssingedOfferData(m_InitializationData.MDData.MDNr, m_CurrentOfferNumber, m_CurrentCustomerNumber, m_CurrentCResponsibleNumber)
		If data Is Nothing Then
			m_Logger.LogError(String.Format("offer could not be founded: OfNr: {0} | KDNr: {1} | ZHDNr: {2}", m_CurrentOfferNumber, m_CurrentCustomerNumber, m_CurrentCResponsibleNumber))
			m_LogWriter.WriteTempLogFile(String.Format("<color=red><b>Fehler:</b> Ihre Offertdaten konnte nicht geladen werden.</color>", m_CurrentOfferNumber), m_LOGFileName)

			Return False
		End If

		Try
			m_CurrentOfferData = data
			eMailSubject = data.OF_Res7
			eMailBody = data.OF_Res8
			Of_Bezeichnung = data.OFLabel
			eMailSubject = LoadAssingedEMailSubject

			If String.IsNullOrWhiteSpace(eMailBody) Then
				m_LogWriter.WriteTempLogFile("<color=red><b>Fehler:</b> In Ihrer Offerte sind die Nachrichtentexte leere.</color>", m_LOGFileName)

				Return False
			End If
			m_EmailLanguageTemplateFolder = Path.GetDirectoryName(m_EmailTemplateFilename)
			m_EMailLanguageTemplateFilename = FileIO.FileSystem.GetName(m_EmailTemplateFilename)

			Dim customerLanguage = data.CustomerLanguage.ToUpper
			If String.IsNullOrWhiteSpace(customerLanguage) Then customerLanguage = "D"
			Select Case Mid(customerLanguage, 1, 1)
				Case "I"
					m_EmailLanguageTemplateFolder = Path.Combine(m_EmailLanguageTemplateFolder, "I")
					m_TemplateParser.DestinationLanguage = "IT"
				Case "F"
					m_EmailLanguageTemplateFolder = Path.Combine(m_EmailLanguageTemplateFolder, "F")
					m_TemplateParser.DestinationLanguage = "FR"

			End Select
			If Not Directory.Exists(m_EmailLanguageTemplateFolder) Then m_EmailLanguageTemplateFolder = Path.GetDirectoryName(m_EmailTemplateFilename)

			m_EMailLanguageTemplateFilename = Path.Combine(m_EmailLanguageTemplateFolder, m_EMailLanguageTemplateFilename)
			If Not File.Exists(m_EMailLanguageTemplateFilename) Then m_EMailLanguageTemplateFilename = m_EmailTemplateFilename

			m_Logger.LogInfo(String.Format("tplFilename: {0}", m_EMailLanguageTemplateFilename))

		Catch ex As Exception
			m_LogWriter.WriteTempLogFile(String.Format("<color=red><b>Fehler:</b> Mail-Vorlage konnte nicht geladen werden. {0}</color>", ex.ToString), m_LOGFileName)

			Return False
		End Try

		m_TemplateParser.CurrentOfferData = data
		m_TemplateParser.EMailSubject = eMailSubject
		m_TemplateParser.EMailLanguageTemplateFilename = m_EMailLanguageTemplateFilename

		Try
			m_Logger.LogDebug(String.Format("SendMailOut is started..."))

			result = SendMailOut(eMailSubject, eMailBody)
			m_Logger.LogDebug(String.Format("SendMailOut is finished: {0}", result))

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))

			Return False
		End Try

		Return result
	End Function

	Private Function SendMailOut(ByVal eMailSubject As String, ByVal eMailBody As String) As Boolean
		Dim result As Boolean = True
		Dim strFullFilename As New List(Of String)
		Dim logEntry As String = String.Empty

		Try
			' Überprüfen ob die Nachricht bereits versendet wurde...
			If Not m_SendAsStaging Then
				Dim sentData = m_ProposeDatabaseAccess.IsAssignedMessageAlreadySent(m_InitializationData.MDData.MDGuid, m_CurrentCustomerNumber, m_CurrentEMailAddress, eMailSubject)
				If Not sentData Is Nothing AndAlso sentData.ID.GetValueOrDefault(0) > 0 Then
					m_Logger.LogWarning(String.Format("Duplicate: iKDNr: {0} | eMailTo: {1} subject: {2}", m_CurrentCustomerNumber, m_CurrentEMailAddress, eMailSubject))
					Dim msg As String = "<color=red><b>Duplikat:</b> Ähnliches Mail wurde bereits am {0:g} durch {1} an {2} gesendet.</color>"
					m_LogWriter.WriteTempLogFile(String.Format(msg, sentData.CreatedOn, sentData.CreatedFrom, sentData.EMailTo), m_LOGFileName)

					Return False
				End If
			End If

			If m_SendAsStaging Then eMailSubject = String.Format("{0} {1}", m_Translate.GetSafeTranslationValue("#Testnachricht#"), eMailSubject)
			eMailBody = m_TemplateParser.ParseTemplateFile(String.Empty)        ' Body
			m_Logger.LogInfo(String.Format("parsed body"))
			If String.IsNullOrWhiteSpace(eMailBody) Then
				m_LogWriter.WriteTempLogFile("<color=red><b>Fehler:</b> leere Nachrichten-Text.</color>", m_LOGFileName)

				Return False
			End If

			If m_SendingData.SendCreatedTemplateAttachments AndAlso m_SendingData.SendScannedAttachments Then
				m_SendFileType = SendFiletype.BothDoc

			ElseIf m_SendingData.SendCreatedTemplateAttachments Then
				m_SendFileType = SendFiletype.EmployeeDoc

			ElseIf m_SendingData.SendScannedAttachments Then
				m_SendFileType = SendFiletype.PresentationDoc
			End If

			If m_SendingData.SendScannedAttachments Then
				strFullFilename = LoadOffersAttachmantData()

			ElseIf m_SendingData.SendCreatedTemplateAttachments Then
				strFullFilename = New List(Of String) From {m_ExportedFilename}

			End If
			m_Logger.LogDebug(String.Format("count of attachments to send: {0}", strFullFilename.Count))

			Dim sendResult = New MailSendValue With {.Value = True}
			If result Then
				sendResult = SendMailToWithExchange(m_AdvisorEmailAddress, m_CurrentEMailAddress, String.Empty, String.Empty, eMailSubject, eMailBody, strFullFilename)
				result = result AndAlso sendResult.Value
			End If
			If Not result Then Throw New Exception(String.Format("<color=red><b>Fehler:</b> Ihre Nachricht konnte nicht gesendet werden.<br>{0}</color>", sendResult.ValueLable))

			Dim contactResult As Boolean
			contactResult = Not m_SendAsStaging AndAlso AddNewMailContactEntry(eMailSubject, eMailBody, strFullFilename)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			m_LogWriter.WriteTempLogFile(ex.ToString, m_LOGFileName)

			Return False
		End Try

		Return result
	End Function

	Private Function SendMailToWithExchange(ByVal strFrom As String, ByVal strTo As String, ByVal CCAddress As String,
																	ByVal BCCAddress As String, ByVal strSubject As String, ByVal strBody As String,
																	ByVal aAttachmentFile As List(Of String)) As MailSendValue
		Dim result As New MailSendValue With {.Value = True, .ValueLable = String.Empty}
		Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
		Dim mailmsg As New System.Net.Mail.MailMessage
		Dim smtpServer As String = m_SmtpServer
		Dim smtpPort As Integer = Val(m_SmtpPort)
		Dim enableSSL As Boolean = EMailEnableSSL
		Dim smtpDelivery As SmtpDeliveryMethod = DeliveryMethod

		Dim strToAdresses = strTo.Split(CChar(";")).ToList()
		Dim strCCAddress = CCAddress.Split(CChar(";")).ToList()
		Dim strBCCAddress = BCCAddress.Split(CChar(";")).ToList()

		Try
			'Dim exporter As New RichEditMailMessageExporter(rtfContent, mailmsg)
			'exporter.Export()

			If smtpPort = 0 Then smtpPort = 25

			Dim strEx_UserName As String = m_CurrentUserData.Exchange_USName
			Dim strEx_UserPW As String = m_CurrentUserData.Exchange_USPW
			Dim _ClsProgsetting As New SPProgUtility.ClsProgSettingPath

			strEx_UserName = _ClsProgsetting.DecryptBase64String(strEx_UserName)
			strEx_UserPW = _ClsProgsetting.DecryptBase64String(strEx_UserPW)

#If DEBUG Then
			smtpServer = "smtpserver"
			strEx_UserName = "username"
			strEx_UserPW = "password"
			smtpPort = 587
			enableSSL = True

			strFrom = "info@domain.com"
			strToAdresses = New List(Of String) From {"user@domain.com"}
#End If

			With mailmsg
				.IsBodyHtml = True
				.To.Clear()
				.From = New MailAddress(strFrom)

				For Each toItem In strToAdresses
					If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
				Next

				.CC.Clear()
				For Each ccItem In strCCAddress
					If Not String.IsNullOrWhiteSpace(ccItem) Then .CC.Add(New MailAddress(ccItem.Trim))
				Next

				.Bcc.Clear()
				For Each bccItem In strBCCAddress
					If Not String.IsNullOrWhiteSpace(bccItem) Then .Bcc.Add(New MailAddress(bccItem.Trim))
				Next


				.ReplyToList.Clear()
				.ReplyToList.Add(.From)

				.Subject = strSubject.Trim()
				.Body = strBody.Trim()

				.Priority = If(m_EMailPriority, Net.Mail.MailPriority.High, MailPriority.Normal)

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

					Dim mailClient As New System.Net.Mail.SmtpClient(smtpServer, smtpPort)
					mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
					mailClient.EnableSsl = enableSSL
					If smtpDelivery = Nothing Then mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network Else mailClient.DeliveryMethod = smtpDelivery

					m_Logger.LogInfo(String.Format("sending email with authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | smtpServer: {2} | smtpPort: {3}", enableSSL, mailClient.DeliveryMethod.ToString, smtpServer, smtpPort))

					mailClient.Send(mailmsg)
					mailClient.Dispose()

				Else

					obj.Port = smtpPort
					obj.EnableSsl = enableSSL
					If smtpDelivery <> Nothing Then obj.DeliveryMethod = smtpDelivery

					obj.Host = smtpServer

					m_Logger.LogInfo(String.Format("sending email with NO authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | smtpServer: {2} | smtpPort: {3}", enableSSL, obj.DeliveryMethod.ToString, smtpServer, smtpPort))

					obj.Send(mailmsg)

				End If

				result.Value = True
				m_Logger.LogDebug(String.Format("SendMailToWithExchange: {0}", result.Value))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.", ex.ToString()))
				result.Value = False
				result.ValueLable = String.Format("(Eror: SendMailToWithExchange) Fehler_1: AN: {0} From: {1} Message: {2}", strTo, strFrom, ex.ToString())
				m_LogWriter.WriteTempLogFile(String.Format("<color=red><b>Fehler:</b> Email-Versand von {0} an {1} war nicht erfolgreich.<br>{2}</color>", strFrom, strTo, ex.ToString), String.Empty)

			Finally
				obj.Dispose()
				mailmsg.Attachments.Dispose()
				mailmsg.Dispose()

			End Try
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.Message))
			result.Value = False
			result.ValueLable = String.Format("(Eror: SendMailToWithExchange) Fehler_2: AN: {0} From: {1} Message: {2}", strTo, strFrom, ex.ToString)
			m_LogWriter.WriteTempLogFile(String.Format("<color=red><b>Fehler:</b> Email-Versand von {0} an {1} war nicht erfolgreich.<br>{2}</color>", strFrom, strTo, ex.ToString), String.Empty)

		End Try

		Return result
	End Function

	Private Function LoadOffersAttachmantData() As List(Of String)
		Dim result As List(Of String) = Nothing

		Dim data = m_ProposeDatabaseAccess.LoadOffersDocumentsForEMailAttachment(m_CurrentOfferNumber)
		If data Is Nothing Then
			m_Logger.LogError(String.Format("attachment could not be loaded: {0}", m_CurrentOfferNumber))

			Return Nothing
		End If

		result = New List(Of String)
		Try

			For Each itm In data
				Dim filename As String = Path.Combine(m_InitializationData.UserData.spAllowedPath, String.Format("Offerte {0}_{1}.{2}", m_CurrentOfferNumber, itm.ID, itm.ScanExtension))

				If File.Exists(filename) Then File.Delete(filename)
				If m_Utility.WriteFileBytes(filename, itm.Content) Then result.Add(filename)
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("attachment could not be loaded: {0} | {1}", m_CurrentOfferNumber, ex.ToString))

			Return Nothing
		End Try

		Return result
	End Function

	Private Function AddNewMailContactEntry(ByVal eMailSubject As String, ByVal eMailBody As String, ByVal attachmentfiles As List(Of String)) As Boolean
		Dim result As Boolean = True

		Try
			Dim bodyAdditional As String = String.Format("Offertennummer: {0}<br>{1}", m_CurrentOfferNumber, eMailBody)
			m_Logger.LogDebug(String.Format("CreateLogToKDKontaktDb wird gestartet... iKDNr: {0} | iKDZNr: {1}", m_CurrentCustomerNumber, m_CurrentCResponsibleNumber, m_CurrentEMailAddress))

			Dim contactData As New SentMailContactData With {.OfNumber = m_CurrentOfferNumber, .Customer_ID = m_InitializationData.MDData.MDGuid, .CustomerNumber = m_CurrentCustomerNumber, .CResponsibleNumber = m_CurrentCResponsibleNumber}
			contactData.CreatedFrom = m_InitializationData.UserData.UserFullName
			contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
			contactData.Body = ConvertHtmlToPlainText(bodyAdditional)
			contactData.Content = If(attachmentfiles.Count > 0, m_Utility.LoadFileBytes(attachmentfiles(0)), Nothing)
			contactData.EMailFrom = m_AdvisorEmailAddress
			contactData.EMailTo = m_CurrentEMailAddress
			contactData.EmployeeNumber = Nothing
			contactData.Subject = eMailSubject
			contactData.AttachmentFileName = If(attachmentfiles.Count > 0, Path.GetFileName(attachmentfiles(0)), String.Empty)
			contactData.SMTPServer = m_SmtpServer
			contactData.MessageGuid = m_MessageGuid

			Dim contactResult As Boolean = result AndAlso m_ProposeDatabaseAccess.AddNewEntryForSentMessage(contactData)
			m_LogWriter.WriteTempLogFile(String.Format("<b>Kontakt-Eintrag:</b> {0}", If(contactResult, "OK", "Fehlerhaft")), m_LOGFileName)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			'm_LogWriter.WriteTempLogFile(String.Format("{0}", ex.ToString), m_LOGFileName)

		End Try

		Return result
	End Function

	Private Function ConvertHtmlToPlainText(ByVal html As String) As String
		Dim htmlutilities As New HTMLUtiles.Utilities

		Dim plainText As String = html
		plainText = htmlutilities.ConvertToPlainText(html)

		Return plainText
	End Function



#Region "Helper classes"

	Private Class MailSendValue
		Public Property Value As Boolean
		Public Property ValueLable As String

	End Class


#End Region

End Class
