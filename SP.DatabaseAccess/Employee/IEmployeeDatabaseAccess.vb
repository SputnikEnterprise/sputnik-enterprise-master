Imports SP.DatabaseAccess.Employee.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.MonthlySalary
Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
Imports SP.DatabaseAccess.Employee.DataObjects.JobInterviewMng
Imports SP.DatabaseAccess.Employee.DataObjects.TodoMng
Imports SP.DatabaseAccess.Employee.DataObjects.MediationMng
Imports SP.DatabaseAccess.Employee.DataObjects.LanguagesAndProfessionsMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary

Imports SP.DatabaseAccess.Employee.DataObjects.AdvancedPaymentMng
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Applicant.DataObjects

Namespace Employee

  ''' <summary>
  ''' Interface for employee database access.
  ''' </summary>
  Public Interface IEmployeeDatabaseAccess

#Region "MasterdataManagement"


    Function LoadEmployeeMasterData(ByVal employeeNumber As Integer, Optional includeImageData As Boolean = False) As EmployeeMasterData
		'Function LoadEmployeeQSTCommunities() As IEnumerable(Of EmployeeQSTCommunityData)
		Function LoadEmployeeBeforeEQuestBackup(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeBackupBeforeEQuestData)
		Function UpdateEmployeeBeforeEQuestBackup(ByVal employeeNumber As Integer?, ByVal checkedUserNumber As Integer) As Boolean
		Function LoadEmployeeQSTCommunitiesWithCanton(ByVal canton As String) As IEnumerable(Of EmployeeQSTCommunityData)
		Function LoadExistingEmployeesBySearchCriteria(ByVal lastname As String, ByVal firstname As String, ByVal street As String, ByVal postcode As String, ByVal location As String, ByVal countryCode As String) As IEnumerable(Of ExistingEmployeeSearchData)
		Function LoadAssignedEmployeesBySearchCriteria(ByVal mySearchData As ExistingEmployeeSearchData) As IEnumerable(Of ExistingEmployeeSearchData)
		Function LoadEmployeeBusinessBranches(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeBusinessBranchData)
		Function LoadEmployeeNoticesData(ByVal employeeNumber As Integer) As EmployeeNoticesData
		Function LoadEmployeeStateData1() As IEnumerable(Of EmployeeStateData)
		Function LoadEmployeeStateData2() As IEnumerable(Of EmployeeStateData)
    Function LoadEmployeeContactsInfo() As IEnumerable(Of EmployeeContactInfoData)
    Function LoadEmployeeContactCommData(ByVal employeeNumber As Integer) As EmployeeContactComm
    Function LoadEmployeeOtherData(ByVal employeeNumber As Integer) As EmployeeOtherData
    Function LoadEmployeeESStateData(ByVal employeeNumber As Integer) As EmployeeESStateData
    Function LoadEmployeeProposeStateData(ByVal employeeNumber As Integer) As EmployeeProposeStateData
    Function LoadQSTData() As IEnumerable(Of QSTData)
    Function LoadChurchTaxCodeData() As IEnumerable(Of ChurchTaxCodeData)
    Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint)
    Function LoadContextMenu4PrintTemplatesData() As IEnumerable(Of ContextMenuForPrintTemplates)
    Function AddNewEmployee(ByVal initData As NewEmployeeInitData) As Boolean
    Function AddEmployeeBussinessBranch(ByVal employeeBusinessBranch As EmployeeBusinessBranchData) As Boolean
    Function UpdateEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData) As Boolean
		Function UpdateEmployeeGeoData(ByVal employee As EmployeeMasterData) As Boolean
		Function UpdateEmployeeNoticesData(ByVal notticeData As EmployeeNoticesData) As Boolean
		Function UpdateEmployeePictureByteData(ByVal employeeNumber As Integer, ByVal filebytes() As Byte) As Boolean
		Function UpdateEmployeeConactCommData(ByVal employeeContactCommData As EmployeeContactComm) As Boolean
    Function UpdateEmployeeOtherData(ByVal emplyoeeOtherData As EmployeeOtherData) As Boolean
		Function UpdateEmployeeBackupHistory(ByVal previousEmployeeMasterData As EmployeeMasterData, ByVal previousEmployeeLOSettingData As EmployeeLOSettingsData,
											 ByVal NewemployeeMasterData As EmployeeMasterData, ByVal NewemployeeLOSettingData As EmployeeLOSettingsData,
											 ByVal createdUserNumber As Integer) As Boolean
		Function ChangeEployeeDataWithApplicantData(ByVal existingEmployeeNumber As Integer, ByVal applicantEmployeeNumber As Integer) As Boolean
		Function DeleteEmployeeBusinessBranch(ByVal id As Integer) As Boolean
		Function DeleteEmployee(employeenumber As Integer, modul As String, username As String, usnr As Integer) As DeleteEmployeeResult


    Function LoadFoundedPayrollForEmployeeMng(employeeNumber As Integer?) As IEnumerable(Of EmployeePayrollProperty)
    Function LoadFoundedAdvancePaymentForEmployeeMng(employeeNumber As Integer?) As IEnumerable(Of EmployeeAdvancePaymentProperty)
    Function LoadFoundedRPForEmployeeMng(employeeNumber As Integer?) As IEnumerable(Of EmployeeReportsProperty)
    Function LoadFoundedESForEmployeeMng(employeeNumber As Integer?) As IEnumerable(Of EmployeeESProperty)
    Function LoadFoundedOfferForEmployeeMng(employeeNumber As Integer?) As IEnumerable(Of EmployeeOfferProperty)
    Function LoadFoundedProposeForEmployeeMng(employeeNumber As Integer?) As IEnumerable(Of EmployeeProposeProperty)

    Function LoadPrintedDocumentsForEmployeeMng(employeeNumber As Integer?, getARG As Boolean, getZV As Boolean, getPayroll As Boolean, getNLA As Boolean, getThisYear As Boolean) As IEnumerable(Of EmployeePrintedDocProperty)
    Function DeletePrintedDocumentsForEmployeeMng(employeeNumber As Integer?, ByVal recID As Integer) As Boolean
		Function LoadEmployeePayrollHistoryDataForEmployeeMng(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeBackupHistoryData)


		' Propose
		Function LoadEmployeeJobInterviewsForPropose(employeeNumber As Integer, proposeNumber As Integer, Optional interviewRecorNumber As Integer? = Nothing) As IEnumerable(Of EmployeeJobAppointmentData)
    Function LoadEmployeeContactOverviewDataForPropose(employeeNumber As Integer, proposeNumber As Integer, bHideTel As Boolean, bHideOffer As Boolean, bHideMail As Boolean, bHideSMS As Boolean, years As Integer()) As IEnumerable(Of EmployeeContactOverviewdata)


#End Region

#Region "MediationManagement"

    Function LoadEmployeeEmployementTypeData() As IEnumerable(Of EmployeeEmployementType)
    Function LoadEmployeeAssignedEmploymentTypeData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedEmploymentTypeData)
    Function LoadEmployeeCommunicationData() As IEnumerable(Of EmployeeCommunicationData)
    Function LoadEmployeeAssignedCommunicationData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedCommunicationData)
    Function LoadAssessmentData() As IEnumerable(Of AssessmentData)
    Function LoadEmployeeAssignedAssessmentData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedAssessmentData)
    Function LoadDrivingLicenceData() As IEnumerable(Of DrivingLicenceData)
    Function LoadVehicleData() As IEnumerable(Of VehicleData)
    Function LoadCarReserveData() As IEnumerable(Of CarReserveData)
    Function LoadContactReserveData(ByVal contactReserveType As EmployeeContactReserveType) As IEnumerable(Of ContactReserveData)
    Function LoadEmployeeContactCommReserve5Data() As IEnumerable(Of ContactReserve5Data)
    Function LoadDeadLineData() As IEnumerable(Of DeadlineData)
    Function LoadWorkPensumData() As IEnumerable(Of WorkPensumData)
    Function AddEmployeeEmploymentTypeAssignment(ByVal employeeEmploymentTypeAssignment As EmployeeAssignedEmploymentTypeData) As Boolean
    Function AddEmployeeCommunicationAssignment(ByVal coummunicationAssignment As EmployeeAssignedCommunicationData) As Boolean
    Function AddEmployeeAssessmentAssignment(ByVal assessmentAssignment As EmployeeAssignedAssessmentData) As Boolean
    Function DeleteEmployeeEmploymentTypeAssignment(ByVal id As Integer) As Boolean
    Function DeleteEmployeeCommunicationAssignment(ByVal id As Integer) As Boolean
    Function DeleteEmployeeAssessmentAssignment(ByVal id As Integer) As Boolean

#End Region

#Region "LanguagesAndProfessions"

    Function LoadJobCandidateLanguageData() As IEnumerable(Of JobCandidateLanguageData)
    Function LoadEmployeeAssignedProfessionData(ByVal employeeNumber As Integer, Optional ByVal gender As Char = "M") As IEnumerable(Of EmployeeAssignedProfessionData)
    Function LoadEmployeeAssignedSectorData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedSectorData)
    Function LoadEmployeeAssignedVerbalLanguageData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedVerbalLanguageData)
    Function LoadEmployeeAssignedWrittenLanguageData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeAssignedWrittenLanguageData)
    Function AddEmployeeProfessionAssignment(ByVal professionAssignment As EmployeeAssignedProfessionData) As Boolean
    Function AddEmployeeSectorAssignment(ByVal sectorAssignment As EmployeeAssignedSectorData) As Boolean
    Function AddEmployeeVerbalLanguageAssignment(ByVal verbalLanguageAssignment As EmployeeAssignedVerbalLanguageData) As Boolean
    Function AddEmployeeWrittenLaguageAssignment(ByVal writtenLaguageAssignment As EmployeeAssignedWrittenLanguageData) As Boolean
    Function DeleteEmployeeProfessionDataAssignment(ByVal id As Integer) As Boolean
    Function DeleteEmployeeSectorAssignment(ByVal id As Integer) As Boolean
    Function DeleteEmployeeVerbalLanguageAssignment(ByVal id As Integer) As Boolean
    Function DeleteEmployeeWrittenLanguageAssignment(ByVal id As Integer) As Boolean
#End Region

#Region "ContactManagemnet"
    Function LoadEmployeeContactOverviewDataBySearchCriteria(ByVal employeeNumber As Integer, ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean, ByVal bHideSMS As Boolean, ByVal years As Integer()) As IEnumerable(Of EmployeeContactOverviewdata)
    Function LoadEmployeeContactDistinctYears(ByVal employeeNumber As Integer) As IEnumerable(Of Integer)
    Function LoadEmployeeContact(ByVal employeeNumber As Integer, ByVal recordNumber As Integer) As EmployeeContactData

    Function LoadContactDocumentData(ByVal docID As Integer, ByVal includeFileBytes As Boolean) As ContactDoc
    Function UpdateEmployeeContact(ByVal contactData As EmployeeContactData) As Boolean
		Function AddEmployeeContact(ByVal contactData As EmployeeContactData) As Boolean
		Function DeleteEmployeeContact(ByVal id As Integer) As Boolean

    Function AddContactDocument(ByVal contactDocument As ContactDoc) As Boolean
    Function UpdateContactDocumentData(ByVal contactDocument As ContactDoc, ByVal ignoreFileBytes As Boolean) As Boolean
    Function DeleteContactDocument(ByVal id As Integer) As Boolean

    Function LoadCustomerDataForContactMng() As IEnumerable(Of CustomerDataForContactMng)
    Function LoadVacancyDataForContactMng(Optional ByVal customerNumber As Integer? = Nothing) As IEnumerable(Of VacancyDataForContactMng)
    Function LoadProposeDataForContactMng(Optional ByVal employeeNumber As Integer? = Nothing, Optional ByVal customerNumber As Integer? = Nothing) As IEnumerable(Of ProposeDataForContactMng)
    Function LoadESDataForContactMng(Optional ByVal employeeNumber As Integer? = Nothing, Optional ByVal customerNumber As Integer? = Nothing) As IEnumerable(Of ESDataForContactMng)
    Function LoadEmployeeDependentCustomerContactData(ByVal maRecID As Integer) As EmployeeDependentCustomerContactData

#End Region

#Region "DocumentManagement"
    Function LoadEmployeeDocumentCategories() As IEnumerable(Of EmployeeDocumentCategoryData)
    Function LoadDistinctDocumentCategoriesOfEmployee(ByVal employeeNumber As Integer?) As IEnumerable(Of EmployeeDocumentCategoryData)
    Function LoadEmployeeDocuments(ByVal employeeNumber As Integer, Optional documentRecordNumber As Integer? = Nothing, Optional categoryNumber As Integer? = Nothing) As IEnumerable(Of EmployeeDocumentData)
    Function LoadEmployeeCV(ByVal employeeNumber As Integer, Optional documentRecordNumber As Integer? = Nothing) As IEnumerable(Of EmployeeCVData)
		Function LoadAssingedEmployeeCVData(ByVal employeeNumber As Integer, ByVal templateName As String, ByVal withBinary As Boolean) As EmployeeCVData
		Function DeleteAssignedEmployeeCVDocument(ByVal id As Integer, ByVal createdUserNumber As Integer, ByVal userName As String) As Boolean
		Function LoadEmployeeDocumentBytesData(ByVal documentId As Integer) As Byte()
		Function LoadEmployeePrintedDocumentBytesData(ByVal documentId As Integer) As Byte()
    Function AddEmployeeDocument(ByVal documentData As EmployeeDocumentData) As Boolean
    Function UpdateEmployeedDocument(ByVal documentData As EmployeeDocumentData) As Boolean
    Function UpdateEmployeeDocumentByteData(ByVal documentId As Integer, ByVal filebytes() As Byte, ByVal fileExtension As String) As Boolean
    Function DeleteEmployeeDocument(ByVal id As Integer) As Boolean
#End Region

#Region "LLZusatzfields"

		Function LoadLLZusatzFieldsData(ByVal dbFieldName As String, ByVal showInMAVersand As Boolean, ByVal showInProposeNavBar As Boolean) As IEnumerable(Of LLZusatzFieldsData)
		Function LoadLLZusatzFieldsTemplateData(ByVal dbFieldName As String) As IEnumerable(Of LLZusatzFieldsTemplateData)

#End Region

#Region "BankManagement"
		Function LoadEmployeeBanks(ByVal employeeNumber As Integer, Optional ByVal bankRecordNumber As Integer? = Nothing, Optional ByVal bankforAdvancePayment As Boolean? = Nothing) As IEnumerable(Of EmployeeBankData)
    Function AddEmployeeBank(ByVal bankData As EmployeeBankData) As Boolean
    Function UpdateEmployeeBank(ByVal bankData As EmployeeBankData) As Boolean
    Function DeleteEmployeeBank(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeBankResult
    Function SearchBankData(ByVal clearingNumber As String, ByVal bankName As String, ByVal bankPostcode As String, ByVal bankLocation As String, ByVal swift As String) As IEnumerable(Of SearchBankDataResult)
#End Region

#Region "JobInterviewManagement"
    Function LoadEmployeeJobInterviews(ByVal employeeNumber As Integer, Optional ByVal interviewRecorNumber As Integer? = Nothing) As IEnumerable(Of EmployeeJobAppointmentData)
    Function LoadCustomerDataForJobInterviewMng() As IEnumerable(Of Employee.DataObjects.JobInterviewMng.CustomerData)
    Function LoadResponsiblePersonDataForJobInterviewMng(ByVal customerNumber As Integer) As IEnumerable(Of Employee.DataObjects.JobInterviewMng.ResponsiblePersonData)
    Function LoadJobAppointmentStateDataeForJobInterviewMng() As IEnumerable(Of Employee.DataObjects.JobInterviewMng.JobAppointmentStateData)
    Function LoadVacancyDataForJobInterviewMng(ByVal customerNumber As Integer) As IEnumerable(Of Employee.DataObjects.JobInterviewMng.VacancyData)
    Function LoadProposeDataForJobInterviewMng(ByVal customerNumber As Integer, ByVal employeeNumber As Integer) As IEnumerable(Of Employee.DataObjects.JobInterviewMng.ProposeData)
    Function AddEmployeeJobInterview(ByVal interviewData As EmployeeJobAppointmentData) As Boolean
    Function UpdateEmployeeJobInterview(ByVal interviewData As EmployeeJobAppointmentData) As Boolean
    Function DeleteEmployeeJobInterview(ByVal id As Integer) As DeleteEmployeeJobAppointmentResult
#End Region

#Region "MonthlySalaryManagement"
    Function LoadEmployeeOverviewListForMonthlySalaryMng() As IEnumerable(Of EmployeeOverviewData)
    Function LoadMonthlySalaryOverviewListForMonthlySalaryMng(ByVal employeeNumber As Integer, ByVal mdnumber As Integer, Optional ByVal onlyCurrentYear As Boolean = False) As IEnumerable(Of MonthlySalaryOverviewData)
    Function LoadESListForMonthlySalaryMng(ByVal employeeNumber As Integer, ByVal mdnumber As Integer) As IEnumerable(Of ESData)
    Function LoadLAListForMonthlySalaryMng(ByVal year As Integer) As IEnumerable(Of LAData)
		Function LoadCantonListForMonthlySalaryMng(ByVal emplyeeNumber As Integer, ByVal mandantenNummer As Integer) As IEnumerable(Of Employee.DataObjects.MonthlySalary.CantonData)
		Function LoadEmployeeBankListForMonthlySalaryMng(ByVal employeeNumber As Integer) As IEnumerable(Of BankData)
    Function LoadEmployeeLOSettingForMonthlySalaryMng(ByVal employeeNumber As Integer) As EmployeeLOSettingData
    Function AddLM(ByVal lmData As LMData) As Integer?
    Function UpdateLM(ByVal lmData As LMData) As Boolean
    Function LoadConflictedLOLRecordsForMonthlySalaryMng(ByVal employeeNumber As Integer, ByVal esNr As Integer?, ByVal lmNr As Integer?, ByVal firstMonth As Integer, ByVal firstYear As Integer, ByVal lastMonth As Integer, ByVal lastYear As Integer, ByRef resultCode As Integer) As IEnumerable(Of ConflictedLOLData)
    Function CheckForExistingLMInPeriodWithLANr(ByVal employeeNumber As Integer, ByVal laNANr As Decimal, ByVal firstMonth As Integer, ByVal firstYear As Integer, ByVal lastMonth As Integer, ByVal lastYear As Integer, ByRef result As Boolean) As Boolean
		Function DeleteLM(ByVal lmNr As Integer, ByVal employeeNumber As Integer, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeLMResult
		Function LoadLMDocListForLM(ByVal lmNr As Integer, Optional ByVal recordNumber As Integer? = Nothing, Optional includeFileBytes As Boolean = False) As IEnumerable(Of LMDocData)
    Function AddLMDoc(ByVal lmDoc As LMDocData) As Boolean
    Function UdateLMDoc(ByVal lmDoc As LMDocData, Optional updateDocScan As Boolean = False) As Boolean
		Function DeleteLMDoc(ByVal id As Integer) As Boolean

#End Region

#Region "SalaryManagement"
		Function LoadEmployeeLOSettings(ByVal employeeNumber As Integer) As EmployeeLOSettingsData
    Function LoadAHVData() As IEnumerable(Of AHVData)
    Function LoadALVData() As IEnumerable(Of ALVData)
    Function LoadBVGData() As IEnumerable(Of BVGData)
    Function LoadSuva2Data() As IEnumerable(Of Suva2Data)
    Function LoadPaymentMethodData() As IEnumerable(Of PaymentMethodData)
    Function LoadCurrenyData() As IEnumerable(Of CurrencyData)
    Function UpdateEmployeeLOSettings(ByVal employeeLOSettings As EmployeeLOSettingsData) As Boolean
#End Region

#Region "AdvancedPaymentManagement"
    Function LoadEmployeeAdvancedPaymentSettings(ByVal employeeNumber As Integer, ByVal Month As Integer?, ByVal lang As String,
                                                 ByVal mdnumber As Integer, Optional ByVal onlyCurrentYear As Boolean = False) As IEnumerable(Of EmployeeAdvancedPaymentData)

#End Region


#Region "applicationMng"

		Function LoadAssignedEmployeeApplications(ByVal customer_ID As String, ByVal employeeNumber As Integer, ByVal applicationNumber As Integer?, ByVal onlyActive As Boolean?) As IEnumerable(Of ApplicationData)
		Function UpdateApplicantToEmployee(ByVal employeeMasterData As EmployeeMasterData) As Boolean
		Function UpdateApplicantFlagData(ByVal employeeMasterData As EmployeeMasterData) As Boolean

#End Region


#Region "TodoManagement"

		Function LoadTodoListDataBySearchCriteria(ByVal customerID As String, ByVal UserNrs As String, ByVal callerUserNumber As Integer) As IEnumerable(Of TodoListData)
		Function LoadTodoDataForAutoCreatedNotify(ByVal customerID As String, ByVal UserNrs As String, ByVal inputSource As Integer, ByVal modulNumber As Integer, ByVal body As String) As IEnumerable(Of TodoData)
		Function LoadTodoData(ByVal customerID As String, ByVal recID As Integer?, ByVal callerUserNumber As Integer) As TodoData
		Function LoadCustomerNameData() As IEnumerable(Of CustomerNameData)
    Function LoadZHDNameData(ByVal KDNr As Integer) As IEnumerable(Of ZHDNameData)

		Function UpdateTodoData(ByVal customerID As String, ByVal todoData As TodoData) As Boolean
		Function InsertTodoUserData(ByVal customerID As String, ByVal todoUserData As TodoUserData) As Boolean
		Function InsertTodoData(ByVal customerID As String, ByVal todoData As TodoData) As Boolean
		Function DeleteTodo(ByVal RecNr As Integer) As Boolean
		Function DeleteTodoUserData(ByVal recID As Integer, ByVal username As String, ByVal usnr As Integer) As Boolean
		Function LoadUserImageData(ByVal USNr As Integer) As UserImageData
		Function LoadTodoUserData(ByVal todoID As Integer?) As IEnumerable(Of TodoUserData)
		Function LoadUserDataFromTODOList(ByVal customerID As String) As IEnumerable(Of TodoUserData)

#End Region


#Region "NLA Data"

		Function LoadEmployeeNLAData(ByVal employeeNumber As Integer) As EmployeeNLAData
		Function SaveEmployeeNLAData(ByVal data As EmployeeNLAData, ByVal employeeNumber As Integer) As Boolean
		Function DeleteEmployeeNLAData(ByVal employeeNumber As Integer) As Boolean

#End Region


#Region "Ki-Au Data"

		Function LoadEmployeeKiAuData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeChldData)
    Function LoadAssignedEmployeeKiAuData(ByVal recID As Integer) As EmployeeChldData
    Function SaveEmployeeKiAuData(ByVal data As EmployeeChldData) As Boolean
    Function AddEmployeeKiAuData(ByVal data As EmployeeChldData) As Boolean
    Function DeleteEmployeeKiAuData(ByVal recID As Integer) As Boolean

#End Region


#Region "divers Addresses"

    Function LoadEmployeeDivAddressData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeSAddressData)
    Function AddEmployeeDivAddressData(ByVal employeeNumber As Integer, ByVal data As EmployeeSAddressData) As Boolean
    Function UpdateEmployeeDivAddressData(ByVal employeeNumber As Integer, ByVal data As EmployeeSAddressData) As Boolean
    Function DeleteEmployeeDivAddressData(ByVal recID As Integer) As Boolean


		Function LoadEmployeeReportAddressData(ByVal employeeNumber As Integer) As EmployeeSAddressData

#End Region


#Region "partnership"

		Function LoadExistingEmployeeForSelectingPartnershipData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeMasterData)
		Function LoadEmployeePartnershipData(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeePartnershipData)
		Function LoadEmployeeAssignedPartnershipData(ByVal recID As Integer) As EmployeePartnershipData
		Function AddEmployeePartnershipData(ByVal employeeNumber As Integer, ByVal data As EmployeePartnershipData) As Boolean
		Function UpdateEmployeePartnershipData(ByVal employeeNumber As Integer, ByVal data As EmployeePartnershipData) As Boolean
		Function DeleteEmployeePartnershipData(ByVal recID As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteEmployeeResult

#End Region


#Region "ZV forms"

		Function LoadEmployeeZvAddressData(ByVal employeeNumber As Integer) As EmployeeSAddressData
		Function LoadESData2ForZVForm(ByVal mdNr As Integer, ByVal maNr As Integer, ByVal startOfMonth As DateTime, ByVal endOfMonth As DateTime) As IEnumerable(Of ZVESData)
		Function GetEmployeeMonthHoursAndAbsenceData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As ZVHourAbsenceData
		Function GetEmployeeMonthHoursGroupedByKSTData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As IEnumerable(Of WorkingHourGroupedWithKSTNrData)
		Function LoadEmployeePayrollData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal year As Integer, ByVal month As Integer) As IEnumerable(Of ZVPayrollData)

		Function LoadEmployeeDocumentForZVData(ByVal employeeNumber As Integer, ByVal recNumber As Integer, ByVal categoryNumber As Integer) As EmployeeDocumentData
    Function LoadEmployeePrintedDocumentForZVData(ByVal employeeNumber As Integer, ByVal month As Integer, ByVal year As Integer, ByVal categoryNumber As Integer) As EmployeeDocumentData

    Function LoadEmployeeARGBAddressData(ByVal employeeNumber As Integer) As EmployeeSAddressData
    Function LoadEmployeePayrollForARGBData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As IEnumerable(Of ARGBPayrollData)
    Function LoadEmployeePayrollForLohnkontiData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As IEnumerable(Of ARGBPayrollData)
    Function LoadEmployeeAHVPayrollForARGBLastMonthData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date) As IEnumerable(Of ARGBAHVPayrollData)
    Function LoadEmployeePayrollForPrintWithARGBData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As IEnumerable(Of PayrollPrintData)

#End Region


#Region "WOS-Setting"

		Function LoadEmployeeDataForWOSExport(ByVal userNumber As Integer?, ByVal employeeNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal rplNumber As Integer?, ByVal rpDocNumber As Integer?, ByVal payrollNumber As Integer?) As EmployeeWOSData
		Function LoadAvailableEmployeeDataForWOSExport(ByVal employeeNumber As Integer?, ByVal logedUserNr As Integer) As AvailableEmployeeWOSData
		'Function LoadEmployeeDataForEmployeeWOSExport(ByVal userNumber As Integer?, ByVal employeeNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal rplNumber As Integer?, ByVal rpDocNumber As Integer?, ByVal payrollNumber As Integer?) As DataTable
		'Function CacheEmployeeWOSDataTemporary(ByVal employeeData As EmployeeWOSData) As Boolean

#End Region


#Region "QuickSearch"

		Function LoadExistingEmployeesANDApplicantData(ByVal mdNr As Integer) As IEnumerable(Of ExistingEmployeeSearchData)
		Function LoadQuickSearchDataWithStoredProcedure(ByVal mdNr As Integer, ByVal storedProcedureName As String) As IEnumerable(Of ExistingEmployeeSearchData)

#End Region



	End Interface

  ''' <summary>
  ''' Employee Contact reserve type.
  ''' </summary>
  Public Enum EmployeeContactReserveType
    Reserve1 = 1
    Reserve2 = 2
    Reserve3 = 3
    Reserve4 = 4
  End Enum

  ''' <summary>
  ''' Result of employee (Kandidat) deletion.
  ''' </summary>
  ''' <remarks></remarks>
  Public Enum DeleteEmployeeResult
    Deleted = 2
    CouldNotDeleteBecauseOfExistingPropose = 4

    CouldNotDeleteBecauseOfExistingES = 10
    CouldNotDeleteBecauseOfExistingRP = 11
    CouldNotDeleteBecauseOfExistingZG = 12

    CouldNotDeleteBecauseOfExistingLM = 13
    CouldNotDeleteBecauseOfExistingLO = 14

    ErrorWhileDelete = 20
  End Enum

	''' <summary>
	''' Result of employee lm deletion.
	''' </summary>
	Public Enum DeleteEmployeeLMResult
		Deleted = 1
		CanNotDeleteBecauseMonthIsClosed = 2
		CouldNotDeleteBecauseOfExistingLO = 14
		ErrorWhileDelete = 20
	End Enum

	''' <summary>
	''' Result of employee bank (MA_Bank) deletion.
	''' </summary>
	Public Enum DeleteEmployeeBankResult
    Deleted = 1
    CouldNotDeleteBecauseOfExistingLM = 2
    ErrorWhileDelete = 3
  End Enum

  ''' <summary>
  ''' Result of employee job appointment (MA_JobTermin) deletion.
  ''' </summary>
  Public Enum DeleteEmployeeJobAppointmentResult
    Deleted = 1
    ErrorWhileDelete = 2
  End Enum

  ''' <summary>
  ''' Result of emplyoee ES state result.
  ''' </summary>
  Public Enum EmployeeESStateResult
    State_Has_An_Active_ES = 1
    State_ES_In_Future = 2
    State_NoES = 10
  End Enum

  ''' <summary>
  ''' Result of emplyoee propose state result.
  ''' </summary>
  Public Enum EmployeeProposeStateResult
    State_Has_Active_Propose = 1
    State_Has_No_Active_Propose = 10
  End Enum


End Namespace