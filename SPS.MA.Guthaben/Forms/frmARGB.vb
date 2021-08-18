
Imports System.IO

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects


Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.XtraBars
Imports DevExpress.Pdf

Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Base
Imports SPS.Listing.Print.Utility.EmployeePrint

Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Imports DevExpress.XtraEditors
Imports SPSSendMail.RichEditSendMail
Imports DevExpress.XtraEditors.DXErrorProvider
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure

Public Class frmARGB
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ESDatabaseAccess As IESDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_ReportDatabaseAccess As IReportDatabaseAccess

	''' <summary>
	''' The invoice data access object.
	''' </summary>
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility
	Private m_DateUtility As DateAndTimeUtily

	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	Private m_CustomerWOSID As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	Private m_SonstigesSetting As String
	Private m_SUVASetting As String
	Private m_AHVSetting As String

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String

	Private m_Years As List(Of IntegerValueViewWrapper)
	Private m_Month As List(Of IntegerValueViewWrapper)
	Private m_EmployeeNumber As Integer?

	Private m_CountWorked6Month As Integer?
	Private m_CountWorked12Month As Integer?
	Private m_CountWorked15Month As Integer?
	Private m_CountWorked24Month As Integer?

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private m_EmployeeAddressData As EmployeeSAddressData
	Private m_EmployeeData As EmployeeMasterData
	Private m_EmployeeContactCommData As EmployeeContactComm
	Private m_ESData As IEnumerable(Of ZVESData)
	Private m_Payrolldata As IEnumerable(Of ARGBPayrollData)
	Private m_PayrollLastMonthAHVdata As IEnumerable(Of ARGBAHVPayrollData)

	Private m_SearchCriteriums As EmployeeARGBSearchData

	Private m_SelectedWOSEnun As WOSZVSENDValue

	Private m_PDFFilesToPrint As List(Of String)
	Private m_ExportPrintInFiles As Boolean
	Private m_PDFUtility As PDFUtilities.Utilities

	Private m_NotifyUser As Boolean
	Private m_JustExportDataIntoFile As Boolean


#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"

#End Region


#Region "private properties"


	''' <summary>
	''' Gets the ahv ausgleichskasse.
	''' </summary>
	Private ReadOnly Property CompensationfundSetting As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVAddressZusatz", m_AHVSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the ahv ausgleichskasse number.
	''' </summary>
	Private ReadOnly Property CompensationfundNumberSetting As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AusgNummer", m_AHVSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the bvg versicherung.
	''' </summary>
	Private ReadOnly Property BVGfundSetting As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/bvgname", m_SonstigesSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the bvg versicherung.
	''' </summary>
	Private ReadOnly Property UnfallfundSetting As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Unfallversicherung", m_SonstigesSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the bvg versicherung.
	''' </summary>
	Private ReadOnly Property UnfallfundNumberSetting As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Abrechnungsnummer", m_SUVASetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the BUR number.
	''' </summary>
	Private ReadOnly Property BURNumberSetting As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/BURNumber", m_SonstigesSetting))

			Return settingValue
		End Get
	End Property


#End Region


#Region "public property"
	''' <summary>
	''' Gets or sets the preselection data.
	''' </summary>
	Public Property PreselectionData As PreselectionARGBData

	Public ReadOnly Property ARGBExportFilename As String
		Get
			If m_PDFFilesToPrint Is Nothing OrElse m_PDFFilesToPrint.Count = 0 Then Return String.Empty
			Return m_PDFFilesToPrint(0)
		End Get
	End Property

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_DateUtility = New DateAndTimeUtily
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_PDFUtility = New PDFUtilities.Utilities
		m_PDFFilesToPrint = New List(Of String)

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ReportDatabaseAccess = New ReportDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
		m_SUVASetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING, m_InitializationData.MDData.MDNr)
		m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		m_NotifyUser = True

		TranslateControls()
		Reset()

		LoadMandantDropDownData()

		AddHandler DateFrom.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler DateTo.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de9_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de9_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de12_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de12_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de14.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de15.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de24_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de24_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de25_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de25_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de26_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de26_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de27_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de27_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de28_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler de28_2.ButtonClick, AddressOf OnDropDown_ButtonClick


	End Sub

#End Region


