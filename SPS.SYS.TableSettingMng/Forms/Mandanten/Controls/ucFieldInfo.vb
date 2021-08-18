

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



Public Class ucFieldInfo

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

#End Region


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

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

	Private Const FORM_XML_REQUIREDFIELDS_KEY As String = "Forms_Normaly/requiredfields"

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
		m_InitializationData = ClsDataDetail.m_InitialData ' _setting

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		If m_InitializationData Is Nothing Then Exit Sub

		m_Year = m_InitializationData.MDData.MDYear
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_ProgPath = New ClsProgPath

		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Me.xtabMain.SelectedTabPage = xtabDefaultValues
		Me.xtabDefaultValue.SelectedTabPage = xtabEmplyeeAutofield
		Me.xtabDefaultValueEmployee.SelectedTabPage = xtabEmployeeRueckstellung
		Me.xtabDefaultValueES.SelectedTabPage = xtabDefaultESAllgemein
		Me.xtabFieldLabel.SelectedTabPage = xtabMAFieldLabel

		Dim printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters
		For Each p In printers
			chkUserMatrixPrinter.Properties.Items.Add(p.ToString)
		Next

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

			Me.xtabMain.SelectedTabPage = xtabDefaultValues
			Me.xtabDefaultValue.SelectedTabPage = xtabEmplyeeAutofield
			Me.xtabDefaultValueEmployee.SelectedTabPage = xtabEmployeeRueckstellung
			Me.xtabDefaultValueES.SelectedTabPage = xtabDefaultESAllgemein
			Me.xtabFieldLabel.SelectedTabPage = xtabMAFieldLabel


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

			success = success And LoadDefaultvaluesEmployee()
			success = success And LoadSonstigeEinstellung()

			success = success And LoadDefaultvaluesCustomer()
			success = success And LoadDefaultvaluesEinsatz()

			success = success And LoadPayrollSetting()

			success = success And LoadDefaultvaluesReportsAdvancPaymentPayroll()

			success = success And LoadDefaultvaluesInvoice()

			success = success And LoadFieldLabelValue()
			success = success And LoadRequiredfields()

			' Webservices
			success = success And LoadWebServiceInfos()

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

		' BVG-Data


		' KTG-Data


		' AHV-Data


		' NBUV-Data

		' ALV1-Data


		' ALV2-Data


		m_SuppressUIEvents = False

	End Sub


	Private Function LoadDefaultvaluesEmployee() As Boolean
		Dim success As Boolean = True

		Try
			' Default Values common
			Dim currencyvalue As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY))
			Dim mainlanguagevalue As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/mainlanguagevalue", FORM_XML_MAIN_KEY))

			' Data...
			Me.cboCurrencyvalue.EditValue = If(currencyvalue = String.Empty, "CHF", currencyvalue)
			Me.cbomainlanguagevalue.EditValue = If(mainlanguagevalue = String.Empty, "deutsch", mainlanguagevalue)


			' Employee
			Dim employeezahlart As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeezahlart", FORM_XML_MAIN_KEY))
			Dim employeebvgcode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeebvgcode", FORM_XML_MAIN_KEY))
			Dim employeebvgcodewithchild As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeebvgcodewithchild", FORM_XML_MAIN_KEY))
			Dim employeerahmenarbeitsvertrag As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeerahmenarbeitsvertrag", FORM_XML_MAIN_KEY)), True)
			Dim employeeferienback As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeeferienback", FORM_XML_MAIN_KEY)), False)
			Dim employeefeiertagback As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeefeiertagback", FORM_XML_MAIN_KEY)), False)
			Dim employee13lohnback As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employee13lohnback", FORM_XML_MAIN_KEY)), False)

			Dim employeenoes As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenoes", FORM_XML_MAIN_KEY)), False)
			Dim employeenolo As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenolo", FORM_XML_MAIN_KEY)), False)
			Dim employeenozg As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeenozg", FORM_XML_MAIN_KEY)), False)
			Dim employeedes As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeedes", FORM_XML_MAIN_KEY)), False)
			Dim childerlacantonsameastaxcanton As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/childerlacantonsameastaxcanton", FORM_XML_MAIN_KEY)), False)

			Dim employeesecsuvacode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeesecsuvacode", FORM_XML_MAIN_KEY))
			Dim employeesstate As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeesstate", FORM_XML_MAIN_KEY))
			Dim emplyoeefstate As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeefstate", FORM_XML_MAIN_KEY))
			Dim employeecontact As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/employeecontact", FORM_XML_MAIN_KEY))


			Dim allowedflextimeinreports As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/allowedflextimeinreports", FORM_XML_MAIN_KEY)), False)
			Dim getflextimefrommandantdatabase As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/getflextimefrommandantdatabase", FORM_XML_MAIN_KEY)), False)


			' Data...
			Me.cboemployeezahlart.Text = If(String.IsNullOrWhiteSpace(employeezahlart), "K", employeezahlart)
			Me.cboemployeebvgcode.Text = If(String.IsNullOrWhiteSpace(employeebvgcode), "9", employeebvgcode)
			Me.cboemployeebvgcodewithchild.Text = If(String.IsNullOrWhiteSpace(employeebvgcodewithchild), "1", employeebvgcodewithchild)

			Me.cboemployeecontact.Text = employeecontact
			Me.cboemployeesstate.Text = employeesstate
			Me.cboemplyoeefstate.Text = emplyoeefstate
			Me.cboemployeesecsuvacode.Text = employeesecsuvacode

			Me.chkemployeeferienback.Checked = employeeferienback
			Me.chkemployeefeiertagback.Checked = employeefeiertagback
			Me.chkemployee13lohnback.Checked = employee13lohnback

			Me.chkemployeenoes.Checked = employeenoes
			Me.chkemployeenolo.Checked = employeenolo
			Me.chkemployeenozg.Checked = employeenozg

			Me.chkemployeedes.Checked = employeedes
			Me.chkemployeerahmenarbeitsvertrag.Checked = employeerahmenarbeitsvertrag
			Me.chkchilderlacantonsameastaxcanton.Checked = childerlacantonsameastaxcanton


			Me.chkallowedflextimeinreports.Checked = If(allowedflextimeinreports Is Nothing, False, allowedflextimeinreports)
			Me.chkgetflextimefrommandantdatabase.Checked = If(getflextimefrommandantdatabase Is Nothing, False, getflextimefrommandantdatabase)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


	Private Function LoadSonstigeEinstellung() As Boolean
		Dim success As Boolean = True

		' Sonstige Einstellungen
		Dim calculatecustomerrefundinmarge As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/calculatecustomerrefundinmarge", FORM_XML_MAIN_KEY)), False)
		Dim askprintgavmaske As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/askprintgavmaske", FORM_XML_MAIN_KEY)), False)
		Dim ask4transferverleihtowos As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/ask4transferverleihtowos", FORM_XML_MAIN_KEY)), False)

		Dim warnbyzerocustomercreditlimit As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnbyzerocustomercreditlimit", FORM_XML_MAIN_KEY)), False)

		Dim TO_DELETE_warnbynocustomercreditlimit As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnbynocustomercreditlimit", FORM_XML_MAIN_KEY)), False)
		Dim warnbynovalidcustomercreditlimit As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/notlistcustomerwithnotvalidcreditdate", FORM_XML_MAIN_KEY)), False)

		Dim companyallowednopvl As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)

		Try
			' Data
			Me.chk_calculatecustomerrefundinmarge.Checked = calculatecustomerrefundinmarge
			Me.chkESPrintGAVForm.Checked = askprintgavmaske
			Me.chk_ask4transferverleihtowos.Checked = ask4transferverleihtowos


			Me.chk_warnbyzerocustomercreditlimit.Checked = warnbyzerocustomercreditlimit
			Me.chk_notlistcustomerwithnotvalidcreditdate.Checked = If(TO_DELETE_warnbynocustomercreditlimit <> warnbynovalidcustomercreditlimit, TO_DELETE_warnbynocustomercreditlimit, warnbynovalidcustomercreditlimit)



			Me.chk_companyallowednopvl.Checked = companyallowednopvl


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function LoadDefaultvaluesCustomer() As Boolean
		Dim success As Boolean = True

		' Customer
		Dim firstcreditlimitamount As Decimal = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/firstcreditlimitamount", FORM_XML_MAIN_KEY)), 0)
		Dim secondcreditlimitamount As Decimal = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/secondcreditlimitamount", FORM_XML_MAIN_KEY)), 0)

		Dim invoicetype As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/invoicetype", FORM_XML_MAIN_KEY))
		Dim invoiceremindercode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/invoiceremindercode", FORM_XML_MAIN_KEY))
		Dim conditionalcash As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/conditionalcash", FORM_XML_MAIN_KEY))

		Dim vattabable As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vattabable", FORM_XML_MAIN_KEY)), True)
		Dim warnbycreditlimitexceeded As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/warnbycreditlimitexceeded", FORM_XML_MAIN_KEY)), False)
		Dim customernotuse As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/customernotuse", FORM_XML_MAIN_KEY)), False)

		Try
			Me.txtFirstcreditlimitamount.Text = firstcreditlimitamount
			Me.txtSecondcreditlimitamount.Text = secondcreditlimitamount

			Me.cboInvoicetype.Text = If(String.IsNullOrWhiteSpace(invoicetype), "R", invoicetype)
			Me.cboInvoiceremindercode.Text = If(String.IsNullOrWhiteSpace(invoiceremindercode), "A", invoiceremindercode)
			Me.cboConditionalcash.Text = conditionalcash

			Me.chkVattabable.Checked = vattabable
			Me.chkWarnbycreditlimitexceeded.Checked = warnbycreditlimitexceeded
			Me.chkCustomernotuse.Checked = customernotuse


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
			Dim esendebynull As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esendebynull", FORM_XML_MAIN_KEY))
			Dim essuvacode As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/essuvacode", FORM_XML_MAIN_KEY))
			Dim escalcferienway As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/escalcferienway", FORM_XML_MAIN_KEY))
			Dim escalc13lohnway As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/escalc13lohnway", FORM_XML_MAIN_KEY))
			Dim eszeit As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/eszeit", FORM_XML_MAIN_KEY))
			Dim esort As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esort", FORM_XML_MAIN_KEY))
			Dim esvertrag As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esvertrag", FORM_XML_MAIN_KEY))
			Dim esverleih As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esverleih", FORM_XML_MAIN_KEY))

			Dim esreportsnotprint As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esreportsnotprint", FORM_XML_MAIN_KEY)), 2)

			Dim setsalarydatetotodayines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/setsalarydatetotodayines", FORM_XML_MAIN_KEY)), False)
			Dim selectadvisorkst As Boolean? = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/selectadvisorkst", FORM_XML_MAIN_KEY)), False)

			' Data...
			Me.txt_ESEndeByNull.Text = esendebynull
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
			Dim copykstintoreportline As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/copykstintoreportline", FORM_XML_MAIN_KEY)), False)
			Dim hoursroundkind As Integer = ParseToInteger(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/hoursroundkind", FORM_XML_MAIN_KEY)), 1)

			Dim strQuery As String = "//Report/matrixprintername"
			Dim matrixprintername As String = sp_Utility.GetXMLValueByQueryWithFilename(m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr), strQuery, Nothing)

			' Data...
			Me.chkcopykstintoreportline.Checked = copykstintoreportline
			Me.cbohoursroundkind.EditValue = If(hoursroundkind = 1, m_Translate.GetSafeTranslationValue("Minuten (genau)"), m_Translate.GetSafeTranslationValue("2-stellige Dezimal (nicht genau!)"))
			' Datamatrixprinter
			Me.chkUserMatrixPrinter.EditValue = matrixprintername


			' AdvacePayment
			Dim advancepaymentdefaultpaymenttype As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentdefaultpaymenttype", FORM_XML_MAIN_KEY))
			Dim advancepaymentwithfee As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentwithfee", FORM_XML_MAIN_KEY)), False)
			Dim setpayoutdatetotodayinadvancepayment As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/setpayoutdatetotodayinadvancepayment", FORM_XML_MAIN_KEY)), False)
			Dim advancepaymentpaymentreason As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentpaymentreason", FORM_XML_MAIN_KEY))

			Dim advancepaymentprintaftercreate8900and8930 As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentprintaftercreate8900and8930", FORM_XML_MAIN_KEY)), True)
			Dim advancepaymentprintaftercreate8920 As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentprintaftercreate8920", FORM_XML_MAIN_KEY)), True)
			Dim advancepaymentopenformaftercreate As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/advancepaymentopenformaftercreate", FORM_XML_MAIN_KEY)), True)

			' Data...
			Me.cboadvancepaymentdefaultpaymenttype.Text = advancepaymentdefaultpaymenttype
			Me.chkadvancepaymentwithfee.Checked = advancepaymentwithfee
			Me.chksetpayoutdatetotodayinadvancepayment.Checked = setpayoutdatetotodayinadvancepayment

			Me.txtadvancepaymentpaymentreason.Text = If(String.IsNullOrWhiteSpace(advancepaymentpaymentreason), "à Konto", advancepaymentpaymentreason)

			Me.chkadvancepaymentprintaftercreate8900and8930.Checked = advancepaymentprintaftercreate8900and8930
			Me.chkadvancepaymentprintaftercreate8920.Checked = advancepaymentprintaftercreate8920
			Me.chkadvancepaymentopenformaftercreate.Checked = advancepaymentopenformaftercreate


			' payroll
			Dim payrollopenprintformaftercreate As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/payrollopenprintformaftercreate", FORM_XML_MAIN_KEY)), True)

			' Data...
			Me.chkpayrollopenprintformaftercreate.Checked = payrollopenprintformaftercreate


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
		Dim create3mahnasuntilnotpaid As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/create3mahnasuntilnotpaid", m_InvoiceSetting)), False)
		Dim printezwithmahn As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/printezwithmahn", m_InvoiceSetting)), False)
		Dim printguonmahnung As Boolean = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/printguonmahnung", m_InvoiceSetting)), False)

		Dim mwstnr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstnr", m_InvoiceSetting))
		Dim mwstsatz As Decimal = ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/mwstsatz", m_InvoiceSetting)), 8)

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
			Me.chkcreate3mahnasuntilnotpaid.Checked = create3mahnasuntilnotpaid
			Me.chkprintezwithmahn.Checked = printezwithmahn
			Me.chkprintguonmahnung.Checked = printguonmahnung

			Me.txtmwstnr.EditValue = mwstnr
			Me.txtmwstsatz.EditValue = mwstsatz

			Me.txtfactoringcustomernumber.EditValue = factoringcustomernumber
			Me.txtinvoicezipfilename.EditValue = invoicezipfilename

			Me.txtmahnspesenab.EditValue = mahnspesenab
			Me.txtmahnspesenchf.EditValue = mahnspesenchf
			Me.txtverzugszinsdaysafter.EditValue = verzugszinsdaysafter
			Me.txtverzugszinsabchf.EditValue = verzugszinsabchf
			Me.txtverzugszinspercent.EditValue = verzugszinspercent


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


