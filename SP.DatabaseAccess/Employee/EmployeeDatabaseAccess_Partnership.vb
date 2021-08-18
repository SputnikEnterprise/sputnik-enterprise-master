
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng


Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess


		Function LoadExistingEmployeeForSelectingPartnershipData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeMasterData) Implements IEmployeeDatabaseAccess.LoadExistingEmployeeForSelectingPartnershipData

			Dim result As List(Of EmployeeMasterData) = Nothing

			Dim sql As String

			sql = "[Load Existing Employee For Selecting In Partnership Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			result = New List(Of EmployeeMasterData)
			Try

				If (Not reader Is Nothing) Then

					While reader.Read()

						Dim data = New EmployeeMasterData

						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.Lastname = SafeGetString(reader, "Nachname")
						data.Firstname = SafeGetString(reader, "Vorname")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")

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
		''' Loads employee NLA data.
		''' </summary>
		''' <returns>List of employee div-address data.</returns>
		Function LoadEmployeePartnershipData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeePartnershipData) Implements IEmployeeDatabaseAccess.LoadEmployeePartnershipData

			Dim result As List(Of EmployeePartnershipData) = Nothing

			Dim sql As String

			sql = "[Load Employee Partnership Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeNumber", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			result = New List(Of EmployeePartnershipData)
			Try

				If (Not reader Is Nothing) Then

					While reader.Read()

						Dim data As New EmployeePartnershipData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.AddressNumber = SafeGetInteger(reader, "AddressNumber", Nothing)
						data.EmployeeNumber = SafeGetInteger(reader, "employeeNumber", Nothing)
						data.ExistingEmployeeNumber = SafeGetInteger(reader, "ExistingEmployeeNumber", Nothing)

						data.Gender = SafeGetString(reader, "Gender")
						data.Lastname = SafeGetString(reader, "Lastname")
						data.Firstname = SafeGetString(reader, "Firstname")
						data.PostOfficeBox = SafeGetString(reader, "PostOfficeBox")
						data.Street = SafeGetString(reader, "Street")
						data.Postcode = SafeGetString(reader, "Postcode")
						data.City = SafeGetString(reader, "City")
						data.Country = SafeGetString(reader, "Country")
						data.Birthdate = SafeGetDateTime(reader, "Birthdate", Nothing)
						data.SocialInsuranceNumber = SafeGetString(reader, "SocialInsuranceNumber")
						data.InEmployment = SafeGetBoolean(reader, "InEmployment", False)

						data.ValidFrom = SafeGetDateTime(reader, "ValidFrom", Nothing)
						data.ValidTo = SafeGetDateTime(reader, "ValidTo", Nothing)

						data.Createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.Createdfrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", Nothing)

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

		Function LoadEmployeeAssignedPartnershipData(ByVal recID As Integer) As EmployeePartnershipData Implements IEmployeeDatabaseAccess.LoadEmployeeAssignedPartnershipData

			Dim result As EmployeePartnershipData = Nothing

			Dim sql As String

			sql = "[Load Employee Assigned Partnership Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ID", recID))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			result = New EmployeePartnershipData
			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.ID = SafeGetInteger(reader, "ID", Nothing)
					result.AddressNumber = SafeGetInteger(reader, "AddressNumber", Nothing)
					result.EmployeeNumber = SafeGetInteger(reader, "employeeNumber", Nothing)
					result.ExistingEmployeeNumber = SafeGetInteger(reader, "ExistingEmployeeNumber", Nothing)

					result.Gender = SafeGetString(reader, "Gender")
					result.Lastname = SafeGetString(reader, "Lastname")
					result.Firstname = SafeGetString(reader, "Firstname")
					result.PostOfficeBox = SafeGetString(reader, "PostOfficeBox")
					result.Street = SafeGetString(reader, "Street")
					result.Postcode = SafeGetString(reader, "Postcode")
					result.City = SafeGetString(reader, "City")
					result.Country = SafeGetString(reader, "Country")
					result.Birthdate = SafeGetDateTime(reader, "Birthdate", Nothing)
					result.SocialInsuranceNumber = SafeGetString(reader, "SocialInsuranceNumber")
					result.InEmployment = SafeGetBoolean(reader, "InEmployment", False)

					result.ValidFrom = SafeGetDateTime(reader, "ValidFrom", Nothing)
					result.ValidTo = SafeGetDateTime(reader, "ValidTo", Nothing)
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.Createdfrom = SafeGetString(reader, "CreatedFrom")
					result.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", Nothing)
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					result.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", Nothing)

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
		''' Add employee div address data.
		''' </summary>
		Function AddEmployeePartnershipData(ByVal employeeNumber As Integer, ByVal data As EmployeePartnershipData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeePartnershipData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Create New Partnership Data For Assigned Employee]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("existingEmployeeNumber", ReplaceMissing(data.ExistingEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Gender", ReplaceMissing(data.Gender, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Lastname", ReplaceMissing(data.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Firstname", ReplaceMissing(data.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PostOfficeBox", ReplaceMissing(data.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Street", ReplaceMissing(data.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postcode", ReplaceMissing(data.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("City", ReplaceMissing(data.City, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Country", ReplaceMissing(data.Country, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Birthdate", ReplaceMissing(data.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SocialInsuranceNumber", ReplaceMissing(data.SocialInsuranceNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("InEmployment", ReplaceMissing(data.InEmployment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ValidFrom", ReplaceMissing(data.ValidFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ValidTo", ReplaceMissing(data.ValidTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.Createdfrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(data.CreatedUserNumber, DBNull.Value)))


			Dim resultNewIDParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			resultNewIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultNewIDParameter)

			Dim resultRecNrParameter = New SqlClient.SqlParameter("@NextAddressNumber", SqlDbType.Int)
			resultRecNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultRecNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not resultNewIDParameter.Value Is Nothing Then
				data.ID = CType(resultNewIDParameter.Value, Integer)
			Else
				success = False
			End If
			If success AndAlso Not resultRecNrParameter.Value Is Nothing Then
				data.AddressNumber = CType(resultRecNrParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function

		''' <summary>
		''' update employee div address data.
		''' </summary>
		Function UpdateEmployeePartnershipData(ByVal employeeNumber As Integer, ByVal data As EmployeePartnershipData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeePartnershipData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Update Assigned Partnership Data]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("existingEmployeeNumber", ReplaceMissing(data.ExistingEmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Gender", ReplaceMissing(data.Gender, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Lastname", ReplaceMissing(data.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Firstname", ReplaceMissing(data.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PostOfficeBox", ReplaceMissing(data.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Street", ReplaceMissing(data.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postcode", ReplaceMissing(data.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("City", ReplaceMissing(data.City, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Country", ReplaceMissing(data.Country, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Birthdate", ReplaceMissing(data.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SocialInsuranceNumber", ReplaceMissing(data.SocialInsuranceNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("InEmployment", ReplaceMissing(data.InEmployment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ValidFrom", ReplaceMissing(data.ValidFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ValidTo", ReplaceMissing(data.ValidTo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(data.ChangedUserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@recID", ReplaceMissing(data.ID, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		''' <summary>
		''' Delete employee NLA data.
		''' </summary>
		''' <returns>boolean.</returns>
		Function DeleteEmployeePartnershipData(ByVal recID As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeResult Implements IEmployeeDatabaseAccess.DeleteEmployeePartnershipData

			Dim success = True

			Dim sql As String

			sql = "[Delete Assigned Partnership Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ID", recID))
			listOfParams.Add(New SqlClient.SqlParameter("Modul", ReplaceMissing(modul, "Partnership")))
			listOfParams.Add(New SqlClient.SqlParameter("Username", ReplaceMissing(username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Usnr", ReplaceMissing(usnr, DBNull.Value)))

			Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteEmployeeResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteEmployeeResult)
				Catch
					resultEnum = DeleteEmployeeResult.ErrorWhileDelete
				End Try
			Else
				resultEnum = DeleteEmployeeResult.ErrorWhileDelete
			End If

			Return resultEnum

		End Function


	End Class


End Namespace
