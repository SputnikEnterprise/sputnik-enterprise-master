
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPBruttolohnjournal.ClsDataDetail


Module FuncLv
  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_md As New Mandant
	Private m_Utility As New SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As New SP.Infrastructure.UI.UtilityUI


#Region "Dropdown-Funktionen für 1. Seite..."

  ' Jahr ---------------------------------------------------------------------------------------------
	Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "Select LO.Jahr From LO "
		strSqlQuery += "Group By LO.Jahr Order By LO.Jahr DESC"


		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader									'

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				strEntry = rFOPrec("Jahr").ToString
				cbo.Properties.Items.Add(New ComboBoxItem(rFOPrec("Jahr").ToString, strEntry))
				iWidth = CInt(IIf(iWidth > CInt(Len(strEntry).ToString), iWidth, CInt(Len(strEntry).ToString)))

				i += 1
			End While

			cbo.EditValue = Date.Now.Year


		Catch e As Exception
			m_UtilityUi.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

  ' Berater ---------------------------------------------------------------------------------------------
	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal filiale As String)
		Dim strEntry As String
		Dim strSqlQuery As String = "[List Benutzer]"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@filiale", filiale)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rESrec.Read
				strEntry = String.Format("{0}, {1} ({2})", rESrec("Nachname").ToString, rESrec("Vorname"), rESrec("KST"))
				cbo.Properties.Items.Add(New ComboBoxItem(strEntry, rESrec("KST").ToString))
				iWidth = CInt(IIf(iWidth > CInt(Len(strEntry).ToString), iWidth, CInt(Len(strEntry).ToString)))

				i += 1
			End While

		Catch e As Exception
			m_UtilityUi.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Filialen ---------------------------------------------------------------------------------------------
	Sub ListUSFilialen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monatVon As String, ByVal monatBis As String, _
										 ByVal jahrVon As String, ByVal jahrBis As String, ByVal kst As String)
		Dim strValue As String
		Dim sql As String

		'sql = "SELECT MA.KST "
		'sql &= "INTO #tblKST FROM LOL "
		'sql &= "Left Join Mitarbeiter MA On MA.MANr = LOL.MANr "
		'sql &= "WHERE "
		'sql += "LOL.MDNr = @MDNr And LOL.LANR < 7000 And "
		'sql += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or"
		'sql += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
		'sql += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))))"
		'sql += "And (@kst = '' Or MA.KST = @kst) "
		'sql += "GROUP BY MA.KST; "

		'sql += "SELECT Benutzer.USFiliale FROM #tblKST "
		'sql += "INNER JOIN Benutzer ON Benutzer.KST = #tblKST.KST "

		''sql += "Benutzer.USFiliale <> '' And Benutzer.USFiliale Is Not Null And "
		''sql += "(Benutzer.KST = #tblKST.KST Or "
		''sql += " Benutzer.KST = Substring(#tblKST.KST, 0, CHARINDEX('/', #tblKST.KST)) Or "
		''sql += " Benutzer.KST = Substring(#tblKST.KST, CHARINDEX('/', #tblKST.KST), LEN(#tblKST.KST))) "
		'sql += "GROUP BY Benutzer.USFiliale "
		'sql += "Order BY Benutzer.USFiliale"

		sql = "[List All UserBranches For Listing In Payroll Lists]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@kst", kst)
			cmd.Parameters.AddWithValue("@monatVon", monatVon)
			cmd.Parameters.AddWithValue("@monatBis", monatBis)
			cmd.Parameters.AddWithValue("@jahrVon", jahrVon)
			cmd.Parameters.AddWithValue("@jahrBis", jahrBis)

			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.Properties.Items.Clear()
			While rFoundedrec.Read
				strValue = rFoundedrec("Filiale").ToString

				cbo.Properties.Items.Add(New ComboBoxItem(strValue, m_Utility.SafeGetString(rFoundedrec, "Filiale")))

			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			m_UtilityUi.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#End Region


#Region "Allgemeine Funktionen"

  Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
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

  End Sub

	Sub ListCboAktiviertNichtAktiviert(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem("Nicht aktiviert", "0"))
		cbo.Properties.Items.Add(New ComboBoxItem("Aktiviert", "1"))
	End Sub

	Sub ListCboVollstaendigNichtVoll(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem("Nicht vollständig", "0"))
		cbo.Properties.Items.Add(New ComboBoxItem("Vollständig", "1"))
	End Sub

	Sub ListJaNein(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem("Nein", "0"))
		cbo.Properties.Items.Add(New ComboBoxItem("Ja", "1"))
	End Sub

	Sub ListCboMonate1Bis12(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		For i As Integer = 1 To 12
			cbo.Properties.Items.Add(New ComboBoxItem(i.ToString, i.ToString))
		Next
	End Sub

	Sub ListLohnarten(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem(m_Translate.GetSafeTranslationValue("Brutto"), "7000"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_Translate.GetSafeTranslationValue("Suva-Basis"), "7302"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_Translate.GetSafeTranslationValue("AHV-Basis"), "7100"))

		For i As Integer = 0 To cbo.Properties.Items.Count - 1
			cbo.Properties.Items(i).CheckState = CheckState.Checked
		Next

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

End Module
