
Imports System.IO
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Utility


Public Class frmSearchRec
  Inherits DevExpress.XtraEditors.XtraForm
	Private Shared m_Logger As ILogger = New Logger()

	'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'Private _ClsReg As New SPProgUtility.ClsDivReg

	Private _ClsData As New ClsDivFunc
  Private Property GetModul2Show As Short

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	Private m_mandant As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

#Region "Constructor"

	Public Sub New(ByVal _Modul2Show As Short)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.GetModul2Show = _Modul2Show

		ClsDataDetail.strKDData = String.Empty
		m_mandant = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.m_InitialData.TranslationData, ClsDataDetail.m_InitialData.ProsonalizedData)

	End Sub

#End Region


  Public ReadOnly Property iMyValue(ByVal strValue As String) As String

    Get
      Dim strBez As String = String.Empty

      'If Me.LvData.SelectedItems.Count > 0 Then
      strBez = _ClsData.GetSelektion
      'End If

      ClsDataDetail.strKDData = strBez

      Return ClsDataDetail.strKDData
    End Get

  End Property

  Private Sub frmSearchRec_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

    If Asc(e.KeyChar) = Keys.Escape Then
      cmdClose_Click(sender, New System.EventArgs)
    End If

  End Sub

	Private Sub Onform_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation_Search = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iSearch_Width = Me.Width
				My.Settings.iSearch_Height = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

  Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
			Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)


		Catch ex As Exception

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				If My.Settings.frmLocation_Search <> String.Empty Then
					Me.Width = My.Settings.iSearch_Width
					Me.Height = My.Settings.iSearch_Height
					Dim aLoc As String() = My.Settings.frmLocation_Search.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
					End If
					Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try


    Try
      Dim dt As New DataTable
      dt = GetDbData4Listing(Me.GetModul2Show)
      grdGrid.DataSource = dt
      grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
      grdGrid.Name = "grd_MATemp"

      grdGrid.ForceInitialize()
      grdGrid.Visible = False
      Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
      grdGrid.Dock = DockStyle.Fill
      AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

      grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
      grdView.ShowFindPanel()

      grdView.OptionsSelection.MultiSelect = True
      grdView.OptionsBehavior.Editable = False
      grdView.OptionsSelection.EnableAppearanceFocusedCell = False
      grdView.OptionsSelection.InvertSelection = False
      grdView.OptionsSelection.EnableAppearanceFocusedRow = True
      grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      grdView.OptionsView.ShowGroupPanel = False
      grdGrid.ForceInitialize()

      Dim i As Integer = 0
      For Each col As GridColumn In grdView.Columns
        Trace.WriteLine(String.Format("{0}", col.FieldName))
        col.MinWidth = 0
        Try
          col.Visible = True ' col.FieldName.ToLower.Contains("firma1")
					col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

        Catch ex As Exception
          col.Visible = False

        End Try
        i += 1
      Next col
      If Me.GetModul2Show = 0 Then
        grdView.Columns("Code").Visible = False
      ElseIf Me.GetModul2Show = 1 Then
        grdView.Columns("Code").Visible = False

      End If
      grdGrid.Visible = True


    Catch ex As Exception

    End Try

  End Sub

  Private Sub ViewKD_RowClick(sender As Object, e As System.EventArgs)
    Dim strValue As String = String.Empty
    Dim strTbleName As String = String.Empty
    Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

    grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

    Try
      For i As Integer = 0 To grdView.SelectedRowsCount - 1
        Dim row As Integer = (grdView.GetSelectedRows()(i))
        If (grdView.GetSelectedRows()(i) >= 0) Then
          Dim dtr As DataRow
          dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
          strValue &= If(strValue = "", "", ", ") & dtr.Item("Bezeichnung").ToString

        End If
      Next i

      _ClsData.GetSelektion = strValue

    Catch ex As Exception

    End Try

    Me.Close()

  End Sub


  Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
    ViewKD_RowClick(Me.grdGrid, e)
  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
    Me.Close()
    Me.Dispose()
  End Sub


End Class