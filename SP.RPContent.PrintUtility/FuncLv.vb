
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions


Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Private _ClsFunc As New ClsDivFunc
  Private _ClsReg As New SPProgUtility.ClsDivReg


  Function GetRPDbData4PrintContent(ByVal _setting As ClsPopupSetting) As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
    Dim strQuery As String = "[List ESData For Search In ESPrint]"
    Dim strMonth As String = String.Empty
    Dim strYear As String = String.Empty

    strQuery = "Select RP.RPNr As [Rapport-Nr.], (Convert(nvarchar(10), RP.Von, 104) + ' - '  + Convert(nvarchar(10), RP.Bis, 104)) As Zeitraum, "
    strQuery &= "(MA.Nachname + ', ' + MA.Vorname) As [Kandidat], "
    strQuery &= "KD.Firma1, ES.ES_Als, RP.RPGAV_Beruf As [GAV-Beruf] From RP "
    strQuery &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
    strQuery &= "Left Join Kunden KD On RP.KDNr = KD.KDNr "
    strQuery &= "Left Join ES On RP.ESNr = ES.ESNr "
    strQuery &= "Where RP.Jahr In (@iYear) And "
    strQuery &= "RP.Monat In (@iMonth) And "
    strQuery &= "(RP.RPGAV_Beruf = @GAVBez Or @GAVBez = '') And "
    strQuery &= "(MA.MAFiliale + KD.KDFiliale) Like "
    strQuery &= "@USFiliale "
    strQuery &= "Order By MA.Nachname"

    For i As Integer = 0 To _setting.SearchMonth.Count - 1
      strMonth &= If(strMonth = "", "", ",") & _setting.SearchMonth(i)
    Next
    For i As Integer = 0 To _setting.SearchYear.Count - 1
      stryear &= If(stryear = "", "", ",") & _setting.SearchYear(i)
    Next

    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.Text

    Dim objAdapter As New SqlDataAdapter
    Dim param As System.Data.SqlClient.SqlParameter

    param = cmd.Parameters.AddWithValue("@GAVBez", _setting.SearchPVLBez)
    param = cmd.Parameters.AddWithValue("@iMonth", strMonth)
    param = cmd.Parameters.AddWithValue("@iYear", strYear)
    param = cmd.Parameters.AddWithValue("@USFiliale", "%%")

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "RPData")

    Return ds.Tables(0)
  End Function

  Sub ListLOMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal _iYear As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Dim strValue As String
      Conn.Open()

      Dim strSqlQuery As String = "[List LOMonth For Search In LO Liste]"
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim pYear As SqlParameter = New SqlParameter("@iYear", SqlDbType.NVarChar, 50)

      Dim rFrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rFrec.Read

        Try
          strValue = String.Format("{0}", CInt(rFrec("LP")))
          cbo.Properties.Items.Add(strValue)

        Catch ex As Exception

        End Try

      End While

    Catch ex As Exception ' Manager
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      MessageBoxShowError(strMethodeName, ex)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListLOYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Dim strValue As String
      Conn.Open()

      Dim strSqlQuery As String = "[List LOYear For Search In LO Liste]"
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rFrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rFrec.Read

        Try
          strValue = String.Format("{0}", String.Format("{0:#####}", CDec(rFrec("Jahr").ToString)))
          cbo.Properties.Items.Add(strValue)

        Catch ex As Exception

        End Try

      End While

    Catch ex As Exception ' Manager
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      MessageBoxShowError(strMethodeName, ex)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListPVLBez(ByVal _setting As ClsPopupSetting)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
    Dim strWhereQuery As String = String.Empty
    Dim strSql As String = String.Empty
    Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = _setting.cbo2Fill

    Try
      If _setting.SearchYear.Count <> 0 And _setting.SearchMonth.Count <> 0 Then
        strSql = "[List PVLData With Month For Search In ESPrint]"
      Else
        strSql = "[List PVLData Without Month For Search In ESPrint]"
      End If
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSql, Conn)
      cmd.CommandType = CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@GAVKanton", _setting.SearchPVLKanton)
      If _setting.SearchYear.Count <> 0 And _setting.SearchMonth.Count <> 0 Then
        param = cmd.Parameters.AddWithValue("@iMonth", _setting.SearchMonth(0))
        param = cmd.Parameters.AddWithValue("@iYear", _setting.SearchYear(0))
      End If

      Dim rFrec As SqlDataReader = cmd.ExecuteReader
      Dim strValue As String = String.Empty

      cbo.Properties.Items.Clear()
      While rFrec.Read
        Try
          strValue = String.Format("{0}", rFrec("GAVGruppe0").ToString)
          cbo.Properties.Items.Add(strValue)

        Catch ex As Exception
          m_Logger.LogError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

        End Try

      End While
      'cbo.Properties.SeparatorChar = CChar(",")
      cbo.Properties.DropDownRows = 20


    Catch ex As Exception ' Manager
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      MessageBoxShowError(strMethodeName, ex)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListPVLKanton(ByVal _setting As ClsPopupSetting)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
    Dim strWhereQuery As String = String.Empty
    Dim strMANr As String = String.Empty
    Dim strSql As String = String.Empty
    Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = _setting.cbo2Fill

    Try
      If _setting.SearchYear.Count <> 0 And _setting.SearchMonth.Count <> 0 Then
        strSql = "[List PVLKantonData With Month For Search In ESPrint]"
      Else
        strSql = "[List PVLKantonData Without Month For Search In ESPrint]"
      End If
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSql, Conn)
      cmd.CommandType = CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@GAVGruppe0", _setting.SearchPVLBez)
      If _setting.SearchYear.Count <> 0 And _setting.SearchMonth.Count <> 0 Then
        param = cmd.Parameters.AddWithValue("@iMonth", _setting.SearchMonth(0))
        param = cmd.Parameters.AddWithValue("@iYear", _setting.SearchYear(0))
      End If

      Dim rFrec As SqlDataReader = cmd.ExecuteReader
      Dim strValue As String = String.Empty

      cbo.Properties.Items.Clear()
      While rFrec.Read
        Try
          strValue = String.Format("{0}", rFrec("GAVKanton").ToString)
          cbo.Properties.Items.Add(strValue) ', CheckState.Unchecked, True)

        Catch ex As Exception
          m_Logger.LogError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

        End Try

      End While
      cbo.Properties.DropDownRows = 20


    Catch ex As Exception ' Manager
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      MessageBoxShowError(strMethodeName, ex)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListFoundedrec(ByVal _setting As ClsRPCSetting)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
    Dim strWhereQuery As String = String.Empty
    Dim strRPNr As String = ""
    Dim strBez As String = String.Empty
    Dim lv As New DevComponents.DotNetBar.Controls.ListViewEx
    lv = _setting.lv2Fill

    Try
      If _setting.SelectedMonth.Count > 0 Then
        For i As Integer = 0 To _setting.SelectedMonth.Count - 1
          If _setting.SelectedMonth(i) <> 0 Then strBez &= If(strBez.Length > 0, ",", "") & _setting.SelectedMonth.Item(i)
        Next
        If strBez <> "" Then strWhereQuery &= String.Format("RP.Monat In ({0}) ", strBez)
      End If
      strBez = String.Empty

      Dim strAndString As String = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
      If _setting.SelectedYear.Count > 0 Then
        For i As Integer = 0 To _setting.SelectedYear.Count - 1
          If _setting.SelectedYear(0) <> 0 Then strBez &= If(strBez.Length > 0, ",", "") & _setting.SelectedYear.Item(i)
        Next
        If strBez <> "" Then strWhereQuery &= String.Format("{0}RP.Jahr In ({1}) ", strAndString, strBez)
      End If
      strBez = String.Empty

      strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
      If _setting.SelectedRPNr.Count > 0 And _setting.SelectedRPNr(0) <> 0 Then
        For i As Integer = 0 To _setting.SelectedRPNr.Count - 1
          strRPNr &= If(strRPNr.Length > 0, ",", "") & _setting.SelectedRPNr.Item(i)
        Next
        If strRPNr <> "" Then strWhereQuery &= String.Format("{0}RP.RPNr In ({1}) ", strAndString, strRPNr)
      End If
      strBez = String.Empty

      strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
      If _setting.SelectedPVLBez <> String.Empty Then
        strWhereQuery &= String.Format("{0}RP.RPGAV_Beruf = '{1}' ", strAndString, _setting.SelectedPVLBez)
      End If

      strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
      If _setting.SelectedKanton <> String.Empty Then
        strWhereQuery &= String.Format("{0}RP.RPGAV_Kanton = '{1}' ", strAndString, _setting.SelectedKanton)
      End If


      If ClsDataDetail.MDData.MultiMD = 1 Then
        strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
        strWhereQuery &= String.Format("{0}RP.MDNr = {1}", strAndString, ClsDataDetail.ProgSettingData.SelectedMDNr)
      End If


      Dim strSqlQuery As String = "Select RP.ID, RP.RPNr, RP.MANr, RP.KDNr, RP.ESNr, RP.RPGAV_Beruf, "
      strSqlQuery &= "(Convert(nvarchar(10), RP.Von, 104) + ' - ' + IsNull(convert(nvarchar(10), RP.Bis, 104), '')) As Zeitraum, "
      strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName, MA.Geschlecht As MAGeschlecht, "
      strSqlQuery += "KD.Firma1, KD.Sprache As KDSprache "
      strSqlQuery &= "From RP "
      strSqlQuery &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
      strSqlQuery &= "Left Join Kunden KD On RP.KDNr = KD.KDNr "
      strSqlQuery &= "Left Join ES On RP.ESNr = ES.ESNr "
      strSqlQuery += "Left Join ESLohn ESL On RP.ESNr = ESL.ESNr And ESL.Aktivlodaten = 1 "
      If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
      strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, ES.ES_Ab ", strSqlQuery, strWhereQuery)

      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim rFrec As SqlDataReader = cmd.ExecuteReader

      Dim strColumnBez As String = m_xml.GetSafeTranslationValue("ID;RPNr;MANr;KDNr;ESNr;Zeitraum;Kandidat;Kunde;GAV-Beruf")
      Dim strColumnWidth As String = If(My.Settings.LV_ColumnWidth = String.Empty Or _
                                        My.Settings.LV_ColumnWidth.Split(CChar(";")).Length <> strColumnBez.Split(CChar(";")).Length, _
                                        "0-0;50-1;0-0;0-0;0-0;100-0;100-0;100-0;100-0", My.Settings.LV_ColumnWidth)
      FillDataHeaderLv(lv, strColumnBez, strColumnWidth)
      lv.Items.Clear()
      lv.FullRowSelect = True
      lv.MultiSelect = True

      Try
        Dim i As Integer = 0
        While rFrec.Read
          With lv
            _setting.FoundedRPNr.Add(rFrec("RPNr"))

            .Items.Add(rFrec("ID").ToString)
            .Items(i).SubItems.Add(rFrec("RPNr").ToString)
            .Items(i).SubItems.Add(rFrec("MANr").ToString)
            .Items(i).SubItems.Add(rFrec("KDNr").ToString)
            .Items(i).SubItems.Add(rFrec("ESNr").ToString)

            .Items(i).SubItems.Add(rFrec("Zeitraum").ToString)
            .Items(i).SubItems.Add(String.Format("{0}", rFrec("MAName").ToString))
            .Items(i).SubItems.Add(String.Format("{0}", rFrec("Firma1").ToString))
            .Items(i).SubItems.Add(String.Format("{0}", rFrec("RPGAV_Beruf").ToString))

          End With

          i += 1
        End While

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

      End Try


    Catch ex As Exception ' Manager
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      MessageBoxShowError(strMethodeName, ex)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

#Region "Sonstige Funktions..."

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

  Sub SetRowColor(ByVal LV As ListView, ByVal iIndex As Integer, ByVal bDiffColor As Boolean)

    For j As Integer = 0 To LV.Items(iIndex).SubItems.Count - 1
      LV.Items(iIndex).BackColor = If(bDiffColor, Color.Yellow, LV.BackColor)
    Next j

  End Sub


#End Region


End Module
