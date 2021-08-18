
Option Strict Off

Imports System.Globalization
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports SPYFakListSearch.ClsDataDetail


Module FuncOpenProg

  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    '    Dim strFieldName As String = "Bezeichnung"
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From ExportDb Where ModulName = @GuidNr "
    strSqlQuery += "Order By RecNr"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@GuidNr", ClsDataDetail.GetAppGuidValue())

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

  Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, _
                        ByVal d3600 As Decimal, _
                        ByVal d3602 As Decimal, _
                        ByVal d3650 As Decimal, _
                        ByVal d3700 As Decimal, _
                        ByVal d3750 As Decimal, _
                        ByVal d3800 As Decimal, _
                        ByVal d3850 As Decimal, _
                        ByVal d3900 As Decimal, _
                        ByVal d3900_1 As Decimal, _
                        ByVal d3901 As Decimal, _
                        ByVal d3901_1 As Decimal)
    Dim i As Integer = 0

    Try
      tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem

      If d3600 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3600): {1}", GetLAName(3600), Format(d3600, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If d3602 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3602): {1}", GetLAName(3602), Format(d3602, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If d3650 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3650): {1}", GetLAName(3650), Format(d3650, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If
      If d3700 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3700): {1}", GetLAName(3700), Format(d3700, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If
      If d3750 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3750): {1}", GetLAName(3750), Format(d3750, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If d3800 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3800): {1}", GetLAName(3800), Format(d3800, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If
      If d3850 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3850}: {1}", GetLAName(3850), Format(d3850, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If
      If d3900 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3900): {1}", GetLAName(3900), Format(d3900, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If d3900_1 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3900.10): {1}", GetLAName(3900.1), Format(d3900_1, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If d3901 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3901): {1}", GetLAName(3901), Format(d3901, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      If d3901_1 <> 0 Then
        mnu = New ToolStripMenuItem()
        mnu.Text = String.Format("{0} (3901.10): {1}", GetLAName(3901.1), Format(d3901_1, "n"))
        tsbMenu.DropDownItems.Add(mnu)
      End If

      tsbMenu.DropDownItems.Add(New ToolStripSeparator)

      mnu = New ToolStripMenuItem()
      Dim dValue As Double = d3600 + d3602 + d3650 + d3700 + d3750 + d3800 + d3850 + d3900 + d3900_1 + d3901 + d3901_1
			mnu.Text = String.Format(m_Translate.GetSafeTranslationValue("Total: {0}"), Format(dValue, "n"))
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


  Sub RunMailModul(ByVal strTempSQL As String)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
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

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      If MsgBox("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?", _
                MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
        oMyProg = CreateObject("SPSModulsView.ClsMain")
        oMyProg.ExportDataToOutlook(strTempSQL, "KD")
      End If

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
