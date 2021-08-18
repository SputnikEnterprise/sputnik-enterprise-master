
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

Module FuncLv
	'Dim _ClsFunc As New ClsDivFunc
	'Dim _ClsReg As New SPProgUtility.ClsDivReg
	' Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListQSTListeJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
		Dim strSqlQuery As String = "Select MD.Jahr From Mandanten MD "
		strSqlQuery += "Group By MD.Jahr Order By MD.Jahr DESC"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rFOPrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rFOPrec.Read
        strEntry = rFOPrec("Jahr").ToString
        cbo.Properties.Items.Add(New ComboValue(rFOPrec("Jahr").ToString, strEntry))
      End While
      cbo.Properties.DropDownRows = 10

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

	Sub ListQSTListLohnarten(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, _
													 ByVal Jahr As Integer?, _
													 ByVal LPVon As Integer?, _
													 ByVal LPBis As Integer?)
		Dim strText As String
		Dim strValue As String
		Dim strSqlQuery As String = "[Show LAData For Search In QSteuer With Mandant]"
		Dim m_Utility As New SPProgUtility.MainUtilities.Utilities

		Dim i As Integer = 0
		Dim existsGrenzgaenger As Boolean = False
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@MDYear", ReplaceMissing(Jahr, Now.Year))
			cmd.Parameters.AddWithValue("@MonatVon", ReplaceMissing(LPVon, Now.Month))
			cmd.Parameters.AddWithValue("@MonatBis", ReplaceMissing(LPBis, Now.Month))

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader									'

			Dim allValues As String = ""
			cbo.Properties.Items.Clear()
			cbo.Text = ""
			While rFOPrec.Read
				If m_Utility.SafeGetBoolean(rFOPrec, "Q_Steuer", False) Then
					existsGrenzgaenger = True
				End If
				strText = String.Format("{0:0000} {1}", rFOPrec("LANR"), rFOPrec("LALOText"))
				strValue = String.Format("{0:0000}", rFOPrec("LANR"))
				allValues += String.Format("{0},", strValue)
				cbo.Properties.Items.Add(New ComboValue(strText, strValue))

				i += 1
			End While
			If CInt(Jahr) >= 2014 AndAlso existsGrenzgaenger Then
				cbo.Properties.Items.Add(New ComboValue("7620 Quellensteuer Deutschland", "7620"))
			End If

			If cbo.Properties.Items.Count > 0 Then
				allValues = allValues.Substring(0, allValues.Length - 1)
				cbo.Properties.Items.Insert(0, New ComboValue("Alle", allValues))
				cbo.SelectedIndex = 0
			End If

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

  Sub ListMonth4LOLists(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Jahr As String)
    Dim strValue As String = String.Empty
    Dim strSqlQuery As String = "[Show LOMonth For Search In LoLists]"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      cmd.Parameters.AddWithValue("@MDYear", Jahr)

      Dim rLOrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      While rLOrec.Read
        strValue = String.Format("{0}", rLOrec("LP"))
        cbo.Properties.Items.Add(strValue)
      End While
      cbo.Properties.DropDownRows = 13


    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

	Sub ListQSTListeKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, _
												 ByVal jahr As String, ByVal mvon As String, _
												 ByVal mbis As String, ByVal lanr As String)

		' Sicherheitskontrolle
		If lanr.Trim().Length = 0 Then Exit Sub

		Dim strEntry As String
		Dim strSqlQuery As String = "Select LOL.S_Kanton From LOL "
		strSqlQuery += String.Format("Where LOL.MDNr = {0} And LOL.Jahr = {1} And ", ClsDataDetail.m_InitialData.MDData.MDNr, jahr)
		strSqlQuery += String.Format("LOL.LP Between {0} And {1} And ", mvon, mbis)
		strSqlQuery += "LOL.S_Kanton <> '' And LOL.S_Kanton Is Not Null And "
		strSqlQuery += String.Format("LOL.LANR In ({0}) And ", lanr)
		strSqlQuery += "LOL.M_BTR <> 0 "
		strSqlQuery += "Group By LOL.S_Kanton Order By LOL.S_Kanton"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader									'

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				strEntry = rFOPrec("S_Kanton").ToString
				cbo.Properties.Items.Add(New ComboValue(rFOPrec("S_Kanton").ToString, strEntry))
			End While
			If ExistSKanton4VD(lanr) Then
				cbo.Properties.Items.Add("")
				cbo.Properties.Items.Add("VD")
			End If
			cbo.Properties.DropDownRows = 27


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub


	' Kanton VD ---------------------------------------------------------------------------------------------
	Function ExistSKanton4VD(ByVal lanr As String) As Boolean
    If lanr.Trim().Length = 0 Then Exit Function

    Dim strEntry As String = String.Empty
    Dim strSqlQuery As String = "Select Top 1 LOL.S_Kanton From LOL "
		strSqlQuery += String.Format("Where LOL.MDNr = {0} And LOL.S_Kanton = 'VD' And ", ClsDataDetail.m_InitialData.MDData.MDNr)
    strSqlQuery += String.Format("LOL.LANR In ({0}) ", lanr)
    strSqlQuery += "Group By LOL.S_Kanton Order By LOL.S_Kanton"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim rFOPrec As SqlDataReader = cmd.ExecuteReader                  '

      rFOPrec.Read()
      If rFOPrec.HasRows Then strEntry = "VD"


    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return strEntry.Trim.Length > 0
  End Function

  ' Gemeinde ---------------------------------------------------------------------------------------------
  Sub ListQSTListeGemeinde(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal jahr As String, ByVal mvon As String, ByVal mbis As String, ByVal lanr As String, ByVal kanton As String)
    Dim strEntry As String
    Dim strSqlQuery As String = "Select LOL.QSTGemeinde From LOL "
    strSqlQuery += String.Format("Where LOL.Jahr = {0} And ", jahr)
		strSqlQuery += String.Format("LOL.MDNr = {0} And ", ClsDataDetail.m_InitialData.MDData.MDNr)
		strSqlQuery += String.Format("LOL.LP Between {0} And {1} And ", mvon, mbis)
    strSqlQuery += "LOL.QSTGemeinde <> '' And LOL.QSTGemeinde Is Not Null And "
    strSqlQuery += String.Format("LOL.LANR In ({0}) And ", lanr)
    strSqlQuery += String.Format("('{0}' = '' Or LOL.S_Kanton = '{0}') And ", kanton)
    strSqlQuery += "LOL.M_BTR <> 0 "
    strSqlQuery += "Group By LOL.QSTGemeinde Order By LOL.QSTGemeinde"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rFOPrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      While rFOPrec.Read
        strEntry = rFOPrec("QSTGemeinde").ToString
        cbo.Properties.Items.Add(New ComboValue(rFOPrec("QSTGemeinde").ToString, strEntry))
      End While
      cbo.Properties.DropDownRows = 10

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

