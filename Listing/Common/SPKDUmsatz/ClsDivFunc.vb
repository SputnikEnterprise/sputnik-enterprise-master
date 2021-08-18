
Imports System.Text.RegularExpressions
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPKDUmsatz.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

#Region "Diverses"

	'// Get4What._strModul4What
	Dim _strModul4What As String
	Public Property Get4What() As String
		Get
			Return _strModul4What
		End Get
		Set(ByVal value As String)
			_strModul4What = value
		End Set
	End Property


	'// Query.GetSearchQuery
	Dim _strQuery As String
	Public Property GetSearchQuery() As String
		Get
			Return _strQuery
		End Get
		Set(ByVal value As String)
			_strQuery = value
		End Set
	End Property

	'// LargerLV
	Dim _bLargerLV As Boolean
	Public Property GetLargerLV() As Boolean
		Get
			Return _bLargerLV
		End Get
		Set(ByVal value As Boolean)
			_bLargerLV = value
		End Set
	End Property


#End Region

	'#Region "Funktionen für LvClick in der Suchmaske..."

	'// OPNr
	Dim _strOPNr As String
	Public Property GetOPNr() As String
		Get
			Return _strOPNr
		End Get
		Set(ByVal value As String)
			_strOPNr = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strKDNr As String
	Public Property GetKDNr() As String
		Get
			Return _strKDNr
		End Get
		Set(ByVal value As String)
			_strKDNr = value
		End Set
	End Property

	'// Firmenname
	Dim _strKDName As String
	Public Property GetKDName() As String
		Get
			Return _strKDName
		End Get
		Set(ByVal value As String)
			_strKDName = value
		End Set
	End Property

