
Imports SP.DatabaseAccess.ModulView.DataObjects

Namespace ModulView

	''' <summary>
	''' Interface for ModulView database access.
	''' </summary>
	Public Interface IModulViewDatabaseAccess

		Function LoadGetDbCustomerContactData(ByVal mdNr As Integer, ByVal customerNumber As Integer?, ByVal year As Integer?, ByVal month As Integer?,
											  ByVal recID As Integer?, ByVal showLatestEntries As Boolean?, ByVal contactPlainText As String) As IEnumerable(Of ModulViewCustomerContactData)
		Function LoadAssignedEmployeeContactData(ByVal mdNr As Integer, ByVal employeeNumber As Integer?, ByVal year As Integer?, ByVal month As Integer?,
												 ByVal recID As Integer?, ByVal showLatestEntries As Boolean?, ByVal contactPlainText As String) As IEnumerable(Of ModulViewEmployeeContactData)
		Function LoadAssignedProposeContactData(ByVal mdNr As Integer, ByVal proposeNumber As Integer?, ByVal recID As Integer?, ByVal showLatestEntries As Boolean?) As IEnumerable(Of ModulViewProposeContactData)

	End Interface


End Namespace
