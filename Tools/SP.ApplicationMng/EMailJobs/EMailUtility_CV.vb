
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.EMailJob.DataObjects

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Namespace ChilKatUtility


	Partial Class EMailUtility


#Region "Private Consts"

		Private Const FILE_VALUE_PATTERN_APPLICATION As String = "{0}: ([A-Za-z0-9._\s-]+)"
		Private Const STRINGFIELD_VALUE_PATTERN_APPLICATION As String = "{0}: ([A-Za-z0-9\s-]+)"
		Private Const STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION As String = "{0}: ([A-Za-zÖÄÜöäüéà'.0-9\s-_]+)"  ' ([A-Za-zÖÄÜöäüéà.0-9\s-_]+)
		Private Const STRINGENGLISHFIELD_VALUE_PATTERN_APPLICATION As String = "{0}: ([A-Za-z-\s-]+)"
		Private Const EMAIL_VALUE_PATTERN_APPLICATION As String = "{0}: ([A-Za-z0-9@._\s-]+)"
		Private Const DATEFIELD_VALUE_PATTERN_APPLICATION As String = "{0}: ([0-9.\s-]+)"
		Private Const TELEPHONFIELD_VALUE_PATTERN_APPLICATION As String = "{0}: ([0-9-\s-+()]+)"
		Private Const INTEGERFIELD_VALUE_PATTERN_APPLICATION As String = "{0}: ([0-9\-]+)"

		' "{0}: ([A-Za-z0-9@._\s-]+)" ' 
#End Region


#Region "Private Fields"

		Private m_CurrentApplicantID As Integer?
		Private m_CurrentApplicationID As Integer?

#End Region


