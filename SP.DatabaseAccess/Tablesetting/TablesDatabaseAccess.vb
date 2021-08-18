
Imports SP.DatabaseAccess.Tablesetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace TableSetting

	''' <summary>
	''' Tablesetting database access class.
	''' </summary>
	Public Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region





#Region "common tables: contact categories"

		Function LoadContactCategoryData() As IEnumerable(Of SP.DatabaseAccess.Common.DataObjects.ContactType1Data) Implements ITablesDatabaseAccess.LoadContactCategoryData

			Dim result As List(Of SP.DatabaseAccess.Common.DataObjects.ContactType1Data) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bez_ID, IconIndex, Result, RecNr, Bez_DE, Bez_FR, Bez_IT, Bez_EN FROM Tab_KontaktType1 ORDER BY RecNr ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SP.DatabaseAccess.Common.DataObjects.ContactType1Data)

					While reader.Read

						Dim contactType1Data = New SP.DatabaseAccess.Common.DataObjects.ContactType1Data()
						contactType1Data.ID = SafeGetInteger(reader, "ID", 0)
						contactType1Data.Bez_ID = SafeGetString(reader, "Bez_ID")
						contactType1Data.IconIndex = SafeGetInteger(reader, "IconIndex", Nothing)
						contactType1Data.Result = SafeGetString(reader, "Result")
						contactType1Data.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						contactType1Data.Caption_DE = SafeGetString(reader, "Bez_DE")
						contactType1Data.Caption_IT = SafeGetString(reader, "Bez_IT")
						contactType1Data.Caption_FR = SafeGetString(reader, "Bez_FR")
						contactType1Data.Caption_EN = SafeGetString(reader, "Bez_EN")

						result.Add(contactType1Data)

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

		Function AddContactCategoryData(ByVal data As SP.DatabaseAccess.Common.DataObjects.ContactType1Data) As Boolean Implements ITablesDatabaseAccess.AddContactCategoryData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Tab_KontaktType1 (Bez_ID, IconIndex, Result, RecNr, Bez_DE, Bez_FR, Bez_IT, Bez_EN) Values ("
			sql &= "@Bez_ID"
			sql &= ", @IconIndex"
			sql &= ", @Result"
			sql &= ", @RecNr"
			sql &= ", @Bez_DE"
			sql &= ", @Bez_FR"
			sql &= ", @Bez_IT"
			sql &= ", @Bez_EN"
			sql &= ")"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_ID", ReplaceMissing(data.Bez_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@IconIndex", ReplaceMissing(data.IconIndex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(data.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(data.RecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_DE", ReplaceMissing(data.Caption_DE, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_FR", ReplaceMissing(data.Caption_FR, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_IT", ReplaceMissing(data.Caption_IT, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_EN", ReplaceMissing(data.Caption_EN, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateContactCategoryData(ByVal data As SP.DatabaseAccess.Common.DataObjects.ContactType1Data) As Boolean Implements ITablesDatabaseAccess.UpdateContactCategoryData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bez_ID From Tab_KontaktType1 Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_KontaktType1 Set "
			sql &= "Bez_ID = @Bez_ID, "
			sql &= "IconIndex = @IconIndex, "
			sql &= "Result = @Result, "
			sql &= "RecNr = @RecNr, "
			sql &= "Bez_DE = @Bez_DE, "
			sql &= "Bez_FR = @Bez_FR, "
			sql &= "Bez_IT = @Bez_IT, "
			sql &= "Bez_EN = @Bez_EN "

			sql &= "Where ID = @ID; "

			sql &= "Update MA_Kontakte Set KontaktType1 = @Bez_ID Where KontaktType1 = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.ID, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Bez_ID", ReplaceMissing(data.Bez_ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@IconIndex", ReplaceMissing(data.IconIndex, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(data.Result, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(data.RecNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_DE", ReplaceMissing(data.Caption_DE, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_FR", ReplaceMissing(data.Caption_FR, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_IT", ReplaceMissing(data.Caption_IT, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_EN", ReplaceMissing(data.Caption_EN, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteContactCategoryData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteContactCategoryData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_KontaktType1 "
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


#Region "common tables: employement categorized"

		Function LoadEmployementCategorizedData() As IEnumerable(Of ES.DataObjects.ESMng.ESCategorizationData) Implements ITablesDatabaseAccess.LoadEmployementCategorizedData

			Dim result As List(Of ES.DataObjects.ESMng.ESCategorizationData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung, Bez_D, Bez_I, Bez_F, Bez_E From dbo.Tab_ESEinstufung Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ES.DataObjects.ESMng.ESCategorizationData)

					While reader.Read

						Dim data = New ES.DataObjects.ESMng.ESCategorizationData()
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Description = SafeGetString(reader, "Bezeichnung")
						data.bez_d = SafeGetString(reader, "Bez_d")
						data.bez_i = SafeGetString(reader, "Bez_i")
						data.bez_f = SafeGetString(reader, "Bez_f")
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

		Function AddEmployementCategorizedData(ByVal data As ES.DataObjects.ESMng.ESCategorizationData) As Boolean Implements ITablesDatabaseAccess.AddEmployementCategorizedData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Tab_ESEinstufung (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_D", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateEmployementCategorizedData(ByVal data As ES.DataObjects.ESMng.ESCategorizationData) As Boolean Implements ITablesDatabaseAccess.UpdateEmployementCategorizedData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_ESEinstufung Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_ESEinstufung Set Bezeichnung = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update ES Set Einstufung = @Description Where Einstufung = @OldBez; "

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

		Function DeleteEmployementCategorizedData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteEmployementCategorizedData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_ESEinstufung "
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


#Region "common tables: Businessbranchs: Filialen"

		Function LoadBusinessBranchsData() As IEnumerable(Of AvilableBusinessBranchData) Implements ITablesDatabaseAccess.LoadBusinessBranchsData

			Dim result As List(Of AvilableBusinessBranchData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung As Bez_Value, Code_1 From dbo.Filialen Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AvilableBusinessBranchData)

					While reader.Read

						Dim data = New AvilableBusinessBranchData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "Bez_Value")

						data.Code_1 = SafeGetString(reader, "Code_1")

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

		Function AddSBusinessBranchsData(ByVal data As AvilableBusinessBranchData) As Boolean Implements ITablesDatabaseAccess.AddBusinessBranchsData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Filialen (Bezeichnung, Code_1) Values ("
			sql &= "@Bez_Value, @Code_1)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Code_1", ReplaceMissing(data.Code_1, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateBusinessBranchsData(ByVal data As AvilableBusinessBranchData) As Boolean Implements ITablesDatabaseAccess.UpdateBusinessBranchsData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Filialen Where ID = @ID), ''); "

			sql &= "Update dbo.Filialen Set "
			sql &= "Bezeichnung = @Bez_Value, "
			sql &= "Code_1 = @Code_1 "
			sql &= "Where ID = @ID; "

			sql &= "Update MA_Filiale Set Bezeichnung = @Bez_Value Where Bezeichnung = @OldBez; "
			sql &= "Update Mitarbeiter Set MAFiliale = Replace(MAFiliale, @OldBez, @Bez_Value) "
			sql &= "Where MAFiliale = @OldBez Or MAFiliale Like @OldBez + ',%' Or MAFiliale Like '%, ' + @OldBez; "

			sql &= "Update KD_Filiale Set Bezeichnung = @Bez_Value Where Bezeichnung = @OldBez; "
			sql &= "Update Kunden Set KDFiliale = Replace(KDFiliale, @OldBez, @Bez_Value) "
			sql &= "Where KDFiliale = @OldBez Or KDFiliale Like @OldBez + ',%' Or KDFiliale Like '%, ' + @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.bez_value, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Code_1", ReplaceMissing(data.Code_1, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteBusinessBranchsData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteBusinessBranchsData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Filialen "
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


#Region "common tables: costcenter: KST1"

		Function LoadCostCenter1() As IEnumerable(Of CostCenter1Data) Implements ITablesDatabaseAccess.LoadCostCenter1

			Dim result As List(Of CostCenter1Data) = Nothing

			Dim sql As String

			sql = "SELECT ID, "
			sql &= "( CASE ISNUMERIC(KSTName) "
			sql &= "WHEN 1 THEN CONVERT(INT, KSTName) "
			sql &= "ELSE 0 "
			sql &= "END ) "
			sql &= "KSTName, KSTBezeichnung "
			sql &= "From dbo.Tab_Kst1"
			sql &= " Order By KSTName"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CostCenter1Data)

					While reader.Read

						Dim data = New CostCenter1Data()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.kstname = SafeGetInteger(reader, "kstname", 0)

						data.kstbezeichnung = SafeGetString(reader, "KSTBezeichnung")

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

		Function AddCostCenter1Data(ByVal data As CostCenter1Data) As Boolean Implements ITablesDatabaseAccess.AddCostCenter1Data

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Tab_Kst1 (KSTName, KSTBezeichnung) Values ("
			sql &= "@kstname, @kstbezeichnung)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@kstname", ReplaceMissing(data.kstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kstbezeichnung", ReplaceMissing(data.kstbezeichnung, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCostCenter1Data(ByVal data As CostCenter1Data) As Boolean Implements ITablesDatabaseAccess.UpdateCostCenter1Data

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Kst1 Set "
			sql &= "KSTName = @kstname, "
			sql &= "KSTBezeichnung = @kstbezeichnung "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@kstname", ReplaceMissing(data.kstname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@kstbezeichnung", ReplaceMissing(data.kstbezeichnung, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCostCenter1Data(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCostCenter1Data

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Kst1 "
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


#Region "common tables: costcenter: KST2"

		Function LoadCostCenter2() As IEnumerable(Of CostCenter2Data) Implements ITablesDatabaseAccess.LoadCostCenter2

			Dim result As List(Of CostCenter2Data) = Nothing

			Dim sql As String

			sql = "SELECT ID, KSTName, KSTBezeichnung, ( CASE ISNUMERIC(KSTName1) "
			sql &= "WHEN 1 THEN CONVERT(INT, KSTName1) "
			sql &= "ELSE 0 "
			sql &= "END ) "
			sql &= "KSTName1 "
			sql &= "From dbo.Tab_Kst2"
			sql &= " Order By KSTName"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CostCenter2Data)

					While reader.Read

						Dim data = New CostCenter2Data()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.kstname = SafeGetString(reader, "kstname")

						data.kstbezeichnung = SafeGetString(reader, "KSTBezeichnung")
						data.kstname1 = SafeGetInteger(reader, "KSTName1", 0)

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

		Function AddCostCenter2Data(ByVal data As CostCenter2Data) As Boolean Implements ITablesDatabaseAccess.AddCostCenter2Data

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Tab_Kst2 (KSTName, KSTBezeichnung, KSTName1) Values ("
			sql &= "@kstname, @kstbezeichnung, IsNull((Select Top 1 KSTName From Tab_Kst1 Where Convert(Int, KSTName) = @KSTName1), 0) )"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@kstname", ReplaceMissing(data.kstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@kstbezeichnung", ReplaceMissing(data.kstbezeichnung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KSTName1", ReplaceMissing(data.kstname1, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCostCenter2Data(ByVal data As CostCenter2Data) As Boolean Implements ITablesDatabaseAccess.UpdateCostCenter2Data

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Kst2 Set "
			sql &= "KSTName = @kstname, "
			sql &= "KSTBezeichnung = @kstbezeichnung, "
			sql &= "KSTName1 = @KSTName1 "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@kstname", ReplaceMissing(data.kstname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@kstbezeichnung", ReplaceMissing(data.kstbezeichnung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@KSTName1", ReplaceMissing(data.kstname1, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCostCenter2Data(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCostCenter2Data

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Kst2 "
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


#Region "common tables: currency: Währung"

		Function LoadCurrencyData() As IEnumerable(Of CurrencyData) Implements ITablesDatabaseAccess.LoadCurrencyData

			Dim result As List(Of CurrencyData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld As Bez_Value, [Description] From dbo.Tab_Currency Order By GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CurrencyData)

					While reader.Read

						Dim data = New CurrencyData()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.code = SafeGetString(reader, "Bez_Value")

						data.description = SafeGetString(reader, "Description")

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

		Function AddCurrencyData(ByVal data As CurrencyData) As Boolean Implements ITablesDatabaseAccess.AddCurrencyData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Tab_Currency (GetField, [Description]) Values ("
			sql &= "@Bez_Value, @Description)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.description, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCurrencyData(ByVal data As CurrencyData) As Boolean Implements ITablesDatabaseAccess.UpdateCurrencyData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Currency Set "
			sql &= "GetField = @Bez_Value, "
			sql &= "[Description] = @Description "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.code, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.description, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteCurrencyData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCurrencyData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Currency "
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


#Region "common tables: Salutation: Anrede und AnredeFormen"


		Function LoadSalutationData() As IEnumerable(Of SalutationData) Implements ITablesDatabaseAccess.LoadSalutationData

			Dim result As List(Of SalutationData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Anrede, Briefform, Anrede_D, Anrede_I, Anrede_F, Anrede_E, BriefForm_D, BriefForm_I, BriefForm_F, BriefForm_E from Anrede Order By ID"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SalutationData)

					While reader.Read

						Dim data = New SalutationData()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.salutation = SafeGetString(reader, "anrede")

						data.salutation_d = SafeGetString(reader, "Anrede_D")
						data.salutation_i = SafeGetString(reader, "Anrede_I")
						data.salutation_f = SafeGetString(reader, "Anrede_F")
						data.salutation_e = SafeGetString(reader, "Anrede_E")

						data.letterform = SafeGetString(reader, "BriefForm")

						data.letterform_d = SafeGetString(reader, "BriefForm_D")
						data.letterform_i = SafeGetString(reader, "BriefForm_I")
						data.letterform_f = SafeGetString(reader, "BriefForm_F")
						data.letterform_e = SafeGetString(reader, "BriefForm_E")

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

		Function AddSalutationData(ByVal data As SalutationData) As Boolean Implements ITablesDatabaseAccess.AddSalutationData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Anrede (Anrede, Briefform, Anrede_D, Anrede_I, Anrede_F, Anrede_E, BriefForm_D, BriefForm_I, BriefForm_F, BriefForm_E) Values ("
			sql &= "@Anrede, @Briefform, @Anrede_D, @Anrede_I, @Anrede_F, @Anrede_E, @BriefForm_D, @BriefForm_I, @BriefForm_F, @BriefForm_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Anrede", ReplaceMissing(data.salutation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Briefform", ReplaceMissing(data.letterform, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@Anrede_D", ReplaceMissing(data.salutation_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Anrede_I", ReplaceMissing(data.salutation_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Anrede_F", ReplaceMissing(data.salutation_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Anrede_E", ReplaceMissing(data.salutation_e, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@Briefform_D", ReplaceMissing(data.letterform_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Briefform_I", ReplaceMissing(data.letterform_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Briefform_F", ReplaceMissing(data.letterform_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Briefform_E", ReplaceMissing(data.letterform_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateSalutationData(ByVal data As SalutationData) As Boolean Implements ITablesDatabaseAccess.UpdateSalutationData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Declare @OldForm nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Anrede From Anrede Where ID = @ID), ''); "
			sql &= "Set @OldForm = IsNull((Select Top 1 Briefform From Anrede Where ID = @ID), ''); "

			sql &= "Update dbo.Anrede Set "
			sql &= "Anrede = @Anrede, "
			sql &= "Anrede_d = @Anrede_d, "
			sql &= "Anrede_I = @Anrede_I, "
			sql &= "Anrede_F = @Anrede_F, "
			sql &= "Anrede_E = @Anrede_E, "

			sql &= "Briefform = @Briefform, "
			sql &= "Briefform_d = @Briefform_d, "
			sql &= "Briefform_I = @Briefform_I, "
			sql &= "Briefform_F = @Briefform_F, "
			sql &= "Briefform_E = @Briefform_E "

			sql &= "Where ID = @ID; "

			sql &= "Update MAKontakt_Komm Set AnredeForm = @Anrede, Briefanrede = @Briefform Where Anredeform = @OldBez; "
			sql &= "Update KD_Zustaendig Set Anrede = @Anrede, AnredeForm = @Briefform Where Anredeform = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Anrede", ReplaceMissing(data.salutation, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Briefform", ReplaceMissing(data.letterform, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Anrede_D", ReplaceMissing(data.salutation_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Anrede_I", ReplaceMissing(data.salutation_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Anrede_F", ReplaceMissing(data.salutation_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Anrede_E", ReplaceMissing(data.salutation_e, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Briefform_D", ReplaceMissing(data.letterform_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Briefform_I", ReplaceMissing(data.letterform_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Briefform_F", ReplaceMissing(data.letterform_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Briefform_E", ReplaceMissing(data.letterform_e, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteSalutationData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteSalutationData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Anrede "
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


#Region "common tables: terms and anditions: AGB für WOS"

		''' <summary>
		''' Loads terms and conditions data.
		''' </summary>
		Function LoadTermsAndConditionsData() As IEnumerable(Of TermsAndConditionsData) Implements ITablesDatabaseAccess.LoadTermsAndConditionsData

			Dim result As List(Of TermsAndConditionsData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Bezeichnung As Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_AGB Order By Bezeichnung"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of TermsAndConditionsData)

					While reader.Read

						Dim EmployeeContactData = New TermsAndConditionsData()
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

		Function AddTermsAndConditionsData(ByVal data As TermsAndConditionsData) As Boolean Implements ITablesDatabaseAccess.AddTermsAndConditionsData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_AGB (Bezeichnung, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateTermsAndConditionsData(ByVal data As TermsAndConditionsData) As Boolean Implements ITablesDatabaseAccess.UpdateTermsAndConditionsData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Bezeichnung From Tab_AGB Where ID = @ID), ''); "

			sql &= "Update dbo.Tab_AGB Set Bezeichnung = @Bez_Value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update MAKontakt_Komm Set AGB_WOS = @Bez_Value Where AGB_WOS = @OldBez; "

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

		Function DeleteTermsAndConditionsData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteTermsAndConditionsData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_AGB "
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


