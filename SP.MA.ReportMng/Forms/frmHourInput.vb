Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.MA.ReportMng.Settings
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.DateAndTimeCalculation

Public Class frmHourInput

#Region "Private Fields"

  ''' <summary>
  ''' The Initialization data.
  ''' </summary>
  Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

  ''' <summary>
  ''' The report data access object.
  ''' </summary>
  Private m_ReportDatabaseAccess As IReportDatabaseAccess

  ''' <summary>
  ''' The common database access.
  ''' </summary>
  Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

  ''' <summary>
  ''' UI Utility functions.
  ''' </summary>
  Private m_UtilityUI As UtilityUI


  Private m_md As Mandant
  Private m_path As ClsProgPath

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger = New Logger()

  ''' <summary>
  ''' The rpl day data.
  ''' </summary>
  Private m_RPLDayData As RPLDayData

  ''' <summary>
  ''' The time table
  ''' </summary>
  Private m_TimeTable As TimeTable

  ''' <summary>
  ''' The absence data (Tab_Fehlzeit).
  ''' </summary>
  Private m_AbsenceTabData As List(Of AbsenceData)

  ''' <summary>
  ''' The RP absence day data (RP_Fehltage)
  ''' </summary>
  Private m_RPAbsenceDayData As RPAbsenceDaysData

  ''' <summary>
  ''' Helper member to calculate customer total on Studenspesen column.
  ''' </summary>
  Private m_TotalStundenSpesen As Decimal = 0


  ''' <summary>
  ''' Helper member to calculate customer total on Studen column.
  ''' </summary>
  Private m_TotalStunden As Decimal = 0

  ''' <summary>
  ''' The readonly flag.
  ''' </summary>
  Private m_IsReadonly As Boolean = False

	''' <summary>
	''' The readonly flag for Absencecode.
	''' </summary>
	Private m_IsAbsenceCodeReadonly As Boolean = False

  ''' <summary>
  ''' Supress UI event flag.
  ''' </summary>
  Private m_SuppressUIEvents As Boolean = False

  ''' <summary>
  ''' The settings manager.
  ''' </summary>
  Private m_SettingsManager As ISettingsManager

  ''' <summary>
  ''' Rundungsart für Arbeitszeit-Werte.
  ''' TODO: Abhängig vom Kunde
  ''' </summary>
  ''' <remarks></remarks>
  Private m_RoundKind As DateAndTimeUtily.HoursRoundKind = DateAndTimeUtily.HoursRoundKind.Minutes


#End Region	' Private Fields


#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  ''' <param name="_setting">The settings.</param>
  Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    m_TimeTable = New TimeTable()

    Try
      ' Mandantendaten
      m_md = New Mandant
      m_path = New ClsProgPath
      ' m_Common = New CommonSetting

      m_InitializationData = _setting
      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    End Try

    m_UtilityUI = New UtilityUI
    m_SettingsManager = New SettingsManager

    Dim connectionString As String = m_InitializationData.MDData.MDDbConn
    m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
    m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		TranslateControls()

  End Sub

#End Region ' Constructor

