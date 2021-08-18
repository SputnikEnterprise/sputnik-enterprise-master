
Imports System.Text.RegularExpressions

Partial Class frmOPSearch

	Function GetStartSQLString() As String
		Dim sql As String = String.Empty

		Try
			Dim sSqlLen As Integer = 0
			Dim sZusatzBez As String = String.Empty
			Dim i As Integer = 0

			Dim invoiceDeadlineDate As Date?
			Dim invoiceFromDate As Date?
			Dim invoiceToDate As Date?
			If Not deStichtag.EditValue Is Nothing AndAlso IsDate(deStichtag.EditValue) Then invoiceDeadlineDate = DateTime.ParseExact(deStichtag.EditValue, "dd.MM.yyyy", Nothing)

			If Not deFakDate_1.EditValue Is Nothing AndAlso IsDate(deFakDate_1.EditValue) Then invoiceFromDate = DateTime.ParseExact(deFakDate_1.EditValue, "dd.MM.yyyy", Nothing)
			If Not deFakDate_2.EditValue Is Nothing AndAlso IsDate(deFakDate_2.EditValue) Then invoiceToDate = DateTime.ParseExact(deFakDate_2.EditValue, "dd.MM.yyyy", Nothing)

			Dim createdFromDate As Date?
			Dim createdToDate As Date?
			If Not deCreated_1.EditValue Is Nothing AndAlso IsDate(deCreated_1.EditValue) Then createdFromDate = DateTime.ParseExact(deCreated_1.EditValue, "dd.MM.yyyy", Nothing)
			If Not deCreated_2.EditValue Is Nothing AndAlso IsDate(deCreated_2.EditValue) Then createdToDate = DateTime.ParseExact(deCreated_2.EditValue, "dd.MM.yyyy", Nothing)

			sql = String.Format("Begin Try Drop Table {0} End Try Begin Catch End Catch ", ClsDataDetail.SPTabNamenDBL)
			sql &= "SELECT RE.ID ,"
			sql &= "RE.RENR ,"
			sql &= "RE.KDNR ,"
			sql &= "RE.ART ,"
			sql &= "RE.KST ,"
			sql &= "RE.LP ,"
			sql &= "RE.FAK_DAT ,"
			sql &= "RE.Currency ,"
			sql &= "RE.BetragOhne ,"
			sql &= "RE.BetragEx ,"
			sql &= "RE.BetragInk ,"
			sql &= "RE.MWST1 ,"
			sql &= "RE.MWSTProz ,"
			sql &= "RE.Bezahlt ,"
			sql &= "RE.SKonto ,"
			sql &= "RE.Verlust ,"
			sql &= "RE.FSKonto ,"
			sql &= "RE.FVerlust ,"
			sql &= "RE.Faellig ,"
			sql &= "RE.Mahncode ,"
			sql &= "RE.SPNr ,"
			sql &= "RE.VerNr ,"
			sql &= "RE.MA0 ,"
			sql &= "RE.MA1 ,"
			sql &= "RE.MA2 ,"
			sql &= "RE.MA3 ,"
			sql &= "RE.Storno ,"
			sql &= "RE.Gebucht ,"
			sql &= "RE.FBMonat ,"
			sql &= "RE.FBDat ,"
			sql &= "RE.FKSoll ,"
			sql &= "RE.FKHaben0 ,"
			sql &= "RE.FKHaben1 ,"
			sql &= "RE.R_Name1 ,"
			sql &= "RE.R_Name2 ,"
			sql &= "RE.R_Name3 ,"
			sql &= "RE.R_ZHD ,"
			sql &= "RE.R_Postfach ,"
			sql &= "RE.R_Strasse ,"
			sql &= "RE.R_Land ,"
			sql &= "RE.R_PLZ ,"
			sql &= "RE.R_Ort ,"
			sql &= "RE.Zahlkond ,"
			sql &= "RE.Result ,"
			sql &= "RE.RefNr ,"
			sql &= "RE.RefFootNr ,"
			sql &= "RE.ESRArt ,"
			sql &= "RE.ESRID ,"
			sql &= "RE.ESRKonto ,"
			sql &= "RE.MWSTNr ,"
			sql &= "RE.KontoNr ,"
			sql &= "RE.BtrFr ,"
			sql &= "RE.btrRp ,"
			sql &= "RE.REKST1 ,"
			sql &= "RE.REKST2 ,"
			sql &= "Convert(Date, RE.PrintedDate) PrintedDate ,"
			sql &= "RE.GebuchtAm ,"
			sql &= "RE.ZEInfo ,"
			sql &= "RE.CreatedOn ,"
			sql &= "RE.CreatedFrom ,"
			sql &= "RE.ChangedOn ,"
			sql &= "RE.ChangedFrom ,"
			sql &= "RE.R_Abteilung ,"
			sql &= "RE.Ma3_RepeatNr ,"
			sql &= "RE.ES_Einstufung ,"
			sql &= "RE.KDBranche ,"
			sql &= "RE.[DTA Name] ,"
			sql &= "RE.[DTA PLZOrt] ,"
			sql &= "RE.[ESR BankName] ,"
			sql &= "RE.[ESR BankAdresse] ,"
			sql &= "RE.[DTA Konto] ,"
			sql &= "RE.IBANDTA ,"
			sql &= "RE.IBANVG ,"
			sql &= "RE.ESR_Swift ,"
			sql &= "RE.ESR_IBAN1 ,"
			sql &= "RE.ESR_IBAN2 ,"
			sql &= "RE.ESR_IBAN3 ,"
			sql &= "RE.ESR_BcNr ,"
			sql &= "RE.ProposeNr ,"
			sql &= "RE.Art_2 ,"
			sql &= "RE.MahnStopUntil ,"
			sql &= "RE.REDoc_Guid ,"
			sql &= "RE.Transfered_User ,"
			sql &= "RE.Transfered_On ,"
			sql &= "RE.ZEBis_0 ,"
			sql &= "RE.ZEBis_1 ,"
			sql &= "RE.ZEBis_2 ,"
			sql &= "RE.ZEBis_3 ,"
			sql &= "RE.MDNr, "

			sql &= "V_Bezahlt = CASE WHEN (RE.ART = 'G' Or RE.ART = 'R') "
			sql += String.Format(" And ('{0:d}' = '' Or Convert(Date, GebuchtAm) <= Convert(Date, '{0:d}')) THEN RE.Bezahlt ", invoiceDeadlineDate) ' invoiceToDate)
			'sql += "THEN RE.Bezahlt "

			sql += "ELSE "
			sql += " IsNull((Select Sum(ZE.Betrag) "
			sql += "  From dbo.ZE "
			sql += "  Where "
			sql += "	 ZE.RENR=RE.RENR And "
			sql += String.Format("('{0:d}' = '' Or Convert(Date, ZE.V_Date) <= Convert(Date, '{0:d}') )), 0)", invoiceDeadlineDate) ' invoiceToDate)
			sql += " END, "
			'' employeeadvisor
			sql += "ISNULL(( SELECT TOP 1 " &
													"(Nachname + ', '+Vorname) AS USName " &
				"FROM dbo.Benutzer " &
									 "WHERE  USNR = ( SELECT TOP 1 " &
				"USNR " &
				"FROM dbo.Benutzer " &
																	 "WHERE  KST LIKE SUBSTRING(RE.Kst, 0, " &
																														 "CHARINDEX('/', " &
																																"RE.Kst)) " &
																					"OR KST LIKE RE.Kst " &
																 ") " &
								 "), '') AS employeeadvisor , "

			'' customeradvisor
			sql += "ISNULL(( SELECT TOP 1 " &
												 "(Nachname + ', '+Vorname) AS USName " &
			 "FROM dbo.Benutzer " &
									 "WHERE  USNR = ( SELECT TOP 1 " &
				"USNR " &
				"FROM dbo.Benutzer " &
																	 "WHERE  KST LIKE SUBSTRING(RE.Kst, " &
																														 "CHARINDEX('/', " &
																																"RE.Kst) + 1, " &
																														 "LEN(RE.Kst)) " &
																					"OR KST LIKE RE.Kst " &
																 ") " &
								 "), '') AS customeradvisor, "

			sql += "KD.KreditLimite, KD.KreditLimiteAb As KDKreditlimiteAb, KD.KL_RefNr As Kredit_RefNr, "
			sql += "KD.KreditLimiteBis As KDKreditlimiteBis, KD.KreditLimite_2, IsNull(KD.KD_UmsMin, 0) As KD_UmsMin, "
			sql += "KD.KDFiliale "

			sql += String.Format("Into {0} ", ClsDataDetail.SPTabNamenDBL)
			sql += "From dbo.RE"
			sql += " Left Join dbo.Kunden KD On KD.KDNr = RE.KDNr "

			sSqlLen = Len(sql)


		Catch ex As Exception
			MessageBox.Show(ex.ToString, "GetStartSQLString", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try


		Return sql
	End Function

	'Function GetStartSQLString_2() As String
	'	Dim sSql As String = String.Empty

	'	Try
	'		Dim sSqlLen As Integer = 0
	'		Dim sZusatzBez As String = String.Empty
	'		Dim i As Integer = 0


	'		sSql = "Select RE.RENr, RE.BetragInk, RE.Bezahlt, RE.Gebucht, RE.Art, RE.GebuchtAm "
	'		sSql += "From dbo.RE "

	'		sSqlLen = Len(sSql)


	'	Catch ex As Exception
	'		MessageBox.Show(ex.ToString, "GetStartSQLString_2", MessageBoxButtons.OK, MessageBoxIcon.Error)
	'	End Try

	'	Return sSql
	'End Function


	Function LoadWherQuery(ByVal sSQLQuery As String) As String
		Dim sSql As String = String.Empty

		Try
			Dim sOldQuery As String = sSQLQuery
			Dim strFieldName As String = String.Empty

			Dim cv As ComboValue
			Dim sSqlLen As Integer = 0
			Dim sZusatzBez As String = String.Empty
			Dim strAndString As String = String.Empty

			Dim strUSFiliale As String = m_InitializationData.UserData.UserFiliale
			Dim iSQLLen As Integer = Len(sSQLQuery)

			Dim strName As String()
			Dim strMyName As String = String.Empty

			Dim invoiceDeadlineDate As Date?
			Dim invoiceFromDate As Date?
			Dim invoiceToDate As Date?
			If Not deStichtag.EditValue Is Nothing AndAlso IsDate(deStichtag.EditValue) Then invoiceDeadlineDate = DateTime.ParseExact(deStichtag.EditValue, "dd.MM.yyyy", Nothing)

			If Not deFakDate_1.EditValue Is Nothing AndAlso IsDate(deFakDate_1.EditValue) Then invoiceFromDate = DateTime.ParseExact(deFakDate_1.EditValue, "dd.MM.yyyy", Nothing)
			If Not deFakDate_2.EditValue Is Nothing AndAlso IsDate(deFakDate_2.EditValue) Then invoiceToDate = DateTime.ParseExact(deFakDate_2.EditValue, "dd.MM.yyyy", Nothing)

			Dim createdFromDate As Date?
			Dim createdToDate As Date?
			If Not deCreated_1.EditValue Is Nothing AndAlso IsDate(deCreated_1.EditValue) Then createdFromDate = DateTime.ParseExact(deCreated_1.EditValue, "dd.MM.yyyy", Nothing)
			If Not deCreated_2.EditValue Is Nothing AndAlso IsDate(deCreated_2.EditValue) Then createdToDate = DateTime.ParseExact(deCreated_2.EditValue, "dd.MM.yyyy", Nothing)

			With Me
				strFieldName = "RE.RENr"
				If .txtOPNr_1.Text = String.Empty And .txtOPNr_2.Text = String.Empty Then
				ElseIf .txtOPNr_1.Text.Contains("*") Or .txtOPNr_1.Text.Contains("%") Then
					'FilterBez = "Fakturanummer wie (" & .txtOPNr_1.Text & ") " & vbLf

					ClsDataDetail.GetFilterBezArray.Add("Fakturanummer wie (" & .txtOPNr_1.Text & ") " & vbLf)
					sSql += strFieldName & " Like " & Replace(.txtOPNr_1.Text, "*", "%")

				ElseIf InStr(.txtOPNr_1.Text, ",") > 0 Then
					'FilterBez = "Fakturanummer wie (" & .txtOPNr_1.Text & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Fakturanummer wie (" & .txtOPNr_1.Text & ") " & vbLf)
					sSql += strFieldName & " In (" & .txtOPNr_1.Text & ")"

				ElseIf .txtOPNr_1.Text = .txtOPNr_2.Text Then
					'FilterBez = "Fakturanummer = " & .txtOPNr_1.Text & " " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Fakturanummer = " & .txtOPNr_1.Text & " " & vbLf)
					sSql += strFieldName & " = " & CInt(.txtOPNr_1.Text)

				ElseIf .txtOPNr_1.Text <> "" And .txtOPNr_2.Text = "" Then
					'FilterBez = "Fakturanummer ab " & .txtOPNr_1.Text & " " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Fakturanummer ab " & .txtOPNr_1.Text & " " & vbLf)
					sSql += strFieldName & " >= " & CInt(.txtOPNr_1.Text)

				ElseIf .txtOPNr_1.Text = "" And .txtOPNr_2.Text <> "" Then
					'FilterBez = "Fakturanummer bis " & .txtOPNr_2.Text & " " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Fakturanummer bis " & .txtOPNr_2.Text & " " & vbLf)
					sSql += strFieldName & " <= " & CInt(.txtOPNr_2.Text)

				Else
					'FilterBez = "Fakturanummer zwischen " & .txtOPNr_1.Text & " und " & .txtOPNr_2.Text & " " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Fakturanummer zwischen " & .txtOPNr_1.Text & " und " & .txtOPNr_2.Text & " " & vbLf)
					sSql += strFieldName & " Between " & CInt(.txtOPNr_1.Text) &
													" And " & CInt(.txtOPNr_2.Text)
				End If

				' KDNr -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.KDNr"
				If .txtKDNr_1.Text = "" And .txtKDNr_2.Text = "" Then
				ElseIf .txtKDNr_1.Text.Contains("*") Or .txtKDNr_1.Text.Contains("%") Then
					ClsDataDetail.GetFilterBezArray.Add("Kundennummer wie (" & .txtKDNr_1.Text & ") " & vbLf)
					sSql += strAndString & strFieldName & " Like " & Replace(.txtKDNr_1.Text, "*", "%") & ""

				ElseIf InStr(.txtKDNr_1.Text, ",") > 0 Then
					ClsDataDetail.GetFilterBezArray.Add("Kundennummer wie (" & .txtKDNr_1.Text & ") " & vbLf)
					sZusatzBez = .txtKDNr_1.Text
					sZusatzBez = Replace(sZusatzBez, ", ", ",")
					sSql += strAndString & strFieldName & " In (" & Replace(sZusatzBez, ",", ",") & ")"

				ElseIf UCase(.txtKDNr_1.Text) = UCase(.txtKDNr_2.Text) Then
					ClsDataDetail.GetFilterBezArray.Add("Kundennummer = " & .txtKDNr_2.Text & vbLf)
					sSql += strAndString & strFieldName & " like " &
										.txtKDNr_1.Text

				ElseIf .txtKDNr_1.Text <> "" And .txtKDNr_2.Text = "" Then
					ClsDataDetail.GetFilterBezArray.Add("Kundennummer ab " & .txtKDNr_1.Text & vbLf)
					sSql += strAndString & strFieldName & " >= " &
										.txtKDNr_1.Text

				ElseIf .txtKDNr_1.Text = "" And .txtKDNr_2.Text <> "" Then
					ClsDataDetail.GetFilterBezArray.Add("Kundennummer bis " & .txtKDNr_2.Text & vbLf)
					sSql += strAndString & strFieldName & " <= " &
										.txtKDNr_2.Text

				Else
					ClsDataDetail.GetFilterBezArray.Add("Kundennummer zwischen " & .txtKDNr_1.Text & " und " & .txtKDNr_2.Text & vbLf)
					sSql += strAndString & strFieldName & " Between " &
								.txtKDNr_1.Text & " And " & .txtKDNr_2.Text
				End If

				' Fak_Dat oder CreatedOn -------------------------------------------------------------------------------------------------------
				'If .txtVGNr_1.Text + .txtVGNr_2.Text <> "" Then
				If .chkValutaFromCreatedOn.Checked Then
					ClsDataDetail.GetFilterBezArray.Add("Es werden nach Erstellungsdatum in den Rechnungen geschaut!" & vbLf)

				Else
					ClsDataDetail.GetFilterBezArray.Add("Es werden nach Fakturadatum in den Rechnungen geschaut!" & vbLf)

				End If
				'End If
				' Vergütungsnummer ---------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = If(.chkValutaFromCreatedOn.Checked, "RE.CreatedOn", "RE.Fak_Dat")
				If Val(.txtVGNr_1.EditValue) = 0 AndAlso Val(.txtVGNr_2.EditValue) = 0 Then

				ElseIf Val(.txtVGNr_1.EditValue) = Val(.txtVGNr_2.EditValue) Then
					'FilterBez += "Verfalltage = " & .txtVGNr_1.Text & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Verfalltage = " & Val(.txtVGNr_1.EditValue) & vbLf)
					sSql += strAndString & "DateDiff(day, " & strFieldName & ", convert(nvarchar(10), GetDate(), 104)) = " &
										Val(.txtVGNr_1.EditValue) & " "

				ElseIf Val(.txtVGNr_1.EditValue) <> 0 And Val(.txtVGNr_2.EditValue) = 0 Then
					'FilterBez += "Verfalltage ab " & .txtVGNr_1.Text & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Verfalltage ab " & Val(.txtVGNr_1.EditValue) & vbLf)
					sSql += strAndString & "DateDiff(day, " & strFieldName & ", convert(nvarchar(10), GetDate(), 104)) >= " &
										Val(.txtVGNr_1.EditValue) & " "

				ElseIf Val(.txtVGNr_1.EditValue) = 0 And Val(.txtVGNr_2.EditValue) <> 0 Then
					'FilterBez += "Verfalltage bis " & .txtKDNr_2.Text & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Verfalltage bis " & Val(.txtVGNr_2.EditValue) & vbLf)
					sSql += strAndString & "DateDiff(day, " & strFieldName & ", convert(nvarchar(10), GetDate(), 104)) <= " &
										Val(.txtVGNr_2.EditValue) & " "

				Else
					'FilterBez += "Verfalltage zwischen " & .txtVGNr_1.Text & " und " & .txtVGNr_2.Text & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Verfalltage zwischen " & Val(.txtVGNr_1.EditValue) & " und " & Val(.txtVGNr_2.EditValue) & vbLf)
					sSql += strAndString & "DateDiff(day, " & strFieldName & ", convert(nvarchar(10), GetDate(), 104)) Between " &
								Val(.txtVGNr_1.EditValue) & " And " & Val(.txtVGNr_2.EditValue) & " "
				End If

				' Mahncode -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.Mahncode"
				If Not String.IsNullOrWhiteSpace(.luePaymentReminderCode.EditValue) Then
					sZusatzBez = .luePaymentReminderCode.EditValue
					'FilterBez += "Mahncode wie (" & sZusatzBez & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Mahncode wie (" & sZusatzBez & ") " & vbLf)

					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					Next
					If strName.Length > 0 Then sZusatzBez = strMyName
					If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

					sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
				End If

				' Mahnstufe -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If UCase(.cbo_Mahnstufe.Text) <> String.Empty Then
					If .cbo_Mahnstufe.Text.Length > 0 Then
						strFieldName = String.Format("RE.Ma{0}", CInt(Val(.cbo_Mahnstufe.Text.Substring(0, 1))))
					End If
					sZusatzBez = .cbo_Mahnstufe.Text.Trim
					'FilterBez += "Mahncode wie (" & sZusatzBez & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Mahnstufe wie (" & sZusatzBez & ") " & vbLf)

					sSql += strAndString & strFieldName & " Is Not Null"
				End If

				' MahnDatum -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If UCase(.deMahnDate.Text) <> String.Empty Then
					If .cbo_Mahnstufe.Text.Length > 0 Then
						strFieldName = String.Format("RE.Ma{0}", CInt(Val(.cbo_Mahnstufe.Text.Substring(0, 1))))
					Else
						strFieldName = String.Empty
					End If
					sZusatzBez = .deMahnDate.Text.Trim
					'FilterBez += "Mahncode wie (" & sZusatzBez & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Mahndatum wie (" & sZusatzBez & ") " & vbLf)

					If strFieldName = String.Empty Then
						sSql += strAndString & String.Format("(ma0 = '{0}' Or ma1 = '{0}' Or ma2 = '{0}' Or ma3 = '{0}')", .deMahnDate.Text)

					Else
						sSql += strAndString & String.Format("ma{1} = '{0}'", .deMahnDate.Text,
																								 CInt(Val(.cbo_Mahnstufe.Text.Substring(0, 1))))

					End If

				End If

				' Faktura-Art -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If Not String.IsNullOrWhiteSpace(.lueDebitorenart.EditValue) Then
					sZusatzBez = .lueDebitorenart.EditValue

					' Bezeichnung ausschreiben
					Dim codeAusgeschrieben As String = ""
					For Each fakturaArt In sZusatzBez.Split(CChar(","))
						Select Case fakturaArt.Trim
							Case "A"
								codeAusgeschrieben += String.Format("(A) Automatische ")
							Case "F"
								codeAusgeschrieben += String.Format("(F) Festanstellung ")
							Case "G"
								codeAusgeschrieben += String.Format("(G) Gutschriften ")
							Case "I"
								codeAusgeschrieben += String.Format("(I) Individuelle ")
							Case "R"
								codeAusgeschrieben += String.Format("(R) Rückvergütung ")
						End Select

					Next
					'FilterBez += String.Format("Faktura-Art: {0}{1}", codeAusgeschrieben, vbLf)
					ClsDataDetail.GetFilterBezArray.Add(String.Format("Faktura-Art: {0}{1}", codeAusgeschrieben, vbLf))

					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					Next
					If strName.Length > 0 Then sZusatzBez = strMyName
					If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

					sSql += String.Format("{0}RE.Art In ('{1}')", strAndString, sZusatzBez)
				End If

				' 1. KST -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.REKst1"
				If UCase(.Cbo_KST1.Text) <> String.Empty Then
					sZusatzBez = .Cbo_KST1.Text.Trim
					'FilterBez += .Label9.Text & " wie (" & sZusatzBez & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add(.lbl1kst.Text & " wie (" & sZusatzBez & ") " & vbLf)

					'If Not .CheckBox1.Checked Then
					'	sSql += strAndString & strFieldName & " Like '%" & sZusatzBez & "%' "

					'Else
					strName = Regex.Split(sZusatzBez.Trim, ",")
						strMyName = String.Empty
						For i As Integer = 0 To strName.Length - 1
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
						Next
						If strName.Length > 0 Then sZusatzBez = strMyName

						If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
							sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
						Else
							If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

							sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
						End If
					'End If

				End If

				' 2. KST -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.REKst2"
				If UCase(.Cbo_KST2.Text) <> String.Empty Then
					sZusatzBez = .Cbo_KST2.Text.Trim
					'FilterBez += .Label28.Text & " wie (" & sZusatzBez & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add(.lbl2kst.Text & " wie (" & sZusatzBez & ") " & vbLf)

					'If Not .CheckBox2.Checked Then
					'	sSql += strAndString & strFieldName & " Like '%" & sZusatzBez & "%' "

					'Else
					strName = Regex.Split(sZusatzBez.Trim, ",")
						strMyName = String.Empty
						For i As Integer = 0 To strName.Length - 1
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
						Next
						If strName.Length > 0 Then sZusatzBez = strMyName

						If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
							sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
						Else
							If InStr(sZusatzBez, ",") > 0 Then
								sZusatzBez = Replace(sZusatzBez, ",", "','")

							End If
							sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
						End If
						'End If

					End If

				' Berater -------------------------------------------------------------------------------------------------------
				If UCase(.Cbo_Berater.Text) <> String.Empty Then
					strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
					strFieldName = "RE.Kst"
					ClsDataDetail.GetFilterBezArray.Add(.lblberater.Text & " wie (" & .Cbo_Berater.Text & ") " & vbLf)

					If Not String.IsNullOrWhiteSpace(.Cbo_Berater.Text) Then
						Dim aUSData As String() = .Cbo_Berater.Text.Split(CChar("("))
						sZusatzBez = aUSData(1).Replace(")", "").ToUpper
					End If
					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i) & "'"
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '" & strName(i) & "/%'"
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%/" & strName(i) & "'"
					Next

					If strName.Length > 0 Then sZusatzBez = strMyName
					sSql += strAndString & " (" & sZusatzBez & ")"
				End If

				' Filiale -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.Kst"
				If UCase(.Cbo_Filiale.Text) <> String.Empty Then
					sZusatzBez = .Cbo_Filiale.Text.Trim
					'sZusatzBez = GetFilialKstData(sZusatzBez)
					Dim data = m_CommonDatabaseAccess.LoadKSTDataForGivenFilial(.Cbo_Filiale.Text.Trim)

					If Not data Is Nothing AndAlso data.Count > 0 Then
						strMyName = String.Empty

						For Each advisor In data
							strMyName &= If(String.IsNullOrWhiteSpace(strMyName), "", " Or ") & String.Format("{0} = '{1}'", strFieldName, advisor.KST)
							strMyName &= If(String.IsNullOrWhiteSpace(strMyName), "", " Or ") & String.Format("{0} Like '{1}/%'", strFieldName, advisor.KST)
							strMyName &= If(String.IsNullOrWhiteSpace(strMyName), "", " Or ") & String.Format("{0} Like '%/{1}'", strFieldName, advisor.KST)
						Next

						sSql += strAndString & " (" & strMyName & ")"

					End If

					'If sZusatzBez <> String.Empty Then
					'	ClsDataDetail.GetFilterBezArray.Add(String.Format("Filiale wie {1}: ({2}){0}", vbNewLine, .Cbo_Filiale.Text.Trim, sZusatzBez))

					'	sZusatzBez = Replace(sZusatzBez, "'", "")
					'	strName = Regex.Split(sZusatzBez.Trim, ",")
					'	strMyName = String.Empty
					'	For i As Integer = 0 To strName.Length - 1
					'		strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i) & "'"
					'		strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '" & strName(i) & "/%'"
					'		strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%/" & strName(i) & "'"
					'	Next
					'	If strName.Length > 0 Then sZusatzBez = strMyName
					'	sSql += strAndString & " (" & sZusatzBez & ")"
					'Else
					'	.Cbo_Filiale.Text = String.Empty
					'End If

				End If

				' Fakturadatum -----------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.Fak_Dat"
				If (.deFakDate_1.Text = String.Empty AndAlso .deFakDate_2.Text = String.Empty) AndAlso (.deCreated_1.Text = String.Empty AndAlso .deCreated_2.Text = String.Empty) AndAlso invoiceDeadlineDate Is Nothing Then

				ElseIf (.deFakDate_1.Text = String.Empty AndAlso .deFakDate_2.Text = String.Empty) AndAlso (.deCreated_1.Text = String.Empty AndAlso .deCreated_2.Text = String.Empty) AndAlso Not invoiceDeadlineDate Is Nothing Then
					ClsDataDetail.GetFilterBezArray.Add(String.Format("Fakturadatum bis {0:d}{1}", invoiceDeadlineDate, vbLf))
					sSql += String.Format("{0}(Convert(Date, {1}) <= Convert(Date, '{2:d}') OR Convert(Date, RE.CreatedOn) <= Convert(Date, '{2:d}') )", strAndString, strFieldName, invoiceDeadlineDate)

				ElseIf .deFakDate_1.Text = .deFakDate_2.Text AndAlso Not String.IsNullOrWhiteSpace(.deFakDate_1.Text) Then
					ClsDataDetail.GetFilterBezArray.Add("Fakturadatum am = " & .deFakDate_1.Text & vbLf)
					sSql += String.Format("{0}Convert(Date, {1}) = Convert(Date, '{2:d}')", strAndString, strFieldName, invoiceFromDate)

				ElseIf .deFakDate_1.Text <> "" And .deFakDate_2.Text = "" Then
					ClsDataDetail.GetFilterBezArray.Add("Fakturadatum ab " & .deFakDate_1.Text & vbLf)
					'sSql += strAndString & strFieldName & " >= '" & Format(CDate(.deFakDate_1.Text), "d") & "'"
					sSql += String.Format("{0}Convert(Date, {1}) >= Convert(Date,'{2:d}')", strAndString, strFieldName, invoiceFromDate)


				ElseIf .deFakDate_1.Text = "" And .deFakDate_2.Text <> "" Then
					ClsDataDetail.GetFilterBezArray.Add("Fakturadatum bis " & .deFakDate_2.Text & vbLf)
					'sSql += strAndString & strFieldName & " <= '" & Format(CDate(.deFakDate_2.Text), "d") & "'"
					sSql += String.Format("{0}Convert(Date, {1}) <= Convert(Date,'{2:d}')", strAndString, strFieldName, invoiceToDate)


				ElseIf .deFakDate_1.Text <> "" And .deFakDate_2.Text <> "" Then
					ClsDataDetail.GetFilterBezArray.Add("Fakturadatum zwischen " & .deFakDate_1.Text & " und " & .deFakDate_2.Text & vbLf)
					'sSql += strAndString & strFieldName & " Between '" & Format(CDate(.deFakDate_1.Text), "d") & "' And '" & Format(CDate(.deFakDate_2.Text), "d") & "'"
					sSql += String.Format("{0}(Convert(Date, {1}) Between Convert(Date, '{2:d}') And Convert(Date, '{3:d}'))", strAndString, strFieldName, invoiceFromDate, invoiceToDate)

				End If

				' Erstelldatum -----------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.CreatedOn"
				If (.deCreated_1.Text = String.Empty AndAlso .deCreated_2.Text = String.Empty) Then
				ElseIf .deCreated_1.Text = .deCreated_2.Text Then
					ClsDataDetail.GetFilterBezArray.Add("Erstelldatum am = " & .deCreated_1.Text & vbLf)
					sSql += String.Format("{0}Convert(Date, {1}) = Convert(Date, '{2:d}')", strAndString, strFieldName, createdFromDate)

				ElseIf .deCreated_1.Text <> "" And .deCreated_2.Text = "" Then
					ClsDataDetail.GetFilterBezArray.Add("Erstelldatum ab " & .deCreated_1.Text & vbLf)
					sSql += String.Format("{0}Convert(Date, {1}) >= Convert(Date,'{2:d}')", strAndString, strFieldName, createdFromDate)

				ElseIf .deCreated_1.Text = "" And .deCreated_2.Text <> "" Then
					ClsDataDetail.GetFilterBezArray.Add("Erstelldatum bis " & .deCreated_2.Text & vbLf)
					sSql += String.Format("{0}Convert(Date, {1}) <= Convert(Date, '{2:d}')", strAndString, strFieldName, createdToDate)

				Else
					ClsDataDetail.GetFilterBezArray.Add("Erstelldatum zwischen " & .deCreated_1.Text & " und " & .deCreated_2.Text & vbLf)
					sSql += String.Format("{0}(Convert(Date, {1}) Between Convert(Date, '{2:d}') And Convert(Date, '{3:d}'))", strAndString, strFieldName, createdFromDate, createdToDate)
				End If

				' Einstufung ----------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.ES_Einstufung"
				If UCase(.Cbo_ESEinstufung.Text) <> String.Empty Then
					sZusatzBez = .Cbo_ESEinstufung.Text.Trim
					'FilterBez += "Einstufung wie (" & sZusatzBez & ") " & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Einstufung wie (" & sZusatzBez & ") " & vbLf)

					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					Next
					If strName.Length > 0 Then sZusatzBez = strMyName
					If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

					If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
						sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
					Else

						sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
					End If
				End If

				' Branche ----------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.KDBranche"
				If UCase(.cbo_ESBranche.Text) <> String.Empty Then
					sZusatzBez = .cbo_ESBranche.Text.Trim
					ClsDataDetail.GetFilterBezArray.Add("Branche wie (" & sZusatzBez & ") " & vbLf)

					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					Next
					If strName.Length > 0 Then sZusatzBez = strMyName
					If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

					If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
						sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
					Else

						sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
					End If
				End If

				' ESR-Bank -------------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "RE.[KontoNr]"
				If UCase(.cbo_ESRBank.Text) <> String.Empty Then
					sZusatzBez = .cbo_ESRBank.Text.Trim
					cv = DirectCast(.cbo_ESRBank.SelectedItem, ComboValue)

					If Not cv Is Nothing Then
						Dim strValue As String = cv.ComboValue
						sZusatzBez = strValue
						strName = Regex.Split(sZusatzBez.Trim, ",")
						strMyName = String.Empty
						For i As Integer = 0 To strName.Length - 1
							strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
						Next
						If strName.Length > 0 Then sZusatzBez = strMyName
						ClsDataDetail.GetFilterBezArray.Add(String.Format("ESR-Bankname wie ({0}) - Konto-Nr.: {1}{2}", .cbo_ESRBank.Text, strValue, vbLf))

						If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")
						sSql += strAndString & strFieldName & " In ('" & strValue & "')"
					End If

				End If

				' Kreditlimite überschritten ---------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If .chkKDKreditlimiteUeberschritten.Checked Then
					' Alle Kunden die eine Kreditlimite haben und diese überschritten ist.
					Dim data = m_ListingDatabaseAccess.LoadCustomerDataForCreditlimits(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserFiliale)

					If Not data Is Nothing AndAlso data.Count > 0 Then
						sSql += String.Format("{0} RE.KDNR In (", strAndString)
						For Each customer In data
							sSql += String.Format("{0},", customer.CustomerNumber)
						Next
						sSql = sSql.Remove(sSql.Length - 1, 1)
						sSql += ") "
						'FilterBez = String.Format("Kreditlimite überschritten {0}", vbLf)
						ClsDataDetail.GetFilterBezArray.Add(String.Format("Kreditlimite überschritten {0}", vbLf))

					End If
					' Alle Kunden die eine Kreditlimite haben und diese überschritten ist.
					'Dim connection As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
					'Dim sqlKreditlimite As String = "[List KDNR Kreditlimite ueberschritten]"
					'Dim cmd As SqlCommand = New SqlCommand(sqlKreditlimite, connection)
					'cmd.CommandType = CommandType.StoredProcedure
					'cmd.Parameters.AddWithValue("@filiale", _ClsProgSetting.GetUSFiliale)

					'connection.Open()

					'Dim reader As SqlDataReader = cmd.ExecuteReader()
					'If reader.HasRows Then
					'	sSql += String.Format("{0} RE.KDNR In (", strAndString)
					'	While reader.Read
					'		sSql += String.Format("{0},", reader("Kdnr"))
					'	End While
					'	sSql = sSql.Remove(sSql.Length - 1, 1)
					'	sSql += ") "
					'	'FilterBez = String.Format("Kreditlimite überschritten {0}", vbLf)
					'	ClsDataDetail.GetFilterBezArray.Add(String.Format("Kreditlimite überschritten {0}", vbLf))

					'End If

					'connection.Close()
				End If


				' Filialen Teilung... ----------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				strFieldName = "KD.KDFiliale"
				If strUSFiliale <> "" Then
					'FilterBez += "Filialteilung wie (" & sZusatzBez & ")" & vbLf
					ClsDataDetail.GetFilterBezArray.Add("Filialteilung wie (" & sZusatzBez & ")" & vbLf)
					sSql += strAndString & strFieldName & " Like '%" & strUSFiliale & "%' "
				End If

				' Filterbedingung allgemein hinzufügen -----------------------------------------------------------------------
				' inkl. MwSt ausser für MwSt-freie Liste
				If .Cbo_ListingArt.Text.StartsWith("5") Then
					'FilterBez += String.Format("Alle Beträge sind ohne MwSt. {0}", vbLf)
					ClsDataDetail.GetFilterBezArray.Add(String.Format("Alle Beträge sind ohne MwSt. {0}", vbLf))
				Else
					'FilterBez += String.Format("Alle Beträge sind inkl. MwSt. {0}", vbLf)
					ClsDataDetail.GetFilterBezArray.Add(String.Format("Alle Beträge sind inkl. MwSt. {0}", vbLf))
				End If

				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql &= String.Format("{0}RE.BetragInk <> 0 ", strAndString)

				If m_InitializationData.MDData.MultiMD = 1 Then
					strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
					sSql &= String.Format("{0}RE.MDNr = {1}", strAndString, m_InitializationData.MDData.MDNr)
				End If


			End With

			'ClsDataDetail.GetFilterBez = FilterBez
		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetQuerySQLString", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		m_Logger.LogInfo(String.Format("OPSearchWhereQuery: {0}", sSql))


		Return sSql
	End Function

	Function GetSortString() As String
		Dim strSort As String = ""
		Try
			Dim strSortBez As String = String.Empty
			Dim strName As String()
			Dim strMyName As String = String.Empty


			'0 - Rechnungsnummer
			'1 - Kundennummer
			'2 - Rechnungsempfänger
			'3 - Valutadatum (Aufsteigend)
			'4 - Valutadatum (Absteigend)
			'5 - Totalbetrag (Aufsteigend)
			'6 - Totalbetrag (Absteigend)
			'7 - Rechnungsempfänger + Rechnungsnummer (Absteigend)
			'8 - Rechnungsempfänger + Rechnungsnummer (Aufsteigend)


			strName = Regex.Split(CboSort.Text.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1

				Select Case CInt(Val(strName(i).ToString))
					Case 0      ' Nach Rechnungsnummer
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "RENr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rechnungsnummer"

					Case 1      ' Kundennummer
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "KDNr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundennummer"

					Case 2      ' Rechnungsempfänger
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "R_Name1 ASC, R_Ort ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rechnungsempfänger, Ort"

					Case 3      ' Valuatdatum (Aufsteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "Fak_Dat ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Valutadatum (Aufsteigend)"

					Case 4      ' Valuatdatum (Absteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "Fak_Dat DESC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Valuatdatum (Absteigend)"

					Case 5      ' Totalbetrag (Aufsteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "BetragInk ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Totalbetrag (Aufsteigend)"

					Case 6      ' Totalbetrag (Aufsteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "BetragInk Desc"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Totalbetrag (Absteigend)"

					Case 7      ' Rechnungsempfänger + Rechnungsnummer (Absteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "R_Name1 ASC, RENr DESC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rechnungsempfänger, Rechnungsnummer (Absteigend)"
					Case 8      ' Rechnungsempfänger + Rechnungsnummer (Aufsteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "R_Name1 ASC, RENr ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rechnungsempfänger, Rechnungsnummer (Aufsteigend)"

					Case Else      ' Rechnungsdatum
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "RENr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rechnungsnummer"


				End Select
			Next


			If strMyName.Length > 0 Then
				strSort = String.Format(" Order by {0}", strMyName)
				ClsDataDetail.GetSortBez = strSortBez
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetSortString", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		Return strSort
	End Function

End Class
