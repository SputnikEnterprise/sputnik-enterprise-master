
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.CommonXmlUtility
Imports System.Threading
Imports SP.Internal.Automations
Imports System.Net

Public Class ClsSMSStart

#Region "Private Consts"

	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "https://soap.ecall.ch/eCall.asmx" ' "MD_{0}/Interfaces/webservices/webserviceecall"
	Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
	Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx" ' "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"

	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

	Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-contact-subject"
	Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY As String = "MD_{0}/Templates/ customer-contact-body"

	Public Const SPUTNIK_ECALL_SMS As String = "ECALL_SMSCREDIT"

#End Region

#Region "Private Fields"

	Protected m_UtilityUI As UtilityUI
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_xml As New ClsXML
	Private m_md As Mandant
	Private m_path As ClsProgPath
	Private m_common As CommonSetting

	Private m_MandantSettingsXml As SettingsXml

	'''<summary>
	'''The mandant.
	'''</summary>
	Private m_MandantData As Mandant

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_eCallWebServiceUri As String

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_PaymentUtilWebServiceUri As String

	'''<summary>
	'''translate values
	'''</summary>
	''' <remarks></remarks>
	Private m_translate As TranslateValues

	Private m_eCallService As SPeCallWebService.eCallSoapClient

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath

	Private m_AccountName As String
	Private m_AccountPassword As String

#End Region

#Region "Constructor"

	Public Sub New(ByVal _Setting As InitializeClass)

		If _Setting.MDData Is Nothing Then
			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		Else
			ModulConstants.MDData = _Setting.MDData
			ModulConstants.UserData = _Setting.UserData
			ModulConstants.ProsonalizedData = _Setting.ProsonalizedData
			ModulConstants.TranslationData = _Setting.TranslationData

		End If

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)

		m_translate = New TranslateValues
		m_MandantData = New Mandant

		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

		Try

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
			m_AccountName = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, ModulConstants.MDData.MDNr)))
			m_AccountPassword = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, ModulConstants.MDData.MDNr)))

			LoadPrivderData()
			m_eCallWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format("MD_{0}/Interfaces/webservices/webserviceecall", ModulConstants.MDData.MDNr))
			m_eCallWebServiceUri = MANDANT_XML_SETTING_SPUTNIK_ECALL_URI

			Dim domainName = ModulConstants.MDData.WebserviceDomain
			m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		m_eCallService = New SPeCallWebService.eCallSoapClient
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
		m_eCallService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_eCallWebServiceUri)

	End Sub

#End Region

	Public Function LoadPrivderData() As ProviderViewData
		Dim providerObj As New ProviderData(m_InitializationData)
		Dim result = providerObj.LoadProviderData(m_InitializationData.MDData.MDGuid, "eCall")

		m_AccountName = String.Empty
		m_AccountPassword = String.Empty

		If String.IsNullOrWhiteSpace(result.UserName) Then Return Nothing

		m_AccountName = result.UserName
		m_AccountPassword = result.UserData


		Return result

	End Function

	''' <summary>
	''' Send SMS to eCall Service
	''' </summary>
	Public Function SendSMS(ByVal message As ShortMessage, ByVal smsCallback As String, ByVal AnswerAddress As String) As ShortMessage

		Try
			' As SPeCallWebService.Response 
			Dim response = m_eCallService.SendSMSBasic(
								  m_AccountName, m_AccountPassword, message.Address,
								  message.Message, message.JobId, smsCallback,
								  Nothing, AnswerAddress, Nothing, Nothing, Nothing, Nothing)

			message.ResponseCode = response.ResponseCode
			message.ResponseText = response.ResponseText

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

			message.ResponseCode = 99999
			message.ResponseText = ex.ToString()
		End Try

		Return message

	End Function

	Public Function GetState(ByVal message As ShortMessage) As ShortMessage


		Try
			Dim response As SPeCallWebService.StatusResponse = m_eCallService.GetStateBasic(m_AccountName, m_AccountPassword, message.JobId, Nothing)
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
			m_Logger.LogError(ex.ToString())
			message.ResponseCode = 99999
			message.ResponseText = ex.ToString()
		End Try

		Return message

	End Function

	Public Sub LogPaymentService(ByVal jobId As String, ByVal pointsUsed As Double)

		Try

			Dim spCustomerServias As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
			spCustomerServias.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

			' Log over web servcice 
			Dim success As Boolean = spCustomerServias.LogeCallUsage(ModulConstants.UserData.UserMDGuid, ModulConstants.UserData.UserGuid, ModulConstants.UserData.UserFullName, SPUTNIK_ECALL_SMS, jobId, pointsUsed.ToString(), DateTime.Now)

			If Not success Then
				m_Logger.LogError(String.Format("Sorry, I could not create log into remote server {0}! I'll try to write into local database!", m_PaymentUtilWebServiceUri))
				' Log over local database.
				LogFailedeCallUsageLogWebserviceCall(jobId)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("LogPaymentService: JobID: {0} | UserGuid: {1} | UserFullName: {2} | Error: {3}", jobId, ModulConstants.UserData.UserGuid, ModulConstants.UserData.UserFullName, ex.ToString))
			' Log over local database.
			LogFailedeCallUsageLogWebserviceCall(jobId)
		End Try

	End Sub

	''' <summary>
	''' Logs a failed eCall Usage logging over webservice.
	''' </summary>
	Private Sub LogFailedeCallUsageLogWebserviceCall(ByVal jobId As String)

		Dim msg As String = String.Format("eCall SMS Usage could not be logged into database.<br><br><b>customerGuid: </b>{0}<br><b>userGuid: </b>{1}<br><b>userName: </b>{2}<br><b>CheckType: </b>{3}<br><b>serviceDate: </b>{4}<br><b>jobId: </b>{5}",
																		ModulConstants.UserData.UserMDGuid,
																		ModulConstants.UserData.UserGuid,
																		ModulConstants.UserData.UserFullName,
																		SPUTNIK_ECALL_SMS,
																		DateTime.Now,
																		jobId)
		Try

			Dim customerDBAccess As New CustomerDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim conStr As String = m_path.GetDbSelectData().MDDbConn

			Dim success As Boolean = customerDBAccess.LogSolvencyUsage(conStr, ModulConstants.UserData.UserMDGuid, ModulConstants.UserData.UserGuid, ModulConstants.UserData.UserFullName, SPUTNIK_ECALL_SMS, jobId, DateTime.Now)

			m_UtilityUI.SendMailNotification("LogFailedeCallUsageLogWebserviceCall", msg, String.Empty, Nothing)

			If Not success Then
				' Log in database did not work -> log solvency check usage in log file.
				m_Logger.LogError(msg)
			End If

		Catch ex As Exception
			m_Logger.LogError(msg)

		End Try


	End Sub

End Class

