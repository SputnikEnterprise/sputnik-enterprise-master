
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
		''' <returns>List of employee NLA data.</returns>
		Public Function LoadEmployeeNLAData(ByVal employeeNumber As Integer) As EmployeeNLAData Implements IEmployeeDatabaseAccess.LoadEmployeeNLAData

			Dim result As EmployeeNLAData = Nothing

			Dim sql As String = String.Empty

			sql &= "Select Top 1 ID, MANr, NLA_LoAusweis, NLA_Befoerderung, NLA_Kantine, "
			sql &= "NLA_2_3, NLA_3_0, NLA_4_0, NLA_7_0, "
			sql &= "NLA_13_1_2, NLA_13_2_3, "
			sql &= "NLA_Nebenleistung_1, NLA_Nebenleistung_2, "
			sql &= "NLA_Bemerkung_1, NLA_Bemerkung_2, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom "
			sql &= "From MA_LOAusweis Where MANr = @MANr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New EmployeeNLAData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.recID = SafeGetInteger(reader, "ID", 0)
					result.employeeNumber = SafeGetInteger(reader, "MANr", 0)
					result.NLA_LoAusweis = SafeGetBoolean(reader, "NLA_LoAusweis", False)
					result.NLA_Befoerderung = SafeGetBoolean(reader, "NLA_Befoerderung", False)
					result.NLA_Kantine = SafeGetBoolean(reader, "NLA_Kantine", False)

					result.NLA_2_3 = SafeGetString(reader, "NLA_2_3")
					result.NLA_3_0 = SafeGetString(reader, "NLA_3_0")
					result.NLA_4_0 = SafeGetString(reader, "NLA_4_0")
					result.NLA_7_0 = SafeGetString(reader, "NLA_7_0")
					result.NLA_13_1_2 = SafeGetString(reader, "NLA_13_1_2")
					result.NLA_13_2_3 = SafeGetString(reader, "NLA_13_2_3")
					result.NLA_Nebenleistung_1 = SafeGetString(reader, "NLA_Nebenleistung_1")
					result.NLA_Nebenleistung_2 = SafeGetString(reader, "NLA_Nebenleistung_2")
					result.NLA_Bemerkung_1 = SafeGetString(reader, "NLA_Bemerkung_1")
					result.NLA_Bemerkung_2 = SafeGetString(reader, "NLA_Bemerkung_2")

					result.createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.createdfrom = SafeGetString(reader, "CreatedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")


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
		''' <returns>List of employee NLA Adress data.</returns>
		Public Function SaveEmployeeNLAData(ByVal data As EmployeeNLAData, ByVal employeeNumber As Integer) As Boolean Implements IEmployeeDatabaseAccess.SaveEmployeeNLAData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "IF NOT EXISTS(SELECT MANr From MA_LOAusweis Where MANr = @MANr)  "
			sql &= "Begin "
			sql &= "Insert Into MA_LOAusweis (MANr, CreatedOn, CreatedFrom) Values (@MANr, GetDate(), @ChangedFrom) "
			sql &= "End "

			sql &= "Update MA_LOAusweis Set "
			sql &= "NLA_LoAusweis = @NLA_LoAusweis, "
			sql &= "NLA_Befoerderung = @NLA_Befoerderung, "
			sql &= "NLA_Kantine = @NLA_Kantine, "
			sql &= "NLA_2_3 = @NLA_2_3, "
			sql &= "NLA_3_0 = @NLA_3_0, "
			sql &= "NLA_4_0 = @NLA_4_0, "
			sql &= "NLA_7_0 = @NLA_7_0, "
			sql &= "NLA_13_1_2 = @NLA_13_1_2, "
			sql &= "NLA_13_2_3 = @NLA_13_2_3, "
			sql &= "NLA_Nebenleistung_1 = @NLA_Nebenleistung_1, "
			sql &= "NLA_Nebenleistung_2 = @NLA_Nebenleistung_2, "
			sql &= "NLA_Bemerkung_1 = @NLA_Bemerkung_1, "
			sql &= "NLA_Bemerkung_2 = @NLA_Bemerkung_2, "
			sql &= "ChangedOn = Getdate(), "
			sql &= "ChangedFrom = @ChangedFrom "
			sql &= "Where MANr = @MANr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			listOfParams.Add(New SqlClient.SqlParameter("NLA_LoAusweis", ReplaceMissing(data.NLA_LoAusweis, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_Befoerderung", ReplaceMissing(data.NLA_Befoerderung, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_Kantine", ReplaceMissing(data.NLA_Kantine, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_2_3", ReplaceMissing(data.NLA_2_3, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("NLA_3_0", ReplaceMissing(data.NLA_3_0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_4_0", ReplaceMissing(data.NLA_4_0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_7_0", ReplaceMissing(data.NLA_7_0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_13_1_2", ReplaceMissing(data.NLA_13_1_2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_13_2_3", ReplaceMissing(data.NLA_13_2_3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_Nebenleistung_1", ReplaceMissing(data.NLA_Nebenleistung_1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_Nebenleistung_2", ReplaceMissing(data.NLA_Nebenleistung_2, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("NLA_Bemerkung_1", ReplaceMissing(data.NLA_Bemerkung_1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NLA_Bemerkung_2", ReplaceMissing(data.NLA_Bemerkung_2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Delete employee NLA data.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function DeleteEmployeeNLAData(ByVal employeeNumber As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeNLAData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete MA_LOAusweis "
			sql &= "Where MANr = @MANr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function


	End Class


End Namespace
