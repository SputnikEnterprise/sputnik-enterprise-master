

Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


		Function LoadDrivingLicenceData() As IEnumerable(Of DrivingLicenceData) Implements ITablesDatabaseAccess.LoadDrivingLicenceData

			Dim result As List(Of DrivingLicenceData) = Nothing

			Dim sql As String

			sql = "SELECT ID, RecValue AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.tbl_base_F_Schein Order By RecValue"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of DrivingLicenceData)

					While reader.Read

						Dim data = New DrivingLicenceData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "bez_value")
						data.bez_d = SafeGetString(reader, "Bez_d")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_e")

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

		Function AddDrivingLicenceData(ByVal data As DrivingLicenceData) As Boolean Implements ITablesDatabaseAccess.AddDrivingLicenceData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.tbl_base_F_Schein (RecValue, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateDrivingLicenceData(ByVal data As DrivingLicenceData) As Boolean Implements ITablesDatabaseAccess.UpdateDrivingLicenceData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 RecValue From tbl_base_F_Schein Where ID = @ID), ''); "

			sql &= "Update dbo.tbl_base_F_Schein Set RecValue = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MASonstiges Set F_Schein1 = @bez_value Where F_Schein1 = @OldBez; "
			sql &= "Update dbo.MASonstiges Set F_Schein2 = @bez_value Where F_Schein1 = @OldBez; "
			sql &= "Update dbo.MASonstiges Set F_Schein3 = @bez_value Where F_Schein1 = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				Dim value As String = ReplaceMissing(data.bez_value, DBNull.Value)
				If Not value Is Nothing Then
					If value.Length > 3 Then value = value.Substring(0, 3)
				End If
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_value", value))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteDrivingLicenceData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteDrivingLicenceData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.tbl_base_F_Schein "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function



		Function LoadVehicleData() As IEnumerable(Of VehicleData) Implements ITablesDatabaseAccess.LoadVehicleData

			Dim result As List(Of VehicleData) = Nothing

			Dim sql As String

			sql = "SELECT ID, RecValue AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.tbl_base_Fahrzeug Order By RecValue"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of VehicleData)

					While reader.Read

						Dim data = New VehicleData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "bez_value")
						data.bez_d = SafeGetString(reader, "Bez_d")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_e")

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

		Function AddVehicleData(ByVal data As VehicleData) As Boolean Implements ITablesDatabaseAccess.AddVehicleData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.tbl_base_Fahrzeug (RecValue, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateVehicleData(ByVal data As VehicleData) As Boolean Implements ITablesDatabaseAccess.UpdateVehicleData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 RecValue From tbl_base_Fahrzeug Where ID = @ID), ''); "

			sql &= "Update dbo.tbl_base_Fahrzeug Set RecValue = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MASonstiges Set Fahrzeug = @bez_value Where Fahrzeug = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteVehicleData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteVehicleData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.tbl_base_Fahrzeug "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function




		Function LoadCarReserveData() As IEnumerable(Of CarReserveData) Implements ITablesDatabaseAccess.LoadCarReserveData

			Dim result As List(Of CarReserveData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_AutoRes Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CarReserveData)

					While reader.Read

						Dim data = New CarReserveData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "bez_value")
						data.bez_d = SafeGetString(reader, "Bez_d")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_e")

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

		Function AddCarReserveData(ByVal data As CarReserveData) As Boolean Implements ITablesDatabaseAccess.AddCarReserveData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_AutoRes (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCarReserveData(ByVal data As CarReserveData) As Boolean Implements ITablesDatabaseAccess.UpdateCarReserveData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_AutoRes Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_AutoRes Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MASonstiges Set AutoReserv = @bez_value Where AutoReserv = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCarReserveData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCarReserveData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_AutoRes "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(recid, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function



	End Class


End Namespace
