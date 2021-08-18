Namespace Customer.DataObjects

  ''' <summary>
  ''' Customer assigned invoice data (RE). 
  ''' </summary>
  Public Class CustomerAssignedInvoiceData
    Public Property ID As Integer
    Public Property InvoiceNumber As Integer?
    Public Property InvoiceDate As DateTime?
    Public Property DueDate As DateTime?
		Public Property AmountEx As Decimal?
		Public Property AmountInk As Decimal?
		Public Property AmountPayed As Decimal?
    Public Property OpenAmount As Decimal?
		Public Property zFiliale As String

  End Class

End Namespace