
Namespace PayrollMng.DataObjects

  Public Class LOMasterData
    Public Property ID As Integer
    Public Property MANR As Integer?
    Public Property MAName As String
    Public Property LONR As Integer?
    Public Property LP As Short?
    Public Property Jahr As Short?
    Public Property KST As String
    Public Property S_Kanton As String
    Public Property Zivilstand As String
    Public Property Kirchensteuer As String
    Public Property Q_Steuer As String
    Public Property AnzahlKinder As Short?
    Public Property Wohnort As String
    Public Property Land As String
    Public Property WorkedDays As Integer?
    Public Property Bruttolohn As Decimal?
    Public Property AHV_Basis As Decimal?
    Public Property AHV_Lohn As Decimal?
    Public Property AHV_Freibetrag As Decimal?
    Public Property Nicht_AHV_pflichtig As Decimal?
    Public Property ALV1_Lohn As Decimal?
    Public Property ALV2_Lohn As Decimal?
    Public Property SUVA_Basis As Decimal?
    Public Property Result As String
    Public Property LOKst1 As String
    Public Property LOKst2 As String
    Public Property ZGNr As Integer?
    Public Property LMID As Integer?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property QSTBasis As Decimal?
    Public Property QSTTarif As String
    Public Property WorkedDate As String
    Public Property DTADate As DateTime?
    Public Property ESData As String
    Public Property BVGBeginn As DateTime?
    Public Property BVGEnde As DateTime?
    Public Property MData As String
    Public Property LODoc_Guid As String
    Public Property Transfered_User As String
    Public Property Transfered_On As String
    Public Property IsComplete As Boolean?
    Public Property BVGBegin As DateTime?
    Public Property BVGEnd As DateTime?
    Public Property MDNr As Integer?
		Public Property CreatedUserNumber As Integer?
	End Class

End Namespace
