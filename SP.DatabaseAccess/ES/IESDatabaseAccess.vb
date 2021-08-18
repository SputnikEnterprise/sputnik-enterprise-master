Imports SP.DatabaseAccess.ES.DataObjects.ESMng

Namespace ES

  ''' <summary>
  ''' Interface for ES (Einsatz) database access.
  ''' </summary>
  Public Interface IESDatabaseAccess

    Function LoadFoundedRPInESMng(esNumber As Integer?, employeeNumber As Integer?, customerNumber As Integer?) As IEnumerable(Of FoundedReports)


    Function LoadEmployeeData() As IEnumerable(Of EmployeeData)
		Function LoadCustomerData(Optional ByVal usFiliale As String = "") As IEnumerable(Of CustomerData)
    Function LoadSalaryCalculationPercentageValues(ByVal age As Integer) As SalaryCalculationPercentageValues
    Function LoadESMasterData(ByVal esNumber As Integer) As ESMasterData
    Function LoadESSalaryData(ByVal esNumber As Integer) As IEnumerable(Of ESSalaryData)
    Function LoadMandantSuvaHLData(ByVal year As Integer, ByVal mdNr As Integer, ByRef success As Boolean) As Decimal?
    Function LoadESCategorizationData() As IEnumerable(Of ESCategorizationData)
    Function LoadESAdditionalSalaryTypeData(ByVal esNumber As Integer, ByVal type As ESAdditionalSalaryType, Optional ByVal esLANr As Integer? = Nothing, Optional ByVal esLohnNumber As Integer? = Nothing) As IEnumerable(Of ESEmployeeAndCustomerLAData)
    Function LoadLAListForESMng(ByVal year As Integer) As IEnumerable(Of LAData)
    Function LoadConflictedLORecordsInPeriod(ByVal employeeNumber As Integer, ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedLOData)
    Function LoadConflictedRPLRecordsInPeriod(ByVal esNumber As Integer, ByVal employeeNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedRPLData)
    Function LoadConflictedMonthCloseRecordsInPeriod(ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime, ByRef resultCode As Integer) As IEnumerable(Of ConflictedMonthCloseData)
    Function LoadMargeBoundaryValuesForMandant(ByVal mdNumber As Integer, ByVal mdYear As Integer) As MandantMargeBoundaryValues
    Function LoadSuvaData() As IEnumerable(Of SuvaData)
    Function LoadExistingReportNumbersForES(ByVal esNr As Integer) As IEnumerable(Of Integer)
    Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint)
    Function LoadContextMenu4PrintTemplatesData() As IEnumerable(Of ContextMenuForPrintTemplates)

    Function CheckIfESSalaryDataCanBeDeleted(ByVal esNumber As Integer, ByVal esLohnNr As Integer) As Boolean
    Function CheckIfRPExistsForES(ByVal esNumber As Integer) As Boolean?
    Function CheckIfKostenteilungCanBeChanged(ByVal esNumber As Integer) As Boolean?
		Function CheckIfLOVonDateCanBeSet(ByVal esNumber As Integer, ByVal esLohnNumber As Integer?, ByVal lovonDate As DateTime) As Boolean?

    Function UpdateESMasterData(ByVal esData As ESMasterData) As Boolean
    Function UpdateESSalrayDataForESMng(ByVal esNumber As Integer, ByVal esLohnNr As Integer, ByVal loVon As DateTime) As Boolean
    Function UpdateESAdditionalSalaryTypeData(ByVal type As ESAdditionalSalaryType, ByVal laSalaryTypeData As ESEmployeeAndCustomerLAData) As Boolean


    Function AddNewESAdditionalSalaryTypeData(ByVal type As ESAdditionalSalaryType, ByVal esLAData As ESEmployeeAndCustomerLAData, ByVal laNrOffset As Integer) As Boolean
    Function AddNewES(ByVal esMasterData As ESMasterData, ByVal esNumberOffset As Integer) As Boolean
    Function AddNewESLohn(ByVal esSalaryData As ESSalaryData) As Boolean
    Function AddNewRP(ByVal rp As RPData, ByVal rpNumberOffset As Integer) As Boolean
    Function AddNewESLohnGAVData(ByVal esSalaryGAVData As ESSalaryGAVData) As Boolean
    Function AddNewESWithESLohnAndRP(ByVal esMasterData As ESMasterData,
                                     ByVal esSalaryData As ESSalaryData,
                                     ByVal esSalaryGAVData As ESSalaryGAVData,
                                     ByVal rpData As RPData,
                                     ByVal esNumberOffset As Integer,
                                     ByVal rpNumberOffset As Integer) As Boolean
    Function AddNewESLohnAndRP(ByVal esNr As Integer,
                                   ByVal esSalaryData As ESSalaryData,
                                   ByVal esSalaryGAVData As ESSalaryGAVData,
                                   ByVal rpData As RPData,
                                   ByVal rpNumberOffset As Integer) As Boolean

    Function ActivateESSalaryData(ByVal esNr As Integer, ByVal esLohnNr As Integer) As Boolean
    Function DeleteESSalaryData(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteESSalaryResult
    Function DeleteESAdditionalSalaryTypeData(ByVal id As Integer, ByVal type As ESAdditionalSalaryType, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteESSalaryTypeResult

  End Interface

  ''' <summary>
  ''' Result of ES salary data (ESLohn) deletion.
  ''' </summary>
  Public Enum DeleteESSalaryResult
    ResultDeleteOk = 1
    ResultCanNotDeleteBecauseOfRPL = 2
    ResultCanNotDeleteBecauseOfAdditionalSalaryType = 3
    ResultOnlyOneRecordLeft = 4
    ResultDeleteError = 5
  End Enum


  ''' <summary>
  ''' Result of ES salary type (ES_KD_LA and ES_MA_LA) deletion.
  ''' </summary>
  Public Enum DeleteESSalaryTypeResult
    ResultDeleteOk = 1
    ResultCanNotDeleteBecauseOfRPL = 2
    ResultDeleteError = 3
  End Enum

  ''' <summary>
  ''' Employee and customer salary type (ES_KD_LA and ES_MA_LA) type.
  ''' </summary>
  Public Enum ESAdditionalSalaryType
    Customer = 1
    Employee = 2
  End Enum

End Namespace