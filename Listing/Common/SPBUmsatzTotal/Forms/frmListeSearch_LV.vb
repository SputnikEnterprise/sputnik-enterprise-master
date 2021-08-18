

Option Strict Off

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPRPUmsatzTotal.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class frmListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm
	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property m_tblName As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreLoYFAKSearchSetting As String
	Private m_xmlSettingLoYFAKSearchFilter As String


#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "db1searchlist"

	Private Const USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/lolisting/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/lolisting/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String, ByVal tableName As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		Me.Sql2Open = strQuery
		m_tblName = tableName

		Try
			m_GridSettingPath = String.Format("{0}{1}\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr), MODUL_NAME_SETTING)

			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.m_InitialData.UserData.UserNr)

			m_xmlSettingRestoreLoYFAKSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoYFAKSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOYFAK_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.m_InitialData.MDData.MDNr))

		Catch ex As Exception

		End Try

		Reset()
		TranslateControls()


		AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler Me.gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		'AddHandler gvPrint.RowCellClick, AddressOf OngvDetail_RowCellClick

		AddHandler Me.gvInvoiceData.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvCustomerJournal.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub

#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Public ReadOnly Property SelectedPayrollKSTDataRecord As SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData
		Get
			Dim gvData = TryCast(grdPayroll_Staging.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvData.GetRow(selectedRows(0)), SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedPayrollAGDataRecord(ByVal sender As GridView) As SP.DatabaseAccess.Listing.DataObjects.DB1PayrollAGAnteilData
		Get
			Dim gvData = sender ' TryCast(grdPayroll_Staging.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvData.GetRow(selectedRows(0)), SP.DatabaseAccess.Listing.DataObjects.DB1PayrollAGAnteilData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedPayrollDataRecord(ByVal sender As GridView) As SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData
		Get
			Dim gvData = sender ' TryCast(grdPayroll_Staging.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvData.GetRow(selectedRows(0)), SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadFoundedSalaryList()
		success = success AndAlso LoadFoundedCustomerList()


		Return success

	End Function

	Public Sub LoadPayrollData(ByVal data As List(Of DB1PayrollData))
		Dim listOfEmployees = data

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New DB1PayrollData With
{.payrollNumber = person.payrollNumber,
.agemployeeAmount = person.agemployeeAmount,
.ahvemployeeAmount = person.ahvemployeeAmount,
.KST = person.KST,
._agAmount = person._agAmount,
.agemployeebvgAmount = person.agemployeebvgAmount,
._ahvAmount = person._ahvAmount
}).ToList()

		Dim listDataSource As BindingList(Of DB1PayrollData) = New BindingList(Of DB1PayrollData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		'grdPayrollData.DataSource = listDataSource

	End Sub

	Public Sub LoadPayrollData_Staging(ByVal data As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData))
		Dim listOfEmployees = data

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData With
											  {.KST = person.KST,
											  .AdvisorName = person.AdvisorName,
											  .PayrollListData = person.PayrollListData,
											  .AGDetailData = person.AGDetailData
											  }).ToList()

		Dim listDataSource As BindingList(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData) = New BindingList(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdPayroll_Staging.DataSource = listDataSource

	End Sub

	Public Sub LoadInvoiceData(ByVal data As List(Of DB1InvoiceData))
		Dim listOfEmployees = data

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New DB1InvoiceData With
					 {.invoicenNumber = person.invoicenNumber,
						.reportNumber = person.reportNumber,
						.KST = person.KST,
						.invoiceArt = person.invoiceArt,
						.invoiceAmount = person.invoiceAmount
					 }).ToList()

		Dim listDataSource As BindingList(Of DB1InvoiceData) = New BindingList(Of DB1InvoiceData)

		For Each p In responsiblePersonsGridData
			'Trace.WriteLine(String.Format("{0} {1}", p.invoicenNumber, p.invoiceAmount))
			listDataSource.Add(p)
		Next

		grdInvoiceData.DataSource = listDataSource

	End Sub


	Private Sub Reset()

		ResetGridSalaryData()
		ResetGridPayrollData_Staging()
		ResetGridPayrollData_L2_Staging()

		'ResetGridPayrollData()
		ResetGridInvoiceData()
		ResetGridCustomerData()

	End Sub

	Private Function LoadFoundedSalaryList() As Boolean

		Dim listOfEmployees = GetDbSalaryData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedData With
{.ID = person.ID,
.MDNr = person.MDNr,
.monat = person.monat,
.jahr = person.jahr,
.CustomerNumber = person.CustomerNumber,
.EmployeeNumber = person.EmployeeNumber,
.EmploymentNumber = person.EmploymentNumber,
.EmployeeFirstName = person.EmployeeFirstName,
.EmployeeLastName = person.EmployeeLastName,
.CustomerName = person.CustomerName,
.kst3_1 = person.kst3_1,
.kst3bez = person.kst3bez,
.USFiliale = person.USFiliale,
._tempumsatz = person._tempumsatz,
._indumsatz = person._indumsatz,
._festumsatz = person._festumsatz,
.bruttolohn = person.bruttolohn,
.ahvlohn = person.ahvlohn,
.agbetrag = person.agbetrag,
.fremdleistung = person.fremdleistung,
.lohnaufwand_1 = person.lohnaufwand_1,
.lohnaufwand_2 = person.lohnaufwand_2,
._marge = person._marge,
._bgtemp = person._bgtemp,
._bgind = person._bgind,
._bgfest = person._bgfest
}).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

		Return Not listOfEmployees Is Nothing
	End Function

	Private Function LoadFoundedCustomerList() As Boolean

		Dim listOfEmployees = GetDbCustomerData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedData With
{.ID = person.ID,
.MDNr = person.MDNr,
.monat = person.monat,
.jahr = person.jahr,
.CustomerNumber = person.CustomerNumber,
.EmployeeNumber = person.EmployeeNumber,
.EmploymentNumber = person.EmploymentNumber,
.EmployeeFirstName = person.EmployeeFirstName,
.EmployeeLastName = person.EmployeeLastName,
.CustomerName = person.CustomerName,
.kst3_1 = person.kst3_1,
.kst3bez = person.kst3bez,
.USFiliale = person.USFiliale,
._tempumsatz = person._tempumsatz,
._indumsatz = person._indumsatz,
._festumsatz = person._festumsatz,
.bruttolohn = person.bruttolohn,
.ahvlohn = person.ahvlohn,
.agbetrag = person.agbetrag,
.fremdleistung = person.fremdleistung,
.lohnaufwand_1 = person.lohnaufwand_1,
.lohnaufwand_2 = person.lohnaufwand_2,
._marge = person._marge,
._bgtemp = person._bgtemp,
._bgind = person._bgind,
._bgfest = person._bgfest
}).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdCustomerJournal.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Private Function GetDbSalaryData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.ID = m_utility.SafeGetInteger(reader, "ID", Nothing)
					overviewData.MDNr = m_InitialData.MDData.MDNr
					overviewData.monat = m_utility.SafeGetInteger(reader, "Monat", Nothing)
					overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", Nothing)
					overviewData.CustomerNumber = m_utility.SafeGetInteger(reader, "CustomerNumber", Nothing)
					overviewData.EmployeeNumber = m_utility.SafeGetInteger(reader, "EmployeeNumber", Nothing)
					overviewData.EmploymentNumber = m_utility.SafeGetInteger(reader, "EmploymentNumber", Nothing)

					overviewData.kst3_1 = m_utility.SafeGetString(reader, "kst3_1")
					overviewData.kst3bez = m_utility.SafeGetString(reader, "kst3bez")
					overviewData.USFiliale = m_utility.SafeGetString(reader, "USFiliale")

					overviewData._tempumsatz = m_utility.SafeGetDecimal(reader, "_tempumsatz", Nothing)
					overviewData._indumsatz = m_utility.SafeGetDecimal(reader, "_indumsatz", Nothing)
					overviewData._festumsatz = m_utility.SafeGetDecimal(reader, "_festumsatz", Nothing)

					overviewData.bruttolohn = m_utility.SafeGetDecimal(reader, "bruttolohn", Nothing)
					overviewData.ahvlohn = m_utility.SafeGetDecimal(reader, "ahvlohn", Nothing)
					overviewData.agbetrag = m_utility.SafeGetDecimal(reader, "agbetrag", Nothing)
					overviewData.fremdleistung = m_utility.SafeGetDecimal(reader, "fremdleistung", Nothing)

					overviewData.lohnaufwand_1 = m_utility.SafeGetDecimal(reader, "_lohnaufwand_1", Nothing)
					overviewData.lohnaufwand_2 = m_utility.SafeGetDecimal(reader, "_lohnaufwand_2", Nothing)
					overviewData._marge = m_utility.SafeGetDecimal(reader, "_marge", Nothing)

					overviewData._bgtemp = m_utility.SafeGetDecimal(reader, "_bgtemp", Nothing)
					overviewData._bgind = m_utility.SafeGetDecimal(reader, "_bgind", Nothing)
					overviewData._bgfest = m_utility.SafeGetDecimal(reader, "_bgfest", Nothing)


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function GetDbCustomerData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing

		Dim sql As String

		sql = String.Format("Select Umj.*, KD.Firma1 From {0} UMJ ", m_tblName)
		sql &= " Left Join Kunden KD On UMJ.CustomerNumber = KD.KDNr "
		sql &= "Order By KD.Firma1"

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.ID = m_utility.SafeGetInteger(reader, "ID", Nothing)
					overviewData.MDNr = m_InitialData.MDData.MDNr
					overviewData.monat = m_utility.SafeGetInteger(reader, "Monat", Nothing)
					overviewData.jahr = m_utility.SafeGetInteger(reader, "Jahr", Nothing)
					overviewData.CustomerNumber = m_utility.SafeGetInteger(reader, "CustomerNumber", Nothing)
					overviewData.EmployeeNumber = m_utility.SafeGetInteger(reader, "EmployeeNumber", Nothing)
					overviewData.EmploymentNumber = m_utility.SafeGetInteger(reader, "EmploymentNumber", Nothing)
					overviewData.CustomerName = m_utility.SafeGetString(reader, "Firma1")

					overviewData.kst3_1 = m_utility.SafeGetString(reader, "kst3_1")
					overviewData.kst3bez = m_utility.SafeGetString(reader, "kst3bez")
					overviewData.USFiliale = m_utility.SafeGetString(reader, "USFiliale")

					overviewData._tempumsatz = m_utility.SafeGetDecimal(reader, "_tempumsatz", Nothing)
					overviewData._indumsatz = m_utility.SafeGetDecimal(reader, "_indumsatz", Nothing)
					overviewData._festumsatz = m_utility.SafeGetDecimal(reader, "_festumsatz", Nothing)

					overviewData.bruttolohn = m_utility.SafeGetDecimal(reader, "bruttolohn", Nothing)
					overviewData.ahvlohn = m_utility.SafeGetDecimal(reader, "ahvlohn", Nothing)
					overviewData.agbetrag = m_utility.SafeGetDecimal(reader, "agbetrag", Nothing)
					overviewData.fremdleistung = m_utility.SafeGetDecimal(reader, "fremdleistung", Nothing)

					overviewData.lohnaufwand_1 = m_utility.SafeGetDecimal(reader, "_lohnaufwand_1", Nothing)
					overviewData.lohnaufwand_2 = m_utility.SafeGetDecimal(reader, "_lohnaufwand_2", Nothing)
					overviewData._marge = m_utility.SafeGetDecimal(reader, "_marge", Nothing)

					overviewData._bgtemp = m_utility.SafeGetDecimal(reader, "_bgtemp", Nothing)
					overviewData._bgind = m_utility.SafeGetDecimal(reader, "_bgind", Nothing)
					overviewData._bgfest = m_utility.SafeGetDecimal(reader, "_bgfest", Nothing)


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function



