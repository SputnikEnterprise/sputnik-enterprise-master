Imports SP.DatabaseAccess.Report.DataObjects

Namespace UI

  Public Class ucBaseControlBottomTab

#Region "Protected Fields"

    ''' <summary>
    ''' Contains the report number.
    ''' </summary>
    Protected m_ReportNumber As Integer?

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Boolean flag indicating if report data is loaded.
    ''' </summary>
    Public ReadOnly Property IsReportDataLoaded As Boolean
      Get
        Return m_ReportNumber.HasValue
      End Get

    End Property

#End Region

  End Class

End Namespace
