Namespace ESRUtility.DataObjects

  ''' <summary>
  ''' RE data (Rechnung).
  ''' </summary>
  Public Class ReData
    Public Property RENR As Integer
    Public Property KDNR As Integer
		Public Property CustomerName As String
		Public Property Fak_Dat As DateTime?
		Public Property Currency As String
    Public Property BetragInk As Decimal
    Public Property MWST1 As Decimal
    Public Property Bezahlt As Decimal
    Public Property Gebucht As Boolean
		Public Property FkSoll As Integer

  End Class


	Public Class EsrRecord

		Public Property paymentID As Integer?
		Public Property paymentNumber As Integer?

		Public Property data As String
		Public Property customerNumber As Integer?
		Public Property invoiceNumber As Integer?
		Public Property bkonto As Integer
		Public Property fksoll As Integer
		Public Property PayedAmount As Decimal?
		Public Property amount As Decimal?
		Public Property bookingamountsum As Decimal?
		Public Property bookingcount As Integer?
		Public Property valutadate As Date?
		Public Property fak_date As Date?
		Public Property iswithtax As Boolean?
		Public Property currency As String

		Public Property isinvoicefinished As Boolean
		Public Property createdfrom As String
		Public Property amountDecision As String

		Public Property fileinfo As System.IO.FileInfo

		Public ReadOnly Property dikey() As String
			Get
				Return String.Format("({0:d}#{0:hh:mm}#{1}) {2}", fileinfo.LastWriteTime, fileinfo.Length, fileinfo.Name)
			End Get
		End Property

	End Class


	Public Class SavedESRData

		Public Property vd As Date?
		Public Property vt As DateTime?

		Public Property createdon As DateTime?
		Public Property createdfrom As String

	End Class

End Namespace
