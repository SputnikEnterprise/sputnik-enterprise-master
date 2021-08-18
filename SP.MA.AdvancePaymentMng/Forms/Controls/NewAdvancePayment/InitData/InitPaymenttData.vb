Imports SP.DatabaseAccess.Employee.DataObjects.BankMng

Namespace UI

	Public Class InitPaymentData

		Public Property Year As Integer
		Public Property Month As Integer
		Public Property PaymentAtDate As DateTime
		Public Property PaymentType As Integer
		Public Property PaymentTypeText As String
		Public Property AmountOfPayment As Decimal
		Public Property PaymentReason As String
		Public Property GebAbzug As Boolean
		Public Property AmountExceeded As Boolean

		Public Property BankData As EmployeeBankData


	End Class

	' ''' <summary>
	' ''' Initial bank data.
	' ''' </summary>
	'Public Class InitBankData

	'	Public Property BankData As EmployeeBankData

	'End Class

End Namespace
