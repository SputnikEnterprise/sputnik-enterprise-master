

Namespace CVLizer.DataObjects


	Public Class CVFileData

		Public Property FileName As String
		Public Property XMLFileName As String
		Public Property FileExtension As String
		Public Property FileDate As DateTime
		Public Property Filesize As Integer
		Public Property FileContent As Byte()
		Public Property FileHash As String

	End Class

	Public Class CVLizerXMLData

		Public Property ProfileID As Integer?
		Public Property Customer_ID As String

		Public Property PersonalInformation As PersonalInformationData
		Public Property Work As WPhaseData
		Public Property Education As EdPhaseData
		Public Property Publication As List(Of PublicationData)
		Public Property AdditionalInformation As OtherInformationData
		Public Property Objective As ObjectiveData
		Public Property Statistics As List(Of CVCodeSummaryData)
		Public Property Documents As List(Of DocumentData)

		Public Property ContinueWithImport As Boolean?

	End Class

	Public Class PersonalInformationData

		Public Property PersonalID As Integer?
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Gender As CodeNameData
		Public Property Title As List(Of String)
		Public Property IsCed As CodeNameData
		Public Property DateOfBirth As Date?
		Public Property DateOfBirthPlace As String
		Public Property Nationality As List(Of CodeNameData)
		Public Property CivilStatus As List(Of CodeNameData)
		Public Property Address As AddressData
		Public Property Email As List(Of String)
		Public Property PhoneNumbers As List(Of String)
		Public Property Homepage As List(Of String)
		Public Property TelefaxNumber As List(Of String)
		Public Property ISValid As Boolean?

		Public ReadOnly Property FullName As String
			Get
				Return String.Format("{0} {1} {2}", Gender.CodeName, FirstName, LastName)
			End Get
		End Property

		Public ReadOnly Property DateOfBirthYear As Integer?
			Get
				Return Year(DateOfBirth)
			End Get
		End Property


	End Class

	Public Class WPhaseData
		Public Property ID As Integer?
		Public Property WorkPhases As List(Of WorkPhaseData)
		Public Property AdditionalText As String

	End Class

	Public Class EdPhaseData
		Public Property ID As Integer?
		Public Property EducationPhases As List(Of EducationPhaseData)
		Public Property AdditionalText As String

	End Class

	Public Class PublicationData
		Inherits Phase

		Public Property PublicationPhaseID As Integer?
		Public Property Author As List(Of String)
		Public Property Proceedings As String
		Public Property Institute As String

	End Class


	Public Class Phase
		Public Property PhaseID As Integer?
		Public Property DateFrom As Date?
		Public Property DateTo As Date?
		Public Property DateFromFuzzy As String
		Public Property DateToFuzzy As String
		Public Property Duration As Integer?
		Public Property Current As Boolean?
		Public Property SubPhase As Boolean?
		Public Property Location As List(Of AddressData)
		Public Property Skill As List(Of CodeNameWeightedData)
		Public Property SoftSkill As List(Of CodeNameWeightedData)

		Public Property OperationAreas As List(Of CodeNameWeightedData)
		Public Property Industries As List(Of CodeNameWeightedData)
		Public Property CustomCodes As List(Of CodeNameWeightedData)

		Public Property Topic As List(Of String)
		Public Property Comments As String
		Public Property PlainText As String
		Public Property InternetRosources As List(Of InternetResource)
		Public Property DocumentID As List(Of Integer)

	End Class

	Public Class WorkPhaseData
		Inherits Phase

		Public Property WorkPhaseID As Integer?

		Public Property Company As List(Of String)
		Public Property Functions As List(Of String)
		Public Property Positions As List(Of CodeNameData)
		Public Property Project As Boolean?
		Public Property Employments As List(Of CodeNameData)
		Public Property WorkTimes As List(Of CodeNameData)

	End Class


	Public Class EducationPhaseData
		Inherits Phase

		Public Property EducationPhaseID As Integer?
		Public Property IsCed As CodeNameData
		Public Property EducationType As List(Of CodeNameWeightedData)
		Public Property SchoolName As List(Of String)
		Public Property Graduation As List(Of String)
		Public Property Completed As Boolean?
		Public Property Score As Integer?

	End Class

	Public Class OtherInformationData

		Public Property ID As Integer?
		Public Property Languages As List(Of LanguageData)
		Public Property DrivingLicence As List(Of String)
		Public Property MilitaryService As Boolean?
		Public Property Competences As String
		Public Property Interests As String
		Public Property Additionals As String
		Public Property UndatedSkill As List(Of CodeNameWeightedData)
		Public Property UndatedOperationArea As List(Of CodeNameWeightedData)
		Public Property UndatedIndustry As List(Of CodeNameWeightedData)
		Public Property InternetRosources As List(Of InternetResource)

	End Class

	Public Class ObjectiveData
		Inherits WorkPhaseData

		Public Property ID As Integer?
		Public Property Salary As List(Of String)
		Public Property AvailabilityDate As Date?

	End Class


	Public Class CVCodeSummaryData
		Inherits CodeNameWeightedData

		Public Property ID As Integer?
		Public Property Duration As Integer?
		Public Property Domain As String

	End Class

	Public Class DocumentData
		Public Property ID As Integer?

		Public Property DocClass As String
		Public Property Pages As Integer?
		Public Property Plaintext As String
		Public Property FileType As String
		Public Property DocBinary As Byte()
		Public Property DocID As Integer?
		Public Property DocSize As Integer?
		Public Property DocLanguage As String
		Public Property FileHashvalue As String
		Public Property DocXML As String

	End Class



	Public Class CodeNameData
		Public Property Code As String
		Public Property CodeName As String

	End Class


	Public Class AddressData
		Public Property Street As String
		Public Property Postcode As String
		Public Property City As String
		Public Property Country As CodeNameData
		Public Property State As String

	End Class

	Public Class CodeNameWeightedData
		Public Property Code As String
		Public Property Name As String
		Public Property Weight As Double?

	End Class

	Public Class InternetResource
		Public Property URL As String
		Public Property Title As String
		Public Property Source As String
		Public Property Snippet As String

	End Class

	Public Class LanguageData
		Inherits CodeNameData

		Public Property Level As CodeNameData

	End Class


	Public Class CustomerPayableUserData
		Public Property CustomerID As String
		Public Property Advisorname As String
		Public Property AdvisorID As String
		Public Property ServiceName As String
		Public Property JobID As String

	End Class


End Namespace

