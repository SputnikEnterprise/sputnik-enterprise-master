Imports SP.Infrastructure.DateAndTimeCalculation

Partial Public Class EmployeePayroll


	Public ReadOnly Property StartofCurrentPayrollMonth As Date
		Get
			Return CDate("01." & LPMonth & "." & LPYear)
		End Get
	End Property

	Public ReadOnly Property EndofCurrentPayrollMonth As Date
		Get
			Return CDate(DateAdd("m", 1, StartofCurrentPayrollMonth.AddDays(-StartofCurrentPayrollMonth.Day + 1))).AddDays(-1)
		End Get
	End Property

	Public ReadOnly Property StartofCurrentPayrollYear As Date
		Get
			Return CDate("01.01." & LPYear)
		End Get
	End Property

	Public ReadOnly Property EndeofCurrentPayrollYear As Date
		Get
			Return CDate("31.12." & LPYear)
		End Get
	End Property



	Private Function Get_ES_Std_Total_New() As Decimal
		Dim bArbTage As Boolean = m_Calculatebvgwithesdays
		Dim result As Decimal = 0D

		strOriginData &= "<br><br><b>BVG-Data:</b>"
		strOriginData &= String.Format("<br>bArbTage:{0}<br>", bArbTage)
		If bArbTage Then
			result = Get_ES_Std_Total_New_1()
		Else
			result = Get_ES_Std_Total_New_0()
		End If
		strOriginData &= String.Format("<br><b>BVG-pflicht:</b> {0:d} - {1:d}<br>", BVGBeginForLO, BVGEndForLO)

		Return result
	End Function

	Private Function Get_ES_Std_Total_New_0() As Decimal
		Dim iAnzAlteDauer As Integer
		Dim dateUtility As New DateAndTimeUtily

		Dim StartofMonth As Date = StartofCurrentPayrollMonth
		Dim EndofMonth As Date = EndofCurrentPayrollMonth
		Dim dOldRPVon As Date
		Dim dOldRPBis As Date
		Dim bvgHoursNumber As Decimal = 0D
		Dim bvgDates As New List(Of BVGDayData)


		Dim bvgDayMessage As String = String.Empty
		Dim msg As String = String.Empty

		' Kandidat hat Priortät 1
		Select Case Val(m_EmployeeLOSetting.BVGCode)
			Case 0                      ' 0; Kein Abzug
				bvgHoursNumber = 0

			Case 1, 2, 3                 ' 1; Ab 1. Tag
				bvgHoursNumber = S(2)
				bvgDates = GetBVGDaysDatainCurrentMonth()

			Case Else                                                               ' 9; Ab 13 Wochen

				' Einsatz ist unbefristet: dann ist es ab dem ersten Tag BVG-pflichtig (spezialfall beachten!!!)
				' Einsatz geht länger als 3 Monate: ist ab 3. Monat BVG-pflichtig
				' Einsatz ist kürtzer als 3 Monate: ist NICHT BVG-pflichtig
				' Die Einsätze sind unterbrochen und der Unterbruch dauert länger als x Wochen:
				' BVG-pflicht fängt von 0 an.
				' Die Einsätze sind unterbrochen und der Unterbruch dauert kürzer als x Wochen:
				' BVG-pflicht geht weiter.
				' Der Mann arbeitet neu: Der BVG-pflicht fängt von 0 an
				bvgHoursNumber = 0

				Dim rpData = m_PayrollDatabaseAccess.LoadRPDataForESStdTotalNew0Calculation(m_EmployeeData.EmployeeNumber, MDNr, DateAdd("m", -CInt(Math.Max(8, m_iESBreakWeek * 7 / 30)), StartofMonth), DateAdd("m", 1, StartofMonth))
				ThrowExceptionOnError(rpData Is Nothing, "RP Daten für ES_Std_Total_New_0 konnten nicht geladen werden.")
				strOriginData &= String.Format("Einsatzdaten werden geschaut zwischen: {0:d} - {1:d}<br>", DateAdd("m", -CInt(Math.Max(8, m_iESBreakWeek * 7 / 30)), StartofMonth), DateAdd("m", 1, StartofMonth))


				If rpData.Count = 0 Then
					S(7) = Math.Min(bvgHoursNumber, S(2))

					Return bvgHoursNumber
				End If

				Dim rRPrec = rpData(0)

				' zum Ersten mal werden die Von und Bis Daten gespeichert...
				dOldRPVon = rRPrec.Von
				dOldRPBis = rRPrec.Bis
				iAnzAlteDauer = rRPrec.ESRPTage
				msg &= String.Format(" ¦ RP-Daten: {0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

				If rRPrec.Monat = LPMonth And Convert.ToInt32(rRPrec.Jahr) = LPYear Then
					' Es ist der erste Monat wo er arbeitet; dann ist es nichts!!!
					bvgHoursNumber = 0

				Else

					For rpIndex As Integer = 1 To rpData.Count - 1

						rRPrec = rpData(rpIndex)
						msg &= String.Format("{0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

						' If Not rRPrec.EOF Then
						If dOldRPVon < rRPrec.Von Then
							If dOldRPBis >= rRPrec.Von Then
								If dOldRPBis <= rRPrec.Bis Then dOldRPBis = rRPrec.Bis

							ElseIf dOldRPBis < rRPrec.Von Then
								' Das Ende der letzte Rapport ist vor dem neuen Rapportbeginn: kann pflichtig werden
								If DateDiff(m_BVGInterval, dOldRPBis, rRPrec.Von, vbUseSystemDayOfWeek, vbUseSystem) > m_iESBreakWeek Then
									' dann fängt alles von 0 an...
									dOldRPVon = rRPrec.Von
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer = Val(rRPrec.ESRPTage)

								Else
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer += Val(rRPrec.ESRPTage)

								End If
							End If

						End If

						If DateDiff(m_BVGInterval, dOldRPVon, dOldRPBis, vbUseSystemDayOfWeek, vbUseSystem) > m_BVGAfter Then

							' es ist dann BVG-pflichtig!!!
							If rRPrec.Monat = LPMonth And rRPrec.Jahr = LPYear Then

								' Testphase...
								Dim dBVGBegin As Date
								' 2. Test
								dBVGBegin = DateAdd(DateInterval.Day, (m_BVGAfter * 7), dOldRPVon)

								If BVGBeginForLO Is Nothing Then BVGBeginForLO = dBVGBegin
								If BVGEndForLO Is Nothing Then BVGEndForLO = rpData.Max(Function(data) data.Bis)

								Dim numberOfDays As Integer = DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1

								bvgDates.Add(New BVGDayData With {.RPNr = rRPrec.RPNr, .Von = rRPrec.Von, .Bis = rRPrec.Bis, .DayCount = numberOfDays})

								If String.IsNullOrWhiteSpace(bvgDayMessage) Then bvgDayMessage = "(BVG-Tage) "
								bvgDayMessage &= String.Format("{0:d} - {1:d}: {2} | ", rRPrec.Von, rRPrec.Bis, numberOfDays)

								If dBVGBegin < dOldRPBis AndAlso Month(dBVGBegin) < Month(dOldRPBis) Then
									' BVGBeginn war bereits letzten Monat
									Dim BVGrpData = rpData.Where(Function(data) data.Monat = LPMonth)
									dBVGBegin = BVGrpData.Min(Function(data) data.Von)
									BVGBeginForLO = dBVGBegin

									bvgHoursNumber = S(2)
									bvgDates = GetBVGDaysDatainCurrentMonth()

									Exit For

								End If

								' End der Testphase...
								bvgHoursNumber += GetBVGStdFromRPL_0(rRPrec.RPNr, dOldRPVon, dOldRPBis)
							End If

						End If

					Next
				End If

		End Select

		If BVGBeginForLO.HasValue AndAlso BVGEndForLO.HasValue Then
			bvgHoursNumber += m_PayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPLShorttime(MDNr, m_EmployeeData.EmployeeNumber, LPMonth, LPYear)
			strOriginData &= String.Format("<br><b>Suche nach Lohnart 103.01 wegen Kurzarbeit: {0:n2}</b><br>", bvgHoursNumber)
		End If
		If BVGBeginForLO.HasValue AndAlso BVGEndForLO Is Nothing Then
			Trace.WriteLine(String.Format("{0} >>> {1}", m_EmployeeData.EmployeeNumber, BVGBeginForLO))
		End If
		If bvgHoursNumber > 0 Then
			Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(m_EmployeeData.EmployeeNumber, MDNr, StartofMonth, EndofMonth)
			ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")

			Dim firstESAb As DateTime? = ESDataRecList.Min(Function(data) data.ES_Ab)
			Dim lastESEnde As DateTime? = Nothing
			strOriginData &= bvgDayMessage

			If ESDataRecList.Any(Function(data) Not data.ES_Ende.HasValue) Then
				lastESEnde = EndofMonth
			Else
				lastESEnde = ESDataRecList.Max(Function(data) data.ES_Ende)
			End If
			If firstESAb Is Nothing Then
				strOriginData &= String.Format(" | firstESAb is nothing! setting to {0}", StartofMonth)
				firstESAb = StartofMonth
			End If
			If lastESEnde Is Nothing Then
				strOriginData &= String.Format(" | lastESEnde is nothing! setting to {0}", EndofMonth)
				lastESEnde = EndofMonth
			End If

			If Not BVGBeginForLO.HasValue Then BVGBeginForLO = firstESAb
			If Not BVGEndForLO.HasValue Then BVGEndForLO = lastESEnde

			BVGBeginForLO = dateUtility.MaxDate(firstESAb, dateUtility.MaxDate(BVGBeginForLO, StartofMonth))
			BVGEndForLO = dateUtility.MinDate(lastESEnde, dateUtility.MinDate(BVGEndForLO, EndofMonth))

			If bvgDates Is Nothing OrElse bvgDates.Count = 0 Then
				bvgDates.Add(New BVGDayData With {.RPNr = 0, .Von = BVGBeginForLO, .Bis = BVGEndForLO,
										 .DayCount = DateDiff(DateInterval.Day, CDate(BVGBeginForLO), CDate(BVGEndForLO), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})
			End If
		End If

		If Not String.IsNullOrWhiteSpace(msg) Then strOriginData &= msg & String.Format("Anzahl Tage: {0} Tage<br>", iAnzAlteDauer)
		If Not bvgDates Is Nothing AndAlso bvgDates.Count > 0 Then
			Dim bvgDayCount As Integer = Get_BVGDays_In_Month_New(bvgDates)

			If Not String.IsNullOrWhiteSpace(msg) Then strOriginData &= String.Format("Anzahl BVG-pflichtige Tage: {0}", bvgDayCount)
			m_BVGDateData = String.Empty
			For Each itm In bvgDates
				m_BVGDateData &= String.Format("{0:d} - {1:d}{2}", itm.Von, itm.Bis, vbNewLine)
			Next

		End If

		S(7) = Math.Min(bvgHoursNumber, S(2))
		' es sollte nur bis einer Maximum 180 Stunden pro Monat kommen
		S(7) = Math.Min(S(7), Val(m_MandantData.BVG_Std) / 12)

		Return Math.Min(bvgHoursNumber, Val(m_MandantData.BVG_Std) / 12)

	End Function

	' Neue Funktionen für BVG-Abrechnung.
	' Achtung nicht 3 Monate mit Ketteneinsätzen sonder nach Einsatzdauer...
	Private Function Get_ES_Std_Total_New_1() As Decimal
		Dim iAnzAlteDauer As Integer
		Dim dateUtility As New DateAndTimeUtily

		Dim StartofMonth As Date = StartofCurrentPayrollMonth
		Dim EndofMonth As Date = EndofCurrentPayrollMonth
		Dim dOldRPVon As Date
		Dim dOldRPBis As Date

		Dim bvgHoursNumber As Decimal = 0D
		Dim bvgDates As New List(Of BVGDayData)
		Dim bvgDayMessage As String = String.Empty
		Dim msg As String = String.Empty

		' Kandidat hat Priortät 1
		Select Case Val(m_EmployeeLOSetting.BVGCode)
			Case 0                      ' 0; Kein Abzug
				bvgHoursNumber = 0

			Case 1, 2, 3                 ' 1; Ab 1. Tag
				bvgHoursNumber = S(2)
				bvgDates = GetBVGDaysDatainCurrentMonth()


			Case Else                               ' 9; Ab 13 Wochen

				Dim rpData = m_PayrollDatabaseAccess.LoadRPDataForESStdTotalNew1Calculation(m_EmployeeData.EmployeeNumber, MDNr, DateAdd("m", -CInt(Math.Max(8, (m_iESBreakWeek * 7 / 30))), StartofMonth), DateAdd("m", 1, StartofMonth))
				ThrowExceptionOnError(rpData Is Nothing, "RP Daten für ES_Std_Total_New_1 konnten nicht geladen werden.")
				strOriginData &= String.Format("Einsatzdaten werden geschaut zwischen: {0:d} - {1:d}<br>", DateAdd("m", -CInt(Math.Max(8, m_iESBreakWeek * 7 / 30)), StartofMonth), DateAdd("m", 1, StartofMonth))

				If rpData.Count = 0 Then
					S(7) = Math.Min(bvgHoursNumber, S(2))
					Return bvgHoursNumber
				End If

				Dim rRPrec = rpData(0)

				' zum Ersten mal werden die Von und Bis Daten gespeichert...
				dOldRPVon = rRPrec.Von
				dOldRPBis = rRPrec.Bis
				iAnzAlteDauer = rRPrec.ESRPTage
				msg &= String.Format(" ¦ RP-Daten: {0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

				If rRPrec.Monat = LPMonth And Convert.ToInt32(rRPrec.Jahr) = LPYear Then
					' Es ist der erste Monat wo er arbeitet; dann ist es nichts!!!
					bvgHoursNumber = 0

				Else

					For rpIndex As Integer = 1 To rpData.Count - 1

						rRPrec = rpData(rpIndex)
						msg &= String.Format("{0}: {1:d} - {2:d} = {3} ¦ ", rRPrec.RPNr, rRPrec.Von, rRPrec.Bis, rRPrec.ESRPTage)

						If dOldRPVon < rRPrec.Von Then
							If dOldRPBis >= rRPrec.Von Then
								If dOldRPBis <= rRPrec.Bis Then dOldRPBis = rRPrec.Bis

							ElseIf dOldRPBis < rRPrec.Von Then
								' Das Ende der letzte Rapport ist vor dem neuen Rapportbeginn: kann pflichtig werden
								If DateDiff(m_BVGInterval, dOldRPBis, rRPrec.Von, vbUseSystemDayOfWeek, vbUseSystem) + 1 > m_iESBreakWeek Then
									' dann fängt alles von 0 an...
									dOldRPVon = rRPrec.Von
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer = Val(rRPrec.ESRPTage)

								Else
									dOldRPVon = rRPrec.Von
									dOldRPBis = rRPrec.Bis
									iAnzAlteDauer = iAnzAlteDauer + Val(rRPrec.ESRPTage)

								End If

							End If

						End If

						If iAnzAlteDauer >= (m_BVGAfter * 7) Then
							' es ist dann BVG-pflichtig!!!
							If rRPrec.Monat = LPMonth And rRPrec.Jahr = LPYear Then
								Dim dBVGBegin As Date
								Dim dDiffDaysbisEndeMonat As Integer

								dDiffDaysbisEndeMonat = Math.Max(0, iAnzAlteDauer - (m_BVGAfter * 7))
								dBVGBegin = DateAdd("d", dDiffDaysbisEndeMonat * (-1), dOldRPBis)

								If BVGBeginForLO Is Nothing Then BVGBeginForLO = dBVGBegin
								If BVGEndForLO Is Nothing Then BVGEndForLO = rpData(rpData.Count - 1).Bis

								If dBVGBegin < dOldRPBis AndAlso Month(dBVGBegin) < Month(dOldRPBis) Then
									' BVGBeginn war bereits letzten Monat
									Dim BVGrpData = rpData.Where(Function(data) data.Monat = LPMonth)
									dBVGBegin = BVGrpData.Min(Function(data) data.Von)
									BVGBeginForLO = dBVGBegin

									Dim numberOfDays As Integer = DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1
									bvgDates.Add(New BVGDayData With {.RPNr = rRPrec.RPNr, .Von = rRPrec.Von, .Bis = rRPrec.Bis, .DayCount = numberOfDays})
									'DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})

									If String.IsNullOrWhiteSpace(bvgDayMessage) Then bvgDayMessage = "(BVG-Tage) "
									bvgDayMessage &= String.Format("{0:d} - {1:d}: {2} | ", rRPrec.Von, rRPrec.Bis, numberOfDays)
									'DateDiff(DateInterval.Day, CDate(rRPrec.Von), CDate(rRPrec.Bis), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1)

									If rpIndex = rpData.Count - 1 Then
										bvgHoursNumber = S(2)
										bvgDates = GetBVGDaysDatainCurrentMonth()

										Exit For
									End If

								End If

								bvgHoursNumber += GetBVGStdFromRPL_1(rRPrec.RPNr, dBVGBegin, dOldRPBis)

							End If
						End If

					Next

					If BVGBeginForLO.HasValue Then BVGEndForLO = dOldRPBis
				End If

		End Select


		If BVGBeginForLO.HasValue AndAlso BVGEndForLO.HasValue Then
			bvgHoursNumber += m_PayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPLShorttime(MDNr, m_EmployeeData.EmployeeNumber, LPMonth, LPYear)
			strOriginData &= String.Format("<br><b>Suche nach Lohnart 103.01 wegen Kurzarbeit: {0:n2}</b><br>", bvgHoursNumber)

		End If
		If BVGBeginForLO.HasValue AndAlso BVGEndForLO Is Nothing Then
			Trace.WriteLine(String.Format("{0} >>> {1}", m_EmployeeData.EmployeeNumber, BVGBeginForLO))
		End If
		If bvgHoursNumber > 0 Then 'AndAlso Not BVGBeginForLO Is Nothing Then
			Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(m_EmployeeData.EmployeeNumber, MDNr, StartofMonth, EndofMonth)
			ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")

			Dim firstESAb As DateTime? = ESDataRecList.Min(Function(data) data.ES_Ab)
			Dim lastESEnde As DateTime? = Nothing

			If ESDataRecList.Any(Function(data) Not data.ES_Ende.HasValue) Then
				lastESEnde = EndofMonth
			Else
				lastESEnde = ESDataRecList.Max(Function(data) data.ES_Ende)
			End If
			If firstESAb Is Nothing Then
				strOriginData &= String.Format(" | firstESAb is nothing! setting to {0}", StartofMonth)
				firstESAb = StartofMonth
			End If
			If lastESEnde Is Nothing Then
				strOriginData &= String.Format(" | lastESEnde is nothing! setting to {0}", EndofMonth)
				lastESEnde = EndofMonth
			End If

			If Not BVGBeginForLO.HasValue Then BVGBeginForLO = firstESAb
			If Not BVGEndForLO.HasValue Then BVGEndForLO = lastESEnde

			BVGBeginForLO = dateUtility.MaxDate(firstESAb, dateUtility.MaxDate(BVGBeginForLO, StartofMonth))
			BVGEndForLO = dateUtility.MinDate(lastESEnde, dateUtility.MinDate(BVGEndForLO, EndofMonth))

			If bvgDates Is Nothing OrElse bvgDates.Count = 0 Then
				bvgDates.Add(New BVGDayData With {.RPNr = 0, .Von = BVGBeginForLO, .Bis = BVGEndForLO,
										 .DayCount = DateDiff(DateInterval.Day, CDate(BVGBeginForLO), CDate(BVGEndForLO), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})
			End If
		End If

		If Not String.IsNullOrWhiteSpace(msg) Then strOriginData &= msg & String.Format(" Anzahl Tage: {0} Tage<br>", iAnzAlteDauer)
		If Not bvgDates Is Nothing AndAlso bvgDates.Count > 0 Then
			Dim bvgDayCount As Integer = Get_BVGDays_In_Month_New(bvgDates)

			If Not String.IsNullOrWhiteSpace(msg) Then strOriginData &= bvgDayMessage & String.Format(" Anzahl BVG-pflichtige Tage: {0} Tage", bvgDayCount)
			m_BVGDateData = String.Empty
			For Each itm In bvgDates
				m_BVGDateData &= String.Format("{0:d} - {1:d}{2}", itm.Von, itm.Bis, vbNewLine)
			Next

		End If

		S(7) = Math.Min(bvgHoursNumber, S(2))
		' es sollte nur bis einer Maximum 180 Stunden pro Monat kommen
		S(7) = Math.Min(S(7), Val(m_MandantData.BVG_Std) / 12)

		Return Math.Min(bvgHoursNumber, Val(m_MandantData.BVG_Std) / 12)
	End Function

	Private Function GetBVGDaysDatainCurrentMonth() As List(Of BVGDayData)
		Dim bvgDates As New List(Of BVGDayData)
		Dim dateUtility As New DateAndTimeUtily

		Dim StartofMonth As Date = StartofCurrentPayrollMonth
		Dim EndofMonth As Date = EndofCurrentPayrollMonth

		Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(m_EmployeeData.EmployeeNumber, MDNr, StartofMonth, EndofMonth)
		ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")
		If ESDataRecList.Count = 0 Then Return bvgDates

		Dim oldBegin As Date = dateUtility.MaxDate(ESDataRecList(0).ES_Ab, StartofMonth)
		Dim oldEnde As Date = dateUtility.MinDate(ESDataRecList(0).ES_Ende.GetValueOrDefault(EndofMonth), EndofMonth)

		Dim esBeginn As Date
		Dim esEnde As Date
		Dim i As Integer = 0

		For Each esdata In ESDataRecList

			esBeginn = dateUtility.MaxDate(esdata.ES_Ab, StartofMonth)
			esEnde = dateUtility.MinDate(esdata.ES_Ende.GetValueOrDefault(EndofMonth), EndofMonth)

			If i > 0 AndAlso esBeginn < oldEnde Then
				esBeginn = oldEnde
			End If

			If i > 0 AndAlso esEnde < oldEnde Then esEnde = oldEnde
			bvgDates.Add(New BVGDayData With {.RPNr = 0, .Von = esBeginn, .Bis = esEnde,
																				.DayCount = DateDiff(DateInterval.Day, CDate(esBeginn), CDate(esEnde), FirstDayOfWeek.System, FirstWeekOfYear.System) + 1})

			oldBegin = esBeginn
			oldEnde = esEnde

			i += 1
		Next

		Return bvgDates

	End Function

	Private Function GetBVGStdFromRPL_0(ByVal lRPNr As Integer, ByVal dRPStart As Date, ByVal dRPEnde As Date) As Decimal
		Dim dBVGStart As Date
		Dim i As Integer
		Dim strFieldBez As String = String.Empty
		Dim cTotalStd As Decimal = 0
		'Dim strBVGInterval As String
		'Dim iBVGAfter As Integer

		'Dim dStartofYear As Date
		Dim dStartofMonth As Date = StartofCurrentPayrollMonth
		Dim dEndofMonth As Date = EndofCurrentPayrollMonth
		'Dim dEndofYear As Date

		'dStartofMonth = CDate("01." & LPMonth & "." & LPYear)
		'dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)
		'dStartofYear = CDate("01.01." & LPYear)
		'dEndofYear = CDate("31.12." & LPYear)

		'Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		'Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))

		''strBVGInterval =
		'bvginterval = "ww" ' DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGIntervalString")
		'iBVGAfter = Val(bvgintervaladd)	'Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGIntervalAdd"))

		''If strBVGInterval = "" Then strBVGInterval = "ww" ' Wochenweise
		'If iBVGAfter = 0 Then iBVGAfter = 13 ' nach 13 Wochen
		'iBVGAfter = iBVGAfter - 1

		' Startdatum für BVG-Beginn...
		dBVGStart = DateAdd(m_BVGInterval, m_BVGAfter, dRPStart)
		dFoundedBVGVon = dBVGStart
		dFoundedBVGBis = dRPEnde
		If dBVGStart > dEndofMonth Then Return cTotalStd

		If Month(dBVGStart) = Month(dRPEnde) And Year(dBVGStart) = Year(dRPEnde) Then
			For i = (dBVGStart.Day) To (dRPEnde.Day)
				strFieldBez = strFieldBez & IIf(strFieldBez = "", "", " + ") & "ISNull(RPL_MA_Day.Tag" & i & ", 0.00)"
			Next i

		Else
			For i = 1 To (dRPEnde.Day)
				strFieldBez = strFieldBez & IIf(strFieldBez = "", "", " + ") & "ISNull(RPL_MA_Day.Tag" & i & ", 0.00)"
			Next i

		End If

		Dim sumStdTotal = m_PayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPL0Calculation(strFieldBez, Convert.ToInt32(lRPNr))

		If sumStdTotal.HasValue Then cTotalStd = sumStdTotal

		Return cTotalStd

	End Function

	Private Function GetBVGStdFromRPL_1(ByVal lRPNr As Integer, ByVal dBVGBegin As Date, ByVal dRPEnde As Date) As Decimal
		Dim i As Integer
		Dim strFieldBez As String = String.Empty
		Dim cTotalStd As Decimal = 0

		Dim dStartofMonth As Date = StartofCurrentPayrollMonth
		Dim dEndofMonth As Date = EndofCurrentPayrollMonth

		If dBVGBegin > dEndofMonth Then Return cTotalStd
		' BVGBeginn war bereits letzten Monat
		If Month(dBVGBegin) <> Month(dRPEnde) Then dBVGBegin = dStartofMonth

		For i = dBVGBegin.Day To dRPEnde.Day
			strFieldBez = strFieldBez & IIf(strFieldBez = "", "", " + ") & "ISNull(RPL_MA_Day.Tag" & i & ", 0.00)"
		Next
		If Not String.IsNullOrWhiteSpace(strFieldBez) Then
			Dim sumStdTotal = m_PayrollDatabaseAccess.LoadStdTotalForBVGStdFromRPL1Calculation(strFieldBez, Convert.ToInt32(lRPNr))

			If sumStdTotal.HasValue Then cTotalStd = sumStdTotal
		End If

		Return cTotalStd
	End Function

End Class
