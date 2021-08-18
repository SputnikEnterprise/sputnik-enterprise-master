Namespace Employee.DataObjects.ContactMng

  ''' <summary>
  ''' ES data (ES).
  ''' </summary>
  Public Class ESDataForContactMng
    Public Property ID As Integer
    Public Property ESNumber As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property ES_As As String
    Public Property ES_FromDate As DateTime?
    Public Property ES_ToDate As DateTime?
		Public Property Customername As String

		Public ReadOnly Property ESPeriode As String
			Get
				Return String.Format("{0:d} - {1:d}", ES_FromDate, ES_ToDate)
			End Get
		End Property

		Public ReadOnly Property ESDataShowESAsWithDate As String
			Get
				Return String.Format("{0}: ({1:d} - {2:d})", ES_As, ES_FromDate, ES_ToDate)
			End Get
		End Property

	End Class

End Namespace
