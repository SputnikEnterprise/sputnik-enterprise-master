
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

'Imports SPMAAddress.ClsDataDetail


'Module FuncLv

'	'Private logger As Logger = LogManager.GetCurrentClassLogger()

'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _ClsFunc As New ClsDivFunc
'  Dim _ClsReg As New SPProgUtility.ClsDivReg

'  Dim strConnString As String = _ClsProgSetting.GetConnString()

'#Region "Sonstige Funktions..."

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
'	'        lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
'	'        If strFieldData.Count > 1 Then
'	'          strFieldAlign = strFieldData(1)
'	'        End If
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

'	' Sub FillFoundedData(ByVal Lv As ListView, ByVal frmSource As frmMAAddress)
'	'   Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'   Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'	'   Dim i As Integer = 0
'	'   Dim _ClsDb As New ClsDbFunc

'	'   Try
'	'     Conn.Open()
'	'     Dim cmd As System.Data.SqlClient.SqlCommand
'	'     Dim strQuery As String = _ClsDb.GetLocalSQLString(frmSource, 0)

'	'     cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'     Dim param As System.Data.SqlClient.SqlParameter
'	'     param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetSelectedMANr)
'	'     param = cmd.Parameters.AddWithValue("@ModulNr", ClsDataDetail.SelectedAddressArt)

'	'     Dim rAdressrec As SqlDataReader = cmd.ExecuteReader
'	'     Lv.Items.Clear()
'	'     Lv.FullRowSelect = True

'	'     Dim Time_1 As Double = System.Environment.TickCount
'	'     Dim iID As Integer = 0

'	'     Lv.BeginUpdate()
'	'     While rAdressrec.Read
'	'       With Lv
'	'         If i = 0 Then iID = CInt(rAdressrec("ID").ToString)
'	'         .Items.Add(rAdressrec("ID").ToString)
'	'         .Items(i).SubItems.Add(rAdressrec("RecNr").ToString)
'	'         .Items(i).SubItems.Add(String.Format("{0}, {1}", rAdressrec("Nachname").ToString, _
'	'                                              rAdressrec("Vorname").ToString))
'	'         .Items(i).SubItems.Add(String.Format("{0} {1} {2}", rAdressrec("Strasse").ToString, _
'	'                                              rAdressrec("PLZ").ToString, rAdressrec("Ort").ToString))

'	'         If Not IsDBNull(rAdressrec("ActiveRec")) Then
'	'           If CBool(rAdressrec("ActiveRec")) Then
'	'             SetRowColor(Lv, i, True)
'	'           End If
'	'         End If

'	'       End With

'	'       i += 1

'	'     End While
'	'     Lv.EndUpdate()
'	'     If Lv.Items.Count = 0 Then
'	'			frmSource.LoadDefaultValues()

'	'     Else
'	'       frmSource.BlankFields()
'	'       DisplayFoundedData(frmSource, iID)
'	'       Lv.Items(0).Selected = True
'	'     End If
'	'     Console.WriteLine(String.Format("Zeit für FillFoundedData: {0} s", _
'	'                                     ((System.Environment.TickCount - Time_1) / 1000).ToString()))


'	'   Catch e As Exception
'	'     Lv.Items.Clear()
'	'     logger.Error(String.Format("{0}.{1}", strMethodeName, e.Message))
'	'     MsgBox(e.Message, MsgBoxStyle.Critical, "FillFoundedData")

'	'   Finally
'	'     Conn.Close()
'	'     Conn.Dispose()

'	'   End Try

'	' End Sub

'	' Sub SetRowColor(ByVal LV As ListView, ByVal iIndex As Integer, ByVal bDiffColor As Boolean)

'	'   For j As Integer = 0 To LV.Items(iIndex).SubItems.Count - 1
'	'     LV.Items(iIndex).BackColor = If(bDiffColor, Color.Yellow, LV.BackColor)
'	'   Next j

'	' End Sub

'	' Sub DisplayFoundedData(ByVal frmSource As frmMAAddress, ByVal iRecID As Integer)
'	'   Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'   Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'	'   Dim i As Integer = 0
'	'   Dim _ClsDb As New ClsDbFunc

'	'   Try
'	'     Conn.Open()
'	'     Dim cmd As System.Data.SqlClient.SqlCommand
'	'		Dim strQuery As String = _ClsDb.GetLocalSQLString(iRecID)
'	'     cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'     Dim param As System.Data.SqlClient.SqlParameter
'	'     param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetSelectedMANr)
'	'		'param = cmd.Parameters.AddWithValue("@ModulNr", ClsDataDetail.SelectedAddressArt)
'	'     param = cmd.Parameters.AddWithValue("@iRecID", iRecID)

'	'     Dim rAdressrec As SqlDataReader = cmd.ExecuteReader

'	'     Dim Time_1 As Double = System.Environment.TickCount

'	'     While rAdressrec.Read
'	'       With frmSource
'	'         .LblRecID.Text = rAdressrec("ID").ToString
'	'         .LblRecNr.Text = rAdressrec("RecNr").ToString
'	'				.grpAdresse.Text = String.Format(m_Translate.GetSafeTranslationValue("Adresse: {0}"), rAdressrec("RecNr").ToString)

'	'         Dim strValue As String = rAdressrec("Geschlecht").ToString
'	'         .lblGeschlecht_ID.Text = strValue
'	'         If strValue.Length = 1 Then
'	'           If strValue = "M" Then
'	'						strValue = m_Translate.GetSafeTranslationValue("männlich")
'	'					ElseIf strValue = "W" Then
'	'						strValue = m_Translate.GetSafeTranslationValue("weiblich")
'	'					End If
'	'				End If
'	'				.lueGender.Properties.NullText = strValue


