Imports SP.TodoMng

Imports System.Reflection.Assembly
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel
Imports SP.MA.ReportMng.Settings
Imports System.Windows.Forms

Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.SPUserSec.ClsUserSec
'Imports SPS.Listing.Print.Utility
Imports System.Threading
Imports System.IO
Imports DevExpress.XtraNavBar
Imports SP.DatabaseAccess.Report
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SPS.ES.Utility
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.ES
Imports SPProgUtility.CommonXmlUtility
Imports SP.MA.ReportMng.SPUpdateUtilitiesService
Imports SP.Infrastructure.DateAndTimeCalculation
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraSplashScreen


Namespace UI


	''' <summary>
	''' Report management.
	''' </summary>
	Public Class frmNewReport


#Region "Constants"

		Private Const DEFAULT_SPUTNIK_UPDATE_UTILITIES_WEBSERVICE_URI = "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
		Private Const MANDANT_XML_SETTING_SPUTNIK_UTILITIES_WEBSERVICE_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceupdateinfoservices"

#End Region

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
		''' The ES database access.
		''' </summary>
		Private m_ESDatabaseAccess As IESDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As DatabaseAccess.Common.ICommonDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' Contains the report number of the loaded report data.
		''' </summary>
		Private m_ReportNumber As Integer?

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' Date and time utitlity.
		''' </summary>
		Private m_DateAndTimeUtility As New DateAndTimeUtily

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_ListOfUserControls As New List(Of ucBaseControl)

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = True

		''' <summary>
		''' Communication support between controls.
		''' </summary>
		Protected m_UCMediator As UserControlFormMediator

		''' <summary>
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_md As Mandant
		Private m_path As ClsProgPath

		Private Property m_PrintJobNr As String
		Private Property m_SQL4Print As String
		Private Property m_bPrintAsDesign As Boolean

		Private m_SaveButton As NavBarItem
		Private m_DeleteButton As NavBarItem

		Private m_IsDataValid As Boolean = True

		''' <summary>
		''' Boolan flag indicating if the form has been initialized.
		''' </summary>
		Private m_IsInitialized = False

		''' <summary>
		''' Check edit for active symbol.
		''' </summary>
		Private m_CheckEditActive As RepositoryItemCheckEdit

		''' <summary>
		''' ES overview data.
		''' </summary>
		Private m_ESOverviewData As IEnumerable(Of SP.DatabaseAccess.Report.DataObjects.ESData)

		''' <summary>
		''' Active report data.
		''' </summary>
		Private m_ActiveReportData As ActiveReportData

		''' <summary>
		''' The active botoom tab page.
		''' </summary>
		Private m_ActiveBottomTabPage As ucBaseControl

		''' <summary>
		''' The create RP service.
		''' </summary>
		Private m_CreateRPService As CreateRPService

		''' <summary>
		''' Boolean flag indicating that the report data has been reloaded due to changed ES.
		''' </summary>
		Private m_ReportHasBeenReloadedDueToChangedES = False

		Private vb6Object As Object = Nothing

		Private Print_P_Data As NavBarItem

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' SPUpdateUtilities service url.
		''' </summary>
		Private m_SPUpdateUtilitiesServiceUrl As String

		''' <summary>
		''' The GAV update info of last check.
		''' </summary>
		Private m_GavUpdateInfoOfLastCheck As GAVVersionDataDTO

		''' <summary>
		''' Error icon.
		''' </summary>
		Private m_ErrorIcon As Image

		Private m_Years As List(Of IntegerValueViewWrapper)
		Private m_Month As List(Of IntegerValueViewWrapper)

		Private m_SearchMandantNumber As Integer
		Private m_SearchYear As Integer
		Private m_SearchMonth As Integer
		Private m_createdReportNumber As Integer()

#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try

				m_ErrorIcon = My.Resources.error_small

				' Mandantendaten
				m_md = New Mandant
				m_path = New ClsProgPath

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

				m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				m_SPUpdateUtilitiesServiceUrl = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_UTILITIES_WEBSERVICE_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

				If String.IsNullOrWhiteSpace(m_SPUpdateUtilitiesServiceUrl) Then
					m_SPUpdateUtilitiesServiceUrl = DEFAULT_SPUTNIK_UPDATE_UTILITIES_WEBSERVICE_URI
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			Dim previousState = SetSuppressUIEventsState(True)
			InitializeComponent()
			SetSuppressUIEventsState(previousState)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility
			m_CreateRPService = New CreateRPService(m_InitializationData.MDData.MDNr, m_InitializationData)

			' Translate controls
			TranslateControls()
			Reset()

		End Sub

#End Region



#Region "Public methodes"

		''' <summary>
		''' Loads the payroll data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadPayrollData() As Boolean

			Dim success As Boolean = True

			success = success And LoadMandantDropDownData()
			PreselectsMandantYearAndMonth()

			Return success

		End Function

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFromSettings()

			Me.Hide()
			Me.Reset() 'Clear all data.
		End Sub

		''' <summary>
		''' Sets the suppress UI events state.
		''' </summary>
		''' <param name="shouldEventsBeSuppressed">Boolean flag indicating the  UI events should be suppressed.</param>
		''' <returns>Previous state of suppress events.</returns>
		Public Function SetSuppressUIEventsState(ByVal shouldEventsBeSuppressed As Boolean)

			Dim orginalState = m_SuppressUIEvents
			m_SuppressUIEvents = shouldEventsBeSuppressed

			Return orginalState

		End Function

		''' <summary>
		''' Gets the selected report overview data.
		''' </summary>
		''' <returns>The selected report overview data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedReportOverviewData As RPOverviewData
			Get
				Dim grdView = TryCast(grdReportOverview.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim rpOverviewData = CType(grdView.GetRow(selectedRows(0)), RPOverviewData)
						Return rpOverviewData
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region


		''' <summary>
		''' Selects a year.
		''' </summary>
		''' <param name="year">The year to select.</param>
		Private Sub SelectYear(ByVal year As Integer)

			If m_Years Is Nothing Then
				m_Years = New List(Of IntegerValueViewWrapper)
				lueYear.Properties.DataSource = m_Years
				lueYear.Properties.ForceInitialize()
			End If

			If Not m_Years.Any(Function(data) data.Value = year) Then
				m_Years.Add(New IntegerValueViewWrapper With {.Value = year})
			End If

			lueYear.EditValue = year

		End Sub

		''' <summary>
		''' Selects a month.
		''' </summary>
		''' <param name="month">The month to select.</param>
		Private Sub SelectMonth(ByVal month As Integer)

			If m_Month Is Nothing Then
				m_Month = New List(Of IntegerValueViewWrapper)
				lueMonth.Properties.DataSource = m_Month
				lueMonth.Properties.ForceInitialize()
			End If

			If Not m_Month.Any(Function(data) data.Value = month) Then
				m_Month.Add(New IntegerValueViewWrapper With {.Value = month})
			End If

			lueMonth.EditValue = month

		End Sub

		''' <summary>
		''' Preselects mandant, year and month.
		''' </summary>
		Private Sub PreselectsMandantYearAndMonth()

			lueMandant.EditValue = m_InitializationData.MDData.MDNr

			PreselectYearAndMonth()

		End Sub

		''' <summary>
		''' Preslects year and month.
		''' </summary>
		Private Sub PreselectYearAndMonth()

			Dim isDateBefore10thDay = (DateTime.Now.Day <= 10)
			Dim today = DateTime.Now.Date
			Dim yearMonthToSelect As DateTime = New DateTime(today.Year, today.Month, 1)

			If isDateBefore10thDay Then
				' Take previous month if date is before 10th.
				yearMonthToSelect = yearMonthToSelect.AddMonths(-1)
			End If

			SelectYear(yearMonthToSelect.Year)
			SelectMonth(yearMonthToSelect.Month)

		End Sub

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_NEW_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_NEW_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_NEW_LOCATION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

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
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_NEW_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_NEW_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_NEW_HEIGHT, Me.Height)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub



		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
			Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)

			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
			Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
			Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
			Me.btnSearch.Text = m_Translate.GetSafeTranslationValue(Me.btnSearch.Text)


			Me.bsiLblRPCount.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRPCount.Caption)

		End Sub

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			Dim previousState = SetSuppressUIEventsState(True)

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_ESDatabaseAccess = New DatabaseAccess.ES.ESDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_Years = Nothing
			m_Month = Nothing

			m_SearchMandantNumber = 0
			m_SearchYear = 0
			m_SearchMonth = 0

			m_ESOverviewData = Nothing
			m_ActiveReportData = Nothing
			m_ReportNumber = Nothing
			m_ReportHasBeenReloadedDueToChangedES = False

			ResetMandantDropDown()
			ResetYearDropDown()
			ResetMonthDropDown()

			'  Reset grids, drop downs and lists, etc.
			ResetReportOverviewGrid()

			SetSuppressUIEventsState(previousState)

			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets the year drop down.
		''' </summary>
		Private Sub ResetYearDropDown()

			lueYear.Properties.DisplayMember = "Value"
			lueYear.Properties.ValueMember = "Value"
			lueYear.Properties.ShowHeader = False

			lueYear.Properties.Columns.Clear()
			lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

			lueYear.Properties.ShowFooter = False
			lueYear.Properties.DropDownRows = 10
			lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueYear.Properties.SearchMode = SearchMode.AutoComplete
			lueYear.Properties.AutoSearchColumnIndex = 0

			lueYear.Properties.NullText = String.Empty
			lueYear.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the month drop down.
		''' </summary>
		Private Sub ResetMonthDropDown()

			lueMonth.Properties.DisplayMember = "Value"
			lueMonth.Properties.ValueMember = "Value"
			lueMonth.Properties.ShowHeader = False

			lueMonth.Properties.Columns.Clear()
			lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

			lueMonth.Properties.ShowFooter = False
			lueMonth.Properties.DropDownRows = 10
			lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMonth.Properties.SearchMode = SearchMode.AutoComplete
			lueMonth.Properties.AutoSearchColumnIndex = 0

			lueMonth.Properties.NullText = String.Empty
			lueMonth.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the Mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"

			lueMandant.Properties.Columns.Clear()
			lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

			lueMandant.Properties.ShowFooter = False
			lueMandant.Properties.DropDownRows = 10
			lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMandant.Properties.SearchMode = SearchMode.AutoComplete
			lueMandant.Properties.AutoSearchColumnIndex = 0

			lueMandant.Properties.NullText = String.Empty
			lueMandant.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the report overview grid.
		''' </summary>
		Private Sub ResetReportOverviewGrid()

			' Reset the grid
			gvReportOverview.OptionsView.ShowIndicator = False
			gvReportOverview.OptionsView.ColumnAutoWidth = True
			gvReportOverview.OptionsView.ShowAutoFilterRow = True

			gvReportOverview.Columns.Clear()

			Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPNr.Caption = m_Translate.GetSafeTranslationValue("RPNr")
			columnRPNr.Name = "RPNr"
			columnRPNr.FieldName = "RPNr"
			columnRPNr.Visible = True
			columnRPNr.Width = 50
			gvReportOverview.Columns.Add(columnRPNr)

			Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESNr.Caption = m_Translate.GetSafeTranslationValue("ESNr")
			columnESNr.Name = "ESNr"
			columnESNr.FieldName = "ESNr"
			columnESNr.Visible = False
			gvReportOverview.Columns.Add(columnESNr)

			Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullName.Name = "EmployeeFullName"
			columnEmployeeFullName.FieldName = "EmployeeFullName"
			columnEmployeeFullName.Visible = True
			gvReportOverview.Columns.Add(columnEmployeeFullName)

			Dim columnCustomer1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCustomer1.Name = "Customer1"
			columnCustomer1.FieldName = "Customer1"
			columnCustomer1.Visible = True
			gvReportOverview.Columns.Add(columnCustomer1)

			Dim columnRPFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnRPFrom.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnRPFrom.Name = "ReportFrom"
			columnRPFrom.FieldName = "ReportFrom"
			columnRPFrom.Visible = False
			gvReportOverview.Columns.Add(columnRPFrom)

			Dim columnRPTo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnRPTo.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnRPTo.Name = "ReportTo"
			columnRPTo.FieldName = "ReportTo"
			columnRPTo.Visible = False
			gvReportOverview.Columns.Add(columnRPTo)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
			columnMonth.Name = "ReportMonth"
			columnMonth.FieldName = "ReportMonth"
			columnMonth.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnMonth.AppearanceHeader.Options.UseTextOptions = True
			columnMonth.Visible = True
			columnMonth.Width = 30
			gvReportOverview.Columns.Add(columnMonth)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnYear.Name = "ReportYear"
			columnYear.FieldName = "ReportYear"
			columnYear.Visible = True
			columnYear.Width = 40
			gvReportOverview.Columns.Add(columnYear)

			grdReportOverview.DataSource = Nothing

		End Sub



		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.EditValue = Nothing
			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantYearDropDownData() As Boolean

			Dim success As Boolean = True

			Dim mandantNumber = lueMandant.EditValue

			Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

			If Not mandantNumber Is Nothing Then

				Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(mandantNumber)

				If (yearData Is Nothing) Then
					success = False
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
				End If

				If Not yearData Is Nothing Then
					wrappedValues = New List(Of IntegerValueViewWrapper)

					For Each yearValue In yearData
						wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
					Next

				End If

			End If

			m_Years = wrappedValues

			lueYear.EditValue = Nothing
			lueYear.Properties.DataSource = m_Years
			lueYear.Properties.ForceInitialize()

			Return success
		End Function

		''' <summary>
		''' Loads the month drop down data.
		''' </summary>
		Private Function LoadMonthDropDownData() As Boolean

			Dim success As Boolean = True

			Dim mandantNumber = lueMandant.EditValue
			Dim year = lueYear.EditValue

			Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

			If Not mandantNumber Is Nothing And
				 Not year Is Nothing Then

				Dim closedMonth = m_CommonDatabaseAccess.LoadClosedMonthOfYear(year, mandantNumber)

				If (closedMonth Is Nothing) Then
					success = False
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verrechnete Monate konnten nicht geladen werden."))
				End If

				If Not closedMonth Is Nothing Then
					wrappedValues = New List(Of IntegerValueViewWrapper)

					For i As Integer = 1 To 12

						If Not closedMonth.Contains(i) Then
							wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
						End If

					Next

				End If

			End If

			m_Month = wrappedValues
			lueMonth.EditValue = Nothing
			lueMonth.Properties.DataSource = m_Month
			lueMonth.Properties.ForceInitialize()

			Return success
		End Function

		''' <summary>
		''' Loads report overview data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadESOverviewData() As Boolean

			m_ESOverviewData = m_ReportDatabaseAccess.LoadESDataForCreateingReport(lueMandant.EditValue, lueYear.EditValue, lueMonth.EditValue)

			If m_ESOverviewData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Daten konnten nicht geladen werden."))
			End If

			Return Not m_ESOverviewData Is Nothing

		End Function

		''' <summary>
		''' Create new report for es data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function CreateNewReport() As Boolean

			m_createdReportNumber = Nothing
			If m_ESOverviewData Is Nothing Then
				Return False
			ElseIf m_ESOverviewData.Count = 0 Then
				Return True
			End If

			Dim RPNumberOffset = ReadRPOffsetFromSettings()
			Dim firstDayOfMonth = New DateTime(lueYear.EditValue, lueMonth.EditValue, 1)
			Dim lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1)
			Dim success As Boolean = True
			Dim reportNumbers As List(Of Integer) = Nothing

			reportNumbers = New List(Of Integer)

			For Each rec In m_ESOverviewData
				Dim newRPVon = If(rec.esAb.HasValue AndAlso rec.esAb.Value.Date > firstDayOfMonth, rec.esAb.Value.Date, firstDayOfMonth)
				Dim newRPBis = If(rec.esEnde.HasValue AndAlso rec.esEnde.Value.Date < lastDayOfMonth, rec.esEnde.Value.Date, lastDayOfMonth)

				Dim iniData As New NewRPForExistingESData

				iniData.ESNr = rec.ESNr
				iniData.RPMonth = lueMonth.EditValue
				iniData.RPYear = lueYear.EditValue
				iniData.RPVon = newRPVon
				iniData.RPBis = newRPBis
				iniData.RPSuva = rec.suva
				iniData.RPKst = rec.eskst
				iniData.RPKst1 = rec.eskst1
				iniData.RPKst2 = rec.eskst2
				iniData.RPKDBranche = rec.esbranche
				iniData.MDNr = lueMandant.EditValue
				iniData.RPNumberOffset = RPNumberOffset
				iniData.CreatedFrom = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_ReportDatabaseAccess.AddNewRPForExistingES(iniData)
				If success Then
					reportNumbers.Add(iniData.NewRPNrOutput)
				End If

			Next
			m_createdReportNumber = reportNumbers.ToArray

			Return success

		End Function

		''' <summary>
		''' Loads report overview data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadReportOverviewData() As Boolean

			If m_createdReportNumber Is Nothing OrElse m_createdReportNumber.Length = 0 Then Return False
			Dim m_ReportOverviewData = m_ReportDatabaseAccess.LoadCreatedRPOverviewListData(lueMandant.EditValue, m_createdReportNumber)

			If m_ReportOverviewData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportübersicht konnte nicht geladen werden."))
			End If

			Dim previousState = SetSuppressUIEventsState(True)
			grdReportOverview.DataSource = m_ReportOverviewData
			SetSuppressUIEventsState(previousState)

			Return Not m_ReportOverviewData Is Nothing

		End Function


		Private Sub OnbtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			CleanupAndHideForm()
		End Sub

		Private Sub OnbtnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

			Dim success As Boolean = True
			Try
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
				SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
				SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

				grdReportOverview.DataSource = Nothing

				success = success AndAlso LoadESOverviewData()

				success = success AndAlso CreateNewReport()

			Catch ex As Exception

			Finally
				SplashScreenManager.CloseForm(False)

			End Try

			If success Then
				If m_createdReportNumber Is Nothing OrElse m_createdReportNumber.Length = 0 Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Keine Einsatzdaten wurden gefunden."))
				Else
					success = success AndAlso LoadReportOverviewData()
				End If

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapporte konnten nicht erfolgreich erstellt werden."))
			End If
			bsiChanged.Caption = gvReportOverview.RowCount


		End Sub

		''' <summary>
		''' Handles edit change event of lueMandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ResetReportOverviewGrid()

			If Not lueMandant.EditValue Is Nothing Then

				If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
					Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

					Dim clsMandant = m_md.GetSelectedMDData(lueMandant.EditValue)
					Dim logedUserData = m_md.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
					Dim personalizedData = m_InitializationData.ProsonalizedData
					Dim translate = m_InitializationData.TranslationData

					m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)
				End If

				Dim conStr = m_md.GetSelectedMDData(lueMandant.EditValue).MDDbConn
				Dim connectionString As String = m_InitializationData.MDData.MDDbConn
				m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
				m_ESDatabaseAccess = New DatabaseAccess.ES.ESDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
				m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
				m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
				m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			End If

			LoadMandantYearDropDownData()

			PreselectYearAndMonth()

		End Sub

		''' <summary>
		''' Handles edit change event of lueYear.
		''' </summary>
		Private Sub OnLueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ResetReportOverviewGrid()

			LoadMonthDropDownData()

		End Sub

		''' <summary>
		''' Handles edit change event of lueMonth.
		''' </summary>
		Private Sub OnLueMonth_EditValueChanged(sender As Object, e As EventArgs) Handles lueMonth.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ResetReportOverviewGrid()

		End Sub


		Private Sub OngvReportOverview_RowCellClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvReportOverview.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvReportOverview.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then

					Dim viewData = CType(dataRow, RPOverviewData)
					If Not viewData Is Nothing Then
						Select Case column.Name.ToLower

							Case "manr".ToLower, "EmployeeFirstname".ToLower, "EmployeeFullName".ToLower, "EmployeeLastname".ToLower
								If viewData.MANr > 0 Then
									Dim hub = MessageService.Instance.Hub
									Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, viewData.MANr)
									hub.Publish(openEmployeeMng)
								End If

							Case "kdnr", "Customer1".ToLower
								If viewData.KDNr > 0 Then
									Dim hub = MessageService.Instance.Hub
									Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, viewData.KDNr)
									hub.Publish(openCustomerMng)
								End If

							Case "esnr"
								If viewData.ESNr > 0 Then
									Dim hub = MessageService.Instance.Hub
									Dim openESMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, viewData.ESNr)
									hub.Publish(openESMng)
								End If

							Case Else
								If viewData.ESNr > 0 Then
									Dim hub = MessageService.Instance.Hub
									Dim openReportMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, viewData.RPNr)
									hub.Publish(openReportMng)
								End If

						End Select

					End If
				End If
			End If


		End Sub


		''' <summary>
		''' Reads the RP offset from the settings.
		''' </summary>
		''' <returns>RP offset or zero if it could not be read.</returns>
		Private Function ReadRPOffsetFromSettings() As Integer

			Dim strQuery As String = "//StartNr/Rapporte"
			Dim r = m_ClsProgSetting.GetUserProfileFile
			Dim rapporteStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
			Dim intVal As Integer

			If Integer.TryParse(rapporteStartNumberSetting, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function




#Region "Helper Classes"


		''' <summary>
		''' Wraps an integer value.
		''' </summary>
		Class IntegerValueViewWrapper
			Public Property Value As Integer
		End Class


#End Region





	End Class


End Namespace
