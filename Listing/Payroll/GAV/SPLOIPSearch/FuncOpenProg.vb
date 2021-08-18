
'Option Strict Off

'Imports System.Data.SqlClient
'Imports System.IO
'Imports System.Text.RegularExpressions
'Imports System.Reflection

'Module FuncOpenProg

'  Dim _ClsFunc As New ClsDivFunc
'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

'  Dim strMDPath As String = ""
'  Dim strInitPath As String = ""

'  Dim iLogedUSNr As Integer = 0

'  Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

'  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
'  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

'  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
'    '    Dim strFieldName As String = "Bezeichnung"
'    Dim i As Integer = 0
'    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From ESExportDb Where ModulName = 'ES' "
'    strSqlQuery += "Order By RecNr"

'    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

'    Try
'      Conn.Open()

'      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
'      cmd.CommandType = Data.CommandType.Text

'      Dim rMnurec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

'      tsbMenu.DropDownItems.Clear()
'      tsbMenu.DropDown.SuspendLayout()

'      Dim mnu As ToolStripMenuItem
'      While rMnurec.Read
'        i += 1

'        If rMnurec("Bezeichnung").ToString = "-" Then
'          Dim sep As New ToolStripSeparator()
'          tsbMenu.DropDownItems.Add(sep)

'        Else
'          mnu = New ToolStripMenuItem()

'          mnu.Text = rMnurec("Bezeichnung").ToString
'          If Not IsDBNull(rMnurec("ToolTip")) Then
'            mnu.ToolTipText = rMnurec("ToolTip").ToString
'          End If
'          If Not IsDBNull(rMnurec("MnuName").ToString) Then
'            mnu.Name = rMnurec("MnuName").ToString
'          End If
'          tsbMenu.DropDownItems.Add(mnu)

'        End If

'      End While
'      tsbMenu.DropDown.ResumeLayout()
'      tsbMenu.ShowDropDown()


'    Catch e As Exception
'      MsgBox(Err.GetException.ToString)

'    Finally
'      Conn.Close()
'      Conn.Dispose()

'    End Try

'  End Sub

'#Region "Funktionen für Exportieren..."

'  Sub RunBewModul(ByVal strTempSQL As String)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
'    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
'    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

'    Try
'      oMyProg = CreateObject("SPSBewUtility.ClsMain")
'      oMyProg.OpenKDFieldsform(strTempSQL)

'    Catch e As Exception

'    End Try

'  End Sub


'  ''' <summary>
'  ''' Fremdrechnungverwaltung öffnen
'  ''' </summary>
'  ''' <param name="iFOPNr"></param>
'  ''' <remarks></remarks>
'  Sub RunOpenFOPForm(ByVal iFOPNr As Integer)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    Try
'      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "FOPNr", iFOPNr.ToString)

'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("FOpUtility.ClsMain", iFOPNr.ToString)
'    Catch e As Exception
'      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenFOPForm")
'    End Try
'  End Sub

'  ''' <summary>
'  ''' Kandidatenverwaltung öffnen
'  ''' </summary>
'  ''' <param name="iMANr"></param>
'  ''' <remarks></remarks>
'  Sub RunOpenMAForm(ByVal iMANr As Integer)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    Try
'      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MANr", iMANr.ToString)

'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("KandidatUtility.ClsMain", iMANr.ToString)

'    Catch e As Exception
'      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMAForm")

'    End Try

'  End Sub


'  ''' <summary>
'  ''' Kundenverwaltung öffnen
'  ''' </summary>
'  ''' <param name="iKDNr"></param>
'  ''' <remarks></remarks>
'  Sub RunOpenKDform(ByVal iKDNr As Integer)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    Try
'      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "KDNr", iKDNr.ToString)

'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("KundenUtility.ClsMain", iKDNr.ToString)

'    Catch e As Exception
'      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenKDForm")

'    End Try

'  End Sub

'  ''' <summary>
'  ''' Einsatzverwaltung öffnen
'  ''' </summary>
'  ''' <param name="iESNr"></param>
'  ''' <remarks></remarks>
'  Sub RunOpenESForm(ByVal iESNr As Integer)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    Try
'      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "ESNr", iESNr.ToString)

'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("ESUtility.ClsMain", iESNr.ToString, 2)
'    Catch e As Exception
'      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMESForm")
'    End Try
'  End Sub


'  Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
'    Dim strFullFileName As String = String.Empty
'    Dim strFilePath As String = String.Empty
'    Dim myStream As Stream = Nothing
'    Dim openFileDialog1 As New OpenFileDialog()

'    openFileDialog1.Title = strFile2Search
'    openFileDialog1.InitialDirectory = strFile2Search
'    openFileDialog1.Filter = "EXE-Dateien (*.exe)|*.exe|Alle Dateien (*.*)|*.*"
'    openFileDialog1.FilterIndex = 1
'    openFileDialog1.RestoreDirectory = True

'    If openFileDialog1.ShowDialog() = DialogResult.OK Then
'      Try

