
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


Public Class FoundedData

  Public Property KDNr As Integer

  Public Property customername As String
  Public Property customeraddress As String

  Public Property betragohne_1 As Decimal
  Public Property betragex_1 As Decimal
  Public Property mwSt_1 As Decimal
  Public Property total_1 As Decimal

  Public Property betragohne_2 As Decimal
  Public Property betragex_2 As Decimal
  Public Property mwSt_2 As Decimal
  Public Property total_2 As Decimal

  Public Property kst As String
  Public Property kst_1 As String
  Public Property kst_2 As String

End Class
