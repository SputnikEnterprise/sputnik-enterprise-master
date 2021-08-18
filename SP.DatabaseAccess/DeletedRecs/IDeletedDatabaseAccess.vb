Imports SP.DatabaseAccess.Deleted.DataObjects

Namespace Deleted

	''' <summary>
	''' Interface for deleted database access.
	''' </summary>
	Public Interface IDeletedDatabaseAccess

		Function LoadDeletedRecsForSelectedModules(ByVal modulname As String, ByVal thisyear As Boolean?, ByVal thismonth As Boolean?) As IEnumerable(Of DeletedData)
		Function AddDeleteRecInfo(ByVal deleteData As DeletedData) As Boolean
		Function UpdateDeleteRecInfo(ByVal deleteData As DeletedData) As Boolean


	End Interface

End Namespace