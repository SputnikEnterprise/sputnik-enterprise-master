
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting

	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess



#Region "Propose state data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		''' <returns>List of state data.</returns>
		Function LoadProposeStateData() As IEnumerable(Of ProposeStateData) Implements ITablesDatabaseAccess.LoadProposeStateData

			Dim result As List(Of ProposeStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_P_State Where Bezeichnung Is Not Null Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ProposeStateData)

					While reader.Read

						Dim CustomerContactData = New ProposeStateData()
						CustomerContactData.recid = SafeGetInteger(reader, "ID", 0)
						CustomerContactData.bez_value = SafeGetString(reader, "Bez_Value")
						CustomerContactData.bez_d = SafeGetString(reader, "Bez_d")
						CustomerContactData.bez_i = SafeGetString(reader, "Bez_I")
						CustomerContactData.bez_f = SafeGetString(reader, "Bez_F")
						CustomerContactData.bez_e = SafeGetString(reader, "Bez_e")


						result.Add(CustomerContactData)

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

		Function AddProposeStateData(ByVal data As ProposeStateData) As Boolean Implements ITablesDatabaseAccess.AddProposeStateData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_P_State (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateProposeStateData(ByVal data As ProposeStateData) As Boolean Implements ITablesDatabaseAccess.UpdateProposeStateData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_P_State Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_P_State Set Bezeichnung = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Propose Set [P_State] = @Bez_Value Where P_State = @OldBez; "

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

		Function DeleteProposeStateData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteProposeStateData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_P_State "
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




#Region "Propose employementtype data"

		''' <summary>
		''' Loads employementtype data.
		''' </summary>
		''' <returns>List of employementtype data.</returns>
		Function LoadProposeEmployementTypeData() As IEnumerable(Of ProposeEmployementTypeData) Implements ITablesDatabaseAccess.LoadProposeEmployementTypeData

			Dim result As List(Of ProposeEmployementTypeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_P_Anstellung Where Bezeichnung Is Not Null Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ProposeEmployementTypeData)

					While reader.Read

						Dim CustomerContactData = New ProposeEmployementTypeData()
						CustomerContactData.recid = SafeGetInteger(reader, "ID", 0)
						CustomerContactData.bez_value = SafeGetString(reader, "Bez_Value")
						CustomerContactData.bez_d = SafeGetString(reader, "Bez_d")
						CustomerContactData.bez_i = SafeGetString(reader, "Bez_I")
						CustomerContactData.bez_f = SafeGetString(reader, "Bez_F")
						CustomerContactData.bez_e = SafeGetString(reader, "Bez_e")


						result.Add(CustomerContactData)

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

		Function AddProposeEmployementTypeData(ByVal data As ProposeEmployementTypeData) As Boolean Implements ITablesDatabaseAccess.AddProposeEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_P_Anstellung (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateProposeEmployementTypeData(ByVal data As ProposeEmployementTypeData) As Boolean Implements ITablesDatabaseAccess.UpdateProposeEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_P_Anstellung Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_P_Anstellung Set Bezeichnung = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Propose Set [P_Anstellung] = @Bez_Value Where P_Anstellung = @OldBez; "

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

		Function DeleteProposeEmployementTypeData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteProposeEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_P_Anstellung "
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




#Region "Propose art data"

		''' <summary>
		''' Loads art data.
		''' </summary>
		''' <returns>List of art data.</returns>
		Function LoadProposeArtData() As IEnumerable(Of ProposeArtData) Implements ITablesDatabaseAccess.LoadProposeArtData

			Dim result As List(Of ProposeArtData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_P_Art Where Bezeichnung Is Not Null Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ProposeArtData)

					While reader.Read

						Dim CustomerContactData = New ProposeArtData()
						CustomerContactData.recid = SafeGetInteger(reader, "ID", 0)
						CustomerContactData.bez_value = SafeGetString(reader, "Bez_Value")
						CustomerContactData.bez_d = SafeGetString(reader, "Bez_d")
						CustomerContactData.bez_i = SafeGetString(reader, "Bez_I")
						CustomerContactData.bez_f = SafeGetString(reader, "Bez_F")
						CustomerContactData.bez_e = SafeGetString(reader, "Bez_e")


						result.Add(CustomerContactData)

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

		Function AddProposeArtData(ByVal data As ProposeArtData) As Boolean Implements ITablesDatabaseAccess.AddProposeArtData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_P_Art (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateProposeArtData(ByVal data As ProposeArtData) As Boolean Implements ITablesDatabaseAccess.UpdateProposeArtData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_P_Art Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_P_Art Set Bezeichnung = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Propose Set [P_Art] = @Bez_Value Where P_Art = @OldBez; "

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

		Function DeleteProposeArtData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteProposeArtData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_P_Art "
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
