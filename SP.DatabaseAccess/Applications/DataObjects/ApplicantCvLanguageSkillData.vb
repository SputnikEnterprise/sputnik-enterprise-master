Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvLanguageSkill. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvLanguageSkillData

    Public Property ID As Integer
    Public Property FK_CvSkill As Integer
    Public Property Text As String
    Public Property FK_CvLanguageSkillType As Integer?
    Public Property FK_CvLanguageProficiency As Integer?
    Public Property IsNativeLanguage As Boolean

  End Class

End Namespace
