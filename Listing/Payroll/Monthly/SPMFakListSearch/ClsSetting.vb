
Imports SPProgUtility

Public Class ClsSetting

	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class MandantenData

	Public Property MDNr As Integer
	Public Property MDName As String
	Public Property MDGuid As String
	Public Property MDConnStr As String
	Public Property MultiMD As Short

End Class


Public Class SearchCriteria

	Public Property mandantenname As String
	Public Property listname As String
	Public Property MANrList As String

	Public Property s_kanton As String
	Public Property filiale As String
	Public Property nationality As String

	Public Property firstYear As Integer
	Public Property lastYear As Integer

	Public Property firstMonth As Integer
	Public Property lastMonth As Integer

	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	Public Property MANr As Integer?

	Public Property employeelastname As String
	Public Property employeefirstname As String

	Public Property LANr As Decimal?
	Public Property m_Bas As Decimal?
	Public Property m_Ans As Decimal?
	Public Property m_Btr As Decimal?
	Public Property MAKIAnz As Integer?

	Public Property monat As Integer?
	'Public Property _3850 As Decimal?

	'Public Property _3900 As Decimal?
	'Public Property _3900_1 As Decimal?
	'Public Property _3901 As Decimal?
	'Public Property _3901_1 As Decimal?


	Public Property jahr As Integer?
	Public Property S_Kanton As String
	'Public Property gebdat As String
	Public Property RPText As String
	'Public Property ahv_nr_new As String


	Public ReadOnly Property LastnameFirstname As String
		Get
			Return String.Format("{0}, {1}", employeelastname, employeefirstname)
		End Get
	End Property



End Class
