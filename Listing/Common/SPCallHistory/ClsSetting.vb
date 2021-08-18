
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


Public Class FoundedCallData

	Public Property recnr As Integer
	Public Property zhdnr As Integer
  Public Property MANr As Integer
  Public Property KDNr As Integer
	Public Property fproperty As Integer?

	Public Property employeename As String
  Public Property customername As String
  Public Property responsiblename As String

  Public Property zeitpunkt As DateTime?
  Public Property recinfo As String
  Public Property berater As String
  Public Property recipient As String


  Public Property contactdescription As String
  Public Property contactdate As Date?
  Public Property contacttype As String
  Public Property contactsubject As String
  Public Property contactbody As String

End Class
