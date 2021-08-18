
Option Strict Off

Imports System.Media
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports SPRPUmsatzTotal.ClsDataDetail


Module FuncOpenProg

  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  'Dim strMDPath As String = ""
  'Dim strInitPath As String = ""

  'Dim iLogedUSNr As Integer = 0

  'Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

  Dim strConnString As String = m_InitialData.MDData.MDDbConn
  'Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
  'Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

  Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, _
                        ByVal dBetrag_1 As Double, ByVal dBetrag_2 As Double, _
                        ByVal dBetrag_3 As Double, ByVal dBetrag_4 As Double, _
                        ByVal dBetrag_5 As Double, ByVal dBetrag_6 As Double, _
                        ByVal dBetrag_7 As Double, ByVal dBetrag_8 As Double, _
                        ByVal dBetrag_9 As Double, ByVal dBetrag_10 As Double, _
                        ByVal dBetrag_11 As Double, ByVal dBetrag_12 As Double)
    Dim i As Integer = 0

    Try
      tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem

      mnu = New ToolStripMenuItem()
      mnu.Text = "Temporärumsatz: " & Format(dBetrag_1, "###,###,###,###,###,###,0.00")
      tsbMenu.DropDownItems.Add(mnu)

      If dBetrag_2 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Individueller Umsatz: " & Format(dBetrag_2, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_3 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Umsatz für Festanstellungen: " & Format(dBetrag_3, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_4 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Gutschriften: " & Format(dBetrag_4, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_5 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Skonto: " & Format(dBetrag_5, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_6 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Erlösminderung: " & Format(dBetrag_6, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_7 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Sonstige Verluste: " & Format(dBetrag_7, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_8 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Fremdrechnungen: " & Format(dBetrag_8, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      tsbMenu.DropDownItems.Add(New ToolStripSeparator)

      If dBetrag_9 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Bruttolohn: " & Format(dBetrag_9, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_10 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Ad.-Kosten: " & Format(dBetrag_10 * (ClsDataDetail.GetXMarge / 100), "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_11 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "AG.-Beitrag: " & Format(dBetrag_11, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If dBetrag_12 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = "Fremdleistungen: " & Format(dBetrag_12, "###,###,###,###,###,###,0.00")
        tsbMenu.DropDownItems.Add(mnu)
      End If

      tsbMenu.DropDownItems.Add(New ToolStripSeparator)
      Dim dTotalUmsatz As Double = dBetrag_1 + dBetrag_2 + dBetrag_3 - ((dBetrag_4 * -1) + dBetrag_5 + dBetrag_6 + dBetrag_7 + dBetrag_8)
      Dim dTotalLohn As Double = dBetrag_9 + (dBetrag_10 * (ClsDataDetail.GetXMarge / 100)) - dBetrag_11 - dBetrag_12
      mnu = New ToolStripMenuItem()
      mnu.Text = "Total Umsastz: " & Format(dTotalUmsatz, "###,###,###,###,###,###,0.00")
      mnu.ForeColor = Color.Blue
      tsbMenu.DropDownItems.Add(mnu)

      mnu = New ToolStripMenuItem()
      mnu.Text = "Total Lohnaufwand: " & Format(dTotalLohn, "###,###,###,###,###,###,0.00")
      mnu.ForeColor = Color.Blue
      tsbMenu.DropDownItems.Add(mnu)

      mnu = New ToolStripMenuItem()
      mnu.Text = "Total Bruttogewinn %: " & Format(((dTotalUmsatz - dTotalLohn) / dTotalUmsatz) * 100, "###,0.00")
      mnu.ForeColor = Color.Blue
      tsbMenu.DropDownItems.Add(mnu)

      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()

    Catch e As Exception
      MsgBox(Err.GetException.ToString)

    Finally

    End Try

  End Sub

  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    '    Dim strFieldName As String = "Bezeichnung"
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From KDUmExportDb Where ModulName = 'KDUmsatz' "
    strSqlQuery += "Order By RecNr"

    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMnurec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

      tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem
      While rMnurec.Read
        i += 1

        If rMnurec("Bezeichnung").ToString = "-" Then
          Dim sep As New ToolStripSeparator()
          tsbMenu.DropDownItems.Add(sep)

        Else
          mnu = New ToolStripMenuItem()

          mnu.Text = rMnurec("Bezeichnung").ToString
          If Not IsDBNull(rMnurec("ToolTip")) Then
            mnu.ToolTipText = rMnurec("ToolTip").ToString
          End If
          If Not IsDBNull(rMnurec("MnuName").ToString) Then
            mnu.Name = rMnurec("MnuName").ToString
          End If
          tsbMenu.DropDownItems.Add(mnu)

        End If
        '        AddHandler mnu.Click, AddressOf SPKDUmsatz.btn_Test_1

        '        tsbMenu.DropDownItems.Add(mnu)
        'm_FileMenu.DropDownItems.Add(mnu)


        '        tsbMenu.DropDownItems.Add(rMnurec(strFieldName).ToString)
        '        tsbMenu.DropDownItems(1).Name = rMnurec("mnuName").ToString

      End While
      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()

    Catch e As Exception
      MsgBox(Err.GetException.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

#Region "Funktionen für Exportieren..."

  Sub RunOpenKDForm(ByVal iKDNr As Integer)
    Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & iSelectedUSNr
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDNr.ToString)

    Try
      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "KDNr", iKDNr.ToString)

      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("KundenUtility.ClsMain", iKDNr.ToString)

    Catch e As Exception
      '      MsgBox(Err.GetException, MsgBoxStyle.Critical, "RunOpenKDForm")

    End Try

  End Sub

  Sub RunOpenOPForm(ByVal iOPNr As Integer)
    Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & iSelectedUSNr
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iOPNr.ToString)

    Try
      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "RENr", iOPNr.ToString)

      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("OPUtility.ClsMain", iOPNr.ToString, 2)

    Catch e As Exception
      MsgBox(e.StackTrace, MsgBoxStyle.Critical, "RunOpenOPForm")

    End Try

  End Sub

  Sub RunKommaModul(ByVal strTempSQL As String)
    Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & iSelectedUSNr
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "OP")

    Catch e As Exception

    End Try

  End Sub

  Sub RunXMLModul(ByVal strTempSQL As String)
    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
    Dim strTranslationProgName As String = String.Empty

    Dim cmd As System.Data.SqlClient.SqlCommand
    cmd = New System.Data.SqlClient.SqlCommand(strTempSQL & " FOR XML AUTO", Conn)

    Try
      Conn.Open()

      Dim Xml_Reader As System.Xml.XmlReader

      Xml_Reader = cmd.ExecuteXmlReader()
      Dim sb As New System.Text.StringBuilder
      sb.Append("<xml>")
      Xml_Reader.Read()
      Do
        Dim node As String = Xml_Reader.ReadOuterXml()
        If node.Length = 0 Then Exit Do
        sb.Append(node)
      Loop
      sb.Append("</xml>")

      Xml_Reader.Close()

      Dim objDateiMacher As StreamWriter
      objDateiMacher = New StreamWriter(_ClsProgSetting.GetPersonalFolder() & "OPList.XML")
      objDateiMacher.Write(sb.ToString)
      objDateiMacher.Close()
      objDateiMacher.Dispose()

      MessageBox.Show("Die Datei " & _ClsProgSetting.GetPersonalFolder() & "OPList.XML" & " wurde erfolgreich erstelle.", "Export in XML", MessageBoxButtons.OK, MessageBoxIcon.Information)


    Catch e As Exception

    End Try

  End Sub

