
Option Strict Off

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Module FuncOpenProg

  Private _ClsFunc As New ClsDivFunc
  Private _ClsReg As New SPProgUtility.ClsDivReg
  'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Private m_xml As New ClsXML

  Private m_UtilityUI As UtilityUI
  Private m_Logger As ILogger = New Logger()


  ''' <summary>
  ''' Add a control to the L List by name.
  ''' Use * at front or at the end if you know only a fragment 
  ''' of the controls name.
  ''' </summary>
  ''' <param name="BaseControl"></param>
  ''' <param name="Key"></param>
  ''' <param name="L"></param>
  ''' <returns>True if one or more controls are found with the 
  ''' requested name</returns>
  ''' <remarks></remarks>
  Public Function MyGetControl(ByVal BaseControl As Control, _
                               ByVal Key As String, _
                               ByRef L As List(Of Control), _
                               Optional ByVal ReturnAtFirstElement As Boolean = False) As Boolean
    If L Is Nothing Then L = New List(Of Control)
    Dim Gut As Boolean
    Dim ReturnFlag As Boolean = False
    If Key IsNot Nothing Then Key = Key.ToLower

    If BaseControl.HasChildren = True Then
      For Each ctl As Control In BaseControl.Controls
        Gut = False
        If Key Is Nothing Then
          Gut = True
        Else
          If ctl.Name.Length >= Key.Length Then
            Key = Key.ToLower
            If Key.StartsWith("*") Then
              If Key.Substring(1) = ctl.Name.ToLower.Substring(ctl.Name.Length - (Key.Length - 1), _
                                                               Key.Length - 1) Then Gut = True
            ElseIf Key.EndsWith("*") Then
              If Key.Substring(0, Key.Length - 1) = ctl.Name.ToLower.Substring(0, Key.Length - 1) Then Gut = True
            Else
              If Key = ctl.Name.ToLower Then Gut = True
            End If
          End If
        End If

        If Gut = True Then
          L.Add(ctl)
          If ReturnAtFirstElement = True Then ReturnFlag = True
        End If
        If ReturnFlag = False Then
          Call MyGetControl(ctl, Key, L)
        End If
      Next
    End If

    If L.Count - 1 > -1 Then
      Return True
    Else
      Return False
    End If
  End Function

  Sub ShowFrmTemplatePopUpMenu(ByVal tsbMenu As ToolStripDropDownButton)

    'Dim tsbMenu As New ToolStripDropDownButton

    Dim mnu As ToolStripMenuItem
    mnu = New ToolStripMenuItem()
    Dim lioFiles As List(Of String) = GetFileList(GetUSFormControlDataPath(), False)

    tsbMenu.DropDownItems.Clear()
    tsbMenu.DropDown.SuspendLayout()

    For i As Integer = 0 To lioFiles.Count - 1
      mnu = New ToolStripMenuItem()
      mnu.Text = System.IO.Path.GetFileName(lioFiles(i))
      mnu.Name = lioFiles(i)

      tsbMenu.DropDownItems.Add(mnu)
    Next i
    tsbMenu.DropDown.ResumeLayout()
    tsbMenu.ShowDropDown()

  End Sub

  Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
    System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
  End Sub

  Public Function GetFileList(ByVal Root As String, Optional ByVal SubFolders As Boolean = True) As List(Of String)
    Dim FileList As New List(Of String)

    SeekFiles(Root, FileList, SubFolders)

    Return FileList
  End Function

  Private Sub SeekFiles(ByVal Root As String, ByRef FileArray As List(Of String), ByVal SubFolders As Boolean)

    Try
      Dim Files() As String = System.IO.Directory.GetFiles(Root)
      Dim Folders() As String = System.IO.Directory.GetDirectories(Root)

      For i As Integer = 0 To UBound(Files)
        FileArray.Add(Files(i).ToString)
      Next

      If SubFolders = True Then
        For i As Integer = 0 To UBound(Folders)
          SeekFiles(Folders(i), FileArray, SubFolders)
        Next
      End If


    Catch Ex As Exception
      m_Logger.LogError(Ex.Message)
      m_UtilityUI.ShowErrorDialog(Ex.Message)

    End Try

  End Sub


  Function GetMenuItems4Export() As List(Of String)
    Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr", _
                                              ClsDataDetail.GetAppGuidValue)
    Dim liResult As New List(Of String)

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMnurec As SqlDataReader = cmd.ExecuteReader

      While rMnurec.Read

        liResult.Add(String.Format("{0}#{1}#{2}", m_xml.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
                                     m_xml.GetSafeTranslationValue(rMnurec("MnuName").ToString),
                                     m_xml.GetSafeTranslationValue(rMnurec("Docname").ToString)))

      End While


    Catch e As Exception
      m_Logger.LogError(e.Message)
      m_UtilityUI.ShowErrorDialog(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return liResult

  End Function


#Region "Funktionen für Exportieren..."

	'Sub RunOffMailingModul(ByVal strTempSQL As String)
	'  Dim ProgObj As New SPSOfferUtility_Net.ClsMain_Net
	'  ProgObj.ShowMainForm(strTempSQL)

	'End Sub

	Sub RunBewModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSBewUtility.ClsMain")
      oMyProg.OpenKDFieldsform(strTempSQL)

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

  End Sub

	'Sub RunOpenKDForm(ByVal iKDNr As Integer)
	'  Dim oMyProg As Object
	'  Dim strTranslationProgName As String = String.Empty

	'  strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDNr.ToString)

	'  Try
	'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "KDNr", iKDNr.ToString)

	'    oMyProg = CreateObject("SPSModulsView.ClsMain")
	'    oMyProg.TranslateProg4Net("KundenUtility.ClsMain", iKDNr.ToString)

	'  Catch e As Exception
	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenKDForm")

	'  End Try

	'End Sub

	'Sub RunOpenKDZhdForm(ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
	'  Dim oMyProg As Object
	'  Dim strTranslationProgName As String = String.Empty

	'  strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)

	'  Try
	'    oMyProg = CreateObject("SPSModulsView.ClsMain")
	'    oMyProg.TranslateProg4Net("SPSKDZHD.ClsMain", iKDNr.ToString, iKDZhdNr.ToString)

	'  Catch e As Exception
	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenKDZhdForm")

	'  End Try

	'End Sub

	'Sub RunTapi_KDZhd(ByVal strNumber As String, ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
	'  Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	''Dim strTranslationProgName As String = String.Empty

	''_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
	''_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)

	'Try
	'	Dim iTest As Integer = 0
	'	Dim oMyProg As New SPSTapi.ClsMain_Net
	'	iTest = oMyProg.ShowfrmTapi(strNumber, 0, iKDNr.ToString, iKDZhdNr.ToString, iTest)

	'Catch e As Exception
	'	m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
	'    m_UtilityUI.ShowErrorDialog(e.Message)

	'  End Try

	'End Sub

	Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
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
				MessageBox.Show(String.Format(TranslateMyText("Kann keine Daten lesen: {0}"), Ex.Message))
			Finally
				' Check this again, since we need to make sure we didn't throw an exception on open.
				If (myStream IsNot Nothing) Then
					myStream.Close()
				End If
			End Try
		End If

		Return strFullFileName
  End Function

  Sub RunSMSProg(ByVal strQuery As String)
    Dim strProgPath As String
    Dim strSMSProgName As String = "Sputnik Suite SMS.EXE"
    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", _
                              "SQLQuery", strQuery)

    Dim strSMSFile As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg")
    If strSMSFile = String.Empty Then
      strProgPath = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath")
      strProgPath = _ClsReg.AddDirSep(strProgPath) & "Binn\"

      If strSMSFile = String.Empty Then strSMSFile = strProgPath & strSMSProgName
    End If

    If Not File.Exists(strSMSFile) Then
      MsgBox(TranslateMyText("Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus.") & vbLf & _
              (strSMSFile), MsgBoxStyle.Critical, "Programm wurde nicht gefunden")

      strSMSFile = ShowMyFileDlg(strSMSFile)
      If strSMSFile <> String.Empty Then
        _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg", strSMSFile)
        Process.Start(strSMSFile)
      End If

    Else
      Process.Start(strSMSFile)

    End If

  End Sub

  Sub RunMailModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMailUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSMailUtility.ClsMain", strTempSQL)

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

  End Sub

  Sub ExportDataToOutlook(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      If MsgBox(TranslateMyText("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?"), _
                MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
        oMyProg = CreateObject("SPSModulsView.ClsMain")
        oMyProg.ExportDataToOutlook(strTempSQL, "KD")
      End If

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

  End Sub

  Sub RunKommaModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "KD")

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

  End Sub

  Sub RunTobitFaxModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "KD", "1")

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)
    End Try

  End Sub

  Sub RuneCallSMSModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = ClsDataDetail.MDData,
                                                                         .PersonalizedData = ClsDataDetail.ProsonalizedData,
                                                                         .TranslationData = ClsDataDetail.TranslationData,
                                                                         .UserData = ClsDataDetail.UserData}

      Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, strTempSQL, SPS.Export.Listing.Utility.ReceiverType.Customer)
			frmSMS2eCall.LoadData()

			frmSMS2eCall.Show()


    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

  End Sub

  Sub RuneCallFaxModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
                                                                          .SQL2Open = strTempSQL,
                                                                          .SelectedMDNr = ClsDataDetail.ProgSettingData.SelectedMDNr,
                                                                          .LogedUSNr = ClsDataDetail.ProgSettingData.LogedUSNr,
                                                                          .SelectedMDYear = ClsDataDetail.ProgSettingData.SelectedMDYear}

      Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
      obj.ShowFax2eCallForm()

    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

  End Sub


#End Region



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


End Module
