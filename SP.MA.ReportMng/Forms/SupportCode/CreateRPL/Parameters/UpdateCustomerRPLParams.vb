Public Class UpdateCustomerRPLParams

  Public Property RPNr As Integer
  Public Property RPMonat As Byte
  Public Property RPJahr As String
  Public Property RPGAVNr As Decimal
  Public Property RPGAV_StdWeek As Decimal
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
  Public Property MATSpesenBas As Decimal
  Public Property MATSpesen As Boolean
  Public Property KstNr As Integer
  Public Property KstBez As String
  Public Property UserName As String
  Public Property RPZusatzText As String

  Public Property TimeTable As TimeTable

End Class
