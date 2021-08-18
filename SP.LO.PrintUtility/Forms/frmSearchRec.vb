
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports SP.LO.PrintUtility.ClsDataDetail
Imports SP.Infrastructure

Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

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

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private _ClsData As New ClsDivFunc
	Private m_md As Mandant

	Private Property GetMDYear As Integer
	Private Property GetMDMonth As Integer
	Private Property Field2Select As String

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView



#Region "Constructor"

	Public Sub New(ByVal _setting As Initialization.InitializeClass, ByVal _field2Select As String, ByVal iYear As Integer, ByVal iMonth As Integer)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_UtilityUI = New UtilityUI
		m_md = New Mandant


		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.GetMDYear = iYear
		Me.GetMDMonth = iMonth
		Me.Field2Select = _field2Select
		ClsDataDetail.strButtonValue = String.Empty

		TranslateControls()

	End Sub


#End Region

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(Me.cmdOK.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)

	End Sub

	Public ReadOnly Property iMyValue(ByVal strValue As String) As String

		Get
			Dim strBez As String = String.Empty

			'If Me.LvData.SelectedItems.Count > 0 Then
			strBez = _ClsData.GetSelektion
			'End If

			ClsDataDetail.strButtonValue = strBez

			Return ClsDataDetail.strButtonValue
		End Get

	End Property

	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			Dim dt As New DataTable
			dt = GetMADbData4PrintLO(m_InitializationData.MDData.MDNr, Me.GetMDYear, Me.GetMDMonth)
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

			grdView.OptionsSelection.MultiSelect = Me.Field2Select = "Kandidat-Nr."
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
			If Me.Field2Select <> "Kandidat-Nr." Then
				grdView.Columns("Kandidat-Nr.").Visible = False
			End If

			grdGrid.Visible = True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

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
					strValue &= If(strValue = "", "", ", ") & dtr.Item(Me.Field2Select).ToString

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
		_ClsData.GetSelektion = String.Empty
		Me.Close()
		Me.Dispose()
	End Sub


End Class