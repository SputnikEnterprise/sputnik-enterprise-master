Imports SP.DatabaseAccess.PayrollMng.DataObjects

Partial Public Class EmployeePayroll

  Function AddGAVALAToLOL(ByVal rTemprec As LAData)

    Dim i As Integer
		'Dim iFuncValue As Integer
		'Dim cFuncNr As Decimal

    Dim iKantonNr As Integer
    Dim iGruppe1Nr As Integer
		'Dim iAnzBerufe As Integer

    If LPMonth < 4 And LPYear <= 2006 Then Exit Function
    Select Case rTemprec.LANr
      Case 795             ' Gleitzeit, Stunden
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettGleitStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
										Len(aGComplettGleitStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 795.1             ' Gleitzeit, Betrag
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettGleitBetrag.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
									Len(aGComplettGleitBetrag.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 6989             ' Arbeitsstunden von GAV / Kanton
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
									Len(aGComplettStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr, True)     ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7395.1             ' FAR
        For i = 0 To 50
          For iKantonNr = 0 To 26
						If Not String.IsNullOrEmpty(aGComplettFAN.GetData(i, iKantonNr).GAVBeruf) AndAlso Len(aGComplettFAN.GetData(i, iKantonNr).GAVBeruf) > 1 Then
							Call InitGAVAutoLA(rTemprec, i, -1, iKantonNr)       ' Hier wird der Datensatz gespeichert

							If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
												rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
												rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
												rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
								Call GetSum_Var(rTemprec)
							End If
							Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

						End If
					Next iKantonNr
        Next i

      Case 7395.2             ' Bildungsfond prozentual
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettWAN.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
																	Len(aGComplettWAN.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)       ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7395.3             ' Bildungsfond Stundenmässig
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettWANStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
																	Len(aGComplettWANStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7395.4             ' Bildungsfond Monat
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettWANM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
																	Len(aGComplettWANM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7395.5             ' Vollzugskosten prozentual
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettVAN.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
																	Len(aGComplettVAN.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7395.6             ' Vollzugskosten Stundenmässig
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettVANStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
																	Len(aGComplettVANStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)       ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7395.7             ' Vollzugskosten Monat
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
							If Not String.IsNullOrEmpty(aGComplettVANM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) AndAlso
																	Len(aGComplettVANM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
								Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)       ' Hier wird der Datensatz gespeichert

								If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
													rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
													rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
													rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
									Call GetSum_Var(rTemprec)
								End If
								Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

							End If
						Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7400.1, 7420.1            ' KTG, Männer / Frauen
        bIsKTGAsZ = U(4) = 0
        For i = 0 To 50
					If Not String.IsNullOrEmpty(aGComplettKKAN.GetData(i).GAVBeruf) AndAlso Len(aGComplettKKAN.GetData(i).GAVBeruf) > 1 Then
						Call InitGAVAutoLA(rTemprec, i)    ' Hier wird der Datensatz gespeichert

						If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl +
											rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis +
											rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag +
											rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
							Call GetSum_Var(rTemprec)
						End If
						Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

					End If
				Next i





        ' Arbeitgeber Beiträge... ---------------------------------------------------------------------------------------------
      Case 7835.1             ' FAR AG
        For i = 0 To 50
          For iKantonNr = 0 To 26
            If Not String.IsNullOrEmpty(aGComplettFAG.GetData(i, iKantonNr).GAVBeruf) And Len(aGComplettFAG.GetData(i, iKantonNr).GAVBeruf) > 1 Then
              Call InitGAVAutoLA(rTemprec, i, -1, iKantonNr)       ' Hier wird der Datensatz gespeichert

              If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                        rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                        rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                        rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                Call GetSum_Var(rTemprec)
              End If
              Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

            End If
          Next iKantonNr
        Next i

      Case 7835.2             ' Bildungsfond prozentual
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
              If Not String.IsNullOrEmpty(aGComplettWAG.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) And _
                                  Len(aGComplettWAG.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
                Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)       ' Hier wird der Datensatz gespeichert

                If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                          rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                          rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                          rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                  Call GetSum_Var(rTemprec)
                End If
                Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

              End If
            Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7835.3             ' Bildungsfond Stundenmässig
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
              If Not String.IsNullOrEmpty(aGComplettWAGStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) And _
                                  Len(aGComplettWAGStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
                Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

                If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                          rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                          rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                          rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                  Call GetSum_Var(rTemprec)
                End If
                Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

              End If
            Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7835.4             ' Bildungsfond Monat
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
              If Not String.IsNullOrEmpty(aGComplettWAGM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) And _
                                  Len(aGComplettWAGM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
                Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

                If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                          rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                          rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                          rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                  Call GetSum_Var(rTemprec)
                End If
                Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

              End If
            Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7835.5             ' Vollzugskosten prozentual
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
              If Not String.IsNullOrEmpty(aGComplettVAG.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) And _
                                  Len(aGComplettVAG.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
                Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)        ' Hier wird der Datensatz gespeichert

                If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                          rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                          rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                          rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                  Call GetSum_Var(rTemprec)
                End If
                Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

              End If
            Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7835.6             ' Vollzugskosten Stundenmässig
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
              If Not String.IsNullOrEmpty(aGComplettVAGStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) And _
                                  Len(aGComplettVAGStd.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
                Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)       ' Hier wird der Datensatz gespeichert

                If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                          rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                          rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                          rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                  Call GetSum_Var(rTemprec)
                End If
                Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

              End If
            Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7835.7             ' Vollzugskosten Monat
        For i = 0 To 50
          For iKantonNr = 0 To 26
            For iGruppe1Nr = 0 To 26
              If Not String.IsNullOrEmpty(aGComplettVAGM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) And _
                                  Len(aGComplettVAGM.GetData(i, iKantonNr, iGruppe1Nr).GAVBeruf) > 1 Then
                Call InitGAVAutoLA(rTemprec, i, iGruppe1Nr, iKantonNr)       ' Hier wird der Datensatz gespeichert

                If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                          rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                          rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                          rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
                  Call GetSum_Var(rTemprec)
                End If
                Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

              End If
            Next iGruppe1Nr
          Next iKantonNr
        Next i

      Case 7840.1, 7850.1            ' KTG, Männer / Frauen
        For i = 0 To 50
          If Not String.IsNullOrEmpty(aGComplettKKAG.GetData(i).GAVBeruf) And Len(aGComplettKKAG.GetData(i).GAVBeruf) > 1 Then
            Call InitGAVAutoLA(rTemprec, i)        ' Hier wird der Datensatz gespeichert

            If (rTemprec.Sum0Anzahl + rTemprec.Sum1Anzahl + _
                      rTemprec.Sum0Basis + rTemprec.Sum1Basis + rTemprec.Sum2Basis + _
                      rTemprec.SumAnsatz + rTemprec.Sum0Betrag + rTemprec.Sum1Betrag + _
                      rTemprec.Sum2Betrag + rTemprec.Sum3Betrag) <> 0 Then
              Call GetSum_Var(rTemprec)
            End If
            Call GetU_Var(rTemprec, 0, LOLBetrag, "", "A")

          End If
        Next i


    End Select


    Exit Function

  End Function

  Function InitGAVAutoLA(ByVal LArec As LAData, _
                        ByVal iGAVBeruf As Integer, _
                        Optional ByVal iGruppe1Nr As Integer = 0, _
                        Optional ByVal iGAVKanton As Integer = 0, _
                        Optional ByVal bShowMessage As Boolean = False) As Boolean

    Dim w_ValAnz As Decimal
    Dim w_ValBas As Decimal
    Dim w_ValAns As Decimal
    Dim w_ValBtr As Decimal
    Dim cFuncNr As Decimal
    Dim TempZahl As Decimal
    Dim Tempstr As String
    Dim Temp1 As String
    Dim strZGGrund As String
    Dim iBnkNr As Integer
    Dim strMassage As String

    If LPMonth < 4 And LPYear <= 2006 Then Exit Function
    With LArec
      If .TypeAnzahl = 2 Or .TypeAnzahl = 3 Then  ' fester Wert
        w_ValAnz = .FixAnzahl

      ElseIf .TypeAnzahl = 4 Then                      ' S-Variable
        w_ValAnz = S(Val(IIf(Len(.MAAnzVar) > 2, .MAAnzVar * -1, .MAAnzVar))) * _
                    IIf(Len(.MAAnzVar) > 2, -1, 1)

      ElseIf .TypeAnzahl = 5 Then                             ' U-Variable
        w_ValAnz = U(Val(IIf(Len(.MAAnzVar) > 2, .MAAnzVar * -1, .MAAnzVar))) * _
                    IIf(Len(.MAAnzVar) > 2, -1, 1)

      ElseIf .TypeAnzahl = 6 Or .TypeAnzahl = 7 Then     ' S-Variable
        Tempstr = .MAAnzVar
        TempZahl = 0
        Do While InStr(1, Tempstr, "+") <> 0
          Temp1 = Mid(Tempstr, 1, 2)
          TempZahl = TempZahl + IIf(.TypeAnzahl = 6, S(Val(Temp1)), U(Val(Temp1)))
          Tempstr = Mid(Tempstr, 4)
        Loop
        w_ValAnz = TempZahl + IIf(.TypeAnzahl = 6, S(Val(Tempstr)), U(Val(Tempstr)))

      ElseIf .TypeAnzahl = 8 Then  ' U-S Variable
        w_ValAnz = U(2) - S(6)

      ElseIf .TypeAnzahl = 13 Then                                        ' Systemkonstanten
        w_ValAnz = GetLAValue(.LANr)

      ElseIf .TypeAnzahl = 30 Then                      ' Systemprogramme
        If InStr(1, .MAAnzVar, ")") = 0 Then
          cFuncNr = Val(Mid(.MAAnzVar, 1, Len(.MAAnzVar)))
        Else
          cFuncNr = Val(Mid(.MAAnzVar, 1, InStr(1, .MAAnzVar, ")") - 1))
        End If

        w_ValAnz = RunBasFunction(cFuncNr, Val(.MAAnzVar), iGAVBeruf, iGruppe1Nr, iGAVKanton, bShowMessage)

      End If

			' Basiswert errechnen...
			w_ValBas = GetBasValue(.LANr, .TypeBasis, .MABasVar, .FixBasis, iGAVBeruf, iGruppe1Nr, iGAVKanton)

			If .TypeAnsatz = 2 Or .TypeAnsatz = 3 Then      ' fester Wert
        w_ValAns = .FixAnsatz

      ElseIf .TypeAnsatz = 13 Then                                        ' Systemkonstanten
        w_ValAns = GetLAValue(.LANr)

      ElseIf .TypeAnsatz = 30 Then                      ' Systemvariable
        If InStr(1, .MAAnsVar, ")") = 0 Then
          cFuncNr = Val(Mid(.MAAnsVar, 1, Len(.MAAnsVar)))
        Else
          cFuncNr = Val(Mid(.MAAnsVar, 1, InStr(1, .MAAnsVar, ")") - 1)) '    Val(Mid(strMABasVar, 1, 2))
        End If

        w_ValAns = RunBasFunction(cFuncNr, Val(.FixAnsatz), iGAVBeruf, iGruppe1Nr, iGAVKanton)
        If w_ValAns = -1 And (LArec.LANr >= 7600 Or LArec.LANr = 7620) Then
          WriteToProtocol(Padright("*** -> InitGAVAutoLA (" & LArec.LANr & "): ", 30, " ") & "(-1) Fehler in Quellensteuercode...")
          Exit Function
        End If

      End If

      w_ValBtr = Format(NumberRound((w_ValAnz * w_ValBas * w_ValAns) * _
                                      IIf(.Vorzeichen = "-", -1, 1) / 100, _
                                      IIf(.Rundung = 0, 2, .Rundung)), "0.00")

      LOLAnzahl = Val(w_ValAnz)
      LOLBasis = Val(w_ValBas)
      LOLAnsatz = Val(w_ValAns)
      LOLBetrag = Val(w_ValBtr)


      If w_ValBtr = 0 And .WarningByZero Then
        strMassage = "Achtung: Für die Lohnabrechnung " & LONewNr & " existiert kein "
				strMassage = strMassage & LArec.LANr & " (" & LArec.LALoText & ") "
				' TODO:
				'm_TaskHelper.InUIAndWait(Function()
				'                               m_UtilityUI.ShowOKDialog(strMassage, "Betrag = 0", MessageBoxIcon.Exclamation)
				'                               Return True
				'                             End Function)

				WriteToProtocol(Padright("M -> (InitGAVAutoLA): ", 30, " ") & strMassage)

      End If
      If w_ValBtr = 0 And Not .ByNullCreate Then Exit Function
    End With

  Dim lol As New LOLMasterData
    lol.MANR = m_EmployeeData.EmployeeNumber
    lol.LONR = LONewNr
    lol.LANR = LArec.LANr
    lol.M_ANZ = w_ValAnz
    lol.M_BAS = (w_ValBas * IIf(LArec.Vorzeichen = "-", -1, 1))
    lol.M_ANS = w_ValAns
    lol.M_BTR = w_ValBtr
    lol.LOLKst1 = Kostenstelle1
    lol.LOLKst2 = Kostenstelle2
    lol.KST = Kostenstelle3
    lol.LP = LPMonth
    lol.Jahr = LPYear
    lol.ModulName = "A"
    lol.AGLA = IIf(LArec.AGLA, 1S, 0S)

    Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(LArec.LANr, m_EmployeeData.Language, LArec.LALoText)

    If bIsKTGAsZ And (LArec.LANr = 7400.1 Or LArec.LANr = 7420.1 Or LArec.LANr = 7840.1 Or LArec.LANr = 7850.1) Then
      ' KTG60 oder KTG720 Tage
      laTranslation = Replace(laTranslation, "Betrieb", "Büro")
      lol.RPText = laTranslation
    Else
      lol.RPText = laTranslation
    End If
    lol.S_Kanton = strQSTKanton
    lol.QSTGemeinde = String.Empty

    If LArec.LANr = 8730 Then
      lol.LMWithDTA = IIf(GetLMDTAStatus(strZGGrund, iBnkNr), True, False)
      lol.ZGGrund = strZGGrund
      lol.BnkNr = iBnkNr
    ElseIf LArec.LANr = 7400.1 Or LArec.LANr = 7420.1 Or LArec.LANr = 7840.1 Or LArec.LANr = 7850.1 Then
      lol.LMWithDTA = 0
      ' KTG60 oder KTG720 Tage
      lol.ZGGrund = IIf(bIs60KTGDay And bIs60KTGDayOK, "1", "0")
      lol.BnkNr = 0

    Else
      lol.LMWithDTA = 0
      lol.ZGGrund = String.Empty
      lol.BnkNr = 0
    End If
    lol.Currency = m_EmployeeLOSetting.Currency
		lol.GAVNr = aGAVPVLNumber(iGAVBeruf)
		lol.GAV_Kanton = IIf(LArec.LANr <> 7400.1 And LArec.LANr <> 7420.1, aGAVKanton(iGAVKanton), "")
		lol.GAV_Beruf = aGAVBerufe(iGAVBeruf)
		lol.GAV_Gruppe1 = aGGruppe1(Math.Max(0, iGruppe1Nr))
		lol.DateOfLO = LpDate
    lol.MDNr = MDNr

    Dim success = m_PayrollDatabaseAccess.AddNewLOL(lol)
    ThrowExceptionOnError(Not success, "LOL Daten konnten nicht hinzugefügt werden (InitGAVAutoLA).")

    InitGAVAutoLA = True

    Exit Function

  End Function



End Class
