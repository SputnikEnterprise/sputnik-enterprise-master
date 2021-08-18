
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


Public Class ucWebServices


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

			' Webservices
			success = success And LoadWebservicesData()

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


	Private Function LoadWebservicesData() As Boolean
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

			' Webservices
			success = success AndAlso SaveWebservicesData()


			' erst wenn in der DB alles OK ist...
			m_SuppressUIEvents = suppressUIEventState


		Catch ex As Exception
      m_Logger.LogError(String.Format("ucWebservices: {0}", ex.ToString))
      success = False

    Finally

		End Try
		IsDataValid = success

		Return success

	End Function



#Region "save data"

	Private Function SaveWebservicesData() As Boolean
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
      m_Logger.LogError(String.Format("ucWebservices: {0}", ex.ToString))
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
