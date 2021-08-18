''' <summary>
''' Inteface to the database.
''' </summary>
Public Interface IDatabaseAccess

  Function LoadEmployeeContactCommData(employeeNumber As Integer) As EmployeeContactComm


	'Function LoadAdvisorData() As IEnumerable(Of AdvisorData)

	Function LoadEmployeeMasterData(employeeNumber As Integer, Optional includeImageData As Boolean = False) As EmployeeMasterData
  Function LoadEmployeeData() As IEnumerable(Of EmployeeData)

  Function LoadCustomerData() As IEnumerable(Of CustomerData)
  Function LoadResponsiblePersonDataActiv(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData)

  Function LoadVacancyMasterData(ByVal vacancyNumber As Integer) As VacancyMasterData
  Function LoadVacancyData(ByVal customerNumber As Integer) As IEnumerable(Of VacancyData)

  Function LoadResponsiblePersonData(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData)

End Interface


''' <summary>
''' Employee master data (Mitarbeiter)
''' </summary>
Public Class EmployeeMasterData

  Public Property ID As Integer
  Public Property EmployeeNumber As Integer?
  Public Property Lastname As String
  Public Property Firstname As String
  Public Property PostOfficeBox As String
  Public Property Street As String
  Public Property Postcode As String
  Public Property Location As String
  Public Property Country As String
  Public Property Language As String
  Public Property Birthdate As DateTime?
  Public Property Gender As String
  Public Property AHV_Nr As String
  Public Property Nationality As String
  Public Property CivilStatus As String
  Public Property Telephone_P As String
  Public Property Telephone2 As String
  Public Property Telephone3 As String
  Public Property Telephone_G As String
  Public Property MobilePhone As String
  Public Property Homepage As String
  Public Property Email As String
  Public Property Facebook As String
  Public Property Xing As String
  Public Property Permission As String
  Public Property PermissionToDate As DateTime?
  Public Property BirthPlace As String
  Public Property Q_Steuer As String
  Public Property S_Canton As String
  Public Property ChurchTax As String
  Public Property Residence As Boolean? ' Ansaessigkeit
  Public Property ChildsCount As Short?
  Public Property Profession As String
  Public Property StaysAt As String ' Wohnt_bei
  Public Property Rapports As String
  Public Property V_Hint As String ' V_Hinweis
  Public Property CreatedOn As DateTime?
  Public Property ChangedOn As DateTime?
  Public Property CreatedFrom As String
  Public Property ChangedFrom As String
  Public Property HasImage As Boolean?
  Public Property MABild As Byte()
  Public Property Result As String
  Public Property KST As String
  Public Property FirstContact As DateTime?
  Public Property LastContact As DateTime?
  Public Property QSTCommunity As String 'QST Gemeinde
  Public Property BusinessBranch As String ' Filiale
  Public Property GAVBez As String
  Public Property CivilState2 As String
  Public Property QLand As String
  Public Property MABusinessBranch As String ' MAFiliale
  Public Property AHV_Nr_New As String
  Public Property MA_Canton As String
  Public Property ANS_OST_Bis As DateTime?
  Public Property Transfered_Guid As String
  Public Property Transfered_User As String
  Public Property Transfered_On As DateTime?
  Public Property Send2WOS As Boolean?
  Public Property WOSGuid As String
  Public Property MA_SMS_Mailing As Boolean?
  Public Property ProfessionCode As Integer?
  Public Property MDNr As Integer?
  Public Property MobilePhone2 As String

End Class


Public Class EmployeeData

  Public Property EmployeeNumber As Integer?
  Public Property LastName As String
  Public Property Firstname As String
  Public Property Postcode As String
  Public Property Location As String

  Public Property fstate As String
  Public Property DStellen As Boolean?
  Public Property NoES As Boolean?

  Public ReadOnly Property LastnameFirstname As String
    Get
      Return String.Format("{0}, {1}", LastName, Firstname)
    End Get
  End Property

  Public ReadOnly Property PostcodeAndLocation As String
    Get
      Return String.Format("{0} {1}", Postcode, Location)
    End Get
  End Property

End Class


