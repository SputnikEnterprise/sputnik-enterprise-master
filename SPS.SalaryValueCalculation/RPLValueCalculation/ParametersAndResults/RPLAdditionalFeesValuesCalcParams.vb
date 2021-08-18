Namespace RPLValueCalculation

  Public Class RPLAdditionalFeesValuesCalcParams

#Region "Constructor"

    Public Sub New(ByVal grundLohnForBaseValueCalculation As Decimal,
                   ByVal esParameters As RPLAdditionalFeesValuesCalcESParams,
                   ByVal laParameters As RPLAdditionalFeesValuesCalcLAParams)

      Me.GrundLohnForBaseValueCalculation = grundLohnForBaseValueCalculation
      Me.ESParams = esParameters
      Me.LAParams = laParameters

    End Sub

#End Region

#Region "Public Properties"

    Public Property GrundLohnForBaseValueCalculation As Decimal

    Public Property ESParams As RPLAdditionalFeesValuesCalcESParams
    Public Property LAParams As RPLAdditionalFeesValuesCalcLAParams

#End Region

#Region "Helper Classes"

    ''' <summary>
    ''' The ES parameters
    ''' </summary>
    Class RPLAdditionalFeesValuesCalcESParams

      Public Sub New(ByVal feierProz As Decimal, ferienProz As Decimal?, ByVal lohn13Proz As Decimal,
                     ByVal loFeiertagWay As Byte, ByVal ferienWay As Short, ByVal lo13Way As Short,
                     ByVal hasGAVData As Boolean)

        Me.FeierProz = feierProz
        Me.FerienProz = ferienProz
        Me.Lohn13Proz = lohn13Proz
        Me.HasGAVData = hasGAVData

        Me.LOFeiertagWay = loFeiertagWay
        Me.FerienWay = ferienWay
        Me.LO13Way = lo13Way

      End Sub

      Public Property FeierProz As Decimal
      Public Property FerienProz As Decimal
      Public Property Lohn13Proz As Decimal
      Public Property LOFeiertagWay As Byte
      Public Property FerienWay As Short
      Public Property LO13Way As Short
      Public Property HasGAVData As Boolean

    End Class

    ''' <summary>
    ''' The LA parameters.
    ''' </summary>
    Class RPLAdditionalFeesValuesCalcLAParams

      Public Sub New(ByVal feierInklusiv As Boolean, ByVal ferienInklusiv As Boolean, ByVal inklusiv13 As Boolean)
        Me.FeierInklusiv = feierInklusiv
        Me.FerienInklusiv = ferienInklusiv
        Me.Inklusiv13 = inklusiv13
      End Sub

      Public Property FeierInklusiv As Boolean
      Public Property FerienInklusiv As Boolean
      Public Property Inklusiv13 As Boolean

    End Class

#End Region

  End Class

End Namespace
