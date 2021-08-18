
Namespace TempData

	Public Class KeywordsData
		Public Property Value As String

	End Class

	Public Class BranchesData
		Public Property Branch As String
		Public Property Contracts As List(Of BranchesContrants)

	End Class

	Public Class BranchesContrants
		Public Property ContractName As String
		Public Property ContractNumber As String
		Public Property VersionNumber As Integer
		Public Property EditionID As Integer

	End Class

	Public Class ContractData
		Public Property ContractName As String
		Public Property ContractNumber As String
		Public Property ContractLanguage As String
		Public Property ContractResponsible As String
		Public Property Link As String
		Public Property FirstContractConclusion As DateTime?

		Public ReadOnly Property ContractViewData As String
			Get
				Return String.Format("{0} - {1}", ContractNumber, ContractName)
			End Get
		End Property

	End Class

	Public Class ContractVersionData
		Public Property Number As Integer?
		Public Property ValidFrom As DateTime?
		Public Property ValidTo As DateTime?
		Public Property Name As String
		Public Property NameShort As String
		Public Property TranslatedToLanguages As List(Of LanguageData)
		Public Property LB As AVEData
		Public Property Annex1 As Boolean?
		Public Property Annex1ExtendedUntil As DateTime?

		Public ReadOnly Property ContractVersionViewData As String
			Get
				Return String.Format("{0} - {1}", Number, Name)
			End Get
		End Property

	End Class

	Public Class ContractVersionEditionData
		Public Property ID As Integer?
		Public Property slEditionID As Integer?
		Public Property Status As String
		Public Property Created As DateTime?
		Public Property clauses As List(Of KlausInData)
		Public Property Cantons As List(Of CantonData)

		Public ReadOnly Property ContractVersionEditionViewData As String
			Get
				Return String.Format("{0} - {1:g}", ID, Created)
			End Get
		End Property

	End Class

	Public Class ContractEditionDocumentData
		Public Property Name As String
		Public Property Filename As String
		Public Property Extension As String
		Public Property Size As Integer?
		Public Property FileID As Integer?

		Public ReadOnly Property ContractEditionDocumentViewData As String
			Get
				Return String.Format("{0} - {1}", FileID, Name)
			End Get
		End Property

	End Class

	Public Class DocumentFileData
		Public Property Filename As String
		Public Property Extension As String
		Public Property Data As Byte()

	End Class

	Public Class ContractEditionLinksData
		Public Property URL As String
		Public Property Description As String

	End Class

	Public Class MindestLohnInputCriteriasData
		Public Property Title As String
		Public Property Name As String
		Public Property ParentID As Integer?
		Public Property ID As Integer?
		Public Property Children As List(Of ChildrenData)

	End Class

	Public Class InputValueData
		Public Property CriteriaStructureID As Integer?
		Public Property Value As String
		Public Property CriteriaListEntryID As Integer?
		Public Property CriteriaListEntryID_Org As Integer?

	End Class

	Public Class InputValueData_Org
		Public Property CriteriaStructureID As Integer?
		Public Property Value As String
		Public Property CriteriaListEntryID As Integer?

	End Class

	Public Class MindestLohnResultData
		Public Property Salary As SalaryData
		Public Property VariableText As VariableTextData
		Public Property AdditionalText As List(Of AdditionalTextData)
		Public Property Footnotes As List(Of FootnotesData)
		Public Property AlternativeTexte As List(Of AlternativeTextData)

	End Class

	Public Class PublicationNewsData
		Public Property ContractNumber As String
		Public Property VersionNumber As Integer?
		Public Property PublicationDate As DateTime?
		Public Property Title As String
		Public Property Content As String

	End Class

	Public Class PublicationNewsViewData
		Inherits PublicationNewsData

		Public Property ID As Integer?
		Public Property Viewed As Boolean?
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?

	End Class




	Public Class KlausInData
		Public Property ClauseTypID As Integer?
		Public Property ClauseTyp As String
		Public Property Value As String

	End Class

	Public Class CantonData
		Public Property Value As String
		Public Property Code As String

	End Class

	Public Class ChildrenData
		Public Property Title As String
		Public Property Name As String
		Public Property ParentID As Integer?
		Public Property ID As Integer?
		Public Property Children As List(Of ChildrenData)

	End Class

	Public Class ChildrenrResultData
		Public Property CriteriaStructureID As Integer?
		Public Property Name As String
		Public Property Value As String
		Public Property CriteriaListEntryId As Integer?

	End Class


	Public Class SalaryData

		Public Property MonthSalary As Decimal?
		Public Property VacationDays As Decimal?
		Public Property PublicHolidays As Decimal?
		Public Property BasicSalary As Decimal?
		Public Property PublicHolidaysCompensation As Decimal?

		Public Property Compensation13thSalary As Decimal?

		Public Property HasERWContribution As Boolean?
		Public Property ERWContribution As Decimal?
		Public Property EREContribution As Decimal?
		Public Property Has13thMonthSalary As Boolean?
		Public Property PCTVacationCompensation As Decimal?
		Public Property PCTHolidaysCompensation As Decimal?
		Public Property PCT13thMonthSalary As Decimal?

		Public Property PublicHolidaysCalculationType As String
		Public Property VacationCalculationType As String
		Public Property Month13thSalaryCalculationType As String








		Public Property VacationCompensation As Decimal?





	End Class

	Public Class VariableTextData
		Public Property AdditionalProp1 As AdditionalTextData
		Public Property AdditionalProp2 As AdditionalTextData
		Public Property AdditionalProp3 As AdditionalTextData


		Public Property pctHolidaysCompensation As PctHolidaysCompensation
		Public Property pctVacationCompensation As PctVacationCompensation
		Public Property publicHolidays As PublicHolidays
		Public Property compensation13thSalary As Compensation13thSalary
		Public Property ereContribution As EreContribution

	End Class


	Public Class AdditionalTextData
		Public Property Titel As String
		Public Property Text As String
		Public Property Description As String

	End Class

	Public Class PctHolidaysCompensation
		Public Property title As String
		Public Property text As String
		Public Property description As String
	End Class

	Public Class PctVacationCompensation
		Public Property title As String
		Public Property text As String
		Public Property description As String
	End Class

	Public Class PublicHolidays
		Public Property title As String
		Public Property text As String
		Public Property description As String
	End Class

	Public Class Compensation13thSalary
		Public Property title As String
		Public Property text As String
		Public Property description As String
	End Class

	Public Class EreContribution
		Public Property title As String
		Public Property text As String
		Public Property description As String
	End Class
	Public Class FootnotesData
		Public Property Titel As String
		Public Property Text As String
		Public Property Description As String

	End Class

	Public Class AlternativeTextData
		Public Property Text As String

	End Class

	Public Class LanguageData
		Public Property Language As String

	End Class

	Public Class AVEData
		Public Property LBStart As DateTime?
		Public Property LBEnd As DateTime?
		Public Property ExpiratyDate As DateTime?

	End Class







	Public Class ResponseErrorData
		Public Property Content As String
		Public Property Type As String
		Public Property Title As String
		Public Property Status As Integer
		Public Property Detail As String
		Public Property Instance As String
		Public Property Extensions As ExtensionsData
		Public Property AdditionalProp1 As String
		Public Property AdditionalProp2 As String
		Public Property AdditionalProp3 As String

	End Class

	Public Class ExtensionsData
		Public Property AdditionalProp1 As String
		Public Property AdditionalProp2 As String
		Public Property AdditionalProp3 As String

	End Class


End Namespace
