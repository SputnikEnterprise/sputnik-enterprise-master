
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function AddAssignedCustomerMasterDataFromAnotherDatabase(ByVal customer As CustomerTranferData) As Boolean Implements IListingDatabaseAccess.AddAssignedCustomerMasterDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Import Assigned Customer Master Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(customer.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customer.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerNumberOffset", ReplaceMissing(customer.DestCustomerOffsetNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCustomerNumber", ReplaceMissing(customer.SourceCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(customer.SourceDataBaseName, DBNull.Value)))


			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewCustomer", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				customer.DestNewCustomerNumber = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function

		Function AddAssignedCResponsibleDataFromAnotherDatabase(ByVal cResponsible As CResponsiblePersonTranferData) As Boolean Implements IListingDatabaseAccess.AddAssignedCResponsibleDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Import Assigned Customer Responsible Person Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(cResponsible.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(cResponsible.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestCustomerNumber", ReplaceMissing(cResponsible.DestCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCustomerNumber", ReplaceMissing(cResponsible.SourceCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCResponsibleNumber", ReplaceMissing(cResponsible.SourceCResponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(cResponsible.SourceDataBaseName, DBNull.Value)))


			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewCResponsible", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				cResponsible.DestNewCResponsibleNumber = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function

		Function AddAssignedCustomerPeripherieDataFromAnotherDatabase(ByVal customer As CustomerTranferData) As Boolean Implements IListingDatabaseAccess.AddAssignedCustomerPeripherieDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Import Assigned Customer Peripherie Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(customer.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customer.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestCustomerOffsetNumber", ReplaceMissing(customer.DestCustomerOffsetNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCustomerNumber", ReplaceMissing(customer.SourceCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(customer.SourceDataBaseName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function UpdateAssignedCustomerPeripherieDataFromAnotherDatabase(ByVal customer As CustomerTranferData) As Boolean Implements IListingDatabaseAccess.UpdateAssignedCustomerPeripherieDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Update Assigned Customer Peripherie Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(customer.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customer.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestNewCustomerNumber", ReplaceMissing(customer.DestNewCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCustomerNumber", ReplaceMissing(customer.SourceCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(customer.SourceDataBaseName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function AddAssignedCResponsiblePeripherieDataFromAnotherDatabase(ByVal cResponsible As CResponsiblePersonTranferData) As Boolean Implements IListingDatabaseAccess.AddAssignedCResponsiblePeripherieDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Import Assigned Customer Responsible Person Peripherie Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(cResponsible.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(cResponsible.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestNewCustomerNumber", ReplaceMissing(cResponsible.DestCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestNewCResponsibleNumber", ReplaceMissing(cResponsible.DestNewCResponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCustomerNumber", ReplaceMissing(cResponsible.SourceCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCResponsibleNumber", ReplaceMissing(cResponsible.SourceCResponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(cResponsible.SourceDataBaseName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function UpdateAssignedCResponsiblePeripherieDataFromAnotherDatabase(ByVal cResponsible As CResponsiblePersonTranferData) As Boolean Implements IListingDatabaseAccess.UpdateAssignedCResponsiblePeripherieDataFromAnotherDatabase

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Update Assigned Customer Responsible Person Peripherie Data From Another Database]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(cResponsible.DestMDNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(cResponsible.DestCustomerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestNewCustomerNumber", ReplaceMissing(cResponsible.DestCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DestNewCResponsibleNumber", ReplaceMissing(cResponsible.DestNewCResponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCustomerNumber", ReplaceMissing(cResponsible.SourceCustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SourceCResponsibleNumber", ReplaceMissing(cResponsible.SourceCResponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dataBaseName", ReplaceMissing(cResponsible.SourceDataBaseName, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function DeleteAssignedImportedCustomerData(ByVal customerNumber As Integer) As Boolean Implements IListingDatabaseAccess.DeleteAssignedImportedCustomerData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[ADMIN-Delete Assigned Customer]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function




	End Class


End Namespace
