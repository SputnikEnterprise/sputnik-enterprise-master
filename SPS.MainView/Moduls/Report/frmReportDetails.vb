
Imports SP.Infrastructure.UI
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Settings
Imports SPS.MainView.ReportSettings
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Views.Base
Imports SPS.MainView.DataBaseAccess
Imports System.ComponentModel
Imports System.IO



Public Class frmReportDetails
	Private Shared m_Logger As ILogger = New Logger()

	Protected m_SettingsManager As ISettingsManager

	Private _ClsSetting As New ClsReportSetting
	Private Property Modul2Open As String

	Private Property MetroForeColor As System.Drawing.Color
	Private Property MetroBorderColor As System.Drawing.Color

	Private m_translate As TranslateValues
	Private m_UitilityUI As UtilityUI

	Private m_GVZGSettingfilename As String
	Private m_GVInvoiceSettingfilename As String

	Private m_GVZGSettingfilenameWithEmployee As String
	Private m_GVInvoiceSettingfilenameWithCustomer As String



	Public Sub New(ByVal _setting As ClsReportSetting, ByVal _m2Open As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._ClsSetting = _setting
		Me.Modul2Open = _m2Open
		Me._ClsSetting.OpenDetailModul = _m2Open
		m_SettingsManager = New SettingsReportManager

		m_translate = New TranslateValues
		m_UitilityUI = New UtilityUI

		Try
			Dim strModulName As String = Me.Modul2Open.ToLower

			m_GVZGSettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVInvoiceSettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

			m_GVZGSettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVInvoiceSettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


		RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
		Me.chkSelMA.Checked = Me._ClsSetting.Data4SelectedRP
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
				m_SettingsManager.WriteString(SettingReportKeys.SETTING_RP_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_FORM_HEIGHT, Me.Height)

				m_SettingsManager.SaveSettings()

				My.Settings.Save()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub frmMADetails_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		OngvColumnPositionChanged(New Object, New System.EventArgs)

	End Sub

	Private Sub frmDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTitle As String = String.Empty


		Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingReportKeys.SETTING_RP_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingReportKeys.SETTING_RP_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingReportKeys.SETTING_RP_FORM_LOCATION)

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
			Case "ZG".ToLower
				strTitle = "Anzeige der Vorschüsse"
				ResetZGDetailGrid()
				LoadReportZGDetailList()

			Case "RE".ToLower
				strTitle = "Anzeige der Rechnungen"
				ResetInvoiceDetailGrid()
				LoadReportInvoiceDetailList()

			Case Else

		End Select
		Me.Text = String.Format(TranslateText(strTitle))
		Me.rlblDetailHeader.Text = String.Format("<b>{0}</b>", TranslateText(strTitle))
		Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

	End Sub

	Private Sub chkSelMA_CheckedChanged(sender As System.Object, e As System.EventArgs)	'Handles chkSelMA.CheckedChanged

		grdDetailrec.BeginUpdate()
		Me.gvDetail.Columns.Clear()
		Me.grdDetailrec.DataSource = Nothing

		Me._ClsSetting.Data4SelectedRP = Me.chkSelMA.Checked
		Select Case Me.Modul2Open.ToLower
			Case "zg".ToLower
				ResetZGDetailGrid()
				LoadReportZGDetailList()

			Case "RE".ToLower
				ResetInvoiceDetailGrid()
				LoadReportInvoiceDetailList()


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

