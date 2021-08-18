Namespace Report.DataObjects

  Public Class UpdateCustomerRPLData
    Public Property RPNr As Integer
    Public Property RPLNr As Integer
    Public Property LANr As Decimal
    Public Property K_Anzahl As Decimal
    Public Property K_Basis As Decimal
    Public Property K_Ansatz As Decimal
    Public Property MWST As Decimal
    Public Property VonDate As DateTime
    Public Property BisDate As DateTime
		Public Property ESLohnNr As Integer
    Public Property KDBetrag As Decimal?
    Public Property TAnzahl As Short
    Public Property MATSpesenBas As Decimal
    Public Property MATSpesen As Boolean
    Public Property KstNr As Integer
    Public Property KstBez As String
    Public Property UserName As String
    Public Property RPZusatzText As String
  End Class

End Namespace
