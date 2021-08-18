Namespace Customer.DataObjects

  ''' <summary>
  ''' Customer assigned credit info data (KD_KreditInfo).
  ''' </summary>
  Public Class CustomerAssignedCreditInfo
    Public Property ID As Integer
    Public Property CustomerNumber As Integer?
    Public Property RecordNumber As Integer?
    Public Property FromDate As DateTime?
    Public Property Description As String
    Public Property ActiveRec As Boolean?
    Public Property ToDate As DateTime?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property DV_ArchiveID As String
    Public Property DV_DecisionID As Byte?
    Public Property DV_DecisionText As String
    Public Property DV_PDFFile As Byte()
    Public Property HasPDFFileFlag As Boolean
    Public Property USNr As Integer?
    Public Property DV_QueryType As Integer?
    Public Property DV_FoundedAddress As String
    Public Property DV_FoundedAddressID As String
  End Class

End Namespace
