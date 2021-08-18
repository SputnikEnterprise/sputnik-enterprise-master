
Namespace Listing.DataObjects

	Public Class InvoicePrintSearchConditionData
		Public Property MDNr As Integer
		Public Property InvoiceNumbers As List(Of Integer)
		Public Property CustomerNumbers As List(Of Integer)
		Public Property OrderByEnum As OrderByValue
		Public Property WOSValueEnum As WOSSearchValue
		Public Property GroupByEMail As Boolean?

	End Class

	Public Class ESRListPrintData

		Public Property ID As Integer?
		Public Property MDNr As Integer
		Public Property AmountDecision As String
		Public Property Rec As String
		Public Property VD As DateTime?
		Public Property CustomerNumber As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property VT As String
		Public Property Company As String
		Public Property DiskInfo As String

		Public Property ESRAmount As Decimal?
		Public Property ESRBookedAmount As Decimal?

		Public Property InvoiceAmount As Decimal?
		Public Property InvoicePayed As Decimal?
		Public Property CreatedOn As DateTime?
		Public Property Createdfrom As String

		Public ReadOnly Property InvoiceOpenAmount As Decimal?
			Get
				Return InvoiceAmount.GetValueOrDefault(0) - InvoicePayed.GetValueOrDefault(0)
			End Get
		End Property

	End Class

	Public Class ESRPaymentListPrintData
		Public Property ZENr As Integer?
		Public Property MDNr As Integer
		Public Property InvoiceNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property Fak_Date As DateTime?
		Public Property ValutaOn As DateTime?
		Public Property BookedOn As DateTime?
		Public Property Company As String
		Public Property Amount As Decimal?
		Public Property InvoiceAmount As Decimal?
		Public Property InvoicePayed As Decimal?
		Public Property CreatedOn As DateTime?
		Public Property Createdfrom As String

		Public ReadOnly Property InvoiceOpenAmount As Decimal?
			Get
				Return InvoiceAmount.GetValueOrDefault(0) - InvoicePayed.GetValueOrDefault(0)
			End Get
		End Property

	End Class

	Public Class MandantDataForPrintDTAESRListing
		Public Property RecNr As Integer?
		Public Property MD_ID As String
		Public Property KontoESR1 As String
		Public Property KontoESR2 As String
		Public Property DTAClnr As String
		Public Property KontoDTA As String
		Public Property KontoVG As String
		Public Property ESRFileName As String
		Public Property BankName As String
		Public Property BankClnr As String
		Public Property BankAdresse As String
		Public Property Swift As String
		Public Property ESRIBAN1 As String
		Public Property ESRIBAN2 As String
		Public Property ESRIBAN3 As String
		Public Property DTAIBAN As String
		Public Property VGIBAN As String
		Public Property DTAAdr1 As String
		Public Property DTAAdr2 As String
		Public Property DTAAdr3 As String
		Public Property DTAAdr4 As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property RecBez As String

	End Class

	Public Class InvoiceArtData
		Public Property Art As String
		Public Property ArtLabel As String

	End Class


End Namespace
