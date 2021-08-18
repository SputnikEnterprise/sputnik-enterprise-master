Namespace Employee.DataObjects.MonthlySalary

  ''' <summary>
  ''' LM_Doc data.
  ''' </summary>
  Public Class LMDocData

    Public Property ID As Integer
    Public Property RecordNumber As Integer?
    Public Property LMNr As Integer?
    Public Property DocDescription As String
    Public Property DocScan As Byte()
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String

  End Class

End Namespace