
Imports SP.DatabaseAccess.PayrollMng.DataObjects


Namespace PayrollMng

	Partial Public Class PayrollDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IPayrollDatabaseAccess


		''' <summary>
		''' Adds a new LO protocol data.
		''' </summary>
		''' <param name="loProtcolData">The LO protocol data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddLOProtocolData(ByVal loProtcolData As LOProtocolData) As Boolean Implements IPayrollDatabaseAccess.AddLOProtocolData

			Dim success = True

			Dim sql As String

			sql = "INSERT INTO "
			sql &= "[dbo].[LOProtokoll] ("
			sql &= "[LONr] "
			sql &= ",[MANr]"
			sql &= ",[MDNr]"
			sql &= ",[LP]"
			sql &= ",[Jahr]"
			sql &= ",[Protokoll]"
			sql &= ",[DebugValue]"
			sql &= ",[CreatedOn]"
			sql &= ") "
			sql &= "VALUES ("
			sql &= "@LONr"
			sql &= ", @MANr"
			sql &= ", @MDNr"
			sql &= ", @LP"
			sql &= ", @Jahr"
			sql &= ", @Protokoll"
			sql &= ", @DebugValue"
			sql &= ", GetDate())"


			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(loProtcolData.LONr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(loProtcolData.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(loProtcolData.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LP", ReplaceMissing(loProtcolData.LP, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(loProtcolData.Jahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Protokoll", ReplaceMissing(loProtcolData.Protokoll, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DebugValue", ReplaceMissing(loProtcolData.DebugValue, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success

		End Function

		''' <summary>
		''' Adds a new LO protocol data.
		''' </summary>
		''' <param name="employeeNumber">The employeeNumber.</param>
		''' <returns>string value of Protocolcontent.</returns>
		Function LoadLOProtocol(ByVal mandantNumber As Integer, ByVal employeeNumber As Integer?, ByVal month As Integer, ByVal year As Integer) As LOProtocolData Implements IPayrollDatabaseAccess.LoadLOProtocol

			Dim result As LOProtocolData = Nothing

			Dim sql As String
			sql = "Select Top 1 MANr, Protokoll, DebugValue, ID_LOProtokoll From [dbo].[LOProtokoll] "
			sql &= "Where (MDNr = @MDNr And LP = @LP And Jahr = @Jahr) And "

			If employeeNumber.HasValue Then
				sql &= "MANr = @MANr "
			Else
				sql &= "MANr Is Null "
			End If

			sql &= "Order By CreatedOn Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(mandantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(month, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(year, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LOProtocolData

					result.MANr = SafeGetInteger(reader, "MANr", Nothing)
					result.Protokoll = SafeGetString(reader, "Protokoll")
					result.DebugValue = SafeGetString(reader, "DebugValue")
					result.ID_LO_Protokoll = SafeGetInteger(reader, "ID_LOProtokoll", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadAssignedPayrollProtocolData(ByVal mandantNumber As Integer, ByVal employeeNumber As Integer?, ByVal payrollNumber As Integer?, ByVal month As Integer?, ByVal year As Integer?) As LOProtocolData Implements IPayrollDatabaseAccess.LoadAssignedPayrollProtocolData

			Dim result As LOProtocolData = Nothing

			Dim sql As String
			sql = "Select Top 1 MANr, Protokoll, DebugValue, ID_LOProtokoll "
			sql &= "From [dbo].[LOProtokoll] "
			sql &= "WHERE MDNr = @MDNr "
			sql &= "And (IsNull(@LP, 0) = 0 OR LP = @LP) "
			sql &= "And (ISNull(@Jahr, 0) = 0 OR Jahr = @Jahr) "
			sql &= "And (ISNull(@MANr, 0) = 0 OR MANr = @MANr) "
			sql &= "And (ISNull(@LONr, 0) = 0 OR LONr = @LONr) "
			sql &= "Order By CreatedOn Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(mandantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LP", ReplaceMissing(month, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@LONr", ReplaceMissing(payrollNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LOProtocolData

					result.MANr = SafeGetInteger(reader, "MANr", Nothing)
					result.Protokoll = SafeGetString(reader, "Protokoll")
					result.DebugValue = SafeGetString(reader, "DebugValue")
					result.ID_LO_Protokoll = SafeGetInteger(reader, "ID_LOProtokoll", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


	End Class

End Namespace
