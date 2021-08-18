
Imports System.Xml
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO

Imports NLog
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports Microsoft.Win32
Imports System.Collections.Generic
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.Utils

Public Class frmSelectMD

	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"

	Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

	Private _ClsSetting As ClsLogingMDData

	Private _ClsReg As SPProgUtility.ClsDivReg
	Private _ClsSystem As ClsMain_Net
	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath

	Private m_InitProgFile As String
	Private Conn As SqlConnection
	Private m_ClsSetting As ClsMDData


#End Region


#Region "private properties"

	Private Property mandantRecNumber As Integer
	Private Property mandantNumber As Integer
	Private Property userNumber As Integer

#End Region

#Region "private consts"

	Private Const LOCAL_UPDATE_ARG As String = ""
	Private Const UPDATE_FILENAME As String = "SPSCLUpdate"
	Private Const DEFAULT_SPUTNIKADMIN_USERNAME As String = ";usernames;"

#End Region


#Region "Constructor"

	Public Sub New()

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		_ClsSetting = New ClsLogingMDData

		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath
		_ClsSystem = New ClsMain_Net
		_ClsReg = New SPProgUtility.ClsDivReg

		m_InitProgFile = _ClsSystem.GetInitIniFile()

		m_ClsSetting = New ClsMDData With {.MDDbConn = String.Empty, .MDNr = 0, .MDYear = Now.Year}
		ClsPublicData.TranslationData = ClsPublicData.Translation

		TranslateControls()

		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
		WindowsFormsSettings.AllowAutoFilterConditionChange = DefaultBoolean.False

		Reset()

		AddHandler Me.gvMDList.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvMDList.FocusedRowChanged, AddressOf Ongv_FocusedChanged

	End Sub