End Class

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant


	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim m_utility As New Utilities
		Dim result As List(Of MandantenData) = Nothing
		m_md = New Mandant

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim advisorData As New MandantenData

					advisorData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					advisorData.MDName = m_utility.SafeGetString(reader, "MDName")
					advisorData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					advisorData.MDConnStr = m_md.GetSelectedMDData(advisorData.MDNr).MDDbConn

					result.Add(advisorData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function


#Region "Funktionen zur Suche nach Daten..."

	Function GetStartGroupedSQLString(ByVal frmSource As frmKDUmsatz, ByVal b4SecYear As Boolean) As String
		Dim sql As String = String.Empty

		With frmSource
			'sql &= "Begin Try Drop Table _KDUmsatz_{0} End Try Begin Catch End Catch; "
			sql &= "Select Re.KDNr, "
			sql &= "Sum(Re.BetragOhne) As tBetragOhne, Sum(Re.BetragEx) As tBetragEx, Sum(Re.MwSt1) As tMwSt1, "
			sql &= "Sum(Re.BetragInk) As tBetragInk "

			sql &= "From RE "
			sql &= "Left Join Kunden On Re.KDNr = Kunden.KDNr "
			sql &= "Where RE.Art In ('A', 'I', 'G', 'F', 'R') "
			sql &= "And RE.BetragInk <> 0 "

			sql = String.Format(sql, m_InitialData.UserData.UserNr)

			Dim strQueryString As String = Trim(GetQuerySQLString(frmSource, b4SecYear))
			sql &= CStr(IIf(strQueryString <> String.Empty, "And ", String.Empty)) & strQueryString & " "
			sql &= "Group By Re.KDNr "  ' Order By Re.KDNr; "

		End With

		Return sql
	End Function

	Function GetKDGroupString4Listing() As String
		Dim Sql As String = String.Empty


		'Sql = "Begin Try Drop Table _KDUmsatz_{0} End Try Begin Catch End Catch; "
		Sql &= "Select KDUms.KDNr, "

		Sql &= "KDUms.FBetragOhne As fBetragOhne, KDUms.fBetragEx As fBetragEx, "
		Sql &= "KDUms.fBetragMwSt As fBetragMwSt, "
		Sql &= "KDUms.fBetragInk As fBetragInk, "

		Sql &= "KDUms.sBetragOhne As sBetragOhne, KDUms.sBetragEx As sBetragEx, "
		Sql &= "KDUms.sBetragMwSt As sBetragMwSt, "
		Sql &= "KDUms.sBetragInk As sBetragInk, "

		Sql &= "Kunden.Firma1 As R_Name1, Kunden.Strasse As R_Strasse, Kunden.Land As R_Land, "
		Sql &= "Kunden.PLZ As R_PLZ, Kunden.Ort As R_Ort, '' As REKst1, '' As REKst2, '' As Kst "

		Sql &= "Into _KDUmsatz_{0} "
		Sql &= "From KDRPUmsatz KDUms Left Join Kunden On KDUms.KDNr = Kunden.KDNr "
		Sql &= "Where KDUms.USNr = {0} "

		Sql = String.Format(Sql, m_InitialData.UserData.UserNr)


		Return Sql
	End Function

	Function GetStartEinzelnSQLString(ByVal frmSource As frmKDUmsatz) As String
		Dim sql As String = String.Empty

		With frmSource
			sql = "Begin Try Drop Table _KDUmsatz_{0} End Try Begin Catch End Catch; "
			sql &= "Select RE.RENr, RE.KDNr, RE.R_Name1, RE.BetragOhne As fBetragOhne, "
			sql &= "RE.BetragEx As fBetragEx, RE.MwSt1 As fBetragMwSt, RE.BetragInk As fBetragInk, "
			sql &= "RE.R_Strasse, RE.R_PLZ, RE.R_Ort, RE.R_Land, RE.Fak_Dat, RE.KST, "
			sql &= "RE.REKST1, RE.REKST2, 0 As sBetragOhne, 0 As sBetragEx, 0 As sBetragMwSt, 0 As sBetragInk "

			sql &= "Into _KDUmsatz_{0} "
			sql &= "From RE Left Join Kunden On RE.KDNr = Kunden.KDNr "
			sql &= "Where RE.Art In ('A', 'I', 'G', 'F', 'R') "
			sql &= "And RE.BetragInk <> 0 "

			sql = String.Format(sql, m_InitialData.UserData.UserNr)

			Dim strQueryString As String = Trim(GetQuerySQLString(frmSource, False))
			sql += CStr(IIf(strQueryString <> String.Empty, "And ", String.Empty)) & strQueryString
		End With

		Return sql
	End Function

	Function GetQuerySQLString(ByVal frmSource As frmKDUmsatz,
														 ByVal b4SecYear As Boolean) As String
		Dim sSql As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = m_InitialData.UserData.UserFiliale
		Dim strName As String()
		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		With frmSource
			If Val(.txtKDNr_1.Text) + Val(.txtKDNr_2.Text) = 0 Then
			ElseIf .txtKDNr_1.Text.Contains("*") Or .txtKDNr_1.Text.Contains("%") Then
				FilterBez = "Kundennummer wie (" & .txtKDNr_1.Text & ") " & vbLf
				sSql += " RE.KDNr Like " & Replace(.txtKDNr_1.Text, "*", "%")

			ElseIf InStr(.txtKDNr_1.Text, ",") > 0 Then
				FilterBez += "Kundennummer wie (" & .txtKDNr_1.Text & ") " & vbLf
				sSql += " RE.KDNr In (" & .txtKDNr_1.Text & ")"

			ElseIf .txtKDNr_1.Text = .txtKDNr_2.Text And .txtKDNr_1.Text <> String.Empty Then
				FilterBez += "Kundennummer = " & .txtKDNr_1.Text & " " & vbLf
				sSql += " RE.KDNr = " & CInt(.txtKDNr_1.Text)

			ElseIf .txtKDNr_1.Text <> "" And .txtKDNr_2.Text = "" Then
				FilterBez += "Kundennummer ab " & .txtKDNr_1.Text & " " & vbLf
				sSql += " RE.KDNr >= " & CInt(.txtKDNr_1.Text)

			ElseIf .txtKDNr_1.Text = "" And .txtKDNr_2.Text <> "" Then
				FilterBez += "Kundennummer bis " & .txtKDNr_2.Text & " " & vbLf
				sSql += " RE.KDNr <= " & CInt(.txtKDNr_2.Text)

			Else
				FilterBez += "Kundennummer zwischen " & .txtKDNr_1.Text & " und " &
												.txtKDNr_2.Text & " " & vbLf
				sSql += " RE.KDNr Between " & CInt(.txtKDNr_1.Text) &
												" And " & CInt(.txtKDNr_2.Text)
			End If

			'' Rechnungsempfänger -------------------------------------------------------------------------------------------------------
			'strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			'If .txtKDName_1.Text & .txtKDName_2.Text = String.Empty Then
			'ElseIf .txtKDName_1.Text.Contains("*") Or .txtKDName_1.Text.Contains("%") Then
			'	FilterBez += "Kundenname wie (" & .txtKDName_1.Text & ") " & vbLf
			'	sSql += String.Format("{0}RE.R_Name1 Like " & Replace(.txtKDName_1.Text, "*", "%"), strAndString)

			'ElseIf InStr(.txtKDName_1.Text, ",") > 0 Then
			'	FilterBez += "Kundenname wie (" & .txtKDName_1.Text & ") " & vbLf
			'	sZusatzBez = .txtKDName_1.Text
			'	sSql += String.Format("{0}RE.R_Name1 In ('" & Replace(sZusatzBez, ",", "','") & "')", strAndString)

			'ElseIf UCase(.txtKDName_1.Text) = UCase(.txtKDName_2.Text) And .txtKDName_1.Text <> String.Empty Then
			'	FilterBez += "Kundenname = " & .txtKDName_2.Text & vbLf
			'	sSql += String.Format("{0}RE.R_Name1 like '" & .txtKDName_1.Text & "'", strAndString)

			'ElseIf .txtKDName_1.Text <> "" And .txtKDName_2.Text = "" Then
			'	FilterBez += "Kundenname ab " & .txtKDName_1.Text & vbLf
			'	sSql += String.Format("{0}RE.R_Name1 >= '" & .txtKDName_1.Text & "'", strAndString)

			'ElseIf .txtKDName_1.Text = "" And .txtKDName_2.Text <> "" Then
			'	FilterBez += "Kundenname bis " & .txtKDName_2.Text & vbLf
			'	sSql += String.Format("{0}RE.R_Name1 <= '" & .txtKDName_2.Text & "'", strAndString)

			'Else
			'	FilterBez += "Kundenname zwischen " & .txtKDName_1.Text & " und " &
			'							.txtKDName_2.Text & vbLf
			'	sSql += String.Format("{0}RE.R_Name1 Between '" & .txtKDName_1.Text & "' And '" & .txtKDName_2.Text & "'", strAndString)
			'End If

			'' Firma1 -------------------------------------------------------------------------------------------------------
			'strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			'If .txtKDName_1.Text & .txtKDName_2.Text = String.Empty Then
			'ElseIf .txtKDName_1.Text.Contains("*") Or .txtKDName_1.Text.Contains("%") Then
			'	FilterBez += String.Format("Kundenname wie ({1}){0}", vbNewLine, .txtKDName_1.Text)
			'	sSql += String.Format("{0}Kunden.Firma1 Like " & Replace(.txtKDName_1.Text, "*", "%"), strAndString)

			'ElseIf InStr(.txtKDName_1.Text, ",") > 0 Then
			'	FilterBez += String.Format("Kundenname wie ({1}){0}", vbNewLine, .txtKDName_1.Text)
			'	sZusatzBez = .txtKDName_1.Text
			'	sSql += String.Format("{0}Kunden.Firma1 In ('" & Replace(sZusatzBez, ",", "','") & "')", strAndString)

			'ElseIf UCase(.txtKDName_1.Text) = UCase(.txtKDName_2.Text) And .txtKDName_1.Text <> String.Empty Then
			'	FilterBez += String.Format("Kundenname = {1}{0}", vbNewLine, .txtKDName_2.Text)
			'	sSql += String.Format("{0}Kunden.Firma1 like '" & .txtKDName_1.Text & "'", strAndString)

			'ElseIf .txtKDName_1.Text <> "" And .txtKDName_2.Text = "" Then
			'	FilterBez += String.Format("Kundenname ab {1}{0}", vbNewLine, .txtKDName_1.Text)
			'	sSql += String.Format("{0}Kunden.Firma1 >= '" & .txtKDName_1.Text & "'", strAndString)

			'ElseIf .txtKDName_1.Text = "" And .txtKDName_2.Text <> "" Then
			'	FilterBez += String.Format("Kundenname bis {1}{0}", vbNewLine, .txtKDName_2.Text)
			'	sSql += String.Format("{0}Kunden.Firma1 <= '" & .txtKDName_2.Text & "'", strAndString)

			'Else
			'	FilterBez += String.Format("Kundenname zwischen {1} und {2}{0}", vbNewLine, .txtKDName_1.Text, .txtKDName_2.Text)
			'	sSql += String.Format("{0}Kunden.Firma1 Between '" & .txtKDName_1.Text & "' And '" & .txtKDName_2.Text & "'", strAndString)
			'End If

			' PLZ -------------------------------------------------------------------------------------------------------
			sZusatzBez = .txtKDPLZ_1.Text.Replace("'", "''").Replace("*", "%").Trim
			' Suche nach einer oder mehrere PLZ
			If sZusatzBez <> String.Empty Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If sZusatzBez.IndexOf(CChar("-")) > 0 Then
					Dim plzVon As String = sZusatzBez.Split(CChar("-"))(0)
					Dim plzBis As String = sZusatzBez.Split(CChar("-"))(1)
					sSql += String.Format("{0}RE.R_PLZ Between '{1}' And '{2}'", strAndString, plzVon, plzBis)
					FilterBez += String.Format("Kunden mit PLZ zwischen {0} und {1}{2}", plzVon, plzBis, vbLf)
				Else
					FilterBez += String.Format(("Kunden mit PLZ ({0}){1}"), sZusatzBez, vbLf)
					sSql += String.Format("{0}(", strAndString)
					Dim strOrString As String = ""
					For Each plz As String In sZusatzBez.Split(CChar(","))
						sSql += String.Format("{0}RE.R_PLZ like '{1}'", strOrString, plz.Trim)
						strOrString = " Or "
					Next
					sSql += ")"
				End If
			End If

			' Ort -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtKDOrt_1.Text) <> String.Empty Then
				sZusatzBez = .txtKDOrt_1.Text.Trim
				sZusatzBez = Replace(sZusatzBez, ", ", ",").Replace("*", "%").Trim
				FilterBez += "Ort wie (" & sZusatzBez & ") " & vbLf
				If InStr(.txtKDOrt_1.Text, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
					sSql += String.Format("{0}(RE.R_Ort Like '" & Replace(sZusatzBez, "*", "%") & "')", strAndString)
				Else
					sSql += String.Format("{0}(RE.R_Ort In ('" & sZusatzBez & "'))", strAndString)
				End If
			End If

			' Land -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtKDLand_1.Text) <> String.Empty Then
				sZusatzBez = .txtKDLand_1.Text.Trim
				sZusatzBez = Replace(sZusatzBez, ", ", ",").Replace("*", "%").Trim
				FilterBez += "Land wie (" & sZusatzBez & ") " & vbLf

				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
				End If
				If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
					sSql += String.Format("{0}RE.R_Land Like '" & Replace(sZusatzBez, "*", "%") & "'", strAndString)
				Else
					sSql += String.Format("{0}RE.R_Land In ('" & sZusatzBez & "')", strAndString)
				End If
			End If

			' Kanton -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_KDKanton.Text) <> String.Empty Then
				sZusatzBez = .Cbo_KDKanton.Text.Trim
				sZusatzBez = Replace(sZusatzBez, ", ", ",")
				FilterBez += "Kanton wie (" & sZusatzBez.Trim & ") " & vbLf

				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
				End If
				sSql += strAndString & "RE.R_PLZ In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

			End If

			If Not b4SecYear Then
				' Von / Bis-Monat 1 --------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If .Cbo_VMonth_1.Text & .Cbo_BMonth_1.Text = String.Empty Then
				ElseIf InStr(.Cbo_VMonth_1.Text, ",") > 0 Then
					FilterBez += "Monat wie (" & .Cbo_VMonth_1.Text & ") " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) In (" & .Cbo_VMonth_1.Text & ")"

				ElseIf Trim(.Cbo_VMonth_1.Text) = Trim(.Cbo_BMonth_1.Text) Then
					FilterBez += "Monat = " & .Cbo_VMonth_1.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) = " & CInt(.Cbo_VMonth_1.Text)

				ElseIf .Cbo_VMonth_1.Text <> "" And .Cbo_BMonth_1.Text = "" Then
					FilterBez += "Monat ab " & .Cbo_VMonth_1.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) >= " & CInt(.Cbo_VMonth_1.Text)

				ElseIf .Cbo_VMonth_1.Text = "" And .Cbo_BMonth_1.Text <> "" Then
					FilterBez += "Monat bis " & .Cbo_BMonth_1.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) <= " & CInt(.Cbo_BMonth_1.Text)

				Else
					FilterBez += "Monat zwischen " & .Cbo_VMonth_1.Text & " und " & .Cbo_BMonth_1.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) Between " & CInt(.Cbo_VMonth_1.Text) & " And " & CInt(.Cbo_BMonth_1.Text)
				End If

				' 1. Jahr ------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If UCase(.Cbo_VYear_1.Text) <> String.Empty Then
					sZusatzBez = .Cbo_VYear_1.Text
					FilterBez += "Jahr wie (" & sZusatzBez & ") " & vbLf
					sSql += strAndString & "Year(RE.Fak_Dat) In (" & sZusatzBez & ")"

				End If

			Else

				' Von / Bis-Monat 2 --------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If .Cbo_VMonth_2.Text & .Cbo_BMonth_2.Text = String.Empty Then
				ElseIf InStr(.Cbo_VMonth_2.Text, ",") > 0 Then
					FilterBez += "Monat wie (" & .Cbo_VMonth_2.Text & ") " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) In (" & .Cbo_VMonth_2.Text & ")"

				ElseIf Trim(.Cbo_VMonth_2.Text) = Trim(.Cbo_BMonth_2.Text) Then
					FilterBez += "Monat = " & .Cbo_VMonth_2.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) = " & CInt(.Cbo_VMonth_2.Text)

				ElseIf .Cbo_VMonth_2.Text <> "" And .Cbo_BMonth_2.Text = "" Then
					FilterBez += "Monat ab " & .Cbo_VMonth_2.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) >= " & CInt(.Cbo_VMonth_2.Text)

				ElseIf .Cbo_VMonth_2.Text = "" And .Cbo_BMonth_2.Text <> "" Then
					FilterBez += "Monat bis " & .Cbo_BMonth_2.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) <= " & CInt(.Cbo_BMonth_2.Text)

				Else
					FilterBez += "Monat zwischen " & .Cbo_VMonth_2.Text & " und " & .Cbo_BMonth_2.Text & " " & vbLf
					sSql += strAndString & "Month(RE.Fak_Dat) Between " & CInt(.Cbo_VMonth_2.Text) & " And " & CInt(.Cbo_BMonth_2.Text)
				End If

				' 1. Jahr ------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				If UCase(.Cbo_VYear_2.Text) <> String.Empty Then
					sZusatzBez = .Cbo_VYear_2.Text
					FilterBez += "Jahr wie (" & sZusatzBez & ") " & vbLf
					sSql += strAndString & "Year(RE.Fak_Dat) In (" & sZusatzBez & ")"

				End If

			End If

			' 1. KST -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "RE.REKst1"
			If UCase(.Cbo_REKst1.Text) <> String.Empty Then
				sZusatzBez = .Cbo_REKst1.Text.Trim
				FilterBez += .LblChange_1.Text & " wie (" & sZusatzBez & ") " & vbLf

				If Not .CheckBox1.Checked Then
					If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
						sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
					Else
						strName = Regex.Split(sZusatzBez.Trim, ",")
						strMyName = String.Empty
						For i As Integer = 0 To strName.Length - 1
							strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
						Next
						If strName.Length > 0 Then sZusatzBez = strMyName

						sSql += strAndString & " (" & sZusatzBez & ")"
					End If
				Else
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
				End If

			End If

			' 2. KST -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "RE.REKst2"
			If UCase(.Cbo_REKst2.Text) <> String.Empty Then
				sZusatzBez = .Cbo_REKst2.Text.Trim
				FilterBez += .LblChange_2.Text & " wie (" & sZusatzBez & ") " & vbLf

				If Not .CheckBox2.Checked Then
					If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
						sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
					Else
						strName = Regex.Split(sZusatzBez.Trim, ",")
						strMyName = String.Empty
						For i As Integer = 0 To strName.Length - 1
							strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
						Next
						If strName.Length > 0 Then sZusatzBez = strMyName

						sSql += strAndString & " (" & sZusatzBez & ")"
					End If
				Else
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
				End If

			End If

			' 3. KST -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "RE.Kst"
			If UCase(.Cbo_REKst3.Text) <> String.Empty Then
				FilterBez += String.Format("Debitoren mit Berater = {0}{1}", .Cbo_REKst3.Text, vbLf)
				sZusatzBez = String.Empty
				strMyName = String.Empty
				Dim strKst As String = String.Empty
				If Not String.IsNullOrWhiteSpace(.Cbo_REKst3.Text) Then
					Dim aUSData As String() = .Cbo_REKst3.Text.Split(CChar("("))
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

			If UCase(.Cbo_Filiale.Text) <> String.Empty Then
				Dim strSearchValue As String = .Cbo_Filiale.Text.Trim

				strSearchValue = GetFilialKstData(strSearchValue)
				If strSearchValue <> String.Empty Then
					strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
					strFieldName = "RE.Kst"
					FilterBez += String.Format("Filiale wie {1}: ({2}){0}", vbNewLine, .Cbo_Filiale.Text.Trim, strSearchValue)

					strSearchValue = Replace(strSearchValue, "'", "")
					strName = Regex.Split(strSearchValue.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i) & "'"
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '" & strName(i) & "/%'"
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%/" & strName(i) & "'"
					Next
					If strName.Length > 0 Then strSearchValue = strMyName
					If InStr(strSearchValue, ",") > 0 Then strSearchValue = Replace(strSearchValue, ",", ",")

					sSql += strAndString & " (" & strSearchValue & ")"
				Else
					.Cbo_Filiale.Text = String.Empty
				End If

			End If






			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			' Filialen Teilung...
			If strUSFiliale <> "" Then
				FilterBez += "(Eigene) Filiale wie (" & sZusatzBez & ") " & vbLf
				sSql += strAndString & "Kunden.KDFiliale Like '%" & strUSFiliale & "%' "
			End If

			' Mandantennummer -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			sSql += String.Format("{0}RE.MDNr = {1}", strAndString, m_InitialData.MDData.MDNr)


		End With
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("KDUmsatzWhereQuery: {0}", sSql))

		Return sSql
	End Function

	Function GetSortString(ByVal IsDetail As Boolean, ByVal frmSource As frmKDUmsatz) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Kundennummer
		'1 - Kundenname
		'2 - Kundenbetrag (Aufsteigend)
		'3 - Kundenbetrag (Absteigend)    
		With frmSource
			strName = Regex.Split(.CboSort.Text.Trim, ",")
			strMyName = String.Empty
			If .CboSort.Text.Contains("-") Then
				For i As Integer = 0 To strName.Length - 1

					' Sortierung Reihenfolge...
					Select Case CInt(Val(Left(strName(i).Trim, 1)))
						Case 0      ' Nummer
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundennummer"
							If IsDetail Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.KDNr ASC"
							Else
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.KDNr ASC"
							End If

						Case 1      ' Kundenname
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenname"
							If IsDetail Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.R_Name1 ASC"
							Else
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.R_Name1 ASC"
							End If

						Case 2
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Betrag (Aufsteigend)"
							If IsDetail Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.fBetragInk ASC"
							Else
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.fBetragInk ASC"
							End If

						Case 3
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Betrag (Absteigend)"
							If IsDetail Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.fBetragInk Desc"
							Else
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.fBetragInk Desc"
							End If

						Case Else
							strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenname"
							If IsDetail Then
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.R_Name1 ASC, KDUms.Fak_Dat ASC"
							Else
								strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " KDUms.R_Name1 ASC"
							End If

					End Select
				Next i
			Else
				strMyName = .CboSort.Text
				strSortBez = String.Format("{0}: {1}", ("Benutzerdefiniert..."), strMyName)

			End If

		End With
		If strMyName.Trim = "" Then
			Return ""
		Else
			strSort = strSort & strMyName
			ClsDataDetail.GetSortBez = strSortBez
			Return strSort
		End If
	End Function


#End Region


	Sub GetJobNr4Print(ByVal frmSource As frmKDUmsatz)
		Dim strModul2print As String = String.Empty

		'0 - Alle Rechnungen
		'1 - Offene/Teil-Offene Rechnungen
		'2 - Gebuchte (bezahlte) Rechnungen
		'3 - Gemahnte Rechnungen
		'4 - MwSt.-pflichtige Liste aller Rechnungen
		'5 - MwSt.-freie Liste aller Rechnungen

		With frmSource
			If Val(.CboLstArt.Text) = 0 Then   ' nicht gruppiert...
				strModul2print = "3.3"  ' Druck für Kundenumsatzliste nach Stunden

			Else
				' Gruppiert...
				If .Cbo_VMonth_2.Text & .Cbo_BMonth_2.Text & .Cbo_VYear_2.Text <> String.Empty Then
					strModul2print = "3.3.2"  ' mit 2 Jahres / Monatsvergleich

				Else
					strModul2print = "3.3.1"  ' Nur ein Jahr / Monat

				End If
			End If
		End With
		ClsDataDetail.GetModulToPrint = strModul2print

	End Sub

End Class