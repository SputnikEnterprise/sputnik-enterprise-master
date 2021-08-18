
Imports SPProgUtility

Public Class ClsLLRPNotFinishedPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property bAsDesign As Boolean
  Public Property ESNr2Print As Integer
  Public Property ESLohnNr2Print As Integer
  Public Property USSignFileName As String

  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

End Class
