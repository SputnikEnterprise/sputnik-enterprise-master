
Imports SPProgUtility

Public Class ClsLLDb1SearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property MANr2Print As Integer
  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

  Public Property XMarge As Double
  Public Property XMarge_2 As Double

  Public Property Kst3_1 As String
  Public Property Kst3Bez As String
  Public Property bAsGrouped As Boolean

  Public Property Filter_Month_1 As String
  Public Property FltJahr As String
  Public Property VonMonat As String
  Public Property BisMonat As String

  Public Property _dTotalTemp As Double
  Public Property _dTotalInd As Double
  Public Property _dTotalFest As Double

End Class
