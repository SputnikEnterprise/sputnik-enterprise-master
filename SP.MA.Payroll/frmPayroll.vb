Imports System.Reflection.Assembly
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports SP.DatabaseAccess.PayrollMng
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.ucListSelectPopup
Imports SP.MA.PayrollMng.Settings
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors.Repository
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Linq
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports System.Text
Imports System.Reflection
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports DevExpress.XtraBars.Docking2010

Namespace UI

	''' <summary>
	''' Payroll management.
	''' </summary>
	Public Class frmPayroll


		Private Delegate Sub StartLoadingData()

#Region "Private Consts"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
		Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"

		Private Const POPUP_DEFAULT_WIDTH As Integer = 420
		Private Const POPUP_DEFAULT_HEIGHT As Integer = 325

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
		''' The Payroll data access object.
		''' </summary>
		Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess
		Private m_ReportDatabaseAccess As IReportDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As DatabaseAccess.Common.ICommonDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		Private m_SearchMandantNumber As Integer
		Private m_SearchYear As Integer
		Private m_SearchMonth As Integer

		Private m_AvailableEmloyees As New List(Of DataObjects.EmployeeDataForPayroll)
		Private m_ChoosenEmployees As New List(Of DataObjects.EmployeeDataForPayroll)

		Private m_FrmInvalidRecordNumbers As frmInvalidRecordNumbers

		Private m_Years As List(Of IntegerValueViewWrapper)
		Private m_Month As List(Of IntegerValueViewWrapper)

		Private m_CheckEdit As RepositoryItemCheckEdit

		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_md As Mandant
		Private m_path As ClsProgPath
		Private m_IsInitialized = False

		Private m_Unprocessed As Image
		Private m_InProcessing As Image
		Private m_Processed As Image
		Private m_Failed As Image

		Private m_TaskHelper As TaskHelper

		Private m_IsProcessing As Boolean = False

		Private m_ResultOfLastLORun As LORunResult = Nothing
		Private m_loRunResult As LORunResult

		Private m_FirstCreatedLONr As Integer?

		Private m_SuccessfullCreatedLONr As List(Of Integer)

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the selected employee in the available employee grid data.
		''' </summary>
		''' <returns>The selected employee or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedEmployeeInAvailableEmployeesGrid As DataObjects.EmployeeDataForPayroll
			Get
				Dim grdView = TryCast(grdAvailableEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(grdView.GetRow(selectedRows(0)), DataObjects.EmployeeDataForPayroll)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected employee in the choosen employee grid data.
		''' </summary>
		''' <returns>The selected employee or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedEmployeeInChoosenEmployeesGrid As DataObjects.EmployeeDataForPayroll
			Get
				Dim grdView = TryCast(grdChoosenEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(grdView.GetRow(selectedRows(0)), DataObjects.EmployeeDataForPayroll)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets or sets the processing state.
		''' </summary>
		Private Property IsProcessing As Boolean
			Get
				Return m_IsProcessing
			End Get
			Set(value As Boolean)

				If Not m_IsProcessing = value Then
					m_IsProcessing = value

					SetControlsStateForProcessingState()

					PublishPayrollProcessingState()

				End If

			End Set
		End Property


		''' <summary>
		''' should open printform?
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property CheckOpenPrintFormAutomaticaly As Boolean
			Get

				Dim value As Boolean = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/payrollopenprintformaftercreate", FORM_XML_MAIN_KEY)), True)

				Return value

			End Get
		End Property

		Private ReadOnly Property AllowedSocialLAWithoutEmployment(ByVal iYear As Integer) As Decimal
			Get
				Dim mdNr = m_InitializationData.MDData.MDNr
				Dim payrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, mdNr)
				Dim value As Boolean = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(mdNr, iYear), String.Format("{0}/allowedsociallawithoutemployment", payrollSetting)), False)
				Return value
			End Get
		End Property


#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			Dim currentDomain As AppDomain = AppDomain.CurrentDomain
			AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				' Mandantendaten
				m_md = New Mandant
				m_path = New ClsProgPath
				m_Common = New CommonSetting

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
				m_TaskHelper = New TaskHelper(TaskScheduler.FromCurrentSynchronizationContext)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			' Important symbol.
			m_CheckEdit = New RepositoryItemCheckEdit()
			m_CheckEdit.PictureChecked = My.Resources.Checked
			m_CheckEdit.PictureUnchecked = Nothing
			m_CheckEdit.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_Unprocessed = My.Resources.Unprocessed
			m_InProcessing = My.Resources.Processing
			m_Processed = My.Resources.Processed
			m_Failed = My.Resources.Failed

			m_ResultOfLastLORun = Nothing
			m_loRunResult = New LORunResult(m_InitializationData) With {.CompletelySuccesful = True}
			m_ResultOfLastLORun = m_loRunResult

			' Translate controls.
			TranslateControls()

			' Button Click Handle

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick


			Reset()
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads the payroll data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadPayrollData() As Boolean

			Dim success As Boolean = True

			If Not IsProcessing Then

				success = success And LoadMandantDropDownData()

				ClearGrids()

				PreselectsMandantYearAndMonth()

			Else
				success = False
			End If

			Return success

		End Function

#End Region

#Region "Private Methods"

