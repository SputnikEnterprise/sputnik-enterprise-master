
Imports SP.DatabaseAccess
'Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports System.Text
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects


Namespace Invoice

	Partial Public Class InvoiceDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IInvoiceDatabaseAccess



#Region "Public Methods"

		Public Function LoadCustomerDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?) As IEnumerable(Of CustomerOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadCustomerDataForSearchAutomatedInvoices

			Dim result As List(Of CustomerOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "[Get CustomerData For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CustomerOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New CustomerOverviewAutomatedInvoiceData

						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						data.BillTypeCode = SafeGetString(reader, "Faktura")
						data.Company1 = SafeGetString(reader, "Firma1")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")
						data.ReportLineBetrag = SafeGetDecimal(reader, "Betrag", 0)


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

		Public Function LoadReportDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal reportNumber As Integer?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadReportDataForSearchAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "[Get ReportData For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)

						data.ReportMonth = SafeGetInteger(reader, "Monat", 0)
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						data.ReportFrom = SafeGetDateTime(reader, "Von", Nothing)
						data.ReportTo = SafeGetDateTime(reader, "Bis", Nothing)

						data.EmployeeLastname = SafeGetString(reader, "Nachname")
						data.EmployeeFirstname = SafeGetString(reader, "Vorname")
						data.Company1 = SafeGetString(reader, "Firma1")

						data.ReportLineBetrag = SafeGetDecimal(reader, "Betrag", 0)

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

		Public Function LoadReportCostcenterDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal reportNumber As Integer?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadReportCostcenterDataForSearchAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "[Get ReportCostcenterData For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						data.KstNr = SafeGetInteger(reader, "KSTNr", 0)
						data.KSTBez = SafeGetString(reader, "KSTBez")

						data.ReportMonth = SafeGetInteger(reader, "Monat", 0)
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						data.ReportFrom = SafeGetDateTime(reader, "Von", Nothing)
						data.ReportTo = SafeGetDateTime(reader, "Bis", Nothing)

						data.EmployeeLastname = SafeGetString(reader, "Nachname")
						data.EmployeeFirstname = SafeGetString(reader, "Vorname")
						data.Company1 = SafeGetString(reader, "Firma1")

						data.ReportLineBetrag = SafeGetDecimal(reader, "Betrag", 0)

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

		Public Function LoadEmploymentDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal employmentNumber As Integer?) As IEnumerable(Of EmploymentOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadEmploymentDataForSearchAutomatedInvoices

			Dim result As List(Of EmploymentOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "[Get EmploymentData For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of EmploymentOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New EmploymentOverviewAutomatedInvoiceData

						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						data.ES_Ab = SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.ES_Ende = SafeGetDateTime(reader, "ES_Ende", Nothing)
						data.EmployeeLastname = SafeGetString(reader, "Nachname")
						data.EmployeeFirstname = SafeGetString(reader, "Vorname")
						data.Company1 = SafeGetString(reader, "Firma1")
						data.ReportLineBetrag = SafeGetDecimal(reader, "Betrag", 0)
						data.RPLYear = SafeGetInteger(reader, "RPLYear", 0)

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

		Public Function LoadReportLineDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?) As IEnumerable(Of ReportLineOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadReportLineDataForSearchAutomatedInvoices

			Dim result As List(Of ReportLineOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "[Get ReportlineData For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportLineOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportLineOverviewAutomatedInvoiceData

						data.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)

						data.Company1 = SafeGetString(reader, "Firma1")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetString(reader, "PLZ")
						data.Location = SafeGetString(reader, "Ort")

						data.BillTypeCode = SafeGetString(reader, "Faktura")
						data.InvoiceOption = SafeGetString(reader, "FakturaOption")

						data.ReportLineBetrag = SafeGetDecimal(reader, "Betrag", 0)

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



#Region "Load reportdata with customernumber"

		Public Function LoadReportDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal closedMonth As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadReportDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select"
			sql &= " RPL.RPNr,"
			sql &= " Convert(INT, RP.Jahr) Jahr"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where"
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			If closedMonth.GetValueOrDefault(False) Then sql &= " And RP.Erfasst = 1"
			sql &= " Group By"
			sql &= " RPL.RPNr,"
			sql &= " RP.Jahr"
			sql &= " Order By RPL.RPNr ASC,"
			sql &= " RP.Jahr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.CustomerNumber = customerNumber
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)

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

		Public Function LoadReportLineDataWithReportNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithReportNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With ReportNumber For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ReportLineCreatingAutomatedInvoiceData

					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadReportLineDataWithCustomerNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithCustomerNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With CustomerNumber For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNR", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Month", ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("closedMonth", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ReportLineCreatingAutomatedInvoiceData

					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

#End Region


#Region "Load employment data with customernumber"

		Public Function LoadEmploymentDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByMonth As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadEmploymentDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select RPL.ESNr, Convert(INT, RP.Jahr) Jahr"
			If groupByMonth.GetValueOrDefault(False) Then sql &= ",Convert(INT, RP.Monat) Monat"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where "
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And (@Erfasst = 0 Or RP.Erfasst = 1)"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			sql &= " Group By RPL.ESNr, RP.Jahr"
			If groupByMonth.GetValueOrDefault(False) Then sql &= ",RP.Monat "
			sql &= " Order By RPL.ESNr ASC, RP.Jahr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Erfasst", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						If groupByMonth.GetValueOrDefault(False) Then
							data.ReportMonth = SafeGetInteger(reader, "Monat", 0)
						End If

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

		Public Function LoadReportLineDataWithEmploymentNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal employmentNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithEmploymentNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With EmploymentNumber For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Month", ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("closedMonth", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				result = New ReportLineCreatingAutomatedInvoiceData

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

#End Region



#Region "Load customer data with customernumber"

		Public Function LoadCustomerDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByMonth As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadCustomerDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select Convert(INT, RP.Jahr) Jahr"
			If groupByMonth.GetValueOrDefault(False) Then sql &= ",Convert(INT, RP.Monat) Monat"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where"
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And (@Erfasst = 0 Or RP.Erfasst = 1)"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			sql &= " Group By RP.Jahr"
			If groupByMonth.GetValueOrDefault(False) Then sql &= ",RP.Monat"
			sql &= " Order By RP.Jahr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Erfasst", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						data.CustomerNumber = customerNumber
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						If groupByMonth.GetValueOrDefault(False) Then							data.ReportMonth = SafeGetInteger(reader, "Monat", 0)
							'Else
							'	data.ReportMonth = Now.Month
							'End If


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


#End Region




#Region "Load cost center data with customernumber"

		Public Function LoadCostcenterDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByReportNumber As Boolean?, ByVal groupByMonth As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadCostcenterDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select"
			If groupByReportNumber Then sql &= " RPL.RPNr,"
			If groupByMonth Then sql &= " Convert(INT, RP.Monat) Monat,"
			sql &= " RPL.KSTNr, Convert(INT, RP.Jahr) Jahr"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where"
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And (@Erfasst = 0 Or RP.Erfasst = 1)"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			sql &= " And (@Erfasst = 0 Or RP.Erfasst = 1)"
			sql &= " Group By"
			If groupByReportNumber Then sql &= " RPL.RPNr,"
			If groupByMonth Then sql &= " RP.Monat,"
			sql &= " RPL.KSTNr, RP.Jahr"
			sql &= " Order By"
			If groupByReportNumber Then sql &= " RPL.RPNr ASC,"
			sql &= " RPL.KSTNr ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Erfasst", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						data.CustomerNumber = customerNumber
						If groupByReportNumber Then data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						If groupByMonth Then data.ReportMonth = SafeGetInteger(reader, "Monat", 0)
						data.KstNr = SafeGetInteger(reader, "KSTNr", 0)
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)


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

		Public Function LoadReportLineDataWithCostcenterNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal kstNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithCostcenterNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With CostCenterNumber For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KSTNr", ReplaceMissing(kstNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Month", ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("closedMonth", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ReportLineCreatingAutomatedInvoiceData

					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

#End Region



#Region "Load weekly data with customernumber"

		Public Function LoadWeeklyDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByEmploymentNumber As Boolean?, ByVal groupByReportNumber As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadWeeklyDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate) As VonWeek, Convert(INT, RP.Jahr) Jahr"
			If groupByEmploymentNumber.GetValueOrDefault(False) Then sql &= ",RPL.ESNr"
			If groupByReportNumber.GetValueOrDefault(False) Then sql &= ",RPL.RPNr"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where "
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And Dbo.F_ISO_WEEK_OF_YEAR(VonDate) = Dbo.F_ISO_WEEK_OF_YEAR(BisDate)"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			sql &= " And (@Erfasst = 0 Or RP.Erfasst = 1)"
			sql &= " Group By Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate), RP.Jahr"
			If groupByEmploymentNumber.GetValueOrDefault(False) Then sql &= ",RPL.ESNr"
			If groupByReportNumber.GetValueOrDefault(False) Then sql &= ",RPL.RPNr"
			sql &= " Order By RP.Jahr ASC, Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate) ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Erfasst", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						If groupByEmploymentNumber.GetValueOrDefault(False) Then data.ESNr = SafeGetInteger(reader, "ESNr", 0)
						If groupByReportNumber.GetValueOrDefault(False) Then data.RPNr = SafeGetInteger(reader, "RPNr", 0)

						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						data.ReportlineWeekFrom = SafeGetInteger(reader, "VonWeek", 0)

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

		Public Function LoadReportLineDataWithWeeklyEmploymentAndReportNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal employmentNumber As Integer?, ByVal weekNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithWeeklyEmploymentAndReportNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With Employment And ReportNumber For Weekly Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNR", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Month", ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("WeekNr", ReplaceMissing(weekNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("closedMonth", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ReportLineCreatingAutomatedInvoiceData

					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadWeeklyCostcenterDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByReportNumber As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadWeeklyCostcenterDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate) As VonWeek, Convert(INT, RP.Jahr) Jahr,"
			sql &= " RPL.KSTNr"
			If groupByReportNumber.GetValueOrDefault(False) Then sql &= ",RPL.RPNr"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Where "
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And Dbo.F_ISO_WEEK_OF_YEAR(VonDate) = Dbo.F_ISO_WEEK_OF_YEAR(BisDate)"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			sql &= " And (@Erfasst = 0 Or RP.Erfasst = 1)"
			sql &= " Group By Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate), RP.Jahr,"
			sql &= " RPL.KSTNr"
			If groupByReportNumber.GetValueOrDefault(False) Then sql &= ",RPL.RPNr"
			sql &= " Order By RP.Jahr ASC, Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate) ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Erfasst", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						If groupByReportNumber.GetValueOrDefault(False) Then data.RPNr = SafeGetInteger(reader, "RPNr", 0)

						data.KstNr = SafeGetInteger(reader, "KSTNr", 0)
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						data.ReportlineWeekFrom = SafeGetInteger(reader, "VonWeek", 0)

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

		Public Function LoadReportLineDataWithWeeklyCostCenterNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal kstNumber As Integer?, ByVal weekNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithWeeklyCostCenterNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With CostCenterNumber For Weekly Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNR", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KSTNr", ReplaceMissing(kstNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Month", ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("WeekNr", ReplaceMissing(weekNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("closedMonth", ReplaceMissing(closedMonth, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ReportLineCreatingAutomatedInvoiceData

					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Public Function LoadWeeklyCostcenterAddressDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByReportNumber As Boolean?) As IEnumerable(Of ReportOverviewAutomatedInvoiceData) Implements IInvoiceDatabaseAccess.LoadWeeklyCostcenterAddressDataForCreatingAutomatedInvoices

			Dim result As List(Of ReportOverviewAutomatedInvoiceData) = Nothing

			Dim sql As String

			sql = "Select Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate) As VonWeek, Convert(INT, RP.Jahr) Jahr,"
			sql &= " kst.REAddressRecNr AddNr"
			If groupByReportNumber.GetValueOrDefault(False) Then sql &= ",RPL.RPNr"
			sql &= " From RPL"
			sql &= " Left Join RP On RPL.RPNr = RP.RPNr"
			sql &= " Left Join KD_KST KST On RPL.KSTNr = Kst.RecNr And RPL.KDNr = Kst.KDNr "
			sql &= " Where "
			sql &= " RP.MDNr = @MDNr"
			sql &= " And RPL.KDNr = @KDNr"
			sql &= " And Dbo.F_ISO_WEEK_OF_YEAR(VonDate) = Dbo.F_ISO_WEEK_OF_YEAR(BisDate)"
			sql &= " And RPL.K_Betrag <> 0"
			sql &= " And RPL.RENr = 0 And RPL.KD = 1"
			sql &= " Group By Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate), RP.Jahr,"
			sql &= " kst.REAddressRecNr"
			If groupByReportNumber.GetValueOrDefault(False) Then sql &= ",RPL.RPNr"
			sql &= " Order by RP.Jahr ASC, kst.REAddressRecNr, Dbo.F_ISO_WEEK_OF_YEAR(RPL.VonDate) ASC"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try
				If reader IsNot Nothing Then

					result = New List(Of ReportOverviewAutomatedInvoiceData)

					While reader.Read
						Dim data = New ReportOverviewAutomatedInvoiceData

						If groupByReportNumber.GetValueOrDefault(False) Then data.RPNr = SafeGetInteger(reader, "RPNr", 0)
						data.KSTAddNr = SafeGetInteger(reader, "AddNr", 0)
						data.ReportYear = SafeGetInteger(reader, "Jahr", 0)
						data.ReportlineWeekFrom = SafeGetInteger(reader, "VonWeek", 0)

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

		Public Function LoadReportLineDataWithWeeklyCostCenterAddressNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal weekNumber As Integer?, ByVal addressNumber As Integer?, ByVal jahr As Integer?) As ReportLineCreatingAutomatedInvoiceData Implements IInvoiceDatabaseAccess.LoadReportLineDataWithWeeklyCostCenterAddressNumberForCreatingAutomatedInvoices

			Dim result As ReportLineCreatingAutomatedInvoiceData = Nothing

			Dim sql As String

			sql = "[Get ReportLineData With CostCenterAddressNumber For Weekly Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNR", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Year", ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("WeekNr", ReplaceMissing(weekNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("AddressNr", ReplaceMissing(addressNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ReportLineCreatingAutomatedInvoiceData

					result.RPNr = SafeGetInteger(reader, "RPNr", 0)
					result.RPLNr = SafeGetInteger(reader, "RPNr", 0)
					result.ESNr = SafeGetInteger(reader, "ESNr", 0)
					result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
					result.ReportLineBetrag = SafeGetDecimal(reader, "K_Betrag", 0)
					result.MwSt = SafeGetDecimal(reader, "MWST", 0)
					result.ReportMonth = SafeGetInteger(reader, "Monat", 0)
					result.ReportYear = SafeGetInteger(reader, "Jahr", 0)

					result.ES_Einstufung = SafeGetString(reader, "ES_Einstufung")
					result.KDBranche = SafeGetString(reader, "KDBranche")
					result.RPKst1 = SafeGetString(reader, "RPKst1")
					result.RPKst2 = SafeGetString(reader, "RPKst2")
					result.RPKst = SafeGetString(reader, "RPKst")

					result.BetragInkMwStTotal = SafeGetDecimal(reader, "BetragInkMwStTotal", 0)
					result.BetragOhneMwStTotal = SafeGetDecimal(reader, "BetragOhneMwStTotal", 0)
					result.RPLID = SafeGetString(reader, "RPLID")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

#End Region



		Public Function LoadCustomerInvoiceDataForAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal kstNumber As Integer?, ByVal addressNumber As Integer?) As CustomerReAddress Implements IInvoiceDatabaseAccess.LoadCustomerInvoiceDataForAutomatedInvoices

			Dim result As CustomerReAddress = Nothing

			Dim sql As String

			sql = "[Get CustomerInvoiceAddressData For Create New Automated Invoices]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KSTNr", ReplaceMissing(kstNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RecNr", ReplaceMissing(addressNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				result = New CustomerReAddress
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.Id = SafeGetInteger(reader, "ID", 0)
					result.KDNr = SafeGetInteger(reader, "KDNr", 0)
					result.REFirma = SafeGetString(reader, "REFirma")
					result.REFirma2 = SafeGetString(reader, "REFirma2")
					result.REFirma3 = SafeGetString(reader, "REFirma3")
					result.REStrasse = SafeGetString(reader, "REStrasse")
					result.REPLZ = SafeGetString(reader, "REPLZ")
					result.REOrt = SafeGetString(reader, "REOrt")
					result.reeMail = SafeGetString(reader, "REeMail")
					result.SendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
					result.RELand = SafeGetString(reader, "RELand")
					result.REAbteilung = SafeGetString(reader, "REAbteilung")
					result.REZhd = SafeGetString(reader, "REZhd")
					result.REPostfach = SafeGetString(reader, "REPostfach")
					result.RecNr = SafeGetShort(reader, "RecNr", 0)
					result.MahnCode = SafeGetString(reader, "MahnCode")
					result.PaymentCondition = SafeGetString(reader, "ZahlKond")
					result.IsActive = SafeGetBoolean(reader, "ActiveRec", False)

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
		''' Updates a reportline with invoicenumber
		''' </summary>
		Public Function UpdateReportlineInoviceNumbers(ByVal invoiceNumber As Integer, ByVal reportlineIDs As String) As Boolean Implements IInvoiceDatabaseAccess.UpdateReportlineInoviceNumbers

			Dim success = False

			Dim sql As String
			sql = "Update dbo.RPL SET"
			sql &= " RENr = @RENr"
			sql &= " Where ID In"
			sql &= String.Format(" ({0}) ", reportlineIDs)
			sql &= " And KD = 1"
			sql &= " And (RENr Is Null Or RENr = 0)"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function

		''' <summary>
		''' delete invoices with invoicenumber. without any saving data!
		''' </summary>
		Public Function DeleteStraightCreatedInvoices(ByVal mdNr As Integer, ByVal invoiceNumbers As Integer()) As Boolean Implements IInvoiceDatabaseAccess.DeleteStraightCreatedInvoices

			Dim success = False

			Dim invoiceNumbersBuffer As String = String.Empty

			For Each number In invoiceNumbers

				invoiceNumbersBuffer = invoiceNumbersBuffer & IIf(invoiceNumbersBuffer <> "", ", ", "") & number

			Next

			Dim sql As String
			sql = "Update RPL SET"
			sql &= " RENr = 0"
			sql &= " Where RENr In"
			sql &= String.Format(" ({0}) ", invoiceNumbersBuffer)
			sql &= " And KD = 1;"
			sql &= " Delete RE"
			sql &= " Where RENr In"
			sql &= String.Format(" ({0}) ", invoiceNumbersBuffer)

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function




#End Region




#Region "Private Methods"


#End Region

	End Class

End Namespace

