Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.MA.ReportMng.TimeTable
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.ES

''' <summary>
''' Create RP service.
''' </summary>
Public Class CreateRPService

#Region "Private Fields"

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger = New Logger()

  ''' <summary>
  ''' UI Utility functions.
  ''' </summary>
  Protected m_UtilityUI As UtilityUI

  ''' <summary>
  ''' The Initialization data.
  ''' </summary>
  Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

  ''' <summary>
  ''' The data access object.
  ''' </summary>
  Protected m_ReportDataAccess As IReportDatabaseAccess

  ''' <summary>
  ''' The data access object.
  ''' </summary>
  Protected m_ESDataAccess As IESDatabaseAccess

  ''' <summary>
  ''' The mandant.
  ''' </summary>
  Private m_md As Mandant

  ''' <summary>
  ''' The cls prog path.
  ''' </summary>
  Private m_ProgPath As ClsProgPath

  ''' <summary>
  ''' The SPProgUtility object.
  ''' </summary>
  Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  ''' <param name="mdNr">The mandant number.</param>
  ''' <param name="_setting">The settings object.</param>
  Public Sub New(ByVal mdNr As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Try
      m_md = New Mandant
      m_ProgPath = New ClsProgPath
      m_InitializationData = _setting
      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    End Try

    m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

    Dim conStr = m_md.GetSelectedMDData(mdNr).MDDbConn

    m_ReportDataAccess = New DatabaseAccess.Report.ReportDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
    m_ESDataAccess = New DatabaseAccess.ES.ESDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Creates a report copy for an existing Es.
  ''' </summary>
  ''' <param name="params">The parameters.</param>
  Public Sub CreateReportCopyForExistingES(ByVal params As CreateRPCopyForExistingESParams)

    Dim firstDayOfMonth = New DateTime(params.Year_OfReportToCopy, params.Month_OfReportToCopy, 1)
    Dim firstDayOfNextMonth = firstDayOfMonth.AddMonths(1)
    Dim lastDayOfNextMonth = firstDayOfNextMonth.AddMonths(1).AddDays(-1)

    Dim es = m_ESDataAccess.LoadESMasterData(params.ESNr)

    If es Is Nothing Then
      params.ResultCode = CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultFailure
      Return
    End If

    Dim esEnde As DateTime? = es.ES_Ende

    ' Check if ES ends before start of next month
    If esEnde.HasValue AndAlso esEnde.Value.Date < firstDayOfNextMonth Then
      params.ResultCode = CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultNoMoreReportsAllowed
      Return
    End If

    ' Check if there is already a report on next moth.
    Dim foundRPNr As Integer? = Nothing
    Dim foundRPID As Integer? = Nothing
    Dim successDBCall As Boolean = m_ReportDataAccess.FindRPNrByESNrMonthAndYear(params.ESNr, firstDayOfNextMonth.Month, firstDayOfNextMonth.Year, foundRPNr, foundRPID)

    If Not successDBCall Then
      params.ResultCode = CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultFailure
      Return
    ElseIf foundRPNr.HasValue Then
      params.ResultCode = CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultReportForNextMonthIsAlreadyExisting
      params.NewIdRPOutput = foundRPID
      params.NewRPNrOutput = foundRPNr
      Return
    End If

    Dim newRPVon = firstDayOfNextMonth
    Dim newRPBis = If(esEnde.HasValue AndAlso esEnde.Value.Date < lastDayOfNextMonth,
                      esEnde.Value.Date,
                      lastDayOfNextMonth)

    Dim iniData As New NewRPForExistingESData

    iniData.ESNr = params.ESNr
    iniData.RPMonth = firstDayOfNextMonth.Month
    iniData.RPYear = firstDayOfNextMonth.Year.ToString()
    iniData.RPVon = newRPVon
    iniData.RPBis = newRPBis
    iniData.RPSuva = params.Suva_OfReprotToCopy
    iniData.RPKst = params.Kst_OfReportToCopy
    iniData.RPKst1 = params.Kst1_OfReportToCopy
    iniData.RPKst2 = params.Kst2_OfReportToCopy
    iniData.RPKDBranche = params.KDBranche_OfReportToCopy
    iniData.MDNr = params.MDNr
    iniData.RPNumberOffset = params.RPNumberOffset
    iniData.CreatedFrom = params.UserName

    successDBCall = m_ReportDataAccess.AddNewRPForExistingES(iniData)

    If Not successDBCall Then
      params.ResultCode = CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultFailure
    Else
      params.ResultCode = CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultSuccess
      params.NewRPNrOutput = iniData.NewRPNrOutput
      params.NewIdRPOutput = iniData.NewIdRPOutput
    End If

  End Sub

#End Region

End Class
