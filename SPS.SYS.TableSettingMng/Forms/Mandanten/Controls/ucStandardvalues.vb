
Imports System.IO
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.LookAndFeel

Imports SP.DatabaseAccess

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData


Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Initialization
Imports System.Xml
Imports SP.Internal.Automations.BaseTable
Imports System.ComponentModel

Public Class ucStandardvalues


#Region "private consts"

	Private Const MANDANT_XML_SETTING_WOS_VACANCY_GUID As String = "MD_{0}/Export/Vak_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_VER_GUID As String = "MD_{0}/Export/Ver_SPUser_ID"

	Private Const MANDANT_XML_SETTING_COCKPIT_EMAIL_TEMPLATE As String = "MD_{0}/Templates/cockpit-email-template"
	Private Const MANDANT_XML_SETTING_COCKPIT_URL As String = "MD_{0}/Templates/cockpit-url"
	Private Const MANDANT_XML_SETTING_COCKPIT_PICTURE As String = "MD_{0}/Templates/cockpit-picture"


	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"

	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"
	Private Const FORM_XML_PRINT_PAYROLL_KEY As String = "Forms_Normaly/Lohnbuchhaltung"

	Private Const FORM_XML_REQUIREDFIELDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As MandantData

	Private m_SuppressUIEvents As Boolean

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

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFile As String
	Private m_MandantUserXMLFile As String

	Private m_MandantSetting As String
	Private m_PayrollSetting As String
	Private m_InvoiceSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath
	Private m_Year As Integer

	Private m_BaseTableUtil As SPSBaseTables

#End Region



#Region "public property"

	Public Property IsDataValid As Boolean


#End Region


#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = ClsDataDetail.m_InitialData

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub
		m_Year = m_InitializationData.MDData.MDYear
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_BaseTableUtil = New SPSBaseTables(m_InitializationData)

		Me.xtabDefaultValue.SelectedTabPage = xtabEmplyeeAutofield
		Me.xtabDefaultValueEmployee.SelectedTabPage = xtabEmployeeRueckstellung
		Me.xtabDefaultValueES.SelectedTabPage = xtabDefaultESAllgemein

		'Dim printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters
		'For Each p In printers
		'	chkUserMatrixPrinter.Properties.Items.Add(p.ToString)
		'Next

		XtraTabControl1.SelectedTabPage = xtabMwStData
		XtraTabPage1.PageVisible = False

		Reset()

	End Sub


