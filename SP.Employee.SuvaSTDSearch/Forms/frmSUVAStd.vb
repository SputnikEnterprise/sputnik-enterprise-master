
Imports System.ComponentModel
Imports System.Reflection

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects


Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.XtraBars
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.Invoice
Imports SPProgUtility.CommonXmlUtility
Imports SPS.Listing.Print.Utility
Imports SP.Infrastructure.DateAndTimeCalculation
Imports DevExpress.XtraEditors.DXErrorProvider
Imports System.Text.RegularExpressions

Partial Public Class frmSUVAStd

	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"


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
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_ReportDatabaseAccess As IReportDatabaseAccess

	''' <summary>
	''' The invoice data access object.
	''' </summary>
	Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

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

	Private m_CustomerWOSID As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String

	Private m_SearchCriteriums As EmployeeSuvaSearchData
	Private m_CurrentSUVAHourData As BindingList(Of SuvaTableListData)

	Private m_AbsenceListData As List(Of String)

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

#End Region


#Region "private consts"

	Private Const MODULNAME_FOR_DELETE As String = "RE"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
	Private Const MANDANT_XML_SETTING_EZ_ON_SEPRATED_PAGE As String = "MD_{0}/Debitoren/ezonsepratedpage"

#End Region


#Region "public property"
	''' <summary>
	''' Gets or sets the preselection data.
	''' </summary>
	Public Property PreselectionData As PreselectionData

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		Dim currentDomain As AppDomain = AppDomain.CurrentDomain

		AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

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

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ReportDatabaseAccess = New DatabaseAccess.Report.ReportDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))


		TranslateControls()
		Reset()

		m_SearchCriteriums = New EmployeeSuvaSearchData With {.MDNr = m_InitializationData.MDData.MDNr, .Jahr = Now.Year}
		LoadDropDownData()

		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueAbsence.ButtonClick, AddressOf OnDropDown_ButtonClick


	End Sub

#End Region


