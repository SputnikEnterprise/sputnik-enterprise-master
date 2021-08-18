
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Vacancy
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten

Namespace AVAMWebServiceProcess


	Public Class WebServiceProcess

#Region "private consts"

		Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_INTERN_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicewosvacancies"
		Private Const DEFAULT_SPUTNIK_VACANCY_UTIL_WEBSERVICE_URI As String = "wssps_services/SPInternVacancies.asmx"
		Private Const DEFAULT_SPUTNIK_AVAM_UTIL_WEBSERVICE_URI As String = "wssps_AvamServices/SPInternAvam.asmx"

		Private Const JOBROOM_USER As String = "username"
		Private Const JOBROOM_PASSWORD As String = "password"
		Private Const JOBROOM_URI As String = "https://api.job-room.ch/jobAdvertisements/v1"
		Private Const JOBROOM_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
		Private Const JOBROOM_SINGLE_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1/{0}"
		Private Const JOBROOM_CANCEL_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1/{0}/cancel"

		Private Const STAGING_JOBROOM_USER As String = "username"
		Private Const STAGING_JOBROOM_PASSWORD As String = "password"
		Private Const STAGING_JOBROOM_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1"
		Private Const STAGING_JOBROOM_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
		Private Const STAGING_JOBROOM_SINGLE_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1/{0}"
		Private Const STAGING_JOBROOM_CANCEL_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1/{0}/cancel"

#End Region

#Region "private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Protected m_Logger As ILogger = New Logger()

		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' Vacancy datbase access.
		''' </summary>
		Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Protected m_Utility As Infrastructure.Utility

		''' <summary>
		''' Service Uri of Sputnik vacancies util webservice.
		''' </summary>
		Private m_VacancyUtilWebServiceUri As String
		'Private m_AvamUtilWebServiceUri As String
		Private m_connectionString As String

		Private m_SearchResultData As JobroomSearchResultData

		Private m_TransmittedSTMPid As String


		Private m_APIResponse As String
		Private m_ReportingObligation As Boolean?
		Private m_QueryResultData As SPAVAMQueryResultData

		Private m_ResultContent As String

		Private m_UserName As String
		Private m_Password As String
		Private m_JobroomURI As String
		Private m_JobroomAllRecordURI As String
		Private m_JobroomSingleRecordURI As String

#End Region

#Region "public properties"

		Public Property AsStaging As Boolean

#End Region

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New Infrastructure.Utility
			m_InitializationData = _setting

			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_connectionString = m_InitializationData.MDData.MDDbConn

			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			Dim domainName = m_InitializationData.MDData.WebserviceDomain

			m_VacancyUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_UTIL_WEBSERVICE_URI)


		End Sub

		Public Async Function webserviceResponse(ByVal sb As StringBuilder, ByVal baseUri As Uri, ByVal Method As String, ByVal User As String, ByVal Password As String) As Task(Of HttpResponseMessage)

			Dim client As HttpClient = New HttpClient()
			Dim resp As HttpResponseMessage = Nothing

			Try
				client.BaseAddress = baseUri
				m_Logger.LogInfo(String.Format("Loging as staging: {0} >>> baseUri: {1} >>> TLS: {2}", AsStaging, baseUri, ServicePointManager.SecurityProtocol))

				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

				If String.IsNullOrEmpty(User) Then
					Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("None")
					client.DefaultRequestHeaders.Authorization = authHeader

				Else
					Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", User, Password))))
					client.DefaultRequestHeaders.Authorization = authHeader

				End If

				Dim timeout As TimeSpan = TimeSpan.FromMinutes(5)
				client.Timeout = timeout

				Dim content As New StringContent(String.Empty)
				If Not sb Is Nothing Then content = New StringContent(sb.ToString, System.Text.Encoding.UTF8, "application/json")

				Dim cancellationToken As CancellationToken

				If Method = "Post" Then
					resp = Await client.PostAsync(baseUri, content, cancellationToken)

				ElseIf Method = "Put" Then
					resp = Await client.PutAsync(baseUri, content, cancellationToken)

				ElseIf Method = "Delete" Then
					resp = Await client.DeleteAsync(baseUri, cancellationToken)

				ElseIf Method = "Patch" Then
					resp = Await HttpClientExtensions.PatchAsync(client, baseUri, content, cancellationToken)

				ElseIf Method = "Get" Then
					resp = Await client.GetAsync(baseUri, cancellationToken)

				End If
				If resp.StatusCode = HttpStatusCode.BadRequest Then Return resp


			Catch ex As Exception
				m_Logger.LogError(String.Format("Error while webserviceResponse:{0}{1}", vbNewLine, ex.ToString))

			End Try

			Return resp
		End Function

	End Class


	Module HttpClientExtensions
		<Extension()>
		Function PatchAsync(ByVal client As HttpClient, ByVal requestUri As String, ByVal content As HttpContent) As Task(Of HttpResponseMessage)
			Return client.PatchAsync(CreateUri(requestUri), content)
		End Function

		<Extension()>
		Function PatchAsync(ByVal client As HttpClient, ByVal requestUri As Uri, ByVal content As HttpContent) As Task(Of HttpResponseMessage)
			Return client.PatchAsync(requestUri, content, CancellationToken.None)
		End Function

		<Extension()>
		Function PatchAsync(ByVal client As HttpClient, ByVal requestUri As String, ByVal content As HttpContent, ByVal cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
			Return client.PatchAsync(CreateUri(requestUri), content, cancellationToken)
		End Function

		<Extension()>
		Function PatchAsync(ByVal client As HttpClient, ByVal requestUri As Uri, ByVal content As HttpContent, ByVal cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
			Return client.SendAsync(New HttpRequestMessage(New HttpMethod("PATCH"), requestUri) With {
			.Content = content
		}, cancellationToken)
		End Function

		Private Function CreateUri(ByVal uri As String) As Uri
			Return If(String.IsNullOrEmpty(uri), Nothing, New Uri(uri, UriKind.RelativeOrAbsolute))
		End Function
	End Module


End Namespace