#End Region

#Region "Allgemeine Funktionen"

	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

  Function ReplaceEmpty(ByVal textValue As String, ByVal replacementValue As Integer) As Integer

    Dim parsedValue As Integer
    If Integer.TryParse(textValue, parsedValue) Then
      Return parsedValue
    Else
      Return replacementValue
    End If

  End Function


  ' Helps extracting a column value form a data reader.
  Function GetColumnTextStr(ByVal dr As SqlDataReader, _
                                          ByVal columnName As String, _
                                          ByVal replacementOnNull As String) As String

    If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
      Return CStr(dr(columnName))
    End If

    Return replacementOnNull
  End Function

	'Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
	'  Dim lstStuff As ListViewItem = New ListViewItem()
	'  Dim lvwColumn As ColumnHeader

	'  With Lv
	'    .Clear()

	'    ' Nr;Nummer;Name;Strasse;PLZ Ort
	'    If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
	'    If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

	'    Dim strCaption As String() = Regex.Split(strColumnList, ";")
	'    ' 0-1;0-1;2000-0;2000-0;2500-0
	'    Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
	'    Dim strFieldWidth As String
	'    Dim strFieldAlign As String = "0"
	'    Dim strFieldData As String()

	'    For i = 0 To strCaption.Length - 1
	'      lvwColumn = New ColumnHeader()
	'      lvwColumn.Text = strCaption(i).ToString
	'      strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

	'      If strFieldInfo(i).ToString.StartsWith("-") Then
	'        strFieldWidth = strFieldData(1)
	'        lvwColumn.Width = CInt(strFieldWidth) * -1
	'        If strFieldData.Count > 1 Then
	'          strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
	'        End If
	'      Else
	'        strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
	'        lvwColumn.Width = CInt(strFieldWidth)   '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
	'        If strFieldData.Count > 1 Then
	'          strFieldAlign = strFieldData(1)
	'        End If
	'        'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
	'      End If
	'      If strFieldAlign = "1" Then
	'        lvwColumn.TextAlign = HorizontalAlignment.Right
	'      ElseIf strFieldAlign = "2" Then
	'        lvwColumn.TextAlign = HorizontalAlignment.Center
	'      Else
	'        lvwColumn.TextAlign = HorizontalAlignment.Left

	'      End If

	'      lstStuff.BackColor = Color.Yellow

	'      .Columns.Add(lvwColumn)
	'    Next

	'    lvwColumn = Nothing
	'  End With

	'End Sub

  ' Aktiviert - Nicht aktiviert
	'Sub ListCboAktiviertNichtAktiviert(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
	'  cbo.Properties.Items.Add(New ComboValue("", ""))
	'  cbo.Properties.Items.Add(New ComboValue("Nicht aktiviert", "0"))
	'  cbo.Properties.Items.Add(New ComboValue("Aktiviert", "1"))
	'End Sub
	'' Vollständig - Nicht vollständig
	'Sub ListCboVollstaendigNichtVoll(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
	'  cbo.Properties.Items.Add(New ComboValue("", ""))
	'  cbo.Properties.Items.Add(New ComboValue("Nicht vollständig", "0"))
	'  cbo.Properties.Items.Add(New ComboValue("Vollständig", "1"))
	'End Sub

	'' Ja - Nein
	'Sub ListJaNein(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
	'  cbo.Properties.Items.Add(New ComboValue("", ""))
	'  cbo.Properties.Items.Add(New ComboValue("Nein", "0"))
	'  cbo.Properties.Items.Add(New ComboValue("Ja", "1"))
	'End Sub

  ' Monate 1 bis 12
  Sub ListCboMonate1Bis12(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    For i As Integer = 1 To 12
      cbo.Properties.Items.Add(New ComboValue(i.ToString, i.ToString))
    Next
  End Sub

	Function CheckIfRunning(ByVal proccessname As String) As Boolean

		For Each clsProcess As Process In Process.GetProcesses()
			If clsProcess.ProcessName.ToLower.Contains(proccessname.ToLower) Then
				Return True
			End If
		Next

		Return False

	End Function

#End Region

End Module