#Region "public methodes"

	''' <summary>
	''' Preselects data.
	''' </summary>
	Public Sub PreselectData()

		Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

		If hasPreselectionData Then

			Dim supressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

			' ---Mandant---
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of SP.DatabaseAccess.Common.DataObjects.MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = PreselectionData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					m_SuppressUIEvents = supressUIEventState
					Return
				End If

			End If

			m_SuppressUIEvents = supressUIEventState
		Else
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of SP.DatabaseAccess.Common.DataObjects.MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = m_InitializationData.MDData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					Return
				End If

			End If

		End If

		If Not PreselectionData.EmployeeNumber Is Nothing Then
			m_EmployeeNumber = PreselectionData.EmployeeNumber
		End If
		If Not PreselectionData.Year Is Nothing Then
			DateFrom.EditValue = CDate(String.Format("01.01.{0}", PreselectionData.Year))
			DateTo.EditValue = CDate(String.Format("31.12.{0}", PreselectionData.Year))
		End If


		bbiPrint.Enabled = False
		bbiClear.Enabled = False
		bbiSendEMail.Enabled = False

	End Sub

	''' <summary>
	''' Loads data.
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		m_SuppressUIEvents = True


		PreselectData()

		m_SearchCriteriums = New EmployeeARGBSearchData With {.MDNr = lueMandant.EditValue, .EmployeeNumbers = New List(Of Integer)({m_EmployeeNumber})}


		m_SuppressUIEvents = False

		Return success

	End Function

	Public Function CreateARGBDocument() As Boolean
		Dim success As Boolean = True

		m_SuppressUIEvents = True

		m_NotifyUser = False
		m_JustExportDataIntoFile = True
		m_PDFFilesToPrint = New List(Of String)

		PreselectData()

		m_SearchCriteriums = New EmployeeARGBSearchData With {.MDNr = PreselectionData.MDNr, .EmployeeNumbers = New List(Of Integer)({m_EmployeeNumber}),
			.DateFrom = CDate(String.Format("01.01.{0}", PreselectionData.Year)), .DateTo = CDate(String.Format("31.12.{0}", PreselectionData.Year))}
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Year(DateFrom.EditValue)))

		ResetAllTabs()
		DisplayEmployeeData()

		success = success AndAlso LoadESOverviewData()
		DisplayESData()

		DisplayPayrollData()
		DisplayPayrollAHVLastFourYearsData()

		m_ExportPrintInFiles = True
		If success Then StartExporting()


		m_SuppressUIEvents = False


		Return success
	End Function


	Public Sub DisplayEmployeeData()
		Dim result As Boolean = True

		If m_EmployeeNumber Is Nothing Then Return
		result = result AndAlso LoadEmployeeAddressData()
		result = result AndAlso LoadEmployeeData()
		result = result AndAlso LoadEmployeeContactCommData()

		If Not result Then Return

		Me.Text = String.Format(m_Translate.GetSafeTranslationValue("Arbeitgeberbescheinigung für: [{0}]"), m_EmployeeAddressData.EmployeeFullname)
		grpadresse.Text = String.Format(m_Translate.GetSafeTranslationValue("Kandidat: {0}"), m_EmployeeNumber)

		lblEmployeeNameValue.Text = m_EmployeeAddressData.EmployeeFullname
		lblEmployeeCoValue.Text = m_EmployeeAddressData.StaysAt
		lblEmployeePostOfficeBoxValue.Text = m_EmployeeAddressData.PostOfficeBox
		lblEmployeeStreetValue.Text = m_EmployeeAddressData.Street
		lblEmployeeAddressValue.Text = m_EmployeeAddressData.EmployeeAddress

		lblEmployeeCivilStatusValue.Text = m_EmployeeData.CivilStatus
		lblBirthdateValue.Text = m_EmployeeData.Birthdate
		If m_EmployeeData.Birthdate Is Nothing Then
			lblAge.Text = String.Empty
		Else
			lblAge.Text = String.Format("({0})", GetAge(m_EmployeeData.Birthdate))
		End If
		lblEmployeeAHVNrValue.Text = m_EmployeeData.AHV_Nr_New

		txtZVEmail.Text = m_EmployeeContactCommData.ZVeMail
		lblALKNumberValue.Text = m_EmployeeContactCommData.ALKNumber.GetValueOrDefault(0)
		lblALKNameValue.Text = m_EmployeeContactCommData.ALKName
		lblALKPOBoxValue.Text = m_EmployeeContactCommData.ALKPOBox
		lblALKStreetValue.Text = m_EmployeeContactCommData.ALKStreet
		lblALKPostcodeValue.Text = m_EmployeeContactCommData.ALKPostcode.GetValueOrDefault(0)
		lblALKTelephoneValue.Text = m_EmployeeContactCommData.ALKTelephone
		lblALKTelefaxValue.Text = m_EmployeeContactCommData.ALKTelefax

		txtCompensationfund.EditValue = String.Format("{0}, {1}", CompensationfundSetting, CompensationfundNumberSetting)

	End Sub


#End Region

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	Private Sub Onfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			If My.Settings.iARGBHeight > 0 Then Me.Height = My.Settings.iARGBHeight
			If My.Settings.iARGBWidth > 0 Then Me.Width = My.Settings.iARGBWidth
			If My.Settings.frmARGBLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmARGBLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

			chkPrintESVertrag.Checked = My.Settings.chkPrintESVertragWithARGB
			chkPrintPayroll.Checked = My.Settings.chkPrintPayrollWithARGB
			chkOpenpdffile.Checked = My.Settings.chkOpenpdffile

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
		End Try

	End Sub

	Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		SplashScreenManager.CloseForm(False)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iARGBHeight = Me.Height
			My.Settings.iARGBWidth = Me.Width
			My.Settings.frmARGBLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.chkPrintESVertragWithARGB = chkPrintESVertrag.Checked
			My.Settings.chkPrintPayrollWithARGB = chkPrintPayroll.Checked
			My.Settings.chkOpenpdffile = chkOpenpdffile.Checked

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	'''  Translate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue("Arbeitgeberbescheinigung für: [{0}]")
		grpadresse.Text = m_Translate.GetSafeTranslationValue("Kandidat: {0}")
		grpData.Text = m_Translate.GetSafeTranslationValue(grpData.Text)
		grpALK.Text = m_Translate.GetSafeTranslationValue(grpALK.Text)

		chkPrintESVertrag.Text = m_Translate.GetSafeTranslationValue(chkPrintESVertrag.Text)
		chkPrintPayroll.Text = m_Translate.GetSafeTranslationValue(chkPrintPayroll.Text)
		chkOpenpdffile.Text = m_Translate.GetSafeTranslationValue(chkOpenpdffile.Text)

		lblNachname.Text = m_Translate.GetSafeTranslationValue(Me.lblNachname.Text)
		lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		lblZwischen.Text = m_Translate.GetSafeTranslationValue(Me.lblZwischen.Text)

		lblCOAdresse.Text = m_Translate.GetSafeTranslationValue(Me.lblCOAdresse.Text)
		lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
		lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)

		lblAdresse.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresse.Text)
		lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGeburtsdatum.Text)
		lblZivilstand.Text = m_Translate.GetSafeTranslationValue(Me.lblZivilstand.Text)
		lblAHVNrNeu.Text = m_Translate.GetSafeTranslationValue(Me.lblAHVNrNeu.Text)
		lblEmailForZv.Text = m_Translate.GetSafeTranslationValue(Me.lblEmailForZv.Text)

		xtabESRP.Text = m_Translate.GetSafeTranslationValue(xtabESRP.Text)
		xtabAbsenzen.Text = m_Translate.GetSafeTranslationValue(xtabAbsenzen.Text)
		xtabSonstigeZahlungen.Text = m_Translate.GetSafeTranslationValue(xtabSonstigeZahlungen.Text)
		xtabAufloesung.Text = m_Translate.GetSafeTranslationValue(xtabAufloesung.Text)
		xtabVerdienst.Text = m_Translate.GetSafeTranslationValue(xtabVerdienst.Text)
		xtabAddress.Text = m_Translate.GetSafeTranslationValue(xtabAddress.Text)

		lbl1.Text = m_Translate.GetSafeTranslationValue(lbl1.Text)
		lbl8.Text = m_Translate.GetSafeTranslationValue(lbl8.Text)

		lbl14.Text = m_Translate.GetSafeTranslationValue(lbl14.Text)
		lbl1.Text = m_Translate.GetSafeTranslationValue(lbl1.Text)

		lbl24.Text = m_Translate.GetSafeTranslationValue(lbl24.Text)
		lbl25.Text = m_Translate.GetSafeTranslationValue(lbl25.Text)
		lbl26.Text = m_Translate.GetSafeTranslationValue(lbl26.Text)
		lbl27.Text = m_Translate.GetSafeTranslationValue(lbl27.Text)
		lbl28.Text = m_Translate.GetSafeTranslationValue(lbl28.Text)

		lbl19.Text = m_Translate.GetSafeTranslationValue(lbl19.Text)
		lbl19_1.Text = m_Translate.GetSafeTranslationValue(lbl19_1.Text)
		lbl20.Text = m_Translate.GetSafeTranslationValue(lbl20.Text)
		lbl20_1.Text = m_Translate.GetSafeTranslationValue(lbl20_1.Text)
		lbl20_2.Text = m_Translate.GetSafeTranslationValue(lbl20_2.Text)
		lbl22.Text = m_Translate.GetSafeTranslationValue(lbl22.Text)
		lbl22_1.Text = m_Translate.GetSafeTranslationValue(lbl22_1.Text)
		lbl23.Text = m_Translate.GetSafeTranslationValue(lbl23.Text)
		lbl23_1.Text = m_Translate.GetSafeTranslationValue(lbl23_1.Text)

		lbl9.Text = m_Translate.GetSafeTranslationValue(lbl9.Text)
		lbl9_1.Text = m_Translate.GetSafeTranslationValue(lbl9_1.Text)
		lbl9_2.Text = m_Translate.GetSafeTranslationValue(lbl9_2.Text)
		lbl10.Text = m_Translate.GetSafeTranslationValue(lbl10.Text)
		lbl11.Text = m_Translate.GetSafeTranslationValue(lbl11.Text)
		lbl12.Text = m_Translate.GetSafeTranslationValue(lbl12.Text)
		lbl12_1.Text = m_Translate.GetSafeTranslationValue(lbl12_1.Text)
		lbl12_2.Text = m_Translate.GetSafeTranslationValue(lbl12_2.Text)

		lbl13.Text = m_Translate.GetSafeTranslationValue(lbl13.Text)
		lbl14.Text = m_Translate.GetSafeTranslationValue(lbl14.Text)
		lbl15.Text = m_Translate.GetSafeTranslationValue(lbl15.Text)

		lbl16.Text = m_Translate.GetSafeTranslationValue(lbl16.Text)
		lbl16_1.Text = m_Translate.GetSafeTranslationValue(lbl16_1.Text)
		lbl16_3.Text = m_Translate.GetSafeTranslationValue(lbl16_3.Text)
		lbl16_5.Text = m_Translate.GetSafeTranslationValue(lbl16_5.Text)
		lbl16_7.Text = m_Translate.GetSafeTranslationValue(lbl16_7.Text)

		lbl17.Text = m_Translate.GetSafeTranslationValue(lbl17.Text)

		lblALKNummer.Text = m_Translate.GetSafeTranslationValue(lblALKNummer.Text)
		lblALKKasse.Text = m_Translate.GetSafeTranslationValue(lblALKKasse.Text)
		lblALKPostfach.Text = m_Translate.GetSafeTranslationValue(lblALKPostfach.Text)
		lblALKStrasse.Text = m_Translate.GetSafeTranslationValue(lblALKStrasse.Text)
		lblALKPLZ.Text = m_Translate.GetSafeTranslationValue(lblALKPLZ.Text)
		lblALKTelefon.Text = m_Translate.GetSafeTranslationValue(lblALKTelefon.Text)
		lblALKTelefax.Text = m_Translate.GetSafeTranslationValue(lblALKTelefax.Text)

		bbiSearch.Caption = m_Translate.GetSafeTranslationValue(bbiSearch.Caption)
		bbiClear.Caption = m_Translate.GetSafeTranslationValue(bbiClear.Caption)
		bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)
		bbiSendEMail.Caption = m_Translate.GetSafeTranslationValue(bbiSendEMail.Caption)

	End Sub


