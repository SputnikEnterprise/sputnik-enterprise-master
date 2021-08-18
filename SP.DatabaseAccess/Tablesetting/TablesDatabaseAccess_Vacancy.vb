
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess





#Region "vacancy contact data"

		''' <summary>
		''' Loads contact data.
		''' </summary>
		''' <returns>List of contact data.</returns>
		Function LoadVacancyContactData() As IEnumerable(Of VacancyContactData) Implements ITablesDatabaseAccess.LoadVacancyContactData

			Dim result As List(Of VacancyContactData) = Nothing

			Dim sql As String

			sql = "SELECT ID, RecValue, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.tbl_base_Vakkontakt Order By RecValue"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of VacancyContactData)

					While reader.Read

						Dim CustomerContactData = New VacancyContactData()
						CustomerContactData.ID = SafeGetInteger(reader, "ID", 0)
						CustomerContactData.recvalue = SafeGetInteger(reader, "RecValue", 0)
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

		Function AddVacancyContactData(ByVal data As VacancyContactData) As Boolean Implements ITablesDatabaseAccess.AddVacancyContactData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.tbl_base_Vakkontakt (RecValue, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@RecValue"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecValue", ReplaceMissing(data.recvalue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateVacancyContactData(ByVal data As VacancyContactData) As Boolean Implements ITablesDatabaseAccess.UpdateVacancyContactData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.tbl_base_Vakkontakt Set RecValue = @RecValue, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@RecValue", ReplaceMissing(data.recvalue, DBNull.Value)))
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

		Function DeleteVacancyContactData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteVacancyContactData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.tbl_base_Vakkontakt "
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


#Region "vacancy state data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadVacancyStateData() As IEnumerable(Of VacancyStateData) Implements ITablesDatabaseAccess.LoadVacancyStateData

			Dim result As List(Of VacancyStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, RecValue, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.tbl_base_VakState Order By RecValue"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of VacancyStateData)

					While reader.Read

						Dim data = New VacancyStateData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.recvalue = SafeGetInteger(reader, "RecValue", 0)
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

		Function AddVacancyStateData(ByVal data As VacancyStateData) As Boolean Implements ITablesDatabaseAccess.AddVacancyStateData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.tbl_base_VakState (RecValue, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@RecValue"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecValue", ReplaceMissing(data.recvalue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateVacancyStateData(ByVal data As VacancyStateData) As Boolean Implements ITablesDatabaseAccess.UpdateVacancyStateData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 RecValue From tbl_base_VakState Where ID = @ID), ''); "

			sql &= "Update dbo.tbl_base_VakState Set RecValue = @RecValue, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Vakanzen Set [VakState] = @RecValue Where VakState = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.id, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@RecValue", ReplaceMissing(data.recvalue, DBNull.Value)))
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

		Function DeleteVacancyStateData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteVacancyStateData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.tbl_base_VakState "
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



#Region "vacancy group data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadVacancyGroupData() As IEnumerable(Of VacancyGroupData) Implements ITablesDatabaseAccess.LoadVacancyGroupData

			Dim result As List(Of VacancyGroupData) = Nothing

			Dim sql As String

			sql = "SELECT ID, FieldValue Rec_Value, Bezeichnung Bez_D, Bezeichnung_IT Bez_I, Bezeichnung_Fr Bez_F, Bezeichnung_EN Bez_E FROM Tab_VakGroup Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of VacancyGroupData)

					While reader.Read

						Dim data = New VacancyGroupData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Bez_Value = SafeGetString(reader, "Rec_Value")
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

		Function LoadVacancySubGroupData() As IEnumerable(Of VacancySubGroupData) Implements ITablesDatabaseAccess.LoadVacancySubGroupData

			Dim result As List(Of VacancySubGroupData) = Nothing

			Dim sql As String

			sql = "SELECT ID, MainGroup, SubGroup, Bez_DE, Bez_IT, Bez_Fr, Bez_EN FROM tbl_Base_VacancySubGroup Order By MainGroup, SubGroup"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of VacancySubGroupData)

					While reader.Read

						Dim data = New VacancySubGroupData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.MainGroup = SafeGetString(reader, "MainGroup")
						data.SubGroup = SafeGetString(reader, "SubGroup")
						data.Bez_DE = SafeGetString(reader, "Bez_DE")
						data.Bez_IT = SafeGetString(reader, "Bez_IT")
						data.Bez_FR = SafeGetString(reader, "Bez_Fr")
						data.Bez_EN = SafeGetString(reader, "Bez_EN")


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

		Function LoadAssingedVacancySubGroupData(ByVal mainGroup As String, ByVal language As String) As IEnumerable(Of VacancySubGroupData) Implements ITablesDatabaseAccess.LoadAssingedVacancySubGroupData

			Dim result As List(Of VacancySubGroupData) = Nothing

			Dim sql As String

			Dim myLanguage As String = ReplaceMissing(language, "DE")
			Dim selLanguage As String = myLanguage
			Select Case myLanguage.ToLower().TrimEnd()
				Case "deutsch", "de", "d"
					selLanguage = "DE"
				Case "italienisch", "it", "i"
					selLanguage = "IT"
				Case "französisch", "fr", "f"
					selLanguage = "FR"
				Case "englisch", "en", "e"
					selLanguage = "EN"

				Case Else
					selLanguage = "DE"
			End Select

			sql = "SELECT ID"
			sql &= ",MainGroup"
			sql &= ",SubGroup"
			sql &= String.Format(",IsNull(Bez_{0}, SubGroup) TranslatedValue ", selLanguage)
			sql &= "FROM tbl_Base_VacancySubGroup "
			sql &= "Where MainGroup = @mainGroup "
			sql &= "Order By MainGroup, SubGroup"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mainGroup", ReplaceMissing(mainGroup, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)


			Try

				If (Not reader Is Nothing) Then

					result = New List(Of VacancySubGroupData)

					While reader.Read

						Dim data = New VacancySubGroupData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.MainGroup = SafeGetString(reader, "MainGroup")
						data.SubGroup = SafeGetString(reader, "SubGroup")
						data.TranslatedValue = SafeGetString(reader, "TranslatedValue")


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

		Function AddVacancyGroupData(ByVal data As VacancyGroupData) As Boolean Implements ITablesDatabaseAccess.AddVacancyGroupData

			Dim success = True

			Dim sql As String
			'			sql = "select ID, FieldValue Rec_Value, Bezeichnung Bez_D, Bezeichnung_IT Bez_I, Bezeichnung_Fr Bez_F, Bezeichnung_EN Bez_E FROM Tab_VakGroup Order By Bezeichnung"

			sql = "Insert Into dbo.Tab_VakGroup (FieldValue, Bezeichnung, Bezeichnung_IT, Bezeichnung_Fr, Bezeichnung_EN) Values ("
			sql &= "@Rec_Value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Rec_Value", ReplaceMissing(data.Bez_Value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateVacancyGroupData(ByVal data As VacancyGroupData) As Boolean Implements ITablesDatabaseAccess.UpdateVacancyGroupData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 FieldValue From Tab_VakGroup Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_VakGroup Set FieldValue = @Description, "
			sql &= "Bezeichnung = @Bez_d, "
			sql &= "Bezeichnung_IT = @Bez_I, "
			sql &= "Bezeichnung_Fr = @Bez_F, "
			sql &= "Bezeichnung_EN = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Vakanzen Set [Gruppe] = @Description Where Gruppe = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Bez_Value, DBNull.Value)))
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

		Function DeleteVacancyGroupData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteVacancyGroupData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_VakGroup "
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




	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess





#Region "Offer contact data"

		''' <summary>
		''' Loads contact data.
		''' </summary>
		''' <returns>List of contact data.</returns>
		Function LoadOfferContactData() As IEnumerable(Of OfferContactData) Implements ITablesDatabaseAccess.LoadOfferContactData

			Dim result As List(Of OfferContactData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_OfferKontakt Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of OfferContactData)

					While reader.Read

						Dim CustomerContactData = New OfferContactData()
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

		Function AddOfferContactData(ByVal data As OfferContactData) As Boolean Implements ITablesDatabaseAccess.AddOfferContactData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_OfferKontakt (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateOfferContactData(ByVal data As OfferContactData) As Boolean Implements ITablesDatabaseAccess.UpdateOfferContactData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_OfferKontakt Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_OfferKontakt Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Offers Set [Of_Kontakt] = @Description Where Of_Kontakt = @OldBez; "

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

		Function DeleteOfferContactData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteOfferContactData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_OfferKontakt "
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


#Region "Offer state data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadOfferStateData() As IEnumerable(Of OfferStateData) Implements ITablesDatabaseAccess.LoadOfferStateData

			Dim result As List(Of OfferStateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_OfferState Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of OfferStateData)

					While reader.Read

						Dim data = New OfferStateData()
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

		Function AddOfferStateData(ByVal data As OfferStateData) As Boolean Implements ITablesDatabaseAccess.AddOfferStateData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_OfferState (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateOfferStateData(ByVal data As OfferStateData) As Boolean Implements ITablesDatabaseAccess.UpdateOfferStateData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_OfferState Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_OfferState Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Offers Set [Of_State] = @Description Where Of_State = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.id, DBNull.Value)))
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

		Function DeleteOfferStateData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteOfferStateData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_OfferState "
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



#Region "Offer group data"

		''' <summary>
		''' Loads state data.
		''' </summary>
		Function LoadOfferGroupData() As IEnumerable(Of OfferGroupData) Implements ITablesDatabaseAccess.LoadOfferGroupData

			Dim result As List(Of OfferGroupData) = Nothing

			Dim sql As String

			sql = "select ID, Bezeichnung, Bez_D, Bez_I, Bez_F, Bez_E FROM Tab_OfferGruppe Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of OfferGroupData)

					While reader.Read

						Dim data = New OfferGroupData()
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

		Function AddOfferGroupData(ByVal data As OfferGroupData) As Boolean Implements ITablesDatabaseAccess.AddOfferGroupData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_OfferGruppe (Bezeichnung, Bez_D, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateOfferGroupData(ByVal data As OfferGroupData) As Boolean Implements ITablesDatabaseAccess.UpdateOfferGroupData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(50); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_OfferGruppe Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_OfferGruppe Set Bezeichnung = @Description, "
			sql &= "Bez_D = @Bez_I, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update dbo.Offers Set [OF_Gruppe] = @Description Where OF_Gruppe = @OldBez; "

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

		Function DeleteOfferGroupData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteOfferGroupData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_OfferGruppe "
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
