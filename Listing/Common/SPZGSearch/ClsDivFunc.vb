
Imports System.Text.RegularExpressions
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

	Private m_md As Mandant
	Private m_utilities As Utilities


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

#Region "Funktionen für LvClick in der Suchmaske..."

	'// ZGNr
	Dim _strZGNr As String
	Public Property GetZGNr() As String
		Get
			Return _strZGNr
		End Get
		Set(ByVal value As String)
			_strZGNr = value
		End Set
	End Property

	'// MANr
	Dim _strMANr As String
	Public Property GetMANr() As String
		Get
			Return _strMANr
		End Get
		Set(ByVal value As String)
			_strMANr = value
		End Set
	End Property

#End Region

#Region "LL_Properties"
	'// Print.LLDocName
	Dim _LLDocName As String
	Public Property LLDocName() As String
		Get
			Return _LLDocName
		End Get
		Set(ByVal value As String)
			_LLDocName = value
		End Set
	End Property

	'// Print.LLDocLabel
	Dim _LLDocLabel As String
	Public Property LLDocLabel() As String
		Get
			Return _LLDocLabel
		End Get
		Set(ByVal value As String)
			_LLDocLabel = value
		End Set
	End Property

	'// Print.LLFontDesent
	Dim _LLFontDesent As Integer
	Public Property LLFontDesent() As Integer
		Get
			Return _LLFontDesent
		End Get
		Set(ByVal value As Integer)
			_LLFontDesent = value
		End Set
	End Property

	'// Print.LLIncPrv
	Dim _LLIncPrv As Integer
	Public Property LLIncPrv() As Integer
		Get
			Return _LLIncPrv
		End Get
		Set(ByVal value As Integer)
			_LLIncPrv = value
		End Set
	End Property

	'// Print.LLParamCheck
	Dim _LLParamCheck As Integer
	Public Property LLParamCheck() As Integer
		Get
			Return _LLParamCheck
		End Get
		Set(ByVal value As Integer)
			_LLParamCheck = value
		End Set
	End Property

	'// Print.LLKonvertName
	Dim _LLKonvertName As Integer
	Public Property LLKonvertName() As Integer
		Get
			Return _LLKonvertName
		End Get
		Set(ByVal value As Integer)
			_LLKonvertName = value
		End Set
	End Property

	'// Print.LLZoomProz
	Dim _LLZoomProz As Integer
	Public Property LLZoomProz() As Integer
		Get
			Return _LLZoomProz
		End Get
		Set(ByVal value As Integer)
			_LLZoomProz = value
		End Set
	End Property

	'// Print.LLCopyCount
	Dim _LLCopyCount As Integer
	Public Property LLCopyCount() As Integer
		Get
			Return _LLCopyCount
		End Get
		Set(ByVal value As Integer)
			_LLCopyCount = value
		End Set
	End Property

	'// Print.LLExportedFilePath
	Dim _LLExportedFilePath As String
	Public Property LLExportedFilePath() As String
		Get
			Return _LLExportedFilePath
		End Get
		Set(ByVal value As String)
			_LLExportedFilePath = value
		End Set
	End Property

	'// Print.LLExportedFileName
	Dim _LLExportedFileName As String
	Public Property LLExportedFileName() As String
		Get
			Return _LLExportedFileName
		End Get
		Set(ByVal value As String)
			_LLExportedFileName = value
		End Set
	End Property

	'// Print.LLExportfilter
	Dim _LLExportfilter As String
	Public Property LLExportfilter() As String
		Get
			Return _LLExportfilter
		End Get
		Set(ByVal value As String)
			_LLExportfilter = value
		End Set
	End Property

	'// Print.LLExporterName
	Dim _LLExporterName As String
	Public Property LLExporterName() As String
		Get
			Return _LLExporterName
		End Get
		Set(ByVal value As String)
			_LLExporterName = value
		End Set
	End Property

	'// Print.LLExporterFileName
	Dim _LLExporterFileName As String
	Public Property LLExporterFileName() As String
		Get
			Return _LLExporterFileName
		End Get
		Set(ByVal value As String)
			_LLExporterFileName = value
		End Set
	End Property

#End Region

