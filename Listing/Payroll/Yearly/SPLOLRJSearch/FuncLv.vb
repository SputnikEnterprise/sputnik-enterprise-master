
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
Imports SPLOLRJSearch.ClsDataDetail

Imports SP.Infrastructure.Logging


Module FuncLv

	Private m_Logger As ILogger = New Logger()


#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As Integer?
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strSqlQuery As String = "Select LO.Jahr From LO Where LO.Jahr > 2006 "
		strSqlQuery += "And MDNr = @MDNr And Jahr Is Not Null Group By LO.Jahr Order By LO.Jahr DESC"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utility.SafeGetInteger(reader, "Jahr", Nothing)

				cbo.Properties.Items.Add(strEntry)

			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub


#End Region


#Region "Allgemeine Funktionen"

	'Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
	'  Try
	'    Dim lstStuff As ListViewItem = New ListViewItem()
	'    Dim lvwColumn As ColumnHeader
	'    With Lv
	'      .Clear()

	'      ' Nr;Nummer;Name;Strasse;PLZ Ort
	'      If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
	'      If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

	'      Dim strCaption As String() = Regex.Split(strColumnList, ";")
	'      ' 0-1;0-1;2000-0;2000-0;2500-0
	'      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
	'      Dim strFieldWidth As String
	'      Dim strFieldAlign As String = "0"
	'      Dim strFieldData As String()

	'      For i = 0 To strCaption.Length - 1
	'        lvwColumn = New ColumnHeader()
	'        lvwColumn.Text = strCaption(i).ToString
	'        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

	'        If strFieldInfo(i).ToString.StartsWith("-") Then
	'          strFieldWidth = strFieldData(1)
	'          lvwColumn.Width = CInt(strFieldWidth) * -1
	'          If strFieldData.Count > 1 Then
	'            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
	'          End If
	'        Else
	'          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
	'          lvwColumn.Width = CInt(strFieldWidth)   '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
	'          If strFieldData.Count > 1 Then
	'            strFieldAlign = strFieldData(1)
	'          End If
	'          'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
	'        End If
	'        If strFieldAlign = "1" Then
	'          lvwColumn.TextAlign = HorizontalAlignment.Right
	'        ElseIf strFieldAlign = "2" Then
	'          lvwColumn.TextAlign = HorizontalAlignment.Center
	'        Else
	'          lvwColumn.TextAlign = HorizontalAlignment.Left

	'        End If

	'        lstStuff.BackColor = Color.Yellow

	'        .Columns.Add(lvwColumn)
	'      Next

	'      lvwColumn = Nothing
	'    End With
	'  Catch ex As Exception ' Manager
	'    _ClsErrException.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "FillDataHeaderLv", ex)
	'  End Try


	'End Sub



	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function


#End Region


	'#Region "Funktionen zur Übersetzung..."

	'	Function TranslateMyText(ByVal strBez As String) As String
	'		Dim strOrgText As String = strBez
	'		Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
	'		Dim _clsLog As New SPProgUtility.ClsEventLog

	'		If _ClsProgSetting.GetLogedUSNr = 1 Then
	'			_clsLog.WriteTempLogFile(String.Format("Progbez: {0}{1}{0} Translatedbez: {0}{2}{0}", _
	'																	Chr(34), strBez, strTranslatedText), _
	'																_ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
	'		End If

	'		Return strTranslatedText
	'	End Function

	'	Function TranslateMyText(ByVal strFuncName As String, _
	'													 ByVal strOrgControlBez As String, _
	'													 ByVal strBez As String) As String
	'		Dim strOrgText As String = strBez
	'		Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
	'		Dim _clsLog As New SPProgUtility.ClsEventLog

	'		If _ClsProgSetting.GetLogedUSNr = 1 Then
	'			_clsLog.WriteTempLogFile(String.Format("{1}: Progbez: {0}{2}{0} Namedbez: {0}{3}{0}, Translatedbez: {0}{4}{0}", _
	'																	Chr(34), strFuncName, strOrgControlBez, strBez, strTranslatedText), _
	'																_ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
	'		End If

	'		Return strTranslatedText
	'	End Function

	'#End Region

End Module
