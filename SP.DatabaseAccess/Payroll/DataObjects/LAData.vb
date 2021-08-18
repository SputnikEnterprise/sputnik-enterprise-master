Namespace PayrollMng.DataObjects
  Public Class LAData
    Public Property LANr As Decimal?
    Public Property LALoText As String
    Public Property Bedingung As String
    Public Property Vorzeichen As String
    Public Property Verwendung As String
    Public Property Sum0Anzahl As Short?
    Public Property Sum1Anzahl As Short?
    Public Property Sum0Basis As Short?
    Public Property Sum1Basis As Short?
    Public Property Sum2Basis As Short?
    Public Property SumAnsatz As Short?
    Public Property Sum0Betrag As Short?
    Public Property Sum1Betrag As Short?
    Public Property Sum2Betrag As Short?
    Public Property Sum3Betrag As Short?
    Public Property BruttoPflichtig As Boolean?
    Public Property AHVPflichtig As Boolean?
    Public Property ALVPflichtig As Boolean?
    Public Property NBUVPflichtig As Boolean?
    Public Property UVPflichtig As Boolean?
    Public Property BVGPflichtig As Boolean?
    Public Property KKPflichtig As Boolean?
    Public Property QSTPflichtig As Boolean?
    Public Property Reserve1 As Boolean?
    Public Property Reserve2 As Boolean?
    Public Property Reserve3 As Boolean?
    Public Property Reserve4 As Boolean?
    Public Property Reserve5 As Boolean?
    Public Property FerienInklusiv As Boolean?
    Public Property FeierInklusiv As Boolean?
    Public Property _13Inklusiv As Boolean?
    Public Property ProTag As Boolean?
    Public Property RunFuncBefore As String
    Public Property GleitTime As Boolean?
    Public Property KumLANr As Integer?
    Public Property AGLA As Boolean?
    Public Property Kumulativ As Boolean?
    Public Property KumulativMonth As Boolean?

    Public Property TypeAnzahl As Short?
    Public Property FixAnzahl As Decimal?
    Public Property MAAnzVar As String
    Public Property TypeBasis As Short?
    Public Property FixBasis As Decimal?
    Public Property MABasVar As String
    Public Property TypeAnsatz As Short?
    Public Property FixAnsatz As Decimal?
    Public Property MAAnsVar As String
    Public Property Rundung As Short?
    Public Property WarningByZero As Boolean?
    Public Property ByNullCreate As Boolean?

		Public Property GroupKey As Decimal?

		Public ReadOnly Property DisplayText As String
			Get
				Return String.Format("{0:0.###} - {1}", LANr, LALoText)
			End Get
		End Property

	End Class

End Namespace
