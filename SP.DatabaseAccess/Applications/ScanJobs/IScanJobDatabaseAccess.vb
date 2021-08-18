
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.ScanJob.DataObjects


Namespace ScanJob


	''' <summary>
	''' Interface for ScanJob database access.
	''' </summary>
	Public Interface IScanJobDatabaseAccess

		Function AddEmployeeScanJobDocument(ByVal documentData As ScanJobData) As Boolean
		Function AddEmploymentEmployeeScanJobDocument(ByVal documentData As ScanJobData) As Boolean
		Function LoadEmployeeReportLineDataForScanJobDocument(ByVal mdNr As Integer, ByVal savescanwithZeroAmount As Boolean, ByVal documentData As ScanJobData) As IEnumerable(Of ReportLineData)
		Function AddPayrollEmployeeScanJobDocument(ByVal documentData As ScanJobData) As Boolean


		Function AddCustomerScanJobDocument(ByVal documentData As ScanJobData) As Boolean
		Function AddEmploymentCustomerScanJobDocument(ByVal documentData As ScanJobData) As Boolean
		Function AddInvoiceCustomerScanJobDocument(ByVal documentData As ScanJobData) As Boolean

		Function LoadCustomerReportLineDataForScanJobDocument(ByVal mdNr As Integer, ByVal savescanwithZeroAmount As Boolean, ByVal documentData As ScanJobData) As IEnumerable(Of ReportLineData)
		Function AddReportScanJobDocument(ByVal documentData As ScanJobData, ByVal reportLineData As ReportLineData) As Boolean
		Function AddReportScanDocumentIntoScanJobDb(ByVal documentData As ScanJobData) As Boolean

		Function LoadDropInDataForApplication(ByVal customerID As String, ByVal dropInFilename As String) As ApplicationData

	End Interface

End Namespace
