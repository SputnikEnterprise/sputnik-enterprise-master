Namespace Listing.DataObjects

	Public Class EmploymentCustomerCostcenterData

		Public Property CostcenterLabel As String

	End Class

	Public Class EmploymentAgreementData
		Public Property EmploymentNumber As Integer
		Public Property EmployeeNumber As Integer
		Public Property CustomerNumber As Integer
		Public Property CResponsibleNumber As Integer

		Public Property EmployeeWOS As Boolean
		Public Property CustomerWOS As Boolean

		Public Property EmployeeLanguage As String
		Public Property CustomerLanguage As String

	End Class

	Public Class ReportScanData
		Public Property ID As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property EmploymentNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property RPLNr As Integer?
		Public Property FileBytes As Byte()
		Public Property Bezeichnung As String
		Public Property Extenstion As String

	End Class

End Namespace