#Region "Reset"

	Private Sub Reset()
		Dim previousState = SetSuppressUIEventsState(True)

		DxErrorProvider1.ClearErrors()

		Dim connectionString As String = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_Years = Nothing
		m_Month = Nothing

		DateFrom.EditValue = Nothing
		DateFrom.Properties.NullText = String.Empty
		DateTo.EditValue = Nothing
		'DateTo.Properties.NullText = String.Empty

		ResetAllTabs()

		ResetMandantDropDown()

		'  Reset grids, drop downs and lists, etc.
		'ResetPayrollGrid()
		ResetESGrid()

		SetSuppressUIEventsState(previousState)

		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetAllTabs()

		Dim supressUIEventState = m_SuppressUIEvents
		m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.
		bbiClear.Enabled = False
		bbiPrint.Enabled = False
		bbiSendEMail.Enabled = False

		m_PDFFilesToPrint.Clear()

		ResetTab1()
		ResetTab2()
		ResetTab3()
		ResetTab4()
		ResetTab5()


		m_SuppressUIEvents = supressUIEventState ' Make sure UI event are fired so that the lookup data is loaded correctly.

	End Sub

	Private Sub ResetTab1()

		ResetESGrid()
		txtCompensationfund.EditValue = Nothing

	End Sub

	Private Sub ResetTab2()

		txt9_1.EditValue = Nothing
		de9_1.EditValue = Nothing
		de9_2.EditValue = Nothing
		de9_2.Properties.TextEditStyle = TextEditStyles.Standard

		op10.Properties.Items.Clear()
		Dim itemValues As Object() = New Object() {0, 1}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("mündlich"), m_Translate.GetSafeTranslationValue("schriftlich")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			op10.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op10.EditValue = Nothing

		txt11.EditValue = Nothing

		op12.Properties.Items.Clear()
		itemValues = New Object() {0, 1, 2}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja, voll"), m_Translate.GetSafeTranslationValue("ja, teilweise"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op12.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op12.EditValue = Nothing
		txt12.EditValue = Nothing
		de12_1.EditValue = Nothing
		de12_2.EditValue = Nothing

		txt13.EditValue = Nothing
		de14.EditValue = Nothing
		de15.EditValue = Nothing

	End Sub

	Private Sub ResetTab3()

		txt16_1.EditValue = 0D
		txt16_2.EditValue = 0D
		txt16_3.EditValue = 0D
		txt16_4.EditValue = 0D

		txt17.EditValue = 0D

		txt18_1.EditValue = 0D
		txt18_2.EditValue = 0D

	End Sub

	Private Sub ResetTab4()

		op19_1.Properties.Items.Clear()
		Dim itemValues As Object() = New Object() {0, 1}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("ja, in der Höhe von"), m_Translate.GetSafeTranslationValue("nein")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			op19_1.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op19_1.EditValue = Nothing
		op19_2.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja, der Betrag im Ziff. 16/17"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op19_2.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op19_2.EditValue = Nothing
		txt19.EditValue = 0D

		op20_1.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja, in der Höhe von"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op20_1.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op20_1.EditValue = Nothing
		op20_2.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja, der Betrag im Ziff. 16/17"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op20_2.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op20_2.EditValue = Nothing
		txt20_1.EditValue = 0D
		txt20_2.EditValue = Nothing

		op21_1.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja, in der Höhe von"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op21_1.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op21_1.EditValue = Nothing
		op21_2.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja, der Betrag im Ziff. 16/17"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op21_2.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op21_2.EditValue = Nothing
		txt21.EditValue = 0D

		op22.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op22.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op22.EditValue = Nothing
		txt22.EditValue = Nothing

		op23.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("ja"), m_Translate.GetSafeTranslationValue("nein")}
		i = 0
		Do While i < itemValues.Length
			op23.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op23.EditValue = Nothing
		txt23.EditValue = Nothing

	End Sub

	Private Sub ResetTab5()

		de24_1.EditValue = Nothing
		de24_2.EditValue = Nothing

		de25_1.EditValue = Nothing
		de25_2.EditValue = Nothing

		de26_1.EditValue = Nothing
		de26_2.EditValue = Nothing

		de27_1.EditValue = Nothing
		de27_2.EditValue = Nothing

		de28_1.EditValue = Nothing
		de28_2.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the Mandant drop down.
	''' </summary>
	Private Sub ResetMandantDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowFooter = False
		lueMandant.Properties.DropDownRows = 10
		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the es overview grid.
	''' </summary>
	Private Sub ResetESGrid()

		' Reset the grid
		gvES.OptionsView.ShowIndicator = False
		gvES.OptionsView.ColumnAutoWidth = True
		gvES.OptionsView.ShowAutoFilterRow = True

		gvES.Columns.Clear()

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnESNr.Name = "ESNR"
		columnESNr.FieldName = "ESNR"
		columnESNr.Visible = False
		gvES.Columns.Add(columnESNr)

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = False
		gvES.Columns.Add(columnCustomerNumber)

		Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerName.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCustomerName.Name = "CustomerName"
		columnCustomerName.FieldName = "CustomerName"
		columnCustomerName.Visible = True
		columnCustomerName.Width = 150
		gvES.Columns.Add(columnCustomerName)

		Dim columnESFromTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESFromTo.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnESFromTo.Name = "ESFromTo"
		columnESFromTo.FieldName = "ESFromTo"
		columnESFromTo.Visible = True
		columnESFromTo.Width = 150
		gvES.Columns.Add(columnESFromTo)

		Dim columnGrundLohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGrundLohn.Caption = m_Translate.GetSafeTranslationValue("GrundLohn")
		columnGrundLohn.Name = "GrundLohn"
		columnGrundLohn.FieldName = "GrundLohn"
		columnGrundLohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnGrundLohn.DisplayFormat.FormatString = "F2"
		columnGrundLohn.Visible = True
		gvES.Columns.Add(columnGrundLohn)

		Dim columnFerienProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFerienProz.Caption = m_Translate.GetSafeTranslationValue("Ferien %")
		columnFerienProz.Name = "FerienProz"
		columnFerienProz.FieldName = "FerienProz"
		columnFerienProz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnFerienProz.DisplayFormat.FormatString = "F2"
		columnFerienProz.Visible = True
		columnFerienProz.Width = 80
		gvES.Columns.Add(columnFerienProz)

		Dim columnFeierProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFeierProz.Caption = m_Translate.GetSafeTranslationValue("Feier %")
		columnFeierProz.Name = "FeierProz"
		columnFeierProz.FieldName = "FeierProz"
		columnFeierProz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnFeierProz.DisplayFormat.FormatString = "F2"
		columnFeierProz.Visible = True
		columnFeierProz.Width = 80
		gvES.Columns.Add(columnFeierProz)

		Dim columnLohn13Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLohn13Proz.Caption = m_Translate.GetSafeTranslationValue("13. %")
		columnLohn13Proz.Name = "Lohn13Proz"
		columnLohn13Proz.FieldName = "Lohn13Proz"
		columnLohn13Proz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnLohn13Proz.DisplayFormat.FormatString = "F2"
		columnLohn13Proz.Visible = True
		columnLohn13Proz.Width = 80
		gvES.Columns.Add(columnLohn13Proz)

		Dim columnGAVData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVData.Caption = m_Translate.GetSafeTranslationValue("GAVData")
		columnGAVData.Name = "GAVData"
		columnGAVData.FieldName = "GAVData"
		columnGAVData.Visible = True
		columnGAVData.Width = 200
		gvES.Columns.Add(columnGAVData)


		grdES.DataSource = Nothing

	End Sub



