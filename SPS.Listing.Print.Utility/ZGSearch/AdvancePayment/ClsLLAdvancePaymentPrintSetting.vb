
Imports SPProgUtility

Public Class ClsLLAdvancePaymentPrintSetting

	Public Property frmhwnd As String
	Public Property ZGNr As Integer
	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationItems As Dictionary(Of String, ClsTranslationData)

	Public Property ShowAsDesign As Boolean
	Public Property DbConnString2Open As String
	Public Property MANr2Print As Integer
	Public Property ListSortBez As String
	Public Property ListFilterBez As List(Of String)
	Public Property USSignFileName As String

	Public Property JobNr2Print As String
	Public Property SQL2Open As String
	Public Property ListBez2Print As String

	'0: Check
	'1: Quittung
	'2: Überweisung
	Public Property docart As Integer

End Class

