

Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.WOS.DataObjects


Namespace WOS

	''' <summary>
	''' Interface for WOS database access.
	''' </summary>
	Public Interface IWOSDatabaseAccess

		Function UpdateEmployeeGuidData(ByVal employeeNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateEmployeeWebExportData(ByVal employeeNumber As Integer, ByVal value As Boolean) As Boolean
		Function UpdateCustomerGuidData(ByVal customerNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateCustomerResponsibleGuidData(ByVal customerNumber As Integer, ByVal recNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateEmploymentGuidData(ByVal esNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateCustomerEmploymentGuidData(ByVal esNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateReportGuidData(ByVal rpNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateReportLineGuidData(ByVal reportNumber As Integer, ByVal reportlineNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdatePayrollGuidData(ByVal loNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateInvoiceGuidData(ByVal reNumber As Integer, ByVal newGuid As String) As Boolean
		Function UpdateProposeGuidData(ByVal proposeNumber As Integer, ByVal newGuid As String) As Boolean


		Function LoadRPLineData(ByVal rpNr As Integer, ByVal rplDataType As RPLType, ByVal rpLineNumber As Integer?) As RPLListData


	End Interface

End Namespace


