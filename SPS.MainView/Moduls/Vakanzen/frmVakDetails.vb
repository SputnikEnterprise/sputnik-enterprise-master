
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid

'Imports DevComponents.DotNetBar
'Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Logging

Imports SP.Infrastructure.UI

Imports DevExpress.XtraGrid.Views.Base
Imports SPS.MainView.DataBaseAccess
Imports System.ComponentModel
Imports System.IO


Public Class frmVakDetails
  Private Shared m_Logger As ILogger = New Logger()


  Private _ClsSetting As New ClsVakSetting
  Private Property Modul2Open As String

  Private Property MetroForeColor As System.Drawing.Color
  Private Property MetroBorderColor As System.Drawing.Color

  Private m_translate As TranslateValues
  Private m_UitilityUI As UtilityUI

  Private m_GVESSettingfilename As String
  Private m_GVProposeSettingfilename As String

  Private m_GVESSettingfilenameWithCustomer As String
  Private m_GVProposeSettingfilenameWithCustomer As String



  Public Sub New(ByVal _setting As ClsVakSetting, ByVal _m2Open As String)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me._ClsSetting = _setting
    Me.Modul2Open = _m2Open
    Me._ClsSetting.OpenDetailModul = _m2Open

    m_translate = New TranslateValues
    m_UitilityUI = New UtilityUI

    Try
      Dim strModulName As String = Me.Modul2Open.ToLower

      m_GVESSettingfilename = String.Format("{0}Vacancy\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
      m_GVProposeSettingfilename = String.Format("{0}Vacancy\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

      m_GVESSettingfilenameWithCustomer = String.Format("{0}Vacancy\Details\{1}_WithVacancy{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
      m_GVProposeSettingfilenameWithCustomer = String.Format("{0}Vacancy\Details\{1}_WithVacancy{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)


    Catch ex As Exception
      m_Logger.LogError(ex.ToString())
    End Try


    RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
    Me.chkSelMA.Checked = Me._ClsSetting.Data4SelectedVak
    AddHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged

    AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
    AddHandler Me.gvDetail.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
    AddHandler Me.gvDetail.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
    AddHandler Me.gvDetail.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

    AddHandler Me.gvDetail.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

  End Sub

  Private Sub frmVakDetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        My.Settings.SETTING_VACANCIES_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
        My.Settings.SETTING_VACANCIES_WIDTH = Me.Width
        My.Settings.SETTING_VACANCIES_HEIGHT = Me.Height

        My.Settings.Save()
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
        If My.Settings.SETTING_VACANCIES_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_VACANCIES_HEIGHT)
        If My.Settings.SETTING_VACANCIES_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_VACANCIES_WIDTH)
        If My.Settings.SETTING_VACANCIES_LOCATION <> String.Empty Then
          Dim aLoc As String() = My.Settings.SETTING_VACANCIES_LOCATION.Split(CChar(";"))
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
      Case "Propose".ToLower
        strTitle = "Anzeige der Vorschläge"
        ResetProposeDetailGrid()
        LoadVacanciesProposeDetailList()

      Case "ES".ToLower
        strTitle = "Anzeige der Einsätze"
        ResetESDetailGrid()
        LoadVacanciesESDetailList()

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

    Me._ClsSetting.Data4SelectedVak = Me.chkSelMA.Checked
    Select Case Me.Modul2Open.ToLower
      Case "Propose".ToLower
        ResetProposeDetailGrid()
        LoadVacanciesProposeDetailList()

      Case "ES".ToLower
        ResetESDetailGrid()
        LoadVacanciesESDetailList()


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


#Region "Details for Vacancies ES"

  Sub ResetESDetailGrid()

    gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
    gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
    gvDetail.OptionsView.ShowGroupPanel = False
    gvDetail.OptionsView.ShowIndicator = False
    gvDetail.OptionsView.ShowAutoFilterRow = True

    gvDetail.Columns.Clear()

    Try

      Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
      columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
      columnmodulname.Name = "esnr"
      columnmodulname.FieldName = "esnr"
      columnmodulname.Visible = False
      gvDetail.Columns.Add(columnmodulname)

      Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
      columnMANr.Name = "manr"
      columnMANr.FieldName = "manr"
      columnMANr.Visible = False
      gvDetail.Columns.Add(columnMANr)

      Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
      columnKDNr.Name = "kdnr"
      columnKDNr.FieldName = "kdnr"
      columnKDNr.Visible = False
      gvDetail.Columns.Add(columnKDNr)

      Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
      columnZHDNr.Name = "zhdnr"
      columnZHDNr.FieldName = "zhdnr"
      columnZHDNr.Visible = False
      gvDetail.Columns.Add(columnZHDNr)

      Dim columnPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
      columnPeriode.Caption = m_translate.GetSafeTranslationValue("Periode")
      columnPeriode.Name = "periode"
      columnPeriode.FieldName = "periode"
      columnPeriode.Visible = False
      gvDetail.Columns.Add(columnPeriode)

      Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
      columnESAls.Caption = m_translate.GetSafeTranslationValue("Als")
      columnESAls.Name = "esals"
      columnESAls.FieldName = "esals"
      columnESAls.Visible = True
      gvDetail.Columns.Add(columnESAls)

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
      columnEmployeename.Visible = False
      gvDetail.Columns.Add(columnEmployeename)

      Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
      columnTarif.Caption = m_translate.GetSafeTranslationValue("Tarif")
      columnTarif.Name = "tarif"
      columnTarif.FieldName = "tarif"
      columnTarif.Visible = False
      gvDetail.Columns.Add(columnTarif)

      Dim columnStundenlohn As New DevExpress.XtraGrid.Columns.GridColumn()
      columnStundenlohn.Caption = m_translate.GetSafeTranslationValue("Stundenlohn")
      columnStundenlohn.Name = "stundenlohn"
      columnStundenlohn.FieldName = "stundenlohn"
      columnStundenlohn.Visible = False
      gvDetail.Columns.Add(columnStundenlohn)

      Dim columnMargeMitBVG As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMargeMitBVG.Caption = m_translate.GetSafeTranslationValue("Marge mit BVG")
      columnMargeMitBVG.Name = "margemitbvg"
      columnMargeMitBVG.FieldName = "margemitbvg"
      columnMargeMitBVG.Visible = False
      gvDetail.Columns.Add(columnMargeMitBVG)

      Dim columnMargeOhneBVG As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMargeOhneBVG.Caption = m_translate.GetSafeTranslationValue("Marge ohne BVG")
      columnMargeOhneBVG.Name = "margeohnebvg"
      columnMargeOhneBVG.FieldName = "margeohnebvg"
      columnMargeOhneBVG.Visible = False
      gvDetail.Columns.Add(columnMargeOhneBVG)

      Dim columnActivES As New DevExpress.XtraGrid.Columns.GridColumn()
      columnActivES.Caption = m_translate.GetSafeTranslationValue("Aktiv?")
      columnActivES.Name = "actives"
      columnActivES.FieldName = "actives"
      columnActivES.Visible = True
      gvDetail.Columns.Add(columnActivES)


      Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
      columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
      columnZFiliale.Name = "zfiliale"
      columnZFiliale.FieldName = "zfiliale"
      columnZFiliale.Visible = False
      gvDetail.Columns.Add(columnZFiliale)

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

  Public Function LoadVacanciesESDetailList() As Boolean
    Dim m_DataAccess As New MainGrid
    Dim vacancyNumber As Integer? = Nothing

    If Me.chkSelMA.Checked Then
      vacancyNumber = Me._ClsSetting.SelectedVakNr
    End If
    Dim listOfEmployees = m_DataAccess.GetDbVacanciesESDataForDetails(vacancyNumber)

    If listOfEmployees Is Nothing Then
      m_UitilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
      Return False
    End If

    Dim responsiblePersonsGridData = (From person In listOfEmployees
    Select New FoundedVacancyESDetailData With
           {.mdnr = person.mdnr,
            .esnr = person.esnr,
            .manr = person.manr,
            .kdnr = person.kdnr,
            .zhdnr = person.zhdnr,
            .periode = person.periode,
            .employeename = person.employeename,
            .customername = person.customername,
            .esals = person.esals,
            .tarif = person.tarif,
            .stundenlohn = person.stundenlohn,
            .margemitbvg = person.margemitbvg,
            .margeohnebvg = person.margeohnebvg,
            .actives = person.actives,
            .createdfrom = person.createdfrom,
            .createdon = person.createdon,
            .zfiliale = person.zfiliale
           }).ToList()

    Dim listDataSource As BindingList(Of FoundedVacancyESDetailData) = New BindingList(Of FoundedVacancyESDetailData)

    For Each p In responsiblePersonsGridData
      listDataSource.Add(p)
    Next

    grdDetailrec.DataSource = listDataSource

    Return Not listOfEmployees Is Nothing
  End Function

#End Region


#Region "Details for vacancies Propose"

  Sub ResetProposeDetailGrid()

    gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
    gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
    gvDetail.OptionsView.ShowGroupPanel = False
    gvDetail.OptionsView.ShowIndicator = False
    gvDetail.OptionsView.ShowAutoFilterRow = True

    gvDetail.Columns.Clear()

    Try

      Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
      columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
      columnmodulname.Name = "pnr"
      columnmodulname.FieldName = "pnr"
      columnmodulname.Visible = False
      gvDetail.Columns.Add(columnmodulname)

      Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
      columnMANr.Name = "manr"
      columnMANr.FieldName = "manr"
      columnMANr.Visible = False
      gvDetail.Columns.Add(columnMANr)

      Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
      columnKDNr.Name = "kdnr"
      columnKDNr.FieldName = "kdnr"
      columnKDNr.Visible = False
      gvDetail.Columns.Add(columnKDNr)

      Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
      columnZHDNr.Name = "zhdnr"
      columnZHDNr.FieldName = "zhdnr"
      columnZHDNr.Visible = False
      gvDetail.Columns.Add(columnZHDNr)

      Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
      columnESAls.Caption = m_translate.GetSafeTranslationValue("Bezeichnung")
      columnESAls.Name = "bezeichung"
      columnESAls.FieldName = "bezeichnung"
      columnESAls.Visible = True
      gvDetail.Columns.Add(columnESAls)

      Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
      columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
      columncustomername.Name = "customername"
      columncustomername.FieldName = "customername"
      columncustomername.Visible = True
      gvDetail.Columns.Add(columncustomername)

      Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
      columnZHDName.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
      columnZHDName.Name = "zhdname"
      columnZHDName.FieldName = "zhdname"
      columnZHDName.Visible = True
      gvDetail.Columns.Add(columnZHDName)

      Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
      columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
      columnEmployeename.Name = "employeename"
      columnEmployeename.FieldName = "employeename"
      columnEmployeename.Visible = False
      gvDetail.Columns.Add(columnEmployeename)

      Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
      columnAdvisor.Caption = m_translate.GetSafeTranslationValue("Berater")
      columnAdvisor.Name = "advisor"
      columnAdvisor.FieldName = "advisor"
      columnAdvisor.Visible = False
      gvDetail.Columns.Add(columnAdvisor)

      Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
      columnPArt.Caption = m_translate.GetSafeTranslationValue("Art")
      columnPArt.Name = "p_art"
      columnPArt.FieldName = "p_art"
      columnPArt.Visible = False
      gvDetail.Columns.Add(columnPArt)

      Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
      columnPState.Caption = m_translate.GetSafeTranslationValue("Status")
      columnPState.Name = "p_state"
      columnPState.FieldName = "p_state"
      columnPState.Visible = False
      gvDetail.Columns.Add(columnPState)


      Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
      columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
      columnZFiliale.Name = "zfiliale"
      columnZFiliale.FieldName = "zfiliale"
      columnZFiliale.Visible = False
      gvDetail.Columns.Add(columnZFiliale)

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

  Public Function LoadVacanciesProposeDetailList() As Boolean
    Dim m_DataAccess As New MainGrid
    Dim vacancyNumber As Integer? = Nothing

    If Me.chkSelMA.Checked Then
      vacancyNumber = Me._ClsSetting.SelectedVakNr
    End If
    Dim listOfEmployees = m_DataAccess.GetDbVacanciesProposalDataForDetails(vacancyNumber)

    If listOfEmployees Is Nothing Then
      m_UitilityUI.ShowErrorDialog("Fehler in der Vorschlag-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
      Return False
    End If

    Dim responsiblePersonsGridData = (From person In listOfEmployees
    Select New FoundedVacancyProposalDetailData With
           {.mdnr = person.mdnr,
            .pnr = person.pnr,
            .manr = person.manr,
            .kdnr = person.kdnr,
            .zhdnr = person.zhdnr,
            .employeename = person.employeename,
            .customername = person.customername,
            .bezeichnung = person.bezeichnung,
            .zhdname = person.zhdname,
            .p_art = person.p_art,
            .p_state = person.p_state,
            .advisor = person.advisor,
            .createdfrom = person.createdfrom,
            .createdon = person.createdon,
            .zfiliale = person.zfiliale
           }).ToList()

    Dim listDataSource As BindingList(Of FoundedVacancyProposalDetailData) = New BindingList(Of FoundedVacancyProposalDetailData)

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
          Case "es".ToLower
            Dim viewData = CType(dataRow, FoundedVacancyESDetailData)

            Select Case column.Name.ToLower
              Case "employeename"
                If viewData.manr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
                End If

              Case "customername"
                If viewData.kdnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
                End If

              Case Else

                If viewData.esnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedESNr = viewData.esnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
                End If

            End Select


          Case "propose".ToLower
            Dim viewData = CType(dataRow, FoundedVacancyProposalDetailData)

            Select Case column.Name.ToLower
              Case "employeename"
                If viewData.manr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
                End If

              Case "customername"
                If viewData.kdnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
                End If

              Case "zhdname"
                If viewData.zhdnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
                  _ClsKD.OpenSelectedCPerson()
                End If

              Case Else
                If viewData.pnr > 0 Then
                  Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedProposeNr = viewData.pnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
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
      Case "es".ToLower
        Try
          If Me.chkSelMA.Checked Then
            If File.Exists(m_GVESSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilenameWithCustomer)
          Else
            If File.Exists(m_GVESSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilename)
          End If

          If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

        Catch ex As Exception

        End Try


      Case "propose".ToLower
        Try
          If Me.chkSelMA.Checked Then
            If File.Exists(m_GVProposeSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilenameWithCustomer)
          Else
            If File.Exists(m_GVProposeSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilename)
          End If

          If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

        Catch ex As Exception

        End Try


      Case Else

        Exit Sub


    End Select


  End Sub

  Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

    If Me.Modul2Open = "es" Then
      If Me.chkSelMA.Checked Then
        gvDetail.SaveLayoutToXml(m_GVESSettingfilenameWithCustomer)
      Else
        gvDetail.SaveLayoutToXml(m_GVESSettingfilename)

      End If

    ElseIf Me.Modul2Open = "propose" Then
      If Me.chkSelMA.Checked Then
        gvDetail.SaveLayoutToXml(m_GVProposeSettingfilenameWithCustomer)
      Else
        gvDetail.SaveLayoutToXml(m_GVProposeSettingfilename)

      End If

    End If

  End Sub


#End Region


End Class