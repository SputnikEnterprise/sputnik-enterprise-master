
Imports SPS.ExternalServices
Imports System.Threading.Tasks
Imports SPS.ExternalServices.DeltavistaWebService
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.Infrastructure.Logging
Imports System.Threading

Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten


''' <summary>
''' Performs a solvency check
''' </summary>
Public Class frmPerformSolvencyCheck


#Region "Private Consts"

  Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
  Public Const DEFAULT_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wssps_services/spcustomerpaymentservices.asmx"

#End Region

#Region "Private Fields"

  ''' <summary>
  ''' The Initialization data.
  ''' </summary>
  Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

  ''' <summary>
  ''' The customer number.
  ''' </summary>
  Private m_CustomerNumber As Integer

  ''' <summary>
  ''' The user name for the web service.
  ''' </summary>
  Private m_UserNameForService As String

  ''' <summary>
  ''' The password for service.
  ''' </summary>
  Private m_PasswordForService As String

  ''' <summary>
  ''' The service url.
  ''' </summary>
  Private m_ServiceUrl As String

  ''' <summary>
  ''' Service Uri of Sputnik payment util webservice.
  ''' </summary>
  Private m_PaymentUtilWebServiceUri As String

  ''' <summary>
  ''' The request data.
  ''' </summary>
  Private m_RequestData As SolvencyReportRequestDataAbs

  ''' <summary>
  ''' The response data.
  ''' </summary>
  Private m_ResponseData As TypeGetReportResponse

  ''' <summary>
  ''' The SPProgUtility object.
  ''' </summary>
  Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger

  ''' <summary>
  ''' The ui task scheduler.
  ''' </summary>
  Private m_UITaskScheduler As TaskScheduler

  Private m_MandantSettingsXml As SettingsXml

  ''' <summary>
  ''' The mandant.
  ''' </summary>
  Private m_MandantData As Mandant

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <param name="userNameForService">The user name for the web service.</param>
  ''' <param name="passwordForService">The password for the web service.</param>
  ''' <param name="serviceUrl">The service url for the web service.</param>
  ''' <param name="requestData">The request data.</param>
  ''' <param name="_setting">The initialization data.</param>
  Sub New(ByVal customerNumber As Integer,
          ByVal userNameForService As String,
          ByVal passwordForService As String,
          ByVal serviceUrl As String,
          ByVal requestData As SolvencyReportRequestDataAbs,
          ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    InitializeComponent()

		m_UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()

		m_RequestData = requestData
    m_UserNameForService = userNameForService
    m_PasswordForService = passwordForService
    m_ServiceUrl = serviceUrl
    m_Logger = New Logger()

    m_MandantData = New Mandant
    m_InitializationData = _setting
    m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

    Me.progressPanel1.AutoHeight = True

    SetCaption(m_translate.GetSafeTranslationValue("Bonitätsüberprüfung wird durchgeführt."))
    SetDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

    Try
      m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
      m_PaymentUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

      If String.IsNullOrWhiteSpace(m_PaymentUtilWebServiceUri) Then
        m_PaymentUtilWebServiceUri = DEFAULT_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_PaymentUtilWebServiceUri = DEFAULT_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI
    End Try

  End Sub

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' Gets the response data.
  ''' </summary>
  ''' <returns>The response data or nothing</returns>
  Public ReadOnly Property ResponseData As TypeGetReportResponse
    Get
      Return m_ResponseData
    End Get
  End Property

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Sets the caption.
  ''' </summary>
  ''' <param name="caption">The caption.</param>
  Public Overrides Sub SetCaption(ByVal caption As String)
    MyBase.SetCaption(caption)
    Me.progressPanel1.Caption = caption
  End Sub

  ''' <summary>
  ''' Sets the description.
  ''' </summary>
  ''' <param name="description">The description.</param>
  Public Overrides Sub SetDescription(ByVal description As String)
    MyBase.SetDescription(description)
    Me.progressPanel1.Description = description
  End Sub

#End Region

#Region "Private Methods"

  ''' <summary>
  ''' Handles form load event.
  ''' </summary>
  Private Sub OnFrmPerformSolvencyCheck_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    PerformCheck()
  End Sub

  ''' <summary>
  ''' Perform the check.
  ''' </summary>
  Private Sub PerformCheck()
    Task.Factory.StartNew(Sub() PerformCheckAsync(),
                                            CancellationToken.None,
                                            TaskCreationOptions.None,
                                            TaskScheduler.Default)

  End Sub

  ''' <summary>
  '''  Performs the check asynchronous.
  ''' </summary>
  Private Sub PerformCheckAsync()

    ' 1. Perform solvency check.
    Try
      Dim solvencyChecker = New DeltavistaServices(m_UserNameForService, m_PasswordForService, m_ServiceUrl)
      m_ResponseData = solvencyChecker.RequestSolvencyReportInformation(m_RequestData)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())
      m_ResponseData = Nothing
    End Try

    ' 2. Log solvency check usage (only in case of exact match)
    If Not m_ResponseData Is Nothing AndAlso m_ResponseData.addressMatchResult.addressMatchResultType = AddressMatchResultType.MATCH Then

      Try

        Dim spCustomerService As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient

				spCustomerService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)
				' Log over web servcice
				Dim success = spCustomerService.LogSolvencyCheckUsage(m_InitializationData.UserData.UserMDGuid, m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName,
																															m_RequestData.ReportTypeStringRepresentation, DateTime.Now)

        If Not success Then
          ' Log over local database.
          LogFailedSolvencyCheckUsageLogWebserviceCall()
        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        ' Log over local database.
        LogFailedSolvencyCheckUsageLogWebserviceCall()
      End Try

    End If

    ' Close ui
    Task.Factory.StartNew(Sub() Close(), CancellationToken.None, TaskCreationOptions.None, m_UITaskScheduler)

  End Sub

  ''' <summary>
  ''' Logs a failed solvency check logging over webservice.
  ''' </summary>
  Private Sub LogFailedSolvencyCheckUsageLogWebserviceCall()

    Dim customerDBAccess As New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

    Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
    Dim conStr = m_path.GetDbSelectData().MDDbConn

		Dim success As Boolean = customerDBAccess.LogSolvencyUsage(conStr,
																															 m_InitializationData.UserData.UserMDGuid,
																															 m_InitializationData.UserData.UserGuid,
																															 m_InitializationData.UserData.UserFullName,
																															 m_RequestData.ReportTypeStringRepresentation,
																															 String.Empty,
																															 DateTime.Now)
    If Not success Then

      ' Log in database did not work -> log solvency check usage in log file.
			m_Logger.LogError(String.Format("Solvency Check Usage could not be logged into database. customerGuid={0}, userGuid={1}, userName={2}, solvencyCheckType={3}, serviceDate{4}",
																			m_InitializationData.UserData.UserMDGuid,
																			m_InitializationData.UserData.UserGuid,
																			m_InitializationData.UserData.UserFullName,
																			m_RequestData.ReportTypeStringRepresentation,
																			DateTime.Now))
    End If


  End Sub

#End Region

End Class
