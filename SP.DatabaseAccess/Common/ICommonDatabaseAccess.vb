Imports SP.DatabaseAccess.Common.DataObjects

Namespace Common

	''' <summary>
	''' Interface for common database access.
	''' </summary>
	Public Interface ICommonDatabaseAccess


		Function LoadContactTypeData1() As IEnumerable(Of ContactType1Data)
		Function LoadCountryData() As IEnumerable(Of CountryData)
		Function LoadPostcodeData() As IEnumerable(Of PostCodeData)
		Function LoadCantonData() As IEnumerable(Of CantonData)
		Function LoadCantonByPostCode(ByVal postCode As String) As String
		Function LoadLanguageData() As IEnumerable(Of LanguageData)
		Function LoadGenderData() As IEnumerable(Of GenderData)
		Function LoadCivilStateData() As IEnumerable(Of CivilStateData)
		Function LoadPermissionData() As IEnumerable(Of PermissionData)
		Function TranslatePermissionCode(ByVal permissionCode As String, ByVal language As String) As String
		'<Obsolete("Die Methode LoadMandantAllowedListData() verwenden, da dort der richtige Mandantname geliefert wird.")>
		Function LoadCompaniesListData() As IEnumerable(Of MandantData)
		''' <summary>
		''' Lädt die Liste der zugelassenen Mandanten.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function LoadMandantAllowedListData() As IEnumerable(Of MandantData)
		Function LoadMandantData(ByVal mandantNumber) As MandantData
		Function LoadUpdateMandantData() As IEnumerable(Of RootMandantData)
		Function LoadAdvisorData() As IEnumerable(Of AdvisorData)
		Function LoadActivatedAdvisorData() As IEnumerable(Of AdvisorData)
		Function LoadAllAdvisorsData() As IEnumerable(Of AdvisorData)
		Function LoadAdvisorDataforGivenAdvisor(ByVal advisorKST As String) As AdvisorData
		Function LoadAdvisorDataforGivenGuid(ByVal advisorGuid As String) As AdvisorData
		Function LoadKSTDataForGivenFilial(ByVal filial As String) As IEnumerable(Of AdvisorData)
		Function LoadAdvisorDataforGivenNumber(ByVal mandantNumber As Integer, ByVal advisorNumber As Integer) As AdvisorData
		Function LoadBusinessBranchsData() As IEnumerable(Of AvilableBusinessBranchData)
		Function LoadBranchData() As IEnumerable(Of BranchData)
		Function LoadSalutationData() As IEnumerable(Of SalutationData)
		Function LoadTermsAndConditionsData() As IEnumerable(Of TermsAndConditionsData)
		Function LoadCurrencyData() As IEnumerable(Of CurrencyData)
		Function LoadAbsenceData() As IEnumerable(Of AbsenceData)
		Function LoadCostCenters() As DataObjects.CostCenters
		Function LoadDefaultCostCenters(ByVal usNr As Integer) As DataObjects.DefaultCostCenters
		Function LoadMandantYears(ByVal mdNumber As Integer) As IEnumerable(Of Integer)
		Function LoadEmployeeExistsPayrollMonthOfYear(ByVal mdNumber As Integer, ByVal employeeNr As Integer, ByVal year As Integer) As IEnumerable(Of Integer)
		Function LoadEmployeeExistsPayrollYear(ByVal mdNumber As Integer, ByVal employeeNr As Integer) As IEnumerable(Of Integer)

		Function LoadPayrollMonthOfYear(ByVal year As Integer, ByVal mdNumber As Integer) As IEnumerable(Of Integer)
		Function LoadClosedMonthOfYear(ByVal year As Integer, ByVal mdNumber As Integer) As IEnumerable(Of Integer)
		Function LoadTranslatedLABez(ByVal laNr As Decimal, ByVal language As String, ByVal defaultText As String) As String


		''' <summary>
		''' loads all monthclose data
		''' </summary>
		''' <param name="mdNumber"></param>
		''' <param name="year"></param>
		''' <param name="month"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function LoadAllMonthCloseData(ByVal mdNumber As Integer, ByVal year As Integer?, ByVal month As Integer?) As IEnumerable(Of MonthCloseData)

		''' <summary>
		''' opens selected closed month
		''' </summary>
		''' <param name="CloseMonth"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function DeleteMonthCloseData(CloseMonth As MonthCloseData) As Boolean

		''' <summary>
		''' close selected month
		''' </summary>
		''' <param name="CloseMonth"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function SaveMonthCloseData(CloseMonth As MonthCloseData) As Boolean

		''' <summary>
		''' loads not valid data for closing month
		''' </summary>
		''' <param name="mdNumber"></param>
		''' <param name="year"></param>
		''' <param name="month"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function LoadNotInvalidDataForClosingMonth(ByVal mdNumber As Integer, ByVal year As Integer?, ByVal month As Integer?) As IEnumerable(Of NotValidatedData)

		Function LoadSearchQueryTemplateData(ByVal mdNumber As Integer, ByVal showMenuIn As String) As IEnumerable(Of SearchQueryTemplateData)


		Function LoadMandantBytesData(ByVal mdNumber As Integer, ByVal logoModul As String) As Byte()

	End Interface

End Namespace