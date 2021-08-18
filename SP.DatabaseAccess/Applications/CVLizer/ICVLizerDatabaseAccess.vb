

Imports SP.DatabaseAccess.CVLizer.DataObjects


Namespace CVLizer


	''' <summary>
	''' Interface for CVLizer database access.
	''' </summary>
	Public Interface ICVLizerDatabaseAccess


#Region "view data"

		Function LoadCVLProfileViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As IEnumerable(Of CVLizerProfileViewData)
		Function LoadAssignedCVLProfileViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As CVLizerProfileViewData

		Function LoadAssignedCVLPersonalViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As PersonalViewData
		Function LoadAssignedCVLPersonalAddressViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As AddressViewData
		Function LoadAssignedCVLPersonalTitleViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData)
		Function LoadAssignedCVLPersonalEMailViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData)
		Function LoadAssignedCVLPersonalHomepageViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData)
		Function LoadAssignedCVLPersonalTelefonNumberViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData)
		Function LoadAssignedCVLPersonalTelefaxNumberViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As IEnumerable(Of PersonalListViewData)


		Function LoadCVLWorkPhaseViewData(ByVal cvlPrifleID As Integer?, ByVal workID As Integer) As IEnumerable(Of WorkPhaseViewData)

		Function LoadAssignedCVLWorkPhaseAddressViewData(ByVal phaseID As Integer) As IEnumerable(Of AddressViewData)
		Function LoadAssignedCVLWorkPhaseSkillViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLWorkPhaseSoftSkillViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLWorkPhaseOperationAreaViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLWorkPhaseIndustryViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLWorkPhaseCustomCodeViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLWorkPhaseTopicViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeViewData)
		Function LoadAssignedCVLWorkPhaseInternetResourceViewData(ByVal phaseID As Integer) As IEnumerable(Of InternetResourceViewData)
		Function LoadAssignedCVLWorkPhaseDocumentIDViewData(ByVal phaseID As Integer) As IEnumerable(Of IDiewData)


		Function LoadAssignedCVLWorkPhaseCompanyViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeViewData)
		Function LoadAssignedCVLWorkPhaseFunctionViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeViewData)
		Function LoadAssignedCVLWorkPhasePositionViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData)
		Function LoadAssignedCVLWorkPhaseEmploymentViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData)
		Function LoadAssignedCVLWorkPhaseWorktimeViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData)

		Function LoadCVLEducationPhaseViewData(ByVal cvlPrifleID As Integer?, ByVal educationID As Integer) As IEnumerable(Of EducationPhaseViewData)

		Function LoadCVLPublicationViewData(ByVal cvlPrifleID As Integer?) As IEnumerable(Of PublicationViewData)


		Function LoadCVLAdditionalInfoViewData(ByVal cvlPrifleID As Integer?, ByVal addID As Integer) As AdditionalInfoViewData
		Function LoadAssignedCVLAdditionalDrivingLicenceViewData(ByVal addID As Integer) As IEnumerable(Of CodeViewData)
		Function LoadAssignedCVLAddtioinalUndatedSkillViewData(ByVal addID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLAddtioinalUndatedOperationAreaViewData(ByVal addID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLAddtioinalUndatedIndustryViewData(ByVal addID As Integer) As IEnumerable(Of CodeNameWeightViewData)
		Function LoadAssignedCVLAddtioinalInternetResourceViewData(ByVal addID As Integer) As IEnumerable(Of InternetResourceViewData)
		Function LoadAssignedCVLAddtioinalLanguageData(ByVal addID As Integer) As IEnumerable(Of LanguageData)


		Function LoadAssignedCVLDocumentData(ByVal cvlPrifleID As Integer) As IEnumerable(Of DocumentViewData)
		Function LoadAssignedDocumentData(ByVal id As Integer) As DocumentViewData
		Function LoadAssignedPersonalInformationDataWithFileHashValueData(ByVal customerID As String, ByVal fileHashvalue As String) As PersonalInformationData


#End Region


#Region "save parsing data"

		Function ExistsCVLFile(ByVal customerID As String, ByVal cvHashvalue As String) As Boolean

		Function AddCvPersonalInformationData(ByVal customerID As String, ByVal cvlXMLData As CVLizerXMLData, ByVal cvPersonalinformationData As PersonalInformationData) As Boolean
		Function AddCVLPhaseData(ByVal profileID As Integer, ByVal cvlPhaseData As Phase) As Boolean
		Function AddCVLPhaseLocationData(ByVal phaseID As Integer, ByVal cvlLocationData As AddressData) As Boolean
		Function AddCVLPhaseSkillData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLPhaseSoftSkillData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLPhaseOperationAreaData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLPhaseIndustryData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLPhaseCustomCodeData(ByVal phaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLPhaseInternetResourceData(ByVal phaseID As Integer, ByVal data As InternetResource) As Boolean

		Function AddCVLWorkPhaseData(ByVal workID As Integer, ByVal phaseID As Integer, ByVal cvlWorkPhaseData As WorkPhaseData) As Boolean
		Function AddCVLWorkPhasePositionData(ByVal WorkPhaseID As Integer, ByVal data As CodeNameData) As Boolean
		Function AddCVLWorkPhaseEmploymentData(ByVal WorkPhaseID As Integer, ByVal data As CodeNameData) As Boolean
		Function AddCVLWorkPhaseWorktimeData(ByVal WorkPhaseID As Integer, ByVal data As CodeNameData) As Boolean


		Function AddCVLEducationPhaseData(ByVal educationID As Integer, ByVal phaseID As Integer, ByVal cvlEducationPhaseData As EducationPhaseData) As Boolean
		Function AddCVLEducationPhaseEducationTypeData(ByVal educationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean


		Function AddCVLPublicationPhaseData(ByVal cvlProfileID As Integer, ByVal phaseID As Integer, ByVal cvPublicationData As PublicationData) As Boolean
		Function AddCVLPublicationPhaseAutorData(ByVal publicationPhaseID As Integer, ByVal autor As String) As Boolean


		Function AddCVLAdditionalInformationData(ByVal cvlProfileID As Integer, ByVal cvAddData As OtherInformationData) As Boolean
		Function AddCVLAdditionalLanguageData(ByVal publicationPhaseID As Integer, ByVal lang As LanguageData) As Boolean
		Function AddCVLAdditionalDriverLicenceData(ByVal publicationPhaseID As Integer, ByVal dLicence As String) As Boolean
		Function AddCVLAdditionalUndateSkillData(ByVal publicationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLAdditionalUndatedOperationAreaData(ByVal publicationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLAdditionalUndatedIndustryData(ByVal publicationPhaseID As Integer, ByVal data As CodeNameWeightedData) As Boolean
		Function AddCVLAdditionalInternetResourceData(ByVal publicationPhaseID As Integer, ByVal data As InternetResource) As Boolean


		Function AddCVLObjectiveData(ByVal cvlProfileID As Integer, ByVal data As ObjectiveData) As Boolean
		Function AddCVLObjectiveWorkPhaseData(ByVal objID As Integer, ByVal phaseID As Integer, ByVal cvlWorkPhaseData As WorkPhaseData) As Boolean
		Function AddCVLObjectivePhasePositionData(ByVal phaseID As Integer, ByVal data As CodeNameData) As Boolean
		Function AddCVLObjectivePhaseEmploymentData(ByVal phaseID As Integer, ByVal data As CodeNameData) As Boolean
		Function AddCVLObjectivePhaseWorktimeData(ByVal phaseID As Integer, ByVal data As CodeNameData) As Boolean
		Function AddCVLObjectiveSalaryData(ByVal objID As Integer, ByVal salary As String) As Boolean


		Function AddCVLStatisticData(ByVal cvlProfileID As Integer, ByVal data As CVCodeSummaryData) As Boolean

		Function AddParsingFileHashData(ByVal customerID As String, ByVal myFilename As String, ByVal fileHashvalue As String) As Boolean
		Function AddCVLDocumentData(ByVal cvlProfileID As Integer, ByVal data As DocumentData) As Boolean


#End Region


		Function AddCustomerPayableServiceUsage(ByVal customerID As String, ByVal userData As CustomerPayableUserData) As Boolean


#Region "search data"

		Function AddCVLExperiencesData(ByVal profileID As Integer, ByVal exData As CVLExperiencesData) As Boolean

#End Region



	End Interface


End Namespace
