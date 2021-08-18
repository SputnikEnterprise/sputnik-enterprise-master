Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Solvency report consumer request data.
''' </summary>
Public Class SolvencyReportCompanyRequestData
    Inherits SolvencyReportRequestDataAbs

#Region "Private Fields"

    Private m_ReportType As CompanyReportType
    Private m_CompanyAddressData As CompanyAddress

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <param name="companyAddress">The company address.</param>
    ''' <param name="reportType">The report type.</param>
    ''' <param name="targetReportFormat">The  target report format.</param>
    Public Sub New(ByVal referenceNumber As String, ByVal companyAddress As CompanyAddress, ByVal reportType As CompanyReportType, Optional ByVal targetReportFormat As TargetReportFormat = DeltavistaWebService.TargetReportFormat.PDF)
        MyBase.New(referenceNumber, targetReportFormat)

        If (companyAddress Is Nothing) Then
            Throw New ArgumentException("Company address must be provided.")
        End If

        m_ReportType = reportType
        m_CompanyAddressData = companyAddress

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the report type.
    ''' </summary>
    ''' <returns>The report type.</returns>
    Public ReadOnly Property ReportType As CompanyReportType
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
            Return m_CompanyAddressData
        End Get
    End Property

#End Region

End Class
