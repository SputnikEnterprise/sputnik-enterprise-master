
Imports System.Collections.Generic


Namespace RichEditSendMail

	Public Class PreselectionInvoiceEMailMergeData
		Public Property MergeData As List(Of InvoiceEMailMergeData)

	End Class

	Public Class InvoiceEMailMergeData
		Public Property NumberOfInvoices As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property Companyname As String
		Public Property REEMail As String
		Public Property InvoiceDate As Date?
		Public Property Attachment As String
		Public Property IndividualAttachments As List(Of String)
		Public Property InvoiceNumbers As List(Of Integer)

	End Class

	Public Class PreselectionPayrollEMailMergeData
		Public Property MergeData As List(Of PayrollEMailMergeData)

	End Class

	Public Class PayrollEMailMergeData
		Public Property NumberOfPayrolls As Integer?
		Public Property PayrollNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property EMail As String
		Public Property PayrollDate As Date?
		Public Property Attachment As String
		Public Property IndividualAttachments As List(Of String)
		Public Property PayrollNumbers As List(Of Integer)

		Public ReadOnly Property Employeefullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property EmployeefullnameWithComma As String
			Get
				Return String.Format("{1}, {0}", Firstname, Lastname)
			End Get
		End Property

	End Class

	Public Enum ProcessState
		Unprocessed
		InProcessing
		Processed
		Failed
	End Enum


End Namespace
