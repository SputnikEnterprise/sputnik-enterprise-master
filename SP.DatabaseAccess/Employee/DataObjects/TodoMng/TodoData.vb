Namespace Employee.DataObjects.TodoMng

  ''' <summary>
  ''' Todo data (tblTODOList)
  ''' </summary>
  Public Class TodoData

    Public Property ID As Integer = 0
		Public Property UserNumber As Integer?
		Public Property TU_UserNumber As Integer?
		Public Property AllUsers As Boolean?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property ResponsiblePersonRecordNumber As Integer?
		Public Property VacancyNumber As Integer?
		Public Property ProposeNumber As Integer?
		Public Property ESNumber As Integer?
		Public Property RPNumber As Integer?
		Public Property LMNumber As Integer?
		Public Property RENumber As Integer?
		Public Property ZENumber As Integer?
		Public Property Subject As String
		Public Property Body As String
		Public Property IsImportant As Boolean?
		Public Property IsCompleted As Boolean?
		Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property Schedulebegins As DateTime?
    Public Property Scheduleends As DateTime?
    Public Property ScheduleRememberIn As Integer?
    Public Property ScheduleRemember As DateTime?

		Public Property TODOSourceEnum As TODOEnum

		Public Function Clone() As TodoData
      Return DirectCast(Me.MemberwiseClone(), TodoData)
    End Function

	End Class

	Public Enum TODOEnum

		CREATED_MANUALLY
		VACANCY_CREATED
		VACANCY_DELETED
		VACANCY_ONLINE_STATE_CHANGE
		VACANCY_AVAM_UPDATED

		PROPOSE_UPDATED
		PROPOSE_UPDATED_FEEDBACK
		PROPOSE_UPDATED_GETRESULT
		PROPOSE_UPDATED_VIEWED

		SYSTEM_UPDATE
		SYSTEM_SCANFILEINFO
		SYSTEM_SCANERROR

	End Enum

End Namespace