#Region "Public Properties"

  ''' <summary>
  ''' Get the time table.
  ''' </summary>
  ''' <returns>The time table.</returns>
  Public ReadOnly Property TimeTable As TimeTable
    Get
      Return m_TimeTable
    End Get
  End Property

  ''' <summary>
	''' Gets or sets the readonly flag.
  ''' </summary>
  Public Property IsReadonly As Boolean
    Get
      Return m_IsReadonly
    End Get

    Set(value As Boolean)
      m_IsReadonly = value

      chkAsNormalHours.Properties.ReadOnly = m_IsReadonly
      txtReplaceHoursThrough.Properties.ReadOnly = m_IsReadonly
      txtReplaceHoursEvenly.Properties.ReadOnly = m_IsReadonly
      lueAbsenceCodeDataReplaceTrough.Properties.ReadOnly = m_IsReadonly
      lueAbsenceCodeDataReplaceTrough.Properties.Buttons(1).Enabled = Not m_IsReadonly
      btnSave.Enabled = Not m_IsReadonly

      gvTimetable.OptionsBehavior.Editable = Not m_IsReadonly
    End Set
  End Property

	''' <summary>
	''' Gets or sets the readonly flag for lueAbsenceCodeDataReplaceTrough.
	''' </summary>
	Public Property IsReadonlyAbsenceCode As Boolean
		Get
			Return m_IsAbsenceCodeReadonly
		End Get

		Set(value As Boolean)
			m_IsAbsenceCodeReadonly = value

			lueAbsenceCodeDataReplaceTrough.Properties.ReadOnly = m_IsAbsenceCodeReadonly
			lueAbsenceCodeDataReplaceTrough.Properties.Buttons(1).Enabled = Not m_IsAbsenceCodeReadonly
			gvTimetable.Columns("FehlzeitCode").OptionsColumn.AllowEdit = Not m_IsAbsenceCodeReadonly

			'gvTimetable.OptionsBehavior.Editable = Not m_IsAbsenceCodeReadonly
		End Set
	End Property

  ''' <summary>
  ''' Gets or sets a boolean flag indicating if the show as normal hours checkbox is checked.
  ''' </summary>
  Public Property IsShowAsNormalHoursChecked
    Get
      Return chkAsNormalHours.Checked
    End Get
    Set(value)
      chkAsNormalHours.Checked = value
    End Set
  End Property

  ''' <summary>
  ''' Sets the maximal working hours per day info.
  ''' </summary>
  Public WriteOnly Property MaximalWorkingHoursPerDayInfo As Decimal

    Set(value As Decimal)

      lblMaximalWorkingHoursPerDay.Text = String.Format("{0}: {1:n2} h", m_Translate.GetSafeTranslationValue("Maximale Arbeitszeit pro Tag"), If(value = Decimal.MaxValue, "-", value))
      lblMaximalWorkingHoursPerDay.Visible = True

    End Set

  End Property

#End Region ' Public Properties

#Region "Private Properties"

  ''' <summary>
  ''' Gets boolean value indicating if hour replacment value is valid.
  ''' </summary>
  Private ReadOnly Property IsHoursReplacementValuesValid As Boolean
    Get

      Dim strValue = txtReplaceHoursThrough.Text

      If String.IsNullOrWhiteSpace(strValue) Then
        Return False
      End If

      Dim value As Decimal = 0D

      If Decimal.TryParse(strValue, value) Then

        If value < 0D Then
          Return False
        End If

        If value > 24D Then
          Return False
        End If

      Else
        Return False
      End If

      Return True
    End Get
  End Property

  ''' <summary>
  ''' Gets boolean value indicating if even hour replacment value is valid.
  ''' </summary>
  Private ReadOnly Property IsEvensHoursReplacmentValuesValid As Boolean
    Get

      Dim strValue = txtReplaceHoursEvenly.Text

      If String.IsNullOrWhiteSpace(strValue) Then
        Return False
      End If

      Dim value As Decimal = 0D

      If Decimal.TryParse(strValue, value) Then

        If value < 0D Then
          Return False
        End If

      Else
        Return False
      End If

      Return True
    End Get
  End Property

  ''' <summary>
  ''' Gets roundkind
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private ReadOnly Property GetHoursRoundKind As Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind
    Get

			Dim FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

			Dim value As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/hoursroundkind", FORM_XML_DEFAULTVALUES_KEY))

			Return If(String.IsNullOrWhiteSpace(value) OrElse Val(value) = 1, Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind.Minutes, Infrastructure.DateAndTimeCalculation.DateAndTimeUtily.HoursRoundKind.HundredthsOfHour)

    End Get
  End Property


