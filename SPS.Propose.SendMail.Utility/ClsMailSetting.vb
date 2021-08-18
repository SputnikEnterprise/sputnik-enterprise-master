
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


Public Class ClsMailSetting

  Public Property ProposeNr2Send As Integer
  Public Property MANr2Send As Integer
  Public Property KDNr2Send As Integer
  Public Property KDZNr2Send As Integer?
  Public Property VakNr2Send As Integer?

  Public Property ProposeTitle As String

  Public Property JobNr2Send As String

  Public Property Doc2Send As New List(Of String)

End Class


Public Class MailTemplateData
	Public Property ID As Integer
	Public Property itemBez As String
	Public Property itemValue As String

End Class
