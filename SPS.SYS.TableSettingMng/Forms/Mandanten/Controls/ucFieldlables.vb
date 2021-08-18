
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



Public Class ucFieldlables

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
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub
		m_Year = m_InitializationData.MDData.MDYear
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Me.xtabFieldLabel.SelectedTabPage = xtabMAFieldLabel

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

			success = success And LoadFieldLabelValue()

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

			strNodeBez = "lblEmployeeSAdressBemerk1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txtlblEmployeeSAdressBemerk1.EditValue = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "lblEmployeeSAdressBemerk2"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txtlblEmployeeSAdressBemerk2.EditValue = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "lblEmployeeSAdressBemerk3"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txtlblEmployeeSAdressBemerk3.EditValue = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "lblEmployeeSAdressBemerk4"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txtlblEmployeeSAdressBemerk4.EditValue = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")


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


			' LAStamm
			strNodeBez = "LAReserve1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_LA_Res1.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "LAReserve1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_LA_Res2.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "LAReserve1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_LA_Res3.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "LAReserve1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_LA_Res4.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

			strNodeBez = "LAReserve1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strNodeBez)
			Me.txt_LA_Res5.Text = m_ProgPath.GetXMLValueByQueryWithFilename(m_MandantFormXMLFile, strQuery, "")

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


#End Region


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

			' labels
			success = success AndAlso SaveFieldLabelValue()


			' erst wenn in der DB alles OK ist...
			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucFieldLables: {0}", ex.ToString))
      success = False

    Finally

		End Try
		IsDataValid = success

		Return success

	End Function



#Region "save data"


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

			searchKey = "lblEmployeeSAdressBemerk1"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txtlblEmployeeSAdressBemerk1.EditValue)

			searchKey = "lblEmployeeSAdressBemerk2"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txtlblEmployeeSAdressBemerk2.EditValue)

			searchKey = "lblEmployeeSAdressBemerk3"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txtlblEmployeeSAdressBemerk3.EditValue)

			searchKey = "lblEmployeeSAdressBemerk4"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txtlblEmployeeSAdressBemerk4.EditValue)


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





			searchKey = "LAReserve1"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_LA_Res1.Text)

			searchKey = "LAReserve2"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_LA_Res2.Text)

			searchKey = "LAReserve3"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_LA_Res3.Text)

			searchKey = "LAReserve4"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_LA_Res4.Text)

			searchKey = "LAReserve5"
			AddOrUpdateFieldLabelNode(xDoc, searchKey, strMainKey, txt_LA_Res5.Text)



			xDoc.Save(m_MandantFormXMLFile)


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucFieldLables: {0}", ex.ToString))
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
