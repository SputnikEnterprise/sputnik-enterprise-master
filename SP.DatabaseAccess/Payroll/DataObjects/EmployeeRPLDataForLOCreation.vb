Namespace PayrollMng.DataObjects

	Public Class EmployeeRPLDataForLOCreation

		Public Property MANr As Integer
		Public Property Name As String
		Public Property Q_Steuer As String
		Public Property Kinder As Short?
		Public Property Zahlart As String
		Public Property NOLO As Boolean?
		Public Property ESNr As Integer?
		Public Property KDNr As Integer?
		Public Property RPNR As Integer?
		Public Property RPLNR As Integer?
		Public Property LANR As Decimal?
		Public Property KompBetrag As Decimal?
		Public Property KompStd As Decimal?
		Public Property M_Anzahl As Decimal?
		Public Property M_Basis As Decimal?
		Public Property M_Ansatz As Decimal?
		Public Property M_Betrag As Decimal?
		Public Property SUVA As String
		Public Property BVGStd As Double?
		Public Property VonDate As DateTime?
		Public Property BisDate As DateTime?
		Public Property RPZusatzText As String
		Public Property LONr As Integer?
		Public Property RPKst1 As String
		Public Property RPKst2 As String
		Public Property RPKst As String
		Public Property Far_pflicht As Boolean?
		Public Property ES_Einstufung As String
		Public Property KDBranche As String
		Public Property RPGAV_Nr As Integer?
		Public Property RPGAV_Kanton As String
		Public Property RPGAV_Beruf As String
		Public Property RPGAV_Gruppe1 As String
		Public Property RPGAV_Gruppe2 As String
		Public Property RPGAV_Gruppe3 As String
		Public Property RPGAV_Text As String
		Public Property RPGAV_FAN As Decimal?
		Public Property RPGAV_FAG As Decimal?
		Public Property RPGAV_WAN As Decimal?
		Public Property RPGAV_WAG As Decimal?
		Public Property RPGAV_VAN As Decimal?
		Public Property RPGAV_VAG As Decimal?
		Public Property RPGAV_WAN_S As Decimal?
		Public Property RPGAV_WAG_S As Decimal?
		Public Property RPGAV_VAN_S As Decimal?
		Public Property RPGAV_VAG_S As Decimal?
		Public Property RPGAV_WAN_M As Decimal?
		Public Property RPGAV_WAG_M As Decimal?
		Public Property RPGAV_VAN_M As Decimal?
		Public Property RPGAV_VAG_M As Decimal?
		Public Property RPGAV_StdMonth As Decimal?
		Public Property RPGAV_StdYear As Decimal?
		Public Property RPText As String
	End Class


	Public Class GAVNumberLabelData
		Public Property GAVNumber As Integer
		Public Property GAVGruppe0 As String
		Public Property GAVGruppe1 As String

	End Class

End Namespace
