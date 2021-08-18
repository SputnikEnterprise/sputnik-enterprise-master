

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports DevExpress.XtraSplashScreen

Imports System.Drawing
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class frmPControl

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private _PControlSetting As New ClsParifondSetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private m_xml As New ClsXML
	Private m_md As Mandant
	Private m_UtilityUI As UtilityUI

	Private Property ESListeDownloaded As Boolean
	Private Property KandidatDownloaded As Boolean
	Private Property KundeDownloaded As Boolean
	Private Property ESVErtragDownloaded As Boolean
	Private Property VerleihDownloaded As Boolean
	Private Property RPContentDownloaded As Boolean
	Private Property LODownloaded As Boolean
	Private Property LohnkontiDownloaded As Boolean

	Private Property ESListeIsWorking As Boolean
	Private Property KandidatIsWorking As Boolean
	Private Property KundeIsWorking As Boolean
	Private Property ESVErtragIsWorking As Boolean
	Private Property VerleihIsWorking As Boolean
	Private Property RPContentIsWorking As Boolean
	Private Property LOIsWorking As Boolean
	Private Property LohnkontiIsWorking As Boolean

	Private WithEvents mGlobalWorker As BackgroundWorker

	Public Property MetroForeColor As System.Drawing.Color
	Public Property MetroBorderColor As System.Drawing.Color
	Private LoadingPanel As New DevExpress.XtraSplashForm.SplashFormBase
	Private Property strWaitMsg As String

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _MySetting As ClsParifondSetting)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_InitializationData = _setting
		m_md = New Mandant
		m_UtilityUI = New UtilityUI

		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._PControlSetting = _MySetting

		Try

			If ModulConstants.MDData.MDNr = 0 Or ModulConstants.MDData Is Nothing Then
				ModulConstants.MDData = ModulConstants.SelectedMDData(0)
				ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

				ModulConstants.PersonalizedData = ModulConstants.PersonalizedValues
				ModulConstants.TranslationData = ModulConstants.TranslationValues

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


		'Dim liColor As List(Of Color) = GetMetroColor("INFO")    ' Color.White |  Color.Orange

		'If liColor.Count < 1 Then liColor = New List(Of Color)(New Color() {Color.White, Color.Orange})
		'Me.MetroForeColor = liColor(0)
		'Me.MetroBorderColor = liColor(1)

	End Sub

	Private Sub frmPControl_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmParfiondLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iParifondWidth = Me.Width
			My.Settings.iParifonHeight = Me.Height

			My.Settings.Save()
		End If

	End Sub

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(Me.cmdClose.Text)
		Me.cmdCreateData.Text = m_Translate.GetSafeTranslationValue(Me.cmdCreateData.Text)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblESListeState.Text = m_Translate.GetSafeTranslationValue(Me.lblMAStamm.Text)

		Me.lblMAStamm.Text = m_Translate.GetSafeTranslationValue(Me.lblMAStamm.Text)
		Me.lblMACount.Text = m_Translate.GetSafeTranslationValue(Me.lblMACount.Text)

		Me.lblKDStamm.Text = m_Translate.GetSafeTranslationValue(Me.lblKDStamm.Text)
		Me.lblKDCount.Text = m_Translate.GetSafeTranslationValue(Me.lblKDCount.Text)

		Me.lblESVertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblESVertrag.Text)
		Me.lblESVertragCount.Text = m_Translate.GetSafeTranslationValue(Me.lblESVertragCount.Text)

		Me.lblVerleihvertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblVerleihvertrag.Text)
		Me.lblVerleihCount.Text = m_Translate.GetSafeTranslationValue(Me.lblVerleihCount.Text)

		Me.lblErfassteRP.Text = m_Translate.GetSafeTranslationValue(Me.lblErfassteRP.Text)
		Me.lblRPCount.Text = m_Translate.GetSafeTranslationValue(Me.lblRPCount.Text)

		Me.lblLohnabrechnungen.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnabrechnungen.Text)
		Me.lblLOCount.Text = m_Translate.GetSafeTranslationValue(Me.lblLOCount.Text)

		lblLohnkonti.Text = m_Translate.GetSafeTranslationValue(lblLohnkonti.Text)
		lblLohnkontiCount.Text = m_Translate.GetSafeTranslationValue(lblLohnkontiCount.Text)

		lblOutputfiles.Text = m_Translate.GetSafeTranslationValue(lblOutputfiles.Text)
		tgsMergePDFFile.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsMergePDFFile.Properties.OffText)
		tgsMergePDFFile.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsMergePDFFile.Properties.OnText)

	End Sub

	Private Sub frmPControl_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		Try
			If Not mGlobalWorker Is Nothing Then
				If mGlobalWorker.IsBusy Then mGlobalWorker.CancelAsync()
				If mGlobalWorker.IsBusy Then e.Cancel = True
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub


	Private Sub frmPControl_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		Try
			Me.CircularProgress1.Visible = False

			Me.lblESListeState.Appearance.ImageList = Me.ImageList1
			Me.lblMACount.Appearance.ImageList = Me.ImageList1
			Me.lblKDCount.Appearance.ImageList = Me.ImageList1
			Me.lblESVertragCount.Appearance.ImageList = Me.ImageList1
			Me.lblVerleihCount.Appearance.ImageList = Me.ImageList1
			Me.lblRPCount.Appearance.ImageList = Me.ImageList1
			Me.lblLOCount.Appearance.ImageList = Me.ImageList1
			Me.lblLohnkontiCount.Appearance.ImageList = Me.ImageList1

			Me.lblESListeState.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblMACount.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblKDCount.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblESVertragCount.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblVerleihCount.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblRPCount.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblLOCount.Appearance.ImageAlign = ContentAlignment.MiddleRight
			Me.lblLohnkontiCount.Appearance.ImageAlign = ContentAlignment.MiddleRight

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Image in Lable:{1}", strMethodeName, ex.Message))

		End Try
		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
			Try
				StyleManager.MetroColorGeneratorParameters = New MetroColorGeneratorParameters(Me.MetroForeColor, Me.MetroBorderColor)
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.StyleManager: {1}", strMethodeName, ex.Message))
			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try


		Try
			If My.Settings.frmParfiondLocation <> String.Empty Then
				Me.Width = Math.Max(My.Settings.iParifondWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iParifonHeight, Me.Height)
				Dim aLoc As String() = My.Settings.frmParfiondLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			Dim test As New List(Of ClsParifondSetting)
			Dim lblOneRecord As String = m_Translate.GetSafeTranslationValue("{0} Datensatz wurde gefunden.")
			Dim lblMoreRecords As String = m_Translate.GetSafeTranslationValue("{0} Datensätze wurden gefunden.")

			Me.lblESListeState.Text = m_Translate.GetSafeTranslationValue("Liste wird erstellt")

			lblMACount.Text = String.Format(If(Me._PControlSetting.liMANr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liMANr2Print.Distinct().ToList().Count)
			lblKDCount.Text = String.Format(If(Me._PControlSetting.liKDNr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liKDNr2Print.Distinct().ToList().Count)
			lblESVertragCount.Text = String.Format(If(Me._PControlSetting.liESNr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liESNr2Print.Distinct().ToList().Count)
			lblVerleihCount.Text = String.Format(If(Me._PControlSetting.liESNr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liESNr2Print.Distinct().ToList().Count)
			lblRPCount.Text = String.Format(If(Me._PControlSetting.liRPNr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liRPNr2Print.Distinct().ToList().Count)
			lblLOCount.Text = String.Format(If(Me._PControlSetting.liLONr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liLONr2Print.Distinct().ToList().Count)
			lblLohnkontiCount.Text = String.Format(If(Me._PControlSetting.liMANr2Print.Count > 1, lblMoreRecords, lblOneRecord), _PControlSetting.liMANr2Print.Distinct().ToList().Count)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Anzahldatensätze. {1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Function DeleteTemporaryFiles() As Boolean
		Dim strMsg As String = String.Empty

		'strMsg = m_Translate.GetSafeTranslationValue("Folgende Verzeichnisse/Unterverzeichnisse müssen gelöscht werden:") & " "
		'strMsg &= String.Format("{1}{1}{0}{1}", _ClsProgSetting.GetSpSBildFiles2DeletePath, vbNewLine)

		'If Directory.Exists(_ClsProgSetting.GetSpSTempFolder) Then
		'	strMsg &= String.Format("{0}*.PDF{1}", _ClsProgSetting.GetSpSTempFolder, vbNewLine)
		'	strMsg &= String.Format("{0}*.J*{1}", _ClsProgSetting.GetSpSTempFolder, vbNewLine)
		'	strMsg &= String.Format("{0}*.B*{1}", _ClsProgSetting.GetSpSTempFolder, vbNewLine)
		'End If

		'If Directory.Exists(_ClsProgSetting.GetSpSMATempPath) Then
		'	strMsg &= String.Format("{0}{1}", _ClsProgSetting.GetSpSMATempPath, vbNewLine)
		'End If

		'If Directory.Exists(_ClsProgSetting.GetSpSKDTempPath) Then
		'	strMsg &= String.Format("{0}{1}", _ClsProgSetting.GetSpSKDTempPath, vbNewLine)
		'End If

		'If Directory.Exists(_ClsProgSetting.GetSpSESTempPath) Then
		'	strMsg &= String.Format("{0}{1}", _ClsProgSetting.GetSpSESTempPath, vbNewLine)
		'End If

		'strMsg &= String.Format("{0}{1}", _ClsProgSetting.GetSpSRPTempPath, vbNewLine)
		'If _ClsProgSetting.GetSpSRPTempPath <> _PControlSetting.GetRPExportPfad AndAlso _PControlSetting.GetRPExportPfad <> String.Empty Then
		'	If Directory.Exists(_PControlSetting.GetRPExportPfad) Then strMsg &= String.Format("{0}{1}", _PControlSetting.GetRPExportPfad, vbNewLine)
		'End If

		'If Directory.Exists(_ClsProgSetting.GetSpSLOTempPath) Then
		'	strMsg &= String.Format("{0}{1}", _ClsProgSetting.GetSpSLOTempPath, vbNewLine)
		'End If

		'strMsg &= String.Format(m_Translate.GetSafeTranslationValue("{0}Darf ich für Sie diese Daten endgültig löschen?{0} Wenn nicht wird der Vorgang abgebrochen!{0}"), vbNewLine)
		'If DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Verzeichnisse bereinigen"),
		'																							MessageBoxButtons.YesNo, MessageBoxIcon.Question,
		'																							MessageBoxDefaultButton.Button1) = DialogResult.No Then
		'	m_Logger.LogInfo("Die Verzeichnisse dürften nicht gelöscht werden!")
		'	Return False
		'End If

		Try
			strMsg = String.Empty
			Try
				If Directory.Exists(m_InitializationData.UserData.spAllowedPicturePath) Then Directory.Delete(m_InitializationData.UserData.spAllowedPicturePath, True)
			Catch ex As Exception
				strMsg = String.Format("Verzeichnis für Bilder: {1}{0}", vbNewLine, ex.Message)

			End Try
			Try
				Dim dir = New DirectoryInfo(m_InitializationData.UserData.SPTempPath)
				For Each file In dir.EnumerateFiles("*.pdf")
					file.Delete()
				Next
				For Each file In dir.EnumerateFiles("*.J*")
					file.Delete()
				Next
				For Each file In dir.EnumerateFiles("*.b*")
					file.Delete()
				Next

			Catch ex As Exception
				strMsg = String.Format("Verzeichnis für Lohnabrechnungen: {1}{0}", vbNewLine, ex.Message)
			End Try

			Try
				If Directory.Exists(m_InitializationData.UserData.spTempPayrollPath) Then Directory.Delete(m_InitializationData.UserData.spTempPayrollPath, True)
			Catch ex As Exception
				strMsg = String.Format("Verzeichnis für Lohnabrechnungen: {1}{0}", vbNewLine, ex.Message)

			End Try
			Try
				If Directory.Exists(m_InitializationData.UserData.spTempEmplymentPath) Then Directory.Delete(m_InitializationData.UserData.spTempEmplymentPath, True)

			Catch ex As Exception
				strMsg &= String.Format("Verzeichnis für Einsatzverträge: {1}{0}", vbNewLine, ex.Message)

			End Try
			Try
				If Directory.Exists(m_InitializationData.UserData.spTempRepportPath) Then Directory.Delete(m_InitializationData.UserData.spTempRepportPath, True)
			Catch ex As Exception
				strMsg &= String.Format("Verzeichnis für Rapportdaten: {1}{0}", vbNewLine, ex.Message)

			End Try
			Try
				If Directory.Exists(m_InitializationData.UserData.spTempEmployeePath) Then Directory.Delete(m_InitializationData.UserData.spTempEmployeePath, True)
			Catch ex As Exception
				strMsg &= String.Format("Verzeichnis für Kandidatenstamm: {1}{0}", vbNewLine, ex.Message)

			End Try
			Try
				If Directory.Exists(m_InitializationData.UserData.spTempCustomerPath) Then Directory.Delete(m_InitializationData.UserData.spTempCustomerPath, True)
			Catch ex As Exception
				strMsg &= String.Format("Verzeichnis für Kundenstamm: {1}{0}", vbNewLine, ex.Message)

			End Try
			If strMsg.Length > 0 Then Throw New Exception(strMsg)

			If Not Directory.Exists(m_InitializationData.UserData.spAllowedPicturePath) Then Directory.CreateDirectory(m_InitializationData.UserData.spAllowedPicturePath)
			If Not Directory.Exists(m_InitializationData.UserData.SPTempPath) Then Directory.CreateDirectory(m_InitializationData.UserData.SPTempPath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempPayrollPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempPayrollPath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempEmplymentPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempEmplymentPath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempEmployeePath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempEmployeePath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempCustomerPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempCustomerPath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempRepportPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempRepportPath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempInvoicePath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempInvoicePath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempNLAPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempNLAPath)
			If Not Directory.Exists(m_InitializationData.UserData.spTempOfferPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempOfferPath)


			Return True

		Catch ex As Exception
			strMsg = "In Ihren Temporären Verzeichnisse existieren Dateien, welche ich löschen möchte.{0}Leider können die Temporären Dateien nicht gelöscht werden.{0}Der Vorgang wird abgebrochen!{0}{1}"
			strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, ex.Message)
			'm_UtilityUI.ShowOKDialog(Me, strMsg, m_Translate.GetSafeTranslationValue("Temporäre Daten löschen"), MessageBoxIcon.stop)

			Return True
		End Try


	End Function

	Private Sub cmdCreateData_Click(sender As System.Object, e As System.EventArgs) Handles cmdCreateData.Click
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMsg As String = String.Empty
		Dim strSuccessMsg As String = String.Empty

		If Not DeleteTemporaryFiles() Then Return

		Me._PControlSetting.liCreatedFinalFile.Clear()
		Me._PControlSetting.liCreatedError.Clear()
		Me._PControlSetting.MergePDFFiles = tgsMergePDFFile.EditValue

		ESListeDownloaded = False
		KandidatDownloaded = False
		KundeDownloaded = False
		ESVErtragDownloaded = False
		VerleihDownloaded = False
		RPContentDownloaded = False
		LODownloaded = False
		LohnkontiDownloaded = False

		KandidatIsWorking = False
		KundeIsWorking = False
		ESVErtragIsWorking = False
		VerleihIsWorking = False
		RPContentIsWorking = False
		LOIsWorking = False
		LohnkontiIsWorking = False

		Me.lblESListeState.Appearance.Image = Nothing
		Me.lblMACount.Appearance.Image = Nothing
		Me.lblKDCount.Appearance.Image = Nothing
		Me.lblESVertragCount.Appearance.Image = Nothing
		Me.lblVerleihCount.Appearance.Image = Nothing
		Me.lblRPCount.Appearance.Image = Nothing
		Me.lblLOCount.Appearance.Image = Nothing
		Me.lblLohnkontiCount.Appearance.Image = Nothing


		If Me.sbESListe.Value Then
			StartWithESListe()
			ESListeDownloaded = True
		End If


		ToastNotification.ToastBackColor = GetToastBackColor("Info")
		ToastNotification.ToastForeColor = GetToastForeColor("Info")
		'ToastNotification.ToastFont = New System.Drawing.Font("tahoma", 8.25F, System.Drawing.FontStyle.Regular)
		ToastNotification.DefaultTimeoutInterval = 0

		mGlobalWorker = New BackgroundWorker
		mGlobalWorker.WorkerReportsProgress = True
		mGlobalWorker.WorkerSupportsCancellation = True

		AddHandler mGlobalWorker.DoWork, AddressOf StartWithGlobal
		AddHandler mGlobalWorker.ProgressChanged, AddressOf mGlobalWorker_ProgressChanged
		AddHandler mGlobalWorker.RunWorkerCompleted, AddressOf StartWithGlobalCompleted

		mGlobalWorker.RunWorkerAsync()

	End Sub

	Sub StartWithGlobal(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)


		Try
			strWaitMsg = m_Translate.GetSafeTranslationValue("Ihre Daten werden zusammengestellt. Bitte warten Sie einen Augenblick") & "..."
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		If Me.sbMAStamm.Value Then
			StartWithKandidat()
			KandidatDownloaded = True
		End If
		If mGlobalWorker.CancellationPending Then e.Cancel = True : Return


		If Me.sbKDStamm.Value Then
			StartWithKunde()
			Me.KundeDownloaded = True
		End If
		If mGlobalWorker.CancellationPending Then e.Cancel = True : Return

		If Me.sbESVertrag.Value Then
			StartWithESVertrag()
			Me.ESVErtragDownloaded = True
		End If
		If mGlobalWorker.CancellationPending Then e.Cancel = True : Return

		If Me.sbVerleih.Value Then
			StartWithVerleih()
			Me.VerleihDownloaded = True
		End If
		If mGlobalWorker.CancellationPending Then e.Cancel = True : Return

		If Me.sbRPContent.Value Then
			StartWithRPContent()
			Me.RPContentDownloaded = True
		End If
		If mGlobalWorker.CancellationPending Then e.Cancel = True : Return

		If Me.sbLO.Value Then
			StartWithLO()
			Me.LODownloaded = True
		End If
		If mGlobalWorker.CancellationPending Then e.Cancel = True : Return

		If Me.sbLohnkonti.Value Then
			StartWithLohnkonti()
			Me.LohnkontiDownloaded = True
		End If

	End Sub

	Sub StartWithGlobalCompleted()

		Me.CircularProgress1.IsRunning = False
		Me.CircularProgress1.Visible = False
		ToastNotification.Close(Me)

		CancelWorkingState()
		SetLableImages()

		Dim frm As New frmParifondResult(m_InitializationData)
		frm.PControlSetting = Me._PControlSetting
		frm.Show()
		frm.BringToFront()

	End Sub

	Private Sub mGlobalWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) 'Handles mGlobalWorker.ProgressChanged
		Me.strWaitMsg = Now.Ticks

		ShowMessage()
	End Sub

	Sub SetLableImages()

		Me.CircularProgress1.Visible = False
		If Me.ESListeIsWorking Then Me.CircularProgress1.Top = Me.sbESListe.Top
		If Me.KandidatIsWorking Then Me.CircularProgress1.Top = Me.sbMAStamm.Top
		If Me.KundeIsWorking Then Me.CircularProgress1.Top = Me.sbKDStamm.Top
		If Me.ESVErtragIsWorking Then Me.CircularProgress1.Top = Me.sbESVertrag.Top
		If Me.VerleihIsWorking Then Me.CircularProgress1.Top = Me.sbVerleih.Top
		If Me.RPContentIsWorking Then Me.CircularProgress1.Top = Me.sbRPContent.Top
		If Me.LOIsWorking Then Me.CircularProgress1.Top = Me.sbLO.Top
		If Me.LohnkontiIsWorking Then Me.CircularProgress1.Top = Me.sbLohnkonti.Top
		Me.CircularProgress1.Visible = ESListeIsWorking OrElse Me.KandidatIsWorking OrElse Me.KundeIsWorking OrElse Me.ESVErtragIsWorking OrElse Me.VerleihIsWorking OrElse Me.RPContentIsWorking OrElse Me.LOIsWorking OrElse LohnkontiIsWorking

		Me.lblESListeState.Appearance.ImageIndex = If(Me.ESListeDownloaded, 0, -1)
		Me.lblMACount.Appearance.ImageIndex = If(Me.KandidatDownloaded, 0, -1)
		Me.lblKDCount.Appearance.ImageIndex = If(Me.KundeDownloaded, 0, -1)
		Me.lblESVertragCount.Appearance.ImageIndex = If(Me.ESVErtragDownloaded, 0, -1)
		Me.lblVerleihCount.Appearance.ImageIndex = If(Me.VerleihDownloaded, 0, -1)
		Me.lblRPCount.Appearance.ImageIndex = If(Me.RPContentDownloaded, 0, -1)
		Me.lblLOCount.Appearance.ImageIndex = If(Me.LODownloaded, 0, -1)
		Me.lblLohnkontiCount.Appearance.ImageIndex = If(Me.LohnkontiDownloaded, 0, -1)

		Me.lblESListeState.Refresh()
		Me.lblMACount.Refresh()
		Me.lblKDCount.Refresh()
		Me.lblESVertragCount.Refresh()
		Me.lblVerleihCount.Refresh()
		Me.lblRPCount.Refresh()
		Me.lblLOCount.Refresh()
		Me.lblLohnkontiCount.Refresh()

	End Sub

	Sub CancelWorkingState()

		Me.ESListeIsWorking = False
		Me.KandidatIsWorking = False
		Me.KundeIsWorking = False
		Me.ESVErtragIsWorking = False
		Me.VerleihIsWorking = False
		Me.RPContentIsWorking = False
		Me.LOIsWorking = False
		Me.LohnkontiIsWorking = False

		Me.ResumeLayout()

	End Sub

#Region "Messaging..."


	Sub ShowMessage()

		Me.SuspendLayout()
		SetLableImages()

		strWaitMsg = m_Translate.GetSafeTranslationValue("Ihre Daten werden importiert. Bitte warten Sie einen Augenblick") & "..."
		Me.CircularProgress1.Visible = True
		Me.CircularProgress1.IsRunning = True

		ToastNotification.Show(Me, strWaitMsg, Nothing, ToastNotification.DefaultTimeoutInterval,
							   eToastGlowColor.None, eToastPosition.BottomLeft)

	End Sub

#End Region



#Region "Kandidaten..."


	Sub StartWithESListe()
		Dim exportFilename = Path.Combine(_ClsProgSetting.GetSpSTempFolder, "Einsatzliste.csv")
		Try
			If File.Exists(exportFilename) Then File.Delete(exportFilename)
		Catch ex As Exception
			Me._PControlSetting.liCreatedError.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Datei Konnte nicht erstellt werden!"), exportFilename))
			Return

		End Try

		Dim ESUtility As New ClsESListUtility(m_InitializationData)

		ESUtility.m_TableName = String.Format("_EinsatzListe_{0}", m_InitializationData.UserData.UserNr)
		ESUtility.PControlSetting = _PControlSetting
		Dim success As Boolean = ESUtility.CreateESListToCsv(exportFilename, True)

		If success Then
			Me._PControlSetting.liCreatedFinalFile.Add(exportFilename)
		Else
			Me._PControlSetting.liCreatedError.Add(m_Translate.GetSafeTranslationValue("Konnte nicht erstellt werden"))
		End If

	End Sub

	Sub StartWithKandidat()
		Dim _clsKandidat As New ClsMAUtility(Me._PControlSetting)

		Try
			CancelWorkingState()
			Me.KandidatIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		Dim strResult = _clsKandidat.CreateData4Kandidaten()
		If File.Exists(strResult) Then
			Me._PControlSetting.liCreatedFinalFile.Add(strResult)

		Else
			Me._PControlSetting.liCreatedError.Add(strResult)

		End If

	End Sub

	Sub StartWithKandidatCompleted()

	End Sub


#End Region


#Region "Kunden..."

	Sub StartWithKunde()
		Dim _clsKunde As New ClsKDUtility(Me._PControlSetting)

		Try
			CancelWorkingState()
			Me.KundeIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		Dim strResult = _clsKunde.CreateData4Kunden()
		If File.Exists(strResult) Then
			Me._PControlSetting.liCreatedFinalFile.Add(strResult)

		Else
			Me._PControlSetting.liCreatedError.Add(strResult)

		End If

	End Sub

	Sub StartWithKundeCompleted()

	End Sub

#End Region


#Region "Einsatverträge..."

	Sub StartWithESVertrag()
		Dim _clsESVertag As New ClsESVertragUtility(Me._PControlSetting)
		Try
			CancelWorkingState()
			Me.ESVErtragIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		Dim strResult = _clsESVertag.CreateData4ESVertrag()
		If File.Exists(strResult) Then
			Me._PControlSetting.liCreatedFinalFile.Add(strResult)

		Else
			Me._PControlSetting.liCreatedError.Add(strResult)

		End If

	End Sub

	Sub StartWithESVertragCompleted()

	End Sub

#End Region


#Region "Verleihverträge..."

	Sub StartWithVerleih()
		Dim _clsESVertag As New ClsESVerleihUtility(Me._PControlSetting)
		Try
			CancelWorkingState()
			Me.VerleihIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		Dim strResult = _clsESVertag.CreateData4Verleih()
		If File.Exists(strResult) Then
			Me._PControlSetting.liCreatedFinalFile.Add(strResult)

		Else
			Me._PControlSetting.liCreatedError.Add(strResult)

		End If

	End Sub

	Sub StartWithVerleihCompleted()

	End Sub


#End Region


#Region "Rapportinhalte..."

	Sub StartWithRPContent()
		Dim _clsRPContent As New ClsRPContentUtility(m_InitializationData, Me._PControlSetting)
		Try
			CancelWorkingState()
			Me.RPContentIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		Dim strResult = _clsRPContent.CreateAllData4RPContent()
		If File.Exists(strResult) Then
			Me._PControlSetting.liCreatedFinalFile.Add(strResult)

		Else
			Me._PControlSetting.liCreatedError.Add(strResult)

		End If

	End Sub

	Sub StartWithRPContentCompleted()

	End Sub


#End Region


#Region "Lohnabrechnungen..."

	Sub StartWithLO()
		Dim _clsLO As New ClsLoUtility(m_InitializationData)
		_clsLO.ParifondSetting = _PControlSetting

		Try
			CancelWorkingState()
			Me.LOIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		If Not mGlobalWorker.CancellationPending Then
			Dim strResult = _clsLO.CreateData4LO()
			If File.Exists(strResult) Then
				Me._PControlSetting.liCreatedFinalFile.Add(strResult)

			Else
				Me._PControlSetting.liCreatedError.Add(strResult)

			End If

		End If


	End Sub

	Sub StartWithLohnkonti()

		Try
			CancelWorkingState()
			Me.LohnkontiIsWorking = True
			Dim MsgDelegate As New MethodInvoker(AddressOf ShowMessage)
			Me.Invoke(MsgDelegate)

		Catch ex As Exception

		End Try

		If Not mGlobalWorker.CancellationPending Then
			Dim firstYear As Integer = _PControlSetting.FirstYear
			Dim lastYear As Integer = _PControlSetting.LastYear

			Dim monthFrom As Integer = 1
			Dim monthTo As Integer = 12

			For i As Integer = firstYear To lastYear

				If i = firstYear AndAlso i = lastYear Then
					monthFrom = _PControlSetting.FirstMonth
					monthTo = _PControlSetting.LastMonth

				ElseIf i = firstYear AndAlso i < lastYear Then
					monthFrom = _PControlSetting.FirstMonth
					monthTo = 12

				ElseIf i > firstYear AndAlso i < lastYear Then
					monthFrom = 1
					monthTo = 12

				ElseIf i > firstYear AndAlso i = lastYear Then
					monthFrom = 1
					monthTo = _PControlSetting.LastMonth

				End If

				'Trace.WriteLine(String.Format("From-To: {0}-{1} {2}", monthFrom, monthTo, i))

				Dim data = LoadEmployeePayrollLohnkontiData(_PControlSetting.liMANr2Print, i, monthFrom, monthTo)
				If data Is Nothing Then Return
				If Not data Is Nothing AndAlso data.Count > 0 Then ExportLohhnkontiData(data, i, monthFrom, monthTo)

			Next

			'Else
			'	Dim data = LoadEmployeePayrollLohnkontiData(_PControlSetting.liMANr2Print, _PControlSetting.SelectedVonDate, _PControlSetting.SelectedBisDate)
			'End If

			'If data Is Nothing Then Return
			'ExportLohhnkontiData(data)

		End If


	End Sub

	Private Function LoadEmployeePayrollLohnkontiData(ByVal employeeNumbers As List(Of Integer), ByVal assignedYear As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer) As IEnumerable(Of ListingPayrollLohnkontiData)
		Dim lohnkontidata = m_ListingDatabaseAccess.LoadAnnualLohnkontiData(m_InitializationData.MDData.MDNr, assignedYear, monthFrom, monthTo, String.Join(",", employeeNumbers))

		If (lohnkontidata Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnkonti-Daten konnten nicht geladen werden."))
			Return Nothing
		End If

		Return lohnkontidata
	End Function

	Private Sub ExportLohhnkontiData(ByVal lohnKontiData As List(Of ListingPayrollLohnkontiData), ByVal assignedYear As Integer, ByVal monthFrom As Integer, ByVal monthTo As Integer)
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLLohnKontiSearchPrintSetting With {.m_InitializationData = m_InitializationData, .LohnKontiData = lohnKontiData}

		_Setting.LohnKontiData = lohnKontiData
		_Setting.SQL2Open = "[Load Payroll Data For LohnKonti]"
		_Setting.PrintJobNumber = "9.4"
		'_Setting.frmhwnd = Me.Handle
		_Setting.ListFilterBez = New List(Of String)(New String() {String.Empty})
		_Setting.ShowAsDesign = False
		_Setting.SelectedYear = assignedYear
		_Setting.SelectedMonth = monthFrom
		_Setting.ExportData = True

		Dim obj As New SPS.Listing.Print.Utility.LohnKontiSearchListing.ClsPrintLohnKontiSearchList(m_InitializationData)
		obj.PrintData = _Setting

		obj.PrintLohnKontiSearchList()
		If Not _Setting.ExportedFiles Is Nothing AndAlso _Setting.ExportedFiles.Count > 0 Then
			For Each itm In _Setting.ExportedFiles
				_PControlSetting.liCreatedFinalFile.Add(itm)
			Next
		End If

		obj.Dispose()

	End Sub

	Sub StartWithLOCompleted()

	End Sub


#End Region


	Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click

		Me.Dispose()

	End Sub



End Class