#Region "Feldbezeichnungen"

	Private Function LoadFieldLabelValue() As Boolean
		Dim success As Boolean = True

		Dim strQuery As String
		Dim strNodeBez As String = String.Empty

		Try
			' Common
			strNodeBez = "BeraterIn"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_BeraterIn.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "BeraterIn")

			strNodeBez = "1. Kst."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_1KST.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "1. Kst.")

			strNodeBez = "2. Kst."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_2KST.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "2. Kst.")

			strNodeBez = "3. Kst."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_3KST.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "3. Kst.")


			' Employee
			strNodeBez = "Telefon privat"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_Telefonprivat.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Telefon (P)")

			strNodeBez = "Fax privat"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_Faxprivat.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Fax (P)")

			strNodeBez = "Telefon G."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_TelefonG.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Telefon (G)")

			strNodeBez = "Fax G."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_FaxG.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Fax (G)")

			strNodeBez = "Qualifikation"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_Qualifikation.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Qualifikation")

			strNodeBez = "Gemeinde"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_Gemeinde.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Gemeinde")

			strNodeBez = "ResAuto"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ResAuto.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "Beurteilung"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_Beurteilung.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Beurteilung")

			strNodeBez = "Sonstige Qualifikation"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_SQualifikation.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Sonstige Qualifikation")

			strNodeBez = "Kommunikationsart"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_Kommunikationsart.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Kommunikationsart")

			strNodeBez = "MAKontakt"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MAKontakt.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Kontakt")

			strNodeBez = "MA1Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA1Status.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "1. Status")

			strNodeBez = "MA2Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA2Status.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "MA_Anstellungsarten"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA_Anstellungsart.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "MA_Anstellungsarten")

			strNodeBez = "MA1Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA1Res.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "MA2Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA2Res.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "MA3Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA3Res.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "MA4Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA4Res.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "MA5Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA5Res.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "MAResLbl"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_MA_ResHeader.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")



			' Customer
			strNodeBez = "KD_1Property"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_1Property.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "1. Eigenschaft")

			strNodeBez = "KD_2Property"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_2Property.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KDKontakt"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KDKontakt.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Kontakt")

			strNodeBez = "KD1Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD1Status.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "1. Status")

			strNodeBez = "KD2Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD2Status.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KD1Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_Res1.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KD2Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_Res2.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KD3Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_Res3.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KD4Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_Res4.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KD5Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_Res5.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KDResLbl"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_ResHeader.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Reserve Felder")

			strNodeBez = "KDMwStLbl"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KDMwStLabel.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "KD_Anstellungsarten"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_KD_Anstellungsart.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "KD_Anstellungsarten")



			strNodeBez = "ZHD_Kontakt"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Kontakt.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Kontakt")

			strNodeBez = "ZHD_1State"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_1State.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "1. Status")

			strNodeBez = "ZHD_2State"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_2State.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "ZHD_Kommunikation"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Kommunikation.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Kommunikation")

			strNodeBez = "ZHD_Versand"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Versand.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Versand")

			strNodeBez = "ZHD_Res1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Res1.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "ZHD_Res2"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Res2.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "ZHD_Res3"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Res3.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "ZHD_Res4"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ZHD_Res4.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")



			' Einsatz
			strNodeBez = "es_einstufung"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_Einstufung.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Einstufung")

			strNodeBez = "es_branche"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_Branche.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Branche")

			strNodeBez = "ES_TSpesen"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_TSpesen.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Tagesspesen / Tag")

			strNodeBez = "ES_1Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_1Zusatz.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Zusatz 1")

			strNodeBez = "ES_2Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_2Zusatz.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Zusatz 2")

			strNodeBez = "ES_3Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_3Zusatz.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Zusatz 3")

			strNodeBez = "ES_4Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_4Zusatz.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Zusatz 4")

			strNodeBez = "ES_5Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_5Zusatz.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Zusatz 5")

			strNodeBez = "ES_6Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_ES_6Zusatz.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "Zusatz 6")

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


