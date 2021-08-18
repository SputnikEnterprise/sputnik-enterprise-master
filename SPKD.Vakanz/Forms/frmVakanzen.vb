
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports System.IO
Imports DevExpress.XtraRichEdit.Services

Imports DevExpress.XtraNavBar
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Popup

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.Infrastructure.ucListSelectPopup
Imports SPKD.Vakanz.Settings
Imports DevExpress.XtraRichEdit.Export
Imports DevExpress.XtraRichEdit.Export.Html
Imports System.Text.RegularExpressions
Imports System.Web
Imports SP.KD.CPersonMng.UI

Imports SP.Infrastructure
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.TableSetting

Imports SPKD.Vakanz.ClsDataDetail
Imports DevComponents.DotNetBar
Imports SP.DatabaseAccess.Vacancy
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.Utils.Win
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.DXErrorProvider
Imports DevExpress.Utils.VisualEffects
Imports System.ComponentModel


Public Class frmVakanzen

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_InitializationChangedData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ESDatabaseAccess As IESDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess


	Protected m_UtilityUI As UtilityUI
	Protected m_Utility As Utility

	Private m_Logger As ILogger = New Logger()
	Private m_SettingsManager As SettingsManager
	Private m_CustomerPopupColumns As New List(Of PopupColumDefintion)

	Private m_CreditInfoDetailForm As frmJobsCH
	Private m_mandant As Mandant
	Private m_path As ClsProgPath
	Private m_common As CommonSetting

	Private pcc As New DevExpress.XtraBars.PopupControlContainer
	Private strLinkName As String = String.Empty
	Private strLinkCaption As String = String.Empty
	Private bChangedContent As Boolean

	Private Property IsTab2Selected As Boolean

	Private Property IsTabJobCHSelected As Boolean
	Private Property IsTabJobWinnerSelected As Boolean

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private m_AllowedDesign As Boolean

	Private Property TranslatedPage As New List(Of Boolean)

	Private Property m_connectionString As String
	Private m_CurrentCustomerNumber As Integer
	Private m_currentVacancyNumber As Integer
	Private m_currentVacancyData As VacancyMasterData


	''' <summary>
	''' The customer data.
	''' </summary>
	Private m_SelectedCustomerData As SP.DatabaseAccess.Customer.DataObjects.CustomerMasterData

	Private m_InernUploader As SP.Vacancies.Intern.InternVacancyUploader
	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml
	Private m_MandantXMLFile As String
	Private m_MandantSetting As String
	Private m_StartNumberSetting As String

	Private m_CheckEditImportant As RepositoryItemCheckEdit
	Private m_CheckEditCompleted As RepositoryItemCheckEdit
	Private m_JobPlattformsData As IEnumerable(Of JobPlattformsInfoData)
	Private m_Badge As DevExpress.Utils.VisualEffects.Badge


	Private Enum MessageBackColorEnum
		SUCCESSMESSAGE = -16744448
		INFOMESSAGE = -16776961
		ERRORMESSAGE = -65536
		WARNINGMESSAGE = -256
	End Enum

	Private Enum MessageTextColorEnum
		BLACK = -16777216
		WHITE = -1
		RED = -65536
	End Enum


#Region "Private Consts"

	Private Const POPUP_DEFAULT_WIDTH As Integer = 420
	Private Const POPUP_DEFAULT_HEIGHT As Integer = 325
	Private Const MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING As String = "MD_{0}/StartNr"

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_InitializationChangedData = _setting
		m_InitialData = m_InitializationData
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_mandant = New Mandant
		m_common = New CommonSetting
		m_path = New ClsProgPath
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_SettingsManager = New SettingsManager

		InitializeComponent()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

		m_SuppressUIEvents = True
		m_InernUploader = New SP.Vacancies.Intern.InternVacancyUploader(m_InitializationData)


		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		Dim allowedChange = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 708)
		Me.lueAdvisor.Enabled = allowedChange
		If m_InitializationData.UserData Is Nothing Then lueAdvisor.Enabled = True

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Reset()
		LoadDropDownData()

		CreateMyNavBar()
		TranslateControls()


		'AddHandler LblVakNr.TextChanged, AddressOf LblVakNr_TextChanged

		AddHandler lueAdvisor.EditValueChanged, AddressOf onAdvisorChanged
		AddHandler lueStatus.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueKontakt.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueGroup.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueSubGroup.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueSBNNumber.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueZHD.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		UcTinyMCE1.CreateEditorControl()

	End Sub

#End Region


#Region "public properties"

	Public Property VacancySetting As New ClsVakSetting

#End Region


#Region "public Methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True


		m_currentVacancyNumber = VacancySetting.SelectedVakNr.GetValueOrDefault(0)
		LoadAdvisorDropDownData()

		success = success AndAlso LoadVacancyData()

		Return success

	End Function

#End Region


#Region "private properties"

	''' <summary>
	''' Reads the employee offset from the settings.
	''' </summary>
	''' <returns>Employee offset or zero if it could not be read.</returns>
	Private Function ReadVacancyOffsetFromSettings() As Integer

		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(lueMandant.EditValue, Now.Year)
		m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		m_StartNumberSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING, lueMandant.EditValue)

		Dim vacancyNumberStartNumberSetting As String = Val(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Vakanzennr", m_StartNumberSetting)))

		Dim intVal As Integer
		If Integer.TryParse(vacancyNumberStartNumberSetting, intVal) Then
			Return intVal
		Else
			Return 0
		End If

	End Function


#End Region


#Region "DropDown-Data..."

	Private Function TranslateUserInfo() As String
		Dim result As String = String.Empty

		Try

			Dim fileExtension = "txt"
			Dim tplUserVakInfoFile = Path.Combine(m_mandant.GetSelectedMDTemplatePath(lueMandant.EditValue), String.Format("tplUserVacancy_{0}.{1}", lueAdvisor.EditValue, fileExtension))
			If Not File.Exists(tplUserVakInfoFile) Then tplUserVakInfoFile = Path.Combine(m_mandant.GetSelectedMDTemplatePath(lueMandant.EditValue),
				String.Format("tplUserVacancy.{0}", fileExtension))

			If File.Exists(tplUserVakInfoFile) Then
				'Dim macrofunc As New TranslateMacros

				Dim ParsedFile As String = String.Empty
				Dim line As String = String.Empty

				Dim lines = IO.File.ReadAllLines(tplUserVakInfoFile, System.Text.Encoding.Default)
				For Each line In lines
					If Not String.IsNullOrWhiteSpace(line) Then
						Dim translatedText As String = ParseTemplateFile(line)
						ParsedFile &= If(String.IsNullOrWhiteSpace(translatedText), String.Empty, translatedText & vbNewLine)
					End If
				Next

				result = ParsedFile.Replace(vbNewLine, "</br>")
			End If


		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(ex.ToString())

		Finally

		End Try

		Return result

	End Function

	Public Sub Reset()

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True


		lblBezeichnung.Text = String.Empty
		pnlAVAM.Visible = False

		grdJobplattforms.DataSource = Nothing


		tgsReportingObligation.EditValue = False
		lblAVAMState.Text = String.Empty
		lblReportingObligationEndDate.Text = String.Empty
		lblReportingDate.Text = String.Empty
		lblAVAMSyncDate.Text = String.Empty


		ResetMandantenDropDown()
		ResetAdvisorDropDown()
		ResetBusinessBranchesDropDown()

		ResetVacanciesGroupDropDown()
		ResetVacanciesSubGroupDropDown()
		ResetSBNNumberDropDown()
		ResetContactInfoDataDropDown()
		ResetStatesDropDown()

		ResetCustomerDropDown()
		ResetResponsiblePersonDropDown()
		ResetPostcodeDropDown()

		m_CheckEditImportant = CType(grdJobplattforms.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditImportant.PictureChecked = My.Resources.bullet_ball_green ' Bullet_green
		m_CheckEditImportant.PictureUnchecked = My.Resources.bullet_ball_red
		m_CheckEditImportant.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		ResetJobplattformsGrid()

		Dim jobplattforms As New List(Of JobPlattformsInfoData)
		jobplattforms.Add(New JobPlattformsInfoData With {.JobplattformLabel = "website", .IsOnline = False, .TranferedJobs = 0})
		jobplattforms.Add(New JobPlattformsInfoData With {.JobplattformLabel = "jobs.ch", .IsOnline = False, .TranferedJobs = 0, .TotalAllowedJobs = 0, .TotalExpireSoonJobs = 0})
		jobplattforms.Add(New JobPlattformsInfoData With {.JobplattformLabel = "ostjob.ch", .IsOnline = False, .TranferedJobs = 0, .TotalAllowedJobs = 0 = .TotalExpireSoonJobs = 0})
		m_JobPlattformsData = jobplattforms


		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub ResetJobplattformsGrid()
		' Reset the grid
		gvJobplattforms.Columns.Clear()


		Dim importantColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		importantColumn.Caption = " "
		importantColumn.Name = "IsOnline"
		importantColumn.FieldName = "IsOnline"
		importantColumn.Visible = True
		importantColumn.ColumnEdit = m_CheckEditImportant
		importantColumn.Width = 20
		gvJobplattforms.Columns.Add(importantColumn)

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Plattform")
		columnDate.Name = "JobplattformLabel"
		columnDate.FieldName = "JobplattformLabel"
		columnDate.Visible = True
		columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gvJobplattforms.Columns.Add(columnDate)

		Dim descriptionColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		descriptionColumn.Caption = m_Translate.GetSafeTranslationValue("Total Slots")
		descriptionColumn.Name = "TotalOpenJobs"
		descriptionColumn.FieldName = "TotalOpenJobs"
		descriptionColumn.Visible = True
		descriptionColumn.Width = 100
		gvJobplattforms.Columns.Add(descriptionColumn)

		Dim kstColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		kstColumn.Caption = m_Translate.GetSafeTranslationValue("Total publiziert")
		kstColumn.Name = "TranferedJobs"
		kstColumn.FieldName = "TranferedJobs"
		kstColumn.Visible = True
		kstColumn.Width = 100
		gvJobplattforms.Columns.Add(kstColumn)

		Dim ColumnTotalExpireSoonJobs As New DevExpress.XtraGrid.Columns.GridColumn()
		ColumnTotalExpireSoonJobs.Caption = m_Translate.GetSafeTranslationValue("Ablauf (heute oder morgen)")
		ColumnTotalExpireSoonJobs.Name = "TotalExpireSoonJobs"
		ColumnTotalExpireSoonJobs.FieldName = "TotalExpireSoonJobs"
		ColumnTotalExpireSoonJobs.Visible = True
		ColumnTotalExpireSoonJobs.Width = 200
		gvJobplattforms.Columns.Add(ColumnTotalExpireSoonJobs)


	End Sub

#Region "Reset"

	''' <summary>
	''' Resets contact info drop down.
	''' </summary>
	Private Sub ResetContactInfoDataDropDown()

		lueKontakt.Properties.DisplayMember = "bez_d"
		lueKontakt.Properties.ValueMember = "recvalue"

		Dim columns = lueKontakt.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueKontakt.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueKontakt.Properties.SearchMode = SearchMode.AutoComplete
		lueKontakt.Properties.AutoSearchColumnIndex = 0

		lueKontakt.Properties.NullText = String.Empty
		lueKontakt.EditValue = Nothing

	End Sub

	Private Sub ResetVacanciesGroupDropDown()

		lueGroup.Properties.DisplayMember = "bez_d"
		lueGroup.Properties.ValueMember = "Bez_Value"

		Dim columns = lueGroup.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueGroup.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGroup.Properties.SearchMode = SearchMode.AutoComplete
		lueGroup.Properties.AutoSearchColumnIndex = 0

		lueGroup.Properties.NullText = String.Empty
		lueGroup.EditValue = Nothing

	End Sub

	Private Sub ResetVacanciesSubGroupDropDown()

		lueSubGroup.Properties.DisplayMember = "TranslatedValue"
		lueSubGroup.Properties.ValueMember = "SubGroup"

		Dim columns = lueSubGroup.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedValue", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueSubGroup.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueSubGroup.Properties.SearchMode = SearchMode.AutoComplete
		lueSubGroup.Properties.AutoSearchColumnIndex = 0

		lueSubGroup.Properties.NullText = String.Empty
		lueSubGroup.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the sbn data drop down.
	''' </summary>
	Private Sub ResetSBNNumberDropDown()

		lueSBNNumber.Properties.DisplayMember = "Bez_Translated" ' "Group_Translated"
		lueSBNNumber.Properties.ValueMember = "TitleNumber"

		gvSBNNumber.OptionsView.ShowIndicator = False
		gvSBNNumber.OptionsView.ShowColumnHeaders = True
		gvSBNNumber.OptionsView.ShowFooter = False
		lueSBNNumber.Properties.View.ExpandAllGroups()

		gvSBNNumber.OptionsView.ShowAutoFilterRow = True
		gvSBNNumber.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvSBNNumber.Columns.Clear()

		Dim columnGroup_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGroup_Translated.Caption = m_Translate.GetSafeTranslationValue("Berufsgruppe")
		columnGroup_Translated.Name = "Group_Translated"
		columnGroup_Translated.FieldName = "Group_Translated"
		columnGroup_Translated.Visible = True
		columnGroup_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvSBNNumber.Columns.Add(columnGroup_Translated)

		Dim columnBez_Translated As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBez_Translated.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnBez_Translated.Name = "Bez_Translated"
		columnBez_Translated.FieldName = "Bez_Translated"
		columnBez_Translated.Visible = True
		columnBez_Translated.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvSBNNumber.Columns.Add(columnBez_Translated)


		lueSBNNumber.Properties.PopupFormSize = My.Settings.sbnformsize ' New Size(lueSBNNumber.Width * 1.75, Math.Max(500, height))

		columnGroup_Translated.GroupIndex = 0
		lueSBNNumber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueSBNNumber.Properties.NullText = String.Empty
		lueSBNNumber.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the customer drop down.
	''' </summary>
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

		Dim columnCustomerPostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerPostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnCustomerPostcodeLocation.Name = "CustomerPostcodeLocation"
		columnCustomerPostcodeLocation.FieldName = "CustomerPostcodeLocation"
		columnCustomerPostcodeLocation.Visible = True
		columnCustomerPostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnCustomerPostcodeLocation)

		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomer.Properties.NullText = String.Empty
		lueCustomer.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the responsible person drop down.
	''' </summary>
	Private Sub ResetResponsiblePersonDropDown()

		lueZHD.Properties.DisplayMember = "SalutationLastNameFirstName"
		lueZHD.Properties.ValueMember = "ResponsiblePersonRecordNumber"

		gvZHD.OptionsView.ShowIndicator = False
		gvZHD.OptionsView.ShowColumnHeaders = True
		gvZHD.OptionsView.ShowFooter = False
		gvZHD.OptionsView.ShowAutoFilterRow = True
		gvZHD.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvZHD.Columns.Clear()

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "ResponsiblePersonRecordNumber"
		columnRecordNumber.FieldName = "ResponsiblePersonRecordNumber"
		columnRecordNumber.Visible = False
		gvZHD.Columns.Add(columnRecordNumber)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnName.Name = "SalutationLastNameFirstName"
		columnName.FieldName = "SalutationLastNameFirstName"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvZHD.Columns.Add(columnName)


		lueZHD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueZHD.Properties.NullText = String.Empty
		lueZHD.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the postcode drop down.
	''' </summary>
	Private Sub ResetPostcodeDropDown()

		luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
		luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

		luePostcode.Properties.DisplayMember = "Postcode"
		luePostcode.Properties.ValueMember = "Postcode"

		Dim columns = luePostcode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Postcode", 0, m_Translate.GetSafeTranslationValue("PLZ")))
		columns.Add(New LookUpColumnInfo("Location", 0, m_Translate.GetSafeTranslationValue("Ort")))

		luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePostcode.Properties.SearchMode = SearchMode.AutoComplete
		luePostcode.Properties.AutoSearchColumnIndex = 1
		luePostcode.Properties.NullText = String.Empty
		luePostcode.EditValue = Nothing

		luePostcode.Properties.DropDownRows = 15

	End Sub

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub


	Private Sub ResetBusinessBranchesDropDown()

		lueBusinessBranches.Properties.DisplayMember = "Name"
		lueBusinessBranches.Properties.ValueMember = "Name"

		Dim columns = lueBusinessBranches.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueBusinessBranches.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueBusinessBranches.Properties.SearchMode = SearchMode.AutoComplete
		lueBusinessBranches.Properties.AutoSearchColumnIndex = 0

		lueBusinessBranches.Properties.NullText = String.Empty
		lueBusinessBranches.EditValue = Nothing

	End Sub


	Private Sub ResetStatesDropDown()

		lueStatus.Properties.DisplayMember = "bez_d"
		lueStatus.Properties.ValueMember = "recvalue"

		Dim columns = lueStatus.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("bez_d", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		lueStatus.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueStatus.Properties.SearchMode = SearchMode.AutoComplete
		lueStatus.Properties.AutoSearchColumnIndex = 0

		lueStatus.Properties.NullText = String.Empty
		lueStatus.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the advisors drop down.
	''' </summary>
	Private Sub ResetAdvisorDropDown()

		lueAdvisor.Properties.DisplayMember = "FullName"
		lueAdvisor.Properties.ValueMember = "KST"

		Dim columns = lueAdvisor.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("KST", 0))
		columns.Add(New LookUpColumnInfo("FullName", 0, "Name"))

		lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
		lueAdvisor.Properties.AutoSearchColumnIndex = 1

		lueAdvisor.Properties.NullText = String.Empty
		lueAdvisor.EditValue = Nothing

	End Sub

#End Region


	Sub LoadDropDownData()

		LoadMandantenDropDown()
		'LoadAdvisorDropDownData()
		LoadBusinessBranchesDropDown()

		LoadVacanciesGroupDropDownData()
		LoadAVAM2020OccupationsDropDownData()

		LoadVacancyContactDropDownData()
		LoadVacancyStateDropDownData()

		LoadCustomerDropDownData()
		LoadPostcodeDropDownData()

	End Sub

#Region "Loading data"

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		'Dim Data = m_DataAccess.LoadMandantenData()
		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData()

		If Data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

		'    Return Not Data Is Nothing
	End Sub

	Private Function LoadAdvisorDropDownData() As Boolean
		'Dim advisorData = m_DataAccess.LoadAdvisorData()
		Dim advisorData As IEnumerable(Of AdvisorData) '= m_CommonDatabaseAccess.LoadAdvisorData()
		If m_currentVacancyNumber = 0 Then
			advisorData = m_CommonDatabaseAccess.LoadActivatedAdvisorData()
		Else
			advisorData = m_CommonDatabaseAccess.LoadAllAdvisorsData()
		End If
		Dim advisorViewDataList As New List(Of AdvisorViewData)

		If Not advisorData Is Nothing Then
			For Each advisor In advisorData
				Dim advisorViewData As AdvisorViewData = New AdvisorViewData
				advisorViewData.UserNumber = advisor.UserNumber
				advisorViewData.KST = advisor.KST
				advisorViewData.Salutation = advisor.Salutation
				advisorViewData.FristName = advisor.Firstname
				advisorViewData.LastName = advisor.Lastname

				advisorViewDataList.Add(advisorViewData)
			Next
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
		End If

		lueAdvisor.Properties.DataSource = advisorViewDataList
		lueAdvisor.Properties.ForceInitialize()

		Return Not advisorData Is Nothing
	End Function

	Private Function LoadVacanciesGroupDropDownData() As Boolean
		Dim data = m_TablesettingDatabaseAccess.LoadVacancyGroupData
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Gruppen konnten nicht geladen werden.")
		End If

		lueGroup.Properties.DataSource = data
		lueGroup.Properties.ForceInitialize()


		Return Not data Is Nothing
	End Function

	Private Function LoadAssignedVacanciesSubGroupDropDownData(ByVal mainGroup As String, ByVal language As String) As Boolean
		Dim data = m_TablesettingDatabaseAccess.LoadAssingedVacancySubGroupData(mainGroup, language)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Unter-Gruppen Daten konnten nicht geladen werden.")
		End If

		lueSubGroup.Properties.DataSource = data
		lueSubGroup.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function

	''' <summary>
	''' Loads the sbn 2000 drop down data.
	''' </summary>
	Private Function LoadAVAMOccupationsDropDownData() As Boolean

		Dim customerAssignedBranchData = m_InernUploader.LoadSTMPAllJobTitileData()
		If customerAssignedBranchData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AVAM Daten konnten nicht geladen werden."))
			Return False
		End If

		lueSBNNumber.Properties.DataSource = customerAssignedBranchData

		Return Not customerAssignedBranchData Is Nothing
	End Function

	Private Function LoadAVAM2020OccupationsDropDownData() As Boolean
		' Load data
		Dim customerAssignedBranchData = LoadAVAM2020OccupationsData(2020)

		lueSBNNumber.Properties.DataSource = customerAssignedBranchData

		Return Not customerAssignedBranchData Is Nothing
	End Function

	Private Function LoadAVAM2021OccupationsDropDownData() As Boolean
		' Load data
		Dim customerAssignedBranchData = LoadAVAM2020OccupationsData(2021)

		lueSBNNumber.Properties.DataSource = customerAssignedBranchData

		Return Not customerAssignedBranchData Is Nothing
	End Function

	Private Function LoadAVAM2020OccupationsData(ByVal year As Integer) As BindingList(Of SP.Vacancies.Intern.STMPJobViewData)
		Dim customerAssignedBranchData As New BindingList(Of SP.Vacancies.Intern.STMPJobViewData)

		If year = 2020 Then
			customerAssignedBranchData = m_InernUploader.LoadSTMP2020AllJobTitileData(year)
		ElseIf year = 2021 Then
			customerAssignedBranchData = m_InernUploader.LoadSTMP2020AllJobTitileData(year)
		End If

		If customerAssignedBranchData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("AVAM ({0}) Daten konnten nicht geladen werden."), year))

			Return Nothing
		End If

		Return customerAssignedBranchData
	End Function

	Private Function LoadSBNumberMappingData(ByVal oldSBNNumber As Integer) As BindingList(Of SP.Vacancies.Intern.STMPMappingViewData)

		Dim customerAssignedBranchData = m_InernUploader.LoadSTMPMappingData(oldSBNNumber)
		If customerAssignedBranchData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AVAM Mapping Daten konnten nicht geladen werden."))

			Return Nothing
		End If

		Return customerAssignedBranchData
	End Function

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim customerData = m_CustomerDatabaseAccess.LoadCustomerData()

		If customerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden."))
			Return False
		End If
		lueCustomer.Properties.DataSource = customerData


		Return True
	End Function

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadResponsiblePersonsDropDownData(ByVal customerNumber As Integer) As Boolean

		Dim responsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonDataActiv(customerNumber)

		Dim responsiblePersonViewData = Nothing

		If Not responsiblePersonData Is Nothing Then

			responsiblePersonViewData = New List(Of ResponsiblePersonViewData)

			For Each person In responsiblePersonData
				responsiblePersonViewData.Add(New ResponsiblePersonViewData With {
																																					.Lastname = person.Lastname,
																																					.Firstname = person.Firstname,
																																					.TranslatedSalutation = person.TranslatedSalutation,
																																					.ResponsiblePersonRecordNumber = person.RecordNumber,
																																					.ZState1 = person.ZState1,
																																					.ZState2 = person.ZState2
																																					 })
			Next

		End If

		lueZHD.EditValue = Nothing
		lueZHD.Properties.DataSource = responsiblePersonViewData

		Return Not responsiblePersonViewData Is Nothing
	End Function

	''' <summary>
	''' Loads the postcode drop downdata.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadPostcodeDropDownData() As Boolean
		Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

		If (postcodeData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
		End If

		luePostcode.Properties.DataSource = postcodeData
		luePostcode.Properties.ForceInitialize()

		Return Not postcodeData Is Nothing
	End Function

	Private Function LoadVacancyContactDropDownData() As Boolean

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadVacancyContactData()

			If (data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontakt-Daten konnten nicht geladen werden."))
				Return False
			End If

			lueKontakt.Properties.DataSource = data
			lueKontakt.Properties.ForceInitialize()


			Return Not data Is Nothing

		Catch ex As Exception

		End Try

	End Function

	Private Function LoadVacancyStateDropDownData() As Boolean

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadVacancyStateData()

			If (data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Status-Daten konnten nicht geladen werden."))
				Return False
			End If

			lueStatus.Properties.DataSource = data
			lueStatus.Properties.ForceInitialize()


			Return Not data Is Nothing

		Catch ex As Exception

		End Try

	End Function

	Private Function LoadBusinessBranchesDropDown() As Boolean

		Dim availableBusinessBranches = m_CommonDatabaseAccess.LoadBusinessBranchsData()

		If (availableBusinessBranches Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Filialen konnten nicht geladen werden."))
		End If

		lueBusinessBranches.Properties.DataSource = availableBusinessBranches
		lueBusinessBranches.Properties.ForceInitialize()

		Return Not availableBusinessBranches Is Nothing
	End Function


#End Region


	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		If m_SuppressUIEvents Then Return

		Dim SelectedData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantData)

		If lueMandant.EditValue = m_InitializationData.MDData.MDNr Then Return
		If Not SelectedData Is Nothing Then
			Dim MandantData = ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			m_InitializationData = MandantData


		Else
			lueCustomer.EditValue = Nothing
			lueZHD.EditValue = Nothing
			lueCustomer.Properties.DataSource = Nothing
			lueZHD.Properties.DataSource = Nothing

		End If
		m_connectionString = m_InitializationData.MDData.MDDbConn
		UpdateVacancyDataWithChengedUserAndMandant()

	End Sub

	Private Sub onAdvisorChanged(sender As System.Object, e As System.EventArgs)

		If m_SuppressUIEvents Then
			Return
		End If
		UpdateVacancyDataWithChengedUserAndMandant()

	End Sub

	Private Sub UpdateVacancyDataWithChengedUserAndMandant()
		Dim strMsg As String = String.Empty
		Dim selectedUserData As AdvisorViewData = TryCast(Me.lueAdvisor.GetSelectedDataRow(), AdvisorViewData)
		Dim m_ini = ChangeMandantData(lueMandant.EditValue, selectedUserData.UserNumber)

		m_InitialChangedData = New SP.Infrastructure.Initialization.InitializeClass(m_ini.TranslationData, m_InitializationData.ProsonalizedData, m_ini.MDData, m_ini.UserData)
		m_InitializationChangedData = m_InitialChangedData

		strMsg = m_Translate.GetSafeTranslationValue("Soll ich die Kontakteinträge und Account-Informationen automatisch auf geänderte Benutzer- und Mandantendaten ändern?")
		If m_currentVacancyData.VakNr.GetValueOrDefault(0) > 0 AndAlso m_UtilityUI.ShowYesNoDialog(strMsg, m_Translate.GetSafeTranslationValue("Benutzer- und Mandantendaten geändert")) = True Then

			Dim userInfo = TranslateUserInfo()
			Dim userData As New AdvisorData With {.Firstname = m_InitializationChangedData.UserData.UserFName,
				.Lastname = m_InitializationChangedData.UserData.UserLName,
				.KST = m_InitializationChangedData.UserData.UserKST,
				.Salutation = m_InitializationChangedData.UserData.UserSalutation,
				.UserNumber = m_InitializationChangedData.UserData.UserNr,
				.UserMDDTelefon = m_InitializationChangedData.UserData.UserMDDTelefon,
				.UserMDeMail = m_InitializationChangedData.UserData.UserMDeMail,
				.UserMDTelefon = m_InitializationChangedData.UserData.UserMDTelefon,
				.UserMDTelefax = m_InitializationChangedData.UserData.UserMDTelefax,
				.UserMDHomepage = m_InitializationChangedData.UserData.UserMDHomepage,
				.UserMobile = m_InitializationChangedData.UserData.UserMobile,
				.UserMDName = m_InitializationChangedData.UserData.UserMDName,
				.UserMDStrasse = m_InitializationChangedData.UserData.UserMDStrasse,
				.UserMDPLZ = m_InitializationChangedData.UserData.UserMDPLZ,
				.UserMDOrt = m_InitializationChangedData.UserData.UserMDOrt}
			Dim success As Boolean = m_VacancyDatabaseAccess.UpdateVacancyJobCHAdvisorData(lueMandant.EditValue, m_currentVacancyNumber, m_InitializationChangedData.UserData.UserNr, userData, userInfo)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gespeichert werden. Bitte ändern Sie die Angaben unter Einstellungen > Einstellungen."))

				Return
			End If
		End If

		LoadLogoDataForTransfer()

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

	Private Sub lueGroup_EditValueChanged(sender As Object, e As EventArgs) Handles lueGroup.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If
		lueSubGroup.EditValue = Nothing

		If lueGroup.EditValue Is Nothing Then Return
		LoadAssignedVacanciesSubGroupDropDownData(lueGroup.EditValue, m_InitializationData.UserData.UserLanguage)

	End Sub

#End Region


	Private Sub ReplaceRichEditCommandFactoryService(ByVal control As DevExpress.XtraRichEdit.RichEditControl)
		Dim service As IRichEditCommandFactoryService = control.GetService(Of IRichEditCommandFactoryService)()

		If service Is Nothing Then Return
		control.RemoveService(GetType(IRichEditCommandFactoryService))
		control.AddService(GetType(IRichEditCommandFactoryService),
											 New CustomCommand.CustomRichEditCommandFactoryService(control, service))

	End Sub


	Private Sub OnLueCustomer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomer.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueCustomer.EditValue Is Nothing Then

			Dim customerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(lueCustomer.EditValue, m_InitializationData.UserData.UserFiliale)

			If customerData Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Kundendaten konnten nicht geladen werden.")
			End If

			If customerData.CreditWarning.GetValueOrDefault(False) AndAlso
					(customerData.CreditLimit1 > 0 AndAlso customerData.OpenInvoiceAmount >= customerData.CreditLimit1) Or
					(customerData.CreditLimit2 > 0 AndAlso customerData.OpenInvoiceAmount >= customerData.CreditLimit2) Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Achtung: Kunden-Kreditlimite wurde erreicht oder überschritten.{0}Offener Debitorenbetrag: {1:n2}{0}1. Kunden-Kreditlimite: {2:n2}{0}2. Kunden-Kreditlimite: {3:n2}")
				msg = String.Format(msg, vbNewLine, customerData.OpenInvoiceAmount, customerData.CreditLimit1, customerData.CreditLimit2)

				m_UtilityUI.ShowInfoDialog(msg)
			End If

			m_SelectedCustomerData = customerData
			grpKundendaten.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde: {0}"), lueCustomer.EditValue)

			LoadResponsiblePersonsDropDownData(lueCustomer.EditValue)
		Else
			grpKundendaten.Text = m_Translate.GetSafeTranslationValue("Kunde")

			m_SelectedCustomerData = Nothing
			lueZHD.EditValue = Nothing
			lueZHD.Properties.DataSource = Nothing
		End If

	End Sub

	Private Sub OnlueCustomer_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueCustomer.ButtonClick
		Const ID_OF_OPEN_BUTTON As Int32 = 2
		If lueCustomer.EditValue Is Nothing Then Return

		If e.Button.Index = ID_OF_OPEN_BUTTON Then
			Dim hub = MessageService.Instance.Hub
			Dim openCustomerMng As New OpenCustomerMngRequest(sender, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, lueCustomer.EditValue)
			hub.Publish(openCustomerMng)
		End If

	End Sub

	Private Sub OnlueZHD_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueZHD.ButtonClick
		Const ID_OF_OPEN_BUTTON As Int32 = 2
		If lueCustomer.EditValue Is Nothing OrElse lueZHD.EditValue Is Nothing Then Return

		If e.Button.Index = ID_OF_OPEN_BUTTON Then
			Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr))

			If (responsiblePersonsFrom.LoadResponsiblePersonData(lueCustomer.EditValue, lueZHD.EditValue)) Then
				responsiblePersonsFrom.Show()
			End If
		End If

	End Sub

	''' <summary>
	''' Handles change of postcode.
	''' </summary>
	Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

		Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)

		If Not postCodeData Is Nothing Then
			txtLocation.Text = postCodeData.Location
		End If

	End Sub

	''' <summary>
	''' Handles new value event on postcode(plz) lookup edit.
	''' </summary>
	Private Sub OnLuePostcode_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles luePostcode.ProcessNewValue

		If Not luePostcode.Properties.DataSource Is Nothing Then

			Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

			Dim newPostcode As New PostCodeData With {.Postcode = e.DisplayValue.ToString()}
			listOfPostcode.Add(newPostcode)

			e.Handled = True
		End If
	End Sub


	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			xtabAllgmein.Text = m_Translate.GetSafeTranslationValue(xtabAllgmein.Text)
			xtabJobCH.Text = m_Translate.GetSafeTranslationValue(xtabJobCH.Text)
			xtabJobwinner.Text = m_Translate.GetSafeTranslationValue(xtabJobwinner.Text)

			'lblEigeneWebsite.Text = m_Translate.GetSafeTranslationValue(lblEigeneWebsite.Text)
			grpvakanz.Text = m_Translate.GetSafeTranslationValue(grpvakanz.Text)

			tgsReportingObligation.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsReportingObligation.Properties.OnText)
			tgsReportingObligation.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsReportingObligation.Properties.OffText)
			lblReportingObligationEndDate.Text = m_Translate.GetSafeTranslationValue(lblReportingObligationEndDate.Text)
			lblReportingDate.Text = m_Translate.GetSafeTranslationValue(lblReportingDate.Text)
			lblAVAMAktualisiert.Text = m_Translate.GetSafeTranslationValue(lblAVAMAktualisiert.Text)

			lblvakanzals.Text = m_Translate.GetSafeTranslationValue(lblvakanzals.Text)
			lblwerbeschlagwort.Text = m_Translate.GetSafeTranslationValue(lblwerbeschlagwort.Text)
			lblgruppe.Text = m_Translate.GetSafeTranslationValue(lblgruppe.Text)
			lblSBN2000.Text = m_Translate.GetSafeTranslationValue(lblSBN2000.Text)

			grpKundendaten.Text = m_Translate.GetSafeTranslationValue(grpKundendaten.Text)
			lblkundennr.Text = m_Translate.GetSafeTranslationValue(lblkundennr.Text)
			lblZHDName.Text = m_Translate.GetSafeTranslationValue(lblZHDName.Text)
			lblkundenlink.Text = m_Translate.GetSafeTranslationValue(lblkundenlink.Text)
			lblArbeitPLZ.Text = m_Translate.GetSafeTranslationValue(lblArbeitPLZ.Text)
			lblArbeitsort.Text = m_Translate.GetSafeTranslationValue(lblArbeitsort.Text)

			grplohn.Text = m_Translate.GetSafeTranslationValue(grplohn.Text)
			lbllohn.Text = m_Translate.GetSafeTranslationValue(lbllohn.Text)
			lblArbeitszeit.Text = m_Translate.GetSafeTranslationValue(lblArbeitszeit.Text)

			grpFiliale.Text = m_Translate.GetSafeTranslationValue(grpFiliale.Text)
			lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
			lblBerater.Text = m_Translate.GetSafeTranslationValue(lblBerater.Text)
			lblFiliale.Text = m_Translate.GetSafeTranslationValue(lblFiliale.Text)
			grpkontakt.Text = m_Translate.GetSafeTranslationValue(grpkontakt.Text)
			lblkontakt.Text = m_Translate.GetSafeTranslationValue(lblkontakt.Text)
			lblstatus.Text = m_Translate.GetSafeTranslationValue(lblstatus.Text)

			grpBemerkung.Text = m_Translate.GetSafeTranslationValue(grpBemerkung.Text)

			bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
			bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsiLblGeaendert.Caption)
			bsiCreated.Caption = String.Empty
			bsiChanged.Caption = String.Empty

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.ToString))
		End Try

	End Sub

	Private Sub frmVakanzen_Disposed(sender As Object, e As System.EventArgs) Handles MyBase.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.UcTinyMCE1.Dispose()
			HidePopups()

			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_VACANCIES_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccAllgemein_P1.SplitterPosition)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_TEXTPAGESPLITTER, Me.sccAllgemein_P2_Tab2_0.SplitterPosition)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub frmVakanzen_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 705, m_InitializationData.MDData.MDNr)

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_VACANCIES_FORM_LOCATION)

			Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)
			Dim setting_form_textpathsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_TEXTPAGESPLITTER)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			If setting_form_mainsplitter > 0 Then Me.sccAllgemein_P1.SplitterPosition = Math.Max(Me.sccAllgemein_P1.SplitterPosition, setting_form_mainsplitter)
			If setting_form_textpathsplitter > 0 Then Me.sccAllgemein_P2_Tab2_0.SplitterPosition = Math.Max(Me.sccAllgemein_P2_Tab2_0.SplitterPosition, setting_form_textpathsplitter)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Private Function LoadVacancyData() As Boolean
		Dim result As Boolean = True

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Dim allowedChangeMandant = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 200002, m_InitializationData.MDData.MDNr)

			Me.lueMandant.Enabled = allowedChangeMandant
			Me.lblMDName.Enabled = allowedChangeMandant

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try


		If VacancySetting.SelectedVakNr.GetValueOrDefault(0) > 0 Then
			LoadAssignedVacancyData()
			If m_currentVacancyData Is Nothing Then Return False

			DispalyVacancyDetails()

		Else
			m_currentVacancyData = New VacancyMasterData With {.VakNr = m_currentVacancyNumber}

			lueAdvisor.EditValue = m_InitializationData.UserData.UserKST
			lueBusinessBranches.EditValue = m_InitializationData.UserData.UserFiliale

			' with selected customer
			Dim customerNumber As Integer
			If Integer.TryParse(VacancySetting.SelectedKDNr, customerNumber) Then
				lueCustomer.EditValue = customerNumber
				LoadResponsiblePersonsDropDownData(lueCustomer.EditValue)

				grpKundendaten.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde: {0}"), customerNumber)
			End If
			' and selected responsible
			Dim CPersonNumber As Integer
			If Integer.TryParse(VacancySetting.SelectedZHDNr, CPersonNumber) Then
				lueZHD.EditValue = CPersonNumber
			End If

		End If
		grpvakanz.Text = String.Format(m_Translate.GetSafeTranslationValue("Vakanz: {0}"), If(m_currentVacancyNumber = 0,
																	 m_Translate.GetSafeTranslationValue("Neu"), m_currentVacancyNumber))
		If VacancySetting.IsAsDuplicated.GetValueOrDefault(False) Then m_UtilityUI.ShowCustomBadgeNotification(xtabMain, m_Badge, AdornerUIManager1,
																										m_Translate.GetSafeTranslationValue("Der Datensatz wurde erfolgreich dupliziert."),
																										New System.Windows.Forms.Padding(3), New System.Windows.Forms.Padding(14), ContentAlignment.TopCenter, TargetElementRegion.Default,
																										BadgePaintStyle.Information, Nothing, Nothing)


		LoadLogoDataForTransfer()

		m_SuppressUIEvents = False


		Return result

	End Function

	Private Sub DispalyVacancyDetails()

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		txt_Bezeichnung.EditValue = m_currentVacancyData.Bezeichnung
		lblBezeichnung.Text = String.Format("<b>{0}</b>", m_currentVacancyData.Bezeichnung)

		txt_Slogan.Text = m_currentVacancyData.Slogan
		lueGroup.EditValue = m_currentVacancyData.Gruppe
		Dim success = LoadAssignedVacanciesSubGroupDropDownData(lueGroup.EditValue, m_InitializationData.UserData.UserLanguage)
		lueSubGroup.EditValue = m_currentVacancyData.SubGroup

		If Year(m_currentVacancyData.CreatedOn) >= 2021 Then
			LoadAVAM2021OccupationsDropDownData()

		ElseIf Year(m_currentVacancyData.CreatedOn) = 2020 Then
			LoadAVAM2020OccupationsDropDownData()

		Else
			LoadAVAMOccupationsDropDownData()

		End If
		lueSBNNumber.EditValue = m_currentVacancyData.SBNNumber

		lueMandant.EditValue = m_currentVacancyData.MDNr
		lueAdvisor.EditValue = m_currentVacancyData.Berater

		Dim userData As AdvisorViewData = TryCast(Me.lueAdvisor.GetSelectedDataRow(), AdvisorViewData)
		If userData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Der gesuchte Berater existiert nicht mehr! Ich werde den Berater durch Ihre Daten ersetzen. Der ursprüngliche Berater lautet: {0}"), m_currentVacancyData.Berater))

			lueAdvisor.EditValue = m_InitializationData.UserData.UserKST
			userData = TryCast(Me.lueAdvisor.GetSelectedDataRow(), AdvisorViewData)
			If userData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Benutzerdatenbank."))
			End If
		End If

		Dim m_ini = ChangeMandantData(lueMandant.EditValue, userData.UserNumber)
		m_InitialChangedData = New SP.Infrastructure.Initialization.InitializeClass(m_ini.TranslationData, m_ini.ProsonalizedData, m_ini.MDData, m_ini.UserData)
		m_InitializationChangedData = m_InitialChangedData

		lueBusinessBranches.EditValue = m_currentVacancyData.Filiale

		lueKontakt.EditValue = m_currentVacancyData.VakKontakt_Value
		lueStatus.EditValue = m_currentVacancyData.VakState_Value

		grpKundendaten.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde: {0}"), m_currentVacancyData.KDNr)
		LoadResponsiblePersonsDropDownData(m_currentVacancyData.KDNr)


		lueCustomer.EditValue = m_currentVacancyData.KDNr
		lueZHD.EditValue = m_currentVacancyData.KDZHDNr


		luePostcode.EditValue = m_currentVacancyData.JobPLZ
		txtLocation.EditValue = m_currentVacancyData.JobOrt

		Try
			txt_KDJobLink.EditValue = m_currentVacancyData.VakLink
			txt_Lohn.EditValue = m_currentVacancyData.MALohn
			txt_Arbzeit.EditValue = m_currentVacancyData.Jobtime
			txt_Bemerkung.EditValue = m_currentVacancyData.Bemerkung

			'lblkundennr.Enabled = ClsDataDetail.bAllowedToChange

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try
		bsiCreated.Caption = String.Format(" {0:f}, {1}", m_currentVacancyData.CreatedOn, m_currentVacancyData.CreatedFrom)
		bsiChanged.Caption = String.Format(" {0:f}, {1}", m_currentVacancyData.ChangedOn, m_currentVacancyData.ChangedFrom)

		m_SuppressUIEvents = suppressUIEventsState

	End Sub

	Private Sub LoadLogoDataForTransfer()

		'peInternLogo.Visible = False

		VacancySetting.IsAllowedJCH = AllowedJobCHTransfer()
		VacancySetting.IsAllowedOstJob = AllowedOstJobTransfer()

		If m_currentVacancyData Is Nothing OrElse m_currentVacancyNumber = 0 Then Return
		SetVakState()

	End Sub

	Private Function LoadAssignedVacancyData() As Boolean

		m_currentVacancyData = m_VacancyDatabaseAccess.LoadVacancyMasterData(m_InitializationData.MDData.MDNr, m_currentVacancyNumber)
		If m_currentVacancyData Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Vakanz-Daten konnten nicht geladen werden.")

			Return False

		End If

		Return (Not m_currentVacancyData Is Nothing)
	End Function

	Private Function AllowedJobCHTransfer() As Boolean
		Dim result As Boolean = True

		result = m_VacancyDatabaseAccess.AllowedForJobCHTransfer(m_InitializationData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr)

		Return result
	End Function

	Private Function AllowedOstJobTransfer() As Boolean
		Dim result As Boolean = True

		result = m_VacancyDatabaseAccess.AllowedForOstJobTransfer(m_InitializationData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr)

		Return result
	End Function

	Sub SetVakState()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim totalCountOfTransferedIntern As Integer = 0
		Dim totalCountOfJobsCH As Integer = 0
		Dim totalCountOfJobsCHExpireSoon As Integer = 0

		Dim totalCountOfTransferedJobsCH As Integer = 0
		Dim totalCountOfOstJob As Integer = 0
		Dim totalCountOfTransferedOstJob As Integer = 0
		Dim totalCountOfOstJobExpireSoon As Integer = 0

		Dim success As Boolean = LoadAssignedVacancyData()

		If Not success Then
			Me.Close()
			Return
		End If

		If VacancySetting.IsAllowedJCH Then
			Dim result = m_VacancyDatabaseAccess.GetJobCHExportedCounterData(m_InitializationData.MDData.MDGuid, m_currentVacancyNumber, Me.lueAdvisor.EditValue)

			VacancySetting.CountTotalJCH = result.AllowedJobQuantity
			VacancySetting.CountExportedJCH = result.ExportedJobQuantity

			totalCountOfJobsCH = result.AllowedJobQuantity
			totalCountOfTransferedJobsCH = result.ExportedJobQuantity
			totalCountOfJobsCHExpireSoon = result.ExpireSoonJobQuantity

			Dim jobCHMasterData = m_VacancyDatabaseAccess.LoadJobCHMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)

			If (jobCHMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanz-Daten (JOBS.CH) konnten nicht geladen werden."))
			Else
				VacancySetting.IsJCHExported = result.IsCounterOK AndAlso jobCHMasterData.IsOnline.GetValueOrDefault(False)
			End If

		End If

		If VacancySetting.IsAllowedOstJob Then
			Dim result = m_VacancyDatabaseAccess.GetOstJobExportedCounterData(m_InitializationData.MDData.MDGuid, m_currentVacancyNumber, Me.lueAdvisor.EditValue)

			VacancySetting.CountTotalOstJob = result.AllowedJobQuantity
			VacancySetting.CountExportedOstJob = result.ExportedJobQuantity

			totalCountOfOstJob = result.AllowedJobQuantity
			totalCountOfTransferedJobsCH = result.ExportedJobQuantity
			totalCountOfOstJobExpireSoon = result.ExpireSoonJobQuantity

			Dim ostjobData = m_VacancyDatabaseAccess.LoadOstJobMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserFullName,
																																		m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
			If ostjobData Is Nothing OrElse ostjobData.VakNr Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die ostjob.ch Daten wurden nicht geladen."))

			Else
				VacancySetting.IsOstJobExported = result.IsCounterOK AndAlso ostjobData.isonline.GetValueOrDefault(False)

			End If

		End If


		If m_mandant.AllowedExportVacancy2WOS(m_InitializationData.MDData.MDNr, Now.Year) Then
			Dim countInternalVacancy = m_VacancyDatabaseAccess.GetCountOfExportedInternVacancies(m_InitializationData.MDData.MDGuid)
			Dim stmpData = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr, m_currentVacancyNumber)

			If Not m_currentVacancyData Is Nothing Then VacancySetting.IsInternExported = m_currentVacancyData.IEExport.GetValueOrDefault(False) Else VacancySetting.IsInternExported = False

			totalCountOfTransferedIntern = countInternalVacancy
			If Not stmpData Is Nothing Then
				If Not String.IsNullOrWhiteSpace(stmpData.JobroomID) Then
					tgsReportingObligation.EditValue = stmpData.ReportingObligation.GetValueOrDefault(False)
					If stmpData.ReportingObligationEndDate.HasValue Then
						lblReportingObligationEndDate.Text = String.Format("{0:d}", stmpData.ReportingObligationEndDate)
					Else
						lblReportingObligationEndDate.Text = "?"
					End If
					lblAVAMState.Text = stmpData.AVAMRecordState
					lblAVAMstellennummerEgov.Text = stmpData.stellennummerEgov
					lblReportingDate.Text = String.Format("{0:g}", stmpData.ReportingDate)
					lblReportingDate.ToolTip = String.Format("{0:g}, {1}", stmpData.ReportingDate, stmpData.ReportingFrom)

					lblAVAMSyncDate.Text = String.Format("{0:g}", stmpData.SyncDate)
					lblAVAMSyncDate.ToolTip = String.Format("{0:g}, {1}", stmpData.SyncDate, stmpData.SyncFrom)

					RefreshAVAMState()
					pnlAVAM.Visible = True
				Else
					pnlAVAM.Visible = False
				End If

			End If

		Else
			pnlAVAM.Visible = False

		End If

		Dim jobplattforms As New List(Of JobPlattformsInfoData)

		jobplattforms.Add(New JobPlattformsInfoData With {.JobplattformLabel = "website", .IsOnline = VacancySetting.IsInternExported, .TranferedJobs = totalCountOfTransferedIntern})
		jobplattforms.Add(New JobPlattformsInfoData With {.JobplattformLabel = "jobs.ch", .IsOnline = VacancySetting.IsJCHExported, .TotalAllowedJobs = totalCountOfJobsCH, .TranferedJobs = totalCountOfTransferedJobsCH, .TotalExpireSoonJobs = totalCountOfJobsCHExpireSoon})
		jobplattforms.Add(New JobPlattformsInfoData With {.JobplattformLabel = "ostjob.ch", .IsOnline = VacancySetting.IsOstJobExported, .TotalAllowedJobs = totalCountOfOstJob, .TranferedJobs = totalCountOfTransferedOstJob, .TotalExpireSoonJobs = totalCountOfOstJobExpireSoon})
		m_JobPlattformsData = jobplattforms

		grdJobplattforms.DataSource = m_JobPlattformsData

		Dim allowedChangeMandant = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 200002, m_InitializationData.MDData.MDNr)
		lueMandant.Enabled = allowedChangeMandant AndAlso Not (VacancySetting.IsInternExported OrElse VacancySetting.IsJCHExported OrElse VacancySetting.IsOstJobExported)

	End Sub

	Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.navBarMain.Items.Clear()
		Try
			navBarMain.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupDatei As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Datei"))
			groupDatei.Name = "gNavDatei"

			Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
			New_P.Name = "New_Vakanz"
			New_P.SmallImage = Me.ImageCollection1.Images(0)

			Dim Save_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
			Save_P_Data.Name = "Save_Vakanz_Data"
			Save_P_Data.SmallImage = Me.ImageCollection1.Images(1)
			Save_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 702)

			Dim Print_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
			Print_P_Data.Name = "Print_Vakanz_Data"
			Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)
			Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 704)

			Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
			Close_P_Data.Name = "Close_Vakanz_Form"
			Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

			Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
			groupDelete.Name = "gNavDelete"
			groupDelete.Appearance.ForeColor = Color.Red

			Dim Delete_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
			Delete_P_Data.Name = "Delete_Vakanz_Data"
			Delete_P_Data.SmallImage = Me.ImageCollection1.Images(4)
			Delete_P_Data.Appearance.ForeColor = Color.Red
			Delete_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 703)


			Dim groupExtras As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
			groupExtras.Name = "gNavExtras"
			groupExtras.Appearance.ForeColor = Color.Red

			Dim Duplicate_V_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vakanz duplizieren"))
			Duplicate_V_Data.Name = "Duplicate_Vakanz_Data"
			Duplicate_V_Data.SmallImage = Me.ImageCollection1.Images(14)
			Duplicate_V_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 702)


			Dim groupJobPlattform As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Job-Plattformen"))
			groupJobPlattform.Name = "gNavJobPlattform"
			Dim JCH_P_Data As New NavBarItem
			Dim JW_P_Data As New NavBarItem

			JCH_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Plattform Einstellungen"))
			JCH_P_Data.Name = "JCH_Zusatz_Setting"
			JCH_P_Data.SmallImage = Me.ImageCollection1.Images(13)

			Try
				navBarMain.BeginUpdate()

				navBarMain.Groups.Add(groupDatei)
				groupDatei.ItemLinks.Add(New_P)
				groupDatei.ItemLinks.Add(Save_P_Data)
				groupDatei.ItemLinks.Add(Print_P_Data)
				groupDatei.ItemLinks.Add(Close_P_Data)
				groupDatei.Expanded = True

				navBarMain.Groups.Add(groupDelete)
				groupDelete.ItemLinks.Add(Delete_P_Data)
				groupDelete.Expanded = False

				navBarMain.Groups.Add(groupExtras)
				groupExtras.ItemLinks.Add(Duplicate_V_Data)
				groupExtras.Expanded = True

				navBarMain.Groups.Add(groupJobPlattform)
				groupJobPlattform.ItemLinks.Add(JCH_P_Data)
				groupJobPlattform.Expanded = True

				navBarMain.EndUpdate()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				MsgBox(String.Format("Fehler (navBarMain): {0}", ex.ToString), MsgBoxStyle.Critical, "Navigationsleiste")

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			MsgBox(String.Format("Fehler (navBarMain): {0}", ex.ToString), MsgBoxStyle.Critical, "Navigationsleiste")

		End Try

	End Sub

	Sub BuildVakZusatzFields()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim liLLAbschnit As New List(Of VacancyZusatzMenuData)

		' Vak_ZusatzData
		Try
			liLLAbschnit = m_VacancyDatabaseAccess.LoadVacancyZusatzMenuInfoData(m_InitializationData.MDData.MDGuid, ExternalPlattforms.INTERNAL) ' ListVakZusatz4Vak_Zusatz()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Interne Jobplattform: Einträge auflisten. {1}", strMethodeName, ex.ToString))

		End Try

		Dim groupLocal As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Interne Felder"))
		groupLocal.Name = "gNavVakZusatzFelder"

		Dim itemMA As New NavBarItem
		navBarRtf.BeginUpdate()
		navBarRtf.Groups.Add(groupLocal)

		For Each itm In liLLAbschnit
			'Dim strText As String() = liLLAbschnit(i).ToString.Split(CChar("#"))
			' Create an Inbox item and assign an image from the SmallImages list to the item.
			itemMA = New NavBarItem(m_Translate.GetSafeTranslationValue(itm.Bezeichnung))
			itemMA.Name = String.Format("V_Zusatz_{0}", itm.DBFieldName)

			groupLocal.ItemLinks.Add(itemMA)

		Next
		groupLocal.Expanded = False

		Try
			liLLAbschnit = m_VacancyDatabaseAccess.LoadVacancyZusatzMenuInfoData(m_InitializationData.MDData.MDGuid, ExternalPlattforms.JOBSCH) ' ListVakZusatz4Jobs_CH()
			If liLLAbschnit.Count > 0 Then
				liLLAbschnit.Add(New VacancyZusatzMenuData With {.Bezeichnung = m_Translate.GetSafeTranslationValue("Vorschau"),
												 .GroupNr = 1, .DBFieldName = "J_JobsCH_Vorschau"})
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Job.ch: Einträge auflisten. {1}", strMethodeName, ex.ToString))

		End Try

		groupLocal = New NavBarGroup(m_Translate.GetSafeTranslationValue("Jobplattformen"))
		groupLocal.Name = "gNavJZusatzFelder"

		itemMA = New NavBarItem
		navBarRtf.Groups.Add(groupLocal)

		For Each itm In liLLAbschnit
			'Dim strText As String() = liLLAbschnit(i).ToString.Split(CChar("#"))
			itemMA = New NavBarItem(m_Translate.GetSafeTranslationValue(itm.Bezeichnung))
			If Not itm.DBFieldName.ToLower.Contains("vorschau") Then
				itemMA.Name = String.Format("J_Zusatz_{0}", itm.DBFieldName)
			Else
				itemMA.Name = String.Format("{0}", itm.DBFieldName)
			End If

			groupLocal.ItemLinks.Add(itemMA)

		Next
		groupLocal.Expanded = True
		navBarRtf.EndUpdate()

	End Sub


