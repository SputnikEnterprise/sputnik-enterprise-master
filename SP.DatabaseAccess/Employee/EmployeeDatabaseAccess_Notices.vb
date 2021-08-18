
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Namespace Employee

	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess


		''' <summary>
		''' Loads employee notices data (tbl_ma_notices).
		''' </summary>
		Function LoadEmployeeNoticesData(ByVal employeeNumber As Integer) As EmployeeNoticesData Implements IEmployeeDatabaseAccess.LoadEmployeeNoticesData

			Dim employeeMasterData As EmployeeNoticesData = Nothing

			Dim sql As String

			sql = "SELECT ID"
			sql &= ",MN.EmployeeNumber "
			sql &= ",MN.Notice_Common V_Hinweis"
			sql &= ",MN.Notice_Employment "
			sql &= ",MN.Notice_Report "
			sql &= ",MN.Notice_AdvancedPayment "
			sql &= ",MN.Notice_Payroll "
			sql &= ",MN.[CreatedOn]"
			sql &= ",MN.[ChangedOn]"
			sql &= ",MN.[CreatedFrom]"
			sql &= ",MN.[ChangedFrom]"

			sql &= " FROM dbo.tbl_MA_Notices MN "
			sql &= "WHERE MN.EmployeeNumber = @employeeNumber "

			' Parameters
			Dim employeeNumberParameter As New SqlClient.SqlParameter("employeeNumber", employeeNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(employeeNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						employeeMasterData = New EmployeeNoticesData

						employeeMasterData.ID = SafeGetInteger(reader, "ID", 0)
						employeeMasterData.EmployeeNumber = SafeGetInteger(reader, "EmployeeNumber", 0)

						employeeMasterData.V_Hint = SafeGetString(reader, "V_Hinweis")
						employeeMasterData.Notice_Employment = SafeGetString(reader, "Notice_Employment")
						employeeMasterData.Notice_Report = SafeGetString(reader, "Notice_Report")
						employeeMasterData.Notice_AdvancedPayment = SafeGetString(reader, "Notice_AdvancedPayment")
						employeeMasterData.Notice_Payroll = SafeGetString(reader, "Notice_Payroll")

						employeeMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						employeeMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						employeeMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						employeeMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom")

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				employeeMasterData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return employeeMasterData

			Return Nothing
		End Function

		''' <summary>
		''' Updates employee notices data (tbl_ma_Notices).
		''' </summary>
		Function UpdateEmployeeNoticesData(ByVal notticeData As EmployeeNoticesData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeNoticesData

		Dim success = True

		Dim sql As String

		sql = "Update [dbo].[tbl_MA_Notices] Set "
		sql &= "[Notice_Common] = @v_Hint"
		sql &= ",[Notice_Employment] = @Notice_Employment"
		sql &= ",[Notice_Report] = @Notice_Report"
		sql &= ",[Notice_AdvancedPayment] = @Notice_AdvancedPayment"
		sql &= ",[Notice_Payroll] = @Notice_Payroll"

		sql &= ",[ChangedOn] = GetDate()"
		sql &= ",[ChangedFrom] = @ChangedFrom"
		sql &= ",[ChangedUserNumber] = @ChangedUserNumber"

		sql = sql & " WHERE EmployeeNumber = @employeeNumber; "

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(notticeData.EmployeeNumber, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("v_Hint", ReplaceMissing(notticeData.V_Hint, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Notice_Employment", ReplaceMissing(notticeData.Notice_Employment, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Notice_Report", ReplaceMissing(notticeData.Notice_Report, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Notice_AdvancedPayment", ReplaceMissing(notticeData.Notice_AdvancedPayment, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Notice_Payroll", ReplaceMissing(notticeData.Notice_Payroll, DBNull.Value)))

		listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(notticeData.ChangedFrom, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(notticeData.ChangedUserNumber, DBNull.Value)))

		success = ExecuteNonQuery(sql, listOfParams)


		Return success

	End Function


	End Class

End Namespace