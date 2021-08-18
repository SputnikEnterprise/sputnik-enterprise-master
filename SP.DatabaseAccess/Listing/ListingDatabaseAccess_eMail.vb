
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language



Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadEMailData(ByVal CustomerID As String, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal createdon As Date?) As IEnumerable(Of ListingMailData) Implements IListingDatabaseAccess.LoadEMailData
			Dim result As List(Of ListingMailData) = Nothing

			Dim SQL As String

			SQL = "Select mk.ID, mk.recNr, mk.Customer_ID, mk.KDNr, mk.KDZNr, mk.MANr, mk.eMail_To, mk.eMail_From, mk.eMail_Subject, mk.eMail_Body, "
			SQL &= "mk.eMail_smtp, mk.AsHtml, mk.AsTelefax, mk.Createdon, mk.CreatedFrom, mk.Message_ID, "
			'SQL &= "mf.[FileName], mf.ScanFile, "

			SQL &= "KD.Firma1, KDz.Anrede As zAnrede, "
			SQL &= "KDZ.Nachname zNachname, KDZ.Vorname zVorname, "
			SQL += "MA.Nachname MANachname , MA.Vorname MAVorname "

			SQL &= "From [Sputnik_MailDb].dbo.Mail_Kontakte mk "
			'SQL &= "Left Join [Sputnik_MailDb].dbo.Mail_FileScan mf On mk.RecNr = mf.RecNr "
			'SQL &= "And mk.eMail_To = mf.eMail_To And mk.Message_ID = mf.Message_ID "

			SQL &= "Left Join Kunden KD On mk.KDNr = KD.KDNr "
			SQL &= "Left Join KD_Zustaendig KDz On mk.KDZNr = KDz.RecNr And mk.KDNr = KDz.KDNr "
			SQL &= "Left Join Mitarbeiter MA On mk.MANr = MA.MANr "

			SQL &= "Where "
			SQL &= "mk.Customer_ID = @Customer_ID "
			SQL &= "And (@Message_ID = '' OR mk.Message_ID = @Message_ID) "
			SQL &= "And Year(mk.CreatedOn) = @jahr "
			SQL &= "And (@monat = 0 Or Month(mk.CreatedOn) = @monat) "

			If createdon.HasValue Then
				SQL &= "And mk.createdon = @createdon "
			End If
			SQL &= "Order By mk.CreatedOn Desc"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", CustomerID))
			listOfParams.Add(New SqlClient.SqlParameter("Message_ID", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(jahr, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(monat, 0)))

			If createdon.HasValue Then
				listOfParams.Add(New SqlClient.SqlParameter("createdon", createdon))
			End If

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ListingMailData)

					While reader.Read()
						Dim overviewData As New ListingMailData

						overviewData.ID = SafeGetInteger(reader, "ID", 0)
						overviewData.recNr = SafeGetInteger(reader, "recnr", 0)
						overviewData.customer_id = SafeGetString(reader, "Customer_ID")
						overviewData.KDNr = SafeGetInteger(reader, "KDNr", 0)
						overviewData.ZHDNr = SafeGetInteger(reader, "KDZNr", 0)
						overviewData.MANr = SafeGetInteger(reader, "MANr", 0)

						overviewData.employeelastname = SafeGetString(reader, "MANachname")
						overviewData.employeefirstname = SafeGetString(reader, "MAVorname")

						overviewData.customername = SafeGetString(reader, "Firma1")
						overviewData.cresponsiblesalution = SafeGetString(reader, "zAnrede")
						overviewData.cresponsiblelastname = SafeGetString(reader, "ZNachname")
						overviewData.cresponsiblefirstname = SafeGetString(reader, "ZVorname")


						overviewData.email_to = SafeGetString(reader, "email_to")
						overviewData.email_from = SafeGetString(reader, "email_from")
						overviewData.email_subject = SafeGetString(reader, "email_subject")
						overviewData.email_body = SafeGetString(reader, "email_body")
						overviewData.messageID = SafeGetString(reader, "Message_ID")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")


						result.Add(overviewData)

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

		Function GetAssignedMailData(ByVal CustomerID As String, ByVal recID As Integer?) As ListingMailData Implements IListingDatabaseAccess.GetAssignedMailData
			Dim result As ListingMailData = Nothing

			Dim SQL As String

			'SQL = "Select mk.ID, mk.RecNr, mk.Customer_ID, mk.MANr, mk.KDNr, mk.KDZNr, mk.eMail_Subject, mk.eMail_Body, "
			'SQL &= "mk.eMail_To, mk.eMail_From, mk.AsHtml, mk.AsTelefax, mk.CreatedOn, "
			'SQL &= "mk.CreatedFrom, mk.Message_ID, "
			'SQL &= "KD.Firma1, KDz.Anrede As zAnrede, "
			'SQL &= "KDZ.Nachname zNachname, KDZ.Vorname zVorname, "
			'SQL += "MA.Nachname MANachname , MA.Vorname MAVorname "

			'SQL &= "From [Sputnik_MailDb].dbo.Mail_Kontakte mk "
			'SQL &= "Left Join Kunden KD On mk.KDNr = KD.KDNr "
			'SQL &= "Left Join KD_Zustaendig KDz On mk.KDZNr = KDz.RecNr And mk.KDNr = KDz.KDNr "
			'SQL &= "Left Join Mitarbeiter MA On mk.MANr = MA.MANr "

			'SQL += "Where mk.Customer_ID = @Customer_ID "
			'SQL &= "And mk.ID = @RecID "

			SQL = "[Load Mail Data For Show In LOG View]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", CustomerID))
			listOfParams.Add(New SqlClient.SqlParameter("RecID", ReplaceMissing(recID, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				result = New ListingMailData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim overviewData As New ListingMailData

					overviewData.ID = SafeGetInteger(reader, "ID", 0)
					overviewData.recNr = SafeGetInteger(reader, "recnr", 0)
					overviewData.customer_id = SafeGetString(reader, "Customer_ID")
					overviewData.KDNr = SafeGetInteger(reader, "KDNr", 0)
					overviewData.ZHDNr = SafeGetInteger(reader, "KDZNr", 0)
					overviewData.MANr = SafeGetInteger(reader, "MANr", 0)

					overviewData.employeelastname = SafeGetString(reader, "MANachname")
					overviewData.employeefirstname = SafeGetString(reader, "MAVorname")

					overviewData.customername = SafeGetString(reader, "Firma1")
					overviewData.cresponsiblesalution = SafeGetString(reader, "zAnrede")
					overviewData.cresponsiblelastname = SafeGetString(reader, "ZNachname")
					overviewData.cresponsiblefirstname = SafeGetString(reader, "ZVorname")


					overviewData.email_to = SafeGetString(reader, "email_to")
					overviewData.email_from = SafeGetString(reader, "email_from")
					overviewData.email_subject = SafeGetString(reader, "email_subject")
					overviewData.email_body = SafeGetString(reader, "email_body")
					overviewData.messageID = SafeGetString(reader, "Message_ID")

					overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
					overviewData.createdfrom = SafeGetString(reader, "createdfrom")


					result = overviewData

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function LoadAssignedEMailAttachmentData(ByVal CustomerID As String, ByVal messageID As String) As IEnumerable(Of ListingMailAttachmentData) Implements IListingDatabaseAccess.LoadAssignedEMailAttachmentData
			Dim result As List(Of ListingMailAttachmentData) = Nothing

			Dim SQL As String

			'SQL = "Select Top 10 mf.ID, mf.recNr, mf.Customer_ID, mf.eMail_To, mf.eMail_From, mf.eMail_Subject, "
			'SQL &= "mf.scanFile, mf.Createdon, mf.CreatedFrom, mf.Message_ID, "
			'SQL &= "mf.[FileName] "

			'SQL &= "FROM [Sputnik_MailDb].dbo.Mail_FileScan mf "
			'SQL &= "Where "
			'SQL &= "mf.Customer_ID = @Customer_ID "
			'SQL &= "And mf.Message_ID = @Message_ID "
			'SQL &= "And ScanFile Is Not Null "
			'SQL &= "Order By mf.ID "

			SQL = "[Load Mail Attachment Data For Show In LOG View]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", CustomerID))
			listOfParams.Add(New SqlClient.SqlParameter("Message_ID", ReplaceMissing(messageID, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ListingMailAttachmentData)

					While reader.Read()
						Dim overviewData As New ListingMailAttachmentData

						overviewData.ID = SafeGetInteger(reader, "ID", 0)
						overviewData.recNr = SafeGetInteger(reader, "recnr", 0)
						overviewData.customer_id = SafeGetString(reader, "Customer_ID")

						overviewData.messageID = SafeGetString(reader, "Message_ID")
						overviewData.email_to = SafeGetString(reader, "email_to")
						overviewData.email_from = SafeGetString(reader, "email_from")
						overviewData.email_subject = SafeGetString(reader, "email_subject")
						overviewData.filename = SafeGetString(reader, "Filename")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")


						result.Add(overviewData)

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

		Function LoadAssignedEMailAttachmentFile(ByVal CustomerID As String, ByVal id As Integer?) As Byte() Implements IListingDatabaseAccess.LoadAssignedEMailAttachmentFile
			Dim result As Byte() = Nothing

			Dim SQL As String

			'SQL = "Select Top 1 mf.ID, mf.recNr, mf.Customer_ID, mf.eMail_To, mf.eMail_From, mf.eMail_Subject, "
			'SQL &= "mf.scanFile, mf.Createdon, mf.CreatedFrom, mf.Message_ID, "
			'SQL &= "mf.[FileName], mf.ScanFile "

			'SQL &= "FROM [Sputnik_MailDb].dbo.Mail_FileScan mf "
			'SQL &= "Where "
			'SQL &= "mf.Customer_ID = @Customer_ID "
			'SQL &= "And mf.ID = @ID "
			'SQL &= "And ScanFile Is Not Null "
			'SQL &= "Order By mf.ID "

			SQL = "[Load Mail Attachment Binary Data For Show In LOG View]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", CustomerID))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(id, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = SafeGetByteArray(reader, "ScanFile")
				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		''' <summary>
		''' add email log data to contact table in sputnik_mailDb.
		''' </summary>
		Function AddEMailLogToContactTable(ByVal mdnr As Integer, ByVal data As ListingMailData) As Boolean Implements IListingDatabaseAccess.AddEMailLogToContactTable

			Dim success = True

			Dim sql As String

			sql = "[Create New Mail LOG In Contact]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(data.customer_id, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(data.KDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZNr", ReplaceMissing(data.ZHDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(data.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_To", ReplaceMissing(data.email_to, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_From", ReplaceMissing(data.email_from, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_Subject", ReplaceMissing(data.email_subject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_Body", ReplaceMissing(data.email_body, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_smtp", ReplaceMissing(data.EMail_SMTP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AsHtml", ReplaceMissing(data.AsHTML, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AsTelefax", ReplaceMissing(data.AsTelefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

			' New RecNr 
			Dim newRecIDParameter = New SqlClient.SqlParameter("@RecID", SqlDbType.Int)
			newRecIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newRecIDParameter)

			Dim newRecNrParameter = New SqlClient.SqlParameter("@RecNr", SqlDbType.Int)
			newRecNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newRecNrParameter)

			' New messageid
			Dim newMessageIDParameter = New SqlClient.SqlParameter("@Message_ID", SqlDbType.NVarChar, 50)
			newMessageIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newMessageIDParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newRecIDParameter.Value Is Nothing AndAlso Not newRecNrParameter.Value Is Nothing AndAlso Not newMessageIDParameter Is Nothing Then
				data.recNr = CType(newRecNrParameter.Value, Integer)
				data.messageID = CType(newMessageIDParameter.Value, String)
			Else
				success = False
			End If


			Return success


		End Function

		''' <summary>
		''' add email attachment data to table in sputnik_mailDb.
		''' </summary>
		Function AddEMailAttachmentLogToContactTable(ByVal mdnr As Integer, ByVal data As ListingMailAttachmentData) As Boolean Implements IListingDatabaseAccess.AddEMailAttachmentLogToContactTable

			Dim success = True

			Dim sql As String

			sql = "[Create New Mail LOG For Mail Attachment]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(data.customer_id, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_To", ReplaceMissing(data.email_to, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_From", ReplaceMissing(data.email_from, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail_Subject", ReplaceMissing(data.email_subject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(data.scanfile, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileName", ReplaceMissing(data.filename, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Message_ID", ReplaceMissing(data.messageID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

			' New RecNr 
			Dim newRecNrParameter = New SqlClient.SqlParameter("@RecNr", SqlDbType.Int)
			newRecNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newRecNrParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso
				Not newRecNrParameter.Value Is Nothing Then
				data.recNr = CType(newRecNrParameter.Value, Integer)
			Else
				success = False
			End If


			Return success


		End Function



	End Class


End Namespace
