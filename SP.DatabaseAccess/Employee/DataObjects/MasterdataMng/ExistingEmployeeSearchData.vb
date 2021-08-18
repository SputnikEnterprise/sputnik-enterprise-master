Namespace Employee.DataObjects.MasterdataMng

  ''' <summary>
  ''' Existing employee search data
  ''' </summary>
  Public Class ExistingEmployeeSearchData
		Public Property EmployeeNumber As Integer?
		Public Property Lastname As String
		Public Property Firstname As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property Location As String
    Public Property CountryCode As String
		Public Property Birthdate As DateTime?
		Public Property Gender As String
		Public Property MobilePhone As String
		Public Property BriefAnrede As String
		Public Property Telephone_P As String
		Public Property Email As String

		Public Property employeeKST As String
		Public Property employeeAdvisor As String

		Public Property MABusinessBranch As String
		Public Property MDNr As Integer?
		Public Property ShowAsApplicant As Boolean?
		Public Property ApplicantLifecycle As Integer?
		Public Property ApplicantID As Integer?
		'Public Property CVLProfleID As Integer?
		Public Property SMS_Mailing As Boolean?

		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String


		Public ReadOnly Property EmployeeFullnameWithComma As String
			Get
				Return String.Format("{1}, {0}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		''' <summary>
		''' employee address CH-5073 Gipf-Oberfrick
		''' </summary>
		''' <returns></returns>
		Public ReadOnly Property EmployeeAddress As String
			Get
				Return String.Format("{0}-{1} {2}", CountryCode, Postcode, Location)
			End Get
		End Property


	End Class

End Namespace