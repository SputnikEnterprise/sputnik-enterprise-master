
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns

Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPBruttolohnjournal.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmSearchRec

	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsData As New ClsDivFunc
	Private strValueSeprator As String = "#@"

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	Private m_SearchCriteria As SearchCriteria
	Private Property m_modulName As String

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI


#Region "Constructor"

	Public Sub New(ByVal _SearchCriteria As SearchCriteria, ByVal _modulname As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.m_SearchCriteria = _SearchCriteria
		m_modulName = _modulname

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI


	End Sub

#End Region


	Public ReadOnly Property iListValue() As String

		Get
			Dim strBez As String = String.Empty

			strBez = _ClsData.GetSelektion
			ClsDataDetail.strBruttolohnjournalData = strBez

			Return _ClsData.GetSelektion
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
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
			Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)

		Catch ex As Exception

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
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
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Try
			Dim dt As New DataTable
			Select Case m_modulName.ToUpper
				Case "madata".ToUpper
					dt = GetMADbData4BLJSearch()

				Case "BranchenData".ToUpper
					dt = GetMADbData4LOLBrancheSearch()

				Case Else
					Exit Sub

			End Select
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
					col.Caption = _ClsProgSetting.TranslateText(col.GetCaption)
					col.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains

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
					Dim dtr As DataRow
					dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))

					If m_modulName.ToUpper = "madata".ToUpper Then
						strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item("MANr").ToString
					Else
						strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item("Branche").ToString
					End If

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

#Region "Datenbankabfragen für SearchRec..."


	Function GetMADbData4BLJSearch() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = "[List MAData For Search In LOListing]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@MonthFrom", m_SearchCriteria.vonmonat)
		param = cmd.Parameters.AddWithValue("@MonthTo", m_SearchCriteria.bismonat)
		param = cmd.Parameters.AddWithValue("@YearFrom", m_SearchCriteria.vonjahr)
		param = cmd.Parameters.AddWithValue("@YearTo", m_SearchCriteria.bisjahr)

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "MAData")

		Return ds.Tables(0)
	End Function

	Function GetMADbData4LOLBrancheSearch() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = "[GetESBranche For Search IN LOListing]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@Month", m_SearchCriteria.vonmonat)
		param = cmd.Parameters.AddWithValue("@Year", m_SearchCriteria.vonjahr)

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "BranchenData")

		Return ds.Tables(0)
	End Function

#End Region




End Class




