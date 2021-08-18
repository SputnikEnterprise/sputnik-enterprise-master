Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvEmail. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvEmailData

    Public Property ID As Integer
    Public Property FK_CvPersonal As Integer
    Public Property FK_CvEmailType As Integer
    Public Property Email As String

  End Class

End Namespace
