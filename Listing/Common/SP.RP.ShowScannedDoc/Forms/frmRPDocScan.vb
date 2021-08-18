
Imports System.Reflection.Assembly
Imports System.ComponentModel
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins
Imports DevExpress.XtraEditors
Imports DevExpress.XtraBars.Helpers
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.Pdf

Imports System
Imports System.Drawing
Imports System.Text
Imports System.IO

Imports System.Security.Cryptography
Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports O2S.Components.PDFRender4NET
Imports System.Data.SqlClient
Imports O2S.Components.PDFView4NET
Imports System.Text.RegularExpressions
Imports DevComponents.DotNetBar

Imports SPProgUtility
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Report
Imports DevExpress.XtraPdfViewer.Commands
Imports SP.RP.ShowScannedDoc.ClsDataDetail
Imports SP.DatabaseAccess.Customer
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Report.DataObjects

Public Class frmRPDocScan

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Dim pcc As New DevExpress.XtraBars.PopupControlContainer
	Private reportDBPersister As IClsDbRegister

	Private m_ESDbAccess As IESDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDbAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The customer data access object.
	''' </summary>
	Private m_CustomerDbAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The report data access object.
	''' </summary>
	Private m_ReportDatabaseAccess As IReportDatabaseAccess


	Dim _ClsReg As New ClsDivReg
	Dim _ClsProgSetting As New ClsProgSettingPath
	Dim _clsLog As New ClsEventLog

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private errorProviderMangement As DXErrorProvider.DXErrorProvider

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SPProgUtility.MainUtilities.Utilities

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI
	Private m_utilitySP As SP.Infrastructure.Utility

	Private m_mandant As Mandant

	Private m_SelectedContenctID As Integer
	Private m_SelectedContentFileName As String

	Private m_OpenedDocGuid As String

	Private m_ScanModulType As ScanTypes

	Private Enum ScanTypes
		employee
		customer
		employe
		reports
		payroll
	End Enum



	Private m_ReportOverviewData As IEnumerable(Of RPOverviewData)



