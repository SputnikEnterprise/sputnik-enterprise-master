Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvDiploma. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvDiplomaData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvDiploma.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum DiplomaEnum As Integer
      Yes = 1
      No = 2
      Unknown = 4
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
