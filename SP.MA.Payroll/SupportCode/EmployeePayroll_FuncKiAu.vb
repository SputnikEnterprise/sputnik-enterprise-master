Partial Public Class EmployeePayroll

	' Funktionen ab 01. 01. 2009
	Private Function Get_KiData_2(ByRef TempLANr As Decimal, ByRef TempAnzahl As Decimal, ByRef TempBasis As Decimal, ByRef TempAnsatz As Decimal, ByRef TempBetrag As Decimal, ByVal strKanton As String, ByVal CalculateWithGivenAmount As Boolean) As Boolean
		Dim result As Boolean = True
		Dim AutoLO As String
		Dim cMaxFakStd As Decimal
		Dim sMsgText As String
		Dim iOrgAnzahl As Integer
		Dim cKI_StdBetrag As Decimal
		Dim cKI_TagBetrag As Decimal
		Dim cMonthLimit As Decimal

		Dim totalAHVLohnInMonthToWatch As Decimal
		Dim cTotalAHVInYear As Decimal = 0
		Dim cKIMonthBetrag As Decimal
		Dim ExistsESBeginEnde As Boolean = False
		Dim IsAHVLohnLowerAsAllowed As Boolean = False
		Dim msg As String


		msg = String.Format("LANr {0}, Kanton: {1} >>> TempAnzahl: {2} * TempBasis: {3} * TempAnsatz: {4} = TempBetrag: {5} | U(2): {6} | CalculateWithGivenAmount: {7}", TempLANr, strKanton, TempAnzahl, TempBasis, TempAnsatz, TempBetrag, U(2), CalculateWithGivenAmount)

		Try
			' U(2) ist das AHV-Basis (aktuellem Monat!!!)
			totalAHVLohnInMonthToWatch = U(2)
			If LPMonth > 1 Then
				cTotalAHVInYear = m_PayrollDatabaseAccess.LoadAHVLohnInYear(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
			End If

			' this should be becacuse this month 7100 la is not yet in db!
			cTotalAHVInYear += totalAHVLohnInMonthToWatch

			Dim err = False
			Dim rTemprec = m_PayrollDatabaseAccess.LoadMD_KiAuDataData(MDNr, strKanton, LPYear, err)
			ThrowExceptionOnError(err, "LoadMD_KiAu Daten konnten nicht geladen werden.")
			If rTemprec Is Nothing Then
				sMsgText = String.Format("<b>Achtung:</b> Mandant: {0}, Jahr: {1}, Kanton: {2} >>> existieren keine Angaben für Kinder- und Ausbildungszulagen. Es wird keine Korrektur durchgenommen!", MDNr, LPYear, strKanton)
				WriteToProtocol(Padright("M -> (Get_KiData2): ", 30, " ") & sMsgText)

				Return False
			End If

			ExistsESBeginEnde = IsESBeginOrEndInMonth()
			If rTemprec.AtEndBeginES Then
				If Not ExistsESBeginEnde Then
					msg &= String.Format("{0}ExistsESBeginEnde: {1} >>> is canceling!", vbNewLine, ExistsESBeginEnde)
					Return False
				End If
			End If

			cMonthLimit = Val(rTemprec.YMinLohn) / 12
			If TempLANr = 3602 Then
				cKIMonthBetrag = Val(rTemprec.Ki2_Month)
			Else
				cKIMonthBetrag = Val(rTemprec.Ki1_Month)
			End If
			iOrgAnzahl = TempAnzahl

			cMaxFakStd = rTemprec.Ki1_FakMax
			cKI_StdBetrag = rTemprec.Ki1_Std
			cKI_TagBetrag = rTemprec.Ki1_Day

			AutoLO = rTemprec.ChangeKiIn
			If String.IsNullOrWhiteSpace(AutoLO) Then AutoLO = "A"

			If cKI_TagBetrag = 0 AndAlso AutoLO = "T" Then cKI_TagBetrag = If(CalculateWithGivenAmount, TempBasis, cKIMonthBetrag) / 30

			msg &= String.Format("{0}cMaxFakStd: {1} | cKI_StdBetrag: {2} | cKI_TagBetrag: {3} | AutoLO: {4} | ExistsESBeginEnde: {5}", vbNewLine, cMaxFakStd, cKI_StdBetrag, cKI_TagBetrag, AutoLO, ExistsESBeginEnde)
			msg &= String.Format("{0}SeeAHVLohnForYear: {1} | cMonthLimit: {2} | cTotalAHVInYear: {3} | totalAHVLohnInMonthToWatch: {4} | cKIMonthBetrag: {5}", vbNewLine, rTemprec.SeeAHVLohnForYear, cMonthLimit, cTotalAHVInYear, totalAHVLohnInMonthToWatch, cKIMonthBetrag)

			If rTemprec.SeeAHVLohnForYear.GetValueOrDefault(False) AndAlso totalAHVLohnInMonthToWatch < cMonthLimit Then
				totalAHVLohnInMonthToWatch = cTotalAHVInYear
				msg &= String.Format("{0}SeeAHVLohnForYear AndAlso totalAHVLohnInMonthToWatch < cMonthLimit: true | totalAHVLohnInMonthToWatch: {1} | cMonthLimit: {2}", vbNewLine, totalAHVLohnInMonthToWatch, cMonthLimit)

				If cTotalAHVInYear <> U(2) Then
					IsAHVLohnLowerAsAllowed = totalAHVLohnInMonthToWatch < rTemprec.YMinLohn.GetValueOrDefault(0)
					msg &= String.Format("{0}cTotalAHVInYear <> U(2): true | totalAHVLohnInMonthToWatch: {1} | YMinLohn : {2}", vbNewLine, totalAHVLohnInMonthToWatch, rTemprec.YMinLohn.GetValueOrDefault(0))

				Else
					IsAHVLohnLowerAsAllowed = totalAHVLohnInMonthToWatch < cMonthLimit
					msg &= String.Format("{0}cTotalAHVInYear <> U(2): false | totalAHVLohnInMonthToWatch: {1} | cMonthLimit : {2}", vbNewLine, totalAHVLohnInMonthToWatch, cMonthLimit)

				End If

			Else
				IsAHVLohnLowerAsAllowed = totalAHVLohnInMonthToWatch < cMonthLimit
				msg &= String.Format("{0}SeeAHVLohnForYear AndAlso totalAHVLohnInMonthToWatch < cMonthLimit: false | totalAHVLohnInMonthToWatch: {1} | cMonthLimit: {2}", vbNewLine, totalAHVLohnInMonthToWatch, cMonthLimit)

			End If
			msg &= String.Format("{0}IsAHVLohnLowerAsAllowed: {1}", vbNewLine, IsAHVLohnLowerAsAllowed)


			If IsAHVLohnLowerAsAllowed Then
				' Total verdienter Lohn < Monatsmindest
				If ExistMLohn() Then
					msg &= String.Format("{0}ExistMLohn: false", vbNewLine)
					Return False
				End If

				sMsgText = String.Format("<b>Achtung:</b> {0} {1} {2} hat mit {3:n2} weniger als vordefinierter Mindestlohn {4:n2} verdient. Die Zulage wird gelöscht!",
																			 IIf(m_EmployeeData.Gender = "M", "Herr", "Frau"),
																			 m_EmployeeData.Lastname, m_EmployeeData.Firstname,
																			 totalAHVLohnInMonthToWatch,
																			 cMonthLimit)

				WriteToProtocol(Padright("M -> (Get_KiData2): ", 30, " ") & sMsgText)

				TempAnzahl = TempAnzahl
				TempBasis = 0
				TempAnsatz = 100
				TempBetrag = 0
				msg = String.Format("{0}changing values >>> TempAnzahl: {1} * TempBasis: {2} * TempAnsatz: {3} = TempBetrag: {4} >>> returning true", vbNewLine, TempAnzahl, TempBasis, TempAnsatz, TempBetrag)

				Return True

			ElseIf ExistsESBeginEnde AndAlso AutoLO = "A" Then
				'ElseIf ESLPTage < 30 AndAlso AutoLO = "A" Then
				msg &= String.Format("{0}ExistsESBeginEnde AndAlso AutoLO = A: true >>> canceling", vbNewLine)

				Return False

			ElseIf Not ExistsESBeginEnde Then
				'ElseIf ESLPTage >= 30 Then
				msg &= String.Format("{0}ExistsESBeginEnde: false >>> canceling", vbNewLine)
				Return False

			End If

			sMsgText = String.Format("<b>Achtung:</b> {0} {1} {2}, ist während des Monats ein- oder ausgetreten. Die Kinderzulage wird entsprechend angepasst.",
																		 IIf(m_EmployeeData.Gender = "M", "Der Kandidat, Herr", "Die Kandidatin, Frau"),
																		 m_EmployeeData.Lastname, m_EmployeeData.Firstname)

			If AutoLO = "S" Then        ' Kinderzulagen in Stunden
				TempAnzahl = S(2) * TempAnzahl
				TempBasis = cKI_StdBetrag
				TempAnsatz = 100
				TempBetrag = Format(Math.Min(cKIMonthBetrag * iOrgAnzahl, TempAnzahl * cKI_StdBetrag), "0.00")
				TempLANr = 3700           ' Kinderzulage in Stunden

			ElseIf AutoLO = "T" Then    ' Kinderzulagen in Tag
				TempAnzahl = ESLPTage * TempAnzahl
				TempBasis = cKI_TagBetrag
				TempAnsatz = 100
				TempBetrag = Format(Math.Min(cKIMonthBetrag * iOrgAnzahl, TempAnzahl * cKI_TagBetrag), "0.00") 'KIZulage
				TempLANr = 3650           ' Kinderzulage in Tag

			End If

			sMsgText &= String.Format("Automatische Änderung: {0}. Neue Lohnart lautet: {1:F4}.", AutoLO, TempLANr)
			msg &= String.Format("{0}Changing: AutoLO: {1} | TempLANr: {2} | TempAnzahl: {3} * TempBasis: {4} * TempAnsatz: {5} = TempBetrag: {6}", vbNewLine, AutoLO, TempLANr, TempAnzahl, TempBasis, TempAnsatz, TempBetrag)

			WriteToProtocol(Padright("M -> (Get_KiData2): ", 30, " ") & sMsgText)
			Return result


		Catch ex As Exception
			sMsgText = ex.ToString
			WriteToProtocol(Padright("*** -> (Get_KiData2): ", 40, " ") & sMsgText)

			Throw ex

		Finally
			strOriginData &= String.Format(" | {1}{0} ", vbNewLine, msg)

		End Try


		Return result

	End Function

	' Funktionen ab 01. 01. 2009
	Private Function Get_AuData_2(ByRef TempLANr As Decimal, ByRef TempAnzahl As Decimal, ByRef TempBasis As Decimal, ByRef TempAnsatz As Decimal, ByRef TempBetrag As Decimal, ByVal strKanton As String, ByVal CalculateWithGivenAmount As Boolean) As Boolean

		Dim result As Boolean = True
		Dim AutoLO As String
		Dim sMsgText As String
		Dim iOrgAnzahl As Integer
		Dim cAu_StdBetrag As Decimal
		Dim cAu_TagBetrag As Decimal
		Dim cMonthLimit As Decimal
		Dim totalAHVLohnInMonthToWatch As Decimal
		Dim cTotalAHVInYear As Decimal = 0
		Dim cAuMonthBetrag As Decimal
		Dim ExistsESBeginEnde As Boolean = False
		Dim IsAHVLohnLowerAsAllowed As Boolean = False
		Dim msg As String


		msg = String.Format("LANr {0}, Kanton: {1} >>> TempAnzahl: {2} * TempBasis: {3} * TempAnsatz: {4} = TempBetrag: {5} | U(2): {6} | CalculateWithGivenAmount: {7}", TempLANr, strKanton, TempAnzahl, TempBasis, TempAnsatz, TempBetrag, U(2), CalculateWithGivenAmount)

		Try

			' U(2) ist das AHV-Basis (aktuellem Monat!!!)
			totalAHVLohnInMonthToWatch = U(2)
			If LPMonth > 1 Then
				cTotalAHVInYear = m_PayrollDatabaseAccess.LoadAHVLohnInYear(m_EmployeeData.EmployeeNumber, MDNr, LPMonth, LPYear)
			End If

			' this should be becacuse this month 7100 la is not yet in db!
			cTotalAHVInYear += totalAHVLohnInMonthToWatch

			Dim err = False
			Dim rTemprec = m_PayrollDatabaseAccess.LoadMD_KiAuDataData(MDNr, strKanton, LPYear, err)
			ThrowExceptionOnError(err, "MD_KiAu Daten konnten nicht geladen werden")
			If rTemprec Is Nothing Then
				sMsgText = String.Format("<b>Achtung:</b> Mandant: {0}, Jahr: {1}, Kanton: {2} >>> existieren keine Angaben für Kinder- und Ausbildungszulagen. Es wird keine Korrektur durchgenommen!", MDNr, LPYear, strKanton)
				WriteToProtocol(Padright("M -> (Get_AuData_2): ", 30, " ") & sMsgText)

				Return False
			End If


			ExistsESBeginEnde = IsESBeginOrEndInMonth()
			If rTemprec.AtEndBeginES Then
				If Not ExistsESBeginEnde Then
					msg &= String.Format("{0}ExistsESBeginEnde: {1} >>> is canceling!", vbNewLine, ExistsESBeginEnde)
					Return False
				End If
			End If

			cMonthLimit = Val(rTemprec.YMinLohn) / 12
			cAuMonthBetrag = Val(rTemprec.Au1_Month)
			iOrgAnzahl = TempAnzahl

			cAu_StdBetrag = rTemprec.Au1_Std
			cAu_TagBetrag = rTemprec.Au1_Day

			AutoLO = rTemprec.ChangeAuIn
			If String.IsNullOrWhiteSpace(AutoLO) Then AutoLO = "A"
			If cAu_TagBetrag = 0 AndAlso AutoLO = "T" Then cAu_TagBetrag = If(CalculateWithGivenAmount, TempBasis, cAuMonthBetrag) / 30

			msg &= String.Format("{0}cAu_StdBetrag: {1} | cAu_TagBetrag: {2} | AutoLO: {3} | ExistsESBeginEnde: {4}", vbNewLine, cAu_StdBetrag, cAu_TagBetrag, AutoLO, ExistsESBeginEnde)
			msg &= String.Format("{0}SeeAHVLohnForYear: {1} | cMonthLimit: {2} | cTotalAHVInYear: {3} | totalAHVLohnInMonthToWatch: {4} | cAuMonthBetrag: {5}", vbNewLine, rTemprec.SeeAHVLohnForYear, cMonthLimit, cTotalAHVInYear, totalAHVLohnInMonthToWatch, cAuMonthBetrag)

			If rTemprec.SeeAHVLohnForYear.GetValueOrDefault(False) AndAlso totalAHVLohnInMonthToWatch < cMonthLimit Then
				totalAHVLohnInMonthToWatch = cTotalAHVInYear
				msg &= String.Format("{0}SeeAHVLohnForYear AndAlso totalAHVLohnInMonthToWatch < cMonthLimit: true | totalAHVLohnInMonthToWatch: {1} | cMonthLimit: {2}", vbNewLine, totalAHVLohnInMonthToWatch, cMonthLimit)

				If cTotalAHVInYear <> U(2) Then
					IsAHVLohnLowerAsAllowed = totalAHVLohnInMonthToWatch < rTemprec.YMinLohn.GetValueOrDefault(0)
					msg &= String.Format("{0}cTotalAHVInYear <> U(2): true | totalAHVLohnInMonthToWatch: {1} | YMinLohn : {2}", vbNewLine, totalAHVLohnInMonthToWatch, rTemprec.YMinLohn.GetValueOrDefault(0))

				Else
					IsAHVLohnLowerAsAllowed = totalAHVLohnInMonthToWatch < cMonthLimit
					msg &= String.Format("{0}cTotalAHVInYear <> U(2): false | totalAHVLohnInMonthToWatch: {1} | cMonthLimit : {2}", vbNewLine, totalAHVLohnInMonthToWatch, cMonthLimit)

				End If

			Else
				IsAHVLohnLowerAsAllowed = totalAHVLohnInMonthToWatch < cMonthLimit
				msg &= String.Format("{0}SeeAHVLohnForYear AndAlso totalAHVLohnInMonthToWatch < cMonthLimit: false | totalAHVLohnInMonthToWatch: {1} | cMonthLimit: {2}", vbNewLine, totalAHVLohnInMonthToWatch, cMonthLimit)

			End If
			msg &= String.Format("{0}IsAHVLohnLowerAsAllowed: {1}", vbNewLine, IsAHVLohnLowerAsAllowed)


			If IsAHVLohnLowerAsAllowed Then
				If ExistMLohn() Then
					msg &= String.Format("{0}ExistMLohn: false", vbNewLine)
					Return False
				End If

				sMsgText = String.Format("<b>Achtung:</b> {0} {1} {2} hat mit {3:n2} weniger als vordefinierter Mindestlohn {4:n2} verdient. Die Zulage wird gelöscht!",
																				 IIf(m_EmployeeData.Gender = "M", "Herr", "Frau"),
																				 m_EmployeeData.Lastname, m_EmployeeData.Firstname,
																				 totalAHVLohnInMonthToWatch,
																				 cMonthLimit)

				WriteToProtocol(Padright("M -> (Get_AuData_2): ", 30, " ") & sMsgText)

				TempAnzahl = TempAnzahl
				TempBasis = 0
				TempAnsatz = 100
				TempBetrag = 0
				msg = String.Format("{0}changing values >>> TempAnzahl: {1} * TempBasis: {2} * TempAnsatz: {3} = TempBetrag: {4} >>> returning true", vbNewLine, TempAnzahl, TempBasis, TempAnsatz, TempBetrag)

				Return True

			ElseIf ExistsESBeginEnde AndAlso AutoLO = "A" Then
				'ElseIf ESLPTage < 30 AndAlso AutoLO = "A" Then
				msg &= String.Format("{0}ExistsESBeginEnde AndAlso AutoLO = A: true >>> canceling", vbNewLine)
				Return False

			ElseIf Not ExistsESBeginEnde Then
				'ElseIf ESLPTage >= 30 Then
				msg &= String.Format("{0}ExistsESBeginEnde: false >>> canceling", vbNewLine)
				Return False

			End If

			sMsgText = String.Format("<b>Achtung:</b> {0} {1} {2}, ist während des Monats ein- oder ausgetreten. Die Ausbildungszulage wird entsprechend angepasst.",
																			 IIf(m_EmployeeData.Gender = "M", "Der Kandidat, Herr", "Die Kandidatin, Frau"),
																			 m_EmployeeData.Lastname, m_EmployeeData.Firstname)

			If AutoLO = "S" Then        ' Ausbildungszulagen in Stunden
				TempAnzahl = S(2) * TempAnzahl
				TempBasis = cAu_StdBetrag
				TempAnsatz = 100
				TempBetrag = Format(Math.Min(cAuMonthBetrag * iOrgAnzahl, TempAnzahl * cAu_StdBetrag), "0.00")
				TempLANr = 3850           ' Ausbildungszulagen in Stunden

				sMsgText &= String.Format("Automatische Änderung: {0}. Neue Lohnart lautet: {1:F4}.", AutoLO, TempLANr)

			ElseIf AutoLO = "T" Then    ' Ausbildungszulagen in Tag
				TempAnzahl = ESLPTage * TempAnzahl
				TempBasis = cAu_TagBetrag
				TempAnsatz = 100
				TempBetrag = Format(Math.Min(cAuMonthBetrag * iOrgAnzahl, TempAnzahl * cAu_TagBetrag), "0.00") ' Ausbildungszulagen
				TempLANr = 3800           ' Ausbildungszulagen in Tag

				sMsgText &= String.Format("Automatische Änderung: {0}. Neue Lohnart lautet: {1:F4}.", AutoLO, TempLANr)

			End If
			msg &= String.Format("{0}Changing: AutoLO: {1} | TempLANr: {2} | TempAnzahl: {3} * TempBasis: {4} * TempAnsatz: {5} = TempBetrag: {6}", vbNewLine, AutoLO, TempLANr, TempAnzahl, TempBasis, TempAnsatz, TempBetrag)

			WriteToProtocol(Padright("M -> (Get_AuData_2): ", 30, " ") & sMsgText)
			Return result

		Catch ex As Exception
			sMsgText = ex.ToString
			WriteToProtocol(Padright("*** -> (Get_KinderAuStatus): ", 40, " ") & sMsgText)

			Throw ex

		Finally
			strOriginData &= String.Format(" | {1}{0} ", vbNewLine, msg)

		End Try


		Return result

	End Function




#Region "old function for kinder und ausbildungszulagen!!!"

	<Obsolete("This method is deprecated, use Get_KiData_2 instead.", False)>
	Private Function Get_KinderZulageStatus(ByRef TempLANr As Decimal,
																		ByRef TempAnzahl As Decimal,
																		ByRef TempBasis As Decimal,
																		ByRef TempAnsatz As Decimal,
																		ByRef TempBetrag As Decimal,
																		ByVal strKanton As String)

		Dim AutoLO As String
		'Dim KIZulage As Decimal
		Dim cMaxFakStd As Decimal
		Dim sMsgText As String
		Dim iOrgAnzahl As Integer
		Dim strZivil2 As String
		Dim cKI_StdBetrag As Decimal
		Dim cKI_TagBetrag As Decimal

		Try

			Dim err = False
			Dim rTemprec = m_PayrollDatabaseAccess.LoadMD_KiAuDataData(MDNr, strKanton, LPYear, err)
			ThrowExceptionOnError(err, "LoadMD_KiAu Daten konnten nicht geladen werden.")

			If Not rTemprec Is Nothing Then

				iOrgAnzahl = TempAnzahl
				strZivil2 = UCase(m_EmployeeData.CivilState2)
				If strZivil2 = "V" Then
					cMaxFakStd = rTemprec.Ki1_FakMax
					cKI_StdBetrag = rTemprec.Ki1_Std
					cKI_TagBetrag = rTemprec.Ki1_Day

				Else
					cMaxFakStd = rTemprec.Ki2_FakMax
					cKI_StdBetrag = IIf(rTemprec.Ki2_Std = 0,
																	rTemprec.Ki1_Std, rTemprec.Ki2_Std)
					cKI_TagBetrag = IIf(rTemprec.Ki2_Day = 0,
																	rTemprec.Ki1_Day, rTemprec.Ki2_Day)

				End If

				If strZivil2 = "A" Then
					If S(2) < rTemprec.Ki2_FakMax Then
						sMsgText = "Achtung: " & IIf(m_EmployeeData.Gender = "M", "Herr ", "Frau ")
						sMsgText = sMsgText & m_EmployeeData.Lastname & " " & m_EmployeeData.Firstname & " hat weniger als definierte Fak-Stunden gearbeitet. "
						sMsgText = sMsgText & "Ich werde die Lohnart (" & TempLANr & ") automatisch für Sie ändern."
						If ExistMLohn() Then Exit Function
						WriteToProtocol(Padright("M -> (Get_KinderZulageStatus): ", 30, " ") & sMsgText)

					Else
						Exit Function

					End If
				Else
					If S(2) < rTemprec.Ki1_FakMax Then
						sMsgText = "Achtung: " & IIf(m_EmployeeData.Gender = "M", "Herr ", "Frau ")
						sMsgText = sMsgText & m_EmployeeData.Lastname & " " & m_EmployeeData.Firstname & " hat weniger als definierte Fak-Stunden gearbeitet. "
						sMsgText = sMsgText & "Ich werde die Lohnart (" & TempLANr & ") automatisch für Sie ändern."
						If ExistMLohn() Then Exit Function
						WriteToProtocol(Padright("M -> (Get_KinderZulageStatus): ", 30, " ") & sMsgText)

					Else
						Exit Function

					End If
				End If

				AutoLO = rTemprec.ChangeKiIn
				If S(2) = 0 Then
					AutoLO = rTemprec.ChangeKiIn_2
				End If
				If AutoLO = "" Then AutoLO = "S"

				If AutoLO = "S" Then        ' Kinderzulagen in Stunden
					'KIZulage = S(2) * cKI_StdBetrag

					TempLANr = 3700           ' Kinderzulage in Stunden
					TempAnzahl = S(2) * TempAnzahl
					TempBasis = cKI_StdBetrag
					TempAnsatz = 100
					TempBetrag = Math.Min(Convert.ToDecimal(rTemprec.Ki1_Month * iOrgAnzahl), TempAnzahl * cKI_StdBetrag)

				ElseIf AutoLO = "T" Then    ' Kinderzulagen in Tag
					'KIZulage = ESLPTage * cKI_TagBetrag

					TempLANr = 3650           ' ' Kinderzulage in Tag
					TempAnzahl = ESLPTage * TempAnzahl
					TempBasis = cKI_TagBetrag
					TempAnsatz = 100
					TempBetrag = Math.Min(Convert.ToDecimal(rTemprec.Ki1_Month * iOrgAnzahl), TempAnzahl * cKI_TagBetrag) 'KIZulage

				End If

			End If

		Catch ex As Exception
			sMsgText = ex.Message ', vbExclamation, "Kinderzulagen"
			WriteToProtocol(Padright("*** -> (Get_KinderZulageStatus): ", 40, " ") & sMsgText)
			Throw ex
		End Try

		Exit Function
	End Function

	' Funktionen bis Jahr 31.12.2008
	<Obsolete("This method is deprecated, use Get_AuData_2 instead.", False)>
	Private Function Get_KinderAuStatus(ByRef TempLANr As Decimal,
																	ByRef TempAnzahl As Decimal,
																	ByRef TempBasis As Decimal,
																	ByRef TempAnsatz As Decimal,
																	ByRef TempBetrag As Decimal,
																	ByVal strKanton As String)
		Dim cMaxFakStd As Decimal
		Dim AutoLO As String
		'Dim KIZulage As Decimal
		Dim sMsgText As String
		Dim iOrgAnzahl As Integer
		Dim strZivil2 As String
		Dim cAu_StdBetrag As Decimal
		Dim cAu_TagBetrag As Decimal

		Try

			Dim err = False
			Dim rTemprec = m_PayrollDatabaseAccess.LoadMD_KiAuDataData(MDNr, strKanton, LPYear, err)
			ThrowExceptionOnError(err, "MD_KiAu Daten konnten nicht geladen werden.")

			If Not rTemprec Is Nothing Then
				iOrgAnzahl = TempAnzahl
				strZivil2 = UCase(m_EmployeeData.CivilState2)
				If strZivil2 = "V" Then
					cMaxFakStd = rTemprec.Ki1_FakMax
					cAu_StdBetrag = rTemprec.Au1_Std
					cAu_TagBetrag = rTemprec.Au1_Day

				Else
					cMaxFakStd = rTemprec.Ki2_FakMax
					cAu_StdBetrag = IIf(rTemprec.Au2_Std = 0,
																	rTemprec.Au1_Std, rTemprec.Au2_Std)
					cAu_TagBetrag = IIf(rTemprec.Au2_Day = 0,
																	rTemprec.Au1_Day, rTemprec.Au2_Day)

				End If

				If strZivil2 = "A" Then
					If S(2) < rTemprec.Ki2_FakMax Then
						sMsgText = "Achtung: " & IIf(m_EmployeeData.Gender = "M", "Herr ", "Frau ")
						sMsgText = sMsgText & m_EmployeeData.Lastname & " " & m_EmployeeData.Firstname & " hat weniger als definierte Fak-Stunden gearbeitet. "
						sMsgText = sMsgText & "Ich werde die Lohnart (" & TempLANr & ") automatisch für Sie ändern."

						If ExistMLohn() Then Exit Function
						WriteToProtocol(Padright("M -> (Get_KinderAuStatus): ", 30, " ") & sMsgText)

					Else
						Exit Function

					End If

				Else
					If S(2) < rTemprec.Ki1_FakMax Then
						sMsgText = "Achtung: " & IIf(m_EmployeeData.Gender = "M", "Herr ", "Frau ")
						sMsgText = sMsgText & m_EmployeeData.Lastname & " " & m_EmployeeData.Firstname & " hat weniger als definierte Fak-Stunden gearbeitet. "
						sMsgText = sMsgText & "Ich werde die Lohnart (" & TempLANr & ") automatisch für Sie ändern."

						If ExistMLohn() Then Exit Function
						WriteToProtocol(Padright("M -> (Get_KinderAuStatus): ", 30, " ") & sMsgText)

					Else
						Exit Function

					End If

				End If
				AutoLO = rTemprec.ChangeAuIn
				If S(2) = 0 Then
					AutoLO = rTemprec.ChangeAuIn_2
				End If
				If AutoLO = "" Then AutoLO = "S"

				If AutoLO = "S" Then        ' Ausbildungszulage in Stunden
					'KIZulage = S(2) * cAu_StdBetrag

					TempLANr = 3850           ' Ausbildungszulage in Stunden
					TempAnzahl = S(2) * TempAnzahl
					TempBasis = cAu_StdBetrag
					TempAnsatz = 100
					TempBetrag = Math.Min(Convert.ToDecimal(rTemprec.Au1_Month * iOrgAnzahl), TempAnzahl * cAu_StdBetrag)

				ElseIf AutoLO = "T" Then    ' Ausbildungszulage in Tag
					'KIZulage = ESLPTage * cAu_TagBetrag

					TempLANr = 3800           ' Ausbildungszulage in Tag
					TempAnzahl = TempAnzahl * ESLPTage
					TempBasis = cAu_TagBetrag
					TempAnsatz = 100
					TempBetrag = Math.Min(Convert.ToDecimal(rTemprec.Au1_Month * iOrgAnzahl), TempAnzahl * cAu_TagBetrag)

				End If

			End If

		Catch ex As Exception
			sMsgText = ex.Message ', vbExclamation, "Ausbildungszulagen"
			WriteToProtocol(Padright("*** -> (Get_KinderAuStatus): ", 40, " ") & sMsgText)

			Throw ex

		End Try

	End Function

#End Region


End Class
