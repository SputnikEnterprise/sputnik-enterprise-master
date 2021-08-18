Imports SP.DatabaseAccess.Applicant.DataObjects

Namespace Applicant

	''' <summary>
  ''' Interface for Applicant Cv database access.
	''' </summary>
  Public Interface IAppCvDatabaseAccess

    ''' <summary>
    ''' Adds CV address data.
    ''' </summary>
    ''' <param name="cvAddressData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvAddressData(ByRef cvAddressData As ApplicantCvAddressData) As Boolean

    ''' <summary>
    ''' Adds CV computer skill data.
    ''' </summary>
    ''' <param name="cvComputerSkillData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvComputerSkillData(ByRef cvComputerSkillData As ApplicantCvComputerSkillData) As Boolean

    ''' <summary>
    ''' Adds CV computer skill type data.
    ''' </summary>
    ''' <param name="cvComputerSkillTypeData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvComputerSkillTypeData(ByRef cvComputerSkillTypeData As ApplicantCvComputerSkillTypeData) As Boolean

    ''' <summary>
    ''' Adds CV custom area data.
    ''' </summary>
    ''' <param name="cvCustomAreaData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvCustomAreaData(ByRef cvCustomAreaData As ApplicantCvCustomAreaData) As Boolean

    ''' <summary>
    ''' Adds CV degree direction data.
    ''' </summary>
    ''' <param name="cvDegreeDirectionData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvDegreeDirectionData(ByRef cvDegreeDirectionData As ApplicantCvDegreeDirectionData) As Boolean

    ''' <summary>
    ''' Adds CV document HTML data.
    ''' </summary>
    ''' <param name="cvDocumentHtmlData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvDocumentHtmlData(ByRef cvDocumentHtmlData As ApplicantCvDocumentHtmlData) As Boolean

    ''' <summary>
    ''' Adds CV document text data.
    ''' </summary>
    ''' <param name="cvDocumentTextData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvDocumentTextData(ByRef cvDocumentTextData As ApplicantCvDocumentTextData) As Boolean

    ''' <summary>
    ''' Adds CV drivers licence type data.
    ''' </summary>
    ''' <param name="cvDriversLicenceData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvDriversLicenceData(ByRef cvDriversLicenceData As ApplicantCvDriversLicenceData) As Boolean

    ''' <summary>
    ''' Adds CV education data.
    ''' </summary>
    ''' <param name="cvEducationData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvEducationData(ByRef cvEducationData As ApplicantCvEducationData) As Boolean

    ''' <summary>
    ''' Adds CV education detail data.
    ''' </summary>
    ''' <param name="cvEducationDetailData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvEducationDetailData(ByRef cvEducationDetailData As ApplicantCvEducationDetailData) As Boolean

    ''' <summary>
    ''' Adds CV education history data.
    ''' </summary>
    ''' <param name="cvEmailData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvEducationHistoryData(ByRef cvEmailData As ApplicantCvEducationHistoryData) As Boolean

    ''' <summary>
    ''' Adds CV employment history data.
    ''' </summary>
    ''' <param name="cvEmailData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvEmploymentHistoryData(ByRef cvEmailData As ApplicantCvEmploymentHistoryData) As Boolean

    ''' <summary>
    ''' Adds CV E-Mail data.
    ''' </summary>
    ''' <param name="cvEmailData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvEmailData(ByRef cvEmailData As ApplicantCvEmailData) As Boolean

    ''' <summary>
    ''' Adds CV extra info data.
    ''' </summary>
    ''' <param name="cvExtraInfoData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvExtraInfoData(ByRef cvExtraInfoData As ApplicantCvExtraInfoData) As Boolean

    ''' <summary>
    ''' Adds CV hobby data.
    ''' </summary>
    ''' <param name="cvHobbyData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvHobbyData(ByRef cvHobbyData As ApplicantCvHobbyData) As Boolean

    ''' <summary>
    ''' Adds CV institute type type data.
    ''' </summary>
    ''' <param name="cvInstituteTypeData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvInstituteTypeData(ByRef cvInstituteTypeData As ApplicantCvInstituteTypeData) As Boolean

    ''' <summary>
    ''' Adds CV job title data.
    ''' </summary>
    ''' <param name="cvJobTitleData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvJobTitleData(ByRef cvJobTitleData As ApplicantCvJobTitleData) As Boolean

    ''' <summary>
    ''' Adds CV language skill data.
    ''' </summary>
    ''' <param name="cvLanguageSkillData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvLanguageSkillData(ByRef cvLanguageSkillData As ApplicantCvLanguageSkillData) As Boolean

    ''' <summary>
    ''' Adds CV other data.
    ''' </summary>
    ''' <param name="cvOtherData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvOtherData(ByRef cvOtherData As ApplicantCvOtherData) As Boolean

    ''' <summary>
    ''' Adds CV personal data.
    ''' </summary>
    ''' <param name="cvPersonalData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvPersonalData(ByRef cvPersonalData As ApplicantCvPersonalData) As Boolean

    ''' <summary>
    ''' Adds CV phone number data.
    ''' </summary>
    ''' <param name="cvPhoneNumberData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvPhoneNumberData(ByRef cvPhoneNumberData As ApplicantCvPhoneNumberData) As Boolean

    ''' <summary>
    ''' Adds CV picture data.
    ''' </summary>
    ''' <param name="cvPictureData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvPictureData(ByRef cvPictureData As ApplicantCvPictureData) As Boolean

    ''' <summary>
    ''' Adds CV profile data.
    ''' </summary>
    ''' <param name="cvProfileData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvProfileData(ByRef cvProfileData As ApplicantCvProfileData) As Boolean

    ''' <summary>
    ''' Adds CV reference data.
    ''' </summary>
    ''' <param name="cvReferenceData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvReferenceData(ByRef cvReferenceData As ApplicantCvReferenceData) As Boolean

    ''' <summary>
    ''' Adds CV social media data.
    ''' </summary>
    ''' <param name="cvSocialMediaData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvSocialMediaData(ByRef cvSocialMediaData As ApplicantCvSocialMediaData) As Boolean

    ''' <summary>
    ''' Adds CV social media type data.
    ''' </summary>
    ''' <param name="cvSocialMediaTypeData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvSocialMediaTypeData(ByRef cvSocialMediaTypeData As ApplicantCvSocialMediaTypeData) As Boolean

    ''' <summary>
    ''' Adds CV skill data.
    ''' </summary>
    ''' <param name="cvSkillData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvSkillData(ByRef cvSkillData As ApplicantCvSkillData) As Boolean

    ''' <summary>
    ''' Adds CV soft skill data.
    ''' </summary>
    ''' <param name="cvSoftSkillData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvSoftSkillData(ByRef cvSoftSkillData As ApplicantCvSoftSkillData) As Boolean

    ''' <summary>
    ''' Adds CV soft skill type data.
    ''' </summary>
    ''' <param name="cvSoftSkillTypeData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvSoftSkillTypeData(ByRef cvSoftSkillTypeData As ApplicantCvSoftSkillTypeData) As Boolean

    ''' <summary>
    ''' Adds CV transportation data.
    ''' </summary>
    ''' <param name="cvTransportationData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AddCvTransportationData(ByRef cvTransportationData As ApplicantCvTransportationData) As Boolean

    ''' <summary>
    ''' Deletes CV profile data.
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="trxmlID"></param>
    ''' <param name="deleteRelated"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function DeleteCvProfileData(
      Optional ByVal id As Integer? = Nothing,
      Optional ByVal trxmlID As Integer? = Nothing,
      Optional ByVal deleteRelated As Boolean = False
      ) As Boolean

    ''' <summary>
    ''' Gets a CV approval data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvApproval(ByVal code As String) As ApplicantCvApprovalData

    ''' <summary>
    ''' Gets a CV availability data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvAvailability(ByVal code As String) As ApplicantCvAvailabilityData

    ''' <summary>
    ''' Gets a CV computer skill type data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvComputerSkillType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvComputerSkillTypeData

    ''' <summary>
    ''' Gets a CV country data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvCountry(ByVal code As String) As ApplicantCvCountryData

    ''' <summary>
    ''' Gets a CV degree direction data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvDegreeDirection(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvDegreeDirectionData

    ''' <summary>
    ''' Gets a CV diploma data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvDiploma(ByVal code As String) As ApplicantCvDiplomaData

    ''' <summary>
    ''' Gets a CV drivers licence data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvDriversLicence(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvDriversLicenceData

    ''' <summary>
    ''' Gets a CV education data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvEducation(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvEducationData

    ''' <summary>
    ''' Gets a CV education detail data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvEducationDetail(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvEducationDetailData

    ''' <summary>
    ''' Gets a CV education level data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvEducationLevel(ByVal code As String) As ApplicantCvEducationLevelData

    ''' <summary>
    ''' Gets a CV E-Mail type by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvEmailType(ByVal code As String) As ApplicantCvEmailTypeData

    ''' <summary>
    ''' Gets a CV gender data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvGender(ByVal code As String) As ApplicantCvGenderData

    ''' <summary>
    ''' Gets a CV highest education level data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvHighestEducationLevel(ByVal code As String) As ApplicantCvHighestEducationLevelData

    ''' <summary>
    ''' Gets a CV institute type data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvInstituteType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvInstituteTypeData

    ''' <summary>
    ''' Gets a CV job title data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvJobTitle(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvJobTitleData

    ''' <summary>
    ''' Gets a CV language skill type data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvLanguageSkillType(ByVal code As String) As ApplicantCvLanguageSkillTypeData
		Function GetCvLanguageSkillTypeByID(ByVal recID As Integer) As List(Of ApplicantCvLanguageSkillTypeData)

		''' <summary>
		''' Gets a CV language proficiency data by code
		''' </summary>
		''' <param name="code"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetCvLanguageProficiency(ByVal code As String) As ApplicantCvLanguageProficiencyData

    ''' <summary>
    ''' Gets a CV marital status data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvMaritalStatus(ByVal code As String) As ApplicantCvMaritalStatusData

    ''' <summary>
    ''' Gets a CV nationality data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvNationality(ByVal code As String) As ApplicantCvNationalityData

    ''' <summary>
    ''' Gets a CV phone number type by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvPhoneNumberType(ByVal code As String) As ApplicantCvPhoneNumberTypeData

    ''' <summary>
    ''' Gets a CV profile status data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvProfileStatus(ByVal code As String) As ApplicantCvProfileStatusData

    ''' <summary>
    ''' Gets a CV region data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvRegion(ByVal code As String) As ApplicantCvRegionData

    ''' <summary>
    ''' Gets a CV salary data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvSalary(ByVal code As String) As ApplicantCvSalaryData

    ''' <summary>
    ''' Gets a CV social media type by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvSocialMediaType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvSocialMediaTypeData

    ''' <summary>
    ''' Gets a CV soft skill type data by code
    ''' </summary>
    ''' <param name="code"></param>
    ''' <param name="name"></param>
    ''' <param name="description"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetCvSoftSkillType(ByVal code As String, Optional ByVal name As String = Nothing, Optional ByVal description As String = Nothing) As ApplicantCvSoftSkillTypeData



#Region "loading data"

		Function LoadAllCvProfileData() As IEnumerable(Of ApplicantCvProfileData)
		Function LoadCvProfileData(ByVal trxmlID As Integer?) As ApplicantCvProfileData
		Function LoadCvPersonalData(ByVal recID As Integer?) As ApplicantCvPersonalData
		Function LoadCvAddressData(ByVal trxmlID As Integer?) As ApplicantCvAddressData
		Function LoadCvPictureData(ByVal trxmlID As Integer?) As ApplicantCvPictureData
		Function LoadCvPhoneNumberData(ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvPhoneNumberData)
		Function LoadCvEMailData(ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvEmailData)
		Function LoadCvSkillData(ByVal trxmlID As Integer?) As IEnumerable(Of ApplicantCvLanguageSkillData)


#End Region


	End Interface

End Namespace
