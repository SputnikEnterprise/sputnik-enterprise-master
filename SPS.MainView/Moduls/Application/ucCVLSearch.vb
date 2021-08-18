
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects

Imports SP.Infrastructure.Logging
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports System.ComponentModel
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Employee
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.DXperience.Demos
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Data
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Internal.Automations.CVLUtilityData
Imports SP.Internal.Automations.CVLUtilityData.CVLData
Imports DevExpress.Utils.Animation
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Base
Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO
Imports System.Text

Public Class ucCVLSearch


#Region "Public Properties"

	'Public Property CurrentApplicationData As MainViewApplicationData

#End Region

#Region "Private Consts"

	Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

#End Region


#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_AppDatabaseAccess As IAppDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ApplicationUtilWebServiceUri As String

	Private m_customerID As String
	Private m_EMailID As Integer?

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connString As String

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility
	Private m_advisor As AdvisorData

	Private m_JobGroup As List(Of SearchExperiencesViewData)
	Private m_CompentenceData As List(Of SearchExperiencesViewData)
	Private m_LanguageData As List(Of SearchLanguageViewData)
	Private m_CurrentQueryResultData As BindingList(Of CVLSearchResultViewData)
	Private m_CurrentSearchHistoryData As BindingList(Of CVLSearchHistoryViewData)

	Private m_CVLUtility As CVLData

#End Region



#Region "constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_customerID = m_InitializationData.MDData.MDGuid
		m_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI

		m_CVLUtility = New CVLData(m_InitializationData)
		m_JobGroup = New List(Of SearchExperiencesViewData)
		m_CompentenceData = New List(Of SearchExperiencesViewData)
		m_LanguageData = New List(Of SearchLanguageViewData)


		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
		WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

		m_connString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)
		m_AppDatabaseAccess = New AppDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)

		If ModulConstants.UserData.UserNr <> 1 Then
			grpJobGroups.CustomHeaderButtons(0).Properties.Enabled = False
			grpJobGroups.CustomHeaderButtons(1).Properties.Enabled = False
			grpCompetences.CustomHeaderButtons(0).Properties.Enabled = False
			grpCompetences.CustomHeaderButtons(1).Properties.Enabled = False
			grpLanguages.CustomHeaderButtons(0).Properties.Enabled = False
			grpLanguages.CustomHeaderButtons(1).Properties.Enabled = False

			grpCompetences.CustomHeaderButtons(0).Properties.Checked = False
			grpCompetences.CustomHeaderButtons(1).Properties.Checked = True
			grpLanguages.CustomHeaderButtons(0).Properties.Checked = False
			grpLanguages.CustomHeaderButtons(1).Properties.Checked = True
		End If

		TranslateControls()
		Reset()

		AddHandler luePostcodeCity.ButtonClick, AddressOf OnDropDownButtonClick
		AddHandler lueRadius.ButtonClick, AddressOf OnDropDownButtonClick
		AddHandler lueopAreas.ButtonClick, AddressOf OnDropDownButtonClick
		AddHandler lueSkills.ButtonClick, AddressOf OnDropDownButtonClick
		AddHandler lueLanguages.ButtonClick, AddressOf OnDropDownButtonClick

		m_SuppressUIEvents = False

	End Sub


#End Region


#Region "Public Methods"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		success = success AndAlso LoadPostcodeCityDropDownData()
		success = success AndAlso LoadJobGroupsDropDownData()
		success = success AndAlso LoadDistancesDropDown()
		success = success AndAlso LoadLanguageDropDownData()
		success = success AndAlso LoadExperienceDropDownData()

		success = success AndAlso LoadCVLSearchHistoryData()

		m_SuppressUIEvents = suppressState

		Return success

	End Function

	Public Sub CleanUp()
		Dim success As Boolean = True


	End Sub


#End Region


