
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Common

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SP.MA.ApplicantMng.Settings
'Imports SP.DatabaseAccess.ES.DataObjects.ESMng
'Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid


''' <summary>
''' Shows founded Reports records..
''' </summary>
Public Class frmEmployeesProperties

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	'Private m_ESDatabaseAccess As IESDatabaseAccess

	''' <summary>
	''' Contains the employee number of the loaded employee data.
	''' </summary>
	Private m_EmployeeNumber As Integer?

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
	Private m_GVESSettingfilenameWithEmployee As String
	Private m_GVProposeSettingfilenameWithEmployee As String
	Private m_GVOffersSettingfilenameWithEmployee As String
	Private m_GVAdvancePaymentSettingfilenameWithEmployee As String
	Private m_GVPayrollSettingfilenameWithEmployee As String
	Private m_GVReportsSettingfilenameWithEmployee As String
	Private m_GVDocumentSettingfilenameWithEmployee As String

	Private Property Searchedmodul As Integer
	Private Property m_EmployeeImage As Image

#End Region


#Region "private consts"

	Private Const MODUL_Propose As Integer = 1
	Private Const MODUL_Offers As Integer = 2

	Private Const MODUL_ES As Integer = 3
	Private Const MODUL_RP As Integer = 4
	Private Const MODUL_ZG As Integer = 5

	Private Const MODUL_LO As Integer = 6
	Private Const MODUL_DOCUMENT As Integer = 7

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass,
								 ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper,
								 ByVal employeeNumber As Integer?,
								 ByVal employeeimage As Image)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		Searchedmodul = MODUL_Propose

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = translate
		m_SettingsManager = New SettingsManager

		m_EmployeeNumber = employeeNumber
		m_EmployeeImage = employeeimage

		connectionString = m_InitializationData.MDData.MDDbConn
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		Try
			GridSettingPath = String.Format("{0}EmployeeMng\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
			If Not Directory.Exists(GridSettingPath) Then Directory.CreateDirectory(GridSettingPath)
			If Not Directory.Exists(String.Format("{0}Properties\", GridSettingPath)) Then Directory.CreateDirectory(String.Format("{0}Properties\", GridSettingPath))

			m_GVProposeSettingfilenameWithEmployee = String.Format("{0}Properties\Propose_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVOffersSettingfilenameWithEmployee = String.Format("{0}Properties\Offers_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

			m_GVESSettingfilenameWithEmployee = String.Format("{0}Properties\ES_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVReportsSettingfilenameWithEmployee = String.Format("{0}Properties\Reports_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

			m_GVAdvancePaymentSettingfilenameWithEmployee = String.Format("{0}Properties\AdvancePayment_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)
			m_GVPayrollSettingfilenameWithEmployee = String.Format("{0}Properties\Payroll_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

			m_GVDocumentSettingfilenameWithEmployee = String.Format("{0}Properties\Document_WithEmployee{1}.xml", GridSettingPath, m_InitializationData.UserData.UserNr)

		Catch ex As Exception

		End Try

		' Translate controls.
		TranslateControls()

		' Creates the navigation bar.
		CreateMyNavBar()

		'ResetProposeGrid()
		'LoadFoundedEmployeeProposeList(m_EmployeeNumber)

		AddHandler gvEmployeeProperty.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvEmployeeProperty.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvEmployeeProperty.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler gvEmployeeProperty.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub


#End Region


#Region "Private Properties"

	''' <summary>
	''' Gets the selected Propose data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedProposeViewData As EmployeeProposeProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim propose = CType(grdView.GetRow(selectedRows(0)), EmployeeProposeProperty)
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
	Private ReadOnly Property SelectedOfferViewData As EmployeeOfferProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim offer = CType(grdView.GetRow(selectedRows(0)), EmployeeOfferProperty)
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
	Private ReadOnly Property SelectedReportsViewData As EmployeeReportsProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim report = CType(grdView.GetRow(selectedRows(0)), EmployeeReportsProperty)
					Return report
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected AdvancePayment data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedAdvancePaymentViewData As EmployeeAdvancePaymentProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim AdvancePayment = CType(grdView.GetRow(selectedRows(0)), EmployeeAdvancePaymentProperty)
					Return AdvancePayment
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected Payroll data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedPayrollViewData As EmployeePayrollProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim Payroll = CType(grdView.GetRow(selectedRows(0)), EmployeePayrollProperty)
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
	Private ReadOnly Property SelectedDocumentViewData As EmployeePrintedDocProperty
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim PrintedDocument = CType(grdView.GetRow(selectedRows(0)), EmployeePrintedDocProperty)
					Return PrintedDocument
				End If

			End If

			Return Nothing
		End Get

	End Property



#End Region


#Region "Private Methods"

	Private Sub ResetImage(ByVal employeeimage As Image)
		Try
			m_EmployeeImage = employeeimage

			employeePicture.Image = employeeimage
			employeePicture.Properties.ShowMenu = False
			employeePicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnchkDocumentFromThisYear_CheckedChanged(sender As Object, e As EventArgs) Handles chkDocumentFromThisYear.CheckedChanged
		If chkDocumentFromThisYear.Checked Then
			Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Gedruckte Dokumente aktuelles Jahr"))
		Else
			Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Gedruckte Dokumente"))
		End If
		LoadFoundedEmployeeDocumentsList(m_EmployeeNumber)
	End Sub

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.rlblHeader.Text = m_Translate.GetSafeTranslationValue(Me.rlblHeader.Text)
		Me.chkDocumentFromThisYear.Text = m_Translate.GetSafeTranslationValue(Me.chkDocumentFromThisYear.Text)
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

			Dim nbiPropose As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschläge"))
			nbiPropose.Name = "Show_Propose"

			Dim nbiOffers As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Offerte"))
			nbiOffers.Name = "Show_Offers"

			Dim nbiES As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsätze"))
			nbiES.Name = "Show_ES"

			Dim nbiReports As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rapporte"))
			nbiReports.Name = "Show_Reports"

			Dim nbiAdvancePayment As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschüsse"))
			nbiAdvancePayment.Name = "Show_AdvancePayment"

			Dim nbiPayroll As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Lohnabrechnungen"))
			nbiPayroll.Name = "Show_Payroll"

			Dim groupPrintModule As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Druck-Aufträge"))
			groupPrintModule.Name = "gNavPrintJobs"

			Dim nbiPrintArg As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Dokumente auflisten"))
			nbiPrintArg.Name = "Show_Print"

			nbiPropose.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 801, m_InitializationData.MDData.MDNr)
			nbiOffers.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 801, m_InitializationData.MDData.MDNr)
			nbiES.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 250, m_InitializationData.MDData.MDNr)
			nbiReports.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 300, m_InitializationData.MDData.MDNr)
			nbiAdvancePayment.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 349, m_InitializationData.MDData.MDNr)
			nbiPayroll.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 16, m_InitializationData.MDData.MDNr)
			nbiPrintArg.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 104, m_InitializationData.MDData.MDNr)

			Try
				navMain.BeginUpdate()

				navMain.Groups.Add(groupModule)
				groupModule.ItemLinks.Add(nbiPropose)
				groupModule.ItemLinks.Add(nbiOffers)

				groupModule.ItemLinks.Add(nbiES)
				groupModule.ItemLinks.Add(nbiReports)
				groupModule.ItemLinks.Add(nbiAdvancePayment)
				groupModule.ItemLinks.Add(nbiPayroll)

				groupModule.Expanded = True

				navMain.Groups.Add(groupPrintModule)
				groupPrintModule.ItemLinks.Add(nbiPrintArg)

				groupPrintModule.Expanded = True

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
			Me.chkDocumentFromThisYear.Visible = False

			LoadData(m_EmployeeNumber, strLinkName.ToLower, m_EmployeeImage)

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
			columncustomername.Visible = True
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
			columnEmployeename.Visible = False
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
			columnPArt.Caption = m_Translate.GetSafeTranslationValue("Art")
			columnPArt.Name = "p_art"
			columnPArt.FieldName = "p_art"
			columnPArt.Visible = True
			gvEmployeeProperty.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.Caption = m_Translate.GetSafeTranslationValue("Status")
			columnPState.Name = "p_state"
			columnPState.FieldName = "p_state"
			columnPState.Visible = True
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
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
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
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
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
			columnEmployeename.Visible = False
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
			columncustomeraddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomeraddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columncustomeraddress.Name = "customeraddress"
			columncustomeraddress.FieldName = "customeraddress"
			columncustomeraddress.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomeraddress)

			Dim columncustomertelefon As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomertelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomertelefon.Caption = m_Translate.GetSafeTranslationValue("Telefon")
			columncustomertelefon.Name = "customertelefon"
			columncustomertelefon.FieldName = "customertelefon"
			columncustomertelefon.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomertelefon)

			Dim columncustomertelefax As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomertelefax.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomertelefax.Caption = m_Translate.GetSafeTranslationValue("Telefax")
			columncustomertelefax.Name = "customertelefax"
			columncustomertelefax.FieldName = "customertelefax"
			columncustomertelefax.Visible = False
			gvEmployeeProperty.Columns.Add(columncustomertelefax)

			Dim columncustomeremail As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomeremail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
			columnzmobile.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnzmobile.Caption = m_Translate.GetSafeTranslationValue("ZHD.-Natel")
			columnzmobile.Name = "zmobile"
			columnzmobile.FieldName = "zmobile"
			columnzmobile.Visible = False
			gvEmployeeProperty.Columns.Add(columnzmobile)

			Dim columnzemail As New DevExpress.XtraGrid.Columns.GridColumn()
			columnzemail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnzemail.Caption = m_Translate.GetSafeTranslationValue("ZHD.-EMail")
			columnzemail.Name = "zemail"
			columnzemail.FieldName = "zemail"
			columnzemail.Visible = False
			gvEmployeeProperty.Columns.Add(columnzemail)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
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
	''' reset report grid
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

			Dim columnPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPeriode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPeriode.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnPeriode.Name = "periode"
			columnPeriode.FieldName = "periode"
			columnPeriode.Visible = True
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
			columncustomername.Visible = True
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTarif.Caption = m_Translate.GetSafeTranslationValue("Tarif")
			columnTarif.Name = "tarif"
			columnTarif.FieldName = "tarif"
			columnTarif.Visible = True
			columnTarif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTarif.AppearanceHeader.Options.UseTextOptions = True
			columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTarif.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnTarif)

			Dim columnStundenlohn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStundenlohn.Caption = m_Translate.GetSafeTranslationValue("Stundenlohn")
			columnStundenlohn.Name = "stundenlohn"
			columnStundenlohn.FieldName = "stundenlohn"
			columnStundenlohn.Visible = True
			columnStundenlohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnStundenlohn.AppearanceHeader.Options.UseTextOptions = True
			columnStundenlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnStundenlohn.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnStundenlohn)

			Dim columnMargeMitBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeMitBVG.Caption = m_Translate.GetSafeTranslationValue("Marge mit BVG")
			columnMargeMitBVG.Name = "margemitbvg"
			columnMargeMitBVG.FieldName = "margemitbvg"
			columnMargeMitBVG.Visible = False
			columnMargeMitBVG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnMargeMitBVG.AppearanceHeader.Options.UseTextOptions = True
			columnMargeMitBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMargeMitBVG.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnMargeMitBVG)

			Dim columnMargeOhneBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeOhneBVG.Caption = m_Translate.GetSafeTranslationValue("Marge ohne BVG")
			columnMargeOhneBVG.Name = "margeohnebvg"
			columnMargeOhneBVG.FieldName = "margeohnebvg"
			columnMargeOhneBVG.Visible = False
			columnMargeOhneBVG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnMargeOhneBVG.AppearanceHeader.Options.UseTextOptions = True
			columnMargeOhneBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMargeOhneBVG.DisplayFormat.FormatString = "N2"
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
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
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
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvEmployeeProperty.Columns.Add(columnBezeichnung)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
			columnMonth.Name = "monat"
			columnMonth.FieldName = "monat"
			columnMonth.Visible = False
			gvEmployeeProperty.Columns.Add(columnMonth)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnYear.Name = "jahr"
			columnYear.FieldName = "jahr"
			columnYear.Visible = False
			gvEmployeeProperty.Columns.Add(columnYear)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvEmployeeProperty.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnrpgav_beruf As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrpgav_beruf.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnrpgav_beruf.Caption = m_Translate.GetSafeTranslationValue("GAV-Beruf")
			columnrpgav_beruf.Name = "rpgav_beruf"
			columnrpgav_beruf.FieldName = "rpgav_beruf"
			columnrpgav_beruf.Visible = True
			gvEmployeeProperty.Columns.Add(columnrpgav_beruf)

			Dim columnIsDone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsDone.Caption = m_Translate.GetSafeTranslationValue("Erfasst")
			columnIsDone.Name = "rpdone"
			columnIsDone.FieldName = "rpdone"
			columnIsDone.Visible = True
			gvEmployeeProperty.Columns.Add(columnIsDone)

			Dim columnFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
	''' reset AdvancePayment grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAdvancePaymentGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Vorschüsse"))

		Try
			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zgnr"
			columnmodulname.FieldName = "zgnr"
			columnmodulname.Visible = True
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvEmployeeProperty.Columns.Add(columnMANr)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
			columnLONr.Name = "lonr"
			columnLONr.FieldName = "lonr"
			columnLONr.Visible = True
			gvEmployeeProperty.Columns.Add(columnLONr)

			Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPNr.Caption = m_Translate.GetSafeTranslationValue("Rapport-Nr.")
			columnRPNr.Name = "rpnr"
			columnRPNr.FieldName = "rpnr"
			columnRPNr.Visible = False
			gvEmployeeProperty.Columns.Add(columnRPNr)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLAName.Caption = m_Translate.GetSafeTranslationValue("Auszahlungsart")
			columnLAName.Name = "laname"
			columnLAName.FieldName = "laname"
			columnLAName.Visible = True
			gvEmployeeProperty.Columns.Add(columnLAName)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "betrag"
			columnBetrag.FieldName = "betrag"
			columnBetrag.Visible = True
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvEmployeeProperty.Columns.Add(columnBetrag)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvEmployeeProperty.Columns.Add(columnEmployeename)

			Dim columnZGPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZGPeriode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZGPeriode.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
			columnZGPeriode.Name = "zgperiode"
			columnZGPeriode.FieldName = "zgperiode"
			columnZGPeriode.Visible = True
			gvEmployeeProperty.Columns.Add(columnZGPeriode)

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "lanr"
			columnLANr.FieldName = "lanr"
			columnLANr.Visible = False
			gvEmployeeProperty.Columns.Add(columnLANr)

			Dim columnAusDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAusDat.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnAusDat.Name = "aus_dat"
			columnAusDat.FieldName = "aus_dat"
			columnAusDat.Visible = True
			gvEmployeeProperty.Columns.Add(columnAusDat)

			Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
			columnMonat.Name = "monat"
			columnMonat.FieldName = "monat"
			columnMonat.Visible = False
			gvEmployeeProperty.Columns.Add(columnMonat)

			Dim columnJahr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnJahr.Name = "jahr"
			columnJahr.FieldName = "jahr"
			columnJahr.Visible = False
			gvEmployeeProperty.Columns.Add(columnJahr)

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
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


	''' <summary>
	''' reset Payroll grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetPayrollGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Lohnabrechnungen"))

		Try
			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "lonr"
			columnmodulname.FieldName = "lonr"
			columnmodulname.Visible = True
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvEmployeeProperty.Columns.Add(columnBezeichnung)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
			columnMonth.Name = "monat"
			columnMonth.FieldName = "monat"
			columnMonth.Visible = False
			gvEmployeeProperty.Columns.Add(columnMonth)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnYear.Name = "jahr"
			columnYear.FieldName = "jahr"
			columnYear.Visible = False
			gvEmployeeProperty.Columns.Add(columnYear)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvEmployeeProperty.Columns.Add(columnEmployeename)

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
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
			columnCreatedFrom.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


	''' <summary>
	''' reset document grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetDocumentGrid()

		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployeeProperty.Columns.Clear()

		Me.rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", m_Translate.GetSafeTranslationValue("Gedruckte Dokumente"))

		Try
			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnmodulname.Name = "recid"
			columnmodulname.FieldName = "recid"
			columnmodulname.Visible = False
			gvEmployeeProperty.Columns.Add(columnmodulname)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnBezeichnung.Name = "manr"
			columnBezeichnung.FieldName = "manr"
			columnBezeichnung.Visible = False
			gvEmployeeProperty.Columns.Add(columnBezeichnung)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMonth.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnMonth.Name = "docname"
			columnMonth.FieldName = "docname"
			columnMonth.Visible = True
			gvEmployeeProperty.Columns.Add(columnMonth)


			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnYear.Caption = m_Translate.GetSafeTranslationValue("Benutzer")
			columnYear.Name = "username"
			columnYear.FieldName = "username"
			columnYear.Visible = True
			gvEmployeeProperty.Columns.Add(columnYear)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Dokument")
			columnEmployeename.Name = "scandoc"
			columnEmployeename.FieldName = "scandoc"
			columnEmployeename.Visible = False
			gvEmployeeProperty.Columns.Add(columnEmployeename)

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

	Private Sub OpenSelectedPropose(ByVal proposeNumber As Integer, ByVal mandantnumber As Integer)

		Dim obj As New SPProposeUtility.ClsMain_Net(m_InitializationData)
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
	Private Sub OpenSelectedAdvancePayment(ByVal advancePaymentNumber As Integer, ByVal mandantnumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, mandantnumber, advancePaymentNumber)
		hub.Publish(openMng)

	End Sub

	Private Sub OpenSelectedPayroll(ByVal payrollNumber As Integer, ByVal mandantnumber As Integer)

		Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.SelectedMDNr = mandantnumber,
																															 .SelectedMANr = New List(Of Integer)(New Integer() {0}),
																															 .SelectedLONr = New List(Of Integer)(New Integer() {payrollNumber}),
																															 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
																															 .SelectedYear = New List(Of Integer)(New Integer() {0}),
																															 .SearchAutomatic = True}
		Dim obj As New SP.LO.PrintUtility.ClsMain_Net(m_InitializationData, _settring)
		obj.ShowfrmLO4Details()

	End Sub

	Private Sub OpenSelectedDocument(ByVal recNr As Integer, ByVal bytes() As Byte)

		Dim selectedDocumentData = SelectedDocumentViewData

		Try

			If Not bytes Is Nothing Then

				Dim tempFileName = System.IO.Path.GetTempFileName()
				Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

				If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

					m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub


