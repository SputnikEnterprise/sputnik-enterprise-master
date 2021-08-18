Namespace UI

  ''' <summary>
  ''' Inititial ES salary data.
  ''' </summary>
  Public Class InitESSalaryData

    Public Property FarPflichtig As Boolean
    Public Property GrundLohn As Decimal

    Public Property FeiertagBasis As Decimal
    Public Property FeiertagAnsatz As Decimal
    Public Property FeiertagBetrag As Decimal

    Public Property FerienBasis As Decimal
    Public Property FerienAnsatz As Decimal
    Public Property FerienBetrag As Decimal

    Public Property Lohn13Basis As Decimal
    Public Property Lohn13Ansatz As Decimal
    Public Property Lohn13Betrag As Decimal

    Public Property StundenLohn As Decimal
    Public Property Lohnspesen As Decimal
    Public Property TagespesenMA As Decimal

    Public Property Tarif As Decimal
    Public Property TagesspesenKD As Decimal
    Public Property MwStPflicht As Boolean
    Public Property MwStBetrag As Decimal

    Public Property CalcFeierWay As Integer
    Public Property CalcFerienWay As Integer
    Public Property CalcLohn13Way As Integer

    Public Property CustomerKST As Integer?
    Public Property CustomerKSTBez As String

    Public Property LOVon As DateTime?

    Public Property SelectedGAVStringData As GAVStringData
    Public Property EffectiveGAVData As EffectiveGAVData
    Public Property MargeData As MargeStringData

    Public Property DidUserConfirmOverrideOfTagespesen As Boolean

    Public ReadOnly Property IsGAVSelected
      Get
        Return Not SelectedGAVStringData Is Nothing
      End Get
    End Property

  End Class

End Namespace
