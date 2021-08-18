
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

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPGAV.ClsDataDetail


Module FuncLv
  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  
#Region "Dropdown-Funktionen"

  Function GetXMLValueByQuery(ByVal strFilename As String, _
                          ByVal strQuery As String, _
                          ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strFilename, strQuery)

    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  ' GAV-Kantone -------------------------------------------------------------------------------
  Sub GetGAVKantone(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
    Try
      conn.Open()
      Dim cmdText As String = "SELECT * "
      cmdText += "FROM Tab_Kanton ORDER BY GetFeld "
      Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
      cmd.CommandType = CommandType.Text
      Dim reader As SqlDataReader = cmd.ExecuteReader()
      cbo.Properties.Items.Clear()
      While reader.Read()
        cbo.Properties.Items.Add(reader("GetFeld"))
      End While
      cbo.Properties.DropDownRows = 27

    Catch ex As Exception
      MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetGAVKantone")

      '_ex.MessageBoxShowError(iLogedUSNr, "GetESDataForGAV", ex)
    Finally
      conn.Close()
    End Try
  End Sub

  ' GAV -------------------------------------------------------------------------------
  Sub GetGAV(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal kdnr As Integer)
    Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
    Try
      conn.Open()
      Dim cmdText As String = ""
      If kdnr = 0 Then
        ' TODO: GetGAV ohne KDNR --> direkt aus der GAV-Datenbank via WS
      Else
        cmdText = "SELECT * FROM KD_GAVGruppe "
        cmdText += "WHERE KDNR = @kdnr ORDER BY Bezeichnung"
      End If
      Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
      cmd.CommandType = CommandType.Text
      Dim pKdnr As SqlParameter = New SqlParameter("@kdnr", SqlDbType.Int)
      pKdnr.Value = kdnr
      cmd.Parameters.Add(pKdnr)
      Dim reader As SqlDataReader = cmd.ExecuteReader()
      cbo.Properties.Items.Clear()
      While reader.Read()
        cbo.Properties.Items.Add(reader("Bezeichnung"))
      End While

    Catch ex As Exception
      MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetESDataForGAV")
      '_ex.MessageBoxShowError(iLogedUSNr, "GetESDataForGAV", ex)
    Finally
      conn.Close()
    End Try
  End Sub

  ' GAV-GRUPPE1 -------------------------------------------------------------------------------
  Sub GetGAVGruppe1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal kanton As String, ByVal gruppe0 As String)
    Dim liGruppe1 As List(Of String) = FuncWS.GetGruppe1ByKantonData(kanton, gruppe0).ToList()
    MsgBox(String.Format("{0} {1} {2}", kanton, gruppe0, liGruppe1.Count), MsgBoxStyle.Information, "GetGAVGruppe1")

    For Each gruppe1 As String In liGruppe1 'FuncWS.GetGruppe1ByKantonData(kanton, gruppe0).ToList()
      If gruppe1.Length > 0 Then
        cbo.Properties.Items.Add(gruppe1)
      End If
    Next
  End Sub

  ' GAV-GRUPPE2 -------------------------------------------------------------------------------
  Sub GetGAVGruppe2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal kanton As String, ByVal gruppe0 As String, ByVal gruppe1 As String)
    For Each gruppe2 As String In FuncWS.GetGruppe2ByKantonData(kanton, gruppe0, gruppe1).ToList()
      If gruppe2.Length > 0 Then
        cbo.Properties.Items.Add(gruppe2)
      End If

    Next
  End Sub

  ' GAV-GRUPPE3 -------------------------------------------------------------------------------
  Sub GetGAVGruppe3(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, _
                    ByVal kanton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String)
    For Each gruppe3 As String In FuncWS.GetGruppe3ByKantonData(kanton, gruppe0, gruppe1, gruppe2).ToList()
      If gruppe3.Length > 0 Then
        cbo.Properties.Items.Add(gruppe3)
      End If

    Next
  End Sub

  ' GAV-Bezeichnung -------------------------------------------------------------------------------
  Sub GetGAVBezeichnung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal kanton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String)
    For Each bezeichnung As String In FuncWS.GetGAVTextData(kanton, gruppe0, gruppe1, gruppe2, gruppe3).ToList()
      If bezeichnung.Length > 0 Then
        cbo.Properties.Items.Add(bezeichnung)
      End If
    Next

  End Sub



