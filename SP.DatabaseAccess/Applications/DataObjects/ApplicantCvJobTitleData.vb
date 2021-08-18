Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvJobTitle. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvJobTitleData

    Public Sub New()
    End Sub

    Public Sub New(aCode As String, aName As String, aDescription As String)
      Code = aCode
      Name = aName
      Description = aDescription
    End Sub

    Public Property ID As Integer
    Public Property Name As String
    Public Property Code As String
    Public Property Description As String

  End Class

End Namespace
