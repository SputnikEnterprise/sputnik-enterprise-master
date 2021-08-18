
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects

Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess


		''' <summary>
		''' Loads employee NLA data.
		''' </summary>
		''' <returns>List of employee div-address data.</returns>
		Public Function LoadEmployeeDivAddressData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeSAddressData) Implements IEmployeeDatabaseAccess.LoadEmployeeDivAddressData

			Dim result As List(Of EmployeeSAddressData) = Nothing

			Dim sql As String

			sql = "SELECT ID,"
			sql &= "RecNr,"
			sql &= "MANr,"
			sql &= "Geschlecht,"
			sql &= "Nachname,"
			sql &= "Vorname,"
			sql &= "Wohnt_Bei,"
			sql &= "Postfach,"
			sql &= "Strasse,"
			sql &= "PLZ,"
			sql &= "Ort,"
			sql &= "Land,"
			sql &= "Add_Bemerkung,"
			sql &= "Add_Res1,"
			sql &= "Add_Res2,"
			sql &= "Add_Res3,"
			sql &= "ForEmployment,"
			sql &= "ForReport,"
			sql &= "ForPayroll,"
			sql &= "ForAGB,"
			sql &= "ForZV,"
			sql &= "ForNLA,"
			sql &= "ForDivers,"

			sql &= "CreatedOn,"
			sql &= "CreatedFrom,"
			sql &= "ChangedOn,"
			sql &= "ChangedFrom,"
			sql &= "ActiveRec"

			sql &= " FROM dbo.MA_SAddress"

			sql &= " Where MANr = @MANr"
			sql &= " Order By ID Desc"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			result = New List(Of EmployeeSAddressData)
			Try

				If (Not reader Is Nothing) Then

					While reader.Read()

						Dim data As New EmployeeSAddressData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.employeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.Gender = SafeGetString(reader, "Geschlecht")
						data.Lastname = SafeGetString(reader, "Nachname")
						data.Firstname = SafeGetString(reader, "Vorname")
						data.StaysAt = SafeGetString(reader, "Wohnt_bei")
						data.PostOfficeBox = SafeGetString(reader, "Postfach")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")
						data.Country = SafeGetString(reader, "Land")

						data.Add_Bemerkung = SafeGetString(reader, "Add_Bemerkung")
						data.Add_Res1 = SafeGetString(reader, "Add_Res1")
						data.Add_Res2 = SafeGetString(reader, "Add_Res2")
						data.Add_Res3 = SafeGetString(reader, "Add_Res3")

						data.ForEmployment = SafeGetBoolean(reader, "ForEmployment", False)
						data.ForReport = SafeGetBoolean(reader, "ForReport", False)
						data.ForPayroll = SafeGetBoolean(reader, "ForPayroll", False)
						data.ForAGB = SafeGetBoolean(reader, "ForAGB", False)
						data.ForZV = SafeGetBoolean(reader, "ForZV", False)
						data.ForNLA = SafeGetBoolean(reader, "ForNLA", False)
						data.ForDivers = SafeGetBoolean(reader, "ForDivers", False)
						data.ActiveRec = SafeGetBoolean(reader, "ActiveRec", False)

						data.Createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.Createdfrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")

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
		''' Add employee div address data.
		''' </summary>
		Public Function AddEmployeeDivAddressData(ByVal employeeNumber As Integer, ByVal data As EmployeeSAddressData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeDivAddressData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Create New Divers Address For Employee]"
			

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(data.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(data.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postfach", ReplaceMissing(data.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Wohnt_bei", ReplaceMissing(data.StaysAt, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Strasse", ReplaceMissing(data.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(data.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(data.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Land", ReplaceMissing(data.Country, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Geschlecht", ReplaceMissing(data.Gender, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Bemerkung", ReplaceMissing(data.Add_Bemerkung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Res1", ReplaceMissing(data.Add_Res1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Res2", ReplaceMissing(data.Add_Res2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Res3", ReplaceMissing(data.Add_Res3, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ForEmployment", ReplaceMissing(data.ForEmployment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForReport", ReplaceMissing(data.ForReport, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForPayroll", ReplaceMissing(data.ForPayroll, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForAGB", ReplaceMissing(data.ForAGB, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForZV", ReplaceMissing(data.ForZV, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForNLA", ReplaceMissing(data.ForNLA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForDivers", ReplaceMissing(data.ForDivers, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ActiveRec", ReplaceMissing(data.ActiveRec, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.Createdfrom, DBNull.Value)))

			Dim resultNewIDParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			resultNewIDParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultNewIDParameter)

			Dim resultRecNrParameter = New SqlClient.SqlParameter("@RecNr", SqlDbType.Int)
			resultRecNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultRecNrParameter)
			
			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not resultNewIDParameter.Value Is Nothing Then
				data.ID = CType(resultNewIDParameter.Value, Integer)
			Else
				success = False
			End If
			If success AndAlso Not resultRecNrParameter.Value Is Nothing Then
				data.RecNr = CType(resultRecNrParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function

		''' <summary>
		''' update employee div address data.
		''' </summary>
		Public Function UpdateEmployeeDivAddressData(ByVal employeeNumber As Integer, ByVal data As EmployeeSAddressData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeDivAddressData

			Dim success As Boolean = True

			Dim sql As String

			sql = "Update MA_SAddress Set "
			sql &= "Geschlecht = @Geschlecht ,"
			sql &= "Nachname = @Nachname ,"
			sql &= "Vorname = @Vorname ,"
			sql &= "Postfach = @Postfach ,"
			sql &= "Wohnt_Bei = @Wohnt_Bei ,"
			sql &= "Strasse = @Strasse ,"
			sql &= "PLZ = @PLZ,"
			sql &= "Ort = @Ort,"
			sql &= "Land = @Land,"
			sql &= "Add_Bemerkung = @Add_Bemerkung,"
			sql &= "Add_Res1 = @Add_Res1,"
			sql &= "Add_Res2 = @Add_Res2,"
			sql &= "Add_Res3 = @Add_Res3,"
			sql &= "ForEmployment = @ForEmployment,"
			sql &= "ForReport = @ForReport,"
			sql &= "ForPayroll = @ForPayroll,"
			sql &= "ForAGB = @ForAGB,"
			sql &= "ForZV = @ForZV,"
			sql &= "ForNLA = @ForNLA,"
			sql &= "ForDivers = @ForDivers,"
			sql &= "ChangedOn = Getdate(),"
			sql &= "ChangedFrom = @ChangedFrom,"
			sql &= "ActiveRec = @ActiveRec"

			sql &= " Where MANr = @MANr"
			sql &= " And ID = @recID"
			

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("Geschlecht", ReplaceMissing(data.Gender, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(data.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(data.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postfach", ReplaceMissing(data.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Wohnt_Bei", ReplaceMissing(data.StaysAt, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Strasse", ReplaceMissing(data.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(data.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(data.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Land", ReplaceMissing(data.Country, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Bemerkung", ReplaceMissing(data.Add_Bemerkung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Res1", ReplaceMissing(data.Add_Res1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Res2", ReplaceMissing(data.Add_Res2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Add_Res3", ReplaceMissing(data.Add_Res3, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ForEmployment", ReplaceMissing(data.ForEmployment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForReport", ReplaceMissing(data.ForReport, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForPayroll", ReplaceMissing(data.ForPayroll, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForAGB", ReplaceMissing(data.ForAGB, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForZV", ReplaceMissing(data.ForZV, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForNLA", ReplaceMissing(data.ForNLA, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ForDivers", ReplaceMissing(data.ForDivers, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ActiveRec", ReplaceMissing(data.ActiveRec, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(data.ID, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' Delete employee NLA data.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function DeleteEmployeeDivAddressData(ByVal recID As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeDivAddressData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete MA_SAddress "
			sql &= "Where ID = @ID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ID", recID))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function


	End Class


End Namespace
