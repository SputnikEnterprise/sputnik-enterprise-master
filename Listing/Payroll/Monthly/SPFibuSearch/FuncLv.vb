
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SP.Infrastructure.Logging
Imports SPFibuSearch.ClsDataDetail


Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

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

  Function GetFibuData4ShowInGrid(ByVal strQuery As String) As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
    'Dim strQuery As String = "[List ESData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.Text

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Sub FillLvData(ByVal LV As ListView)
    Dim strOperator As String = "="
    Dim strSqlQuery As String = ClsDataDetail.GetSQLQuery()
    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()
      Dim cmd As New SqlCommand(strSqlQuery, Conn)
      Dim rDbrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank

      LV.Items.Clear()
      LV.FullRowSelect = True
      Dim j As Integer = 0

      While rDbrec.Read
        LV.Items.Add("")
        LV.Items(j).SubItems.Add(rDbrec("LANr").ToString)
        LV.Items(j).SubItems.Add(rDbrec("Bezeichnung").ToString)
        LV.Items(j).SubItems.Add(Format(rDbrec("TotalBetrag"), "n"))

        For i As Integer = 3 To rDbrec.FieldCount - 5
          LV.Items(j).SubItems.Add(Format(CDbl(rDbrec.Item(i)), "n"))
        Next

        j += 1
      End While

    Catch e As SqlException
      LV.Items.Clear()
      MsgBox("Möglicherweise wurden keine Daten gefunden." & vbNewLine & _
             "Bitte versuchen Sie Ihre Suchkriterien zu anpassen.", MsgBoxStyle.Information, "Daten auflisten")

    Catch e As Exception
      LV.Items.Clear()
      MsgBox(e.StackTrace & vbNewLine & _
             e.Message, MsgBoxStyle.Information, "FillLvData_0")

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

