
Imports SPProgUtility

Public Class ClsLLPVLSearchPrintSetting

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

  Public Property ListBez2Print As String
	Public Property PauschaleChecked As Boolean?
	Public Property SearchAddressDataFromTableseting As Boolean?
	Public Property Jahreslohnsumme As Decimal
	Public Property AGPauschale As Decimal
	Public Property GAVBezeichnung As String


End Class
