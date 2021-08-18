Namespace DTAUtility.DataObjects


  Public Class ZgData

    Public Property ZGNr As Integer?
    Public Property MANr As Integer?
    Public Property LONr As Integer?
    Public Property ZGGrund As String
    Public Property Betrag As Decimal?
    Public Property Currency As String
    Public Property LP As Integer?
    Public Property Jahr As Integer?
		Public Property ClearingNr As String
		Public Property Bank As String
    Public Property KontoNr As String
    Public Property BankOrt As String
    Public Property DTAAdr1 As String
    Public Property DTAAdr2 As String
    Public Property DTAAdr3 As String
    Public Property DTAAdr4 As String
    Public Property IBANNr As String
    Public Property Swift As String
    Public Property BLZ As String
    Public Property Nachname As String
    Public Property Vorname As String
		Public Property EmployeeCountry As String
		Public Property NoZG As Boolean
		Public Property NoLO As Boolean

  End Class


	Public Class DtaDataForListing

		Public Property ZGNr As Integer?
		Public Property MANr As Integer?
		Public Property LONr As Integer?
		Public Property ZGGrund As String
		Public Property Betrag As Decimal?
		Public Property Currency As String
		Public Property LP As Integer?
		Public Property Jahr As Integer?
		Public Property ClearingNr As Integer?
		Public Property Bank As String
		Public Property KontoNr As String
		Public Property BankOrt As String
		Public Property DTAAdr1 As String
		Public Property DTAAdr2 As String
		Public Property DTAAdr3 As String
		Public Property DTAAdr4 As String
		Public Property IBANNr As String
		Public Property Swift As String
		Public Property BLZ As String
		Public Property Nachname As String
		Public Property Vorname As String
		Public Property Strasse As String
		Public Property PLZ As String
		Public Property Ort As String
		Public Property Land As String
		Public Property EmployeeCountry As String

	End Class

End Namespace