#End Region


	Private Sub Reset()

		ResetMDListDetailGrid()


	End Sub

	Private Sub frmSelectMD_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			My.Settings.frmHeight = Me.Height
			My.Settings.frmWidth = Me.Width
			My.Settings.Save()

			Try
				If Not Conn Is Nothing Then Conn.Close()
				If Not Conn Is Nothing Then Conn.Dispose()

			Catch ex As Exception
				logger.Error(String.Format("Close DBConn.{0}.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub frmSelectMD_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
		Me.grdMDList.Focus()
	End Sub

	Private Sub frmSelectMD_KeyPress(ByVal eventSender As System.Object,
																	 ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

		If KeyAscii = System.Windows.Forms.Keys.Escape Then
			Me.Close()

		ElseIf KeyAscii = System.Windows.Forms.Keys.Return Then
			GetSelectedMDData()

			Return

			'ElseIf KeyAscii = System.Windows.Forms.Keys.ControlKey + Keys.F Then
			'	Me.grdMDList.ForceInitialize()
			'	Me.gvMDList.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle
			'	Me.gvMDList.FocusedColumn = gvMDList.Columns("MDName")
			'	Me.gvMDList.ShowEditor()

		End If

		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then eventArgs.Handled = True

	End Sub


	Private Sub frmSelectMD_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

		Dim success As Boolean = True
		Dim Conn As SqlConnection
		Dim sSql As String = ""
		Dim sLSelectedMD As Short = 0
		Dim strMDSection As String = String.Format("{0}0000", My.Resources.str365)

		success = success AndAlso LoadFormSetting()
		success = success AndAlso LoadStartSettings()
		If Not success Then End

		success = success AndAlso ISLicencOK()

		lblIsUpdateOK.Visible = Not IsUpdateOK()


		Try
			Me._ClsSetting = New ClsLogingMDData With {.MDDbConn = String.Empty, .MDNr = 0, .MDYear = Now.Year}

			Conn = GetDatabase(strMDSection)

			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str259, My.Resources.str198, _ClsReg.GetINIString(_ClsSystem.GetInitIniFile(), strMDSection, "ConnStr"))
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str259, My.Resources.str198 & ".Net", Conn.ConnectionString)

			_ClsData.DbSelectConn = Conn.ConnectionString

			m_ClsSetting.RootDbServer = Conn.DataSource
			m_ClsSetting.RootDbName = Conn.Database
			m_ClsSetting.RootDbConn = Conn.ConnectionString
			sLSelectedMD = CShort(Val(_ClsReg.GetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str290)).ToString.Trim)

			LoadMDListDetailList(sLSelectedMD, Conn)

			Me.Visible = True
			Me.WindowState = FormWindowState.Normal

		Catch ex As Exception
			logger.Error(String.Format("{0}", ex.ToString))
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, GetSafeTranslationValue("Mandanten auflisten"),
																								 MessageBoxButtons.OK,
																								 MessageBoxIcon.Stop)

		End Try

		Me.grdMDList.Focus()

	End Sub


	Private Sub LvgMDData_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

		RunSelectedEntry()

	End Sub


	Private Sub TranslateControls()

		Try
			Me.Text = GetSafeTranslationValue(Me.Text)
			Me.lblHeader1.Text = GetSafeTranslationValue(Me.lblHeader1.Text)
			Me.lblHeader2.Text = GetSafeTranslationValue(Me.lblHeader2.Text)
			Me.lblMandanten.Text = GetSafeTranslationValue(Me.lblMandanten.Text)
			Me.CmdXPOK.Text = GetSafeTranslationValue(Me.CmdXPOK.Text)
			Me.CmdXPCancel.Text = GetSafeTranslationValue(Me.CmdXPCancel.Text)


		Catch ex As Exception
			logger.Error(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Function LoadFormSetting() As Boolean

		Dim result As Boolean = True
		Dim msgCurrency As String = String.Empty
		Dim msgDate As String = String.Empty
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strLangInfo As String = CultureInfo.CurrentUICulture.Name
		Dim strDecSep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
		Dim strDateSep As String = CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator

		result = result AndAlso String.Format("{0}{1}", strDecSep, strDateSep) = ".."

		Me.Width = Math.Max(My.Settings.frmWidth, Me.Width)
		Me.Height = Math.Max(My.Settings.frmHeight, Me.Height)

		If Not result Then
			Dim systemMessage As String

			If strDecSep <> "." Then
				msgCurrency = "Möglicherweise ist das Dezimaltrennzeichen in Ihrem System nicht auf 'Deutsch (Schweiz)' eingestellt({0}). "
				msgCurrency &= "Bitte ändern Sie in Systemsteurung -> Region -> Weitere Einstellungen -> 'Dezimaltrennzeichen' durch einem (.) Punkt."

				logger.Warn(String.Format("msgCurrency: {0}", msgCurrency))

				msgCurrency = String.Format(GetSafeTranslationValue(msgCurrency), strDecSep, vbNewLine)
			End If

			logger.Warn(String.Format("strDateSep: {0}", strDateSep))
			If strDateSep <> "." Then
				msgDate = "Das Datumsformat ist in Ihrem System nicht auf 'Deutsch (Schweiz)' eingestellt({0}). "
				msgDate &= "Bitte ändern Sie in Systemsteurung -> Region -> Weitere Einstellungen -> Datum -> 'Datumsformate' durch einem (.) Punkt."

				logger.Warn(String.Format("msgDate: {0}", msgDate))

				msgDate = String.Format(GetSafeTranslationValue(msgDate), strDateSep)

			End If

			systemMessage = String.Format(GetSafeTranslationValue("{1}{0}{2}{0}{0}Das Programm wird abgebrochen!"), vbNewLine, msgCurrency, msgDate)
			DevExpress.XtraEditors.XtraMessageBox.Show(systemMessage, GetSafeTranslationValue("Spracheinstellung"),
																								 MessageBoxButtons.OK,
																								 MessageBoxIcon.Stop)

			Return False
		End If

		ExistsCurrentYearData = True

		Select Case (strLangInfo).ToLower
			Case "fr-fr"
				strLangInfo = "F"

			Case "it-it"
				strLangInfo = "I"

			Case Else
				strLangInfo = ""

		End Select
		logger.Debug("starting regsetting!")

		_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "USLanguage", strLangInfo)
		Me.Left = 0
		Me.Top = 0
		logger.Debug("finish loadformsetting!")


		Return result

	End Function


	Sub RunSelectedEntry()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim autoLogin As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.AltKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim conn As SqlConnection
		Dim sSql As String
		Dim cmd As System.Data.SqlClient.SqlCommand
		Dim rMDrec As SqlDataReader
		Dim strMDSection As String = ""
		ExistsCurrentYearData = True

		If mandantRecNumber = 0 Then Return

		Dim userName As String = String.Format("{0}\{1}", Environment.UserDomainName, Environment.UserName).ToLower
		autoLogin = autoLogin AndAlso DEFAULT_SPUTNIKADMIN_USERNAME.Contains(String.Format(";{0};", userName))

		strMDSection = String.Format("MD_{0}", mandantNumber) 'SelectedMDNr)
		Dim iniValue As String = _ClsReg.GetINIString(SrvSettingFullFileName, strMDSection, "ConnStr")
		If String.IsNullOrWhiteSpace(iniValue) Then

			iniValue = _ClsReg.GetINIString(SrvSettingFullFileName, strMDSection, "ConnStr_Net")
			If String.IsNullOrWhiteSpace(iniValue) Then

				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(GetSafeTranslationValue("Sie dürfen sich nicht an die Datenank anmelden. Bitte kontaktieren Sie Ihren Systemadministrator.")),
														   GetSafeTranslationValue("Fehlende Datenbankverbindung"),
														   MessageBoxButtons.OK, MessageBoxIcon.Information)

				Me.Close()

				Return
			End If

			_ClsReg.SetINIString(SrvSettingFullFileName, strMDSection, "ConnStr", String.Format("Provider=SQLOLEDB.1;{0}", iniValue))
		End If

		conn = GetDatabase(strMDSection)
		If conn Is Nothing Then
			logger.Error(String.Format("{0}.GetDatabase", strMethodeName))
			CmdXPCancel_Click(CmdXPCancel, New System.EventArgs())

			Return
		End If
		Me.m_ClsSetting.MDDbConn = conn.ConnectionString
		Me.m_ClsSetting.MDDbName = conn.Database
		Me.m_ClsSetting.MDDbServer = conn.DataSource


		' Connectionstring wird in Registry gespeichert (Alte und Neue)
		_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str259, My.Resources.str288, _ClsReg.GetINIString(_ClsSystem.GetInitIniFile(), strMDSection, "ConnStr"))
		_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str259, My.Resources.str288 & ".Net", (conn.ConnectionString))
		_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str259, My.Resources.str288 & "._Net", _ClsProgSetting.EncryptString128Bit(conn.ConnectionString))

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
			logger.Debug(String.Format("{0}.{1}", strMethodeName, "Keine Mandantendaten wurden gefunden..."))
			DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(GetSafeTranslationValue("Für dieses Jahr konnte ich keine Mandantendaten {0} finden.{1}" &
						 "Bitte erstellen Sie in der Mandatenverwaltung einen neuen Mandanten."), SelMDCustomer_ID, vbNewLine), GetSafeTranslationValue("Keine Daten"),
				 MessageBoxButtons.OK, MessageBoxIcon.Information)

			Me.Close()
			Return

		Else
			Try
				_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str264, "MD_Customer_ID", rMDrec("Customer_ID").ToString)
				If rMDrec("Jahr").ToString <> Now.Year Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(GetSafeTranslationValue("Für das aktuelle Jahr existieren keine Daten.{0}" &
																																									 "Bitte erstellen Sie in der Mandantenverwaltung neue Daten fürs aktuelle Jahr."), vbNewLine),
																																							 GetSafeTranslationValue("Alte Daten"),
																																							 MessageBoxButtons.OK, MessageBoxIcon.Warning)
					ExistsCurrentYearData = False
				End If
				If Not autoLogin AndAlso Not String.IsNullOrWhiteSpace(rMDrec("Passwort4").ToString) Then
					_ClsData.MDPw4 = rMDrec("Passwort4").ToString

					Dim frmMDLogin As Form = New frmMDLogin(Me.m_ClsSetting)
					frmMDLogin.Show()

				Else
					Dim frmTest = New frmLogin(Me.m_ClsSetting)
					frmTest.FormLeft = Me.Left + Me.Width + 10
					frmTest.FormTop = Me.Top

					If autoLogin Then
						frmTest.DoAutoLogin = autoLogin
						Dim success = frmTest.DoLoging()

						If success Then
							frmTest = Nothing
							StartMainView()

							Me.Close()
							End
						End If

					End If
					frmTest.Show()
					frmTest.BringToFront()

				End If


			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.ToString()))
				DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString(), GetSafeTranslationValue("Mandantenauswahl"), MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

		End If

	End Sub

	Private Sub CmdXPOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXPOK.Click
		'If Val(Me.LblChanged.Text) = 0 Then Exit Sub
		GetSelectedMDData()
		'RunSelectedEntry()
	End Sub

	Private Sub CmdXPCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXPCancel.Click
		Me.Close()
	End Sub


