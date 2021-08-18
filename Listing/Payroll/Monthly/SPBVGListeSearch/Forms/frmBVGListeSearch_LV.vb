

Option Strict Off

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SP.Infrastructure.UI
Imports SPBVGListeSearch.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Logging

Public Class frmBVGListeSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities

	Public Property RecCount As Integer
	Private Property Sql2Open As String

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVEmployeeSearchSettingfilename As String

	Private m_xmlSettingRestoreEmployeeSearchSetting As String
	Private m_xmlSettingEmployeeSearchFilter As String
	Private m_SearchCriteria As New SearchCriteria

	Private m_IsBVGListForAXA As Boolean


#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "bvgsearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_QST_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/bvgsearchlist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_QST_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/bvgsearchlist/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String, ByVal m_searchcriteria As SearchCriteria)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		Me.m_SearchCriteria = m_searchcriteria
		Me.Sql2Open = strQuery

		Dim _ClsDb As New ClsDbFunc(m_searchcriteria)
		m_IsBVGListForAXA = _ClsDb.IsBVGListForAXA()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.m_InitialData.TranslationData, ClsDataDetail.m_InitialData.ProsonalizedData)

		Try
			m_GridSettingPath = String.Format("{0}BVGSearchList\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr))
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

	Function GetDbSalaryData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing

		Dim sql As String

		sql = Sql2Open


		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					If Not m_IsBVGListForAXA Then overviewData.rownr = CInt(m_utility.SafeGetInteger(reader, "row", Nothing))
					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", Nothing))

					If Not m_IsBVGListForAXA Then overviewData.lanr = CInt(m_utility.SafeGetDecimal(reader, "lanr", Nothing))
					If Not m_IsBVGListForAXA Then overviewData.lalotext = m_utility.SafeGetString(reader, "lalotext")
					If Not m_IsBVGListForAXA Then overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
					If Not m_IsBVGListForAXA Then overviewData.monat = m_utility.SafeGetInteger(reader, "monat", Nothing)
					If Not m_IsBVGListForAXA Then overviewData.lonr = m_utility.SafeGetInteger(reader, "lonr", Nothing)

					If Not m_IsBVGListForAXA Then overviewData.m_anz = m_utility.SafeGetDecimal(reader, "m_anz", Nothing)
					If Not m_IsBVGListForAXA Then overviewData.m_ans = m_utility.SafeGetDecimal(reader, "m_ans", Nothing)
					overviewData.m_bas = m_utility.SafeGetDecimal(reader, "m_bas", Nothing)
					overviewData.m_btr = m_utility.SafeGetDecimal(reader, "m_btr", Nothing)

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "Nachname"), m_utility.SafeGetString(reader, "Vorname"))

					If Not m_IsBVGListForAXA Then overviewData.vonmonat = m_utility.SafeGetInteger(reader, "monatvon", Nothing)
					If Not m_IsBVGListForAXA Then overviewData.bismonat = m_utility.SafeGetInteger(reader, "monatbis", Nothing)
					If Not m_IsBVGListForAXA Then overviewData.vonjahr = m_utility.SafeGetInteger(reader, "jahrvon", Nothing)
					If Not m_IsBVGListForAXA Then overviewData.bisjahr = m_utility.SafeGetInteger(reader, "jahrbis", Nothing)

					overviewData.gebdat = m_utility.SafeGetString(reader, "gebdat")
					overviewData.ahv_nr = m_utility.SafeGetString(reader, "ahv_nr")
					overviewData.ahv_nr_new = m_utility.SafeGetString(reader, "ahv_nr_new")
					overviewData.geschlecht = m_utility.SafeGetString(reader, "geschlecht")


					overviewData.employeestreet = m_utility.SafeGetString(reader, "mastrasse")
					overviewData.employeepostcode = m_utility.SafeGetString(reader, "maplz")
					overviewData.employeecity = m_utility.SafeGetString(reader, "maort")
					overviewData.employeepostcodecity = m_utility.SafeGetString(reader, "maplzort")
					overviewData.employeecountry = m_utility.SafeGetString(reader, "maland")

					overviewData.zivilstand = m_utility.SafeGetString(reader, "zivilstand")
					overviewData.employeelanguage = m_utility.SafeGetString(reader, "sprache")

					overviewData.bvgein = m_utility.SafeGetString(reader, "bvgein")
					overviewData.bvgaus = m_utility.SafeGetString(reader, "bvgaus")

					overviewData.ahvlohn = m_utility.SafeGetDecimal(reader, "ahvlohn", Nothing)
					overviewData.bvgstd = m_utility.SafeGetDecimal(reader, "bvgstd", Nothing)
					If m_IsBVGListForAXA Then overviewData.bvgdays = m_utility.SafeGetDecimal(reader, "BVGDays", Nothing)

					overviewData.kinder = m_utility.SafeGetInteger(reader, "kinder", Nothing)


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

	Private Function LoadFoundedSalaryList() As Boolean

		Dim listOfEmployees = GetDbSalaryData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.rownr = person.rownr,
																							.MANr = person.MANr,
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
																							.geschlecht = person.geschlecht,
																							.employeepostcodecity = person.employeepostcodecity,
																							.employeecity = person.employeecity,
																							.employeecountry = person.employeecountry,
																							.zivilstand = person.zivilstand,
																							.employeelanguage = person.employeelanguage,
																							.kinder = person.kinder,
																							.bvgstd = person.bvgstd,
																							.bvgdays = person.bvgdays,
																							.ahvlohn = person.ahvlohn,
																							.bvgein = person.bvgein,
																							.bvgaus = person.bvgaus
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridSalaryData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

		gvRP.Columns.Clear()


		Dim columnRowNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRowNr.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRowNr.Name = "rownr"
		columnRowNr.FieldName = "rownr"
		columnRowNr.Visible = False
		gvRP.Columns.Add(columnRowNr)


		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
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

		Dim columnVonJahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVonJahr.Caption = m_Translate.GetSafeTranslationValue("Von-Jahr")
		columnVonJahr.Name = "vonmonat"
		columnVonJahr.FieldName = "vonmonat"
		columnVonJahr.Visible = False
		gvRP.Columns.Add(columnVonJahr)

		Dim columnbisJahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbisJahr.Caption = m_Translate.GetSafeTranslationValue("Bis-Jahr")
		columnbisJahr.Name = "bisjahr"
		columnbisJahr.FieldName = "bisjahr"
		columnbisJahr.Visible = False
		gvRP.Columns.Add(columnbisJahr)

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
		columns_kanton.Caption = m_Translate.GetSafeTranslationValue("Zivilstand")
		columns_kanton.Name = "zivilstand"
		columns_kanton.FieldName = "zivilstand"
		columns_kanton.Visible = False
		columns_kanton.BestFit()
		gvRP.Columns.Add(columns_kanton)

		Dim columngeschlecht As New DevExpress.XtraGrid.Columns.GridColumn()
		columngeschlecht.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columngeschlecht.Name = "geschlecht"
		columngeschlecht.FieldName = "geschlecht"
		columngeschlecht.BestFit()
		columngeschlecht.Visible = True
		gvRP.Columns.Add(columngeschlecht)

		Dim columnqstgemeinde As New DevExpress.XtraGrid.Columns.GridColumn()
		columnqstgemeinde.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnqstgemeinde.Caption = m_Translate.GetSafeTranslationValue("Eintritt")
		columnqstgemeinde.Name = "bvgein"
		columnqstgemeinde.FieldName = "bvgein"
		columnqstgemeinde.Visible = False
		columnqstgemeinde.BestFit()
		gvRP.Columns.Add(columnqstgemeinde)

		Dim columnbvgaus As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbvgaus.Caption = m_Translate.GetSafeTranslationValue("Austritt")
		columnbvgaus.Name = "bvgaus"
		columnbvgaus.FieldName = "bvgaus"
		columnbvgaus.BestFit()
		columnbvgaus.Visible = True
		gvRP.Columns.Add(columnbvgaus)


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


		Dim columnbvgstd As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbvgstd.Caption = m_Translate.GetSafeTranslationValue("BVG-Stunden")
		columnbvgstd.Name = "bvgstd"
		columnbvgstd.FieldName = "bvgstd"
		columnbvgstd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnbvgstd.AppearanceHeader.Options.UseTextOptions = True
		columnbvgstd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbvgstd.DisplayFormat.FormatString = "N2"
		columnbvgstd.Visible = False
		columnbvgstd.SummaryItem.DisplayFormat = "{0:n2}"
		columnbvgstd.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbvgstd.SummaryItem.Tag = "Sumbvgstd"
		gvRP.Columns.Add(columnbvgstd)

		If m_IsBVGListForAXA Then
			Dim columnbvgdays As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbvgdays.Caption = m_Translate.GetSafeTranslationValue("BVG-Tage")
			columnbvgdays.Name = "bvgdays"
			columnbvgdays.FieldName = "bvgdays"
			columnbvgdays.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnbvgdays.AppearanceHeader.Options.UseTextOptions = True
			columnbvgdays.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnbvgdays.DisplayFormat.FormatString = "N2"
			columnbvgdays.Visible = False
			columnbvgdays.SummaryItem.DisplayFormat = "{0:n2}"
			columnbvgdays.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnbvgdays.SummaryItem.Tag = "Sumbvgdays"
			gvRP.Columns.Add(columnbvgdays)
		End If

		Dim columnahvlohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvlohn.Caption = m_Translate.GetSafeTranslationValue("AHV-Lohn")
		columnahvlohn.Name = "ahvlohn"
		columnahvlohn.FieldName = "ahvlohn"
		columnahvlohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnahvlohn.AppearanceHeader.Options.UseTextOptions = True
		columnahvlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnahvlohn.DisplayFormat.FormatString = "N2"
		columnahvlohn.Visible = False
		columnahvlohn.SummaryItem.DisplayFormat = "{0:n2}"
		columnahvlohn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnahvlohn.SummaryItem.Tag = "Sumahvlohn"
		gvRP.Columns.Add(columnahvlohn)


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
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)

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
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower

					Case Else
						If viewData.MANr.HasValue Then obj.OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "kinder" Or e.Column.FieldName = "m_anz" Or e.Column.FieldName = "m_ans" Or e.Column.FieldName = "m_bas" Or e.Column.FieldName = "m_btr" Or e.Column.FieldName = "bvgstd" Or e.Column.FieldName = "bvgdays" Or e.Column.FieldName = "ahvlohn" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("m_anz").Visible OrElse gvRP.Columns("m_bas").Visible OrElse gvRP.Columns("m_btr").Visible OrElse gvRP.Columns("bvgstd").Visible OrElse gvRP.Columns("bvgdays").Visible OrElse gvRP.Columns("ahvlohn").Visible)
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


End Class



Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, Employeenumber)
				hub.Publish(openMng)

				'Dim frm As frmEmployees = CType(ClsDataDetail.GetModuleCach.GetModuleForm(ClsDataDetail.m_InitialData.MDData.MDNr, SP.ModuleCaching.ModuleName.EmployeeMng), frmEmployees)
				'frm.LoadEmployeeData(Employeenumber)

				'If frm.IsEmployeeDataLoaded Then
				'	frm.Show()
				'	frm.BringToFront()
				'End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub
	End Class

End Namespace

