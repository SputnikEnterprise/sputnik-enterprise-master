Namespace Customer.DataObjects


  Public Class CustomerDependentEmployeeContactData

    Public Property EmployeeContactID As Integer
    Public Property EmployeeContactRecordNumber As Integer
    Public Property CustomerContactID As Integer
    Public Property EmployeeNumber As Integer
    Public Property LastName As String
    Public Property FirstName As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property Location As String
    Public Property CountryCode As String

    Public ReadOnly Property Name
      Get
        Return String.Format("{0}, {1}", LastName, FirstName).Trim()
      End Get
    End Property

    Public ReadOnly Property Address
      Get
        Return String.Format("{0}, {1}-{2} {3}", Street, CountryCode, Postcode, Location).Trim()
      End Get
    End Property

  End Class

End Namespace
