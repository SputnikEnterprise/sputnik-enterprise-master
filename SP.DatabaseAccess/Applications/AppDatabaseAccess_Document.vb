
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports System.Text
'Imports System.Transactions


Namespace Applicant



	Partial Class AppDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IAppDatabaseAccess


		''' <summary>
		''' Loads all Document Data
		''' </summary>
		''' <returns>The user data.</returns>
		Function LoadDocumentData(ByVal customerID As String, ByVal businessBranch As String) As IEnumerable(Of ApplicantDocumentData) Implements IAppDatabaseAccess.LoadDocumentData

			Dim result As List(Of ApplicantDocumentData) = Nothing

			Dim sql As String


			sql = "SELECT D.ID"
			sql &= ",D.FK_ApplicantID"
			sql &= ",D.Type"
			sql &= ",D.Flag"
			sql &= ",D.Title"
			sql &= ",D.FileExtension"
			sql &= ",D.CreatedOn"
			sql &= ",D.CreatedFrom"

			sql &= " FROM [dbo].tbl_applicant_Document D "
			sql &= " Left Join tbl_Applicant A On D.FK_ApplicantID = A.ID"
			sql &= " Where (A.Customer_ID = @CustomerID)"
			sql &= " Order By D.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(businessBranch, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ApplicantDocumentData)

					While reader.Read

						Dim data = New ApplicantDocumentData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.FK_ApplicantID = SafeGetInteger(reader, "FK_ApplicantID", 0)
						data.Flag = SafeGetInteger(reader, "Flag", Nothing)
						data.Type = SafeGetInteger(reader, "Type", Nothing)

						data.Title = SafeGetString(reader, "Title")
						data.FileExtension = SafeGetString(reader, "FileExtension")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


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

		''' <summary>
		''' Checks if the document hashvalue exists.
		''' </summary>
		Function ExistsApplicantDocumentWithHashData(ByVal customerID As String, ByVal hashData As String) As ApplicantDocumentData Implements IAppDatabaseAccess.ExistsApplicantDocumentWithHashData

			Dim result As ApplicantDocumentData = Nothing

			Dim sql As String


			sql = "Select D.ID, D.FK_ApplicantID FROM [dbo].tbl_applicant_Document D"
			sql &= " Left Join [dbo].tbl_applicant A On D.FK_ApplicantID = A.ID"
			sql &= " Where A.Customer_ID = @customerID"
			sql &= " And D.HashValue = @hashData;"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("hashData", ReplaceMissing(hashData, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.Text)

			result = New ApplicantDocumentData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.FK_ApplicantID = SafeGetInteger(reader, "FK_ApplicantID", 0)

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			' Error case
			Return result

		End Function

		''' <summary>
		''' Loads all Document Data
		''' </summary>
		''' <returns>The user data.</returns>
		Function LoadAssignedDocumentData(ByVal recID As Integer) As ApplicantDocumentData Implements IAppDatabaseAccess.LoadAssignedDocumentData

			Dim result As ApplicantDocumentData = Nothing

			Dim sql As String


			sql = "SELECT D.ID"
			sql &= ",D.FK_ApplicantID"
			sql &= ",D.Type"
			sql &= ",D.Flag"
			sql &= ",D.Title"
			sql &= ",D.FileExtension"
			sql &= ",D.Content"
			sql &= ",D.CreatedOn"
			sql &= ",D.CreatedFrom"

			sql &= " FROM [dbo].tbl_applicant_Document D "
			sql &= " Where D.ID = @recID"
			sql &= " Order By D.ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(recID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New ApplicantDocumentData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.FK_ApplicantID = SafeGetInteger(reader, "FK_ApplicantID", 0)
					result.Flag = SafeGetInteger(reader, "Flag", Nothing)
					result.Type = SafeGetInteger(reader, "Type", Nothing)

					result.Title = SafeGetString(reader, "Title")
					result.FileExtension = SafeGetString(reader, "FileExtension")
					result.Content = SafeGetByteArray(reader, "Content")
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadAssignedDocumentContentData(ByVal docID As Integer) As Byte() Implements IAppDatabaseAccess.LoadAssignedDocumentContentData

			Dim result As Byte() = Nothing

			Dim sql As String


			sql = "SELECT D.Content"
			sql &= " FROM [dbo].tbl_applicant_Document D "
			sql &= " Where"
			sql &= " D.ID = @docID"
			sql &= " AND D.Content Is Not Null"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("docID", ReplaceMissing(docID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetByteArray(reader, "Content")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function DeleteAssignedDocument(ByVal customerID As String, ByVal docID As Integer) As Boolean Implements IAppDatabaseAccess.DeleteAssignedDocument

			Dim success = True

			Dim sql As String


			sql = "DELETE tbl_applicant_Document"
			sql &= " Where "
			sql &= " D.ID = @docID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("docID", ReplaceMissing(docID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


	End Class


End Namespace
