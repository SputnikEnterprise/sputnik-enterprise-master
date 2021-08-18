Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvHighestEducationLevel. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvHighestEducationLevelData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvHighestEducationLevel.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum HighestEducationLevelEnum As Integer
      SecondaryEducation = 1
      VocationalEducation = 2
      University = 3
      Bachelor = 4
      Master = 5
      PostMaster = 6
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
