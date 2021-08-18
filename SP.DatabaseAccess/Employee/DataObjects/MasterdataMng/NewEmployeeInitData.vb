Namespace Employee.DataObjects.MasterdataMng

	''' <summary>
	''' Init data for new employee creation.
	''' </summary>
	Public Class NewEmployeeInitData

		' ---Mitarbeiter table data---

		Public Property KST As String

		Public Property Lastname As String
		Public Property Firstname As String
		Public Property Street As String
		Public Property Postcode As String
		Public Property Latitude As Double?
		Public Property Longitude As Double?
		Public Property Location As String
		Public Property CountryCode As String
		Public Property Gender As String
		Public Property Nationality As String
		Public Property Civilstate As String
		Public Property Birthdate As DateTime?
		Public Property Language As String

		Public Property Permission As String
		Public Property PermissionToDate As DateTime?
		Public Property CHPartner As Boolean?
		Public Property ValidatePermissionWithTax As Boolean?
		Public Property NoSpecialTax As Boolean?
		Public Property BirthPlace As String
		Public Property S_Canton As String
		Public Property Residence As Boolean?
		Public Property ANS_QST_Bis As DateTime?
		Public Property Q_Steuer As String
		Public Property ChurchTax As String
		Public Property ChildsCount As Short?
		Public Property QSTCommunity As String
		Public Property TaxCommunityLabel As String
		Public Property TaxCommunityCode As Integer?


		' ---MAKontakt table data---
		Public Property DStellen As Boolean
		Public Property NoES As Boolean
		Public Property Stat1 As String
		Public Property Stat2 As String
		Public Property Contact As String
		Public Property Profession As String
		Public Property ProfessionCode As Integer?
		Public Property QLand As String
		Public Property RahmenCheck As Boolean

		' ---MA_LOAusweis table data---
		Public Property NoZG As Boolean
		Public Property NoLO As Boolean
		Public Property Currency As String
		Public Property Zahlart As String
		Public Property BVGCode As String
		Public Property SecSuvaCode As String

		' ---MA_LOAusweis table data---
		Public Property FerienBack As Short
		Public Property FeiertagBack As Short
		Public Property L13Back As Short

		' ---Common data---
		Public Property EmployeeNumberOffset As Integer
		Public Property MDNr As Integer
		Public Property UserKST As String
		Public Property CreatedFrom As String

		'---Id of new employee (return value)---
		Public Property IdNewEmployee As Integer?
		Public Property ShowAsApplicant As Boolean?

		Public Property ForeignCategory As String
		Public Property ZEMISNumber As String
		Public Property TypeOfStay As String
		Public Property EmploymentType As String
		Public Property OtherEmploymentType As String
		Public Property EmployeePartnerRecID As Integer?
		Public Property CreatedUserNumber As Integer?

		Public ReadOnly Property TaxCommunity As String
			Get
				Return String.Format("{0}-{1}", TaxCommunityCode, TaxCommunityLabel)
			End Get
		End Property

	End Class

End Namespace