#Region "Funktionen zum Füllen der Comboboxen..."

	Sub FillMyListing(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal liEntry As List(Of String))

		For i As Integer = 0 To liEntry.Count - 1
			cbo.Properties.Items.Add(m_Translate.GetSafeTranslationValue(liEntry.Item(i).ToString))
		Next

	End Sub

	'Private Sub cbo_Region_DropDown(ByVal sender As Object, ByVal e As System.EventArgs)
	'  Dim liResult As New List(Of String)

	'  'If Me.cbo_Region.Items.Count > 0 Then Exit Sub
	'  liResult = ListLocalRegion()
	'  'FillMyListing(Me.cbo_Region, liResult)

	'End Sub

#End Region

	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is DevExpress.XtraEditors.LookUpEdit Then
				Dim lookupEdit As DevExpress.XtraEditors.LookUpEdit = CType(sender, DevExpress.XtraEditors.LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DevExpress.XtraEditors.GridLookUpEdit Then
				Dim lookupEdit As DevExpress.XtraEditors.GridLookUpEdit = CType(sender, DevExpress.XtraEditors.GridLookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DevExpress.XtraEditors.DateEdit Then
				Dim dateEdit As DevExpress.XtraEditors.DateEdit = CType(sender, DevExpress.XtraEditors.DateEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub

	Private Sub OngvJobplattforms_CustomDrawCell(sender As Object, e As RowCellCustomDrawEventArgs) Handles gvJobplattforms.CustomDrawCell
		If e.Column.FieldName = "TotalExpireSoonJobs" Then
			Dim warningImage As Image = My.Resources.bullet_ball_yellow ' Image.("c:\warningImage.png")
			e.DefaultDraw()
			If Convert.ToInt32(e.CellValue) > 0 Then

				'e.Cache.FillRectangle(Color.Salmon, e.Bounds)
				'e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds)
				'e.Handled = True
				Dim cellInfo As GridCellInfo = CType(e.Cell, GridCellInfo)
				Dim viewInfo As TextEditViewInfo = CType(cellInfo.ViewInfo, TextEditViewInfo)
				Dim imageRect As Rectangle = viewInfo.ContextImageBounds
				imageRect.X = e.Bounds.X + e.Bounds.Width / 2 - imageRect.Width / 2
				imageRect.Y = e.Bounds.Y + e.Bounds.Height / 2 - imageRect.Height / 2
				'e.Appearance.FillRectangle(e.Cache, e.Bounds)
				'e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds)
				'e.Cache.DrawImage(warningImage, imageRect)
				'e.Handled = True

				e.Cache.DrawImage(warningImage, imageRect.X, e.Bounds.Y)
			End If
		End If
	End Sub

	Private Sub OngvJobplattforms_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvJobplattforms.CustomColumnDisplayText

		If e.Column.FieldName = "TotalExpireSoonJobs" OrElse e.Column.FieldName = "TranferedJobs" OrElse e.Column.FieldName = "TotalAllowedJobs" OrElse e.Column.FieldName = "TotalOpenJobs" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	'Private Sub OngvJobplattforms_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvJobplattforms.RowStyle

	'	'If Not m_ShowcustomerinColor Then Return
	'	If e.RowHandle >= 0 Then

	'		Dim rowData = CType(gvJobplattforms.GetRow(e.RowHandle), JobPlattformsInfoData)

	'		If rowData.TotalExpireSoonJobs > 0 Then
	'			e.Appearance.ForeColor = ColorTranslator.FromWin32(rowData.customerfproperty)
	'		End If

	'	End If

	'End Sub


	Sub OpenTemplateMnuValue(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMenuName As String = e.Item.AccessibleName.ToString
		If strMenuName = String.Empty Then Exit Sub
		Dim strTemplateFilename As String = String.Format("{0}{1}", m_mandant.GetPath4Templates(m_InitializationData.MDData.MDNr, Now.Year), strMenuName)
		Dim bOpenTemplate As Boolean = File.Exists(strTemplateFilename)

		Dim strNewTemplateFile As String = m_path.GetSpS2DeleteHomeFolder & String.Format("tpl_Vak_{0}", strMenuName)
		File.Copy(strTemplateFilename, strNewTemplateFile, True)
		'Me.rtfContent.LoadDocument(strNewTemplateFile, DevExpress.XtraRichEdit.DocumentFormat.Html)

		'Me.FileSaveItem1.Enabled = True
		Me.bChangedContent = True

	End Sub

	Sub CloseForm()

		Try
			Try

				'My.Settings.iHeight = Me.Height
				'My.Settings.iWidth = Me.Width
				'My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

				'My.Settings.Save()

			Catch ex As Exception

			End Try

			Me.Close()
			'Me.Dispose()

		Catch ex As Exception

		End Try

	End Sub

	Sub NewVakData()

		m_currentVacancyData = New VacancyMasterData With {.VakNr = Nothing}
		m_currentVacancyNumber = 0

		lblBezeichnung.Text = String.Empty

		txt_Bezeichnung.Text = String.Empty
		txt_Slogan.Text = String.Empty
		lueBusinessBranches.EditValue = Nothing

		txt_KDJobLink.Text = String.Empty
		txt_Lohn.Text = String.Empty
		txt_Arbzeit.Text = String.Empty
		luePostcode.EditValue = Nothing
		txtLocation.Text = String.Empty
		txt_Bemerkung.Text = String.Empty
		lueKontakt.EditValue = Nothing
		lueStatus.EditValue = Nothing

		VacancySetting.SelectedKDNr = 0
		VacancySetting.SelectedZHDNr = 0

		ResetAllTabEntries()

		'Reset()
		'LoadDropDownData()

		'ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
		'ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		lueAdvisor.Enabled = True
		lueAdvisor.EditValue = m_InitializationData.UserData.UserKST
		lueBusinessBranches.Enabled = True

		xtabJobCH.PageEnabled = False
		txt_Bezeichnung.Focus()

	End Sub


#Region "Funktionen zum Reseten der Daten..."

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()

		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.xtabMain.Name).Controls
				For Each ctrls In tabPg.Controls
					ResetControl(ctrls)
				Next
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

		If TypeOf (con) Is DevExpress.XtraEditors.LookUpEdit Then
			Dim c As DevExpress.XtraEditors.LookUpEdit = CType(con, DevExpress.XtraEditors.LookUpEdit)
			c.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
			Dim c As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
			c.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim c As DevExpress.XtraEditors.ComboBoxEdit = CType(con, DevExpress.XtraEditors.ComboBoxEdit)
			c.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
			Dim c As DevExpress.XtraEditors.DateEdit = CType(con, DevExpress.XtraEditors.DateEdit)
			c.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
			Dim c As DevExpress.XtraEditors.CheckEdit = CType(con, DevExpress.XtraEditors.CheckEdit)
			c.Checked = False

		ElseIf TypeOf (con) Is CheckBox Then
			Dim cbo As CheckBox = con
			cbo.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is ListBox Then
			Dim lst As ListBox = con
			lst.Items.Clear()

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

	End Sub

#End Region

#Region "Funktionen zum Speichern der Daten..."

	Private Function SaveSelectedVacancy(ByVal showMsg As Boolean) As Boolean
		Dim success As Boolean = True
		Dim newRec As Boolean = m_currentVacancyNumber = 0
		Dim userKontakt As String = String.Empty
		Dim userEmail As String = String.Empty

		success = success AndAlso ValidateData()
		If Not success Then Return False

		If newRec Then
			Dim vacancyNumberOffsetFromSettings As Integer = ReadVacancyOffsetFromSettings()
			m_currentVacancyData.VacancyNumberOffset = vacancyNumberOffsetFromSettings

		Else
			success = success AndAlso LoadAssignedVacancyData()
			If Not success Then Return False

			m_currentVacancyData.VakNr = m_currentVacancyNumber

			userKontakt = m_currentVacancyData.UserKontakt
			userEmail = m_currentVacancyData.UserEMail
		End If

		If String.IsNullOrWhiteSpace(userKontakt) Then userKontakt = TranslateUserInfo()
		If String.IsNullOrWhiteSpace(userKontakt) Then
			If m_InitializationChangedData Is Nothing Then m_InitializationChangedData = m_InitializationData
			userKontakt = m_InitializationChangedData.UserData.UserFullName
			userKontakt &= String.Format("{0}{1}<br />", "<br />", m_InitializationChangedData.UserData.UserMDName)
			userKontakt &= String.Format("{0}{1}<br />", "", m_InitializationChangedData.UserData.UserMDStrasse)
			userKontakt &= String.Format("{0}{1} {2}<br />", "", m_InitializationChangedData.UserData.UserMDPLZ, m_InitializationChangedData.UserData.UserMDOrt)
			userKontakt &= String.Format("{0}{1}<br />", "", m_InitializationChangedData.UserData.UserMDTelefon)
			userKontakt &= String.Format("{0}{1}<br />", "", m_InitializationChangedData.UserData.UserMDTelefax)
			userKontakt &= String.Format("{0}{1}<br />", "", m_InitializationChangedData.UserData.UserMDeMail)
			userKontakt &= String.Format("{0}{1}", "", m_InitializationChangedData.UserData.UserMDHomepage)
		End If
		If String.IsNullOrWhiteSpace(userEmail) Then userEmail = m_InitializationChangedData.UserData.UserMDeMail

		m_currentVacancyData.MDNr = lueMandant.EditValue
		m_currentVacancyData.Customer_Guid = m_InitializationData.MDData.MDGuid
		m_currentVacancyData.Bezeichnung = txt_Bezeichnung.EditValue
		m_currentVacancyData.Slogan = txt_Slogan.EditValue
		m_currentVacancyData.Gruppe = lueGroup.EditValue
		m_currentVacancyData.SubGroup = lueSubGroup.EditValue
		m_currentVacancyData.SBNNumber = CType(lueSBNNumber.EditValue, Integer)
		m_currentVacancyData.KDNr = CType(lueCustomer.EditValue, Integer)
		m_currentVacancyData.KDZHDNr = CType(lueZHD.EditValue, Integer)
		m_currentVacancyData.ExistLink = Not String.IsNullOrWhiteSpace(txt_KDJobLink.EditValue)
		m_currentVacancyData.VakLink = txt_KDJobLink.EditValue
		m_currentVacancyData.Berater = lueAdvisor.EditValue
		m_currentVacancyData.Filiale = lueBusinessBranches.EditValue
		m_currentVacancyData.VakKontakt = lueKontakt.EditValue
		m_currentVacancyData.VakState = lueStatus.EditValue
		m_currentVacancyData.MALohn = txt_Lohn.EditValue
		m_currentVacancyData.Jobtime = txt_Arbzeit.EditValue
		m_currentVacancyData.JobPLZ = luePostcode.EditValue
		m_currentVacancyData.JobOrt = txtLocation.EditValue
		m_currentVacancyData.Bemerkung = txt_Bemerkung.EditValue

		m_currentVacancyData.UserKontakt = userKontakt
		m_currentVacancyData.UserEMail = userEmail


		If newRec Then
			m_currentVacancyData.CreatedFrom = m_InitializationData.UserData.UserFullNameWithComma
			m_currentVacancyData.CreatedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_VacancyDatabaseAccess.AddNewVacancy(m_currentVacancyData)
		Else
			m_currentVacancyData.ChangedFrom = m_InitializationData.UserData.UserFullNameWithComma
			m_currentVacancyData.ChangedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyMasterData(m_currentVacancyData)
		End If

		If success Then
			If showMsg Then m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Vakanz wurden gespeichert."))
			bsiCreated.Caption = String.Format(" {0:f}, {1}", m_currentVacancyData.CreatedOn, m_currentVacancyData.CreatedFrom)
			bsiChanged.Caption = String.Format(" {0:f}, {1}", m_currentVacancyData.ChangedOn, m_currentVacancyData.ChangedFrom)
		Else
			m_currentVacancyData.VakNr = Nothing
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Vakanz konnte nicht gespeichert werden."))

		End If
		m_currentVacancyNumber = m_currentVacancyData.VakNr

		If m_currentVacancyNumber > 0 Then Me.xtabJobCH.PageEnabled = True
		Me.pcEditor.Enabled = True
		grpvakanz.Text = String.Format(m_Translate.GetSafeTranslationValue("Vakanz: {0}"), m_currentVacancyNumber)


		Return success

	End Function

	Private Function DuplicateSelectedVacancy() As Boolean
		Dim success As Boolean = True
		Dim newRec As Boolean = True
		Dim userKontakt As String = String.Empty
		Dim userEmail As String = String.Empty

		Dim msg As String
		msg = "Hiermit erstellen Sie eine neue Kopie von Ihren Vakanzendaten.{0}Sind Sie sicher?"
		If m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine), m_Translate.GetSafeTranslationValue("Daten kopieren?")) = False Then Return False

		Dim oldVacancyNumber As Integer = m_currentVacancyNumber
		m_currentVacancyData = m_VacancyDatabaseAccess.LoadVacancyMasterData(m_currentVacancyData.MDNr, oldVacancyNumber)

		success = success AndAlso ValidateData()
		If Not success Then Return False

		Dim vacancyNumberOffsetFromSettings As Integer = ReadVacancyOffsetFromSettings()
		m_currentVacancyData.VacancyNumberOffset = vacancyNumberOffsetFromSettings

		m_currentVacancyData.CreatedFrom = m_InitializationData.UserData.UserFullNameWithComma
		m_currentVacancyData.CreatedUserNumber = m_InitializationData.UserData.UserNr
		Dim thisYear = Year(Now.Date)

		If m_currentVacancyData.SBNNumber.GetValueOrDefault(0) > 0 AndAlso Year(m_currentVacancyData.CreatedOn) <= 2019 AndAlso thisYear >= 2020 Then
			Dim newAVAMData = LoadAVAM2020OccupationsData(Year(m_currentVacancyData.CreatedOn))
			Dim mappingData = LoadSBNumberMappingData(m_currentVacancyData.SBNNumber.GetValueOrDefault(0))
			Dim newSBNNumber As Integer = 0

			If Not mappingData Is Nothing AndAlso mappingData.Count > 0 Then
				newSBNNumber = mappingData(0).New_AVAMNumber.GetValueOrDefault(0)
			End If

			If newSBNNumber = 0 AndAlso m_currentVacancyData.SBNNumber.GetValueOrDefault(0) > 0 Then
				msg = "<b>Achtung:</b><br>Dieser Vakanz ist nicht mehr ab 2020 Meldepflichtig. Daher wir der Beruf nicht mehr übernommen!"
			End If
			If Not newAVAMData Is Nothing AndAlso newSBNNumber > 0 Then
				Dim searchData = newAVAMData.Where(Function(x) x.TitleNumber = newSBNNumber).FirstOrDefault
				If Not searchData Is Nothing Then

					If searchData.Notifiable Then
						msg = String.Format(m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Der alte Meldepflichtige Berufscode {0} wurde durch {1} ersetzt."), m_currentVacancyData.SBNNumber, searchData.TitleNumber)
						newSBNNumber = searchData.TitleNumber
					Else
						msg = String.Format(m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Der alte Meldepflichtige Berufscode {0} ist nicht mehr meldepflichtig und wird daher gelöscht."), m_currentVacancyData.SBNNumber)
						newSBNNumber = 0
					End If

				Else
					msg = String.Format(m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Der alte Meldepflichtige Berufscode {0} wurde nicht gefunden, und wird daher gelöscht."), m_currentVacancyData.SBNNumber)
					newSBNNumber = 0
				End If

			Else
				msg = String.Format(m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Der alte Meldepflichtige Berufscode {0} wurde nicht gefunden, und wird daher gelöscht."), m_currentVacancyData.SBNNumber)
				newSBNNumber = 0
			End If

			m_UtilityUI.ShowInfoDialog(msg)

			m_currentVacancyData.SBNNumber = newSBNNumber
		End If

		success = success AndAlso m_VacancyDatabaseAccess.DuplicateVacancyData(lueMandant.EditValue, oldVacancyNumber, m_currentVacancyData)

		If success Then
			msg = "Ihre Vakanz wurde erfolgreich dupliziert.{0}Bitte kontrollieren Sie die Start und End-Datum der Publikationen für die Job-Plattformen!"
			m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine))

			Dim frmVacancy = New SPKD.Vakanz.frmVakanzen(m_InitializationData)
			Dim setting = New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = m_currentVacancyData.VakNr, .SelectedKDNr = m_currentVacancyData.KDNr, .SelectedZHDNr = m_currentVacancyData.KDZHDNr, .IsAsDuplicated = True}
			frmVacancy.VacancySetting = setting
			If Not frmVacancy.LoadData Then Close()

			frmVacancy.Show()
			frmVacancy.BringToFront()

		Else
			m_currentVacancyData.VakNr = Nothing
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Vakanz konnte nicht dupliziert werden."))

		End If

		Close()


		Return success

	End Function


#End Region

#Region "Navigationsleiste..."

	Private Sub nbMain_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navBarMain.LinkClicked
		Dim bForDesign As Boolean = False
		'Me.rtfContent.HtmlText = String.Empty
		Me.UcTinyMCE1.HtmlContent = String.Empty
		Try
			Trace.WriteLine(String.Format("{0} >>> {1}", e.Link.ItemName, e.Link.Caption))
			strLinkName = e.Link.ItemName
			strLinkCaption = e.Link.Caption

			For i As Integer = 0 To Me.navBarMain.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			If Not strLinkName.ToLower.Contains("save") And
							Not strLinkName.ToLower.Contains("delete") And
							Not strLinkName.ToLower.Contains("print") And
							Not strLinkName.ToLower.Contains("new") Then e.Link.Item.Appearance.ForeColor = Color.Orange

			Select Case strLinkName.ToLower
				Case "New_Vakanz".ToLower
					NewVakData()

				Case "Save_Vakanz_Data".ToLower
					SaveSelectedVacancy(True)

				Case "Print_Vakanz_Data".ToLower
					Me.SQL4Print = "[Get SelectedVakData For Print Stammblatt]"
					Me.PrintJobNr = "19.0.1"

					StartPrinting()


				Case "Close_Vakanz_Form".ToLower
					CloseForm()
					Return

				Case "delete_Vakanz_Data".ToLower
					If DeleteAssignedVacancy() Then
						CloseForm()
						Return
					End If

				Case "Duplicate_Vakanz_Data".ToLower
					AdornerUIManager1.Elements.Remove(m_Badge)
					AdornerUIManager1.Hide()

					DuplicateSelectedVacancy()
					Return

				Case "JCH_Zusatz_Setting".ToLower
					If SaveSelectedVacancy(False) Then If m_currentVacancyNumber > 0 AndAlso ValidateJobPlattformData() Then ShowJobsCHProperties()


					'Case "JW_Zusatz_Setting".ToLower
					'	ShowJobsCHProperties()

					'Case "P_MakePropose".ToLower

					'Case "P_MakeES".ToLower


				Case Else


			End Select


		Catch ex As Exception
			If m_InitializationData.UserData.UserNr = 1 Then DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Linkclicked",
																												MessageBoxButtons.OK, MessageBoxIcon.Error)

		Finally

		End Try

	End Sub

	Private Function DeleteAssignedVacancy() As Boolean
		Dim success As Boolean = True
		Dim msg As String

		Dim jobCHMasterData = m_VacancyDatabaseAccess.LoadJobCHMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		If jobCHMasterData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanz-Daten (JOBS.CH) konnten nicht geladen werden."))

			Return False
		End If
		Dim ostjobData = m_VacancyDatabaseAccess.LoadOstJobMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserFullName,
																																		m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		If ostjobData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die ostjob.ch Daten wurden nicht geladen."))

			Return False
		End If

		If m_currentVacancyData.IEExport.GetValueOrDefault(False) OrElse jobCHMasterData.IsOnline.GetValueOrDefault(False) OrElse ostjobData.isonline.GetValueOrDefault(False) Then
			msg = m_Translate.GetSafeTranslationValue("Sie haben diesen Datensatz bereits veröffentlicht. Wenn Sie den Datensatz löschen möchten, müssen Sie ihn zuerst offline stellen.")
			m_UtilityUI.ShowInfoDialog(msg)

			Return False
		End If

		Dim StmpData = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		If Not StmpData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(StmpData.JobroomID) AndAlso Not (StmpData.AVAMStateEnum = SP.DatabaseAccess.Vacancy.DataObjects.AVAMState.PUBLISHED_PUBLIC OrElse StmpData.AVAMStateEnum = SP.DatabaseAccess.Vacancy.DataObjects.AVAMState.PUBLISHED_RESTRICTED) Then
			msg = m_Translate.GetSafeTranslationValue("Der Datensatz kann nicht gelöscht werden. Bitte deaktivieren Sie die Vakanz von der AVAM-Jobportal.")
			m_UtilityUI.ShowErrorDialog(msg, m_Translate.GetSafeTranslationValue("Datensatz löschen"))
			success = False
		End If

		msg = m_Translate.GetSafeTranslationValue("Der Datensatz wird gelöscht. Möchten Sie wirklich diesen Datensatz löschen?")
		If m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False Then Return False

		success = success AndAlso m_VacancyDatabaseAccess.DeleteVacancyData(m_currentVacancyData.VakNr, m_InitializationData.UserData.UserNr)
		If success Then
			msg = "Der Datensatz wurde erfolgreich gelöscht."
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))

		Else
			msg = "Die Vakanz konnte nicht gelöscht werden."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

		End If


		Return success

	End Function

	Sub StartPrinting()
		Dim MyNetwork = New Microsoft.VisualBasic.Devices.Network
		Dim MyComputer = New Microsoft.VisualBasic.Devices.Computer
		Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (MyComputer.Keyboard.CtrlKeyDown AndAlso MyComputer.Keyboard.ShiftKeyDown)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLVakSearchPrintSetting With {.DbConnString2Open = m_connectionString,
																																									 .SQL2Open = Me.SQL4Print,
																																									 .JobNr2Print = Me.PrintJobNr,
																																									 .VakNr2Print = m_currentVacancyNumber,
																																									 .ListFilterBez = New List(Of String)(New String() {"",
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}),
																																									 .PerosonalizedData = m_InitializationChangedData.ProsonalizedData,
																																									 .TranslationData = m_InitializationChangedData.TranslationData,
																																									 .SelectedMDNr = m_InitializationChangedData.MDData.MDNr,
																																									 .LogedUSNr = m_InitializationChangedData.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.VakSearchListing.ClsPrintVakSearchList(_Setting)
		Dim strResult As String = obj.PrintVakTpl_1(ShowDesign, m_currentVacancyNumber)

	End Sub

	Function SavertfContent() As Boolean

		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 702) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Rechte um Daten zu sichern."))

			Return False
		End If

		If CBool(CStr(strLinkName.ToLower.StartsWith("J_Zusatz_Jobs_".ToLower))) Then
			Me.SaveSelectedJobCHDbField(Me.strLinkName)

		ElseIf CBool(CStr(strLinkName.ToLower.StartsWith("V_Zusatz_".ToLower))) Then
			Me.SaveSelectedVakZusatzDbField(Me.strLinkName)

		End If

	End Function

	Private Sub navBarRtf_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navBarRtf.LinkClicked

		If m_currentVacancyNumber = 0 Then
			m_Logger.LogWarning("currentvacancynumber is null!!!")

			Return
		End If
		SavertfContent()
		Me.UcTinyMCE1.HtmlContent = String.Empty
		Try
			Trace.WriteLine(String.Format("{0} >>> {1}", e.Link.ItemName, e.Link.Caption))
			strLinkName = e.Link.ItemName
			strLinkCaption = e.Link.Caption

			For i As Integer = 0 To Me.navBarRtf.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange

			Select Case strLinkName.ToLower
				Case "V_Zusatz_KDBeschreibung".ToLower, "V_Zusatz_KDBietet".ToLower,
					"V_Zusatz_SBeschreibung".ToLower, "V_Zusatz_Reserve1".ToLower,
					"V_Zusatz_Taetigkeit".ToLower, "V_Zusatz_Anforderung".ToLower,
					"V_Zusatz_Reserve2".ToLower, "V_Zusatz_Reserve3".ToLower,
					"V_Zusatz_Ausbildung".ToLower, "V_Zusatz_Weiterbildung".ToLower,
					"V_Zusatz_SKennt".ToLower, "V_Zusatz_EDVKennt".ToLower

					Me.pcEditor.Visible = True
					DisplaySelectedVakZusatzDbField(strLinkName)
					bChangedContent = False

				Case "J_JobsCH_Vorschau".ToLower
					Me.pcEditor.Visible = True
					DisplayJobCHVorschau()
					bChangedContent = False

				Case "J_Zusatz_Jobs_Vorspann".ToLower
					Me.pcEditor.Visible = True
					DisplaySelectedJobCHDbField(strLinkName)
					bChangedContent = False

				Case "J_Zusatz_Jobs_Aufgabe".ToLower
					Me.pcEditor.Visible = True
					DisplaySelectedJobCHDbField(strLinkName)
					bChangedContent = False

				Case "J_Zusatz_Jobs_Anforderung".ToLower
					Me.pcEditor.Visible = True
					DisplaySelectedJobCHDbField(strLinkName)

					bChangedContent = False

				Case "J_Zusatz_Jobs_wirbieten".ToLower
					Me.pcEditor.Visible = True
					DisplaySelectedJobCHDbField(strLinkName)

					bChangedContent = False


					'Case "JW_Zusatz_JobWinner_Headline".ToLower
					'	Me.pcEditor.Visible = True
					'	DisplaySelectedJobWinnerDbField(strLinkName)

					'	bChangedContent = False

					'Case "JW_Zusatz_JobWinner_Aufgaben".ToLower
					'	Me.pcEditor.Visible = True
					'	DisplaySelectedJobWinnerDbField(strLinkName)
					'	bChangedContent = False

					'Case "JW_Zusatz_JobWinner_Profil".ToLower
					'	Me.pcEditor.Visible = True
					'	DisplaySelectedJobWinnerDbField(strLinkName)
					'	bChangedContent = False

					'Case "JW_Zusatz_JobWinner_Bemerkung".ToLower
					'	Me.pcEditor.Visible = True
					'	DisplaySelectedJobWinnerDbField(strLinkName)
					'	bChangedContent = False

			End Select


		Catch ex As Exception
			If m_InitializationData.UserData.UserNr = 1 Then DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Linkclicked",
																												MessageBoxButtons.OK, MessageBoxIcon.Error)

		Finally

		End Try

	End Sub


