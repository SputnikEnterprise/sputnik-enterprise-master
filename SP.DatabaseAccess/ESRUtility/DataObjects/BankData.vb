Namespace ESRUtility.DataObjects

	Public Class BankData
		Public Property ID As Integer
		Public Property RecNr As Integer
		Public Property ESRCustomerID As String
		Public Property KontoESR1 As String
		Public Property KontoESR2 As String
		Public Property BankName As String
		Public Property ESRIban1 As String
		Public Property ESRIban2 As String
		Public Property Swift As String
		Public Property AsStandard As Boolean

		Public ReadOnly Property DisplayName As String
			Get
				Dim name As String = String.Format("({0}) - {1}", KontoESR1, BankName)

				Return name
			End Get
		End Property

	End Class

End Namespace