#Region "public methodes"

	''' <summary>
	''' Preselects data.
	''' </summary>
	Public Sub PreselectData()

		Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

		Dim supressUIEventState = m_SuppressUIEvents
		If hasPreselectionData Then

			m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

			' ---Mandant---
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then
					m_SuppressUIEvents = True
					' Mandant is required
					lueMandant.EditValue = PreselectionData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					m_SuppressUIEvents = supressUIEventState
					Return
				End If

			End If

		Else
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

					m_SuppressUIEvents = True
					' Mandant is required
					lueMandant.EditValue = m_InitializationData.MDData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					Return
				End If

			End If

		End If

		Try
			If Not PreselectionData.EmployeeNumbers Is Nothing AndAlso PreselectionData.EmployeeNumbers.Count > 0 Then
				Dim numbers As String = String.Join(",", PreselectionData.EmployeeNumbers.ToArray)
				lueEmployee.EditValue = numbers
			End If

			If PreselectionData.ListYear.HasValue Then
				m_SuppressUIEvents = True
				lueYear.EditValue = PreselectionData.ListYear
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

		m_SuppressUIEvents = supressUIEventState

		bbiPrint.Enabled = False

	End Sub


#End Region


#Region "private property"

	'''' <summary>
	'''' Gets the selected customer.
	'''' </summary>
	'''' <returns>The selected customer or nothing if none is selected.</returns>
	'Private ReadOnly Property SelectedCustomerRecord As CustomertReportHoursData
	'	Get
	'		Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

	'		If Not (gvRP Is Nothing) Then

	'			Dim selectedRows = gvRP.GetSelectedRows()

	'			If (selectedRows.Count > 0) Then
	'				Dim report = CType(gvRP.GetRow(selectedRows(0)), CustomertReportHoursData)

	'				Return report
	'			End If

	'		End If

	'		Return Nothing
	'	End Get

	'End Property

	'''' <summary>
	'''' Gets the selected employee.
	'''' </summary>
	'''' <returns>The selected employee or nothing if none is selected.</returns>
	'Private ReadOnly Property SelectedEmployeeRecord As EmployeeReportHoursData
	'	Get
	'		Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

	'		If Not (gvRP Is Nothing) Then

	'			Dim selectedRows = gvRP.GetSelectedRows()

	'			If (selectedRows.Count > 0) Then
	'				Dim report = CType(gvRP.GetRow(selectedRows(0)), EmployeeReportHoursData)

	'				Return report
	'			End If

	'		End If

	'		Return Nothing
	'	End Get

	'End Property

#End Region


	Private Sub Reset()

		ResetMandantenDropDown()
		LoadMandantenDropDown()

		ResetDropDown()

		'Me.lueEmployee.EditValue = Nothing

		bbiPrint.Enabled = False

	End Sub

#Region "Lookup Edit Reset und Load..."

	Private Sub ResetDropDown()

		ResetEmployeeDropDown()
		ResetMonthDropDown()
		ResetYearDropDown()

		ResetPVLDropDown()
		ResetAbsenceDropDown()

	End Sub

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetEmployeeDropDown()

		lueEmployee.Properties.DisplayMember = "EmployeeFullnameWithComma"
		lueEmployee.Properties.ValueMember = "EmployeeNumber"

		' Reset the grid view
		gvEmployee.OptionsView.ShowIndicator = False
		gvEmployee.OptionsView.ShowAutoFilterRow = True


		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.MaxWidth = 150
		columnEmployeeNumber.Visible = True
		gvEmployee.Columns.Add(columnEmployeeNumber)

		Dim columnEmployeeFullnameWithComma As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullnameWithComma.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullnameWithComma.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullnameWithComma.Name = "EmployeeFullnameWithComma"
		columnEmployeeFullnameWithComma.FieldName = "EmployeeFullnameWithComma"
		columnEmployeeFullnameWithComma.MaxWidth = 300
		columnEmployeeFullnameWithComma.Visible = True
		gvEmployee.Columns.Add(columnEmployeeFullnameWithComma)


		lueEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployee.Properties.NullText = String.Empty
		lueEmployee.Properties.DataSource = Nothing
		lueEmployee.EditValue = Nothing

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
		lueMonth.Properties.DropDownRows = 12
		lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonth.Properties.SearchMode = SearchMode.AutoComplete
		lueMonth.Properties.AutoSearchColumnIndex = 0

		lueMonth.Properties.NullText = String.Empty
		lueMonth.EditValue = Nothing
	End Sub

	Private Sub ResetPVLDropDown()

		luePVL.Properties.DisplayMember = "GAVGruppe0"
		luePVL.Properties.ValueMember = "GAVNr"

		' Reset the grid view
		gvluePVL.OptionsView.ShowIndicator = False

		Dim columnBranchText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBranchText.Caption = m_Translate.GetSafeTranslationValue("PVL-Berufe")
		columnBranchText.Name = "GAVGruppe0"
		columnBranchText.FieldName = "GAVGruppe0"
		columnBranchText.Visible = True
		gvluePVL.Columns.Add(columnBranchText)

		luePVL.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePVL.Properties.NullText = String.Empty
		luePVL.Properties.DataSource = Nothing
		luePVL.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the absence drop down.
	''' </summary>
	Private Sub ResetAbsenceDropDown()

		lueAbsence.Properties.Items.Clear()
		lueAbsence.Properties.DisplayMember = "TranslatedDescription"
		lueAbsence.Properties.ValueMember = "GetFeld"
		'lueAbsence.Properties.ShowHeader = False

		'lueAbsence.Properties.Columns.Clear()
		'lueAbsence.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "TranslatedDescription",
		'																				 .Width = 100,
		'																				 .Caption = m_Translate.GetSafeTranslationValue("TranslatedDescription")})

		'lueAbsence.Properties.ShowFooter = False
		lueAbsence.Properties.DropDownRows = 10
		'lueAbsence.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'lueAbsence.Properties.SearchMode = SearchMode.AutoComplete
		'lueAbsence.Properties.AutoSearchColumnIndex = 0
		lueAbsence.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueAbsence.Properties.NullText = String.Empty
		lueAbsence.EditValue = Nothing
	End Sub