#Region "private properties"

	Private ReadOnly Property SelectedGVLueJobGroupsRecord As SearchExperiencesViewData
		Get
			Dim gvRP = TryCast(lueopAreas.Properties.View, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchExperiencesViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedJobGroupRecord As SearchExperiencesViewData
		Get
			Dim gvRP = TryCast(grdopAreas.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchExperiencesViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedGVLueCompentenceRecord As SearchExperiencesViewData
		Get
			Dim gvRP = TryCast(lueSkills.Properties.View, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchExperiencesViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedCompentenceRecord As SearchExperiencesViewData
		Get
			Dim gvRP = TryCast(grdSkills.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchExperiencesViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedGVLueLanguageRecord As SearchLanguageViewData
		Get
			Dim gvRP = TryCast(lueLanguages.Properties.View, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchLanguageViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedLanguageRecord As SearchLanguageViewData
		Get
			Dim gvRP = TryCast(grdLanguages.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), SearchLanguageViewData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedSearchHistoryRecord As CVLSearchHistoryViewData
		Get
			Dim gvRP = TryCast(grdSearchHistory.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim searchData = CType(gvRP.GetRow(selectedRows(0)), CVLSearchHistoryViewData)
					Return searchData
				End If

			End If

			Return Nothing
		End Get

	End Property


#End Region


	Sub TranslateControls()

		Dim andLabel As String = m_Translate.GetSafeTranslationValue("und")
		Dim orLabel As String = m_Translate.GetSafeTranslationValue("oder")

		grpBewerbung.Text = m_Translate.GetSafeTranslationValue(grpBewerbung.Text)
		lblOrtRadius.Text = m_Translate.GetSafeTranslationValue(lblOrtRadius.Text)

		grpBewerbung.CustomHeaderButtons(0).Properties.Caption = grpBewerbung.CustomHeaderButtons(0).Properties.Caption.ToString()
		grpBewerbung.CustomHeaderButtons(1).Properties.Caption = grpBewerbung.CustomHeaderButtons(1).Properties.Caption.ToString()

		lblBeruf.Text = m_Translate.GetSafeTranslationValue(lblBeruf.Text)
		lblTextsuche.Text = m_Translate.GetSafeTranslationValue(lblTextsuche.Text)

		grpJobGroups.Text = m_Translate.GetSafeTranslationValue(grpJobGroups.Text)
		grpJobGroups.CustomHeaderButtons(0).Properties.Caption = andLabel.ToString().ToUpper
		grpJobGroups.CustomHeaderButtons(1).Properties.Caption = orLabel.ToString().ToUpper

		grpCompetences.Text = m_Translate.GetSafeTranslationValue(grpCompetences.Text)
		grpCompetences.CustomHeaderButtons(0).Properties.Caption = andLabel.ToString().ToUpper
		grpCompetences.CustomHeaderButtons(1).Properties.Caption = orLabel.ToString().ToUpper

		grpLanguages.Text = m_Translate.GetSafeTranslationValue(grpLanguages.Text)
		grpLanguages.CustomHeaderButtons(0).Properties.Caption = andLabel.ToString().ToUpper
		grpLanguages.CustomHeaderButtons(1).Properties.Caption = orLabel.ToString().ToUpper

		grpResult.Text = m_Translate.GetSafeTranslationValue(grpResult.Text)

		bsiLblCurrentCount.Caption = m_Translate.GetSafeTranslationValue(bsiLblCurrentCount.Caption)
		bsiLblJobNotifyer.Caption = m_Translate.GetSafeTranslationValue(bsiLblJobNotifyer.Caption)
		bbiSearch.Caption = m_Translate.GetSafeTranslationValue(bbiSearch.Caption)

	End Sub


#Region "reset"

	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		luePostcodeCity.EditValue = Nothing
		lueopAreas.EditValue = Nothing
		lueSkills.EditValue = Nothing
		lueLanguages.EditValue = Nothing
		lueRadius.EditValue = Nothing

		txtJobtitels.EditValue = String.Empty
		pnlQualificationField.Visible = False

		grdopAreas.DataSource = Nothing
		grdSkills.DataSource = Nothing
		grdLanguages.DataSource = Nothing
		grdResult.DataSource = Nothing

		ResetPostcodeCityDropDown()

		ResetJobGroupsDropDown()
		ResetJobGroupsGrid()

		ResetCompetencesDropDown()
		ResetCompetencesGrid()

		ResetLanguagesDropDown()
		ResetLanguagesGrid()

		bbiJobNotifyer.Enabled = True
		beiTplName.Enabled = True

		pnlSearchhistory.Visible = False

		grdResult.Dock = DockStyle.Fill
		ResetSearchHistoryGrid()
		ResetSearchResultGrid()


		m_SuppressUIEvents = suppressState

	End Sub

	''' <summary>
	''' Resets the checkedfrom advisors drop down.
	''' </summary>
	Private Sub ResetPostcodeCityDropDown()

		luePostcodeCity.Properties.DisplayMember = "Addresse"
		luePostcodeCity.Properties.ValueMember = "Postcode"

		gvluePostcodeCity.OptionsView.ShowIndicator = False
		gvluePostcodeCity.OptionsView.ShowColumnHeaders = True
		gvluePostcodeCity.OptionsView.ShowFooter = False
		luePostcodeCity.Properties.View.ExpandAllGroups()

		gvluePostcodeCity.OptionsView.ShowAutoFilterRow = True
		gvluePostcodeCity.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvluePostcodeCity.Columns.Clear()

		Dim columnGroup_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGroup_Translated.Caption = m_Translate.GetSafeTranslationValue("PLZ")
		columnGroup_Translated.Name = "Postcode"
		columnGroup_Translated.FieldName = "Postcode"
		columnGroup_Translated.Visible = True
		columnGroup_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvluePostcodeCity.Columns.Add(columnGroup_Translated)

		Dim columnBez_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBez_Translated.Caption = m_Translate.GetSafeTranslationValue("Ortschaft")
		columnBez_Translated.Name = "City"
		columnBez_Translated.FieldName = "City"
		columnBez_Translated.Visible = True
		columnBez_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvluePostcodeCity.Columns.Add(columnBez_Translated)


		'columnGroup_Translated.GroupIndex = 0
		luePostcodeCity.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePostcodeCity.Properties.NullText = String.Empty
		luePostcodeCity.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the advisors drop down.
	''' </summary>
	Private Sub ResetJobGroupsDropDown()

		lueopAreas.Properties.DisplayMember = "ExperienceLabel"
		lueopAreas.Properties.ValueMember = "Code"

		gvlueGroups.OptionsView.ShowIndicator = False
		gvlueGroups.OptionsView.ShowColumnHeaders = True
		gvlueGroups.OptionsView.ShowFooter = False
		lueopAreas.Properties.View.ExpandAllGroups()

		gvlueGroups.OptionsView.ShowAutoFilterRow = True
		gvlueGroups.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvlueGroups.Columns.Clear()

		Dim columnGroup_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGroup_Translated.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnGroup_Translated.Name = "ExperienceLabel"
		columnGroup_Translated.FieldName = "ExperienceLabel"
		columnGroup_Translated.Visible = True
		columnGroup_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueGroups.Columns.Add(columnGroup_Translated)


		'columnGroup_Translated.GroupIndex = 0
		lueopAreas.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueopAreas.Properties.NullText = String.Empty
		lueopAreas.EditValue = Nothing

	End Sub

	Private Sub ResetJobGroupsGrid()

		gvJobGroups.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvJobGroups.OptionsView.ShowIndicator = False
		gvJobGroups.OptionsView.ShowAutoFilterRow = True
		gvJobGroups.OptionsView.ColumnAutoWidth = True
		gvJobGroups.OptionsView.RowAutoHeight = True
		gvJobGroups.OptionsView.ShowColumnHeaders = False

		Dim showFooter As Boolean = False
		gvJobGroups.OptionsView.ShowFooter = showFooter

		gvJobGroups.Columns.Clear()

		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Code")
		columnCreatedFrom.Name = "Code"
		columnCreatedFrom.FieldName = "Code"
		columnCreatedFrom.BestFit()
		columnCreatedFrom.Visible = False
		gvJobGroups.Columns.Add(columnCreatedFrom)

		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsColumn.AllowEdit = False
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnServiceDate.Name = "ExperienceLabel"
		columnServiceDate.FieldName = "ExperienceLabel"
		columnServiceDate.BestFit()
		columnServiceDate.Visible = True
		gvJobGroups.Columns.Add(columnServiceDate)



		m_SuppressUIEvents = True
		grdopAreas.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Resets the competences drop down.
	''' </summary>
	Private Sub ResetCompetencesDropDown()

		lueSkills.Properties.DisplayMember = "ExperienceLabel"
		lueSkills.Properties.ValueMember = "Code"

		gvlueCompetences.OptionsView.ShowIndicator = False
		gvlueCompetences.OptionsView.ShowColumnHeaders = True
		gvlueCompetences.OptionsView.ShowFooter = False
		lueSkills.Properties.View.ExpandAllGroups()

		gvlueCompetences.OptionsView.ShowAutoFilterRow = True
		gvlueCompetences.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvlueCompetences.Columns.Clear()

		Dim columnGroup_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGroup_Translated.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnGroup_Translated.Name = "ExperienceLabel"
		columnGroup_Translated.FieldName = "ExperienceLabel"
		columnGroup_Translated.Visible = True
		columnGroup_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCompetences.Columns.Add(columnGroup_Translated)


		'columnGroup_Translated.GroupIndex = 0
		lueSkills.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueSkills.Properties.NullText = String.Empty
		lueSkills.EditValue = Nothing

	End Sub

	Private Sub ResetCompetencesGrid()

		gvCompetences.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCompetences.OptionsView.ShowIndicator = False
		gvCompetences.OptionsView.ShowAutoFilterRow = True
		gvCompetences.OptionsView.ColumnAutoWidth = True
		gvCompetences.OptionsView.RowAutoHeight = True
		gvCompetences.OptionsView.ShowColumnHeaders = False

		Dim showFooter As Boolean = False
		gvCompetences.OptionsView.ShowFooter = showFooter

		gvCompetences.Columns.Clear()


		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Code")
		columnCreatedFrom.Name = "Code"
		columnCreatedFrom.FieldName = "Code"
		columnCreatedFrom.BestFit()
		columnCreatedFrom.Visible = False
		gvCompetences.Columns.Add(columnCreatedFrom)

		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsColumn.AllowEdit = False
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnServiceDate.Name = "ExperienceLabel"
		columnServiceDate.FieldName = "ExperienceLabel"
		columnServiceDate.BestFit()
		columnServiceDate.Visible = True
		gvCompetences.Columns.Add(columnServiceDate)


		m_SuppressUIEvents = True
		grdSkills.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Resets the language drop down.
	''' </summary>
	Private Sub ResetLanguagesDropDown()

		lueLanguages.Properties.DisplayMember = "LanguageLabel"
		lueLanguages.Properties.ValueMember = "LanguageCode"

		gvlueLanguages.OptionsView.ShowIndicator = False
		gvlueLanguages.OptionsView.ShowColumnHeaders = True
		gvlueLanguages.OptionsView.ShowFooter = False
		lueLanguages.Properties.View.ExpandAllGroups()

		gvlueLanguages.OptionsView.ShowAutoFilterRow = True
		gvlueLanguages.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvlueLanguages.Columns.Clear()

		Dim columnGroup_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGroup_Translated.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnGroup_Translated.Name = "LanguageLabel"
		columnGroup_Translated.FieldName = "LanguageLabel"
		columnGroup_Translated.Visible = True
		columnGroup_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueLanguages.Columns.Add(columnGroup_Translated)


		'columnGroup_Translated.GroupIndex = 0
		lueLanguages.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLanguages.Properties.NullText = String.Empty
		lueLanguages.EditValue = Nothing

	End Sub

	Private Sub ResetLanguagesGrid()

		gvLanguages.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvLanguages.OptionsView.ShowIndicator = False
		gvLanguages.OptionsView.ShowAutoFilterRow = True
		gvLanguages.OptionsView.ColumnAutoWidth = True
		gvLanguages.OptionsView.RowAutoHeight = True
		gvLanguages.OptionsView.ShowColumnHeaders = False

		Dim showFooter As Boolean = False
		gvLanguages.OptionsView.ShowFooter = showFooter

		gvLanguages.Columns.Clear()


		Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedFrom.OptionsColumn.AllowEdit = False
		columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Code")
		columnCreatedFrom.Name = "LanguageCode"
		columnCreatedFrom.FieldName = "LanguageCode"
		columnCreatedFrom.BestFit()
		columnCreatedFrom.Visible = False
		gvLanguages.Columns.Add(columnCreatedFrom)

		Dim columnServiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnServiceDate.OptionsColumn.AllowEdit = False
		columnServiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnServiceDate.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnServiceDate.Name = "LanguageLabel"
		columnServiceDate.FieldName = "LanguageLabel"
		columnServiceDate.BestFit()
		columnServiceDate.Visible = True
		gvLanguages.Columns.Add(columnServiceDate)


		m_SuppressUIEvents = True
		grdLanguages.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetSearchHistoryGrid()

		gvSearchhistory.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvSearchhistory.OptionsView.ShowIndicator = False
		gvSearchhistory.OptionsView.ShowAutoFilterRow = True
		gvSearchhistory.OptionsView.ColumnAutoWidth = True
		gvSearchhistory.OptionsView.RowAutoHeight = True
		gvSearchhistory.OptionsView.ShowColumnHeaders = True

		Dim showFooter As Boolean = False
		gvSearchhistory.OptionsView.ShowFooter = showFooter

		gvSearchhistory.Columns.Clear()


		Dim columnApplicantFullnameWithComma As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicantFullnameWithComma.OptionsColumn.AllowEdit = False
		columnApplicantFullnameWithComma.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicantFullnameWithComma.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnApplicantFullnameWithComma.Name = "QueryNameWithCount"
		columnApplicantFullnameWithComma.FieldName = "QueryNameWithCount"
		columnApplicantFullnameWithComma.BestFit()
		columnApplicantFullnameWithComma.Visible = True
		gvSearchhistory.Columns.Add(columnApplicantFullnameWithComma)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsColumn.AllowEdit = False
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.BestFit()
		columnCreatedOn.Visible = True
		gvSearchhistory.Columns.Add(columnCreatedOn)

		Dim columnJobTitel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobTitel.OptionsColumn.AllowEdit = False
		columnJobTitel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnJobTitel.Caption = m_Translate.GetSafeTranslationValue("Notify")
		columnJobTitel.Name = "Notify"
		columnJobTitel.FieldName = "Notify"
		columnJobTitel.BestFit()
		columnJobTitel.Visible = True
		gvSearchhistory.Columns.Add(columnJobTitel)


		m_SuppressUIEvents = True
		grdSearchHistory.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetSearchResultGrid()

		gvResult.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvResult.OptionsView.ShowIndicator = False
		gvResult.OptionsView.ShowAutoFilterRow = True
		gvResult.OptionsView.ColumnAutoWidth = True
		gvResult.OptionsView.RowAutoHeight = True
		'gvResult.OptionsView.ShowColumnHeaders = False

		Dim showFooter As Boolean = False
		gvResult.OptionsView.ShowFooter = showFooter

		gvResult.Columns.Clear()


		Dim columnApplicantFullnameWithComma As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicantFullnameWithComma.OptionsColumn.AllowEdit = False
		columnApplicantFullnameWithComma.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicantFullnameWithComma.Caption = m_Translate.GetSafeTranslationValue("Nach-/Vorname")
		columnApplicantFullnameWithComma.Name = "ApplicantFullnameWithComma"
		columnApplicantFullnameWithComma.FieldName = "ApplicantFullnameWithComma"
		columnApplicantFullnameWithComma.BestFit()
		columnApplicantFullnameWithComma.Visible = True
		gvResult.Columns.Add(columnApplicantFullnameWithComma)

		Dim columnJobTitel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobTitel.OptionsColumn.AllowEdit = False
		columnJobTitel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnJobTitel.Caption = m_Translate.GetSafeTranslationValue("Beruf")
		columnJobTitel.Name = "JobTitel"
		columnJobTitel.FieldName = "JobTitel"
		columnJobTitel.BestFit()
		columnJobTitel.Visible = True
		gvResult.Columns.Add(columnJobTitel)

		Dim columnApplicantPostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnApplicantPostcodeLocation.OptionsColumn.AllowEdit = False
		columnApplicantPostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnApplicantPostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("PLZ / Ort")
		columnApplicantPostcodeLocation.Name = "ApplicantPostcodeLocation"
		columnApplicantPostcodeLocation.FieldName = "ApplicantPostcodeLocation"
		columnApplicantPostcodeLocation.BestFit()
		columnApplicantPostcodeLocation.Visible = True
		gvResult.Columns.Add(columnApplicantPostcodeLocation)

		Dim columnAge As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAge.OptionsColumn.AllowEdit = False
		columnAge.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnAge.Caption = m_Translate.GetSafeTranslationValue("Alter")
		columnAge.Name = "EmployeeAge"
		columnAge.FieldName = "EmployeeAge"
		columnAge.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnAge.AppearanceHeader.Options.UseTextOptions = True
		columnAge.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnAge.DisplayFormat.FormatString = "F0"
		columnAge.MaxWidth = 50
		columnAge.Visible = True
		gvResult.Columns.Add(columnAge)

		Dim columnCreatedMonth As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedMonth.OptionsColumn.AllowEdit = False
		columnCreatedMonth.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
		columnCreatedMonth.Name = "CreatedMonth"
		columnCreatedMonth.FieldName = "CreatedMonth"
		columnCreatedMonth.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnCreatedMonth.AppearanceHeader.Options.UseTextOptions = True
		columnCreatedMonth.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnCreatedMonth.DisplayFormat.FormatString = "F0"
		columnCreatedMonth.MaxWidth = 50
		columnCreatedMonth.Visible = False
		gvResult.Columns.Add(columnCreatedMonth)

		Dim columnCreatedYear As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedYear.OptionsColumn.AllowEdit = False
		columnCreatedYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
		columnCreatedYear.Name = "CreatedYear"
		columnCreatedYear.FieldName = "CreatedYear"
		columnCreatedYear.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnCreatedYear.AppearanceHeader.Options.UseTextOptions = True
		columnCreatedYear.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnCreatedYear.DisplayFormat.FormatString = "F0"
		columnCreatedYear.MaxWidth = 50
		columnCreatedYear.Visible = False
		gvResult.Columns.Add(columnCreatedYear)

		Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedOn.OptionsColumn.AllowEdit = False
		columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCreatedOn.Name = "CreatedOn"
		columnCreatedOn.FieldName = "CreatedOn"
		columnCreatedOn.BestFit()
		columnCreatedOn.Visible = True
		gvResult.Columns.Add(columnCreatedOn)


		m_SuppressUIEvents = True
		grdResult.DataSource = Nothing
		m_SuppressUIEvents = False

	End Sub

#End Region


#Region "loading data"

	''' <summary>
	''' Loads the postcode and city drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadPostcodeCityDropDownData() As Boolean

		Dim listDataSource As BindingList(Of SearchPostcodeCityViewData) = New BindingList(Of SearchPostcodeCityViewData)
		listDataSource = m_CVLUtility.LoadCVLPostcodeCityData()

		luePostcodeCity.Properties.DataSource = listDataSource

		Return Not listDataSource Is Nothing

	End Function

	''' <summary>
	''' Loads the job group drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadJobGroupsDropDownData() As Boolean

		Dim listDataSource As BindingList(Of SearchExperiencesViewData) = New BindingList(Of SearchExperiencesViewData)
		listDataSource = m_CVLUtility.LoadJobGroupsData()

		lueopAreas.Properties.DataSource = listDataSource

		Return Not listDataSource Is Nothing

	End Function

	Function LoadDistancesDropDown() As Boolean
		Dim list = New List(Of Integer)
		list = m_CVLUtility.LoadDistancesData()

		lueRadius.Properties.DataSource = list


		Return (Not list Is Nothing)

	End Function

	''' <summary>
	''' Loads the language drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadLanguageDropDownData() As Boolean

		Dim listDataSource As BindingList(Of SearchLanguageViewData) = New BindingList(Of SearchLanguageViewData)
		listDataSource = m_CVLUtility.LoadLanguageData()

		lueLanguages.Properties.DataSource = listDataSource

		Return Not listDataSource Is Nothing

	End Function

	''' <summary>
	''' Loads the compentence drop down data.
	''' </summary>
	Private Function LoadExperienceDropDownData() As Boolean

		Dim listDataSource As BindingList(Of SearchExperiencesViewData) = New BindingList(Of SearchExperiencesViewData)
		listDataSource = m_CVLUtility.LoadExperienceData()

		lueSkills.Properties.DataSource = listDataSource

		Return Not listDataSource Is Nothing

	End Function

	''' <summary>
	''' Loads the search history grid.
	''' </summary>
	Private Function LoadCVLSearchHistoryData() As Boolean

		Dim listDataSource As BindingList(Of CVLSearchHistoryViewData) = New BindingList(Of CVLSearchHistoryViewData)

		Try

			listDataSource = m_CVLUtility.LoadCVLSearchHistoryData()
			grdSearchHistory.DataSource = listDataSource


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return False
		End Try

		m_CurrentSearchHistoryData = listDataSource
		grpBewerbung.CustomHeaderButtons(1).Properties.Enabled = listDataSource.Count > 0
		pnlSearchhistory.Visible = listDataSource.Count > 0


		Return Not listDataSource Is Nothing

	End Function

	''' <summary>
	''' Loads assigned search history result data.
	''' </summary>
	Private Function LoadAssignedCVLSearchHistoryResultData(ByVal searchID As Integer) As Boolean

		Dim listDataSource As BindingList(Of CVLSearchResultViewData) = New BindingList(Of CVLSearchResultViewData)

		Try
			listDataSource = m_CVLUtility.LoadAssignedCVLSearchHistoryResultData(searchID)

			grdResult.DataSource = listDataSource
			m_CurrentQueryResultData = listDataSource

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return False
		End Try

		bbiJobNotifyer.Enabled = listDataSource.Count > 0
		beiTplName.Enabled = listDataSource.Count > 0

		Return Not listDataSource Is Nothing

	End Function

	''' <summary>
	''' delete assigned search history data.
	''' </summary>
	Private Function DeleteAssignedCVLSearchHistoryData(ByVal searchID As Integer) As Boolean

		Dim success As Boolean = True
		Try

			success = success AndAlso m_CVLUtility.DeleteAssignedCVLSearchHistoryData(searchID)

			grdResult.DataSource = Nothing
			m_CurrentQueryResultData = Nothing

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		End Try

		bbiJobNotifyer.Enabled = False
		beiTplName.Enabled = False

		'FocusSearchHistoryData(0)

		Return success

	End Function


#End Region


	Private Sub bbiSearch_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		grdResult.DataSource = Nothing

		Dim data = LoadSearchResultData()
		If data Is Nothing Then
			bsiCurrentCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), m_Translate.GetSafeTranslationValue("Fehler in der Abfrage."))
			m_UtilityUI.ShowErrorDialog(String.Format("Keine Daten wurden gefunden. Fehlerhafte Abfrage."))

			Return
		End If

		grdResult.DataSource = data

		' load new saved search history data
		LoadCVLSearchHistoryData()


		bsiCurrentCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), data.Count)

	End Sub

	Private Sub OnbbiClear_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		luePostcodeCity.EditValue = Nothing
		lueRadius.EditValue = Nothing

		lueopAreas.EditValue = Nothing
		grdopAreas.DataSource = Nothing

		lueSkills.EditValue = Nothing
		grdSkills.DataSource = Nothing

		lueLanguages.EditValue = Nothing
		grdLanguages.DataSource = Nothing

		grdResult.DataSource = Nothing

		beiTplName.EditValue = Nothing
		bbiJobNotifyer.EditValue = True

	End Sub

	Private Function LoadSearchResultData() As IEnumerable(Of CVLSearchResultViewData)
		Dim listDataSource As BindingList(Of CVLSearchResultViewData) = New BindingList(Of CVLSearchResultViewData)

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim postCodeCityData = New SearchPostcodeCityViewData
		Dim radius As Integer = 0
		Dim jobTitelData = New List(Of SearchExperiencesViewData)
		Dim jobGroupData = New List(Of SearchExperiencesViewData)
		Dim experienceData = New List(Of SearchExperiencesViewData)
		Dim languageData = New List(Of SearchLanguageViewData)

		If Not luePostcodeCity.EditValue Is Nothing Then
			'Dim data = CType(luePostcodeCity.EditValue, SearchPostcodeCityViewData)
			postCodeCityData.Customer_ID = m_customerID
			postCodeCityData.Postcode = luePostcodeCity.EditValue
			radius = lueRadius.EditValue
		End If

		Dim titels = txtJobtitels.EditValue.ToString.Split(",").ToList()
		If pnlQualificationField.Visible AndAlso Not titels Is Nothing AndAlso titels.Count > 0 Then
			For Each itm In titels
				Dim data = New SearchExperiencesViewData
				data.Customer_ID = m_customerID
				data.Code = itm
				data.ExperienceLabel = itm

				jobTitelData.Add(data)
			Next
		End If

		If Not m_JobGroup Is Nothing AndAlso m_JobGroup.Count > 0 Then
			For Each itm In m_JobGroup
				Dim data = New SearchExperiencesViewData
				data.Customer_ID = m_customerID
				data.Code = itm.Code
				data.ExperienceLabel = itm.ExperienceLabel

				jobGroupData.Add(data)
			Next
		End If
		If Not m_CompentenceData Is Nothing AndAlso m_CompentenceData.Count > 0 Then
			For Each itm In m_CompentenceData
				Dim data = New SearchExperiencesViewData
				data.Customer_ID = m_customerID
				data.Code = itm.Code
				data.ExperienceLabel = itm.ExperienceLabel

				experienceData.Add(data)
			Next
		End If
		If Not m_LanguageData Is Nothing AndAlso m_LanguageData.Count > 0 Then
			For Each itm In m_LanguageData
				Dim data = New SearchLanguageViewData
				data.Customer_ID = m_customerID
				data.LanguageCode = itm.LanguageCode
				data.LanguageLabel = itm.LanguageLabel

				languageData.Add(data)
			Next
		End If

		Try
			Dim joinEnum As JoinENum = JoinENum.ODER

			m_CVLUtility.PostCodeCityData = New List(Of SearchPostcodeCityViewData)
			m_CVLUtility.PostCodeCityData.Add(postCodeCityData)
			m_CVLUtility.SearchRadius = radius
			m_CVLUtility.JobTitelsData = jobTitelData

			m_CVLUtility.OperationAreasData = jobGroupData ' New List(Of SearchExperiencesViewData)
			If grpJobGroups.CustomHeaderButtons(0).Properties.Checked Then joinEnum = JoinENum.UND Else joinEnum = JoinENum.ODER
			m_CVLUtility.OperationAreasJoin = joinEnum

			m_CVLUtility.SkillsData = experienceData
			If grpCompetences.CustomHeaderButtons(0).Properties.Checked Then joinEnum = JoinENum.UND Else joinEnum = JoinENum.ODER
			m_CVLUtility.SkillsJoin = joinEnum

			m_CVLUtility.LanguagesData = languageData
			If grpLanguages.CustomHeaderButtons(0).Properties.Checked Then joinEnum = JoinENum.UND Else joinEnum = JoinENum.ODER
			m_CVLUtility.LanguagesJoin = joinEnum
			m_CVLUtility.SearchLabel = beiTplName.EditValue
			m_CVLUtility.SetNotification = bbiJobNotifyer.EditValue

			If postCodeCityData Is Nothing AndAlso (jobGroupData Is Nothing OrElse jobGroupData.Count = 0) AndAlso (experienceData Is Nothing OrElse experienceData.Count = 0) AndAlso (languageData Is Nothing OrElse languageData.Count = 0) Then Return Nothing

			Dim filename As String = "C:\Path\test.xml"
			Dim xml = GenericSerializer.Serialize(Of List(Of SearchExperiencesViewData))(jobGroupData)
			Dim test = GenericSerializer.Deserialize(Of List(Of SearchExperiencesViewData))(xml)

			'Dim serializer = New XmlSerializer(experienceData.GetType())
			'Using writer = XmlWriter.Create(filename)
			'	serializer.Serialize(writer, experienceData)
			'End Using

			''Dim dSerializer As New XmlSerializer(jobGroupData.GetType())
			'Using reader = XmlReader.Create(filename)
			'	jobGroupData = CType(serializer.Deserialize(reader), List(Of SearchExperiencesViewData))
			'	grdopAreas.DataSource = jobGroupData
			'End Using

			listDataSource = m_CVLUtility.LoadSearchResultData()
			m_CurrentQueryResultData = listDataSource


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString()))

			listDataSource = Nothing

		Finally
			SplashScreenManager.CloseForm(False)

		End Try


		Return listDataSource

	End Function

	Private Sub OnrepTplNameButtonEdit1_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles repTplNameButtonEdit1.ButtonClick

		If e.Button.Index = 0 Then

			If m_CurrentQueryResultData Is Nothing Then
				Dim msg As String = "Achtung, Sie haben keine Suche gestartet. Ihre Abfrage kann nicht gespeichert werden!"
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

				Return
			End If

			LoadSearchResultData()
		End If

	End Sub

	Private Sub OngvlueGroups_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles gvlueGroups.RowCellClick
		Dim addItem As Boolean = True

		If m_SuppressUIEvents Then
			Return
		End If
		Dim data = SelectedGVLueJobGroupsRecord
		For Each itm In m_JobGroup
			If itm.Code = data.Code Then
				addItem = False
				Exit For
			End If
		Next
		If addItem Then
			m_JobGroup.Add(New SearchExperiencesViewData With {.Customer_ID = m_customerID, .Code = data.Code, .ExperienceLabel = data.ExperienceLabel})
		End If
		grdopAreas.DataSource = m_JobGroup
		gvJobGroups.RefreshData()

	End Sub

	Private Sub OngvlueCompetences_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles gvlueCompetences.RowCellClick
		Dim addItem As Boolean = True

		If m_SuppressUIEvents Then
			Return
		End If

		Dim data = SelectedGVLueCompentenceRecord
		For Each itm In m_CompentenceData
			If itm.Code = data.Code Then
				addItem = False
				Exit For
			End If
		Next
		If addItem Then
			m_CompentenceData.Add(New SearchExperiencesViewData With {.Customer_ID = m_customerID, .Code = data.Code, .ExperienceLabel = data.ExperienceLabel})
		End If
		grdSkills.DataSource = m_CompentenceData
		gvCompetences.RefreshData()

	End Sub

	Private Sub OngvlueLanguages_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles gvlueLanguages.RowCellClick
		Dim addItem As Boolean = True

		If m_SuppressUIEvents Then
			Return
		End If

		Dim data = SelectedGVLueLanguageRecord
		For Each itm In m_LanguageData
			If itm.LanguageCode = data.LanguageCode Then
				addItem = False
				Exit For
			End If
		Next
		If addItem Then
			m_LanguageData.Add(New SearchLanguageViewData With {.Customer_ID = m_customerID, .LanguageCode = data.LanguageCode, .LanguageLabel = data.LanguageLabel})
		End If
		grdLanguages.DataSource = m_LanguageData
		gvLanguages.RefreshData()

	End Sub

	Private Sub OngrpJobGroups_CustomButtonChecked(sender As Object, e As BaseButtonEventArgs) Handles grpJobGroups.CustomButtonChecked

		If e.Button.Properties.GroupIndex = 0 Then
			grpJobGroups.CustomHeaderButtons(0).Properties.Checked = True
			grpJobGroups.CustomHeaderButtons(1).Properties.Checked = False
		ElseIf e.Button.Properties.GroupIndex = 1 Then
			grpJobGroups.CustomHeaderButtons(0).Properties.Checked = False
			grpJobGroups.CustomHeaderButtons(1).Properties.Checked = True
		End If

	End Sub

	Private Sub OngrpCompetences_CustomButtonChecked(sender As Object, e As BaseButtonEventArgs) Handles grpCompetences.CustomButtonChecked

		If e.Button.Properties.GroupIndex = 0 Then
			grpCompetences.CustomHeaderButtons(0).Properties.Checked = True
			grpCompetences.CustomHeaderButtons(1).Properties.Checked = False
		ElseIf e.Button.Properties.GroupIndex = 1 Then
			grpCompetences.CustomHeaderButtons(0).Properties.Checked = False
			grpCompetences.CustomHeaderButtons(1).Properties.Checked = True
		End If

	End Sub

	Private Sub OnggrpLanguages_CustomButtonChecked(sender As Object, e As BaseButtonEventArgs) Handles grpLanguages.CustomButtonChecked

		If e.Button.Properties.GroupIndex = 0 Then
			grpLanguages.CustomHeaderButtons(0).Properties.Checked = True
			grpLanguages.CustomHeaderButtons(1).Properties.Checked = False
		ElseIf e.Button.Properties.GroupIndex = 1 Then
			grpLanguages.CustomHeaderButtons(0).Properties.Checked = False
			grpLanguages.CustomHeaderButtons(1).Properties.Checked = True
		End If

	End Sub

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvResult.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvResult.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, CVLSearchResultViewData)

				If viewData.EmployeeID > 0 Then
					Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr, .SelectedMANr = viewData.EmployeeID})

					Dim data = LoadApplicantData(viewData.EmployeeID)
					If data.ShowAsApplicant.GetValueOrDefault(False) Then
						_ClsMA.OpenSelectedApplicant(m_InitializationData.MDData.MDNr, ModulConstants.UserData.UserNr)
					Else
						_ClsMA.OpenSelectedEmployee(m_InitializationData.MDData.MDNr, ModulConstants.UserData.UserNr)
					End If
				End If

			End If

		End If

	End Sub

	Private Sub OngvSearchhistory_RowCellClick(sender As Object, e As RowCellClickEventArgs) Handles gvSearchhistory.RowCellClick
		Dim addItem As Boolean = True

		If m_SuppressUIEvents Then
			Return
		End If

		m_SuppressUIEvents = True
		Dim column = e.Column
		Dim dataRow = gvSearchhistory.GetRow(e.RowHandle)
		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, CVLSearchHistoryViewData)

			If viewData.ID.GetValueOrDefault(0) > 0 Then
				LoadAssignedCVLSearchHistoryResultData(viewData.ID.GetValueOrDefault(0))

				beiTplName.EditValue = viewData.QueryName
				bbiJobNotifyer.EditValue = viewData.Notify
			End If

		End If
		m_SuppressUIEvents = False

	End Sub



	Private Sub OngvJobGroups_KeyUp(sender As Object, e As KeyEventArgs) Handles gvJobGroups.KeyUp

		If e.KeyCode = Keys.Delete Then
			m_JobGroup.Remove(SelectedJobGroupRecord)
			grdopAreas.DataSource = m_JobGroup

			gvJobGroups.RefreshData()
		End If

	End Sub

	Private Sub OngvCompetences_KeyUp(sender As Object, e As KeyEventArgs) Handles gvCompetences.KeyUp

		If e.KeyCode = Keys.Delete Then
			m_CompentenceData.Remove(SelectedCompentenceRecord)
			grdSkills.DataSource = m_CompentenceData

			gvCompetences.RefreshData()
		End If

	End Sub

	Private Sub OngvLanguages_KeyUp(sender As Object, e As KeyEventArgs) Handles gvLanguages.KeyUp

		If e.KeyCode = Keys.Delete Then
			m_LanguageData.Remove(SelectedLanguageRecord)
			grdLanguages.DataSource = m_LanguageData

			gvLanguages.RefreshData()
		End If

	End Sub

	Private Sub OngvSearchhistory_KeyUp(sender As Object, e As KeyEventArgs) Handles gvSearchhistory.KeyUp

		If e.KeyCode = Keys.Delete Then
			Dim msg As String = String.Empty
			msg = "Hiermit wird die Such-History gelöscht. Sind Sie sicher?"

			If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg)) = False Then Return

			Dim searchData = SelectedSearchHistoryRecord

			If searchData Is Nothing Then Return

			Dim success = DeleteAssignedCVLSearchHistoryData(searchData.ID)

			If success Then
				msg = "Ihre Such-History wurde erfolgreich gelöscht."
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))

				LoadCVLSearchHistoryData()
			Else
				msg = "Ihre Such-History wurde nicht erfolgreich gelöscht."
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			End If

		End If

	End Sub

	Private Sub OngvSearchhistory_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles gvSearchhistory.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		m_SuppressUIEvents = True

		Dim searchData = SelectedSearchHistoryRecord

		If searchData Is Nothing Then Return
		If searchData.ID.GetValueOrDefault(0) > 0 Then
			LoadAssignedCVLSearchHistoryResultData(searchData.ID.GetValueOrDefault(0))
		End If
		m_SuppressUIEvents = False

	End Sub

	Private Sub grpBewerbung_CustomButtonClick(sender As Object, e As BaseButtonEventArgs) Handles grpBewerbung.CustomButtonClick
		Dim transiton As Transition = New Transition
		Dim manager As TransitionManager = New TransitionManager

		If e.Button.Properties.GroupIndex = 0 Then
			Dim oldSearchHistoryVisiblity As Boolean = pnlSearchhistory.Visible

			transiton.Control = pnlSearchhistory
			transiton.ShowWaitingIndicator = DefaultBoolean.False
			transiton.TransitionType = New SlideFadeTransition
			'Dim manager As TransitionManager = New TransitionManager
			manager.Transitions.Add(transiton)
			Dim r As Random = New Random

			manager.StartTransition(pnlSearchhistory)
			LoadCVLSearchHistoryData()
			pnlSearchhistory.Visible = Not oldSearchHistoryVisiblity

		ElseIf e.Button.Properties.GroupIndex = 1 Then
			'Dim transiton As Transition = New Transition
			Dim oldQualificationfieldVisiblity As Boolean = pnlQualificationField.Visible

			transiton.Control = pnlQualificationField
			transiton.ShowWaitingIndicator = DefaultBoolean.False
			transiton.TransitionType = New SlideFadeTransition
			'Dim manager As TransitionManager = New TransitionManager
			manager.Transitions.Add(transiton)
			Dim r As Random = New Random

			manager.StartTransition(pnlQualificationField)
			pnlQualificationField.Visible = Not oldQualificationfieldVisiblity

			'manager.EndTransition()
		End If

		manager.EndTransition()

	End Sub

	Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is BaseEdit Then
				If CType(sender, BaseEdit).Properties.ReadOnly Then
					' nothing
				Else
					CType(sender, BaseEdit).EditValue = Nothing
				End If
			End If
		End If

	End Sub

	Private Function LoadApplicantData(ByVal applicantID As Integer) As EmployeeMasterData

		Dim data = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(applicantID, False)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten-/ Bewerber-Daten konnten nicht geladen werden."))

			Return Nothing
		End If

		Return data

	End Function


