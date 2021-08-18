
Imports System.IO
Imports System.Data.SqlClient
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions

Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Columns
Imports SP.Infrastructure.Logging

Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_Seprator As String = "#@"
	Private m_mandant As Mandant

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView
	Private Property SelectedValue As String
	Dim m_Field2Select As String



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _field As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_mandant = New Mandant
		SelectedValue = String.Empty
		m_Field2Select = _field

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			If My.Settings.frmLocation_SearchRec <> String.Empty Then
				Me.Width = My.Settings.iSearchRec_Width
				Me.Height = My.Settings.iSearchRec_Height
				Dim aLoc As String() = My.Settings.frmLocation_SearchRec.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		TranslateControls()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

	End Sub

#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblHeaderfett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderfett.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(Me.cmdOK.Text)

	End Sub

	Public ReadOnly Property iKDValue() As String
		Get
			Return SelectedValue
		End Get
	End Property

	'Public ReadOnly Property iKDValue(ByVal strValue As String) As String

	'  Get
	'    Dim strBez As String = String.Empty

	'    If Me.LvData.SelectedItems.Count > 0 Then

	'      If UCase(strValue) = UCase("OPNr") Then
	'        strBez = _ClsData.GetOPNr

	'      ElseIf UCase(strValue) = UCase("KDNr") Then
	'        strBez = _ClsData.GetKDNr

	'      Else
	'        strBez = _ClsData.GetOPNr

	'      End If

	'      Select Case UCase(strValue)
	'        Case UCase("OPNr")
	'          Me.LblChanged.Text = strBez                   ' KDNr

	'        Case UCase("KDNr")
	'          Me.LblChanged.Text = strBez


	'      End Select
	'    End If

	'    ClsDataDetail.strKDData = CStr(Me.LblChanged.Text)

	'    Return ClsDataDetail.strKDData
	'  End Get

	'End Property

	Private Sub frmSearchRec_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation_SearchRec = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iSearchRec_Width = Me.Width
				My.Settings.iSearchRec_Height = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub


	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Dim dt As SqlDataReader
		Select Case m_Field2Select.ToUpper
			Case "invoice".ToUpper
				dt = LoadInvoiceData()
				m_Field2Select = "RENr"

			Case "customer".ToUpper
				dt = LoadCustomerData()
				m_Field2Select = "KDNr"

			Case Else
				Return

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

		grdView.OptionsSelection.MultiSelect = True
		grdView.OptionsBehavior.Editable = False
		grdView.OptionsSelection.EnableAppearanceFocusedCell = False
		grdView.OptionsSelection.InvertSelection = False
		grdView.OptionsSelection.EnableAppearanceFocusedRow = True
		grdView.OptionsView.ShowAutoFilterRow = True
		grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		grdView.OptionsView.ShowGroupPanel = False
		grdGrid.ForceInitialize()

		Dim i As Integer = 0
		For Each col As GridColumn In grdView.Columns
			col.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains

			Dim caption As String = col.FieldName
			Trace.WriteLine(String.Format("{0}", col.FieldName))
			col.MinWidth = 0
			Try
				If caption.ToUpper = "KDNr".ToUpper Then
					caption = "Kunden-Nr."
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

					strValue &= If(strValue = "", "", m_Seprator) & grdView.GetRowCellValue(row, m_Field2Select).ToString



					'If Not dtr Is Nothing Then
					'	'strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(Me.Field2Select).ToString
					'End If

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
		SelectedValue = String.Empty

		Me.Close()
		'Me.Dispose()
	End Sub

	Private Function LoadInvoiceData() As SqlDataReader
		Dim sql = "[List InvoiceData For Search in OPListing]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Conn.Open()
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@Filiale", "%" & m_InitializationData.UserData.UserFiliale & "%")

		Dim reader As SqlDataReader = cmd.ExecuteReader

		Return reader

	End Function

	Private Function LoadCustomerData() As SqlDataReader
		Dim sql = "[List CustomerData For Search In OPListing]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Conn.Open()
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@Filiale", "%" & m_InitializationData.UserData.UserFiliale & "%")

		Dim reader As SqlDataReader = cmd.ExecuteReader

		Return reader

	End Function



End Class