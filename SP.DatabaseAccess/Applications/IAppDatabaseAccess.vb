
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.CVLizer.DataObjects
Imports SP.DatabaseAccess.EMailJob.DataObjects.EMailSettingData
Imports SP.DatabaseAccess.ScanJob.DataObjects

Namespace Applicant


	''' <summary>
	''' Interface for Applicant database access.
	''' </summary>
	Public Interface IAppDatabaseAccess


		Function AddApplication(ByVal application As ApplicationData, ByVal applicant As ApplicantData) As Boolean
		Function AddApplicationWithApplicant(ByVal application As ApplicationData, ByVal applicant As ApplicantData) As Boolean
		Function AddApplicationToClientDb(ByVal application As ApplicationData) As Boolean
		Function AddProfileDataForNotValidatedCVLData(ByVal applicantID As Integer, ByVal applicationID As Integer) As Boolean
		Function AddApplicantDocument(ByVal document As ApplicantDocumentData) As Boolean
		Function UpdateApplicatantWithCVLData(ByVal cvlData As CVLizerXMLData, ByVal applicantID As Integer, ByVal applicationID As Integer, ByVal priorityModul As PriorityModulEnum) As Boolean
		Function UpdateApplicationWithScanDropInData(ByVal customerID As String, ByVal applicationID As Integer, ByVal scanData As ApplicationData) As Boolean
		Function UpdateNewApplicationWithOldApplicantData(ByVal customerID As String, ByVal cvlProfileID As Integer, ByVal applicationID As Integer?, ByVal applicantID As Integer?) As Boolean

		Function LoadApplicationData(ByVal customerID As String, ByVal businessBranch As String) As IEnumerable(Of ApplicationData)
		Function LoadAssignedApplicantApplications(ByVal customerID As String, ByVal applicantNumber As Integer) As IEnumerable(Of ApplicationData)
		Function UpdateNewApplicationWithExistingApplicantData(ByVal customerID As String, ByVal applicationID As Integer, ByVal applicantID As Integer, ByVal hashValues As String) As Boolean
		Function DeleteAssignedApplication(ByVal customerID As String, ByVal applicationNumber As Integer) As Boolean
		Function DeleteAllrelatedApplicantData(ByVal customerID As String, ByVal applicantID As Integer) As Boolean

		'Function LoadApplicationDataForMainView(ByVal mdNr As Integer, ByVal sql As String, ByVal usFiliale As String) As IEnumerable(Of MainViewApplicationData)
		Function LoadAssignedApplicationDataForMainView(ByVal recID As Integer) As MainViewApplicationData
		Function UpdateApplicationWithAdvisorData(ByVal data As ApplicationData) As Boolean
		Function UpdateMainViewApplicationWithAdvisorData(ByVal data As MainViewApplicationData) As Boolean
		Function UpdateAllApplicantApplicationFlagData(ByVal data As MainViewApplicationData) As Boolean
		Function UpdateApplicationLabelData(ByVal data As MainViewApplicationData) As Boolean


		Function LoadApplicantData(ByVal customerID As String, ByVal businessBranch As String) As IEnumerable(Of ApplicantData)
		Function LoadAssignedApplicantData(ByVal customerID As String, ByVal applicantData As ApplicantData) As ApplicantData


		Function LoadDocumentData(ByVal customerID As String, ByVal businessBranch As String) As IEnumerable(Of ApplicantDocumentData)
		Function ExistsApplicantDocumentWithHashData(ByVal customerID As String, ByVal hashData As String) As ApplicantDocumentData
		Function LoadAssignedDocumentData(ByVal recID As Integer) As ApplicantDocumentData
		Function LoadAssignedDocumentContentData(ByVal docID As Integer) As Byte()
		Function DeleteAssignedDocument(ByVal customerID As String, ByVal docID As Integer) As Boolean


		''' <summary>
		''' loading cv data
		''' </summary>
		Function LoadCVPersonalData(ByVal customerID As String, ByVal businessBranchNumber As String, ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvPersonalData)


		''' <summary>
		''' notification
		''' </summary>
		Function AddApplicantDocumentToEmployee(ByVal documentData As ApplicantDocumentData) As Boolean


	End Interface


End Namespace
