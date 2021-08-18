
Imports SPProgUtility

Public Class ClsSetting

  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class MandantenData

  Public Property MDNr As Integer
  Public Property MDName As String
  Public Property MDGuid As String
  Public Property MDConnStr As String
  Public Property MultiMD As Short

End Class


Public Class FoundedData

  Public Property RPNr As Integer
  Public Property ZGNr As Integer

  Public Property MANr As Integer
  Public Property LANr As Integer
  Public Property LONr As Integer
  Public Property VGNr As Integer

  Public Property employeename As String
  Public Property zggrund As String

  Public Property betrag As Decimal
  Public Property monat As Integer
  Public Property jahr As Integer
  Public Property datum As Date
  Public Property ersteller As String


End Class
