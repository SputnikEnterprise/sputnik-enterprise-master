

Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.Internal.Automations.SPWOSCustomerWebService
Imports SP.Internal.Automations.SPWOSEmployeeWebService


Public Class WOSDocumentData


#Region "Private Consts"

	Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx" ' "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
	Private Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPIBANUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"
	Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"
	Public Const DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSCustomerUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPWOSCustomerUtilities.asmx"
	Private Const DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSEmployeeUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPWOSEmployeeUtilities.asmx"

	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
	Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "MD_{0}/Interfaces/webservices/webserviceecall"

	Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
	Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"
	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

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
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_PaymentUtilWebServiceUri As String

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_CustomerWosUtilWebServiceUri As String

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_EmployeeWosUtilWebServiceUri As String
	Private m_NotificationUtilWebServiceUri As String

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_eCallWebServiceUri As String

	Private m_AccountName As String
	Private m_AccountPassword As String

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
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	Private m_WOSDocumentData As WOSViewData

	''' <summary>
	''' is wos allowed
	''' </summary>
	Private m_WOSFunctionalityAllowed As Boolean
	Private m_EmployeeWOSID As String
	Private m_CustomerWOSID As String

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_InitializationData = _setting
		m_SettingsManager = New SettingsManager
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		m_SuppressUIEvents = True

		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

			m_AccountName = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, m_InitializationData.MDData.MDNr)))
			m_AccountPassword = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, m_InitializationData.MDData.MDNr)))
			m_eCallWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_URI, m_InitializationData.MDData.MDNr))


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"
		'#If DEBUG Then
		'			domainName = "http://localhost"
		'#End If

		m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
		m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)
		m_CustomerWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI)
		m_EmployeeWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI)


		m_EmployeeWOSID = EmployeeWOSID
		m_CustomerWOSID = CustomerWOSID

		m_WOSFunctionalityAllowed = (m_EmployeeWOSID & m_CustomerWOSID).Length > 50


	End Sub

#End Region


#Region "public property"

	Public Property EmployeeWOSID
	Public Property CustomerWOSID

	Public Property WOSModul As WOSEnum

	Public Enum WOSEnum
		EMPLOYEE
		CUSTOMER
	End Enum

#End Region


#Region "public methods"

	Public Sub LoadDocument(ByVal wosmodul As WOSEnum, ByVal recID As Integer)

		If wosmodul = WOSEnum.EMPLOYEE Then
			m_WOSDocumentData = PerformWOSEmployeeDocWebserviceCall(recID)

		ElseIf wosmodul = WOSEnum.CUSTOMER Then
			m_WOSDocumentData = PerformWOSCustomerDocWebserviceCall(recID)

		Else
			Return
		End If

		If Not m_WOSDocumentData Is Nothing Then
			OpenDocument()
		End If

	End Sub


#End Region


#Region "private methods"

	''' <summary>
	'''  Performs employee wos doc check.
	''' </summary>
	Private Function PerformWOSEmployeeDocWebserviceCall(ByVal recID As Integer) As WOSViewData

		Dim result As WOSViewData = Nothing

		Dim webservice As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)

		Dim searchResult As EmployeeWOSDataDTO = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetAssignedEmployeeDocumentsData(m_EmployeeWOSID, recID)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		result = New WOSViewData With {
			.ID = searchResult.ID,
			.Customer_ID = searchResult.Customer_ID,
			.DocGuid = searchResult.DocGuid,
			.DocumentArt = searchResult.DocumentArt,
			.DocumentInfo = searchResult.DocumentInfo,
			.LastNotification = searchResult.LastNotification,
			.TransferedOn = searchResult.TransferedOn,
			.TransferedUser = searchResult.TransferedUser,
			.ScanContent = searchResult.ScanContent
		}


		Return result

	End Function

	''' <summary>
	'''  Performs customer wos doc check.
	''' </summary>
	Private Function PerformWOSCustomerDocWebserviceCall(ByVal recID As Integer) As WOSViewData

		Dim result As WOSViewData = Nothing
		If String.IsNullOrWhiteSpace(m_CustomerWOSID) Then
			m_Logger.LogWarning(String.Format("customer_ID was not licenced for WOS: {0}", m_InitializationData.MDData.MDGuid))

			Return Nothing
		End If

		Dim webservice As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

		Dim searchResult As CustomerWOSDataDTO = Nothing
		Try
			' Read data over webservice
			searchResult = webservice.GetAssignedCustomerDocumentsData(m_CustomerWOSID, recID)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		result = New WOSViewData With {
			.ID = searchResult.ID,
			.Customer_ID = searchResult.Customer_ID,
			.DocGuid = searchResult.DocGuid,
			.DocumentArt = searchResult.DocumentArt,
			.DocumentInfo = searchResult.DocumentInfo,
			.LastNotification = searchResult.LastNotification,
			.TransferedOn = searchResult.TransferedOn,
			.TransferedUser = searchResult.TransferedUser,
			.ScanContent = searchResult.ScanContent
		}


		Return result

	End Function


	Private Sub OpenDocument()
		Dim fileExtension As String = "PDF"

		If Not m_WOSDocumentData Is Nothing Then
			Dim bytes() = m_WOSDocumentData.ScanContent
			Dim tempFileName = System.IO.Path.GetTempFileName()
			Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, fileExtension)

			If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

				m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

			End If

		End If

	End Sub

#End Region



#Region "Helpers"


	Class WOSViewData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property DocumentArt As String
		Public Property DocumentInfo As String

		Public Property TransferedOn As DateTime?
		Public Property TransferedUser As String
		Public Property LastNotification As DateTime?

		Public Property DocGuid As String
		Public Property ScanContent As Byte()

	End Class


#End Region


End Class