#End Region




	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()
	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)
		If m_SuppressUIEvents Then Return

		If Not SelectedData Is Nothing Then
			Dim MandantData = ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			m_InitializationData = MandantData

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_SearchCriteriums = New EmployeeSuvaSearchData With {.MDNr = m_InitializationData.MDData.MDNr, .Jahr = If(lueYear.EditValue Is Nothing, 0, lueYear.EditValue)}
			LoadDropDownData()

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub

	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadDropDownData() As Boolean
		Dim success As Boolean = True

		m_SuppressUIEvents = True

		success = success AndAlso LoadEmployeeDropDownData()
		success = success AndAlso LoadYearDropDownData()
		success = success AndAlso LoadMonthDropDownData()

		success = success AndAlso LoadEmploymentPVLDropDownData()
		success = success AndAlso LoadReportAbsenceDropDownData()

		m_SuppressUIEvents = False

		Return success

	End Function

	''' <summary>
	''' Loads the year drop down data.
	''' </summary>
	Private Function LoadYearDropDownData() As Boolean

		Dim success As Boolean = True

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not m_InitializationData Is Nothing Then

			Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(m_InitializationData.MDData.MDNr)

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

		lueYear.EditValue = Nothing
		lueYear.Properties.DataSource = wrappedValues
		'lueYear.EditValue = Now.Year
		lueYear.Properties.ForceInitialize()

		Return success
	End Function

	''' <summary>
	''' Loads the month drop down data.
	''' </summary>
	Private Function LoadMonthDropDownData() As Boolean

		Dim success As Boolean = True

		Dim year = lueYear.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not m_InitializationData Is Nothing Then ' and Not year Is Nothing Then

			wrappedValues = New List(Of IntegerValueViewWrapper)
			For i As Integer = 1 To 12
				wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
			Next

		End If

		lueMonth.EditValue = Nothing
		lueMonth.Properties.DataSource = wrappedValues
		lueMonth.Properties.ForceInitialize()

		Return success
	End Function



#Region "loading employment data"

	''' <summary>
	''' Load einstufung drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData() As Boolean
		Dim year As Integer?
		Dim month As Integer?
		Dim currentEmployeeNumber As Integer?

		Dim MDNr As Integer = m_InitializationData.MDData.MDNr
		If Not lueYear.EditValue Is Nothing Then year = lueYear.EditValue
		If Not lueMonth.EditValue Is Nothing Then month = lueMonth.EditValue
		Dim searchData As New EmployeeSuvaSearchData With {.MDNr = MDNr, .Jahr = year, .Monat = month}
		If Not lueEmployee.EditValue Is Nothing Then currentEmployeeNumber = Val(lueEmployee.EditValue)

		Dim data = m_ListingDatabaseAccess.LoadEmployeesForSelectionData(searchData)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Daten konnten nicht geladen werden."))
		End If

		lueEmployee.Properties.DataSource = data
		lueEmployee.EditValue = Nothing

		If currentEmployeeNumber.GetValueOrDefault(0) > 0 Then
			If data.Any(Function(md) md.EmployeeNumber = currentEmployeeNumber) Then
				lueEmployee.EditValue = currentEmployeeNumber
			End If
		End If


		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Load einstufung drop down data.
	''' </summary>
	Private Function LoadEmploymentPVLDropDownData() As Boolean
		Dim data = m_ListingDatabaseAccess.LoadEmploymentExistingPVLData(lueMandant.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("PVL Daten konnten nicht geladen werden."))
		End If

		luePVL.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Load absence drop down data.
	''' </summary>
	Private Function LoadReportAbsenceDropDownData() As Boolean
		Dim data = m_CommonDatabaseAccess.LoadAbsenceData()

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehlcode Daten konnten nicht geladen werden."))
		End If

		lueAbsence.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

#End Region


	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)

		lblNumbers.Text = m_Translate.GetSafeTranslationValue(lblNumbers.Text)

		lblJahr.Text = m_Translate.GetSafeTranslationValue(lblJahr.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)

		lblPVLBeruf.Text = m_Translate.GetSafeTranslationValue(lblPVLBeruf.Text)
		lblFehltage.Text = m_Translate.GetSafeTranslationValue(lblFehltage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)

	End Sub

