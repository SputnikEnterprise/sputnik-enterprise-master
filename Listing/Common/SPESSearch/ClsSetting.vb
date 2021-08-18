
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

Public Class EmployKSTData

	Public Property KSTNr As Integer
	Public Property KSTName As String

End Class

Public Class FoundedData

  Public Property ESNr As Integer
  Public Property MANr As Integer
  Public Property KDNr As Integer
  Public Property ZHDNr As Integer

  Public Property VakNr As Integer
  Public Property ProposeNr As Integer

  Public Property esals As String

  Public Property esbeginn As Date
  Public Property esend As Date?
  Public Property esbeginnend As String

  Public Property gavqualification As String
  Public Property gavnumber As String

  Public Property esmargewithbvg As Decimal
  Public Property esmargewithoutbvg As Decimal

  Public Property estarif As Decimal
  Public Property esstdlohn As Decimal
  Public Property gavstdlohn As Decimal

  Public Property employeename As String
  Public Property employeeaddress As String

  Public Property employeeemail As String
  Public Property employeepermission As String
  Public Property employeepermissionuntil As Date?
  Public Property employeequalification As String

  Public Property customername As String
  Public Property customeraddress As String
  Public Property customertelefon As String
  Public Property customeremail As String

  Public Property responsiblename As String
  Public Property responsibletelefon As String
  Public Property responsiblemobile As String
  Public Property responsibleemail As String

  Public Property foffice As String
  Public Property soffice As String

  Public Property kst1 As String
  Public Property kst2 As String
  Public Property kst3 As String

  Public Property customeradvisor As String
  Public Property employeeadvisor As String
  Public Property esadvisor As String

  Public Property vvisexported As Boolean
  Public Property esvisexported As Boolean

  Public Property vvbacked As Boolean
  Public Property esvbacked As Boolean


End Class




Public Class AssamlyInfo

	Public Property Filename As String
	Public Property Filelocation As String
	Public Property FileVersion As String
	Public Property FileProcessArchitecture As String
	Public Property FileCreatedon As DateTime

End Class
