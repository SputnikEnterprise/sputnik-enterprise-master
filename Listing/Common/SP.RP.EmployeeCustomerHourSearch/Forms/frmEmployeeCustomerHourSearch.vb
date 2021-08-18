
Imports System.Reflection.Assembly
Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Reflection

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
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
Imports DevExpress.Utils
Imports DevExpress.XtraPrintingLinks
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports SPS.Listing.Print.Utility
Imports System.Text




Public Class frmHourSearch

	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The invoice data access object.
	''' </summary>
	Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

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

	Private m_SearchKindEnum As HourSearchTypeEnum
	Private m_SearchCriteriums As HourSearchData


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

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
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
		m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))


		TranslateControls()
		Reset()


		AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler cbFProperty.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueContactInfo.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueState1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueState2.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueEmployeeContactInfo.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployeeState1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployeeState2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployeeReserve1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployeeReserve2.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueKst1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueKst2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueAdvisorMA.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueESEinstufung.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueBranch.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region


#Region "public methodes"

	''' <summary>
	''' Preselects data.
	''' </summary>
	Public Sub PreselectData()

		Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

		If hasPreselectionData Then

			Dim supressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

			' ---Mandant---
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = PreselectionData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					m_SuppressUIEvents = supressUIEventState
					Return
				End If

			End If

			m_SuppressUIEvents = supressUIEventState
		Else
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = m_InitializationData.MDData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					Return
				End If

			End If

		End If

		Try
			If Not PreselectionData.InvoiceNumbers Is Nothing Then
				Dim invoiceNumbers As String = String.Empty
				For Each itm In PreselectionData.InvoiceNumbers
					invoiceNumbers &= If(Not String.IsNullOrWhiteSpace(invoiceNumbers), ",", String.Empty) & itm
				Next
				txtNumbers.EditValue = invoiceNumbers
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

		bbiPrint.Enabled = False
		bbiExport.Enabled = False

	End Sub


#End Region


#Region "private property"

	''' <summary>
	''' Gets the selected customer.
	''' </summary>
	''' <returns>The selected customer or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedCustomerRecord As CustomertReportHoursData
		Get
			Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim report = CType(gvRP.GetRow(selectedRows(0)), CustomertReportHoursData)

					Return report
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedEmployeeRecord As EmployeeReportHoursData
		Get
			Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim report = CType(gvRP.GetRow(selectedRows(0)), EmployeeReportHoursData)

					Return report
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


	Private Sub Reset()

		ResetMandantenDropDown()
		LoadMandantenDropDown()

		ResetDropDown()

		Me.txtNumbers.EditValue = Nothing

		sbHourDb.Value = True
		bbiPrint.Enabled = False
		bbiExport.Enabled = False

	End Sub

#Region "Lookup Edit Reset und Load..."

	Private Sub ResetDropDown()

		ResetMonthDropDown()
		ResetYearDropDown()
		ResetFirstPropertyDropDown()
		ResetCustomerContactDropDown()
		ResetCustomerStates1DropDown()
		ResetCustomerStates2DropDown()

		ResetEmployeeContactInfoDropDown()
		ResetEmployeeStates1DropDown()
		ResetEmployeeStates2DropDown()
		ResetReserveOneToTwoDropDowns()

		ResetKST1DropDown()
		ResetKST2DropDown()
		ResetAdvisorMADropDown()
		ResetESCategorizationDropDown()
		ResetBranchDropDown()
		ResetPVLDropDown()

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



#Region "customer"

	''' <summary>
	''' Resets the first property drop down.
	''' </summary>
	Private Sub ResetFirstPropertyDropDown()
		cbFProperty.SelectedItem = Nothing
	End Sub

	''' <summary>
	''' Resets the month drop down.
	''' </summary>
	Private Sub ResetCustomerContactDropDown()

		lueContactInfo.Properties.DisplayMember = "bez_d"
		lueContactInfo.Properties.ValueMember = "bez_value"
		lueContactInfo.Properties.ShowHeader = False

		lueContactInfo.Properties.Columns.Clear()
		lueContactInfo.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "bez_d",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("bez_d")})

		lueContactInfo.Properties.ShowFooter = False
		lueContactInfo.Properties.DropDownRows = 10
		lueContactInfo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueContactInfo.Properties.SearchMode = SearchMode.AutoComplete
		lueContactInfo.Properties.AutoSearchColumnIndex = 0

		lueContactInfo.Properties.NullText = String.Empty
		lueContactInfo.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets available customer states1 drop down.
	''' </summary>
	Public Sub ResetCustomerStates1DropDown()

		lueState1.Properties.DisplayMember = "bez_d"
		lueState1.Properties.ValueMember = "bez_value"
		lueState1.Properties.ShowHeader = False

		Dim columns = lueState1.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueState1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueState1.Properties.SearchMode = SearchMode.AutoComplete
		lueState1.Properties.AutoSearchColumnIndex = 0

		lueState1.Properties.NullText = String.Empty
		lueState1.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets available customer states2 drop down.
	''' </summary>
	Public Sub ResetCustomerStates2DropDown()

		lueState2.Properties.DisplayMember = "bez_d"
		lueState2.Properties.ValueMember = "bez_value"
		lueState2.Properties.ShowHeader = False

		Dim columns = lueState2.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueState2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueState2.Properties.SearchMode = SearchMode.AutoComplete
		lueState2.Properties.AutoSearchColumnIndex = 0

		lueState2.Properties.NullText = String.Empty
		lueState2.EditValue = Nothing

	End Sub


#End Region


