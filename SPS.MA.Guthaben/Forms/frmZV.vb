
Imports System.IO

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects


Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.XtraBars
Imports System.ComponentModel

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
Imports DevExpress.Pdf
Imports DevExpress.XtraEditors
Imports SPSSendMail.RichEditSendMail
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure

Public Class frmZV
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
	Private m_AHVSetting As String

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String

	Private m_Years As List(Of IntegerValueViewWrapper)
	Private m_Month As List(Of IntegerValueViewWrapper)
	Private m_EmployeeNumber As Integer?

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private m_EmployeeAddressData As EmployeeSAddressData
	Private m_EmployeeData As EmployeeMasterData
	Private m_EmployeeContactCommData As EmployeeContactComm
	Private m_ESData As IEnumerable(Of ZVESData)
	Private m_HourAbsencedata As ZVHourAbsenceData
	Private m_Payrolldata As IEnumerable(Of ZVPayrollData)

	Private m_SearchCriteriums As EmployeeSuvaSearchData

	Private m_SelectedWOSEnun As WOSZVSENDValue

	Private m_PDFFilesToPrint As List(Of String)
	Private m_ExportPrintInFiles As Boolean

	Private m_Percent_500 As Decimal?
	Private m_Percent_600 As Decimal?
	Private m_Percent_700 As Decimal?
	Private m_PDFUtility As PDFUtilities.Utilities

	Private m_NotifyUser As Boolean
	Private m_JustExportDataIntoFile As Boolean

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
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
	Public Property PreselectionData As PreselectionZVData

	Public ReadOnly Property ZVExportFilename As String
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
		m_PDFFilesToPrint = New List(Of String)
		m_PDFUtility = New PDFUtilities.Utilities

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
		m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		m_NotifyUser = True

		TranslateControls()
		Reset()

		LoadMandantDropDownData()

		AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick


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

		Try
			If Not PreselectionData.EmployeeNumber Is Nothing Then
				m_EmployeeNumber = PreselectionData.EmployeeNumber
			End If

			If Not PreselectionData.Year Is Nothing Then
				m_Years = New List(Of IntegerValueViewWrapper)
				m_Years.Add(New IntegerValueViewWrapper With {.Value = PreselectionData.Year})
				lueYear.EditValue = m_Years(0)
			End If
			If Not PreselectionData.Month Is Nothing Then
				m_Month = New List(Of IntegerValueViewWrapper)
				m_Month.Add(New IntegerValueViewWrapper With {.Value = PreselectionData.Month})
				lueMonth.EditValue = m_Month(0)
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

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

		m_SearchCriteriums = New EmployeeSuvaSearchData With {.MDNr = lueMandant.EditValue, .EmployeeNumbers = New List(Of Integer)({m_EmployeeNumber})}

		success = success AndAlso LoadMandantYearDropDownData()
		success = success AndAlso LoadMonthDropDownData()


		m_SuppressUIEvents = False

		If Not (lueMandant.EditValue Is Nothing AndAlso lueYear.EditValue Is Nothing AndAlso lueMonth.EditValue Is Nothing) Then Search()

		Return success

	End Function

	Public Function CreateZVDocument() As Boolean
		Dim success As Boolean = True

		m_SuppressUIEvents = True

		m_NotifyUser = False
		m_JustExportDataIntoFile = True
		m_PDFFilesToPrint = New List(Of String)

		PreselectData()

		m_SearchCriteriums = New EmployeeSuvaSearchData With {.MDNr = lueMandant.EditValue, .Monat = PreselectionData.Month, .Jahr = PreselectionData.Year, .EmployeeNumbers = New List(Of Integer)({m_EmployeeNumber})}

		success = success AndAlso LoadMandantYearDropDownData()
		success = success AndAlso LoadMonthDropDownData()
		If Not success Then Return success

		DisplayEmployeeData()

		ResetAllTabs()
		success = success AndAlso LoadESOverviewData()
		If Not success Then Return success

		DisplayESData()
		success = success AndAlso CreateEmployeeHourData()

		DisplayPayrollData()

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

		Me.Text = String.Format(m_Translate.GetSafeTranslationValue("Zwischenverdienstformular für: [{0}]"), m_EmployeeAddressData.EmployeeFullname)
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
			If My.Settings.iZVHeight > 0 Then Me.Height = My.Settings.iZVHeight
			If My.Settings.iZVWidth > 0 Then Me.Width = My.Settings.iZVWidth
			If My.Settings.frmZVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmZVLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

			chkPrintESVertrag.Checked = My.Settings.chkPrintESVertragWithZV
			chkPrintPayroll.Checked = My.Settings.chkPrintPayrollWithZV
			chkOpenpdffile.Checked = My.Settings.chkOpenpdffile

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
		End Try

	End Sub

	Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		SplashScreenManager.CloseForm(False)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iZVHeight = Me.Height
			My.Settings.iZVWidth = Me.Width
			My.Settings.frmZVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.chkPrintESVertragWithZV = chkPrintESVertrag.Checked
			My.Settings.chkPrintPayrollWithZV = chkPrintPayroll.Checked
			My.Settings.chkOpenpdffile = chkOpenpdffile.Checked

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	'''  Translate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue("Zwischenverdienstformular für: {0}")
		lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)

		grpadresse.Text = m_Translate.GetSafeTranslationValue("Kandidat: {0}")
		grpData.Text = m_Translate.GetSafeTranslationValue(grpData.Text)
		grpALK.Text = m_Translate.GetSafeTranslationValue(grpALK.Text)

		chkPrintESVertrag.Text = m_Translate.GetSafeTranslationValue(chkPrintESVertrag.Text)
		chkPrintPayroll.Text = m_Translate.GetSafeTranslationValue(chkPrintPayroll.Text)
		chkOpenpdffile.Text = m_Translate.GetSafeTranslationValue(chkOpenpdffile.Text)

		lblNachname.Text = m_Translate.GetSafeTranslationValue(Me.lblNachname.Text)

		lblCOAdresse.Text = m_Translate.GetSafeTranslationValue(Me.lblCOAdresse.Text)
		lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
		lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)

		lblAdresse.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresse.Text)
		lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGeburtsdatum.Text)
		lblZivilstand.Text = m_Translate.GetSafeTranslationValue(Me.lblZivilstand.Text)
		lblAHVNrNeu.Text = m_Translate.GetSafeTranslationValue(Me.lblAHVNrNeu.Text)
		lblEmailForZv.Text = m_Translate.GetSafeTranslationValue(Me.lblEmailForZv.Text)

		xtabESRP.Text = m_Translate.GetSafeTranslationValue(xtabESRP.Text)
		xtabArbeitsangebot.Text = m_Translate.GetSafeTranslationValue(xtabArbeitsangebot.Text)
		xtabUnsTaetigkeit.Text = m_Translate.GetSafeTranslationValue(xtabUnsTaetigkeit.Text)
		xtabSonstiges.Text = m_Translate.GetSafeTranslationValue(xtabSonstiges.Text)
		xtabSelbTaetigkeit.Text = m_Translate.GetSafeTranslationValue(xtabSelbTaetigkeit.Text)
		xtabAddressFund.Text = m_Translate.GetSafeTranslationValue(xtabAddressFund.Text)

		lbl1.Text = m_Translate.GetSafeTranslationValue(lbl1.Text)
		lbl1_1.Text = m_Translate.GetSafeTranslationValue(lbl1_1.Text)
		lbl1_2.Text = m_Translate.GetSafeTranslationValue(lbl1_2.Text)
		lbl1_3.Text = m_Translate.GetSafeTranslationValue(lbl1_3.Text)
		lbl25.Text = m_Translate.GetSafeTranslationValue(lbl25.Text)

		lbl6.Text = m_Translate.GetSafeTranslationValue(lbl6.Text)
		lbl6_1.Text = m_Translate.GetSafeTranslationValue(lbl6_1.Text)
		lbl6_2.Text = m_Translate.GetSafeTranslationValue(lbl6_2.Text)
		lbl6_3.Text = m_Translate.GetSafeTranslationValue(lbl6_3.Text)
		lbl7.Text = m_Translate.GetSafeTranslationValue(lbl7.Text)
		lbl8.Text = m_Translate.GetSafeTranslationValue(lbl8.Text)
		lbl9.Text = m_Translate.GetSafeTranslationValue(lbl9.Text)
		lbl10_1.Text = m_Translate.GetSafeTranslationValue(lbl10_1.Text)

		lbl10.Text = m_Translate.GetSafeTranslationValue(lbl10.Text)
		lbl10_3.Text = m_Translate.GetSafeTranslationValue(lbl10_3.Text)
		lbl10_5.Text = m_Translate.GetSafeTranslationValue(lbl10_5.Text)
		lbl10_7.Text = m_Translate.GetSafeTranslationValue(lbl10_7.Text)
		lbl10_9.Text = m_Translate.GetSafeTranslationValue(lbl10_9.Text)

		lbl11.Text = m_Translate.GetSafeTranslationValue(lbl11.Text)
		lbl11_1.Text = m_Translate.GetSafeTranslationValue(lbl11_1.Text)
		lbl11_2.Text = m_Translate.GetSafeTranslationValue(lbl11_2.Text)
		chk11_1.Text = m_Translate.GetSafeTranslationValue(chk11_1.Text)
		chk11_2.Text = m_Translate.GetSafeTranslationValue(chk11_2.Text)

		lbl14.Text = m_Translate.GetSafeTranslationValue(lbl14.Text)
		lbl14_1.Text = m_Translate.GetSafeTranslationValue(lbl14_1.Text)
		lbl14_2.Text = m_Translate.GetSafeTranslationValue(lbl14_2.Text)
		lbl14_3.Text = m_Translate.GetSafeTranslationValue(lbl14_3.Text)
		lbl14_4.Text = m_Translate.GetSafeTranslationValue(lbl14_4.Text)

		lbl15.Text = m_Translate.GetSafeTranslationValue(lbl15.Text)
		chk15.Text = m_Translate.GetSafeTranslationValue(chk15.Text)
		lbl15_1.Text = m_Translate.GetSafeTranslationValue(lbl15_1.Text)
		lbl15_2.Text = m_Translate.GetSafeTranslationValue(lbl15_2.Text)

		lbl16.Text = m_Translate.GetSafeTranslationValue(lbl16.Text)
		lbl17.Text = m_Translate.GetSafeTranslationValue(lbl17.Text)

		lbl18.Text = m_Translate.GetSafeTranslationValue(lbl18.Text)
		lbl18_1.Text = m_Translate.GetSafeTranslationValue(lbl18_1.Text)
		lbl18_2.Text = m_Translate.GetSafeTranslationValue(lbl18_2.Text)
		lbl18_3.Text = m_Translate.GetSafeTranslationValue(lbl18_3.Text)
		lbl18_4.Text = m_Translate.GetSafeTranslationValue(lbl18_4.Text)
		lbl18_5.Text = m_Translate.GetSafeTranslationValue(lbl18_5.Text)

		lbl12.Text = m_Translate.GetSafeTranslationValue(lbl12.Text)
		lbl13.Text = m_Translate.GetSafeTranslationValue(lbl13.Text)
		lbl19_1.Text = m_Translate.GetSafeTranslationValue(lbl19_1.Text)
		lbl19_2.Text = m_Translate.GetSafeTranslationValue(lbl19_2.Text)
		lbl19_3.Text = m_Translate.GetSafeTranslationValue(lbl19_3.Text)
		lbl19_4.Text = m_Translate.GetSafeTranslationValue(lbl19_4.Text)
		lbl19_5.Text = m_Translate.GetSafeTranslationValue(lbl19_5.Text)
		lbl19_6.Text = m_Translate.GetSafeTranslationValue(lbl19_6.Text)
		lbl19_7.Text = m_Translate.GetSafeTranslationValue(lbl19_7.Text)

		bbiSearch.Caption = m_Translate.GetSafeTranslationValue(bbiSearch.Caption)
		bbiClear.Caption = m_Translate.GetSafeTranslationValue(bbiClear.Caption)
		bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)
		bbiSendEMail.Caption = m_Translate.GetSafeTranslationValue(bbiSendEMail.Caption)

	End Sub


#Region "Reset"

	Private Sub Reset()
		Dim previousState = SetSuppressUIEventsState(True)

		Dim connectionString As String = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_Years = Nothing
		m_Month = Nothing

		ResetAllTabs()

		ResetMandantDropDown()
		ResetYearDropDown()
		ResetMonthDropDown()

		'  Reset grids, drop downs and lists, etc.
		ResetReportGrid()
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
		ResetTab6()

		m_SuppressUIEvents = supressUIEventState ' Make sure UI event are fired so that the lookup data is loaded correctly.

	End Sub

	Private Sub ResetTab1()

		m_HourAbsencedata = Nothing
		m_ESData = Nothing
		m_Payrolldata = Nothing

		grdTotalHours.DataSource = Nothing
		grdES.DataSource = Nothing

		lbltotalstdmonthValue.Text = "0.00"
		lblAuszahlungGleitValue.Text = "0.00"
		lblNachtzeitValue.Text = "0.00"
		lblAuszahlungNachtzulageValue.Text = "0.00"

	End Sub

	Private Sub ResetTab2()

		op6.Properties.Items.Clear()
		Dim itemValues As Object() = New Object() {0, 1}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("ja"), m_Translate.GetSafeTranslationValue("nein")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			op6.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op6.EditValue = 1

		txt6_1.EditValue = 0D
		txt6_2.EditValue = 0D
		txt6_3.EditValue = 0D

		txt7.EditValue = Nothing

	End Sub

	Private Sub ResetTab3()

		txt8_1.EditValue = 0D
		txt8_2.EditValue = 0D
		txt9.EditValue = 0D

		txt10_1.EditValue = 0D
		txt10_2.EditValue = 0D
		txt10_3.EditValue = 0D
		txt10_4.EditValue = 0D
		txt10_5.EditValue = Nothing
		txt10_6.EditValue = 0D

		txt11_1.EditValue = Nothing
		txt11_2.EditValue = 0D
		chk11_1.Checked = False
		chk11_2.Checked = False

	End Sub

	Private Sub ResetTab4()

		txt14_1.EditValue = Nothing
		txt14_2.EditValue = 0D
		txt14_3.EditValue = 0D
		txt14_4.EditValue = Nothing
		txt14_5.EditValue = 0D
		txt14_6.EditValue = Nothing

		txt15_1.EditValue = Nothing
		txt15_2.EditValue = Nothing
		txt15_3.EditValue = Nothing
		txt15_4.EditValue = Nothing

		op15.Properties.Items.Clear()
		Dim itemValues As Object() = New Object() {0, 1}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("ja, auf unbestimmte Zeit"), m_Translate.GetSafeTranslationValue("ja, voraussichtlich bis")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			op15.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op15.EditValue = Nothing
		chk15.Checked = False

		txt16.EditValue = Nothing

		op17.Properties.Items.Clear()
		itemValues = New Object() {0, 1}
		itemDescriptions = New String() {m_Translate.GetSafeTranslationValue("Ja"), m_Translate.GetSafeTranslationValue("Nein")}
		i = 0
		Do While i < itemValues.Length
			op17.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op17.EditValue = 1

	End Sub

	Private Sub ResetTab5()

		txt18_1.EditValue = 0D
		txt18_2.EditValue = 0D
		txt18_3.EditValue = 0D
		txt18_4.EditValue = 0D
		txt18_5.EditValue = 0D

	End Sub

	Private Sub ResetTab6()

		op12.Properties.Items.Clear()
		Dim itemValues As Object() = New Object() {0, 1}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("Ja"), m_Translate.GetSafeTranslationValue("Nein")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			op12.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		op12.EditValue = Nothing

		txtBVGfund.EditValue = Nothing
		txtCompensationfund.EditValue = Nothing

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
	''' Resets the year drop down.
	''' </summary>
	Private Sub ResetYearDropDown()

		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the month drop down.
	''' </summary>
	Private Sub ResetMonthDropDown()

		lueMonth.Properties.DisplayMember = "Value"
		lueMonth.Properties.ValueMember = "Value"
		lueMonth.Properties.ShowHeader = False

		lueMonth.Properties.Columns.Clear()
		lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueMonth.Properties.ShowFooter = False
		lueMonth.Properties.DropDownRows = 10
		lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonth.Properties.SearchMode = SearchMode.AutoComplete
		lueMonth.Properties.AutoSearchColumnIndex = 0

		lueMonth.Properties.NullText = String.Empty
		lueMonth.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the report overview grid.
	''' </summary>
	Private Sub ResetReportGrid()

		' Reset the grid
		gvTotalHours.OptionsView.ShowIndicator = False
		gvTotalHours.OptionsView.ColumnAutoWidth = True
		gvTotalHours.OptionsView.ShowAutoFilterRow = False

		gvTotalHours.Columns.Clear()

		Dim columnWorkingDay As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkingDay.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnWorkingDay.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnWorkingDay.Name = "WorkingDay"
		columnWorkingDay.FieldName = "WorkingDay"
		columnWorkingDay.MaxWidth = 100
		columnWorkingDay.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnWorkingDay.DisplayFormat.FormatString = "d"
		columnWorkingDay.Visible = True
		gvTotalHours.Columns.Add(columnWorkingDay)

		Dim columnWorkingHour As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkingHour.Caption = m_Translate.GetSafeTranslationValue("Stunden")
		columnWorkingHour.Name = "WorkingHour"
		columnWorkingHour.FieldName = "WorkingHour"
		columnWorkingHour.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnWorkingHour.DisplayFormat.FormatString = "F2"
		columnWorkingHour.Visible = True
		columnWorkingHour.Width = 50
		gvTotalHours.Columns.Add(columnWorkingHour)

		Dim columnAbsenceCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAbsenceCode.Caption = m_Translate.GetSafeTranslationValue("Fehlcode")
		columnAbsenceCode.Name = "AbsenceCode"
		columnAbsenceCode.FieldName = "AbsenceCode"
		columnAbsenceCode.Visible = True
		columnAbsenceCode.Width = 30
		gvTotalHours.Columns.Add(columnAbsenceCode)


		grdTotalHours.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the es overview grid.
	''' </summary>
	Private Sub ResetESGrid()

		' Reset the grid
		gvES.OptionsView.ShowIndicator = False
		gvES.OptionsView.ColumnAutoWidth = True
		gvES.OptionsView.ShowAutoFilterRow = False

		gvES.Columns.Clear()

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.Caption = m_Translate.GetSafeTranslationValue("ESNr")
		columnESNr.Name = "ESNr"
		columnESNr.FieldName = "ESNr"
		columnESNr.Visible = False
		gvES.Columns.Add(columnESNr)

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
	''' Handles edit change event of lueYear.
	''' </summary>
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		If m_EmployeeNumber Is Nothing Then Return

		Dim success = LoadMandantYearDropDownData()
		success = success AndAlso LoadMonthDropDownData()


	End Sub

	''' <summary>
	''' Loads the mandant drop down data.
	''' </summary>
	Private Function LoadMandantYearDropDownData() As Boolean

		Dim success As Boolean = True

		Dim mandantNumber = lueMandant.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not mandantNumber Is Nothing Then

			Dim yearData = m_CommonDatabaseAccess.LoadEmployeeExistsPayrollYear(mandantNumber, m_EmployeeNumber)

			If (yearData Is Nothing) Then
				success = False
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Lohndaten) konnten nicht geladen werden."))
			End If

			If Not yearData Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For Each yearValue In yearData
					wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
				Next

			End If

		End If

		m_Years = wrappedValues

		lueYear.EditValue = If(PreselectionData.Year Is Nothing OrElse Not m_Years.Any(Function(myObject) myObject.Value = PreselectionData.Year), Nothing, PreselectionData.Year)
		lueYear.Properties.DataSource = m_Years
		lueYear.Properties.ForceInitialize()

		Return success
	End Function

	''' <summary>
	''' Loads the month drop down data.
	''' </summary>
	Private Function LoadMonthDropDownData() As Boolean

		Dim success As Boolean = True

		Dim mandantNumber = lueMandant.EditValue
		Dim year = lueYear.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not mandantNumber Is Nothing AndAlso Not year Is Nothing Then

			Dim payrollMonth = m_CommonDatabaseAccess.LoadEmployeeExistsPayrollMonthOfYear(mandantNumber, m_EmployeeNumber, year)

			If (payrollMonth Is Nothing) Then
				success = False
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verrechnete Monate konnten nicht geladen werden."))
			End If

			If Not payrollMonth Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For i As Integer = 1 To 12

					If payrollMonth.Contains(i) Then
						wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
					End If

				Next

			End If

		End If

		m_Month = wrappedValues
		lueMonth.EditValue = If(PreselectionData.Month Is Nothing OrElse Not m_Month.Any(Function(myObject) myObject.Value = PreselectionData.Month), Nothing, PreselectionData.Month)
		lueMonth.Properties.DataSource = m_Month
		lueMonth.Properties.ForceInitialize()

		Return success
	End Function



	''' <summary>
	''' Load employee zv address data.
	''' </summary>
	Private Function LoadEmployeeAddressData() As Boolean

		m_EmployeeAddressData = m_EmployeeDatabaseAccess.LoadEmployeeZvAddressData(m_EmployeeNumber)

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
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date

		dStartofMonth = CDate("01." & lueMonth.EditValue & "." & lueYear.EditValue)
		dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)

		m_ESData = m_EmployeeDatabaseAccess.LoadESData2ForZVForm(lueMandant.EditValue, m_EmployeeNumber, dStartofMonth, dEndofMonth)

		If (m_ESData Is Nothing OrElse m_ESData.Count = 0) AndAlso m_NotifyUser Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Daten konnten nicht geladen werden."))
		End If


		grdES.DataSource = m_ESData

		Return (Not m_ESData Is Nothing) AndAlso m_ESData.Count > 0

	End Function

	''' <summary>
	''' Loads report overview data.
	''' </summary>
	Private Function CreateEmployeeHourData() As Boolean
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date
		Dim esEnd As Date?
		Dim esBegin As Date?

		dStartofMonth = CDate("01." & lueMonth.EditValue & "." & lueYear.EditValue)
		dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)

		If m_ESData Is Nothing OrElse m_ESData.Count = 0 Then Return False
		esBegin = m_ESData(0).ES_Ab
		Dim query = From p In m_ESData
					Group p By p.ES_Ende Into g = Group
					Select ES_Ende, MaxPrice = g.Max(Function(p) p.ES_Ende)
		esEnd = (From d In m_ESData Select d.ES_Ende).Max()
		For Each es In m_ESData
			If es.ES_Ende Is Nothing Then
				esEnd = dEndofMonth

				Exit For
			End If
		Next


		If esEnd Is Nothing Then esEnd = New DateTime(3999, 12, 31)
		esBegin = m_DateUtility.MaxDate(esBegin, dStartofMonth)
		esEnd = m_DateUtility.MinDate(esEnd, dEndofMonth)

		Dim foundedData = LoadEmployeeMonthHourData(m_SearchCriteriums)

		Dim listDataSource As BindingList(Of ZVHourAbsenceViewData) = New BindingList(Of ZVHourAbsenceViewData)
		If lueYear.EditValue Is Nothing OrElse lueMonth.EditValue Is Nothing Then Return False

		For i As Integer = esBegin.GetValueOrDefault(Now).Day To esEnd.GetValueOrDefault(Now).Day
			Dim cViewData = New ZVHourAbsenceViewData() With {
					.WorkingDay = New DateTime(lueYear.EditValue, lueMonth.EditValue, i),
					.WorkingHour = foundedData.GetWorkingHoursOfDay(i),
					.AbsenceCode = foundedData.GetAbsenceDayCodeOfDay(i)
			}

			listDataSource.Add(cViewData)

		Next
		grdTotalHours.DataSource = listDataSource


		Return Not listDataSource Is Nothing

	End Function

	Private Function LoadEmployeeMonthHourData(ByVal searchKind As EmployeeSuvaSearchData) As ZVHourAbsenceData
		m_HourAbsencedata = m_EmployeeDatabaseAccess.GetEmployeeMonthHoursAndAbsenceData(lueMandant.EditValue, m_EmployeeNumber, 0, lueYear.EditValue, lueMonth.EditValue)

		If (m_HourAbsencedata Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten-Rapportstunden Daten konnten nicht geladen werden."))
		End If

		Return m_HourAbsencedata
	End Function

	Private Sub DisplayESData()
		Dim amount_100 As Decimal = 0
		Dim percent_500 As Decimal = 0
		Dim percent_600 As Decimal = 0
		Dim percent_700 As Decimal = 0
		Dim amount_StdLohn As Decimal = 0
		Dim amount_500 As Decimal = 0
		Dim amount_600 As Decimal = 0
		Dim amount_700 As Decimal = 0
		Dim amount_5000 As Decimal = 0

		Dim unDefined As Boolean?
		Dim esEnd As Date?
		Dim esBegin As Date?
		Dim dStartofMonth As Date
		Dim dEndofMonth As Date

		dStartofMonth = CDate("01." & lueMonth.EditValue & "." & lueYear.EditValue)
		dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth.AddDays(-dStartofMonth.Day + 1))).AddDays(-1)
		If m_ESData Is Nothing OrElse m_ESData.Count = 0 Then Return

		For Each rec In m_ESData
			If esBegin Is Nothing Then esBegin = rec.ES_Ab
			If rec.ES_Ende Is Nothing Then
				If unDefined Is Nothing Then unDefined = True
			Else
				If Not unDefined.GetValueOrDefault(False) Then esEnd = rec.ES_Ende
			End If

			amount_100 += rec.GrundLohn.GetValueOrDefault(0)
			percent_500 += rec.FeierProz.GetValueOrDefault(0)
			amount_500 += rec.Feier.GetValueOrDefault(0)
			percent_600 += rec.FerienProz.GetValueOrDefault(0)
			amount_600 += rec.Ferien.GetValueOrDefault(0)
			percent_700 += rec.Lohn13Proz.GetValueOrDefault(0)
			amount_700 += rec.Lohn13.GetValueOrDefault(0)
			amount_5000 += rec.MAStdSpesen.GetValueOrDefault(0)

			amount_StdLohn += rec.StundenLohn.GetValueOrDefault(0)

		Next

		m_Percent_500 = percent_500 / m_ESData.Count
		m_Percent_600 = percent_600 / m_ESData.Count
		m_Percent_700 = percent_700 / m_ESData.Count
		lbl10_3.Text = String.Format(m_Translate.GetSafeTranslationValue("Feiertagsentschädigung ({0:n2} %)"), m_Percent_500)
		lbl10_5.Text = String.Format(m_Translate.GetSafeTranslationValue("Ferienentschädigung ({0:n2} %)"), m_Percent_600)
		lbl10_7.Text = String.Format(m_Translate.GetSafeTranslationValue("13. Monatslohn ({0:n2} %)"), m_Percent_700)

		txt8_1.EditValue = String.Format("{0:n2}", amount_StdLohn / m_ESData.Count)

		txt10_1.EditValue = String.Format("{0:n2}", amount_100 / m_ESData.Count)
		txt10_2.EditValue = String.Format("{0:n2}", amount_500 / m_ESData.Count)
		txt10_3.EditValue = String.Format("{0:n2}", amount_600 / m_ESData.Count)
		txt10_4.EditValue = String.Format("{0:n2}", amount_700 / m_ESData.Count)

		If amount_5000 > 0 Then
			txt10_5.EditValue = m_Translate.GetSafeTranslationValue("Lohnspesen")
			txt10_6.EditValue = String.Format("{0:n2}", amount_5000 / m_ESData.Count)
		End If

		If esEnd > dEndofMonth Then
			op15.EditValue = 1
			txt15_1.EditValue = String.Format("{0:d}", esEnd)
		Else
			If unDefined.HasValue Then
				If unDefined Then
					op15.EditValue = 0
				Else
					op15.EditValue = 1
				End If
			End If
		End If

		Dim dismissalData = m_ESData.Where(Function(s) s.dismissalon IsNot Nothing) ' order by es_ab desc)
		If dismissalData Is Nothing OrElse dismissalData.Count = 0 Then Return
		dismissalData = dismissalData.OrderBy(Function(s) s.ES_Ende)

		chk15.Checked = True
		txt15_2.EditValue = String.Format("{0}", dismissalData(dismissalData.Count - 1).dismissalwho)
		txt15_3.EditValue = String.Format("{0:d}", dismissalData(dismissalData.Count - 1).dismissalon)
		txt15_4.EditValue = String.Format("{0:d}", dismissalData(dismissalData.Count - 1).dismissalfor)
		txt16.EditValue = String.Format("{0}", dismissalData(dismissalData.Count - 1).dismissalreason)

	End Sub

	Private Sub DisplayPayrollData()
		Dim anz_101 As Decimal = 0
		Dim anz_290 As Decimal = 0
		Dim amount_530 As Decimal = 0
		Dim amount_560 As Decimal = 0
		Dim amount_630 As Decimal = 0
		Dim amount_660 As Decimal = 0
		Dim amount_730 As Decimal = 0
		Dim amount_760 As Decimal = 0
		Dim amount_800 As Decimal = 0
		Dim amount_1000 As Decimal = 0

		Dim amount_2100 As Decimal = 0
		Dim amount_2500 As Decimal = 0

		Dim anz_KI As Decimal = 0
		Dim Bas_KI As Decimal = 0
		Dim amount_KI As Decimal = 0
		Dim anz_AU As Decimal = 0
		Dim Bas_AU As Decimal = 0
		Dim amount_AU As Decimal = 0

		Dim amount_7100 As Decimal = 0
		Dim amount_BVG As Decimal = 0
		Dim amount_Div As Decimal = 0
		Dim label_Div As String = String.Empty

		Dim foundedData = LoadEmployeeMonthPayrollData(m_SearchCriteriums)
		If foundedData Is Nothing Then Return

		For Each la In foundedData
			Select Case la.LANr
				Case 101
					anz_101 += la.TotalAnzahl
				Case 290
					anz_290 += la.TotalBetrag

				Case 530
					amount_530 += If(la.Bruttopflichtig AndAlso la.AHVpflichtig, la.TotalBetrag, 0)
				Case 560
					amount_560 += If(la.Bruttopflichtig AndAlso la.AHVpflichtig, la.TotalBetrag, 0)

				Case 630
					amount_630 += If(la.Bruttopflichtig AndAlso la.AHVpflichtig, la.TotalBetrag, 0)
				Case 660
					amount_660 += If(la.Bruttopflichtig AndAlso la.AHVpflichtig, la.TotalBetrag, 0)

				Case 730
					amount_730 += If(la.Bruttopflichtig AndAlso la.AHVpflichtig, la.TotalBetrag, 0)
				Case 760
					amount_760 += If(la.Bruttopflichtig AndAlso la.AHVpflichtig, la.TotalBetrag, 0)

				Case 1000
					amount_1000 += la.TotalBetrag
				Case 2100, 2300
					amount_2100 += la.TotalBetrag
				Case 2500
					amount_2500 += la.TotalBetrag

				Case 3600, 3602, 3650, 3700
					anz_KI += la.TotalAnzahl
					Bas_KI += la.TotalBasis
					amount_KI += la.TotalBetrag

				Case 3750, 3800, 3850
					anz_AU += la.TotalAnzahl
					Bas_AU += la.TotalBasis
					amount_AU += la.TotalBetrag

				Case 7100
					amount_7100 += la.TotalBetrag

				Case 7590, 7592, 7596
					amount_BVG += la.TotalBetrag

				Case Else
					amount_Div += la.TotalBetrag
					label_Div += If(String.IsNullOrWhiteSpace(la.RPText), String.Empty, String.Format(";{0}", la.RPText))

			End Select
		Next
		If Not m_HourAbsencedata Is Nothing Then
			lbltotalstdmonthValue.Text = String.Format("{0:n2}", m_HourAbsencedata.TotalAmountOfHours)
		End If

		lblAuszahlungGleitValue.Text = String.Format("{0:n2}", amount_800)
		lblNachtzeitValue.Text = String.Format("{0:n2}", anz_101)
		lblAuszahlungNachtzulageValue.Text = String.Format("{0:n2}", anz_290)

		txt8_2.EditValue = String.Format("{0:n2}", amount_1000)
		txt9.EditValue = String.Format("{0:n2}", amount_7100)

		op12.EditValue = If(amount_BVG <> 0, 0, 1)
		If amount_BVG <> 0 Then
			txtBVGfund.EditValue = BVGfundSetting
		End If

		txt14_1.EditValue = String.Format("{0:n2}", anz_KI)
		txt14_2.EditValue = String.Format("{0:n2}", amount_KI)
		txt14_3.EditValue = String.Format("{0:n2}", amount_2500)

		txt14_4.EditValue = String.Format("{0:n2}", anz_AU)
		txt14_5.EditValue = String.Format("{0:n2}", amount_AU)



	End Sub

	Private Function LoadEmployeeMonthPayrollData(ByVal searchKind As EmployeeSuvaSearchData) As IEnumerable(Of ZVPayrollData)
		m_Payrolldata = m_EmployeeDatabaseAccess.LoadEmployeePayrollData(lueMandant.EditValue, m_EmployeeNumber, lueYear.EditValue, lueMonth.EditValue)

		If (m_Payrolldata Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht geladen werden."))
			Return Nothing
		End If


		Return m_Payrolldata
	End Function


	''' <summary>
	''' Handles edit change event of lueYear.
	''' </summary>
	Private Sub OnLueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged

		If m_SuppressUIEvents OrElse lueYear.EditValue Is Nothing Then
			Return
		End If

		LoadMonthDropDownData()

	End Sub

	''' <summary>
	''' Handles edit change event of lueMonth.
	''' </summary>
	Private Sub OnLueMonth_EditValueChanged(sender As Object, e As EventArgs) Handles lueMonth.EditValueChanged

		If m_SuppressUIEvents OrElse lueMonth.EditValue Is Nothing Then
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

	Private Sub bbiSearch_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSearch.ItemClick
		Search()
	End Sub

	Private Function Search() As Boolean
		Dim result As Boolean = True
		If lueYear.EditValue Is Nothing OrElse lueMonth.EditValue Is Nothing Then Return False

		m_SearchCriteriums = New EmployeeSuvaSearchData With {.MDNr = lueMandant.EditValue, .EmployeeNumbers = New List(Of Integer)({m_EmployeeNumber}), .Jahr = lueYear.EditValue, .Monat = lueMonth.EditValue}
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, lueYear.EditValue))

		m_Logger.LogInfo(String.Format("ZV-Search is started ->>> employee: {0} | year: {1} | month: {2}", m_EmployeeNumber, lueYear.EditValue, lueMonth.EditValue))

		ResetAllTabs()
		DisplayEmployeeData()

		result = result AndAlso LoadESOverviewData()
		'DisplayESData()

		If Not m_ESData Is Nothing AndAlso m_ESData.Count > 0 Then
			DisplayESData()
			result = result AndAlso CreateEmployeeHourData()
		End If

		DisplayPayrollData()
		CreatePrintPopupMenu()
		bbiClear.Enabled = True
		bbiPrint.Enabled = Not m_ESData Is Nothing AndAlso m_ESData.Count > 0
		bbiSendEMail.Enabled = Not m_ESData Is Nothing AndAlso m_ESData.Count > 0

		Return result

	End Function

	Private Sub OngvTotalHours_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvTotalHours.CustomColumnDisplayText

		If e.Column.FieldName = "WorkingHour" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub bbiClear_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiClear.ItemClick
		ResetAllTabs()
	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl
		CheckValidityValues()

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
	End Sub

	Private Sub OnbbiSendEMail_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSendEMail.ItemClick
		m_SelectedWOSEnun = WOSZVSENDValue.PrintWithoutSending
		m_ExportPrintInFiles = True
		CheckValidityValues()

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
					m_ExportPrintInFiles = False

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

	Private Sub CheckValidityValues()

		If chk15.Checked Then
			op15.EditValue = Nothing
		End If

	End Sub

	Sub StartPrinting()
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim _setting As New ZVPrintData With {.frmhwnd = Me.Handle, .PrintJobNumber = "1.5.1", .ShowAsDesign = ShowDesign}
		m_PDFFilesToPrint.Clear()

		_setting.m_EmployeeAddressData = m_EmployeeAddressData
		_setting.m_EmployeeContactCommData = m_EmployeeContactCommData
		_setting.m_EmployeeData = m_EmployeeData
		_setting.m_ESData = m_ESData
		_setting.m_HourAbsencedata = m_HourAbsencedata
		_setting.m_Payrolldata = m_Payrolldata

		_setting.WOSSendValueEnum = m_SelectedWOSEnun
		_setting.ExportPrintInFiles = If(m_SelectedWOSEnun = WOSZVSENDValue.PrintOtherSendWOS, False, chkPrintESVertrag.Checked OrElse chkPrintESVertrag.Checked) ' m_ExportPrintInFiles

		Dim pagedata = LoadZVPageData()

		Dim printUtil = New PrintEmployeeData(m_InitializationData)
		printUtil.PrintData = _setting
		printUtil.PageData = pagedata

		Dim result = printUtil.PrintEmployeeZVData()

		printUtil.Dispose()

		If Not ShowDesign AndAlso result.Printresult AndAlso result.WOSresult AndAlso Not m_SelectedWOSEnun = WOSSENDValue.PrintWithoutSending Then
			Dim msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")

			m_UtilityUI.ShowInfoDialog(msg)

			Return
		ElseIf Not result.Printresult Then
			If Not String.IsNullOrWhiteSpace(result.PrintresultMessage) Then m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)

		Else

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
				Dim newFilename As String = Path.Combine(Path.GetDirectoryName(m_PDFFilesToPrint(0)), String.Format("{0}", "Zwischenverdienst-Unterlagen.PDF"))
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
		Dim _setting As New ZVPrintData With {.frmhwnd = Me.Handle, .PrintJobNumber = "1.5.1", .ShowAsDesign = False}
		Dim success As Boolean = True
		Dim pdfFilesToSend As New List(Of String)
		m_PDFFilesToPrint.Clear()

		_setting.m_EmployeeAddressData = m_EmployeeAddressData
		_setting.m_EmployeeContactCommData = m_EmployeeContactCommData
		_setting.m_EmployeeData = m_EmployeeData
		_setting.m_ESData = m_ESData
		_setting.m_HourAbsencedata = m_HourAbsencedata
		_setting.m_Payrolldata = m_Payrolldata

		_setting.ExportPrintInFiles = m_ExportPrintInFiles
		_setting.WOSSendValueEnum = m_SelectedWOSEnun

		Dim pagedata = LoadZVPageData()

		Dim printUtil = New PrintEmployeeData(m_InitializationData)
		printUtil.PrintData = _setting
		printUtil.PageData = pagedata

		Dim result = printUtil.PrintEmployeeZVData()

		printUtil.Dispose()


		If Not result.Printresult Then
			m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)
			Return

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
					Dim newFilename As String = Path.Combine(Path.GetDirectoryName(m_PDFFilesToPrint(0)), String.Format("{0}", "Zwischenverdienst-Unterlagen.PDF"))

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

			Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.ZV, .EmployeeNumber = m_EmployeeNumber, .PDFFilesToSend = pdfFilesToSend}
			frmMail.PreselectionData = preselectionSetting

			frmMail.LoadData()

			frmMail.Show()
			frmMail.BringToFront()

		End If

	End Sub

	Private Function LoadZVPageData() As ZVPageData
		Dim result As ZVPageData = Nothing

		Dim pageData As New ZVPageData

		pageData.Monat = lueMonth.EditValue
		pageData.Jahr = lueYear.EditValue

		pageData.lblAuszahlungGleitValue = CType(CDbl(lblAuszahlungGleitValue.Text), Decimal)
		pageData.lblAuszahlungNachtzulageValue = CType(CDbl(lblAuszahlungNachtzulageValue.Text), Decimal)
		pageData.lblNachtzeitValue = CType(CDbl(lblNachtzeitValue.Text), Decimal)

		pageData.op6 = op6.EditValue
		pageData.txt6_1 = CType(CDbl(txt6_1.EditValue), Decimal)
		pageData.txt6_2 = CType(CDbl(txt6_2.EditValue), Decimal)
		pageData.txt6_3 = CType(CDbl(txt6_3.EditValue), Decimal)
		pageData.txt7 = txt7.EditValue

		pageData.txt8_1 = CType(CDbl(txt8_1.EditValue), Decimal)
		pageData.txt8_2 = CType(CDbl(txt8_2.EditValue), Decimal)
		pageData.txt9 = CType(CDbl(txt9.EditValue), Decimal)

		pageData.txt10_1 = CType(CDbl(txt10_1.EditValue), Decimal)
		pageData.m_Percent_500 = CType(CDbl(m_Percent_500), Decimal)
		pageData.m_Percent_600 = CType(CDbl(m_Percent_600), Decimal)
		pageData.m_Percent_700 = CType(CDbl(m_Percent_700), Decimal)
		pageData.txt10_2 = CType(CDbl(txt10_2.EditValue), Decimal)
		pageData.txt10_3 = CType(CDbl(txt10_3.EditValue), Decimal)
		pageData.txt10_4 = CType(CDbl(txt10_4.EditValue), Decimal)
		pageData.txt10_5 = txt10_5.EditValue
		pageData.txt10_6 = CType(CDbl(txt10_6.EditValue), Decimal)

		pageData.chk11_1 = chk11_1.CheckState
		pageData.chk11_2 = chk11_2.CheckState
		pageData.txt11_1 = txt11_1.EditValue
		pageData.txt11_2 = CType(CDbl(txt11_2.EditValue), Decimal)

		pageData.op12 = op12.EditValue
		pageData.txt12 = txtBVGfund.EditValue
		pageData.Compensationfund = txtCompensationfund.EditValue
		pageData.CompanyBURNumber = BURNumberSetting

		pageData.txt14_1 = CType(CDbl(txt14_1.EditValue), Decimal)
		pageData.txt14_2 = CType(CDbl(txt14_2.EditValue), Decimal)
		pageData.txt14_3 = CType(CDbl(txt14_3.EditValue), Decimal)
		pageData.txt14_4 = CType(CDbl(txt14_4.EditValue), Decimal)
		pageData.txt14_5 = CType(CDbl(txt14_5.EditValue), Decimal)
		pageData.txt14_6 = txt14_6.EditValue

		pageData.op15 = op15.EditValue
		pageData.txt15_1 = txt15_1.EditValue
		pageData.chk15 = chk15.CheckState
		pageData.txt15_2 = txt15_2.EditValue
		pageData.txt15_3 = txt15_3.EditValue
		pageData.txt15_4 = txt15_4.EditValue
		pageData.txt16 = txt16.EditValue
		pageData.op17 = op17.EditValue

		pageData.txt18_1 = CType(CDbl(txt18_1.EditValue), Decimal)
		pageData.txt18_2 = CType(CDbl(txt18_2.EditValue), Decimal)
		pageData.txt18_3 = CType(CDbl(txt18_3.EditValue), Decimal)
		pageData.txt18_4 = CType(CDbl(txt18_4.EditValue), Decimal)
		pageData.txt18_5 = CType(CDbl(txt18_5.EditValue), Decimal)


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
		If m_Payrolldata Is Nothing Then Return False

		For Each ES In m_Payrolldata
			Dim number As Integer = ES.LONr.GetValueOrDefault(0)
			Dim data = m_EmployeeDatabaseAccess.LoadEmployeeDocumentForZVData(m_EmployeeNumber, number, 212)
			If data.ID = 0 Then
				data = m_EmployeeDatabaseAccess.LoadEmployeePrintedDocumentForZVData(m_EmployeeNumber, lueMonth.EditValue, lueYear.EditValue, 212)
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

			Exit For
		Next


		Return result

	End Function

	Private Function MergeAndPrintExtraDocuments(ByVal printJob As Boolean?) As Boolean
		Dim result As Boolean = True

		If m_PDFFilesToPrint.Count = 0 Then Return False
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



		'Dim fileName As String = m_PDFFilesToPrint(0)
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
			m_UtilityUI.ShowErrorDialog(String.Format("Möglicherweise sind die Anhänge nicht in PDF-Format.<br>{0}", ex.ToString))
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


Public Class PreselectionZVData
	Public Property MDNr As Integer
	Public Property Year As Integer?
	Public Property Month As Integer?
	Public Property EmployeeNumber As Integer?

End Class
