Namespace Report.DataObjects

  Public Class ESSalaryData

    Public Property ID As Integer
		Public Property ESLohnNr As Integer?
    Public Property KSTNr As Integer?
    Public Property GrundLohn As Decimal?
    Public Property StundenLohn As Decimal?
    Public Property Tarif As Decimal?
    Public Property MWStBetrag As Decimal?
		Public Property CustomerMwStPflicht As Boolean?
		Public Property LOVon As DateTime?
		Public Property LOBis As DateTime?
		Public Property Createdon As DateTime?
		Public Property AktivLODaten As Boolean?
    Public Property FeierProz As Decimal?
    Public Property FerienProz As Decimal?
    Public Property Lohn13Proz As Decimal?
    Public Property GAVText As String
    Public Property GAVNr As Integer?
    Public Property LOFeiertagWay As Byte?
    Public Property FerienWay As Short?
    Public Property LO13Way As Short?
    Public Property MAStdSpesen As Decimal?
    Public Property MATSpesen As Decimal?
    Public Property KDTSpesen As Decimal?
    Public Property IsPVL As Byte?
		Public Property GAVInfo_String As String

		Public ReadOnly Property HasGavData As Boolean
      Get
        Return GAVNr.HasValue AndAlso GAVNr > 0
      End Get
    End Property

		Public ReadOnly Property ESLohnIsMwStPflichtig As Boolean
			Get
				Dim result As Boolean = True

				If CustomerMwStPflicht.GetValueOrDefault(False) Then
					If Tarif.GetValueOrDefault(0) > 0 AndAlso MWStBetrag = 0 Then
						result = False
					End If

				Else
					If MWStBetrag.GetValueOrDefault(0) = 0 Then
						result = False
					End If

				End If

				Return result
			End Get
		End Property


	End Class


	Public Class ESData
		Public Property ID As Integer
		Public Property ESNr As Integer
		Public Property MANr As Integer
		Public Property KDNr As Integer

		Public Property esAb As Date?
		Public Property esEnde As Date?

		Public Property eskst1 As String
		Public Property eskst2 As String
		Public Property eskst As String
		Public Property suva As String
		Public Property esbranche As String

	End Class

End Namespace
