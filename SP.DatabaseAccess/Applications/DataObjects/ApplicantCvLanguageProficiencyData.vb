Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvLanguageProficiency. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvLanguageProficiencyData

    ''' <summary>
    ''' Enum korrespondierend zu tbl_CvLanguageProficiency.ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum LanguageProficiencyEnum As Integer
      No = 1
      Elementary = 2
      Good = 3
      Advanced = 4
      BusinessFluent = 5
      Native = 6
    End Enum

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
