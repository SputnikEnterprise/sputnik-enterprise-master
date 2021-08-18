Imports System.Data.SqlClient

Namespace Vacancy

	Partial Class VacancyDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IVacancyDatabaseAccess


		''' <summary>
		''' Get the vacancancy for export to jobch.
		''' </summary>
		''' <param name="customerGuid">The customer guid.</param>
		''' <param name="vacancyNumber">The vacancy number.</param>
		''' <returns>Datatable with data or nothing in error case.</returns>
		Function GetVacancyForExportToJobsCH(ByVal customerGuid As String, ByVal vacancyNumber As Integer) As DataTable Implements IVacancyDatabaseAccess.GetVacancyForExportToJobsCH

			Dim sql As String

			sql = "[Get Vacancy For Export to Jobs.CH]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid ", ReplaceMissing(customerGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim dataTable = FillDataTable(sql, listOfParams, CommandType.StoredProcedure)

			Return dataTable
		End Function

		''' <summary>
		''' Get the vacancancy for export to ostjobch.
		''' </summary>
		''' <param name="customerGuid">The customer guid.</param>
		''' <param name="vacancyNumber">The vacancy number.</param>
		''' <returns>Datatable with data or nothing in error case.</returns>
		Function GetVacancyForExportToOstJobsCH(ByVal customerGuid As String, ByVal vacancyNumber As Integer) As DataTable Implements IVacancyDatabaseAccess.GetVacancyForExportToOstJobsCH

			Dim sql As String

			sql = "[Get Vacancy For Export to OstJob.CH]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid ", ReplaceMissing(customerGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim dataTable = FillDataTable(sql, listOfParams, CommandType.StoredProcedure)

			Return dataTable
		End Function

		Function GetVacancyForExportToIntern(ByVal customerGuid As String, ByVal vacancyNumber As Integer) As DataTable Implements IVacancyDatabaseAccess.GetVacancyForExportToIntern

			Dim sql As String

			sql = "[Get Vacancy For Export to Intern]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid ", ReplaceMissing(customerGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vacancyNumber, DBNull.Value)))

			Dim dataTable = FillDataTable(sql, listOfParams, CommandType.StoredProcedure)

			Return dataTable
		End Function


	End Class

End Namespace
