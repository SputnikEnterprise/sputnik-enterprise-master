
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer.DataObjects.CustomerMasterData	'DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Customer.DataObjects

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SP.KD.CustomerMng.Settings
Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid


''' <summary>
''' Shows founded Reports records..
''' </summary>
Public Class frmCustomersProperties

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	'Private m_ESDatabaseAccess As IESDatabaseAccess

	''' <summary>
	''' Contains the employee number of the loaded employee data.
	''' </summary>
	Private m_CustomerNumber As Integer?

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

	Private Const MODUL_Vacancies As Integer = 1
	Private Const MODUL_Propose As Integer = 2
	Private Const MODUL_Offers As Integer = 3

	Private Const MODUL_ES As Integer = 4
	Private Const MODUL_RP As Integer = 5

	Private Const MODUL_RE As Integer = 6
	Private Const MODUL_ZE As Integer = 7

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass,
								 ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper,
								 ByVal customerNumber As Integer?)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		Searchedmodul = MODUL_Vacancies

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = translate
		m_SettingsManager = New SettingsManager

		m_CustomerNumber = CustomerNumber

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		Try
			GridSettingPath = String.Format("{0}CustomerMng\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
			If Not Directory.Exists(GridSettingPath) Then Directory.CreateDirectory(GridSettingPath)
			If Not Directory.Exists(String.Format("{0}Properties\", GridSettingPath)) Then Directory.CreateDirectory(String.Format("{0}Properties\", GridSettingPath))

			m_GVVacanciesSettingfilenameWithCustomer = String.Format("{0}Properties\Vacancies_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVProposeSettingfilenameWithCustomer = String.Format("{0}Properties\Propose_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVOffersSettingfilenameWithCustomer = String.Format("{0}Properties\Offers_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

			m_GVESSettingfilenameWithCustomer = String.Format("{0}Properties\ES_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVReportsSettingfilenameWithCustomer = String.Format("{0}Properties\Reports_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

			m_GVInvoiceSettingfilenameWithCustomer = String.Format("{0}Properties\Invoice_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVPaymentSettingfilenameWithCustomer = String.Format("{0}Properties\Payment_WithCustomer{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)


		Catch ex As Exception

		End Try

		' Translate controls.
		TranslateControls()

		' Creates the navigation bar.
		CreateMyNavBar()

		AddHandler gvEmployeeProperty.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvEmployeeProperty.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvEmployeeProperty.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler gvEmployeeProperty.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub


#End Region


#Region "Private Properties"

	Private Sub chkOffeneInvoice_CheckedChanged(sender As Object, e As EventArgs) Handles chkOffeneInvoice.CheckedChanged
		If chkOffeneInvoice.Checked Then
			Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Nicht beglichene Debitoren"))
		Else
			Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Alle Debitoren"))
		End If
		LoadFoundedCustomerInvoiceList(m_CustomerNumber)
	End Sub

	''' <summary>
	''' Gets the selected Propose data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedProposeViewData As CustomerProposeProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim propose = CType(grdView.GetRow(selectedRows(0)), CustomerProposeProperty)
					Return propose
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected Offer data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedOfferViewData As CustomerOfferProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim offer = CType(grdView.GetRow(selectedRows(0)), CustomerOfferProperty)
					Return offer
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected Report data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedReportsViewData As CustomerReportsProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim report = CType(grdView.GetRow(selectedRows(0)), CustomerReportsProperty)
					Return report
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected invoice data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedInvoiceViewData As CustomerInvoiceProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim AdvancePayment = CType(grdView.GetRow(selectedRows(0)), CustomerInvoiceProperty)
					Return AdvancePayment
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected payment data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedPaymentViewData As CustomerPaymentProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim Payroll = CType(grdView.GetRow(selectedRows(0)), CustomerPaymentProperty)
					Return Payroll
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected document data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedDocumentViewData As CustomerVacanciesProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim PrintedDocument = CType(grdView.GetRow(selectedRows(0)), CustomerVacanciesProperty)
					Return PrintedDocument
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

			Dim nbiVacancies As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vakanzen"))
			nbiVacancies.Name = "Show_Vacancies"

			Dim nbiPropose As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschläge"))
			nbiPropose.Name = "Show_Propose"

			Dim nbiOffers As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Offerte"))
			nbiOffers.Name = "Show_Offers"

			Dim nbiES As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsätze"))
			nbiES.Name = "Show_ES"

			Dim nbiReports As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rapporte"))
			nbiReports.Name = "Show_Reports"

			Dim nbiInvoice As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rechnungen"))
			nbiInvoice.Name = "Show_Invoice"

			Dim nbiPayment As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Zahlungseingänge"))
			nbiPayment.Name = "Show_Payment"

			nbiVacancies.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 701, m_InitializationData.MDData.MDNr)
			nbiPropose.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 801, m_InitializationData.MDData.MDNr)
			nbiOffers.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 801, m_InitializationData.MDData.MDNr)

			nbiES.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 250, m_InitializationData.MDData.MDNr)
			nbiReports.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 300, m_InitializationData.MDData.MDNr)
			nbiInvoice.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 14, m_InitializationData.MDData.MDNr)
			nbiPayment.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 15, m_InitializationData.MDData.MDNr)


			Try
				navMain.BeginUpdate()

				navMain.Groups.Add(groupModule)
				groupModule.ItemLinks.Add(nbiVacancies)
				groupModule.ItemLinks.Add(nbiPropose)
				groupModule.ItemLinks.Add(nbiOffers)

				groupModule.ItemLinks.Add(nbiES)
				groupModule.ItemLinks.Add(nbiReports)
				groupModule.ItemLinks.Add(nbiInvoice)
				groupModule.ItemLinks.Add(nbiPayment)

				groupModule.Expanded = True

				navMain.EndUpdate()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))

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

			LoadData(m_CustomerNumber, strLinkName.ToLower)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally

		End Try

	End Sub


