Partial Class frmOPSearch

	Function GetMyQueryString() As Boolean

		Try
			Dim sSql1Query As String = String.Empty
			Dim sSql2Query As String = String.Empty
			Dim strSort As String = String.Empty
			'Dim bSearchOpRE As Boolean
			Dim strArtQuery As String = String.Empty

			Dim _ClsDb As New ClsDbFunc
			If String.IsNullOrWhiteSpace(txt_IndSQLQuery.Text) Then

				' Die Tabellennamen für das Speichern der Tabellen auf der DB
				' 04.02.2010
				ClsDataDetail.SPTabNamenDBL = String.Format("[_DebitorenListe_{0}]", _ClsProgSetting.GetLogedUSGuid)
				ClsDataDetail.SPTabNamenDBLFälligkeiten = String.Format("[_DebitorenListeFaelligkeiten_{0}]", _ClsProgSetting.GetLogedUSGuid)
				ClsDataDetail.SPTabNamenDBLStat = String.Format("[_DebitorenListeBezRechnStat_{0}]", _ClsProgSetting.GetLogedUSGuid)
				ClsDataDetail.SPTabNamenDBLnachFakturadatum = String.Format("[_DebitorenListeNachFakturadatum_{0}]", _ClsProgSetting.GetLogedUSGuid)
				ClsDataDetail.SPTabNamenDBLVK = String.Format("[_DebitorenListeVK_{0}]", _ClsProgSetting.GetLogedUSGuid)
				Dim sSqlSelectPostQuery As String = String.Format(" Select * From {0} RE", ClsDataDetail.SPTabNamenDBL)

				'TODO: ?? Der offene Gesamtbetrag pro Kunde kann möglicherweise doch noch mit Faktura-Datum kombiniert werden.
				If (Me.deFakDate_1.Text.Length > 0 Or Me.deFakDate_2.Text.Length > 0) Then
					'Me.txt_OpenBetrag_1.Text = "0.00"
					'Me.txt_OpenBetrag_2.Text = "0.00"

				ElseIf Val(Me.txt_OpenBetrag_1.Text) + Val(Me.txt_OpenBetrag_2.Text) > 0 Then
					If CShort(Me.Cbo_ListingArt.Text.Trim.Substring(0, 1)) <> 1 Then
						Me.deFakDate_1.Text = String.Empty
						Me.deFakDate_2.Text = String.Empty
					End If

				ElseIf Val(Me.txt_OpenBetrag_1.Text) + Val(Me.txt_OpenBetrag_2.Text) = 0 Then
					If CShort(Me.Cbo_ListingArt.Text.Trim.Substring(0, 1)) = 1 Then
						'Me.txt_FakDat_2.Text = "31.12.3099"
						'Me.txt_FakDat_2.ForeColor = Color.White
					End If

				End If
				' DIESER TEIL WEGLASSEN UND IMMER MIT EINER QUERY ARBEITEN. 12.03.2010------------------------------------------
				' Wenn 1 - Offene/Teil-Offene Rechnungen ausgewählt und ein Faktura-Datum angibt, so SELECT ohne Kunden-Tabelle 


				sSql1Query = GetStartSQLString()    ' 1. String

				' Den SELECT mit oder ohne KUNDEN übergeben, aber es spielt gar keine Rolle. 
				' Die WHERE-Klausel wird unabhängig erstellt.
				sSql2Query = LoadWherQuery(sSql1Query)   ' Where Klausel

				' Die neue Query2 wird NEU immer gefüllt sein, da die ausgeglichene Gutschriften NEU abgefangen werden müssen.
				' Ergo, die vorgehende Query1 wird nun ein Where erhalten.
				If Not String.IsNullOrWhiteSpace(sSql2Query) Then sSql1Query += " Where "

				' Wir haben hier ein SELECT und FROM ohne KUNDEN
				' Andernfalls ein SELECT und FROM mit KUNDEN
				strSort = GetSortString()      ' Sort Klausel
				' In der WHERE-ZUSATZ-Klausel wird je nach Listen-Art eine bestimmte Einschränkung hinzugefügt.
				strArtQuery = GetListArt()

				' Wenn die WHERE-Klause leer und die WHERE-ZUSATZ-Klausel nicht leer ist, so
				' die WHERE-ZUSATZ-Klausel mit dem Where versehen.
				'If Trim(sSql2Query) = String.Empty And strArtQuery <> String.Empty Then strArtQuery = " Where " & strArtQuery
				If String.IsNullOrWhiteSpace(sSql2Query) AndAlso Not String.IsNullOrWhiteSpace(strArtQuery) Then strArtQuery = " Where " & strArtQuery

				' Wenn die WHERE-ZUSATZ-Klausel nicht leer ist, so muss ein And vorangestellt werden.
				If Not String.IsNullOrWhiteSpace(sSql2Query) AndAlso Not String.IsNullOrWhiteSpace(strArtQuery) Then strArtQuery = " And " & strArtQuery

				' (SELECT und FROM mit KUNDEN) + (die unabhängige WHERE-Klausel) + (WHERE-ZUSATZ-Klausel) + (die SORT-Klausel)
				Me.txt_SQLQuery.Text = sSql1Query & sSql2Query & strArtQuery & sSqlSelectPostQuery & strSort
				If strLastSortBez = String.Empty Then strLastSortBez = strSort

				Me.txtAdminQuery.Text = sSqlSelectPostQuery & strSort

				'End If

			Else
				Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
				Me.txtAdminQuery.Text = Me.txt_SQLQuery.Text

			End If
			_ClsDb.GetJobNr4Print(Me)

		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetMyQueryString", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try


		Return True
	End Function

	'Private Sub InsertKreditInfoToDebitorenListe()
	'	Dim conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn) '_ClsProgSetting.GetConnString)
	'	Dim cmdText As String = String.Format("SELECT * FROM {0}", ClsDataDetail.SPTabNamenDBL)
	'	Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
	'	Dim dt As DataTable = New DataTable("Debitorenliste")
	'	Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
	'	Dim clsDbFunc As ClsDbFunc = New ClsDbFunc
	'	da.Fill(dt)
	'	For Each row In dt.Rows
	'		row("KreditInfo1") = GetKDKreditInfoLine(CInt(row("KDNR").ToString))
	'	Next
	'	For Each col As DataColumn In dt.Columns

	'	Next
	'	cmdText = String.Format("UPDATE {0}", ClsDataDetail.SPTabNamenDBL)
	'	cmd.CommandText = cmdText

	'	da.Update(dt)
	'End Sub


	Function GetListArt() As String
		Dim strQuery As String = String.Empty

		Try
			Dim strPrintNr As String = String.Empty
			Dim strKDNot As String = String.Empty
			Dim bezahlteGutschriftenAusblenden As Boolean = True
			Dim offeneGutschriftenAusblenden As Boolean = True
			Dim strAndString As String = ""

			Dim invoiceStichDate As Date?
			Dim invoiceFromDate As Date?
			Dim invoiceToDate As Date?
			If Not deStichtag.EditValue Is Nothing AndAlso IsDate(deStichtag.EditValue) Then invoiceStichDate = DateTime.ParseExact(deStichtag.EditValue, "dd.MM.yyyy", Nothing)
			If invoiceStichDate Is Nothing Then invoiceStichDate = Now.Date

			If Not deFakDate_1.EditValue Is Nothing AndAlso IsDate(deFakDate_1.EditValue) Then invoiceFromDate = DateTime.ParseExact(deFakDate_1.EditValue, "dd.MM.yyyy", Nothing)
			If Not deFakDate_2.EditValue Is Nothing AndAlso IsDate(deFakDate_2.EditValue) Then invoiceToDate = DateTime.ParseExact(deFakDate_2.EditValue, "dd.MM.yyyy", Nothing)

			Dim createdFromDate As Date?
			Dim createdToDate As Date?
			If Not deCreated_1.EditValue Is Nothing AndAlso IsDate(deCreated_1.EditValue) Then createdFromDate = DateTime.ParseExact(deCreated_1.EditValue, "dd.MM.yyyy", Nothing)
			If Not deCreated_2.EditValue Is Nothing AndAlso IsDate(deCreated_2.EditValue) Then createdToDate = DateTime.ParseExact(deCreated_2.EditValue, "dd.MM.yyyy", Nothing)


			'0 - Alle Rechnungen
			'1 - Offene/Teil-Offene Rechnungen
			'2 - Gebuchte (bezahlte) Rechnungen
			'3 - Gemahnte Rechnungen
			'4 - Gemahnte Rechnungen offene Debitoren
			'5 - MwSt.-pflichtige Liste aller Rechnungen
			'6 - MwSt.-freie Liste aller Rechnungen

			Dim listArt As Integer = CInt(Val(Cbo_ListingArt.EditValue))
			Dim invoiceArt As String = lueDebitorenart.EditValue
			Dim openAmountFrom As Decimal? = CDbl(Val(txt_OpenBetrag_1.Text))
			Dim openAmountTo As Decimal? = CDbl(Val(txt_OpenBetrag_2.Text))

			'With frmTest
			Select Case listArt
				Case 0      ' Alle Rechnungen
					'strArtBez = "Liste aller Rechnungen"
					' Nur alle Gutschriften anzeigen, wenn OP-Art leer oder 'G' ist:
					If String.IsNullOrWhiteSpace(invoiceArt) OrElse invoiceArt = "G" Then
						bezahlteGutschriftenAusblenden = False
						offeneGutschriftenAusblenden = False
					End If

				Case 1      ' Offene/Teil-Offene Rechnungen
					' 12.03.2010 Teilzeilungen berücksichtigen bis Faktura-Bis-Datum
					' 05.05.2010 Gutschriften haben keine Zahlungseingänge, somit muss anhand der RE.Bezahlt ausgegangen werden.
					' 05.07.2011 Gutschriften GebuchtAm berücksichtigen, wenn zwischen Fak_Dat und GebuchtAm gesucht wird.
					strQuery = String.Format("(((RE.ART = 'G' Or RE.ART = 'R') And (((Len('{0:d}') = 0 And Round(RE.Bezahlt, 2) <> Round(RE.BetragInk, 2) ) Or ", invoiceStichDate) ' invoiceToDate)
					strQuery += String.Format("  (Len('{0:d}') > 0 And (Convert(Date, RE.GebuchtAm) > Convert(Date,'{0:d}')))) Or RE.GebuchtAm is Null)) Or ", invoiceStichDate) ' invoiceToDate)
					strQuery += "(RE.ART <> 'G' And RE.ART <> 'R' And Round(RE.BetragInk, 2) > Round(IsNull((SELECT Sum(ZE.Betrag) FROM dbo.ZE WHERE ZE.RENR = RE.RENR "

					If m_InitializationData.MDData.MultiMD = 1 Then
						strQuery &= String.Format("And ZE.MDNr = {0} And RE.MDNr = {0} ", m_InitializationData.MDData.MDNr)
					End If

					strQuery += String.Format(" And Convert(Date, ZE.V_Date) <= Convert(Date, '{0:d}')", invoiceStichDate) ' invoiceToDate)
					'If Not createdToDate Is Nothing Then strQuery += String.Format(" And Convert(Date, ZE.V_Date) <= Convert(Date, '{0:d}')", createdToDate)
					strQuery += "), 0), 2) ) )"

					If openAmountFrom.GetValueOrDefault(0) + openAmountTo.GetValueOrDefault(0) > 0 Then
						strKDNot = GetOPBetragBetween(openAmountFrom.GetValueOrDefault(0), openAmountTo.GetValueOrDefault(0))
						If strKDNot <> String.Empty Then
							strQuery += "And KD.KDNr In (" & strKDNot & ")"
						Else
							strQuery += "And KD.KDNr In (0)"
						End If

					End If

					'strArtBez = "Liste aller offenen und teiloffenen Rechnungen"
					If String.IsNullOrWhiteSpace(invoiceArt) OrElse invoiceArt = "G" Then
						offeneGutschriftenAusblenden = False
					End If

				Case 2      ' Gebuchte (bezahlte) Rechnungen
					' 19.03.2010 Bezahlte Rechnungen bis Faktura-Bis-Datum berücksichtigen
					' 05.05.2010 Gutschriften haben keine Zahlungseingänge, somit muss anhand der RE.Bezahlt ausgegangen werden.
					strQuery = "(((RE.ART = 'G' Or RE.ART = 'R') And Round(RE.Bezahlt, 2) = Round(RE.BetragInk, 2) ) Or "
					strQuery += "(RE.ART <> 'G' And RE.ART <> 'R' And Round(RE.BetragInk, 2) <= Round( IsNull((SELECT Sum(ZE.Betrag) FROM dbo.ZE WHERE ZE.RENR = RE.RENR "
					If Not invoiceStichDate Is Nothing Then
						strQuery += String.Format(" And Convert(Date, ZE.V_Date) <= Convert(Date, '{0:d}')", invoiceStichDate) ' invoiceToDate)
					End If
					strQuery += "), 0), 2) ))"

					'strArtBez = "Liste aller bezahlten Rechnungen"

					If String.IsNullOrWhiteSpace(invoiceArt) OrElse invoiceArt = "G" Then
						bezahlteGutschriftenAusblenden = False
					End If

				Case 3      ' Gemahnte Rechnungen
					strQuery = "RE.Ma0 Is Not Null And RE.Art Not In ('G', 'R') "
						'strArtBez = "Liste aller gemahnten Rechnungen"

				Case 4      ' Gemahnte Rechnungen weleche auch offen sind
					strQuery = "RE.Ma0 Is Not Null And "
					strQuery &= "Round(RE.BetragInk, 2) > Round(RE.Bezahlt, 2) And RE.Art Not In ('G', 'R') "

				Case 5      ' MwSt.-pflichtige Liste aller Rechnungen
					strQuery = "RE.BetragEx <> 0 "
					'strArtBez = "MwSt.-pflichtige Liste aller Rechnungen"
					' Nur alle Gutschriften anzeigen, wenn OP-Art leer oder 'G' ist:
					If String.IsNullOrWhiteSpace(invoiceArt) OrElse invoiceArt = "G" Then
						bezahlteGutschriftenAusblenden = False
						offeneGutschriftenAusblenden = False
					End If

				Case 6      ' MwSt.-freie Liste aller Rechnungen
					strQuery = "RE.BetragEx = 0 "
					'strArtBez = "MwSt.-freie Liste aller Rechnungen"
					' Nur alle Gutschriften anzeigen, wenn OP-Art leer oder 'G' ist:
					If String.IsNullOrWhiteSpace(invoiceArt) OrElse invoiceArt = "G" Then
						bezahlteGutschriftenAusblenden = False
						offeneGutschriftenAusblenden = False
					End If

			End Select

			' Ermittlung Datum für die Gutschriften
			'Dim vonDatum As String = .deFakDate_1.Text
			'Dim bisDatum As String = .deFakDate_2.Text

			If bezahlteGutschriftenAusblenden Then
				Dim finishedCredits = m_ListingDatabaseAccess.LoadInvoiceDataForFinishedCredits(m_InitializationData.MDData.MDNr, invoiceStichDate, invoiceFromDate, invoiceToDate, createdFromDate, createdToDate)

				If Not finishedCredits Is Nothing AndAlso finishedCredits.Count > 0 Then
					strAndString = IIf(strQuery <> String.Empty, " And ", String.Empty).ToString

					strQuery += String.Format("{0}RE.RENR Not In (", strAndString)
					Dim invoiceNumber As String = String.Empty
					For Each itm In finishedCredits
						invoiceNumber &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(invoiceNumber), "", ","), itm.RENr)
					Next

					strQuery &= invoiceNumber
					strQuery += ") "
				End If

			End If

			'Dim connection As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			'Dim sqlGutschrift As String = ""
			'Dim cmd As SqlCommand = New SqlCommand(sqlGutschrift, connection)
			'cmd.CommandType = CommandType.StoredProcedure
			'cmd.Parameters.AddWithValue("@vonDatum", invoiceFromDate)
			'cmd.Parameters.AddWithValue("@bisDatum", invoiceToDate)

			'If bezahlteGutschriftenAusblenden Then
			'	' Die ausgeglichene Gutschriften ausblenden.
			'	cmd.CommandText = "[List RENR Gutschrift ausgeglichen]"

			'	connection.Open()

			'	strAndString = IIf(strQuery <> String.Empty, " And ", String.Empty).ToString
			'	Dim reader As SqlDataReader = cmd.ExecuteReader()
			'	If reader.HasRows Then
			'		strQuery += String.Format("{0}RE.RENR Not In (", strAndString)
			'		While reader.Read
			'			strQuery += String.Format("{0},", reader("RENR"))
			'		End While
			'		strQuery = strQuery.Remove(strQuery.Length - 1, 1)
			'		strQuery += ") "
			'	End If

			'	connection.Close()
			'Else

			'End If

			If offeneGutschriftenAusblenden Then
				' Die offene Gutschrifen ausblenden
				Dim finishedCredits = m_ListingDatabaseAccess.LoadInvoiceDataForNOTFinishedCredits(m_InitializationData.MDData.MDNr, invoiceStichDate, invoiceFromDate, invoiceToDate, createdFromDate, createdToDate)

				If Not finishedCredits Is Nothing AndAlso finishedCredits.Count > 0 Then
					strAndString = IIf(strQuery <> String.Empty, " And ", String.Empty).ToString

					strQuery += String.Format("{0}RE.RENR Not In (", strAndString)
					Dim invoiceNumber As String = String.Empty
					For Each itm In finishedCredits
						invoiceNumber &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(invoiceNumber), "", ","), itm.RENr)
					Next

					strQuery &= invoiceNumber
					strQuery += ") "
				End If



				'	cmd.CommandText = "[List RENR Gutschrift nicht ausgeglichen]"

				'	connection.Open()

				'	strAndString = IIf(strQuery <> String.Empty, " And ", String.Empty).ToString
				'	Dim reader As SqlDataReader = cmd.ExecuteReader()
				'	If reader.HasRows Then
				'		strQuery += String.Format("{0}RE.RENR Not In (", strAndString)
				'		While reader.Read
				'			strQuery += String.Format("{0},", reader("RENR"))
				'		End While
				'		strQuery = strQuery.Remove(strQuery.Length - 1, 1)
				'		strQuery += ") "
				'	End If

				'	connection.Close()
				'Else

			End If

			'End With


		Catch ex As Exception
			MessageBox.Show(ex.ToString, "GetListArt", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try


		Return strQuery
	End Function

	Function GetOPBetragBetween(ByVal dFBetrag As Decimal?, ByVal dLBetrag As Decimal?) As String
		Dim strKDNr As String = String.Empty
		'Dim sSql As String = String.Empty
		Dim strWhereQuery As String = String.Empty

		'Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			'Dim cmd_ZE As New System.Data.SqlClient.SqlCommand

			'Conn.Open()

			strWhereQuery = LoadWherQuery(String.Empty)
			Dim data = m_ListingDatabaseAccess.LoadInvoiceOpenAmountData(m_InitializationData.MDData.MDNr, strWhereQuery, dFBetrag.GetValueOrDefault(0), dLBetrag.GetValueOrDefault(0))

			'sSql = "Select RE.KDNr, Sum(Round(BetragInk, 2) - Round(Bezahlt, 2)) As TotalOffen From RE "
			'If strWhereQuery.Length > 0 Then
			'	sSql += String.Format("Where {0}", strWhereQuery)
			'End If
			'sSql += " Group By RE.KDNR HAVING Sum(Round(BetragInk, 2) - Round(Bezahlt, 2)) "
			'If dFBetrag > 0 And dLBetrag > 0 Then
			'	sSql += " Between " & dFBetrag & " And " & dLBetrag
			'ElseIf dFBetrag > 0 Then
			'	sSql += String.Format(" > {0}", dFBetrag)
			'Else
			'	sSql += String.Format(" < {0}", dLBetrag)
			'End If
			'sSql += " Order By RE.KDNr"

			'cmd_ZE = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			'Dim rOPrec As SqlDataReader = cmd_ZE.ExecuteReader
			'Dim strTestNr As String = String.Empty

			For Each itm In data
				If itm.CustomerNumber > 0 Then
					strKDNr += If(String.IsNullOrWhiteSpace(strKDNr), "", ",") & itm.CustomerNumber
				End If
			Next

			ClsDataDetail.GetFilterBezArray.Add("Offener Betrag zwischen " & dFBetrag & " und " & dLBetrag & vbLf)
			ClsDataDetail.GetstrKDNr4Date_2 = strKDNr


		Catch ex As Exception
			MsgBox(ex.Message)
			MessageBox.Show(ex.Message, "GetOPBetragBetween", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		Return strKDNr
	End Function

	'Function GetKDKreditInfoLine(ByVal iKDNr As Integer) As String
	'	Dim strResult As String = String.Empty
	'	Dim sSql As String = String.Empty
	'	Dim strWhereQuery As String = String.Empty
	'	'Dim FilterBez As String = String.Empty
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

	'	sSql = "Select KD_KreditInfo.Ab_Date, KD_KreditInfo.Beschreibung, "
	'	sSql += "KD_KreditInfo.CreatedOn, KD_KreditInfo.CreatedFrom From "
	'	sSql += "KD_KreditInfo Where KD_KreditInfo.KDNr = @KDNr And KD_KreditInfo.ActiveRec = 1"

	'	Try

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		Dim rKDKreditrec As SqlClient.SqlDataReader

	'		Try
	'			Conn.Open()

	'		Catch ex As Exception
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetKDKreditInfoLine_0")

	'		End Try

	'		param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)

	'		rKDKreditrec = cmd.ExecuteReader          ' Kreditinfo Datenbank von Kunden
	'		While rKDKreditrec.Read()
	'			If Not IsDBNull(rKDKreditrec("Ab_Date")) Then
	'				strResult += String.Format("{0}: {1}", Format(rKDKreditrec("Ab_Date"), "d"), rKDKreditrec("Beschreibung").ToString)
	'			End If

	'		End While

	'	Catch ex As Exception
	'		MsgBox(ex.Message, MsgBoxStyle.Critical, "GetKDKreditInfoLine")

	'	End Try

	'	Conn.Close()
	'	Return strResult
	'End Function


End Class
