Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvPersonal. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvPersonalData

    Public Property ID As Integer
    Public Property Initials As String
    Public Property Title As String
    Public Property FirstName As String
    Public Property MiddleName As String
    Public Property LastNamePrefix As String
    Public Property LastName As String
    Public Property FullName As String
    Public Property DateOfBirth As DateTime?
    Public Property PlaceOfBirth As String
    Public Property FK_CvNationality As Integer?
    Public Property FK_CvGender As Integer?
    Public Property FK_CvDriversLicence As Integer?
    Public Property FK_CvMaritalStatus As Integer?
    Public Property Availability As String
    Public Property MilitaryService As String
    Public Property FK_CvAddress As Integer
    Public Property CreatedOn As DateTime
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String

  End Class

End Namespace
