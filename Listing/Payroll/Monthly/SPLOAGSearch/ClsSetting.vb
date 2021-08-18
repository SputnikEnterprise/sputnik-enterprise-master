
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
	Public Property listname As String
	Public Property manr As String
	Public Property vonmonat As String
	Public Property bismonat As String
	Public Property vonjahr As String
	Public Property bisjahr As String
	Public Property lohnarten As String
	Public Property deletenull As Boolean?
	Public Property gavberuf As String
	Public Property gav1kategorie As String

End Class

Public Class FoundedLORekapData

	Public Property lanr As Decimal?
	Public Property lalotext As String

	Public Property jahr As Integer?
	Public Property monat As Integer?

	Public Property hkonto As Integer?
	Public Property skonto As Integer?

	Public Property betrag As Decimal?
	Public Property kumulativ As Decimal?

	Public Property userNumber As Integer?

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

	Public Property gebdat As Date?

	Public Property ahv_nr As String
	Public Property ahv_nr_new As String

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeepostcode As String
	Public Property employeepostcodecity As String
	Public Property employeecity As String
	Public Property employeecountry As String

	Public Property m_anz As Decimal?
	Public Property m_bas As Decimal?
	Public Property m_ans As Decimal?
	Public Property m_btr As Decimal?

	Public Property gav_beruf As String
	Public Property gav_kanton As String
	Public Property gav_gruppe1 As String


End Class

