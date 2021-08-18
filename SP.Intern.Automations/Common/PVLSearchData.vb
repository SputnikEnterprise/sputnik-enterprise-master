
Imports SP.Infrastructure.Logging
Imports SP.Internal.Automations.SPUpdateUtilitiesService


Public Class PVLSearchData


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
	Private m_UpdateNotificationWebServiceUri As String

#End Region


#Region "private consts"

	Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPUpdateUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"

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

		m_UpdateNotificationWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

	End Sub

#End Region


	Public Function LoadGAVUpdateNotificationData(ByVal gavNumber As Integer) As GAVVersionData

		Dim result As GAVVersionData = Nothing

		Try

			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateNotificationWebServiceUri)

			Dim searchResult As GAVVersionDataDTO = Nothing
			Try
				' Read data over webservice
				searchResult = webservice.GetGAVVersionNotificationData(m_InitializationData.MDData.MDGuid, gavNumber)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			result = New GAVVersionData With {
				.ID = searchResult.ID,
				.GAVNumber = searchResult.GAVNumber,
				.GAVDate = searchResult.GAVDate,
				.GAVInfo = searchResult.GAVInfo,
				.schema_version = searchResult.schema_version
			}


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return result

	End Function


End Class

Public Class GAVVersionData

	Public Property ID As Integer
	Public Property GAVNumber As Integer?
	Public Property GAVDate As DateTime?
	Public Property GAVInfo As String
	Public Property schema_version As String

End Class