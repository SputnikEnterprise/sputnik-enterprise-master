
Imports SPProgUtility

Public Class ClsSetting

	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


'Public Class MandantenData

'	Public Property MDNr As Integer
'	Public Property MDName As String
'	Public Property MDGuid As String
'	Public Property MDConnStr As String
'	Public Property MultiMD As Short

'End Class

Public Class CodeData
	Public Property Code As String
	Public Property CodeLabel As String

	Public ReadOnly Property CodeViewData As String
		Get
			Return String.Format("{0}: {1}", Code, CodeLabel)
		End Get
	End Property

End Class

Public Class FoundedData

	Public Property MANr As Integer?

	Public Property vonmonat As Integer?
	Public Property bismonat As Integer?
	Public Property jahr As Integer?
	Public Property gebdat As DateTime?

	Public Property ahv_nr As String
	Public Property PermissionCode As String
	Public Property CivilStatus As String
	Public Property CivilState As String
	Public Property ahv_nr_new As String
	Public Property s_kanton As String
	Public Property qstgemeinde As String
	Public Property TaxCommunityLabel As String
	Public Property TaxCommunityCode As Integer?

	Public Property EmploymentType As String
	Public Property OtherEmploymentType As String
	Public Property TypeofStay As String
	Public Property ForeignCategory As String
	Public Property SocialInsuranceNumber As String
	Public Property NumberOfChildren As Integer

	Public Property TaxChurchCode As String
	Public Property PartnerLastName As String
	Public Property PartnerFirstname As String
	Public Property PartnerStreet As String
	Public Property PartnerPostcode As String
	Public Property PartnerCity As String
	Public Property PartnerCountry As String
	Public Property InEmployment As Boolean
	Public Property EmploymentLocation As String
	Public Property EmploymentPostcode As String
	Public Property EmploymentStreet As String
	Public Property EmploymentCommunityCode As Integer

	Public Property geschlecht As String

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeepostcode As String
	Public Property employeecity As String
	Public Property employeecountry As String

	Public Property monat As Integer?
	Public Property kinder As Integer?
	Public Property employeelanguage As String
	Public Property m_anz As Decimal?
	Public Property m_bas As Decimal?
	Public Property m_ans As Decimal?
	Public Property m_btr As Decimal?
	Public Property Bruttolohn As Decimal?
	Public Property qstbasis As Decimal?

	Public Property Exception_KTGBetrag_Amount As Decimal?
	Public Property Exception_SuvaBetrag_Amount As Decimal?
	Public Property Exception_KiAuBetrag_Amount As Decimal?
	Public Property Exception_OtherServices_Amount As Decimal?
	Public Property Exception_OtherNotDefined_Amount As Decimal?
	Public Property Exception_OtherServicesLAData_Amount As Decimal?
	Public Property Exception_OtherNotDefinedLAData_Amount As String
	Public Property Exception_SPayed_Amount As Decimal?
	Public Property Exception_SBacked_Amount As Decimal?

	Public Property stdanz As Decimal?

	Public Property WorkingHoursWeek As Decimal?
	Public Property WorkingHoursMonth As Decimal?
	Public Property ESLohnNumber As Integer?
	Public Property EmploymentNumber As Integer?
	Public Property ReportNumber As Integer?
	Public Property WorkingPensum As String
	Public Property GAVStringInfo As String
	Public Property Dismissalreason As String

	Public Property tarifcode As String
	Public Property workeddays As Integer?

	Public Property esab As DateTime?
	Public Property esende As DateTime?

	Public Property EmployeePartnerRecID As Integer?
	Public Property EmployeeLOHistoryID As Integer?

	Public ReadOnly Property EmployeeAddress As String
		Get
			Return (String.Format("{0}, {1}-{2} {3}", employeestreet, employeecountry, employeepostcode, employeecity))
		End Get
	End Property
	Public ReadOnly Property VacationDaysCount As Integer
		Get
			Dim result As Integer = 0
			Dim value As List(Of String) = GAVStringInfo.Split("¦").ToList()
			Dim vacationString = value.Where(Function(x As String) x.Contains("FerienJahr:")).FirstOrDefault
			If vacationString Is Nothing Then Return result

			result = Convert.ToInt32(vacationString.Split(":")(1))

			Return result
		End Get
	End Property

	Public ReadOnly Property TypeofStayViewData As String
		Get
			Select Case EmploymentType
				Case "H1"
					Return "DAILY"
				Case "H2"
					Return "WEEKLY"

				Case Else
					Return "CH"
			End Select

		End Get
	End Property

	Public ReadOnly Property EmploymentTypeViewData As String
		Get
			Select Case OtherEmploymentType
				Case "01"
					Return "MAIN_JOB"
				Case "02"
					Return "SIDE_JOB"

				Case Else
					Return String.Empty
			End Select

		End Get
	End Property

	Public ReadOnly Property TaxCivilStatusCode As String
		Get

			Select Case CivilState
				Case "L"
					Return "SINGLE"
				Case "V"
					Return "MARRIED"
				Case "W"
					Return "WIDOWED"
				Case "G"
					Return "DIVORCED"
				Case "T"
					Return "SEPARATED"
				Case "P"
					Return "REGISTERED_PARTNERSHIP"

				Case "D"
					Return "PARTNERSHIP_DISSOLVED_BY_LAW"
				Case "N", "S"
					Return "PARTNERSHIP_DISSOLVED_BY_DEATH"

				Case Else
					Return String.Empty
			End Select

		End Get
	End Property

	Public ReadOnly Property TaxChurchCodeViewData As String
		Get
			If Not String.IsNullOrWhiteSpace(TaxChurchCode) Then
				If TaxChurchCode.ToUpper.EndsWith("Y") Then
					Return "yes"
				Else
					Return "no"
				End If

			Else
				Return String.Empty
			End If

		End Get
	End Property

End Class


Public Class SortData
	Public Property SortNumber As Integer
	Public Property SortBez As String

End Class


Public Class XMLGECommunityData
	Public Property CommunityNumber As Integer

End Class


Public Class ChildData

	Public Property MANr As Integer
	Public Property Nachname As String
	Public Property Vorname As String
	Public Property GebDate As Date?

	Public ReadOnly Property ChildName As String
		Get
			Return String.Format("{0} {1}", Vorname, Nachname)
		End Get
	End Property

	Public ReadOnly Property childJahrgang As Integer
		Get
			If GebDate.HasValue Then
				Return Now.Year - Year(GebDate.GetValueOrDefault(Date.MinValue))
			Else
				Return 0
			End If
		End Get
	End Property


End Class



