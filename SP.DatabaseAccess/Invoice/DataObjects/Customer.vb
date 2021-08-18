Namespace Invoice.DataObjects

  ''' <summary>
  ''' The Customers
  ''' </summary>
  ''' <remarks></remarks>
  Public Class Customer
    Public Property KDNr As Integer
    Public Property Firma1 As String
    Public Property Strasse As String
    Public Property PLZ As String
    Public Property Ort As String
    Public Property Land As String
    Public Property Sprache As String
    Public Property Currency As String
    Public Property MWST As Boolean?

    Public ReadOnly Property Address As String
      Get
        Return String.Format("{0}, {1} {2}", Strasse, PLZ, Ort)
      End Get
    End Property
  End Class

  Public Class CustomerReAddress
    Public Property Id As Integer?
    Public Property KDNr As Integer
    Public Property REFirma As String
    Public Property REFirma2 As String
    Public Property REFirma3 As String
    Public Property REStrasse As String
    Public Property REPLZ As String
    Public Property REOrt As String
		Public Property REeMail As String
		Public Property SendAsZip As Boolean?
		Public Property RELand As String
		Public Property REAbteilung As String
    Public Property REZhd As String
    Public Property REPostfach As String
    Public Property RecNr As Integer?
    Public Property MahnCode As String
		Public Property PaymentCondition As String
		Public Property IsActive As Boolean

    Public ReadOnly Property Address As String
      Get
        Return String.Format("{0}, {1} {2}", REStrasse, REPLZ, REOrt)
      End Get
    End Property
  End Class

End Namespace

