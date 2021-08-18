
Imports SPProgUtility

Public Class ClsSetting

	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class SearchCriteria

	Public Property mandantenname As String
	Public Property manr As String
	Public Property vonmonat As String
	Public Property bismonat As String
	Public Property vonjahr As String
	Public Property bisjahr As String
	Public Property lohnarten As String
	Public Property deletenull As Boolean?
	Public Property getfirstes As Boolean?

	Public Property jobnrforprint As String

End Class


Public Class BVGDayData

	Public Property RPNr As Integer
	Public Property Von As Date
	Public Property Bis As Date
	Public Property DayCount As Integer

End Class


Public Class FoundedData

	Public Property rownr As Integer?
	Public Property MANr As Integer?

	Public Property lanr As Decimal?
	Public Property lalotext As String

	Public Property vonmonat As Integer?
	Public Property bismonat As Integer?
	Public Property vonjahr As Integer?
	Public Property bisjahr As Integer?
	Public Property lonr As Integer?

	Public Property jahr As Integer?
	Public Property monat As Integer

	Public Property gebdat As String

	Public Property ahv_nr As String
	Public Property ahv_nr_new As String
	Public Property geschlecht As String

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeepostcode As String
	Public Property employeepostcodecity As String
	Public Property employeecity As String
	Public Property employeecountry As String

	Public Property kinder As Integer?
	Public Property employeelanguage As String
	Public Property zivilstand As String

	Public Property m_anz As Decimal?
	Public Property m_bas As Decimal?
	Public Property m_ans As Decimal?
	Public Property m_btr As Decimal?

	Public Property bvgein As String
	Public Property bvgaus As String

	Public Property ahvlohn As Decimal?
	Public Property bvgstd As Decimal?
	Public Property bvgdays As Decimal?

	Public Property esab As String
	Public Property esende As String


End Class

