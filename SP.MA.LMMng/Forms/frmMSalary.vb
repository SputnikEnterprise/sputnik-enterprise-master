
Imports System.Reflection.Assembly

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.ucListSelectPopup
Imports System.IO
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraBars
Imports SP.MA.MLohnMng.Settings
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee.DataObjects.MonthlySalary
Imports SPS.SalaryValueCalculation
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben

Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.TodoMng
Imports SPProgUtility.ProgPath
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing

''' <summary>
''' Monthly salary management.
''' </summary>
Public Class frmMSalary

	Public Delegate Sub MonthlySalaryDataSavedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmNr As Integer)
	Public Delegate Sub MonthlySalaryDataDeletedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmNr As Integer)

#Region "Private Consts"

	Public Const Anzahl_Default_Value As Decimal = 0.0
	Public Const Basis_Default_Value As Decimal = 0.0
	Public Const Ansatz_Default_Value As Decimal = 0.0
	Public Const Betrag_Default_Value As Decimal = 0.0

	Public Const MANDANT_XML_SETTING_SHOW_GUTHABEN_PER_EACH_ES As String = "MD_{0}/Lohnbuchhaltung/guthaben/showguthabenpereaches"
	Public Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

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

	Private m_CommonDbAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' Mandant Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

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
	''' Contains the employee number.
	''' </summary>
	Private m_CurrentEmployeeNumber As Integer

	''' <summary>
	''' Employee data.
	''' </summary>
	Private m_EmployeeList As IEnumerable(Of EmployeeOverviewData)

	''' <summary>
	''' Employee ES data.
	''' </summary>
	Private m_EmployeeESList As IEnumerable(Of ESData)

	''' <summary>
	''' Employee Bank data.
	''' </summary>
	Private m_EmployeeBankList As IEnumerable(Of BankData)

	''' <summary>
	''' LA List.
	''' </summary>
	Private m_LAList As IEnumerable(Of LAData)

	''' <summary>
	''' Current LMNr.
	''' </summary>
	Private m_CurrentLMNr As Integer?

	''' <summary>
	''' Salary value calculator.
	''' </summary>
	Private m_SalaryValueCalculator As SalaryValueCalculator

	''' <summary>
	''' Salary value upper bounds.
	''' </summary>
	Private m_SalaryValueUpperBounds As SalaryValueUpperBounds

	''' <summary>
	''' Option calculate Guthaben without Es.
	''' </summary>
	Private m_CalculateGuthabenWithouthES As Boolean

	''' <summary>
	''' Option calculate Guthaben without Es.
	''' </summary>
	Private m_ChildernLACantonSameAsTaxCanton As Boolean

	''' <summary>
	''' Boolean flag indicating if initial data has been loaded.
	''' </summary>
	Private m_IsInitialDataLoaded As Boolean = False

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The LM docs forms.
	''' </summary>
	Private m_LMDocsForms As frmLMDocs

	''' <summary>
	''' The cls prog path.
	''' </summary>
	Private m_ProgPath As ClsProgPath

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_Mandant As Mandant
	Private m_AllowedDesign As Boolean


#End Region

#Region "Events"

	Public Event MonthlySalaryDataSaved As MonthlySalaryDataSavedHandler
	Public Event MonthlySalaryDataDeleted As MonthlySalaryDataDeletedHandler

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_ProgPath = New ClsProgPath
		m_Mandant = New Mandant
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_InitializationData = _setting

		m_CommonDbAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		m_SalaryValueCalculator = New SalaryValueCalculator(m_InitializationData) '.MDData.MDNr, m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SalaryValueUpperBounds = New SalaryValueUpperBounds

		Try
			m_MandantSettingsXml = New SettingsXml(m_Mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try
		m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 611, m_InitializationData.MDData.MDNr)

		InitializeComponent()


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		gvEmployees.OptionsView.ShowIndicator = False
		gvEmployees.OptionsView.ShowAutoFilterRow = True

		gvExistsMSalary.OptionsView.ShowIndicator = False
		gvExistsMSalary.OptionsView.ShowAutoFilterRow = True

		gvGuthaben.OptionsView.ShowIndicator = False


		Reset()

		Dim success As Boolean = True
		success = success AndAlso LoadEmployeeList()

		AddHandler lueESData.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueLAData.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCanton.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployeeBank.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region

	Sub ReInitialData()

		m_CommonDbAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SalaryValueCalculator = New SalaryValueCalculator(m_InitializationData) '.MDData.MDNr, m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Try
			m_MandantSettingsXml = New SettingsXml(m_Mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


	End Sub

#Region "Public Properties"

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedEmployee As EmployeeOverviewData
		Get
			Dim grdView = TryCast(grdEmployees.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(grdView.GetRow(selectedRows(0)), EmployeeOverviewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected monthly salary.
	''' </summary>
	''' <returns>The selected monthly salary or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedMonthlySalary As MonthlySalaryListItemViewData
		Get
			Dim grdView = TryCast(grdExistsMSalary.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim monthlySalary = CType(grdView.GetRow(selectedRows(0)), MonthlySalaryListItemViewData)
					Return monthlySalary
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the first monthly salary in the list of monthly salaries.
	''' </summary>
	''' <returns>First contact in list or nothing.</returns>
	Public ReadOnly Property FirstRowInListOfMonthlySalary As MonthlySalaryListItemViewData
		Get
			If gvExistsMSalary.RowCount > 0 Then

				Dim rowHandle = gvExistsMSalary.GetVisibleRowHandle(0)
				Return CType(gvExistsMSalary.GetRow(rowHandle), MonthlySalaryListItemViewData)
			Else
				Return Nothing
			End If

		End Get
	End Property

	''' <summary>
	''' Get or sets a boolean flag indicating if only the monthly salary records of the current year should be shown.
	''' </summary>
	''' <returns>The filter setting.</returns>
	Public Property IsFilterMonthlySalaryOnlyOfCurrentYearEnabled As Boolean
		Get
			Return chkThisYear.Checked
		End Get

		Set(value As Boolean)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			chkThisYear.Checked = value
			m_SuppressUIEvents = suppressUIEventsState

		End Set
	End Property

#End Region

#Region "Private Properties"


	Private ReadOnly Property GetHwnd() As String
		Get
			Return CStr(Me.Handle)
		End Get
	End Property

	''' <summary>
	'''  trannslate controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		btnSave.Text = m_Translate.GetSafeTranslationValue(btnSave.Text)
		btnNewMSalary.Text = m_Translate.GetSafeTranslationValue(btnNewMSalary.Text)
		btnDeleteMSalary.Text = m_Translate.GetSafeTranslationValue(btnDeleteMSalary.Text)
		btnCreateTODO.Text = m_Translate.GetSafeTranslationValue(btnCreateTODO.Text)
		btnOpenLMDocument.Text = m_Translate.GetSafeTranslationValue(btnOpenLMDocument.Text)
		btnPrint.Text = m_Translate.GetSafeTranslationValue(btnPrint.Text)

		chkThisYear.Text = m_Translate.GetSafeTranslationValue(chkThisYear.Text)

		xtabEmployeeList.Text = m_Translate.GetSafeTranslationValue(xtabEmployeeList.Text)
		xtabEmployeeDetail.Text = m_Translate.GetSafeTranslationValue(xtabEmployeeDetail.Text)

		grpKandidat.Text = m_Translate.GetSafeTranslationValue(grpKandidat.Text)
		lblKandidat.Text = m_Translate.GetSafeTranslationValue(lblKandidat.Text)
		lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(lblGeschlecht.Text)
		lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(lblGeburtsdatum.Text)
		lblAnzahlkinder.Text = m_Translate.GetSafeTranslationValue(lblAnzahlkinder.Text)
		lblnationalitaet.Text = m_Translate.GetSafeTranslationValue(lblnationalitaet.Text)
		lblzivilstand.Text = m_Translate.GetSafeTranslationValue(lblzivilstand.Text)
		lblbewilligung.Text = m_Translate.GetSafeTranslationValue(lblbewilligung.Text)
		lblgueltig.Text = m_Translate.GetSafeTranslationValue(lblgueltig.Text)

		grpguthaben.Text = m_Translate.GetSafeTranslationValue(grpguthaben.Text)

		grpDetail.Text = m_Translate.GetSafeTranslationValue(grpDetail.Text)
		grpEinsatz.Text = m_Translate.GetSafeTranslationValue(grpEinsatz.Text)
		lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)
		lblEinsatz.Text = m_Translate.GetSafeTranslationValue(lblEinsatz.Text)
		lblGAV.Text = m_Translate.GetSafeTranslationValue(lblGAV.Text)
		lblEinstufung.Text = m_Translate.GetSafeTranslationValue(lblEinstufung.Text)
		lblBranche.Text = m_Translate.GetSafeTranslationValue(lblBranche.Text)
		lblDatum.Text = m_Translate.GetSafeTranslationValue(lblDatum.Text)
		lblSuva.Text = m_Translate.GetSafeTranslationValue(lblSuva.Text)
		lblLohnart.Text = m_Translate.GetSafeTranslationValue(lblLohnart.Text)
		lblZusatz.Text = m_Translate.GetSafeTranslationValue(lblZusatz.Text)
		grpKanton.Text = m_Translate.GetSafeTranslationValue(grpKanton.Text)
		lblkanton.Text = m_Translate.GetSafeTranslationValue(lblkanton.Text)

		lblVon.Text = m_Translate.GetSafeTranslationValue(lblVon.Text)
		lblBis.Text = m_Translate.GetSafeTranslationValue(lblBis.Text)

		grpBetrag.Text = m_Translate.GetSafeTranslationValue(grpBetrag.Text)
		lblAnzahl.Text = m_Translate.GetSafeTranslationValue(lblAnzahl.Text)
		lblBasis.Text = m_Translate.GetSafeTranslationValue(lblBasis.Text)
		lblAnsatz.Text = m_Translate.GetSafeTranslationValue(lblAnsatz.Text)

		grpdta.Text = m_Translate.GetSafeTranslationValue(grpdta.Text)
		lblBank.Text = m_Translate.GetSafeTranslationValue(lblBank.Text)
		lblESR.Text = m_Translate.GetSafeTranslationValue(lblESR.Text)
		chkWithDTA.Text = m_Translate.GetSafeTranslationValue(chkWithDTA.Text)

		bsiLblRecordcount.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRecordcount.Caption)
		bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblErstellt.Caption)
		bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblGeaendert.Caption)

	End Sub

	''' <summary>
	''' Gets or set sets the Anzahl.
	''' </summary>
	Public Property Anzahl As Decimal
		Get
			If Not String.IsNullOrEmpty(txtAnzahl.Text) Then
				Return Convert.ToDecimal(txtAnzahl.Text)
			Else
				Return Anzahl_Default_Value
			End If
		End Get

		Set(value As Decimal)
			txtAnzahl.Text = value
		End Set
	End Property

	''' <summary>
	''' Gets or set sets the Basis.
	''' </summary>
	Public Property Basis As Decimal
		Get
			If Not String.IsNullOrEmpty(txtBasis.Text) Then
				Return Convert.ToDecimal(txtBasis.Text)
			Else
				Return Basis_Default_Value
			End If
		End Get

		Set(value As Decimal)
			txtBasis.Text = value
		End Set
	End Property

	''' <summary>
	''' Gets or set sets the Ansatz.
	''' </summary>
	Public Property Ansatz As Decimal
		Get
			If Not String.IsNullOrEmpty(txtAnsatz.Text) Then
				Return Convert.ToDecimal(txtAnsatz.Text)
			Else
				Return Ansatz_Default_Value
			End If
		End Get

		Set(value As Decimal)
			txtAnsatz.Text = value
		End Set
	End Property

	''' <summary>
	''' Gets or set sets the Betrag.
	''' </summary>
	Public Property Betrag As Decimal
		Get
			If Not String.IsNullOrEmpty(txtBetrag.Text) Then
				Return Convert.ToDecimal(txtBetrag.Text)
			Else
				Return Betrag_Default_Value
			End If
		End Get

		Set(value As Decimal)

			Dim la = SelectedLA
			Dim rounding As Short = 2
			If Not la Is Nothing AndAlso la.Rounding.HasValue Then
				rounding = la.Rounding
				txtBetrag.Properties.Mask.EditMask = "n" + rounding.ToString()
			Else
				txtBetrag.Properties.Mask.EditMask = "n2"

			End If
			txtBetrag.Text = Math.Round(value, rounding)

		End Set
	End Property

	''' <summary>
	''' Gets the selected LA.
	''' </summary>
	''' <returns>The selected LA or nothing.</returns>
	Public ReadOnly Property SelectedLA As LAData
		Get

			If lueLAData.EditValue Is Nothing Then
				Return Nothing
			End If

			Dim laData = m_LAList.Where(Function(data) data.LANr = lueLAData.EditValue).FirstOrDefault()
			Return laData
		End Get
	End Property

	''' <summary>
	''' Gets the BasisFactor.
	''' </summary>
	Public ReadOnly Property BasisFactor As Decimal
		Get

			Dim laData = SelectedLA
			Dim factor As Decimal = 1.0

			If Not laData Is Nothing Then
				factor = If(laData.Sign.Equals("-"), -1.0, 1.0)
			End If

			Return factor

		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Loads monthly salary data of an employee.
	''' </summary>
	''' <param name="manNr">The employee number.</param>
	''' <param name="calculateGuthabenWithouthES">Option calculate Guthaben without ES.</param>
	''' <returns>Boolean value indicating success.</returns>
	Public Function LoadData(ByVal manNr As Integer, ByVal calculateGuthabenWithouthES As Boolean) As Boolean

		Dim success As Boolean = PrepareFrom(manNr)
		success = success AndAlso manNr > 0 AndAlso LoadEmployeeMonthlySalaryData(manNr)

		Return success
	End Function

	''' <summary>
	''' Loads monthly salary data of an employee.
	''' </summary>
	''' <param name="manNr">The employee number.</param>
	''' <param name="lmNrToSelect">The lmNr to select number.</param>
	''' <param name="calculateGuthabenWithouthES">Option calculate Guthaben without ES.</param>
	''' <returns>Boolean value indicating success.</returns>
	Public Function LoadData(ByVal manNr As Integer, ByVal lmNrToSelect As Integer, ByVal calculateGuthabenWithouthES As Boolean) As Boolean

		Dim success As Boolean = PrepareFrom(manNr)
		success = success AndAlso LoadEmployeeMonthlySalaryData(manNr, False)

		Dim monthlySalaryViewDataToSelect = GetMonthlySalaryViewDataByRecordNumber(lmNrToSelect)

		If Not monthlySalaryViewDataToSelect Is Nothing Then
			PresentMonthlySalaryDetailData(monthlySalaryViewDataToSelect)
			FocusMonthlySalary(lmNrToSelect)
		Else

			Dim firstMS = FirstRowInListOfMonthlySalary

			If Not firstMS Is Nothing Then
				PresentMonthlySalaryDetailData(firstMS)
			Else
				PrepareForNew()
			End If

		End If

		Return success
	End Function

	''' <summary>
	''' Allows to insert a new monthly salary.
	''' </summary>
	''' <param name="manNr">The employee number.</param>
	''' <param name="calculateGuthabenWithouthES">Option calculate Guthaben without ES.</param>
	''' <returns>Boolean value indicating success.</returns>
	Public Function NewMonthlySalary(ByVal manNr As Integer, ByVal calculateGuthabenWithouthES As Boolean)

		Dim success As Boolean = PrepareFrom(manNr)
		success = success AndAlso LoadEmployeeMonthlySalaryData(manNr, False)

		PrepareForNew()

		Return success

	End Function

#End Region

#Region "Private Methods"

	''' <summary>
	''' Prepares the from.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function PrepareFrom(ByVal employeeNumber As Integer) As Boolean

		Dim success As Boolean = True

		success = success AndAlso LoadMandantDropDownData()

		' Suppress UI Events
		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		' Disable Supress UI events 
		m_SuppressUIEvents = suppressUIEventsState

		If Not m_IsInitialDataLoaded Then
			success = success AndAlso LoadCantonData()
			m_IsInitialDataLoaded = True
		End If

		' TODO Fardin: neu ist deaktiviert!!!
		'Reset()

		m_CurrentEmployeeNumber = employeeNumber
		m_CalculateGuthabenWithouthES = (m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SHOW_GUTHABEN_PER_EACH_ES, m_InitializationData.MDData.MDNr)) = "0")
		m_ChildernLACantonSameAsTaxCanton = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr),
																																														String.Format("{0}/childerlacantonsameastaxcanton", FORM_XML_MAIN_KEY)), False)


		'success = success AndAlso LoadEmployeeList()
		FocusEmployee(employeeNumber)

		Return success
	End Function

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		m_CurrentEmployeeNumber = Nothing
		m_CurrentLMNr = Nothing
		m_EmployeeESList = Nothing
		m_EmployeeBankList = Nothing

		' Suppress UI Events
		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True


		lueESData.EditValue = Nothing
		ClearESFields()

		lueLAData.EditValue = Nothing
		txtAdditionalInfo.Text = String.Empty
		txtAdditionalInfo.Properties.MaxLength = 255
		txtAdditionalInfo.Enabled = True

		lueCanton.EditValue = Nothing
		cboFirstMonth.EditValue = Nothing
		cboFirstYear.EditValue = Nothing
		cboLastMonth.EditValue = Nothing
		cboLastYear.EditValue = Nothing

		m_SalaryValueUpperBounds.Reset()

		Anzahl = Anzahl_Default_Value
		txtAnzahl.Properties.MaxLength = 16
		txtAnzahl.Enabled = True

		Basis = Basis_Default_Value
		txtBasis.Properties.MaxLength = 16
		txtBasis.Enabled = True

		Ansatz = Ansatz_Default_Value
		txtAnsatz.Properties.MaxLength = 16
		txtAnsatz.Enabled = True

		Betrag = Betrag_Default_Value
		txtBetrag.Properties.MaxLength = 16

		chkWithDTA.Checked = False
		lueEmployeeBank.Visible = False
		txtRefNr.Visible = False

		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

		grpEinsatz.Enabled = True
		grpBetrag.Enabled = True
		grpdta.Enabled = True

		btnDeleteMSalary.Enabled = True
		btnOpenLMDocument.Enabled = False

		' Disable Supress UI events 
		m_SuppressUIEvents = suppressUIEventsState

		' ---Reset drop downs, grids and lists---

		ResetMandantDropDown()
		ResetESDrowDown()
		ResetLADropDown()
		ResetCantonDropDown()
		ResetBankDropDown()

		ResetMonthComboBoxes()
		ResetYearComboBoxes()

		ResetEmployeeOverviewGrid()
		ResetMonthlySalaryOverviewGrid()
		ResetGuthabenGrid()

		TranslateControls()

		Dim allowedUserToChange As Boolean = True
		allowedUserToChange = allowedUserToChange AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 115, m_InitializationData.MDData.MDNr)
		'btnSave.Visible = allowedUserToChange
		'btnDeleteMSalary.Visible = allowedUserToChange
		'btnNewMSalary.Visible = allowedUserToChange
		'btnOpenLMDocument.Visible = allowedUserToChange
		grpDetail.Enabled = allowedUserToChange

		Dim allowedUserToPrint As Boolean = True
		allowedUserToPrint = allowedUserToPrint AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 631, m_InitializationData.MDData.MDNr)
		btnPrint.Enabled = allowedUserToPrint


		' Clear errors
		errorProvider.Clear()

	End Sub

	''' <summary>
	''' Resets the employee overview grid.
	''' </summary>
	Private Sub ResetEmployeeOverviewGrid()

		' Reset the grid
		gvEmployees.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = True
		columnEmployeeNumber.Width = 35
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployees.Columns.Add(columnEmployeeNumber)

		Dim cloumnName As New DevExpress.XtraGrid.Columns.GridColumn()
		cloumnName.Caption = m_Translate.GetSafeTranslationValue("Kandidaten")
		cloumnName.Name = "Name"
		cloumnName.FieldName = "Name"
		cloumnName.Visible = True
		cloumnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployees.Columns.Add(cloumnName)
	End Sub

	''' <summary>
	''' Resets the monthly salary overview grid.
	''' </summary>
	Private Sub ResetMonthlySalaryOverviewGrid()

		' Reset the grid
		gvExistsMSalary.Columns.Clear()

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart-Nr.")
		columnLANr.Name = "LANr"
		columnLANr.FieldName = "LANr"
		columnLANr.Visible = True
		columnLANr.Width = 35
		columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnLANr.DisplayFormat.FormatString = "0.###"
		gvExistsMSalary.Columns.Add(columnLANr)

		Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLAName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnLAName.Name = "LAName"
		columnLAName.FieldName = "LAName"
		columnLAName.Visible = True
		columnLAName.Width = 235
		gvExistsMSalary.Columns.Add(columnLAName)

		Dim columnFromDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFromDate.Caption = m_Translate.GetSafeTranslationValue("Von")
		columnFromDate.Name = "FromDate"
		columnFromDate.FieldName = "FromDate"
		columnFromDate.Visible = True
		columnFromDate.Width = 70
		gvExistsMSalary.Columns.Add(columnFromDate)

		Dim columnToDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnToDate.Caption = m_Translate.GetSafeTranslationValue("Bis")
		columnToDate.Name = "ToDate"
		columnToDate.FieldName = "ToDate"
		columnToDate.Visible = True
		columnToDate.Width = 70
		gvExistsMSalary.Columns.Add(columnToDate)

		Dim columnCount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCount.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
		columnCount.Name = "Count"
		columnCount.FieldName = "Count"
		columnCount.Visible = True
		columnCount.Width = 70
		columnCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnCount.DisplayFormat.FormatString = "N"
		columnCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnCount.AppearanceHeader.Options.UseTextOptions = True
		columnCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnCount.AppearanceCell.Options.UseTextOptions = True
		gvExistsMSalary.Columns.Add(columnCount)

		Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBasis.Caption = ("Basis")
		columnBasis.Name = "Basis"
		columnBasis.FieldName = "Basis"
		columnBasis.Visible = True
		columnBasis.Width = 70
		columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBasis.DisplayFormat.FormatString = "N"
		columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBasis.AppearanceHeader.Options.UseTextOptions = True
		columnBasis.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBasis.AppearanceCell.Options.UseTextOptions = True
		gvExistsMSalary.Columns.Add(columnBasis)

		Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnAmount.Name = "Amount"
		columnAmount.FieldName = "Amount"
		columnAmount.Visible = True
		columnAmount.Width = 70
		columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAmount.DisplayFormat.FormatString = "N"
		columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAmount.AppearanceHeader.Options.UseTextOptions = True
		columnAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAmount.AppearanceCell.Options.UseTextOptions = True
		gvExistsMSalary.Columns.Add(columnAmount)

		Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
		docType.Caption = " "
		docType.Name = "docType"
		docType.FieldName = "docType"
		docType.Visible = True
		Dim picutureEdit As New RepositoryItemPictureEdit()
		picutureEdit.NullText = " "
		docType.ColumnEdit = picutureEdit
		docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
		docType.Width = 20
		gvExistsMSalary.Columns.Add(docType)

	End Sub

	''' <summary>
	''' Resets the Guthaben grid.
	''' </summary>
	Private Sub ResetGuthabenGrid()

		gvGuthaben.BorderStyle = BorderStyles.NoBorder

		gvGuthaben.OptionsView.ShowColumnHeaders = False
		gvGuthaben.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
		gvGuthaben.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
		gvGuthaben.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
		gvGuthaben.OptionsSelection.EnableAppearanceFocusedRow = False
		gvGuthaben.OptionsSelection.EnableAppearanceHideSelection = False


		' Reset the grid
		gvGuthaben.Columns.Clear()

		Dim columnValueName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnValueName.Caption = "ValueName"
		columnValueName.Name = "ValueName"
		columnValueName.FieldName = "ValueName"
		columnValueName.Visible = True
		gvGuthaben.Columns.Add(columnValueName)

		Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnValue.Caption = "Value"
		columnValue.Name = "Value"
		columnValue.FieldName = "Value"
		columnValue.Visible = True
		columnValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnValue.DisplayFormat.FormatString = "N"
		columnValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnValue.AppearanceHeader.Options.UseTextOptions = True
		columnValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnValue.AppearanceCell.Options.UseTextOptions = True
		gvGuthaben.Columns.Add(columnValue)

		Dim columnUnit As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUnit.Caption = "Unit"
		columnUnit.Name = "Unit"
		columnUnit.FieldName = "Unit"
		columnUnit.Visible = True
		columnUnit.Width = 40
		gvGuthaben.Columns.Add(columnUnit)

	End Sub

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
	''' Resets the ES drop down data.
	''' </summary>
	Private Sub ResetESDrowDown()

		lueESData.Properties.DisplayMember = "DisplayText"
		lueESData.Properties.ValueMember = "ESNr"

		Dim columns = lueESData.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("ESNr", 0))
		columns.Add(New LookUpColumnInfo("Company1", 0))

		Dim fromDateColumn = New LookUpColumnInfo("ES_From", 0)
		fromDateColumn.FormatString = "d"
		fromDateColumn.FormatType = DevExpress.Utils.FormatType.Custom
		columns.Add(fromDateColumn)

		Dim toDateColumn = New LookUpColumnInfo("ES_To", 0)
		toDateColumn.FormatString = "d"
		toDateColumn.FormatType = DevExpress.Utils.FormatType.Custom
		columns.Add(toDateColumn)

		lueESData.Properties.ShowHeader = False
		lueESData.Properties.ShowFooter = False
		lueESData.Properties.DropDownRows = 10
		lueESData.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueESData.Properties.SearchMode = SearchMode.AutoComplete
		lueESData.Properties.AutoSearchColumnIndex = 0

		lueESData.Properties.NullText = String.Empty
		lueESData.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the LA drop down data.
	''' </summary>
	Private Sub ResetLADropDown()

		lueLAData.Enabled = True

		lueLAData.Properties.DisplayMember = "DisplayText"
		lueLAData.Properties.ValueMember = "LANr"

		Dim columns = lueLAData.Properties.Columns
		columns.Clear()

		Dim laNrColumn As New LookUpColumnInfo("LANr", 0)
		laNrColumn.FormatString = "0.###"
		laNrColumn.FormatType = DevExpress.Utils.FormatType.Custom

		columns.Add(laNrColumn)
		columns.Add(New LookUpColumnInfo("LALoText", 0))

		lueLAData.Properties.ShowHeader = False
		lueLAData.Properties.ShowFooter = False
		lueLAData.Properties.DropDownRows = 10
		lueLAData.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLAData.Properties.SearchMode = SearchMode.AutoComplete
		lueLAData.Properties.AutoSearchColumnIndex = 0

		lueLAData.Properties.NullText = String.Empty

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueLAData.EditValue = Nothing
		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the canton drop down.
	''' </summary>
	Private Sub ResetCantonDropDown()

		lueCanton.Enabled = True

		lueCanton.Properties.DisplayMember = "Canton"
		lueCanton.Properties.ValueMember = "Canton"

		Dim columns = lueCanton.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Canton", 0))

		lueCanton.Properties.ShowHeader = False
		lueCanton.Properties.ShowFooter = False
		lueCanton.Properties.DropDownRows = 10
		lueCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCanton.Properties.SearchMode = SearchMode.AutoComplete
		lueCanton.Properties.AutoSearchColumnIndex = 0

		lueCanton.Properties.NullText = String.Empty

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueCanton.EditValue = Nothing
		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the bank drop down.
	''' </summary>
	Private Sub ResetBankDropDown()

		lueEmployeeBank.Properties.DisplayMember = "BankData4View"
		lueEmployeeBank.Properties.ValueMember = "RecNr"

		Dim columns = lueEmployeeBank.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("BankName", 0))
		columns.Add(New LookUpColumnInfo("BankLocation", 0))
		columns.Add(New LookUpColumnInfo("BankAccountNumber", 0))

		lueEmployeeBank.Properties.ShowHeader = False
		lueEmployeeBank.Properties.ShowFooter = False
		lueEmployeeBank.Properties.DropDownRows = 10
		lueEmployeeBank.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployeeBank.Properties.SearchMode = SearchMode.AutoComplete
		lueEmployeeBank.Properties.AutoSearchColumnIndex = 0

		lueEmployeeBank.Properties.NullText = String.Empty
		lueEmployeeBank.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the month combo boxes.
	''' </summary>
	Private Sub ResetMonthComboBoxes()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		cboFirstMonth.Properties.Items.Clear()
		cboFirstMonth.Enabled = True

		cboLastMonth.Properties.Items.Clear()
		cboLastMonth.Enabled = True

		For i As Integer = 1 To 12
			cboFirstMonth.Properties.Items.Add(i)
			cboLastMonth.Properties.Items.Add(i)
		Next

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the year combo boxes.
	''' </summary>
	Private Sub ResetYearComboBoxes()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		cboFirstYear.Properties.Items.Clear()
		cboFirstYear.Enabled = True

		cboLastYear.Properties.Items.Clear()
		cboLastYear.Enabled = True

		For i As Integer = DateTime.Now.Year - 2 To DateTime.Now.Year + 10
			cboFirstYear.Properties.Items.Add(i)
			cboLastYear.Properties.Items.Add(i)
		Next

		m_SuppressUIEvents = suppressUIEventsState

	End Sub


	''' <summary>
	''' Loads the mandant drop down data.
	''' </summary>
	Private Function LoadMandantDropDownData() As Boolean
		Dim mandantData = m_CommonDbAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()

		Return mandantData IsNot Nothing
	End Function

	''' <summary>
	''' Loads LA data.
	''' </summary>
	''' <param name="year">The year.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadLAData(ByVal year As Integer) As Boolean

		m_LAList = m_EmployeeDatabaseAccess.LoadLAListForMonthlySalaryMng(year)

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueLAData.Properties.DataSource = m_LAList
		lueLAData.Properties.ForceInitialize()

		If Not lueLAData.EditValue Is Nothing AndAlso
			Not m_LAList.Any(Function(data) data.LANr = lueLAData.EditValue) Then
			lueLAData.EditValue = Nothing
		End If

		m_SuppressUIEvents = suppressUIEventsState

		If m_LAList Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten konnten nicht geladen werden."))
		End If

		Return Not m_LAList Is Nothing

	End Function

	''' <summary>
	''' Loads canton data.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadCantonData() As Boolean

		Dim cantonData = m_EmployeeDatabaseAccess.LoadCantonListForMonthlySalaryMng(m_CurrentEmployeeNumber, m_InitializationData.MDData.MDNr)

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueCanton.Properties.DataSource = cantonData
		lueCanton.Properties.ForceInitialize()
		m_SuppressUIEvents = suppressUIEventsState
		Return Not cantonData Is Nothing

	End Function

	''' <summary>
	''' Loads the employee list.
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeList() As Boolean

		m_EmployeeList = m_EmployeeDatabaseAccess.LoadEmployeeOverviewListForMonthlySalaryMng()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		grdEmployees.DataSource = m_EmployeeList
		m_SuppressUIEvents = suppressUIEventsState

		Return Not m_EmployeeList Is Nothing
	End Function


	''' <summary>
	''' Loads employee monthly salary data. data.
	''' </summary>
	''' <param name="employeeNumber">The employee number</param>
	''' <param name="loadFirstRecord">Boolean flag indicating if first record of new data should be loaded.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeMonthlySalaryData(ByVal employeeNumber As Integer, Optional ByVal loadFirstRecord As Boolean = True) As Boolean

		m_CurrentEmployeeNumber = employeeNumber
		m_EmployeeESList = Nothing
		m_EmployeeBankList = Nothing
		m_CurrentLMNr = Nothing
		' Clear errors
		errorProvider.Clear()

		CleanupLMDocsForm()

		Dim success As Boolean = True

		success = success AndAlso LoadEmployeeDetailData(m_CurrentEmployeeNumber)
		If success Then Me.xtabEmployeeData.SelectedTabPageIndex = 1

		success = success AndAlso LoadEmployeeGuthabenData(m_CurrentEmployeeNumber)
		success = success AndAlso LoadEmployeeESData(m_CurrentEmployeeNumber)
		success = success AndAlso LoadEmployeeBankData(m_CurrentEmployeeNumber)
		success = success AndAlso LoadMonthlySalaryList(m_CurrentEmployeeNumber, IsFilterMonthlySalaryOnlyOfCurrentYearEnabled)

		If loadFirstRecord Then

			Dim firstMS = FirstRowInListOfMonthlySalary

			If Not firstMS Is Nothing Then
				PresentMonthlySalaryDetailData(firstMS)
			Else
				PrepareForNew()
			End If

		End If

		Return success

	End Function

	''' <summary>
	''' Loads detail data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeDetailData(ByVal employeeNumber As Integer) As Boolean

		Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber)

		If employeeData Is Nothing Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Detaildaten konnten nicht geladen werden."))

			Return False
		End If

		lblEmployee.Text = String.Format("{0} {1}", employeeData.Lastname, employeeData.Firstname).Trim()
		lblGender.Text = employeeData.Gender
		lblBirthday.Text = String.Format("{0:d}", employeeData.Birthdate)
		lblChildCount.Text = employeeData.ChildsCount
		lblNationality.Text = employeeData.Nationality
		lblCivilState.Text = employeeData.CivilStatus
		lblPermit.Text = employeeData.Permission
		lblPermitDate.Text = String.Format("{0:d}", employeeData.PermissionToDate)

		Return True
	End Function

	''' <summary>
	''' Loads Guthaben data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeGuthabenData(ByVal employeeNumber As Integer) As Boolean

		Dim success As Boolean = True
		Dim currency As String = String.Empty

		Dim employeeLOSetting = m_EmployeeDatabaseAccess.LoadEmployeeLOSettingForMonthlySalaryMng(m_CurrentEmployeeNumber)

		If Not employeeLOSetting Is Nothing Then
			currency = employeeLOSetting.Currency
		Else
			success = False
		End If

		Dim listOfGuthabenViewData As New List(Of GuthabenViewData)
		Dim payrollUtility = New SPS.MA.Lohn.Utility.PayrollUtility(lueMandant.EditValue)

		Dim value As Decimal? = Nothing

		' Ferien
		value = payrollUtility.LoadAssignedEmployeeFerienData(lueMandant.EditValue, m_CurrentEmployeeNumber, 0)
		'value = GetFerGuthaben(m_CurrentEmployeeNumber, 0)
		If value > 0 Then
			Dim ferienGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Ferien"), .Value = value, .Unit = currency}
			listOfGuthabenViewData.Add(ferienGuthaben)
		End If

		' Feiertag
		value = payrollUtility.LoadAssignedEmployeeFeierTagData(lueMandant.EditValue, m_CurrentEmployeeNumber, 0)
		'value = GetFeierGuthaben(m_CurrentEmployeeNumber, 0)
		If value > 0 Then
			Dim feiertagsGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Feiertag"), .Value = value, .Unit = currency}
			listOfGuthabenViewData.Add(feiertagsGuthaben)
		End If

		' 13. Lohn
		value = payrollUtility.LoadAssignedEmployee13LohnData(lueMandant.EditValue, m_CurrentEmployeeNumber, 0)
		'value = Get13Guthaben(m_CurrentEmployeeNumber, 0)
		If value > 0 Then
			Dim lohn13Guthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("13. Lohn"), .Value = value, .Unit = currency}
			listOfGuthabenViewData.Add(lohn13Guthaben)
		End If

		' Darlehen 
		value = payrollUtility.LoadAssignedEmployeeDarlehenData(lueMandant.EditValue, m_CurrentEmployeeNumber)
		'value = GetDarlehenGuthaben(m_CurrentEmployeeNumber, m_InitializationData.MDData.MDNr)
		If value > 0 Then
			Dim darlehnGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Darlehen"), .Value = value, .Unit = currency}
			listOfGuthabenViewData.Add(darlehnGuthaben)
		End If

		' Gleitzeit
		Dim cRestStd As Decimal = 0
		Dim cRestBetrag As Decimal = 0

		'GetAnzGStd(m_CurrentEmployeeNumber, cRestStd, cRestBetrag, m_InitializationData.MDData.MDNr)
		Dim data = m_ListingDatabaseAccess.LoadFlexibleWorkingHoursData(m_InitializationData.MDData.MDNr, m_CurrentEmployeeNumber)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Gleitzeit Daten konnten nicht geladen werden.")

		Else
			If data.CreditHours.GetValueOrDefault(0) <> 0 AndAlso data.CreditAmount.GetValueOrDefault(0) <> 0 Then

				Dim gleitzeitStdGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Gleitzeit"), .Value = data.CreditHours.GetValueOrDefault(0), .Unit = m_Translate.GetSafeTranslationValue("Std")}
				listOfGuthabenViewData.Add(gleitzeitStdGuthaben)

				Dim gleitzeitBetragGuthaben As New GuthabenViewData With {.ValueName = String.Empty, .Value = data.CreditAmount.GetValueOrDefault(0), .Unit = currency}
				listOfGuthabenViewData.Add(gleitzeitBetragGuthaben)
			End If

		End If

		' Nachtzulagen (LANr = 290)
		Dim cNachtStd As Decimal = 0
		Dim cNachtBetrag As Decimal = 0

		GetAnzNightStd(m_CurrentEmployeeNumber, cNachtStd, cNachtBetrag, m_InitializationData.MDData.MDNr)

		If cNachtStd > 0 Then

			Dim nachzulageStdGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Nachtzulage"), .Value = cNachtStd, .Unit = m_Translate.GetSafeTranslationValue("Std")}
			listOfGuthabenViewData.Add(nachzulageStdGuthaben)

			Dim nachtzulageBetragGuthaben As New GuthabenViewData With {.ValueName = String.Empty, .Value = cNachtBetrag, .Unit = currency}
			listOfGuthabenViewData.Add(nachtzulageBetragGuthaben)

		End If


		' Apply data to gridview
		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		grdGuthaben.DataSource = listOfGuthabenViewData
		m_SuppressUIEvents = suppressUIEventsState

		Return success
	End Function

	''' <summary>
	''' Loads employee ES data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeESData(ByVal employeeNumber As Integer) As Boolean

		m_EmployeeESList = m_EmployeeDatabaseAccess.LoadESListForMonthlySalaryMng(employeeNumber, lueMandant.EditValue)

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueESData.Properties.DataSource = m_EmployeeESList
		lueESData.Properties.ForceInitialize()
		m_SuppressUIEvents = suppressUIEventsState

		If m_EmployeeESList Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden."))
		End If

		Return Not m_EmployeeESList Is Nothing

	End Function

	''' <summary>
	''' Loads emplyoee Bank data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeBankData(ByVal employeeNumber As Integer) As Boolean

		m_EmployeeBankList = m_EmployeeDatabaseAccess.LoadEmployeeBankListForMonthlySalaryMng(employeeNumber)

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		Dim employeeBankViewDataList As New List(Of BankViewData)

		If Not m_EmployeeBankList Is Nothing Then
			For Each field In m_EmployeeBankList
				Dim viewData As BankViewData = New BankViewData
				viewData.RecNr = field.RecNr
				viewData.BankName = field.BankName
				viewData.BankLocation = field.BankLocation
				viewData.BankAccountNumber = field.BankAccountNumber

				employeeBankViewDataList.Add(viewData)
			Next
		Else
			m_UtilityUI.ShowErrorDialog("Bankdaten konnten nicht geladen werden.")
		End If

		lueEmployeeBank.Properties.DataSource = employeeBankViewDataList
		lueEmployeeBank.Properties.ForceInitialize()
		m_SuppressUIEvents = suppressUIEventsState

		Return Not employeeBankViewDataList Is Nothing

	End Function

	''' <summary>
	''' Loads the monthly salary overview list.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="onlyCurrentYear">Boolean value indicating if only data of current year should be shown.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadMonthlySalaryList(ByVal employeeNumber As Integer, ByVal onlyCurrentYear As Boolean) As Boolean
		Dim salaryOverviewList = m_EmployeeDatabaseAccess.LoadMonthlySalaryOverviewListForMonthlySalaryMng(employeeNumber, m_InitializationData.MDData.MDNr, onlyCurrentYear)

		If salaryOverviewList Is Nothing Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnangaben konnten nicht geladen werden."))

			Return False

		End If

		Dim listOfMonthlySalaryViewData As New List(Of MonthlySalaryListItemViewData)

		For Each element In salaryOverviewList

			Dim viewData As New MonthlySalaryListItemViewData
			viewData.MDNr = element.MDNr
			viewData.LMNr = element.LMNr
			viewData.ESNr = element.ESNr
			viewData.LANr = element.LANr
			viewData.LAName = element.LAName
			viewData.FromDate = String.Format("{0} / {1}", element.LP_From, element.Year_From)
			viewData.LP_From = element.LP_From
			Integer.TryParse(element.Year_From, viewData.YearFrom)
			viewData.ToDate = String.Format("{0} / {1}", element.LP_To, element.Year_To)
			viewData.LP_To = element.LP_To
			Integer.TryParse(element.Year_To, viewData.YearTo)

			viewData.Count = element.M_Anz
			viewData.Ans = element.M_Ans
			viewData.Basis = element.M_Bas
			viewData.Amount = element.M_BTR
			viewData.LAIndBez = element.LAIndBez
			viewData.Canton = element.Canton
			viewData.ZGGrund = element.ZGGrund
			viewData.BankNr = element.BankNr
			viewData.CreatedFrom = element.CreatedFrom
			viewData.CreatedOn = element.CreatedOn
			viewData.ChangedFrom = element.ChangedFrom
			viewData.ChangedOn = element.ChangedOn
			viewData.Sign = If(element.Sign Is Nothing, String.Empty, element.Sign)
			viewData.NumberOfExistingLOLRecords = element.NumberOfExistingLOLRecords

			If element.NumberOfExistingLMDocRecords > 0 Then
				viewData.PDFImage = My.Resources.pdf
			End If

			listOfMonthlySalaryViewData.Add(viewData)

		Next

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		grdExistsMSalary.DataSource = listOfMonthlySalaryViewData
		bsiRecordcount.Caption = String.Format("{0}", listOfMonthlySalaryViewData.Count)

		m_SuppressUIEvents = suppressUIEventsState

		Return True
	End Function

	''' <summary>
	''' Present monthly salary (LM) detail data.
	''' </summary>
	''' <param name="monthlySalaryViewData">The monthly salary view data.</param>
	Private Sub PresentMonthlySalaryDetailData(ByVal monthlySalaryViewData As MonthlySalaryListItemViewData)
		' Clear errors
		errorProvider.Clear()

		CleanupLMDocsForm()

		If Not monthlySalaryViewData Is Nothing Then
			m_CurrentLMNr = monthlySalaryViewData.LMNr

			lueMandant.EditValue = monthlySalaryViewData.MDNr
			SelectES(monthlySalaryViewData.ESNr)

			' Suppress UI events
			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			' Load LA data.
			lueLAData.EditValue = monthlySalaryViewData.LANr
			LoadLAData(monthlySalaryViewData.YearFrom)

			txtAdditionalInfo.Text = monthlySalaryViewData.LAIndBez

			lueCanton.EditValue = monthlySalaryViewData.Canton

			cboFirstMonth.EditValue = monthlySalaryViewData.LP_From
			cboFirstYear.EditValue = monthlySalaryViewData.YearFrom
			cboLastMonth.EditValue = monthlySalaryViewData.LP_To
			cboLastYear.EditValue = monthlySalaryViewData.YearTo

			lblNegSign.Visible = monthlySalaryViewData.Sign.Equals("-")

			' Recalculate Salary values bases on parameters
			CalculateSalaryValues(False)

			Anzahl = If(monthlySalaryViewData.Count.HasValue, monthlySalaryViewData.Count, Anzahl_Default_Value)

			If monthlySalaryViewData.Basis.HasValue Then
				Basis = If(monthlySalaryViewData.Sign.Equals("-"), monthlySalaryViewData.Basis.Value * -1.0, monthlySalaryViewData.Basis.Value)
			Else
				Basis = Basis_Default_Value
			End If

			Ansatz = If(monthlySalaryViewData.Ans.HasValue, monthlySalaryViewData.Ans, Ansatz_Default_Value)
			Betrag = If(monthlySalaryViewData.Amount.HasValue, monthlySalaryViewData.Amount, Betrag_Default_Value)

			lueEmployeeBank.EditValue = If(monthlySalaryViewData.BankNr.HasValue, CType(monthlySalaryViewData.BankNr.Value, Integer), Nothing)
			txtRefNr.Text = monthlySalaryViewData.ZGGrund
			lueMandant.Enabled = False

			' Enable/Disable controls
			Dim noLOLRecordsExisting = monthlySalaryViewData.NumberOfExistingLOLRecords <= 0
			grpEinsatz.Enabled = noLOLRecordsExisting

			grpBetrag.Enabled = noLOLRecordsExisting
			grpdta.Enabled = noLOLRecordsExisting AndAlso Not m_EmployeeBankList Is Nothing AndAlso m_EmployeeBankList.Count > 0
			lueLAData.Enabled = noLOLRecordsExisting
			txtAdditionalInfo.Enabled = noLOLRecordsExisting
			lueCanton.Enabled = noLOLRecordsExisting
			cboFirstMonth.Enabled = noLOLRecordsExisting
			cboFirstYear.Enabled = noLOLRecordsExisting

			btnDeleteMSalary.Enabled = noLOLRecordsExisting
			If lueESData.EditValue Is Nothing OrElse lueESData.EditValue = 0 Then
				btnDeleteMSalary.Enabled = False
				btnSave.Enabled = False
			End If

			btnOpenLMDocument.Enabled = True

			' Disable suppress UI events
			m_SuppressUIEvents = suppressUIEventsState

			chkWithDTA.Checked = ((monthlySalaryViewData.BankNr.HasValue AndAlso monthlySalaryViewData.BankNr.Value > 0) OrElse Not String.IsNullOrEmpty(monthlySalaryViewData.ZGGrund))

			bsiCreated.Caption = String.Format("{0:f}, {1}", monthlySalaryViewData.CreatedOn, monthlySalaryViewData.CreatedFrom)
			bsiChanged.Caption = String.Format("{0:f}, {1}", monthlySalaryViewData.ChangedOn, monthlySalaryViewData.ChangedFrom)

		End If

	End Sub

	''' <summary>
	''' Selects ES data record.
	''' </summary>
	''' <param name="esnr">The es nr.</param>
	Private Sub SelectES(ByVal esnr As Integer?)

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		lueESData.EditValue = esnr
		m_SuppressUIEvents = suppressUIEventsState
		DisplayESData(esnr)

	End Sub

	''' <summary>
	''' Clears ES fields.
	''' </summary>
	Private Sub ClearESFields()

		lblESDate.Text = String.Empty
		lblESSuva.Text = String.Empty
		lblGAVBeruf.Text = String.Empty
		lblEinstufungValue.Text = String.Empty
		lblBrancheValue.Text = String.Empty

	End Sub

	''' <summary>
	''' Displays ES detail data.
	''' </summary>
	''' <param name="esNumber">The es number.</param>
	Private Sub DisplayESData(ByVal esNumber As Integer)

		ClearESFields()

		If Not m_EmployeeESList Is Nothing Then

			Dim es = m_EmployeeESList.Where(Function(data) data.ESNr = esNumber).FirstOrDefault()

			If Not es Is Nothing Then

				lblESDate.Text = String.Format("{0:d} - {1:d}", es.ES_From, es.ES_To)
				lblESSuva.Text = es.Suva
				lblGAVBeruf.Text = String.Format("({0}) {1}", es.GAVNr, es.GAVGruppe0)
				lblEinstufungValue.Text = es.Einstufung
				lblBrancheValue.Text = es.ESBranche
			End If

		End If

	End Sub

	''' <summary>
	''' Handles focus change of employee row.
	''' </summary>
	Private Sub OnEmployee_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvEmployees.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selEmployee = SelectedEmployee

		If Not selEmployee Is Nothing Then
			If Not LoadEmployeeMonthlySalaryData(selEmployee.EmployeeNumber) = True Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Monatliche Lohnangaben konnten nicht geladen werden."))
			End If
		End If

	End Sub

	''' <summary>
	''' Handles focus change of monthly salary row.
	''' </summary>
	Private Sub OnMonthlySalary_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvExistsMSalary.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedMS = SelectedMonthlySalary

		If Not selectedMS Is Nothing Then
			PresentMonthlySalaryDetailData(selectedMS)
		Else
			PrepareForNew()
		End If

	End Sub

	Private Sub OnMonthlySalary_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvExistsMSalary.RowCellClick

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedMS = SelectedMonthlySalary

		If Not selectedMS Is Nothing Then
			PresentMonthlySalaryDetailData(selectedMS)
		Else
			PrepareForNew()
		End If

	End Sub

	''' <summary>
	''' Handles change of mandant.
	''' </summary>
	Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueMandant.EditValue Is Nothing Then

			Dim mandantData = CType(lueMandant.GetSelectedDataRow(), MandantData)

			'm_SelectedMandantData = mandantData
			m_InitializationData = CreateInitialData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			ReInitialData()

			ClearESFields()
			LoadEmployeeGuthabenData(m_CurrentEmployeeNumber)
			LoadEmployeeMonthlySalaryData(m_CurrentEmployeeNumber)
			LoadEmployeeESData(m_CurrentEmployeeNumber)
			lueESData.Enabled = Not (m_EmployeeESList Is Nothing OrElse m_EmployeeESList.Count = 0)
			lueLAData.Enabled = Not (m_EmployeeESList Is Nothing OrElse m_EmployeeESList.Count = 0)
			btnSave.Enabled = Not (m_EmployeeESList Is Nothing OrElse m_EmployeeESList.Count = 0)

		Else
			'm_SelectedMandantData = Nothing

			lueESData.EditValue = Nothing
			lueESData.Properties.DataSource = Nothing

		End If

	End Sub

	''' <summary>
	''' Handles edit value changed event of lueESData.
	''' </summary>
	Private Sub OnLueESData_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueESData.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim esNr = lueESData.EditValue

		DisplayESData(esNr)
		lueLAData.EditValue = Nothing

	End Sub

	''' <summary>
	''' Handles click on new button.
	''' </summary>
	Private Sub OnBtnNewMSalary_Click(sender As System.Object, e As System.EventArgs) Handles btnNewMSalary.Click
		PrepareForNew()
	End Sub

	''' <summary>
	''' Handles click on delete button.
	''' </summary>
	Private Sub OnBtnDeleteMSalary_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteMSalary.Click
		If m_InitializationData.MDData.ClosedMD = 1 Then Return

		If m_CurrentLMNr Is Nothing Then Return
		If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																																m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
			Return
		End If

		Dim result = DeleteEmployeeLMResult.Deleted
		result = m_EmployeeDatabaseAccess.DeleteLM(m_CurrentLMNr, m_CurrentEmployeeNumber, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)


		Dim msg As String = String.Empty
		Select Case result
			Case DeleteEmployeeLMResult.CouldNotDeleteBecauseOfExistingLO
				msg = "Die ausgewählte Lohnart wurde in einer Lohnabrechnung integriert."

			Case DeleteEmployeeLMResult.ErrorWhileDelete
				msg = "Die Daten konnten nicht gelöscht werden."

			Case DeleteEmployeeLMResult.CanNotDeleteBecauseMonthIsClosed
				msg = "Die ausgewählte Monat wurde bereits abgeschlossen."

			Case DeleteEmployeeLMResult.Deleted
				RaiseEvent MonthlySalaryDataDeleted(Me, m_CurrentEmployeeNumber, m_CurrentLMNr)

				msg = "Der ausgewählte Datensatz wurde erfolgreich gelöscht."


		End Select
		msg = m_Translate.GetSafeTranslationValue(msg)
		If result <> DeleteEmployeeLMResult.Deleted Then
			Dim errorMsg As String = String.Format(m_Translate.GetSafeTranslationValue("{0}Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen."), vbNewLine)
			m_UtilityUI.ShowOKDialog(String.Format("{0}{1}", msg, errorMsg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
			Return
		End If

		If Not LoadEmployeeMonthlySalaryData(m_CurrentEmployeeNumber) = True Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Monatliche Lohnangaben konnten nicht geladen werden."))
		Else
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)
		End If

	End Sub

	''' <summary>
	''' Handles click on LM document.
	''' </summary>
	Private Sub OnBtnOpenLMDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenLMDocument.Click

		Dim candiateOverviewData = GetOverviewDataOfEmployee(m_CurrentEmployeeNumber)

		Dim candidateName = String.Empty

		If Not candiateOverviewData Is Nothing Then
			candidateName = candiateOverviewData.Name
		End If

		If m_LMDocsForms Is Nothing Then

			m_LMDocsForms = New frmLMDocs(candidateName, m_InitializationData)
			m_LMDocsForms.Location = grpDetail.PointToScreen(lblLMDocPopupPosition.Location)
			AddHandler m_LMDocsForms.NumberDocumentsChanged, AddressOf OnNumbeOfLMDocumentsChanged

		ElseIf m_LMDocsForms.IsDisposed Then
			RemoveHandler m_LMDocsForms.NumberDocumentsChanged, AddressOf OnNumbeOfLMDocumentsChanged

			m_LMDocsForms = New frmLMDocs(candidateName, m_InitializationData)
			m_LMDocsForms.Location = grpDetail.PointToScreen(lblLMDocPopupPosition.Location)
			AddHandler m_LMDocsForms.NumberDocumentsChanged, AddressOf OnNumbeOfLMDocumentsChanged
		End If

		m_LMDocsForms.Show()
		m_LMDocsForms.LoadDocumentsData(m_CurrentLMNr)
		m_LMDocsForms.TopMost = True

	End Sub

	''' <summary>
	''' Handles changes of attached LM documetns.
	''' </summary>
	Private Sub OnNumbeOfLMDocumentsChanged(sender As System.Object, e As System.EventArgs)
		LoadMonthlySalaryList(m_CurrentEmployeeNumber, IsFilterMonthlySalaryOnlyOfCurrentYearEnabled)
		FocusMonthlySalary(m_CurrentLMNr)
	End Sub


	''' <summary>
	''' Handles click on save button.
	''' </summary>
	Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

		If ValidateInputData() Then

			Dim existingLMInPeriodWithSameLANr As Boolean = False

			If m_CurrentLMNr Is Nothing AndAlso Not m_EmployeeDatabaseAccess.CheckForExistingLMInPeriodWithLANr(m_CurrentEmployeeNumber, lueLAData.EditValue, cboFirstMonth.EditValue, cboFirstYear.EditValue, cboLastMonth.EditValue, cboLastYear.EditValue, existingLMInPeriodWithSameLANr) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konten nicht gespeichert werden."))
				Return
			End If

			If existingLMInPeriodWithSameLANr Then

				Dim feedback As Boolean = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Es existiert bereits ein Eintrag in der angegebenen Zeitspanne mit derselben LANr, möchten Sie trotzdem speichern?"),
																															m_Translate.GetSafeTranslationValue("Existierender Datensatz"))

				If feedback = False Then
					Return
				End If

			End If

			Dim es = m_EmployeeESList.Where(Function(data) data.ESNr = lueESData.EditValue).FirstOrDefault()
			Dim la = m_LAList.Where(Function(data) data.LANr = lueLAData.EditValue).FirstOrDefault

			If (es Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konten nicht gespeichert werden. (Einsatzdaten sind nicht vorhanden)"))
				Return
			End If

			If (la Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konten nicht gespeichert werden. (Lohnangaben sind nicht vorhanden)"))
				Return
			End If

			Dim lmData As New LMData

			Dim dt As DateTime = DateTime.Now
			If Not m_CurrentLMNr Is Nothing Then
				lmData.ChangedFrom = m_InitializationData.UserData.UserFullName
				lmData.LMNr = m_CurrentLMNr
			Else
				lmData.CreatedFrom = m_InitializationData.UserData.UserFullName
			End If

			lmData.EmployeeNumber = m_CurrentEmployeeNumber
			lmData.ESNr = lueESData.EditValue
			lmData.LMKst1 = es.ESKst1
			lmData.LMKst2 = es.ESKst2
			lmData.Kst = es.ESKst

			lmData.LANR = lueLAData.EditValue
			lmData.LP_VON = cboFirstMonth.EditValue
			lmData.LP_BIS = cboLastMonth.EditValue
			lmData.JahrVon = cboFirstYear.EditValue
			lmData.JahrBis = cboLastYear.EditValue

			lmData.SUVA = es.Suva
			lmData.M_ANZ = Anzahl
			lmData.M_BAS = Basis * BasisFactor
			lmData.M_ANS = Ansatz
			lmData.M_BTR = Betrag

			lmData.LAName = la.LALoText

			lmData.LAIndBez = txtAdditionalInfo.Text
			lmData.ZGGrund = txtRefNr.Text
			lmData.BnkNr = If(lueEmployeeBank.EditValue Is Nothing, Nothing, Convert.ToInt32(lueEmployeeBank.EditValue))
			lmData.LMWithDTA = If(lueEmployeeBank.EditValue Is Nothing, False, Convert.ToBoolean(Convert.ToInt32(lueEmployeeBank.EditValue) > 0))
			lmData.Kanton = lueCanton.EditValue

			lmData.GAVNr = es.GAVNr
			lmData.GAVKanton = es.GAVKanton
			lmData.GAVGruppe0 = es.GAVGruppe0
			lmData.GAVGruppe1 = es.GAVGruppe1
			lmData.GAVGruppe2 = es.GAVGruppe2
			lmData.GAVGruppe3 = es.GAVGruppe3
			lmData.GAVBezeichnung = es.GAVBezeichnung

			lmData.GAV_FAG = es.GAV_FAG
			lmData.GAV_FAN = es.GAV_FAN
			lmData.GAV_WAG = es.GAV_WAG
			lmData.GAV_WAN = es.GAV_WAN
			lmData.GAV_VAG = es.GAV_VAG
			lmData.GAV_VAN = es.GAV_VAN

			lmData.GAV_FAG_S = es.GAV_FAG_S
			lmData.GAV_FAN_S = es.GAV_FAN_S
			lmData.GAV_WAG_S = es.GAV_WAG_S
			lmData.GAV_WAN_S = es.GAV_WAN_S
			lmData.GAV_VAG_S = es.GAV_VAG_S
			lmData.GAV_VAN_S = es.GAV_VAN_S

			lmData.GAV_FAG_M = es.GAV_FAG_M
			lmData.GAV_FAN_M = es.GAV_FAN_M
			lmData.GAV_WAG_M = es.GAV_WAG_M
			lmData.GAV_WAN_M = es.GAV_WAN_M
			lmData.GAV_VAG_M = es.GAV_VAG_M
			lmData.GAV_VAN_M = es.GAV_VAN_M

			lmData.GAV_FAG_J = es.GAV_FAG_J
			lmData.GAV_FAN_J = es.GAV_FAN_J
			lmData.GAV_WAG_J = es.GAV_WAG_J
			lmData.GAV_WAN_J = es.GAV_WAN_J
			lmData.GAV_VAG_J = es.GAV_VAG_J
			lmData.GAV_VAN_J = es.GAV_VAN_J

			lmData.ESEinstufung = es.Einstufung
			lmData.ESBranche = es.ESBranche

			lmData.KDNr = es.CustomerNumber
			lmData.ESLohnNr = es.ESLohnNr

			lmData.USNr = m_InitializationData.UserData.UserNr

			Dim success As Boolean = True

			If Not lmData.LMNr.HasValue Then

				Dim newLMNr = m_EmployeeDatabaseAccess.AddLM(lmData)

				success = success AndAlso newLMNr.HasValue

				If success Then
					m_CurrentLMNr = newLMNr
					bsiCreated.Caption = String.Format("{0:f}, {1}", dt, lmData.CreatedFrom)
					bsiChanged.Caption = String.Format("{0:f}, {1}", dt, lmData.ChangedFrom)
				End If

			Else
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateLM(lmData)

				If success Then
					bsiChanged.Caption = String.Format("{0:f}, {1}", dt, lmData.ChangedFrom)
				End If

			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konten nicht gespeichert werden."))
			Else
				LoadMonthlySalaryList(m_CurrentEmployeeNumber, IsFilterMonthlySalaryOnlyOfCurrentYearEnabled)
				FocusMonthlySalary(m_CurrentLMNr)
				btnOpenLMDocument.Enabled = True

				RaiseEvent MonthlySalaryDataSaved(Me, m_CurrentEmployeeNumber, m_CurrentLMNr)
			End If

		End If

	End Sub

	Private Sub OnbtnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click

		If Not m_CurrentLMNr Is Nothing Then

			Try
				Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
				Dim _settring As New SPS.Listing.Print.Utility.ClsLLLMSearchPrintSetting With {.DbConnString2Open = "",
																																											 .ShowAsDesign = ShowDesign,
																																											 .SelectedLMNr = m_CurrentLMNr,
																																											 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																											 .LogedUSNr = m_InitializationData.UserData.UserNr,
																																											 .JobNr2Print = "1.1.7"}

				Dim obj As New SPS.Listing.Print.Utility.MASearchListing.ClsPrintLMSearchList(_settring)
				obj.PrintLMListing(New List(Of String)({""}))


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		End If

	End Sub

	''' <summary>
	''' Handles check changed event on chkThisYear.
	''' </summary>
	Private Sub OnChkThisYear_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkThisYear.CheckedChanged

		If m_SuppressUIEvents Then
			Return
		End If

		LoadMonthlySalaryList(m_CurrentEmployeeNumber, IsFilterMonthlySalaryOnlyOfCurrentYearEnabled)

	End Sub

	''' <summary>
	''' Handles check changed on chkWithDTA
	''' </summary>
	Private Sub OnChkWithDTA_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkWithDTA.CheckedChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If chkWithDTA.Checked Then
			lueEmployeeBank.Visible = True
			txtRefNr.Visible = True

		Else
			lueEmployeeBank.EditValue = Nothing
			lueEmployeeBank.Visible = False
			txtRefNr.Visible = False
		End If

	End Sub

	''' <summary>
	''' Handles change of lueLAData.
	''' </summary>
	Private Sub OnLueLAData_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueLAData.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		lblNegSign.Visible = (BasisFactor < 0.0)


		Dim employeeData = GetOverviewDataOfEmployee(m_CurrentEmployeeNumber)

		If Not employeeData Is Nothing Then
			lueCanton.EditValue = If(Not employeeData.S_Canton Is Nothing, employeeData.S_Canton, m_InitializationData.MDData.MDCanton)
		Else
			lueCanton.EditValue = Nothing
		End If

		If lueLAData.EditValue = 3600 OrElse lueLAData.EditValue = 3602 OrElse lueLAData.EditValue = 3650 OrElse
			lueLAData.EditValue = 3700 OrElse lueLAData.EditValue = 3750 OrElse
			lueLAData.EditValue = 3800 OrElse lueLAData.EditValue = 3850 OrElse
			lueLAData.EditValue = 3900 OrElse lueLAData.EditValue = 3900.1 OrElse lueLAData.EditValue = 3901 OrElse lueLAData.EditValue = 3901.1 Then
			If m_ChildernLACantonSameAsTaxCanton Then
				lueCanton.EditValue = If(Not employeeData.S_Canton Is Nothing, employeeData.S_Canton, m_InitializationData.MDData.MDCanton)
			Else
				lueCanton.EditValue = m_InitializationData.UserData.UserMDCanton
			End If

		End If


		CalculateSalaryValues(True)

	End Sub

	''' <summary>
	''' Handles change of lueCanton.
	''' </summary>
	Private Sub OnLueCanton_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCanton.EditValueChanged

		If m_SuppressUIEvents = True Then
			Return
		End If
		If lueCanton.EditValue = Nothing Then Exit Sub

		CalculateSalaryValues(True)

	End Sub

	''' <summary>
	''' Handles change of cboFirstMonth.
	''' </summary>
	Private Sub OnCboFirstMonth_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboFirstMonth.SelectedIndexChanged

		If m_SuppressUIEvents = True Then
			Return
		End If

		CalculateSalaryValues(True)

	End Sub

	''' <summary>
	''' Handles change of cboFirstYear.
	''' </summary>
	Private Sub OncboFirstYear_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboFirstYear.SelectedIndexChanged

		If m_SuppressUIEvents = True Then
			Return
		End If

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		LoadLAData(cboFirstYear.EditValue)
		m_SuppressUIEvents = suppressUIEventsState

		CalculateSalaryValues(True)

	End Sub

	''' <summary>
	''' Handles edit value changing event of txtAnzahl, txtBasis and txtAnsatz.
	''' </summary>
	Private Sub OnSalaryValue_EditValueChanging(sender As System.Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtAnzahl.EditValueChanging, txtBasis.EditValueChanging, txtAnsatz.EditValueChanging

		If (m_SuppressUIEvents) Then
			Return
		End If
		If sender Is txtAnzahl Then
			e.Cancel = Not m_SalaryValueUpperBounds.IsAnzahlValueInBoundary(Convert.ToDecimal(e.NewValue))
		ElseIf sender Is txtBasis Then
			e.Cancel = Not m_SalaryValueUpperBounds.IsBasisValueInBoundary(Convert.ToDecimal(e.NewValue) * BasisFactor)
		ElseIf sender Is txtAnsatz Then
			e.Cancel = Not m_SalaryValueUpperBounds.IsAnsatzValueInBoundary(Convert.ToDecimal(e.NewValue))
		End If

	End Sub

	''' <summary>
	''' Handles change of txtAnzahl, txtBasis and txtAnsatz EditValue change.
	''' </summary>
	Private Sub OnSalaryValue_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles txtAnzahl.EditValueChanged, txtBasis.EditValueChanged, txtAnsatz.EditValueChanged

		If m_SuppressUIEvents = True Then
			Return
		End If

		RecalculateAmount()

	End Sub

	''' <summary>
	''' Refetches salary values.
	''' </summary>
	Private Sub CalculateSalaryValues(ByVal copyResultToTextboxes As Boolean)

		' Disable UI events.
		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		Try

			Dim mergedCalculationResult As SalaryCalculationResult = New SalaryCalculationResult

			If Not lueLAData.EditValue Is Nothing Then

				Dim esNr As Integer = If(lueESData.EditValue Is Nothing, 0, lueESData.EditValue)

				Dim parameters As New SalaryCalculationParameters With {.MANr = m_CurrentEmployeeNumber,
																																.LANr = lueLAData.EditValue,
																																.ESNr = esNr,
																																.Month = cboFirstMonth.EditValue,
																																.Year = cboFirstYear.EditValue,
																																.OptionCalculateGuthabenWithouthES = m_CalculateGuthabenWithouthES,
																																.Canton = lueCanton.EditValue}

				' Anzahl
				Dim anzahlResult = m_SalaryValueCalculator.CalculateCount(parameters)
				MergeResults(anzahlResult, mergedCalculationResult)

				' Basis
				Dim basisResult = m_SalaryValueCalculator.CalculateBasisValue(parameters)
				MergeResults(basisResult, mergedCalculationResult)

				' Ansatz
				Dim ansatzResult = m_SalaryValueCalculator.CalculateAnsatz(parameters)
				MergeResults(ansatzResult, mergedCalculationResult)
			End If

			' Anzahl
			If (copyResultToTextboxes) Then
				Anzahl = If(mergedCalculationResult.Anzahl.HasValue, mergedCalculationResult.Anzahl, Anzahl_Default_Value)
			End If
			txtAnzahl.Enabled = If(mergedCalculationResult.IsAnzahlReadonly.HasValue, Not mergedCalculationResult.IsAnzahlReadonly, True)

			' Basis
			If (copyResultToTextboxes) Then
				Basis = If(mergedCalculationResult.Basis.HasValue, mergedCalculationResult.Basis, Basis_Default_Value)
			End If
			txtBasis.Enabled = If(mergedCalculationResult.IsBasisReadonly.HasValue, Not mergedCalculationResult.IsBasisReadonly, True)

			' Ansatz
			If copyResultToTextboxes Then
				Ansatz = If(mergedCalculationResult.Ansatz.HasValue, mergedCalculationResult.Ansatz, Ansatz_Default_Value)
			End If
			txtAnsatz.Enabled = If(mergedCalculationResult.IsAnsatzReadonly.HasValue, Not mergedCalculationResult.IsAnsatzReadonly, True)

			If copyResultToTextboxes Then
				RecalculateAmount()
			End If

			m_SalaryValueUpperBounds.Reset()
			txtAnzahl.ToolTip = String.Empty
			txtBasis.ToolTip = String.Empty
			txtAnsatz.ToolTip = String.Empty

			' Remember upper bounds
			Dim laData = SelectedLA

			If Not laData Is Nothing Then

				Dim MIN_VALUE_STR As String = m_Translate.GetSafeTranslationValue("Minimaler")
				Dim MAX_VALUE_STR As String = m_Translate.GetSafeTranslationValue("Maximaler")
				Dim Wert_STR As String = m_Translate.GetSafeTranslationValue("Wert")

				If Not laData.AllowMoreAnzahl AndAlso laData.TypeAnzahl.HasValue AndAlso laData.TypeAnzahl = 2 Then
					m_SalaryValueUpperBounds.AnzahlBoundaryValue = Anzahl
					txtAnzahl.ToolTip = String.Format("{0} {1}: {2}", If(Anzahl < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, Anzahl)
				End If

				If Not laData.AllowMoreBasis AndAlso laData.TypeBasis.HasValue AndAlso laData.TypeBasis = 2 Then
					m_SalaryValueUpperBounds.BasisBoundaryValue = Basis * BasisFactor
					txtBasis.ToolTip = String.Format("{0} {1}: {2}", If(Basis * BasisFactor < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, Basis * BasisFactor)
				End If

				If Not laData.AllowMoreAnsatz AndAlso laData.TypeAnsatz.HasValue AndAlso laData.TypeAnsatz = 2 Then
					m_SalaryValueUpperBounds.AnsatzBoundaryValue = Ansatz
					txtAnsatz.ToolTip = String.Format("{0} {1}: {2}", If(Ansatz < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, Ansatz)
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Basis, Anzahl und Ansatz konnten nicht berechnet werden."))
		Finally

			m_SuppressUIEvents = suppressUIEventsState
		End Try

	End Sub

	''' <summary>
	''' Evaluates the salary value calculation result.
	''' </summary>
	''' <param name="result">The calculation result.</param>
	''' <param name="meregedResult">The merged result.</param>
	Private Sub MergeResults(ByVal result As SalaryCalculationResult, ByVal meregedResult As SalaryCalculationResult)

		' Anzahl
		If result.Anzahl.HasValue Then
			meregedResult.Anzahl = result.Anzahl
		End If

		If result.IsAnzahlReadonly.HasValue Then
			meregedResult.IsAnzahlReadonly = result.IsAnzahlReadonly
		End If

		' Basis
		If result.Basis.HasValue Then
			meregedResult.Basis = result.Basis
		End If

		If result.IsBasisReadonly.HasValue Then
			meregedResult.IsBasisReadonly = result.IsBasisReadonly
		End If

		' Ansatz
		If result.Ansatz.HasValue Then
			meregedResult.Ansatz = result.Ansatz
		End If

		If result.IsAnsatzReadonly.HasValue Then
			meregedResult.IsAnsatzReadonly = result.IsAnsatzReadonly
		End If

	End Sub

	''' <summary>
	''' Recalculates the amount.
	''' </summary>
	Private Sub RecalculateAmount()

		If String.IsNullOrEmpty(txtAnzahl.Text) Or
			String.IsNullOrEmpty(txtBasis.Text) Or
			String.IsNullOrEmpty(txtAnsatz.Text) Then

			Return
		End If

		Dim count As Decimal = Anzahl
		Dim bas As Decimal = Basis * BasisFactor
		Dim ans As Decimal = Ansatz / 100.0

		Betrag = count * bas * ans

	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnFrmMSalary_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Me.KeyPreview = True
		Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_Mandant.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
			Dim settting_filter_onlyCurrentYear As Boolean = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_FILTER_ONLY_CURENT_YEAR)
			Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)
			Dim setting_form_detailsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_DETAILSPLITTER)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			If setting_form_mainsplitter > 0 Then Me.sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_form_mainsplitter)
			If setting_form_detailsplitter > 0 Then Me.sccDetail.SplitterPosition = Math.Max(Me.sccDetail.SplitterPosition, setting_form_detailsplitter)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

			IsFilterMonthlySalaryOnlyOfCurrentYearEnabled = settting_filter_onlyCurrentYear

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles the form disposed event.
	''' </summary>
	Private Sub OnFrmMSalary_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
				m_SettingsManager.WriteBoolean(SettingKeys.SETTING_FILTER_ONLY_CURENT_YEAR, IsFilterMonthlySalaryOnlyOfCurrentYearEnabled)

				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccMain.SplitterPosition)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_DETAILSPLITTER, Me.sccDetail.SplitterPosition)

				m_SettingsManager.SaveSettings()
			End If

			CleanupLMDocsForm()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Keypreview for Modul-version
	''' </summary>
	Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

		'If e.KeyCode = Keys.F12 And ModulConstants.UserData.UserNr = 1 Then
		'  Dim strRAssembly As String = ""
		'  Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
		'  For Each a In AppDomain.CurrentDomain.GetAssemblies()
		'    strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
		'  Next
		'  strMsg = String.Format(strMsg, vbNewLine, _
		'                         GetExecutingAssembly().FullName, _
		'                         GetExecutingAssembly().Location, _
		'                         strRAssembly)
		'  DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		'End If
	End Sub

	''' <summary>
	''' Handles row style event on gvExistsMSalary
	''' </summary>
	Private Sub OnGvExistsMSalary_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvExistsMSalary.RowStyle

		If e.RowHandle >= 0 Then
			Dim row As MonthlySalaryListItemViewData = gvExistsMSalary.GetRow(e.RowHandle)

			If row.Amount.HasValue AndAlso row.Amount < 0 Then
				e.Appearance.ForeColor = Color.Red
			End If

		End If
	End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			End If
		End If
	End Sub

	''' <summary>
	''' Prepare form for new LM.
	''' </summary>
	Private Sub PrepareForNew()
		m_CurrentLMNr = Nothing

		' Suppress UI events
		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		lueMandant.Enabled = True
		lueESData.EditValue = Nothing
		ClearESFields()

		lueLAData.EditValue = Nothing
		lueLAData.Enabled = True
		LoadLAData(DateTime.Now.Year)

		txtAdditionalInfo.Text = String.Empty
		txtAdditionalInfo.Enabled = True

		Dim employeeData = GetOverviewDataOfEmployee(m_CurrentEmployeeNumber)

		If Not employeeData Is Nothing Then
			lueCanton.EditValue = If(Not employeeData.Canton Is Nothing, employeeData.Canton, m_InitializationData.MDData.MDCanton)
		Else
			lueCanton.EditValue = Nothing
		End If

		lueCanton.Enabled = True

		cboFirstMonth.EditValue = DateTime.Now.Month
		cboFirstMonth.Enabled = True
		cboFirstYear.EditValue = DateTime.Now.Year
		cboFirstYear.Enabled = True

		cboLastMonth.EditValue = DateTime.Now.Month
		cboLastYear.EditValue = DateTime.Now.Year

		lblNegSign.Visible = False

		m_SalaryValueUpperBounds.Reset()

		txtAnzahl.Enabled = True
		txtBasis.Enabled = True
		txtAnsatz.Enabled = True

		Anzahl = Anzahl_Default_Value
		Basis = Basis_Default_Value
		Ansatz = Ansatz_Default_Value
		Betrag = Betrag_Default_Value

		grpEinsatz.Enabled = True
		grpBetrag.Enabled = True
		grpdta.Enabled = Not m_EmployeeBankList Is Nothing AndAlso m_EmployeeBankList.Count > 0

		chkWithDTA.Checked = False
		lueEmployeeBank.EditValue = Nothing
		txtRefNr.Text = String.Empty
		lueEmployeeBank.Visible = False
		txtRefNr.Visible = False

		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

		btnDeleteMSalary.Enabled = False
		btnOpenLMDocument.Enabled = False

		' Disable UI event suppress
		m_SuppressUIEvents = suppressUIEventsState

		' Clear errors
		errorProvider.Clear()

	End Sub

	''' <summary>
	''' Validates input data.
	''' </summary>
	Private Function ValidateInputData() As Boolean

		Const RESULT_OK As Integer = 0
		Const RESULT_CONFLICT As Integer = 1
		Const RESULT_MONTH_YEAR_FROM_MUST_NOT_BE_CHANGED = 2

		Dim errorText As String = "Bitte geben Sie einen Wert ein."
		Dim errorTextZeroAmount As String = "Der Betrag 0.00 kann nicht erfasst werden. Bitte kontrollieren Sie Ihre Eingabe."

		Dim isValid As Boolean = True

		Dim fromDate = New DateTime(cboFirstYear.EditValue, cboFirstMonth.EditValue, 1)
		Dim toDate As New DateTime(cboLastYear.EditValue, cboLastMonth.EditValue, 1)

		isValid = isValid And SetErrorIfInvalid(lueCanton, errorProvider, String.IsNullOrEmpty(lueCanton.EditValue), errorText)
		If lueLAData.EditValue <> 8100 Then
			isValid = isValid And SetErrorIfInvalid(lueESData, errorProvider, lueESData.EditValue Is Nothing OrElse lueESData.EditValue = 0, errorText)
		End If
		isValid = isValid And SetErrorIfInvalid(lueLAData, errorProvider, lueLAData.EditValue Is Nothing OrElse lueLAData.EditValue = 0, errorText)
		isValid = isValid And SetErrorIfInvalid(cboLastYear, errorProvider, fromDate > toDate, "Von Monat/Jahr ist nach Bis Monat/Jahr")
		isValid = isValid And SetErrorIfInvalid(txtBetrag, errorProvider, Betrag = 0, errorTextZeroAmount)
		isValid = isValid And SetErrorIfInvalid(txtBetrag, errorProvider, m_InitializationData.MDData.ClosedMD = 1, errorTextZeroAmount)

		If isValid Then

			Dim resultCode As Integer = 0
			Dim conflictedLOL = m_EmployeeDatabaseAccess.LoadConflictedLOLRecordsForMonthlySalaryMng(m_CurrentEmployeeNumber, lueESData.EditValue, m_CurrentLMNr, cboFirstMonth.EditValue,
																																															 cboFirstYear.EditValue, cboLastMonth.EditValue, cboLastYear.EditValue, resultCode)

			If conflictedLOL Is Nothing OrElse resultCode = -1 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler bei Konfliktprüfung"))
				Return False
			Else
				Select Case resultCode
					Case RESULT_OK
						' no conflicts
					Case RESULT_CONFLICT
						isValid = False

						Dim frmConflictedLOL As New frmConflictedLOL(m_Translate)
						frmConflictedLOL.ShowConflicts(conflictedLOL)

					Case RESULT_MONTH_YEAR_FROM_MUST_NOT_BE_CHANGED
						' This should no happen because Von Monat/Jahr are locked if there is already an LOL with DestLMMnr = LMNr
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Von Monat/Jahr darf nicht verändert werden."))
						isValid = False
				End Select

			End If

		End If

		Return isValid
	End Function

	''' <summary>
	''' Validates a control.
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
	''' Focus an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	Private Sub FocusEmployee(ByVal employeeNumber As Integer)

		Dim listDataSource As List(Of EmployeeOverviewData) = grdEmployees.DataSource

		If listDataSource Is Nothing Then
			Return
		End If

		Dim employeeData = listDataSource.Where(Function(data) data.EmployeeNumber = employeeNumber).FirstOrDefault()

		If Not employeeData Is Nothing Then
			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			Dim sourceIndex = listDataSource.IndexOf(employeeData)
			Dim rowHandle = gvEmployees.GetRowHandle(sourceIndex)
			gvEmployees.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = suppressUIEventsState
		End If

	End Sub

	''' <summary>
	''' Focus a monthly salary.
	''' </summary>
	''' <param name="lmNr">The lm number.</param>
	Private Sub FocusMonthlySalary(ByVal lmNr As Integer)

		Dim listDataSource As List(Of MonthlySalaryListItemViewData) = grdExistsMSalary.DataSource

		If listDataSource Is Nothing Then
			Return
		End If

		Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.LMNr = lmNr).FirstOrDefault()

		If Not monthlySalaryViewData Is Nothing Then
			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
			Dim rowHandle = gvExistsMSalary.GetRowHandle(sourceIndex)
			gvExistsMSalary.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = suppressUIEventsState
		End If

	End Sub

	''' <summary>
	''' Opens TODO form.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub btnCreateTODO_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateTODO.Click
		Dim frmTodo As New frmTodo(m_InitializationData)
		' optional init new todo
		Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
		Dim EmployeeNumber As Integer? = m_CurrentEmployeeNumber
		Dim CustomerNumber As Integer? = Nothing
		Dim ResponsiblePersonRecordNumber As Integer? = Nothing
		Dim VacancyNumber As Integer? = Nothing
		Dim ProposeNumber As Integer? = Nothing
		Dim ESNumber As Integer? = Nothing
		Dim RPNumber As Integer? = Nothing
		Dim LMNumber As Integer? = Nothing
		Dim RENumber As Integer? = Nothing
		Dim ZENumber As Integer? = Nothing
		Dim Subject As String = String.Empty
		Dim Body As String = ""

		frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber,
												CustomerNumber, ResponsiblePersonRecordNumber,
												VacancyNumber, ProposeNumber, ESNumber, RPNumber,
												LMNumber, RENumber, ZENumber)

		frmTodo.Show()

	End Sub

	''' <summary>
	''' Handles unbound column data event.
	''' </summary>
	Private Sub OnGvExistsMSalaryOnGvContacts_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvExistsMSalary.CustomUnboundColumnData

		If e.Column.Name = "docType" Then
			If (e.IsGetData()) Then
				e.Value = CType(e.Row, MonthlySalaryListItemViewData).PDFImage
			End If
		End If
	End Sub

	''' <summary>
	''' Gets the overview data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>The employee overview data or nothing if not found.</returns>
	Function GetOverviewDataOfEmployee(ByVal employeeNumber As Integer) As EmployeeOverviewData

		If Not m_EmployeeList Is Nothing Then

			Dim overviewData = m_EmployeeList.Where(Function(data) data.EmployeeNumber = employeeNumber).FirstOrDefault()

			Return overviewData
		End If

		Return Nothing
	End Function

	''' <summary>
	''' Gets monthly salary view data by  recordnumber.
	''' </summary>
	''' <param name="recordNumber">The LMnr record number.</param>
	''' <returns>The monthly salary view data or nothing.</returns>
	Private Function GetMonthlySalaryViewDataByRecordNumber(ByVal recordNumber As Integer) As MonthlySalaryListItemViewData

		If gvExistsMSalary.DataSource Is Nothing Then
			Return Nothing
		End If

		Dim monthlySalaryData = CType(gvExistsMSalary.DataSource, List(Of MonthlySalaryListItemViewData))

		Dim viewData = monthlySalaryData.Where(Function(data) data.LMNr = recordNumber).FirstOrDefault

		Return viewData
	End Function

	''' <summary>
	''' Cleanups for LMDocs window.
	''' </summary>
	Private Sub CleanupLMDocsForm()
		If Not m_LMDocsForms Is Nothing Then
			m_LMDocsForms.Close()
			RemoveHandler m_LMDocsForms.NumberDocumentsChanged, AddressOf OnNumbeOfLMDocumentsChanged
			m_LMDocsForms.Dispose()
			m_LMDocsForms = Nothing
		End If
	End Sub

#End Region


#Region "Helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		m_Mandant = New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_Mandant.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_Mandant.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_Mandant.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#End Region

#Region "Helper classes"

	''' <summary>
	''' Salary value upper bounds
	''' </summary>
	Class SalaryValueUpperBounds
