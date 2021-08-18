Namespace Employee.DataObjects.ContactMng

  ''' <summary>
  ''' Vacancy data (Vakanzen).
  ''' </summary>
  Public Class VacancyDataForContactMng
    Public Property ID As Integer
    Public Property VacancyNumber As Integer?
    Public Property Description As String
		Public Property VakState As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property Customername As String

	End Class

End Namespace