#Region "Reset Form"

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()

			m_SuppressUIEvents = True

			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
			m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_Years = Nothing
			m_Month = Nothing

			m_SearchMandantNumber = 0
			m_SearchYear = 0
			m_SearchMonth = 0

			lueMandant.Enabled = True
			lueYear.Enabled = True
			lueMonth.Enabled = True

			grdAvailableEmployees.Enabled = True
			grdEmployeeDetail.Enabled = True
			grdChoosenEmployees.Enabled = True

			btnSearch.Enabled = True
			btnSelectEmployee.Enabled = True
			btnSelectAllEmployee.Enabled = True
			btnUnselectEmployee.Enabled = True
			btnUnselectAllEmployees.Enabled = True
			btnCreatePayrolls.Enabled = True

			tgsSelection.EditValue = True
			tgsSelection.Enabled = True
			chkSetEmployeeLOBackSetting.Enabled = True
			chkEmployeeWithQSTFirst.Enabled = True

			' ---Reset drop downs, grids and lists---

			ResetMandantDropDown()
			ResetYearDropDown()
			ResetMonthDropDown()

			ResetAvailableEmployeesForPayrollGrid()
			ResetChoosenEmployeesForPayrollGrid()
			ResetEmployeeDetailGrid()

			m_SuppressUIEvents = False

			CloseInvalidRecordNumbersForm()

			Dim ballowed = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
			Me.lueMandant.Visible = ballowed
			Me.lblMandant.Visible = ballowed

			errorProvider.Clear()

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
		''' Resets the available employees detail grid.
		''' </summary>
		Private Sub ResetAvailableEmployeesForPayrollGrid()

			' Reset the grid
			gvAvailableEmployees.OptionsView.ShowIndicator = False
			gvAvailableEmployees.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvAvailableEmployees.OptionsView.ColumnAutoWidth = False
			gvAvailableEmployees.OptionsView.ColumnAutoWidth = False
			gvAvailableEmployees.OptionsView.ShowAutoFilterRow = True

			gvAvailableEmployees.Columns.Clear()

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Visible = True
			columnMANr.Width = 75
			gvAvailableEmployees.Columns.Add(columnMANr)

			Dim columnLastNameFirstName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLastNameFirstName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLastNameFirstName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnLastNameFirstName.Name = "Nachname_Vorname"
			columnLastNameFirstName.FieldName = "Nachname_Vorname"
			columnLastNameFirstName.Visible = True
			columnLastNameFirstName.Width = 120
			gvAvailableEmployees.Columns.Add(columnLastNameFirstName)

			Dim columnSteuerInfo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSteuerInfo.Caption = m_Translate.GetSafeTranslationValue("SteuerInfo")
			columnSteuerInfo.Name = "SteuerInfo"
			columnSteuerInfo.FieldName = "SteuerInfo"
			columnSteuerInfo.Visible = True
			columnSteuerInfo.Width = 70
			gvAvailableEmployees.Columns.Add(columnSteuerInfo)

			Dim columnChildrenCount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChildrenCount.Caption = m_Translate.GetSafeTranslationValue("Kinder")
			columnChildrenCount.Name = "Kinder"
			columnChildrenCount.FieldName = "Kinder"
			columnChildrenCount.Visible = True
			columnChildrenCount.Width = 45
			gvAvailableEmployees.Columns.Add(columnChildrenCount)

			Dim columnPaymentType As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPaymentType.Caption = m_Translate.GetSafeTranslationValue("Zahlart")
			columnPaymentType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnPaymentType.AppearanceHeader.Options.UseTextOptions = True
			columnPaymentType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnPaymentType.AppearanceCell.Options.UseTextOptions = True
			columnPaymentType.Name = "Zahlart"
			columnPaymentType.FieldName = "Zahlart"
			columnPaymentType.Visible = True
			columnPaymentType.Width = 55
			gvAvailableEmployees.Columns.Add(columnPaymentType)

			Dim columnFerienBack As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFerienBack.Caption = m_Translate.GetSafeTranslationValue("Fer")
			columnFerienBack.Name = "FerienBack"
			columnFerienBack.FieldName = "FerienBack"
			columnFerienBack.Visible = True
			columnFerienBack.ColumnEdit = m_CheckEdit
			columnFerienBack.Width = 40
			gvAvailableEmployees.Columns.Add(columnFerienBack)

			Dim columnFeierBack As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFeierBack.Caption = m_Translate.GetSafeTranslationValue("Fei")
			columnFeierBack.Name = "FeierBack"
			columnFeierBack.FieldName = "FeierBack"
			columnFeierBack.Visible = True
			columnFeierBack.ColumnEdit = m_CheckEdit
			columnFeierBack.Width = 40
			gvAvailableEmployees.Columns.Add(columnFeierBack)

			Dim columnLohn13Back As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLohn13Back.Caption = m_Translate.GetSafeTranslationValue("13.L")
			columnLohn13Back.Name = "Lohn13Back"
			columnLohn13Back.FieldName = "Lohn13Back"
			columnLohn13Back.Visible = True
			columnLohn13Back.ColumnEdit = m_CheckEdit
			columnLohn13Back.Width = 40
			gvAvailableEmployees.Columns.Add(columnLohn13Back)

			Dim columnMAGleitzeit As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMAGleitzeit.Caption = m_Translate.GetSafeTranslationValue("GZ")
			columnMAGleitzeit.Name = "MAGleitzeit"
			columnMAGleitzeit.FieldName = "MAGleitzeit"
			columnMAGleitzeit.Visible = True
			columnMAGleitzeit.ColumnEdit = m_CheckEdit
			columnMAGleitzeit.Width = 40
			gvAvailableEmployees.Columns.Add(columnMAGleitzeit)

			Dim columnInZV As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInZV.Caption = m_Translate.GetSafeTranslationValue("ZV")
			columnInZV.Name = "InZV"
			columnInZV.FieldName = "InZV"
			columnInZV.Visible = True
			columnInZV.ColumnEdit = m_CheckEdit
			columnInZV.Width = 40
			gvAvailableEmployees.Columns.Add(columnInZV)

		End Sub

		''' <summary>
		''' Resets the choosen employees grid.
		''' </summary>
		Private Sub ResetChoosenEmployeesForPayrollGrid()

			' Reset the grid
			gvChoosenEmployees.OptionsView.ShowIndicator = False
			gvChoosenEmployees.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvChoosenEmployees.Columns.Clear()

			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = String.Empty
			docType.Name = "Status"
			docType.FieldName = "Status"
			docType.Visible = True
			docType.ColumnEdit = New RepositoryItemPictureEdit()
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			gvChoosenEmployees.Columns.Add(docType)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.Caption = m_Translate.GetSafeTranslationValue("LONr")
			columnLONr.Name = "LONr"
			columnLONr.FieldName = "LONr"
			columnLONr.Visible = True
			columnLONr.Width = 75
			gvChoosenEmployees.Columns.Add(columnLONr)


			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Visible = True
			columnMANr.Width = 75
			gvChoosenEmployees.Columns.Add(columnMANr)

			Dim columnLastNameFirstName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLastNameFirstName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLastNameFirstName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnLastNameFirstName.Name = "Nachname_Vorname"
			columnLastNameFirstName.FieldName = "Nachname_Vorname"
			columnLastNameFirstName.Visible = True
			columnLastNameFirstName.Width = 120
			gvChoosenEmployees.Columns.Add(columnLastNameFirstName)

			Dim columnSteuerInfo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSteuerInfo.Caption = m_Translate.GetSafeTranslationValue("SteuerInfo")
			columnSteuerInfo.Name = "SteuerInfo"
			columnSteuerInfo.FieldName = "SteuerInfo"
			columnSteuerInfo.Visible = True
			columnSteuerInfo.Width = 70
			gvChoosenEmployees.Columns.Add(columnSteuerInfo)

			Dim columnChildrenCount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChildrenCount.Caption = m_Translate.GetSafeTranslationValue("Kinder")
			columnChildrenCount.Name = "Kinder"
			columnChildrenCount.FieldName = "Kinder"
			columnChildrenCount.Visible = True
			columnChildrenCount.Width = 45
			gvChoosenEmployees.Columns.Add(columnChildrenCount)

			Dim columnPaymentType As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPaymentType.Caption = m_Translate.GetSafeTranslationValue("Zahlart")
			columnPaymentType.Name = "Zahlart"
			columnPaymentType.FieldName = "Zahlart"
			columnPaymentType.Visible = True
			columnPaymentType.Width = 55
			gvChoosenEmployees.Columns.Add(columnPaymentType)

		End Sub

		''' <summary>
		''' Resets the employee detail grid.
		''' </summary>
		Private Sub ResetEmployeeDetailGrid()

			' Reset the grid
			gvEmployeeDetail.OptionsView.ShowIndicator = False
			gvEmployeeDetail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvEmployeeDetail.OptionsView.ColumnAutoWidth = False
			gvEmployeeDetail.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "LANR"
			columnLANr.FieldName = "LANR"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			columnLANr.Width = 50
			gvEmployeeDetail.Columns.Add(columnLANr)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "LALoText"
			columnDescription.FieldName = "LALoText"
			columnDescription.Visible = True
			columnDescription.Width = 100
			gvEmployeeDetail.Columns.Add(columnDescription)

			Dim columnCount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCount.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
			columnCount.Name = "M_Anzahl"
			columnCount.FieldName = "M_Anzahl"
			columnCount.Visible = True
			columnCount.Width = 70
			columnCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnCount.DisplayFormat.FormatString = "N"
			columnCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnCount.AppearanceHeader.Options.UseTextOptions = True
			columnCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnCount.AppearanceCell.Options.UseTextOptions = True
			gvEmployeeDetail.Columns.Add(columnCount)

			Dim columnAnsatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAnsatz.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
			columnAnsatz.Name = "M_Ansatz"
			columnAnsatz.FieldName = "M_Ansatz"
			columnAnsatz.Visible = False
			columnAnsatz.Width = 70
			columnAnsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAnsatz.DisplayFormat.FormatString = "N"
			columnAnsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnsatz.AppearanceHeader.Options.UseTextOptions = True
			columnAnsatz.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnsatz.AppearanceCell.Options.UseTextOptions = True
			gvEmployeeDetail.Columns.Add(columnAnsatz)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Basis")
			columnBasis.Name = "M_Basis"
			columnBasis.FieldName = "M_Basis"
			columnBasis.Visible = True
			columnBasis.Width = 70
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N"
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceCell.Options.UseTextOptions = True
			gvEmployeeDetail.Columns.Add(columnBasis)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "M_Betrag"
			columnBetrag.FieldName = "M_Betrag"
			columnBetrag.Visible = True
			columnBetrag.Width = 70
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N"
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceCell.Options.UseTextOptions = True
			gvEmployeeDetail.Columns.Add(columnBetrag)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCompany1.Name = "Company1"
			columnCompany1.FieldName = "Company1"
			columnCompany1.Visible = True
			columnCompany1.Width = 100
			gvEmployeeDetail.Columns.Add(columnCompany1)

			Dim columnModulNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnModulNumber.Caption = m_Translate.GetSafeTranslationValue("ModulNr")
			columnModulNumber.Name = "ModulNr"
			columnModulNumber.FieldName = "ModulNr"
			columnModulNumber.Visible = True
			columnModulNumber.Width = 70
			columnModulNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnModulNumber.AppearanceHeader.Options.UseTextOptions = True
			columnModulNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnModulNumber.AppearanceCell.Options.UseTextOptions = True
			columnModulNumber.Visible = False
			gvEmployeeDetail.Columns.Add(columnModulNumber)

			Dim columnModulName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnModulName.Caption = m_Translate.GetSafeTranslationValue("Modul")
			columnModulName.Name = "ModulName"
			columnModulName.FieldName = "ModulName"
			columnModulName.Visible = True
			columnModulName.Width = 70
			gvEmployeeDetail.Columns.Add(columnModulName)

			Dim columnNotice_Payroll As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNotice_Payroll.Caption = m_Translate.GetSafeTranslationValue("Bemerkung")
			columnNotice_Payroll.Name = "Notice_Payroll"
			columnNotice_Payroll.FieldName = "Notice_Payroll"
			columnNotice_Payroll.Visible = False
			columnNotice_Payroll.Width = 70
			gvEmployeeDetail.Columns.Add(columnNotice_Payroll)

		End Sub

