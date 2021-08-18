Partial Class frmOPSearch

	'Function GetZEBetrag(ByVal rOPrec As SqlDataReader, ByVal strALLOPNr As String, ByVal frmTest As frmOPSearch) As Double
	'	Dim strOpRENr As String = String.Empty
	'	Dim dTotalPayedZE As Double = 0
	'	Dim sSql As String = String.Empty
	'	Dim iOPNr As Integer = 0
	'	Dim strAndString As String = String.Empty
	'	Dim strFieldName As String = String.Empty
	'	Dim strSQLDateQuery As String = String.Empty

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

	'	Try
	'		With frmTest

	'			' Fakturadatum -----------------------------------------------------------------------------------------------------
	'			strAndString = " And "
	'			strFieldName = "ZE.V_Date"
	'			If .deFakDate_1.Text = String.Empty And .deFakDate_2.Text = String.Empty Then
	'			ElseIf .deFakDate_1.Text = .deFakDate_2.Text Then
	'				sSql += strAndString & strFieldName & " = '" &
	'										Format(CDate(.deFakDate_1.Text), "d") & "'"

	'			ElseIf .deFakDate_1.Text <> "" And .deFakDate_2.Text = "" Then
	'				sSql += strAndString & strFieldName & " >= '" &
	'										Format(CDate(.deFakDate_1.Text), "d") & "'"

	'			ElseIf .deFakDate_1.Text = "" And .deFakDate_2.Text <> "" Then
	'				sSql += strAndString & strFieldName & " <= '" &
	'										Format(CDate(.deFakDate_2.Text), "d") & "'"

	'			Else
	'				sSql += strAndString & strFieldName & " Between '" &
	'										Format(CDate(.deFakDate_1.Text), "d") & "' And '" &
	'										Format(CDate(.deFakDate_2.Text), "d") & "'"
	'			End If


	'			ClsDataDetail.GetFDate4OP = .deFakDate_1.Text
	'			ClsDataDetail.GetSDate4OP = .deFakDate_2.Text

	'			strSQLDateQuery = sSql
	'		End With

	'		Dim cmd_ZE As New System.Data.SqlClient.SqlCommand
	'		Dim rZErec As SqlClient.SqlDataReader

	'		Conn.Open()

	'		Try
	'			While rOPrec.Read
	'				iOPNr = CInt(rOPrec("RENr").ToString)
	'				If iOPNr = 34486 Then
	'					Trace.WriteLine(iOPNr)
	'				End If
	'				sSql = "Select Sum(ZE.Betrag) As TotalBetrag From ZE Where RENr = " & iOPNr
	'				With frmTest
	'					If rOPrec("Art").ToString.ToUpper = "G".ToUpper Then

	'						' Wenn kein Gebucht-Dt vorhanden ist, ...
	'						If IsDBNull(rOPrec("GebuchtAm")) Then
	'							' ist es noch nicht ausgeglichen worden...
	'							' dann ist der Gutschrift nicht erledigt gewesen...
	'							strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'							dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)

	'						Else
	'							' ??? Wenn Faktura-Von-Dt angegeben und kein Faktura-Bis-Dt, so...
	'							If ClsDataDetail.GetFDate4OP <> String.Empty And ClsDataDetail.GetSDate4OP = String.Empty Then

	'								' ??? Wenn Faktura-Von-Dt und -Bis-Dt nicht angegeben, soll nichts passieren. Wird nie der Fall sein.
	'								If .deFakDate_1.Text = String.Empty And .deFakDate_2.Text = String.Empty Then
	'									' ??? Faktura-Von-Dt gleich Faktura-Bis-Dt wird auch nie der Fall sein.
	'								ElseIf .deFakDate_1.Text = .deFakDate_2.Text Then
	'									If CDate(rOPrec("GebuchtAm").ToString) > CDate(.deFakDate_2.Text) Then
	'										' ist es noch nicht ausgeglichen worden...
	'										' dann ist der Gutschrift nicht erledigt gewesen...
	'										strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'										dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)
	'									End If

	'									' Wenn Faktura-Von-Dt angegeben und Faktura-Bis-Dt nicht, so
	'									' wenn das Gebucht-Dt grösser ist als das Faktura-Von-Dt...
	'								ElseIf .deFakDate_1.Text <> "" And .deFakDate_2.Text = "" Then
	'									If CDate(rOPrec("GebuchtAm").ToString) > CDate(.deFakDate_1.Text) Then
	'										' ist es noch nicht ausgeglichen worden...
	'										' dann ist der Gutschrift nicht erledigt gewesen...
	'										strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'										dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)
	'									End If
	'									' ??? Wenn Faktura-Von-Dt nicht angegeben, das Faktura-Bis-Dt aber, dann...wird auch nie der Fall sein.
	'								ElseIf .deFakDate_1.Text = "" And .deFakDate_2.Text <> "" Then
	'									If CDate(rOPrec("GebuchtAm").ToString) < CDate(.deFakDate_2.Text) Then
	'										' ist es noch nicht ausgeglichen worden...
	'										' dann ist der Gutschrift nicht erledigt gewesen...
	'										strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'										dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)
	'									End If

	'								Else
	'									' Wenn das Gebucht-Dt kleiner ist als das Faktura-Bis-Dt und
	'									' das Gebucht-Dt grösser ist als das Faktura-Von-Dt, dann...
	'									If CDate(rOPrec("GebuchtAm").ToString) < CDate(.deFakDate_2.Text) And
	'															CDate(rOPrec("GebuchtAm").ToString) > CDate(.deFakDate_1.Text) Then
	'										' ist es noch nicht ausgeglichen worden...
	'										' dann ist der Gutschrift nicht erledigt gewesen...
	'										strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'										dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)
	'									End If

	'								End If

	'							End If

	'							'ElseIf CDate(rOPrec("GebuchtAm").ToString) > CDate(.txt_FakDat_2.Text) Then
	'							'    ' dann ist der Gutschrift nicht erledigt gewesen...
	'							'    strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'							'    dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)

	'						End If

	'					Else    ' Ist kein Gutschrift...
	'						sSql += strSQLDateQuery

	'						cmd_ZE = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'						rZErec = cmd_ZE.ExecuteReader          ' ZE-Datenbank

	'						rZErec.Read()
	'						If IsDBNull(rZErec("TotalBetrag")) Then
	'							' nicht gefunden! ganz offen...
	'							strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'							dTotalPayedZE += CDbl(rOPrec("BetragInk").ToString)

	'						ElseIf CDbl(rZErec("TotalBetrag").ToString) < CDbl(rOPrec("BetragInk").ToString) Then
	'							' Teilbezahlt...
	'							strOpRENr += CStr(IIf(strOpRENr.Trim = String.Empty, "", ",")) & iOPNr
	'							dTotalPayedZE += (CDbl(rOPrec("BetragInk").ToString) - CDbl(rZErec("TotalBetrag").ToString))

	'						Else
	'							' ganz bezahlt

	'						End If
	'						rZErec.Close()

	'					End If

	'				End With

	'			End While
	'			ClsDataDetail.GetstrOPNr4Date = strOpRENr
	'			ClsDataDetail.GetTotalOpenBetrag4Date = dTotalPayedZE

	'		Catch ex As Exception
	'			MsgBox(ex.StackTrace & vbNewLine & ex.Message, MsgBoxStyle.Critical, "GetZEBetrag_1")

	'		End Try

	'	Catch ex As Exception
	'		MsgBox(ex.StackTrace & vbNewLine & ex.Message, MsgBoxStyle.Critical, "GetZEBetrag_0")

	'	End Try

	'End Function


End Class