#End Region

#Region "Sonstige Funktions..."

	'Function GetESDataForGAV(ByVal modul As String, ByVal manr As Integer, ByVal kdnr As Integer, _
	'                         ByVal esnr As Integer, ByVal kanton As String) As String
	'  Dim strResult As New System.Text.StringBuilder
	'  Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

	'  Try
	'    conn.Open()
	'    Dim cmdText As String = "SELECT MA.Nachname, MA.Vorname, MA.GebDat, MA.Beruf, MA.QLand, MA.Bewillig, MA.Bew_Bis, "
	'    cmdText += "KD.Firma1, KD.PLZ As KDPLZ FROM Mitarbeiter MA, Kunden KD "
	'    cmdText += "WHERE MA.MANR = @manr And KD.KDNr = @KDNr"
	'    Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
	'    cmd.CommandType = CommandType.Text
	'    Dim param As System.Data.SqlClient.SqlParameter
	'    param = cmd.Parameters.AddWithValue("@manr", manr)
	'    param = cmd.Parameters.AddWithValue("@KDNr", kdnr)

	'    Dim reader As SqlDataReader = cmd.ExecuteReader()
	'    If reader.Read() Then
	'      strResult.Append(String.Format("{0}, {1}¦", reader("Vorname"), reader("Nachname")))     ' 0
	'      strResult.Append(String.Format("{0}¦", Format(CDate(reader("GebDat").ToString), "d"))) ' 1
	'      strResult.Append(String.Format("{0}¦{1}¦", reader("Bewillig"), reader("Bew_Bis"))) ' 2, 3
	'      strResult.Append(String.Format("{0}¦{1}¦", reader("Beruf"), reader("QLand"))) ' 4, 5

	'      ' DateTime.Now.Year - DateTime.Parse(reader("GebDat").ToString()).Year & "¦") ' 6
	'      strResult.Append(String.Format("{0}¦", GetSUVA_MAAlter(CDate(reader("GebDat")))))
	'      strResult.Append(String.Format("{0}¦", reader("Firma1"))) ' 7
	'      strResult.Append(String.Format("{0}¦", reader("KDPLZ"))) ' 8
	'    End If
	'    conn.Close()

	'    conn.Open()
	'    cmdText = "SELECT Kanton, GAVNumber "
	'    'cmdText += ",(SELECT Count(*) FROM KD_GAVGruppe K1 "
	'    'cmdText += " WHERE K1.KDNR = K0.KDNR) As RecCount "
	'    cmdText += "FROM KD_GAVGruppe "
	'    cmdText += "WHERE KDNR = @kdnr Group By Kanton, GAVNumber Order By GAVNumber"
	'    cmd.CommandText = cmdText
	'    Dim pKDNR As SqlParameter = New SqlParameter("@kdnr", SqlDbType.Int)
	'    cmd.Parameters.Clear()
	'    cmd.Parameters.Add(pKDNR)
	'    pKDNR.Value = kdnr
	'    reader = cmd.ExecuteReader()
	'    If reader.Read() Then
	'      If Not IsDBNull(reader("Kanton")) Then
	'        strResult.Append(String.Format("{0}¦", reader("Kanton").ToString)) ' 9
	'      End If
	'      If Not IsDBNull(reader("GAVNumber")) Then
	'        strResult.Append(String.Format("{0}", reader("GAVNumber").ToString)) ' 10
	'      End If

	'    End If


	'  Catch ex As Exception
	'    MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetESDataForGAV")
	'    '_ex.MessageBoxShowError(iLogedUSNr, "GetESDataForGAV", ex)
	'  Finally
	'    conn.Close()
	'  End Try

	'  Return strResult.ToString
	'End Function

	'Function LoadESData(ByVal manr As Integer, ByVal kdnr As Integer) As ESData
	'	Dim strResult As New System.Text.StringBuilder
	'	'Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim result As ESData = Nothing
	'	Dim m_utility As New Utilities

	'	Dim sql As String
	'	sql = "SELECT MA.Nachname, MA.Vorname, MA.GebDat, MA.Beruf, MA.QLand, MA.Bewillig, MA.Bew_Bis, "
	'	sql &= "KD.Firma1, KD.PLZ As KDPLZ FROM Mitarbeiter MA, Kunden KD "
	'	sql &= "WHERE MA.MANR = @manr And KD.KDNr = @KDNr"

	'	' Parameters
	'	Dim listOfParams As New List(Of SqlClient.SqlParameter)

	'	listOfParams.Add(New SqlClient.SqlParameter("manr", manr))
	'	listOfParams.Add(New SqlClient.SqlParameter("KDNr", kdnr))

	'	Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)


	'	If (Not reader Is Nothing) Then

	'		result = New ESData

	'		While reader.Read()

	'			result.EmployeeFirstName = m_utility.SafeGetString(reader, "Vorname")
	'			result.EmployeeLastName = m_utility.SafeGetString(reader, "Nachname")

	'			result.EmployeeGebDate = m_utility.SafeGetDateTime(reader, "GebDat", Nothing)
	'			result.EmployeeAlter = GetSUVA_MAAlter(m_utility.SafeGetDateTime(reader, "GebDat", Nothing))

	'			result.EmployeeBewilligung = m_utility.SafeGetString(reader, "Bewillig")
	'			result.EmployeeBewilligungDate = m_utility.SafeGetDateTime(reader, "Bew_Bis", Nothing)

	'			result.EmployeeBeruf = m_utility.SafeGetString(reader, "Beruf")
	'			result.EmployeeBerufLand = m_utility.SafeGetString(reader, "QLand")

	'			result.CustomerName = m_utility.SafeGetString(reader, "Firma1")
	'			result.CustomerPLZ = m_utility.SafeGetString(reader, "KDPLZ")

	'		End While

	'	End If

	'	Return result


	'	'Try
	'	'	conn.Open()
	'	'	Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
	'	'	cmd.CommandType = CommandType.Text
	'	'	Dim param As System.Data.SqlClient.SqlParameter
	'	'	param = cmd.Parameters.AddWithValue("@manr", manr)
	'	'	param = cmd.Parameters.AddWithValue("@KDNr", kdnr)

	'	'	Dim reader As SqlDataReader = cmd.ExecuteReader()
	'	'	If reader.Read() Then
	'	'		strResult.Append(String.Format("{0}, {1}¦", reader("Vorname"), reader("Nachname")))			' 0
	'	'		strResult.Append(String.Format("{0}¦", Format(CDate(reader("GebDat").ToString), "d"))) ' 1
	'	'		strResult.Append(String.Format("{0}¦{1}¦", reader("Bewillig"), reader("Bew_Bis"))) ' 2, 3
	'	'		strResult.Append(String.Format("{0}¦{1}¦", reader("Beruf"), reader("QLand")))	' 4, 5

	'	'		' DateTime.Now.Year - DateTime.Parse(reader("GebDat").ToString()).Year & "¦") ' 6
	'	'		strResult.Append(String.Format("{0}¦", GetSUVA_MAAlter(CDate(reader("GebDat")))))
	'	'		strResult.Append(String.Format("{0}¦", reader("Firma1")))	' 7
	'	'		strResult.Append(String.Format("{0}¦", reader("KDPLZ"))) ' 8
	'	'	End If
	'	'	conn.Close()

	'	'	conn.Open()
	'	'	cmdText = "SELECT Kanton, GAVNumber "
	'	'	'cmdText += ",(SELECT Count(*) FROM KD_GAVGruppe K1 "
	'	'	'cmdText += " WHERE K1.KDNR = K0.KDNR) As RecCount "
	'	'	cmdText += "FROM KD_GAVGruppe "
	'	'	cmdText += "WHERE KDNR = @kdnr Group By Kanton, GAVNumber Order By GAVNumber"
	'	'	cmd.CommandText = cmdText
	'	'	Dim pKDNR As SqlParameter = New SqlParameter("@kdnr", SqlDbType.Int)
	'	'	cmd.Parameters.Clear()
	'	'	cmd.Parameters.Add(pKDNR)
	'	'	pKDNR.Value = kdnr
	'	'	reader = cmd.ExecuteReader()
	'	'	If reader.Read() Then
	'	'		If Not IsDBNull(reader("Kanton")) Then
	'	'			strResult.Append(String.Format("{0}¦", reader("Kanton").ToString)) ' 9
	'	'		End If
	'	'		If Not IsDBNull(reader("GAVNumber")) Then
	'	'			strResult.Append(String.Format("{0}", reader("GAVNumber").ToString)) ' 10
	'	'		End If

	'	'	End If


	'	'Catch ex As Exception
	'	'	MsgBox(String.Format("{0}", ex.Message), MsgBoxStyle.Critical, "GetESDataForGAV")
	'	'	'_ex.MessageBoxShowError(iLogedUSNr, "GetESDataForGAV", ex)
	'	'Finally
	'	'	conn.Close()
	'	'End Try

	'	'Return strResult.ToString
	'End Function

	'Function GetKDGAVListe(ByVal iKDNr As Integer) As List(Of String)
	'   Dim strResult As New List(Of String)
	'   Dim conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

	'   Try
	'     conn.Open()
	'     Dim cmdText As String = "SELECT GAVNumber, Bezeichnung "
	'     cmdText &= "FROM KD_GAVGruppe K0 "
	'     cmdText += "WHERE K0.KDNR = @KDNr "
	'     Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
	'     cmd.CommandType = CommandType.Text
	'     Dim param As System.Data.SqlClient.SqlParameter
	'     param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)

	'     Dim reader As SqlDataReader = cmd.ExecuteReader()

	'     While reader.Read
	'       strResult.Add(String.Format("{0}", reader("GAVNumber").ToString))
	'     End While

	'   Catch ex As Exception

	'   End Try


	'   Return strResult
	' End Function

	' Function GetSUVA_MAAlter(ByVal dGebDate As Date) As Integer

	'   Dim dGeburtstag As Date = CDate(dGebDate)
	'   Dim today As DateTime = DateTime.Now
	'   Dim birth As DateTime = New DateTime(dGeburtstag.Year, dGeburtstag.Month, dGeburtstag.Day)
	'   Dim alter As Integer = today.Year - birth.Year
	'   If (today.Month < birth.Month) Then
	'     alter = alter - 1
	'   ElseIf (today.Month = birth.Month And today.Day < birth.Day) Then
	'     alter = alter - 1
	'   End If

	'   Return alter
	' End Function

	Private Sub SetComboBoxWidth(ByRef cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim iWidth As Integer = 0
    For Each cboItem As ComboBoxItem In cbo.Properties.Items
      iWidth = CInt(IIf(iWidth > cboItem.Text.Length, iWidth, cboItem.Text.Length))
    Next
    '    cbo.DropDownWidth = CInt((iWidth * 7) + 20)
  End Sub

  Function MeasureString(ByVal Text As String, ByVal FontName As String, ByVal FontSize As Single) As SizeF
    Dim Bitmap As Bitmap
    Dim Graphic As Graphics
    Dim Font As New Font(FontName, FontSize)

    Bitmap = New Bitmap(1, 1)
    Graphic = Graphics.FromImage(Bitmap)
    MeasureString = Graphic.MeasureString(Text, Font)
    Graphic.Dispose()
    Bitmap.Dispose()
  End Function


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