#End Region


#Region "Load data"

		''' <summary>
		''' Search payroll data.
		''' </summary>
		Private Sub Search()

			Dim mandant = lueMandant.EditValue
			Dim year = lueYear.EditValue
			Dim month = lueMonth.EditValue

			If mandant Is Nothing Or year Is Nothing Or month Is Nothing Then
				Return
			End If

			m_PayrollDatabaseAccess.CleanupAllInvalidLO(mandant, year, month)

			m_SearchMandantNumber = mandant
			m_SearchYear = year
			m_SearchMonth = month

			ClearGrids()
			LoadAvailableEmployeesForPayroll()
			LoadEmployeeDetailData()

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
			lueYear.Properties.DropDownRows = m_Years.Count

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
			If year Is Nothing Then year = m_InitializationData.MDData.MDYear

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
			lueMonth.Properties.DropDownRows = m_Month.Count

			lueMonth.Properties.ForceInitialize()

			Return success
		End Function

		''' <summary>
		''' Loads the available employees for payroll.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAvailableEmployeesForPayroll() As Boolean
			Dim success As Boolean = True
			Dim createdMissingReports As Boolean = True

			If m_SearchMonth > Now.Month And m_SearchYear >= Now.Year Then
				Dim msg As String
				msg = String.Format(m_Translate.GetSafeTranslationValue("Möchten Sie die fehlenden Rapporte für {0}/{1} erstellen?"), m_SearchMonth, m_SearchYear)

				createdMissingReports = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Fehlende Rapporte"))

			End If
			If createdMissingReports Then success = success AndAlso CreateNewReport()
			If Not success Then Return False

			m_AvailableEmloyees = m_PayrollDatabaseAccess.LoadAvailableEmployeesForPayroll(m_SearchMandantNumber, m_SearchMonth, m_SearchYear)
			bsiEmployeeCount.Caption = String.Empty

			If m_AvailableEmloyees Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Mitarbeiterdaten für die Lohnbuchhaltung konnten nicht geladen werden."))
			ElseIf tgsSelection.EditValue Then
				m_AvailableEmloyees = m_AvailableEmloyees.Where(Function(data) Not data.IsEmployeeInvalidForPayroll).ToList()
			Else
				m_AvailableEmloyees = m_AvailableEmloyees.Where(Function(data) Not data.Is_Current_LO_Existing).ToList()

			End If

			grdAvailableEmployees.DataSource = m_AvailableEmloyees

			bsiEmployeeCount.Caption = m_AvailableEmloyees.Count

			Return Not m_AvailableEmloyees Is Nothing

		End Function

		''' <summary>
		''' Loads employee detail data.
		''' </summary>
		''' <returns>Boolean truth value indicating success.</returns>
		Private Function LoadEmployeeDetailData() As Boolean

			Dim selectedEmployee = SelectedEmployeeInAvailableEmployeesGrid
			bsiDetailCount.Caption = String.Empty

			If selectedEmployee Is Nothing Then
				grdEmployeeDetail.DataSource = Nothing
				grdEmployeeDetail.RefreshDataSource()
				Return False
			End If

			Dim detailData = m_PayrollDatabaseAccess.LoadEmplyoeeDetailDataForPayroll(selectedEmployee.MANr, m_SearchMandantNumber, m_SearchMonth, m_SearchYear, m_InitializationData.UserData.UserLanguage)

			If detailData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Detaildaten konnten nicht geladen werden."))
			End If

			grdEmployeeDetail.DataSource = detailData
			bsiDetailCount.Caption = detailData.Count

			Return Not detailData Is Nothing
		End Function

		''' <summary>
		''' Clears the grids.
		''' </summary>
		Private Sub ClearGrids()

			m_AvailableEmloyees = New List(Of DataObjects.EmployeeDataForPayroll)
			grdAvailableEmployees.DataSource = m_AvailableEmloyees

			m_ChoosenEmployees = New List(Of DataObjects.EmployeeDataForPayroll)
			grdChoosenEmployees.DataSource = m_ChoosenEmployees

			grdEmployeeDetail.DataSource = New List(Of DataObjects.EmployeeDetailDataForPayroll)

			bbiPrint.Enabled = False

		End Sub

