
Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


#Region "document methodes"


#Region "viewing data"

		Function LoadAssignedCVLDocumentData(ByVal cvlPrifleID As Integer) As IEnumerable(Of DocumentViewData) Implements ICVLizerDatabaseAccess.LoadAssignedCVLDocumentData
			Dim result As List(Of DocumentViewData) = Nothing

			Dim sql = "[Load Assigned CVL Documents]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of DocumentViewData)

					While reader.Read
						Dim data = New DocumentViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.DocClass = SafeGetString(reader, "DocClass")
						data.Pages = SafeGetInteger(reader, "Pages", Nothing)
						data.Plaintext = SafeGetString(reader, "Plaintext")
						data.FileType = SafeGetString(reader, "FileType")
						data.DocID = SafeGetInteger(reader, "DocID", Nothing)
						data.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
						data.DocLanguage = SafeGetString(reader, "DocLanguage")
						data.FileHashvalue = SafeGetString(reader, "FileHashvalue")
						data.DocXML = SafeGetString(reader, "DocXML")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Function LoadAssignedDocumentData(ByVal id As Integer) As DocumentViewData Implements ICVLizerDatabaseAccess.LoadAssignedDocumentData
			Dim result As DocumentViewData = Nothing

			Dim sql = "[Load Assigned Document Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(id, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New DocumentViewData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.DocClass = SafeGetString(reader, "DocClass")
						result.Pages = SafeGetInteger(reader, "Pages", Nothing)
						result.Plaintext = SafeGetString(reader, "Plaintext")
						result.FileType = SafeGetString(reader, "FileType")
						result.DocBinary = SafeGetByteArray(reader, "DocBinary")
						result.DocID = SafeGetInteger(reader, "DocID", Nothing)
						result.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
						result.DocLanguage = SafeGetString(reader, "DocLanguage")
						result.FileHashvalue = SafeGetString(reader, "FileHashvalue")
						result.DocXML = SafeGetString(reader, "DocXML")

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Function LoadAssignedPersonalInformationDataWithFileHashValueData(ByVal customerID As String, ByVal fileHashvalue As String) As PersonalInformationData Implements ICVLizerDatabaseAccess.LoadAssignedPersonalInformationDataWithFileHashValueData
			Dim result As PersonalInformationData = Nothing

			Dim sql = "[Load Assigned CVLPersonalInformation With Document Filehash Values]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("FileHashvalue", ReplaceMissing(fileHashvalue, String.Empty)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New PersonalInformationData

					While reader.Read

						result.PersonalID = SafeGetInteger(reader, "FK_CVLID", Nothing)
						result.FirstName = SafeGetString(reader, "Firstname")
						result.LastName = SafeGetString(reader, "Lastname")
						result.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLPersonalPhotoViewData(ByVal cvlPrifleID As Integer) As DocumentData
			Dim result As DocumentData = Nothing

			Dim sql = "[Load Assigned CVL Personal Photo]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New DocumentData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.DocClass = SafeGetString(reader, "DocClass")
						result.Pages = SafeGetInteger(reader, "Pages", Nothing)
						result.Plaintext = SafeGetString(reader, "Plaintext")
						result.FileType = SafeGetString(reader, "FileType")
						result.DocBinary = SafeGetByteArray(reader, "DocBinary")
						result.DocID = SafeGetInteger(reader, "DocID", Nothing)
						result.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
						result.DocLanguage = SafeGetString(reader, "DocLanguage")
						result.FileHashvalue = SafeGetString(reader, "FileHashvalue")
						result.DocXML = SafeGetString(reader, "DocXML")

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result
		End Function


#End Region




		Function AddParsingFileHashData(ByVal customerID As String, ByVal myFilename As String, ByVal fileHashvalue As String) As Boolean Implements ICVLizerDatabaseAccess.AddParsingFileHashData
			Dim success As Boolean

			Dim sql = "[Add File Hash Info]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("myFilename", ReplaceMissing(myFilename, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("FileHashvalue", ReplaceMissing(fileHashvalue, String.Empty)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success
		End Function

		Function AddCVLDocumentData(ByVal cvlProfileID As Integer, ByVal data As DocumentData) As Boolean Implements ICVLizerDatabaseAccess.AddCVLDocumentData
			Dim success As Boolean

			Dim sql = "[CreateCVLDocument]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)


			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlProfileID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocClass", ReplaceMissing(data.DocClass, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Pages", ReplaceMissing(data.Pages, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Plaintext", ReplaceMissing(data.Plaintext, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FileType", ReplaceMissing(data.FileType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocBinary", ReplaceMissing(data.DocBinary, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocID", ReplaceMissing(data.DocID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocSize", ReplaceMissing(data.DocSize, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocLanguage", ReplaceMissing(data.DocLanguage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FileHashvalue", ReplaceMissing(data.FileHashvalue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Docxml", ReplaceMissing(data.DocXML, DBNull.Value)))


			' Output Parameters
			Dim newDocumentIdParameter = New SqlClient.SqlParameter("@NewDocumentId", SqlDbType.Int)
			newDocumentIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newDocumentIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newDocumentIdParameter.Value Is Nothing Then
				data.ID = CType(newDocumentIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success
		End Function







#End Region


	End Class


End Namespace
