
Imports SP.DatabaseAccess.Invoice.DataObjects

Namespace Invoice

	''' <summary>
	''' Interface for Invoice (Debitorenbuchaltung) database access.
	''' </summary>
	Public Interface IInvoiceDatabaseAccess


		Function LoadInvoice(ByVal invoiceNumber As Integer) As DataObjects.Invoice
		Function LoadInvoiceWithOpenAmount(ByVal mdNumber As Integer) As IEnumerable(Of DataObjects.Invoice)
		Function ReloadInvoiceValues(ByVal invoiceData As DataObjects.Invoice) As Boolean
		Function AddNewInvoice(ByVal invoiceData As DataObjects.Invoice, ByVal reNumberOffset As Integer) As Boolean
		Function UpdateInvoice(ByVal invoiceData As DataObjects.Invoice) As Boolean
		Function LoadInvoiceIndividual(ByVal invoiceNumber As Integer) As List(Of DataObjects.InvoiceIndividual)
		Function AddNewInvoiceIndividual(ByVal invoiceRowData As DataObjects.InvoiceIndividual, ByVal refereceNumbersTo10 As Boolean) As Boolean
		Function UpdateInvoiceIndividual(ByVal invoiceRowData As DataObjects.InvoiceIndividual, ByVal refereceNumbersTo10 As Boolean) As Boolean
		Function UpdateInvoiceReferenceNumbers(ByVal reNr As Integer, ByVal refNrTo10 As Boolean) As Boolean
		Function UpdateDunningAndArrearReferenceNumbers(ByVal mdNr As Integer, ByVal reNr As Integer, ByVal kdNr As Integer, ByVal betragkInk As Decimal, ByVal refNrTo10 As Boolean) As Boolean
		Function DeleteInvoiceIndividual(ByVal invoiceRowData As DataObjects.InvoiceIndividual, ByVal modul As String, ByVal username As String, ByVal usnr As Integer, ByVal refereceNumbersTo10 As Boolean) As DeleteREIndResult
		Function DeleteInvoice(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteREResult
		Function DeleteInvoiceAndInsertInvoiceDocumentIntoDeleteDb(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer, ByVal invoiceDocument As Byte()) As DeleteREResult
		Function LoadInvoiceRPT(ByVal invoiceNumber As Integer, ByVal KDNr As Integer, ByVal lang As String) As List(Of DataObjects.InvoiceRPL)
		Function UpdateInvoiceRPT(ByVal invoiceRowData As DataObjects.InvoiceRPL) As Boolean
		Function LoadBankData(ByVal MDNr As Integer) As List(Of DataObjects.BankData)

		Function LoadCustomerData() As List(Of DataObjects.Customer)
		Function LoadCustomerReAddressData(ByVal KDNr As Integer) As List(Of DataObjects.CustomerReAddress)

		Function LoadConflictedMonthCloseRecordsInPeriod(ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime,
																										 ByRef resultCode As Integer) As IEnumerable(Of DataObjects.ConflictedMonthCloseData)

		Function LoadFoundedPaymentForInvoiceMng(invoiceNumber As Integer?) As IEnumerable(Of DataObjects.InvoicePaymentProperty)


		Function LoadCustomerDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?) As IEnumerable(Of DataObjects.CustomerOverviewAutomatedInvoiceData)
		Function LoadReportDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal reportNumber As Integer?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportCostcenterDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal reportNumber As Integer?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadEmploymentDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal employmentNumber As Integer?) As IEnumerable(Of DataObjects.EmploymentOverviewAutomatedInvoiceData)
		Function LoadReportLineDataForSearchAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?) As IEnumerable(Of DataObjects.ReportLineOverviewAutomatedInvoiceData)

		''' <summary>
		''' report data "R"
		''' </summary>
		Function LoadReportDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal closedMonth As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportLineDataWithReportNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?) As DataObjects.ReportLineCreatingAutomatedInvoiceData
		Function LoadReportLineDataWithCustomerNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As DataObjects.ReportLineCreatingAutomatedInvoiceData


		''' <summary>
		''' employment data "E"
		''' </summary>
		Function LoadEmploymentDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByMonth As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportLineDataWithEmploymentNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal employmentNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As DataObjects.ReportLineCreatingAutomatedInvoiceData


		''' <summary>
		''' customer data "K"
		''' </summary>
		Function LoadCustomerDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByMonth As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)



		''' <summary>
		''' cost center data "S"
		''' </summary>
		Function LoadCostcenterDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByReportNumber As Boolean?, ByVal groupByMonth As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportLineDataWithCostcenterNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal kstNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As DataObjects.ReportLineCreatingAutomatedInvoiceData



		''' <summary>
		''' weekly data "E_W"
		''' </summary>
		Function LoadWeeklyDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByEmploymentNumber As Boolean?, ByVal groupByReportNumber As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportLineDataWithWeeklyEmploymentAndReportNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal employmentNumber As Integer?, ByVal weekNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As DataObjects.ReportLineCreatingAutomatedInvoiceData
		Function LoadWeeklyCostcenterDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByReportNumber As Boolean?, ByVal closedMonth As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportLineDataWithWeeklyCostCenterNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal kstNumber As Integer?, ByVal weekNumber As Integer?, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal closedMonth As Boolean?) As DataObjects.ReportLineCreatingAutomatedInvoiceData


		Function LoadWeeklyCostcenterAddressDataForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal groupByReportNumber As Boolean?) As IEnumerable(Of DataObjects.ReportOverviewAutomatedInvoiceData)
		Function LoadReportLineDataWithWeeklyCostCenterAddressNumberForCreatingAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal reportNumber As Integer?, ByVal weekNumber As Integer?, ByVal addressNumber As Integer?, ByVal jahr As Integer?) As DataObjects.ReportLineCreatingAutomatedInvoiceData

		Function LoadCustomerInvoiceDataForAutomatedInvoices(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal kstNumber As Integer?, ByVal addressNumber As Integer?) As DataObjects.CustomerReAddress
		Function UpdateReportlineInoviceNumbers(ByVal invoiceNumber As Integer, ByVal reportlineIDs As String) As Boolean
		Function DeleteStraightCreatedInvoices(ByVal mdNr As Integer, ByVal invoiceNumbers As Integer()) As Boolean


		''' <summary>
		''' creating dunning for invoice
		''' </summary>
		Function LoadDunningDatesData(ByVal mdNumber As Integer, ByVal dunningLevel As Integer, ByVal mahnDate As Date?) As IEnumerable(Of DataObjects.Invoice)

		Function LoadInvoiceDataForCreatingKontoauszug(ByVal mdNumber As Integer, ByVal zeUntil As Date) As IEnumerable(Of DataObjects.Invoice)
		Function LoadInvoiceDataForCreatingFirstDunning(ByVal mdNumber As Integer, ByVal zeUntil As Date) As IEnumerable(Of DataObjects.Invoice)
		Function LoadInvoiceDataForCreatingSecondDunning(ByVal mdNumber As Integer, ByVal zeUntil As Date) As IEnumerable(Of DataObjects.Invoice)
		Function LoadInvoiceDataForCreatingThirdDunning(ByVal mdNumber As Integer, ByVal zeUntil As Date, ByVal createDunningAgain As Boolean?) As IEnumerable(Of DataObjects.Invoice)
		Function LoadDunningDateData(ByVal mdNr As Integer, ByVal dunningLevel As Integer) As IEnumerable(Of DataObjects.DunningDateData)


		Function UpdateInvoiceDataForKontoauszug(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date) As Boolean
		Function UpdateInvoiceDataForFirstDunning(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date, ByVal dunningAmountFromSetting As Integer, ByVal dunningAmount As Decimal, ByVal verzugAmountperDay As Decimal) As Boolean
		Function UpdateInvoiceDataForSecondDunning(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date, ByVal dunningAmountFromSetting As Integer, ByVal dunningAmount As Decimal, ByVal verzugAmountperDay As Decimal) As Boolean
		Function UpdateInvoiceDataForThirdDunning(ByVal mdNumber As Integer, ByVal invoiceNumber As Integer, ByVal zeUntil As Date, ByVal dunningDate As Date) As Boolean

		Function AddNewDunning(ByVal mdNumber As Integer, ByVal invoiceData As DataObjects.DunningAndArrearsData) As Boolean


		Function DeleteStraightCreatedDunning(ByVal mdNumber As Integer, ByVal invoiceNumbers As Integer(), ByVal dunningLevel As Integer) As Boolean
		Function DeleteCreatedDunning(ByVal mdNumber As Integer, ByVal dunningData As Listing.DataObjects.DunningPrintData, ByVal dunningLevel As Integer, ByVal dunningDate As Date?) As Boolean



#Region "payment"

		Function LoadPaymentData(ByVal mdNr As Integer, ByVal paymentNumber As Integer) As PaymentMasterData
		Function AddNewPayment(ByVal initData As NewPaymentInitData) As Boolean
		Function UpdatePaymentMasterData(ByVal paymentNumber As Integer, ByVal paymentData As PaymentMasterData) As Boolean
		Function DeleteAssingedPaymentData(ByVal mdNr As Integer, ByVal paymentNumber As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteREResult

#End Region


	End Interface

	''' <summary>
	''' Result of RE deletion.
	''' </summary>
	Public Enum DeleteREResult
		ResultDeleteOk = 1
		ResultCanNotDeleteBecauseMonthIsClosed = 2
		ResultCanNotDeleteBecauseOfExistingZE = 3
		ResultCanNotDeleteBecauseOfPartlyPayed = 4
		ResultDeleteError = 5
	End Enum

	''' <summary>
	''' Result of REInd deletion.
	''' </summary>
	Public Enum DeleteREIndResult
		ResultDeleteOk = 1
		ResultCanNotDeleteBecauseMonthIsClosed = 2
		ResultCanNotDeleteBecauseOfExistingZE = 3
		ResultCanNotDeleteBecauseOfPartlyPayed = 4
		ResultDeleteError = 5
	End Enum

End Namespace