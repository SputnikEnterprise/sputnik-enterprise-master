

Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
'Imports SP.DatabaseAccess.Common.DataObjects

Namespace Listing

	''' <summary>
	''' Interface for Listing database access.
	''' </summary>
	Public Interface IListingDatabaseAccess


		''' <summary>
		''' Print documents
		''' </summary>
		Function LoadAssignedPrintJobData(ByVal mandantenNumber As Integer, ByVal language As String, ByVal jobnr As String) As PrintJobData
		Function LoadUserAndMandantData(ByVal mandantenNumber As Integer, ByVal userFirstname As String, ByVal userLastname As String) As UserAndMandantPrintData
		Function LoadUserInformationData(ByVal mandantenNumber As Integer, ByVal userNumber As Integer) As UserAndMandantPrintData
		Function AddAssignedPrintedDocumentInForEmployee(ByVal data As Employee.DataObjects.MasterdataMng.EmployeePrintedDocProperty) As Boolean


		''' <summary>
		''' Print ESR List
		''' </summary>
		Function LoadESRDataForPrintList(ByVal mdNr As Integer, ByVal diskIdentity As String) As IEnumerable(Of ESRListPrintData)
		Function LoadPaymentDataForESRPrintList(ByVal mdNr As Integer, ByVal firstPaymentNumber As List(Of Integer)) As IEnumerable(Of ESRPaymentListPrintData)
		Function LoadMandantDataForPrintDTAESRListData(ByVal mdNr As Integer, ByVal bankIDNumber As Integer?) As MandantDataForPrintDTAESRListing


		''' <summary>
		''' Mail-Listing
		''' </summary>
		Function LoadEMailData(ByVal messageID As String, ByVal jahr As Integer?, ByVal monat As Integer?, ByVal createdon As Date?) As IEnumerable(Of ListingMailData)
		Function GetAssignedMailData(ByVal CustomerID As String, ByVal recID As Integer?) As ListingMailData
		Function LoadAssignedEMailAttachmentData(ByVal CustomerID As String, ByVal messageID As String) As IEnumerable(Of ListingMailAttachmentData)
		Function LoadAssignedEMailAttachmentFile(ByVal CustomerID As String, ByVal id As Integer?) As Byte()
		Function AddEMailLogToContactTable(ByVal mdnr As Integer, ByVal data As ListingMailData) As Boolean
		Function AddEMailAttachmentLogToContactTable(ByVal mdnr As Integer, ByVal data As ListingMailAttachmentData) As Boolean


		''' <summary>
		''' sending eMail
		''' </summary>
		Function LoadTemplateDataToSendEmail(ByVal eMailType As String) As IEnumerable(Of EMailTemplateData)
		Function LoadOfferDataToSendEmail(ByVal eMailType As String) As IEnumerable(Of EMailOfferData)
		Function LoadCustomerDataToSendBulkEmail(ByVal userNumber As Integer, ByVal modulName As String) As IEnumerable(Of CustomerBulkEMailData)


		Function LoadAnnualLohnkontiData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer?, ByVal monthTo As Integer?, ByVal maNrList As String) As IEnumerable(Of ListingPayrollLohnkontiData)
		Function LoadAnnualNLAData(ByVal sql As String) As IEnumerable(Of ListingPayrollNLAData)


		''' <summary>
		''' outgoing eMail for Moduls
		''' </summary>
		Function LoadAssignedModulOutgoingEMailData(ByVal CustomerID As String, ByVal modulNumber As Integer?, ByVal number As Integer?) As IEnumerable(Of OutgoingEMailData)
		Function AddOutgoingEMailData(ByVal customer_ID As String, ByVal data As OutgoingEMailData) As Boolean



		''' <summary>
		''' Payroll print 
		''' </summary>
		Function LoadPayrollsPrintData(ByVal searchdata As PayrollSearchData) As IEnumerable(Of PayrollPrintData)
		Function LoadPayrollsDetailData(ByVal searchdata As PayrollSearchData) As IEnumerable(Of PayrollDetailData)
		Function DeletePayroll(ByVal mdNumber As Integer, ByVal payrollNumber As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeletePayrollResult


		''' <summary>
		''' Parifond-Controll list
		''' </summary>
		Function LoadESListData(ByVal tableName As String, ByVal reportNumbers As List(Of Integer)) As IEnumerable(Of ListingESListData)


		''' <summary>
		''' invoice print 
		''' </summary>
		Function LoadInvoiceData(ByVal searchData As InvoicePrintSearchConditionData) As IEnumerable(Of InvoiceData)
		Function LoadInvoiceForEMailSendingData(ByVal searchData As InvoicePrintSearchConditionData) As IEnumerable(Of InvoiceData)
		'Function LoadInvoiceData(ByVal mdNr As Integer, ByVal invoiceNumbers As Integer(), ByVal customerNumbers As Integer(), ByVal orderbyEnum As OrderByValue, ByVal WOSValueEnum As WOSSearchValue) As IEnumerable(Of InvoiceData)
		Function LoadAssignedAutomatedInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData)
		Function LoadAssignedCustomInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData)
		Function LoadAssignedCreditInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData)
		Function LoadAssignedRefundInvoicePrintData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer, ByVal customerLanguage As String) As IEnumerable(Of InvoicePrintData)
		Function LoadInvoiceReportData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of InvoiceReportData)
		Function LoadInvoiceReportLinesGroupedByKSTData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of InvoiceReportData)
		Function LoadInvoiceReportLinesGroupedData(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of InvoiceReportData)
		Function UpdateInvoiceDatabaseWithPrintDate(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As Boolean
		Function LoadReportDataForPrintedInvoice(ByVal mdNr As Integer, ByVal invoiceNumber As Integer) As IEnumerable(Of ReportForPrintInvoiceData)



		''' <summary>
		''' dunning print 
		''' </summary>
		Function LoadDunningData(ByVal mdNr As Integer, ByVal dunningLevel As Integer, ByVal dunningDate As Date, ByVal orderbyEnum As OrderByValue) As IEnumerable(Of DunningPrintData)
		Function LoadAssignedDunningDetailData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal dunningLevel As Integer, ByVal rName1 As String, ByVal dunningDate As Date) As IEnumerable(Of InvoiceDunningPrintData)
		Function LoadAssignedCustomerDunningPrintData(ByVal mdNr As Integer, ByVal dunningData As DunningPrintData, ByVal printNotDoneCredits As Boolean?, ByVal dunningLevel As Integer, ByVal dunningDate As Date
																				) As IEnumerable(Of InvoiceDunningPrintData)
		Function LoadAssignedCustomerDunningWithFeesPrintData(ByVal mdNr As Integer, ByVal dunningData As DunningPrintData, ByVal printNotDoneCredits As Boolean?, ByVal dunningLevel As Integer, ByVal dunningDate As Date
																				) As IEnumerable(Of InvoiceDunningPrintData)
		Function LoadInvoiceForAssignedDunningData(ByVal mdNr As Integer, ByVal dunningData As DunningPrintData, ByVal dunningLevel As Integer, ByVal dunningDate As Date) As IEnumerable(Of InvoiceData)


		Function AddWeeklyReportData(ByVal data As ReportWeeklyPrintData) As Boolean
		Function DeleteUserReportPrintData(ByVal data As ReportWeeklyPrintData) As Boolean
		Function LoadReportDataForPrinting(ByVal searchdata As RPSearchData) As IEnumerable(Of RPPrintData)


		''' <summary>
		''' list of employee and customer hours
		''' </summary>
		Function LoadCustomerExistingFirstPropertyData(ByVal mdNr As Integer) As IEnumerable(Of Customer.DataObjects.FirstPropertyData)
		Function LoadCustomerExistingContactInfo(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.CustomerContactData)
		Function LoadCustomerExistingStateData1(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.CustomerStateData)
		Function LoadCustomerExistingStateData2(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.CustomerStateData)


		Function LoadEmployeeExistingContactsInfo(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.EmployeeContactData)
		Function LoadEmployeeExistingStateData1(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.EmployeeStateData)
		Function LoadEmployeeExistingStateData2(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.EmployeeStateData)
		Function LoadEmployeeExistingContactReserveData(ByVal mdNr As Integer, ByVal contactReserveType As TableSetting.DataObjects.ContactReserveType) As IEnumerable(Of TableSetting.DataObjects.ContactReserveData)


		Function LoadEmploymentExistingCostCenter1Data(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.CostCenter1Data)
		Function LoadEmploymentExistingCostCenter2Data(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.CostCenter2Data)
		Function LoadEmploymentExistingAdvisorData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.AdvisorData)
		Function LoadEmploymentExistingCategorizationData(ByVal mdNr As Integer) As IEnumerable(Of ES.DataObjects.ESMng.ESCategorizationData)
		Function LoadEmploymentExistingSectorData(ByVal mdNr As Integer) As IEnumerable(Of TableSetting.DataObjects.SectorData)
		Function LoadEmploymentExistingPVLData(ByVal mdNr As Integer) As IEnumerable(Of ES.DataObjects.ESMng.ESSalaryData)


		Function SearchCustomerHoursReportlineData(ByVal mdNr As Integer, ByVal searchType As HourSearchTypeEnum, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of CustomertReportHoursData)
		Function SearchAssignedCustomerHoursReportlineData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of ReportlineHoursData)


		Function SearchEmployeeHoursReportlineData(ByVal mdNr As Integer, ByVal searchType As HourSearchTypeEnum, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of EmployeeReportHoursData)
		Function SearchAssignedEmployeeHoursReportlineData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of ReportlineHoursData)


		Function SearchCustomerHoursReportlineEachCostcenterData(ByVal mdNr As Integer, ByVal searchType As HourSearchTypeEnum, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of CustomertReportHoursData)
		Function SearchAssignedCustomerHoursReportlineEachCostcenterData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal CustomerCostcenter As Integer, ByVal reportKST As String, ByVal laNumber As Decimal?, ByVal usFiliale As String, ByVal searchData As HourSearchData) As IEnumerable(Of ReportlineHoursData)


		''' <summary>
		''' employment data
		''' </summary>
		Function LoadPermissionForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.PermissionData)
		Function LoadTaxForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Listing.DataObjects.QSTCodeData)
		Function LoadCountryForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CountryData)
		Function LoadNationalityForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CountryData)
		Function LoadCivilstateForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CivilStateData)
		Function LoadTaxCantonForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CantonData)
		Function LoadCustomerKSTForEmploymentData(ByVal mdNr As Integer) As IEnumerable(Of EmploymentCustomerCostcenterData)

		Function LoadActiveReportData(ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime, ByVal userKST As String) As IEnumerable(Of RPMasterData)
		Function AddReportFinishingFlagCheck(ByVal mdNr As Integer, ByVal esNumber As Integer, ByVal rpNumber As Integer, ByVal userInfo As String) As Boolean


		''' <summary>
		''' employee data
		''' </summary>
		Function LoadAllEmployeeMasterData(Optional includeImageData As Boolean = False) As IEnumerable(Of EmployeeMasterData)
		Function LoadAllEmployeeCountryCodeData() As IEnumerable(Of EmployeeMasterData)
		Function LoadAllEmployeeCommunityCodeData() As IEnumerable(Of EmployeeMasterData)
		Function LoadAllUnDefinedEmployeeCommunityLabelData() As IEnumerable(Of EmployeeMasterData)
		Function UpdateUnDefinedEmployeeCommunityData(ByVal employeeData As EmployeeMasterData) As Boolean
		Function UpdateEmployeeCommunityData(ByVal employeeData As EmployeeMasterData) As Boolean
		Function LoadAllEmployeeNationalityCodeData() As IEnumerable(Of EmployeeMasterData)
		Function UpdateEmployeeCountryData(ByVal oldCountryCode As String, ByVal newCountryCode As String) As Boolean
		Function UpdateEmployeeNationalityData(ByVal oldCountryCode As String, ByVal newCountryCode As String) As Boolean


		''' <summary>
		''' employee search
		''' </summary>
		''' <returns></returns>
		Function LoadAllEmployeeEmploymentTypeData() As IEnumerable(Of DataObjects.EmploymentTypeData)
		Function LoadAllEmployeeOtherEmploymentTypeData() As IEnumerable(Of DataObjects.EmploymentTypeData)
		Function LoadAllEmployeeTypeOfStayData() As IEnumerable(Of DataObjects.TypeOfStayData)
		Function LoadAllEmployeeForeignCategoryData() As IEnumerable(Of DataObjects.PermissionData)
		Function LoadAllEmployeeTaxCantonData() As IEnumerable(Of CantonData)
		Function LoadAllEmployeeBirthPlaceData() As IEnumerable(Of AnyStringValueData)


		Function LoadPermissionForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.PermissionData)
		Function LoadTaxForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Listing.DataObjects.QSTCodeData)
		Function LoadCommunityForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of QSTCommunityData)
		Function LoadCountryForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CountryData)
		Function LoadNationalityForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CountryData)
		Function LoadCivilstateForEmployeeSearchData(ByVal mdNr As Integer) As IEnumerable(Of Common.DataObjects.CivilStateData)


		''' <summary>
		''' customer search
		''' </summary>
		''' <returns></returns>
		Function LoadAllCustomerMasterData() As IEnumerable(Of CustomerMasterData)
		Function LoadAllCustomerCountryCodeData() As IEnumerable(Of CustomerMasterData)
		Function UpdateCustomerCountryData(ByVal oldCountryCode As String, ByVal newCountryCode As String) As Boolean


		' TWIX
		Function LoadAllTwixCustomerMasterData(ByVal mdNr As Integer) As IEnumerable(Of CustomerMasterData)


		''' <summary>
		''' PVL-List data
		''' </summary>
		Function LoadInkassopoolPauschale(ByVal mdNr As Integer, ByVal gavBeruf As String, ByVal gavcanton As String, ByVal firstmonat As Integer, ByVal lastmonat As Integer, ByVal year As Integer, ByVal employeenumbers As String) As Decimal?
		Function LoadGEFAKPauschale(ByVal mdNr As Integer, ByVal gavBeruf As String, ByVal firstmonat As Integer, ByVal lastmonat As Integer, ByVal year As Integer, ByVal employeenumbers As String) As Decimal?



		''' <summary>
		''' SUVA-Std data
		''' </summary>
		Function LoadEmployeesForSelectionData(ByVal searchData As EmployeeSuvaSearchData) As IEnumerable(Of Employee.DataObjects.MasterdataMng.EmployeeMasterData)
		Function LoadEmployeeAllReportData(ByVal searchData As EmployeeSuvaSearchData, ByVal usFiliale As String) As IEnumerable(Of RPMasterData)
		Function AddEmployeeSUVAWeekDaysData(ByVal mdnr As Integer, ByVal data As SuvaWeekData) As Boolean
		Function LoadEmployeeSUVAHourData(ByVal searchData As EmployeeSuvaSearchData) As IEnumerable(Of SuvaTableListData)


		Function LoadRPPrintWeeklyData(ByVal MDNr As Integer, ByVal printYear As Integer, ByVal firstWeek As Integer, ByVal lastWeek As Integer, ByVal userNumber As Integer) As IEnumerable(Of RPPrintWeeklyData)
		Function UpdateAssignedReportWithPrintData(ByVal MDNr As Integer, ByVal printData As RPPrintWeeklyData, ByVal userNumber As Integer) As Boolean


		' QST-Listing
		Function LoadQSTCantonMasterData(ByVal searchdata As PayrollListingSearchData) As QSTCantonMasterData
		Function LoadQSTYearData(ByVal mdNr As Integer) As IEnumerable(Of Integer)
		Function LoadQSTMonthData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of Integer)
		Function LoadQSTLAData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?) As IEnumerable(Of QSTLAData)
		Function LoadQSTCantonData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal laNumbers As String) As IEnumerable(Of CantonData)
		Function LoadQSTCommunityData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal laNumbers As String, ByVal canton As String) As IEnumerable(Of QSTCommunityData)
		Function LoadTaxCountryCodeData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String) As IEnumerable(Of Common.DataObjects.CountryData)
		Function LoadTaxNationalityCodeData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String) As IEnumerable(Of Common.DataObjects.CountryData)
		Function LoadQSTCodeData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String) As IEnumerable(Of QSTCodeData)
		Function LoadQSTPermissionData(ByVal mdNr As Integer, ByVal year As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer?, ByVal canton As String, ByVal qstCode As String) As IEnumerable(Of QSTPermissionData)

		Function DeleteAllUserTaxData(ByVal mdNr As Integer, ByVal userNumber As Integer) As Boolean
		Function AddTaxListDataToTaxData(ByVal listData As TaxListTableData) As Boolean


		Function LoadSearchResultOfTaxData(ByVal mdNr As Integer, ByVal userNr As Integer) As IEnumerable(Of SearchRestulOfTaxData)


#Region "delete employments"

		Function LoadAssignedEmploymentsData(ByVal mdNr As Integer, ByVal esNumbers As List(Of Integer)) As IEnumerable(Of ESMasterData)
		Function LoadAssignedEmploymentDependentData(ByVal mdNr As Integer, ByVal esNumber As Integer) As IEnumerable(Of EmploymentDependentData)

#End Region

#Region "Import data from another database"

		Function DeleteAssignedImportedEmployeeData(ByVal employeeNumber As Integer) As Boolean

		Function AddAssignedEmployeeMasterDataFromAnotherDatabase(ByVal employee As EmployeeTranferData) As Boolean
		Function AddAssignedEmployeePeripherieDataFromAnotherDatabase(ByVal employee As EmployeeTranferData) As Boolean
		Function UpdateAssignedEmployeePeripherieDataFromAnotherDatabase(ByVal employee As EmployeeTranferData) As Boolean


		Function AddAssignedCustomerMasterDataFromAnotherDatabase(ByVal customer As CustomerTranferData) As Boolean
		Function AddAssignedCResponsibleDataFromAnotherDatabase(ByVal cResponsible As CResponsiblePersonTranferData) As Boolean
		Function AddAssignedCustomerPeripherieDataFromAnotherDatabase(ByVal customer As CustomerTranferData) As Boolean
		Function UpdateAssignedCustomerPeripherieDataFromAnotherDatabase(ByVal customer As CustomerTranferData) As Boolean

		Function AddAssignedCResponsiblePeripherieDataFromAnotherDatabase(ByVal cResponsible As CResponsiblePersonTranferData) As Boolean
		Function UpdateAssignedCResponsiblePeripherieDataFromAnotherDatabase(ByVal cResponsible As CResponsiblePersonTranferData) As Boolean

		Function DeleteAssignedImportedCustomerData(ByVal customerNumber As Integer) As Boolean

#End Region


#Region "Employee Credits"

		Function LoadFlexibleWorkingHoursData(ByVal mdNr As Integer, ByVal employeeNumber As Integer) As EmployeeCreditData
		Function LoadFlexibleWorkingHoursForPayrollData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal year As Integer, ByVal month As Integer) As EmployeeCreditData

#End Region


#Region "down time"

		Function LoadDowntimeCustomerData(ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of CustomerMasterData)
		Function LoadDowntimeDataData(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of DowntimeData)

#End Region


#Region "invoice search"

		Function LoadCustomerDataForCreditlimits(ByVal mdNr As Integer, ByVal filiale As String) As IEnumerable(Of CustomerCreditData)
		Function LoadInvoiceDataForFinishedCredits(ByVal mdNr As Integer, ByVal invoiceDeadlineDate As Date?, ByVal invoiceFromDate As Date?, ByVal invoiceToDate As Date?, ByVal createdFromDate As Date?, ByVal createdToDate As Date?) As IEnumerable(Of InvoiceData)
		Function LoadInvoiceDataForNOTFinishedCredits(ByVal mdNr As Integer, ByVal invoiceDeadlineDate As Date?, ByVal invoiceFromDate As Date?, ByVal invoiceToDate As Date?, ByVal createdFromDate As Date?, ByVal createdToDate As Date?) As IEnumerable(Of InvoiceData)
		Function LoadInvoiceOpenAmountData(ByVal mdNr As Integer, ByVal whereQuery As String, ByVal firstOpenAmount As Decimal?, ByVal secondOpenAmount As Decimal?) As IEnumerable(Of CustomerOpenAmountData)

		Function LoadPaymentReminderCodeInInvoicesData() As IEnumerable(Of PaymentReminderCodeData)
		Function LoadInvoiceArtCodeInInvoicesData() As IEnumerable(Of InvoiceArtData)

#End Region

#Region "DB1 Listing"

		Function LoadDB1LOLAmount(ByVal query As String) As Decimal?
		Function LoadDB1PayrollData(ByVal query As String, ByVal dataType As DB1DataRecordType?) As IEnumerable(Of DB1PayrollData)
		Function LoadPayrollAGData(ByVal payrollNumber As Integer?, ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?, ByVal esNumber As Integer?) As DB1PayrollAGAnteilData

#End Region



#Region "ExportData"

		Function LoadEmploymentsEmployeeData() As IEnumerable(Of EmployeeMasterData)
		Function LoadInvoicesCustomerData() As IEnumerable(Of CustomerMasterData)
		Function LoadEmploymentAgreementData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of EmploymentAgreementData)
		Function LoadEmploymentReportsForGivenTimeperiodData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of ListingESListData)
		Function LoadReportNumberData(ByVal mdnr As Integer, ByVal year As Integer?, ByVal WithScans As Boolean) As IEnumerable(Of Integer)
		Function LoadReportScanData(ByVal rpNr As Integer?, ByVal id As Integer?, ByVal includeFileBytes As Boolean) As IEnumerable(Of ReportScanData)
		Function LoadEmployeePayrollData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollPrintData)
		Function LoadEmployeesZVGroupByMonthYearData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollPrintData)
		Function LoadEmployeesArgbGroupByYearData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollPrintData)
		Function LoadEmployeesNLAGroupByYearData(ByVal mdnr As Integer, ByVal year As Integer?, ByVal manrListe As String) As IEnumerable(Of PayrollNLAData)
		Function LoadPayrollEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of PayrollEvaluationListData)
		Function LoadAdvancedpaymentEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of AdvancedpaymentEvaluationListData)

		Function LoadInvoiceForExportData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of Integer)
		Function LoadInvoiceEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of Invoice.DataObjects.Invoice)
		Function LoadPaymentEvaluationData(ByVal mdnr As Integer, ByVal year As Integer?) As IEnumerable(Of Invoice.DataObjects.PaymentMasterData)

#End Region


	End Interface


	Public Enum OrderByValue

		OrderByInvoiceNumber
		OrderByCustomerName
		OrderByInvoiceDate

	End Enum

	Public Enum WOSSENDValue

		PrintWithoutSending
		PrintOtherSendWOS
		PrintAndSend

	End Enum

	Public Enum WOSSearchValue

		SearchWOSCustomer
		SearchNOTWOSCustomer
		SearchAllCustomer

	End Enum

	Public Enum WhatToPrintValue

		DetailAndEZ
		Detail
		EZ

	End Enum

End Namespace


