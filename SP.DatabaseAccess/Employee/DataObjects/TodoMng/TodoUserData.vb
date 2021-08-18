
Namespace Employee.DataObjects.TodoMng

	''' <summary>
	''' Todo user data(tblTODOList)
	''' </summary>
	Public Class TodoUserData

		Public Property Customer_ID As String
		Public Property ID As Integer?
		Public Property FK_ToDoID As Integer?
		Public Property UserNumber As Integer
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Done As Boolean
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String


		Public ReadOnly Property Fullname As String
			Get
				Return LastName & ", " & FirstName
			End Get
		End Property

	End Class

End Namespace