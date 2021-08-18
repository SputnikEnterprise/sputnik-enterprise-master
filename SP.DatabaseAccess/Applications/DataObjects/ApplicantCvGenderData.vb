Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvGender. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvGenderData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvGender.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum GenderEnum As Integer
      NotKnown = 0
      Male = 1
      Female = 2
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
