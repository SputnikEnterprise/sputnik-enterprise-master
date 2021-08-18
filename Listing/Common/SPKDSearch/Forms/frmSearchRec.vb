
Imports System.IO
Imports System.Data.SqlClient

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.CommonSettings
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns

Imports SPKDSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private strValueSeprator As String = "#@"

	Private bAllowedtowrite As Boolean

	Private m_mandant As Mandant

	Private grdGrid As New DevExpress.XtraGrid.GridControl
	Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView
	Dim m_Field2Select As String
	Private Property SelectedValue As String

#Region "Constructor"

	Public Sub New(ByVal _field As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_mandant = New Mandant
		SelectedValue = String.Empty
		m_Field2Select = _field

		InitializeComponent()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
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

		Try
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
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			Dim dt As New DataTable
			Select Case m_Field2Select.ToUpper
				Case "KDNr".ToUpper, "Firma1".ToUpper
					dt = LoadCustomerNameData()

				Case "KDJob".ToUpper
					dt = LoadCustomerJobData()
					m_Field2Select = "Bezeichnung"

				Case "KDBranches".ToUpper
					dt = LoadCustomerBranchesData()
					m_Field2Select = "Bezeichnung"

				Case "KDGAV".ToUpper
					dt = LoadCustomerGAVData()
					m_Field2Select = "Bezeichnung"

				Case "KDAnstellung".ToUpper
					dt = LoadCustomerAnstellungData()
					m_Field2Select = "Bezeichnung"

				Case "KDStichwort".ToUpper
					dt = LoadCustomerStichwortData()
					m_Field2Select = "Bezeichnung"

				Case "KDFiliale".ToUpper
					dt = LoadCustomerFilialeData()
					m_Field2Select = "Bezeichnung"


				Case "ZHDVersand".ToUpper
					dt = LoadCResponsibleVersandData()
					m_Field2Select = "Bezeichnung"

				Case "ZHDKomm".ToUpper
					dt = LoadCResponsibleKommunikationData()
					m_Field2Select = "Bezeichnung"

				Case "ZHDJob".ToUpper
					dt = LoadCResponsibleJobData()
					m_Field2Select = "Bezeichnung"

				Case "ZHDBranches".ToUpper
					dt = LoadCResponsibleBranchesData()
					m_Field2Select = "Bezeichnung"

				Case "ZHDRes1".ToUpper
					dt = LoadCResponsibleRes1Data()
					m_Field2Select = "Bezeichnung"
				Case "ZHDRes2".ToUpper
					dt = LoadCResponsibleRes2Data()
					m_Field2Select = "Bezeichnung"
				Case "ZHDRes3".ToUpper
					dt = LoadCResponsibleRes3Data()
					m_Field2Select = "Bezeichnung"
				Case "ZHDRes4".ToUpper
					dt = LoadCResponsibleRes4Data()
					m_Field2Select = "Bezeichnung"

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
					strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(m_Field2Select).ToString

				End If
			Next i
			'_ClsData.GetSelektion = strValue
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
		Me.Dispose()
	End Sub


	Private Function LoadCustomerNameData() As DataTable
		Dim sql = "[List KDNameData For Search In KDSearch]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerNumberData() As DataTable
		Dim sql = "[List KDNameData For Search In KDSearch]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerJobData() As DataTable
		Dim sql = "[Get KDBerufe From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerBranchesData() As DataTable
		Dim sql = "[Get KDBranche From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerGAVData() As DataTable
		Dim sql = "[Get KDGAV From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerAnstellungData() As DataTable
		Dim sql = "[Get KDAnstellung From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerStichwortData() As DataTable
		Dim sql = "[Get KDStichwort From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCustomerFilialeData() As DataTable
		Dim sql = "[Get KDFiliale From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function




	Private Function LoadCResponsibleVersandData() As DataTable
		Dim sql = "[Get KDZKontaktArt From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleKommunikationData() As DataTable
		Dim sql = "[Get KDZKom From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleJobData() As DataTable
		Dim sql = "[Get KDZBerufe From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleBranchesData() As DataTable
		Dim sql = "[Get KDZBranche From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleRes1Data() As DataTable
		Dim sql = "[Get KDZ1Res From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleRes2Data() As DataTable
		Dim sql = "[Get KDZ2Res From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleRes3Data() As DataTable
		Dim sql = "[Get KDZ3Res From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function

	Private Function LoadCResponsibleRes4Data() As DataTable
		Dim sql = "[Get KDZ4Res From KDDataBase]"
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "Data")

		Return ds.Tables(0)

	End Function


End Class
