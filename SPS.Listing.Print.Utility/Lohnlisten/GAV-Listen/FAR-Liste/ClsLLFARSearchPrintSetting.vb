
Imports SPProgUtility

Public Class ClsLLFARSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

	Public Property ShowAsDesign As Boolean

  Public Property JobNr2Print As String
  Public Property SQL2Open As String
	Public Property FORResor As Boolean

	Public Property ListBez2Print As String
  Public Property FARMitgliedNr As String

End Class
