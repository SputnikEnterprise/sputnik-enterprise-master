
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Invoice.DataObjects


Namespace UI

	Public Class InitPaymentDataPage1

		Public Property MandantData As MandantData
		Public Property InvoiceData As DatabaseAccess.Invoice.DataObjects.Invoice
		Public Property FKSOLL As Integer?
		Public Property VDate As Date?
		Public Property BDate As Date?
		Public Property Amount As Decimal?


	End Class


End Namespace