#Region "US Setting"

	Public Property USeMail() As String
	Public Property USTelefon() As String
	Public Property USTelefax() As String
	Public Property USVorname() As String
	Public Property USAnrede() As String
	Public Property USNachname() As String
	Public Property USMDname() As String
	Public Property USMDname2() As String
	Public Property USMDname3() As String
	Public Property USMDPostfach() As String
	Public Property USMDStrasse() As String
	Public Property USMDOrt() As String
	Public Property USMDPlz() As String
	Public Property USMDLand() As String
	Public Property USMDTelefax() As String
	Public Property USMDeMail() As String
	Public Property USMDHomepage() As String

#End Region


#Region "Funktionen..."
	Dim _BetragSign As Boolean
	Public Property GetBetragSign() As Boolean
		Get
			Return _BetragSign
		End Get

		Set(value As Boolean)
			Dim bResult As Boolean
			'Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath
			'm_md = New Mandant
			m_utilities = New Utilities

			'Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim strQuery As String = "//SPSZGSearch/ZGSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/BetragSign"

			Dim strBez As String = m_utilities.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDSQLDataXMLFilename(ClsDataDetail.m_InitialData.MDData.MDNr), strQuery, "")
			'Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsSystem.GetSQLDataFile(), strQuery)
			If strBez <> String.Empty Then
				If strBez = CStr(1) Then bResult = True
			End If
			ClsDataDetail.ShowBetragAsPositiv = bResult
			_BetragSign = bResult

		End Set

	End Property

	'Dim bResult As Boolean
	'Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath
	'Dim _ClsReg As New SPProgUtility.ClsDivReg
	'Dim strQuery As String = "//SPSZGSearch/ZGSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/BetragSign"

	'Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsSystem.GetSQLDataFile(), strQuery)
	'  If strBez <> String.Empty Then
	'    If strBez = CStr(1) Then bResult = True
	'  End If
	'  ClsDataDetail.ShowBetragAsPositiv = bResult

	'End Sub