#Region "Reset grids"


	''' <summary>
	''' reset Propose grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetVacanciesGrid()

		gvEmployeeProperty.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvEmployeeProperty.OptionsView.ShowGroupPanel = False
		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True

		gvEmployeeProperty.Columns.Clear()


		rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Vakanzen"))

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "vaknr"
			columnmodulname.FieldName = "vaknr"
			columnmodulname.Visible = False
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "kdzhdnr"
			columnZHDNr.FieldName = "kdzhdnr"
			columnZHDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnZHDNr)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvEmployeeProperty.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "firma1"
			columncustomername.FieldName = "firma1"
			columncustomername.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.Name = "kdzname"
			columnZHDName.FieldName = "kdzname"
			columnZHDName.Visible = True
			gvEmployeeProperty.Columns.Add(columnZHDName)

			Dim columnjchisonline As New DevExpress.XtraGrid.Columns.GridColumn()
			columnjchisonline.Caption = m_Translate.GetSafeTranslationValue("Jobs.ch")
			columnjchisonline.Name = "jchisonline"
			columnjchisonline.FieldName = "jchisonline"
			columnjchisonline.Visible = False
			gvEmployeeProperty.Columns.Add(columnjchisonline)

			Dim columnojisonline As New DevExpress.XtraGrid.Columns.GridColumn()
			columnojisonline.Caption = m_Translate.GetSafeTranslationValue("ostjob.ch")
			columnojisonline.Name = "ojisonline"
			columnojisonline.FieldName = "ojisonline"
			columnojisonline.Visible = False
			gvEmployeeProperty.Columns.Add(columnojisonline)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Eigene Plattform")
			columnAdvisor.Name = "ourisonline"
			columnAdvisor.FieldName = "ourisonline"
			columnAdvisor.Visible = False
			gvEmployeeProperty.Columns.Add(columnAdvisor)

			Dim columnjobchdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnjobchdate.Caption = m_Translate.GetSafeTranslationValue("Jobs.ch Datum")
			columnjobchdate.Name = "jobchdate"
			columnjobchdate.FieldName = "jobchdate"
			columnjobchdate.Visible = False
			gvEmployeeProperty.Columns.Add(columnjobchdate)

			Dim columnostjobchdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnostjobchdate.Caption = m_Translate.GetSafeTranslationValue("ostjob.ch Datum")
			columnostjobchdate.Name = "ostjobchdate"
			columnostjobchdate.FieldName = "ostjobchdate"
			columnostjobchdate.Visible = False
			gvEmployeeProperty.Columns.Add(columnostjobchdate)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPArt.Caption = m_Translate.GetSafeTranslationValue("Kontakt")
			columnPArt.Name = "vakkontakt"
			columnPArt.FieldName = "vakkontakt"
			columnPArt.Visible = False
			gvEmployeeProperty.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPState.Caption = m_Translate.GetSafeTranslationValue("Status")
			columnPState.Name = "vakstate"
			columnPState.FieldName = "vakstate"
			columnPState.Visible = False
			gvEmployeeProperty.Columns.Add(columnPState)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvEmployeeProperty.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub

	''' <summary>
	''' reset Propose grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetProposeGrid()

		gvEmployeeProperty.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvEmployeeProperty.OptionsView.ShowGroupPanel = False
		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True

		gvEmployeeProperty.Columns.Clear()


		rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Vorschläge"))

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "pnr"
			columnmodulname.FieldName = "pnr"
			columnmodulname.Visible = False
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvEmployeeProperty.Columns.Add(columnMANr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnZHDNr)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvEmployeeProperty.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.Name = "zhdname"
			columnZHDName.FieldName = "zhdname"
			columnZHDName.Visible = True
			gvEmployeeProperty.Columns.Add(columnZHDName)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "advisor"
			columnAdvisor.FieldName = "advisor"
			columnAdvisor.Visible = False
			gvEmployeeProperty.Columns.Add(columnAdvisor)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPArt.Caption = m_Translate.GetSafeTranslationValue("Art")
			columnPArt.Name = "p_art"
			columnPArt.FieldName = "p_art"
			columnPArt.Visible = False
			gvEmployeeProperty.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPState.Caption = m_Translate.GetSafeTranslationValue("Status")
			columnPState.Name = "p_state"
			columnPState.FieldName = "p_state"
			columnPState.Visible = False
			gvEmployeeProperty.Columns.Add(columnPState)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvEmployeeProperty.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


	''' <summary>
	''' reset Offers grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetOffersGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Offerte"))

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "ofnr"
			columnmodulname.FieldName = "ofnr"
			columnmodulname.Visible = True
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvEmployeeProperty.Columns.Add(columnMANr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnZHDNr)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvEmployeeProperty.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.Name = "zname"
			columnZHDName.FieldName = "zname"
			columnZHDName.Visible = True
			gvEmployeeProperty.Columns.Add(columnZHDName)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "advisor"
			columnAdvisor.FieldName = "advisor"
			columnAdvisor.Visible = True
			gvEmployeeProperty.Columns.Add(columnAdvisor)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPArt.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnPArt.Name = "customerstreet"
			columnPArt.FieldName = "customerstreet"
			columnPArt.Visible = False
			gvEmployeeProperty.Columns.Add(columnPArt)

			Dim columncustomeraddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomeraddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columncustomeraddress.Name = "customeraddress"
			columncustomeraddress.FieldName = "customeraddress"
			columncustomeraddress.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomeraddress)

			Dim columncustomertelefon As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomertelefon.Caption = m_Translate.GetSafeTranslationValue("Telefon")
			columncustomertelefon.Name = "customertelefon"
			columncustomertelefon.FieldName = "customertelefon"
			columncustomertelefon.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomertelefon)

			Dim columncustomertelefax As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomertelefax.Caption = m_Translate.GetSafeTranslationValue("Telefax")
			columncustomertelefax.Name = "customertelefax"
			columncustomertelefax.FieldName = "customertelefax"
			columncustomertelefax.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomertelefax)

			Dim columncustomeremail As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomeremail.Caption = m_Translate.GetSafeTranslationValue("EMail")
			columncustomeremail.Name = "customeremail"
			columncustomeremail.FieldName = "customeremail"
			columncustomeremail.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomeremail)

			Dim columnztelefon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnztelefon.Caption = m_Translate.GetSafeTranslationValue("ZHD.-Telefon")
			columnztelefon.Name = "ztelefon"
			columnztelefon.FieldName = "ztelefon"
			columnztelefon.Visible = False
			gvEmployeeProperty.Columns.Add(columnztelefon)

			Dim columnzmobile As New DevExpress.XtraGrid.Columns.GridColumn()
			columnzmobile.Caption = m_Translate.GetSafeTranslationValue("ZHD.-Natel")
			columnzmobile.Name = "zmobile"
			columnzmobile.FieldName = "zmobile"
			columnzmobile.Visible = False
			gvEmployeeProperty.Columns.Add(columnzmobile)

			Dim columnzemail As New DevExpress.XtraGrid.Columns.GridColumn()
			columnzemail.Caption = m_Translate.GetSafeTranslationValue("ZHD.-EMail")
			columnzemail.Name = "zemail"
			columnzemail.FieldName = "zemail"
			columnzemail.Visible = False
			gvEmployeeProperty.Columns.Add(columnzemail)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPState.Caption = m_Translate.GetSafeTranslationValue("Status")
			columnPState.Name = "offerstate"
			columnPState.FieldName = "offerstate"
			columnPState.Visible = False
			gvEmployeeProperty.Columns.Add(columnPState)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvEmployeeProperty.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


	''' <summary>
	''' reset ES grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetESGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Einsätze"))

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "esnr"
			columnmodulname.FieldName = "esnr"
			columnmodulname.Visible = False
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvEmployeeProperty.Columns.Add(columnMANr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnZHDNr)

			Dim columnPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPeriode.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnPeriode.Name = "periode"
			columnPeriode.FieldName = "periode"
			columnPeriode.Visible = False
			gvEmployeeProperty.Columns.Add(columnPeriode)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Caption = m_Translate.GetSafeTranslationValue("Als")
			columnESAls.Name = "esals"
			columnESAls.FieldName = "esals"
			columnESAls.Visible = True
			gvEmployeeProperty.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTarif.Caption = m_Translate.GetSafeTranslationValue("Tarif")
			columnTarif.Name = "tarif"
			columnTarif.FieldName = "tarif"
			columnTarif.Visible = False
			columnTarif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTarif.AppearanceHeader.Options.UseTextOptions = True
			columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTarif.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnTarif)

			Dim columnCustomerTagesSpesen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerTagesSpesen.Caption = m_Translate.GetSafeTranslationValue("Kunden-Tagesspesen")
			columnCustomerTagesSpesen.Name = "CustomerTagesSpesen"
			columnCustomerTagesSpesen.FieldName = "CustomerTagesSpesen"
			columnCustomerTagesSpesen.Visible = True
			columnCustomerTagesSpesen.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnCustomerTagesSpesen.AppearanceHeader.Options.UseTextOptions = True
			columnCustomerTagesSpesen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnCustomerTagesSpesen.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnCustomerTagesSpesen)

			Dim columnStundenlohn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStundenlohn.Caption = m_Translate.GetSafeTranslationValue("Stundenlohn")
			columnStundenlohn.Name = "stundenlohn"
			columnStundenlohn.FieldName = "stundenlohn"
			columnStundenlohn.Visible = False
			columnStundenlohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnStundenlohn.AppearanceHeader.Options.UseTextOptions = True
			columnStundenlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnStundenlohn.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnStundenlohn)

			Dim columnEmployeeTagesSpesen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeTagesSpesen.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Tagesspesen")
			columnEmployeeTagesSpesen.Name = "EmployeeTagesSpesen"
			columnEmployeeTagesSpesen.FieldName = "EmployeeTagesSpesen"
			columnEmployeeTagesSpesen.Visible = True
			columnEmployeeTagesSpesen.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnEmployeeTagesSpesen.AppearanceHeader.Options.UseTextOptions = True
			columnEmployeeTagesSpesen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnEmployeeTagesSpesen.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnEmployeeTagesSpesen)

			Dim columnEmployeeStundenSpesen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeStundenSpesen.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Stundenspesen")
			columnEmployeeStundenSpesen.Name = "EmployeeStundenSpesen"
			columnEmployeeStundenSpesen.FieldName = "EmployeeStundenSpesen"
			columnEmployeeStundenSpesen.Visible = True
			columnEmployeeStundenSpesen.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnEmployeeStundenSpesen.AppearanceHeader.Options.UseTextOptions = True
			columnEmployeeStundenSpesen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnEmployeeStundenSpesen.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnEmployeeStundenSpesen)

			Dim columnMargeMitBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeMitBVG.Caption = m_Translate.GetSafeTranslationValue("Marge mit BVG")
			columnMargeMitBVG.Name = "margemitbvg"
			columnMargeMitBVG.FieldName = "margemitbvg"
			columnMargeMitBVG.Visible = False
			gvEmployeeProperty.Columns.Add(columnMargeMitBVG)

			Dim columnMargeOhneBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeOhneBVG.Caption = m_Translate.GetSafeTranslationValue("Marge ohne BVG")
			columnMargeOhneBVG.Name = "margeohnebvg"
			columnMargeOhneBVG.FieldName = "margeohnebvg"
			columnMargeOhneBVG.Visible = False
			gvEmployeeProperty.Columns.Add(columnMargeOhneBVG)

			Dim columnActivES As New DevExpress.XtraGrid.Columns.GridColumn()
			columnActivES.Caption = m_Translate.GetSafeTranslationValue("Aktiv?")
			columnActivES.Name = "actives"
			columnActivES.FieldName = "actives"
			columnActivES.Visible = True
			gvEmployeeProperty.Columns.Add(columnActivES)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvEmployeeProperty.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


	''' <summary>
	''' reset report grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetReportGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Rapporte"))

		Try
			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "rpnr"
			columnmodulname.FieldName = "rpnr"
			columnmodulname.Visible = True
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
			columnLONr.Name = "lonr"
			columnLONr.FieldName = "lonr"
			columnLONr.Visible = True
			gvEmployeeProperty.Columns.Add(columnLONr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvEmployeeProperty.Columns.Add(columnBezeichnung)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
			columnMonth.Name = "monat"
			columnMonth.FieldName = "monat"
			columnMonth.Visible = True
			gvEmployeeProperty.Columns.Add(columnMonth)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnYear.Name = "jahr"
			columnYear.FieldName = "jahr"
			columnYear.Visible = True
			gvEmployeeProperty.Columns.Add(columnYear)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnIsDone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsDone.Caption = m_Translate.GetSafeTranslationValue("Erfasst")
			columnIsDone.Name = "rpdone"
			columnIsDone.FieldName = "rpdone"
			columnIsDone.Visible = True
			gvEmployeeProperty.Columns.Add(columnIsDone)



			Dim columnFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnFiliale.Name = "zfiliale"
			columnFiliale.FieldName = "zfiliale"
			columnFiliale.Visible = False
			gvEmployeeProperty.Columns.Add(columnFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


	''' <summary>
	''' reset invoice grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetInvoiceGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Rechnungen"))

		Try
			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "renr"
			columnmodulname.FieldName = "renr"
			columnmodulname.Visible = False
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCustomerName.Name = "firma1"
			columnCustomerName.FieldName = "firma1"
			columnCustomerName.Visible = False
			gvEmployeeProperty.Columns.Add(columnCustomerName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "plzort"
			columnAddress.FieldName = "plzort"
			columnAddress.Visible = True
			gvEmployeeProperty.Columns.Add(columnAddress)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "fakdate"
			columnFakDat.FieldName = "fakdate"
			columnFakDat.Visible = True
			gvEmployeeProperty.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Default
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "betragink"
			columnBetragInk.FieldName = "betragink"
			columnBetragInk.Visible = True
			columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnBetragInk)

			Dim columnBetragOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragOpen.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Default
			columnBetragOpen.Caption = m_Translate.GetSafeTranslationValue("Offener Betrag")
			columnBetragOpen.Name = "betragopen"
			columnBetragOpen.FieldName = "betragopen"
			columnBetragOpen.Visible = True
			columnBetragOpen.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragOpen.AppearanceHeader.Options.UseTextOptions = True
			columnBetragOpen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragOpen.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnBetragOpen)

			Dim columnIsOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsOpen.Caption = m_Translate.GetSafeTranslationValue("Offen?")
			columnIsOpen.Name = "isopen"
			columnIsOpen.FieldName = "isopen"
			columnIsOpen.Visible = True
			gvEmployeeProperty.Columns.Add(columnIsOpen)

			Dim columnREKst As New DevExpress.XtraGrid.Columns.GridColumn()
			columnREKst.Caption = m_Translate.GetSafeTranslationValue("KST")
			columnREKst.Name = "rekst"
			columnREKst.FieldName = "rekst"
			columnREKst.Visible = True
			gvEmployeeProperty.Columns.Add(columnREKst)

			Dim columnCustomerAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerAdvisor.Caption = m_Translate.GetSafeTranslationValue("Kunden-Berater")
			columnCustomerAdvisor.Name = "customeradvisor"
			columnCustomerAdvisor.FieldName = "customeradvisor"
			columnCustomerAdvisor.Visible = True
			gvEmployeeProperty.Columns.Add(columnCustomerAdvisor)

			Dim columnEmployeeAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeAdvisor.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Berater")
			columnEmployeeAdvisor.Name = "employeeadvisor"
			columnEmployeeAdvisor.FieldName = "employeeadvisor"
			columnEmployeeAdvisor.Visible = True
			gvEmployeeProperty.Columns.Add(columnEmployeeAdvisor)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
			columnmodulname.Visible = False
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnrenr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrenr.Caption = m_Translate.GetSafeTranslationValue("Rechnung-Nr.")
			columnrenr.Name = "renr"
			columnrenr.FieldName = "renr"
			columnrenr.Visible = True
			gvEmployeeProperty.Columns.Add(columnrenr)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCustomerName.Name = "firma1"
			columnCustomerName.FieldName = "firma1"
			columnCustomerName.Visible = False
			gvEmployeeProperty.Columns.Add(columnCustomerName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "plzort"
			columnAddress.FieldName = "plzort"
			columnAddress.Visible = True
			gvEmployeeProperty.Columns.Add(columnAddress)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "valutadate"
			columnFakDat.FieldName = "valutadate"
			columnFakDat.Visible = True
			gvEmployeeProperty.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "zebetrag"
			columnBetragInk.FieldName = "zebetrag"
			columnBetragInk.Visible = True
			columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnBetragInk)

			Dim columnCustomerAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerAdvisor.Caption = m_Translate.GetSafeTranslationValue("Kunden-Berater")
			columnCustomerAdvisor.Name = "customeradvisor"
			columnCustomerAdvisor.FieldName = "customeradvisor"
			columnCustomerAdvisor.Visible = False
			gvEmployeeProperty.Columns.Add(columnCustomerAdvisor)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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



#Region "Open modules"

	Private Sub OpenSelectedVacancies(ByVal vacanciesNumber As Integer, ByVal kdnr As Integer, ByVal zhdnr As Integer, ByVal mandantnumber As Integer)

		Dim frmVacancy = New SPKD.Vakanz.frmVakanzen(m_InitializationData)
		Dim setting = New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = vacanciesNumber, .SelectedKDNr = kdnr, .SelectedZHDNr = zhdnr}
		frmVacancy.VacancySetting = setting
		If Not frmVacancy.LoadData Then Return

		frmVacancy.Show()
		frmVacancy.BringToFront()

	End Sub

	Private Sub OpenSelectedPropose(ByVal proposeNumber As Integer, ByVal mandantnumber As Integer)

		Dim obj As New SPProposeUtility.ClsMain_Net(m_InitializationData)	' New SPProposeUtility.ClsSetting With {.SelectedMDNr = mandantnumber, .LogedUSNr = m_InitializationData.UserData.UserNr})
		obj.ShowfrmProposal(proposeNumber)

	End Sub

	Private Sub OpenSelectedOffer(ByVal offerNumber As Integer, ByVal mandantnumber As Integer)
		Dim oMyProg As Object

		oMyProg = CreateObject("SPSModulsView.ClsMain")
		oMyProg.TranslateProg4Net("OfferUtility.ClsMain", offerNumber)

	End Sub

	Private Sub OpenSelectedEmployee(ByVal employeenumber As Integer, ByVal mandantnumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, employeenumber)
		hub.Publish(openMng)

	End Sub

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