#Region "loading mandant data"

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
			columnmodulname.Caption = GetSafeTranslationValue("RecNr")
			columnmodulname.Name = "RecNr"
			columnmodulname.FieldName = "RecNr"
			columnmodulname.Visible = False
			gvMDList.Columns.Add(columnmodulname)

			Dim columnmdnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmdnr.Caption = GetSafeTranslationValue("MDNr")
			columnmdnr.Name = "MDNr"
			columnmdnr.FieldName = "MDNr"
			columnmdnr.Visible = True
			columnmdnr.Width = 5
			gvMDList.Columns.Add(columnmdnr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = GetSafeTranslationValue("MDGroupNr")
			columnKDNr.Name = "MDGroupNr"
			columnKDNr.FieldName = "MDGroupNr"
			columnKDNr.Visible = False
			gvMDList.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = GetSafeTranslationValue("MDYear")
			columnZHDNr.Name = "MDYear"
			columnZHDNr.FieldName = "MDYear"
			columnZHDNr.Visible = False
			gvMDList.Columns.Add(columnZHDNr)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.Caption = GetSafeTranslationValue("MDName")
			columnMANr.Name = "MDName"
			columnMANr.FieldName = "MDName"
			columnMANr.Visible = True
			gvMDList.Columns.Add(columnMANr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = GetSafeTranslationValue("MDGuid")
			columnBezeichnung.Name = "MDGuid"
			columnBezeichnung.FieldName = "MDGuid"
			columnBezeichnung.Visible = False
			gvMDList.Columns.Add(columnBezeichnung)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = GetSafeTranslationValue("MDMainPath")
			columncustomername.Name = "MDMainPath"
			columncustomername.FieldName = "MDMainPath"
			columncustomername.Visible = False
			gvMDList.Columns.Add(columncustomername)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = GetSafeTranslationValue("MDDbServer")
			columnZFiliale.Name = "MDDbServer"
			columnZFiliale.FieldName = "MDDbServer"
			columnZFiliale.Visible = False
			gvMDList.Columns.Add(columnZFiliale)

			Dim columnMDDbConn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDDbConn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMDDbConn.Caption = GetSafeTranslationValue("MDDbConn")
			columnMDDbConn.Name = "MDDbConn"
			columnMDDbConn.FieldName = "MDDbConn"
			columnMDDbConn.Visible = False
			gvMDList.Columns.Add(columnMDDbConn)


		Catch ex As Exception
			logger.Error(ex.ToString)
		End Try

		grdMDList.DataSource = Nothing

	End Sub

	Private Function LoadMDListDetailList(ByVal sRecIndex As Short, ByVal Conn As SqlConnection) As Boolean
		Dim listOfMandant = LoadMDEntries(sRecIndex, Conn)

		If listOfMandant Is Nothing Then
			XtraMessageBox.Show(GetSafeTranslationValue("Fehler in der Mandanten-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt."))

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

			Me.lblMandantNameValue.Text = MandantGridData.ElementAt(index).MDName

		End If

		Return Not listOfMandant Is Nothing
	End Function


	Sub Ongv_FocusedChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)

		Dim dataRow = gvMDList.GetRow(e.FocusedRowHandle)
		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, ClsLogingMDData)

			Me.lblMandantNameValue.Text = viewData.MDName
		End If

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
		'm_Autologin = False

		If (e.Clicks = 2) Then

			GetSelectedMDData()

			'Dim autoLogin As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.AltKeyDown)
			'If autoLogin Then
			'	Dim oldPassword As String = ModulConstants.UserData.UserLoginPassword
			'	If oldPassword.Length > 4 Then
			'		txtPassword.Text = oldPassword.Substring(0, ModulConstants.UserData.UserLoginPassword.Length - 4)
			'		m_Autologin = True
			'		Dim result = ValidateLogin()

			'		If m_Autologin AndAlso result = LoginResult.loginok Then Me.ParentForm.Close()
			'	End If

			'Else


			'End If

		End If

	End Sub

	Sub GetSelectedMDData()
		Dim viewData = SelectedRecord ' CType(dataRow, ClsLogingMDData)

		If viewData Is Nothing Then Return

		If viewData.MDNr > 0 Then

			mandantRecNumber = viewData.RecNr
			mandantNumber = viewData.MDNr
			Me.lblMandantNameValue.Text = viewData.MDName
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
			_ClsData.MDName1 = Me.lblMandantNameValue.Text
			_ClsData.MDPath = SelMDYearPath
			_ClsData.MDGuid = SelMDCustomer_ID
			SelectedMDName = Me.lblMandantNameValue.Text
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



