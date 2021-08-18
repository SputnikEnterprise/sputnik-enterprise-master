Namespace Employee.DataObjects.MasterdataMng

  ''' <summary>
  ''' Employee contact info data.
  ''' </summary>
  Public Class EmployeeContactInfoData
    Public Property ID As Integer
    Public Property GetFeld As String
    Public Property Description As String

    ''' <summary>
    ''' Gets or sets the translated contact info data.
    ''' </summary>
    ''' <returns>Returns the translated cantact info text.</returns>
    Public Property TranslatedContactInfoText As String

  End Class

End Namespace