#End Region

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvEmployeeProperty.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Searchedmodul
					Case 1
						HandleRowClickForPropose(column.Name, dataRow)

					Case 2
						HandleRowClickForOffer(column.Name, dataRow)

					Case 3
						HandleRowClickForES(column.Name, dataRow)

					Case 4
						HandleRowClickForReport(column.Name, dataRow)

					Case 5
						HandleRowClickForAdvancePayment(column.Name, dataRow)

					Case 6
						HandleRowClickForPayroll(column.Name, dataRow)

					Case 7
						HandleRowClickForDocument(column.Name, dataRow)


					Case Else
						Exit Sub

				End Select

			End If

		End If

	End Sub

	Sub HandleRowClickForPropose(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, EmployeeProposeProperty)

			Select Case column.ToLower
				Case "customername", "kdnr"
					If viewData.kdnr > 0 Then OpenSelectedCustomer(viewData.kdnr, viewData.mdnr)

				Case Else
					If viewData.pnr > 0 Then OpenSelectedPropose(viewData.pnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForOffer(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, EmployeeOfferProperty)

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
			Dim viewData = CType(dataRow, EmployeeESProperty)

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
			Dim viewData = CType(dataRow, EmployeeReportsProperty)

			Select Case column.ToLower
				Case "customername", "kdnr"
					If viewData.kdnr > 0 Then OpenSelectedCustomer(viewData.kdnr, viewData.mdnr)

				Case "es_als", "esnr"
					If viewData.esnr > 0 Then OpenSelectedES(viewData.esnr, viewData.mdnr)

				Case "lonr"
					If viewData.lonr > 0 Then OpenSelectedPayroll(viewData.lonr, viewData.mdnr)

				Case Else
					If viewData.rpnr > 0 Then OpenSelectedReport(viewData.rpnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForAdvancePayment(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, EmployeeAdvancePaymentProperty)

			Select Case column.ToLower
				Case "rpgav_beruf", "rpnr"
					If viewData.rpnr > 0 Then OpenSelectedReport(viewData.rpnr, viewData.mdnr)

				Case "lonr"
					If viewData.lonr > 0 Then OpenSelectedPayroll(viewData.lonr, viewData.mdnr)

				Case Else
					If viewData.zgnr > 0 Then OpenSelectedAdvancePayment(viewData.zgnr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForPayroll(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, EmployeePayrollProperty)

			Select Case column.ToLower
				Case "lonr"
					If viewData.lonr > 0 Then OpenSelectedPayroll(viewData.lonr, viewData.mdnr)

				Case Else
					If viewData.lonr > 0 Then OpenSelectedPayroll(viewData.lonr, viewData.mdnr)

			End Select

		End If

	End Sub

	Sub HandleRowClickForDocument(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, EmployeePrintedDocProperty)

			If viewData.recID > 0 Then OpenSelectedDocument(viewData.recID, viewData.scandoc)

		End If

	End Sub


#End Region



#Region "Public Methods"

	Public Sub LoadData(ByVal employeeNumber As Integer, ByVal modulName As String, ByVal img As Image)

		If employeeNumber <> m_EmployeeNumber Then
			ResetImage(img)
		End If
		m_EmployeeNumber = employeeNumber

		Select Case modulName.ToLower
			Case "show_propose".ToLower
				Searchedmodul = 1
				ResetProposeGrid()
				LoadFoundedEmployeeProposeList(m_EmployeeNumber)

			Case "show_offers".ToLower
				Searchedmodul = 2
				ResetOffersGrid()
				LoadFoundedEmployeeOfferList(m_EmployeeNumber)

			Case "show_es".ToLower
				Searchedmodul = 3
				ResetESGrid()
				LoadFoundedEmployeeESList(m_EmployeeNumber)

			Case "show_reports".ToLower
				Searchedmodul = 4
				ResetReportGrid()
				LoadFoundedEmployeeReportsList(m_EmployeeNumber)

			Case "show_advancepayment".ToLower
				Searchedmodul = 5
				ResetAdvancePaymentGrid()
				LoadFoundedEmployeeAdvancePaymentList(m_EmployeeNumber)

			Case "show_payroll".ToLower
				Searchedmodul = 6
				ResetPayrollGrid()
				LoadFoundedEmployeePayrollList(m_EmployeeNumber)


			Case "show_print".ToLower
				Searchedmodul = 7
				ResetDocumentGrid()
				LoadFoundedEmployeeDocumentsList(m_EmployeeNumber)
				Me.chkDocumentFromThisYear.Visible = True

			Case Else
				' Do nothing
		End Select

	End Sub

	Public Function LoadFoundedEmployeeProposeList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfPropose = m_EmployeeDatabaseAccess.LoadFoundedProposeForEmployeeMng(employeeNumber)
		If listOfPropose Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Vorschlag-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfPropose
		Select New EmployeeProposeProperty With
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

		Dim listDataSource As BindingList(Of EmployeeProposeProperty) = New BindingList(Of EmployeeProposeProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfPropose.Count

		Return Not listOfPropose Is Nothing
	End Function

	Public Function LoadFoundedEmployeeOfferList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfOffers = m_EmployeeDatabaseAccess.LoadFoundedOfferForEmployeeMng(employeeNumber)
		If listOfOffers Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Offerten-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfOffers
		Select New EmployeeOfferProperty With
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

		Dim listDataSource As BindingList(Of EmployeeOfferProperty) = New BindingList(Of EmployeeOfferProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfOffers.Count

		Return Not listOfOffers Is Nothing
	End Function

	Public Function LoadFoundedEmployeeESList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfES = m_EmployeeDatabaseAccess.LoadFoundedESForEmployeeMng(employeeNumber)
		If listOfES Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfES
		Select New EmployeeESProperty With
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
						.margemitbvg = report.margemitbvg,
						.margeohnebvg = report.margeohnebvg,
						.actives = report.actives,
						.createdfrom = report.createdfrom,
						.createdon = report.createdon,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of EmployeeESProperty) = New BindingList(Of EmployeeESProperty)

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
	''' <param name="employeenumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedEmployeeReportsList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfReports = m_EmployeeDatabaseAccess.LoadFoundedRPForEmployeeMng(employeeNumber)
		If listOfReports Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfReports
		Select New EmployeeReportsProperty With
					 {.mdnr = report.mdnr,
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

		Dim listDataSource As BindingList(Of EmployeeReportsProperty) = New BindingList(Of EmployeeReportsProperty)

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
	''' <param name="employeenumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedEmployeeAdvancePaymentList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfAdvancePayment = m_EmployeeDatabaseAccess.LoadFoundedAdvancePaymentForEmployeeMng(employeeNumber)
		If listOfAdvancePayment Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfAdvancePayment
		Select New EmployeeAdvancePaymentProperty With
					 {.mdnr = report.mdnr,
						.zgnr = report.zgnr,
						.rpnr = report.rpnr,
						.manr = report.manr,
						.vgnr = report.vgnr,
						.lonr = report.lonr,
						.monat = report.monat,
						.jahr = report.jahr,
						.betrag = report.betrag,
						.employeename = report.employeename,
						.zgperiode = report.zgperiode,
						.aus_dat = report.aus_dat,
						.lanr = report.lanr,
						.laname = report.laname,
						.isaslo = report.isaslo,
						.isout = report.isout,
						.createdfrom = report.createdfrom,
						.createdon = report.createdon,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of EmployeeAdvancePaymentProperty) = New BindingList(Of EmployeeAdvancePaymentProperty)

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
	''' <param name="employeenumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedEmployeePayrollList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfPayroll = m_EmployeeDatabaseAccess.LoadFoundedPayrollForEmployeeMng(employeeNumber)
		If listOfPayroll Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Lohn-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfPayroll
		Select New EmployeePayrollProperty With
					 {.mdnr = report.mdnr,
						.lonr = report.lonr,
						.monat = report.monat,
						.jahr = report.jahr,
						.manr = report.manr,
						.periode = report.periode,
						.employeename = report.employeename,
						.createdfrom = report.createdfrom,
						.createdon = report.createdon,
						.zfiliale = report.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of EmployeePayrollProperty) = New BindingList(Of EmployeePayrollProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfPayroll.Count

		Return Not listOfPayroll Is Nothing
	End Function


	''' <summary>
	''' Loads Documents for selected employee
	''' </summary>
	''' <param name="employeenumber"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function LoadFoundedEmployeeDocumentsList(ByVal employeeNumber As Integer?) As Boolean

		Dim listOfDocuments = m_EmployeeDatabaseAccess.LoadPrintedDocumentsForEmployeeMng(employeeNumber, True, True, True, True, chkDocumentFromThisYear.Checked)
		If listOfDocuments Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Druckvorlagen-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim reportGridData = (From report In listOfDocuments
		Select New EmployeePrintedDocProperty With
					 {.createdfrom = report.createdfrom,
						.createdon = report.createdon,
						.docname = report.docname,
						.manr = report.manr,
						.recID = report.recID,
						.scandoc = report.scandoc,
						.username = report.username
					 }).ToList()

		Dim listDataSource As BindingList(Of EmployeePrintedDocProperty) = New BindingList(Of EmployeePrintedDocProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdEmployeeProperty.DataSource = listDataSource
		bsiRecCount.Caption = listOfDocuments.Count

		Return Not listOfDocuments Is Nothing
	End Function

	Public Function DeleteFoundedEmployeeDocumentsList(ByVal employeeNumber As Integer?, ByVal recID As Integer) As Boolean

		Dim success = m_EmployeeDatabaseAccess.DeletePrintedDocumentsForEmployeeMng(employeeNumber, recID)
		If success Then
			m_UtilityUI.ShowOKDialog("Das Dokument wurde gelöscht.")
		Else
			m_UtilityUI.ShowErrorDialog("Das Dokument konnte nicht erfolgreich gelöscht werden.")
		End If

		Return success

	End Function


#End Region


#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case Me.Searchedmodul
			Case 1
				Try
					If File.Exists(m_GVProposeSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVProposeSettingfilenameWithEmployee)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case 2
				Try
					If File.Exists(m_GVOffersSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVOffersSettingfilenameWithEmployee)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 3
				Try
					If File.Exists(m_GVESSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVESSettingfilenameWithEmployee)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 4
				Try
					If File.Exists(m_GVReportsSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVReportsSettingfilenameWithEmployee)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try

			Case 5
				Try
					If File.Exists(m_GVAdvancePaymentSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVAdvancePaymentSettingfilenameWithEmployee)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case 6
				Try
					If File.Exists(m_GVPayrollSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVPayrollSettingfilenameWithEmployee)

					If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case 7
				Try
					If File.Exists(m_GVDocumentSettingfilenameWithEmployee) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVDocumentSettingfilenameWithEmployee)

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
				gvEmployeeProperty.SaveLayoutToXml(m_GVProposeSettingfilenameWithEmployee)

			Case 2
				gvEmployeeProperty.SaveLayoutToXml(m_GVOffersSettingfilenameWithEmployee)

			Case 3
				gvEmployeeProperty.SaveLayoutToXml(m_GVESSettingfilenameWithEmployee)

			Case 4
				gvEmployeeProperty.SaveLayoutToXml(m_GVReportsSettingfilenameWithEmployee)

			Case 5
				gvEmployeeProperty.SaveLayoutToXml(m_GVAdvancePaymentSettingfilenameWithEmployee)

			Case 6
				gvEmployeeProperty.SaveLayoutToXml(m_GVPayrollSettingfilenameWithEmployee)

			Case 7
				gvEmployeeProperty.SaveLayoutToXml(m_GVDocumentSettingfilenameWithEmployee)

		End Select

	End Sub


	Private Sub OngvEmployeeProperty_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvEmployeeProperty.RowCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

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

	Private Sub grdEmployeeProperty_KeyDown(sender As Object, e As KeyEventArgs) Handles grdEmployeeProperty.KeyDown

		If Searchedmodul = 7 AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 138, m_InitializationData.MDData.MDNr) AndAlso (e.KeyCode = Keys.Delete) Then

			Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim docRowData = CType(grdView.GetRow(selectedRows(0)), EmployeePrintedDocProperty)

					If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																					m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
						Return
					End If

					Dim result = DeleteFoundedEmployeeDocumentsList(m_EmployeeNumber, docRowData.recID)

					LoadFoundedEmployeeDocumentsList(m_EmployeeNumber)
				End If
			End If

		End If

	End Sub

End Class
