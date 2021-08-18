
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData
Imports System.ComponentModel

Imports SP.Infrastructure.UI.UtilityUI
Imports SP.Infrastructure.Utility
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten

Imports SPS.SYS.DocUtility.ClsDataDetail
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee


Public Class frmDocUtility
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"

	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
	Private Const MANDANT_XML_SETTING_SPUTNIK_TEMPLATES_SETTING As String = "MD_{0}/Templates"
	Private Const MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING As String = "MD_{0}/Mailing"
	Private Const MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING As String = "MD_{0}/BuchungsKonten"

#End Region

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDbAccess As IEmployeeDatabaseAccess

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

	Private m_Mandant As Mandant

	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUI As UtilityUI

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private connectionString As String

	Private m_CurrentRecordNumber As Integer?
	Private m_SonstigesSetting As String
	Private m_TemplatesSetting As String
	Private m_MailingSetting As String
	Private m_xmlMDFilename As String


	Private Structure LvData
		Public iID As String
		Public Bezeichnung As String
		Public DocName As String
		Public JobNr As String

		Public Sub New(ByVal recDoc As SqlClient.SqlDataReader)
			Try
				With recDoc
					Me.iID = recDoc("ID").ToString
					Me.Bezeichnung = recDoc("Bezeichnung").ToString
					Me.DocName = recDoc("DocName").ToString
					Me.JobNr = recDoc("JobNr").ToString

				End With
			Catch ex As Exception
				' Kein Fehler
			End Try


		End Sub

	End Structure


#Region "Constructur"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_Logger = New Logger
		m_Mandant = New Mandant
		m_Utility = New SPProgUtility.MainUtilities.Utilities
		m_UtilityUI = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDbAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)


		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

		m_MandantXMLFile = m_Mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)
		If Not System.IO.File.Exists(m_MandantXMLFile) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
		Else
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		m_xmlMDFilename = m_Mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)
		m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
		m_TemplatesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_TEMPLATES_SETTING, m_InitializationData.MDData.MDNr)
		m_MailingSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING, m_InitializationData.MDData.MDNr)


		TranslateControls()
		Reset()


		AddHandler gvDoc.RowCellClick, AddressOf Ongv_RowCellClick

	End Sub

#End Region

#Region "public methods"

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		result = result AndAlso LoadDocuments()
		result = result AndAlso LoadZVTemplateDropDown()
		result = result AndAlso LoadARGBTemplateDropDown()
		result = result AndAlso LoadTemplateDropDown()

		result = result AndAlso LoadProposeAttachmentJobNumberDropDown()
		result = result AndAlso LoadAvailableEmployeesTemplateJobNumberDropDown()
		result = result AndAlso LoadccboOffMailTemplateJobNumberDropDown()

		result = result AndAlso LoadEmployeeDocumentCategoriesDropDownData(m_InitializationData.UserData.UserLanguage)
		result = result AndAlso LoadProgSettingValue()

		Return result
	End Function


#End Region


#Region "readonly properties"

	''' <summary>
	''' Gets the selected document.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedRecord As MandantDocumentData
		Get
			Dim gv = TryCast(grdDoc.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gv Is Nothing) Then

				Dim selectedRows = gv.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim data = CType(gv.GetRow(selectedRows(0)), MandantDocumentData)
					Return data
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedEinzelOfferTemplate() As TemplateData
		Get
			Dim gvRP = CType(lueEinzelOffer.GetSelectedDataRow(), TemplateData)

			Return gvRP
		End Get

	End Property

	Private ReadOnly Property SelectedMassenOfferTemplate() As TemplateData
		Get
			Dim gvRP = CType(lueMassenOffer.GetSelectedDataRow(), TemplateData)

			Return gvRP
		End Get

	End Property


#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.ReflectionLabel1.Text = m_Translate.GetSafeTranslationValue("Verwaltung von Dokumenten")
		lblRecNummer.Text = m_Translate.GetSafeTranslationValue(lblRecNummer.Text)

		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bbiCopyDocumentRecord.Caption = m_Translate.GetSafeTranslationValue(Me.bbiCopyDocumentRecord.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)
		Me.bbiDesign.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDesign.Caption)

	End Sub

	Private Sub Reset()

		tpMainSetting.SelectedPage = tnpEMailTemplates

		ResetProposeAttachmentJobNumberDropDown()
		ResetEmployeeDocumentCategoryDropDown()
		ResetEinzelOfferDropDown()
		ResetMassenOfferDropDown()

		ResetZVTemplateDocNameDropDown()
		ResetARGBTemplateDocNameDropDown()
		ResetAvailableEmployeesTemplateJobNumberDropDown()
		ResetOffMailingTemplateJobNumberDropDown()

		ResetGridDocumentData()

	End Sub