#End Region

  Public Sub PlaySound(ByVal sSound As Short)

    If Not My.Settings.bPlaySound Then Exit Sub
    If sSound = 0 Then
      System.Media.SystemSounds.Asterisk.Play()

    ElseIf sSound = 2 Then
      System.Media.SystemSounds.Exclamation.Play()

    ElseIf sSound = 3 Then
      System.Media.SystemSounds.Hand.Play()

    ElseIf sSound = 4 Then
      System.Media.SystemSounds.Question.Play()

    Else
      System.Media.SystemSounds.Beep.Play()

    End If

  End Sub

  Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
    Dim bResult As Boolean = False

    ' alle geöffneten Forms durchlauden
    For Each oForm As Form In Application.OpenForms
      If oForm.Name.ToLower = sName.ToLower Then
        If bDisposeForm Then oForm.Dispose() : Exit For
        bResult = True : Exit For
      End If
    Next

    Return (bResult)
  End Function

  Function ExtraRights(ByVal lModulNr As Integer) As Boolean
    Dim bAllowed As Boolean
    Dim strModulCode As String

    ' 10200        ' Fremdrechnung
    ' 10201        ' Rapportinhalt
    ' 10202        ' Export nach Abacus
    ' 10206        ' Export nach Sesam
    strModulCode = _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile, "ExtraModuls", CStr(lModulNr))
    If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

    ExtraRights = bAllowed

  End Function



End Module
