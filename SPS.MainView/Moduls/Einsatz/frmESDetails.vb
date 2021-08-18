
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.UI

Imports DevExpress.XtraGrid.Views.Grid

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Settings
Imports SPS.MainView.ESSettings
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Views.Base
Imports SPS.MainView.DataBaseAccess
Imports System.IO


Public Class frmESDetails
  Private Shared m_Logger As ILogger = New Logger()

  Protected m_SettingsManager As ISettingsManager

  Private _ClsSetting As New ClsESSetting
  Private Property Modul2Open As String

  Private Property MetroForeColor As System.Drawing.Color
  Private Property MetroBorderColor As System.Drawing.Color

  Private m_GVReportSettingfilename As String
  Private m_GVReportSettingfilenameWithCustomer As String


  Private m_translate As TranslateValues
  Private m_UitilityUI As UtilityUI


  Public Sub New(ByVal _setting As ClsESSetting, ByVal _m2Open As String)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me._ClsSetting = _setting
    Me.Modul2Open = _m2Open
    Me._ClsSetting.OpenDetailModul = _m2Open
    m_SettingsManager = New SettingsESManager
    m_translate = New TranslateValues
    m_UitilityUI = New UtilityUI

    Try
      Dim strModulName As String = Me.Modul2Open.ToLower

      m_GVReportSettingfilename = String.Format("{0}Employment\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
      m_GVReportSettingfilenameWithCustomer = String.Format("{0}Employment\Details\{1}_WithEmployment{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())
    End Try


    RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
    Me.chkSelMA.Checked = Me._ClsSetting.Data4SelectedES
    AddHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged

    AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
    AddHandler Me.gvDetail.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
    AddHandler Me.gvDetail.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
    AddHandler Me.gvDetail.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

  End Sub

  Private Sub frmVakDetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        m_SettingsManager.WriteString(SettingESKeys.SETTING_ES_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
        m_SettingsManager.WriteInteger(SettingESKeys.SETTING_ES_FORM_WIDTH, Me.Width)
        m_SettingsManager.WriteInteger(SettingESKeys.SETTING_ES_FORM_HEIGHT, Me.Height)

        m_SettingsManager.SaveSettings()
      End If

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

    End Try

  End Sub

  Private Sub OnForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    OngvColumnPositionChanged(New Object, New System.EventArgs)
  End Sub

  Private Sub frmDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strTitle As String = String.Empty

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingESKeys.SETTING_ES_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingESKeys.SETTING_ES_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingESKeys.SETTING_ES_FORM_LOCATION)

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
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

      End Try

		Me.sccMain.Dock = DockStyle.Fill
    Me.bsiRecCount.Caption = TranslateText("Bereit")

    Me.gvDetail.OptionsView.ShowIndicator = False
    Select Case Me.Modul2Open.ToLower
      Case "RP".ToLower
        strTitle = "Anzeige der Rapporte"

        ResetReportDetailGrid()
        LoadESReportDetailList()

      Case Else

    End Select
    Me.Text = String.Format(TranslateText(strTitle))
		Me.rlblDetailHeader.Text = String.Format("<b>{0}</b>", TranslateText(strTitle))
		Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

  End Sub

  Private Sub chkSelMA_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles chkSelMA.CheckedChanged

    grdDetailrec.BeginUpdate()
    Me.gvDetail.Columns.Clear()
    Me.grdDetailrec.DataSource = Nothing

    Me._ClsSetting.Data4SelectedES = Me.chkSelMA.Checked
    Select Case Me.Modul2Open.ToLower
      Case "rp"
        ResetReportDetailGrid()
        LoadESReportDetailList()

      Case Else

    End Select
    Me.grdDetailrec.EndUpdate()
    Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

  End Sub

  Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

    Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
    Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvDetail.RowCount)
    OngvColumnPositionChanged(sender, e)

  End Sub



#Region "Details for employee Reports"

  Sub ResetReportDetailGrid()

    gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
    gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
    gvDetail.OptionsView.ShowGroupPanel = False
    gvDetail.OptionsView.ShowIndicator = False
    gvDetail.OptionsView.ShowAutoFilterRow = True

    gvDetail.Columns.Clear()

    Try

      Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
      columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
      columnmodulname.Name = "rpnr"
      columnmodulname.FieldName = "rpnr"
      columnmodulname.Visible = True
      gvDetail.Columns.Add(columnmodulname)

      Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnLONr.Caption = m_translate.GetSafeTranslationValue("Lohn-Nr.")
      columnLONr.Name = "lonr"
      columnLONr.FieldName = "lonr"
      columnLONr.Visible = True
      gvDetail.Columns.Add(columnLONr)

      Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
      columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Periode")
      columnBezeichnung.Name = "periode"
      columnBezeichnung.FieldName = "periode"
      columnBezeichnung.Visible = True
      gvDetail.Columns.Add(columnBezeichnung)

      Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMonth.Caption = m_translate.GetSafeTranslationValue("Monat")
      columnMonth.Name = "monat"
      columnMonth.FieldName = "monat"
      columnMonth.Visible = True
      gvDetail.Columns.Add(columnMonth)

      Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
      columnYear.Caption = m_translate.GetSafeTranslationValue("Jahr")
      columnYear.Name = "jahr"
      columnYear.FieldName = "jahr"
      columnYear.Visible = True
      gvDetail.Columns.Add(columnYear)

      Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
      columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
      columncustomername.Name = "customername"
      columncustomername.FieldName = "customername"
      columncustomername.Visible = True
      gvDetail.Columns.Add(columncustomername)

      Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
      columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
      columnEmployeename.Name = "employeename"
      columnEmployeename.FieldName = "employeename"
      columnEmployeename.Visible = True
      gvDetail.Columns.Add(columnEmployeename)

      Dim columnIsDone As New DevExpress.XtraGrid.Columns.GridColumn()
      columnIsDone.Caption = m_translate.GetSafeTranslationValue("Erfasst")
      columnIsDone.Name = "rpdone"
      columnIsDone.FieldName = "rpdone"
      columnIsDone.Visible = True
      gvDetail.Columns.Add(columnIsDone)



      Dim columnFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
      columnFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
      columnFiliale.Name = "zfiliale"
      columnFiliale.FieldName = "zfiliale"
      columnFiliale.Visible = False
      gvDetail.Columns.Add(columnFiliale)

      Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
      columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
      columnCreatedOn.Name = "createdon"
      columnCreatedOn.FieldName = "createdon"
      columnCreatedOn.Visible = False
      columnCreatedOn.BestFit()
      gvDetail.Columns.Add(columnCreatedOn)

      Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
      columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
      columnCreatedFrom.Name = "createdfrom"
      columnCreatedFrom.FieldName = "createdfrom"
      columnCreatedFrom.Visible = False
      columnCreatedFrom.BestFit()
      gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
    End Try

    grdDetailrec.DataSource = Nothing

  End Sub

  Public Function LoadESReportDetailList() As Boolean
    Dim m_DataAccess As New MainGrid
    Dim esNumber As Integer? = Nothing

    If Me.chkSelMA.Checked Then
      esNumber = Me._ClsSetting.SelectedESNr
    End If
    Dim listOfEmployees = m_DataAccess.GetDbEinsatzReportDataForDetails(esNumber)

    If listOfEmployees Is Nothing Then
      m_UitilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
      Return False
    End If

    Dim responsiblePersonsGridData = (From person In listOfEmployees
    Select New FoundedESReportDetailData With
           {.mdnr = person.mdnr,
            .employeeMDNr = person.employeeMDNr,
            .customerMDNr = person.customerMDNr,
            .rpnr = person.rpnr,
            .lonr = person.lonr,
            .esnr = person.esnr,
            .manr = person.manr,
            .kdnr = person.kdnr,
            .monat = person.monat,
            .jahr = person.jahr,
            .periode = person.periode,
            .employeename = person.employeename,
            .customername = person.customername,
            .rpgav_beruf = person.rpgav_beruf,
            .rpdone = person.rpdone,
            .createdon = person.createdon,
            .createdfrom = person.createdfrom,
            .zfiliale = person.zfiliale
           }).ToList()

    Dim listDataSource As BindingList(Of FoundedESReportDetailData) = New BindingList(Of FoundedESReportDetailData)

    For Each p In responsiblePersonsGridData
      listDataSource.Add(p)
    Next

    grdDetailrec.DataSource = listDataSource

    Return Not listOfEmployees Is Nothing
  End Function

#End Region


  Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

    If (e.Clicks = 2) Then

      Dim column = e.Column
      Dim dataRow = gvDetail.GetRow(e.RowHandle)
      If Not dataRow Is Nothing Then

        Select Case Me.Modul2Open.ToLower
          Case "rp"
            Dim viewData = CType(dataRow, FoundedESReportDetailData)

            Select Case column.Name.ToLower
              Case "employeename"
                If viewData.manr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.employeeMDNr, ModulConstants.UserData.UserNr)
                End If

              Case "customername"
                If viewData.kdnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
                End If

              Case Else
                If viewData.rpnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
                End If

            End Select

        End Select

      End If

    End If

  End Sub


  Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
    Dim s As String = m_translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

    Try
      s = TranslateText(s)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
    End Try

    Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
    Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
    e.Graphics.DrawString(s, font, Brushes.Black, r)

  End Sub



#Region "GridSettings"


  Private Sub RestoreGridLayoutFromXml()
    Dim keepFilter = False
    Dim restoreLayout = True

    Select Case Me.Modul2Open

      Case "rp".ToLower
        Try
          If Me.chkSelMA.Checked Then
            If File.Exists(m_GVReportSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilenameWithCustomer)
          Else
            If File.Exists(m_GVReportSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilename)
          End If

          If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

        Catch ex As Exception

        End Try


      Case Else

        Exit Sub


    End Select


  End Sub

  Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

    If Me.Modul2Open = "rp" Then

      If Me.chkSelMA.Checked Then
        gvDetail.SaveLayoutToXml(m_GVReportSettingfilenameWithCustomer)
      Else
        gvDetail.SaveLayoutToXml(m_GVReportSettingfilename)

      End If

    End If

  End Sub

#End Region


End Class