#End Region



	Sub DisplaySelectedVakZusatzDbField(ByVal strFieldName As String)

		If String.IsNullOrWhiteSpace(strFieldName) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurden keine Feldnamen für die Einträge ermittelt."))
		End If

		Dim strFieldValue As String = String.Empty
		Dim data = m_VacancyDatabaseAccess.LoadVacancyInseratData(m_currentVacancyNumber)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Keine Daten wurden gefunden."))

			Return
		End If

		Select Case strFieldName.ToLower

			Case "V_Zusatz_KDBeschreibung".ToLower
				strFieldValue = data.KDBeschreibung

			Case "V_Zusatz_KDBietet".ToLower
				strFieldValue = data.KDBietet

			Case "V_Zusatz_SBeschreibung".ToLower
				strFieldValue = data.SBeschreibung

			Case "V_Zusatz_Reserve1".ToLower
				strFieldValue = data.Reserve1

			Case "V_Zusatz_Taetigkeit".ToLower
				strFieldValue = data.Taetigkeit

			Case "V_Zusatz_Anforderung".ToLower
				strFieldValue = data.Anforderung

			Case "V_Zusatz_Reserve2".ToLower
				strFieldValue = data.Reserve2

			Case "V_Zusatz_Reserve3".ToLower
				strFieldValue = data.Reserve3

			Case "V_Zusatz_Ausbildung".ToLower
				strFieldValue = data.Ausbildung

			Case "V_Zusatz_Weiterbildung".ToLower
				strFieldValue = data.Weiterbildung

			Case "V_Zusatz_SKennt".ToLower
				strFieldValue = data.SKennt

			Case "V_Zusatz_EDVKennt".ToLower
				strFieldValue = data.EDVKennt

		End Select

		'If strFieldName <> String.Empty Then strFieldValue = GetVakDbFieldValue(strFieldName, m_currentVacancyNumber)
		Me.UcTinyMCE1.HtmlContent = strFieldValue

	End Sub

	Sub SaveSelectedVakZusatzDbField(ByVal strFieldName As String)

		If String.IsNullOrWhiteSpace(strFieldName) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurden keine Feldnamen für die Einträge ermittelt."))
		End If
		If m_currentVacancyNumber = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurde keine Vakanzennummer ermittelt."))

			Return
		End If

		Dim strFieldValue As String = UcTinyMCE1.HtmlContent
		Dim strTextValue As String = HTMLSnippetToPlainText(strFieldValue)

		Dim success = m_VacancyDatabaseAccess.AddDefaultDataIntoJobCHDb(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyZusatzDbFieldValue(strFieldName, strFieldValue, strTextValue, m_currentVacancyNumber)

		'If strFieldName <> String.Empty Then strFieldValue = SaveJobCHDbFieldValue(strFieldName,
		'																																			strFieldValue,
		'																																			strTextValue,
		'																																			m_currentVacancyNumber)
		If Not success Then
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden.") & " {0}"), strFieldName)
		End If
		bChangedContent = Not success







		'If m_currentVacancyNumber = 0 Then Exit Sub
		'Dim strFieldValue As String = UcTinyMCE1.HtmlContent
		'Dim strPlaintext As String = HTMLSnippetToPlainText(strFieldValue)
		'If strFieldName <> String.Empty Then strFieldValue = SaveVakZusatzDbFieldValue(strFieldName,
		'																																			strFieldValue,
		'																																			strPlaintext,
		'																																			m_currentVacancyNumber)
		'If strFieldValue.ToLower.Contains("error") Then
		'	DevExpress.XtraEditors.XtraMessageBox.Show(strFieldValue & vbNewLine & strFieldValue,
		'																						 "SaveSelectedVakZusatzDbField",
		'																						 MessageBoxButtons.OK, MessageBoxIcon.Error)
		'	bChangedContent = True
		'Else
		'	'Me.FileSaveItem1.Enabled = False
		'	bChangedContent = False
		'End If

	End Sub

	Sub DisplaySelectedJobCHDbField(ByVal strFieldName As String)

		If String.IsNullOrWhiteSpace(strFieldName) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurden keine Feldnamen für die Einträge ermittelt."))
		End If
		If m_currentVacancyNumber = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurde keine Vakanzennummer ermittelt."))

			Return
		End If

		Dim strFieldValue As String = String.Empty
		Dim data = m_VacancyDatabaseAccess.LoadJobCHInseratData(m_currentVacancyNumber)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Keine Daten wurden gefunden."))

			Return
		End If

		Select Case strFieldName.ToLower

			Case "J_Zusatz_Jobs_Vorspann".ToLower
				strFieldValue = data.Vorspann

			Case "J_Zusatz_Jobs_Aufgabe".ToLower
				strFieldValue = data.Aufgabe

			Case "J_Zusatz_Jobs_Anforderung".ToLower
				strFieldValue = data.Anforderung

			Case "J_Zusatz_Jobs_wirbieten".ToLower
				strFieldValue = data.Wirbieten

		End Select

		'strFieldValue = GetJobCHDbFieldValue(strFieldName, m_currentVacancyNumber)
		Me.UcTinyMCE1.HtmlContent = strFieldValue

	End Sub

	Sub DisplayJobCHVorschau()
		Dim strFieldValue As String = String.Empty

		If m_currentVacancyNumber = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurde keine Vakanzennummer ermittelt."))

			Return
		End If

		Dim data = m_VacancyDatabaseAccess.LoadJobCHInseratData(m_currentVacancyNumber)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Keine Daten wurden gefunden."))

			Return
		End If
		strFieldValue = data.Vorschau
		'GetJobCHDbFieldValue("J_Zusatz_Jobs_Vorspann, _J_Zusatz_Jobs_Aufgabe, _J_Zusatz_Jobs_Anforderung, _J_Zusatz_Jobs_Wirbieten", m_currentVacancyNumber)
		Me.UcTinyMCE1.HtmlContent = strFieldValue

	End Sub

	Sub SaveSelectedJobCHDbField(ByVal strFieldName As String)

		If String.IsNullOrWhiteSpace(strFieldName) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurden keine Feldnamen für die Einträge ermittelt."))
		End If
		If m_currentVacancyNumber = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Abfrage der Daten. Es wurde keine Vakanzennummer ermittelt."))

			Return
		End If

		Dim strFieldValue As String = UcTinyMCE1.HtmlContent
		Dim strTextValue As String = HTMLSnippetToPlainText(strFieldValue)

		Dim success As Boolean = True
		'= m_VacancyDatabaseAccess.AddDefaultDataIntoJobCHDb(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		Dim jobCHData = m_VacancyDatabaseAccess.LoadJobCHMasterData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		success = success AndAlso Not (jobCHData Is Nothing)

		success = success AndAlso m_VacancyDatabaseAccess.UpdateJobCHDbFieldValue(strFieldName, strFieldValue, strTextValue, m_currentVacancyNumber)

		If Not success Then
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden.") & " {0}"), strFieldName)
		End If

	End Sub

	Private Sub RefreshAVAMState()

		Try
			Dim stmpData = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)

			Dim orgUserdata = m_mandant.GetSelectedUserData(m_InitializationData.MDData.MDNr, m_InitializationChangedData.UserData.UserNr)
			m_InernUploader = New SP.Vacancies.Intern.InternVacancyUploader(m_InitializationChangedData)
			If stmpData Is Nothing Then Return
			If stmpData.AVAMStateEnum = AVAMState.CANCELLED Then tgsReportingObligation.Enabled = False
			If String.IsNullOrWhiteSpace(stmpData.JobroomID) OrElse (stmpData.AVAMStateEnum <> AVAMState.INSPECTING AndAlso stmpData.AVAMStateEnum <> AVAMState.PUBLISHED_RESTRICTED AndAlso stmpData.AVAMStateEnum <> AVAMState.PUBLISHED_PUBLIC) Then
				Return
			End If

			m_InernUploader.CurrentVacancyNumber = m_currentVacancyNumber
			Dim jobRoomID As String = stmpData.JobroomID

			Dim stmpExportResult = m_InernUploader.LoadAsgingedJobAdvertisement(m_InitialChangedData.MDData.MDGuid, m_InitialChangedData.UserData.UserGuid, m_InitializationData.UserData.UserNr = 1, m_currentVacancyNumber, jobRoomID)
			If stmpExportResult Is Nothing Then Return

			tgsReportingObligation.EditValue = stmpExportResult.ReportingObligation.GetValueOrDefault(False)
			If stmpExportResult.reportingObligationEndDate.HasValue Then
				lblReportingObligationEndDate.Text = String.Format("{0:d}", stmpExportResult.ReportingObligationEndDate)
				If stmpExportResult.ReportingObligationEndDate >= Now.Date Then lblReportingObligationEndDate.Appearance.ForeColor = Color.Red

			Else
				lblReportingObligationEndDate.Text = "? (Wird ermittelt.)"
			End If
			lblAVAMState.Text = stmpExportResult.AVAMRecordState

			stmpData.AVAMRecordState = stmpExportResult.AVAMRecordState
			stmpData.ReportingObligation = stmpExportResult.ReportingObligation.GetValueOrDefault(False)
			stmpData.ReportingObligationEndDate = stmpExportResult.reportingObligationEndDate
			stmpData.SyncDate = stmpExportResult.SyncDate
			stmpData.SyncFrom = m_InitializationData.UserData.UserFullName

			lblReportingDate.Text = String.Format("{0:g}", stmpData.ReportingDate)
			lblReportingDate.ToolTip = String.Format("{0:g}, {1}", stmpData.ReportingDate, stmpData.ReportingFrom)
			lblAVAMSyncDate.Text = String.Format("{0:g}", stmpData.SyncDate)
			lblAVAMSyncDate.ToolTip = String.Format("{0:g}, {1}", stmpData.SyncDate, stmpData.SyncFrom)

			Dim result = m_VacancyDatabaseAccess.UpdateVacancyStmpSettingData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserNr, stmpData)
			pnlAVAM.Visible = result

			'Dim msg As String
			'msg = "Meldepflichtig: {1}{0}"
			'msg = "Status der Übermittlung: {2}{0}"
			'msg &= "Übermittelt am: {3}{0:g}"
			'msg &= "Durch: {4}{0}"
			'msg &= "Synchronisiert am: {5}{0:g}"
			'msg &= "Durch: {6}{0}"

			'msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, stmpData.ReportingObligation, stmpData.AVAMRecordState, stmpData.ReportingDate, stmpData.SyncFrom, stmpData.SyncDate, stmpData.SyncFrom)
			'm_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("AVAM-Update"), MessageBoxIcon.Information)



		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		End Try

	End Sub

	Private Sub tgsReportingObligation_Click(sender As Object, e As EventArgs) Handles tgsReportingObligation.Click

		If tgsReportingObligation.EditValue Then
			Dim selectedUserData As AdvisorViewData = TryCast(Me.lueAdvisor.GetSelectedDataRow(), AdvisorViewData)
			If m_InitialChangedData Is Nothing Then
				Dim m_ini = ChangeMandantData(lueMandant.EditValue, selectedUserData.UserNumber)
				m_InitialChangedData = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, m_InitializationData.MDData, m_ini.UserData)
			End If

			Dim frm As New frmCancelAdvertisment(m_InitializationData, m_InitialChangedData)
			frm.VacancySettingData = VacancySetting
			frm.CurrentVacancyNumber = m_currentVacancyNumber

			frm.LoadData()
			frm.ShowDialog()

			RefreshAVAMState()

		End If

	End Sub

	Private Sub libJobLink_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblkundenlink.LinkClicked
		If Not String.IsNullOrWhiteSpace(Me.txt_KDJobLink.Text) Then
			Process.Start(Me.txt_KDJobLink.Text)
		End If

	End Sub

	Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabMain.SelectedPageChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If Me.xtabMain.SelectedTabPageIndex = 0 Then
			SavertfContent()

		ElseIf xtabMain.SelectedTabPageIndex = 1 Then
			Try
				SaveSelectedVacancy(False)
				If Not Me.IsTab2Selected Then BuildVakZusatzFields()
				Me.pcEditor.Visible = False
				Me.IsTab2Selected = True

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navigationsleiste für Text-Bausteine. {1}", strMethodeName, ex.ToString))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("{0}", ex.ToString), "Navigationsleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End Try

		End If

	End Sub

	Sub ShowJobsCHProperties()

		Dim selectedUserData As AdvisorViewData = TryCast(Me.lueAdvisor.GetSelectedDataRow(), AdvisorViewData)
		If m_InitialChangedData Is Nothing Then
			Dim m_ini = ChangeMandantData(lueMandant.EditValue, selectedUserData.UserNumber)
			m_InitialChangedData = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, m_InitializationData.MDData, m_ini.UserData)
		End If

		Dim frm As New frmJobsCH(m_InitializationData, m_InitialChangedData)

		If Not selectedUserData Is Nothing Then
			frm.VacancyAdvisorNumber = selectedUserData.UserNumber
		Else
			frm.VacancyAdvisorNumber = m_InitialChangedData.UserData.UserNr
		End If

		frm.VacancySettingData = VacancySetting
		frm.CurrentVacancyData = m_currentVacancyData
		frm.SBNNumber = CType(lueSBNNumber.EditValue, Integer)
		frm.SBNPublicationState = m_currentVacancyData.SBNPublicationState

		frm.LoadData()

		AddHandler frm.CreditInfoDataSaved, AddressOf SetVakState
		frm.ShowDialog()

	End Sub


