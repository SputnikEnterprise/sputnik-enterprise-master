
Namespace AVAMWebServiceProcess


	Public Class JobroomSearchResultData
		Public Property Content As List(Of SPJobroomData)
		Public Property TotalElements As Integer?
		Public Property TotalPages As Integer?
		Public Property CurrentPage As Integer?
		Public Property CurrentSize As Integer?
		Public Property First As Boolean?
		Public Property Last As Boolean?
		Public Property ErrorMessage As SPErrorData

	End Class

	Public Class SPAVAMCreationResultData
		Public Property RecID As Integer?
		Public Property JobroomID As String
		Public Property AVAMRecordState As String
		Public Property ExternalReference As String
		Public Property StellennummerEgov As String
		Public Property StellennummerAvam As String
		Public Property Fingerprint As String
		Public Property ApprovalDate As String
		Public Property JobCenterCode As String
		Public Property RejectionDate As String
		Public Property RejectionCode As String
		Public Property RejectionReason As String
		Public Property CancellationDate As String
		Public Property CancellationCode As String
		Public Property ReportingObligation As Boolean?
		Public Property ReportingObligationEndDate As DateTime?
		Public Property ErrorMessage As SPErrorData
		Public Property SyncDate As DateTime?
		Public Property SyncFrom As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property QueryContent As String
		Public Property ResultContent As String
		Public Property State As Boolean?

	End Class

	Public Class SPAVAMJobCreationData
		Public Property RecID As Integer?
		Public Property AVAMRecordState As String
		Public Property JobroomID As String
		Public Property QueryContent As String
		Public Property ResultContent As String
		Public Property ReportingObligation As Boolean?
		Public Property reportingObligationEndDate As DateTime?
		Public Property State As Boolean?
		Public Property Content As SPJobContentData
		Public Property ErrorMessage As SPErrorData
		Public Property SyncDate As DateTime?
		Public Property SyncFrom As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

	Public Class SPErrorData
		Public Property Content As String
		Public Property Title As String
		Public Property Status As String
		Public Property Message As String
		Public Property Detail As String

	End Class


	Public Class SPJobroomData
		Public Property ID As String
		Public Property Status As String
		Public Property SourceSystem As String
		Public Property ExternalReference As String
		Public Property StellennummerEgov As String
		Public Property StellennummerAvam As String
		Public Property Fingerprint As String
		Public Property ReportingObligation As Boolean?
		Public Property ReportingObligationEndDate As Date?
		Public Property ReportToAvam As Boolean?
		Public Property JobCenterCode As String
		Public Property ApprovalDate As Date?
		Public Property RejectionDate As Date?
		Public Property RejectionCode As String
		Public Property RejectionReason As String
		Public Property CancellationDate As Date?
		Public Property CancellationCode As String
		Public Property JobContent As SPJobContentData
		Public Property Publication As PublicationData

	End Class

	Public Class SPAVAMQueryResultData
		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property User_ID As String
		Public Property Advertisment_ID As Integer?
		Public Property JobroomID As String
		Public Property ResultContent As String
		Public Property ReportingObligation As Boolean?
		Public Property ReportingObligationEndDate As Date?
		Public Property CreatedOn As Date?
		Public Property CreatedFrom As String

	End Class

	Public Class SPJobContentData
		Public Property ExternalUrl As String
		Public Property NumberOfJobs As String
		Public Property JobDescriptions As List(Of SPJobDescriptionData)
		Public Property Company As SPCompanyData
		Public Property Employment As SPEmploymentData
		Public Property Location As SPLocationData
		Public Property Occupations As List(Of SPOccupationsData)
		Public Property LanguageSkills As List(Of SPLanguageSkillsData)
		Public Property ApplyChannel As SPApplyChannelData
		Public Property PublicContact As SPPublicContactData

	End Class

	Public Class SPJobDescriptionData
		Public Property LanguageIsoCode As String
		Public Property Title As String
		Public Property Description As String

	End Class

	Public Class SPCompanyData
		Public Property Name As String
		Public Property Street As String
		Public Property HouseNumber As String
		Public Property PostalCode As String
		Public Property City As String
		Public Property CountryIsoCode As String
		Public Property PostOfficeBoxNumber As String
		Public Property PostOfficeBoxPostalCode As String
		Public Property PostOfficeBoxCity As String
		Public Property Phone As String
		Public Property Email As String
		Public Property Website As String
		Public Property Surrogate As Boolean?

	End Class

	Public Class SPEmploymentData
		Public Property StartDate As Date?
		Public Property EndDate As Date?
		Public Property ShortEmployment As Boolean?
		Public Property Immediately As Boolean?
		Public Property Permanent As Boolean?
		Public Property WorkloadPercentageMin As String
		Public Property WorkloadPercentageMax As String
		Public Property WorkForms As List(Of SPWorkFormsData)

	End Class

	Public Class SPLocationData
		Public Property Remarks As String
		Public Property City As String
		Public Property PostalCode As String
		Public Property CommunalCode As String
		Public Property RegionCode As String
		Public Property CantonCode As String
		Public Property CountryIsoCode As String
		Public Property Coordinates As String

	End Class

	Public Class SPWorkFormsData
		Public Property Value As String

	End Class

	Public Class SPOccupationsData
		Public Property AvamOccupationCode As Integer?
		Public Property WorkExperience As String
		Public Property EducationCode As String

	End Class

	Public Class SPLanguageSkillsData
		Public Property LanguageIsoCode As String
		Public Property SpokenLevel As String
		Public Property WrittenLevel As String

	End Class

	Public Class SPApplyChannelData
		Public Property MailAddress As String
		Public Property EmailAddress As String
		Public Property PhoneNumber As String
		Public Property FormUrl As String
		Public Property AdditionalInfo As String

	End Class

	Public Class SPPublicContactData
		Public Property Salutation As String
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Phone As String
		Public Property Email As String

	End Class

	Public Class PublicationData
		Public Property StartDate As Date?
		Public Property EndDate As Date?
		Public Property EuresDisplay As Boolean?
		Public Property EuresAnonymous As Boolean?
		Public Property PublicDisplay As Boolean?
		Public Property PublicAnonymous As Boolean?
		Public Property RestrictedDisplay As Boolean?
		Public Property RestrictedAnonymous As Boolean?
		Public Property CompanyAnonymous As Boolean?

	End Class

	Public Enum AVAMAdvertismentCancelReasonENUM

		OCCUPIED_JOBCENTER
		OCCUPIED_AGENCY
		OCCUPIED_JOBROOM
		OCCUPIED_OTHER
		NOT_OCCUPIED
		CHANGE_OR_REPOSE

	End Enum


End Namespace