#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Loads data of the form.
  ''' </summary>
  ''' <param name="rpNr">The rpNr.</param>
  ''' <param name="rplNr">The rplNr.</param>
  ''' <param name="type">The RPL type.</param>
  ''' <param name="dateFrom">The from date.</param>
  ''' <param name="dateTo">The to daye.</param>
  ''' <returns>Boolean flag indicating success.</returns>
  Public Function LoadData(ByVal rpNr As Integer, ByVal rplNr As Integer, ByVal type As RPLType, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As Boolean

    Dim success As Boolean = True

    Reset()

    CreateTimeTable(dateFrom, dateTo)

    success = success AndAlso LoadAbsenceCodeColumnData(rpNr)
    success = success AndAlso LoadWorkingHoursColumnData(rpNr, rplNr, type)

    gvTimetable.FocusedColumn = gvTimetable.VisibleColumns(3)
    gvTimetable.ShowEditor()

    Return success
  End Function

  ''' <summary>
  ''' Allows new input of hours.
  ''' </summary>
  ''' <param name="rpNr">The report number.</param>
  ''' <param name="dateFrom">The from date.</param>
  ''' <param name="dateto">The to date.</param>
  Public Function NewInput(ByVal rpNr As Integer, ByVal dateFrom As DateTime, ByVal dateTo As DateTime) As Boolean

    Reset()

    CreateTimeTable(dateFrom, dateTo)

    Dim success As Boolean = True

    success = success AndAlso LoadAbsenceCodeColumnData(rpNr)

    gvTimetable.FocusedColumn = gvTimetable.VisibleColumns(3)
    gvTimetable.ShowEditor()



    Return success
  End Function

#End Region ' Public Methods

#Region "Private Methods"

  ''' <summary>
  ''' load form
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub frmHourInput_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    Me.KeyPreview = True
  End Sub

  ''' <summary>
  ''' keyup methode for form
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub frmHourInput_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    Try
      If e.KeyCode = Keys.F5 OrElse (e.Control And e.KeyCode = Keys.S) Then
        btnSave.Focus()
        OnBtnSave_Click(sender, New System.EventArgs)
      End If

    Catch ex As Exception
      m_Logger.LogError(String.Format("KeyUp: {0}", ex.Message))
    End Try

  End Sub

  ''' <summary>
  ''' Loads form settings if form gets visible.
  ''' </summary>
  Private Sub OnFrm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

    If Visible Then
      LoadFormSettings()
    End If

  End Sub

  ''' <summary>
  ''' Handles form closing event.
  ''' </summary>
  Private Sub OnFrm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

    SaveFromSettings()

  End Sub

  ''' <summary>
  ''' Loads form settings.
  ''' </summary>
  Private Sub LoadFormSettings()

    Try
      Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_STD_HEIGHT)
      Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_STD_WIDTH)
      Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_STD_LOCATION)
			Dim setting_form_printdatamatrixcode = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_STD_PRINTDATAMATRIXCODE)

      If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
      If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			chkAutoPrinBarcode.Checked = setting_form_printdatamatrixcode

      If Not String.IsNullOrEmpty(setting_form_location) Then
        Dim aLoc As String() = setting_form_location.Split(CChar(";"))
        If Screen.AllScreens.Length = 1 Then
          If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
        End If
        Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())

    End Try

  End Sub

  ''' <summary>
  ''' Saves the form settings.
  ''' </summary>
  Private Sub SaveFromSettings()

    ' Save form location, width and height in setttings
    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_STD_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_STD_WIDTH, Me.Width)
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_STD_HEIGHT, Me.Height)

				m_SettingsManager.WriteBoolean(SettingKeys.SETTING_FORM_STD_PRINTDATAMATRIXCODE, chkAutoPrinBarcode.Checked)

        m_SettingsManager.SaveSettings()
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())

    End Try

  End Sub

#Region "Reset"

  ''' <summary>
  ''' Resets the form.
  ''' </summary>
  Private Sub Reset()

    Dim previousState = m_SuppressUIEvents
    m_SuppressUIEvents = True

    m_RPLDayData = Nothing
    m_TimeTable.Clear()
    m_TotalStundenSpesen = 0D

    chkAsNormalHours.Checked = False

    txtReplaceHoursThrough.Text = "0.0"
    txtReplaceHoursEvenly.Text = "0.0"
		lblMaximalWorkingHoursPerDay.Text = String.Empty

    '  Reset grids, drop downs and lists, etc.
    ReseAbsenceCodeReplacementValueDropDown()
    ResetTimeTableGrid()

    m_SuppressUIEvents = previousState

  End Sub

  ''' <summary>
  ''' Resets the absence code replacment value drop down.
  ''' </summary>
  Private Sub ReseAbsenceCodeReplacementValueDropDown()

    lueAbsenceCodeDataReplaceTrough.Properties.ReadOnly = False

    lueAbsenceCodeDataReplaceTrough.Properties.DisplayMember = "GetFeld"
    lueAbsenceCodeDataReplaceTrough.Properties.ValueMember = "GetFeld"

    Dim columns = lueAbsenceCodeDataReplaceTrough.Properties.Columns
    columns.Clear()

    columns.Add(New LookUpColumnInfo("GetFeld", 0))
    columns.Add(New LookUpColumnInfo("TranslatedDescription", 0))

    lueAbsenceCodeDataReplaceTrough.Properties.ShowHeader = False
    lueAbsenceCodeDataReplaceTrough.Properties.ShowFooter = False
    lueAbsenceCodeDataReplaceTrough.Properties.DropDownRows = 10
    lueAbsenceCodeDataReplaceTrough.Properties.BestFitMode = BestFitMode.BestFitResizePopup
    lueAbsenceCodeDataReplaceTrough.Properties.SearchMode = SearchMode.AutoComplete
    lueAbsenceCodeDataReplaceTrough.Properties.AutoSearchColumnIndex = 0

    lueAbsenceCodeDataReplaceTrough.Properties.NullText = String.Empty
    lueAbsenceCodeDataReplaceTrough.EditValue = Nothing
    lueAbsenceCodeDataReplaceTrough.Properties.DataSource = Nothing

  End Sub

  ''' <summary>
  ''' Resets the RPL flexible time grid.
  ''' </summary>
  Private Sub ResetTimeTableGrid()

    ' Reset the grid
    gvTimetable.OptionsView.ShowIndicator = False
    'gvTimetable.OptionsView.ColumnAutoWidth = False
    gvTimetable.OptionsBehavior.Editable = True
    gvTimetable.OptionsView.ShowFooter = True

    gvTimetable.Columns.Clear()
    'https://www1.devexpress.com/Support/Center/Question/Details/DQ19920
    gvTimetable.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click
    grdTimeTable.RepositoryItems.Clear()

    Dim columnCalendarWeek As New DevExpress.XtraGrid.Columns.GridColumn()
    columnCalendarWeek.Caption = m_Translate.GetSafeTranslationValue("Woche")
    columnCalendarWeek.Name = "CalendarWeek"
    columnCalendarWeek.FieldName = "CalendarWeek"
    columnCalendarWeek.Visible = True
    columnCalendarWeek.OptionsColumn.AllowEdit = False
    gvTimetable.Columns.Add(columnCalendarWeek)

    Dim columnDayName As New DevExpress.XtraGrid.Columns.GridColumn()
    columnDayName.Caption = " "
    columnDayName.Name = "TranslatedDayOfWeekText"
    columnDayName.FieldName = "TranslatedDayOfWeekText"
    columnDayName.Visible = True
    columnDayName.OptionsColumn.AllowEdit = False
    gvTimetable.Columns.Add(columnDayName)

    Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
    columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
    columnDate.Name = "DayDate"
    columnDate.FieldName = "DayDate"
    columnDate.Visible = True
    columnDate.OptionsColumn.AllowEdit = False
    gvTimetable.Columns.Add(columnDate)

    Dim textEditor As New RepositoryItemTextEdit()
    textEditor.AllowNullInput = DevExpress.Utils.DefaultBoolean.True
    textEditor.ValidateOnEnterKey = True
    textEditor.NullText = String.Empty
    textEditor.NullValuePromptShowForEmptyValue = False

    Dim columnHours As New DevExpress.XtraGrid.Columns.GridColumn()
    columnHours.Caption = m_Translate.GetSafeTranslationValue("Stunden")
    columnHours.Name = "WorkHours"
    columnHours.FieldName = "WorkHourEditText"
    columnHours.ColumnEdit = textEditor
    columnHours.AppearanceCell.Options.UseTextOptions = True
    columnHours.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnHours.AppearanceHeader.Options.UseTextOptions = True
    columnHours.Visible = True
    columnHours.OptionsColumn.AllowEdit = True
    columnHours.SummaryItem.DisplayFormat = "{0:n2}"
    columnHours.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom
    columnHours.SummaryItem.Tag = "SumWorkingHour"

    gvTimetable.Columns.Add(columnHours)

    'http://documentation.devexpress.com/#windowsforms/DevExpressXtraGridColumnsGridColumn_ColumnEdittopic

    ' --- Fehlzeiten ---
    If m_AbsenceTabData Is Nothing Then
      LoadAbsenceTabData()
    End If

    ' Create lookup edit to allow to select 'Absence' codes (Fehlzeiten)
    Dim fehlzeitenEdit As RepositoryItemLookUpEdit = New RepositoryItemLookUpEdit
    fehlzeitenEdit.DisplayMember = "GetFeld"
    fehlzeitenEdit.ValueMember = "GetFeld"

    Dim columns = fehlzeitenEdit.Columns
    columns.Clear()
    columns.Add(New LookUpColumnInfo("GetFeld", 0, String.Empty))
    columns.Add(New LookUpColumnInfo("TranslatedDescription", 0, String.Empty))
    fehlzeitenEdit.DataSource = m_AbsenceTabData
    grdTimeTable.RepositoryItems.Add(fehlzeitenEdit)

    Dim columnAbsenceCode As New DevExpress.XtraGrid.Columns.GridColumn()
    columnAbsenceCode.Caption = m_Translate.GetSafeTranslationValue("Fehltage")
    columnAbsenceCode.Name = "FehlzeitCode"
    columnAbsenceCode.FieldName = "FehlzeitCode"
    columnAbsenceCode.Visible = True
    columnAbsenceCode.OptionsColumn.AllowEdit = True
		columnAbsenceCode.ColumnEdit = fehlzeitenEdit
		gvTimetable.Columns.Add(columnAbsenceCode)

    ' --- end of Fehlzeiten ---

    'Dim columnTagespesen As New DevExpress.XtraGrid.Columns.GridColumn()
    'columnTagespesen.Caption = m_Translate.GetSafeTranslationValue("Tagesspesen")
    'columnTagespesen.Name = "Tagesspesen"
    'columnTagespesen.FieldName = "Tagesspesen"
    'columnTagespesen.Visible = True
    'columnTagespesen.OptionsColumn.AllowEdit = True
    'columnTagespesen.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
    'columnTagespesen.SummaryItem.DisplayFormat = "{0}"
    'gvTimetable.Columns.Add(columnTagespesen)

    'Dim columnStundenSpesen As New DevExpress.XtraGrid.Columns.GridColumn()
    'columnStundenSpesen.Caption = m_Translate.GetSafeTranslationValue("Stundenspesen")
    'columnStundenSpesen.Name = "Stundenspesen"
    'columnStundenSpesen.FieldName = "Stundenspesen"
    'columnStundenSpesen.Visible = True
    'columnStundenSpesen.OptionsColumn.AllowEdit = True
    'columnStundenSpesen.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom
    'columnStundenSpesen.SummaryItem.DisplayFormat = "{0:n2}"
    'columnStundenSpesen.SummaryItem.Tag = "SumStundenspesen"
    'gvTimetable.Columns.Add(columnStundenSpesen)

    grdTimeTable.DataSource = Nothing

  End Sub