#End Region


#Region "RequiredFields"

	Private Function LoadRequiredfields() As Boolean
		Dim success As Boolean = True

		Try
			' Employee
			Dim emplyoeeadvisorselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeeadvisorselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeecontactselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeecontactselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeefstateselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeefstateselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeesstateselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeesstateselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeequalificationselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeequalificationselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeepermitselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeepermitselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeepermitdateselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeepermitdateselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeehometownselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeehometownselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeesteuerselectionifnotch As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeesteuerselectionifnotch", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeekirchensteuerselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeekirchensteuerselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim emplyoeesteuercantonselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/emplyoeesteuercantonselection", FORM_XML_REQUIREDFIELDS_KEY)), False)

			' Data
			Me.chkemplyoeeadvisorselection.Checked = emplyoeeadvisorselection
			Me.chkemplyoeecontactselection.Checked = emplyoeecontactselection
			Me.chkemplyoeefstateselection.Checked = emplyoeefstateselection
			Me.chkemplyoeesstateselection.Checked = emplyoeesstateselection
			Me.chkemplyoeequalificationselection.Checked = emplyoeequalificationselection
			Me.chkemplyoeepermitselection.Checked = emplyoeepermitselection
			Me.chkemplyoeepermitdateselection.Checked = emplyoeepermitdateselection
			Me.chkemplyoeehometownselection.Checked = emplyoeehometownselection
			Me.chkemplyoeesteuerselectionifnotch.Checked = emplyoeesteuerselectionifnotch
			Me.chkemplyoeekirchensteuerselection.Checked = emplyoeekirchensteuerselection
			Me.chkemplyoeesteuercantonselection.Checked = emplyoeesteuercantonselection



			' Vacancy
			Dim vacancygruppeselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancygruppeselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancycontactselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancycontactselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancystateselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancystateselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancyjobpostcodeselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyjobpostcodeselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancyjobcityselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyjobcityselection", FORM_XML_REQUIREDFIELDS_KEY)), False)

			Dim vacancyvorspannselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyvorspannselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancyactivityselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyactivityselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancyrequirementselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyrequirementselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancyweofferselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyweofferselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancycontonselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancycontonselection", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim vacancyregionselection As String = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/vacancyregionselection", FORM_XML_REQUIREDFIELDS_KEY)), False)

			' Data
			Me.chkvacancygruppeselection.Checked = vacancygruppeselection
			Me.chkvacancycontactselection.Checked = vacancycontactselection
			Me.chkvacancystateselection.Checked = vacancystateselection
			Me.chkvacancyjobpostcodeselection.Checked = vacancyjobpostcodeselection
			Me.chkvacancyjobcityselection.Checked = vacancyjobcityselection

			Me.chkvacancyvorspannselection.Checked = vacancyvorspannselection
			Me.chkvacancyactivityselection.Checked = vacancyactivityselection
			Me.chkvacancyrequirementselection.Checked = vacancyrequirementselection
			Me.chkvacancyweofferselection.Checked = vacancyweofferselection
			Me.chkvacancycontonselection.Checked = vacancycontonselection
			Me.chkvacancyregionselection.Checked = vacancyregionselection



			' Einsatz
			Dim gavselectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/gavselectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim kst1selectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/kst1selectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim kst2selectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/kst2selectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim esadvisorselectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esadvisorselectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim timeselectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/timeselectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim eseinstufungselectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/eseinstufungselectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)
			Dim esbrancheselectionines As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esbrancheselectionines", FORM_XML_REQUIREDFIELDS_KEY)), False)


			' Data
			Me.chkkst1selectionines.Checked = kst1selectionines
			Me.chkkst2selectionines.Checked = kst2selectionines
			Me.chktimeselectionines.Checked = timeselectionines
			Me.chkgavselectionines.Checked = gavselectionines
			Me.chkeseinstufungselectionines.Checked = eseinstufungselectionines
			Me.chkesbrancheselectionines.Checked = esbrancheselectionines



			' Customer
			Dim customeradvisorselection As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/customeradvisorselection", FORM_XML_REQUIREDFIELDS_KEY)), False)

			' Data
			Me.chkcustomeradvisorselection.Checked = customeradvisorselection


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

