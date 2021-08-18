Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.DatabaseAccess.PayrollMng.DataObjects

Partial Public Class EmployeePayroll

  Private Function Get_BVG_Std_Basis() As Decimal

    Dim bvgStdBasis = m_PayrollDatabaseAccess.LoadBVGStdBasis(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
    ThrowExceptionOnError(Not bvgStdBasis.HasValue, "BVG Std Basis konnte nicht geladen werden.")
    Get_BVG_Std_Basis = bvgStdBasis
  End Function

  ' TODO: Fardin Klären warum keine Rückgabe
  Private Function GetStdInYearForBVG() As Decimal
    Dim Temprec As Decimal?
    Dim BVGAbzugAfter As Integer

    Dim bvgafter As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgafter", m_PayrollSetting))
    BVGAbzugAfter = bvgafter ' Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGAbzugAfter"))
    Temprec = m_PayrollDatabaseAccess.LoadStdInYearForBVG(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)

    If Not Temprec.HasValue Then
      S(7) = 0
    Else
      ' wenn der BVG-Code auf 9 steht!!!
      S(7) = Temprec.Value

      ' war der Kandidat letzten Monat bereits pflichtig...
      If S(7) - S(2) > BVGAbzugAfter Then
        S(7) = S(2)
      Else
        S(7) = Math.Max((S(7) - BVGAbzugAfter), 0)
      End If

      ' ansonsten...
      If Val(m_EmployeeLOSetting.BVGCode) = 0 Then S(7) = 0 ' nicht pflichtig
      If Val(m_EmployeeLOSetting.BVGCode) = 1 Then S(7) = S(2) ' ab dem 1. Tag pflichtig
    End If

  End Function

	Private Function BVG_Lohn_Std() As Decimal
		Dim result As Decimal = 0
		Dim BVG_MaxStdLohn As Decimal
		Dim BVG_MinStdLohn As Decimal
		Dim cBVGStdLohn As Decimal
		Dim cBVGKoordStd As Decimal

		cBVGStdLohn = S(8)			' BVG-Stundenlohn in der BVG-Datenbank in ES
		cBVGKoordStd = Val(m_MandantData.BVG_Koordination_Jahr) / Val(m_MandantData.BVG_Std)

		If S(8) > cBVGKoordStd And Val(m_MandantData.BVG_Std) > 0 Then
			BVG_MaxStdLohn = Val(m_MandantData.BVG_Max_Jahr) / Val(m_MandantData.BVG_Std)

			' Der mindestlohn, der versichert werden muss,
			' wenn jemand mehr als Koordinationsabzug verdient.
			BVG_MinStdLohn = (Val(m_MandantData.BVG_Koordination_Jahr) + Val(m_MandantData.BVG_Min_Jahr)) / Val(m_MandantData.BVG_Std)

			If S(8) <= BVG_MinStdLohn Then cBVGStdLohn = BVG_MinStdLohn
			result = S(7) * (Math.Min(cBVGStdLohn, BVG_MaxStdLohn) - cBVGKoordStd)
		Else
			result = 0
		End If

		strOriginData &= String.Format(" | <b>BVG_Lohn_Std:</b>cBVGStdLohn: {0} | BVG_Koordination_Jahr: {1} | BVG_Std: {2} | BVG_Max_Jahr: {3} | BVG_Min_Jahr: {4} | BVG_MaxStdLohn: {5} | BVG_MinStdLohn: {6} | S(7): {7} | cBVGKoordStd: {8} | result: {9}",
																	 cBVGStdLohn, Val(m_MandantData.BVG_Koordination_Jahr), Val(m_MandantData.[BVG_Std]), Val(m_MandantData.BVG_Max_Jahr), Val(m_MandantData.BVG_Min_Jahr), BVG_MaxStdLohn, BVG_MinStdLohn, S(7), cBVGKoordStd, result)

		Return result

	End Function

	Private Function BVG_Lohn_Std_New_WithAllBVGLohn() As Decimal
		Dim result As Decimal = 0
		'Dim BVG_MaxStdLohn As Decimal
		Dim BVGLALohnInMonth As Decimal
		Dim cBVGKoordStd As Decimal
		Dim cBVGWorkedStdInMonth As Decimal
		Dim bvgBasisInMonth As Decimal
		Dim donotcontrolbvgMinLohn As Boolean = False
		Dim LohnInStd As Decimal = 0D
		Dim bvgStdinMonth As Decimal = 0D
		Dim IsEmployeeFullBVG As Boolean = True

		Dim dStartofMonth As Date = StartofCurrentPayrollMonth

		' darf erst abgezogen werden wenn BVG-Stundentotal > 0 
		'If S(7) = 0 AndAlso Not BVGBeginForLO.HasValue Then Return 0
		If (S(7) = 0 AndAlso S(2) = 0) OrElse Val(m_EmployeeLOSetting.BVGCode) = 0 Then Return 0

		BVGLALohnInMonth = U(8)     ' Total der BVG-pflichtigen Löhne
		bvgStdinMonth = (Val(m_MandantData.BVG_Std) / 12) ' maximal bvg Stunden im Monat

		' Achtung: gibt unterschiedliche Pläne. Nach Lohn oder nach effektiv geleisteten Stunden!!!
		cBVGWorkedStdInMonth = Math.Min(Val(S(7)), bvgStdinMonth) ' If(S(7) = 0, bvgStdinMonth, Math.Min(Val(S(7)), bvgStdinMonth)) ' BVG-Koordniationasstunde für den Monat
		cBVGKoordStd = Val(m_MandantData.BVG_Koordination_Jahr) / Val(m_MandantData.BVG_Std)  ' BVG-Koordinationsbeitrag pro Stunde

		If BVGBeginForLO.HasValue AndAlso BVGBeginForLO <= dStartofMonth Then
			IsEmployeeFullBVG = True
			cBVGWorkedStdInMonth = S(2)

		Else
			IsEmployeeFullBVG = False

		End If
		LohnInStd = BVGLALohnInMonth / S(2)

		Dim maximalLohnInStd = Val(m_MandantData.BVG_Max_Jahr) / Val(m_MandantData.BVG_Std)
		bvgBasisInMonth = Math.Min((LohnInStd * Math.Abs(cBVGWorkedStdInMonth)), (maximalLohnInStd * Math.Abs(cBVGWorkedStdInMonth))) - (Math.Abs(cBVGWorkedStdInMonth) * cBVGKoordStd)
		If cBVGWorkedStdInMonth < 0 Then bvgBasisInMonth = bvgBasisInMonth * (-1)

		Dim bvgBasisperStd = Math.Min(LohnInStd, maximalLohnInStd) - cBVGKoordStd

		Dim value As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/donotcontrolbvgMinLohn", m_PayrollSetting))
		If Not String.IsNullOrWhiteSpace(value) Then
			donotcontrolbvgMinLohn = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/donotcontrolbvgMinLohn", m_PayrollSetting)), True)
		Else
			donotcontrolbvgMinLohn = True
		End If
		If Not donotcontrolbvgMinLohn AndAlso (BVGLALohnInMonth * 12) < m_MandantData.BVG_Min_Jahr Then
			bvgBasisInMonth = 0
		End If


		Dim msg As String = "<br><b>BVG_Lohn_Std:</b> "
		msg &= String.Format("| donotcontrolbvgMinLohn: {0} ", donotcontrolbvgMinLohn)
		msg &= String.Format("| IsEmployeeFullBVG: {0} ", IsEmployeeFullBVG)
		msg &= String.Format("| S(7): {0} Std. ", S(7))
		msg &= String.Format("| S(2): {0} Std. ", S(2))
		msg &= String.Format("| BVG_Max_Jahr: {0:n2} ", Val(m_MandantData.BVG_Max_Jahr))
		msg &= String.Format("| BVG_Min_Jahr: {0:n2} ", Val(m_MandantData.BVG_Min_Jahr))
		msg &= String.Format("| BVG_Std: {0:n2} ", Val(m_MandantData.BVG_Std))
		msg &= String.Format("| BVG_Koordination_Jahr: {0:n2} ", Val(m_MandantData.BVG_Koordination_Jahr))
		msg &= String.Format("| maximalLohnInStd: {0:n2} ", maximalLohnInStd)
		msg &= String.Format("| cBVGKoordStd: {0:n5} ", cBVGKoordStd)

		msg &= String.Format("| cBVGStdInMonth: {0} ", cBVGWorkedStdInMonth)
		msg &= String.Format("| BVGLALohnInMonth (U8): {0:n2} ", BVGLALohnInMonth)
		msg &= String.Format("| LohnInStd (BVGLALohnInMonth / S(2)): {0:n5} ", LohnInStd)
		msg &= String.Format("| bvgBasisperStd: {0:n5} ", bvgBasisperStd)
		msg &= String.Format("| bvgBasisInMonth: {0:n5} ", bvgBasisInMonth)
		If (BVGLALohnInMonth * 12) < m_MandantData.BVG_Min_Jahr Then msg &= String.Format("<br><b>Achtung:</b> BVG-Lohn {0:n5} < minimal versicherte Lohn von {1:n5} ", (BVGLALohnInMonth * 12), m_MandantData.BVG_Min_Jahr)

		strOriginData &= msg


		result = bvgBasisInMonth

		Return result
	End Function

	Private Function Get_BVGDays_In_Month(ByVal MANr As Integer, ByVal firstdate As Date?, ByVal lastdate As Date?) As Integer
		Dim ESBegin As Date
		Dim ESEnde As Date
		Dim BVGBegin As Date
		Dim BVGDays As Integer
		Dim BVGDaysOld As Integer

		Dim BVGIntervalStr As String
		Dim BVGAfter As Integer

		Dim StartofMonth As Date
		Dim EndofMonth As Date
		Dim EndofYear As Date
		Dim dateUtility As New DateAndTimeUtily

		StartofMonth = StartofCurrentPayrollMonth   ' CDate("01." & LPMonth & "." & LPYear)
		EndofMonth = EndofCurrentPayrollMonth   ' CDate(DateAdd("m", 1, StartofMonth.AddDays(-StartofMonth.Day + 1))).AddDays(-1)
		EndofYear = EndeofCurrentPayrollYear ' CDate("31.12." & LPYear)


		Dim bvginterval As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvginterval", m_PayrollSetting))
		BVGIntervalStr = bvginterval ' DivReg.GetINIString(MDIniFullname, LoadResString(377), _ "BVGIntervalString")

		If BVGIntervalStr = "" Then BVGIntervalStr = "ww"

		' BVG-Pflicht erst nach...
		Dim bvgintervaladd As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgintervaladd", m_PayrollSetting))
		BVGAfter = If(String.IsNullOrWhiteSpace(bvgintervaladd), 13, Val(bvgintervaladd))   ' Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), _ "BVGIntervalAdd"))

		If BVGAfter = 0 Then BVGAfter = 13

		If firstdate.HasValue Then StartofMonth = firstdate
		If lastdate.HasValue Then EndofMonth = lastdate

		Dim ESDataRecList = m_PayrollDatabaseAccess.LoadESDataForBVGDaysInMonthCalculation(m_EmployeeData.EmployeeNumber, MDNr, StartofMonth, EndofMonth)
		ThrowExceptionOnError(ESDataRecList Is Nothing, "Einsatzdaten für BVG Tage im Monat Berechnung konnnten nicht geladen werden.")

		' Kandidat hat Priortät 1
		If m_EmployeeLOSetting.BVGCode = 0 Then                     ' Kein Abzug
			S(16) = 0
		ElseIf m_EmployeeLOSetting.BVGCode = 1 Then             ' Ab 1. Tag
			BVGDays = ESLPTage
		ElseIf m_EmployeeLOSetting.BVGCode = 2 Then             ' 2; Ab 1. Tag, Speziell
			BVGDays = ESLPTage
		ElseIf m_EmployeeLOSetting.BVGCode = 3 Then             ' 3; Ab 1. Tag, Speziell
			BVGDays = ESLPTage

		Else                                                                ' Ab 540 Stunden oder nach Einsatz

			For Each ESDataRec In ESDataRecList
				ESBegin = ESDataRec.ES_Ab
				If Not ESDataRec.ES_Ende.HasValue Then
					ESEnde = EndofMonth

				Else
					ESEnde = dateUtility.MinDate(ESDataRec.ES_Ende, EndofMonth)

				End If

				BVGBegin = DateAdd(BVGIntervalStr, BVGAfter, ESBegin)

				' der BVG-pflicht ist vor dem Monatbeginn
				If BVGBegin <= StartofMonth Then
					If ESEnde < EndofMonth Then
						BVGDays = DateDiff("d", BVGBegin, ESEnde, vbUseSystemDayOfWeek, vbUseSystem) + 1

					ElseIf ESEnde = EndofMonth Then
						BVGDays = ESLPTage

					End If

				ElseIf BVGBegin > StartofMonth And BVGBegin <= ESEnde Then
					BVGDays = DateDiff("d", BVGBegin, ESEnde, vbUseSystemDayOfWeek, vbUseSystem) + 1

				ElseIf BVGBegin > EndofMonth Then
					' Nichts machen...

				End If
				If ESEnde = EndofMonth Or BVGDays >= ESLPTage Then Exit For

				If BVGDaysOld = 0 Then
					BVGDaysOld = BVGDays
				Else
					If BVGDaysOld > BVGDays Then
						BVGDays = BVGDaysOld
					End If
				End If

			Next

		End If

		Return Math.Min(BVGDays, ESLPTage)

	End Function


	Private Function Get_BVGDays_In_Month_New(ByVal bvgDates As IEnumerable(Of BVGDayData)) As Integer
		Dim rpBegin As Date
		Dim rpEnde As Date
		Dim bvgDays As Integer
		Dim bvgBusinessDays As Integer
		Dim dateUtility As New DateAndTimeUtily
		Dim StartofMonth As Date
		Dim EndofMonth As Date

		If bvgDates Is Nothing OrElse bvgDates.Count = 0 Then Return 0

		Dim ESDataRecList = bvgDates
		Dim getbvgasbusinessdays As Boolean = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgasbusinessdays", m_PayrollSetting)), False)
		strOriginData &= String.Format(" ¦ getbvgasbusinessdays: {0} ¦ ", getbvgasbusinessdays)

		StartofMonth = StartofCurrentPayrollMonth
		EndofMonth = EndofCurrentPayrollMonth

		' Kandidat hat Priortät 1
		Select m_EmployeeLOSetting.BVGCode
			Case 0						 ' Kein Abzug
				bvgDays = 0
				'Case 1, 2, 3				 ' Ab 1. Tag
				'	bvgDays = ESLPTage

			Case Else																' automatisch ab 13 Wochen
				Dim oldBegin As Date = ESDataRecList(0).Von
				Dim oldEnde As Date = dateUtility.MinDate(ESDataRecList(0).Bis, EndofMonth)
				Dim i As Integer = 0

				For Each ESDataRec In ESDataRecList

					rpBegin = ESDataRec.Von
					rpEnde = dateUtility.MinDate(ESDataRec.Bis, EndofMonth)

					If i > 0 AndAlso rpBegin <= oldEnde AndAlso rpEnde <= oldEnde Then

					Else
						If i > 0 AndAlso rpBegin < oldEnde Then
							rpBegin = oldEnde
						End If

						If i > 0 AndAlso rpEnde < oldEnde Then rpEnde = oldEnde

						bvgDays += DateDiff(DateInterval.Day, rpBegin, rpEnde, FirstDayOfWeek.System, FirstWeekOfYear.System) + 1
						bvgBusinessDays += Weekdays(rpBegin, rpEnde) '+ 1

						oldBegin = rpBegin
						oldEnde = rpEnde
					End If

					i += 1

				Next
				If getbvgasbusinessdays Then bvgDays = bvgBusinessDays

		End Select
		S(16) = bvgDays

		Return Math.Min(bvgDays, ESLPTage)

	End Function

	Private Function Weekdays(ByVal startDate As Date, ByVal endDate As Date) As Integer
		Dim numWeekdays As Integer = 0
		Dim totalDays As Integer = 0
		Dim WeekendDays As Integer = 0

		totalDays = DateDiff(DateInterval.Day, startDate, endDate, FirstDayOfWeek.System, FirstWeekOfYear.System) + 1

		For i As Integer = 1 To totalDays

			If DatePart(DateInterval.Weekday, startDate) = 1 Then
				WeekendDays = WeekendDays + 1
			End If
			If DatePart(DateInterval.Weekday, startDate) = 7 Then
				WeekendDays = WeekendDays + 1
			End If
			startDate = DateAdd("d", 1, startDate)
		Next

		numWeekdays = totalDays - WeekendDays

		Return numWeekdays

	End Function



	Private Function Get_ES_Std_Total() As Decimal
		Dim ESDatarec As List(Of ESDataForESStdTotalCalculation)
		Dim StartofMonth As Date
		Dim EndofMonth As Date
		Dim EndofYear As Date
		Dim TotalStd As Decimal
		Dim CurrentESTotalStd As Decimal
		Dim BVGAbzugAfter As Integer

		Dim bvgafter As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/bvgafter", m_PayrollSetting))
		BVGAbzugAfter = bvgafter ' Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), "BVGAbzugAfter"))

		StartofMonth = CDate("01." & LPMonth & "." & LPYear)
		EndofMonth = CDate(DateAdd("m", 1, StartofMonth.AddDays(-StartofMonth.Day + 1))).AddDays(-1)
		EndofYear = CDate("31.12." & LPYear)

		' Kandidat hat Priortät 1
		If m_EmployeeLOSetting.BVGCode = 0 Then           ' 0; Kein Abzug
			S(7) = 0

		ElseIf m_EmployeeLOSetting.BVGCode = 1 Then       ' 1; Ab 1. Tag
			S(7) = S(2)
		ElseIf m_EmployeeLOSetting.BVGCode = 2 Then       ' 2; Ab 1. Tag, Speziell
			S(7) = S(2)
		ElseIf m_EmployeeLOSetting.BVGCode = 3 Then       ' 3; Ab 1. Tag, Speziell
			S(7) = S(2)

		Else                                ' 9; Ab 540 Stunden oder nach Einsatz

			ESDatarec = m_PayrollDatabaseAccess.LoadESDataForESStdTotalCalculation(m_EmployeeData.EmployeeNumber, MDNr, StartofMonth, EndofMonth)
			ThrowExceptionOnError(ESDatarec Is Nothing, "Einsatzdaten für ES Std Total Berechnung konnten nicht geladen werden.")

			For Each esData In ESDatarec

				' Einzelne Einsätze anschauen...
				' Standard '1 = ab 1. Tag' annehmen...
				CurrentESTotalStd = esData.TotalESStd
				If Val(m_EmployeeLOSetting.BVGCode) = 9 Then       ' Ab 540 Stunden
					If CurrentESTotalStd >= BVGAbzugAfter Then
						CurrentESTotalStd = CurrentESTotalStd - BVGAbzugAfter
					Else                                                      ' kein Abzug
						CurrentESTotalStd = 0
					End If

				ElseIf Val(m_EmployeeLOSetting.BVGCode) = 0 Then
					CurrentESTotalStd = 0
				End If

				TotalStd += CurrentESTotalStd
				strOriginData &= String.Format(" ¦ Einsatzstunden: {0} >>> {1}¦ ", esData.ESNr, CurrentESTotalStd)

			Next

			Dim sumData = m_PayrollDatabaseAccess.LoadLOLDataForESStdTotalCalculation(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
			ThrowExceptionOnError(sumData Is Nothing, "Lohndaten für ES Total Berechnung konnten nicht geladen werden.")

			If sumData.Sum1.HasValue AndAlso sumData.Sum2.HasValue Then
				TotalStd = sumData.Sum2.Value - TotalStd
			End If

			If TotalStd < 0 Then TotalStd = TotalStd * -1

			S(7) = Math.Min(TotalStd, S(2))
		End If

		Return TotalStd
	End Function


End Class
