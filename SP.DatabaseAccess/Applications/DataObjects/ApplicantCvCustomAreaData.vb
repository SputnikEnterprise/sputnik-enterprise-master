Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvCustomArea. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvCustomAreaData

    Public Property ID As Integer
    Public Property FK_CvProfile As Integer
    Public Property CvTitle As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedBy As String
    Public Property EditedOn As DateTime?
    Public Property EditedBy As String
    Public Property TotalExperienceYears As Integer?
    Public Property CurrentJob As String
    Public Property CurrentEmployer As String
    Public Property Last3Experiences As String
    Public Property FK_CvHighestEducationLevel As Integer?
    Public Property FK_CvSalary As Integer?
    Public Property FK_CvProfileStatus As Integer?
    Public Property FK_CvAvailability As Integer?
    Public Property CvComment As String
    Public Property LearnedOccupation As String
    Public Property FK_CvApproval As Integer?
    Public Property POBox As String
    Public Property ExternalID As String
    Public Property FK_CvProfilePicture As Integer?

  End Class

End Namespace
