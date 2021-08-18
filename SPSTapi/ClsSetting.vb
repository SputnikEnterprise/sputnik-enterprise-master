
Imports SPProgUtility


Public Class ClsSetting

  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class



Public Class TapiData

	Public Property NumbertoCall As String
	Public Property iModulNr As Short
	Public Property EmployeeNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property cResponsibleNumber As Integer

End Class

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


Public Class AssamlyInfo

	Public Property Filename As String
	Public Property Filelocation As String
	Public Property FileVersion As String
	Public Property FileProcessArchitecture As String
	Public Property FileCreatedon As DateTime

End Class