#Region "Public Properties"

		Public Property AnzahlBoundaryValue As Decimal?
		Public Property BasisBoundaryValue As Decimal?
		Public Property AnsatzBoundaryValue As Decimal?

#End Region

#Region "Public Methods"

		''' <summary>
		''' Resets the values.
		''' </summary>
		Public Sub Reset()
			AnzahlBoundaryValue = Nothing
			BasisBoundaryValue = Nothing
			AnsatzBoundaryValue = Nothing
		End Sub

		''' <summary>
		''' Checks if Anzahl value is in boundary.
		''' </summary>
		''' <param name="anzahl">The Anzahl value.</param>
		''' <returns>Boolean value indicating if value is in boundary.</returns>
		Public Function IsAnzahlValueInBoundary(ByVal anzahl As Decimal) As Boolean
			Return IsValueInBoundary(anzahl, AnzahlBoundaryValue)
		End Function

		''' <summary>
		''' Checks if Basis value is in boundary.
		''' </summary>
		''' <param name="Basis">The Basis value.</param>
		''' <returns>Boolean value indicating if value is in boundary.</returns>
		Public Function IsBasisValueInBoundary(ByVal basis As Decimal) As Boolean
			Return IsValueInBoundary(basis, BasisBoundaryValue)
		End Function

		''' <summary>
		''' Checks if Ansatz value is in boundary.
		''' </summary>
		''' <param name="Ansatz">The Ansatz value.</param>
		''' <returns>Boolean value indicating if value is in boundary.</returns>
		Public Function IsAnsatzValueInBoundary(ByVal ansatz As Decimal) As Boolean
			Return IsValueInBoundary(ansatz, AnsatzBoundaryValue)
		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		''' Checks if a value in in boundary.
		''' </summary>
		''' <param name="value">The value.</param>
		''' <param name="boundaryValue">The boundary value.</param>
		''' <returns>Boolean value indicating if value is in boundary.</returns>
		Private Function IsValueInBoundary(ByVal value As Decimal, ByVal boundaryValue As Decimal?) As Boolean

			Dim inBoundary As Boolean = True

			If boundaryValue.HasValue Then
				If boundaryValue > 0 Then
					inBoundary = value <= boundaryValue
				Else
					inBoundary = value >= boundaryValue
				End If

			End If

			Return inBoundary
		End Function

#End Region

	End Class

#End Region

#Region "View helper classes"

	''' <summary>
	''' Monthly salary list item view data.
	''' </summary>
	Class MonthlySalaryListItemViewData

		Public Property MDNr As Integer?
		Public Property LMNr As Integer?
		Public Property ESNr As Integer?
		Public Property LANr As Decimal?
		Public Property LAName As String
		Public Property FromDate As String
		Public Property ToDate As String
		Public Property LP_From As Integer?
		Public Property YearFrom As Integer
		Public Property LP_To As Integer?
		Public Property YearTo As Integer
		Public Property Count As Decimal?
		Public Property Ans As Decimal?
		Public Property Basis As Decimal?
		Public Property Amount As Decimal?
		Public Property LAIndBez As String
		Public Property Canton As String
		Public Property ZGGrund As String
		Public Property BankNr As Integer?
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property Sign As String
		Public Property NumberOfExistingLOLRecords As Integer
		Public Property PDFImage As Image

	End Class

	''' <summary>
	''' Guthaben list item view data.
	''' </summary>
	Class GuthabenViewData
		Public Property ValueName As String
		Public Property Value As Decimal
		Public Property Unit As String
	End Class


	''' <summary>
	''' to show bankdata in lueemployeebank
	''' </summary>
	''' <remarks></remarks>
	Private Class BankViewData

		Public Property RecNr As Integer
		Public Property BankName As String
		Public Property BankLocation As String
		Public Property BankAccountNumber As String

		Public ReadOnly Property BankData4View As String
			Get
				Return String.Format("{0}, {1}, {2}", BankName, BankLocation, BankAccountNumber)
			End Get
		End Property

	End Class


#End Region

End Class