#End Region

#Region "Load, save Data"

		''' <summary>
		''' Validates the data on the form.
		''' </summary>
		Private Function ValidateData() As Boolean

			Dim valid As Boolean = True
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Try

			Catch ex As Exception
				valid = False
			End Try

			Return valid

		End Function

#End Region

#Region "Event Handles"

		''' <summary>
		''' Handle click on create pay rolls buttons.
		''' </summary>
		Private Sub OnBtnCreatePayrolls_Click(sender As Object, e As EventArgs) Handles btnCreatePayrolls.Click
			StartProcessEmployeesAsync()
		End Sub

		''' <summary>
		''' Starts employee payroll processing async.
		''' </summary>
		Private Sub StartProcessEmployeesAsync()

			IsProcessing = True

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			Task(Of Boolean).Factory.StartNew(Function() ProcessEmployeesForPayroll(),
																						 CancellationToken.None,
																						 TaskCreationOptions.None,
																						 TaskScheduler.Default).ContinueWith(Sub(t) FinishProcessEmployees(t), CancellationToken.None, TaskContinuationOptions.None, uiSynchronizationContext)


		End Sub

		''' <summary>
		''' Sets control state for processing state.
		''' </summary>
		Private Sub SetControlsStateForProcessingState()

			Dim isNotProcessing = Not IsProcessing

			lueMandant.Enabled = isNotProcessing
			lueYear.Enabled = isNotProcessing
			lueMonth.Enabled = isNotProcessing

			grdAvailableEmployees.Enabled = isNotProcessing
			grdEmployeeDetail.Enabled = isNotProcessing

			btnSearch.Enabled = isNotProcessing
			btnSelectEmployee.Enabled = isNotProcessing
			btnSelectAllEmployee.Enabled = isNotProcessing
			btnUnselectEmployee.Enabled = isNotProcessing
			btnUnselectAllEmployees.Enabled = isNotProcessing
			btnCreatePayrolls.Enabled = isNotProcessing

			tgsSelection.Enabled = isNotProcessing
			chkSetEmployeeLOBackSetting.Enabled = isNotProcessing
			chkEmployeeWithQSTFirst.Enabled = isNotProcessing

		End Sub

		''' <summary>
		''' Processes employees for payroll.
		''' </summary>
		''' <returns>Boolean flag indicating sucess.</returns>
		Private Function ProcessEmployeesForPayroll() As Boolean

			Dim success As Boolean = True
			Dim DONotShowQSTForm As Boolean?

			m_ResultOfLastLORun = Nothing
			'Dim loRunResult As New LORunResult With {.CompletelySuccesful = True}
			m_loRunResult = New LORunResult(m_InitializationData) With {.CompletelySuccesful = True}
			m_SuccessfullCreatedLONr = New List(Of Integer)

			Dim commonData = New EmployeePayroll_CommonData(m_SearchMandantNumber, m_InitializationData)
			commonData.LoadLAData(m_SearchYear)
			commonData.LoadLABezData()
			Dim allowedZeroWorkdaysWithSocialLA = AllowedSocialLAWithoutEmployment(m_SearchYear)

			'm_PayrollDatabaseAccess.CleanupAllInvalidLO()

			Dim employeesToProcess = m_ChoosenEmployees.Where(Function(data) data.EmployeeLOProcessState = EmployeeLOProcessState.Unprocessed)
			m_Logger.LogDebug("1. Phase: employeesToProcess")

			m_loRunResult.AppendProtocolText(Now & vbTab & m_Translate.GetSafeTranslationValue("Task gestartet"))
			m_loRunResult.AppendProtocolText("")
			m_loRunResult.SucesfullyProcessedEmployees.Clear()
			m_loRunResult.SucesfullyProcessedPayrolls.Clear()

			For Each employee In employeesToProcess
				m_Logger.LogDebug("Phase: FocusChoosenEmployee- entring for employee in employeesToProcess")

				Dim payrollCreateService = New EmployeePayroll(m_SearchMandantNumber, m_InitializationData, commonData, m_TaskHelper, DONotShowQSTForm.GetValueOrDefault(False))
				payrollCreateService.AllowedZeroWorkdaysWithSocialLA = allowedZeroWorkdaysWithSocialLA
				m_Logger.LogDebug(String.Format("Phase: payrollCreateService: {0}", employee.EmployeeLOProcessState))

				' TODO: deactivate 
				employee.EmployeeLOProcessState = EmployeeLOProcessState.InProcessing
				RefreshGrids()
				' TODO: activate
				'm_TaskHelper.InUIAndWait(Function()
				'													 employee.EmployeeLOProcessState = EmployeeLOProcessState.InProcessing
				'													 RefreshGrids()
				'													 FocusChoosenEmployee(employee.MANr)
				'													 Return True
				'												 End Function)

				m_Logger.LogDebug("Phase: InUIAndWait- payrollCreateService")

				Try
					success = payrollCreateService.CreatePayRollForEmployee(employee.MANr, m_SearchMandantNumber, m_SearchMonth, m_SearchYear, chkSetEmployeeLOBackSetting.Checked)
					m_Logger.LogDebug(String.Format("Phase: payrollCreateService.CreatePayRollForEmployee: {0}", success))

					'If Not DONotShowQSTForm.HasValue Then DONotShowQSTForm = Not payrollCreateService.ShowAgainQSTForm
					DONotShowQSTForm = Not payrollCreateService.ShowAgainQSTForm


					If success Then
						employee.LONr = payrollCreateService.LONewNr

						m_loRunResult.SucesfullyProcessedEmployees.Add(employee.MANr)
						m_loRunResult.SucesfullyProcessedPayrolls.Add(employee.LONr)

						m_SuccessfullCreatedLONr.Add(employee.LONr)

						If Not m_FirstCreatedLONr.HasValue Then m_FirstCreatedLONr = employee.LONr
					End If

					m_loRunResult.CompletelySuccesful = m_loRunResult.CompletelySuccesful And success
					m_loRunResult.AppendProtocolText(payrollCreateService.ProtocolText)

				Catch ex As Exception
					m_loRunResult.CompletelySuccesful = False
					m_loRunResult.AppendProtocolText(payrollCreateService.ProtocolText)
					employee.EmployeeLOProcessState = EmployeeLOProcessState.Failed

				End Try
				Trace.WriteLine(String.Format("Länge des Protokolls: {0}", Len(m_loRunResult.ProtocolText)))

				' TODO: deactivate 
				employee.EmployeeLOProcessState = If(success, EmployeeLOProcessState.Processed, EmployeeLOProcessState.Failed)
				RefreshGrids()
				' TODO: activate
				'm_TaskHelper.InUIAndWait(Function()
				'													 employee.EmployeeLOProcessState = If(success, EmployeeLOProcessState.Processed, EmployeeLOProcessState.Failed)
				'													 RefreshGrids()
				'													 FocusChoosenEmployee(employee.MANr)
				'													 Return True
				'												 End Function)
			Next

			m_loRunResult.AppendProtocolEnd()

			m_ResultOfLastLORun = m_loRunResult

			Return True
		End Function

		''' <summary>
		''' Finished employee payroll processing.
		''' </summary>
		Private Sub FinishProcessEmployees(ByVal result As Task(Of Boolean))

			WriteProtocolForLastLORun(Nothing)
			ShowPayrollResultData()

			bbiPrint.Enabled = m_SuccessfullCreatedLONr.Count > 0

			IsProcessing = False

			If CheckOpenPrintFormAutomaticaly Then
				'#If release Then
				OpenLOPrintForm()
				'#End If

			End If

		End Sub

		Private Sub OpenLOPrintForm()

			Dim liLONr As New List(Of Integer)
			If m_SuccessfullCreatedLONr.Count = 0 Then Return

			liLONr = m_SuccessfullCreatedLONr

			Try
				Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.LogedUSNr = m_InitializationData.UserData.UserNr, .SelectedMDNr = m_SearchMandantNumber,
					.SelectedMANr = New List(Of Integer)(New Integer() {0}), .SelectedLONr = liLONr, .SelectedMonth = New List(Of Integer)(New Integer() {m_SearchMonth}),
					.SelectedYear = New List(Of Integer)(New Integer() {m_SearchYear}), .SearchAutomatic = True}
				'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(m_InitializationData, _settring)
				'obj.ShowfrmLO4Print()

				Dim obj As New SP.LO.PrintUtility.frmLOPrint(m_InitializationData)
				obj.LOSetting = _settring

				obj.Show()
				obj.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		''' <summary>
		''' Shows the ZV Form for the last LO run.
		''' </summary>
		Private Sub ShowPayrollResultData()

			If Not m_ResultOfLastLORun Is Nothing Then

				If m_SuccessfullCreatedLONr.Count > 0 OrElse m_ResultOfLastLORun.SucesfullyProcessedEmployees.Count > 0 Then

					Dim frmZVMA As New frmZVMA(m_SearchMandantNumber, m_SearchYear, m_SearchMonth, m_InitializationData)

					Dim success As Boolean = frmZVMA.LoadData(m_SuccessfullCreatedLONr.ToArray(), m_ResultOfLastLORun.SucesfullyProcessedEmployees.ToArray())

					If success Then
						'If frmZVMA.gvEmployees.RowCount > 0 Then
						frmZVMA.Show()
						frmZVMA.BringToFront()
					Else
						frmZVMA.Close()

						'End If

					End If

				End If

			End If

		End Sub

		''' <summary>
		''' Writes LO protocol for last LO run.
		''' </summary>
		Private Sub WriteProtocolForLastLORun(ByVal employeeNumber As Integer?)
			Dim success As Boolean = m_PayrollDatabaseAccess.AddLOProtocolData(New LOProtocolData With {.MANr = employeeNumber,
																			   .MDNr = m_SearchMandantNumber,
																			   .LP = m_SearchMonth,
																			   .Jahr = m_SearchYear,
																			   .Protokoll = m_ResultOfLastLORun.ProtocolText,
																			   .DebugValue = String.Empty,
																			   .CreatedOn = DateTime.Now
																			   })

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Protokoll für Lohnlauf konnte nicht erstellt werden."))
			End If

		End Sub

		''' <summary>
		''' Writes LO protocol for last LO run.
		''' </summary>
		Private Sub LoadProtocol(ByVal employeeNumber As Integer?)
			Dim protocoldata = m_PayrollDatabaseAccess.LoadLOProtocol(m_SearchMandantNumber, employeeNumber, m_SearchMonth, m_SearchYear)

			If protocoldata Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Protokoll für Lohnlauf konnte nicht erstellt werden."))
			End If

			Try

				Dim tempFileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), String.Format("Protokoll Lohnlauf ({0:dd_MM_yyyy hh_mm}).txt", DateTime.Now))

				Dim file As System.IO.StreamWriter
				file = My.Computer.FileSystem.OpenTextFileWriter(tempFileName, False)
				file.WriteLine(protocoldata.Protokoll)
				file.Close()


				Process.Start(tempFileName)
			Catch ex As Exception
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Protokoll konnte nicht angezeigt werden."))
			End Try

		End Sub

		''' <summary>
		''' Handles edit change event of lueMandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ClearGrids()

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
				m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
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

			ClearGrids()

			LoadMonthDropDownData()

		End Sub

		''' <summary>
		''' Handles edit change event of lueMonth.
		''' </summary>
		Private Sub OnLueMonth_EditValueChanged(sender As Object, e As EventArgs) Handles lueMonth.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ClearGrids()

			Search()

		End Sub

		''' <summary>
		''' Handles checked change event of invalid employees checkbox.
		''' </summary>
		Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled
			'Private Sub OnChkHideInvalidEmployees_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideInvalidEmployees.CheckedChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ClearGrids()

			LoadAvailableEmployeesForPayroll()
			LoadEmployeeDetailData()

		End Sub

		''' <summary>
		''' Handles checked change event employees with QST first checkbox..
		''' </summary>
		Private Sub OnChkEmployeeWithQSTFirst_CheckedChanged(sender As Object, e As EventArgs) Handles chkEmployeeWithQSTFirst.CheckedChanged
			RefreshGrids()
		End Sub

		''' <summary>
		''' Handles click on search button.
		''' </summary>
		Private Sub OnBtnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
			Search()
		End Sub

		''' <summary>
		''' Handles click on select employee button.
		''' </summary>
		Private Sub OnBtnSelectEmployee_Click(sender As Object, e As EventArgs) Handles btnSelectEmployee.Click

			Dim employee = SelectedEmployeeInAvailableEmployeesGrid

			If employee Is Nothing OrElse employee.IsEmployeeInvalidForPayroll Then
				Return
			End If

			m_AvailableEmloyees.Remove(employee)
			m_ChoosenEmployees.Add(employee)


			Dim employeeNewinList = SelectedEmployeeInAvailableEmployeesGrid

			RefreshGrids()
			If Not employeeNewinList Is Nothing Then
				FocusAvailableEmployee(employeeNewinList.MANr)
			End If

			LoadEmployeeDetailData()

		End Sub

		''' <summary>
		''' Handles click onunselect employee button.
		''' </summary>
		Private Sub OnBtnUnselectEmployee_Click(sender As Object, e As EventArgs) Handles btnUnselectEmployee.Click

			Dim employee = SelectedEmployeeInChoosenEmployeesGrid

			If employee Is Nothing Then
				Return
			End If

			m_ChoosenEmployees.Remove(employee)
			m_AvailableEmloyees.Add(employee)

			RefreshGrids()
			LoadEmployeeDetailData()

		End Sub

		''' <summary>
		''' Handles click on select all employee button.
		''' </summary>
		Private Sub OnBtnSelectAllEmployees_Click(sender As Object, e As EventArgs) Handles btnSelectAllEmployee.Click

			Dim employeesToMove = m_AvailableEmloyees.Where(Function(data) Not data.IsEmployeeInvalidForPayroll).ToList()
			m_ChoosenEmployees.AddRange(employeesToMove)

			For Each employee In employeesToMove
				m_AvailableEmloyees.Remove(employee)
			Next

			RefreshGrids()
			LoadEmployeeDetailData()
		End Sub

		''' <summary>
		''' Handles click on usselect all employee button.
		''' </summary>
		Private Sub OnBtnUnselectAllEmployees_Click(sender As Object, e As EventArgs) Handles btnUnselectAllEmployees.Click

			m_AvailableEmloyees.AddRange(m_ChoosenEmployees)
			m_ChoosenEmployees.Clear()

			RefreshGrids()
			LoadEmployeeDetailData()

		End Sub

		''' <summary>
		''' Handles row click on available employees grid.
		''' </summary>
		Private Sub OnGvAvailableEmployees_RowClick(sender As Object, e As RowClickEventArgs) Handles gvAvailableEmployees.RowClick
			LoadEmployeeDetailData()
		End Sub

		''' <summary>
		''' Handles keyup on available employees grid.
		''' </summary>
		Private Sub OnGrdAvailableEmployees_KeyUp(sender As Object, e As KeyEventArgs) Handles grdAvailableEmployees.KeyUp
			LoadEmployeeDetailData()
		End Sub

		''' <summary>
		''' Handles row style event of available employee grid.
		''' </summary>
		Private Sub OnGvAvailableEmployees_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvAvailableEmployees.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim rowData = CType(view.GetRow(e.RowHandle), DataObjects.EmployeeDataForPayroll)

				If (rowData.IsEmployeeInvalidForPayroll) Then
					e.Appearance.BackColor = Color.Yellow
				End If

			End If

		End Sub

		''' <summary>
		''' Handles cell click on available employees grid.
		''' </summary>
		Private Sub OnGvAvailableEmployees_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvAvailableEmployees.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvAvailableEmployees.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim employeeData = CType(dataRow, DataObjects.EmployeeDataForPayroll)

					Select Case column.Name.ToLower
						Case "MANr".ToLower, "Nachname_Vorname".ToLower

							' Send a request to open a employeeMng form.
							Dim hub = MessageService.Instance.Hub
							Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_SearchMandantNumber, employeeData.MANr)
							hub.Publish(openEmployeeMng)
						Case Else

							If employeeData.IsEmployeeInvalidForPayroll Then
								ShowInvalidRecordsForm(employeeData.MANr)
							Else
								OnBtnSelectEmployee_Click(Me, New EventArgs())
							End If

					End Select

				End If

			End If

		End Sub

		''' <summary>
		''' Handles cell click on choosen employees grid.
		''' </summary>
		Private Sub OnGvChoosenEmployees_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvChoosenEmployees.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvChoosenEmployees.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim employeeData = CType(dataRow, DataObjects.EmployeeDataForPayroll)

					Select Case column.Name.ToLower
						Case "Status".ToLower
							LoadProtocol(employeeData.MANr)

						Case "lonr".ToLower
							OpenLOPrintForm()

						Case "MANr".ToLower, "Nachname_Vorname".ToLower

							' Send a request to open a employeeMng form.
							Dim hub = MessageService.Instance.Hub
							Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_SearchMandantNumber, employeeData.MANr)
							hub.Publish(openEmployeeMng)
						Case Else

							' do nothing

					End Select

				End If

			End If

		End Sub

		''' <summary>
		''' Handles double click on employee detail grid.
		''' </summary>
		Private Sub OnGvEmployeeDetail_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvEmployeeDetail.DoubleClick
			Dim selectedRows = gvEmployeeDetail.GetSelectedRows()

			If (selectedRows.Count > 0) Then

				If (gvEmployeeDetail.GetRow(selectedRows(0)) Is Nothing) Then Return

				Dim employeeDetailData = CType(gvEmployeeDetail.GetRow(selectedRows(0)), DataObjects.EmployeeDetailDataForPayroll)

				Dim hub = MessageService.Instance.Hub
				Select Case employeeDetailData.ModulName
					Case "R"
						Dim openReportsMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_SearchMandantNumber, employeeDetailData.ModulNr)
						hub.Publish(openReportsMng)
					Case "L"
						Dim openMonthlySalaryMng As New OpenMLohnMngRequest(Me, m_InitializationData.UserData.UserNr, m_SearchMandantNumber, employeeDetailData.MANr, employeeDetailData.ModulNr)
						hub.Publish(openMonthlySalaryMng)
					Case "Z"
						Dim openAdvancePaymentMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, m_SearchMandantNumber, employeeDetailData.ModulNr)
						hub.Publish(openAdvancePaymentMng)
					Case Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Unbekannter Modulname"))
				End Select

			End If
		End Sub

		''' <summary>
		''' Handles unbound column data event of choosen employee grid.
		''' </summary>
		Private Sub OngvChoosenEmployee_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvChoosenEmployees.CustomUnboundColumnData

			If e.Column.Name = "Status" Then
				If (e.IsGetData()) Then
					Dim state = CType(e.Row, EmployeeDataForPayroll).EmployeeLOProcessState

					Select Case state
						Case EmployeeLOProcessState.Unprocessed
							e.Value = m_Unprocessed
						Case EmployeeLOProcessState.InProcessing
							e.Value = m_InProcessing
						Case EmployeeLOProcessState.Processed
							e.Value = m_Processed
						Case EmployeeLOProcessState.Failed
							e.Value = m_Failed
						Case Else
							e.Value = m_Unprocessed
					End Select

				End If
			End If
		End Sub

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmPayroll_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmPayroll_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnFrmPayroll_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

			If IsProcessing Then
				e.Cancel = True
				Return
			End If

			CleanupAndHideForm()
			e.Cancel = True

		End Sub

		''' <summary>
		''' Keypreview for Modul-version
		''' </summary>
		Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
			Dim strRAssembly As String = ""
			Try

				If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
					Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
					For Each a In AppDomain.CurrentDomain.GetAssemblies()
						m_Logger.LogInfo(String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase))
						strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)

					Next
					strMsg = String.Format(strMsg, vbNewLine, GetExecutingAssembly().FullName, GetExecutingAssembly().Location, strRAssembly)
					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If

			Catch ex As Exception

			End Try

		End Sub

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is BaseEdit Then
					If CType(sender, BaseEdit).Properties.ReadOnly Then
						' nothing
					Else
						CType(sender, BaseEdit).EditValue = Nothing
					End If
				End If
			End If

		End Sub

		''' <summary>
		''' handels click on bbi print button.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub OnbbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

			OpenLOPrintForm()

		End Sub

		''' <summary>
		''' Handles click on bar button missing ZV.
		''' </summary>
		Private Sub OnbbiMissingZV_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiMissingZV.ItemClick

			Dim frmZV = New SP.MA.PayrollMng.UI.frmForgottenZVARGB(m_InitializationData)

			frmZV.LoadData()
			frmZV.Show()
			frmZV.BringToFront()


			'Dim frmZVMA As New frmZVMA(m_SearchMandantNumber, m_SearchYear, m_SearchMonth, m_InitializationData)
			'frmZVMA.LoadData(Nothing, Nothing)

			'frmZVMA.Show()
			'frmZVMA.BringToFront()

		End Sub

		''' <summary>
		''' handels printing grid for Available Employees
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub OngrpAvailableEmployees_CustomButtonClick(sender As Object, e As BaseButtonEventArgs) Handles grpAvailableEmployees.CustomButtonClick

			If Not grdAvailableEmployees.IsPrintingAvailable Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Drucken ist nicht möglich. Bitte kontaktieren Sie Ihr Softwarehersteller."))
				Return
			End If

			' Opens the Preview window. 
			grdAvailableEmployees.ShowPrintPreview()

		End Sub

		''' <summary>
		''' handels printing grid for created payrolls
		''' </summary>
		Private Sub OngrpSelectedEmployees_CustomButtonClick(sender As Object, e As BaseButtonEventArgs) Handles grpSelectedEmployees.CustomButtonClick

			If Not grdChoosenEmployees.IsPrintingAvailable Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Drucken ist nicht möglich. Bitte kontaktieren Sie Ihr Softwarehersteller."))
				Return
			End If

			' Opens the Preview window. 
			grdChoosenEmployees.ShowPrintPreview()

		End Sub


