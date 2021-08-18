
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
	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	Public Property MDNr As Integer?
	Public Property lanr As Decimal?

	Public Property bezeichnung As String

	Public Property jahr As Integer?

	Public Property januar As Decimal?
	Public Property februar As Decimal?
	Public Property maerz As Decimal?
	Public Property april As Decimal?
	Public Property mai As Decimal?
	Public Property juni As Decimal?
	Public Property juli As Decimal?
	Public Property august As Decimal?
	Public Property september As Decimal?
	Public Property oktober As Decimal?
	Public Property november As Decimal?
	Public Property dezember As Decimal?
	Public Property kumulativ As Decimal?


End Class
