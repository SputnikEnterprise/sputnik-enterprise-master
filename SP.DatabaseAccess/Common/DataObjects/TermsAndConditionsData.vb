Namespace Common.DataObjects

  ''' <summary>
  ''' Terms and conditions data (Tab_AGB)
  ''' </summary>
  Public Class TermsAndConditionsData
    Public Property ID As Integer
    Public Property Description As String

    ''' <summary>
    ''' Gets or sets the translated terms and conditions data.
    ''' </summary>
    ''' <returns>Returns the translated terms and condition text.</returns>
    Public Property TranslatedTermsAndConditions As String

  End Class

End Namespace