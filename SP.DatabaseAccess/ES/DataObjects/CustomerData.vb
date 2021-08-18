Namespace ES.DataObjects.ESMng

  Public Class CustomerData

    Public Property CustomerNumber As Integer
    Public Property Company1 As String
    Public Property Street As String
    Public Property Postcode As String
    Public Property Location As String

    Public ReadOnly Property PostcodeAndLocation
      Get

        Return String.Format("{0} {1}", Postcode, Location)

      End Get
    End Property

  End Class

End Namespace
