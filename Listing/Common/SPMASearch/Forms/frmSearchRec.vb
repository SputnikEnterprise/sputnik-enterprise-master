
Imports System.IO
Imports System.Data.SqlClient

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.CommonSettings
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns

Imports SPMASearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private strValueSeprator As String = "#@"

	Private m_utility As Utilities
	Private m_mandant As Mandant

	'Private grdGrid As New DevExpress.XtraGrid.GridControl
	'Private grdView As New DevExpress.XtraGrid.Views.Grid.GridView
	Private m_Field2Select As String
	Private Property SelectedValue As String


#Region "Constructor"

	Public Sub New(ByVal _field As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_utility = New Utilities
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

			'grdGrid.ForceInitialize()
			'grdGrid.Visible = False
			'Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			'grdGrid.Dock = DockStyle.Fill
			'grdView.ShowFindPanel()

			'grdView.OptionsSelection.MultiSelect = True
			'grdView.OptionsBehavior.Editable = False
			'grdView.OptionsSelection.EnableAppearanceFocusedCell = False
			'grdView.OptionsSelection.InvertSelection = False
			'grdView.OptionsSelection.EnableAppearanceFocusedRow = True
			'grdView.OptionsView.ShowAutoFilterRow = True
			'grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			'grdView.OptionsView.ShowGroupPanel = False
			'grdGrid.ForceInitialize()

			AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		TranslateControls()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

	End Sub

#End Region


	Public ReadOnly Property iMAValue() As String
		Get
			Return SelectedValue
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
			'grdGrid.ForceInitialize()
			'grdGrid.Visible = False
			'Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			'grdGrid.Dock = DockStyle.Fill
			'grdView.ShowFindPanel()

			'grdView.OptionsSelection.MultiSelect = True
			'grdView.OptionsBehavior.Editable = False
			'grdView.OptionsSelection.EnableAppearanceFocusedCell = False
			'grdView.OptionsSelection.InvertSelection = False
			'grdView.OptionsSelection.EnableAppearanceFocusedRow = True
			'grdView.OptionsView.ShowAutoFilterRow = True
			'grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			'grdView.OptionsView.ShowGroupPanel = False
			'grdGrid.ForceInitialize()

			Dim dt As New DataTable
			Select Case m_Field2Select.ToUpper
				Case "MANr".ToUpper, "Nachname".ToUpper, "Vorname".ToUpper
					dt = LoadEmployeeNameData()

				Case "MABerufeCode".ToUpper
					ResetGridQualificationData()


					'dt = LoadEmployeeJobData()
					m_Field2Select = "MABerufeCode"

				Case "QSTGemeinde".ToUpper
					dt = LoadEmployeeTaxGemeindenData()
					m_Field2Select = "Bezeichnung"

				Case "MABeurteilung".ToUpper
					dt = LoadEmployeeAssessmentData()
					m_Field2Select = "Bezeichnung"

				Case "MAMSprachen".ToUpper
					dt = LoadEmployeeMLanguageData()
					m_Field2Select = "Bezeichnung"

				Case "MASSprachen".ToUpper
					dt = LoadEmployeeSLanguageData()
					m_Field2Select = "Bezeichnung"

				Case "MAAnstellArt".ToUpper
					dt = LoadEmployeeEmployementTypeData()
					m_Field2Select = "Bezeichnung"

				Case "MAKommArt".ToUpper
					dt = LoadEmployeeCommunicationTypeData()
					m_Field2Select = "Bezeichnung"

				Case "MASonstQual".ToUpper
					dt = LoadEmployeeSJobData()
					m_Field2Select = "Bezeichnung"

				Case "MABranchen".ToUpper
					dt = LoadEmployeeSectorData()
					m_Field2Select = "Bezeichnung"


				Case Else
					Return

			End Select
			'grdGrid.DataSource = dt
			'grdGrid.MainView = grdGrid.CreateView("view_MAtemp")
			'grdGrid.Name = "grd_ESTemp"

			'grdGrid.ForceInitialize()
			'grdGrid.Visible = False
			'Me.PanelControl1.Controls.AddRange(New Control() {grdGrid})
			'grdGrid.Dock = DockStyle.Fill
			'AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

			'grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
			'grdView.ShowFindPanel()

			'grdView.OptionsSelection.MultiSelect = True
			'grdView.OptionsBehavior.Editable = False
			'grdView.OptionsSelection.EnableAppearanceFocusedCell = False
			'grdView.OptionsSelection.InvertSelection = False
			'grdView.OptionsSelection.EnableAppearanceFocusedRow = True
			'grdView.OptionsView.ShowAutoFilterRow = True
			'grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			'grdView.OptionsView.ShowGroupPanel = False
			'grdGrid.ForceInitialize()


			If m_Field2Select.ToUpper = "MABerufeCode".ToUpper Then
				grdGrid.DataSource = LoadEmployeeJobData()
				'ResetGridQualificationData()
			Else

				grdView.OptionsSelection.MultiSelect = True
				Dim i As Integer = 0
				For Each col As GridColumn In grdView.Columns
					col.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains

					Dim caption As String = col.FieldName
					Trace.WriteLine(String.Format("{0}", col.FieldName))
					col.MinWidth = 0
					Try
						col.Visible = True
						If caption.ToUpper = "MANr".ToUpper Then
							caption = "Kandidaten-Nr."
						End If
						caption = m_Translate.GetSafeTranslationValue(caption)
						col.Caption = caption

					Catch ex As Exception
						col.Visible = False

					End Try
					i += 1
				Next col
				grdGrid.DataSource = dt
			End If
			'grdGrid.Visible = True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub ResetGridQualificationData()

		grdView.OptionsSelection.MultiSelect = True
		grdView.OptionsView.ShowAutoFilterRow = True
		grdView.OptionsView.ShowIndicator = False
		grdView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		grdView.Columns.Clear()

		Dim columnProfessionCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnProfessionCode.Caption = m_Translate.GetSafeTranslationValue("Berufscode")
		columnProfessionCode.Name = "ProfessionCode"
		columnProfessionCode.FieldName = "ProfessionCode"
		columnProfessionCode.Visible = False
		grdView.Columns.Add(columnProfessionCode)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_Translate.GetSafeTranslationValue("Berufsbezeichnung")
		columnEmployee.Name = "ProfessionText"
		columnEmployee.FieldName = "ProfessionText"
		columnEmployee.BestFit()
		columnEmployee.Visible = True
		grdView.Columns.Add(columnEmployee)


		grdGrid.DataSource = Nothing

	End Sub

	Private Sub ViewKD_RowClick(sender As Object, e As System.EventArgs)
		Dim strValue As String = String.Empty
		Dim strTbleName As String = String.Empty
		Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

		grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		Try

			If m_Field2Select.ToUpper = "MABerufeCode".ToUpper Then

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()
					If Not selectedRows Is Nothing AndAlso selectedRows.Count > 0 Then
						For i As Integer = 0 To selectedRows.Count - 1
							Dim employee = CType(grdView.GetRow(selectedRows(i)), EmployeeAssignedProfessionData)
							strValue &= If(strValue = "", "", strValueSeprator) & employee.ProfessionTextCode
						Next
					End If

				End If

			Else

				For i As Integer = 0 To grdView.SelectedRowsCount - 1
					Dim row As Integer = (grdView.GetSelectedRows()(i))
					If (grdView.GetSelectedRows()(i) >= 0) Then
						Dim dtr As DataRow
						dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
						strValue &= If(strValue = "", "", strValueSeprator) & dtr.Item(m_Field2Select).ToString

					End If
				Next i
			End If

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


#Region "database query"

	Private Function LoadEmployeeNameData() As DataTable
		Dim sql = "[List EmployeeData For EmployeeListing]"
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

	'Private Function LoadEmployeeJobData() As DataTable
	'	Dim sql = "[List EmployeeJob For Search in EmployeeListing]"
	'	Dim ds As New DataSet
	'	Dim dt As New DataTable
	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sql, Conn)
	'	cmd.CommandType = CommandType.StoredProcedure

	'	Dim objAdapter As New SqlDataAdapter

	'	objAdapter.SelectCommand = cmd
	'	objAdapter.Fill(ds, "Data")

	'	Return ds.Tables(0)

	'End Function

	Function LoadEmployeeJobData() As IEnumerable(Of EmployeeAssignedProfessionData)
		Dim result As List(Of EmployeeAssignedProfessionData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = "[List EmployeeJob For Search in EmployeeListing]"

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of EmployeeAssignedProfessionData)

				While reader.Read()
					Dim overviewData As New EmployeeAssignedProfessionData

					overviewData.ProfessionCode = m_utility.SafeGetInteger(reader, "BerufCode", 0)
					overviewData.ProfessionText = m_utility.SafeGetString(reader, "Bezeichnung")


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function





	Private Function LoadEmployeeTaxGemeindenData() As DataTable
		Dim sql = "[Get EmployeeTaxGemeinden For Search In EmployeeListing]"
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

	Private Function LoadEmployeeAssessmentData() As DataTable
		Dim sql = "[Get EmployeeAssessment For Search In EmployeeListing]"
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

	Private Function LoadEmployeeMLanguageData() As DataTable
		Dim sql = "[Show EmployeeMLanguage For Search In EmployeeListing]"
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

	Private Function LoadEmployeeSLanguageData() As DataTable
		Dim sql = "[Show EmployeeSLanguage For Search In EmployeeListing]"
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

	Private Function LoadEmployeeEmployementTypeData() As DataTable
		Dim sql = "[Show EmployeeEmployementTypeData For Search In EmployeeListing]"
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

	Private Function LoadEmployeeCommunicationTypeData() As DataTable
		Dim sql = "[Show EmployeeCommunicationTypeData For Search In EmployeeListing]"
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

	Private Function LoadEmployeeSJobData() As DataTable
		Dim sql = "[List EmployeeSJob For Search in EmployeeListing]"
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

	Private Function LoadEmployeeSectorData() As DataTable
		Dim sql = "[Show EmployeeSectorData For Search In EmployeeListing]"
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

#End Region



End Class