#End Region

#Region "Helper Methods"

		''' <summary>
		'''  Trannslate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
			Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
			Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
			Me.grpAvailableEmployees.Text = m_Translate.GetSafeTranslationValue(Me.grpAvailableEmployees.Text)
			Me.grpSelectedEmployees.Text = m_Translate.GetSafeTranslationValue(Me.grpSelectedEmployees.Text)
			Me.grpEmployeeDetail.Text = m_Translate.GetSafeTranslationValue(Me.grpEmployeeDetail.Text)
			Me.btnSearch.Text = m_Translate.GetSafeTranslationValue(Me.btnSearch.Text)
			tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OffText)
			tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OnText)
			Me.chkSetEmployeeLOBackSetting.Text = m_Translate.GetSafeTranslationValue(Me.chkSetEmployeeLOBackSetting.Text)

			bsiLblEmployeeCount.Caption = m_Translate.GetSafeTranslationValue(bsiLblEmployeeCount.Caption)
			bsiLblDetailCount.Caption = m_Translate.GetSafeTranslationValue(bsiLblDetailCount.Caption)

			bbiMissingZV.Caption = m_Translate.GetSafeTranslationValue(bbiMissingZV.Caption)
			bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)

		End Sub

		''' <summary>
		''' Refreshes the grids.
		''' </summary>
		Private Sub RefreshGrids()

			If Me.InvokeRequired = True Then
				Me.Invoke(New StartLoadingData(AddressOf RefreshGrids))
			Else

				m_Logger.LogDebug(String.Format("Phase (RefreshGrids): sorting m_AvailableEmloyees: {0}", m_AvailableEmloyees.Count))
				m_AvailableEmloyees = m_AvailableEmloyees.OrderBy(Function(data) data.Nachname_Vorname).ToList()
				m_Logger.LogDebug("Phase (RefreshGrids): m_AvailableEmloyees")

				If chkEmployeeWithQSTFirst.Checked Then

					Dim employeesWithQST = m_ChoosenEmployees.Where(Function(data) Not data.Q_Steuer = "0").ToList()
					Dim employeesNoQST = m_ChoosenEmployees.Except(employeesWithQST).ToList()

					Dim listWithQSTEmployeesFirst As New List(Of EmployeeDataForPayroll)
					listWithQSTEmployeesFirst.AddRange(employeesWithQST.OrderBy(Function(data) data.Nachname_Vorname).ToList())
					listWithQSTEmployeesFirst.AddRange(employeesNoQST.OrderBy(Function(data) data.Nachname_Vorname).ToList())

					m_ChoosenEmployees = listWithQSTEmployeesFirst

				Else
					m_ChoosenEmployees = m_ChoosenEmployees.OrderBy(Function(data) data.Nachname_Vorname).ToList()
				End If
				m_Logger.LogDebug("Phase (RefreshGrids): endif")

				grdAvailableEmployees.DataSource = m_AvailableEmloyees
				grdChoosenEmployees.DataSource = m_ChoosenEmployees

				grdAvailableEmployees.RefreshDataSource()
				grdChoosenEmployees.RefreshDataSource()
				grdEmployeeDetail.RefreshDataSource()

			End If

		End Sub

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
		''' Sets the valid state of a control.
		''' </summary>
		''' <param name="control">The control to validate.</param>
		''' <param name="errorProvider">The error providor.</param>
		''' <param name="invalid">Boolean flag if data is invalid.</param>
		''' <param name="errorText">The error text.</param>
		''' <returns>Valid flag</returns>
		Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText)
			Else
				errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Private Sub CleanupAndHideForm()

			SaveFromSettings()

			CloseInvalidRecordNumbersForm()

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub

		''' <summary>
		''' Closes invalid record numbers form.
		''' </summary>
		Private Sub CloseInvalidRecordNumbersForm()

			If Not m_FrmInvalidRecordNumbers Is Nothing AndAlso
							 Not m_FrmInvalidRecordNumbers.IsDisposed Then
				Try
					m_FrmInvalidRecordNumbers.Close()
					m_FrmInvalidRecordNumbers.Dispose()
				Catch
					' Do nothing
				End Try
			End If

		End Sub

		''' <summary>
		''' Shows the invalid records form.
		''' </summary>
		''' <param name="maNr">The employee number.</param>
		Private Sub ShowInvalidRecordsForm(ByVal maNr As Integer)

			If m_FrmInvalidRecordNumbers Is Nothing OrElse m_FrmInvalidRecordNumbers.IsDisposed Then
				m_FrmInvalidRecordNumbers = New frmInvalidRecordNumbers(m_SearchMandantNumber, m_InitializationData)
				m_FrmInvalidRecordNumbers.StartPosition = FormStartPosition.Manual


				m_FrmInvalidRecordNumbers.Location = New System.Drawing.Point(Me.Location.X + (Me.Width - m_FrmInvalidRecordNumbers.Width) / 2,
																																																																							Me.Location.Y + (Me.Height - m_FrmInvalidRecordNumbers.Height) / 2)


			End If

			m_FrmInvalidRecordNumbers.LoadData(m_SearchMandantNumber, maNr, m_SearchMonth, m_SearchYear)
			m_FrmInvalidRecordNumbers.Show()
			m_FrmInvalidRecordNumbers.BringToFront()

		End Sub


		''' <summary>
		''' Focuses an employee in the choosen employees list.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		Private Sub FocusChoosenEmployee(ByVal employeeNumber As Integer)

			m_Logger.LogDebug("Phase: FocusChoosenEmployee- Entring")

			If Not gvChoosenEmployees.DataSource Is Nothing Then

				Dim employeeList = CType(gvChoosenEmployees.DataSource, List(Of DataObjects.EmployeeDataForPayroll))

				Dim index = employeeList.ToList().FindIndex(Function(data) data.MANr = employeeNumber)

				m_SuppressUIEvents = True
				Dim rowHandle = gvChoosenEmployees.GetRowHandle(index)
				gvChoosenEmployees.FocusedRowHandle = rowHandle
				m_SuppressUIEvents = False
			End If
			m_Logger.LogDebug("Phase: FocusChoosenEmployee- ending")

		End Sub

		''' <summary>
		''' Focuses an employee in the available employees list.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		Private Sub FocusAvailableEmployee(ByVal employeeNumber As Integer)

			If Not gvAvailableEmployees.DataSource Is Nothing Then

				Dim employeeList = CType(gvAvailableEmployees.DataSource, List(Of DataObjects.EmployeeDataForPayroll))

				Dim index = employeeList.ToList().FindIndex(Function(data) data.MANr = employeeNumber)

				m_SuppressUIEvents = True
				Dim rowHandle = gvAvailableEmployees.GetRowHandle(index)
				gvAvailableEmployees.FocusedRowHandle = rowHandle
				m_SuppressUIEvents = False
			End If

		End Sub

		Private Sub PublishPayrollProcessingState()

			' Send a request to open a employeeMng form.
			Dim hub = MessageService.Instance.Hub
			Dim payrollProcessingStateChange As New PayrollProcessingStateHasChanged(Me, m_InitializationData.UserData.UserNr, m_SearchMandantNumber, IsProcessing)
			hub.Publish(payrollProcessingStateChange)

		End Sub

		Private Function CreateNewReport() As Boolean

			Dim esData = LoadESOverviewData()

			If esData Is Nothing Then
				Return False
			ElseIf esData.Count = 0 Then
				Return True
			End If

			Dim RPNumberOffset = ReadReportOffsetFromSettings()
			Dim firstDayOfMonth = New DateTime(lueYear.EditValue, lueMonth.EditValue, 1)
			Dim lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1)
			Dim success As Boolean = True
			Dim reportNumbers As List(Of Integer) = Nothing

			reportNumbers = New List(Of Integer)

			For Each rec In esData
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
				iniData.MDNr = m_SearchMandantNumber
				iniData.RPNumberOffset = RPNumberOffset
				iniData.CreatedFrom = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_ReportDatabaseAccess.AddNewRPForExistingES(iniData)
				If success Then
					reportNumbers.Add(iniData.NewRPNrOutput)
				End If

			Next

			Dim sMsgText = String.Format(m_Translate.GetSafeTranslationValue("<b>folgende Rapporte wurden neu erstellt:</b> {0}"), String.Join(", ", reportNumbers))
			m_loRunResult.AppendProtocolText(sMsgText)


			Return success

		End Function

		Private Function LoadESOverviewData() As IEnumerable(Of SP.DatabaseAccess.Report.DataObjects.ESData)

			Dim esOverviewData = m_ReportDatabaseAccess.LoadESDataForCreateingReport(m_SearchMandantNumber, lueYear.EditValue, lueMonth.EditValue)

			If esOverviewData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatz-Daten konnten nicht geladen werden."))
			End If

			Return esOverviewData

		End Function

		Private Function ReadReportOffsetFromSettings() As Integer

			Dim strQuery As String = "//StartNr/Rapporte"

			Dim esNumberStartNumberSetting As String = m_md.GetSelectedMDProfilValue(m_SearchMandantNumber, lueYear.EditValue, "StartNr", "Rapporte", 0)
			Dim intVal As Integer

			If Integer.TryParse(esNumberStartNumberSetting, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function

		Private Shared Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly
			'This handler is called only when the common language runtime tries to bind to the assembly and fails.        
			m_Logger = New Logger

			'Retrieve the list of referenced assemblies in an array of AssemblyName.
			Dim objExecutingAssemblies As [Assembly]
			objExecutingAssemblies = [Assembly].GetExecutingAssembly()
			Dim arrReferencedAssmbNames() As AssemblyName
			arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies()

			'Loop through the array of referenced assembly names.
			Dim strAssmbName As AssemblyName
			For Each strAssmbName In arrReferencedAssmbNames

				'Look for the assembly names that have raised the "AssemblyResolve" event.
				If (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) = args.Name.Substring(0, args.Name.IndexOf(","))) Then

					'Build the path of the assembly from where it has to be loaded.
					Dim strTempAssmbPath As String = String.Empty
					strTempAssmbPath = IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")

					If IO.File.Exists(strTempAssmbPath) Then
						Dim msg = String.Format("loading Assembly: {0}", strTempAssmbPath)
						m_Logger.LogWarning(msg)
						'Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
						Dim MyAssembly As [Assembly]

						'Load the assembly from the specified path. 
						MyAssembly = [Assembly].LoadFrom(strTempAssmbPath)

						'Return the loaded assembly.
						Return MyAssembly
					Else
						Dim msg = String.Format("Assembly could not be found: {0}", strTempAssmbPath)
						m_Logger.LogWarning(msg)
						Trace.WriteLine(msg)
					End If

				End If
			Next

			Return Nothing

		End Function

#End Region

#End Region

#Region "Helper Classes"

		''' <summary>
		''' Wraps an integer value.
		''' </summary>
		Class IntegerValueViewWrapper
			Public Property Value As Integer
		End Class

		Class LORunResult