#Region "Private Properties"

		Private ReadOnly Property GetCustomerIDValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.CustomerID)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "CustomerID"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetFirstnameValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Firstname)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Firstname"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim
				Else

					result.FieldName = "not defined!"

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetLastnameValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Lastname)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Lastname"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim
				Else

					result.FieldName = "not defined!"
				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetStreetValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Street)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Street"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetLocationValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Location)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Location"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetGenderValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGENGLISHFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Gender)

				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Gender"

					Dim key As String = m.Groups(1).Value
					If key.Trim.ToLower = "Frau".ToLower Then result.FieldValue = "f" Else result.FieldValue = "m"


				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetPostofficeBoxValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.PostofficeBox)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "PostofficeBox"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetPostcodeValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Postcode)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Postcode"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetCountryValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Country)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Country"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

					If String.IsNullOrWhiteSpace(key) Then key = "not defined!"
					result.FieldValue = key
				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetNationalityValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Nationality)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Nationality"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetEMailValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(EMAIL_VALUE_PATTERN_APPLICATION, m_PatternData.EMail)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "EMail"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetTelephoneValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(TELEPHONFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Telephone)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Telephone"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetBirthdateValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(DATEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Birthdate)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Birthdate"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

					Dim birthDay As Date
					If Not Date.TryParse(result.FieldValue, birthDay) Then
						result.FieldValue = Nothing
					End If
					If Now.Year - birthDay.Year <= 10 OrElse Now.Year - birthDay.Year >= 90 Then
						birthDay = Nothing
					End If
					If birthDay = Nothing Then
						result.FieldValue = String.Empty
					Else
						result.FieldValue = birthDay
					End If

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetPermissionValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Permission)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Permission"

					Dim key As String = m.Groups(1).Value

					If key.ToLower.Contains("Schweizer") Then key = "S" Else key = key.Split("-")(0).Trim
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetProfessionValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Profession)
				'Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Profession)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Profession"

					Dim key As String = m.Groups(1).Value
					If key.ToLower.Contains("kein") OrElse key.ToLower = "nein" OrElse key.ToLower = "..." OrElse key.ToLower = "-" Then key = String.Empty

					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetOtherProfessionValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.OtherProfession)
				'Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.OtherProfession)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "OtherProfession"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "OtherProfession"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetMobilePhoneValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.MobilePhone)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "MobilePhone"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "MobilePhone"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAutoValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Auto)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Auto"

					Dim key As String = m.Groups(1).Value
					If key.ToLower = "nein" Then
						result.FieldValue = "0"
					Else
						result.FieldValue = "1"
					End If

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Auto"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	If value.ToLower = "nein" Then
				'		result.FieldValue = "0"
				'	Else
				'		result.FieldValue = "1"
				'	End If

				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetMotorcycleValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Motorcycle)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Motorcycle"

					Dim key As String = m.Groups(1).Value
					If key.ToLower = "nein" Then
						result.FieldValue = "0"
					Else
						result.FieldValue = "1"
					End If

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Motorcycle"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	If value.ToLower = "nein" Then
				'		result.FieldValue = "0"
				'	Else
				'		result.FieldValue = "1"
				'	End If
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetBicycleValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Bicycle)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Bicycle"

					Dim key As String = m.Groups(1).Value
					If key.ToLower = "nein" Then
						result.FieldValue = "0"
					Else
						result.FieldValue = "1"
					End If

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Bicycle"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	If value.ToLower = "nein" Then
				'		result.FieldValue = "0"
				'	Else
				'		result.FieldValue = "1"
				'	End If
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetDrivingLicence1Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.DrivingLicence1)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "DrivingLicence1"

					Dim key As String = m.Groups(1).Value
					If key.ToLower.Contains("kein") OrElse key.ToLower = "-" Then key = String.Empty

					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "DrivingLicence1"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	If value.ToLower.Contains("kein") OrElse value.ToLower = "-" Then value = String.Empty
				'	result.FieldValue = value
				'End If


				Return result
			End Get
		End Property

		Private ReadOnly Property GetDrivingLicence2Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.DrivingLicence2)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "DrivingLicence2"

					Dim key As String = m.Groups(1).Value
					If key.ToLower.Contains("kein") OrElse key.ToLower = "-" Then key = String.Empty

					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "DrivingLicence2"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	If value.ToLower.Contains("kein") OrElse value.ToLower = "-" Then value = String.Empty
				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetDrivingLicence3Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.DrivingLicence3)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "DrivingLicence3"

					Dim key As String = m.Groups(1).Value
					If key.ToLower.Contains("kein") OrElse key.ToLower = "-" Then key = String.Empty

					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "DrivingLicence3"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	If value.ToLower.Contains("kein") OrElse value.ToLower = "-" Then value = String.Empty
				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetCivilStateValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.CivilState)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "CivilState"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "CivilState"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetLanguageValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Language)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Language"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Language"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetLanguageLevelValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.LanguageLevel)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "LanguageLevel"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "LanguageLevel"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetVacancyCustomerIDValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim vacancyCustomerID As String = String.Empty
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.VacancyCustomerID)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "VacancyCustomerID"

					vacancyCustomerID = m.Groups(1).Value
					vacancyCustomerID = vacancyCustomerID.Trim

				End If
				result.FieldValue = vacancyCustomerID

				Return result
			End Get
		End Property

		Private ReadOnly Property GetVacancyNumberValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim vacancyNumber As String = String.Empty
				Dim pattern = String.Format(INTEGERFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.VacancyNumber)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "VacancyNumber"

					vacancyNumber = m.Groups(1).Value
					vacancyNumber = vacancyNumber.Trim

				End If
				result.FieldValue = vacancyNumber

				Return result
			End Get
		End Property

		Private ReadOnly Property GetApplicationLabelValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGNAMEFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.ApplicationLabel)
				'Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.ApplicationLabel)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "ApplicationLabel"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAdvisorValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Advisor)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Advisor"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Advisor"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetDismissalperiodValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Dismissalperiod)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Dismissalperiod"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Dismissalperiod"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetBusinessBranchValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.BusinessBranch)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "BusinessBranch"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "BusinessBranch"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAvailabilityValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(STRINGFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Availability)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Availability"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Availability"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetCommentValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				'Dim pattern = String.Format(STRINGCOMMENTFIELD_VALUE_PATTERN_APPLICATION, m_PatternData.Comment)
				'Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				'If m.Success Then
				'	result.FieldName = "Comment"

				'	Dim key As String = m.Groups(1).Value
				'	result.FieldValue = key.Trim

				'End If

				Dim stringToParse = myHtmlString.ToString().Split(":"c)
				Dim value As String
				If stringToParse.Length > 1 Then
					value = myHtmlString.ToString.Replace(String.Format("{0}:", stringToParse(0)), "").Trim
				Else
					value = myHtmlString.ToString
				End If
				result.FieldName = "Comment"
				result.FieldValue = value.Trim

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Comment"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAttachmentCVValue(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(FILE_VALUE_PATTERN_APPLICATION, m_PatternData.Attachment_CV)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "AttachmentCV"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "AttachmentCV"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAttachment1Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(FILE_VALUE_PATTERN_APPLICATION, m_PatternData.Attachment_1)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Attachment1"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Attachment1"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAttachment2Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(FILE_VALUE_PATTERN_APPLICATION, m_PatternData.Attachment_2)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Attachment2"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Attachment2"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAttachment3Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(FILE_VALUE_PATTERN_APPLICATION, m_PatternData.Attachment_3)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Attachment3"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Attachment3"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAttachment4Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(FILE_VALUE_PATTERN_APPLICATION, m_PatternData.Attachment_4)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Attachment4"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Attachment4"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

		Private ReadOnly Property GetAttachment5Value(ByVal myHtmlString) As ParsedStringData
			Get
				Dim result As New ParsedStringData
				Dim pattern = String.Format(FILE_VALUE_PATTERN_APPLICATION, m_PatternData.Attachment_5)
				Dim m As Match = Regex.Match(myHtmlString, pattern, RegexOptions.IgnoreCase)
				If m.Success Then
					result.FieldName = "Attachment5"

					Dim key As String = m.Groups(1).Value
					result.FieldValue = key.Trim

				End If

				'Dim splitTRArray = Regex.Split(myHtmlString, pattern).Where(Function(s) s.Trim <> "")
				'If splitTRArray.Count = 1 AndAlso splitTRArray(0).Trim <> "&nbsp;" Then
				'	result.FieldName = "Attachment5"

				'	Dim value = ConvertHtmlToPlainText(Trim(splitTRArray(0).ToString))
				'	If value.Contains(vbNewLine) Then
				'		value = value.Replace(vbNewLine, "#").Split("#")(1)
				'	End If

				'	result.FieldValue = value
				'End If

				Return result
			End Get
		End Property

