Namespace Customer.DataObjects

  ''' <summary>
  ''' User data.
  ''' </summary>
  Public Class UserData
    Public Property UsrNr As Integer
    Public Property FirstName As String
    Public Property LastName As String
    Public Property KST As String
    Public ReadOnly Property Fullname As String
      Get
        Return Lastname & ", " & Firstname
      End Get
    End Property
  End Class

End Namespace