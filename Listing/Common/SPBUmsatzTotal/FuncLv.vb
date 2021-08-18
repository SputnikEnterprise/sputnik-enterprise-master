
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPRPUmsatzTotal.ClsDataDetail
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsFunc As New ClsDivFunc
	'Dim _ClsReg As New SPProgUtility.ClsDivReg
	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath



	Private m_md As Mandant
	Private m_common As CommonSetting
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI


	''' <summary>
	''' 0 = MANationality
	''' 1 = MALand
	''' 2 = MAOrt
	''' 3 = ESGewerbe
	''' </summary>
	''' <param name="Modul2Show"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetDbData4Listing(ByVal Modul2Show As Integer) As DataTable
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = ""

		Select Case Modul2Show
			Case 0
				strQuery = "[GetMANationality]"

			Case 1
				strQuery = "[GetMALand]"

			Case 2
				strQuery = "[GetMAOrt]"

			Case 3
				strQuery = "[GetESBranche]"

			Case 4
				strQuery = "[Get Employement Data For Listing in DB1]"

			Case 5
				strQuery = "[Get Employee Data For Listing in DB1]"

			Case 6
				strQuery = "[Get Customer Data For Listing in DB1]"


		End Select

		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter
		'Dim param As System.Data.SqlClient.SqlParameter

		'param = cmd.Parameters.AddWithValue("@MDYear", iYear)

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "DbData")

		Return ds.Tables(0)
	End Function

	Function EnablingMarsintoConnString(ByVal _strConnString As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTempConnString As String = _strConnString
		Dim _clsReg As New SPProgUtility.ClsDivReg

		Try
			'  & ";mars=true"
			If Not strTempConnString.ToUpper.Contains("MultipleActiveResultSets=") Then
				strTempConnString &= ";MultipleActiveResultSets="
				Dim strQuery As String = "//SPBUmsatzTotal/frmUmsatz/DiffSetting[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/MARS"

				Dim strBez As String = _clsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
				If strBez <> String.Empty Then
					strTempConnString &= strBez

				Else
					strTempConnString &= "True"
				End If
			End If

		Catch ex As Exception

		End Try

		Try
			'  & ";pooling=true"
			If Not strTempConnString.ToUpper.Contains("Pooling=") Then
				strTempConnString &= ";Pooling="
				Dim strQuery As String = "//SPBUmsatzTotal/frmUmsatz/DiffSetting[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/Pooling"

				Dim strBez As String = _clsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
				If strBez <> String.Empty Then
					strTempConnString &= strBez

				Else
					strTempConnString &= "true"
				End If
			End If

		Catch ex As Exception

		End Try


		Try
			'  & ";Connect Timeout=120"
			If Not strTempConnString.ToUpper.Contains("timeout=".ToUpper) Then
				strTempConnString &= ";Connect Timeout="
				Dim strQuery As String = "//SPBUmsatzTotal/frmUmsatz/DiffSetting[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/Timeout"

				Dim strBez As String = _clsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
				If strBez <> String.Empty Then
					strTempConnString &= strBez

				Else
					strTempConnString &= "300"
				End If
			End If

		Catch ex As Exception

		End Try

		Return strTempConnString
	End Function

	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim lstStuff As ListViewItem = New ListViewItem()
		Dim lvwColumn As ColumnHeader

		With Lv
			.Clear()

			' Nr;Nummer;Name;Strasse;PLZ Ort
			If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
			If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

			Dim strCaption As String() = Regex.Split(strColumnList, ";")
			' 0-1;0-1;2000-0;2000-0;2500-0
			Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
			Dim strFieldWidth As String
			Dim strFieldAlign As String = "0"
			Dim strFieldData As String()

			For i = 0 To strCaption.Length - 1
				lvwColumn = New ColumnHeader()
				lvwColumn.Text = strCaption(i).ToString
				strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

				If strFieldInfo(i).ToString.StartsWith("-") Then
					strFieldWidth = strFieldData(1)
					lvwColumn.Width = CInt(strFieldWidth) * -1
					If strFieldData.Count > 1 Then
						strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
					End If
				Else
					strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
					lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
					If strFieldData.Count > 1 Then
						strFieldAlign = strFieldData(1)
					End If
					'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
				End If
				If strFieldAlign = "1" Then
					lvwColumn.TextAlign = HorizontalAlignment.Right
				ElseIf strFieldAlign = "2" Then
					lvwColumn.TextAlign = HorizontalAlignment.Center
				Else
					lvwColumn.TextAlign = HorizontalAlignment.Left

				End If

				'ElseIf strFieldInfo(i).ToString.EndsWith("2") Then
				'  lvwColumn.TextAlign = HorizontalAlignment.Center
				'Else
				'  lvwColumn.TextAlign = HorizontalAlignment.Left
				'End If

				lstStuff.BackColor = Color.Yellow

				.Columns.Add(lvwColumn)
			Next

			lvwColumn = Nothing
		End With

	End Sub

	Sub FillFoundedKstBez(ByVal Lst As ListBox, ByVal _SearchCriteria As SearchCriteria)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim LoiKstBez As List(Of String) = FillLoi4KstBez(_SearchCriteria)

		If LoiKstBez.Count = 0 Then
			LoiKstBez = FillLoi4KstBez(_SearchCriteria)
			If LoiKstBez.Count = 0 Then Return
		End If

		m_UtilityUi = New UtilityUI
		'Dim Time_1 As Double = System.Environment.TickCount

		Lst.Items.Clear()

		Try
			Lst.BeginUpdate()

			For i As Integer = 0 To LoiKstBez.Count - 1
				Trace.WriteLine(LoiKstBez(i).ToString)
				With Lst
					.Items.Add(LoiKstBez(i))

				End With
			Next i
			Lst.EndUpdate()


		Catch e As SqlException
			m_UtilityUi.ShowErrorDialog(e.ToString)
			Err.Clear()

		Catch e As Exception
			m_UtilityUi.ShowErrorDialog(e.ToString)
			Err.Clear()

		Finally

		End Try

	End Sub

	Function FillLoi4KstBez(ByVal _SearchCriteria As SearchCriteria) As List(Of String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim loiBez As New List(Of String)

		'Dim Time_1 As Double = System.Environment.TickCount
		Dim sSql As String = "[GetBUmsatzKst3_Db1_1 With Mandant]"

		m_UtilityUi = New UtilityUI
		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandTimeout = 400
			'Console.WriteLine("CommandTimeout: (" & (cmd.CommandTimeout / 1000).ToString() + " s)")
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter
			Console.WriteLine("Param: (OK)")

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@LPVon", _SearchCriteria.FirstMonth) ' strFMonth)
			param = cmd.Parameters.AddWithValue("@LPBis", _SearchCriteria.LastMonth) ' strLMonth)
			param = cmd.Parameters.AddWithValue("@MDVYear", _SearchCriteria.FirstYear) ' strVYear)
			param = cmd.Parameters.AddWithValue("@MDBYear", _SearchCriteria.LastYear) ' strBYear)

			param = cmd.Parameters.AddWithValue("@MANr", ReplaceMissing(_SearchCriteria.employeeNumber, 0))  ' strBYear)
			param = cmd.Parameters.AddWithValue("@KDNr", ReplaceMissing(_SearchCriteria.customerNumber, 0))  ' strBYear)
			param = cmd.Parameters.AddWithValue("@ESNr", ReplaceMissing(_SearchCriteria.esNumber, 0))  ' strBYear)

			Dim rLOLrec As SqlDataReader = cmd.ExecuteReader

			While rLOLrec.Read
				loiBez.Add("")
				loiBez(i) = rLOLrec("KST").ToString

				i += 1
			End While


			'Dim Time_2 As Double = System.Environment.TickCount
			'Console.WriteLine("Zeit für LOL.KST: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Catch e As SqlException
			Trace.WriteLine(m_InitialData.MDData.MDDbConn)
			'MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "SQL:FillFoundedKstBez_1")
			'Err.Clear()
			loiBez.Clear()

		Catch e As Exception
			Trace.WriteLine(m_InitialData.MDData.MDDbConn)
			m_UtilityUi.ShowErrorDialog(e.ToString)
			Err.Clear()
			loiBez.Clear()

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return loiBez
	End Function

	Function GetLandCode(ByVal strLand As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strLandResult As String = ","
		Dim strFieldName As String = "Code"

		Dim strSqlQuery As String = "Select LND.Code From LND "
		strSqlQuery += "Where LND.Land In ('" & strLand & "') "
		strSqlQuery += "Group By LND.Code Order By LND.Code"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		m_UtilityUi = New UtilityUI
		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rLandrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rLandrec.Read
				strLandResult += rLandrec(strFieldName).ToString & ","

			End While
			If strLandResult.Length > 1 Then
				strLandResult = Mid(strLandResult, 2, Len(strLandResult) - 2)
				strLandResult = Replace(strLandResult, ",", "','")
			Else
				strLandResult = String.Empty
			End If

		Catch e As Exception
			strLandResult = String.Empty
			m_UtilityUi.ShowErrorDialog(e.StackTrace & vbNewLine & e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strLandResult
	End Function

	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		m_UtilityUi = New UtilityUI
		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rPLZrec.Read
				strPLZResult += rPLZrec(strFieldName).ToString & ","

			End While
			If strPLZResult.Length > 1 Then
				strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
				strPLZResult = Replace(strPLZResult, ",", "','")
			Else
				strPLZResult = String.Empty
			End If

		Catch e As Exception
			strPLZResult = String.Empty
			m_UtilityUi.ShowErrorDialog(e.StackTrace & vbNewLine & e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select KST, USFiliale From Benutzer Where USFiliale = '" & strFiliale & "' And KST Is Not Null "
		strSqlQuery &= "Group By KST, USFiliale Order By KST"

		'Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr "
		'strSqlQuery += "Where US_Filiale.Bezeichnung = '" & strFiliale & "' And "
		'strSqlQuery += "US_Filiale.Bezeichnung <> '' "
		'strSqlQuery += "And US_Filiale.Bezeichnung Is Not Null And Benutzer.KST Is Not Null "
		'strSqlQuery += "Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		m_UtilityUi = New UtilityUI
		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rPLZrec.Read
				If Not IsDBNull(rPLZrec(strFieldName)) Then
					strKSTResult += rPLZrec(strFieldName).ToString & ","
				End If
			End While
			Console.WriteLine("strKSTResult: " & strKSTResult)
			If strKSTResult.Length > 1 Then
				strKSTResult = Mid(strKSTResult, 2, Len(strKSTResult) - 2)
				strKSTResult = Replace(strKSTResult, ",", "','")
			Else
				strKSTResult = String.Empty
			End If

		Catch e As Exception
			strKSTResult = String.Empty
			m_UtilityUi.ShowErrorDialog(e.StackTrace & vbNewLine & e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function

	'Sub DeleteAllRecinUJDb()
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Dim sSql As String = "Select * Into #UmsatzJournal_New From UmsatzJournal_New Where ID = 0" ' "Delete UmsatzJournal_New Where UserNr In (@LogedUSNr, 0)"
	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'    cmd.CommandType = Data.CommandType.Text
	'    'Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

	'    'param = cmd.Parameters.AddWithValue("@LogedUSNr", _ClsProgSetting.GetLogedUSNr())
	'    cmd.ExecuteNonQuery()


	'  Catch e As Exception
	'    MsgBox(e.StackTrace)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	Sub GetGroupKDData(ByVal sSql As String, ByVal bSecYear As Boolean)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rOPrec As SqlDataReader = cmd.ExecuteReader

			'      While rOPrec.Read
			If bSecYear Then
				InsertIntoUmsatzDb(rOPrec, bSecYear)

			Else
				InsertIntoUmsatzDb(rOPrec, bSecYear)

			End If

			'      End While

		Catch e As Exception
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function UpdateDbWithValues(ByVal iKDNr As Integer,
															ByVal tBetragOhne As Decimal, ByVal tBetragEx As Decimal,
															ByVal tMwSt1 As Decimal, ByVal tBetragInk As Decimal) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iUsNr As Integer = iSelectedUSNr

		Try
			Conn.Open()
			Dim strInsertString = "Update KDRPUmsatz Set sBetragOhne = @tBetragOhne, sBetragEx = @tBetragEx, "
			strInsertString += "sBetragMwSt = @tMwSt1, sBetragInk = @tBetragInk Where KDNr = @KDNr And USNr = @LogedUSNr"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strInsertString, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = New SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@tBetragOhne", tBetragOhne.ToString)
			param = cmd.Parameters.AddWithValue("@tBetragEx", tBetragEx.ToString)
			param = cmd.Parameters.AddWithValue("@tMwSt1", tMwSt1.ToString)
			param = cmd.Parameters.AddWithValue("@tBetragInk", tBetragInk.ToString)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr.ToString)
			param = cmd.Parameters.AddWithValue("@LogedUSNr", iUsNr)

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()


		Catch e As Exception
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Function

	Sub InsertIntoUmsatzDb(ByVal rOPrec As SqlDataReader, ByVal bSecYear As Boolean)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iUsNr As Integer = iSelectedUSNr
		Dim str1KDNr As String = ","
		Dim bInsert As Boolean

		Try
			Conn.Open()

			Dim strInsertString = String.Empty
			If Not bSecYear Then
				strInsertString = "Insert Into KDRPUmsatz (KDNr, FBetragOhne, FBetragEx, FBetragMwSt, FBetragInk, "
				strInsertString += "USNr) Values (@KDNr, @BetragOhne, @BetragEx, @BetragMwSt, @BetragInk, @LogedUSNr)"
			Else
				strInsertString = "Insert Into KDRPUmsatz (KDNr, sBetragOhne, sBetragEx, sBetragMwSt, sBetragInk, "
				strInsertString += "USNr) Values (@KDNr, @BetragOhne, @BetragEx, @BetragMwSt, @BetragInk, @LogedUSNr)"

			End If

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strInsertString, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			While rOPrec.Read
				'        If bSecYear Then

				'        Else
				param = New SqlClient.SqlParameter
				If Not bSecYear Then
					str1KDNr += rOPrec("KDNr").ToString & ","
					bInsert = True

				Else
					If InStr(ClsDataDetail.strAllKDNr, "," & rOPrec("KDNr").ToString & ",") > 0 Then
						UpdateDbWithValues(CInt(rOPrec("KDNr").ToString),
															CDec(rOPrec("tBetragOhne").ToString), CDec(rOPrec("tBetragEx").ToString),
															CDec(rOPrec("tMwSt1").ToString), CDec(rOPrec("tBetragInk").ToString))
						bInsert = False
					Else
						bInsert = True
					End If

				End If

				If bInsert Then
					param = cmd.Parameters.AddWithValue("@KDNr", rOPrec("KDNr").ToString)
					param = cmd.Parameters.AddWithValue("@BetragOhne", rOPrec("tBetragOhne").ToString)
					param = cmd.Parameters.AddWithValue("@BetragEx", rOPrec("tBetragEx").ToString)
					param = cmd.Parameters.AddWithValue("@BetragMwSt", rOPrec("tMwSt1").ToString)
					param = cmd.Parameters.AddWithValue("@BetragInk", rOPrec("tBetragInk").ToString)
					param = cmd.Parameters.AddWithValue("@LogedUSNr", iUsNr)

					cmd.ExecuteNonQuery()
					cmd.Parameters.Clear()
				End If

				'       End If

			End While
			If Not bSecYear Then ClsDataDetail.strAllKDNr = str1KDNr

		Catch e As Exception
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListKDKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit) ' SPRPUmsatzTotal.myCbo)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select PLZ.Kanton From Kunden Left Join PLZ on Replace(Replace(Kunden.PLZ, ' ', ''), '-', '') = Convert(nvarchar(10), PLZ.Plz) AND PLZ.Land = 'CH' "
		strSqlQuery += "Where Kunden.Land = 'CH' "
		strSqlQuery += "And Kunden.ort <> '' And len(Kunden.PLZ) = 4 And PLZ.Kanton Is Not Null "
		strSqlQuery += "And Kunden.KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
		strSqlQuery += "Group By PLZ.Kanton Order By PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("MDDbConn: {0}", m_InitialData.MDData.MDDbConn))

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				If rDbrec(strFieldName).ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)
					If InStr(1, cbo.Text.ToUpper.Trim, rDbrec(strFieldName).ToString.ToUpper) > 0 Then
						'cbo.properties.itemsChecks(cbo.properties.items.Count - 1) = CheckState.Checked
					End If

				End If
			End While
			cbo.Properties.DropDownRows = 27

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("MDName: {0} | MDNr: {1} | MDDbConn: {2}", m_InitialData.MDData.MDName, m_InitialData.MDData.MDNr, m_InitialData.MDData.MDDbConn))

		Dim strFieldName As String = "Monat"

		Try
			Conn.Open()

			strSqlQuery = "[GetRPLP]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				If rDbrec(strFieldName).ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(CType(rDbrec(strFieldName), Integer))
				End If
			End While
			cbo.Properties.DropDownRows = 13

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("MDDbConn: {0}", m_InitialData.MDData.MDDbConn))

		Dim strFieldName As String = "Jahr"

		Try
			Conn.Open()

			strSqlQuery = "[GetRPYear]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				If rDbrec(strFieldName).ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(CType(rDbrec(strFieldName), Integer))
				End If
			End While
			cbo.Properties.DropDownRows = 15

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMAKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("MDDbConn: {0}", m_InitialData.MDData.MDDbConn))

		Dim strFieldName As String = "MA_Kanton"

		Try
			Conn.Open()

			strSqlQuery = "[GetMAKanton]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				If rDbrec(strFieldName).ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)

				End If
			End While
			cbo.Properties.DropDownRows = 27

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	' Berater ----------------------------------------------------------------------------------------------------
	Function ListBerater(Optional ByVal filiale As String = "") As List(Of String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strEntry As String
		Dim strSqlQuery As String = "[List Benutzer]"
		Dim liResult As New List(Of String)

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("MDDbConn: {0}", m_InitialData.MDData.MDDbConn))

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@filiale", filiale)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			liResult.Clear()

			While rESrec.Read
				strEntry = String.Format("{0}", rESrec("KST"))
				liResult.Add(strEntry)
			End While


		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function

	Function GetUSNameFromKst(ByVal strSelectedKst As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = "[Get UserData With USKst]"
		Dim strResult As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("MDDbConn: {0}", m_InitialData.MDData.MDDbConn))

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@USKst", strSelectedKst)
			Dim rRec As SqlDataReader = cmd.ExecuteReader

			While rRec.Read
				strResult = String.Format("{0}, {1}", rRec("Nachname"), rRec("Vorname"))
			End While

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Sub ListOPKst(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Lst As ListBox, ByVal _SearchCriteria As SearchCriteria)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strUserName As String = String.Empty

		Lst.Items.Clear()
		FillFoundedKstBez(Lst, _SearchCriteria)
		Dim Time_1 As Double = System.Environment.TickCount

		Try
			cbo.Properties.Items.Clear()

			Dim strKSTbez As String()
			Dim strKst As String
			Dim strAllKst As String = String.Empty
			Dim bInsertItem As Boolean

			cbo.Properties.Items.BeginUpdate()
			For i As Integer = 0 To Lst.Items.Count - 1
				With cbo
					strKst = Lst.Items(i).ToString
					strKSTbez = strKst.Split(CChar("/"))
					For j As Integer = 0 To strKSTbez.Length - 1
						strUserName = GetUSNameFromKst(strKSTbez(j))
						bInsertItem = AllowedtoInsertToCbo(cbo, strUserName)

						If bInsertItem Then
							.Properties.Items.Add(String.Format("{0} ({1})", strUserName, strKSTbez(j)))
						End If

					Next
				End With
			Next
			cbo.Properties.Items.EndUpdate()
			cbo.Properties.Sorted = True
			cbo.Properties.DropDownRows = 30

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für LOL.KST: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function AllowedtoInsertToCbo(ByVal Cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal strBez As String) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		For i As Integer = 0 To Cbo.Properties.Items.Count - 1
			If Cbo.Properties.Items(i).ToString.ToLower.Contains(strBez.ToLower) Then
				Return False
			End If
		Next

		Return True
	End Function

	Sub ListMDFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iWidth As Integer
		Dim strFieldName As String = "Bezeichnung"

		Try
			Conn.Open()

			strSqlQuery = "[GetMDFiliale]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rDbrec(strFieldName).ToString)), iWidth, _
													CInt(Len(rDbrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMANationality(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iWidth As Integer
		Dim strFieldName As String = "Bezeichnung"

		Try
			Conn.Open()

			strSqlQuery = "[GetMANationality]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rDbrec(strFieldName).ToString)), iWidth, _
													CInt(Len(rDbrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.Properties.DropDownRows = 10

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMALand(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iWidth As Integer
		Dim strFieldName As String = "Bezeichnung"

		Try
			Conn.Open()

			strSqlQuery = "[GetMALand]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rDbrec(strFieldName).ToString)), iWidth, _
													CInt(Len(rDbrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.Properties.DropDownRows = 10

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMAOrt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iWidth As Integer
		Dim strFieldName As String = "Bezeichnung"

		Try
			Conn.Open()

			strSqlQuery = "[GetMAOrt]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rDbrec(strFieldName).ToString)), iWidth, _
													CInt(Len(rDbrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.Properties.DropDownRows = 10

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListESBranche(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iWidth As Integer
		Dim strFieldName As String = "Bezeichnung"

		Try
			Conn.Open()

			strSqlQuery = "[GetESBranche]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rDbrec.Read
				cbo.Properties.Items.Add(rDbrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rDbrec(strFieldName).ToString)), iWidth, _
													CInt(Len(rDbrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub


#End Region

	Sub ListForActivate(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			cbo.Properties.Items.Add("")
			cbo.Properties.Items.Add("Aktiviert")
			cbo.Properties.Items.Add("Nicht Aktiviert")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally

		End Try

	End Sub

	Function GetLAAHVPflicht(ByVal strYear As Integer) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "0"
		'    Dim sSql As String = "Select AHVPflichtig From LA Where LANr = 530 And LAJahr = @MDYear"
		Dim sSql As String = "[Get LAAHVPflicht For R_A]"
		If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return String.Empty

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDYear", strYear)

			Try
				Dim rLOLrec As SqlDataReader = cmd.ExecuteReader

				While rLOLrec.Read
					If Not String.IsNullOrEmpty(rLOLrec("LAPflicht").ToString) Then
						strResult = rLOLrec("LAPflicht").ToString
					End If

				End While

			Catch ex As Exception
				m_UtilityUi.ShowErrorDialog(ex.ToString)
			End Try


		Catch ex As SqlException
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return strResult
	End Function

	Function AllowedToShowEachEmployee(ByVal year As Integer) As Boolean
		Dim result As Boolean = False
		Dim sql As String = String.Empty

		sql = "[Get LA Info 4 Netto_2]"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("MDYear", year))

		Dim Conn = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Conn.Open()

		Dim reader = OpenReader(Conn, sql, listOfParams, CommandType.StoredProcedure)

		If (Not reader Is Nothing AndAlso reader.Read()) Then
			Dim value = SafeGetInteger(reader, "_FeierNetto_2", 0)
			result = value
		End If

		Return result

	End Function


#Region "Database"

	''' <summary>
	''' Opens a SqlClient.SqlDataReader object. 
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters collection.</param>
	''' <returns>The open reader or nothing in error case.</returns>
	''' <remarks>The reader is opened with the CloseConnection option, so when the reader is closed the underlying database connection will also be closed.</remarks>
	Function OpenReader(ByVal conn As SqlClient.SqlConnection, ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As SqlClient.SqlDataReader

		'Dim Conn As SqlClient.SqlConnection = Nothing

		'If m_ExcplicitConnection Is Nothing Then
		'	' Open a new connection
		'	Conn = New SqlClient.SqlConnection(DBConnectionstring)
		'Else
		'	' Use the explicit connection
		'	Conn = m_ExcplicitConnection
		'End If

		Dim reader As SqlClient.SqlDataReader

		Try

			If conn Is Nothing Then
				' Only open connection if its not an explicit connection
				conn = New SqlClient.SqlConnection(m_InitialData.MDData.MDDbConn)
				conn.Open()
			End If

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, conn)
			cmd.CommandType = commandType

			'If IsExplicitTransactionAvailable Then
			'	' Attach explicit transaction if available
			'	cmd.Transaction = m_ExplicitTransaction
			'End If

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)
				Next
			End If

			'If IsExplicitConnectionOpen Then
			reader = cmd.ExecuteReader()
			'Else
			' Close connection together with reader close if its not an explicit connection.
			'reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
			'End If

		Catch e As Exception
			m_Logger.LogError(String.Format("SQL={0}. Exception={1}", sql, e.ToString()))

			'If Not IsExplicitConnectionOpen Then
			'	' Only close local connection.
			'	conn.Close()
			'	conn.Dispose()
			'End If

			reader = Nothing
		End Try

		Return reader
	End Function


	''' <summary>
	''' Returns a string or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetString(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a boolean or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetBoolean(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an integer or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetInt32(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an short integer or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetShort(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Short?) As Short?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetInt16(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an byte or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetByte(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Byte?) As Byte?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetByte(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a decimal or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetDecimal(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Decimal?) As Decimal?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDecimal(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a double or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetDouble(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Double?) As Double?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDouble(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an datetime or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDateTime(columnIndex)
		Else
			Return defaultValue
		End If
	End Function


	''' <summary>
	''' Returns an byte array or nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader(columnIndex)
		Else
			Return Nothing
		End If
	End Function

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function


#End Region



End Module

