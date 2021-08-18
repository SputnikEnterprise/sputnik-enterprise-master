Imports SP.DatabaseAccess.SalaryValueCalculation.DataObjects


Namespace SalaryValueCalculation

  ''' <summary>
  ''' Interface for salary value calculation database access.
  ''' </summary>
  Public Interface ISalaryValueCalculationDatabaseAccess

    Function LoadEmployeeData(ByVal maNr As Integer) As EmployeeData
    Function LoadLAData(ByVal laNr As Decimal, ByVal laYear As Integer) As LAData
    Function LoadFixBasisFromMDKiAu(ByVal maBasVar As String, ByVal fakCanton As String, ByVal year As Integer) As MDKiAuData
		Function LoadFeierTagGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal
		Function LoadFerienGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal
		Function Load13LohnGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal

		Function LoadDarlehenGuthaben(ByVal mdNr As Integer, ByVal employeeNumber As Integer) As Decimal

		Function LoadAmountOfNightInReport(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As NightHourData

	End Interface

End Namespace