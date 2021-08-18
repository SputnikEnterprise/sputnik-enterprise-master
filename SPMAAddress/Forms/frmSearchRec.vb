
'Imports System.IO
'Imports System.Data.SqlClient

'Public Class frmSearchRec
'  Inherits DevExpress.XtraEditors.XtraForm

'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsData As New ClsDivFunc

'  Dim strConnString As String = _ClsProgSetting.GetConnString()
'  Dim iLogedUSNr As Integer = 0
'  Dim bAllowedtowrite As Boolean

'  Public ReadOnly Property iKDValue(ByVal strValue As String) As String

'    Get
'      Dim strBez As String = String.Empty

'      If Me.LvData.SelectedItems.Count > 0 Then

'        If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then
'          strBez = _ClsData.GetKDNr

'        ElseIf ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_Country Then
'          strBez = _ClsData.GetKDName

'        Else
'          strBez = _ClsData.GetKDName

'        End If

'        If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then
'          Me.LblChanged.Text = strBez                   ' PLZ

'        ElseIf ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_Country Then
'          Me.LblChanged.Text = strBez                   ' Land


'        End If
'      End If

'      ClsDataDetail.strKDData = CStr(Me.LblChanged.Text)

'      Return ClsDataDetail.strKDData
'    End Get

'  End Property

'  Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

'    SetLvwHeader()
'    Me.LblChanged.Text = String.Empty

'    FillLvData(Me.txtSearchValue.Text)
'    bAllowedtowrite = True

'    ' Set Focus to Textbox
'    Me.txtSearchValue.Focus()

'    Dim _ClsXML As New ClsXML
'    Dim Time_1 As Double = System.Environment.TickCount
'    _ClsXML.GetChildChildBez(Me)
'    Dim Time_2 As Double = System.Environment.TickCount
'    Trace.WriteLine("1. Verbrauchte Zeit: " & ((Time_2 - Time_1) / 1000) & " s.")

'  End Sub

'  Sub SetLvwHeader()
'    Dim strColumnString As String = String.Empty
'    Dim strColumnWidthInfo As String = String.Empty
'    Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()

'    If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then
'      Dim strQuery As String = "//LV_Control[@Name=" & Chr(34) & _
'                              ClsDataDetail.GetAppGuidValue() & "_1" & Chr(34) & "]/HeaderString" & strUSLang
'      Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
'      If strBez <> String.Empty Then
'        strColumnString = strBez

'      Else
'        strColumnString = "PLZ;Ort;Kanton"

'      End If

'      strQuery = "//LV_Control[@Name=" & Chr(34) & _
'                              ClsDataDetail.GetAppGuidValue() & "_1" & Chr(34) & "]/HeaderWidth" & strUSLang
'      strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
'      If strBez <> String.Empty Then
'        strColumnWidthInfo = strBez

'      Else
'        strColumnWidthInfo = "100-1;200-0;100-0"
'        If My.Settings.LV_2_Size <> String.Empty Then strColumnWidthInfo = My.Settings.LV_2_Size

'      End If

'    ElseIf ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_Country Then
'      Dim strQuery As String = "//LV_Control[@Name=" & Chr(34) & _
'                              ClsDataDetail.GetAppGuidValue() & "_2" & Chr(34) & "]/HeaderString" & strUSLang
'      Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
'      If strBez <> String.Empty Then
'        strColumnString = strBez

'      Else
'        strColumnString = "Code;Land"

'      End If

'      strQuery = "//LV_Control[@Name=" & Chr(34) & _
'                              ClsDataDetail.GetAppGuidValue() & "_2" & Chr(34) & "]/HeaderWidth" & strUSLang
'      strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
'      If strBez <> String.Empty Then
'        strColumnWidthInfo = strBez

'      Else
'        strColumnWidthInfo = "100-0;400-0"
'        If My.Settings.LV_3_Size <> String.Empty Then strColumnWidthInfo = My.Settings.LV_3_Size
'      End If

'    End If
'    strColumnString = TranslateMyText(strColumnString)
'    FillDataHeaderLv(Me.LvData, strColumnString, strColumnWidthInfo)

'  End Sub

'  Sub FillLvData(ByVal strFieldValue As String)
'    Dim strOperator As String = "="
'    Dim strSqlQuery As String = ""
'    Dim i As Integer = 0
'    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'    Dim str1Param As String = String.Empty
'    Dim str2Param As String = String.Empty
'    Dim str3Param As String = String.Empty

'    Try
'      Conn.Open()

'      Dim cmd As System.Data.SqlClient.SqlCommand

'      If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then
'        strSqlQuery = "[List All Plz]"
'        str1Param = "@Value2Search"
'        str2Param = "@FieldID"

'      ElseIf ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_Country Then
'        strSqlQuery = "[List All Countries]"
'        str1Param = "@Value2Search"

'      End If

