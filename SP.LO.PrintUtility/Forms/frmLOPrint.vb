
Option Strict Off

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects


Imports System.Reflection.Assembly
Imports System.IO
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Threading
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SPS.MA.Lohn.Utility.SPSLohnUtility
Imports SP.Infrastructure.UI
Imports SPS.Listing.Print.Utility

Imports SP.LO.PrintUtility.ClsDataDetail
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPS.MA.Lohn.Utility
Imports SP.DatabaseAccess.Listing.DataObjects.PayrollSearchData
Imports DevExpress.XtraTab.ViewInfo
Imports SP.DatabaseAccess.PayrollMng
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.ButtonsPanelControl
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class frmLOPrint
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private consts"

	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING As String = "MD_{0}/Mailing"

#End Region

#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	'Private LOSettings As ClsLOSetting
	Private _ClsFunc As New ClsDivFunc


	Private bAllowedtowrite As Boolean

	Private PrintListingThread As Thread

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String

	Private m_SearchData As PayrollSearchData

	Private m_SelectedPayroll As List(Of PayrollPrintData)

	Private m_SelectedPrintValue As WOSSENDValue

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private m_MailingSetting As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	Private m_SearchType As SearchTypeEnum

	Private m_PayrollEachFileName As String
	Private m_PayrollZipFileName As String

	Private Enum SearchTypeEnum
		PRINT
		EMAIL
	End Enum

#End Region


#Region "Private properties"

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private Property SelectedYear2Print As Integer

	Private Property PrintJobNr As String
	Private Property SQL4Print As String

	Private Property bPrintAsDesign As Boolean
	Private Property bPrintAsExport As Boolean
	Private Property bSendPrintJob2WOS As Boolean
	Private Property bSend_And_PrintJob2WOS As Boolean
	Private Property WOSProperty4Search As Short

	Private Property SelectedLONr As New List(Of Integer)
	Private Property SelectedMANr As New List(Of Integer)
	Private Property SelectedData2WOS As New List(Of Boolean)
	Private Property SelectedMALang As New List(Of String)
	Private Property ChangedSearchPrintContent As Boolean
	Private Property ChangedSearchDetailContent As Boolean


	''' <summary>
	''' Gets the selected payroll.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As PayrollPrintData
		Get
			Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), PayrollPrintData)

					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

