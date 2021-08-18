Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvOther. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvOtherData

    Public Property ID As Integer
    Public Property FK_CvProfile As Integer
    Public Property TotalExperience As String
    Public Property Salary As String
    Public Property Benefits As String

  End Class

End Namespace
