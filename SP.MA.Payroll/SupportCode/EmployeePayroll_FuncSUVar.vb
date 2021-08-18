Imports SP.DatabaseAccess.PayrollMng.DataObjects

Partial Public Class EmployeePayroll

	Private Sub GetSum_Var(ByVal laData As LAData,
						Optional ByVal MitAbzug As Boolean = True,
						Optional ByVal ModulName As String = "Z",
						Optional ByVal bAuslandBank As Boolean = False)
		Dim S0Anz As Integer
		Dim S1Anz As Integer
		Dim S0Bas As Integer
		Dim S1Bas As Integer
		Dim S2Bas As Integer
		Dim SAns As Integer
		Dim S0Btr As Integer
		Dim S1Btr As Integer
		Dim S2Btr As Integer
		Dim S3Btr As Integer

		'If laData.LANr = 7420 Then
		'	Trace.WriteLine(laData.LANr)
		'End If
		'Trace.WriteLine(String.Format("{0} >>> 1. s(2) = {1}", laData.LANr, S(2)))

		' Variable von Anzahl berechnen
		If laData.Sum0Anzahl <> 0 Then
			If ModulName = "ZG" Then
				If MitAbzug Then
					If Not (laData.LANr = 8920 AndAlso bAuslandBank) Then
						S(laData.Sum0Anzahl) += 1
					End If
					'If Not bAuslandBank Then S(laData.Sum0Anzahl) += 1
					'S(laData.Sum0Anzahl) += 1
				End If

				'	If laData.LANr = 8900 OrElse laData.LANr = 8930 Then
				'		S(laData.Sum0Anzahl) += 1
				'	Else
				'		S(laData.Sum0Anzahl) += 1
				'		'If Not bAuslandBank Then S(laData.Sum0Anzahl) += 1
				'	End If
				'End If

			Else
				If laData.Sum0Anzahl.Value < 0 Then
					S0Anz = Math.Abs(laData.Sum0Anzahl.Value)
					S(S0Anz) = S(S0Anz) - LOLAnzahl

				Else
					S(laData.Sum0Anzahl) = S(laData.Sum0Anzahl) + LOLAnzahl

				End If

			End If
		End If

		If laData.Sum1Anzahl <> 0 Then
			If laData.Sum1Anzahl.Value < 0 Then
				S0Anz = Math.Abs(laData.Sum1Anzahl.Value)
				S(S1Anz) = S(S1Anz) - LOLAnzahl

			Else
				S(laData.Sum1Anzahl) = S(laData.Sum1Anzahl) + LOLAnzahl
			End If

			If ModulName = "ZG" Then
				If MitAbzug And Not bAuslandBank Then S(laData.Sum1Anzahl) = S(laData.Sum1Anzahl) + 1
			End If
		End If

		' Variable von Basis berechnen
		If laData.Sum0Basis <> 0 Then
			If laData.Sum0Basis.Value < 0 Then
				S0Bas = Math.Abs(laData.Sum0Basis.Value)
				S(S0Bas) = S(S0Bas) - LOLBasis

			Else
				S(laData.Sum0Basis) = S(laData.Sum0Basis) + LOLBasis

			End If
		End If

		If laData.Sum1Basis <> 0 Then
			If laData.Sum1Basis.Value < 0 Then
				S1Bas = Math.Abs(laData.Sum1Basis.Value)
				S(S1Bas) = S(S1Bas) - LOLBasis

			Else
				S(laData.Sum1Basis) = S(laData.Sum1Basis) + LOLBasis

			End If
		End If
		If laData.Sum2Basis <> 0 Then
			If laData.Sum2Basis.Value < 0 Then
				S2Bas = Math.Abs(laData.Sum2Basis.Value)
				S(S2Bas) = S(S2Bas) - LOLBasis

			Else
				S(laData.Sum2Basis) = S(laData.Sum2Basis) + LOLBasis

			End If
		End If

		' Variable von Ansatz berechnen
		If laData.SumAnsatz <> 0 Then
			If laData.SumAnsatz.Value < 0 Then
				SAns = Math.Abs(laData.SumAnsatz.Value)
				S(SAns) = S(SAns) - LOLAnsatz

			Else
				S(laData.SumAnsatz) = S(laData.SumAnsatz) + LOLAnsatz

			End If
		End If

		' Variable von Betrag berechnen
		If laData.Sum0Betrag <> 0 Then
			If laData.Sum0Betrag.Value < 0 Then
				S0Btr = Math.Abs(laData.Sum0Betrag.Value)
				S(S0Btr) = S(S0Btr) - LOLBetrag

			Else
				S(laData.Sum0Betrag) = S(laData.Sum0Betrag) + LOLBetrag

			End If
		End If

		If laData.Sum1Betrag <> 0 Then
			If laData.Sum1Betrag.Value < 0 Then
				S1Btr = Math.Abs(laData.Sum1Betrag.Value)
				S(S1Btr) = S(S1Btr) - LOLBetrag

			Else
				S(laData.Sum1Betrag) = S(laData.Sum1Betrag) + LOLBetrag

			End If
		End If

		If laData.Sum2Betrag <> 0 Then
			If laData.Sum2Betrag.Value < 0 Then
				S2Btr = Math.Abs(laData.Sum2Betrag.Value)
				S(S2Btr) = S(S2Btr) - LOLBetrag

			Else
				S(laData.Sum2Betrag) = S(laData.Sum2Betrag) + LOLBetrag

			End If
		End If
		If laData.Sum3Betrag <> 0 Then
			If laData.Sum3Betrag.Value < 0 Then
				S3Btr = Math.Abs(laData.Sum3Betrag.Value)
				S(S3Btr) = S(S3Btr) - LOLBetrag

			Else
				S(laData.Sum3Betrag) = S(laData.Sum3Betrag) + LOLBetrag

			End If
		End If

		'Trace.WriteLine(String.Format("{0} >>> 2. s(2) = {1}", laData.LANr, S(2)))

	End Sub

	Private Sub GetU_Var(ByVal laData As LAData,
					 ByVal cTempAnz As Decimal,
					 ByVal Tempwert As Decimal,
					 ByVal SUVACode As String,
					 ByVal ModulName As String,
					 Optional ByVal Far As Boolean = False)

		If String.IsNullOrEmpty(SUVACode) Then SUVACode = If(String.IsNullOrWhiteSpace(TempSuvaCode), "A1", TempSuvaCode)

		If laData.LANr = 530 OrElse laData.LANr = 630 OrElse laData.LANr = 730 OrElse (laData.LANr = 560 OrElse laData.LANr = 660 OrElse laData.LANr = 760) Then
			'Trace.WriteLine(laData.LANr)
		End If

		With laData
			If .BruttoPflichtig Then U(1) = Tempwert + U(1)
			If .AHVPflichtig Then
				U(2) = Tempwert + U(2)
				' Für die Zusatzlohnarten für Bau-Branche
				If Far Then U(30) = Tempwert + U(30)
			End If
			If .ALVPflichtig Then U(3) = Tempwert + U(3)

			' SUVA-Code auswerten. Die Abfrage habe ich rausgenommen
			' wegen Rückstellung Ferien und Feiertag...
			'  If LArec!Verwendung <> "0" Or (Not !NBUVPflichtig And Not !UVPflichtig And Not !KKPflichtig) Then
			If .NBUVPflichtig Then                                   ' Arbeitnehmer
				U(21) = Tempwert + U(21)                               ' SUVA(A+Z)
				If Left(SUVACode, 1) = "Z" Then                        ' Büro
					U(5) = Tempwert + U(5)                               ' SUVA(A+Z)

					If Right(SUVACode, 1) = "0" Then                     ' Z0
						U(24) = Tempwert + U(24)

					ElseIf Right(SUVACode, 1) = "1" Then                 ' Z1
						U(11) = Tempwert + U(11)

					ElseIf Right(SUVACode, 1) = "2" Then                 ' Z2
						U(18) = Tempwert + U(18)

					ElseIf Right(SUVACode, 1) = "3" Then                 ' Z3
						U(25) = Tempwert + U(25)

					End If

				ElseIf Left(SUVACode, 1) = "A" Then                    ' Betrieb
					U(4) = U(4) + Tempwert

					If Right(SUVACode, 1) = "0" Then    ' A0
						U(22) = Tempwert + U(22)

					ElseIf Right(SUVACode, 1) = "1" Then                 ' A1
						U(10) = Tempwert + U(10)

					ElseIf Right(SUVACode, 1) = "2" Then                 ' A2
						U(17) = Tempwert + U(17)

					ElseIf Right(SUVACode, 1) = "3" Then                 ' A3
						U(23) = Tempwert + U(23)
					End If

				End If

			End If

			If .UVPflichtig Then                            ' BUV-pflichtig (Arbeitgeber)
				If Left(SUVACode, 1) = "Z" Then                         ' Büro
					If Val(Right(SUVACode, 1)) > 0 Then                     ' Z1, 2, 3
						U(20) = Tempwert + U(20)
					End If

				ElseIf Left(SUVACode, 1) = "A" Then                     ' Betrieb
					If Val(Right(SUVACode, 1)) > 0 Then                     ' A1, 2, 3
						U(19) = Tempwert + U(19)
					End If

				End If
			End If

			' Krankentaggelder
			If .KKPflichtig Then
				If Left(SUVACode, 1) = "Z" Then
					U(7) = Tempwert + U(7)

				ElseIf Left(SUVACode, 1) = "A" Then
					U(6) = Tempwert + U(6)
				End If

			End If

			'  End If

			If .BVGPflichtig Then
				U(8) = Tempwert + U(8)

				' Monatsvariablenbasis
				If Trim(ModulName) = "L" Then U(34) = Tempwert + U(34)

			End If

			If .QSTPflichtig Then U(9) = Tempwert + U(9)
			If .Reserve1 Then U(12) = Tempwert + U(12)
			If .Reserve2 Then U(13) = Tempwert + U(13)
			If .Reserve3 Then U(14) = Tempwert + U(14)
			If .Reserve4 Then U(15) = Tempwert + U(15)
			If .Reserve5 Then U(16) = Tempwert + U(16)

		End With

		If ModulName = "R" Then
			GetRPGAV_U_Var(laData, cTempAnz, Tempwert, SUVACode, ModulName)

		ElseIf ModulName = "L" Then
			Call GetLMGAV_U_Var(laData, cTempAnz, Tempwert, SUVACode, ModulName)

		ElseIf ModulName = "A" AndAlso (laData.LANr = 530 OrElse laData.LANr = 630 OrElse laData.LANr = 730) Then   ' Or LArec!LANr = 795.1) Then
			GetVarKorrektur(laData, Tempwert, SUVACode, ModulName)

		End If

	End Sub

	Private Sub BlankSUVars()

		' FAR ist von AHV-pflichtigen Lohn
		cFAGBasis = 0
		cFANBasis = 0
		' Parifond ist von SUVA-pflichtigen Lohn
		cWAGBasis = 0
		cWANBasis = 0
		cVAGBasis = 0
		cVANBasis = 0
		cGAVKTGAGBasis = 0
		cGAVKTGANBasis = 0
		'cGAVKTGAGBasis60 = 0
		'cGAVKTGANBasis60 = 0

		For i = 0 To 70
			S(i) = 0
			G(i) = 0
		Next i

		For i = 0 To 10
			Div(i) = 0
		Next i

		For i = 0 To 50
			U(i) = 0

			aGAVFAG(i) = 0
			aGAVFAN(i) = 0
			aGAVWAG(i) = 0
			aGAVWAN(i) = 0
			aGAVVAG(i) = 0
			aGAVVAN(i) = 0
			aGAVKKAG(i) = 0
			aGAVKKAN(i) = 0

			aFeiertag(i) = 0
			aFerien(i) = 0
			a13Lohn(i) = 0
			aGAVPVLNumber(i) = 0
			aGAVBerufe(i) = ""
		Next

		' Reset 
		Try
			aGComplettStd = New GAVDataPerNumberCantonAndGroup
			aGComplettFAG = New GAVDataPerNumberAndCanton
			aGComplettFAN = New GAVDataPerNumberAndCanton
			aGComplettWAG = New GAVDataPerNumberCantonAndGroup
			aGComplettWAN = New GAVDataPerNumberCantonAndGroup
			aGComplettVAG = New GAVDataPerNumberCantonAndGroup
			aGComplettVAN = New GAVDataPerNumberCantonAndGroup

			aGComplettWAGStd = New GAVDataPerNumberCantonAndGroup
			aGComplettWANStd = New GAVDataPerNumberCantonAndGroup
			aGComplettVAGStd = New GAVDataPerNumberCantonAndGroup
			aGComplettVANStd = New GAVDataPerNumberCantonAndGroup

			aGComplettWAGM = New GAVDataPerNumberCantonAndGroup
			aGComplettWANM = New GAVDataPerNumberCantonAndGroup
			aGComplettVAGM = New GAVDataPerNumberCantonAndGroup
			aGComplettVANM = New GAVDataPerNumberCantonAndGroup

			aGComplettGleitBetrag = New GAVDataPerNumberCantonAndGroup
			aGComplettGleitStd = New GAVDataPerNumberCantonAndGroup

		Catch ex As System.OutOfMemoryException
			ThrowExceptionOnError(True, String.Format("OutOfMemoryException: {0}.", ex.ToString))
		Catch ex As Exception
			ThrowExceptionOnError(True, String.Format("BlankSUVars: {0}.", ex.ToString))

		End Try

		aGComplettKKAG = New GAVDataPerNumber
		aGComplettKKAN = New GAVDataPerNumber

		aGAVStdAnzGleit = New GleizeitDataPerGAVNumber

	End Sub

	Private Function RepeatLA4LOBack(ByVal lFromLANr As Decimal, ByVal lNewLANr As Decimal,
								ByVal iAGLA As Integer,
								ByVal strLAName As String)

		Dim rTemprec As LOLDataFoRepeatLA4LOBack

		Dim lolList = m_PayrollDatabaseAccess.LoadLOLDataForRepeatLA4LOBack(m_EmployeeData.EmployeeNumber, MDNr, LONewNr, lFromLANr)
		ThrowExceptionOnError(lolList Is Nothing, "LOL Daten konnten nicht geladen werden (RepeatLA4LOBack).")


		For Each rTemprec In lolList

			Dim lol As New LOLMasterData
			With rTemprec
				lol.MANR = m_EmployeeData.EmployeeNumber
				lol.LONR = LONewNr
				lol.LANR = lNewLANr
				lol.LOLKst1 = .LOLKst1
				lol.LOLKst2 = .LOLKst2
				lol.KST = .Kst
				lol.M_ANZ = .m_Anz
				lol.M_BAS = .m_Bas * -1
				lol.M_ANS = .m_Ans
				lol.M_BTR = .m_Btr * -1
				lol.LP = LPMonth
				lol.Jahr = LPYear
				lol.ModulName = "A"
				lol.AGLA = iAGLA
				lol.SUVA = IIf(String.IsNullOrWhiteSpace(.Suva), "A1", .Suva)
				lol.RPText = strLAName
				lol.KW = .KW
				lol.KW2 = .KW2
				lol.DestRPNr = .DestRPNr
				lol.DestESNr = .DestESNr
				lol.GAVNr = .GAVNr
				lol.GAV_Kanton = .GAV_Kanton
				lol.GAV_Beruf = .GAV_Beruf
				lol.GAV_Gruppe1 = .GAV_Gruppe1
				lol.GAV_Gruppe2 = .GAV_Gruppe2
				lol.GAV_Gruppe3 = .GAV_Gruppe3
				lol.GAV_Text = .GAV_Text
				lol.ESEinstufung = .ESEinstufung
				lol.ESBranche = .ESBranche
				lol.DateOfLO = LpDate
				lol.MDNr = MDNr
			End With

			Dim success = m_PayrollDatabaseAccess.AddNewLOL(lol)
			ThrowExceptionOnError(Not success, "LOL Daten konnten nicht hinzugefügt werden (RepeatLA4LOBack).")

		Next

	End Function

	Function GetKumWert(ByVal LAOrgrec As LAData)

		If LAOrgrec.Kumulativ Then

			Dim yearCumulativeData = New YearCumulativeData
			yearCumulativeData.MANummer = m_EmployeeData.EmployeeNumber
			yearCumulativeData.LONewNr = LONewNr
			yearCumulativeData.LANr = LAOrgrec.LANr
			yearCumulativeData.KumLANr = LAOrgrec.KumLANr
			yearCumulativeData.Kst1 = Kostenstelle1
			yearCumulativeData.Kst2 = Kostenstelle2
			yearCumulativeData.LP = LPMonth
			yearCumulativeData.MDYear = LPYear
			yearCumulativeData.ModulName = "A"
			yearCumulativeData.AGLA = IIf(LAOrgrec.AGLA, 1S, 0S)
			yearCumulativeData.DateOfLO = LpDate
			yearCumulativeData.MDNr = MDNr

			Dim success = m_PayrollDatabaseAccess.AddYearCumulativeData(yearCumulativeData)
			ThrowExceptionOnError(Not success, "Jahr Kumulativ Daten konnten nicht hinzugefügt werden.")

		ElseIf LAOrgrec.KumulativMonth Then

			Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(LAOrgrec.LANr, m_EmployeeData.Language, LAOrgrec.LALoText)

			Dim monthCumulativeData = New MonthCumulativeData
			monthCumulativeData.MANummer = m_EmployeeData.EmployeeNumber
			monthCumulativeData.LONewNr = LONewNr
			monthCumulativeData.LANr = LAOrgrec.LANr
			monthCumulativeData.KumLANr = LAOrgrec.KumLANr
			monthCumulativeData.Kst1 = Kostenstelle1
			monthCumulativeData.Kst2 = Kostenstelle2
			monthCumulativeData.LP = LPMonth
			monthCumulativeData.MDYear = LPYear
			monthCumulativeData.ModulName = "A"
			monthCumulativeData.AGLA = IIf(LAOrgrec.AGLA, 1S, 0S)
			monthCumulativeData.LAName = laTranslation
			monthCumulativeData.DateOfLO = LpDate
			monthCumulativeData.MDNr = MDNr

			Dim success = m_PayrollDatabaseAccess.AddMonthCumulativeData(monthCumulativeData)
			ThrowExceptionOnError(Not success, "Monat Kumulativ Daten konnten nicht hinzugefügt werden.")

		End If

	End Function

End Class
