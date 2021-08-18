Imports SP.DatabaseAccess.PayrollMng.DataObjects

Partial Public Class EmployeePayroll

	Private Sub GetRPLrec(ByVal bForMa As Boolean)

		AllRPLOrec = m_PayrollDatabaseAccess.LoadEmployeeRPLDataForLOCreation(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
		ThrowExceptionOnError(AllRPLOrec Is Nothing, "RPL Daten für LO Erzeugung konnten nicht geladen werden.")
	End Sub

	Private Sub GetLMrec(ByVal bForMa As Boolean)

		AllLMLOrec = m_PayrollDatabaseAccess.LoadEmployeeLMDataForLOCreation(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
		ThrowExceptionOnError(AllLMLOrec Is Nothing, "LM Daten für LO Erzeugung konnten nicht geladen werden.")
	End Sub

	Private Function ExistsSocialLMForEmployee() As Boolean
		Dim result As Boolean = True

		Dim data = m_PayrollDatabaseAccess.LoadEmployeeSozialleistungpflichtigLMDataForLOCreation(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
		ThrowExceptionOnError(data Is Nothing, "Sozialleistungspflichtige LM Daten für LO Erzeugung konnten nicht geladen werden.")
		If data.Count = 0 Then
			result = False
		End If

		Return result
	End Function

	Private Sub GetZGrec(ByVal bForMa As Boolean)
		AllZGLOrec = m_PayrollDatabaseAccess.LoadEmployeeZGDataForLOCreation(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
		ThrowExceptionOnError(AllZGLOrec Is Nothing, "ZG Daten für LO Erzeugung konnten nicht geladen werden.")
	End Sub

	Private Function SetFilterOnRecs(ByVal MANr As Integer, Optional ByVal FilterStr As String = "", Optional ByVal LACaption As String = "") As Boolean

		Dim result As Boolean = False
		Dim OrgFilterStr As String
		Dim FilterNr As String
		Dim SIndex As String    ' Index von Arrays
		Dim StrEnd As String    ' Reststring nach ")"
		Dim StrSigne As String  ' die Vergleichszeichen
		Dim err As Boolean = False
		Dim matches As Boolean? = Nothing
		Dim matchError = "Bedingung konnte nicht geprüft werden."

		If FilterStr = "" Then Return True ' : Exit Function

		If FilterStr = "False" Then Return False ' : Exit Function
		OrgFilterStr = FilterStr
		FilterNr = Mid(FilterStr, 1, InStr(1, FilterStr, ")") - 1)
		FilterStr = Mid(FilterStr, InStr(1, FilterStr, ") ") + 2)

		Select Case FilterNr
			Case "1"
				matches = m_PayrollDatabaseAccess.DoesFilterConditionMatch(FilterStr, MANr)
				ThrowExceptionOnError(Not matches.HasValue, matchError)
				result = matches

			Case "2"
				matches = m_PayrollDatabaseAccess.DoesFilterConditionMatch(FilterStr, MANr, S(5))
				ThrowExceptionOnError(Not matches.HasValue, matchError)
				result = matches

			Case "3"
				matches = m_PayrollDatabaseAccess.DoesFilterConditionMatch(FilterStr, MANr, S(5), S(19))
				ThrowExceptionOnError(Not matches.HasValue, matchError)
				result = matches

			Case "4"
				'TODO Prüfen ob korrekt funtioniert
				FilterStr = FilterStr.Replace("[", "").Replace("]", "")
				Dim betrag As Decimal? = m_PayrollDatabaseAccess.LoadFilterConditionDataForCase4(FilterStr, MANr, LONewNr, err)
				ThrowExceptionOnError(err, matchError)
				result = (betrag.HasValue AndAlso betrag.Value <> 0)

			Case "4.1"
				' for ki-Zulage daily
				result = ESLPTage <> 0
			Case "4.11"
				' for ki-Zulage hour
				result = S(2) <> 0
			Case "4.13"
				' both: daily and hour
				result = (S(2) <> 0) AndAlso (ESLPTage <> 0)


			Case "S0"   ' alle Summ-Varialben mit Abfragen
				SIndex = Mid$(FilterStr, 1, InStr(1, FilterStr, " ") - 1)
				StrEnd = Mid$(FilterStr, InStr(1, FilterStr, " ") + 1)
				StrSigne = Mid$(LTrim(StrEnd), 1, InStr(1, StrEnd, " "))
				StrEnd = Mid$(StrEnd, InStr(1, StrEnd, " "))

				If Trim(StrSigne) = "<>" Then
					result = (S(Val(SIndex)) <> Val(StrEnd))

				ElseIf Trim(StrSigne) = "<" Then
					result = S(Val(SIndex)) < Val(StrEnd)

				ElseIf Trim(StrSigne) = ">" Then
					result = S(Val(SIndex)) > Val(StrEnd)

				ElseIf Trim(StrSigne) = "=" Then
					result = (S(Val(SIndex)) = Val(StrEnd))
				End If

			Case "S1"
				result = S(3) < 0

			Case "S2"
				result = S(3) > 0

			Case "S3"
				result = (S(17) <> 0 AndAlso S(4) - S(17) > 0)

				Dim sMsgText = "S(4): {0:n2} | S(17): {1:n2}"
				sMsgText = String.Format(sMsgText, S(4), S(17))
				WriteToProtocol(Padright("M -> (SetFilterOnRecs): ", 30, " ") & sMsgText)

			Case "S4"
				If S(17) > 0 Then result = True

			Case "S5"
				If S(6) <> 0 OrElse S(52) <> 0 Then result = True

			Case "S3.1"
				' die summe von diversen S-Variablen
				Dim strTempStr As String = FilterStr
				Dim strTemp1 As String = Mid(strTempStr, 1, 2)
				Dim cTempZahl As Decimal = S(Val(strTemp1))
				strTempStr = Mid(strTempStr, 3)

				Do While InStr(1, strTempStr, "+") <> 0 Or InStr(1, strTempStr, "-") <> 0
					strTemp1 = Mid(strTempStr, 2, 2)
					If Mid(strTempStr, 1, 1) = "+" Then
						cTempZahl += S(Val(strTemp1))

					ElseIf Mid(strTempStr, 1, 1) = "-" Then
						cTempZahl -= S(Val(strTemp1))

					Else
						Exit Do

					End If

					strTempStr = Mid(strTempStr, 4)
				Loop
				result = (cTempZahl > 0 AndAlso S(17) <> 0)

			Case "U0"   ' alle U-Varialben mit Abfragen
				SIndex = Mid$(FilterStr, 1, InStr(1, FilterStr, " ") - 1)
				StrEnd = Mid$(FilterStr, InStr(1, FilterStr, " ") + 1)
				StrSigne = Mid$(LTrim(StrEnd), 1, InStr(1, StrEnd, " "))
				StrEnd = Mid$(StrEnd, InStr(1, StrEnd, " "))
				If Trim(StrSigne) = "<>" Then
					result = (U(Val(SIndex)) <> Val(StrEnd))

				ElseIf Trim(StrSigne) = "<" Then
					result = U(Val(SIndex)) < Val(StrEnd)

				ElseIf Trim(StrSigne) = ">" Then
					result = U(Val(SIndex)) > Val(StrEnd)

				ElseIf Trim(StrSigne) = "=" Then
					result = ((U(Val(SIndex)) = Val(StrEnd)))

				End If

			Case 5
				SIndex = Mid$(FilterStr, 1, 2)
				Select Case Val(SIndex)
					Case 1
						If IsToYoung OrElse m_EmployeeLOSetting.AHVCode = 0 Then
							result = True
						End If

					Case 2
						If IsRentner Then
							result = True
						End If

					Case 3
						If IsToYoung OrElse IsRentner Then
							result = True
						End If

					Case 4
						If Not IsRentner AndAlso m_EmployeeLOSetting.ALVCode <> 0 AndAlso Not IsToYoung Then
							result = True
						End If

				End Select

			Case 30    ' Function
				' TODO: runbasfunction is decimal!!!
				SIndex = Mid(OrgFilterStr, InStr(1, OrgFilterStr, ") ") + 2)
				Dim funcValue = RunBasFunction(Val(SIndex))

				result = (funcValue = -1)

		End Select


		Return result

	End Function

	Function GetMDAnsatzData() As Boolean

		Dim rTemprec As MandantAnsatzData

		rTemprec = m_PayrollDatabaseAccess.LoadMandantAnsatzData(MDNr, LPYear)
		ThrowExceptionOnError(rTemprec Is Nothing, "Mandant Ansatzdaten konnten nicht geladen werden.")

		RentFrei_Monat_ans = Val(rTemprec.RentFrei_Monat)
		AHV_AN_ans = Val(rTemprec.AHV_AN)
		AHV_2_AN_ans = Val(rTemprec.AHV_2_AN)
		ALV1_HL__ans = Val(rTemprec.ALV1_HL_)
		ALV2_HL__ans = Val(rTemprec.ALV2_HL_)
		ALV_AN_ans = Val(rTemprec.ALV_AN)
		ALV2_An_ans = Val(rTemprec.ALV2_An)
		SUVA_HL__ans = Val(rTemprec.SUVA_HL_)
		NBUV_M_ans = Val(rTemprec.NBUV_M)
		NBUV_M_Z_ans = Val(rTemprec.NBUV_M_Z)
		NBUV_W_ans = Val(rTemprec.NBUV_W)
		NBUV_W_Z_ans = Val(rTemprec.NBUV_W_Z)

		KK_An_MA_ans = Val(rTemprec.KK_An_MA)
		KK_An_MZ_ans = Val(rTemprec.KK_An_MZ)
		KK_An_WA_ans = Val(rTemprec.KK_An_WA)
		KK_An_WZ_ans = Val(rTemprec.KK_An_WZ)

		AHV_AG_ans = Val(rTemprec.AHV_AG)
		AHV_2_AG_ans = Val(rTemprec.AHV_2_AG)

		ALV_AG_ans = Val(rTemprec.ALV_AG)
		ALV2_AG_ans = Val(rTemprec.ALV2_AG)

		NBUV_M_ans = Val(rTemprec.NBUV_M)
		NBUV_M_Z_ans = Val(rTemprec.NBUV_M_Z)
		NBUV_W_ans = Val(rTemprec.NBUV_W)
		NBUV_W_Z_ans = Val(rTemprec.NBUV_W_Z)
		Suva_A_ans = Val(rTemprec.Suva_A)
		Suva_Z_ans = Val(rTemprec.Suva_Z)

		UVGZ_A_ans = Val(rTemprec.UVGZ_A)
		UVGZ_B_ans = Val(rTemprec.UVGZ_B)
		UVGZ2_A_ans = Val(rTemprec.UVGZ2_A)
		UVGZ2_B_ans = Val(rTemprec.UVGZ2_B)

		KK_AG_MA_ans = Val(rTemprec.KK_AG_MA)
		KK_AG_MZ_ans = Val(rTemprec.KK_AG_MZ)
		KK_AG_WA_ans = Val(rTemprec.KK_AG_WA)
		KK_AG_WZ_ans = Val(rTemprec.KK_AG_WZ)

		Fak_Proz_ans = Val(rTemprec.Fak_Proz)

		Dim advancepaymentcheckfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/advancepaymentcheckfee", m_PayrollSetting))
		Dim payrollcheckfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/payrollcheckfee", m_PayrollSetting))
		Dim advancepaymenttransferfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/advancepaymenttransferfee", m_PayrollSetting))
		Dim advancepaymentcashfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/advancepaymentcashfee", m_PayrollSetting))
		Dim advancepaymenttransferinternationalfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/advancepaymenttransferinternationalfee", m_PayrollSetting))
		Dim payrolltransferinternationalfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/payrolltransferinternationalfee", m_PayrollSetting))

		' Checkgebühren Vorschuss
		CheckGebuehrZG_ = Val(advancepaymentcheckfee)        ' Val(Format(DivReg.GetINIString(MDIniFullname, LoadResString(377), _LoadResString(378)), "0.00"))
		' Checkgebühren für Lohnzahlungen
		CheckGebuehrAuszahlung_ = Val(payrollcheckfee)      ' Val(Format(DivReg.GetINIString(MDIniFullname, LoadResString(377), LoadResString(379)), "0.00"))
		' Überweisungsgebühren für Vorschuss
		UeberweisungGebuehrZG_ = Val(advancepaymenttransferfee) 'Val(Format(Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), _ LoadResString(341))), "0.00"))
		'' Gebühren für Barauszahlung in Vorschuss
		BarGebuehrZG_ = Val(advancepaymentcashfee) ' Val(Format(Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), _ LoadResString(224))), "0.00"))

		' Gebühren für Vorschussüberweisung Ausland
		UeberweisungGebuehrZGAusland_ = Val(advancepaymenttransferinternationalfee) 'Val(Format(Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), _                  LoadResString(110))), "0.00"))
		' Gebühren für Lohnüberweisung Ausland
		UeberweisungGebuehrZGAuslandLO_ = Val(payrolltransferinternationalfee) ' Val(Format(Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), _ LoadResString(111))), "0.00"))

		GetMDAnsatzData = True

	End Function


	Function GetLAValue(ByVal lLANr As Decimal) As Decimal
		Dim value As Decimal


		Select Case lLANr

			Case 7140
				value = RentFrei_Monat_ans

			Case 7190
				value = AHV_AN_ans

			Case 7190.1
				value = AHV_2_AN_ans

			Case 7210
				value = ALV1_HL__ans

			Case 7230
				value = ALV2_HL__ans

			Case 7290
				value = ALV_AN_ans

			Case 7294
				value = ALV2_An_ans

			Case 7310
				value = SUVA_HL__ans


			Case 7389, 7389.011
				value = NBUV_M_Z_ans
			Case 7390, 7390.011
				value = NBUV_M_ans

			Case 7394, 7394.011
				value = NBUV_W_Z_ans
			Case 7395, 7395.011
				value = NBUV_W_ans


			Case 7400
				value = KK_An_MA_ans
			Case 7410
				value = KK_An_MZ_ans
			Case 7420
				value = KK_An_WA_ans
			Case 7430
				value = KK_An_WZ_ans


			Case 7800
				value = AHV_AG_ans
			Case 7800.1
				value = AHV_2_AG_ans

			Case 7810
				value = ALV_AG_ans
			Case 7815
				value = ALV2_AG_ans

			Case 7820
				value = NBUV_M_ans
			Case 7819
				value = NBUV_M_Z_ans
			Case 7825
				value = NBUV_W_ans
			Case 7824
				value = NBUV_W_Z_ans

			Case 7830
				value = Suva_A_ans
			Case 7830.01 ' special for stellenwerk!
				value = UVGZ_A_ans
			Case 7830.02 ' special for stellenwerk!
				value = UVGZ2_A_ans

			Case 7835
				value = Suva_Z_ans
			Case 7835.01  ' special for stellenwerk!
				value = UVGZ_B_ans
			Case 7835.02  ' special for stellenwerk!
				value = UVGZ2_B_ans


			Case 7840
				value = KK_AG_MA_ans
			Case 7845
				value = KK_AG_MZ_ans
			Case 7850
				value = KK_AG_WA_ans
			Case 7855
				value = KK_AG_WZ_ans

			Case 7890
				value = Fak_Proz_ans

			Case 8950
				value = CheckGebuehrZG_
			Case 8951
				value = UeberweisungGebuehrZG_
			Case 8952
				value = BarGebuehrZG_
			Case 9000
				value = CheckGebuehrAuszahlung_


			Case Else
				value = 0

		End Select

		GetLAValue = value

	End Function

	Function IsQSTCodeRight() As Boolean

		Dim bValue As Boolean = False
		Dim sMsgText As String

		If String.IsNullOrEmpty(m_EmployeeData.Q_Steuer) Then
			sMsgText = "Lohnabrechnung wird gelöscht: Fehlende Quellensteuercode. Bitte überprüfen sie die Angaben des Kandidaten und erstellen Sie die Lohnabrechnung erneut." & vbNewLine &
								  m_EmployeeData.Lastname & " " & m_EmployeeData.Firstname
			WriteToProtocol(Padright("*** -> IsQSTCodeRight: ", 30, " ") & sMsgText)

			bValue = False

		ElseIf m_EmployeeData.Q_Steuer <> "0" And m_EmployeeData.Q_Steuer <> "G" Then

			Try

				Dim ws = New SP.Internal.Automations.EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient

				ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxInfoServiceUrl)

				Dim dto = ws.LoadAllowedQstCode(m_InitializationData.MDData.MDGuid, strQSTKanton, LPYear, m_EmployeeData.ChildsCount.GetValueOrDefault(0), m_EmployeeData.Q_Steuer, m_EmployeeData.ChurchTax, m_EmployeeData.Gender)
				ThrowExceptionOnError(dto Is Nothing, "QST Code konnte nicht über Webservice geprüft werden.")

				If Not dto.IsQstDataAllowed Then
					Dim msg As String = String.Format("Lohnabrechnung wird gelöscht: Die Kombination des Quellensteuercodes ist ungültig. Bitte überprüfen sie die Angaben des Kandidaten und erstellen Sie die Lohnabrechnung erneut.{0}{1}, {2}{0}",
																										vbNewLine, m_EmployeeData.Lastname, m_EmployeeData.Firstname)
					msg &= String.Format("Kanton: {0}, LPYear: {1}, ChildsCount: {2}, Q_Steuer: {3}, ChurchTax: {4}, Gender: {5}",
																			 strQSTKanton, LPYear, m_EmployeeData.ChildsCount.GetValueOrDefault(0), m_EmployeeData.Q_Steuer, m_EmployeeData.ChurchTax, m_EmployeeData.Gender)

					WriteToProtocol(Padright("*** -> IsQSTCodeRight: ", 30, " ") & msg)
					bValue = False
				Else
					bValue = True
				End If

			Catch ex As Exception

				m_Logger.LogError(ex.ToString)
				Throw ex

			End Try
		Else
			bValue = True

		End If

		Return bValue

	End Function


End Class
