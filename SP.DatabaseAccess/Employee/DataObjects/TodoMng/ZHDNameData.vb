Namespace Employee.DataObjects.TodoMng

  ''' <summary>
  ''' ZHD Name data.
  ''' </summary>
  Public Class ZHDNameData
    Public Property RecNr As Integer
    Public Property KDNr As Integer
    Public Property Lastname As String
    Public Property Firstname As String
    Public ReadOnly Property Fullname As String
      Get
        Return Lastname & ", " & Firstname
      End Get
    End Property
  End Class

End Namespace