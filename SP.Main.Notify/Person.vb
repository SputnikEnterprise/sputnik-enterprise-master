

Public Class PreselectionData
	Public Property MDNr As Integer?
	Public Property UserNumber As Integer?
	Public Property CustomerID As String

End Class


Public Class NotifyData

	Public Property ID As Integer
	Public Property NotifyArt As NotifyEnum
	Public Property NotifyHeader As String
	Public Property NotifyComments As String

	Public Enum NotifyEnum
		AsError
		AsImportant
		AsInfo
	End Enum

End Class
