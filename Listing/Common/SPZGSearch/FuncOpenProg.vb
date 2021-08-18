
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO


Module FuncOpenProg

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    '    Dim strFieldName As String = "Bezeichnung"
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From ZGExportDb Where ModulName = 'ZG' "
    strSqlQuery += "Order By RecNr"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

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

	Function GetMenuItems4Export() As List(Of String)
		Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr",
																							ClsDataDetail.GetAppGuidValue)
		Dim liResult As New List(Of String)

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMnurec As SqlDataReader = cmd.ExecuteReader

      While rMnurec.Read

				liResult.Add(String.Format("{0}#{1}#{2}", ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
																		 ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("MnuName").ToString),
																		 ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("Docname").ToString)))

			End While


    Catch e As Exception
      MsgBox(e.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return liResult

  End Function

  Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, ByVal dBetrag_1 As Double)
    Dim i As Integer = 0

    Try
      tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem

      mnu = New ToolStripMenuItem()
      mnu.Text = "Totalbetrag: " & Format(dBetrag_1, "###,###,###,###,###,###,0.00")
      tsbMenu.DropDownItems.Add(mnu)

      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()

    Catch e As Exception
      MsgBox(Err.GetException.ToString)

    Finally

    End Try

  End Sub

#Region "Funktionen für Exportieren..."

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

	Sub RunSMSProg()
		Dim strProgPath As String
		Dim strSMSProgName As String = "Sputnik Suite SMS.EXE"

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
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMailUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSMailUtility.ClsMain", strTempSQL)

		Catch e As Exception

		End Try

	End Sub

	Sub ExportDataToOutlook(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			If MsgBox("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
				oMyProg = CreateObject("SPSModulsView.ClsMain")
				oMyProg.ExportDataToOutlook(strTempSQL, "KD")
			End If

		Catch e As Exception

		End Try

	End Sub

	Sub RunKommaModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "ZG")

		Catch e As Exception

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
