
Imports SP.Infrastructure.Logging
Imports System.IO
Imports System.Windows.Forms
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten


Public Class frmSesam
  Inherits DevExpress.XtraEditors.XtraForm
	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_md As Mandant
	Private m_ExportSetting As ClsCSVSettings

	Sub New(ByVal _Setting As ClsCSVSettings, ByVal _init As SP.Infrastructure.Initialization.InitializeClass)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _init
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_md = New Mandant

		InitializeComponent()

		m_ExportSetting = New ClsCSVSettings
		m_ExportSetting = _Setting
		txtFilename.Text = If(String.IsNullOrWhiteSpace(_Setting.ExportFileName), My.Settings.Filename4SesamLO, _Setting.ExportFileName)
		chkExportOPData.Checked = My.Settings.ExportInvoiceData

		TranslateControls()

	End Sub

	Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim myDirName As String = m_InitializationData.UserData.SPTempPath
		Dim strResult As String = String.Empty

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick") & "..."
		If String.IsNullOrWhiteSpace(Me.txtFilename.Text) Then Me.txtFilename.Text = Path.Combine(m_InitializationData.UserData.SPTempPath, "Sesam_Export.TAF")

		m_ExportSetting.ExportFileName = Me.txtFilename.Text
		'_ExportSetting.FieldIn = Me.cbo_Darstellungszeichen.Text
		m_ExportSetting.FieldSeprator = " "
		m_ExportSetting.ExportInvoiceData = chkExportOPData.Checked

		If Me.m_ExportSetting.ModulName.ToLower = "sesamlo" Then
			Dim obj As New ExportSage.ClsExportLOInSesam(m_ExportSetting, m_InitializationData)
			strResult = obj.GetAllValueToSesam()
		End If

		If File.Exists(strResult) Then
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Die Datei wurde erfolgreich erstellt.")

		Else
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(String.Format("Fehler: {0}", strResult))

		End If

	End Sub

	Private Sub frmSesam_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = System.Windows.Forms.FormWindowState.Maximized Then
			My.Settings.frmSesamLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iSesamWidth = Me.Width
			My.Settings.iSesamHeight = Me.Height

			If m_ExportSetting.ModulName.ToLower = "sesamlo" Then My.Settings.Filename4SesamLO = Me.txtFilename.Text
			My.Settings.ExportInvoiceData = chkExportOPData.Checked


			My.Settings.Save()
		End If

	End Sub

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
		cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)

		chkExportOPData.Text = m_Translate.GetSafeTranslationValue(chkExportOPData.Text)
		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)

		lblHeader1.Text = m_Translate.GetSafeTranslationValue(lblHeader1.Text)
		lblHeader2.Text = m_Translate.GetSafeTranslationValue(lblHeader2.Text)
		lblDatei.Text = m_Translate.GetSafeTranslationValue(lblDatei.Text)
		lblInfo.Text = m_Translate.GetSafeTranslationValue(lblInfo.Text)

	End Sub

	Private Sub frmSesam_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsMainSetting.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				Me.Width = Math.Max(My.Settings.iSesamWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iSesamHeight, Me.Height)

				If My.Settings.frmSesamLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmSesamLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
					End If
					Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click

		Me.Dispose()

	End Sub

	Private Sub txtFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFilename.ButtonClick
		Dim fldlgList As New FolderBrowserDialog

		With fldlgList
			.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei.")
			.SelectedPath = Me.txtFilename.Text

      .ShowNewFolderButton = True
      If .ShowDialog() = DialogResult.OK Then
        Me.txtFilename.Text = String.Format("{0}{1}Sesam_Export.TAF", .SelectedPath,
                                             If(.SelectedPath.ToString.EndsWith("\"), "", "\"))
      End If

    End With

  End Sub


End Class