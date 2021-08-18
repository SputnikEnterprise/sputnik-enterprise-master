
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Threading
Imports SP.Internal.Automations.Settings_
Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService
Imports SP.Internal.Automations.SPeCallWebService
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports DevExpress.XtraEditors
Imports System.Net

Public Class frmMain

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_eCallWebServiceUri As String

	Private m_AccountName As String
	Private m_AccountPassword As String


#Region "Private Consts"

	'Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "https://soap.ecall.ch/eCall.asmx"

	Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

	Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-contact-subject"
	Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY As String = "MD_{0}/Templates/ customer-contact-body"

	Public Const SPUTNIK_ECALL_SMS As String = "ECALL_SMSCREDIT"

#End Region

#Region "Privte Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		m_InitializationData = _setting
		m_SettingsManager = New SettingsManager

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_ClsProgSetting = New SPProgUtility.ClsProgSettingPath
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI

		m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
			m_AccountName = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, m_InitializationData.MDData.MDNr)))
			m_AccountPassword = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, m_InitializationData.MDData.MDNr)))

			m_eCallWebServiceUri = MANDANT_XML_SETTING_SPUTNIK_ECALL_URI

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Reset()

	End Sub

#End Region


	Public Function GetState(ByVal message As ShortMessage) As ShortMessage
		Dim result As ShortMessage = message
		Dim m_eCallService As SPeCallWebService.eCallSoapClient

		m_eCallService = New SPeCallWebService.eCallSoapClient
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
		m_eCallService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_eCallWebServiceUri)

		Dim response As SPeCallWebService.StatusResponse = m_eCallService.GetStateBasic(m_AccountName, m_AccountPassword, message.JobId, Nothing)

		Try

			message.PointsUsed = response.JobResponse.PointsUsed
			If response.ServiceResponse.ResponseCode <> 0 Then
				message.ResponseCode = response.ServiceResponse.ResponseCode
				message.ResponseText = response.ServiceResponse.ResponseText
			Else
				message.SendState = response.JobResponse.SendState
				message.ErrorState = response.JobResponse.ErrorState
				message.FinishDate = response.JobResponse.FinishDate
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)
		End Try

		Return result

	End Function

	Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
		Dim message As ShortMessage = Nothing
		Dim jobID As String = "1BZDKG22ZG7HT4F_PKX1"    'String.Empty

		message = New ShortMessage With {.JobId = jobID}
		GetState(message)


	End Sub

	Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
		Dim Customer_ID As String = m_InitializationData.MDData.MDGuid

		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)

		Dim resultVersion = baseTableData.LoadIBANLibraryVersionOverWebService()
		If resultVersion Is Nothing Then Return
		Dim msg As String = String.Empty
		msg = String.Format("IBAN-Version: {0} {1} >>> {2}", resultVersion.MajorVersion, resultVersion.MinorVersion, resultVersion.ValidUntil)

		m_UtilityUI.ShowInfoDialog(msg)

	End Sub


	Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
		Dim o2Open As New frmeCalllogs(m_InitializationData)
		o2Open.LoadData()

		o2Open.Show()
		o2Open.BringToFront()

	End Sub

	Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
		Dim o2Open As New frmQuellensteuer

		o2Open.Show()
		o2Open.BringToFront()

	End Sub

	Private Sub OnbtnRefreshEmployeeGeoData_Click(sender As Object, e As EventArgs) Handles btnRefreshEmployeeGeoData.Click

		RefreshEmployeeGeoDataViaWebService()

	End Sub

	Private Sub OnbtnRefreshCustomerGeoData_Click(sender As Object, e As EventArgs) Handles btnRefreshCustomerGeoData.Click

		RefreshCustomerGeoDataViaWebService()

	End Sub

	Private Sub OnbtnRefreshEmployeeCountryCodeData_Click(sender As Object, e As EventArgs) Handles btnRefreshEmployeeCountryCodeData.Click

		RefreshEmployeeCountryDataViaWebService()

	End Sub

	Private Sub OnbtnRefreshCustomerCountryCodeData_Click(sender As Object, e As EventArgs) Handles btnRefreshCustomerCountryCodeData.Click

		RefreshCustomerCountryDataViaWebService()

	End Sub

	Private Sub OnbtnUpdateEmployeeTaxCoummunity_Click(sender As Object, e As EventArgs) Handles btnUpdateEmployeeTaxCoummunity.Click

		RefreshEmployeeTaxCommunityDataViaWebService()

	End Sub

	Private Sub btnUpdateWOSMySetting_Click(sender As Object, e As EventArgs) Handles btnUpdateWOSMySetting.Click
		Try

			Dim o2Open As New WOSUtilityUI.frmSaveWOSData(m_InitializationData)

			o2Open.Show()
			o2Open.BringToFront()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnbtnReadPDFFields_Click(sender As Object, e As EventArgs) Handles btnReadPDFFields.Click
		Dim frmPDF As New frmPDFFormData(m_InitializationData)

		frmPDF.Show()
		frmPDF.BringToFront()

	End Sub


End Class


Public Class ShortMessage


	Public Property ReceiverId As Integer

	Public Property Address As String
	Public Property Message As String

	Public Property JobId As String

	Public Property ResponseCode As Long
	Public Property ResponseText As String

	Public Property SendState As Long
	Public Property ErrorState As Long

	Public Property FinishDate As Date

	Public Property PointsUsed As Double
	Public Property AnswerAddress As String


	Sub New()
		ResponseCode = 0
		SendState = 0
		ErrorState = 0
		ResponseText = String.Empty
		PointsUsed = 0.0
	End Sub

	Public Sub UpdateStatus(ByVal status As ShortMessage)
		If status.ResponseCode <> -1 Then
			ResponseCode = status.ResponseCode
		End If

		If status.SendState <> -1 Then
			SendState = status.SendState
		End If

		If status.ErrorState <> -1 Then
			ErrorState = status.ErrorState
		End If

		If Not String.IsNullOrEmpty(status.ResponseText) Then
			ResponseText = status.ResponseText
		End If

	End Sub

	Public Function Clone() As ShortMessage
		Return DirectCast(Me.MemberwiseClone(), ShortMessage)
	End Function


End Class
