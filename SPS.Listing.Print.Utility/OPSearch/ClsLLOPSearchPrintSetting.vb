
Imports SPProgUtility

Public Class ClsLLOPSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationData As Dictionary(Of String, ClsTranslationData)

	Public Property DbConnString2Open As String
	Public Property ShowAsDesign As Boolean
	Public Property HideOPInfoLine As Boolean?
	Public Property HideRefNrLine As Boolean?
	Public Property HideKreditInfoLine As Boolean?

	Public Property OPNr2Print As Integer
	Public Property ListSortBez As String
	Public Property ListFilterBez As List(Of String)
	Public Property USSignFileName As String

	Public Property JobNr2Print As String
	Public Property SQL2Open As String

	Public Property IsJobAsListing As Boolean

	Public Property DocBez As String
	Public Property TotalOpenBetrag4Date As Double
	Public Property FirstDate As String
	Public Property LastDate As String

	Public Property bGetCreatedOnInsteadFakDate As Boolean
	Public Property bShowFaelligInColumn As Boolean
	Public Property bShow15DayAsFirst As Boolean

End Class