#Region "SMS-Templates"

		''' <summary>
		''' Loads sms template data.
		''' </summary>
		Function LoadSMSTemplateData() As IEnumerable(Of SMSTemplateData) Implements ITablesDatabaseAccess.LoadSMSTemplateData

			Dim result As List(Of SMSTemplateData) = Nothing

			Dim sql As String

			sql = "SELECT ID, bez_value, Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_SMSTemplates Order By Bez_Value"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SMSTemplateData)

					While reader.Read

						Dim data = New SMSTemplateData()
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

		Function AddSMSTemplateData(ByVal data As SMSTemplateData) As Boolean Implements ITablesDatabaseAccess.AddSMSTemplateData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_SMSTemplates (Bez_value, Bez_d, Bez_I, Bez_F, Bez_E) Values ("
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

		Function UpdateSMSTemplateData(ByVal data As SMSTemplateData) As Boolean Implements ITablesDatabaseAccess.UpdateSMSTemplateData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_SMSTemplates Set bez_value = @bez_value, "
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

		Function DeleteSMSTemplateData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteSMSTemplateData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_SMSTemplates "
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


#Region "common tables: BVG Mann und Frau"

		Function LoadBVGData(ByVal mdNr As Integer, ByVal gender As String, ByVal year As Integer?) As IEnumerable(Of BVGData) Implements ITablesDatabaseAccess.LoadBVGData

			Dim result As List(Of BVGData) = Nothing
			gender = If(gender = "M", "Mann", "Frauen")

			Dim sql As String

			sql = "SELECT ID, Convert(Int, [Alter]) [Alter] , Convert(Money, ProzentSatz) ProzentSatz, Convert(Int, ProzJahr) ProzJahr, MDNr  From TabBVG{0}"
			sql &= " Where MDNr = @MDNr"
			sql &= " And (@Year = 0 Or ProzJahr = @Year)"
			sql &= " Order By [Alter]"
			sql = String.Format(sql, gender)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(mdNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("@Year", ReplaceMissing(year, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of BVGData)

					While reader.Read

						Dim data = New BVGData()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.Alter = SafeGetInteger(reader, "Alter", 0)

						data.ProzentSatz = SafeGetDecimal(reader, "ProzentSatz", 0)
						data.ProzJahr = SafeGetInteger(reader, "ProzJahr", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)

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

		Function AddBVGData(ByVal gender As String, ByVal data As BVGData) As Boolean Implements ITablesDatabaseAccess.AddBVGData

			Dim success = True
			gender = If(gender = "M", "Mann", "Frauen")

			Dim sql As String
			sql = "Insert Into dbo.TabBVG{0} ([Alter], ProzentSatz, ProzJahr, MDNr) Values ("
			sql &= "@Alter, @ProzentSatz, @ProzJahr, @MDNr)"
			sql = String.Format(sql, gender)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Alter", ReplaceMissing(data.Alter, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ProzentSatz", ReplaceMissing(data.ProzentSatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ProzJahr", ReplaceMissing(data.ProzJahr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateBVGData(ByVal gender As String, ByVal data As BVGData) As Boolean Implements ITablesDatabaseAccess.UpdateBVGData

			Dim success = True
			gender = If(gender = "M", "Mann", "Frauen")

			Dim sql As String

			sql = "Update dbo.TabBVG{0} Set "
			sql &= "[Alter] = @Alter, "
			sql &= "ProzentSatz = @ProzentSatz, "
			sql &= "ProzJahr = @ProzJahr, "
			sql &= "MDNr = @MDNr "

			sql &= "Where ID = @ID"
			sql = String.Format(sql, gender)

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Alter", ReplaceMissing(data.Alter, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@ProzentSatz", ReplaceMissing(data.ProzentSatz, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@ProzJahr", ReplaceMissing(data.ProzJahr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteBVGData(ByVal gender As String, ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteBVGData

			Dim success = True
			gender = If(gender = "M", "Mann", "Frauen")

			Dim sql As String

			sql = "Delete dbo.TabBVG{0} "
			sql &= "Where ID = @ID"
			sql = String.Format(sql, gender)

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


#Region "common tables: ff13lohn: [TabFerien/Feier/13]"

		Function LoadFF13Lohn() As IEnumerable(Of FF13LohnData) Implements ITablesDatabaseAccess.LoadFF13Lohn

			Dim result As List(Of FF13LohnData) = Nothing

			Dim sql As String

			' Select ID, Convert(INT, Jahrgang) Jahrgang, Convert(money, FerProzentSatz) FerProzentSatz, Convert(money, FeierProzentSatz) FeierProzentSatz, Convert(money, [13ProzentSatz]) [13ProzentSatz] From [TabFerien/Feier/13]

			sql = "SELECT ID, Convert(INT, Jahrgang) Jahrgang, Convert(money, FerProzentSatz) FerProzentSatz, Convert(money, FeierProzentSatz) FeierProzentSatz, Convert(money, [13ProzentSatz]) [13ProzentSatz] From [TabFerien/Feier/13] Order By Convert(INT, Jahrgang)"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FF13LohnData)

					While reader.Read

						Dim data = New FF13LohnData()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.Jahrgang = SafeGetInteger(reader, "Jahrgang", 0)
						data.FerProzentSatz = SafeGetDecimal(reader, "FerProzentSatz", 0)
						data.FeierProzentSatz = SafeGetDecimal(reader, "FeierProzentSatz", 0)
						data.Prozent13Satz = SafeGetDecimal(reader, "13ProzentSatz", 0)

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

		Function AddFF13LohnData(ByVal data As FF13LohnData) As Boolean Implements ITablesDatabaseAccess.AddFF13LohnData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.[TabFerien/Feier/13] (Jahrgang, FerProzentSatz, FeierProzentSatz, [13ProzentSatz]) Values ("
			sql &= "@Jahrgang, @FerProzentSatz, @FeierProzentSatz, @Prozent13Satz)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Jahrgang", ReplaceMissing(data.Jahrgang, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FerProzentSatz", ReplaceMissing(data.FerProzentSatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@FeierProzentSatz", ReplaceMissing(data.FeierProzentSatz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Prozent13Satz", ReplaceMissing(data.Prozent13Satz, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateFF13LohnData(ByVal data As FF13LohnData) As Boolean Implements ITablesDatabaseAccess.UpdateFF13LohnData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.[TabFerien/Feier/13] Set "
			sql &= "Jahrgang = @Jahrgang "
			sql &= ", FerProzentSatz = @FerProzentSatz "
			sql &= ", FeierProzentSatz = @FeierProzentSatz "
			sql &= ", [13ProzentSatz] = @Prozent13Satz "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Jahrgang", ReplaceMissing(data.Jahrgang, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@FerProzentSatz", ReplaceMissing(data.FerProzentSatz, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@FeierProzentSatz", ReplaceMissing(data.FeierProzentSatz, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Prozent13Satz", ReplaceMissing(data.Prozent13Satz, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteFF13LohnData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteFF13LohnData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.[TabFerien/Feier/13] "
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


#Region "common tables: qstdata: Tab_QSTInfo"

		Function LoadQSTInfo() As IEnumerable(Of QSTInfoData) Implements ITablesDatabaseAccess.LoadQSTInfo

			Dim result As List(Of QSTInfoData) = Nothing

			Dim sql As String

			sql = "SELECT ID, SKanton, Convert(Int, MonthStd) MonthStd, "
			sql &= "Convert(Bit, StdDown) StdDown, "
			sql &= "Convert(Bit, StdUp) StdUp, "
			sql &= "Convert(Bit, DESameAsCH) DESameAsCH, "
			sql &= "Convert(Bit, JustAtEndBegin) JustAtEndBegin, "
			sql &= "Convert(Bit, CalendarDay) CalendarDay, "
			sql &= "Convert(Bit, WithFLeistung) WithFLeistung, "
			sql &= "Convert(Bit, HandleAsAutomation) HandleAsAutomation, "
			sql &= "Convert(Bit, StdDownAtEndBegin) StdDownAtEndBegin "

			sql &= "From Tab_QSTInfo "
			sql &= "Order By SKanton"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of QSTInfoData)

					While reader.Read

						Dim data = New QSTInfoData()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.SKanton = SafeGetString(reader, "SKanton")
						data.MonthStd = SafeGetInteger(reader, "MonthStd", 180)
						data.StdDown = SafeGetBoolean(reader, "StdDown", False)
						data.StdUp = SafeGetBoolean(reader, "StdUp", True)
						data.DESameAsCH = SafeGetBoolean(reader, "DESameAsCH", True)
						data.JustAtEndBegin = SafeGetBoolean(reader, "JustAtEndBegin", True)
						data.CalendarDay = SafeGetBoolean(reader, "CalendarDay", False)
						data.WithFLeistung = SafeGetBoolean(reader, "WithFLeistung", False)
						data.HandleAsAutomation = SafeGetBoolean(reader, "HandleAsAutomation", True)
						data.StdDownAtEndBegin = SafeGetBoolean(reader, "StdDownAtEndBegin", True)

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

		Function AddQSTInfoData(ByVal data As QstInfoData) As Boolean Implements ITablesDatabaseAccess.AddQSTInfoData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Tab_QSTInfo ("
			sql &= "SKanton "
			sql &= ", MonthStd "
			sql &= ", StdDown "
			sql &= ", StdUp "
			sql &= ", DESameAsCH "
			sql &= ", JustAtEndBegin "
			sql &= ", CalendarDay "
			sql &= ", WithFLeistung"
			sql &= ", HandleAsAutomation"
			sql &= ", StdDownAtEndBegin"
			sql &= ") "
			sql &= "Values ("
			sql &= "@SKanton "
			sql &= ", @MonthStd "
			sql &= ", @StdDown "
			sql &= ", @StdUp "
			sql &= ", @DESameAsCH "
			sql &= ", @JustAtEndBegin "
			sql &= ", @CalendarDay "
			sql &= ", @WithFLeistung"
			sql &= ", @HandleAsAutomation"
			sql &= ", @StdDownAtEndBegin"
			sql &= ")"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@SKanton", ReplaceMissing(data.SKanton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MonthStd", ReplaceMissing(data.MonthStd, 180)))

			listOfParams.Add(New SqlClient.SqlParameter("@StdDown", ReplaceMissing(data.StdDown, False)))
			listOfParams.Add(New SqlClient.SqlParameter("@StdUp", ReplaceMissing(data.StdUp, True)))
			listOfParams.Add(New SqlClient.SqlParameter("@DESameAsCH", ReplaceMissing(data.DESameAsCH, False)))
			listOfParams.Add(New SqlClient.SqlParameter("@JustAtEndBegin", ReplaceMissing(data.JustAtEndBegin, True)))
			listOfParams.Add(New SqlClient.SqlParameter("@CalendarDay", ReplaceMissing(data.CalendarDay, False)))
			listOfParams.Add(New SqlClient.SqlParameter("@WithFLeistung", ReplaceMissing(data.WithFLeistung, False)))
			listOfParams.Add(New SqlClient.SqlParameter("@HandleAsAutomation", ReplaceMissing(data.HandleAsAutomation, False)))
			listOfParams.Add(New SqlClient.SqlParameter("@StdDownAtEndBegin", ReplaceMissing(data.StdDownAtEndBegin, True)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateQSTInfoData(ByVal data As QstInfoData) As Boolean Implements ITablesDatabaseAccess.UpdateQSTInfoData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_QSTInfo Set "
			sql &= "SKanton = @SKanton "
			sql &= ", MonthStd = @MonthStd "
			sql &= ", StdDown = @StdDown "
			sql &= ", StdUp = @StdUp "
			sql &= ", DESameAsCH = @DESameAsCH "
			sql &= ", JustAtEndBegin = @JustAtEndBegin "
			sql &= ", CalendarDay = @CalendarDay "
			sql &= ", WithFLeistung = @WithFLeistung "
			sql &= ", HandleAsAutomation = @HandleAsAutomation "
			sql &= ", StdDownAtEndBegin = @StdDownAtEndBegin "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@SKanton", ReplaceMissing(data.SKanton, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@MonthStd", ReplaceMissing(data.MonthStd, 180)))

				listOfParams.Add(New SqlClient.SqlParameter("@StdDown", ReplaceMissing(data.StdDown, False)))
				listOfParams.Add(New SqlClient.SqlParameter("@StdUp", ReplaceMissing(data.StdUp, True)))
				listOfParams.Add(New SqlClient.SqlParameter("@DESameAsCH", ReplaceMissing(data.DESameAsCH, False)))
				listOfParams.Add(New SqlClient.SqlParameter("@JustAtEndBegin", ReplaceMissing(data.JustAtEndBegin, True)))
				listOfParams.Add(New SqlClient.SqlParameter("@CalendarDay", ReplaceMissing(data.CalendarDay, False)))
				listOfParams.Add(New SqlClient.SqlParameter("@WithFLeistung", ReplaceMissing(data.WithFLeistung, False)))
				listOfParams.Add(New SqlClient.SqlParameter("@HandleAsAutomation", ReplaceMissing(data.HandleAsAutomation, False)))
				listOfParams.Add(New SqlClient.SqlParameter("@StdDownAtEndBegin", ReplaceMissing(data.StdDownAtEndBegin, True)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteQSTInfoData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteQSTInfoData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_QSTInfo "
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



#Region "common tables: branchs: Branchen"

		Function LoadSectorData() As IEnumerable(Of SectorData) Implements ITablesDatabaseAccess.LoadSectorData

			Dim result As List(Of SectorData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Convert(Int, Code) As Bez_Value, "
			sql &= "Branche As Bezeichnung, "
			sql &= "[BranchenBezeichnung D] Bez_D, "
			sql &= "[BranchenBezeichnung I] Bez_I, "
			sql &= "[BranchenBezeichnung F] Bez_F, "
			sql &= "[BranchenBezeichnung E] Bez_E, "
			sql &= "CreatedFrom, "
			sql &= "CreatedOn "
			sql &= "From Branchen "
			sql &= "Order By Branche"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of SectorData)

					While reader.Read

						Dim data = New SectorData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.code = SafeGetInteger(reader, "Bez_Value", 0)
						data.branche = SafeGetString(reader, "Bezeichnung")
						data.bez_d = SafeGetString(reader, "Bez_D")
						data.bez_i = SafeGetString(reader, "Bez_I")
						data.bez_f = SafeGetString(reader, "Bez_F")
						data.bez_e = SafeGetString(reader, "Bez_E")
						data.createdon = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.createdfrom = SafeGetString(reader, "CreatedFrom")

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

		Function AddSectorData(ByVal data As SectorData) As Boolean Implements ITablesDatabaseAccess.AddSectorData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.Branchen ("
			sql &= "Code, "
			sql &= "Branche, "
			sql &= "[BranchenBezeichnung D], "
			sql &= "[BranchenBezeichnung I], "
			sql &= "[BranchenBezeichnung F], "
			sql &= "[BranchenBezeichnung E], "
			sql &= "CreatedFrom, "
			sql &= "CreatedOn "
			sql &= ") Values ("

			sql &= "ISNULL((SELECT TOP 1 ID FROM Branchen WHERE ID + 1 NOT IN (SELECT Code FROM Branchen) ORDER BY ID DESC), 0) + 1, "
			sql &= "@Bezeichnung, "
			sql &= "@Bez_D, "
			sql &= "@Bez_I, "
			sql &= "@Bez_F, "
			sql &= "@Bez_E, "
			sql &= "@CreatedFrom, "
			sql &= "getdate()"

			sql &= ")"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(data.branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_D", ReplaceMissing(data.bez_d, data.branche)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, data.branche)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, data.branche)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, data.branche)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateSectorData(ByVal data As SectorData) As Boolean Implements ITablesDatabaseAccess.UpdateSectorData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Branche From Branchen Where ID = @ID), ''); "

			sql &= "Update dbo.Branchen Set "
			sql &= "Code = @Bez_Value, "
			sql &= "Branche = @Bezeichnung, "
			sql &= "[BranchenBezeichnung D] = @Bez_D, "
			sql &= "[BranchenBezeichnung I] = @Bez_I, "
			sql &= "[BranchenBezeichnung F] = @Bez_F, "
			sql &= "[BranchenBezeichnung E] = @Bez_E, "
			sql &= "CreatedFrom = @CreatedFrom, "
			sql &= "CreatedOn = GetDate() "

			sql &= "Where ID = @ID; "

			sql &= "Update KD_Branche Set Bezeichnung = @Bezeichnung, BranchenCode = @Bez_Value Where Bezeichnung = @OldBez; "
			sql &= "Update KD_ZBranche Set Bezeichnung = @Bezeichnung, BranchenCode = @Bez_Value Where Bezeichnung = @OldBez; "
			sql &= "Update MA_Branche Set Bezeichnung = @Bezeichnung, BranchenCode = @Bez_Value Where Bezeichnung = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Bez_Value", ReplaceMissing(data.code, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(data.branche, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_D", ReplaceMissing(data.bez_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteSectorData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteSectorData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Branchen "
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



#Region "common tables: Qualifications: job"

		Function LoadJobData() As IEnumerable(Of JobData) Implements ITablesDatabaseAccess.LoadJobData

			Dim result As List(Of JobData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Code, Beruf, "
			sql &= "[BerufsBezeichnung D M] As Beruf_D_M, [BerufsBezeichnung D W] As Beruf_D_W, "
			sql &= "[BerufsBezeichnung I M] As Beruf_I_M, [BerufsBezeichnung I W] As Beruf_I_W, "
			sql &= "[BerufsBezeichnung F M] As Beruf_F_M, [BerufsBezeichnung F W] As Beruf_F_W, "
			sql &= "[BerufsBezeichnung E M] As Beruf_E_M, [BerufsBezeichnung E W] As Beruf_E_W, "
			sql &= "[Fachrichtung D] As Fach_D, [Fachrichtung I] As Fach_I, [Fachrichtung F] As Fach_F "
			sql &= "From dbo.job Order By Beruf"


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of JobData)

					While reader.Read

						Dim data = New JobData()
						data.recId = SafeGetInteger(reader, "ID", 0)
						data.code = SafeGetInteger(reader, "code", 0)
						data.beruf = SafeGetString(reader, "Beruf")

						data.beruf_d_m = SafeGetString(reader, "Beruf_D_M")
						data.beruf_d_w = SafeGetString(reader, "Beruf_D_W")
						data.beruf_i_m = SafeGetString(reader, "Beruf_I_M")
						data.beruf_i_w = SafeGetString(reader, "Beruf_I_W")
						data.beruf_f_m = SafeGetString(reader, "Beruf_F_M")
						data.beruf_f_w = SafeGetString(reader, "Beruf_F_W")
						data.beruf_e_m = SafeGetString(reader, "Beruf_E_M")
						data.beruf_e_w = SafeGetString(reader, "Beruf_E_W")
						data.fach_d = SafeGetString(reader, "Fach_D")
						data.fach_i = SafeGetString(reader, "Fach_I")
						data.fach_f = SafeGetString(reader, "Fach_F")


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

		Function AddJobData(ByVal data As JobData) As Boolean Implements ITablesDatabaseAccess.AddJobData

			Dim success = True

			Dim sql As String
			sql = "IF NOT Exists(Select Top (1) Code From dbo.Job Where Convert(INT, Job.Code) = @Code) "
			sql &= "Begin "
			sql &= "Insert Into Job ("
			sql &= "Code, Beruf, "
			sql &= "[BerufsBezeichnung D M], [BerufsBezeichnung D W], "
			sql &= "[BerufsBezeichnung I M], [BerufsBezeichnung I W], "
			sql &= "[BerufsBezeichnung F M], [BerufsBezeichnung F W], "
			sql &= "[BerufsBezeichnung E M], [BerufsBezeichnung E W], "
			sql &= "[Fachrichtung D], [Fachrichtung I], [Fachrichtung F], Createdon, CreatedFrom) "
			sql &= "Values ("
			sql &= "IsNull(@Code, ISNULL((SELECT TOP 1 ID FROM job WHERE ID + 1 NOT IN (SELECT Code FROM Job) ORDER BY ID DESC), 0) + 1), "
			sql &= "@Beruf, "
			sql &= "@Beruf_D_M, @Beruf_D_W, "
			sql &= "@Beruf_I_M, @Beruf_I_W, "
			sql &= "@Beruf_F_M, @Beruf_F_W, "
			sql &= "@Beruf_E_M, @Beruf_E_W, "
			sql &= "@Fach_D, @Fach_I, @Fach_F, getDate(), @CreatedFrom) "
			sql &= "End;"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@beruf", ReplaceMissing(data.beruf, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_D_M", ReplaceMissing(data.beruf_d_m, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_D_W", ReplaceMissing(data.beruf_d_w, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_I_M", ReplaceMissing(data.beruf_i_m, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_I_W", ReplaceMissing(data.beruf_i_w, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_F_M", ReplaceMissing(data.beruf_f_m, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_F_W", ReplaceMissing(data.beruf_f_w, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_E_M", ReplaceMissing(data.beruf_e_m, data.beruf)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beruf_E_W", ReplaceMissing(data.beruf_e_w, data.beruf)))

			listOfParams.Add(New SqlClient.SqlParameter("@Fach_D", ReplaceMissing(data.Fach_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fach_I", ReplaceMissing(data.fach_i, DBNull.Value))) ' data.fach_d)))
			listOfParams.Add(New SqlClient.SqlParameter("@Fach_F", ReplaceMissing(data.fach_f, DBNull.Value))) ' data.fach_d)))

			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, True)

			Return success

		End Function

		Function UpdateJobData(ByVal data As JobData) As Boolean Implements ITablesDatabaseAccess.UpdateJobData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Beruf From Job Where Code = @Code), ''); "

			sql &= "Update dbo.Job Set "
			sql &= "Code = @Code, "
			sql &= "Beruf = @Beruf, "
			sql &= "[BerufsBezeichnung D M] = @Beruf_D_M, "
			sql &= "[BerufsBezeichnung I M] = @Beruf_I_M, "
			sql &= "[BerufsBezeichnung F M] = @Beruf_F_M, "
			sql &= "[BerufsBezeichnung E M] = @Beruf_E_M, "

			sql &= "[BerufsBezeichnung D w] = @Beruf_D_w, "
			sql &= "[BerufsBezeichnung I w] = @Beruf_I_w, "
			sql &= "[BerufsBezeichnung F w] = @Beruf_F_w, "
			sql &= "[BerufsBezeichnung E w] = @Beruf_E_w, "

			sql &= "[Fachrichtung d] = @fach_d,"
			sql &= "[Fachrichtung i] = @fach_i,"
			sql &= "[Fachrichtung f] = @fach_f "

			sql &= "Where ID = @ID; "

			sql &= "Update KD_Berufe Set Bezeichnung = @Beruf, BerufCode = @Code Where Bezeichnung = @OldBez; "
			sql &= "Update KD_ZBerufe Set Bezeichnung = @Beruf, BerufCode = @Code Where Bezeichnung = @OldBez; "
			sql &= "Update MA_ES_Als Set BerufsText = @Beruf, BerufCode = @Code Where BerufsText = @OldBez; "
			sql &= "Update Mitarbeiter Set Beruf = @Beruf, BerufCode = @Code Where Beruf = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recId, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@code", ReplaceMissing(data.code, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@beruf", ReplaceMissing(data.beruf, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_D_M", ReplaceMissing(data.beruf_d_m, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_D_W", ReplaceMissing(data.beruf_d_w, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_I_M", ReplaceMissing(data.beruf_i_m, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_I_W", ReplaceMissing(data.beruf_i_w, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_F_M", ReplaceMissing(data.beruf_f_m, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_F_W", ReplaceMissing(data.beruf_f_w, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_E_M", ReplaceMissing(data.beruf_e_m, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Beruf_E_W", ReplaceMissing(data.beruf_e_w, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@Fach_D", ReplaceMissing(data.fach_d, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Fach_I", ReplaceMissing(data.fach_i, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Fach_F", ReplaceMissing(data.fach_f, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteJobData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteJobData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Job "
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




#Region "common table: country: LND"

		''' <summary>
		''' Loads sms template data.
		''' </summary>
		Function LoadCountryData() As IEnumerable(Of CountryData) Implements ITablesDatabaseAccess.LoadCountryData

			Dim result As List(Of CountryData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Code, Land AS Bez_Value, Bez_d, Bez_I, Bez_F, Bez_E FROM lnd ORDER BY land"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CountryData)

					While reader.Read

						Dim data = New CountryData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.code = SafeGetString(reader, "code")
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

		Function AddCountryData(ByVal data As CountryData) As Boolean Implements ITablesDatabaseAccess.AddCountryData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.lnd (Code, Land, bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@code"
			sql &= ", @bez_value"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@code", ReplaceMissing(data.code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateCountryData(ByVal data As CountryData) As Boolean Implements ITablesDatabaseAccess.UpdateCountryData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez nvarchar(255); "
			sql &= "Set @OldBez = IsNull((Select Top 1 Code From lnd Where ID = @ID), ''); "

			sql &= "Update dbo.lnd Set Code = @Code, "
			sql &= "Land = @bez_value, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID; "

			sql &= "Update Mitarbeiter Set Land = @Code Where LAND = @OldBez; "
			sql &= "Update Mitarbeiter Set Nationality = @Code Where Nationality = @OldBez; "
			sql &= "Update Kunden Set Land = @Code Where LAND = @OldBez; "
			sql &= "Update KD_Zustaendig Set Land = @Code Where LAND = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Code", ReplaceMissing(data.code, DBNull.Value)))
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

		Function DeleteCountryData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteCountryData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.LND "
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