#End Region

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvEmployeeProperty.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Searchedmodul
					Case 1
						HandleRowClickForVacancies(column.Name, dataRow)

					Case 2
						HandleRowClickForPropose(column.Name, dataRow)


					Case 3
						HandleRowClickForOffer(column.Name, dataRow)

					Case 4
						HandleRowClickForES(column.Name, dataRow)

					Case 5
						HandleRowClickForReport(column.Name, dataRow)

					Case 6
						HandleRowClickForInvoice(column.Name, dataRow)

					Case 7
						HandleRowClickForPayment(column.Name, dataRow)


					Case Else
						Exit Sub

				End Select

			End If

		End If

	End Sub

	Sub HandleRowClickForVacancies(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerVacanciesProperty)

			Select Case column.ToLower
				Case "customername", "kdnr"
					If viewData.kdnr > 0 Then OpenSelectedCustomer(viewData.kdnr, viewData.mdnr)

				Case Else
					If viewData.vaknr > 0 Then OpenSelectedVacancies(viewData.vaknr, viewData.kdnr, viewData.kdzhdnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForPropose(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerProposeProperty)

			Select Case column.ToLower
				Case "employeename", "manr"
					If viewData.kdnr > 0 Then OpenSelectedEmployee(viewData.manr, viewData.mdnr)

				Case Else
					If viewData.pnr > 0 Then OpenSelectedPropose(viewData.pnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForOffer(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerOfferProperty)

			Select Case column.ToLower
				Case "customername", "kdnr"
					If viewData.kdnr > 0 Then OpenSelectedCustomer(viewData.kdnr, viewData.mdnr)

				Case Else
					If viewData.ofnr > 0 Then OpenSelectedOffer(viewData.ofnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForES(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerESProperty)

			Select Case column.ToLower
				Case "customername", "kdnr"
					If viewData.kdnr > 0 Then OpenSelectedCustomer(viewData.kdnr, viewData.mdnr)

				Case Else
					If viewData.esnr > 0 Then OpenSelectedES(viewData.esnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForReport(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerReportsProperty)

			Select Case column.ToLower
				Case "employeename", "manr"
					If viewData.manr > 0 Then OpenSelectedEmployee(viewData.manr, viewData.employeeMDNr)

				Case "es_als", "esnr"
					If viewData.esnr > 0 Then OpenSelectedES(viewData.esnr, viewData.mdnr)


				Case Else
					If viewData.rpnr > 0 Then OpenSelectedReport(viewData.rpnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForInvoice(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerInvoiceProperty)

			Select Case column.ToLower

				Case Else
					If viewData.renr > 0 Then OpenSelectedInvoice(viewData.renr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForPayment(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CustomerPaymentProperty)

			If viewData.renr > 0 Then OpenSelectedInvoice(viewData.renr, viewData.mdnr)

		End If

	End Sub

	Private Sub OngvEmployeeProperty_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvEmployeeProperty.CustomColumnDisplayText

		If e.Column.FieldName = "betragohne" OrElse e.Column.FieldName = "betragex" OrElse e.Column.FieldName = "betragink" OrElse e.Column.FieldName = "mwst1" OrElse
				e.Column.FieldName = "mwstproz" OrElse e.Column.FieldName = "bezahlt" OrElse e.Column.FieldName = "EmployeeStundenSpesen" OrElse e.Column.FieldName = "EmployeeTagesSpesen" OrElse
				e.Column.FieldName = "stundenlohn" OrElse e.Column.FieldName = "CustomerTagesSpesen" OrElse e.Column.FieldName = "tarif" OrElse e.Column.FieldName = "margemitbvg" OrElse
				e.Column.FieldName = "margeohnebvg" OrElse e.Column.FieldName = "betragopen" OrElse e.Column.FieldName = "zebetrag" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

#End Region



#Region "Public Methods"

	Public Sub LoadData(ByVal customerNumber As Integer, ByVal modulName As String)

		m_CustomerNumber = customerNumber

		Select Case modulName.ToLower
			Case "show_vacancies".ToLower
				Searchedmodul = 1
				ResetVacanciesGrid()
				LoadFoundedCustomerVacanciesList(m_CustomerNumber)

			Case "show_propose".ToLower
				Searchedmodul = 2
				ResetProposeGrid()
				LoadFoundedCustomerProposeList(m_CustomerNumber)

			Case "show_offers".ToLower
				Searchedmodul = 3
				ResetOffersGrid()
				LoadFoundedCusotmerOfferList(m_CustomerNumber)

			Case "show_es".ToLower
				Searchedmodul = 4
				ResetESGrid()
				LoadFoundedCustomerESList(m_CustomerNumber)

			Case "show_reports".ToLower
				Searchedmodul = 5
				ResetReportGrid()
				LoadFoundedCustomerReportsList(m_CustomerNumber)

			Case "show_invoice".ToLower
				chkOffeneInvoice.Visible = True
				Searchedmodul = 6
				ResetInvoiceGrid()
				LoadFoundedCustomerInvoiceList(m_CustomerNumber)

			Case "show_payment".ToLower
				Searchedmodul = 7
				ResetPaymentGrid()
				LoadFoundedCustomerPayrmentList(m_CustomerNumber)


			Case Else
				' Do nothing
		End Select

	End Sub

	Public Function LoadFoundedCustomerVacanciesList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfPropose = m_CustomerDatabaseAccess.LoadFoundedVacanciesForCustomerMng(customerNumber)
		If listOfPropose Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Vakanzen-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfPropose
		Select New CustomerVacanciesProperty With
					 {.mdnr = report.mdnr,
						.vaknr = report.vaknr,
						.kdnr = report.kdnr,
						.kdzhdnr = report.kdzhdnr,
						.vakstate = report.vakstate,
						.vak_kanton = report.vak_kanton,
						.vaklink = report.vaklink,
						.vakkontakt = report.vakkontakt,
						.vacancygruppe = report.vacancygruppe,
						.vacancyplz = report.vacancyplz,
						.vacancyort = report.vacancyort,
						.titelforsearch = report.titelforsearch,
						.shortdescription = report.shortdescription,
						.firma1 = report.firma1,
						.bezeichnung = report.bezeichnung,
						.createdon = report.createdon,
						.createdfrom = report.createdfrom,
						.kdzname = report.kdzname,
						.advisor = report.advisor,
						.kdemail = report.kdemail,
						.zemail = report.zemail,
						.jchisonline = report.jchisonline,
						.ojisonline = report.ojisonline,
						.ourisonline = report.ourisonline,
						.kdtelefon = report.kdtelefon,
						.kdtelefax = report.kdtelefax,
						.ztelefon = report.ztelefon,
						.ztelefax = report.ztelefax,
						.znatel = report.znatel,
						.jobchdate = report.jobchdate,
						.ostjobchdate = report.ostjobchdate,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of CustomerVacanciesProperty) = New BindingList(Of CustomerVacanciesProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfPropose.Count

		Return Not listOfPropose Is Nothing
	End Function

	Public Function LoadFoundedCustomerProposeList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfPropose = m_CustomerDatabaseAccess.LoadFoundedProposeForCustomerMng(customerNumber)
		If listOfPropose Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Vorschlag-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfPropose
		Select New CustomerProposeProperty With
					 {.mdnr = report.mdnr,
						.pnr = report.pnr,
						.manr = report.manr,
						.kdnr = report.kdnr,
						.zhdnr = report.zhdnr,
						.employeename = report.employeename,
						.customername = report.customername,
						.bezeichnung = report.bezeichnung,
						.zhdname = report.zhdname,
						.p_art = report.p_art,
						.p_state = report.p_state,
						.advisor = report.advisor,
						.createdfrom = report.createdfrom,
						.createdon = report.createdon,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of CustomerProposeProperty) = New BindingList(Of CustomerProposeProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfPropose.Count

		Return Not listOfPropose Is Nothing
	End Function

	Public Function LoadFoundedCusotmerOfferList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfOffers = m_CustomerDatabaseAccess.LoadFoundedOfferForcustomerMng(customerNumber)
		If listOfOffers Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Offerten-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfOffers
		Select New CustomerOfferProperty With
					 {.mdnr = report.mdnr,
						.ofnr = report.ofnr,
						.kdnr = report.kdnr,
						.zhdnr = report.zhdnr,
						.manr = report.manr,
						.employeename = report.employeename,
						.bezeichnung = report.bezeichnung,
						.createdon = report.createdon,
						.createdfrom = report.createdfrom,
						.offerstate = report.offerstate,
						.customername = report.customername,
						.customerstreet = report.customerstreet,
						.customeraddress = report.customeraddress,
						.customertelefon = report.customertelefon,
						.customertelefax = report.customertelefax,
						.customeremail = report.customeremail,
						.zname = report.zname,
						.ztelefon = report.ztelefon,
						.zmobile = report.zmobile,
						.zemail = report.zemail,
						.advisor = report.advisor,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of CustomerOfferProperty) = New BindingList(Of CustomerOfferProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfOffers.Count

		Return Not listOfOffers Is Nothing
	End Function

	Public Function LoadFoundedCustomerESList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfES = m_CustomerDatabaseAccess.LoadFoundedESForCustomerMng(customerNumber)
		If listOfES Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfES
							  Select New CustomerESProperty With
					 {.mdnr = report.mdnr,
						.esnr = report.esnr,
						.manr = report.manr,
						.kdnr = report.kdnr,
						.zhdnr = report.zhdnr,
						.periode = report.periode,
						.employeename = report.employeename,
						.customername = report.customername,
						.esals = report.esals,
						.tarif = report.tarif,
						.stundenlohn = report.stundenlohn,
						.EmployeeStundenSpesen = report.EmployeeStundenSpesen,
						.EmployeeTagesSpesen = report.EmployeeTagesSpesen,
						.CustomerTagesSpesen = report.CustomerTagesSpesen,
						.margemitbvg = report.margemitbvg,
						.margeohnebvg = report.margeohnebvg,
						.actives = report.actives,
						.createdfrom = report.createdfrom,
						.createdon = report.createdon,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of CustomerESProperty) = New BindingList(Of CustomerESProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfES.Count

		Return Not listOfES Is Nothing
	End Function

	''' <summary>
	''' Loads Reports for selected employee
	''' </summary>
	''' <param name="customerNumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedCustomerReportsList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfReports = m_CustomerDatabaseAccess.LoadFoundedRPForcustomerMng(customerNumber)
		If listOfReports Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfReports
		Select New CustomerReportsProperty With
					 {.mdnr = report.mdnr,
						.employeeMDNr = report.employeeMDNr,
						.customerMDNr = report.customerMDNr,
						.rpnr = report.rpnr,
						.lonr = report.lonr,
						.esnr = report.esnr,
						.manr = report.manr,
						.kdnr = report.kdnr,
						.monat = report.monat,
						.jahr = report.jahr,
						.periode = report.periode,
						.employeename = report.employeename,
						.customername = report.customername,
						.rpgav_beruf = report.rpgav_beruf,
						.rpdone = report.rpdone,
						.createdon = report.createdon,
						.createdfrom = report.createdfrom,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of CustomerReportsProperty) = New BindingList(Of CustomerReportsProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfReports.Count

		Return Not listOfReports Is Nothing
	End Function


	''' <summary>
	''' Loads AdvancePayment for selected employee
	''' </summary>
	''' <param name="customerNumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedCustomerInvoiceList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfAdvancePayment = m_CustomerDatabaseAccess.LoadFoundedInvoiceForCustomerMng(customerNumber, chkOffeneInvoice.Checked)
		If listOfAdvancePayment Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Debitoren-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfAdvancePayment
		Select New CustomerInvoiceProperty With
					 {.mdnr = report.mdnr,
						.customerMDNr = report.customerMDNr,
						.renr = report.renr,
						.kdnr = report.kdnr,
						.firma1 = report.firma1,
						.plzort = report.plzort,
						.fakdate = report.fakdate,
						.betragink = report.betragink,
						.betragopen = report.betragopen,
						.rekst = report.rekst,
						.isopen = report.isopen,
						.customeradvisor = report.customeradvisor,
						.employeeadvisor = report.employeeadvisor,
						.createdon = report.createdon,
						.createdfrom = report.createdfrom,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of CustomerInvoiceProperty) = New BindingList(Of CustomerInvoiceProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfAdvancePayment.Count

		Return Not listOfAdvancePayment Is Nothing
	End Function


	''' <summary>
	''' Loads Payroll for selected employee
	''' </summary>
	''' <param name="customerNumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedCustomerPayrmentList(ByVal customerNumber As Integer?) As Boolean

		Dim listOfPayroll = m_CustomerDatabaseAccess.LoadFoundedPaymentForCustomerMng(customerNumber)
		If listOfPayroll Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Zahlungseingang-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfPayroll
		Select New CustomerPaymentProperty With
					 {.mdnr = report.mdnr,
						.customerMDNr = report.customerMDNr,
						.customeradvisor = report.customeradvisor,
						.firma1 = report.firma1,
						.plzort = report.plzort,
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

		Dim listDataSource As BindingList(Of CustomerPaymentProperty) = New BindingList(Of CustomerPaymentProperty)

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
			Case 1
				Try
					If File.Exists(m_GVVacanciesSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVVacanciesSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 2
				Try
					If File.Exists(m_GVProposeSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVProposeSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case 3
				Try
					If File.Exists(m_GVOffersSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVOffersSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 4
				Try
					If File.Exists(m_GVESSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVESSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 5
				Try
					If File.Exists(m_GVReportsSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVReportsSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 6
				Try
					If File.Exists(m_GVInvoiceSettingfilenameWithCustomer) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVInvoiceSettingfilenameWithCustomer)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



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
			Case 1
				gvEmployeeProperty.SaveLayoutToXml(m_GVVacanciesSettingfilenameWithCustomer)

			Case 2
				gvEmployeeProperty.SaveLayoutToXml(m_GVProposeSettingfilenameWithCustomer)

			Case 3
				gvEmployeeProperty.SaveLayoutToXml(m_GVOffersSettingfilenameWithCustomer)

			Case 4
				gvEmployeeProperty.SaveLayoutToXml(m_GVESSettingfilenameWithCustomer)

			Case 5
				gvEmployeeProperty.SaveLayoutToXml(m_GVReportsSettingfilenameWithCustomer)

			Case 6
				gvEmployeeProperty.SaveLayoutToXml(m_GVInvoiceSettingfilenameWithCustomer)

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
