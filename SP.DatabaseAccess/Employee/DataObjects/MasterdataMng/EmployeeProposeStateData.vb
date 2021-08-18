Namespace Employee.DataObjects.MasterdataMng

  Public Class EmployeeProposeStateData
		Public Property ProposeNumber As Integer
		Public Property State As Integer
		Public Property ProposeCreatedOn As DateTime?
    Public Property ProposeDescription As String
    Public Property Customer As String

    Public ReadOnly Property EmployeeProposeStateResult
      Get
        Return CType(State, EmployeeProposeStateResult)
      End Get
    End Property

  End Class

End Namespace
