Namespace Employee.DataObjects.ContactMng

  ''' <summary>
  ''' Customer data (Kunden).
  ''' </summary>
  Public Class CustomerDataForContactMng
    Public Property CustomerNumber As Integer
    Public Property Company1 As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property Location As String

    ''' <summary>
    ''' Gets the post code and location.
    ''' </summary>
    Public ReadOnly Property PostcodeAndLocation
      Get
        Return String.Format("{0} {1}", Postcode, Location)
      End Get
    End Property

  End Class

End Namespace
