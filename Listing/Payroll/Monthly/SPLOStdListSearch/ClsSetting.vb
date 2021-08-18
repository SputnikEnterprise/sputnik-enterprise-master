
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

Public Class ComboboxValue
	Public Property ComboValue As String
	Public Property ComboCaption As String

End Class


Public Class SearchCriteria

	Public Property mandantenname As String
	Public Property listname As String
	Public Property Kanton As String
	Public Property filiale As String

	Public Property EmployeeNumbers As String

	Public Property FirstMonth As Integer
	Public Property LastMonth As Integer

	Public Property FirstYear As Integer
	Public Property LastYear As Integer
	Public Property Gavberuf As String

	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	Public Property MDNr As Integer?
	Public Property LONr As Integer?
	Public Property MANr As Integer?

	Public Property employeelastname As String
	Public Property employeefirstname As String

	Public Property lanr As Decimal?
	Public Property lp As Integer?
	Public Property jahr As Integer?
	Public Property m_btr As Decimal?

	Public Property gav_kanton As String
	Public Property gav_beruf As String

	Public ReadOnly Property employeename As String
		Get
			Return String.Format("{0}, {1}", employeelastname, employeefirstname)
		End Get
	End Property



End Class
