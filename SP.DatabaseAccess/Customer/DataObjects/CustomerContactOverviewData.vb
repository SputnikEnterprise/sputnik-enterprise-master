Namespace Customer.DataObjects

  ''' <summary>
  ''' Customer contact overview total data.
  ''' </summary>
  Public Class CustomerContactOverviewData

    Public Property ID As Integer
    Public Property CustomerNumber As Integer?
    Public Property ResponsiblePersonRecordNumber As Integer?
    Public Property RecNr As Integer?
    Public Property ContactDate As DateTime?
    Public Property minContactDate As DateTime?
    Public Property maxContactDate As DateTime?
    Public Property PersonOrSubject As String
    Public Property Description As String
    Public Property IsImportant As Boolean?
    Public Property IsCompleted As Boolean?
    Public Property Creator As String
    Public Property DocumentID As Integer?

  End Class

End Namespace


