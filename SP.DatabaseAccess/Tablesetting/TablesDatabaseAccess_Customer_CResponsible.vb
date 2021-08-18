
Imports SP.DatabaseAccess.TableSetting.DataObjects


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess




#Region "responsible person contact data"

		''' <summary>
		''' Loads contact type1 data.
		''' </summary>
		''' <returns>List of contact type1 data.</returns>
		Function LoadResponsiblepersonContactData() As IEnumerable(Of Customer.DataObjects.ResponsiblePersonContactInfo) Implements ITablesDatabaseAccess.LoadResponsiblepersonContactData

			Dim result As List(Of Customer.DataObjects.ResponsiblePersonContactInfo) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_ZHDKontakt Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.ResponsiblePersonContactInfo)

					While reader.Read

						Dim CustomerContactData = New Customer.DataObjects.ResponsiblePersonContactInfo()
						CustomerContactData.ID = SafeGetInteger(reader, "ID", 0)
						CustomerContactData.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersonContactData(ByVal data As Customer.DataObjects.ResponsiblePersonContactInfo) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersonContactData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_ZHDKontakt (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersonContactData(ByVal data As Customer.DataObjects.ResponsiblePersonContactInfo) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersonContactData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_ZHDKontakt Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_ZHDKontakt Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_Zustaendig Set KDZHowKontakt = @Description Where KDZHowKontakt = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersonContactData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersonContactData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_ZHDKontakt "
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


#Region "responsible person state data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadResponsiblepersonStateData1() As IEnumerable(Of Customer.DataObjects.ResponsiblePersonStateData) Implements ITablesDatabaseAccess.LoadResponsiblepersonStateData1

			Dim result As List(Of Customer.DataObjects.ResponsiblePersonStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_ZHDState1 Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.ResponsiblePersonStateData)

					While reader.Read

						Dim data = New Customer.DataObjects.ResponsiblePersonStateData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersonStateData1(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersonStateData1

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_ZHDState1 (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersonStateData1(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersonStateData1

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_ZHDState1 Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_ZHDState1 Set "
			sql &= "Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_Zustaendig Set KDZState1 = @Description Where KDZState1 = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersonStateData1(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersonStateData1

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_ZHDState1 "
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
		Function LoadResponsiblepersonStateData2() As IEnumerable(Of Customer.DataObjects.ResponsiblePersonStateData) Implements ITablesDatabaseAccess.LoadResponsiblepersonStateData2

			Dim result As List(Of Customer.DataObjects.ResponsiblePersonStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_ZHDState2 Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.ResponsiblePersonStateData)

					While reader.Read

						Dim data = New Customer.DataObjects.ResponsiblePersonStateData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersonStateData2(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersonStateData2

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_ZHDState2 (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersonStateData2(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersonStateData2

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_ZHDState2 Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_ZHDState2 Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_Zustaendig Set KDZState2 = @Description Where KDZState2 = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersonStateData2(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersonStateData2

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_ZHDState2 "
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



#Region "responsible person department"

		Function LoadResponsiblepersonDepartment() As IEnumerable(Of Customer.DataObjects.DepartmentData) Implements ITablesDatabaseAccess.LoadResponsiblepersonDepartment

			Dim result As List(Of Customer.DataObjects.DepartmentData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_Abteilung Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.DepartmentData)

					While reader.Read

						Dim data = New Customer.DataObjects.DepartmentData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersonDepartment(ByVal data As Customer.DataObjects.DepartmentData) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersonDepartment

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Abteilung (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersonDepartment(ByVal data As Customer.DataObjects.DepartmentData) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersonDepartment

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_Abteilung Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_Abteilung Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_Zustaendig Set Abteilung = @Description Where Abteilung = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersonDepartment(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersonDepartment

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Abteilung "
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



#Region "responsible person position"

		Function LoadResponsiblepersonPosition() As IEnumerable(Of Customer.DataObjects.PositionData) Implements ITablesDatabaseAccess.LoadResponsiblepersonPosition

			Dim result As List(Of Customer.DataObjects.PositionData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_Position Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.PositionData)

					While reader.Read

						Dim data = New Customer.DataObjects.PositionData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersonPosition(ByVal data As Customer.DataObjects.PositionData) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersonPosition

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Position (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersonPosition(ByVal data As Customer.DataObjects.PositionData) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersonPosition

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_Position Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_Position Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_Zustaendig Set Position = @Description Where Position = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersonPosition(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersonPosition

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Position "
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


#Region "responsible person communication"

		Function LoadResponsiblepersonCommunication() As IEnumerable(Of Customer.DataObjects.CustomerCommunicationData) Implements ITablesDatabaseAccess.LoadResponsiblepersoncommunication

			Dim result As List(Of Customer.DataObjects.CustomerCommunicationData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_KDKommunikation Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.CustomerCommunicationData)

					While reader.Read

						Dim data = New Customer.DataObjects.CustomerCommunicationData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersoncommunication(ByVal data As Customer.DataObjects.CustomerCommunicationData) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersoncommunication

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDKommunikation (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersoncommunication(ByVal data As Customer.DataObjects.CustomerCommunicationData) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersoncommunication

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_KDKommunikation Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KDKommunikation Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_ZKomm Set Bezeichnung = @Description Where Bezeichnung = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersoncommunication(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersoncommunication

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDKommunikation "
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



#Region "responsible person communication  type"

		Function LoadResponsiblepersonCommunicationType() As IEnumerable(Of Customer.DataObjects.CustomerCommunicationTypeData) Implements ITablesDatabaseAccess.LoadResponsiblepersoncommunicationType

			Dim result As List(Of Customer.DataObjects.CustomerCommunicationTypeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_KDKommArt Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.CustomerCommunicationTypeData)

					While reader.Read

						Dim data = New Customer.DataObjects.CustomerCommunicationTypeData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddResponsiblepersonCommunicationType(ByVal data As Customer.DataObjects.CustomerCommunicationTypeData) As Boolean Implements ITablesDatabaseAccess.AddResponsiblepersonCommunicationType

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDKommArt (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateResponsiblepersonCommunicationType(ByVal data As Customer.DataObjects.CustomerCommunicationTypeData) As Boolean Implements ITablesDatabaseAccess.UpdateResponsiblepersonCommunicationType

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_KDKommArt Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KDKommArt Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_ZKontaktArt Set Bezeichnung = @Description Where Bezeichnung = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteResponsiblepersonCommunicationType(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteResponsiblepersonCommunicationType

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDKommArt "
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



#Region "Customer Responsible contact reserve data"

		Function LoadCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType) As IEnumerable(Of Customer.DataObjects.ResponsiblePersonReserveData) Implements ITablesDatabaseAccess.LoadCResponsibleContactReserveData

			Dim result As List(Of Customer.DataObjects.ResponsiblePersonReserveData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, Bezeichnung, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_ZHDRes{0} Order By Bezeichnung", CType(contactReserveType, Integer))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.ResponsiblePersonReserveData)

					While reader.Read

						Dim data = New Customer.DataObjects.ResponsiblePersonReserveData()
						data.id = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
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

		Function AddCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.ResponsiblePersonReserveData) As Boolean Implements ITablesDatabaseAccess.AddCResponsibleContactReserveData

			Dim success = True

			Dim sql As String

			sql = String.Format("Insert Into dbo.Tab_ZHDRes{0} (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values (", CType(contactReserveType, Integer))
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.ResponsiblePersonReserveData) As Boolean Implements ITablesDatabaseAccess.UpdateCResponsibleContactReserveData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_ZHDRes{0} Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_ZHDRes{0} Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update KD_ZRes{0} Set Bezeichnung = @Description Where Bezeichnung = @OldBez; "
			sql = String.Format(sql, CType(contactReserveType, Integer))

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
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

		Function DeleteCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCResponsibleContactReserveData

			Dim success = True

			Dim sql As String

			sql = String.Format("Delete dbo.Tab_ZHDRes{0} ", CType(contactReserveType, Integer))
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