'	'				.txtLastname.Text = rAdressrec("Nachname").ToString
'	'				.txtFirstname.Text = rAdressrec("Vorname").ToString

'	'				.txtCOAdress.Text = rAdressrec("Wohnt_bei").ToString
'	'				.txtPostOfficeBox.Text = rAdressrec("Postfach").ToString
'	'				.txtStreet.Text = rAdressrec("Strasse").ToString
'	'				.txt_Land.Text = rAdressrec("Land").ToString
'	'				.txt_PLZ.Text = rAdressrec("PLZ").ToString
'	'				.txtLocation.Text = rAdressrec("Ort").ToString

'	'				.txt_Bemerkung_1.Text = rAdressrec("Add_Bemerkung").ToString
'	'				.txt_Bemerkung_2.Text = rAdressrec("Add_Res1").ToString
'	'				.txt_Bemerkung_3.Text = rAdressrec("Add_Res2").ToString
'	'				.txt_Bemerkung_4.Text = rAdressrec("Add_Res3").ToString

'	'				.chk_AktivAdress.Checked = CBool(If(IsDBNull(rAdressrec("ActiveRec")), False, _
'	'																						rAdressrec("ActiveRec")))

'	'				.bsiCreated.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0:G} {1}"),
'	'																						rAdressrec("CreatedOn").ToString, rAdressrec("CreatedFrom").ToString)
'	'				.bsiChanged.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0:G} {1}"),
'	'																						rAdressrec("ChangedOn").ToString, rAdressrec("ChangedFrom").ToString)


'	'				For Each ctl As Control In .Controls
'	'					.ErrorProvider1.SetError(ctl, String.Empty)
'	'				Next

'	'			End With


'	'			i += 1

'	'		End While


'	'	Catch e As Exception
'	'		logger.Error(String.Format("{0}.{1}", strMethodeName, e.Message))
'	'		MsgBox(e.Message, MsgBoxStyle.Critical, "DisplayFoundedData")

'	'	Finally
'	'		Conn.Close()
'	'		Conn.Dispose()

'	'	End Try

'	'End Sub

'	'Function GetMADefaultData(ByVal iMANr As Integer) As List(Of String)
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	Dim liMAData As New List(Of String)
'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'	'	Dim i As Integer = 0

'	'	Try

'	'		Conn.Open()
'	'		Dim cmd As System.Data.SqlClient.SqlCommand
'	'		Dim strQuery As String = "Select Top 1 MANr, Geschlecht, Nachname, Vorname, Postfach, Wohnt_bei, "
'	'		strQuery &= "Strasse, PLZ, Ort, Land From Mitarbeiter Where MANr = @MANr"
'	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'		Dim param As System.Data.SqlClient.SqlParameter
'	'		param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetSelectedMANr)

'	'		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
'	'		rFoundedrec.Read()

'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("MANr")), String.Empty, rFoundedrec("MANr"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Geschlecht")), String.Empty, rFoundedrec("Geschlecht"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Nachname")), String.Empty, rFoundedrec("Nachname"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Vorname")), String.Empty, rFoundedrec("Vorname"))))

'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Wohnt_bei")), String.Empty, rFoundedrec("Wohnt_bei"))))

'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Postfach")), String.Empty, rFoundedrec("Postfach"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Strasse")), String.Empty, rFoundedrec("Strasse"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("PLZ")), String.Empty, rFoundedrec("PLZ"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Ort")), String.Empty, rFoundedrec("Ort"))))
'	'		liMAData.Add(CStr(If(IsDBNull(rFoundedrec("Land")), String.Empty, rFoundedrec("Land"))))

'	'		rFoundedrec.Close()


'	'	Catch ex As Exception
'	'		logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
'	'		MsgBox(String.Format("Fehler_GetMADefaultData_0: {0}", ex.Message))

'	'	End Try

'	'	Return liMAData
'	'End Function

'	Function GetCitynameFromBox(ByVal strPLZ As String) As String
'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		Dim strResult As String = String.Empty
'		Dim Conn As New SqlConnection(strConnString)

'		If strPLZ = String.Empty Then Return strResult

'		Dim strQuery As String = "Select Top 1 Ort From PLZ Where Land = 'CH' And PLZ = @Plz"

'		Try
'			Conn.Open()
'			Dim cmd As System.Data.SqlClient.SqlCommand
'			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'			Dim param As System.Data.SqlClient.SqlParameter

'			param = cmd.Parameters.AddWithValue("@Plz", strPLZ)

'			Dim rDbrec As SqlDataReader = cmd.ExecuteReader					 ' Offertendatenbank
'			While rDbrec.Read()
'				strResult = rDbrec("Ort").ToString
'				Exit While
'			End While


'		Catch ex As Exception
'			'logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
'			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetCitynameFromBox_0")

'		End Try

'		Return strResult
'	End Function

'#End Region

'#Region "Funktionen zur Übersetzung..."

'	Function TranslateMyText(ByVal strBez As String) As String
'		Dim strOrgText As String = strBez
'		Dim strTranslatedText As String = m_Translate.GetSafeTranslationValue(strBez)
'		Dim _clsLog As New SPProgUtility.ClsEventLog

'		Return strTranslatedText
'	End Function


'#End Region

'End Module
