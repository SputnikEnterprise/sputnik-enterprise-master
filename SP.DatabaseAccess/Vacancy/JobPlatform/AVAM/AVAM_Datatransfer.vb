
Imports System.Data.SqlClient


Namespace JobPlatform.AVAM

	Partial Class AVAMDataBase
		Inherits DatabaseAccessBase
		Implements IAVAMDatabaseAccess


		Function AddAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal jobroomID As String, ByVal resultContent As String, ByVal syncFrom As String) As Boolean Implements IAVAMDatabaseAccess.AddAVAMNotifyResultData
			Dim success As Boolean = True


			Dim sql As String
			sql = "[Add AVAM Query Result]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("customer_ID", customerID))
			listOfParams.Add(New SqlParameter("User_ID", ReplaceMissing(userid, DBNull.Value)))
			listOfParams.Add(New SqlParameter("JobroomID", ReplaceMissing(jobroomID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ResultContent", ReplaceMissing(resultContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(syncFrom, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If Not newIdParameter.Value Is Nothing Then
					success = True
				Else
					success = False
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			Finally

			End Try

			Return success
		End Function



	End Class


End Namespace