#Region "reset"

	Private Sub ResetGridSalaryData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMDNr.Caption = m_Translate.GetSafeTranslationValue("MDNr")
		columnMDNr.Name = "MDNr"
		columnMDNr.FieldName = "MDNr"
		columnMDNr.Visible = False
		columnMDNr.BestFit()
		gvRP.Columns.Add(columnMDNr)

		Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnMonat.Name = "monat"
		columnMonat.FieldName = "monat"
		columnMonat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnMonat.AppearanceHeader.Options.UseTextOptions = True
		columnMonat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnMonat.Visible = False
		columnMonat.BestFit()
		gvRP.Columns.Add(columnMonat)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjahr.AppearanceHeader.Options.UseTextOptions = True
		columnjahr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjahr.Visible = False
		columnjahr.BestFit()
		gvRP.Columns.Add(columnjahr)


		Dim columnkst3_1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst3_1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst3_1.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
		columnkst3_1.Name = "kst3_1"
		columnkst3_1.FieldName = "kst3_1"
		columnkst3_1.Visible = False
		columnkst3_1.BestFit()
		gvRP.Columns.Add(columnkst3_1)

		Dim columnkst3bez As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst3bez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst3bez.Caption = m_Translate.GetSafeTranslationValue("Berater")
		columnkst3bez.Name = "kst3bez"
		columnkst3bez.FieldName = "kst3bez"
		columnkst3bez.Visible = True
		columnkst3bez.BestFit()
		gvRP.Columns.Add(columnkst3bez)

		Dim columnUSFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUSFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUSFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
		columnUSFiliale.Name = "USFiliale"
		columnUSFiliale.FieldName = "USFiliale"
		columnUSFiliale.Visible = False
		columnUSFiliale.BestFit()
		gvRP.Columns.Add(columnUSFiliale)




		Dim column_Tempumsatz As New DevExpress.XtraGrid.Columns.GridColumn()
		column_Tempumsatz.Caption = m_Translate.GetSafeTranslationValue("Temporärumsatz")
		column_Tempumsatz.Name = "_tempumsatz"
		column_Tempumsatz.FieldName = "_tempumsatz"
		column_Tempumsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_Tempumsatz.AppearanceHeader.Options.UseTextOptions = True
		column_Tempumsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_Tempumsatz.DisplayFormat.FormatString = "N2"
		column_Tempumsatz.Visible = True
		column_Tempumsatz.BestFit()
		gvRP.Columns.Add(column_Tempumsatz)

		Dim column_indumsatz As New DevExpress.XtraGrid.Columns.GridColumn()
		column_indumsatz.Caption = m_Translate.GetSafeTranslationValue("Sonstige Umsätze")
		column_indumsatz.Name = "_indumsatz"
		column_indumsatz.FieldName = "_indumsatz"
		column_indumsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_indumsatz.AppearanceHeader.Options.UseTextOptions = True
		column_indumsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_indumsatz.DisplayFormat.FormatString = "N2"
		column_indumsatz.Visible = True
		column_indumsatz.BestFit()
		gvRP.Columns.Add(column_indumsatz)

		Dim column_festumsatz As New DevExpress.XtraGrid.Columns.GridColumn()
		column_festumsatz.Caption = m_Translate.GetSafeTranslationValue("Fest Umsätze")
		column_festumsatz.Name = "_festumsatz"
		column_festumsatz.FieldName = "_festumsatz"
		column_festumsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_festumsatz.AppearanceHeader.Options.UseTextOptions = True
		column_festumsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_festumsatz.DisplayFormat.FormatString = "N2"
		column_festumsatz.Visible = True
		column_festumsatz.BestFit()
		gvRP.Columns.Add(column_festumsatz)


		Dim columnbruttolohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbruttolohn.Caption = m_Translate.GetSafeTranslationValue("Bruttolohn")
		columnbruttolohn.Name = "bruttolohn"
		columnbruttolohn.FieldName = "bruttolohn"
		columnbruttolohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbruttolohn.AppearanceHeader.Options.UseTextOptions = True
		columnbruttolohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbruttolohn.DisplayFormat.FormatString = "N2"
		columnbruttolohn.Visible = True
		columnbruttolohn.BestFit()
		gvRP.Columns.Add(columnbruttolohn)

		Dim columnahvlohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvlohn.Caption = m_Translate.GetSafeTranslationValue("AHV-Lohn")
		columnahvlohn.Name = "ahvlohn"
		columnahvlohn.FieldName = "ahvlohn"
		columnahvlohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnahvlohn.AppearanceHeader.Options.UseTextOptions = True
		columnahvlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnahvlohn.DisplayFormat.FormatString = "N2"
		columnahvlohn.Visible = True
		columnahvlohn.BestFit()
		gvRP.Columns.Add(columnahvlohn)

		Dim columnagbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnagbetrag.Caption = m_Translate.GetSafeTranslationValue("AG-Beitrag")
		columnagbetrag.Name = "agbetrag"
		columnagbetrag.FieldName = "agbetrag"
		columnagbetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnagbetrag.AppearanceHeader.Options.UseTextOptions = True
		columnagbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnagbetrag.DisplayFormat.FormatString = "N2"
		columnagbetrag.Visible = True
		columnagbetrag.BestFit()
		gvRP.Columns.Add(columnagbetrag)

		Dim columnfremdleistung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfremdleistung.Caption = m_Translate.GetSafeTranslationValue("fremdleistung")
		columnfremdleistung.Name = "fremdleistung"
		columnfremdleistung.FieldName = "fremdleistung"
		columnfremdleistung.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnfremdleistung.AppearanceHeader.Options.UseTextOptions = True
		columnfremdleistung.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnfremdleistung.DisplayFormat.FormatString = "N2"
		columnfremdleistung.Visible = False
		columnfremdleistung.BestFit()
		gvRP.Columns.Add(columnfremdleistung)


		Dim columnlohnaufwand_1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlohnaufwand_1.Caption = m_Translate.GetSafeTranslationValue("1. Lohnaufwand")
		columnlohnaufwand_1.Name = "lohnaufwand_1"
		columnlohnaufwand_1.FieldName = "lohnaufwand_1"
		columnlohnaufwand_1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlohnaufwand_1.AppearanceHeader.Options.UseTextOptions = True
		columnlohnaufwand_1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlohnaufwand_1.DisplayFormat.FormatString = "N2"
		columnlohnaufwand_1.Visible = True
		columnlohnaufwand_1.BestFit()
		gvRP.Columns.Add(columnlohnaufwand_1)

		Dim columnlohnaufwand_2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlohnaufwand_2.Caption = m_Translate.GetSafeTranslationValue("2. Lohnaufwand")
		columnlohnaufwand_2.Name = "lohnaufwand_2"
		columnlohnaufwand_2.FieldName = "lohnaufwand_2"
		columnlohnaufwand_2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlohnaufwand_2.AppearanceHeader.Options.UseTextOptions = True
		columnlohnaufwand_2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlohnaufwand_2.DisplayFormat.FormatString = "N2"
		columnlohnaufwand_2.Visible = False
		columnlohnaufwand_2.BestFit()
		gvRP.Columns.Add(columnlohnaufwand_2)

		Dim column_marge As New DevExpress.XtraGrid.Columns.GridColumn()
		column_marge.Caption = m_Translate.GetSafeTranslationValue("Temporäre Marge")
		column_marge.Name = "_marge"
		column_marge.FieldName = "_marge"
		column_marge.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_marge.AppearanceHeader.Options.UseTextOptions = True
		column_marge.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_marge.DisplayFormat.FormatString = "N2"
		column_marge.Visible = True
		column_marge.BestFit()
		gvRP.Columns.Add(column_marge)

		Dim column_bgtemp As New DevExpress.XtraGrid.Columns.GridColumn()
		column_bgtemp.Caption = m_Translate.GetSafeTranslationValue("Bruttogewinn Temporär")
		column_bgtemp.Name = "_bgtemp"
		column_bgtemp.FieldName = "_bgtemp"
		column_bgtemp.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_bgtemp.AppearanceHeader.Options.UseTextOptions = True
		column_bgtemp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_bgtemp.DisplayFormat.FormatString = "N2"
		column_bgtemp.Visible = True
		column_bgtemp.BestFit()
		gvRP.Columns.Add(column_bgtemp)

		Dim column_bgind As New DevExpress.XtraGrid.Columns.GridColumn()
		column_bgind.Caption = m_Translate.GetSafeTranslationValue("Bruttogewinn sonst. Umsätze")
		column_bgind.Name = "_bgind"
		column_bgind.FieldName = "_bgind"
		column_bgind.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_bgind.AppearanceHeader.Options.UseTextOptions = True
		column_bgind.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_bgind.DisplayFormat.FormatString = "N2"
		column_bgind.Visible = False
		column_bgind.BestFit()
		gvRP.Columns.Add(column_bgind)

		Dim column_bgfest As New DevExpress.XtraGrid.Columns.GridColumn()
		column_bgfest.Caption = m_Translate.GetSafeTranslationValue("Bruttogewinn Fest")
		column_bgfest.Name = "_bgfest"
		column_bgfest.FieldName = "_bgfest"
		column_bgfest.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_bgfest.AppearanceHeader.Options.UseTextOptions = True
		column_bgfest.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_bgfest.DisplayFormat.FormatString = "N2"
		column_bgfest.Visible = False
		column_bgfest.BestFit()
		gvRP.Columns.Add(column_bgfest)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub

	'Private Sub ResetGridPayrollData()

	'	gvPayrollData.OptionsView.ShowIndicator = False
	'	gvPayrollData.OptionsView.ShowAutoFilterRow = True
	'	gvPayrollData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
	'	gvPayrollData.OptionsView.ShowFooter = True

	'	gvPayrollData.Columns.Clear()

	'	Dim columnKST As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnKST.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'	columnKST.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
	'	columnKST.Name = "KST"
	'	columnKST.FieldName = "KST"
	'	columnKST.Visible = True
	'	columnKST.BestFit()
	'	gvPayrollData.Columns.Add(columnKST)

	'	Dim columnpayrollNumber As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnpayrollNumber.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
	'	columnpayrollNumber.Name = "payrollNumber"
	'	columnpayrollNumber.FieldName = "payrollNumber"
	'	columnpayrollNumber.Visible = True
	'	columnpayrollNumber.BestFit()
	'	gvPayrollData.Columns.Add(columnpayrollNumber)

	'	Dim columnahvemployeeAmount As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnahvemployeeAmount.Caption = m_Translate.GetSafeTranslationValue("AHV-Beiträge pro Kandidat")
	'	columnahvemployeeAmount.Name = "ahvemployeeAmount"
	'	columnahvemployeeAmount.FieldName = "ahvemployeeAmount"
	'	columnahvemployeeAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'	columnahvemployeeAmount.AppearanceHeader.Options.UseTextOptions = True
	'	columnahvemployeeAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'	columnahvemployeeAmount.DisplayFormat.FormatString = "N2"
	'	columnahvemployeeAmount.Visible = True
	'	columnahvemployeeAmount.BestFit()
	'	columnahvemployeeAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
	'	columnahvemployeeAmount.SummaryItem.DisplayFormat = "{0:n2}"
	'	gvPayrollData.Columns.Add(columnahvemployeeAmount)

	'	Dim column_ahvAmount As New DevExpress.XtraGrid.Columns.GridColumn()
	'	column_ahvAmount.Caption = m_Translate.GetSafeTranslationValue("AHV-Beiträge")
	'	column_ahvAmount.Name = "_ahvAmount"
	'	column_ahvAmount.FieldName = "_ahvAmount"
	'	column_ahvAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'	column_ahvAmount.AppearanceHeader.Options.UseTextOptions = True
	'	column_ahvAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'	column_ahvAmount.DisplayFormat.FormatString = "N2"
	'	column_ahvAmount.Visible = True
	'	column_ahvAmount.BestFit()
	'	column_ahvAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
	'	column_ahvAmount.SummaryItem.DisplayFormat = "{0:n2}"
	'	gvPayrollData.Columns.Add(column_ahvAmount)

	'	Dim columnagemployeeAmount As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnagemployeeAmount.Caption = m_Translate.GetSafeTranslationValue("AG-Beiträge pro Kandidat")
	'	columnagemployeeAmount.Name = "agemployeeAmount"
	'	columnagemployeeAmount.FieldName = "agemployeeAmount"
	'	columnagemployeeAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'	columnagemployeeAmount.AppearanceHeader.Options.UseTextOptions = True
	'	columnagemployeeAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'	columnagemployeeAmount.DisplayFormat.FormatString = "N2"
	'	columnagemployeeAmount.Visible = True
	'	columnagemployeeAmount.BestFit()
	'	columnagemployeeAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
	'	columnagemployeeAmount.SummaryItem.DisplayFormat = "{0:n2}"
	'	gvPayrollData.Columns.Add(columnagemployeeAmount)

	'	Dim columnagemployeebvgAmount As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnagemployeebvgAmount.Caption = m_Translate.GetSafeTranslationValue("BVG-Beiträge pro Kandidat")
	'	columnagemployeebvgAmount.Name = "agemployeebvgAmount"
	'	columnagemployeebvgAmount.FieldName = "agemployeebvgAmount"
	'	columnagemployeebvgAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'	columnagemployeebvgAmount.AppearanceHeader.Options.UseTextOptions = True
	'	columnagemployeebvgAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'	columnagemployeebvgAmount.DisplayFormat.FormatString = "N2"
	'	columnagemployeebvgAmount.Visible = True
	'	columnagemployeebvgAmount.BestFit()
	'	columnagemployeebvgAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
	'	columnagemployeebvgAmount.SummaryItem.DisplayFormat = "{0:n2}"
	'	gvPayrollData.Columns.Add(columnagemployeebvgAmount)

	'	Dim column_agAmount As New DevExpress.XtraGrid.Columns.GridColumn()
	'	column_agAmount.Caption = m_Translate.GetSafeTranslationValue("AG-Beiträge")
	'	column_agAmount.Name = "_agAmount"
	'	column_agAmount.FieldName = "_agAmount"
	'	column_agAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'	column_agAmount.AppearanceHeader.Options.UseTextOptions = True
	'	column_agAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'	column_agAmount.DisplayFormat.FormatString = "N2"
	'	column_agAmount.Visible = True
	'	column_agAmount.BestFit()
	'	column_agAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
	'	column_agAmount.SummaryItem.DisplayFormat = "{0:n2}"
	'	gvPayrollData.Columns.Add(column_agAmount)


	'	grdPayrollData.DataSource = Nothing

	'End Sub

	Private Sub ResetGridPayrollData_Staging()

		'Return

		gvPayroll_Staging.OptionsView.ShowIndicator = False
		gvPayroll_Staging.OptionsView.ShowAutoFilterRow = False
		gvPayroll_Staging.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPayroll_Staging.OptionsView.ShowFooter = False
		gvPayroll_Staging.OptionsView.AllowCellMerge = False
		gvPayroll_Staging.OptionsBehavior.Editable = False
		gvPayroll_Staging.OptionsDetail.ShowDetailTabs = True

		'gvPayroll_Staging.OptionsDetail.DetailMode = DetailMode.Embedded

		gvPayroll_Staging.Columns.Clear()

		Dim columnKSTViewData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKSTViewData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKSTViewData.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
		columnKSTViewData.Name = "KSTViewData"
		columnKSTViewData.FieldName = "KSTViewData"
		columnKSTViewData.Visible = True
		columnKSTViewData.BestFit()
		gvPayroll_Staging.Columns.Add(columnKSTViewData)


		grdPayroll_Staging.DataSource = Nothing

	End Sub

	Private Sub ResetGridPayrollData_L2_Staging()

		'Return

		gvLevel1.OptionsView.ShowIndicator = False
		gvLevel1.OptionsView.ShowAutoFilterRow = False
		gvLevel1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvLevel1.OptionsView.ShowFooter = False
		gvLevel1.OptionsView.ShowGroupPanel = False
		gvLevel1.OptionsView.AllowCellMerge = False
		gvLevel1.OptionsBehavior.Editable = False
		gvLevel1.OptionsDetail.DetailMode = DetailMode.Embedded
		gvLevel1.OptionsDetail.ShowDetailTabs = False

		gvLevel1.Columns.Clear()


		Dim columnReportNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnReportNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnReportNumber.OptionsColumn.AllowEdit = False
		columnReportNumber.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnReportNumber.Name = "PayrollNumber"
		columnReportNumber.FieldName = "PayrollNumber"
		columnReportNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnReportNumber.AppearanceHeader.Options.UseTextOptions = True
		columnReportNumber.Visible = True
		columnReportNumber.Width = 100
		gvLevel1.Columns.Add(columnReportNumber)

		Dim columnLANumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLANumber.OptionsColumn.AllowEdit = False
		columnLANumber.Caption = m_Translate.GetSafeTranslationValue("Lohnart-Nr.")
		columnLANumber.Name = "LANumber"
		columnLANumber.FieldName = "LANumber"
		columnLANumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnLANumber.AppearanceHeader.Options.UseTextOptions = True
		columnLANumber.Visible = True
		columnLANumber.Width = 100
		gvLevel1.Columns.Add(columnLANumber)

		Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAmount.OptionsColumn.AllowEdit = False
		columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnAmount.Name = "Amount"
		columnAmount.FieldName = "Amount"
		columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAmount.DisplayFormat.FormatString = "N2"
		columnAmount.AppearanceHeader.Options.UseTextOptions = True
		columnAmount.Visible = True
		columnAmount.Width = 150
		gvLevel1.Columns.Add(columnAmount)

	End Sub

	Private Sub OngvPayroll_Staging_MasterRowExpanded(sender As Object, e As CustomMasterRowEventArgs) Handles gvPayroll_Staging.MasterRowExpanded

		'Return
		Dim view As GridView = TryCast(TryCast(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
		'Return
		view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		view.OptionsView.ShowIndicator = False
		view.OptionsBehavior.Editable = False
		view.OptionsView.ShowAutoFilterRow = False
		view.OptionsView.ColumnAutoWidth = False
		view.OptionsView.ShowFooter = False
		view.OptionsView.AllowHtmlDrawGroups = True
		view.OptionsBehavior.Editable = False
		view.OptionsView.AllowCellMerge = True
		view.OptionsDetail.DetailMode = DetailMode.Classic
		view.OptionsDetail.ShowDetailTabs = False

		'Return
		view.Columns.Clear()

		grdPayroll_Staging.LevelTree.Nodes(0).RelationName="Detailiert Lohnarten"


		If e.RelationIndex = 0 Then
			Dim columnDataType As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDataType.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDataType.OptionsColumn.AllowEdit = False
			columnDataType.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDataType.Name = "DataTypeLabel"
			columnDataType.FieldName = "DataTypeLabel"
			columnDataType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDataType.AppearanceHeader.Options.UseTextOptions = True
			columnDataType.Visible = True
			columnDataType.Width = 200
			view.Columns.Add(columnDataType)

			Dim columnDataTypeAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDataTypeAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDataTypeAmount.OptionsColumn.AllowEdit = False
			columnDataTypeAmount.Caption = m_Translate.GetSafeTranslationValue("Total Betrag")
			columnDataTypeAmount.Name = "DataTypeAmount"
			columnDataTypeAmount.FieldName = "DataTypeAmount"
			columnDataTypeAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnDataTypeAmount.AppearanceHeader.Options.UseTextOptions = True
			columnDataTypeAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnDataTypeAmount.DisplayFormat.FormatString = "N2"
			columnDataTypeAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnDataTypeAmount.AppearanceCell.Options.UseTextOptions = True
			columnDataTypeAmount.Visible = True
			columnDataTypeAmount.Width = 100
			view.Columns.Add(columnDataTypeAmount)


		ElseIf e.RelationIndex = 1 Then
			Dim columnPayrollNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPayrollNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPayrollNumber.OptionsColumn.AllowEdit = False
			columnPayrollNumber.Caption = m_Translate.GetSafeTranslationValue("Payroll-Number")
			columnPayrollNumber.Name = "PayrollNumber"
			columnPayrollNumber.FieldName = "PayrollNumber"
			columnPayrollNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			columnPayrollNumber.AppearanceHeader.Options.UseTextOptions = True
			columnPayrollNumber.Visible = True
			columnPayrollNumber.Width = 100
			view.Columns.Add(columnPayrollNumber)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLONr.OptionsColumn.AllowEdit = False
			columnLONr.Caption = m_Translate.GetSafeTranslationValue("AHV-Anteil")
			columnLONr.Name = "AHVAnteil"
			columnLONr.FieldName = "AHVAnteil"
			columnLONr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnLONr.AppearanceHeader.Options.UseTextOptions = True
			columnLONr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnLONr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLONr.DisplayFormat.FormatString = "N2"
			columnLONr.AppearanceCell.Options.UseTextOptions = True
			columnLONr.Width = 100
			columnLONr.Visible = True
			view.Columns.Add(columnLONr)

			Dim columnAHVAmountEachEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAHVAmountEachEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAHVAmountEachEmployee.OptionsColumn.AllowEdit = False
			columnAHVAmountEachEmployee.Caption = m_Translate.GetSafeTranslationValue("AHVAmount-Employee")
			columnAHVAmountEachEmployee.Name = "AHVAmountEachEmployee"
			columnAHVAmountEachEmployee.FieldName = "AHVAmountEachEmployee"
			columnAHVAmountEachEmployee.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAHVAmountEachEmployee.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAHVAmountEachEmployee.DisplayFormat.FormatString = "N2"
			columnAHVAmountEachEmployee.AppearanceHeader.Options.UseTextOptions = True
			columnAHVAmountEachEmployee.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAHVAmountEachEmployee.AppearanceCell.Options.UseTextOptions = True
			columnAHVAmountEachEmployee.Width = 100
			columnAHVAmountEachEmployee.Visible = False
			view.Columns.Add(columnAHVAmountEachEmployee)

			Dim columnAGAnteil As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAGAnteil.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAGAnteil.OptionsColumn.AllowEdit = False
			columnAGAnteil.Caption = m_Translate.GetSafeTranslationValue("AG-Anteil")
			columnAGAnteil.Name = "AGAnteil"
			columnAGAnteil.FieldName = "AGAnteil"
			columnAGAnteil.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGAnteil.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAGAnteil.DisplayFormat.FormatString = "N2"
			columnAGAnteil.AppearanceHeader.Options.UseTextOptions = True
			columnAGAnteil.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGAnteil.AppearanceCell.Options.UseTextOptions = True
			columnAGAnteil.Width = 100
			columnAGAnteil.Visible = True
			view.Columns.Add(columnAGAnteil)

			Dim columnAGAmountEachEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAGAmountEachEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAGAmountEachEmployee.OptionsColumn.AllowEdit = False
			columnAGAmountEachEmployee.Caption = m_Translate.GetSafeTranslationValue("AGAmount-Employee")
			columnAGAmountEachEmployee.Name = "AGAmountEachEmployee"
			columnAGAmountEachEmployee.FieldName = "AGAmountEachEmployee"
			columnAGAmountEachEmployee.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGAmountEachEmployee.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAGAmountEachEmployee.DisplayFormat.FormatString = "N2"
			columnAGAmountEachEmployee.AppearanceHeader.Options.UseTextOptions = True
			columnAGAmountEachEmployee.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGAmountEachEmployee.AppearanceCell.Options.UseTextOptions = True
			columnAGAmountEachEmployee.Width = 100
			columnAGAmountEachEmployee.Visible = False
			view.Columns.Add(columnAGAmountEachEmployee)

			Dim columnAGBVGAmountEachEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAGBVGAmountEachEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAGBVGAmountEachEmployee.OptionsColumn.AllowEdit = False
			columnAGBVGAmountEachEmployee.Caption = m_Translate.GetSafeTranslationValue("AGBVGAmount-Employee")
			columnAGBVGAmountEachEmployee.Name = "AGBVGAmountEachEmployee"
			columnAGBVGAmountEachEmployee.FieldName = "AGBVGAmountEachEmployee"
			columnAGBVGAmountEachEmployee.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGBVGAmountEachEmployee.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAGBVGAmountEachEmployee.DisplayFormat.FormatString = "N2"
			columnAGBVGAmountEachEmployee.AppearanceHeader.Options.UseTextOptions = True
			columnAGBVGAmountEachEmployee.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGBVGAmountEachEmployee.AppearanceCell.Options.UseTextOptions = True
			columnAGBVGAmountEachEmployee.Width = 100
			columnAGBVGAmountEachEmployee.Visible = False
			view.Columns.Add(columnAGBVGAmountEachEmployee)

			Dim columnAGKSTProcent As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAGKSTProcent.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAGKSTProcent.OptionsColumn.AllowEdit = False
			columnAGKSTProcent.Caption = m_Translate.GetSafeTranslationValue("AGKST-Procent")
			columnAGKSTProcent.Name = "AGKSTProcent"
			columnAGKSTProcent.FieldName = "AGKSTProcent"
			columnAGKSTProcent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGKSTProcent.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAGKSTProcent.DisplayFormat.FormatString = "N2"
			columnAGKSTProcent.AppearanceHeader.Options.UseTextOptions = True
			columnAGKSTProcent.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGKSTProcent.AppearanceCell.Options.UseTextOptions = True
			columnAGKSTProcent.Width = 100
			columnAGKSTProcent.Visible = True
			view.Columns.Add(columnAGKSTProcent)

			Dim columnAGEOAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAGEOAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAGEOAmount.OptionsColumn.AllowEdit = False
			columnAGEOAmount.Caption = m_Translate.GetSafeTranslationValue("AGEO-Amount")
			columnAGEOAmount.Name = "AGEOAmount"
			columnAGEOAmount.FieldName = "AGEOAmount"
			columnAGEOAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGEOAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAGEOAmount.DisplayFormat.FormatString = "N2"
			columnAGEOAmount.AppearanceHeader.Options.UseTextOptions = True
			columnAGEOAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAGEOAmount.AppearanceCell.Options.UseTextOptions = True
			columnAGEOAmount.Width = 100
			columnAGEOAmount.Visible = False
			view.Columns.Add(columnAGEOAmount)


		Else
			Return

		End If

		Dim detailView As GridView = CType(CType(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
		RemoveHandler detailView.RowCellClick, AddressOf Ongv_RowCellClick
		RemoveHandler detailView.RowCellClick, AddressOf OngvAG_RowCellClick
		RemoveHandler detailView.MasterRowExpanded, AddressOf Ongv_MasterExpanded
		If (Not (detailView) Is Nothing) Then
			'detailView.ParentView
			If e.RelationIndex = 1 Then AddHandler detailView.RowCellClick, AddressOf OngvAG_RowCellClick
			AddHandler detailView.MasterRowExpanded, AddressOf Ongv_MasterExpanded
		End If

	End Sub

	Private Sub Ongv_MasterExpanded(sender As Object, e As CustomMasterRowEventArgs)
		Dim view As GridView = TryCast(TryCast(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
		If view Is Nothing Then Return

		view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		view.OptionsView.ShowIndicator = False
		view.OptionsBehavior.Editable = False
		view.OptionsView.ShowAutoFilterRow = False
		view.OptionsView.ColumnAutoWidth = False
		view.OptionsView.ShowFooter = False
		view.OptionsView.AllowHtmlDrawGroups = True
		view.OptionsBehavior.Editable = False
		view.OptionsView.AllowCellMerge = True
		view.OptionsDetail.DetailMode = DetailMode.Classic
		view.OptionsDetail.ShowDetailTabs = False

		view.Columns.Clear()

		Dim columnReportNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnReportNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnReportNumber.OptionsColumn.AllowEdit = False
		columnReportNumber.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnReportNumber.Name = "PayrollNumber"
		columnReportNumber.FieldName = "PayrollNumber"
		columnReportNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnReportNumber.AppearanceHeader.Options.UseTextOptions = True
		columnReportNumber.Visible = True
		columnReportNumber.Width = 100
		view.Columns.Add(columnReportNumber)

		Dim columnLANumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLANumber.OptionsColumn.AllowEdit = False
		columnLANumber.Caption = m_Translate.GetSafeTranslationValue("Lohnart-Nr.")
		columnLANumber.Name = "LANumber"
		columnLANumber.FieldName = "LANumber"
		columnLANumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnLANumber.AppearanceHeader.Options.UseTextOptions = True
		columnLANumber.Visible = True
		columnLANumber.Width = 100
		view.Columns.Add(columnLANumber)

		Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAmount.OptionsColumn.AllowEdit = False
		columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnAmount.Name = "Amount"
		columnAmount.FieldName = "Amount"
		columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAmount.DisplayFormat.FormatString = "N2"
		columnAmount.AppearanceHeader.Options.UseTextOptions = True
		columnAmount.Visible = True
		columnAmount.Width = 150
		view.Columns.Add(columnAmount)


		Dim detailView As GridView = CType(CType(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
		RemoveHandler detailView.RowCellClick, AddressOf Ongv_RowCellClick
		If (Not (detailView) Is Nothing) Then
			'detailView.ParentView
			AddHandler detailView.RowCellClick, AddressOf Ongv_RowCellClick
		End If


	End Sub

	Sub OngvAG_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			If e.Column.Name <> "PayrollNumber" Then Return

			Dim column = e.Column
			Dim detailView As GridView = CType(sender, GridView)
			If (Not (detailView) Is Nothing) Then
				Dim dataRow = detailView.GetRow(e.RowHandle)

				Dim viewData = SelectedPayrollAGDataRecord(detailView)
				If viewData Is Nothing Then Return

				If viewData.PayrollNumber > 0 Then OpenAssignedPayroll(viewData.PayrollNumber)
			End If
		End If

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			If e.Column.Name <> "PayrollNumber" Then Return

			Dim column = e.Column
			Dim detailView As GridView = CType(sender, GridView)
			If (Not (detailView) Is Nothing) Then
				Dim dataRow = detailView.GetRow(e.RowHandle)

				Dim viewData = SelectedPayrollDataRecord(detailView)
				If viewData Is Nothing Then Return

				If viewData.PayrollNumber > 0 Then OpenAssignedPayroll(viewData.PayrollNumber)
			End If
		End If

	End Sub

	Private Sub OpenAssignedPayroll(ByVal payrollNumber As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try

			Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With
				{.SelectedMDNr = m_InitialData.MDData.MDNr,
				 .SelectedMANr = New List(Of Integer)(New Integer() {0}),
				 .SelectedLONr = New List(Of Integer)(New Integer() {payrollNumber}),
				 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
				 .SelectedYear = New List(Of Integer)(New Integer() {0}), .SearchAutomatic = True}

			'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(init, _settring)
			'obj.ShowfrmLO4Details()

			Dim obj As New SP.LO.PrintUtility.frmLOPrint(m_InitialData)
			obj.LOSetting = _settring

			obj.Show()
			obj.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUi.ShowOKDialog(String.Format("{0}: {1}", strMethodeName, ex.ToString), m_Translate.GetSafeTranslationValue("Lohnabrechnung öffnen"), MessageBoxIcon.Error)

		End Try

	End Sub

	Private Sub OngvPayroll_Staging_MasterRowGetRelationCount(ByVal sender As Object, ByVal e As MasterRowGetRelationCountEventArgs) Handles gvPayroll_Staging.MasterRowGetRelationCount
		e.RelationCount = 2
	End Sub

	'Private Sub OngvPayroll_Staging_MasterRowGetRelationName(sender As Object, e As MasterRowGetRelationNameEventArgs) Handles gvPayroll_Staging.MasterRowGetRelationName

	'	e.RelationName = "Lohn-Struckturen"
	'	'If e.RelationIndex = 0 Then e.RelationName = "Lohn-Struckturen"
	'	'If e.RelationIndex = 1 Then e.RelationName = "Details zur AG-Beiträge"

	'End Sub

	Private Sub OngvPayroll_Staging_MasterRowGetRelationDisplayCaption(sender As Object, e As MasterRowGetRelationNameEventArgs) Handles gvPayroll_Staging.MasterRowGetRelationDisplayCaption

		'e.RelationName = "Lohn-Struckturen"
		If e.RelationIndex = 0 Then e.RelationName = "Lohn-Struckturen"
		If e.RelationIndex = 1 Then e.RelationName = "Details zur AG-Beiträge"

	End Sub
	Private Sub OngvPayroll_Staging_MasterRowEmpty(ByVal sender As Object, ByVal e As MasterRowEmptyEventArgs) Handles gvPayroll_Staging.MasterRowEmpty
		e.IsEmpty = False
	End Sub



	'Private Sub OngvPrint_MasterRowGetRelationCount(ByVal sender As Object, ByVal e As MasterRowGetRelationCountEventArgs) Handles gvPrint.MasterRowGetRelationCount
	'	e.RelationCount = 2
	'End Sub

	'Private Sub OngvPrint_Staging_MasterRowGetRelationName(sender As Object, e As MasterRowGetRelationNameEventArgs) Handles gvPrint.MasterRowGetRelationName

	'	If e.RelationIndex = 0 Then e.RelationName = "Lohn-Struckturen"
	'	If e.RelationIndex = 1 Then e.RelationName = "Details zur AG-Beiträge"

	'End Sub

	'Private Sub OngvLevel1_MasterRowEmpty(ByVal sender As Object, ByVal e As MasterRowEmptyEventArgs) Handles gvLevel1.MasterRowEmpty
	'	e.IsEmpty = False
	'End Sub


	'Private Sub OngvPayroll_Staging_MasterRowGetChildList(ByVal sender As Object, ByVal e As MasterRowGetChildListEventArgs) Handles gvPayroll_Staging.MasterRowGetChildList
	'	'Dim dunningdata = SelectedPayrollDataRecord
	'	'Dim listOfInvoiceData = dunningdata.PayrollListData

	'	''Dim details As New BindingList(Of listOfInvoiceData)
	'	'e.ChildList = listOfInvoiceData
	'	''Dim child As New InvoiceDunningPrintViewData
	'	'For Each itm In listOfInvoiceData
	'	'	'	child = New InvoiceDunningPrintViewData

	'	'	'	child.ReNr = itm.ReNr
	'	'	'	child.BetragInk = itm.BetragInk
	'	'	'	child.FakDat = itm.FakDat
	'	'	'	child.Art = itm.Art
	'	'	'	child.KST = itm.KST

	'	'	If itm.DataType = DB1DataRecordType.BRUTTOLOHN = itm.DataType =

	'	'Next

	'End Sub



	Private Sub ResetGridInvoiceData()

		gvInvoiceData.OptionsView.ShowIndicator = False
		gvInvoiceData.OptionsView.ShowAutoFilterRow = True
		gvInvoiceData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvInvoiceData.OptionsView.ShowFooter = True
		gvInvoiceData.OptionsView.AllowHtmlDrawGroups = True

		gvInvoiceData.Columns.Clear()

		Dim columnKST As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKST.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKST.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
		columnKST.Name = "KST"
		columnKST.FieldName = "KST"
		columnKST.Visible = True
		columnKST.BestFit()
		gvInvoiceData.Columns.Add(columnKST)

		Dim columnreportNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnreportNumber.Caption = m_Translate.GetSafeTranslationValue("Rapport-Nr.")
		columnreportNumber.Name = "reportNumber"
		columnreportNumber.FieldName = "reportNumber"
		columnreportNumber.Visible = True
		columnreportNumber.BestFit()
		gvInvoiceData.Columns.Add(columnreportNumber)

		Dim columninvoicenNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columninvoicenNumber.Caption = m_Translate.GetSafeTranslationValue("Rechnungs-Nr.")
		columninvoicenNumber.Name = "invoicenNumber"
		columninvoicenNumber.FieldName = "invoicenNumber"
		columninvoicenNumber.Visible = True
		columninvoicenNumber.BestFit()
		gvInvoiceData.Columns.Add(columninvoicenNumber)

		Dim columninvoiceArt As New DevExpress.XtraGrid.Columns.GridColumn()
		columninvoiceArt.Caption = m_Translate.GetSafeTranslationValue("Art")
		columninvoiceArt.Name = "invoiceArt"
		columninvoiceArt.FieldName = "invoiceArt"
		columninvoiceArt.Visible = True
		columninvoiceArt.BestFit()
		gvInvoiceData.Columns.Add(columninvoiceArt)

		Dim columninvoiceAmount As New DevExpress.XtraGrid.Columns.GridColumn()
		columninvoiceAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columninvoiceAmount.Name = "invoiceAmount"
		columninvoiceAmount.FieldName = "invoiceAmount"
		columninvoiceAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columninvoiceAmount.AppearanceHeader.Options.UseTextOptions = True
		columninvoiceAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columninvoiceAmount.DisplayFormat.FormatString = "N2"
		columninvoiceAmount.Visible = True
		columninvoiceAmount.BestFit()
		columninvoiceAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columninvoiceAmount.SummaryItem.DisplayFormat = "{0:n2}"
		gvInvoiceData.Columns.Add(columninvoiceAmount)


		Dim grpinvoiceArt = New GridGroupSummaryItem()
		grpinvoiceArt.FieldName = "invoiceAmount"
		grpinvoiceArt.SummaryType = DevExpress.Data.SummaryItemType.Sum
		grpinvoiceArt.DisplayFormat = m_Translate.GetSafeTranslationValue("Totalbetrag") & " = {0:n2}"
		gvInvoiceData.GroupFormat = "{1}: {2}"
		gvInvoiceData.GroupSummary.Add(grpinvoiceArt)


		grdInvoiceData.DataSource = Nothing

	End Sub

	Private Sub ResetGridCustomerData()

		gvCustomerJournal.OptionsView.ShowIndicator = False
		gvCustomerJournal.OptionsView.ShowAutoFilterRow = True
		gvCustomerJournal.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvCustomerJournal.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.BestFit()
		gvCustomerJournal.Columns.Add(columnCustomerNumber)

		Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnMonat.Name = "monat"
		columnMonat.FieldName = "monat"
		columnMonat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnMonat.AppearanceHeader.Options.UseTextOptions = True
		columnMonat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnMonat.Visible = False
		columnMonat.BestFit()
		gvCustomerJournal.Columns.Add(columnMonat)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnjahr.AppearanceHeader.Options.UseTextOptions = True
		columnjahr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnjahr.Visible = False
		columnjahr.BestFit()
		gvCustomerJournal.Columns.Add(columnjahr)


		Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerName.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCustomerName.Name = "CustomerName"
		columnCustomerName.FieldName = "CustomerName"
		columnCustomerName.Visible = True
		columnCustomerName.BestFit()
		gvCustomerJournal.Columns.Add(columnCustomerName)

		Dim columnkst3bez As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst3bez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst3bez.Caption = m_Translate.GetSafeTranslationValue("Berater")
		columnkst3bez.Name = "kst3bez"
		columnkst3bez.FieldName = "kst3bez"
		columnkst3bez.Visible = False
		columnkst3bez.BestFit()
		gvCustomerJournal.Columns.Add(columnkst3bez)

		Dim columnUSFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUSFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUSFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
		columnUSFiliale.Name = "USFiliale"
		columnUSFiliale.FieldName = "USFiliale"
		columnUSFiliale.Visible = False
		columnUSFiliale.BestFit()
		gvCustomerJournal.Columns.Add(columnUSFiliale)




		Dim column_Tempumsatz As New DevExpress.XtraGrid.Columns.GridColumn()
		column_Tempumsatz.Caption = m_Translate.GetSafeTranslationValue("Temporärumsatz")
		column_Tempumsatz.Name = "_tempumsatz"
		column_Tempumsatz.FieldName = "_tempumsatz"
		column_Tempumsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_Tempumsatz.AppearanceHeader.Options.UseTextOptions = True
		column_Tempumsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_Tempumsatz.DisplayFormat.FormatString = "N2"
		column_Tempumsatz.Visible = True
		column_Tempumsatz.BestFit()
		gvCustomerJournal.Columns.Add(column_Tempumsatz)

		Dim column_indumsatz As New DevExpress.XtraGrid.Columns.GridColumn()
		column_indumsatz.Caption = m_Translate.GetSafeTranslationValue("Sonstige Umsätze")
		column_indumsatz.Name = "_indumsatz"
		column_indumsatz.FieldName = "_indumsatz"
		column_indumsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_indumsatz.AppearanceHeader.Options.UseTextOptions = True
		column_indumsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_indumsatz.DisplayFormat.FormatString = "N2"
		column_indumsatz.Visible = True
		column_indumsatz.BestFit()
		gvCustomerJournal.Columns.Add(column_indumsatz)

		Dim column_festumsatz As New DevExpress.XtraGrid.Columns.GridColumn()
		column_festumsatz.Caption = m_Translate.GetSafeTranslationValue("Fest Umsätze")
		column_festumsatz.Name = "_festumsatz"
		column_festumsatz.FieldName = "_festumsatz"
		column_festumsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_festumsatz.AppearanceHeader.Options.UseTextOptions = True
		column_festumsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_festumsatz.DisplayFormat.FormatString = "N2"
		column_festumsatz.Visible = True
		column_festumsatz.BestFit()
		gvCustomerJournal.Columns.Add(column_festumsatz)


		Dim columnbruttolohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbruttolohn.Caption = m_Translate.GetSafeTranslationValue("Bruttolohn")
		columnbruttolohn.Name = "bruttolohn"
		columnbruttolohn.FieldName = "bruttolohn"
		columnbruttolohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbruttolohn.AppearanceHeader.Options.UseTextOptions = True
		columnbruttolohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbruttolohn.DisplayFormat.FormatString = "N2"
		columnbruttolohn.Visible = True
		columnbruttolohn.BestFit()
		gvCustomerJournal.Columns.Add(columnbruttolohn)

		Dim columnahvlohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvlohn.Caption = m_Translate.GetSafeTranslationValue("AHV-Lohn")
		columnahvlohn.Name = "ahvlohn"
		columnahvlohn.FieldName = "ahvlohn"
		columnahvlohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnahvlohn.AppearanceHeader.Options.UseTextOptions = True
		columnahvlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnahvlohn.DisplayFormat.FormatString = "N2"
		columnahvlohn.Visible = True
		columnahvlohn.BestFit()
		gvCustomerJournal.Columns.Add(columnahvlohn)

		Dim columnagbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnagbetrag.Caption = m_Translate.GetSafeTranslationValue("AG-Beitrag")
		columnagbetrag.Name = "agbetrag"
		columnagbetrag.FieldName = "agbetrag"
		columnagbetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnagbetrag.AppearanceHeader.Options.UseTextOptions = True
		columnagbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnagbetrag.DisplayFormat.FormatString = "N2"
		columnagbetrag.Visible = True
		columnagbetrag.BestFit()
		gvCustomerJournal.Columns.Add(columnagbetrag)

		Dim columnfremdleistung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfremdleistung.Caption = m_Translate.GetSafeTranslationValue("fremdleistung")
		columnfremdleistung.Name = "fremdleistung"
		columnfremdleistung.FieldName = "fremdleistung"
		columnfremdleistung.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnfremdleistung.AppearanceHeader.Options.UseTextOptions = True
		columnfremdleistung.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnfremdleistung.DisplayFormat.FormatString = "N2"
		columnfremdleistung.Visible = False
		columnfremdleistung.BestFit()
		gvCustomerJournal.Columns.Add(columnfremdleistung)


		Dim columnlohnaufwand_1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlohnaufwand_1.Caption = m_Translate.GetSafeTranslationValue("1. Lohnaufwand")
		columnlohnaufwand_1.Name = "lohnaufwand_1"
		columnlohnaufwand_1.FieldName = "lohnaufwand_1"
		columnlohnaufwand_1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlohnaufwand_1.AppearanceHeader.Options.UseTextOptions = True
		columnlohnaufwand_1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlohnaufwand_1.DisplayFormat.FormatString = "N2"
		columnlohnaufwand_1.Visible = True
		columnlohnaufwand_1.BestFit()
		gvCustomerJournal.Columns.Add(columnlohnaufwand_1)

		Dim columnlohnaufwand_2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlohnaufwand_2.Caption = m_Translate.GetSafeTranslationValue("2. Lohnaufwand")
		columnlohnaufwand_2.Name = "lohnaufwand_2"
		columnlohnaufwand_2.FieldName = "lohnaufwand_2"
		columnlohnaufwand_2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnlohnaufwand_2.AppearanceHeader.Options.UseTextOptions = True
		columnlohnaufwand_2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnlohnaufwand_2.DisplayFormat.FormatString = "N2"
		columnlohnaufwand_2.Visible = False
		columnlohnaufwand_2.BestFit()
		gvCustomerJournal.Columns.Add(columnlohnaufwand_2)

		Dim column_marge As New DevExpress.XtraGrid.Columns.GridColumn()
		column_marge.Caption = m_Translate.GetSafeTranslationValue("Temporäre Marge")
		column_marge.Name = "_marge"
		column_marge.FieldName = "_marge"
		column_marge.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_marge.AppearanceHeader.Options.UseTextOptions = True
		column_marge.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_marge.DisplayFormat.FormatString = "N2"
		column_marge.Visible = True
		column_marge.BestFit()
		gvCustomerJournal.Columns.Add(column_marge)

		Dim column_bgtemp As New DevExpress.XtraGrid.Columns.GridColumn()
		column_bgtemp.Caption = m_Translate.GetSafeTranslationValue("Bruttogewinn Temporär")
		column_bgtemp.Name = "_bgtemp"
		column_bgtemp.FieldName = "_bgtemp"
		column_bgtemp.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_bgtemp.AppearanceHeader.Options.UseTextOptions = True
		column_bgtemp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_bgtemp.DisplayFormat.FormatString = "N2"
		column_bgtemp.Visible = True
		column_bgtemp.BestFit()
		gvCustomerJournal.Columns.Add(column_bgtemp)

		Dim column_bgind As New DevExpress.XtraGrid.Columns.GridColumn()
		column_bgind.Caption = m_Translate.GetSafeTranslationValue("Bruttogewinn sonst. Umsätze")
		column_bgind.Name = "_bgind"
		column_bgind.FieldName = "_bgind"
		column_bgind.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_bgind.AppearanceHeader.Options.UseTextOptions = True
		column_bgind.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_bgind.DisplayFormat.FormatString = "N2"
		column_bgind.Visible = False
		column_bgind.BestFit()
		gvCustomerJournal.Columns.Add(column_bgind)

		Dim column_bgfest As New DevExpress.XtraGrid.Columns.GridColumn()
		column_bgfest.Caption = m_Translate.GetSafeTranslationValue("Bruttogewinn Fest")
		column_bgfest.Name = "_bgfest"
		column_bgfest.FieldName = "_bgfest"
		column_bgfest.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		column_bgfest.AppearanceHeader.Options.UseTextOptions = True
		column_bgfest.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		column_bgfest.DisplayFormat.FormatString = "N2"
		column_bgfest.Visible = False
		column_bgfest.BestFit()
		gvCustomerJournal.Columns.Add(column_bgfest)


		grdCustomerJournal.DataSource = Nothing

	End Sub


#End Region


#Region "Form Properties..."


	Private Sub OnFrmDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(xtabAllgemein.Text)
			Me.xtabInvoices.Text = m_Translate.GetSafeTranslationValue(xtabInvoices.Text)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.ToString))
		End Try

	End Sub

	Private Sub OnFrmLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

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
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

		End Try

		Try


		Catch ex As Exception

		End Try

	End Sub


#End Region


	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.RecCount = gvRP.RowCount
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)

	End Sub

	Private Sub OnGvCurrentList_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "_tempumsatz" OrElse e.Column.FieldName = "_indumsatz" OrElse e.Column.FieldName = "_festumsatz" OrElse
			e.Column.FieldName = "bruttolohn" OrElse e.Column.FieldName = "ahvlohn" OrElse e.Column.FieldName = "agbetrag" OrElse
			e.Column.FieldName = "fremdleistung" OrElse e.Column.FieldName = "lohnaufwand_1" OrElse e.Column.FieldName = "lohnaufwand_2" OrElse
			e.Column.FieldName = "_marge" OrElse
			e.Column.FieldName = "_bgtemp" OrElse e.Column.FieldName = "_bgind" OrElse e.Column.FieldName = "_bgfest" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	'Private Sub OngvPayrollData_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs)

	'	If e.Column.FieldName = "ahvemployeeAmount" OrElse e.Column.FieldName = "_ahvAmount" OrElse e.Column.FieldName = "agemployeeAmount" OrElse
	'		e.Column.FieldName = "agemployeebvgAmount" OrElse e.Column.FieldName = "_agAmount" Then
	'		If Val(e.Value) = 0 Then e.DisplayText = String.Empty
	'	End If

	'End Sub

	Private Sub OngvInvoiceData_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvInvoiceData.CustomColumnDisplayText

		If e.Column.FieldName = "invoiceAmount" OrElse e.Column.FieldName = "reportNumber" OrElse e.Column.FieldName = "invoicenNumber" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvCustomerJournal_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvCustomerJournal.CustomColumnDisplayText

		If e.Column.FieldName = "_tempumsatz" OrElse e.Column.FieldName = "_indumsatz" OrElse e.Column.FieldName = "_festumsatz" OrElse
			e.Column.FieldName = "bruttolohn" OrElse e.Column.FieldName = "ahvlohn" OrElse e.Column.FieldName = "agbetrag" OrElse
			e.Column.FieldName = "fremdleistung" OrElse e.Column.FieldName = "lohnaufwand_1" OrElse e.Column.FieldName = "lohnaufwand_2" OrElse
			e.Column.FieldName = "_marge" OrElse
			e.Column.FieldName = "_bgtemp" OrElse e.Column.FieldName = "_bgind" OrElse e.Column.FieldName = "_bgfest" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoYFAKSearchSetting, False), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoYFAKSearchFilter, False), False)

		Catch ex As Exception

		End Try
		If File.Exists(m_GVSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = "Keine Daten sind vorhanden"

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

End Class