#Region "View helper classes"

	''' <summary>
	''' Advisor view data.
	''' </summary>
	Private Class AdvisorViewData

		Public Property UserNumber As Integer
		Public Property KST As String
		Public Property Salutation As String
		Public Property FristName As String
		Public Property LastName As String

		Public ReadOnly Property LastName_FirstName As String
			Get
				Return LastName & ", " & FristName
			End Get
		End Property

		Public ReadOnly Property FullName As String
			Get
				Return String.Format("{0} {1} {2}", Salutation, FristName, LastName)
			End Get
		End Property

	End Class

#End Region


	Private Sub txt_Bezeichnung_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_Bezeichnung.ButtonClick
		Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
		obj.SelectMultirecords = False
		Dim success = True

		success = success AndAlso obj.LoadQualificationData("M")
		If Not success Then Return

		obj.ShowDialog()
		Dim selectedProfessionsString = obj.GetSelectedData
		If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return

		Me.txt_Bezeichnung.Text = If(selectedProfessionsString.Contains("#"), selectedProfessionsString.Split("#")(1), selectedProfessionsString)

	End Sub


#Region "DropDown für Customer"

	''' <summary>
	''' Reads a popup size setting.
	''' </summary>
	''' <param name="settingKey">The settings key.</param>
	''' <returns>The size setting.</returns>
	Private Function ReadPopupSizeSetting(ByRef settingKey As String) As Size

		' Load width/height setting
		Dim popupSizeSetting As String = String.Empty
		Dim popupSize As Size
		popupSize.Width = POPUP_DEFAULT_WIDTH
		popupSize.Height = POPUP_DEFAULT_HEIGHT

		Try
			popupSizeSetting = m_SettingsManager.ReadString(settingKey)

			If Not String.IsNullOrEmpty(popupSizeSetting) Then
				Dim arrSize As String() = popupSizeSetting.Split(CChar(";"))
				popupSize.Width = arrSize(0)
				popupSize.Height = arrSize(1)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		Return popupSize
	End Function

	''' <summary>
	''' Handles poupup employee size change.
	''' </summary>
	Private Sub OnPopupCustomer_SizeChanged(ByVal sender As Object, ByVal newWidth As Integer, ByVal newHeight As Integer)
		Try
			Dim setting As String = String.Format("{0};{1}", newWidth, newHeight)
			m_SettingsManager.WriteString(Settings.SettingKeys.SETTING_POPUP_CUSTOMER_SIZE, setting)
			m_SettingsManager.SaveSettings()
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	''' <summary>
	''' Hides the popups.
	''' </summary>
	Private Sub HidePopups()
		'ucCustomerPopup.HidePopup()
	End Sub

