
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SP.Infrastructure.Logging


Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SPS.SYS.DocUtility.ClsDataDetail


Module FuncOpenProg
	Private m_Logger As ILogger = New Logger()

  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Dim strMDPath As String = ""
  Dim strInitPath As String = ""

  Dim iLogedUSNr As Integer = 0

  Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim i As Integer = 0
		Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From MAExportDb Where ModulName = 'MA' "
    strSqlQuery += "And MnuName <> 'XML' Order By RecNr"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMnurec As SqlDataReader = cmd.ExecuteReader

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

      End While
      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()


    Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
      MsgBox(Err.GetException.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

#Region "Funktionen für Exportieren..."


	' Sub RunBewModul(ByVal strTempSQL As String)
	'   Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'   Dim oMyProg As Object
	'   Dim strTranslationProgName As String = String.Empty

	'   strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'   _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
	'   _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

	'   Try
	'     oMyProg = CreateObject("SPSBewUtility.ClsMain")
	'     oMyProg.OpenKDFieldsform(strTempSQL)

	'   Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		MessageBoxShowError(strMethodeName, ex)

	'	End Try

	'End Sub

	'Sub RunOpenMAForm(ByVal iMANr As Integer)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim oMyProg As Object

	'	Try
	'		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MANr", iMANr.ToString)

	'		oMyProg = CreateObject("SPSModulsView.ClsMain")
	'		oMyProg.TranslateProg4Net("KandidatUtility.ClsMain", iMANr.ToString)

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		MessageBoxShowError(strMethodeName, ex)

	'	End Try

	'End Sub

	'Sub RunOpenESForm()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim _setting As New SP.ES.PrintUtility.ClsESSetting With {.SelectedMonth = New List(Of Short)(New Short() {CShort(Now.Month)}), _
	'																														.SelectedYear = New List(Of Integer)(New Integer() {CInt(Now.Year)}), _
	'																														.SelectedESNr = New List(Of Integer)(New Integer() {0})}
	'	Dim o2Open As New SP.ES.PrintUtility.ClsMain_Net(_setting)

	'	Try
	'		o2Open.ShowfrmES4Print()
	'		'oMyProg = CreateObject("SPSModulsView.ClsMain")
	'		'oMyProg.TranslateProg4Net("SPSESUtil.ClsMain")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		MessageBoxShowError(strMethodeName, ex)

	'	End Try

	'End Sub

	'Sub RunOpenAuszahlungCheckForm()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim oMyProg As Object
	'	Dim strTranslationProgName As String = String.Empty
	'	Try

	'		oMyProg = CreateObject("SPSModulsView.ClsMain")
	'		oMyProg.TranslateProg4Net("SPSCPrintUtil.ClsMain")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		MessageBoxShowError(strMethodeName, ex)

	'	End Try

	'End Sub

	'Sub RunOpenAuszahlungQuittungenForm()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim oMyProg As Object
	'	Dim strTranslationProgName As String = String.Empty
	'	Try

	'		oMyProg = CreateObject("SPSModulsView.ClsMain")
	'		oMyProg.TranslateProg4Net("SPSQPrintUtil.ClsMain")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		MessageBoxShowError(strMethodeName, ex)

	'	End Try

	'End Sub

	'Sub RunOpenAuszahlungKontoForm()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim oMyProg As Object
	'	Dim strTranslationProgName As String = String.Empty
	'	Try

	'		oMyProg = CreateObject("SPSModulsView.ClsMain")
	'		oMyProg.TranslateProg4Net("SPSKontoPrintUtil.ClsMain")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		MessageBoxShowError(strMethodeName, ex)

	'	End Try

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

	Sub RunSMSProg(ByVal strQuery As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		' Umstellung von der neuen SQL-Query wieder zur alten Version.
		strQuery = strQuery.Replace("MANachname", "Nachname").Replace("MAVorname", "Vorname")

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
			MsgBox("Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus." & vbLf & _
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

		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMailUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSMailUtility.ClsMain", strTempSQL)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		End Try

	End Sub

	Sub ExportDataToOutlook(ByVal strTempSQL As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			If MsgBox("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
				oMyProg = CreateObject("SPSModulsView.ClsMain")
				oMyProg.ExportDataToOutlook(strTempSQL, "MA")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		End Try

	End Sub

	Sub RunKommaModul(ByVal strTempSQL As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "MA")

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		End Try

	End Sub

	Sub RunTobitFaxModul(ByVal strTempSQL As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "MA", "1")

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))

		End Try

	End Sub


#End Region


	Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
		System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
	End Sub


End Module
