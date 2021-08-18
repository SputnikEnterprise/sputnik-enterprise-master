Namespace Report.DataObjects

  Public Class RPOverviewData

    Public Property ID As Integer
    Public Property RPNr As Integer
    Public Property ESNr As Integer
		Public Property MANr As Integer
		Public Property KDNr As Integer
		Public Property EmployeeLastname As String
    Public Property EmployeeFirstname As String
    Public Property Customer1 As String
    Public Property ReportFrom As DateTime?
    Public Property ReportTo As DateTime?
		Public Property ReportMonth As Integer
		Public Property ReportYear As Integer
    Public Property Erfasst As Boolean
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String

    Public ReadOnly Property EmployeeFullName As String
      Get

        If String.IsNullOrWhiteSpace(EmployeeFirstname) Then
          Return EmployeeLastname
        Else
          Return String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname)
        End If

      End Get
    End Property

		Public ReadOnly Property ReportMonthAndYear As String
			Get
				Return String.Format("{0:00}, {1:0000}", ReportMonth, ReportYear)
			End Get
		End Property

  End Class

End Namespace
