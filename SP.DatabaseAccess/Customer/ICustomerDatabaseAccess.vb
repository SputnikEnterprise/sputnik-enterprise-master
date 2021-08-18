Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Namespace Customer

	''' <summary>
	''' Interface for customer database access.
	''' </summary>
	Public Interface ICustomerDatabaseAccess


		Function LoadCustomerMasterData(ByVal customerNumber As Integer, ByVal usFiliale As String) As CustomerMasterData
		Function LoadCustomerData(Optional ByVal usFiliale As String = "") As IEnumerable(Of CustomerMasterData)
		Function LoadResponsiblePersonMasterData(ByVal customerNumer As Integer, ByVal recordNumber As Integer) As ResponsiblePersonMasterData
		Function LoadResponsiblePersonsOverviewDataForPersonManagement(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonOverviewDataForPersonManagement)
		Function LoadResponsiblePersonData(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData)
		Function LoadResponsiblePersonDataActiv(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData)

		Function LoadAssignedBusinessBranchsDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedBusinessBranchData)
		Function LoadAssignedProfessionDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedProfessionData)
		Function LoadAssignedProfessionDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedProfessionData)
		Function LoadAssignedSectorDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedSectorData)
		Function LoadAssignedSectorDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedSectorData)
		Function LoadAssignedEmploymentTypeDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedEmploymentTypeData)
		Function LoadAssignedKeywordDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedKeywordData)
		Function LoadAssignedGAVGroupDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedGAVGroupData)
		Function LoadAllCustomerGAVGroupData(ByVal mdNr As Integer, ByVal kst As String) As IEnumerable(Of CustomerAssignedGAVGroupData)
		Function LoadAssignedInvoiceAddressDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedInvoiceAddressData)
		Function LoadAssignedInvoiceAddressDataOfCustomerByRecordNumber(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As CustomerAssignedInvoiceAddressData
		Function LoadAssignedKSTsOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedKSTData)
		Function LoadAssignedKSTsOfCustomerByRecordNumber(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As CustomerAssignedKSTData
		Function LoadAssignedEmailsOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedEmailData)
		Function LoadAssignedOpenDebitorInvoicesOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedInvoiceData)
		Function LoadAssignedResponsiblePersonDocumentData(ByVal customerNumber As Integer, Optional ByVal responsiblePersonRecordNumber As Integer? = Nothing, Optional documentRecordNumber As Integer? = Nothing, Optional categoryNumber As Integer? = Nothing) As IEnumerable(Of ResponsiblePersonAssignedDocumentData)
		Function LoadAssignedResponsiblePersonDocumentBytesData(ByVal documentId As Integer) As Byte()
		Function LoadAssignedCreditInfosOfCustomer(ByVal customerNumber As Integer, ByVal recordNumber? As Integer, ByVal includeReportFileBytes As Boolean) As IEnumerable(Of CustomerAssignedCreditInfo)
		Function LoadAsssignedCommunicationDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedCommuncationData)
		Function LoadAssignedConcatTypeDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedContactTypeData)
		Function LoadAssignedReserveDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer, ByVal reserveType As ResponsiblePersonReserveDataType) As IEnumerable(Of ResponsiblePersonAssignedReserveData)
		Function LoadAssignedContactDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonNumber As Integer?, ByVal recordNumber As Integer) As ResponsiblePersonAssignedContactData
		Function LoadSalesVolumeDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of SalesVolumeData)
		Function LoadDistinctDocumentCategorieDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?) As IEnumerable(Of CustomerDocumentCategoryData)
		Function LoadLatestCustomerSolvencyCheckCreditInfo(ByVal customerNumber As Integer, ByVal includeReportFileBytes As Boolean) As CustomerAssignedCreditInfo
		Function LoadUserData() As IEnumerable(Of UserData)
		Function LoadCustomerContactInfoData() As IEnumerable(Of CustomerContactInfoData)
		Function LoadCustomerCommunicationData() As IEnumerable(Of CustomerCommunicationData)
		Function LoadCustomerCommunicationTypeData() As IEnumerable(Of CustomerCommunicationTypeData)
		Function LoadResponsiblePersonContactInfoData() As IEnumerable(Of ResponsiblePersonContactInfo)
		Function LoadResponsiblePersonReserveData(ByVal reserveType As ResponsiblePersonReserveDataType) As IEnumerable(Of ResponsiblePersonReserveData)
		Function LoadCustomerStateData1() As IEnumerable(Of CustomerStateData)
		Function LoadCustomerStateData2() As IEnumerable(Of CustomerStateData)
		Function LoadResponsiblePersonStateData1() As IEnumerable(Of ResponsiblePersonStateData)
		Function LoadResponsiblePersonStateData2() As IEnumerable(Of ResponsiblePersonStateData)
		Function LoadFirstPropertyData() As IEnumerable(Of FirstPropertyData)
		Function LoadSecondPropertyData() As IEnumerable(Of SecondPropertyData)
		Function LoadEmploymentTypeData() As IEnumerable(Of EmploymentTypeData)
		Function LoadKeywordData() As IEnumerable(Of KeywordData)
		Function LoadOPShipmentData() As IEnumerable(Of OPShipmentData)
		Function LoadInvoiceTypeData() As IEnumerable(Of InvoiceTypeData)
		Function LoadNumberOfEmployeesData() As IEnumerable(Of NumberOfEmployeesData)
		Function LoadInvoiceOptionsData() As IEnumerable(Of InvoiceOptionData)
		Function LoadPaymentConditionData() As IEnumerable(Of PaymentConditionData)
		Function LoadPaymentReminderCodeData() As IEnumerable(Of PaymentReminderCodeData)
		Function LoadCustomerReserveData(ByVal reserveDataType As CustomerReserveDataType) As IEnumerable(Of CustomerReserveData)
		Function LoadCustomerDocumentCategoryData() As IEnumerable(Of CustomerDocumentCategoryData)
		Function LoadDepartmentData() As IEnumerable(Of DepartmentData)
		Function LoadPositionData() As IEnumerable(Of PositionData)
		Function LoadEmployeeData(Optional ByVal maNr As Integer? = Nothing) As IEnumerable(Of EmployeeData)
		Function LoadVacancyData(ByVal customerNumber As Integer, Optional ByVal responsiblePersonNumber As Integer? = Nothing) As IEnumerable(Of VacancyData)
		Function LoadProposeData(ByVal customerNumber As Integer, Optional ByVal responsiblePersonNumber As Integer? = Nothing) As IEnumerable(Of ProposeData)
		Function LoadESData(ByVal customerNumber As Integer) As IEnumerable(Of ESData)
		Function LoadContactDocumentData(ByVal contactId As Integer, ByVal includeFileBytes As Boolean) As ContactDoc
		Function LoadExistingCustomersBySearchCriteria(ByVal company As String, ByVal street As String, ByVal postcode As String, ByVal location As String, ByVal countryCode As String) As IEnumerable(Of ExistingCustomerSearchData)
		Function LoadCustomerContactOverviewlDataBySearchCriteria(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean, ByVal bHideSMS As Boolean, ByVal years As Integer()) As IEnumerable(Of CustomerContactOverviewData)
		Function LoadCustomerContactTotalDistinctYears(ByVal customerNumber As Integer, Optional ByVal responsiblePersonRecordNumber As Integer? = Nothing) As IEnumerable(Of Integer)
		Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint)
		Function LoadContextMenu4PrintTemplatesData() As IEnumerable(Of ContextMenuForPrintTemplates)
		Function LoadCustomerDependentEmployeeContactData(ByVal kdRecID As Integer) As IEnumerable(Of CustomerDependentEmployeeContactData)
		'Function AddNewCustomer(ByVal company As String, ByVal street As String, ByVal country As String, ByVal plz As Integer,
		'												ByVal location As String, ByVal Kst As String, ByVal customerNumberOffset As Integer, ByVal usFiliale As String, ByVal createdBy As String,
		'												ByVal iMDNr As Integer,
		'												ByVal currencyvalue As String,
		'												ByVal invoiceremindercode As String,
		'												ByVal conditionalcash As String,
		'												ByVal invoicetype As String,
		'												ByVal customernotuse As Boolean?,
		'												ByVal warnbycreditlimitexceeded As Boolean?,
		'												ByVal firstcreditlimitamount As Decimal?,
		'												ByVal secondcreditlimitamount As Decimal?) As Integer

		Function AddNewCustomer(ByVal customer As NewCustomerInitData) As Boolean
		Function AddCustomerCreditInfoAssignment(ByVal customerCreditInfo As CustomerAssignedCreditInfo) As Boolean
		Function AddNewResponsiblePerson(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData) As Boolean
		Function AddCustomerBussinessBranchAssignment(ByVal customerBusinessBranch As CustomerAssignedBusinessBranchData) As Boolean
		Function AddCustomerProfessionAssignment(ByVal customerProfession As CustomerAssignedProfessionData) As Boolean
		Function AddResponsiblePersonProfessionAssignment(ByVal responsiblePersonProfession As ResponsiblePersonAssignedProfessionData) As Boolean
		Function AddCustomerSectorAssignment(ByVal customerSector As CustomerAssignedSectorData) As Boolean
		Function AddResponsiblePersonSectorAssignment(ByVal responsiblePersonSector As ResponsiblePersonAssignedSectorData) As Boolean
		Function AddCustomerEmploymentTypeAssignment(ByVal customerEmploymentType As CustomerAssignedEmploymentTypeData) As Boolean
		Function AddCustomerKeywordAssignment(ByVal customerKeyword As CustomerAssignedKeywordData) As Boolean
		Function AddCustomerGAVGroupAssignment(ByVal customerGAVGroup As CustomerAssignedGAVGroupData) As Boolean
		Function AddCustomerEmailAssignment(ByVal customerEmailData As CustomerAssignedEmailData) As Boolean
		Function AddCustomerInvoiceAddressAssignment(ByVal invoiceAddress As CustomerAssignedInvoiceAddressData) As Boolean
		Function AddCustomerKSTAssignment(ByVal kst As CustomerAssignedKSTData) As Boolean
		Function AddResponsiblePersonCommunicationAssignment(ByVal communicationData As ResponsiblePersonAssignedCommuncationData) As Boolean
		Function AddResponsiblePersonContactTypeAssignment(ByVal contactTypeData As ResponsiblePersonAssignedContactTypeData) As Boolean
		Function AddResponsiblePersonReserveAssignment(ByVal reserveData As ResponsiblePersonAssignedReserveData, ByVal reserveType As ResponsiblePersonReserveDataType) As Boolean
		Function AddResponsiblePersonDocumentAssignment(ByVal documentData As ResponsiblePersonAssignedDocumentData) As Boolean
		Function AddResponsiblePersonContactAssignment(ByVal contactData As ResponsiblePersonAssignedContactData) As Boolean
		Function AddContactDocument(ByVal contactDocument As ContactDoc) As Boolean
		Function UpdateCustomerMasterData(ByVal customerMasterData As CustomerMasterData) As Boolean
		Function UpdateCustomerGeoData(ByVal customerMasterData As CustomerMasterData) As Boolean
		Function UpdateResponsiblePersonMasterData(ByVal responsiblePeresonMasterData As ResponsiblePersonMasterData) As Boolean
		Function UpdateCustomerAssignedInvoiceAddress(ByVal invoiceAddress As CustomerAssignedInvoiceAddressData) As Boolean
		Function UpdateCustomerAssignedEMailDeliveryProperties(ByVal customerData As CustomerMasterData) As Boolean
		Function UpdateCustomerAssignedKST(ByVal kstData As CustomerAssignedKSTData) As Boolean
		Function UpdateResponsiblePersonAssignedDocumentData(ByVal documentData As ResponsiblePersonAssignedDocumentData) As Boolean
		Function UpdateResponsiblePersonAssignedDocumentByteData(ByVal documentId As Integer, ByVal filebytes() As Byte, ByVal fileExtension As String) As Boolean
		Function UpdateCustomerAssignedCreditInfoData(ByVal creditInfoData As CustomerAssignedCreditInfo) As Boolean
		Function UpdateCustomerAssignedCreditInfoByteData(ByVal creditInfoId As Integer, ByVal filebytes() As Byte) As Boolean
		Function UpdateResponsiblePersonAssignedContactData(ByVal contactData As ResponsiblePersonAssignedContactData) As Boolean
		Function UpdateContactDocumentData(ByVal contactDocument As ContactDoc, ByVal ignoreFileBytes As Boolean) As Boolean
		Function LoadCustomerNoticesData(ByVal customerNumber As Integer) As CustomerNoticesData
		Function UpdateCustomerNoticesData(ByVal notticeData As CustomerNoticesData) As Boolean

		Function DeleteCustomerAddressAssignment(ByVal CustomerNumber As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteCustomerAddressAssignmentResult

		Function DeleteCustomerBusinessBranchDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteCustomerProfessionDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteResponsiblePersonProfessionDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteCustomerSectorDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteResponsiblePersonSectorDataAssigment(ByVal id As Integer) As Boolean
		Function DeleteCustomerEmploymentTypeDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteCustomerKeywordDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteCustomerGAVGroupDataAssignment(ByVal id As Integer) As Boolean
		Function DeleteCustomerEmailAssignment(ByVal id As Integer) As Boolean

		Function DeleteCustomerInvoiceAddressAssignment(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteCustomerInvoiceAddressAssignmentResult
		Function DeleteCustomerKSTAssignment(ByVal id As Integer) As DeleteCustomerKSTAssignmentResult
		Function DeleteCustomerOrRespPersonDocumentAssignment(ByVal id As Integer) As Boolean
		Function DeleteCustomerCreditInfoAssignment(ByVal id As Integer) As Boolean
		Function DeleteResponsiblePersonCommunicationAssignment(ByVal id As Integer) As Boolean
		Function DeleteResponsiblePersonContactTypeAssignment(ByVal id As Integer) As Boolean
		Function DeleteResponsiblePersonReserveAssignment(ByVal id As Integer, ByVal reserveType As ResponsiblePersonReserveDataType) As Boolean
		Function DeleteResponsiblePersonContactAssignment(ByVal id As Integer) As Boolean
		Function DeleteResponsiblePerson(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As Boolean
		Function DeleteContactDocument(ByVal id As Integer) As Boolean

		Function LogSolvencyUsage(ByVal connectionString As String, ByVal customerGuid As String, ByVal userGuid As String, ByVal userName As String, ByVal solvencyCheckType As String,
															ByVal jobID As String, ByVal serviceDate As DateTime) As Boolean
		Function CopyCustomerAddressToResponsiblePersons(ByVal customerNumber As Integer) As Boolean

		''' <summary>
		''' Customer properties
		''' </summary>
		Function LoadFoundedVacanciesForCustomerMng(customerNumber As Integer?) As IEnumerable(Of CustomerVacanciesProperty)
		Function LoadFoundedProposeForCustomerMng(customerNumber As Integer?) As IEnumerable(Of CustomerProposeProperty)
		Function LoadFoundedOfferForCustomerMng(customerNumber As Integer?) As IEnumerable(Of CustomerOfferProperty)
		Function LoadFoundedESForCustomerMng(customerNumber As Integer?) As IEnumerable(Of CustomerESProperty)
		Function LoadFoundedRPForCustomerMng(customerNumber As Integer?) As IEnumerable(Of CustomerReportsProperty)
		Function LoadFoundedInvoiceForCustomerMng(customerNumber As Integer?, ByVal justOpenInvoices As Boolean?) As IEnumerable(Of CustomerInvoiceProperty)
		Function LoadFoundedPaymentForCustomerMng(customerNumber As Integer?) As IEnumerable(Of CustomerPaymentProperty)


		' Propose
		Function LoadCustomerContactTotalDataForPropose(ByVal customerNumber As Integer, ByVal proposeNumber As Integer, ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean, ByVal bHideSMS As Boolean, ByVal years As Integer()) As IEnumerable(Of CustomerContactOverviewData)


		' WOSExport
		Function LoadCustomerDataForWOSExport(ByVal userNumber As Integer?, ByVal customerNumber As Integer?, ByVal cresponsibleNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal invoiceNumber As Integer?) As CustomerWOSData
		Function LoadCustomerDataForCustomerWOSExport(ByVal userNumber As Integer?, ByVal customerNumber As Integer?, ByVal cresponsibleNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal invoiceNumber As Integer?) As DataTable
		Function CacheCustomerWOSDataTemporary(ByVal customerData As CustomerWOSData) As Boolean


		' Report Working hours
		Function GetCustomerMonthHoursAndAbsenceData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As ZVHourAbsenceData
		Function GetCustomerMonthHoursGroupedByKSTData(ByVal mdNr As Integer, ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal year As Integer, ByVal month As Integer) As IEnumerable(Of WorkingHourGroupedWithKSTNrData)


