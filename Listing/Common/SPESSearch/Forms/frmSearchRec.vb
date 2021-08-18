
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPESSearch.ClsDataDetail
Imports SP.Infrastructure.Logging


Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	'Private m_xml As New ClsXML

	Private m_md As Mandant
	Private _ClsData As New ClsDivFunc
	Private bAllowedtowrite As Boolean
	Private strValueSeprator As String = "#@"

	Public Property MetroForeColor As System.Drawing.Color
	Public Property MetroBorderColor As System.Drawing.Color

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	Private Property Field2Select As String


#Region "Constructor"

	Public Sub New(ByVal _field2Select As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()
		m_md = New Mandant

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.Field2Select = _field2Select
		ClsDataDetail.strButtonValue = String.Empty

		TranslateControls()

	End Sub


#End Region


	Public ReadOnly Property iESValue(ByVal strValue As String) As String

		Get
			Dim strBez As String = String.Empty

			strBez = _ClsData.GetSelektion

			ClsDataDetail.strESData = strBez

			Return ClsDataDetail.strESData
		End Get

	End Property

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblHeaderfett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderfett.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(Me.cmdOK.Text)

	End Sub

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

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
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
			Dim dt As New DataTable
			Select Case Me.Field2Select.ToUpper
				Case "ES-Nr.".ToUpper
					dt = GetESDbData4ESSearch()

				Case "MA-Nr.".ToUpper
					dt = GetESMADbData4ESSearch()

				Case "KD-Nr.".ToUpper
					dt = GetESKDDbData4ESSearch()

				Case "Einsatz-als".ToUpper
					dt = GetESAlsDbData4ESSearch()

				Case "Einsatz-Branche".ToUpper
					dt = GetESBrancheDbData4ESSearch()

				Case "GAV-Berufe".ToUpper
					dt = GetESGAVBerufDbData4ESSearch()

				Case "1. KD-Kredit".ToUpper
					dt = GetKDKredit_1DbData4ESSearch()

				Case "2. KD-Kredit".ToUpper
					dt = GetKDKredit_2DbData4ESSearch()


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
			grdView.ShowFindPanel()

			grdView.OptionsSelection.MultiSelect = True	'Me.Field2Select = "MA-Nr."
			grdView.OptionsBehavior.Editable = False
			grdView.OptionsSelection.EnableAppearanceFocusedCell = False
			grdView.OptionsSelection.InvertSelection = False
			grdView.OptionsSelection.EnableAppearanceFocusedRow = True
			grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			grdView.OptionsView.ShowAutoFilterRow = True
			grdView.OptionsView.ShowGroupPanel = False
			grdGrid.ForceInitialize()

			Dim i As Integer = 0
			For Each col As GridColumn In grdView.Columns
				col.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains

				Dim caption As String = col.FieldName
				Trace.WriteLine(String.Format("{0}", col.FieldName))
				col.MinWidth = 0
				Try
					If caption.ToUpper = "ESNr".ToUpper Or caption.ToUpper = "ES-Nr.".ToUpper Then
						caption = "Einsatz-Nr."
					End If
					col.Visible = True
					caption = m_Translate.GetSafeTranslationValue(caption)
					col.Caption = caption

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
					strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(Me.Field2Select).ToString

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
