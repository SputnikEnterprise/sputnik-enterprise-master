
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports System.IO
Imports DevExpress.XtraGrid.Views.Base
Imports SPS.MainView.DataBaseAccess
Imports System.ComponentModel

Public Class frmREDetails
	Private Shared m_Logger As ILogger = New Logger()


	Private _ClsSetting As New ClsRESetting
	Private Property Modul2Open As String

	Private Property MetroForeColor As System.Drawing.Color
	Private Property MetroBorderColor As System.Drawing.Color


	Private m_GVReportSettingfilename As String
	Private m_GVReportSettingfilenameWithCustomer As String

	Private m_GVZESettingfilename As String
	Private m_GVZESettingfilenameWithCustomer As String

	Private m_translate As TranslateValues
	Private m_UitilityUI As UtilityUI

	Public Sub New(ByVal _setting As ClsRESetting, ByVal _m2Open As String)

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

			m_GVReportSettingfilename = String.Format("{0}Invoice\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVReportSettingfilenameWithCustomer = String.Format("{0}Invoice\Details\{1}_WithInvoice{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

			m_GVZESettingfilename = String.Format("{0}Invoice\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVZESettingfilenameWithCustomer = String.Format("{0}Invoice\Details\{1}_WithInvoice{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


		RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
		Me.chkSelMA.Checked = Me._ClsSetting.Data4SelectedKD
		AddHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged

		AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvDetail.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvDetail.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvDetail.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

	End Sub

	Private Sub frmKDDetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.SETTING_CUSTOMER_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.SETTING_CUSTOMER_WIDTH = Me.Width
				My.Settings.SETTING_CUSTOMER_HEIGHT = Me.Height

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
			If My.Settings.SETTING_CUSTOMER_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_CUSTOMER_HEIGHT)
			If My.Settings.SETTING_CUSTOMER_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_CUSTOMER_WIDTH)
			If My.Settings.SETTING_CUSTOMER_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_CUSTOMER_LOCATION.Split(CChar(";"))
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

			Case "ze".ToLower
				strTitle = "Anzeige der Zahlungseingänge"
				ResetZEDetailGrid()
				LoadInvoiceRecipientOfPaymentsDetailList()

				'Me._ClsSetting.gvDetailDisplayMember = "Eingang-Nr."
				'Dim _ClsZE As New ClsREZEDetails(Me._ClsSetting)
				'_ClsZE.FillKDOpenZE(Me.grdDetailrec)

			Case "rp".ToLower
				strTitle = "Anzeige der Rapporte"
				ResetInvoiceDetailGrid()
				LoadInvoiceReportDetailList()

				'Me._ClsSetting.gvDetailDisplayMember = "Rapport-Nr."
				'Dim _ClsRP As New ClsRERPDetails(Me._ClsSetting)
				'_ClsRP.FillKDOpenRP(Me.grdDetailrec)


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

		Me._ClsSetting.Data4SelectedKD = Me.chkSelMA.Checked
		Select Case Me.Modul2Open.ToLower

			Case "ze".ToLower
				ResetZEDetailGrid()
				LoadInvoiceRecipientOfPaymentsDetailList()

				'Me._ClsSetting.gvDetailDisplayMember = "Eingang-Nr."
				'Dim _ClsZE As New ClsREZEDetails(Me._ClsSetting)
				'_ClsZE.FillKDOpenZE(Me.grdDetailrec)

			Case "rp".ToLower
				ResetInvoiceDetailGrid()
				LoadInvoiceReportDetailList()

				'Me._ClsSetting.gvDetailDisplayMember = "Rapport-Nr."
				'Dim _ClsRP As New ClsRERPDetails(Me._ClsSetting)
				'_ClsRP.FillKDOpenRP(Me.grdDetailrec)


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

			Dim columnCustomeramount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomeramount.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnCustomeramount.Name = "customeramount"
			columnCustomeramount.FieldName = "customeramount"
			columnCustomeramount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnCustomeramount.DisplayFormat.FormatString = "N2"
			columnCustomeramount.Visible = True
			gvDetail.Columns.Add(columnCustomeramount)

			Dim columnrpgav_beruf As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrpgav_beruf.Caption = m_translate.GetSafeTranslationValue("GAV-Beruf")
			columnrpgav_beruf.Name = "rpgav_beruf"
			columnrpgav_beruf.FieldName = "rpgav_beruf"
			columnrpgav_beruf.Visible = True
			gvDetail.Columns.Add(columnrpgav_beruf)

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

	Public Function LoadInvoiceReportDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim invoiceNumber As Integer? = Nothing

		If Me.chkSelMA.Checked Then
			invoiceNumber = Me._ClsSetting.SelectedRENr
		End If
		Dim listOfEmployees = m_DataAccess.GetDbInvoiceReportDataForDetails(invoiceNumber)

		If listOfEmployees Is Nothing Then
			m_UitilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedInvoiceReportDetailData With
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
														  .customeramount = person.customeramount,
														  .rpgav_beruf = person.rpgav_beruf,
														  .rpdone = person.rpdone,
														  .createdon = person.createdon,
														  .createdfrom = person.createdfrom,
														  .zfiliale = person.zfiliale
													   }).ToList()

		Dim listDataSource As BindingList(Of FoundedInvoiceReportDetailData) = New BindingList(Of FoundedInvoiceReportDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdDetailrec.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

#End Region


#Region "ZE Funktionen..."


	Sub ResetZEDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = False

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zenr"
			columnmodulname.FieldName = "zenr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnInvoicenumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoicenumber.Caption = m_translate.GetSafeTranslationValue("Rechnungs-Nr.")
			columnInvoicenumber.Name = "renr"
			columnInvoicenumber.FieldName = "renr"
			columnInvoicenumber.Visible = False
			gvDetail.Columns.Add(columnInvoicenumber)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_translate.GetSafeTranslationValue("Valuta")
			columnFakDat.Name = "valutadate"
			columnFakDat.FieldName = "valutadate"
			columnFakDat.Visible = True
			gvDetail.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "zebetrag"
			columnBetragInk.FieldName = "zebetrag"
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			columnBetragInk.Visible = True
			gvDetail.Columns.Add(columnBetragInk)

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

	Public Function LoadInvoiceRecipientOfPaymentsDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim invoiceNumber As Integer? = Nothing

		If Me.chkSelMA.Checked Then
			invoiceNumber = Me._ClsSetting.SelectedRENr
		End If
		Dim listOfEmployees = m_DataAccess.GetDbInvoiceRecipientOfPaymentsDataForDetails(invoiceNumber)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedInvoiceROPDetailData With
													   {.mdnr = person.mdnr,
														  .customerMDNr = person.customerMDNr,
														  .zenr = person.zenr,
														  .renr = person.renr,
														  .kdnr = person.kdnr,
														  .valutadate = person.valutadate,
														  .zebetrag = person.zebetrag,
														  .rekst = person.rekst,
														  .createdon = person.createdon,
														  .createdfrom = person.createdfrom,
														  .zfiliale = person.zfiliale
													   }).ToList()

		Dim listDataSource As BindingList(Of FoundedInvoiceROPDetailData) = New BindingList(Of FoundedInvoiceROPDetailData)

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

					Case "ze"
						Dim viewData = CType(dataRow, FoundedInvoiceROPDetailData)

						Select Case column.Name.ToLower
							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
								End If

							Case "renr"
								If viewData.renr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedRENr = viewData.renr})
									_ClsKD.OpenSelectedInvoice(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else
								If viewData.zenr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZENr = viewData.zenr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedPayment()
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



	'Private Sub gvDetail_DoubleClick(sender As Object, e As System.EventArgs) Handles gvDetail.DoubleClick

	'	Select Case Me.Modul2Open.ToLower
	'		Case "reze".ToLower
	'			Dim _Clsze As New ClsREZEDetails(Me._ClsSetting)
	'			_Clsze.OpenSelectedRecord()

	'		Case "reRp".ToLower
	'			Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ClsSetting.SelectedMDNr,
	'																											 .SelectedRPNr = Me._ClsSetting.SelectedRPNr,
	'																											 .SelectedKDNr = Me._ClsSetting.SelectedKDNr})
	'			obj.OpenSelectedReport()


	'		Case Else

	'	End Select

	'End Sub

	'Private Sub gvDetail_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvDetail.FocusedRowChanged
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strValue As Integer = 0
	'	Dim view As GridView = TryCast(sender, GridView)

	'	Try
	'		For i As Integer = 0 To view.SelectedRowsCount - 1
	'			Dim row As Integer = (view.GetSelectedRows()(i))

	'			If (view.GetSelectedRows()(i) >= 0) Then
	'				Dim dtr As DataRow
	'				dtr = view.GetDataRow(gvDetail.GetSelectedRows()(i))
	'				strValue = CInt(dtr.Item(Me._ClsSetting.gvDetailDisplayMember).ToString)

	'			Else
	'				Exit Sub

	'			End If
	'		Next i
	'		Me._ClsSetting.SelectedDetailNr = strValue


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
	'		ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub


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


			Case "ze".ToLower
				Try
					If Me.chkSelMA.Checked Then
						If File.Exists(m_GVZESettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVZESettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVZESettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVZESettingfilename)
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

		ElseIf Me.Modul2Open = "ze" Then

			If Me.chkSelMA.Checked Then
				gvDetail.SaveLayoutToXml(m_GVZESettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVZESettingfilename)

			End If

		End If

	End Sub

#End Region


End Class