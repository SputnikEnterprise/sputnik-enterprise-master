
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.ScanJob.DataObjects
Imports System.Text
'Imports System.Transactions
Imports SP.DatabaseAccess.Applicant.DataObjects

Namespace ScanJob


	Partial Class ScanJobDatabaseAccess


		Inherits DatabaseAccessBase
		Implements IScanJobDatabaseAccess


		''' <summary>
		''' Adds a employee document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddEmployeeScanJobDocument(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddEmployeeScanJobDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New ScanJobDocument For Employee]"

			' Parameters
			Dim docDescription As String = String.Format("{0}|{1}|{2}", documentData.FoundedCodeValue, documentData.ImportedFileGuid, documentData.ImportedDocGuid)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocPath", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(documentData.DocumentCategoryNumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("Pages", 0))
			listOfParams.Add(New SqlClient.SqlParameter("FileSize", 0))
			listOfParams.Add(New SqlClient.SqlParameter("DocXML", DBNull.Value))
			listOfParams.Add(New SqlClient.SqlParameter("PlainText", DBNull.Value))
			listOfParams.Add(New SqlClient.SqlParameter("FileHashvalue", DBNull.Value))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a employment into employee document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddEmploymentEmployeeScanJobDocument(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddEmploymentEmployeeScanJobDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New Employment ScanJobDocument For Employee]"

			' Parameters
			Dim docDescription As String = String.Format("{0}|{1}|{2}", documentData.FoundedCodeValue, documentData.ImportedFileGuid, documentData.ImportedDocGuid)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocPath", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(documentData.DocumentCategoryNumber, 0)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If

			Return success

		End Function

		Function LoadEmployeeReportLineDataForScanJobDocument(ByVal mdNr As Integer, ByVal savescanwithZeroAmount As Boolean, ByVal documentData As ScanJobData) As IEnumerable(Of ReportLineData) Implements IScanJobDatabaseAccess.LoadEmployeeReportLineDataForScanJobDocument
			Dim result As List(Of ReportLineData) = Nothing

			Dim sql As String
			sql = "[Load Created Employee Reportline For ScanJobDocument]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FDay", ReplaceMissing(documentData.ReportFirstDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LDay", ReplaceMissing(documentData.ReportLastDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(documentData.ReportMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(documentData.ReportYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedZero", ReplaceMissing(savescanwithZeroAmount, False)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportLineData)

					While reader.Read
						Dim data = New ReportLineData

						data.RecID = SafeGetInteger(reader, "ID", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.ReportNumber = SafeGetInteger(reader, "RPNr", Nothing)
						data.ReportLineNumber = SafeGetInteger(reader, "RPLNr", Nothing)
						data.ReportLineFrom = SafeGetDateTime(reader, "VonDate", Nothing)
						data.ReportLineTo = SafeGetDateTime(reader, "BisDate", Nothing)


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function


		''' <summary>
		''' Adds a payroll into employee document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddPayrollEmployeeScanJobDocument(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddPayrollEmployeeScanJobDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New Payroll ScanJobDocument For Employee]"

			' Parameters
			Dim docDescription As String = String.Format("{0}|{1}|{2}", documentData.FoundedCodeValue, documentData.ImportedFileGuid, documentData.ImportedDocGuid)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocPath", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(documentData.DocumentCategoryNumber, 0)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a Customer document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddCustomerScanJobDocument(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddCustomerScanJobDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New ScanJobDocument For Customer]"

			' Parameters
			Dim docDescription As String = String.Format("{0}|{1}|{2}", documentData.FoundedCodeValue, documentData.ImportedFileGuid, documentData.ImportedDocGuid)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocPath", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(documentData.DocumentCategoryNumber, 0)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a employment Customer document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddEmploymentCustomerScanJobDocument(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddEmploymentCustomerScanJobDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New Employment ScanJobDocument For Customer]"

			' Parameters
			Dim docDescription As String = String.Format("{0}|{1}|{2}", documentData.FoundedCodeValue, documentData.ImportedFileGuid, documentData.ImportedDocGuid)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocPath", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(documentData.DocumentCategoryNumber, 0)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a invoice Customer document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddInvoiceCustomerScanJobDocument(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddInvoiceCustomerScanJobDocument

			Dim success = True

			Dim sql As String

			sql = "[Create New Invoice ScanJobDocument For Customer]"

			' Parameters
			Dim docDescription As String = String.Format("{0}|{1}|{2}", documentData.FoundedCodeValue, documentData.ImportedFileGuid, documentData.ImportedDocGuid)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocPath", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", ReplaceMissing(documentData.DocumentCategoryNumber, 0)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If

			Return success

		End Function

		Function LoadCustomerReportLineDataForScanJobDocument(ByVal mdNr As Integer, ByVal savescanwithZeroAmount As Boolean, ByVal documentData As ScanJobData) As IEnumerable(Of ReportLineData) Implements IScanJobDatabaseAccess.LoadCustomerReportLineDataForScanJobDocument
			Dim result As List(Of ReportLineData) = Nothing

			Dim sql As String
			sql = "[Load Created Customer Reportline For ScanJobDocument]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FDay", ReplaceMissing(documentData.ReportFirstDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LDay", ReplaceMissing(documentData.ReportLastDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(documentData.ReportMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(documentData.ReportYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedZero", ReplaceMissing(savescanwithZeroAmount, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ReportLineID", ReplaceMissing(documentData.ReportLineID, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportLineData)

					While reader.Read
						Dim data = New ReportLineData

						data.RecID = SafeGetInteger(reader, "ID", Nothing)
						data.LANr = SafeGetDecimal(reader, "LANr", Nothing)
						data.ReportNumber = SafeGetInteger(reader, "RPNr", Nothing)
						data.ReportLineNumber = SafeGetInteger(reader, "RPLNr", Nothing)
						data.ReportLineFrom = SafeGetDateTime(reader, "VonDate", Nothing)
						data.ReportLineTo = SafeGetDateTime(reader, "BisDate", Nothing)


						result.Add(data)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function AddReportScanJobDocument(ByVal documentData As ScanJobData, ByVal reportLineData As ReportLineData) As Boolean Implements IScanJobDatabaseAccess.AddReportScanJobDocument
			Dim success As Boolean = True
			Dim newDocGuid As String = Guid.NewGuid.ToString

			Dim sql As String
			sql = "[Create New Report ScanJobDocument For Report]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", ReplaceMissing(reportLineData.ReportLineNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", "PDF"))
			'listOfParams.Add(New SqlClient.SqlParameter("ExistsFileGuid", ReplaceMissing(documentData.ImportedFileGuid, Guid.NewGuid.ToString)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			Dim NewDocGuidParameter = New SqlClient.SqlParameter("@NewDocGuid", SqlDbType.NVarChar, 50)
			NewDocGuidParameter.Direction = ParameterDirection.Output
			listOfParams.Add(NewDocGuidParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing AndAlso Not NewDocGuidParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.ImportedDocRecNr = CType(recNrParameter.Value, Integer)
				documentData.ImportedDocGuid = CType(NewDocGuidParameter.Value, String)
				documentData.IsValid = True
				documentData.CreatedOn = Now

			Else
				success = False
			End If


			Return success

		End Function

		Function AddReportScanDocumentIntoScanJobDb(ByVal documentData As ScanJobData) As Boolean Implements IScanJobDatabaseAccess.AddReportScanDocumentIntoScanJobDb
			Dim success As Boolean = True


			Dim sql As String
			sql = "[Create New Report ScanDocument For ScanJobDb]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(documentData.Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ImportedFileGuid", ReplaceMissing(documentData.ImportedFileGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CalenderWeek", ReplaceMissing(documentData.ReportWeek, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", ReplaceMissing(documentData.ReportYear, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", ReplaceMissing(documentData.ReportMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FDay", ReplaceMissing(documentData.ReportFirstDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LDay", ReplaceMissing(documentData.ReportLastDay, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(documentData.RecordNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("FoundedCodeValue", ReplaceMissing(documentData.FoundedCodeValue, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("IsValid", ReplaceMissing(documentData.IsValid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.ScanContent, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If success AndAlso Not newIdParameter.Value Is Nothing Then
				documentData.ImportedDocID = CType(newIdParameter.Value, Integer)
				documentData.IsValid = True
				documentData.CreatedOn = Now
			Else
				success = False
			End If


			Return success

		End Function

		Function LoadDropInDataForApplication(ByVal customerID As String, ByVal dropInFilename As String) As ApplicationData Implements IScanJobDatabaseAccess.LoadDropInDataForApplication
			Dim result As ApplicationData = Nothing

			Dim sql As String
			sql = "[Load DropIn Data For Creating Application]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("dropInFilename", ReplaceMissing(dropInFilename, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New ApplicationData

					result.Advisor = SafeGetString(reader, "CreatedFrom")
					result.BusinessBranch = SafeGetString(reader, "BusinessBranch")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

	End Class


End Namespace
