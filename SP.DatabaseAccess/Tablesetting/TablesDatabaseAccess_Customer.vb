
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess




#Region "Customer Property data"

		''' <summary>
		''' Loads Property data.
		''' </summary>
		''' <returns>List of Property data.</returns>
		Function LoadCustomerPropertyData() As IEnumerable(Of CustomerPropertyData) Implements ITablesDatabaseAccess.LoadCustomerPropertyData

			Dim result As List(Of CustomerPropertyData) = Nothing

			Dim sql As String

			sql = "SELECT ID, convert(money, Bez_Value) Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_KDFProperty Where Bez_Value Is Not Null Order By Bez_Value"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerPropertyData)

					While reader.Read

						Dim CustomerContactData = New CustomerPropertyData()
						CustomerContactData.recid = SafeGetInteger(reader, "ID", 0)
						CustomerContactData.bez_value = SafeGetDecimal(reader, "Bez_Value", 0)
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

		Function AddCustomerPropertyData(ByVal data As CustomerPropertyData) As Boolean Implements ITablesDatabaseAccess.AddCustomerPropertyData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDFProperty (ColorCode, Bezeichnung, bez_Value, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Bez_Value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_Value"
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

		Function UpdateCustomerPropertyData(ByVal data As CustomerPropertyData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerPropertyData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez money; "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bez_Value From Tab_KDFProperty Where ID = @ID), 0); "

			sql &= "Update dbo.Tab_KDFProperty Set [ColorCode] = @Bez_Value, Bezeichnung = @Bez_d, Bez_Value = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Kunden Set [FProperty] = @Bez_Value Where FProperty = @OldBez; "

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

		Function DeleteCustomerPropertyData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerPropertyData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDFProperty "
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


#Region "Customer contact data"

		''' <summary>
		''' Loads contact data.
		''' </summary>
		''' <returns>List of contact data.</returns>
		Function LoadCustomerContactData() As IEnumerable(Of CustomerContactData) Implements ITablesDatabaseAccess.LoadCustomerContactData

			Dim result As List(Of CustomerContactData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.TAB_KDkontakt Order By Bez_Value"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerContactData)

					While reader.Read

						Dim CustomerContactData = New CustomerContactData()
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

		Function AddCustomerContactData(ByVal data As CustomerContactData) As Boolean Implements ITablesDatabaseAccess.AddCustomerContactData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.TAB_KDkontakt ([Description], Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Bez_Value"
			sql &= ", @Bez_Value"
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

		Function UpdateCustomerContactData(ByVal data As CustomerContactData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerContactData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Description From TAB_KDkontakt Where ID = @ID), ''); "

			sql &= "Update dbo.TAB_KDkontakt Set [Description] = @Bez_Value, "
			sql &= "Bez_Value = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Kunden Set [HowKontakt] = @Bez_Value Where HowKontakt = @OldBez; "

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

		Function DeleteCustomerContactData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerContactData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.TAB_KDkontakt "
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


#Region "Customer state data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadCustomerStateData1() As IEnumerable(Of CustomerStateData) Implements ITablesDatabaseAccess.LoadCustomerStateData1

			Dim result As List(Of CustomerStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_KDStat Order By Bez_Value"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerStateData)

					While reader.Read

						Dim data = New CustomerStateData()
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

		Function AddCustomerStateData1(ByVal data As CustomerStateData) As Boolean Implements ITablesDatabaseAccess.AddCustomerStateData1

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDStat ([Description], Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @bez_value"
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

		Function UpdateCustomerStateData1(ByVal data As CustomerStateData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerStateData1

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Description] From Tab_KDStat Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KDStat Set [Description] = @bez_value, "
			sql &= "Bez_Value = @Bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update Kunden Set KDState1 = @bez_value Where KDState1 = @Oldbez; "

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

		Function DeleteCustomerStateData1(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerStateData1

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDStat "
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
		Function LoadCustomerStateData2() As IEnumerable(Of CustomerStateData) Implements ITablesDatabaseAccess.LoadCustomerStateData2

			Dim result As List(Of CustomerStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.TAB_KDStat2 Order By Bez_Value"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerStateData)

					While reader.Read

						Dim data = New CustomerStateData()
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

		Function AddCustomerStateData2(ByVal data As CustomerStateData) As Boolean Implements ITablesDatabaseAccess.AddCustomerStateData2

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDStat2 (Bezeichnung, Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @bez_value"
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

		Function UpdateCustomerStateData2(ByVal data As CustomerStateData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerStateData2

			Dim success = True

			Dim sql As String

			sql = "Declare @Oldbez nvarchar(255); "
			sql &= "Set @Oldbez = IsNull((Select Top 1 [Bezeichnung] From Tab_KDStat2 Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KDStat2 Set Bezeichnung = @bez_value, "
			sql &= "Bez_Value = @Bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update Kunden Set KDState2 = @bez_value Where KDState2 = @Oldbez; "

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

		Function DeleteCustomerStateData2(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerStateData2

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDStat2 "
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


#Region "customer employement type"

		Function LoadCustomerEmployementTypeData() As IEnumerable(Of CustomerEmployementTypeData) Implements ITablesDatabaseAccess.LoadCustomerEmployementTypeData

			Dim result As List(Of CustomerEmployementTypeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_KDAnstellung Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerEmployementTypeData)

					While reader.Read

						Dim data = New CustomerEmployementTypeData()
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

		Function AddCustomerEmployementTypeData(ByVal data As CustomerEmployementTypeData) As Boolean Implements ITablesDatabaseAccess.AddCustomerEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDAnstellung (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateCustomerEmployementTypeData(ByVal data As CustomerEmployementTypeData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_KDAnstellung Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KDAnstellung Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.KD_Anstellung Set [Bezeichnung] = @Bez_Value Where Bezeichnung = @OldBez; "

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

		Function DeleteCustomerEmployementTypeData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerEmployementTypeData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDAnstellung "
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



#Region "customer stichwort"

		Function LoadCustomerStichwortData() As IEnumerable(Of CustomerStichwortData) Implements ITablesDatabaseAccess.LoadCustomerStichwortData

			Dim result As List(Of CustomerStichwortData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_Stichwort Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerStichwortData)

					While reader.Read

						Dim data = New CustomerStichwortData()
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

		Function AddCustomerStichwortData(ByVal data As CustomerStichwortData) As Boolean Implements ITablesDatabaseAccess.AddCustomerStichwortData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Stichwort (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateCustomerStichwortData(ByVal data As CustomerStichwortData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerStichwortData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_Stichwort Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_Stichwort Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID "

			sql &= "Update dbo.KD_Stichwort Set [Bezeichnung] = @Bez_Value Where Bezeichnung = @OldBez; "

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

		Function DeleteCustomerStichwortData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerStichwortData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Stichwort "
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



#Region "Customer contact reserve data"

		Function LoadCustomerContactReserveData(ByVal contactReserveType As ContactReserveType) As IEnumerable(Of Customer.DataObjects.CustomerReserveData) Implements ITablesDatabaseAccess.LoadCustomerContactReserveData

			Dim result As List(Of Customer.DataObjects.CustomerReserveData) = Nothing

			Dim sql As String

			sql = String.Format("SELECT ID, Bezeichnung, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_KDRes{0} Order By Bezeichnung", CType(contactReserveType, Integer))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.CustomerReserveData)

					While reader.Read

						Dim data = New Customer.DataObjects.CustomerReserveData()
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

		Function AddCustomerContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.CustomerReserveData) As Boolean Implements ITablesDatabaseAccess.AddCustomerContactReserveData

			Dim success = True

			Dim sql As String

			sql = String.Format("Insert Into dbo.Tab_KDRes{0} (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values (", CType(contactReserveType, Integer))
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

		Function UpdateCustomerContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.CustomerReserveData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerContactReserveData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_KDRes{0} Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KDRes{0} Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Kunden Set KDRes{0} = @Description Where KDRes{0} = @OldBez; "
			sql &= "Update dbo.KD_ZRes{0} Set Bezeichnung = @Description Where Bezeichnung = @OldBez; "

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

		Function DeleteCustomerContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerContactReserveData

			Dim success = True

			Dim sql As String

			sql = String.Format("Delete dbo.Tab_KDRes{0} ", CType(contactReserveType, Integer))
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



#Region "Customer document category data"

		''' <summary>
		''' Loads document category data.
		''' </summary>
		Function LoadCustomerDocumentCategoryData() As IEnumerable(Of Customer.DataObjects.CustomerDocumentCategoryData) Implements ITablesDatabaseAccess.LoadCustomerDocumentCategoryData

			Dim result As List(Of Customer.DataObjects.CustomerDocumentCategoryData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Categorie_Nr, Bez_D, Bez_F, Bez_I, Bez_E FROM Tab_KDDocCategories ORDER BY ID ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of Customer.DataObjects.CustomerDocumentCategoryData)

					While reader.Read

						Dim categoryDataData As New Customer.DataObjects.CustomerDocumentCategoryData
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

		Function AddCustomerDocumentCategoryData(ByVal data As Customer.DataObjects.CustomerDocumentCategoryData) As Boolean Implements ITablesDatabaseAccess.AddCustomerDocumentCategoryData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_KDDocCategories (Categorie_Nr, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateCustomerDocumentCategoryData(ByVal data As Customer.DataObjects.CustomerDocumentCategoryData) As Boolean Implements ITablesDatabaseAccess.UpdateCustomerDocumentCategoryData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez int; "
			sql &= "Set @OldBez = IsNull((Select Top 1 Categorie_Nr From Tab_KDDocCategories Where ID = @ID), 0); "

			sql &= "Update dbo.Tab_KDDocCategories Set Categorie_Nr = @Categorie_Nr, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "

			sql &= "Where ID = @ID; "

			sql &= "Update dbo.KD_ZDoc Set [Categorie_Nr] = @Categorie_Nr Where Categorie_Nr = @OldBez; "

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

		Function DeleteCustomerDocumentCategoryData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCustomerDocumentCategoryData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KDDocCategories "
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
