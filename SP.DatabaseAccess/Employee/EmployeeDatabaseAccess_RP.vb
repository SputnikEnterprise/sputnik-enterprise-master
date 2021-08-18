
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng


Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess




		''' <summary>
		''' Loads employee report data.
		''' </summary>
		''' <returns>List of employee div-address data.</returns>
		Function LoadEmployeeReportAddressData(ByVal employeeNumber As Integer) As EmployeeSAddressData Implements IEmployeeDatabaseAccess.LoadEmployeeReportAddressData

			Dim result As EmployeeSAddressData = Nothing

			Dim sql As String

			sql = "[Get MAAdressData For Selected MA In Report]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANummer", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			result = New EmployeeSAddressData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.Gender = SafeGetString(reader, "Geschlecht")
					result.Lastname = SafeGetString(reader, "MANachname")
					result.Firstname = SafeGetString(reader, "MAVorname")
					result.StaysAt = SafeGetString(reader, "MACo")
					result.PostOfficeBox = SafeGetString(reader, "MAPostfach")
					result.Street = SafeGetString(reader, "MAStrasse")
					result.Postcode = SafeGetString(reader, "MAPLZ")
					result.Location = SafeGetString(reader, "MAOrt")
					result.Country = SafeGetString(reader, "MALand")
					result.Add_Bemerkung = SafeGetString(reader, "Add_Bemerkung")
					result.Add_Res1 = SafeGetString(reader, "Add_Res1")
					result.Add_Res2 = SafeGetString(reader, "Add_Res2")
					result.Add_Res3 = SafeGetString(reader, "Add_Res3")


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
