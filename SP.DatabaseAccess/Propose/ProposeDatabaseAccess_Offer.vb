
Imports System.Data.SqlClient
Imports SP.DatabaseAccess.Propose.DataObjects


Namespace Propose


	Partial Class ProposeDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IProposeDatabaseAccess



		Function CheckIfEmployeeInOfferExists(ByVal mdNr As Integer, ByVal ofNr As Integer) As Boolean? Implements IProposeDatabaseAccess.CheckIfEmployeeInOfferExists

			Dim doesExist As Boolean = False

			Dim sql As String

			sql = "Select count(*) From dbo.OFF_MASelection Where OfNr = @OfNr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("OfNr", ReplaceMissing(ofNr, DBNull.Value)))


			Dim existingEmployeeRecord = ExecuteScalar(sql, listOfParams, CommandType.Text)

			If Not existingEmployeeRecord Is Nothing Then
				doesExist = (existingEmployeeRecord > 0)

				Return doesExist
			Else
				Return Nothing
			End If

		End Function

		Function LoadAssingedOfferData(ByVal mdNr As Integer, ByVal ofNr As Integer, ByVal customerNumber As Integer?, ByVal cResponsiblepersonNumber As Integer?) As OffersMasterData Implements IProposeDatabaseAccess.LoadAssingedOfferData

			Dim result As OffersMasterData = Nothing

			Dim sql As String

			sql = "[Load Offers Master Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OfNumber", ReplaceMissing(ofNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNumber", ReplaceMissing(customerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDNumber", ReplaceMissing(cResponsiblepersonNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New OffersMasterData

					While reader.Read()
						Dim itm = New OffersMasterData

						itm.OfNumber = SafeGetInteger(reader, "OfNr", Nothing)
						itm.OF_Res1 = SafeGetString(reader, "OF_Res1")
						itm.OF_Res2 = SafeGetString(reader, "OF_Res2")
						itm.OF_Res3 = SafeGetString(reader, "OF_Res3")
						itm.OF_Res4 = SafeGetString(reader, "OF_Res4")
						itm.OF_Res5 = SafeGetString(reader, "OF_Res5")
						itm.OF_Res6 = SafeGetString(reader, "OF_Res6")
						itm.OF_Res7 = SafeGetString(reader, "OF_Res7")
						itm.OF_Res8 = SafeGetString(reader, "OF_Res8")
						itm.OF_Slogan = SafeGetString(reader, "Of_Slogan")
						itm.OF_Group = SafeGetString(reader, "OF_Gruppe")
						itm.OF_Kontakt = SafeGetString(reader, "OF_Kontakt")
						itm.OFLabel = SafeGetString(reader, "OF_Bezeichnung")

						itm.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						itm.CustomerCompany = SafeGetString(reader, "Firma1")
						itm.CustomerNotMailing = SafeGetBoolean(reader, "KD_Mail_Mailing", False)
						itm.CustomerEMail = SafeGetString(reader, "KDeMail")
						itm.CustomerLanguage = SafeGetString(reader, "KDSprache")

						itm.CResponsibleNumber = SafeGetInteger(reader, "RecNr", Nothing)
						itm.CResponsibleLastname = SafeGetString(reader, "KDZNachname")
						itm.CResponsibleFirstname = SafeGetString(reader, "KDZVorname")
						itm.CResponsibleSalution = SafeGetString(reader, "KDZAnrede")
						itm.CResponsibleLetterSalutation = SafeGetString(reader, "KDZAnredeForm")
						itm.CResponsibleNotMailing = SafeGetBoolean(reader, "ZHD_Mail_Mailing", False)
						itm.CResponsibleEMail = SafeGetString(reader, "ZHDeMail")


						result = itm

					End While
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		Function LoadOffersDocumentsForEMailAttachment(ByVal offerNumber As Integer) As IEnumerable(Of OffersDocumentData) Implements IProposeDatabaseAccess.LoadOffersDocumentsForEMailAttachment
			Dim result As List(Of OffersDocumentData) = Nothing

			Dim sql As String

			sql = "Select ID, Bezeichnung, DocScan, ScanExtension From dbo.OFF_Doc "
			sql &= "Where "
			sql &= "OfNr = @OfNr "
			sql &= "Order By ID"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("OfNr", ReplaceMissing(offerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			result = New List(Of OffersDocumentData)
			Try

				If (Not reader Is Nothing) Then
					result = New List(Of OffersDocumentData)

					While reader.Read()
						Dim data = New OffersDocumentData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.ScanExtension = SafeGetString(reader, "ScanExtension")
						data.Content = SafeGetByteArray(reader, "DocScan")


						result.Add(data)

					End While
				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function IsAssignedMessageAlreadySent(ByVal mdGuid As String, ByVal iKDNr As Integer, ByVal streMailTo As String, ByVal strSubject As String) As EMailJob.DataObjects.EMailData Implements IProposeDatabaseAccess.IsAssignedMessageAlreadySent
			Dim result As EMailJob.DataObjects.EMailData

			Dim sql As String

			sql = "Select Top 1 "
			sql &= "MK.ID"
			sql &= ",MK.eMail_To"
			sql &= ",MK.eMail_Subject"
			sql &= ",MK.eMail_From"
			sql &= ",MK.CreatedOn"
			sql &= ",MK.CreatedFrom"
			sql &= " From [Sputnik_MailDb].dbo.Mail_Kontakte MK "
			sql &= "Where "
			sql &= "MK.Customer_ID = @Customer_ID "
			sql &= "And MK.eMail_To = @mailTo "
			sql &= "And MK.KDNr = @customerNumber "
			sql &= "And MK.eMail_Subject = @subject "
			sql &= "And Patindex('#Testnachricht#%', MK.eMail_Subject) = 0"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(mdGuid, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("mailTo", ReplaceMissing(streMailTo, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(iKDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("subject", ReplaceMissing(strSubject, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			result = New EMailJob.DataObjects.EMailData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					Dim data = New EMailJob.DataObjects.EMailData

					data.ID = SafeGetInteger(reader, "ID", Nothing)
					data.EMailTo = SafeGetString(reader, "eMail_To")
					data.EMailSubject = SafeGetString(reader, "eMail_Subject")
					data.EMailFrom = SafeGetString(reader, "eMail_From")
					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


					result = data

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function AddNewEntryForSentMessage(ByVal contactData As SentMailContactData) As Boolean Implements IProposeDatabaseAccess.AddNewEntryForSentMessage
			Dim success As Boolean = True


			Dim sql As String

			If contactData.Content Is Nothing Then
				sql = "[Create New Entry For Sent Messages]"
			Else
				sql = "[Create New Entry For Sent Messages With Attachment]"
			End If


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(contactData.Customer_ID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(contactData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(contactData.CResponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OfNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("eMailTo", ReplaceMissing(contactData.EMailTo, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("eMailFrom", ReplaceMissing(contactData.EMailFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMailsubject", ReplaceMissing(contactData.Subject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Message_ID", ReplaceMissing(contactData.MessageGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Body", ReplaceMissing(contactData.Body, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SMTPServer", ReplaceMissing(contactData.SMTPServer, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("filename", ReplaceMissing(contactData.AttachmentFileName, DBNull.Value)))
			If Not contactData.Content Is Nothing Then listOfParams.Add(New SqlClient.SqlParameter("docScan", ReplaceMissing(contactData.Content, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(contactData.CreatedUserNumber, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				success = True
			Else
				success = False
			End If

			Return success
		End Function

		Function LoadSentMessageAttachmentBytesData(ByVal mailId As Integer) As Byte() Implements IProposeDatabaseAccess.LoadSentMessageAttachmentBytesData
			Dim result As Byte() = Nothing

			Dim sql As String

			sql = "SELECT DocScan FROM Sputnik_MailDb.dbo.[tbl_EMail_Scan] WHERE MailID = @mailId"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mailId", mailId))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetByteArray(reader, "DocScan")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

	End Class


End Namespace
