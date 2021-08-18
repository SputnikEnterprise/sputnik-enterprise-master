
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.Infrastructure.UI.UtilityUI
Imports SP.Infrastructure.Settings

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports System.Data.SqlClient

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPS.MainView.EmployeeSettings

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports System.Threading.Tasks
Imports System.Threading

Public Class MDList

	Private _ClsSetting As ClsLogingMDData

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UitilityUI As UtilityUI
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private sLogTry As Integer = 0
	Private m_Autologin As Boolean

	Private Property AskPassword As Boolean
	Private Property mandantRecNumber As Integer
	Private Property mandantNumber As Integer
	Private Property userNumber As Integer


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		sLogTry = 0

		_ClsSetting = New ClsLogingMDData

		m_md = New Mandant
		m_utility = New Utilities
		m_UitilityUI = New UtilityUI
		m_common = New CommonSetting
		m_path = New ClsProgPath
		m_translate = New TranslateValues

		grpUserLoging.Visible = False
		grpMDLogin.Visible = False

		ResetMDListDetailGrid()

		AddHandler Me.gvMDList.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvMDList.FocusedRowChanged, AddressOf Ongv_FocusedChanged

	End Sub


#End Region



#Region "MDGridFunctions"

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As ClsLogingMDData
		Get
			Dim gvRP = TryCast(grdMDList.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim mddata = CType(gvRP.GetRow(selectedRows(0)), ClsLogingMDData)
					Return mddata
				End If

			End If

			Return Nothing
		End Get

	End Property

	Sub ResetMDListDetailGrid()

		gvMDList.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMDList.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMDList.OptionsView.ShowGroupPanel = False
		gvMDList.OptionsView.ShowIndicator = False
		gvMDList.OptionsView.ShowAutoFilterRow = True

		gvMDList.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("RecNr")
			columnmodulname.Name = "RecNr"
			columnmodulname.FieldName = "RecNr"
			columnmodulname.Visible = False
			gvMDList.Columns.Add(columnmodulname)

			Dim columnmdnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmdnr.Caption = m_translate.GetSafeTranslationValue("MDNr")
			columnmdnr.Name = "MDNr"
			columnmdnr.FieldName = "MDNr"
			columnmdnr.Visible = True
			columnmdnr.Width = 5
			gvMDList.Columns.Add(columnmdnr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("MDGroupNr")
			columnKDNr.Name = "MDGroupNr"
			columnKDNr.FieldName = "MDGroupNr"
			columnKDNr.Visible = False
			gvMDList.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_translate.GetSafeTranslationValue("MDYear")
			columnZHDNr.Name = "MDYear"
			columnZHDNr.FieldName = "MDYear"
			columnZHDNr.Visible = False
			gvMDList.Columns.Add(columnZHDNr)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.Caption = m_translate.GetSafeTranslationValue("MDName")
			columnMANr.Name = "MDName"
			columnMANr.FieldName = "MDName"
			columnMANr.Visible = True
			gvMDList.Columns.Add(columnMANr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("MDGuid")
			columnBezeichnung.Name = "MDGuid"
			columnBezeichnung.FieldName = "MDGuid"
			columnBezeichnung.Visible = False
			gvMDList.Columns.Add(columnBezeichnung)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_translate.GetSafeTranslationValue("MDMainPath")
			columncustomername.Name = "MDMainPath"
			columncustomername.FieldName = "MDMainPath"
			columncustomername.Visible = False
			gvMDList.Columns.Add(columncustomername)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("MDDbServer")
			columnZFiliale.Name = "MDDbServer"
			columnZFiliale.FieldName = "MDDbServer"
			columnZFiliale.Visible = False
			gvMDList.Columns.Add(columnZFiliale)

			Dim columnMDDbConn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDDbConn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMDDbConn.Caption = m_translate.GetSafeTranslationValue("MDDbConn")
			columnMDDbConn.Name = "MDDbConn"
			columnMDDbConn.FieldName = "MDDbConn"
			columnMDDbConn.Visible = False
			gvMDList.Columns.Add(columnMDDbConn)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMDList.DataSource = Nothing

	End Sub

	Public Function LoadMDListDetailList(ByVal sRecIndex As Short, ByVal Conn As SqlConnection) As Boolean
		Dim listOfMandant = LoadMDEntries(sRecIndex, Conn)

		If listOfMandant Is Nothing Then
			m_UitilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Fehler in der Mandanten-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt."))
			Return False
		End If

		Dim MandantGridData = (From person In listOfMandant
		Select New ClsLogingMDData With
					 {.MDNr = person.MDNr,
						.RootDbServer = person.RootDbServer,
						.RootDbName = person.RootDbName,
						.RootDbConn = person.RootDbConn,
						.RecNr = person.RecNr,
						.MDGroupNr = person.MDGroupNr,
						.MDYear = person.MDYear,
						.MDName = person.MDName,
						.MDGuid = person.MDGuid,
						.MDMainPath = person.MDMainPath,
						.MDDbServer = person.MDDbServer,
						.MDDbName = person.MDDbName,
						.MDDbConn = person.MDDbConn
					 }).ToList()

		Dim listDataSource As BindingList(Of ClsLogingMDData) = New BindingList(Of ClsLogingMDData)

		For Each p In MandantGridData
			listDataSource.Add(p)
		Next

		grdMDList.DataSource = listDataSource

		If listDataSource.Count > 0 Then
			Dim index = MandantGridData.ToList().FindIndex(Function(data) data.RecNr = sRecIndex)

			index = Math.Max(index, 0)
			Dim rowHandle = gvMDList.GetRowHandle(index)
			gvMDList.FocusedRowHandle = rowHandle

			Me.LblChanged_2.Text = MandantGridData.ElementAt(index).MDName

		End If

		Return Not listOfMandant Is Nothing
	End Function


	Sub Ongv_FocusedChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)

		Dim dataRow = gvMDList.GetRow(e.FocusedRowHandle)
		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, ClsLogingMDData)

			Me.LblChanged_2.Text = viewData.MDName
		End If

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
		m_Autologin = False

		If (e.Clicks = 2) Then

			GetSelectedMDData()
			Dim autoLogin As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.AltKeyDown)
			If autoLogin Then
				Dim oldPassword As String = ModulConstants.UserData.UserLoginPassword
				If oldPassword.Length > 4 Then
					txtPassword.Text = oldPassword.Substring(0, ModulConstants.UserData.UserLoginPassword.Length - 4)
					m_Autologin = True
					Dim result = ValidateLogin()

					If m_Autologin AndAlso result = LoginResult.loginok Then Me.ParentForm.Close()
				End If

			Else


			End If

		End If

	End Sub

	Sub GetSelectedMDData()
		Dim viewData = SelectedRecord	' CType(dataRow, ClsLogingMDData)

		If viewData Is Nothing Then Exit Sub

		If viewData.MDNr > 0 Then

			mandantRecNumber = viewData.RecNr
			mandantNumber = viewData.MDNr
			Me.LblChanged_2.Text = viewData.MDName
			SelMDMainPath = viewData.MDMainPath
			SelMDCustomer_ID = viewData.MDGuid
			SelectedMDGroupNr = viewData.MDGroupNr
			SelectedFileServerPath = viewData.MDDbServer

			SelMDYearPath = String.Format("{0}{1}\", SelMDMainPath, (Year(Now)))
			If Not Directory.Exists(SelMDYearPath) Then
				SelMDYearPath = String.Format("{0}{1}\", SelMDMainPath, (Year(Now) - 1))
				If Not Directory.Exists(SelMDYearPath) Then SelMDYearPath = String.Empty
			End If

			_ClsData.MDID = CInt(mandantRecNumber)
			_ClsData.MDNr = CInt(mandantNumber)
			_ClsData.MDName1 = Me.LblChanged_2.Text
			_ClsData.MDPath = SelMDYearPath
			_ClsData.MDGuid = SelMDCustomer_ID
			SelectedMDName = Me.LblChanged_2.Text
			'SelectedMDNr = CInt(mandantNumber)

			_ClsData.SelectMDGroupNr = SelectedMDGroupNr
			_ClsData.SelectedFileServerPath = SelectedFileServerPath

			Me._ClsSetting.MDNr = mandantNumber
			Me._ClsSetting.MDName = SelectedMDName
			Me._ClsSetting.MDMainPath = SelMDMainPath
			Me._ClsSetting.MDGuid = SelMDCustomer_ID

			RunSelectedEntry()

		End If

	End Sub


#End Region


	Public Function LoadMDList() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim result As Boolean = False

		Try
			Me.Text = m_translate.GetSafeTranslationValue(Me.Text)
			Me.lblMandanten.Text = m_translate.GetSafeTranslationValue(Me.lblMandanten.Text)

			grpUserLoging.Text = m_translate.GetSafeTranslationValue(grpUserLoging.Text)
			Me.lblHeaderNormal.Text = m_translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
			Me.CmdLoginCancel.Text = m_translate.GetSafeTranslationValue(Me.CmdLoginCancel.Text)
			Me.lblUser.Text = m_translate.GetSafeTranslationValue(Me.lblUser.Text)
			Me.lblPassword.Text = m_translate.GetSafeTranslationValue(Me.lblPassword.Text)

			grpMDLogin.Text = m_translate.GetSafeTranslationValue(grpMDLogin.Text)
			Me.lblMDHeader1.Text = m_translate.GetSafeTranslationValue(Me.lblMDHeader1.Text)
			Me.lblmdpasswort.Text = m_translate.GetSafeTranslationValue(Me.lblmdpasswort.Text)
			Me.cmdMDLogin.Text = m_translate.GetSafeTranslationValue(Me.cmdMDLogin.Text)
			Me.cmdMDLoginCancel.Text = m_translate.GetSafeTranslationValue(Me.cmdMDLoginCancel.Text)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Translation: {1}", strMethodeName, ex.Message))

		End Try

        LoadMDListEntry()

		grdMDList.Focus()

		Return True

	End Function

	Sub LoadMDListEntry()

		Dim Conn As SqlConnection
		Dim sSql As String = ""
		Dim sLSelectedMD As Short = 0
		Dim strMDSection As String = String.Format("{0}0000", "Mandant")

		Try
			Me._ClsSetting = New ClsLogingMDData With {.MDDbConn = String.Empty, .MDNr = 0, .MDYear = Now.Year}
			Me.Left = 0
			Me.Top = 0

			Try
				Conn = GetDatabase(strMDSection)
				' Connectionstring wird in Registry gespeichert...
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "RootConnStr", _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile(), strMDSection, "ConnStr"))
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "RootConnStr" & ".Net", Conn.ConnectionString)

				_ClsData.DbSelectConn = Conn.ConnectionString

				Me._ClsSetting.RootDbServer = Conn.DataSource
				Me._ClsSetting.RootDbName = Conn.Database
				Me._ClsSetting.RootDbConn = Conn.ConnectionString
				sLSelectedMD = CShort(Val(_ClsReg.GetRegKeyValue(RegStr_Net & "\Path", "LastSelectedMDIndex")).ToString.Trim)

				Try
					LoadMDListDetailList(sLSelectedMD, Conn)

				Catch ex As Exception
					m_Logger.LogError(String.Format("FillMDLvg: {0}", ex.Message))
					m_UitilityUI.ShowErrorDialog(ex.Message)

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("GetDatabase: {0}", ex.Message))
				m_UitilityUI.ShowErrorDialog(ex.Message)
			End Try

			'Me.LvgMDData.Focus()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Allgemeiner Fehler: {0}", ex.Message))
			m_UitilityUI.ShowErrorDialog(ex.Message & vbCrLf & ex.GetBaseException.ToString)

		End Try

	End Sub

	Sub RunSelectedEntry()
		Dim conn As SqlConnection
		Dim sSql As String
		Dim cmd As System.Data.SqlClient.SqlCommand
		Dim rMDrec As SqlDataReader
		Dim strMDSection As String = ""
		ExistsCurrentYearData = True
		'Dim bIsOldMDName As Boolean

		If mandantRecNumber = 0 Then Return

		grpUserLoging.Visible = False
		grpMDLogin.Visible = False

		strMDSection = String.Format("MD_{0}", mandantNumber)
		Dim iniValue As String = _ClsReg.GetINIString(SrvSettingFullFileName, strMDSection, "ConnStr")
		If String.IsNullOrWhiteSpace(iniValue) Then

			iniValue = _ClsReg.GetINIString(SrvSettingFullFileName, strMDSection, "ConnStr_Net")
			If String.IsNullOrWhiteSpace(iniValue) Then
				m_UitilityUI.ShowOKDialog(Me, m_translate.GetSafeTranslationValue("Sie dürfen sich nicht an die Datenank anmelden. Bitte kontaktieren Sie Ihren Systemadministrator."),
										  m_translate.GetSafeTranslationValue("Fehlende Datenbankverbindung"), MessageBoxIcon.Error)

				Return
			End If

			_ClsReg.SetINIString(SrvSettingFullFileName, strMDSection, "ConnStr", String.Format("Provider=SQLOLEDB.1;{0}", iniValue))
		End If

		conn = GetDatabase(strMDSection)
		If conn Is Nothing Then
			m_Logger.LogError(String.Format("GetDatabase: conn is nothing"))

			Return
		End If
		Me._ClsSetting.MDDbConn = conn.ConnectionString
		Me._ClsSetting.MDDbName = conn.Database
		Me._ClsSetting.MDDbServer = conn.DataSource

		' Connectionstring wird in Registry gespeichert (Alte und Neue)
		_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "Connection String", _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile(), strMDSection, "ConnStr"))
		_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "Connection String" & ".Net", (conn.ConnectionString))
		_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "Connection String" & "._Net", EncryptBase64String(conn.ConnectionString))

		sSql = "Update Mandanten Set MDFullFileName = @MDFullFileName, MDNr = @MDNr "
		sSql &= "Where Customer_ID = @Customer_ID "
		sSql &= "Select Top 1 MDNr, Passwort4, Jahr, Customer_ID From Mandanten "
		sSql &= "Where (Jahr = @ThisYear Or Jahr = @ThisYear - 1) And Customer_ID = @Customer_ID Order By Jahr Desc"

		cmd = New System.Data.SqlClient.SqlCommand(sSql, conn)
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@Customer_ID", SelMDCustomer_ID)
		param = cmd.Parameters.AddWithValue("@MDFullFileName", SelMDMainPath)
		param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
		param = cmd.Parameters.AddWithValue("@ThisYear", Now.Year)
		rMDrec = cmd.ExecuteReader
		rMDrec.Read()
		SelectedDbName = cmd.Connection.Database

		If Not rMDrec.HasRows Then
			DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_translate.GetSafeTranslationValue("Für dieses Jahr konnte ich keine Mandantendaten finden.{0}" &
						 "Bitte erstellen Sie in der Mandatenverwaltung einen neuen Mandanten."), vbNewLine), m_translate.GetSafeTranslationValue("Keine Daten"),
				 MessageBoxButtons.OK, MessageBoxIcon.Information)

			Exit Sub

		Else
			Try
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "MD_Customer_ID", rMDrec("Customer_ID").ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "LastSelectedMDIndex", mandantRecNumber)
				If rMDrec("Jahr").ToString <> Now.Year Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_translate.GetSafeTranslationValue("Für das aktuelle Jahr existieren keine Daten.{0}" &
																																									 "Bitte erstellen Sie in der Mandantenverwaltung neue Daten fürs aktuelle Jahr."), vbNewLine),
																																							 m_translate.GetSafeTranslationValue("Alte Daten"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
					ExistsCurrentYearData = False
				End If
				If ModulConstants.UserData.UserNr <> 1 AndAlso Not String.IsNullOrWhiteSpace(rMDrec("Passwort4").ToString) Then
					_ClsData.MDPw4 = rMDrec("Passwort4").ToString

					grpMDLogin.Visible = True
					txtUserName.Focus()

				Else
					Me.lblMDName.Text = String.Format("[{0}] | {1}", SelectedDbName, SelectedMDName)

					grpUserLoging.Visible = True
					txtUserName.Text = ModulConstants.UserData.UserLoginname
					txtPassword.Focus()

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))
				DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString(),
																									 m_translate.GetSafeTranslationValue("Mandantenauswahl"),
																									 MessageBoxButtons.OK,
																									 MessageBoxIcon.Error)

			End Try

		End If

	End Sub

	Private Sub ShowUserLoginForm()

		If mandantRecNumber = 0 Then Exit Sub

		txtUserName.Text = String.Empty
		txtPassword.Text = String.Empty

		txtMDLogin.Text = String.Empty

		RunSelectedEntry()

	End Sub

	Function SaveAndCheckForlogin() As LoginResult
		Dim result As New LoginResult
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim sql As String
		sql = "Select * From Benutzer Where [US_Name] = @strEnteredName And ISNULL(Deaktiviert, 0) = 0 And ISNULL(AsCostCenter, 0) = 0 "
		sql &= "AND (AktivUntil IS NULL OR convert(DATE, AktivUntil, 104) >= CONVERT(DATE, GETDATE(), 104))"

		Dim strEnteredName As String = String.Empty
		Dim strEnteredPass As String = String.Empty
		Dim strUSeMail As String = String.Empty
		Dim strResult As String = String.Empty
		Dim strUSLang As String = String.Empty

		Dim Conn As New SqlConnection(Me._ClsSetting.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		DevComponents.DotNetBar.ToastNotification.Close(Me)
		DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor = DevComponents.DotNetBar.eToastGlowColor.Red
		DevComponents.DotNetBar.ToastNotification.ToastFont = New System.Drawing.Font("tahoma", 8.25F, System.Drawing.FontStyle.Bold)
		DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval = 2000

		If Me.txtPassword.Text = String.Empty Or Me.txtUserName.Text = String.Empty Then
			strMessage = "Leere Anmeldung ist nicht erlaubt."
			DevComponents.DotNetBar.ToastNotification.Show(Me, _
																										 m_translate.GetSafeTranslationValue(strMessage), _
																										 Nothing, DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval, _
																										 DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor,
																										 DevComponents.DotNetBar.eToastPosition.BottomCenter)

			Return LoginResult.loginfailed
		End If
		strEnteredName = EncryptMyString(UCase(Me.txtUserName.Text), strEncryptionKey)
		strEnteredPass = EncryptMyString(Me.txtPassword.Text & strExtraPass, strEncryptionKey)

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@strEnteredName", strEnteredName)
			Dim rUSrec As SqlDataReader = cmd.ExecuteReader
			rUSrec.Read()

			sLogTry += 1
			If sLogTry = 4 Then
				strMessage = String.Format(m_translate.GetSafeTranslationValue("Sie hatten {0} nicht erfolgreiche Anmeldungen gehabt.{1}Das Programm wird beendet."), CStr(sLogTry), vbNewLine)
				m_UitilityUI.ShowErrorDialog(strMessage)

				Return LoginResult.tryfailed
			End If

			If rUSrec.HasRows() Then

				If strEnteredPass <> rUSrec("PW") Then
					strMessage = m_translate.GetSafeTranslationValue("Benutzername oder Kennwort ungültig.")
					DevComponents.DotNetBar.ToastNotification.Show(grpUserLoging, _
																								 m_translate.GetSafeTranslationValue(strMessage), _
																								 Nothing, DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval, _
																								 DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor, _
																								 DevComponents.DotNetBar.eToastPosition.BottomCenter)
					System.Windows.Forms.SendKeys.Send("{Home}+{End}")

					InsertLogToUSLog(Me.txtUserName.Text, 0)
					rUSrec.Close()

					Return LoginResult.loginfailed
				End If

				If Not IsDBNull(rUSrec("eMail")) Then strUSeMail = rUSrec("eMail")

			Else
				strMessage = m_translate.GetSafeTranslationValue("Benutzername oder Kennwort ungültig.")
				DevComponents.DotNetBar.ToastNotification.Show(grpUserLoging, _
																		 m_translate.GetSafeTranslationValue(strMessage), _
																		 Nothing, DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval, _
																		 DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor, _
																		 DevComponents.DotNetBar.eToastPosition.BottomCenter)
				rUSrec.Close()

				Return LoginResult.loginfailed

			End If

			mandantNumber = CInt(rUSrec(("MDNr")).ToString)
			userNumber = CInt(rUSrec("USNr"))
			Call GetUSFiliale(userNumber, mandantNumber)

			With rUSrec


				Try
					ClsPublicData.MDData = ClsPublicData.SelectedMDData(mandantNumber)
					SelMDMainPath = ClsPublicData.MDData.MDMainPath
					SelMDYearPath = String.Format("{0}{1}\", SelMDMainPath, (Year(Now)))

				Catch ex As Exception
					m_Logger.LogError(String.Format("Mandantdaten konnten nicht erneut geladen werden!!! {0}", SelectedMDName))

				End Try

				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "MDPath", SelMDYearPath)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "SelMDMainPath", SelMDMainPath)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "MDYear", CStr(Year(Today)))


				' Mandantennummer
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "MDNr", mandantNumber)

				' Mandantenname
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "MDName", SelectedMDName)

				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "UserNr", CStr(rUSrec("USNr")))
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USSecLevel", CStr(rUSrec("SecLevel")))
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USKst2", rUSrec(("USKst2")).ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USKst1", rUSrec(("USKst1")).ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USKst", rUSrec(("Kst")).ToString)

				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USVorname", rUSrec(("Vorname")).ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USNachname", rUSrec(("Nachname")).ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "MyGuid", rUSrec(("Transfered_Guid")).ToString)

				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USMainMDNr", rUSrec(("MDNr")).ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "SelectedMDGroupNr", SelectedMDGroupNr.ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "SelectedFileServerPath", SelectedFileServerPath.ToString)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "SelectedDBName", String.Format("[{0}]", SelectedDbName.ToString))

				If Not IsDBNull(rUSrec("USLanguage")) Then
					strUSLang = Mid(rUSrec("USLanguage").ToString.ToUpper, 1, 1)
					If strUSLang.ToUpper.StartsWith("F") Or strUSLang.ToUpper.StartsWith("I") Then
						strUSLang = strUSLang
					Else
						strUSLang = String.Empty
					End If
				End If
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USLanguage", strUSLang)

				Dim aIP As Array = GetIP().ToArray
				Dim strMyIP As String = String.Empty
				For i As Integer = 0 To aIP.Length - 1
					strMyIP = IIf(aIP(i).ToString.Length < 16, aIP(i).ToString, "")
				Next

				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "LocalIPAddress", strMyIP)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "LocalHostName", GetIPHostName())

				_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "spUsereMail", strUSeMail)

				Dim CheckFile As New FileInfo(My.Application.Info.DirectoryPath)
				_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "ProgUpperPath", AddDirSep(CheckFile.Directory.ToString))

				Try
					InsertLogToUSLog(Me.txtUserName.Text, 1)

				Catch ex As Exception

				End Try
			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			strMessage = String.Format("{1}:{0}{2}", vbNewLine, m_translate.GetSafeTranslationValue("Fehler bei der Anmeldung."), ex.ToString())
			DevExpress.XtraEditors.XtraMessageBox.Show(strMessage, m_translate.GetSafeTranslationValue("Anmeldung"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

			Return LoginResult.loginfailed

		End Try


		Try
			' Die Standardverzeichnisse anlegen...
			Dim obj As New SPProgUtility.ClsProgSettingPath
			obj.CreateSPSDirectories()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Verzeichnisse anlegen:{1}", strMethodeName, ex.Message))

		End Try
		sLogTry = 0

		Return LoginResult.loginok

	End Function

	Sub GetUSFiliale(ByVal iUSNr As Integer, ByVal iMDNr As Integer)

		Dim strUSFiliale As String = ""
		Try
			If IsUserActionAllowed(iUSNr, 672, iMDNr) Then
				strUSFiliale = String.Empty

			Else

				Dim sql As String
				sql = "Select Top 1 IsNull(Bezeichnung, '') As Bezeichnung From US_Filiale Where USNr = @USNr Order By Bezeichnung"
				Dim Conn As New SqlConnection(Me._ClsSetting.MDDbConn)
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				Conn.Open()

				param = cmd.Parameters.AddWithValue("@USNr", iUSNr)
				Dim rFilialrec As SqlDataReader = cmd.ExecuteReader

				If Not rFilialrec.HasRows Then
					strUSFiliale = ""
				Else
					While rFilialrec.Read()
						strUSFiliale = rFilialrec("Bezeichnung")
					End While

				End If
				rFilialrec.Close()

			End If

			_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USFiliale", strUSFiliale)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UitilityUI.ShowErrorDialog(ex.ToString)

		Finally

		End Try

	End Sub

	Private Sub InsertLogToUSLog(ByVal strUserName As String, ByVal strResult As String)
		Dim strUsName As String = Environment.UserName
		Dim machineName As String = Environment.MachineName
		Dim Conn As New SqlConnection(Me._ClsSetting.MDDbConn)
		Dim sSql As String = "Insert Into [LOG] (UserFullName, UserName, LogDate, Result) Values (@UserFullName, @UserName, @LogDate, @Result)"

		m_UitilityUI = New UtilityUI

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@UserFullName", machineName & " " & strUsName)
			param = cmd.Parameters.AddWithValue("@UserName", strUserName)
			param = cmd.Parameters.AddWithValue("@LogDate", Date.Now)
			param = cmd.Parameters.AddWithValue("@Result", strResult)

			' hinzufügen...
			cmd.Connection = Conn
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
			m_UitilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()

		End Try

	End Sub





	Private Sub CheckMDLogin()
		Dim strEnteredName As String = ""
		Dim strEnteredPass As String = ""

		If Me.txtPassword.Text = "" Then grpMDLogin.Visible = False : Exit Sub

		strEnteredPass = EncryptMyString(Me.txtPassword.Text & strExtraPass, strEncryptionKey)
		If strEnteredPass <> _ClsData.MDPw4 Then
			m_UitilityUI.ShowErrorDialog(String.Format(m_translate.GetSafeTranslationValue("Geben Sie das Kennwort erneut ein.{0}Beachten Sie die Gross-/Kleinschreibung."), vbNewLine))

			Me.txtMDLogin.Text = ""
			Me.txtMDLogin.Focus()

		Else
			grpMDLogin.Hide()

			grpUserLoging.Visible = True
			txtPassword.Focus()

		End If

	End Sub


	Private Sub OnCmdLoginClick(sender As Object, e As EventArgs) Handles CmdLogin.Click
		m_Autologin = False
		Dim result = ValidateLogin()
	End Sub

	Private Function ValidateLogin() As LoginResult
		Dim result = SaveAndCheckForlogin()

		If result = LoginResult.loginok Then
			ModulConstants.MDData = ModulConstants.SelectedMDData(mandantNumber)
			If ModulConstants.MDData Is Nothing Then
				m_UitilityUI.ShowInfoDialog("Die Mandantendaten konnten nicht geladen werden! Das Programm muss beendet werden.", "Fehlerhafte Mandantendaten", MessageBoxIcon.Error)

				Return LoginResult.loginfailed
			End If
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, userNumber)
			ModulConstants.SecLevelData = ModulConstants.LogedUSSecData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			'CreateXMLFile4LogedUser()
			'CreateXMLFile4SelectedMandant()


			If Not m_Autologin Then Me.ParentForm.Close()

		ElseIf result = LoginResult.tryfailed Then
			txtUserName.Text = String.Empty
			txtPassword.Text = String.Empty

			grpUserLoging.Visible = False

		ElseIf m_Autologin AndAlso result = LoginResult.loginfailed Then
			txtPassword.Text = String.Empty


		End If


		Return result

	End Function

	Private Sub CreateXMLFile4LogedUser()

		Try
			'm_utility.CreateUserProfileXMLFile(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserNr, ModulConstants.MDData.MDNr)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub CreateXMLFile4SelectedMandant()

		Try
			'm_utility.CreateXMLFile4Mandant(_ClsProgSetting.GetMDData_XMLFile, ModulConstants.MDData.MDNr)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub TerminateSelectedProcess(ByVal strProgFullname As String)

		Try
			Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName(strProgFullname)
			For Each p As Process In pProcess
				p.Kill()
			Next

		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnCmdMDLoginClick(sender As Object, e As EventArgs) Handles cmdMDLogin.Click

		CheckMDLogin()

	End Sub

	Private Sub OnCmdXPCancelClick(sender As Object, e As EventArgs) Handles CmdLoginCancel.Click

		grpUserLoging.Visible = False

	End Sub

	Private Sub OnCmdMDLoginCancelClick(sender As Object, e As EventArgs) Handles cmdMDLoginCancel.Click

		grpMDLogin.Visible = False

	End Sub

	Private Sub gvMDList_KeyDown(sender As Object, e As KeyEventArgs) Handles gvMDList.KeyDown

		If e.Control And e.KeyCode = Keys.S Then
			Trace.WriteLine("Control and S-Taste wurde gedruckt...")
			If e.KeyCode = Keys.K Then
				Trace.WriteLine("K-Taste wurde gedruckt...")
			End If
		End If

	End Sub

	Private Sub gvMDList_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gvMDList.KeyPress

		If e.KeyChar = ChrW(Keys.Return) Then
			GetSelectedMDData()
		End If

	End Sub

	Private Sub OntxtUserName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUserName.KeyPress

		If e.KeyChar = ChrW(Keys.Return) Then
			txtPassword.Focus()
		End If

	End Sub

	Private Sub OntxtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress

		If e.KeyChar = ChrW(Keys.Return) Then
			ValidateLogin()
		End If

	End Sub

	Private Sub OntxtMDLogin_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMDLogin.KeyPress

		If e.KeyChar = ChrW(Keys.Return) Then
			CheckMDLogin()
		End If

	End Sub



	Private Function EncryptBase64String(ByVal myString As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

End Class


