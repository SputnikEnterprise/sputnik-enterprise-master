
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting


	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


		Function LoadPrintTemplatesData() As IEnumerable(Of PrintTemplatesData) Implements ITablesDatabaseAccess.LoadPrintTemplatesData

			Dim result As List(Of PrintTemplatesData) = Nothing

			Dim sql As String

			sql = "Select ID, RecNr, SecNr, DocNr, DocFullName, MakroName, MenuLabel, ItemShowIn, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom "
			sql &= "From Tab_TemplateMenu order by itemshowin, RecNr, ID"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PrintTemplatesData)

					While reader.Read

						Dim data = New PrintTemplatesData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.recnr = SafeGetInteger(reader, "recnr", 0)
						data.secnr = SafeGetInteger(reader, "secnr", 0)
						data.docnr = SafeGetString(reader, "docnr")

						data.docfullname = SafeGetString(reader, "docfullname")
						data.makroname = SafeGetString(reader, "makroname")
						data.menulabel = SafeGetString(reader, "menulabel")
						data.itemshowin = SafeGetString(reader, "itemshowin")

						data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						data.createdfrom = SafeGetString(reader, "createdfrom")
						data.changedon = SafeGetDateTime(reader, "changedon", Nothing)
						data.changedfrom = SafeGetString(reader, "changedfrom")

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

		Function AddPrintTemplatesData(ByVal data As PrintTemplatesData) As Boolean Implements ITablesDatabaseAccess.AddPrintTemplatesData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.Tab_TemplateMenu (RecNr, SecNr, DocNr, DocFullName, MakroName, MenuLabel, ItemShowIn, CreatedOn, CreatedFrom) Values ("
			sql &= "@RecNr"
			sql &= ", @SecNr"
			sql &= ", @DocNr"
			sql &= ", @DocFullName"
			sql &= ", @MakroName"
			sql &= ", @MenuLabel"
			sql &= ", @itemshowin"
			sql &= ", GetDate()"
			sql &= ", @CreatedFrom)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(data.recnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@SecNr", ReplaceMissing(data.secnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocNr", ReplaceMissing(data.docnr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@DocFullName", ReplaceMissing(data.docfullname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MakroName", ReplaceMissing(data.makroname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@MenuLabel", ReplaceMissing(data.menulabel, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@itemshowin", ReplaceMissing(data.itemshowin, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(data.createdfrom, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdatePrintTemplatesData(ByVal data As PrintTemplatesData) As Boolean Implements ITablesDatabaseAccess.UpdatePrintTemplatesData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.Tab_TemplateMenu Set RecNr = @RecNr, "
			sql &= "SecNr = @SecNr, "
			sql &= "DocNr = @DocNr, "
			sql &= "DocFullName = @DocFullName, "
			sql &= "MakroName = @MakroName, "
			sql &= "MenuLabel = @MenuLabel, "
			sql &= "ItemShowIn = @itemshowin, "
			sql &= "ChangedOn = Getdate(), "
			sql &= "ChangedFrom = @ChangedFrom "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(data.recnr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@SecNr", ReplaceMissing(data.secnr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@DocNr", ReplaceMissing(data.docnr, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@DocFullName", ReplaceMissing(data.docfullname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@MakroName", ReplaceMissing(data.makroname, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("@MenuLabel", ReplaceMissing(data.menulabel, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("@itemshowin", ReplaceMissing(data.itemshowin, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(data.changedfrom, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeletePrintTemplatesData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeletePrintTemplatesData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.Tab_TemplateMenu "
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
		''' Exporttemplate
		''' </summary>
		''' <returns></returns>
		Function LoadExportTemplatesData() As IEnumerable(Of ExportTemplatesData) Implements ITablesDatabaseAccess.LoadExportTemplatesData

			Dim result As List(Of ExportTemplatesData) = Nothing

			Dim sql As String

			sql = "Select ID, RecNr, Modulname, Bezeichnung, DocName, Tooltip, MnuName "
			sql &= "From ExportDb order by ModulName, RecNr, MnuName"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ExportTemplatesData)

					While reader.Read

						Dim data = New ExportTemplatesData()
						data.recid = SafeGetInteger(reader, "ID", 0)
						data.recnr = SafeGetInteger(reader, "recnr", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung", 0)
						data.DocName = SafeGetString(reader, "DocName")

						data.ModulName = SafeGetString(reader, "ModulName")

						data.MnuName = SafeGetString(reader, "MnuName")
						data.Tooltip = SafeGetString(reader, "Tooltip")

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

		Function AddExportTemplatesData(ByVal data As ExportTemplatesData) As Boolean Implements ITablesDatabaseAccess.AddExportTemplatesData

			Dim success = True

			Dim sql As String

			sql = "Insert Into dbo.ExportDb (RecNr, Modulname, Bezeichnung, DocName, Tooltip, MnuName) Values ("
			sql &= "@RecNr"
			sql &= ", @Modulname"
			sql &= ", @Bezeichnung"
			sql &= ", @DocName"
			sql &= ", @Tooltip"
			sql &= ", @MnuName"
			sql &= ")"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(data.recnr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Modulname", ReplaceMissing(data.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(data.Bezeichnung, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@DocName", ReplaceMissing(data.DocName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Tooltip", ReplaceMissing(data.Tooltip, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("@MnuName", ReplaceMissing(data.MnuName, String.Empty)))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		Function UpdateExportTemplatesData(ByVal data As ExportTemplatesData) As Boolean Implements ITablesDatabaseAccess.UpdateExportTemplatesData

			Dim success = True

			Dim sql As String

			sql = "Update dbo.ExportDb Set RecNr = @RecNr, "
			sql &= "Modulname = @Modulname, "
			sql &= "Bezeichnung = @Bezeichnung, "
			sql &= "DocName = @DocName, "
			sql &= "Tooltip = @Tooltip, "
			sql &= "MnuName = @MnuName "

			sql &= "Where ID = @ID"

			Try
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@ID", ReplaceMissing(data.recid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(data.recnr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Modulname", ReplaceMissing(data.ModulName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(data.Bezeichnung, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("@DocName", ReplaceMissing(data.DocName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Tooltip", ReplaceMissing(data.Tooltip, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("@MnuName", ReplaceMissing(data.MnuName, String.Empty)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			End Try

			Return success

		End Function

		Function DeleteExportTemplatesData(ByVal recid As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteExportTemplatesData

			Dim success = True

			Dim sql As String

			sql = "Delete dbo.ExportDb "
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