'      cmd = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
'      cmd.CommandType = CommandType.StoredProcedure
'      Dim param As System.Data.SqlClient.SqlParameter

'      param = cmd.Parameters.AddWithValue(str1Param, "%" & Me.txtSearchValue.Text & "%")
'      If str2Param <> String.Empty Then
'        param = cmd.Parameters.AddWithValue(str2Param, IIf(IsNumeric(Me.txtSearchValue.Text), "0", "1"))
'      End If

'      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader          ' Datenbank
'      Me.LvData.Items.Clear()
'      Me.LvData.FullRowSelect = True
'      Me.LvData.MultiSelect = False

'      While rFoundedrec.Read

'        If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then
'          Me.LvData.Items.Add(rFoundedrec("PLZ").ToString)
'          If Not String.IsNullOrEmpty(rFoundedrec("PLZ").ToString) Then
'            Me.LvData.Items(i).SubItems.Add(rFoundedrec("Ort").ToString)
'            Me.LvData.Items(i).SubItems.Add(rFoundedrec("Kanton").ToString)
'          End If

'        ElseIf ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_Country Then
'          Me.LvData.Items.Add(rFoundedrec("Code").ToString)
'          If Not String.IsNullOrEmpty(rFoundedrec("Code").ToString) Then
'            Me.LvData.Items(i).SubItems.Add(rFoundedrec("Land").ToString)
'          End If

'        End If
'        i += 1

'      End While

'    Catch e As Exception
'      Me.LvData.Items.Clear()
'      MsgBox(e.Message)

'    Finally
'      Conn.Close()
'      Conn.Dispose()

'    End Try

'  End Sub

'  Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
'    Me.Close()
'  End Sub

'  Private Sub LvData_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles LvData.ColumnWidthChanged
'    If bAllowedtowrite Then SaveLV_ColumnInfo()
'  End Sub

'  Private Sub LvData_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LvData.DoubleClick
'    cmdOK_Click(sender, e)
'  End Sub

'  Private Sub LvData_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LvData.MouseClick
'    Dim strValue As String = String.Empty
'    Dim strBez As String = String.Empty

'    If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then

'      For i As Integer = 0 To LvData.Items.Count - 1
'        If LvData.Items(i).Selected = True Then
'          strBez += String.Format("{0}#{1}", LvData.Items(i).SubItems(0).Text, _
'                                 LvData.Items(i).SubItems(1).Text) & "#@"
'        End If

'      Next
'      If strBez.EndsWith("#@") Then strBez = Mid(strBez, 1, Len(strBez) - 2)
'      _ClsData.GetKDNr = strBez

'    Else
'      For i As Integer = 0 To LvData.Items.Count - 1
'        If LvData.Items(i).Selected Then
'          strBez += LvData.Items(i).SubItems(0).Text & "#@"
'        End If

'      Next

'      If strBez.EndsWith("#@") Then strBez = Mid(strBez, 1, Len(strBez) - 2)
'      _ClsData.GetKDName = strBez
'      Trace.WriteLine(strBez)

'    End If
'    Me.LblChanged.Text = strBez

'  End Sub

'  Private Sub txtSearchValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchValue.KeyPress

'    If Asc(e.KeyChar) = Keys.Enter Then
'      FillLvData(Me.txtSearchValue.Text)
'    End If

'  End Sub

'  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
'    ClsDataDetail.strButtonValue = String.Empty
'    Me.Close()
'    Me.Dispose()
'  End Sub

'  Sub SaveLV_ColumnInfo()
'    Try
'      Dim strColInfo As String = String.Empty
'      Dim strColInfo_1 As String = String.Empty
'      Dim strColAlign As String = String.Empty

'      For i As Integer = 0 To LvData.Columns.Count - 1
'        If LvData.Columns.Item(i).TextAlign = HorizontalAlignment.Center Then
'          strColAlign = "2"

'        ElseIf LvData.Columns.Item(i).TextAlign = HorizontalAlignment.Right Then
'          strColAlign = "1"
'        Else
'          strColAlign = "0"
'        End If

'        strColInfo &= CStr(IIf(strColInfo = String.Empty, "", ";")) & (LvData.Columns.Item(i).Width) & "-" & strColAlign

'      Next

'      Try
'        If ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_PLZ Then
'          My.Settings.LV_2_Size = strColInfo
'        ElseIf ClsDataDetail.SelectedSearchModul = ClsDataDetail.SearchModul.Search_Country Then
'          My.Settings.LV_3_Size = strColInfo
'        End If

'        Trace.WriteLine(strColInfo)
'        My.Settings.Save()

'      Catch ex As Exception
'        ' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
'      End Try

'    Catch ex As Exception ' Manager
'      MsgBox(String.Format("Fehler_SaveLV_ColumnInfo_0: {0}", ex.Message))

'    End Try

'  End Sub

'End Class