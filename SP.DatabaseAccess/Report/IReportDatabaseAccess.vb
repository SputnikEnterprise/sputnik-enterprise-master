Imports SP.DatabaseAccess.Report.DataObjects

Namespace Report

  ''' <summary>
  ''' Interface for Report (Rapport) database access.
  ''' </summary>
  Public Interface IReportDatabaseAccess



    Function LoadRPMasterData(ByVal rpNr As Integer) As RPMasterData
    Function LoadRPAbsenceDaysData(ByVal rpNr As Integer) As RPAbsenceDaysData
    Function LoadRPOverviewListData() As IEnumerable(Of RPOverviewData)
    Function LoadRPDetailData(ByVal rpNr As Integer) As RPDetailData
		Function ExistsRPLLADataForPeriode(ByVal rpNr As Integer, ByVal lanr As Decimal, ByVal vonDate As DateTime, ByVal bisDate As DateTime, ByVal isKD As Boolean?) As Boolean

		Function LoadRPLListData(ByVal rpNr As Integer, ByVal lang As String, ByVal rplDataType As RPLType, Optional ByVal esLohnNr As Integer? = Nothing) As IEnumerable(Of RPLListData)
    Function LoadLAListData(ByVal year As Integer, ByVal usLanguage As String, ByVal rplType As RPLType, Optional laNr As Decimal? = Nothing) As IEnumerable(Of LAData)
    Function LoadESSalaryData(ByVal esNumber As Integer) As IEnumerable(Of ESSalaryData)
		Function LoadESDataForCreateingReport(ByVal mandantNumber As Integer, ByVal jahr As Integer, ByVal monat As Integer) As IEnumerable(Of ESData)
		Function LoadCreatedRPOverviewListData(mandantNumber As Integer, rpnumbers As Integer()) As IEnumerable(Of RPOverviewData)
		Function LoadAdditionalFeeListOfEmployeeRPL(ByVal rpNr As Integer, ByVal rplNr As Integer) As IEnumerable(Of RPLAdditionalFee)
    Function LoadFlexibleTimeListOfRPL(ByVal rpNr As Integer, ByVal rpLNr As Integer) As IEnumerable(Of RPLFlexibleTimeData)
		Function LoadMandantTSPLMVSpesenHourValue(ByVal gavNumber As Integer, ByVal year As Integer) As Decimal?
		Function LoadManantTSPLMVWorkingHoursPerWeek(ByVal gavNumber As Integer, ByVal year As Integer) As Decimal?
		Function LoadMonthCloseData(ByVal mandantnumber As Integer, ByVal month As Integer, ByVal year As Integer) As MonthCloseData
    Function LoadRPFinishedFlag(ByVal rpNr As Integer, ByRef isReportFinished As Boolean) As Boolean
    Function LoadRPLAdditionalTexts(ByVal customerNumber As Integer) As IEnumerable(Of RPLAdditionalTextData)
    Function GetNextFeeRPLNumber(ByVal rpNr As Integer) As Integer?
    Function FindRPNrByESNrMonthAndYear(ByVal esNr As Integer, ByVal month As Byte, ByVal year As Integer, ByRef foundRPNr As Integer?, ByRef foundRPId As Integer?) As Boolean

    Function ExistsRPLDayDataForRPL(ByVal rpNr As Integer, ByVal rplNr As Integer, ByVal rplDataType As RPLType) As Boolean?
    Function ExistsRPAbsenceDaysDataForRP(ByVal rpNr As Integer) As Boolean?
    Function LoadRPLDayData(ByVal rpNr As Integer, ByVal rplDataType As RPLType, Optional ByVal rplNr As Integer? = Nothing) As IEnumerable(Of RPLDayData)
    Function AddNewEmployeeRPLData(ByVal initData As NewEmployeeRPLInitData) As Boolean
    Function AddNewCustomerRPLData(ByVal initData As NewCustomerRPLInitData) As Boolean
    Function AddNewEmployeeRPLDayData(ByVal rplDayData As RPLDayData) As Boolean
    Function AddNewCustomerRPLDayData(ByVal rplDayData As RPLDayData) As Boolean
    Function AddNewAbsenceDayData(ByVal absenceDayData As RPAbsenceDaysData) As Boolean
    Function AddNewRPForExistingES(ByVal initData As NewRPForExistingESData) As Boolean

    Function UpdateEmployeeRPLData(ByVal updateData As UpdateEmployeeRPLData) As Boolean
    Function UpdateCustomerRPLData(ByVal updateData As UpdateCustomerRPLData) As Boolean
    Function UpdateEmployeeRPLDayData(ByVal rplDayData As RPLDayData) As Boolean
    Function UpdateCustomerRPLDayData(ByVal rplDayData As RPLDayData) As Boolean
    Function UpdateRPAbsenceDaysData(ByVal rpAbsenceDaysData As RPAbsenceDaysData) As Boolean
    Function UpdateRPFinishedFlag(ByVal rpNr As Integer, ByVal finished As Boolean) As Boolean
    Function UpdateEmployeeRPLTSpesenData(ByVal rpNr As Integer, ByVal anzTSpesen As Integer, ByVal esLohnNr As Integer) As Boolean
    Function CorrectRPAbsenceDaysDataAfterDeleteOfRPL(ByVal rpNr As Integer, ByVal rpYear As Integer, ByVal rpMonth As Integer) As Boolean

    Function GetRPLDayHoursTotal(ByVal rpNr As Integer, ByVal rplDataType As RPLType) As RPLDayHoursTotal

		Function GetRPLScanDocGuid(ByVal rpNr As Integer, ByVal rplNr As Integer) As String
		Function DeleteEmployeeRPLData(ByVal rpNr As Integer, ByVal rplNr As Integer) As DeleteMARPLDataResult
    Function DeleteCustomerRPLData(ByVal rpNr As Integer, ByVal rplNr As Integer) As DeleteKDRPLDataResult

    Function ClearFlexibleTimeOfRPL(ByVal rpNr As Integer, ByVal rplNr As Integer) As Boolean?

  End Interface

  Public Enum RPLType
    Employee = 1
    Customer = 2
  End Enum

  ''' <summary>
  ''' Result of MA_RPL Data deletion.
  ''' </summary>
  Public Enum DeleteMARPLDataResult
    ResultDeleteOk = 1
    ResultCanNotDeleteBecauseMonthIsClosed = 2
    ResultCanNotDeleteBecauseLoNrIsPresent = 3
    ResultDeleteError = 4
  End Enum

  ''' <summary>
  ''' Result of KD_RPL Data deletion.
  ''' </summary>
  Public Enum DeleteKDRPLDataResult
    ResultDeleteOk = 1
    ResultCanNotDeleteBecauseMonthIsClosed = 2
    ResultCanNotDeleteBecauseOfExistingRe = 3
    ResultDeleteError = 4
  End Enum

End Namespace