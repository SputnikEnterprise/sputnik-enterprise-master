
Imports SPProgUtility

Public Class ClsLLCallHistoryPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property ShowAsDesign As Boolean

  Public Property MANr2Print As Integer
  Public Property ListSortBez As String
  Public Property USSignFileName As String
  Public Property ListFilterBez As List(Of String)

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

  Public Property SelectedLang As String
  Public Property AnzahlCopies As Integer
  Public Property liMANr2Print As List(Of Integer)
  Public Property ListOfExportedFilesMAStamm As New List(Of String)

End Class

