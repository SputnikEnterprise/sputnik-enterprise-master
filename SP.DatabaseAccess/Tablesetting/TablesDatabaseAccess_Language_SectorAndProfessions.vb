

Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess



		Function LoadMainLanguageData() As IEnumerable(Of LanguageData) Implements ITablesDatabaseAccess.LoadMainLanguageData

			Dim result As List(Of LanguageData) = Nothing

			Dim sql As String

			sql = "SELECT ID, [Description] AS bez_value, bez_d, bez_i, bez_f, bez_e FROM TAB_Sprache Order By [Description]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LanguageData)

					While reader.Read

						Dim data = New LanguageData()
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

		Function AddMainLanguageData(ByVal data As LanguageData) As Boolean Implements ITablesDatabaseAccess.AddMainLanguageData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.TAB_Sprache ([Description], Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateMainLanguageData(ByVal data As LanguageData) As Boolean Implements ITablesDatabaseAccess.UpdateMainLanguageData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 [Description] From TAB_Sprache Where ID = @ID), ''); "

			sql = "Update dbo.TAB_Sprache Set [Description] = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Mitarbeiter Set [Sprache] = @bez_value Where Sprache = @OldBez; "
			sql &= "Update dbo.Kunden Set [Sprache] = @bez_value Where Sprache = @OldBez; "

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

		Function DeleteMainLanguageData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteMainLanguageData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.TAB_Sprache "
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
