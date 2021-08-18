
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


  Public Property VakNr As Integer
  Public Property KDNr As Integer
  Public Property ZHDNr As Integer

  Public Property title As String

  Public Property customername As String
  Public Property customeraddress As String

  Public Property responsiblename As String

  Public Property appointed As String
  Public Property employment As String
  Public Property duration As String

  Public Property customertelefon As String
  Public Property customeremail As String

  Public Property responsibletelefon As String
  Public Property responsiblemobile As String
  Public Property responsibleemail As String

  Public Property creator As String
  Public Property adviser As String
  Public Property office As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String

  Public Property isexported As Boolean
	Public Property jchonline As Boolean
	Public Property ostjobonline As Boolean

End Class
