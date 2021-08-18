Namespace Employee.DataObjects.Salary

	Public Class EmployeeLOSettingsData

		Public Property ID As Integer
		Public Property EmployeeNumber As Integer?
		Public Property Currency As String
		Public Property Zahlart As String
		Public Property NoZG As Boolean?
		Public Property NoZGWhy As String
		Public Property NoLO As Boolean?
		Public Property NoLOWhy As String
		Public Property AHVCode As String
		Public Property ALVCode As String
		Public Property BVGCode As String
		Public Property KTGPflicht As Boolean?
		Public Property WeeklyPayment As Boolean?
		Public Property KKPflicht As Boolean?
		Public Property LORes1Pflicht As Boolean?
		Public Property LoRes2Pflicht As Boolean?
		Public Property FerienBack As Boolean?
		Public Property FeierBack As Boolean?
		Public Property Lohn13Back As Boolean?
		Public Property Result As String
		Public Property NoRPPrint As Boolean?
		Public Property PayrollSendAsZip As Boolean?
		Public Property SecSuvaCode As String
		Public Property MAGleitzeit As Boolean?
		Public Property KI As Boolean?
		Public Property Max_NegativSalary As Decimal?
		Public Property AHVAnAm As DateTime?


		Public Function Clone() As EmployeeLOSettingsData
			Return DirectCast(Me.MemberwiseClone(), EmployeeLOSettingsData)
		End Function

	End Class


	Public Class EmployeeBackupHistoryData

		Public Property ID As Integer
		Public Property EmployeeNumber As Integer?

		Public Property CivilState As String
		Public Property Birthdate As DateTime?
		Public Property Nationality As String
		Public Property SocialInsuranceNumber As String
		Public Property PermissionCode As String
		Public Property PermissionValidTo As DateTime?
		Public Property ForeignCategory As String
		Public Property BirthPlace As String
		Public Property CHPartner As Boolean?
		Public Property NoSpecialTax As Boolean?
		Public Property ZemisNumber As String

		Public Property TaxCanton As String
		Public Property TaxCode As String
		Public Property TaxCommunityCode As Integer?
		Public Property TaxCommunityLabel As String
		Public Property TaxChurchCode As String
		Public Property NumberOfChildren As Integer?
		Public Property EmploymentType As String
		Public Property OtherEmploymentType As String
		Public Property TypeofStay As String
		Public Property CertificateForResidence As Boolean?
		Public Property CertificateForResidenceValidTo As Date?

		Public Property AHVCode As String
		Public Property AHVAnAm As DateTime?
		Public Property ALVCode As String
		Public Property BVGCode As String
		Public Property KTGPflicht As Boolean?
		Public Property KKPflicht As Boolean?
		Public Property FerienBack As Boolean?
		Public Property FeierBack As Boolean?
		Public Property Lohn13Back As Boolean?
		Public Property MAGleitzeit As Boolean?

		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CreatedUserNumber As Integer?

		Public ReadOnly Property TaxViewData As String
			Get
				Return String.Format("{0}{1}{2} ({3})", TaxCode, NumberOfChildren, TaxChurchCode, TaxCanton)
			End Get
		End Property

		Public ReadOnly Property CommunityViewData As String
			Get
				Return String.Format("{0} - {1}", TaxCommunityCode, TaxCommunityLabel)
			End Get
		End Property

		Public ReadOnly Property PermissionViewData As String
			Get
				Return String.Format("{0} - {1:dd.MM.yyyy}", PermissionCode, PermissionValidTo)
			End Get
		End Property

	End Class


End Namespace
