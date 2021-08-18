Imports SP.DatabaseAccess.PayrollMng.DataObjects

Namespace PayrollMng

	''' <summary>
	''' Interface for Payroll (Lohnbuchhaltung) database access.
	''' </summary>
	Public Interface IPayrollDatabaseAccess

		'--- Anzeige ---
		Function LoadPayrollMasterData(ByVal payrollNumber As Integer) As LOMasterData
		Function LoadAvailableEmployeesForPayroll(ByVal mandatNumber As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeDataForPayroll)
		Function LoadEmplyoeeDetailDataForPayroll(ByVal maNr As Integer, ByVal mandantNumber As Integer, ByVal month As Integer, ByVal year As Integer, ByVal language As String) As IEnumerable(Of EmployeeDetailDataForPayroll)
		Function LoadInvalidRecordNumbersForPayroll(ByVal mandantNumber As Integer, ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of ModuleRecordNumber)

		' --- Erzeugung ---
		Function LoadLABezDataForPayroll() As IEnumerable(Of LA_BezData)
		Function LoadESWorkDaysForAMonth(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal year As Integer, ByVal month As Integer) As Integer?
		Function LoadESDataForESDayInYearCalculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As List(Of ESDataForESDayInYearCalculation)
		Function LoadMandantData(ByVal year As Integer, ByVal mdNr As Integer) As MandantData
		Function LoadMandantAnsatzData(ByVal mdNr As Integer, ByVal year As Integer) As MandantAnsatzData
		Function LoadExistLoForPayroll(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByRef err As Boolean) As Integer?
		Function FindNewLONr(ByVal offset As Integer) As Integer?
		Function FindNewLMNr() As Integer?
		Function LoadMD_KK_LMV_Data(ByVal mdnr As Integer, ByVal year As Integer, ByVal gavNr As Integer, ByRef err As Boolean) As MD_KK_LMV_Data
		Function LoadEmployeeRPLDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeRPLDataForLOCreation)
		Function LoadEmployeeLMDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeLMDataForLOCreation)
		Function LoadEmployeeSozialleistungpflichtigLMDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeLMDataForLOCreation)
		Function LoadEmployeeZGDataForLOCreation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As IEnumerable(Of EmployeeZGDataForPayroll)
		Function LoadLAData(ByVal year As Integer, ByVal verwendung() As Integer, Optional laNr As Decimal? = Nothing) As IEnumerable(Of LAData)
		Function LoadGAVGruppe0Data(ByVal month As Integer, ByVal year As Integer, ByVal maNr As Integer, ByVal mdNr As Integer) As List(Of GAVNumberLabelData)
		Function LoadGAVGruppe1Data(ByVal month As Integer, ByVal year As Integer, ByVal maNr As Integer, ByVal mdNr As Integer) As String()
		Function LoadMD_KiAuDataData(ByVal mdNr As Integer, ByVal canton As String, ByVal year As Integer, ByRef err As Boolean) As MD_KiAuData
		Function CheckIfLOLExists(ByVal laNrs As String, maNr As Integer, ByVal loNr As Integer, ByVal mdNr As Integer) As Boolean?
		Function LoadSuvaData1ForPayroll(ByVal month As Integer, ByVal year As String, ByVal maNr As Integer, ByVal mdNr As Integer, ByRef err As Boolean) As SuvaData1ForPayroll
		Function LoadSuvaData2ForPayroll(ByVal month As Integer, ByVal year As String, ByVal maNr As Integer, ByVal mdNr As Integer, ByRef err As Boolean) As SuvaData2ForPayroll
		Function LoadWorkedDaysInLpForPayroll(ByVal maNr As Integer, ByVal startOfMonth As DateTime, ByVal endofMonth As DateTime, ByVal endOfYear As DateTime, ByVal mdNr As Integer) As IEnumerable(Of WorkedDaysInLP)
		Function LoadAHVLohnInYear(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As Decimal?
		Function LoadAHVFreibetragForPayroll(ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal monthNumber As Integer, ByVal mdNr As Integer, ByVal err As Boolean) As AHVFreibetragData
		Function LoadAHVRentnerBetragForPayroll(ByVal maNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal mdNr As Integer, ByVal err As Boolean) As Decimal
		Function LoadALV1Lohn(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal ALV1_HL As Decimal, ByVal ESYearTage As Decimal) As Decimal
		Function LoadALV2Lohn(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal ALV2_HL As Decimal, ByVal ESYearTage As Decimal) As Decimal
		Function LoadESDataForBVGDaysInMonthCalculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As IEnumerable(Of ESDataForBVGDaysInMonthCalculation)
		Function LoadBVGAnsMForPayroll(ByVal age As Integer, ByVal year As Integer, ByVal mdNr As Integer) As Decimal?
		Function LoadBVGAnsWForPayroll(ByVal age As Integer, ByVal year As Integer, ByVal mdNr As Integer) As Decimal?
		Function CheckIfLAInLOExists(ByVal loNr As Integer, ByVal mdNr As Integer) As Boolean?
		Function VerifyUnusualPayrollData(ByVal mdNr As Integer, ByVal MANr As Integer, ByVal loNr As Integer, ByVal monat As Integer, ByVal jahr As Integer, ByVal createdUserNumber As Integer, ByVal bvgStartDate As Date?, ByVal bvgToDate As Date?) As IEnumerable(Of PayrollUnusualData)
		Function LoadBVGStdBasis(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As Decimal?
		Function LoadStdInYearForBVG(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As Decimal?
		Function LoadAnzZGABank(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer) As Integer?
		Function LoadAktivMABankDataForPayroll(ByVal maNr As Integer, ByRef err As Boolean) As AktivMABankDataForPayroll
		Function LoadAnzLO(ByVal maNr As Integer, ByVal mdNr As Integer) As Integer?
		Function LoadRPDataForESStdTotalNew0Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStdTotalNew0Calculation)
		Function LoadRPDataForESStdTotalNew1Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStdTotalNew1Calculation)
		Function LoadStdTotalForBVGStdFromRPL0Calculation(ByVal strFieldBez As String, ByVal rpNr As Integer) As Decimal?
		Function LoadStdTotalForBVGStdFromRPL1Calculation(ByVal strFieldBez As String, ByVal rpNr As Integer) As Decimal?
		Function LoadStdTotalForBVGStdFromRPLShorttime(ByVal mdnr As Integer, ByVal maNr As Integer, ByVal monat As Integer, ByVal jahr As Integer) As Decimal?
		Function LoadESDataForESStdTotalCalculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As List(Of ESDataForESStdTotalCalculation)
		Function LoadESDataForRPStdTotal(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal monat As Integer, ByVal jahr As Integer) As Decimal?
		Function LoadLOLDataForESStdTotalCalculation(ByVal maNr As Integer, mdNr As Integer, ByVal month As Integer, ByVal year As Integer) As LOLDataForESStdTotalCalculation
		Function CheckIfESExistsForCheckForBVG(maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As Boolean?
		Function CheckIfExistDiffBeitrag(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer, ByVal mode As Integer) As Boolean?
		Function LoadRPDataForESStd4NewKTG0Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStd4NewKTG0Calculation)
		Function LoadRPDataForESStd4NewKTG1Calculation(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal date1 As DateTime, ByVal date2 As DateTime) As List(Of RPDataForESStd4NewKTG1Calculation)
		Function DoesFilterConditionMatch(ByVal storedProcedureName As String, ByVal maNr As Integer) As Boolean?
		Function LoadCurrentMonthShorttimeWorkAmount(ByVal mdNr As Integer, ByVal maNr As Integer, ByVal lonr As Integer) As Decimal?
		Function DoesFilterConditionMatch(ByVal storedProcedureName As String, ByVal maNr As Integer, ByVal sVar As Decimal) As Boolean?
		Function DoesFilterConditionMatch(ByVal storedProcedureName As String, ByVal maNr As Integer, ByVal sVar1 As Decimal, ByVal sVar2 As Decimal) As Boolean?
		Function LoadFilterConditionDataForCase4(ByVal storedProcedureName As String, ByVal maNr As Integer, ByVal lONewNr As Integer, ByVal err As Boolean) As Decimal?
		Function LoadLOLDataForRepeatLA4LOBack(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer, ByVal laNr As Integer) As IEnumerable(Of LOLDataFoRepeatLA4LOBack)
		Function LoadValueAmountDataForCase(ByVal functionNumber As Decimal, ByVal maNr As Integer, ByVal lONewNr As Integer, ByVal err As Boolean) As Decimal?
		Function LoadLOLDataForGetLMDTAStatus(ByVal manr As Integer, ByVal loNr As Integer, ByRef err As Boolean) As LOLDataForGetLMDTAStatus
		Function LoadHighestZGNrForSaveFinalData() As Integer
		Function LoadHighestLMIDForSaveFinalData() As Integer
		Function LoadLALoTextForSaveFinalData(ByVal year As Integer) As String
		Function LoadLolLANrDataForSaveFinalData(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer, ByVal month As Integer, ByVal year As Integer) As List(Of Integer)
		Function LoadEmployeeBnkDataForPayroll(ByVal maNr As Integer, ByVal loNr As Integer) As EmployeeBnkDataForPayroll
		Function LoadTagGeldBetragForMonth(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal s_canton As String, ByRef err As Boolean) As TagGeldBetragForMonth
		Function LoadQSTInfo(ByVal sCanton As String, ByRef err As Boolean) As TabQSTInfoData
		Function LoadLOLDataForQSTCantonUR(ByVal loNr As Integer, ByVal mdNr As Integer) As LOLDataForQSTCantonUR
		Function LoadESData1ForQSTDataForm(ByVal maNr As Integer, ByVal mdNr As Integer) As IEnumerable(Of ESData1ForQSTDataForm)
		Function LoadESData2ForQSTDataForm(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As IEnumerable(Of ESData2ForQSTDataForm)
		Function LoadFeiertagGuthaben(ByVal maNr As Integer, Optional ByVal esNr As Integer = 0) As GuthabenData
		Function LoadFeiertagGuthaben1(ByVal maNr As Integer) As GuthabenData
		Function LoadFerienGuthaben(ByVal maNr As Integer, Optional ByVal esNr As Integer = 0) As GuthabenData
		Function LoadFerienGuthaben1(ByVal maNr As Integer) As GuthabenData
		Function Load13LohnGuthaben(ByVal maNr As Integer, Optional ByVal esNr As Integer = 0) As GuthabenData
		Function Load13LohnGuthaben1(ByVal maNr As Integer) As GuthabenData
		Function LoadListOfAllKandidaten4NotCreatedZV(ByVal username As String, ByVal mdNr As Integer) As IEnumerable(Of EmployeeDataForZV)
		Function LoadEmployeeData4NotCreatedZV(ByVal employeeNumbers As Integer()) As IEnumerable(Of EmployeeDataForZV)
		Function LoadEmployeesForForgottenZV(ByVal mdNr As Integer, ByVal month As Integer, ByVal year As Integer, ByVal username As String) As IEnumerable(Of EmployeeDataForZV)
		Function IsLOFinished(ByVal loNr As Integer, ByVal mdNr As Integer) As Boolean?
		Function CleanupAllInvalidLO(ByVal mandantNumber As Integer, ByVal year As Integer, ByVal month As Integer) As Boolean
		Function CleanupLO(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer) As Boolean

		Function AddNewLO(ByVal loMasterData As LOMasterData) As Boolean
		Function AddEndDataToLOrec(ByVal data As EndDataForLO) As Boolean
		Function AddLMDataForSaveFinalData(ByVal data As LMDataForSaveFinalData) As Boolean
		Function AddNewLOL(ByVal lolMasterData As LOLMasterData) As Boolean
		Function AddYearCumulativeData(ByVal yearCumulative As YearCumulativeData) As Boolean
		Function AddMonthCumulativeData(ByVal monthCumulative As MonthCumulativeData) As Boolean
		Function LoadLATotalBasisInMonth(ByVal laData As MonthCumulativeData) As Decimal?
		Function AddLOProtocolData(ByVal loProtcolData As LOProtocolData) As Boolean
		Function LoadLOProtocol(ByVal mandantNumber As Integer, ByVal employeeNumber As Integer?, ByVal month As Integer, ByVal year As Integer) As LOProtocolData
		Function LoadAssignedPayrollProtocolData(ByVal mandantNumber As Integer, ByVal employeeNumber As Integer?, ByVal payrollNumber As Integer?, ByVal month As Integer?, ByVal year As Integer?) As LOProtocolData
		Function UpdateZGDataWithLONrForPayroll(ByVal loNewNr As Integer, ByVal zgNr As Integer) As Boolean
		Function UpdateRPDataWithLONrForPayroll(ByVal loNewNr As Integer, ByVal rpNr As Integer) As Boolean
		Function UpdateLOLDataForGetLMDTAStatus(ByVal maNr As Integer, ByVal mdNr As Integer, ByVal loNr As Integer) As Boolean
		Function UpdateLOForSaveFinalData(ByVal lmId As Integer, ByVal loNr As Integer) As Boolean
		Function UpdateLOLForSaveFinalData(ByVal maNr As Integer, ByVal loNr As Integer, ByVal rpText As String) As Boolean
		Function SetLOIsCompleteFlag(ByVal loNr As Integer, ByVal maNr As Integer, ByVal mdNr As Integer) As Boolean
		Function DeleteFromLOL(ByVal loNr As Integer) As Boolean
		Function DeleteAssignedInvalidPayroll(ByVal loNr As Integer) As Boolean


		' Netto
		Function IsVacationBrutto(ByVal mdNr As Integer, ByVal year As Integer, ByVal err As Boolean) As Boolean
		Function LoadFeierBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer, ByVal err As Boolean) As Decimal
		Function LoadFerienBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer, ByVal err As Boolean) As Decimal
		Function Load13LohnBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer, ByVal err As Boolean) As Decimal


#Region "controlling created payrolls"

		Function LoadSuspectPayrollsAfterCreate(ByVal mdNr As Integer, ByVal loNr As Integer()) As IEnumerable(Of SuspectPayrollData)
		Function LoadEmployeesForPrintCheckCashAfterPayroll(ByVal mdNr As Integer, ByVal loNr As Integer()) As IEnumerable(Of PayrollCheckCashData)

#End Region


#Region "fremd guthaben"

		Function LoadFremdGuthabenData(ByVal mandantNumber As Integer, ByVal employeeNumber As Integer) As IEnumerable(Of LOLDataFoRepeatLA4LOBack)
		Function AddFremdGuthabenIntoLOL(ByVal data As LOLDataFoRepeatLA4LOBack) As Boolean
		Function UpdateFremdGuthabenIntoLOL(ByVal data As LOLDataFoRepeatLA4LOBack) As Boolean
		Function DeleteAssignedFremdGuthaben(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteLOLForCorrectionAssignmentResult


#End Region



	End Interface



End Namespace