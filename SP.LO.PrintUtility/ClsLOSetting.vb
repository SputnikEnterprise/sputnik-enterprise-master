

Imports SPProgUtility

'Public Class ClsSetting

'  Public Property SelectedMDNr As Integer
'  Public Property SelectedMDYear As Integer
'  Public Property SelectedMDGuid As String
'  Public Property LogedUSNr As Integer

'  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
'  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

'End Class


Public Class MandantenData

  Public Property MDNr As Integer
  Public Property MDName As String
  Public Property MDGuid As String
  Public Property MDConnStr As String
  Public Property MultiMD As Short

End Class


Public Class FoundedRPData

  Public Property RPNr As Integer
  Public Property MANr As Integer
  Public Property KDNr As Integer
  Public Property ESNr As Integer

  Public Property employeename As String
  Public Property customername As String

  Public Property monthyear As String
  Public Property esperiode As String
  Public Property es_als As String
  Public Property rpperiode As String
  Public Property weeknumbers As String


End Class


Public Class ClsLOSetting

  Public Property SelectedMDNr As Integer
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String

  Public Property SelectedYear As List(Of Integer)
	Public Property SelectedMonth As List(Of Integer)

  Public Property SelectedMANr As List(Of Integer)
  Public Property SelectedLONr As List(Of Integer)

  Public Property SortBezeichnung As String

  ''' <summary>
  ''' 0 = Ohne WOS Kandidaten
  ''' 1 = Nur WOS Kandidaten
  ''' 2 = Alle suchen
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SelectedWOSProperty As Short

  Public Property MetroForeColor As System.Drawing.Color
  Public Property MetroBorderColor As System.Drawing.Color
  Public Property SearchAutomatic As Boolean
  Public Property ShowNullBetrag As Boolean
  Public Property FormOpenArt As OpenFormArt
  Public Property ShowLODetails As Boolean

  Enum OpenFormArt
    PrintingLO
    DeleteingLO
  End Enum


End Class