#End Region



	''' <summary>
	''' Loads the mandant drop down data.
	''' </summary>
	Private Function LoadMandantDropDownData() As Boolean
		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.EditValue = Nothing
		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()

		Return mandantData IsNot Nothing
	End Function

	''' <summary>
	''' Load employee zv address data.
	''' </summary>
	Private Function LoadEmployeeAddressData() As Boolean

		m_EmployeeAddressData = m_EmployeeDatabaseAccess.LoadEmployeeARGBAddressData(m_EmployeeNumber)

		If (m_EmployeeAddressData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Adressdaten konnten nicht geladen werden."))
		End If


		Return Not m_EmployeeAddressData Is Nothing
	End Function

	''' <summary>
	''' Load employee data.
	''' </summary>
	Private Function LoadEmployeeData() As Boolean

		m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, True)

		If (m_EmployeeData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Daten konnten nicht geladen werden."))
		End If


		Return Not m_EmployeeData Is Nothing
	End Function

	''' <summary>
	''' Load MAKontakt_Komm data.
	''' </summary>
	Private Function LoadEmployeeContactCommData() As Boolean

		m_EmployeeContactCommData = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_EmployeeNumber)

		If (m_EmployeeData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Daten konnten nicht geladen werden."))
		End If


		Return Not m_EmployeeContactCommData Is Nothing
	End Function


	''' <summary>
	''' Loads es overview data.
	''' </summary>
	Private Function LoadESOverviewData() As Boolean

		m_ESData = m_EmployeeDatabaseAccess.LoadESData2ForZVForm(lueMandant.EditValue, m_EmployeeNumber, DateFrom.EditValue, DateTo.EditValue)

		If m_ESData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Daten konnten nicht geladen werden."))
		End If


		grdES.DataSource = m_ESData

		Return Not m_ESData Is Nothing

	End Function

	Private Sub DisplayESData()
		Dim unDefined As Boolean?
		Dim esEnd As Date?
		Dim esBegin As Date?

		For Each rec In m_ESData
			If esBegin Is Nothing Then esBegin = rec.ES_Ab
			If rec.ES_Ende Is Nothing Then
				If unDefined Is Nothing Then unDefined = True
			Else
				If Not unDefined.GetValueOrDefault(False) Then esEnd = rec.ES_Ende
			End If
		Next
		If Not esEnd.HasValue OrElse esEnd > DateTo.EditValue Then esEnd = DateTo.EditValue

		Dim dismissalData = m_ESData.Where(Function(s) s.dismissalon IsNot Nothing)

		If dismissalData Is Nothing OrElse dismissalData.Count = 0 Then Return
		dismissalData = dismissalData.OrderBy(Function(s) s.ES_Ende)

		txt9_1.EditValue = String.Format("{0}", dismissalData(dismissalData.Count - 1).dismissalwho)
		de9_1.EditValue = dismissalData(dismissalData.Count - 1).dismissalon ' String.Format("{0:d}", dismissalData(dismissalData.Count - 1).dismissalon)
		de9_2.EditValue = dismissalData(dismissalData.Count - 1).dismissalfor ' String.Format("{0:d}", dismissalData(dismissalData.Count - 1).dismissalfor)
		If dismissalData(dismissalData.Count - 1).dismissalkind = "mündlich" Then
			op10.EditValue = 0
		Else
			op10.EditValue = 1
		End If
		txt13.EditValue = String.Format("{0}", dismissalData(dismissalData.Count - 1).dismissalreason)

		de14.EditValue = CType(esEnd, Date) ' String.Format("{0:d}", esEnd)
		de15.EditValue = CType(esEnd, Date) ' String.Format("{0:d}", esEnd)

		op12.EditValue = 2

	End Sub

	Private Sub DisplayPayrollData()
		Dim amount_800 As Decimal = 0
		Dim amount_660 As Decimal = 0
		Dim amount_7110 As Decimal = 0
		Dim amount_18_1 As Decimal = 0
		Dim amount_18_2 As Decimal = 0
		Dim amount_660_Bruttopflichtig As Boolean?
		Dim amount_800_Bruttopflichtig As Boolean?

		Dim foundedData = LoadEmployeePayrollData(m_SearchCriteriums)
		If foundedData Is Nothing Then Return

		Dim ahvAmountData = m_Payrolldata.Where(Function(s) s.LANr = 7110).ToList
		If ahvAmountData.Count > 0 Then
			amount_7110 = ahvAmountData(0).KumulativAmount
		End If

		Dim FerienAmountData = m_Payrolldata.Where(Function(s) s.LANr = 660).ToList
		If FerienAmountData.Count > 0 Then
			amount_660 = FerienAmountData(0).KumulativAmount
			amount_660_Bruttopflichtig = FerienAmountData(0).Bruttopflichtig.GetValueOrDefault(False)
		End If

		Dim GleitTimeAmountData = m_Payrolldata.Where(Function(s) s.LANr = 800).ToList
		If GleitTimeAmountData.Count > 0 Then
			amount_800 = GleitTimeAmountData(0).KumulativAmount
			amount_800_Bruttopflichtig = GleitTimeAmountData(0).Bruttopflichtig.GetValueOrDefault(False)
		End If

		Dim UnterkunftAmountData = m_Payrolldata.Where(Function(s) s.ARGB_Verdienst_Unterkunft = 1 And s.Bruttopflichtig = 1 And s.AHVpflichtig = 0).ToList
		If UnterkunftAmountData.Count > 0 Then
			amount_18_1 = UnterkunftAmountData(0).KumulativAmount
		End If
		Dim mahlzeitAmountData = m_Payrolldata.Where(Function(s) s.ARGB_Verdienst_Mahlzeit = 1 And s.Bruttopflichtig = 1 And s.AHVpflichtig = 0).ToList
		If mahlzeitAmountData.Count > 0 Then
			amount_18_2 = mahlzeitAmountData(0).KumulativAmount
		End If

		txt17.EditValue = String.Format("{0:n2}", amount_7110)
		txt18_1.EditValue = String.Format("{0:n2}", amount_18_1)
		txt18_2.EditValue = String.Format("{0:n2}", amount_18_2)

		op19_1.EditValue = 1
		op19_2.EditValue = Nothing

		op20_1.EditValue = If(amount_660 <> 0, 0, 1)
		txt20_1.EditValue = String.Format("{0:n2}", amount_660)
		op20_2.EditValue = If(amount_660_Bruttopflichtig.GetValueOrDefault(False), 0, 1)

		op21_1.EditValue = If(amount_800 <> 0, 0, 1)
		txt21.EditValue = String.Format("{0:n2}", amount_800)
		op21_2.EditValue = If(amount_800_Bruttopflichtig.GetValueOrDefault(False), 0, 1)

		op22.EditValue = 1
		op23.EditValue = 1

	End Sub

	Private Sub DisplayPayrollAHVLastFourYearsData()
		Dim last6MonthCount As Decimal?
		Dim last12MonthCount As Decimal?
		Dim last15MonthCount As Decimal?
		Dim last24MonthCount As Decimal?

		Dim foundedData = LoadEmployeePayrollLastMonthsAHVData(m_SearchCriteriums)
		If foundedData Is Nothing Then Return

		m_CountWorked6Month = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 6 Select d.AHVAmount).Count
		If m_CountWorked6Month.GetValueOrDefault(0) > 0 Then last6MonthCount = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 6 Select d.AHVAmount).Average

		m_CountWorked12Month = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 12 Select d.AHVAmount).Count
		If m_CountWorked12Month.GetValueOrDefault(0) > 0 Then last12MonthCount = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 12 Select d.AHVAmount).Average

		m_CountWorked15Month = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 15 Select d.AHVAmount).Count
		If m_CountWorked15Month.GetValueOrDefault(0) > 0 Then last15MonthCount = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 15 Select d.AHVAmount).Average

		m_CountWorked24Month = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 24 Select d.AHVAmount).Count
		If m_CountWorked24Month.GetValueOrDefault(0) > 0 Then last24MonthCount = (From d In m_PayrollLastMonthAHVdata Where d.MonthBefore = 24 Select d.AHVAmount).Average

		txt16_1.EditValue = String.Format("{0:n2}", last6MonthCount.GetValueOrDefault(0))
		txt16_2.EditValue = String.Format("{0:n2}", last12MonthCount.GetValueOrDefault(0))
		txt16_3.EditValue = String.Format("{0:n2}", last15MonthCount.GetValueOrDefault(0))
		txt16_4.EditValue = String.Format("{0:n2}", last24MonthCount.GetValueOrDefault(0))

		lbl16_1.Visible = last6MonthCount.GetValueOrDefault(0) <> 0
		txt16_1.Visible = last6MonthCount.GetValueOrDefault(0) <> 0
		lbl16_2.Visible = last6MonthCount.GetValueOrDefault(0) <> 0

		lbl16_3.Visible = last12MonthCount.GetValueOrDefault(0) <> 0
		txt16_2.Visible = last12MonthCount.GetValueOrDefault(0) <> 0
		lbl16_4.Visible = last12MonthCount.GetValueOrDefault(0) <> 0

		lbl16_5.Visible = last15MonthCount.GetValueOrDefault(0) <> 0
		txt16_3.Visible = last15MonthCount.GetValueOrDefault(0) <> 0
		lbl16_6.Visible = last15MonthCount.GetValueOrDefault(0) <> 0

		lbl16_7.Visible = last24MonthCount.GetValueOrDefault(0) <> 0
		txt16_4.Visible = last24MonthCount.GetValueOrDefault(0) <> 0
		lbl16_8.Visible = last24MonthCount.GetValueOrDefault(0) <> 0

	End Sub

	Private Function LoadEmployeePayrollData(ByVal searchKind As EmployeeARGBSearchData) As IEnumerable(Of ARGBPayrollData)
		m_Payrolldata = m_EmployeeDatabaseAccess.LoadEmployeePayrollForARGBData(lueMandant.EditValue, m_EmployeeNumber, DateFrom.EditValue, DateTo.EditValue)

		If (m_Payrolldata Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht geladen werden."))
			Return Nothing
		End If


		Return m_Payrolldata
	End Function

	Private Function LoadEmployeePayrollLohnkontiData(ByVal searchKind As EmployeeARGBSearchData) As IEnumerable(Of ARGBPayrollData)
		Dim lohnkontidata = m_EmployeeDatabaseAccess.LoadEmployeePayrollForLohnkontiData(lueMandant.EditValue, m_EmployeeNumber, DateFrom.EditValue, DateTo.EditValue)

		If (lohnkontidata Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnkonti-Daten konnten nicht geladen werden."))
			Return Nothing
		End If


		Return lohnkontidata
	End Function

	Private Function LoadEmployeePayrollLastMonthsAHVData(ByVal searchKind As EmployeeARGBSearchData) As IEnumerable(Of ARGBAHVPayrollData)
		m_PayrollLastMonthAHVdata = m_EmployeeDatabaseAccess.LoadEmployeeAHVPayrollForARGBLastMonthData(lueMandant.EditValue, m_EmployeeNumber, DateTo.EditValue)

		If (m_PayrollLastMonthAHVdata Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AHV-Lohndaten konnten nicht geladen werden."))
			Return Nothing
		End If


		Return m_PayrollLastMonthAHVdata
	End Function

	''' <summary>
	''' Handles edit change event of lueYear.
	''' </summary>
	Private Sub OnDateFrom_EditValueChanged(sender As Object, e As EventArgs) Handles DateFrom.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not DateTo.EditValue Is Nothing AndAlso DateFrom.EditValue <= DateTo.EditValue Then Search()

	End Sub

	''' <summary>
	''' Handles edit change event of lueMonth.
	''' </summary>
	Private Sub OnDateTo_EditValueChanged(sender As Object, e As EventArgs) Handles DateTo.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Search()

	End Sub

	Sub OngvES_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvES.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvES.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Dim viewData = CType(dataRow, ZVESData)

				Select Case column.ToString.ToLower
					Case "CustomerName".ToLower, "CustomerNumber".ToLower
						If viewData.CustomerNumber > 0 Then OpenSelectedCustomer(viewData.CustomerNumber, viewData.MDNr)

					Case Else
						If viewData.ESNR > 0 Then OpenSelectedES(viewData.ESNR, viewData.MDNr)

				End Select

			End If

		End If

	End Sub

	Private Sub OnbbiSearch_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSearch.ItemClick
		Search()
	End Sub

	Private Function Search() As Boolean
		Dim result As Boolean = True

		DxErrorProvider1.ClearErrors()
		If DateFrom.EditValue Is Nothing OrElse DateTo.EditValue Is Nothing Then Return False

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
		Dim isValid As Boolean = True

		isValid = isValid AndAlso SetErrorIfInvalid(DateFrom, DxErrorProvider1, DateFrom.EditValue Is Nothing, errorText)
		If Not isValid Then Return False

		If Year(DateFrom.EditValue) <> Year(DateTo.EditValue) Then
			DateTo.EditValue = CType(String.Format("31.12.{0}", Year(DateFrom.EditValue)), Date)
		End If

		m_SearchCriteriums = New EmployeeARGBSearchData With {.MDNr = lueMandant.EditValue, .EmployeeNumbers = New List(Of Integer)({m_EmployeeNumber}), .DateFrom = DateFrom.EditValue, .DateTo = DateTo.EditValue}
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Year(DateFrom.EditValue)))

		ResetAllTabs()
		DisplayEmployeeData()

		result = result AndAlso LoadESOverviewData()
		DisplayESData()

		DisplayPayrollData()
		DisplayPayrollAHVLastFourYearsData()

		CreatePrintPopupMenu()
		bbiClear.Enabled = True
		bbiPrint.Enabled = True
		bbiSendEMail.Enabled = True

		Return result

	End Function

	Private Sub OngvPayroll_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs)

		If e.Column.FieldName = "Month1" OrElse e.Column.FieldName = "Month2" OrElse e.Column.FieldName = "Month3" OrElse e.Column.FieldName = "Month4" OrElse e.Column.FieldName = "Month5" OrElse e.Column.FieldName = "Month6" OrElse e.Column.FieldName = "Month7" OrElse e.Column.FieldName = "Month8" OrElse e.Column.FieldName = "Month9" OrElse e.Column.FieldName = "Month10" OrElse e.Column.FieldName = "Month11" OrElse e.Column.FieldName = "Month12" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub bbiClear_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiClear.ItemClick
		ResetAllTabs()
	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
	End Sub

	Private Sub OnbbiSendEMail_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSendEMail.ItemClick
		m_SelectedWOSEnun = WOSZVSENDValue.PrintWithoutSending
		m_ExportPrintInFiles = True
		StartExporting()
	End Sub

	Private Sub CreatePrintPopupMenu()

		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Drucken#Print"}

		Try
			Dim employeeWOS As Boolean = m_EmployeeData.Send2WOS AndAlso Not String.IsNullOrWhiteSpace(m_EmployeeData.Email)

			bbiPrint.Manager = Me.BarManager1
			Dim allowedEmployeWOS As Boolean = m_mandant.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year) AndAlso employeeWOS
			BarManager1.ForceInitialize()

			If Not allowedEmployeWOS Then
				liMnu = New List(Of String) From {"Drucken#Print"}
			Else
				liMnu = New List(Of String) From {"Drucken ohne Übermittlung#Print", "_WOS -> übermitteln#SendWOS_PrintRest", "Drucken mit Übermittlung#SendAndPrint"}
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
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Try
			Select Case e.Item.Name.ToUpper
				Case "Print".ToUpper
					m_SelectedWOSEnun = WOSZVSENDValue.PrintWithoutSending

				Case "SendWOS_PrintRest".ToUpper
					m_SelectedWOSEnun = WOSZVSENDValue.PrintOtherSendWOS

				Case "SendAndPrint".ToUpper
					m_SelectedWOSEnun = WOSZVSENDValue.PrintAndSend

				Case Else
					Return

			End Select
			StartPrinting()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub StartPrinting()
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim _setting As New ARGBPrintData With {.frmhwnd = Me.Handle, .PrintJobNumber = "1.7.2", .ShowAsDesign = ShowDesign}
		m_PDFFilesToPrint.Clear()

		_setting.m_EmployeeAddressData = m_EmployeeAddressData
		_setting.m_EmployeeContactCommData = m_EmployeeContactCommData
		_setting.m_EmployeeData = m_EmployeeData
		_setting.m_ESData = m_ESData
		_setting.m_PayrollLohnkontidata = LoadEmployeePayrollLohnkontiData(m_SearchCriteriums)

		_setting.WOSSendValueEnum = m_SelectedWOSEnun
		_setting.ExportPrintInFiles = If(m_SelectedWOSEnun = WOSZVSENDValue.PrintOtherSendWOS, False, chkPrintESVertrag.Checked OrElse chkPrintESVertrag.Checked) ' False

		Dim pagedata = LoadARGBPageData()

		Dim printUtil = New PrintEmployeeData(m_InitializationData)
		printUtil.PrintARGBData = _setting
		printUtil.PageARGBData = pagedata

		Dim result = printUtil.PrintEmployeeARGBData()

		printUtil.Dispose()

		If Not ShowDesign AndAlso result.Printresult AndAlso result.WOSresult AndAlso Not m_SelectedWOSEnun = WOSSENDValue.PrintWithoutSending Then
			Dim msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")

			m_UtilityUI.ShowInfoDialog(msg)

			Return
		ElseIf result.Printresult = False Then
			If Not String.IsNullOrWhiteSpace(result.PrintresultMessage) Then m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)

		End If

		If result.Printresult Then
			If _setting.ExportedFiles.Count > 0 Then
				For Each fileName In _setting.ExportedFiles
					m_PDFFilesToPrint.Add(fileName)
				Next
			End If

			If Not chkPrintESVertrag.Checked AndAlso Not chkPrintPayroll.Checked AndAlso Not _setting.ExportPrintInFiles Then Return
			If chkPrintESVertrag.Checked Then LoadEmployeeESVertrag()
			If chkPrintPayroll.Checked Then LoadEmployeePayrollDocument()

			MergeAndPrintExtraDocuments(False)
			Dim pdfFilesToSend As New List(Of String)
			If m_PDFFilesToPrint.Count = 1 Then
				Dim newFilename As String = Path.Combine(Path.GetDirectoryName(m_PDFFilesToPrint(0)), String.Format("{0}", "Arbeitgeberbescheinigung-Unterlagen.PDF"))
				If File.Exists(newFilename) Then File.Delete(newFilename)
				IO.File.Move(m_PDFFilesToPrint(0), newFilename)
				pdfFilesToSend.Add(newFilename)

			End If

			If pdfFilesToSend.Count > 0 Then
				Dim tmpfileName As String = pdfFilesToSend(0)
				Dim pdfViewer As New frmViewPDF(tmpfileName)
				pdfViewer.OpenPDFDocument()

				If chkOpenpdffile.Checked Then
					pdfViewer.Show()
					pdfViewer.BringToFront()
				Else
					pdfViewer.PdfViewer.Print()
					pdfViewer.PdfViewer.CloseDocument()
				End If
			End If

		End If

	End Sub

	Sub StartExporting()
		Dim _setting As New ARGBPrintData With {.frmhwnd = Me.Handle, .PrintJobNumber = "1.7.2", .ShowAsDesign = False}
		Dim success As Boolean = True
		Dim pdfFilesToSend As New List(Of String)
		m_PDFFilesToPrint.Clear()

		_setting.m_EmployeeAddressData = m_EmployeeAddressData
		_setting.m_EmployeeContactCommData = m_EmployeeContactCommData
		_setting.m_EmployeeData = m_EmployeeData
		_setting.m_ESData = m_ESData
		_setting.m_PayrollLohnkontidata = LoadEmployeePayrollLohnkontiData(m_SearchCriteriums)

		_setting.ExportPrintInFiles = True
		_setting.WOSSendValueEnum = m_SelectedWOSEnun

		Dim pagedata = LoadARGBPageData()

		Dim printUtil = New PrintEmployeeData(m_InitializationData)
		printUtil.PrintARGBData = _setting
		printUtil.PageARGBData = pagedata

		Dim result = printUtil.PrintEmployeeARGBData()

		printUtil.Dispose()

		If Not result.Printresult Then
			m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)

		Else

			If _setting.ExportedFiles.Count > 0 Then
				For Each fileName In _setting.ExportedFiles
					m_PDFFilesToPrint.Add(fileName)
				Next
			End If

			If m_JustExportDataIntoFile Then Return

			If chkPrintESVertrag.Checked Then LoadEmployeeESVertrag()
			If chkPrintPayroll.Checked Then LoadEmployeePayrollDocument()

			success = success AndAlso MergeAndPrintExtraDocuments(False)
			Try
				If m_PDFFilesToPrint.Count = 1 Then
					Dim newFilename As String = Path.Combine(Path.GetDirectoryName(m_PDFFilesToPrint(0)), String.Format("{0}", "Arbeitgeberbescheinigung-Unterlagen.PDF"))
					If File.Exists(newFilename) Then File.Delete(newFilename)
					IO.File.Move(m_PDFFilesToPrint(0), newFilename)
					pdfFilesToSend.Add(newFilename)

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
			End Try

		End If

		If success Then
			Dim frmMail = New frmMailTpl(m_InitializationData)

			Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.ARGB, .EmployeeNumber = m_EmployeeNumber, .PDFFilesToSend = pdfFilesToSend}
			frmMail.PreselectionData = preselectionSetting

			frmMail.LoadData()

			frmMail.Show()
			frmMail.BringToFront()

		End If

	End Sub


	Private Function LoadARGBPageData() As ARGBPageData
		Dim result As ARGBPageData = Nothing

		Dim pageData As New ARGBPageData

		pageData.DateFrom = DateFrom.EditValue
		pageData.DateTo = DateTo.EditValue
		pageData.Compensationfund = txtCompensationfund.EditValue
		pageData.CompanyBURNumber = BURNumberSetting
		pageData.CompanyUnfallfund = UnfallfundSetting
		pageData.CompanyUnfallfundNumber = UnfallfundNumberSetting

		pageData.txt9_1 = txt9_1.EditValue
		pageData.de9_1 = CType(de9_1.EditValue, Date)
		pageData.de9_2 = CType(de9_2.EditValue, Date)
		pageData.op10 = op10.EditValue
		pageData.txt11 = txt11.EditValue
		pageData.op12 = op12.EditValue
		pageData.txt12 = txt12.EditValue
		pageData.de12_1 = CType(de12_1.EditValue, Date)
		pageData.de12_2 = CType(de12_2.EditValue, Date)
		pageData.txt13 = txt13.EditValue
		pageData.de14 = CType(de14.EditValue, Date)
		pageData.de15 = CType(de15.EditValue, Date)

		pageData.m_CountWorked6Month = m_CountWorked6Month
		pageData.m_CountWorked12Month = m_CountWorked12Month
		pageData.m_CountWorked15Month = m_CountWorked15Month
		pageData.m_CountWorked24Month = m_CountWorked24Month

		pageData.txt16_1 = CType(CDbl(txt16_1.EditValue), Decimal)
		pageData.txt16_2 = CType(CDbl(txt16_2.EditValue), Decimal)
		pageData.txt16_3 = CType(CDbl(txt16_3.EditValue), Decimal)
		pageData.txt16_4 = CType(CDbl(txt16_4.EditValue), Decimal)
		pageData.txt17 = CType(CDbl(txt17.EditValue), Decimal)
		pageData.txt18_1 = CType(CDbl(txt18_1.EditValue), Decimal)
		pageData.txt18_2 = CType(CDbl(txt18_2.EditValue), Decimal)

		pageData.op19_1 = op19_1.EditValue
		pageData.op19_2 = op19_2.EditValue
		pageData.txt19 = CType(CDbl(txt19.EditValue), Decimal)
		pageData.op20_1 = op20_1.EditValue
		pageData.txt20_1 = CType(CDbl(txt20_1.EditValue), Decimal)
		pageData.txt20_2 = CType(CDbl(txt20_2.EditValue), Decimal)
		pageData.op20_2 = op20_2.EditValue
		pageData.op21_1 = op21_1.EditValue
		pageData.txt21 = CType(CDbl(txt21.EditValue), Decimal)
		pageData.op22 = op22.EditValue
		pageData.txt22 = txt22.EditValue
		pageData.op23 = op23.EditValue
		pageData.txt23 = txt23.EditValue

		pageData.de24_1 = CType(de24_1.EditValue, Date)
		pageData.de24_2 = CType(de24_2.EditValue, Date)
		pageData.de25_1 = CType(de25_1.EditValue, Date)
		pageData.de25_2 = CType(de25_2.EditValue, Date)
		pageData.de26_1 = CType(de26_1.EditValue, Date)
		pageData.de26_2 = CType(de26_2.EditValue, Date)
		pageData.de27_1 = CType(de27_1.EditValue, Date)
		pageData.de27_2 = CType(de27_2.EditValue, Date)
		pageData.de28_1 = CType(de28_1.EditValue, Date)
		pageData.de28_2 = CType(de28_2.EditValue, Date)


		Return pageData

	End Function

	Private Function LoadEmployeeESVertrag() As Boolean
		Dim result As Boolean = True

		For Each ES In m_ESData
			Dim number As Integer = ES.ESNR.GetValueOrDefault(0)
			Dim data = m_EmployeeDatabaseAccess.LoadEmployeeDocumentForZVData(m_EmployeeNumber, number, 211)
			If data.ID > 0 Then
				Dim fileName As String = LoadAssignedDocument(data)
				If Not String.IsNullOrWhiteSpace(fileName) Then
					m_PDFFilesToPrint.Add(fileName)
				End If
			End If
		Next


		Return result

	End Function

	Private Function LoadEmployeePayrollDocument() As Boolean
		Dim result As Boolean = True
		Dim payrollNumbers = m_EmployeeDatabaseAccess.LoadEmployeePayrollForPrintWithARGBData(m_SearchCriteriums.MDNr, m_EmployeeNumber, m_SearchCriteriums.DateFrom, m_SearchCriteriums.DateTo)

		For Each itm In payrollNumbers
			Dim number As Integer = itm.LONr
			Dim data = m_EmployeeDatabaseAccess.LoadEmployeeDocumentForZVData(m_EmployeeNumber, number, 212)
			If data.ID = 0 Then
				data = m_EmployeeDatabaseAccess.LoadEmployeePrintedDocumentForZVData(m_EmployeeNumber, itm.monat, itm.jahr, 212)
				If data.ID > 0 Then
					Dim fileName As String = LoadAssignedPrintedDocument(data)
					If Not String.IsNullOrWhiteSpace(fileName) Then
						m_PDFFilesToPrint.Add(fileName)
					End If
				End If

			Else

				Dim fileName As String = LoadAssignedDocument(data)
				If Not String.IsNullOrWhiteSpace(fileName) Then
					m_PDFFilesToPrint.Add(fileName)
				End If
			End If
		Next


		Return result

	End Function

	Private Function MergeAndPrintExtraDocuments(ByVal printJob As Boolean?) As Boolean
		Dim result As Boolean = True

		If m_PDFFilesToPrint.Count = 0 Then Return Nothing
		Dim fileName As String = m_PDFFilesToPrint(0)

		Try

			fileName = Path.GetRandomFileName
			fileName = Path.ChangeExtension(fileName, "pdf")
			fileName = Path.Combine(m_InitializationData.UserData.spTempEmployeePath, fileName)
			If m_PDFFilesToPrint.Count > 1 Then
				Dim mergePDF = result AndAlso m_PDFUtility.MergePdfFiles(m_PDFFilesToPrint.ToArray, fileName)
				If Not mergePDF Then

				End If
			Else
				File.Copy(m_PDFFilesToPrint(0), fileName)

			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(String.Format("Möglicherweise sind die Anhänge nicht in PDF-Format.<br>{0}", ex.ToString))
			m_Logger.LogError(ex.ToString)

			Return False
		End Try


		'pdfDocument.LoadDocument(fileName)
		'For i As Integer = 1 To m_PDFFilesToPrint.Count - 1
		'	pdfDocument.AppendDocument(m_PDFFilesToPrint(i))
		'	pdfDocument.SaveDocument(fileName)
		'Next

		Try

			If printJob.GetValueOrDefault(True) Then
				Dim pdfDocument As New PdfDocumentProcessor()
				Dim dlgPrinter As New PrintDialog
				Dim pdfPrinterSettingsData As New Printing.PrinterSettings

				dlgPrinter.ShowNetwork = True

				If dlgPrinter.ShowDialog(Me) <> DialogResult.OK Then Return False
				pdfPrinterSettingsData = dlgPrinter.PrinterSettings

				Dim pdfPrinterSettings As New PdfPrinterSettings(pdfPrinterSettingsData)

				pdfDocument.Print(pdfPrinterSettings)
				pdfDocument.CloseDocument()

			End If

			m_PDFFilesToPrint.Clear()
			m_PDFFilesToPrint.Add(fileName)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			m_Logger.LogError(ex.ToString)

			Return False
		End Try


		Return result

	End Function

#Region "Helper Methodes"

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

	Private Function LoadAssignedDocument(ByVal docData As EmployeeDocumentData) As String
		Dim currentExtension As String = String.Empty

		If Not docData Is Nothing Then
			Dim bytes() = m_EmployeeDatabaseAccess.LoadEmployeeDocumentBytesData(docData.ID)
			Dim tempFileName = System.IO.Path.GetTempFileName()

			If docData.ScanExtension = String.Empty Then
				If docData.FileFullPath <> String.Empty AndAlso IO.File.Exists(docData.FileFullPath) Then
					currentExtension = System.IO.Path.GetExtension(docData.FileFullPath)
					currentExtension = currentExtension.Replace(".", "")
				End If

			Else

				currentExtension = docData.ScanExtension

			End If
			Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, currentExtension)

			If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
				Return tempFileFinal
			End If

		End If


		Return String.Empty

	End Function

	Private Function LoadAssignedPrintedDocument(ByVal docData As EmployeeDocumentData) As String
		Dim currentExtension As String = String.Empty

		If Not docData Is Nothing Then
			Dim bytes() = m_EmployeeDatabaseAccess.LoadEmployeePrintedDocumentBytesData(docData.ID)
			Dim tempFileName = System.IO.Path.GetTempFileName()

			If docData.ScanExtension = String.Empty Then
				If docData.FileFullPath <> String.Empty AndAlso IO.File.Exists(docData.FileFullPath) Then
					currentExtension = System.IO.Path.GetExtension(docData.FileFullPath)
					currentExtension = currentExtension.Replace(".", "")
				End If

			Else

				currentExtension = docData.ScanExtension

			End If
			Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, currentExtension)

			If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
				Return tempFileFinal
			End If

		End If


		Return String.Empty

	End Function

	''' <summary>
	''' Gets the age in years.
	''' </summary>
	''' <param name="birthDate">The birthdate.</param>
	''' <returns>Age in years.</returns>
	Private Function GetAge(ByVal birthDate As DateTime)

		' Get year diff
		Dim years As Integer = DateTime.Now.Year - birthDate.Year

		birthDate = birthDate.AddYears(years)

		' Subtract another year if its a day before the the birth day
		If (DateTime.Today.CompareTo(birthDate) < 0) Then
			years = years - 1
		End If

		Return years

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
			ElseIf TypeOf sender Is DateEdit Then
				Dim de As DateEdit = CType(sender, DateEdit)
				de.EditValue = Nothing
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
				comboboxEdit.EditValue = Nothing
			End If
		End If
	End Sub

	''' <summary>
	''' Sets the suppress UI events state.
	''' </summary>
	''' <param name="shouldEventsBeSuppressed">Boolean flag indicating the  UI events should be suppressed.</param>
	''' <returns>Previous state of suppress events.</returns>
	Public Function SetSuppressUIEventsState(ByVal shouldEventsBeSuppressed As Boolean)

		Dim orginalState = m_SuppressUIEvents
		m_SuppressUIEvents = shouldEventsBeSuppressed

		Return orginalState

	End Function

	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText, ErrorType.Critical)
		End If

		Return Not invalid

	End Function

	Private Function SetWarnIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText, ErrorType.Warning)
		End If

		Return Not invalid

	End Function

	Private Function SafeGetValueAsDecimal(ByVal value As Object, ByVal defaultValue As Decimal?) As Decimal?

		If (Not value Is Nothing AndAlso Not String.IsNullOrWhiteSpace(value)) Then
			Return CType(value, Decimal)
		Else
			Return defaultValue
		End If
	End Function


#End Region


#Region "Helper Classes"


	''' <summary>
	''' Wraps an integer value.
	''' </summary>
	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class

	Public Class ZVHourAbsenceViewData
		Public Property WorkingDay As Date?
		Public Property WorkingHour As Decimal?
		Public Property AbsenceCode As String
	End Class


#End Region


End Class


Public Class PreselectionARGBData
	Public Property MDNr As Integer
	Public Property Year As Integer?
	Public Property Month As Integer?
	Public Property EmployeeNumber As Integer?

End Class
