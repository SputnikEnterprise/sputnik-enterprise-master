Namespace Report.DataObjects

  Public Class RPLListData

    Public Property ID As Integer
    Public Property RPNr As Integer?
    Public Property RPLNr As Integer?
		Public Property KSTNr As Integer?
		Public Property kstname As String
    Public Property LANr As Decimal?
    Public Property ESLohnNr As Integer?
    Public Property RENr As Integer?
    Public Property RPZusatzText As String
    Public Property Anzahl As Decimal?
    Public Property Basis As Decimal?
    Public Property Ansatz As Decimal?
    Public Property Betrag As Decimal?
    Public Property MWST As Decimal?

		Public Property VonDate As DateTime?
		Public Property BisDate As DateTime?
		Public Property rpltime As String
		Public Property rplkwvon As Integer?
		Public Property rplkwbis As Integer?
		Public Property rplkw As String

		Public Property Sign As String
    Public Property TranslatedLAText As String
    Public Property HasDocument As Boolean
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String

    Public Property Type As RPLType

  End Class

End Namespace
