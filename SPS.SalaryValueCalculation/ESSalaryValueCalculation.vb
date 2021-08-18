
''' <summary>
''' Calculates salary values for ES (Einsatz).
''' </summary>
Public Class ESSalaryValueCalculation

#Region "Public Methods"

  Public Function CalculateGrundLohnWithoutGAV(ByVal hourlyEarnings As Decimal,
                                                           ByVal feiertagAnsatz As Decimal,
                                                           ByVal ferienAnsatz As Decimal,
                                                           ByVal lohn13Ansatz As Decimal,
                                                           ByVal calcFerienWay As Integer,
                                                           ByVal calcLohnWay As Integer) As Decimal

    Dim pFeiertagBasis As Decimal
    Dim pFerienBasis As Decimal
    Dim pLohn13Basis As Decimal
    Dim pFeiertagAbs As Decimal
    Dim pFerienAbs As Decimal
    Dim pLohn13Abs As Decimal

    CalcBaseAndAbsRatioWithoutGAV(feiertagAnsatz,
                               ferienAnsatz,
                               lohn13Ansatz,
                               calcFerienWay,
                               calcLohnWay,
                               pFeiertagBasis,
                               pFerienBasis,
                               pLohn13Basis,
                               pFeiertagAbs,
                               pFerienAbs,
                               pLohn13Abs)

    Dim pTotalAbs As Decimal = 1D + pFeiertagAbs + pFerienAbs + pLohn13Abs
    Dim grundlohn As Decimal = hourlyEarnings / pTotalAbs
    Return grundlohn

  End Function

  Public Function CalculateFerFeier13BasisWithoutGAV(ByVal grundlohn As Decimal,
                                                           ByVal feiertagAnsatz As Decimal,
                                                           ByVal ferienAnsatz As Decimal,
                                                           ByVal lohn13Ansatz As Decimal,
                                                           ByVal calcFerienWay As Integer,
                                                           ByVal calcLohnWay As Integer) As CalcFerFeier13BasisResult

    Dim pFeiertagBasis As Decimal
    Dim pFerienBasis As Decimal
    Dim pLohn13Basis As Decimal
    Dim pFeiertagAbs As Decimal
    Dim pFerienAbs As Decimal
    Dim pLohn13Abs As Decimal

    CalcBaseAndAbsRatioWithoutGAV(feiertagAnsatz,
                               ferienAnsatz,
                               lohn13Ansatz,
                               calcFerienWay,
                               calcLohnWay,
                               pFeiertagBasis,
                               pFerienBasis,
                               pLohn13Basis,
                               pFeiertagAbs,
                               pFerienAbs,
                               pLohn13Abs)

    Dim result As New CalcFerFeier13BasisResult With {
      .GrundLohn = grundlohn,
      .FeiertagBetrag = grundlohn * pFeiertagAbs,
      .FerienBetrag = grundlohn * pFerienAbs,
      .Lohn13Betrag = grundlohn * pLohn13Abs,
      .FeiertagBasis = grundlohn * pFeiertagBasis,
      .FerienBasis = grundlohn * pFerienBasis,
      .Lohn13Basis = grundlohn * pLohn13Basis
    }
    Return result

  End Function

  Public Function CalculateGrundLohnWithGAV(ByVal hourlyEarnings As Decimal,
                                                          ByVal feiertagAnsatz As Decimal,
                                                          ByVal ferienAnsatz As Decimal,
                                                          ByVal lohn13Ansatz As Decimal,
                                                          ByVal calcFeierWay As Integer,
                                                          ByVal calcFerienWay As Integer,
                                                          ByVal calcLohnWay As Integer) As Decimal

    Dim pFeiertagBasis As Decimal
    Dim pFerienBasis As Decimal
    Dim pLohn13Basis As Decimal
    Dim pFeiertagAbs As Decimal
    Dim pFerienAbs As Decimal
    Dim pLohn13Abs As Decimal

    CalcBaseAndAbsRatioWithGAV(feiertagAnsatz,
                               ferienAnsatz,
                               lohn13Ansatz,
                               calcFeierWay,
                               calcFerienWay,
                               calcLohnWay,
                               pFeiertagBasis,
                               pFerienBasis,
                               pLohn13Basis,
                               pFeiertagAbs,
                               pFerienAbs,
                               pLohn13Abs)

    Dim pTotalAbs As Decimal = 1D + pFeiertagAbs + pFerienAbs + pLohn13Abs
    Dim grundlohn As Decimal = hourlyEarnings / pTotalAbs
    Return grundlohn

  End Function

  Public Function CalcFerFeier13BasisWithGAV(ByVal grundLohn As Decimal,
                                                          ByVal feiertagAnsatz As Decimal,
                                                          ByVal ferienAnsatz As Decimal,
                                                          ByVal lohn13Ansatz As Decimal,
                                                          ByVal calcFeierWay As Integer,
                                                          ByVal calcFerienWay As Integer,
                                                          ByVal calcLohnWay As Integer) As CalcFerFeier13BasisResult

    Dim pFeiertagBasis As Decimal
    Dim pFerienBasis As Decimal
    Dim pLohn13Basis As Decimal
    Dim pFeiertagAbs As Decimal
    Dim pFerienAbs As Decimal
    Dim pLohn13Abs As Decimal

    CalcBaseAndAbsRatioWithGAV(feiertagAnsatz,
                               ferienAnsatz,
                               lohn13Ansatz,
                               calcFeierWay,
                               calcFerienWay,
                               calcLohnWay,
                               pFeiertagBasis,
                               pFerienBasis,
                               pLohn13Basis,
                               pFeiertagAbs,
                               pFerienAbs,
                               pLohn13Abs)

    Dim result As New CalcFerFeier13BasisResult With {
      .GrundLohn = grundLohn,
      .FeiertagBetrag = grundLohn * pFeiertagAbs,
      .FerienBetrag = grundLohn * pFerienAbs,
      .Lohn13Betrag = grundLohn * pLohn13Abs,
      .FeiertagBasis = grundLohn * pFeiertagBasis,
      .FerienBasis = grundLohn * pFerienBasis,
      .Lohn13Basis = grundLohn * pLohn13Basis
    }
    Return result

  End Function

#End Region

#Region "Private Shared Methods"

  ''' <summary>
  ''' Berechnet Basis und absolute Prozentsätze der einzelnen Lohnbestandteile (ohne GAV).
  ''' </summary>
  ''' <param name="feiertagAnsatz"></param>
  ''' <param name="ferienAnsatz"></param>
  ''' <param name="lohn13Ansatz"></param>
  ''' <param name="calcFerienWay"></param>
  ''' <param name="calcLohnWay"></param>
  ''' <param name="pFeiertagBasis">Basis Prozentsatz bezüglich Grundlohn für Feiertags-Entschädigung</param>
  ''' <param name="pFerienBasis">Basis Prozentsatz bezüglich Grundlohn für Ferien-Entschädigung</param>
  ''' <param name="pLohn13Basis">Basis Prozentsatz bezüglich Grundlohn für 13. Monatslohn-Entschädigung</param>
  ''' <param name="pFeiertagAbs">Prozentsatz bezüglich Grundlohn der Feiertags-Entschädigung</param>
  ''' <param name="pFerienAbs">Prozentsatz bezüglich Grundlohn der Ferien-Entschädigung</param>
  ''' <param name="pLohn13Abs">Prozentsatz bezüglich Grundlohn der 13. Monatslohn-Entschädigung</param>
  ''' <remarks></remarks>
  Private Shared Sub CalcBaseAndAbsRatioWithoutGAV(ByVal feiertagAnsatz As Decimal,
                                                     ByVal ferienAnsatz As Decimal,
                                                     ByVal lohn13Ansatz As Decimal,
                                                     ByVal calcFerienWay As Integer,
                                                     ByVal calcLohnWay As Integer,
                                                     ByRef pFeiertagBasis As Decimal,
                                                     ByRef pFerienBasis As Decimal,
                                                     ByRef pLohn13Basis As Decimal,
                                                     ByRef pFeiertagAbs As Decimal,
                                                     ByRef pFerienAbs As Decimal,
                                                     ByRef pLohn13Abs As Decimal
                                                     )

    ' Prozentsätze berechnen
    Dim pFeiertag As Decimal = feiertagAnsatz / 100
    Dim pFerien As Decimal = ferienAnsatz / 100
    Dim pLohn13 As Decimal = lohn13Ansatz / 100

    ' Feiertagsentschädigung - immer Grundlohn als Basis
    pFeiertagBasis = 1D
    pFeiertagAbs = pFeiertagBasis * pFeiertag

    ' Ferienentschädigung
    Select Case calcFerienWay
      Case 1
        ' Grundlohn als Basis
        pFerienBasis = 1D
      Case Else
        ' Grundlohn + Feiertag als Basis
        pFerienBasis = 1D + pFeiertagAbs
    End Select
    pFerienAbs = pFerienBasis * pFerien

    ' 13. Monatslohn
    Select Case calcLohnWay
      Case 0
        ' Grundlohn als Basis
        pLohn13Basis = 1D
      Case 1
        ' Grundlohn + Feiertag als Basis
        pLohn13Basis = 1D + pFeiertagAbs
      Case 2
        ' Grundlohn + Feiertag + Ferien als Basis
        pLohn13Basis = 1D + pFeiertagAbs + pFerienAbs
      Case 3
        ' Grundlohn + Ferien als Basis
        pLohn13Basis = 1D + pFerienAbs
      Case Else
        ' Falsche Eingabe -> wie 2
        ' Grundlohn + Feiertag + Ferien als Basis
        pLohn13Basis = 1D + pFeiertagAbs + pFerienAbs
    End Select
    pLohn13Abs = pLohn13Basis * pLohn13

  End Sub

  ''' <summary>
  ''' Berechnet Basis und absolute Prozentsätze der einzelnen Lohnbestandteile (mit GAV).
  ''' </summary>
  ''' <param name="feiertagAnsatz"></param>
  ''' <param name="ferienAnsatz"></param>
  ''' <param name="lohn13Ansatz"></param>
  ''' <param name="calcFeierWay"></param>
  ''' <param name="calcFerienWay"></param>
  ''' <param name="calcLohnWay"></param>
  ''' <param name="pFeiertagBasis">Basis Prozentsatz bezüglich Grundlohn für Feiertags-Entschädigung</param>
  ''' <param name="pFerienBasis">Basis Prozentsatz bezüglich Grundlohn für Ferien-Entschädigung</param>
  ''' <param name="pLohn13Basis">Basis Prozentsatz bezüglich Grundlohn für 13. Monatslohn-Entschädigung</param>
  ''' <param name="pFeiertagAbs">Prozentsatz bezüglich Grundlohn der Feiertags-Entschädigung</param>
  ''' <param name="pFerienAbs">Prozentsatz bezüglich Grundlohn der Ferien-Entschädigung</param>
  ''' <param name="pLohn13Abs">Prozentsatz bezüglich Grundlohn der 13. Monatslohn-Entschädigung</param>
  ''' <remarks></remarks>
  Private Shared Sub CalcBaseAndAbsRatioWithGAV(ByVal feiertagAnsatz As Decimal,
                                                     ByVal ferienAnsatz As Decimal,
                                                     ByVal lohn13Ansatz As Decimal,
                                                     ByVal calcFeierWay As Integer,
                                                     ByVal calcFerienWay As Integer,
                                                     ByVal calcLohnWay As Integer,
                                                     ByRef pFeiertagBasis As Decimal,
                                                     ByRef pFerienBasis As Decimal,
                                                     ByRef pLohn13Basis As Decimal,
                                                     ByRef pFeiertagAbs As Decimal,
                                                     ByRef pFerienAbs As Decimal,
                                                     ByRef pLohn13Abs As Decimal
                                                     )

    ' Prozentsätze berechnen
    Dim pFeiertag As Decimal = feiertagAnsatz / 100
    Dim pFerien As Decimal = ferienAnsatz / 100
    Dim pLohn13 As Decimal = lohn13Ansatz / 100

    ' Feiertagsentschädigung
    Select Case calcFeierWay
      Case 1
        ' Grundlohn als Basis
        pFeiertagBasis = 1D
      Case 2
        ' Grundlohn + Ferien als Basis
        pFeiertagBasis = IIf(calcFerienWay > 0, 1D + pFerien, 1D)
      Case Else
        ' Keine Entschädigung
        pFeiertagBasis = 0D
    End Select
    pFeiertagAbs = pFeiertagBasis * pFeiertag

    ' Ferienentschädigung
    Select Case calcFerienWay
      Case 1
        ' Grundlohn als Basis
        pFerienBasis = 1D
      Case 3
        ' Grundlohn + Feiertag als Basis
        pFerienBasis = 1D + pFeiertagAbs
      Case Else
        ' Keine Entschädigung
        pFerienBasis = 0D
    End Select
    pFerienAbs = pFerienBasis * pFerien

    ' 13. Monatslohn
    Select Case calcLohnWay
      Case 1
        ' Grundlohn als Basis
        pLohn13Basis = 1D
      Case 2
        ' Grundlohn + Ferien als Basis
        pLohn13Basis = 1D + pFerienAbs
      Case 3
        ' Grundlohn + Feiertag als Basis
        pLohn13Basis = 1D + pFeiertagAbs
      Case 4
        ' Grundlohn + Feiertag + Ferien als Basis
        pLohn13Basis = 1D + pFeiertagAbs + pFerienAbs
      Case Else
        ' Keine Entschädigung
        pLohn13Basis = 0D
    End Select
    pLohn13Abs = pLohn13Basis * pLohn13

  End Sub

#End Region

#Region "Support Classes"

  Public Class CalcFerFeier13BasisResult

    Public Property GrundLohn As Decimal

    Public Property FeiertagBetrag As Decimal
    Public Property FerienBetrag As Decimal
    Public Property Lohn13Betrag As Decimal

    Public Property FeiertagBasis As Decimal
    Public Property FerienBasis As Decimal
    Public Property Lohn13Basis As Decimal

    Public ReadOnly Property BetragSumme As Decimal
      Get
        Return GrundLohn + FeiertagBetrag + FerienBetrag + Lohn13Betrag
      End Get
    End Property

  End Class

#End Region

End Class