#Region "update"

	Private Function IsUpdateOK() As Boolean
		Dim result As Boolean = True

		Try
			Dim value = _ClsReg.GetRegKeyValue("HKLM\Software\Microsoft\Windows\CurrentVersion\Policies" & "\System", "EnableLUA")
			If String.IsNullOrWhiteSpace(value) Then
				result = False
			Else
				result = Not _ClsReg.GetRegKeyValue("HKLM\Software\Microsoft\Windows\CurrentVersion\Policies" & "\System", "EnableLUA")
			End If
			If Not Environment.UserName.ToUpper.Contains("Fardin".ToUpper) Then
				result = True
			Else
				result = CBool(_ClsReg.GetRegKeyValue(RegStr_Net & "\Options", "ShowUpdateOnStart"))
			End If
			logger.Info(String.Format("Domainname: {0}", Environment.UserDomainName.ToLower))
			If Environment.UserDomainName.ToLower = "zeda" OrElse Environment.UserDomainName.ToLower = "da" OrElse Environment.UserDomainName.ToLower = "work" Then
				Dim reg As Microsoft.Win32.RegistryKey
				reg = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System", True)
				Dim s = reg.GetValue("EnableLUA", "")
				If s = "1" Then result = False
			End If

		Catch ex As Exception
			logger.Warn(String.Format("Fehler beim Lesen von Policies. Das Updateprogramm wird nicht ausgeführt."))
			result = False

		End Try
		result = result AndAlso RunProgUpdate()


		Return result

	End Function

	Private Function RunProgUpdate() As Boolean
		Dim result As Boolean = True

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim _clsSystem As New ClsMain_Net

		Try

			SrvBinPath = _clsSystem.GetInitPath()

			SrvSettingFullFileName = _clsSystem.GetInitIniFile()
			logger.Debug("Update-Process is starting...")

			' zurzeit abschalten!
			Dim continueworking As Boolean = IsIPAdressAllowedForUpdate()
			If Not continueworking Then Return False

			' Updateprogramme: SPSCLUpdate.exe

			Dim localSputnikPath = _clsSystem.GetLocalInitPath()
			If localSputnikPath.EndsWith("\") Then localSputnikPath = Mid(localSputnikPath, 1, Len(localSputnikPath) - 1)

			Dim localSputnikUpdatePath As String = localSputnikPath
			Dim localSputnikParentPath = Directory.GetParent(localSputnikPath).FullName
			If Directory.Exists(Path.Combine(localSputnikParentPath, "Update")) Then
				localSputnikUpdatePath = Path.Combine(localSputnikParentPath, "Update")
			End If

			Dim localUpdatefilename As String = Path.Combine(localSputnikUpdatePath, UPDATE_FILENAME & ".exe")
			Dim remoteUpdatefilename As String = Path.Combine(_ClsProgSetting.GetUpdatePath, "Binn", "Net", UPDATE_FILENAME & ".exe")
			Dim localFileChangedDate As DateTime = File.GetLastWriteTime(localUpdatefilename)
			Dim remoteFileChangedDate As DateTime = File.GetLastWriteTime(remoteUpdatefilename)

			logger.Debug(String.Format("Localfile: {0}, Fileversion: {1} | remotefile: {2}, Fileversion: {3}", localUpdatefilename, localFileChangedDate, remoteUpdatefilename, remoteFileChangedDate))

			If File.Exists(remoteUpdatefilename) AndAlso localFileChangedDate <> remoteFileChangedDate Then
				logger.Debug(String.Format("{0}: {1} | {2}: {3}", localUpdatefilename, localFileChangedDate, remoteUpdatefilename, remoteFileChangedDate))

				Try
					FileCopy(remoteUpdatefilename, localUpdatefilename)

				Catch ex As UnauthorizedAccessException
					logger.Error(String.Format("Update exe-file could not be copied (UnauthorizedAccessException)!"))
					Return False
				Catch ex As IOException
					logger.Error(String.Format("Update exe-file could not be copied (IOException)!"))
					Return False
				Catch ex As Exception
					logger.Error(String.Format("Update exe-file could not be copied!"))
					Return False
				End Try

				Try
					Dim updateConfigFilename As String = UPDATE_FILENAME & ".exe.config"
					FileCopy(Path.Combine(_ClsProgSetting.GetUpdatePath, "Binn", "Net", updateConfigFilename), Path.Combine(localSputnikPath, updateConfigFilename))

				Catch ex As UnauthorizedAccessException
					logger.Error(String.Format("Update config-file could not be copied (UnauthorizedAccessException)!"))
					Return False
				Catch ex As IOException
					logger.Error(String.Format("Update config-file could not be copied (IOException)!"))
					Return False
				Catch ex As Exception
					logger.Error(String.Format("Update config-file could not be copied!"))
					Return False

				End Try

			End If

			' Update installieren
			Try
				If File.Exists(localUpdatefilename) Then
					Dim startInfo As New ProcessStartInfo(localUpdatefilename)
					startInfo.Arguments = If(LOCAL_UPDATE_ARG = String.Empty, "/AUTOSTART", LOCAL_UPDATE_ARG)
					Process.Start(startInfo)

					result = True
				End If

			Catch ex As UnauthorizedAccessException
				logger.Error(String.Format("Update can not start (UnauthorizedAccessException)!"))
				Return False

			Catch ex As Exception
				logger.Error(String.Format("Update can not start!"))
				Return False

			End Try

		Catch ex As Exception
			logger.Error(String.Format("{0}", ex.ToString))

			Return False
		End Try



		Return result

	End Function

	Private Function IsIPAdressAllowedForUpdate() As Boolean
		Dim success As Boolean = True
		Dim stationAdress As String = String.Empty
		Dim blacklist As New List(Of String)(New String() {"10.23.223.11", "10.23.223.12", "10.23.223.13", "10.23.223.14"})

		Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
		Dim adapter As NetworkInterface
		For Each adapter In adapters
			Dim properties As IPInterfaceProperties = adapter.GetIPProperties()
			If properties.UnicastAddresses.Count > 0 Then
				For Each unicastadress As UnicastIPAddressInformation In properties.UnicastAddresses
					Dim ip As IPAddress = unicastadress.Address
					If ip.AddressFamily = AddressFamily.InterNetwork Then
						stationAdress &= If(String.IsNullOrWhiteSpace(stationAdress), "", "|") & ip.ToString
					End If
				Next unicastadress
			End If
		Next adapter
		logger.Debug(String.Format("{0}", stationAdress))

		For Each itm As String In blacklist
			If stationAdress.Contains(itm) Then
				logger.Warn(String.Format("{0} not allowed...", itm))
				success = False
				Exit For
			End If
		Next

		Return success

	End Function

#End Region


End Class





Public Class ClsLogingMDData

	Public Property RootDbServer As String
	Public Property RootDbName As String
	Public Property RootDbConn As String

	Public Property RecNr As Integer
	Public Property MDNr As Integer
	Public Property MDGroupNr As Integer
	Public Property MDYear As Integer

	Public Property MDName As String
	Public Property MDGuid As String

	Public Property MDMainPath As String
	Public Property MDDbServer As String
	Public Property MDDbName As String
	Public Property MDDbConn As String

	Public ReadOnly Property SQLDateFormat As String
		Get
			Dim _ClsReg As New ClsDivReg

			Dim strFormat As String = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "DBServer")
			If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"

			Return strFormat
		End Get
	End Property

End Class
