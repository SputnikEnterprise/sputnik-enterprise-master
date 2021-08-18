Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Namespace Employee

	''' <summary>
	''' Employee database access class.
	''' </summary>
	Public Class EmployeeDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IEmployeeDatabaseAccess

		''' <summary>
		''' Loads employee document category data.
		''' </summary>
		''' <returns>List of employee category data.</returns>
		Public Function LoadEmployeeDocumentCategories() As IEnumerable(Of EmployeeDocumentCategoryData) Implements IEmployeeDatabaseAccess.LoadEmployeeDocumentCategories

			Dim result As List(Of EmployeeDocumentCategoryData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Categorie_Nr, Bez_d, Bez_F, Bez_I FROM Tab_MADocCategories Where IsNull(Categorie_Nr, 0) <> 0  ORDER BY Categorie_Nr ASC"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDocumentCategoryData)

					While reader.Read()
						Dim categoryDataData As New EmployeeDocumentCategoryData
						categoryDataData.ID = SafeGetInteger(reader, "ID", 0)
						categoryDataData.CategoryNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
						categoryDataData.DescriptionGerman = SafeGetString(reader, "bez_d")
						categoryDataData.DescriptionFrench = SafeGetString(reader, "bez_F")
						categoryDataData.DescriptionItalian = SafeGetString(reader, "Bez_I")

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

		''' <summary>
		''' Loads distinct document categories of an employee.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Distinct document category data.</returns>
		Public Function LoadDistinctDocumentCategoriesOfEmployee(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeDocumentCategoryData) Implements IEmployeeDatabaseAccess.LoadDistinctDocumentCategoriesOfEmployee

			Dim result As List(Of EmployeeDocumentCategoryData) = Nothing

			Dim sql As String

			sql = "SELECT * FROM Tab_MADocCategories TabCategories WHERE Categorie_Nr IN "
			sql = sql & "(SELECT DISTINCT Categorie_Nr FROM MA_LLDoc WHERE MANr = @maNumber)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNumber", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDocumentCategoryData)

					While reader.Read()
						Dim categoryDataData As New EmployeeDocumentCategoryData
						categoryDataData.ID = SafeGetInteger(reader, "ID", 0)
						categoryDataData.CategoryNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
						categoryDataData.DescriptionGerman = SafeGetString(reader, "bez_d")
						categoryDataData.DescriptionFrench = SafeGetString(reader, "bez_f")
						categoryDataData.DescriptionItalian = SafeGetString(reader, "bez_i")

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

		''' <summary>
		''' Loads employee documents (MA_LLDoc).
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="documentRecordNumber">The document record number.</param>
		''' <param name="categoryNumber">The category number.</param>
		''' <returns>List employee documents.</returns>
		Public Function LoadEmployeeDocuments(ByVal employeeNumber As Integer, Optional documentRecordNumber As Integer? = Nothing, Optional categoryNumber As Integer? = Nothing) As IEnumerable(Of EmployeeDocumentData) Implements IEmployeeDatabaseAccess.LoadEmployeeDocuments
			Dim result As List(Of EmployeeDocumentData) = Nothing

			Dim sql As String

			sql = "SELECT Doc.ID"
			sql &= ", Doc.MANr"
			sql &= ", Doc.DocPath"
			sql &= ", Doc.RecNr"
			sql &= ", Doc.CreatedOn"
			sql &= ", Doc.CreatedFrom "
			sql &= ", Doc.ChangedOn"
			sql &= ", Doc.ChangedFrom"
			sql &= ", Doc.Bezeichnung"
			sql &= ", Doc.Beschreibung"
			sql &= ", Doc.ScanExtension "
			sql &= ", Doc.USNr"
			sql &= ", Doc.Categorie_Nr"
			sql &= ", Doc.Pages"
			sql &= ", Doc.FileSize"
			sql &= ", Doc.PlainText"
			sql &= ", Doc.DocXML"
			sql &= ", Doc.FileHashvalue"

			sql &= ", Cat.Bez_D "

			sql &= "FROM dbo.MA_LLDoc Doc "
			sql &= "LEFT JOIN Tab_MADocCategories Cat ON Doc.Categorie_Nr = Cat.Categorie_Nr "
			sql &= "WHERE Doc.MANr = @maNumber "
			sql &= "AND (@documentRecordNumber IS NULL OR Doc.RecNr = @documentRecordNumber) "
			sql &= "AND (@categoryNumber IS NULL OR Doc.Categorie_Nr = @categoryNumber) "
			sql &= "Order By CreatedOn DESC"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNumber", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("documentRecordNumber", ReplaceMissing(documentRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("categoryNumber", ReplaceMissing(categoryNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeDocumentData)

					While reader.Read()
						Dim documentData As New EmployeeDocumentData
						documentData.ID = SafeGetInteger(reader, "ID", 0)
						documentData.DocumentRecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
						documentData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						documentData.Name = SafeGetString(reader, "Bezeichnung")
						documentData.Description = SafeGetString(reader, "Beschreibung")
						documentData.DocPath = SafeGetString(reader, "DocPath", Nothing)
						documentData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						documentData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						documentData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						documentData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						documentData.ScanExtension = SafeGetString(reader, "ScanExtension")
						documentData.FileFullPath = SafeGetString(reader, "DocPath")
						documentData.USNr = SafeGetInteger(reader, "USNr", Nothing)
						documentData.CategorieNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
						documentData.CategoryName = SafeGetString(reader, "Bez_D")
						documentData.Pages = SafeGetInteger(reader, "Pages", Nothing)
						documentData.FileSize = SafeGetInteger(reader, "FileSize", Nothing)
						documentData.PlainText = SafeGetString(reader, "PlainText")
						documentData.DocXML = SafeGetString(reader, "DocXML")
						documentData.FileHashvalue = SafeGetString(reader, "FileHashvalue")

						result.Add(documentData)

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

		''' <summary>
		'''  Loads the employee document bytes data (MA_LLDoc).
		''' </summary>
		''' <param name="documentId">The document id.</param>
		''' <returns>The bytes of the document.</returns>
		Function LoadEmployeeDocumentBytesData(ByVal documentId As Integer) As Byte() Implements IEmployeeDatabaseAccess.LoadEmployeeDocumentBytesData
			Dim result As Byte() = Nothing

			Dim sql As String

			sql = "SELECT DocScan FROM MA_LLDoc WHERE ID = @id"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("id", documentId))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetByteArray(reader, "DocScan")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		'''  Loads the employee document bytes data (MA_Printed_Docs).
		''' </summary>
		''' <param name="documentId">The document id.</param>
		''' <returns>The bytes of the document.</returns>
		Function LoadEmployeePrintedDocumentBytesData(ByVal documentId As Integer) As Byte() Implements IEmployeeDatabaseAccess.LoadEmployeePrintedDocumentBytesData
			Dim result As Byte() = Nothing

			Dim sql As String

			sql = "SELECT ScanDoc FROM MA_Printed_Docs WHERE ID = @id"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("id", documentId))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetByteArray(reader, "ScanDoc")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


		''' <summary>
		''' Adds a employee document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddEmployeeDocument(ByVal documentData As EmployeeDocumentData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New MA_LLDoc]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(documentData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DocPath", ReplaceMissing(documentData.DocPath, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beschreibung", ReplaceMissing(documentData.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(documentData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(documentData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(documentData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(documentData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(documentData.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ScanExtension", ReplaceMissing(documentData.ScanExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@UsNr", ReplaceMissing(documentData.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Categorie_Nr", ReplaceMissing(documentData.CategorieNumber, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso
					Not recNrParameter.Value Is Nothing Then
				documentData.ID = CType(newIdParameter.Value, Integer)
				documentData.DocumentRecordNumber = CType(recNrParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Updates an employee document.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		Public Function UpdateEmployeedDocument(ByVal documentData As EmployeeDocumentData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeedDocument

			Dim success = True

			Dim sql As String

			sql = "UPDATE MA_LLDoc SET "
			sql = sql & "MANr = @maNumber "
			sql = sql & ",DocPath = @docPath "
			sql = sql & ",Beschreibung = @description "
			sql = sql & ",RecNr = @documentRecordNumber "
			sql = sql & ",CreatedOn = @createdOn "
			sql = sql & ",CreatedFrom = @createdFrom "
			sql = sql & ",ChangedOn = @changedOn "
			sql = sql & ",ChangedFrom = @changedFrom "
			sql = sql & ",Bezeichnung = @name "
			sql = sql & ",ScanExtension = @scanExtension "
			sql = sql & ",UsNr = @usnr "
			sql = sql & ",Categorie_Nr = @categoryNumber "
			sql = sql & ",Pages = @Pages "
			sql = sql & ",FileSize = @FileSize "
			sql = sql & ",PlainText = @PlainText "
			sql = sql & ",DocXML = @DocXML "
			sql = sql & ",FileHashvalue = @FileHashvalue "

			sql = sql & "WHERE ID = @id "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNumber", ReplaceMissing(documentData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("docPath", ReplaceMissing(documentData.DocPath, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(documentData.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("documentRecordNumber", ReplaceMissing(documentData.DocumentRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(documentData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(documentData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("changedOn", ReplaceMissing(documentData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(documentData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("name", ReplaceMissing(documentData.Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("scanExtension", ReplaceMissing(documentData.ScanExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(documentData.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("categoryNumber", ReplaceMissing(documentData.CategorieNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Pages", ReplaceMissing(documentData.Pages, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileSize", ReplaceMissing(documentData.FileSize, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PlainText", ReplaceMissing(documentData.PlainText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocXML", ReplaceMissing(documentData.DocXML, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileHashvalue", ReplaceMissing(documentData.FileHashvalue, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("id", documentData.ID))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Updates employee document byte data.
		''' </summary>
		''' <param name="documentId">The document id.</param>
		''' <param name="filebytes">The file bytes.</param>
		''' <param name="fileExtension">The file extension.</param>
		''' <returns>Boolean value indicating success.</returns>
		''' <remarks></remarks>
		Function UpdateEmployeeDocumentByteData(ByVal documentId As Integer, ByVal filebytes() As Byte, ByVal fileExtension As String) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeDocumentByteData

			Dim success = True

			Dim sql As String

			sql = "UPDATE MA_LLDoc SET "
			sql = sql & "DocScan = @fileBytes, "
			sql = sql & "ScanExtension = @scanExtension "
			sql = sql & "WHERE ID = @id "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(filebytes, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("scanExtension", ReplaceMissing(fileExtension, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("id", documentId))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' Deletes a employee document.
		''' </summary>
		''' <param name="id">The database id of the document.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function DeleteEmployeeDocument(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeDocument
			Dim success = True

			Dim sql As String

			sql = "DELETE FROM MA_LLDoc WHERE ID = @id"

			Dim idParameter As New SqlClient.SqlParameter("id", id)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(idParameter)

			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function

		''' <summary>
		''' gets employee cv data
		''' </summary>
		''' <param name="employeeNumber"></param>
		''' <param name="documentRecordNumber"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function LoadEmployeeCV(ByVal employeeNumber As Integer, Optional documentRecordNumber As Integer? = Nothing) As IEnumerable(Of EmployeeCVData) Implements IEmployeeDatabaseAccess.LoadEmployeeCV
			Dim result As List(Of EmployeeCVData) = Nothing

			Dim sql As String = String.Empty

			sql = "SELECT ID, MANr, LL_Name, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom FROM MA_Lebenslauf "
			sql &= "WHERE MANr = @maNumber And LL_Name Is Not Null And Reserve0 Is Not Null "
			sql &= "Order By LL_Name ASC"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNumber", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeCVData)

					While reader.Read()
						Dim documentData As New EmployeeCVData
						documentData.ID = SafeGetInteger(reader, "ID", 0)
						documentData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						documentData.Name = SafeGetString(reader, "LL_Name")

						documentData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						documentData.CreatedFrom = SafeGetString(reader, "createdFrom")
						documentData.ChangedOn = SafeGetDateTime(reader, "changedon", Nothing)
						documentData.ChangedFrom = SafeGetString(reader, "changedfrom")

						result.Add(documentData)

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

		Function LoadAssingedEmployeeCVData(ByVal employeeNumber As Integer, ByVal templateName As String, ByVal withBinary As Boolean) As EmployeeCVData Implements IEmployeeDatabaseAccess.LoadAssingedEmployeeCVData
			Dim result As EmployeeCVData = Nothing

			Dim sql As String = String.Empty

			sql = "SELECT ID, MANr, LL_Name, "
			If withBinary Then sql &= "Reserve0, _Reserve0, PDFFile, DocumentContent, "
			sql &= "CreatedOn, CreatedFrom, ChangedOn, ChangedFrom "
			sql &= "FROM MA_Lebenslauf "
			sql &= "WHERE MANr = @maNumber "
			sql &= "And LL_Name = @templateName "
			'sql &= "And Reserve0 Is Not Null "
			sql &= "Order By LL_Name ASC"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("maNumber", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("templateName", ReplaceMissing(templateName, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New EmployeeCVData

					While reader.Read()
						Dim documentData As New EmployeeCVData

						documentData.ID = SafeGetInteger(reader, "ID", 0)
						documentData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						documentData.Name = SafeGetString(reader, "LL_Name")

						If withBinary Then
							documentData.ReserveTextContent = SafeGetString(reader, "Reserve0")
							documentData.ReserveRTFContent = SafeGetString(reader, "_Reserve0")

							documentData.PDFContent = SafeGetByteArray(reader, "PDFFile")
							documentData.DocumentContent = SafeGetByteArray(reader, "DocumentContent")
						End If

						documentData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						documentData.CreatedFrom = SafeGetString(reader, "createdFrom")
						documentData.ChangedOn = SafeGetDateTime(reader, "changedon", Nothing)
						documentData.ChangedFrom = SafeGetString(reader, "changedfrom")

						result = documentData

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

		Function DeleteAssignedEmployeeCVDocument(ByVal id As Integer, ByVal createdUserNumber As Integer, ByVal userName As String) As Boolean Implements IEmployeeDatabaseAccess.DeleteAssignedEmployeeCVDocument
			Dim success = True

			Dim sql As String

			sql = "[DELETE Assigned Employee CV Document]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("id", ReplaceMissing(id, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedUserNumber", ReplaceMissing(createdUserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DeletedUserName", ReplaceMissing(userName, DBNull.Value)))

			Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not resultParameter Is Nothing AndAlso resultParameter.Value = 2 Then success = True Else success = False


			Return success

		End Function

		Function LoadLLZusatzFieldsData(ByVal dbFieldName As String, ByVal showInMAVersand As Boolean, ByVal showInProposeNavBar As Boolean) As IEnumerable(Of LLZusatzFieldsData) Implements IEmployeeDatabaseAccess.LoadLLZusatzFieldsData
			Dim result As List(Of LLZusatzFieldsData) = Nothing

			Dim sql As String
			sql = "Select * "
			sql &= "From tab_LLZusatzFields "
			sql &= "Where "
			sql &= "(@ShowInMAVersand = 0 Or ShowInMAVersand = @ShowInMAVersand) "
			sql &= "AND (@ShowInProposeNavBar = 0 Or ShowInProposeNavBar = @ShowInProposeNavBar) "
			sql &= "AND (@DBFieldName = '' Or DBFieldName Like @DBFieldName) "

			sql &= "Order By RecNr, Bezeichnung"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("DBFieldName", ReplaceMissing(dbFieldName, "%%")))
			listOfParams.Add(New SqlClient.SqlParameter("ShowInMAVersand", ReplaceMissing(showInMAVersand, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ShowInMAVersand", ReplaceMissing(showInMAVersand, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LLZusatzFieldsData)

					While reader.Read()
						Dim data = New LLZusatzFieldsData()

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.GroupNr = SafeGetInteger(reader, "GroupNr", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.DbFieldName = SafeGetString(reader, "DbFieldName")
						data.ShowInMAVersand = SafeGetBoolean(reader, "ShowInMAVersand", False)
						data.ShowInProposeNavBar = SafeGetBoolean(reader, "ShowInProposeNavBar", False)
						data.ModulName = SafeGetString(reader, "ModulName")


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

		Function LoadLLZusatzFieldsTemplateData(ByVal dbFieldName As String) As IEnumerable(Of LLZusatzFieldsTemplateData) Implements IEmployeeDatabaseAccess.LoadLLZusatzFieldsTemplateData
			Dim result As List(Of LLZusatzFieldsTemplateData) = Nothing

			Dim sql As String
			sql = "Select * "
			sql &= "From tab_LLZusatzFields_Template "
			sql &= "Where "
			sql &= "(@DBFieldName = '' Or DBFieldName Like @DBFieldName) "

			sql &= "Order By RecNr, Bezeichnung"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("DBFieldName", ReplaceMissing(dbFieldName, "%%")))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LLZusatzFieldsTemplateData)

					While reader.Read()
						Dim data = New LLZusatzFieldsTemplateData()

						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.DbFieldName = SafeGetString(reader, "DbFieldName")
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.FileName = SafeGetString(reader, "FileName")


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


	End Class


End Namespace