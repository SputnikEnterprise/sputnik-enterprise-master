Public Class EmployeePayroll

	'Private m_BVGInterval As String
	'Private m_BVGAfter As Integer
	'Private m_iESBreakWeek As Integer


	Private Function Get_ES_Std_4_New_KTG() As Decimal
		Dim bArbTage As Boolean
		Dim result As Decimal = 0

		bArbTage = m_Calculatebvgwithesdays

		'Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		'Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		'If m_BVGInterval = "" Then m_BVGInterval = "ww" ' Wochenweise
		'If m_BVGAfter = 0 Then m_BVGAfter = 13 ' nach 13 Wochen
		'm_iESBreakWeek = Math.Max(m_MandantData.BVG_Aus1Woche.Value, 13)

		'strOriginData &= String.Format("|calculatebvgwithesdays: {0}|strBVGInterval: {1}|iBVGAfter: {2}|iESBreakWeek: {3}", bArbTage, m_BVGInterval, m_BVGAfter, m_iESBreakWeek)


		If m_EmployeeLOSetting.BVGCode = 1 Then
			result = 1

		Else
			If bArbTage Then
				result = Get_ES_Std_4_New_KTG_1()

			Else
				result = Get_ES_Std_4_New_KTG_0()
			End If

		End If

		Return result
	End Function

	Private Function Get_ES_Std_4_New_KTG_0() As Decimal

		Dim dStartofYear As Date
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date
		Dim dEndofYear As Date
		Dim dOldRPVon As Date
		Dim dOldRPBis As Date

		Dim cKTGStd As Decimal
		'Dim i As Integer
		'Dim strBVGInterval As String
		'Dim iBVGAfter As Integer
		'  Dim iESBreakWeek As Integer

		'Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		'  Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		'  strBVGInterval = bvginterval ' DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGIntervalString")
		'  iBVGAfter = Val(bvgintervaladd) 'Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGIntervalAdd"))

		'  If strBVGInterval = "" Then strBVGInterval = "ww" ' Wochenweise
		'  If iBVGAfter = 0 Then iBVGAfter = 13 ' nach 13 Wochen
		'iBVGAfter = iBVGAfter - 1

		'  Dim anzLO As Integer? = m_PayrollDatabaseAccess.LoadAnzLO(m_EmployeeData.EmployeeNumber, MDNr)
		'  ThrowExceptionOnError(Not anzLO.HasValue, "Anzahl LO konnte nicht geladen werden.")


		'  If anzLO.Value <= 6 Then
		'    iESBreakWeek = Math.Max(Val(m_MandantData.BVG_Aus1Woche), 2)

		'  Else
		'	iESBreakWeek = IIf(Val((m_MandantData.BVG_Aus1Woche)) = 0, 52, Val((m_MandantData.BVG_Aus1Woche)))

		'End If
		''iESBreakWeek = iESBreakWeek - 1
		'strOriginData &= "|KTG-Data (Get_ES_Std_4_New_KTG_0):" & "#strBVGInterval: " & strBVGInterval & "#iBVGAfter: " & iBVGAfter & "#iESBreakWeek: " & iESBreakWeek & "# "

		dStartofMonth = CDate("01." & LPMonth & "." & LPYear)
		dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)
		dStartofYear = CDate("01.01." & LPYear)
		dEndofYear = CDate("31.12." & LPYear)

		' Kandidat hat Priortät 1
		If m_EmployeeLOSetting.BVGCode = 1 Then       ' 1; Ab 1. Tag
			cKTGStd = 1 ' S(2)

		Else                                ' 9; Ab 540 Stunden oder nach Einsatz
			cKTGStd = 0

			Dim rpData = m_PayrollDatabaseAccess.LoadRPDataForESStd4NewKTG0Calculation(m_EmployeeData.EmployeeNumber, MDNr, DateAdd("m", -CInt(Math.Max(8, (m_iESBreakWeek * 7 / 30))), dStartofMonth), DateAdd("m", 1, dStartofMonth))
			ThrowExceptionOnError(rpData Is Nothing, "RP Daten für ES_Std_4_New_KTG_0 konnten nicht geladen werden.")

			If rpData.Count = 0 Then
				'      S(7) = Minimum(cKTGStd, S(2))
				Return cKTGStd
			End If

			Dim rpRec = rpData(0)

			' zum Ersten mal werden die Von und Bis Daten gespeichert...
			dOldRPVon = rpRec.Von
			dOldRPBis = rpRec.Bis

			If rpRec.Monat = LPMonth And Convert.ToInt32(rpRec.Jahr) = LPYear Then
				' Es ist der erste Monat wo er arbeitet; dann ist es nichts!!!
				cKTGStd = 0

			Else

				For rpIndex As Integer = 1 To rpData.Count - 1

					rpRec = rpData(rpIndex)

					'If Not rRPrec.EOF Then
					If dOldRPVon < rpRec.Von Then
						If dOldRPBis >= rpRec.Von Then
							If dOldRPBis <= rpRec.Bis Then _
							  dOldRPBis = rpRec.Bis

						ElseIf dOldRPBis < rpRec.Von Then
							' Das Ende der letzte Rapport ist vor dem neuen Rapportbeginn: kann pflichtig werden
							If DateDiff(m_BVGInterval, dOldRPBis, rpRec.Von,
														vbUseSystemDayOfWeek, vbUseSystem) > m_iESBreakWeek Then
								' dann fängt alles von 0 an...
								dOldRPVon = rpRec.Von
								dOldRPBis = rpRec.Bis

							Else

								dOldRPBis = rpRec.Bis
							End If
						End If

					End If

					If DateDiff(m_BVGInterval, dOldRPVon, dOldRPBis,
														vbUseSystemDayOfWeek, vbUseSystem) > m_BVGAfter Then
						' es ist dann BVG-pflichtig!!!
						If rpRec.Monat = LPMonth And
													 rpRec.Jahr = LPYear Then
							cKTGStd = cKTGStd + GetBVGStdFromRPL_0(rpRec.RPNr, dOldRPVon, dOldRPBis)
						End If
					End If
					' End If

				Next
				'    End If

			End If

		End If
		'S(7) = Minimum(cKTGStd, S(2))
		'' es sollte nur bis einer Maximum 180 Stunden pro Monat kommen
		'S(7) = Minimum(S(7), Val(DivFunc.vFieldVal(MDrec![BVG_Std])) / 12)

		Return Math.Min(cKTGStd, Val(m_MandantData.BVG_Std) / 12)

	End Function

	' Neue Funktionen für BVG-Abrechnung.
	' Achtung nicht 3 Monate mit Ketteneinsätzen sonder nach Einsatzdauer...
	Private Function Get_ES_Std_4_New_KTG_1() As Decimal
		Dim iAnzAlteDauer As Integer

		Dim dStartofYear As Date
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date
		Dim dEndofYear As Date
		Dim dOldRPVon As Date
		Dim dOldRPBis As Date

		Dim cKTGStd As Decimal
		'Dim i As Integer
		'  Dim strBVGInterval As String
		'  Dim iBVGAfter As Integer
		'  Dim iESBreakWeek As Integer

		'  Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		'  Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		'  strBVGInterval = bvginterval ' DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGIntervalString")
		'  iBVGAfter = Val(bvgintervaladd) 'Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGIntervalAdd"))

		'  strBVGInterval = "ww" ' Wochenweise
		'  If iBVGAfter = 0 Then iBVGAfter = 13 ' nach 13 Wochen
		'' Achtung: Hier darf er nicht um eins substrahieren, da er hier pro Tag rechnet;
		'' ab 91sten Tag ist der Kandidat BVG-pflicht.

		'iESBreakWeek = Math.Max(m_MandantData.BVG_Aus1Woche.Value, 13)
		''iESBreakWeek = iESBreakWeek - 1
		'strOriginData &= "|KTG-Data (Get_ES_Std_4_New_KTG_1):" & "#strBVGInterval: " & strBVGInterval & "#iBVGAfter: " & iBVGAfter & "#iESBreakWeek: " & iESBreakWeek & "# "

		dStartofMonth = CDate("01." & LPMonth & "." & LPYear)
		dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)
		dStartofYear = CDate("01.01." & LPYear)
		dEndofYear = CDate("31.12." & LPYear)

		' Kandidat hat Priortät 1
		If m_EmployeeLOSetting.BVGCode = 1 Then       ' 1; Ab 1. Tag
			cKTGStd = 1 ' S(2)

		Else                                ' 9; Ab 540 Stunden oder nach Einsatz
			' Der Mann arbeitet neu: Der BVG-pflicht fängt von 0 an
			cKTGStd = 0

			Dim rpData = m_PayrollDatabaseAccess.LoadRPDataForESStd4NewKTG1Calculation(m_EmployeeData.EmployeeNumber, MDNr, DateAdd("m", -CInt(Math.Max(8, (m_iESBreakWeek * 7 / 30))), dStartofMonth), DateAdd("m", 1, dStartofMonth))
			ThrowExceptionOnError(rpData Is Nothing, "RP Daten für ES_Std_4_New_KTG_1 konnten nicht geladen werden.")

			If rpData.Count = 0 Then
				'      S(7) = Minimum(cKTGStd, S(2))
				Return cKTGStd
			End If

			Dim rpRec = rpData(0)

			' zum Ersten mal werden die Von und Bis Daten gespeichert...
			dOldRPVon = rpRec.Von
			dOldRPBis = rpRec.Bis
			iAnzAlteDauer = rpRec.ESRPTage

			If rpRec.Monat = LPMonth And Convert.ToInt32(rpRec.Jahr) = LPYear Then
				' Es ist der erste Monat wo er arbeitet; dann ist es nichts!!!
				cKTGStd = 0

			Else

				For rpIndex As Integer = 1 To rpData.Count - 1

					rpRec = rpData(rpIndex)

					' If Not rpRec.EOF Then
					If dOldRPVon < rpRec.Von Then
						If dOldRPBis >= rpRec.Von Then
							If dOldRPBis <= rpRec.Bis Then _
							  dOldRPBis = rpRec.Bis

						ElseIf dOldRPBis < rpRec.Von Then
							' Das Ende der letzte Rapport ist vor dem neuen Rapportbeginn: kann pflichtig werden
							If DateDiff(m_BVGInterval, dOldRPBis, rpRec.Von, vbUseSystemDayOfWeek, vbUseSystem) + 1 > m_iESBreakWeek Then
								' dann fängt alles von 0 an...
								dOldRPVon = rpRec.Von
								dOldRPBis = rpRec.Bis
								iAnzAlteDauer = Val(rpRec.ESRPTage)

							Else

								dOldRPVon = rpRec.Von
								dOldRPBis = rpRec.Bis
								iAnzAlteDauer = iAnzAlteDauer + Val(rpRec.ESRPTage)
							End If
						End If

					End If

					If iAnzAlteDauer >= (m_BVGAfter * 7) Then
						' es ist dann BVG-pflichtig!!!
						If rpRec.Monat = LPMonth And rpRec.Jahr = LPYear Then
							Dim dBVGBegin As Date
							Dim dDiffDaysbisEndeMonat As Integer

							dDiffDaysbisEndeMonat = Math.Max(0, iAnzAlteDauer - (m_BVGAfter * 7))
							dBVGBegin = DateAdd("d", dDiffDaysbisEndeMonat * (-1), dOldRPBis)
							If Month(dBVGBegin) < Month(dOldRPBis) Then
								' BVGBeginn war bereits letzten Monat
								dBVGBegin = dStartofMonth
								cKTGStd = S(2)

								Exit For
							End If

							cKTGStd = cKTGStd + GetBVGStdFromRPL_1(rpRec.RPNr, dBVGBegin, dOldRPBis)
						End If
					End If
					' End If

				Next

			End If

		End If
		'S(7) = Minimum(cKTGStd, S(2))
		'' es sollte nur bis einer Maximum 180 Stunden pro Monat kommen
		'S(7) = Minimum(S(7), Val(DivFunc.vFieldVal(MDrec![BVG_Std])) / 12)

		Return Math.Min(cKTGStd, Val(m_MandantData.BVG_Std) / 12)

	End Function


End Class
