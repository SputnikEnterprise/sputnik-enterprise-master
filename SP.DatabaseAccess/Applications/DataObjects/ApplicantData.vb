
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
'Imports System.Drawing

Namespace Applicant.DataObjects

	Public Class ApplicationData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property FK_ApplicantID As Integer
		Public Property ApplicationID As Integer
		Public Property VacancyNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property ApplicationLabel As String
		Public Property BusinessBranch As String
		Public Property Advisor As String
		Public Property AdvisorLastname As String
		Public Property AdvisorFirstname As String
		Public Property Dismissalperiod As String
		Public Property Availability As String
		Public Property Comment As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property ApplicationLifecycle As Integer?
		Public Property EmployeeID As Integer

		Public ReadOnly Property AdvisorFullname() As String
			Get
				Return String.Format("{0} {1}", AdvisorFirstname, AdvisorLastname)
			End Get
		End Property

		Public ReadOnly Property ApplicationLifecycleLabel As String

			Get
				Select Case ApplicationLifecycle

					Case ApplicationLifecycelEnum.APPLICATIONCLOSED
						Return String.Format("{0}", "Geschlossen")
					Case ApplicationLifecycelEnum.APPLICATIONFORWARDED
						Return String.Format("{0}", "Weitergeleitet")
					Case ApplicationLifecycelEnum.APPLICATIONREJECTED
						Return String.Format("{0}", "Abgesagt")
					Case ApplicationLifecycelEnum.APPLICATIONSUCCESS
						Return String.Format("{0}", "Erfolgreich")
					Case ApplicationLifecycelEnum.APPLICATIONVIEWED
						Return String.Format("{0}", "Kontrolliert")
					Case ApplicationLifecycelEnum.PROPOSE
						Return String.Format("{0}", "Vorgeschlagen")

					Case Else
						Return String.Format("{0}", "Neu")

				End Select

			End Get
		End Property


	End Class


	Public Class MainViewApplicationData

		Public Property ID As Integer
		Public Property ApplicationID As Integer
		Public Property MDNr As Integer
		Public Property zfiliale As String
		Public Property Customer_ID As String
		Public Property EmployeeID As Integer
		Public Property ApplicationLabel As String
		Public Property VacancyNumber As Integer?
		Public Property Advisor As String
		Public Property BusinessBranch As String
		Public Property Dismissalperiod As String
		Public Property Availability As String
		Public Property Comment As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property ApplicationLifecycle As Integer?
		Public Property ApplicantLastname As String
		Public Property ApplicantFirstname As String
		Public Property Birthdate As Date?
		Public Property ApplicantStreet As String
		Public Property ApplicantPostcode As String
		Public Property ApplicantLocation As String
		Public Property ApplicantCountry As String
		Public Property CVLProfileID As Integer?
		Public Property ApplicantLifecycle As Integer
		Public Property Customernumber As Integer
		Public Property Customername As String
		Public Property CustomerStreet As String
		Public Property CustomerPostcode As String
		Public Property CustomerLocation As String
		Public Property CustomerCountry As String
		Public Property ApplicationAdvisorFirstName As String
		Public Property VacancyLable As String
		Public Property ApplicationAdvisorLastName As String
		Public Property ApplicationMandantName As String
		Public Property ShowAsApplicant As Boolean?


		Public ReadOnly Property ApplicantFullname As String
			Get
				Return String.Format("{1} {0}", ApplicantFirstname, ApplicantLastname)
			End Get
		End Property

		Public ReadOnly Property ApplicantPostcodeLocation As String
			Get
				Return String.Format("{0} {1} ", ApplicantPostcode, ApplicantLocation)
			End Get
		End Property

		Public ReadOnly Property ApplicantAddress As String
			Get
				Return String.Format("{0}, {1} {2}", ApplicantStreet, ApplicantPostcode, ApplicantLocation)
			End Get
		End Property

		Public ReadOnly Property ApplicationAdvisorFullname As String
			Get
				Return String.Format("{1} {0}", ApplicationAdvisorFirstName, ApplicationAdvisorLastName)
			End Get
		End Property

		Public ReadOnly Property CustomerAddress As String
			Get
				Return String.Format("{0}, {1} {2}", CustomerStreet, CustomerPostcode, CustomerLocation)
			End Get
		End Property

		Public ReadOnly Property ApplicationLifeCycelLabel As String
			Get
				Select Case ApplicationLifecycle

					Case ApplicationLifecycelEnum.APPLICATIONCLOSED
						Return String.Format("{0}", "Geschlossen")
					Case ApplicationLifecycelEnum.APPLICATIONFORWARDED
						Return String.Format("{0}", "Weitergeleitet")
					Case ApplicationLifecycelEnum.APPLICATIONREJECTED
						Return String.Format("{0}", "Abgesagt")
					Case ApplicationLifecycelEnum.APPLICATIONSUCCESS
						Return String.Format("{0}", "Erfolgreich")
					Case ApplicationLifecycelEnum.APPLICATIONVIEWED
						Return String.Format("{0}", "Kontrolliert")
					Case ApplicationLifecycelEnum.PROPOSE
						Return String.Format("{0}", "Vorgeschlagen")

					Case Else
						Return String.Format("{0}", "Neu")

				End Select


			End Get
		End Property

		'Public Enum ApplicantLifecycelEnum

		'	OPEN
		'	EMPLOYEE
		'	PBREJECTED
		'	EMPLOYEEREJECTED

		'End Enum

		'Public Enum ApplicationLifecycelEnum

		'	APPLICATIONNEW
		'	APPLICATIONREJECTED
		'	APPLICATIONVIEWED
		'	APPLICATIONFORWARDED
		'	PROPOSE
		'	APPLICATIONCLOSED
		'	APPLICATIONSUCCESS

		'End Enum

	End Class

	Public Enum ApplicationLifecycelEnum

		APPLICATIONNEW
		APPLICATIONREJECTED
		APPLICATIONVIEWED
		APPLICATIONFORWARDED
		PROPOSE
		APPLICATIONCLOSED
		APPLICATIONSUCCESS

	End Enum

	Public Enum ApplicantLifecycelEnum

		OPEN
		EMPLOYEE
		PBREJECTED
		EMPLOYEEREJECTED

	End Enum

	Public Class ApplicantData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property ApplicantNumber As Integer
		Public Property EmployeeID As Integer
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property Gender As String
		Public Property Street As String
		Public Property PostOfficeBox As String
		Public Property Postcode As String
		Public Property Latitude As Double?
		Public Property Longitude As Double?
		Public Property Location As String
		Public Property Country As String
		Public Property Nationality As String
		Public Property EMail As String
		Public Property Telephone As String
		Public Property MobilePhone As String
		Public Property Birthdate As Date?
		Public Property Permission As String
		Public Property Profession As String
		Public Property Auto As Boolean
		Public Property Motorcycle As Boolean
		Public Property Bicycle As Boolean
		Public Property DrivingLicence1 As String
		Public Property DrivingLicence2 As String
		Public Property DrivingLicence3 As String
		Public Property CivilState As Integer
		Public Property Language As String
		Public Property LanguageLevel As Integer
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ApplicantLifecycle As Integer?
		Public Property CVLProfileID As Integer?

		Public ReadOnly Property ApplicantFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property Address As String
			Get
				Return String.Format("{0}, {1} {2}", Street, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property GenderLabel As String
			Get
				If Gender = 0 Then Return "männlich" Else Return "weiblich"
			End Get
		End Property


	End Class


	Public Class ApplicantDocumentData

		Public Property ID As Integer
		Public Property FK_ApplicantID As Integer
		Public Property DocClass As String
		Public Property Type As Integer?
		Public Property Flag As Integer?
		Public Property Category As Integer?
		Public Property Title As String
		Public Property FileExtension As String
		Public Property Content As Byte()
		Public Property HashValue As String
		Public Property Pages As Integer?
		Public Property FileSize As Integer?
		Public Property PlainText As String
		Public Property DocXML As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class


End Namespace
