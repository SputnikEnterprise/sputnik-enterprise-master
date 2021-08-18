
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Report

Namespace UI

  ''' <summary>
  ''' Mediator to commuicate between user controls and the parent form.
  ''' </summary>
  Public Class UserControlFormMediator

#Region "Private Fields"

    Private m_ReportFrm As frmReportMng
    Private m_ucMainContent As ucMainContent
    Private m_ucReportDetailData As ucReportDetailData
    Private m_ucReportDetailData2 As ucReportDetailData2
    Private m_ucTimetableAndInfoData As ucTimetableAndInfoData
    Private m_ucNotes As ucNotes
    Private m_ucCredit As ucCredit
    Private m_ucMonthlySalaryData As ucMonthlySalaryData
    Private m_ucAdvancePayment As ucAdvancePayment

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="frmReport">The report form.</param>
    ''' <param name="mainContent">The main content.</param>
    ''' <param name="reportDetailData">The report detail data.</param>
    ''' <param name="reportDetailData2">The report detail data2.</param>
    ''' <param name="timeTableAndInfoData">Time table and info data.</param>
    ''' <param name="notes">The notes data.</param>
    ''' <param name="credit">The credit data.</param>
    ''' <param name="monthlySalaryData">The monthly salary data.</param>
    ''' <param name="advancePayment">The advance payment data.</param>
    Public Sub New(ByVal frmReport As frmReportMng,
                   ByVal mainContent As ucMainContent,
                   ByVal reportDetailData As ucReportDetailData,
                   ByVal reportDetailData2 As ucReportDetailData2,
                   ByVal timeTableAndInfoData As ucTimetableAndInfoData,
                   ByVal notes As ucNotes,
                   ByVal credit As ucCredit,
                   ByVal monthlySalaryData As ucMonthlySalaryData,
                   ByVal advancePayment As ucAdvancePayment)

      Me.m_ReportFrm = frmReport
      Me.m_ucMainContent = mainContent
      Me.m_ucReportDetailData = reportDetailData
      Me.m_ucReportDetailData2 = reportDetailData2
      Me.m_ucTimetableAndInfoData = timeTableAndInfoData
      Me.m_ucNotes = notes
      Me.m_ucCredit = credit
      Me.m_ucMonthlySalaryData = monthlySalaryData
      Me.m_ucAdvancePayment = advancePayment

      m_ucMainContent.UCMediator = Me
      m_ucReportDetailData.UCMediator = Me
      m_ucReportDetailData2.UCMediator = Me
      m_ucTimetableAndInfoData.UCMediator = Me
      m_ucNotes.UCMediator = Me
      m_ucCredit.UCMediator = Me
      m_ucMonthlySalaryData.UCMediator = Me
      m_ucAdvancePayment.UCMediator = Me

    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets the active report data.
    ''' </summary>
    ''' <returns>The active report data.</returns>
    Public ReadOnly Property ActiveReportData As ActiveReportData
      Get
        Return m_ReportFrm.AcitiveReportData
      End Get

    End Property

    ''' <summary>
    ''' Gets the selected ES salary data.
    ''' </summary>
    ''' <returns>The selected ES salary data or nothing if none is selected.</returns>
    Public ReadOnly Property SelectedESSalaryData As ESSalaryData
      Get
        Return m_ucReportDetailData.SelectedESSalaryData
      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Handles change of ES salary data selection.
    ''' </summary>
    Public Sub HandleESSalaryDataSelectionHasChange()
      m_ucMainContent.HandleESSalaryDataSelectionChange()

      Dim selectedESData = SelectedESSalaryData

			If selectedESData Is Nothing Then Return
			m_ReportFrm.CheckGAVValidityOfESSalaryData(selectedESData)

		End Sub

    ''' <summary>
    ''' Loads day total data of an rpl type.
    ''' </summary>
    ''' <param name="rplTypeToLoad">The rpl type to load.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function LoadDayTotalDataOfRPLType(ByVal rplTypeToLoad As RPLType) As Boolean

      Return m_ucTimetableAndInfoData.LoadDayTotalDataOfRPLType(rplTypeToLoad)

    End Function

		''' <summary>
		''' is active report done now?
		''' </summary>
		Public Function IsAktiveReportDone() As Boolean

			Return m_ucTimetableAndInfoData.IsReportDone()

		End Function
		''' <summary>
		''' Highlights RPL time table data.
		''' </summary>
		''' <param name="rplListData">The rpl list data.</param>
		Public Sub HighlightRPLTimeTableData(ByVal rplListData As RPLListData)
      m_ucTimetableAndInfoData.HighlightRPLTimeTableData(rplListData)
    End Sub

    ''' <summary>
    ''' Updates RPL change time in status bar.
    ''' </summary>
    ''' <param name="rplChangeTime">The rpl change time.</param>
    ''' <param name="changedFrom">The changed from string.</param>
    Public Sub UpdateRPLChangeTimeInStatusBar(ByVal rplChangeTime As DateTime?, ByVal changedFrom As String)
      m_ReportFrm.UpdateRPLChangeTimeInStatusBar(rplChangeTime, changedFrom)
    End Sub

    ''' <summary>
    ''' Checks GAV validity of ES salary data.
    ''' </summary>
    ''' <param name="esLohn">The ES Salary data.</param>
    Public Sub CheckGAVValidityOfESSalaryData(ByVal esLohn As ESSalaryData)
			m_ReportFrm.CheckGAVValidityOfESSalaryData(esLohn)
		End Sub

#End Region

  End Class

End Namespace