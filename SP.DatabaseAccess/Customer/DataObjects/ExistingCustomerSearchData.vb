Namespace Customer.DataObjects

  ''' <summary>
  ''' Existing customer search data
  ''' </summary>
  Public Class ExistingCustomerSearchData
    Public Property Company As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property Location As String
    Public Property CountryCode As String

		Public Property customerKST As String
		Public Property customerAdvisor As String

	End Class


	Public Class CommonTelephonyData

		Public Property ModulSource As TelephonyRecordSource
		Public Property ZNumber As Integer?
		Public Property RecNumber As Integer?
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property Company As String
		Public Property Telephon As String

		Public ReadOnly Property FullName As String
			Get
				If String.IsNullOrWhiteSpace(Lastname) AndAlso String.IsNullOrWhiteSpace(Firstname) Then Return String.Empty
				Return String.Format("{0}{1} {2}", Lastname, If(String.IsNullOrWhiteSpace(Firstname), "", ","), Firstname)
			End Get
		End Property

	End Class

	Public Enum TelephonyRecordSource
		Employee
		Customer
		ResponsiblePerson
	End Enum

	Public Class CallHistoryData

		Public Property ID As Integer?
		Public Property USNr As Integer?
		Public Property EventTime As DateTime?
		Public Property Advisor As String
		Public Property CallHandle As Integer?
		Public Property CallID As Integer?
		Public Property CallInfo As String
		Public Property CalledFrom As String
		Public Property CalledTo As String
		Public Property Incoming As Boolean?
		Public Property CustomerNumber As Integer?
		Public Property ResponslibePerson As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property ModeNr As Integer?
		Public Property CallDuration As String
		Public Property UserTapiID As String


	End Class


End Namespace