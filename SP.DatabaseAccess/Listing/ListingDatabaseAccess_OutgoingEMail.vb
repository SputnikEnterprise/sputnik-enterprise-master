
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language



Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadAssignedModulOutgoingEMailData(ByVal CustomerID As String, ByVal modulNumber As Integer?, ByVal number As Integer?) As IEnumerable(Of OutgoingEMailData) Implements IListingDatabaseAccess.LoadAssignedModulOutgoingEMailData
			Dim result As List(Of OutgoingEMailData) = Nothing

			Dim SQL As String

			SQL = "[Load Outgoing EMail Data For Assigned ModulNumber]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", CustomerID))
			listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", ReplaceMissing(modulNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Number", ReplaceMissing(number, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				result = New List(Of OutgoingEMailData)
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim overviewData As New OutgoingEMailData

					overviewData.ID = SafeGetInteger(reader, "ID", 0)
					overviewData.Customer_ID = SafeGetString(reader, "Customer_ID")
					overviewData.ModulNumber = SafeGetInteger(reader, "ModulNumber", 0)
					overviewData.Number = SafeGetInteger(reader, "Number", 0)
					overviewData.Receiver = SafeGetString(reader, "Receiver")
					overviewData.Sender = SafeGetString(reader, "Sender")

					overviewData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
					overviewData.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", 0)
					overviewData.CreatedFrom = SafeGetString(reader, "createdfrom")


					result.Add(overviewData)

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function AddOutgoingEMailData(ByVal customer_ID As String, ByVal data As OutgoingEMailData) As Boolean Implements IListingDatabaseAccess.AddOutgoingEMailData

			Dim success = True

			Dim sql As String

			sql = "[Create New Outgoing EMail Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(data.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", ReplaceMissing(data.ModulNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Number", ReplaceMissing(data.Number, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("sender", ReplaceMissing(data.Sender, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Receiver", ReplaceMissing(data.Receiver, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(data.CreatedUserNumber, DBNull.Value)))

			' New id 
			Dim newIDParameter = New SqlClient.SqlParameter("@NewID", SqlDbType.Int)
			newIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIDParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIDParameter.Value Is Nothing Then
				data.ID = CType(newIDParameter.Value, Integer)

			Else
				success = False

			End If


			Return success

		End Function



	End Class

End Namespace
