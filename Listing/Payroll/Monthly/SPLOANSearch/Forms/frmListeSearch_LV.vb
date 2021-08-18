
Option Strict Off

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.UI
Imports SPLOANSearch.ClsDataDetail
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Logging

Public Class frmListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

#Region "Private Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property Modul2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreLoAnSearchSetting As String
	Private m_xmlSettingLoAnSearchFilter As String

	Private m_xmlSettingRestoreLoRekapSearchSetting As String
	Private m_xmlSettingLoRekapSearchFilter As String

	Private m_xmlSettingRestoreLoKTGSearchSetting As String
	Private m_xmlSettingLoKTGSearchFilter As String

#End Region


#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "losearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_LOAN_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/loansearchlist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOAN_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/loansearchlist/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_LOREKAP_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/lorekapsearchlist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOREKAP_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/lorekapsearchlist/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_LOKTG_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/loktgsearchlist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOKTG_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/loktgsearchlist/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String, ByVal _modulname As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		Me.Sql2Open = strQuery
		Me.Modul2Open = _modulname


		Try
			If Modul2Open = "loan" Then
				m_GridSettingPath = String.Format("{0}LOANSearchList\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr))
			ElseIf Modul2Open = "lorekap" Then
				m_GridSettingPath = String.Format("{0}LORekapSearchList\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr))
			Else
				m_GridSettingPath = String.Format("{0}LOKTGSearchList\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr))
			End If

			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.m_InitialData.UserData.UserNr)

			m_xmlSettingRestoreLoAnSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOAN_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoAnSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOAN_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreLoRekapSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOREKAP_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoRekapSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOREKAP_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreLoKTGSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOKTG_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoKTGSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOKTG_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.m_InitialData.MDData.MDNr))

		Catch ex As Exception

		End Try

		If Modul2Open = "loan" Then
			ResetGridLOANData()

		ElseIf Modul2Open = "lorekap" Then
			ResetGridLORekapData()

		Else
			ResetGridLOKTGData()

		End If

		AddHandler Me.gvRP.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged


	End Sub