#End Region


	'Private Sub txt_KDNr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_KDNr.ButtonClick
	'	Dim cursorPos = Cursor.Position

	'	HidePopups()

	'	Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_CUSTOMER_SIZE)
	'	Dim position = Cursor.Position

	'	If (m_CustomerPopupData Is Nothing) Then
	'		Dim customerData = m_DataAccess.LoadCustomerData()

	'		If Not customerData Is Nothing Then

	'			Dim listOfEmployeeViewData As New List(Of CustomerViewData)
	'			For Each myData In customerData
	'				listOfEmployeeViewData.Add(TranformToCustomerViewData(myData))
	'			Next

	'			m_CustomerPopupData = listOfEmployeeViewData

	'		End If

	'	End If

	'	ucCustomerPopup.InitPopup(m_CustomerPopupData, m_CustomerPopupColumns, False, True, ShowFilterPanelMode.Never)
	'	ucCustomerPopup.ShowPopup(position, popupSize)

	'End Sub

	'''' <summary>
	'''' Transforms employee data to view data.
	'''' </summary>
	'''' <param name="customerData">The employee data.</param>
	'''' <returns>The employee view data.</returns>
	'Private Function TranformToCustomerViewData(ByVal customerData As CustomerData) As CustomerViewData

	'	Dim customerVieData As New CustomerViewData

	'	customerVieData.customernumber = customerData.CustomerNumber
	'	customerVieData.customername = String.Format("{0}", customerData.customername).Trim()
	'	customerVieData.customeraddress = String.Format("{0}", customerData.customeraddress).Trim()

	'	Return customerVieData
	'End Function

	Private Sub txt_Bezeichnung_TextChanged(sender As Object, e As System.EventArgs) Handles txt_Bezeichnung.TextChanged
		lblBezeichnung.Text = String.Format("<b>{0}</b>", txt_Bezeichnung.Text)
	End Sub

	Private Sub ToggleFontBoldItem2_CheckedChanged(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs)

	End Sub

	Private Sub OnrtfContent_BeforeExport(sender As Object, e As DevExpress.XtraRichEdit.BeforeExportEventArgs)
		Dim options As HtmlDocumentExporterOptions = TryCast(e.Options, HtmlDocumentExporterOptions)
		If options IsNot Nothing Then
			options.CssPropertiesExportType = CssPropertiesExportType.Inline
			options.HtmlNumberingListExportFormat = HtmlNumberingListExportFormat.HtmlFormat
			options.TargetUri = Path.GetFileNameWithoutExtension("")
		End If

	End Sub

	''' <summary>
	''' Converts a html string to plain text format.
	''' </summary>
	''' <param name="htmlString">The html string.</param>
	''' <returns>Plaint text without html tags.</returns>
	Private Function HTMLSnippetToPlainText(ByVal htmlString As String) As String
		Dim stringBuilder As New System.Text.StringBuilder(htmlString)

		stringBuilder.Replace("<br>", Environment.NewLine)
		stringBuilder.Replace("<br/>", Environment.NewLine)
		stringBuilder.Replace("<br />", Environment.NewLine)
		stringBuilder.Replace("</p>", Environment.NewLine)
		stringBuilder.Replace("<li>", "- ")

		Dim strStep1 = stringBuilder.ToString()

		' Replace all other tags with empty string
		Dim reg = New Regex("<[^>]+>", RegexOptions.IgnoreCase)
		Dim strStep2 = reg.Replace(strStep1, String.Empty)

		' Decode some special character literals like &nbsp; or german 'Umlaute'.
		Dim strStep3 = HttpUtility.HtmlDecode(strStep2)

		Return strStep3.Trim()

	End Function



	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


	Private Sub frmVakanzen_Shown(sender As Object, e As System.EventArgs) Handles MyBase.Shown
		'm_SuppressUIEvents = True
	End Sub

	Function ValidateData() As Boolean

		ErrorProvider.ClearErrors()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		Dim m_MDOk = lueMandant.EditValue Is Nothing
		isValid = isValid AndAlso SetDXErrorIfInvalid(lueMandant, ErrorProvider, m_MDOk, errorText)

		Dim m_AdvisorOk = lueAdvisor.EditValue Is Nothing
		isValid = isValid AndAlso SetDXErrorIfInvalid(lueAdvisor, ErrorProvider, m_AdvisorOk, errorText)

		Dim m_BezOk = txt_Bezeichnung.Text.Length >= 5
		isValid = isValid AndAlso SetDXErrorIfInvalid(txt_Bezeichnung, ErrorProvider, Not m_BezOk, errorText)

		'Dim m_KDNrOk = Not lueCustomer.EditValue Is Nothing AndAlso lueCustomer.EditValue > 0
		isValid = isValid AndAlso SetDXErrorIfInvalid(lueCustomer, ErrorProvider, lueCustomer.EditValue Is Nothing OrElse lueCustomer.EditValue = 0, errorText)

		If Not (lueSBNNumber.EditValue Is Nothing OrElse lueSBNNumber.EditValue = 0) Then
			isValid = isValid AndAlso SetDXErrorIfInvalid(luePostcode, ErrorProvider, luePostcode.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePostcode.EditValue), errorText)
			isValid = isValid AndAlso SetDXErrorIfInvalid(txtLocation, ErrorProvider, txtLocation.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(txtLocation.EditValue), errorText)
		End If

		' must fields...
		Dim xmlFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
		Dim FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
		Dim mustVacancyGroupBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancygruppeselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		Dim mustVacancysbn2000BeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancysbn2000selection", FORM_XML_REQUIREDFIEKDS_KEY)), False)

		isValid = isValid AndAlso SetDXErrorIfInvalid(lueGroup, ErrorProvider, (mustVacancyGroupBeSelected And String.IsNullOrEmpty(lueGroup.EditValue)), errorText)
		isValid = isValid AndAlso SetDXErrorIfInvalid(lueSBNNumber, ErrorProvider, (mustVacancysbn2000BeSelected And String.IsNullOrEmpty(lueSBNNumber.EditValue)), errorText)

		Dim mustVacancyContactBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancycontactselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		isValid = isValid AndAlso SetDXErrorIfInvalid(lueKontakt, ErrorProvider, (mustVacancyContactBeSelected And (lueKontakt.EditValue Is Nothing OrElse String.IsNullOrEmpty(lueKontakt.Text))), errorText)

		Dim mustVacancyStateBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancystateselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		isValid = isValid AndAlso SetDXErrorIfInvalid(lueStatus, ErrorProvider, (mustVacancyStateBeSelected And (lueStatus.EditValue Is Nothing OrElse String.IsNullOrEmpty(lueStatus.EditValue))), errorText)

		Dim mustVacancyJobPoBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyjobpostcodeselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		isValid = isValid AndAlso SetDXErrorIfInvalid(luePostcode, ErrorProvider, (mustVacancyJobPoBeSelected And String.IsNullOrEmpty(luePostcode.EditValue)), errorText)

		Dim mustVacancyJobCityBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/vacancyjobcityselection", FORM_XML_REQUIREDFIEKDS_KEY)), False)
		isValid = isValid AndAlso SetDXErrorIfInvalid(txtLocation, ErrorProvider, (mustVacancyJobCityBeSelected And String.IsNullOrEmpty(txtLocation.Text)), errorText)


		Return isValid

	End Function

	Function ValidateJobPlattformData() As Boolean

		Dim PublicationFields = m_VacancyDatabaseAccess.LoadJobCHInseratData(m_currentVacancyNumber)

		ErrorProvider.ClearErrors()

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		' must fields...
		Dim xmlFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
		Dim FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

		Dim strMsg As String = String.Empty
		Dim allowedExternJobplattforms As Boolean = True
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

		If strMsg.Length > 0 Then
			m_UtilityUI.ShowInfoDialog(String.Format(strMsg, vbNewLine), m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Warning)

			isValid = False
		End If

		Return isValid

	End Function

	Protected Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	Protected Function SetDXErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText, ErrorType.Critical)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	Protected Function SetDXWarningIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText, ErrorType.Warning)
		End If

		Return Not invalid

	End Function

	Private Sub lueSBNNumber_Popup(sender As Object, e As EventArgs) Handles lueSBNNumber.Popup

		Dim popupWindow As PopupGridLookUpEditForm = CType((CType(lueSBNNumber, IPopupControl)).PopupWindow, PopupGridLookUpEditForm)
		Dim vInfo As GridViewInfo = TryCast(gvSBNNumber.GetViewInfo(), GridViewInfo)
		popupWindow.Height = vInfo.CalcRealViewHeight(New Rectangle(0, 0, 500, 500)) + popupWindow.CloseButton.Height + 10
		popupWindow.Width = lueSBNNumber.Width * 1.75
		popupWindow.Refresh()

	End Sub


	Class ResponsiblePersonViewData

		Public Property Lastname As String
		Public Property Firstname As String
		Public Property TranslatedSalutation As String
		Public Property ResponsiblePersonRecordNumber As Integer?

		Public Property ZState1 As String
		Public Property ZState2 As String

		Public ReadOnly Property IsZHDActiv As Boolean?
			Get
				Dim isZActiv As Boolean = True
				Dim state1 As String = If(String.IsNullOrWhiteSpace(ZState1), String.Empty, ZState1.ToLower)
				Dim state2 As String = If(String.IsNullOrWhiteSpace(ZState2), String.Empty, ZState2.ToLower)

				isZActiv = Not (state1.Contains("inaktiv") OrElse state1.Contains("mehr aktiv") OrElse state2.Contains("inaktiv") OrElse state2.Contains("mehr aktiv"))
				Return isZActiv
			End Get
		End Property

		Public ReadOnly Property SalutationLastNameFirstName
			Get
				Return String.Format("{0} {1} {2}", TranslatedSalutation, Lastname, Firstname)
			End Get
		End Property
	End Class

End Class

Public Class JobPlattformsInfoData
	Public Property JobplattformLabel As String
	Public Property IsOnline As Boolean
	Public Property TranferedJobs As Integer
	Public Property TotalAllowedJobs As Integer
	Public Property TotalExpireSoonJobs As Integer

	Public ReadOnly Property TotalOpenJobs As Integer
		Get
			If JobplattformLabel.ToLower = "website" Then Return 0
			Return TotalAllowedJobs - TranferedJobs
		End Get
	End Property
End Class



'''' <summary>
'''' Customer view data.
'''' </summary>
'Class CustomerViewData

'	Public Property customernumber As Integer
'	Public Property customername As String
'	Public Property customeraddress As String

'End Class

'Public Class CustomerData

'	Public Property ID As Integer
'	Public Property CustomerNumber As Integer?
'	Public Property customername As String
'	Public Property Street As String
'	Public Property Postcode As String
'	Public Property Location As String
'	Public Property CountryCode As String
'	Public Property customeraddress As String

'End Class