#Region "Formhandle"

	Private Sub OnCmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		SplashScreenManager.CloseForm(False)

		FormIsLoaded("frmSearch_LV", True)
		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.LastSelectedAbsenceCode = lueAbsence.EditValue

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
			If My.Settings.iHeight > 0 Then Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
			If My.Settings.iWidth > 0 Then Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			If Not String.IsNullOrWhiteSpace(My.Settings.LastSelectedAbsenceCode) Then lueAbsence.EditValue = My.Settings.LastSelectedAbsenceCode


		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
		End Try

	End Sub


#End Region

	Private Sub OnlueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged
		If m_SuppressUIEvents Then Return
		LoadEmployeeDropDownData()
	End Sub

	Private Sub OnlueMonth_EditValueChanged(sender As Object, e As EventArgs) Handles lueMonth.EditValueChanged
		If m_SuppressUIEvents Then Return
		LoadEmployeeDropDownData()
	End Sub

	Private Sub bbiSearch_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSearch.ItemClick
		DxErrorProvider1.ClearErrors()

		FormIsLoaded("frmSearch_LV", True)
		If LoadSearchKonditions() Then
			LoadEmployeeSUVAData()
		End If


	End Sub

	Private Function LoadSearchKonditions() As Boolean

		m_SearchCriteriums = Nothing
		Dim searchData = New SP.DatabaseAccess.Listing.DataObjects.EmployeeSuvaSearchData

		DxErrorProvider1.ClearErrors()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(lueEmployee, DxErrorProvider1, String.IsNullOrEmpty(lueEmployee.Text), errorText)
		isValid = isValid And SetErrorIfInvalid(lueYear, DxErrorProvider1, lueYear.EditValue Is Nothing OrElse (lueYear.EditValue) = 0, errorText)
		If Not isValid Then Return False

		searchData.MDNr = lueMandant.EditValue
		searchData.EmployeeNumbers = GetFormatedNumber()
		searchData.Jahr = lueYear.EditValue

		searchData.Monat = lueMonth.EditValue

		m_SearchCriteriums = searchData


		Return Not m_SearchCriteriums Is Nothing

	End Function

	Private Sub LoadEmployeeSUVAData()
		Dim success As Boolean = True

		success = success AndAlso CreateEmployeeHourData()
		Dim data = m_ListingDatabaseAccess.LoadEmployeeSUVAHourData(m_SearchCriteriums)
		If data Is Nothing Then Return

		m_CurrentSUVAHourData = New BindingList(Of SuvaTableListData)
		For Each p In data
			m_CurrentSUVAHourData.Add(p)
		Next

		If (m_CurrentSUVAHourData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("SUVA-Stunden Daten konnten nicht geladen werden."))

			Return
		End If
		bbiPrint.Enabled = m_CurrentSUVAHourData.Count > 0
		bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet"), m_CurrentSUVAHourData.Count)

		If m_CurrentSUVAHourData.Count > 0 Then

			Dim frm As New frmSearch_LV(m_InitializationData)
			frm.EmployeeHoursToPrintData = m_CurrentSUVAHourData

			frm.Show()
			frm.BringToFront()
		End If

	End Sub

	Private Function CreateEmployeeHourData() As Boolean
		Dim rpDate As DateTime
		Dim result As Boolean = True
		Dim suvaData As SuvaWeekData = Nothing
		Dim absenceCodeAsWorkDay As String = String.Empty
		Dim addWorkDay As Boolean = False

		If Not lueAbsence.EditValue Is Nothing Then absenceCodeAsWorkDay = lueAbsence.EditValue
		absenceCodeAsWorkDay = absenceCodeAsWorkDay.Replace(" ", "")
		m_AbsenceListData = New List(Of String)
		m_AbsenceListData.Clear()

		Dim foundedData = LoadEmployeeReportResultData(m_SearchCriteriums)
		If (foundedData Is Nothing) Then Return False

		Try

			For Each rp In foundedData
				rpDate = rp.Von
				If rp.Monat = 2 Then
					Trace.WriteLine(rp.Monat)
				End If
				Dim rpStdData = LoadRPLDayData(rp.MDNr, rp.RPNR)
				Dim rpAbsenceData = LoadRPAbsenceData(rp.RPNR)
				suvaData = New SuvaWeekData With {.CalendarYear = rp.Jahr, .CalendarMonth = rp.Monat, .CalendarWeek = m_DateUtility.GetCalendarWeeksBetweenDates(rpDate, rpDate)(0),
					.ReportNumber = rp.RPNR, .EmployeeNumber = rp.EmployeeNumber, .CustomerNumber = rp.CustomerNumber, .EmploymentNumber = rp.ESNR, .WorkedDaysInWeek = 0}

				While rpDate <= rp.Bis
					If rpStdData Is Nothing OrElse rpAbsenceData Is Nothing Then Exit While

					suvaData.CalendarWeek = m_DateUtility.GetCalendarWeeksBetweenDates(rpDate, rpDate)(0)
					Select Case rpDate.DayOfWeek
						Case DayOfWeek.Monday
							suvaData.MondayDate = rpDate
							suvaData.MondayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.MondayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.MondayAbsence) Then m_AbsenceListData.Add(suvaData.MondayAbsence)

							If suvaData.MondayStd.GetValueOrDefault(0) = 0 AndAlso Not String.IsNullOrWhiteSpace(suvaData.MondayAbsence.ToString) AndAlso absenceCodeAsWorkDay.Contains(suvaData.MondayAbsence.ToString) Then
								suvaData.WorkedDaysInWeek += 1
							ElseIf suvaData.MondayStd.GetValueOrDefault(0) > 0 Then
								suvaData.WorkedDaysInWeek += 1
							End If

						Case DayOfWeek.Tuesday
							suvaData.TuesdayDate = rpDate
							suvaData.TuesdayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.TuesdayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.TuesdayAbsence) Then m_AbsenceListData.Add(suvaData.TuesdayAbsence)

							If Regex.IsMatch(absenceCodeAsWorkDay, "\b" + Regex.Escape(suvaData.TuesdayAbsence.ToString) + "\b") Then

							End If

							If suvaData.TuesdayStd.GetValueOrDefault(0) = 0 AndAlso Not String.IsNullOrWhiteSpace(suvaData.TuesdayAbsence.ToString) AndAlso absenceCodeAsWorkDay.Contains(suvaData.TuesdayAbsence.ToString) Then
								suvaData.WorkedDaysInWeek += 1
							ElseIf suvaData.TuesdayStd.GetValueOrDefault(0) > 0 Then
								suvaData.WorkedDaysInWeek += 1
							End If

						Case DayOfWeek.Wednesday
							suvaData.WednesdayDate = rpDate
							suvaData.WednesdayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.WednesdayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.WednesdayAbsence) Then m_AbsenceListData.Add(suvaData.WednesdayAbsence)

							If suvaData.WednesdayStd.GetValueOrDefault(0) = 0 AndAlso Not String.IsNullOrWhiteSpace(suvaData.WednesdayAbsence.ToString) AndAlso absenceCodeAsWorkDay.Contains(suvaData.WednesdayAbsence.ToString) Then
								suvaData.WorkedDaysInWeek += 1
							ElseIf suvaData.WednesdayStd.GetValueOrDefault(0) > 0 Then
								suvaData.WorkedDaysInWeek += 1
							End If

						Case DayOfWeek.Thursday
							suvaData.ThursdayDate = rpDate
							suvaData.ThursdayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.ThursdayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.ThursdayAbsence) Then m_AbsenceListData.Add(suvaData.ThursdayAbsence)

							If suvaData.ThursdayStd.GetValueOrDefault(0) = 0 AndAlso Not String.IsNullOrWhiteSpace(suvaData.ThursdayAbsence.ToString) AndAlso absenceCodeAsWorkDay.Contains(suvaData.ThursdayAbsence.ToString) Then
								suvaData.WorkedDaysInWeek += 1
							ElseIf suvaData.ThursdayStd.GetValueOrDefault(0) > 0 Then
								suvaData.WorkedDaysInWeek += 1
							End If

						Case DayOfWeek.Friday
							suvaData.FridayDate = rpDate
							suvaData.FridayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.FridayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.FridayAbsence) Then m_AbsenceListData.Add(suvaData.FridayAbsence)

							If suvaData.FridayStd.GetValueOrDefault(0) = 0 AndAlso Not String.IsNullOrWhiteSpace(suvaData.FridayAbsence.ToString) AndAlso absenceCodeAsWorkDay.Contains(suvaData.FridayAbsence.ToString) Then
								suvaData.WorkedDaysInWeek += 1
							ElseIf suvaData.FridayStd.GetValueOrDefault(0) > 0 Then
								suvaData.WorkedDaysInWeek += 1
							End If

						Case DayOfWeek.Saturday
							suvaData.SaturdayDate = rpDate
							suvaData.SaturdayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.SaturdayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.SaturdayAbsence) Then m_AbsenceListData.Add(suvaData.SaturdayAbsence)

							If suvaData.SaturdayStd.GetValueOrDefault(0) > 0 Then suvaData.WorkedDaysInWeek += 1

						Case DayOfWeek.Sunday
							suvaData.SundayDate = rpDate
							suvaData.SundayStd = rpStdData.GetWorkingHoursOfDay(rpDate.Day)
							suvaData.SundayAbsence = rpAbsenceData.GetAbsenceDayCodeOfDay(rpDate.Day)
							If Not m_AbsenceListData.Contains(suvaData.SundayAbsence) Then m_AbsenceListData.Add(suvaData.SundayAbsence)

							If suvaData.SundayStd.GetValueOrDefault(0) > 0 AndAlso suvaData.GetDayCountValueOfWeek > 0 Then
								suvaData.WorkedDaysInWeek += 1
							Else
								If suvaData.GetDayCountValueOfWeek = 0 Then suvaData.WorkedDaysInWeek = 0
							End If
							'If suvaData.SundayStd.GetValueOrDefault(0) > 0 Then suvaData.WorkedDaysInWeek += 1


							result = result AndAlso AddSuvaDaysToDb(suvaData)
							suvaData = New SuvaWeekData With {.CalendarYear = rp.Jahr, .CalendarMonth = rp.Monat, .CalendarWeek = m_DateUtility.GetCalendarWeeksBetweenDates(rpDate, rpDate)(0),
								.ReportNumber = rp.RPNR, .EmployeeNumber = rp.EmployeeNumber, .CustomerNumber = rp.CustomerNumber, .EmploymentNumber = rp.ESNR, .WorkedDaysInWeek = 0}

					End Select
					rpDate = rpDate.AddDays(1)

					If rpDate > rp.Bis AndAlso rpDate.AddDays(-1).DayOfWeek <> DayOfWeek.Sunday Then

						If suvaData.GetDayCountValueOfWeek = 0 Then
							suvaData.WorkedDaysInWeek = 0
						End If

						result = result AndAlso AddSuvaDaysToDb(suvaData)
						suvaData = New SuvaWeekData With {.CalendarYear = rp.Jahr, .CalendarMonth = rp.Monat, .CalendarWeek = m_DateUtility.GetCalendarWeeksBetweenDates(rpDate, rpDate)(0),
					.ReportNumber = rp.RPNR, .EmployeeNumber = rp.EmployeeNumber, .CustomerNumber = rp.CustomerNumber, .EmploymentNumber = rp.ESNR, .WorkedDaysInWeek = 0}
					End If
					addWorkDay = False

				End While

			Next
			m_AbsenceListData.RemoveAll(Function(str) String.IsNullOrWhiteSpace(str))
			m_AbsenceListData.Sort()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			Return False
		End Try


		Return result

	End Function

	Private Function LoadEmployeeReportResultData(ByVal searchKind As EmployeeSuvaSearchData) As IEnumerable(Of RPMasterData)
		Dim data = m_ListingDatabaseAccess.LoadEmployeeAllReportData(m_SearchCriteriums, m_InitializationData.UserData.UserFiliale)

		If (data Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten-Rapportdaten konnten nicht geladen werden."))
		End If

		Return data
	End Function

	Private Function LoadRPLDayData(ByVal mdnr As Integer, ByVal reporNumber As Integer) As RPLDayHoursTotal
		Dim result As RPLDayHoursTotal = Nothing

		Dim data = m_ReportDatabaseAccess.GetRPLDayHoursTotal(reporNumber, Report.RPLType.Employee)
		If (data Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten-Rapportdaten konnten nicht geladen werden."))
		End If

		Return data
	End Function

	Private Function LoadRPAbsenceData(ByVal reporNumber As Integer) As RPAbsenceDaysData
		Dim result As RPAbsenceDaysData = Nothing

		' The data should exist -> load it.
		Dim data = m_ReportDatabaseAccess.LoadRPAbsenceDaysData(reporNumber)
		If (data Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapport Absens-Daten konnten nicht geladen werden."))
		End If

		Return data
	End Function

	Private Function AddSuvaDaysToDb(ByVal suvaData As SuvaWeekData) As Boolean
		Dim success As Boolean = True

		success = m_ListingDatabaseAccess.AddEmployeeSUVAWeekDaysData(m_InitializationData.MDData.MDNr, suvaData)
		If Not (success) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("SUVA-Stunden Daten konnten nicht gespeichert werden."))
		End If

		Return success
	End Function


#Region "Helpers"

	Private Function GetFormatedNumber() As List(Of Integer)

		Dim numbers As String = lueEmployee.EditValue
		If String.IsNullOrWhiteSpace(numbers) Then Return Nothing
		Dim invoiceNumbers As New List(Of Integer)
		Dim inputComma = numbers.ToString.Split(New String() {",", ";"}, StringSplitOptions.RemoveEmptyEntries)
		Dim inputTil = numbers.ToString.Split(New String() {"-"}, StringSplitOptions.RemoveEmptyEntries)
		If inputComma.Length > 1 AndAlso inputTil.Length > 1 Then
			SplashScreenManager.CloseForm(False)

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Nummern könen entweder durch ',', ';' oder '-' trennen. Bitte versuchen Sie eine Variante aus."))
			Return Nothing
		End If
		If inputComma.Length >= 1 AndAlso Not inputComma(0).Contains("-") Then
			For Each itm In inputComma
				invoiceNumbers.Add(CType(itm, Integer))
			Next

		ElseIf inputTil.Length > 1 Then
			Dim firstNumber As Integer = CType(inputTil(0), Integer)
			Dim lastNumber As Integer = CType(inputTil(1), Integer)

			If firstNumber > lastNumber And lastNumber > 0 Then
				lastNumber = firstNumber
				firstNumber = lastNumber
			End If

			If lastNumber > 0 Then
				For i As Integer = firstNumber To lastNumber
					invoiceNumbers.Add(CType(i, Integer))
				Next
			Else
				invoiceNumbers.Add(CType(firstNumber, Integer))
				invoiceNumbers.Add(CType(lastNumber, Integer))

			End If

		End If


		Return invoiceNumbers

	End Function

	Private Function ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

	''' <summary>
	''' Fills an image combobox with color values.s
	''' </summary>
	''' <param name="imageCombobox">The image combobox.</param>
	''' <param name="colors">The colors.</param>
	Private Sub FillImageComboxWithColorValues(ByVal imageCombobox As ImageComboBoxEdit, ByVal colors As IEnumerable(Of ComboboxColorValueViewData))

		imageCombobox.Properties.Items.Clear()

		' Add the ImageComboBoxItem items.
		' Description -> ColorName
		' Value -> RawValue
		For Each item In colors
			imageCombobox.Properties.Items.Add(New ImageComboBoxItem(item.ColorName, item.RawValue))
		Next

		' Now create a list of images from the raw color values.
		Dim imageList As New ImageList
		imageCombobox.Properties.SmallImages = imageList

		For Each item In imageCombobox.Properties.Items

			Dim width As Integer = 16
			Dim height As Integer = 16
			Dim bmp As New Bitmap(width, height)

			Using g = Graphics.FromImage(bmp)
				g.DrawRectangle(New Pen(Color.Black, 2), 0, 0, width, height)
				g.FillRectangle(New SolidBrush(ColorTranslator.FromWin32(CType(item.Value, Decimal))), 1, 1, width - 2, height - 2)

			End Using

			imageList.Images.Add(bmp)
			item.ImageIndex = imageList.Images.Count - 1

		Next

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
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
				comboboxEdit.EditValue = Nothing
			ElseIf TypeOf sender Is CheckedComboBoxEdit Then
				Dim comboboxEdit As CheckedComboBoxEdit = CType(sender, CheckedComboBoxEdit)
				comboboxEdit.EditValue = Nothing
				comboboxEdit.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

			End If
		End If

	End Sub

#End Region


	''' <summary>
	''' Wraps an integer value.
	''' </summary>
	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class

	''' <summary>
	''' Combobox Color value view data.
	''' </summary>
	''' <remarks></remarks>
	Private Class ComboboxColorValueViewData
		Public Property ColorName As String
		Public Property RawValue As Decimal
		Public Color As System.Drawing.Color
	End Class


	Private Sub bbiClear_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiClear.ItemClick

		DxErrorProvider1.ClearErrors()

		Dim supressUIEventState = m_SuppressUIEvents

		m_SuppressUIEvents = True

		FormIsLoaded("frmSearch_LV", True)
		lueEmployee.EditValue = Nothing
		lueYear.EditValue = Nothing
		lueMonth.EditValue = Nothing

		Me.lueEmployee.EditValue = Nothing
		luePVL.EditValue = Nothing
		lueAbsence.EditValue = Nothing

		m_CurrentSUVAHourData = Nothing
		bbiPrint.Enabled = False

		LoadDropDownData()

		m_SuppressUIEvents = supressUIEventState

	End Sub

	Private Sub bbiPrint_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiPrint.ItemClick

		StartPrinting()

	End Sub

	Private Sub StartPrinting()
		Dim result As PrintResult
		Dim allowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 641, m_InitializationData.MDData.MDNr)
		Dim absenceCode As String = String.Empty

		SplashScreenManager.CloseForm(False)

		Dim showDesign As Boolean = allowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim filterConditions As New List(Of String)
		filterConditions.Add(String.Format("Mandant: {0}", m_InitializationData.MDData.MDName))
		If Not m_SearchCriteriums.Jahr Is Nothing Then filterConditions.Add(String.Format("Jahr: {0}", m_SearchCriteriums.Jahr))
		If Not m_SearchCriteriums.Monat Is Nothing Then filterConditions.Add(String.Format("Monat: {0}", m_SearchCriteriums.Monat))

		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmployeeNumbers.Count > 0) Then filterConditions.Add(String.Format("Kandidaten-Nummer: {0}", m_SearchCriteriums.EmployeeNumbers))
		If Not m_SearchCriteriums.EmploymentPVL Is Nothing Then filterConditions.Add(String.Format("GAV-Beruf: {0}, {1}", m_SearchCriteriums.EmploymentPVL, luePVL.Text))
		If Not lueAbsence.EditValue Is Nothing Then absenceCode = lueAbsence.EditValue
		absenceCode = absenceCode.Replace(" ", "")

		Dim _Setting As New SPS.Listing.Print.Utility.ReportPrint.SUVAHoursPrintData With {.ShowAsDesign = showDesign,
																																										 .FilterData = filterConditions,
																																										 .PrintJobNumber = "1.4",
																																										 .EmployeeHoursToPrintData = m_CurrentSUVAHourData,
																																										 .AbsenceDataForCalculatingDayCount = absenceCode,
																																										 .DynamicAbsenceListData = m_AbsenceListData,
																																										 .frmhwnd = Me.Handle}
		'_Setting.EmployeeHoursToPrintData = grdPrint.DataSource
		Dim obj As New SPS.Listing.Print.Utility.ReportPrint.PrintEmployeeSUVAHourData(m_InitializationData)
		obj.PrintData = _Setting

		result = obj.PrintEmployeeSUVAHourData

		obj.Dispose()

	End Sub


#Region "Helpers"

	Private Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlaufen
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function

	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

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
					Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
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



End Class



Public Class PreselectionData
	Public Property MDNr As Integer
	Public Property EmployeeNumbers As List(Of Integer?)
	Public Property ListYear As Integer?
	Public Property RechnungsDatum As DateTime?
	Public Property DebitorenArt As String
	Public Property CustomerNumber As Integer?
	Public Property InvoiceNumbers As List(Of Integer?)

End Class