#Region "employee"

	''' <summary>
	''' Resets contact info drop down.
	''' </summary>
	Private Sub ResetEmployeeContactInfoDropDown()
		lueEmployeeContactInfo.Properties.DisplayMember = "bez_d"
		lueEmployeeContactInfo.Properties.ValueMember = "bez_value"
		lueEmployeeContactInfo.Properties.ShowHeader = False

		Dim columns = lueEmployeeContactInfo.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueEmployeeContactInfo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployeeContactInfo.Properties.SearchMode = SearchMode.AutoComplete
		lueEmployeeContactInfo.Properties.AutoSearchColumnIndex = 0

		lueEmployeeContactInfo.Properties.NullText = String.Empty
		lueEmployeeContactInfo.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets employee states1 drop down.
	''' </summary>
	Private Sub ResetEmployeeStates1DropDown()

		lueEmployeeState1.Properties.DisplayMember = "bez_d"
		lueEmployeeState1.Properties.ValueMember = "bez_value"
		lueEmployeeState1.Properties.ShowHeader = False

		Dim columns = lueEmployeeState1.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueEmployeeState1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployeeState1.Properties.SearchMode = SearchMode.AutoComplete
		lueEmployeeState1.Properties.AutoSearchColumnIndex = 0

		lueEmployeeState1.Properties.NullText = String.Empty
		lueEmployeeState1.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets employee states2 drop down.
	''' </summary>
	Private Sub ResetEmployeeStates2DropDown()

		lueEmployeeState2.Properties.DisplayMember = "bez_d"
		lueEmployeeState2.Properties.ValueMember = "bez_value"
		lueEmployeeState2.Properties.ShowHeader = False

		Dim columns = lueEmployeeState2.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueEmployeeState2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployeeState2.Properties.SearchMode = SearchMode.AutoComplete
		lueEmployeeState2.Properties.AutoSearchColumnIndex = 0

		lueEmployeeState2.Properties.NullText = String.Empty
		lueEmployeeState2.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the reserve 1-2 drop downs
	''' </summary>
	Private Sub ResetReserveOneToTwoDropDowns()

		Dim controls As New List(Of LookUpEdit)
		controls.Add(lueEmployeeReserve1)
		controls.Add(lueEmployeeReserve2)
		lueEmployeeReserve1.Properties.ShowHeader = False
		lueEmployeeReserve2.Properties.ShowHeader = False

		For Each ctrl In controls
			ctrl.Properties.SearchMode = SearchMode.OnlyInPopup
			ctrl.Properties.TextEditStyle = TextEditStyles.Standard
			ctrl.Properties.ShowHeader = False
			ctrl.Properties.ShowFooter = False
			ctrl.Properties.DropDownRows = 20
			ctrl.Properties.DisplayMember = "bez_d"
			ctrl.Properties.ValueMember = "bez_value"

			Dim columns = ctrl.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			ctrl.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			ctrl.Properties.SearchMode = SearchMode.AutoComplete
			ctrl.Properties.AutoSearchColumnIndex = 0

			ctrl.Properties.NullText = String.Empty
			ctrl.EditValue = Nothing
		Next

	End Sub

#End Region


