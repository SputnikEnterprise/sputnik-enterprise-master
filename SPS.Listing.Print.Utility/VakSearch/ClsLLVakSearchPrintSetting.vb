
Imports SPProgUtility

Public Class ClsLLVakSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property VakNr2Print As Integer
  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

  Public Property JobNr2Print As String
  Public Property SQL2Open As String
  Public Property IsJobAsListing As Boolean

  ''' <summary>
  ''' 0 = Jobprozent
  ''' 1 = Anstellung
  ''' 2 = MAAge
  ''' 3 = SprachBez
  ''' 4 = SprachNiveauBez
  ''' 5 = BerufGruppe
  ''' 6 = FachGruppe
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property ExtraVakFieldData As New List(Of String)

End Class
