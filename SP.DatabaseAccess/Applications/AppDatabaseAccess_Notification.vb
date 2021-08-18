
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports System.Text
'Imports System.Transactions
Imports SP.DatabaseAccess.ScanJob.DataObjects


Namespace Applicant


	Partial Class AppDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IAppDatabaseAccess


		''' <summary>
		''' Adds a application data.
		''' </summary>
		Public Function AddApplicationToClientDb(ByVal application As ApplicationData) As Boolean Implements IAppDatabaseAccess.AddApplicationToClientDb

			Dim success = True

			Dim sql As String

			sql = "[Create Assigned Application InTo Local Database]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(application.Customer_ID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ApplicationID", ReplaceMissing(application.ApplicationID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("VacancyNumber", ReplaceMissing(application.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(application.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(application.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Advisor", ReplaceMissing(application.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Dismissalperiod", ReplaceMissing(application.Dismissalperiod, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Availability", ReplaceMissing(application.Availability, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Comment", ReplaceMissing(application.Comment, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("EmployeeID", ReplaceMissing(application.EmployeeID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedOn", ReplaceMissing(application.CreatedOn, Now)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				application.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a employee document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddApplicantDocumentToEmployee(ByVal documentData As ApplicantDocumentData) As Boolean Implements IAppDatabaseAccess.AddApplicantDocumentToEmployee

			Dim success = True

			Dim sql As String

			sql = "[Create New ScanJobDocument For Employee]"

			' Parameters
			Dim docDescription As String = String.Format("{0}", documentData.ID)
			Dim docCategory As Integer = documentData.Category.GetValueOrDefault(0)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(documentData.FK_ApplicantID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", ReplaceMissing(documentData.Title, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("Beschreibung", ReplaceMissing(docDescription, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("DocPath", String.Empty))
      listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(documentData.Content, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("ScanExtension", documentData.FileExtension))
      listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", docCategory))
      listOfParams.Add(New SqlClient.SqlParameter("Pages", ReplaceMissing(documentData.Pages, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("FileSize", ReplaceMissing(documentData.FileSize, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("DocXML", ReplaceMissing(documentData.DocXML, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("PlainText", ReplaceMissing(documentData.PlainText, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("FileHashvalue", ReplaceMissing(documentData.HashValue, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				documentData.ID = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If

			Return success

		End Function


	End Class


End Namespace