#Region "reset"

	Private Sub ResetGridDocumentData()

		gvDoc.OptionsView.ShowIndicator = False
		gvDoc.OptionsView.ShowAutoFilterRow = True
		gvDoc.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvDoc.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnMANr.Name = "ID"
		columnMANr.FieldName = "ID"
		columnMANr.Visible = False
		gvDoc.Columns.Add(columnMANr)

		Dim columnzNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzNr.Caption = m_Translate.GetSafeTranslationValue("ModulNr")
		columnzNr.Name = "ModulNr"
		columnzNr.FieldName = "ModulNr"
		columnzNr.Visible = False
		gvDoc.Columns.Add(columnzNr)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("JobNr")
		columncustomername.Name = "jobNr"
		columncustomername.FieldName = "jobNr"
		columncustomername.BestFit()
		columncustomername.Visible = True
		gvDoc.Columns.Add(columncustomername)

		Dim columncustomername2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername2.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columncustomername2.Name = "Bezeichnung"
		columncustomername2.FieldName = "Bezeichnung"
		columncustomername2.BestFit()
		columncustomername2.Visible = True
		gvDoc.Columns.Add(columncustomername2)

		Dim columncustomerstreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerstreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomerstreet.Caption = m_Translate.GetSafeTranslationValue("DocName")
		columncustomerstreet.Name = "DocName"
		columncustomerstreet.FieldName = "DocName"
		columncustomerstreet.BestFit()
		columncustomerstreet.Visible = True
		gvDoc.Columns.Add(columncustomerstreet)

		Dim columnEcustomeradress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEcustomeradress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEcustomeradress.Caption = m_Translate.GetSafeTranslationValue("ExportedFileName")
		columnEcustomeradress.Name = "ExportedFileName"
		columnEcustomeradress.FieldName = "ExportedFileName"
		columnEcustomeradress.BestFit()
		columnEcustomeradress.Visible = True
		gvDoc.Columns.Add(columnEcustomeradress)


		grdDoc.DataSource = Nothing

	End Sub

	Private Sub ResetEinzelOfferDropDown()

		lueEinzelOffer.Properties.DisplayMember = "LabelViewData"
		lueEinzelOffer.Properties.ValueMember = "jobNr"

		gvEinzelOffer.OptionsView.ShowIndicator = False
		gvEinzelOffer.OptionsView.ShowColumnHeaders = True
		gvEinzelOffer.OptionsView.ShowFooter = False

		gvEinzelOffer.OptionsView.ShowAutoFilterRow = True
		gvEinzelOffer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEinzelOffer.Columns.Clear()

		Dim columnjobNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjobNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnjobNr.Caption = m_Translate.GetSafeTranslationValue("Job-Nr.")
		columnjobNr.Name = "jobNr"
		columnjobNr.FieldName = "jobNr"
		columnjobNr.Visible = True
		gvEinzelOffer.Columns.Add(columnjobNr)

		Dim columndocBez As New DevExpress.XtraGrid.Columns.GridColumn()
		columndocBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columndocBez.Caption = m_Translate.GetSafeTranslationValue("Dokument")
		columndocBez.Name = "DocFileName"
		columndocBez.FieldName = "DocFileName"
		columndocBez.Visible = True
		gvEinzelOffer.Columns.Add(columndocBez)

		lueEinzelOffer.EditValue = Nothing

	End Sub

	Private Sub ResetMassenOfferDropDown()

		lueMassenOffer.Properties.DisplayMember = "LabelViewData"
		lueMassenOffer.Properties.ValueMember = "jobNr"

		gvMassenOffer.OptionsView.ShowIndicator = False
		gvMassenOffer.OptionsView.ShowColumnHeaders = True
		gvMassenOffer.OptionsView.ShowFooter = False

		gvMassenOffer.OptionsView.ShowAutoFilterRow = True
		gvMassenOffer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvMassenOffer.Columns.Clear()

		Dim columnjobNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjobNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnjobNr.Caption = m_Translate.GetSafeTranslationValue("Job-Nr.")
		columnjobNr.Name = "jobNr"
		columnjobNr.FieldName = "jobNr"
		columnjobNr.Visible = True
		gvMassenOffer.Columns.Add(columnjobNr)

		Dim columndocBez As New DevExpress.XtraGrid.Columns.GridColumn()
		columndocBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columndocBez.Caption = m_Translate.GetSafeTranslationValue("Dokument")
		columndocBez.Name = "DocFileName"
		columndocBez.FieldName = "DocFileName"
		columndocBez.Visible = True
		gvMassenOffer.Columns.Add(columndocBez)

		lueMassenOffer.EditValue = Nothing

	End Sub

	'''' <summary>
	'''' Resets the document category drop down.
	'''' </summary>
	'Private Sub ResetDocumentCategoryDropDown()

	'	lstDocumentCategory.DisplayMember = "Description"
	'	lstDocumentCategory.ValueMember = "CategoryNumber"

	'End Sub

	Private Sub ResetZVTemplateDocNameDropDown()

		lueZVTemplate.Properties.DisplayMember = "LabelViewData"
		lueZVTemplate.Properties.ValueMember = "DocFileName"

		gvZV.OptionsView.ShowIndicator = False
		gvZV.OptionsView.ShowColumnHeaders = True
		gvZV.OptionsView.ShowFooter = False

		gvZV.OptionsView.ShowAutoFilterRow = True
		gvZV.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvZV.Columns.Clear()

		Dim columnjobNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjobNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnjobNr.Caption = m_Translate.GetSafeTranslationValue("Job-Nr.")
		columnjobNr.Name = "jobNr"
		columnjobNr.FieldName = "jobNr"
		columnjobNr.Visible = True
		gvZV.Columns.Add(columnjobNr)

		Dim columndocBez As New DevExpress.XtraGrid.Columns.GridColumn()
		columndocBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columndocBez.Caption = m_Translate.GetSafeTranslationValue("Vorlage")
		columndocBez.Name = "DocFileName"
		columndocBez.FieldName = "DocFileName"
		columndocBez.Visible = True
		gvZV.Columns.Add(columndocBez)

		lueZVTemplate.EditValue = Nothing

	End Sub

	Private Sub ResetARGBTemplateDocNameDropDown()

		lueArgbTemplate.Properties.DisplayMember = "LabelViewData"
		lueArgbTemplate.Properties.ValueMember = "DocFileName"

		gvArgb.OptionsView.ShowIndicator = False
		gvArgb.OptionsView.ShowColumnHeaders = True
		gvArgb.OptionsView.ShowFooter = False

		gvArgb.OptionsView.ShowAutoFilterRow = True
		gvArgb.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvArgb.Columns.Clear()

		Dim columnjobNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnjobNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnjobNr.Caption = m_Translate.GetSafeTranslationValue("Job-Nr.")
		columnjobNr.Name = "jobNr"
		columnjobNr.FieldName = "jobNr"
		columnjobNr.Visible = True
		gvArgb.Columns.Add(columnjobNr)

		Dim columndocBez As New DevExpress.XtraGrid.Columns.GridColumn()
		columndocBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columndocBez.Caption = m_Translate.GetSafeTranslationValue("Vorlage")
		columndocBez.Name = "DocFileName"
		columndocBez.FieldName = "DocFileName"
		columndocBez.Visible = True
		gvArgb.Columns.Add(columndocBez)

		lueArgbTemplate.EditValue = Nothing

	End Sub

	Private Sub ResetProposeAttachmentJobNumberDropDown()

		ccboPMail_tplNr.Properties.DisplayMember = "docBez"
		ccboPMail_tplNr.Properties.ValueMember = "jobNr"

		ccboPMail_tplNr.EditValue = Nothing

	End Sub

	Private Sub ResetEmployeeDocumentCategoryDropDown()

		ccboEmployeeDocCategory.Properties.DisplayMember = "Description"
		ccboEmployeeDocCategory.Properties.ValueMember = "CategoryNumber"

		ccboEmployeeDocCategory.EditValue = Nothing

	End Sub

	Private Sub ResetAvailableEmployeesTemplateJobNumberDropDown()

		ccboavailable_employee_wos_template_jobnr.Properties.DisplayMember = "LabelViewData"
		ccboavailable_employee_wos_template_jobnr.Properties.ValueMember = "jobNr"

		ccboavailable_employee_wos_template_jobnr.EditValue = Nothing

	End Sub

	Private Sub ResetOffMailingTemplateJobNumberDropDown()

		ccboOffMail_tplDocNr.Properties.DisplayMember = "LabelViewData"
		ccboOffMail_tplDocNr.Properties.ValueMember = "jobNr"

		ccboOffMail_tplDocNr.EditValue = Nothing

	End Sub


#End Region


	Private Function LoadDocuments() As Boolean
		Dim success = True

		Dim listOfData = m_TablesettingDatabaseAccess.LoadMandantDocumentData()
		If listOfData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Dokumentdaten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
						Select New MandantDocumentData With
			 {.ID = person.ID,
				.RecNr = person.RecNr,
				.Anzahlkopien = person.Anzahlkopien,
				.Bezeichnung = person.Bezeichnung,
				.DocName = person.DocName,
				.ExportedFileName = person.ExportedFileName,
				.FontDesent = person.FontDesent,
				.IncPrv = person.IncPrv,
				.InsertFileToDb = person.InsertFileToDb,
				.jobNr = person.jobNr,
				.KonvertName = person.KonvertName,
				.Meldung0 = person.Meldung0,
				.Meldung1 = person.Meldung1,
				.ModulNr = person.ModulNr,
				.ParamCheck = person.ParamCheck,
				.PrintInDiffColor = person.PrintInDiffColor,
				.TempDocPath = person.TempDocPath,
				.ZoomProz = person.ZoomProz
			 }).ToList()

		Dim listDataSource As BindingList(Of MandantDocumentData) = New BindingList(Of MandantDocumentData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdDoc.DataSource = listDataSource
		Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvDoc.RowCount)


		Return Not listOfData Is Nothing

	End Function

	Private Function LoadProgSettingValue() As Boolean
		Dim result As Boolean = True

		Me.lblMandantDocumentPath.Text = m_Mandant.GetSelectedMDDocPath(m_InitializationData.MDData.MDNr)
		Me.lblTemplatePath_1.Text = m_Mandant.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr)

		Try
			' Schriftarten...
			Dim FontName As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FontName", m_SonstigesSetting))
			Dim LL_IndentSize As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/LL_IndentSize", m_SonstigesSetting))
			Dim FontSize As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/FontSize", m_SonstigesSetting))
			Dim LLTemplateExtension As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/LLTemplateExtension", m_SonstigesSetting))
			Dim MailFontName As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/MailFontName", m_SonstigesSetting))
			Dim MailFontSize As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/MailFontSize", m_SonstigesSetting))

			Me.cbo_FontEdit.Text = ReplaceEmptyValue(FontName, "Calibri")
			Me.sp_Fontsize.Text = ReplaceEmptyValue(FontSize, 11)
			Me.sp_FontIndent.Text = ReplaceEmptyValue(LL_IndentSize, 20)
			Me.cbo_LLTExtension.Text = ReplaceEmptyValue(LLTemplateExtension, "Doc")

			' Mail-Schriftarten...
			Me.cbo_MailFont.Text = ReplaceEmptyValue(MailFontName, "Calibri")
			Me.sp_MailFontsize.Text = ReplaceEmptyValue(MailFontSize, 11)

			' Vorlagen...
			Dim available_employee_wos_template_jobnr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/available_employee_wos_template_jobnr", m_TemplatesSetting))
			Dim OffMail_tplDocNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/OffMail_tplDocNr", m_TemplatesSetting))
			Dim PMail_tplDocNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/PMail_tplDocNr", m_TemplatesSetting))
			Dim eMail_Template_JobNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMail-Template-JobNr", m_TemplatesSetting))
			Dim MassenOffer_eMail_Template_JobNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/MassenOffer-eMail-Template-JobNr", m_TemplatesSetting))


			lueEinzelOffer.EditValue = eMail_Template_JobNr
			lueMassenOffer.EditValue = MassenOffer_eMail_Template_JobNr

			Dim invoiceeachfilename As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/invoiceeachfilename", m_MailingSetting))
			Dim invoicezipfilename As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/invoicezipfilename", m_MailingSetting))
			Dim Invoice_eMail_subject As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Invoice-eMail-subject", m_MailingSetting))
			Dim moreinvoices_email_subject As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/moreinvoices-email-subject", m_MailingSetting))
			Dim payroll_email_subject As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/payroll-email-subject", m_MailingSetting))
			Dim morepayroll_email_subject As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/morepayroll-email-subject", m_MailingSetting))

			Dim Zwischenverdienstformular_Doc_eMail_Template As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Zwischenverdienstformular_Doc-eMail-Template", m_TemplatesSetting))
			Dim Arbeitgeberbescheinigung_Doc_eMail_Template As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Arbeitgeberbescheinigung_Doc-eMail-Template", m_TemplatesSetting))
			Dim MADoc_eMail_Template As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/MADoc-eMail-Template", m_TemplatesSetting))
			Dim KDDoc_eMail_Template As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/KDDoc-eMail-Template", m_TemplatesSetting))
			Dim ZHDDoc_eMail_Template As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/ZHDDoc-eMail-Template", m_TemplatesSetting))
			Dim proposemailsubject As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/proposemailsubject", m_TemplatesSetting))
			Dim proposemaildocumentcategorynumber As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/proposemaildocumentcategorynumber", m_TemplatesSetting))

			Me.txtInvoiceEachFileName.EditValue = ReplaceEmptyValue(invoiceeachfilename, "RE {Nummer} {MDName} - {MDOrt}")
			Me.txtInvoiceFinalFileName.EditValue = ReplaceEmptyValue(invoicezipfilename, "Rechnungen {MDName} - {MDOrt}")
			Me.txtInvoice_eMail_subject.EditValue = ReplaceEmptyValue(Invoice_eMail_subject, "Versand von Rechnung {TMPL_VAR name='invoiceNumber'}")
			Me.txtInvoice_eMail_subject.EditValue = ReplaceEmptyValue(Invoice_eMail_subject, "Versand von Rechnung {TMPL_VAR name='invoiceNumber'}")
			Me.txtmoreinvoices_email_subject.EditValue = ReplaceEmptyValue(moreinvoices_email_subject, "Versand von Rechnungen {TMPL_VAR name='invoiceNumber'}")
			Me.txtpayroll_email_subject.EditValue = ReplaceEmptyValue(payroll_email_subject, "Versand von Lohnabrechnung {TMPL_VAR name='invoiceNumber'}")
			Me.txtmorepayroll_email_subject.EditValue = ReplaceEmptyValue(morepayroll_email_subject, "Versand von Lohnabrechnungen {TMPL_VAR name='invoiceNumber'}")


			lueZVTemplate.EditValue = ReplaceEmptyValue(Zwischenverdienstformular_Doc_eMail_Template, "MailTempl_ZVDoc.txt")
			lueArgbTemplate.EditValue = ReplaceEmptyValue(Arbeitgeberbescheinigung_Doc_eMail_Template, "MailTempl_ArbgDoc.txt")

			Me.txtTplMADoc.EditValue = ReplaceEmptyValue(MADoc_eMail_Template, "MailTempl_MADoc.txt")
			Me.txtTplKDDoc.EditValue = ReplaceEmptyValue(KDDoc_eMail_Template, "MailTempl_KDDoc.txt")
			Me.txtTplZHDDoc.EditValue = ReplaceEmptyValue(ZHDDoc_eMail_Template, "MailTempl_ZHDDoc.txt")
			Me.txtProposeSubject.EditValue = ReplaceEmptyValue(proposemailsubject, String.Empty)


			ccboavailable_employee_wos_template_jobnr.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.CSV
			ccboavailable_employee_wos_template_jobnr.SetEditValue(available_employee_wos_template_jobnr)

			ccboOffMail_tplDocNr.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.CSV
			ccboOffMail_tplDocNr.SetEditValue(OffMail_tplDocNr)

			ccboPMail_tplNr.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.CSV
			ccboPMail_tplNr.SetEditValue(PMail_tplDocNr)
			Dim selectedCategoryNumber = proposemaildocumentcategorynumber
			ccboEmployeeDocCategory.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.CSV
			ccboEmployeeDocCategory.SetEditValue(selectedCategoryNumber)

			Dim ZeugnisDeckblatt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/ZeugnisDeckblatt", m_TemplatesSetting))
			Dim AGBDeckblatt As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AGBDeckblatt", m_TemplatesSetting))
			Dim CreatedLLFieAs As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/CreatedLLFieAs", m_TemplatesSetting))

			Dim AGB4Temp As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AGB4Temp", m_TemplatesSetting))
			Dim AGB4Fest As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AGB4Fest", m_TemplatesSetting))
			Dim eMailImageVar1 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMailImageVar1", m_TemplatesSetting))
			Dim eMailImageVar2 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMailImageVar2", m_TemplatesSetting))
			Dim eMailImageVar3 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMailImageVar3", m_TemplatesSetting))
			Dim eMailImageValue1 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMailImageValue1", m_TemplatesSetting))
			Dim eMailImageValue2 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMailImageValue2", m_TemplatesSetting))
			Dim eMailImageValue3 As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/eMailImageValue3", m_TemplatesSetting))

			Me.txtZDeckblatt.EditValue = ZeugnisDeckblatt
			Me.txtADeckblatt.EditValue = AGBDeckblatt
			Me.txt_LLFilename.EditValue = CreatedLLFieAs

			Me.txtAGBTemp.EditValue = AGB4Temp
			Me.txtAGBFest.EditValue = AGB4Fest

			Me.txt_imgvar1.EditValue = eMailImageVar1
			Me.txt_imgvar2.EditValue = eMailImageVar2
			Me.txt_imgvar3.EditValue = eMailImageVar3
			Me.txt_imgvalue1.EditValue = eMailImageValue1
			Me.txt_imgvalue2.EditValue = eMailImageValue2
			Me.txt_imgvalue3.EditValue = eMailImageValue3


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False

		End Try


		Return result

	End Function


#Region "DropDown Funktionen 2. Seite (Betreuung)"

	' Zoom
	Private Sub Cbo_Zoom_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Zoom.QueryPopUp
		If Me.Cbo_Zoom.Properties.Items.Count = 0 Then ListZoom(Cbo_Zoom)
	End Sub

#End Region

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmDocUtility_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			My.Settings.frmLocation = String.Empty
			My.Settings.iLeft = Me.Top
			My.Settings.iTop = Me.Left
			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub


#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		Dim msg As String = String.Empty
		Dim success As Boolean = True

		Try
			If Not Me.xscDocContent.Enabled Then
				success = SaveProgSettings()

			ElseIf xtabMainControl.SelectedTabPage Is xtabDocListing Then
				success = success AndAlso SaveDocValueInDb()

			End If

			If success Then
				msg = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
				m_UtilityUI.ShowInfoDialog(msg)

			Else
				msg = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
				m_UtilityUI.ShowErrorDialog(msg)

			End If
			bsiInfo.Caption = msg


		Catch ex As Exception

		Finally

		End Try

	End Sub

	Private Sub bbiDesign_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDesign.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiDesign.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'Dim bshowMnu As Boolean = True
		'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		'Dim bShowDesign As Boolean = True
		'Dim liMnu As New List(Of String) From {"Kandidaten- und Kundenstammblatt#mnuEmployee",
		'																			 "Vakanzenstammblatt#mnuVacancy",
		'																			 "Offertenstammblatt#mnuOffers",
		'																			 "Einsatzverträge verwalten#mnuES"}

		'Try
		'	bbiDesign.Manager = Me.BarManager1
		'	BarManager1.ForceInitialize()

		'	Me.bbiDesign.ActAsDropDown = False
		'	Me.bbiDesign.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
		'	Me.bbiDesign.DropDownEnabled = True
		'	Me.bbiDesign.DropDownControl = popupMenu
		'	'MebbiDesign.Visibility = BarItemVisibility.Always
		'	Me.bbiDesign.Enabled = True

		'	For i As Integer = 0 To liMnu.Count - 1
		'		Dim myValue As String() = liMnu(i).Split(CChar("#"))
		'		bshowMnu = myValue(0).ToString <> String.Empty

		'		If bshowMnu Then
		'			popupMenu.Manager = BarManager1

		'			Dim itm As New DevExpress.XtraBars.BarButtonItem
		'			itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
		'			itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

		'			If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
		'			AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
		'		End If

		'	Next

		'Catch ex As Exception
		'	m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		'End Try
	End Sub



	Private Function SaveDocValueInDb() As Boolean
		Dim success As Boolean = True

		success = success AndAlso ValidateInputData()
		If Not success Then Return success
		Try

			Dim data As MandantDocumentData = Nothing
			data = New MandantDocumentData

			data.ID = m_CurrentRecordNumber.GetValueOrDefault(0)

			data.ModulNr = CType(Val(txtModulNr.EditValue), Integer)
			data.RecNr = CType(Val(txtRecNr.EditValue), Integer)
			data.Anzahlkopien = CType(Val(txtAnzKopien.EditValue), Integer)
			data.Bezeichnung = txtBezeichnung.EditValue

			data.jobNr = txtJobNr.EditValue
			data.Meldung0 = txtMeldung1.EditValue
			data.Meldung1 = txtMeldung2.EditValue
			data.TempDocPath = txtAusgabepfad.EditValue
			data.DocName = txtDateiname.EditValue
			data.ExportedFileName = txtExportedFilename.EditValue

			data.ZoomProz = CType(Val(Cbo_Zoom.EditValue), Integer)

			data.ParamCheck = chk_LL_OPTION_NOPARAMETERCHECK.Checked
			data.KonvertName = chk_LL_OPTION_XLATVERNAMES.Checked
			data.IncPrv = chk_LL_OPTION_INCREMENTAL_PREVIEW.Checked
			data.FontDesent = chk_LL_OPTION_INCLUDEFONTDESCENT.Checked
			data.PrintInDiffColor = chk_LL_Print_Lines_In_Different_Color.Checked
			data.InsertFileToDb = chk_LL_Insert_Print_Job_To_Db.Checked

			If m_CurrentRecordNumber.GetValueOrDefault(0) = 0 Then
				success = m_TablesettingDatabaseAccess.AddAssignedMandantDocumentData(data)

			Else
				success = m_TablesettingDatabaseAccess.UpdateAssignedMandantDocumentData(data)

			End If

			If success Then
				LoadDocuments()
				FocusDocrecorde(data.ID)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False
		End Try


		Return success

	End Function

	Private Function SaveProgSettings() As Boolean
		Dim success As Boolean = True

		Try
			Dim selectedValue As String = String.Empty

			'For Each item As Object In lstDocumentCategory.CheckedItems

			'	Dim row As CategoryVieData = CType(item, CategoryVieData)
			'	selectedCategoryNumber &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(selectedCategoryNumber), "", ","), row.CategoryNumber)

			'Next
			selectedValue = String.Empty
			ccboPMail_tplNr.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.List
			For Each item As String In ccboPMail_tplNr.Properties.GetCheckedItems
				selectedValue &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(selectedValue), "", ccboPMail_tplNr.Properties.SeparatorChar), item)
			Next
			XMLWriter4MDFile("Templates", "PMail_tplDocNr", selectedValue)

			selectedValue = String.Empty
			ccboEmployeeDocCategory.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.List
			For Each item As String In ccboEmployeeDocCategory.Properties.GetCheckedItems
				selectedValue &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(selectedValue), "", ccboEmployeeDocCategory.Properties.SeparatorChar), item)
			Next
			XMLWriter4MDFile("Templates", "proposemaildocumentcategorynumber", selectedValue)

			selectedValue = String.Empty
			ccboavailable_employee_wos_template_jobnr.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.List
			For Each item As String In ccboavailable_employee_wos_template_jobnr.Properties.GetCheckedItems
				selectedValue &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(selectedValue), "", ccboavailable_employee_wos_template_jobnr.Properties.SeparatorChar), item)
			Next
			XMLWriter4MDFile("Templates", "available_employee_wos_template_jobnr", selectedValue)

			selectedValue = String.Empty
			ccboOffMail_tplDocNr.Properties.EditValueType = DevExpress.XtraEditors.Repository.EditValueTypeCollection.List
			For Each item As String In ccboOffMail_tplDocNr.Properties.GetCheckedItems
				selectedValue &= String.Format("{0}{1}", If(String.IsNullOrWhiteSpace(selectedValue), "", ccboOffMail_tplDocNr.Properties.SeparatorChar), item)
			Next
			XMLWriter4MDFile("Templates", "OffMail_tplDocNr", selectedValue)


			XMLWriter4MDFile("Sonstiges", "FontName", ReplaceEmptyValue(Me.cbo_FontEdit.Text, "Calibri"))
			XMLWriter4MDFile("Sonstiges", "FontSize", ReplaceEmptyValue(Val(sp_Fontsize.Text), 11))
			XMLWriter4MDFile("Sonstiges", "LL_IndentSize", ReplaceEmptyValue(Val(Me.sp_FontIndent.Text), 20))
			XMLWriter4MDFile("Sonstiges", "LLTemplateExtension", ReplaceEmptyValue(cbo_LLTExtension.Text, "doc"))

			' Mail-Schriften...
			XMLWriter4MDFile("Sonstiges", "MailFontName", ReplaceEmptyValue(Me.cbo_MailFont.Text, "Calibri"))
			XMLWriter4MDFile("Sonstiges", "MailFontSize", ReplaceEmptyValue(Val(sp_MailFontsize.Text), 11))


			' Vorlagen...
			XMLWriter4MDFile("Templates", "eMail-Template-JobNr", lueEinzelOffer.EditValue)
			Dim einzelData = SelectedEinzelOfferTemplate
			Dim bez As String = String.Empty
			If Not einzelData Is Nothing Then
				bez = einzelData.docBez
			End If
			XMLWriter4MDFile("Templates", "eMail-Template", bez)

			XMLWriter4MDFile("Templates", "MassenOffer-eMail-Template-JobNr", Me.lueMassenOffer.EditValue)
			bez = String.Empty
			Dim massenData = SelectedMassenOfferTemplate
			If Not massenData Is Nothing Then
				bez = massenData.docBez
			End If
			XMLWriter4MDFile("Templates", "MassenOffer-eMail-Template", bez)


			XMLWriter4MDFile("Mailing", "invoiceeachfilename", Me.txtInvoiceEachFileName.Text)
			XMLWriter4MDFile("Mailing", "invoicezipfilename", Me.txtInvoiceFinalFileName.Text)

			XMLWriter4MDFile("Mailing", "Invoice-eMail-subject", Me.txtInvoice_eMail_subject.Text)
			XMLWriter4MDFile("Mailing", "moreinvoices-email-subject", Me.txtmoreinvoices_email_subject.Text)
			XMLWriter4MDFile("Mailing", "payroll-email-subject", Me.txtpayroll_email_subject.Text)
			XMLWriter4MDFile("Mailing", "morepayroll-email-subject", Me.txtmorepayroll_email_subject.Text)

			XMLWriter4MDFile("Templates", "Zwischenverdienstformular_Doc-eMail-Template", lueZVTemplate.EditValue)
			XMLWriter4MDFile("Templates", "Arbeitgeberbescheinigung_Doc-eMail-Template", lueArgbTemplate.EditValue)
			XMLWriter4MDFile("Templates", "MADoc-eMail-Template", Me.txtTplMADoc.Text)

			XMLWriter4MDFile("Templates", "KDDoc-eMail-Template", Me.txtTplKDDoc.Text)
			XMLWriter4MDFile("Templates", "ZHDDoc-eMail-Template", Me.txtTplZHDDoc.Text)

			XMLWriter4MDFile("Templates", "proposemailsubject", Me.txtProposeSubject.Text)

			XMLWriter4MDFile("Templates", "ZeugnisDeckblatt", Me.txtZDeckblatt.Text)
			XMLWriter4MDFile("Templates", "AGBDeckblatt", Me.txtADeckblatt.Text)
			XMLWriter4MDFile("Templates", "CreatedLLFieAs", Me.txt_LLFilename.Text)

			XMLWriter4MDFile("Templates", "AGB4Temp", Me.txtAGBTemp.Text)
			XMLWriter4MDFile("Templates", "AGB4Fest", Me.txtAGBFest.Text)


			' Link für Images in Mailvorlagen...
			XMLWriter4MDFile("Templates", "eMailImageVar1", Me.txt_imgvar1.Text)
			XMLWriter4MDFile("Templates", "eMailImageVar2", Me.txt_imgvar2.Text)
			XMLWriter4MDFile("Templates", "eMailImageVar3", Me.txt_imgvar3.Text)

			XMLWriter4MDFile("Templates", "eMailImageValue1", Me.txt_imgvalue1.Text)
			XMLWriter4MDFile("Templates", "eMailImageValue2", Me.txt_imgvalue2.Text)
			XMLWriter4MDFile("Templates", "eMailImageValue3", Me.txt_imgvalue3.Text)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try


		Return success

	End Function

	Function ValidateInputData() As Boolean
		Dim success As Boolean = True
		Dim meldung As String = ""

		' Modul-Nr
		If Me.txtModulNr.Text.Length > 0 Then
			If Not IsNumeric(Me.txtModulNr.Text) Then
				success = False
				meldung += String.Format("Die Modul-Nr. muss numerisch sein.{0}", vbLf)
			Else
				If CInt(Me.txtModulNr.Text) > Int16.MaxValue Or CInt(Me.txtModulNr.Text) < 0 Then
					success = False
					meldung += String.Format("Bitte wählen Sie für die Modul-Nr. eine Zahl zwischen 0 und {0}.{1}", Int16.MaxValue, vbLf)
				End If
			End If
		End If

		' Dokumentennummer (JobNr)
		If Me.txtJobNr.Text.Trim().Length = 0 Then
			success = False
			meldung += String.Format("Es wurde keine Dokumentennummer angegeben.{0}", vbLf)
		End If

		' Anzahl Kopien
		If Not IsNumeric(Me.txtAnzKopien.Text) OrElse CInt(Me.txtAnzKopien.Text) < 0 Then
			Me.txtAnzKopien.Text = "1"
		End If

		' Zoom
		If Not IsNumeric(Me.Cbo_Zoom.Text) OrElse CInt(Me.Cbo_Zoom.Text) < 0 Then
			Me.Cbo_Zoom.Text = "150"
		End If

		' Bezeichnung
		If Me.txtBezeichnung.Text.Trim().Length = 0 Then
			success = False
			meldung += String.Format("Es wurde keine Bezeichnung angegeben.{0}", vbLf)
		End If

		If Not success Then
			Dim strMsg As String = String.Format("Das Dokument kann nicht gespeichert werden.{0}{0}{1}", vbLf, meldung)
			m_UtilityUI.ShowErrorDialog(strMsg)
		End If

		Return success

	End Function

	''' <summary>
	''' new record
	''' </summary>
	Private Sub OnbbiCopyDocumentRecord_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCopyDocumentRecord.ItemClick

		Dim Data = SelectedRecord

		If Not Data Is Nothing Then
			m_CurrentRecordNumber = Nothing

			'Me.txtModulNr.EditValue = 0
			Me.txtRecNr.EditValue = 0
			Me.txtJobNr.EditValue = String.Empty
			'Me.txtBezeichnung.EditValue = String.Empty

			txtJobNr.Focus()

		End If

	End Sub

	Private Sub bbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
		Dim selectedData = SelectedRecord
		Dim success As Boolean = True

		If Not selectedData Is Nothing Then

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
																m_Translate.GetSafeTranslationValue("Daten endgültig löschen?"))) Then

				success = m_TablesettingDatabaseAccess.DeleteAssignedMandantDocumentData(selectedData.ID)
			Else
				Return

			End If

		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnte nicht gelöscht werden."))
		End If

		If success AndAlso LoadDocuments() Then
			FocusDocrecorde(selectedData.ID)
		End If

	End Sub

	''' <summary>
	''' Alle Textfelder leeren um einen neuen Dokument einzugeben.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllEntries()
		ResetControl(xscDocContent)
	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion mit rekursivem Aufruf.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		If TypeOf (con) Is TextBox Then
			Dim tb As TextBox = con
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
			Dim cbo As DevExpress.XtraEditors.TextEdit = con
			cbo.Text = String.Empty
			cbo.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
			cbo.Text = String.Empty
			cbo.EditValue = Nothing

		ElseIf TypeOf (con) Is ListBox Then
			Dim lst As ListBox = con
			lst.Items.Clear()

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
			Dim chk As DevExpress.XtraEditors.CheckEdit = con
			chk.Checked = False

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

		If TypeOf (con) Is TabPage Then
			Dim tabPg As TabPage = con
			tabPg.Text = tabPg.Text.Replace("*", "")
		End If
	End Sub

