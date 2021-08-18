
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Internal.Automations.SPNotificationWebService
Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService

Public Class ProviderData


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

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_NotificationUtilWebServiceUri As String
	Private m_PaymentUtilWebServiceUri As String

#End Region


#Region "private consts"

	Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"
	Private Const DEFAULT_SPUTNIK_PAYMENTSERVICE_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx"

#End Region


#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)
		m_InitializationData = _setting

		Dim domainName As String = m_InitializationData.MDData.WebserviceDomain

		m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
		m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PAYMENTSERVICE_UTIL_WEBSERVICE_URI)

	End Sub

#End Region


	Public Function LoadProviderData(ByVal customerID As String, ByVal providerName As String) As ProviderViewData

		Dim providerResult As ProviderViewData = Nothing

		Try

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			Dim searchResult As ProviderDataDTO = Nothing
			Try
				' Read data over webservice
				searchResult = webservice.GetProviderData(customerID, providerName)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			providerResult = New ProviderViewData With {
				.ID = searchResult.ID,
				.ProviderName = searchResult.ProviderName,
				.AccountName = searchResult.AccountName,
				.UserName = searchResult.UserName,
				.UserData = searchResult.UserData
			}


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return providerResult

	End Function

	Public Function IsCustomerAllowedToUseServiceData(ByVal customerID As String, ByVal serviceName As String) As Boolean

		Dim result As Boolean = True

		Try
			Dim searchResult = LoadCustomerDeniedServiceData(customerID)
			If searchResult Is Nothing Then
				m_Logger.LogError("could not list denied severices!")

				Return Nothing
			End If

			result = Not searchResult.Contains(serviceName.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return result

	End Function

	Function LoadCustomerDeniedServiceData(ByVal customerID As String) As List(Of String)

		Dim serviceResult As List(Of String) = Nothing

		Dim webservice As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

		Try
			' Read data over webservice
			Dim searchResult = webservice.GetCustomerDeniedListOfServices(customerID)
			If searchResult Is Nothing Then
				m_Logger.LogError("could not list denied severices!")

				Return Nothing
			End If

			serviceResult = New List(Of String)
			For Each itm In searchResult
				serviceResult.Add(itm)
			Next


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return serviceResult

	End Function


End Class
