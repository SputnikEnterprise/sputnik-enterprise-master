Public Class CreateEmployeeRPLParams

  Public Property RPNr As Integer
  Public Property RPMonat As Byte
  Public Property RPJahr As String
  Public Property RPGAVNr As Decimal
  Public Property RPGAV_StdWeek As Decimal
  Public Property MDNr As Integer
  Public Property KDNr As Integer
  Public Property MANr As Integer
  Public Property ESNr As Integer
  Public Property KSTNR As Integer
  Public Property KstBez As String
  Public Property GAVText As String
  Public Property Currency As String
  Public Property LANr As Decimal
  Public Property M_Anzahl As Decimal
  Public Property M_Basis As Decimal
  Public Property M_Ansatz As Decimal
  Public Property SUVA As String
  Public Property M_Ferien As Decimal
  Public Property M_Feier As Decimal
  Public Property M_13 As Decimal
  Public Property VonDate As DateTime
  Public Property BisDate As DateTime
  Public Property FerBas As Decimal
  Public Property Basis13 As Decimal
	Public Property ESLohnNr As Integer
  Public Property LOSpesenBas As Decimal
  Public Property LOSpesen As Boolean
  Public Property MATSpesenBas As Decimal
  Public Property MATSpesen As Boolean
  Public Property StdTotal As Decimal
  Public Property FeierTotal As Decimal
  Public Property FerTotal As Decimal
  Public Property Total13 As Decimal
  Public Property UserName As String
  Public Property IsPVL As Byte
  Public Property RPZusatzText As String

  Public Property Stundenlohn As Decimal
  Public Property TimeTable As TimeTable
  Public Property IsFlexibleTimeActive As Boolean

  Public Property AdditionalParamsForCustomerRPLDuplication As AdditionalParamsForCustomerRPLDuplication

	Public Property NewRPLNr As Integer
	Public Property NewCustomerDuplicatedRPLNr As Integer?

End Class

Public Class AdditionalParamsForCustomerRPLDuplication
  Public Property BasisValue As Decimal
  Public Property AnsatzValue As Decimal
  Public Property LABasisFactor As Decimal
  Public Property KDTSpesen As Decimal
  Public Property MwSt As Decimal
End Class