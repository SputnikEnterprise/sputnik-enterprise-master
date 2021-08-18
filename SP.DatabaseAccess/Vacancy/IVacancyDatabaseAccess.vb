

Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Vacancy.DataObjects

Namespace Vacancy

	''' <summary>
	''' Interface for vacancy database access.
	''' </summary>
	Public Interface IVacancyDatabaseAccess

		Function LoadVacancyMasterData(ByVal mdNr As Integer, ByVal VacancyNumber As Integer) As VacancyMasterData
		Function LoadJobCHMasterData(ByVal customerID As String, ByVal userNumber As Integer, ByVal VacancyNumber As Integer) As VacancyJobCHMasterData
		Function LoadOstJobMasterData(ByVal customerID As String, ByVal userName As String, ByVal userNumber As Integer, ByVal VacancyNumber As Integer) As VacancyOstJobMasterData
		Function LoadStmpSettingData(ByVal customerID As String, ByVal userName As String, ByVal userNumber As Integer, ByVal VacancyNumber As Integer) As VacancyStmpSettingData
		Function UpdateVacancyStmpSettingData(ByVal customerID As String, ByVal userNumber As Integer, ByVal stmpData As VacancyStmpSettingData) As Boolean
		Function UpdateVacancyOstJobMasterData(ByVal customerID As String, ByVal userNumber As Integer, ByVal ojdata As VacancyOstJobMasterData) As Boolean

		Function LoadJobCHBerufData(ByVal vacancyNumber As Integer) As IEnumerable(Of VacancyJobCHBerufData)
		Function LoadJobCHBildungsNiveauData(ByVal vacancyNumber As Integer) As IEnumerable(Of VacancyJobCHPeripheryData)
		Function LoadJobCHRegionData(ByVal vacancyNumber As Integer) As IEnumerable(Of VacancyJobCHPeripheryData)
		Function LoadVacancyLanguageData(ByVal vacancyNumber As Integer, ByVal jobPlattform As ExternalPlattforms) As IEnumerable(Of VacancyJobCHLanguageData)

		Function AddNewVacancy(ByVal vacancyMasterData As VacancyMasterData) As Boolean
		Function UpdateVacancyMasterData(ByVal vacancyMasterData As VacancyMasterData) As Boolean
		Function UpdateVacancyOnlineData(ByVal vacancyNumber As Integer, ByVal customerNumber As Integer, ByVal ownerOnline As Boolean, ByVal JobChannelPriority As Boolean?, ByVal jobCHOnline As Boolean, ByVal ostJobOnline As Boolean) As Boolean
		Function UpdateOtherVacanciesAsOffline(ByVal vacancyNumbers As List(Of Integer)) As Boolean
		Function UpdateVacancyJobCHMasterData(ByVal JobCHMasterData As VacancyJobCHMasterData) As Boolean


		Function AddDefaultDataIntoJobCHDb(ByVal customerID As String, ByVal userNumber As Integer, ByVal vacancyNumber As Integer) As Boolean
		Function UpdateJobCHDbFieldValue(ByVal myDbFieldName As String, ByVal formatedText As String, ByVal plainText As String, ByVal vacancyNumber As Integer) As Boolean
		Function UpdateVacancyZusatzDbFieldValue(ByVal myDbFieldName As String, ByVal formatedText As String, ByVal plainText As String, ByVal vacancyNumber As Integer) As Boolean
		Function DuplicateVacancyData(ByVal mdNumber As Integer, ByVal oldVacancyNumber As Integer, ByVal vacancyMasterData As VacancyMasterData) As Boolean
		Function LoadVacancyInseratData(ByVal vacancyNumber As Integer) As VacancyInseratData
		Function LoadJobCHInseratData(ByVal vacancyNumber As Integer) As VacancyInseratJobCHData

		Function UpdateJobCHOccupationData(ByVal firstOccupation_Value As Integer?, secondOccupation_Value As Integer?,
																			 ByVal firstExperience_Value As Integer?, secondExperience_Value As Integer?,
																			 ByVal firstPosition_Value As Integer?, secondPosition_Value As Integer?,
																			 ByVal firstOccupation_Label As String, ByVal secondOccupation_Label As String,
																			 ByVal firstExperience_Label As String, ByVal secondExperience_Label As String,
																			 ByVal firstPosition_Label As String, ByVal secondPosition_Label As String,
																			 ByVal vacancyNumber As Integer) As Boolean
		Function UpdateJobCHBildungsniveauData(ByVal firstBildung_Value As Integer?, secondBildung_Value As Integer?, thirdBildung_Value As Integer?,
																					 ByVal firstBildung_Label As String, ByVal secondBildung_Label As String, ByVal thirdBildung_Label As String,
																					 ByVal vacancyNumber As Integer) As Boolean
		Function UpdateJobCHBrunchesData(ByVal bez_Value As Integer?, ByVal bez_Label As String, ByVal vacancyNumber As Integer) As Boolean
		Function UpdateJobCHRegionData(ByVal firstRegion_Value As Integer?, secondRegion_Value As Integer?,
																					 ByVal firstRegion_Label As String, ByVal secondRegion_Label As String,
																					 ByVal vacancyNumber As Integer) As Boolean
		Function UpdateVacancyLanguageData(ByVal languageData As VacancyJobCHLanguageData, ByVal jobPlattform As ExternalPlattforms) As Boolean

		Function DeleteVacancyData(ByVal vacancyNumber As Integer, ByVal userNumber As Integer) As Boolean
		Function DeleteJobCHLanguageData(ByVal languageData As VacancyJobCHLanguageData) As Boolean


		Function AllowedForJobCHTransfer(ByVal customerID As String, ByVal userNumber As Integer) As Boolean
		Function AllowedForOstJobTransfer(ByVal customerID As String, ByVal userNumber As Integer) As Boolean
		Function GetJobCHExportedCounterData(ByVal customerID As String, ByVal iVakNr As Integer, ByVal UserKST As String) As JobCHCounterData
		Function GetOstJobExportedCounterData(ByVal customerID As String, ByVal iVakNr As Integer, ByVal UserKST As String) As OstJobCounterData
		Function GetCountOfExportedInternVacancies(ByVal customerID As String) As Integer


		Function GetVacancyForExportToJobsCH(ByVal customerGuid As String, ByVal vacancyNumber As Integer) As DataTable
		Function GetVacancyForExportToOstJobsCH(ByVal customerGuid As String, ByVal vacancyNumber As Integer) As DataTable
		Function GetVacancyForExportToIntern(ByVal customerGuid As String, ByVal vacancyNumber As Integer) As DataTable


		Function LoadJobCHCustomerData(ByVal customerID As String, ByVal userNumber As Integer, ByVal vacancyNumber As Integer, ByVal plattformEnum As ExternalPlattforms) As VacancyJobCHPlattformCustomerData
		Function UpdateJobCHCustomerData(ByVal JobCHMasterData As VacancyJobCHMasterData) As Boolean
		Function UpdateVacancyJobCHAdvisorData(ByVal mdNr As Integer, ByVal vacancyNumber As Integer, ByVal userNumber As Integer, ByVal userData As AdvisorData, ByVal templateData As String) As Boolean

		Function LoadOstJobCustomerData(ByVal customerID As String, ByVal userNumber As Integer, ByVal vacancyNumber As Integer, ByVal plattformEnum As ExternalPlattforms) As VacancyOstJobPlattformCustomerData


		Function LoadVacancyZusatzMenuInfoData(ByVal customerID As String, ByVal plattformEnum As ExternalPlattforms) As IEnumerable(Of VacancyZusatzMenuData)


	End Interface

End Namespace