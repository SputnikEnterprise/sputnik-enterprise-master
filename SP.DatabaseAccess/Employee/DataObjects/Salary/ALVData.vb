Namespace Employee.DataObjects.Salary

  Public Class ALVData

    Public Property ID As Integer
    Public Property GetField As Short?
    Public Property Description As String


    Public ReadOnly Property ALVCodeStr As String
      Get

        If GetField.HasValue Then
          Return GetField.ToString()
        End If

        Return Nothing

      End Get
    End Property

    Public Property TranslatedALVText As String

  End Class

End Namespace
