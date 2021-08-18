
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
Imports SPRPListSearch
Imports SPGAV.TempData

Namespace UI

	''' <summary>
	''' Report management.
	''' </summary>
	Public Class frmReportMng

#Region "Constants"

		Private Const DEFAULT_SPUTNIK_UPDATE_UTILITIES_WEBSERVICE_URI = "wsSPS_services/SPUpdateUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
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
		''' Report overview data.
		''' </summary>
		Private m_ReportOverviewData As IEnumerable(Of RPOverviewData)

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
		Private m_tempDataMergedNews As PublicationNewsViewData

		''' <summary>
		''' Error icon.
		''' </summary>
		Private m_ErrorIcon As Image

		Private m_AllowedTempDataPVL As Boolean

#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try

				m_ErrorIcon = My.Resources.error_small

				' Mandantendaten
				m_md = New Mandant
				m_path = New ClsProgPath
				m_AllowedTempDataPVL = True

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

				m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

				Dim domainName = m_InitializationData.MDData.WebserviceDomain
				m_SPUpdateUtilitiesServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_UPDATE_UTILITIES_WEBSERVICE_URI)

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

			' Active symbol.
			m_CheckEditActive = CType(grdReportOverview.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditActive.PictureChecked = My.Resources.Checked
			m_CheckEditActive.PictureUnchecked = Nothing
			m_CheckEditActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_ListOfUserControls.Add(ucReportDetailData)
			m_ListOfUserControls.Add(ucReportDetailData2)
			m_ListOfUserControls.Add(ucMainContent)
			m_ListOfUserControls.Add(ucTimetableAndInfoData)
			m_ListOfUserControls.Add(ucNotes)
			m_ListOfUserControls.Add(ucCredit)
			m_ListOfUserControls.Add(ucMonthlySalaryData)
			m_ListOfUserControls.Add(ucAdvancePayment)

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfUserControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
			Next

			m_UCMediator = New UserControlFormMediator(Me,
													   ucMainContent,
													   ucReportDetailData,
													   ucReportDetailData2,
													   ucTimetableAndInfoData,
													   ucNotes,
													   ucCredit,
													   ucMonthlySalaryData,
													   ucAdvancePayment)

			m_ActiveBottomTabPage = ucNotes

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_ESDatabaseAccess = New DatabaseAccess.ES.ESDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility
			m_CreateRPService = New CreateRPService(m_InitializationData.MDData.MDNr, m_InitializationData)

			' Translate controls
			TranslateControls()

			' Creates the navigation bar.
			CreateMyNavBar()

			Dim communicationHub = MessageService.Instance.Hub
			communicationHub.Subscribe(Of ESDataHasChanged)(AddressOf HandleOESDataHasChangedMsg)

		End Sub

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

		''' <summary>
		''' Gets the UC control mediator.
		''' </summary>
		''' <returns>The UC control mediator.</returns>
		Public ReadOnly Property UCMediator As UserControlFormMediator
			Get
				Return m_UCMediator
			End Get
		End Property

		''' <summary>
		''' Gets or sets data valid flag.
		''' </summary>
		''' <returns>Data valid flag</returns>
		Public Property IsDataValid As Boolean
			Get
				Return m_IsDataValid
			End Get
			Set(value As Boolean)

				m_IsDataValid = value

				If Not m_IsDataValid AndAlso Not m_SaveButton Is Nothing Then
					m_SaveButton.Enabled = False
				End If

			End Set
		End Property

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

		''' <summary>
		''' Gets the first report overview in list.
		''' </summary>
		''' <returns>First report overview in list or nothing.</returns>
		Public ReadOnly Property FirstReportOverviewInList As RPOverviewData
			Get
				If gvReportOverview.RowCount > 0 Then

					Dim rowHandle = gvReportOverview.GetVisibleRowHandle(0)
					Return CType(gvReportOverview.GetRow(rowHandle), RPOverviewData)
				Else
					Return Nothing
				End If

			End Get
		End Property

		''' <summary>
		''' Gets the active report data.
		''' </summary>
		Public ReadOnly Property AcitiveReportData As ActiveReportData
			Get
				Return m_ActiveReportData
			End Get
		End Property

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets a boolean flag indiciating if the ui events should be suppressed.
		''' </summary>
		Private ReadOnly Property AreUIEventsSuppressed As Boolean
			Get
				Return m_SuppressUIEvents
			End Get
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Show the data of a report.
		''' </summary>
		''' <param name="reportNumber">The report number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadReportData(ByVal reportNumber As Integer) As Boolean

			If Not m_SaveButton Is Nothing Then
				m_SaveButton.Enabled = True
			End If

			If Not m_IsInitialized Then
				Reset()
				m_IsInitialized = True
			End If

			CleanUp()

			SetSuppressUIEventsState(True)

			Dim success As Boolean = True

			success = success AndAlso LoadReportOverviewData()
			success = success AndAlso LoadReportDataInternal(reportNumber)

			If success Then
				FocusReportOverviewData(reportNumber)
			End If

			m_ReportNumber = IIf(success, reportNumber, Nothing)
			If Not m_ReportNumber Is Nothing Then Me.xtabReportOverviewData.SelectedTabPageIndex = 1

			IsDataValid = success

			If IsDataValid Then
				CheckGAVValidityOfSelectedESSalaryData()
			End If

			SetSuppressUIEventsState(False)

			Return success
		End Function

#End Region

#Region "Private Methods"

#Region "Reset"

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			Dim previousState = SetSuppressUIEventsState(True)

			m_ReportOverviewData = Nothing
			m_ActiveReportData = Nothing
			m_ReportNumber = Nothing
			m_ReportHasBeenReloadedDueToChangedES = False

			m_SaveButton.Enabled = True
			m_DeleteButton.Enabled = True

			' Reset all the child controls
			For Each ctrl In m_ListOfUserControls
				ctrl.Reset()
			Next

			xtabReportOverviewData.SelectedTabPageIndex = 0
			chkBtnReportDetails.Checked = True

			ucReportDetailData.Width = SplitContainerControl1.Panel1.Width
			ucReportDetailData2.Width = SplitContainerControl1.Panel1.Width

			' Reset GAV Info check information
			bsiBtnGavInfo.Caption = String.Empty
			bsiBtnGavInfo.Glyph = Nothing

			'  Reset grids, drop downs and lists, etc.

			ResetReportOverviewGrid()

			SetSuppressUIEventsState(previousState)
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
			columnRPFrom.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnRPFrom.Name = "ReportFrom"
			columnRPFrom.FieldName = "ReportFrom"
			columnRPFrom.Visible = False
			gvReportOverview.Columns.Add(columnRPFrom)

			Dim columnRPTo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPTo.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnRPTo.Name = "ReportTo"
			columnRPTo.FieldName = "ReportTo"
			columnRPTo.Visible = False
			gvReportOverview.Columns.Add(columnRPTo)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = "M"
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
			'columnYear.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			'columnYear.DisplayFormat.FormatString = "N4"

			columnYear.Width = 40
			gvReportOverview.Columns.Add(columnYear)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			'activeColumn.Caption = m_Translate.GetSafeTranslationValue("Erfasst")
			activeColumn.Name = "Erfasst"
			activeColumn.FieldName = "Erfasst"
			activeColumn.Visible = True
			activeColumn.ColumnEdit = m_CheckEditActive
			activeColumn.Width = 10
			gvReportOverview.Columns.Add(activeColumn)

			grdReportOverview.DataSource = Nothing

		End Sub

		''' <summary>
		''' CleanUp the form
		''' </summary>
		Private Sub CleanUp()

			' Cleanup all the child controls
			For Each ctrl In m_ListOfUserControls
				ctrl.CleanUp()
			Next

		End Sub

#End Region

#Region "Load, save, delete and new report data"

		''' <summary>
		''' Loads report overview data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadReportOverviewData() As Boolean

			m_ReportOverviewData = m_ReportDatabaseAccess.LoadRPOverviewListData()

			If m_ReportOverviewData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportübersicht konnte nicht geladen werden."))
			End If

			Dim previousState = SetSuppressUIEventsState(True)
			grdReportOverview.DataSource = m_ReportOverviewData
			SetSuppressUIEventsState(previousState)

			Return Not m_ReportOverviewData Is Nothing

		End Function

		''' <summary>
		''' Loads the report data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadReportDataInternal(ByVal rpNr As Integer) As Boolean

			Dim rpMasterData = m_ReportDatabaseAccess.LoadRPMasterData(rpNr)

			If rpMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Reportstammdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(rpMasterData.EmployeeNumber, False)
			Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(rpMasterData.CustomerNumber, m_InitializationData.UserData.UserFiliale)

			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterstammdaten konnten nicht geladen werden."))
			End If

			If customerMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundenstammdaten konnten nicht geladen werden."))
			End If

			If Not rpMasterData Is Nothing And
			  Not employeeMasterData Is Nothing And
			  Not customerMasterData Is Nothing Then

				m_ActiveReportData = New ActiveReportData With {
				.ReportData = rpMasterData,
				.EmployeeOfActiveReport = employeeMasterData,
				.CustomerOfActiveReport = customerMasterData
			   }

				Dim esData = m_ESDatabaseAccess.LoadESMasterData(rpMasterData.ESNR)

				' Check if ES data can be loaded. Otherwhise the report is invalid an should be deleted.
				If (esData Is Nothing) Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Einsatzdaten des Rapports konnten nicht geladen werden. Der Rapport ist ungültig und wird gelöscht."))

					DeleteReport()
					Return False
				End If

				m_ActiveReportData.ESDataOfActiveReport = esData

			Else
				m_ActiveReportData = Nothing
			End If

			' Do not change the sequence of the data load (ucMainContent control depends on selected salary data from ucReportDetailData control)
			ucReportDetailData.LoadDataOfActiveReport()
			ucReportDetailData2.LoadDataOfActiveReport()
			ucTimetableAndInfoData.LoadDataOfActiveReport()

			ucMainContent.LoadDataOfActiveReport()

			m_ActiveBottomTabPage.Activate()

			m_DeleteButton.Enabled = Not m_ActiveReportData.ReportData.IsMonthClosed
			m_SaveButton.Enabled = Not m_ActiveReportData.ReportData.IsMonthClosed

			If rpMasterData.IsMonthClosed Then

				Dim monthClosedData = m_ReportDatabaseAccess.LoadMonthCloseData(rpMasterData.MDNr, CType(rpMasterData.Monat, Integer), rpMasterData.Jahr)
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

		''' <summary>
		''' Saves report data.
		''' </summary>
		Public Sub SaveReportData()

			If (IsReportDataLoaded) Then

				Dim success As Boolean = True

				' Save data of bottom tops
				success = success And ucNotes.SaveReportData()
				success = success And ucCredit.SaveReportData()
				success = success And ucMonthlySalaryData.SaveReportData()
				success = success And ucAdvancePayment.SaveReportData()

				Dim message As String = String.Empty

				If (success) Then
					'bsiChanged.Caption = String.Format("{0:f}", DateTime.Now)

					DevExpress.XtraEditors.XtraMessageBox.Show((m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")),
															   m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxButtons.OK, MessageBoxIcon.Information)
				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))

				End If

			End If

		End Sub

		''' <summary>
		''' Updates RPL change time in status bar.
		''' </summary>
		''' <param name="rplChangeTime">The rpl change time.</param>
		''' <param name="changedFrom">The changed from string.</param>
		Public Sub UpdateRPLChangeTimeInStatusBar(ByVal rplChangeTime As DateTime?, ByVal changedFrom As String)

			bsiChanged.Caption = If(rplChangeTime.HasValue, String.Format("{0:f}, {1}", rplChangeTime, changedFrom), String.Empty)

			If m_ActiveReportData Is Nothing Then
				Return
			End If

			'Dim customerMediator = UCMediator.ActiveReportData.CustomerOfActiveReport
			'Dim employeeMediator = UCMediator.ActiveReportData.EmployeeOfActiveReport
			Dim einsatzMediator = UCMediator.ActiveReportData.ESDataOfActiveReport

			Dim allowedtoPrint As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 303, m_InitializationData.MDData.MDNr)
			If einsatzMediator.PrintNoRP.HasValue Then
				allowedtoPrint = allowedtoPrint And Not einsatzMediator.PrintNoRP
			End If

			'			If Not customerMediator.NotPrintReports.HasValue Then
			Print_P_Data.Enabled = allowedtoPrint
			'Else
			'Print_P_Data.Enabled = allowedtoPrint

			'End If

		End Sub

		''' <summary>
		''' Deletes the active report.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function DeleteReport()

			Dim success As Boolean = True

			If m_ActiveReportData Is Nothing Then
				Return False
			End If

			Dim iRPNr As Integer = 0
			Dim iESNr As Integer = 0
			Dim iMANr As Integer = 0
			Dim iKDNr As Integer = 0
			Dim rpData = AcitiveReportData.ReportData
			Dim strResult As String = "Success..."

			Dim _clsRPDeleteFunc As New ClsDeleteRPData

			iRPNr = rpData.RPNR
			iMANr = rpData.EmployeeNumber
			iKDNr = rpData.CustomerNumber
			iESNr = rpData.ESNR

			Dim _setting As New ClsRPDataSetting With {.SelectedRPNr = iRPNr,
													   .SelectedMANr = iMANr,
													   .SelectedKDNr = iKDNr,
													   .SelectedESNr = iESNr,
													   .ShowMsgBox = True}

			Try
				strResult = _clsRPDeleteFunc.DeleteSelectedRPFromDb(_setting)
			Catch ex As Exception
				success = False
				strResult = String.Empty
			End Try

			If strResult.ToLower.Contains("error") Then
				success = False
			End If

			If success Then
				LoadReportOverviewData()

				Dim firstReport = FirstReportOverviewInList

				If Not firstReport Is Nothing Then
					LoadReportData(firstReport.RPNr)
				Else
					Reset()
				End If
			End If

			Return success
		End Function

		''' <summary>
		''' Shows a todo From.
		''' </summary>
		Private Sub ShowTodo()
			Dim frmTodo As New frmTodo(m_InitializationData)
			' optional init new todo

			If m_ActiveReportData Is Nothing Then
				Exit Sub
			End If

			Dim rpData = AcitiveReportData.ReportData

			Dim _clsRPDeleteFunc As New ClsDeleteRPData

			Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
			Dim EmployeeNumber As Integer? = rpData.EmployeeNumber
			Dim CustomerNumber As Integer? = rpData.CustomerNumber
			Dim ResponsiblePersonRecordNumber As Integer? = Nothing
			Dim VacancyNumber As Integer? = Nothing
			Dim ProposeNumber As Integer? = Nothing
			Dim ESNumber As Integer? = rpData.ESNR
			Dim RPNumber As Integer? = rpData.RPNR
			Dim LMNumber As Integer? = Nothing
			Dim RENumber As Integer? = Nothing
			Dim ZENumber As Integer? = Nothing
			Dim Subject As String = String.Empty
			Dim Body As String = ""

			frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
								VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

			frmTodo.Show()

		End Sub


		''' <summary>
		''' Validates the data on the tabs.
		''' </summary>
		Private Function ValidateData() As Boolean

			Dim valid As Boolean = True
			For Each userControl In m_ListOfUserControls
				valid = valid AndAlso userControl.ValidateData()
			Next
			Return valid

		End Function

		''' <summary>
		''' Creates a new report for next month.
		''' </summary>
		Private Sub NewReportForNextMonth()

			Dim success As Boolean = True

			If AcitiveReportData Is Nothing Then
				Return
			End If

			Dim rpData = AcitiveReportData.ReportData

			Dim initData As New CreateRPCopyForExistingESParams

			initData.ESNr = rpData.ESNR
			initData.Month_OfReportToCopy = rpData.Monat
			initData.Year_OfReportToCopy = rpData.Jahr
			initData.VonDate_OfReportToCopy = rpData.Von
			initData.BisDate_OfReportToCopy = rpData.Bis
			initData.Suva_OfReprotToCopy = rpData.SUVA
			initData.Kst_OfReportToCopy = rpData.RPKST
			initData.Kst1_OfReportToCopy = rpData.RPKST1
			initData.Kst2_OfReportToCopy = rpData.RPKST2
			initData.KDBranche_OfReportToCopy = rpData.KDBranche

			initData.MDNr = rpData.MDNr
			initData.RPNumberOffset = ReadRPOffsetFromSettings()
			initData.UserName = m_InitializationData.UserData.UserFullName

			m_CreateRPService.CreateReportCopyForExistingES(initData)

			Select Case initData.ResultCode
				Case CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultSuccess
					LoadReportData(initData.NewRPNrOutput)
				Case CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultFailure
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler beim Erstellen des Rapports."))
				Case CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultNoMoreReportsAllowed
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Für diesen Einsatz können keine Rapporte mehr erstellt werden.") & vbCrLf &
											 m_Translate.GetSafeTranslationValue("Möglicherweise wurde der letzte Rapport bereits erstellt."),
											 m_Translate.GetSafeTranslationValue("Neuer Rapport"), MessageBoxIcon.Information)
				Case CreateRPCopyForExistingESParams.CreateRPCopyForExistingESResult.ResultReportForNextMonthIsAlreadyExisting
					LoadReportData(initData.NewRPNrOutput)
					m_UtilityUI.ShowOKDialog(String.Format("{0}. {1}: {2}", m_Translate.GetSafeTranslationValue("Der Rapport für den nächsten Monat existiert bereits"),
																			m_Translate.GetSafeTranslationValue("Rapportnummer"),
																			initData.NewRPNrOutput), m_Translate.GetSafeTranslationValue("Neuer Rapport"), MessageBoxIcon.Information)

			End Select

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

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles ES change notifcation messages.
		''' </summary>
		''' <param name="msg">The ES data changed message.</param>
		Private Sub HandleOESDataHasChangedMsg(ByVal msg As ESDataHasChanged)

			If m_ActiveReportData Is Nothing Then
				Return
			End If

			If msg.ESNr = m_ActiveReportData.ReportData.ESNR Then
				LoadReportData(m_ActiveReportData.ReportData.RPNR)
				m_ReportHasBeenReloadedDueToChangedES = True
			End If

		End Sub

		''' <summary>
		''' Handles tab control selection changing
		''' </summary>
		Private Sub OnxtraTabControl_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles XtraTabControl1.SelectedPageChanging

			If m_SuppressUIEvents Then
				Return
			End If

			Dim page = e.Page

			If Not (m_ActiveBottomTabPage Is Nothing) Then
				m_ActiveBottomTabPage.Deactivate()
			End If

			If (Object.ReferenceEquals(page, xtabBemerkung)) Then
				ucNotes.Activate()
				m_ActiveBottomTabPage = ucNotes
			ElseIf (Object.ReferenceEquals(page, xtabGuthaben)) Then
				ucCredit.Activate()
				m_ActiveBottomTabPage = ucCredit
			ElseIf (Object.ReferenceEquals(page, xtabLM)) Then
				ucMonthlySalaryData.Activate()
				m_ActiveBottomTabPage = ucMonthlySalaryData

			ElseIf (Object.ReferenceEquals(page, xtabZG)) Then
				ucAdvancePayment.Activate()
				m_ActiveBottomTabPage = ucAdvancePayment

			End If

		End Sub

		''' <summary>
		''' Handles change of focus on report overview grid.
		''' </summary>
		Private Sub OngvReportOverview_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvReportOverview.FocusedRowChanged

			If AreUIEventsSuppressed Then
				Return
			End If

			Dim selectedReportOverview = SelectedReportOverviewData

			If Not selectedReportOverview Is Nothing Then

				Dim success As Boolean = True

				success = LoadReportDataInternal(SelectedReportOverviewData.RPNr)

				m_ReportNumber = IIf(success, SelectedReportOverviewData.RPNr, Nothing)

				IsDataValid = success

				Me.xtabReportOverviewData.SelectedTabPageIndex = 1
			End If
		End Sub


		''' <summary>
		''' Handles click on report detail or report info button.
		''' </summary>
		Private Sub OnChkBtnReportDetails_Click(sender As System.Object, e As System.EventArgs) Handles chkBtnReportDetails.Click, chkBtnReportInfo.Click

			If Object.ReferenceEquals(sender, chkBtnReportDetails) Then
				ucReportDetailData.Visible = True
				ucReportDetailData2.Visible = False
			Else
				ucReportDetailData2.Visible = True
				ucReportDetailData.Visible = False
			End If

		End Sub

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmEmployees_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmEmployees_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnFrmResponsiblePerson_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

			CleanupAndHideForm()

			e.Cancel = True

		End Sub

		''' <summary>
		''' Handles form activated event.
		''' </summary>
		Private Sub OnFrmReportMng_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated

			If m_ReportHasBeenReloadedDueToChangedES Then
				m_ReportHasBeenReloadedDueToChangedES = False
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Daten wurden neue geladen, da der zugrunde liegende Einsatz verändert wurde."))
			End If

		End Sub

		''' <summary>
		''' Keypreview for Modul-version
		''' </summary>
		Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
			If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
				Dim strRAssembly As String = ""
				Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
				For Each a In AppDomain.CurrentDomain.GetAssemblies()
					strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
				Next
				strMsg = String.Format(strMsg, vbNewLine,
									   GetExecutingAssembly().FullName,
									   GetExecutingAssembly().Location,
									   strRAssembly)
				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If
		End Sub

		''' <summary>
		''' Clickevent for Navbar.
		''' </summary>
		Private Sub OnnbMain_LinkClicked(ByVal sender As Object,
									 ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim bForDesign As Boolean = False
			Try
				Dim strLinkName As String = e.Link.ItemName
				Dim strLinkCaption As String = e.Link.Caption

				For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
					e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
				Next
				e.Link.Item.Appearance.ForeColor = Color.Orange

				Select Case strLinkName.ToLower
					Case "New_Report".ToLower()
						NewReportForNextMonth()
					Case "Close_Report_Form".ToLower
						CleanupAndHideForm()

					Case "Save_Report_Data".ToLower
						SaveReportData()

					Case "Print_Report_Data".ToLower
						PrintSelectedReport()

					Case "Delete_Report_Data".ToLower()
						DeleteReport()

					Case "CreateTODO".ToLower
						ShowTodo()

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(ex.Message)

			Finally

			End Try

		End Sub

#End Region

#Region "Helper Methods"


		Private Sub PrintSelectedReport()
			Dim employeeNumber As Integer = m_UCMediator.ActiveReportData.ReportData.EmployeeNumber
			Dim customerNumber As Integer = m_UCMediator.ActiveReportData.ReportData.CustomerNumber
			Dim employmentNumber As Integer = m_UCMediator.ActiveReportData.ReportData.ESNR
			Dim reportNumber As Integer = m_UCMediator.ActiveReportData.ReportData.RPNR

			Try
				'If My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown Then
				Try
					Dim frm = New frmRPListSearch(m_InitializationData)
					frm.EmployeeNumber = employeeNumber
					frm.CustomerNumber = customerNumber
					frm.EmploymentNumber = employmentNumber
					frm.ReportNumber = reportNumber
					frm.AssignedMonth = m_UCMediator.ActiveReportData.ReportData.Monat
					frm.AssignedYear = m_UCMediator.ActiveReportData.ReportData.Jahr

					frm.LoadData()

					frm.Show()
					frm.BringToFront()

				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog(ex.ToString)
					m_Logger.LogError(ex.ToString)

					Return
				End Try

				'Else
				'	If vb6Object Is Nothing Then
				'		vb6Object = CreateObject("SPSModulsView.ClsMain")
				'	End If
				'	vb6Object.TranslateProg4Net("PrintSelectedRP", reportNumber)

				'End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("{0}: {1}", ex.ToString))

			End Try

		End Sub

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			xtabEmployeeList.Text = m_Translate.GetSafeTranslationValue(xtabEmployeeList.Text)
			xtabEmployeeDetail.Text = m_Translate.GetSafeTranslationValue(xtabEmployeeDetail.Text)
			chkBtnReportDetails.Text = m_Translate.GetSafeTranslationValue(chkBtnReportDetails.Text)
			chkBtnReportInfo.Text = m_Translate.GetSafeTranslationValue(chkBtnReportInfo.Text)

			xtabBemerkung.Text = m_Translate.GetSafeTranslationValue(xtabBemerkung.Text)
			xtabGuthaben.Text = m_Translate.GetSafeTranslationValue(xtabGuthaben.Text)
			xtabLM.Text = m_Translate.GetSafeTranslationValue(xtabLM.Text)
			xtabZG.Text = m_Translate.GetSafeTranslationValue(xtabZG.Text)

			Me.bsiLblGeandert.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblGeandert.Caption)
		End Sub

		''' <summary>
		''' Focuses report overview data.
		''' </summary>
		''' <param name="rpNr">The report number.</param>
		Private Sub FocusReportOverviewData(ByVal rpNr As Integer)

			If Not grdReportOverview.DataSource Is Nothing Then

				Dim reportOverviewData = CType(grdReportOverview.DataSource, List(Of RPOverviewData))

				Dim index = reportOverviewData.ToList().FindIndex(Function(data) data.RPNr = rpNr)

				Dim previousState = SetSuppressUIEventsState(True)
				Dim rowHandle = gvReportOverview.GetRowHandle(index)
				gvReportOverview.FocusedRowHandle = rowHandle
				previousState = SetSuppressUIEventsState(previousState)
			End If

		End Sub

		''' <summary>
		''' Creates Navigationbar
		''' </summary>
		Private Sub CreateMyNavBar()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Me.navMain.Items.Clear()
			Try
				navMain.PaintStyleName = "SkinExplorerBarView"

				' Create a Local group.
				Dim groupDatei As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Datei"))
				groupDatei.Name = "gNavDatei"

				Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
				New_P.Name = "New_Report"
				New_P.SmallImage = Me.ImageCollection1.Images(0)

				m_SaveButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
				m_SaveButton.Name = "Save_Report_Data"
				m_SaveButton.SmallImage = Me.ImageCollection1.Images(1)
				m_SaveButton.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 301, m_InitializationData.MDData.MDNr) AndAlso IsDataValid

				Print_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
				Print_P_Data.Name = "Print_Report_Data"
				Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)

				Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
				Close_P_Data.Name = "Close_Report_Form"
				Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

				Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
				groupDelete.Name = "gNavDelete"
				groupDelete.Appearance.ForeColor = Color.Red

				m_DeleteButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
				m_DeleteButton.Name = "Delete_Report_Data"
				m_DeleteButton.SmallImage = Me.ImageCollection1.Images(4)
				m_DeleteButton.Appearance.ForeColor = Color.Red
				m_DeleteButton.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 302, m_InitializationData.MDData.MDNr)

				Dim groupExtra As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
				groupExtra.Name = "gNavExtra"

				Dim TODO_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("To-do erstellen"))
				TODO_P_Data.Name = "CreateTODO"
				TODO_P_Data.SmallImage = Me.ImageCollection1.Images(9)

				Try
					navMain.BeginUpdate()

					navMain.Groups.Add(groupDatei)
					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 301, m_InitializationData.MDData.MDNr) Then
						groupDatei.ItemLinks.Add(New_P)
						groupDatei.ItemLinks.Add(m_SaveButton)
					End If

					groupDatei.ItemLinks.Add(Print_P_Data)
					groupDatei.ItemLinks.Add(Close_P_Data)
					groupDatei.Expanded = True

					navMain.Groups.Add(groupDelete)
					groupDelete.ItemLinks.Add(m_DeleteButton)
					groupDelete.Expanded = False

					navMain.Groups.Add(groupExtra)
					groupExtra.ItemLinks.Add(TODO_P_Data)

					groupExtra.Expanded = True

					navMain.EndUpdate()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message),
														   "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

		End Sub

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)

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
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFromSettings()

			' Cleanup child panels.
			For Each ctrl In m_ListOfUserControls
				ctrl.CleanUp()
			Next

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
		''' Checks the GAV validity of the selected ES salary data.
		''' </summary>
		Private Sub CheckGAVValidityOfSelectedESSalaryData()

			Dim selectedSalaryData = m_UCMediator.SelectedESSalaryData
			If selectedSalaryData Is Nothing Then Return

			CheckGAVValidityOfESSalaryData(selectedSalaryData)

		End Sub

		''' <summary>
		''' Checks GAV validity of ES salary data.
		''' </summary>
		''' <param name="esLohn">The ES Salary data.</param>
		Public Sub CheckGAVValidityOfESSalaryData(ByVal esLohn As ESSalaryData)

			bsiBtnGavInfo.Glyph = Nothing
			bsiBtnGavInfo.Caption = String.Empty

			If (m_ActiveBottomTabPage Is Nothing) Then Return

			If m_AllowedTempDataPVL Then
				CheckGAVValidityData_API(esLohn)

			Else
				CheckGAVValidityData_XML(esLohn)

			End If

		End Sub

		Private Sub CheckGAVValidityData_XML(ByVal eslohn As ESSalaryData)

			Dim esData = m_ActiveReportData.ESDataOfActiveReport

			Dim esEnde = If(esData.ES_Ende.HasValue, esData.ES_Ende.Value, New DateTime(3999, 12, 31))
			Dim esLohnBis = If(eslohn.LOBis.HasValue, eslohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
			Dim effectiveESLohnBis = m_DateAndTimeUtility.MinDate(esEnde, esLohnBis)

			If effectiveESLohnBis < DateTime.Now.Date Then
				' Do not check ESLohn data that are not valid any more.
				Return
			End If

			Dim errorMsg = m_Translate.GetSafeTranslationValue("GAV Version konnte nicht geprüft werden.")
			Dim gavVersionChecked As String = m_Translate.GetSafeTranslationValue("GAV Version wurde überprüft.")
			Dim gavDataHasChanged As String = m_Translate.GetSafeTranslationValue("GAV Daten haben sich geändert.")

			Try
				If eslohn.GAVNr > 10000 AndAlso eslohn.GAVNr <> 99999 Then

					Dim ws = New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
					ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPUpdateUtilitiesServiceUrl)
					m_GavUpdateInfoOfLastCheck = ws.GetGAVVersionNotificationData(m_InitializationData.MDData.MDGuid, eslohn.GAVNr)

					If (m_GavUpdateInfoOfLastCheck Is Nothing) Then
						m_UtilityUI.ShowErrorDialog(errorMsg)
						Return
					End If

					If m_GavUpdateInfoOfLastCheck.GAVDate.HasValue Then

						Dim esLohnVonDate As DateTime = eslohn.Createdon ' LOVon
						Dim esLohnBisDate As DateTime = If(eslohn.LOBis.HasValue, eslohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
						Dim gavUpdateDate As DateTime = m_GavUpdateInfoOfLastCheck.GAVDate.Value '.Date

						' Check if the GAVNr ha between updated between ESLohnVon and ESLohnBis.
						If gavUpdateDate >= esLohnVonDate AndAlso gavUpdateDate <= esLohnBisDate Then

							bsiBtnGavInfo.Glyph = m_ErrorIcon
							bsiBtnGavInfo.Caption = gavDataHasChanged

						Else
							bsiBtnGavInfo.Caption = gavVersionChecked
						End If

					Else
						bsiBtnGavInfo.Caption = gavVersionChecked
					End If
				Else
					bsiBtnGavInfo.Caption = errorMsg
				End If

			Catch ex As Exception

				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(errorMsg)

			End Try

		End Sub

		Private Sub CheckGAVValidityData_API(ByVal esLohn As ESSalaryData)

			Dim esData = m_ActiveReportData.ESDataOfActiveReport
			Dim rpData = m_ActiveReportData.ReportData

			Dim esEnde = If(esData.ES_Ende.HasValue, esData.ES_Ende.Value, New DateTime(3999, 12, 31))
			Dim esLohnBis = If(esLohn.LOBis.HasValue, esLohn.LOBis.Value.Date, New DateTime(3999, 12, 31))
			Dim effectiveESLohnBis = m_DateAndTimeUtility.MinDate(esEnde, esLohnBis)

			If effectiveESLohnBis < DateTime.Now.Date Then
				' Do not check ESLohn data that are not valid any more.
				'Return
			End If

			Dim errorMsg = m_Translate.GetSafeTranslationValue("GAV Version konnte nicht geprüft werden.")
			Dim gavVersionChecked As String = m_Translate.GetSafeTranslationValue("GAV Version wurde überprüft.")
			Dim gavDataHasChanged As String = m_Translate.GetSafeTranslationValue("GAV Daten haben sich geändert.")

			Try
				If esLohn.GAVNr > 10000 AndAlso esLohn.GAVNr <> 99999 Then

					Dim newsObj = New SPGAV.UI.frmPublicationNews(m_InitializationData)
					Dim newsData = newsObj.LoadMergedNewsForAssignedConctractData(esLohn.GAVNr)

					If (newsData Is Nothing) Then
						m_UtilityUI.ShowErrorDialog(errorMsg)

						Return
					End If

					m_tempDataMergedNews = newsData.Where(Function(x) x.ContractNumber = esLohn.GAVNr And x.PublicationDate >= esLohn.LOVon And x.PublicationDate <= rpData.Bis And x.PublicationDate <= esEnde).FirstOrDefault
					If m_tempDataMergedNews Is Nothing Then
						bsiBtnGavInfo.Caption = gavVersionChecked
						bsiBtnGavInfo.Glyph = My.Resources.apply_16x16

						Return
					End If
					Dim gavUpdateDate As DateTime = m_tempDataMergedNews.PublicationDate

					If rpData.Bis < gavUpdateDate OrElse esEnde < gavUpdateDate Then Return

					' Check if the GAVNr has between updated between ESLohnVon and ESLohnBis.
					If gavUpdateDate >= esLohn.LOVon AndAlso gavUpdateDate <= rpData.Bis Then

						bsiBtnGavInfo.Glyph = m_ErrorIcon
						bsiBtnGavInfo.Caption = gavDataHasChanged

					Else
						bsiBtnGavInfo.Caption = gavVersionChecked
						bsiBtnGavInfo.Glyph = My.Resources.apply_16x16

					End If

				Else
					bsiBtnGavInfo.Caption = errorMsg

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Service-URL: {0} | GAVNumber: {1} | Error: {2}", m_SPUpdateUtilitiesServiceUrl, esLohn.GAVNr, ex.ToString))
				m_UtilityUI.ShowErrorDialog(errorMsg)

			End Try

		End Sub


		''' <summary>
		''' Handles click on bsiBtnGavInfo.
		''' </summary>
		Private Sub OnbsiBtnGavInfo_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bsiBtnGavInfo.ItemClick

			If m_AllowedTempDataPVL Then
				ShowGAVNotificationMessage_API()

			Else
				ShowGAVNotificationMessage_XML()

			End If

		End Sub

		Private Sub ShowGAVNotificationMessage_XML()

			If Not m_GavUpdateInfoOfLastCheck Is Nothing AndAlso Not String.IsNullOrWhiteSpace(m_GavUpdateInfoOfLastCheck.GAVInfo) Then

				Dim infoText = String.Format(m_GavUpdateInfoOfLastCheck.GAVInfo, vbCrLf)

				m_UtilityUI.ShowOKDialog(infoText, String.Format("{0} (Nr.: {1}): {2:dd.MM.yyyy}",
														 m_Translate.GetSafeTranslationValue("Datum letzte GAV Anpassung"),
														 m_GavUpdateInfoOfLastCheck.GAVNumber,
														 m_GavUpdateInfoOfLastCheck.GAVDate.Value))

			End If

		End Sub

		Private Sub ShowGAVNotificationMessage_API()

			If m_tempDataMergedNews Is Nothing OrElse m_tempDataMergedNews.VersionNumber.GetValueOrDefault(0) = 0 Then Return

			Dim infoText = String.Format("{0:G}:<br>{1}", m_tempDataMergedNews.PublicationDate, m_tempDataMergedNews.Content)
			m_UtilityUI.ShowOKDialog(Me, infoText, m_Translate.GetSafeTranslationValue("GAV Anpassung"), MessageBoxIcon.Asterisk)

		End Sub

#End Region

#End Region


	End Class

End Namespace
