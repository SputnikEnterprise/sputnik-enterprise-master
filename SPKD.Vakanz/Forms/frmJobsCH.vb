
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Controls

Imports SPKD.Vakanz.Settings
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports SPKD.Vakanz.ClsDataDetail
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports SP.DatabaseAccess.Vacancy
Imports SP.Internal.Automations
Imports SPProgUtility.CommonXmlUtility
Imports System.Text
Imports System.IO
'Imports Html2Markdown.Converter
'Imports Newtonsoft.Json

Public Class frmJobsCH

	Public Delegate Sub CreditInfoDataSavedHandler()
	Public Event CreditInfoDataSaved As CreditInfoDataSavedHandler

#Region "private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_EXPORT_SETTING As String = "MD_{0}/Export"

#End Region


#Region "private fields"

	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_InitializationChangedData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_SuppressUIEvents As Boolean = False

	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess

	Protected m_UtilityUI As UtilityUI
	Protected m_Utility As Utility

	Private m_mandant As Mandant
	Private m_path As ClsProgPath
	Private m_common As CommonSetting
	Private m_SettingsManager As SettingsManager

	Private m_recID As Integer?
	Private m_ExportSetting As String

	Private m_JobCHMasterData As VacancyJobCHMasterData
	Private m_OstJobMasterData As VacancyOstJobMasterData
	Private m_connectionString As String
	Private m_currentVacancyNumber As Integer
	Private m_VacancySetting As ClsVakSetting
	Private m_LanguageForPlattform As ExternalPlattforms
	Private m_MandantSettingsXml As SettingsXml

	Private m_AVAMRestricted As Boolean


	''' <summary>
	''' The translate values.
	''' </summary>
	Private m_AllowedVacancy2WOS As Boolean
	Private m_AllowedVacancyJobChannelPriority As Boolean

#End Region


#Region "private properties"
	Private Property IsInternSaved As UploadResult
	Private Property IsJobCHSaved As UploadResult
	Private Property IsOstJobSaved As UploadResult

#End Region


#Region "public properties"

	Public Property VacancyAdvisorNumber As Integer
	Public Property SBNNumber As Integer?
	Public Property SBNPublicationState As Integer?
	Public Property VacancySettingData As ClsVakSetting
	Public Property CurrentVacancyData As VacancyMasterData

#End Region



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _changedSetting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_InitializationChangedData = _changedSetting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_mandant = New Mandant
		m_common = New CommonSetting
		m_path = New ClsProgPath
		m_SettingsManager = New SettingsManager

		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		m_VacancySetting = New ClsVakSetting
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationChangedData.MDData.MDNr, Now.Year))
		m_ExportSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_EXPORT_SETTING, m_InitializationChangedData.MDData.MDNr)


		m_SuppressUIEvents = True

		InitializeComponent()

		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		TranslateControls()
		Reset()


		AddHandler lueBeruf_1.ButtonClick, AddressOf ResetBeruf_1
		AddHandler lueBeruf_2.ButtonClick, AddressOf ResetBeruf_2
		AddHandler lueErfahrung_1.ButtonClick, AddressOf ResetFachBereich_1
		AddHandler lueErfahrung_2.ButtonClick, AddressOf ResetFachBereich_2

		AddHandler lueBEPosition_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueBEPosition_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePosition.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueBNiveau_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueBNiveau_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueBNiveau_3.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueBranche.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueRegion_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueRegion_2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCanton.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueGender.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueAVAMEducation.ButtonClick, AddressOf OnDropDown_ButtonClick


	End Sub

#End Region


#Region "Public Methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		ReInitialData()

		success = success AndAlso LoadOccupationDropDownData()
		'success = success AndAlso LoadAllExperienceDropDownData()
		success = success AndAlso LoadPositionDropDownData()
		success = success AndAlso LoadBildungsniveauDropDownData()
		success = success AndAlso LoadBrancheDropDownData()
		success = success AndAlso LoadJobCHLanguageDropDownData()
		success = success AndAlso LoadRegionDropDownData()

		LoadCantonDropDownData()
		LoadGenderDropDownData()
		LoadAVAMEducationDropDownData()

		success = success AndAlso LoadJobCHMasterData()
		success = success AndAlso LoadOstJobMasterData()

		success = success AndAlso LoadJobCHVacancyDetails()
		success = success AndAlso LoadOstJobDetails()
		success = success AndAlso LoadAVAMVacancyData()

		m_SuppressUIEvents = False

		Return success
	End Function

#End Region

#Region "private properties"

	Private ReadOnly Property SelectedJobCHLanguageViewData() As VacancyJobCHLanguageData
		Get
			Dim data = TryCast(lstJobCHLanguages.SelectedItem, VacancyJobCHLanguageData)

			Return data
		End Get

	End Property

	Private ReadOnly Property SelectedAVAMLanguageViewData() As VacancyJobCHLanguageData
		Get
			Dim data = TryCast(lstAVAMLanguages.SelectedItem, VacancyJobCHLanguageData)

			Return data
		End Get

	End Property

	Private ReadOnly Property AllowedVacancyJobChannelPriority As Boolean
		Get
			Dim value As Boolean = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/jobchannelpriority", m_ExportSetting)), False)

			Return value
		End Get
	End Property

#End Region

	Sub ReInitialData()

		m_currentVacancyNumber = CurrentVacancyData.VakNr

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_AllowedVacancy2WOS = m_mandant.AllowedExportVacancy2WOS(m_InitializationData.MDData.MDNr, Now.Year)
		m_AllowedVacancyJobChannelPriority = AllowedVacancyJobChannelPriority

		m_VacancySetting = VacancySettingData

		sbIsInternOnline.Enabled = m_AllowedVacancy2WOS
		sbIsJCHOnline.Enabled = m_VacancySetting.IsAllowedJCH
		chkJobChannelPriority.Enabled = m_AllowedVacancyJobChannelPriority
		sbIsostjobOnline.Enabled = m_VacancySetting.IsAllowedOstJob

		xtabostjob.PageEnabled = m_VacancySetting.IsAllowedOstJob
		grpBeruf.Enabled = m_VacancySetting.IsAllowedJCH
		grpBildung.Enabled = m_VacancySetting.IsAllowedJCH
		pnlMain.Enabled = IsUserActionAllowed(m_InitialChangedData.UserData.UserNr, 702)

	End Sub


	Private Sub Reset()

		Me.txtShortDescription.Properties.MaxLength = 150

		ResetBerufDropDown()
		ResetErfahrungDropDown()
		ResetBEPositionDropDown()
		ResetPositionDropDown()

		ResetBildungsNiveauDropDown()
		ResetBrancheDropDown()

		ResetLanguageNameDropDown()
		ResetLanguageNiveauDropDown()

		ResetCantonDropDown()
		ResetRegionDropDown()
		ResetGenderDropDown()

		ResetAVAMEducationDropDown()

	End Sub

#Region "Reset"

	Private Sub ResetBerufDropDown()

		lueBeruf_1.Properties.DisplayMember = "TranslatedLabel"
		lueBeruf_1.Properties.ValueMember = "ID"

		Dim columns1 = lueBeruf_1.Properties.Columns
		columns1.Clear()
		columns1.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBeruf_1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBeruf_1.Properties.SearchMode = SearchMode.AutoComplete
		lueBeruf_1.Properties.AutoSearchColumnIndex = 0
		lueBeruf_1.Properties.NullText = String.Empty
		lueBeruf_1.EditValue = Nothing


		lueBeruf_2.Properties.DisplayMember = "TranslatedLabel"
		lueBeruf_2.Properties.ValueMember = "ID"

		Dim columns2 = lueBeruf_2.Properties.Columns
		columns2.Clear()
		columns2.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBeruf_2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBeruf_2.Properties.SearchMode = SearchMode.AutoComplete
		lueBeruf_2.Properties.AutoSearchColumnIndex = 0
		lueBeruf_2.Properties.NullText = String.Empty
		lueBeruf_2.EditValue = Nothing

	End Sub

	Private Sub ResetErfahrungDropDown()

		lueErfahrung_1.Properties.DisplayMember = "TranslatedLabel"
		lueErfahrung_1.Properties.ValueMember = "ID"

		Dim columns1 = lueErfahrung_1.Properties.Columns
		columns1.Clear()
		columns1.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueErfahrung_1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueErfahrung_1.Properties.SearchMode = SearchMode.AutoComplete
		lueErfahrung_1.Properties.AutoSearchColumnIndex = 0
		lueErfahrung_1.Properties.NullText = String.Empty
		lueErfahrung_1.EditValue = Nothing


		lueErfahrung_2.Properties.DisplayMember = "TranslatedLabel"
		lueErfahrung_2.Properties.ValueMember = "ID"

		Dim columns2 = lueErfahrung_2.Properties.Columns
		columns2.Clear()
		columns2.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueErfahrung_2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueErfahrung_2.Properties.SearchMode = SearchMode.AutoComplete
		lueErfahrung_2.Properties.AutoSearchColumnIndex = 0
		lueErfahrung_2.Properties.NullText = String.Empty
		lueErfahrung_2.EditValue = Nothing

	End Sub

	Private Sub ResetBEPositionDropDown()

		lueBEPosition_1.Properties.DisplayMember = "TranslatedLabel"
		lueBEPosition_1.Properties.ValueMember = "ID"

		Dim columns1 = lueBEPosition_1.Properties.Columns
		columns1.Clear()
		columns1.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBEPosition_1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBEPosition_1.Properties.SearchMode = SearchMode.AutoComplete
		lueBEPosition_1.Properties.AutoSearchColumnIndex = 0
		lueBEPosition_1.Properties.NullText = String.Empty
		lueBEPosition_1.EditValue = Nothing


		lueBEPosition_2.Properties.DisplayMember = "TranslatedLabel"
		lueBEPosition_2.Properties.ValueMember = "ID"

		Dim columns2 = lueBEPosition_2.Properties.Columns
		columns2.Clear()
		columns2.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBEPosition_2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBEPosition_2.Properties.SearchMode = SearchMode.AutoComplete
		lueBEPosition_2.Properties.AutoSearchColumnIndex = 0
		lueBEPosition_2.Properties.NullText = String.Empty
		lueBEPosition_2.EditValue = Nothing

	End Sub

	Private Sub ResetPositionDropDown()

		luePosition.Properties.DisplayMember = "TranslatedLabel"
		luePosition.Properties.ValueMember = "ID"

		Dim columns = luePosition.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		luePosition.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePosition.Properties.SearchMode = SearchMode.AutoComplete
		luePosition.Properties.AutoSearchColumnIndex = 0
		luePosition.Properties.NullText = String.Empty
		luePosition.EditValue = Nothing

	End Sub

	Private Sub ResetBildungsNiveauDropDown()

		lueBNiveau_1.Properties.DisplayMember = "TranslatedLabel"
		lueBNiveau_1.Properties.ValueMember = "ID"

		Dim columns1 = lueBNiveau_1.Properties.Columns
		columns1.Clear()
		columns1.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBNiveau_1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBNiveau_1.Properties.SearchMode = SearchMode.AutoComplete
		lueBNiveau_1.Properties.AutoSearchColumnIndex = 0
		lueBNiveau_1.Properties.NullText = String.Empty
		lueBNiveau_1.EditValue = Nothing


		lueBNiveau_2.Properties.DisplayMember = "TranslatedLabel"
		lueBNiveau_2.Properties.ValueMember = "ID"

		Dim columns2 = lueBNiveau_2.Properties.Columns
		columns2.Clear()
		columns2.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBNiveau_2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBNiveau_2.Properties.SearchMode = SearchMode.AutoComplete
		lueBNiveau_2.Properties.AutoSearchColumnIndex = 0
		lueBNiveau_2.Properties.NullText = String.Empty
		lueBNiveau_2.EditValue = Nothing

		lueBNiveau_3.Properties.DisplayMember = "TranslatedLabel"
		lueBNiveau_3.Properties.ValueMember = "ID"

		Dim columns3 = lueBNiveau_3.Properties.Columns
		columns3.Clear()
		columns3.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBNiveau_3.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBNiveau_3.Properties.SearchMode = SearchMode.AutoComplete
		lueBNiveau_3.Properties.AutoSearchColumnIndex = 0
		lueBNiveau_3.Properties.NullText = String.Empty
		lueBNiveau_3.EditValue = Nothing

	End Sub

	Private Sub ResetBrancheDropDown()

		lueBranche.Properties.DisplayMember = "TranslatedLabel"
		lueBranche.Properties.ValueMember = "ID"

		Dim columns = lueBranche.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBranche.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBranche.Properties.SearchMode = SearchMode.AutoComplete
		lueBranche.Properties.AutoSearchColumnIndex = 0
		lueBranche.Properties.NullText = String.Empty
		lueBranche.EditValue = Nothing

	End Sub

	Private Sub ResetLanguageNameDropDown()

		lueLanguageName.Properties.DisplayMember = "TranslatedLabel"
		lueLanguageName.Properties.ValueMember = "ID"

		Dim columns = lueLanguageName.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Sprache")))

		lueLanguageName.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLanguageName.Properties.SearchMode = SearchMode.AutoComplete
		lueLanguageName.Properties.AutoSearchColumnIndex = 0
		lueLanguageName.Properties.NullText = String.Empty
		lueLanguageName.EditValue = Nothing

	End Sub

	Private Sub ResetLanguageNiveauDropDown()

		lueLanguageNiveau.Properties.DisplayMember = "TranslatedLabel"
		lueLanguageNiveau.Properties.ValueMember = "ID"

		Dim columns = lueLanguageNiveau.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Niveau")))

		lueLanguageNiveau.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLanguageNiveau.Properties.SearchMode = SearchMode.AutoComplete
		lueLanguageNiveau.Properties.AutoSearchColumnIndex = 0
		lueLanguageNiveau.Properties.NullText = String.Empty
		lueLanguageNiveau.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the canton drop down.
	''' </summary>
	Private Sub ResetCantonDropDown()

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

	Private Sub ResetRegionDropDown()

		lueRegion_1.Properties.DisplayMember = "TranslatedLabel"
		lueRegion_1.Properties.ValueMember = "ID"

		Dim columns = lueRegion_1.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueRegion_1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueRegion_1.Properties.SearchMode = SearchMode.AutoComplete
		lueRegion_1.Properties.AutoSearchColumnIndex = 0
		lueRegion_1.Properties.NullText = String.Empty
		lueRegion_1.EditValue = Nothing

		lueRegion_2.Properties.DisplayMember = "TranslatedLabel"
		lueRegion_2.Properties.ValueMember = "ID"

		Dim columns2 = lueRegion_2.Properties.Columns
		columns2.Clear()
		columns2.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueRegion_2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueRegion_2.Properties.SearchMode = SearchMode.AutoComplete
		lueRegion_2.Properties.AutoSearchColumnIndex = 0
		lueRegion_2.Properties.NullText = String.Empty
		lueRegion_2.EditValue = Nothing

	End Sub


	''' <summary>
	''' Resets the gender drop down.
	''' </summary>
	Private Sub ResetGenderDropDown()

		lueGender.Properties.ShowHeader = False
		lueGender.Properties.ShowFooter = False
		lueGender.Properties.DropDownRows = 10

		lueGender.Properties.DisplayMember = "TranslatedGender"
		lueGender.Properties.ValueMember = "RecValue"

		Dim columns = lueGender.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedGender", 0, m_Translate.GetSafeTranslationValue("Geschlecht")))

		lueGender.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGender.Properties.SearchMode = SearchMode.AutoComplete
		lueGender.Properties.AutoSearchColumnIndex = 0

		lueGender.Properties.NullText = String.Empty
		lueGender.EditValue = Nothing

	End Sub

	Private Sub ResetAVAMEducationDropDown()

		lueAVAMEducation.Properties.DisplayMember = "TranslatedLabel"
		lueAVAMEducation.Properties.ValueMember = "RecNr"

		Dim columns1 = lueAVAMEducation.Properties.Columns
		columns1.Clear()
		columns1.Add(New LookUpColumnInfo("TranslatedLabel", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueAVAMEducation.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAVAMEducation.Properties.SearchMode = SearchMode.AutoComplete
		lueAVAMEducation.Properties.AutoSearchColumnIndex = 0
		lueAVAMEducation.Properties.NullText = String.Empty
		lueAVAMEducation.EditValue = Nothing

	End Sub

#End Region

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			xtabjobsch.Text = m_Translate.GetSafeTranslationValue(xtabjobsch.Text)

			btnSave.Text = m_Translate.GetSafeTranslationValue(btnSave.Text)
			btnJCHSetting.Text = m_Translate.GetSafeTranslationValue(btnJCHSetting.Text)
			sbIsInternOnline.OnText = m_Translate.GetSafeTranslationValue(sbIsInternOnline.OnText)
			chkJobChannelPriority.Text = m_Translate.GetSafeTranslationValue(chkJobChannelPriority.Text)
			sbIsInternOnline.OffText = m_Translate.GetSafeTranslationValue(sbIsInternOnline.OffText)

			sbIsJCHOnline.OnText = m_Translate.GetSafeTranslationValue(sbIsJCHOnline.OnText)
			sbIsJCHOnline.OffText = m_Translate.GetSafeTranslationValue(sbIsJCHOnline.OffText)

			grpBeruf.Text = m_Translate.GetSafeTranslationValue(grpBeruf.Text)
			grpBildung.Text = m_Translate.GetSafeTranslationValue(grpBildung.Text)

			grpAntritt.Text = m_Translate.GetSafeTranslationValue(grpAntritt.Text)
			lblKanton.Text = m_Translate.GetSafeTranslationValue(lblKanton.Text)
			lblRegion.Text = m_Translate.GetSafeTranslationValue(lblRegion.Text)
			chkAnstellung_1.Text = m_Translate.GetSafeTranslationValue(chkAnstellung_1.Text)
			chkAnstellung_2.Text = m_Translate.GetSafeTranslationValue(chkAnstellung_2.Text)
			chkAnstellung_3.Text = m_Translate.GetSafeTranslationValue(chkAnstellung_3.Text)
			chkAnstellung_4.Text = m_Translate.GetSafeTranslationValue(chkAnstellung_4.Text)
			chkAnstellung_5.Text = m_Translate.GetSafeTranslationValue(chkAnstellung_5.Text)
			chkAnstellung_6.Text = m_Translate.GetSafeTranslationValue(chkAnstellung_6.Text)
			lblAnstellungsart.Text = m_Translate.GetSafeTranslationValue(lblAnstellungsart.Text)
			lblArbeitspensum.Text = m_Translate.GetSafeTranslationValue(lblArbeitspensum.Text)
			lblAntrittper.Text = m_Translate.GetSafeTranslationValue(lblAntrittper.Text)
			lblDauer.Text = m_Translate.GetSafeTranslationValue(lblDauer.Text)
			lblAlter.Text = m_Translate.GetSafeTranslationValue(lblAlter.Text)
			lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(lblGeschlecht.Text)
			chkAlter_1.Text = m_Translate.GetSafeTranslationValue(chkAlter_1.Text)
			chkAlter_2.Text = m_Translate.GetSafeTranslationValue(chkAlter_2.Text)
			chkAlter_3.Text = m_Translate.GetSafeTranslationValue(chkAlter_3.Text)
			chkAlter_4.Text = m_Translate.GetSafeTranslationValue(chkAlter_4.Text)

			grpposition.Text = m_Translate.GetSafeTranslationValue(grpposition.Text)
			grpBranche.Text = m_Translate.GetSafeTranslationValue(grpBranche.Text)
			grpSprache.Text = m_Translate.GetSafeTranslationValue(grpSprache.Text)
			lblSprache.Text = m_Translate.GetSafeTranslationValue(lblSprache.Text)
			lblNiveau.Text = m_Translate.GetSafeTranslationValue(lblNiveau.Text)
			btnSaveLang.Text = m_Translate.GetSafeTranslationValue(btnSaveLang.Text)

			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try
		Text = m_Translate.GetSafeTranslationValue(Text)

	End Sub

	Private Sub frm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_JCH_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_JCH_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_VACANCIES_FORM_JCH_LOCATION)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	''' <summary>
	''' Loads the canton drop downdata.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadCantonDropDownData() As Boolean
		Dim cantonData = m_CommonDatabaseAccess.LoadCantonData()

		If (cantonData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kantondaten konnten nicht geladen werden."))
		End If

		lueCanton.Properties.DataSource = cantonData
		lueCanton.Properties.ForceInitialize()

		Return Not cantonData Is Nothing
	End Function

	''' <summary>
	''' Loads gender drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadGenderDropDownData() As Boolean
		Dim genderData = m_CommonDatabaseAccess.LoadGenderData()

		If (genderData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Geschlechtsdaten konnten nicht geladen werden."))
		End If

		lueGender.Properties.DataSource = genderData
		lueGender.Properties.ForceInitialize()

		Return Not genderData Is Nothing
	End Function

	Private Function LoadOccupationDropDownData() As Boolean
		' Load data
		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHOccupationList(m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruf Daten konnten nicht geladen werden."))

			Return False
		End If

		lueBeruf_1.Properties.DataSource = data
		lueBeruf_2.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

	'Private Function LoadAllExperienceDropDownData() As Boolean
	'	' Load data
	'	Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
	'	Dim data = InernUploader.GetJobCHOccupationExperienceList(Nothing, m_InitializationData.UserData.UserLanguage)
	'	If data Is Nothing Then
	'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruferfahrung Daten konnten nicht geladen werden."))

	'		Return False
	'	End If

	'	lueErfahrung_1.Properties.DataSource = data
	'	lueErfahrung_2.Properties.DataSource = data

	'	Return Not data Is Nothing
	'End Function

	Private Function LoadFirstExperienceDropDownData(ByVal parentID As Integer?) As Boolean
		' Load data
		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHOccupationExperienceList(parentID.GetValueOrDefault(0), m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruferfahrung Daten konnten nicht geladen werden."))

			Return False
		End If

		lueErfahrung_1.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

	Private Function LoadSecondExperienceDropDownData(ByVal parentID As Integer?) As Boolean
		' Load data
		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHOccupationExperienceList(parentID.GetValueOrDefault(0), m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruferfahrung Daten konnten nicht geladen werden."))

			Return False
		End If

		lueErfahrung_2.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

	Private Function LoadPositionDropDownData() As Boolean
		' Load data
		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHPositionList(m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Position Daten konnten nicht geladen werden."))

			Return False
		End If

		lueBEPosition_1.Properties.DataSource = data
		lueBEPosition_2.Properties.DataSource = data

		luePosition.Properties.DataSource = data

		Return Not data Is Nothing
	End Function

	Private Function LoadBildungsniveauDropDownData() As Boolean

		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHBildungsniveauList(m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bildungsniveau Daten konnten nicht geladen werden."))

			Return False
		End If

		lueBNiveau_1.Properties.DataSource = data
		lueBNiveau_2.Properties.DataSource = data
		lueBNiveau_3.Properties.DataSource = data


		Return Not data Is Nothing
	End Function

	Private Function LoadBrancheDropDownData() As Boolean
		' Load data
		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHBrunchesList(m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Branchen Daten konnten nicht geladen werden."))

			Return False
		End If

		lueBranche.Properties.DataSource = data


		Return Not data Is Nothing
	End Function

	Private Function LoadJobCHLanguageDropDownData() As Boolean

		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim dataName = InernUploader.GetJobCHLanguageNameList(m_InitializationData.UserData.UserLanguage)
		If dataName Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(Jobs.CH)Sprach Daten konnten nicht geladen werden."))

			Return False
		End If

		lueLanguageName.Properties.DataSource = dataName

		Dim dataLevel = InernUploader.GetJobCHLanguageNiveauList(m_InitializationData.UserData.UserLanguage)
		If dataLevel Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(Jobs.CH)Sprachniveau Daten konnten nicht geladen werden."))

			Return False
		End If

		lueLanguageNiveau.Properties.DataSource = dataLevel


		Return Not dataName Is Nothing
	End Function

	Private Function LoadRegionDropDownData() As Boolean
		' Load data
		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetJobCHRegionList(m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Region Daten konnten nicht geladen werden."))

			Return False
		End If

		lueRegion_1.Properties.DataSource = data
		lueRegion_2.Properties.DataSource = data


		Return Not data Is Nothing
	End Function

	Private Function LoadAVAMLanguageDropDownData() As Boolean

		Dim data = New List(Of VacancyJobCHPeripheryViewData) From {
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Deutsch"), .ID = 1},
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Französisch"), .ID = 2},
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Italienisch"), .ID = 4},
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Englisch"), .ID = 3}
		}
		lueLanguageName.Properties.DataSource = data
		lueLanguageName.Properties.ForceInitialize()


		Dim dataLevel = New List(Of VacancyJobCHPeripheryViewData) From {
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Keine"), .ID = 1},
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Basic"), .ID = 2},
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Mittel"), .ID = 3},
			New VacancyJobCHPeripheryViewData With {.TranslatedLabel = m_Translate.GetSafeTranslationValue("Professionell"), .ID = 4}
		}
		lueLanguageNiveau.Properties.DataSource = dataLevel
		lueLanguageNiveau.Properties.ForceInitialize()


		Return Not data Is Nothing
	End Function

	Private Function LoadAVAMEducationDropDownData() As Boolean

		Dim InernUploader As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)
		Dim data = InernUploader.GetAVAMEducationList(m_InitializationData.UserData.UserLanguage)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(AVAM) Bildung Daten konnten nicht geladen werden."))

			Return False
		End If

		lueAVAMEducation.Properties.DataSource = data


		Return Not data Is Nothing
	End Function

	Private Function LoadJobCHVacancyDetails() As Boolean
		Dim success As Boolean = True

		Try
			txtTitelforSearch.EditValue = m_JobCHMasterData.TitelForSearch
			txtShortDescription.EditValue = m_JobCHMasterData.ShortDescription

			Dim berufData = LoadAssignedBerufData()
			If Not berufData Is Nothing Then
				If berufData.Count > 0 Then
					lueBeruf_1.EditValue = berufData(0).BerufGruppe_Value
					If LoadFirstExperienceDropDownData(berufData(0).BerufGruppe_Value) Then
						lueErfahrung_1.EditValue = berufData(0).Fachrichtung_Value
						lueBEPosition_1.EditValue = berufData(0).Position_Value
					End If

					If berufData.Count > 1 Then
						lueBeruf_2.EditValue = berufData(1).BerufGruppe_Value
						If LoadSecondExperienceDropDownData(berufData(1).BerufGruppe_Value) Then
							lueErfahrung_2.EditValue = berufData(1).Fachrichtung_Value
							lueBEPosition_2.EditValue = berufData(1).Position_Value
						End If
					End If

				End If
			End If

			luePosition.EditValue = m_JobCHMasterData.Position_Value

			Dim bildNiveauData = LoadAssignedBildungsNiveauData()
			If Not bildNiveauData Is Nothing Then
				If bildNiveauData.Count > 0 Then lueBNiveau_1.EditValue = bildNiveauData(0).Bez_Value
				If bildNiveauData.Count > 1 Then lueBNiveau_2.EditValue = bildNiveauData(1).Bez_Value
				If bildNiveauData.Count > 2 Then lueBNiveau_3.EditValue = bildNiveauData(2).Bez_Value
			End If
			lueBranche.EditValue = m_JobCHMasterData.BranchenValue

			lueCanton.EditValue = m_JobCHMasterData.Vak_Kanton
			Dim regionData = LoadAssignedRegionData()
			If Not regionData Is Nothing Then
				If regionData.Count > 0 Then lueRegion_1.EditValue = regionData(0).Bez_Value
				If regionData.Count > 1 Then lueRegion_2.EditValue = regionData(1).Bez_Value
			End If

			sbIsJCHOnline.Value = m_JobCHMasterData.IsOnline
			sbIsInternOnline.Value = m_JobCHMasterData.IEExport
			chkJobChannelPriority.Checked = m_JobCHMasterData.JobChannelPriority.GetValueOrDefault(False)
			txt_Antrittper.EditValue = m_JobCHMasterData.Beginn

			Dim jobProzent As String = m_JobCHMasterData.JobProzent
			Dim aValue As String()
			If Not String.IsNullOrWhiteSpace(jobProzent) Then
				aValue = jobProzent.Split(CChar("#"))
				For i = 0 To aValue.Length - 1
					If i = 0 Then seArbpensum_Von.Value = Val(aValue(i)) Else seArbpensum_Bis.Value = Val(aValue(i))
				Next
			End If

			If Val(seArbpensum_Von.EditValue) = 0 Then seArbpensum_Von.EditValue = 100
			If Val(seArbpensum_Bis.EditValue) = 0 Then seArbpensum_Bis.EditValue = 100

			Dim anstellung As String = m_JobCHMasterData.Anstellung
			If Not String.IsNullOrWhiteSpace(anstellung) Then
				aValue = anstellung.Split(CChar("#"))
				For i = 0 To aValue.Length - 1
					If Val(aValue(i)) = 1 Then
						chkAnstellung_1.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 2 Then
						chkAnstellung_2.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 3 Then
						chkAnstellung_3.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 4 Then
						chkAnstellung_4.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 5 Then
						chkAnstellung_5.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 6 Then
						chkAnstellung_6.CheckState = CheckState.Checked
					End If
				Next
			End If
			txt_Dauer.Text = m_JobCHMasterData.Dauer

			Dim employeeAge As String = m_JobCHMasterData.MAAge
			If Not String.IsNullOrWhiteSpace(employeeAge) Then
				aValue = employeeAge.Split(CChar("#"))
				For i = 0 To aValue.Length - 1
					If Val(aValue(i)) = 1 Then
						chkAlter_1.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 2 Then
						chkAlter_2.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 3 Then
						chkAlter_3.CheckState = CheckState.Checked
					ElseIf Val(aValue(i)) = 4 Then
						chkAlter_4.CheckState = CheckState.Checked
					End If
				Next
			End If

			lueGender.EditValue = m_JobCHMasterData.MASex

			Dim languageData = LoadAssignedJobCHLanguageData()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try


		Return success
	End Function

	Private Function LoadJobCHMasterData() As Boolean
		m_JobCHMasterData = m_VacancyDatabaseAccess.LoadJobCHMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)

		If (m_JobCHMasterData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanz-Daten (JOBS.CH) konnten nicht geladen werden."))

			Return False
		End If

		Return Not m_JobCHMasterData Is Nothing
	End Function

	Private Function LoadOstJobMasterData() As Boolean

		m_OstJobMasterData = m_VacancyDatabaseAccess.LoadOstJobMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserFullName, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		If m_OstJobMasterData.VakNr Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die ostjob.ch Daten wurden nicht geladen."))

			Return False
		End If

		Return Not m_OstJobMasterData Is Nothing
	End Function

	Private Function LoadAssignedBerufData() As IEnumerable(Of VacancyJobCHBerufData)
		Dim result = m_VacancyDatabaseAccess.LoadJobCHBerufData(m_currentVacancyNumber)

		If (result Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruf / Erfahrung / Position Daten konnten nicht geladen werden."))

			Return Nothing
		End If

		Return result
	End Function

	Private Function LoadAssignedBildungsNiveauData() As IEnumerable(Of VacancyJobCHPeripheryData)
		Dim result = m_VacancyDatabaseAccess.LoadJobCHBildungsNiveauData(m_currentVacancyNumber)

		If (result Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bildungsniveau Daten konnten nicht geladen werden."))

			Return Nothing
		End If

		Return result
	End Function

	Private Function LoadAssignedRegionData() As IEnumerable(Of VacancyJobCHPeripheryData)
		Dim result = m_VacancyDatabaseAccess.LoadJobCHRegionData(m_currentVacancyNumber)

		If (result Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Region Daten konnten nicht geladen werden."))

			Return Nothing
		End If

		Return result
	End Function

	Private Function LoadAssignedJobCHLanguageData() As Boolean

		Dim jobCHLanguageData = m_VacancyDatabaseAccess.LoadVacancyLanguageData(m_currentVacancyNumber, ExternalPlattforms.JOBSCH)

		lstJobCHLanguages.DisplayMember = "LanguageViewData"
		lstJobCHLanguages.ValueMember = "ID"

		If (jobCHLanguageData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprach Daten konnten nicht geladen werden."))
			Return False
		End If

		lstJobCHLanguages.DataSource = jobCHLanguageData


		Return True

	End Function

	Private Function LoadAssignedAVAMLanguageData() As Boolean

		Dim avamLanguageData = m_VacancyDatabaseAccess.LoadVacancyLanguageData(m_currentVacancyNumber, ExternalPlattforms.AVAM)

		lstAVAMLanguages.DisplayMember = "LanguageViewData"
		lstAVAMLanguages.ValueMember = "ID"

		If (avamLanguageData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprach Daten konnten nicht geladen werden."))
			Return False
		End If

		lstAVAMLanguages.DataSource = avamLanguageData

		Return True

	End Function

	Private Function LoadOstJobDetails() As Boolean
		Dim success As Boolean = True

		Try
			m_recID = m_OstJobMasterData.id
			txtojInterneID.Text = m_OstJobMasterData.interneid
			If Me.txtojInterneID.Text = String.Empty Then Me.txtojInterneID.Text = m_currentVacancyNumber
			txtojKeywords.Text = m_OstJobMasterData.keywords
			txtojdirektlinkIFrame.Text = String.Format(m_OstJobMasterData.Direkt_Link, m_currentVacancyNumber)
			txtojBewerberLink.Text = m_OstJobMasterData.bewerberlink

			sbIsostjobOnline.Value = m_OstJobMasterData.isonline.GetValueOrDefault(False)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			success = False
		End Try

		Return success
	End Function

	Private Function LoadAVAMVacancyData() As Boolean
		Dim success As Boolean = True

		Try
			Dim result = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserFullName,
																	 m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
			If result.VakNr Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten für STMP konnte nicht geladen werden."))

				Return False
			End If

			LoadAssignedAVAMLanguageData()
			lueAVAMEducation.EditValue = result.EducationCode


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			success = False
		End Try


		Return success

	End Function



#Region "Funktionen zum Reseten der Daten..."

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()

		Try
			For Each ctrls As Control In Me.Controls
				ResetControl(ctrls)
			Next

		Catch ex As Exception

		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Trace.WriteLine(String.Format("{0}: {1}", con.GetType, con.Name))

		Select Case True
			Case TypeOf (con) Is System.Windows.Forms.TextBox
				'Trace.WriteLine(String.Format("TextBox: {0}", con.Name))
				Dim tb As System.Windows.Forms.TextBox = CType(con, System.Windows.Forms.TextBox)
				tb.Text = String.Empty

			Case TypeOf (con) Is System.Windows.Forms.ComboBox
				Dim cbo As System.Windows.Forms.ComboBox = CType(con, System.Windows.Forms.ComboBox)
				cbo.Text = String.Empty

			Case TypeOf (con) Is System.Windows.Forms.GroupBox
				'Dim grp As Control = con
				If con.HasChildren Then
					For Each ctrl In con.Controls
						'Trace.WriteLine(String.Format("GroupBox: {0} / {1}", ctrl.GetType.ToString, ctrl.Name))
						ResetControl(CType(ctrl, Control))
					Next
				End If

			Case TypeOf (con) Is System.Windows.Forms.ListBox
				Dim lst As ListBox = CType(con, ListBox)
				'Trace.WriteLine(String.Format("ListBox: {0}", lst.Name))
				lst.Items.Clear()

			Case TypeOf (con) Is System.Windows.Forms.ListView
				Dim lv As ListView = CType(con, ListView)
				'Trace.WriteLine(String.Format("ListView: {0}", con.Name))
				lv.Items.Clear()

			Case TypeOf (con) Is DevExpress.XtraEditors.MemoExEdit
				Dim tb As DevExpress.XtraEditors.MemoExEdit = CType(con, DevExpress.XtraEditors.MemoExEdit)
				tb.Text = String.Empty

			Case TypeOf (con) Is DevExpress.XtraEditors.CalcEdit
				Dim tb As DevExpress.XtraEditors.CalcEdit = CType(con, DevExpress.XtraEditors.CalcEdit)
				tb.Text = String.Empty

			Case TypeOf (con) Is DevExpress.XtraEditors.DateEdit
				Dim tb As DevExpress.XtraEditors.DateEdit = CType(con, DevExpress.XtraEditors.DateEdit)
				tb.Text = String.Empty

			Case TypeOf (con) Is DevExpress.XtraEditors.SpinEdit
				Dim tb As DevExpress.XtraEditors.SpinEdit = CType(con, DevExpress.XtraEditors.SpinEdit)
				tb.Text = 100

			Case TypeOf (con) Is DevExpress.XtraRichEdit.RichEditControl
				Dim tb As DevExpress.XtraRichEdit.RichEditControl = CType(con, DevExpress.XtraRichEdit.RichEditControl)
				tb.HtmlText = String.Empty

			Case TypeOf (con) Is DevExpress.XtraEditors.GroupControl
				If con.HasChildren Then
					For Each ctrl In con.Controls
						ResetControl(CType(ctrl, Control))
					Next
				End If

			Case TypeOf (con) Is DevExpress.XtraEditors.PanelControl
				If con.HasChildren Then
					For Each ctrl In con.Controls
						ResetControl(CType(ctrl, Control))
					Next
				End If

			Case TypeOf (con) Is DevExpress.XtraEditors.CheckEdit
				Dim grp As DevExpress.XtraEditors.CheckEdit = CType(con, DevExpress.XtraEditors.CheckEdit)
				grp.Checked = False
				'Trace.WriteLine(String.Format("CheckEdit: {0}", grp.Name))

			Case TypeOf (con) Is DevExpress.XtraTab.XtraTabControl
				If con.HasChildren Then
					For Each ctrl In con.Controls
						ResetControl(CType(ctrl, Control))
					Next
				End If

			Case TypeOf (con) Is DevExpress.XtraTab.XtraTabPage
				If con.HasChildren Then
					For Each ctrl In con.Controls
						ResetControl(CType(ctrl, Control))
					Next
				End If

			Case TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl
				Dim lst As DevExpress.XtraEditors.ListBoxControl = CType(con, DevExpress.XtraEditors.ListBoxControl)
				'Trace.WriteLine(String.Format("ListBoxControl: {0}", lst.Name))
				lst.Items.Clear()

			Case TypeOf (con) Is DevExpress.XtraEditors.LookUpEdit
				Dim le As DevExpress.XtraEditors.LookUpEdit = CType(con, DevExpress.XtraEditors.LookUpEdit)
				'Trace.WriteLine(String.Format("LookUpEdit: {0}", le.Name))
				le.EditValue = 0
				le.Properties.NullText = String.Empty


		End Select

	End Sub

#End Region


#Region "Funktionen für Berufsgruppe in Job.ch"



#End Region


	Function ValidateData() As Boolean

		Dim PublicationFields = m_VacancyDatabaseAccess.LoadJobCHInseratData(m_currentVacancyNumber)

		ErrorProvider1.Clear()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		' must fields...
		Dim xmlFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
		Dim FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

		Dim mustVacancyCantonBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancycontonselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		isValid = isValid And SetErrorIfInvalid(lueCanton, ErrorProvider1, (mustVacancyCantonBeSelected AndAlso lueCanton Is Nothing AndAlso String.IsNullOrEmpty(lueCanton.EditValue)), errorText)

		Dim mustVacancyRegionBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyregionselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		isValid = isValid And SetErrorIfInvalid(lueRegion_1, ErrorProvider1, (mustVacancyRegionBeSelected AndAlso String.IsNullOrEmpty(lueRegion_1.Text)), errorText)

		Dim strMsg As String = String.Empty
		Dim allowedExternJobplattforms As Boolean = (sbIsJCHOnline.Value OrElse sbIsostjobOnline.Value)
		Dim mustVacancyVorspannBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyvorspannselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		If mustVacancyVorspannBeSelected Then
			isValid = isValid And PublicationFields.Vorspann
			If Not PublicationFields.Vorspann Then
				strMsg = m_Translate.GetSafeTranslationValue("Sie haben keinen 'Vorspann' eingetragen.{0}")
			End If
		End If

		Dim mustVacancyTaetigkeitBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyactivityselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		If mustVacancyTaetigkeitBeSelected Then
			isValid = isValid AndAlso Not String.IsNullOrWhiteSpace(PublicationFields.Aufgabe)

			If String.IsNullOrWhiteSpace(PublicationFields.Aufgabe) Then
				strMsg &= m_Translate.GetSafeTranslationValue("Sie haben keine 'Tätigkeit' eingetragen.{0}")
			End If
		End If

		Dim mustVacancyAnforderungBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyrequirementselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		If mustVacancyAnforderungBeSelected Then
			isValid = isValid AndAlso Not String.IsNullOrWhiteSpace(PublicationFields.Anforderung)
			If String.IsNullOrWhiteSpace(PublicationFields.Anforderung) Then
				strMsg &= m_Translate.GetSafeTranslationValue("Sie haben keine 'Anforderung' eingetragen.{0}")
			End If
		End If

		Dim mustVacancyWirBietenBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyweofferselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		If mustVacancyWirBietenBeSelected Then
			isValid = isValid AndAlso Not String.IsNullOrWhiteSpace(PublicationFields.Wirbieten)
			If String.IsNullOrWhiteSpace(PublicationFields.Wirbieten) Then
				strMsg &= m_Translate.GetSafeTranslationValue("Sie haben keine 'Wir bieten' eingetragen.{0}")
			End If
		End If

		If Not lueBeruf_1.EditValue Is Nothing Then
			If lueErfahrung_1.EditValue Is Nothing Then
				errorText = "Sie müssen 1. Berufserfahrung eintragen."
				isValid = isValid And SetErrorIfInvalid(lueErfahrung_1, ErrorProvider1, (lueErfahrung_1.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueErfahrung_1.EditValue)), errorText)

				strMsg &= m_Translate.GetSafeTranslationValue("Sie müssen 1. Berufserfahrung eintragen.{0}")
			End If
			If lueBEPosition_1.EditValue Is Nothing Then
				errorText = "Sie müssen 1. Berufsposition eintragen."
				isValid = isValid And SetErrorIfInvalid(lueBEPosition_1, ErrorProvider1, (lueBEPosition_1.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueBEPosition_1.EditValue)), errorText)

				strMsg &= m_Translate.GetSafeTranslationValue("Sie müssen 1. Berufsposition eintragen.{0}")
			End If
		End If

		If Not lueBeruf_2.EditValue Is Nothing Then
			If lueErfahrung_2.EditValue Is Nothing Then
				errorText = "Sie müssen 2. Berufserfahrung eintragen."
				isValid = isValid And SetErrorIfInvalid(lueErfahrung_2, ErrorProvider1, (lueErfahrung_2.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueErfahrung_2.EditValue)), errorText)

				strMsg &= m_Translate.GetSafeTranslationValue("Sie müssen 2. Berufserfahrung eintragen.{0}")
			End If
			If lueBEPosition_2.EditValue Is Nothing Then
				errorText = "Sie müssen 2. Berufsposition eintragen."
				isValid = isValid And SetErrorIfInvalid(lueBEPosition_2, ErrorProvider1, (lueBEPosition_2.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueBEPosition_2.EditValue)), errorText)

				strMsg &= m_Translate.GetSafeTranslationValue("Sie müssen 2. Berufsposition eintragen.{0}")
			End If
		End If


		If strMsg.Length > 0 Then
			m_UtilityUI.ShowInfoDialog(String.Format(strMsg, vbNewLine), m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Warning)

			isValid = False
		End If

		Return isValid

	End Function


	Private Sub OnbtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
		Dim success As Boolean = True
		Dim msg As String = String.Empty

		pcc_Language.HidePopup()

		success = success AndAlso UpdateAllJobPlattformData()

		If Not success Then
			msg = "Ihre Daten konnten nicht gespeichert werden."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			Return
		End If

		success = success AndAlso m_mandant.GetWOSGuid(m_InitializationChangedData.MDData.MDNr, Now.Year).WOSVacancyGuid.Length > 10
		success = success AndAlso TransferDataToWebService()

		success = success AndAlso LoadUpdatedVacancyData()

		sbIsInternOnline.Value = m_JobCHMasterData.IEExport.GetValueOrDefault(False)
		chkJobChannelPriority.Checked = m_JobCHMasterData.JobChannelPriority.GetValueOrDefault(False)

		sbIsJCHOnline.Value = m_JobCHMasterData.IsOnline.GetValueOrDefault(False)
		sbIsostjobOnline.Value = m_OstJobMasterData.isonline.GetValueOrDefault(False)

	End Sub

	Private Sub sbIsInternOnline_ValueChanged(sender As Object, e As EventArgs) Handles sbIsInternOnline.ValueChanged
		chkJobChannelPriority.Visible = sbIsInternOnline.Value
	End Sub

	Private Function UpdateAllJobPlattformData() As Boolean
		Dim success As Boolean = True
		Dim msg As String = String.Empty


		If Not ValidateData() Then Return False

		m_JobCHMasterData.VakNr = m_currentVacancyNumber

		m_JobCHMasterData.TitelForSearch = Me.txtTitelforSearch.EditValue
		m_JobCHMasterData.ShortDescription = Me.txtShortDescription.EditValue

		If lueBeruf_1.EditValue Is Nothing Then ResetFachBereich_1()
		If lueBeruf_2.EditValue Is Nothing Then ResetFachBereich_2()

		m_JobCHMasterData.IsOnline = Me.sbIsJCHOnline.Value
		m_JobCHMasterData.IEExport = Me.sbIsInternOnline.Value
		m_JobCHMasterData.JobChannelPriority = chkJobChannelPriority.Checked

		m_JobCHMasterData.Beginn = Me.txt_Antrittper.EditValue
		m_JobCHMasterData.JobProzent = String.Format("{0}#{1}", If(Me.seArbpensum_Von.Text = 0, 100, Me.seArbpensum_Von.Text),
																							If(Me.seArbpensum_Bis.Text = 0, 100, Me.seArbpensum_Bis.Text))
		' Anstellungsarten...
		Try
			Dim strValue As New List(Of String)

			If Me.chkAnstellung_1.CheckState = CheckState.Checked Then strValue.Add("1")
			If Me.chkAnstellung_2.CheckState = CheckState.Checked Then strValue.Add("2")
			If Me.chkAnstellung_3.CheckState = CheckState.Checked Then strValue.Add("3")
			If Me.chkAnstellung_4.CheckState = CheckState.Checked Then strValue.Add("4")
			If Me.chkAnstellung_5.CheckState = CheckState.Checked Then strValue.Add("5")
			If Me.chkAnstellung_6.CheckState = CheckState.Checked Then strValue.Add("6")

			m_JobCHMasterData.Anstellung = String.Join("#", strValue.ToArray())

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler: die Anstellungen werden nicht gespeichert."))

		End Try

		m_JobCHMasterData.Dauer = Me.txt_Dauer.Text
		' Alter...
		Try
			Dim strValue As New List(Of String)

			If Me.chkAlter_1.CheckState = CheckState.Checked Then strValue.Add("1")
			If Me.chkAlter_2.CheckState = CheckState.Checked Then strValue.Add("2")
			If Me.chkAlter_3.CheckState = CheckState.Checked Then strValue.Add("3")
			If Me.chkAlter_4.CheckState = CheckState.Checked Then strValue.Add("4")

			m_JobCHMasterData.MAAge = String.Join("#", strValue.ToArray())

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler: die Alterseinstufungen werden nicht gespeichert."))

		End Try
		m_JobCHMasterData.MASex = lueGender.EditValue
		m_JobCHMasterData.Vak_Kanton = lueCanton.EditValue
		m_JobCHMasterData.Position_Value = luePosition.EditValue
		m_JobCHMasterData.ChangedUserNumber = m_InitializationData.UserData.UserNr

		success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyJobCHMasterData(m_JobCHMasterData)
		If Not success Then
			m_Logger.LogError("jobs.ch data could not be saved!")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gespeichert werden. Der Vorgang wird abgebrochen!"))

			Return False
		End If

		success = success AndAlso m_VacancyDatabaseAccess.UpdateJobCHBrunchesData(lueBranche.EditValue, lueBranche.Text, CurrentVacancyData.VakNr)
		If Not success Then
			m_Logger.LogError("jobs.ch brunches could not be saved!")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Branche konnten nicht gespeichert werden. Der Vorgang wird abgebrochen!"))

			Return False
		End If
		success = success AndAlso m_VacancyDatabaseAccess.UpdateJobCHOccupationData(lueBeruf_1.EditValue, lueBeruf_2.EditValue, lueErfahrung_1.EditValue, lueErfahrung_2.EditValue,
																																								lueBEPosition_1.EditValue, lueBEPosition_2.EditValue,
																																								lueBeruf_1.Text, lueBeruf_2.Text, lueErfahrung_1.Text, lueErfahrung_2.Text,
																																								lueBEPosition_1.Text, lueBEPosition_2.Text,
																																								CurrentVacancyData.VakNr)
		If Not success Then
			m_Logger.LogError("jobs.ch occupation could not be saved!")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Berufdaten konnten nicht gespeichert werden. Der Vorgang wird abgebrochen!"))

			Return False
		End If

		success = success AndAlso m_VacancyDatabaseAccess.UpdateJobCHBildungsniveauData(lueBNiveau_1.EditValue, lueBNiveau_2.EditValue, lueBNiveau_3.EditValue, lueBNiveau_1.Text, lueBNiveau_2.Text, lueBNiveau_3.Text, CurrentVacancyData.VakNr)
		If Not success Then
			m_Logger.LogError("jobs.ch bildungsniveau could not be saved!")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Bildungsniveaus konnten nicht gespeichert werden. Der Vorgang wird abgebrochen!"))

			Return False
		End If

		success = success AndAlso m_VacancyDatabaseAccess.UpdateJobCHRegionData(lueRegion_1.EditValue, lueRegion_2.EditValue, lueRegion_1.Text, lueRegion_2.Text, CurrentVacancyData.VakNr)
		If Not success Then
			m_Logger.LogError("jobs.ch regions could not be saved!")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Regionen konnten nicht gespeichert werden. Der Vorgang wird abgebrochen!"))

			Return False
		End If

		If Not success Then Return False

		If success AndAlso sbIsostjobOnline.Enabled Then success = success AndAlso SaveOstJobData()
		success = success AndAlso SaveStmpSettingData()


		Return success

	End Function

	Private Function LoadUpdatedVacancyData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadJobCHMasterData()
		success = success AndAlso LoadOstJobMasterData()


		Return success

	End Function

	Private Function TransferDataToWebService() As Boolean
		Dim success As Boolean = True
		Dim msg As String = String.Empty

		m_VacancySetting.IsInternExported = sbIsInternOnline.Value
		m_VacancySetting.IsJCHExported = sbIsJCHOnline.Value
		m_VacancySetting.IsOstJobExported = sbIsostjobOnline.Value
		m_AVAMRestricted = False

		Try

			PublishForJobplattforms()
			If Not m_AVAMRestricted Then
				msg = "Ihre Daten wurden gespeichert.{0}"

				' interne Jobplattform
				If m_mandant.GetWOSGuid(m_InitializationChangedData.MDData.MDNr, Now.Year).WOSVacancyGuid.Length > 10 Then
					msg &= "<b>Interne Jobplattform:</b> {1}{0}"
					msg = String.Format(msg, vbNewLine, If(Me.IsInternSaved.value, "OK", String.Format("Fehler: {0}", Me.IsInternSaved.message)))
				End If

				' jobs.ch
				If sbIsJCHOnline.Enabled AndAlso Me.m_VacancySetting.IsAllowedJCH Then
					msg &= "<b>Jobs.ch:</b> {1}{0}"
					msg = String.Format(msg, vbNewLine, If(Me.IsJobCHSaved.value, "OK", String.Format("Fehler: {0}", Me.IsJobCHSaved.message)))
				End If

				' ostjob.ch
				If sbIsostjobOnline.Enabled AndAlso Me.m_VacancySetting.IsAllowedOstJob Then
					msg &= "<b>ostjob.ch:</b> {1}{0}"
					msg = String.Format(msg, vbNewLine, If(Me.IsOstJobSaved.value, "OK", String.Format("Fehler: {0}", Me.IsOstJobSaved.message)))
				End If

			Else
				msg = "Ihre Daten wurden gespeichert.<br>Ihre Daten können noch nicht an einem Jobplattform übermittelt werden, da die Stelle noch <b>gesperrt</b> ist!"
				IsInternSaved.value = False
				IsJobCHSaved.value = False
				IsOstJobSaved.value = False

			End If
			m_UtilityUI.ShowInfoDialog(msg)

			success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyOnlineData(m_currentVacancyNumber,
																					  m_VacancySetting.SelectedKDNr, IsInternSaved.value AndAlso sbIsInternOnline.Value, chkJobChannelPriority.Checked,
																					  IsJobCHSaved.value AndAlso sbIsJCHOnline.Value,
																					  IsOstJobSaved.value AndAlso sbIsostjobOnline.Value)


			RaiseEvent CreditInfoDataSaved()

			If IsInternSaved.value Then sbIsInternOnline.Value = sbIsInternOnline.Value Else sbIsInternOnline.Value = False
			If IsJobCHSaved.value Then sbIsJCHOnline.Value = sbIsJCHOnline.Value Else sbIsJCHOnline.Value = False
			If IsOstJobSaved.value Then
				sbIsostjobOnline.Value = sbIsostjobOnline.Value
			Else
				sbIsostjobOnline.Value = False
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return True
	End Function

	Private Sub PublishForJobplattforms()
		Dim success As Boolean = True

		Me.IsInternSaved = New UploadResult
		Me.IsJobCHSaved = New UploadResult
		Me.IsOstJobSaved = New UploadResult

		' jobroom transfer
		m_AVAMRestricted = False
		If sbIsInternOnline.Value OrElse sbIsJCHOnline.Value OrElse sbIsostjobOnline.Value Then m_AVAMRestricted = Not UploadDataSBN2000Data()
		m_Logger.LogInfo(String.Format("vacancy number {0} >>> AVAM is restricted: {0}", CurrentVacancyData.VakNr, m_AVAMRestricted))

		If m_AVAMRestricted Then Return

		' intern transfer
		Try
			Dim orgUserdata = m_mandant.GetSelectedUserData(m_InitializationData.MDData.MDNr, VacancyAdvisorNumber)
			Dim InernUploader As New SP.Vacancies.Intern.InternVacancyUploader(m_InitializationChangedData)
			Dim internexportresult = InernUploader.UploadVacancies(orgUserdata.UserGuid, m_InitializationChangedData.MDData.MDGuid, CurrentVacancyData.VakNr)
			If internexportresult Is Nothing Then
				Me.IsInternSaved.value = False
				Me.IsInternSaved.message = "internal error"

			Else
				Me.IsInternSaved.value = internexportresult.value
				Me.IsInternSaved.message = internexportresult.message

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		End Try

		' jobs.ch transfer
		Try
			If Me.m_VacancySetting.IsAllowedJCH Then ' AndAlso sbIsJCHOnline.Enabled Then
				'If m_JobCHMasterData.Organisation_ID = 0 OrElse m_JobCHMasterData.Organisation_SubID = 0 Then
				'	Me.IsJobCHSaved.value = False
				'	Me.IsJobCHSaved.message = m_Translate.GetSafeTranslationValue("Kundennummer in Einstellungen > Jobs.CH > Zur Publikation - ist nicht eingetragen!")
				'Else

				Dim result = m_VacancyDatabaseAccess.GetJobCHExportedCounterData(m_InitializationChangedData.MDData.MDGuid, m_currentVacancyNumber, m_InitializationChangedData.UserData.UserKST)
				If result.IsCounterOK Then
					Dim jobCHUploader As New SP.Vacancies.JobCH.JobCHVacancyUploader(New SP.Vacancies.JobCH.InitializeClass With {.MDData = m_InitializationChangedData.MDData,
																																										 .UserData = m_InitializationChangedData.UserData,
																																										 .ProsonalizedData = m_InitializationData.ProsonalizedData,
																																										 .TranslationData = m_InitializationData.TranslationData})
					Dim jobchexportresult = jobCHUploader.UploadVacancies(m_InitializationChangedData.UserData.UserGuid, m_InitializationChangedData.MDData.MDGuid, CurrentVacancyData.VakNr)
					If jobchexportresult Is Nothing Then
						Me.IsJobCHSaved.value = False
						Me.IsJobCHSaved.message = "internal error"

					Else
						Me.IsJobCHSaved.value = jobchexportresult.value
						Me.IsJobCHSaved.message = jobchexportresult.message

					End If

				Else
					Me.IsJobCHSaved.value = False
					Me.IsJobCHSaved.message = m_Translate.GetSafeTranslationValue("Ihre Kontingent wird hiermit überschritten!")
				End If

				'End If
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		End Try

		' ostjob.ch transfer
		Try
			If Me.m_VacancySetting.IsAllowedOstJob Then
				Dim result = m_VacancyDatabaseAccess.GetOstJobExportedCounterData(m_InitializationChangedData.MDData.MDGuid, m_currentVacancyNumber, m_InitializationChangedData.UserData.UserKST)
				If result.IsCounterOK AndAlso result.AllowedJobQuantity > 0 Then
					Dim ostjobUploader As New SP.Vacancies.OstJobCH.OstJobCHVacancyUploader(New SP.Vacancies.OstJobCH.InitializeClass With {.MDData = m_InitializationChangedData.MDData,
																																										 .UserData = m_InitializationChangedData.UserData,
																																										 .ProsonalizedData = m_InitializationData.ProsonalizedData,
																																										 .TranslationData = m_InitializationData.TranslationData})
					Dim ostjobexportresult = ostjobUploader.UploadVacancies(m_InitializationChangedData.UserData.UserGuid, m_InitializationChangedData.MDData.MDGuid, CurrentVacancyData.VakNr)
					If ostjobexportresult Is Nothing Then
						Me.IsOstJobSaved.value = False
						Me.IsOstJobSaved.message = "internal error"

					Else
						Me.IsOstJobSaved.value = ostjobexportresult.value
						Me.IsOstJobSaved.message = ostjobexportresult.message
					End If

				Else
					Me.IsOstJobSaved.value = False
					Me.IsOstJobSaved.message = m_Translate.GetSafeTranslationValue("Ihre Kontingent wird hiermit überschritten!")
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		End Try

	End Sub

	Private Function UploadDataSBN2000Data() As Boolean
		Dim result As Boolean = True

		Dim data = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitialChangedData.MDData.MDGuid, m_InitialChangedData.UserData.UserFullNameWithComma, m_InitialChangedData.UserData.UserNr, m_currentVacancyNumber)
		If data Is Nothing Then Return False
		Dim SBN2000Number = CurrentVacancyData.SBNNumber.GetValueOrDefault(0)
		If Now < New DateTime(2018, 7, 1) Then SBN2000Number = 0
		If SBN2000Number = 0 Then Return True

		If Not String.IsNullOrWhiteSpace(data.JobroomID) Then
			If Not data.ReportingObligation.GetValueOrDefault(False) Then Return True
			If Not data.ReportingObligationEndDate.HasValue Then
				If data.AVAMStateEnum = AVAMState.INSPECTING OrElse data.AVAMStateEnum = AVAMState.PUBLISHED_RESTRICTED Then
					Return False
				End If
			End If

			If CType(data.ReportingObligationEndDate, Date) >= Now.Date Then
				Return False
			Else
				Return True
			End If

		End If

		If SBN2000Number > 0 AndAlso result Then
			Dim allowedUserTransfer As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 709)

			Dim continiuewithTransfer As Boolean = False
			Dim notPossibleToTransfermsg As String = m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Diese Stelle muss unter bestimmten Voraussetzungen dem zuständigen RAV-Amt gemeldet werden! Bitte leiten Sie die Stelle manuell weiter!")

			Dim newTransfermsg As String = m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Diese Stelle muss dem zuständigen RAV-Amt gemeldet werden!<br>Ich werde dies für Sie übernehmen.")

			If Not allowedUserTransfer Then
				newTransfermsg = String.Format("Ihnen fehlen die nötigen Rechte zur Übermittlung der Daten an RAV-Amt. Ihre Stelle wird nicht auf die gewünschten Job-Plattformen publiziert!<br>Bitte kontaktieren Sie Ihrem Systemadministrator.")
			End If

			Dim oldTransferedmsg As String = m_Translate.GetSafeTranslationValue("<b>Achtung:</b> Diese Stelle ist seit {0:G} durch {1} übermittelt worden! Falls die Sperrfrist erreicht ist, können Sie diese Stelle veröffentlichen. Möchten Sie die Stelle jetzt veröffentlichen?")
			oldTransferedmsg = String.Format("{1}{0}{0}{2}", vbNewLine, oldTransferedmsg, m_Translate.GetSafeTranslationValue("JA: Die Stelle wird auf die gewählten Plattformen publiziert."))
			oldTransferedmsg = String.Format("{1}{0}{2}", vbNewLine, oldTransferedmsg, m_Translate.GetSafeTranslationValue("NEIN: Die Vorgang wird abgebrochen."))

			If data.ReportingDate.HasValue Then
				continiuewithTransfer = Not m_UtilityUI.ShowYesNoDialog(String.Format(oldTransferedmsg, data.ReportingDate, data.ReportingFrom), m_Translate.GetSafeTranslationValue("Stellenmeldepflicht"), MessageBoxDefaultButton.Button1)
				Return continiuewithTransfer

			ElseIf data.ReportingDate Is Nothing Then

				m_UtilityUI.ShowOKDialog(Me, String.Format(newTransfermsg, data.ReportingDate), m_Translate.GetSafeTranslationValue("Stellenmeldepflicht"), MessageBoxIcon.Information)
				If Not allowedUserTransfer Then Return False
			End If

		End If


		Try
			m_Logger.LogError(String.Format("vacancy {0} data will be transmitted...", m_currentVacancyNumber))

			Dim orgUserdata = m_mandant.GetSelectedUserData(m_InitializationData.MDData.MDNr, VacancyAdvisorNumber)
			Dim InernUploader As New SP.Vacancies.Intern.InternVacancyUploader(m_InitializationChangedData)

			InernUploader.CurrentVacancyNumber = m_currentVacancyNumber

			Dim avamTransferResult = InernUploader.AddSTMPVacancyToRAV(m_InitialChangedData.MDData.MDGuid, m_InitialChangedData.UserData.UserGuid, m_InitializationData.UserData.UserNr = 1)

			result = result AndAlso Not avamTransferResult Is Nothing ' AndAlso avamTransferResult.State.GetValueOrDefault(False)

			If Not result Then
				m_Logger.LogError("stmp data could not be transmitted")

				Dim msg As String = m_Translate.GetSafeTranslationValue("Ihre Meldepflichtigen Daten konnten nicht übermittelt werden.")
				If Not String.IsNullOrWhiteSpace(avamTransferResult.ErrorMessage.Content) Then
					msg &= String.Format(m_Translate.GetSafeTranslationValue("{0}{0}Titel: {1}{0}Detail: {2}{0}{3}"), vbNewLine, avamTransferResult.ErrorMessage.Title, avamTransferResult.ErrorMessage.Message, avamTransferResult.ErrorMessage.Detail)
				Else
					msg &= String.Format(m_Translate.GetSafeTranslationValue("{0}{0}Titel: Validierung der Daten{0}Detail: Ihre Daten konnten nicht validiert werden!{0}"), vbNewLine)

				End If

				m_UtilityUI.ShowErrorDialog(msg)

				Return False
			End If

			If Not avamTransferResult.ReportingObligation.GetValueOrDefault(False) Then
				Dim msgReportingObligation As String = m_Translate.GetSafeTranslationValue("Achtung: Ihre Stelle unterliegt nicht der Meldepflicht. Ich werde Ihre Stelle auf gewünschten Job-Plattformen übermitteln.")
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msgReportingObligation), m_Translate.GetSafeTranslationValue("Meldepflicht"), MessageBoxIcon.Information)
			End If
			data.ReportingDate = Now
			data.ReportingFrom = m_InitializationData.UserData.UserFullName

			data.AVAMRecordState = avamTransferResult.AVAMRecordState
			data.JobroomID = avamTransferResult.JobroomID
			data.stellennummerEgov = avamTransferResult.StellennummerEgov
			data.ReportingObligation = avamTransferResult.ReportingObligation.GetValueOrDefault(False)
			data.ReportingObligationEndDate = avamTransferResult.ReportingObligationEndDate
			data.AVAMRecordState = avamTransferResult.AVAMRecordState

			result = result AndAlso m_VacancyDatabaseAccess.UpdateVacancyStmpSettingData(m_InitialChangedData.MDData.MDGuid, m_InitialChangedData.UserData.UserNr, data)

			If avamTransferResult.ReportingObligation.GetValueOrDefault(False) Then
				m_UtilityUI.ShowOKDialog(Me, m_Translate.GetSafeTranslationValue("Die Stelle wurde erfolgreich an das AVAM übermittelt."), m_Translate.GetSafeTranslationValue("Meldepflicht"), MessageBoxIcon.Information)
				m_Logger.LogWarning(String.Format("database update was: {0} >>> ReportingObligation is {1}!", result, avamTransferResult.ReportingObligation.GetValueOrDefault(False)))

				result = False
			End If

		Catch ex As Exception
			Return False

		End Try


		Return result

	End Function

	'Private Function BuildJasonstring(ByVal customerID As String, ByVal userID As String, ByVal vacancyData As VacancyMasterData, ByVal vacancyJobCHData As VacancyInseratJobCHData, ByVal vacancyStmpData As VacancyStmpSettingData) As StringBuilder
	'	Dim msgContent = "library is started..."

	'	Dim htmlToMarkdown As New Html2Markdown.Converter
	'	Dim userFullname As String = "System"

	'	Try
	'		If m_InitializationData.UserData Is Nothing Then
	'			msgContent = "userData was null"
	'			Throw New Exception(msgContent)
	'		End If
	'		userFullname = m_InitializationData.UserData.UserFullName

	'		If vacancyData Is Nothing Then
	'			msgContent = "vacancyData was null"
	'			Throw New Exception(msgContent)
	'		End If
	'		If vacancyJobCHData Is Nothing Then
	'			msgContent = "vacancyJobCHData was null"
	'			Throw New Exception(msgContent)
	'		End If
	'		If vacancyStmpData Is Nothing Then
	'			msgContent = "vacancyStmpData was null"
	'			Throw New Exception(msgContent)
	'		End If
	'		If m_InitializationData.MDData Is Nothing Then
	'			msgContent = "MDData was null"
	'			Throw New Exception(msgContent)
	'		End If

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'		m_UtilityUI.ShowErrorDialog(ex.ToString, "AddAVAMAdvertisementToRAV.GettingObjects")

	'		Return Nothing

	'	End Try

	'	Dim sb As New StringBuilder()
	'	Dim sw As New StringWriter(sb)

	'	Try

	'		Using writer As JsonWriter = New JsonTextWriter(sw)

	'			writer.WriteStartObject()

	'			'writer.WritePropertyName("externalUrl")
	'			'writer.WriteValue("externalUrl_Value")
	'			'writer.WritePropertyName("externalReference")
	'			'writer.WriteValue("externalReference_Value")
	'			writer.WritePropertyName("reportToAvam")
	'			If vacancyStmpData.ReportToAvam.GetValueOrDefault(False) Then
	'				writer.WriteValue("true")
	'			Else
	'				writer.WriteValue("false")
	'			End If

	'			If vacancyStmpData.NumberOfJobs.GetValueOrDefault(1) > 1 Then
	'				writer.WritePropertyName("numberOfJobs")
	'				writer.WriteValue(vacancyStmpData.NumberOfJobs.GetValueOrDefault(1))
	'			End If

	'			' contact
	'			' Provide an administrative contact (e. g. an HR employee); this contact is used for email notifications concerning the reporting obligation
	'			writer.WritePropertyName("contact")
	'			writer.WriteStartObject()

	'			writer.WritePropertyName("languageIsoCode")
	'			writer.WriteValue("de")
	'			writer.WritePropertyName("salutation")
	'			If m_InitializationData.UserData.UserSalutation = "Herr" Then
	'				writer.WriteValue("MR")
	'			Else
	'				writer.WriteValue("MS")
	'			End If
	'			writer.WritePropertyName("firstName")
	'			writer.WriteValue(m_InitializationData.UserData.UserFName)
	'			writer.WritePropertyName("lastName")
	'			writer.WriteValue(m_InitializationData.UserData.UserLName)
	'			writer.WritePropertyName("phone")
	'			writer.WriteValue((m_InitializationData.UserData.UserMDTelefon))
	'			writer.WritePropertyName("email")
	'			writer.WriteValue(m_InitializationData.UserData.UserMDeMail)

	'			writer.WriteEndObject()


	'			'' jobDescriptions
	'			' The text of the job advertisement; may be multilingual
	'			writer.WritePropertyName("jobDescriptions")
	'			writer.WriteStartArray()
	'			writer.WriteStartObject()

	'			writer.WritePropertyName("languageIsoCode")
	'			writer.WriteValue("de")
	'			writer.WritePropertyName("title")
	'			writer.WriteValue(vacancyData.Bezeichnung)
	'			writer.WritePropertyName("description")
	'			Dim anforderung = vacancyJobCHData.Anforderung
	'			Dim taetigkeit = vacancyJobCHData.Aufgabe
	'			If String.IsNullOrWhiteSpace(anforderung) Then
	'				If Not taetigkeit.Contains("Tätigkeit:") Then taetigkeit = String.Format("Tätigkeit:<br>{0}", taetigkeit)
	'				anforderung = taetigkeit
	'			Else

	'				If Not anforderung.Contains("Anforderung:") Then anforderung = String.Format("Anforderung:<br>{0}", anforderung)
	'				anforderung &= String.Format("<br>{0}", taetigkeit)

	'			End If
	'			writer.WriteValue(htmlToMarkdown.Convert(anforderung))

	'			writer.WriteEndObject()
	'			writer.WriteEndArray()



	'			writer.WriteEndObject()

	'		End Using

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'		m_UtilityUI.ShowErrorDialog(ex.ToString, "AddAVAMAdvertisementToRAV.StringWriter")

	'		Return Nothing

	'	End Try


	'	Return sb

	'End Function



	Private Function SaveOstJobData() As Boolean
		Dim result As Boolean = False

		Dim data = m_VacancyDatabaseAccess.LoadOstJobMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserFullName, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		data.id = m_recID

		' Datumfelder überprüfen...
		If data.startdate Is Nothing OrElse data.startdate > Now.Date Then
			data.startdate = Now.Date
		End If
		If DateDiff(DateInterval.Day, data.startdate.GetValueOrDefault(Now), Now, FirstDayOfWeek.System, FirstWeekOfYear.System) > 7 AndAlso data.enddate >= Now.Date Then
			data.startdate = Now.Date
		End If
		data.enddate = If(data.enddate Is Nothing, data.startdate.GetValueOrDefault(Now).AddDays(14), CDate(data.enddate))
		If data.enddate < data.startdate Then data.enddate = data.startdate.GetValueOrDefault(Now).AddDays(14)

		Dim strMsg As String = String.Empty
		If data.enddate < Now.Date Then
			strMsg = "Das Enddatum für die Publikation auf Ostjob.ch liegt in der Vergangenheit.{0}"
			Me.sbIsostjobOnline.Value = False
		End If

		data.VakNr = CurrentVacancyData.VakNr

		data.interneid = Me.txtojInterneID.EditValue
		data.keywords = Me.txtojKeywords.EditValue
		data.linkiframe = String.Format(Me.txtojdirektlinkIFrame.EditValue, CurrentVacancyData.VakNr)
		data.bewerberlink = Me.txtojBewerberLink.EditValue

		data.changedon = Now.ToString
		data.changedfrom = m_InitializationData.UserData.UserFullName
		data.isonline = Me.sbIsostjobOnline.Value
		data.ChangedUserNumber = m_InitializationData.UserData.UserNr


		result = m_VacancyDatabaseAccess.UpdateVacancyOstJobMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, data)

		Return result
	End Function

	Private Function SaveStmpSettingData() As Boolean
		Dim success As Boolean = True

		Dim data = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserFullName, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		data.EducationCode = lueAVAMEducation.EditValue

		success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyStmpSettingData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, data)
		If Not success Then
			m_Logger.LogError("AVAM bildung could not be saved!")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(AVAM) Bildung Daten konnten nicht gespeichert werden. Der Vorgang wird abgebrochen!"))

			Return False
		End If


		Return success

	End Function

	Private Sub cmdJCHSetting_Click(sender As System.Object, e As System.EventArgs) Handles btnJCHSetting.Click

		Dim frmSetting = New frmVacancySetting(m_InitializationData)

		frmSetting.VacancyDatabaseAccess = m_VacancyDatabaseAccess
		frmSetting.CurrentVacancyData = CurrentVacancyData
		frmSetting.VacancySettingData = m_VacancySetting
		frmSetting.CurrentVacancyJobCHData = m_JobCHMasterData
		frmSetting.ShouldbeOnline = sbIsJCHOnline.Enabled AndAlso sbIsJCHOnline.Value

		frmSetting.LoadData()
		frmSetting.Show()
		frmSetting.TopMost = m_InitializationData.UserData.UserNr <> 1
		frmSetting.BringToFront()

		'pccJCHSetting.SuspendLayout()
		'Me.pccJCHSetting.Manager = New DevExpress.XtraBars.BarManager

		'pccJCHSetting.ShowCloseButton = True
		'pccJCHSetting.ShowSizeGrip = True

		'pccJCHSetting.ShowPopup(Cursor.Position)
		'pccJCHSetting.ResumeLayout()

	End Sub


#Region "Regionen..."

	'Private Sub lueRegion_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueRegion_1.ButtonClick
	'	If e.Button.Index = 1 Then
	'		Me.lueRegion_1.EditValue = 0
	'		Me.lueRegion_1.Properties.NullText = String.Empty
	'		Me.lblRegion1_ID.Text = String.Empty
	'	End If
	'End Sub

	'Private Sub lueRegion_1_EditValueChanged(sender As Object,
	'																				 e As System.EventArgs) Handles lueRegion_1.EditValueChanged
	'	Dim test As Object = Me.lueRegion_1.GetSelectedDataRow
	'	Dim currow As DataRowView = TryCast(lueRegion_1.GetSelectedDataRow(), DataRowView)
	'	If Not currow Is Nothing Then
	'		Me.lblRegion1_ID.Text = currow("ID_1").ToString()
	'	End If

	'End Sub

	'Private Sub lueRegion_1_QueryCloseUp(sender As Object,
	'																		 e As System.ComponentModel.CancelEventArgs) Handles lueRegion_1.QueryCloseUp

	'	Dim test As Object = Me.lueRegion_1.GetSelectedDataRow
	'	Dim currow As DataRowView = TryCast(lueRegion_1.GetSelectedDataRow(), DataRowView)
	'	If Not currow Is Nothing Then
	'		Me.lblRegion1_ID.Text = currow("ID_1").ToString()
	'	End If

	'End Sub

	'Private Sub lueRegion_1_QueryPopUp(sender As Object,
	'																	 e As System.ComponentModel.CancelEventArgs) Handles lueRegion_1.QueryPopUp

	'	Me.lueRegion_1.Properties.Columns.Clear()
	'	Dim dt As DataTable = ListRegion("DE").Tables(0)
	'	Me.lueRegion_1.Properties.DataSource = dt

	'	lueRegion_1.Properties.DisplayMember = "Bezeichnung"
	'	lueRegion_1.Properties.ValueMember = "ID_1"

	'	Dim Col0 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID_1", "ID_1", 0)
	'	Dim Col1 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Bezeichnung", m_Translate.GetSafeTranslationValue("Region"), 100)

	'	Col0.Visible = False
	'	lueRegion_1.Properties.Columns.Add(Col0)
	'	lueRegion_1.Properties.Columns.Add(Col1)

	'	lueRegion_1.Properties.DropDownRows = Math.Min(dt.DefaultView.Count, Math.Max(My.Settings.iRowCount_Region_1, 20))
	'	lueRegion_1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
	'	lueRegion_1.Properties.ForceInitialize()

	'End Sub

	'Private Sub lueRegion_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueRegion_2.ButtonClick
	'	If e.Button.Index = 1 Then
	'		Me.lueRegion_2.EditValue = 0
	'		Me.lueRegion_2.Properties.NullText = String.Empty
	'		Me.lblRegion2_ID.Text = String.Empty
	'	End If
	'End Sub

	'Private Sub lueRegion_2_EditValueChanged(sender As Object,
	'																			 e As System.EventArgs) Handles lueRegion_2.EditValueChanged
	'	Dim test As Object = Me.lueRegion_2.GetSelectedDataRow
	'	Dim currow As DataRowView = TryCast(lueRegion_2.GetSelectedDataRow(), DataRowView)
	'	If Not currow Is Nothing Then
	'		Me.lblRegion2_ID.Text = currow("ID_1").ToString()
	'	End If

	'End Sub

	'Private Sub lueRegion_2_QueryCloseUp(sender As Object,
	'																		 e As System.ComponentModel.CancelEventArgs) Handles lueRegion_2.QueryCloseUp

	'	Dim test As Object = Me.lueRegion_2.GetSelectedDataRow
	'	Dim currow As DataRowView = TryCast(lueRegion_2.GetSelectedDataRow(), DataRowView)
	'	If Not currow Is Nothing Then
	'		Me.lblRegion2_ID.Text = currow("ID_1").ToString()
	'	End If

	'End Sub

	'Private Sub lueRegion_2_QueryPopUp(sender As Object,
	'																	 e As System.ComponentModel.CancelEventArgs) Handles lueRegion_2.QueryPopUp

	'	Me.lueRegion_2.Properties.Columns.Clear()
	'	Dim dt As DataTable = ListRegion("DE").Tables(0)
	'	Me.lueRegion_2.Properties.DataSource = dt

	'	lueRegion_2.Properties.DisplayMember = "Bezeichnung"
	'	lueRegion_2.Properties.ValueMember = "ID_1"

	'	Dim Col0 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID_1", "ID_1", 0)
	'	Dim Col1 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Bezeichnung", m_Translate.GetSafeTranslationValue("Region"), 100)

	'	Col0.Visible = False
	'	lueRegion_2.Properties.Columns.Add(Col0)
	'	lueRegion_2.Properties.Columns.Add(Col1)

	'	lueRegion_2.Properties.DropDownRows = Math.Min(dt.DefaultView.Count, Math.Max(My.Settings.iRowCount_Region_2, 20))
	'	lueRegion_2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
	'	lueRegion_2.Properties.ForceInitialize()

	'End Sub

#End Region


#Region "Sprache..."


	Private Sub OnbtnAddJobCHLanguage_Click(sender As Object, e As EventArgs) Handles btnAddJobCHLanguage.Click

		Dim success = LoadJobCHLanguageDropDownData()
		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(Jobs.CH)Sprach Daten konnten nicht geladen werden."))
			Return
		End If

		m_LanguageForPlattform = ExternalPlattforms.JOBSCH

		pcc_Language.Manager = New DevExpress.XtraBars.BarManager
		pcc_Language.ShowCloseButton = True
		pcc_Language.ShowSizeGrip = True

		lueLanguageName.EditValue = Nothing
		lueLanguageNiveau.EditValue = Nothing

		pcc_Language.ShowPopup(Cursor.Position)
		pcc_Language.ResumeLayout()

	End Sub

	Private Sub OnbtnAddAVAMLanguage_Click(sender As Object, e As EventArgs) Handles btnAddAVAMLanguage.Click

		Dim success = LoadAVAMLanguageDropDownData()
		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(Meldepflicht)Sprach Daten konnten nicht geladen werden."))
			Return
		End If

		m_LanguageForPlattform = ExternalPlattforms.AVAM

		pcc_Language.Manager = New DevExpress.XtraBars.BarManager
		pcc_Language.ShowCloseButton = True
		pcc_Language.ShowSizeGrip = True

		lueLanguageName.EditValue = Nothing
		lueLanguageNiveau.EditValue = Nothing

		pcc_Language.ShowPopup(Cursor.Position) ' Cursor.Position)
		pcc_Language.ResumeLayout()

	End Sub

	Private Sub cmdSaveLang_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveLang.Click
		Dim success As Boolean = True

		If lueLanguageName.EditValue Is Nothing OrElse lueLanguageNiveau.EditValue Is Nothing Then Return

		Dim data As New VacancyJobCHLanguageData

		data.VakNr = m_currentVacancyNumber

		data.Bezeichnung = lueLanguageName.Text
		data.Bezeichnung_Value = lueLanguageName.EditValue
		data.LanguageNiveau = lueLanguageNiveau.Text
		data.LanguageNiveau_Value = lueLanguageNiveau.EditValue

		success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyLanguageData(data, m_LanguageForPlattform)

		Me.pcc_Language.HidePopup()

		If success Then
			If m_LanguageForPlattform = ExternalPlattforms.AVAM Then
				LoadAssignedAVAMLanguageData()

			Else
				LoadAssignedJobCHLanguageData()
			End If

		Else
			Dim msg As String = "Ihre Sprach Daten konnten nicht gespeichert werden."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			Return
		End If


	End Sub

	Private Sub OnlstJobCHLanguages_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lstJobCHLanguages.KeyDown

		If e.KeyCode = Keys.Delete Then
			Dim success As Boolean = True
			Dim data = SelectedJobCHLanguageViewData
			If data Is Nothing Then Return


			success = success AndAlso m_VacancyDatabaseAccess.DeleteJobCHLanguageData(data)

			If success Then
				LoadAssignedJobCHLanguageData()

			Else
				Dim msg As String = "Ihre Sprach Daten konnten nicht gelöscht werden."
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			End If
		End If

	End Sub

	Private Sub OnlstAVAMLanguages_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lstAVAMLanguages.KeyDown

		If e.KeyCode = Keys.Delete Then
			Dim success As Boolean = True
			Dim data = SelectedAVAMLanguageViewData
			If data Is Nothing Then Return


			success = success AndAlso m_VacancyDatabaseAccess.DeleteJobCHLanguageData(data)

			If success Then
				LoadAssignedAVAMLanguageData()

			Else
				Dim msg As String = "Ihre Sprach Daten konnten nicht gelöscht werden."
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			End If
		End If

	End Sub

#End Region


	'Private Sub OnXtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabSettingJobplattforms.SelectedPageChanged

	'	grpAntritt.Visible = e.Page Is xtabjobsch
	'	grpSprache.Visible = e.Page Is xtabjobsch

	'End Sub


	Private Sub frmJobsCH_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			pcc_Language.HidePopup()

			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_VACANCIES_FORM_JCH_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_JCH_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_JCH_HEIGHT, Me.Height)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub


	Protected Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	Private Sub ResetBeruf_1()
		lueBeruf_1.EditValue = Nothing
		lueErfahrung_1.EditValue = Nothing
		lueBEPosition_1.EditValue = Nothing
	End Sub

	Private Sub ResetFachBereich_1()
		lueErfahrung_1.EditValue = Nothing
		lueBEPosition_1.EditValue = Nothing
	End Sub

	Private Sub ResetBeruf_2()
		lueBeruf_2.EditValue = Nothing
		lueErfahrung_2.EditValue = Nothing
		lueBEPosition_2.EditValue = Nothing
	End Sub

	Private Sub ResetFachBereich_2()
		lueErfahrung_2.EditValue = Nothing
		lueBEPosition_2.EditValue = Nothing
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
			ElseIf TypeOf sender Is DateEdit Then
				Dim dateEdit As DateEdit = CType(sender, DateEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub

	Private Sub lueBeruf_1_EditValueChanged(sender As Object, e As EventArgs) Handles lueBeruf_1.EditValueChanged

		If m_SuppressUIEvents Then Return

		ResetFachBereich_1()

		LoadFirstExperienceDropDownData(lueBeruf_1.EditValue)

		m_SuppressUIEvents = False

	End Sub

	Private Sub lueBeruf_2_EditValueChanged(sender As Object, e As EventArgs) Handles lueBeruf_2.EditValueChanged

		If m_SuppressUIEvents Then Return

		ResetFachBereich_2()

		LoadSecondExperienceDropDownData(lueBeruf_2.EditValue)

		m_SuppressUIEvents = False

	End Sub



#Region "helper class"


	Private Class DropDownViewData
		Public Property DisplayText As String
		Public Property Value As String

	End Class


#End Region


End Class


