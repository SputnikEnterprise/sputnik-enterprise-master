
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Partial Class SalaryValueCalculator

	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	'Public Function LoadEmployeeFlexibleWorkingHoursData(ByVal mdNr As Integer, ByVal employeeNumber As Integer) As EmployeeCreditData
	'	Dim data = m_ListingDatabaseAccess.LoadFlexibleWorkingHoursData(mdNr, employeeNumber)

	'	If data Is Nothing Then
	'		m_Logger.LogError(String.Format("Gleitzeit Daten konnten nicht geladen werden. MDNr: {0} >>> MANr: {1}", mdNr, employeeNumber))

	'		Return Nothing
	'	End If

	'	Return data
	'End Function


End Class
