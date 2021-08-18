

'Public Class DB1CalculationData

'	Public Property KST As String
'	Public Property PayrollData As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData)

'End Class



Public Class DB1PayrollData

	Public Property payrollNumber As Integer
	Public Property _ahvAmount As Decimal
	Public Property ahvemployeeAmount As Decimal
	Public Property _agAmount As Decimal
	Public Property agemployeeAmount As Decimal
	Public Property agemployeebvgAmount As Decimal
	Public Property KST As String
	'Public Property DB1Details As List(Of DB1DetailData)

End Class

Public Class DB1DetailData

	'Inherits SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData

End Class






Public Class DB1InvoiceData

	Public Property reportNumber As Integer
	Public Property invoicenNumber As Integer
	Public Property invoiceArt As String
	Public Property KST As String
	Public Property invoiceAmount As Decimal

End Class

Public Class DB1ESData

	Public Property ESNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property EmployeeNumber As Integer

	Public Property employeefullname As String

	Public Property customername As String

	Public Property es_ab As Date?
	Public Property es_ende As Date?

	Public ReadOnly Property ESDataToShow
		Get
			Return String.Format("{0}: {1} - {2}", ESNumber, es_ab, es_ende)
		End Get
	End Property

End Class


Public Class DB1EmployeeData

	Public Property ESNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property EmployeeNumber As Integer

	Public Property employeefullname As String

	Public Property customername As String

	Public Property es_ab As Date?
	Public Property es_ende As Date?

	Public ReadOnly Property EmployeeDataToShow
		Get
			Return String.Format("{0}: {1}", EmployeeNumber, employeefullname)
		End Get
	End Property

End Class


Public Class DB1CustomerData

	Public Property ESNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property EmployeeNumber As Integer

	Public Property employeefullname As String

	Public Property customername As String

	Public Property es_ab As Date?
	Public Property es_ende As Date?

	Public ReadOnly Property CustomerDataToShow
		Get
			Return String.Format("{0}: {1}", CustomerNumber, customername)
		End Get
	End Property

End Class

