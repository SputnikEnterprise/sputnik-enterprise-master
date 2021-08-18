Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Customer
Imports DevExpress.XtraGrid
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.Globalization
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Data

Namespace UI

	Public Class frmTimeTable

#Region "Private Fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		Private m_ESDatabaseAccess As IESDatabaseAccess
		Private m_ListingDatabaseAccess As IListingDatabaseAccess
		Private m_ReportDatabaseAccess As IReportDatabaseAccess

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility
		Private m_DateUtility As DateAndTimeUtily

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connectionString As String

		''' <summary>
		''' Report overview data.
		''' </summary>
		Private m_ReportMasterData As RPMasterData

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
		Private m_HourAbsencedata As ZVHourAbsenceData
		Private m_HourGroupedByKSTNrdata As IEnumerable(Of WorkingHourGroupedWithKSTNrData)

		''' <summary>
		''' The RPL data to highlight.
		''' </summary>
		Private m_RPLDataToHighlight As RPLListData
		Private m_RPLTypeToLoad As RPLType
		Private m_CurrentEmployeeNumber As Integer
		Private m_CurrentCustomerNumber As Integer


#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_mandant = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_DateUtility = New DateAndTimeUtily
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ReportDatabaseAccess = New ReportDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			'm_TimeTable = New TimeTable()

			Reset()

		End Sub

#End Region


#Region "Private Properties"

		''' <summary>
		''' Gets roundkind
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetHoursRoundKind As DateAndTimeUtily.HoursRoundKind
			Get

				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim m_Utility As New Utility
				Dim m_md As New Mandant

				Dim value As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/hoursroundkind", FORM_XML_MAIN_KEY))

				Return If(String.IsNullOrWhiteSpace(value) OrElse Val(value) = 1, DateAndTimeUtily.HoursRoundKind.Minutes, DateAndTimeUtily.HoursRoundKind.HundredthsOfHour)

			End Get
		End Property

#End Region


#Region "public properties"

		Public Property CurrentReportNumber As Integer

#End Region

#Region "Public Methods"


		''' <summary>
		''' Loads day total data of an rpl type.
		''' </summary>
		''' <param name="rplTypeToLoad">The rpl type to load.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadDayTotalDataOfRPLType(ByVal rplTypeToLoad As RPLType) As Boolean
			Dim success As Boolean = True

			m_RPLTypeToLoad = rplTypeToLoad
			success = success AndAlso LoadReportDataInternal(CurrentReportNumber)
			ResetTotalHoursGroupedGrid(m_ReportMasterData.Von, m_ReportMasterData.Bis)

			If rplTypeToLoad = RPLType.Employee Then
				CreateEmployeeHourData(m_ReportMasterData.Von, m_ReportMasterData.Bis)
				CreateEmployeeHourGroupedByKSNrData(m_CurrentEmployeeNumber, m_ReportMasterData.Von, m_ReportMasterData.Bis)

			Else
				CreateEmployeeHourData(m_ReportMasterData.Von, m_ReportMasterData.Bis)
				CreateEmployeeHourGroupedByKSNrData(m_CurrentEmployeeNumber, m_ReportMasterData.Von, m_ReportMasterData.Bis)
			End If


			'CreateTimeTable(m_ReportMasterData.Von, m_ReportMasterData.Bis)
			'success = success AndAlso LoadWorkingHoursColumnData(m_ReportMasterData.RPNR, rplTypeToLoad)
			'success = success AndAlso LoadAbsenceCodeColumnData(m_ReportMasterData.RPNR)

			'success = success AndAlso LoadReportFinishedFlag()

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stundenplan konnte nicht vollständig initialisiert werden."))
			End If

			Return success
		End Function


#End Region



#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

		End Sub