'        myStream = openFileDialog1.OpenFile()
'        If (myStream IsNot Nothing) Then
'          strFullFileName = openFileDialog1.FileName()

'          ' Insert code to read the stream here.
'        End If

'      Catch Ex As Exception
'        MessageBox.Show("Kann keine Daten lesen: " & Ex.Message)
'      Finally
'        ' Check this again, since we need to make sure we didn't throw an exception on open.
'        If (myStream IsNot Nothing) Then
'          myStream.Close()
'        End If
'      End Try
'    End If

'    Return strFullFileName
'  End Function

'  Sub RunSMSProg(ByVal strQuery As String)

'    ' Umstellung von der neuen SQL-Query wieder zur alten Version.
'    strQuery = strQuery.Replace("MANachname", "Nachname").Replace("MAVorname", "Vorname")

'    Dim strProgPath As String
'    Dim strSMSProgName As String = "Sputnik Suite SMS.EXE"
'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", _
'               "SQLQuery", strQuery)

'    Dim strSMSFile As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg")
'    If strSMSFile = String.Empty Then
'      strProgPath = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath")
'      strProgPath = _ClsReg.AddDirSep(strProgPath) & "Binn\"

'      If strSMSFile = String.Empty Then strSMSFile = strProgPath & strSMSProgName
'    End If

'    If Not File.Exists(strSMSFile) Then
'      MsgBox("Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus." & vbLf & _
'          (strSMSFile), MsgBoxStyle.Critical, "Programm wurde nicht gefunden")

'      strSMSFile = ShowMyFileDlg(strSMSFile)
'      If strSMSFile <> String.Empty Then
'        _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg", strSMSFile)
'        Process.Start(strSMSFile)
'      End If

'    Else
'      Process.Start(strSMSFile)

'    End If

'  End Sub

'  Sub RunMailModul(ByVal strTempSQL As String)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
'    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMailUtility.ClsMain")
'    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

'    Try
'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("SPSMailUtility.ClsMain", strTempSQL)

'    Catch e As Exception

'    End Try

'  End Sub

'  Sub ExportDataToOutlook(ByVal strTempSQL As String)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
'    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
'    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

'    Try
'      If MsgBox("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
'        oMyProg = CreateObject("SPSModulsView.ClsMain")
'        oMyProg.ExportDataToOutlook(strTempSQL, "KD")
'      End If

'    Catch e As Exception

'    End Try

'  End Sub

'  Sub RunKommaModul(ByVal strTempSQL As String)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
'    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
'    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

'    Try
'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "KD")

'    Catch e As Exception

'    End Try

'  End Sub

'  Sub RunTobitFaxModul(ByVal strTempSQL As String)
'    Dim oMyProg As Object
'    Dim strTranslationProgName As String = String.Empty

'    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
'    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
'    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

'    Try
'      oMyProg = CreateObject("SPSModulsView.ClsMain")
'      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "MA", "1")

'    Catch e As Exception

'    End Try

'  End Sub

'  Sub RunXMLModul(ByVal strTempSQL As String)
'    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'    Dim strTranslationProgName As String = String.Empty

'    Dim cmd As System.Data.SqlClient.SqlCommand
'    cmd = New System.Data.SqlClient.SqlCommand(strTempSQL & " FOR XML AUTO", Conn)

'    Try
'      Conn.Open()

'      Dim Xml_Reader As System.Xml.XmlReader

'      Xml_Reader = cmd.ExecuteXmlReader()
'      Dim sb As New System.Text.StringBuilder
'      sb.Append("<xml>")
'      Xml_Reader.Read()
'      Do
'        Dim node As String = Xml_Reader.ReadOuterXml()
'        If node.Length = 0 Then Exit Do
'        sb.Append(node)
'      Loop
'      sb.Append("</xml>")

'      Xml_Reader.Close()

'      Dim objDateiMacher As StreamWriter
'      objDateiMacher = New StreamWriter(_ClsProgSetting.GetPersonalFolder() & "KDList.XML")
'      objDateiMacher.Write(sb.ToString)
'      objDateiMacher.Close()
'      objDateiMacher.Dispose()


'    Catch e As Exception

'    End Try

'  End Sub


'#End Region



'  Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
'    Dim bResult As Boolean = False

'    ' alle geöffneten Forms durchlaufen
'    For Each oForm As Form In Application.OpenForms
'      If oForm.Name.ToLower = sName.ToLower Then
'        If bDisposeForm Then oForm.Dispose() : Exit For
'        bResult = True : Exit For
'      End If
'    Next

'    Return (bResult)
'  End Function

'  Function ExtraRights(ByVal lModulNr As Integer) As Boolean
'    Dim bAllowed As Boolean
'    Dim strModulCode As String

'    ' 10200        ' Fremdrechnung
'    ' 10201        ' Rapportinhalt
'    ' 10202        ' Export nach Abacus
'    ' 10206        ' Export nach Sesam
'    strModulCode = _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile, "ExtraModuls", CStr(lModulNr))
'    If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

'    ExtraRights = bAllowed

'  End Function


'	Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
'		System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
'	End Sub


'End Module
