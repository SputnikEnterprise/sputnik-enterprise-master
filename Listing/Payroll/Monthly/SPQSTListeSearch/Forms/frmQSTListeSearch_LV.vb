
Option Strict Off

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports System.ComponentModel
Imports SPQSTListeSearch.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class frmQSTListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVEmployeeSearchSettingfilename As String
	Private m_GVEmployeeSearchVacancySettingfilename As String

	Private m_xmlSettingRestoreEmployeeSearchSetting As String
	Private m_xmlSettingEmployeeSearchFilter As String

	Private m_LoadedData As IEnumerable(Of SearchRestulOfTaxData)



#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "qstsearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_QST_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/qstsearchlist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_QST_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/qstsearchlist/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		Me.Sql2Open = strQuery

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.m_InitialData.TranslationData, ClsDataDetail.m_InitialData.ProsonalizedData)

		Try
			m_ListingDatabaseAccess = New ListingDatabaseAccess(ClsDataDetail.m_InitialData.MDData.MDDbConn, ClsDataDetail.m_InitialData.UserData.UserLanguage)

			m_GridSettingPath = String.Format("{0}QSTSearchList\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVEmployeeSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.m_InitialData.UserData.UserNr)

			m_xmlSettingRestoreEmployeeSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_QST_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingEmployeeSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_QST_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)


			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.m_InitialData.MDData.MDNr))

		Catch ex As Exception

		End Try

		ResetGridSalaryData()

		AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

	End Sub

#End Region


#Region "public properties"

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As SearchRestulOfTaxData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchRestulOfTaxData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Public ReadOnly Property LoadFoundedData As IEnumerable(Of SearchRestulOfTaxData)
		Get
			Return m_LoadedData
		End Get

	End Property


