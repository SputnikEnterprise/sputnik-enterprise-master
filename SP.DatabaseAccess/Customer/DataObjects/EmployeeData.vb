Namespace Customer.DataObjects

  ''' <summary>
  ''' Employee data (Mitarbeiter).
  ''' </summary>
  Public Class EmployeeData
    Public Property ID As Integer
    Public Property EmployeeNumber As Integer?
    Public Property Lastname As String
    Public Property Firstname As String
    Public Property Gender As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property Location As String
    Public Property CountryCode As String
    Public ReadOnly Property Fullname As String
      Get
        Return Lastname & ", " & Firstname
      End Get
    End Property
  End Class

End Namespace
