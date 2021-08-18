
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Internal.Automations.SPNotificationWebService
Imports SP.DatabaseAccess.TableSetting

Public Class SendSputnikLoginInfomations


#Region "private fields"

	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

	Private m_connectionString As String

	''' <summary>
	''' Service Uri of payment util webservice.
	''' </summary>
	Private m_PaymentUtilWebServiceUri As String
	Private m_NotificationUtilWebServiceUri As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant
	Private m_MailingSetting As String


#End Region


#Region "private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
	Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"
	Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx" ' "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
	Private Const MANDANT_XML_SETTING_MAILING As String = "MD_{0}/Mailing"

	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

#End Region


#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
		m_MailingSetting = String.Format(MANDANT_XML_SETTING_MAILING, m_InitializationData.MDData.MDNr)

		'm_NotificationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI
		'm_PaymentUtilWebServiceUri = DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI


		Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"
		'#If DEBUG Then
		'			domainName = "http://localhost"
		'#End If

		m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
		m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)

	End Sub

#End Region




#Region "private properties"

	''' <summary>
	''' Gets the email smtp server.
	''' </summary>
	Private ReadOnly Property EMailSMTPServer As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SMTP-Server", m_MailingSetting))

			Return settingValue
		End Get
	End Property

	Private ReadOnly Property EmployeeWOSIDSetting() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property

	Private ReadOnly Property CustomerWOSIDSetting() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property


#End Region


	Public Function SendSputnikUserDataWithWebservice(ByVal userNumber As Integer) As Boolean

		Dim success As Boolean = True

		Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
#If DEBUG Then
		'm_NotificationUtilWebServiceUri = "http://localhost:44721/SPNotification.asmx"
