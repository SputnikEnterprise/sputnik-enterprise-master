Namespace Employee.DataObjects.ContactMng

  ''' <summary>
  ''' Propose data (Propose).
  ''' </summary>
  Public Class ProposeDataForContactMng
    Public Property ID As Integer
    Public Property EmployeeNumber As Integer?
    Public Property ProposeNumber As Integer?
    Public Property Description As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property P_State As String

		Public Property Customername As String

	End Class

End Namespace