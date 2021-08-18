
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


		Function LoadAssessmentData() As IEnumerable(Of AssessmentData) Implements ITablesDatabaseAccess.LoadAssessmentData

			Dim result As List(Of AssessmentData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_Beurteilung Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AssessmentData)

					While reader.Read

						Dim data = New AssessmentData()
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

		Function AddAssessmentData(ByVal data As AssessmentData) As Boolean Implements ITablesDatabaseAccess.AddAssessmentData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Beurteilung (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateAssessmentData(ByVal data As AssessmentData) As Boolean Implements ITablesDatabaseAccess.UpdateAssessmentData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_Beurteilung Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_Beurteilung Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MA_Beurteilung Set [Bezeichnung] = @Bez_Value Where Bezeichnung = @OldBez; "

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

		Function DeleteAssessmentData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteAssessmentData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Beurteilung "
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



		Function LoadCommunicationData() As IEnumerable(Of CommunicationTypeData) Implements ITablesDatabaseAccess.LoadCommunicationTypeData

			Dim result As List(Of CommunicationTypeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung AS bez_value, bez_d, bez_i, bez_f, bez_e FROM dbo.Tab_MAKommunikation Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CommunicationTypeData)

					While reader.Read

						Dim data = New CommunicationTypeData()
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

		Function AddCommunicationData(ByVal data As CommunicationTypeData) As Boolean Implements ITablesDatabaseAccess.AddCommunicationTypeData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_MAKommunikation (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateCommunicationData(ByVal data As CommunicationTypeData) As Boolean Implements ITablesDatabaseAccess.UpdateCommunicationTypeData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_MAKommunikation Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_MAKommunikation Set Bezeichnung = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.MA_Kommunikation Set [Bezeichnung] = @Bez_Value Where Bezeichnung = @OldBez; "

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

		Function DeleteCommunicationData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCommunicationTypeData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_MAKommunikation "
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
