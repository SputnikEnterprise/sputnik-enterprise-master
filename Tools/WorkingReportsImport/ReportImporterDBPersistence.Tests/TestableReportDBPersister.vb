'------------------------------------
' File: TestableReportDBPersister.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Testable sub class of ReportDBPersister.
''' </summary>
Public Class TestableReportDBPersister
    Inherits ReportDBPersister

#Region "Private Fields"

    ''' <summary>
    ''' The number of existing reports that should be returned for testing. 
    ''' </summary>
    Private returnNumberOfExistingReports As Integer

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="scanJobsConnectionString">The connection string to the ScanJobs database.</param>
    ''' <param name="returnNumberOfExistingReports">The number of existing reports that should be returned for testing.</param>
    Public Sub New(ByVal scanJobsConnectionString As String, ByVal returnNumberOfExistingReports As Integer)
		MyBase.New(scanJobsConnectionString, "DE") 'scanJobsConnectionString)

		Me.returnNumberOfExistingReports = returnNumberOfExistingReports
    End Sub

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' Overrides the ReadNumberOfExistingReports method of the ReportDBPersister base class for testing.
    ''' </summary>
    ''' <param name="reportDBInformation">The report db information object.</param>
    ''' <returns>The configured number of existing reports.</returns>
    Protected Overrides Function ReadNumberOfExistingReports(ByVal reportDBInformation As ReportDBInformation) As Integer?
        Return Me.returnNumberOfExistingReports
    End Function

#End Region

End Class
