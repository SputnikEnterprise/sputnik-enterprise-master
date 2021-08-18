
Public Class TrXMLData

	Public Property TrxmlID As Integer
	Public Property FirstName As String
	Public Property LastName As String
	Public Property DateOfBirth As String
	Public Property Nationality As String
	Public Property Gender As String

	Public Property StreetName As String
	Public Property StreetNumberBase As String
	Public Property PostalCode As String
	Public Property City As String
	Public Property CountryCode As String

	Public Property HomePhone As String
	Public Property MobilePhone As String

	Public Property Email As String

	Public Property MaritalStatusCode As String

	Public Property Auto As Boolean?
	Public Property Motorcycle As Boolean?
	Public Property Bicycle As Boolean?

	Public Property DrivingLicence As String
	Public Property DocumentText As String
	Public Property PreProcessingTime As String
	Public Property ProcessingTime As String
	Public Property NormalizationTime As String
	Public Property TemplatingTime As String
	Public Property Totaltime As String

	Public Property ProfilePicture As String


	Public ReadOnly Property GenderLabel As String
		Get
			Select Case Gender
				Case 1
					Return "männlich"
				Case 2
					Return "weiblich"

				Case Else
					Return "unbekannt"

			End Select

		End Get
	End Property

	Public ReadOnly Property MaritalStatusLable As String
		Get
			Select Case MaritalStatusCode
				Case 1
					Return "Verheiratet"
				Case 2
					Return "Unverheiratet"
				Case 3
					Return "Zusammenlebende"
				Case 4
					Return "Verwitwet"
				Case 5
					Return "Geschieden"
				Case 6
					Return "Eingetragenen Partnerschaft"

				Case Else
					Return "Nicht bekannt"

			End Select

		End Get
	End Property


End Class
