Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Customer

	Partial Class CustomerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICustomerDatabaseAccess

		Function LoadCustomerNoticesData(ByVal customerNumber As Integer) As CustomerNoticesData Implements ICustomerDatabaseAccess.LoadCustomerNoticesData

			Dim noticeData As CustomerNoticesData = Nothing

			Dim sql As String

			sql = "SELECT ID"
			sql &= ",KN.CustomerNumber "
			sql &= ",KN.Notice_Common "
			sql &= ",KN.Notice_Employment "
			sql &= ",KN.Notice_Report "
			sql &= ",KN.Notice_Invoice "
			sql &= ",KN.Notice_Payment "
			sql &= ",KN.[CreatedOn]"
			sql &= ",KN.[ChangedOn]"
			sql &= ",KN.[CreatedFrom]"
			sql &= ",KN.[ChangedFrom]"

			sql &= " FROM dbo.tbl_KD_Notices KN "
			sql &= "WHERE KN.customerNumber = @customerNumber "

			' Parameters
			Dim employeeNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(employeeNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						noticeData = New CustomerNoticesData

						noticeData.ID = SafeGetInteger(reader, "ID", 0)
						noticeData.CustomerNumber = SafeGetInteger(reader, "CustomerNumber", 0)

						noticeData.Common = SafeGetString(reader, "Notice_Common")
						noticeData.Notice_Employment = SafeGetString(reader, "Notice_Employment")
						noticeData.Notice_Report = SafeGetString(reader, "Notice_Report")
						noticeData.Notice_Invoice = SafeGetString(reader, "Notice_Invoice")
						noticeData.Notice_Payment = SafeGetString(reader, "Notice_Payment")

						noticeData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						noticeData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						noticeData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						noticeData.ChangedFrom = SafeGetString(reader, "ChangedFrom")

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				noticeData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return noticeData

			Return Nothing
		End Function

		Function UpdateCustomerNoticesData(ByVal notticeData As CustomerNoticesData) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerNoticesData

			Dim success = True

			Dim sql As String

			sql = "Update [dbo].[tbl_KD_Notices] Set "
			sql &= "[Notice_Common] = @Comment"
			sql &= ",[Notice_Employment] = @Notice_Employment"
			sql &= ",[Notice_Report] = @Notice_Report"
			sql &= ",[Notice_Invoice] = @Notice_Invoice"
			sql &= ",[Notice_Payment] = @Notice_Payment"

			sql &= ",[ChangedOn] = GetDate()"
			sql &= ",[ChangedFrom] = @ChangedFrom"
			sql &= ",[ChangedUserNumber] = @ChangedUserNumber"

			sql = sql & " WHERE CustomerNumber = @CustomerNumber; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerNumber", ReplaceMissing(notticeData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Comment", ReplaceMissing(notticeData.Common, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Employment", ReplaceMissing(notticeData.Notice_Employment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Report", ReplaceMissing(notticeData.Notice_Report, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Invoice", ReplaceMissing(notticeData.Notice_Invoice, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Payment", ReplaceMissing(notticeData.Notice_Payment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(notticeData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(notticeData.ChangedUserNumber, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


	End Class

End Namespace