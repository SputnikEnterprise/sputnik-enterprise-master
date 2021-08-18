Namespace Customer.DataObjects

  ''' <summary>
  ''' Customer assigned invoice address data (KD_RE_Address)
  ''' </summary>
  Public Class CustomerAssignedInvoiceAddressData
    Public Property ID As Integer
    Public Property CustomerNumber As Integer?
    Public Property RecordNumber As Short
    Public Property InvoiceCompany As String
    Public Property InvoiceCompany2 As String
    Public Property InvoiceCompany3 As String
    Public Property InvoiceForTheAttentionOf As String
    Public Property InvoicePostOfficeBox As String
    Public Property InvoiceStreet As String
    Public Property InvoicePostcode As String
    Public Property InvoiceLocation As String
    Public Property InvoiceCountryCode As String
		Public Property InvoiceEMailAddress As String
		Public Property InvoiceSendAsZip As Boolean?
		Public Property KSTDescription As String
		Public Property CurrencyCode As String
    Public Property Active As Boolean?
    Public Property InvoiceDepartment As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property PaymentCondition As String
    Public Property ReminderCode As String
  End Class
End Namespace