#End Region

	Function GetDbSalaryData4Show() As IEnumerable(Of SearchRestulOfTaxData)
		Dim result As List(Of SearchRestulOfTaxData) = Nothing

		Dim data = m_ListingDatabaseAccess.LoadSearchResultOfTaxData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.m_InitialData.UserData.UserNr)


		Return data


		'Dim sql As String

		'sql = Sql2Open
		'sql = "[Load Montly Tax Data For Search In TAX Listing]"
		'Dim listOfParams As New List(Of SqlClient.SqlParameter)
		'listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(ClsDataDetail.m_InitialData.MDData.MDNr, DBNull.Value)))
		'listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(ClsDataDetail.m_InitialData.UserData.UserNr, DBNull.Value)))

		'Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		'Try

		'	If (Not reader Is Nothing) Then

		'		result = New List(Of FoundedData)

		'		While reader.Read()
		'			Dim overviewData As New FoundedData

		'			overviewData.MANr = m_utility.SafeGetInteger(reader, "MANr", Nothing)

		'			overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "Nachname"), m_utility.SafeGetString(reader, "Vorname"))

		'			overviewData.vonmonat = m_utility.SafeGetInteger(reader, "vonmonat", Nothing)
		'			overviewData.bismonat = m_utility.SafeGetInteger(reader, "bismonat", Nothing)
		'			overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)

		'			overviewData.gebdat = m_utility.SafeGetDateTime(reader, "gebdat", Nothing)
		'			overviewData.ahv_nr = m_utility.SafeGetString(reader, "ahv_nr")
		'			overviewData.ahv_nr_new = m_utility.SafeGetString(reader, "ahv_nr_new")
		'			overviewData.s_kanton = m_utility.SafeGetString(reader, "s_kanton")
		'			overviewData.qstgemeinde = m_utility.SafeGetString(reader, "S_Gemeinde")

		'			overviewData.TaxCommunityLabel = m_utility.SafeGetString(reader, "TaxCommunityLabel")
		'			overviewData.TaxCommunityCode = m_utility.SafeGetInteger(reader, "TaxCommunityCode", 0)

		'			overviewData.EmploymentType = m_utility.SafeGetString(reader, "EmploymentType")
		'			overviewData.OtherEmploymentType = m_utility.SafeGetString(reader, "OtherEmploymentType")
		'			overviewData.TypeofStay = m_utility.SafeGetString(reader, "TypeofStay")
		'			overviewData.ForeignCategory = m_utility.SafeGetString(reader, "ForeignCategory")
		'			overviewData.SocialInsuranceNumber = m_utility.SafeGetString(reader, "SocialInsuranceNumber")
		'			overviewData.CivilState = m_utility.SafeGetString(reader, "CivilState")
		'			overviewData.NumberOfChildren = m_utility.SafeGetInteger(reader, "NumberOfChildren", 0)
		'			overviewData.TaxChurchCode = m_utility.SafeGetString(reader, "TaxChurchCode")
		'			overviewData.PartnerLastName = m_utility.SafeGetString(reader, "PartnerLastName")
		'			overviewData.PartnerFirstname = m_utility.SafeGetString(reader, "PartnerFirstname")
		'			overviewData.PartnerStreet = m_utility.SafeGetString(reader, "PartnerStreet")
		'			overviewData.PartnerPostcode = m_utility.SafeGetString(reader, "PartnerPostcode")
		'			overviewData.PartnerCity = m_utility.SafeGetString(reader, "PartnerCity")
		'			overviewData.PartnerCountry = m_utility.SafeGetString(reader, "PartnerCountry")
		'			overviewData.InEmployment = m_utility.SafeGetBoolean(reader, "InEmployment", False)

		'			overviewData.EmploymentLocation = m_utility.SafeGetString(reader, "ESOrt")
		'			overviewData.EmploymentPostcode = m_utility.SafeGetString(reader, "ESPLZ")
		'			overviewData.EmploymentStreet = m_utility.SafeGetString(reader, "ESStrasse")

		'			overviewData.geschlecht = m_utility.SafeGetString(reader, "geschlecht")

		'			overviewData.employeestreet = m_utility.SafeGetString(reader, "mastrasse")
		'			overviewData.employeepostcode = m_utility.SafeGetString(reader, "maplz")
		'			overviewData.employeecity = m_utility.SafeGetString(reader, "maort")
		'			overviewData.employeecountry = m_utility.SafeGetString(reader, "maland")

		'			overviewData.monat = m_utility.SafeGetInteger(reader, "monat", Nothing)
		'			overviewData.kinder = m_utility.SafeGetInteger(reader, "kinder", Nothing)
		'			overviewData.employeelanguage = m_utility.SafeGetString(reader, "sprache")

		'			overviewData.m_anz = m_utility.SafeGetDecimal(reader, "m_anz", Nothing)
		'			overviewData.m_bas = m_utility.SafeGetDecimal(reader, "m_bas", Nothing)
		'			overviewData.m_ans = m_utility.SafeGetDecimal(reader, "m_ans", Nothing)
		'			overviewData.m_btr = m_utility.SafeGetDecimal(reader, "m_btr", Nothing)
		'			overviewData.Bruttolohn = m_utility.SafeGetDecimal(reader, "bruttolohn", Nothing)
		'			overviewData.qstbasis = m_utility.SafeGetDecimal(reader, "qstbasis", Nothing)
		'			overviewData.stdanz = m_utility.SafeGetDecimal(reader, "stdanz", Nothing)

		'			overviewData.tarifcode = m_utility.SafeGetString(reader, "tarifcode")
		'			overviewData.workeddays = m_utility.SafeGetInteger(reader, "workeddays", Nothing)

		'			overviewData.WorkingHoursMonth = m_utility.SafeGetDecimal(reader, "RPGAVStdMonth", 0)
		'			overviewData.WorkingHoursWeek = m_utility.SafeGetDecimal(reader, "RPGAVStdWeek", 0)
		'			overviewData.EmploymentNumber = m_utility.SafeGetInteger(reader, "AssignedESNr", 0)
		'			overviewData.ESLohnNumber = m_utility.SafeGetInteger(reader, "AssignedESLohnNr", 0)
		'			overviewData.ReportNumber = m_utility.SafeGetInteger(reader, "AssignedRPNr", 0)
		'			overviewData.WorkingPensum = m_utility.SafeGetString(reader, "Arbeitspensum")
		'			overviewData.GAVStringInfo = m_utility.SafeGetString(reader, "GAVInfo")
		'			overviewData.Dismissalreason = m_utility.SafeGetString(reader, "Dismissalreason")

		'			overviewData.EmployeePartnerRecID = m_utility.SafeGetInteger(reader, "EmployeePartnerRecID", 0)
		'			overviewData.EmployeeLOHistoryID = m_utility.SafeGetInteger(reader, "EmployeeLOHistoryID", 0)

		'			overviewData.esab = m_utility.SafeGetDateTime(reader, "esab", Nothing)
		'			overviewData.esende = m_utility.SafeGetDateTime(reader, "esende", Nothing)

		'			result.Add(overviewData)

		'		End While

		'	End If


		'Catch e As Exception
		'	result = Nothing
		'	m_Logger.LogError(e.ToString())

		'Finally
		'	m_utility.CloseReader(reader)

		'End Try

		'Return result
	End Function

	Private Function LoadFoundedSalaryList() As Boolean

		Dim listOfEmployees = GetDbSalaryData4Show()
		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Keine Daten wurden gefunden.")

			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New SearchRestulOfTaxData With
																						 {.MANr = person.MANr,
																							.vonmonat = person.vonmonat,
																							.bismonat = person.bismonat,
																							.jahr = person.jahr,
																							.gebdat = person.gebdat,
																							.CivilStatus = person.CivilStatus,
																							.ahv_nr = person.ahv_nr,
																							.ahv_nr_new = person.ahv_nr_new,
																							.s_kanton = person.s_kanton,
																							.qstgemeinde = person.qstgemeinde,
																							.geschlecht = person.geschlecht,
																							.monat = person.monat,
																							.employeename = person.employeename,
																							.employeepostcode = person.employeepostcode,
																							.employeecity = person.employeecity,
																							.employeecountry = person.employeecountry,
																							.kinder = person.kinder,
																							.employeelanguage = person.employeelanguage,
																							.m_anz = person.m_anz,
																							.m_bas = person.m_bas,
																							.m_ans = person.m_ans,
																							.m_btr = person.m_btr,
																							.Bruttolohn = person.Bruttolohn,
																							.qstbasis = person.qstbasis,
																							.stdanz = person.stdanz,
																							.tarifcode = person.tarifcode,
																							.workeddays = person.workeddays,
																							.esab = person.esab,
																							.esende = person.esende,
											  .CivilState = person.CivilState,
											  .TaxCommunityCode = person.TaxCommunityCode,
																							.TaxCommunityLabel = person.TaxCommunityLabel,
																							.EmploymentType = person.EmploymentType,
																							.OtherEmploymentType = person.OtherEmploymentType,
																							.TypeofStay = person.TypeofStay,
																							.ForeignCategory = person.ForeignCategory,
																							.SocialInsuranceNumber = person.SocialInsuranceNumber,
																							.NumberOfChildren = person.NumberOfChildren,
																							.TaxChurchCode = person.TaxChurchCode,
																							.PartnerLastName = person.PartnerLastName,
																							.PartnerFirstname = person.PartnerFirstname,
																							.PartnerStreet = person.PartnerStreet,
																							.PartnerPostcode = person.PartnerPostcode,
																							.PartnerCountry = person.PartnerCountry,
																							.InEmployment = person.InEmployment,
																							.WorkingHoursMonth = person.WorkingHoursMonth,
																							.WorkingHoursWeek = person.WorkingHoursWeek,
																							.EmploymentNumber = person.EmploymentNumber,
																							.ESLohnNumber = person.ESLohnNumber,
																							.ReportNumber = person.ReportNumber,
																							.WorkingPensum = person.WorkingPensum,
																							.GAVStringInfo = person.GAVStringInfo,
																							.Dismissalreason = person.Dismissalreason,
																							.EmployeePartnerRecID = person.EmployeePartnerRecID,
											  .EmployeeLOHistoryID = person.EmployeeLOHistoryID
																						 }).ToList()

		Dim listDataSource As BindingList(Of SearchRestulOfTaxData) = New BindingList(Of SearchRestulOfTaxData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource
		m_LoadedData = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridSalaryData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

		gvRP.Columns.Clear()


		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MaNr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnVonMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVonMonat.Caption = m_Translate.GetSafeTranslationValue("Von")
		columnVonMonat.Name = "vonmonat"
		columnVonMonat.FieldName = "vonmonat"
		columnVonMonat.Visible = False
		gvRP.Columns.Add(columnVonMonat)

		Dim columnbismonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbismonat.Caption = m_Translate.GetSafeTranslationValue("Bis")
		columnbismonat.Name = "bismonat"
		columnbismonat.FieldName = "bismonat"
		columnbismonat.Visible = False
		gvRP.Columns.Add(columnbismonat)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.Visible = False
		gvRP.Columns.Add(columnjahr)

		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Geb.-Datum")
		columngebdat.Name = "gebdat"
		columngebdat.FieldName = "gebdat"
		columngebdat.Visible = False
		gvRP.Columns.Add(columngebdat)

		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeename.Name = "employeename"
		columnEmployeename.FieldName = "employeename"
		columnEmployeename.Visible = True
		columnEmployeename.BestFit()
		gvRP.Columns.Add(columnEmployeename)

		Dim columnahv_nr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahv_nr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnahv_nr.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr.")
		columnahv_nr.Name = "ahv_nr"
		columnahv_nr.FieldName = "ahv_nr"
		columnahv_nr.Visible = False
		columnahv_nr.BestFit()
		gvRP.Columns.Add(columnahv_nr)

		Dim columnahv_nr_new As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahv_nr_new.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnahv_nr_new.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr. neu")
		columnahv_nr_new.Name = "ahv_nr_new"
		columnahv_nr_new.FieldName = "ahv_nr_new"
		columnahv_nr_new.Visible = False
		columnahv_nr_new.BestFit()
		gvRP.Columns.Add(columnahv_nr_new)

		Dim columns_kanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columns_kanton.Caption = m_Translate.GetSafeTranslationValue("Kanton")
		columns_kanton.Name = "s_kanton"
		columns_kanton.FieldName = "s_kanton"
		columns_kanton.Visible = False
		columns_kanton.BestFit()
		gvRP.Columns.Add(columns_kanton)

		Dim columnqstgemeinde As New DevExpress.XtraGrid.Columns.GridColumn()
		columnqstgemeinde.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnqstgemeinde.Caption = m_Translate.GetSafeTranslationValue("Gemeinde")
		columnqstgemeinde.Name = "qstgemeinde"
		columnqstgemeinde.FieldName = "qstgemeinde"
		columnqstgemeinde.Visible = False
		columnqstgemeinde.BestFit()
		gvRP.Columns.Add(columnqstgemeinde)


		Dim columngeschlecht As New DevExpress.XtraGrid.Columns.GridColumn()
		columngeschlecht.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columngeschlecht.Name = "geschlecht"
		columngeschlecht.FieldName = "geschlecht"
		columngeschlecht.BestFit()
		columngeschlecht.Visible = True
		gvRP.Columns.Add(columngeschlecht)


		Dim columnmonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnmonat.Name = "monat"
		columnmonat.FieldName = "monat"
		columnmonat.BestFit()
		columnmonat.Visible = True
		gvRP.Columns.Add(columnmonat)


		Dim columnkinder As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkinder.Caption = m_Translate.GetSafeTranslationValue("Kinder")
		columnkinder.Name = "kinder"
		columnkinder.FieldName = "kinder"
		columnkinder.Visible = False
		columnkinder.BestFit()
		gvRP.Columns.Add(columnkinder)

		Dim columnemployeelanguage As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeelanguage.Caption = m_Translate.GetSafeTranslationValue("Sprache")
		columnemployeelanguage.Name = "employeelanguage"
		columnemployeelanguage.FieldName = "employeelanguage"
		columnemployeelanguage.Visible = False
		columnemployeelanguage.BestFit()
		gvRP.Columns.Add(columnemployeelanguage)


		Dim columnm_anz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_anz.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
		columnm_anz.Name = "m_anz"
		columnm_anz.FieldName = "m_anz"
		columnm_anz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_anz.AppearanceHeader.Options.UseTextOptions = True
		columnm_anz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_anz.DisplayFormat.FormatString = "N2"
		columnm_anz.Visible = False
		columnm_anz.BestFit()
		columnm_anz.SummaryItem.DisplayFormat = "{0:n2}"
		columnm_anz.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnm_anz.SummaryItem.Tag = "Summ_anz"
		gvRP.Columns.Add(columnm_anz)

		Dim columnm_bas As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_bas.Caption = m_Translate.GetSafeTranslationValue("Basis")
		columnm_bas.Name = "m_bas"
		columnm_bas.FieldName = "m_bas"
		columnm_bas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_bas.AppearanceHeader.Options.UseTextOptions = True
		columnm_bas.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_bas.DisplayFormat.FormatString = "N2"
		columnm_bas.Visible = False
		columnm_bas.BestFit()
		columnm_bas.SummaryItem.DisplayFormat = "{0:n2}"
		columnm_bas.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnm_bas.SummaryItem.Tag = "Summ_bas"
		gvRP.Columns.Add(columnm_bas)

		Dim columnm_ans As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_ans.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
		columnm_ans.Name = "m_ans"
		columnm_ans.FieldName = "m_ans"
		columnm_ans.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_ans.AppearanceHeader.Options.UseTextOptions = True
		columnm_ans.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_ans.DisplayFormat.FormatString = "N2"
		columnm_ans.Visible = False
		columnm_ans.BestFit()
		gvRP.Columns.Add(columnm_ans)

		Dim columnm_btr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_btr.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnm_btr.Name = "m_btr"
		columnm_btr.FieldName = "m_btr"
		columnm_btr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_btr.AppearanceHeader.Options.UseTextOptions = True
		columnm_btr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_btr.DisplayFormat.FormatString = "N2"
		columnm_btr.Visible = True
		columnm_btr.BestFit()
		columnm_btr.SummaryItem.DisplayFormat = "{0:n2}"
		columnm_btr.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnm_btr.SummaryItem.Tag = "Summ_btr"
		gvRP.Columns.Add(columnm_btr)

		Dim columnbruttolohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbruttolohn.Caption = m_Translate.GetSafeTranslationValue("Bruttolohn")
		columnbruttolohn.Name = "Bruttolohn"
		columnbruttolohn.FieldName = "Bruttolohn"
		columnbruttolohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbruttolohn.AppearanceHeader.Options.UseTextOptions = True
		columnbruttolohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbruttolohn.DisplayFormat.FormatString = "N2"
		columnbruttolohn.Visible = True
		columnbruttolohn.BestFit()
		columnbruttolohn.SummaryItem.DisplayFormat = "{0:n2}"
		columnbruttolohn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbruttolohn.SummaryItem.Tag = "Sumbruttolohn"
		gvRP.Columns.Add(columnbruttolohn)

		Dim columnqstbasis As New DevExpress.XtraGrid.Columns.GridColumn()
		columnqstbasis.Caption = m_Translate.GetSafeTranslationValue("QST.-Basis")
		columnqstbasis.Name = "qstbasis"
		columnqstbasis.FieldName = "qstbasis"
		columnqstbasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnqstbasis.AppearanceHeader.Options.UseTextOptions = True
		columnqstbasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnqstbasis.DisplayFormat.FormatString = "N2"
		columnqstbasis.Visible = True
		columnqstbasis.BestFit()
		columnqstbasis.SummaryItem.DisplayFormat = "{0:n2}"
		columnqstbasis.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnqstbasis.SummaryItem.Tag = "Sumqstbasis"
		gvRP.Columns.Add(columnqstbasis)


		Dim columnstdanz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnstdanz.Caption = m_Translate.GetSafeTranslationValue("Std.-Anzahl")
		columnstdanz.Name = "stdanz"
		columnstdanz.FieldName = "stdanz"
		columnstdanz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnstdanz.AppearanceHeader.Options.UseTextOptions = True
		columnstdanz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnstdanz.DisplayFormat.FormatString = "N2"
		columnstdanz.Visible = False
		columnstdanz.SummaryItem.DisplayFormat = "{0:n2}"
		columnstdanz.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnstdanz.SummaryItem.Tag = "Sumstdanz"
		gvRP.Columns.Add(columnstdanz)

		Dim columntarifcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columntarifcode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columntarifcode.Caption = m_Translate.GetSafeTranslationValue("Tarifcode")
		columntarifcode.Name = "tarifcode"
		columntarifcode.FieldName = "tarifcode"
		columntarifcode.BestFit()
		columntarifcode.Visible = True
		gvRP.Columns.Add(columntarifcode)

		Dim columnworkeddays As New DevExpress.XtraGrid.Columns.GridColumn()
		columnworkeddays.Caption = m_Translate.GetSafeTranslationValue("Anzahl Tage")
		columnworkeddays.Name = "workeddays"
		columnworkeddays.FieldName = "workeddays"
		columnworkeddays.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnworkeddays.AppearanceHeader.Options.UseTextOptions = True
		columnworkeddays.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnworkeddays.DisplayFormat.FormatString = "N0"
		columnworkeddays.Visible = False
		gvRP.Columns.Add(columnworkeddays)


		Dim columnesab As New DevExpress.XtraGrid.Columns.GridColumn()
		columnesab.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Beginn")
		columnesab.Name = "esab"
		columnesab.FieldName = "esab"
		columnworkeddays.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnworkeddays.DisplayFormat.FormatString = "G"
		columnesab.Visible = False
		gvRP.Columns.Add(columnesab)

		Dim columnesende As New DevExpress.XtraGrid.Columns.GridColumn()
		columnesende.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Ende")
		columnesende.Name = "esende"
		columnesende.FieldName = "esende"
		columnworkeddays.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnworkeddays.DisplayFormat.FormatString = "G"
		columnesende.Visible = False
		gvRP.Columns.Add(columnesende)

		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


#Region "Form Properties..."


	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		StartTranslation()
		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, 100)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, 100)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try

			LoadFoundedSalaryList()
			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		Catch ex As Exception

		End Try

	End Sub


