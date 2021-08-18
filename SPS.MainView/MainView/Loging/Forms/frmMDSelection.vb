
Imports SPProgUtility.SPUserSec.ClsUserSec

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
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPS.MainView.EmployeeSettings

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SPS.MainView.MainViewSettings


Public Class frmMDSelection

	Private Shared m_Logger As ILogger = New Logger()
	Protected m_SettingsManager As ISettingsManager

	Private m_md As Mandant
	Private m_UitilityUI As UtilityUI
	Private m_translate As TranslateValues


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant
		m_UitilityUI = New UtilityUI
		m_translate = New TranslateValues
		m_SettingsManager = New SettingsMainViewManager

		If Not LoadStartSettings() Then Exit Sub

	End Sub

#End Region

	Private Sub frmMDSelection_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		SaveFormProperties()
	End Sub

	Private Sub frmMDSelection_Load(sender As Object, e As EventArgs) Handles Me.Load

		SetFormLayout()

		ucMDList.LoadMDList()
		ucMDList.grdMDList.Focus()

	End Sub

	Private Sub OnFormKeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.Escape Then
			Me.Close()
		End If

	End Sub


	Private Sub SetFormLayout()

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Try
			Dim setingHight = m_SettingsManager.ReadInteger(SettingMainViewKeys.SETTING_MAINVIEW_FORM_HEIGHT)
			Dim setingWidth = m_SettingsManager.ReadInteger(SettingMainViewKeys.SETTING_MAINVIEW_FORM_WIDTH)
			Dim setingLocation = m_SettingsManager.ReadString(SettingMainViewKeys.SETTING_MAINVIEW_FORM_LOCATION)

			If My.Settings.SETTING_MAIN_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, setingHight)
			If My.Settings.SETTING_MAIN_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, setingWidth)
			If setingLocation <> String.Empty Then
				Dim aLoc As String() = setingLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Sub SaveFormProperties()

		m_SettingsManager.WriteInteger(SettingMainViewKeys.SETTING_MAINVIEW_FORM_HEIGHT, Me.Height)
		m_SettingsManager.WriteInteger(SettingMainViewKeys.SETTING_MAINVIEW_FORM_WIDTH, Me.Width)

		m_SettingsManager.WriteString(SettingMainViewKeys.SETTING_MAINVIEW_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))

		m_SettingsManager.SaveSettings()

	End Sub


End Class