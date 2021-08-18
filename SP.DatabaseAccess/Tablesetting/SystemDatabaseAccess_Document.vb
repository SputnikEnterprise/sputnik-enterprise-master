
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


#Region "Document Data"

		Function LoadTemplateDataForSendBulkCustomer(ByVal tplArt As String) As IEnumerable(Of DocumentData) Implements ITablesDatabaseAccess.LoadTemplateDataForSendBulkCustomer
			Dim result As List(Of DocumentData) = Nothing

			Dim SQL As String

			SQL = "Select DocDb.JobNr, DocDb.Bezeichnung From Dokprint DocDb Where DocDb.JobNr Like @JobNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If tplArt.ToUpper = "FAX".ToUpper Then
				listOfParams.Add(New SqlClient.SqlParameter("JobNr", "Offer.Fax%"))
			ElseIf tplArt.ToUpper = "Mail".ToUpper Then
				listOfParams.Add(New SqlClient.SqlParameter("JobNr", "Offer.eMail%"))
			Else
				listOfParams.Add(New SqlClient.SqlParameter("JobNr", "15.%"))

			End If

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of DocumentData)

					While reader.Read()
						Dim overviewData As New DocumentData

						overviewData.JobNr = SafeGetString(reader, "JobNr")
						overviewData.Bezeichnung = SafeGetString(reader, "Bezeichnung")


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

		''' <summary>
		''' Loads all Document Data
		''' </summary>
		''' <returns>The user data.</returns>
		Public Function LoadDocumentData(ByVal mandantenNumber As Integer, ByVal userNumber As Integer) As IEnumerable(Of DocumentData) Implements ITablesDatabaseAccess.LoadDocumentData

			Dim result As List(Of DocumentData) = Nothing

			Dim sql As String

			'sql = "[List User DocRights Data]"

			sql = "SELECT D.[ID]"
			sql &= ",D.[JobNr]"
			sql &= ",D.[DocName]"
			sql &= ",D.[Bezeichnung]"

			sql &= ",U.id UDR_ID"
			sql &= ",U.[MDNr]"
			sql &= ",U.[USNr]"
			sql &= ",U.[ChangedOn]"
			sql &= ",U.[ChangedFrom]"
			sql &= ",U.[AllowedToExport]"
			sql &= ",[LogActivity]"

			sql &= " FROM [dbo].[DOKPrint] D "
			sql &= "Left Join [dbo].[UserDocumentRights] U On U.JobNr = D.JobNr And U.MDNr = @MDNr And U.USNr = @USNr"

			sql &= " Order By JobNr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", mandantenNumber))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", userNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)	', CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of DocumentData)

					While reader.Read

						Dim data = New DocumentData()

						data.recid = SafeGetInteger(reader, "ID", 0)
						data.UDR_ID = SafeGetInteger(reader, "UDR_ID", Nothing)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.USNr = SafeGetInteger(reader, "USNr", 0)

						data.JobNr = SafeGetString(reader, "jobNr")
						data.DocName = SafeGetString(reader, "DocName")
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")

						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")

						data.AllowedToExport = SafeGetBoolean(reader, "AllowedToExport", False)
						data.LogActivity = SafeGetBoolean(reader, "LogActivity", False)

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
		''' update assigned user document rights data.
		''' </summary>
		''' <returns>boolean</returns>
		Public Function UpdateAssignedUserDocumentRightsData(ByVal data As DocumentData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedUserDocumentRightsData

			Dim success As Boolean = True

			Dim sql As String

			sql = "Update UserDocumentRights Set "
			sql &= "[MDNr] = @MDNr"
			sql &= ",[USNr] = @UsNr"
			sql &= ",[JobNr] = @JobNr"
			sql &= ",[ChangedOn] = GetDate()"
			sql &= ",[ChangedFrom] = @ChangedFrom"
			sql &= ",[AllowedToExport] = @AllowedToExport"
			sql &= ",[LogActivity] = @LogActivity"

			sql &= " Where [ID] = @UDR_ID "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("UDR_ID", data.UDR_ID))

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", data.MDNR))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", data.USNr))
			listOfParams.Add(New SqlClient.SqlParameter("JobNr", ReplaceMissing(data.JobNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedToExport", ReplaceMissing(data.AllowedToExport, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LogActivity", ReplaceMissing(data.LogActivity, False)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' update user document for all users data.
		''' </summary>
		''' <returns>boolean</returns>
		Public Function UpdateAssignedUserDocumentRightsForAllUsersData(ByVal data As DocumentData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedUserDocumentRightsForAllUsersData

			Dim success As Boolean = True

			Dim sql As String

			sql = "Delete UserDocumentRights Where "
			sql &= "[USNr] Not In (1) And [JobNr] = @JobNr And [MDNr] = @MDNr And [USNr] = @UsNr; "

			sql &= "Insert Into UserDocumentRights ("
			sql &= "[MDNr]"
			sql &= ",[USNr]"
			sql &= ",[JobNr]"
			sql &= ",[ChangedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[AllowedToExport]"
			sql &= ",[LogActivity]"
			sql &= ") "
			sql &= "Values ("
			sql &= "@MDNr"
			sql &= ",@UsNr"
			sql &= ",@JobNr"
			sql &= ",GetDate()"
			sql &= ",@ChangedFrom"
			sql &= ",@AllowedToExport"
			sql &= ",@LogActivity"

			sql &= "); "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", data.recid))

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", data.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", data.USNr))
			listOfParams.Add(New SqlClient.SqlParameter("JobNr", ReplaceMissing(data.JobNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedToExport", ReplaceMissing(data.AllowedToExport, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LogActivity", ReplaceMissing(data.LogActivity, False)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' add user document rights data.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function AddAssignedUserDocumentRightsData(ByVal data As DocumentData) As Boolean Implements ITablesDatabaseAccess.AddAssignedUserDocumentRightsData

			Dim success As Boolean = True

			Dim sql As String = "[Create New UserDocumentRight]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.USNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("JobNr", ReplaceMissing(data.JobNr, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedToExport", ReplaceMissing(data.AllowedToExport, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LogActivity", ReplaceMissing(data.LogActivity, False)))


			Try
				' New ID of user
				Dim newIdParameter = New SqlClient.SqlParameter("NewID", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success Then
					If Not newIdParameter.Value Is Nothing Then
						data.UDR_ID = CType(newIdParameter.Value, Integer)
					End If

				Else
					success = False
				End If

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function



#End Region



	End Class


End Namespace

