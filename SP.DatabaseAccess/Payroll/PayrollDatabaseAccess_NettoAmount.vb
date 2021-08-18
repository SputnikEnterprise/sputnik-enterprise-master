Imports SP.DatabaseAccess
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation

Namespace PayrollMng


	Partial Public Class PayrollDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IPayrollDatabaseAccess

		Function IsVacationBrutto(ByVal mdNr As Integer, ByVal year As Integer, ByVal err As Boolean) As Boolean Implements IPayrollDatabaseAccess.IsVacationBrutto
			Dim result As Boolean = False

			Dim sql As String

			sql = "Select Top (1) isnull(LA.AHVPflichtig, 0) AHVPflichtig "
			sql &= "From dbo.LA LA "
			sql &= "Where LAJahr = @year "
			sql &= "And LA.LANr = 530"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = SafeGetBoolean(reader, "AHVPflichtig", 0)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function


		Function LoadFeierBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer, ByVal err As Boolean) As Decimal Implements IPayrollDatabaseAccess.LoadFeierBackNettoBasis
			Dim result As Decimal

			Dim anAmount As Decimal
			Dim amount As Decimal


			Dim sql As String

			sql = "[Load Netto Amount For FeierBetrag In Payroll]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LONr", payrollNumber))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					anAmount = SafeGetDecimal(reader, "ANAmount", 0)
					amount = SafeGetDecimal(reader, "Amount", 0)


					'result = amount - anAmount
					result = anAmount

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		''' <summary>
		''' Die Rückstellung vom Ferienentschädigung bei Netto-Berechnung (630 >> 8000.20)
		''' </summary>
		Function LoadFerienBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer, ByVal err As Boolean) As Decimal Implements IPayrollDatabaseAccess.LoadFerienBackNettoBasis
			Dim result As Decimal
			Dim anAmount As Decimal
			Dim amount As Decimal

			Dim sql As String

			sql = "[Load Netto Amount For FerienBetrag In Payroll]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LONr", payrollNumber))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					anAmount = SafeGetDecimal(reader, "ANAmount", 0)
					amount = SafeGetDecimal(reader, "Amount", 0)


					'result = amount - anAmount
					result = anAmount
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result
		End Function

		Function Load13LohnBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer, ByVal err As Boolean) As Decimal Implements IPayrollDatabaseAccess.Load13LohnBackNettoBasis
			Dim result As Decimal

			Dim sSql As String = "[Get Data 4 13LohnBetrag in Netto]"
			Dim anAmount As Decimal
			Dim amount As Decimal

			Dim sql As String

			sql = "[Load Netto Amount For 13LohnBetrag In Payroll]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LONr", payrollNumber))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					anAmount = SafeGetDecimal(reader, "ANAmount", 0)
					amount = SafeGetDecimal(reader, "Amount", 0)


					'result = amount - anAmount
					result = anAmount

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				err = True
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


	End Class

End Namespace
