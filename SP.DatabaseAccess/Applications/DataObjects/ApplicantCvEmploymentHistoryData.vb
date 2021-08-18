Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvEmploymentHistory. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvEmploymentHistoryData

    Public Property ID As Integer
    Public Property FK_CvProfile As Integer
    Public Property JobTitle As String
    Public Property FK_CvJobTitle As Integer?
    Public Property StartDate As DateTime?
    Public Property EndDate As DateTime?
    Public Property ExperienceYears As Integer?
    Public Property EmployerNameAndPlace As String
    Public Property EmployerName As String
    Public Property EmployerPlace As String
    Public Property Description As String
    Public Property QuitReason As String
    Public Property IsLastItem As Boolean
    Public Property IsLastItemWithJobTitle As Boolean
    Public Property IsCurrentEmployer As Boolean
    Public Property Remarks As String

  End Class

End Namespace
