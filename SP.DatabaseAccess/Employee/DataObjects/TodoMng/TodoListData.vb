Namespace Employee.DataObjects.TodoMng

  ''' <summary>
  ''' Todo list data (tblTODOList)
  ''' </summary>
  Public Class TodoListData

    Public Property ID As Integer
		Public Property AllUsers As Boolean?
		Public Property RecNumber As Integer?
		Public Property UserNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property Subject As String
		Public Property IsImportant As Boolean?
		Public Property IsCompleted As Boolean?
		Public Property Schedulebegins As DateTime?
		Public Property Scheduleends As DateTime?
		Public Property TODOSourceEnum As TODOEnum

	End Class

End Namespace