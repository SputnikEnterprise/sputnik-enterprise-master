
Imports SPProgUtility

Public Class ClsLLKDUmsatzSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property ShowAsDesign As Boolean

  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

  Public Property IsJobAsListing As Boolean
  Public Property DocBez As String

  Public Property Filter_VonMonth_1 As String
  Public Property Filter_BisMonth_1 As String
  Public Property Filter_VonYear_1 As String

  Public Property Filter_VonMonth_2 As String
  Public Property Filter_BisMonth_2 As String
  Public Property Filter_VonYear_2 As String

End Class
