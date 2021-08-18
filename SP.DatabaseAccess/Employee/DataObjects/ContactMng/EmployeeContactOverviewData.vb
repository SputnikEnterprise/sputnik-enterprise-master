Namespace Employee.DataObjects.ContactMng

  ''' <summary>
  ''' Employee contact overview data (MA_Kontakte).
  ''' </summary>
  Public Class EmployeeContactOverviewdata
    Public Property ID As Integer
    Public Property EmployeeNumber As Integer?
    Public Property RecNr As Integer?
    Public Property ContactDate As DateTime?
    Public Property PersonOrSubject As String
    Public Property Description As String
    Public Property IsImportant As Boolean?
    Public Property IsCompleted As Boolean?
    Public Property CreatedFrom As String
    Public Property DocumentID As Integer?
    Public Property KDKontactRecID As Integer?

    Public Property minContactDate As DateTime?
    Public Property maxContactDate As DateTime?

  End Class

End Namespace
