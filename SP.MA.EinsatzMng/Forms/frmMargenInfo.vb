
Imports SP.DatabaseAccess
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports SP.Infrastructure
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.MA.EinsatzMng.Settings




''' <summary>
''' Shows margen info records..
''' </summary>
Public Class frmMargenInfo

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_ESDatabaseAccess As IESDatabaseAccess

	Private m_MargenString As String

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass,
								 ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper,
								 ByVal margenString As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting
		m_Translate = translate
		m_SettingsManager = New SettingsManager

		m_MargenString = margenString

		TranslateControls()
		ResetReportGrid()

	End Sub


#End Region


#Region "Private Methods"


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

	End Sub

	''' <summary>
	''' reset report grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetReportGrid()

		gvMargenInfo.OptionsView.ShowIndicator = False
		gvMargenInfo.OptionsView.ShowAutoFilterRow = False
		gvMargenInfo.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvMargenInfo.Columns.Clear()

		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_Translate.GetSafeTranslationValue("Details")
		columnCustomer.Name = "Value"
		columnCustomer.FieldName = "Value"
		columnCustomer.Visible = True
		gvMargenInfo.Columns.Add(columnCustomer)


		grdMargenInfo.DataSource = Nothing

	End Sub

	'Private Sub frmFoundedReports_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
	'	SaveFromSettings()
	'End Sub

	'''' <summary>
	'''' Loads form settings if form gets visible.
	'''' </summary>
	'Private Sub OnFrmFoundedReports_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

	'	If Visible Then
	'		LoadFormSettings()
	'	End If

	'End Sub

	'''' <summary>
	'''' Loads form settings.
	'''' </summary>
	'Private Sub LoadFormSettings()

	'	Try
	'		Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_REPORT_FORM_HEIGHT)
	'		Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_REPORT_FORM_WIDTH)
	'		Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_REPORT_FORM_LOCATION)

	'		If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
	'		If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

	'		If Not String.IsNullOrEmpty(setting_form_location) Then
	'			Dim aLoc As String() = setting_form_location.Split(CChar(";"))
	'			If Screen.AllScreens.Length = 1 Then
	'				If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
	'			End If
	'			Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
	'		End If

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString())

	'	End Try

	'End Sub

	'''' <summary>
	'''' Saves the form settings.
	'''' </summary>
	'Private Sub SaveFromSettings()

	'	' Save form location, width and height in setttings
	'	Try
	'		If Not Me.WindowState = FormWindowState.Minimized Then
	'			m_SettingsManager.WriteString(SettingKeys.SETTING_REPORT_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
	'			m_SettingsManager.WriteInteger(SettingKeys.SETTING_REPORT_FORM_WIDTH, Me.Width)
	'			m_SettingsManager.WriteInteger(SettingKeys.SETTING_REPORT_FORM_HEIGHT, Me.Height)

	'			m_SettingsManager.SaveSettings()
	'		End If

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString())

	'	End Try

	'End Sub


	'Private Sub OnChkJustThisES_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkJustThisES.CheckedChanged

	'	If chkJustThisES.CheckState = CheckState.Unchecked Then
	'		LoadFoundedCustomerList(m_ESDatabaseAccess, Nothing, Nothing, Nothing)
	'	Else
	'		LoadFoundedCustomerList(m_ESDatabaseAccess, m_ESNumber, Nothing, Nothing)
	'	End If

	'End Sub

	'''' <summary>
	'''' Handles focus change of report row.
	'''' </summary>
	'Private Sub OpenSelectedReport(ByVal reportnumber As Integer)

	'	Dim selectedDocument = SelectedReportsViewData

	'	If Not selectedDocument Is Nothing Then

	'		Dim hub = MessageService.Instance.Hub
	'		Dim openreportMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, reportnumber)
	'		hub.Publish(openreportMng)


	'		'Dim frmReportMng As New SP.MA.ReportMng.UI.frmReportMng(m_InitializationData)
	'		'frmReportMng.LoadReportData(reportnumber)
	'		'frmReportMng.Show()
	'		'frmReportMng.BringToFront()


	'	End If

	'End Sub

	'Private Sub OpenSelectedEmployee(ByVal employeenumber As Integer)

	'	Dim selectedDocument = SelectedReportsViewData

	'	If Not selectedDocument Is Nothing Then

	'		Dim hub = MessageService.Instance.Hub
	'		Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeenumber)
	'		hub.Publish(openEmployeeMng)

	'	End If

	'End Sub

	'Private Sub OpenSelectedCustomer(ByVal customernumber As Integer)

	'	Dim selectedDocument = SelectedReportsViewData

	'	If Not selectedDocument Is Nothing Then

	'		Dim hub = MessageService.Instance.Hub
	'		Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customernumber)
	'		hub.Publish(openCustomerMng)

	'	End If

	'End Sub

	'Sub OngvReport_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

	'	If (e.Clicks = 2) Then

	'		Dim column = e.Column
	'		Dim dataRow = gvReport.GetRow(e.RowHandle)
	'		If Not dataRow Is Nothing Then
	'			Dim viewData = CType(dataRow, FoundedReports)

	'			Select Case column.Name.ToLower
	'				Case "employeename"
	'					If viewData.MANr > 0 Then OpenSelectedEmployee(viewData.MANr)

	'				Case "customername"
	'					If viewData.KDNr > 0 Then OpenSelectedCustomer(viewData.KDNr)


	'				Case Else
	'					If viewData.RPNr > 0 Then
	'						OpenSelectedReport(viewData.RPNr)

	'					End If

	'			End Select

	'		End If

	'	End If

	'End Sub


#End Region



#Region "Public Methods"

	''' <summary>
	''' Loads Reports for selected ES
	''' </summary>
	Public Function LoadMargenInfoList() As Boolean

		Dim margendData As New List(Of String)
		margendData = m_MargenString.Split("¦"c).ToList

		'Dim reportGridData = (From report In listOfReports
		'											Select New FoundedReports With
		'			 {.RPNr = report.RPNr,
		'				.MANr = report.MANr,
		'				.KDNr = report.KDNr,
		'				.employeename = report.employeename,
		'				.customername = report.customername,
		'				.esals = report.esals,
		'				.rpmonth = report.rpmonth,
		'				.rpyear = report.rpyear,
		'				.isfinished = report.isfinished
		'			 }).ToList()

		Dim listDataSource As BindingList(Of MargenInfo) = New BindingList(Of MargenInfo)

		For Each p In margendData
			listDataSource.Add(New MargenInfo With {.Value = p})
		Next

		grdMargenInfo.DataSource = listDataSource
		'bsiRecCount.Caption = listOfReports.Count

		Return Not listDataSource Is Nothing
	End Function

#End Region

	Private Class MargenInfo
		Public Property Value As String

	End Class


End Class
