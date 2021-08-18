
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.Utils
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility
Imports SP.Infrastructure.Logging

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.MA.MassenVersand.ClsDataDetail


Public Class frmMABerufgruppe
	Inherits XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Private _MANr As Integer

	Private Property GetSelectedID As Integer
	Private Property GetBerufID As Integer
	Private Property GetBerufBez As String
	Private Property GetFachID As Integer
	Private Property GetFachBez As String

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.m_InitialData.TranslationData, ClsDataDetail.m_InitialData.ProsonalizedData)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()


	End Sub

#End Region


	Private Sub frmMABerufgruppe_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Try
			If Me.WindowState <> FormWindowState.Minimized Then
				My.Settings.frmGruppLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

				My.Settings.Save()
			End If

		Catch ex As Exception

		End Try

	End Sub

	Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)

			Me.lblBerufgruppe.Text = m_Translate.GetSafeTranslationValue(Me.lblBerufgruppe.Text)
			Me.lblFachbereiche.Text = m_Translate.GetSafeTranslationValue(Me.lblFachbereiche.Text)
			Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(Me.cmdClose.Text)
			Me.cmdDelete.Text = m_Translate.GetSafeTranslationValue(Me.cmdDelete.Text)
			Me.cmdSave.Text = m_Translate.GetSafeTranslationValue(Me.cmdSave.Text)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub frmMABerufgruppe_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Me._MANr = ClsDataDetail.GetMANumber

		Try
			If My.Settings.frmGruppLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmGruppLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			Try
				Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
				If strStyleName <> String.Empty Then
					UserLookAndFeel.Default.SetSkinStyle(strStyleName)
				End If

			Catch ex As Exception

			End Try

		Catch ex As Exception

		End Try

		FillBerufGruppeLV(Me.lvMABerufe, Me._MANr)

	End Sub


	Private Sub LookUpEdit15_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEdit15.ButtonClick

		If e.Button.Index = 1 Then
			sender.EditValue = 0
			sender.Properties.NullText = String.Empty
			Me.lblJBerufErfahrung_0.Text = String.Empty

			Me.LookUpEdit16.EditValue = 0
			Me.LookUpEdit16.Properties.NullText = String.Empty
			Me.lblJBerufErfahrungFach_0.Text = String.Empty

			Me.GetBerufID = 0
			Me.GetFachID = 0
			Me.GetBerufBez = String.Empty
			Me.GetFachBez = String.Empty
			Me.GetSelectedID = 0
		End If

	End Sub


	' Berufserfahrungen...
	Private Sub LookUpEdit15_EditValueChanged(sender As Object, _
															 e As System.EventArgs) Handles LookUpEdit15.EditValueChanged
		Dim test As Object = Me.LookUpEdit15.GetSelectedDataRow
		Dim currow As DataRowView = TryCast(LookUpEdit15.GetSelectedDataRow(), DataRowView)
		If Not currow Is Nothing Then
			Me.lblJBerufErfahrung_0.Text = currow("ID_1").ToString()
			Me.GetBerufID = currow("ID_1").ToString()
			Me.GetBerufBez = Me.LookUpEdit15.Text

			Me.GetFachID = 0
			Me.GetFachBez = String.Empty

		End If

	End Sub

	Private Sub LookUpEdit15_QueryCloseUp(sender As Object, _
																			 e As System.ComponentModel.CancelEventArgs) Handles LookUpEdit15.QueryCloseUp

		Dim test As Object = Me.LookUpEdit15.GetSelectedDataRow
		Dim currow As DataRowView = TryCast(LookUpEdit15.GetSelectedDataRow(), DataRowView)
		If Not currow Is Nothing Then
			Me.lblJBerufErfahrung_0.Text = currow("ID_1").ToString()
			Me.GetBerufID = currow("ID_1").ToString()
			Me.GetBerufBez = Me.LookUpEdit15.Text

			Me.GetFachID = 0
			Me.GetFachBez = String.Empty
		End If

	End Sub

	Private Sub LookUpEdit15_QueryPopUp(sender As Object, _
																		 e As System.ComponentModel.CancelEventArgs) Handles LookUpEdit15.QueryPopUp
		Dim _ClsWSDb As New ClsWSFunctions
		If Me._MANr = 0 Then Exit Sub
		Me.LookUpEdit15.Properties.Columns.Clear()
		Dim dt As DataTable = _ClsWSDb.ListJobCHBerufGruppe("DE").Tables(0)
		Me.LookUpEdit15.Properties.DataSource = dt

		LookUpEdit15.Properties.DisplayMember = "Bezeichnung"
		LookUpEdit15.Properties.ValueMember = "ID_1" ' "ZHDFullName"

		Dim Col0 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID_1", "ID_1", 0)
		Dim Col1 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Bezeichnung", _
																																		 _ClsProgSetting.TranslateText("Bezeichnung"), 300)

		'    Col2.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
		Col0.Visible = False
		LookUpEdit15.Properties.Columns.Add(Col0)
		LookUpEdit15.Properties.Columns.Add(Col1)

		LookUpEdit15.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		LookUpEdit15.Properties.ForceInitialize()

	End Sub

	Private Sub LookUpEdit16_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles LookUpEdit16.ButtonClick

		If e.Button.Index = 1 Then
			sender.EditValue = 0
			sender.Properties.NullText = String.Empty
			Me.lblJBerufErfahrungFach_0.Text = String.Empty

			Me.GetBerufID = 0
			Me.GetBerufBez = String.Empty
			Me.GetSelectedID = 0
		End If

	End Sub

	Private Sub LookUpEdit16_EditValueChanged(sender As Object, _
															 e As System.EventArgs) Handles LookUpEdit16.EditValueChanged
		Dim test As Object = Me.LookUpEdit16.GetSelectedDataRow
		Dim currow As DataRowView = TryCast(LookUpEdit16.GetSelectedDataRow(), DataRowView)
		If Not currow Is Nothing Then
			Me.lblJBerufErfahrungFach_0.Text = currow("ID_1").ToString()
			Me.GetFachID = currow("ID_1").ToString()
			Me.GetFachBez = Me.LookUpEdit16.Text
		End If

	End Sub

	Private Sub LookUpEdit16_QueryCloseUp(sender As Object, _
																			 e As System.ComponentModel.CancelEventArgs) Handles LookUpEdit16.QueryCloseUp

		Dim test As Object = Me.LookUpEdit16.GetSelectedDataRow
		Dim currow As DataRowView = TryCast(LookUpEdit16.GetSelectedDataRow(), DataRowView)
		If Not currow Is Nothing Then
			Me.lblJBerufErfahrungFach_0.Text = currow("ID_1").ToString()
			Me.GetFachID = currow("ID_1").ToString()
			Me.GetFachBez = Me.LookUpEdit16.Text
		End If

	End Sub

	Private Sub LookUpEdit16_QueryPopUp(sender As Object, _
																		 e As System.ComponentModel.CancelEventArgs) Handles LookUpEdit16.QueryPopUp
		If Me.LookUpEdit15.Text = String.Empty Then
			Dim strMessage As String = String.Empty
			strMessage = _ClsProgSetting.TranslateText("Sie müssen zuerst eine Berufegruppe auswählen.")
			Dim args As DevExpress.Utils.ToolTipControllerShowEventArgs = New DevExpress.Utils.ToolTipControllerShowEventArgs()
			Dim tip As DevExpress.Utils.SuperToolTip = New DevExpress.Utils.SuperToolTip()

			tip.AllowHtmlText = DefaultBoolean.True
			tip.DistanceBetweenItems = 20

			args.AppearanceTitle.TextOptions.WordWrap = WordWrap.Wrap
			args.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip
			args.AllowHtmlText = DefaultBoolean.True
			args.SuperTip = tip
			args.IconType = ToolTipIconType.Information
			args.Rounded = True
			args.Title = _ClsProgSetting.TranslateText("Fachbereiche auflisten")
			args.ToolTip = strMessage
			'      tip.Items(0).Appearance.Options.
			args.IconSize = ToolTipIconSize.Small
			args.AutoHide = True
			args.ToolTipStyle = ToolTipStyle.Windows7

			ToolTipController1.AllowHtmlText = True
			ToolTipController1.ShowHint(args, New Point(Cursor.Position.X, Cursor.Position.Y))


			'DevExpress.XtraEditors.XtraMessageBox.Show(strMessage, _ClsProgSetting.TranslateText("Daten auswählen"), _
			'                                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			LookUpEdit16.Properties.DataSource = Nothing
			Exit Sub
		End If
		Dim _ClsWSDb As New ClsWSFunctions

		If Me._MANr = 0 Then Exit Sub
		Me.LookUpEdit16.Properties.Columns.Clear()
		Dim dt As DataTable = _ClsWSDb.ListJobCHBerufFachbereich("DE", CInt(Val(Me.lblJBerufErfahrung_0.Text))).Tables(0)
		Me.LookUpEdit16.Properties.DataSource = dt

		LookUpEdit16.Properties.DisplayMember = "Bezeichnung"
		LookUpEdit16.Properties.ValueMember = "ID_1" ' "ZHDFullName"

		Dim Col0 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID_1", "ID_1", 0)
		Dim Col1 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Bezeichnung", _
																																		 _ClsProgSetting.TranslateText("Bezeichnung"), 300)

		'    Col2.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
		Col0.Visible = False
		LookUpEdit16.Properties.Columns.Add(Col0)
		LookUpEdit16.Properties.Columns.Add(Col1)

		LookUpEdit16.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		LookUpEdit16.Properties.ForceInitialize()

	End Sub



	Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click

		If Me.GetBerufID + Me.GetFachID = 0 Or Me.LookUpEdit15.Text = String.Empty Then Exit Sub
		If Me.GetBerufBez Is Nothing Then Me.GetBerufBez = String.Empty
		If Me.GetFachBez Is Nothing Then Me.GetFachBez = String.Empty
		Dim strResult As String = SaveBerufgruppe(Me._MANr, Me.GetBerufID, Me.GetFachID, Me.GetBerufBez, Me.GetFachBez)
		If strResult.ToString.ToLower.Contains("erfolg") Then
			Me.GetSelectedID = 0
			FillBerufGruppeLV(Me.lvMABerufe, Me._MANr)

			Me.LookUpEdit15.Properties.NullText = String.Empty
			Me.LookUpEdit16.Properties.NullText = String.Empty
			Me.LookUpEdit15.Properties.DataSource = Nothing
			Me.LookUpEdit16.Properties.DataSource = Nothing

		Else
			DevExpress.XtraEditors.XtraMessageBox.Show(strResult, _ClsProgSetting.TranslateText("Daten speichern"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)

		End If

	End Sub

	Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete.Click

		DeleteSelectedRec()

	End Sub

	Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
		Me.Close()
	End Sub

	Sub DeleteSelectedRec()

		If Me.GetSelectedID = 0 Then Exit Sub
		Dim strResult As String = DeleteBerufgruppe(Me.GetSelectedID, Me._MANr)
		If strResult.ToString.ToLower.Contains("erfolg") Then
			Me.GetSelectedID = 0

			Me.GetBerufID = 0
			Me.GetFachID = 0
			Me.GetBerufBez = String.Empty
			Me.GetFachBez = String.Empty

			For Each i As ListViewItem In Me.lvMABerufe.SelectedItems
				Me.lvMABerufe.Items.Remove(i)
			Next

		Else
			DevExpress.XtraEditors.XtraMessageBox.Show(strResult, _ClsProgSetting.TranslateText("Daten löschen"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)

		End If

	End Sub

	Private Sub lvMABerufe_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lvMABerufe.KeyUp
		If e.KeyCode = Keys.Delete Then
			DeleteSelectedRec()
		End If
	End Sub

	Private Sub lvMABerufe_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lvMABerufe.SelectedIndexChanged

		For Each index As Integer In lvMABerufe.SelectedIndices
			Me.GetSelectedID = lvMABerufe.Items(index).SubItems(0).Text

			Me.LookUpEdit15.Properties.NullText = String.Empty ' lvMABerufe.Items(index).SubItems(2).Text
			Me.LookUpEdit16.Properties.NullText = String.Empty ' lvMABerufe.Items(index).SubItems(4).Text
			Me.LookUpEdit15.Properties.DataSource = Nothing
			Me.LookUpEdit16.Properties.DataSource = Nothing

			Me.GetBerufID = lvMABerufe.Items(index).SubItems(1).Text
			Me.GetBerufBez = lvMABerufe.Items(index).SubItems(2).Text

			Me.GetFachID = lvMABerufe.Items(index).SubItems(3).Text
			Me.GetFachBez = lvMABerufe.Items(index).SubItems(4).Text

		Next

	End Sub


End Class