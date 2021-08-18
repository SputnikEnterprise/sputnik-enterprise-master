
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports SPKDUmsatz.ClsDataDetail

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath



	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
		Dim lstStuff As ListViewItem = New ListViewItem()
		Dim lvwColumn As ColumnHeader

		With Lv
			.Clear()

			' Nr;Nummer;Name;Strasse;PLZ Ort
			Dim strCaption As String() = Regex.Split(strColumnList, ";")
			' 0-1;0-1;2000-0;2000-0;2500-0
			Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")

			For i = 0 To strCaption.Length - 1
				lvwColumn = New ColumnHeader()
				lvwColumn.Text = strCaption(i).ToString

				lvwColumn.Width = CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
				If CInt(Right(strFieldInfo(i).ToString, 1)) = 1 Then
					lvwColumn.TextAlign = HorizontalAlignment.Right
				ElseIf CInt(Right(strFieldInfo(i).ToString, 1)) = 2 Then
					lvwColumn.TextAlign = HorizontalAlignment.Center
				Else
					lvwColumn.TextAlign = HorizontalAlignment.Left
				End If

				.Columns.Add(lvwColumn)
			Next

			lvwColumn = Nothing
		End With

	End Sub

	Sub FillFoundedKDEinzelnData(ByVal Lv As ListView, ByVal strQuery As String)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Lv.Items.Clear()
			Lv.FullRowSelect = True

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			While rKDrec.Read
				With Lv
					.Items.Add(String.Empty)
					.Items(i).SubItems.Add(rKDrec("KDNr").ToString)
					.Items(i).SubItems.Add(rKDrec("R_Name1").ToString)
					.Items(i).SubItems.Add(rKDrec("R_Strasse").ToString)
					.Items(i).SubItems.Add(rKDrec("R_Land").ToString & "-" & rKDrec("R_PLZ").ToString & " " & rKDrec("R_Ort").ToString)

					If Not IsDBNull(rKDrec("fBetragOhne")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("fBetragOhne")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("fBetragEx")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("fBetragEx")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("fBetragMwSt")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("fBetragMwSt")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("fBetragInk")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("fBetragInk")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If


					If Not IsDBNull(rKDrec("sBetragOhne")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("sBetragOhne")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("sBetragEx")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("sBetragEx")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("sBetragMwSt")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("sBetragMwSt")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("sBetragInk")) Then
						.Items(i).SubItems.Add(Format(CDec(rKDrec("sBetragInk")), "n"))
					Else
						.Items(i).SubItems.Add("")
					End If


					If Not IsDBNull(rKDrec("REKST1")) Then
						.Items(i).SubItems.Add(Format(rKDrec("REKST1").ToString))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rKDrec("REKST2")) Then
						.Items(i).SubItems.Add(Format(rKDrec("REKST2").ToString))
					Else
						.Items(i).SubItems.Add("")
					End If

					If Not IsDBNull(rKDrec("KST")) Then
						.Items(i).SubItems.Add(Format(rKDrec("KST").ToString))
					Else
						.Items(i).SubItems.Add("")
					End If

					.Items(i).SubItems.Add("")    ' 17
					.Items(i).SubItems.Add("[]")  ' 18

				End With

				i += 1
				Lv.EndUpdate()

			End While

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für ListMailToFields: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


		Catch e As Exception
			Lv.Items.Clear()
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

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
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr "
		strSqlQuery += "Where US_Filiale.Bezeichnung = '" & strFiliale & "' And "
		strSqlQuery += "US_Filiale.Bezeichnung <> '' "
		strSqlQuery += "And US_Filiale.Bezeichnung Is Not Null And Benutzer.KST Is Not Null "
		strSqlQuery += "Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

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
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function

	Sub DeleteAllRecinUmsatzDb()
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim Sql As String = String.Empty

		Sql &= "Begin Try Drop Table _KDUmsatz_{0} End Try Begin Catch End Catch; "
		Sql &= "Delete KDRPUmsatz Where USNr In (@LogedUSNr, 0)"
		Sql = String.Format(Sql, m_InitialData.UserData.UserNr)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@LogedUSNr", _ClsProgSetting.GetLogedUSNr())
			cmd.ExecuteNonQuery()


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub GetGroupKDData(ByVal sql As String, ByVal bSecYear As Boolean)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
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
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function UpdateDbWithValues(ByVal iKDNr As Integer,
															ByVal tBetragOhne As Decimal, ByVal tBetragEx As Decimal,
															ByVal tMwSt1 As Decimal, ByVal tBetragInk As Decimal) As Boolean
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

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
			param = cmd.Parameters.AddWithValue("@LogedUSNr", m_InitialData.UserData.UserNr)

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Function

	Sub InsertIntoUmsatzDb(ByVal rOPrec As SqlDataReader, ByVal bSecYear As Boolean)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
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
					param = cmd.Parameters.AddWithValue("@LogedUSNr", m_InitialData.UserData.UserNr)

					cmd.ExecuteNonQuery()

					cmd.Parameters.Clear()
				End If


			End While
			If Not bSecYear Then ClsDataDetail.strAllKDNr = str1KDNr

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub


