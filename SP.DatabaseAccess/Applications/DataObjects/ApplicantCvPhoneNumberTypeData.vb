Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvEmailType. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvPhoneNumberTypeData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvPhoneNumberType.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PhoneNumberTypeEnum As Integer
      Mobile = 1
      Home = 2
      Fax = 3
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
