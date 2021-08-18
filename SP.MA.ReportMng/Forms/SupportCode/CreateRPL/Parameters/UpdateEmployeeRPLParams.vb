Public Class UpdateEmployeeRPLParams

  Public Property RPNr As Integer
  Public Property RPMonat As Byte
  Public Property RPJahr As String
  Public Property RPGAVNr As Decimal
  Public Property RPGAV_StdWeek As Decimal
  Public Property RPLNr As Integer
  Public Property LANr As Decimal
  Public Property M_Anzahl As Decimal
  Public Property M_Basis As Decimal
  Public Property M_Ansatz As Decimal
  Public Property M_Ferien As Decimal
  Public Property M_Feier As Decimal
  Public Property M_13 As Decimal
  Public Property VonDate As DateTime
  Public Property BisDate As DateTime
	Public Property ESLohnNr As Integer
  Public Property LOSpesenBas As Decimal
  Public Property LOSpesen As Boolean
  Public Property StdTotal As Decimal
  Public Property FeierTotal As Decimal
  Public Property FerTotal As Decimal
  Public Property Total13 As Decimal
  Public Property FerBas As Decimal
  Public Property Basis13 As Decimal
  Public Property MATSpesenBas As Decimal
  Public Property MATSpesen As Boolean
  Public Property KstNr As Integer
  Public Property KstBez As String
  Public Property KompStd As Decimal
  Public Property KompBetrag As Decimal
  Public Property UserName As String
  Public Property IsPVL As Byte
  Public Property RPZusatzText As String

  Public Property TimeTable As TimeTable

End Class
