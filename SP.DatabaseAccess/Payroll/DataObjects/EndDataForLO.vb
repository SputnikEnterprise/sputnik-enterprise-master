Namespace PayrollMng.DataObjects

	Public Class EndDataForLO

		Public Property LONewNr As Integer?
		Public Property MANummer As Integer?
		Public Property Kst1 As String
		Public Property Kst2 As String
		Public Property LP As Short?
		Public Property Jahr As String
		Public Property S_Kanton As String
		Public Property QSTTarif As String
		Public Property Zivilstand As String
		Public Property Kirchensteuer As String
		Public Property Q_Steuer As String
		Public Property Kinder As Short?
		Public Property QSTBasis As Decimal?
		Public Property strESData As String
		Public Property Wohnort As String
		Public Property CHPartner As Boolean
		Public Property NoSpecialTax As Boolean
		Public Property Permission As String
		Public Property PermissionToDate As DateTime?
		Public Property EmployeePartnerRecID As Integer?
		Public Property EmployeeLOHistoryRecID As Integer?
		Public Property WorkedDay As Short?
		Public Property Land As String
		Public Property Brutto As Decimal?
		Public Property AHVBas As Decimal?
		Public Property AHVLohn As Decimal?
		Public Property AHVFrei As Decimal?
		Public Property NAHVPf As Decimal?
		Public Property ALV1Lohn As Decimal?
		Public Property ALV2Lohn As Decimal?
		Public Property SUVABas As Decimal?
		Public Property MAName As String
		Public Property BVGBeginn As DateTime?
		Public Property BVGEnde As DateTime?
		Public Property MData As String
		Public Property ZGNumber As Integer?
		Public Property BVGBegin As DateTime?
		Public Property BVGEnd As DateTime?
		Public Property BVGDateData As String
		Public Property MDNr As Integer?
		Public Property CreatedUserNumber As Integer?

	End Class

	Public Class SuspectPayrollData

		Public Property LONr As Integer
		Public Property MANr As Integer
		Public Property LANr As Decimal?
		Public Property M_Ans As Decimal?
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Reason As String

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0}, {1}", LastName, FirstName)
			End Get
		End Property

	End Class

	Public Class PayrollCheckCashData

		Public Property LONr As Integer
		Public Property ZGNr As Integer
		Public Property MANr As Integer
		Public Property LANr As Decimal?
		Public Property Betrag As Decimal?
		Public Property LALabel As String
		Public Property FirstName As String
		Public Property LastName As String

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0}, {1}", LastName, FirstName)
			End Get
		End Property

	End Class

	Public Class PayrollUnusualData
		Public Property MANr As Integer
		Public Property LANr As Decimal?
		Public Property FirstName As String
		Public Property LastName As String
		Public Property LALabel As String
		Public Property Reason As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedUserNumber As Integer

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0}, {1}", LastName, FirstName)
			End Get
		End Property

	End Class


End Namespace
