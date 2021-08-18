
Option Strict Off

Imports SPS.Listing.Print.Utility.PrintReport
Imports System.Data.SqlClient
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPRPListSearch.ClsDataDetail
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.ComponentModel
Imports SPSSendMail.RichEditSendMail
Imports DevExpress.XtraSplashScreen

Public Class frmRPListSearch
	Inherits DevExpress.XtraEditors.XtraForm


	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility
	Private m_utility_SP As Utilities

	Public Shared frmMyLV As frmRPListSearch_LV

	''' <summary>
	''' The mandant.
	''' </summary>
	''' <remarks></remarks>
	Private m_Mandant As Mandant

	Private m_path As ClsProgPath

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The current connection string.
	''' </summary>
	Private m_CurrentConnectionString = String.Empty

	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_ReportDatabaseAccess As IReportDatabaseAccess

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean
	Private m_SearchCriteria As New SearchCriteria

	Private m_EmployeeData As IEnumerable(Of EmployeeData)
	Private m_CustomerData As IEnumerable(Of CustomerData)
	Private m_EmploymentData As IEnumerable(Of EmploymentData)
	Private m_ReportData As IEnumerable(Of ReportData)

#Region "Constructor..."

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		' Mandantendaten
		m_Mandant = New Mandant
		m_path = New ClsProgPath

		m_InitializationData = _setting
		m_InitialData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		ClsDataDetail.m_Translate = m_Translate


		m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn

		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ReportDatabaseAccess = New ReportDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try

		Reset()

		ResetMandantenDropDown()
		LoadMandantenDropDown()


		TranslateControls()

		AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployment.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueReport.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region


#Region "public properties"
	Public Property AssignedYear As Integer?
	Public Property AssignedMonth As Integer?
	Public Property EmployeeNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property EmploymentNumber As Integer?
	Public Property ReportNumber As Integer?

#End Region


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		m_SuppressUIEvents = True

		Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
		CboSort.Properties.DataSource = LoadSortData()

		If AssignedYear Is Nothing Then
			Cbo_Year.EditValue = Now.Year
			Cbo_Month.EditValue = If(Now.Day > 10, Now.Month, Now.Month - 1)

		Else
			Cbo_Year.EditValue = If(AssignedYear Is Nothing, Now.Year, AssignedYear)
			Cbo_Month.EditValue = If(AssignedMonth Is Nothing, Now.Month, AssignedMonth)

		End If

		LoadEmployeeDropDownData()
		lueEmployee.EditValue = EmployeeNumber

		LoadCustomerDropDownData()
		lueCustomer.EditValue = CustomerNumber

		LoadEmploymentDropDownData()
		lueEmployment.EditValue = EmploymentNumber

		LoadReportDropDownData()
		lueReport.EditValue = ReportNumber

		LoadAdvisorValues()
		ValidateInputOfWeeks()

		m_SuppressUIEvents = False

		Return result

	End Function

#End Region

	Private ReadOnly Property SelectedEmploymentData As EmploymentData
		Get
			Dim SelectedData = TryCast(lueEmployment.GetSelectedDataRow(), EmploymentData)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedReportData As ReportData
		Get
			Dim SelectedData = TryCast(lueReport.GetSelectedDataRow(), ReportData)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
																					 .Width = 100,
																					 .Caption = "Mandant"})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If m_SuppressUIEvents Then Return

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)

			m_InitializationData = ChangeMandantData
			m_InitialData = ChangeMandantData

			m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn

			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_ReportDatabaseAccess = New ReportDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub

#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(xtabAllgemein.Text)
		xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(xtabSQLAbfrage.Text)

		lblHeader1.Text = m_Translate.GetSafeTranslationValue(lblHeader1.Text)
		lblHeader2.Text = m_Translate.GetSafeTranslationValue(lblHeader2.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lblsortierung.Text = m_Translate.GetSafeTranslationValue(lblsortierung.Text)
		lblJahr.Text = m_Translate.GetSafeTranslationValue(lblJahr.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)

		lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(lblMitarbeiter.Text)
		lblFirma.Text = m_Translate.GetSafeTranslationValue(lblFirma.Text)
		lblEinsatz.Text = m_Translate.GetSafeTranslationValue(lblEinsatz.Text)
		lblRapport.Text = m_Translate.GetSafeTranslationValue(lblRapport.Text)

		lbl1KST.Text = m_Translate.GetSafeTranslationValue(lbl1KST.Text)
		lbl2KST.Text = m_Translate.GetSafeTranslationValue(lbl2KST.Text)
		lblBerater.Text = m_Translate.GetSafeTranslationValue(lblBerater.Text)
		lblFiliale.Text = m_Translate.GetSafeTranslationValue(lblFiliale.Text)

		lblAktuellQery.Text = m_Translate.GetSafeTranslationValue(lblAktuellQery.Text)

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
		bbiSearch.Caption = m_Translate.GetSafeTranslationValue(bbiSearch.Caption)
		bbiClear.Caption = m_Translate.GetSafeTranslationValue(bbiClear.Caption)
		bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)
		bbiExport.Caption = m_Translate.GetSafeTranslationValue(bbiExport.Caption)

	End Sub

	Private Sub Reset()
		Dim visibleMandantSelect As Boolean

		visibleMandantSelect = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
		Me.lueMandant.Visible = visibleMandantSelect
		lblMDName.Visible = visibleMandantSelect

		Cbo_Month.EditValue = Nothing
		Cbo_Year.EditValue = Nothing
		Cbo_KST1.EditValue = Nothing
		Cbo_KST2.EditValue = Nothing
		Cbo_Berater.EditValue = Nothing
		Cbo_Berater.Properties.Items.Clear()
		Cbo_Filiale.EditValue = Nothing

		ResetSortDropDown()

		ResetEmployeeDropDown()
		m_SuppressUIEvents = True

		ResetCustomerDropDown()
		ResetEmploymentDropDown()
		ResetReportDropDown()

		m_SuppressUIEvents = False

	End Sub


	Private Sub ResetSortDropDown()

		CboSort.Properties.DisplayMember = "BezValue"
		CboSort.Properties.ValueMember = "BezValue"

		CboSort.Properties.NullText = String.Empty
		CboSort.EditValue = Nothing

	End Sub

	Private Sub LoadStaticData()

		If Cbo_Month.EditValue Is Nothing Then Cbo_Month.EditValue = Now.Month
		If Cbo_Year.EditValue Is Nothing Then Cbo_Year.EditValue = Now.Year

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		LoadEmployeeDropDownData()
		LoadCustomerDropDownData()
		LoadEmploymentDropDownData()
		LoadReportDropDownData()

		Try

			If Not EmployeeNumber Is Nothing Then
				If Not m_EmployeeData Is Nothing Then
					Dim data = m_EmployeeData.Where(Function(x) x.EmployeeNumber = EmployeeNumber).FirstOrDefault
					If data Is Nothing Then
						lueEmployee.EditValue = Nothing
					Else
						lueEmployee.EditValue = EmployeeNumber
					End If
				End If
			End If

			If Not CustomerNumber Is Nothing Then
				If Not m_CustomerData Is Nothing Then
					Dim data = m_CustomerData.Where(Function(x) x.CustomerNumber = CustomerNumber).FirstOrDefault
					If data Is Nothing Then
						lueCustomer.EditValue = Nothing
					Else
						lueCustomer.EditValue = CustomerNumber
					End If
				End If
			End If

			If Not EmploymentNumber Is Nothing Then
				If Not m_EmploymentData Is Nothing Then
					Dim data = m_EmploymentData.Where(Function(x) x.EmploymentNumber = EmploymentNumber).FirstOrDefault
					If data Is Nothing Then
						lueEmployment.EditValue = Nothing
					Else
						lueEmployment.EditValue = EmploymentNumber
					End If
				End If
			End If

			If Not ReportNumber Is Nothing Then
				If Not m_ReportData Is Nothing Then
					Dim data = m_ReportData.Where(Function(x) x.ReportNumber = ReportNumber).FirstOrDefault
					If data Is Nothing Then
						lueReport.EditValue = Nothing
					Else
						lueReport.EditValue = ReportNumber
					End If
				End If
			End If

		Catch ex As Exception
			lueEmployee.EditValue = Nothing
			lueCustomer.EditValue = Nothing
			lueEmployment.EditValue = Nothing
			lueReport.EditValue = Nothing

		End Try

		m_SuppressUIEvents = suppressUIEventsState

		txtWeeks.EditValue = String.Empty
		ValidateInputOfWeeks()

	End Sub

	''' <summary>
	''' Resets the employee drop down.
	''' </summary>
	Private Sub ResetEmployeeDropDown()

		lueEmployee.Properties.DisplayMember = "LastnameFirstname"
		lueEmployee.Properties.ValueMember = "EmployeeNumber"

		gvEmployee.OptionsView.ShowIndicator = False
		gvEmployee.OptionsView.ShowColumnHeaders = True
		gvEmployee.OptionsView.ShowFooter = False

		gvEmployee.OptionsView.ShowAutoFilterRow = True
		gvEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployee.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = True
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnEmployeeNumber)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnLastnameFirstname.Name = "LastnameFirstname"
		columnLastnameFirstname.FieldName = "LastnameFirstname"
		columnLastnameFirstname.Visible = True
		columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnLastnameFirstname)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnPostcodeAndLocation)

		lueEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployee.Properties.NullText = String.Empty
		lueEmployee.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the customer drop down.
	''' </summary>
	Private Sub ResetCustomerDropDown()

		lueCustomer.Properties.DisplayMember = "Company1"
		lueCustomer.Properties.ValueMember = "CustomerNumber"

		gvCustomer.OptionsView.ShowIndicator = False
		gvCustomer.OptionsView.ShowColumnHeaders = True
		gvCustomer.OptionsView.ShowFooter = False

		gvCustomer.OptionsView.ShowAutoFilterRow = True
		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCustomer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnPostcodeAndLocation)

		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomer.Properties.NullText = String.Empty
		lueCustomer.EditValue = Nothing

	End Sub

	Private Sub ResetEmploymentDropDown()

		lueEmployment.Properties.DisplayMember = "EmploymentViewData"
		lueEmployment.Properties.ValueMember = "EmploymentNumber"

		gvEmployment.OptionsView.ShowIndicator = False
		gvEmployment.OptionsView.ShowColumnHeaders = True
		gvEmployment.OptionsView.ShowFooter = False

		gvEmployment.OptionsView.ShowAutoFilterRow = True
		gvEmployment.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployment.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "EmploymentNumber"
		columnCustomerNumber.FieldName = "EmploymentNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployment.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployment.Columns.Add(columnCompany1)

		Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullname.Name = "EmployeeFullname"
		columnEmployeeFullname.FieldName = "EmployeeFullname"
		columnEmployeeFullname.Visible = True
		columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployment.Columns.Add(columnEmployeeFullname)

		Dim columnEmploymentFromTO As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentFromTO.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnEmploymentFromTO.Name = "EmploymentFromTO"
		columnEmploymentFromTO.FieldName = "EmploymentFromTO"
		columnEmploymentFromTO.Visible = True
		columnEmploymentFromTO.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployment.Columns.Add(columnEmploymentFromTO)

		Dim columnEmploymentAs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentAs.Caption = m_Translate.GetSafeTranslationValue("Einsatz als")
		columnEmploymentAs.Name = "EmploymentAs"
		columnEmploymentAs.FieldName = "EmploymentAs"
		columnEmploymentAs.Visible = True
		columnEmploymentAs.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployment.Columns.Add(columnEmploymentAs)


		lueEmployment.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployment.Properties.NullText = String.Empty
		lueEmployment.EditValue = Nothing

	End Sub

	Private Sub ResetReportDropDown()

		lueReport.Properties.DisplayMember = "ReportViewData"
		lueReport.Properties.ValueMember = "ReportNumber"

		gvReport.OptionsView.ShowIndicator = False
		gvReport.OptionsView.ShowColumnHeaders = True
		gvReport.OptionsView.ShowFooter = False

		gvReport.OptionsView.ShowAutoFilterRow = True
		gvReport.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvReport.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "ReportNumber"
		columnCustomerNumber.FieldName = "ReportNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvReport.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvReport.Columns.Add(columnCompany1)

		Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullname.Name = "EmployeeFullname"
		columnEmployeeFullname.FieldName = "EmployeeFullname"
		columnEmployeeFullname.Visible = True
		columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvReport.Columns.Add(columnEmployeeFullname)

		Dim columnEmploymentFromTO As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentFromTO.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
		columnEmploymentFromTO.Name = "ReportFromTO"
		columnEmploymentFromTO.FieldName = "ReportFromTO"
		columnEmploymentFromTO.Visible = True
		columnEmploymentFromTO.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvReport.Columns.Add(columnEmploymentFromTO)

		Dim columnEmploymentAs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentAs.Caption = m_Translate.GetSafeTranslationValue("Einsatz als")
		columnEmploymentAs.Name = "EmploymentAs"
		columnEmploymentAs.FieldName = "EmploymentAs"
		columnEmploymentAs.Visible = True
		columnEmploymentAs.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvReport.Columns.Add(columnEmploymentAs)


		lueReport.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueReport.Properties.NullText = String.Empty
		lueReport.EditValue = Nothing

	End Sub

	Private Sub OnlueEmployee_EditValueChanged(sender As Object, e As EventArgs) Handles lueEmployee.EditValueChanged

		If m_SuppressUIEvents Then Return

		LoadCustomerDropDownData()
		LoadEmploymentDropDownData()
		LoadReportDropDownData()

	End Sub

	Private Sub OnlueCustomer_EditValueChanged(sender As Object, e As EventArgs) Handles lueCustomer.EditValueChanged

		If m_SuppressUIEvents Then Return

		LoadEmploymentDropDownData()
		LoadReportDropDownData()

	End Sub

	Private Sub OnlueEmployment_EditValueChanged(sender As Object, e As EventArgs) Handles lueEmployment.EditValueChanged

		If m_SuppressUIEvents Then Return

		LoadReportDropDownData()

	End Sub



#Region "Dropdown Funktionen 1. Seite..."

	Function LoadSortData() As IEnumerable(Of SortData)
		Dim result As List(Of SortData) = Nothing

		'0 - Rapportnummer
		'1 - Rapportdatum
		'2 - Kandidatennummer
		'3 - Kandidatenname
		'4 - Kundennummer
		'5 - Kundenname
		result = New List(Of SortData)

		result.Add(New SortData With {.BezValue = String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Rapportnummer"))})
		result.Add(New SortData With {.BezValue = String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Rapportdatum"))})
		result.Add(New SortData With {.BezValue = String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennummer"))})
		result.Add(New SortData With {.BezValue = String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Kandidatenname"))})
		result.Add(New SortData With {.BezValue = String.Format("4 - {0}", m_Translate.GetSafeTranslationValue("Kundennummer"))})
		result.Add(New SortData With {.BezValue = String.Format("5 - {0}", m_Translate.GetSafeTranslationValue("Kundenname"))})

		Return result

	End Function


	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData() As Boolean

		Dim _ClsFunc As New ClsDbFunc
		Dim firstMonth As Integer = Now.Month
		Dim firstYear As Integer = Now.Year
		If Not Cbo_Month.EditValue Is Nothing Then firstMonth = Cbo_Month.EditValue
		If Not Cbo_Year.EditValue Is Nothing Then firstYear = Cbo_Year.EditValue

		Dim employeeData = _ClsFunc.LoadEmployeeData(firstMonth, firstYear)
		m_EmployeeData = employeeData

		If employeeData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnen nicht geladen werden."))
			Return False
		End If

		lueEmployee.EditValue = Nothing
		lueEmployee.Properties.DataSource = employeeData

		Return True

	End Function

	''' <summary>
	''' Loads the customer drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim _ClsFunc As New ClsDbFunc
		Dim firstMonth As Integer = Now.Month
		Dim firstYear As Integer = Now.Year
		If Not Cbo_Month.EditValue Is Nothing Then firstMonth = Cbo_Month.EditValue
		If Not Cbo_Year.EditValue Is Nothing Then firstYear = Cbo_Year.EditValue

		Dim customerData = _ClsFunc.LoadCustomerData(firstMonth, firstYear, lueEmployee.EditValue)
		m_CustomerData = customerData

		If customerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))
			Return False
		End If

		lueCustomer.EditValue = Nothing
		lueCustomer.Properties.DataSource = customerData

		Return True

	End Function

	Private Function LoadEmploymentDropDownData() As Boolean

		Dim _ClsFunc As New ClsDbFunc
		Dim firstMonth As Integer = Now.Month
		Dim firstYear As Integer = Now.Year
		If Not Cbo_Month.EditValue Is Nothing Then firstMonth = Cbo_Month.EditValue
		If Not Cbo_Year.EditValue Is Nothing Then firstYear = Cbo_Year.EditValue

		Dim employmentData = _ClsFunc.LoadEmploymentData(firstMonth, firstYear, lueEmployee.EditValue, lueCustomer.EditValue)
		m_EmploymentData = employmentData

		If employmentData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatzdaten konnen nicht geladen werden."))
			Return False
		End If

		lueEmployment.EditValue = Nothing
		lueEmployment.Properties.DataSource = employmentData

		Return True

	End Function

	Private Function LoadReportDropDownData() As Boolean

		Dim _ClsFunc As New ClsDbFunc
		Dim firstMonth As Integer = Now.Month
		Dim firstYear As Integer = Now.Year
		If Not Cbo_Month.EditValue Is Nothing Then firstMonth = Cbo_Month.EditValue
		If Not Cbo_Year.EditValue Is Nothing Then firstYear = Cbo_Year.EditValue

		Dim reportData = _ClsFunc.LoadReportData(firstMonth, firstYear, lueEmployee.EditValue, lueCustomer.EditValue, lueEmployment.EditValue)
		m_ReportData = reportData

		If reportData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportdaten konnen nicht geladen werden."))
			Return False
		End If

		lueReport.EditValue = Nothing
		lueReport.Properties.DataSource = reportData

		Return True

	End Function

	''' <summary>
	''' Checks the entered week number for correctness
	''' </summary>
	Private Function ValidateInputOfWeeks() As Boolean
		Dim success As Boolean = True
		Dim DateVon As Date
		Dim DateBis As Date
		Dim intFirstWeek As Integer
		Dim intLastWeek As Integer

		Dim strWeeksText As String = String.Format("{0}", txtWeeks.EditValue)

		If Cbo_Month.EditValue Is Nothing Then Cbo_Month.EditValue = Now.Month
		If Cbo_Year.EditValue Is Nothing Then Cbo_Year.EditValue = Now.Year
		lblWochenView.Text = String.Empty

		Try
			strWeeksText = strWeeksText.Replace(" ", "")

			DateVon = Date.Parse("01." & Cbo_Month.EditValue & "." & Cbo_Year.EditValue)
			DateBis = GetLastDayInMonth(DateVon)
			intFirstWeek = GetDateToWeek(DateVon)
			intLastWeek = GetDateToWeek(DateBis)

			Dim weekSeprator As String
			If strWeeksText.Contains("-") Then
				weekSeprator = "-"
			ElseIf strWeeksText.Contains(",") Then
				weekSeprator = ","
			Else
				lblWochenView.Text = String.Format("{0}-{1}", intFirstWeek, intLastWeek)
				weekSeprator = "-"
			End If
			Dim assignedWeeks As New List(Of Integer)
			If String.IsNullOrWhiteSpace(strWeeksText) Then

				If intFirstWeek > intLastWeek Then
					assignedWeeks.Add(intFirstWeek)
					For i = 1 To intLastWeek
						assignedWeeks.Add(i)
					Next

				Else
					assignedWeeks = lblWochenView.Text.Split(New String() {weekSeprator}, StringSplitOptions.RemoveEmptyEntries).Select(Function(n) Integer.Parse(n)).ToList()
				End If

			Else
					assignedWeeks = strWeeksText.Split(New String() {weekSeprator}, StringSplitOptions.RemoveEmptyEntries).Select(Function(n) Integer.Parse(n)).ToList()
			End If

			If intFirstWeek < intLastWeek Then assignedWeeks.Sort()
			Dim assignedFirstweek As Integer = assignedWeeks(0)
			Dim assignedLastweek As Integer = assignedWeeks(assignedWeeks.Count - 1)

			If weekSeprator = "-" Then
				If (assignedFirstweek < intFirstWeek AndAlso intFirstWeek < 52) OrElse assignedFirstweek > intLastWeek Then assignedFirstweek = intFirstWeek
				If (assignedLastweek > intLastWeek AndAlso assignedLastweek < 52) OrElse (assignedLastweek < intFirstWeek AndAlso intFirstWeek < 52) Then assignedLastweek = intLastWeek

				intFirstWeek = assignedFirstweek
				intLastWeek = assignedLastweek ' If(assignedWeeks(assignedWeeks.Count - 1) > intLastWeek AndAlso assignedWeeks(assignedWeeks.Count - 1) >= 52, assignedWeeks(assignedWeeks.Count - 1), intLastWeek)


				assignedWeeks.Clear()
				Dim i As Integer = intFirstWeek
				If intFirstWeek > intLastWeek Then

					assignedWeeks.Add(intFirstWeek)
					For i = 1 To intLastWeek
						assignedWeeks.Add(i)
					Next

					lblWochenView.Text = String.Join(",", assignedWeeks)

				Else

					While i <= intLastWeek
						assignedWeeks.Add(i)

						i += 1
					End While
					lblWochenView.Text = String.Join(",", assignedWeeks)
				End If

			Else
				lblWochenView.Text = String.Join(",", assignedWeeks)

			End If


		Catch ex As Exception
			lblWochenView.Text = String.Empty
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			success = False

		End Try

		Return success
	End Function

	Private Function GetLastDayInMonth(ByVal dDate As Date) As Date
		dDate = DateAdd(DateInterval.Month, 1, dDate)
		dDate = DateAdd(DateInterval.Day, -1, dDate)

		Return dDate
	End Function


	Private Sub Cbo_KST1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST1.QueryPopUp
		ListREKst1(Me.Cbo_KST1, Cbo_Month.EditValue, Cbo_Year.EditValue)
	End Sub

	Private Sub Cbo_KST2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST2.QueryPopUp
		ListREKst2(Me.Cbo_KST2, Cbo_Month.EditValue, Cbo_Year.EditValue)
	End Sub

	Private Sub Cbo_Berater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		ListBerater(Me.Cbo_Berater, Cbo_Month.EditValue, Cbo_Year.EditValue)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		ListRPFiliale(Me.Cbo_Filiale, Cbo_Month.EditValue, Cbo_Year.EditValue, Cbo_Berater.EditValue)
	End Sub

	Private Sub Cbo_Month_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.QueryPopUp
		ListRPMonth(Cbo_Month)
	End Sub

	Private Sub Cbo_Year_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.QueryPopUp
		ListRPYear(Cbo_Year)
	End Sub

	Private Sub OnCbo_Month_SelectedValueChanged(sender As Object, e As EventArgs) Handles Cbo_Month.SelectedValueChanged
		LoadStaticData()
	End Sub

	Private Sub OnCbo_Year_SelectedValueChanged(sender As Object, e As EventArgs) Handles Cbo_Year.SelectedValueChanged
		LoadStaticData()
	End Sub

	Private Sub txtWochen_LostFocus(sender As Object, e As EventArgs) Handles txtWeeks.LostFocus
		If String.IsNullOrWhiteSpace(txtWeeks.EditValue) Then ValidateInputOfWeeks()
	End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmRPListSearch_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width
				My.Settings.frm_Sort = CboSort.EditValue
				My.Settings.TestPrintCount = Val(txtTestPrintCount.EditValue)


				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub


	Private Sub OnFormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSort As String = My.Settings.frm_Sort
		Me.CboSort.EditValue = strSort

		If String.IsNullOrWhiteSpace(strSort) Then
			Dim sortData = CboSort.Properties.DataSource
			If Not sortData Is Nothing Then

				For Each itm As SortData In sortData
					strSort = itm.BezValue

					Exit For
				Next

			End If
		End If
		Me.CboSort.EditValue = strSort

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			txtTestPrintCount.EditValue = If(My.Settings.TestPrintCount = 0, 5, My.Settings.TestPrintCount)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize: {1}", strMethodeName, ex.Message))

		End Try


		Me.xtabSQLAbfrage.PageVisible = m_InitializationData.UserData.UserNr = 1

		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	Sub LoadAdvisorValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_common As New CommonSetting

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 672, m_InitializationData.MDData.MDNr) Then
			Try
				Me.Cbo_Filiale.Enabled = False
				Me.Cbo_KST1.Enabled = False
				Me.Cbo_KST2.Enabled = False
				Dim strUSTitle As String = GetUSTitle(m_InitializationData.UserData.UserNr)

				Me.Cbo_Berater.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
				Me.Cbo_Filiale.Text = m_common.GetLogedUserFiliale
				ListBerater(Me.Cbo_Berater, Me.Cbo_Filiale.Text)


				For Each item As CheckedListBoxItem In Me.Cbo_Berater.Properties.Items
					Dim cv As ComboValue = DirectCast(item.Value, ComboValue)
					Dim strKst As String = cv.ComboValue.Trim
					Dim strUserName As String = cv.Text.Trim
					If strUserName.ToLower.Contains(String.Format("{0}, {1}", m_common.GetLogedUserLastName, m_common.GetLogedUserFirstName).ToLower) Then
						item.CheckState = CheckState.Checked
						Me.Cbo_Berater.Text = cv.Text
						Exit For
					End If

				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BenutzerTitel auflisten: {1}", strMethodeName, ex.Message))
			End Try

		End If

	End Sub

	Sub StartPrinting(ByVal exportinFiles As Boolean)
		Dim bShowDesign As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 614, m_InitializationData.MDData.MDNr)
		Dim ShowDesign As Boolean = bShowDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Dim PrintYear As Integer
		Dim FirstWeek As Integer
		Dim LastWeek As Integer
		Dim DateVon As Date
		Dim DateBis As Date

		DateVon = Date.Parse("01." & Cbo_Month.EditValue & "." & Cbo_Year.EditValue)
		DateBis = GetLastDayInMonth(DateVon)
		PrintYear = m_SearchCriteria.FromYear
		FirstWeek = m_SearchCriteria.FirstWeek
		LastWeek = m_SearchCriteria.LastWeek

		Dim _Setting As New ReportPrintData With {.ShowAsDesign = ShowDesign, .PrintJobNumber = "10.4", .frmhwnd = GetHwnd, .FirstWeek = FirstWeek, .LastWeek = LastWeek, .PrintYear = PrintYear}
		_Setting.CountOfTestPrint = If(Val(txtTestPrintCount.EditValue) = 0, 5, Val(txtTestPrintCount.EditValue))
		_Setting.ExportPrintInFiles = exportinFiles


		Dim obj As New ClsLLWeeklyMonthlyReportPrint(m_InitializationData)

		obj.PrintData = _Setting
		obj.PrintEmployeeRepportData()

	End Sub

