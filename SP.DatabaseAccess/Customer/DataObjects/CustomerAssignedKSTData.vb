Namespace Customer.DataObjects

  ''' <summary>
  ''' Customer assigned KST data.
  ''' </summary>
  Public Class CustomerAssignedKSTData
    Public Property ID As Integer
    Public Property RecordNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property InvoiceAddressRecordNumber As Integer?
    Public Property Description As String
    Public Property Result As String
    Public Property EmploymentPostCode As String
    Public Property BKPostCode As String
    Public Property Info1 As String
    Public Property Info2 As String

    Public Property InvoiceAddressInfo As String

  End Class

End Namespace
