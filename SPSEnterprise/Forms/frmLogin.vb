
Imports System.Reflection.Assembly

Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports NLog
Imports DevExpress.LookAndFeel
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten


Public Class frmLogin

	Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath
	Private _ClsReg As SPProgUtility.ClsDivReg
	Private _ClsSystem As ClsMain_Net
	Private _ClsSetting As ClsMDData
	Private m_md As Mandant

	Private m_utility As Utilities
  Private sLogTry As Short
	Private m_UserNumber As Integer
	Private m_UserPass As String


	Private Const SPUTNIK_ADMIN_USER_NAME As String = "username"
	Private Const SPUTNIK_ADMIN_USER_PASS As String = "password"

	Public Property DoAutoLogin As Boolean
	Public Property FormTop As Integer
	Public Property FormLeft As Integer


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsMDData)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_utility = New Utilities
		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath
		_ClsReg = New SPProgUtility.ClsDivReg
		_ClsSystem = New ClsMain_Net
		_ClsSetting = New ClsMDData
		m_md = New Mandant


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._ClsSetting = _setting

		TranslateControls()

	End Sub

#End Region


  Private Sub CmdXPCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXPCancel.Click
    Me.Close()
  End Sub

  Private Sub CmdXPOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXPOK.Click

		Dim success As Boolean = True

		success = success AndAlso DoLoging()
		success = success AndAlso InsertLogToUSLog(Me.txtUserName.Text, success)

		If Not success AndAlso sLogTry >= 4 Then Me.Close()
		If ExistsCurrentYearData AndAlso success Then
			StartMainView()

			frmSelectMD.Close()
			frmSelectMD = Nothing

			End

		End If

	End Sub






	Public Function DoLoging() As Boolean
		Dim success As Boolean = False

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim sql As String

		Dim enteredName As String = Me.txtUserName.EditValue
		Dim enteredPass As String = Me.txtPassword.EditValue
		Dim encryptedName As String = String.Empty
		Dim encryptedPass As String = String.Empty

		Dim strUSeMail As String = String.Empty
		Dim strResult As String = String.Empty
		Dim strUSLang As String = String.Empty


		sql = "Update Benutzer Set Transfered_Guid = NewID() Where Transfered_Guid = '' Or Transfered_Guid Is Null; "
		sql &= " Select AktivUntil, PW, ISNULL(eMail, '') eMail, USNr, MDNr, SecLevel, USLanguage,"
		sql &= " USKst2, USKst1, KST, Vorname, Nachname, Transfered_Guid"
		sql &= " From Benutzer Where [US_Name] = @strEnteredName And ISNULL(Deaktiviert, 0) = 0 And ISNULL(AsCostCenter, 0) = 0"
		sql &= " AND (AktivUntil IS NULL OR convert(DATE, AktivUntil, 104) >= CONVERT(DATE, GETDATE(), 104))"

		Dim Conn As New SqlConnection(Me._ClsSetting.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		DevComponents.DotNetBar.ToastNotification.Close(Me)
		DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor = DevComponents.DotNetBar.eToastGlowColor.Red
		DevComponents.DotNetBar.ToastNotification.ToastFont = New System.Drawing.Font("tahoma", 8.25F, System.Drawing.FontStyle.Bold)
		DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval = 2000

		If DoAutoLogin Then
			enteredName = SPUTNIK_ADMIN_USER_NAME
			enteredPass = SPUTNIK_ADMIN_USER_PASS
		End If

		If String.IsNullOrWhiteSpace(enteredName) OrElse String.IsNullOrWhiteSpace(enteredPass) Then
			DevComponents.DotNetBar.ToastNotification.Show(Me,
																										 GetSafeTranslationValue(strMessage),
																										 Nothing, DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval,
																										 DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor,
																										 DevComponents.DotNetBar.eToastPosition.BottomCenter)
			DoAutoLogin = False

			Return success
		End If

		encryptedName = EncryptMyString(UCase(enteredName), strEncryptionKey)
		encryptedPass = EncryptMyString(enteredPass & strExtraPass, strEncryptionKey)
		m_UserPass = encryptedPass

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@strEnteredName", encryptedName)
			Dim rUSrec As SqlDataReader = cmd.ExecuteReader
			rUSrec.Read()

			sLogTry += 1
			If rUSrec.HasRows() Then
				If encryptedPass = rUSrec("PW") Then success = True

			End If
			If Not success Then
				strMessage = GetSafeTranslationValue("Benutzername oder Kennwort ungültig.")

				DevComponents.DotNetBar.ToastNotification.Show(Me,
																			 GetSafeTranslationValue(strMessage),
																			 Nothing, DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval,
																			 DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor,
																			 DevComponents.DotNetBar.eToastPosition.BottomCenter)
				rUSrec.Close()
				DoAutoLogin = False

				Return success

			End If

			Try
				strUSeMail = rUSrec("eMail")
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str550, strUSeMail)
				m_UserNumber = CInt(rUSrec("USNr"))

				success = success AndAlso GetUSFiliale(m_userNumber)

				' Mandantennummer in der Liste
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str290, _ClsData.MDID) ' frmSelectMD.LblChanged.Text)
				SelectedMDNr = CInt(rUSrec("MDNr").ToString)

				ClsPublicData.MDData = ClsPublicData.SelectedMDData(SelectedMDNr)
				If ClsPublicData.MDData.MDMainPath Is Nothing Then
					logger.Error(String.Format("({0}) Mandantenpath konnte nicht gefunden werden!", SelectedMDNr))
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("({0}) Mandantenpath konnte nicht gefunden werden!", SelectedMDNr),
																										 GetSafeTranslationValue("Mandantendaten"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
					Return False
				End If
				SelMDMainPath = ClsPublicData.MDData.MDMainPath
				SelMDYearPath = Path.Combine(SelMDMainPath, Year(Now)) & "\"

			Catch ex As Exception
				logger.Error(String.Format("Mandantdaten konnten nicht erneut geladen werden!!! {0}", SelectedMDName))
			End Try

			With rUSrec

				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str261, SelMDYearPath)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str435, SelMDMainPath)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str265, CStr(Year(Today)))


				' Mandantennummer
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str268, SelectedMDNr)

				' Mandantenname
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str103, SelectedMDName)

				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str267, m_UserNumber)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str289, CStr(rUSrec("SecLevel")))
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str374, rUSrec("USKst2").ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str408, rUSrec("USKst1").ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str409, rUSrec("Kst").ToString)

				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str375, rUSrec("Vorname").ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str376, rUSrec("Nachname").ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "MyGuid", rUSrec("Transfered_Guid").ToString)

				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "USMainMDNr", SelectedMDNr)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "SelectedMDGroupNr", SelectedMDGroupNr.ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "SelectedFileServerPath", SelectedFileServerPath.ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "SelectedDBName", String.Format("[{0}]", SelectedDbName.ToString))

				If Not IsDBNull(rUSrec("USLanguage")) Then
					strUSLang = Mid(rUSrec("USLanguage").ToString.ToUpper, 1, 1)
					If strUSLang.Contains("F") Or strUSLang.Contains("I") Then
						strUSLang = strUSLang
					Else
						strUSLang = String.Empty
					End If
				End If
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "USLanguage", strUSLang)

				Dim aIP As Array = GetIP().ToArray
				Dim strMyIP As String = String.Empty
				For i As Integer = 0 To aIP.Length - 1
					strMyIP = IIf(aIP(i).ToString.Length < 16, aIP(i).ToString, "")
				Next

				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str548, strMyIP)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str549, GetIPHostName())


				Dim CheckFile As New FileInfo(My.Application.Info.DirectoryPath)
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str134, AddDirSep(CheckFile.Directory.ToString))

			End With

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			strMessage = String.Format("{1}:{0}{2}", vbNewLine, GetSafeTranslationValue("Fehler bei der Anmeldung."), ex.ToString())
			DevExpress.XtraEditors.XtraMessageBox.Show(strMessage, GetSafeTranslationValue("Anmeldung"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Return success

		End Try

		Try
			If Not ExistsCurrentYearData Then
				If IsUserActionAllowed(m_UserNumber, 651) Then
					Dim _ClsSec As New SPProgUtility.SPUserSec.ClsUserSec
					Dim _ClsNewMD As New SPS.MD.CreateNewUtility.ClsMain_Net(New SPS.MD.CreateNewUtility.ClsSetting With {.LogedUSNr = m_UserNumber, .SelectedMDNr = SelectedMDNr, .SelectedMDYear = Now.Year, .SelectedMDGuid = SelMDCustomer_ID})
					_ClsNewMD.ShowfrmCreateNewYear()
					sLogTry = 4

					Return success

				Else
					strMessage = "Für das aktuelle Jahr existieren keine Daten.{0}"
					strMessage &= "Da Sie keine Berechtigung dazu haben, bitte kontaktieren Sie Ihrem Systemadministrator."
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(GetSafeTranslationValue(strMessage), vbNewLine), _
																																		 GetSafeTranslationValue("Alte Daten"), MessageBoxButtons.OK, _
																																		 MessageBoxIcon.Warning)

				End If

			End If

		Catch ex As Exception
			logger.Error(String.Format("{0}.Mandanten Jahr anlegen:{1}", strMethodeName, ex.ToString))

		End Try

		Try
			ChangeAppConfigFile()
			' Die Standardverzeichnisse anlegen...
			_ClsProgSetting.CreateSPSDirectories()
			'success = success AndAlso SendDataToSputnikServer()


		Catch ex As Exception
			logger.Error(String.Format("{0}.Verzeichnisse anlegen:{1}", strMethodeName, ex.ToString))

		End Try
		sLogTry = 0


		Return success

	End Function

	Private Function GetUSFiliale(ByVal iUSNr As Integer) As Boolean
		Dim result As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = "Select * From US_Filiale Where USNr = @USNr Order By Bezeichnung"
		Dim Conn As New SqlConnection(Me._ClsSetting.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Dim strUSFiliale As String = ""

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@USNr", iUSNr)
			Dim rFilialrec As SqlDataReader = cmd.ExecuteReader

			If Not rFilialrec.HasRows Then
				strUSFiliale = ""
			Else
				While rFilialrec.Read()
					strUSFiliale += IIf(strUSFiliale = "", "", ", ") & rFilialrec("Bezeichnung").ToString
				End While

			End If
			If IsUserActionAllowed(iUSNr, 672) Then
				strUSFiliale = String.Empty
			End If

			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, My.Resources.str447, strUSFiliale)
			rFilialrec.Close()

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			MsgBox(Err.Description, MsgBoxStyle.Critical, "GetUSFiliale")
			result = False

		Finally

		End Try


		Return result

	End Function

	Function IsUserActionAllowed(ByVal iUSNr As Integer, ByVal iFuncName As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iLogedUsnr As Integer = iUSNr
		Dim bResult As Boolean
		Dim sSql As String = "[Get User SecLevel For Selected Moduls]"
		Dim iMyFuncNr As Integer = iFuncName
		If iMyFuncNr = 0 Then
			logger.Warn(String.Format("{0}.Kein Modul wurde ausgewählt...", strMethodeName))
			Return False
		End If
		Dim ConnDbSelect As New SqlConnection(_ClsSetting.MDDbConn)

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			Dim rSecrec As SqlClient.SqlDataReader

			Try
				ConnDbSelect.Open()
				param = cmd.Parameters.AddWithValue("@LogedUSNr", iLogedUsnr)
				param = cmd.Parameters.AddWithValue("@FuncSecNr", iMyFuncNr)

				rSecrec = cmd.ExecuteReader          ' UserSecDatenbank
				rSecrec.Read()
				If rSecrec.HasRows Then
					If Not IsDBNull(rSecrec("IsAllowed")) Then
						bResult = CBool(rSecrec("IsAllowed"))
					Else
						bResult = False
					End If
				End If
				rSecrec.Close()

			Catch ex As Exception
				logger.Error(String.Format("{0}.SecDb lesen:{1}", strMethodeName, ex.Message))

			Finally

			End Try

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			ConnDbSelect.Close()

		End Try
		Return bResult

	End Function

	Private Sub frmLogin_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		frmSelectMD.grdMDList.Focus()
	End Sub

	Private Sub frmLogin_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		frmSelectMD.grdMDList.Enabled = True
	End Sub

	Private Sub frmLogin_KeyPress(ByVal eventSender As System.Object, _
																ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

		If KeyAscii = System.Windows.Forms.Keys.Escape Then
			KeyAscii = 0
			CmdXPCancel_Click(CmdXPCancel, New System.EventArgs())
		End If

		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then eventArgs.Handled = True

	End Sub

	Private Sub frmLogin_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

		frmSelectMD.grdMDList.Enabled = False

		Try
			Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
			Dim strStyleName As String = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(_ClsSetting.MDNr, 0), strQuery, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception

		End Try

		Me.Left = FormLeft ' frmSelectMD.Left + frmSelectMD.Width + 10
		Me.Top = FormTop ' frmSelectMD.Top

	End Sub

	Private Sub txtUserName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUserName.KeyPress

		Dim keyascii As Integer = AscW(e.KeyChar)
		Dim bNumber As Boolean = Char.IsNumber(e.KeyChar)
		Dim bLetter As Boolean = Char.IsLetter(e.KeyChar)

		If keyascii = System.Windows.Forms.Keys.Return Then
			System.Windows.Forms.SendKeys.Send("{Tab}")
			e.Handled = True
		ElseIf keyascii = System.Windows.Forms.Keys.Escape Then
			Me.Close()

		End If

	End Sub

	Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
		Dim keyascii As Integer = AscW(e.KeyChar)
		Dim bNumber As Boolean = Char.IsNumber(e.KeyChar)
		Dim bLetter As Boolean = Char.IsLetter(e.KeyChar)

		If keyascii = System.Windows.Forms.Keys.Return Then
			CmdXPOK_Click(Me.CmdXPOK, e)
			e.Handled = True

		ElseIf keyascii = System.Windows.Forms.Keys.Escape Then
			Me.Close()
		End If

	End Sub

	Private Sub TranslateControls()

		Me.mandantName.Text = String.Format("[{0}] | {1}", SelectedDbName, SelectedMDName)
		Me.Text = GetSafeTranslationValue(Me.Text)
		Me.lblHeader1.Text = GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.CmdXPOK.Text = GetSafeTranslationValue(Me.CmdXPOK.Text)
		Me.CmdXPCancel.Text = GetSafeTranslationValue(Me.CmdXPCancel.Text)
		Me.lblUser.Text = GetSafeTranslationValue(Me.lblUser.Text)
		Me.lblPassword.Text = GetSafeTranslationValue(Me.lblPassword.Text)

	End Sub

	Private Sub ChangeAppConfigFile()

		Dim serverfilename As String = Path.Combine(_ClsProgSetting.GetUpdatePath, "Binn\Net\SPS.MainView.exe.config")
		Dim localfilename As String = Path.Combine(My.Computer.FileSystem.CurrentDirectory, "SPS.MainView.exe.config")

		Try
			If Not File.Exists(serverfilename) Then Return
			Dim lines As String() = IO.File.ReadAllLines(serverfilename)
			For i As Integer = lines.Length - 2 To lines.Length - 1
				Dim line As String = lines(i)
				If line.Contains(vbNullChar) Then

					' attension: its casesensitiv!!!
					Dim url As String = "http://www.domain.com/sps.mainview.exe.config"
					If DownloadFile(url, serverfilename) Then
						Try
							If File.Exists(serverfilename) Then
								My.Computer.FileSystem.CopyFile(serverfilename, localfilename, True)
							End If
						Catch ex As Exception
							logger.Error(String.Format("local copy of configfile: {0}", ex.ToString))
						End Try

						logger.Debug("File ist replaced...")
						Return
					End If

				End If
			Next

		Catch ex As Exception
			logger.Error(String.Format("Update copy of configfile: {0}", ex.ToString))
		End Try


	End Sub

	Private Sub ChangeAppConfigFile_()

		Dim filename As String = _ClsProgSetting.GetUpdatePath & "Binn\Net\SPS.MainView.exe.config"

		Dim lines As String = IO.File.ReadAllText(filename)
		If lines.Contains(vbNullChar) Then
			Dim url As String = "http://www.domain.com/sps.mainview.exe.config"
			If DownloadFile(url, filename) Then
				logger.Debug("File ist replaced...")
				Return
			End If
		End If

	End Sub


	Private Function DownloadFile(ByVal _url As String, ByVal _filename As String) As Boolean
		Try
			Dim _webClient As New System.Net.WebClient
			_webClient.DownloadFile(_url, _filename)

			Return IO.File.Exists(_filename)

		Catch ex As Exception
			logger.Error(String.Format("File could not be downloaded! {0}", ex.ToString))

			Return False
		End Try

	End Function


	Private Function InsertLogToUSLog(ByVal strUserName As String, ByVal result As Boolean) As Boolean
		Dim success As Boolean = True

		Dim strConnString As String = _ClsSystem.GetConnString()
		Dim Conn As New SqlConnection(strConnString)
		Dim Sql As String

		Sql = "Insert Into [LOG] (USNr, UserFullName, UserName, Password, LogDate, Result) Values"
		Sql &= " (@USNr, @UserFullName, @UserName, @Password, getdate(), @Result)"

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@USNr", m_utility.ReplaceMissing(m_UserNumber, DBNull.Value))
			param = cmd.Parameters.AddWithValue("@UserFullName", String.Format("{0}\{1}", Environment.MachineName, Environment.UserName))
			param = cmd.Parameters.AddWithValue("@UserName", m_utility.ReplaceMissing(strUserName, DBNull.Value))
			param = cmd.Parameters.AddWithValue("@Password", m_utility.ReplaceMissing(m_UserPass, DBNull.Value))
			param = cmd.Parameters.AddWithValue("@Result", If(result, 1, 0))

			' hinzufügen...
			cmd.Connection = Conn
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			success = False
			logger.Error(ex.ToString)
			MsgBox(Err.Description, MsgBoxStyle.Critical, "InsertLogToUSLog")

		Finally
			Conn.Close()

		End Try


		Return success

	End Function



End Class