#Region "Private Fields"

			''' <summary>
			''' The Initialization data.
			''' </summary>
			Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

			''' <summary>
			''' The translation value helper.
			''' </summary>
			Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

			Private m_Protocol As New StringBuilder

#End Region

#Region "Constructor"

			Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

				m_Protocol.AppendLine(m_Translate.GetSafeTranslationValue("<br><b>Protokoll für Lohnlauf:</b>"))
				m_Protocol.AppendLine("<br>")
				m_Protocol.AppendLine(m_Translate.GetSafeTranslationValue("<br>Dieses Protokoll enthält alle wichtigen Meldungen & Fehler welche während der Erstellung des Lohnlaufes unterdrückt wurden."))
				m_Protocol.AppendLine(m_Translate.GetSafeTranslationValue("<br>Bitte lesen Sie das Protokoll sorgfälltig durch und überprüfen Sie die Meldungen & Fehler."))
				m_Protocol.AppendLine("<br>")
				m_Protocol.AppendLine(String.Format("<br>{0}", New String("-", 50)))
				m_Protocol.AppendLine(m_Translate.GetSafeTranslationValue("<br>Legende:"))
				m_Protocol.AppendLine(m_Translate.GetSafeTranslationValue("<br>M = Meldung"))
				m_Protocol.AppendLine(m_Translate.GetSafeTranslationValue("<br>*** = Wichtiger Fehler"))
				m_Protocol.AppendLine(Padright("Exit =", 8, " ") & m_Translate.GetSafeTranslationValue("<br>Die Funktion wurde übersprungen"))
				m_Protocol.AppendLine(String.Format("<br>{0}", New String("-", 50)))
				m_Protocol.AppendLine("<br>")

			End Sub

#End Region

#Region "Public Properties"

			Public Property CompletelySuccesful As Boolean

			Public Property SucesfullyProcessedPayrolls As New List(Of Integer)
			Public Property SucesfullyProcessedEmployees As New List(Of Integer)

			Public ReadOnly Property ProtocolText
				Get
					Return m_Protocol.ToString()
				End Get
			End Property

#End Region

#Region "Public Methods"

			Public Sub AppendProtocolText(ByVal text)
				m_Protocol.AppendLine("<br>" & text)
			End Sub

			Public Sub AppendProtocolEnd()
				m_Protocol.AppendLine("<br>")

				m_Protocol.AppendLine("<br>" & Now & vbTab & m_Translate.GetSafeTranslationValue("Task abgeschlossen."))
				m_Protocol.AppendLine("<br>")
			End Sub

#End Region

#Region "Private Methods"

			Private Function Padright(ByVal Tempwert As Object,
									 ByVal StrAfter As Integer,
									 ByVal FillStr As String) As String

				Padright = Tempwert & New String(FillStr, StrAfter - Math.Min(Len(Tempwert), StrAfter))

			End Function

#End Region

		End Class


#End Region


	End Class


End Namespace
