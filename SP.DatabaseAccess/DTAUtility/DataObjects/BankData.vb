Namespace DTAUtility.DataObjects

	''' <summary>
	''' Bank data.
	''' </summary>
	Public Class BankData
		Public ID As Integer
		Public Property RecNr As Integer
		Public Property KontoESR2 As String
		Public Property BankName As String
		Public Property RecBez As String
		Public Property AsStandard As Boolean
		Public Property Swift As String
		Public Property IBANNr As String

		Public ReadOnly Property DisplayName As String
			Get
				Dim name As String = String.Format("{0} - {1}", ID, If(Not String.IsNullOrWhiteSpace(BankName), BankName, RecBez))

				Return name
			End Get
		End Property

	End Class

End Namespace
