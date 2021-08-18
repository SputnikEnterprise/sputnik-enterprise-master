Namespace ES.DataObjects.ESMng

  ''' <summary>
  ''' LA Data.
  ''' </summary>
  Public Class LAData
    Public Property LANr As Decimal?
    Public Property LALoText As String
    Public Property Sign As String
    Public Property AllowMoreAnzahl As Boolean
    Public Property AllowMoreBasis As Boolean
    Public Property AllowMoreAnsatz As Boolean
    Public Property AllowMoreBetrag As Boolean
    Public Property Rounding As Short?
    Public Property TypeAnzahl As Short?
    Public Property TypeBasis As Short?
    Public Property TypeAnsatz As Short?

		Public Property MABasVar As String
		Public Property KDBasis As String

		Public Property FixAnzahl As Decimal?
		Public Property FixBasis As Decimal?
		Public Property FixAnsatz As Decimal?
		Public Property MWSTPflichtig As Boolean

    Public ReadOnly Property DisplayText As String
      Get
        Return String.Format("{0:0.###} - {1}", LANr, LALoText)
      End Get
    End Property

  End Class

End Namespace
