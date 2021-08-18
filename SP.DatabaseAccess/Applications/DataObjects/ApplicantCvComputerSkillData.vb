Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvComputerSkill. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvComputerSkillData

    Public Property ID As Integer
    Public Property FK_CvSkill As Integer
    Public Property Text As String
    Public Property FK_CvComputerSkillType As Integer?
    Public Property Duration As String

  End Class

End Namespace