#End Region


	''' <summary>
	''' Inits the control with configuration information.
	''' </summary>
	'''<param name="initializationClass">The initialization class.</param>
	'''<param name="translationHelper">The translation helper.</param>
	Public Overridable Sub InitWithConfigurationData(ByVal initializationClass As InitializeClass, ByVal translationHelper As TranslateValuesHelper, _Year As Integer)

		m_InitializationData = initializationClass
		m_Translate = translationHelper
		m_Year = _Year
		IsDataValid = True

		Try
			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
			m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
			If Not System.IO.File.Exists(m_MandantFormXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantFormXMLFile))
				IsDataValid = False
				Return
			End If
			m_MandantUserXMLFile = m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
			If Not System.IO.File.Exists(m_MandantUserXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantUserXMLFile))
				IsDataValid = False
				Return
			End If

			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
			m_InvoiceSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING, m_InitializationData.MDData.MDNr)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			Reset()
			TranslateControls()

			Me.xtabDefaultValue.SelectedTabPage = xtabEmplyeeAutofield
			Me.xtabDefaultValueEmployee.SelectedTabPage = xtabEmployeeRueckstellung
			Me.xtabDefaultValueES.SelectedTabPage = xtabDefaultESAllgemein


		Catch ex As Exception
			IsDataValid = False

		End Try

	End Sub

	Public Function LoadSettingData() As Boolean
		Dim success As Boolean = True

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				IsDataValid = False
				Return False
			End If
			success = success And LoadDropDownData()

			success = success And LoadDefaultvaluesEmployee()
			success = success And LoadSonstigeEinstellung()

			success = success And LoadDefaultvaluesCustomer()
			success = success And LoadDefaultvaluesEinsatz()

			success = success And LoadPayrollSetting()

			success = success And LoadDefaultvaluesReportsAdvancPaymentPayroll()

			success = success And LoadDefaultvaluesInvoice()


		Catch ex As Exception
			IsDataValid = False

		Finally

		End Try

		Return success

	End Function

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

	End Sub

	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		ResetPermissionDropDown()

		ResetCodeDropDown(lueTaxMarriedCode)
		ResetCodeDropDown(lueTaxSingleCode)
		ResetChurchTaxDropDown(lueChurchTaxMarried)
		ResetChurchTaxDropDown(lueChurchTaxSingle)
		ResetNumberOfChildrenDropDown(lueNumberOfChildrenMarried)
		ResetNumberOfChildrenDropDown(lueNumberOfChildrenSingle)

		ResetTransactionDataDropDown()

		ResetBankChargesDropDown()


		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetTransactionDataDropDown()
		lueTransactionType.Properties.DisplayMember = "Description"

		lueTransactionType.Properties.Columns.Clear()
		lueTransactionType.Properties.Columns.Add(New LookUpColumnInfo("Description", 0))
		lueTransactionType.EditValue = Nothing
	End Sub

	Private Sub ResetBankChargesDropDown()
		lueBankCharges.Properties.DisplayMember = "Description"

		lueBankCharges.Properties.Columns.Clear()
		lueBankCharges.Properties.Columns.Add(New LookUpColumnInfo("Description", 0))
		lueBankCharges.EditValue = Nothing
	End Sub

	Private Sub ResetPermissionDropDown()

		luePermission.Properties.DisplayMember = "Translated_Value"
		luePermission.Properties.ValueMember = "Rec_Value"

		Dim columns = luePermission.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Rec_Value", m_Translate.GetSafeTranslationValue("Code"), 100))
		columns.Add(New LookUpColumnInfo("Translated_Value", m_Translate.GetSafeTranslationValue("Kategorie"), 500))

		luePermission.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePermission.Properties.SearchMode = SearchMode.AutoComplete
		luePermission.Properties.AutoSearchColumnIndex = 1
		luePermission.Properties.NullText = String.Empty

		luePermission.Properties.PopupWidth = 700
		luePermission.Properties.PopupSizeable = True

		luePermission.EditValue = Nothing
	End Sub

	Private Sub ResetCodeDropDown(ByVal lookupEdit As DevExpress.XtraEditors.LookUpEdit)

		lookupEdit.Properties.DisplayMember = "Translated_Value"
		lookupEdit.Properties.ValueMember = "Rec_Value"

		Dim columns = lookupEdit.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Rec_Value", 0, m_Translate.GetSafeTranslationValue("Code")))
		columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lookupEdit.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lookupEdit.Properties.SearchMode = SearchMode.AutoComplete
		lookupEdit.Properties.AutoSearchColumnIndex = 1
		lookupEdit.Properties.NullText = String.Empty
		lookupEdit.EditValue = Nothing

	End Sub

	Private Sub ResetChurchTaxDropDown(ByVal lookupEdit As DevExpress.XtraEditors.LookUpEdit)

		lookupEdit.Properties.DisplayMember = "Translated_Value"
		lookupEdit.Properties.ValueMember = "Rec_Value"

		Dim columns = lookupEdit.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Rec_Value", 0, m_Translate.GetSafeTranslationValue("Code")))
		columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lookupEdit.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lookupEdit.Properties.SearchMode = SearchMode.AutoComplete
		lookupEdit.Properties.AutoSearchColumnIndex = 1
		lookupEdit.Properties.NullText = String.Empty
		lookupEdit.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the number of children drop down.
	''' </summary>
	Private Sub ResetNumberOfChildrenDropDown(ByVal lookupEdit As DevExpress.XtraEditors.LookUpEdit)

		lookupEdit.Properties.DisplayMember = "NumberOfChildren"
		lookupEdit.Properties.ValueMember = "NumberOfChildren"

		Dim columns = lookupEdit.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("NumberOfChildren", 0, m_Translate.GetSafeTranslationValue("Anzahl Kinder")))

		lookupEdit.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lookupEdit.Properties.SearchMode = SearchMode.AutoComplete
		lookupEdit.Properties.AutoSearchColumnIndex = 0
		lookupEdit.Properties.NullText = String.Empty
		lookupEdit.EditValue = Nothing

	End Sub

	Private Function LoadDropDownData() As Boolean
		Dim result As Boolean = True

		result = result AndAlso LoadPermissionDropDownData()
		result = result AndAlso LoadTaxCodeDropDownData()
		result = result AndAlso LoadTaxChurchCodeDropDownData()
		result = result AndAlso LoadTaxNumberOfChildrenMarriedDropDownData()
		result = result AndAlso LoadTaxNumberOfChildrenSingleDropDownData()

		Return result
	End Function

	Private Function LoadPermissionDropDownData() As Boolean
		Dim language = m_InitializationData.UserData.UserLanguage
		Dim searchResult = m_BaseTableUtil.PerformPermissionDataOverWebService(language)

		If (searchResult Is Nothing) Then
			m_Logger.LogWarning("no permission data was founded!")

			Return False
		End If

		Dim supressUIState = m_SuppressUIEvents
		m_SuppressUIEvents = True


		luePermission.Properties.DataSource = searchResult
		luePermission.Properties.ForceInitialize()

		If Not searchResult Is Nothing Then luePermission.Properties.DropDownRows = searchResult.Count + 1

		m_SuppressUIEvents = supressUIState

		Return Not searchResult Is Nothing
	End Function

	Private Function LoadTaxCodeDropDownData() As Boolean
		Dim language = m_InitializationData.UserData.UserLanguage
		Dim searchResult = m_BaseTableUtil.PerformTaxCodeDataOverWebService(m_InitializationData.UserData.UserLanguage)

		If (searchResult Is Nothing) Then
			m_Logger.LogWarning("no permission data was founded!")

			Return False
		End If

		Dim supressUIState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		lueTaxMarriedCode.Properties.DataSource = searchResult
		lueTaxSingleCode.Properties.DataSource = searchResult

		lueTaxMarriedCode.Properties.ForceInitialize()
		lueTaxSingleCode.Properties.ForceInitialize()

		If Not searchResult Is Nothing Then
			lueTaxMarriedCode.Properties.DropDownRows = searchResult.Count + 1
			lueTaxSingleCode.Properties.DropDownRows = searchResult.Count + 1
		End If

		m_SuppressUIEvents = supressUIState

		Return Not searchResult Is Nothing
	End Function

	Private Function LoadTaxChurchCodeDropDownData() As Boolean
		Dim language = m_InitializationData.UserData.UserLanguage
		Dim searchResult = m_BaseTableUtil.PerformTaxChurchCodeDataOverWebService(m_InitializationData.UserData.UserLanguage)
		'Dim searchResult = m_BaseTableUtil.PerformTaxInfoDataOverWebService("AG", Now.Year, m_InitializationData.UserData.UserLanguage)

		If (searchResult Is Nothing) Then
			m_Logger.LogWarning("no church data was founded!")

			Return False
		End If

		Dim supressUIState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		lueChurchTaxMarried.Properties.DataSource = searchResult
		lueChurchTaxSingle.Properties.DataSource = searchResult

		lueTaxMarriedCode.Properties.ForceInitialize()
		lueTaxSingleCode.Properties.ForceInitialize()

		If Not searchResult Is Nothing Then
			lueChurchTaxMarried.Properties.DropDownRows = searchResult.Count + 1
			lueChurchTaxSingle.Properties.DropDownRows = searchResult.Count + 1
		End If

		m_SuppressUIEvents = supressUIState

		Return Not searchResult Is Nothing
	End Function

	Private Function LoadTaxNumberOfChildrenMarriedDropDownData() As Boolean
		Dim language = m_InitializationData.UserData.UserLanguage
		Dim code As String = String.Format("{0}", lueTaxMarriedCode.EditValue)
		Dim church As String = String.Format("{0}", lueChurchTaxMarried.EditValue)

		If String.IsNullOrWhiteSpace(code) Then code = "B"
		If String.IsNullOrWhiteSpace(church) Then church = "Y"

		Dim searchResult = m_BaseTableUtil.PerformTaxNumberOfChildernDataOverWebService(m_InitializationData.UserData.UserLanguage, m_InitializationData.MDData.MDCanton, code, church)
		If (searchResult Is Nothing) Then
			m_Logger.LogWarning("no church data was founded!")

			Return False
		End If
		Dim listData = New BindingList(Of NumberOfChildrenViewData)

		For Each itm In searchResult
			Dim data = New NumberOfChildrenViewData

			data.NumberOfChildren = itm

			listData.Add(data)
		Next


		Dim supressUIState = m_SuppressUIEvents
		m_SuppressUIEvents = True


		lueNumberOfChildrenMarried.Properties.DataSource = listData
		lueNumberOfChildrenMarried.Properties.ForceInitialize()

		If Not listData Is Nothing Then lueNumberOfChildrenMarried.Properties.DropDownRows = listData.Count + 1


		m_SuppressUIEvents = supressUIState

		Return Not searchResult Is Nothing
	End Function

	Private Function LoadTaxNumberOfChildrenSingleDropDownData() As Boolean
		Dim language = m_InitializationData.UserData.UserLanguage
		Dim code As String = String.Format("{0}", lueTaxSingleCode.EditValue)
		Dim church As String = String.Format("{0}", lueChurchTaxSingle.EditValue)

		If String.IsNullOrWhiteSpace(code) Then code = "A"
		If String.IsNullOrWhiteSpace(church) Then church = "Y"

		Dim searchResult = m_BaseTableUtil.PerformTaxNumberOfChildernDataOverWebService(m_InitializationData.UserData.UserLanguage, m_InitializationData.MDData.MDCanton, code, church)
		If (searchResult Is Nothing) Then
			m_Logger.LogWarning("no church data was founded!")

			Return False
		End If
		Dim listData = New BindingList(Of NumberOfChildrenViewData)

		For Each itm In searchResult
			Dim data = New NumberOfChildrenViewData

			data.NumberOfChildren = itm

			listData.Add(data)
		Next

		Dim supressUIState = m_SuppressUIEvents
		m_SuppressUIEvents = True


		lueNumberOfChildrenSingle.Properties.DataSource = listData
		lueNumberOfChildrenSingle.Properties.ForceInitialize()

		If Not listData Is Nothing Then lueNumberOfChildrenSingle.Properties.DropDownRows = listData.Count + 1


		m_SuppressUIEvents = supressUIState

		Return Not searchResult Is Nothing
	End Function

	Private Function LoadDefaultvaluesEmployee() As Boolean
		Dim success As Boolean = True

		Try
			' Default Values common
			Dim currencyvalue As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/currencyvalue", FORM_XML_DEFAULTVALUES_KEY))
			Dim mainlanguagevalue As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/mainlanguagevalue", FORM_XML_DEFAULTVALUES_KEY))

			' Data...
			Me.cboCurrencyvalue.EditValue = If(currencyvalue = String.Empty, "CHF", currencyvalue)
			Me.cbomainlanguagevalue.EditValue = If(mainlanguagevalue = String.Empty, "deutsch", mainlanguagevalue)


			' Employee
			Dim employeezahlart As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeezahlart", FORM_XML_DEFAULTVALUES_KEY))
			Dim employeebvgcode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeebvgcode", FORM_XML_DEFAULTVALUES_KEY))
			Dim employeebvgcodewithchild As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeebvgcodewithchild", FORM_XML_DEFAULTVALUES_KEY))
			Dim employeerahmenarbeitsvertrag As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeerahmenarbeitsvertrag", FORM_XML_DEFAULTVALUES_KEY)), True)
			Dim employeeferienback As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeeferienback", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim employeefeiertagback As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeefeiertagback", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim employee13lohnback As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employee13lohnback", FORM_XML_DEFAULTVALUES_KEY)), False)

			Dim employeenoes As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenoes", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim employeenolo As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenolo", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim employeenozg As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenozg", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim employeedes As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeedes", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim childerlacantonsameastaxcanton As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/childerlacantonsameastaxcanton", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim donotaskforcreatingemployeecontactinpropose As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/donotaskforcreatingemployeecontactinpropose", FORM_XML_DEFAULTVALUES_KEY)), False)

			Dim permissioncodeonnotswiss As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/permissioncodeonnotswiss", FORM_XML_DEFAULTVALUES_KEY))
			Dim taxcodeforcivilstatemarriedperson As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/taxcodeforcivilstatemarriedperson", FORM_XML_DEFAULTVALUES_KEY))
			Dim taxcodeforcivilstatesingleperson As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/taxcodeforcivilstatesingleperson", FORM_XML_DEFAULTVALUES_KEY))

			Dim employeesstate As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeesstate", FORM_XML_DEFAULTVALUES_KEY))
			Dim emplyoeefstate As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeefstate", FORM_XML_DEFAULTVALUES_KEY))
			Dim employeenationality As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenationality", FORM_XML_DEFAULTVALUES_KEY))
			Dim employeecountryqualification As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeecountryqualification", FORM_XML_DEFAULTVALUES_KEY))
			Dim employeecontact As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeecontact", FORM_XML_DEFAULTVALUES_KEY))
			Dim searchforchangingapplicanttoemployee As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/searchforchangingapplicanttoemployee", FORM_XML_DEFAULTVALUES_KEY))


			Dim allowedflextimeinreports As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/allowedflextimeinreports", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim getflextimefrommandantdatabase As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/getflextimefrommandantdatabase", FORM_XML_DEFAULTVALUES_KEY)), False)

			' Data...
			Me.cboemployeezahlart.Text = If(String.IsNullOrWhiteSpace(employeezahlart), "K", employeezahlart)
			Me.cboemployeebvgcode.Text = If(String.IsNullOrWhiteSpace(employeebvgcode), "9", employeebvgcode)
			Me.cboemployeebvgcodewithchild.Text = If(String.IsNullOrWhiteSpace(employeebvgcodewithchild), "1", employeebvgcodewithchild)

			Me.cboemployeecontact.Text = employeecontact
			Me.cboemployeesstate.Text = employeesstate
			Me.cboemplyoeefstate.Text = emplyoeefstate

			Me.cboemployeenationality.Text = employeenationality
			Me.cboemployeecountryqualification.Text = employeecountryqualification

			luePermission.EditValue = permissioncodeonnotswiss

			If Not String.IsNullOrWhiteSpace(taxcodeforcivilstatemarriedperson) AndAlso taxcodeforcivilstatemarriedperson.Length = 3 Then
				Dim code = taxcodeforcivilstatemarriedperson.Substring(0, 1)
				Dim numberOfChildrenMarried As Integer = taxcodeforcivilstatemarriedperson.Substring(1, 1)
				Dim church = taxcodeforcivilstatemarriedperson.Substring(2, 1)

				lueTaxMarriedCode.EditValue = code
				lueChurchTaxMarried.EditValue = church
				lueNumberOfChildrenMarried.EditValue = numberOfChildrenMarried
			End If
			If Not String.IsNullOrWhiteSpace(taxcodeforcivilstatesingleperson) AndAlso taxcodeforcivilstatesingleperson.Length = 3 Then
				Dim code = taxcodeforcivilstatesingleperson.Substring(0, 1)
				Dim numberOfChildrenSingle As Integer = taxcodeforcivilstatesingleperson.Substring(1, 1)
				Dim church = taxcodeforcivilstatesingleperson.Substring(2, 1)

				lueTaxSingleCode.EditValue = code
				lueChurchTaxSingle.EditValue = church
				lueNumberOfChildrenSingle.EditValue = numberOfChildrenSingle
			End If



			Me.chkemployeeferienback.Checked = employeeferienback
			Me.chkemployeefeiertagback.Checked = employeefeiertagback
			Me.chkemployee13lohnback.Checked = employee13lohnback

			Me.chkemployeenoes.Checked = employeenoes
			Me.chkemployeenolo.Checked = employeenolo
			Me.chkemployeenozg.Checked = employeenozg

			chkemployeedes.Checked = employeedes
			chkemployeerahmenarbeitsvertrag.Checked = employeerahmenarbeitsvertrag
			chkchilderlacantonsameastaxcanton.Checked = childerlacantonsameastaxcanton

			chkdonotaskforcreatingemployeecontactinpropose.Checked = donotaskforcreatingemployeecontactinpropose
			txtsearchforchangingapplicanttoemployee.EditValue = searchforchangingapplicanttoemployee

			Me.chkallowedflextimeinreports.Checked = If(allowedflextimeinreports Is Nothing, False, allowedflextimeinreports)
			Me.chkgetflextimefrommandantdatabase.Checked = If(getflextimefrommandantdatabase Is Nothing, False, getflextimefrommandantdatabase)

			LoadTransactionDropDown()
			LoadBankChargesDropDown()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Sub LoadTransactionDropDown()
		Dim listValue = New List(Of TransactionData)

		listValue.Add(New TransactionData With {.Value = 0, .Description = m_Translate.GetSafeTranslationValue("DTA-Datei erstellen (Schweiz)")})
		listValue.Add(New TransactionData With {.Value = 1, .Description = m_Translate.GetSafeTranslationValue("DTA-Datei erstellen (Ausland)")})
		listValue.Add(New TransactionData With {.Value = 2, .Description = m_Translate.GetSafeTranslationValue("DTA-Datei für Kreditoren erstellen (Schweiz)")})
		listValue.Add(New TransactionData With {.Value = 3, .Description = m_Translate.GetSafeTranslationValue("Vergütungsauftrag erstellen")})

		lueTransactionType.Properties.DataSource = listValue
		lueTransactionType.Properties.DropDownRows = listValue.Count

		Dim resetvalueaftercreateddtajob As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/resetvalueaftercreateddtajob", FORM_XML_DEFAULTVALUES_KEY)), 0)
		If resetvalueaftercreateddtajob > 3 Then resetvalueaftercreateddtajob = 3
		If resetvalueaftercreateddtajob < 0 Then resetvalueaftercreateddtajob = 0

		lueTransactionType.EditValue = listValue(resetvalueaftercreateddtajob)
		lueTransactionType.Properties.ForceInitialize()

	End Sub

	Private Sub LoadBankChargesDropDown()
		Dim listValue = New List(Of BankCharges)

		listValue.Add(New BankCharges With {.Value = 0, .Description = m_Translate.GetSafeTranslationValue("(OUR) Alle Spesen zu Lasten Auftraggeber")})
		listValue.Add(New BankCharges With {.Value = 1, .Description = m_Translate.GetSafeTranslationValue("(BEN) Alle Spesen zu Lasten Begünstigte")})
		listValue.Add(New BankCharges With {.Value = 2, .Description = m_Translate.GetSafeTranslationValue("(SHA) Spesen-Teilung")})

		lueBankCharges.Properties.DataSource = listValue
		lueBankCharges.Properties.DropDownRows = listValue.Count

		Dim defaultbankchargeto As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/defaultbankchargeto", FORM_XML_DEFAULTVALUES_KEY)), 1)
		If defaultbankchargeto > 2 Then defaultbankchargeto = 2
		If defaultbankchargeto < 0 Then defaultbankchargeto = 0

		lueBankCharges.EditValue = listValue(defaultbankchargeto)
		lueBankCharges.Properties.ForceInitialize()

	End Sub

	Private Function LoadSonstigeEinstellung() As Boolean
		Dim success As Boolean = True

		' Sonstige Einstellungen
		Dim calculatecustomerrefundinmarge As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/calculatecustomerrefundinmarge", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim askprintgavmaske As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/askprintgavmaske", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim ask4transferverleihtowos As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/ask4transferverleihtowos", FORM_XML_DEFAULTVALUES_KEY)), False)

		Dim warnbyzerocustomercreditlimit As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnbyzerocustomercreditlimit", FORM_XML_DEFAULTVALUES_KEY)), False)

		Dim TO_DELETE_warnbynocustomercreditlimit As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnbynocustomercreditlimit", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim warnbynovalidcustomercreditlimit As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/notlistcustomerwithnotvalidcreditdate", FORM_XML_DEFAULTVALUES_KEY)), False)

		Dim companyallowednopvl As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/companyallowednopvl", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim insertnewlineintoemploymentnotice As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/insertnewlineintoemploymentnotice", FORM_XML_DEFAULTVALUES_KEY)), False)

		Try
			' Data
			Me.chk_calculatecustomerrefundinmarge.Checked = calculatecustomerrefundinmarge
			Me.chkESPrintGAVForm.Checked = askprintgavmaske
			Me.chk_ask4transferverleihtowos.Checked = ask4transferverleihtowos


			Me.chk_warnbyzerocustomercreditlimit.Checked = warnbyzerocustomercreditlimit
			Me.chk_notlistcustomerwithnotvalidcreditdate.Checked = If(TO_DELETE_warnbynocustomercreditlimit <> warnbynovalidcustomercreditlimit, TO_DELETE_warnbynocustomercreditlimit, warnbynovalidcustomercreditlimit)



			Me.chk_companyallowednopvl.Checked = companyallowednopvl
			Me.chk_insertnewlineintoemploymentnotice.Checked = insertnewlineintoemploymentnotice


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function LoadDefaultvaluesCustomer() As Boolean
		Dim success As Boolean = True

		' Customer
		Dim firstcreditlimitamount As Decimal = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/firstcreditlimitamount", FORM_XML_DEFAULTVALUES_KEY)), 0)
		Dim secondcreditlimitamount As Decimal = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/secondcreditlimitamount", FORM_XML_DEFAULTVALUES_KEY)), 0)

		Dim invoicetype As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/invoicetype", FORM_XML_DEFAULTVALUES_KEY))
		Dim invoiceremindercode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/invoiceremindercode", FORM_XML_DEFAULTVALUES_KEY))
		Dim conditionalcash As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/conditionalcash", FORM_XML_DEFAULTVALUES_KEY))

		Dim vattabable As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vattabable", FORM_XML_DEFAULTVALUES_KEY)), True)
		Dim warnbycreditlimitexceeded As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnbycreditlimitexceeded", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim customernotuse As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/customernotuse", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim donotaskforcreatingcustomercontactinpropose As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/donotaskforcreatingcustomercontactinpropose", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim opencontactforminpropose As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/opencontactforminpropose", FORM_XML_DEFAULTVALUES_KEY)), False)
		Dim warnnotseenproposessince As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnnotseenproposessince", FORM_XML_DEFAULTVALUES_KEY)), 0)

		Try
			Me.txtFirstcreditlimitamount.Text = firstcreditlimitamount
			Me.txtSecondcreditlimitamount.Text = secondcreditlimitamount

			Me.cboInvoicetype.Text = If(String.IsNullOrWhiteSpace(invoicetype), "R", invoicetype)
			Me.cboInvoiceremindercode.Text = If(String.IsNullOrWhiteSpace(invoiceremindercode), "A", invoiceremindercode)
			Me.cboConditionalcash.Text = conditionalcash

			Me.chkVattabable.Checked = vattabable
			Me.chkWarnbycreditlimitexceeded.Checked = warnbycreditlimitexceeded
			Me.chkCustomernotuse.Checked = customernotuse

			chkdonotaskforcreatingcustomercontactinpropose.Checked = donotaskforcreatingcustomercontactinpropose
			chkopencontactforminpropose.Checked = opencontactforminpropose
			sewarnnotseenproposessince.EditValue = warnnotseenproposessince


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function LoadDefaultvaluesEinsatz() As Boolean
		Dim success As Boolean = True

		Try

			' Einsatz
			Dim esendebynull As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esendebynull", FORM_XML_DEFAULTVALUES_KEY))
			Dim esendebynullvv As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esendebynullvv", FORM_XML_DEFAULTVALUES_KEY))
			Dim essuvacode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/essuvacode", FORM_XML_DEFAULTVALUES_KEY))
			Dim escalcferienway As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/escalcferienway", FORM_XML_DEFAULTVALUES_KEY))
			Dim escalc13lohnway As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/escalc13lohnway", FORM_XML_DEFAULTVALUES_KEY))
			Dim eszeit As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/eszeit", FORM_XML_DEFAULTVALUES_KEY))
			Dim esort As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esort", FORM_XML_DEFAULTVALUES_KEY))
			Dim esvertrag As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esvertrag", FORM_XML_DEFAULTVALUES_KEY))
			Dim esverleih As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esverleih", FORM_XML_DEFAULTVALUES_KEY))

			Dim esreportsnotprint As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esreportsnotprint", FORM_XML_DEFAULTVALUES_KEY)), 2)

			Dim setsalarydatetotodayines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/setsalarydatetotodayines", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim selectadvisorkst As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/selectadvisorkst", FORM_XML_DEFAULTVALUES_KEY)), False)

			' Data...
			Me.txt_ESEndeByNull.Text = esendebynull
			Me.txt_ESEndeByNullvv.Text = esendebynullvv
			Me.cbo_ESSuvaCode.Text = essuvacode
			Me.cbo_escalcferienway.Text = escalcferienway
			Me.cbo_escalc13lohnway.Text = escalc13lohnway
			Me.txtESZeit.Text = eszeit
			Me.txtESOrt.Text = esort
			Me.txtESVertrag.Text = esvertrag
			Me.txtESVerleih.Text = esverleih

			Select Case esreportsnotprint
				Case 0
					Me.cboesreportsnotprint.EditValue = "0 - Nicht aktiviert (Rapporte drucken)"

				Case 1
					Me.cboesreportsnotprint.EditValue = "1 - Aktiviert (Rapporte NICHT drucken)"

				Case 2
					Me.cboesreportsnotprint.EditValue = "2 - Wie in der Kundenverwaltung"

				Case 3
					Me.cboesreportsnotprint.EditValue = "3 - Wie in der Kandidatenverwaltung"

				Case Else
					Me.cboesreportsnotprint.EditValue = "2 - Wie in der Kundenverwaltung"

			End Select

			Me.chksetsalarydatetotodayines.Checked = setsalarydatetotodayines
			Me.chk_selectadvisorkst.Checked = selectadvisorkst


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function LoadDefaultvaluesReportsAdvancPaymentPayroll() As Boolean
		Dim success As Boolean = True
		Dim sp_Utility As New SPProgUtility.MainUtilities.Utilities

		Try

			' Reports
			Dim copykstintoreportline As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/copykstintoreportline", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim hoursroundkind As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/hoursroundkind", FORM_XML_DEFAULTVALUES_KEY)), 1)
			Dim byzerohourdonotprintlabel As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/byzerohourdonotprintlabel", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim datamatrixcodestringforlabel As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/datamatrixcodestringforlabel", FORM_XML_DEFAULTVALUES_KEY))

			'Dim strQuery As String = "//Report/matrixprintername"
			'Dim matrixprintername As String = sp_Utility.GetXMLValueByQueryWithFilename(m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr), strQuery, Nothing)

			' Data...
			Me.chkcopykstintoreportline.Checked = copykstintoreportline
			Me.cbohoursroundkind.EditValue = If(hoursroundkind = 1, m_Translate.GetSafeTranslationValue("Minuten (genau)"), m_Translate.GetSafeTranslationValue("2-stellige Dezimal (nicht genau!)"))
			' Datamatrixprinter
			'Me.chkUserMatrixPrinter.EditValue = matrixprintername

			' datamatrixcodestringforlabel
			Me.chkbyzerohourdonotprintlabel.Checked = byzerohourdonotprintlabel
			Me.txtdatamatrixcodestringforlabel.EditValue = datamatrixcodestringforlabel


			' AdvacePayment
			Dim advancepaymentdefaultpaymenttype As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentdefaultpaymenttype", FORM_XML_DEFAULTVALUES_KEY))
			Dim advancepaymentwithfee As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentwithfee", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim setpayoutdatetotodayinadvancepayment As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/setpayoutdatetotodayinadvancepayment", FORM_XML_DEFAULTVALUES_KEY)), False)
			Dim advancepaymentpaymentreason As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentpaymentreason", FORM_XML_DEFAULTVALUES_KEY))

			Dim advancepaymentprintaftercreate8900and8930 As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentprintaftercreate8900and8930", FORM_XML_DEFAULTVALUES_KEY)), True)
			Dim advancepaymentprintaftercreate8920 As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentprintaftercreate8920", FORM_XML_DEFAULTVALUES_KEY)), True)
			Dim advancepaymentopenformaftercreate As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentopenformaftercreate", FORM_XML_DEFAULTVALUES_KEY)), True)
			Dim insertnewlineintoadvancedpaymentnotice As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/insertnewlineintoadvancedpaymentnotice", FORM_XML_DEFAULTVALUES_KEY)), True)

			' Data...
			Me.cboadvancepaymentdefaultpaymenttype.Text = advancepaymentdefaultpaymenttype
			Me.chkadvancepaymentwithfee.Checked = advancepaymentwithfee
			Me.chksetpayoutdatetotodayinadvancepayment.Checked = setpayoutdatetotodayinadvancepayment

			Me.txtadvancepaymentpaymentreason.Text = If(String.IsNullOrWhiteSpace(advancepaymentpaymentreason), "à Konto", advancepaymentpaymentreason)

			Me.chkadvancepaymentprintaftercreate8900and8930.Checked = advancepaymentprintaftercreate8900and8930
			Me.chkadvancepaymentprintaftercreate8920.Checked = advancepaymentprintaftercreate8920
			Me.chkadvancepaymentopenformaftercreate.Checked = advancepaymentopenformaftercreate
			Me.chk_insertnewlineintoadvancedpaymentnotice.Checked = insertnewlineintoadvancedpaymentnotice


			' payroll
			Dim payrollopenprintformaftercreate As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/payrollopenprintformaftercreate", FORM_XML_DEFAULTVALUES_KEY)), True)

			Dim printfeiertaginpayslip As Boolean? = Nothing


			If Not String.IsNullOrWhiteSpace(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY))) Then
				printfeiertaginpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
			End If
			Dim printFerienInpayslip As Boolean = True
			Dim print13LohnInpayslip As Boolean = True
			Dim printDarleheninpayslip As Boolean = True
			Dim printGleitStdinpayslip As Boolean = True
			Dim printNightStdinpayslip As Boolean = True
			Dim sortkwlanrinpayslip As Boolean = False

			Dim minAmountfeiertaginpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
			Dim minAmountFerienInpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountFerienInpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
			Dim minAmount13LohnInpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmount13LohnInpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
			Dim minAmountDarleheninpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountDarleheninpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
			Dim minAmountGleitStdinpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountGleitStdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
			Dim minAmountNightStdinpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountNightStdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)


			If printfeiertaginpayslip.HasValue Then
				printfeiertaginpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
				printFerienInpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printferieninpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
				print13LohnInpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/print13lohninpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
				printDarleheninpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printdarleheninpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
				printGleitStdinpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printgleitstdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
				printNightStdinpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/printnightstdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), True)
				sortkwlanrinpayslip = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/sortkwlanrinpayslip", FORM_XML_DEFAULTVALUES_KEY)), False)

			Else
				' aus kompatibilitätesgründen!!!
				Dim local_utility As New SPProgUtility.MainUtilities.Utilities
				printfeiertaginpayslip = CBool(local_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printfeiertaginlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
				printFerienInpayslip = CBool(local_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printferieninlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
				print13LohnInpayslip = CBool(local_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/print13lohninlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
				printDarleheninpayslip = CBool(local_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printdarleheninlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
				printGleitStdinpayslip = CBool(local_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printgleitstdinlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))
				printNightStdinpayslip = CBool(local_utility.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, String.Format("{0}/printnightstdinlo".ToLower, FORM_XML_PRINT_PAYROLL_KEY), "0"))

				Dim _ClsReg As New SPProgUtility.ClsDivReg
				Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
				If Val(_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Lohnbuchhaltung", "SortKW+LANr", "0")) <> 0 Then
					sortkwlanrinpayslip = True
				Else
					sortkwlanrinpayslip = False
				End If

			End If

			' Data...
			Me.chkpayrollopenprintformaftercreate.Checked = payrollopenprintformaftercreate

			Me.chkprintfeiertaginpayslip.Checked = printfeiertaginpayslip
			Me.chkprintFerieninpayslip.Checked = printFerienInpayslip
			Me.chkprint13Lohninpayslip.Checked = print13LohnInpayslip
			Me.chkprintdarleheninpayslip.Checked = printDarleheninpayslip
			Me.chkprintgleitstdinpayslip.Checked = printGleitStdinpayslip
			Me.chkprintnightstdinpayslip.Checked = printNightStdinpayslip
			Me.chksortkwlanrinpayslip.Checked = sortkwlanrinpayslip

			txt_minAmountfeiertaginpayslip.EditValue = minAmountfeiertaginpayslip
			txt_minAmountFerienInpayslip.EditValue = minAmountFerienInpayslip
			txt_minAmount13LohnInpayslip.EditValue = minAmount13LohnInpayslip
			txt_minAmountDarleheninpayslip.EditValue = minAmountDarleheninpayslip
			txt_minAmountGleitStdinpayslip.EditValue = minAmountGleitStdinpayslip
			txt_minAmountNightStdinpayslip.EditValue = minAmountNightStdinpayslip


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


	Private Function LoadDefaultvaluesInvoice() As Boolean
		Dim success As Boolean = True

		Dim ref10forfactoring As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/ref10forfactoring", m_InvoiceSetting)), False)
		Dim ezonsepratedpage As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/ezonsepratedpage", m_InvoiceSetting)), False)

		Dim setfakdatetoendofreportmonth As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/setfakdatetoendofreportmonth", m_InvoiceSetting)), False)
		Dim calculateduedatefromcreatedon As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/calculateduedatefromcreatedon", m_InvoiceSetting)), False)
		Dim create3mahnasuntilnotpaid As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/create3mahnasuntilnotpaid", m_InvoiceSetting)), False)
		Dim printezwithmahn As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/printezwithmahn", m_InvoiceSetting)), False)
		Dim printguonmahnung As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/printguonmahnung", m_InvoiceSetting)), False)

		Dim mwstnr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstnr", m_InvoiceSetting))
		Dim mwstsatz As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", m_InvoiceSetting)), 8)
		Dim roundopenamountforbooking As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/roundopenamountforbooking", m_InvoiceSetting)), False)

		Dim factoringcustomernumber As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/factoringcustomernumber", m_InvoiceSetting))
		Dim invoicezipfilename As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/invoicezipfilename", m_InvoiceSetting))

		Dim mahnspesenab As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mahnspesenab", m_InvoiceSetting)), 0)
		Dim mahnspesenchf As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mahnspesenchf", m_InvoiceSetting)), 0)

		Dim verzugszinsdaysafter As Integer = ParseToInteger(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/verzugszinsdaysafter", m_InvoiceSetting)), 0)
		Dim verzugszinsabchf As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/verzugszinsabchf", m_InvoiceSetting)), 0)
		Dim verzugszinspercent As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/verzugszinspercent", m_InvoiceSetting)), 0)


		Try
			Me.chkref10forfactoring.Checked = ref10forfactoring
			Me.chkezonsepratedpage.Checked = ezonsepratedpage
			Me.chksetfakdatetoendofreportmonth.Checked = setfakdatetoendofreportmonth
			Me.chkcalculateduedatefromcreatedon.Checked = setfakdatetoendofreportmonth
			Me.chkcreate3mahnasuntilnotpaid.Checked = create3mahnasuntilnotpaid
			Me.chkprintezwithmahn.Checked = printezwithmahn
			Me.chkprintguonmahnung.Checked = printguonmahnung

			Me.txtmwstnr.EditValue = mwstnr
			Me.txtmwstsatz.EditValue = mwstsatz
			Me.chkroundopenamountforbooking.Checked = roundopenamountforbooking

			'Me.txtfactoringcustomernumber.EditValue = factoringcustomernumber
			'Me.txtinvoicezipfilename.EditValue = invoicezipfilename

			'Me.txtmahnspesenab.EditValue = mahnspesenab
			'Me.txtmahnspesenchf.EditValue = mahnspesenchf
			'Me.txtverzugszinsdaysafter.EditValue = verzugszinsdaysafter
			'Me.txtverzugszinsabchf.EditValue = verzugszinsabchf
			'Me.txtverzugszinspercent.EditValue = verzugszinspercent


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function LoadPayrollSetting() As Boolean
		Dim success As Boolean = True

		Try
			Me.chkGuthabenperES.Checked = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Lohnbuchhaltung/guthaben/showguthabenpereaches", m_MandantSetting)) = "1"

		Catch ex As Exception
			Me.chkGuthabenperES.Checked = False
		End Try
		Try
			Me.txttagesspesenstdab.EditValue = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Lohnbuchhaltung/report/tagesspesenstdab", m_MandantSetting))

		Catch ex As Exception
			Me.txttagesspesenstdab.EditValue = 8.25
		End Try

		Return success

	End Function


	Public Function SaveSettingData() As Boolean
		Dim success As Boolean = True
		If Not IsDataValid Then Return False

		Dim suppressUIEventState = m_SuppressUIEvents
		m_SuppressUIEvents = False

		Try
			If (m_MandantDatabaseAccess Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				success = False
			End If

			' User setting
			'success = success AndAlso SaveUserfieldData()

			' invoice defaultfields
			success = success AndAlso SaveInvocieSettings()

			' payroll setting
			success = success AndAlso SavePayrollSetting()

			success = success AndAlso SaveDefaultFieldData()

			' erst wenn in der DB alles OK ist...
			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
			m_Logger.LogError(String.Format("ucStandardvalues: {0}", ex.ToString))
			success = False

		Finally

		End Try
		IsDataValid = success

		Return success

	End Function



#Region "save data"

	Private Function SaveInvocieSettings() As Boolean
		Dim success As Boolean = True

		Try

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/mwstnr", m_MandantSetting), txtmwstnr.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/mwstsatz", m_MandantSetting), txtmwstsatz.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/roundopenamountforbooking", m_MandantSetting), If(chkroundopenamountforbooking.Checked, "true", "false"))

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/ref10forfactoring", m_MandantSetting), If(chkref10forfactoring.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/ezonsepratedpage", m_MandantSetting), If(chkezonsepratedpage.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/setfakdatetoendofreportmonth", m_MandantSetting), If(chksetfakdatetoendofreportmonth.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/calculateduedatefromcreatedon", m_MandantSetting), If(chkcalculateduedatefromcreatedon.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/create3mahnasuntilnotpaid", m_MandantSetting), If(chkcreate3mahnasuntilnotpaid.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/printezwithmahn", m_MandantSetting), If(chkprintezwithmahn.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/printguonmahnung", m_MandantSetting), If(chkprintguonmahnung.Checked, "true", "false"))

			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/mahnspesenab", m_MandantSetting), txtmahnspesenab.EditValue)
			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/mahnspesenchf", m_MandantSetting), txtmahnspesenchf.EditValue)
			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/verzugszinsdaysafter", m_MandantSetting), txtverzugszinsdaysafter.EditValue)
			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/verzugszinspercent", m_MandantSetting), txtverzugszinspercent.EditValue)

			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/verzugszinsabchf", m_MandantSetting), txtverzugszinsabchf.EditValue)
			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/factoringcustomernumber", m_MandantSetting), txtfactoringcustomernumber.EditValue)
			'm_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/invoicezipfilename", m_MandantSetting), txtinvoicezipfilename.EditValue)

		Catch ex As Exception
			m_Logger.LogError(String.Format("ucStandardvalues: {0}", ex.ToString))
			success = False

		End Try

		Return success

	End Function


	Private Function SavePayrollSetting() As Boolean
		Dim success As Boolean = True

		Try
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Lohnbuchhaltung/guthaben/showguthabenpereaches", m_MandantSetting), CStr(If(Me.chkGuthabenperES.Checked, 1, 0)))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Lohnbuchhaltung/report/tagesspesenstdab", m_MandantSetting),
																							If(Val(Me.txttagesspesenstdab.EditValue) = 0, 8.25, Val(Me.txttagesspesenstdab.EditValue)))

		Catch ex As Exception
			m_Logger.LogError(String.Format("ucStandardvalues: {0}", ex.ToString))
			success = False

		End Try

		Return success

	End Function



	Private Function AddOrUpdateFieldLabelNode(ByVal xDoc As XmlDocument,
														 ByVal strGuid As String, ByVal strMainKey As String,
														 ByVal KeyValue As String) As Boolean
		Dim success As Boolean = True
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing
		Dim strKeyName As String = String.Empty
		strKeyName = "CtlLabel"

		Try

			xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
			If xNode Is Nothing Then
				Dim newNode As Xml.XmlElement = xDoc.CreateElement("Control")

				newNode.SetAttribute("Name", strGuid)
				xDoc.DocumentElement.AppendChild(newNode)
				xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
			End If

			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
			End If

			If TypeOf xNode Is XmlElement Then xElmntFamily = CType(xNode, XmlElement)
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))

			With xElmntFamily
				.SetAttribute("Name", String.Format("{0}", strGuid))
				.AppendChild(xDoc.CreateElement("CtlLabel")).InnerText = KeyValue
			End With

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		' evreytime returns true!
		Return success

	End Function


	Private Function SaveDefaultFieldData() As Boolean
		Dim success As Boolean = True
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		xDoc.Load(m_MandantFormXMLFile)

		Try
			xNode = xDoc.SelectSingleNode("*//Field_DefaultValues")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Field_DefaultValues", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				Dim strKey As String = String.Empty

				' Währung 
				strKey = "currencyvalue"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboCurrencyvalue.Text)

				' Haupt Sprache
				strKey = "mainlanguagevalue"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cbomainlanguagevalue.Text)

				' 1. Kreditlimite 
				strKey = "firstcreditlimitamount"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, Val(txtFirstcreditlimitamount.Text))
				' 2. Kreditlimite 
				strKey = "secondcreditlimitamount"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, Val(txtSecondcreditlimitamount.Text))

				' Fakturacode
				strKey = "invoicetype"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboInvoicetype.Text)
				' Mahncode
				strKey = "invoiceremindercode"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboInvoiceremindercode.Text)
				' Zahlungskondition 
				strKey = "conditionalcash"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboConditionalcash.Text)

				' Kopie KST into AdditionalInfo
				strKey = "copykstintoreportline"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkcopykstintoreportline.Checked, "true", "false"))

				' Roundkind für Runden der Dezimal und Normal-Stunden in Rapport
				strKey = "hoursroundkind"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(cbohoursroundkind.EditValue = "Minuten (genau)", 1, 2))

				' chkbyzerohourdonotprintlabel
				strKey = "byzerohourdonotprintlabel"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkbyzerohourdonotprintlabel.Checked, "true", "false"))

				' format for label
				strKey = "datamatrixcodestringforlabel"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txtdatamatrixcodestringforlabel.EditValue)

				' MwSt.-pflicht
				strKey = "vattabable"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkVattabable.Checked, "true", "false"))

				strKey = "vatable"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkVattabable.Checked, "true", "false"))

				' Warnung bei Kreditlimitenüberschreitung
				strKey = "warnbycreditlimitexceeded"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkWarnbycreditlimitexceeded.Checked, "true", "false"))

				' Einsätze sperren
				strKey = "customernotuse"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkCustomernotuse.Checked, "true", "false"))

				' propose
				strKey = "donotaskforcreatingcustomercontactinpropose"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkdonotaskforcreatingcustomercontactinpropose.Checked, "true", "false"))

				strKey = "opencontactforminpropose"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkopencontactforminpropose.Checked, "true", "false"))

				strKey = "warnnotseenproposessince"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, sewarnnotseenproposessince.EditValue)
				' Employee

				strKey = "employeezahlart"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeezahlart.Text)
				strKey = "employeebvgcode"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeebvgcode.Text)
				strKey = "employeebvgcodewithchild"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeebvgcodewithchild.Text)

				strKey = "employeerahmenarbeitsvertrag"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeerahmenarbeitsvertrag.Checked, "true", "false"))
				strKey = "employeeferienback"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeeferienback.Checked, "true", "false"))
				strKey = "employeefeiertagback"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeefeiertagback.Checked, "true", "false"))

				strKey = "employee13lohnback"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployee13lohnback.Checked, "true", "false"))

				strKey = "employeenoes"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeenoes.Checked, "true", "false"))
				strKey = "employeenolo"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeenolo.Checked, "true", "false"))
				strKey = "employeenozg"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeenozg.Checked, "true", "false"))
				strKey = "employeedes"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemployeedes.Checked, "true", "false"))
				strKey = "childerlacantonsameastaxcanton"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkchilderlacantonsameastaxcanton.Checked, "true", "false"))

				strKey = "donotaskforcreatingemployeecontactinpropose"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkdonotaskforcreatingemployeecontactinpropose.Checked, "true", "false"))

				Dim stringData As String = txtsearchforchangingapplicanttoemployee.EditValue
				If Not String.IsNullOrWhiteSpace(stringData) Then
					Dim value As List(Of String) = stringData.Split(New Char() {";"c, ","c, "#"c, "|"c}).ToList

					If value.Count > 0 Then
						Dim lastname As String = value.Where(Function(x) x = "lastname").FirstOrDefault
						Dim firstname As String = value.Where(Function(x) x = "firstname").FirstOrDefault
						Dim eMail As String = value.Where(Function(x) x = "email").FirstOrDefault
						Dim searchforBirthdate As String = value.Where(Function(x) x = "birthdate").FirstOrDefault

						If String.IsNullOrWhiteSpace(lastname) OrElse String.IsNullOrWhiteSpace(firstname) Then stringData = String.Empty
					Else
						stringData = String.Empty
					End If
				End If

				strKey = "searchforchangingapplicanttoemployee"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, stringData)


				strKey = "employeecontact"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeecontact.Text)
				strKey = "emplyoeefstate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemplyoeefstate.Text)
				strKey = "employeesstate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeesstate.Text)
				strKey = "employeenationality"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeenationality.Text)
				strKey = "employeecountryqualification"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeecountryqualification.Text)

				strKey = "permissioncodeonnotswiss"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, luePermission.EditValue)

				strKey = "taxcodeforcivilstatemarriedperson"
				Dim taxvalue As String
				If lueTaxSingleCode.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueTaxSingleCode.EditValue) Then
					taxvalue = String.Empty
				Else
					taxvalue = String.Format("{0}{1}{2}", lueTaxMarriedCode.EditValue, If(lueNumberOfChildrenMarried.EditValue Is Nothing, 0, lueNumberOfChildrenMarried.EditValue), If(lueChurchTaxMarried.EditValue Is Nothing, "Y", lueChurchTaxMarried.EditValue))
				End If
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, taxvalue)

				strKey = "taxcodeforcivilstatesingleperson"
				If lueTaxSingleCode.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueTaxSingleCode.EditValue) Then
					taxvalue = String.Empty
				Else
					taxvalue = String.Format("{0}{1}{2}", lueTaxSingleCode.EditValue, If(lueNumberOfChildrenSingle.EditValue Is Nothing, 0, lueNumberOfChildrenSingle.EditValue), If(lueChurchTaxSingle.EditValue Is Nothing, "Y", lueChurchTaxSingle.EditValue))
				End If
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, taxvalue)


				strKey = "essuvacode"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cbo_ESSuvaCode.Text)

				strKey = "escalcferienway"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cbo_escalcferienway.Text)

				strKey = "escalc13lohnway"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cbo_escalc13lohnway.Text)

				strKey = "ask4transferverleihtowos"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_ask4transferverleihtowos.Checked, "true", "false"))

				strKey = "warnbyzerocustomercreditlimit"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_warnbyzerocustomercreditlimit.Checked, "true", "false"))

				strKey = "warnbynocustomercreditlimit"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_notlistcustomerwithnotvalidcreditdate.Checked, "true", "false"))

				strKey = "notlistcustomerwithnotvalidcreditdate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_notlistcustomerwithnotvalidcreditdate.Checked, "true", "false"))

				strKey = "companyallowednopvl"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_companyallowednopvl.Checked, "true", "false"))

				strKey = "selectadvisorkst"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_selectadvisorkst.Checked, "true", "false"))

				strKey = "calculatecustomerrefundinmarge"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_calculatecustomerrefundinmarge.Checked, "true", "false"))

				strKey = "askprintgavmaske"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkESPrintGAVForm.Checked, "true", "false"))

				strKey = "insertnewlineintoemploymentnotice"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_insertnewlineintoemploymentnotice.Checked, "true", "false"))


				strKey = "esendebynull".ToLower
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_ESEndeByNull.Text)

				strKey = "esendebynullvv".ToLower
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_ESEndeByNullvv.Text)

				strKey = "eszeit"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txtESZeit.Text)

				strKey = "esort"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txtESOrt.Text)

				strKey = "esvertrag"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txtESVertrag.Text)

				strKey = "esverleih"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txtESVerleih.Text)

				strKey = "esreportsnotprint"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(cboesreportsnotprint.EditValue Is Nothing, 2, Val(cboesreportsnotprint.EditValue)))

				strKey = "setsalarydatetotodayines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chksetsalarydatetotodayines.Checked, "true", "false"))

				' Reports ...
				strKey = "allowedflextimeinreports"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkallowedflextimeinreports.Checked, "true", "false"))

				strKey = "getflextimefrommandantdatabase"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkgetflextimefrommandantdatabase.Checked, "true", "false"))

				' AdvancePayment
				strKey = "advancepaymentpaymentreason"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txtadvancepaymentpaymentreason.Text)

				strKey = "advancepaymentwithfee"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkadvancepaymentwithfee.Checked, "true", "false"))

				strKey = "advancepaymentdefaultpaymenttype"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboadvancepaymentdefaultpaymenttype.Text)

				strKey = "setpayoutdatetotodayinadvancepayment"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chksetpayoutdatetotodayinadvancepayment.Checked, "true", "false"))

				strKey = "advancepaymentprintaftercreate8900and8930"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkadvancepaymentprintaftercreate8900and8930.Checked, "true", "false"))

				strKey = "advancepaymentprintaftercreate8920"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkadvancepaymentprintaftercreate8920.Checked, "true", "false"))

				strKey = "advancepaymentopenformaftercreate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkadvancepaymentopenformaftercreate.Checked, "true", "false"))

				strKey = "insertnewlineintoadvancedpaymentnotice"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chk_insertnewlineintoadvancedpaymentnotice.Checked, "true", "false"))

				strKey = "resetvalueaftercreateddtajob"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, TryCast(lueTransactionType.EditValue, TransactionData).Value)

				strKey = "defaultbankchargeto"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, TryCast(lueBankCharges.EditValue, BankCharges).Value)




				strKey = "payrollopenprintformaftercreate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkpayrollopenprintformaftercreate.Checked, "true", "false"))


				strKey = "printfeiertaginpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkprintfeiertaginpayslip.Checked, "true", "false"))

				strKey = "printferieninpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkprintFerieninpayslip.Checked, "true", "false"))

				strKey = "print13lohninpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkprint13Lohninpayslip.Checked, "true", "false"))

				strKey = "printdarleheninpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkprintdarleheninpayslip.Checked, "true", "false"))

				strKey = "printgleitstdinpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkprintgleitstdinpayslip.Checked, "true", "false"))

				strKey = "printnightstdinpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkprintnightstdinpayslip.Checked, "true", "false"))


				strKey = "minAmountfeiertaginpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_minAmountfeiertaginpayslip.EditValue)

				strKey = "minAmountFerienInpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_minAmountFerienInpayslip.EditValue)


				strKey = "minAmount13LohnInpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_minAmount13LohnInpayslip.EditValue)


				strKey = "minAmountDarleheninpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_minAmountDarleheninpayslip.EditValue)


				strKey = "minAmountGleitStdinpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_minAmountGleitStdinpayslip.EditValue)


				strKey = "minAmountNightStdinpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_minAmountNightStdinpayslip.EditValue)




				strKey = "sortkwlanrinpayslip"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chksortkwlanrinpayslip.Checked, "true", "false"))


			End If

			xDoc.Save(m_MandantFormXMLFile)


		Catch ex As Exception
			m_Logger.LogError(String.Format("ucStandardvalues: {0}", ex.ToString))
			success = False

		End Try

		Return success

	End Function







	'Private Function SaveUserfieldData() As Boolean
	'	Dim success As Boolean = True
	'	'Dim xElmntFamily As XmlElement = Nothing

	'	'Dim xEle As XElement = XElement.Load(m_MandantUserXMLFile)
	'	'Try

	'	'	Dim MyNode = xEle.Elements("Layouts").Elements("Report").Elements("matrixprintername").ToList()
	'	'	If MyNode.Count > 0 Then
	'	'		For Each cEle As XElement In MyNode
	'	'			cEle.ReplaceNodes(String.Format("{0}", chkUserMatrixPrinter.EditValue))
	'	'		Next cEle

	'	'	Else
	'	'		xEle.AddFirst(New XElement("Layouts", _
	'	'																 New XElement("Report", _
	'	'																							New XElement("matrixprintername", chkUserMatrixPrinter.EditValue))))
	'	'	End If

	'	Dim xDoc As XmlDocument = New XmlDocument()
	'	Dim xNode As XmlNode
	'	Dim xElmntFamily As XmlElement = Nothing

	'	xDoc.Load(m_MandantUserXMLFile)

	'	Try
	'		xNode = xDoc.SelectSingleNode("*//Report")
	'		If xNode Is Nothing Then
	'			xNode = xDoc.CreateNode(XmlNodeType.Element, "Report", "")
	'			xDoc.DocumentElement.AppendChild(xNode)
	'		End If

	'		If xNode IsNot Nothing Then
	'			If TypeOf xNode Is XmlElement Then
	'				xElmntFamily = CType(xNode, XmlElement)
	'			End If

	'			Dim strKey As String = String.Empty

	'			strKey = "matrixprintername"
	'			If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
	'			InsertTextNode(xDoc, xElmntFamily, strKey, chkUserMatrixPrinter.EditValue)

	'		End If

	'		xDoc.Save(m_MandantUserXMLFile)


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("ucStandardvalues: {0}", ex.ToString))
	'		IsDataValid = False

	'	End Try

	'	Return success

	'End Function


#End Region




#Region "Helpers"

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
		Dim result As Integer
		If (Not Integer.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function


#End Region


#Region "Helper Class"

	Private Class BankCharges

		Public Property Value As Integer
		Public Property Description As String

	End Class

	Private Class TransactionData

		Public Property Value As Integer
		Public Property Description As String

	End Class


	Private Class QSTCodeViewData
		Public Property Code As String
		Public Property Translation As String

		Public ReadOnly Property CodeAndTranslation As String
			Get
				Return String.Format("{0} - {1}", Code, Translation)
			End Get
		End Property
	End Class

	''' <summary>
	''' Church view data.
	''' </summary>
	Private Class ChurchViewData
		Public Property ChurchTaxCode As String
		Public Property Translation As String
		Public ReadOnly Property ChurchCodeAndTranslation As String
			Get
				Return String.Format("{0} - {1}", ChurchTaxCode, Translation)
			End Get
		End Property
	End Class

	''' <summary>
	''' Number of children view data.
	''' </summary>
	Private Class NumberOfChildrenViewData
		Public Property NumberOfChildren As Integer
	End Class


#End Region



End Class