#End Region


#Region "Public methodes"

		Public Function ParsReceivedEMailBody(ByVal email As EMailData) As ParsResult
			Dim result As ParsResult = Nothing
			Dim eMailBody As String
			Dim parsedString As New Dictionary(Of String, String)


			parsedString = LoadPattern()
			If email Is Nothing Then
				m_Logger.LogError("not mail was selected!")
				Return Nothing
			End If
			m_customerID = CurrentEMailData.Customer_ID
			eMailBody = email.EMailBody
			Dim parsedData As New ParsedStringData
			m_Logger.LogInfo("body is now will be parsed...")

			Try

				Dim plainText As String
				If email.HasHtmlBody Then
					m_Logger.LogDebug("converting html to plain text!")
					plainText = ConvertHtmlToPlainText(Trim(eMailBody))
				Else
					plainText = email.EMailPlainTextBody
				End If
				Dim eMailLines = plainText.Split(New [Char]() {CChar(vbCrLf)}, StringSplitOptions.RemoveEmptyEntries)

				For Each line In eMailLines
					line = line.ToString.Replace(vbLf, "").Trim
					Trace.WriteLine(String.Format("{0}", line))

					If LineStartsWithFieldName(line) Then ' line.ToString.Split(": ").Length > 1 Then

						Try
							Dim viewData = FindParseStructure(line)
							If Not viewData Is Nothing AndAlso Not viewData.FieldName Is Nothing Then
								Dim fieldName As String = viewData.FieldName
								Dim fieldValue As String = viewData.FieldValue

								parsedString.Item(viewData.FieldName) = viewData.FieldValue
								Trace.WriteLine(String.Format("FieldName: {0} | FieldValue: {1}", fieldName, fieldValue))

								parsedData = viewData

							Else
								Trace.WriteLine(String.Format("FieldName not founded: {0} ", line))
							End If

						Catch ex As Exception

						End Try

					ElseIf Not String.IsNullOrWhiteSpace(line) AndAlso Not parsedData.FieldName Is Nothing Then
						parsedString.Item(parsedData.FieldName) = String.Format("{0} {1}", parsedString.Item(parsedData.FieldName), line)
					End If
				Next


				If String.IsNullOrWhiteSpace(parsedString.Item("CustomerID")) Then parsedString.Item("CustomerID") = m_customerID
				If String.IsNullOrWhiteSpace(parsedString.Item("VacancyCustomerID")) Then parsedString.Item("VacancyCustomerID") = m_customerID

				result = New ParsResult
				Dim applicantFullName As String = String.Format("{0} {1}", parsedString.Item("Firstname"), parsedString.Item("Lastname"))
				Dim birthDayISOK As Boolean = Not String.IsNullOrWhiteSpace(parsedString.Item("Birthdate"))
				Dim eMailISOK As Boolean = Not String.IsNullOrWhiteSpace(parsedString.Item("EMail"))
				Dim vacancyNumber As Integer = 0

				Try
					Dim subject = email.EMailSubject

					vacancyNumber = CType(Val(parsedString.Item("VacancyNumber")), Integer)
					If vacancyNumber = 0 AndAlso subject.ToString.Contains("Bewerbung zu Stellennr. ") Then
						vacancyNumber = Regex.Replace(subject, "\D", "")
					End If
					If vacancyNumber > 0 AndAlso String.IsNullOrWhiteSpace(parsedString.Item("Advisor")) Then
						parsedString.Item("VacancyNumber") = String.Format("{0}", vacancyNumber)
						Dim vacancyData = PerformVacancyDetailsWebserviceCall(m_customerID, vacancyNumber)
						If Not vacancyData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(vacancyData.Customer_ID) Then
							m_Logger.LogInfo("vacancy was founded.")
							parsedString.Item("Advisor") = vacancyData.UserGuid
							If String.IsNullOrWhiteSpace(parsedString.Item("ApplicationLabel")) Then parsedString.Item("ApplicationLabel") = vacancyData.VacancyTitle
						Else
							m_Logger.LogInfo("vacancy was not founded.")
						End If
					End If
					If String.IsNullOrWhiteSpace(parsedString.Item("ApplicationLabel")) Then parsedString.Item("ApplicationLabel") = subject.Replace("WG: ", String.Empty).Replace("Bewerbung als ", String.Empty)
					If String.IsNullOrWhiteSpace(parsedString.Item("Advisor")) Then
						m_Logger.LogInfo(String.Format("advisor is as mail sender: {0}", AssignedUserID))
						parsedString.Item("Advisor") = AssignedUserID
					End If


				Catch ex As Exception
					m_Logger.LogError(String.Format("error during parsing vacancydata and applicationlabel: {0}", ex.ToString))

				End Try

				If applicantFullName.Length > 5 AndAlso (birthDayISOK OrElse eMailISOK) Then
					result.ParseValue = SaveParsedEMail(parsedString)

				Else
					Dim success = SaveNotParseableEMail(parsedString)
					result.ParseValue = False
					result.ParseMessage = "Not defined"
				End If

				result.ApplicantID = m_CurrentApplicantID
				result.ApplicationID = m_CurrentApplicationID
				m_Logger.LogInfo(String.Format("body is now parsed with ApplicationID: {0} and ApplicantID: {1}", m_CurrentApplicationID, m_CurrentApplicantID))


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try


			Return result

		End Function

		Public Function ParsReceivedDropIn(ByVal customerID As String) As ParsResult
			Dim result As ParsResult = Nothing
			Dim parsedString As New Dictionary(Of String, String)

			parsedString = LoadPattern()
			If parsedString Is Nothing Then
				m_Logger.LogError("parsing terminated!")
				Return Nothing
			End If
			m_customerID = customerID

			Try
				If parsedString.Item("CustomerID") = String.Empty Then parsedString.Item("CustomerID") = m_customerID

				result = New ParsResult
				Dim applicantFullName As String = String.Format("{0}{1}", parsedString.Item("Firstname"), parsedString.Item("Lastname"))
				Dim birthDayISOK As Boolean = Not String.IsNullOrWhiteSpace(parsedString.Item("Birthdate"))
				Dim eMailISOK As Boolean = Not String.IsNullOrWhiteSpace(parsedString.Item("EMail"))

				Dim success = SaveNotParseableEMail(parsedString)
				result.ParseValue = False
				result.ParseMessage = "Not defined"

				result.ApplicantID = m_CurrentApplicantID
				result.ApplicationID = m_CurrentApplicationID


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try


			Return result

		End Function


