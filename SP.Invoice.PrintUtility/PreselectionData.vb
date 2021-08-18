
Public Class PreselectionData
	Public Property MDNr As Integer
	Public Property BeraterMA As String
	Public Property BeraterKD As String
	Public Property RechnungsDatum As DateTime?
	Public Property DebitorenArt As String
	Public Property CustomerNumber As Integer?
	Public Property InvoiceNumbers As List(Of Integer?)
	Public Property CustomerNumbers As List(Of Integer?)

End Class


Public Class PreselectionDunningData
	Public Property MDNr As Integer
	Public Property DunningLevel As Integer?
	Public Property DunningDate As Date?

End Class

Public Class DeleteResult

	Public Property Value As Boolean?
	Public Property Message As String

End Class

Public Class SelectionData

	Public Property InvoiceNumbers As List(Of Integer?)

End Class


Public Class DunningLevel
	Public Property Value As Integer
	Public Property Display As String
	Public ReadOnly Property Label As String
		Get
			Return String.Format("{0} - {1}", Value, Display)
		End Get
	End Property
End Class
