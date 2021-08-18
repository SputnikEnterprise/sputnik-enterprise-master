Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure
Imports SPProgUtility.Mandanten

Namespace UI
	Public Class ucTimetableAndInfoData

#Region "Private Fields"

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
		''' The RPL data to highlight.
		''' </summary>
		Private m_RPLDataToHighlight As RPLListData

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			m_TimeTable = New TimeTable()

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
				Dim m_Utility As New Utility
				Dim m_md As New Mandant

				Dim value As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/hoursroundkind", FORM_XML_MAIN_KEY))

				Return If(String.IsNullOrWhiteSpace(value) OrElse Val(value) = 1, Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind.Minutes, Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind.HundredthsOfHour)

			End Get
		End Property

#End Region


#Region "Public Methods"

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			Dim previousState = SetSuppressUIEventsState(True)

			m_TimeTable.Clear()
			m_RPLDayDataList = Nothing
			m_RPAbsenceDayData = Nothing
			m_RPLDataToHighlight = Nothing

			btnDone.Text = String.Empty

			ResetTotalHoursGrid()

			SetSuppressUIEventsState(previousState)

		End Sub

		''' <summary>
		''' Loads data of the active report.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadDataOfActiveReport() As Boolean
			SetSuppressUIEventsState(True)
			' Additonal loading code if required ...
			SetSuppressUIEventsState(False)
			Return True
		End Function

		''' <summary>
		''' Loads day total data of an rpl type.
		''' </summary>
		''' <param name="rplTypeToLoad">The rpl type to load.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadDayTotalDataOfRPLType(ByVal rplTypeToLoad As RPLType) As Boolean

			Dim reportData = m_UCMediator.ActiveReportData.ReportData

			CreateTimeTable(reportData.Von, reportData.Bis)

			Dim success As Boolean = True

			success = success AndAlso LoadWorkingHoursColumnData(reportData.RPNR, rplTypeToLoad)
			success = success AndAlso LoadAbsenceCodeColumnData(reportData.RPNR)

			If rplTypeToLoad = RPLType.Employee AndAlso
				Not reportData.IsMonthClosed Then

				Dim reportFinishFlagUpdate = New ReportMng.ReportFinishedFlagUpdater(m_InitializationData, m_Translate)
				success = success AndAlso reportFinishFlagUpdate.UpdateFinishedFlagOfSingleReport(reportData.RPNR)

			End If

			success = success AndAlso LoadReportFinishedFlag()

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stundenplan konnte nicht vollständig initialisiert werden."))
			End If

			Return success
		End Function

		''' <summary>
		''' Higlights RPL time table data.
		''' </summary>
		''' <param name="rplListData">The rpl list data.</param>
		Public Sub HighlightRPLTimeTableData(ByVal rplListData As RPLListData)

			m_RPLDataToHighlight = rplListData

			grdTotalHours.RefreshDataSource()
		End Sub

		Public ReadOnly Property IsReportDone()
			Get
				Dim rpData = m_UCMediator.ActiveReportData
				Dim isReportFinished As Boolean = False
				Dim success As Boolean = m_ReportDataAccess.LoadRPFinishedFlag(rpData.ReportData.RPNR, isReportFinished)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("'Erfasst' Spalte konnte nicht gelesen werden."))
				End If

				Return isReportFinished
			End Get
		End Property

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()
		End Sub

		''' <summary>
		''' Resets the total hours grid.
		''' </summary>
		Private Sub ResetTotalHoursGrid()

			' Reset the grid
			gvTotalHours.OptionsView.ShowIndicator = False
			gvTotalHours.OptionsView.ColumnAutoWidth = True
			gvTotalHours.OptionsSelection.EnableAppearanceFocusedRow = False
			gvTotalHours.OptionsView.ShowFooter = True

			gvTotalHours.Columns.Clear()

			Dim columnCalendarWeek As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCalendarWeek.Caption = m_Translate.GetSafeTranslationValue("W")
			columnCalendarWeek.Name = "CalendarWeek"
			columnCalendarWeek.FieldName = "CalendarWeek"
			columnCalendarWeek.Visible = True
			columnCalendarWeek.OptionsColumn.AllowEdit = False
			gvTotalHours.Columns.Add(columnCalendarWeek)

			Dim columnDayName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDayName.Caption = " "
			columnDayName.Name = "TranslatedDayOfWeekText"
			columnDayName.FieldName = "TranslatedDayOfWeekText"
			columnDayName.Visible = True
			columnDayName.OptionsColumn.AllowEdit = False
			gvTotalHours.Columns.Add(columnDayName)

			Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDate.Caption = m_Translate.GetSafeTranslationValue("Tag")
			columnDate.Name = "DayDate"
			columnDate.FieldName = "DayDate"
			columnDate.Visible = True
			columnDate.OptionsColumn.AllowEdit = False
			gvTotalHours.Columns.Add(columnDate)

			Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTarif.Caption = m_Translate.GetSafeTranslationValue("Std")
			columnTarif.Name = "WorkHours"
			columnTarif.FieldName = "WorkHours"
			columnTarif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTarif.AppearanceHeader.Options.UseTextOptions = True
			columnTarif.Visible = True
			columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTarif.DisplayFormat.FormatString = "N2"
			columnTarif.SummaryItem.DisplayFormat = "{0:n2}"
			columnTarif.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnTarif.SummaryItem.Tag = "SumWorkingHour"

			gvTotalHours.Columns.Add(columnTarif)

			Dim columnAbsenceCode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAbsenceCode.Caption = m_Translate.GetSafeTranslationValue("F")
			columnAbsenceCode.Name = "FehlzeitCode"
			columnAbsenceCode.FieldName = "FehlzeitCode"
			columnAbsenceCode.Visible = True
			columnAbsenceCode.OptionsColumn.AllowEdit = True
			gvTotalHours.Columns.Add(columnAbsenceCode)

			grdTotalHours.DataSource = Nothing

		End Sub

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

			grdTotalHours.DataSource = m_TimeTable.GetTimeTableData

		End Sub

		''' <summary>
		''' Loads working hours column data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <param name="type">The rpl type.</param>
		''' <returns>Boolean falg indicating success.</returns>
		Private Function LoadWorkingHoursColumnData(ByVal rpNr As Integer, ByVal type As RPLType) As Boolean

			Dim success As Boolean = True
			success = success AndAlso LoadRPLDayDataOfReport(rpNr, type)
			success = success AndAlso ApplyRPLDayDataOfReportToTimeTable()

			Return success

		End Function

		''' <summary>
		''' Loads absence code column data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAbsenceCodeColumnData(ByVal rpNr As Integer) As Boolean

			Dim success As Boolean = True

			Dim exists As Boolean? = m_ReportDataAccess.ExistsRPAbsenceDaysDataForRP(rpNr)

			If Not exists.HasValue Then
				m_Logger.LogError(String.Format("ExistsRPAbsenceDaysDataForRP has no value. reportnumber: {0}", rpNr))
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
		''' <param name="type">The RPL type.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadRPLDayDataOfReport(ByVal rpNr As Integer, ByVal type As RPLType)

			' The data should exist -> load it.
			m_RPLDayDataList = m_ReportDataAccess.LoadRPLDayData(rpNr, type)

			If m_RPLDayDataList Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stundenplan-Daten konnten nicht geladen werden."))
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
				m_UtilityUI.ShowErrorDialog("Rapport Fehlzeiten konnte nicht geladen werden.")
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

			grdTotalHours.RefreshDataSource()
			gvTotalHours.BestFitColumns()

			Return True
		End Function

		''' <summary>
		''' Applies RP absence days data to the timte table.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function ApplyRPAbsenceDaysDataToTimeTable() As Boolean

			If Not m_RPAbsenceDayData Is Nothing Then

				Dim absenceCodesOfAllDaysInMonth = m_RPAbsenceDayData.GetAbsenceDayCodesOfAllDaysInMonth()

				If absenceCodesOfAllDaysInMonth Is Nothing Then
					m_Logger.LogError(String.Format("absenceCodesOfAllDaysInMonth is nothing. reportnumber: {0}", m_RPAbsenceDayData.RPNr))
				End If
				m_TimeTable.SetAbsenceCodeOfManyDays(absenceCodesOfAllDaysInMonth.ToArray())

			End If

			grdTotalHours.RefreshDataSource()
			gvTotalHours.BestFitColumns()

			Return True
		End Function

		''' <summary>
		''' Loads the report finished flag.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadReportFinishedFlag() As Boolean

			Dim rpData = m_UCMediator.ActiveReportData
			Dim isReportFinished As Boolean = False
			Dim success As Boolean = m_ReportDataAccess.LoadRPFinishedFlag(rpData.ReportData.RPNR, isReportFinished)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("'Erfasst' Spalte konnte nicht gelesen werden. Rapportnummer: {0}"), rpData.ReportData.RPNR))
			End If

			If isReportFinished Then
				btnDone.Text = m_Translate.GetSafeTranslationValue("Dieser Rapport ist vollständig...")
			Else
				btnDone.Text = m_Translate.GetSafeTranslationValue("Dieser Rapport ist unvollständig...")
			End If

			Return success
		End Function

		''' <summary>
		''' Handles row style event of salary data grid.
		''' </summary>
		Private Sub OnGvSalaryData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvTotalHours.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim timeTableDayData = CType(view.GetRow(e.RowHandle), TimeTableDayData)

				If (Not m_RPLDataToHighlight Is Nothing AndAlso
					 timeTableDayData.DayDate >= m_RPLDataToHighlight.VonDate AndAlso
					 timeTableDayData.DayDate <= m_RPLDataToHighlight.BisDate) Then
					e.Appearance.ForeColor = Color.Red
				End If

			End If

		End Sub

		''' <summary>
		''' Handles custom column display text event of gvTotalHours.
		''' </summary>
		Private Sub OnGvTotalHours_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles gvTotalHours.CustomColumnDisplayText

			If e.Column.FieldName = "WorkHours" Or e.Column.FieldName = "FehlzeitCode" Then

				Try
					Dim dataSourceIndex = e.ListSourceRowIndex

					If (dataSourceIndex >= 0) Then

						Dim view As GridView = CType(sender, GridView)
						'Dim timeTableDayData = CType(view.GetRow(e.GroupRowHandle), TimeTableDayData)
						Dim timeTableDayData = CType(view.GetRow(dataSourceIndex), TimeTableDayData)

						If e.Column.FieldName = "WorkHours" Then
							If (Not timeTableDayData.WorkHours.HasValue) Then
								e.DisplayText = "?"
							End If

						ElseIf e.Column.FieldName = "FehlzeitCode" Then
							If (timeTableDayData.WorkHours.HasValue AndAlso timeTableDayData.WorkHours.Value = 0) And
									String.IsNullOrEmpty(timeTableDayData.FehlzeitCode) Then
								e.DisplayText = "?"
							End If

						End If

					End If

				Catch ex As Exception
					m_Logger.LogError(ex.ToString())
				End Try

			End If
		End Sub

		Private Function GetRowHandle(ByVal dataSourceIndex As Integer) As Integer
			Return dataSourceIndex
		End Function

		Private Sub btnDone_Click(sender As System.Object, e As System.EventArgs) Handles btnDone.Click
			'Dim oMyProg As Object

			Try
				Dim reportNumber As Integer = m_UCMediator.ActiveReportData.ReportData.RPNR
				Dim RPLType As SP.DatabaseAccess.Report.RPLType

				If RPLType.Customer Then
					RPLType = SP.DatabaseAccess.Report.RPLType.Customer
				Else
					RPLType = SP.DatabaseAccess.Report.RPLType.Employee
				End If
				Dim frmTimeTable = New SPS.MA.Guthaben.UI.frmTimeTable(m_InitializationData)

				frmTimeTable.CurrentReportNumber = reportNumber
				frmTimeTable.LoadDayTotalDataOfRPLType(RPLType)

				frmTimeTable.Show()
				frmTimeTable.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler bei der Anzeige der Stunden-Details: {0}", ex.ToString))

			End Try

		End Sub

#End Region


	End Class

End Namespace