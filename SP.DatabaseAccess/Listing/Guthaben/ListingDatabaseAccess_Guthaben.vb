
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

		Function LoadFlexibleWorkingHoursData(ByVal mdNr As Integer, ByVal employeeNumber As Integer) As EmployeeCreditData Implements IListingDatabaseAccess.LoadFlexibleWorkingHoursData
			Dim result As EmployeeCreditData = Nothing

			Dim backedHours As Decimal
			Dim backedAmount As Decimal
			Dim payedHours As Decimal
			Dim payedAmount As Decimal

			Dim sql As String

			sql = "[Load Data For Employee Flexible Working Hours]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("mdnr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					backedHours = SafeGetDecimal(reader, "BackedStd", 0)
					backedAmount = SafeGetDecimal(reader, "BackedBetrag", 0)
					payedHours = SafeGetDecimal(reader, "PayedStd", 0)
					payedAmount = SafeGetDecimal(reader, "PayedBetrag", 0)


					result = New EmployeeCreditData With {.BackedAmount = backedAmount, .BackedHours = backedHours, .PayedAmount = payedAmount, .PayedHours = payedHours}

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadFlexibleWorkingHoursForPayrollData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal year As Integer, ByVal month As Integer) As EmployeeCreditData Implements IListingDatabaseAccess.LoadFlexibleWorkingHoursForPayrollData
			Dim result As EmployeeCreditData = Nothing

			Dim backedHours As Decimal
			Dim backedAmount As Decimal
			Dim payedHours As Decimal
			Dim payedAmount As Decimal

			Dim sql As String

			sql = "[Load For Employee Flexible Working Hours In Payroll]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("mdnr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))
			listOfParams.Add(New SqlClient.SqlParameter("month", month))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					backedHours = SafeGetDecimal(reader, "BackedStd", 0)
					backedAmount = SafeGetDecimal(reader, "BackedBetrag", 0)
					payedHours = SafeGetDecimal(reader, "PayedStd", 0)
					payedAmount = SafeGetDecimal(reader, "PayedBetrag", 0)


					result = New EmployeeCreditData With {.BackedAmount = backedAmount, .BackedHours = backedHours, .PayedAmount = payedAmount, .PayedHours = payedHours}

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

	End Class


End Namespace


