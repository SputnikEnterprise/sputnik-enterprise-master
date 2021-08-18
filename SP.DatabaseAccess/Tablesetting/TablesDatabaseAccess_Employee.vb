
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess



#Region "Employee contact data"

		''' <summary>
		''' Loads contact type1 data.
		''' </summary>
		''' <returns>List of contact type1 data.</returns>
		Function LoadEmployeeContactData() As IEnumerable(Of EmployeeContactData) Implements ITablesDatabaseAccess.LoadEmployeeContactData

			Dim result As List(Of EmployeeContactData) = Nothing

			Dim sql As String

			sql = "SELECT ID, [Description] As Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.TAB_MAKontakt Order By [Description]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeContactData)

					While reader.Read

						Dim EmployeeContactData = New EmployeeContactData()
						EmployeeContactData.recid = SafeGetInteger(reader, "ID", 0)
						EmployeeContactData.bez_value = SafeGetString(reader, "Bez_Value")
						EmployeeContactData.bez_d = SafeGetString(reader, "Bez_d")
						EmployeeContactData.bez_i = SafeGetString(reader, "Bez_I")
						EmployeeContactData.bez_f = SafeGetString(reader, "Bez_F")
						EmployeeContactData.bez_e = SafeGetString(reader, "Bez_e")


						result.Add(EmployeeContactData)

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

		Function AddEmployeeContactData(ByVal data As EmployeeContactData) As Boolean Implements ITablesDatabaseAccess.AddEmployeeContactData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.TAB_MAKontakt ([Description], Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Bez_Value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployeeContactData(ByVal data As EmployeeContactData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployeeContactData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 [Description] From TAB_MAKontakt Where ID = @ID), ''); "

			sql &= "Update dbo.TAB_MAKontakt Set [Description] = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update MAKontakt_Komm Set KontaktHow = @Bez_Value Where KontaktHow = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.bez_value, DBNull.Value)))
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

		Function DeleteEmployeeContactData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployeeContactData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.TAB_MAKontakt "
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


#End Region


