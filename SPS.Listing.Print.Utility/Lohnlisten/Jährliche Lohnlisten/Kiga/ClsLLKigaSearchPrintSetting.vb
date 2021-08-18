
Imports SPProgUtility

Public Class ClsLLKigaSearchPrintSetting

	Public Property frmhwnd As String
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

	Public Property JahrVon2Print As String
	Public Property JahrBis2Print As String
	Public Property MonatVon2Print As String
	Public Property MonatBis2Print As String

	Public Property TotalSuva As New List(Of Decimal)
	Public Property esAnzM As Integer
	Public Property esAnzF As Integer

	Public Property LLShowESAnzJahr As Boolean
	Public Property LLESAnzJahr As Integer
	Public Property MDsuva_hl As Decimal
	Public Property ausgleichsNr As String

	Public Property SecSuvaCode As New Dictionary(Of String, String)
	Public Property PrintUVGWithSuvaRekap As Boolean

End Class
