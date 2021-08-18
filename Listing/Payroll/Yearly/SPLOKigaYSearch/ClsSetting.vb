
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

	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	Public Property MDNr As Integer?

	Public Property jahr As Integer?

	Public Property anzsmaenner As Decimal?
	Public Property anzsfrauen As Decimal?


	Public Property anzamaenner As Decimal?
	Public Property anzafrauen As Decimal?


	Public Property totalsmaenner As Decimal?
	Public Property totalsfrauen As Decimal?
	Public Property totalamaenner As Decimal?
	Public Property totalafrauen As Decimal?


End Class
