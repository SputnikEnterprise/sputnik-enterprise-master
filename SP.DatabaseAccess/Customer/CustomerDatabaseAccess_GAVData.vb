Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Report.DataObjects

Namespace Customer


	Partial Class CustomerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICustomerDatabaseAccess


		''' <summary>
		''' Loads assigned customer GAV group data (KD_GAVGruppe).
		''' </summary>
		''' <returns>List of GAV group data.</returns>
		Function LoadAssignedGAVGroupDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedGAVGroupData) Implements ICustomerDatabaseAccess.LoadAssignedGAVGroupDataOfCustomer

			Dim result As List(Of CustomerAssignedGAVGroupData) = Nothing

			Dim sql As String

			sql = "SELECT ID, KDNr, Bezeichnung, Kanton, GAVNumber FROM KD_GAVGruppe WHERE  KDNr = @customerNumber Order By Bezeichnung ASC"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(customerNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerAssignedGAVGroupData)

					While reader.Read()
						Dim gavGroupData As New CustomerAssignedGAVGroupData
						gavGroupData.ID = SafeGetInteger(reader, "ID", 0)
						gavGroupData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						gavGroupData.Description = SafeGetString(reader, "Bezeichnung")
						gavGroupData.Canton = SafeGetString(reader, "Kanton")
						gavGroupData.GAVNUmber = SafeGetInteger(reader, "GAVNumber", Nothing)

						result.Add(gavGroupData)

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

		Function LoadAllCustomerGAVGroupData(ByVal mdNr As Integer, ByVal kst As String) As IEnumerable(Of CustomerAssignedGAVGroupData) Implements ICustomerDatabaseAccess.LoadAllCustomerGAVGroupData

			Dim result As List(Of CustomerAssignedGAVGroupData) = Nothing

			Dim sql As String

			sql = "[Load GAV Data From All Customers]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kst", ReplaceMissing(kst, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerAssignedGAVGroupData)

					While reader.Read()
						Dim gavGroupData As New CustomerAssignedGAVGroupData
						gavGroupData.GAVNUmber = SafeGetInteger(reader, "GAVNumber", Nothing)

						result.Add(gavGroupData)

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


	End Class

End Namespace
