Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvEmailType. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvEmailTypeData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvEmailType.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EmailTypeEnum As Integer
      Unspecified = 1
      Corporate = 2
      NonCorporate = 3
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