#End Region ' Reset

#Region "Load Data"

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

      ' If the day is saturday or sunday set initial fehlzeitCode to 'S'
      Select Case day.DayOfWeek
        Case DayOfWeek.Saturday, DayOfWeek.Sunday
          fehlzeitCode = "S"
        Case Else
          fehlzeitCode = String.Empty
      End Select

      ' Create object for time table day entry.
			Dim dayHourData As TimeTableDayData = New TimeTableDayData(m_Translate, hoursRoundkind) With {
					.DayDate = day.Date,
					.WorkHours = 0D,
					.FehlzeitCode = fehlzeitCode,
					.Tagesspesen = False,
					.Stundenspesen = False
			}

      m_TimeTable.AddDayHourData(day, dayHourData)

      day = day.AddDays(1)
    End While

    grdTimeTable.DataSource = m_TimeTable.GetTimeTableData

  End Sub

  ''' <summary>
  ''' Loads working hours column data.
  ''' </summary>
  ''' <param name="rpNr">The report number.</param>
  ''' <param name="type">The rpl type.</param>
  ''' <param name="rplNr">The rplNr.</param>
  ''' <returns>Boolean falg indicating success.</returns>
  Private Function LoadWorkingHoursColumnData(ByVal rpNr As Integer, ByVal rplNr As Integer, ByVal type As RPLType) As Boolean

    Dim success As Boolean = True
    success = success AndAlso LoadRPLDayData(rpNr, rplNr, type)
    success = success AndAlso ApplyRPLDayDataToTimeTable()

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
  ''' <param name="rplNr">The rplNr.</param>
  ''' <param name="type">The RPL type.</param>
  ''' <returns>Boolean flag indicating success.</returns>
  Private Function LoadRPLDayData(ByVal rpNr As Integer, ByVal rplNr As Integer, ByVal type As RPLType)

    Dim success As Boolean = True

    ' First check if RPL day data exists.
    Dim existsRPLDayData As Boolean? = m_ReportDatabaseAccess.ExistsRPLDayDataForRPL(rpNr, rplNr, type)

    If existsRPLDayData Is Nothing Then
      ' Something went wrong with the check on existence.
      success = False
    Else
      If existsRPLDayData.Value Then

        ' The data should exist -> load it.
        Dim listOfRPLData = m_ReportDatabaseAccess.LoadRPLDayData(rpNr, type, rplNr)

        If listOfRPLData Is Nothing OrElse (Not listOfRPLData.Count = 1) Then

          ' The data could ne be loaded -> something went wrong.
          success = False
        Else
          m_RPLDayData = listOfRPLData(0)
        End If

      Else
        ' No data exists 
        success = False
      End If

    End If

    If Not success Then
      m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stundenplan-Daten konnten nicht aufgefüllt werden"))
    End If

    Return success
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
  ''' Loads absence tab data.
  ''' </summary>
  ''' <returns>Boolean flag indicating success.</returns>
  Private Function LoadAbsenceTabData() As Boolean

    m_AbsenceTabData = m_CommonDatabaseAccess.LoadAbsenceData()

    If m_AbsenceTabData Is Nothing Then
      m_UtilityUI.ShowErrorDialog("Fehlzeitenstammdaten konnten nicht geladen werden.")
      Return False
    Else
      m_AbsenceTabData.Insert(0, New AbsenceData() With {.GetFeld = String.Empty, .TranslatedDescription = String.Empty})

      lueAbsenceCodeDataReplaceTrough.Properties.DataSource = m_AbsenceTabData
      lueAbsenceCodeDataReplaceTrough.Properties.ForceInitialize()
    End If

    Return True

  End Function

  ''' <summary>
  ''' Applies RPL day data to the time table.
  ''' </summary>
  ''' <returns>Boolean flag indicating succcess.</returns>
  Private Function ApplyRPLDayDataToTimeTable() As Boolean

    If Not m_RPLDayData Is Nothing Then

      Dim workingHoursOfAllDaysInMonth = m_RPLDayData.GetWorkingHoursOfAllDaysInMonth()

      m_TimeTable.SetWorkinHoursOfManyDays(workingHoursOfAllDaysInMonth.ToArray())

    End If

    grdTimeTable.RefreshDataSource()

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

    grdTimeTable.RefreshDataSource()

    Return True
  End Function

#End Region ' Load Data

#Region "Event Handlers"

  ''' <summary>
  ''' Handles check changed event of chkAsNormalHours.
  ''' </summary>
  Private Sub OnChkAsNormalHours_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAsNormalHours.CheckedChanged

    If m_SuppressUIEvents Then
      Return
    End If

    m_TimeTable.SetWorkTimeFormat(showMinutes:=chkAsNormalHours.Checked)

    grdTimeTable.RefreshDataSource()

  End Sub

  ''' <summary>
  ''' Handles button click on txtReplaceHoursThrough.
  ''' </summary>
  Private Sub OnTxtReplaceHoursThrough_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtReplaceHoursThrough.ButtonClick

    If e.Button.Index = 0 AndAlso IsHoursReplacementValuesValid Then

      Dim hourValue As Decimal = Convert.ToDecimal(txtReplaceHoursThrough.Text)
      m_TimeTable.ReplaceEmptyHours(hourValue)

    End If

    grdTimeTable.RefreshDataSource()
  End Sub

  ''' <summary>
  ''' Handles button click on txtReplaceHoursEvenly.
  ''' </summary>
  Private Sub OnTxtReplaceHoursEvenly_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtReplaceHoursEvenly.ButtonClick

    If e.Button.Index = 0 AndAlso IsEvensHoursReplacmentValuesValid Then

      Dim totalHourValue As Decimal = Convert.ToDecimal(txtReplaceHoursEvenly.Text)
      m_TimeTable.ReplaceEmptyHoursEvenly(totalHourValue)

    End If

    grdTimeTable.RefreshDataSource()
  End Sub

  ''' <summary>
  ''' Handles button click on txtReplaceAbsenceCodesThrough.
  ''' </summary>
  Private Sub OnTxtReplaceAbsenceCodesThrough_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueAbsenceCodeDataReplaceTrough.ButtonClick

    If e.Button.Index = 1 Then

      Dim absenceCodeValue As String = lueAbsenceCodeDataReplaceTrough.Text
      m_TimeTable.ReplaceEmptyAbsenceCodes(absenceCodeValue)

    End If

    grdTimeTable.RefreshDataSource()
  End Sub

  ''' <summary>
  ''' Handles validating event of txtReplaceHoursThrough.
  ''' </summary>
  Private Sub OnTxtReplaceHoursThrough_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtReplaceHoursThrough.Validating

    If Not IsHoursReplacementValuesValid Then
      e.Cancel = True
    End If

  End Sub

  ''' <summary>
  ''' Handles Validating event of txtReplaceHoursEvenly.
  ''' </summary>
  Private Sub OnTxtReplaceHoursEvenly_Validating(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles txtReplaceHoursEvenly.Validating

    If Not IsEvensHoursReplacmentValuesValid Then
      e.Cancel = True
    End If

  End Sub

  ''' <summary>
  ''' Handles process grid key event on grdTimeTable.
  ''' </summary>
  Private Sub OnGrdTimeTable_ProcessGridKey(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdTimeTable.ProcessGridKey
    If e.KeyCode = Keys.Enter Then
      Try
        Me.MoveFocus()
        '  Dim cv = CType(grdTimeTable.FocusedView, ColumnView)
        '  cv.FocusedRowHandle = cv.FocusedRowHandle + 1
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try
    End If
  End Sub

  Private Sub MoveFocus()
    If Me.InvokeRequired Then
      Me.Invoke(Sub() MoveFocus())
      Return
    End If
    gvTimetable.MoveNext()
  End Sub

  ''' <summary>
  ''' Handles row cell style event of gvTimetable.
  ''' </summary>
  Private Sub OnGvTimetable_CellStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvTimetable.RowCellStyle

    If e.Column.FieldName = "TranslatedDayOfWeekText" Or e.Column.FieldName = "DayDate" Then

      If (e.RowHandle >= 0) Then

        Dim view As GridView = CType(sender, GridView)
        Dim dayHourData = CType(view.GetRow(e.RowHandle), TimeTableDayData)

        ' Set foreground color of saturday and sunday to red all other days to blue.
        Select Case dayHourData.DayDate.DayOfWeek
          Case DayOfWeek.Saturday, DayOfWeek.Sunday
            e.Appearance.ForeColor = Color.Red
          Case Else
            e.Appearance.ForeColor = Color.Blue
        End Select
      End If

    End If
  End Sub

  ''' <summary>
  ''' Handles cell change event on gvTimetable.
  ''' </summary>
  Private Sub OnGvTimetable_CellValueChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gvTimetable.CellValueChanged

    If e.Column.SummaryItem.SummaryType <> DevExpress.Data.SummaryItemType.None Then
      ' Update the summaries immediately after cell edit has been finsihed.
      gvTimetable.UpdateSummary()
    End If

  End Sub

  ''' <summary>
  ''' Handles customer summary calculate event of gvTimetable.
  ''' </summary>
  Private Sub OnGvTimetable_CustomSummaryCalculate(sender As System.Object, e As DevExpress.Data.CustomSummaryEventArgs) Handles gvTimetable.CustomSummaryCalculate

    Dim item = CType(e.Item, GridColumnSummaryItem)
    Dim view As GridView = CType(sender, GridView)

    '' Calulate the sum of the Stundenspesen
    'If String.Equals(item.Tag, "SumStundenspesen") Then

    '  If (e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Start) Then
    '    m_TotalStundenSpesen = 0
    '  ElseIf (e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Calculate) Then

    '    ' Check if current cell 'Stundenspesen' is checked.
    '    If Convert.ToBoolean(view.GetRowCellValue(e.RowHandle, "Stundenspesen")) Then
    '      ' The 'Stundenspesen' cell is checked so the work hours can be added to the total sum.
    '      ' TODO set work hours
    '      Dim workHours As Decimal? = view.GetRowCellValue(e.RowHandle, "WorkHours")
    '      m_TotalStundenSpesen = m_TotalStundenSpesen + workHours
    '    End If
    '  ElseIf (e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Finalize) Then
    '    ' Apply total sum to total.

    '    e.TotalValue = m_TotalStundenSpesen

    '  End If

    'End If

    If String.Equals(item.Tag, "SumWorkingHour") Then

      If (e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Start) Then
        m_TotalStunden = 0
      ElseIf (e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Calculate) Then

        ' Check if current cell 'Stundenspesen' is checked.
        ' The 'Stundenspesen' cell is checked so the work hours can be added to the total sum.
        'Dim workHours As Decimal? = view.GetRowCellValue(e.RowHandle, "WorkHours")
        'm_TotalStunden = m_TotalStunden + workHours

      ElseIf (e.SummaryProcess = DevExpress.Data.CustomSummaryProcess.Finalize) Then

        e.TotalValue = m_TimeTable.SumHourData()

      End If

    End If

  End Sub

  Private Sub OngvTimetable_ShownEditor(sender As Object, e As EventArgs) Handles gvTimetable.ShownEditor
    Dim edit As DevExpress.XtraEditors.TextEdit = gvTimetable.ActiveEditor
    If Not edit Is Nothing Then
      AddHandler edit.Spin, AddressOf OnGVedit_Spin
    End If

  End Sub

  ''' <summary>
  ''' for mousewheel
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub OnGVedit_Spin(sender As Object, e As SpinEventArgs)
    e.Handled = True
    gvTimetable.TopRowIndex += e.IsSpinUp

  End Sub

	''' <summary>
	''' Handles click on save button
	''' </summary>
	Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
		DialogResult = DialogResult.OK
		Close()
	End Sub

	Private Sub OntxtReplaceHoursThrough_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtReplaceHoursThrough.KeyPress

    If e.KeyChar = ChrW(Keys.Return) AndAlso IsHoursReplacementValuesValid Then

      Dim hourValue As Decimal = Convert.ToDecimal(txtReplaceHoursThrough.Text)
      m_TimeTable.ReplaceEmptyHours(hourValue)

    End If

    grdTimeTable.RefreshDataSource()

  End Sub

  Private Sub OntxtReplaceHoursEvenly_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtReplaceHoursEvenly.KeyPress

    If e.KeyChar = ChrW(Keys.Return) AndAlso IsEvensHoursReplacmentValuesValid Then

      Dim totalHourValue As Decimal = Convert.ToDecimal(txtReplaceHoursEvenly.Text)
      m_TimeTable.ReplaceEmptyHoursEvenly(totalHourValue)

    End If

    grdTimeTable.RefreshDataSource()

  End Sub

#End Region ' Event Handlers

#Region "Helper Methods"

  ''' <summary>
  '''  Translate controls.
  ''' </summary>
  Private Sub TranslateControls()
    Me.chkAsNormalHours.Text = m_Translate.GetSafeTranslationValue(Me.chkAsNormalHours.Text)
    Me.lblReplaceHours.Text = m_Translate.GetSafeTranslationValue(Me.lblReplaceHours.Text)
    Me.lblReplaceHourSumEvenly.Text = m_Translate.GetSafeTranslationValue(Me.lblReplaceHourSumEvenly.Text)
    Me.lblReplaceAbsenceCodes.Text = m_Translate.GetSafeTranslationValue(Me.lblReplaceAbsenceCodes.Text)
    Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
  End Sub

#End Region ' Helper Methods

#End Region ' Private Methods

End Class