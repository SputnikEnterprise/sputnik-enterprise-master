
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
	
	Public Property FirstYear As Integer
	Public Property filiale As String
	Public Property kanton As String
	Public Property nationality As String

	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	Public Property MDNr As Integer?
	Public Property jahr As Integer

	Public Property manr As Integer?

	Public Property employeename As String
	Public Property employeeFirstname As String

	Public Property ablp As Integer?
	Public Property bislp As Integer?


	Public Property _7100 As Decimal?
	Public Property _7110 As Decimal?
	Public Property _7120 As Decimal?
	Public Property _7220 As Decimal?
	Public Property _7240 As Decimal?


	Public Property ahvnr As String
	Public Property mageschlecht As String

	Public Property filiale As String
	Public Property kst As String

	Public Property ahvgebdat As DateTime?


	Public ReadOnly Property abbislp As String
		Get
			Return String.Format("{0:00}-{1:00}", ablp, bislp)
		End Get
	End Property


End Class
