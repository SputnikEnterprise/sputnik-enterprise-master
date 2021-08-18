Namespace Employee.DataObjects.MasterdataMng

  Public Class EmployeeESStateData

		Public Property ESNumber As Integer
		Public Property State As Integer
    Public Property Last_Es_Ab As DateTime?
    Public Property Last_Es_Ende As DateTime?
    Public Property Last_Es_Als As String
    Public Property Customer As String

    Public ReadOnly Property EmployeeESStateResult
      Get
        Return CType(State, EmployeeESStateResult)
      End Get
    End Property
  End Class

End Namespace
