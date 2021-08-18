
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



Public Class ucRequiredFields


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

			success = success And LoadRequiredfields()


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

			' requiredfields
			success = success AndAlso SaveRequiredFieldData()


			' erst wenn in der DB alles OK ist...
			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucRequiredFields: {0}", ex.ToString))
      success = False

    Finally

		End Try
		IsDataValid = success

		Return success

	End Function



#Region "save data"


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
      m_Logger.LogError(String.Format("ucRequiredFields: {0}", ex.ToString))
      success = False

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