#Region "rest data"

		''' <summary>
		''' Resets the control.
		''' </summary>
		Private Sub Reset()

			'm_TimeTable.Clear()
			m_RPLDayDataList = Nothing
			m_RPAbsenceDayData = Nothing
			m_RPLDataToHighlight = Nothing

			ResetTotalHoursGrid()

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
			columnCalendarWeek.Caption = m_Translate.GetSafeTranslationValue("Woche")
			columnCalendarWeek.Name = "CalendarWeek"
			columnCalendarWeek.FieldName = "CalendarWeek"
			columnCalendarWeek.Visible = True
			columnCalendarWeek.OptionsColumn.AllowEdit = False
			gvTotalHours.Columns.Add(columnCalendarWeek)

			Dim columnWorkingDay As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWorkingDay.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnWorkingDay.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnWorkingDay.Name = "WorkingDay"
			columnWorkingDay.FieldName = "WorkingDay"
			columnWorkingDay.MaxWidth = 100
			columnWorkingDay.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnWorkingDay.DisplayFormat.FormatString = "d"
			columnWorkingDay.Visible = True
			gvTotalHours.Columns.Add(columnWorkingDay)

			Dim columnWorkingHour As New DevExpress.XtraGrid.Columns.GridColumn()
			columnWorkingHour.Caption = m_Translate.GetSafeTranslationValue("Stunden")
			columnWorkingHour.Name = "WorkingHour"
			columnWorkingHour.FieldName = "WorkingHour"
			columnWorkingHour.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnWorkingHour.AppearanceHeader.Options.UseTextOptions = True
			columnWorkingHour.Visible = True
			columnWorkingHour.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnWorkingHour.DisplayFormat.FormatString = "N2"
			columnWorkingHour.SummaryItem.DisplayFormat = "{0:n2}"
			columnWorkingHour.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnWorkingHour.SummaryItem.Tag = "SumWorkingHour"
			columnWorkingHour.MaxWidth = 200
			gvTotalHours.Columns.Add(columnWorkingHour)

			Dim columnAbsenceCode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAbsenceCode.Caption = m_Translate.GetSafeTranslationValue("Fehlcode")
			columnAbsenceCode.Name = "AbsenceCode"
			columnAbsenceCode.FieldName = "AbsenceCode"
			columnAbsenceCode.Visible = True
			columnAbsenceCode.Width = 30
			gvTotalHours.Columns.Add(columnAbsenceCode)

			Dim grpAuthorizedItems = New GridGroupSummaryItem()
			grpAuthorizedItems.FieldName = "WorkingHour"
			grpAuthorizedItems.SummaryType = DevExpress.Data.SummaryItemType.Sum
			grpAuthorizedItems.DisplayFormat = m_Translate.GetSafeTranslationValue("Stundentotal") & ": {0:n2}"
			gvTotalHours.GroupFormat = "{0} {1}: {2}"
			gvTotalHours.GroupSummary.Add(grpAuthorizedItems)
			'gvTotalHours.Columns(0).OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True

			gvTotalHours.BeginSort()
			Try
				gvTotalHours.ClearGrouping()
				gvTotalHours.Columns("CalendarWeek").GroupIndex = 0
				'gvTotalHours.Columns("Category").GroupIndex = 1
			Finally
				gvTotalHours.EndSort()
			End Try


			grdTotalHours.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the total hours grid grouped by kstnr.
		''' </summary>
		Private Sub ResetTotalHoursGroupedGrid(ByVal dateFrom As DateTime, ByVal dateTo As DateTime)

			' Reset the grid
			gvTotalHoursGrouped.OptionsView.ShowIndicator = False
			gvTotalHoursGrouped.OptionsView.ColumnAutoWidth = True
			gvTotalHoursGrouped.OptionsSelection.EnableAppearanceFocusedRow = False
			gvTotalHoursGrouped.OptionsView.ShowFooter = True

			gvTotalHoursGrouped.Columns.Clear()


			Dim columnCalendarWeek As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCalendarWeek.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
			columnCalendarWeek.Name = "KSTBez"
			columnCalendarWeek.FieldName = "KSTBez"
			columnCalendarWeek.Visible = True
			columnCalendarWeek.Width = 200
			columnCalendarWeek.OptionsColumn.AllowEdit = False
			gvTotalHoursGrouped.Columns.Add(columnCalendarWeek)

			For i As Integer = dateFrom.Day To dateTo.Day
				Dim columnTag As New DevExpress.XtraGrid.Columns.GridColumn()
				columnTag.Caption = String.Format("{0:dd.MM}", New DateTime(dateFrom.Year, dateFrom.Month, i))
				columnTag.Name = String.Format("Tag{0}", i)
				columnTag.FieldName = String.Format("Tag{0}", i)
				columnTag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
				columnTag.AppearanceHeader.Options.UseTextOptions = True
				columnTag.Visible = True
				columnTag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
				columnTag.DisplayFormat.FormatString = "N2"
				columnTag.SummaryItem.DisplayFormat = "{0:n2}"
				columnTag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
				columnTag.SummaryItem.Tag = String.Format("SumTag{0}", i)
				columnTag.BestFit()
				gvTotalHoursGrouped.Columns.Add(columnTag)

			Next

			Dim columnTotalAmountOfHours As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTotalAmountOfHours.Caption = m_Translate.GetSafeTranslationValue("Total")
			columnTotalAmountOfHours.Name = "TotalAmountOfHours"
			columnTotalAmountOfHours.FieldName = "TotalAmountOfHours"
			columnTotalAmountOfHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTotalAmountOfHours.AppearanceHeader.Options.UseTextOptions = True
			columnTotalAmountOfHours.Visible = True
			columnTotalAmountOfHours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTotalAmountOfHours.DisplayFormat.FormatString = "N2"
			columnTotalAmountOfHours.SummaryItem.DisplayFormat = "{0:n2}"
			columnTotalAmountOfHours.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnTotalAmountOfHours.SummaryItem.Tag = "SumEachKST"
			columnTotalAmountOfHours.BestFit()
			gvTotalHoursGrouped.Columns.Add(columnTotalAmountOfHours)

			Dim grpAuthorizedItems = New GridGroupSummaryItem()
			grpAuthorizedItems.FieldName = "TotalAmountOfHours"
			grpAuthorizedItems.SummaryType = DevExpress.Data.SummaryItemType.Average
			grpAuthorizedItems.DisplayFormat = m_Translate.GetSafeTranslationValue("Stundentotal") & ": {0:n2}"
			gvTotalHoursGrouped.GroupFormat = "{1}: {2}"
			gvTotalHoursGrouped.GroupSummary.Add(grpAuthorizedItems)
			'gvTotalHoursGrouped.Columns(0).OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True

			gvTotalHoursGrouped.BeginSort()
			Try
				gvTotalHoursGrouped.ClearGrouping()
				gvTotalHoursGrouped.Columns("KSTBez").GroupIndex = 0
				'gvTotalHoursGrouped.Columns("Category").GroupIndex = 1
			Finally
				gvTotalHoursGrouped.EndSort()
			End Try


			grdTotalHoursGrouped.DataSource = Nothing

		End Sub

		'Dim columnTag1 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag1.Caption = m_Translate.GetSafeTranslationValue("Tag1")
		'columnTag1.Name = "Tag1"
		'columnTag1.FieldName = "Tag1"
		'columnTag1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag1.AppearanceHeader.Options.UseTextOptions = True
		'columnTag1.Visible = True
		'columnTag1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag1.DisplayFormat.FormatString = "N2"
		'columnTag1.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag1.SummaryItem.Tag = "SumTag1"
		'gvTotalHoursGrouped.Columns.Add(columnTag1)

		'Dim columnTag2 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag2.Caption = m_Translate.GetSafeTranslationValue("Tag2")
		'columnTag2.Name = "Tag2"
		'columnTag2.FieldName = "Tag2"
		'columnTag2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag2.AppearanceHeader.Options.UseTextOptions = True
		'columnTag2.Visible = True
		'columnTag2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag2.DisplayFormat.FormatString = "N2"
		'columnTag2.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag2.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag2.SummaryItem.Tag = "SumTag2"
		'gvTotalHoursGrouped.Columns.Add(columnTag2)

		'Dim columnTag3 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag3.Caption = m_Translate.GetSafeTranslationValue("Tag3")
		'columnTag3.Name = "Tag3"
		'columnTag3.FieldName = "Tag3"
		'columnTag3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag3.AppearanceHeader.Options.UseTextOptions = True
		'columnTag3.Visible = True
		'columnTag3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag3.DisplayFormat.FormatString = "N2"
		'columnTag3.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag3.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag3.SummaryItem.Tag = "SumTag3"
		'gvTotalHoursGrouped.Columns.Add(columnTag3)

		'Dim columnTag4 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag4.Caption = m_Translate.GetSafeTranslationValue("Tag4")
		'columnTag4.Name = "Tag4"
		'columnTag4.FieldName = "Tag4"
		'columnTag4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag4.AppearanceHeader.Options.UseTextOptions = True
		'columnTag4.Visible = True
		'columnTag4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag4.DisplayFormat.FormatString = "N2"
		'columnTag4.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag4.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag4.SummaryItem.Tag = "SumTag4"
		'gvTotalHoursGrouped.Columns.Add(columnTag4)

		'Dim columnTag5 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag5.Caption = m_Translate.GetSafeTranslationValue("Tag5")
		'columnTag5.Name = "Tag5"
		'columnTag5.FieldName = "Tag5"
		'columnTag5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag5.AppearanceHeader.Options.UseTextOptions = True
		'columnTag5.Visible = True
		'columnTag5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag5.DisplayFormat.FormatString = "N2"
		'columnTag5.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag5.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag5.SummaryItem.Tag = "SumTag5"
		'gvTotalHoursGrouped.Columns.Add(columnTag5)

		'Dim columnTag6 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag6.Caption = m_Translate.GetSafeTranslationValue("Tag6")
		'columnTag6.Name = "Tag6"
		'columnTag6.FieldName = "Tag6"
		'columnTag6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag6.AppearanceHeader.Options.UseTextOptions = True
		'columnTag6.Visible = True
		'columnTag6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag6.DisplayFormat.FormatString = "N2"
		'columnTag6.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag6.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag6.SummaryItem.Tag = "SumTag6"
		'gvTotalHoursGrouped.Columns.Add(columnTag6)

		'Dim columnTag7 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag7.Caption = m_Translate.GetSafeTranslationValue("Tag7")
		'columnTag7.Name = "Tag7"
		'columnTag7.FieldName = "Tag7"
		'columnTag7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag7.AppearanceHeader.Options.UseTextOptions = True
		'columnTag7.Visible = True
		'columnTag7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag7.DisplayFormat.FormatString = "N2"
		'columnTag7.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag7.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag7.SummaryItem.Tag = "SumTag7"
		'gvTotalHoursGrouped.Columns.Add(columnTag7)

		'Dim columnTag8 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag8.Caption = m_Translate.GetSafeTranslationValue("Tag8")
		'columnTag8.Name = "Tag8"
		'columnTag8.FieldName = "Tag8"
		'columnTag8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag8.AppearanceHeader.Options.UseTextOptions = True
		'columnTag8.Visible = True
		'columnTag8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag8.DisplayFormat.FormatString = "N2"
		'columnTag8.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag8.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag8.SummaryItem.Tag = "SumTag8"
		'gvTotalHoursGrouped.Columns.Add(columnTag8)

		'Dim columnTag9 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag9.Caption = m_Translate.GetSafeTranslationValue("Tag9")
		'columnTag9.Name = "Tag9"
		'columnTag9.FieldName = "Tag9"
		'columnTag9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag9.AppearanceHeader.Options.UseTextOptions = True
		'columnTag9.Visible = True
		'columnTag9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag9.DisplayFormat.FormatString = "N2"
		'columnTag9.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag9.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag9.SummaryItem.Tag = "SumTag9"
		'gvTotalHoursGrouped.Columns.Add(columnTag9)

		'Dim columnTag10 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag10.Caption = m_Translate.GetSafeTranslationValue("Tag10")
		'columnTag10.Name = "Tag10"
		'columnTag10.FieldName = "Tag10"
		'columnTag10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag10.AppearanceHeader.Options.UseTextOptions = True
		'columnTag10.Visible = True
		'columnTag10.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag10.DisplayFormat.FormatString = "N2"
		'columnTag10.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag10.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag10.SummaryItem.Tag = "SumTag10"
		'gvTotalHoursGrouped.Columns.Add(columnTag10)



		'Dim columnTag11 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag11.Caption = m_Translate.GetSafeTranslationValue("Tag11")
		'columnTag11.Name = "Tag11"
		'columnTag11.FieldName = "Tag11"
		'columnTag11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag11.AppearanceHeader.Options.UseTextOptions = True
		'columnTag11.Visible = True
		'columnTag11.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag11.DisplayFormat.FormatString = "N2"
		'columnTag11.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag11.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag11.SummaryItem.Tag = "SumTag11"
		'gvTotalHoursGrouped.Columns.Add(columnTag11)

		'Dim columnTag12 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag12.Caption = m_Translate.GetSafeTranslationValue("Tag12")
		'columnTag12.Name = "Tag12"
		'columnTag12.FieldName = "Tag12"
		'columnTag12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag12.AppearanceHeader.Options.UseTextOptions = True
		'columnTag12.Visible = True
		'columnTag12.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag12.DisplayFormat.FormatString = "N2"
		'columnTag12.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag12.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag12.SummaryItem.Tag = "SumTag12"
		'gvTotalHoursGrouped.Columns.Add(columnTag12)

		'Dim columnTag13 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag13.Caption = m_Translate.GetSafeTranslationValue("Tag13")
		'columnTag13.Name = "Tag13"
		'columnTag13.FieldName = "Tag13"
		'columnTag13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag13.AppearanceHeader.Options.UseTextOptions = True
		'columnTag13.Visible = True
		'columnTag13.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag13.DisplayFormat.FormatString = "N2"
		'columnTag13.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag13.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag13.SummaryItem.Tag = "SumTag13"
		'gvTotalHoursGrouped.Columns.Add(columnTag13)

		'Dim columnTag14 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag14.Caption = m_Translate.GetSafeTranslationValue("Tag14")
		'columnTag14.Name = "Tag14"
		'columnTag14.FieldName = "Tag14"
		'columnTag14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag14.AppearanceHeader.Options.UseTextOptions = True
		'columnTag14.Visible = True
		'columnTag14.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag14.DisplayFormat.FormatString = "N2"
		'columnTag14.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag14.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag14.SummaryItem.Tag = "SumTag14"
		'gvTotalHoursGrouped.Columns.Add(columnTag14)

		'Dim columnTag15 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag15.Caption = m_Translate.GetSafeTranslationValue("Tag15")
		'columnTag15.Name = "Tag15"
		'columnTag15.FieldName = "Tag15"
		'columnTag15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag15.AppearanceHeader.Options.UseTextOptions = True
		'columnTag15.Visible = True
		'columnTag15.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag15.DisplayFormat.FormatString = "N2"
		'columnTag15.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag15.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag15.SummaryItem.Tag = "SumTag15"
		'gvTotalHoursGrouped.Columns.Add(columnTag15)

		'Dim columnTag16 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag16.Caption = m_Translate.GetSafeTranslationValue("Tag16")
		'columnTag16.Name = "Tag16"
		'columnTag16.FieldName = "Tag16"
		'columnTag16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag16.AppearanceHeader.Options.UseTextOptions = True
		'columnTag16.Visible = True
		'columnTag16.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag16.DisplayFormat.FormatString = "N2"
		'columnTag16.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag16.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag16.SummaryItem.Tag = "SumTag16"
		'gvTotalHoursGrouped.Columns.Add(columnTag16)

		'Dim columnTag17 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag17.Caption = m_Translate.GetSafeTranslationValue("Tag17")
		'columnTag17.Name = "Tag17"
		'columnTag17.FieldName = "Tag17"
		'columnTag17.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag17.AppearanceHeader.Options.UseTextOptions = True
		'columnTag17.Visible = True
		'columnTag17.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag17.DisplayFormat.FormatString = "N2"
		'columnTag17.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag17.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag17.SummaryItem.Tag = "SumTag17"
		'gvTotalHoursGrouped.Columns.Add(columnTag17)

		'Dim columnTag18 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag18.Caption = m_Translate.GetSafeTranslationValue("Tag18")
		'columnTag18.Name = "Tag18"
		'columnTag18.FieldName = "Tag18"
		'columnTag18.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag18.AppearanceHeader.Options.UseTextOptions = True
		'columnTag18.Visible = True
		'columnTag18.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag18.DisplayFormat.FormatString = "N2"
		'columnTag18.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag18.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag18.SummaryItem.Tag = "SumTag18"
		'gvTotalHoursGrouped.Columns.Add(columnTag18)

		'Dim columnTag19 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag19.Caption = m_Translate.GetSafeTranslationValue("Tag19")
		'columnTag19.Name = "Tag19"
		'columnTag19.FieldName = "Tag19"
		'columnTag19.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag19.AppearanceHeader.Options.UseTextOptions = True
		'columnTag19.Visible = True
		'columnTag19.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag19.DisplayFormat.FormatString = "N2"
		'columnTag19.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag19.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag19.SummaryItem.Tag = "SumTag19"
		'gvTotalHoursGrouped.Columns.Add(columnTag19)

		'Dim columnTag20 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag20.Caption = m_Translate.GetSafeTranslationValue("Tag20")
		'columnTag20.Name = "Tag20"
		'columnTag20.FieldName = "Tag20"
		'columnTag20.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag20.AppearanceHeader.Options.UseTextOptions = True
		'columnTag20.Visible = True
		'columnTag20.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag20.DisplayFormat.FormatString = "N2"
		'columnTag20.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag20.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag20.SummaryItem.Tag = "SumTag20"
		'gvTotalHoursGrouped.Columns.Add(columnTag19)



		'Dim columnTag21 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag21.Caption = m_Translate.GetSafeTranslationValue("Tag21")
		'columnTag21.Name = "Tag21"
		'columnTag21.FieldName = "Tag21"
		'columnTag21.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag21.AppearanceHeader.Options.UseTextOptions = True
		'columnTag21.Visible = True
		'columnTag21.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag21.DisplayFormat.FormatString = "N2"
		'columnTag21.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag21.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag21.SummaryItem.Tag = "SumTag21"
		'gvTotalHoursGrouped.Columns.Add(columnTag21)

		'Dim columnTag22 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag22.Caption = m_Translate.GetSafeTranslationValue("Tag22")
		'columnTag22.Name = "Tag22"
		'columnTag22.FieldName = "Tag22"
		'columnTag22.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag22.AppearanceHeader.Options.UseTextOptions = True
		'columnTag22.Visible = True
		'columnTag22.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag22.DisplayFormat.FormatString = "N2"
		'columnTag22.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag22.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag22.SummaryItem.Tag = "SumTag22"
		'gvTotalHoursGrouped.Columns.Add(columnTag22)

		'Dim columnTag23 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag23.Caption = m_Translate.GetSafeTranslationValue("Tag23")
		'columnTag23.Name = "Tag23"
		'columnTag23.FieldName = "Tag23"
		'columnTag23.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag23.AppearanceHeader.Options.UseTextOptions = True
		'columnTag23.Visible = True
		'columnTag23.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag23.DisplayFormat.FormatString = "N2"
		'columnTag23.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag23.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag23.SummaryItem.Tag = "SumTag23"
		'gvTotalHoursGrouped.Columns.Add(columnTag23)

		'Dim columnTag24 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag24.Caption = m_Translate.GetSafeTranslationValue("Tag24")
		'columnTag24.Name = "Tag24"
		'columnTag24.FieldName = "Tag24"
		'columnTag24.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag24.AppearanceHeader.Options.UseTextOptions = True
		'columnTag24.Visible = True
		'columnTag24.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag24.DisplayFormat.FormatString = "N2"
		'columnTag24.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag24.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag24.SummaryItem.Tag = "SumTag24"
		'gvTotalHoursGrouped.Columns.Add(columnTag24)

		'Dim columnTag25 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag25.Caption = m_Translate.GetSafeTranslationValue("Tag25")
		'columnTag25.Name = "Tag25"
		'columnTag25.FieldName = "Tag25"
		'columnTag25.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag25.AppearanceHeader.Options.UseTextOptions = True
		'columnTag25.Visible = True
		'columnTag25.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag25.DisplayFormat.FormatString = "N2"
		'columnTag25.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag25.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag25.SummaryItem.Tag = "SumTag25"
		'gvTotalHoursGrouped.Columns.Add(columnTag25)

		'Dim columnTag26 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag26.Caption = m_Translate.GetSafeTranslationValue("Tag26")
		'columnTag26.Name = "Tag26"
		'columnTag26.FieldName = "Tag26"
		'columnTag26.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag26.AppearanceHeader.Options.UseTextOptions = True
		'columnTag26.Visible = True
		'columnTag26.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag26.DisplayFormat.FormatString = "N2"
		'columnTag26.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag26.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag26.SummaryItem.Tag = "SumTag26"
		'gvTotalHoursGrouped.Columns.Add(columnTag26)

		'Dim columnTag27 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag27.Caption = m_Translate.GetSafeTranslationValue("Tag27")
		'columnTag27.Name = "Tag27"
		'columnTag27.FieldName = "Tag27"
		'columnTag27.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag27.AppearanceHeader.Options.UseTextOptions = True
		'columnTag27.Visible = True
		'columnTag27.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag27.DisplayFormat.FormatString = "N2"
		'columnTag27.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag27.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag27.SummaryItem.Tag = "SumTag27"
		'gvTotalHoursGrouped.Columns.Add(columnTag27)

		'Dim columnTag28 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag28.Caption = m_Translate.GetSafeTranslationValue("Tag28")
		'columnTag28.Name = "Tag28"
		'columnTag28.FieldName = "Tag28"
		'columnTag28.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag28.AppearanceHeader.Options.UseTextOptions = True
		'columnTag28.Visible = True
		'columnTag28.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag28.DisplayFormat.FormatString = "N2"
		'columnTag28.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag28.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag28.SummaryItem.Tag = "SumTag28"
		'gvTotalHoursGrouped.Columns.Add(columnTag28)

		'Dim columnTag29 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag29.Caption = m_Translate.GetSafeTranslationValue("Tag29")
		'columnTag29.Name = "Tag29"
		'columnTag29.FieldName = "Tag29"
		'columnTag29.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag29.AppearanceHeader.Options.UseTextOptions = True
		'columnTag29.Visible = True
		'columnTag29.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag29.DisplayFormat.FormatString = "N2"
		'columnTag29.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag29.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag29.SummaryItem.Tag = "SumTag29"
		'gvTotalHoursGrouped.Columns.Add(columnTag29)

		'Dim columnTag30 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag30.Caption = m_Translate.GetSafeTranslationValue("Tag30")
		'columnTag30.Name = "Tag30"
		'columnTag30.FieldName = "Tag30"
		'columnTag30.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag30.AppearanceHeader.Options.UseTextOptions = True
		'columnTag30.Visible = True
		'columnTag30.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag30.DisplayFormat.FormatString = "N2"
		'columnTag30.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag30.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag30.SummaryItem.Tag = "SumTag30"
		'gvTotalHoursGrouped.Columns.Add(columnTag30)

		'Dim columnTag31 As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTag31.Caption = m_Translate.GetSafeTranslationValue("Tag31")
		'columnTag31.Name = "Tag31"
		'columnTag31.FieldName = "Tag31"
		'columnTag31.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTag31.AppearanceHeader.Options.UseTextOptions = True
		'columnTag31.Visible = True
		'columnTag31.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTag31.DisplayFormat.FormatString = "N2"
		'columnTag31.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTag31.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTag31.SummaryItem.Tag = "SumTag31"
		'gvTotalHoursGrouped.Columns.Add(columnTag31)










		'Dim columnCalendarWeek As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnCalendarWeek.Caption = m_Translate.GetSafeTranslationValue("Woche")
		'columnCalendarWeek.Name = "CalendarWeek"
		'columnCalendarWeek.FieldName = "CalendarWeek"
		'columnCalendarWeek.Visible = True
		'columnCalendarWeek.OptionsColumn.AllowEdit = False
		'gvTotalHours.Columns.Add(columnCalendarWeek)

		'Dim columnDayName As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnDayName.Caption = " "
		'columnDayName.Name = "TranslatedDayOfWeekText"
		'columnDayName.FieldName = "TranslatedDayOfWeekText"
		'columnDayName.Visible = True
		'columnDayName.OptionsColumn.AllowEdit = False
		'gvTotalHours.Columns.Add(columnDayName)

		'Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnDate.Caption = m_Translate.GetSafeTranslationValue("Tag")
		'columnDate.Name = "DayDate"
		'columnDate.FieldName = "DayDate"
		'columnDate.Visible = True
		'columnDate.OptionsColumn.AllowEdit = False
		'gvTotalHours.Columns.Add(columnDate)

		'Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnTarif.Caption = m_Translate.GetSafeTranslationValue("Std")
		'columnTarif.Name = "WorkHours"
		'columnTarif.FieldName = "WorkHours"
		'columnTarif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnTarif.AppearanceHeader.Options.UseTextOptions = True
		'columnTarif.Visible = True
		'columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnTarif.DisplayFormat.FormatString = "N2"
		'columnTarif.SummaryItem.DisplayFormat = "{0:n2}"
		'columnTarif.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnTarif.SummaryItem.Tag = "SumWorkingHour"

		'gvTotalHours.Columns.Add(columnTarif)

		'Dim columnAbsenceCode As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnAbsenceCode.Caption = m_Translate.GetSafeTranslationValue("F")
		'columnAbsenceCode.Name = "FehlzeitCode"
		'columnAbsenceCode.FieldName = "FehlzeitCode"
		'columnAbsenceCode.Visible = True
		'columnAbsenceCode.OptionsColumn.AllowEdit = True
		'gvTotalHours.Columns.Add(columnAbsenceCode)

		Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.iHeight_Timetable = Me.Height
				My.Settings.iWidth_Timetable = Me.Width
				My.Settings.frmLocation_Timetable = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

				My.Settings.Save()
			End If

		End Sub

		''' <summary>
		''' Starten von Anwendung.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub Onfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

			Try
				If My.Settings.iHeight_Timetable > 0 Then Me.Height = My.Settings.iHeight_Timetable
				If My.Settings.iWidth_Timetable > 0 Then Me.Width = My.Settings.iWidth_Timetable
				If My.Settings.frmLocation_Timetable <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
			End Try

		End Sub


