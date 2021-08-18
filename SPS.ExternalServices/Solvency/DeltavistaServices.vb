
Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Deltavista services.
''' </summary>
Public Class DeltavistaServices
    Implements IDeltavistaServices

#Region "Private Constants"

    Private Const MAJOR_VERSION As Integer = 1
    Private Const MINOR_VERSION As Integer = 0

#End Region

#Region "Private Fields"

    Private m_UserName As String
    Private m_Password As String
    Private m_ServiceUrl As String

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="userName">The user name for the external service.</param>
    ''' <param name="password">The password for the external service.</param>
    ''' <param name="serviceUrl">The service url of the external serfice</param>
    Public Sub New(ByVal userName As String, ByVal password As String, ByVal serviceUrl As String)
        m_UserName = userName
        m_Password = password
        m_ServiceUrl = serviceUrl
    End Sub

#End Region

#Region "Public Methods"

    ''' <see cref="SPS.ExternalServices.IDeltavistaServices.RequestSolvencyReportInformation" />
    Public Function RequestSolvencyReportInformation(ByVal requestData As SolvencyReportRequestDataAbs) As TypeGetReportResponse Implements IDeltavistaServices.RequestSolvencyReportInformation

        If requestData Is Nothing Then
            Throw New ArgumentException("Request data must be provided.")
        End If

        Dim request = PrepareGetReportRequest(requestData.ReferenceNumber, requestData.ReportFormat, requestData.ReportTypeStringRepresentation)

        If Not requestData.Address.AddressIdentifier Is Nothing Then
            request.identifier = New Identifier()
            request.identifier.identifierText = requestData.Address.AddressIdentifier.identifierText
            request.identifier.identifierType = requestData.Address.AddressIdentifier.identifierType
        Else
            request.searchedAddress = requestData.Address.AddressDescription
        End If

        Return RequestReportDataOverWebService(request)

    End Function


    ''' <see cref="SPS.ExternalServices.IDeltavistaServices.RequestArchivedSolvencyReportInformation" />
    Public Function RequestArchivedSolvencyReportInformation(ByVal requestData As SolvencyReportArchivedRequestData) As TypeGetArchivedReportResponse Implements IDeltavistaServices.RequestArchivedSolvencyReportInformation

        If requestData Is Nothing Then
            Throw New ArgumentException("Request data must be provided.")
        End If

        Dim request = PrepareGetArchivedReportRequest(requestData.ReferenceNumber, requestData.ArchivingId, requestData.ReportFormat)
        Return RequestArchivedReportDataOverWebService(request)

    End Function

    ''' <see cref="SPS.ExternalServices.IDeltavistaServices.RequestDebtDetails" />
    Public Function RequestDebtDetails(ByVal requestData As DeptDetailsRequestData) As TypeGetDebtDetailsResponse Implements IDeltavistaServices.RequestDebtDetails

        If requestData Is Nothing Then
            Throw New ArgumentException("Request data must be provided.")
        End If

        Dim request = PrepareGetDebtDetailsRequest(requestData.ReferenceNumber, requestData.Identifer)
        Return RequestDebtDetailsOverWebService(request)

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Prepares a get report request.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <returns>The request object.</returns>
    Private Function PrepareGetReportRequest(ByVal referenceNumber As String, ByVal targetReportFormat As TargetReportFormat, ByVal reportType As String) As TypeGetReportRequest
        Dim request As TypeGetReportRequest = New TypeGetReportRequest()
        request.referenceNumber = referenceNumber
        request.targetReportFormat = targetReportFormat
        request.targetReportFormatSpecified = True
        request.reportType = reportType
        request.control = CreateControlData()
        request.identityDescriptor = CreateCredentials()

        Return request
    End Function

    ''' <summary>
    ''' Prepares a get archived report request.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <param name="archivingId">The archivingid.</param>
    ''' <returns>The request object.</returns>
    Private Function PrepareGetArchivedReportRequest(ByVal referenceNumber As String, ByVal archivingId As Long, ByVal targetReportFormat As TargetReportFormat) As TypeGetArchivedReportRequest
        Dim request As TypeGetArchivedReportRequest = New TypeGetArchivedReportRequest()
        request.referenceNumber = referenceNumber
        request.archivingId = archivingId
        request.targetFormat = targetReportFormat
        request.control = CreateControlData()
        request.identityDescriptor = CreateCredentials()

        Return request
    End Function


    ''' <summary>
    ''' Prepares a get debt details request.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <param name="identifier">The identifier.</param>
    Private Function PrepareGetDebtDetailsRequest(ByVal referenceNumber As String, ByVal identifier As Identifier) As TypeGetDebtDetailsRequest
        Dim request As TypeGetDebtDetailsRequest = New TypeGetDebtDetailsRequest()
        request.referenceNumber = referenceNumber
        request.identifier = identifier
        request.control = CreateControlData()
        request.identityDescriptor = CreateCredentials()

        Return request
    End Function

    ''' <summary>
    ''' Creates control data.
    ''' </summary>
    ''' <returns>The control data.</returns>
    Private Function CreateControlData() As Control
        Dim control = New Control()
        control.majorVersion = MAJOR_VERSION
        control.minorVersion = MINOR_VERSION

        Return control
    End Function

    ''' <summary>
    ''' Create credential data.
    ''' </summary>
    ''' <returns>The credential data.</returns>
    Private Function CreateCredentials() As IdentityDescriptor
        Dim identity As IdentityDescriptor = New IdentityDescriptor()
        identity.userName = m_UserName
        identity.password = m_Password

        Return identity

    End Function

    ''' <summary>
    ''' Request reqport data over web service.
    ''' </summary>
    ''' <param name="request">The request.</param>
    ''' <returns>The response.</returns>
    Private Function RequestReportDataOverWebService(ByRef request As TypeGetReportRequest) As TypeGetReportResponse

        Dim response As TypeGetReportResponse = Nothing

        Try

            Dim service As New DeltavistaWebService.CrifSoapServicePortTypeV1_0Client()
            service.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ServiceUrl)

            response = service.getReport(request)

        Catch ex As Exception
            Throw New Exception("Error while requesting report data over Deltavista Webservice. See inner exception for more details.", ex)
        End Try

        Return response

    End Function

    ''' <summary>
    ''' Request archived report data over web service.
    ''' </summary>
    ''' <param name="request">The request.</param>
    ''' <returns>The response.</returns>
    Private Function RequestArchivedReportDataOverWebService(ByRef request As TypeGetArchivedReportRequest) As TypeGetArchivedReportResponse

        Dim response As TypeGetArchivedReportResponse = Nothing

        Try
            Dim service As New DeltavistaWebService.CrifSoapServicePortTypeV1_0Client()
            service.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ServiceUrl)

            response = service.getArchivedReport(request)

            If (response.report Is Nothing) Then
                Throw New Exception("Report must not be null!")
            End If

        Catch ex As Exception
            Throw New Exception("Error while requesting archived report data over Deltavista Webservice. See inner exception for more details.", ex)
        End Try

        Return response

    End Function

    ''' <summary>
    ''' Request get debt details report data over web service.
    ''' </summary>
    ''' <param name="request">The request.</param>
    ''' <returns>The response.</returns>
    Private Function RequestDebtDetailsOverWebService(ByRef request As TypeGetDebtDetailsRequest) As TypeGetDebtDetailsResponse

        Dim response As TypeGetDebtDetailsResponse = Nothing

        Try
            Dim service As New DeltavistaWebService.CrifSoapServicePortTypeV1_0Client()
            service.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ServiceUrl)

            response = service.getDebtDetails(request)

        Catch ex As Exception
            Throw New Exception("Error while requesting debt details data over Deltavista Webservice. See inner exception for more details.", ex)
        End Try

        Return response

    End Function

#End Region

End Class
