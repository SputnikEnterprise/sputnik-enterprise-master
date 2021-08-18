Namespace Employee.DataObjects.Salary

  Public Class AHVData
    Public Property ID As Integer
    Public Property GetField As Short?
    Public Property Description As String

    Public ReadOnly Property AHVCodeStr As String
      Get

        If GetField.HasValue Then
          Return GetField.ToString()
        End If

        Return Nothing

      End Get
    End Property

    Public Property TranslatedAHVText As String
  End Class

End Namespace
