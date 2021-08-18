Namespace Employee.DataObjects.ContactMng

  ''' <summary>
  ''' Employee contact data (MA_Kontakte)
  ''' </summary>
  Public Class EmployeeContactData

    Public Property ID As Integer
    Public Property EmployeeNumber As Integer?
    Public Property ContactsString As String
    Public Property RecordNumber As Integer?
    Public Property ContactType1 As String
    Public Property ContactType2 As Short?
    Public Property ContactDate As DateTime?
    Public Property ContactPeriodString As String
    Public Property ContactImportant As Boolean?
    Public Property ContactFinished As Boolean?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedUserNumber As Integer?
		Public Property ProposeNr As Integer?
		Public Property VacancyNumber As Integer?
    Public Property OfNumber As Integer?
    Public Property Mail_ID As Integer?
    Public Property TaskRecNr As Integer?
    Public Property UsNr As Integer?
    Public Property ESNr As Integer?
    Public Property CustomerNumber As Integer?
    Public Property CustomerContactRecId As Integer?
    Public Property CustomerContactKDNr As Integer?
    Public Property CustomerContactRecNr As Integer?
    Public Property KontaktDocID As Integer?


  End Class

End Namespace