#End Region


#Region "private methodes"

		Private Function FindParseStructure(ByVal myString As String) As ParsedStringData
			Dim result As New ParsedStringData

			Select Case True
				Case myString.Trim.Contains(m_PatternData.CustomerID) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.CustomerID) AndAlso myString.Trim.StartsWith(m_PatternData.CustomerID)
					result = GetCustomerIDValue(myString)
				Case myString.Trim.Contains(m_PatternData.Firstname) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Firstname) AndAlso myString.Trim.StartsWith(m_PatternData.Firstname)
					result = GetFirstnameValue(myString)
				Case myString.Trim.Contains(m_PatternData.Lastname) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Lastname) AndAlso myString.Trim.StartsWith(m_PatternData.Lastname)
					result = GetLastnameValue(myString)
				Case myString.Trim.Contains(m_PatternData.Street) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Street) AndAlso myString.Trim.StartsWith(m_PatternData.Street)
					result = GetStreetValue(myString)
				Case myString.Trim.Contains(m_PatternData.Location) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Location) AndAlso myString.Trim.StartsWith(m_PatternData.Location)
					result = GetLocationValue(myString)
				Case myString.Trim.Contains(m_PatternData.Gender) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Gender) AndAlso myString.Trim.StartsWith(m_PatternData.Gender)
					result = GetGenderValue(myString)
				Case myString.Trim.Contains(m_PatternData.PostofficeBox) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.PostofficeBox) AndAlso myString.Trim.StartsWith(m_PatternData.PostofficeBox)
					result = GetPostofficeBoxValue(myString)
				Case myString.Trim.Contains(m_PatternData.Postcode) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Postcode) AndAlso myString.Trim.StartsWith(m_PatternData.Postcode)
					result = GetPostcodeValue(myString)
				Case myString.Trim.Contains(m_PatternData.Country) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Country) AndAlso myString.Trim.StartsWith(m_PatternData.Country)
					result = GetCountryValue(myString)
				Case myString.Trim.Contains(m_PatternData.Nationality) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Nationality) AndAlso myString.Trim.StartsWith(m_PatternData.Nationality)
					result = GetNationalityValue(myString)
				Case myString.Trim.Contains(m_PatternData.EMail) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.EMail) AndAlso myString.Trim.StartsWith(m_PatternData.EMail)
					result = GetEMailValue(myString)
				Case myString.Trim.Contains(m_PatternData.Telephone) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Telephone) AndAlso myString.Trim.StartsWith(m_PatternData.Telephone)
					result = GetTelephoneValue(myString)
				Case myString.Trim.Contains(m_PatternData.Birthdate) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Birthdate) AndAlso myString.Trim.StartsWith(m_PatternData.Birthdate)
					result = GetBirthdateValue(myString)
				Case myString.Trim.Contains(m_PatternData.Permission) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Permission) AndAlso myString.Trim.StartsWith(m_PatternData.Permission)
					result = GetPermissionValue(myString)
				Case myString.Trim.Contains(m_PatternData.Profession) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Profession) AndAlso myString.Trim.StartsWith(m_PatternData.Profession)
					result = GetProfessionValue(myString)
				Case myString.Trim.Contains(m_PatternData.OtherProfession) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.OtherProfession) AndAlso myString.Trim.StartsWith(m_PatternData.OtherProfession)
					result = GetOtherProfessionValue(myString)
				Case myString.Trim.Contains(m_PatternData.MobilePhone) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.MobilePhone) AndAlso myString.Trim.StartsWith(m_PatternData.MobilePhone)
					result = GetMobilePhoneValue(myString)
				Case myString.Trim.Contains(m_PatternData.Auto) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Auto) AndAlso myString.Trim.StartsWith(m_PatternData.Auto)
					result = GetAutoValue(myString)
				Case myString.Trim.Contains(m_PatternData.Motorcycle) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Motorcycle) AndAlso myString.Trim.StartsWith(m_PatternData.Motorcycle)
					result = GetMotorcycleValue(myString)
				Case myString.Trim.Contains(m_PatternData.Bicycle) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Bicycle) AndAlso myString.Trim.StartsWith(m_PatternData.Bicycle)
					result = GetBicycleValue(myString)
				Case myString.Trim.Contains(m_PatternData.DrivingLicence1) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.DrivingLicence1) AndAlso myString.Trim.StartsWith(m_PatternData.DrivingLicence1)
					result = GetDrivingLicence1Value(myString)
				Case myString.Trim.Contains(m_PatternData.DrivingLicence2) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.DrivingLicence2) AndAlso myString.Trim.StartsWith(m_PatternData.DrivingLicence2)
					result = GetDrivingLicence2Value(myString)
				Case myString.Trim.Contains(m_PatternData.DrivingLicence3) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.DrivingLicence3) AndAlso myString.Trim.StartsWith(m_PatternData.DrivingLicence3)
					result = GetDrivingLicence3Value(myString)
				Case myString.Trim.Contains(m_PatternData.CivilState) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.CivilState) AndAlso myString.Trim.StartsWith(m_PatternData.CivilState)
					result = GetCivilStateValue(myString)
				Case myString.Trim.Contains(m_PatternData.Language) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Language) AndAlso myString.Trim.StartsWith(m_PatternData.Language)
					result = GetLanguageValue(myString)
				Case myString.Trim.Contains(m_PatternData.LanguageLevel) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.LanguageLevel) AndAlso myString.Trim.StartsWith(m_PatternData.LanguageLevel)
					result = GetLanguageLevelValue(myString)
				Case myString.Trim.Contains(m_PatternData.VacancyCustomerID) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.VacancyCustomerID) AndAlso myString.Trim.StartsWith(m_PatternData.VacancyCustomerID)
					result = GetVacancyCustomerIDValue(myString)

				Case myString.Trim.Contains(m_PatternData.VacancyNumber) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.VacancyNumber) AndAlso myString.Trim.StartsWith(m_PatternData.VacancyNumber)
					result = GetVacancyNumberValue(myString)
				Case myString.Trim.Contains(m_PatternData.ApplicationLabel) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.ApplicationLabel) AndAlso myString.Trim.StartsWith(m_PatternData.ApplicationLabel)
					result = GetApplicationLabelValue(myString)
				Case myString.Trim.Contains(m_PatternData.Advisor) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Advisor) AndAlso myString.Trim.StartsWith(m_PatternData.Advisor)
					result = GetAdvisorValue(myString)
				Case myString.Trim.Contains(m_PatternData.BusinessBranch) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.BusinessBranch) AndAlso myString.Trim.StartsWith(m_PatternData.BusinessBranch)
					result = GetBusinessBranchValue(myString)
				Case myString.Trim.Contains(m_PatternData.Dismissalperiod) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Dismissalperiod) AndAlso myString.Trim.StartsWith(m_PatternData.Dismissalperiod)
					result = GetDismissalperiodValue(myString)
				Case myString.Trim.Contains(m_PatternData.Availability) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Availability) AndAlso myString.Trim.StartsWith(m_PatternData.Availability)
					result = GetAvailabilityValue(myString)

				Case myString.Trim.Contains(m_PatternData.Attachment_CV) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_CV) AndAlso myString.Trim.StartsWith(m_PatternData.Attachment_CV)
					result = GetAttachmentCVValue(myString)
				Case myString.Trim.Contains(m_PatternData.Attachment_1) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_1) AndAlso myString.Trim.StartsWith(m_PatternData.Attachment_1)
					result = GetAttachment1Value(myString)
				Case myString.Trim.Contains(m_PatternData.Attachment_2) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_2) AndAlso myString.Trim.StartsWith(m_PatternData.Attachment_2)
					result = GetAttachment2Value(myString)
				Case myString.Trim.Contains(m_PatternData.Attachment_3) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_3) AndAlso myString.Trim.StartsWith(m_PatternData.Attachment_3)
					result = GetAttachment3Value(myString)
				Case myString.Trim.Contains(m_PatternData.Attachment_4) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_4) AndAlso myString.Trim.StartsWith(m_PatternData.Attachment_4)
					result = GetAttachment4Value(myString)
				Case myString.Trim.Contains(m_PatternData.Attachment_5) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_5) AndAlso myString.Trim.StartsWith(m_PatternData.Attachment_5)
					result = GetAttachment5Value(myString)

				Case myString.Trim.Contains(m_PatternData.Comment) AndAlso Not String.IsNullOrWhiteSpace(m_PatternData.Comment) AndAlso myString.Trim.StartsWith(m_PatternData.Comment)
					result = GetCommentValue(myString)


				Case Else
					Return Nothing

			End Select


			Return result

		End Function

		Private Function LineStartsWithFieldName(ByVal myString As String) As Boolean
			Dim result As Boolean = True

			Select Case True
				Case Not String.IsNullOrWhiteSpace(m_PatternData.CustomerID) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.CustomerID))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.VacancyCustomerID) AndAlso myString.Trim.Contains(String.Format("{0}:", m_PatternData.VacancyCustomerID))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Firstname) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Firstname))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Lastname) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Lastname))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Street) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Street))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Location) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Location))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Gender) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Gender))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.PostofficeBox) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.PostofficeBox))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Postcode) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Postcode))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Country) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Country))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Nationality) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Nationality))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.EMail) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.EMail))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Telephone) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Telephone))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Birthdate) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Birthdate))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Permission) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Permission))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Profession) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Profession))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.OtherProfession) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.OtherProfession))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.MobilePhone) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.MobilePhone))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Auto) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Auto))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Motorcycle) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Motorcycle))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Bicycle) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Bicycle))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.DrivingLicence1) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.DrivingLicence1))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.DrivingLicence2) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.DrivingLicence2))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.DrivingLicence3) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.DrivingLicence3))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.CivilState) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.CivilState))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Language) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Language))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.LanguageLevel) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.LanguageLevel))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.VacancyCustomerID) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.VacancyCustomerID))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.VacancyNumber) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.VacancyNumber))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.ApplicationLabel) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.ApplicationLabel))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Advisor) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Advisor))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.BusinessBranch) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.BusinessBranch))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Dismissalperiod) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Dismissalperiod))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Availability) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Availability))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_CV) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Attachment_CV))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_1) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Attachment_1))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_2) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Attachment_2))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_3) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Attachment_3))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_4) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Attachment_4))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Attachment_5) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Attachment_5))
				Case Not String.IsNullOrWhiteSpace(m_PatternData.Comment) AndAlso myString.Trim.StartsWith(String.Format("{0}:", m_PatternData.Comment))


				Case Else
					Return False

			End Select


			Return result

		End Function

		Private Function LoadPattern() As Dictionary(Of String, String)

			Dim parsedString As Dictionary(Of String, String) = Nothing
			parsedString = New Dictionary(Of String, String)

			parsedString.Add("CustomerID", "")
			parsedString.Add("Firstname", "")
			parsedString.Add("Lastname", "")
			parsedString.Add("Street", "")
			parsedString.Add("Location", "")
			parsedString.Add("Gender", "")
			parsedString.Add("PostofficeBox", "")
			parsedString.Add("Postcode", "")
			parsedString.Add("Country", "")
			parsedString.Add("Nationality", "")
			parsedString.Add("EMail", "")
			parsedString.Add("Telephone", "")
			parsedString.Add("Birthdate", "")
			parsedString.Add("Permission", "")
			parsedString.Add("Profession", "")
			parsedString.Add("MobilePhone", "")
			parsedString.Add("Auto", "0")
			parsedString.Add("Motorcycle", "0")
			parsedString.Add("Bicycle", "0")
			parsedString.Add("DrivingLicence1", "")
			parsedString.Add("DrivingLicence2", "")
			parsedString.Add("DrivingLicence3", "")
			parsedString.Add("CivilState", "0")
			parsedString.Add("Language", "")
			parsedString.Add("LanguageLevel", "0")
			parsedString.Add("VacancyCustomerID", "")
			parsedString.Add("VacancyNumber", "0")
			parsedString.Add("ApplicationLabel", "")
			parsedString.Add("Advisor", "")
			parsedString.Add("BusinessBranch", "")
			parsedString.Add("Dismissalperiod", "")
			parsedString.Add("Availability", "")
			parsedString.Add("Comment", "")

			parsedString.Add("Attachment_CV", "")
			parsedString.Add("Attachment1", "")
			parsedString.Add("Attachment2", "")
			parsedString.Add("Attachment3", "")
			parsedString.Add("Attachment4", "")
			parsedString.Add("Attachment5", "")


			Return parsedString

		End Function


		Private Function SaveParsedEMail(ByVal data As Dictionary(Of String, String)) As Boolean
			Dim success As Boolean = True

			Try

				Dim application = New ApplicationData
				Dim applicant = New ApplicantData

				application.Customer_ID = m_customerID ' If(String.IsNullOrWhiteSpace(data.Item("CustomerID")), m_customerID, data.Item("CustomerID"))
				application.VacancyNumber = CType(Val(data.Item("VacancyNumber")), Integer)
				application.ApplicationLabel = data.Item("ApplicationLabel")
				application.Advisor = data.Item("Advisor")
				application.Availability = data.Item("Availability")
				application.Dismissalperiod = data.Item("Dismissalperiod")
				application.BusinessBranch = data.Item("BusinessBranch")
				application.Comment = data.Item("Comment")

				applicant.Customer_ID = m_customerID ' If(String.IsNullOrWhiteSpace(data.Item("CustomerID")), m_customerID, data.Item("CustomerID"))
				applicant.Lastname = data.Item("Lastname")
				applicant.Firstname = data.Item("Firstname")
				applicant.Gender = data.Item("Gender") ' CType(Val(data.Item("Gender")), Integer)
				applicant.Street = data.Item("Street")
				applicant.PostOfficeBox = data.Item("PostofficeBox")
				applicant.Postcode = data.Item("Postcode")
				applicant.Location = data.Item("Location")
				applicant.Country = data.Item("Country")

				applicant.Nationality = data.Item("Nationality")
				applicant.EMail = data.Item("EMail")
				applicant.Telephone = data.Item("Telephone")
				applicant.MobilePhone = data.Item("MobilePhone")
				If Not String.IsNullOrWhiteSpace(data.Item("Birthdate")) Then
					applicant.Birthdate = ParseToDate(data.Item("Birthdate"), Nothing)
				End If
				applicant.Permission = data.Item("Permission")
				applicant.Profession = data.Item("Profession")
				applicant.Auto = m_Utility.ParseToBoolean(data.Item("Auto"), False)
				applicant.Motorcycle = m_Utility.ParseToBoolean(data.Item("Motorcycle"), False)
				applicant.Bicycle = m_Utility.ParseToBoolean(data.Item("Bicycle"), False)

				applicant.DrivingLicence1 = data.Item("DrivingLicence1")
				applicant.DrivingLicence2 = data.Item("DrivingLicence2")
				applicant.DrivingLicence3 = data.Item("DrivingLicence3")
				applicant.CivilState = CType(Val(data.Item("CivilState")), Integer)
				applicant.Language = data.Item("Language")
				applicant.LanguageLevel = CType(Val(data.Item("LanguageLevel")), Integer)

				Dim assignedApplicant = m_AppDatabaseAccess.LoadAssignedApplicantData(applicant.Customer_ID, applicant)
				If assignedApplicant Is Nothing OrElse assignedApplicant.Customer_ID Is Nothing Then
					success = success AndAlso m_AppDatabaseAccess.AddApplicationWithApplicant(application, applicant)
				Else
					applicant = assignedApplicant
					success = success AndAlso m_AppDatabaseAccess.AddApplication(application, applicant)
				End If

				If success AndAlso CurrentEMailAttachmentData.Count > 0 Then
					Dim resultAttachmentSave = success AndAlso SaveApplicantAttachments(data, application.ID, applicant.ID)
					If Not resultAttachmentSave Then
						m_Logger.LogError("attachment could not be saved!")
					End If
				End If

				m_CurrentApplicantID = applicant.ID
				m_CurrentApplicationID = application.ID


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try


			Return success

		End Function

		Private Function SaveNotParseableEMail(ByVal data As Dictionary(Of String, String)) As Boolean
			Dim success As Boolean = True

			Try

				Dim application = New ApplicationData
				Dim applicant = New ApplicantData

				application.Customer_ID = If(String.IsNullOrWhiteSpace(data.Item("CustomerID")), m_customerID, data.Item("CustomerID"))
				application.VacancyNumber = CType(Val(data.Item("VacancyNumber")), Integer)
				application.ApplicationLabel = data.Item("ApplicationLabel")
				application.Advisor = data.Item("Advisor")
				application.Availability = data.Item("Availability")
				application.Dismissalperiod = data.Item("Dismissalperiod")
				application.BusinessBranch = data.Item("BusinessBranch")
				application.Comment = data.Item("Comment")

				applicant.Customer_ID = If(String.IsNullOrWhiteSpace(data.Item("CustomerID")), m_customerID, data.Item("CustomerID"))
				applicant.Lastname = data.Item("Lastname")
				applicant.Firstname = data.Item("Firstname")
				applicant.Gender = data.Item("Gender")
				applicant.Street = data.Item("Street")
				applicant.PostOfficeBox = data.Item("PostofficeBox")
				applicant.Postcode = data.Item("Postcode")
				applicant.Location = data.Item("Location")
				applicant.Country = data.Item("Country")

				applicant.Nationality = data.Item("Nationality")
				applicant.EMail = data.Item("EMail")
				applicant.Telephone = data.Item("Telephone")
				applicant.MobilePhone = data.Item("MobilePhone")
				If Not String.IsNullOrWhiteSpace(data.Item("Birthdate")) Then
					applicant.Birthdate = ParseToDate(data.Item("Birthdate"), Nothing)
				End If
				applicant.Permission = data.Item("Permission")
				applicant.Profession = data.Item("Profession")
				applicant.Auto = m_Utility.ParseToBoolean(data.Item("Auto"), False)
				applicant.Motorcycle = m_Utility.ParseToBoolean(data.Item("Motorcycle"), False)
				applicant.Bicycle = m_Utility.ParseToBoolean(data.Item("Bicycle"), False)

				applicant.DrivingLicence1 = data.Item("DrivingLicence1")
				applicant.DrivingLicence2 = data.Item("DrivingLicence2")
				applicant.DrivingLicence3 = data.Item("DrivingLicence3")
				applicant.CivilState = CType(Val(data.Item("CivilState")), Integer)
				applicant.Language = data.Item("Language")
				applicant.LanguageLevel = CType(Val(data.Item("LanguageLevel")), Integer)


				success = success AndAlso m_AppDatabaseAccess.AddApplicationWithApplicant(application, applicant)

				m_CurrentApplicantID = applicant.ID
				m_CurrentApplicationID = application.ID
				If Not CurrentEMailAttachmentData Is Nothing Then
					If success AndAlso CurrentEMailAttachmentData.Count > 0 Then success = success AndAlso SaveApplicantAttachments(data, application.ID, applicant.ID)
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try


			Return success

		End Function

		Private Function SaveApplicantAttachments(ByVal data As Dictionary(Of String, String), ByVal applicationID As Integer?, ByVal applicantID As Integer?) As Boolean
			Dim success As Boolean = True
			Dim document As ApplicantDocumentData

			Try
				If applicantID = 476 Or applicantID = 512 Then
					Trace.WriteLine("")
				End If

				For Each attachment In CurrentEMailAttachmentData
					document = New ApplicantDocumentData

					If attachment.AttachmentName.ToLower.EndsWith(".bat") AndAlso attachment.AttachmentName.ToLower.EndsWith(".exe") AndAlso attachment.AttachmentName.ToLower.EndsWith(".dll") Then Continue For
					Dim bytes() = attachment.AttachmentSize
					Dim s As New SHA256Managed
					Dim hash() As Byte = s.ComputeHash(bytes)
					Dim hashValue = Convert.ToBase64String(hash)

					Dim myFileExtension = GetMimeFromBytes(bytes)

					Dim extension = (From kp As KeyValuePair(Of String, String) In MIMETypesDictionary
									 Where kp.Value = myFileExtension
									 Select kp.Key).ToList()

					document.FK_ApplicantID = applicantID
					document.Content = attachment.AttachmentSize
					document.HashValue = hashValue
					document.FileExtension = Path.GetExtension(attachment.AttachmentName)
					document.CreatedFrom = "System"
					document.CreatedOn = Now
					document.Flag = 0

					If data.Item("Attachment_CV") = attachment.AttachmentName OrElse attachment.AttachmentName.ToLower.Contains("lebenslauf") OrElse
						attachment.AttachmentName.ToLower.Contains("cv ") OrElse attachment.AttachmentName.ToLower.Contains("cv_") OrElse attachment.AttachmentName.ToLower.Contains("resume") OrElse
						attachment.AttachmentName.ToLower.Contains("curriculum") Then
						document.Title = String.Format("{0} | {1}", m_PatternData.Attachment_CV, attachment.AttachmentName)
						document.Type = 201
					ElseIf data.Item("Attachment1") = attachment.AttachmentName Then
						document.Title = String.Format("{0} |{1}", m_PatternData.Attachment_1, attachment.AttachmentName)
						document.Type = 202
					ElseIf data.Item("Attachment2") = attachment.AttachmentName Then
						document.Title = String.Format("{0} |{1}", m_PatternData.Attachment_2, attachment.AttachmentName)
						document.Type = 202
					ElseIf data.Item("Attachment3") = attachment.AttachmentName Then
						document.Title = String.Format("{0} |{1}", m_PatternData.Attachment_3, attachment.AttachmentName)
						document.Type = 202
					ElseIf data.Item("Attachment4") = attachment.AttachmentName Then
						document.Title = String.Format("{0} |{1}", m_PatternData.Attachment_4, attachment.AttachmentName)
						document.Type = 202
					ElseIf data.Item("Attachment5") = attachment.AttachmentName Then
						document.Title = String.Format("{0} |{1}", m_PatternData.Attachment_5, attachment.AttachmentName)
						document.Type = 202
					Else
						document.Title = String.Format("{0}", Path.GetFileNameWithoutExtension(attachment.AttachmentName))

					End If

					Dim existsAppDocument = m_AppDatabaseAccess.ExistsApplicantDocumentWithHashData(data.Item("CustomerID"), document.HashValue)
					If Not existsAppDocument Is Nothing AndAlso existsAppDocument.FK_ApplicantID > 0 Then
						m_Logger.LogWarning(String.Format("same document was founded with FK_ApplicantID: {0}", existsAppDocument.FK_ApplicantID))

					Else
						success = success AndAlso m_AppDatabaseAccess.AddApplicantDocument(document)

					End If

				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try


			Return success

		End Function


