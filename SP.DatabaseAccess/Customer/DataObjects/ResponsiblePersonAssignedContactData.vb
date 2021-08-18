Namespace Customer.DataObjects

  ''' <summary>
  ''' Responsible person assigned contact data (KD_KontaktTotal)
  ''' </summary>
  Public Class ResponsiblePersonAssignedContactData

    Public Property ID As Integer
    Public Property CustomerNumber As Integer?
    Public Property ResponsiblePersonNumber As Integer?
    Public Property ContactDate As DateTime?
    Public Property ContactsString As String
    Public Property Username As String
    Public Property RecordNumber As Integer?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedUserNumber As Integer?
		Public Property ContactType1 As String
		Public Property ContactType2 As Short?
    Public Property ContactPeriodString As String
    Public Property ContactImportant As Boolean?
    Public Property ContactFinished As Boolean?
    Public Property MANr As Integer?
    Public Property ProposeNr As Integer?
    Public Property VacancyNumber As Integer?
    Public Property OfNumber As Integer?
    Public Property Mail_ID As Integer?
    Public Property TaskRecNr As Integer?
    Public Property UsNr As Integer?
    Public Property ESNr As Integer?
    Public Property KontaktDocID As Integer?

    ' MAKontaktRecID
    Public Property EmployeeContactRecID As Integer?
    Public Property EmployeeContactEmployeeNr As Integer?
    Public Property EmplyoeeContactRecNr As Integer?

    Public Function ShallowCopy() As ResponsiblePersonAssignedContactData
      Return DirectCast(Me.MemberwiseClone(), ResponsiblePersonAssignedContactData)
    End Function

  End Class

End Namespace