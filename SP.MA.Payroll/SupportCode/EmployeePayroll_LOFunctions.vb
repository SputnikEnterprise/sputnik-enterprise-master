Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.DatabaseAccess

Partial Public Class EmployeePayroll

	Private Function AddDataToLOLrec() As Boolean
		Dim success As Boolean = True

		GetRPLrec(True)
		GetLMrec(True)
		GetZGrec(True)

		InitialALLGAVGruppe0()
		InitialALLGAVGruppe1()

		success = success AndAlso AddRPLLOLrec()
		success = success AndAlso AddLMLOLrec()
		success = success AndAlso AddZGLOLrec()

		Return success
	End Function

	Private Function AddRPLLOLrec() As Boolean

		Dim success As Boolean = True
		Dim sMsgText As String

		If AllRPLOrec.Count = 0 Then
			' musst return true!
			m_Logger.LogWarning("no rp data in db!")
			Return success
		End If

		Div(0) = 0
		TempSuvaCode = "A1"

		Try

			For Each rplData In AllRPLOrec

				currRplData = rplData

				Div(0) = 0
				Div(1) = 0

				Kostenstelle1 = rplData.RPKst1
				Kostenstelle2 = rplData.RPKst2
				Kostenstelle3 = rplData.RPKst
				Dim strRPText = If(String.IsNullOrWhiteSpace(rplData.RPZusatzText), rplData.RPText, rplData.RPZusatzText)

				Dim laData = GetLAData(rplData.LANR, m_EmployeePayrollCommonData.LAData_Verwendung_1_3)

				If laData Is Nothing Then
					sMsgText = "Fehler in den Rapportlohnarten.{0}Rapportnummer: {1}{0}Lohnart: {2}{0}Kandidat:{3}{0}wurde nicht gefunden.{0}"
					sMsgText &= "Möglicherweise sollte die Lohnart anderes definiert werden.{0}{0}"
					sMsgText &= "Die Lohnart wird nicht in die Lohnabrechnung integriert."
					sMsgText = String.Format(m_Translate.GetSafeTranslationValue(sMsgText), vbNewLine, rplData.RPNR, rplData.LANR, m_EmployeeData.EmployeeNumber)

					WriteToProtocol(Padright("M -> (AddRPLLOLrec): ", 30, " ") & sMsgText)
					' TODO:

					'm_TaskHelper.InUIAndWait(Function()
					'													 m_UtilityUI.ShowErrorDialog(
					'														 m_Translate.GetSafeTranslationValue("Fehler in den Rapportlohnarten.") & vbLf &
					'															m_Translate.GetSafeTranslationValue("LO-Nr") & ": " & vbTab & LONewNr & vbLf &
					'														 m_Translate.GetSafeTranslationValue("Lohnart") & ": " & vbTab & rplData.LANR & vbLf &
					'														 m_Translate.GetSafeTranslationValue("Rapport") & ": " & vbTab & rplData.RPNR & vbLf & m_Translate.GetSafeTranslationValue("wurde nicht gefunden. ") &
					'														 m_Translate.GetSafeTranslationValue("Möglicherweise sollte die Lohnart anderes definiert werden.") & vbLf & vbLf &
					'														 m_Translate.GetSafeTranslationValue("Die Lohnart wird nicht in die Lohnabrechnung integriert."))
					'													 Return True
					'												 End Function)

				Else

					Dim rpVonWeekNr As Integer = DatePart(DateInterval.WeekOfYear, rplData.VonDate.Value, FirstDayOfWeek.System, FirstWeekOfYear.System)
					Dim rpBisWeekNr As Integer = DatePart(DateInterval.WeekOfYear, rplData.BisDate.Value, FirstDayOfWeek.System, FirstWeekOfYear.System)
					If rpVonWeekNr = 1 And rplData.VonDate.Value.Month = 12 Then rpVonWeekNr = 53
					If rpVonWeekNr > 50 And rplData.VonDate.Value.Month = 1 Then rpVonWeekNr = 1
					If rpBisWeekNr = 1 And rplData.VonDate.Value.Month = 12 Then rpBisWeekNr = 53
					If rpBisWeekNr > 50 And rplData.VonDate.Value.Month = 1 Then rpBisWeekNr = 1

					Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(rplData.LANR, m_EmployeeData.Language, laData.LALoText)

					Dim lol As New LOLMasterData
					lol.LONR = LONewNr
					lol.MANR = rplData.MANr
					lol.LANR = rplData.LANR
					lol.LP = LPMonth
					lol.Jahr = LPYear.ToString()
					lol.ModulName = "R"
					lol.Currency = Nothing
					lol.M_ANZ = rplData.M_Anzahl
					lol.M_BAS = rplData.M_Basis
					lol.M_ANS = rplData.M_Ansatz
					Dim rounding As Short = 2
					If laData.Rundung.GetValueOrDefault(0) > 2 Then
						lol.M_BTR = rplData.M_Betrag
					Else
						lol.M_BTR = NumberRound(rplData.M_Betrag, 2)
					End If

					lol.SUVA = If(String.IsNullOrWhiteSpace(rplData.SUVA), "A1", rplData.SUVA)
					lol.KST = Kostenstelle3
					lol.RPText = laTranslation & If(String.IsNullOrWhiteSpace(strRPText), "", " / " & strRPText)
					lol.AGLA = Nothing
					lol.S_Kanton = Nothing
					lol.Result = Nothing
					lol.KW = rpVonWeekNr
					lol.LOLKst1 = Kostenstelle1
					lol.LOLKst2 = Kostenstelle2
					lol.DestRPNr = rplData.RPNR
					lol.DestZGNr = Nothing
					lol.DestLMNr = Nothing
					lol.KW2 = rpBisWeekNr
					lol.ZGAusDate = Nothing
					lol.LMWithDTA = Nothing
					lol.ZGGrund = Nothing
					lol.BnkNr = Nothing
					lol.VGNr = Nothing
					lol.DTADate = Nothing
					lol.GAVNr = rplData.RPGAV_Nr
					lol.GAV_Kanton = rplData.RPGAV_Kanton
					lol.GAV_Beruf = rplData.RPGAV_Beruf
					lol.GAV_Gruppe1 = rplData.RPGAV_Gruppe1
					lol.GAV_Gruppe2 = rplData.RPGAV_Gruppe2
					lol.GAV_Gruppe3 = rplData.RPGAV_Gruppe3
					lol.GAV_Text = rplData.RPGAV_Text
					lol.DestESNr = rplData.ESNr
					lol.DateOfLO = LpDate
					lol.QSTGemeinde = Nothing
					lol.DestKDNr = rplData.KDNr
					lol.ESBranche = rplData.KDBranche
					lol.ESEinstufung = rplData.ES_Einstufung
					lol.MDNr = MDNr

					Dim succesAddNewLol = m_PayrollDatabaseAccess.AddNewLOL(lol)
					ThrowExceptionOnError(Not succesAddNewLol, "Datensatz (LOL) konnte nicht erstellt werden (RPL)")

					' Aktualisierung RP Datensatz verschoben auf Ende von RP Berechnung, falls etwas schlief läuft
					m_RPRecordsToUpdate.Add(rplData.RPNR)

					LOLAnzahl = rplData.M_Anzahl
					LOLBasis = rplData.M_Basis
					LOLAnsatz = rplData.M_Ansatz
					'LOLBetrag = NumberRound(rplData.M_Betrag, 2)

					If laData.Rundung.GetValueOrDefault(0) > 2 Then
						LOLBetrag = rplData.M_Betrag
					Else
						LOLBetrag = NumberRound(rplData.M_Betrag, 2)
					End If


					Dim suvaCode = If(String.IsNullOrWhiteSpace(rplData.SUVA), "A1", rplData.SUVA)
					TempSuvaCode = suvaCode

					If LOLAnzahl * Math.Round(LOLBasis, 2) * (LOLAnsatz / 100.0) > LOLBetrag + 0.5 Then

						sMsgText = "Lohnabrechnungsnummer: {1}{0}Bitte überprüfen Sie die Angaben in Ihrem Rapport: {2}, Lohnartennummer: {3:F3} ({4:d} - {5:d}), Woche: {6}.{0}"
						sMsgText &= "Anscheinend ist der Gesamtbetrag von {7:n2} ({8:n2} * {9:n2} * {10:n2}) falsch.{0}"
						sMsgText &= "Bitte löschen Sie die Lohnabrechnung und korrigieren den betroffenen Rapport."

						sMsgText = String.Format(sMsgText, vbNewLine, LONewNr, rplData.RPNR, rplData.LANR, rplData.VonDate,
												 rplData.BisDate, rpVonWeekNr, rplData.M_Betrag, rplData.M_Anzahl, rplData.M_Basis, rplData.M_Ansatz)
						WriteToProtocol(Padright("*** -> (AddRPLLOLrec): ", 40, " ") & sMsgText)

					End If

					' Die S-Variable werden gebildet
					GetSum_Var(laData)

					' Die U-Variable werden gebildet
					Try
						GetU_Var(laData, rplData.M_Anzahl, LOLBetrag, suvaCode, "R", 0)
					Catch ex As Exception
						sMsgText = "FEHLER: LONr: {1} | RPNr: {2} | LANr: {3} >>> Von/bis: {4}-{5}{0}{6}"
						sMsgText = String.Format(sMsgText, vbNewLine, LONewNr, rplData.RPNR, rplData.LANR, rplData.VonDate, rplData.BisDate, ex.ToString)
						WriteToProtocol(Padright("*** -> (AddRPLLOLrec): ", 40, " ") & sMsgText)

						ThrowExceptionOnError(True, sMsgText)

					End Try

				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try


		Return success

	End Function

	Private Function AddLMLOLrec() As Boolean
		Dim success As Boolean = True

		Dim TempLANr As Decimal
		Dim FuncNumber As Integer
		Dim bAutomatedKiAu As Boolean

		Dim TempLAAnzahl As Decimal
		Dim TempLABasis As Decimal
		Dim TempLAAnsatz As Decimal
		Dim TempLABetrag As Decimal
		Dim iFilterBedingung As Boolean = True
		Dim sMsgText As String

		If AllLMLOrec.Count = 0 Then
			' musst return true!
			m_Logger.LogWarning("no lm data in db!")
			Return success
		End If

		Try

			For Each lmData In AllLMLOrec

				currLMData = lmData

				Kostenstelle1 = lmData.LMKst1
				Kostenstelle2 = lmData.LMKst2
				Kostenstelle3 = lmData.Kst

				Dim laData = GetLAData(lmData.LANR, m_EmployeePayrollCommonData.LAData_Verwendung_2_3)

				If laData Is Nothing Then
					sMsgText = "Fehler in den monatlichen Lohnangaben.{0}Lohnart: {1}{0}Kandidat:{2}{0}wurde nicht gefunden.{0}"
					sMsgText &= "Möglicherweise sollte die Lohnart anderes definiert werden.{0}{0}"
					sMsgText &= "Die Lohnart wird nicht in die Lohnabrechnung integriert."
					sMsgText = String.Format(m_Translate.GetSafeTranslationValue(sMsgText), vbNewLine, lmData.LANR, m_EmployeeData.EmployeeNumber)

					WriteToProtocol(Padright("M -> (AddLMLOLrec): ", 30, " ") & sMsgText)
					' TODO:
					'm_TaskHelper.InUIAndWait(Function()
					'													 m_UtilityUI.ShowErrorDialog(
					'														 m_Translate.GetSafeTranslationValue("Fehler in den Monatlichen Lohnangaben.") & vbLf &
					'														 m_Translate.GetSafeTranslationValue("Lohnart") & ": " & vbTab & lmData.LANR & vbLf &
					'														 m_Translate.GetSafeTranslationValue("Kandidat") & ": " & vbTab & m_EmployeeData.EmployeeNumber & vbLf & m_Translate.GetSafeTranslationValue("wurde nicht gefunden. ") &
					'														 m_Translate.GetSafeTranslationValue("Möglicherweise sollte die Lohnart anderes definiert werden.") & vbLf & vbLf &
					'														 m_Translate.GetSafeTranslationValue("Die Lohnart wird nicht in die Lohnabrechnung integriert."))
					'													 Return True
					'												 End Function)
					Continue For
				End If
				'Else

				iFilterBedingung = SetFilterOnRecs(m_EmployeeData.EmployeeNumber, laData.Bedingung, laData.LALoText)

				If Not iFilterBedingung Then  ' die Filters bewerten
					sMsgText = m_Translate.GetSafeTranslationValue("Achtung: Der monatliche Lohnangabe ({0}) {1} konnte nicht validiert werden!")
					sMsgText = String.Format(sMsgText, laData.LANr, laData.LALoText)
					WriteToProtocol(Padright("M -> (AddLMLOLrec): ", 30, " ") & sMsgText)

					Continue For
				End If


				If String.IsNullOrEmpty(laData.RunFuncBefore) Then

					Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(lmData.LANR, m_EmployeeData.Language, lmData.LAName)

					Dim lol As New LOLMasterData
					lol.LONR = LONewNr
					lol.MANR = m_EmployeeData.EmployeeNumber
					lol.LANR = lmData.LANR
					lol.LP = LPMonth
					lol.Jahr = LPYear
					lol.ModulName = "L"
					lol.Currency = Nothing
					lol.M_ANZ = lmData.M_Anz
					lol.M_BAS = lmData.M_Bas
					lol.M_ANS = lmData.M_Ans
					lol.M_BTR = lmData.M_Btr
					lol.SUVA = lmData.SUVA
					lol.KST = Kostenstelle3
					lol.RPText = laTranslation & IIf(lmData.LAIndBez <> "", " / " & lmData.LAIndBez, "")
					lol.AGLA = Nothing
					lol.S_Kanton = If(Not String.IsNullOrWhiteSpace(lmData.Kanton), lmData.Kanton, strQSTKanton)
					lol.Result = Nothing
					lol.KW = Nothing
					lol.LOLKst1 = Kostenstelle1
					lol.LOLKst2 = Kostenstelle2
					lol.DestRPNr = Nothing
					lol.DestZGNr = Nothing
					lol.DestLMNr = lmData.LMNr
					lol.KW2 = Nothing
					lol.ZGAusDate = Nothing
					lol.LMWithDTA = lmData.LMWithDTA.HasValue AndAlso lmData.LMWithDTA
					lol.ZGGrund = lmData.ZGGrund
					lol.BnkNr = lmData.BnkNr
					lol.VGNr = Nothing
					lol.DTADate = Nothing
					lol.GAVNr = lmData.GAVNr
					lol.GAV_Kanton = lmData.GAVKanton
					lol.GAV_Beruf = lmData.GAVGruppe0
					lol.GAV_Gruppe1 = lmData.GAVGruppe1
					lol.GAV_Gruppe2 = lmData.GAVGruppe2
					lol.GAV_Gruppe3 = lmData.GAVGruppe3
					lol.GAV_Text = lmData.GAVBezeichnung
					lol.DestESNr = lmData.ESNr
					lol.DateOfLO = LpDate
					lol.QSTGemeinde = If(String.IsNullOrEmpty(m_EmployeeData.QSTCommunity), String.Empty, m_EmployeeData.QSTCommunity)
					lol.DestKDNr = lmData.KDNr
					lol.ESBranche = lmData.ESBranche
					lol.ESEinstufung = lmData.ESEinstufung
					lol.MDNr = MDNr

					If Not m_PayrollDatabaseAccess.AddNewLOL(lol) Then
						Throw New Exception("Datensatz (LOL) konnte nicht erstellt werden (LM).")
					End If

					LOLAnzahl = lmData.M_Anz
					LOLBasis = lmData.M_Bas
					LOLAnsatz = lmData.M_Ans
					LOLBetrag = lmData.M_Btr

					' Die S-Variable werden gebildet
					Call GetSum_Var(laData)

					' Die U-Variable werden gebildet
					Try
						Call GetU_Var(laData, lmData.M_Anz, LOLBetrag, lmData.SUVA, "L", 0)
					Catch ex As Exception
						sMsgText = "FEHLER: LONr: {1} >>> LANr: {2}{0}{3}"
						sMsgText = String.Format(sMsgText, vbNewLine, LONewNr, lmData.LANR, ex.ToString)
						WriteToProtocol(Padright("*** -> (AddLMLOLrec): ", 30, " ") & sMsgText)

						ThrowExceptionOnError(True, sMsgText)

					End Try

				Else
					FuncNumber = Val(Mid(laData.RunFuncBefore, 1, 2))

					TempLANr = laData.LANr
					TempLAAnzahl = lmData.M_Anz
					TempLABasis = lmData.M_Bas
					TempLAAnsatz = lmData.M_Ans
					TempLABetrag = lmData.M_Btr

					If FuncNumber = 13 Then     ' Kinderzulagen monatlich überprüfen (bis 31.12.2008)!!!

						Call Get_KinderZulageStatus(TempLANr, TempLAAnzahl, TempLABasis, TempLAAnsatz, TempLABetrag, lmData.Kanton)
						bAutomatedKiAu = True

					ElseIf FuncNumber = 14 Then     ' Ausbildungszulagen monatlich überprüfen (bis 31.12.2008)!!!
						Call Get_KinderAuStatus(TempLANr, TempLAAnzahl, TempLABasis, TempLAAnsatz, TempLABetrag, lmData.Kanton)
						bAutomatedKiAu = True


					ElseIf FuncNumber = 15 Or FuncNumber = 17 Then          ' Kinderzulagen monatlich überprüfen (ab 01.01.2009)!!!
						cBetragKIZulage = TempLABetrag

						bAutomatedKiAu = Get_KiData_2(TempLANr, TempLAAnzahl, TempLABasis, TempLAAnsatz, TempLABetrag, lmData.Kanton, FuncNumber = 17)
						'bAutomatedKiAu = True

					ElseIf FuncNumber = 16 Or FuncNumber = 18 Then          ' Ausbildungszulagen monatlich überprüfen (ab 01.01.2009)!!!
						cBetragAuZulage = TempLABetrag
						bAutomatedKiAu = Get_AuData_2(TempLANr, TempLAAnzahl, TempLABasis, TempLAAnsatz, TempLABetrag, lmData.Kanton, FuncNumber = 18)
						'bAutomatedKiAu = True

					Else
						bAutomatedKiAu = False

					End If
					If bAutomatedKiAu Then
						If Format(TempLAAnzahl * TempLABasis * TempLAAnsatz / 100, "0.00") <> Format(TempLABetrag, "0.00") Then
							sMsgText = "Achtung: möglicherweise sind die Ansätze für Kinder- /Ausbildungszulagen nicht richtig. Bitte kontrollieren Sie die Gesetzeskonstanten."
							WriteToProtocol(Padright("M -> (AddLMLOLrec): ", 30, " ") & sMsgText)

							TempLANr = laData.LANr
							TempLAAnzahl = lmData.M_Anz
							TempLABasis = lmData.M_Bas
							TempLAAnsatz = lmData.M_Ans
							TempLABetrag = lmData.M_Btr
						End If
					End If

					Dim tempLAList = m_PayrollDatabaseAccess.LoadLAData(LPYear, Nothing, TempLANr)
					If tempLAList Is Nothing Then
						Throw New Exception("Lohnart konnte nicht geladen werden. (" + TempLANr + ")")
					End If

					If tempLAList.Count = 0 Then
						sMsgText = String.Format("Fehler bei der Korrektur der Kinderzulagen. Bitte kontrollieren Sie die Lohnabrechnung mit Nummer {0}.", LONewNr)
						WriteToProtocol(Padright("M -> (AddLMLOLrec): ", 30, " ") & sMsgText)

					Else

						Dim TempLArec = tempLAList(0)

						Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(TempLArec.LANr, m_EmployeeData.Language, TempLArec.LALoText)

						Dim lol As New LOLMasterData
						lol.LONR = LONewNr
						lol.MANR = m_EmployeeData.EmployeeNumber
						lol.LANR = TempLArec.LANr
						lol.LP = LPMonth
						lol.Jahr = LPYear
						lol.ModulName = "L"
						lol.Currency = Nothing
						lol.M_ANZ = TempLAAnzahl
						lol.M_BAS = TempLABasis
						lol.M_ANS = TempLAAnsatz
						lol.M_BTR = TempLABetrag
						lol.SUVA = lmData.SUVA
						lol.KST = Kostenstelle3
						lol.RPText = laTranslation & IIf(lmData.LAIndBez <> "", " / " & lmData.LAIndBez, "")
						lol.AGLA = Nothing
						lol.S_Kanton = If(Not String.IsNullOrWhiteSpace(lmData.Kanton), lmData.Kanton, strQSTKanton)
						lol.Result = Nothing
						lol.KW = Nothing
						lol.LOLKst1 = Kostenstelle1
						lol.LOLKst2 = Kostenstelle2
						lol.DestRPNr = Nothing
						lol.DestZGNr = Nothing
						lol.DestLMNr = lmData.LMNr
						lol.KW2 = Nothing
						lol.ZGAusDate = Nothing
						lol.LMWithDTA = lmData.LMWithDTA.HasValue AndAlso lmData.LMWithDTA
						lol.ZGGrund = lmData.ZGGrund
						lol.BnkNr = lmData.BnkNr
						lol.VGNr = Nothing
						lol.DTADate = Nothing
						lol.GAVNr = lmData.GAVNr
						lol.GAV_Kanton = lmData.GAVKanton
						lol.GAV_Beruf = lmData.GAVGruppe0
						lol.GAV_Gruppe1 = lmData.GAVGruppe1
						lol.GAV_Gruppe2 = lmData.GAVGruppe2
						lol.GAV_Gruppe3 = lmData.GAVGruppe3
						lol.GAV_Text = lmData.GAVBezeichnung
						lol.DestESNr = lmData.ESNr
						lol.DateOfLO = LpDate
						lol.QSTGemeinde = If(String.IsNullOrEmpty(m_EmployeeData.QSTCommunity), String.Empty, m_EmployeeData.QSTCommunity)
						lol.DestKDNr = lmData.KDNr
						lol.ESBranche = lmData.ESBranche
						lol.ESEinstufung = lmData.ESEinstufung
						lol.MDNr = MDNr

						If Not m_PayrollDatabaseAccess.AddNewLOL(lol) Then
							Throw New Exception("Datensatz (LOL) konnte nicht erstellt werden (LM).")
						End If

						LOLAnzahl = TempLAAnzahl
						LOLBasis = TempLABasis ' * IIf(TempLArec!Vorzeichen = "-", -1, 1))
						LOLAnsatz = TempLAAnsatz
						LOLBetrag = TempLABetrag ' * IIf(TempLArec!Vorzeichen = "-", -1, 1))

						Call GetSum_Var(TempLArec)      ' Die S-Variable werden gebildet
						' Die U-Variable werden gebildet

						Try
							Call GetU_Var(TempLArec, 0, LOLBetrag, lmData.SUVA, "L", 0)
						Catch ex As Exception
							sMsgText = "FEHLER: LONr: {1} >>> LANr: {2}{0}{3}"
							sMsgText = String.Format(sMsgText, vbNewLine, LONewNr, lmData.LANR, ex.ToString)
							WriteToProtocol(Padright("*** -> (AddLMLOLrec): ", 30, " ") & sMsgText)

							ThrowExceptionOnError(True, sMsgText)

						End Try

					End If


				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		End Try


		Return success

	End Function

	Private Function AddZGLOLrec() As Boolean
		Dim success As Boolean = True
		Dim iCharForAbzug As Integer       ' Ascii Zeichen für Vorschüsse mit Abzug
		Dim sMsgText As String

		If AllZGLOrec.Count = 0 Then
			' musst return true!
			m_Logger.LogWarning("no zg data in db!")
			Return True
		End If

		Dim stringforadvancepaymentwithfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/stringforadvancepaymentwithfee", m_PayrollSetting))
		iCharForAbzug = Val(stringforadvancepaymentwithfee)

		'With AllZGLOrec
		Try

			For Each zgData In AllZGLOrec

				currZGData = zgData

				Dim laData = GetLAData(zgData.LANR, m_EmployeePayrollCommonData.LAData_Verwendung_4)

				If laData Is Nothing Then
					' Der Datensatz wurde nicht gefunden...
					sMsgText = "Fehler in den Vorschusslohnarten.{0}Lohnart: {1}{0}Vorschuss: {1}{0}Kandidat:{3}{0}wurde nicht gefunden.{0}"
					sMsgText &= "Möglicherweise sollte die Lohnart anderes definiert werden.{0}{0}"
					sMsgText &= "Die Lohnart wird nicht in die Lohnabrechnung integriert."
					sMsgText = String.Format(m_Translate.GetSafeTranslationValue(sMsgText), vbNewLine, zgData.LANR, zgData.ZGNr, m_EmployeeData.EmployeeNumber)

					WriteToProtocol(Padright("M -> (AddZGLOLrec): ", 30, " ") & sMsgText)

					' TODO:
					'm_TaskHelper.InUIAndWait(Function()
					'																m_UtilityUI.ShowErrorDialog(
					'																 m_Translate.GetSafeTranslationValue("Fehler in den Vorschusslohnarten.") & vbLf & _
					'																 m_Translate.GetSafeTranslationValue("Lohnart") & ": " & vbTab & zgData.LANR & vbLf & _
					'																 m_Translate.GetSafeTranslationValue("ZG-Nr") & ": " & vbTab & zgData.ZGNr & vbLf & m_Translate.GetSafeTranslationValue("wurde nicht gefunden. ") & _
					'																 m_Translate.GetSafeTranslationValue("Möglicherweise sollte die Lohnart anderes definiert werden.") & vbLf & vbLf & _
					'																 m_Translate.GetSafeTranslationValue("Die Lohnart wird nicht in die Lohnabrechnung integriert."))
					'																Return True
					'															End Function)


					Continue For
				End If
				'Else

				Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(zgData.LANR, m_EmployeeData.Language, laData.LALoText)

				Dim lol As New LOLMasterData
				lol.LONR = LONewNr
				lol.MANR = m_EmployeeData.EmployeeNumber
				lol.LANR = zgData.LANR
				lol.LP = LPMonth
				lol.Jahr = LPYear
				lol.ModulName = "Z"
				lol.Currency = Nothing
				lol.M_ANZ = zgData.Anzahl
				lol.M_BAS = zgData.Basis
				lol.M_ANS = zgData.Ansatz
				lol.M_BTR = zgData.Betrag
				lol.SUVA = Nothing
				lol.KST = Nothing
				lol.RPText = laTranslation & " " & IIf(zgData.GebAbzug And iCharForAbzug > 32, Chr(iCharForAbzug), "")
				lol.AGLA = Nothing
				lol.S_Kanton = Nothing
				lol.Result = Nothing
				lol.KW = Nothing
				lol.LOLKst1 = m_InitializationData.UserData.UserKST_1
				lol.LOLKst2 = m_InitializationData.UserData.UserKST_2
				lol.DestRPNr = Nothing
				lol.DestZGNr = zgData.ZGNr
				lol.DestLMNr = Nothing
				lol.KW2 = Nothing
				lol.ZGAusDate = zgData.Aus_Dat
				lol.LMWithDTA = Nothing
				lol.ZGGrund = zgData.ZGGRUND
				lol.BnkNr = Nothing
				lol.VGNr = Nothing
				lol.DTADate = Nothing
				lol.GAVNr = Nothing
				lol.GAV_Kanton = Nothing
				lol.GAV_Beruf = Nothing
				lol.GAV_Gruppe1 = Nothing
				lol.GAV_Gruppe2 = Nothing
				lol.GAV_Gruppe3 = Nothing
				lol.GAV_Text = Nothing
				lol.DestESNr = Nothing
				lol.DateOfLO = LpDate
				lol.QSTGemeinde = Nothing
				lol.DestKDNr = Nothing
				lol.ESBranche = Nothing
				lol.ESEinstufung = Nothing
				lol.MDNr = MDNr

				If Not m_PayrollDatabaseAccess.AddNewLOL(lol) Then
					Throw New Exception("Datensatz (LOL) konnte nicht erstellt werden (ZG).")
				End If

				' Aktualisierung ZG Datensatz verschoben auf Ende von RP Berechnung, falls etwas schlief läuft
				m_ZGRecordsToUpdate.Add(zgData.ZGNr)

				LOLAnzahl = zgData.Anzahl
				LOLBasis = zgData.Basis
				LOLAnsatz = zgData.Ansatz
				LOLBetrag = zgData.Betrag

				' Die S-Variable werden gebildet
				Call GetSum_Var(laData, zgData.GebAbzug, "ZG", zgData.BnkAu)
				' Die U-Variable werden gebildet
				Call GetU_Var(laData, 0, LOLBetrag, "0", "Z")


				'End If

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		'End With


		Return success

	End Function

	Function AddALaToLOL() As Boolean
		Dim result As Boolean = True
		Dim iFuncValue As Integer
		Dim cFuncNr As Decimal
		Dim iFilterBedingung As Boolean = True

		For Each TEmpRec In m_EmployeePayrollCommonData.LAData_Verwendung_0
			iFuncValue = 1

			iFilterBedingung = SetFilterOnRecs(m_EmployeeData.EmployeeNumber, TEmpRec.Bedingung, TEmpRec.LALoText)
			If iFilterBedingung Then   ' die Filters bewerten

				If TEmpRec.RunFuncBefore <> "" Then
					If InStr(1, TEmpRec.RunFuncBefore, ")") = 0 Then
						cFuncNr = Val(Mid(TEmpRec.RunFuncBefore, 1, Len(TEmpRec.RunFuncBefore)))
					Else
						cFuncNr = Val(Mid(TEmpRec.RunFuncBefore, 1, InStr(1, TEmpRec.RunFuncBefore, ")") - 1))
					End If

					iFuncValue = Math.Max(RunBasFunction(cFuncNr), 1)
				End If

				Select Case TEmpRec.LANr
					Case 11502
					Case 795, 795.1, 6989, 7395.1, 7395.2, 7395.3, 7395.4, 7395.5, 7395.6, 7395.7, 7400.1, 7420.1, 7835.1, 7835.2, 7835.3, 7835.4, 7835.5, 7835.6, 7835.7, 7840.1, 7850.1

						' Arbeitsstunden von GAV / Kanton, FAR, Weiterbildungsfond, Vollzugsfond, KTG Männer
						Call AddGAVALAToLOL(TEmpRec)

					Case Else

						Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(TEmpRec.LANr, m_EmployeeData.Language, TEmpRec.LALoText)

						' Rückstellungen von Ferien, Feiertag und 13. Lohn
						If TEmpRec.LANr = 502 OrElse TEmpRec.LANr = 602 OrElse TEmpRec.LANr = 702 Then
							RepeatLA4LOBack(TEmpRec.KumLANr, TEmpRec.LANr, TEmpRec.AGLA, laTranslation)

						ElseIf TEmpRec.KumLANr.GetValueOrDefault(0) <> 0 AndAlso (TEmpRec.Kumulativ.GetValueOrDefault(False) OrElse TEmpRec.KumulativMonth.GetValueOrDefault(False)) Then
							Call GetKumWert(TEmpRec)

						Else
							If TEmpRec.LANr = 7600 OrElse TEmpRec.LANr = 7620 Then
								If Not InitAutoLA(TEmpRec) Then Return False

							Else
								Call InitAutoLA(TEmpRec)   ' Hier wird der Datensatz gespeichert

							End If
						End If

						If (TEmpRec.Sum0Anzahl + TEmpRec.Sum1Anzahl + TEmpRec.Sum0Basis + TEmpRec.Sum1Basis + TEmpRec.Sum2Basis + TEmpRec.SumAnsatz + TEmpRec.Sum0Betrag + TEmpRec.Sum1Betrag + TEmpRec.Sum2Betrag + TEmpRec.Sum3Betrag) <> 0 Then
							Call GetSum_Var(TEmpRec)
						End If

						Call GetU_Var(TEmpRec, 0, LOLBetrag, "", "A")
				End Select

				'ElseIf iFilterBedingung = 2 Then
				'	Return False

			End If

		Next
		result = verifyCreatedPayrollData

		Return result
	End Function

	Private Function ExistLAInLO() As Boolean

		Dim exists As Boolean? = m_PayrollDatabaseAccess.CheckIfLAInLOExists(LONewNr, MDNr)
		ThrowExceptionOnError(Not exists.HasValue, "Prüfung auf existende LA in LO ist fehlgeschlagen.")

		Return exists

	End Function

	Private Function VerifyCreatedPayrollData() As Boolean
		Dim result As Boolean = True

		Dim bvgStartDate As Date? = BVGBeginForLO
		Dim bvgToDate As Date? = BVGEndForLO
		Try

			Dim verifyData = m_PayrollDatabaseAccess.VerifyUnusualPayrollData(MDNr, m_EmployeeData.EmployeeNumber, LONewNr, LPMonth, LPYear, m_InitializationData.UserData.UserNr, bvgStartDate, bvgToDate)
			ThrowExceptionOnError(verifyData Is Nothing, "Prüfung auf existente LA in LO ist fehlgeschlagen.")

			Dim msg As String = String.Empty
			If Not verifyData Is Nothing AndAlso verifyData.Count > 0 Then
				msg &= String.Format("Es existieren einige Warnungen für die Lohnabrechnung: {0}<br>Anzahl Warnungen: {1}<br><br>", LONewNr, verifyData.Count)

				Dim detail As String = String.Empty
				For Each rec In verifyData
					detail &= String.Format("<b>Grund:</b> {0}<br>", rec.Reason)
				Next
				msg &= detail
				WriteToProtocol(Padright("Überprüfung der Lohnabrechnung: ", 30, " ") & msg)

				m_UtilityUI.ShowOKDialog(msg, "Warnung", MessageBoxIcon.Asterisk)
			End If


		Catch ex As Exception

		End Try

		Return result

	End Function

	Function InitAutoLA(ByVal LArec As LAData) As Boolean

		Dim w_ValAnz As Decimal
		Dim w_ValBas As Decimal
		Dim w_ValAns As Decimal
		Dim w_ValBtr As Decimal
		Dim cFuncNr As Decimal
		Dim TempZahl As Decimal
		Dim Tempstr As String
		Dim Temp1 As String
		Dim strZGGrund As String = String.Empty
		Dim iBnkNr As Integer
		Dim sMsgText As String

		If LArec.LANr = 1000.05 OrElse LArec.LANr = 8800 Then
			Debug.Print(LArec.LANr)
		End If

		With LArec
			If .TypeAnzahl = 2 Or .TypeAnzahl = 3 Then  ' fester Wert
				w_ValAnz = .FixAnzahl

			ElseIf .TypeAnzahl = 4 Then                      ' S-Variable
				w_ValAnz = S(Val(IIf(Len(.MAAnzVar) > 2, .MAAnzVar * -1, .MAAnzVar))) * IIf(Len(.MAAnzVar) > 2, -1, 1)

			ElseIf .TypeAnzahl = 5 Then                             ' U-Variable
				w_ValAnz = U(Val(IIf(Len(.MAAnzVar) > 2, .MAAnzVar * -1, .MAAnzVar))) * IIf(Len(.MAAnzVar) > 2, -1, 1)

			ElseIf .TypeAnzahl = 6 Or .TypeAnzahl = 7 Then     ' S-Variable
				Tempstr = .MAAnzVar
				TempZahl = 0
				Do While InStr(1, Tempstr, "+") <> 0
					Temp1 = Mid(Tempstr, 1, 2)
					TempZahl = TempZahl + IIf(.TypeAnzahl = 6, S(Val(Temp1)), U(Val(Temp1)))
					Tempstr = Mid(Tempstr, 4)
				Loop
				w_ValAnz = TempZahl + IIf(.TypeAnzahl = 6, S(Val(Tempstr)), U(Val(Tempstr)))


				If LArec.LANr = 8730 Then
					' 04-17-37-38-39+47+48+49
					sMsgText = "LANr: {0} (1) >>> S(4): {2:n2} | S(17): {3:n2} | S(37): {4:n2} | S(38): {5:n2} | S(39): {6:n2} | S(47): {7:n2} | S(48): {8:n2} | S(49): {9:n2}"
					sMsgText = String.Format(sMsgText, LArec.LANr, LArec.MAAnzVar, S(4), S(17), S(37), S(38), S(39), S(47), S(48), S(49))

					WriteToProtocol(Padright("M -> (InitAutoLA): ", 30, " ") & sMsgText)
				End If


			ElseIf .TypeAnzahl = 8 Then  ' U-S Variable
					w_ValAnz = U(2) - S(6)

				ElseIf .TypeAnzahl = 13 Then                                        ' Systemkonstanten
					w_ValAnz = GetLAValue(.LANr)

			End If

			' Basiswert errechnen...
			w_ValBas = GetBasValue(.LANr, .TypeBasis, .MABasVar, .FixBasis)

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

				w_ValAns = RunBasFunction(cFuncNr, Val(.FixAnsatz))
				If w_ValAns = -1 And (LArec.LANr = 7600 Or LArec.LANr = 7620) Then
					WriteToProtocol(Padright("*** -> InitAutoLA (" & LArec.LANr & "): ", 30, " ") & "(-1) Fehler in Quellensteuercode...")
					Return False
				End If

			End If

			'  If LArec!lanr = 530 Or LArec!lanr = 630 Or LArec!lanr = 730 Then
			'    w_ValBtr = w_ValAnz * w_ValBas * (w_ValAns / 100) * IIf(!Vorzeichen = "-", -1, 1)
			'  Else
			w_ValBtr = Format(NumberRound((w_ValAnz * w_ValBas * w_ValAns) * IIf(.Vorzeichen = "-", -1, 1) / 100, IIf(Val(.Rundung) = 0, 2, Val(.Rundung))), "0.00")

			'End If

			' ahv-beitrag korrektur
			If S(52) <> 0 AndAlso LArec.LANr = 7190 Then

				Dim newBetrag As Decimal = Val(w_ValBtr) + S(52)
				w_ValBtr = newBetrag
				If S(6) <> 0 Then
					w_ValAnz = newBetrag / (Val(w_ValBas) * Val(w_ValAns) / 100)
				Else
					w_ValAnz = 1
					w_ValBas = newBetrag / (Val(w_ValAns) / 100)
				End If
				If w_ValAnz < 0 Then w_ValAnz = w_ValAnz * (-1)

				sMsgText = "Achtung: Da eine Korrektur von 7189.01 existiert muss ich eine Anpassung in Anzahl bei der Lohnart {0:F5}: {1} durchführen!"
				sMsgText = String.Format(sMsgText, LArec.LANr, LArec.LALoText)
				WriteToProtocol(Padright("M -> (InitAutoLA): ", 30, " ") & sMsgText)

			End If

			LOLAnzahl = Val(w_ValAnz)
			LOLBasis = Val(w_ValBas)
			LOLAnsatz = Val(w_ValAns)
			LOLBetrag = Val(w_ValBtr)

			If w_ValBtr = 0 AndAlso .WarningByZero.GetValueOrDefault(False) Then
				sMsgText = "Achtung: Für die Lohnabrechnung " & LONewNr & " existiert kein "
				sMsgText = sMsgText & LArec.LANr & " (" & LArec.LALoText & ") "
				WriteToProtocol(Padright("M -> (InitAutoLA): ", 30, " ") & sMsgText)

			End If

			If w_ValBtr = 0 And Not .ByNullCreate Then Return False
		End With

		Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(LArec.LANr, m_EmployeeData.Language, LArec.LALoText)

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
		lol.RPText = laTranslation
		lol.S_Kanton = strQSTKanton
		lol.QSTGemeinde = strQstGemeinde

		If LArec.LANr = 8730 Then
			lol.LMWithDTA = IIf(GetLMDTAStatus(strZGGrund, iBnkNr), True, False)
			lol.ZGGrund = strZGGrund
			lol.BnkNr = iBnkNr
		Else
			lol.LMWithDTA = False
			lol.ZGGrund = String.Empty
			lol.BnkNr = 0
		End If

		lol.Currency = m_EmployeeLOSetting.Currency
		lol.GAV_Kanton = String.Empty
		lol.GAV_Beruf = String.Empty
		lol.GAV_Gruppe1 = String.Empty
		lol.DateOfLO = LpDate
		lol.MDNr = MDNr

		Dim success = m_PayrollDatabaseAccess.AddNewLOL(lol)
		ThrowExceptionOnError(Not success, "LOL Datensatz konnte nicht angelegt werden (InitAutoLA)")


		Return True

	End Function

	Function GetLMDTAStatus(ByRef strZGGrund As String, ByRef iBnkNr As Integer) As Boolean
		Dim rTemprec As LOLDataForGetLMDTAStatus = Nothing
		Dim err As Boolean = False

		rTemprec = m_PayrollDatabaseAccess.LoadLOLDataForGetLMDTAStatus(m_EmployeeData.EmployeeNumber, LONewNr, err)
		ThrowExceptionOnError(err, "LOL Daten konnten nicht geladen werden (GetLMDTAStatus).")

		If rTemprec Is Nothing Then
			Exit Function

		Else
			strZGGrund = rTemprec.ZGGrund
			iBnkNr = rTemprec.BnkNr

			Dim succssUpdate = m_PayrollDatabaseAccess.UpdateLOLDataForGetLMDTAStatus(m_EmployeeData.EmployeeNumber, MDNr, LONewNr)
			ThrowExceptionOnError(Not succssUpdate, "LOL Datensatz konnte nicht aktualisiert werden (GetLMDTAStatus).")

			GetLMDTAStatus = True

		End If

	End Function

	Function AddLorec() As Boolean

		Dim strZahlart As String
		Dim err As Boolean = False

		' Variable zurücksetzen
		Call BlankSUVars()
		IsRentner = False
		IsToYoung = False
		LOLAnzahl = 0
		LOLBasis = 0
		LOLAnsatz = 0
		LOLBetrag = 0
		strESData = ""
		QSTTarif = ""
		strQSTKanton = ""
		strOriginData = ""
		cBetragKIZulage = 0
		cBetragAuZulage = 0

		Dim existingLONr As Integer? = m_PayrollDatabaseAccess.LoadExistLoForPayroll(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear, err)
		ThrowExceptionOnError(err, "Abfrage auf bereits existierende LO ist fehlgeschlagen.")

		If existingLONr.HasValue Then

			' toto:
			Dim msg_1 = "Für diese Lohnperiode existiert schon eine Lohnabrechnung mit der Nummer: {1}{0}. Möglicherweise wurde nach der Erstellung der Lohnabrechnung einen neuen Einsatz erfasst. Sie müssen die Lohnabrechnung löschen und neu erstellen."
			msg_1 = String.Format(msg_1, vbNewLine, existingLONr.Value)

			WriteToProtocol(msg_1)
			'm_TaskHelper.InUIAndWait(Function()
			'                              m_UtilityUI.ShowOKDialog("Für diese Lohnperiode existiert schon eine Lohnabrechnung mit der Nummer: " & _
			'                                              existingLONr.Value & "." & vbLf & _
			'                                              "Möglicherweise wurde nach der Erstellung der Lohnabrechnung einen " & _
			'                                              "neuen Einsatz erfasst. " & _
			'                                              "Sie müssen die Lohnabrechnung löschen und neu erstellen.", _
			'                                              "Lohnabrechnung erstellen", MessageBoxIcon.Exclamation)

			'                              Return True
			'                            End Function)

			Return False
		Else

			LONewNr = FindNewLONr()

			Dim newLORecord As New LOMasterData
			newLORecord.LONR = LONewNr
			newLORecord.MANR = m_EmployeeData.EmployeeNumber
			newLORecord.Jahr = LPYear
			newLORecord.MDNr = MDNr
			newLORecord.CreatedOn = DateTime.Now
			newLORecord.CreatedFrom = m_InitializationData.UserData.UserFullName
			newLORecord.CreatedUserNumber = m_InitializationData.UserData.UserNr

			Dim insertSuccessFully = m_PayrollDatabaseAccess.AddNewLO(newLORecord)
			ThrowExceptionOnError(Not insertSuccessFully, "Neuer LO Datensatz konnte nicht eingefügt werden.")
		End If

		Dim payrollluepaymentmethodifempty As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/payrollluepaymentmethodifempty", m_PayrollSetting))
		strZahlart = payrollluepaymentmethodifempty
		If String.IsNullOrWhiteSpace(strZahlart) Then strZahlart = "K"

		WriteToProtocol("Erstellen von LoNr: " & LONewNr & " -> " & IIf(UCase(m_EmployeeData.Gender) = "M", "Herrn ", "Frau ") & m_EmployeeData.EmployeeFullname)

		strQSTKanton = m_EmployeeData.S_Canton

		If Not IsQSTCodeRight() Then
			Return False
		End If

		m_IsBackBrutto = IsVacationBrutto(MDNr, LPYear)
		IsRentner = Rentner(m_EmployeeData.Birthdate, m_EmployeeData.Gender, m_EmployeeLOSetting.AHVCode, LpDate)
		IsToYoung = ToYoung(m_EmployeeData.Birthdate)

		Call GetQSTGemeinde()       ' Quellensteuergemeinde setzen...
		Dim msg As String = String.Empty

		strOriginData &= String.Format("MDNr: {0}|Birthdate: {1:d}|ChildsCount: {2}|ChurchTax: {3}", MDNr, m_EmployeeData.Birthdate, m_EmployeeData.ChildsCount, m_EmployeeData.ChurchTax)
		strOriginData &= String.Format("|AHV: {0}|ALV: {1}|BVG: {2}|KTG: {3}", m_EmployeeLOSetting.AHVCode, m_EmployeeLOSetting.ALVCode, m_EmployeeLOSetting.BVGCode, m_EmployeeLOSetting.KTGPflicht)
		strOriginData &= String.Format("|FeierBack: {0}|FerienBack: {1}|13Back: {2}|Gleitzeit: {3}", m_EmployeeLOSetting.FeierBack, m_EmployeeLOSetting.FerienBack, m_EmployeeLOSetting.Lohn13Back, m_EmployeeLOSetting.MAGleitzeit)

		m_Calculatebvgwithesdays = StrToBool(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/calculatebvgwithesdays", m_PayrollSetting)))

		Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		If m_BVGInterval = "" Then m_BVGInterval = "ww" ' Wochenweise
		If m_BVGAfter = 0 Then m_BVGAfter = 13 ' nach 13 Wochen
		m_iESBreakWeek = Math.Max(m_MandantData.BVG_Aus1Woche.Value, 13)

		strOriginData &= String.Format("|calculatebvgwithesdays: {0}|m_BVGInterval: {1}|m_BVGAfter: {2}|m_iESBreakWeek: {3}", m_Calculatebvgwithesdays, m_BVGInterval, m_BVGAfter, m_iESBreakWeek)

		' Geleistete Tage im gewählten Monat
		ESLPTage = m_PayrollDatabaseAccess.LoadESWorkDaysForAMonth(m_EmployeeData.EmployeeNumber, MDNr, LPYear, LPMonth) ' Get_WorkedDays_In_Month(MANumber)
		If ESLPTage = 0 AndAlso AllowedZeroWorkdaysWithSocialLA AndAlso ExistsSocialLMForEmployee() Then
			msg = String.Format(" >>> Achtung: Für {0} {1} existiert kein Einsatz im aktuellen Monat. Daher als Annahme wird voller Monat als Einsatzdauer verwendet! Bitte die Lohnabrechnung kontrollieren!!! <<<",
																														IIf(UCase(m_EmployeeData.Gender) = "M", "Herrn ", "Frau "), m_EmployeeData.EmployeeFullname)
			WriteToProtocol(msg)
			strOriginData &= String.Format("|ESLPTage: Keine Einsätze, es wird mit voller Monat berechnet!")
			ESLPTage = 30
		End If
		iESLP4QST = ESLPTage


		ESYearTage = GetESDayInYear(m_EmployeeData.EmployeeNumber)
		ESYearTage = Math.Max(ESLPTage, ESYearTage)

		' Kein PVL unterstellt...
		Dim bNoPVL As Boolean

		Dim companyallowednopvl As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)
		bNoPVL = (companyallowednopvl.HasValue AndAlso companyallowednopvl.Value) ' IIf(Val(DivReg.GetINIString(MDIniFullname, LoadResString(106), "NotPVL")) = 0, False, True)


		If bNoPVL Then
			bIs60KTGDay = False
		Else
			bIs60KTGDay = (Get_ES_Std_4_New_KTG() = 0)
		End If
		strOriginData &= String.Format("|ESLPTage: {0}|ESYearTage: {1}|bIs60KTGDay: {2}", ESLPTage, ESYearTage, bIs60KTGDay)
		strOriginData &= String.Format("|IsBackBrutto: {0}", m_IsBackBrutto)


		'ESYearTage = Stamm_Db.Execute("Get_WorkedDays " & MANumber & ", " & _
		'            LPMonth & ", " & LPYear)!WorkedDays + ESLPTage
		Return True

	End Function

	Sub SaveFinalDataToLO()

		Dim NewZGNr As Integer
		Dim ZGLANr As Decimal
		Dim t As Integer
		Dim h As Integer
		Dim Z As Integer
		Dim e As Integer
		Dim DispName As String
		Dim BetragAsStr As String
		Dim EmployeeGender As String
		Dim EmployeeNameWithComma As String
		Dim sMsgText As String

		DispName = m_InitializationData.UserData.UserFullName

		If S(11) > 0 Then
			NewZGNr = m_PayrollDatabaseAccess.LoadHighestZGNrForSaveFinalData()
		End If
		EmployeeGender = IIf(m_EmployeeData.Gender = "M", "Herrn", "Frau")
		EmployeeNameWithComma = String.Format("{0}, {1}", m_EmployeeData.Lastname, m_EmployeeData.Firstname)

		Dim endDataForLORec = New EndDataForLO
		endDataForLORec.LONewNr = LONewNr
		endDataForLORec.MANummer = m_EmployeeData.EmployeeNumber
		endDataForLORec.Kst1 = Kostenstelle1
		endDataForLORec.Kst2 = Kostenstelle2
		endDataForLORec.LP = LPMonth
		endDataForLORec.Jahr = LPYear
		endDataForLORec.S_Kanton = strQSTKanton
		endDataForLORec.QSTTarif = QSTTarif
		endDataForLORec.Zivilstand = m_EmployeeData.CivilStatus
		endDataForLORec.Kirchensteuer = m_EmployeeData.ChurchTax
		endDataForLORec.Q_Steuer = m_EmployeeData.Q_Steuer
		endDataForLORec.Kinder = m_EmployeeData.ChildsCount
		endDataForLORec.QSTBasis = U(49)
		endDataForLORec.strESData = strESData
		endDataForLORec.Wohnort = IIf(String.IsNullOrEmpty(m_EmployeeData.BirthPlace), m_EmployeeData.Location, m_EmployeeData.BirthPlace)
		endDataForLORec.CHPartner = m_EmployeeData.CHPartner
		endDataForLORec.Permission = m_EmployeeData.Permission
		endDataForLORec.PermissionToDate = m_EmployeeData.PermissionToDate
		endDataForLORec.NoSpecialTax = m_EmployeeData.NoSpecialTax
		endDataForLORec.EmployeePartnerRecID = m_EmployeeData.EmployeePartnerRecID
		endDataForLORec.EmployeeLOHistoryRecID = m_EmployeeData.EmployeeLOHistoryRecID
		endDataForLORec.WorkedDay = Convert.ToInt16(IIf(ESLPTage <> iESLP4QST, iESLP4QST, ESLPTage))
		endDataForLORec.Land = m_EmployeeData.Country
		endDataForLORec.Brutto = S(40)
		endDataForLORec.AHVBas = S(41)
		endDataForLORec.AHVLohn = S(42)
		endDataForLORec.AHVFrei = S(43)
		endDataForLORec.NAHVPf = S(44)
		endDataForLORec.ALV1Lohn = S(45)
		endDataForLORec.ALV2Lohn = S(46)
		endDataForLORec.SUVABas = 0
		endDataForLORec.MAName = EmployeeNameWithComma
		endDataForLORec.BVGBeginn = dFoundedBVGVon
		endDataForLORec.BVGEnde = dFoundedBVGBis
		endDataForLORec.MData = strOriginData
		endDataForLORec.ZGNumber = IIf(S(11) <= 0, -1, NewZGNr)
		endDataForLORec.BVGBegin = BVGBeginForLO
		endDataForLORec.BVGEnd = BVGEndForLO
		endDataForLORec.BVGDateData = m_BVGDateData
		endDataForLORec.MDNr = MDNr

		Dim successAddEndData = m_PayrollDatabaseAccess.AddEndDataToLOrec(endDataForLORec)
		ThrowExceptionOnError(Not successAddEndData, "LO Enddaten konnten nicht gespeichert werden.")

		Dim deletepayrollwithbruttozero As Boolean = StrToBool(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/deletepayrollwithbruttozero", m_PayrollSetting)))

		If S(11) < 0 Then           ' Die Lohnabrechnung ist negativ!!!

			Kostenstelle1 = m_InitializationData.UserData.UserKST_1
			Kostenstelle2 = m_InitializationData.UserData.UserKST_2
			Kostenstelle3 = m_InitializationData.UserData.UserKST

			Dim laLoText = m_PayrollDatabaseAccess.LoadLALoTextForSaveFinalData(LPYear)
			Dim laTranslation = m_EmployeePayrollCommonData.GetTranslatedLABez(8100, m_EmployeeData.Language, laLoText)

			Dim lmData = New LMDataForSaveFinalData
			lmData.LMNr = FindNewLMNr()
			lmData.MANr = m_EmployeeData.EmployeeNumber
			lmData.ESNr = 0
			lmData.LMKst1 = Kostenstelle1
			lmData.LMKst2 = Kostenstelle2
			lmData.KST = Kostenstelle3
			lmData.LANr = 8100
			lmData.LP_Von = Convert.ToInt16(IIf(LPMonth = 12, 1, LPMonth + 1))
			lmData.LP_Bis = Convert.ToInt16(IIf(LPMonth = 12, 1, LPMonth + 1))
			lmData.Jahr_von = Trim(Str(IIf(LPMonth = 12, LPYear + 1, LPYear)))
			lmData.Jahr_Bis = Trim(Str(IIf(LPMonth = 12, LPYear + 1, LPYear)))
			lmData.Suva = ""
			lmData.M_Anz = 1
			lmData.M_Bas = S(11)
			lmData.m_Ans = 100
			lmData.M_Btr = S(11)
			lmData.LAName = laTranslation
			lmData.FarPflicht = False
			lmData.MDNr = MDNr
			lmData.CreatedFrom = DispName
			lmData.CreatedOn = Now
			lmData.ChangedFrom = DispName
			lmData.ChangedOn = Now

			Dim successAddLMData = m_PayrollDatabaseAccess.AddLMDataForSaveFinalData(lmData)
			ThrowExceptionOnError(Not successAddLMData, "LM Daten konnten nicht gespeichert werden.")

			sMsgText = String.Format("Lohnabrechnung für {0} {1} wurde erfolgreich erstellt. Da aber der Endbetrag negativ ist, habe ich die Lohnart 'Minuslohnvortrag' mit Betrag {2:n2} für den nächsten Monat übernommen.",
																																		 EmployeeGender, EmployeeNameWithComma, S(11))
			WriteToProtocol(Padright("M -> (SaveFinalDataToLO): ", 30, " ") & m_Translate.GetSafeTranslationValue(sMsgText))

			Dim successUpdateLO = m_PayrollDatabaseAccess.UpdateLOForSaveFinalData(GetLastLMID, LONewNr)
			ThrowExceptionOnError(Not successUpdateLO, "LO Datensatz konnte nicht aktualisiert werden.")

			Return       ' Keine weitere Vorgehen...

		ElseIf S(11) = 0 And U(1) = 0 And deletepayrollwithbruttozero Then          ' Die Auszahlung und Bruttolohn = 0

			Dim succDeleteLOL = m_PayrollDatabaseAccess.DeleteFromLOL(LONewNr)
			ThrowExceptionOnError(Not succDeleteLOL, "LOL Datensätze konnten nicht gelöscht werden.")

			Return       ' ist bereits gelöscht...

		ElseIf S(11) = 0 Then
			Return       ' Beenden bereits auf 0...
		End If

		' Lohnart suchen für Vorschussdatensatz
		Dim listOfLANr = m_PayrollDatabaseAccess.LoadLolLANrDataForSaveFinalData(m_EmployeeData.EmployeeNumber, MDNr, LONewNr, LPMonth, LPYear)
		ThrowExceptionOnError(listOfLANr Is Nothing, "LANr Daten konnten nicht von LOL geladen werden.")

		If listOfLANr.Count = 0 Then Exit Sub

		Dim lanr = listOfLANr(0)
		Select Case lanr
			Case 9300               ' überweisung
				ZGLANr = 8920

			Case 9200               ' Barauszahlung
				ZGLANr = 8930

			Case 9100               ' Checkauszahlung
				ZGLANr = 8900

			Case Else
				ZGLANr = 8930        ' Ansonsten soll sie auf bar rauskommen...

		End Select

		Dim employeeBankData = m_PayrollDatabaseAccess.LoadEmployeeBnkDataForPayroll(m_EmployeeData.EmployeeNumber, LONewNr)

		If employeeBankData Is Nothing And ZGLANr = 8920 Then
			sMsgText = String.Format("Sie haben noch keine Bankverbindung für {0} {1} erstellt. Ihre Lohnabrechnung kann nicht mit Kontoüberweisung erstellt werden. Die Lohnabrechnung wird mit 'Auszahlung Bar' gebucht.",
																																		EmployeeGender, EmployeeNameWithComma)

			WriteToProtocol(Padright("M -> (SaveFinalDataToLO): ", 30, " ") & m_Translate.GetSafeTranslationValue(sMsgText))

			ZGLANr = 8930

			' Die Lohnart = 9300 ('Auszahlung auf Ihr Konto') in Bar ändern...
			Dim loTranslationForUpdate = m_EmployeePayrollCommonData.GetTranslatedLABez(9200, m_EmployeeData.Language, m_Translate.GetSafeTranslationValue("Auszahlung Bar"))
			Dim succUpdateLOL = m_PayrollDatabaseAccess.UpdateLOLForSaveFinalData(m_EmployeeData.EmployeeNumber, LONewNr, loTranslationForUpdate)
			ThrowExceptionOnError(Not succUpdateLOL, "LOL Datensatz konnte nicht aktualisiert werden.")
		End If

		t = Int(S(11) * 0.001)
		h = Int(S(11) * 0.01) - t * 10
		Z = Int(S(11) * 0.1) - t * 100 - h * 10
		e = Int(S(11)) - t * 1000 - h * 100 - Z * 10

		Dim zgData As New DatabaseAccess.AdvancePaymentMng.DataObjects.ZGMasterData
		zgData.ZGNr = NewZGNr
		zgData.RPNR = 0
		zgData.VGNR = 0
		zgData.MANR = m_EmployeeData.EmployeeNumber
		zgData.LANR = ZGLANr
		zgData.Currency = m_EmployeeLOSetting.Currency
		zgData.Anzahl = 1
		zgData.Basis = (S(11) * -1)
		zgData.Ansatz = 100
		zgData.Betrag = (S(11) * -1)
		zgData.LP = LPMonth
		zgData.JAHR = LPYear
		zgData.ZGGRUND = String.Format(m_Translate.GetSafeTranslationValue("Lohnabrechnungsnr.: {0}"), LONewNr)

		If ZGLANr = 8920 Then            ' Kontoüberweisung
			zgData.Aus_Dat = DateTime.Now.Date
			zgData.ClearingNr = employeeBankData.DTABCNR
			zgData.BLZ = employeeBankData.BLZ
			zgData.Bank = employeeBankData.Bank
			zgData.BankOrt = employeeBankData.BankOrt
			zgData.Swift = employeeBankData.Swift
			zgData.KontoNr = employeeBankData.KontoNr
			zgData.IBANNr = employeeBankData.IBANNr
			zgData.DTAAdr1 = employeeBankData.DTAAdr1
			zgData.DTAAdr2 = employeeBankData.DTAAdr2
			zgData.DTAAdr3 = employeeBankData.DTAAdr3
			zgData.DTAAdr4 = employeeBankData.DTAAdr4
			zgData.N2Char = m_NumberToWordConverter.ConvertNumberToGermanText(S(11))

			If Not employeeBankData.BnkAu.HasValue Then
				zgData.BnkAU = 0
			Else
				zgData.BnkAU = employeeBankData.BnkAu.HasValue AndAlso employeeBankData.BnkAu.Value
			End If

		Else
			zgData.Aus_Dat = DateTime.Now.Date
			zgData.ClearingNr = 0
			zgData.BLZ = String.Empty
			zgData.Bank = String.Empty
			zgData.BankOrt = String.Empty
			zgData.Swift = String.Empty
			zgData.KontoNr = String.Empty
			zgData.IBANNr = String.Empty
			zgData.DTAAdr1 = String.Empty
			zgData.DTAAdr2 = String.Empty
			zgData.DTAAdr3 = String.Empty
			zgData.DTAAdr4 = String.Empty
			zgData.N2Char = m_NumberToWordConverter.ConvertNumberToGermanText(S(11))
			zgData.BnkAU = 0
		End If

		BetragAsStr = Trim(Str(CDbl(S(11))))
		If InStr(1, BetragAsStr, ".") = 0 Then
			BetragAsStr = BetragAsStr & ".00"
		End If
		If Len(Mid(BetragAsStr, 1, InStr(1, BetragAsStr, ".") - 1)) > 7 Then
			sMsgText = "Der Betrag ist zu hoch und kann nicht gültig sein. Bitte teilen Sie den Betrag in mehreren Vorschüsse."

			WriteToProtocol(Padright("Exit -> (SaveFinalDataToLO): ", 40, " ") & m_Translate.GetSafeTranslationValue(sMsgText))

			Return
		End If
		BetragAsStr = Mid(BetragAsStr, 1, InStr(1, BetragAsStr, ".") - 1)

		Dim digits(BetragAsStr.Length - 1) As String

		For i As Integer = 0 To BetragAsStr.Length - 1
			digits(i) = BetragAsStr.Substring(i, 1)
		Next i

		zgData._1000000 = If(digits.Count > 6, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 7)), String.Empty)
		zgData._100000 = If(digits.Count > 5, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 6)), String.Empty)
		zgData._10000 = If(digits.Count > 4, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 5)), String.Empty)
		zgData._1000 = If(digits.Count > 3, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 4)), String.Empty)
		zgData._100 = If(digits.Count > 2, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 3)), String.Empty)
		zgData._10 = If(digits.Count > 1, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 2)), String.Empty)
		zgData._1 = If(digits.Count > 0, m_NumberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 1)), String.Empty)

		Dim initDate = DateTime.Now

		zgData.USName = DispName
		zgData.CheckNumber = 0
		zgData.LONR = LONewNr
		zgData.MDNr = MDNr
		zgData.CreatedFrom = m_InitializationData.UserData.UserFullName
		zgData.CreatedOn = initDate
		zgData.ChangedFrom = m_InitializationData.UserData.UserFullName
		zgData.ChangedOn = initDate
		zgData.IsCreatedWithLO = True

		Dim successAddZG = m_AdvancePaymentDatabaseAccess.AddNewZGData(zgData, ReadZGOffsetFromSettings)
		ThrowExceptionOnError(Not successAddZG, "Vorschussdatensatz konnte nicht angelegt werden")


	End Sub

	Function IsESBeginOrEndInMonth() As Boolean
		Dim result As Boolean = False
		Dim i As Integer
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date
		Dim dESEnde As Date

		dStartofMonth = StartofCurrentPayrollMonth
		dEndofMonth = EndofCurrentPayrollMonth

		Dim esDataList2 = m_PayrollDatabaseAccess.LoadESData2ForQSTDataForm(m_EmployeeData.EmployeeNumber, MDNr, dStartofMonth, dEndofMonth)
		ThrowExceptionOnError(esDataList2 Is Nothing, "Einsatz(2) konnten nicht geladen werden.")
		i = 0

		Dim currentESRec As ESData2ForQSTDataForm = Nothing

		If esDataList2.Count > 0 Then
			currentESRec = esDataList2(0)
		End If

		If Not currentESRec Is Nothing Then

			Do While Not currentESRec Is Nothing

				If Not currentESRec.ES_Ende.HasValue Then
					' first of month is deactivated!
					If currentESRec.ES_Ab > dStartofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						result = True
					End If

				Else

					If currentESRec.ES_Ab <= dStartofMonth AndAlso currentESRec.ES_Ende >= dEndofMonth Then
						' nix machen

					ElseIf currentESRec.ES_Ab > dStartofMonth OrElse currentESRec.ES_Ende < dEndofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						result = True

					ElseIf currentESRec.ES_Ab > dStartofMonth AndAlso currentESRec.ES_Ende > dEndofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						result = True

					ElseIf currentESRec.ES_Ab < dEndofMonth AndAlso currentESRec.ES_Ende < dEndofMonth Then
						' ist Beginn oder Ende im gleichen Monat
						result = True

					ElseIf currentESRec.ES_Ende < dEndofMonth Then
						result = True

					End If

				End If

				dESEnde = Format(IIf(Not currentESRec.ES_Ende.HasValue, CDate("31.12.2999"), currentESRec.ES_Ende))

				i += 1
				If i > esDataList2.Count - 1 Then
					currentESRec = Nothing
				Else
					currentESRec = esDataList2(i)
				End If
			Loop

		End If


		Return result

	End Function


	Private Sub UpdateZGAndRP()

		Dim success As Boolean = False
		Dim payrollDBBase As DatabaseAccessBase = CType(m_PayrollDatabaseAccess, DatabaseAccessBase)
		Try

			payrollDBBase.OpenExplicitConnection()
			payrollDBBase.OpenExplicitTransaction()

			For Each rpNr In m_RPRecordsToUpdate
				Dim updateSuccess = m_PayrollDatabaseAccess.UpdateRPDataWithLONrForPayroll(LONewNr, rpNr)
				ThrowExceptionOnError(Not updateSuccess, "Aktualisierung RP Data mit LONr ist fehlgeschlagen.")
			Next

			For Each zgNr In m_ZGRecordsToUpdate
				Dim updateSuccess = m_PayrollDatabaseAccess.UpdateZGDataWithLONrForPayroll(LONewNr, zgNr)
				ThrowExceptionOnError(Not updateSuccess, "Aktualisierung ZG Data mit LONr ist fehlgeschlagen.")
			Next
			success = True

		Finally
			If success Then
				payrollDBBase.CommitExplicitTransaction()
			Else
				payrollDBBase.RollbackExplicitTransaction()
			End If

			payrollDBBase.CloseExplicitConnection()

		End Try

	End Sub

	Private Sub UpdateEmployeeLOBackSettings()

		If SetEmployeeLOBackSetting Then

			Dim employeeferienback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(MDNr), String.Format("{0}/employeeferienback", FORM_XML_MAIN_KEY)), False)
			Dim employeefeiertagback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(MDNr), String.Format("{0}/employeefeiertagback", FORM_XML_MAIN_KEY)), False)
			Dim employee13lohnback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(MDNr), String.Format("{0}/employee13lohnback", FORM_XML_MAIN_KEY)), False)

			' TODO Fardin: Gleitzeit einbauen

			' Reload employee LO setting before update
			m_EmployeeLOSetting = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(m_EmployeeData.EmployeeNumber)

			If Not m_EmployeeLOSetting.FeierBack.GetValueOrDefault(False) Then m_EmployeeLOSetting.FerienBack = (employeeferienback.HasValue AndAlso employeeferienback.Value)
			If Not m_EmployeeLOSetting.FeierBack.GetValueOrDefault(False) Then m_EmployeeLOSetting.FeierBack = (employeefeiertagback.HasValue AndAlso employeefeiertagback.Value)
			If Not m_EmployeeLOSetting.Lohn13Back.GetValueOrDefault(False) Then m_EmployeeLOSetting.Lohn13Back = (employee13lohnback.HasValue AndAlso employee13lohnback.Value)

			Dim success = m_EmployeeDatabaseAccess.UpdateEmployeeLOSettings(m_EmployeeLOSetting)
			ThrowExceptionOnError(Not success, "Rückstellungseinstellungen konnten nicht aktualisiert werden.")

		End If
	End Sub

	''' <summary>
	''' Reads the ZG offset from the settings.
	''' </summary>
	''' <returns>ZG offset or zero if it could not be read.</returns>
	Private Function ReadZGOffsetFromSettings() As Integer

		Dim zgNumberStartNumberSetting As String = m_md.GetSelectedMDProfilValue(MDNr, LPYear, "StartNr", "Vorschussverwaltung", 0)
		Dim intVal As Integer

		If Integer.TryParse(zgNumberStartNumberSetting, intVal) Then
			Return intVal
		Else
			Return 0
		End If

	End Function

	Private Function IsLOFinished() As Boolean

		Dim isFinished As Boolean? = m_PayrollDatabaseAccess.IsLOFinished(LONewNr, MDNr)
		ThrowExceptionOnError(Not isFinished.HasValue, "Abschlussprüfung LO ist fehlgeschlagen.")

		Return isFinished.Value
	End Function

End Class