#Region "Employee state data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadEmployeeStateData1() As IEnumerable(Of EmployeeStateData) Implements ITablesDatabaseAccess.LoadEmployeeStateData1

			Dim result As List(Of EmployeeStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, [Description] As Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.TAB_MAStat Order By [Description]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeStateData)

					While reader.Read

						Dim data = New EmployeeStateData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "Bez_Value")
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

		Function AddEmployeeStateData1(ByVal data As EmployeeStateData) As Boolean Implements ITablesDatabaseAccess.AddEmployeeStateData1

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.TAB_MAStat ([Description], Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployeeStateData1(ByVal data As EmployeeStateData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployeeStateData1

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Description] From TAB_MAStat Where ID = @ID), ''); "
			sql &= "Update dbo.TAB_MAStat Set [Description] = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update MAKontakt_Komm Set KStat1 = @bez_value Where KStat1 = @Oldbez; "

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

		Function DeleteEmployeeStateData1(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployeeStateData1

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.TAB_MAStat "
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



		''' <summary>
		''' Loads state2 data.
		''' </summary>
		Function LoadEmployeeStateData2() As IEnumerable(Of EmployeeStateData) Implements ITablesDatabaseAccess.LoadEmployeeStateData2

			Dim result As List(Of EmployeeStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung As Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.TAB_MAStat2 Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeStateData)

					While reader.Read

						Dim data = New EmployeeStateData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "Bez_Value")
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

		Function AddEmployeeStateData2(ByVal data As EmployeeStateData) As Boolean Implements ITablesDatabaseAccess.AddEmployeeStateData2

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.TAB_MAStat2 (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployeeStateData2(ByVal data As EmployeeStateData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployeeStateData2

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Bezeichnung] From TAB_MAStat2 Where ID = @ID), ''); "
			sql &= "Update dbo.TAB_MAStat2 Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update MAKontakt_Komm Set KStat2 = @bez_value Where KStat2 = @Oldbez; "

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

		Function DeleteEmployeeStateData2(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployeeStateData2

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.TAB_MAStat2 "
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


#End Region





#Region "Employee civilstate data"

		''' <summary>
		''' Loads civilstate data.
		''' </summary>
		''' <returns>List of civilstate data.</returns>
		Function LoadEmployeeCivilstateData() As IEnumerable(Of Common.DataObjects.CivilStateData) Implements ITablesDatabaseAccess.LoadEmployeeCivilstateData

			Dim result As List(Of Common.DataObjects.CivilStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld, [Description], Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.TAB_Zivilstand Order By [Description]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Common.DataObjects.CivilStateData)

					While reader.Read

						Dim EmployeeContactData = New Common.DataObjects.CivilStateData()
						EmployeeContactData.recid = SafeGetInteger(reader, "ID", 0)
						EmployeeContactData.GetField = SafeGetString(reader, "GetFeld")
						EmployeeContactData.Description = SafeGetString(reader, "Description")
						EmployeeContactData.bez_d = SafeGetString(reader, "Bez_d")
						EmployeeContactData.bez_i = SafeGetString(reader, "Bez_I")
						EmployeeContactData.bez_f = SafeGetString(reader, "Bez_F")
						EmployeeContactData.bez_e = SafeGetString(reader, "Bez_e")


						result.Add(EmployeeContactData)

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

		Function AddEmployeeCivilstateData(ByVal data As Common.DataObjects.CivilStateData) As Boolean Implements ITablesDatabaseAccess.AddEmployeeCivilstateData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.TAB_Zivilstand (GetFeld, [Description], Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@GetFeld "
			sql &= ", @Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@GetFeld", ReplaceMissing(data.GetField, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployeeCivilstateData(ByVal data As Common.DataObjects.CivilStateData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployeeCivilstateData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.TAB_Zivilstand Set GetFeld = @GetFeld, "
			sql &= "[Description] = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@GetFeld", ReplaceMissing(data.GetField, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
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

		Function DeleteEmployeeCivilstateData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployeeCivilstateData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.TAB_Zivilstand "
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


#End Region



#Region "Employee language data"

		Function LoadJobLanguageData() As IEnumerable(Of JobLanguageData) Implements ITablesDatabaseAccess.LoadJobLanguageData

			Dim result As List(Of JobLanguageData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld As bez_value, Bez_d, Bez_i, Bez_f, Bez_E FROM Tab_Bew_Sprache Order By GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of JobLanguageData)

					While reader.Read

						Dim data = New JobLanguageData()
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

		Function AddJobLanguageData(ByVal data As JobLanguageData) As Boolean Implements ITablesDatabaseAccess.AddJobLanguageData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Bew_Sprache (GetFeld, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateJobLanguageData(ByVal data As JobLanguageData) As Boolean Implements ITablesDatabaseAccess.UpdateJobLanguageData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 GetFeld From Tab_Bew_Sprache Where ID = @ID), ''); "

			sql = "Update dbo.Tab_Bew_Sprache Set GetFeld = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MA_MSprachen Set [Bezeichnung] = @Bez_Value Where Bezeichnung = @OldBez; "
			sql &= "Update dbo.MA_SSprachen Set [Bezeichnung] = @Bez_Value Where Bezeichnung = @OldBez; "


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

		Function DeleteJobLanguageData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteJobLanguageData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Bew_Sprache "
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


#End Region



#Region "Employee contact reserve data"

		Function LoadContactReserveData(ByVal contactReserveType As ContactReserveType) As IEnumerable(Of ContactReserveData) Implements ITablesDatabaseAccess.LoadContactReserveData

			Dim result As List(Of ContactReserveData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_KontaktRes{0} Order By ID", CType(contactReserveType, Integer))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ContactReserveData)

					While reader.Read

						Dim data = New ContactReserveData()
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

		Function AddContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As ContactReserveData) As Boolean Implements ITablesDatabaseAccess.AddContactReserveData

			Dim success = True

			Dim sql As String

			sql = String.Format("Insert Into dbo.Tab_KontaktRes{0} (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values (", CType(contactReserveType, Integer))
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

		Function UpdateContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As ContactReserveData) As Boolean Implements ITablesDatabaseAccess.UpdateContactReserveData

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Bezeichnung] From Tab_KontaktRes{0} Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KontaktRes{0} Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update MAKontakt_Komm Set Res{0} = @bez_value Where Res{0} = @Oldbez; "

			sql = String.Format(sql, CType(contactReserveType, Integer))

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

		Function DeleteContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteContactReserveData

			Dim success = True

			Dim sql As String

			sql = String.Format("Delete dbo.Tab_KontaktRes{0} ", CType(contactReserveType, Integer))
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


#End Region




#Region "Employee Deadline and Pensum: Kündigungen, Arbeitspensum"

		Function LoadDeadLineData() As IEnumerable(Of DeadlineData) Implements ITablesDatabaseAccess.LoadDeadLineData

			Dim result As List(Of DeadlineData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_Kundfristen Order By GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of DeadlineData)

					While reader.Read

						Dim data = New DeadlineData()
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

		Function AddDeadLineData(ByVal data As DeadlineData) As Boolean Implements ITablesDatabaseAccess.AddDeadLineData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Kundfristen (GetFeld, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateDeadLineData(ByVal data As DeadlineData) As Boolean Implements ITablesDatabaseAccess.UpdateDeadLineData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Kundfristen Set GetFeld = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID"

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

		Function DeleteDeadLineData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteDeadLineData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Kundfristen "
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



		Function LoadWorkPensumData() As IEnumerable(Of WorkPensumData) Implements ITablesDatabaseAccess.LoadWorkPensumData

			Dim result As List(Of WorkPensumData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld AS bez_value FROM dbo.Tab_ArbPensum Order By GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of WorkPensumData)

					While reader.Read

						Dim data = New WorkPensumData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "bez_value")

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

		Function AddWorkPensumData(ByVal data As WorkPensumData) As Boolean Implements ITablesDatabaseAccess.AddWorkPensumData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_ArbPensum (GetFeld) Values ("
			sql &= "@bez_value)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateWorkPensumData(ByVal data As WorkPensumData) As Boolean Implements ITablesDatabaseAccess.UpdateWorkPensumData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_ArbPensum Set GetFeld = @bez_value "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteWorkPensumData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteWorkPensumData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_ArbPensum "
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


#End Region




#Region "Employee EmployementType: Anstellungsart"


		Function LoadEmployementTypeData() As IEnumerable(Of EmployeeEmployementTypeData) Implements ITablesDatabaseAccess.LoadEmployementTypeData

			Dim result As List(Of EmployeeEmployementTypeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_MAAnstellung Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeEmployementTypeData)

					While reader.Read

						Dim data = New EmployeeEmployementTypeData()
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

		Function AddEmployementTypeData(ByVal data As EmployeeEmployementTypeData) As Boolean Implements ITablesDatabaseAccess.AddEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_MAAnstellung (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateEmployementTypeData(ByVal data As EmployeeEmployementTypeData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Bezeichnung] From Tab_MAAnstellung Where ID = @ID), ''); "
			sql &= "Update dbo.Tab_MAAnstellung Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update MA_Anstellung Set Bezeichnung = @bez_value Where Bezeichnung = @Oldbez; "

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

		Function DeleteEmployementTypeData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_MAAnstellung "
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


#End Region


#Region "employee document category data"

		''' <summary>
		''' Loads document category data.
		''' </summary>
		Function LoadEmployeeDocumentCategoryData() As IEnumerable(Of Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) Implements ITablesDatabaseAccess.LoadEmployeeDocumentCategoryData

			Dim result As List(Of Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Categorie_Nr, Bez_D, Bez_F, Bez_I, Bez_E FROM Tab_MADocCategories ORDER BY ID ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData)

					While reader.Read

						Dim categoryDataData As New Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData
						categoryDataData.ID = SafeGetInteger(reader, "ID", 0)
						categoryDataData.CategoryNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
						categoryDataData.DescriptionGerman = SafeGetString(reader, "Bez_D")
						categoryDataData.DescriptionItalian = SafeGetString(reader, "Bez_I")
						categoryDataData.DescriptionFrench = SafeGetString(reader, "Bez_F")
						categoryDataData.DescriptionEnglish = SafeGetString(reader, "Bez_E")

						result.Add(categoryDataData)

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

		Function AddEmployeeDocumentCategoryData(ByVal data As Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) As Boolean Implements ITablesDatabaseAccess.AddEmployeeDocumentCategoryData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_MADocCategories (Categorie_Nr, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Categorie_Nr"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Categorie_Nr", ReplaceMissing(data.CategoryNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.DescriptionGerman, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.DescriptionItalian, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.DescriptionFrench, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.DescriptionEnglish, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployeeDocumentCategoryData(ByVal data As Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployeeDocumentCategoryData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez int; "
			sql &= "Set @OldBez = IsNull((Select Top 1 Categorie_Nr From Tab_MADocCategories Where ID = @ID), 0); "

			sql &= "Update dbo.Tab_MADocCategories Set Categorie_Nr = @Categorie_Nr, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "

			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MA_LLDoc Set [Categorie_Nr] = @Categorie_Nr Where Categorie_Nr = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Categorie_Nr", ReplaceMissing(data.CategoryNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.DescriptionGerman, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.DescriptionItalian, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.DescriptionFrench, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.DescriptionEnglish, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteEmployeeDocumentCategoryData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployeeDocumentCategoryData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_MADocCategories "
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


#End Region


#Region "Employee Interview state"


		Function LoadInterviewStateData() As IEnumerable(Of EmployeeInteriviewStateData) Implements ITablesDatabaseAccess.LoadInterviewStateData

			Dim result As List(Of EmployeeInteriviewStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, Bez_D, Bez_I, Bez_F, Bez_E FROM dbo.Tab_JobTerminStatus Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeInteriviewStateData)

					While reader.Read

						Dim data = New EmployeeInteriviewStateData()
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

		Function AddInterviewStateData(ByVal data As EmployeeInteriviewStateData) As Boolean Implements ITablesDatabaseAccess.AddInterviewStateData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_JobTerminStatus (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E"
			sql &= ")"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateInterviewStateData(ByVal data As EmployeeInteriviewStateData) As Boolean Implements ITablesDatabaseAccess.UpdateInterviewStateData

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Bezeichnung] From Tab_JobTerminStatus Where ID = @ID), ''); "
			sql &= "Update dbo.Tab_JobTerminStatus Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "
			
			sql &= "Update MA_JobTermin Set JobTerminStatus = @bez_value Where JobTerminStatus = @Oldbez; "

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

		Function DeleteInterviewStateData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteInterviewStateData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_JobTerminStatus "
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


#End Region


	End Class



End Namespace
