

Imports System.ComponentModel
Imports System.IO
Imports DevExpress.Export.Xl
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraBars.Navigation
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SPS.Listing.Print.Utility

Namespace UI


	Public Class frmKAEListing


#Region "Private Consts"

		Private Const MODUL_NAME_SETTING = "downtimesearch"

		Private Const USER_XML_SETTING_SPUTNIK_DOWNTIME_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/downtimesearch/{1}/restorelayoutfromxml"
		Private Const USER_XML_SETTING_SPUTNIK_DOWNTIME_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/downtimesearch/{1}/keepfilter"

#End Region

#Region "private fields"


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
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess


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
		Private m_DowntimeData As BindingList(Of DowntimeData)


		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connectionString As String

		Private m_userGridSettingsXml As SettingsXml
		Private m_GVSearchResultSettingfilename As String
		Private m_GridSettingPath As String
		Private m_xmlSettingRestoreSearchSetting As String
		Private m_xmlSettingSearchFilter As String


		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Protected m_SuppressUIEvents As Boolean = False

#End Region


#Region "public property"
		''' <summary>
		''' Gets or sets the preselection data.
		''' </summary>
		Public Property PreselectionData As PreselectionKAEData

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

			Dim xmlFilename = m_mandant.GetAllUserGridSettingXMLFilename(m_InitializationData.MDData.MDNr)
			m_userGridSettingsXml = New SettingsXml(xmlFilename)

			m_GridSettingPath = Path.Combine(m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr), "DowntimeSearch")
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)
			m_GVSearchResultSettingfilename = Path.Combine(m_GridSettingPath, String.Format("{0}{1}.xml", Me.grdPrint.Name, m_InitializationData.UserData.UserNr))

			m_xmlSettingRestoreSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_DOWNTIME_SEARCH_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_DOWNTIME_SEARCH_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)

			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			m_connectionString = m_InitializationData.MDData.MDDbConn

			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			TranslateControls()
			Reset()
			acMain.OptionsFooter.ActiveGroupDisplayMode = DevExpress.XtraBars.Navigation.ActiveGroupDisplayMode.GroupHeaderAndContent


			AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler gvPrint.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvPrint.RowCountChanged, AddressOf OngvMain_RowCountChanged
			AddHandler Me.gvPrint.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvPrint.ColumnWidthChanged, AddressOf OngvColumnPositionChanged


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
				m_SuppressUIEvents = True ' Make sure UI event are fired so that the lookup data is loaded correctly.

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
				LoadDropDownData()

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = True

				If Not PreselectionData.Month = 0 Then
					lueMonth.EditValue = PreselectionData.Month
				Else
					lueMonth.EditValue = Now.Month
				End If

				If Not PreselectionData.Year = 0 Then
					lueYear.EditValue = PreselectionData.Year
				Else
					lueYear.EditValue = Now.Year
				End If

				If Not PreselectionData.CustomerNumber Is Nothing Then
					lueCustomer.EditValue = PreselectionData.CustomerNumber
				End If
				LoadDowntimeDataGrid()


				m_SuppressUIEvents = supressUIEventState


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try


		End Sub


#End Region


		Private Sub Reset()

			ResetMandantenDropDown()
			LoadMandantenDropDown()

			ResetDropDown()

		End Sub

#Region "reset"

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

		Private Sub ResetDropDown()

			ResetMonthDropDown()
			ResetYearDropDown()
			ResetCustomerDropDown()

			ResetDowntimeGrid()

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

		Private Sub ResetDowntimeGrid()

			gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvPrint.OptionsView.ShowIndicator = False
			gvPrint.OptionsBehavior.Editable = True
			gvPrint.OptionsView.ShowAutoFilterRow = True
			gvPrint.OptionsView.ColumnAutoWidth = True
			gvPrint.OptionsView.ShowFooter = True
			gvPrint.OptionsView.AllowHtmlDrawGroups = True

			gvPrint.Columns.Clear()


			Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMDNr.OptionsColumn.AllowEdit = False
			columnMDNr.Caption = "MDNr"
			columnMDNr.Name = "MDNr"
			columnMDNr.FieldName = "MDNr"
			columnMDNr.Width = 60
			columnMDNr.Visible = False
			gvPrint.Columns.Add(columnMDNr)

			Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeNumber.OptionsColumn.AllowEdit = False
			columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnEmployeeNumber.Name = "EmployeeNumber"
			columnEmployeeNumber.FieldName = "EmployeeNumber"
			columnEmployeeNumber.Width = 60
			columnEmployeeNumber.Visible = False
			gvPrint.Columns.Add(columnEmployeeNumber)

			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerNumber.OptionsColumn.AllowEdit = False
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnCustomerNumber.Name = "CustomerNumber"
			columnCustomerNumber.FieldName = "CustomerNumber"
			columnCustomerNumber.Width = 60
			columnCustomerNumber.Visible = False
			gvPrint.Columns.Add(columnCustomerNumber)

			Dim columnPayrollNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPayrollNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPayrollNumber.OptionsColumn.AllowEdit = False
			columnPayrollNumber.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
			columnPayrollNumber.Name = "PayrollNumber"
			columnPayrollNumber.FieldName = "PayrollNumber"
			columnPayrollNumber.Width = 60
			columnPayrollNumber.Visible = False
			gvPrint.Columns.Add(columnPayrollNumber)

			Dim columnEmployeeSocialSecurityNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeSocialSecurityNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeSocialSecurityNumber.OptionsColumn.AllowEdit = False
			columnEmployeeSocialSecurityNumber.Caption = m_Translate.GetSafeTranslationValue("Sozialversicherungsnummer")
			columnEmployeeSocialSecurityNumber.Name = "EmployeeSocialSecurityNumber"
			columnEmployeeSocialSecurityNumber.FieldName = "EmployeeSocialSecurityNumber"
			columnEmployeeSocialSecurityNumber.Width = 60
			columnEmployeeSocialSecurityNumber.Visible = True
			gvPrint.Columns.Add(columnEmployeeSocialSecurityNumber)

			Dim columnEmployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeFullname.OptionsColumn.AllowEdit = False
			columnEmployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullname.Name = "EmployeeFullname"
			columnEmployeeFullname.FieldName = "EmployeeFullname"
			columnEmployeeFullname.Width = 150
			columnEmployeeFullname.Visible = True
			gvPrint.Columns.Add(columnEmployeeFullname)

			Dim columnEmployeeBirthdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeBirthdate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeBirthdate.OptionsColumn.AllowEdit = False
			columnEmployeeBirthdate.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
			columnEmployeeBirthdate.Name = "EmployeeBirthdate"
			columnEmployeeBirthdate.FieldName = "EmployeeBirthdate"
			columnEmployeeBirthdate.Width = 60
			columnEmployeeBirthdate.Visible = True
			gvPrint.Columns.Add(columnEmployeeBirthdate)

			Dim columnPayrollYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPayrollYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPayrollYear.OptionsColumn.AllowEdit = False
			columnPayrollYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
			columnPayrollYear.Name = "PayrollYear"
			columnPayrollYear.FieldName = "PayrollYear"
			columnPayrollYear.Width = 60
			columnPayrollYear.Visible = False
			gvPrint.Columns.Add(columnPayrollYear)

			Dim columnPayrollMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPayrollMonth.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPayrollMonth.OptionsColumn.AllowEdit = False
			columnPayrollMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
			columnPayrollMonth.Name = "PayrollMonth"
			columnPayrollMonth.FieldName = "PayrollMonth"
			columnPayrollMonth.Width = 60
			columnPayrollMonth.Visible = False
			gvPrint.Columns.Add(columnPayrollMonth)

			Dim columnCustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomername.OptionsColumn.AllowEdit = False
			columnCustomername.Caption = m_Translate.GetSafeTranslationValue("Einsatzfirma")
			columnCustomername.Name = "Customername"
			columnCustomername.FieldName = "Customername"
			columnCustomername.Visible = True
			columnCustomername.Width = 80
			gvPrint.Columns.Add(columnCustomername)

			Dim columnLALabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLALabel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLALabel.OptionsColumn.AllowEdit = False
			columnLALabel.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnLALabel.Name = "LALabel"
			columnLALabel.FieldName = "LALabel"
			columnLALabel.Visible = False
			columnLALabel.Width = 80
			gvPrint.Columns.Add(columnLALabel)

			Dim columnTargetHours As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTargetHours.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTargetHours.OptionsColumn.AllowEdit = False
			columnTargetHours.Caption = m_Translate.GetSafeTranslationValue("Sollstunden")
			columnTargetHours.Name = "TargetHours"
			columnTargetHours.FieldName = "TargetHours"
			columnTargetHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTargetHours.AppearanceHeader.Options.UseTextOptions = True
			columnTargetHours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTargetHours.DisplayFormat.FormatString = "N2"
			columnTargetHours.Width = 60
			columnTargetHours.Visible = True
			columnTargetHours.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnTargetHours.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnTargetHours)

			Dim columnDowntimeHours As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDowntimeHours.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDowntimeHours.OptionsColumn.AllowEdit = False
			columnDowntimeHours.Caption = m_Translate.GetSafeTranslationValue("Ausfallstunden")
			columnDowntimeHours.Name = "DowntimeHours"
			columnDowntimeHours.FieldName = "DowntimeHours"
			columnDowntimeHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDowntimeHours.AppearanceHeader.Options.UseTextOptions = True
			columnDowntimeHours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnDowntimeHours.DisplayFormat.FormatString = "N2"
			columnDowntimeHours.Width = 60
			columnDowntimeHours.Visible = True
			columnDowntimeHours.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnDowntimeHours.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnDowntimeHours)

			Dim columnDowntimeProcentage As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDowntimeProcentage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDowntimeProcentage.OptionsColumn.AllowEdit = False
			columnDowntimeProcentage.Caption = m_Translate.GetSafeTranslationValue("Prozentualer Arbeitsausfall")
			columnDowntimeProcentage.Name = "DowntimeProcentage"
			columnDowntimeProcentage.FieldName = "DowntimeProcentage"
			columnDowntimeProcentage.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDowntimeProcentage.AppearanceHeader.Options.UseTextOptions = True
			columnDowntimeProcentage.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnDowntimeProcentage.DisplayFormat.FormatString = "N2"
			columnDowntimeProcentage.Width = 60
			columnDowntimeProcentage.Visible = True
			gvPrint.Columns.Add(columnDowntimeProcentage)

			Dim columnAHVBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAHVBasis.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAHVBasis.OptionsColumn.AllowEdit = False
			columnAHVBasis.Caption = m_Translate.GetSafeTranslationValue("AHV pflichtige Lohnsumme")
			columnAHVBasis.Name = "AHVBasis"
			columnAHVBasis.FieldName = "AHVBasis"
			columnAHVBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAHVBasis.AppearanceHeader.Options.UseTextOptions = True
			columnAHVBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAHVBasis.DisplayFormat.FormatString = "N2"
			columnAHVBasis.Width = 60
			columnAHVBasis.Visible = True
			columnAHVBasis.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnAHVBasis.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnAHVBasis)

			Dim columnDowntimeAHVBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDowntimeAHVBasis.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDowntimeAHVBasis.OptionsColumn.AllowEdit = False
			columnDowntimeAHVBasis.Caption = m_Translate.GetSafeTranslationValue("Lohnsumme der ausgefallenen Stunden")
			columnDowntimeAHVBasis.Name = "DowntimeAHVBasis"
			columnDowntimeAHVBasis.FieldName = "DowntimeAHVBasis"
			columnDowntimeAHVBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDowntimeAHVBasis.AppearanceHeader.Options.UseTextOptions = True
			columnDowntimeAHVBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnDowntimeAHVBasis.DisplayFormat.FormatString = "N2"
			columnDowntimeAHVBasis.Width = 60
			columnDowntimeAHVBasis.Visible = True
			columnDowntimeAHVBasis.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnDowntimeAHVBasis.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnDowntimeAHVBasis)

			Dim columnDowntimeCompensation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDowntimeCompensation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDowntimeCompensation.OptionsColumn.AllowEdit = False
			columnDowntimeCompensation.Caption = m_Translate.GetSafeTranslationValue("Enschädigung 80% der Lohnsumme für ausgefallene Stunden")
			columnDowntimeCompensation.Name = "DowntimeCompensation"
			columnDowntimeCompensation.FieldName = "DowntimeCompensation"
			columnDowntimeCompensation.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDowntimeCompensation.AppearanceHeader.Options.UseTextOptions = True
			columnDowntimeCompensation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnDowntimeCompensation.DisplayFormat.FormatString = "N2"
			columnDowntimeCompensation.Width = 60
			columnDowntimeCompensation.Visible = True
			columnDowntimeCompensation.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnDowntimeCompensation.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnDowntimeCompensation)

			Dim columnDowntimeCompensationAG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDowntimeCompensationAG.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDowntimeCompensationAG.OptionsColumn.AllowEdit = False
			columnDowntimeCompensationAG.Caption = m_Translate.GetSafeTranslationValue("6.375% Arbeitgeberbeiträge")
			columnDowntimeCompensationAG.Name = "DowntimeCompensationAG"
			columnDowntimeCompensationAG.FieldName = "DowntimeCompensationAG"
			columnDowntimeCompensationAG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDowntimeCompensationAG.AppearanceHeader.Options.UseTextOptions = True
			columnDowntimeCompensationAG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnDowntimeCompensationAG.DisplayFormat.FormatString = "N2"
			columnDowntimeCompensationAG.Width = 60
			columnDowntimeCompensationAG.Visible = True
			columnDowntimeCompensationAG.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnDowntimeCompensationAG.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnDowntimeCompensationAG)

			Dim columnTotalCompensation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTotalCompensation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnTotalCompensation.OptionsColumn.AllowEdit = False
			columnTotalCompensation.Caption = m_Translate.GetSafeTranslationValue("Kurzarbeitsentschädigung")
			columnTotalCompensation.Name = "TotalCompensation"
			columnTotalCompensation.FieldName = "TotalCompensation"
			columnTotalCompensation.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTotalCompensation.AppearanceHeader.Options.UseTextOptions = True
			columnTotalCompensation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTotalCompensation.DisplayFormat.FormatString = "N2"
			columnTotalCompensation.Width = 60
			columnTotalCompensation.Visible = True
			columnTotalCompensation.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnTotalCompensation.SummaryItem.DisplayFormat = "{0:n2}"
			gvPrint.Columns.Add(columnTotalCompensation)

			Dim columnNeedAttention As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNeedAttention.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNeedAttention.OptionsColumn.AllowEdit = False
			columnNeedAttention.Caption = m_Translate.GetSafeTranslationValue("Achtung")
			columnNeedAttention.Name = "NeedAttention"
			columnNeedAttention.FieldName = "NeedAttention"
			columnNeedAttention.Visible = True
			columnNeedAttention.Width = 80
			gvPrint.Columns.Add(columnNeedAttention)

			RestoreGridLayoutFromXml()

			grdPrint.DataSource = Nothing

		End Sub

#End Region

		Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		End Sub

		Private Sub LoadMandantenDropDown()
			Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

			lueMandant.Properties.DataSource = Data
			lueMandant.Properties.ForceInitialize()

		End Sub

		' Mandantendaten...
		Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents OrElse lueYear.EditValue Is Nothing Then
				Return
			End If

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

			Me.btnPrint.Enabled = False 'Not (m_InitializationData.MDData Is Nothing)

		End Sub

		Private Sub OnlueYear_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueYear.EditValueChanged

			If m_SuppressUIEvents OrElse lueYear.EditValue Is Nothing Then
				Return
			End If

			Dim success = LoadCustomerDropDownData()
			LoadDowntimeDataGrid()

			Me.btnPrint.Enabled = False 'Not (m_InitializationData.MDData Is Nothing)

		End Sub

		Private Sub OnlueMonth_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMonth.EditValueChanged

			If m_SuppressUIEvents OrElse lueMonth.EditValue Is Nothing Then
				Return
			End If

			Dim success = LoadCustomerDropDownData()
			LoadDowntimeDataGrid()

			Me.btnPrint.Enabled = False 'Not (m_InitializationData.MDData Is Nothing)

		End Sub

		Private Sub OnlueCustomer_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueCustomer.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			LoadDowntimeDataGrid()

			Me.btnPrint.Enabled = False ' Not (m_InitializationData.MDData Is Nothing)

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
			success = success AndAlso LoadCustomerDropDownData()


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

		Private Function LoadCustomerDropDownData() As Boolean

			Dim supressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			SplashScreenManager.CloseForm(False)

			If lueMonth.EditValue Is Nothing Then lueMonth.EditValue = Now.Month
			If lueYear.EditValue Is Nothing Then lueYear.EditValue = Now.Year
			m_SuppressUIEvents = supressUIEventState

			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Dim customerData = m_ListingDatabaseAccess.LoadDowntimeCustomerData(lueMandant.EditValue, lueMonth.EditValue, lueYear.EditValue)

			If customerData Is Nothing Then
				SplashScreenManager.CloseForm(False)

				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))

				Return False
			End If

			lueCustomer.EditValue = Nothing
			lueCustomer.Properties.DataSource = customerData

			SplashScreenManager.CloseForm(False)

			Return True

		End Function

		Private Sub LoadDowntimeDataGrid()

			SplashScreenManager.CloseForm(False)
			If lueMonth.EditValue Is Nothing Then Return
			If lueYear.EditValue Is Nothing Then Return

			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")


			Dim searchResult = m_ListingDatabaseAccess.LoadDowntimeDataData(lueMandant.EditValue, lueCustomer.EditValue, lueMonth.EditValue, lueYear.EditValue)

			If (searchResult Is Nothing) Then
				SplashScreenManager.CloseForm(False)

				Return
			End If


			Dim gridData = (From person In searchResult
							Select New DowntimeData With {.MDNr = person.MDNr,
								.EmployeeNumber = person.EmployeeNumber,
								.CustomerNumber = person.CustomerNumber,
								.PayrollNumber = person.PayrollNumber,
								.PayrollMonth = person.PayrollMonth,
								.PayrollYear = person.PayrollYear,
								.Customername = person.Customername,
								.EmployeeLastname = person.EmployeeLastname,
								.EmployeeFirstname = person.EmployeeFirstname,
								.EmployeeSocialSecurityNumber = person.EmployeeSocialSecurityNumber,
								.EmployeeBirthdate = person.EmployeeBirthdate,
								.LALabel = person.LALabel,
								.NeedAttention = person.NeedAttention,
								.TargetHours = person.TargetHours,
								.DowntimeHours = person.DowntimeHours,
								.DowntimeProcentage = person.DowntimeProcentage,
								.AHVBasis = person.AHVBasis,
								.DowntimeAHVBasis = person.DowntimeAHVBasis,
								.DowntimeCompensation = person.DowntimeCompensation,
								.DowntimeCompensationAG = person.DowntimeCompensationAG,
								.TotalCompensation = person.TotalCompensation
								}).ToList()

			Dim listDataSource As BindingList(Of DowntimeData) = New BindingList(Of DowntimeData)

			For Each p In gridData
				listDataSource.Add(p)
			Next
			m_DowntimeData = listDataSource
			btnPrint.Enabled = False ' listDataSource.Count > 0


			grdPrint.DataSource = listDataSource

			SplashScreenManager.CloseForm(False)

		End Sub

		Private Sub OnLueCustomer_ButtonClick(sender As Object, e As ButtonPressedEventArgs)

			If lueMandant.EditValue Is Nothing Or lueCustomer.EditValue Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then
				Dim hub = MessageService.Instance.Hub
				Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueCustomer.EditValue)
				hub.Publish(openCustomerMng)
			End If

		End Sub

		Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvPrint.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim viewData = CType(dataRow, DowntimeData)

					Select Case column.Name.ToLower
						Case "CustomerNumber".ToLower
							If viewData.CustomerNumber > 0 Then
								Dim hub = MessageService.Instance.Hub
								Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueCustomer.EditValue)
								hub.Publish(openCustomerMng)
							End If

						Case "PayrollNumber".ToLower
							If viewData.PayrollNumber > 0 Then PrintAssingedPayorll(viewData.EmployeeNumber, viewData.PayrollNumber)


						Case Else
							If viewData.EmployeeNumber > 0 Then
								Dim hub = MessageService.Instance.Hub
								Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, viewData.EmployeeNumber)
								hub.Publish(openMng)

							End If

					End Select

				End If

			End If

		End Sub

		Private Sub bbiExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
			CreateExcelSheet()
		End Sub

		Private Sub OngvMain_RowCountChanged(sender As Object, e As EventArgs)

			Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvPrint.RowCount)

		End Sub

		Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

			gvPrint.SaveLayoutToXml(m_GVSearchResultSettingfilename)

		End Sub

		Private Sub RestoreGridLayoutFromXml()
			Dim keepFilter = False
			Dim restoreLayout = True

			Try
				restoreLayout = m_Utility.ParseToBoolean(m_userGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreSearchSetting), True)
				keepFilter = m_Utility.ParseToBoolean(m_userGridSettingsXml.GetSettingByKey(m_xmlSettingSearchFilter), False)
			Catch ex As Exception

			End Try
			If File.Exists(m_GVSearchResultSettingfilename) Then gvPrint.RestoreLayoutFromXml(m_GVSearchResultSettingfilename)
			If restoreLayout AndAlso Not keepFilter Then gvPrint.ActiveFilterCriteria = Nothing

		End Sub

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

		Private Function ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Sub PrintAssingedPayorll(ByVal employeeNumber As Integer, ByVal payrollNumber As Integer)
			Dim liLONr As New List(Of Integer)
			Dim mdnr As Integer = m_InitializationData.MDData.MDNr
			Dim bSendPrintJob2WOS As Boolean = False
			Dim bSend_And_PrintJob2WOS As Boolean = False

			Try
				Dim _PSetting As New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting
				_PSetting = New SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																							 .SQL2Open = String.Empty,
																																							 .JobNr2Print = "9.1",
																																							 .Is4Export = False,
																																							 .SendData2WOS = bSendPrintJob2WOS,
																																							 .SendAndPrintData2WOS = bSend_And_PrintJob2WOS,
																																							 .liLONr2Print = New List(Of Integer)(New Integer() {payrollNumber}),
																																							 .liMANr2Print = New List(Of Integer)(New Integer() {employeeNumber}),
																																							 .liLOSend2WOS = New List(Of Boolean)(New Boolean() {False}),
																																							 .LiMALang = New List(Of String)(New String() {"DE"}),
																																							 .SelectedLONr2Print = 0,
																																							 .SelectedMANr2Print = 0,
																											.SelectedMDNr = m_InitializationData.MDData.MDNr,
																											.LogedUSNr = m_InitializationData.UserData.UserNr,
																											.PerosonalizedData = m_InitializationData.ProsonalizedData,
																											.TranslationData = m_InitializationData.TranslationData
																																							 }
				Dim obj As New LOSearchListing.ClsPrintLOSearchList(m_InitializationData)
				obj.PrintData = _PSetting
				Dim result As PrintResult = obj.PrintLOSearchList(False)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		'Private Sub OnacMain_CustomDrawElement(ByVal sender As Object, ByVal e As CustomDrawElementEventArgs) Handles acMain.CustomDrawElement
		'	If e.Element.Style = DevExpress.XtraBars.Navigation.ElementStyle.Group Then 'AndAlso e.Element.Text = "Options" Then
		'		e.DrawImage()
		'		e.DrawText()
		'		e.DrawContextButtons()
		'		'uncomment the following line to draw the expand\collapse button 
		'		e.DrawExpandCollapseButton()
		'		e.Handled = True
		'	End If
		'End Sub

#Region "Excel export"

		Private Sub CreateExcelSheet()
			Dim data = m_DowntimeData ' CType(grdPrint.DataSource, BindingList(Of DowntimeData))
			Dim excelFilename As String = Path.Combine(m_InitializationData.UserData.spAllowedPath, "KAE-Abrechnung.Xlsx")

			Try

				Dim exporter As IXlExporter = XlExport.CreateExporter(XlDocumentFormat.Xlsx)

				Using stream As FileStream = New FileStream(excelFilename, FileMode.Create, FileAccess.ReadWrite)

					Using document As IXlDocument = exporter.CreateDocument(stream)

						Using sheet As IXlSheet = document.CreateSheet()

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 20.0F
							End Using

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 20.0F
							End Using

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 20.0F
							End Using

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 15
							End Using

							Dim i As Integer = 0
							Dim table As IXlTable

							' Specify an array containing column headings for a table.
							'Dim columnNames() As String = {"Product", "Q1", "Q2", "Q3", "Q4", "Yearly Total"}
							Dim columns = New List(Of XlTableColumnInfo) From {New XlTableColumnInfo("Versicherten - Nr."),
								New XlTableColumnInfo("Name") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Vorname") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Geburtsdatum") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Jahr") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Monat") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Einsatzfirma") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Sollstunden") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Ausfallstunden") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Prozentualer Arbeitsausfall") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("AHV pflichtige Lohnsumme") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Lohnsumme der ausgefallenen Stunden") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Enschädigung 80% der Lohnsumme für ausgefallene Stunden") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("6.375% Arbeitgeberbeiträge") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Kurzarbeitsentschädigung") With {.DataFormatting = XlFill.NoFill},
								New XlTableColumnInfo("Achtung") With {.DataFormatting = XlFill.NoFill}
							}

							Dim headerRowFormatting As New XlCellFormatting()
							headerRowFormatting.Font = New XlFont()
							headerRowFormatting.Font.Bold = True
							headerRowFormatting.Font.Size = 12
							headerRowFormatting.Font.Color = XlColor.FromTheme(XlThemeColor.Dark1, 0.0)
							headerRowFormatting.Fill = XlFill.NoFill ' SolidFill(XlColor.FromTheme(XlThemeColor.Accent2, 0.0))

							columns(3).DataFormatting.NumberFormat = XlNumberFormat.LongDate ' XlCellFormatting.FromNetFormat("d", True) ' XlNumberFormat.ShortDate
							columns(4).DataFormatting.NumberFormat = XlNumberFormat.Number
							columns(3).DataFormatting.NumberFormat = XlNumberFormat.Number

							columns(7).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(8).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(9).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(10).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(11).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(12).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(13).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2
							columns(14).DataFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2

							' Create the first row in the worksheet from which the table starts.
							Using row As IXlRow = sheet.CreateRow()
								' Start generating the table with a header row displayed.
								table = row.BeginTable(columns, True, headerRowFormatting) ' XlCellFormatting.Heading1)
								table.Style.Name = XlBuiltInTableStyleId.Light1


								'Dim font_Renamed As New XlFont()
								'font_Renamed.Size = 20
								'columns(0).HeaderRowFormatting = New XlDifferentialFormatting
								'columns(0).HeaderRowFormatting = headerRowFormatting

								' Disable banded row formatting for the table.
								table.Style.ShowRowStripes = False
								' Disable the filtering functionality for the table. 
								table.HasAutoFilter = False

								' Specify formatting settings for the total row of the table.
								table.TotalRowFormatting = XlFill.SolidFill(XlColor.FromTheme(XlThemeColor.Dark1, 0.9))
								table.TotalRowFormatting.Border = New XlBorder() 'With {.BottomColor = XlColor.FromTheme(XlThemeColor.Accent6, 0.0), .BottomLineStyle = XlBorderLineStyle.Thick, .TopColor = XlColor.FromArgb(0, 0, 0), .TopLineStyle = XlBorderLineStyle.Dashed}
								'Dim accounting As XlNumberFormat = "_([409]* #,##0.00_);_([409]* \(#,##0.00\);_([409]* ""-""??_);_(@_)"


								' Specify the total row label.
								table.Columns(0).TotalRowLabel = String.Format(m_Translate.GetSafeTranslationValue("Anzahl von Kurzarbeit (KA) betroffene Arbeitnehmende: {0}"), m_DowntimeData.Count)

								' Specify the function to calculate the total.
								table.Columns(7).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(8).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(9).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(10).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(11).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(12).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(13).TotalRowFunction = XlTotalRowFunction.Sum
								table.Columns(14).TotalRowFunction = XlTotalRowFunction.Sum
								table.TotalRowFormatting.NumberFormat = XlNumberFormat.NumberWithThousandSeparator2 ' accounting

							End Using

							i = 0
							' Generate table rows and populate them with data.
							Dim attentionLabel = m_Translate.GetSafeTranslationValue("Achtung!")
							For i = 0 To gvPrint.RowCount - 1
								Using row As IXlRow = sheet.CreateRow()
									row.BulkCells(New Object() {m_DowntimeData(i).EmployeeSocialSecurityNumber,
									m_DowntimeData(i).EmployeeLastname,
									m_DowntimeData(i).EmployeeFirstname,
									String.Format("{0:d}", m_DowntimeData(i).EmployeeBirthdate),
									m_DowntimeData(i).PayrollYear,
									m_DowntimeData(i).PayrollMonth,
									m_DowntimeData(i).Customername,
									m_DowntimeData(i).TargetHours,
									m_DowntimeData(i).DowntimeHours,
									m_DowntimeData(i).DowntimeProcentage,
									m_DowntimeData(i).AHVBasis,
									m_DowntimeData(i).DowntimeAHVBasis,
									m_DowntimeData(i).DowntimeCompensation,
									m_DowntimeData(i).DowntimeCompensationAG,
									m_DowntimeData(i).TotalCompensation,
									String.Format("{0}", If(m_DowntimeData(i).NeedAttention.GetValueOrDefault(False), attentionLabel, String.Empty))},
												  Nothing)
								End Using
							Next

							' Create the total row and finish the table.
							Using row As IXlRow = sheet.CreateRow()
								row.EndTable(table, True)
							End Using

						End Using
						
					End Using

				End Using

				Process.Start(excelFilename)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Private Sub CreateManuellExcelSheet()
			Dim data = m_DowntimeData ' CType(grdPrint.DataSource, BindingList(Of DowntimeData))
			Dim excelFilename As String = Path.Combine(m_InitializationData.UserData.spAllowedPath, "KAE-Abrechnung Manuell.Xlsx")

			Try

				Dim exporter As IXlExporter = XlExport.CreateExporter(XlDocumentFormat.Xlsx)

				Using stream As FileStream = New FileStream(excelFilename, FileMode.Create, FileAccess.ReadWrite)

					Using document As IXlDocument = exporter.CreateDocument(stream)

						Using sheet As IXlSheet = document.CreateSheet()

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 20.0F
							End Using

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 20.0F
							End Using

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 20.0F
							End Using

							Using column As IXlColumn = sheet.CreateColumn()
								column.WidthInCharacters = 15
							End Using
							CreateHeader(sheet)

							Dim i As Integer = 0
							For i = 0 To gvPrint.RowCount - 1
								ExportRow(sheet, i, gvPrint)
							Next

						End Using

					End Using

				End Using

				Process.Start(excelFilename)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Private Sub CreateHeader(ByVal sheet As IXlSheet)

			Using row As IXlRow = sheet.CreateRow()

				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Versicherten-Nr."
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Name"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Vorname"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Geburtsdatum"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Jahr"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Monat"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Sollstunden"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Ausfallstunden"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Prozentualer Arbeitsausfall"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "AHV pflichtige Lohnsumme"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Lohnsumme der ausgefallenen Stunden"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Enschädigung 80% der Lohnsumme für ausgefallene Stunden"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "6.375% Arbeitgeberbeiträge"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Kurzarbeitsentschädigung"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using
				Using cell As IXlCell = row.CreateCell()
					cell.Value = "Achtung"
					cell.ApplyFormatting(XlCellFormatting.Heading1)
				End Using

			End Using

		End Sub
		Private Sub ExportRow(ByVal sheet As IXlSheet, ByVal gridRowHandle As Integer, ByVal gridView As GridView)
			Using row As IXlRow = sheet.CreateRow()
				ExportCells(row, gridRowHandle, gridView)
			End Using
		End Sub

		Private Sub ExportCells(ByVal row As IXlRow, ByVal gridRowHandle As Integer, ByVal gridView As GridView)
			Dim needAttention As Boolean = m_DowntimeData(gridRowHandle).NeedAttention.GetValueOrDefault(False)

			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).EmployeeSocialSecurityNumber)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).EmployeeLastname)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).EmployeeFirstname)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).EmployeeBirthdate)
				cell.Formatting = XlNumberFormat.ShortDate
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).PayrollYear)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).PayrollMonth)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).TargetHours)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).DowntimeHours)
				cell.Formatting = XlNumberFormat.NumberWithThousandSeparator2
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).DowntimeProcentage)
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).AHVBasis)
				cell.Formatting = XlNumberFormat.NumberWithThousandSeparator2
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).DowntimeAHVBasis)
				cell.Formatting = XlNumberFormat.NumberWithThousandSeparator2
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).DowntimeCompensation)
				cell.Formatting = XlNumberFormat.NumberWithThousandSeparator2
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).DowntimeCompensationAG)
				cell.Formatting = XlNumberFormat.NumberWithThousandSeparator2
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using
			Using cell = row.CreateCell()
				cell.Value = XlVariantValue.FromObject(m_DowntimeData(gridRowHandle).TotalCompensation)
				cell.Formatting = XlNumberFormat.NumberWithThousandSeparator2
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using

			Using cell = row.CreateCell()
				If needAttention Then cell.Value = "Achtung!"
				If needAttention Then cell.ApplyFormatting(XlCellFormatting.WarningText)
			End Using

			'For Each column As GridColumn In gridView.Columns
			'If Not column.Visible Then Continue For

			'Using cell = row.CreateCell()
			'	cell.Value = XlVariantValue.FromObject(gridView.GetRowCellValue(gridRowHandle, column))
			'End Using


			'Next

		End Sub


#End Region

#Region "Helper Classes"

		Class IntegerValueViewWrapper
			Public Property Value As Integer
		End Class

#End Region


	End Class


	Public Class PreselectionKAEData
		Public Property MDNr As Integer
		Public Property Year As Integer
		Public Property Month As Integer
		Public Property CustomerNumber As Integer?
		Public Property CustomerNumbers As List(Of Integer?)

	End Class

End Namespace