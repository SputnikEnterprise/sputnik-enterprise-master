
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Repository

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten



Public Class frmMDFak


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI = "wsSPS_services/SPEmployeeTaxInfoService.asmx"

#End Region


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As ChildEducationData

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_ListOfUserControls As New List(Of DevExpress.XtraEditors.XtraUserControl)

	Private m_ISAlreadySaved As Boolean

	Private connectionString As String

	''' <summary>
	''' Record number of selected row.
	''' </summary>
	Private m_CurrentRecordNumber As Integer?

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_PayrollSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml
	Private m_TaxInfoServiceUrl As String

	Private errorProviderMangement As DXErrorProvider.DXErrorProvider

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		ClsDataDetail.m_InitialData = m_InitializationData

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		errorProviderMangement = New DXErrorProvider.DXErrorProvider
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		InitializeComponent()


		' Translate controls.
		TranslateControls()

		bbiPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

		m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)
		If Not System.IO.File.Exists(m_MandantXMLFile) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
		Else
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		'm_TaxInfoServiceUrl = DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI
		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		m_TaxInfoServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEE_TAXINFO_WEBSERVCIE_URI)

		Reset()

		AddHandler Me.gvKiAu.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged


	End Sub


#End Region


	Private Sub Reset()

		PreparForNewRecord()
		txt_MitgliedNr.EditValue = String.Empty
		txt_KassenNr.EditValue = String.Empty

		ResetTaxCantonDropDown()
		ResetGridSalaryData()
		' do not set it to empty, because its one and not per each record!
		errorProviderMangement.ClearErrors()

	End Sub

	Private Sub PreparForNewRecord()

		m_CurrentRecordNumber = Nothing

		txt_Jahreslohn.EditValue = 0D
		txt_Ki_Std.EditValue = 0D
		txt_Ki_Day.EditValue = 0D
		txt_Ki1_Month.EditValue = 0D
		txt_Ki2_Month.EditValue = 0D
		cbo_Ki_ChangeIn.EditValue = "T"

		txt_Au_Std.EditValue = 0D
		txt_Au_Day.EditValue = 0D
		txt_Au_Month.EditValue = 0D
		cbo_Au_ChangeIn.EditValue = "T"

		txt_Name.EditValue = String.Empty
		txt_Zusatz1.EditValue = String.Empty
		txt_Postfach.EditValue = String.Empty
		txt_Strasse.EditValue = String.Empty
		txt_plzort.EditValue = String.Empty

		chkCalculateAHVForYear.Checked = True

	End Sub

	''' <summary>
	''' Resets the tax canton drop down.
	''' </summary>
	Private Sub ResetTaxCantonDropDown()

		lueCanton.Properties.DisplayMember = "Description"
		lueCanton.Properties.ValueMember = "GetField"

		Dim columns = lueCanton.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
		columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Kanton")))

		lueCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCanton.Properties.SearchMode = SearchMode.AutoComplete
		lueCanton.Properties.AutoSearchColumnIndex = 0
		lueCanton.Properties.NullText = String.Empty
		lueCanton.EditValue = Nothing
	End Sub

	Private Sub ResetGridSalaryData()

		gvKiAu.OptionsView.ShowIndicator = False
		gvKiAu.OptionsView.ShowAutoFilterRow = True
		gvKiAu.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvKiAu.OptionsView.ShowFooter = True

		gvKiAu.Columns.Clear()


		Dim columnRowNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRowNr.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnRowNr.Name = "recid"
		columnRowNr.FieldName = "recid"
		columnRowNr.Visible = False
		gvKiAu.Columns.Add(columnRowNr)


		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kanton")
		columnMANr.Name = "fak_kanton"
		columnMANr.FieldName = "fak_kanton"
		columnMANr.Visible = True
		gvKiAu.Columns.Add(columnMANr)

		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Min. Jahreslohn")
		columngebdat.Name = "yminlohn"
		columngebdat.FieldName = "yminlohn"
		columngebdat.Visible = True
		gvKiAu.Columns.Add(columngebdat)

		Dim columnbisJahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbisJahr.Caption = m_Translate.GetSafeTranslationValue("Ki.-Zulage-Std")
		columnbisJahr.Name = "ki1_std"
		columnbisJahr.FieldName = "ki1_std"
		columnbisJahr.Visible = True
		gvKiAu.Columns.Add(columnbisJahr)

		Dim columnjahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjahr.Caption = m_Translate.GetSafeTranslationValue("Ki.-Zulage-Tag")
		columnjahr.Name = "ki2_day"
		columnjahr.FieldName = "ki2_day"
		columnjahr.Visible = True
		gvKiAu.Columns.Add(columnjahr)

		Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMonat.Caption = m_Translate.GetSafeTranslationValue("Ki.-Zulage-Monat")
		columnMonat.Name = "ki1_month"
		columnMonat.FieldName = "ki1_month"
		columnMonat.Visible = True
		gvKiAu.Columns.Add(columnMonat)

		Dim columns_kanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columns_kanton.Caption = m_Translate.GetSafeTranslationValue("Ki. änderen in")
		columns_kanton.Name = "changekiin"
		columns_kanton.FieldName = "changekiin"
		columns_kanton.Visible = True
		columns_kanton.BestFit()
		gvKiAu.Columns.Add(columns_kanton)


		Dim columnbismonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbismonat.Caption = m_Translate.GetSafeTranslationValue("Au.-Zulage-Std")
		columnbismonat.Name = "au1_std"
		columnbismonat.FieldName = "au1_std"
		columnbismonat.Visible = True
		gvKiAu.Columns.Add(columnbismonat)

		Dim columnVonMonat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVonMonat.Caption = m_Translate.GetSafeTranslationValue("Au.-Zulage-Tag")
		columnVonMonat.Name = "au1_day"
		columnVonMonat.FieldName = "au1_day"
		columnVonMonat.Visible = True
		gvKiAu.Columns.Add(columnVonMonat)

		Dim columnVonJahr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVonJahr.Caption = m_Translate.GetSafeTranslationValue("Au.-Zulage-Monat")
		columnVonJahr.Name = "au1_month"
		columnVonJahr.FieldName = "au1_month"
		columnVonJahr.Visible = True
		gvKiAu.Columns.Add(columnVonJahr)

		Dim columnahv_nr_new As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahv_nr_new.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnahv_nr_new.Caption = m_Translate.GetSafeTranslationValue("Au. änderen in")
		columnahv_nr_new.Name = "changeauin"
		columnahv_nr_new.FieldName = "changeauin"
		columnahv_nr_new.Visible = True
		columnahv_nr_new.BestFit()
		gvKiAu.Columns.Add(columnahv_nr_new)


		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Erfasst am")
		columnEmployeename.Name = "createdon"
		columnEmployeename.FieldName = "createdon"
		columnEmployeename.Visible = True
		columnEmployeename.BestFit()
		gvKiAu.Columns.Add(columnEmployeename)

		Dim columnahv_nr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahv_nr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnahv_nr.Caption = m_Translate.GetSafeTranslationValue("Erfasst durch")
		columnahv_nr.Name = "createdfrom"
		columnahv_nr.FieldName = "createdfrom"
		columnahv_nr.Visible = True
		columnahv_nr.BestFit()
		gvKiAu.Columns.Add(columnahv_nr)


		'Dim columnm_bas As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnm_bas.Caption = m_Translate.GetSafeTranslationValue("Basis")
		'columnm_bas.Name = "m_bas"
		'columnm_bas.FieldName = "m_bas"
		'columnm_bas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		'columnm_bas.AppearanceHeader.Options.UseTextOptions = True
		'columnm_bas.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		'columnm_bas.DisplayFormat.FormatString = "N2"
		'columnm_bas.Visible = False
		'columnm_bas.BestFit()
		'columnm_bas.SummaryItem.DisplayFormat = "{0:n2}"
		'columnm_bas.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		'columnm_bas.SummaryItem.Tag = "Summ_bas"
		'gvRP.Columns.Add(columnm_bas)



		grdKiAu.DataSource = Nothing

	End Sub


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As ChildEducationData
		Get
			Dim gvRP = TryCast(grdKiAu.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), ChildEducationData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#Region "Private Methods"



	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
		Me.lblLoadWebServiceData.Text = m_Translate.GetSafeTranslationValue(Me.lblLoadWebServiceData.Text)

		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)

	End Sub



	Public Function LoadChildeducationData() As Boolean

		LoadTaxCantonDropDownData()
		LoadChildeducationList()


	End Function

	''' <summary>
	''' Loads the tax canton drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadTaxCantonDropDownData() As Boolean
		Dim cantonData = m_CommonDatabaseAccess.LoadCantonData()

		If (cantonData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantondaten konnten nicht geladen werden."))
		End If

		lueCanton.Properties.DataSource = cantonData
		lueCanton.Properties.ForceInitialize()

		If Not (cantonData Is Nothing) Then lueCanton.Properties.DropDownRows = Math.Min(27, cantonData.Count)


		Return Not cantonData Is Nothing
	End Function

	Private Function LoadChildeducationList() As Boolean

		Dim listOfEmployees = m_TablesettingDatabaseAccess.LoadChildEducationData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New ChildEducationData With
					 {.au1_day = person.au1_day,
						.au1_month = person.au1_month,
						.au1_std = person.au1_std,
						.au2_day = person.au2_day,
						.au2_std = person.au2_std,
						.bemerkung_1 = person.bemerkung_1,
						.bemerkung_2 = person.bemerkung_2,
						.bemerkung_3 = person.bemerkung_3,
						.bemerkung_4 = person.bemerkung_4,
						.changeauin = person.changeauin,
						.changeauin_2 = person.changeauin_2,
						.changedfrom = person.changedfrom,
						.changedon = person.changedon,
						.changekiin = person.changekiin,
						.changekiin_2 = person.changekiin_2,
						.createdfrom = person.createdfrom,
						.createdon = person.createdon,
						.fak_kanton = person.fak_kanton,
						.fak_knr = person.fak_knr,
						.fak_mnr = person.fak_knr,
						.fak_name = person.fak_name,
						.fak_plzort = person.fak_plzort,
						.fak_postfach = person.fak_postfach,
						.fak_strasse = person.fak_strasse,
						.fak_zhd = person.fak_zhd,
						.ki2_fakmax = person.ki2_fakmax,
						.ki1_day = person.ki1_day,
						.ki1_fakmax = person.ki1_fakmax,
						.ki1_month = person.ki1_month,
						.ki1_std = person.ki1_std,
						.ki2_day = person.ki2_day,
						.ki2_month = person.ki2_month,
						.ki2_std = person.ki2_std,
						.yminlohn = person.yminlohn,
						.mdnr = person.mdnr,
						.mdyear = person.mdyear,
						.recid = person.recid,
						.recnr = person.recnr
					 }).ToList()

		Dim listDataSource As BindingList(Of ChildEducationData) = New BindingList(Of ChildEducationData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdKiAu.DataSource = listDataSource
		Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", gvKiAu.RowCount)

		Return Not listOfEmployees Is Nothing

	End Function

	Private Sub lblLoadWebServiceData_Click(sender As Object, e As EventArgs) Handles lblLoadWebServiceData.Click
		If lueCanton.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueCanton.EditValue) Then
			Dim msg As String = "Sie haben kein Kanton ausgewählt."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			Return
		End If
		PreparForNewRecord()
		Dim data = PerformWebserviceCall()

		If Not data Is Nothing Then
			txt_Jahreslohn.EditValue = data.YMinLohn
			txt_Ki_Std.EditValue = data.Ki1_Std
			txt_Ki_Day.EditValue = data.Ki1_Day
			txt_Ki1_Month.EditValue = data.Ki1_Month
			txt_Ki2_Month.EditValue = data.Ki2_Month
			cbo_Ki_ChangeIn.EditValue = If(String.IsNullOrWhiteSpace(data.ChangeKiIn), "T", data.ChangeKiIn)

			txt_Au_Std.EditValue = data.Au1_Std
			txt_Au_Day.EditValue = data.Au1_Day
			txt_Au_Month.EditValue = data.Au1_Month
			cbo_Au_ChangeIn.EditValue = If(String.IsNullOrWhiteSpace(data.ChangeAuIn), "T", data.ChangeAuIn)

			txt_Name.EditValue = data.Fak_Name
			txt_Zusatz1.EditValue = data.Fak_ZHD
			txt_Postfach.EditValue = data.Fak_Postfach
			txt_Strasse.EditValue = data.Fak_Strasse
			txt_plzort.EditValue = data.Fak_PLZOrt

			chkCalculateAHVForYear.Checked = True
		End If


	End Sub

	Private Function PerformWebserviceCall() As ChildEducationViewData


		Dim listDataSource = New ChildEducationData

		Dim ws = New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
		ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxInfoServiceUrl)

		' Read data over webservice
		Dim searchResult = ws.LoadChildEducationData(m_InitializationData.MDData.MDGuid, lueCanton.EditValue, Now.Year)


		Dim viewData = New ChildEducationViewData With {
			.ID = searchResult.ID,
			.MDYear = searchResult.MDYear,
			.FAK_Kanton = searchResult.FAK_Kanton,
			.Fak_Name = searchResult.Fak_Name,
			.Fak_ZHD = searchResult.Fak_zhd,
			.Fak_Postfach = searchResult.Fak_Postfach,
			.Fak_Strasse = searchResult.Fak_Strasse,
			.Fak_PLZOrt = searchResult.Fak_PLZOrt,
			.YMinLohn = searchResult.YMinLohn,
			.Ki1_Month = searchResult.Ki1_Month,
			.Ki2_Month = searchResult.Ki2_Month,
			.Au1_Month = searchResult.Au1_Month,
			.Ki1_Std = searchResult.Ki1_Std,
			.Ki2_Std = searchResult.Ki2_Std,
			.Ki1_Day = searchResult.Ki1_Day,
			.Ki2_Day = searchResult.Ki2_Day,
			.ChangeKiIn = searchResult.ChangeKiIn,
			.ChangeKiIn_2 = searchResult.ChangeKiIn_2,
			.Au1_Std = searchResult.Au1_Std,
			.Au2_Std = searchResult.Au2_Std,
			.Au1_Day = searchResult.Au1_Day,
			.Au2_Day = searchResult.Au2_Day,
			.ChangeAuIn = searchResult.ChangeAuIn,
			.ChangeAuIn_2 = searchResult.ChangeAuIn_2,
			.CreatedOn = searchResult.CreatedOn,
			.CreatedFrom = searchResult.CreatedFrom,
			.ChangedOn = searchResult.ChangedOn,
			.ChangedFrom = searchResult.ChangedFrom,
			.Ki1_FakMax = searchResult.Ki1_FakMax,
			.Ki2_FakMax = searchResult.Ki2_FakMax,
			.Fak_Proz = searchResult.Fak_Proz,
			.Geb_Zulage = searchResult.Geb_Zulage,
			.Ado_Zulage = searchResult.Ado_Zulage,
			.Bemerkung_1 = searchResult.Bemerkung_1,
			.Bemerkung_2 = searchResult.Bemerkung_2,
			.Bemerkung_3 = searchResult.Bemerkung_3,
			.Bemerkung_4 = searchResult.Bemerkung_4
		}


		Return viewData

	End Function

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidthChildeducation, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeightChildeducation, Me.Height)

			If My.Settings.frmLocationChildeducation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocationChildeducation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvKiAu.RowCount)

	End Sub

	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvKiAu.CustomColumnDisplayText

		If e.Column.FieldName = "kinder" Or e.Column.FieldName = "m_anz" Or e.Column.FieldName = "m_ans" Or e.Column.FieldName = "m_bas" Or e.Column.FieldName = "m_btr" Or e.Column.FieldName = "bvgstd" Or e.Column.FieldName = "ahvlohn" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	''' <summary>
	''' Handles focus change of row.
	''' </summary>
	Private Sub OngvRP_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvKiAu.FocusedRowChanged

		Dim selectedData = SelectedRecord

		If Not selectedData Is Nothing Then
			Dim success = LoadDetailData(selectedData.recid)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
			End If

		End If

	End Sub

	''' <summary>
	''' Loads document detail data.
	''' </summary>
	''' <param name="recid">The id of record.</param>
	Private Function LoadDetailData(ByVal recid As Integer) As Boolean

		Dim recordSearchResult = m_TablesettingDatabaseAccess.LoadAssignedChildEducationData(recid)

		If Not recordSearchResult Is Nothing Then

			lueCanton.EditValue = recordSearchResult.fak_kanton
			txt_Jahreslohn.EditValue = recordSearchResult.yminlohn
			txt_Ki_Std.EditValue = recordSearchResult.ki1_std
			txt_Ki_Day.EditValue = recordSearchResult.ki1_day
			txt_Ki1_Month.EditValue = recordSearchResult.ki1_month
			txt_Ki2_Month.EditValue = recordSearchResult.ki2_month
			cbo_Ki_ChangeIn.EditValue = recordSearchResult.changekiin

			txt_Au_Std.EditValue = recordSearchResult.au1_std
			txt_Au_Day.EditValue = recordSearchResult.au1_day
			txt_Au_Month.EditValue = recordSearchResult.au1_month
			cbo_Au_ChangeIn.EditValue = recordSearchResult.changeauin

			txt_Name.EditValue = recordSearchResult.fak_name
			txt_Zusatz1.EditValue = recordSearchResult.fak_zhd
			txt_Postfach.EditValue = recordSearchResult.fak_postfach
			txt_Strasse.EditValue = recordSearchResult.fak_strasse
			txt_plzort.EditValue = recordSearchResult.fak_plzort

			txt_MitgliedNr.EditValue = recordSearchResult.fak_mnr
			txt_KassenNr.EditValue = recordSearchResult.fak_knr
			chkCalculateAHVForYear.Checked = recordSearchResult.SeeAHVLohnForYear.GetValueOrDefault(False)

			Dim kizulagenotiflanrcontains As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/kizulagenotiflanrcontains", m_PayrollSetting))
			txt_LASpezial.EditValue = kizulagenotiflanrcontains


			m_CurrentRecordNumber = recordSearchResult.recid

			Return True
		Else
			Return False
		End If

	End Function

	Private Function SaveChildeductionData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso ValidateInputData()
		If Not success Then Return success
		Try

			Dim recordData As ChildEducationData = Nothing
			recordData = New ChildEducationData

			recordData.recid = m_CurrentRecordNumber.GetValueOrDefault(0)
			recordData.fak_kanton = lueCanton.EditValue
			recordData.yminlohn = txt_Jahreslohn.EditValue
			recordData.ki1_std = txt_Ki_Std.EditValue
			recordData.ki1_day = txt_Ki_Day.EditValue
			recordData.ki1_month = txt_Ki1_Month.EditValue
			recordData.ki2_month = txt_Ki2_Month.EditValue
			recordData.changekiin = cbo_Ki_ChangeIn.EditValue

			recordData.au1_std = txt_Au_Std.EditValue
			recordData.au1_day = txt_Au_Day.EditValue
			recordData.au1_month = txt_Au_Month.EditValue
			recordData.changeauin = cbo_Au_ChangeIn.EditValue


			recordData.fak_name = txt_Name.EditValue
			recordData.fak_zhd = txt_Zusatz1.EditValue
			recordData.fak_postfach = txt_Postfach.EditValue
			recordData.fak_strasse = txt_Strasse.EditValue
			recordData.fak_plzort = txt_plzort.EditValue

			recordData.fak_mnr = txt_MitgliedNr.EditValue
			recordData.fak_knr = txt_KassenNr.EditValue

			recordData.createdfrom = m_InitializationData.UserData.UserFullName
			recordData.changedfrom = m_InitializationData.UserData.UserFullName

			recordData.mdnr = m_InitializationData.MDData.MDNr
			recordData.mdyear = m_InitializationData.MDData.MDYear
			recordData.au2_day = 0D
			recordData.au2_std = 0D
			recordData.changeauin_2 = recordData.changeauin
			recordData.changekiin_2 = recordData.changekiin
			recordData.ki2_fakmax = 0D
			recordData.ki2_fakmax = 0D
			recordData.ki2_day = recordData.ki1_day
			recordData.AtEndBeginES = False
			recordData.SeeAHVLohnForYear = chkCalculateAHVForYear.Checked

			If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
				success = m_TablesettingDatabaseAccess.AddChildEducationData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear, recordData)

			Else
				success = m_TablesettingDatabaseAccess.UpdateAssignedChildEducationData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear, recordData)

			End If
			If success Then
				m_MandantSettingsXml.AddOrUpdateSetting(String.Format("{0}/kizulagenotiflanrcontains", m_PayrollSetting), txt_LASpezial.EditValue)
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			success = False
		End Try


		Return success

	End Function

	Private Function DeleteChildeductionData() As Boolean
		Dim success As Boolean = False

		Dim selectedData = SelectedRecord

		If Not selectedData Is Nothing Then

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																m_Translate.GetSafeTranslationValue("Daten endgültig löschen?"))) Then

				success = m_TablesettingDatabaseAccess.DeleteChildEducationData(selectedData.recid)

			End If

		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnte nicht gelöscht werden."))
		End If


		Return success

	End Function

	''' <summary>
	''' Validates document input data.
	''' </summary>
	Private Function ValidateInputData() As Boolean

		errorProviderMangement.ClearErrors()

		Dim errorText As String = "Bitte geben Sie einen Wert ein."
		Dim errorTextMissingFile As String = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Datei aus.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(lueCanton, errorProviderMangement, String.IsNullOrEmpty(lueCanton.EditValue), errorText)
		isValid = isValid And SetErrorIfInvalid(txt_Jahreslohn, errorProviderMangement, txt_Jahreslohn.EditValue <= 0, errorText)

		isValid = isValid And SetErrorIfInvalid(txt_Ki1_Month, errorProviderMangement, txt_Ki1_Month.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(cbo_Ki_ChangeIn, errorProviderMangement, String.IsNullOrEmpty(cbo_Ki_ChangeIn.EditValue), errorText)

		isValid = isValid And SetErrorIfInvalid(txt_Au_Month, errorProviderMangement, txt_Au_Month.EditValue <= 0, errorText)
		isValid = isValid And SetErrorIfInvalid(cbo_Au_ChangeIn, errorProviderMangement, String.IsNullOrEmpty(cbo_Au_ChangeIn.EditValue), errorText)


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
	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider.DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

#End Region


#Region "Form Properties..."

	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocationChildeducation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeightChildeducation = Me.Height
				My.Settings.ifrmWidthChildeducation = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnbbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		Dim success = SaveChildeductionData()

		success = success AndAlso LoadChildeducationList()

		If success Then OngvRP_FocusedRowChanged(gvKiAu, Nothing)

	End Sub

	Private Sub OnbbiNew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick

		PreparForNewRecord()
		lueCanton.Focus()

	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		Dim success = DeleteChildeductionData()
		PreparForNewRecord()
		success = success AndAlso LoadChildeducationList()
		If success Then OngvRP_FocusedRowChanged(gvKiAu, Nothing)

		lueCanton.Focus()

	End Sub

	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub


#End Region


	Private Class ChildEducationViewData

		Public Property ID As Integer
		Public Property MDYear As Integer
		Public Property FAK_Kanton As String
		Public Property Fak_Name As String
		Public Property Fak_ZHD As String
		Public Property Fak_Postfach As String
		Public Property Fak_Strasse As String
		Public Property Fak_PLZOrt As String
		Public Property YMinLohn As Decimal?
		Public Property Ki1_Month As Decimal?
		Public Property Ki2_Month As Decimal?
		Public Property Au1_Month As Decimal?
		Public Property Ki1_Std As Decimal?
		Public Property Ki2_Std As Decimal?
		Public Property Ki1_Day As Decimal?
		Public Property Ki2_Day As Decimal?
		Public Property ChangeKiIn As String
		Public Property ChangeKiIn_2 As String
		Public Property Au1_Std As Decimal?
		Public Property Au2_Std As Decimal?
		Public Property Au1_Day As Decimal?
		Public Property Au2_Day As Decimal?
		Public Property ChangeAuIn As String
		Public Property ChangeAuIn_2 As String

		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

		Public Property Ki1_FakMax As Decimal?
		Public Property Ki2_FakMax As Decimal?
		Public Property Fak_Proz As Decimal?
		Public Property Geb_Zulage As Decimal?
		Public Property Ado_Zulage As Decimal?

		Public Property Bemerkung_1 As String
		Public Property Bemerkung_2 As String
		Public Property Bemerkung_3 As String
		Public Property Bemerkung_4 As String

	End Class


End Class