Public Class EmployeeContactComm

  Public Property ID As Integer
  Public Property EmployeeNumber As Integer
  Public Property AnredeForm As String
  Public Property BriefAnrede As String
  Public Property KontaktHow As String
  Public Property KStat1 As String
  Public Property KStat2 As String
  Public Property WebExport As Boolean?
  Public Property ESAb As DateTime?
  Public Property ESEnde As DateTime?
  Public Property Absenzen As String
  Public Property NoWorkAS As String
  Public Property InLandSeit As String
  Public Property GetAHVKarte As Boolean?
  Public Property GetAHVKarteBez As String
  Public Property AHVKarteBacked As Boolean?
  Public Property AHVKateBackedBez As String
  Public Property InZV As Boolean?
  Public Property InZVBez As String
  Public Property RahmenArbeit As Boolean?
  Public Property RahemArbeitBez As String
  Public Property Res1 As String
  Public Property Res2 As String
  Public Property Res3 As String
  Public Property Res4 As String
  Public Property KundFristen As String
  Public Property KundGrund As String
  Public Property Arbeitspensum As String
  Public Property GehaltAlt As Decimal?
  Public Property GehaltNeu As Decimal?
  Public Property GotDocs As Boolean?
  Public Property Result As String
  Public Property GehaltPerMonth As Decimal?
  Public Property GehaltPerStd As Decimal?
  Public Property DStellen As Boolean?
  Public Property NoES As Boolean?
  Public Property Res5 As String
  Public Property AGB_WOS As String
  Public Property ZVeMail As String
  Public Property ZVVersand As String

End Class



Public Class CustomerData

  Public Property CustomerNumber As Integer
  Public Property Company1 As String
  Public Property Street As String
  Public Property Postcode As String
  Public Property Location As String

  Public Property fstate As String
  Public Property sstate As String
  Public Property howcontact As String
  Public Property noes As Boolean?

  Public ReadOnly Property PostcodeAndLocation
    Get

      Return String.Format("{0} {1}", Postcode, Location)

    End Get
  End Property

End Class

Public Class VacancyMasterData

  Public Property vacancynumber As Integer
  Public Property vacancybez As String

End Class

Public Class VacancyData

  Public Property vacancynumber As Integer
  Public Property vacancybez As String
  Public Property vacancystate As String
  Public Property customername As String
  Public Property street As String
  Public Property postcode As String
  Public Property location As String
  Public Property createdon As DateTime?

  Public ReadOnly Property PostcodeAndLocation
    Get

      Return String.Format("{0} {1}", Postcode, Location)

    End Get
  End Property

End Class


''' <summary>
''' The customer reserve data type.
''' </summary>
Public Enum CustomerReserveDataType
    Reserve1 = 1
    Reserve2 = 2
    Reserve3 = 3
    Reserve4 = 4
End Enum

''' <summary>
''' The responsible person reserve data type.
''' </summary>
Public Enum ResponsiblePersonReserveDataType
    Reserve1 = 1
    Reserve2 = 2
    Reserve3 = 3
    Reserve4 = 4
End Enum

''' <summary>
''' Result of customer invoice address assignment (KD_RE_Address) deletion.
''' </summary>
''' <remarks></remarks>
Public Enum DeleteCustomerInvoiceAddressAssignmentResult
    CouldNotDeleteOnlyOneRecordLeft = 1
    Deleted = 2
    CouldNotDeleteBecauseOfExistingKST = 3
    ErrorWhileDelete = 4
End Enum

''' <summary>
''' Result of customer KST assignment (KD_KST) deletion.
''' </summary>
Public Enum DeleteCustomerKSTAssignmentResult
    CouldNotDeleteOnlyOneRecordLeft = 1
    Deleted = 2
    CouldNotDeleteBecauseOfExistingEsLohn = 3
    CouldNotDeleteBecauseOfExistingRapport = 4
    ErrorWhileDelete = 5
End Enum

''' <summary>
''' Decision result.
''' </summary>
Public Enum DecisionResult
    LightGreen = 1
    Green = 2
    YellowGreen = 3
    Yellow = 4
    Orange = 5
    Red = 6
    DarkRed = 7
End Enum

''' <summary>
''' Business solvency check type.
''' </summary>
Public Enum BusinessSolvencyCheckType
    QuickBusinessCheck = 1
    BusinessCheck = 2
End Enum