Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvEducationLevel. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvEducationLevelData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvEducationLevel.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EducationLevelEnum As Integer
      SecondaryEducation = 1
      VocationalEducation = 2
      University = 3
      Bachelor = 4
      Master = 5
      PostMaster = 6
      Course = 7
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
