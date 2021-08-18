Namespace Employee.DataObjects.JobInterviewMng

  ''' <summary>
  ''' Vacancy data (Vakanzen).
  ''' </summary>
  Public Class VacancyData
    Public Property ID As Integer
    Public Property VacancyNumber As Integer?
    Public Property Description As String

		Public Property VakState As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

End Namespace