#Region "helpers"


	'Private Sub SerializeArrayList(ByVal obj As ArrayList, ByVal FilePath As String)
	'	Dim doc As System.Xml.XmlDocument = New XmlDocument()
	'	Dim extraTypes As Type() = New Type(0) {}
	'	extraTypes(0) = GetType(CategoryInfo)
	'	Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(ArrayList)) ', extraTypes)
	'	Dim stream As New System.IO.MemoryStream()

	'	Try
	'		Dim myCon = New Container()
	'		Container.Add(obj)
	'		Dim serializer_1 = New XmlSerializer(TypeOf (obj))


	'	Catch ex As Exception

	'	End Try

	'	Try
	'		serializer.Serialize(stream, obj)
	'		stream.Position = 0
	'		doc.Load(stream)
	'		'  Return doc.InnerXml
	'		'save doc to file.
	'		SaveTextToFile(doc.InnerXml, FilePath)
	'	Catch ex As Exception

	'		Throw
	'	Finally
	'		stream.Close()
	'		stream.Dispose()
	'	End Try
	'End Sub

	Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

		Dim bAns As Boolean = False
		Dim objReader As StreamWriter
		Try
			objReader = New StreamWriter(FullPath)
			objReader.Write(strData)
			objReader.Close()
			bAns = True
		Catch Ex As Exception
			ErrInfo = Ex.Message
		End Try

		Return bAns
	End Function

	Private Function SaveToXml(Of T)(ByVal instance As T, ByVal filePath As String) As Boolean
		Dim objSerialize As System.Xml.Serialization.XmlSerializer
		Dim fs As System.IO.FileStream

		Try
			If instance Is Nothing Then
				Throw New ArgumentNullException("instance")
			End If

			objSerialize = New System.Xml.Serialization.XmlSerializer(instance.GetType())
			fs = New System.IO.FileStream(filePath, System.IO.FileMode.Create)
			objSerialize.Serialize(fs, instance)
			fs.Close()

			Return True

		Catch ex As Exception
			Throw
		End Try

	End Function