#End Region

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

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_utility.SafeGetString(reader, "MDName")
					recData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_md.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

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

	Function GetStartSQLString() As String
		Dim sSql As String = String.Empty

		sSql = "Select ZG.*, "
		sSql += "Mitarbeiter.Nachname, Mitarbeiter.Vorname From ZG "
		sSql += "Left Join Mitarbeiter On ZG.MANr = Mitarbeiter.MANr "

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal frmTest As frmZGSearch) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery
		Dim strFieldName As String = String.Empty
		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = ClsDataDetail.m_InitialData.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)

		Dim strName As String()
		Dim strMyName As String = String.Empty

		With frmTest
			If .txtZGNr_1.Text = String.Empty And .txtZGNr_2.Text = String.Empty Then
			ElseIf .txtZGNr_1.Text.Contains("*") Or .txtZGNr_1.Text.Contains("%") Then
				FilterBez = "Auszahlungsnummer wie (" & .txtZGNr_1.Text & ") " & vbLf
				sSql += " ZG.ZGNr Like " & Replace(.txtZGNr_1.Text, "*", "%")

			ElseIf InStr(.txtZGNr_1.Text, ",") > 0 Then
				FilterBez = "Auszahlungsnummer wie (" & .txtZGNr_1.Text & ") " & vbLf
				sSql += " ZG.ZGNr In (" & .txtZGNr_1.Text & ")"

			ElseIf .txtZGNr_1.Text = .txtZGNr_2.Text Then
				FilterBez = "Auszahlungsnummer = " & .txtZGNr_1.Text & " " & vbLf
				sSql += " ZG.ZGNr = " & CInt(.txtZGNr_1.Text)

			ElseIf .txtZGNr_1.Text <> "" And .txtZGNr_2.Text = "" Then
				FilterBez = "Auszahlungsnummer ab " & .txtZGNr_1.Text & " " & vbLf
				sSql += " ZG.ZGNr >= " & CInt(.txtZGNr_1.Text)

			ElseIf .txtZGNr_1.Text = "" And .txtZGNr_2.Text <> "" Then
				FilterBez = "Auszahlungsnummer bis " & .txtZGNr_2.Text & " " & vbLf
				sSql += " ZG.ZGNr <= " & CInt(.txtZGNr_2.Text)

			Else
				FilterBez = "Auszahlungsnummer zwischen " & .txtZGNr_1.Text & " und " &
												.txtZGNr_2.Text & " " & vbLf
				sSql += " ZG.ZGNr Between " & CInt(.txtZGNr_1.Text) &
												" And " & CInt(.txtZGNr_2.Text)
			End If

			' MANr -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .txtMANr_1.Text = "" And .txtMANr_2.Text = "" Then
			ElseIf .txtMANr_1.Text.Contains("*") Or .txtMANr_1.Text.Contains("%") Then
				FilterBez += "Kandidatennummer wie (" & .txtMANr_1.Text & ") " & vbLf
				sSql += strAndString & " ZG.MANr Like '" & Replace(.txtMANr_1.Text, "*", "%") & "'"

			ElseIf InStr(.txtMANr_1.Text, ",") > 0 Then
				FilterBez += "Kandidatennummer wie (" & .txtMANr_1.Text & ") " & vbLf
				sZusatzBez = .txtMANr_1.Text
				sZusatzBez = Replace(sZusatzBez, ", ", ",")
				sSql += strAndString & " ZG.MANr In ('" & Replace(sZusatzBez, ",", "','") & "')"

			ElseIf UCase(.txtMANr_1.Text) = UCase(.txtMANr_2.Text) Then
				FilterBez += "Kandidatennummer = " & .txtMANr_2.Text & vbLf
				sSql += strAndString & "ZG.MANr like '" &
									.txtMANr_1.Text & "'"

			ElseIf .txtMANr_1.Text <> "" And .txtMANr_2.Text = "" Then
				FilterBez += "Kandidatennummer ab " & .txtMANr_1.Text & vbLf
				sSql += strAndString & "ZG.MANr >= '" &
									.txtMANr_1.Text & "'"

			ElseIf .txtMANr_1.Text = "" And .txtMANr_2.Text <> "" Then
				FilterBez += "Kandidatennummer bis " & .txtMANr_2.Text & vbLf
				sSql += strAndString & "ZG.MANr <= '" &
									.txtMANr_2.Text & "'"

			Else
				FilterBez += "Kandidatennummer zwischen " & .txtMANr_1.Text & " und " &
										.txtMANr_2.Text & vbLf
				sSql += strAndString & "ZG.MANr Between '" &
							.txtMANr_1.Text & "' And '" & .txtMANr_2.Text & "'"
			End If

			' Vergütungsnummer ---------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.txtVGNr_1.Text) <> String.Empty Then
				sZusatzBez = .txtVGNr_1.Text.Trim
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Vergütungsnummer wie (" & sZusatzBez & ") " & vbLf

				sSql += strAndString & "ZG.VGNr In (" & sZusatzBez.Trim & ")"

			End If

			' Monat -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Month.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Month.Text.Trim
				FilterBez += "Monat wie (" & sZusatzBez & ") " & vbLf

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & "ZG.LP In (" & sZusatzBez & ")"
			End If

			' Jahr -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Year.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Year.Text.Trim
				FilterBez += "Jahr wie (" & sZusatzBez & ") " & vbLf

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & "ZG.Jahr In (" & sZusatzBez & ")"
			End If

			' Zahlart -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_LANr.Text) <> String.Empty Then
				sZusatzBez = .Cbo_LANr.Text.Trim
				FilterBez += "Lohnart wie (" & sZusatzBez & ") " & vbLf

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & "ZG.LANr In (" & sZusatzBez & ")"
			End If

			' Währung -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Currency.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Currency.Text.Trim

				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName
				FilterBez += "Währung wie (" & sZusatzBez & ") " & vbLf

				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")
				End If
				sSql += strAndString & "ZG.Currency In ('" & sZusatzBez & "')"

			End If

			' Berater -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "Mitarbeiter.KST"
			If UCase(.Cbo_Berater.Text) <> String.Empty Then
				FilterBez += String.Format("Vorschüsse mit Berater = {0}{1}", .Cbo_Berater.Text, vbLf)
				sZusatzBez = String.Empty
				strMyName = String.Empty
				Dim strKst As String = String.Empty
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

			'strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			'If UCase(.Cbo_Berater.Text) <> String.Empty Then
			'  sZusatzBez = .Cbo_Berater.Text.Trim
			'  FilterBez += "Berater wie (" & sZusatzBez & ") " & vbLf

			'  strName = Regex.Split(sZusatzBez.Trim, ",")
			'  strMyName = String.Empty
			'  For i As Integer = 0 To strName.Length - 1
			'    strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			'  Next
			'  If strName.Length > 0 Then sZusatzBez = strMyName

			'  If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
			'    sSql += strAndString & " (Mitarbeiter.KST = '' Or Mitarbeiter.KST Is Null) "
			'  Else
			'    If InStr(sZusatzBez, ",") > 0 Then
			'      sZusatzBez = Replace(sZusatzBez, ",", "','")

			'    End If
			'    sSql += strAndString & "Mitarbeiter.KST In('" & sZusatzBez & "')"
			'  End If

			'End If

			' Filiale -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			strFieldName = "Mitarbeiter.KST"
			If UCase(.Cbo_Filiale.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Filiale.Text.Trim
				FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

				If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
					sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
				Else

					sZusatzBez = GetFilialKstData(sZusatzBez)
					sZusatzBez = Replace(sZusatzBez, "'", "")
					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
					Next
					If strName.Length > 0 Then sZusatzBez = strMyName

					sSql += strAndString & " (" & sZusatzBez & ")"
				End If

			End If

			' Is eine überwiesen -----------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_Paryed.Text) <> String.Empty Then
				sZusatzBez = .Cbo_Paryed.Text
				Dim strvalue As String = DirectCast(.Cbo_Paryed.SelectedItem, ComboBoxItem).ValueMember

				If CInt(strvalue) = 0 Then 'InStr(1, UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Nicht überwiesene Zahlungen " & vbLf
					sSql += strAndString & " (ZG.VGNr = 0 Or ZG.VGNr Is Null) "

				Else
					FilterBez += "Überwiesene Zahlungen " & vbLf
					sSql += strAndString & "ZG.VGNr > 0 "
				End If

			End If

			' Daten fürs Lohnzahlung -----------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(.Cbo_LO.Text) <> String.Empty Then
				sZusatzBez = .Cbo_LO.Text
				Dim strvalue As String = DirectCast(.Cbo_LO.SelectedItem, ComboBoxItem).ValueMember

				If CInt(strvalue) = 0 Then 'InStr(1, UCase(sZusatzBez), UCase("nicht")) > 0 Then
					FilterBez += "Nicht Lohnzahlungen " & vbLf
					sSql += strAndString & " (ZG.ZGGrund Not Like 'Lohnabrechnung%' Or ZG.ZGGrund Is Null) "

				Else
					FilterBez += "Nur Lohnzahlungen " & vbLf
					sSql += strAndString & "(ZG.ZGGrund Like 'Lohnabrechnung%') "
				End If

			End If

			' Auszahlung am -----------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .deOuton_1.Text = String.Empty And .deOuton_2.Text = String.Empty Then
			ElseIf .deOuton_1.Text = .deOuton_2.Text Then
				FilterBez += "Bezahlt am = " & .deOuton_1.Text & vbLf
				sSql += strAndString & "Convert(Date, ZG.Aus_Dat, 104) = Convert(Date, '" & Format(CDate(.deOuton_1.Text), "d") & "', 104) " ' And '" & Format(CDate(.deOuton_2.Text), "d") & "'"

			ElseIf .deOuton_1.Text <> "" And .deOuton_2.Text = "" Then
				FilterBez += "Bezahlt ab " & .deOuton_1.Text & vbLf
				sSql += strAndString & "ZG.Aus_Dat >= Convert(Date, '" & Format(CDate(.deOuton_1.Text), "d") & "', 104) "

			ElseIf .deOuton_1.Text = "" And .deOuton_2.Text <> "" Then
				FilterBez += "Bezahlt bis " & .deOuton_2.Text & vbLf
				sSql += strAndString & "ZG.Aus_Dat <= Convert(Date, '" & Format(CDate(.deOuton_2.Text), "d") & "', 104) "

			Else
				FilterBez += "Bezahlt zwischen " & .deOuton_1.Text & " und " & .deOuton_2.Text & vbLf
				sSql += strAndString & "ZG.Aus_Dat Between Convert(Date, '" & Format(CDate(.deOuton_1.Text), "d") & "', 104) And Convert(Date, '" & Format(CDate(.deOuton_2.Text), "d") & "', 104) "
			End If

			' Printed am -----------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If .dePrintedOn_1.Text = String.Empty And .dePrintedOn_2.Text = String.Empty Then
			ElseIf .dePrintedOn_1.Text = .dePrintedOn_2.Text Then
				FilterBez += "Gedruckt am = " & .dePrintedOn_1.Text & vbLf
				sSql += strAndString & "Convert(date, ZG.Printed_Dat, 104) = Convert(Date, '" & Format(CDate(.dePrintedOn_1.Text), "d") & "', 104) " ' And Convert(Date, '" & Format(CDate(.dePrintedOn_2.Text), "d") & "', 104) "

			ElseIf .dePrintedOn_1.Text <> "" And .dePrintedOn_2.Text = "" Then
				FilterBez += "Gedruckt ab " & .dePrintedOn_1.Text & vbLf
				sSql += strAndString & "Convert(date, ZG.Printed_Dat, 104) >= Convert(Date, '" & Format(CDate(.dePrintedOn_1.Text), "d") & "', 104) "

			ElseIf .dePrintedOn_1.Text = "" And .dePrintedOn_2.Text <> "" Then
				FilterBez += "Gedruckt bis " & .dePrintedOn_2.Text & vbLf
				sSql += strAndString & "Convert(date, ZG.Printed_Dat, 104) <= Convert(Date, '" & Format(CDate(.dePrintedOn_2.Text), "d") & "', 104) "

			Else
				FilterBez += "Gedruckt zwischen " & .dePrintedOn_1.Text &
										" und " & .dePrintedOn_2.Text & vbLf
				sSql += strAndString & "Convert(date, ZG.Printed_Dat, 104) Between Convert(Date, '" & Format(CDate(.dePrintedOn_1.Text), "d") & "', 104) And Convert(Date, '" & Format(CDate(.dePrintedOn_2.Text), "d") & "', 104) "
			End If

			' Filialen Teilung...
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If strUSFiliale <> "" Then
				FilterBez += "Mitarbeiterfiliale wie (" & sZusatzBez & ")" & vbLf
				sSql += strAndString & "Mitarbeiter.MaFiliale Like '%" & strUSFiliale & "%' "
			End If

			' Mandantennummer -------------------------------------------------------------------------------------------------------
			If ClsDataDetail.m_InitialData.MDData.MultiMD = 1 Then
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				sSql += String.Format("{0}ZG.MDNr = {1}", strAndString, ClsDataDetail.m_InitialData.MDData.MDNr)
			End If

		End With
		ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("ZGSearchWhereQuery: {0}", sSql))


		Return sSql
	End Function

	Function GetSortString(ByVal frmTest As frmZGSearch) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Auszahlungsnummer
		'1 - Kandidatennummer
		'2 - Kandidatenname
		'3 - Auszahlungdatum (Aufsteigend)
		'4 - Auszahlungdatum (Absteigend)
		'5 - Betrag (Aufsteigend)
		'6 - Betrag (Absteigend)

		With frmTest
			strName = Regex.Split(.CboSort.Text.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1

				Select Case CInt(Left(strName(i).ToString.Trim, 1))
					Case 0      ' Nach Auszahlungsnummer
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ZG.ZGNr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Auszahlungsnummer"

					Case 1      ' Kandidatennummer
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ZG.MANr"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatennummer"

					Case 2      ' Kandidatenname
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Mitarbeiter.Nachname ASC, Mitarbeiter.Vorname ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"

					Case 3      ' Auszahlungdatum (Aufsteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ZG.Aus_Dat ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Auszahlungsdatum (Aufsteigend)"

					Case 4      ' Auszahlungdatum (Absteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ZG.Aus_Dat DESC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Auszahlungsdatum (Absteigend)"

					Case 5      ' Auszahlungdatum (Absteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ZG.Betrag ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Auszahlungsbetrag (Aufsteigend)"

					Case 6      ' Auszahlungdatum (Absteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " ZG.Betrag DESC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Auszahlungsbetrag (Absteigend)"


					Case Else      ' Auszahlungdatum (Absteigend)
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " Mitarbeiter.Nachname ASC, Mitarbeiter.Vorname ASC"
						strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"


				End Select

			Next i
		End With
		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function

	Sub GetJobNr4Print(ByVal frmSource As frmZGSearch)
		Dim strModul2print As String = String.Empty

		'0 - Alle Rechnungen
		'1 - Offene/Teil-Offene Rechnungen
		'2 - Gebuchte (bezahlte) Rechnungen
		'3 - Gemahnte Rechnungen
		'4 - MwSt.-pflichtige Liste aller Rechnungen
		'5 - MwSt.-freie Liste aller Rechnungen

		With frmSource
			strModul2print = "5.0"  ' Druck für Auszahlungsliste

		End With
		ClsDataDetail.GetModulToPrint = strModul2print

	End Sub

#End Region

	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strBerufItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strBerufItems += lst.Items(i).ToString & ","
		Next

		Return Left(strBerufItems, Len(strBerufItems) - 1)
	End Function



End Class