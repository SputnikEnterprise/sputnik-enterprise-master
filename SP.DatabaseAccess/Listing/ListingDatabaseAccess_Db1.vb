
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language



Namespace Listing


	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadDB1LOLAmount(ByVal query As String) As Decimal? Implements IListingDatabaseAccess.LoadDB1LOLAmount

			Dim result As Decimal?

			Dim sql As String = String.Format("{0}", query)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			'listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = SafeGetDecimal(reader, "m_btr", 0)

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadDB1PayrollData(ByVal query As String, ByVal dataType As DB1DataRecordType?) As IEnumerable(Of DB1PayrollData) Implements IListingDatabaseAccess.LoadDB1PayrollData

			Dim result As List(Of DB1PayrollData) = Nothing

			Dim sql As String = String.Format("{0}", query)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			'listOfParams.Add(New SqlClient.SqlParameter("MANummer", maNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of DB1PayrollData)

					While reader.Read

						Dim data = New DB1PayrollData

						data.PayrollNumber = SafeGetInteger(reader, "LONr", 0)
						data.LANumber = SafeGetDecimal(reader, "LANr", Nothing)
						data.Amount = SafeGetDecimal(reader, "m_btr", 0)


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

		Function LoadPayrollAGData(ByVal payrollNumber As Integer?, ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?, ByVal esNumber As Integer?) As DB1PayrollAGAnteilData Implements IListingDatabaseAccess.LoadPayrollAGData
			Dim result As DB1PayrollAGAnteilData = Nothing
			Dim Sql As String

			Sql = "[Get AGAnteil In Db1Listing For Payroll]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(payrollNumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(Sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New DB1PayrollAGAnteilData

					result.AHVAmountEachEmployee = SafeGetDecimal(reader, "AHVAnteilEachEmployee", 0)
					result.AHVAnteil = SafeGetDecimal(reader, "AHVAnteil", 0)

					result.AGAmountEachEmployee = SafeGetDecimal(reader, "AGAnteilEachEmployee", 0)
					result.AGBVGAmountEachEmployee = SafeGetDecimal(reader, "AGBVGAmountEachEmployee", 0)
					result.AGAnteil = SafeGetDecimal(reader, "AGAnteil", 0)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.StackTrace())
				result = Nothing

			End Try


			Return result

		End Function



	End Class

End Namespace
