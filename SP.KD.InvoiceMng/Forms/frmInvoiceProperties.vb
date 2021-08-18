
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Invoice.DataObjects.Invoice
Imports SP.DatabaseAccess.Invoice.DataObjects

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SP.KD.InvoiceMng.Settings
Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid


''' <summary>
''' Shows founded Reports records..
''' </summary>
Public Class frmInvoiceProperties

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess
	'Private m_ESDatabaseAccess As IESDatabaseAccess

	''' <summary>
	''' Contains the employee number of the loaded employee data.
	''' </summary>
	Private m_invoiceNumber As Integer?

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private connectionString As String

	Private GridSettingPath As String
	Private m_GVVacanciesSettingfilenameWithCustomer As String
	Private m_GVProposeSettingfilenameWithCustomer As String
	Private m_GVOffersSettingfilenameWithCustomer As String
	Private m_GVESSettingfilenameWithCustomer As String
	Private m_GVReportsSettingfilenameWithCustomer As String
	Private m_GVInvoiceSettingfilenameWithCustomer As String
	Private m_GVPaymentSettingfilenameWithCustomer As String

	Private Property Searchedmodul As Integer

#End Region


#Region "private consts"

	Private Const MODUL_ZE As Integer = 7

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass,
								 ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper,
								 ByVal invoiceNumber As Integer?)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		Searchedmodul = MODUL_ZE

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = translate
		m_SettingsManager = New SettingsManager

		m_invoiceNumber = invoiceNumber

		connectionString = m_InitializationData.MDData.MDDbConn
		m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		Try
			GridSettingPath = String.Format("{0}InvoiceMng\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
			If Not Directory.Exists(GridSettingPath) Then Directory.CreateDirectory(GridSettingPath)
			If Not Directory.Exists(String.Format("{0}Properties\", GridSettingPath)) Then Directory.CreateDirectory(String.Format("{0}Properties\", GridSettingPath))

			m_GVPaymentSettingfilenameWithCustomer = String.Format("{0}Properties\Payment_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)


		Catch ex As Exception

		End Try

		' Translate controls.
		TranslateControls()

		' Creates the navigation bar.
		CreateMyNavBar()

		ResetPaymentGrid()

		AddHandler gvEmployeeProperty.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvEmployeeProperty.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvEmployeeProperty.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler gvEmployeeProperty.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub


#End Region


#Region "Private Properties"

	''' <summary>
	''' Gets the selected Payroll data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedPayrollViewData As InvoicePaymentProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim Payroll = CType(grdView.GetRow(selectedRows(0)), InvoicePaymentProperty)
					Return Payroll
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

		Me.rlblHeader.Text = m_Translate.GetSafeTranslationValue(Me.rlblHeader.Text)
		Me.chkOffeneInvoice.Text = m_Translate.GetSafeTranslationValue(Me.chkOffeneInvoice.Text)
		Me.bsiLblRecCount.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRecCount.Caption)

	End Sub


	''' <summary>
	''' Creates Navigationbar
	''' </summary>
	Private Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.navMain.Items.Clear()
		Try
			navMain.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupModule As NavBarGroup = New NavBarGroup(("Module"))
			groupModule.Name = "gNavModule"

			Dim nbiPayment As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Zahlungseingänge"))
			nbiPayment.Name = "Show_Payment"

			nbiPayment.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 15, m_InitializationData.MDData.MDNr)


			Try
				navMain.BeginUpdate()

				navMain.Groups.Add(groupModule)
				groupModule.ItemLinks.Add(nbiPayment)

				groupModule.Expanded = True

				navMain.EndUpdate()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message),
																								 "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

		End Try

	End Sub

	''' <summary>
	''' Clickevent for Navbar.
	''' </summary>
	Private Sub OnnbMain_LinkClicked(ByVal sender As Object, _
															 ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bForDesign As Boolean = False
		Try
			Dim strLinkName As String = e.Link.ItemName
			Dim strLinkCaption As String = e.Link.Caption

			For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange
			Me.rlblHeader.BackColor = Color.Transparent
			Me.chkOffeneInvoice.Visible = False

			Select Case strLinkName.ToLower
				Case "show_payment".ToLower
					Searchedmodul = 7
					ResetPaymentGrid()
					LoadFoundedInvoicePayrmentList(m_invoiceNumber)


				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally

		End Try

	End Sub


#Region "Reset grids"


	''' <summary>
	''' reset Payment grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetPaymentGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Zahlungseingänge"))

		Try
			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zenr"
			columnmodulname.FieldName = "zenr"
			columnmodulname.Visible = True
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnInvoicenumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoicenumber.Caption = m_Translate.GetSafeTranslationValue("Rechnungs-Nr.")
			columnInvoicenumber.Name = "renr"
			columnInvoicenumber.FieldName = "renr"
			columnInvoicenumber.Visible = False
			gvEmployeeProperty.Columns.Add(columnInvoicenumber)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Valuta")
			columnFakDat.Name = "valutadate"
			columnFakDat.FieldName = "valutadate"
			columnFakDat.Visible = True
			gvEmployeeProperty.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "zebetrag"
			columnBetragInk.FieldName = "zebetrag"
			columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			columnBetragInk.Visible = True
			gvEmployeeProperty.Columns.Add(columnBetragInk)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvEmployeeProperty.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


#End Region



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
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_PROPERTIES_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_PROPERTIES_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_PROPERTIES_FORM_LOCATION)

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
				m_SettingsManager.WriteString(SettingKeys.SETTING_PROPERTIES_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_PROPERTIES_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_PROPERTIES_FORM_HEIGHT, Me.Height)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvEmployeeProperty.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Searchedmodul

					Case 7
						HandleRowClickForPayment(column.Name, dataRow)


					Case Else
						Exit Sub

				End Select

			End If

		End If

	End Sub

	Sub HandleRowClickForPayment(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, InvoicePaymentProperty)

			Select Case column.ToLower
				Case "renr"
					If viewData.renr > 0 Then OpenSelectedInvoice(viewData.renr, viewData.mdnr)

				Case Else
					If viewData.zenr > 0 Then OpenSelectedPayment(viewData.zenr, viewData.mdnr)

			End Select

		End If

	End Sub


#End Region

#Region "Open modules"

	Private Sub OpenSelectedCustomer(ByVal customernumber As Integer, ByVal mandantnumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, customernumber)
		hub.Publish(openMng)

	End Sub

	''' <summary>
	''' Handles focus change of es row.
	''' </summary>
	Private Sub OpenSelectedES(ByVal esnumber As Integer, ByVal mandantnumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, esnumber)
		hub.Publish(openMng)

	End Sub

	''' <summary>
	''' Handles focus change of report row.
	''' </summary>
	Private Sub OpenSelectedReport(ByVal reportNumber As Integer, ByVal mandantnumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, reportNumber)
		hub.Publish(openMng)

	End Sub

	''' <summary>
	''' Handles focus change of advancepayment row.
	''' </summary>
	Private Sub OpenSelectedInvoice(ByVal invoiceNumber As Integer, ByVal mandantnumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, invoiceNumber)
		hub.Publish(openMng)

	End Sub

	Private Sub OpenSelectedPayment(ByVal paymentNumber As Integer, ByVal mandantnumber As Integer)

		Dim frmPaymentMng = New SP.KD.InvoiceMng.UI.frmZEedit(m_InitializationData)

		frmPaymentMng.CurrentPaymentNumber = paymentNumber
		If frmPaymentMng.LoadData() Then
			frmPaymentMng.Show()
			frmPaymentMng.BringToFront()
		End If

	End Sub


#End Region


#Region "Public Methods"

	''' <summary>
	''' Loads payment for selected invoice
	''' </summary>
	''' <param name="invoiceNumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedInvoicePayrmentList(ByVal invoiceNumber As Integer?) As Boolean

		If invoiceNumber.HasValue Then m_invoiceNumber = invoiceNumber
		Dim listOfPayroll = m_InvoiceDatabaseAccess.LoadFoundedPaymentForInvoiceMng(invoiceNumber)
		If listOfPayroll Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Zahlungseingang-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfPayroll
		Select New InvoicePaymentProperty With
					 {.mdnr = report.mdnr,
						.customerMDNr = report.customerMDNr,
						.zenr = report.zenr,
						.renr = report.renr,
						.kdnr = report.kdnr,
						.valutadate = report.valutadate,
						.zebetrag = report.zebetrag,
						.rekst = report.rekst,
						.createdon = report.createdon,
						.createdfrom = report.createdfrom,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of InvoicePaymentProperty) = New BindingList(Of InvoicePaymentProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfPayroll.Count

		Return Not listOfPayroll Is Nothing
	End Function


#End Region


#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case Me.Searchedmodul
			Case 7
				Try
					If File.Exists(m_GVPaymentSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVPaymentSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case Else

				Exit Sub


		End Select


	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		Select Case Me.Searchedmodul

			Case 7
				gvEmployeeProperty.SaveLayoutToXml(m_GVPaymentSettingfilenameWithCustomer)


		End Select

	End Sub


	Private Sub OngvEmployeeProperty_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvEmployeeProperty.RowCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub


#End Region



End Class
