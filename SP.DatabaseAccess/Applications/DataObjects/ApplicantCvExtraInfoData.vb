Namespace Applicant.DataObjects

  ''' <summary>
  ''' Data Class zur Tabelle tbl_CvExtraInfo. 
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ApplicantCvExtraInfoData

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="id_cvCustomArea"></param>
    ''' <param name="kvp"></param>
    ''' <remarks></remarks>
    Public Sub New(id_cvCustomArea As Integer, kvp As KeyValuePair(Of String, String))
      FK_CvCustomArea = id_cvCustomArea
      Key = kvp.Key
      Value = kvp.Value
    End Sub

    Public Property ID As Integer
    Public Property FK_CvCustomArea As Integer
    Public Property Key As String
    Public Property Value As String

  End Class

End Namespace
