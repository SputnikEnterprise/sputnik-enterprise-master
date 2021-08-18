Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvSocialMedia. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvSocialMediaData

    Public Property ID As Integer
    Public Property FK_CvPersonal As Integer
    Public Property FK_CvSocialMediaType As Integer?
    Public Property Url As String

  End Class

End Namespace
