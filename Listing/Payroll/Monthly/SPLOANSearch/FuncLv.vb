
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SPLOANSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()


#Region "Dropdown-Funktionen für 1. Seite..."

	' Jahr ---------------------------------------------------------------------------------------------
	Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strEntry As String
		Dim strSqlQuery As String = "Select LO.Jahr From LO "
		strSqlQuery += "Group By LO.Jahr Order By LO.Jahr DESC"


		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader                  '

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				strEntry = rFOPrec("Jahr").ToString
				'cbo.Items.Add(New ComboBoxItem(rFOPrec("Jahr").ToString, strEntry))
				cbo.Properties.Items.Add(rFOPrec("Jahr").ToString)

				i += 1
			End While

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("ListLOJahr", ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Beruf ---------------------------------------------------------------------------------------------
	Sub ListBeruf(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal JahrVon As String, ByVal JahrBis As String,
								ByVal MonatVon As String, ByVal MonatBis As String, ByVal Gruppe1 As String, ByVal MANr As String,
								ByVal LANr As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Dim strValue As String
			If Not IsNumeric(JahrBis) Then
				JahrBis = JahrVon
			End If
			If Not IsNumeric(MonatBis) Then
				MonatBis = MonatVon
			End If

			Dim strSqlQuery As String = "SELECT LOL.GAV_Beruf "
			strSqlQuery += "FROM LOL "
			strSqlQuery += "WHERE LOL.MDNr = @MDNr And "
			If MANr.Length > 0 Then
				strSqlQuery += String.Format("LOL.MANR IN ({0}) And ", MANr)
			End If
			If LANr.Length > 2 Then
				strSqlQuery += String.Format("LOL.LANR IN ({0}) And ", LANr)
			End If
			strSqlQuery += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
			strSqlQuery += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
			strSqlQuery += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "
			If Gruppe1 = "Leere Felder" Then
				strSqlQuery += "LOL.GAV_Gruppe1 = '' "
			Else
				strSqlQuery += "(@gruppe1 = '' Or LOL.GAV_Gruppe1 = @gruppe1) "
			End If

			strSqlQuery += "GROUP BY LOL.GAV_Beruf ORDER BY LOL.GAV_Beruf ASC "

			Dim i As Integer = 0
			Dim iWidth As Integer = 0

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			Dim pGruppe1 As SqlParameter = New SqlParameter("@gruppe1", SqlDbType.NVarChar, 100)
			Dim pJahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
			Dim pJahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
			Dim pMonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
			Dim pMonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
			pGruppe1.Value = Gruppe1
			pJahrVon.Value = JahrVon
			pJahrBis.Value = JahrBis
			pMonatVon.Value = MonatVon
			pMonatBis.Value = MonatBis
			cmd.Parameters.Add(pGruppe1)
			cmd.Parameters.Add(pJahrVon)
			cmd.Parameters.Add(pJahrBis)
			cmd.Parameters.Add(pMonatVon)
			cmd.Parameters.Add(pMonatBis)

			Dim rec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rec.Read
				If (IsDBNull(rec("GAV_Beruf")) OrElse rec("GAV_Beruf").ToString = "") Then
					strValue = "Leere Felder"
				Else
					strValue = rec("GAV_Beruf").ToString
				End If
				'If (strValue = "Leere Felder" Xor cbo.Properties.Items.ContainsValue("Leere Felder")) Or strValue <> "Leere Felder" Then
				'cbo.Properties.Items.Add(New ComboBoxItem(strValue, strValue))
				cbo.Properties.Items.Add(strValue)
				'iWidth = CInt(IIf(iWidth > CInt(Len(strValue).ToString), iWidth, CInt(Len(strValue).ToString)))

				i += 1

				'End If
			End While
			'cbo.Properties.DropDownWidth = CInt((iWidth * 7) + 20)

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("ListBeruf", ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' 1.Kategorie ---------------------------------------------------------------------------------------------
	Sub List1Kategorie(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal JahrVon As String, ByVal JahrBis As String,
										 ByVal MonatVon As String, ByVal MonatBis As String, ByVal Beruf As String, ByVal MANr As String,
										 ByVal LANr As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Dim strValue As String
			If Not IsNumeric(JahrBis) Then
				JahrBis = JahrVon
			End If
			If Not IsNumeric(MonatBis) Then
				MonatBis = MonatVon
			End If

			Dim strSqlQuery As String = "SELECT LOL.GAV_Gruppe1 "
			strSqlQuery += "FROM LOL "
			strSqlQuery += "WHERE LOL.MDNr = @MDNr And "
			If MANr.Length > 0 Then
				strSqlQuery += String.Format("LOL.MANR IN ({0}) And ", MANr)
			End If
			If LANr.Length > 0 Then
				strSqlQuery += String.Format("LOL.LANR IN ({0}) And ", LANr)
			End If
			strSqlQuery += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
			strSqlQuery += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
			strSqlQuery += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "
			If Beruf = "Leere Felder" Then
				strSqlQuery += "LOL.GAV_Beruf = '' "
			Else
				strSqlQuery += "(@beruf = '' Or LOL.GAV_Beruf = @beruf) "
			End If

			strSqlQuery += "GROUP BY LOL.GAV_Gruppe1 ORDER BY LOL.GAV_Gruppe1 ASC "

			Dim i As Integer = 0
			Dim iWidth As Integer = 0

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@beruf", Beruf)
			cmd.Parameters.AddWithValue("@monatVon", MonatVon)
			cmd.Parameters.AddWithValue("@monatBis", MonatBis)
			cmd.Parameters.AddWithValue("@jahrVon", JahrVon)
			cmd.Parameters.AddWithValue("@jahrBis", JahrBis)

			Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rESrec.Read
				If (IsDBNull(rESrec("GAV_Gruppe1")) OrElse rESrec("GAV_Gruppe1").ToString = "") Then
					strValue = "Leere Felder"
				Else
					strValue = rESrec("GAV_Gruppe1").ToString
				End If
				'If (strValue = "Leere Felder" Xor cbo.ContainsValue("Leere Felder")) Or strValue <> "Leere Felder" Then
				cbo.Properties.Items.Add(strValue)
				iWidth = CInt(IIf(iWidth > CInt(Len(strValue).ToString), iWidth, CInt(Len(strValue).ToString)))

				i += 1

				'End If
			End While
			'cbo.DropDownWidth = CInt((iWidth * 7) + 20)

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("List1Kategorie", ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub



#End Region

#Region "Allgemeine Funktionen"

	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
		Try
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
						lvwColumn.Width = CInt(strFieldWidth)   '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
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

					lstStuff.BackColor = Color.Yellow

					.Columns.Add(lvwColumn)
				Next

				lvwColumn = Nothing
			End With
		Catch ex As Exception ' Manager
			MessageBoxShowError("FillDataHeaderLv", ex)
		End Try


	End Sub

	' Art der Liste
	Sub ListCboArtderListe(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add(m_Translate.GetSafeTranslationValue("Liste der Mitarbeiterlohnarten")) ' New ComboBoxItem(m_Translate.GetSafeTranslationValue("Liste der Mitarbeiterlohnarten"), "11.0"))
		cbo.Properties.Items.Add(m_Translate.GetSafeTranslationValue("Liste der Lohnartenrekapitulation Arbeitnehmer")) ' , "11.3"))
		cbo.Properties.Items.Add(m_Translate.GetSafeTranslationValue("Liste der Krankentaggeld-Beiträge")) ' , "11.0"))
		'    cbo.SelectedIndex = 0
	End Sub

	' Monate 1 bis 12
	Sub ListCboMonate1Bis12(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		For i As Integer = 1 To 12
			cbo.Properties.Items.Add(i.ToString)
		Next
	End Sub

	' Lohnarten
	Sub ListLohnarten(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal JahrVon As String, ByVal JahrBis As String,
										 ByVal MonatVon As String, ByVal MonatBis As String, ByVal Beruf As String,
										 ByVal Gruppe1 As String, ByVal MANr As String, ByVal strLANr As String)

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			If Not IsNumeric(JahrBis) Then
				JahrBis = JahrVon
			End If
			If Not IsNumeric(MonatBis) Then
				MonatBis = MonatVon
			End If

			Dim strValue As String
			Dim strSqlQuery As String = "SELECT LA.LANR, LA.LALOText "
			strSqlQuery += "FROM LA "
			strSqlQuery += "INNER JOIN LOL ON "
			strSqlQuery += "LOL.LANR = LA.LANR And "
			If MANr.Length > 0 Then
				strSqlQuery += String.Format("LOL.MANR IN ({0}) And ", MANr)
			End If
			If strLANr.Length > 0 Then
				strSqlQuery += String.Format("LOL.LANr IN ({0}) And ", strLANr)
			End If

			strSqlQuery += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
			strSqlQuery += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
			strSqlQuery += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "
			If Beruf = "Leere Felder" Then
				strSqlQuery += "LOL.GAV_Beruf = '' And "
			Else
				strSqlQuery += "(@beruf = '' Or LOL.GAV_Beruf = @beruf) And "
			End If
			If Gruppe1 = "Leere Felder" Then
				strSqlQuery += "LOL.GAV_Gruppe1 = '' "
			Else
				strSqlQuery += "(@gruppe1 = '' Or LOL.GAV_Gruppe1 = @gruppe1) "
			End If
			strSqlQuery += "WHERE LOL.MDNr = @MDNr And "
			strSqlQuery += "LA.LAJahr Between @jahrVon And @jahrBis And "
			strSqlQuery += "LA.AGLA = 0 And "
			strSqlQuery += "LA.NoListing = 0 "
			strSqlQuery += "GROUP BY LA.LANR, LA.LALOText "
			strSqlQuery += "ORDER BY LA.LANR "

			Dim i As Integer = 0
			Dim iWidth As Integer = 0

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			Dim pBeruf As SqlParameter = New SqlParameter("@beruf", SqlDbType.NVarChar, 100)
			Dim pGruppe1 As SqlParameter = New SqlParameter("@gruppe1", SqlDbType.NVarChar, 100)
			Dim pJahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
			Dim pJahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
			Dim pMonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
			Dim pMonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
			pBeruf.Value = Beruf
			pGruppe1.Value = Gruppe1
			pJahrVon.Value = Val(JahrVon)
			pJahrBis.Value = Val(JahrBis)
			pMonatVon.Value = Val(MonatVon)
			pMonatBis.Value = Val(MonatBis)
			cmd.Parameters.Add(pBeruf)
			cmd.Parameters.Add(pGruppe1)
			cmd.Parameters.Add(pJahrVon)
			cmd.Parameters.Add(pJahrBis)
			cmd.Parameters.Add(pMonatVon)
			cmd.Parameters.Add(pMonatBis)

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader                  '

			cbo.Properties.Items.Clear()
			While rFOPrec.Read

				Try

					strValue = String.Format("{0} {1}", String.Format("{0:#####.0000}", CDec(rFOPrec("LANR").ToString)), rFOPrec("LALOText").ToString)
					cbo.Properties.Items.Add(strValue, CheckState.Unchecked, True)

				Catch ex As Exception

				End Try

				i += 1
			End While
			cbo.Properties.SeparatorChar = CChar(", ")


		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("ListLohnarten", ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region

	Function CheckIfRunning(ByVal proccessname As String) As Boolean

		For Each clsProcess As Process In Process.GetProcesses()
			If clsProcess.ProcessName.ToLower.Contains(proccessname.ToLower) Then
				Return True
			End If
		Next

		Return False

	End Function



#Region "Funktion zur Ermittlung der BVG-Beginn..."

#End Region


End Module