#Region "Public Properties"

	''' <summary>
	''' Gets or sets the preselection data.
	''' </summary>
	Public Property PreselectionData As PreselectionData

#End Region



#Region "Private properties"

	ReadOnly Property MaximumFileSize2Import() As Short
		Get
			Dim strQuery As String = "//RPSetting/RPFilesize"
			Dim strBez As Short = CShort(Val(GetXMLValueByQuery(_ClsProgSetting.GetFormDataFile, strQuery, 2)))
			Return strBez
		End Get
	End Property

	ReadOnly Property GetPDFVW_O2SSerial() As String
		Get
			Return _ClsProgSetting.GetPDFVW_O2SSerial  ' "PDFVW4WIN-FHKHA-MTOKR-KYXGK-S5OE4-H3IT3"
		End Get
	End Property

	ReadOnly Property GetPDF_O2SSerial() As String
		Get
			Return _ClsProgSetting.GetPDF_O2SSerial '  "PDF4NET-MT735-CUBQB-6D8HV-I82RS-BO1VB"
		End Get
	End Property

	'Private Property PDFViewChanged As Boolean

	'Private Property SelectedContentFileName As String

	Private Property SelectedMainFileGuid As String
	Private Property iOpenedMANr As Integer = 0
	Private Property iOpenedKDNr As Integer = 0
	Private Property iOpenedESNr As Integer = 0
	Private Property iOpenedRPNr As Integer = 0
	Private Property iOpenedRPLNr As Integer = 0
	Private Property sOpenedKW As Short = 1

	Private Property IsForIndividualReport As Boolean

	''' <summary>
	''' Gets the selected File.
	''' </summary>
	''' <returns>The selected LA or nothing.</returns>
	Private ReadOnly Property SelectedScanFile As ScanedData
		Get

			If lueScaned.EditValue Is Nothing Then
				Return Nothing
			End If
			Dim ScanedFileData As IEnumerable(Of ScanedData) = lueScaned.Properties.DataSource
			Dim laData = ScanedFileData.Where(Function(data) data.ImportedFileGuild = lueScaned.EditValue).FirstOrDefault()

			Return laData
		End Get
	End Property

	''' <summary>
	''' Gets the selected record.
	''' </summary>
	''' <returns>The selected user or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedContentRecord As ScanedContentData
		Get
			Dim gv = TryCast(grdScanContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gv Is Nothing) Then

				Dim selectedRows = gv.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim content = CType(gv.GetRow(selectedRows(0)), ScanedContentData)
					If content Is Nothing Then Return content

					Dim data = reportDBPersister.LoadAssignedScannedContentData(content.ID)
					If data Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler bei der Anzeige der Daten."))
					End If
					Return data

				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected report line.
	''' </summary>
	''' <returns>The selected LA or nothing.</returns>
	Private ReadOnly Property SelectedreportLine As ReportLineData
		Get

			If lueScaned.EditValue Is Nothing Then
				Return Nothing
			End If
			Dim ScanedFileData As IEnumerable(Of ReportLineData) = lueReportLineID.Properties.DataSource
			Dim rplData = ScanedFileData.Where(Function(data) data.RPLNr = lueReportLineID.EditValue).FirstOrDefault()

			Return rplData
		End Get
	End Property

	''' <summary>
	''' Gets the selected record for import.
	''' </summary>
	''' <returns>The selected user or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedImportContentRecord As AssignedDataToImport
		Get
			Dim gv = TryCast(grdImportScanContent.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gv Is Nothing) Then

				Dim selectedRows = gv.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim content = CType(gv.GetRow(selectedRows(0)), AssignedDataToImport)

					Return content

				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_mandant = New SPProgUtility.Mandanten.Mandant
		m_InitialData = _setting

		m_SuppressUIEvents = True
		Me.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False

		errorProviderMangement = New DXErrorProvider.DXErrorProvider

		Dim connectionString = m_InitialData.MDData.MDDbConn
		m_ESDbAccess = New ESDatabaseAccess(connectionString, m_InitialData.UserData.UserLanguage)
		m_EmployeeDbAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)
		m_CustomerDbAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)
		m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(connectionString, m_InitialData.UserData.UserLanguage)

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
		m_Utility = New SPProgUtility.MainUtilities.Utilities
		m_utilitySP = New SP.Infrastructure.Utility
		m_UtilityUI = New UtilityUI
		IsForIndividualReport = False

		InitializeComponent()

		Try
			Me.xtabControl.Dock = DockStyle.Fill
			Me.sccRPStd.Dock = DockStyle.Fill
			Me.pcRapportDaten.Dock = DockStyle.Fill
			Me.PdfViewer1.Dock = DockStyle.Fill
			Me.pcSPStdInfoHeader.Dock = DockStyle.Fill
			Me.grdImportScanContent.Dock = DockStyle.Fill

			pnlPDFViewer.BorderStyle = BorderStyles.NoBorder
			pcRapportDaten.BorderStyle = BorderStyles.NoBorder
			Me.sccControl.BorderStyle = BorderStyles.NoBorder
			Me.sccRPStd.BorderStyle = BorderStyles.NoBorder

			Me.xtabControl.SelectedTabPage = xtabScanedDoc

			Me.xtabControl.TabPages(0).PageEnabled = True
			Me.xtabControl.TabPages(1).PageEnabled = True
			Me.rpgEinzelnrapport.Visible = False
			Me.rpgRapportinfo.Visible = False

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		TranslateControls()

		Reset()
		Dim success As Boolean = True
		success = success AndAlso LoadScanedDropDownData()
		success = success AndAlso LoadReportsDropDownData()


		AddHandler lueCategory.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueRecordNumber.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler gvImportScanContent.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvScanContent.RowCellClick, AddressOf OngvScanContent_RowCellClick

		m_SuppressUIEvents = False

	End Sub


#End Region


#Region "Public methods"

	Public Sub LoadIndividualReport()

		m_SuppressUIEvents = True
		iOpenedMANr = PreselectionData.EmployeeNumber.GetValueOrDefault(0)
		iOpenedKDNr = PreselectionData.CustomerNumber.GetValueOrDefault(0)
		iOpenedESNr = PreselectionData.ESNumber.GetValueOrDefault(0)
		iOpenedRPNr = PreselectionData.ReportNumber.GetValueOrDefault(0)
		iOpenedRPLNr = PreselectionData.ReportLineNumber.GetValueOrDefault(0)
		sOpenedKW = PreselectionData.CalendarWeek.GetValueOrDefault(0)

		beiRPNumber.EditValue = iOpenedRPNr
		beiKW.EditValue = sOpenedKW

		IsForIndividualReport = True

		sccScanRPMain.PanelVisibility = SplitPanelVisibility.Panel2
		sccControl.PanelVisibility = SplitPanelVisibility.Panel2

		Me.xtabControl.TabPages(0).PageEnabled = True
		Me.xtabControl.TabPages(1).PageEnabled = False

		Dim rpdata As New DBInformation With {.SelectedRecordNumber = iOpenedRPNr,
																				.SelectedRPLNr = iOpenedRPLNr,
																				.EmployeeNumber = iOpenedMANr,
																				.CustomerNumber = iOpenedKDNr}

		Me.reportDBPersister = New ClsDbFunc()
		Dim data = reportDBPersister.LoadAssignedScannedReport(rpdata)

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das gescannted Dokument konnte nicht geladen werden."))
			Return
		End If

		Dim bytes() = data.DocScan
		Dim tempFileName = System.IO.Path.GetTempFileName()
		Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

		If (Not bytes Is Nothing AndAlso m_utilitySP.WriteFileBytes(tempFileFinal, bytes)) Then
			m_SelectedContentFileName = tempFileFinal

			ShowFileInViewer(tempFileFinal, Me.PdfViewer1)
			pcRapportDaten.Enabled = True
			m_OpenedDocGuid = data.RPDoc_Guid

		End If
		m_SuppressUIEvents = False

		Me.rpgEinzelnrapport.Visible = True

	End Sub


#End Region


#Region "Private Methods"


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		beiRPNumber.Caption = m_Translate.GetSafeTranslationValue(Me.beiRPNumber.Caption)
		beiKW.Caption = m_Translate.GetSafeTranslationValue(Me.beiKW.Caption)
		bbiRotate.Caption = m_Translate.GetSafeTranslationValue(Me.bbiRotate.Caption)
		bbiRotate_.Caption = m_Translate.GetSafeTranslationValue(Me.bbiRotate_.Caption)
		bbiSaveIndividualReportIntoDb.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSaveIndividualReportIntoDb.Caption)
		bbiDeleteIndividualReport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDeleteIndividualReport.Caption)
		bbiOpenOneRP.Caption = m_Translate.GetSafeTranslationValue(Me.bbiOpenOneRP.Caption)

		rpgEinzelnrapport.Text = m_Translate.GetSafeTranslationValue(rpgEinzelnrapport.Text)
		rpgDokumentseite.Text = m_Translate.GetSafeTranslationValue(rpgDokumentseite.Text)
		rpgWOS.Text = m_Translate.GetSafeTranslationValue(rpgWOS.Text)
		rpgRapportinfo.Text = m_Translate.GetSafeTranslationValue(rpgRapportinfo.Text)
		bbiActualSizeOne.Caption = m_Translate.GetSafeTranslationValue(Me.bbiActualSizeOne.Caption)
		bbiZoomOneIn.Caption = m_Translate.GetSafeTranslationValue(Me.bbiZoomOneIn.Caption)
		bbiZoomOneOut.Caption = m_Translate.GetSafeTranslationValue(Me.bbiZoomOneOut.Caption)
		bbiPrintIndividualReport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrintIndividualReport.Caption)
		bbiSaveIndividualReportIntoFile.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSaveIndividualReportIntoFile.Caption)

		lblRecordNumber.Text = m_Translate.GetSafeTranslationValue(lblRecordNumber.Text)
		lblKategorie.Text = m_Translate.GetSafeTranslationValue(lblKategorie.Text)
		lblRapperNr.Text = m_Translate.GetSafeTranslationValue(lblRapperNr.Text)
		lblErfasstzeile.Text = m_Translate.GetSafeTranslationValue(lblErfasstzeile.Text)

		bChkMAWOS.Caption = m_Translate.GetSafeTranslationValue(bChkMAWOS.Caption)
		bChkKDWOS.Caption = m_Translate.GetSafeTranslationValue(bChkKDWOS.Caption)

		xtabScanedDoc.Text = m_Translate.GetSafeTranslationValue(xtabScanedDoc.Text)
		xtabSummery.Text = m_Translate.GetSafeTranslationValue(xtabSummery.Text)
		Me.bsiState.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Me.bsiLblRapportnummer.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRapportnummer.Caption)
		Me.bsiLblRapportzeile.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRapportzeile.Caption)

	End Sub

	Private Sub Reset()

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		ResetScanedDropDown()
		ResetScanContentGrid()

		ResetDocumentCategoryDropDown()
		ResetReportDropDown()
		ResetReportLineDropDown()


		m_SuppressUIEvents = suppressUIEventsState
		errorProviderMangement.ClearErrors()

	End Sub


#Region "reset grid and dropdown"

	''' <summary>
	''' Resets the Scaned drop down.
	''' </summary>
	Private Sub ResetScanedDropDown()

		lueScaned.Properties.DisplayMember = "File_ScannedOn"
		lueScaned.Properties.ValueMember = "ImportedFileGuild"

		gvScanedDoc.OptionsView.ShowIndicator = False
		gvScanedDoc.OptionsView.ShowColumnHeaders = True
		gvScanedDoc.OptionsView.ShowFooter = False

		gvScanedDoc.OptionsView.ShowAutoFilterRow = True
		gvScanedDoc.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvScanedDoc.Columns.Clear()

		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		gvScanedDoc.Columns.Add(columnID)

		Dim columnFile_ScannedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFile_ScannedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFile_ScannedOn.Caption = m_Translate.GetSafeTranslationValue("Scan-Daten")
		columnFile_ScannedOn.Name = "File_ScannedOn"
		columnFile_ScannedOn.FieldName = "File_ScannedOn"
		columnFile_ScannedOn.Visible = True
		columnFile_ScannedOn.Width = 200
		gvScanedDoc.Columns.Add(columnFile_ScannedOn)

		Dim columnImportedFileGuild As New DevExpress.XtraGrid.Columns.GridColumn()
		columnImportedFileGuild.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnImportedFileGuild.Caption = m_Translate.GetSafeTranslationValue("ImportedFileGuild")
		columnImportedFileGuild.Name = "ImportedFileGuild"
		columnImportedFileGuild.FieldName = "ImportedFileGuild"
		columnImportedFileGuild.Visible = False
		gvScanedDoc.Columns.Add(columnImportedFileGuild)

		Dim columnModulRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnModulRecordNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnModulRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnModulRecordNumber.Name = "ModulRecordNumber"
		columnModulRecordNumber.FieldName = "ModulRecordNumber"
		columnModulRecordNumber.Visible = False
		gvScanedDoc.Columns.Add(columnModulRecordNumber)

		Dim columnModulNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnModulNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnModulNumber.Caption = m_Translate.GetSafeTranslationValue("Modul")
		columnModulNumber.Name = "ModulNumber"
		columnModulNumber.FieldName = "ModulNumber"
		columnModulNumber.Visible = False
		gvScanedDoc.Columns.Add(columnModulNumber)


		lueScaned.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueScaned.Properties.NullText = String.Empty
		lueScaned.EditValue = Nothing

	End Sub

	Private Sub ResetScanContentGrid()

		gvScanContent.OptionsView.ShowIndicator = False
		gvScanContent.OptionsView.ShowAutoFilterRow = True
		gvScanContent.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvScanContent.OptionsView.ShowFooter = True

		gvScanContent.Columns.Clear()


		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		gvScanContent.Columns.Add(columnID)

		Dim columnModulNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnModulNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnModulNumber.Caption = m_Translate.GetSafeTranslationValue("Modul")
		columnModulNumber.Name = "ModulNumber"
		columnModulNumber.FieldName = "ModulNumber"
		columnModulNumber.Width = 30
		columnModulNumber.Visible = False
		gvScanContent.Columns.Add(columnModulNumber)

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "RecordNumber"
		columnRecordNumber.FieldName = "RecordNumber"
		columnRecordNumber.Visible = False
		columnRecordNumber.Width = 30
		gvScanContent.Columns.Add(columnRecordNumber)

		Dim columnDataToShow As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDataToShow.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDataToShow.Caption = m_Translate.GetSafeTranslationValue("Daten")
		columnDataToShow.Name = "DataToShow"
		columnDataToShow.FieldName = "DataToShow"
		columnDataToShow.Visible = True
		columnDataToShow.Width = 100
		gvScanContent.Columns.Add(columnDataToShow)


		grdScanContent.DataSource = Nothing

	End Sub


	''' <summary>
	''' Resets the employee drop down.
	''' </summary>
	Private Sub ResetEmployeeDropDown()

		lueRecordNumber.Properties.DisplayMember = "LastnameFirstname"
		lueRecordNumber.Properties.ValueMember = "EmployeeNumber"

		gvRecordNumber.OptionsView.ShowIndicator = False
		gvRecordNumber.OptionsView.ShowColumnHeaders = True
		gvRecordNumber.OptionsView.ShowFooter = False

		gvRecordNumber.OptionsView.ShowAutoFilterRow = True
		gvRecordNumber.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRecordNumber.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = True
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnEmployeeNumber)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnLastnameFirstname.Name = "LastnameFirstname"
		columnLastnameFirstname.FieldName = "LastnameFirstname"
		columnLastnameFirstname.Visible = True
		columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnLastnameFirstname)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnPostcodeAndLocation)

		lueRecordNumber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueRecordNumber.Properties.NullText = String.Empty
		lueRecordNumber.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the customer drop down.
	''' </summary>
	Private Sub ResetCustomerDropDown()

		lueRecordNumber.Properties.DisplayMember = "Company1"
		lueRecordNumber.Properties.ValueMember = "CustomerNumber"

		gvRecordNumber.OptionsView.ShowIndicator = False
		gvRecordNumber.OptionsView.ShowColumnHeaders = True
		gvRecordNumber.OptionsView.ShowFooter = False

		gvRecordNumber.OptionsView.ShowAutoFilterRow = True
		gvRecordNumber.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRecordNumber.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvRecordNumber.Columns.Add(columnPostcodeAndLocation)

		lueRecordNumber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueRecordNumber.Properties.NullText = String.Empty
		lueRecordNumber.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the document category drop down.
	''' </summary>
	Private Sub ResetDocumentCategoryDropDown()

		lueCategory.Properties.DisplayMember = "Description"
		lueCategory.Properties.ValueMember = "CategoryNumber"

		Dim columns = lueCategory.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Description", 0))

		lueCategory.Properties.ShowFooter = False
		lueCategory.Properties.DropDownRows = 10
		lueCategory.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCategory.Properties.SearchMode = SearchMode.AutoComplete
		lueCategory.Properties.AutoSearchColumnIndex = 0

		lueCategory.Properties.NullText = String.Empty
		lueCategory.EditValue = Nothing

	End Sub


	''' <summary>
	''' Resets the Reports drop down.
	''' </summary>
	Private Sub ResetReportDropDown()

		lueReportNumber.Properties.DisplayMember = "RPNr"
		lueReportNumber.Properties.ValueMember = "RPNr"

		gvReportNumber.OptionsView.ShowIndicator = False
		gvReportNumber.OptionsView.ShowColumnHeaders = True
		gvReportNumber.OptionsView.ShowFooter = False

		gvReportNumber.OptionsView.ShowAutoFilterRow = True
		gvReportNumber.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvReportNumber.Columns.Clear()

		Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRPNr.Caption = m_Translate.GetSafeTranslationValue("RPNr")
		columnRPNr.Name = "RPNr"
		columnRPNr.FieldName = "RPNr"
		columnRPNr.Visible = True
		columnRPNr.Width = 50
		gvReportNumber.Columns.Add(columnRPNr)

		Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullName.Name = "EmployeeFullName"
		columnEmployeeFullName.FieldName = "EmployeeFullName"
		columnEmployeeFullName.Visible = True
		columnEmployeeFullName.Width = 100
		gvReportNumber.Columns.Add(columnEmployeeFullName)

		Dim columnCustomer1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCustomer1.Name = "Customer1"
		columnCustomer1.FieldName = "Customer1"
		columnCustomer1.Visible = True
		columnCustomer1.Width = 100
		gvReportNumber.Columns.Add(columnCustomer1)

		Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnValue.Caption = m_Translate.GetSafeTranslationValue("Von")
		columnValue.Name = "ReportFrom"
		columnValue.FieldName = "ReportFrom"
		columnValue.Visible = True
		columnValue.Width = 100
		gvReportNumber.Columns.Add(columnValue)

		Dim columnReportTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnReportTo.Caption = m_Translate.GetSafeTranslationValue("Bis")
		columnReportTo.Name = "ReportTo"
		columnReportTo.FieldName = "ReportTo"
		columnReportTo.Visible = True
		columnReportTo.Width = 100
		gvReportNumber.Columns.Add(columnReportTo)

		lueReportNumber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueReportNumber.Properties.NullText = String.Empty
		lueReportNumber.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the Report line drop down.
	''' </summary>
	Private Sub ResetReportLineDropDown()

		lueReportLineID.Properties.DisplayMember = "LANr"
		lueReportLineID.Properties.ValueMember = "RPLNr"

		gvReportLineID.OptionsView.ShowIndicator = False
		gvReportLineID.OptionsView.ShowColumnHeaders = True
		gvReportLineID.OptionsView.ShowFooter = False

		gvReportLineID.OptionsView.ShowAutoFilterRow = True
		gvReportLineID.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvReportLineID.Columns.Clear()

		Dim columnRPLNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRPLNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRPLNr.Name = "RPLNr"
		columnRPLNr.FieldName = "RPLNr"
		columnRPLNr.Visible = False
		columnRPLNr.Width = 100
		gvReportLineID.Columns.Add(columnRPLNr)

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
		columnLANr.Name = "LANr"
		columnLANr.FieldName = "LANr"
		columnLANr.Visible = True
		columnLANr.Width = 100
		gvReportLineID.Columns.Add(columnLANr)

		Dim columnVonDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVonDate.Caption = String.Empty
		columnVonDate.Name = "VonDate"
		columnVonDate.FieldName = "VonDate"
		columnVonDate.Visible = True
		columnVonDate.Width = 100
		gvReportLineID.Columns.Add(columnVonDate)

		Dim columnBisDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBisDate.Caption = String.Empty
		columnBisDate.Name = "BisDate"
		columnBisDate.FieldName = "BisDate"
		columnBisDate.Visible = True
		columnBisDate.Width = 100
		gvReportLineID.Columns.Add(columnBisDate)


		lueReportLineID.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueReportLineID.Properties.NullText = String.Empty
		lueReportLineID.EditValue = Nothing

	End Sub

	Private Sub ResetImportScanContentGrid()

		gvImportScanContent.OptionsView.ShowIndicator = False
		gvImportScanContent.OptionsView.ShowAutoFilterRow = True
		gvImportScanContent.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvImportScanContent.Columns.Clear()



		Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnID.Name = "ID"
		columnID.FieldName = "ID"
		columnID.Visible = False
		columnID.OptionsColumn.AllowEdit = False
		columnID.OptionsColumn.AllowShowHide = False
		gvImportScanContent.Columns.Add(columnID)

		Dim columnModulNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnModulNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnModulNumber.Caption = m_Translate.GetSafeTranslationValue("Modulnummer")
		columnModulNumber.Name = "ModulNumber"
		columnModulNumber.FieldName = "ModulNumber"
		columnModulNumber.Visible = False
		columnModulNumber.Width = 20
		columnModulNumber.OptionsColumn.AllowEdit = False
		columnModulNumber.OptionsColumn.AllowShowHide = False
		gvImportScanContent.Columns.Add(columnModulNumber)

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "RecordNumber"
		columnRecordNumber.FieldName = "RecordNumber"
		columnRecordNumber.Visible = True
		columnRecordNumber.Width = 30
		columnRecordNumber.OptionsColumn.AllowEdit = False
		gvImportScanContent.Columns.Add(columnRecordNumber)

		Dim columnFile_ScannedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFile_ScannedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFile_ScannedOn.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnFile_ScannedOn.Name = "File_ScannedOn"
		columnFile_ScannedOn.FieldName = "File_ScannedOn"
		columnFile_ScannedOn.Width = 80
		columnFile_ScannedOn.Visible = True
		columnFile_ScannedOn.OptionsColumn.AllowEdit = False
		gvImportScanContent.Columns.Add(columnFile_ScannedOn)


		If m_ScanModulType = ScanTypes.reports Then

			Dim columnCalendarWeek As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCalendarWeek.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCalendarWeek.Caption = m_Translate.GetSafeTranslationValue("Woche")
			columnCalendarWeek.Name = "CalendarWeek"
			columnCalendarWeek.FieldName = "CalendarWeek"
			columnCalendarWeek.Visible = True
			columnCalendarWeek.Width = 30
			columnCalendarWeek.OptionsColumn.AllowEdit = False
			gvImportScanContent.Columns.Add(columnCalendarWeek)

			Dim columnLAOPText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAOPText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLAOPText.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
			columnLAOPText.Name = "LAOPText"
			columnLAOPText.FieldName = "LAOPText"
			columnLAOPText.Visible = True
			columnLAOPText.Width = 100
			columnLAOPText.OptionsColumn.AllowEdit = False
			gvImportScanContent.Columns.Add(columnLAOPText)

			Dim columnPeriod As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPeriod.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPeriod.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
			columnPeriod.Name = "Period"
			columnPeriod.FieldName = "Period"
			columnPeriod.Visible = True
			columnPeriod.Width = 80
			columnPeriod.OptionsColumn.AllowEdit = False
			gvImportScanContent.Columns.Add(columnPeriod)

		Else

			Dim columnDocumentCategoryNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDocumentCategoryNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDocumentCategoryNumber.Caption = m_Translate.GetSafeTranslationValue("Dokumenten-Kategorie")
			columnDocumentCategoryNumber.Name = "DocumentCategoryNumber"
			columnDocumentCategoryNumber.FieldName = "DocumentCategoryNumber"
			columnDocumentCategoryNumber.Visible = True
			columnDocumentCategoryNumber.Width = 20
			columnDocumentCategoryNumber.OptionsColumn.AllowEdit = False
			gvImportScanContent.Columns.Add(columnDocumentCategoryNumber)

		End If

		Dim columnRecipientName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecipientName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRecipientName.Caption = m_Translate.GetSafeTranslationValue("Empfänger")
		columnRecipientName.Name = "RecipientName"
		columnRecipientName.FieldName = "RecipientName"
		columnRecipientName.Visible = True
		columnRecipientName.Width = 100
		columnRecipientName.OptionsColumn.AllowEdit = False
		gvImportScanContent.Columns.Add(columnRecipientName)

		Dim columnImport As New DevExpress.XtraGrid.Columns.GridColumn()
		columnImport.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnImport.Caption = m_Translate.GetSafeTranslationValue("Übernehmen") & "?"
		columnImport.Name = "IsSelected"
		columnImport.FieldName = "IsSelected"
		columnImport.Visible = True
		columnImport.Width = 80
		columnImport.OptionsColumn.AllowEdit = True
		gvImportScanContent.Columns.Add(columnImport)


		grdImportScanContent.DataSource = Nothing

	End Sub


#End Region


	Private Sub ResetMaskData()

		pnlScanData.Visible = False
		pnlReportData.Visible = False

		pnlScanData.Top = btnSaveContent.Top
		pnlReportData.Top = btnSaveContent.Top

		ResetScanContentGrid()
		PdfViewer1.CloseDocument()

	End Sub

#End Region

	Private Sub frmRPDocScan_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try

			Try
				My.Settings.iHeight = Me.Height
				My.Settings.iWidth = Me.Width
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

				If Not IsForIndividualReport Then
					My.Settings.sccScanRPMain_panel1Width = Me.sccScanRPMain.SplitterPosition
					My.Settings.sccControl_panel1Height = Me.sccControl.SplitterPosition
					My.Settings.sccRPStd_panel1Height = Me.sccRPStd.SplitterPosition

				Else
					My.Settings.bSendRPToKDWOS = Me.bChkKDWOS.EditValue
					My.Settings.bSendRPToMAWOS = Me.bChkMAWOS.EditValue

				End If

				My.Settings.Save()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Formsetting speichern: {1}", strMethodeName, ex.Message))

			End Try

			Me.Dispose()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub form_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"

			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next

			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

		End If
	End Sub

	Private Sub OnForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Try
			If My.Settings.frmLocation <> String.Empty Then
				Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			Me.sccScanRPMain.SplitterPosition = If(My.Settings.sccScanRPMain_panel1Width = 0, 50, My.Settings.sccScanRPMain_panel1Width)
			Me.sccControl.SplitterPosition = If(My.Settings.sccControl_panel1Height = 0, 50, My.Settings.sccControl_panel1Height)
			Me.sccRPStd.SplitterPosition = If(My.Settings.sccRPStd_panel1Height = 0, 50, My.Settings.sccRPStd_panel1Height)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Me.pcRapportDaten.Enabled = False
		Try
			If _ClsProgSetting.bAllowedKDDocTransferTo_WS Then
				Me.bChkKDWOS.EditValue = My.Settings.bSendRPToKDWOS

			Else
				Me.bChkKDWOS.EditValue = False
				My.Settings.bSendRPToKDWOS = False
				Me.bChkKDWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
			End If

			If _ClsProgSetting.bAllowedMADocTransferTo_WS Then
				Me.bChkMAWOS.EditValue = My.Settings.bSendRPToMAWOS

			Else
				Me.bChkMAWOS.EditValue = False
				My.Settings.bSendRPToMAWOS = False
				Me.bChkMAWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

			End If
			If Me.bChkKDWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Never And Me.bChkMAWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Never Then
				Me.rpgWOS.Visible = False
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Form Checkbox: {1}", strMethodeName, ex.Message))

		End Try


	End Sub

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub OngvScanContent_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		FocusScanContentData()

	End Sub

	Private Sub OngvScanContent_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvScanContent.FocusedRowChanged

		FocusScanContentData()

	End Sub

	Private Sub FocusScanContentData()
		Dim selecteddata = SelectedContentRecord

		If selecteddata Is Nothing Then
			Return
		End If
		'Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		'm_SuppressUIEvents = True


		SplashScreenManager.ShowForm(Me, GetType(frmWait), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Daten werden geladen") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim bytes() = selecteddata.Scan_Komplett
		Dim tempFileName = System.IO.Path.GetTempFileName()
		Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

		m_SelectedContenctID = selecteddata.ID
		m_ScanModulType = selecteddata.ModulNumber
		If (Not bytes Is Nothing AndAlso m_utilitySP.WriteFileBytes(tempFileFinal, bytes)) Then
			m_SelectedContentFileName = tempFileFinal
			ShowFileInViewer(tempFileFinal, Me.PdfViewer1)
			Me.pcRapportDaten.Enabled = True
		End If

		'm_SuppressUIEvents = suppressUIEventsState

		InitialmaskData(selecteddata)
		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub InitialmaskData(ByVal data As ScanedContentData)
		Dim dataMatrixCode = data.FoundedCodeValue

		bChkKDWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		bChkMAWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		lueRecordNumber.EditValue = Nothing
		lueCategory.EditValue = Nothing

		Select Case m_ScanModulType
			'Case ScanTypes.employee
			'	lblRecordNumber.Text = m_Translate.GetSafeTranslationValue("Kandidat")
			'	pnlScanData.Visible = True
			'	ResetEmployeeDropDown()
			'	Dim employeedata = LoadEmployeeDropDownData()
			'	Dim categoriyData = LoadEmployeeCategoriesDropDownData(m_InitialData.UserData.UserLanguage)

			'	If Not employeedata Then
			'		m_UtilityUI.ShowErrorDialog("Kandidatendaten konnten nicht geladen werden.")
			'		Return
			'	End If

			'	lueRecordNumber.EditValue = data.RecordNumber
			'	lueCategory.EditValue = data.DocumentCategoryNumber

			'Case ScanTypes.customer
			'	lblRecordNumber.Text = m_Translate.GetSafeTranslationValue("Kunde")
			'	pnlScanData.Visible = True
			'	ResetCustomerDropDown()
			'	Dim customerdata = LoadCustomerDropDownData()
			'	Dim categoriyData = LoadCustomerCategoriesDropDownData(m_InitialData.UserData.UserLanguage)

			'	If Not customerdata Then
			'		m_UtilityUI.ShowErrorDialog("Kundendaten konnten nicht geladen werden.")
			'		Return
			'	End If

			'	lueRecordNumber.EditValue = data.RecordNumber
			'	lueCategory.EditValue = data.DocumentCategoryNumber

			Case ScanTypes.reports

				Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
				m_SuppressUIEvents = True

				lueReportNumber.EditValue = data.RecordNumber
				txtKW.EditValue = data.CalendarWeek
				lueReportLineID.EditValue = data.RPLID
				txt_Beginn.EditValue = Format(data.Monday, "d")
				txt_Ende.EditValue = Format(data.Sunday, "d")

				m_SuppressUIEvents = m_SuppressUIEvents


				pnlReportData.Visible = True
				pnlScanData.Visible = False

				bChkKDWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
				bChkMAWOS.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

		End Select

	End Sub



	Sub ShowFileInViewer(ByVal strFilename As String, ByVal pdfViewer As DevExpress.XtraPdfViewer.PdfViewer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim myfileinfo As FileInfo = FileIO.FileSystem.GetFileInfo(strFilename)

		Try
			If myfileinfo.Extension.ToLower <> ".pdf" Then
				Dim strNewGuid As String = Guid.NewGuid.ToString
				Dim strFinalGrafikFilename As String = String.Format("{0}\{1}.tif", _ClsProgSetting.GetSpSBildFiles2DeletePath, strNewGuid)
				Dim strFinalPDFFilename As String = String.Format("{0}\{1}.PDF", _ClsProgSetting.GetSpSFiles2DeletePath, strNewGuid)
				Dim bDeleteGrafikFile As Boolean = True
				O2S.Components.PDF4NET.PDFFile.PDFFile.SerialNumber = Me.GetPDF_O2SSerial

				If myfileinfo.Extension.ToLower = ".jpg" Or myfileinfo.Extension.ToLower = ".jepg" Or myfileinfo.Extension.ToLower = ".xps" Then
					Dim Img As Image = Image.FromFile(myfileinfo.FullName)
					Img.Save(strFinalGrafikFilename, System.Drawing.Imaging.ImageFormat.Tiff)

				ElseIf myfileinfo.Extension.ToLower = ".tif" Or myfileinfo.Extension.ToLower = ".tiff" Then
					strFinalGrafikFilename = myfileinfo.FullName
					bDeleteGrafikFile = False

				Else
					Throw New Exception(String.Format(m_Translate.GetSafeTranslationValue("Das ausgewählte Datei-Format wird nicht unterstützt!{0}{1}"), vbNewLine, myfileinfo.FullName))

				End If

				Dim strMessage As String = m_Translate.GetSafeTranslationValue("Ihre Datei liegt in einem Grafik-Format vor. Für die bessere Kompatibilität muss ich diese in ein PDF-Format umwandeln. Dies geschieht vollautomatisch.{0}Soll ich mit der Umwandlung fortsetzen?")
				strMessage = String.Format(strMessage, vbNewLine)
				Dim iResult = m_UtilityUI.ShowYesNoDialog(strMessage, m_Translate.GetSafeTranslationValue("Datei umwandeln"), MessageBoxDefaultButton.Button1)
				If iResult = MsgBoxResult.No Then Return
				O2S.Components.PDF4NET.Converters.PDFConverter.ConvertTiffToPDF(strFinalGrafikFilename, strFinalPDFFilename)


				Try
					If bDeleteGrafikFile Then File.Delete(strFinalGrafikFilename)
				Catch ex As Exception

				End Try

				m_SelectedContentFileName = strFinalPDFFilename
				strFilename = m_SelectedContentFileName
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Try
			Me.bsiState.Caption = m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Momment") & "..."
			pdfViewer.CloseDocument()
			pdfViewer.LoadDocument(strFilename)
			pdfViewer.ZoomMode = PDFZoomMode.Custom
			pdfViewer.CurrentPageNumber = 1
			bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Seite {0} von {1}"), PdfViewer1.CurrentPageNumber, PdfViewer1.PageCount)
			bsiState.Caption = m_Translate.GetSafeTranslationValue("Bereit")

			pdfViewer.Visible = True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If e.Clicks = 2 Then
			Dim selected = SelectedImportContentRecord
			If selected Is Nothing Then Return

			Select Case m_ScanModulType
				Case ScanTypes.employee
					OpenEmployee(selected.RecordNumber)

				Case ScanTypes.customer
					OpenEmployee(selected.RecordNumber)

				Case ScanTypes.reports
					If e.Column.FieldName.ToLower = "RecipientName".ToLower Then
						OpenCustomer(selected.SP_CustomerNumber)
					Else
						OpenReport(selected.RecordNumber)
					End If

			End Select
		End If

	End Sub

	''' <summary>
	''' Handles change of luescaned.
	''' </summary>
	Private Sub OnlueScaned_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueScaned.EditValueChanged

		ResetMaskData()

		If lueScaned.EditValue Is Nothing Then Return
		SelectedMainFileGuid = lueScaned.EditValue
		If Not SelectedMainFileGuid Is Nothing Then
			LoadScanedContentData()
		End If

		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Handles change of report number.
	''' </summary>
	Private Sub OnlueReportNumber_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueReportNumber.EditValueChanged

		'If m_SuppressUIEvents Then
		'	Return
		'End If

		If Not lueReportNumber.EditValue Is Nothing Then
			m_SuppressUIEvents = True

			LoadReportLineDropDownData(lueReportNumber.EditValue)

			m_SuppressUIEvents = False

		End If

	End Sub

	''' <summary>
	''' Handles change of report number.
	''' </summary>
	Private Sub OnlueReportlineID_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueReportLineID.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		txt_Beginn.EditValue = Nothing
		txt_Ende.EditValue = Nothing

		If Not lueReportLineID.EditValue Is Nothing Then
			Dim data = SelectedreportLine
			If data Is Nothing Then
				'm_UtilityUI.ShowErrorDialog("Fehler in der Anzeige der Rapportzeilen.")

				Return
			End If

			txt_Beginn.EditValue = Format(data.VonDate, "d")
			txt_Ende.EditValue = Format(data.BisDate, "d")

		End If

	End Sub


	'Sub ListDbContentFiles()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Me.reportDBPersister = New ClsDbFunc(Me.SelectedMainFileGuid, m_SelectedContenctID)

	'	Dim dt As DataTable = Me.reportDBPersister.ListNewScannedFiles(Me.SelectedMainFileGuid)
	'	Dim i As Integer = 0
	'	grdScanContent.DataSource = dt

	'	If dt.Rows.Count > 0 Then
	'		For Each col As GridColumn In Me.gvScanContent.Columns
	'			Trace.WriteLine(col.FieldName)
	'			col.MinWidth = 0
	'			Dim strColName As String = col.FieldName.ToLower
	'			Try
	'				'          col.Visible = strColName.Contains("File_ScannedOn".ToLower)
	'				col.Visible = strColName.Contains("RPData".ToLower)
	'				col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

	'			Catch ex As Exception
	'				col.Visible = False

	'			End Try
	'			i += 1
	'		Next col
	'		Me.xtabControl.TabPages(2).PageEnabled = True
	'	End If

	'End Sub


	''' <summary>
	''' Loads the scaned drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadScanedDropDownData() As Boolean

		Me.reportDBPersister = New ClsDbFunc
		Dim listOfData = reportDBPersister.LoadScannedData()

		Dim gridData = (From person In listOfData
										Select New ScanedData With
													 {.ID = person.ID,
														.File_ScannedOn = person.File_ScannedOn,
														.ImportedFileGuild = person.ImportedFileGuild
													 }).ToList()

		Dim listDataSource As BindingList(Of ScanedData) = New BindingList(Of ScanedData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		lueScaned.Properties.DataSource = listDataSource


		Return Not listOfData Is Nothing

	End Function

	''' <summary>
	''' Loads the scaned drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadScanedContentData() As Boolean

		Me.reportDBPersister = New ClsDbFunc()
		Dim listOfData = reportDBPersister.LoadScannedContentData(SelectedMainFileGuid)
		If listOfData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New ScanedContentData With
													 {.ID = person.ID,
														.File_ScannedOn = person.File_ScannedOn,
														.ImportedFileGuild = person.ImportedFileGuild,
														.CalendarWeek = person.CalendarWeek,
														.ModulNumber = person.ModulNumber,
														.RecordNumber = person.RecordNumber,
														.DocumentCategoryNumber = person.DocumentCategoryNumber
													 }).ToList()

		Dim listDataSource As BindingList(Of ScanedContentData) = New BindingList(Of ScanedContentData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdScanContent.DataSource = listDataSource
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl der Datensätze: {0}"), gvScanContent.RowCount)


		Return Not listOfData Is Nothing

	End Function

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData() As Boolean

		Dim employeeData = m_ESDbAccess.LoadEmployeeData()

		If employeeData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnen nicht geladen werden."))
			Return False
		End If

		lueRecordNumber.EditValue = Nothing
		lueRecordNumber.Properties.DataSource = employeeData

		Return True

	End Function

	''' <summary>
	''' Loads the customer drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim customerData = m_ESDbAccess.LoadCustomerData(m_InitialData.UserData.UserFiliale)

		If customerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))
			Return False
		End If

		lueRecordNumber.EditValue = Nothing
		lueRecordNumber.Properties.DataSource = customerData

		Return True

	End Function

	''' <summary>
	''' Loads the employee document category data.
	''' </summary>
	Private Function LoadEmployeeCategoriesDropDownData(ByVal language As String) As Boolean
		Dim categoryData = m_EmployeeDbAccess.LoadEmployeeDocumentCategories()

		Dim categoryViewData = Nothing
		If Not categoryData Is Nothing Then

			categoryViewData = New List(Of CategoryVieData)

			For Each category In categoryData

				Dim categoryDescription As String = String.Empty
				Select Case language.ToLower().Trim()
					Case "d", "de"
						categoryDescription = category.DescriptionGerman
					Case "f", "fr"
						categoryDescription = category.DescriptionFrench
					Case "i", "it"
						categoryDescription = category.DescriptionItalian
					Case Else
						categoryDescription = category.DescriptionGerman
				End Select

				categoryViewData.Add(New CategoryVieData With {.CategoryNumber = category.CategoryNumber,
																											 .Description = categoryDescription})
			Next

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Dokumentenkategorieauswahldaten konnten nicht geladen werden."))
		End If

		lueCategory.Properties.DataSource = categoryViewData
		lueCategory.Properties.ForceInitialize()

		Return Not categoryViewData Is Nothing
	End Function

	''' <summary>
	''' Loads the customer document category data.
	''' </summary>
	Private Function LoadCustomerCategoriesDropDownData(ByVal language As String) As Boolean
		Dim categoryData = m_CustomerDbAccess.LoadCustomerDocumentCategoryData()

		Dim categoryViewData = Nothing
		If Not categoryData Is Nothing Then

			categoryViewData = New List(Of CategoryVieData)

			For Each category In categoryData

				Dim categoryDescription As String = String.Empty
				Select Case language.ToLower().Trim()
					Case "d", "de"
						categoryDescription = category.DescriptionGerman
					Case "f", "fr"
						categoryDescription = category.DescriptionFrench
					Case "i", "it"
						categoryDescription = category.DescriptionItalian
					Case Else
						categoryDescription = category.DescriptionGerman
				End Select

				categoryViewData.Add(New CategoryVieData With {.CategoryNumber = category.CategoryNumber,
																											 .Description = categoryDescription})
			Next

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kunden Dokumentenkategorieauswahldaten konnten nicht geladen werden."))
		End If

		lueCategory.Properties.DataSource = categoryViewData
		lueCategory.Properties.ForceInitialize()

		Return Not categoryViewData Is Nothing
	End Function

	''' <summary>
	''' Loads the report drop down data.
	''' </summary>
	Private Function LoadReportsDropDownData() As Boolean

		m_ReportOverviewData = m_ReportDatabaseAccess.LoadRPOverviewListData()

		If m_ReportOverviewData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportdaten konnen nicht geladen werden."))
			Return False
		End If

		lueReportNumber.EditValue = Nothing
		lueReportNumber.Properties.DataSource = m_ReportOverviewData


		Return True

	End Function

	''' <summary>
	''' Loads the report drop down data.
	''' </summary>
	Private Function LoadReportLineDropDownData(ByVal reportNumber As Integer) As Boolean

		Dim reportOverviewData = reportDBPersister.LoadReportLineData(reportNumber)

		If reportOverviewData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnen nicht geladen werden."))
			Return False
		End If

		lueReportLineID.EditValue = Nothing
		lueReportLineID.Properties.DataSource = reportOverviewData

		Return True

	End Function

	''' <summary>
	''' Loads the import data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadImportScanedContentData() As Boolean

		Dim data = SelectedContentRecord
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Keine Daten wurden ausgewählt.")

			Return False
		End If

		Dim listOfData As IEnumerable(Of AssignedDataToImport)
		Select Case data.ModulNumber
			Case 0, 1
				listOfData = reportDBPersister.LoadAssignedScanContentWithForImport()


			Case Else
				listOfData = reportDBPersister.LoadAssignedReportScanContentWithForImport(data.ImportedFileGuild)

		End Select

		If listOfData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten zum Import konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New AssignedDataToImport With
													 {.ID = person.ID,
														.File_ScannedOn = person.File_ScannedOn,
														.ImportedFileGuild = person.ImportedFileGuild,
														.IsSelected = person.IsSelected,
														.LAOPText = person.LAOPText,
														.Period = person.Period,
														.SP_CustomerNumber = person.SP_CustomerNumber,
														.SP_EinsatzNumber = person.SP_EinsatzNumber,
														.SP_EmployeeNumber = person.SP_EmployeeNumber,
														.RecipientName = person.RecipientName,
														.SP_Period = person.SP_Period,
														.SP_RecordNumber = person.SP_RecordNumber,
														.SP_RPLNumber = person.SP_RPLNumber,
														.CalendarWeek = person.CalendarWeek,
														.ModulNumber = person.ModulNumber,
														.RecordNumber = person.RecordNumber,
														.DocumentCategoryNumber = person.DocumentCategoryNumber
													 }).ToList()

		Dim listDataSource As BindingList(Of AssignedDataToImport) = New BindingList(Of AssignedDataToImport)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdImportScanContent.DataSource = listDataSource
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl der Datensätze: {0}"), gvScanContent.RowCount)


		Return Not listOfData Is Nothing

	End Function



	Private Sub btnSaveSelectedRPContent_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveContent.Click

		Dim recordNumber As Integer?
		Dim rplID As Integer?
		Dim rplFrom As Date?
		Dim rplTo As Date?
		Dim calendarWeek As Integer?
		Dim categoryNumber As Integer?
		Dim selecteddata = SelectedContentRecord

		If selecteddata Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Keine Daten wurden ausgewählt.")

			Return
		End If

		If m_ScanModulType = ScanTypes.reports Then
			recordNumber = lueReportNumber.EditValue
			rplID = lueReportLineID.EditValue
			rplFrom = CType(txt_Beginn.EditValue, Date)
			rplTo = CType(txt_Ende.EditValue, Date)
			calendarWeek = txtKW.EditValue

		Else
			recordNumber = lueRecordNumber.EditValue
			categoryNumber = lueCategory.EditValue
		End If

		Dim success As Boolean = True
		Dim data As DBInformation
		Try
			data = New DBInformation With {.SelectedFileID = selecteddata.ID,
																		 .SelectedRecordNumber = recordNumber,
																		 .CalendarWeek = calendarWeek,
																		 .SelectedRPLID = rplID,
																		 .RPLFrom = rplFrom,
																		 .RPLTo = rplTo,
																		 .SelectedCategoryNumber = categoryNumber}

			Dim fileInfo As New IO.FileInfo(PdfViewer1.DocumentFilePath)
			' Load the file bytes
			Dim bytes() = m_utilitySP.LoadFileBytes(fileInfo.FullName)

			If bytes Is Nothing Then
				data.DocScan = Nothing
			Else
				' Save the Doc.
				data.DocScan = bytes
			End If

			success = success AndAlso Me.reportDBPersister.UpdateFileContentData(data)
			Dim msg As String
			If success Then
				msg = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
				m_UtilityUI.ShowInfoDialog(msg)

			Else
				msg = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
				m_UtilityUI.ShowErrorDialog(msg)

			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub


	Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabControl.SelectedPageChanged

		ToastNotification.Close(Me.pcSPStdInfoHeader)

		If xtabControl.SelectedTabPage Is xtabSummery Then
			ResetImportScanContentGrid()

			LoadImportScanedContentData()

			Me.btnSaveCheckedContentInfoLocalDb.Visible = Me.gvImportScanContent.RowCount > 0
		End If

	End Sub


	Public Function ContainsMoreString(ByVal str As String, ByVal ParamArray values As String()) As Boolean

		For Each value As String In values
			If str = value Then
				Return True
			End If
		Next

		Return False
	End Function

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			End If


		End If

	End Sub

	''' <summary>
	''' Handles button click on record number.
	''' </summary>
	Private Sub OnlueRecordNumber_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueRecordNumber.ButtonClick

		If lueRecordNumber.EditValue Is Nothing Then
			Return
		End If

		If (e.Button.Index = 2) Then

			'Dim hub = MessageService.Instance.Hub
			Select Case m_ScanModulType
				Case ScanTypes.employee
					OpenEmployee(lueRecordNumber.EditValue)
					'Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, lueRecordNumber.EditValue)
					'hub.Publish(openEmployeeMng)

				Case ScanTypes.customer
					OpenCustomer(lueRecordNumber.EditValue)
					'Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, lueRecordNumber.EditValue)
					'hub.Publish(openCustomerMng)

				Case ScanTypes.reports
					OpenReport(lueRecordNumber.EditValue)
					'Dim openreportMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, lueRecordNumber.EditValue)
					'hub.Publish(openreportMng)


			End Select


		End If

	End Sub

	Private Sub OpenEmployee(ByVal employeeNumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, employeeNumber)
		hub.Publish(openEmployeeMng)

	End Sub

	Private Sub OpenCustomer(ByVal customerNumber As Integer)
		Dim hub = MessageService.Instance.Hub
		Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, customerNumber)
		hub.Publish(openCustomerMng)

	End Sub

	Private Sub OpenReport(ByVal reportNumber As Integer)
		Dim hub = MessageService.Instance.Hub
		Dim openreportMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, reportNumber)
		hub.Publish(openreportMng)

	End Sub

	Private Sub btnSaveCheckedContentInfoLocalDb_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveCheckedContentInfoLocalDb.Click
		Dim selecteddata = SelectedImportContentRecord

		If selecteddata Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Es sind keine Daten zum Import vorhanden.")
			Return
		End If
		InsertRecordIntoMainTables()

		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'Dim strValue As String = String.Empty
		'Dim strTbleName As String = String.Empty
		'Dim iRowCount As Integer = gvImportScanContent.RowCount - 1

		'Try

		'For i As Integer = 0 To iRowCount
		'	If i > iRowCount Then Exit For
		'	ToastNotification.Close(Me.pcSPStdInfoHeader)
		'	ToastNotification.Show(Me.pcSPStdInfoHeader, strMsg, Nothing, ToastNotification.DefaultTimeoutInterval, _
		'												 eToastGlowColor.None, eToastPosition.MiddleCenter)

		'	Dim dtr As DataRow
		'	Dim iRecID As Integer = 0
		'	dtr = gvImportScanContent.GetDataRow(i)
		'	strValue = dtr.Item("übernehmen?").ToString
		'	If Not String.IsNullOrWhiteSpace(strValue) Then
		'		If CBool(strValue) Then
		'			iRecID = CInt(Val(dtr.Item("ID").ToString))

		'			' jetzt sollte der Datensatz in Sputnik hinzugefügt werden...
		'			Dim iRPNr As Integer = CInt(Val(dtr.Item("RPNr").ToString))
		'			Dim iRPLNr As Integer = CInt(Val(dtr.Item("SPRPLNr").ToString))
		'			Dim iMANr As Integer = CInt(Val(dtr.Item("SPMANr").ToString))
		'			Dim iKDNr As Integer = CInt(Val(dtr.Item("SPKDNr").ToString))
		'			Dim iESNr As Integer = CInt(Val(dtr.Item("SPESNr").ToString))

		'			Dim test As DBInformation = New DBInformation With {.SelectedRecordNumber = iRPNr, _
		'																													.SelectedRPLNr = iRPLNr, _
		'																													.EmployeeNumber = iMANr, _
		'																													.CustomerNumber = iKDNr, _
		'																													.ESNumber = iESNr, _
		'																													.SelectedFileID = iRecID, _
		'																													.SendRPToKDWOS = Me.bChkKDWOS.EditValue, _
		'																													.SendRPToMAWOS = Me.bChkMAWOS.EditValue}
		'			Me.reportDBPersister.SaveCheckedContentIntoSPDb(test)
		'			dtr.Delete()
		'			i = -1
		'			iRowCount = gvImportScanContent.RowCount - 1
		'		End If
		'	End If
		'Next i

		'Catch ex As Exception
		'	m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		'	m_UtilityUI.ShowErrorDialog(ex.ToString)
		'End Try

	End Sub

	Private Sub InsertRecordIntoMainTables()
		Dim importdatalist As BindingList(Of AssignedDataToImport) = New BindingList(Of AssignedDataToImport)
		Dim success As Boolean = True
		Dim successCount = 0
		Dim invalidCount = 0
		Dim msg As String = String.Empty

		importdatalist = CType(grdImportScanContent.DataSource, BindingList(Of AssignedDataToImport))
		If importdatalist Is Nothing Then Return

		Dim selectedData = importdatalist.Where(Function(data) data.IsSelected = True).ToList()

		For Each rec In selectedData
			rec.SendToCustomerWOS = bChkKDWOS.EditValue
			rec.SendToEmployeeWOS = bChkMAWOS.EditValue

			If m_ScanModulType = ScanTypes.employee Then
				'success = success AndAlso reportDBPersister.AddAssignedEmployeeContentIntoFinalTable(rec)

			ElseIf m_ScanModulType = ScanTypes.customer Then
				'success = success AndAlso reportDBPersister.AddAssignedCustomerContentIntoFinalTable(rec)

			ElseIf m_ScanModulType = ScanTypes.reports Then
				success = success AndAlso reportDBPersister.AddAssignedReportContentIntoFinalTable(rec)

			End If

			successCount += If(success, 1, 0)
			invalidCount += If(Not success, 1, 0)

		Next

		msg = String.Format(m_Translate.GetSafeTranslationValue("Erfolgreich importiert: {1}{0}Fehlerhaft: {2}"), vbNewLine, successCount, invalidCount)
		If successCount + invalidCount > 0 Then m_UtilityUI.ShowInfoDialog(msg)

		If successCount > 0 Then LoadImportScanedContentData()

	End Sub


	Private Sub OnbbiOpenOneRP_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiOpenOneRP.ItemClick
		OpenOneScanFile()
	End Sub

	Private Sub OnbbiSaveIndividualReportIntoDb_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSaveIndividualReportIntoDb.ItemClick
		SaveIndividualReportIntoDatabase()
	End Sub

	Private Sub OnbbiDeleteIndividualReport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDeleteIndividualReport.ItemClick
		DeleteOpenedScanFile()
	End Sub


	Sub OpenOneScanFile()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sFileSize As Short = Me.MaximumFileSize2Import
		Dim strFilefilter As String = "(Alle PDF-Dateien)|*.PDF|(Alle JPG-Dateien)|*.JPG|(Alle TIF-Dateien)|*.TIF|(Alle XPS-Dateien)|*.XPS"
		Dim odlg As New OpenFileDialog With {.Filter = strFilefilter,
																		 .InitialDirectory = My.Settings.Folder2Select,
																		 .FileName = My.Settings.LastselectedFile}
		Dim iDlgResult As DialogResult = odlg.ShowDialog()
		If iDlgResult <> DialogResult.OK Then Exit Sub
		Dim myfileinfo As FileInfo = FileIO.FileSystem.GetFileInfo(odlg.FileName)
		Dim MyFileSize As Single = myfileinfo.Length / 1024 ^ 2
		If MyFileSize > sFileSize Then
			Dim strMessage As String = "Die ausgewählte Datei ist zu gross! Bitte komprimieren Sie die Datei{0}{1}{0}auf maximum {2} MB."
			strMessage = String.Format(strMessage, vbNewLine, myfileinfo.FullName, sFileSize)

			m_UtilityUI.ShowInfoDialog(Me, m_Translate.GetSafeTranslationValue(strMessage))
			Return
		End If
		m_SelectedContentFileName = odlg.FileName

		Try
			ShowFileInViewer(m_SelectedContentFileName, Me.PdfViewer1)

			My.Settings.Folder2Select = odlg.InitialDirectory
			My.Settings.LastselectedFile = m_SelectedContentFileName
			My.Settings.Save()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	Sub SaveIndividualReportIntoDatabase()

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return
		Dim strNewFullFilename As String = PdfViewer1.DocumentProperties.FilePath

		Try
			Dim success As Boolean = True
			Dim docData As DBInformation = New DBInformation With {.EmployeeNumber = Me.iOpenedMANr,
																													.CustomerNumber = Me.iOpenedKDNr,
																													.ESNumber = Me.iOpenedESNr,
																													.SelectedRecordNumber = Me.iOpenedRPNr,
																													.SelectedRPLNr = Me.iOpenedRPLNr,
																													.CalendarWeek = Me.sOpenedKW,
																													.OriginFileGuid = m_OpenedDocGuid,
																													.SendRPToKDWOS = Me.bChkKDWOS.EditValue,
																													.SendRPToMAWOS = Me.bChkMAWOS.EditValue}

			Dim fileInfo As New IO.FileInfo(PdfViewer1.DocumentFilePath)
			' Load the file bytes
			Dim bytes() = m_utilitySP.LoadFileBytes(fileInfo.FullName)

			If bytes Is Nothing Then
				docData.DocScan = Nothing
			Else
				' Save the Doc.
				docData.DocScan = bytes
			End If

			success = Me.reportDBPersister.UpdateAssignedReport(docData)
			Dim strMessage As String = String.Empty

			If success Then
				strMessage = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gespeichert.")
				m_OpenedDocGuid = docData.OriginFileGuid
			Else
				strMessage = m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gespeichert werden!")
			End If
			Me.bsiState.Caption = strMessage


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		End Try

	End Sub

	Sub DeleteOpenedScanFile()

		Dim test As DBInformation = New DBInformation With {.EmployeeNumber = Me.iOpenedMANr, _
																												.CustomerNumber = Me.iOpenedKDNr, _
																												.ESNumber = Me.iOpenedESNr, _
																												.SelectedRecordNumber = Me.iOpenedRPNr, _
																												.SelectedRPLNr = Me.iOpenedRPLNr, _
																												.OriginFileGuid = m_OpenedDocGuid}
		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return
		Dim strMessage As String = m_Translate.GetSafeTranslationValue("Hiermit löschen Sie das Dokument aus der Datenbank.{0}Möchten Sie wirklich das Dokument löschen?")
		strMessage = String.Format(strMessage, vbNewLine)
		Dim success = m_UtilityUI.ShowYesNoDialog(strMessage, m_Translate.GetSafeTranslationValue("Dokument löschen?"), MessageBoxDefaultButton.Button1)
		If Not success Then Return

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True
		Try
			success = success AndAlso Me.reportDBPersister.DeleteAssignedReport(test)

			If success Then
				strMessage = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gelöscht.")
				PdfViewer1.CloseDocument()
				m_OpenedDocGuid = Nothing

			Else
				strMessage = m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gelöscht werden!")

			End If
			Me.bsiState.Caption = strMessage

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		Finally
			m_SuppressUIEvents = suppressUIEventsState

		End Try

	End Sub

	Private Sub RotateRight()

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return
		Dim filename As String = PdfViewer1.DocumentProperties.FilePath
		Dim angle As Integer = 0
		Dim pdfDocumentProcessor As New PdfDocumentProcessor()
		pdfDocumentProcessor.LoadDocument(filename)
		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		For Each page In pdfDocumentProcessor.Document.Pages
			angle = (page.Rotate + 90) Mod 360
			page.Rotate = angle
		Next page

		Dim tempFileName = System.IO.Path.GetTempFileName()
		Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")
		pdfDocumentProcessor.SaveDocument(tempFileFinal)

		pdfDocumentProcessor.CloseDocument()
		PdfViewer1.CloseDocument()
		m_SuppressUIEvents = suppressUIEventsState

		Try
			File.Delete(filename)
		Catch ex As Exception

		End Try

		PdfViewer1.LoadDocument(tempFileFinal)

	End Sub

	Private Sub RotateLeft()

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return
		Dim filename As String = PdfViewer1.DocumentProperties.FilePath
		Dim angle As Integer = 0
		Dim pdfDocumentProcessor As New PdfDocumentProcessor()
		pdfDocumentProcessor.LoadDocument(filename)
		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		For Each page In pdfDocumentProcessor.Document.Pages
			angle = (page.Rotate - 90) Mod 360
			page.Rotate = angle
		Next page

		Dim tempFileName = System.IO.Path.GetTempFileName()
		Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")
		pdfDocumentProcessor.SaveDocument(tempFileFinal)

		pdfDocumentProcessor.CloseDocument()
		PdfViewer1.CloseDocument()
		m_SuppressUIEvents = suppressUIEventsState

		Try
			File.Delete(filename)
		Catch ex As Exception

		End Try
		PdfViewer1.LoadDocument(tempFileFinal)

	End Sub

	Private Sub ZoomIn()

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		PdfViewer1.ZoomFactor = CSng(PdfViewer1.ZoomFactor * 1.1)

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub ZoomOut()

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		PdfViewer1.ZoomFactor = CSng(PdfViewer1.ZoomFactor / 1.1)

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub ZoomReset()

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		PdfViewer1.ZoomFactor = CSng(100)

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub bbiRotate__ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiRotate_.ItemClick
		RotateRight()
	End Sub

	Private Sub bbiRotate_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiRotate.ItemClick
		RotateLeft()
	End Sub


	Private Sub bbiZoomOneIn_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiZoomOneIn.ItemClick
		ZoomIn()
	End Sub

	Private Sub bbiZoomOneOut_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiZoomOneOut.ItemClick
		ZoomOut()
	End Sub

	Private Sub bbiActualSizeOne_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiActualSizeOne.ItemClick
		ZoomReset()
	End Sub


	Private Sub OnbbiPrintIndividualReport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintIndividualReport.ItemClick
		PdfViewer1.Print()
	End Sub

	Private Sub OnbbiSaveIndividualReportIntoFile_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSaveIndividualReportIntoFile.ItemClick

		If String.IsNullOrWhiteSpace(PdfViewer1.DocumentFilePath) Then Return
		Dim strFile2Print As String = String.Format("{0}{1}.PDF", _ClsProgSetting.GetSpSFiles2DeletePath, Guid.NewGuid.ToString)

		Dim dlg As New System.Windows.Forms.SaveFileDialog With {.FileName = FileIO.FileSystem.GetName(strFile2Print), _
																														 .InitialDirectory = _ClsProgSetting.GetSpSFiles2DeletePath, _
																														 .Filter = "(Alle PDF-Dateien)|*.PDF"}
		Dim iDglResult As DialogResult = dlg.ShowDialog()
		If iDglResult <> DialogResult.OK Then Return

		PdfViewer1.SaveDocument(dlg.FileName)
		Dim strMessage As String = "Ihre Datei wurde erfolgreich unter {0} gespeichert."
		strMessage = String.Format(m_Translate.GetSafeTranslationValue(strMessage), dlg.FileName)
		m_UtilityUI.ShowOKDialog(strMessage)

	End Sub



#Region "Datenaufruf für Rapportsuche in Scanning..."

	'Private Sub lib_RPNr_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblRapperNr.LinkClicked
	'	Dim grdGrid As New DevExpress.XtraGrid.GridControl
	'	Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	'	Dim dt As New DataTable
	'	pcc = New DevExpress.XtraBars.PopupControlContainer
	'	pcc.Name = "pcc_kdTemp"

	'	pcc.SuspendLayout()
	'	pcc.Manager = New DevExpress.XtraBars.BarManager
	'	pcc.Left = Me.lblRapperNr.Location.X
	'	pcc.Top = Me.lblRapperNr.Location.Y
	'	pcc.ShowCloseButton = True
	'	pcc.ShowSizeGrip = True

	'	grdGrid.Dock = DockStyle.Fill

	'	dt = Me.reportDBPersister.GetRPDb4SelectingRP
	'	grdGrid.DataSource = dt
	'	grdGrid.MainView = grdGrid.CreateView("view_KDtemp")
	'	grdGrid.Name = "grd_KDTemp"

	'	grdGrid.ForceInitialize()
	'	grdGrid.Visible = False
	'	Me.Controls.AddRange(New Control() {pcc})
	'	pcc.Controls.AddRange(New Control() {grdGrid})
	'	If My.Settings.pcc_RP2SelectSize <> "" Then
	'		Dim aSize As String() = My.Settings.pcc_RP2SelectSize.Split(CChar(";"))
	'		pcc.Size = New Size(CInt(aSize(0)), CInt(aSize(1)))
	'	Else
	'		pcc.Size = New Size(600, 400)
	'	End If

	'	'    AddHandler GridView1.RowClick, AddressOf EditValueChanged
	'	AddHandler pcc.SizeChanged, AddressOf pcc_SizeChanged
	'	AddHandler grdGrid.DoubleClick, AddressOf ViewKD_RowClick

	'	pcc.ShowPopup(Cursor.Position)
	'	grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
	'	'If My.Settings.bgrdView_EnterpriseShowGroup Then _
	'	grdView.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways
	'	grdView.ShowFindPanel()

	'	grdView.OptionsBehavior.Editable = False
	'	grdView.OptionsSelection.EnableAppearanceFocusedCell = False
	'	grdView.OptionsSelection.InvertSelection = False
	'	grdView.OptionsSelection.EnableAppearanceFocusedRow = True
	'	grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus

	'	Dim i As Integer = 0
	'	For Each col As GridColumn In grdView.Columns
	'		Trace.WriteLine(String.Format("{0}", col.FieldName))
	'		col.MinWidth = 0
	'		Try

	'			col.Visible = col.FieldName.ToLower.Contains("firma1") Or _
	'														col.FieldName.ToLower.Contains("MAName".ToLower) Or _
	'														col.FieldName.ToLower.Contains("von") Or _
	'														col.FieldName.ToLower.Contains("bis")
	'			col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

	'		Catch ex As Exception
	'			col.Visible = False

	'		End Try
	'		i += 1
	'	Next col
	'	grdGrid.Visible = True
	'	pcc.ResumeLayout()

	'End Sub

	'Sub ViewKD_RowClick(sender As Object, e As System.EventArgs)
	'	Dim strValue As String = String.Empty
	'	Dim strTbleName As String = String.Empty
	'	Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	'	grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

	'	Try
	'		For i As Integer = 0 To grdView.SelectedRowsCount - 1
	'			Dim row As Integer = (grdView.GetSelectedRows()(i))
	'			If (grdView.GetSelectedRows()(i) >= 0) Then
	'				Dim dtr As DataRow
	'				dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
	'				strValue = dtr.Item("RPNr").ToString
	'				Me.lueReportNumber.Text = strValue
	'				Me.txtKW.Text = 0

	'				Me.txt_Beginn.Text = String.Empty
	'				Me.txt_Ende.Text = String.Empty
	'				Me.txtRPLID.Text = String.Empty
	'			End If
	'		Next i
	'		pcc.HidePopup()

	'	Catch ex As Exception

	'	End Try

	'End Sub

	Private Sub pcc_SizeChanged(sender As Object, e As System.EventArgs)

		My.Settings.pcc_RP2SelectSize = String.Format("{0};{1}", sender.Size.Width, sender.Size.Height)
		My.Settings.Save()

	End Sub

	Private Sub pcc_RPLDataSizeChanged(sender As Object, e As System.EventArgs)

		My.Settings.pcc_RPL2SelectSize = String.Format("{0};{1}", sender.Size.Width, sender.Size.Height)
		My.Settings.Save()

	End Sub

	'Private Sub lib_RPLData_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblErfasstzeile.LinkClicked
	'	Dim grdGrid As New DevExpress.XtraGrid.GridControl
	'	Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	'	Dim dt As New DataTable
	'	pcc = New DevExpress.XtraBars.PopupControlContainer
	'	pcc.Name = "pcc_RPLTemp"

	'	pcc.SuspendLayout()
	'	pcc.Manager = New DevExpress.XtraBars.BarManager
	'	'pcc.Left = Me.lib_RPNr.Location.X
	'	'pcc.Top = Me.lib_RPNr.Location.Y
	'	pcc.ShowCloseButton = True
	'	pcc.ShowSizeGrip = True

	'	grdGrid.Dock = DockStyle.Fill

	'	dt = Me.reportDBPersister.GetKWDataInRP(Me.lueReportNumber.Text)
	'	grdGrid.DataSource = dt
	'	grdGrid.MainView = grdGrid.CreateView("view_RPLtemp")
	'	grdGrid.Name = "grd_RPLTemp"

	'	grdGrid.ForceInitialize()
	'	grdGrid.Visible = False
	'	Me.Controls.AddRange(New Control() {pcc})
	'	pcc.Controls.AddRange(New Control() {grdGrid})
	'	If My.Settings.pcc_RPL2SelectSize <> "" Then
	'		Dim aSize As String() = My.Settings.pcc_RPL2SelectSize.Split(CChar(";"))
	'		pcc.Size = New Size(CInt(aSize(0)), CInt(aSize(1)))
	'	Else
	'		pcc.Size = New Size(600, 400)
	'	End If

	'	'    AddHandler GridView1.RowClick, AddressOf EditValueChanged
	'	AddHandler pcc.SizeChanged, AddressOf pcc_RPLDataSizeChanged
	'	AddHandler grdGrid.DoubleClick, AddressOf ViewRPL_RowClick

	'	pcc.ShowPopup(Cursor.Position)
	'	grdView = TryCast(grdGrid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)
	'	'If My.Settings.bgrdView_EnterpriseShowGroup Then _
	'	grdView.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways
	'	grdView.ShowFindPanel()

	'	grdView.OptionsBehavior.Editable = False
	'	grdView.OptionsSelection.EnableAppearanceFocusedCell = False
	'	grdView.OptionsSelection.InvertSelection = False
	'	grdView.OptionsSelection.EnableAppearanceFocusedRow = True
	'	grdView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus

	'	Dim i As Integer = 0
	'	For Each col As GridColumn In grdView.Columns
	'		Trace.WriteLine(String.Format("{0}", col.FieldName))
	'		col.MinWidth = 0
	'		Try

	'			col.Visible = col.FieldName.ToLower.Contains("RPLNr".ToLower) Or _
	'														col.FieldName.ToLower.Contains("VonDate".ToLower) Or _
	'														col.FieldName.ToLower.Contains("BisDate".ToLower)
	'			col.Caption = m_Translate.GetSafeTranslationValue(col.GetCaption)

	'		Catch ex As Exception
	'			col.Visible = False

	'		End Try
	'		i += 1
	'	Next col
	'	grdGrid.Visible = True
	'	pcc.ResumeLayout()

	'End Sub

	'Sub ViewRPL_RowClick(sender As Object, e As System.EventArgs)
	'	Dim strValue As String = String.Empty
	'	Dim strTbleName As String = String.Empty
	'	Dim grdView As New DevExpress.XtraGrid.Views.Grid.GridView

	'	grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

	'	Try
	'		For i As Integer = 0 To grdView.SelectedRowsCount - 1
	'			Dim row As Integer = (grdView.GetSelectedRows()(i))
	'			If (grdView.GetSelectedRows()(i) >= 0) Then
	'				Dim dtr As DataRow
	'				dtr = grdView.GetDataRow(grdView.GetSelectedRows()(i))
	'				strValue = dtr.Item("RPLID").ToString
	'				Me.txtRPLID.Text = strValue

	'				strValue = Format(dtr.Item("VonDate"), "d")
	'				Me.txt_Beginn.Text = strValue
	'				strValue = Format(dtr.Item("BisDate"), "d")
	'				Me.txt_Ende.Text = strValue

	'			End If
	'		Next i
	'		pcc.HidePopup()

	'	Catch ex As Exception

	'	End Try

	'End Sub


#End Region


	Private Sub RadialMenu1_ItemClick(sender As System.Object, e As System.EventArgs) Handles RadialMenu1.ItemClick
		Dim item As RadialMenuItem = TryCast(sender, RadialMenuItem)

		If m_SelectedContenctID = 0 Then Return
		If item IsNot Nothing AndAlso (Not String.IsNullOrEmpty(item.Text)) Then
			Dim data = SelectedContentRecord
			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Keine Daten sind vorhanden.")

				Return
			End If

			If item.Name = Me.rmRPDelete.Name Then
				Dim success As Boolean = True
				Dim msg = m_Translate.GetSafeTranslationValue("Hiermit löschen Sie den gescannten Rapport endgültig. Möchten Sie wirklich mit dem Vorgang fortfahren?")
				success = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Gescanntes Dokument löschen"), MessageBoxDefaultButton.Button1)

				success = success AndAlso Me.reportDBPersister.DeleteAssignedFileContent(data.ID)
				success = success AndAlso LoadScanedContentData()
				If success Then
					LoadScanedDropDownData()
					ResetMaskData()
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihr Dokument wurde gelöscht."))
				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihr Dokument konnte nicht gelöscht wurde!"))

				End If

			ElseIf item.Name = Me.rmRPPrint.Name Then
				PdfViewer1.Print()

			ElseIf item.Name = Me.rmRPOpen.Name Then

				Try
					Dim hub = MessageService.Instance.Hub
					Dim openMng As New OpenReportsMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, data.RecordNumber)
					hub.Publish(openMng)

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(ex.ToString)

				End Try

			End If
		End If

	End Sub

	Private Sub grdQualifikation_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles grdScanContent.KeyUp
		Dim b As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs = Nothing
		'		gvScanContent_RowClick(sender, b)
	End Sub

	Private Sub RadialMenu1_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles RadialMenu1.MouseClick
		'Dim test As DBInformation = New DBInformation With {.SelectedFileID = m_SelectedContenctID, _
		'																										.SelectedRecordNumber = CInt(Me.lueReportNumber.EditValue)}
		Dim data = SelectedContentRecord
		If data Is Nothing Then Return
		Me.rmRPOpen.Visible = data.RecordNumber > 0
		Me.rmRPDelete.Visible = data.ID > 0
		Me.rmRPPrint.Visible = data.ID > 0

	End Sub

	Private Sub PdfViewer1_CurrentPageChanged(sender As Object, e As DevExpress.XtraPdfViewer.PdfCurrentPageChangedEventArgs) Handles PdfViewer1.CurrentPageChanged

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Seite {0} von {1}"), PdfViewer1.CurrentPageNumber, PdfViewer1.PageCount)

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub OnPdfViewer1_DocumentChanged(sender As Object, e As DevExpress.XtraPdfViewer.PdfDocumentChangedEventArgs) Handles PdfViewer1.DocumentChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If IsForIndividualReport Then SaveIndividualReportIntoDatabase()

	End Sub

	'Private Sub PdfViewer1_PopupMenuShowing(sender As Object, e As DevExpress.XtraPdfViewer.PdfPopupMenuShowingEventArgs) Handles PdfViewer1.PopupMenuShowing

	'	e.Menu.BeginUpdate()
	'	e.Menu.ItemLinks.Insert(1, New DevExpress.XtraBars.BarButtonItem)
	'	e.Menu.ItemLinks(1).Caption = "Move Page"
	'	e.Menu.ItemLinks(1).Item.Name = "PrintReport"
	'	e.Menu.ItemLinks(1).Item.ItemShortcut = New DevExpress.XtraBars.BarShortcut((Keys.Alt Or Keys.P))

	'	AddHandler e.Menu.ItemLinks(1).Item.ItemClick, AddressOf Item_ItemClick

	'	'e.Menu.ItemLinks(1).Item.ItemClick = (e.Menu.ItemLinks(1).Item.ItemClick + Item_ItemClick)
	'	e.Menu.EndUpdate()


	'	'e.Menu.ItemLinks.Insert(0, New PdfOpenFileCommand(PdfViewer1).CreateContextMenuBarItem(e.Menu.Manager))
	'End Sub


	'Private Sub Item_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

	'	Trace.WriteLine(e.Link.Caption)

	'End Sub




#Region "Helper Classess"

	''' <summary>
	''' Category view data.
	''' </summary>
	Class CategoryVieData

		Public Property CategoryNumber As Integer?
		Public Property Description As String

	End Class


	Private Class YearValueView
		Public Property Value As Integer

	End Class

	Private Sub grdScanContent_Click(sender As Object, e As EventArgs) Handles grdScanContent.Click

	End Sub

#End Region


End Class
