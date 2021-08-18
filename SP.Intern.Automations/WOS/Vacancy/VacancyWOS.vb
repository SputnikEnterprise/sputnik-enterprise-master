
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.Internal.Automations.SPNotificationWebService


Namespace WOSUtility

	Public Class VacancyData


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

#End Region


#Region "private consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"

#End Region


#Region "Constructor"


		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)
			m_InitializationData = _setting

			'm_NotificationUtilWebServiceUri = DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI
			Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"
			'#If DEBUG Then
			'			domainName = "http://localhost"
			'#End If

			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

		End Sub

#End Region


		Public Function LoadNumberOfVacancyJobPlattformData(ByVal customerID As String, ByVal wos_id As String, ByVal jobsCHAccountNumber As Integer, ByVal ostjobAccountNumber As String) As JobplattformCounterViewData

			Dim providerResult As JobplattformCounterViewData = Nothing
			' 	Function GetJobplattformCounterData(ByVal customerID As String, ByVal wos_id As String, ByVal jobsCHAccountNumber As Integer, ByVal ostjobAccountNumber As String) As JobplattformCounterDataDTO

			Try

				Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

				Dim searchResult As JobplattformCounterDataDTO = Nothing
				Try
					' Read data over webservice
					searchResult = webservice.getjobplattformCounterData(customerID, wos_id, jobsCHAccountNumber, ostjobAccountNumber)

				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
				End Try

				providerResult = New JobplattformCounterViewData With {
				.ID = searchResult.ID,
				.Customer_ID = searchResult.Customer_ID,
				.WOS_ID = searchResult.WOS_ID,
				.JobChannelPriorityCounter = searchResult.JobChannelPriorityCounter,
				.JobsCHCounter = searchResult.JobsCHCounter,
				.OstJobCounter = searchResult.OstJobCounter,
				.OwnCounter = searchResult.OwnCounter
			}


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Return providerResult

		End Function

		Public Function IsAssignedVacancyOnline(ByVal customerID As String, ByVal wos_id As String, ByVal vacancyNumber As Integer, ByVal jobsCHAccountNumber As Integer, ByVal ostjobAccountNumber As String) As VacancyStateViewData

			Dim providerResult As VacancyStateViewData = Nothing

			Try


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Return providerResult

		End Function


	End Class

End Namespace