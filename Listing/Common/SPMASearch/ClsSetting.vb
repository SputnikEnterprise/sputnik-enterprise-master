
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

  Public Property MANr As Integer

  Public Property employeelastname As String
  Public Property employeefirstname As String
  Public Property employeefullname As String

  Public Property employeeco As String
  Public Property employeepostcode As String
  Public Property employeestreet As String
  Public Property employeeplz As String
  Public Property employeecity As String

  Public Property employeeaddress As String

  Public Property employeetelefon As String
  Public Property employeemobil As String

  Public Property employeequalification As String
  Public Property employeeemail As String
  Public Property employeeadvisor As String


  Public Property employeebirthday As Date?
  Public Property employeeahvnumber As String
  Public Property employeepermision As String
  Public Property employeepermisiontil As Date?
  Public Property employeenationality As String
  Public Property employeezivilstand As String
  Public Property employeecanton As String

  Public Property employeefstate As String
  Public Property employeesstate As String
	Public Property employeeHomeland As String


End Class


Public Class AssamlyInfo

	Public Property Filename As String
	Public Property Filelocation As String
	Public Property FileVersion As String
	Public Property FileProcessArchitecture As String
	Public Property FileCreatedon As DateTime

End Class


Public Class EmployeeAssignedProfessionData
	Public Property ProfessionCode As Integer?
	Public Property ProfessionText As String

	Public ReadOnly Property ProfessionTextCode As String
		Get
			Return String.Format("{0}|{1}", ProfessionCode, ProfessionText)

		End Get
	End Property

End Class

Public Class EmployeeAddressSourceData
	Public Property AddressType As Integer
	Public Property AddressTypeLabel As String

End Class
