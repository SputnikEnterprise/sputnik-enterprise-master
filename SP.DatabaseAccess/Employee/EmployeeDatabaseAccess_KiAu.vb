
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects

Namespace Employee


	Partial Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess


		''' <summary>
		''' Loads employee Ki-Au data.
		''' </summary>
		''' <returns>List of employee Ki-Au data.</returns>
		Public Function LoadEmployeeKiAuData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeChldData) Implements IEmployeeDatabaseAccess.LoadEmployeeKiAuData

			Dim result As List(Of EmployeeChldData) = Nothing

			Dim sql As String = String.Empty

			sql &= "Select ID, RecNr, MANr, Nachname, Vorname, Gebdat, Geschlecht, LANr, ZulageArt, Bemerkung, VonMonth, VonYear, BisMonth, BisYear, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom "
			sql &= "From MA_KIAddress Where MANr = @MANr ORDER BY GebDat, Vorname"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

        If (Not reader Is Nothing) Then

					result = New List(Of EmployeeChldData)

					While reader.Read()
						Dim data As New EmployeeChldData

						data.recID = SafeGetInteger(reader, "ID", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.employeeNumber = SafeGetInteger(reader, "MANr", 0)

						data.childFirstname = SafeGetString(reader, "Vorname")
						data.childLastname = SafeGetString(reader, "Nachname")

						data.childGebDat = SafeGetDateTime(reader, "GebDat", Nothing)
						data.childsex = SafeGetString(reader, "Geschlecht")

						data.laNumber = SafeGetDecimal(reader, "LANr", 0)
						data.ZulageArt = SafeGetString(reader, "ZulageArt")
						data.vonMonth = SafeGetInteger(reader, "VonMonth", 0)
						data.vonYear = SafeGetInteger(reader, "VonYear", 0)
						data.bisMonth = SafeGetInteger(reader, "BisMonth", 0)
						data.bisYear = SafeGetInteger(reader, "BisYear", 0)

						data.bemerkung = SafeGetString(reader, "Bemerkung")

						data.createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.createdfrom = SafeGetString(reader, "CreatedFrom")
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
		''' Loads assigned employee Ki-Au data.
		''' </summary>
		''' <returns>employee Ki-Au data.</returns>
		Public Function LoadAssignedEmployeeKiAuData(ByVal recID As Integer) As EmployeeChldData Implements IEmployeeDatabaseAccess.LoadAssignedEmployeeKiAuData

			Dim result As EmployeeChldData = Nothing

			Dim sql As String = String.Empty

			sql &= "Select ID, RecNr, MANr, Nachname, Vorname, Gebdat, Geschlecht, LANr, ZulageArt, Bemerkung, VonMonth, VonYear, BisMonth, BisYear, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom "
			sql &= "From MA_KIAddress Where ID = @RecID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RecID", recID))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				result = New EmployeeChldData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.recID = SafeGetInteger(reader, "ID", 0)
					result.RecNr = SafeGetInteger(reader, "RecNr", 0)

					result.childFirstname = SafeGetString(reader, "Vorname")
					result.childLastname = SafeGetString(reader, "Nachname")
					result.childGebDat = SafeGetDateTime(reader, "GebDat", Nothing)
					result.childsex = SafeGetString(reader, "Geschlecht")

					result.laNumber = SafeGetDecimal(reader, "LANr", 0)
					result.ZulageArt = SafeGetString(reader, "ZulageArt")
					result.vonMonth = SafeGetInteger(reader, "VonMonth", 0)
					result.vonYear = SafeGetInteger(reader, "VonYear", 0)
					result.bisMonth = SafeGetInteger(reader, "BisMonth", 0)
					result.bisYear = SafeGetInteger(reader, "BisYear", 0)

					result.bemerkung = SafeGetString(reader, "Bemerkung")

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
		''' saves employee Ki-Au data.
		''' </summary>
		''' <returns>true if success.</returns>
		Public Function SaveEmployeeKiAuData(ByVal data As EmployeeChldData) As Boolean Implements IEmployeeDatabaseAccess.SaveEmployeeKiAuData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Update MA_KIAddress Set "
			sql &= "RecNr = IsNull((Select Top 1 RecNr From MA_KIAddress Where MANr = @MANr Order By RecNr), 0) + 1, "
			sql &= "MANr = @MANr, "
			sql &= "Vorname = @Vorname, "
			sql &= "Nachname = @Nachname, "
			sql &= "GebDat = @GebDat, "
			sql &= "Geschlecht = @Geschlecht, "
			sql &= "LANr = @LANr, "
			sql &= "ZulageArt = @ZulageArt, "
			sql &= "VonMonth = @VonMonth, "
			sql &= "VonYear = @VonYear, "
			sql &= "BisMonth = @BisMonth, "
			sql &= "BisYear = @BisYear, "
			sql &= "Bemerkung = @Bemerkung, "

			sql &= "ChangedOn = Getdate(), "
			sql &= "ChangedFrom = @ChangedFrom "
			sql &= "Where ID = @recID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recID", data.recID))

			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(data.employeeNumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("Vorname", data.childFirstname))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", data.childLastname))
			listOfParams.Add(New SqlClient.SqlParameter("GebDat", data.childGebDat))
			listOfParams.Add(New SqlClient.SqlParameter("Geschlecht", data.childsex))
			listOfParams.Add(New SqlClient.SqlParameter("LANr", ReplaceMissing(data.laNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ZulageArt", data.ZulageArt))

			listOfParams.Add(New SqlClient.SqlParameter("VonMonth", ReplaceMissing(data.vonMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("VonYear", ReplaceMissing(data.vonYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("BisMonth", ReplaceMissing(data.bisMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("BisYear", ReplaceMissing(data.bisYear, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("Bemerkung", data.bemerkung))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", data.ChangedFrom))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Add new employee Ki-Au data.
		''' </summary>
		''' <returns>true if success.</returns>
		Public Function AddEmployeeKiAuData(ByVal data As EmployeeChldData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeKiAuData

			Dim success As Boolean = True

			Dim sql As String = String.Empty
			sql = "[Create New MA_KIAuZulage]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(data.employeeNumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(data.childFirstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(data.childLastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("GebDat", ReplaceMissing(data.childGebDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Geschlecht", ReplaceMissing(data.childsex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LANr", ReplaceMissing(data.laNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ZulageArt", ReplaceMissing(data.ZulageArt, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("VonMonth", ReplaceMissing(data.vonMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("VonYear", ReplaceMissing(data.vonYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("BisMonth", ReplaceMissing(data.bisMonth, Now.Month)))
			listOfParams.Add(New SqlClient.SqlParameter("BisYear", ReplaceMissing(data.bisYear, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("Bemerkung", ReplaceMissing(data.bemerkung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", data.createdfrom))


			Dim newIdParameter = New SqlClient.SqlParameter("@NewRecId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso
					Not recNrParameter.Value Is Nothing Then
				data.recID = CType(newIdParameter.Value, Integer)
				data.RecNr = CType(recNrParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Delete employee Ki-Au data.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function DeleteEmployeeKiAuData(ByVal recID As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeKiAuData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete MA_KIAddress "
			sql &= "Where ID = @recID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recID", recID))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function


	End Class


End Namespace


