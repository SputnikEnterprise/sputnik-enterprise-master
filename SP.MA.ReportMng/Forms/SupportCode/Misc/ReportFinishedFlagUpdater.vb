Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Report
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Initialization
Imports SP.DatabaseAccess.ES
Imports SPProgUtility.Mandanten

Public Class ReportFinishedFlagUpdater

#Region "Private Fields"

	''' <summary>
	''' The common database access.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The es data access object.
	''' </summary>
	Private m_ESDatabaseAccess As IESDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_ReportDataAccess As IReportDatabaseAccess

	''' <summary>
	''' The reprot master data.
	''' </summary>
	Private m_ReportData As SP.DatabaseAccess.Report.DataObjects.RPMasterData

	''' <summary>
	''' The time table
	''' </summary>
	Private m_TimeTable As TimeTable

	''' <summary>
	''' The RPL day data list.
	''' </summary>
	Private m_RPLDayDataList As IEnumerable(Of RPLDayData)

	''' <summary>
	''' The RP absence day data (RP_Fehltage)
	''' </summary>
	Private m_RPAbsenceDayData As RPAbsenceDaysData

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Thre translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal initializationClass As InitializeClass, ByVal translationHelper As TranslateValuesHelper)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		m_InitializationData = initializationClass
		m_Translate = translationHelper

		m_TimeTable = New TimeTable()
		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New DatabaseAccess.ES.ESDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ReportDataAccess = New DatabaseAccess.Report.ReportDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

	End Sub

#End Region

#Region "Private Properties"

	''' <summary>
	''' Gets roundkind
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Private ReadOnly Property GetHoursRoundKind As Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind
		Get

			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			'Dim m_Utility As New Utility
			Dim m_md As New Mandant

			Dim value As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/hoursroundkind", FORM_XML_MAIN_KEY))

			Return If(String.IsNullOrWhiteSpace(value) OrElse Val(value) = 1, Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind.Minutes, Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind.HundredthsOfHour)

		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Updates the finished flag of all report of ES.
	''' </summary>
	''' <param name="esNr">The Es number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function UpdateFinishedFlagOfAllReportsOfES(ByVal esNr As Integer) As Boolean

		Dim success = True

		Dim listOrRPNrs = m_ESDatabaseAccess.LoadExistingReportNumbersForES(esNr)

		If listOrRPNrs Is Nothing Then
			Return success
		End If

		For Each rpNr In listOrRPNrs
			success = success And UpdateFinishedFlagOfSingleReport(rpNr)
		Next

		Return success
	End Function

	''' <summary>
	''' Updates the finished flag of a single rport.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function UpdateFinishedFlagOfSingleReport(ByVal rpNr As Integer) As Boolean

		m_ReportData = m_ReportDataAccess.LoadRPMasterData(rpNr)

		If (m_ReportData Is Nothing) Then
			Return False
		End If

		Return LoadTimeTableDataAndUpdateFinishedFlagOfReport()

	End Function

	''' <summary>
	''' Loads time table data and updates the finished flag of the report.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadTimeTableDataAndUpdateFinishedFlagOfReport() As Boolean

		Dim reportData = m_ReportData

		CreateTimeTable(reportData.Von, reportData.Bis)

		Dim success As Boolean = True

		success = success AndAlso LoadWorkingHoursData(reportData.RPNR)
		success = success AndAlso LoadAbsenceCodeData(reportData.RPNR)

		If Not reportData.IsMonthClosed Then
			success = success AndAlso UpdateFinishedFlagOfReport()
		End If

		Return success
	End Function

#End Region

#Region "Private Methods"

	''' <summary>
	''' Creates a time table between to dates.
	''' </summary>
	''' <param name="dateFrom">The from date.</param>
	''' <param name="dateTo">The to date.</param>
	Private Sub CreateTimeTable(ByVal dateFrom As DateTime, ByVal dateTo As DateTime)

		m_TimeTable.Clear()

		Dim day = dateFrom.Date
		Dim endDate = dateTo.Date
		Dim hoursRoundkind = GetHoursRoundKind

		While day <= endDate

			Dim fehlzeitCode As String = String.Empty

			' Create object for time table day entry.
			Dim dayHourData As TimeTableDayData = New TimeTableDayData(m_Translate, hoursRoundkind) With {
					.DayDate = day.Date,
					.WorkHours = Nothing,
					.FehlzeitCode = String.Empty,
					.Tagesspesen = False,
					.Stundenspesen = False
			}

			m_TimeTable.AddDayHourData(day, dayHourData)

			day = day.AddDays(1)
		End While

	End Sub

	''' <summary>
	''' Loads working hours data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadWorkingHoursData(ByVal rpNr As Integer) As Boolean

		Dim success As Boolean = True
		success = success AndAlso LoadRPLDayDataOfReport(rpNr)
		success = success AndAlso ApplyRPLDayDataOfReportToTimeTable()

		Return success

	End Function

	''' <summary>
	''' Loads absence code data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadAbsenceCodeData(ByVal rpNr As Integer) As Boolean

		Dim success As Boolean = True

		Dim exists As Boolean? = m_ReportDataAccess.ExistsRPAbsenceDaysDataForRP(rpNr)

		If Not exists.HasValue Then
			Return False
		End If

		If exists.Value Then
			success = success AndAlso LoadRPAbsenceDaysData(rpNr)
			success = success AndAlso ApplyRPAbsenceDaysDataToTimeTable()
		End If

		Return success
	End Function

	''' <summary>
	''' Loads RPL day data.
	''' </summary>
	''' <param name="rpNr">The rpNr.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadRPLDayDataOfReport(ByVal rpNr As Integer)

		' The data should exist -> load it.
		m_RPLDayDataList = m_ReportDataAccess.LoadRPLDayData(rpNr, RPLType.Employee)

		If m_RPLDayDataList Is Nothing Then
			Return False
		End If

		Return True

	End Function

	''' <summary>
	''' Loads RPL absence days data.
	''' </summary>
	''' <param name="rpNr">The rpNr.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadRPAbsenceDaysData(ByVal rpNr As Integer) As Boolean

		m_RPAbsenceDayData = m_ReportDataAccess.LoadRPAbsenceDaysData(rpNr)

		If m_RPAbsenceDayData Is Nothing Then
			Return False
		End If

		Return True
	End Function

	''' <summary>
	''' Applies RPL day data to the time table.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function ApplyRPLDayDataOfReportToTimeTable() As Boolean

		If Not m_RPLDayDataList Is Nothing Then

			For Each indvidualRPLDayData In m_RPLDayDataList

				Dim workingHoursOfAllDaysInMonth = indvidualRPLDayData.GetWorkingHoursOfAllDaysInMonth()
				If Not indvidualRPLDayData.isdecimal Then
					For Each s In workingHoursOfAllDaysInMonth
						If s.WorkingHours.HasValue Then s.WorkingHours = TimeTable.NormalToDecimalTime(s.WorkingHours)
					Next
				End If

				m_TimeTable.AddWorkinHoursOfManyDays(workingHoursOfAllDaysInMonth.ToArray())

			Next

		End If

		Return True
	End Function

	''' <summary>
	''' Applies RP absence days data to the timte table.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function ApplyRPAbsenceDaysDataToTimeTable() As Boolean

		If Not m_RPAbsenceDayData Is Nothing Then

			Dim absenceCodesOfAllDaysInMonth = m_RPAbsenceDayData.GetAbsenceDayCodesOfAllDaysInMonth()

			m_TimeTable.SetAbsenceCodeOfManyDays(absenceCodesOfAllDaysInMonth.ToArray())

		End If

		Return True
	End Function

	''' <summary>
	''' Updates  finsihed flag of report ('Erfasst').
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function UpdateFinishedFlagOfReport() As Boolean

		If m_RPLDayDataList Is Nothing Then
			Return True
		End If

		Dim success As Boolean = True

		Dim isRPFinished As Boolean = True

		Dim rpData = m_ReportData

		' Check if there days with working hours that are '0' or nothing and do not have an absence day code.

		Dim timeTableData = m_TimeTable.GetDateAndHourData()

		Dim dateToCheck = m_ReportData.Von.Value.Date
		Dim endDate = m_ReportData.Bis.Value.Date

		While dateToCheck <= endDate

			Dim day = dateToCheck.Day
			Dim absenceCode As String = String.Empty

			If Not m_RPAbsenceDayData Is Nothing Then
				absenceCode = m_RPAbsenceDayData.GetAbsenceDayCodeOfDay(day)
			End If

			If String.IsNullOrEmpty(absenceCode) Then

				Dim dataOfDay = timeTableData.Where(Function(data) data.DayDate = dateToCheck).FirstOrDefault()

				If dataOfDay Is Nothing Then
					' There must be data for this day -> error.
					success = False
					Exit While
				End If

				Dim workingHoursAtThatDay As Decimal? = dataOfDay.WorkingHours

				If (Not workingHoursAtThatDay.HasValue) OrElse
					 (workingHoursAtThatDay.Value = 0) Then
					isRPFinished = False
					Exit While
				End If

			End If

			dateToCheck = dateToCheck.AddDays(1)
		End While

		If success Then
			success = success AndAlso m_ReportDataAccess.UpdateRPFinishedFlag(m_ReportData.RPNR, isRPFinished)
			m_ReportData.Erfasst = isRPFinished
		End If

		Return success
	End Function

#End Region

End Class
