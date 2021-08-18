Imports SPS.ExternalServices.DeltavistaWebService

''' <summary>
''' Abstract base class for solvency report request.
''' </summary>
Public MustInherit Class SolvencyReportRequestDataAbs

#Region "Private Fields"
    Private m_ReferenceNumber As String
    Private m_TargetReportFormat As TargetReportFormat
#End Region


#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <param name="targetReportFormat">The  target report format.</param>
    Public Sub New(ByVal referenceNumber As String, Optional ByVal targetReportFormat As TargetReportFormat = DeltavistaWebService.TargetReportFormat.PDF)

        If (String.IsNullOrEmpty(referenceNumber)) Then
            Throw New ArgumentException("Refrence number must be provided.")
        End If

        m_ReferenceNumber = referenceNumber
        m_TargetReportFormat = targetReportFormat

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the reference number.
    ''' </summary>
    ''' <returns>The reference number.</returns>
    Public ReadOnly Property ReferenceNumber As String
        Get
            Return m_ReferenceNumber
        End Get
    End Property

    ''' <summary>
    ''' Gets the report format.
    ''' </summary>
    ''' <returns>The report format.</returns>
    Public ReadOnly Property ReportFormat As TargetReportFormat
        Get
            Return m_TargetReportFormat
        End Get
    End Property

    ''' <summary>
    ''' Gets the string representation of the report type.
    ''' </summary>
    Public MustOverride ReadOnly Property ReportTypeStringRepresentation As String

    ''' <summary>
    ''' Gets the address.
    ''' </summary>
    Public MustOverride ReadOnly Property Address As AddressAbs

#End Region

End Class
