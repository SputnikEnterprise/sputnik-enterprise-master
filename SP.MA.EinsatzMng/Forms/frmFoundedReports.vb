
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
''' Shows founded Reports records..
''' </summary>
Public Class frmFoundedReports

#Region "Private Fields"

  ''' <summary>
  ''' The Initialization data.
  ''' </summary>
  Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  Private m_ESDatabaseAccess As IESDatabaseAccess

  Private m_ESNumber As Integer?

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
                 ByVal esnumber As Integer)

    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    m_InitializationData = _setting
    m_Translate = translate
    m_SettingsManager = New SettingsManager

    m_ESNumber = esnumber

    TranslateControls()
    ResetReportGrid()

    AddHandler gvReport.RowCellClick, AddressOf OngvReport_RowCellClick

  End Sub


#End Region


#Region "Private Properties"

  ''' <summary>
  ''' Gets the selected Report data.
  ''' </summary>
  ''' <returns>The selected document or nothing if none is selected.</returns>
  Private ReadOnly Property SelectedReportsViewData As FoundedReports
    Get
      Dim grdView = TryCast(grdReport.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      If Not (grdView Is Nothing) Then

        Dim selectedRows = grdView.GetSelectedRows()

        If (selectedRows.Count > 0) Then
          Dim report = CType(grdView.GetRow(selectedRows(0)), FoundedReports)
          Return report
        End If

      End If

      Return Nothing
    End Get

  End Property


#End Region

#Region "Private Methods"


  ''' <summary>
  '''  Trannslate controls.
  ''' </summary>
  Private Sub TranslateControls()

    Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

    Me.lblInfo.Text = m_Translate.GetSafeTranslationValue(Me.lblInfo.Text)
    Me.bsiLblRecCount.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRecCount.Caption)

  End Sub

  ''' <summary>
  ''' reset report grid
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub ResetReportGrid()

    gvReport.OptionsView.ShowIndicator = False
    gvReport.OptionsView.ShowAutoFilterRow = True
    gvReport.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
    gvReport.Columns.Clear()

    Dim columnRPLNr As New DevExpress.XtraGrid.Columns.GridColumn()
    columnRPLNr.Caption = m_Translate.GetSafeTranslationValue("Rapport-Nr.")
    columnRPLNr.Name = "RPNr"
    columnRPLNr.FieldName = "RPNr"
    columnRPLNr.Visible = True
    columnRPLNr.Width = 35
    gvReport.Columns.Add(columnRPLNr)

    Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
    columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnEmployee.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
    columnEmployee.Name = "employeename"
    columnEmployee.FieldName = "employeename"
    columnEmployee.Visible = True
    gvReport.Columns.Add(columnEmployee)

    Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
    columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnCustomer.Caption = m_Translate.GetSafeTranslationValue("Kunde")
    columnCustomer.Name = "customername"
    columnCustomer.FieldName = "customername"
    columnCustomer.Visible = True
    gvReport.Columns.Add(columnCustomer)

    Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
    columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
    columnESAls.Caption = m_Translate.GetSafeTranslationValue("Einsatz als")
    columnESAls.Name = "esals"
    columnESAls.FieldName = "esals"
    columnESAls.Visible = True
    gvReport.Columns.Add(columnESAls)


    Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
    columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
    columnMonat.Name = "rpmonth"
    columnMonat.FieldName = "rpmonth"
    columnMonat.Visible = True
    gvReport.Columns.Add(columnMonat)

    Dim columnJahr As New DevExpress.XtraGrid.Columns.GridColumn()
    columnJahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
    columnJahr.Name = "rpyear"
    columnJahr.FieldName = "rpyear"
    columnJahr.Visible = True
    gvReport.Columns.Add(columnJahr)

    Dim columnFinished As New DevExpress.XtraGrid.Columns.GridColumn()
    columnFinished.Caption = m_Translate.GetSafeTranslationValue("Erfasst")
    columnFinished.Name = "isfinished"
    columnFinished.FieldName = "isfinished"
    columnFinished.Visible = True
    gvReport.Columns.Add(columnFinished)


  End Sub

  Private Sub frmFoundedReports_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
    SaveFromSettings()
  End Sub

  ''' <summary>
  ''' Loads form settings if form gets visible.
  ''' </summary>
  Private Sub OnFrmFoundedReports_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

    If Visible Then
      LoadFormSettings()
    End If

  End Sub

  ''' <summary>
  ''' Loads form settings.
  ''' </summary>
  Private Sub LoadFormSettings()

    Try
      Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_REPORT_FORM_HEIGHT)
      Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_REPORT_FORM_WIDTH)
      Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_REPORT_FORM_LOCATION)

      If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
      If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

      If Not String.IsNullOrEmpty(setting_form_location) Then
        Dim aLoc As String() = setting_form_location.Split(CChar(";"))
        If Screen.AllScreens.Length = 1 Then
          If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
        End If
        Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())

    End Try

  End Sub

  ''' <summary>
  ''' Saves the form settings.
  ''' </summary>
  Private Sub SaveFromSettings()

    ' Save form location, width and height in setttings
    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        m_SettingsManager.WriteString(SettingKeys.SETTING_REPORT_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_REPORT_FORM_WIDTH, Me.Width)
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_REPORT_FORM_HEIGHT, Me.Height)

        m_SettingsManager.SaveSettings()
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())

    End Try

  End Sub


  Private Sub OnChkJustThisES_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkJustThisES.CheckedChanged

    If chkJustThisES.CheckState = CheckState.Unchecked Then
      LoadFoundedCustomerList(m_ESDatabaseAccess, Nothing, Nothing, Nothing)
    Else
      LoadFoundedCustomerList(m_ESDatabaseAccess, m_ESNumber, Nothing, Nothing)
    End If

  End Sub

  ''' <summary>
  ''' Handles focus change of report row.
  ''' </summary>
  Private Sub OpenSelectedReport(ByVal reportnumber As Integer)

    Dim selectedDocument = SelectedReportsViewData

    If Not selectedDocument Is Nothing Then

			Dim hub = MessageService.Instance.Hub
			Dim openreportMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, reportnumber)
			hub.Publish(openreportMng)


			'Dim frmReportMng As New SP.MA.ReportMng.UI.frmReportMng(m_InitializationData)
			'frmReportMng.LoadReportData(reportnumber)
			'frmReportMng.Show()
			'frmReportMng.BringToFront()


    End If

  End Sub

  Private Sub OpenSelectedEmployee(ByVal employeenumber As Integer)

    Dim selectedDocument = SelectedReportsViewData

    If Not selectedDocument Is Nothing Then

      Dim hub = MessageService.Instance.Hub
      Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeenumber)
      hub.Publish(openEmployeeMng)

    End If

  End Sub

  Private Sub OpenSelectedCustomer(ByVal customernumber As Integer)

    Dim selectedDocument = SelectedReportsViewData

    If Not selectedDocument Is Nothing Then

      Dim hub = MessageService.Instance.Hub
      Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customernumber)
      hub.Publish(openCustomerMng)

    End If

  End Sub

  Sub OngvReport_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

    If (e.Clicks = 2) Then

      Dim column = e.Column
      Dim dataRow = gvReport.GetRow(e.RowHandle)
      If Not dataRow Is Nothing Then
        Dim viewData = CType(dataRow, FoundedReports)

        Select Case column.Name.ToLower
          Case "employeename"
            If viewData.MANr > 0 Then OpenSelectedEmployee(viewData.MANr)

          Case "customername"
            If viewData.KDNr > 0 Then OpenSelectedCustomer(viewData.KDNr)


          Case Else
            If viewData.RPNr > 0 Then
              OpenSelectedReport(viewData.RPNr)

            End If

        End Select

      End If

    End If

  End Sub


