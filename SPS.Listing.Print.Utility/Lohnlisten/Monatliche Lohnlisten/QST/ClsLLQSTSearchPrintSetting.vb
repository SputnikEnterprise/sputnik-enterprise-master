
Imports SPProgUtility

Public Class ClsLLQSTSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property ShowAsDesgin As Boolean
  Public Property ShowPrintBox As Boolean

  Public Property MANr2Print As Integer
  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

  Public Property ListBez2Print As String
  Public Property SelectedCanton As String
  Public Property SelectedCommunity As String

	Public Property SetEmptyGemeindeWithCity As Boolean
	Public Property HideBruttolohnColumn As Boolean
	Public Property HideQSTBasisColumn As Boolean
	Public Property HideQSTBasis2Column As Boolean

	Public Property Gemeinde As String
  Public Property Adresse As String
  Public Property Zusatz As String
  Public Property ZHD As String
  Public Property Postfach As String
  Public Property Strasse As String
  Public Property Land As String
  Public Property Ort As String
  Public Property PLZ As String
  Public Property StammNr As String
  Public Property Provision As Decimal
  Public Property Kanton As String
  
End Class