#Region "common tables: FIBU Konten"

		Function LoadFIBUKontenData(ByVal language As String) As IEnumerable(Of FIBUData) Implements ITablesDatabaseAccess.LoadFIBUKontenData

			Dim result As List(Of FIBUData) = Nothing

			Dim sql As String
			Dim myLanguage As String = ReplaceMissing(language, "D")
			Dim selField As String = myLanguage

			sql = "SELECT ID"
			sql &= ", Convert(Int, KontoNr) KontoNr"
			sql &= ", KontoName"
			sql &= ", Bez_D"
			sql &= ", Bez_I"
			sql &= ", Bez_F"
			sql &= ", Bez_E "

			Select Case myLanguage.ToLower().TrimEnd()
				Case "deutsch", "de", "d"
					selField = "Bez_D"
				Case "italienisch", "it", "i"
					selField = "Bez_I"
				Case "französisch", "fr", "f"
					selField = "Bez_F"
				Case "englisch", "en", "e"
					selField = "Bez_E"

				Case Else
					selField = "Bez_D"
			End Select
			sql &= String.Format(",{0} TranslatedBez", selField)


			sql &= " From dbo.FBK "
			sql &= "Order By Convert(Int, KontoNr)"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of FIBUData)

					While reader.Read

						Dim data = New FIBUData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.KontoNr = SafeGetInteger(reader, "KontoNr", 0)
						data.KontoName = SafeGetString(reader, "KontoName")
						data.bez_d = SafeGetString(reader, "Bez_d")
						data.bez_i = SafeGetString(reader, "Bez_i")
						data.bez_f = SafeGetString(reader, "Bez_f")
						data.bez_e = SafeGetString(reader, "Bez_e")

						data.TranslatedLabel = SafeGetString(reader, "TranslatedBez")

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

		Function AddFIBUKontenData(ByVal data As FIBUData) As Boolean Implements ITablesDatabaseAccess.AddFIBUKontenData

			Dim success = True

			Dim sql As String
			sql = "Insert Into dbo.FBK (KontoNr, KontoName, Bez_D, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@KontoNr"
			sql &= ", @KontoName"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KontoNr", ReplaceMissing(data.KontoNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontoName", ReplaceMissing(data.KontoName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_D", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateFIBUKontenData(ByVal data As FIBUData) As Boolean Implements ITablesDatabaseAccess.UpdateFIBUKontenData

			Dim success = True

			Dim sql As String

			sql = "Declare @OldBez int; "
			sql &= "Set @OldBez = IsNull((Select Top 1 KontoNr From FBK Where ID = @recid), 0); "


			sql &= "Update dbo.FBK Set KontoNr = @KontoNr, "
			sql &= "KontoName = @KontoName, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @recid; "

			sql &= "Update LA Set SKonto = @KontoNr Where SKonto = @OldBez; "
			sql &= "Update LA Set HKonto = @KontoNr Where HKonto = @OldBez; "

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@recid", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@KontoNr", ReplaceMissing(data.KontoNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@KontoName", ReplaceMissing(data.KontoName, DBNull.Value)))
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

		Function DeleteFIBUKontenData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteFIBUKontenData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.FBK "
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


#Region "report table - absence code: Fehlcode"

		''' <summary>
		''' Loads sms template data.
		''' </summary>
		''' <returns>List of sms template data.</returns>
		Function LoadAbsenceData() As IEnumerable(Of AbsenceData) Implements ITablesDatabaseAccess.LoadAbsenceData

			Dim result As List(Of AbsenceData) = Nothing

			Dim sql As String

			sql = "SELECT ID, GetFeld As bez_value, [Description], Bez_d, Bez_I, Bez_F, Bez_E FROM dbo.Tab_Fehlzeit Order By GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of AbsenceData)

					While reader.Read

						Dim data = New AbsenceData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.bez_value = SafeGetString(reader, "bez_value")
						data.Description = SafeGetString(reader, "Description")
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

		Function AddAbsenceData(ByVal data As AbsenceData) As Boolean Implements ITablesDatabaseAccess.AddAbsenceData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_Fehlzeit (GetFeld, [Description], Bez_d, Bez_I, Bez_F, Bez_E) Values ("
			sql &= "@bez_value"
			sql &= ", @Description"
			sql &= ", @Bez_d"
			sql &= ", @Bez_I"
			sql &= ", @Bez_F"
			sql &= ", @Bez_E)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Description", ReplaceMissing(data.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_d", ReplaceMissing(data.bez_d, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_I", ReplaceMissing(data.bez_i, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_F", ReplaceMissing(data.bez_f, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bez_E", ReplaceMissing(data.bez_e, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateAbsenceData(ByVal data As AbsenceData) As Boolean Implements ITablesDatabaseAccess.UpdateAbsenceData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_Fehlzeit Set GetFeld = @bez_value, "
			sql &= "[Description] = @Description, "
			sql &= "Bez_d = @Bez_d, "
			sql &= "Bez_I = @Bez_I, "
			sql &= "Bez_F = @Bez_F, "
			sql &= "Bez_E = @Bez_E "
			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@bez_value", ReplaceMissing(data.bez_value, DBNull.Value)))
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

		Function DeleteAbsenceData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteAbsenceData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_Fehlzeit "
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

