
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports System.IO
Imports System.Data.SqlClient

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities



Public Class frmSearchRec

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private strValueSeprator As String = "#@"

	Private bAllowedtowrite As Boolean
	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	Private Property SelectedValue As String
	Private Property Field2Select As String

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As SearchCriteria


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _search As SearchCriteria)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_SearchCriteria = _search
		Field2Select = "MANr"

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

	End Sub

#End Region


	Public ReadOnly Property iListeValue() As String

		Get
			Return SelectedValue

		End Get

	End Property



	Private Sub frmSearchRec_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

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
			Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(cmdClose.Text)
			Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)


		Catch ex As Exception

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
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
				m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("FormStyle: {0}", ex.ToString))

		End Try

		Try
			Dim dt As New DataTable
			dt = GetMADbData4QSTSearch()

			'Select Case Me.Field2Select.ToUpper
			'	Case "MA-Nr.".ToUpper
			'		dt = GetMADbData4QSTSearch()

			'	Case Else
			'		Exit Sub

			'End Select
			grdGrid.DataSource = dt
			grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
			grdGrid.Name = "grd_ESTemp"

			grdGrid.ForceInitialize()
			grdGrid.Visible = False
			Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			grdGrid.Dock = DockStyle.Fill
			AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

			grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
			grdView.ShowFindPanel()
			grdView.OptionsView.ShowIndicator = False
			grdView.OptionsView.ShowAutoFilterRow = True
			grdView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			grdView.OptionsSelection.MultiSelect = True 'Me.Field2Select = "MA-Nr."
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
					col.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains

				Catch ex As Exception
					col.Visible = False

				End Try
				i += 1
			Next col

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
					strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(Me.Field2Select).ToString

				End If
			Next i
			SelectedValue = strValue

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

#Region "Datenbankabfragen für SearchRec..."


	Function GetMADbData4QSTSearch() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		'Dim strQuery As String = "[List MAData For Search Year LOLKonti]"

		Dim sql As String
		sql = "Select MANr, MAName "
		sql &= "From LO Where MDNr = @MDNr "
		sql &= "And Jahr = @Year "
		sql &= "And (@month = 0 Or LO.LP = @month) "
		sql &= "Group By MANr, MAName Order By MAName"

		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.Text

		Dim objAdapter As New SqlDataAdapter
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("MDNr", m_InitializationData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("Year", m_Utility.ReplaceMissing(m_SearchCriteria.FromYear, Now.Year))
		param = cmd.Parameters.AddWithValue("month", m_Utility.ReplaceMissing(m_SearchCriteria.FromMonth, 0))

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "MAData")

		Return ds.Tables(0)
	End Function

#End Region


End Class
