
Imports SP.DatabaseAccess.TableSetting.DataObjects

Namespace TableSetting

	''' <summary>
	''' Interface for common database access.
	''' </summary>
	Public Interface ITablesDatabaseAccess


		''' <summary>
		''' employee tables
		''' </summary>
		Function LoadEmployeeContactData() As IEnumerable(Of EmployeeContactData)
		Function AddEmployeeContactData(ByVal Data As EmployeeContactData) As Boolean
		Function UpdateEmployeeContactData(ByVal Data As EmployeeContactData) As Boolean
		Function DeleteEmployeeContactData(ByVal recid As Integer) As Boolean

		Function LoadEmployeeStateData1() As IEnumerable(Of EmployeeStateData)
		Function AddEmployeeStateData1(ByVal Data As EmployeeStateData) As Boolean
		Function UpdateEmployeeStateData1(ByVal Data As EmployeeStateData) As Boolean
		Function DeleteEmployeeStateData1(ByVal recid As Integer) As Boolean

		Function LoadEmployeeStateData2() As IEnumerable(Of EmployeeStateData)
		Function AddEmployeeStateData2(ByVal Data As EmployeeStateData) As Boolean
		Function UpdateEmployeeStateData2(ByVal Data As EmployeeStateData) As Boolean
		Function DeleteEmployeeCivilstateData(ByVal recid As Integer) As Boolean

		Function LoadEmployeeCivilstateData() As IEnumerable(Of Common.DataObjects.CivilStateData)
		Function AddEmployeeCivilstateData(ByVal Data As Common.DataObjects.CivilStateData) As Boolean
		Function UpdateEmployeeCivilstateData(ByVal Data As Common.DataObjects.CivilStateData) As Boolean
		Function DeleteEmployeeStateData2(ByVal recid As Integer) As Boolean

		Function LoadJobLanguageData() As IEnumerable(Of JobLanguageData)
		Function AddJobLanguageData(ByVal data As JobLanguageData) As Boolean
		Function UpdateJobLanguageData(ByVal data As JobLanguageData) As Boolean
		Function DeleteJobLanguageData(ByVal recid As Integer) As Boolean

		Function LoadAssessmentData() As IEnumerable(Of AssessmentData)
		Function AddAssessmentData(ByVal data As AssessmentData) As Boolean
		Function UpdateAssessmentData(ByVal data As AssessmentData) As Boolean
		Function DeleteAssessmentData(ByVal recid As Integer) As Boolean

		Function LoadCommunicationTypeData() As IEnumerable(Of CommunicationTypeData)
		Function AddCommunicationTypeData(ByVal data As CommunicationTypeData) As Boolean
		Function UpdateCommunicationTypeData(ByVal data As CommunicationTypeData) As Boolean
		Function DeleteCommunicationTypeData(ByVal recid As Integer) As Boolean

		Function LoadCarReserveData() As IEnumerable(Of CarReserveData)
		Function AddCarReserveData(ByVal data As CarReserveData) As Boolean
		Function UpdateCarReserveData(ByVal data As CarReserveData) As Boolean
		Function DeleteCarReserveData(ByVal recid As Integer) As Boolean

		Function LoadDrivingLicenceData() As IEnumerable(Of DrivingLicenceData)
		Function AddDrivingLicenceData(ByVal data As DrivingLicenceData) As Boolean
		Function UpdateDrivingLicenceData(ByVal data As DrivingLicenceData) As Boolean
		Function DeleteDrivingLicenceData(ByVal recid As Integer) As Boolean

		Function LoadVehicleData() As IEnumerable(Of VehicleData)
		Function AddVehicleData(ByVal data As VehicleData) As Boolean
		Function UpdateVehicleData(ByVal data As VehicleData) As Boolean
		Function DeleteVehicleData(ByVal recid As Integer) As Boolean

		Function LoadContactReserveData(ByVal contactReserveType As ContactReserveType) As IEnumerable(Of ContactReserveData)
		Function AddContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As ContactReserveData) As Boolean
		Function UpdateContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As ContactReserveData) As Boolean
		Function DeleteContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal recid As Integer) As Boolean

		Function LoadDeadLineData() As IEnumerable(Of DeadlineData)
		Function AddDeadLineData(ByVal data As DeadlineData) As Boolean
		Function UpdateDeadLineData(ByVal data As DeadlineData) As Boolean
		Function DeleteDeadLineData(ByVal recid As Integer) As Boolean

		Function LoadWorkPensumData() As IEnumerable(Of WorkPensumData)
		Function AddWorkPensumData(ByVal data As WorkPensumData) As Boolean
		Function UpdateWorkPensumData(ByVal data As WorkPensumData) As Boolean
		Function DeleteWorkPensumData(ByVal recid As Integer) As Boolean

		Function LoadEmployementTypeData() As IEnumerable(Of EmployeeEmployementTypeData)
		Function AddEmployementTypeData(ByVal data As EmployeeEmployementTypeData) As Boolean
		Function UpdateEmployementTypeData(ByVal data As EmployeeEmployementTypeData) As Boolean
		Function DeleteEmployementTypeData(ByVal recid As Integer) As Boolean

		Function LoadEmployeeDocumentCategoryData() As IEnumerable(Of Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData)
		Function AddEmployeeDocumentCategoryData(ByVal data As Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) As Boolean
		Function UpdateEmployeeDocumentCategoryData(ByVal data As Employee.DataObjects.DocumentMng.EmployeeDocumentCategoryData) As Boolean
		Function DeleteEmployeeDocumentCategoryData(ByVal recid As Integer) As Boolean


		Function LoadInterviewStateData() As IEnumerable(Of EmployeeInteriviewStateData)
		Function AddInterviewStateData(ByVal data As EmployeeInteriviewStateData) As Boolean
		Function UpdateInterviewStateData(ByVal data As EmployeeInteriviewStateData) As Boolean
		Function DeleteInterviewStateData(ByVal recid As Integer) As Boolean




		''' <summary>
		''' Customer setting
		''' </summary>
		Function LoadCustomerPropertyData() As IEnumerable(Of CustomerPropertyData)
		Function AddCustomerPropertyData(ByVal Data As CustomerPropertyData) As Boolean
		Function UpdateCustomerPropertyData(ByVal Data As CustomerPropertyData) As Boolean
		Function DeleteCustomerPropertyData(ByVal recid As Integer) As Boolean

		Function LoadCustomerContactData() As IEnumerable(Of CustomerContactData)
		Function AddCustomerContactData(ByVal Data As CustomerContactData) As Boolean
		Function UpdateCustomerContactData(ByVal Data As CustomerContactData) As Boolean
		Function DeleteCustomerContactData(ByVal recid As Integer) As Boolean

		Function LoadCustomerStateData1() As IEnumerable(Of CustomerStateData)
		Function AddCustomerStateData1(ByVal Data As CustomerStateData) As Boolean
		Function UpdateCustomerStateData1(ByVal Data As CustomerStateData) As Boolean
		Function DeleteCustomerStateData1(ByVal recid As Integer) As Boolean

		Function LoadCustomerStateData2() As IEnumerable(Of CustomerStateData)
		Function AddCustomerStateData2(ByVal Data As CustomerStateData) As Boolean
		Function UpdateCustomerStateData2(ByVal Data As CustomerStateData) As Boolean
		Function DeleteCustomerStateData2(ByVal recid As Integer) As Boolean

		Function LoadCustomerEmployementTypeData() As IEnumerable(Of CustomerEmployementTypeData)
		Function AddCustomerEmployementTypeData(ByVal Data As CustomerEmployementTypeData) As Boolean
		Function UpdateCustomerEmployementTypeData(ByVal Data As CustomerEmployementTypeData) As Boolean
		Function DeleteCustomerEmployementTypeData(ByVal recid As Integer) As Boolean

		Function LoadCustomerStichwortData() As IEnumerable(Of CustomerStichwortData)
		Function AddCustomerStichwortData(ByVal Data As CustomerStichwortData) As Boolean
		Function UpdateCustomerStichwortData(ByVal Data As CustomerStichwortData) As Boolean
		Function DeleteCustomerStichwortData(recid As Integer) As Boolean


		Function LoadCustomerContactReserveData(ByVal contactReserveType As ContactReserveType) As IEnumerable(Of Customer.DataObjects.CustomerReserveData)
		Function AddCustomerContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.CustomerReserveData) As Boolean
		Function UpdateCustomerContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.CustomerReserveData) As Boolean
		Function DeleteCustomerContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal recid As Integer) As Boolean


		Function LoadCustomerPaymentReminderCodeData() As IEnumerable(Of Customer.DataObjects.PaymentReminderCodeData)
		Function AddCustomerPaymentReminderCodeData(ByVal data As Customer.DataObjects.PaymentReminderCodeData) As Boolean
		Function UpdateCustomerPaymentReminderCodeData(ByVal data As Customer.DataObjects.PaymentReminderCodeData) As Boolean
		Function DeleteCustomerPaymentReminderCodeData(ByVal recid As Integer) As Boolean


		Function LoadCustomerPaymentConditionData() As IEnumerable(Of Customer.DataObjects.PaymentConditionData)
		Function AddCustomerPaymentConditionData(ByVal data As Customer.DataObjects.PaymentConditionData) As Boolean
		Function UpdateCustomerPaymentConditionData(ByVal data As Customer.DataObjects.PaymentConditionData) As Boolean
		Function DeleteCustomerPaymentConditionData(ByVal recid As Integer) As Boolean


		Function LoadCustomerInvoiceOptionData() As IEnumerable(Of Customer.DataObjects.InvoiceOptionData)
		Function AddCustomerInvoiceOptionData(ByVal data As Customer.DataObjects.InvoiceOptionData) As Boolean
		Function UpdateCustomerInvoiceOptionData(ByVal data As Customer.DataObjects.InvoiceOptionData) As Boolean
		Function DeleteCustomerInvoiceOptionData(ByVal recid As Integer) As Boolean


		Function LoadCustomerInvoiceTypeData() As IEnumerable(Of Customer.DataObjects.InvoiceTypeData)
		Function AddCustomerInvoiceTypeData(ByVal data As Customer.DataObjects.InvoiceTypeData) As Boolean
		Function UpdateCustomerInvoiceTypeData(ByVal data As Customer.DataObjects.InvoiceTypeData) As Boolean
		Function DeleteCustomerInvoiceTypeData(ByVal recid As Integer) As Boolean

		Function LoadCustomerInvoiceShipment() As IEnumerable(Of Customer.DataObjects.OPShipmentData)
		Function AddCustomerInvoiceShipment(ByVal data As Customer.DataObjects.OPShipmentData) As Boolean
		Function UpdateCustomerInvoiceShipment(ByVal data As Customer.DataObjects.OPShipmentData) As Boolean
		Function DeleteCustomerInvoiceShipment(ByVal recid As Integer) As Boolean

		Function LoadCustomerNumberOfEmployeesData() As IEnumerable(Of Customer.DataObjects.NumberOfEmployeesData)
		Function AddCustomerNumberOfEmployeesData(ByVal data As Customer.DataObjects.NumberOfEmployeesData) As Boolean
		Function UpdateCustomerNumberOfEmployeesData(ByVal data As Customer.DataObjects.NumberOfEmployeesData) As Boolean
		Function DeleteCustomerNumberOfEmployeesData(ByVal recid As Integer) As Boolean

		Function LoadCustomerDocumentCategoryData() As IEnumerable(Of Customer.DataObjects.CustomerDocumentCategoryData)
		Function AddCustomerDocumentCategoryData(ByVal data As Customer.DataObjects.CustomerDocumentCategoryData) As Boolean
		Function UpdateCustomerDocumentCategoryData(ByVal data As Customer.DataObjects.CustomerDocumentCategoryData) As Boolean
		Function DeleteCustomerDocumentCategoryData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' responsible person
		''' </summary>
		Function LoadResponsiblepersonContactData() As IEnumerable(Of Customer.DataObjects.ResponsiblePersonContactInfo)
		Function AddResponsiblepersonContactData(ByVal data As Customer.DataObjects.ResponsiblePersonContactInfo) As Boolean
		Function UpdateResponsiblepersonContactData(ByVal data As Customer.DataObjects.ResponsiblePersonContactInfo) As Boolean
		Function DeleteResponsiblepersonContactData(ByVal recid As Integer) As Boolean


		Function LoadResponsiblepersonStateData1() As IEnumerable(Of Customer.DataObjects.ResponsiblePersonStateData)
		Function AddResponsiblepersonStateData1(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean
		Function UpdateResponsiblepersonStateData1(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean
		Function DeleteResponsiblepersonStateData1(ByVal recid As Integer) As Boolean


		Function LoadResponsiblepersonStateData2() As IEnumerable(Of Customer.DataObjects.ResponsiblePersonStateData)
		Function AddResponsiblepersonStateData2(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean
		Function UpdateResponsiblepersonStateData2(ByVal data As Customer.DataObjects.ResponsiblePersonStateData) As Boolean
		Function DeleteResponsiblepersonStateData2(ByVal recid As Integer) As Boolean


		Function LoadResponsiblepersonDepartment() As IEnumerable(Of Customer.DataObjects.DepartmentData)
		Function AddResponsiblepersonDepartment(ByVal data As Customer.DataObjects.DepartmentData) As Boolean
		Function UpdateResponsiblepersonDepartment(ByVal data As Customer.DataObjects.DepartmentData) As Boolean
		Function DeleteResponsiblepersonDepartment(ByVal recid As Integer) As Boolean


		Function LoadResponsiblepersonPosition() As IEnumerable(Of Customer.DataObjects.PositionData)
		Function AddResponsiblepersonPosition(ByVal data As Customer.DataObjects.PositionData) As Boolean
		Function UpdateResponsiblepersonPosition(ByVal data As Customer.DataObjects.PositionData) As Boolean
		Function DeleteResponsiblepersonPosition(ByVal recid As Integer) As Boolean

		Function LoadResponsiblepersonCommunication() As IEnumerable(Of Customer.DataObjects.CustomerCommunicationData)
		Function AddResponsiblepersonCommunication(ByVal data As Customer.DataObjects.CustomerCommunicationData) As Boolean
		Function UpdateResponsiblepersonCommunication(ByVal data As Customer.DataObjects.CustomerCommunicationData) As Boolean
		Function DeleteResponsiblepersonCommunication(ByVal recid As Integer) As Boolean


		Function LoadResponsiblepersonCommunicationType() As IEnumerable(Of Customer.DataObjects.CustomerCommunicationTypeData)
		Function AddResponsiblepersonCommunicationType(ByVal data As Customer.DataObjects.CustomerCommunicationTypeData) As Boolean
		Function UpdateResponsiblepersonCommunicationType(ByVal data As Customer.DataObjects.CustomerCommunicationTypeData) As Boolean
		Function DeleteResponsiblepersonCommunicationType(ByVal recid As Integer) As Boolean


		Function LoadCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType) As IEnumerable(Of Customer.DataObjects.ResponsiblePersonReserveData)
		Function AddCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.ResponsiblePersonReserveData) As Boolean
		Function UpdateCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal data As Customer.DataObjects.ResponsiblePersonReserveData) As Boolean
		Function DeleteCResponsibleContactReserveData(ByVal contactReserveType As ContactReserveType, ByVal recid As Integer) As Boolean





		''' <summary>
		''' Vacancy
		''' </summary>
		Function LoadVacancyContactData() As IEnumerable(Of VacancyContactData)
		Function AddVacancyContactData(ByVal data As VacancyContactData) As Boolean
		Function UpdateVacancyContactData(ByVal data As VacancyContactData) As Boolean
		Function DeleteVacancyContactData(ByVal recid As Integer) As Boolean

		Function LoadVacancyStateData() As IEnumerable(Of VacancyStateData)
		Function AddVacancyStateData(ByVal data As VacancyStateData) As Boolean
		Function UpdateVacancyStateData(ByVal data As VacancyStateData) As Boolean
		Function DeleteVacancyStateData(ByVal recid As Integer) As Boolean

		Function LoadVacancyGroupData() As IEnumerable(Of VacancyGroupData)
		Function LoadVacancySubGroupData() As IEnumerable(Of VacancySubGroupData)
		Function LoadAssingedVacancySubGroupData(ByVal mainGroup As String, ByVal language As String) As IEnumerable(Of VacancySubGroupData)
		Function AddVacancyGroupData(ByVal data As VacancyGroupData) As Boolean
		Function UpdateVacancyGroupData(ByVal data As VacancyGroupData) As Boolean
		Function DeleteVacancyGroupData(ByVal recid As Integer) As Boolean



		''' <summary>
		''' Offer
		''' </summary>
		Function LoadOfferContactData() As IEnumerable(Of OfferContactData)
		Function AddOfferContactData(ByVal data As OfferContactData) As Boolean
		Function UpdateOfferContactData(ByVal data As OfferContactData) As Boolean
		Function DeleteOfferContactData(ByVal recid As Integer) As Boolean

		Function LoadOfferStateData() As IEnumerable(Of OfferStateData)
		Function AddOfferStateData(ByVal data As OfferStateData) As Boolean
		Function UpdateOfferStateData(ByVal data As OfferStateData) As Boolean
		Function DeleteOfferStateData(ByVal recid As Integer) As Boolean

		Function LoadOfferGroupData() As IEnumerable(Of OfferGroupData)
		Function AddOfferGroupData(ByVal data As OfferGroupData) As Boolean
		Function UpdateOfferGroupData(ByVal data As OfferGroupData) As Boolean
		Function DeleteOfferGroupData(ByVal recid As Integer) As Boolean



		''' <summary>
		''' Propose
		''' </summary>
		Function LoadProposeStateData() As IEnumerable(Of ProposeStateData)
		Function AddProposeStateData(ByVal data As ProposeStateData) As Boolean
		Function UpdateProposeStateData(ByVal data As ProposeStateData) As Boolean
		Function DeleteProposeStateData(ByVal recid As Integer) As Boolean

		Function LoadProposeEmployementTypeData() As IEnumerable(Of ProposeEmployementTypeData)
		Function AddProposeEmployementTypeData(ByVal data As ProposeEmployementTypeData) As Boolean
		Function UpdateProposeEmployementTypeData(ByVal data As ProposeEmployementTypeData) As Boolean
		Function DeleteProposeEmployementTypeData(ByVal recid As Integer) As Boolean

		Function LoadProposeArtData() As IEnumerable(Of ProposeArtData)
		Function AddProposeArtData(ByVal data As ProposeArtData) As Boolean
		Function UpdateProposeArtData(ByVal data As ProposeArtData) As Boolean
		Function DeleteProposeArtData(ByVal recid As Integer) As Boolean






		''' <summary>
		''' common tables
		''' Job: Berufe
		''' </summary>
		Function LoadContactCategoryData() As IEnumerable(Of SP.DatabaseAccess.Common.DataObjects.ContactType1Data)
		Function AddContactCategoryData(ByVal data As SP.DatabaseAccess.Common.DataObjects.ContactType1Data) As Boolean
		Function UpdateContactCategoryData(ByVal data As SP.DatabaseAccess.Common.DataObjects.ContactType1Data) As Boolean
		Function DeleteContactCategoryData(ByVal recid As Integer) As Boolean


		Function LoadEmployementCategorizedData() As IEnumerable(Of ES.DataObjects.ESMng.ESCategorizationData)
		Function AddEmployementCategorizedData(ByVal data As ES.DataObjects.ESMng.ESCategorizationData) As Boolean
		Function UpdateEmployementCategorizedData(ByVal data As ES.DataObjects.ESMng.ESCategorizationData) As Boolean
		Function DeleteEmployementCategorizedData(ByVal recid As Integer) As Boolean


		Function LoadJobData() As IEnumerable(Of JobData)
		Function AddJobData(ByVal data As JobData) As Boolean
		Function UpdateJobData(ByVal data As JobData) As Boolean
		Function DeleteJobData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' sectors: branchen
		''' </summary>
		Function LoadSectorData() As IEnumerable(Of SectorData)
		Function AddSectorData(ByVal data As SectorData) As Boolean
		Function UpdateSectorData(ByVal data As SectorData) As Boolean
		Function DeleteSectorData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' main language: sprachen
		''' </summary>
		Function LoadMainLanguageData() As IEnumerable(Of LanguageData)
		Function AddMainLanguageData(ByVal data As LanguageData) As Boolean
		Function UpdateMainLanguageData(ByVal data As LanguageData) As Boolean
		Function DeleteMainLanguageData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' salutation: Anrede und Anredeform
		''' </summary>
		Function LoadSalutationData() As IEnumerable(Of SalutationData)
		Function AddSalutationData(ByVal data As SalutationData) As Boolean
		Function UpdateSalutationData(ByVal data As SalutationData) As Boolean
		Function DeleteSalutationData(ByVal recid As Integer) As Boolean

		''' <summary>
		''' businessbranchs: Filiale
		''' </summary>
		Function LoadBusinessBranchsData() As IEnumerable(Of AvilableBusinessBranchData)
		Function AddBusinessBranchsData(ByVal data As AvilableBusinessBranchData) As Boolean
		Function UpdateBusinessBranchsData(ByVal data As AvilableBusinessBranchData) As Boolean
		Function DeleteBusinessBranchsData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' terms and conditions: AGB
		''' </summary>
		Function LoadTermsAndConditionsData() As IEnumerable(Of TermsAndConditionsData)
		Function AddTermsAndConditionsData(ByVal data As TermsAndConditionsData) As Boolean
		Function UpdateTermsAndConditionsData(ByVal data As TermsAndConditionsData) As Boolean
		Function DeleteTermsAndConditionsData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' currency: Währung
		''' </summary>
		Function LoadCurrencyData() As IEnumerable(Of CurrencyData)
		Function AddCurrencyData(ByVal data As CurrencyData) As Boolean
		Function UpdateCurrencyData(ByVal data As CurrencyData) As Boolean
		Function DeleteCurrencyData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' costcenter1: KST1
		''' </summary>
		Function LoadCostCenter1() As IEnumerable(Of CostCenter1Data)
		Function AddCostCenter1Data(ByVal data As CostCenter1Data) As Boolean
		Function UpdateCostCenter1Data(ByVal data As CostCenter1Data) As Boolean
		Function DeleteCostCenter1Data(ByVal recid As Integer) As Boolean



		''' <summary>
		''' costcenter2: KST2
		''' </summary>
		Function LoadCostCenter2() As IEnumerable(Of CostCenter2Data)
		Function AddCostCenter2Data(ByVal data As CostCenter2Data) As Boolean
		Function UpdateCostCenter2Data(ByVal data As CostCenter2Data) As Boolean
		Function DeleteCostCenter2Data(ByVal recid As Integer) As Boolean



		''' <summary>
		''' sms templates
		''' </summary>
		Function LoadSMSTemplateData() As IEnumerable(Of SMSTemplateData)
		Function AddSMSTemplateData(ByVal data As SMSTemplateData) As Boolean
		Function UpdateSMSTemplateData(ByVal data As SMSTemplateData) As Boolean
		Function DeleteSMSTemplateData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' bvg tables
		''' </summary>
		Function LoadBVGData(ByVal mdNr As Integer, ByVal gender As String, ByVal year As Integer?) As IEnumerable(Of BVGData)
		Function AddBVGData(ByVal gender As String, ByVal data As BVGData) As Boolean
		Function UpdateBVGData(ByVal gender As String, ByVal data As BVGData) As Boolean
		Function DeleteBVGData(ByVal gender As String, ByVal recid As Integer) As Boolean



		''' <summary>
		''' Ferien, Feiertag und 13. Lohn
		''' </summary>
		Function LoadFF13Lohn() As IEnumerable(Of FF13LohnData)
		Function AddFF13LohnData(ByVal data As FF13LohnData) As Boolean
		Function UpdateFF13LohnData(ByVal data As FF13LohnData) As Boolean
		Function DeleteFF13LohnData(ByVal recid As Integer) As Boolean



		''' <summary>
		''' qstinfo
		''' </summary>
		Function LoadQSTInfo() As IEnumerable(Of QstInfoData)
		Function AddQSTInfoData(ByVal data As QstInfoData) As Boolean
		Function UpdateQSTInfoData(ByVal data As QstInfoData) As Boolean
		Function DeleteQSTInfoData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' country
		''' </summary>
		Function LoadCountryData() As IEnumerable(Of CountryData)
		Function AddCountryData(ByVal data As CountryData) As Boolean
		Function UpdateCountryData(ByVal data As CountryData) As Boolean
		Function DeleteCountryData(ByVal recid As Integer) As Boolean



		''' <summary>
		''' absence code: Fehlcode
		''' </summary>
		Function LoadAbsenceData() As IEnumerable(Of AbsenceData)
		Function AddAbsenceData(ByVal data As AbsenceData) As Boolean
		Function UpdateAbsenceData(ByVal data As AbsenceData) As Boolean
		Function DeleteAbsenceData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' Fibu konten
		''' </summary>
		Function LoadFIBUKontenData(ByVal language As String) As IEnumerable(Of FIBUData)
		Function AddFIBUKontenData(ByVal data As FIBUData) As Boolean
		Function UpdateFIBUKontenData(ByVal data As FIBUData) As Boolean
		Function DeleteFIBUKontenData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' Print Template
		''' </summary>
		Function LoadPrintTemplatesData() As IEnumerable(Of PrintTemplatesData)
		Function AddPrintTemplatesData(ByVal data As PrintTemplatesData) As Boolean
		Function UpdatePrintTemplatesData(ByVal data As PrintTemplatesData) As Boolean
		Function DeletePrintTemplatesData(ByVal recid As Integer) As Boolean


		''' <summary>
		''' Export Template
		''' </summary>
		Function LoadExportTemplatesData() As IEnumerable(Of ExportTemplatesData)
		Function AddExportTemplatesData(ByVal data As ExportTemplatesData) As Boolean
		Function UpdateExportTemplatesData(ByVal data As ExportTemplatesData) As Boolean
		Function DeleteExportTemplatesData(ByVal recid As Integer) As Boolean



		''' <summary>
		''' mandant setting
		''' </summary>
		Function LoadMandantData(ByVal mdNr As Integer, ByVal year As Integer) As MandantData

		Function SaveMandantData(ByVal mdNr As Integer, ByVal year As Integer, MDData As MandantData) As Boolean

		Function LoadAssignedChildEducationData(ByVal recid As Integer) As ChildEducationData

		Function LoadChildEducationData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of ChildEducationData)

		Function UpdateAssignedChildEducationData(ByVal mdNr As Integer, year As Integer, MDData As ChildEducationData) As Boolean

		Function AddChildEducationData(ByVal mdNr As Integer, year As Integer, MDData As ChildEducationData) As Boolean

		Function DeleteChildEducationData(ByVal recid As Integer?) As Boolean


		''' <summary>
		''' lmv KTG
		''' </summary>
		Function LoadAssignedKTGForLmvData(ByVal recid As Integer?, ByVal mdNr As Integer, ByVal mdYear As Integer, ByVal gavNumber As Integer) As lmvKTGData
		Function LoadKTGForLmvData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of lmvKTGData)

		Function AddKTGForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvKTGData) As Boolean

		Function UpdateAssignedKTGForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvKTGData) As Boolean

		Function DeleteKTGForLmvData(ByVal recid As Integer?) As Boolean


		''' <summary>
		''' LMV Tagesspesen
		''' </summary>
		Function LoadAssignedTSpesenForLmvData(ByVal recid As Integer?, ByVal mdNr As Integer, ByVal mdYear As Integer, ByVal gavNumber As Integer) As lmvTSpesenData
		Function LoadTSpesenForLmvData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of lmvTSpesenData)

		Function AddTSpesenForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvTSpesenData) As Boolean

		Function UpdateAssignedTSpesenForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvTSpesenData) As Boolean

		Function DeleteTSpesenForLmvData(ByVal recid As Integer?) As Boolean


		''' <summary>
		''' Lohnartenstamm
		''' </summary>
		Function LoadAssignedLAStammData(ByVal recid As Integer, ByVal year As Integer) As LAStammData

		Function LoadLAStammData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of LAStammData)

		Function UpdateAssignedLAStammData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As LAStammData) As Boolean

		Function AddLAStammData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As LAStammData) As Boolean

		Function DeleteLAStammData(recid As Integer?, ByVal userName As String, ByVal userNr As Integer) As Boolean


		''' <summary>
		''' Lohnartenstamm translation
		''' </summary>
		Function LoadAssignedLATranslationData(ByVal recid As Integer) As LATranslationData

		Function LoadLATranslationData() As IEnumerable(Of LATranslationData)

		Function UpdateAssignedLATranslationData(ByVal data As LATranslationData) As Boolean

		Function AddLATranslationData(ByVal data As LATranslationData) As Boolean

		Function DeleteLATranslationData(recid As Integer?) As Boolean


		''' <summary>
		''' userdata
		''' </summary>
		Function LoadAssignedUserAddressData(ByVal userNumber As Integer) As UserAddressData

		Function AddUserAddressData(ByVal userRecID As Integer, ByVal data As UserAddressData) As Boolean

		Function UpdateAssignedUserAddressData(ByVal data As UserAddressData) As Boolean


		Function LoadAssignedUserData(ByVal recid As Integer) As UserData

		Function LoadUserData(ByVal userNumber As Integer?) As IEnumerable(Of UserData)

		Function UpdateAssignedUserData(ByVal data As UserData) As Boolean

		Function UpdateAssignedUserPictureAndSignData(ByVal data As UserData) As Boolean

		Function AddUserData(ByVal data As UserData) As Boolean

		Function DeleteUserData(ByVal recid As Integer?, ByVal operatorNumber As Integer) As DeleteUserResult


		''' <summary>
		''' rights group
		''' </summary>
		Function LoadAssignedRightsData(ByVal recid As Integer) As RightsData

		Function LoadRightsData() As IEnumerable(Of RightsData)

		Function SaveUSRightsWithSelectedTemplates(ByVal mandantenNumber As Integer, ByVal userNumber As Integer, ByVal rightProc As String) As Boolean


		''' <summary>
		''' document data
		''' </summary>
		Function LoadDocumentData(ByVal mandantenNumber As Integer, ByVal userNumber As Integer) As IEnumerable(Of DocumentData)
		Function LoadTemplateDataForSendBulkCustomer(ByVal tplArt As String) As IEnumerable(Of DocumentData)


		''' <summary>
		''' user document rights
		''' </summary>
		Function UpdateAssignedUserDocumentRightsForAllUsersData(ByVal data As DocumentData) As Boolean
		Function UpdateAssignedUserDocumentRightsData(ByVal data As DocumentData) As Boolean

		Function AddAssignedUserDocumentRightsData(ByVal data As DocumentData) As Boolean



		''' <summary>
		''' rights group
		''' </summary>
		Function LoadAssignedUserRightsData(ByVal userNumber As Integer, ByVal mandantNumber As Integer, ByVal ModulName As String) As IEnumerable(Of UserRightData)
		Function UpdateAssignedUserRightsData(ByVal data As UserRightData) As Boolean
		Function DeleteAssignedUserRightsData(ByVal data As UserRightData, ByVal userName As String, ByVal usnr As Integer, ByVal deleteAllUsers As Boolean) As Boolean
		Function AddAssignedRightsForAllUsersData(ByVal data As UserRightData) As Boolean
		Function UpdateUserRightsForAllUserData(ByVal data As UserRightData) As Boolean
		Function CopyUserRightsFromAnotherUser(ByVal newUserNumber As Integer, ByVal data As UserRightData) As Boolean
		Function CopyUserRightsFromMainMandantToAnotherSubMandant(ByVal mandantFrom As Integer, ByVal mandantTo As Integer, ByVal userNumber As Integer) As Boolean


		''' <summary>
		''' Mandant document data
		''' </summary>
		Function LoadMandantDocumentData() As IEnumerable(Of MandantDocumentData)
		Function LoadMandantAssignedDocumentData(ByVal jobNr As String) As MandantDocumentData
		Function UpdateAssignedMandantDocumentData(ByVal data As MandantDocumentData) As Boolean
		Function AddAssignedMandantDocumentData(ByVal data As MandantDocumentData) As Boolean
		Function DeleteAssignedMandantDocumentData(ByVal id As Integer) As Boolean



#Region "MD_ESRDTA"

		Function LoadMandantBankData(ByVal mandantenNumber As Integer, ByVal modulNumber As BankModulEnum) As IEnumerable(Of MDBankData)
		Function UpdateAssignedMandantBankData(ByVal data As MDBankData) As Boolean
		Function AddNewMandantBankData(ByVal data As MDBankData) As Boolean
		Function DeleteAssignedMandantBankData(ByVal id As Integer, ByVal advisorNumber As Integer) As SP.DatabaseAccess.Common.DataObjects.DeleteResult

#End Region

	End Interface


End Namespace