#Region "Details for employee ZG"

	Sub ResetZGDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zgnr"
			columnmodulname.FieldName = "zgnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvDetail.Columns.Add(columnMANr)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.Caption = m_translate.GetSafeTranslationValue("Lohn-Nr.")
			columnLONr.Name = "lonr"
			columnLONr.FieldName = "lonr"
			columnLONr.Visible = False
			gvDetail.Columns.Add(columnLONr)

			Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPNr.Caption = m_translate.GetSafeTranslationValue("Rapport-Nr.")
			columnRPNr.Name = "rpnr"
			columnRPNr.FieldName = "rpnr"
			columnRPNr.Visible = False
			gvDetail.Columns.Add(columnRPNr)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_translate.GetSafeTranslationValue("Auszahlungsart")
			columnLAName.Name = "laname"
			columnLAName.FieldName = "laname"
			columnLAName.Visible = True
			gvDetail.Columns.Add(columnLAName)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "betrag"
			columnBetrag.FieldName = "betrag"
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnBetrag)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnZGPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZGPeriode.Caption = m_translate.GetSafeTranslationValue("Zeitraum")
			columnZGPeriode.Name = "zgperiode"
			columnZGPeriode.FieldName = "zgperiode"
			columnZGPeriode.Visible = False
			gvDetail.Columns.Add(columnZGPeriode)

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "lanr"
			columnLANr.FieldName = "lanr"
			columnLANr.Visible = False
			gvDetail.Columns.Add(columnLANr)

			Dim columnAusDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAusDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnAusDat.Name = "aus_dat"
			columnAusDat.FieldName = "aus_dat"
			columnAusDat.Visible = False
			gvDetail.Columns.Add(columnAusDat)

			Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonat.Caption = m_translate.GetSafeTranslationValue("Monat")
			columnMonat.Name = "monat"
			columnMonat.FieldName = "monat"
			columnMonat.Visible = False
			gvDetail.Columns.Add(columnMonat)

			Dim columnJahr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJahr.Caption = m_translate.GetSafeTranslationValue("Jahr")
			columnJahr.Name = "jahr"
			columnJahr.FieldName = "jahr"
			columnJahr.Visible = False
			gvDetail.Columns.Add(columnJahr)

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

	Public Function LoadReportZGDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim reportNumber As Integer? = Nothing

		If Me.chkSelMA.Checked Then
			reportNumber = Me._ClsSetting.SelectedRPNr
		End If
		Dim listOfEmployees = m_DataAccess.GetDbReportZGDataForDetails(reportNumber)

		If listOfEmployees Is Nothing Then
			m_UitilityUI.ShowErrorDialog("Fehler in der Vorschuss-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New FoundedReportZGDetailData With
					 {.mdnr = person.mdnr,
						.employeeMDNr = person.employeeMDNr,
						.zgnr = person.zgnr,
						.rpnr = person.rpnr,
						.manr = person.manr,
						.vgnr = person.vgnr,
						.lonr = person.lonr,
						.monat = person.monat,
						.jahr = person.jahr,
						.betrag = person.betrag,
						.employeename = person.employeename,
						.zgperiode = person.zgperiode,
						.aus_dat = person.aus_dat,
						.lanr = person.lanr,
						.laname = person.laname,
						.isaslo = person.isaslo,
						.isout = person.isout,
						.createdfrom = person.createdfrom,
						.createdon = person.createdon,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedReportZGDetailData) = New BindingList(Of FoundedReportZGDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdDetailrec.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

#End Region


#Region "Details for customer invoice"

	Sub ResetInvoiceDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "renr"
			columnmodulname.FieldName = "renr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columnCustomerName.Name = "firma1"
			columnCustomerName.FieldName = "firma1"
			columnCustomerName.Visible = True
			gvDetail.Columns.Add(columnCustomerName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "plzort"
			columnAddress.FieldName = "plzort"
			columnAddress.Visible = True
			gvDetail.Columns.Add(columnAddress)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "fakdate"
			columnFakDat.FieldName = "fakdate"
			columnFakDat.Visible = True
			gvDetail.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "betragink"
			columnBetragInk.FieldName = "betragink"
			columnBetragInk.Visible = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnBetragInk)

			Dim columnBetragOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragOpen.Caption = m_translate.GetSafeTranslationValue("Offener Betrag")
			columnBetragOpen.Name = "betragopen"
			columnBetragOpen.FieldName = "betragopen"
			columnBetragOpen.Visible = True
			columnBetragOpen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragOpen.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnBetragOpen)

			Dim columnIsOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsOpen.Caption = m_translate.GetSafeTranslationValue("Offen?")
			columnIsOpen.Name = "isopen"
			columnIsOpen.FieldName = "isopen"
			columnIsOpen.Visible = True
			gvDetail.Columns.Add(columnIsOpen)

			Dim columnREKst As New DevExpress.XtraGrid.Columns.GridColumn()
			columnREKst.Caption = m_translate.GetSafeTranslationValue("KST")
			columnREKst.Name = "rekst"
			columnREKst.FieldName = "rekst"
			columnREKst.Visible = True
			gvDetail.Columns.Add(columnREKst)

			Dim columnCustomerAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerAdvisor.Caption = m_translate.GetSafeTranslationValue("Kunden-Berater")
			columnCustomerAdvisor.Name = "customeradvisor"
			columnCustomerAdvisor.FieldName = "customeradvisor"
			columnCustomerAdvisor.Visible = True
			gvDetail.Columns.Add(columnCustomerAdvisor)

			Dim columnEmployeeAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeAdvisor.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Berater")
			columnEmployeeAdvisor.Name = "employeeadvisor"
			columnEmployeeAdvisor.FieldName = "employeeadvisor"
			columnEmployeeAdvisor.Visible = True
			gvDetail.Columns.Add(columnEmployeeAdvisor)

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

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadReportInvoiceDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim reportNumber As Integer? = Nothing

		If Me.chkSelMA.Checked Then
			reportNumber = Me._ClsSetting.SelectedRPNr
		End If
		Dim listOfEmployees = m_DataAccess.GetDbReportInvoiceDataForDetails(reportNumber)

		If listOfEmployees Is Nothing Then
			m_UitilityUI.ShowErrorDialog("Fehler in der Debitoren-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New FoundedReportInvoiceDetailData With
					 {.mdnr = person.mdnr,
						.customerMDNr = person.customerMDNr,
						.renr = person.renr,
						.kdnr = person.kdnr,
						.firma1 = person.firma1,
						.plzort = person.plzort,
						.fakdate = person.fakdate,
						.betragink = person.betragink,
						.betragopen = person.betragopen,
						.rekst = person.rekst,
						.isopen = person.isopen,
						.customeradvisor = person.customeradvisor,
						.employeeadvisor = person.employeeadvisor,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedReportInvoiceDetailData) = New BindingList(Of FoundedReportInvoiceDetailData)

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
					Case "zg"
						Dim viewData = CType(dataRow, FoundedReportZGDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.employeeMDNr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.zgnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZGNr = viewData.zgnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedAdvancePayment(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select

					Case "re"
						Dim viewData = CType(dataRow, FoundedReportInvoiceDetailData)

						Select Case column.Name.ToLower
							Case "firma1"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.renr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedInvoice(viewData.mdnr, ModulConstants.UserData.UserNr)
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
			Case "zg".ToLower
				Try
					If Me.chkSelMA.Checked Then
						If File.Exists(m_GVZGSettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVZGSettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVZGSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVZGSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case Else

				Exit Sub


		End Select


	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)


		If Me.Modul2Open = "zg" Then
			If Me.chkSelMA.Checked Then
				gvDetail.SaveLayoutToXml(m_GVZGSettingfilenameWithEmployee)
			Else
				gvDetail.SaveLayoutToXml(m_GVZGSettingfilename)

			End If

		ElseIf Me.Modul2Open = "re" Then

			If Me.chkSelMA.Checked Then
				gvDetail.SaveLayoutToXml(m_GVInvoiceSettingfilenameWithCustomer)

			Else

				gvDetail.SaveLayoutToXml(m_GVInvoiceSettingfilename)

			End If

		End If

	End Sub


#End Region



End Class