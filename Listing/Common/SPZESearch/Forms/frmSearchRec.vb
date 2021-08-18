
Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging

Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_Mandant As Mandant
	Private _ClsData As New ClsDivFunc

	Private strValueSeprator As String = "#@"

	Public Property MetroForeColor As System.Drawing.Color
	Public Property MetroBorderColor As System.Drawing.Color

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	Private Property Field2Select As String

	Public ReadOnly Property iESValue(ByVal strValue As String) As String

		Get
			Dim strBez As String = String.Empty

			'If Me.LvData.SelectedItems.Count > 0 Then
			strBez = _ClsData.GetSelektion
			'End If

			ClsDataDetail.strESData = strBez

			Return ClsDataDetail.strESData
		End Get

	End Property

#Region "Constructor"

	Public Sub New()

		m_InitializationData = ClsDataDetail.m_InitialData
		m_Translate = ClsDataDetail.m_Translate

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_Mandant = New Mandant

	End Sub

	Public Sub New(ByVal _field2Select As String)

		m_InitializationData = ClsDataDetail.m_InitialData
		m_Translate = ClsDataDetail.m_Translate

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()
		m_Mandant = New Mandant

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.Field2Select = _field2Select
		ClsDataDetail.strButtonValue = String.Empty

	End Sub

#End Region


	Private Sub frmSearchRec_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation_LVESSearch = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iLVESSearch_Width = Me.Width
				My.Settings.iLVESSearch_Height = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub



	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
		cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)

		Try
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				If My.Settings.frmLocation_LVESSearch <> String.Empty Then
					Me.Width = My.Settings.iLVESSearch_Width
					Me.Height = My.Settings.iLVESSearch_Height
					Dim aLoc As String() = My.Settings.frmLocation_LVESSearch.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
					End If
					Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Try
			Dim dt As SqlDataReader
			Select Case Me.Field2Select.ToUpper
				Case "zenr".ToUpper
					dt = GetZEDbData4ZESearch()

				Case "renr".ToUpper
					dt = GetZEREDbData4ZESearch()

				Case "kdnr".ToUpper
					dt = GetZEKDDbData4ZESearch()

				Case Else
					dt = GetZEKDDbData4ZESearch()


			End Select
			grdGrid.DataSource = dt
			grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
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
			'grdView.OptionsView.ShowAutoFilterRow = True

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
					'Dim dtr As DataRow
					'					dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))

					strValue &= If(strValue = "", "", strValueSeprator) & grdView.GetRowCellValue(row, Me.Field2Select).ToString



					'If Not dtr Is Nothing Then
					'	'strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(Me.Field2Select).ToString
					'End If

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
		'_ClsData.GetSelektion = String.Empty
		Me.Close()
		Me.Dispose()
	End Sub



End Class