#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedLOANData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedLOANData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Function GetDbLOANData4Show() As IEnumerable(Of FoundedLOANData)
		Dim result As List(Of FoundedLOANData) = Nothing

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedLOANData)

				' SELECT LOL.MANR, LOL.Jahr, LOL.LP, LOL.LANR, LA.LALOText, LOL.M_Bas, LOL.M_Anz, LOL.M_Ans, LOL.M_Btr, LOL.GAV_Kanton, LOL.GAV_Beruf, LOL.GAV_Gruppe1, 
				' Mitarbeiter.Nachname, Mitarbeiter.Vorname, Mitarbeiter.AHV_Nr, Mitarbeiter.AHV_Nr_New, Mitarbeiter.Land As MALand, Mitarbeiter.PLZ, Mitarbeiter.Ort, Mitarbeiter.Strasse, 
				' Mitarbeiter.GebDat, Mitarbeiter.KST As MAKST, 2014 As VonJahr, 1 As VonMonat, 2014 As BisJahr, 1 As BisMonat, LOL.GAV_Kanton,  Mitarbeiter.PLZ, Mitarbeiter.Ort

				While reader.Read()
					Dim overviewData As New FoundedLOANData

					overviewData.MANr = m_utility.SafeGetInteger(reader, "MANr", Nothing)

					overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
					overviewData.monat = m_utility.SafeGetInteger(reader, "lp", Nothing)

					overviewData.lanr = m_utility.SafeGetDecimal(reader, "lanr", Nothing)
					overviewData.lalotext = m_utility.SafeGetString(reader, "lalotext")

					overviewData.m_anz = m_utility.SafeGetDecimal(reader, "m_anz", Nothing)
					overviewData.m_bas = m_utility.SafeGetDecimal(reader, "m_bas", Nothing)
					overviewData.m_ans = m_utility.SafeGetDecimal(reader, "m_ans", Nothing)
					overviewData.m_btr = m_utility.SafeGetDecimal(reader, "m_btr", Nothing)

					overviewData.gav_beruf = m_utility.SafeGetString(reader, "GAV_Beruf")
					overviewData.gav_kanton = m_utility.SafeGetString(reader, "GAV_Kanton")
					overviewData.gav_gruppe1 = m_utility.SafeGetString(reader, "GAV_Gruppe1")

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "Nachname"), m_utility.SafeGetString(reader, "Vorname"))
					overviewData.employeestreet = m_utility.SafeGetString(reader, "Strasse")
					overviewData.employeepostcode = m_utility.SafeGetString(reader, "PLZ")
					overviewData.employeecity = m_utility.SafeGetString(reader, "Ort")
					overviewData.employeepostcodecity = String.Format("{0} {1}", overviewData.employeepostcode, overviewData.employeecity)
					overviewData.employeecountry = m_utility.SafeGetString(reader, "MALand")

					overviewData.vonmonat = m_utility.SafeGetInteger(reader, "VonMonat", Nothing)
					overviewData.bismonat = m_utility.SafeGetInteger(reader, "BisMonat", Nothing)

					overviewData.vonjahr = m_utility.SafeGetInteger(reader, "VonJahr", Nothing)
					overviewData.bisjahr = m_utility.SafeGetInteger(reader, "BisJahr", Nothing)

					overviewData.gebdat = m_utility.SafeGetString(reader, "gebdat")
					overviewData.ahv_nr = m_utility.SafeGetString(reader, "ahv_nr")
					overviewData.ahv_nr_new = m_utility.SafeGetString(reader, "ahv_nr_new")


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

	Function GetDbLORekapData4Show() As IEnumerable(Of FoundedLORekapData)
		Dim result As List(Of FoundedLORekapData) = Nothing

		Dim sql As String

		sql = String.Format("Select * From _LOANRekap_{0} Order By LANr", ClsDataDetail.m_InitialData.UserData.UserNr)

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedLORekapData)

				'lanr, jahr, lp, betrag, kumulativ, hkonto, skonto, bezeichnung, usnr 

				While reader.Read()
					Dim overviewData As New FoundedLORekapData

					overviewData.lanr = m_utility.SafeGetDecimal(reader, "lanr", Nothing)
					overviewData.lalotext = m_utility.SafeGetString(reader, "Bezeichnung")

					overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
					overviewData.monat = m_utility.SafeGetInteger(reader, "lp", Nothing)

					overviewData.betrag = m_utility.SafeGetDecimal(reader, "betrag", Nothing)
					overviewData.kumulativ = m_utility.SafeGetDecimal(reader, "kumulativ", Nothing)

					overviewData.hkonto = m_utility.SafeGetDecimal(reader, "hkonto", Nothing)
					overviewData.skonto = m_utility.SafeGetDecimal(reader, "skonto", Nothing)

					overviewData.userNumber = m_utility.SafeGetInteger(reader, "usnr", Nothing)

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

	Function GetDbLOKTGData4Show() As IEnumerable(Of FoundedLOKTGData)
		Dim result As List(Of FoundedLOKTGData) = Nothing

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedLOKTGData)

				' LOL.LONr, LOL.MANR, LOL.Jahr, LOL.LP, LOL.LANR, LA.LALOText, LOL.M_Bas, LOL.M_Anz, LOL.M_Ans, LOL.M_Btr, LOL.GAV_Kanton, LOL.GAV_Beruf, LOL.GAV_Gruppe1, Mitarbeiter.Nachname, 
				' Mitarbeiter.Vorname, Mitarbeiter.AHV_Nr, Mitarbeiter.AHV_Nr_New, Mitarbeiter.Land As MALand, Mitarbeiter.PLZ, Mitarbeiter.Ort, Mitarbeiter.Strasse, Mitarbeiter.GebDat, 
				' Mitarbeiter.KST As MAKST, 2014 As VonJahr, 1 As VonMonat, 2014 As BisJahr, 1 As BisMonat, LOL.GAV_Kanton, Mitarbeiter.PLZ, Mitarbeiter.Ort, tblBVG.ESBegin, tblBVG.BVGBEgin 

				While reader.Read()
					Dim overviewData As New FoundedLOKTGData

					overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)
					overviewData.MANr = m_utility.SafeGetInteger(reader, "MANr", Nothing)

					overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
					overviewData.monat = m_utility.SafeGetInteger(reader, "lp", Nothing)

					overviewData.lanr = m_utility.SafeGetDecimal(reader, "lanr", Nothing)
					overviewData.lalotext = m_utility.SafeGetString(reader, "lalotext")

					overviewData.m_anz = m_utility.SafeGetDecimal(reader, "m_anz", Nothing)
					overviewData.m_bas = m_utility.SafeGetDecimal(reader, "m_bas", Nothing)
					overviewData.m_ans = m_utility.SafeGetDecimal(reader, "m_ans", Nothing)
					overviewData.m_btr = m_utility.SafeGetDecimal(reader, "m_btr", Nothing)

					overviewData.gav_beruf = m_utility.SafeGetString(reader, "GAV_Beruf")
					overviewData.gav_kanton = m_utility.SafeGetString(reader, "GAV_Kanton")
					overviewData.gav_gruppe1 = m_utility.SafeGetString(reader, "GAV_Gruppe1")

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "Nachname"), m_utility.SafeGetString(reader, "Vorname"))
					overviewData.employeestreet = m_utility.SafeGetString(reader, "Strasse")
					overviewData.employeepostcode = m_utility.SafeGetString(reader, "PLZ")
					overviewData.employeecity = m_utility.SafeGetString(reader, "Ort")
					overviewData.employeepostcodecity = String.Format("{0} {1}", overviewData.employeepostcode, overviewData.employeecity)
					overviewData.employeecountry = m_utility.SafeGetString(reader, "MALand")

					overviewData.vonmonat = m_utility.SafeGetInteger(reader, "VonMonat", Nothing)
					overviewData.bismonat = m_utility.SafeGetInteger(reader, "BisMonat", Nothing)

					overviewData.vonjahr = m_utility.SafeGetInteger(reader, "VonJahr", Nothing)
					overviewData.bisjahr = m_utility.SafeGetInteger(reader, "BisJahr", Nothing)

					overviewData.gebdat = m_utility.SafeGetString(reader, "gebdat")
					overviewData.ahv_nr = m_utility.SafeGetString(reader, "ahv_nr")
					overviewData.ahv_nr_new = m_utility.SafeGetString(reader, "ahv_nr_new")

					overviewData.esbegin = m_utility.SafeGetString(reader, "ESBegin")
					overviewData.bvgbegin = m_utility.SafeGetString(reader, "BVGBEgin")

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

	Private Function LoadFoundedLOANList() As Boolean

		Dim listOfEmployees = GetDbLOANData4Show()

		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Die Daten konnten nicht geladen werden.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedLOANData With
																						 {.MANr = person.MANr,
																							.lanr = person.lanr,
																							.lalotext = person.lalotext,
																							.jahr = person.jahr,
																							.monat = person.monat,
																							.lonr = person.lonr,
																							.m_anz = person.m_anz,
																							.m_bas = person.m_bas,
																							.m_ans = person.m_ans,
																							.m_btr = person.m_btr,
																							.employeename = person.employeename,
																							.vonmonat = person.vonmonat,
																							.bismonat = person.bismonat,
																							.vonjahr = person.vonjahr,
																							.bisjahr = person.bisjahr,
																							.gebdat = person.gebdat,
																							.ahv_nr = person.ahv_nr,
																							.ahv_nr_new = person.ahv_nr_new,
																							.gav_beruf = person.gav_beruf,
																							.gav_gruppe1 = person.gav_gruppe1,
																							.gav_kanton = person.gav_kanton,
																							.employeepostcodecity = person.employeepostcodecity,
																							.employeecity = person.employeecity,
																							.employeecountry = person.employeecountry
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedLOANData) = New BindingList(Of FoundedLOANData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Function LoadFoundedLORekapList() As Boolean

		Dim listOfEmployees = GetDbLORekapData4Show()

		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Die Daten konnten nicht geladen werden.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedLORekapData With
																						 {.lanr = person.lanr,
																							.lalotext = person.lalotext,
																							.jahr = person.jahr,
																							.monat = person.monat,
																							.betrag = person.betrag,
																							.kumulativ = person.kumulativ,
																							.hkonto = person.hkonto,
																							.skonto = person.skonto,
																							.userNumber = person.userNumber
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedLORekapData) = New BindingList(Of FoundedLORekapData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Function LoadFoundedLOKTGList() As Boolean

		Dim listOfEmployees = GetDbLOKTGData4Show()

		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Die Daten konnten nicht geladen werden.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedLOKTGData With
																						 {.MANr = person.MANr,
																							.lanr = person.lanr,
																							.lalotext = person.lalotext,
																							.jahr = person.jahr,
																							.monat = person.monat,
																							.lonr = person.lonr,
																							.m_anz = person.m_anz,
																							.m_bas = person.m_bas,
																							.m_ans = person.m_ans,
																							.m_btr = person.m_btr,
																							.employeename = person.employeename,
																							.vonmonat = person.vonmonat,
																							.bismonat = person.bismonat,
																							.vonjahr = person.vonjahr,
																							.bisjahr = person.bisjahr,
																							.gebdat = person.gebdat,
																							.ahv_nr = person.ahv_nr,
																							.ahv_nr_new = person.ahv_nr_new,
																							.gav_beruf = person.gav_beruf,
																							.gav_gruppe1 = person.gav_gruppe1,
																							.gav_kanton = person.gav_kanton,
																							.employeepostcodecity = person.employeepostcodecity,
																							.employeecity = person.employeecity,
																							.employeecountry = person.employeecountry
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedLOKTGData) = New BindingList(Of FoundedLOKTGData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function




	Private Sub ResetGridLOANData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeename.Name = "employeename"
		columnEmployeename.FieldName = "employeename"
		columnEmployeename.Visible = True
		columnEmployeename.BestFit()
		gvRP.Columns.Add(columnEmployeename)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.Visible = False
		gvRP.Columns.Add(columnjahr)

		Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnMonat.Name = "monat"
		columnMonat.FieldName = "monat"
		columnMonat.Visible = False
		gvRP.Columns.Add(columnMonat)


		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Geb.-Datum")
		columngebdat.Name = "gebdat"
		columngebdat.FieldName = "gebdat"
		columngebdat.Visible = False
		gvRP.Columns.Add(columngebdat)

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
		gvRP.Columns.Add(columnm_btr)

		Dim columnGAVBeruf As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVBeruf.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVBeruf.Caption = m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columnGAVBeruf.Name = "gav_beruf"
		columnGAVBeruf.FieldName = "gav_beruf"
		columnGAVBeruf.Visible = True
		columnGAVBeruf.BestFit()
		gvRP.Columns.Add(columnGAVBeruf)

		Dim columnGAV1Kat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAV1Kat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAV1Kat.Caption = m_Translate.GetSafeTranslationValue("GAV-Kategorie")
		columnGAV1Kat.Name = "gav_gruppe1"
		columnGAV1Kat.FieldName = "gav_gruppe1"
		columnGAV1Kat.Visible = True
		columnGAV1Kat.BestFit()
		gvRP.Columns.Add(columnGAV1Kat)

		Dim columnGAVKanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVKanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVKanton.Caption = m_Translate.GetSafeTranslationValue("GAV-Kanton")
		columnGAVKanton.Name = "gav_kanton"
		columnGAVKanton.FieldName = "gav_kanton"
		columnGAVKanton.Visible = True
		columnGAVKanton.BestFit()
		gvRP.Columns.Add(columnGAVKanton)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub

	Private Sub ResetGridLORekapData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnm_anz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_anz.Caption = m_Translate.GetSafeTranslationValue("LANr")
		columnm_anz.Name = "lanr"
		columnm_anz.FieldName = "lanr"
		columnm_anz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_anz.AppearanceHeader.Options.UseTextOptions = True
		columnm_anz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_anz.DisplayFormat.FormatString = "N3"
		columnm_anz.Visible = True
		columnm_anz.BestFit()
		gvRP.Columns.Add(columnm_anz)

		Dim columnlalotext As New DevExpress.XtraGrid.Columns.GridColumn()
		columnlalotext.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnlalotext.Name = "lalotext"
		columnlalotext.FieldName = "lalotext"
		columnlalotext.Visible = True
		gvRP.Columns.Add(columnlalotext)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.Visible = False
		gvRP.Columns.Add(columnjahr)

		Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnMonat.Name = "monat"
		columnMonat.FieldName = "monat"
		columnMonat.Visible = False
		gvRP.Columns.Add(columnMonat)


		Dim columnbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnbetrag.Name = "betag"
		columnbetrag.FieldName = "betrag"
		columnbetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbetrag.AppearanceHeader.Options.UseTextOptions = True
		columnbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbetrag.DisplayFormat.FormatString = "N2"
		columnbetrag.Visible = True
		columnbetrag.BestFit()
		gvRP.Columns.Add(columnbetrag)

		Dim columnkumulativ As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkumulativ.Caption = m_Translate.GetSafeTranslationValue("Kumulativ")
		columnkumulativ.Name = "kumulativ"
		columnkumulativ.FieldName = "kumulativ"
		columnkumulativ.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnkumulativ.AppearanceHeader.Options.UseTextOptions = True
		columnkumulativ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnkumulativ.DisplayFormat.FormatString = "N2"
		columnkumulativ.Visible = True
		columnkumulativ.BestFit()
		gvRP.Columns.Add(columnkumulativ)

		Dim columnhkonto As New DevExpress.XtraGrid.Columns.GridColumn()
		columnhkonto.Caption = m_Translate.GetSafeTranslationValue("HABEN-Konten")
		columnhkonto.Name = "hkonto"
		columnhkonto.FieldName = "hkonto"
		columnhkonto.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnhkonto.AppearanceHeader.Options.UseTextOptions = True
		columnhkonto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnhkonto.DisplayFormat.FormatString = "N0"
		columnhkonto.Visible = False
		columnhkonto.BestFit()
		gvRP.Columns.Add(columnhkonto)

		Dim columnskonto As New DevExpress.XtraGrid.Columns.GridColumn()
		columnskonto.Caption = m_Translate.GetSafeTranslationValue("SOLL-Konten")
		columnskonto.Name = "skonto"
		columnskonto.FieldName = "skonto"
		columnskonto.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnskonto.AppearanceHeader.Options.UseTextOptions = True
		columnskonto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnskonto.DisplayFormat.FormatString = "N0"
		columnskonto.Visible = False
		columnskonto.BestFit()
		gvRP.Columns.Add(columnskonto)


		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub

	Private Sub ResetGridLOKTGData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeename.Name = "employeename"
		columnEmployeename.FieldName = "employeename"
		columnEmployeename.Visible = True
		columnEmployeename.BestFit()
		gvRP.Columns.Add(columnEmployeename)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnjahr.Name = "jahr"
		columnjahr.FieldName = "jahr"
		columnjahr.Visible = False
		gvRP.Columns.Add(columnjahr)

		Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMonat.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnMonat.Name = "monat"
		columnMonat.FieldName = "monat"
		columnMonat.Visible = False
		gvRP.Columns.Add(columnMonat)


		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Geb.-Datum")
		columngebdat.Name = "gebdat"
		columngebdat.FieldName = "gebdat"
		columngebdat.Visible = False
		gvRP.Columns.Add(columngebdat)

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
		columnahv_nr_new.Visible = True
		columnahv_nr_new.BestFit()
		gvRP.Columns.Add(columnahv_nr_new)


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
		gvRP.Columns.Add(columnm_btr)

		Dim columnGAVBeruf As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVBeruf.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVBeruf.Caption = m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columnGAVBeruf.Name = "gav_beruf"
		columnGAVBeruf.FieldName = "gav_beruf"
		columnGAVBeruf.Visible = True
		columnGAVBeruf.BestFit()
		gvRP.Columns.Add(columnGAVBeruf)

		Dim columnGAV1Kat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAV1Kat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAV1Kat.Caption = m_Translate.GetSafeTranslationValue("GAV-Kategorie")
		columnGAV1Kat.Name = "gav_gruppe1"
		columnGAV1Kat.FieldName = "gav_gruppe1"
		columnGAV1Kat.Visible = False
		columnGAV1Kat.BestFit()
		gvRP.Columns.Add(columnGAV1Kat)

		Dim columnGAVKanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVKanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVKanton.Caption = m_Translate.GetSafeTranslationValue("GAV-Kanton")
		columnGAVKanton.Name = "gav_kanton"
		columnGAVKanton.FieldName = "gav_kanton"
		columnGAVKanton.Visible = False
		columnGAVKanton.BestFit()
		gvRP.Columns.Add(columnGAVKanton)


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

			If Modul2Open = "loan" Then
				LoadFoundedLOANList()
			ElseIf Modul2Open = "lorekap" Then
				LoadFoundedLORekapList()
			Else
				LoadFoundedLOKTGList()
			End If

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
			Dim viewData As FoundedLOANData
			Dim viewKTGData As FoundedLOKTGData
			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)

			If Not dataRow Is Nothing Then

				If Modul2Open = "loan" Then
					viewData = CType(dataRow, FoundedLOANData)
					If viewData.MANr.HasValue Then
						Dim hub = MessageService.Instance.Hub
						Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, viewData.MANr)
						hub.Publish(openMng)

					End If
					Return

				ElseIf Modul2Open = "lorekap" Then
					Return

				Else
					viewKTGData = CType(dataRow, FoundedLOKTGData)

					If viewKTGData.MANr.HasValue Then
						Dim hub = MessageService.Instance.Hub
						Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, viewKTGData.MANr)
						hub.Publish(openMng)

					End If
					Return

				End If

				'Select Case column.Name.ToLower

				'	Case Else
				'		If viewData.MANr.HasValue Then
				'			Dim hub = MessageService.Instance.Hub
				'			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, viewData.MANr)
				'			hub.Publish(openMng)

				'		End If
				'		'obj.OpenSelectedEmployee(viewData.MANr)

				'End Select

			End If

		End If

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case Modul2Open
			Case "loan"
				Try
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoAnSearchSetting, False), True)
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoAnSearchFilter, False), False)

				Catch ex As Exception

				End Try

			Case "lorekap"
				Try
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoRekapSearchSetting, False), True)
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoRekapSearchFilter, False), False)

				Catch ex As Exception

				End Try

			Case "loktg"
				Try
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoKTGSearchSetting, False), True)
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoKTGSearchFilter, False), False)

				Catch ex As Exception

				End Try

			Case Else
				Return

		End Select

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub


End Class



Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		'Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

		'	Try
		'		Dim hub = MessageService.Instance.Hub
		'		Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, Employeenumber)
		'		hub.Publish(openMng)

		'		'Dim frm As frmEmployees = CType(ClsDataDetail.GetModuleCach.GetModuleForm(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr, SP.ModuleCaching.ModuleName.EmployeeMng), frmEmployees)
		'		'frm.LoadEmployeeData(Employeenumber)

		'		'If frm.IsEmployeeDataLoaded Then
		'		'	frm.Show()
		'		'	frm.BringToFront()
		'		'End If

		'	Catch ex As Exception
		'		m_Logger.LogError(ex.ToString())
		'	End Try

		'End Sub


	End Class

End Namespace