#Region "employment"

	''' <summary>
	''' Resets the KST1 drop down.
	''' </summary>
	Private Sub ResetKST1DropDown()

		lueKst1.Properties.DisplayMember = "kstbezeichnung"
		lueKst1.Properties.ValueMember = "kstname"
		lueKst1.Properties.ShowHeader = False

		Dim columns = lueKst1.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("kstbezeichnung", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueKst1.Properties.ShowFooter = False
		lueKst1.Properties.DropDownRows = 10
		lueKst1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueKst1.Properties.SearchMode = SearchMode.AutoComplete
		lueKst1.Properties.AutoSearchColumnIndex = 0

		lueKst1.Properties.NullText = String.Empty
		lueKst1.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the KST2 drop down.
	''' </summary>
	Private Sub ResetKST2DropDown()

		lueKst2.Properties.DisplayMember = "kstbezeichnung"
		lueKst2.Properties.ValueMember = "kstname"
		lueKst2.Properties.ShowHeader = False

		Dim columns = lueKst2.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("kstbezeichnung", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueKst2.Properties.ShowFooter = False
		lueKst2.Properties.DropDownRows = 10
		lueKst2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueKst2.Properties.SearchMode = SearchMode.AutoComplete
		lueKst2.Properties.AutoSearchColumnIndex = 0

		lueKst2.Properties.NullText = String.Empty
		lueKst2.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the advisor drop down.
	''' </summary>
	Private Sub ResetAdvisorMADropDown()

		lueAdvisorMA.Properties.DisplayMember = "UserFullname"
		lueAdvisorMA.Properties.ValueMember = "KST"
		lueAdvisorMA.Properties.ShowHeader = False

		Dim columns = lueAdvisorMA.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("Benutzer")))

		lueAdvisorMA.Properties.ShowFooter = False
		lueAdvisorMA.Properties.DropDownRows = 10
		lueAdvisorMA.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAdvisorMA.Properties.SearchMode = SearchMode.AutoComplete
		lueAdvisorMA.Properties.AutoSearchColumnIndex = 0

		lueAdvisorMA.Properties.NullText = String.Empty
		lueAdvisorMA.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the ES categorization drop down.
	''' </summary>
	Private Sub ResetESCategorizationDropDown()

		lueESEinstufung.Properties.DisplayMember = "bez_d"
		lueESEinstufung.Properties.ValueMember = "Description"

		Dim columns = lueESEinstufung.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Einstufung")))

		lueESEinstufung.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueESEinstufung.Properties.SearchMode = SearchMode.AutoComplete
		lueESEinstufung.Properties.AutoSearchColumnIndex = 0

		lueESEinstufung.Properties.NullText = String.Empty
		lueESEinstufung.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the branch drop down.
	''' </summary>
	Private Sub ResetBranchDropDown()

		lueBranch.Properties.DisplayMember = "bez_d"
		lueBranch.Properties.ValueMember = "Branche"

		' Reset the grid view
		gvLueBranch.OptionsView.ShowIndicator = False

		Dim columnBranchText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBranchText.Caption = m_Translate.GetSafeTranslationValue("Branchen")
		columnBranchText.Name = "bez_d"
		columnBranchText.FieldName = "bez_d"
		columnBranchText.Visible = True
		gvLueBranch.Columns.Add(columnBranchText)

		lueBranch.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBranch.Properties.NullText = String.Empty
		lueBranch.Properties.DataSource = Nothing
		lueBranch.EditValue = Nothing
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
	''' Resets customer grid.
	''' </summary>
	Private Sub ResetCustomerGrid()

		gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPrint.OptionsView.ShowIndicator = False
		gvPrint.OptionsBehavior.Editable = True
		gvPrint.OptionsView.ShowAutoFilterRow = True
		gvPrint.OptionsView.ColumnAutoWidth = True
		gvPrint.OptionsView.ShowFooter = True
		gvPrint.OptionsView.AllowHtmlDrawGroups = True

		gvPrint.Columns.Clear()


		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerNumber.OptionsColumn.AllowEdit = False
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Width = 60
		columnCustomerNumber.Visible = False
		gvPrint.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCompany1.OptionsColumn.AllowEdit = False
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.Width = 80
		gvPrint.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnStreet.OptionsColumn.AllowEdit = False
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.Width = 100
		gvPrint.Columns.Add(columnStreet)

		Dim columnCustomerPostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerPostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerPostcodeLocation.OptionsColumn.AllowEdit = False
		columnCustomerPostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("PLZ, Ort")
		columnCustomerPostcodeLocation.Name = "CustomerPostcodeLocation"
		columnCustomerPostcodeLocation.FieldName = "CustomerPostcodeLocation"
		columnCustomerPostcodeLocation.Visible = True
		columnCustomerPostcodeLocation.Width = 50
		gvPrint.Columns.Add(columnCustomerPostcodeLocation)

		Dim columnTotalHours As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTotalHours.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTotalHours.OptionsColumn.AllowEdit = False
		columnTotalHours.Caption = m_Translate.GetSafeTranslationValue("Total Stunden")
		columnTotalHours.Name = "TotalHours"
		columnTotalHours.FieldName = "TotalHours"
		columnTotalHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnTotalHours.AppearanceHeader.Options.UseTextOptions = True
		columnTotalHours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnTotalHours.DisplayFormat.FormatString = "N2"
		columnTotalHours.Width = 60
		columnTotalHours.Visible = True
		columnTotalHours.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnTotalHours.SummaryItem.DisplayFormat = "{0:n2}"
		gvPrint.Columns.Add(columnTotalHours)


		grdPrint.DataSource = Nothing

	End Sub

	Private Sub ResetEmployeeGrid()

		gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPrint.OptionsView.ShowIndicator = False
		gvPrint.OptionsBehavior.Editable = True
		gvPrint.OptionsView.ShowAutoFilterRow = True
		gvPrint.OptionsView.ColumnAutoWidth = True
		gvPrint.OptionsView.ShowFooter = True
		gvPrint.OptionsView.AllowHtmlDrawGroups = True

		gvPrint.Columns.Clear()


		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerNumber.OptionsColumn.AllowEdit = False
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnCustomerNumber.Name = "Employeenumber"
		columnCustomerNumber.FieldName = "Employeenumber"
		columnCustomerNumber.Width = 60
		columnCustomerNumber.Visible = False
		gvPrint.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCompany1.OptionsColumn.AllowEdit = False
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnCompany1.Name = "EmployeeLastnameFirstname"
		columnCompany1.FieldName = "EmployeeLastnameFirstname"
		columnCompany1.Visible = True
		columnCompany1.Width = 80
		gvPrint.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnStreet.OptionsColumn.AllowEdit = False
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.Width = 100
		gvPrint.Columns.Add(columnStreet)

		Dim columnCustomerPostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerPostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerPostcodeLocation.OptionsColumn.AllowEdit = False
		columnCustomerPostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnCustomerPostcodeLocation.Name = "EmployeePostcodeLocation"
		columnCustomerPostcodeLocation.FieldName = "EmployeePostcodeLocation"
		columnCustomerPostcodeLocation.Visible = True
		columnCustomerPostcodeLocation.Width = 50
		gvPrint.Columns.Add(columnCustomerPostcodeLocation)

		Dim columnTotalHours As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTotalHours.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnTotalHours.OptionsColumn.AllowEdit = False
		columnTotalHours.Caption = m_Translate.GetSafeTranslationValue("Total Stunden")
		columnTotalHours.Name = "TotalHours"
		columnTotalHours.FieldName = "TotalHours"
		columnTotalHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnTotalHours.AppearanceHeader.Options.UseTextOptions = True
		columnTotalHours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnTotalHours.DisplayFormat.FormatString = "N2"
		columnTotalHours.Width = 60
		columnTotalHours.Visible = True
		columnTotalHours.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnTotalHours.SummaryItem.DisplayFormat = "{0:n2}"
		gvPrint.Columns.Add(columnTotalHours)


		grdPrint.DataSource = Nothing

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

		If Not SelectedData Is Nothing Then
			Dim MandantData = ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			m_InitializationData = MandantData

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			LoadDropDownData()

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub

	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadDropDownData() As Boolean
		Dim success As Boolean = True

		m_SuppressUIEvents = True

		success = success AndAlso LoadYearDropDownData()
		success = success AndAlso LoadMonthDropDownData()
		success = success AndAlso LoadFirstPropertyDropwDownData()
		success = success AndAlso LoadCustomerContactInfoDropDownData()
		success = success AndAlso LoadCustomerStates1DropDownData()
		success = success AndAlso LoadCustomerStates2DropDownData()

		success = success AndAlso LoadEmployeeContactInfoDropDownData()
		success = success AndAlso LoadEmployeeStates1DropDownData()
		success = success AndAlso LoadEmployeeStates2DropDownData()
		success = success AndAlso LoadEmployeeStates2DropDownData()
		success = success AndAlso LoadReserveOneToTwoDropDownData()

		success = success AndAlso LoadEmploymentKST1DropDownData()
		success = success AndAlso LoadEmploymentKST2DropDownData()
		success = success AndAlso LoadEmploymentAdvisorDropDownData()
		success = success AndAlso LoadESCategorizationDropDownData()
		success = success AndAlso LoadEmploymentSectorDropDownData()
		success = success AndAlso LoadEmploymentPVLDropDownData()

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


#Region "loading customer data"

	''' <summary>
	''' Loads the frist property drop down data.
	''' </summary>
	Private Function LoadFirstPropertyDropwDownData() As Boolean
		Const WHITE_COLOR_WIN32_CODE As Integer = 16777215

		Dim firstPropertyData = m_ListingDatabaseAccess.LoadCustomerExistingFirstPropertyData(lueMandant.EditValue)

		If (firstPropertyData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten für 1. Eigenschaft konnten nicht geladen werden."))
		End If

		Dim listOfColors = New List(Of ComboboxColorValueViewData)

		If (Not firstPropertyData Is Nothing) Then

			' Convert the first property data to color values
			For Each fproperty In firstPropertyData

				Dim colorValue = New ComboboxColorValueViewData
				Dim rawColorValue As Decimal? = fproperty.FPropertyValue

				If Not rawColorValue.HasValue Then
					colorValue.ColorName = "Nicht definierte Farbe"
					colorValue.Color = Color.Transparent
				Else

					If String.IsNullOrEmpty(fproperty.Description) And
							Not rawColorValue = 0 Then

						' The color name is not in the database -> create the name from the Win32 color name.
						Dim strColorname = Regex.Split(ColorTranslator.FromWin32(rawColorValue).ToString(), " ")(1)
						strColorname = Microsoft.VisualBasic.Strings.Replace(strColorname, "[", "")
						strColorname = Microsoft.VisualBasic.Strings.Replace(strColorname, "]", "")
						colorValue.ColorName = strColorname
					Else
						colorValue.ColorName = fproperty.Description.Trim
					End If

					' Special case: Color 0 is transformed to white
					If (rawColorValue = 0) Then
						rawColorValue = WHITE_COLOR_WIN32_CODE
						colorValue.ColorName = "White"
					End If
					colorValue.RawValue = rawColorValue
					colorValue.Color = ColorTranslator.FromWin32(rawColorValue)
				End If

				listOfColors.Add(colorValue)

			Next

			FillImageComboxWithColorValues(cbFProperty, listOfColors)

		End If

		Return Not firstPropertyData Is Nothing
	End Function

	''' <summary>
	''' Load contact info drop down data.
	''' </summary>
	Private Function LoadCustomerContactInfoDropDownData() As Boolean
		Dim contactInfoData = m_ListingDatabaseAccess.LoadCustomerExistingContactInfo(lueMandant.EditValue)

		If (contactInfoData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Kontaktarten konnten nicht geladen werden."))
		End If

		lueContactInfo.Properties.DataSource = contactInfoData
		lueContactInfo.Properties.ForceInitialize()

		Return Not contactInfoData Is Nothing
	End Function

	''' <summary>
	''' Load customer states1 drop down data.
	''' </summary>
	Private Function LoadCustomerStates1DropDownData() As Boolean
		Dim availableCustomerStates1 = m_ListingDatabaseAccess.LoadCustomerExistingStateData1(lueMandant.EditValue)

		If (availableCustomerStates1 Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Status1 konnten nicht geladen werden."))
		End If

		lueState1.Properties.DataSource = availableCustomerStates1
		lueState1.Properties.ForceInitialize()

		Return Not availableCustomerStates1 Is Nothing
	End Function

	''' <summary>
	''' Load customer states2 drop down data.
	''' </summary>
	Private Function LoadCustomerStates2DropDownData() As Boolean
		Dim availableCustomerStates2 = m_ListingDatabaseAccess.LoadCustomerExistingStateData2(lueMandant.EditValue)

		If (availableCustomerStates2 Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Status2 konnten nicht geladen werden."))
		End If

		lueState2.Properties.DataSource = availableCustomerStates2
		lueState2.Properties.ForceInitialize()

		Return Not availableCustomerStates2 Is Nothing
	End Function

#End Region


#Region "loading employee data"

	''' <summary>
	''' Load contact info drop down data.
	''' </summary>
	Private Function LoadEmployeeContactInfoDropDownData() As Boolean
		Dim contactInfoData = m_ListingDatabaseAccess.LoadEmployeeExistingContactsInfo(lueMandant.EditValue)

		If (contactInfoData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Kontaktarten konnten nicht geladen werden."))
		End If

		lueEmployeeContactInfo.Properties.DataSource = contactInfoData
		lueEmployeeContactInfo.Properties.ForceInitialize()

		Return Not contactInfoData Is Nothing
	End Function

	''' <summary>
	''' Load customer states1 drop down data.
	''' </summary>
	Private Function LoadEmployeeStates1DropDownData() As Boolean
		Dim availableCustomerStates1 = m_ListingDatabaseAccess.LoadEmployeeExistingStateData1(lueMandant.EditValue)

		If (availableCustomerStates1 Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Status1 konnten nicht geladen werden."))
		End If

		lueEmployeeState1.Properties.DataSource = availableCustomerStates1
		lueEmployeeState1.Properties.ForceInitialize()

		Return Not availableCustomerStates1 Is Nothing
	End Function

	''' <summary>
	''' Load customer states2 drop down data.
	''' </summary>
	Private Function LoadEmployeeStates2DropDownData() As Boolean
		Dim availableCustomerStates2 = m_ListingDatabaseAccess.LoadEmployeeExistingStateData2(lueMandant.EditValue)

		If (availableCustomerStates2 Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Status2 konnten nicht geladen werden."))
		End If

		lueEmployeeState2.Properties.DataSource = availableCustomerStates2
		lueEmployeeState2.Properties.ForceInitialize()

		Return Not availableCustomerStates2 Is Nothing
	End Function

	''' <summary>
	''' Loads the reserve one to four drop down data.
	''' </summary>
	Private Function LoadReserveOneToTwoDropDownData() As Boolean
		Dim reserve1Data = m_ListingDatabaseAccess.LoadEmployeeExistingContactReserveData(lueMandant.EditValue, 1)
		Dim reserve2Data = m_ListingDatabaseAccess.LoadEmployeeExistingContactReserveData(lueMandant.EditValue, 2)

		Dim success As Boolean = True
		If (reserve1Data Is Nothing OrElse reserve2Data Is Nothing) Then
			success = False
			m_UtilityUI.ShowErrorDialog("Reservedaten (1-2) konnten nicht vollständig geladen werden.")
		End If

		lueEmployeeReserve1.Properties.DataSource = reserve1Data
		lueEmployeeReserve1.Properties.ForceInitialize()

		lueEmployeeReserve2.Properties.DataSource = reserve2Data
		lueEmployeeReserve2.Properties.ForceInitialize()

		Return success
	End Function

#End Region


#Region "loading employment data"

	''' <summary>
	''' Load kst1 drop down data.
	''' </summary>
	Private Function LoadEmploymentKST1DropDownData() As Boolean
		Dim data = m_ListingDatabaseAccess.LoadEmploymentExistingCostCenter1Data(lueMandant.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("1. Kostenstellen konnten nicht geladen werden."))
		End If

		lueKst1.Properties.DataSource = data
		lueKst1.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Load kst2 drop down data.
	''' </summary>
	Private Function LoadEmploymentKST2DropDownData() As Boolean
		Dim data = m_ListingDatabaseAccess.LoadEmploymentExistingCostCenter2Data(lueMandant.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("2. Kostenstellen konnten nicht geladen werden."))
		End If

		lueKst2.Properties.DataSource = data
		lueKst2.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Load advisor drop down data.
	''' </summary>
	Private Function LoadEmploymentAdvisorDropDownData() As Boolean
		Dim data = m_ListingDatabaseAccess.LoadEmploymentExistingAdvisorData(lueMandant.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("BeraterInnen konnten nicht geladen werden."))
		End If

		lueAdvisorMA.Properties.DataSource = data
		lueAdvisorMA.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Load einstufung drop down data.
	''' </summary>
	Private Function LoadESCategorizationDropDownData() As Boolean
		Dim data = m_ListingDatabaseAccess.LoadEmploymentExistingCategorizationData(lueMandant.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("ES-Einstufungen konnten nicht geladen werden."))
		End If

		lueESEinstufung.Properties.DataSource = data
		lueESEinstufung.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Load einstufung drop down data.
	''' </summary>
	Private Function LoadEmploymentSectorDropDownData() As Boolean
		Dim data = m_ListingDatabaseAccess.LoadEmploymentExistingSectorData(lueMandant.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("ES-Branchen konnten nicht geladen werden."))
		End If

		lueBranch.Properties.DataSource = data

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

#End Region



#End Region

	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		sbHourDb.OffText = m_Translate.GetSafeTranslationValue(sbHourDb.OffText)
		sbHourDb.OnText = m_Translate.GetSafeTranslationValue(sbHourDb.OnText)

		lblNumbers.Text = m_Translate.GetSafeTranslationValue(lblNumbers.Text)
		lblJahr.Text = m_Translate.GetSafeTranslationValue(lblJahr.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)

		gpCustomerKriterien.Text = m_Translate.GetSafeTranslationValue(gpCustomerKriterien.Text)
		lblFProperty.Text = m_Translate.GetSafeTranslationValue(lblFProperty.Text, True)
		lblKontakt.Text = m_Translate.GetSafeTranslationValue(lblKontakt.Text, True)
		lbl1Status.Text = m_Translate.GetSafeTranslationValue(lbl1Status.Text, True)
		lbl2Status.Text = m_Translate.GetSafeTranslationValue(lbl2Status.Text, True)

		gpEmploymentKriterien.Text = m_Translate.GetSafeTranslationValue(gpEmploymentKriterien.Text)
		lbl1KST.Text = m_Translate.GetSafeTranslationValue(lbl1KST.Text, True)
		lbl2KST.Text = m_Translate.GetSafeTranslationValue(lbl2KST.Text, True)
		lblBerater.Text = m_Translate.GetSafeTranslationValue(lblBerater.Text, True)
		lblESEinstufung.Text = m_Translate.GetSafeTranslationValue(lblESEinstufung.Text, True)
		lblBranche.Text = m_Translate.GetSafeTranslationValue(lblBranche.Text)
		lblPVLBeruf.Text = m_Translate.GetSafeTranslationValue(lblPVLBeruf.Text)

		gpEmployeeKriterien.Text = m_Translate.GetSafeTranslationValue(gpEmployeeKriterien.Text)
		lblMAKontakt.Text = m_Translate.GetSafeTranslationValue(lblMAKontakt.Text, True)
		lblMA1State.Text = m_Translate.GetSafeTranslationValue(lblMA1State.Text, True)
		lblMA2State.Text = m_Translate.GetSafeTranslationValue(lblMA2State.Text, True)
		lblMA1Res.Text = m_Translate.GetSafeTranslationValue(lblMA1Res.Text, True)
		lblMA2Res.Text = m_Translate.GetSafeTranslationValue(lblMA2Res.Text, True)

		lbCostcenterInfo.Text = m_Translate.GetSafeTranslationValue(lbCostcenterInfo.Text)

		xtabAllegemein.Text = m_Translate.GetSafeTranslationValue(xtabAllegemein.Text)
		xtabResultat.Text = m_Translate.GetSafeTranslationValue(xtabResultat.Text)

		Me.bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiPrintinfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)

	End Sub

#Region "Formhandle"

	Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
		Me.Dispose()
	End Sub

	Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		SplashScreenManager.CloseForm(False)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

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

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
		End Try

	End Sub


#End Region

	Private Sub bbiSearch_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSearch.ItemClick

		If LoadSearchKonditions() Then
			If m_SearchKindEnum = HourSearchTypeEnum.Customer Then
				ResetCustomerGrid()
				LoadCurstomerHourGrid()

			ElseIf m_SearchKindEnum = HourSearchTypeEnum.Employee Then
				ResetEmployeeGrid()
				LoadEmployeeHourGrid()

			End If
			xtabCtlMain.SelectedTabPageIndex = If(gvPrint.RowCount > 0, 1, 0)
		End If

	End Sub

	Private Function LoadSearchKonditions() As Boolean
		Dim searchText As String

		m_SearchCriteriums = Nothing
		Dim searchData = New SP.DatabaseAccess.Listing.DataObjects.HourSearchData
		m_SearchKindEnum = If(sbHourDb.Value, HourSearchTypeEnum.Customer, HourSearchTypeEnum.Employee)

		searchData.MDNr = lueMandant.EditValue
		searchData.Numbers = GetFormatedNumber()
		searchData.Monat = lueMonth.EditValue
		searchData.Jahr = lueYear.EditValue

		searchData.CustomerFProperty = cbFProperty.EditValue
		searchData.CustomerContact = lueContactInfo.EditValue
		searchData.CustomerState1 = lueState1.EditValue
		searchData.CustomerState2 = lueState2.EditValue

		searchData.EmployeeContact = lueEmployeeContactInfo.EditValue
		searchData.EmployeeState1 = lueEmployeeState1.EditValue
		searchData.EmployeeState2 = lueEmployeeState2.EditValue
		searchData.EmployeeReserve1 = lueEmployeeReserve1.EditValue
		searchData.EmployeeReserve2 = lueEmployeeReserve2.EditValue

		searchData.EmploymentKst1 = CType(lueKst1.EditValue, String)
		searchData.EmploymentKst2 = CType(lueKst2.EditValue, String)
		searchData.EmploymentAdvisor = CType(lueAdvisorMA.EditValue, String)
		searchData.EmploymentESCategorize = lueESEinstufung.EditValue
		searchData.EmploymentBranch = lueBranch.EditValue
		searchData.EmploymentPVL = luePVL.EditValue

		m_SearchCriteriums = searchData
		searchText = String.Format("m_SearchKindEnum: {0} | ", m_SearchKindEnum)
		searchText &= String.Format("MDNr: {0} | ", searchData.MDNr)
		searchText &= String.Format("Numbers:  {0} | ", searchData.Numbers)
		searchText &= String.Format("Monat/Jahr: {0} / {1} | ", searchData.Monat, searchData.Jahr)
		searchText &= String.Format("CustomerFProperty:  {0} | ", searchData.CustomerFProperty)
		searchText &= String.Format("CustomerContact: {0} | ", searchData.CustomerContact)
		searchText &= String.Format("CustomerState1:  {0} | ", searchData.CustomerState1)
		searchText &= String.Format("CustomerState2: {0} | ", searchData.CustomerState2)
		searchText &= String.Format("EmployeeContact:  {0} | ", searchData.EmployeeContact)
		searchText &= String.Format("EmployeeState1: {0} | ", searchData.EmployeeState1)
		searchText &= String.Format("EmployeeState2:  {0} | ", searchData.EmployeeState2)
		searchText &= String.Format("EmployeeReserve1: {0} | ", searchData.EmployeeReserve1)
		searchText &= String.Format("EmployeeReserve2  {0} | ", searchData.EmployeeReserve2)
		searchText &= String.Format("EmploymentKst1 {0} | ", searchData.EmploymentKst1)
		searchText &= String.Format("EmploymentKst2  {0} | ", searchData.EmploymentKst2)
		searchText &= String.Format("EmploymentAdvisor {0} | ", searchData.EmploymentAdvisor)
		searchText &= String.Format("EmploymentESCategorize  {0} | ", searchData.EmploymentESCategorize)
		searchText &= String.Format("EmploymentBranch: {0} | ", searchData.EmploymentBranch)
		searchText &= String.Format("EmploymentPVL:  {0} ", searchData.EmploymentPVL)

		m_Logger.LogInfo(String.Format("SP.RP.EmployeeCustomerHourSearch-WhereQuery: {0}", searchText))


		Return Not m_SearchCriteriums Is Nothing

	End Function

	Private Sub LoadCurstomerHourGrid()

		Dim foundedData = LoadCustomerSearchResultData(m_SearchKindEnum)
		If (foundedData Is Nothing) Then Return

		Dim gridData = (From person In foundedData
										Select New CustomertReportHoursData With {.MDNr = person.MDNr,
																															.CustomerNumber = person.CustomerNumber,
																															.Company1 = person.Company1,
																															.Company2 = person.Company2,
																															.Company3 = person.Company3,
																															.Street = person.Street,
																															.Postcode = person.Postcode,
																															.PostOfficeBox = person.PostOfficeBox,
																															.Location = person.Location,
																															.CountryCode = person.CountryCode,
																															.EMail = person.EMail,
																															.Telefax = person.Telefax,
																															.Telephone = person.Telephone,
																															.TotalHours = person.TotalHours,
																															.FirstProperty = person.FirstProperty
																														 }).ToList()

		Dim listDataSource As BindingList(Of CustomertReportHoursData) = New BindingList(Of CustomertReportHoursData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		bbiPrint.Enabled = listDataSource.Count > 0
		bbiExport.Enabled = listDataSource.Count > 0

		grdPrint.DataSource = listDataSource

	End Sub

	Private Sub LoadEmployeeHourGrid()

		Dim foundedData = LoadEmployeeSearchResultData(m_SearchKindEnum)
		If (foundedData Is Nothing) Then Return

		Dim gridData = (From person In foundedData
										Select New EmployeeReportHoursData With {.MDNr = person.MDNr,
																														 .Employeenumber = person.Employeenumber,
																														 .Lastname = person.Lastname,
																														 .Firstname = person.Firstname,
																														 .Street = person.Street,
																														 .Postcode = person.Postcode,
																														 .PostOfficeBox = person.PostOfficeBox,
																														 .Location = person.Location,
																														 .CountryCode = person.CountryCode,
																														 .EMail = person.EMail,
																														 .Mobile = person.Mobile,
																														 .Telephone_P = person.Telephone_P,
																														 .Telephone_2 = person.Telephone_2,
																														 .Telephone_3 = person.Telephone_3,
																														 .Telephone_G = person.Telephone_G,
																														 .TotalHours = person.TotalHours
																														}).ToList()

		Dim listDataSource As BindingList(Of EmployeeReportHoursData) = New BindingList(Of EmployeeReportHoursData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		bbiPrint.Enabled = listDataSource.Count > 0
		bbiExport.Enabled = listDataSource.Count > 0

		grdPrint.DataSource = listDataSource

	End Sub

	Private Function LoadCustomerSearchResultData(ByVal searchKind As HourSearchTypeEnum) As IEnumerable(Of CustomertReportHoursData)
		Dim data = m_ListingDatabaseAccess.SearchCustomerHoursReportlineData(lueMandant.EditValue, m_SearchKindEnum, m_InitializationData.UserData.UserFiliale, m_SearchCriteriums)

		If (data Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht geladen werden."))
		End If

		Return data
	End Function

	Private Function LoadEmployeeSearchResultData(ByVal searchKind As HourSearchTypeEnum) As IEnumerable(Of EmployeeReportHoursData)
		Dim data = m_ListingDatabaseAccess.SearchEmployeeHoursReportlineData(lueMandant.EditValue, m_SearchKindEnum, m_InitializationData.UserData.UserFiliale, m_SearchCriteriums)

		If (data Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht geladen werden."))
		End If

		Return data
	End Function

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub OngvPrint_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvPrint.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPrint.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, CustomertReportHoursData)
				If viewData.CustomerNumber > 0 Then OpenSelectedcustomer(viewData.CustomerNumber)
			End If

		End If

	End Sub

	Private Sub OngvPrint_MasterRowGetRelationCount(ByVal sender As Object, ByVal e As MasterRowGetRelationCountEventArgs) Handles gvPrint.MasterRowGetRelationCount
		e.RelationCount = 1
	End Sub

	Private Sub OngvPrint_MasterRowGetRelationName(ByVal sender As Object, ByVal e As MasterRowGetRelationNameEventArgs) Handles gvPrint.MasterRowGetRelationName
		e.RelationName = "Rapporte"
	End Sub

	Private Sub OngvPrint_MasterRowEmpty(ByVal sender As Object, ByVal e As MasterRowEmptyEventArgs) Handles gvPrint.MasterRowEmpty
		e.IsEmpty = False
	End Sub

	Private Sub OngvPrint_MasterRowGetChildList(ByVal sender As Object, ByVal e As MasterRowGetChildListEventArgs) Handles gvPrint.MasterRowGetChildList
		Dim listOfInvoiceData As IEnumerable(Of ReportlineHoursData) = Nothing

		If m_SearchKindEnum = HourSearchTypeEnum.Customer Then
			Dim customerData = SelectedCustomerRecord
			listOfInvoiceData = m_ListingDatabaseAccess.SearchAssignedCustomerHoursReportlineData(lueMandant.EditValue, customerData.CustomerNumber, m_InitializationData.UserData.UserFiliale, m_SearchCriteriums)

		ElseIf m_SearchKindEnum = HourSearchTypeEnum.Employee Then
			Dim employeeData = SelectedEmployeeRecord
			listOfInvoiceData = m_ListingDatabaseAccess.SearchAssignedEmployeeHoursReportlineData(lueMandant.EditValue, employeeData.Employeenumber, m_InitializationData.UserData.UserFiliale, m_SearchCriteriums)

		End If
		If listOfInvoiceData Is Nothing Then Return

		e.ChildList = listOfInvoiceData

		Dim gridData = (From person In listOfInvoiceData
										Select New ReportlineHoursData With {.MDNr = person.MDNr,
																												 .ReportNumber = person.ReportNumber,
																												 .BisDate = person.BisDate,
																												 .CountHour = person.CountHour,
																												 .Einstufung = person.Einstufung,
																												 .Jahr = person.Jahr,
																												 .Monat = person.Monat,
																												 .RPKst = person.RPKst,
																												 .RPKst1 = person.RPKst1,
																												 .RPKst2 = person.RPKst2,
																												 .VonDate = person.VonDate
																												}).ToList()

		Dim listDataSource As BindingList(Of ReportlineHoursData) = New BindingList(Of ReportlineHoursData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

	End Sub

	Private Sub gvPrint_MasterRowExpanded(sender As Object, e As CustomMasterRowEventArgs) Handles gvPrint.MasterRowExpanded
		Dim view As GridView = TryCast(TryCast(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)

		view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		view.OptionsView.ShowIndicator = False
		view.OptionsBehavior.Editable = False
		view.OptionsView.ShowAutoFilterRow = True
		view.OptionsView.ColumnAutoWidth = False
		view.OptionsView.ShowFooter = True
		view.OptionsView.AllowHtmlDrawGroups = True

		view.Columns.Clear()

		Dim columnRENr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRENr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRENr.OptionsColumn.AllowEdit = False
		columnRENr.Caption = m_Translate.GetSafeTranslationValue("Report-Nr.")
		columnRENr.Name = "ReportNumber"
		columnRENr.FieldName = "ReportNumber"
		columnRENr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnRENr.AppearanceHeader.Options.UseTextOptions = True
		columnRENr.Width = 100
		columnRENr.Visible = True
		view.Columns.Add(columnRENr)

		Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFakDat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFakDat.OptionsColumn.AllowEdit = False
		columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Monat / Jahr")
		columnFakDat.Name = "ReportMonthYear"
		columnFakDat.FieldName = "ReportMonthYear"
		columnFakDat.Visible = False
		columnFakDat.Width = 100
		view.Columns.Add(columnFakDat)

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLANr.OptionsColumn.AllowEdit = False
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
		columnLANr.Name = "LANr"
		columnLANr.FieldName = "LANr"
		columnLANr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnLANr.AppearanceHeader.Options.UseTextOptions = True
		columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnLANr.DisplayFormat.FormatString = "N3"
		columnLANr.Width = 100
		columnLANr.Visible = True
		view.Columns.Add(columnLANr)

		Dim columnLALOText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLALOText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLALOText.OptionsColumn.AllowEdit = False
		columnLALOText.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnLALOText.Name = "LALOText"
		columnLALOText.FieldName = "LALOText"
		columnLALOText.Visible = True
		columnLALOText.Width = 200
		view.Columns.Add(columnLALOText)

		Dim columnCustomerReportLineFromTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerReportLineFromTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerReportLineFromTo.OptionsColumn.AllowEdit = False
		columnCustomerReportLineFromTo.Caption = m_Translate.GetSafeTranslationValue("Von - Bis")
		columnCustomerReportLineFromTo.Name = "ReportLineFromTo"
		columnCustomerReportLineFromTo.FieldName = "ReportLineFromTo"
		columnCustomerReportLineFromTo.Visible = True
		columnCustomerReportLineFromTo.Width = 150
		view.Columns.Add(columnCustomerReportLineFromTo)

		Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBetragInk.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBetragInk.OptionsColumn.AllowEdit = False
		columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Stunden")
		columnBetragInk.Name = "CountHour"
		columnBetragInk.FieldName = "CountHour"
		columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
		columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBetragInk.DisplayFormat.FormatString = "N2"
		columnBetragInk.Width = 100
		columnBetragInk.Visible = True
		columnBetragInk.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnBetragInk.SummaryItem.DisplayFormat = "{0:n2}"
		view.Columns.Add(columnBetragInk)

		Dim columnKST As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKST.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKST.OptionsColumn.AllowEdit = False
		columnKST.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
		columnKST.Name = "RPKst"
		columnKST.FieldName = "RPKst"
		columnKST.Visible = True
		columnKST.Width = 100
		view.Columns.Add(columnKST)


		Dim detailView As GridView = CType(CType(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
		RemoveHandler detailView.RowCellClick, AddressOf OngvDetail_RowCellClick
		If (Not (detailView) Is Nothing) Then
			'detailView.ParentView
			AddHandler detailView.RowCellClick, AddressOf OngvDetail_RowCellClick
		End If

	End Sub

	Private Sub OpenSelectedcustomer(ByVal customerNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customerNumber)
			hub.Publish(openMng)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub OngvDetail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim detailView As GridView = CType(sender, GridView)
			If (Not (detailView) Is Nothing) Then
				Dim dataRow = detailView.GetRow(e.RowHandle)
				Dim viewData = CType(dataRow, ReportlineHoursData)
				If viewData.ReportNumber > 0 Then OpenSelectedreport(viewData.ReportNumber)
			End If
		End If

	End Sub


#Region "Helpers"

	Private Function GetFormatedNumber() As List(Of Integer)

		Dim numbers As String = txtNumbers.EditValue
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
			ElseIf TypeOf sender Is GridLookUpEdit Then
				Dim grdlookupEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
				grdlookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
				comboboxEdit.EditValue = Nothing
			End If
		End If
	End Sub

	Private Sub sbHourDb_ValueChanged(sender As Object, e As EventArgs) Handles sbHourDb.ValueChanged

		gpCustomerKriterien.Visible = sbHourDb.Value

		gpEmployeeKriterien.Left = gpCustomerKriterien.Left
		gpEmployeeKriterien.Top = gpCustomerKriterien.Top
		gpEmployeeKriterien.Visible = Not sbHourDb.Value

		lblNumbers.Text = String.Format(m_Translate.GetSafeTranslationValue("{0}-Nummer"), m_Translate.GetSafeTranslationValue(If(sbHourDb.Value, "Kunden", "Kandidaten")))

	End Sub

	Sub OpenSelectedreport(ByVal reportNumber As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, reportNumber)
		hub.Publish(openMng)

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

		ResetDropDown()

		LoadDropDownData()
		Me.txtNumbers.EditValue = Nothing

		xtabCtlMain.SelectedTabPageIndex = 0
		grdPrint.DataSource = Nothing
		bbiPrint.Enabled = False
		bbiExport.Enabled = False

	End Sub

	Private Sub bbiPrint_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiPrint.ItemClick

		StartPrinting()

	End Sub

	Private Sub StartPrinting()
		Dim result As PrintResult
		Dim allowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 641, m_InitializationData.MDData.MDNr)

		Dim showDesign As Boolean = allowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim filterConditions As New List(Of String)
		filterConditions.Add(String.Format("Mandant: {0}", m_InitializationData.MDData.MDName))
		If Not m_SearchCriteriums.Jahr Is Nothing Then filterConditions.Add(String.Format("Jahr: {0}", m_SearchCriteriums.Jahr))
		If Not m_SearchCriteriums.Monat Is Nothing Then filterConditions.Add(String.Format("Monat: {0}", m_SearchCriteriums.Monat))

		If Not m_SearchCriteriums.CustomerFProperty Is Nothing Then filterConditions.Add(String.Format("1. Eigenschaft: {0}", m_SearchCriteriums.CustomerFProperty))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.CustomerContact) Then filterConditions.Add(String.Format("Kundenkontakt: {0}", m_SearchCriteriums.CustomerContact))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.CustomerState1) Then filterConditions.Add(String.Format("Kunden 1. Status: {0}", m_SearchCriteriums.CustomerState1))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.CustomerState2) Then filterConditions.Add(String.Format("Kunden 2. Status: {0}", m_SearchCriteriums.CustomerState2))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmployeeContact) Then filterConditions.Add(String.Format("Kandidatenkontakt: {0}", m_SearchCriteriums.EmployeeContact))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmployeeState1) Then filterConditions.Add(String.Format("Kandidaten 1. Status: {0}", m_SearchCriteriums.EmployeeState1))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmployeeState2) Then filterConditions.Add(String.Format("Kandidaten 2. Status: {0}", m_SearchCriteriums.EmployeeState2))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmployeeReserve1) Then filterConditions.Add(String.Format("Kandidaten 1. Reserve: {0}", m_SearchCriteriums.EmployeeReserve1))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmployeeReserve2) Then filterConditions.Add(String.Format("Kandidaten 2. Reserve: {0}", m_SearchCriteriums.EmployeeReserve2))

		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmploymentKst1) Then filterConditions.Add(String.Format("Einsatz 1. KST: {0}", m_SearchCriteriums.EmploymentKst1))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmploymentKst2) Then filterConditions.Add(String.Format("Einsatz 2. KST: {0}", m_SearchCriteriums.EmploymentKst2))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmploymentAdvisor) Then filterConditions.Add(String.Format("Einsatz BeraterIn: {0}", m_SearchCriteriums.EmploymentAdvisor))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmploymentESCategorize) Then filterConditions.Add(String.Format("Einsatz Einstufung: {0}", m_SearchCriteriums.EmploymentESCategorize))
		If Not String.IsNullOrWhiteSpace(m_SearchCriteriums.EmploymentBranch) Then filterConditions.Add(String.Format("Einsatz Branche: {0}", m_SearchCriteriums.EmploymentBranch))
		If Not m_SearchCriteriums.EmploymentPVL Is Nothing Then filterConditions.Add(String.Format("GAV-Beruf: {0}, {1}", m_SearchCriteriums.EmploymentPVL, luePVL.Text))

		Dim _Setting As New SPS.Listing.Print.Utility.ReportPrint.HoursPrintData With {.ShowAsDesign = showDesign,
																																									 .FilterData = filterConditions,
																																									 .SearchKindEnum = m_SearchKindEnum,
																																									 .PrintJobNumber = If(m_SearchKindEnum = HourSearchTypeEnum.Customer, "2.3.2", "1.3.2")}
		If m_SearchKindEnum = HourSearchTypeEnum.Customer Then
			_Setting.CustomerHoursToPrintData = grdPrint.DataSource
		Else
			_Setting.EmployeeHoursToPrintData = grdPrint.DataSource
		End If
		Dim obj As New SPS.Listing.Print.Utility.ReportPrint.ClsPrintingReportHours(m_InitializationData)
		obj.PrintData = _Setting

		result = obj.PrintCustomerHours

	End Sub



#Region "bbExport"

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																					 .SQL2Open = String.Empty,
																																					 .ModulName = If(m_SearchKindEnum = HourSearchTypeEnum.Customer, "CustomerHourListing", "EmployeeHourListing")}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		If m_SearchKindEnum = HourSearchTypeEnum.Customer Then
			obj.ExportCSVFromCustomerHourData(grdPrint.DataSource)
		Else
			obj.ExportCSVFromEmployeeHourData(grdPrint.DataSource)
		End If


	End Sub

#End Region




End Class


Public Class PreselectionData
	Public Property MDNr As Integer
	Public Property BeraterMA As String
	Public Property BeraterKD As String
	Public Property RechnungsDatum As DateTime?
	Public Property DebitorenArt As String
	Public Property CustomerNumber As Integer?
	Public Property InvoiceNumbers As List(Of Integer?)

End Class

