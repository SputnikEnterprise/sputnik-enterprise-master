
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.IO

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SPOPSearch.ClsDataDetail
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Logging

Public Class frmOPSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_utility As Utilities
	Private m_utilityUI As UtilityUI

	Private m_md As Mandant

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Private Property _dBetragohne As Decimal?
	Private Property _dBetragex As Decimal?
	Private Property _dBetragink As Decimal?
	Private Property _dBetragmwst1 As Decimal?
	Private Property _dBetragbezahlt As Decimal?

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreSearchSetting As String
	Private m_xmlSettingSearchFilter As String


#Region "Private Consts"


	Private Const MODUL_NAME_SETTING = "invoicesearch"

	Private Const USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/invoicesearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/invoicesearch/{1}/keepfilter"


#End Region



#Region "Constructor..."

	Public Sub New(ByVal strQuery As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_utilityUI = New UtilityUI

		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery

		_dBetragohne = 0
		_dBetragex = 0
		_dBetragink = 0
		_dBetragmwst1 = 0
		_dBetragbezahlt = 0

		Try
			m_GridSettingPath = String.Format("{0}InvoiceSearch\", m_md.GetGridSettingPath(m_InitialData.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, m_InitialData.UserData.UserNr)

			m_xmlSettingRestoreSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_RESTORE, m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_SEARCH_GRIDSETTING_FILTER, m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(m_InitialData.MDData.MDNr))

		Catch ex As Exception

		End Try


		ResetGridCustomerData()

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


	Function GetDbCustomerData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData
					overviewData.RENr = CInt(m_utility.SafeGetInteger(reader, "RENr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))

					overviewData.reart = m_utility.SafeGetString(reader, "art")
					overviewData.kst = m_utility.SafeGetString(reader, "kst")

					overviewData.fakdat = m_utility.SafeGetDateTime(reader, "fak_dat", Nothing)
					overviewData.currency = String.Format("{0}", m_utility.SafeGetString(reader, "currency"))

					overviewData.betragohne = m_utility.SafeGetDecimal(reader, "betragohne", Nothing)
					overviewData.betragex = m_utility.SafeGetDecimal(reader, "betragex", Nothing)
					overviewData.betragink = m_utility.SafeGetDecimal(reader, "betragink", Nothing)
					overviewData.mwst1 = m_utility.SafeGetDecimal(reader, "mwst1", Nothing)
					overviewData.mwstproz = m_utility.SafeGetDecimal(reader, "mwstproz", Nothing)
					overviewData.bezahlt = m_utility.SafeGetDecimal(reader, "bezahlt", Nothing)

					_dBetragohne += overviewData.betragohne
					_dBetragex += overviewData.betragex
					_dBetragink += overviewData.betragink
					_dBetragmwst1 += overviewData.mwst1
					_dBetragbezahlt += overviewData.bezahlt

					overviewData.skonto = m_utility.SafeGetInteger(reader, "fskonto", Nothing)


					overviewData.faellig = m_utility.SafeGetDateTime(reader, "faellig", Nothing)
					overviewData.mahncode = m_utility.SafeGetString(reader, "mahncode")

					overviewData.ma0 = m_utility.SafeGetDateTime(reader, "ma0", Nothing)
					overviewData.ma1 = m_utility.SafeGetDateTime(reader, "ma1", Nothing)
					overviewData.ma2 = m_utility.SafeGetDateTime(reader, "ma2", Nothing)
					overviewData.ma3 = m_utility.SafeGetDateTime(reader, "ma3", Nothing)


					overviewData.fksoll = m_utility.SafeGetInteger(reader, "fksoll", Nothing)
					overviewData.fkhaben0 = m_utility.SafeGetInteger(reader, "fkhaben0", Nothing)
					overviewData.fkhaben1 = m_utility.SafeGetInteger(reader, "fkhaben1", Nothing)

					overviewData.rname1 = m_utility.SafeGetString(reader, "r_name1")
					overviewData.rname2 = m_utility.SafeGetString(reader, "r_name2")
					overviewData.rname3 = m_utility.SafeGetString(reader, "r_name3")
					overviewData.rzhd = m_utility.SafeGetString(reader, "r_zhd")
					overviewData.rabteilung = m_utility.SafeGetString(reader, "r_abteilung")
					overviewData.rpostfach = m_utility.SafeGetString(reader, "r_postfach")
					overviewData.rstrasse = m_utility.SafeGetString(reader, "r_strasse")
					overviewData.rplz = m_utility.SafeGetString(reader, "r_plz")
					overviewData.rort = m_utility.SafeGetString(reader, "r_ort")
					overviewData.rland = m_utility.SafeGetString(reader, "r_land")

					overviewData.zahlkond = m_utility.SafeGetString(reader, "zahlkond")
					overviewData.printeddate = m_utility.SafeGetDateTime(reader, "printeddate", Nothing)

					overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
					overviewData.createdfrom = String.Format("{0}", m_utility.SafeGetString(reader, "createdfrom"))
					overviewData.changedon = m_utility.SafeGetDateTime(reader, "changedon", Nothing)
					overviewData.changedfrom = m_utility.SafeGetString(reader, "changedfrom")

					overviewData.eseinstufung = m_utility.SafeGetString(reader, "es_einstufung")
					overviewData.kdbranche = m_utility.SafeGetString(reader, "kdbranche")

					overviewData.mdnr = m_utility.SafeGetInteger(reader, "mdnr", Nothing)
					overviewData.rekst1 = m_utility.SafeGetString(reader, "rekst1")
					overviewData.rekst2 = m_utility.SafeGetString(reader, "rekst2")

					overviewData.reart2 = m_utility.SafeGetString(reader, "art_2")
					overviewData.kreditrefnr = m_utility.SafeGetString(reader, "kredit_refnr")
					overviewData.kreditlimite = m_utility.SafeGetDecimal(reader, "kreditlimite", Nothing)

					overviewData.kreditlimiteab = m_utility.SafeGetDateTime(reader, "kdkreditlimiteab", Nothing)
					overviewData.kreditlimitebis = m_utility.SafeGetDateTime(reader, "kdkreditlimitebis", Nothing)

					overviewData.kreditlimite2 = m_utility.SafeGetDecimal(reader, "kreditlimite_2", Nothing)
					overviewData.kdumsmin = m_utility.SafeGetDecimal(reader, "kd_umsmin", Nothing)

					overviewData.employeeadvisor = m_utility.SafeGetString(reader, "employeeadvisor")
					overviewData.customeradvisor = m_utility.SafeGetString(reader, "customeradvisor")

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

	Private Function LoadFoundedCustomerList() As Boolean

		Dim listOfEmployees = GetDbCustomerData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.RENr = person.RENr,
																							.KDNr = person.KDNr,
																							.reart = person.reart,
																							.kst = person.kst,
																							.fakdat = person.fakdat,
																							.currency = person.currency,
																							.betragohne = person.betragohne,
																							.betragex = person.betragex,
																							.betragink = person.betragink,
																							.mwst1 = person.mwst1,
																							.mwstproz = person.mwstproz,
																							.bezahlt = person.bezahlt,
																							.skonto = person.skonto,
																							.faellig = person.faellig,
																							.mahncode = person.mahncode,
																							.ma0 = person.ma0,
																							.ma1 = person.ma1,
																							.ma2 = person.ma2,
																							.ma3 = person.ma3,
																							.fksoll = person.fksoll,
																							.fkhaben0 = person.fkhaben0,
																							.fkhaben1 = person.fkhaben1,
																						 .rname1 = person.rname1,
																						 .rname2 = person.rname2,
																						 .rname3 = person.rname3,
																						 .rzhd = person.rzhd,
																						 .rabteilung = person.rabteilung,
																						 .rpostfach = person.rpostfach,
																						 .rplz = person.rplz,
																						 .rort = person.rort,
																						 .rland = person.rland,
																						 .zahlkond = person.zahlkond,
																						 .printeddate = person.printeddate,
																						 .createdon = person.createdon,
																						 .createdfrom = person.createdfrom,
																						 .changedon = person.changedon,
																						 .changedfrom = person.changedfrom,
																						 .eseinstufung = person.eseinstufung,
																						 .kdbranche = person.kdbranche,
																						 .mdnr = person.mdnr,
																						 .reart2 = person.reart2,
																						 .kreditrefnr = person.kreditrefnr,
																						 .kreditlimite = person.kreditlimite,
																						 .employeeadvisor = person.employeeadvisor,
																			.customeradvisor = person.customeradvisor
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridCustomerData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

		gvRP.Columns.Clear()


		Dim columnRENr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRENr.Caption = m_Translate.GetSafeTranslationValue("Debitoren-Nr.")
		columnRENr.Name = "RENr"
		columnRENr.FieldName = "RENr"
		columnRENr.Visible = True
		gvRP.Columns.Add(columnRENr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnrname1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrname1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrname1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnrname1.Name = "rname1"
		columnrname1.FieldName = "rname1"
		columnrname1.Visible = True
		gvRP.Columns.Add(columnrname1)

		Dim columnrname2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrname2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrname2.Caption = m_Translate.GetSafeTranslationValue("Firma2")
		columnrname2.Name = "rname2"
		columnrname2.FieldName = "rname2"
		columnrname2.Visible = False
		gvRP.Columns.Add(columnrname2)

		Dim columnrname3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrname3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrname3.Caption = m_Translate.GetSafeTranslationValue("Firma3")
		columnrname3.Name = "rname3"
		columnrname3.FieldName = "rname3"
		columnrname3.Visible = False
		gvRP.Columns.Add(columnrname3)

		Dim columnfakdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfakdat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfakdat.Caption = m_Translate.GetSafeTranslationValue("Fakturadatum")
		columnfakdat.Name = "fakdat"
		columnfakdat.FieldName = "fakdat"
		columnfakdat.BestFit()
		columnfakdat.Visible = True
		gvRP.Columns.Add(columnfakdat)


		Dim columnbetragohne As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbetragohne.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnbetragohne.Caption = m_Translate.GetSafeTranslationValue("Betrag MwSt.-frei")
		columnbetragohne.Name = "betragohne"
		columnbetragohne.FieldName = "betragohne"
		columnbetragohne.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbetragohne.DisplayFormat.FormatString = "n2"
		columnbetragohne.BestFit()
		columnbetragohne.Visible = False
		columnbetragohne.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbetragohne.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnbetragohne)

		Dim columnbetragex As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbetragex.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnbetragex.Caption = m_Translate.GetSafeTranslationValue("Betrag exk. MwSt.")
		columnbetragex.Name = "betragex"
		columnbetragex.FieldName = "betragex"
		columnbetragex.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbetragex.DisplayFormat.FormatString = "n2"
		columnbetragex.BestFit()
		columnbetragex.Visible = False
		columnbetragex.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbetragex.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnbetragex)

		Dim columnbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnbetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag inkl. MwSt.")
		columnbetrag.Name = "betragink"
		columnbetrag.FieldName = "betragink"
		columnbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbetrag.DisplayFormat.FormatString = "n2"
		columnbetrag.BestFit()
		columnbetrag.Visible = True
		columnbetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbetrag.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnbetrag)

		Dim columnmwstbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmwstbetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnmwstbetrag.Caption = m_Translate.GetSafeTranslationValue("MwSt.-Betrag")
		columnmwstbetrag.Name = "mwst1"
		columnmwstbetrag.FieldName = "mwst1"
		columnmwstbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmwstbetrag.DisplayFormat.FormatString = "n2"
		columnmwstbetrag.BestFit()
		columnmwstbetrag.Visible = False
		columnmwstbetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnmwstbetrag.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnmwstbetrag)

		Dim columnmwstproz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmwstproz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnmwstproz.Caption = m_Translate.GetSafeTranslationValue("MwSt.-Prozent")
		columnmwstproz.Name = "mwstproz"
		columnmwstproz.FieldName = "mwstproz"
		columnmwstproz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmwstproz.DisplayFormat.FormatString = "n2"
		columnmwstproz.BestFit()
		columnmwstproz.Visible = False
		gvRP.Columns.Add(columnmwstproz)

		Dim columnbezahlt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbezahlt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnbezahlt.Caption = m_Translate.GetSafeTranslationValue("Bezahlt")
		columnbezahlt.Name = "bezahlt"
		columnbezahlt.FieldName = "bezahlt"
		columnbezahlt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbezahlt.DisplayFormat.FormatString = "n2"
		columnbezahlt.BestFit()
		columnbezahlt.Visible = True
		columnbezahlt.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbezahlt.SummaryItem.DisplayFormat = "{0:n2}"
		gvRP.Columns.Add(columnbezahlt)

		Dim columnfaellig As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfaellig.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfaellig.Caption = m_Translate.GetSafeTranslationValue("Fällig am")
		columnfaellig.Name = "faellig"
		columnfaellig.FieldName = "faellig"
		columnfaellig.BestFit()
		columnfaellig.Visible = False
		gvRP.Columns.Add(columnfaellig)

		Dim columnprinteddate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnprinteddate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnprinteddate.Caption = m_Translate.GetSafeTranslationValue("Gedruckt am")
		columnprinteddate.Name = "printeddate"
		columnprinteddate.FieldName = "printeddate"
		columnprinteddate.BestFit()
		columnprinteddate.Visible = False
		gvRP.Columns.Add(columnprinteddate)


		Dim columnkonto As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkonto.Caption = m_Translate.GetSafeTranslationValue("Konto")
		columnkonto.Name = "fksoll"
		columnkonto.FieldName = "fksoll"
		columnkonto.Visible = True
		columnkonto.BestFit()
		gvRP.Columns.Add(columnkonto)

		Dim columnkst As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst.Caption = m_Translate.GetSafeTranslationValue("KST")
		columnkst.Name = "kst"
		columnkst.FieldName = "kst"
		columnkst.Visible = False
		columnkst.BestFit()
		gvRP.Columns.Add(columnkst)


		Dim columnkreditref As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditref.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditref.Caption = m_Translate.GetSafeTranslationValue("Kredit-Referenznummer")
		columnkreditref.Name = "kreditrefnr"
		columnkreditref.FieldName = "kreditrefnr"
		columnkreditref.Visible = False
		columnkreditref.BestFit()
		gvRP.Columns.Add(columnkreditref)

		Dim columnkreditlimite As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditlimite.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditlimite.Caption = m_Translate.GetSafeTranslationValue("Kreditlimit")
		columnkreditlimite.Name = "kreditlimite"
		columnkreditlimite.FieldName = "kreditlimite"
		columnkreditlimite.Visible = False
		columnkreditlimite.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnkreditlimite.DisplayFormat.FormatString = "f2"
		columnkreditlimite.BestFit()
		gvRP.Columns.Add(columnkreditlimite)

		Dim columnemployeeadvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeadvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeadvisor.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Berater")
		columnemployeeadvisor.Name = "employeeadvisor"
		columnemployeeadvisor.FieldName = "employeeadvisor"
		columnemployeeadvisor.Visible = False
		columnemployeeadvisor.BestFit()
		gvRP.Columns.Add(columnemployeeadvisor)

		Dim columncustomeradvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeradvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeradvisor.Caption = m_Translate.GetSafeTranslationValue("Kunden-Berater")
		columncustomeradvisor.Name = "customeradvisor"
		columncustomeradvisor.FieldName = "customeradvisor"
		columncustomeradvisor.Visible = False
		columncustomeradvisor.BestFit()
		gvRP.Columns.Add(columncustomeradvisor)

		Dim columnreart As New DevExpress.XtraGrid.Columns.GridColumn()
		columnreart.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnreart.Caption = m_Translate.GetSafeTranslationValue("Art")
		columnreart.Name = "reart"
		columnreart.FieldName = "reart"
		columnreart.Visible = False
		columnreart.BestFit()
		gvRP.Columns.Add(columnreart)

		Dim columnreart2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnreart2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnreart2.Caption = m_Translate.GetSafeTranslationValue("2. Art")
		columnreart2.Name = "reart2"
		columnreart2.FieldName = "reart2"
		columnreart2.Visible = False
		columnreart2.BestFit()
		gvRP.Columns.Add(columnreart2)

		Dim columnrefkhaben0 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrefkhaben0.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrefkhaben0.Caption = m_Translate.GetSafeTranslationValue("HAHBEN 0")
		columnrefkhaben0.Name = "fkhaben0"
		columnrefkhaben0.FieldName = "fkhaben0"
		columnrefkhaben0.Visible = False
		columnrefkhaben0.BestFit()
		gvRP.Columns.Add(columnrefkhaben0)

		Dim columnrefkhaben1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrefkhaben1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrefkhaben1.Caption = m_Translate.GetSafeTranslationValue("HABEN 1")
		columnrefkhaben1.Name = "fkhaben1"
		columnrefkhaben1.FieldName = "fkhaben1"
		columnrefkhaben1.Visible = False
		columnrefkhaben1.BestFit()
		gvRP.Columns.Add(columnrefkhaben1)

		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columncreatedon.Name = "createdon"
		columncreatedon.FieldName = "createdon"
		columncreatedon.Visible = False
		columncreatedon.BestFit()
		gvRP.Columns.Add(columncreatedon)

		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "createdfrom"
		columncreatedfrom.FieldName = "createdfrom"
		columncreatedfrom.Visible = False
		columncreatedfrom.BestFit()
		gvRP.Columns.Add(columncreatedfrom)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


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

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiTotalbetrag.Caption = m_Translate.GetSafeTranslationValue(Me.bbiTotalbetrag.Caption)
			bbiPrintList.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()
		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, Me.Height)

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
			LoadFoundedCustomerList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)
			If Me.RecCount > 0 Then CreateExportPopupMenu()

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

			AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OngvMain_ColumnFilterChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		Catch ex As Exception

		End Try

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case "rname1", "kdnr", "rname2", "rname3"
						If viewData.KDNr > 0 Then RunOpenKDForm(viewData.KDNr)


					Case Else
						If viewData.RENr > 0 Then
							RunOpenOPForm(viewData.RENr, CInt(viewData.mdnr))
						End If


				End Select

			End If

		End If

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub OnGvCurrentList_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "betragohne" Or e.Column.FieldName = "betragex" Or e.Column.FieldName = "betragink" Or e.Column.FieldName = "mwst1" Or
				e.Column.FieldName = "mwstproz" Or e.Column.FieldName = "bezahlt" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub bbiTotalbetrag_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiTotalbetrag.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiTotalbetrag.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As List(Of String) = GetMenuItems4Currencies()

		Try
			bbiTotalbetrag.Manager = Me.BarManager1
			BarManager1.ForceInitialize()
			Me.bbiTotalbetrag.ActAsDropDown = False
			Me.bbiTotalbetrag.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiTotalbetrag.DropDownEnabled = True
			Me.bbiTotalbetrag.DropDownControl = popupMenu
			Me.bbiTotalbetrag.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString).Replace("-", "")
					'itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
					'itm.AccessibleName = myValue(2).ToString
					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Function GetMenuItems4Currencies() As List(Of String)
		Dim liResult As New List(Of String)

		Try
			If Me._dBetragex <> 0 Then liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("Totalbetrag MwSt.-frei: {0}"), Format(Me._dBetragohne, "c2")))
			If Me._dBetragex <> 0 Then liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("Totalbetrag exkl. MwSt.: {0}"), Format(Me._dBetragex, "c2")))
			If Me._dBetragex <> 0 Then liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("Totalbetrag inkl. MwSt.: {0}"), Format(Me._dBetragink, "c2")))
			If Me._dBetragex <> 0 Then liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("-Totalbetrag der MwSt.: {0}"), Format(Me._dBetragmwst1, "c2")))
			If Me._dBetragex <> 0 Then liResult.Add(String.Format(m_Translate.GetSafeTranslationValue("-Total bezahlte Beträge: {0}"), Format(Me._dBetragbezahlt, "c2")))

		Catch ex As Exception
			m_utilityUI.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return liResult

	End Function

	Private Sub OngvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvRP.RowCount)

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("betragohne").Visible OrElse gvRP.Columns("betragex").Visible OrElse
																	gvRP.Columns("betragink").Visible OrElse gvRP.Columns("mwst1").Visible OrElse
																	gvRP.Columns("bezahlt").Visible)
		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreSearchSetting), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingSearchFilter), False)
		Catch ex As Exception

		End Try
		If File.Exists(m_GVSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub


	Sub RunOpenKDForm(ByVal iKDNr As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, iKDNr)
			hub.Publish(openMng)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub

	Sub RunOpenOPForm(ByVal iRENr As Integer, ByVal mandantNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenInvoiceMngRequest(Me, m_InitialData.UserData.UserNr, mandantNumber, iRENr)
			hub.Publish(openMng)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub


End Class