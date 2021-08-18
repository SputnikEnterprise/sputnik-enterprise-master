Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Solvency report consumer request data.
''' </summary>
Public Class SolvencyReportConsumerRequestData
    Inherits SolvencyReportRequestDataAbs

#Region "Private Fields"

    Private m_ReportType As ConsumerReportType
    Private m_ConsumerAddressData As ConsumerAddress

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <param name="consumerAddress">The consumer address.</param>
    ''' <param name="reportType">The report type.</param>
    ''' <param name="targetReportFormat">The  target report format.</param>
    Public Sub New(ByVal referenceNumber As String, ByVal consumerAddress As ConsumerAddress, ByVal reportType As ConsumerReportType, Optional ByVal targetReportFormat As TargetReportFormat = DeltavistaWebService.TargetReportFormat.PDF)
        MyBase.New(referenceNumber, targetReportFormat)

        If (consumerAddress Is Nothing) Then
            Throw New ArgumentException("Consumer address must be provided.")
        End If

        m_ReportType = reportType
        m_ConsumerAddressData = consumerAddress

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the report type.
    ''' </summary>
    ''' <returns>The report type.</returns>
    Public ReadOnly Property ReportType As ConsumerReportType
        Get
            Return m_ReportType
        End Get
    End Property

    ''' <summary>
    ''' Gets the string representation of the report type.
    ''' </summary>
    Public Overrides ReadOnly Property ReportTypeStringRepresentation As String
        Get
            Return m_ReportType.ToString()
        End Get
    End Property

    ''' <summary>
    ''' Gets the address.
    ''' </summary>
    Public Overrides ReadOnly Property Address As AddressAbs
        Get
            Return m_ConsumerAddressData
        End Get
    End Property

#End Region

End Class
