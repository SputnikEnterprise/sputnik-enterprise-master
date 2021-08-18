
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Module FuncOpenProg

  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Dim strMDPath As String = ""
  Dim strInitPath As String = ""

  Dim iLogedUSNr As Integer = 0

  Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

  Dim strConnString As String = _ClsProgSetting.GetConnString()
  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    '    Dim strFieldName As String = "Bezeichnung"
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From KDExportDb Where ModulName = 'KD' "
    strSqlQuery += "Order By RecNr"

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

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


  Sub RunKommaModul(ByVal strTempSQL As String)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "KD")

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
