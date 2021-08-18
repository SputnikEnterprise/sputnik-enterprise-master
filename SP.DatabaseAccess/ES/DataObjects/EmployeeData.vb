Namespace ES.DataObjects.ESMng

  Public Class EmployeeData

    Public Property EmployeeNumber As Integer?
    Public Property LastName As String
    Public Property Firstname As String
    Public Property Postcode As String
    Public Property Location As String

    Public ReadOnly Property LastnameFirstname As String
      Get
				Return String.Format("{0}, {1}", LastName, Firstname)
      End Get
    End Property

    Public ReadOnly Property PostcodeAndLocation As String
      Get
        Return String.Format("{0} {1}", Postcode, Location)
      End Get
    End Property

  End Class

End Namespace
