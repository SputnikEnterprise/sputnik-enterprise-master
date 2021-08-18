
''' <summary>
''' Solvency report archived request data.
''' </summary>
Public Class SolvencyReportArchivedRequestData
    Inherits SolvencyReportRequestDataAbs

#Region "Private Fields"

    Private m_ArchivingId As Long

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="referenceNumber">The reference number.</param>
    ''' <param name="archivingId">The archivingid.</param>
    ''' <param name="targetReportFormat">The  target report format.</param>
    Public Sub New(ByVal referenceNumber As String, ByVal archivingId As Long, Optional ByVal targetReportFormat As DeltavistaWebService.TargetReportFormat = DeltavistaWebService.TargetReportFormat.PDF)
        MyBase.New(referenceNumber, targetReportFormat)

        m_ArchivingId = archivingId

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the archivingid.
    ''' </summary>
    ''' <returns>The archivingid.</returns>
    Public ReadOnly Property ArchivingId As Long
        Get
            Return m_ArchivingId
        End Get
    End Property

    ''' <summary>
    ''' Gets the string representation of the report type.
    ''' </summary>
    Public Overrides ReadOnly Property ReportTypeStringRepresentation As String
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets the address.
    ''' </summary>
    Public Overrides ReadOnly Property Address As AddressAbs
        Get
            Return Nothing
        End Get
    End Property


#End Region

End Class