#End Region


	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub



	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			Dim obj As New ThreadTesting.OpenFormsWithThreading()

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, SearchRestulOfTaxData)

				Select Case column.Name.ToLower

					Case Else
						If CheckIfRunning("SPS.MainView") Then
							If viewData.MANr.HasValue Then obj.OpenSelectedEmployee(viewData.MANr)
						End If

				End Select

			End If

		End If

	End Sub


	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "kinder" Or e.Column.FieldName = "m_anz" Or e.Column.FieldName = "m_bas" Or e.Column.FieldName = "m_ans" Or e.Column.FieldName = "m_btr" Or
			e.Column.FieldName = "bruttolohn" Or e.Column.FieldName = "qstbasis" Or e.Column.FieldName = "stdanz" Or e.Column.FieldName = "workeddays" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("m_anz").Visible OrElse gvRP.Columns("m_bas").Visible OrElse gvRP.Columns("m_btr").Visible OrElse gvRP.Columns("stdanz").Visible OrElse gvRP.Columns("bruttolohn").Visible OrElse gvRP.Columns("qstbasis").Visible)
		gvRP.SaveLayoutToXml(m_GVEmployeeSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreEmployeeSearchSetting, False), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingEmployeeSearchFilter, False), False)

		Catch ex As Exception

		End Try

		If restoreLayout AndAlso File.Exists(m_GVEmployeeSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVEmployeeSearchSettingfilename)
		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing


	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintlist.ItemClick

		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub



End Class



Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		Private Property SelectedMANr As Integer
		Private Property SQL2Open As String

		Private m_ui As New UtilityUI

		Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, Employeenumber)
				hub.Publish(openMng)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub
	End Class

End Namespace