#End Region


#Region "Sonstige Funktionen..."

	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)    '.Item(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception

		End Try

	End Function

#End Region

	Private Sub DisplayData()
		Dim Data = SelectedRecord

		Try
			If Data Is Nothing Then
				ResetAllEntries()

				Return
			End If

			m_CurrentRecordNumber = Data.ID

			txtModulNr.EditValue = Data.ModulNr
			txtRecNr.EditValue = Data.RecNr
			txtAnzKopien.EditValue = Data.Anzahlkopien
			txtBezeichnung.EditValue = Data.Bezeichnung

			txtJobNr.EditValue = Data.jobNr
			txtMeldung1.EditValue = Data.Meldung0
			txtMeldung2.EditValue = Data.Meldung1
			txtAusgabepfad.EditValue = Data.TempDocPath
			txtDateiname.EditValue = Data.DocName
			txtExportedFilename.EditValue = Data.ExportedFileName

			Cbo_Zoom.EditValue = Data.ZoomProz

			chk_LL_OPTION_NOPARAMETERCHECK.Checked = Data.ParamCheck
			chk_LL_OPTION_XLATVERNAMES.Checked = Data.KonvertName
			chk_LL_OPTION_INCREMENTAL_PREVIEW.Checked = Data.IncPrv
			chk_LL_OPTION_INCLUDEFONTDESCENT.Checked = Data.FontDesent
			chk_LL_Print_Lines_In_Different_Color.Checked = Data.PrintInDiffColor
			chk_LL_Insert_Print_Job_To_Db.Checked = Data.InsertFileToDb

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Private Sub btnDateiname_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim openFile As OpenFileDialog = New OpenFileDialog()
		If openFile.ShowDialog() = DialogResult.OK Then
			Me.txtDateiname.Text = openFile.SafeFileName
		End If
	End Sub

	Private Sub xtabMainControl_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabMainControl.SelectedPageChanged

		' Diese Buttons sind nur dann aktiv, wenn der Reiter Dokumentliste gewählt ist.
		Me.bbiCopyDocumentRecord.Enabled = xtabMainControl.SelectedTabPage Is xtabDocListing
		Me.bbiDelete.Enabled = xtabMainControl.SelectedTabPage Is xtabDocListing
		Me.bbiDesign.Enabled = xtabMainControl.SelectedTabPage Is xtabDocListing

		Me.xscDocContent.Enabled = xtabMainControl.SelectedTabPage Is xtabDocListing

	End Sub

	Private Sub OngvDoc_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvDoc.FocusedRowChanged
		Dim Data = SelectedRecord

		If Not Data Is Nothing Then
			DisplayData()
		Else
			ResetAllEntries()
		End If

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		Dim Data = SelectedRecord

		If Not Data Is Nothing Then
			DisplayData()
		Else
			ResetAllEntries()
		End If


	End Sub

	Private Sub FocusDocrecorde(ByVal recID As Integer)

		Dim listDataSource As BindingList(Of MandantDocumentData) = grdDoc.DataSource

		If listDataSource Is Nothing Then
			Return
		End If

		Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.ID = recID).FirstOrDefault()

		If Not monthlySalaryViewData Is Nothing Then
			'Dim suppressUIEventsState = m_SuppressUIEvents
			'm_SuppressUIEvents = True
			Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
			Dim rowHandle = gvDoc.GetRowHandle(sourceIndex)
			gvDoc.FocusedRowHandle = rowHandle
			'm_SuppressUIEvents = suppressUIEventsState
		End If

	End Sub

	Private Function LoadZVTemplateDropDown() As Boolean
		Dim result As List(Of TemplateData) = Nothing
		Dim docData = CType(gvDoc.DataSource, BindingList(Of MandantDocumentData))

		Try
			Dim assignedData As List(Of MandantDocumentData) = docData.Where(Function(x) x.jobNr.ToUpper.Contains("Mail.ZV".ToUpper)).ToList()
			If Not assignedData Is Nothing Then
				result = New List(Of TemplateData)

				For Each itm In assignedData
					result.Add(New TemplateData With {.jobNr = itm.jobNr, .LabelViewData = itm.LabelViewData, .DocFileName = itm.DocName})
				Next

			End If

			lueZVTemplate.Properties.DataSource = result

			If result.Count = 0 Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Mail-Vorlage für Versand von <b>Zwischenverdienstformular:</b> Keine Daten wurden gefunden.")
				m_UtilityUI.ShowErrorDialog(strMsg)

			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString())
			result = Nothing

		End Try


		Return Not (result Is Nothing)

	End Function

	Private Function LoadARGBTemplateDropDown() As Boolean
		Dim result As List(Of TemplateData) = Nothing
		Dim docData = CType(gvDoc.DataSource, BindingList(Of MandantDocumentData))

		Try
			Dim assignedData As List(Of MandantDocumentData) = docData.Where(Function(x) x.jobNr.ToUpper.Contains("Mail.ARGB".ToUpper)).ToList()
			If Not assignedData Is Nothing Then
				result = New List(Of TemplateData)

				For Each itm In assignedData
					result.Add(New TemplateData With {.jobNr = itm.jobNr, .LabelViewData = itm.LabelViewData, .DocFileName = itm.DocName})
				Next

			End If

			lueArgbTemplate.Properties.DataSource = result

			If result.Count = 0 Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Mail-Vorlage für Versand von <b>Arbeitgeberbescheinigung:</b> Keine Daten wurden gefunden.")
				m_UtilityUI.ShowErrorDialog(strMsg)

			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString())
			result = Nothing

		End Try


		Return Not (result Is Nothing)

	End Function

	Private Function LoadTemplateDropDown() As Boolean
		Dim result As List(Of TemplateData) = Nothing
		Dim docData = CType(gvDoc.DataSource, BindingList(Of MandantDocumentData))

		Try
			Dim assignedData As List(Of MandantDocumentData) = docData.Where(Function(x) x.ModulNr = 20 AndAlso Not x.jobNr.ToUpper.Contains("MAIL.ZV") AndAlso Not x.jobNr.ToUpper.Contains("MAIL.ARGB") AndAlso
																				 Not x.DocName.ToUpper.EndsWith(".LST") AndAlso Not x.DocName.ToUpper.EndsWith(".CRD")).ToList()
			If Not assignedData Is Nothing Then
				result = New List(Of TemplateData)

				For Each itm In assignedData
					result.Add(New TemplateData With {.LabelViewData = itm.LabelViewData, .docBez = itm.DocName, .jobNr = itm.jobNr, .DocFileName = itm.DocName})
				Next

			End If

			lueEinzelOffer.Properties.DataSource = result
			lueMassenOffer.Properties.DataSource = result

			If result.Count = 0 Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.")
				'm_UtilityUI.ShowErrorDialog(strMsg)

			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString())
			result = Nothing

		End Try


		Return Not (result Is Nothing)

	End Function

	Private Function LoadProposeAttachmentJobNumberDropDown() As Boolean
		Dim result As List(Of TemplateData) = Nothing
		Dim docData = CType(gvDoc.DataSource, BindingList(Of MandantDocumentData))

		Try
			Dim assignedData As List(Of MandantDocumentData) = docData.Where(Function(x) Not x.jobNr.ToUpper.Contains("MAIL.ZV".ToUpper) AndAlso Not x.jobNr.ToUpper.Contains("MAIL.ARGB".ToUpper) AndAlso
																				 Not x.jobNr.ToUpper.Contains("AvailableEmployee".ToUpper)).ToList()
			If Not assignedData Is Nothing Then
				result = New List(Of TemplateData)

				For Each itm In assignedData
					result.Add(New TemplateData With {.docBez = itm.LabelViewData, .jobNr = itm.jobNr})
				Next

			End If

			ccboPMail_tplNr.Properties.DataSource = result

			If result.Count = 0 Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert.")
				'm_UtilityUI.ShowErrorDialog(strMsg)

			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString())
			result = Nothing

		End Try


		Return Not (result Is Nothing)

	End Function

	Private Function LoadAvailableEmployeesTemplateJobNumberDropDown() As Boolean
		Dim result As List(Of TemplateData) = Nothing
		Dim docData = CType(gvDoc.DataSource, BindingList(Of MandantDocumentData))

		Try
			Dim assignedData As List(Of MandantDocumentData) = docData.Where(Function(x) x.jobNr.ToUpper.Contains("AvailableEmployee".ToUpper)).ToList()
			If Not assignedData Is Nothing Then
				result = New List(Of TemplateData)

				For Each itm In assignedData
					result.Add(New TemplateData With {.LabelViewData = itm.LabelViewData, .jobNr = itm.jobNr})
				Next

			End If

			ccboavailable_employee_wos_template_jobnr.Properties.DataSource = result

			If result.Count = 0 Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Vorlage für Verfügbare Kandidaten für Versand an WOS: Keine Daten wurden gefunden.")
				'm_UtilityUI.ShowErrorDialog(strMsg)

			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString())
			result = Nothing

		End Try


		Return Not (result Is Nothing)

	End Function

	Private Function LoadccboOffMailTemplateJobNumberDropDown() As Boolean
		Dim result As List(Of TemplateData) = Nothing
		Dim docData = CType(gvDoc.DataSource, BindingList(Of MandantDocumentData))

		Try
			Dim assignedData As List(Of MandantDocumentData) = docData.Where(Function(x) x.jobNr.ToUpper.Contains("offer.email".ToUpper)).ToList()
			If Not assignedData Is Nothing Then
				result = New List(Of TemplateData)

				For Each itm In assignedData
					result.Add(New TemplateData With {.LabelViewData = itm.LabelViewData, .jobNr = itm.jobNr})
				Next

			End If

			ccboOffMail_tplDocNr.Properties.DataSource = result

			If result.Count = 0 Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Vorlage für <b>Offertenversand:</b> Keine Daten wurden gefunden.")
				'm_UtilityUI.ShowErrorDialog(strMsg)

			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString())
			result = Nothing

		End Try


		Return Not (result Is Nothing)

	End Function

	Private Sub OnccboPMail_tplNr_DrawListBoxItem(sender As Object, e As DevExpress.XtraEditors.ListBoxDrawItemEventArgs) Handles ccboPMail_tplNr.DrawListBoxItem
		If (e.Index Mod 2) = 0 Then
			Return
		End If
		e.AllowDrawSkinBackground = False
		'e.Appearance.BackColor = Color.Orange
	End Sub

	Private Sub OnccboEmployeeDocCategory_DrawListBoxItem(sender As Object, e As DevExpress.XtraEditors.ListBoxDrawItemEventArgs) Handles ccboEmployeeDocCategory.DrawListBoxItem
		If (e.Index Mod 2) = 0 Then
			Return
		End If
		e.AllowDrawSkinBackground = False
		'e.Appearance.BackColor = Color.Light
	End Sub

	''' <summary>
	''' Loads the employee document category data.
	''' </summary>
	Private Function LoadEmployeeDocumentCategoriesDropDownData(ByVal language As String) As Boolean
		Dim categoryData = m_EmployeeDbAccess.LoadEmployeeDocumentCategories()

		Dim categoryViewData = Nothing
		If Not categoryData Is Nothing Then

			categoryViewData = New List(Of CategoryVieData)

			For Each category In categoryData

				Dim categoryDescription As String = String.Empty
				Select Case language.ToLower().Trim()
					Case "d", "de"
						categoryDescription = category.DescriptionGerman
					Case "f", "fr"
						categoryDescription = category.DescriptionFrench
					Case "i", "it"
						categoryDescription = category.DescriptionItalian
					Case Else
						categoryDescription = category.DescriptionGerman
				End Select

				categoryViewData.Add(New CategoryVieData With {.CategoryNumber = category.CategoryNumber,
																											 .Description = categoryDescription})

			Next

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kategorieauswahldaten konnten nicht geladen werden."))
			categoryViewData = Nothing

		End If

		ccboEmployeeDocCategory.Properties.DataSource = categoryViewData

		'lstDocumentCategory.DataSource = categoryViewData
		'lstDocumentCategory.ForceInitialize()

		Return Not categoryViewData Is Nothing
	End Function

	'''' <summary>
	'''' Focus a category filter.
	'''' </summary>
	'''' <param name="categoryNumber">The category filter number.</param>
	'Private Sub FocusCategoryFilter(ByVal categoryNumber As Integer)

	'	If Not lstDocumentCategory.DataSource Is Nothing Then

	'		Dim categoryViewData = (CType(lstDocumentCategory.DataSource, List(Of CategoryVieData)))

	'		Dim index = categoryViewData.FindIndex(Function(data) data.CategoryNumber.HasValue AndAlso data.CategoryNumber = categoryNumber)

	'		If index = -1 Then
	'			index = categoryViewData.FindIndex(Function(data) Not data.CategoryNumber.HasValue)
	'		End If

	'		lstDocumentCategory.SelectedIndex = index

	'	End If

	'End Sub

	'Private Sub OncbeEinzelOffer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueEinzelOffer.EditValueChanged
	'	lblEinzelOffer.Text = String.Empty
	'	Dim data = SelectedEinzelOfferTemplate
	'	If Not data Is Nothing Then
	'		lblEinzelOffer.Text = data.docBez
	'	End If

	'End Sub

	'Private Sub OncbeMassenOffer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMassenOffer.EditValueChanged
	'	lblMassenOffer.Text = String.Empty
	'	Dim data = SelectedMassenOfferTemplate
	'	If Not data Is Nothing Then
	'		lblMassenOffer.Text = data.docBez
	'	End If

	'End Sub

	Private Function ReplaceEmptyValue(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) OrElse (String.IsNullOrWhiteSpace(obj)) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function




	Private Class ListItem

		Private m_Value As String
		Private m_Text2Show As String
		Public Sub New(strText2Show As String, strValue As String)
			Me.m_Text2Show = strText2Show
			Me.m_Value = strValue
		End Sub

		Public Property GetText2Show() As String
			Get
				Return Me.m_Text2Show
			End Get
			Set(value As String)
				Me.m_Text2Show = value
			End Set
		End Property

		Public Property GetValue() As String
			Get
				Return Me.m_Value
			End Get
			Set(value As String)
				Me.m_Value = value
			End Set
		End Property

		Public Overrides Function ToString() As String
			Return GetText2Show
		End Function

	End Class



End Class



Public Class TemplateData
	Public Property jobNr As String
	Public Property docBez As String
	Public Property DocFileName As String
	Public Property LabelViewData As String

End Class