#Region "Dropdown-Funktionen für 1. Seite..."

  Sub ListUJFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If Not ClsDataDetail.IsNewVersion Then Exit Sub
		Dim strSqlQuery As String = String.Empty
    Dim i As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
    Dim strOldName As String = cbo.Text.Trim

    Try
      Conn.Open()

			Dim strTableName As String = String.Format("[UmsatzJournal_New_{0}]", m_InitialData.UserData.UserGuid)
      Dim sSql As String = String.Empty
      If ClsDataDetail.IsNewVersion Then
        'sSql = "[dbo].[List AllFilial From _LOLFibu_]"
        sSql = "[dbo].[List AllFilial From New_LOLFibu]"
				strTableName = String.Format("[_LOLFibu_{0}]", m_InitialData.UserData.UserGuid)
      Else
        sSql = "[dbo].[List AllFilial From UJournal]"
        strTableName = strTableName

      End If

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@tblName", strTableName)

      Dim rUJrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("Alle")

      While rUJrec.Read
        cbo.Properties.Items.Add(rUJrec("USFiliale").ToString.Trim)
      End While


    Catch e As Exception
      m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, e.Message))

    Finally
      Conn.Close()
      Conn.Dispose()

      cbo.Text = strOldName

    End Try

  End Sub

  Sub ListLOMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal iYear As Integer)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()
      strSqlQuery = "[Get LOMonth In Selected Year]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@Year", iYear)

      Dim rLOrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rLOrec.Read
        cbo.Properties.Items.Add(rLOrec("LP").ToString)
      End While
      cbo.Properties.DropDownRows = 13


    Catch e As Exception
      m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, e.Message))
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListLOYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim i As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
    Dim iWidth As Integer

    Try
      Conn.Open()

      strSqlQuery = "[GetLOYear]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rLOrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      cbo.Properties.Items.Clear()
      While rLOrec.Read
        cbo.Properties.Items.Add(rLOrec("Jahr").ToString)
        iWidth = CInt(IIf(iWidth > CInt(Len(rLOrec("Jahr").ToString)), iWidth, CInt(Len(rLOrec("Jahr").ToString))))

        i += 1
      End While
      cbo.Properties.DropDownRows = 13

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub FillFoundedKstBez(ByVal Lst As ListBox, _
                      ByVal strFMonth As String, _
                      ByVal strLMonth As String, _
                      ByVal strYear As String)
    Dim LoiKstBez As List(Of String) = FillLoi4KstBez(strFMonth, strLMonth, strYear)
    If LoiKstBez.Count = 0 Then
      LoiKstBez = FillLoi4KstBez(strFMonth, strLMonth, strYear)
      If LoiKstBez.Count = 0 Then Exit Sub
    End If

    Dim Time_1 As Double = System.Environment.TickCount

    Lst.Items.Clear()
    Try
      Lst.BeginUpdate()
      Console.WriteLine("BeginUpdate: (OK)")
      For i As Integer = 0 To LoiKstBez.Count - 1
        With Lst
          .Items.Add(LoiKstBez(i))

        End With
      Next i
      Lst.EndUpdate()

      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für LOL.KST: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

    Catch e As SqlException
      Trace.WriteLine(m_InitialData.MDData.MDDbConn)
      MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "SQL:FillFoundedKstBez_1")
      Err.Clear()

    Catch e As Exception
      Trace.WriteLine(m_InitialData.MDData.MDDbConn)
      MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "SQL:FillFoundedKstBez_2")
      Err.Clear()

    Finally

    End Try

  End Sub

  Function FillLoi4KstBez(ByVal strFMonth As String, _
                      ByVal strLMonth As String, _
                      ByVal strYear As String) As List(Of String)

    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
    Dim i As Integer = 0
    Dim loiBez As New List(Of String)

    If strFMonth = String.Empty Then strFMonth = CStr(Month(Now))
    If strLMonth = String.Empty Then strLMonth = CStr(Month(Now))
    If strYear = String.Empty Then strYear = CStr(Year(Now))

    Dim Time_1 As Double = System.Environment.TickCount
    Dim sSql As String = "[GetBUmsatzKst3]"

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandTimeout = 400
      Console.WriteLine("CommandTimeout: (" & (cmd.CommandTimeout / 1000).ToString() + " s)")
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter
      Console.WriteLine("Param: (OK)")

      param = cmd.Parameters.AddWithValue("@LPVon", strFMonth)
      param = cmd.Parameters.AddWithValue("@LPBis", strLMonth)
      param = cmd.Parameters.AddWithValue("@MDYear", strYear)

      Dim rLOLrec As SqlDataReader = cmd.ExecuteReader
      Console.WriteLine("ExecuteReader: (OK)")

      Console.WriteLine("BeginUpdate: (OK)")
      While rLOLrec.Read
        loiBez.Add("")
        loiBez(i) = rLOLrec("KST").ToString

        i += 1
      End While

      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für LOL.KST: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

    Catch e As SqlException
      Trace.WriteLine(m_InitialData.MDData.MDDbConn)
      loiBez.Clear()

    Catch e As Exception
      Trace.WriteLine(m_InitialData.MDData.MDDbConn)
      MsgBox(e.StackTrace & vbNewLine & e.Message, MsgBoxStyle.Critical, "SQL:FillFoundedKstBez_2")
      Err.Clear()
      loiBez.Clear()

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return loiBez
  End Function

#End Region

  Function EnablingMarsintoConnString(ByVal _strConnString As String) As String
    Dim strTempConnString As String = _strConnString

    Try
      '  & ";mars=true"
      If Not strTempConnString.ToUpper.Contains("MultipleActiveResultSets=") Then
        strTempConnString &= ";MultipleActiveResultSets="
        Dim strQuery As String = "//SPFibuSearch/SPFibuSearch/DiffSetting[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/MARS"

        Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
        If strBez <> String.Empty Then
          strTempConnString &= strBez

        Else
          strTempConnString &= "True"
        End If
      End If

    Catch ex As Exception

    End Try

    Try
      '  & ";pooling=true"
      If Not strTempConnString.ToUpper.Contains("Pooling=") Then
        strTempConnString &= ";Pooling="
        Dim strQuery As String = "//SPFibuSearch/SPFibuSearch/DiffSetting[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/Pooling"

        Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
        If strBez <> String.Empty Then
          strTempConnString &= strBez

        Else
          strTempConnString &= "true"
        End If
      End If

    Catch ex As Exception

    End Try


    Try
      '  & ";Connect Timeout=120"
      If Not strTempConnString.ToUpper.Contains("timeout=".ToUpper) Then
        strTempConnString &= ";Connect Timeout="
        Dim strQuery As String = "//SPFibuSearch/SPFibuSearch/DiffSetting[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/Timeout"

        Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetSQLDataFile(), strQuery)
        If strBez <> String.Empty Then
          strTempConnString &= strBez

        Else
          strTempConnString &= "300"
        End If
      End If

    Catch ex As Exception

    End Try

    Return strTempConnString
  End Function

End Module
