
Imports SP.DatabaseAccess.Listing.DataObjects

Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadCustomerDataForCreditlimits(ByVal mdNr As Integer, ByVal filiale As String) As IEnumerable(Of CustomerCreditData) Implements IListingDatabaseAccess.LoadCustomerDataForCreditlimits
			Dim result As List(Of CustomerCreditData) = Nothing

			Dim SQL As String

			SQL = "[List KDNR Kreditlimite ueberschritten]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("filiale", ReplaceMissing(filiale, String.Empty)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerCreditData)

					While reader.Read()
						Dim overviewData As New CustomerCreditData

						overviewData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						overviewData.FirstCreditlimit = SafeGetDecimal(reader, "Kreditlimite", Nothing)
						overviewData.SecondCreditlimit = SafeGetDecimal(reader, "Kreditlimite_2", Nothing)


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

		Function LoadInvoiceDataForFinishedCredits(ByVal mdNr As Integer, ByVal invoiceDeadlineDate As Date?, ByVal invoiceFromDate As Date?, ByVal invoiceToDate As Date?, ByVal createdFromDate As Date?, ByVal createdToDate As Date?) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceDataForFinishedCredits
			Dim result As List(Of InvoiceData) = Nothing

			Dim SQL As String

			SQL = "[List Invoices For Finished Credits]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceDeadlineDate", ReplaceMissing(invoiceDeadlineDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceFromDate", ReplaceMissing(invoiceFromDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceToDate", ReplaceMissing(invoiceToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFromDate", ReplaceMissing(createdFromDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdToDate", ReplaceMissing(createdToDate, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of InvoiceData)

					While reader.Read()
						Dim overviewData As New InvoiceData

						overviewData.RENr = SafeGetInteger(reader, "RENr", 0)
						overviewData.InvoiceDate = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						overviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


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

		Function LoadInvoiceDataForNOTFinishedCredits(ByVal mdNr As Integer, ByVal invoiceDeadlineDate As Date?, ByVal invoiceFromDate As Date?, ByVal invoiceToDate As Date?, ByVal createdFromDate As Date?, ByVal createdToDate As Date?) As IEnumerable(Of InvoiceData) Implements IListingDatabaseAccess.LoadInvoiceDataForNOTFinishedCredits
			Dim result As List(Of InvoiceData) = Nothing

			Dim SQL As String

			SQL = "[List Invoices For ALL Credits]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceDeadlineDate", ReplaceMissing(invoiceDeadlineDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceFromDate", ReplaceMissing(invoiceFromDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceToDate", ReplaceMissing(invoiceToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFromDate", ReplaceMissing(createdFromDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdToDate", ReplaceMissing(createdToDate, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of InvoiceData)

					While reader.Read()
						Dim overviewData As New InvoiceData

						overviewData.RENr = SafeGetInteger(reader, "RENr", 0)
						overviewData.InvoiceDate = SafeGetDateTime(reader, "Fak_Dat", Nothing)
						overviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


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

		Function LoadInvoiceOpenAmountData(ByVal mdNr As Integer, ByVal whereQuery As String, ByVal firstOpenAmount As Decimal?, ByVal secondOpenAmount As Decimal?) As IEnumerable(Of CustomerOpenAmountData) Implements IListingDatabaseAccess.LoadInvoiceOpenAmountData

			Dim result As List(Of CustomerOpenAmountData) = Nothing

			Dim sql As String

			sql = "Select RE.KDNr, Sum(Round(BetragInk, 2) - Round(Bezahlt, 2)) As TotalOffen "
			sql &= "From dbo.RE "

			If Not String.IsNullOrWhiteSpace(whereQuery) Then sql &= String.Format("Where {0}", whereQuery)
			sql &= " Group By RE.KDNR HAVING Sum(Round(RE.BetragInk, 2) - Round(RE.Bezahlt, 2)) "

			If firstOpenAmount > 0 And secondOpenAmount > 0 Then
				sql &= " Between " & firstOpenAmount & " And " & secondOpenAmount

			ElseIf firstOpenAmount > 0 Then
				sql &= String.Format(" > {0}", firstOpenAmount)

			Else
				sql &= String.Format(" < {0}", secondOpenAmount)

			End If
			sql &= " Order By RE.KDNr"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerOpenAmountData)

					While reader.Read

						Dim viewData = New CustomerOpenAmountData

						viewData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						viewData.OpenAmount = SafeGetDecimal(reader, "TotalOffen", 0)


						result.Add(viewData)

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