#End Region


		''' <summary>
		''' Loads the report data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadReportDataInternal(ByVal rpNr As Integer) As Boolean

			m_ReportMasterData = m_ReportDatabaseAccess.LoadRPMasterData(rpNr)

			If m_ReportMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Reportstammdaten konnten nicht geladen werden."))
				Return False
			End If
			m_CurrentEmployeeNumber = m_ReportMasterData.EmployeeNumber
			m_CurrentCustomerNumber = m_ReportMasterData.CustomerNumber

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, False)
			Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CurrentCustomerNumber, m_InitializationData.UserData.UserFiliale)

			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterstammdaten konnten nicht geladen werden."))
			End If

			If customerMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundenstammdaten konnten nicht geladen werden."))
			End If

			If Not m_ReportMasterData Is Nothing And
				Not employeeMasterData Is Nothing And
				Not customerMasterData Is Nothing Then


				Dim esData = m_ESDatabaseAccess.LoadESMasterData(m_ReportMasterData.ESNR)

				' Check if ES data can be loaded. Otherwhise the report is invalid an should be deleted.
				If (esData Is Nothing) Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Einsatzdaten des Rapports konnten nicht geladen werden. Der Rapport ist ungültig und wird gelöscht."))

					Return False
				End If

			End If

			If m_ReportMasterData.IsMonthClosed Then

				Dim monthClosedData = m_ReportDatabaseAccess.LoadMonthCloseData(m_ReportMasterData.MDNr, CType(m_ReportMasterData.Monat, Integer), m_ReportMasterData.Jahr)
				Dim userHowClosedTheMonth As String = String.Empty
				Dim monthClosedDate As DateTime? = Nothing

				If Not monthClosedData Is Nothing Then
					userHowClosedTheMonth = monthClosedData.UserName
					monthClosedDate = monthClosedData.CreatedOn
				End If

				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Sie können keine Daten ändern/löschen da der Monat bereits abgeschlossen ist.") & vbCrLf & vbCrLf &
																String.Format(m_Translate.GetSafeTranslationValue("Der Monat wurde am {0:dd.MM.yyyy} durch {1} abgeschlossen."), monthClosedDate, userHowClosedTheMonth) & vbCrLf &
															 m_Translate.GetSafeTranslationValue("Bitte kontaktieren sie Ihren Systemadministrator."), m_Translate.GetSafeTranslationValue("Daten ändern"), MessageBoxIcon.Information)
			End If

			Return True

		End Function


		Private Function CreateEmployeeHourData(ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As Boolean
			'Dim hoursRoundkind = GetHoursRoundKind
			Dim dfi As DateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo
			Dim cal As Calendar = dfi.Calendar

			Dim foundedData As ZVHourAbsenceData
			If m_RPLTypeToLoad = RPLType.Employee Then
				foundedData = LoadEmployeeMonthHourData(m_CurrentEmployeeNumber, dateFrom)
			Else
				foundedData = LoadCustomerMonthHourData(m_CurrentCustomerNumber, dateFrom)
			End If
			If foundedData Is Nothing Then Return False

			Dim listDataSource As BindingList(Of ZVHourAbsenceViewData) = New BindingList(Of ZVHourAbsenceViewData)

			For i As Integer = dateFrom.Day To dateTo.Day
				'	Dim cViewData = New TimeTableDayData(m_Translate, hoursRoundkind) With {
				'		.WorkingDay = New DateTime(dateFrom.Year, dateFrom.Month, i),
				'		.WorkingHour = foundedData.GetWorkingHoursOfDay(i),
				'		.AbsenceCode = foundedData.GetAbsenceDayCodeOfDay(i)
				'}
				Dim myDate As DateTime = New DateTime(dateFrom.Year, dateFrom.Month, i)
				Dim weekNumber = cal.GetWeekOfYear(myDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek)

				Dim dayHourData As ZVHourAbsenceViewData = New ZVHourAbsenceViewData With {
					.CalendarWeek = weekNumber,
					.WorkingDay = myDate,
					.WorkingHour = foundedData.GetWorkingHoursOfDay(i),
					.AbsenceCode = foundedData.GetAbsenceDayCodeOfDay(i)
				}

				listDataSource.Add(dayHourData)

			Next
			grdTotalHours.DataSource = listDataSource
			gvTotalHours.ExpandAllGroups()


			Return listDataSource Is Nothing

		End Function

		Private Function LoadEmployeeMonthHourData(ByVal emplyeeNumber As Integer, ByVal reportStartDate As DateTime) As ZVHourAbsenceData
			m_HourAbsencedata = m_EmployeeDatabaseAccess.GetEmployeeMonthHoursAndAbsenceData(m_InitializationData.MDData.MDNr, emplyeeNumber, CurrentReportNumber, reportStartDate.Year, reportStartDate.Month)

			If (m_HourAbsencedata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppierte Kandidaten Rapportstunden Daten konnten nicht geladen werden."))

				Return Nothing
			End If


			Return m_HourAbsencedata
		End Function

		Private Function LoadCustomerMonthHourData(ByVal customerNumber As Integer, ByVal reportStartDate As DateTime) As ZVHourAbsenceData
			m_HourAbsencedata = m_CustomerDatabaseAccess.GetCustomerMonthHoursAndAbsenceData(m_InitializationData.MDData.MDNr, customerNumber, CurrentReportNumber, reportStartDate.Year, reportStartDate.Month)

			If (m_HourAbsencedata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppierte Kunden Rapportstunden Daten konnten nicht geladen werden."))

				Return Nothing
			End If


			Return m_HourAbsencedata
		End Function

		Private Function CreateEmployeeHourGroupedByKSNrData(ByVal emplyeeNumber As Integer, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As Boolean
			Dim dfi As DateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo
			Dim cal As Calendar = dfi.Calendar

			Dim foundedData As List(Of WorkingHourGroupedWithKSTNrData)

			If m_RPLTypeToLoad = RPLType.Employee Then
				foundedData = LoadEmployeeMonthHourGroupedByKSTNrData(m_CurrentEmployeeNumber, dateFrom)
			Else
				foundedData = LoadCustomerMonthHourGroupedByKSTNrData(m_CurrentCustomerNumber, dateFrom)
			End If
			If foundedData Is Nothing Then Return False

			grdTotalHoursGrouped.DataSource = foundedData
			gvTotalHoursGrouped.ExpandAllGroups()


			Return foundedData Is Nothing

		End Function

		Private Function LoadEmployeeMonthHourGroupedByKSTNrData(ByVal emplyeeNumber As Integer, ByVal reportStartDate As DateTime) As List(Of WorkingHourGroupedWithKSTNrData)
			m_HourGroupedByKSTNrdata = m_EmployeeDatabaseAccess.GetEmployeeMonthHoursGroupedByKSTData(m_InitializationData.MDData.MDNr, emplyeeNumber, CurrentReportNumber, reportStartDate.Year, reportStartDate.Month)

			If (m_HourAbsencedata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppierte Kandidaten Rapportstunden Daten konnten nicht geladen werden."))

				Return Nothing
			End If


			Return m_HourGroupedByKSTNrdata
		End Function

		Private Function LoadCustomerMonthHourGroupedByKSTNrData(ByVal customerNumber As Integer, ByVal reportStartDate As DateTime) As List(Of WorkingHourGroupedWithKSTNrData)
			m_HourGroupedByKSTNrdata = m_CustomerDatabaseAccess.GetCustomerMonthHoursGroupedByKSTData(m_InitializationData.MDData.MDNr, customerNumber, CurrentReportNumber, reportStartDate.Year, reportStartDate.Month)

			If (m_HourAbsencedata Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gruppierte Kunden Rapportstunden Daten konnten nicht geladen werden."))

				Return Nothing
			End If


			Return m_HourGroupedByKSTNrdata
		End Function

		Private Sub OngvTotalHours_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvTotalHours.CustomColumnDisplayText

			If e.Column.FieldName = "WorkingHour" Then
				If Val(e.Value) = 0 Then e.DisplayText = String.Empty
			End If

		End Sub

		Private Sub OngvTotalHoursGrouped_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvTotalHoursGrouped.CustomColumnDisplayText

			If e.Column.FieldName.ToString.Contains("Tag") Then
				If Val(e.Value) = 0 Then e.DisplayText = String.Empty
			End If

		End Sub

		Private Sub OngvTotalHoursGrouped_CustomDrawRowFooterCell(ByVal sender As Object, ByVal e As FooterCellCustomDrawEventArgs) Handles gvTotalHoursGrouped.CustomDrawFooterCell
			If Convert.ToDecimal(e.Info.Value) = 0 Then
				e.Info.DisplayText = String.Empty
				e.Info.Visible = False
			End If
		End Sub

#End Region


#Region "Test modules"

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

			Dim exists As Boolean? = m_ReportDatabaseAccess.ExistsRPAbsenceDaysDataForRP(rpNr)

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
		''' <param name="type">The RPL type.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadRPLDayDataOfReport(ByVal rpNr As Integer, ByVal type As RPLType)

			' The data should exist -> load it.
			m_RPLDayDataList = m_ReportDatabaseAccess.LoadRPLDayData(rpNr, type)

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

			m_RPAbsenceDayData = m_ReportDatabaseAccess.LoadRPAbsenceDaysData(rpNr)

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

			Dim isReportFinished As Boolean = False
			Dim success As Boolean = m_ReportDatabaseAccess.LoadRPFinishedFlag(CurrentReportNumber, isReportFinished)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("'Erfasst' Spalte konnte nicht gelesen werden."))
			End If

			Return success
		End Function

		'''' <summary>
		'''' Handles row style event of salary data grid.
		'''' </summary>
		'Private Sub OnGvSalaryData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvTotalHours.RowStyle
		'	If (e.RowHandle >= 0) Then
		'		Dim view As GridView = CType(sender, GridView)
		'		Dim timeTableDayData = CType(view.GetRow(e.RowHandle), TimeTableDayData)

		'		If (Not m_RPLDataToHighlight Is Nothing AndAlso
		'			 timeTableDayData.DayDate >= m_RPLDataToHighlight.VonDate AndAlso
		'			 timeTableDayData.DayDate <= m_RPLDataToHighlight.BisDate) Then
		'			e.Appearance.ForeColor = Color.Red
		'		End If

		'	End If

		'End Sub

		'''' <summary>
		'''' Handles custom column display text event of gvTotalHours.
		'''' </summary>
		'Private Sub OnGvTotalHours_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles gvTotalHours.CustomColumnDisplayText

		'	If e.Column.FieldName = "WorkHours" Or e.Column.FieldName = "FehlzeitCode" Then

		'		Try
		'			Dim dataSourceIndex = e.ListSourceRowIndex

		'			If (dataSourceIndex >= 0) Then

		'				Dim view As GridView = CType(sender, GridView)
		'				'Dim timeTableDayData = CType(view.GetRow(e.GroupRowHandle), TimeTableDayData)
		'				Dim timeTableDayData = CType(view.GetRow(dataSourceIndex), TimeTableDayData)

		'				If e.Column.FieldName = "WorkHours" Then
		'					If (Not timeTableDayData.WorkHours.HasValue) Then
		'						e.DisplayText = "?"
		'					End If

		'				ElseIf e.Column.FieldName = "FehlzeitCode" Then
		'					If (timeTableDayData.WorkHours.HasValue AndAlso timeTableDayData.WorkHours.Value = 0) And
		'							String.IsNullOrEmpty(timeTableDayData.FehlzeitCode) Then
		'						e.DisplayText = "?"
		'					End If

		'				End If

		'			End If

		'		Catch ex As Exception
		'			m_Logger.LogError(ex.ToString())
		'		End Try

		'	End If
		'End Sub

		Private Function GetRowHandle(ByVal dataSourceIndex As Integer) As Integer
			Return dataSourceIndex
		End Function

#End Region




#Region "Helpers"

		Private Function IsFullExpanded(ByVal view As GridView) As Boolean
			If view.GroupCount = 0 Then
				Return True
			End If
			If view.RowCount = 0 Then
				Return True
			End If
			For i As Integer = -1 To Integer.MinValue + 1 Step -1
				If (Not view.IsValidRowHandle(i)) Then
					Return True
				End If
				If view.IsGroupRow(i) AndAlso (Not view.GetRowExpanded(i)) Then
					Return False
				End If
			Next i
			Return True
		End Function

		Private Function IsFullCollapsed(ByVal view As GridView) As Boolean
			If view.GroupCount = 0 Then
				Return False
			End If
			If view.RowCount = 0 Then
				Return False
			End If
			For i As Integer = -1 To Integer.MinValue + 1 Step -1
				If (Not view.IsValidRowHandle(i)) Then
					Return True
				End If
				If view.IsGroupRow(i) AndAlso view.GetRowExpanded(i) Then
					Return False
				End If
			Next i
			Return True
		End Function


#End Region



#Region "Helper class"

		Private Class ZVHourAbsenceViewData
			Public Property CalendarWeek As Integer?
			Public Property WorkingDay As Date?
			Public Property WorkingHour As Decimal?
			Public Property AbsenceCode As String
		End Class

#End Region



	End Class

End Namespace
