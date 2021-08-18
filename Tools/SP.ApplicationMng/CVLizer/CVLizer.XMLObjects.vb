

Namespace CVLizer.DataObject

	Public Class JsonData

		Public Property model As String
		Public Property language As String
		Public Property filename As String
		Public Property data As String
		Public Property Documents As List(Of InputDocs)


	End Class

	Public Class InputDocs
		Public Property filename As String
		Public Property data As Byte()

	End Class

	Public Class EntryLOGData
		Public Property LogDate As DateTime?
		Public Property LogType As String
		Public Property Message As String

	End Class

End Namespace
