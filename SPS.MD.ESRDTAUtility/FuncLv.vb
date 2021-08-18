
'Imports System.Data.SqlClient
'Imports System.IO
'Imports System.Text.RegularExpressions
'Imports System.Reflection

'Imports System
'Imports System.Drawing
'Imports System.Collections
'Imports System.ComponentModel
'Imports System.Windows.Forms
'Imports System.Data

'Imports SPS.MD.ESRDTAUtility.ClsDataDetail



'Module FuncLv
'  Dim _ClsFunc As New ClsDivFunc



'#Region "Sonstige Funktions..."

'	'Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
'	'	Dim lstStuff As ListViewItem = New ListViewItem()
'	'	Dim lvwColumn As ColumnHeader

'	'	With Lv
'	'		.Clear()

'	'		' Nr;Nummer;Name;Strasse;PLZ Ort
'	'		If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
'	'		If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

'	'		Dim strCaption As String() = Regex.Split(strColumnList, ";")
'	'		' 0-1;0-1;2000-0;2000-0;2500-0
'	'		Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
'	'		Dim strFieldWidth As String
'	'		Dim strFieldAlign As String = "0"
'	'		Dim strFieldData As String()

'	'		For i = 0 To strCaption.Length - 1
'	'			lvwColumn = New ColumnHeader()
'	'			lvwColumn.Text = strCaption(i).ToString
'	'			strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

'	'			If strFieldInfo(i).ToString.StartsWith("-") Then
'	'				strFieldWidth = strFieldData(1)
'	'				lvwColumn.Width = CInt(strFieldWidth) * -1
'	'				If strFieldData.Count > 1 Then
'	'					strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
'	'				End If
'	'			Else
'	'				strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
'	'				lvwColumn.Width = CInt(strFieldWidth)	'* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
'	'				If strFieldData.Count > 1 Then
'	'					strFieldAlign = strFieldData(1)
'	'				End If
'	'			End If
'	'			If strFieldAlign = "1" Then
'	'				lvwColumn.TextAlign = HorizontalAlignment.Right
'	'			ElseIf strFieldAlign = "2" Then
'	'				lvwColumn.TextAlign = HorizontalAlignment.Center
'	'			Else
'	'				lvwColumn.TextAlign = HorizontalAlignment.Left

'	'			End If
'	'			lstStuff.BackColor = Color.Yellow
'	'			.Columns.Add(lvwColumn)
'	'		Next

'	'		lvwColumn = Nothing
'	'	End With

'	'End Sub

'	'Sub FillFoundedData(ByVal frmTest As frmESRDTA, ByVal Lv As ListView, ByVal iRecNr As Integer, _
'	'										ByVal bSelectAsDTA As Boolean,
'	'										ByVal iMDNr As Integer)
'	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
'	'	'm_InitialData.MDData.MDDbConn

'	'	Dim i As Integer = 0
'	'	Dim _ClsDb As New ClsDbFunc
'	'	Dim strQuery As String = String.Empty

'	'	Try
'	'		Conn.Open()
'	'		Dim cmd As System.Data.SqlClient.SqlCommand
'	'		strQuery = _ClsDb.GetLocalSQLString(0, bSelectAsDTA)
'	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'		Dim param As System.Data.SqlClient.SqlParameter
'	'		If iRecNr > 0 Then
'	'			param = cmd.Parameters.AddWithValue("@iRecID", iRecNr)
'	'		End If
'	'		param = cmd.Parameters.AddWithValue("@MDNr", iMDNr)

'	'		Dim rAdressrec As SqlDataReader = cmd.ExecuteReader					 ' Bankdaten
'	'		Lv.Items.Clear()
'	'		Lv.FullRowSelect = True

'	'		Dim Time_1 As Double = System.Environment.TickCount

'	'		Lv.BeginUpdate()
'	'		While rAdressrec.Read
'	'			With Lv
'	'				.Items.Add(rAdressrec("ID").ToString)
'	'				.Items(i).SubItems.Add(rAdressrec("RecNr").ToString)
'	'				If bSelectAsDTA Then
'	'					.Items(i).SubItems.Add(rAdressrec("KontoDTA").ToString)
'	'				Else
'	'					.Items(i).SubItems.Add(rAdressrec("KontoESR1").ToString)
'	'				End If
'	'				.Items(i).SubItems.Add(rAdressrec("RecBez").ToString)

'	'			End With
'	'			If CInt(rAdressrec("RecNr")) = iRecNr Then DisplayFoundedData(frmTest, iRecNr, bSelectAsDTA, iMDNr)
'	'			i += 1
'	'		End While
'	'		Lv.EndUpdate()
'	'		Console.WriteLine(String.Format("Zeit für FillFoundedData: {0} s", _
'	'																		((System.Environment.TickCount - Time_1) / 1000).ToString()))


'	'	Catch e As Exception
'	'		Lv.Items.Clear()
'	'		MsgBox(e.Message, MsgBoxStyle.Critical, "FillFoundedData")

'	'	Finally
'	'		Conn.Close()
'	'		Conn.Dispose()

'	'	End Try

'	'End Sub


'#End Region

'End Module
