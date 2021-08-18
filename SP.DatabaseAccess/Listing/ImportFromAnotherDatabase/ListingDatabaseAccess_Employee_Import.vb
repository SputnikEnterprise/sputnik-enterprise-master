
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function AddAssignedEmployeeMasterDataFromAnotherDatabase(ByVal employee As EmployeeTranferData) As Boolean Implements IListingDatabaseAccess.AddAssignedEmployeeMasterDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Import Assigned Employee Master Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(employee.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(employee.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumberOffset", ReplaceMissing(employee.DestEmployeeOffsetNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sourceEmployeeNumber", ReplaceMissing(employee.SourceEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(employee.SourceDataBaseName, DBNull.Value)))


			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewEmployee", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				employee.DestNewEmployeeNumber = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function

		Function AddAssignedEmployeePeripherieDataFromAnotherDatabase(ByVal employee As EmployeeTranferData) As Boolean Implements IListingDatabaseAccess.AddAssignedEmployeePeripherieDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Import Assigned Employee Peripherie Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(employee.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(employee.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destNewEmployeeNumber", ReplaceMissing(employee.DestNewEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sourceEmployeeNumber", ReplaceMissing(employee.SourceEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(employee.SourceDataBaseName, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function UpdateAssignedEmployeePeripherieDataFromAnotherDatabase(ByVal employee As EmployeeTranferData) As Boolean Implements IListingDatabaseAccess.UpdateAssignedEmployeePeripherieDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Update Assigned Employee Peripherie Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(employee.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(employee.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destNewEmployeeNumber", ReplaceMissing(employee.DestNewEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("sourceEmployeeNumber", ReplaceMissing(employee.SourceEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(employee.SourceDataBaseName, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function
		Function DeleteAssignedImportedEmployeeData(ByVal employeeNumber As Integer) As Boolean Implements IListingDatabaseAccess.DeleteAssignedImportedEmployeeData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[ADMIN-Delete Assigned Employee]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function




	End Class


End Namespace
