
Option Strict Off

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports System.Data.SqlClient
Imports System.IO
Imports SP.Infrastructure.Logging
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPFibuSearch.ClsDataDetail


Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath


  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim i As Integer = 0
    Dim bIsAllowed As Boolean = False
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = @GuidID "
    strSqlQuery += "Order By RecNr"

    Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@GuidID", ClsDataDetail.GetAppGuidValue())

      Dim rMnurec As SqlDataReader = cmd.ExecuteReader          ' Allgemeine Exportdatenbank

      '  tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem
      While rMnurec.Read
        i += 1

        If rMnurec("Bezeichnung").ToString = "-" Then
          Dim sep As New ToolStripSeparator()
          tsbMenu.DropDownItems.Add(sep)

        Else

					' Berechtigung für bestimmte Einträge ohne ExtraRechte ausser Kraft setzen
					If rMnurec("MnuName").ToString.ToUpper.Contains("Comatic".ToUpper) Then
						bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
						'bIsAllowed = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10211") = _
						'                "+{4401D7E1-D512-420d-8822-2106456B33C0}+"
					ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Abacus".ToUpper) Then
						bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
						'_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10202") = _
						'                "+{B2705597-5217-4778-96F6-2998CCEF0598}+"
					ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Cresus".ToUpper) Then
						bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
						'_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10202") = _
						'                "+{B2705597-5217-4778-96F6-2998CCEF0598}+"
					ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Sesam".ToUpper) Then
						bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
            'bIsAllowed = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10206") = _
            '                "+{172195F3-8C5A-41df-9018-6D3527CFD807}+"
          ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("SWIFAC".ToUpper) Then
            bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
            'bIsAllowed = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10212") = _
            '                "+{AC1200CE-AE49-4f40-A28E-EA5891449595}+"
          ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("KMUFactoring".ToUpper) Then
            bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
            'bIsAllowed = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10213") = _
            '                "+{799327A2-E69D-4021-AF83-072CA0468AAE}+"
          ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("CSOPListe".ToUpper) Then
            bIsAllowed = IsModulLicenceOK("CSOPList".ToLower)
            'bIsAllowed = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", "10214") = _
            '                "+{982E690D-39E4-4D8C-9028-01F2714E3A49}+"
          ElseIf rMnurec("MnuName").ToString.ToUpper = "CRF".ToUpper Then
            bIsAllowed = IsModulLicenceOK("CSOPList".ToLower)

          Else
            bIsAllowed = True ' Standardmässig darf man alles sehen
          End If

          If bIsAllowed Then
            mnu = New ToolStripMenuItem()

            mnu.Text = TranslateText(rMnurec("Bezeichnung").ToString.Replace("³", "ü"))
            If Not IsDBNull(rMnurec("ToolTip")) Then
              mnu.ToolTipText = TranslateText(rMnurec("ToolTip").ToString)
            End If
            If Not IsDBNull(rMnurec("MnuName").ToString) Then
              mnu.Name = rMnurec("MnuName").ToString
            End If
            If Not IsDBNull(rMnurec("Docname")) Then
              mnu.Tag = rMnurec("Docname").ToString
            End If
            tsbMenu.DropDownItems.Add(mnu)
          End If

        End If

      End While
      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      MsgBox(Err.GetException.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, ByVal dBetrag_1 As Double)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim i As Integer = 0

    Try
      tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem

      mnu = New ToolStripMenuItem()
      mnu.Text = "Totalbetrag: " & Format(dBetrag_1, "n")
      tsbMenu.DropDownItems.Add(mnu)

      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()

    Catch e As Exception
      MsgBox(Err.GetException.ToString)

    Finally

    End Try

  End Sub

#Region "Funktionen für Exportieren..."

  Sub RunSesamExport()
    Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitialData.MDData.MDDbConn, _
                                                                    .ModulName = "SesamLO".ToLower, _
                                                                    .SQL2Open = ClsDataDetail.GetSQLQuery, _
                                                                        .SelectedMonth = ClsDataDetail.GetLP, _
                                                                        .SelectedYear = ClsDataDetail.GetYear}
    Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

    Try
      obj.ShowSesamForm()

      'Dim frmSource As New frmSesam(ClsDataDetail.GetLP, ClsDataDetail.GetYear)

      'frmSource.Show()

    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "RunSesamExport_0")

    End Try

  End Sub

	Sub RunAbacusExport()
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitialData.MDData.MDDbConn,
																		.ModulName = "AbaLO".ToLower,
																		.SQL2Open = "",
																		.SelectedYear = ClsDataDetail.GetYear,
																		.SelectedMonth = ClsDataDetail.GetLP}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			'  Dim frmSource As New frmAbacus(ClsDataDetail.GetLP, ClsDataDetail.GetYear)
			'frmSource.Show()

			obj.ShowAbacusForm()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunAbacusExport_0")

		End Try

	End Sub

	Sub RunCresusExport()
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitialData.MDData.MDDbConn,
																		.ModulName = "CresusLO".ToLower,
																		.SQL2Open = "",
																		.SelectedYear = ClsDataDetail.GetYear,
																		.SelectedMonth = ClsDataDetail.GetLP}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			'  Dim frmSource As New frmAbacus(ClsDataDetail.GetLP, ClsDataDetail.GetYear)
			'frmSource.Show()

			obj.ShowCresusForm()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunCresusExport_0")

		End Try

	End Sub

	Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
    Dim strFullFileName As String = String.Empty
    Dim strFilePath As String = String.Empty
    Dim myStream As Stream = Nothing
    Dim openFileDialog1 As New OpenFileDialog()

    openFileDialog1.Title = strFile2Search
    openFileDialog1.InitialDirectory = strFile2Search
    openFileDialog1.Filter = "EXE-Dateien (*.exe)|*.exe|Alle Dateien (*.*)|*.*"
    openFileDialog1.FilterIndex = 1
    openFileDialog1.RestoreDirectory = True

    If openFileDialog1.ShowDialog() = DialogResult.OK Then
      Try

        myStream = openFileDialog1.OpenFile()
        If (myStream IsNot Nothing) Then
          strFullFileName = openFileDialog1.FileName()

          ' Insert code to read the stream here.
        End If

      Catch Ex As Exception
        MessageBox.Show("Kann keine Daten lesen: " & Ex.Message)
      Finally
        ' Check this again, since we need to make sure we didn't throw an exception on open.
        If (myStream IsNot Nothing) Then
          myStream.Close()
        End If
      End Try
    End If

    Return strFullFileName
  End Function

  Sub RunKommaModul(ByVal strTempSQL As String)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsSystem.GetPersonalFolder() & "SPTranslationProg" & _ClsSystem.GetLogedUSNr()
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "ZG")

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
      objDateiMacher = New StreamWriter(_ClsSystem.GetPersonalFolder() & "FibuList.XML")
      objDateiMacher.Write(sb.ToString)
      objDateiMacher.Close()
      objDateiMacher.Dispose()


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
    strModulCode = _ClsReg.GetINIString(_ClsSystem.GetInitIniFile, "ExtraModuls", CStr(lModulNr))
    If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

    ExtraRights = bAllowed

  End Function



End Module
