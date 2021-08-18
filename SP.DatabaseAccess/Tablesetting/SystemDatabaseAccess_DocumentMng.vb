
Imports SP.DatabaseAccess.TableSetting.DataObjects


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess



		Function LoadMandantDocumentData() As IEnumerable(Of MandantDocumentData) Implements ITablesDatabaseAccess.LoadMandantDocumentData
			Dim result As List(Of MandantDocumentData) = Nothing

			Dim SQL As String

			SQL = "[Show Documents]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MandantDocumentData)

					While reader.Read()
						Dim overviewData As New MandantDocumentData

						overviewData.ID = SafeGetInteger(reader, "ID", 0)
						overviewData.ModulNr = SafeGetInteger(reader, "ModulNr", 0)
						overviewData.RecNr = SafeGetInteger(reader, "RecNr", 0)
						overviewData.jobNr = SafeGetString(reader, "JobNr")
						overviewData.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						overviewData.DokNameToShow = SafeGetString(reader, "DokNameToShow")
						overviewData.WindowsTitle = SafeGetString(reader, "WindowsTitle")

						overviewData.LeftMargin = SafeGetInteger(reader, "LeftMargin", 0)
						overviewData.RightMargin = SafeGetInteger(reader, "RightMargin", 0)
						overviewData.TopMargin = SafeGetInteger(reader, "TopMargin", 0)
						overviewData.BottomMargin = SafeGetInteger(reader, "BottomMargin", 0)
						overviewData.Anzahlkopien = SafeGetInteger(reader, "Anzahlkopien", 0)

						overviewData.DocName = SafeGetString(reader, "DocName")
						overviewData.TempDocPath = SafeGetString(reader, "TempDocPath")
						overviewData.Meldung0 = SafeGetString(reader, "Meldung0")
						overviewData.Meldung1 = SafeGetString(reader, "Meldung1")
						overviewData.ZoomProz = SafeGetInteger(reader, "ZoomProz", 0)

						overviewData.ParamCheck = SafeGetBoolean(reader, "ParamCheck", False)
						overviewData.KonvertName = SafeGetBoolean(reader, "KonvertName", False)
						overviewData.FontDesent = SafeGetBoolean(reader, "FontDesent", False)
						overviewData.IncPrv = SafeGetBoolean(reader, "IncPrv", False)
						overviewData.PrintInDiffColor = SafeGetBoolean(reader, "PrintInDiffColor", False)
						overviewData.InsertFileToDb = SafeGetBoolean(reader, "InsertFileToDb", False)

						overviewData.ExportedFileName = SafeGetString(reader, "ExportedFileName")


						result.Add(overviewData)

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

		Function LoadMandantAssignedDocumentData(ByVal jobNr As String) As MandantDocumentData Implements ITablesDatabaseAccess.LoadMandantAssignedDocumentData
			Dim result As MandantDocumentData = Nothing

			Dim SQL As String

			SQL = "Select "
			SQL &= "ID ,"
			SQL &= "Convert(Int, ModulNr) ModulNr ,"
			SQL &= "RecNr ,"
			SQL &= "JobNr ,"
			SQL &= "Bezeichnung ,"
			SQL &= "DokNameToShow ,"
			SQL &= "WindowsTitle ,"
			SQL &= "LeftMargin ,"
			SQL &= "RightMargin ,"
			SQL &= "TopMargin ,"
			SQL &= "BottomMargin ,"
			SQL &= "Result ,"
			SQL &= "Convert(Int, Anzahlkopien) Anzahlkopien ,"
			SQL &= "DocName ,"
			SQL &= "TempDocPath ,"
			SQL &= "Meldung0 ,"
			SQL &= "Meldung1 ,"
			SQL &= "ZoomProz ,"
			SQL &= "ParamCheck ,"
			SQL &= "KonvertName ,"
			SQL &= "FontDesent ,"
			SQL &= "IncPrv ,"
			SQL &= "PrintInDiffColor ,"
			SQL &= "InsertFileToDb ,"
			SQL &= "ExportedFileName "

			SQL &= "From DokPrint "
			SQL &= "Where JobNr In (@jobNr)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("jobNr", ReplaceMissing(jobNr, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.Text)

			Try

				result = New MandantDocumentData
				If (Not reader Is Nothing AndAlso reader.Read()) Then


					Dim overviewData As New MandantDocumentData

					overviewData.ID = SafeGetInteger(reader, "ID", 0)
					overviewData.ModulNr = SafeGetInteger(reader, "ModulNr", 0)
					overviewData.RecNr = SafeGetInteger(reader, "RecNr", 0)
					overviewData.jobNr = SafeGetString(reader, "JobNr")
					overviewData.Bezeichnung = SafeGetString(reader, "Bezeichnung")
					overviewData.DokNameToShow = SafeGetString(reader, "DokNameToShow")
					overviewData.WindowsTitle = SafeGetString(reader, "WindowsTitle")

					overviewData.LeftMargin = SafeGetInteger(reader, "LeftMargin", 0)
					overviewData.RightMargin = SafeGetInteger(reader, "RightMargin", 0)
					overviewData.TopMargin = SafeGetInteger(reader, "TopMargin", 0)
					overviewData.BottomMargin = SafeGetInteger(reader, "BottomMargin", 0)
					overviewData.Anzahlkopien = SafeGetInteger(reader, "Anzahlkopien", 0)

					overviewData.DocName = SafeGetString(reader, "DocName")
					overviewData.TempDocPath = SafeGetString(reader, "TempDocPath")
					overviewData.Meldung0 = SafeGetString(reader, "Meldung0")
					overviewData.Meldung1 = SafeGetString(reader, "Meldung1")
					overviewData.ZoomProz = SafeGetInteger(reader, "ZoomProz", 0)

					overviewData.ParamCheck = SafeGetBoolean(reader, "ParamCheck", False)
					overviewData.KonvertName = SafeGetBoolean(reader, "KonvertName", False)
					overviewData.FontDesent = SafeGetBoolean(reader, "FontDesent", False)
					overviewData.IncPrv = SafeGetBoolean(reader, "IncPrv", False)
					overviewData.PrintInDiffColor = SafeGetBoolean(reader, "PrintInDiffColor", False)
					overviewData.InsertFileToDb = SafeGetBoolean(reader, "InsertFileToDb", False)

					overviewData.ExportedFileName = SafeGetString(reader, "ExportedFileName")


					result = overviewData

				End If


			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)

			End Try

			Return result
		End Function

		Function UpdateAssignedMandantDocumentData(ByVal data As MandantDocumentData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedMandantDocumentData

			Dim success As Boolean = True

			Dim sql As String

			sql = "UPDATE DokPrint "
			sql &= "SET ModulNr = @modulNr"
			sql &= ", RecNr = @RecNr"
			sql &= ", JobNr = @jobNr"
			sql &= ", AnzahlKopien = @anzKop"
			sql &= ", Bezeichnung = @bez"
			sql &= ", Meldung0 = @meld1"
			sql &= ", Meldung1 = @meld2"
			sql &= ", DocName = @docname"
			sql &= ", TempDocPath = @tempDocPath"
			sql &= ", ZoomProz = @zoom"
			sql &= ", ParamCheck = @paramCheck"
			sql &= ", KonvertName = @konvName"
			sql &= ", FontDesent = @fontDescent"
			sql &= ", IncPrv = @incPrv"
			sql &= ", PrintInDiffColor = @printDiffColor"
			sql &= ", InsertFileToDb = @insertFile"
			sql &= ", ExportedFileName = @ExportedFilename "

			sql &= " Where [ID] = @id "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("id", ReplaceMissing(data.ID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("modulNr", ReplaceMissing(data.ModulNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecNr", ReplaceMissing(data.RecNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jobNr", ReplaceMissing(data.jobNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("anzKop", ReplaceMissing(data.Anzahlkopien, 1)))
			listOfParams.Add(New SqlClient.SqlParameter("bez", ReplaceMissing(data.Bezeichnung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("meld1", ReplaceMissing(data.Meldung0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("meld2", ReplaceMissing(data.Meldung1, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("docname", ReplaceMissing(data.DocName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("tempDocPath", ReplaceMissing(data.TempDocPath, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zoom", ReplaceMissing(data.ZoomProz, 150)))
			listOfParams.Add(New SqlClient.SqlParameter("paramCheck", ReplaceMissing(data.ParamCheck, False)))
			listOfParams.Add(New SqlClient.SqlParameter("konvName", ReplaceMissing(data.KonvertName, False)))
			listOfParams.Add(New SqlClient.SqlParameter("fontDescent", ReplaceMissing(data.FontDesent, False)))
			listOfParams.Add(New SqlClient.SqlParameter("incPrv", ReplaceMissing(data.IncPrv, False)))
			listOfParams.Add(New SqlClient.SqlParameter("printDiffColor", ReplaceMissing(data.PrintInDiffColor, False)))
			listOfParams.Add(New SqlClient.SqlParameter("insertFile", ReplaceMissing(data.InsertFileToDb, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ExportedFilename", ReplaceMissing(data.ExportedFileName, DBNull.Value)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' add document data.
		''' </summary>
		''' <returns>boolean.</returns>
		Function AddAssignedMandantDocumentData(ByVal data As MandantDocumentData) As Boolean Implements ITablesDatabaseAccess.AddAssignedMandantDocumentData

			Dim success As Boolean = True

			Dim sql As String = "[Create New Mandant Document]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("modulNr", ReplaceMissing(data.ModulNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("jobNr", ReplaceMissing(data.jobNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RecNr", ReplaceMissing(data.RecNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("anzKop", ReplaceMissing(data.Anzahlkopien, 1)))
			listOfParams.Add(New SqlClient.SqlParameter("bez", ReplaceMissing(data.Bezeichnung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("meld1", ReplaceMissing(data.Meldung0, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("meld2", ReplaceMissing(data.Meldung1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("docname", ReplaceMissing(data.DocName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("tempDocPath", ReplaceMissing(data.TempDocPath, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("zoom", ReplaceMissing(data.ZoomProz, 150)))
			listOfParams.Add(New SqlClient.SqlParameter("paramCheck", ReplaceMissing(data.ParamCheck, False)))
			listOfParams.Add(New SqlClient.SqlParameter("konvName", ReplaceMissing(data.KonvertName, False)))
			listOfParams.Add(New SqlClient.SqlParameter("fontDescent", ReplaceMissing(data.FontDesent, False)))
			listOfParams.Add(New SqlClient.SqlParameter("incPrv", ReplaceMissing(data.IncPrv, False)))
			listOfParams.Add(New SqlClient.SqlParameter("printDiffColor", ReplaceMissing(data.PrintInDiffColor, False)))
			listOfParams.Add(New SqlClient.SqlParameter("insertFile", ReplaceMissing(data.InsertFileToDb, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ExportedFilename", ReplaceMissing(data.ExportedFileName, DBNull.Value)))


			Try
				' New ID of user
				Dim newIdParameter = New SqlClient.SqlParameter("NewDocId", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success Then
					If Not newIdParameter.Value Is Nothing Then
						data.ID = CType(newIdParameter.Value, Integer)
					End If

				Else
					success = False
				End If

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' delete document data.
		''' </summary>
		''' <returns>boolean.</returns>
		Function DeleteAssignedMandantDocumentData(ByVal id As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteAssignedMandantDocumentData

			Dim success As Boolean = True

			Dim sql As String
			sql = "Delete Dokprint "
			sql &= "Where ID = @ID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(id, DBNull.Value)))

			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

				Return success

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

	End Class


End Namespace