#Region "Dropdown-Funktionen für 1. Seite..."

	Sub LoadListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Kundennummer")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kundenname")))
		cbo.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Kundenbetrag (Aufsteigend)")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Kundenbetrag (Absteigend)")))

		cbo.Properties.DropDownRows = 7
	End Sub

	Sub LoadListArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Liste mit Details")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Liste gruppiert")))

		cbo.Properties.DropDownRows = 7
	End Sub

	Sub LoadListKDKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select PLZ.Kanton From Kunden Left Join PLZ on Replace(Replace(Kunden.PLZ, ' ', ''), '-', '') = Convert(nvarchar(10), PLZ.Plz) AND PLZ.Land = 'CH' "
		strSqlQuery += "Where Kunden.Land = 'CH' "
		strSqlQuery += "And Kunden.ort <> '' And len(Kunden.PLZ) = 4 And PLZ.Kanton Is Not Null "
		strSqlQuery += "And Kunden.KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
		strSqlQuery += "Group By PLZ.Kanton Order By PLZ.Kanton"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				If rKDrec(strFieldName).ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
					If InStr(1, cbo.Text.ToUpper.Trim, rKDrec(strFieldName).ToString.ToUpper) > 0 Then
						'cbo.properties.itemsChecks(cbo.properties.items.Count - 1) = CheckState.Checked
					End If
					iWidth = CInt(IIf(iWidth > CInt(Len(rKDrec(strFieldName).ToString)), iWidth, CInt(Len(rKDrec(strFieldName).ToString))))

					i += 1
				End If
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListListMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetOPMonth]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				If rKDrec("Bezeichnung").ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(rKDrec("Bezeichnung").ToString)
				End If
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub LoadListYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetOPYear]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				If rKDrec("Bezeichnung").ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(rKDrec("Bezeichnung").ToString)
				End If
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub LoadListOPKst(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal strKstFieldNr As String)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetOPKst" & strKstFieldNr & "]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				If rKDrec("Bezeichnung").ToString.Trim <> String.Empty Then
					cbo.Properties.Items.Add(rKDrec("Bezeichnung").ToString)
				End If

			End While
			cbo.Properties.DropDownRows = 20


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub LoadListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			strSqlQuery = "[Get OPBerater]" ' "[GetREKDBetrater]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rFrec.Read
				cbo.Properties.Items.Add(rFrec("USName").ToString)
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub LoadListOPKst(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Lst As ListBox)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strUserName As String = String.Empty

		'Lst.Items.Clear()
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
			cbo.Properties.DropDownRows = 20

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

	Function GetUSNameFromKst(ByVal strSelectedKst As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = "[Get UserData With USKst]"
		Dim strResult As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

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

	Sub LoadListKDFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetKDFiliale]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rFrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rFrec.Read
				cbo.Properties.Items.Add(rFrec("Bezeichnung").ToString)
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#End Region

	Sub LoadListForActivate(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		Try

			cbo.Properties.Items.Add("")

			cbo.Properties.Items.Add("Aktiviert")
			cbo.Properties.Items.Add("Nicht Aktiviert")

		Catch e As Exception
			MsgBox(e.Message)

		Finally

		End Try

	End Sub



End Module