#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		Try
			If ValidateData() = False Then Return
			m_SearchCriteria = GetSearchKrieteria()

			Me.txt_SQL_1.EditValue = String.Empty
			ClsDataDetail.GetSQLQuery() = String.Empty
			ClsDataDetail.GetSQLSortString() = String.Empty

			FormIsLoaded("frmRPListSearch_LV", True)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			GetMyQueryString()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			bbiSearch.Enabled = True

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim vonMonth As Integer? = Now.Month
		Dim vonYear As Integer? = Now.Year

		If Not Cbo_Month.EditValue Is Nothing Then vonMonth = Val(Cbo_Month.EditValue)
		If Not Cbo_Year.EditValue Is Nothing Then vonYear = Val(Cbo_Year.EditValue)

		result.FromMonth = vonMonth
		result.FromYear = vonYear

		result.SortIn = CboSort.EditValue
		result.mandantenname = lueMandant.Text

		result.EmployeeNumber = lueEmployee.EditValue
		result.CustomerNumber = lueCustomer.EditValue
		result.Employmentnumber = lueEmployment.EditValue
		result.Reportnumber = lueReport.EditValue

		Dim DateVon As Date = Date.Parse("01." & Cbo_Month.EditValue & "." & Cbo_Year.EditValue)
		Dim DateBis As Date = GetLastDayInMonth(DateVon)
		Dim FirstWeek As Integer = GetDateToWeek(DateVon)
		Dim LastWeek As Integer = GetDateToWeek(DateBis)

		result.ReportWeeks = lblWochenView.Text
		result.FromDate = DateVon
		result.ToDate = DateBis
		result.FirstWeek = FirstWeek
		result.LastWeek = LastWeek

		result.kst1 = Cbo_KST1.EditValue
		result.kst2 = Cbo_KST2.EditValue

		Dim berater As String = String.Empty
		For Each item As CheckedListBoxItem In Cbo_Berater.Properties.Items
			If item.CheckState = CheckState.Checked Then
				Dim cv = DirectCast(item.Value, ComboValue)
				Dim strKst As String = cv.ComboValue.Trim.Replace("'", "").Replace("*", "%")
				berater &= If(String.IsNullOrWhiteSpace(berater), "", ",") & strKst
			End If

		Next

		result.Berater = berater ' Cbo_Berater.EditValue
		result.filiale = Cbo_Filiale.EditValue

		Return result

	End Function

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty
		Dim _ClsDb As New ClsDbFunc

		If ClsDataDetail.GetSQLQuery() = String.Empty Then
			Me.bbiSearch.Enabled = False

			sSql1Query = _ClsDb.GetStartSQLString()    ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, m_SearchCriteria)    ' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If

			ClsDataDetail.GetSQLQuery = String.Format("{0} {1} Order By RP.RPNr", sSql1Query, sSql2Query)
			strSort = _ClsDb.GetSortString(m_SearchCriteria)     ' Sort Klausel
			ClsDataDetail.GetSQLSortString = strSort

			Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery

			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		End If

		Return True

	End Function

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		FormIsLoaded("frmRPListSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		m_SuppressUIEvents = True

		Reset()

		m_SuppressUIEvents = False

		Cbo_Year.EditValue = Now.Year
		Cbo_Month.EditValue = If(Now.Day > 10, Now.Month, Now.Month - 1)

		txt_SQL_1.EditValue = String.Empty
		txt_SQL_2.EditValue = String.Empty


		ClsDataDetail.GetSQLQuery() = String.Empty
		ClsDataDetail.GetSQLSortString() = String.Empty

		LoadStaticData()

	End Sub

#End Region

	Private Sub frm_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmRPListSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		Dim _ClsDb As New ClsDbFunc
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		CheckForIllegalCrossThreadCalls = False
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		_ClsDb.BuildRPDayDb(ClsDataDetail.GetSQLQuery) 'sSql1Query + sSql2Query & " Order By RP.RPNr")
		sSql1Query = _ClsDb.GetStartSQLString_2()    ' 2. String für Drucken (die Whereklausel kommt nicht mehr.
		ClsDataDetail.GetSQLQuery() = sSql1Query + ClsDataDetail.GetSQLSortString()

		CreateTableRPPrint()

		e.Result = True
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			m_UtilityUI.ShowErrorDialog(e.Error.ToString)
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Vorgang wurde abgebrochen."))

			Else
				BackgroundWorker1.CancelAsync()

				If Not FormIsLoaded("frmRPListSearch_LV", True) Then
					frmMyLV = New frmRPListSearch_LV(m_InitializationData)

					frmMyLV.PrintYear = m_SearchCriteria.FromYear
					frmMyLV.FirstWeek = m_SearchCriteria.FirstWeek
					frmMyLV.LastWeek = m_SearchCriteria.LastWeek

					frmMyLV.LoadReportWeeklyPrintData()
					frmMyLV.Show()
					frmMyLV.BringToFront()

					Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", frmMyLV.RecCount)

				End If
				Me.bbiSearch.Enabled = True
				Me.txt_SQL_2.Text = ClsDataDetail.GetSQLQuery()

				Me.bbiPrint.Enabled = frmMyLV.gvRP.RowCount > 0
				Me.bbiExport.Enabled = frmMyLV.gvRP.RowCount > 0

				If frmMyLV.gvRP.RowCount > 0 Then CreateExportPopupMenu()

			End If
		End If
		'Naas - Only for Testing
		'Debug.Assert(False)
		'm_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		'Dim data = m_ListingDatabaseAccess.LoadRPPrintWeeklyData(Year(Now), 1, 53, m_InitializationData.UserData.UserNr)
		'For Each itm In data
		'  Debug.Print(itm.Woche)
		'Next


	End Sub

#End Region


#Region "Contextmenü für Print und Export..."

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
		StartPrinting(False)

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {If(IsUserActionAllowed(m_InitializationData.UserData.UserNr, 604, m_InitializationData.MDData.MDNr), "Liste drucken#mnuRPListPrint", "")}

	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))
	'			bshowMnu = myValue(0).ToString <> String.Empty

	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

	'				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try
	'End Sub

	'Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

	'	Me.SQL4Print = String.Empty
	'	Me.bPrintAsDesign = False

	'	Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
	'	Select Case e.Item.Name.ToUpper
	'		Case "mnuRPListPrint".ToUpper
	'			GetData4Print(False, False, ClsDataDetail.GetModulToPrint())

	'		Case "PrintDesign".ToUpper
	'			GetData4Print(True, False, ClsDataDetail.GetModulToPrint())


	'		Case Else
	'			Exit Sub

	'	End Select

	'End Sub


	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim employmentData = SelectedEmploymentData
		Dim reportData = SelectedReportData
		Dim employeeNumber As Integer? = lueEmployee.EditValue
		Dim customerNumber As Integer? = lueCustomer.EditValue
		Dim employmentNumber As Integer? = lueEmployment.EditValue

		If Not reportData Is Nothing AndAlso employmentData Is Nothing Then
			employeeNumber = reportData.EmployeeNumber
			customerNumber = reportData.CustomerNumber
			employmentNumber = reportData.EmploymentNumber
		ElseIf Not employmentData Is Nothing Then
			employeeNumber = employmentData.EmployeeNumber
			customerNumber = employmentData.CustomerNumber
		End If

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		If lueEmployee.EditValue Is Nothing Then lueEmployee.EditValue = employeeNumber
		If lueCustomer.EditValue Is Nothing Then lueCustomer.EditValue = customerNumber
		If lueEmployment.EditValue Is Nothing Then lueEmployment.EditValue = employmentNumber

		m_SuppressUIEvents = suppressUIEventsState

		Dim liMnu As New List(Of String) From {"Alle Daten in CSV- / TXT exportieren...#CSV",
			If(Not lueEmployee.EditValue Is Nothing, "-An Kandidaten Mail-Adresse senden#employeemail", ""),
			If(Not lueCustomer.EditValue Is Nothing, If(lueEmployee.EditValue Is Nothing, "-", "") & "An Kunden Mail-Adresse senden#customermail", ""),
			"-Alle Daten für eCall™-SMS-Versand exportieren......#eCall-SMS"}


		Try
			bbiExport.Manager = Me.BarManager1
			BarManager1.ForceInitialize()
			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			Me.bbiExport.Enabled = True
			Dim allowedWOS As Boolean = True
			allowedWOS = m_Mandant.AllowedExportCustomer2WOS(lueMandant.EditValue, m_SearchCriteria.FromYear.GetValueOrDefault(Now.Year)) OrElse m_Mandant.AllowedExportEmployee2WOS(lueMandant.EditValue, m_SearchCriteria.FromYear.GetValueOrDefault(Now.Year))

			For Each mnuItm In liMnu
				If String.IsNullOrWhiteSpace(mnuItm) Then Continue For

				Dim itm As New DevExpress.XtraBars.BarButtonItem

				Dim myValue As String() = mnuItm.Split(CChar("#"))
				Dim beginGroup As Boolean = mnuItm.ToString.StartsWith("-") OrElse mnuItm.ToString.StartsWith("_")
				Dim mnuCaption As String = myValue(0).ToString
				Dim mnuName As String = myValue(1).ToString

				If beginGroup Then mnuCaption = mnuCaption.Remove(0, 1)
				itm.Caption = m_Translate.GetSafeTranslationValue(mnuCaption)
				itm.Name = mnuName
				bshowMnu = Not String.IsNullOrWhiteSpace(bshowMnu)
				If mnuName = "employeemail" OrElse mnuName = "customermail" Then itm.Enabled = allowedWOS

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					If beginGroup Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If

				bshowMnu = True

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = ClsDataDetail.GetSQLQuery()

		Select Case UCase(e.Item.Name.ToUpper)

			Case UCase("eCall-SMS")
				Dim sql As String
				sql = "Select MA.MANr, MA.Nachname, MA.Vorname, "
				sql &= "	( "
				sql &= "Case MA.MA_SMS_Mailing "
				sql &= "When 0 Then MA.Natel Else '' End) As Natel, "
				sql &= "MA.Geschlecht, "
				sql &= "mak.Briefanrede AS Anredeform, "
				sql &= "MA.Strasse, MA.Land, MA.Plz, MA.Ort "
				sql &= "From RPDayDb R "
				sql &= "LEFT JOIN dbo.RP ON RP.RPNr = R.RPNr "
				sql &= "LEFT JOIN dbo.Mitarbeiter MA ON MA.manr = RP.manr "
				sql &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = RP.manr "
				sql &= "Where USNr = {0} And (MA.Natel <> '' And MA.Natel Is Not Null ) And MA.MA_SMS_Mailing <> 1 "
				sql &= "Order by MA.Nachname, MA.Vorname"

				strSQL = String.Format(sql, m_InitializationData.UserData.UserNr)

				Call RuneCallSMSModul(strSQL)

			Case UCase("TXT"), UCase("CSV")
				StartExportESModul()

			Case UCase("employeemail")
				StartExportToMail("employeemail")

			Case UCase("customermail")
				StartExportToMail("customermail")


			Case Else
				Return

		End Select

	End Sub

	Sub StartExportESModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn, .SQL2Open = ClsDataDetail.GetSQLQuery(), .ModulName = "RPNotfinishedSearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromRPNotFoundedSearchListing(ClsDataDetail.GetSQLQuery())

	End Sub

	Sub StartExportToMail(ByVal mailType As String)
		Dim frmMail = New frmMailTpl(m_InitializationData)

		Dim PrintYear As Integer
		Dim FirstWeek As Integer
		Dim LastWeek As Integer
		Dim DateVon As Date
		Dim DateBis As Date

		DateVon = Date.Parse("01." & Cbo_Month.EditValue & "." & Cbo_Year.EditValue)
		DateBis = GetLastDayInMonth(DateVon)
		PrintYear = m_SearchCriteria.FromYear
		FirstWeek = m_SearchCriteria.FirstWeek
		LastWeek = m_SearchCriteria.LastWeek

		Try

			Dim _Setting As New ReportPrintData With {.ShowAsDesign = False, .PrintJobNumber = "10.4", .frmhwnd = GetHwnd, .FirstWeek = FirstWeek, .LastWeek = LastWeek, .PrintYear = PrintYear}
			_Setting.CountOfTestPrint = Integer.MaxValue
			_Setting.ExportPrintInFiles = True


			Dim obj As New ClsLLWeeklyMonthlyReportPrint(m_InitializationData)

			obj.PrintData = _Setting
			Dim printResult = obj.PrintEmployeeRepportData()
			If Not printResult.Printresult OrElse _Setting.ListOfExportedFiles Is Nothing OrElse _Setting.ListOfExportedFiles.Count = 0 Then Return

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(GetType(WaitForm1), True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Mail-Versand") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Die Daten werden zusammengestellt") & "...")


			Dim preselectionSetting As New PreselectionMailData

			If mailType = "employeemail" Then
				preselectionSetting = New PreselectionMailData With {.MailType = MailTypeEnum.EMPLOYEECOMMON, .EmployeeNumber = lueEmployee.EditValue, .PDFFilesToSend = _Setting.ListOfExportedFiles}
			Else
				preselectionSetting = New PreselectionMailData With {.MailType = MailTypeEnum.CUSTOMERCOMMON, .CustomerNumber = lueCustomer.EditValue, .PDFFilesToSend = _Setting.ListOfExportedFiles}
			End If

			frmMail.PreselectionData = preselectionSetting

			frmMail.LoadData()

			frmMail.Show()
			frmMail.BringToFront()

		Catch ex As Exception
			SplashScreenManager.CloseForm(False)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Sub RuneCallSMSModul(ByVal strTempSQL As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = m_InitializationData.MDData, .PersonalizedData = m_InitializationData.ProsonalizedData, .TranslationData = m_InitializationData.TranslationData, .UserData = m_InitializationData.UserData}

			Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, strTempSQL, SPS.Export.Listing.Utility.ReceiverType.Employee)
			frmSMS2eCall.LoadData()

			frmSMS2eCall.Show()


		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			m_UtilityUI.ShowErrorDialog(e.Message)

		End Try

	End Sub

	Function LoadSearchQueryResult() As IEnumerable(Of RPDataForRPPrint)

		Dim result As List(Of RPDataForRPPrint) = Nothing
		m_utility_SP = New Utilities

		Dim sql As String

		sql = ClsDataDetail.GetSQLQuery()

		Dim reader As SqlClient.SqlDataReader = m_utility_SP.OpenReader(m_InitializationData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of RPDataForRPPrint)

				While reader.Read()
					Dim overviewData As New RPDataForRPPrint
					overviewData.RPNr = CInt(m_utility_SP.SafeGetInteger(reader, "RPNr", 0))
					overviewData.MANr = CInt(m_utility_SP.SafeGetInteger(reader, "MANr", 0))
					overviewData.KDNr = CInt(m_utility_SP.SafeGetInteger(reader, "KDNr", 0))
					overviewData.ESNr = CInt(m_utility_SP.SafeGetInteger(reader, "ESNr", 0))
					overviewData.Monat = CInt(m_utility_SP.SafeGetInteger(reader, "Monat", 0))
					overviewData.Jahr = m_utility_SP.SafeGetString(reader, "Jahr", "")
					overviewData.Von = m_utility_SP.SafeGetDateTime(reader, "Von", Nothing)
					overviewData.Bis = m_utility_SP.SafeGetDateTime(reader, "Bis", Nothing)
					overviewData.PrintedWeeks = m_utility_SP.SafeGetString(reader, "PrintedWeeks", Nothing)
					overviewData.MDNr = m_utility_SP.SafeGetString(reader, "MDNr", Nothing)
					result.Add(overviewData)
				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility_SP.CloseReader(reader)

		End Try

		Return result
	End Function

	''' <summary>
	''' Creates the database table 'RPPrint'
	''' </summary>
	Private Sub CreateTableRPPrint()

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim DeleteExistigRecords As Boolean = True
		Dim success As Boolean = True
		Dim strSelectedWeeks As String = String.Empty

		Dim resultRPPrint As List(Of ReportWeeklyPrintData)
		Dim InsertRPPrintData As New ReportWeeklyPrintData
		Dim listOfRPData = LoadSearchQueryResult()
		Dim weekOfVonDate As Integer?
		Dim weekOfBisDate As Integer?
		Dim WochenTag As Integer?
		Dim MoDate As Date?
		Dim DiDate As Date?
		Dim MiDate As Date?
		Dim DoDate As Date?
		Dim FrDate As Date?
		Dim SaDate As Date?
		Dim SoDate As Date?
		Dim RpDat As Date
		Dim InsertDataToDb As Boolean = False

		Dim strMsgText = m_Translate.GetSafeTranslationValue("Der Wochenrapport konnte nicht geschrieben werden!")
		Dim strMsgHeader = m_Translate.GetSafeTranslationValue("Daten schreiben")


		success = success AndAlso m_ListingDatabaseAccess.DeleteUserReportPrintData(New ReportWeeklyPrintData With {.MDNr = m_InitializationData.MDData.MDNr, .UserNr = m_InitializationData.UserData.UserNr})
		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler beim Löschen der Rapport-Druck Daten."))

			Return
		End If
		Try

			strSelectedWeeks = ReadSelectedWeeks()
			resultRPPrint = New List(Of ReportWeeklyPrintData)

			For Each Field In listOfRPData
				RpDat = Field.Von

				Do While RpDat <= Field.Bis
					Trace.WriteLine(String.Format("day: {0}", RpDat))

					weekOfVonDate = GetDateToWeek(RpDat)
					weekOfBisDate = GetDateToWeek(Field.Bis)
					Dim fromWeek As String = String.Format("|{0}|", weekOfVonDate)

					Dim verifyAlreadyPrinted As Boolean = tgsPrintAlreadyPrinted.EditValue
					If Not verifyAlreadyPrinted Then
						verifyAlreadyPrinted = (InStr(strSelectedWeeks, fromWeek) > 0 AndAlso (String.IsNullOrWhiteSpace(Field.PrintedWeeks) OrElse Not Field.PrintedWeeks.Contains(weekOfVonDate)))
					Else
						verifyAlreadyPrinted = InStr(strSelectedWeeks, fromWeek) > 0
					End If

					If Not verifyAlreadyPrinted Then
						RpDat = RpDat.AddDays(1)

						Continue Do
					End If

					If weekOfVonDate = 1 And Month(RpDat) = 12 Then
						weekOfVonDate = 53
					ElseIf weekOfVonDate = 53 And Month(RpDat) = 1 Then
						'weekOfVonDate = 1
					End If

					WochenTag = Weekday(RpDat, FirstDayOfWeek.System)
					Select Case WochenTag
						Case 1 : MoDate = RpDat     '        'vbMonday
						Case 2 : DiDate = RpDat
						Case 3 : MiDate = RpDat
						Case 4 : DoDate = RpDat
						Case 5 : FrDate = RpDat
						Case 6 : SaDate = RpDat
						Case 7 : SoDate = RpDat
					End Select

					InsertDataToDb = WochenTag = 7 OrElse RpDat = Field.Bis
					If InsertDataToDb Then
						InsertRPPrintData.MDNr = m_InitializationData.MDData.MDNr
						InsertRPPrintData.RPNr = Field.RPNr
						InsertRPPrintData.MANr = Field.MANr
						InsertRPPrintData.KDNr = Field.KDNr
						InsertRPPrintData.ESNr = Field.ESNr
						InsertRPPrintData.MondayDate = MoDate
						InsertRPPrintData.TuesdayDate = DiDate
						InsertRPPrintData.WednesdayDate = MiDate
						InsertRPPrintData.ThursdayDate = DoDate
						InsertRPPrintData.FridayDate = FrDate
						InsertRPPrintData.SaturdayDate = SaDate
						InsertRPPrintData.SundayDate = SoDate
						InsertRPPrintData.Month = Field.Monat
						InsertRPPrintData.Week = weekOfVonDate
						InsertRPPrintData.Year = Field.Jahr
						InsertRPPrintData.PrintedWeeks = String.Empty
						InsertRPPrintData.PrintedDates = String.Empty
						InsertRPPrintData.UserNr = m_InitializationData.UserData.UserNr

						resultRPPrint.Add(InsertRPPrintData)

						success = success AndAlso m_ListingDatabaseAccess.AddWeeklyReportData(InsertRPPrintData)

						If Not success Then
							m_Logger.LogError(strMsgText)
							m_UtilityUI.ShowOKDialog(strMsgText, strMsgHeader, MessageBoxIcon.Exclamation)

							Exit For
						End If

						DeleteExistigRecords = False
						MoDate = Nothing
						DiDate = Nothing
						MiDate = Nothing
						DoDate = Nothing
						FrDate = Nothing
						SaDate = Nothing
						SoDate = Nothing
					End If

					RpDat = RpDat.AddDays(1)

				Loop

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.Message)

		End Try

	End Sub

	''' <summary>
	''' Returns the User selected weeks 
	''' </summary>
	Private Function ReadSelectedWeeks() As String
		Dim SelectedWeeks As String = String.Empty
		'Dim strOperant As String = String.Empty
		'Dim shtWeekFrom As Short
		'Dim shtWeekUntil As Short
		'Dim intFirstWeek As Integer
		'Dim intLastWeek As Integer
		'Dim DateVon As Date
		'Dim DateBis As Date

		Try
			SelectedWeeks = lblWochenView.Text.Replace(" ", "")
			'If String.IsNullOrWhiteSpace(SelectedWeeks) Then
			'	DateVon = Date.Parse("01." & Cbo_Month.EditValue & "." & Cbo_Year.EditValue)
			'	DateBis = GetLastDayInMonth(DateVon)
			'	intFirstWeek = GetDateToWeek(DateVon)
			'	intLastWeek = GetDateToWeek(DateBis)
			'	SelectedWeeks = String.Format("{0}-{1}", intFirstWeek, intLastWeek)
			'End If

			'Dim tmpWeeks = SelectedWeeks.Split("-")
			'If tmpWeeks.Length > 1 Then
			'	shtWeekFrom = tmpWeeks(0)
			'	shtWeekUntil = tmpWeeks(1)
			'	SelectedWeeks = ""
			'	For shtCount2 = shtWeekFrom To shtWeekUntil
			'		SelectedWeeks &= shtCount2.ToString & "|"
			'	Next

			'End If
			''If InStr(SelectedWeeks, "-") > 0 Then
			''	shtPosition = InStr(SelectedWeeks, "-")
			''	shtWeekFrom = Val(SelectedWeeks.Substring(0, shtPosition - 1))
			''	shtWeekUntil = Val(SelectedWeeks.Substring(shtPosition + 1))
			''	SelectedWeeks = ""
			''	For shtCount2 = shtWeekFrom To shtWeekUntil
			''		SelectedWeeks &= shtCount2.ToString & "|"
			''	Next
			''End If

			If Not String.IsNullOrWhiteSpace(SelectedWeeks) Then
				SelectedWeeks = String.Format("|{0}|", SelectedWeeks)
				SelectedWeeks = SelectedWeeks.Replace(",", "|")
				SelectedWeeks = SelectedWeeks.Replace("||", "|")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString()))
			SelectedWeeks = String.Empty

		End Try

		Return SelectedWeeks

	End Function

	''' <summary>
	''' Returns the week number based on the date
	''' </summary>
	''' <param name="dDate"> The Date for the calculation </param>
	Private Function GetDateToWeek(ByVal dDate As Date) As Integer

		Return DatePart(DateInterval.WeekOfYear, dDate, FirstDayOfWeek.System, FirstWeekOfYear.System)

	End Function

	'Naas Added - 29.08.2018
	Private Function ValidateData() As Boolean

		Dim valid As Boolean = True
		'Naas - ToDo -- (Text Übersetzung)
		Dim errorTextWeek As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie die richtigen Wochen ein.")
		Dim errorTextMonth As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen gültigen Monat ein.")
		Dim errorTextYear As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen gültiges Jahr ein.")

		ErrorProvider.Clear()

		Try
			'mandatory fields
			valid = valid And SetErrorIfInvalid(Cbo_Year, ErrorProvider, Cbo_Year.EditValue Is Nothing, errorTextYear)
			valid = valid And SetErrorIfInvalid(Cbo_Month, ErrorProvider, Cbo_Month.EditValue Is Nothing, errorTextMonth)
			valid = valid And SetErrorIfInvalid(txtWeeks, ErrorProvider, Not ValidateInputOfWeeks(), errorTextWeek)

		Catch ex As Exception
			valid = False
		End Try

		Return valid

	End Function
	'Naas Added - 29.08.2018
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
		End If

		Return Not invalid

	End Function

#End Region


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

End Class

