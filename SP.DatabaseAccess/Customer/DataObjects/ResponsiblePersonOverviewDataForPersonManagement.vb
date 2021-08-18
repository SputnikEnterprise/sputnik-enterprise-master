Namespace Customer.DataObjects

  ''' <summary>
  ''' Responsible person overview data for person management
  ''' </summary>
  Public Class ResponsiblePersonOverviewDataForPersonManagement
    Public Property ID As Integer
    Public Property CustomerNumber As Integer
    Public Property RecordNumber As Integer
    Public Property Name As String
    Public Property Telephone As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String

		Public Property ZState1 As String
		Public Property ZState2 As String

	End Class

End Namespace