
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
  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  'Dim _ClsSystem As New SPProgUtility.ClsMain_Net
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Dim strMDPath As String = ""
  Dim strInitPath As String = ""

  Dim iLogedUSNr As Integer = 0

  Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

#Region "Dropdown-Funktionen"

	Sub ListZoom(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(100)
		cbo.Properties.Items.Add(150)
		cbo.Properties.Items.Add(200)
		cbo.Properties.Items.Add(300)
		cbo.Properties.Items.Add(400)

	End Sub

#End Region

#Region "Sonstige Funktions..."

	'Private Sub SetComboBoxWidth(ByRef cbo As myCbo)
	'  Dim iWidth As Integer = 0
	'  For Each cboItem As ComboBoxItem In cbo.Items
	'    iWidth = CInt(IIf(iWidth > cboItem.Text.Length, iWidth, cboItem.Text.Length))
	'  Next
	'  cbo.DropDownWidth = CInt((iWidth * 7) + 20)
	'End Sub

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

        lstStuff.BackColor = Color.Yellow

        .Columns.Add(lvwColumn)
      Next

      lvwColumn = Nothing
    End With

  End Sub

#End Region


End Module
