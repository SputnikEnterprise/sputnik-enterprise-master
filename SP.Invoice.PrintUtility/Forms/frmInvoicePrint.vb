

Imports System.Reflection.Assembly
Imports System.IO

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects


Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Pdf
Imports SP.DatabaseAccess.Invoice
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.Utils
Imports SPS.Listing.Print.Utility
Imports SPSSendMail.RichEditSendMail
Imports System.IO.Compression
Imports System.Text
Imports DevExpress.XtraBars.Navigation

Public Class frmInvoicePrint
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private consts"

	Private Const MODULNAME_FOR_DELETE As String = "RE"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
	Private Const MANDANT_XML_SETTING_EZ_ON_SEPRATED_PAGE As String = "MD_{0}/Debitoren/ezonsepratedpage"
	Private Const MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING As String = "MD_{0}/Mailing"

#End Region

#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The invoice data access object.
	''' </summary>
	Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private bAllowedtowrite As Boolean
	Private m_MailingSetting As String

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	Private m_CustomerWOSID As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String
	Private m_InvoiceEachFileName As String
	Private m_InvoiceZipFileName As String

	Private m_SelectedWOSEnun As WOSSENDValue
	Private m_SelectedWhatToPrint As WhatToPrintValue
	Private m_OrderBy As OrderByValue
	Private m_PrintEZOnSeperatedPage As Boolean

	Private m_SearchType As SearchTypeEnum

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

	Private Enum SearchTypeEnum
		PRINT
		EMAIL
	End Enum

#End Region


#Region "private property"

	Private Property WOSProperty4Search As WOSSearchValue
	Private Property WhatToPrintProperty4Search As Integer?

	Private ReadOnly Property PrintEZOnSepratedPage() As Boolean
		Get
			Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_EZ_ON_SEPRATED_PAGE, m_InitializationData.MDData.MDNr))
			Dim ezonsepratedpage As Boolean?
			If String.IsNullOrWhiteSpace(value) Then
				ezonsepratedpage = Nothing
			Else
				ezonsepratedpage = CBool(value)
			End If

			Return ezonsepratedpage.HasValue AndAlso ezonsepratedpage

		End Get
	End Property


#Region "xml data"

	''' <summary>
	''' Gets the email subject for zv.
	''' </summary>
	Private ReadOnly Property InvoiceEachFileName As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/invoiceeachfilename", m_MailingSetting))
			If String.IsNullOrWhiteSpace(settingValue) Then settingValue = "RE {Nummer} {MDName} - {MDOrt}"

			Return settingValue
		End Get
	End Property

	Private ReadOnly Property InvoiceZipFileName As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/invoicezipfilename", m_MailingSetting))
			If String.IsNullOrWhiteSpace(settingValue) Then settingValue = "Rechnungen {MDName} - {MDOrt}"

			Return settingValue
		End Get
	End Property

	Private ReadOnly Property CustomerWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property


#End Region


#End Region


#Region "public property"
	''' <summary>
	''' Gets or sets the preselection data.
	''' </summary>
	Public Property PreselectionData As PreselectionData

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		m_MailingSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING, m_InitializationData.MDData.MDNr)


		TranslateControls()
		Reset()

	End Sub

#End Region


	'''' <summary>
	'''' Gets the selected payroll.
	'''' </summary>
	'''' <returns>The selected employee or nothing if none is selected.</returns>
	'Private ReadOnly Property SelectedRecord As InvoiceData
	'	Get
	'		Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

	'		If Not (gvRP Is Nothing) Then

	'			Dim selectedRows = gvRP.GetSelectedRows()

	'			If (selectedRows.Count > 0) Then
	'				Dim invoice = CType(gvRP.GetRow(selectedRows(0)), InvoiceData)

	'				Return invoice
	'			End If

	'		End If

	'		Return Nothing
	'	End Get

	'End Property


#Region "public methodes"

	''' <summary>
	''' Preselects data.
	''' </summary>
	Public Sub PreselectData()

		Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

		If hasPreselectionData Then

			Dim supressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

			' ---Mandant---
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = PreselectionData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					m_SuppressUIEvents = supressUIEventState
					Return
				End If

			End If

			m_SuppressUIEvents = supressUIEventState
		Else
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = m_InitializationData.MDData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					Return
				End If

			End If

		End If

		Try
			If Not PreselectionData.CustomerNumber Is Nothing Then
				cbo_KDNr.EditValue = PreselectionData.CustomerNumber

			ElseIf Not PreselectionData.CustomerNumbers Is Nothing Then

				Dim customerNumbers As String = String.Join(",", PreselectionData.CustomerNumbers)
				'For Each itm In PreselectionData.CustomerNumbers
				'	customerNumbers &= If(Not String.IsNullOrWhiteSpace(customerNumbers), ",", String.Empty) & itm
				'Next
				cbo_KDNr.EditValue = customerNumbers

			End If

			If Not PreselectionData.InvoiceNumbers Is Nothing Then
				Dim invoiceNumbers As String = String.Join(",", PreselectionData.InvoiceNumbers)
				'For Each itm In PreselectionData.InvoiceNumbers
				'	invoiceNumbers &= If(Not String.IsNullOrWhiteSpace(invoiceNumbers), ",", String.Empty) & itm
				'Next
				cbo_ReNr.EditValue = invoiceNumbers
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

		bbiPrint.Enabled = False
		bbiDelete.Enabled = False
		bbiSendMail.Enabled = False
		bbiExport.Enabled = False

	End Sub


#End Region


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()
	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)

		If Not SelectedData Is Nothing Then
			Dim MandantData = ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)
			m_InitializationData = MandantData

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		Else
			' do nothing
		End If

		m_PrintEZOnSeperatedPage = PrintEZOnSepratedPage
		Me.btnWhatToPrint.Visible = m_PrintEZOnSeperatedPage
		Me.lblWhatToPrint.Visible = m_PrintEZOnSeperatedPage

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiDelete.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiSendMail.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub


#End Region




	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
		gpSuchKriterien.Text = m_Translate.GetSafeTranslationValue(gpSuchKriterien.Text)

		chk_AsCopy.Text = m_Translate.GetSafeTranslationValue(chk_AsCopy.Text)
		chkOpenAmount.Text = m_Translate.GetSafeTranslationValue(chkOpenAmount.Text)
		chkAnonymEmployee.Text = m_Translate.GetSafeTranslationValue(chkAnonymEmployee.Text)

		Dim sTip_AsCopy As DevExpress.Utils.SuperToolTip = New SuperToolTip() With {.AllowHtmlText = DefaultBoolean.True}
		Dim ttTitleAsCopyItem1 As ToolTipTitleItem = New ToolTipTitleItem() With {.Text = m_Translate.GetSafeTranslationValue("Kopie")}
		ttTitleAsCopyItem1.ImageOptions.Image = ImageCollection1.Images("info_16x16.png")

		Dim ttAsCopyItem1 As ToolTipItem = New ToolTipItem() With {.Text = m_Translate.GetSafeTranslationValue("Es werden <b>nur</b> die Rechnungsdetail mit Vermerk ""Kopie"" gedruckt. Die Einzahlungsscheine werden <b>nicht</b> gedruckt.")}
		Dim ttAsCopySeparatorItem1 As ToolTipSeparatorItem = New ToolTipSeparatorItem()
		sTip_AsCopy.Items.Add(ttTitleAsCopyItem1)
		sTip_AsCopy.Items.Add(ttAsCopySeparatorItem1)
		sTip_AsCopy.Items.Add(ttAsCopyItem1)
		chk_AsCopy.SuperTip = sTip_AsCopy

		lblWarnByCopy.Text = m_Translate.GetSafeTranslationValue("Es werden <b>nur</b> die Rechnungsdetail mit Vermerk <i>Kopie</i> gedruckt.<br>Die Einzahlungsscheine werden <b>nicht</b> gedruckt.")

		Dim sTip_OpenAmount As DevExpress.Utils.SuperToolTip = New SuperToolTip() With {.AllowHtmlText = DefaultBoolean.True}
		Dim toolTipTitleItem1 As ToolTipTitleItem = New ToolTipTitleItem() With {.Text = m_Translate.GetSafeTranslationValue("Offener Betrag")}
		toolTipTitleItem1.ImageOptions.Image = ImageCollection1.Images("info_16x16.png")

		Dim toolTipItem1 As ToolTipItem = New ToolTipItem() With {.Text = m_Translate.GetSafeTranslationValue("Wenn aktiviert, druckt das System den offenen Rechnungsbetrag auf Einzahlungsschein.")}
		Dim toolTipItem2 As ToolTipTitleItem = New ToolTipTitleItem() With {.Text = m_Translate.GetSafeTranslationValue("<b>Achtung:</b> die Vorlage für Rechnungsdetail muss angepasst werden!")}

		Dim toolTipSeparatorItem1 As ToolTipSeparatorItem = New ToolTipSeparatorItem()

		sTip_OpenAmount.Items.Add(toolTipTitleItem1)
		sTip_OpenAmount.Items.Add(toolTipItem1)
		sTip_OpenAmount.Items.Add(toolTipSeparatorItem1)
		sTip_OpenAmount.Items.Add(toolTipItem2)
		chkOpenAmount.SuperTip = sTip_OpenAmount


		Dim sTip_AnonymEmployee As DevExpress.Utils.SuperToolTip = New SuperToolTip() With {.AllowHtmlText = DefaultBoolean.True} 'chkOpenAmount.SuperTip
		Dim ttTitleAnonymItem1 As ToolTipTitleItem = New ToolTipTitleItem() With {.Text = m_Translate.GetSafeTranslationValue("Anonymisieren")}
		ttTitleAnonymItem1.ImageOptions.Image = ImageCollection1.Images("info_16x16.png")

		Dim ttAnonymItem1 As ToolTipItem = New ToolTipItem() With {.Text = m_Translate.GetSafeTranslationValue("Wenn aktiviert, dann druckt das System <b>keine</b> Kandidatennamen aus.")}
		Dim ttAnonymSeparatorItem1 As ToolTipSeparatorItem = New ToolTipSeparatorItem()
		sTip_AnonymEmployee.Items.Add(ttTitleAnonymItem1)
		sTip_AnonymEmployee.Items.Add(ttAnonymSeparatorItem1)
		sTip_AnonymEmployee.Items.Add(ttAnonymItem1)
		chkAnonymEmployee.SuperTip = sTip_AnonymEmployee

		lblWhatToPrint.Text = m_Translate.GetSafeTranslationValue(lblWhatToPrint.Text)

		tnpPrint.Caption = m_Translate.GetSafeTranslationValue(tnpPrint.Caption)
		tnpMail.Caption = m_Translate.GetSafeTranslationValue(tnpMail.Caption)
		tnpEMailSummery.Caption = m_Translate.GetSafeTranslationValue(tnpEMailSummery.Caption)

		tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OffText)
		tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OnText)

		tnpWOSSetting.Caption = m_Translate.GetSafeTranslationValue(tnpWOSSetting.Caption)
		tnpMailSetting.Caption = m_Translate.GetSafeTranslationValue(tnpMailSetting.Caption)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lblReNr.Text = m_Translate.GetSafeTranslationValue(lblReNr.Text)
		lblWOSBez.Text = m_Translate.GetSafeTranslationValue(lblWOSBez.Text)

		Me.bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiPrintinfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)

	End Sub


	Private Sub Reset()

		ResetMandantenDropDown()

		Me.CboSort.Properties.Items.Clear()
		Me.CboSort.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsnummer")))
		Me.CboSort.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsempfänger")))
		Me.CboSort.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Faktura-Datum")))

		Me.CboSort.EditValue = String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsnummer"))
		lblWarnByCopy.Visible = chk_AsCopy.Checked

		m_PrintEZOnSeperatedPage = PrintEZOnSepratedPage
		m_CustomerWOSID = CustomerWOSID
		m_InvoiceEachFileName = InvoiceEachFileName
		m_InvoiceZipFileName = InvoiceZipFileName

		m_SearchType = SearchTypeEnum.PRINT
		cbEMail.Checked = False
		cbPrintWOS.Checked = True

		tpMainSetting.SelectedPage = tnpWOSSetting
		tnpMailSetting.PageVisible = False
		bbiSendMail.Visibility = BarItemVisibility.Never

		If Not m_mandant.AllowedExportCustomer2WOS(lueMandant.EditValue, Now.Year) Then
			Me.btnWOSProperty.Text = String.Empty
			Me.btnWOSProperty.Visible = False
			Me.lblWOSBez.Visible = False
			cbEMail.Enabled = False
			tnpMailSetting.Enabled = False
		End If

		tpMainView.SelectedPageIndex = 0
		tnpMail.PageVisible = False
		tnpEMailSummery.PageVisible = False

		CreateWOSPopup()
		WOSProperty4Search = WOSSearchValue.SearchAllCustomer

		CreateWhatToPrintPopup()
		WhatToPrintProperty4Search = 0

		Me.btnWhatToPrint.Visible = m_PrintEZOnSeperatedPage
		Me.lblWhatToPrint.Visible = m_PrintEZOnSeperatedPage


		ResetInvoicePrintGrid()
		ResetInvoiceEMailGrid()
		ResetEMailSummeryGrid()


		LoadMandantenDropDown()
		'LoadMailAttachmentSetting()
		'LoadSearchTypeSetting()

		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 551) Then Me.bbiDelete.Visibility = BarItemVisibility.Never
		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554) Then
			Me.bbiPrint.Visibility = BarItemVisibility.Never
			Me.bbiExport.Visibility = BarItemVisibility.Never
		End If

		bbiPrint.Enabled = False
		bbiDelete.Enabled = False
		bbiExport.Enabled = False
		bbiSendMail.Enabled = False

	End Sub

	'Private Sub LoadMailAttachmentSetting()

	'	Dim data = New List(Of OptionSettingData) From {New OptionSettingData With {.Value = 0, .Label = m_Translate.GetSafeTranslationValue("Alle in einer ZIP-Datei packen")}}
	'	'New EMailAttachmentSettingData With {.Value = 1, .Label = m_Translate.GetSafeTranslationValue("Alle in einer PDF-Datei zusammen stellen (mehrere Seiten)")},
	'	'New EMailAttachmentSettingData With {.Value = 2, .Label = m_Translate.GetSafeTranslationValue("Alle in einzelne PDF-Datei erstellen")}}

	'	rep_MailAttachmentSetting.Properties.Items.Clear()
	'	For Each itm In data
	'		rep_MailAttachmentSetting.Properties.Items.Add(New RadioGroupItem With {.Description = itm.Label, .Value = itm.Value})
	'	Next
	'	rep_MailAttachmentSetting.EditValue = 0

	'End Sub

	'Private Sub LoadSearchTypeSetting()

	'	Dim data = New List(Of OptionSettingData) From {New OptionSettingData With {.Value = 0, .Label = m_Translate.GetSafeTranslationValue("Drucken / WOS")},
	'	New OptionSettingData With {.Value = 1, .Label = m_Translate.GetSafeTranslationValue("Email-Versand")}}

	'	rep_SearchType.Properties.Items.Clear()
	'	For Each itm In data
	'		rep_SearchType.Properties.Items.Add(New RadioGroupItem With {.Description = itm.Label, .Value = itm.Value})
	'	Next
	'	rep_SearchType.EditValue = 0

	'End Sub


	Private Sub ResetInvoicePrintGrid()

		gvPrint.OptionsView.ShowIndicator = False
		gvPrint.OptionsView.ShowAutoFilterRow = True
		gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPrint.OptionsView.ShowFooter = False
		gvPrint.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvPrint.Columns.Clear()


		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "SelectedRec"
		columnSelectedRec.FieldName = "SelectedRec"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 50
		gvPrint.Columns.Add(columnSelectedRec)

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLONr.OptionsColumn.AllowEdit = False
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Rechnung-Nr.")
		columnLONr.Name = "RENr"
		columnLONr.FieldName = "RENr"
		columnLONr.Width = 60
		columnLONr.Visible = True
		gvPrint.Columns.Add(columnLONr)

		Dim columnInvoiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnInvoiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnInvoiceDate.OptionsColumn.AllowEdit = False
		columnInvoiceDate.Caption = m_Translate.GetSafeTranslationValue("Fakturadatum")
		'columnInvoiceDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		'columnInvoiceDate.DisplayFormat.FormatString = "G"
		columnInvoiceDate.Name = "InvoiceDate"
		columnInvoiceDate.FieldName = "InvoiceDate"
		columnInvoiceDate.Visible = True
		columnInvoiceDate.Width = 80
		gvPrint.Columns.Add(columnInvoiceDate)


		Dim columnzeitraum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzeitraum.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnzeitraum.OptionsColumn.AllowEdit = False
		columnzeitraum.Caption = m_Translate.GetSafeTranslationValue("Sprache")
		columnzeitraum.Name = "Language"
		columnzeitraum.FieldName = "Language"
		columnzeitraum.Visible = False
		columnzeitraum.Width = 50
		gvPrint.Columns.Add(columnzeitraum)

		Dim columnemployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeFullname.OptionsColumn.AllowEdit = False
		columnemployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnemployeeFullname.Name = "CustomerName"
		columnemployeeFullname.FieldName = "CustomerName"
		columnemployeeFullname.Visible = True
		columnemployeeFullname.Width = 150
		gvPrint.Columns.Add(columnemployeeFullname)

		Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBetragInk.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBetragInk.OptionsColumn.AllowEdit = False
		columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag ink. MwSt.")
		columnBetragInk.Name = "BetragInk"
		columnBetragInk.FieldName = "BetragInk"
		columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
		columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBetragInk.DisplayFormat.FormatString = "N2"
		columnBetragInk.Visible = True
		columnBetragInk.Width = 60
		gvPrint.Columns.Add(columnBetragInk)

		Dim columnBezahlt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBezahlt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBezahlt.OptionsColumn.AllowEdit = False
		columnBezahlt.Caption = m_Translate.GetSafeTranslationValue("Bezahlt")
		columnBezahlt.Name = "Bezahlt"
		columnBezahlt.FieldName = "Bezahlt"
		columnBezahlt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBezahlt.AppearanceHeader.Options.UseTextOptions = True
		columnBezahlt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBezahlt.DisplayFormat.FormatString = "N2"
		columnBezahlt.Visible = True
		columnBezahlt.Width = 60
		gvPrint.Columns.Add(columnBezahlt)


		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedon.OptionsColumn.AllowEdit = False
		columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columncreatedon.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columncreatedon.DisplayFormat.FormatString = "G"
		columncreatedon.Name = "CreatedOn"
		columncreatedon.FieldName = "CreatedOn"
		columncreatedon.Visible = False
		columncreatedon.Width = 100
		gvPrint.Columns.Add(columncreatedon)

		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.OptionsColumn.AllowEdit = False
		columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "CreatedFrom"
		columncreatedfrom.FieldName = "CreatedFrom"
		columncreatedfrom.Visible = False
		columncreatedfrom.Width = 100
		gvPrint.Columns.Add(columncreatedfrom)


		grdPrint.DataSource = Nothing

	End Sub

	Private Sub ResetInvoiceEMailGrid()

		gvInvoiceEMail.OptionsView.ShowIndicator = False
		gvInvoiceEMail.OptionsView.ShowAutoFilterRow = True
		gvInvoiceEMail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvInvoiceEMail.OptionsView.ShowFooter = False
		gvInvoiceEMail.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvInvoiceEMail.Columns.Clear()


		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "SelectedRec"
		columnSelectedRec.FieldName = "SelectedRec"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 50
		gvInvoiceEMail.Columns.Add(columnSelectedRec)

		Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLONr.OptionsColumn.AllowEdit = False
		columnLONr.Caption = m_Translate.GetSafeTranslationValue("Rechnung-Nr.")
		columnLONr.Name = "RENr"
		columnLONr.FieldName = "RENr"
		columnLONr.Width = 60
		columnLONr.Visible = True
		gvInvoiceEMail.Columns.Add(columnLONr)

		Dim columnemployeeFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeFullname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeFullname.OptionsColumn.AllowEdit = False
		columnemployeeFullname.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnemployeeFullname.Name = "CustomerName"
		columnemployeeFullname.FieldName = "CustomerName"
		columnemployeeFullname.Visible = True
		columnemployeeFullname.Width = 150
		gvInvoiceEMail.Columns.Add(columnemployeeFullname)

		Dim columnREEmail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnREEmail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnREEmail.OptionsColumn.AllowEdit = False
		columnREEmail.Caption = m_Translate.GetSafeTranslationValue("EMail")
		columnREEmail.Name = "REEmail"
		columnREEmail.FieldName = "REEmail"
		columnREEmail.Visible = True
		columnREEmail.Width = 100
		gvInvoiceEMail.Columns.Add(columnREEmail)


		grdInvoiceEMail.DataSource = Nothing

	End Sub

	Private Sub ResetEMailSummeryGrid()

		gvEMailSummery.OptionsView.ShowIndicator = False
		gvEMailSummery.OptionsView.ShowAutoFilterRow = True
		gvEMailSummery.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEMailSummery.OptionsView.ShowFooter = False
		gvEMailSummery.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvEMailSummery.Columns.Clear()


		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "SelectedRec"
		columnSelectedRec.FieldName = "SelectedRec"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 50
		gvEMailSummery.Columns.Add(columnSelectedRec)

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerNumber.OptionsColumn.AllowEdit = False
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Width = 60
		columnCustomerNumber.Visible = False
		gvEMailSummery.Columns.Add(columnCustomerNumber)

		Dim columnCompanyname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompanyname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCompanyname.OptionsColumn.AllowEdit = False
		columnCompanyname.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCompanyname.Name = "Companyname"
		columnCompanyname.FieldName = "Companyname"
		columnCompanyname.Visible = True
		columnCompanyname.Width = 150
		gvEMailSummery.Columns.Add(columnCompanyname)

		Dim columnREEmail As New DevExpress.XtraGrid.Columns.GridColumn()
		columnREEmail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnREEmail.OptionsColumn.AllowEdit = False
		columnREEmail.Caption = m_Translate.GetSafeTranslationValue("EMail")
		columnREEmail.Name = "REEMail"
		columnREEmail.FieldName = "REEMail"
		columnREEmail.Visible = True
		columnREEmail.Width = 100
		gvEMailSummery.Columns.Add(columnREEmail)

		Dim columnNumberOfInvoicesInJob As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNumberOfInvoicesInJob.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnNumberOfInvoicesInJob.OptionsColumn.AllowEdit = False
		columnNumberOfInvoicesInJob.Caption = m_Translate.GetSafeTranslationValue("Anzahl Rechnungen")
		columnNumberOfInvoicesInJob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnNumberOfInvoicesInJob.Name = "NumberOfInvoicesInJob"
		columnNumberOfInvoicesInJob.FieldName = "NumberOfInvoicesInJob"
		columnNumberOfInvoicesInJob.Visible = True
		columnNumberOfInvoicesInJob.Width = 100
		gvEMailSummery.Columns.Add(columnNumberOfInvoicesInJob)


		grdEMailSummery.DataSource = Nothing

	End Sub

	Private Function LoadInvoicePrintList() As Boolean

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim invoiceNumbers = InvoiceNumberToSearch()
		Dim customerNumbers = customerNumberToSearch()

		If Val(CboSort.EditValue) = 0 Then
			m_OrderBy = OrderByValue.OrderByInvoiceNumber
		ElseIf Val(CboSort.EditValue) = 1 Then
			m_OrderBy = OrderByValue.OrderByCustomerName
		Else
			m_OrderBy = OrderByValue.OrderByInvoiceDate
		End If

		Dim wosValue As WOSSearchValue = WOSProperty4Search
		Dim groupByEMail As Boolean = True

		Dim searchConditions As New InvoicePrintSearchConditionData With {.MDNr = lueMandant.EditValue}
		searchConditions.InvoiceNumbers = invoiceNumbers
		searchConditions.CustomerNumbers = customerNumbers
		searchConditions.GroupByEMail = groupByEMail
		searchConditions.WOSValueEnum = wosValue
		searchConditions.OrderByEnum = m_OrderBy

		Dim listOfData As IEnumerable(Of InvoiceData) '= m_ListingDatabaseAccess.LoadInvoiceData(searchConditions) 'lueMandant.EditValue, invoiceNumbers.ToArray, customerNumbers.ToArray, m_OrderBy, wosValue, groupByEMail)
		listOfData = m_ListingDatabaseAccess.LoadInvoiceData(searchConditions)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
						Select New InvoiceData With {.Art = person.Art,
																				 .ID = person.ID,
																				 .CreditInvoiceAutomated = person.CreditInvoiceAutomated,
																				 .InvoiceDate = person.InvoiceDate,
																				 .Language = person.Language,
																				 .CustomerName = person.CustomerName,
																				 .REEmail = person.REEmail,
																				 .BetragInk = person.BetragInk,
																				 .Bezahlt = person.Bezahlt,
																				 .MDNr = person.MDNr,
																				 .PrintWithReport = person.PrintWithReport,
																				 .RefNr = person.RefNr,
																				 .KDNr = person.KDNr,
																				 .RENr = person.RENr,
																				 .DocGuid = person.DocGuid,
																				 .Send2WOS = person.Send2WOS,
																				 .CreatedOn = person.CreatedOn,
																				 .CreatedFrom = person.CreatedFrom,
																				 .SelectedRec = tgsSelection.EditValue
																				}).ToList()

		Dim listDataSource As BindingList(Of InvoiceData) = New BindingList(Of InvoiceData)

		For Each p In gridData
			If Not (Not String.IsNullOrWhiteSpace(p.REEmail) AndAlso tgsNoEMailCustomers.EditValue) Then listDataSource.Add(p)
		Next

		grdPrint.DataSource = listDataSource
		bsiPrintinfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		SplashScreenManager.CloseForm(False)


		Return Not listOfData Is Nothing

	End Function

	Private Function LoadInvoiceEMailList() As Boolean

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim invoiceNumbers = InvoiceNumberToSearch()
		Dim customerNumbers = customerNumberToSearch()

		If Val(CboSort.EditValue) = 0 Then
			m_OrderBy = OrderByValue.OrderByInvoiceNumber
		ElseIf Val(CboSort.EditValue) = 1 Then
			m_OrderBy = OrderByValue.OrderByCustomerName
		Else
			m_OrderBy = OrderByValue.OrderByInvoiceDate
		End If

		Dim wosValue As WOSSearchValue = WOSProperty4Search
		Dim groupByEMail As Boolean = True

		Dim searchConditions As New InvoicePrintSearchConditionData With {.MDNr = lueMandant.EditValue}
		searchConditions.InvoiceNumbers = invoiceNumbers
		searchConditions.CustomerNumbers = customerNumbers
		searchConditions.GroupByEMail = groupByEMail
		searchConditions.WOSValueEnum = wosValue
		searchConditions.OrderByEnum = m_OrderBy

		Dim listOfData As IEnumerable(Of InvoiceData)

		listOfData = m_ListingDatabaseAccess.LoadInvoiceForEMailSendingData(searchConditions)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
						Select New InvoiceData With {.CreditInvoiceAutomated = person.CreditInvoiceAutomated,
																				 .CustomerName = person.CustomerName,
																				 .REEmail = person.REEmail,
																				 .SendAsZip = person.SendAsZip,
																				 .OneInvoicePerMail = person.OneInvoicePerMail,
																				 .MDNr = person.MDNr,
																				 .PrintWithReport = person.PrintWithReport,
																				 .KDNr = person.KDNr,
																				 .RENr = person.RENr,
																				 .SelectedRec = tgsSelection.EditValue
																				}).ToList()

		Dim listDataSource As BindingList(Of InvoiceData) = New BindingList(Of InvoiceData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdInvoiceEMail.DataSource = listDataSource
		bsiPrintinfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		SplashScreenManager.CloseForm(False)


		Return Not listOfData Is Nothing

	End Function


#Region "Formhandle"

	Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
		Me.Dispose()
	End Sub

	Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		SplashScreenManager.CloseForm(False)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.NoEMailCustomersForPrint = tgsNoEMailCustomers.EditValue
			My.Settings.IndividalFilesForEMail = tgsIndividalFiles.EditValue

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Onfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			If My.Settings.iHeight > 0 Then Me.Height = My.Settings.iHeight
			If My.Settings.iWidth > 0 Then Me.Width = My.Settings.iWidth
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			tgsNoEMailCustomers.EditValue = My.Settings.NoEMailCustomersForPrint
			tgsIndividalFiles.EditValue = My.Settings.IndividalFilesForEMail

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
		End Try

	End Sub

#End Region


	Private Sub CreatePrintPopupMenu()

		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Rechnungen Drucken#PrintRE",
																					 "Drucken ohne Übermittlung#PrintRE",
																					 "_WOS -> übermitteln / restliche -> Drucken#SendWOS_PrintRest",
																					 "Drucken mit Übermittlung#SendAndPrint"}
		Try

			bbiPrint.Manager = Me.BarManager1
			Dim allowedEmployeWOS As Boolean = m_mandant.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year)
			BarManager1.ForceInitialize()
			If allowedEmployeWOS Then
				liMnu.RemoveAt(0)

			Else
				liMnu.RemoveRange(1, liMnu.Count - 1)

			End If

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString.Replace("_", ""))
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(0).StartsWith("_") Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintRE".ToUpper
					m_SelectedWOSEnun = WOSSENDValue.PrintWithoutSending

				Case "SendWOS_PrintRest".ToUpper
					m_SelectedWOSEnun = WOSSENDValue.PrintOtherSendWOS

				Case "SendAndPrint".ToUpper
					m_SelectedWOSEnun = WOSSENDValue.PrintAndSend

				Case Else
					Return

			End Select
			Select Case WhatToPrintProperty4Search
				Case 0
					m_SelectedWhatToPrint = WhatToPrintValue.DetailAndEZ

				Case 1
					m_SelectedWhatToPrint = WhatToPrintValue.Detail

				Case 2
					m_SelectedWhatToPrint = WhatToPrintValue.EZ

			End Select

			Dim listData = GetSelectedInvoiceNumbers()
			If listData.Count > 0 Then
				StartPrinting()

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub CreateWOSPopup()

		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim BarManagerContextMenu As New BarManager
		'Dim liMnu As New List(Of String) From {"Nur WOS-Kunden#0", "Keine WOS-Kunden#1", "Alle#2"}

		Dim data = New List(Of MenuSettingData) From {New MenuSettingData With {.Value = 0, .Label = m_Translate.GetSafeTranslationValue("Nur WOS-Kunden")},
			New MenuSettingData With {.Value = 1, .Label = m_Translate.GetSafeTranslationValue("Keine WOS-Kunden")},
			New MenuSettingData With {.Value = 2, .Label = "-" & m_Translate.GetSafeTranslationValue("Alle")}}

		Try
			Me.btnWOSProperty.DropDownControl = popupMenu
			For Each itm In data

				If Not String.IsNullOrWhiteSpace(itm.Label) Then
					popupMenu.Manager = BarManagerContextMenu
					Dim bbiItem As New DevExpress.XtraBars.BarButtonItem
					Dim caption As String = itm.Label
					Dim beginGroup As Boolean = False

					If caption.StartsWith("-") Then
						beginGroup = True
						caption = itm.Label.Remove(0, 1)
					End If

					bbiItem.Caption = caption
					bbiItem.Name = itm.Value


					popupMenu.AddItem(bbiItem).BeginGroup = beginGroup

					AddHandler bbiItem.ItemClick, AddressOf GetWOSPopupMnu

				End If

			Next

			popupMenu.ItemLinks(2).Focus()
			btnWOSProperty.Text = popupMenu.ItemLinks(2).Caption


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub GetWOSPopupMnu(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.btnWOSProperty.Text = e.Item.Caption
		WOSProperty4Search = CType(CShort(e.Item.Name), WOSSearchValue)
	End Sub

	Private Sub CreateWhatToPrintPopup()

		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim BarManagerContextMenu As New BarManager
		'Dim liMnu As New List(Of String) From {"Detail + Einzahlungsschein#0", "Nur Rechnungsdetail#1", "Nur Einzahlungsschein#2"}
		Dim data = New List(Of MenuSettingData) From {New MenuSettingData With {.Value = 0, .Label = m_Translate.GetSafeTranslationValue("Detail + Einzahlungsschein")},
			New MenuSettingData With {.Value = 1, .Label = "-" & m_Translate.GetSafeTranslationValue("Nur Rechnungsdetail")},
			New MenuSettingData With {.Value = 2, .Label = m_Translate.GetSafeTranslationValue("Nur Einzahlungsschein")}}

		Try
			Me.btnWhatToPrint.DropDownControl = popupMenu
			For Each itm In data

				If Not String.IsNullOrWhiteSpace(itm.Label) Then
					popupMenu.Manager = BarManagerContextMenu
					Dim bbiItem As New DevExpress.XtraBars.BarButtonItem
					Dim caption As String = itm.Label
					Dim beginGroup As Boolean = False

					If caption.StartsWith("-") Then
						beginGroup = True
						caption = itm.Label.Remove(0, 1)
					End If

					bbiItem.Caption = caption
					bbiItem.Name = itm.Value


					popupMenu.AddItem(bbiItem).BeginGroup = beginGroup

					AddHandler bbiItem.ItemClick, AddressOf GetWhatToPrintPopupMnu

				End If

			Next

			'Me.btnWhatToPrint.DropDownControl = popupMenu
			'For i As Integer = 0 To liMnu.Count - 1
			'	Dim myValue As String() = liMnu(i).Split(CChar("#"))

			'	If myValue(0).ToString <> String.Empty Then
			'		popupMenu.Manager = BarManager1

			'		Dim itm As New DevExpress.XtraBars.BarButtonItem

			'		itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
			'		itm.Name = myValue(1).ToString

			'		popupMenu.AddItem(itm)
			'		AddHandler itm.ItemClick, AddressOf GetWhatToPrintPopupMnu

			'	End If

			'Next
			popupMenu.ItemLinks(0).Focus()
			btnWhatToPrint.Text = popupMenu.ItemLinks(0).Caption


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub GetWhatToPrintPopupMnu(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.btnWhatToPrint.Text = e.Item.Caption
		WhatToPrintProperty4Search = CType(CShort(e.Item.Name), Integer)
		If WhatToPrintProperty4Search = 2 Then
			chk_AsCopy.Checked = False
		End If
		chk_AsCopy.Visible = WhatToPrintProperty4Search <> 2
	End Sub

	Private Sub OnbbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		SearchData()
	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		For Each itm In BarManager1.Items
			Dim value = itm.name.ToString.ToLower
			If value = "SendWOS_PrintRest".ToLower OrElse value = "SendAndPrint".ToLower Then
				itm.enabled = Not chk_AsCopy.Checked
			End If
		Next

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		Dim result = DeleteInvoice()

		If result.Value Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Rechnung wurde gelöscht."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(result.Message))
		End If
		SearchData()

	End Sub

	Private Sub SearchData()
		Dim success As Boolean = True

		bbiPrint.Visibility = BarItemVisibility.Never ' Enabled = False
		bbiDelete.Visibility = BarItemVisibility.Never 'Enabled = False
		bbiSendMail.Visibility = BarItemVisibility.Never 'Enabled = False
		bbiExport.Visibility = BarItemVisibility.Never 'Enabled = False

		tnpEMailSummery.PageVisible = False

		If m_SearchType = SearchTypeEnum.PRINT Then
			tnpPrint.PageVisible = True
			tnpMail.PageVisible = False

			tpMainView.SelectedPage = tnpPrint

			success = success AndAlso LoadInvoicePrintList()

			If success Then CreatePrintPopupMenu()

			bbiPrint.Visibility = BarItemVisibility.Always ' Enabled = False
			bbiDelete.Visibility = BarItemVisibility.Always 'Enabled = False
			bbiSendMail.Visibility = BarItemVisibility.Never 'Enabled = False
			bbiExport.Visibility = BarItemVisibility.Always 'Enabled = False

			bbiPrint.Enabled = gvPrint.RowCount > 0
			bbiDelete.Enabled = gvPrint.RowCount > 0
			bbiExport.Enabled = gvPrint.RowCount > 0

		Else
			tnpPrint.PageVisible = False
			tnpMail.PageVisible = True

			tpMainView.SelectedPageIndex = 1

			success = success AndAlso LoadInvoiceEMailList()
			bbiSendMail.Visibility = BarItemVisibility.Always
			bbiSendMail.Enabled = gvInvoiceEMail.RowCount > 0

		End If

		SplashScreenManager.CloseForm(False)

	End Sub

	''' <summary>
	''' Deletes the invoice.
	''' </summary>
	Private Function DeleteInvoice() As DeleteResult

		Dim filename As String = String.Empty
		Dim success As Boolean = True
		Dim result As DeleteResult = New DeleteResult With {.Value = True}
		Dim msg As String = String.Empty

		If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie Rechnung wirklich löschen?"), m_Translate.GetSafeTranslationValue("Rechnung löschen")) = False) Then
			Return New DeleteResult With {.Value = False, .Message = "Der Vorgang wurde abgebrochen!"}
		End If
		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim listData = GetSelectedInvoiceData()
		If listData.Count = 0 Then
			msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt!")

			Return New DeleteResult With {.Value = False, .Message = msg}
		End If

		Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = Me.Handle, .PrintInvoiceAsCopy = False, .ShowAsDesign = False, .ExportPrintInFiles = True}

		For Each itm In listData

			Dim invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(itm.RENr)

			If invoiceData Is Nothing Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Die Rechnung {0} wurde nicht gefunden!"), itm.RENr)

				Return New DeleteResult With {.Value = False, .Message = msg}
			End If

			Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
			_setting.InvoiceNumbers = New List(Of Integer)(New Integer() {itm.RENr})
			printUtil.PrintData = _setting
			Dim exportResult = printUtil.PrintInvoice()
			printUtil.Dispose()

			If exportResult.JobResultInvoiceData.Count = 0 Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Die Rechnungsvorlage {0} konnte nicht erstellt werden."), itm.RENr)

				Return New DeleteResult With {.Value = False, .Message = msg}

			Else
				filename = exportResult.JobResultInvoiceData(0).ExportedFileName
				If exportResult.JobResultInvoiceData.Count > 1 Then
					Dim pdfDocument As New PdfDocumentProcessor()
					pdfDocument.LoadDocument(filename)

					For i As Integer = 1 To _setting.ExportedFiles.Count - 1
						pdfDocument.AppendDocument(_setting.ExportedFiles(i))
						pdfDocument.SaveDocument(filename)
					Next
					pdfDocument.CloseDocument()
				End If

				If Not File.Exists(filename) Then
					msg = String.Format("Die Datei für die Rechnungsvorlage {0} konnte nicht gefunden werden.", itm.RENr)
					m_Logger.LogWarning(msg)

					Return New DeleteResult With {.Value = False, .Message = msg}
				End If

			End If
			Dim fileByte() = m_Utility.LoadFileBytes(filename)

			If success AndAlso result.Value Then result = DeleteAssignedInvoice(itm, fileByte)
			success = success AndAlso result.Value

			itm.DocGuid = invoiceData.REDoc_Guid
			If success AndAlso Not String.IsNullOrWhiteSpace(m_CustomerWOSID) AndAlso Not String.IsNullOrWhiteSpace(itm.DocGuid) Then
				Dim deleteWOSResult = printUtil.DeleteCustomerInvoiceFromWOS(itm)
				result.Message = deleteWOSResult.Message

				success = success AndAlso deleteWOSResult.Value
			End If

			If Not success Then Exit For
		Next
		SplashScreenManager.CloseForm(False)


		Return result

	End Function

	Private Function DeleteAssignedInvoice(ByVal itm As InvoiceData, ByVal fileByte() As Byte) As DeleteResult
		Dim success As DeleteResult = New DeleteResult With {.Value = True}
		Dim msg As String = String.Empty

		Dim result = m_InvoiceDatabaseAccess.DeleteInvoiceAndInsertInvoiceDocumentIntoDeleteDb(itm.ID, MODULNAME_FOR_DELETE,
																							   String.Format("{0}", m_InitializationData.UserData.UserFullNameWithComma),
																							   m_InitializationData.UserData.UserNr, fileByte)
		Select Case result
			Case DeleteREResult.ResultCanNotDeleteBecauseMonthIsClosed
				msg = String.Format("Die Rechnung {0} kann nicht gelöscht werden, da der Monat bereits abgeschlossen ist.", itm.RENr)
				Return New DeleteResult With {.Value = False, .Message = msg}

			Case DeleteREResult.ResultCanNotDeleteBecauseOfExistingZE
				msg = String.Format("Die Rechnung {0} kann nicht gelöscht werden, da bereits ein Zahlungseingang existiert.", itm.RENr)
				Return New DeleteResult With {.Value = False, .Message = msg}

			Case DeleteREResult.ResultCanNotDeleteBecauseOfPartlyPayed
				msg = String.Format("Die Rechnung kann nicht gelöscht werden, da bereits eine Teilzahlung existiert.", itm.RENr)
				Return New DeleteResult With {.Value = False, .Message = msg}

			Case DeleteREResult.ResultDeleteError
				msg = String.Format("Die Rechnung {0} konnte nicht gelöscht werden.", itm.RENr)
				Return New DeleteResult With {.Value = False, .Message = msg}

			Case DeleteREResult.ResultDeleteOk

				Return success

		End Select


		Return success

	End Function

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim invoiceNumbers = GetSelectedInvoiceNumbers()
			If invoiceNumbers.Count > 0 Then
				StartExporting()

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

			End If

		Catch ex As Exception
			m_Logger.LogInfo(String.Format("{0}:{1}", strMethodeName, ex.ToString))

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Sub StartPrinting()
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Dim invoiceNumbers = GetSelectedInvoiceNumbers()

		Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = Me.Handle,
			.InvoiceNumbers = invoiceNumbers,
			.PrintInvoiceAsCopy = chk_AsCopy.CheckState,
			.ShowAsDesign = ShowDesign,
			.OrderByEnum = m_OrderBy,
			.WhatToPrintValueEnum = m_SelectedWhatToPrint,
			.WOSSendValueEnum = m_SelectedWOSEnun,
			.PrintOpenAmount = chkOpenAmount.Checked,
			.PrintEmployeeAsAnonym = chkAnonymEmployee.Checked
		}

		Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
		printUtil.PrintData = _setting
		Dim result = printUtil.PrintInvoice()

		printUtil.Dispose()

		If Not ShowDesign AndAlso result.Printresult AndAlso Not m_SelectedWOSEnun = WOSSENDValue.PrintWithoutSending Then
			Dim msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")

			m_UtilityUI.ShowInfoDialog(msg)

		ElseIf result.Printresult = False Then
			m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)

		End If


	End Sub

	Sub StartExporting()

		Dim invoiceNumbers = GetSelectedInvoiceNumbers()
		Dim exportPath As String = m_InitializationData.UserData.spTempInvoicePath

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim result = ExportInvoices(invoiceNumbers)
		If result.Printresult AndAlso result.JobResultInvoiceData.Count > 0 Then

			Dim newMergedFilename As String = Path.Combine(m_InitializationData.UserData.spTempInvoicePath, String.Format("Rechnungen {0}.ex", m_InitializationData.MDData.MDName, m_InitializationData.MDData.MDCity)) ' result.JobResultInvoiceData(0).CustomerNumber))
			newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")

			If File.Exists(newMergedFilename) Then
				Try
					File.Delete(newMergedFilename)
				Catch ex As Exception
					newMergedFilename = Path.Combine(m_InitializationData.UserData.spTempInvoicePath, String.Format("{0} - {1}.ex", Path.GetRandomFileName, result.JobResultInvoiceData(0).CustomerNumber))
					newMergedFilename = Path.ChangeExtension(newMergedFilename, ".pdf")

				End Try
			End If

			Dim fileList As New List(Of String)
			For Each itm In result.JobResultInvoiceData
				fileList.Add(itm.ExportedFileName)
			Next

			If result.JobResultInvoiceData.Count > 1 Then
				Dim pdfUtility As New SP.Infrastructure.PDFUtilities.Utilities
				Dim success = pdfUtility.MergePdfFiles(fileList.ToArray, newMergedFilename)
			Else
				newMergedFilename = result.JobResultInvoiceData(0).ExportedFileName
			End If

			SplashScreenManager.CloseForm(False)
			Dim msg As String = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich exportiert und zusammengestellt.")
			m_UtilityUI.ShowInfoDialog(msg)

			Process.Start("explorer.exe", "/select," & newMergedFilename)
			'Process.Start(newFilename)

		Else
				Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.") & "<br>{0}<br>{1}",
																					 exportPath, result.PrintresultMessage)
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(strMsg)
		End If

	End Sub

	Private Function ExportInvoices(ByVal invoiceNumbers As List(Of Integer)) As PrintResult
		Dim result As New PrintResult

		Dim deleteDirectory = m_Utility.ClearAssignedFiles(m_InitializationData.UserData.spTempInvoicePath, "*.*", SearchOption.TopDirectoryOnly)

		Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = Me.Handle, .InvoiceNumbers = invoiceNumbers,
			.PrintInvoiceAsCopy = False,
			.ShowAsDesign = False,
			.OrderByEnum = m_OrderBy,
			.WhatToPrintValueEnum = m_SelectedWhatToPrint,
			.ExportPrintInFiles = True,
			.PrintOpenAmount = chkOpenAmount.Checked,
			.PrintEmployeeAsAnonym = chkAnonymEmployee.Checked
		}
		Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
		printUtil.PrintData = _setting
		result = printUtil.PrintInvoice()

		printUtil.Dispose()

		Return result
	End Function

	Private Function GetSelectedInvoiceNumbers() As List(Of Integer)

		Dim result As List(Of Integer)

		gvPrint.FocusedColumn = gvPrint.VisibleColumns(1)
		grdPrint.RefreshDataSource()
		Dim printList As BindingList(Of InvoiceData) = grdPrint.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		result = New List(Of Integer)

		For Each receiver In sentList
			result.Add(receiver.RENr)
		Next


		Return result

	End Function

	Private Function GetSelectedInvoiceData() As BindingList(Of InvoiceData)

		Dim result As BindingList(Of InvoiceData)

		gvPrint.FocusedColumn = gvPrint.VisibleColumns(1)
		grdPrint.RefreshDataSource()
		Dim printList As BindingList(Of InvoiceData) = grdPrint.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		result = New BindingList(Of InvoiceData)

		For Each receiver In sentList
			result.Add(receiver)
		Next


		Return result

	End Function

	Private Function GetSelectedInvoiceEMailNumbers() As List(Of Integer)

		Dim result As List(Of Integer)

		gvInvoiceEMail.FocusedColumn = gvInvoiceEMail.VisibleColumns(1)
		grdInvoiceEMail.RefreshDataSource()
		Dim printList As BindingList(Of InvoiceData) = grdInvoiceEMail.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		result = New List(Of Integer)

		For Each receiver In sentList
			result.Add(receiver.RENr)
		Next


		Return result

	End Function

	Private Function GetSelectedInvoiceEMailData() As BindingList(Of InvoiceData)

		Dim result As BindingList(Of InvoiceData)

		gvInvoiceEMail.FocusedColumn = gvInvoiceEMail.VisibleColumns(1)
		grdInvoiceEMail.RefreshDataSource()
		Dim printList As BindingList(Of InvoiceData) = grdInvoiceEMail.DataSource
		Dim sentList = (From r In printList Where r.SelectedRec = True).ToList()

		result = New BindingList(Of InvoiceData)

		For Each receiver In sentList
			result.Add(receiver)
		Next


		Return result

	End Function

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub OngvPrint_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvPrint.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPrint.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, InvoiceData)

				Select Case column.Name.ToLower
					Case "kdnr".ToLower
						If String.IsNullOrWhiteSpace(viewData.CustomerName) Then OpenSelectedcustomer(viewData.KDNr)


					Case Else
						If viewData.RENr > 0 Then OpenSelectedInvoice(viewData.RENr)

				End Select

			End If

		End If

	End Sub

	Sub OngvInvoiceEMail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvInvoiceEMail.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvInvoiceEMail.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, InvoiceData)

				Select Case column.Name.ToLower
					Case "kdnr".ToLower
						If String.IsNullOrWhiteSpace(viewData.CustomerName) Then OpenSelectedcustomer(viewData.KDNr)


					Case Else
						If viewData.RENr > 0 Then OpenSelectedInvoice(viewData.RENr)

				End Select

			End If

		End If

	End Sub

	Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled
		If tpMainView.SelectedPage Is tnpPrint Then
			SelDeSelectPrintItems(tgsSelection.EditValue)
		Else
			SelDeSelectEMailItems(tgsSelection.EditValue)
		End If
	End Sub

	Private Sub SelDeSelectPrintItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of InvoiceData) = grdPrint.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.SelectedRec = selectItem
			Next
		End If

		gvPrint.RefreshData()

	End Sub

	Private Sub SelDeSelectEMailItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of InvoiceData) = grdInvoiceEMail.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.SelectedRec = selectItem
			Next
		End If

		gvInvoiceEMail.RefreshData()

	End Sub

#Region "Helpers"

	Private Function ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

	Private Sub OpenSelectedcustomer(ByVal customerNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customerNumber)
			hub.Publish(openMng)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub OpenSelectedInvoice(ByVal invoiceNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, invoiceNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Function InvoiceNumberToSearch() As List(Of Integer)
		Dim invoiceNumbers As New List(Of Integer)
		Dim inputInvoiceComma = cbo_ReNr.EditValue.ToString.Split(New String() {",", ";"}, StringSplitOptions.RemoveEmptyEntries)
		Dim inputInvoiceTil = cbo_ReNr.EditValue.ToString.Split(New String() {"-"}, StringSplitOptions.RemoveEmptyEntries)
		If inputInvoiceComma.Length > 1 AndAlso inputInvoiceTil.Length > 1 Then
			SplashScreenManager.CloseForm(False)

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie können entweder Rechnungsnummer durch ',', ';' oder '-' trennen. Bitte versuchen Sie eine Variante aus."))
			Return invoiceNumbers
		End If
		If inputInvoiceComma.Length >= 1 AndAlso Not inputInvoiceComma(0).Contains("-") Then
			For Each itm In inputInvoiceComma
				invoiceNumbers.Add(CType(itm, Integer))
			Next

		ElseIf inputInvoiceTil.Length > 1 Then
			Dim firstNumber As Integer = CType(inputInvoiceTil(0), Integer)
			Dim lastNumber As Integer = CType(inputInvoiceTil(1), Integer)

			If firstNumber > lastNumber And lastNumber > 0 Then
				lastNumber = firstNumber
				firstNumber = lastNumber
			End If

			If lastNumber > 0 Then
				For i As Integer = firstNumber To lastNumber
					invoiceNumbers.Add(CType(i, Integer))
				Next
			Else
				invoiceNumbers.Add(CType(firstNumber, Integer))
				invoiceNumbers.Add(CType(lastNumber, Integer))

			End If

		End If

		Return invoiceNumbers
	End Function

	Private Function customerNumberToSearch() As List(Of Integer)
		Dim customerNumbers As New List(Of Integer)
		Dim inputCustomerComma = cbo_KDNr.EditValue.ToString.Split(New String() {",", ";"}, StringSplitOptions.RemoveEmptyEntries)
		Dim inputCustomerTil = cbo_KDNr.EditValue.ToString.Split(New String() {"-"}, StringSplitOptions.RemoveEmptyEntries)
		If inputCustomerComma.Length > 1 AndAlso inputCustomerTil.Length > 1 Then
			SplashScreenManager.CloseForm(False)

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie können entweder die Kundennummer durch ',', ';' oder '-' trennen. Bitte versuchen Sie eine Variante aus."))
			Return customerNumbers
		End If
		If inputCustomerComma.Length >= 1 AndAlso Not inputCustomerComma(0).Contains("-") Then
			For Each itm In inputCustomerComma
				customerNumbers.Add(CType(itm, Integer))
			Next

		ElseIf inputCustomerTil.Length > 1 Then
			Dim firstNumber As Integer = CType(inputCustomerTil(0), Integer)
			Dim lastNumber As Integer = CType(inputCustomerTil(1), Integer)

			If firstNumber > lastNumber And lastNumber > 0 Then
				lastNumber = firstNumber
				firstNumber = lastNumber
			End If

			If lastNumber > 0 Then
				For i As Integer = firstNumber To lastNumber
					customerNumbers.Add(CType(i, Integer))
				Next
			Else
				customerNumbers.Add(CType(firstNumber, Integer))
				customerNumbers.Add(CType(lastNumber, Integer))

			End If

		End If

		Return customerNumbers
	End Function

	Private Sub chk_AsCopy_CheckedChanged(sender As Object, e As EventArgs) Handles chk_AsCopy.CheckedChanged

		If btnWhatToPrint.Visible Then
			Dim PopupMenu = CType(btnWhatToPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

			If chk_AsCopy.Checked Then
				WhatToPrintProperty4Search = 1
				PopupMenu.ItemLinks(1).Focus()
				btnWhatToPrint.Text = PopupMenu.ItemLinks(1).Caption

			Else
				WhatToPrintProperty4Search = 0
				PopupMenu.ItemLinks(0).Focus()
				btnWhatToPrint.Text = PopupMenu.ItemLinks(0).Caption

			End If

		End If
		lblWarnByCopy.Visible = chk_AsCopy.Checked

	End Sub

	Private Sub OncbPrintWOS_Click(sender As Object, e As EventArgs) Handles cbPrintWOS.Click
		If m_SuppressUIEvents Then Return

		If cbPrintWOS.Checked AndAlso m_SearchType = SearchTypeEnum.PRINT Then Return
		m_SuppressUIEvents = True

		m_SearchType = SearchTypeEnum.PRINT

		cbPrintWOS.Checked = True
		cbEMail.Checked = False

		SetGridVisibility()

		m_SuppressUIEvents = False

	End Sub

	Private Sub cbEMail_Click(sender As Object, e As EventArgs) Handles cbEMail.Click
		If m_SuppressUIEvents Then Return

		If cbEMail.Checked AndAlso m_SearchType = SearchTypeEnum.EMAIL Then Return
		m_SuppressUIEvents = True

		m_SearchType = SearchTypeEnum.EMAIL

		cbPrintWOS.Checked = False
		cbEMail.Checked = True

		SetGridVisibility()

		m_SuppressUIEvents = False

	End Sub

	Private Sub SetGridVisibility()

		tnpMailSetting.PageVisible = False
		tnpWOSSetting.PageVisible = False

		tnpPrint.PageVisible = m_SearchType = SearchTypeEnum.PRINT
		tnpMail.PageVisible = m_SearchType = SearchTypeEnum.EMAIL

		If m_SearchType = SearchTypeEnum.PRINT Then
			tnpWOSSetting.PageVisible = True
			tpMainView.SelectedPage = tnpPrint
			tpMainSetting.SelectedPage = tnpWOSSetting

		Else
			tnpMailSetting.PageVisible = False
			tpMainView.SelectedPage = tnpMail
			'tpMainSetting.SelectedPage = tnpMailSetting
			tpMainSetting.SelectedPage = Nothing

		End If

		bbiPrint.Enabled = gvPrint.RowCount > 0
		bbiDelete.Enabled = gvPrint.RowCount > 0
		bbiSendMail.Enabled = gvInvoiceEMail.RowCount > 0
		bbiExport.Enabled = gvPrint.RowCount > 0

		bbiPrint.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Always, BarItemVisibility.Never)
		bbiDelete.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Always, BarItemVisibility.Never)
		bbiSendMail.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Never, BarItemVisibility.Always)
		bbiExport.Visibility = If(m_SearchType = SearchTypeEnum.PRINT, BarItemVisibility.Always, BarItemVisibility.Never)

	End Sub


#End Region

	Private Class OptionSettingData
		Public Property Value As Integer
		Public Property Label As String
	End Class

	Private Class MenuSettingData
		Public Property Value As Integer
		Public Property Label As String
	End Class

	Private Sub cbEMail_CheckedChanged(sender As Object, e As EventArgs) Handles cbEMail.CheckedChanged

	End Sub
End Class
