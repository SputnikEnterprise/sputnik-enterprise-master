
'Imports System.Runtime.InteropServices

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

'Imports SPFremdListSearch.ClsDataDetail


'Public Class MyApi
'	<DllImport("user32.dll")> Public Shared Function _
'				 FindWindow(ByVal strClassName As String, ByVal strWindowName As String) As Integer
'	End Function
'End Class



'Module FuncLv
'	'Private _ClsFunc As New ClsDivFunc
'	'Private _ClsReg As New SPProgUtility.ClsDivReg
'	'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

'#Region "Dropdown-Funktionen für 1. Seite..."

'  ' Jahr ---------------------------------------------------------------------------------------------
'  Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
'    Dim strEntry As String
'    Dim strSqlQuery As String = "Select LO.Jahr From LO "
'    strSqlQuery += "Group By LO.Jahr Order By LO.Jahr DESC"


'    Dim i As Integer = 0
'    Dim iWidth As Integer = 0
'		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

'    Try
'      Conn.Open()

'      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
'      cmd.CommandType = Data.CommandType.Text

'      Dim rFOPrec As SqlDataReader = cmd.ExecuteReader                  '

'      cbo.Properties.Items.Clear()
'      While rFOPrec.Read
'        strEntry = rFOPrec("Jahr").ToString
'        cbo.Properties.Items.Add(strEntry)

'        i += 1
'      End While


'    Catch e As Exception
'      MsgBox(e.Message)

'    Finally
'      Conn.Close()
'      Conn.Dispose()

'    End Try
'  End Sub

'  ' Geschäftsstellen
'  Sub ListMAGeschäftsstellen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
'    Dim strFieldName As String = "Bezeichnung"

'    Dim strSqlQuery As String = "[GetMAFiliale]"


'    Dim i As Integer = 0
'    Dim iWidth As Integer = 0
'		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

'    Try
'      Conn.Open()

'      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
'      cmd.CommandType = Data.CommandType.StoredProcedure

'      Dim rMArec As SqlDataReader = cmd.ExecuteReader

'      cbo.Properties.Items.Clear()
'      cbo.Properties.Items.Add("Leere Felder")
'      While rMArec.Read
'        cbo.Properties.Items.Add(rMArec(strFieldName).ToString)

'        i += 1
'      End While


'    Catch e As Exception
'      MsgBox(e.Message)

'    Finally
'      Conn.Close()
'      Conn.Dispose()

'    End Try
'  End Sub


'#End Region

'#Region "Allgemeine Funktionen"

'	'Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
'	'  Dim lstStuff As ListViewItem = New ListViewItem()
'	'  Dim lvwColumn As ColumnHeader

'	'  With Lv
'	'    .Clear()

'	'    ' Nr;Nummer;Name;Strasse;PLZ Ort
'	'    If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
'	'    If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

'	'    Dim strCaption As String() = Regex.Split(strColumnList, ";")
'	'    ' 0-1;0-1;2000-0;2000-0;2500-0
'	'    Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
'	'    Dim strFieldWidth As String
'	'    Dim strFieldAlign As String = "0"
'	'    Dim strFieldData As String()

'	'    For i = 0 To strCaption.Length - 1
'	'      lvwColumn = New ColumnHeader()
'	'      lvwColumn.Text = strCaption(i).ToString
'	'      strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

'	'      If strFieldInfo(i).ToString.StartsWith("-") Then
'	'        strFieldWidth = strFieldData(1)
'	'        lvwColumn.Width = CInt(strFieldWidth) * -1
'	'        If strFieldData.Count > 1 Then
'	'          strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
'	'        End If
'	'      Else
'	'        strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
'	'        lvwColumn.Width = CInt(strFieldWidth)   '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
'	'        If strFieldData.Count > 1 Then
'	'          strFieldAlign = strFieldData(1)
'	'        End If
'	'        'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
'	'      End If
'	'      If strFieldAlign = "1" Then
'	'        lvwColumn.TextAlign = HorizontalAlignment.Right
'	'      ElseIf strFieldAlign = "2" Then
'	'        lvwColumn.TextAlign = HorizontalAlignment.Center
'	'      Else
'	'        lvwColumn.TextAlign = HorizontalAlignment.Left

'	'      End If

'	'      lstStuff.BackColor = Color.Yellow

'	'      .Columns.Add(lvwColumn)
'	'    Next

'	'    lvwColumn = Nothing
'	'  End With

'	'End Sub




'	'Function CheckIfRunning(ByVal proccessname As String) As Boolean

'	'	For Each clsProcess As Process In Process.GetProcesses()
'	'		If clsProcess.ProcessName.ToLower.Contains(proccessname.ToLower) Then
'	'			Return True
'	'		End If
'	'	Next

'	'	Return False

'	'End Function


'#End Region


'End Module
