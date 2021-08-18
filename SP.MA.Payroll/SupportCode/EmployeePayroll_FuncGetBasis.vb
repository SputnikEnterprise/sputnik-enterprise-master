Partial Public Class EmployeePayroll

	Function GetBasValue(ByVal lanr As Decimal, ByVal iTypeBasis As Integer,
											 ByVal strMABasVar As String,
											 ByVal cFixBasis As Decimal,
											 Optional ByVal iGAVBeruf As Integer = 0,
											 Optional ByVal iGruppe1Nr As Integer = 0,
											 Optional ByVal iGAVKanton As Integer = 0) As Decimal
		Dim strTempStr As String
		Dim strTemp1 As String
		Dim strFBasisValue As String
		Dim strSBasisValue As String
		Dim cTempZahl As Decimal
		Dim cFirstZahl As Decimal
		Dim cSecZahl As Decimal
		Dim cW_ValBas As Decimal
		Dim cFuncNr As Decimal

		Try

			Dim msg As String = String.Empty
			msg = String.Format("LANr: {0} <<< MABasVar: {1}", lanr, strMABasVar)

			Select Case iTypeBasis
				Case 2, 3                                       ' fester Wert
					cW_ValBas = cFixBasis

				Case 4                                            ' S-Variable
					cW_ValBas = S(Val(If(Len(strMABasVar) > 2, strMABasVar * -1, strMABasVar))) * If(Len(strMABasVar) > 2, -1, 1)

				Case 5                                            ' U-Variable
					cW_ValBas = U(Val(If(Len(strMABasVar) > 2, strMABasVar * -1, strMABasVar))) * If(Len(strMABasVar) > 3, -1, 1)

				Case 6, 7                                         ' S / U-Variable ADDIEREN
					strTempStr = strMABasVar

					msg = String.Format("LANr: {0} <<< Basis: {1}", lanr, strTempStr)

					' z. B. Max(04-17-30-31-37-38, #0)
					If InStr(1, UCase(strTempStr), "MAX") > 0 OrElse InStr(1, UCase(strTempStr), "MIN") > 0 Then

						cW_ValBas = CalculateBasValueForMaxAndMinVariables(lanr, iTypeBasis, strMABasVar)
						'' 04-17-30-31-37-38
						'strFBasisValue = Mid(strTempStr, InStr(1, strTempStr, "("))
						'strFBasisValue = Trim(Mid(strFBasisValue, 2, InStr(1, strFBasisValue, ",") - 2))
						'msg &= String.Format(" | strFBasisValue: {0}", strFBasisValue)


						'' #0
						'strSBasisValue = Mid(strTempStr, InStr(1, strTempStr, ",") + 1)
						'strSBasisValue = Trim(Mid(strSBasisValue, 1, Len(strSBasisValue) - 1))
						'msg &= String.Format(" | strSBasisValue: {0}", strSBasisValue)

						'' die erste Parameter berechnen...
						'strTemp1 = Mid(strFBasisValue, 1, 2)    ' 04
						'cTempZahl = If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))      ' S(6)
						'msg &= String.Format(" | {0}({1}): {2}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), cTempZahl)



						'strTempStr = Mid(strFBasisValue, 3)
						'Do While InStr(1, strFBasisValue, "+") <> 0 Or InStr(1, strFBasisValue, "-") <> 0
						'	strTemp1 = Mid(strTempStr, 2, 2)
						'	If Mid(strTempStr, 1, 1) = "+" Then
						'		cTempZahl = cTempZahl + If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
						'		msg &= String.Format(" |(+) {0}({1}): {2} | Total: {3}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), S(Val(strTemp1)), cTempZahl)

						'	ElseIf Mid(strTempStr, 1, 1) = "-" Then
						'		cTempZahl = cTempZahl - If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
						'		msg &= String.Format(" |(-) {0}({1}): {2} | Total: {3}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), S(Val(strTemp1)), cTempZahl)

						'	Else
						'		Exit Do

						'	End If

						'	strTempStr = Mid(strTempStr, 4)
						'	'If String.IsNullOrWhiteSpace(strTempStr) Then Exit Do
						'Loop
						'cFirstZahl = cTempZahl        ' 1. Parameter weist eine Zahl aus...

						'' die zweite Parameter rechnen...
						'strTempStr = strSBasisValue
						'strTemp1 = Mid(strTempStr, 1, 2)
						'If InStr(1, strTemp1, "#") = 0 Then
						'	cTempZahl = If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
						'	strTempStr = Mid(strTempStr, 3)
						'	Do While InStr(1, strTempStr, "+") <> 0 Or InStr(1, strTempStr, "-") <> 0
						'		strTemp1 = Mid(strTempStr, 2, 2)
						'		If Mid(strTempStr, 1, 1) = "+" Then
						'			cTempZahl = cTempZahl + If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))

						'		ElseIf Mid(strTempStr, 1, 1) = "-" Then
						'			cTempZahl = cTempZahl - If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))

						'		Else
						'			Exit Do

						'		End If

						'		strTempStr = Mid(strTempStr, 4)
						'	Loop


						'Else
						'	cTempZahl = Val(Mid(strTemp1, 2))

						'End If
						'cSecZahl = cTempZahl          ' 2. Parameter weist 2. Zahl aus...

						'If InStr(1, UCase(strMABasVar), "MAX") > 0 Then cW_ValBas = Math.Max(cFirstZahl, cSecZahl)
						'If InStr(1, UCase(strMABasVar), "MIN") > 0 Then cW_ValBas = Math.Min(cFirstZahl, cSecZahl)


					Else
						strTemp1 = Mid(strTempStr, 1, 2)
						cTempZahl = If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
						strTempStr = Mid(strTempStr, 3)
						Do While InStr(1, strTempStr, "+") <> 0 Or InStr(1, strTempStr, "-") <> 0
							strTemp1 = Mid(strTempStr, 2, 2)
							If Mid(strTempStr, 1, 1) = "+" Then
								cTempZahl = cTempZahl + If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))

							ElseIf Mid(strTempStr, 1, 1) = "-" Then
								cTempZahl = cTempZahl - If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))

							Else
								Exit Do

							End If

							strTempStr = Mid(strTempStr, 4)
						Loop

						cW_ValBas = cTempZahl
					End If

				Case 8                                                ' U-S Variable
					cW_ValBas = U(2) - S(6)

				Case 9, 10                                            ' S / U-Variable multiplizieren
					strTempStr = strMABasVar
					cTempZahl = 1
					Do While InStr(1, strTempStr, "*") <> 0
						strTemp1 = Mid(strTempStr, 1, 2)
						cTempZahl = cTempZahl * If(iTypeBasis = 9, S(Val(strTemp1)), U(Val(strTemp1)))
						strTempStr = Mid(strTempStr, 4)
					Loop
					cW_ValBas = cTempZahl * If(iTypeBasis = 9, S(Val(strTempStr)), U(Val(strTempStr)))

				Case 11                                               ' S * U-Variable multiplizieren
					strTempStr = strMABasVar
					cW_ValBas = S(Val(Mid(strTempStr, 1, 2))) * U(Val(Mid(strTempStr, 4)))

				Case 12                                               ' für GAV!!! G-Variable
					cW_ValBas = G(Val(If(Len(strMABasVar) > 2, strMABasVar * -1, strMABasVar))) * If(Len(strMABasVar) > 2, -1, 1)
				Case 13                                         ' Systemkonstanten
					cW_ValBas = GetLAValue(lanr)

				Case 14                                             ' S - U Variable Suva-A
					Dim difAmountUVNBU_A As Decimal = U(4) - U(19) ' Differnz zwischen NUBV pflichtigen und UV nicht pflichtigen
					cW_ValBas = S(28) - (difAmountUVNBU_A)

				Case 15                                                ' S - U Variable Suva-Z
					Dim difAmountUVNBU_Z As Decimal = U(5) - U(20)  ' Differnz zwischen NUBV pflichtigen und UV nicht pflichtigen
					cW_ValBas = S(29) - (difAmountUVNBU_Z)


				Case 30                                               ' Functionaufruf
					If InStr(1, strMABasVar, ")") = 0 Then
						cFuncNr = Val(Mid(strMABasVar, 1, Len(strMABasVar)))
					Else
						cFuncNr = Val(Mid(strMABasVar, 1, InStr(1, strMABasVar, ")") - 1)) '    Val(Mid(strMABasVar, 1, 2))
					End If
					cW_ValBas = RunBasFunction(cFuncNr, , iGAVBeruf, iGruppe1Nr, iGAVKanton)


			End Select


		Catch ex As Exception
			Throw New Exception(String.Format("LANR: {0} | {1}", lanr, ex.ToString))
		End Try


		Return cW_ValBas

	End Function

	Private Function CalculateBasValueForMaxAndMinVariables(ByVal lanr As Decimal, ByVal iTypeBasis As Integer, ByVal strMABasVar As String) As Decimal
		Dim result As Decimal = 0

		Dim strTempStr As String
		Dim strTemp1 As String
		Dim strFBasisValue As String
		Dim strSBasisValue As String
		Dim cTempZahl As Decimal
		Dim cFirstZahl As Decimal
		Dim cSecZahl As Decimal
		Dim cW_ValBas As Decimal = 0
		Dim msg As String = String.Empty


		strTempStr = strMABasVar
		msg = String.Format("LANr: {0} >>> MABasVar: {1}", lanr, strMABasVar)

		' z. B. Max(04-17-30-31-37-38, #0)

		' 04-17-30-31-37-38
		strFBasisValue = Mid(strTempStr, InStr(1, strTempStr, "("))
		strFBasisValue = Trim(Mid(strFBasisValue, 2, InStr(1, strFBasisValue, ",") - 2))
		'msg &= String.Format(" | strFBasisValue: {0} | strSBasisValue: {1}", strFBasisValue, strSBasisValue)


		' #0
		strSBasisValue = Mid(strTempStr, InStr(1, strTempStr, ",") + 1)
		strSBasisValue = Trim(Mid(strSBasisValue, 1, Len(strSBasisValue) - 1))
		'msg &= String.Format(" | strSBasisValue: {0}", strSBasisValue)
		msg &= String.Format(" | strFBasisValue: {0} | strSBasisValue: {1}", strFBasisValue, strSBasisValue)


		' die erste Parameter berechnen...
		strTemp1 = Mid(strFBasisValue, 1, 2)    ' 04
		cTempZahl = If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))      ' S(6)
		msg &= String.Format("{0} >>> {1}({2}): {3}", vbNewLine, If(iTypeBasis = 6, "S", "U"), Val(strTemp1), cTempZahl)



		strTempStr = Mid(strFBasisValue, 3)
		Do While InStr(1, strFBasisValue, "+") <> 0 Or InStr(1, strFBasisValue, "-") <> 0
			strTemp1 = Mid(strTempStr, 2, 2)
			If Mid(strTempStr, 1, 1) = "+" Then
				cTempZahl = cTempZahl + If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
				'msg &= String.Format(" |(+) {0}({1}): {2} | Total: {3}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), S(Val(strTemp1)), cTempZahl)
				msg &= String.Format(" |(+) {0}({1}): {2}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), S(Val(strTemp1)))

			ElseIf Mid(strTempStr, 1, 1) = "-" Then
				cTempZahl = cTempZahl - If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
				'msg &= String.Format(" |(-) {0}({1}): {2} | Total: {3}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), S(Val(strTemp1)), cTempZahl)
				msg &= String.Format(" |(-) {0}({1}): {2}", If(iTypeBasis = 6, "S", "U"), Val(strTemp1), S(Val(strTemp1)))

			Else
				msg &= String.Format(" | Total: {0}", cTempZahl)
				Exit Do

			End If

			strTempStr = Mid(strTempStr, 4)
			'If String.IsNullOrWhiteSpace(strTempStr) Then Exit Do
		Loop
		cFirstZahl = cTempZahl        ' 1. Parameter weist eine Zahl aus...

		' die zweite Parameter rechnen...
		strTempStr = strSBasisValue
		strTemp1 = Mid(strTempStr, 1, 2)
		If InStr(1, strTemp1, "#") = 0 Then
			cTempZahl = If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))
			strTempStr = Mid(strTempStr, 3)
			Do While InStr(1, strTempStr, "+") <> 0 Or InStr(1, strTempStr, "-") <> 0
				strTemp1 = Mid(strTempStr, 2, 2)
				If Mid(strTempStr, 1, 1) = "+" Then
					cTempZahl = cTempZahl + If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))

				ElseIf Mid(strTempStr, 1, 1) = "-" Then
					cTempZahl = cTempZahl - If(iTypeBasis = 6, S(Val(strTemp1)), U(Val(strTemp1)))

				Else
					Exit Do

				End If

				strTempStr = Mid(strTempStr, 4)
			Loop


		Else
			cTempZahl = Val(Mid(strTemp1, 2))

		End If
		cSecZahl = cTempZahl          ' 2. Parameter weist 2. Zahl aus...

		If InStr(1, UCase(strMABasVar), "MAX") > 0 Then
			cW_ValBas = Math.Max(cFirstZahl, cSecZahl)
		Else
			cW_ValBas = Math.Min(cFirstZahl, cSecZahl)
		End If

		If lanr = 8730 Then strOriginData &= String.Format(" | {1}{0} ", vbNewLine, msg)

		result = cW_ValBas


		Return result
	End Function


End Class