#End If
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

		Try
			Dim userData = m_TablesettingDatabaseAccess.LoadUserData(userNumber)

			If (userData Is Nothing OrElse userData.Count = 0) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzerdaten konnten nicht geladen werden."))
				Return False
			End If
			Dim assigneduserData = userData(0)

			Dim mandantUserdata = m_TablesettingDatabaseAccess.LoadAssignedUserAddressData(assigneduserData.USNr)
			If (mandantUserdata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandanten Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim mandantdata = m_TablesettingDatabaseAccess.LoadMandantData(assigneduserData.MDNr, Now.Year)
			If (mandantdata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandanten Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim setting = New SystemUserData With {.Customer_ID = mandantdata.Customer_ID, .UserGuid = assigneduserData.Transfered_Guid, .UserNr = userNumber}
			Dim employeeWOSID = EmployeeWOSIDSetting
			Dim customerWOSID = CustomerWOSIDSetting

			setting.EmployeeWOSID = employeeWOSID
			setting.CustomerWOSID = customerWOSID

			setting.UserMDName = mandantdata.MD_Name1.ToString.Trim
			setting.UserFName = assigneduserData.Vorname.ToString.Trim
			setting.UserLName = assigneduserData.Nachname.ToString.Trim
			setting.UserSalutation = assigneduserData.Anrede
			setting.UserLoginPassword = assigneduserData.PW
			setting.UserLoginname = assigneduserData.US_Name

			setting.UserFTitel = assigneduserData.USTitel_1
			setting.UserSTitel = assigneduserData.USTitel_2
			setting.UserMobile = assigneduserData.Natel
			setting.Birthdate = assigneduserData.GebDat
			setting.CreatedFrom = assigneduserData.CreatedFrom
			setting.ChangedFrom = assigneduserData.ChangedFrom
			setting.ChangedFrom = assigneduserData.ChangedFrom
			setting.ChangedOn = assigneduserData.ChangedOn
			setting.Deactivated = assigneduserData.Deaktiviert
			setting.EMail_UserName = assigneduserData.EMail_UserName
			setting.EMail_UserPW = assigneduserData.EMail_UserPW
			setting.EMail_SMTP = EMailSMTPServer

			setting.jch_layoutID = assigneduserData.jch_layoutID
			setting.jch_logoID = assigneduserData.jch_logoID
			setting.JCH_SubID = assigneduserData.JCH_SubID
			setting.OstJob_ID = assigneduserData.OstJob_ID
			setting.ostjob_Kontingent = assigneduserData.ostjob_Kontingent
			setting.UserBranchOffice = assigneduserData.USFiliale
			setting.UserKST = assigneduserData.KST
			setting.UserPicture = assigneduserData.USBild
			setting.UserSign = assigneduserData.USSign
			setting.AsCostCenter = assigneduserData.AsCostCenter
			setting.LogonMorePlaces = assigneduserData.LogonMorePlaces

			setting.UserMDPostfach = mandantUserdata.MD_Postfach
			setting.UserMDStrasse = mandantUserdata.MD_Strasse
			setting.UserMDLand = mandantUserdata.MD_Land
			setting.UserMDPLZ = mandantUserdata.MD_PLZ
			setting.UserMDOrt = mandantUserdata.MD_Ort
			setting.UserMDTelefon = mandantUserdata.MD_Telefon
			setting.UserMDTelefax = mandantUserdata.MD_Telefax
			setting.UserMDeMail = mandantUserdata.MD_eMail
			setting.UserMDHomepage = mandantUserdata.MD_Homepage


			success = success AndAlso webservice.AddUserData(mandantdata.Customer_ID, setting)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	Public Function UpdateActivationFlagForUserDataWithWebservice(ByVal userNumber As Integer) As Boolean

		Dim success As Boolean = True

		Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
#If DEBUG Then
		'm_NotificationUtilWebServiceUri = "http://localhost:44721/SPNotification.asmx"
#End If
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

		Try
			Dim userData = m_TablesettingDatabaseAccess.LoadUserData(userNumber)

			If (userData Is Nothing OrElse userData.Count = 0) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzerdaten konnten nicht geladen werden."))
				Return False
			End If
			Dim assigneduserData = userData(0)

			Dim mandantUserdata = m_TablesettingDatabaseAccess.LoadAssignedUserAddressData(assigneduserData.USNr)
			If (mandantUserdata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandanten Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim mandantdata = m_TablesettingDatabaseAccess.LoadMandantData(assigneduserData.MDNr, Now.Year)
			If (mandantdata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandanten Daten konnten nicht geladen werden."))
				Return False
			End If

			Dim setting = New SystemUserData With {.Customer_ID = mandantdata.Customer_ID, .UserGuid = assigneduserData.Transfered_Guid, .UserNr = userNumber}
			Dim employeeWOSID = EmployeeWOSIDSetting
			Dim customerWOSID = CustomerWOSIDSetting

			setting.EmployeeWOSID = employeeWOSID
			setting.CustomerWOSID = customerWOSID

			setting.UserMDName = mandantdata.MD_Name1.ToString.Trim
			setting.UserFName = assigneduserData.Vorname.ToString.Trim
			setting.UserLName = assigneduserData.Nachname.ToString.Trim
			setting.UserSalutation = assigneduserData.Anrede
			setting.UserLoginPassword = assigneduserData.PW
			setting.UserLoginname = assigneduserData.US_Name

			setting.UserFTitel = assigneduserData.USTitel_1
			setting.UserSTitel = assigneduserData.USTitel_2
			setting.UserMobile = assigneduserData.Natel
			setting.Birthdate = assigneduserData.GebDat
			setting.CreatedFrom = assigneduserData.CreatedFrom
			setting.ChangedFrom = assigneduserData.ChangedFrom
			setting.ChangedFrom = m_InitializationData.UserData.UserFullName.ToString.Trim
			setting.ChangedOn = Now
			setting.Deactivated = True
			setting.EMail_UserName = assigneduserData.EMail_UserName
			setting.EMail_UserPW = assigneduserData.EMail_UserPW
			setting.EMail_SMTP = EMailSMTPServer

			setting.jch_layoutID = assigneduserData.jch_layoutID
			setting.jch_logoID = assigneduserData.jch_logoID
			setting.JCH_SubID = assigneduserData.JCH_SubID
			setting.OstJob_ID = assigneduserData.OstJob_ID
			setting.ostjob_Kontingent = assigneduserData.ostjob_Kontingent
			setting.UserBranchOffice = assigneduserData.USFiliale
			setting.UserKST = assigneduserData.KST
			setting.UserPicture = assigneduserData.USBild
			setting.UserSign = assigneduserData.USSign
			setting.AsCostCenter = assigneduserData.AsCostCenter
			setting.LogonMorePlaces = assigneduserData.LogonMorePlaces

			setting.UserMDPostfach = mandantUserdata.MD_Postfach
			setting.UserMDStrasse = mandantUserdata.MD_Strasse
			setting.UserMDLand = mandantUserdata.MD_Land
			setting.UserMDPLZ = mandantUserdata.MD_PLZ
			setting.UserMDOrt = mandantUserdata.MD_Ort
			setting.UserMDTelefon = mandantUserdata.MD_Telefon
			setting.UserMDTelefax = mandantUserdata.MD_Telefax
			setting.UserMDeMail = mandantUserdata.MD_eMail
			setting.UserMDHomepage = mandantUserdata.MD_Homepage


			success = success AndAlso webservice.AddUserData(mandantdata.Customer_ID, setting)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	Public Function AddSputnikLoginDataWithWebservice() As Boolean

		Dim success As Boolean = True

		Try

			Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
			'm_PaymentUtilWebServiceUri = "http://localhost:44721/SPCustomerPaymentServices.asmx"
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

			Try
				' Read data over webservice
				Dim strStationID As String = String.Format("Machinename: {0}|Domainname: {1}|UserName: {2}|MDName: {3}|MDGuid: {4}|{5}|Windowskey: {6}",
																						 Environment.MachineName,
																						 Environment.UserDomainName,
																						 Environment.UserName,
																						 m_InitializationData.MDData.MDName,
																						 m_InitializationData.MDData.MDGuid,
																						 String.Empty,
																						 String.Empty)

				success = success AndAlso webservice.LogSputnikLoginUsage(m_InitializationData.MDData.MDGuid, m_InitializationData.MDData.MDName,
																		  m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName.ToString.Trim,
																		  Environment.UserName, Environment.MachineName, Environment.UserDomainName)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False
			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function


End Class
