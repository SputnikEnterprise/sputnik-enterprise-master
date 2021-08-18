Namespace Customer.DataObjects

  ''' <summary>
  ''' ContactDoc data (Kontakt_Doc)
  ''' </summary>
  Public Class ContactDoc

    Public Property ID As Integer
    Public Property FileBytes As Byte()
    Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property FileExtension As String

  End Class

End Namespace