#Region "xml data"

	''' <summary>
	''' Gets the email subject for zv.
	''' </summary>
	Private ReadOnly Property PayrollEachFileName As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/payrolleachfilename", m_MailingSetting))
			If String.IsNullOrWhiteSpace(settingValue) Then settingValue = "LO {Nummer} {MDName} - {MDOrt}"

			Return settingValue
		End Get
	End Property

	Private ReadOnly Property PayrollZipFileName As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/payrollzipfilename", m_MailingSetting))
			If String.IsNullOrWhiteSpace(settingValue) Then settingValue = "Lohnabrechnungen {MDName} - {MDOrt}"

			Return settingValue
		End Get
	End Property

	Private ReadOnly Property EmployeeWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property


#End Region


#End Region


#Region "public Properties"

	Public Property LOSetting As ClsLOSetting

#End Region

#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
		m_MailingSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING, m_InitializationData.MDData.MDNr)

		Reset()

		LoadMandantenDropDown()

		TranslateControls()

	End Sub

#End Region

#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()
	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			m_InitializationData = ChangeMandantData

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub


#End Region


	Private Sub Reset()

		ResetMandantenDropDown()

		Dim options As BeakPanelOptions
		Dim buttonPanelOptions As FlyoutPanelButtonOptions
		buttonPanelOptions = flyoutPanel.OptionsButtonPanel

		options = flyoutPanel.OptionsBeakPanel
		options.CloseOnOuterClick = False
		'buttonPanelOptions.Buttons.Insert(0, CreateButton())
		flyoutPanel.OptionsButtonPanel.ShowButtonPanel = True

		rtfContent.Unit = DevExpress.Office.DocumentUnit.Centimeter
		rtfContent.Document.Sections(0).Page.PaperKind = Printing.PaperKind.A4
		rtfContent.Text = String.Empty
		rtfContent.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden

		rtfContent.Font = New System.Drawing.Font("Calibri", 10, FontStyle.Regular)
		rtfContent.Document.DefaultCharacterProperties.FontName = "Calibri"
		rtfContent.Document.DefaultCharacterProperties.FontSize = 10
		rtfContent.ReadOnly = True

		rtfContent.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple
		rtfContent.Views.SimpleView.Padding = New DevExpress.Portable.PortablePadding(0) ' New System.Windows.Forms.Padding(10) ' 
		rtfContent.Document.Sections(0).Margins.Left = 0
		rtfContent.Document.Sections(0).Margins.Right = 0
		rtfContent.Document.Sections(0).Margins.Top = 0
		rtfContent.Document.Sections(0).Margins.Bottom = 0
		rtfContent.ReadOnly = True

		xtabLo.CustomHeaderButtons(1).Visible = m_InitializationData.UserData.UserNr = 1


		m_PayrollEachFileName = PayrollEachFileName
		m_PayrollZipFileName = PayrollZipFileName

		m_SearchType = SearchTypeEnum.PRINT
		cbEMail.Checked = False
		cbPrintWOS.Checked = True

		tpMainSetting.SelectedPage = tnpWOSSetting
		tnpMailSetting.PageVisible = False
		bbiSendMail.Visibility = BarItemVisibility.Never

		If Not m_mandant.AllowedExportEmployee2WOS(lueMandant.EditValue, Now.Year) Then
			Me.btnWOSProperty.Text = String.Empty
			Me.btnWOSProperty.Visible = False
			Me.lblWOSBez.Visible = False
			cbEMail.Enabled = False
			tnpMailSetting.Enabled = False
		End If

		tpMainView.SelectedPageIndex = 0
		tnpMail.PageVisible = False
		tnpEMailSummery.PageVisible = False


		ResetPayrollGrid()
		ResetPayrollEMailGrid()
		ResetPayrollDetailGrid()

	End Sub


#Region "reset"

	Private Sub ResetPayrollGrid()

		gvPrint.OptionsView.ShowIndicator = False
		gvPrint.OptionsView.ShowAutoFilterRow = True
		gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPrint.OptionsView.ShowFooter = False
		gvPrint.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		'CType(grdPayrollDetail.MainView, GridView).OptionsPrint.PrintHeader = True
		CType(grdPrint.MainView, GridView).OptionsPrint.PrintHorzLines = False
		CType(grdPrint.MainView, GridView).OptionsPrint.PrintVertLines = False

		gvPrint.Columns.Clear()


		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "SelectedRec"
		columnSelectedRec.FieldName = "SelectedRec"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 50
		gvPrint.Columns.Add(columnSelectedRec)

		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.OptionsColumn.AllowEdit = False
		columnrecid.Name = "recid"
		columnrecid.FieldName = "recid"
		columnrecid.Visible = False
		gvPrint.Columns.Add(columnrecid)

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLONr.OptionsColumn.AllowEdit = False
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnLONr.Name = "LONr"
		columnLONr.FieldName = "LONr"
		columnLONr.Width = 60
		columnLONr.Visible = True
		gvPrint.Columns.Add(columnLONr)

		Dim columnzeitraum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitraum.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitraum.OptionsColumn.AllowEdit = False
		columnzeitraum.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnzeitraum.Name = "zeitraum"
		columnzeitraum.FieldName = "zeitraum"
		columnzeitraum.Visible = True
		columnzeitraum.Width = 50
		gvPrint.Columns.Add(columnzeitraum)

		Dim columnemployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeFullname.OptionsColumn.AllowEdit = False
		columnemployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeeFullname.Name = "employeeFullname"
		columnemployeeFullname.FieldName = "employeeFullname"
		columnemployeeFullname.Visible = True
		columnemployeeFullname.Width = 150
		gvPrint.Columns.Add(columnemployeeFullname)


		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.OptionsColumn.AllowEdit = False
		columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "createdfrom"
		columncreatedfrom.FieldName = "createdfrom"
		columncreatedfrom.Visible = True
		columncreatedfrom.Width = 100
		gvPrint.Columns.Add(columncreatedfrom)

		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedon.OptionsColumn.AllowEdit = False
		columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columncreatedon.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columncreatedon.DisplayFormat.FormatString = "G"
		columncreatedon.Name = "createdon"
		columncreatedon.FieldName = "createdon"
		columncreatedon.Visible = True
		columncreatedon.Width = 100
		gvPrint.Columns.Add(columncreatedon)


		grdPrint.DataSource = Nothing

	End Sub

	Private Sub ResetPayrollDetailGrid()

		gvPayrollDetail.OptionsView.ShowIndicator = False
		gvPayrollDetail.OptionsView.ShowAutoFilterRow = True
		gvPayrollDetail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPayrollDetail.OptionsView.ShowFooter = False
		gvPayrollDetail.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		'CType(grdPayrollDetail.MainView, GridView).OptionsPrint.PrintHeader = True
		CType(grdPayrollDetail.MainView, GridView).OptionsPrint.PrintHorzLines = False
		CType(grdPayrollDetail.MainView, GridView).OptionsPrint.PrintVertLines = False

		gvPayrollDetail.Columns.Clear()


		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.OptionsColumn.AllowEdit = False
		columnrecid.Name = "recid"
		columnrecid.FieldName = "recid"
		columnrecid.Visible = False
		gvPayrollDetail.Columns.Add(columnrecid)

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLONr.OptionsColumn.AllowEdit = False
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnLONr.Name = "LONr"
		columnLONr.FieldName = "LONr"
		columnLONr.Width = 60
		columnLONr.Visible = True
		gvPayrollDetail.Columns.Add(columnLONr)

		Dim columnzeitraum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitraum.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitraum.OptionsColumn.AllowEdit = False
		columnzeitraum.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnzeitraum.Name = "zeitraum"
		columnzeitraum.FieldName = "zeitraum"
		columnzeitraum.Visible = True
		columnzeitraum.Width = 50
		gvPayrollDetail.Columns.Add(columnzeitraum)

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLANr.OptionsColumn.AllowEdit = False
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
		columnLANr.Name = "LANr"
		columnLANr.FieldName = "LANr"
		columnLANr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnLANr.AppearanceHeader.Options.UseTextOptions = True
		columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnLANr.DisplayFormat.FormatString = "f4"
		columnLANr.Width = 50
		columnLANr.Visible = True
		gvPayrollDetail.Columns.Add(columnLANr)

		Dim columnemployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeFullname.OptionsColumn.AllowEdit = False
		columnemployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeeFullname.Name = "employeeFullname"
		columnemployeeFullname.FieldName = "employeeFullname"
		columnemployeeFullname.Visible = True
		columnemployeeFullname.Width = 150
		gvPayrollDetail.Columns.Add(columnemployeeFullname)

		Dim columnLALoText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLALoText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLALoText.OptionsColumn.AllowEdit = False
		columnLALoText.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnLALoText.Name = "LALoText"
		columnLALoText.FieldName = "LALoText"
		columnLALoText.Visible = True
		columnLALoText.Width = 200
		gvPayrollDetail.Columns.Add(columnLALoText)

		Dim columnm_Anz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Anz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnm_Anz.OptionsColumn.AllowEdit = False
		columnm_Anz.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
		columnm_Anz.Name = "m_Anz"
		columnm_Anz.FieldName = "m_Anz"
		columnm_Anz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Anz.AppearanceHeader.Options.UseTextOptions = True
		columnm_Anz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Anz.DisplayFormat.FormatString = "N2"
		columnm_Anz.Visible = True
		columnm_Anz.Width = 60
		gvPayrollDetail.Columns.Add(columnm_Anz)

		Dim columnm_Bas As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Bas.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnm_Bas.OptionsColumn.AllowEdit = False
		columnm_Bas.Caption = m_Translate.GetSafeTranslationValue("Basis")
		columnm_Bas.Name = "m_Bas"
		columnm_Bas.FieldName = "m_Bas"
		columnm_Bas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Bas.AppearanceHeader.Options.UseTextOptions = True
		columnm_Bas.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Bas.DisplayFormat.FormatString = "N2"
		columnm_Bas.Visible = True
		columnm_Bas.Width = 60
		gvPayrollDetail.Columns.Add(columnm_Bas)

		Dim columnm_Ans As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Ans.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnm_Ans.OptionsColumn.AllowEdit = False
		columnm_Ans.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
		columnm_Ans.Name = "m_Ans"
		columnm_Ans.FieldName = "m_Ans"
		columnm_Ans.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Ans.AppearanceHeader.Options.UseTextOptions = True
		columnm_Ans.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Ans.DisplayFormat.FormatString = "F5"
		columnm_Ans.Visible = True
		columnm_Ans.Width = 60
		gvPayrollDetail.Columns.Add(columnm_Ans)

		Dim columnm_btr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_btr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnm_btr.OptionsColumn.AllowEdit = False
		columnm_btr.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnm_btr.Name = "m_btr"
		columnm_btr.FieldName = "m_btr"
		columnm_btr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_btr.AppearanceHeader.Options.UseTextOptions = True
		columnm_btr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_btr.DisplayFormat.FormatString = "N4"
		columnm_btr.Visible = True
		columnm_btr.Width = 60
		gvPayrollDetail.Columns.Add(columnm_btr)


		grdPayrollDetail.DataSource = Nothing

	End Sub

	Private Sub ResetPayrollEMailGrid()

		gvPayrollEMail.OptionsView.ShowIndicator = False
		gvPayrollEMail.OptionsView.ShowAutoFilterRow = True
		gvPayrollEMail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPayrollEMail.OptionsView.ShowFooter = False
		gvPayrollEMail.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvPayrollEMail.Columns.Clear()


		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "SelectedRec"
		columnSelectedRec.FieldName = "SelectedRec"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 50
		gvPayrollEMail.Columns.Add(columnSelectedRec)

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLONr.OptionsColumn.AllowEdit = False
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnLONr.Name = "LONr"
		columnLONr.FieldName = "LONr"
		columnLONr.Width = 60
		columnLONr.Visible = True
		gvPayrollEMail.Columns.Add(columnLONr)

		Dim columnzeitraum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitraum.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitraum.OptionsColumn.AllowEdit = False
		columnzeitraum.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnzeitraum.Name = "zeitraum"
		columnzeitraum.FieldName = "zeitraum"
		columnzeitraum.Visible = True
		columnzeitraum.Width = 50
		gvPayrollEMail.Columns.Add(columnzeitraum)


		Dim columnemployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeFullname.OptionsColumn.AllowEdit = False
		columnemployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeeFullname.Name = "employeeFullname"
		columnemployeeFullname.FieldName = "employeeFullname"
		columnemployeeFullname.Visible = True
		columnemployeeFullname.Width = 150
		gvPayrollEMail.Columns.Add(columnemployeeFullname)

		Dim columnREEmail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnREEmail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnREEmail.OptionsColumn.AllowEdit = False
		columnREEmail.Caption = m_Translate.GetSafeTranslationValue("EMail")
		columnREEmail.Name = "EmployeeEMail"
		columnREEmail.FieldName = "EmployeeEMail"
		columnREEmail.Visible = True
		columnREEmail.Width = 100
		gvPayrollEMail.Columns.Add(columnREEmail)


		grdPayrollEMail.DataSource = Nothing

	End Sub


#End Region


#Region "loading data"

	Private Function LoadPayrollPrintList() As Boolean

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim listOfData = m_ListingDatabaseAccess.LoadPayrollsPrintData(m_SearchData)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnabrechnungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
						Select New PayrollPrintData With
									 {.recID = person.recID,
									 .LONr = person.LONr,
										.MANr = person.MANr,
										.MDNr = person.MDNr,
										.monat = person.monat,
										.jahr = person.jahr,
										.employeeLanguage = person.employeeLanguage,
										.EmployeeEMail = person.EmployeeEMail,
										.createdon = person.createdon,
										.createdfrom = person.createdfrom,
										.Send2WOS = person.Send2WOS,
										.SendDataWithEMail = person.SendDataWithEMail,
										.employeelastname = person.employeelastname,
										.employeefirstname = person.employeefirstname,
										.SelectedRec = tgsSelection.EditValue
									 }).ToList()

		Dim listDataSource As BindingList(Of PayrollPrintData) = New BindingList(Of PayrollPrintData)

		If tgsNoEMailEmployee.EditValue Then
			For Each p In gridData
				If Not p.SendDataWithEMail.GetValueOrDefault(False) Then
					listDataSource.Add(p)
				End If
			Next

		Else
			For Each p In gridData
				listDataSource.Add(p)
			Next

		End If


		grdPrint.DataSource = listDataSource
		bsiPrintinfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Return Not listOfData Is Nothing

	End Function

	Private Function LoadPayrollDetailList() As Boolean

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim listOfData = m_ListingDatabaseAccess.LoadPayrollsDetailData(m_SearchData)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndetail-Daten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
						Select New PayrollDetailData With
									 {.recID = person.recID,
									 .LONr = person.LONr,
										.MANr = person.MANr,
										.MDNr = person.MDNr,
										.monat = person.monat,
										.jahr = person.jahr,
										.employeeLanguage = person.employeeLanguage,
										.LALoText = person.LALoText,
										.LANr = person.LANr,
										.m_Anz = person.m_Anz,
										.m_Bas = person.m_Bas,
										.m_Ans = person.m_Ans,
										.m_btr = person.m_btr,
										.Send2WOS = person.Send2WOS,
										.employeelastname = person.employeelastname,
										.employeefirstname = person.employeefirstname
									 }).ToList()

		Dim listDataSource As BindingList(Of PayrollDetailData) = New BindingList(Of PayrollDetailData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdPayrollDetail.DataSource = listDataSource
		bsiDetailInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Detail-Datensätze: {0}"), listDataSource.Count)

		Return Not listOfData Is Nothing

	End Function

	Private Function LoadPayrollEMailList() As Boolean

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim listOfData = m_ListingDatabaseAccess.LoadPayrollsPrintData(m_SearchData)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnabrechnungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
						Select New PayrollPrintData With
									 {.recID = person.recID,
									 .LONr = person.LONr,
										.MANr = person.MANr,
										.MDNr = person.MDNr,
										.monat = person.monat,
										.jahr = person.jahr,
										.employeeLanguage = person.employeeLanguage,
										.EmployeeEMail = person.EmployeeEMail,
										.createdon = person.createdon,
										.createdfrom = person.createdfrom,
										.Send2WOS = person.Send2WOS,
										.SendDataWithEMail = person.SendDataWithEMail,
										.SendAsZIP = person.SendAsZIP,
										.employeelastname = person.employeelastname,
										.employeefirstname = person.employeefirstname,
										.SelectedRec = tgsSelection.EditValue
									 }).ToList()

		Dim listDataSource As BindingList(Of PayrollPrintData) = New BindingList(Of PayrollPrintData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdPayrollEMail.DataSource = listDataSource
		bsiPrintinfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Return Not listOfData Is Nothing

	End Function


#End Region

	Private Sub FocusPrintData(ByVal recordID As Integer)

		If Not grdPrint.DataSource Is Nothing Then

			Dim esSalaryData = CType(gvPrint.DataSource, BindingList(Of PayrollPrintData))

			Dim index = esSalaryData.ToList().FindIndex(Function(data) data.recID >= recordID)

			Dim rowHandle = gvPrint.GetRowHandle(index)
			gvPrint.FocusedRowHandle = rowHandle

		End If

	End Sub

	Private Sub FocusPayrollDetailData(ByVal recordID As Integer)

		If Not grdPayrollDetail.DataSource Is Nothing Then

			Dim esSalaryData = CType(gvPayrollDetail.DataSource, BindingList(Of PayrollDetailData))

			Dim index = esSalaryData.ToList().FindIndex(Function(data) data.recID >= recordID)

			Dim rowHandle = gvPayrollDetail.GetRowHandle(index)
			gvPayrollDetail.FocusedRowHandle = rowHandle

		End If

	End Sub

	Private Function GetSelectedPayrollItems() As List(Of PayrollPrintData)

		m_SelectedPayroll = New List(Of PayrollPrintData)
		gvPrint.FocusedColumn = gvPrint.VisibleColumns(1)
		grdPrint.RefreshDataSource()
		Dim printList As BindingList(Of PayrollPrintData) = grdPrint.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		SelectedLONr.Clear()
		SelectedMANr.Clear()
		SelectedData2WOS.Clear()
		SelectedMALang.Clear()

		For Each receiver In sentList

			If receiver.SelectedRec Then
				m_SelectedPayroll.Add(New PayrollPrintData With {
					.recID = receiver.recID,
					.createdfrom = receiver.createdfrom,
					.createdon = receiver.createdon,
					.employeefirstname = receiver.employeefirstname,
					.employeeLanguage = receiver.employeeLanguage,
					.employeelastname = receiver.employeelastname,
					.jahr = receiver.jahr,
					.LONr = receiver.LONr,
					.MANr = receiver.MANr,
					.MDNr = receiver.MDNr,
					.monat = receiver.monat,
					.Send2WOS = receiver.Send2WOS
				})

				SelectedLONr.Add(receiver.LONr)
				SelectedMANr.Add(receiver.MANr)
				SelectedData2WOS.Add(If(m_SelectedPrintValue = WOSSENDValue.PrintWithoutSending, False, receiver.Send2WOS))
				SelectedMALang.Add(receiver.employeeLanguage)

			End If

		Next


		Return m_SelectedPayroll

	End Function

	Private Function GetSelectedPayrollEMailItems() As List(Of PayrollPrintData)

		m_SelectedPayroll = New List(Of PayrollPrintData)
		gvPayrollEMail.FocusedColumn = gvPayrollEMail.VisibleColumns(1)
		grdPayrollEMail.RefreshDataSource()
		Dim printList As BindingList(Of PayrollPrintData) = grdPayrollEMail.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		SelectedLONr.Clear()
		SelectedMANr.Clear()
		SelectedData2WOS.Clear()
		SelectedMALang.Clear()

		For Each receiver In sentList

			If receiver.SelectedRec Then
				m_SelectedPayroll.Add(New PayrollPrintData With {
					.recID = receiver.recID,
					.createdfrom = receiver.createdfrom,
					.createdon = receiver.createdon,
					.employeefirstname = receiver.employeefirstname,
					.employeeLanguage = receiver.employeeLanguage,
					.employeelastname = receiver.employeelastname,
					.jahr = receiver.jahr,
					.LONr = receiver.LONr,
					.MANr = receiver.MANr,
					.MDNr = receiver.MDNr,
					.monat = receiver.monat,
					.SendAsZIP = receiver.SendAsZIP,
					.Send2WOS = receiver.Send2WOS
				})

				SelectedLONr.Add(receiver.LONr)
				SelectedMANr.Add(receiver.MANr)
				SelectedData2WOS.Add(If(m_SelectedPrintValue = WOSSENDValue.PrintWithoutSending, False, receiver.Send2WOS))
				SelectedMALang.Add(receiver.employeeLanguage)

			End If

		Next


		Return m_SelectedPayroll

	End Function

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub OngvPrint_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvPrint.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPrint.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, PayrollPrintData)

				Select Case column.Name.ToLower

					Case "employeeFullname".ToLower
						If viewData.MANr.HasValue Then OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub

	Sub OngvPayrollEMail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvPayrollEMail.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPayrollEMail.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, PayrollPrintData)

				Select Case column.Name.ToLower

					Case "employeeFullname".ToLower
						If viewData.MANr.HasValue Then OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub

	Sub OngvPayrollDetail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvPayrollDetail.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPayrollDetail.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, PayrollDetailData)

				Select Case column.Name.ToLower

					Case "employeeFullname".ToLower
						If viewData.MANr.HasValue Then OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub

	Private Sub OngvPayrollDetail_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvPayrollDetail.CustomColumnDisplayText

		If e.Column.FieldName = "m_anz" Or e.Column.FieldName = "m_ans" Or e.Column.FieldName = "m_bas" Or e.Column.FieldName = "m_btr" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub


	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
		gpSuchKriterien.Text = m_Translate.GetSafeTranslationValue(gpSuchKriterien.Text)

		tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OffText)
		tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OnText)

		xtabLOPrint.Text = m_Translate.GetSafeTranslationValue(xtabLOPrint.Text)
		xtabLODetail.Text = m_Translate.GetSafeTranslationValue(xtabLODetail.Text)

		lblSortierennach.Text = m_Translate.GetSafeTranslationValue(lblSortierennach.Text)
		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lbljahr.Text = m_Translate.GetSafeTranslationValue(lbljahr.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)
		lblKandidatenNr.Text = m_Translate.GetSafeTranslationValue(lblKandidatenNr.Text)
		lblLohnNr.Text = m_Translate.GetSafeTranslationValue(lblLohnNr.Text)
		lblWOSBez.Text = m_Translate.GetSafeTranslationValue(lblWOSBez.Text)

		bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue(bsiPrintinfo.Caption)
		bsiDetailInfo.Caption = m_Translate.GetSafeTranslationValue(bsiDetailInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiSetting.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSetting.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)

	End Sub


#Region "Form"


	Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmLOPrint_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.NoEMailEmployeeForPrint = tgsNoEMailEmployee.EditValue
			My.Settings.IndividalFilesForEMail = tgsIndividalFiles.EditValue

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmLOPrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If My.Settings.iHeight > 0 Then Me.Height = My.Settings.iHeight
			If My.Settings.iWidth > 0 Then Me.Width = My.Settings.iWidth
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			tgsNoEMailEmployee.EditValue = My.Settings.NoEMailEmployeeForPrint
			tgsIndividalFiles.EditValue = My.Settings.IndividalFilesForEMail

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))
		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.ToString))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

		Try
			If Not m_mandant.AllowedExportEmployee2WOS(LOSetting.SelectedMDNr, Now.Year) Then
				Me.btnWOSProperty.Visible = False
				Me.btnWOSProperty.Text = String.Empty
				Me.lblWOSBez.Visible = False

			Else
				CreateWOSPopup()
				Me.btnWOSProperty.Text = m_Translate.GetSafeTranslationValue("Alle")

			End If
			Me.WOSProperty4Search = 2

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.btnWOSProperty bilden: {1}", strMethodeName, ex.ToString))

		End Try

		Try

			Dim strMANr As String = String.Empty
			For i As Integer = 0 To LOSetting.SelectedMANr.Count - 1
				strMANr &= If(Not String.IsNullOrWhiteSpace(strMANr), ",", "") & LOSetting.SelectedMANr(i)
			Next
			Dim strLONr As String = String.Empty
			For i As Integer = 0 To LOSetting.SelectedLONr.Count - 1
				strLONr &= If(Not String.IsNullOrWhiteSpace(strLONr), ",", "") & LOSetting.SelectedLONr(i)
			Next
			Me.cbo_MANr.Text = strMANr
			Me.cbo_LONr.Text = strLONr
			Me.cbo_Month.Text = LOSetting.SelectedMonth(0)
			Me.cbo_Year.Text = LOSetting.SelectedYear(0)

			Me.bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
			Me.ChangedSearchPrintContent = True
			Me.ChangedSearchDetailContent = True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Variable setzen: {1}", strMethodeName, ex.ToString))

		End Try

		Try
			If LOSetting.SearchAutomatic Then
				LoadLODataList()

				If LOSetting.ShowLODetails Then
					Me.bbiPrint.Enabled = False
					Me.bbiExport.Enabled = False
					Me.xtabLo.SelectedTabPageIndex = 1
				End If

			End If
			Me.CboSort.Properties.Items.Clear()
			Me.CboSort.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Kandidatenname")))
			Me.CboSort.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennummer")))
			Me.CboSort.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Erstellt am")))
			Me.CboSort.Text = String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Kandidatenname"))

			Try
				If Not IsUserActionAllowed(0, 551) Then Me.bbiDelete.Visibility = BarItemVisibility.Never
				If Not IsUserActionAllowed(0, 554) Then
					Me.bbiPrint.Visibility = BarItemVisibility.Never
					Me.bbiExport.Visibility = BarItemVisibility.Never
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Rechte überprüfen: {1}", strMethodeName, ex.ToString))
				Me.bbiPrint.Visibility = BarItemVisibility.Never
				Me.bbiExport.Visibility = BarItemVisibility.Never
				Me.bbiDelete.Visibility = BarItemVisibility.Never
			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.SearchAutomatic: {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub form_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			'For i As Integer = 0 To GetExecutingAssembly.GetReferencedAssemblies.Count - 1
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase) ' GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next

			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)

		End If
	End Sub

#End Region


#Region "Funktionen für Reset der Controls..."

	Sub BlankFields()
		ResetAllTabEntries()

		Me.bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

	End Sub


	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetAllTabEntries()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			For Each ctrls In Me.Controls
				ResetControl(ctrls)
			Next

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			'_ClsErrException.MessageBoxShowError(m_InitializationData.UserData.UserNr, "ResetControl", ex)
		End Try

	End Sub

	Private Sub ResetControl(ByVal con As Control)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			' Rekursiver Aufruf
			If con.HasChildren Then
				For Each childCon In con.Controls
					ResetControl(childCon)
				Next
			Else
				' Sonst Control zurücksetzen
				If TypeOf (con) Is TextBox Then
					Dim tb As TextBox = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is System.Windows.Forms.ComboBox Or TypeOf (con) Is ComboBoxEdit Or TypeOf (con) Is CheckedComboBoxEdit Then
					Dim cbo As System.Windows.Forms.ComboBox = con
					cbo.Text = String.Empty
					cbo.SelectedIndex = -1

				ElseIf TypeOf (con) Is ListBox Then
					Dim lst As ListBox = con
					lst.Items.Clear()

				End If
			End If

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub


#End Region


#Region "Sonstige Funktionen..."

	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception

		End Try

	End Function

	Private Sub txt_Ort_KeyPress(ByVal sender As Object,
																		ByVal e As System.Windows.Forms.KeyPressEventArgs)

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.ToString, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

#End Region


	Sub FillFieldsWithDefaults()

		BlankFields()
		Me.cbo_Month.Text = If(Now.Day < 15, Now.Month - 1, Now.Month)
		Me.cbo_Year.Text = If(Now.Day < 15 And Now.Month = 1, Now.Year - 1, Now.Year)

	End Sub

	Private Sub InitialSearchData()
		Dim lilonr As String() = Me.cbo_LONr.Text.Split(CChar(","))
		Dim liMAnr As String() = Me.cbo_MANr.Text.Split(CChar(","))
		Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))
		Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))

		If Not LOSetting.SelectedYear Is Nothing Then LOSetting.SelectedYear.Clear()
		If Not LOSetting.SelectedMonth Is Nothing Then LOSetting.SelectedMonth.Clear()
		LOSetting.SelectedWOSProperty = Me.WOSProperty4Search
		LOSetting.SortBezeichnung = Me.CboSort.Text

		If Not LOSetting.SelectedMANr Is Nothing Then LOSetting.SelectedMANr.Clear() Else LOSetting.SelectedMANr = New List(Of Integer)
		If Not LOSetting.SelectedLONr Is Nothing Then LOSetting.SelectedLONr.Clear() Else LOSetting.SelectedLONr = New List(Of Integer)

		LOSetting.ShowNullBetrag = True
		For i As Integer = 0 To aYear.Length - 1
			LOSetting.SelectedYear.Add(CInt(Val(aYear(i))))
		Next
		For i As Integer = 0 To aMonth.Length - 1
			LOSetting.SelectedMonth.Add(CInt((Val(aMonth(i)))))
		Next

		For i As Integer = 0 To liMAnr.Length - 1
			Try
				If liMAnr(i).Contains(CChar("-")) Then
					Dim aNr As String() = liMAnr(i).Split(CChar("-"))
					Dim iFirstNr As Integer = CInt(aNr(0))
					Dim iLastNr As Integer = CInt(aNr(1))
					For j As Integer = iFirstNr To iLastNr
						If j <> 0 Then LOSetting.SelectedMANr.Add(j)
					Next
				Else
					If CInt(Val(liMAnr(i))) <> 0 Then LOSetting.SelectedMANr.Add(CInt(Val(liMAnr(i))))
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		Next

		For i As Integer = 0 To lilonr.Length - 1
			Try
				If lilonr(i).Contains(CChar("-")) Then
					Dim alonr As String() = lilonr(i).Split(CChar("-"))
					Dim iFirstLONr As Integer = CInt(alonr(0))
					Dim iLastLONr As Integer = CInt(alonr(1))
					For j As Integer = iFirstLONr To iLastLONr
						If j <> 0 Then LOSetting.SelectedLONr.Add(j)
					Next
				Else
					If CInt(Val(lilonr(i))) <> 0 Then LOSetting.SelectedLONr.Add(CInt(Val(lilonr(i))))
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try
		Next


		m_SearchData = New PayrollSearchData

		m_SearchData.MDNr = lueMandant.EditValue
		m_SearchData.jahr = LOSetting.SelectedYear
		m_SearchData.monat = LOSetting.SelectedMonth
		m_SearchData.MANr = LOSetting.SelectedMANr
		m_SearchData.LONr = LOSetting.SelectedLONr
		If LOSetting.SelectedWOSProperty = 0 Then m_SearchData.mawos = PayrollSearchData.WOSValue.WithoutWOS
		If LOSetting.SelectedWOSProperty = 1 Then m_SearchData.mawos = PayrollSearchData.WOSValue.WithWOS
		If LOSetting.SelectedWOSProperty = 2 Then m_SearchData.mawos = PayrollSearchData.WOSValue.All

		m_SearchData.sortvalue = Val(CboSort.EditValue)
		m_SearchData.GroupByEMail = m_SearchType = SearchTypeEnum.EMAIL

	End Sub

	Private Sub cbo_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cbo_MANr.ButtonClick
		Dim frmTest As New frmSearchRec(m_InitializationData, "Kandidat-Nr.", CInt(Val(Me.cbo_Year.Text)), CInt(Val(Me.cbo_Month.Text)))

		ClsDataDetail.strButtonValue = Me.cbo_MANr.Text

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMyValue(_ClsFunc.GetSelektion)
		Me.cbo_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

	Private Sub OnxtabLo_CustomHeaderButtonClick(sender As Object, e As CustomHeaderButtonEventArgs) Handles xtabLo.CustomHeaderButtonClick

		Select Case Val(e.Button.Tag)
			Case 1
				Dim filename = Path.GetRandomFileName
				filename = Path.ChangeExtension(filename, "xlsx")
				filename = Path.Combine(m_InitialData.UserData.spAllowedPath, filename)

				Dim advOptions As DevExpress.XtraPrinting.XlsxExportOptionsEx = New DevExpress.XtraPrinting.XlsxExportOptionsEx()
				advOptions.AllowSortingAndFiltering = DevExpress.Utils.DefaultBoolean.False
				advOptions.ShowColumnHeaders = DevExpress.Utils.DefaultBoolean.True
				advOptions.ShowGridLines = True
				advOptions.ApplyFormattingToEntireColumn = DevExpress.Utils.DefaultBoolean.True
				advOptions.AllowGrouping = DevExpress.Utils.DefaultBoolean.False
				advOptions.ShowTotalSummaries = DevExpress.Utils.DefaultBoolean.False

				If xtabLo.SelectedTabPage Is xtabLOPrint Then
					If tnpMail.Visible Then
						advOptions.SheetName = m_Translate.GetSafeTranslationValue("Lohnabrechnungen zum Mail-Versand")
						grdPayrollEMail.ExportToXlsx(filename, advOptions)
					Else
						advOptions.SheetName = m_Translate.GetSafeTranslationValue("Lohnabrechnungen zum Drucken oder Löschen")
						grdPrint.ExportToXlsx(filename, advOptions)

					End If

				ElseIf xtabLo.SelectedTabPage Is xtabLODetail Then
					advOptions.SheetName = m_Translate.GetSafeTranslationValue("Lohndetail")
					grdPayrollDetail.ExportToXlsx(filename, advOptions)

				Else
					Return

				End If
				Process.Start(filename)

			Case 2
				Dim data = SelectedMANr
				LoadBugReport()


			Case Else
				Return

		End Select

	End Sub

	Private Sub cbo_LONr_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_LONr.QueryPopUp
		Dim liMAnr As String() = Me.cbo_MANr.Text.Split(CChar(","))
		Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))
		Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))

		Try
			LOSetting.SelectedYear.Clear()
			LOSetting.SelectedMonth.Clear()
			LOSetting.SelectedWOSProperty = Me.WOSProperty4Search
			If Not IsNothing(LOSetting.SelectedMANr) Then LOSetting.SelectedMANr.Clear()
			LOSetting.SelectedLONr.Clear()

			For i As Integer = 0 To aYear.Length - 1
				LOSetting.SelectedYear.Add(CInt(Val(aYear(i))))
			Next
			For i As Integer = 0 To aMonth.Length - 1
				LOSetting.SelectedMonth.Add(CInt((aMonth(i))))
			Next

			For i As Integer = 0 To liMAnr.Length - 1
				If CInt(Val(liMAnr(i))) <> 0 Then LOSetting.SelectedMANr.Add(CInt(Val(liMAnr(i))))
			Next
			Me.Cursor = Cursors.WaitCursor
			ListLohnNr(cbo_LONr, LOSetting)

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		Finally
			Me.Cursor = Cursors.Default
		End Try

	End Sub

	Private Sub cbo_Year_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Year.QueryPopUp
		ListLOYear(sender)
	End Sub

	Private Sub cbo_Month_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Month.QueryPopUp
		ListLOMonth(sender, Me.cbo_Year.Text)
	End Sub


#Region "WOS-Popup..."

	Sub CreateWOSPopup()
		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		'Dim liMnu As New List(Of String) From {"Nur WOS-Kandidaten#0",
		'																			 "Keine WOS-Kandidaten#10",
		'																			 "Alle#2"}

		'Try
		'	Me.btnWOSProperty.DropDownControl = popupMenu
		'	For i As Integer = 0 To liMnu.Count - 1
		'		Dim myValue As String() = liMnu(i).Split(CChar("#"))

		'		If myValue(0).ToString <> String.Empty Then
		'			popupMenu.Manager = BarManager1

		'			Dim itm As New DevExpress.XtraBars.BarButtonItem

		'			itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
		'			itm.Name = myValue(1).ToString

		'			popupMenu.AddItem(itm)
		'			AddHandler itm.ItemClick, AddressOf GetWOSPopupMnu

		'		End If

		'	Next

		'Catch ex As Exception
		'	m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		'End Try





		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim BarManagerContextMenu As New BarManager

		Dim data = New List(Of MenuSettingData) From {New MenuSettingData With {.Value = 0, .Label = m_Translate.GetSafeTranslationValue("Nur WOS-Kandidaten")},
			New MenuSettingData With {.Value = 1, .Label = m_Translate.GetSafeTranslationValue("Keine WOS-Kandidaten")},
			New MenuSettingData With {.Value = 2, .Label = "-" & m_Translate.GetSafeTranslationValue("Alle")}}

		Try
			Me.btnWOSProperty.DropDownControl = popupMenu
			For Each itm In data

				If Not String.IsNullOrWhiteSpace(itm.Label) Then
					popupMenu.Manager = BarManagerContextMenu
					Dim bbiItem As New DevExpress.XtraBars.BarButtonItem
					Dim caption As String = itm.Label
					Dim beginGroup As Boolean = False

					If caption.StartsWith("-") Then
						beginGroup = True
						caption = itm.Label.Remove(0, 1)
					End If

					bbiItem.Caption = caption
					bbiItem.Name = itm.Value


					popupMenu.AddItem(bbiItem).BeginGroup = beginGroup

					AddHandler bbiItem.ItemClick, AddressOf GetWOSPopupMnu

				End If

			Next

			popupMenu.ItemLinks(2).Focus()
			btnWOSProperty.Text = popupMenu.ItemLinks(2).Caption

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Public Sub GetWOSPopupMnu(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.btnWOSProperty.Text = e.Item.Caption
		Me.WOSProperty4Search = CShort(e.Item.Name)
	End Sub

#End Region


	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Lohnabrechnungen Drucken#PrintLO",
																					 "Drucken ohne Übermittlung#PrintLO",
																					 "_WOS -> übermitteln / restliche -> Drucken#SendWOS_PrintRest",
																					 "Drucken mit Übermittlung#SendAndPrint"}
		Try

			bbiPrint.Manager = Me.BarManager1
			Dim allowedEmployeWOS As Boolean = m_mandant.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year)
			BarManager1.ForceInitialize()
			If allowedEmployeWOS Then
				liMnu.RemoveAt(0)

			Else
				liMnu.RemoveRange(1, liMnu.Count - 1)

			End If

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString.Replace("_", ""))
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(0).StartsWith("_") Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		bPrintAsDesign = False
		Me.bPrintAsExport = False
		Me.bSendPrintJob2WOS = False
		Me.bSend_And_PrintJob2WOS = False

		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintLO".ToUpper
					m_SelectedPrintValue = WOSSENDValue.PrintWithoutSending

				Case "SendWOS_PrintRest".ToUpper
					Me.bSendPrintJob2WOS = True
					m_SelectedPrintValue = WOSSENDValue.PrintOtherSendWOS

				Case "SendAndPrint".ToUpper
					Me.bSend_And_PrintJob2WOS = True
					m_SelectedPrintValue = WOSSENDValue.PrintAndSend

				Case Else
					Return

			End Select

			Dim listData = GetSelectedPayrollItems()
			If listData.Count > 0 Then
				StartPrinting()

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		LoadLODataList()
	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim listData = GetSelectedPayrollItems()
			If listData.Count > 0 Then
				StartExporting()

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

			End If

		Catch ex As Exception
			m_Logger.LogInfo(String.Format("{0}:{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub StartPrinting()
		'Dim _Setting As New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting

		bPrintAsDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 558, lueMandant.EditValue) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		bPrintAsExport = False
		bSendPrintJob2WOS = m_SelectedPrintValue = WOSSENDValue.PrintOtherSendWOS
		bSend_And_PrintJob2WOS = m_SelectedPrintValue = WOSSENDValue.PrintAndSend

		Me.SQL4Print = String.Empty
		Me.PrintJobNr = "9.1"

		Try

			Dim _Setting = New ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																							 .SQL2Open = Me.SQL4Print,
																																							 .JobNr2Print = Me.PrintJobNr,
																																							 .Is4Export = Me.bPrintAsExport,
																																							 .SendData2WOS = Me.bSendPrintJob2WOS,
																																							 .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS,
																																							 .liLONr2Print = Me.SelectedLONr,
																																							 .liMANr2Print = Me.SelectedMANr,
																																							 .liLOSend2WOS = Me.SelectedData2WOS,
																																							 .WOSSendValueEnum = m_SelectedPrintValue,
																																							 .SortValue = Val(CboSort.EditValue),
																																							 .LiMALang = Me.SelectedMALang,
																																							 .SelectedLONr2Print = Val(Me.cbo_LONr.Text),
																																							 .SelectedMANr2Print = Val(Me.cbo_MANr.Text),
																																																				 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																																				 .LogedUSNr = m_InitializationData.UserData.UserNr,
																																								 .PerosonalizedData = ClsDataDetail.ProsonalizedData, .TranslationData = ClsDataDetail.TranslationData
																																							 }
			Dim obj As New LOSearchListing.ClsPrintLOSearchList(m_InitializationData)
			obj.PrintData = _Setting
			Dim result As PrintResult = obj.PrintLOSearchList(Me.bPrintAsDesign)

			If result.WOSresult Then
				Dim msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")
				m_UtilityUI.ShowInfoDialog(msg)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Sub StartExporting()
		Dim exportPath As String = m_InitializationData.UserData.spTempPayrollPath

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Daten werden zusammengestellt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim payrollData = GetSelectedPayrollItems()

		If payrollData Is Nothing OrElse payrollData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnabrechnung Daten konnten nicht geladen werden."))

			Return
		End If
		Dim result = ExportPayroll(payrollData)
		SplashScreenManager.CloseForm(False)

		If result.Printresult AndAlso result.JobResultPayrollData.Count > 0 Then

			Dim newMergedFilename As String = Path.Combine(m_InitializationData.UserData.spTempPayrollPath, String.Format("Lohnabrechnungen {0}.ex", result.JobResultPayrollData(0).EmployeeNumber))
			newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")

			If File.Exists(newMergedFilename) Then
				Try
					File.Delete(newMergedFilename)
				Catch ex As Exception
					newMergedFilename = Path.Combine(m_InitializationData.UserData.spTempInvoicePath, String.Format("{0} - {1}.ex", Path.GetRandomFileName, result.JobResultPayrollData(0).EmployeeNumber))
					newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")

				End Try
			End If





			Dim fileList As New List(Of String)
			For Each itm In result.JobResultPayrollData
				fileList.Add(itm.ExportedFileName)
			Next

			If result.JobResultPayrollData.Count > 1 Then
				Dim pdfUtility As New SP.Infrastructure.PDFUtilities.Utilities
				Dim success = pdfUtility.MergePdfFiles(fileList.ToArray, newMergedFilename)
			Else
				newMergedFilename = result.JobResultPayrollData(0).ExportedFileName
			End If

			SplashScreenManager.CloseForm(False)
			Dim msg As String = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich exportiert und zusammengestellt.")
			m_UtilityUI.ShowInfoDialog(msg)

			Process.Start("explorer.exe", "/select," & newMergedFilename)
			'Process.Start(newFilename)

		Else
			Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.") & "<br>{0}<br>{1}",
																					 exportPath, result.PrintresultMessage)
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(strMsg)
		End If

	End Sub

	Private Function ExportPayroll(ByVal payrolls As List(Of PayrollPrintData)) As PrintResult
		Dim result As New PrintResult

		Dim deleteDirectory = m_Utility.ClearAssignedFiles(m_InitializationData.UserData.spTempPayrollPath, "*.*", SearchOption.TopDirectoryOnly)

		bPrintAsDesign = False
		bPrintAsExport = False
		Me.bSendPrintJob2WOS = False
		Me.bSend_And_PrintJob2WOS = False

		Me.SQL4Print = String.Empty
		Me.PrintJobNr = "9.1"

		Dim _Setting = New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
			.SQL2Open = Me.SQL4Print,
			.JobNr2Print = Me.PrintJobNr,
			.Is4Export = Me.bPrintAsExport,
			.SendData2WOS = Me.bSendPrintJob2WOS,
			.SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS,
			.WOSSendValueEnum = m_SelectedPrintValue,
			.liLONr2Print = Me.SelectedLONr,
			.liMANr2Print = Me.SelectedMANr,
			.liLOSend2WOS = Me.SelectedData2WOS,
			.LiMALang = Me.SelectedMALang,
			.SelectedLONr2Print = Val(Me.cbo_LONr.Text),
			.SelectedMANr2Print = Val(Me.cbo_MANr.Text),
			.SelectedMDNr = m_InitializationData.MDData.MDNr,
			.LogedUSNr = m_InitializationData.UserData.UserNr,
			.PerosonalizedData = ClsDataDetail.ProsonalizedData,
			.TranslationData = ClsDataDetail.TranslationData
		}

		Dim obj As New SPS.Listing.Print.Utility.LOSearchListing.ClsPrintLOSearchList(m_InitializationData)
		obj.PrintData = _Setting
		result = obj.ExportPayrollsData()

		Return result
	End Function

	Private Function ExportSinglePayrollIntoFileSystem(ByVal loNumber As Integer) As String
		Dim result As String = String.Empty

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Eine Kopie wird erstellt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		bPrintAsDesign = False
		bPrintAsExport = True
		Me.bSendPrintJob2WOS = False
		Me.bSend_And_PrintJob2WOS = False

		Me.SQL4Print = String.Empty
		Me.PrintJobNr = "9.1"
		Dim _Setting = New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
			.SQL2Open = Me.SQL4Print,
			.JobNr2Print = Me.PrintJobNr,
			.liLONr2Print = New List(Of Integer)(New Integer() {loNumber}),
			.Is4Export = True,
			.SelectedLONr2Print = loNumber,
			.SelectedMDNr = m_InitializationData.MDData.MDNr,
			.LogedUSNr = m_InitializationData.UserData.UserNr,
			.PerosonalizedData = ClsDataDetail.ProsonalizedData,
			.TranslationData = ClsDataDetail.TranslationData
		}

		Dim obj As New SPS.Listing.Print.Utility.LOSearchListing.ClsPrintLOSearchList(m_InitializationData)
		obj.PrintData = _Setting
		result = obj.ExportLOSearchList()

		SplashScreenManager.CloseForm(False)

		If Not String.IsNullOrWhiteSpace(result) AndAlso File.Exists(result) Then

		Else
			Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.{0}{1}"),
																					 vbNewLine, result)
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(strMsg)
		End If

		Return result
	End Function

	Private Sub bbiSetting_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSetting.ItemClick
		Dim frm As New frmLOPrintSetting(m_InitializationData)

		frm.Top = (Me.Top + Me.Height) - frm.Height - 50
		frm.Left = (Me.Left + Me.Width) - frm.Width - 50

		frm.LoadData()

		frm.Show()
		frm.BringToFront()

	End Sub

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub bbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
		DeleteSelectedLOData()
	End Sub

	Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled
		SelDeSelectItems(tgsSelection.EditValue)

		If tpMainView.SelectedPage Is tnpPrint Then
			SelDeSelectItems(tgsSelection.EditValue)
		Else
			SelDeSelectEMailItems(tgsSelection.EditValue)
		End If

	End Sub

	Private Sub SelDeSelectItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of PayrollPrintData) = grdPrint.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.SelectedRec = selectItem
			Next
		End If

		gvPrint.RefreshData()

	End Sub

	Private Sub SelDeSelectEMailItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of PayrollPrintData) = grdPayrollEMail.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.SelectedRec = selectItem
			Next
		End If

		gvPayrollEMail.RefreshData()

	End Sub

	Private Function GetSelectedPayrollEMailNumbers() As List(Of Integer)

		Dim result As List(Of Integer)

		gvPayrollEMail.FocusedColumn = gvPayrollEMail.VisibleColumns(1)
		grdPayrollEMail.RefreshDataSource()
		Dim printList As BindingList(Of InvoiceData) = grdPayrollEMail.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		result = New List(Of Integer)

		For Each receiver In sentList
			result.Add(receiver.RENr)
		Next


		Return result

	End Function

	Private Function GetSelectedPayrollEMailData() As BindingList(Of InvoiceData)

		Dim result As BindingList(Of InvoiceData)

		gvPayrollEMail.FocusedColumn = gvPayrollEMail.VisibleColumns(1)
		grdPayrollEMail.RefreshDataSource()
		Dim printList As BindingList(Of InvoiceData) = grdPayrollEMail.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		result = New BindingList(Of InvoiceData)

		For Each receiver In sentList
			result.Add(receiver)
		Next


		Return result

	End Function



	Private Sub xtabLo_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabLo.SelectedPageChanged
		Dim b As DevExpress.XtraBars.ItemClickEventArgs = Nothing

		If Me.xtabLo.SelectedTabPage Is xtabLODetail Then

		Else
			Me.bbiPrint.Enabled = gvPrint.RowCount > 0
			Me.bbiDelete.Enabled = gvPrint.RowCount > 0
			Me.bbiExport.Enabled = gvPrint.RowCount > 0

		End If

	End Sub


	Private Sub cbo_LONr_TextChanged(sender As Object, e As System.EventArgs) Handles cbo_Year.TextChanged,
		cbo_Month.TextChanged, cbo_MANr.TextChanged, cbo_LONr.TextChanged, btnWOSProperty.TextChanged

		Me.ChangedSearchPrintContent = True
		Me.ChangedSearchDetailContent = True

		grdPrint.DataSource = Nothing
		grdPayrollDetail.DataSource = Nothing

	End Sub

	Private Sub CboSort_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(Me.CboSort)
	End Sub

	Private Sub LoadLODataList()
		Dim success As Boolean = True

		bsiPrintinfo.Caption = String.Empty
		bsiDetailInfo.Caption = String.Empty

		InitialSearchData()


		bbiPrint.Visibility = BarItemVisibility.Never ' Enabled = False
		bbiDelete.Visibility = BarItemVisibility.Never 'Enabled = False
		bbiSendMail.Visibility = BarItemVisibility.Never 'Enabled = False
		bbiExport.Visibility = BarItemVisibility.Never 'Enabled = False

		tnpEMailSummery.PageVisible = False

		If m_SearchType = SearchTypeEnum.PRINT Then
			tnpPrint.PageVisible = True
			tnpMail.PageVisible = False

			tpMainView.SelectedPage = tnpPrint

			success = success AndAlso LoadPayrollPrintList()


			If success Then CreatePrintPopupMenu()

			bbiPrint.Visibility = BarItemVisibility.Always ' Enabled = False
			bbiDelete.Visibility = BarItemVisibility.Always 'Enabled = False
			bbiSendMail.Visibility = BarItemVisibility.Never 'Enabled = False
			bbiExport.Visibility = BarItemVisibility.Always 'Enabled = False

			bbiPrint.Enabled = gvPrint.RowCount > 0
			bbiDelete.Enabled = gvPrint.RowCount > 0
			bbiExport.Enabled = gvPrint.RowCount > 0

		Else
			tnpPrint.PageVisible = False
			tnpMail.PageVisible = True

			tpMainView.SelectedPageIndex = 1

			success = success AndAlso LoadPayrollEMailList()
			bbiSendMail.Visibility = BarItemVisibility.Always
			bbiSendMail.Enabled = gvPayrollEMail.RowCount > 0

		End If

		success = success AndAlso LoadPayrollDetailList()
		SplashScreenManager.CloseForm(False)











		'success = LoadPayrollPrintList()
		'success = success AndAlso LoadPayrollDetailList()

		'CreatePrintPopupMenu()
		'SplashScreenManager.CloseForm(False)

		'Me.bbiPrint.Enabled = success ' AndAlso Me.xtabLo.SelectedTabPage Is Me.xtabLOPrint
		'Me.bbiExport.Enabled = success ' AndAlso Me.xtabLo.SelectedTabPage Is Me.xtabLOPrint
		'Me.bbiDelete.Enabled = success ' AndAlso Me.xtabLo.SelectedTabPage Is Me.xtabLOPrint

	End Sub

	Private Function LoadBugReport() As Boolean
		Dim result As Boolean = True
		Dim data = SelectedRecord


		If data Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
			m_UtilityUI.ShowInfoDialog(msg, "Protokoll anzeigen")

			Return False
		End If
		Dim protocoldata = m_PayrollDatabaseAccess.LoadAssignedPayrollProtocolData(m_InitializationData.MDData.MDNr, data.MANr, data.LONr, Nothing, Nothing)

		If protocoldata Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Protokoll für Lohnlauf konnte nicht geladen werden."))

			Return False
		End If

		Try
			If flyoutPanel.FlyoutPanelState.IsActive Then Return False
			'flyoutPanel.Height = xtabLo.Height
			'Dim view As GridView = CType(gvPrint, GridView)
			'flyoutPanel.OwnerControl = view.ActiveEditor ' grdPrint

			flyoutPanel.ShowBeakForm(Control.MousePosition) ' GetFocusedRowPoint)
			Dim debugValue As String = protocoldata.DebugValue
			debugValue = debugValue.Replace("|", "<br>").Replace("¦", "<br>")
			rtfContent.HtmlText = debugValue


		Catch ex As Exception
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Protokoll konnte nicht angezeigt werden."))
		End Try


		Return result
	End Function

	Private Function GetFocusedRowPoint() As Point
		Dim info As GridViewInfo = TryCast(gvPrint.GetViewInfo(), GridViewInfo)
		Dim GridRowInfo As GridRowInfo = info.GetGridRowInfo(gvPrint.FocusedRowHandle)

		If (Object.Equals(GridRowInfo, Nothing)) Then
			Return New Point()
		End If

		Return New Point(GridRowInfo.Bounds.X, GridRowInfo.Bounds.Y)
	End Function

	Private Function GetHotPoint() As Point
		Dim ctl As Control = xtabLo  ' gpSuchKriterien ' xtabLo ' gpSuchKriterien
		Dim pt As New Point(0, ctl.Height \ 2)
		Dim edtiValue As BeakPanelBeakLocation = BeakPanelBeakLocation.Left

		If edtiValue = BeakPanelBeakLocation.Right Then
			Return ctl.PointToScreen(pt)
		End If
		If edtiValue = BeakPanelBeakLocation.Left Then
			pt.X += ctl.Width
			Return ctl.PointToScreen(pt)
		End If
		pt = New Point(ctl.Width \ 2, 0)
		If edtiValue = BeakPanelBeakLocation.Top Then
			pt.Y += ctl.Height
		End If

		Return ctl.PointToScreen(pt)
	End Function

	'Private Sub UpdateControls()
	'	UpdateFlyoutHintLabelLocation()
	'End Sub

	'Private Function CreateButton() As ButtonControl
	'	Dim button = New PeekFormButton()

	'	button.ToolTip = "Custom Button"
	'	button.UseCaption = False
	'	button.ImageOptions.SvgImage = svgImageCollection1(0)
	'	button.ImageOptions.SvgImageSize = svgImageCollection1.ImageSize

	'	Return button
	'End Function

	Private Sub OnFlyoutPanelButtonClick(ByVal sender As Object, ByVal e As FlyoutPanelButtonClickEventArgs) Handles flyoutPanel.ButtonClick
		Dim _tag As String = TryCast(e.Button.Tag, String)
		If String.Equals(_tag, "Exit", StringComparison.OrdinalIgnoreCase) Then
			Me.flyoutPanel.HideBeakForm()
		End If
	End Sub

	'Private Sub UpdateFlyoutHintLabelLocation()
	'	Dim loc As Point = Me.flyoutPanelHintLabel.Location
	'	loc.Y = (Me.flyoutPanel.Height - Me.flyoutPanelHintLabel.Height) \ 2
	'	Me.flyoutPanelHintLabel.Location = loc
	'End Sub


	Private Sub DeleteSelectedLOData()
		Dim success As Boolean = True


		Try
			Dim listData = GetSelectedPayrollItems()

			Dim strMsg As String
			If listData.Count = 1 Then
				strMsg = m_Translate.GetSafeTranslationValue("Der Datensatz wird gelöscht. Möchten Sie wirklich diesen Datensatz löschen?")

			ElseIf listData.Count > 1 Then
				strMsg = String.Format(m_Translate.GetSafeTranslationValue("Die Daten werden endgültig gelöscht. Möchten Sie wirklich {0} Lohnabrechnungen löschen?"), listData.Count)

			Else
				strMsg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

				Return
			End If
			If Not m_UtilityUI.ShowYesNoDialog(strMsg, m_Translate.GetSafeTranslationValue("Datensatz löschen?"), MessageBoxDefaultButton.Button1) Then Return

			Dim addRecord As Boolean = True

			For Each itm In listData
				Dim closedMonth = m_CommonDatabaseAccess.LoadClosedMonthOfYear(itm.jahr, itm.MDNr)
				If (closedMonth Is Nothing) Then
					success = False
				End If
				success = success AndAlso Not closedMonth.Contains(itm.monat)
				If success Then
					SelectedLONr.Add(itm.LONr)
				Else
					strMsg = m_Translate.GetSafeTranslationValue("Der ausgewählte Monat ist bereits abgeschlossen.")
					m_UtilityUI.ShowInfoDialog(strMsg)

				End If
			Next


			'SelectedLONr = SelectedLONr.GroupBy(Function(m) m).Where(Function(g) g.Count() > 1).Select(Function(g) g.Key).ToList()
			If SelectedLONr.Count > 0 Then success = success AndAlso StartDeleteingSelected()
			If success Then
				strMsg = "Die ausgewählten Lohnabrechnungen wurden erfolgreich gelöscht."
				m_UtilityUI.ShowOKDialog(Me, m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Lohnabrechnung löschen"), MessageBoxIcon.Information)

			Else
				strMsg = "Die Daten konnten nicht gelöscht werden."
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg))
				m_UtilityUI.ShowOKDialog(Me, m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Lohnabrechnung löschen"), MessageBoxIcon.Error)

			End If
			LoadLODataList()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Function StartDeleteingSelected() As Boolean
		Dim success As Boolean = True

		Try
			Dim obj As New SPS.MA.Lohn.Utility.DeletePayroll(m_InitializationData)
			Dim deleteData As New List(Of SPS.MA.Lohn.Utility.DeleteData)

			Dim selectedData = SelectedLONr.GroupBy(Function(m) m).Where(Function(g) g.Count() > 1).Select(Function(g) g.Key).ToList()

			For Each loNumber As Integer In selectedData
				Dim payrollData As New SPS.MA.Lohn.Utility.DeleteData
				payrollData.PayrollNumber = loNumber

				'#If Not DEBUG Then

				payrollData.ExportedFilename = ExportSinglePayrollIntoFileSystem(loNumber)
				If String.IsNullOrWhiteSpace(payrollData.ExportedFilename) OrElse Not File.Exists(payrollData.ExportedFilename) Then
					Dim strMsg As String = "Eine Kopie der Abrechnung konnte nicht angelegt werden. Ihre Daten werden nicht gelöscht."
					SplashScreenManager.CloseForm(False)
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg))

					success = False
				End If
				'#End If

				If success Then deleteData.Add(payrollData)

			Next
			SplashScreenManager.CloseForm(False)

			obj.CurrentDelteData = deleteData
			success = success AndAlso obj.DeleteSelectedLO(False)


		Catch ex As Exception
			m_Logger.LogInfo(String.Format("{0}", ex.ToString))
			success = False

		End Try

		Return success

	End Function

	Sub OpenSelectedEmployee(ByVal employeeNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub



	Private Sub OncbPrintWOS_Click(sender As Object, e As EventArgs) Handles cbPrintWOS.Click
		If m_SuppressUIEvents Then Return

		If cbPrintWOS.Checked AndAlso m_SearchType = SearchTypeEnum.PRINT Then Return
		m_SuppressUIEvents = True

		m_SearchType = SearchTypeEnum.PRINT

		cbPrintWOS.Checked = True
		cbEMail.Checked = False

		SetGridVisibility()

		m_SuppressUIEvents = False

	End Sub

	Private Sub cbEMail_Click(sender As Object, e As EventArgs) Handles cbEMail.Click
		If m_SuppressUIEvents Then Return

		If cbEMail.Checked AndAlso m_SearchType = SearchTypeEnum.EMAIL Then Return
		m_SuppressUIEvents = True

		m_SearchType = SearchTypeEnum.EMAIL

		cbPrintWOS.Checked = False
		cbEMail.Checked = True

		SetGridVisibility()

		m_SuppressUIEvents = False

	End Sub

	Private Sub SetGridVisibility()

		tnpMailSetting.PageVisible = False
		tnpWOSSetting.PageVisible = False

		tnpPrint.PageVisible = m_SearchType = SearchTypeEnum.PRINT
		tnpMail.PageVisible = m_SearchType = SearchTypeEnum.EMAIL

		If m_SearchType = SearchTypeEnum.PRINT Then
			tnpWOSSetting.PageVisible = True
			tpMainView.SelectedPage = tnpPrint
			tpMainSetting.SelectedPage = tnpWOSSetting

		Else
			tnpMailSetting.PageVisible = False
			tpMainView.SelectedPage = tnpMail
			tpMainSetting.SelectedPage = Nothing

		End If

		bbiPrint.Enabled = gvPrint.RowCount > 0
		bbiDelete.Enabled = gvPrint.RowCount > 0
		bbiSendMail.Enabled = gvPayrollEMail.RowCount > 0
		bbiExport.Enabled = gvPrint.RowCount > 0

		bbiPrint.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Always, BarItemVisibility.Never)
		bbiDelete.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Always, BarItemVisibility.Never)
		bbiSendMail.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Never, BarItemVisibility.Always)
		bbiExport.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Always, BarItemVisibility.Never)

	End Sub






#Region "Helper Class"

	Private Class MenuSettingData
		Public Property Value As Integer
		Public Property Label As String
	End Class

	Private Sub grdPrint_Click(sender As Object, e As EventArgs) Handles grdPrint.Click

	End Sub


#End Region

End Class

