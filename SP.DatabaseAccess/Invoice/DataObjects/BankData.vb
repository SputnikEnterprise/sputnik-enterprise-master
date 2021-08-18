Namespace Invoice.DataObjects

  ''' <summary>
	''' The esr bank data from mandant
  ''' </summary>
  ''' <remarks></remarks>
	Public Class BankData
		Public Property ID As Integer
		Public Property MDNr As Integer
		Public Property RecNr As Integer
		Public Property MD_ID As String
		Public Property KontoESR1 As String
		Public Property KontoESR2 As String
		Public Property ESRIBAN1 As String
		Public Property ESRIBAN2 As String
		Public Property ESRIBAN3 As String
		Public Property RecBez As String
		Public Property BankClnr As String
		Public Property BankAdresse As String
		Public Property Swift As String
		Public Property BankName As String
		Public Property AsStandard As Boolean

	End Class

End Namespace
