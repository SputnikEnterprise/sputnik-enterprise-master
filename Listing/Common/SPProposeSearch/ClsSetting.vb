
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

  Public Property PNr As Integer
  Public Property MANr As Integer
  Public Property KDNr As Integer
  Public Property ZHDNr As Integer
  Public Property VakNr As Integer

  Public Property kst As String
  Public Property berater As String

  Public Property kd_kst As String
  Public Property ma_kst As String

  Public Property bezeichnung As String

  Public Property p_state As String
  Public Property p_art As String
  Public Property p_anstellung As String

  Public Property employeename As String
  Public Property ma_tel As String
  Public Property ma_natel As String

  Public Property customername As String
  Public Property kd_tel As String

  Public Property zhdname As String
  Public Property zhd_tel As String
  Public Property zhd_natel As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String


End Class