#End Region


	Private Class FormSerialisor


	End Class

End Class


''' <summary>
''' A generic class used to serialize objects.
''' </summary>
Public Class GenericSerializer

	''' <summary>
	''' Serializes the given object.
	''' </summary>
	''' <typeparam name="T">The type of the object to be serialized.</typeparam>
	''' <param name="obj">The object to be serialized.</param>
	''' <returns>String representation of the serialized object.</returns>
	Public Overloads Shared Function Serialize(Of T)(ByVal obj As T) As String
		Dim xs As XmlSerializer = Nothing
		Dim sw As StringWriter = Nothing
		Try
			xs = New XmlSerializer(GetType(T))
			sw = New StringWriter
			xs.Serialize(sw, obj)
			sw.Flush()
			Return sw.ToString
		Catch ex As Exception
			Throw ex
		Finally
			If (Not (sw) Is Nothing) Then
				sw.Close()
				sw.Dispose()
			End If

		End Try

	End Function

	Public Overloads Shared Function Serialize(Of T)(ByVal obj As T, ByVal extraTypes() As Type) As String
		Dim xs As XmlSerializer = Nothing
		Dim sw As StringWriter = Nothing
		Try
			xs = New XmlSerializer(GetType(T), extraTypes)
			sw = New StringWriter
			xs.Serialize(sw, obj)
			sw.Flush()
			Return sw.ToString
		Catch ex As Exception
			Throw ex
		Finally
			If (Not (sw) Is Nothing) Then
				sw.Close()
				sw.Dispose()
			End If

		End Try

	End Function

	''' <summary>
	''' Serializes the given object.
	''' </summary>
	''' <typeparam name="T">The type of the object to be serialized.</typeparam>
	''' <param name="obj">The object to be serialized.</param>
	''' <param name="writer">The writer to be used for output in the serialization.</param>
	Public Overloads Shared Sub Serialize(Of T)(ByVal obj As T, ByVal writer As XmlWriter)
		Dim xs As XmlSerializer = New XmlSerializer(GetType(T))
		xs.Serialize(writer, obj)
	End Sub

	''' <summary>
	''' Serializes the given object.
	''' </summary>
	''' <typeparam name="T">The type of the object to be serialized.</typeparam>
	''' <param name="obj">The object to be serialized.</param>
	''' <param name="writer">The writer to be used for output in the serialization.</param>
	''' <param name="extraTypes"><c>Type</c> array
	'''       of additional object types to serialize.</param>
	Public Overloads Shared Sub Serialize(Of T)(ByVal obj As T, ByVal writer As XmlWriter, ByVal extraTypes() As Type)
		Dim xs As XmlSerializer = New XmlSerializer(GetType(T), extraTypes)
		xs.Serialize(writer, obj)
	End Sub

	''' <summary>
	''' Deserializes the given object.
	''' </summary>
	''' <typeparam name="T">The type of the object to be deserialized.</typeparam>
	''' <param name="reader">The reader used to retrieve the serialized object.</param>
	''' <returns>The deserialized object of type T.</returns>
	Public Overloads Shared Function Deserialize(Of T)(ByVal reader As XmlReader) As T
		Dim xs As XmlSerializer = New XmlSerializer(GetType(T))
		Return CType(xs.Deserialize(reader), T)
	End Function

	''' <summary>
	''' Deserializes the given object.
	''' </summary>
	''' <typeparam name="T">The type of the object to be deserialized.</typeparam>
	''' <param name="reader">The reader used to retrieve the serialized object.</param>
	''' <param name="extraTypes"><c>Type</c> array
	'''           of additional object types to deserialize.</param>
	''' <returns>The deserialized object of type T.</returns>
	Public Overloads Shared Function Deserialize(Of T)(ByVal reader As XmlReader, ByVal extraTypes() As Type) As T
		Dim xs As XmlSerializer = New XmlSerializer(GetType(T), extraTypes)
		Return CType(xs.Deserialize(reader), T)
	End Function

	''' <summary>
	''' Deserializes the given object.
	''' </summary>
	''' <typeparam name="T">The type of the object to be deserialized.</typeparam>
	''' <param name="XML">The XML file containing the serialized object.</param>
	''' <returns>The deserialized object of type T.</returns>
	Public Overloads Shared Function Deserialize(Of T)(ByVal XML As String) As T
		If ((XML Is Nothing) OrElse (XML = String.Empty)) Then
			Return Nothing
		End If

		Dim xs As XmlSerializer = Nothing
		Dim sr As StringReader = Nothing
		Try
			xs = New XmlSerializer(GetType(T))
			sr = New StringReader(XML)
			Return CType(xs.Deserialize(sr), T)
		Catch ex As Exception
			Throw ex
		Finally
			If (Not (sr) Is Nothing) Then
				sr.Close()
				sr.Dispose()
			End If

		End Try

	End Function

	Public Overloads Shared Function Deserialize(Of T)(ByVal XML As String, ByVal extraTypes() As Type) As T
		If ((XML Is Nothing) _
										OrElse (XML = String.Empty)) Then
			Return Nothing
		End If

		Dim xs As XmlSerializer = Nothing
		Dim sr As StringReader = Nothing
		Try
			xs = New XmlSerializer(GetType(T), extraTypes)
			sr = New StringReader(XML)
			Return CType(xs.Deserialize(sr), T)
		Catch ex As Exception
			Throw ex
		Finally
			If (Not (sr) Is Nothing) Then
				sr.Close()
				sr.Dispose()
			End If

		End Try

	End Function

	Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String, ByVal encoding As Encoding, ByVal extraTypes() As Type)
		If File.Exists(FileName) Then
			File.Delete(FileName)
		End If

		Dim di As DirectoryInfo = New DirectoryInfo(Path.GetDirectoryName(FileName))
		If Not di.Exists Then
			di.Create()
		End If

		Dim document As XmlDocument = New XmlDocument
		Dim wSettings As XmlWriterSettings = New XmlWriterSettings
		wSettings.Indent = True
		wSettings.Encoding = encoding
		wSettings.CloseOutput = True
		wSettings.CheckCharacters = False
		Dim writer As XmlWriter = XmlWriter.Create(FileName, wSettings)
		If (Not (extraTypes) Is Nothing) Then
			GenericSerializer.Serialize(Of T)(Obj, writer, extraTypes)
		Else
			GenericSerializer.Serialize(Of T)(Obj, writer)
		End If

		writer.Flush()
		document.Save(writer)
	End Sub

	Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String, ByVal extraTypes() As Type)
		GenericSerializer.SaveAs(Of T)(Obj, FileName, Encoding.UTF8, extraTypes)
	End Sub

	Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String, ByVal encoding As Encoding)
		GenericSerializer.SaveAs(Of T)(Obj, FileName, encoding, Nothing)
	End Sub

	Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String)
		GenericSerializer.SaveAs(Of T)(Obj, FileName, Encoding.UTF8)
	End Sub

	Public Overloads Shared Function Open(Of T)(ByVal FileName As String, ByVal extraTypes() As Type) As T
		Dim obj As T
		If File.Exists(FileName) Then
			Dim rSettings As XmlReaderSettings = New XmlReaderSettings
			rSettings.CloseInput = True
			rSettings.CheckCharacters = False
			Dim reader As XmlReader = XmlReader.Create(FileName, rSettings)
			reader.ReadOuterXml()

			If (Not (extraTypes) Is Nothing) Then
				obj = GenericSerializer.Deserialize(Of T)(reader, extraTypes)
			Else
				obj = GenericSerializer.Deserialize(Of T)(reader)
			End If

		End If

		Return obj
	End Function

	Public Overloads Shared Function Open(Of T)(ByVal FileName As String) As T
		Return GenericSerializer.Open(Of T)(FileName, Nothing)
	End Function
End Class