#End Region



#Region "Public Methods"

  ''' <summary>
  ''' Loads Reports for selected ES
  ''' </summary>
  ''' <param name="esnumber"></param>
  ''' <param name="employeenumber"></param>
  ''' <param name="customernumber"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function LoadFoundedCustomerList(ByVal ESDatabaseAccess As IESDatabaseAccess, ByVal esnumber As Integer?, ByVal employeenumber As Integer?, ByVal customernumber As Integer?) As Boolean

    m_ESDatabaseAccess = ESDatabaseAccess

		If esnumber.HasValue Then m_ESNumber = esnumber
    Dim listOfReports = m_ESDatabaseAccess.LoadFoundedRPInESMng(esnumber, employeenumber, customernumber)

    Dim reportGridData = (From report In listOfReports
    Select New FoundedReports With
           {.RPNr = report.RPNr,
            .MANr = report.MANr,
            .KDNr = report.KDNr,
            .employeename = report.employeename,
            .customername = report.customername,
            .esals = report.esals,
            .rpmonth = report.rpmonth,
            .rpyear = report.rpyear,
            .isfinished = report.isfinished
           }).ToList()

    Dim listDataSource As BindingList(Of FoundedReports) = New BindingList(Of FoundedReports)

    For Each p In reportGridData
      listDataSource.Add(p)
    Next

    grdReport.DataSource = listDataSource
    bsiRecCount.Caption = listOfReports.Count

    Return Not listOfReports Is Nothing
  End Function

#End Region


End Class
