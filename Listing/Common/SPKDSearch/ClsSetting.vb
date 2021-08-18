
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


Public Class FoundedData

  Public Property KDNr As Integer
  Public Property KDZNr As Integer?

	Public Property customername As String
  Public Property customername2 As String
  Public Property customername3 As String

  Public Property cresponsiblefirstname As String
  Public Property cresponsiblelastname As String
  Public Property cresponsiblefullname As String

  Public Property customerstreet As String
  Public Property customerpostcode As String
  Public Property customercity As String
  Public Property customerbox As String
  Public Property customeraddress As String
  Public Property customertelefon As String
  Public Property customertelefax As String
  Public Property customeremail As String

  Public Property kreditlimite As Decimal?
  Public Property kreditlimite2 As Decimal?
  Public Property kreditab As Date?
  Public Property kreditbis As Date?
  Public Property kreditrefnr As String
  Public Property kreditwarning As Boolean?
  Public Property customerumsatzmin As Decimal?

  Public Property cresponsibletelefon As String
  Public Property cresponsibletelefax As String
  Public Property cresponsiblemobil As String
  Public Property cresponsibleemail As String
  Public Property cresponsibleabteilung As String
  Public Property cresponsibleposition As String
  Public Property cresponsiblefstate As String
  Public Property cresponsiblesstate As String

  Public Property customercanton As String

	Public Property customerfproperty As Integer?

	Public Property customeradvisor As String
  Public Property cresponsibleadvisor As String

End Class


Public Class AssamlyInfo

	Public Property Filename As String
	Public Property Filelocation As String
	Public Property FileVersion As String
	Public Property FileProcessArchitecture As String
	Public Property FileCreatedon As DateTime

End Class
