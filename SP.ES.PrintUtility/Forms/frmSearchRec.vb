
Imports System.IO
Imports System.Data.SqlClient
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Data.Filtering

Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings


Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsData As New ClsDivFunc
	Private bAllowedtowrite As Boolean
	Private m_xml As New ClsXML
	Private m_md As Mandant

	Private Property GetMDYear As Integer
	Private Property GetMDMonth As Integer
	Private Property GetGAVBez As String
	Private Property GetGAVKanton As String

	Private Property Field2Select As String

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView


#Region "Constructor"


	Public Sub New()

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		GetMDYear = Year(Now)
		m_md = New Mandant

	End Sub

	Public Sub New(ByVal _strGAVBez As String, ByVal _strGAVKanton As String, _
								 ByVal _field2Select As String, ByVal _iYear As Integer, ByVal _iMonth As Integer)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()
		m_md = New Mandant

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.GetGAVBez = _strGAVBez
		Me.GetGAVKanton = _strGAVKanton
		Me.GetMDYear = _iYear
		Me.GetMDMonth = _iMonth
		Me.Field2Select = _field2Select

		ClsDataDetail.strButtonValue = String.Empty

	End Sub

#End Region


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
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception

		End Try

		Try
			Dim m_xml As New ClsXML
			Dim Time_1 As Double = System.Environment.TickCount
			m_xml.GetChildChildBez(Me)
			Dim Time_2 As Double = System.Environment.TickCount
			Trace.WriteLine("1. Verbrauchte Zeit: " & ((Time_2 - Time_1) / 1000) & " s.")

		Catch ex As Exception	' Manager
			MessageBoxShowError("frmDataSel_Load", ex)
		End Try

		Try
			Dim dt As New DataTable
			dt = GetMADbData4PrintLO(GetGAVBez, GetGAVKanton, Me.GetMDYear, Me.GetMDMonth)
			grdGrid.DataSource = dt
			grdGrid.MainView = grdGrid.CreateView("view_EStemp")
			grdGrid.Name = "grd_ESTemp"

			grdGrid.ForceInitialize()
			grdGrid.Visible = False
			Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			grdGrid.Dock = DockStyle.Fill
			'AddHandler pcc.SizeChanged, AddressOf pcc_SizeChanged
			AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

			grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
			'If My.Settings.bgrdView_EnterpriseShowGroup Then _
			'  grdView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways
			grdView.ShowFindPanel()

			grdView.OptionsSelection.MultiSelect = True	' Me.Field2Select = "Einsatz-Nr."
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
					col.Caption = m_xml.GetSafeTranslationValue(col.GetCaption)

				Catch ex As Exception
					col.Visible = False

				End Try
				i += 1
			Next col
			'If Me.Field2Select <> "Einsatz-Nr." Then
			'  grdView.Columns("Einsatz-Nr.").Visible = False
			'End If
			'FilterControl1.FilterColumns.Add(New UnboundFilterColumn("MA Name", "MA Name", GetType(String), New RepositoryItemTextEdit(), FilterColumnClauseClass.String))

			grdGrid.Visible = True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

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