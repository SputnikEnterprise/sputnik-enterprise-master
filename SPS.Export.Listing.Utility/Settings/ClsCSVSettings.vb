
Imports SPProgUtility

Public Class ClsCSVSettings

  Public Property DbConnString2Open As String

  Public Property SelectedMonth As Integer
  Public Property SelectedYear As Integer

  Public Property ModulName As String
  Public Property SQL2Open As String
  Public Property SQLTableName As String
  Public Property SQL4FieldShow As String
  Public Property SQLFields As String
	Public Property SQLLabel As String

  Public Property ExportFileName As String
  Public Property FieldSeprator As String
  Public Property FieldIn As String
  Public Property KDRefNrAsFKSoll As Boolean
  Public Property MitGegenKostenart As Boolean

  Public Property MwStCode As String
	Public Property ExportInvoiceData As Boolean



	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)


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


End Class
