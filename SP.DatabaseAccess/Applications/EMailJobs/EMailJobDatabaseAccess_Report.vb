Imports SP.DatabaseAccess.EMailJob.DataObjects
'Imports System.Transactions


Namespace EMailJob


	Partial Class EMailJobDatabaseAccess


		Inherits DatabaseAccessBase
		Implements IEMailJobDatabaseAccess


		Function AddEMailJob(ByVal eMailData As EMailData, ByVal applicationID As Integer?) As Boolean Implements IEMailJobDatabaseAccess.AddEMailJob

			Dim success = True

			Dim sql As String

			sql = "[Create New EMailJob]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(eMailData.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("applicationID", ReplaceMissing(applicationID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailFrom", ReplaceMissing(eMailData.EMailFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailTo", ReplaceMissing(eMailData.EMailTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailSubject", ReplaceMissing(eMailData.EMailSubject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailBody", ReplaceMissing(eMailData.EMailBody, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailPlainTextBody", ReplaceMissing(eMailData.EMailPlainTextBody, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailUidl", ReplaceMissing(eMailData.EMailUidl, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailDate", ReplaceMissing(eMailData.EMailDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", 3))
			listOfParams.Add(New SqlClient.SqlParameter("ExistsAttachment", ReplaceMissing(eMailData.ExistsAttachment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("HasHtmlBody", ReplaceMissing(eMailData.HasHtmlBody, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(eMailData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailMime", ReplaceMissing(eMailData.EMailMime, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Content", ReplaceMissing(eMailData.EMailContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMLFileName", ReplaceMissing(eMailData.EMLFilename, DBNull.Value)))


			Dim recNrParameter = New SqlClient.SqlParameter("@NewID", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not recNrParameter.Value Is Nothing Then
				eMailData.ID = CType(recNrParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		Function LoadAssigendApplicationEMailData(ByVal customerID As String, ByVal applicationId As Integer?, ByVal searchDate As Date?) As EMailData Implements IEMailJobDatabaseAccess.LoadAssigendApplicationEMailData

			Dim result As EMailData = Nothing

			Dim sql As String

			sql = "[Load Assigned Application EMail Data]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationID", ReplaceMissing(applicationId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailDate", ReplaceMissing(searchDate, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New EMailData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.EMailDate = SafeGetDateTime(reader, "EMailDate", Nothing)
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.ApplicationID = SafeGetInteger(reader, "ApplicationID", Nothing)
					result.EmployeeID = SafeGetInteger(reader, "EmployeeID", Nothing)
					result.EMailUidl = SafeGetInteger(reader, "EMailUidl", Nothing)
					result.EMLFilename = SafeGetString(reader, "EMLFileName")

				End If

				Return result


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			' Error case
			Return Nothing

		End Function

		Function LoadExistingAssigendEMailData(ByVal eMailData As EMailData) As EMailData Implements IEMailJobDatabaseAccess.LoadExistingAssigendEMailData

			Dim result As EMailData = Nothing

			Dim sql As String

			sql = "[Load Assigned EMail Data For Duplicate Check]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(eMailData.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailSubject", ReplaceMissing(eMailData.EMailSubject, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailBody", ReplaceMissing(eMailData.EMailBody, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailPlainTextBody", ReplaceMissing(eMailData.EMailPlainTextBody, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New EMailData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.EMailDate = SafeGetDateTime(reader, "EMailDate", Nothing)
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.ApplicationID = SafeGetInteger(reader, "ApplicationID", Nothing)
					result.EmployeeID = SafeGetInteger(reader, "EmployeeID", Nothing)
					result.EMailUidl = SafeGetInteger(reader, "EMailUidl", Nothing)
					result.EMLFilename = SafeGetString(reader, "EMLFileName")

				End If

				Return result


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			' Error case
			Return Nothing

		End Function


		Function AddEMailAttachmentJob(ByVal eMailID As Integer, ByVal attachmentData As EMailAttachment) As Boolean Implements IEMailJobDatabaseAccess.AddEMailAttachmentJob

			Dim success = True

			Dim sql As String

			sql = "[Create New EMailAttachment]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("AttachmentName", ReplaceMissing(attachmentData.AttachmentName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanContent", ReplaceMissing(attachmentData.AttachmentSize, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(attachmentData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocumentCategoryNumber", ReplaceMissing(attachmentData.DocumentCategoryNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailID", ReplaceMissing(eMailID, DBNull.Value)))


			Dim recNrParameter = New SqlClient.SqlParameter("@NewID", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not recNrParameter.Value Is Nothing Then
				attachmentData.ID = CType(recNrParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function


	End Class


End Namespace
