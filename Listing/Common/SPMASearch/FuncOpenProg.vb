
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

  Private m_xml As New ClsXML
  Private m_UtilityUI As UtilityUI
  Private m_Logger As ILogger = New Logger()



  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From ExportDb Where ModulName = @GuidNr "
    strSqlQuery += "Order By RecNr"

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Dim strQuery As String = String.Empty
    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@GuidNr", ClsDataDetail.GetAppGuidValue())

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

          mnu.Text = m_xml.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString)
          If Not IsDBNull(rMnurec("ToolTip")) Then
            mnu.ToolTipText = m_xml.GetSafeTranslationValue(rMnurec("ToolTip").ToString)
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
      m_Logger.LogError(e.ToString)
      m_UtilityUI.ShowErrorDialog(e.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return liResult

  End Function



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
				m_Logger.LogError(Ex.ToString)
				m_UtilityUI.ShowErrorDialog(Ex.ToString)
			Finally
				' Check this again, since we need to make sure we didn't throw an exception on open.
				If (myStream IsNot Nothing) Then
					myStream.Close()
				End If
			End Try
		End If

		Return strFullFileName
	End Function

	Sub RuneCallSMSModul(ByVal strTempSQL As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = ClsDataDetail.MDData,
                                                                         .PersonalizedData = ClsDataDetail.ProsonalizedData,
                                                                         .TranslationData = ClsDataDetail.TranslationData,
                                                                         .UserData = ClsDataDetail.UserData}

      Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, strTempSQL, SPS.Export.Listing.Utility.ReceiverType.Employee)
			frmSMS2eCall.LoadData()

			frmSMS2eCall.Show()


    Catch e As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
      m_UtilityUI.ShowErrorDialog(e.Message)

    End Try

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
      If MsgBox("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?", _
                MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
        oMyProg = CreateObject("SPSModulsView.ClsMain")
        oMyProg.ExportDataToOutlook(strTempSQL, "MA")
      End If

    Catch e As Exception
      m_Logger.LogError(e.ToString)
      m_UtilityUI.ShowErrorDialog(e.ToString)

    End Try

  End Sub

  Sub RunKommaModul(ByVal strTempSQL As String)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "MA")

    Catch e As Exception
      m_Logger.LogError(e.ToString)
      m_UtilityUI.ShowErrorDialog(e.ToString)

    End Try

  End Sub

  Sub RunTobitFaxModul(ByVal strTempSQL As String)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "MA", "1")

    Catch e As Exception
      m_Logger.LogError(e.ToString)
      m_UtilityUI.ShowErrorDialog(e.ToString)

    End Try

  End Sub


#End Region



  Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
    Dim bResult As Boolean = False

    ' alle geöffneten Forms durchlaufen
    For Each oForm As Form In Application.OpenForms
      If oForm.Name.ToLower = sName.ToLower Then
        If bDisposeForm Then oForm.Dispose() : Exit For
        bResult = True : Exit For
      End If
    Next

    Return (bResult)
  End Function


	Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
    System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
	End Sub


End Module
