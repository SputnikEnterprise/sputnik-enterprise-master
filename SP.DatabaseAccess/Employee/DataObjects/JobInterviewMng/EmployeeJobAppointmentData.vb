Namespace Employee.DataObjects.JobInterviewMng

  ''' <summary>
  ''' Job appointment data (MA_JobTermin). 
  ''' </summary>
  Public Class EmployeeJobAppointmentData

    Public Property ID As Integer
    Public Property RecordNumber As Integer?
    Public Property JobTitle As String
    Public Property EmployeeNumber As Integer?
    Public Property AppointmentDate As DateTime?
    Public Property CustomerNumber As Integer?
    Public Property Company As String
    Public Property AppointmentWith As String
    Public Property Location As String
    Public Property Telephone As String
    Public Property Telefax As String
    Public Property Homepage As String
    Public Property eMail As String
    Public Property Outcome As String ' == Ergebnis
    Public Property JobAppointmentState As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property Result As String
    Public Property ProposeNr As Integer?
    Public Property VakNr As Integer?
    Public Property OfNr As Integer?
    Public Property ResponsiblePersonNumber As Integer?

  End Class

End Namespace