#End Region


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

	Private Function LoadWebServiceInfos() As Boolean
		Dim success As Boolean = True


		Try
			Me.txtWebserviceJob.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicejobdatabase", m_MandantSetting))
			If Me.txtWebserviceJob.Text = String.Empty Then Me.txtWebserviceJob.Text = "http://asmx.domain.com/wsSPS_services/SPModulUtil.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebserviceBank.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicebankdatabase", m_MandantSetting))
			If Me.txtWebserviceBank.Text = String.Empty Then Me.txtWebserviceBank.Text = "http://asmx.domain.com/wsSPS_services/SPBankUtil.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebServiceQST.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webserviceqstdatabase", m_MandantSetting))
			If Me.txtWebServiceQST.Text = String.Empty Then Me.txtWebServiceQST.Text = "http://asmx.domain.com/wsSPS_services/SPModulUtil.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebServiceGAV.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicegavdatabase", m_MandantSetting))
			If Me.txtWebServiceGAV.Text = String.Empty Then Me.txtWebServiceGAV.Text = "http://asmx.domain.com/spgav_services/SPGAV2012Data.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebServiceGAVutility.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicegavutility", m_MandantSetting))
			If Me.txtWebServiceGAVutility.Text = String.Empty Then Me.txtWebServiceGAVutility.Text = "http://asmx.domain.com/spwebservice/SPGAVData.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebservicewosEmployee.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicewosemployeedatabase", m_MandantSetting))
			If Me.txtWebservicewosEmployee.Text = String.Empty Then Me.txtWebservicewosEmployee.Text = "http://asmx.domain.com/spwebservice/spmafunctions.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebservicewosCustomer.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicewoscustomer", m_MandantSetting))
			If Me.txtWebservicewosCustomer.Text = String.Empty Then Me.txtWebservicewosCustomer.Text = "http://asmx.domain.com/spwebservice/SPKDFunctions.asmx".ToLower
		Catch ex As Exception

		End Try
		Try
			Me.txtWebservicewosVacancies.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicewosvacancies", m_MandantSetting))
			If Me.txtWebservicewosVacancies.Text = String.Empty Then Me.txtWebservicewosVacancies.Text = "http://asmx.domain.com/wsSPS_services/SPInternVacancies.asmx".ToLower
		Catch ex As Exception

		End Try

		Try
			Me.txtWebserviceJobCH.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicejobchvacancies", m_MandantSetting))
			If Me.txtWebserviceJobCH.Text = String.Empty Then Me.txtWebserviceJobCH.Text = "http://asmx.domain.com/wsSPS_services/SPJobsCHVacancies.asmx".ToLower
		Catch ex As Exception

		End Try

		Try
			Me.txtWebserviceOstJob.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webserviceostjobvacancies", m_MandantSetting))

		Catch ex As Exception

		End Try

		Try
			Me.txtWebserviceSuedost.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicesuedostvacancies", m_MandantSetting))

		Catch ex As Exception

		End Try

		Try
			Me.txtWebservicepayment.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicepaymentservices", m_MandantSetting))
			If Me.txtWebservicepayment.Text = String.Empty Then Me.txtWebservicepayment.Text = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx".ToLower

		Catch ex As Exception

		End Try

		Try
			Me.txtWebserviceeCall.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webserviceecall", m_MandantSetting))
			'If Me.txtWebserviceeCall.Text = String.Empty Then Me.txtWebserviceeCall.Text = "http://www1.ecall.ch/ecallwebservice/eCall.asmx".ToLower

		Catch ex As Exception

		End Try

		Try
			Me.txtWebserviceTaxDb.Text = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}Interfaces/webservices/webservicetaxinfoservices", m_MandantSetting))
			If Me.txtWebserviceTaxDb.Text = String.Empty Then Me.txtWebserviceTaxDb.Text = "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx".ToLower

		Catch ex As Exception

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
			success = success AndAlso SaveUserfieldData()

			' invoice defaultfields
			success = success AndAlso SaveInvocieSettings()

			' payroll setting
			success = success AndAlso SavePayrollSetting()

			' labels
			success = success AndAlso SaveFieldLabelValue()

			' default field data
			success = success AndAlso SaveDefaultFieldData()

			' requiredfields
			success = success AndAlso SaveRequiredFieldData()

			' Webservices
			success = success AndAlso SaveMDLicenseData()


			' erst wenn in der DB alles OK ist...
			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
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

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/ref10forfactoring", m_MandantSetting), If(chkref10forfactoring.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/ezonsepratedpage", m_MandantSetting), If(chkezonsepratedpage.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/setfakdatetoendofreportmonth", m_MandantSetting), If(chksetfakdatetoendofreportmonth.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/create3mahnasuntilnotpaid", m_MandantSetting), If(chkcreate3mahnasuntilnotpaid.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/printezwithmahn", m_MandantSetting), If(chkprintezwithmahn.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/printguonmahnung", m_MandantSetting), If(chkprintguonmahnung.Checked, "true", "false"))

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/mahnspesenab", m_MandantSetting), txtmahnspesenab.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/mahnspesenchf", m_MandantSetting), txtmahnspesenchf.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/verzugszinsdaysafter", m_MandantSetting), txtverzugszinsdaysafter.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/verzugszinspercent", m_MandantSetting), txtverzugszinspercent.EditValue)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/verzugszinsabchf", m_MandantSetting), txtverzugszinsabchf.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/factoringcustomernumber", m_MandantSetting), txtfactoringcustomernumber.EditValue)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Debitoren/invoicezipfilename", m_MandantSetting), txtinvoicezipfilename.EditValue)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
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
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function SaveFieldLabelValue() As Boolean
		Dim success As Boolean = True
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim strMainKey As String = "//Control[@Name='{0}']"
		Dim searchKey As String = String.Empty

		xDoc.Load(m_MandantFormXMLFile)

		Try
			searchKey = "BeraterIn"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_BeraterIn.Text)

			searchKey = "Telefon privat"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_Telefonprivat.Text)

			searchKey = "Fax privat"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_Faxprivat.Text)

			searchKey = "Telefon G."
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_TelefonG.Text)

			searchKey = "Fax G."
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_FaxG.Text)

			searchKey = "Qualifikation"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_Qualifikation.Text)

			searchKey = "Gemeinde"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_Gemeinde.Text)

			searchKey = "ResAuto"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ResAuto.Text)

			searchKey = "Beurteilung"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_Beurteilung.Text)

			searchKey = "Sonstige Qualifikation"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_SQualifikation.Text)

			searchKey = "Kommunikationsart"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_Kommunikationsart.Text)

			searchKey = "MAKontakt"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MAKontakt.Text)

			searchKey = "MA1Status"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA1Status.Text)

			searchKey = "MA2Status"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA2Status.Text)

			searchKey = "MA_Anstellungsarten"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA_Anstellungsart.Text)

			searchKey = "MA1Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA1Res.Text)

			searchKey = "MA2Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA2Res.Text)

			searchKey = "MA3Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA3Res.Text)

			searchKey = "MA4Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA4Res.Text)

			searchKey = "MA5Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA5Res.Text)

			searchKey = "MAResLbl"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_MA_ResHeader.Text)


			searchKey = "KD_1Property"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_1Property.Text)

			searchKey = "KD_2Property"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_2Property.Text)

			searchKey = "KDKontakt"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KDKontakt.Text)

			searchKey = "KD1Status"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD1Status.Text)

			searchKey = "KD2Status"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD2Status.Text)

			searchKey = "KD_Anstellungsarten"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_Anstellungsart.Text)

			searchKey = "KD1Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_Res1.Text)

			searchKey = "KD2Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_Res2.Text)

			searchKey = "KD3Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_Res3.Text)

			searchKey = "KD4Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_Res4.Text)

			searchKey = "KD5Res"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_Res5.Text)

			searchKey = "KDResLbl"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KD_ResHeader.Text)

			searchKey = "KDMwStLbl"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_KDMwStLabel.Text)


			searchKey = "ZHD_Res1"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Res1.Text)

			searchKey = "ZHD_Res2"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Res2.Text)

			searchKey = "ZHD_Res3"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Res3.Text)

			searchKey = "ZHD_Res4"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Res4.Text)

			searchKey = "ZHD_Kontakt"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Kontakt.Text)

			searchKey = "ZHD_1State"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_1State.Text)

			searchKey = "ZHD_2State"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_2State.Text)

			searchKey = "ZHD_Kommunikation"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Kommunikation.Text)

			searchKey = "ZHD_Versand"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ZHD_Versand.Text)



			searchKey = "es_einstufung"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_Einstufung.Text)

			searchKey = "es_branche"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_Branche.Text)

			searchKey = "ES_TSpesen"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_TSpesen.Text)

			searchKey = "ES_1Zusatz"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_1Zusatz.Text)

			searchKey = "ES_2Zusatz"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_2Zusatz.Text)

			searchKey = "ES_3Zusatz"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_3Zusatz.Text)

			searchKey = "ES_4Zusatz"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_4Zusatz.Text)

			searchKey = "ES_5Zusatz"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_5Zusatz.Text)

			searchKey = "ES_6Zusatz"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_ES_6Zusatz.Text)

			searchKey = "1. Kst."
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_1KST.Text)

			searchKey = "2. Kst."
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_2KST.Text)

			searchKey = "3. Kst."
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_3KST.Text)


			xDoc.Save(m_MandantFormXMLFile)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
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

				strKey = "employeecontact"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeecontact.Text)
				strKey = "emplyoeefstate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemplyoeefstate.Text)
				strKey = "employeesstate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeesstate.Text)
				strKey = "employeesecsuvacode"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, cboemployeesecsuvacode.Text)

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



				strKey = "esendebynull".ToLower
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, txt_ESEndeByNull.Text)

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

				strKey = "payrollopenprintformaftercreate"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkpayrollopenprintformaftercreate.Checked, "true", "false"))


			End If

			xDoc.Save(m_MandantFormXMLFile)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function

	Private Function SaveRequiredFieldData() As Boolean
		Dim success As Boolean = True
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		xDoc.Load(m_MandantFormXMLFile)

		Try
			xNode = xDoc.SelectSingleNode("*//requiredfields")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "requiredfields", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				Dim strKey As String = String.Empty

				strKey = "gavselectionines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkgavselectionines.Checked, "true", "false"))

				strKey = "kst1selectionines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkkst1selectionines.Checked, "true", "false"))

				strKey = "kst2selectionines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkkst2selectionines.Checked, "true", "false"))

				strKey = "timeselectionines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chktimeselectionines.Checked, "true", "false"))

				strKey = "eseinstufungselectionines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkeseinstufungselectionines.Checked, "true", "false"))

				strKey = "esbrancheselectionines"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkesbrancheselectionines.Checked, "true", "false"))


				' Employee

				strKey = "emplyoeeadvisorselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeeadvisorselection.Checked, "true", "false"))

				strKey = "emplyoeequalificationselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeequalificationselection.Checked, "true", "false"))

				strKey = "emplyoeefstateselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeefstateselection.Checked, "true", "false"))

				strKey = "emplyoeesstateselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeesstateselection.Checked, "true", "false"))

				strKey = "emplyoeecontactselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeecontactselection.Checked, "true", "false"))

				strKey = "emplyoeepermitselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeepermitselection.Checked, "true", "false"))

				strKey = "emplyoeepermitdateselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeepermitdateselection.Checked, "true", "false"))

				strKey = "emplyoeehometownselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeehometownselection.Checked, "true", "false"))

				strKey = "emplyoeesteuerselectionifnotch"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeesteuerselectionifnotch.Checked, "true", "false"))

				strKey = "emplyoeekirchensteuerselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeekirchensteuerselection.Checked, "true", "false"))

				strKey = "emplyoeesteuercantonselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkemplyoeesteuercantonselection.Checked, "true", "false"))

				' customer
				strKey = "customeradvisorselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkcustomeradvisorselection.Checked, "true", "false"))

				' Vacancy

				strKey = "vacancygruppeselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancygruppeselection.Checked, "true", "false"))

				strKey = "vacancycontactselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancycontactselection.Checked, "true", "false"))

				strKey = "vacancystateselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancystateselection.Checked, "true", "false"))

				strKey = "vacancyjobpostcodeselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyjobpostcodeselection.Checked, "true", "false"))

				strKey = "vacancyjobcityselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyjobcityselection.Checked, "true", "false"))

				strKey = "vacancyvorspannselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyvorspannselection.Checked, "true", "false"))

				strKey = "vacancyactivityselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyactivityselection.Checked, "true", "false"))

				strKey = "vacancyrequirementselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyrequirementselection.Checked, "true", "false"))

				strKey = "vacancyweofferselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyweofferselection.Checked, "true", "false"))

				strKey = "vacancycontonselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancycontonselection.Checked, "true", "false"))

				strKey = "vacancyregionselection"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(chkvacancyregionselection.Checked, "true", "false"))

			End If
			xDoc.Save(m_MandantFormXMLFile)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


	Private Function SaveMDLicenseData() As Boolean
		Dim success As Boolean = True

		Try

			' Set Defaultsetting
			If Me.txtWebserviceJob.Text = String.Empty Then Me.txtWebserviceJob.Text = "http://asmx.domain.com/wsSPS_services/SPModulUtil.asmx".ToLower
			If Me.txtWebserviceBank.Text = String.Empty Then Me.txtWebserviceBank.Text = "http://asmx.domain.com/wsSPS_services/SPBankUtil.asmx".ToLower
			If Me.txtWebServiceQST.Text = String.Empty Then Me.txtWebServiceQST.Text = "http://asmx.domain.com/wsSPS_services/SPModulUtil.asmx".ToLower
			If Me.txtWebServiceGAV.Text = String.Empty Then Me.txtWebServiceGAV.Text = "http://asmx.domain.com/spgav_services/SPGAV2012Data.asmx".ToLower
			If Me.txtWebServiceGAVutility.Text = String.Empty Then Me.txtWebServiceGAVutility.Text = "http://asmx.domain.com/spwebservice/SPGAVData.asmx".ToLower
			If Me.txtWebservicewosEmployee.Text = String.Empty Then Me.txtWebservicewosEmployee.Text = "http://asmx.domain.com/spwebservice/spmafunctions.asmx".ToLower
			If Me.txtWebservicewosCustomer.Text = String.Empty Then Me.txtWebservicewosCustomer.Text = "http://asmx.domain.com/spwebservice/SPKDFunctions.asmx".ToLower
			If Me.txtWebservicewosVacancies.Text = String.Empty Then Me.txtWebservicewosVacancies.Text = "http://asmx.domain.com/wsSPS_services/SPInternVacancies.asmx".ToLower
			If Me.txtWebserviceJobCH.Text = String.Empty Then Me.txtWebserviceJobCH.Text = "http://asmx.domain.com/wsSPS_services/SPJobsCHVacancies.asmx".ToLower

			If Me.txtWebservicepayment.Text = String.Empty Then Me.txtWebservicepayment.Text = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx".ToLower
			'If Me.txtWebserviceeCall.Text = String.Empty Then Me.txtWebserviceeCall.Text = "http://www1.ecall.ch/ecallwebservice/eCall.asmx".ToLower

			If Me.txtWebserviceTaxDb.Text = String.Empty Then Me.txtWebserviceTaxDb.Text = "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx".ToLower

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		' Save data
		Try

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicejobdatabase", m_MandantSetting), Me.txtWebserviceJob.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicebankdatabase", m_MandantSetting), Me.txtWebserviceBank.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webserviceqstdatabase", m_MandantSetting), Me.txtWebServiceQST.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicegavdatabase", m_MandantSetting), Me.txtWebServiceGAV.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicegavutility", m_MandantSetting), Me.txtWebServiceGAVutility.Text)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicewosemployeedatabase", m_MandantSetting), Me.txtWebservicewosEmployee.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicewoscustomer", m_MandantSetting), Me.txtWebservicewosCustomer.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicewosvacancies", m_MandantSetting), Me.txtWebservicewosVacancies.Text)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicejobchvacancies", m_MandantSetting), Me.txtWebserviceJobCH.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webserviceostjobvacancies", m_MandantSetting), Me.txtWebserviceOstJob.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicesuedostvacancies", m_MandantSetting), Me.txtWebserviceSuedost.Text)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicepaymentservices", m_MandantSetting), Me.txtWebservicepayment.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webserviceecall", m_MandantSetting), Me.txtWebserviceeCall.Text)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}Interfaces/webservices/webservicetaxinfoservices", m_MandantSetting), Me.txtWebserviceTaxDb.Text)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function




	Private Function SaveUserfieldData() As Boolean
		Dim success As Boolean = True
		'Dim xElmntFamily As XmlElement = Nothing

		'Dim xEle As XElement = XElement.Load(m_MandantUserXMLFile)
		'Try

		'	Dim MyNode = xEle.Elements("Layouts").Elements("Report").Elements("matrixprintername").ToList()
		'	If MyNode.Count > 0 Then
		'		For Each cEle As XElement In MyNode
		'			cEle.ReplaceNodes(String.Format("{0}", chkUserMatrixPrinter.EditValue))
		'		Next cEle

		'	Else
		'		xEle.AddFirst(New XElement("Layouts", _
		'																 New XElement("Report", _
		'																							New XElement("matrixprintername", chkUserMatrixPrinter.EditValue))))
		'	End If

		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		xDoc.Load(m_MandantUserXMLFile)

		Try
			xNode = xDoc.SelectSingleNode("*//Report")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Report", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If

			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				Dim strKey As String = String.Empty

				strKey = "matrixprintername"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, chkUserMatrixPrinter.EditValue)

			End If

			xDoc.Save(m_MandantUserXMLFile)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			IsDataValid = False

		End Try

		Return success

	End Function


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





End Class
