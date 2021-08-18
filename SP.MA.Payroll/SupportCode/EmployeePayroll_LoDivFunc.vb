Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenIndividuell
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenProLohnabrechnung
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLohn


Partial Public Class EmployeePayroll

	Private Function ToYoung(ByVal MAGebDat As Date) As Boolean
		Dim result As Boolean = False

		If LPYear - Year(MAGebDat) < m_MandantData.MindestAlter Then result = True


		Return result

	End Function

	Private Function GetNBUVBasis()

		' Suva-Lohn Z
		Dim HLTEs As Decimal
		Dim SLK As Decimal
		Dim MaxHL As Decimal                         '
		Dim UVAGZ As Decimal
		Dim UVAGA As Decimal
		Dim NBUVANZ As Decimal
		Dim NBUVANA As Decimal
		Dim NBUVAGZ As Decimal
		Dim NBUVAGA As Decimal
		Dim X10 As Decimal
		Dim X11 As Decimal
		Dim X17 As Decimal
		Dim X18 As Decimal
		Dim X23 As Decimal
		Dim X25 As Decimal

		Dim err As Boolean
		Dim Suva_Bas_OhneAbzugrec = m_PayrollDatabaseAccess.LoadSuvaData1ForPayroll(LPMonth, LPYear, m_EmployeeData.EmployeeNumber, MDNr, err)
		ThrowExceptionOnError(err, "Suva Daten 1 konnten nicht geladen werden.")

		With Suva_Bas_OhneAbzugrec  ' Basen, auf die kein Abzug in den Vormonaten erfolgten
			' A1 ohne Abzug =A1_Basis - ((NBUV_Basis_Männer) + (NBUV_Basis_Frauen))
			X10 = Math.Max(.S7321.Value - ((.S7390.Value * -1D) + (.S7395.Value * -1D)), 0D)     ' A1

			' Z1 ohne Abzug = A1_Basis + Z1_Basis - _
			' ((NBUV_Basis_Männer) + (NBUV_Basis_Frauen) + A1_Ohne_Abzug)
			X11 = Math.Max(.S7321.Value + .S7325.Value - ((.S7390.Value * -1D) + (.S7395.Value * -1D) + X10), 0D)     ' Z1


			' A2 ohne Abzug =A2_Basis - ((NBUV_Basis_Männer_AG) + (NBUV_Basis_Frauen_AG))
			X17 = Math.Max(.S7322.Value - ((.S7820.Value * -1D) + (.S7825.Value * -1D)), 0D)         ' A2

			' Z2 ohne Abzug = A2_Basis + Z2_Basis - _
			'     ((NBUV_Basis_Männer_AG) + (NBUV_Basis_Frauen_AG) + A2_Ohne_Abzug)
			X18 = Math.Max(.S7322.Value + .S7326.Value - ((.S7820.Value * -1D) + (.S7825.Value * -1D) + X17), 0D)     ' Z2


			' A3_ohne_Abzug = A3_Basis - ((UV_A_AG) - (NBUV_Basis_Männer) - (NBUV_Basis_Frauen) - _
			'     (NBUV_Basis_Männer_AG) - (NBUV_Basis_Frauen_AG))
			X23 = .S7323 - Math.Max((.S7830.Value * -1D) - (.S7390.Value * -1D) - (.S7395.Value * -1D) -
								(.S7820.Value * -1D) - (.S7825.Value * -1D) - X10 - X17, 0D) '_ 'X10 - X17, 0) ' A3

			' Z3_ohne_Abzug = A3_Basis + Z3_Basis - _
			'     ((UV_Z_AG) - (NBUV_Basis_Männer) - (NBUV_Basis_Frauen) - _
			'     (NBUV_Basis_Männer_AG) - (NBUV_Basis_Frauen_AG) + A3_Ohne_Abzug)
			X25 = .S7323 + .S7327 - Math.Max((.S7830.Value * -1D) + (.S7835.Value * -1D) -
								(.S7390.Value * -1D) - (.S7395.Value * -1D) - (.S7820.Value * -1D) - (.S7825.Value * -1D) -
								X10 - X11 - X17 - X18 - X23, 0)              ' Z3

		End With

		'HLTEs = Round((MDrec!ALV1_HL / 360) * ESYearTage, 2)         ' muss noch Revisor gefragt werden
		'HLTEs = NumberRound(MDrec!Suva_HL / 360, 2) * ESYearTage     ' muss noch Revisor gefragt werden
		Dim Fullmonth As Integer

		Fullmonth = Int(ESYearTage / 30)
		HLTEs = m_MandantData.Suva_HL / 12 * Fullmonth
		' Höchstmögliche Höchstlohn bis Ende der aktuellen Monat
		HLTEs = HLTEs + NumberRound(m_MandantData.Suva_HL / 360 * (ESYearTage - 30 * Fullmonth), 2)
		' Auf diesen ganzen Betrag wurde SUVA abgerechnet (Vormonat).
		SLK = Suva_Bas_OhneAbzugrec.SLK     ' 7830+7835+7320+7324; (UV_AG + A0 + Z0)

		' Maximale SUVA-Lohn der akt. LP; Muss verteilt werden auf A0 bis Z3
		' Auf maxhl wurde noch keine SUVA bezahlt.
		MaxHL = HLTEs - SLK

		' MaxHL muss auf die Basen von der SUVA-Codes verteilt werden...

		' Auf A0 und Z0 zahlen wir keine SUVA, also ABZIEHEN.
		MaxHL = MaxHL - (U(22) - U(24))
		' Z3
		If MaxHL > 0 Then
			UVAGZ = UVAGZ + Math.Min(MaxHL, U(25) + X25) ' MaxHL Auf Z3 verteilen.
			MaxHL = MaxHL - Math.Min(MaxHL, U(25) + X25)
		End If

		' A3
		If MaxHL > 0 Then
			UVAGA = UVAGA + Math.Min(MaxHL, U(23) + X23) ' MaxHL Auf A3 verteilen.
			MaxHL = MaxHL - Math.Min(MaxHL, U(23) + X23)
		End If

		' Z1, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			UVAGZ = UVAGZ + Math.Min(MaxHL, U(11) + X11) ' MaxHL Auf Z1 verteilen.
			NBUVANZ = NBUVANZ + Math.Min(MaxHL, U(11) + X11)
			MaxHL = MaxHL - Math.Min(MaxHL, U(11) + X11)
		End If

		' A1, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			UVAGA = UVAGA + Math.Min(MaxHL, U(10) + X10) ' MaxHL Auf A1 verteilen.
			NBUVANA = NBUVANA + Math.Min(MaxHL, U(10) + X10)
			MaxHL = MaxHL - Math.Min(MaxHL, U(10) + X10)
		End If

		' Z2, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			UVAGZ = UVAGZ + Math.Min(MaxHL, U(18) + X18) ' MaxHL Auf Z2 verteilen.
			NBUVAGZ = NBUVAGZ + Math.Min(MaxHL, U(18) + X18)
			MaxHL = MaxHL - Math.Min(MaxHL, U(18) + X18)
		End If

		' A2, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			UVAGA = UVAGA + Math.Min(MaxHL, U(17) + X17) ' MaxHL Auf A2 verteilen.
			NBUVAGA = NBUVAGA + Math.Min(MaxHL, U(17) + X17)
			MaxHL = MaxHL - Math.Min(MaxHL, U(17) + X17)
		End If

		'' Wenn Z-Basis auf Vormonate Offen ist, aber keine Z-Basis im aktuellen Monat
		'' bis hier wurden alle SUVA-Basen der akt. LP verteilt
		'' wenn noch Höchstlohn offen ist, dann offene Basen von Vormonaten holen.
		'If MaxHL > 0 Then
		'  SLVorZ = Suva_Bas_OhneAbzugrec!SKBZ - Suva_Bas_OhneAbzugrec!S7324 - _
		'            Suva_Bas_OhneAbzugrec!s7835 - X11 - X18 - X25
		'  UVAGZ = UVAGZ + Minimum(MaxHL, SLVorZ)
		'  NBUVANZ = NBUVANZ + Minimum(MaxHL, SLVorZ)
		'  MaxHL = MaxHL - Minimum(MaxHL, SLVorZ)
		'End If
		'
		'If MaxHL > 0 Then
		'  SLVorA = Suva_Bas_OhneAbzugrec!SKBA - Suva_Bas_OhneAbzugrec!S7320 - _
		'            Suva_Bas_OhneAbzugrec!s7830 - X10 - X17 - X23
		'  UVAGA = UVAGA + Minimum(MaxHL, SLVorA)
		'  NBUVANA = NBUVANA + Minimum(MaxHL, SLVorA)
		'  MaxHL = MaxHL - Minimum(MaxHL, SLVorA)
		'End If

		S(28) = UVAGA        ' Suva-Lohn A
		S(29) = UVAGZ        ' Suva-Lohn Z

		U(26) = NBUVANA + NBUVANZ       ' NBUV Beitrag Männer durch AN (7390)
		U(27) = U(26)                   ' NBUV Beitrag Frauen durch AN (7395)
		U(28) = NBUVAGA + NBUVAGZ       ' NBUV Beitrag Männer durch AN (7820)
		U(29) = U(28)                   ' NBUV Beitrag Frauen durch AN (7825)

	End Function

	Private Function GetNew_NBUVBasis()
		Dim HLTEs As Decimal
		Dim SLK As Decimal
		Dim MaxHL As Decimal                         '
		Dim UVAGZ As Decimal
		Dim UVAGA As Decimal
		Dim NBUVANZ As Decimal
		Dim NBUVANA As Decimal
		Dim NBUVAGZ As Decimal
		Dim NBUVAGA As Decimal
		Dim X10 As Decimal
		Dim X11 As Decimal
		Dim X17 As Decimal
		Dim X18 As Decimal
		Dim X23 As Decimal
		Dim X25 As Decimal

		Dim err As Boolean
		Dim Suva_Bas_OhneAbzugrec = m_PayrollDatabaseAccess.LoadSuvaData2ForPayroll(LPMonth, LPYear, m_EmployeeData.EmployeeNumber, MDNr, err)
		ThrowExceptionOnError(err, "Suva Daten 2 konnten nicht geladen werden.")

		Dim msg As String = "<br><b>GetNew_NBUVBasis:</b> "
		With Suva_Bas_OhneAbzugrec  ' Basen, auf die kein Abzug in den Vormonaten erfolgten
			' A1 ohne Abzug =A1_Basis - ((NBUV_Basis_Männer) + (NBUV_Basis_Frauen))
			X10 = Math.Max(.S7321.Value - ((.S7390.Value * -1) + (.S7395.Value * -1)), 0)     ' A1

			' Z1 ohne Abzug = A1_Basis + Z1_Basis - _
			' ((NBUV_Basis_Männer) + (NBUV_Basis_Frauen) + A1_Ohne_Abzug)
			X11 = Math.Max(.S7321.Value + .S7325.Value - ((.S7389.Value * -1) + (.S7394.Value * -1) + (.S7390.Value * -1) + (.S7395.Value * -1) + X10), 0)    ' Z1


			' A2 ohne Abzug =A2_Basis - ((NBUV_Basis_Männer_AG) + (NBUV_Basis_Frauen_AG))
			X17 = Math.Max(.S7322.Value - ((.S7820.Value * -1) + (.S7825.Value * -1)), 0)         ' A2

			' Z2 ohne Abzug = A2_Basis + Z2_Basis - _
			'     ((NBUV_Basis_Männer_AG) + (NBUV_Basis_Frauen_AG) + A2_Ohne_Abzug)
			X18 = Math.Max(.S7322.Value + .S7326.Value - ((.S7819.Value * -1) + (.S7824.Value * -1) + (.S7820.Value * -1) + (.S7825.Value * -1) + X17), 0)    ' Z2


			' A3_ohne_Abzug = A3_Basis - ((UV_A_AG) - (NBUV_Basis_Männer) - (NBUV_Basis_Frauen) - (NBUV_Basis_Männer_AG) - (NBUV_Basis_Frauen_AG))
			X23 = .S7323 - Math.Max((.S7830.Value * -1) - (.S7390.Value * -1) - (.S7395.Value * -1) - (.S7820.Value * -1) - (.S7825.Value * -1) - X10 - X17, 0) ' _
			'X10 - X17, 0) ' A3

			' Z3_ohne_Abzug = A3_Basis + Z3_Basis - ((UV_Z_AG) - (NBUV_Basis_Männer) - (NBUV_Basis_Frauen) - (NBUV_Basis_Männer_AG) - (NBUV_Basis_Frauen_AG) + A3_Ohne_Abzug)
			X25 = .S7323 + .S7327 - Math.Max((.S7830.Value * -1) + (.S7835.Value * -1) -
								(.S7389.Value * -1) - (.S7394.Value * -1) - (.S7819.Value * -1) - (.S7824.Value * -1) - (.S7390.Value * -1) - (.S7395.Value * -1) - (.S7820.Value * -1) - (.S7825.Value * -1) -
								X10 - X11 - X17 - X18 - X23, 0)              ' Z3

			If X10 <> .X10 OrElse X11 <> .X11 OrElse X17 <> .X17 OrElse X18 <> .X18 OrElse X23 <> .X23 OrElse X25 <> .X25 Then
				msg &= String.Format("| <b>X-Values are diffrent!!! >>></b> X10: {0:n2} > db-x10: {1:n2} >>> X11: {2:n2} > db-x11: {3:n2} ", X10, .X10, X11, .X11)
				msg &= String.Format("| X17: {0:n2} > db-x17: {1:n2} >>> X18: {2:n2} > db-x18: {3:n2} ", X17, .X17, X18, .X18)
				msg &= String.Format("| X23: {0:n2} > db-x23: {1:n2} >>> X25: {2:n2} > db-x25: {3:n2} ", X23, .X23, X25, .X25)
				msg &= String.Format("| finished.")
			End If

			msg &= String.Format("| A-Values: X10: {0:n2} >>> X17: {1:n2} >>> 103_01A-Values: {2:n2} >>> X23: {3:n2} ", X10, X17, .S103_01A, X23)
			msg &= String.Format("| Z-Values: X11: {0:n2} >>> X18: {1:n2} >>> 103_01Z-Values: {2:n2} >>> X25: {3:n2} ", X11, X18, .S103_01Z, X25)

		End With

		'HLTEs = Round((MDrec!ALV1_HL / 360) * ESYearTage, 2)         ' muss noch Revisor gefragt werden
		'HLTEs = NumberRound(MDrec!Suva_HL / 360, 2) * ESYearTage     ' muss noch Revisor gefragt werden
		Dim Fullmonth As Integer

		Fullmonth = Int(ESYearTage / 30)
		HLTEs = m_MandantData.Suva_HL / 12 * Fullmonth
		' Höchstmögliche Höchstlohn bis Ende der aktuellen Monat
		HLTEs = HLTEs + NumberRound(m_MandantData.Suva_HL / 360 * (ESYearTage - 30 * Fullmonth), 2)
		' Auf diesen ganzen Betrag wurde SUVA abgerechnet (Vormonat).
		SLK = Suva_Bas_OhneAbzugrec.SLK     ' 7830+7835+7320+7324; (UV_AG + A0 + Z0)

		' Maximale SUVA-Lohn der akt. LP; Muss verteilt werden auf A0 bis Z3
		' Auf maxhl wurde noch keine SUVA bezahlt.
		MaxHL = HLTEs - SLK

		' MaxHL muss auf die Basen von der SUVA-Codes verteilt werden...
		msg &= String.Format("| Fullmonth: {0} >>> HLTEs: {1:n2} >>> SLK: {2:n2} >>> MaxHL(0): {3:n2}", Fullmonth, HLTEs, SLK, MaxHL)

		' Auf A0 und Z0 zahlen wir keine SUVA, also ABZIEHEN.
		MaxHL = MaxHL - (U(22) - U(24))
		msg &= String.Format("| U(22): {0:n2} >>> U(24): {1:n2} >>> MaxHL(1): {2:n2}", U(22), U(24), MaxHL)

		' Z3
		If MaxHL > 0 Then
			'UVAGZ = UVAGZ + Math.Min(MaxHL, U(25) + X25) ' MaxHL Auf Z3 verteilen.
			UVAGZ = Math.Min(MaxHL, U(25) + X25) ' MaxHL Auf Z3 verteilen.
			MaxHL = MaxHL - UVAGZ 'Math.Min(MaxHL, U(25) + X25)
			msg &= String.Format("| U(25): {0:n2} >>> X25: {1:n2} >>> UVAGZ: {2:n2} >>> MaxHL(2): {3:n2}", U(25), X25, UVAGZ, MaxHL)
		End If

		' A3
		If MaxHL > 0 Then
			'UVAGA = UVAGA + Math.Min(MaxHL, U(23) + X23) ' MaxHL Auf A3 verteilen.
			'MaxHL = MaxHL - Math.Min(MaxHL, U(23) + X23)

			UVAGA = Math.Min(MaxHL, U(23) + X23) ' MaxHL Auf A3 verteilen.
			MaxHL = MaxHL - UVAGA
			msg &= String.Format("| U(23): {0:n2} >>> X23: {1:n2} >>> UVAGZ: {2:n2} >>> MaxHL(3): {3:n2}", U(23), X23, UVAGA, MaxHL)
		End If

		' Z1, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			UVAGZ = UVAGZ + Math.Min(MaxHL, U(11) + X11) ' MaxHL Auf Z1 verteilen.
			'NBUVANZ = NBUVANZ + Math.Min(MaxHL, U(11) + X11)
			NBUVANZ = Math.Min(MaxHL, U(11) + X11)
			MaxHL = MaxHL - NBUVANZ ' Math.Min(MaxHL, U(11) + X11)
			msg &= String.Format("| U(11): {0:n2} >>> X11: {1:n2} >>> UVAGZ: {2:n2} >>> NBUVANZ: {3:n2} >>> MaxHL(4): {4:n2}", U(11), X11, UVAGZ, NBUVANZ, MaxHL)
		End If

		' A1, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			UVAGA = UVAGA + Math.Min(MaxHL, U(10) + X10) ' MaxHL Auf A1 verteilen.
			'NBUVANA = NBUVANA + Math.Min(MaxHL, U(10) + X10)
			NBUVANA = Math.Min(MaxHL, U(10) + X10)
			MaxHL = MaxHL - NBUVANA 'Math.Min(MaxHL, U(10) + X10)
			msg &= String.Format("| U(10): {0:n2} >>> X10: {1:n2} >>> UVAGZ: {2:n2} >>> NBUVANA: {3:n2} >>> MaxHL(5): {4:n2}", U(10), X10, UVAGA, NBUVANA, MaxHL)
		End If

		' Z2, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			'UVAGZ = UVAGZ + Math.Min(MaxHL, U(18) + X18) ' MaxHL Auf Z2 verteilen.
			'NBUVAGZ = NBUVAGZ + Math.Min(MaxHL, U(18) + X18)
			'MaxHL = MaxHL - Math.Min(MaxHL, U(18) + X18)

			NBUVAGZ = Math.Min(MaxHL, U(18) + X18)
			UVAGZ = UVAGZ + NBUVAGZ ' MaxHL Auf Z2 verteilen.
			MaxHL = MaxHL - NBUVAGZ
			msg &= String.Format("| U(18): {0:n2} >>> X18: {1:n2} >>> UVAGZ: {2:n2} >>> NBUVANA: {3:n2} >>> MaxHL(6): {4:n2}", U(18), X18, UVAGZ, NBUVAGZ, MaxHL)
		End If

		' A2, Achtung NBUV-pflichtig
		If MaxHL > 0 Then
			'UVAGA = UVAGA + Math.Min(MaxHL, U(17) + X17) ' MaxHL Auf A2 verteilen.
			'NBUVAGA = NBUVAGA + Math.Min(MaxHL, U(17) + X17)
			'MaxHL = MaxHL - Math.Min(MaxHL, U(17) + X17)

			NBUVAGA = Math.Min(MaxHL, U(17) + X17)
			UVAGA = UVAGA + NBUVAGA   ' MaxHL Auf A2 verteilen.
			MaxHL = MaxHL - NBUVAGA
			msg &= String.Format("| U(17): {0:n2} >>> X17: {1:n2} >>> UVAGZ: {2:n2} >>> NBUVANA: {3:n2} >>> MaxHL(7): {4:n2}", U(17), X17, UVAGA, NBUVAGA, MaxHL)
		End If

		'' Wenn Z-Basis auf Vormonate Offen ist, aber keine Z-Basis im aktuellen Monat
		'' bis hier wurden alle SUVA-Basen der akt. LP verteilt
		'' wenn noch Höchstlohn offen ist, dann offene Basen von Vormonaten holen.
		'If MaxHL > 0 Then
		'  SLVorZ = Suva_Bas_OhneAbzugrec!SKBZ - Suva_Bas_OhneAbzugrec!S7324 - _
		'            Suva_Bas_OhneAbzugrec!s7835 - X11 - X18 - X25
		'  UVAGZ = UVAGZ + Minimum(MaxHL, SLVorZ)
		'  NBUVANZ = NBUVANZ + Minimum(MaxHL, SLVorZ)
		'  MaxHL = MaxHL - Minimum(MaxHL, SLVorZ)
		'End If
		'
		'If MaxHL > 0 Then
		'  SLVorA = Suva_Bas_OhneAbzugrec!SKBA - Suva_Bas_OhneAbzugrec!S7320 - _
		'            Suva_Bas_OhneAbzugrec!s7830 - X10 - X17 - X23
		'  UVAGA = UVAGA + Minimum(MaxHL, SLVorA)
		'  NBUVANA = NBUVANA + Minimum(MaxHL, SLVorA)
		'  MaxHL = MaxHL - Minimum(MaxHL, SLVorA)
		'End If

		S(28) = UVAGA        ' Suva-Lohn A
		S(29) = UVAGZ        ' Suva-Lohn Z

		U(26) = NBUVANA                 ' NBUV Beitrag Männer durch AN (7390) A
		U(31) = NBUVANZ                 ' NBUV Beitrag Männer durch AN (7389) Z

		U(27) = U(26)                   ' NBUV Beitrag Frauen durch AN (7395) A
		U(32) = U(31)                   ' NBUV Beitrag Frauen durch AN (7394) Z

		U(28) = NBUVAGA                 ' NBUV Beitrag Männer durch AG (7820) A
		U(33) = NBUVAGZ                 ' NBUV Beitrag Männer durch AG (7819) Z

		U(29) = U(28)                   ' NBUV Beitrag Frauen durch AG (7825) A
		U(34) = U(33)                   ' NBUV Beitrag Frauen durch AG (7824) Z

		msg &= String.Format("| Zusammenstellung: S(28): {0:n2} >>> S(29): {1:n2} >>> U(26): {2:n2} >>> U(31): {3:n2}", S(28), S(29), U(26), U(31))
		msg &= String.Format("| U(27): {0:n2} >>> U(32): {1:n2} >>> U(28): {2:n2} >>> U(33): {3:n2}", U(27), U(32), U(28), U(33))
		msg &= String.Format("| U(29): {0:n2} >>> U(34): {1:n2}", U(29), U(34))

		strOriginData &= msg

	End Function

	Private Function RunBasFunction(ByVal FunctionNr As Decimal,
							Optional ByVal lFixAnsatz As Decimal = 0,
							Optional ByVal iGAVBeruf As Integer = 0,
							Optional ByVal iGruppe1 As Integer = 0,
							Optional ByVal iGAVKanton As Integer = 0,
							Optional ByVal bShowMessage As Boolean = False) As Decimal

		Dim result As Decimal = 0
		Dim err As Boolean = False

		If FunctionNr = 0 Then Return result

		Select Case FunctionNr
			Case 1        ' AHVBas()
				result = AHVBas()

			' Basis für SUVA-Lohn A, Z (Achtung: alte Version, ohne NBUV-A und Z Anteil)
			Case 2
				result = GetNBUVBasis()

			Case 3        ' Die Suva-Variable berechnen
				result = GetSuvaVar()

			Case 4        ' Geleistete Arbeitstage im Monat berechnen
				If (ESLPTage > 0) Then
					result = -1
				Else
					result = 0
				End If

			Case 5        ' Basis für ALV1-Lohn
				result = GetALV1Lohn()

			Case 6        ' Basis für ALV2-Lohn
				result = GetALV2Lohn()

			Case 7              ' BVG
				Dim startNewBVGNow As Boolean?
				Dim value As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/startnewbvgcalculationnow", m_PayrollSetting))
				If Not String.IsNullOrWhiteSpace(value) Then
					startNewBVGNow = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/startnewbvgcalculationnow", m_PayrollSetting)), Nothing)
				End If

				If startNewBVGNow.HasValue Then
					If startNewBVGNow Then
						result = BVG_Lohn_Std_New_WithAllBVGLohn()

					Else
						If LPYear > 2014 OrElse (LPMonth >= 9 And LPYear = 2014) Then
							result = BVG_Lohn_Std_New_WithAllBVGLohn()
						Else
							result = BVG_Lohn_Std()
						End If

					End If

				Else
					'If LPMonth > 11 And LPYear >= 2014 Then
					If LPYear > 2014 Then
						result = BVG_Lohn_Std_New_WithAllBVGLohn()
					Else
						result = BVG_Lohn_Std()
					End If

				End If


			Case 7.1                ' BVG New with BVGLohn
				result = BVG_Lohn_Std_New_WithAllBVGLohn()

			Case 8              ' Ansatz für Quellensteuer

				'frmCreateLO.StatusBar1.Panels(2).Text = "Angaben über Quellensteuer..."
				result = GetQSTAnsInUI(lFixAnsatz)

				If result <> -1 Then
					'frmCreateLO.StatusBar1.Panels(2).Text = "Anzahl der ausgewählten Kandidaten: " & _ frmCreateLO.LvgSelMA.Rows
				Else
					WriteToProtocol(Padright("*** -> RunBasFunc: ", 30, " ") & "(-1) Fehler in Quellensteuercode...")
					result = -1
				End If

			Case 9              ' Versicherte Tage für BVG in einer Lohnperiode
				result = Get_BVGDays_In_Month(m_EmployeeData.EmployeeNumber, Nothing, Nothing)

			Case 10
				result = Get_BVGAns(m_EmployeeData.Birthdate, m_EmployeeData.Gender)
			Case 10.2
				result = Get_BVGAns_WithRentner(m_EmployeeData.Birthdate, m_EmployeeData.Gender)

			Case 10.1
				result = ExistLAInLO()

			Case 11
				result = Get_BVG_Std_Basis()             ' BVG-Stundenlohn von BVG-Datenbank im Monat

					'Case 12
					'	result = Get_BVG_Monat_Basis()		 ' BVG-Basis im Monat

			Case 13              ' Basis für SUVA-Lohn A, Z (Neue Version)
				result = GetNew_NBUVBasis()


			Case 15 ' wird nicht mehr benutzt!!!
				result = GetStdInYearForBVG()

			Case 16 ' wird nicht mehr benutzt!!!
				result = Get_ES_Std_Total()

					' Aktuelle Auszahlung
			Case 17
				result = Get_FeierGuthaben(m_EmployeeData.EmployeeNumber)
					' Vorjahres Auszahlung
			Case 17.1
				result = Get_FeierGuthaben_1(m_EmployeeData.EmployeeNumber)

					' Aktuelle Auszahlung
			Case 18
				result = Get_FerGuthaben(m_EmployeeData.EmployeeNumber)
					' Vorjahres Auszahlung
			Case 18.1
				result = Get_FerGuthaben_1(m_EmployeeData.EmployeeNumber)

					' Aktuelle Auszahlung
			Case 19
				result = Get_13Guthaben(m_EmployeeData.EmployeeNumber)
					' Vorjahres Auszahlung
			Case 19.1
				result = Get_13Guthaben_1(m_EmployeeData.EmployeeNumber)


			Case 20
					'    result = Get_DarlehenGuthaben(MArec!MANr)

			Case 21
				result = Get_AnzZGABank(m_EmployeeData.EmployeeNumber)

			Case 22
				result = Get_ES_Std_Total_New()

			Case 22.1
				' Sollte keine Rückgabewerte geben. Diese ist nur eine Meldung und nichts mehr. RunFuncBefore!!!
				CheckForBVG()


					' die GAV-Functions
			Case 23
				result = GetKTGAGBasis(iGAVBeruf)
			Case 23.1
				result = GetKTGAGAns(iGAVBeruf)
			Case 23.2, 23.21
				result = GetKTGANBasis(iGAVBeruf)
			Case 23.3
				result = GetKTGANAns(iGAVBeruf)

			Case 23.4
				result = GetKTGNormalValue()


					' Far-Beiträge AG
			Case 24
				result = GetFAGBasis(iGAVBeruf, iGAVKanton)
			Case 24.1
				result = GetFAGAns(iGAVBeruf, iGAVKanton)

			Case 24.2
				result = GetFANBasis(iGAVBeruf, iGAVKanton)
			Case 24.3
				result = GetFANAns(iGAVBeruf, iGAVKanton)
					' Anzahl der Far
			Case 24.4
					'    result = ExistsMoreFar


					' Weiterbildung
			Case 25
				Dim basis = GetWAGBasis(iGAVBeruf, iGruppe1, iGAVKanton)
				Dim kurzArbeit As Decimal = 0

				'Dim data = m_PayrollDatabaseAccess.LoadCurrentMonthShorttimeWorkAmount(MDNr, m_EmployeeData.EmployeeNumber, LONewNr)
				'If data Is Nothing Then kurzArbeit = 0 Else kurzArbeit = data
				'If basis = 0 Then kurzArbeit = 0
				result = basis - kurzArbeit

			Case 25.1
				result = GetWAGAns(iGAVBeruf, iGruppe1, iGAVKanton)

			Case 25.2
				Dim basis = GetWANBasis(iGAVBeruf, iGruppe1, iGAVKanton)
				Dim kurzArbeit As Decimal = 0

				'Dim data = m_PayrollDatabaseAccess.LoadCurrentMonthShorttimeWorkAmount(MDNr, m_EmployeeData.EmployeeNumber, LONewNr)
				'If data Is Nothing Then kurzArbeit = 0 Else kurzArbeit = data
				'If basis = 0 Then kurzArbeit = 0
				result = basis - kurzArbeit


			Case 25.3
				result = GetWANAns(iGAVBeruf, iGruppe1, iGAVKanton)
					' Anzahl der Weiterbildungsfond
			Case 25.4
				result = GetWANStdBasis(iGAVBeruf, iGruppe1, iGAVKanton)
			Case 25.5
				result = GetWAGStdBasis(iGAVBeruf, iGruppe1, iGAVKanton)
			Case 25.6
				result = GetWAGMBasis(iGAVBeruf, iGruppe1, iGAVKanton)
			Case 25.7
				result = GetWANMBasis(iGAVBeruf, iGruppe1, iGAVKanton)


					' Vollzugskosten
			Case 26
				Dim basis = GetVAGBasis(iGAVBeruf, iGruppe1, iGAVKanton)
				Dim kurzArbeit As Decimal = 0
				result = basis - kurzArbeit

			Case 26.1
				result = GetVAGAns(iGAVBeruf, iGruppe1, iGAVKanton)

			Case 26.2
				Dim basis = GetVANBasis(iGAVBeruf, iGruppe1, iGAVKanton)
				Dim kurzArbeit As Decimal = 0
				result = basis - kurzArbeit

			Case 26.3
				result = GetVANAns(iGAVBeruf, iGruppe1, iGAVKanton)
					' Anzahl der Vollzugssfond
			Case 26.4
				result = GetVANStdBasis(iGAVBeruf, iGruppe1, iGAVKanton)
			Case 26.5
				result = GetVAGStdBasis(iGAVBeruf, iGruppe1, iGAVKanton)
			Case 26.6
				result = GetVAGMBasis(iGAVBeruf, iGruppe1, iGAVKanton)
			Case 26.7
				result = GetVANMBasis(iGAVBeruf, iGruppe1, iGAVKanton)


			Case 27
				result = GetStdBasis(iGAVBeruf, iGruppe1, iGAVKanton, bShowMessage)

			Case 27.1
				result = GetStdBasis(iGAVBeruf, iGruppe1, iGAVKanton, bShowMessage)

			Case 28
				result = ExistDiffBeitrag(0)            ' NBUV-Beitrag

			Case 29
				result = GetGleitAnz(iGAVBeruf, iGruppe1, iGAVKanton)            ' Rückstellung der Gleitzeit aus den Rapporten
			Case 29.1
				result = GetGleitBas(iGAVBeruf, iGruppe1, iGAVKanton)            ' Rückstellung der Gleitzeit aus den Rapporten

			Case 31.01
				'TODO Prüfen ob korrekt funtioniert
				Dim betrag As Decimal? = m_PayrollDatabaseAccess.LoadValueAmountDataForCase(FunctionNr, m_EmployeeData.EmployeeNumber, LONewNr, err)
				ThrowExceptionOnError(err, "Basis kann nicht berrechnet werden!")
				result = m_Utility.SwissCommercialRound(betrag.GetValueOrDefault(0))


			Case 50
				If LPYear >= 2020 AndAlso LPMonth >= 2 Then
					result = LoadFeierBackNettoBasis(LONewNr, m_EmployeeData.EmployeeNumber)         ' Rückstellung der Ferienentschädigung
				Else
					result = GetFeierBackNettoBasis(LONewNr, m_EmployeeData.EmployeeNumber)         ' Rückstellung der Ferienentschädigung
				End If

				S(47) = S(47) + NumberRound(result, 2) ' Achtung der s(47) ist durch LANr 530 bereits negativ

			Case 51
				If LPYear >= 2020 AndAlso LPMonth >= 2 Then
					result = LoadFerienBackNettoBasis(LONewNr, m_EmployeeData.EmployeeNumber)         ' Rückstellung der Ferienentschädigung
				Else
					result = GetFerienBackNettoBasis(LONewNr, m_EmployeeData.EmployeeNumber)         ' Rückstellung der Ferienentschädigung
				End If

				S(48) = S(48) + NumberRound(result, 2) ' Achtung der s(48) ist durch LANr 630 bereits negativ

			Case 52
				If LPYear >= 2020 AndAlso LPMonth >= 2 Then
					result = Load13LohnBackNettoBasis(LONewNr, m_EmployeeData.EmployeeNumber)         ' Rückstellung der Ferienentschädigung
				Else
					result = Get13LohnBackNettoBasis(LONewNr, m_EmployeeData.EmployeeNumber)         ' Rückstellung der Ferienentschädigung
				End If

				S(49) = S(49) + NumberRound(result, 2) ' Achtung der s(49) ist durch LANr 730 bereits negativ


			Case Else
				result = 0

		End Select


		Return result

	End Function

	Private Function AHVBas() As Decimal
		Dim result As Decimal = 0D
		Dim nBack As Decimal
		Dim dHeute As Date
		Dim AnzAHVFreiMonat As Integer
		Dim i As Integer
		Dim StartofMonth As Date
		Dim EndofMonth As Date
		Dim EndofYear As Date

		Dim FirstMonat As Boolean
		Dim MonatNr As Integer
		Dim err As Boolean

		EndofYear = CDate("31.12." & LPYear)

		If IsRentner Then
			If S(20) <> 0 Then
				FirstMonat = True
				For i = 1 To LPMonth
					Dim bIsRentner As Boolean
					If Not bIsRentner Then bIsRentner = Rentner(m_EmployeeData.Birthdate, m_EmployeeData.Gender, m_EmployeeLOSetting.AHVCode, CDate("1." & i & "." & LPYear))

					If bIsRentner Then
						If FirstMonat Then MonatNr = i : FirstMonat = False
						StartofMonth = CDate("01." & i & "." & LPYear)
						EndofMonth = CDate(DateAdd("m", 1, StartofMonth.AddDays(-StartofMonth.Day + 1))).AddDays(-1)

						' ob der Kandidaten bereits dann Rentner war?
						Dim workedDays As IEnumerable(Of WorkedDaysInLP) = m_PayrollDatabaseAccess.LoadWorkedDaysInLpForPayroll(m_EmployeeData.EmployeeNumber, StartofMonth, EndofMonth, EndofYear, MDNr)
						ThrowExceptionOnError(workedDays Is Nothing, "Arbeitstage in der Lohnperiode konnten nicht geladen werden.")
						If Not workedDays.Count = 0 Then

							AnzAHVFreiMonat += 1

						End If
					End If

				Next i

				Dim ahvFreiBetrag = m_PayrollDatabaseAccess.LoadAHVFreibetragForPayroll(m_EmployeeData.EmployeeNumber, LPMonth, LPYear, MonatNr, MDNr, err)
				ThrowExceptionOnError(err, "AHV Freibetrag konnte nicht geladen werden.")

				S(33) = AnzAHVFreiMonat * Val(ahvFreiBetrag.AHV_Freigrenze) - Val(ahvFreiBetrag.Sum_AHV_Freibetrag)
				S(33) = Math.Min(S(33), U(2) + Val(ahvFreiBetrag.AHVLohnAbRentner))
				WriteToProtocol(Padright(String.Format("-> (AHVBas): Renter: AnzAHVFreiMonat={0} | ", AnzAHVFreiMonat) & String.Format("Beginn Monat: {0} | AHV_Freigrenze: {1:n5} | Sum_AHV_Freibetrag: {2:n5} | S(20): {3:n2} | S(33): {4:n2}",
																																			MonatNr, ahvFreiBetrag.AHV_Freigrenze, ahvFreiBetrag.Sum_AHV_Freibetrag, S(20), S(33)), 255, " "))

				' - Val(DivFunc.vFieldVal(AFBrec!Sum_AHV_Freibetrag)))
				result = Math.Max(0, (S(20) - S(33)))

			End If

		Else                                                                ' Alte Function
			nBack = U(2)
			dHeute = CDate("01." & Val(LPMonth) & "." & Val(LPYear))
			If IsToYoung Or m_EmployeeLOSetting.AHVCode = 0 Then
				nBack = 0
			Else
				If IsRentner OrElse m_EmployeeLOSetting.AHVCode = 2 Then
					nBack = m_PayrollDatabaseAccess.LoadAHVRentnerBetragForPayroll(m_EmployeeData.EmployeeNumber, LPMonth, LPYear, MDNr, err)
					ThrowExceptionOnError(err, "AHV Renterbetrag konnte nicht geladen werden.")
				End If
			End If

			result = nBack
		End If


		Return result

	End Function

	Private Function IsVacationBrutto(ByVal mdNr As Integer, ByVal year As Integer) As Boolean
		Dim result As Boolean = False
		Dim err As Boolean

		result = m_PayrollDatabaseAccess.IsVacationBrutto(mdNr, year, err)
		ThrowExceptionOnError(err, "Kann nicht ermittlt werden ob die Rückstellungen Bruttopflichtig sind.")

		Return result
	End Function

	Private Function Rentner(ByVal MAGebDat As Date, ByVal MAGeschlecht As String, ByVal MAAhv As Integer, ByVal FirstOfMonth As Date) As Boolean
		Dim result As Boolean = False
		Dim RentAlter As Integer
		Dim MAAlter As Integer

		RentAlter = IIf(MAGeschlecht = "M", m_MandantData.RentAlter_M, m_MandantData.RentAlter_W)
		MAAlter = WhatAge(FirstOfMonth, MAGebDat)
		'If MAAlter = RentAlter Then
		'  'Month(MAGebDat) > Val(frmcreateLO.LPCb.Text) Then falls ab Geb.Monat kein Abzug mehr.
		'  If Month(MAGebDat) >= Month(FirstOfMonth) Then
		'    MAAlter = MAAlter - 1
		'  End If
		'End If

		If MAAlter >= RentAlter OrElse MAAhv = 2 Then
			result = True
		Else
			result = False
		End If

		Return result
	End Function

	Private Function WhatAge(ByVal FirstOfMonth As Date, ByVal Gebdat As Date) As Integer
		Dim result As Integer = 0

		result = Year(FirstOfMonth) - Year(Gebdat)
		If Month(FirstOfMonth) <= Month(Gebdat) Then result -= 1

		Return result
	End Function

	' Die Priorität wird gesetzt.
	Private Function GetSuvaVar()
		If U(22) <> 0 Then        ' A0
			If U(10) <> 0 Or U(11) <> 0 Then    ' A1, Z1
				U(10) = U(10) + U(22)
				U(22) = 0
			ElseIf U(17) <> 0 Or U(18) <> 0 Then     ' A2, Z2
				U(17) = U(17) + U(22)
				U(22) = 0
			ElseIf U(23) <> 0 Or U(25) <> 0 Then    ' A3, Z3
				U(10) = U(10) + U(22)
				U(22) = 0
			End If
		End If

		If U(23) <> 0 Then        ' A3
			If U(10) <> 0 Or U(11) <> 0 Then    ' A1, Z1
				U(10) = U(10) + U(23)
				U(23) = 0
			ElseIf U(17) <> 0 Or U(18) <> 0 Then     ' A2, Z2
				U(17) = U(17) + U(23)
				U(23) = 0
			End If
		End If

		If U(24) <> 0 Then        ' Z0
			If U(11) <> 0 Or U(10) <> 0 Then    ' Z1, A1
				U(11) = U(11) + U(24)
				U(24) = 0
			ElseIf U(18) <> 0 Or U(17) <> 0 Then     ' Z2, A2
				U(18) = U(18) + U(24)
				U(24) = 0
			ElseIf U(25) <> 0 Or U(23) <> 0 Then    ' Z3, A3
				U(11) = U(11) + U(24)
				U(24) = 0
			End If
		End If

		If U(25) <> 0 Then        ' Z3
			If U(11) <> 0 Or U(10) <> 0 Then    ' Z1, A1
				U(11) = U(11) + U(25)
				U(25) = 0
			ElseIf U(18) <> 0 Or U(17) <> 0 Then     ' Z2, A2
				U(18) = U(18) + U(25)
				U(25) = 0
			End If
		End If

	End Function


	Private Function LoadFeierBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer) As Decimal
		Dim result As Decimal = 0
		Dim err As Boolean

		result = m_PayrollDatabaseAccess.LoadFeierBackNettoBasis(payrollNumber, employeeNumber, err)
		ThrowExceptionOnError(err, "Daten für Feiertagsentschädigung konnten nicht geladen werden.")

		Return result
	End Function

	Private Function LoadFerienBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer) As Decimal
		Dim result As Decimal = 0
		Dim err As Boolean

		result = m_PayrollDatabaseAccess.LoadFerienBackNettoBasis(payrollNumber, employeeNumber, err)
		ThrowExceptionOnError(err, "Daten für Ferienentschädigung konnten nicht geladen werden.")

		Return result
	End Function

	Private Function Load13LohnBackNettoBasis(ByVal payrollNumber As Integer, ByVal employeeNumber As Integer) As Decimal
		Dim result As Decimal = 0
		Dim err As Boolean

		result = m_PayrollDatabaseAccess.Load13LohnBackNettoBasis(payrollNumber, employeeNumber, err)
		ThrowExceptionOnError(err, "Daten für 13. Lohn konnten nicht geladen werden.")

		Return result
	End Function



	Private Function GetALV1Lohn() As Decimal

		GetALV1Lohn = m_PayrollDatabaseAccess.LoadALV1Lohn(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear, m_MandantData.ALV1_HL, ESYearTage)

	End Function

	Private Function GetALV2Lohn() As Decimal
		Dim result As Decimal = 0

		If U(3) = 0 Then Return result
		' OLD-Version: Exit Function
		result = m_PayrollDatabaseAccess.LoadALV2Lohn(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear, m_MandantData.ALV2_HL, ESYearTage)

		Return result

	End Function

	Private Function ExistMLohn() As Boolean
		Dim result As Boolean = False
		Dim strLANr As String

		Dim kizulagenotiflanrcontains As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/kizulagenotiflanrcontains", m_PayrollSetting))
		strLANr = kizulagenotiflanrcontains

		If String.IsNullOrWhiteSpace(strLANr) Then
			strLANr = "1000"

		ElseIf strLANr = "0" Then
			Return False

		End If
		WriteToProtocol(Padright(String.Format("<b>Ausnahmelohnarten für Kinderzulagen: {0}</b>", strLANr), 255, " "))

		Dim exists As Boolean? = m_PayrollDatabaseAccess.CheckIfLOLExists(strLANr, m_EmployeeData.EmployeeNumber, LONewNr, MDNr)
		If exists.HasValue AndAlso exists Then result = True


		Return result
	End Function

	' 0 = NBUV-Beitrag
	Private Function ExistDiffBeitrag(ByVal iMode As Integer) As Boolean

		Dim bExistBeitrag As Boolean = True
		Dim strMassage As String

		strMassage = String.Format("Achtung: Für die Lohnabrechnung {0} existiert kein ", LONewNr)
		Select Case iMode
			Case 0    ' NBUV-Beitrag
				strMassage &= "Suva-Abzug!"

			Case Else

		End Select
		strMassage = String.Format("{0} Bitte kontrollieren Sie die Einsatzdaten.", strMassage)

		Dim exists As Boolean? = m_PayrollDatabaseAccess.CheckIfExistDiffBeitrag(m_EmployeeData.EmployeeNumber, MDNr, LONewNr, iMode)
		ThrowExceptionOnError(Not exists.HasValue, "Prüfung (ExistDiffBeitrag) ist fehlgeschlagen.")

		If Not exists Then
			WriteToProtocol(Padright("M -> (ExistDiffBeitrag): ", 30, " ") & strMassage)

			bExistBeitrag = False

		End If

		Return bExistBeitrag

	End Function

	Private Function GetESDayInYear(ByVal lMANumber As Integer) As Integer
		Dim ESAb1 As Date
		Dim ESBis1 As Date
		Dim StartOfYear As Date
		Dim EndofMonth As Date
		Dim StartofMonth As Date
		Dim EndofYear As Date
		Dim HLTage As Integer = 0
		Dim Temprec As List(Of ESDataForESDayInYearCalculation)

		StartofMonth = CDate("01." & LPMonth & "." & LPYear)
		EndofMonth = CDate(DateAdd("m", 1, StartofMonth)).AddDays(-1)

		StartOfYear = CDate("01.01." & LPYear)
		EndofYear = CDate("31.12." & LPYear)

		Temprec = m_PayrollDatabaseAccess.LoadESDataForESDayInYearCalculation(m_EmployeeData.EmployeeNumber, MDNr, StartOfYear, EndofMonth)
		ThrowExceptionOnError(Temprec Is Nothing, "Einsatzdaten für Berechnung der Einsatztage im Jahr konnten nicht geladen werden.")


		If Temprec.Count = 0 Then
			' Keine Einsätze vorhanden!!!
			' (vielleicht einige Lohnarten (Monatliche Lohnvariabel die Keine Einsätze benötigen"
			Return 0
		End If

		Dim currentESData = Temprec(0)

		ESAb1 = IIf(currentESData.ES_Ab < StartOfYear, StartOfYear, currentESData.ES_Ab)
		If Not currentESData.ES_Ende.HasValue Then
			ESBis1 = EndofMonth
		Else
			ESBis1 = currentESData.ES_Ende
		End If

		With Temprec

			'  Do While ESBis1 < EndOfMonth And Not .EOF
			'      If !ES_Ab - 1 <= ESBis1 Then
			'        If !ES_Ende > ESBis1 Or DivFunc.vFieldVal(!ES_Ende) = "" Then
			'          If DivFunc.vFieldVal(Temprec!ES_Ende) = "" Then
			'            ESBis1 = EndOfMonth
			'          Else
			'            ESBis1 = Temprec!ES_Ende
			'          End If
			'        End If
			'      Else
			'        If Year(ESBis1) > Year(ESAb1) Then ESBis1 = EndOfMonth
			'        HLTage = HLTage + HLTageForLP(ESAb1, ESBis1)
			'        ESAb1 = !ES_Ab
			'        If DivFunc.vFieldVal(Temprec!ES_Ende) = "" Then
			'          ESBis1 = EndOfMonth
			'        Else
			'          ESBis1 = Temprec!ES_Ende
			'        End If
			'
			'      End If
			'    .MoveNext
			'  Loop

			For currentESIndex = 1 To Temprec.Count - 1

				currentESData = Temprec(currentESIndex)

				If ESBis1 >= EndofMonth Then Exit For

				If currentESData.ES_Ab.Value.AddDays(-1) <= ESBis1 Then        ' Gibt einen Unterbruch!!!
					If currentESData.ES_Ende > ESBis1 Then
						ESBis1 = currentESData.ES_Ende
					ElseIf Not currentESData.ES_Ende.HasValue Then
						ESBis1 = EndofMonth
					End If
				Else                                ' gibt einen Unterbruch!!!
					If ESBis1 > EndofMonth Then ESBis1 = EndofMonth
					HLTage = HLTage + HLTageForLP(ESAb1, ESBis1)
					ESAb1 = currentESData.ES_Ab
					If Not currentESData.ES_Ende.HasValue Then
						ESBis1 = EndofMonth
					Else
						ESBis1 = currentESData.ES_Ende
					End If

				End If

			Next

		End With
		If ESBis1 > EndofMonth Then ESBis1 = EndofMonth
		HLTage = HLTage + HLTageForLP(ESAb1, ESBis1)

		Return HLTage

	End Function

	Private Function HLTageForLP(ByVal TempESAb1 As Date, ByVal TempESBis1 As Date) As Integer
		Dim TempDays As Integer
		Dim EndOfFeb As Date
		Dim DayAb As Integer
		Dim DayBis As Integer

		TempDays = (Month(TempESBis1) - Month(TempESAb1)) * 30

		DayBis = TempESBis1.Day
		DayAb = TempESAb1.Day
		EndOfFeb = CDate(DateAdd("m", 1, CDate("01.02." & Year(TempESAb1)))).AddDays(-1)

		If DayAb = 31 Then DayAb = 30
		If DayBis = 31 Then DayBis = 30
		If TempESAb1 = EndOfFeb Then DayAb = 30
		If TempESBis1 = EndOfFeb Then DayBis = 30

		TempDays = TempDays + DayBis - DayAb + 1

		'If Month(TempESAb1) = 2 And Day(TempESAb1) <> 1 And _
		'        Day(TempESAb1) <> Day(EndOfFeb) And _
		'        TempESBis1 > EndOfFeb Then
		'  TempDays = TempDays - (30 - Day(EndOfFeb))
		'End If
		HLTageForLP = TempDays

	End Function

	Public Function Get_WorkedDays_In_Month4QST(ByVal MANr As Long) As Integer

		Dim EsBeginn As Date
		Dim ESEnde As Date
		Dim EsEnde2 As Date
		Dim StartofMonth As Date
		Dim EndofMonth As Date
		Dim EndofYear As Date
		Dim WorkedDay As Long

		StartofMonth = CDate("01." & LPMonth & "." & LPYear)
		EndofMonth = CDate(DateAdd("m", 1, StartofMonth.AddDays(-StartofMonth.Day + 1))).AddDays(-1)
		EndofYear = CDate("31.12." & LPYear)

		Dim workedDays As IEnumerable(Of WorkedDaysInLP) = m_PayrollDatabaseAccess.LoadWorkedDaysInLpForPayroll(m_EmployeeData.EmployeeNumber, StartofMonth, EndofMonth, EndofYear, MDNr)
		ThrowExceptionOnError(workedDays Is Nothing, "Arbeitstage in der Lohnperiode konnten nicht geladen werden.")

		Dim currentRecord As WorkedDaysInLP = Nothing
		Dim currentIndex As Integer = 0

		If workedDays.Count = 0 Then
			Return 0
		Else
			currentRecord = workedDays(0)
			EsBeginn = currentRecord.ES_Ab
		End If

		With currentRecord

			Do While Not currentRecord Is Nothing
				ESEnde = IIf(.ES_Ende > EndofMonth Or (.ES_Ende Is Nothing), EndofMonth, .ES_Ende)

				If EsBeginn < StartofMonth Then
					EsBeginn = StartofMonth
				ElseIf .ES_Ab > EsBeginn Then
					EsBeginn = .ES_Ab
				End If

				If EsBeginn > ESEnde Then EsBeginn = ESEnde
				WorkedDay += DateDiff("d", EsBeginn, ESEnde, vbUseSystemDayOfWeek, vbUseSystem) + 1
				'    If Day(ESEnde) = 28 And LPMonth = 2 Then WorkedDay = WorkedDay + 2
				'    If Day(ESEnde) = 29 And LPMonth = 2 Then WorkedDay = WorkedDay + 1

				currentIndex += 1
				currentRecord = If(currentIndex > workedDays.Count - 1, Nothing, workedDays(currentIndex))
				Do While Not currentRecord Is Nothing

					EsEnde2 = IIf(.ES_Ende > EndofMonth OrElse (.ES_Ende Is Nothing), EndofMonth, .ES_Ende)
					'      If Day(EsEnde2) = 31 Then _
					'            EsEnde2 = CDate("30." & Month(EsEnde2) & "." & Year(EsEnde2))

					If .ES_Ab > EsBeginn And EsEnde2 > ESEnde Then
						EsBeginn = ESEnde.AddDays(1)
						Exit Do
					ElseIf .ES_Ab < EsBeginn Then
					ElseIf .ES_Ab = EsBeginn Then
					End If

					If EsEnde2 > ESEnde Then
						EsBeginn = ESEnde.AddDays(1)
						Exit Do
					ElseIf EsEnde2 < ESEnde Then
					ElseIf EsEnde2 = ESEnde Then

					End If

					currentIndex += 1
					currentRecord = If(currentIndex > workedDays.Count - 1, Nothing, workedDays(currentIndex))
				Loop
			Loop
		End With

		Return WorkedDay

	End Function

	Function GetQSTAnsInUI(Optional ByVal lFixAnsatz As Decimal = 0) As Decimal

		Dim result As Decimal = 0
		' TODO:
		result = GetQSTAns(lFixAnsatz)

		'm_TaskHelper.InUIAndWait(Function()
		'													 result = GetQSTAns(lFixAnsatz)
		'													 Return True
		'												 End Function)

		Return result

	End Function

	Function GetQSTAns(Optional ByVal lFixAnsatz As Decimal = 0) As Decimal

		Dim payrollContext As New frmQST.PayrollContextData With {
			.EmplPayroll = Me,
			.MANr = m_EmployeeData.EmployeeNumber,
			.MDnr = MDNr,
			.LPMonth = LPMonth,
			.LPYear = LPYear,
			.LONewNr = LONewNr,
			.DONotShowAgainQSTForm = DONotShowAgainQSTForm.GetValueOrDefault(False),
			.S = S,
			.U = U
			}

		Dim frmQst As New frmQST(MDNr, m_InitializationData, payrollContext)
		With frmQst
			.LoadData()

			' Bedingung 30 Code rausgenommen (braucht es nicht mehr)
			If Not DONotShowAgainQSTForm.GetValueOrDefault(False) Then
				.TopMost = True
				.ShowDialog()
				DONotShowAgainQSTForm = payrollContext.DONotShowAgainQSTForm

			Else
				.AcceptFormDat()
				DONotShowAgainQSTForm = DONotShowAgainQSTForm.GetValueOrDefault(False)

			End If
			.Dispose()
			'DONotShowAgainQSTForm = payrollContext.DONotShowAgainQSTForm

		End With
		GetQSTAns = U(50)

	End Function


End Class