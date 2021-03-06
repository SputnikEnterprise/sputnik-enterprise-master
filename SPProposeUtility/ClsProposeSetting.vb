

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

End Class


Public Class ClsProposeSetting

  Public Property SelectedProposeNr As Integer?

  Public Property SelectedVakNr As Integer?
  Public Property SelectedKDNr As Integer?
  Public Property SelectedZHDNr As Integer?
  Public Property SelectedMANr As Integer?
	Public Property ApplicationNumber As Integer?
	Public Property IsAsDuplicated As Boolean?


End Class