#End Region


#Region "helpers"

		''' <summary>
		'''  Performs vacancy detail.
		''' </summary>
		''' <returns>The vacancy response.</returns>
		Private Function PerformVacancyDetailsWebserviceCall(ByVal customer_ID As String, ByVal vacancyNumber As Integer) As VacancyViewData

			Dim listDataSource As VacancyViewData = New VacancyViewData
			If String.IsNullOrWhiteSpace(customer_ID) Then customer_ID = m_customerID
			If vacancyNumber = 0 Then Return listDataSource

			Dim webservice As New ExternalVacancyService.SPVacancyServicesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyExternalUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedVacancyWithCustomerID(customer_ID, vacancyNumber).ToList

			For Each result In searchResult

				Dim viewData = New VacancyViewData With {
					.ID = result.RecID,
					.VacancyNumber = result.VakNr,
					.Customer_ID = result.Customer_ID,
					.VacancyTitle = result.Bezeichnung,
					.UserGuid = result.User_Guid
				}

				listDataSource = viewData
			Next


			Return listDataSource

		End Function

		Private Function ParseToDate(ByVal stringvalue As String, ByVal value As Date?) As Date
			Dim result As Date
			If (Not Date.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function


#End Region




		Private Class ParsedStringData
			Public Property FieldName As String
			Public Property FieldValue As String

		End Class


		Private Class VacancyViewData

			Public Property ID As Integer
			Public Property VacancyNumber As Integer
			Public Property Customer_ID As String
			Public Property VacancyTitle As String
			Public Property UserGuid As String

		End Class


	End Class

	Public Class ParsResult
		Public Property ParseValue As Boolean
		Public Property ParseMessage As String

		Public Property ApplicantID As Integer?
		Public Property ApplicationID As Integer?

	End Class




End Namespace
