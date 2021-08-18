
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraEditors

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports SP.Infrastructure.Logging
Imports System.Text.RegularExpressions

Public Class frmMessageList
  Inherits DevExpress.XtraEditors.XtraForm

	Private m_Logger As ILogger = New Logger()

  Dim _ClsXML As New ClsXML
  Dim ClsReg As New SPProgUtility.ClsDivReg
  Dim ClsFunc As New ClsDivFunc

  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Dim strConnString As String = _ClsProgSetting.GetConnString
  Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile
  Dim strInitProgFile As String = _ClsProgSetting.GetInitIniFile
  Dim strMDGuid As String = _ClsProgSetting.GetMDGuid

  Dim Conn As SqlConnection

  Private Sub frmMessageList_FormClosing(ByVal sender As Object, _
                                         ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_0", CStr(Me.cboDbField.SelectedIndex))
    ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_1", CStr(Me.cboOperator.SelectedIndex))
    ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_2", CStr(Me.cboMessageArt.SelectedIndex))

    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        My.Settings.frmMsgLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
        My.Settings.iMsgWidth = Me.Width
        My.Settings.iMsgHeight = Me.Height

        My.Settings.Save()
      End If

    Catch ex As Exception
      ' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
    End Try

  End Sub

  Sub StartTranslation()
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Dim Time_1 As Double = System.Environment.TickCount
    Try
      _ClsXML.GetChildChildBez(Me)

    Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try
		Me.Text = TranslateText(Me.Text)

		Try
			For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.XtraTabControl1.TabPages
				tbp.Text = TranslateText(tbp.Text)
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("0=>{0}.{1}", strMethodeName, ex.Message))

		End Try

		If _ClsProgSetting.GetLogedUSNr = 1 Then
			m_Logger.LogInfo(String.Format("{0}. Ladezeit für Translation: {1} s.", strMethodeName, ((System.Environment.TickCount - Time_1) / 1000)))
		End If

	End Sub


	Private Sub frmMessageList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim iLastIndex_0 As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", _
																																	 Me.Name & "_0").ToString))
			Dim iLastIndex_1 As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", _
																																	 Me.Name & "_1").ToString))
			Dim iLastIndex_2 As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", _
																																	 Me.Name & "_2").ToString))

			Try
				' UpdateMethod() enthält den Code, der ein Windows-Steuerelement modifiziert.
				Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
				' UpdateMethod() wird nun auf dem Benutzeroberflächen-Thread aufgerufen.
				Me.Invoke(UpdateDelegate)

			Catch ex As Exception
				m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))

			End Try
			Try
				Me.KeyPreview = True
				Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
				Dim strValue As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, String.Empty)
				If strValue <> String.Empty Then
					Me.LookAndFeel.UseDefaultLookAndFeel = False
					Me.LookAndFeel.UseWindowsXPTheme = False
					Me.LookAndFeel.SkinName = strValue
				End If

				Try
					If My.Settings.frmMsgLocation <> String.Empty Then
						Me.Width = Math.Max(My.Settings.iMsgWidth, Me.Width)
						Me.Height = Math.Max(My.Settings.iMsgHeight, Me.Height)
						Dim aLoc As String() = My.Settings.frmMsgLocation.Split(CChar(";"))

						If Screen.AllScreens.Length = 1 Then
							If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
						End If
						Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

			End Try

			Conn = New SqlConnection(strConnString)
			Conn.Open()

			SetLvwHeader()
			Me.txtSearch.Text = (Today.Date).ToShortDateString

			Me.cboDbField.SelectedIndex = iLastIndex_0
			Me.cboOperator.SelectedIndex = iLastIndex_1
			Me.cboMessageArt.SelectedIndex = iLastIndex_2
			If ClsDataDetail.GetMessageGuid <> String.Empty Then
				ReadLogFile(Me.txtAllMessage, String.Format("{0}{1}.{2}", _ClsProgSetting.GetSpSFiles2DeletePath, _
																											ClsDataDetail.GetMessageGuid, "tmp"))
				If Not Me.txtAllMessage.Text.ToLower.Contains("erfolgreich".ToLower) Then
					Me.XtraTabControl1.TabPages.Remove(Me.xtabDetail)
					Me.cmdOK.Visible = False
				End If
			Else
				Me.XtraTabControl1.TabPages.Remove(Me.xtabZusammen)
			End If


		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "frmMessageList_Load")

		End Try

	End Sub

  Sub ReadLogFile(ByVal MytxtBox As DevExpress.XtraEditors.TextEdit, ByVal strFullFilename As String)
    Dim objReader As New StreamReader(strFullFilename)
    Dim sLine As String = ""
    Dim arrText As New ArrayList()

    Do
      sLine = objReader.ReadLine()
      If Not sLine Is Nothing Then
        arrText.Add(sLine)
      End If
    Loop Until sLine Is Nothing
    objReader.Close()

    For Each sLine In arrText
      MytxtBox.Text &= sLine & vbNewLine
    Next

  End Sub

  Sub SetLvwHeader()
    Dim strColumnString As String = String.Empty
    Dim strColumnWidthInfo As String = String.Empty
    Dim strUSLang As String = ""

    strColumnString = "RecNr;Datum;Betreff;An;Von;AsHtml;Dateiname"
    strColumnWidthInfo = "0-1;150-0;150-0;150-0;150-0;0-0;0-0"

    strColumnString = TranslateText(strColumnString)
    FillDataHeaderLv(Me.lvMail, strColumnString, strColumnWidthInfo)

  End Sub

  Sub FillDataHeaderLv(ByVal Lv As DevComponents.DotNetBar.Controls.ListViewEx,
                       ByRef strColumnList As String, ByRef strColumnInfo As String)
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

      For i As Integer = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = strCaption(i).ToString
        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

        If strFieldInfo(i).ToString.StartsWith("-") Then
          strFieldWidth = strFieldData(1)
          lvwColumn.Width = CInt(strFieldWidth) * -1
          If strFieldData.Length > 1 Then
            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
          End If

        Else
          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
          lvwColumn.Width = CInt(strFieldWidth)
          If strFieldData.Length > 1 Then
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
        .Columns.Add(lvwColumn)
      Next

      lvwColumn = Nothing
    End With

  End Sub

  Sub ListSelectedData()

    Me.lvMail.Items.Clear()
    Me.lvMail.ResumeLayout()
    Me.lvMail.BeginUpdate()

    Try
      BackgroundWorker1.WorkerSupportsCancellation = True
      BackgroundWorker1.WorkerReportsProgress = True
      BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

    Catch ex As Exception
      'MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "ListSelectedData_0")

    End Try

  End Sub

  Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
    If Asc(e.KeyChar) = Keys.Enter Then
      e.Handled = True
      Me.ListSelectedData()
    End If
  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click

    Conn.Close()
    Conn.Dispose()

    Me.Close()
    Me.Dispose()

  End Sub

  Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

    Me.ListSelectedData()

  End Sub

  Private Sub LvMail_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMail.DoubleClick
    Dim frmTest As New MainForm
    Dim strSqlQuery As String = ""
    Dim i As Integer = 0
    Dim _ClsSystemL As New ClsMain_Net
    Dim cForeColor As New System.Drawing.Color

    Conn = New SqlConnection(strConnString)
    Conn.Open()

    Try
      strSqlQuery = "Select Mail.MANr, Mail.KDNr, Mail.KDZNr, Mail.RecNr, Mail.eMail_Subject, Mail.eMail_Body, "
      strSqlQuery += "Mail.eMail_To, Mail.eMail_From, Mail.AsHtml, Mail.AsTelefax, Mail.CreatedOn, "
      strSqlQuery += "Mail.CreatedFrom, Mail_File.FileName, "
      strSqlQuery += "KD.Firma1, KDz.Anrede As KDzAnrede, (KDZ.Nachname + ', ' + KDZ.Vorname) As KDZName, "
      strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName "

      strSqlQuery += "From [{0}].dbo.Mail_Kontakte Mail "
      strSqlQuery += "Left Join [{0}].dbo.Mail_FileScan Mail_File On Mail.RecNr = Mail_File.RecNr "
      strSqlQuery += "Left Join Kunden KD On Mail.KDNr = KD.KDNr "
      strSqlQuery += "Left Join KD_Zustaendig KDz On Mail.KDZNr = KDz.RecNr And Mail.KDNr = KDz.KDNr "
      strSqlQuery += "Left Join Mitarbeiter MA On Mail.MANr = MA.MANr "

      strSqlQuery += "Where Mail.RecNr = @RecNr And Mail.Customer_ID = @Customer_ID"
      strSqlQuery = String.Format(strSqlQuery, ClsDataDetail.GetMailDbName)

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@RecNr", Me.LblChanged.Text)
      param = cmd.Parameters.AddWithValue("@Customer_ID", strMDGuid)
      Dim rKontaktrec As SqlDataReader = cmd.ExecuteReader

      While rKontaktrec.Read

        If Not IsDBNull(rKontaktrec("FileName")) Then
          frmTest.lstAttachments.Enabled = True
          frmTest.lstAttachments.Items.Add(rKontaktrec("FileName"))
        Else
          frmTest.lstAttachments.Enabled = False
          frmTest.lstAttachments.Items.Clear()

        End If
        frmTest.LblChanged_0.Text = (rKontaktrec("CreatedOn").ToString)
        frmTest.LblChanged.Text = rKontaktrec("RecNr").ToString
        frmTest.txtFrom.Text = rKontaktrec("eMail_From").ToString
        frmTest.txtTo.Text = rKontaktrec("eMail_To").ToString
        frmTest.txtSubject.Text = rKontaktrec("eMail_Subject").ToString

        Dim strModulName As String = "Kundendaten"
        Dim strData As String = String.Empty
        If IsDBNull(rKontaktrec("Firma1")) Then
          strModulName = String.Empty

          If Not IsDBNull(rKontaktrec("MAName")) Then
            strModulName = "Kandidatendaten"
            strData = rKontaktrec("MAName").ToString

          Else
            strModulName = String.Empty
            strData = String.Empty

          End If

        Else
          strData = String.Format("{1}{0}{2} {3}", vbNewLine, rKontaktrec("Firma1").ToString,
                                 If(IsDBNull(rKontaktrec("KDzAnrede")), "", rKontaktrec("KDzAnrede").ToString),
                                 If(IsDBNull(rKontaktrec("KDzName")), "", rKontaktrec("KDzName").ToString))

        End If
        frmTest.lblKDMAData.Text = If(strModulName & strData = String.Empty, String.Empty,
                                      String.Format("<b>{1}:</b>{0}{2}", vbNewLine, strModulName, strData))

        frmTest.wbHtml.Visible = True
        frmTest.Label2.Visible = True
        If CInt(rKontaktrec("AsHtml")) = 0 Then
          frmTest.rtxtText.LoadFile(rKontaktrec("eMail_Body").ToString, RichTextBoxStreamType.PlainText)
          frmTest.wbHtml.Visible = False
          frmTest.rtxtText.Visible = True

        Else
          frmTest.wbHtml.DocumentText = rKontaktrec("eMail_Body").ToString
          frmTest.wbHtml.Visible = True
          frmTest.rtxtText.Visible = False

        End If

        If Not IsDBNull(rKontaktrec("AsTelefax")) Then
          If CBool(rKontaktrec("AsTelefax")) Then
            frmTest.rtxtText.Visible = False
            frmTest.wbHtml.Visible = False
            frmTest.Label2.Visible = False

            frmTest.wbHtml.Visible = False
            frmTest.Label2.Visible = False
          End If
        End If

        frmTest.wbHtml.Update()
        frmTest.Show()

        i += 1
      End While

    Catch ex As Exception
      Me.lvMail.Items.Clear()
      MsgBox(ex.Message)

    Finally
      Conn.Close()

    End Try

  End Sub

  Private Sub LvMail_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvMail.SelectedIndexChanged

    If Me.lvMail.SelectedItems.Count > 0 Then
      Me.LblChanged.Text = Me.lvMail.Items(Me.lvMail.FocusedItem.Index).Text              ' RecNr
      Me.LblChanged_1.Text = Me.lvMail.Items(Me.lvMail.FocusedItem.Index).SubItems(5).Text    ' IsAsHtml
      Me.LblChanged_2.Text = Me.lvMail.Items(Me.lvMail.FocusedItem.Index).SubItems(6).Text    ' FileName
    End If

  End Sub

  Private Sub cboMessageArt_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMessageArt.SelectedValueChanged

    Me.ListSelectedData()

  End Sub

