
Imports DevExpress.XtraGrid.Columns
Imports SP.Infrastructure.Logging

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.UI
Imports SPFibuSearch.ClsDataDetail
Imports DevExpress.LookAndFeel

Public Class frmFibuSearch_LV

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private _ClsFunc As New ClsDivFunc
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strLastSortBez As String

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView
	Public Property RecCount As Integer

#Region "Header erstellen..."

	'Sub SetLvwHeader(ByVal frmTest As frmFibuSearch)
	'	Dim strColumnString As String = String.Empty
	'	Dim strColumnWidthInfo As String = String.Empty

	'	strColumnString = "Test;Lohnart;Bezeichnung;Total"
	'	strColumnWidthInfo = "0-1;-2-1;-2-0;-2-1"

	'	strColumnString = TranslateText(strColumnString)
	'	'FillDataHeaderLv(Me.LvFoundedrecs, strColumnString, strColumnWidthInfo)

	'End Sub

#End Region

	Public Sub New(ByVal frmTest As frmFibuSearch, ByVal strQuery As String, ByVal LX As Integer, ByVal LY As Integer, _
								 ByVal lHeight As Integer)

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()
		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		'Dim strForWidth As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Coordination\" & _
		'																									 ClsDataDetail.GetAppGuidValue(), Me.Name & "_0")

		'Me.Width = CInt(Val(strForWidth))
		'Me.Height = lHeight
		'Me.Location = New Point(LX - Me.Width - 5, LY)
		'Trace.WriteLine("LX: " & LX.ToString & vbTab & "LY: " & LY.ToString & "Top: " & Me.Top.ToString & vbTab & "Left: " & Me.Left.ToString)

		'SetLvwHeader(frmTest)
		_ClsFunc.GetSearchQuery = strQuery
		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		' zur(Virtualisierung)
		'Dim Time_1 As Double = System.Environment.TickCount
		'FillLvData(Me.LvFoundedrecs)
		'Trace.WriteLine(String.Format("{0}. Ladezeit für LvFoundedrecs: {1} s.", strMethodeName, ((System.Environment.TickCount - Time_1) / 1000)))

		Try
			'Time_1 = System.Environment.TickCount
			Me.PanelControl1.Dock = DockStyle.Fill
			Dim dt As DataTable = GetFibuData4ShowInGrid(strQuery)
			grdGrid.DataSource = dt
			grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
			grdGrid.Name = "grd_ESTemp"

			grdGrid.ForceInitialize()
			grdGrid.Visible = False
			Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			grdGrid.Dock = DockStyle.Fill
			'AddHandler pcc.SizeChanged, AddressOf pcc_SizeChanged
			'AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

			grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
			'If My.Settings.bgrdView_EnterpriseShowGroup Then _
			'  grdView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways
			'grdView.ShowFindPanel()

			grdView.OptionsSelection.MultiSelect = False 'Me.Field2Select = "MA-Nr."
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
					col.Visible = col.FieldName.ToLower.Contains("LANr".ToLower) Or col.FieldName.ToLower.Contains("Bezeichnung".ToLower) Or col.FieldName.ToLower.Contains("HKonto".ToLower) Or col.FieldName.ToLower.Contains("SKonto".ToLower) Or col.FieldName.ToLower.Contains("Totalbetrag".ToLower)
					col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

				Catch ex As Exception
					col.Visible = False

				End Try
				i += 1
			Next col
			'If Me.Field2Select <> "Kandidat-Nr." Then
			'  grdView.Columns("Kandidat-Nr.").Visible = False
			'End If
			'FilterControl1.FilterColumns.Add(New UnboundFilterColumn("MA Name", "MA Name", GetType(String), New RepositoryItemTextEdit(), FilterColumnClauseClass.String))

			grdGrid.Visible = True
			Me.RecCount = grdView.RowCount

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.RecCount = 0

		End Try


	End Sub

	Private Sub OnForm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, 100)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, 100)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		End Try


	End Sub

End Class