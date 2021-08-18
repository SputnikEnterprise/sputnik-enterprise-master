
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

  Public Property RENr As Integer
  Public Property KDNr As Integer

  Public Property fakdat As Date?
  Public Property currency As String

  Public Property betragohne As Decimal?
  Public Property betragex As Decimal?
  Public Property betragink As Decimal?
  Public Property mwst1 As Decimal?
  Public Property mwstproz As Decimal?
  Public Property bezahlt As Decimal?

  Public Property skonto As Integer?

  Public Property faellig As Date?

  Public Property mahncode As String

  Public Property ma0 As Date?
  Public Property ma1 As Date?
  Public Property ma2 As Date?
  Public Property ma3 As Date?

  Public Property gebucht As Boolean



  Public Property fksoll As Integer?
  Public Property fkhaben0 As Integer?
  Public Property fkhaben1 As Integer?

  Public Property rname1 As String
  Public Property rname2 As String
  Public Property rname3 As String
  Public Property rzhd As String
  Public Property rabteilung As String
  Public Property rpostfach As String
  Public Property rstrasse As String
  Public Property rplz As String
  Public Property rort As String
  Public Property rland As String

  Public Property zahlkond As String
  Public Property printeddate As Date?

  Public Property eseinstufung As String
  Public Property kdbranche As String

  Public Property createdon As Date?
  Public Property createdfrom As String
  Public Property changedon As Date?
  Public Property changedfrom As String

	Public Property mdnr As Integer?

  Public Property reart As String
  Public Property reart2 As String

  Public Property kst As String
  Public Property rekst1 As String
  Public Property rekst2 As String

  Public Property kreditrefnr As String
  Public Property kreditlimite As Decimal?
  Public Property kreditlimite2 As Decimal?

  Public Property kreditlimiteab As Date?
  Public Property kreditlimitebis As Date?

  Public Property kdumsmin As Decimal?

  Public Property employeeadvisor As String
  Public Property customeradvisor As String


End Class


Public Class AssamlyInfo

	Public Property Filename As String
	Public Property Filelocation As String
	Public Property FileVersion As String
	Public Property FileProcessArchitecture As String
	Public Property FileCreatedon As DateTime

End Class