#Region "phone number"

		Function LoadCommonPhoneNumberData(ByVal phoneNumber As String) As IEnumerable(Of CommonTelephonyData)
		Function AddCallHistory(ByVal mandantNr As Integer, ByVal callData As CallHistoryData) As Boolean

#End Region


	End Interface

	''' <summary>
	''' The customer reserve data type.
	''' </summary>
	Public Enum CustomerReserveDataType
		Reserve1 = 1
		Reserve2 = 2
		Reserve3 = 3
		Reserve4 = 4
	End Enum

	''' <summary>
	''' The responsible person reserve data type.
	''' </summary>
	Public Enum ResponsiblePersonReserveDataType
		Reserve1 = 1
		Reserve2 = 2
		Reserve3 = 3
		Reserve4 = 4
	End Enum

	''' <summary>
	''' Result of customer address assignment (Kunde) deletion.
	''' </summary>
	''' <remarks></remarks>
	Public Enum DeleteCustomerAddressAssignmentResult
		Deleted = 2
		CouldNotDeleteBecauseOfExistingVac = 3
		CouldNotDeleteBecauseOfExistingPropose = 4

		CouldNotDeleteBecauseOfExistingES = 10
		CouldNotDeleteBecauseOfExistingRP = 11
		CouldNotDeleteBecauseOfExistingRE = 12
		CouldNotDeleteBecauseOfExistingZE = 13

		ErrorWhileDelete = 20
	End Enum

	''' <summary>
	''' Result of customer invoice address assignment (KD_RE_Address) deletion.
	''' </summary>
	''' <remarks></remarks>
	Public Enum DeleteCustomerInvoiceAddressAssignmentResult
		CouldNotDeleteOnlyOneRecordLeft = 1
		Deleted = 2
		CouldNotDeleteBecauseOfExistingKST = 3
		ErrorWhileDelete = 4
	End Enum

	''' <summary>
	''' Result of customer KST assignment (KD_KST) deletion.
	''' </summary>
	Public Enum DeleteCustomerKSTAssignmentResult
		CouldNotDeleteOnlyOneRecordLeft = 1
		Deleted = 2
		CouldNotDeleteBecauseOfExistingEsLohn = 3
		CouldNotDeleteBecauseOfExistingRapport = 4
		ErrorWhileDelete = 5
	End Enum

	''' <summary>
	''' Decision result.
	''' </summary>
	Public Enum DecisionResult
		LightGreen = 1
		Green = 2
		YellowGreen = 3
		Yellow = 4
		Orange = 5
		Red = 6
		DarkRed = 7
	End Enum

	''' <summary>
	''' Business solvency check type.
	''' </summary>
	Public Enum BusinessSolvencyCheckType
		QuickBusinessCheck = 1
		BusinessCheck = 2
	End Enum


End Namespace