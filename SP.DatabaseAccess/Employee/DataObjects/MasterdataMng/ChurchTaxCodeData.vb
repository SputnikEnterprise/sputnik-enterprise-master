Namespace Employee.DataObjects.MasterdataMng

  Public Class ChurchTaxCodeData
    Public Property ID As Integer
    Public Property Code As String
    Public Property Description As String

    ''' <summary>
    ''' Gets or sets the translated church code data.
    ''' </summary>
    ''' <returns>Returns the translated church code text.</returns>
    Public Property TranslateChurchCodeText As String
  End Class

End Namespace
