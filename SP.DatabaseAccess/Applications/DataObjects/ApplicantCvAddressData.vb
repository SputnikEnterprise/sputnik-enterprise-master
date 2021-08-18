Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvAddress. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvAddressData

    Public Property ID As Integer
    Public Property AddressLine As String
    Public Property StreetName As String
    Public Property StreetNumberBase As String
    Public Property StreetNumberExtension As String
    Public Property PostalCode As String
    Public Property City As String
    Public Property FK_CvRegion As Integer?
    Public Property FK_CvCountry As Integer?
    Public Property CreatedOn As DateTime
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String

  End Class

End Namespace
