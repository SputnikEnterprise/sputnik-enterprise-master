Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvSoftSkill. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvSoftSkillData

    Public Property ID As Integer
    Public Property FK_CvSkill As Integer
    Public Property Text As String
    Public Property FK_CvSoftSkillType As Integer?

  End Class

End Namespace