#Region "Multitreading..."

  Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, _
                                       ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    CheckForIllegalCrossThreadCalls = False
    Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

    Dim strOperator As String = "="
    Dim strSqlQuery As String = ""
    Dim i As Integer = 0
    Dim ListItem As ListViewItem
    Dim cForeColor As New System.Drawing.Color
    Dim _ClsSystemL As New ClsMain_Net

    Me.cmdOK.Enabled = False
    Me.LblWaitPic.Visible = True
    Me.LblWait.Visible = Not Me.LblWaitPic.Visible
    Me.StatusStrip1.Update()
    Conn = New SqlConnection(strConnString)
    Conn.Open()

    Try
      strOperator = Me.cboOperator.Text
      strSqlQuery = "Select Mail.RecNr, Mail.eMail_Subject, "
      strSqlQuery += "Mail.eMail_To, Mail.eMail_From, Mail.AsHtml, Mail.AsTelefax, Mail.CreatedOn, "
      strSqlQuery += "Mail.CreatedFrom, Mail_File.FileName "
      strSqlQuery += "From [{0}].dbo.Mail_Kontakte Mail Left Join [{0}].dbo.Mail_FileScan Mail_File On "
      strSqlQuery += "Mail.RecNr = Mail_File.RecNr "
      strSqlQuery += "And Mail.eMail_To = Mail_File.eMail_To And Mail.Message_ID = Mail_File.Message_ID "
      strSqlQuery += "Where Mail.Customer_ID = @Customer_ID And Mail.Message_ID "
      If ClsDataDetail.GetMessageGuid() <> "" Then
        strSqlQuery += "= @Message_ID "
      Else
        strSqlQuery += "<> '' "
      End If


      If Me.txtSearch.Text <> String.Empty Then
        strSqlQuery += "And "
        If Me.cboDbField.SelectedIndex = 0 Then
          '          strSqlQuery += "Convert(nvarchar(10), Mail.CreatedOn, 104) " & strOperator
          strSqlQuery += "Mail.CreatedOn " & strOperator
        Else
          strSqlQuery += "Mail." & Me.cboDbField.Text & " " & strOperator

        End If
        strSqlQuery += "@CreatedOn "

        If Me.cboMessageArt.SelectedIndex = 1 Then
          strSqlQuery += "And AsTelefax = 0 "
        ElseIf Me.cboMessageArt.SelectedIndex = 2 Then
          strSqlQuery += "And AsTelefax = 1 "

        End If

      End If
      strSqlQuery += "Order by Mail.CreatedOn Desc"
      strSqlQuery = String.Format(strSqlQuery, ClsDataDetail.GetMailDbName)

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@Customer_ID", strMDGuid)
      If ClsDataDetail.GetMessageGuid() <> "" Then param = cmd.Parameters.AddWithValue("@Message_ID", _
                                                                                        ClsDataDetail.GetMessageGuid())
      If Me.txtSearch.Text <> String.Empty Then
        If IsDate(Me.txtSearch.Text) Then
          param = cmd.Parameters.AddWithValue("@CreatedOn", CType(CDate(Me.txtSearch.Text), Date).ToShortDateString & " 00:00")
        Else
          param = cmd.Parameters.AddWithValue("@CreatedOn", Me.txtSearch.Text & " 00:00")
        End If
      End If

      Dim rKontaktrec As SqlDataReader = cmd.ExecuteReader
      While rKontaktrec.Read
        If IsDBNull(rKontaktrec("FileName")) Then
          cForeColor = Color.BlueViolet
        Else
          cForeColor = Color.Cyan
        End If
        Me.lvMail.Items.Add(rKontaktrec("RecNr").ToString)
        Me.lvMail.Items(i).SubItems.Add(rKontaktrec("CreatedOn").ToString)

        Me.lvMail.Items(i).SubItems.Add(rKontaktrec("eMail_Subject").ToString)
        Me.lvMail.Items(i).SubItems.Add(rKontaktrec("eMail_To").ToString)
        Me.lvMail.Items(i).SubItems.Add(rKontaktrec("eMail_From").ToString)
        If CInt(rKontaktrec("AsHtml")) = 0 Then
          Me.lvMail.Items(i).SubItems.Add("0")
        Else
          Me.lvMail.Items(i).SubItems.Add("1")
        End If
        Me.lvMail.Items(i).SubItems.Add(rKontaktrec("Filename").ToString)

        If Not IsDBNull(rKontaktrec("FileName")) Then
          For Each ListItem In Me.lvMail.Items
            ListItem.ForeColor = Color.BlueViolet
          Next
        End If
        If Not IsDBNull(rKontaktrec("AsTelefax")) Then
          If CBool(rKontaktrec("AsTelefax").ToString) Then
            For Each ListItem In Me.lvMail.Items
              ListItem.BackColor = Color.Coral
            Next
          End If
        End If
        i += 1

      End While


    Catch ex As Exception
      Me.lvMail.Items.Clear()
      MsgBox(ex.Message)

    Finally
      Conn.Close()
      e.Result = True
      If bw.CancellationPending Then e.Cancel = True

    End Try

  End Sub

  Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
    Trace.WriteLine(e.ToString)
  End Sub

  Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

    If (e.Error IsNot Nothing) Then
      MessageBox.Show("Fehler in Ihrer Anwendung." & vbNewLine & e.Error.Message)
    Else
      If e.Cancelled = True Then
        Me.cmdOK.Enabled = True
        MessageBox.Show("Aktion abgebrochen!")

      Else
        BackgroundWorker1.CancelAsync()
        '        MessageBox.Show(e.Result.ToString())
        Dim i As Integer = Me.lvMail.Items.Count
        Me.ResumeLayout()
        Me.LblWaitPic.Visible = False
        Me.LblWait.Text = String.Format("Bereit, {0} Datens{1} wurde{2} gefunden...", i, _
                                        If(i > 1, "ätze", "atz"), If(i > 1, "n", ""))
        Me.LblWait.Visible = Not Me.LblWaitPic.Visible
        Me.StatusStrip1.Update()

      End If

      Me.cmdOK.Enabled = True
      System.Media.SystemSounds.Asterisk.Play()

      Me.lvMail.EndUpdate()
      Me.lvMail.ResumeLayout()

    End If

  End Sub

#End Region



End Class