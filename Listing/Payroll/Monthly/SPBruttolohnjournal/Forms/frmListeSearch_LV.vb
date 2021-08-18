

Option Strict Off

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.CommonXmlUtility

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.UI
Imports SPBruttolohnjournal.ClsDataDetail

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
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

	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVSearchSettingfilename As String

	Private m_xmlSettingRestoreLoBruttolohnSearchSetting As String
	Private m_xmlSettingLoBruttolohnSearchFilter As String

	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "bruttolohnsearchlist"

	Private Const USER_XML_SETTING_SPUTNIK_LOBRUTTOLOHN_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/lobruttolohnsearchlist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_LOBRUTTOLOHN_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/lobruttolohnsearchlist/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String, ByVal _SearchCriteria As SearchCriteria)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		Me.Sql2Open = strQuery
		Me.m_SearchCriteria = _SearchCriteria


		Try
			m_GridSettingPath = String.Format("{0}BruttolohnSearchList\", m_md.GetGridSettingPath(ClsDataDetail.m_InitialData.MDData.MDNr))

			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.m_InitialData.UserData.UserNr)

			m_xmlSettingRestoreLoBruttolohnSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_LOBRUTTOLOHN_SEARCH_LIST_GRIDSETTING_RESTORE, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingLoBruttolohnSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_LOBRUTTOLOHN_SEARCH_LIST_GRIDSETTING_FILTER, ClsDataDetail.m_InitialData.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.m_InitialData.MDData.MDNr))

		Catch ex As Exception

		End Try

		ResetGridLOData()
		ResetGridSummeryData()

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

	Function GetDbLOData4Show() As IEnumerable(Of FoundedLOANData)
		Dim result As List(Of FoundedLOANData) = Nothing

		Dim sql As String

		sql = Sql2Open

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
		listOfParams.Add(New SqlClient.SqlParameter("manrListe", m_SearchCriteria.manr))
		listOfParams.Add(New SqlClient.SqlParameter("monatVon", m_SearchCriteria.vonmonat))
		listOfParams.Add(New SqlClient.SqlParameter("monatBis", m_SearchCriteria.bismonat))
		listOfParams.Add(New SqlClient.SqlParameter("jahrVon", m_SearchCriteria.vonjahr))
		listOfParams.Add(New SqlClient.SqlParameter("jahrBis", m_SearchCriteria.bisjahr))

		listOfParams.Add(New SqlClient.SqlParameter("kst", m_SearchCriteria.kst))
		listOfParams.Add(New SqlClient.SqlParameter("filiale", m_SearchCriteria.filiale))
		listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", m_InitialData.UserData.UserNr))
		listOfParams.Add(New SqlClient.SqlParameter("branchenListe", m_SearchCriteria.esbranche))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedLOANData)

				While reader.Read()
					Dim overviewData As New FoundedLOANData

					overviewData.MANr = m_utility.SafeGetInteger(reader, "MANr", Nothing)

					overviewData.jahr = m_utility.SafeGetInteger(reader, "jahr", Nothing)
					overviewData.monat = m_utility.SafeGetInteger(reader, "lp", Nothing)
					overviewData.kst = m_utility.SafeGetString(reader, "kst")

					overviewData.lanr = m_utility.SafeGetDecimal(reader, "lanr", Nothing)

					overviewData.ahvpflichtig = m_utility.SafeGetBoolean(reader, "ahvpflichtig", Nothing)
					overviewData.bruttopflichtig = m_utility.SafeGetBoolean(reader, "bruttopflichtig", Nothing)
					overviewData.nbuvpflichtig = m_utility.SafeGetBoolean(reader, "nbuvpflichtig", Nothing)
					overviewData.m_btr = m_utility.SafeGetDecimal(reader, "m_btr", Nothing)

					overviewData.gav_beruf = m_utility.SafeGetString(reader, "GAV_Beruf")
					overviewData.gav_kanton = m_utility.SafeGetString(reader, "GAV_Kanton")
					overviewData.esbranche = m_utility.SafeGetString(reader, "ESBranche")
					overviewData.eseinstufung = m_utility.SafeGetString(reader, "ESEinstufung")

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "Nachname"), m_utility.SafeGetString(reader, "Vorname"))

					overviewData.filiale1 = m_utility.SafeGetString(reader, "Filiale1")
					overviewData.filiale2 = m_utility.SafeGetString(reader, "Filiale2")

					overviewData.vonmonat = m_utility.SafeGetInteger(reader, "MonatVon", Nothing)
					overviewData.bismonat = m_utility.SafeGetInteger(reader, "MonatBis", Nothing)

					overviewData.vonjahr = m_utility.SafeGetInteger(reader, "JahrVon", Nothing)
					overviewData.bisjahr = m_utility.SafeGetInteger(reader, "JahrBis", Nothing)

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())
			m_UtilityUi.ShowErrorDialog(String.Format("Fehler bei Auflistung der Daten.{0}{1}", vbNewLine, e.Message))

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Function LoadLOLSummeryData() As IEnumerable(Of FoundedLOSummeryData)
		Dim result As List(Of FoundedLOSummeryData) = Nothing

		Dim sql As String

		sql = "DECLARE @tbl TABLE (Filiale NVARCHAR(50), Bruttolohn MONEY, Suvabasis MONEY, AHVBasis MONEY) "
		sql &= "INSERT INTO @tbl (Filiale) 	"
		sql &= "( "
		sql &= "(SELECT Filiale1 "
		sql &= "FROM {0} B "
		sql &= "GROUP BY Filiale1) "
		sql &= ") "

		sql &= "Update @tbl SET Bruttolohn = "
		sql &= "(SELECT SUM(B.m_btr) "
		sql &= "FROM {0} B "
		sql &= "WHERE B.Bruttopflichtig = 1 AND B.Filiale1 = Filiale "
		sql &= "GROUP BY B.Filiale1) "
		sql &= ", SuvaBasis = "
		sql &= "(SELECT SUM(B.m_btr) "
		sql &= "FROM {0} B "
		sql &= "WHERE B.NBUVpflichtig = 1 AND B.Filiale1 = Filiale "
		sql &= "GROUP BY B.Filiale1) "
		sql &= ", AHVBasis = "
		sql &= "(SELECT SUM(B.m_btr) "
		sql &= "FROM {0} B "
		sql &= "WHERE B.Ahvpflichtig = 1 AND B.Filiale1 = Filiale "
		sql &= "GROUP BY B.Filiale1); "

		sql &= "SELECT * FROM @tbl ORDER BY filiale "
		sql = String.Format(sql, ClsDataDetail.TablenameSource)

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedLOSummeryData)

				While reader.Read()
					Dim overviewData As New FoundedLOSummeryData

					overviewData.filiale = m_utility.SafeGetString(reader, "filiale")

					overviewData.bruttolohn = m_utility.SafeGetDecimal(reader, "bruttolohn", Nothing)
					overviewData.suvabasis = m_utility.SafeGetDecimal(reader, "suvabasis", Nothing)
					overviewData.ahvbasis = m_utility.SafeGetDecimal(reader, "ahvbasis", Nothing)


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())
			m_UtilityUi.ShowErrorDialog(String.Format("Fehler bei Auflistung der Daten.{0}{1}", vbNewLine, e.Message))

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedLOList() As Boolean

		Dim listOfEmployees = GetDbLOData4Show()

		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Die Daten konnten nicht geladen werden.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedLOANData With
																						 {.MANr = person.MANr,
																							.lanr = person.lanr,
																							.lonr = person.lonr,
																							.filiale1 = person.filiale1,
																							.filiale2 = person.filiale2,
																							.jahr = person.jahr,
																							.monat = person.monat,
																							.m_btr = person.m_btr,
																							.employeename = person.employeename,
																							.vonmonat = person.vonmonat,
																							.bismonat = person.bismonat,
																							.vonjahr = person.vonjahr,
																							.bisjahr = person.bisjahr,
																							.gav_beruf = person.gav_beruf,
																							.gav_kanton = person.gav_kanton,
																							.esbranche = person.esbranche,
																							.eseinstufung = person.eseinstufung,
																							.bruttopflichtig = person.bruttopflichtig,
																							.ahvpflichtig = person.ahvpflichtig,
																							.nbuvpflichtig = person.nbuvpflichtig
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedLOANData) = New BindingList(Of FoundedLOANData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Function LoadSummeryList() As Boolean

		Dim listOfEmployees = LoadLOLSummeryData()

		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Die Daten konnten nicht geladen werden.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedLOSummeryData With
																						 {.filiale = person.filiale,
																							.bruttolohn = person.bruttolohn,
																							.suvabasis = person.suvabasis,
																							.ahvbasis = person.ahvbasis
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedLOSummeryData) = New BindingList(Of FoundedLOSummeryData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdSummery.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


#Region "reset gird"

	Private Sub ResetGridLOData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowFooter = True

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

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Lohn-Nr.")
		columnLONr.Name = "lonr"
		columnLONr.FieldName = "lonr"
		columnLONr.Visible = False
		gvRP.Columns.Add(columnLONr)

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
		columnm_btr.SummaryItem.Tag = "Sum_m_btr"
		gvRP.Columns.Add(columnm_btr)

		Dim columnGAVBeruf As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVBeruf.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVBeruf.Caption = m_Translate.GetSafeTranslationValue("GAV-Beruf")
		columnGAVBeruf.Name = "gav_beruf"
		columnGAVBeruf.FieldName = "gav_beruf"
		columnGAVBeruf.Visible = True
		columnGAVBeruf.BestFit()
		gvRP.Columns.Add(columnGAVBeruf)

		Dim columnGAVKanton As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVKanton.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVKanton.Caption = m_Translate.GetSafeTranslationValue("GAV-Kanton")
		columnGAVKanton.Name = "gav_kanton"
		columnGAVKanton.FieldName = "gav_kanton"
		columnGAVKanton.Visible = False
		columnGAVKanton.BestFit()
		gvRP.Columns.Add(columnGAVKanton)

		Dim columnESBranche As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESBranche.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESBranche.Caption = m_Translate.GetSafeTranslationValue("ES-Branche")
		columnESBranche.Name = "esbranche"
		columnESBranche.FieldName = "esbranche"
		columnESBranche.Visible = False
		columnESBranche.BestFit()
		gvRP.Columns.Add(columnESBranche)

		Dim columnESEinstufung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESEinstufung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESEinstufung.Caption = m_Translate.GetSafeTranslationValue("ES-Einstufung")
		columnESEinstufung.Name = "eseinstufung"
		columnESEinstufung.FieldName = "eseinstufung"
		columnESEinstufung.Visible = False
		columnESEinstufung.BestFit()
		gvRP.Columns.Add(columnESEinstufung)


		Dim columnBruttopflichtig As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBruttopflichtig.Caption = m_Translate.GetSafeTranslationValue("Brutto-pflichtig")
		columnBruttopflichtig.Name = "bruttopflichtig"
		columnBruttopflichtig.FieldName = "bruttopflichtig"
		columnBruttopflichtig.Visible = False
		columnBruttopflichtig.BestFit()
		gvRP.Columns.Add(columnBruttopflichtig)

		Dim columnAhvpflichtig As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAhvpflichtig.Caption = m_Translate.GetSafeTranslationValue("AHV-pflichtig")
		columnAhvpflichtig.Name = "ahvpflichtig"
		columnAhvpflichtig.FieldName = "ahvpflichtig"
		columnAhvpflichtig.Visible = False
		columnAhvpflichtig.BestFit()
		gvRP.Columns.Add(columnAhvpflichtig)

		Dim columnnbuvpflichtig As New DevExpress.XtraGrid.Columns.GridColumn()
		columnnbuvpflichtig.Caption = m_Translate.GetSafeTranslationValue("NBUV-pflichtig")
		columnnbuvpflichtig.Name = "nbuvpflichtig"
		columnnbuvpflichtig.FieldName = "nbuvpflichtig"
		columnnbuvpflichtig.Visible = False
		columnnbuvpflichtig.BestFit()
		gvRP.Columns.Add(columnnbuvpflichtig)

		Dim columnfiliale1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfiliale1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfiliale1.Caption = m_Translate.GetSafeTranslationValue("1. Filiale")
		columnfiliale1.Name = "filiale1"
		columnfiliale1.FieldName = "filieal1"
		columnfiliale1.Visible = False
		columnfiliale1.BestFit()
		gvRP.Columns.Add(columnfiliale1)

		Dim columnfiliale2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfiliale2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfiliale2.Caption = m_Translate.GetSafeTranslationValue("2. Filiale")
		columnfiliale2.Name = "filiale2"
		columnfiliale2.FieldName = "filiale2"
		columnfiliale2.Visible = False
		columnfiliale2.BestFit()
		gvRP.Columns.Add(columnfiliale2)

		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub

	Private Sub ResetGridSummeryData()

		gvSummery.OptionsView.ShowIndicator = False
		gvSummery.OptionsView.ShowAutoFilterRow = True
		gvSummery.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvSummery.OptionsView.ShowFooter = True

		gvSummery.Columns.Clear()

		Dim columnfiliale As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
		columnfiliale.Name = "filiale"
		columnfiliale.FieldName = "filiale"
		columnfiliale.Visible = True
		columnfiliale.BestFit()
		gvSummery.Columns.Add(columnfiliale)

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
		columnbruttolohn.SummaryItem.DisplayFormat = "{0:n2}"
		columnbruttolohn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnbruttolohn.SummaryItem.Tag = "Sum_bruttolohn"
		gvSummery.Columns.Add(columnbruttolohn)

		Dim columnsuvabasis As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsuvabasis.Caption = m_Translate.GetSafeTranslationValue("SUVA-Basis")
		columnsuvabasis.Name = "suvabasis"
		columnsuvabasis.FieldName = "suvabasis"
		columnsuvabasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnsuvabasis.AppearanceHeader.Options.UseTextOptions = True
		columnsuvabasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnsuvabasis.DisplayFormat.FormatString = "N2"
		columnsuvabasis.Visible = True
		columnsuvabasis.BestFit()
		columnsuvabasis.SummaryItem.DisplayFormat = "{0:n2}"
		columnsuvabasis.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnsuvabasis.SummaryItem.Tag = "Sum_suvabasis"
		gvSummery.Columns.Add(columnsuvabasis)

		Dim columnahvbasis As New DevExpress.XtraGrid.Columns.GridColumn()
		columnahvbasis.Caption = m_Translate.GetSafeTranslationValue("AHV-Basis")
		columnahvbasis.Name = "ahvbasis"
		columnahvbasis.FieldName = "ahvbasis"
		columnahvbasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnahvbasis.AppearanceHeader.Options.UseTextOptions = True
		columnahvbasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnahvbasis.DisplayFormat.FormatString = "N2"
		columnahvbasis.Visible = True
		columnahvbasis.BestFit()
		columnahvbasis.SummaryItem.DisplayFormat = "{0:n2}"
		columnahvbasis.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnahvbasis.SummaryItem.Tag = "Sum_ahvbasis"
		gvSummery.Columns.Add(columnahvbasis)


		RestoreGridLayoutFromXml()

		grdSummery.DataSource = Nothing

	End Sub


#End Region


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

			LoadFoundedLOList()
			LoadSummeryList()




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


	Private Sub OnGV_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvRP.CustomColumnDisplayText

		If e.Column.FieldName = "m_btr" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			Dim obj As New ThreadTesting.OpenFormsWithThreading()
			Dim viewData As FoundedLOANData
			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)

			If Not dataRow Is Nothing Then
				viewData = CType(dataRow, FoundedLOANData)

				Select Case column.Name.ToLower

					Case Else
						If viewData.MANr.HasValue Then obj.OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.OptionsView.ShowFooter = (gvRP.Columns("m_btr").Visible)
		gvRP.SaveLayoutToXml(m_GVSearchSettingfilename)

	End Sub

	Private Sub gvSummery_KeyDown(sender As Object, e As KeyEventArgs) Handles gvSummery.KeyDown

		If (My.Computer.Keyboard.CtrlKeyDown AndAlso e.KeyCode = Keys.P) Then
			gvSummery.OptionsPrint.PrintPreview = True
			grdSummery.ShowPrintPreview()
		End If

	End Sub

	Private Sub OngvSummery_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvSummery.RowCellStyle

		If (e.RowHandle >= 0) Then
			Dim view As GridView = CType(sender, GridView)
			Dim data = CType(view.GetRow(e.RowHandle), FoundedLOSummeryData)

			If data.filiale = "not defined" Then e.Appearance.BackColor = Color.PaleVioletRed

		End If

	End Sub

	Private Sub OngvSummery_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvSummery.CustomColumnDisplayText
		If e.Column.FieldName = "bruttolohn" OrElse e.Column.FieldName = "suvabasis" OrElse e.Column.FieldName = "ahvbasis" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If
	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLoBruttolohnSearchSetting, False), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLoBruttolohnSearchFilter, False), False)

		Catch ex As Exception

		End Try
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

