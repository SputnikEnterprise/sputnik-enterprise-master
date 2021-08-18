Namespace Customer.DataObjects

  ''' <summary>
  ''' Responsible person assigned document data (KD_ZDoc)
  ''' </summary>
  Public Class ResponsiblePersonAssignedDocumentData

    Public Property ID As Integer
    Public Property DocumentRecordNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property ResponsiblePersonRecordNumber As Integer?
    Public Property DocPath As String
    'Name -> Bezeichnung
    Public Property Name As String
    Public Property Description As String
    Public Property ScanExtension As String
    Public Property FileFullPath As String
    Public Property USNr As Integer?
    Public Property CategorieNumber As Integer?
    Public Property TranslatedSalutation As String
    Public Property Firstname As String
    Public Property Lastname As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
  End Class

End Namespace