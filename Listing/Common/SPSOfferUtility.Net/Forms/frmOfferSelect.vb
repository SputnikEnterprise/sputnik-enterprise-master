
'Imports System.ComponentModel
'Imports DevExpress.XtraEditors.Controls
'Imports DevExpress.LookAndFeel

'Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.UI
'Imports SP.Infrastructure

'Imports SP.Infrastructure.Messaging
'Imports SP.Infrastructure.Messaging.Messages

'Imports System.IO

'Imports SPProgUtility.Mandanten
'Imports DevExpress.XtraSplashScreen

'Imports System.Data.SqlClient
'Imports System.Threading

'Imports DevExpress.XtraGrid.Columns

'Imports SPSOfferUtility_Net.ClsOfDetails
''Imports SPSSendMail
'Imports SPProgUtility.ProgPath

'Imports SP.DatabaseAccess.TableSetting
'Imports SP.DatabaseAccess.Listing
'Imports SP.DatabaseAccess.Listing.DataObjects
'Imports SP.DatabaseAccess.Customer
'Imports SP.DatabaseAccess.TableSetting.DataObjects
'Imports SP.KD.CPersonMng.UI
'Imports SP.DatabaseAccess.Propose
'Imports System.Threading.Tasks

'Public Class frmOfferSelect



'#Region "Private Fields"

'	Private Shared m_Logger As ILogger = New Logger()

'	Private MyDataReader As SqlDataReader
'	Private _ClsReg As New SPProgUtility.ClsDivReg
'	Private _ClsLog As New SPProgUtility.ClsEventLog

'	''' <summary>
'	''' The Initialization data.
'	''' </summary>
'	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

'	''' <summary>
'	''' The translation value helper.
'	''' </summary>
'	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

'	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
'	Private m_ListingDatabaseAccess As IListingDatabaseAccess
'	Private m_TableSettingDatabaseAccess As ITablesDatabaseAccess
'	Private m_ProposeDatabaseAccess As IProposeDatabaseAccess


'	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath
'	Private m_mandant As Mandant
'	Private m_path As SPProgUtility.ProgPath.ClsProgPath


'	''' <summary>
'	''' Utility functions.
'	''' </summary>
'	Private m_Utility As Utility

'	''' <summary>
'	''' UI Utility functions.
'	''' </summary>
'	Private m_UtilityUI As UtilityUI

'	Private m_connectionString As String
'	Private m_CustomerBulkEMailData As IEnumerable(Of CustomerBulkEMailData)


'	Private PrintListingThread As Thread

'	Private m_ExportedFilename As String
'	Private m_SuccessfullSentData As Integer
'	Private m_ErrorSentData As Integer


'	Private m_SendAsStaging As Boolean
'	Private m_LOGFileName As String
'	Private m_MessageGuid As String

'	Private m_RecipientFieldData As List(Of RecipientFieldData)
'	Private m_AssignedRecipientFieldData As RecipientFieldData
'	Private m_AssignedTemplateData As DocumentData

'	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

'	Private m_CurrentOfferNumber As Integer?

'#End Region

'#Region "public properties"

'	Public Property m_GetSearchQuery As String

'#End Region


'#Region "private properties"

'	Private Property PrintJobNr As String
'	Private Property SQL4Print As String
'	Private Property Sql2Open4Grid As String

'	Private ReadOnly Property SelectedTemplateRecord As DocumentData
'		Get
'			Dim gvData = TryCast(grdTemplates.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

'			If Not (gvData Is Nothing) Then

'				Dim selectedRows = gvData.GetSelectedRows()

'				If (selectedRows.Count > 0) Then
'					Dim employee = CType(gvData.GetRow(selectedRows(0)), DocumentData)
'					Return employee
'				End If

'			End If

'			Return Nothing
'		End Get

'	End Property

'	Private ReadOnly Property CreateNewMessageGuid As String
'		Get
'			Return Guid.NewGuid.ToString()
'		End Get
'	End Property

'#End Region


'#Region "Constructor"

'	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

'		DevExpress.UserSkins.BonusSkins.Register()
'		DevExpress.Skins.SkinManager.EnableFormSkins()

'		m_InitializationData = _setting
'		m_mandant = New Mandant
'		m_path = New ClsProgPath
'		m_Utility = New Utility
'		m_UtilityUI = New UtilityUI
'		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
'		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

'		m_connectionString = m_InitializationData.MDData.MDDbConn
'		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
'		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
'		m_TableSettingDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
'		m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

'		m_AdvisorEmailAddress = LoadUserEMailAddress
'		m_EmailTemplateFilename = LoadTplFilename

'		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)
'		If System.IO.File.Exists(m_MandantXMLFile) Then
'			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
'		End If

'		m_MailingSetting = String.Format(MANDANT_XML_SETTING_MAILING, m_InitializationData.MDData.MDNr)
'		m_TemplatesSetting = String.Format(MANDANT_XML_SETTING_TEMPLATES, m_InitializationData.MDData.MDNr)
'		m_SmtpServer = EMailSMTPServer
'		m_SmtpPort = EMailSMTPServerPort
'		m_MessageGuid = CreateNewMessageGuid




'		' Dieser Aufruf ist für den Designer erforderlich.
'		InitializeComponent()

'		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
'		Try
'			Me.KeyPreview = True
'			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
'			If strStyleName <> String.Empty Then
'				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
'			End If

'		Catch ex As Exception
'			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

'		End Try


'		Reset()
'		TranslateControls()

'		AddHandler gvTemplates.RowCellClick, AddressOf Ongv_TemplateRowCellClick
'		AddHandler gvEMail.RowCellClick, AddressOf Ongv_EMailRowCellClick
'		AddHandler lueOffer.EditValueChanged, AddressOf OnlueOffer_EditValueChanged

'	End Sub


'#End Region


'#Region "public methodes"

'	Public Function LoadData() As Boolean
'		Dim result As Boolean = True

'		result = result AndAlso LoadOfferDropDownData()

'		' "Vorlage für Mail-Versand", "Vorlage für Fax-Versand", "Sonstige Vorlagen"
'		cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Mail-Versand"), "Mail"))
'		cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Fax-Versand"), "Fax"))
'		cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Sonstige Vorlagen"), "Sonst"))
'		cboMailTemplate.SelectedIndex = 0

'		CboFormat.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("PDF"), "PDF"))
'		CboFormat.SelectedIndex = 0

'		cboEmailTemplateFilename.Properties.Items.Add(m_EmailTemplateFilename)
'		cboEmailTemplateFilename.EditValue = m_EmailTemplateFilename

'		result = result AndAlso LoadAssignedOfferAdvisorData()
'		result = result AndAlso LoadDocTemplateDropDownData("Mail")
'		result = result AndAlso LoadRecipientFieldData()
'		result = result AndAlso LoadCustomerDropDownData()
'		result = result AndAlso LoadEMailData()

'		Return result
'	End Function

'#End Region


'#Region "Private Methods"

'	''' <summary>
'	'''  Trannslate controls.
'	''' </summary>
'	Private Sub TranslateControls()

'		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

'		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
'		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
'		Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(Me.cmdClose.Text)

'		Me.lblOfNr.Text = m_Translate.GetSafeTranslationValue(Me.lblOfNr.Text)

'		Me.lblVersandmethode.Text = m_Translate.GetSafeTranslationValue(Me.lblVersandmethode.Text)
'		Me.lblVorhandeneOfferte.Text = m_Translate.GetSafeTranslationValue(Me.lblVorhandeneOfferte.Text)
'		Me.lblExportformat.Text = m_Translate.GetSafeTranslationValue(Me.lblExportformat.Text)

'		Me.CmdExport.Text = m_Translate.GetSafeTranslationValue(Me.CmdExport.Text)
'		Me.btnSendTelefax.Text = m_Translate.GetSafeTranslationValue(Me.btnSendTelefax.Text)
'		Me.CmdPrint.Text = m_Translate.GetSafeTranslationValue(Me.CmdPrint.Text)

'		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)

'	End Sub


'	Private Sub Reset()

'		m_CurrentOfferNumber = Nothing
'		m_CurrentCustomerNumber = Nothing
'		m_CurrentCResponsibleNumber = Nothing
'		lueOffer.EditValue = Nothing

'		ResetTemplateGridData()
'		ResetEMailGridData()
'		ResetOfferDropDown()
'		ResetCustomerDropDown()

'	End Sub

'	Private Sub ResetTemplateGridData()

'		gvTemplates.OptionsView.ShowIndicator = False
'		gvTemplates.OptionsView.ShowAutoFilterRow = True
'		gvTemplates.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
'		gvTemplates.OptionsView.ShowFooter = False
'		gvTemplates.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

'		gvTemplates.Columns.Clear()

'		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnMANr.OptionsColumn.AllowEdit = False
'		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Job-Nr.")
'		columnMANr.Name = "JobNr"
'		columnMANr.FieldName = "JobNr"
'		columnMANr.Visible = False
'		gvTemplates.Columns.Add(columnMANr)

'		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
'		columncustomername.OptionsColumn.AllowEdit = False
'		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
'		columncustomername.Name = "Bezeichnung"
'		columncustomername.FieldName = "Bezeichnung"
'		columncustomername.BestFit()
'		columncustomername.Visible = True
'		gvTemplates.Columns.Add(columncustomername)


'		grdTemplates.DataSource = Nothing


'	End Sub

'	Private Sub ResetOfferDropDown()

'		lueOffer.Properties.DisplayMember = "OfferLabel"
'		lueOffer.Properties.ValueMember = "OfferNumber"

'		gvOffer.OptionsView.ShowIndicator = False
'		gvOffer.OptionsView.ShowColumnHeaders = True
'		gvOffer.OptionsView.ShowFooter = False

'		gvOffer.OptionsView.ShowAutoFilterRow = True
'		gvOffer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
'		gvOffer.Columns.Clear()

'		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
'		columnCustomerNumber.Name = "OfferNumber"
'		columnCustomerNumber.FieldName = "OfferNumber"
'		columnCustomerNumber.Visible = True
'		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		gvOffer.Columns.Add(columnCustomerNumber)

'		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
'		columnCompany1.Name = "OfferLabel"
'		columnCompany1.FieldName = "OfferLabel"
'		columnCompany1.Visible = True
'		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		gvOffer.Columns.Add(columnCompany1)

'		lueOffer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
'		lueOffer.Properties.NullText = String.Empty
'		lueOffer.EditValue = Nothing

'	End Sub

'	''' <summary>
'	''' Resets the customer drop down.
'	''' </summary>
'	Private Sub ResetCustomerDropDown()

'		lueCustomer.Properties.DisplayMember = "Company1"
'		lueCustomer.Properties.ValueMember = "CustomerNumber"

'		gvCustomer.OptionsView.ShowIndicator = False
'		gvCustomer.OptionsView.ShowColumnHeaders = True
'		gvCustomer.OptionsView.ShowFooter = False

'		gvCustomer.OptionsView.ShowAutoFilterRow = True
'		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
'		gvCustomer.Columns.Clear()

'		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
'		columnCustomerNumber.Name = "CustomerNumber"
'		columnCustomerNumber.FieldName = "CustomerNumber"
'		columnCustomerNumber.Visible = True
'		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		gvCustomer.Columns.Add(columnCustomerNumber)

'		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
'		columnCompany1.Name = "Company1"
'		columnCompany1.FieldName = "Company1"
'		columnCompany1.Visible = True
'		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		gvCustomer.Columns.Add(columnCompany1)

'		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
'		columnStreet.Name = "Street"
'		columnStreet.FieldName = "Street"
'		columnStreet.Visible = True
'		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		gvCustomer.Columns.Add(columnStreet)

'		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
'		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
'		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
'		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
'		columnPostcodeAndLocation.Visible = True
'		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'		gvCustomer.Columns.Add(columnPostcodeAndLocation)

'		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
'		lueCustomer.Properties.NullText = String.Empty
'		lueCustomer.EditValue = Nothing

'	End Sub

'#End Region


'	Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'		Try
'			Me.Close()
'			Me.Dispose()

'		Catch ex As Exception
'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

'		End Try

'	End Sub

'	'Private Sub frmOfferSelect_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'	'	_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_0", CStr(Me.cboMailTemplate.SelectedIndex))
'	'	_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_1", CStr(Me.CboFormat.SelectedIndex))
'	'	_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_3", lueCustomer.EditValue)

'	'	My.Settings.lastRecipientField = rgRecipients.EditValue
'	'	My.Settings.Save()

'	'End Sub

'	'Private Sub frmOfferSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'	'	Dim iLastIndex_0 As Integer = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_0").ToString))
'	'	Dim iLastIndex_1 As Integer = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_1").ToString))
'	'	Dim strLastKDNr As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", Me.Name & "_3")

'	'	lueCustomer.EditValue = strLastKDNr

'	'End Sub

'	Private Function LoadAssignedOfferAdvisorData() As Boolean

'		m_CurrentUserData = m_ListingDatabaseAccess.LoadUserInformationData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
'		If m_CurrentUserData Is Nothing Then
'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzer Daten konnten nicht geladen werden."))

'			Return False
'		End If

'		Return Not m_CurrentUserData Is Nothing
'	End Function

'	Private Function LoadDocTemplateDropDownData(ByVal bTemplateArt As String) As Boolean

'		grdTemplates.DataSource = Nothing
'		Dim offerData = m_TableSettingDatabaseAccess.LoadTemplateDataForSendBulkCustomer(bTemplateArt)

'		If offerData Is Nothing Then
'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlage Daten konnen nicht geladen werden."))
'			Return False
'		End If

'		Dim templateGridData = (From person In offerData
'								Select New DocumentData With {
'												  .JobNr = person.JobNr,
'												  .Bezeichnung = person.Bezeichnung
'												  }).ToList()

'		Dim listDataSource As BindingList(Of DocumentData) = New BindingList(Of DocumentData)

'		For Each p In templateGridData
'			listDataSource.Add(p)
'		Next
'		grdTemplates.DataSource = listDataSource


'		Return Not listDataSource Is Nothing

'	End Function

'	Private Function LoadOfferDropDownData() As Boolean

'		Dim offerData = m_ListingDatabaseAccess.LoadOfferDataToSendEmail(m_InitializationData.UserData.UserFiliale)
'		If offerData Is Nothing Then
'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Offerte Daten konnen nicht geladen werden."))
'			Return False
'		End If

'		lueOffer.EditValue = Nothing
'		lueOffer.Properties.DataSource = offerData


'		Return Not offerData Is Nothing

'	End Function

'	''' <summary>
'	''' Loads the employee drop down data.
'	''' </summary>
'	Private Function LoadCustomerDropDownData() As Boolean

'		Dim customerData = m_CustomerDatabaseAccess.LoadCustomerData(m_InitializationData.UserData.UserFiliale)

'		If customerData Is Nothing Then
'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))
'			Return False
'		End If

'		lueCustomer.EditValue = Nothing
'		lueCustomer.Properties.DataSource = customerData

'		Return True

'	End Function

'	Sub Ongv_TemplateRowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
'		m_AssignedTemplateData = SelectedTemplateRecord
'	End Sub

'	Private Sub cboMailTemplate_EditValueChanged(sender As Object, e As System.EventArgs) Handles cboMailTemplate.EditValueChanged
'		Dim cv As ComboValue = DirectCast(Me.cboMailTemplate.SelectedItem, ComboValue)

'		LoadDocTemplateDropDownData(cv.ComboValue)

'	End Sub

'	Private Sub cboMailTemplate_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboMailTemplate.QueryPopUp
'		Me.cboMailTemplate.Properties.Items.Clear()

'		Me.cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Mail-Versand"), "Mail"))
'		Me.cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Fax-Versand"), "Fax"))
'		Me.cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Sonstige Vorlagen"), "Sonstige"))

'	End Sub

'	Sub StartPrinting(ByVal bForExport As Boolean)
'		Dim iKDNr As Integer = 0
'		Dim iKDZNr As Integer = 0
'		Dim bResult As Boolean = True
'		Dim bWithKD As Boolean = True
'		Dim tplData = SelectedTemplateRecord
'		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

'		Try
'			If tplData Is Nothing Then
'				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Vorlage ausgewählt."))

'				Return
'			End If
'			If grdEMail.DataSource Is Nothing Then Return
'			Dim CustomerNumber As Integer = 0
'			Dim CResponsibleNumber As Integer = 0

'			Dim data = CType(grdEMail.DataSource, IEnumerable(Of CustomerBulkEMailData))
'			data = (From r In data Where r.Selected = True).ToList()
'			If data Is Nothing OrElse data.Count = 0 Then
'				data = CType(grdEMail.DataSource, IEnumerable(Of CustomerBulkEMailData))
'				If data Is Nothing Then Return
'			End If
'			GetSelectedRecipientFieldData()

'			CustomerNumber = data(0).KDNr
'			CResponsibleNumber = data(0).ZHDRecNr

'			Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = m_InitializationData, .offerNumber = m_CurrentOfferNumber.GetValueOrDefault(0),
'				.JobNr2Print = tplData.JobNr, .ShowAsExport = bForExport, .ShowAsDesgin = ShowDesign}

'			_setting.customerNumber = CustomerNumber
'			_setting.cresponsibleNumber = CResponsibleNumber

'			Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
'			Dim strOfferblattFileName As String = printTemplate.PrintOfferTemplate()

'			If bForExport Then Process.Start(strOfferblattFileName)


'		Catch ex As Exception
'			m_Logger.LogError(ex.ToString)

'		End Try


'		Return

'	End Sub

'	Private Sub CmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdPrint.Click
'		StartPrinting(False)
'	End Sub

'	Private Sub CmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdExport.Click
'		StartPrinting(True)
'	End Sub

'	'Private Sub CmdMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
'	'	SendData2Mail()
'	'End Sub


'#Region "Multitreading..."

'	'Private Sub SendMessagesToAssignedlist()


'	'	btnSendEMail.Enabled = False
'	'	btnSendTestEMail.Enabled = False

'	'	ClsOfDetails.GetMessageGuid = m_MessageGuid

'	'	txtAllMessage.EditValue = String.Empty
'	'	m_LOGFileName = String.Format("{0}{1}.{2}", _ClsProgSetting.GetSpSFiles2DeletePath, m_MessageGuid, "tmp")
'	'	_ClsLog.WriteTempLogFile(String.Format("***Programmstart: {0}", Now.ToString), m_LOGFileName)
'	'	m_Logger.LogInfo(String.Format("mailing ist started..."))

'	'	Try
'	'		Dim result = PerformSendingEmailCallAsync()

'	'		_ClsLog.WriteTempLogFile(String.Format("Erfolgreich gesendete Nachrichten: {0}", m_SuccessfullSentData), m_LOGFileName)
'	'		_ClsLog.WriteTempLogFile(String.Format("Fehlerhaft gesendete Nachrichten: {1}{0}", vbNewLine, m_ErrorSentData), m_LOGFileName)
'	'		_ClsLog.WriteTempLogFile(String.Format("***Ende der Versandvorgang: {0}", Now.ToString), m_LOGFileName)
'	'		If File.Exists(m_LOGFileName) Then
'	'			ReadLogFile(m_LOGFileName)

'	'			xtabMain.SelectedTabPage = xtabEMailData
'	'			xtabEMail.SelectedTabPage = xtabMailingLog

'	'		Else
'	'			SplashScreenManager.CloseForm(False)

'	'			Dim msg As String = "Der Vorgang wurde abgeschlossen. Leider konnte keinen Bericht erstellt werden."
'	'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

'	'		End If

'	'		'PlaySound(0)
'	'		'SplashScreenManager.CloseForm(False)

'	'		btnSendEMail.Enabled = True
'	'		btnSendTestEMail.Enabled = True

'	'	Catch ex As Exception
'	'		SplashScreenManager.CloseForm(False)
'	'		m_Logger.LogError(ex.ToString)

'	'	End Try

'	'	Return

'	'	'Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

'	'	'Task(Of Boolean).Factory.StartNew(Function() PerformSendingEmailCallAsync(),
'	'	'																				CancellationToken.None,
'	'	'																				TaskCreationOptions.None,
'	'	'																				TaskScheduler.Default).ContinueWith(Sub(t) FinishCurrentlistWebserviceCallTask(t), CancellationToken.None,
'	'	'																																						TaskContinuationOptions.None, uiSynchronizationContext)

'	'End Sub

'	'Private Sub FinishCurrentlistWebserviceCallTask(ByVal t As Task(Of Boolean))

'	'	Try
'	'		Select Case t.Status
'	'			Case TaskStatus.RanToCompletion
'	'				' Webservice call was successful.
'	'				_ClsLog.WriteTempLogFile(String.Format("Erfolgreich gesendete Nachrichten: {0}", m_SuccessfullSentData), m_LOGFileName)
'	'				_ClsLog.WriteTempLogFile(String.Format("Fehlerhaft gesendete Nachrichten: {1}{0}", vbNewLine, m_ErrorSentData), m_LOGFileName)
'	'				_ClsLog.WriteTempLogFile(String.Format("***Ende der Versandvorgang: {0}", Now.ToString), m_LOGFileName)
'	'				If File.Exists(m_LOGFileName) Then
'	'					ReadLogFile(m_LOGFileName)
'	'					Me.BringToFront()

'	'					xtabMain.SelectedTabPage = xtabEMailData
'	'					xtabEMail.SelectedTabPage = xtabMailingLog

'	'				Else
'	'					SplashScreenManager.CloseForm(False)

'	'					Dim msg As String = "Der Vorgang wurde abgeschlossen. Leider konnte keinen Bericht erstellt werden."
'	'					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

'	'				End If

'	'				PlaySound(0)

'	'			Case TaskStatus.Faulted
'	'				' Something went wrong -> log error.
'	'				SplashScreenManager.CloseForm(False)
'	'				m_Logger.LogError(t.Exception.ToString())
'	'				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

'	'			Case Else
'	'				' Do nothing
'	'		End Select
'	'	Catch ex As Exception
'	'		m_Logger.LogError(ex.ToString)

'	'	End Try

'	'	SplashScreenManager.CloseForm(False)

'	'	btnSendEMail.Enabled = True
'	'	btnSendTestEMail.Enabled = True
'	'	Me.Visible = True
'	'	Me.BringToFront()

'	'End Sub

'#End Region


'	'Private Sub BackgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	m_SuccessfullSentData = 0
'	'	m_ErrorSentData = 0

'	'	SplashScreenManager.CloseForm(False)
'	'	SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
'	'	SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Ihr Versand wird gestartet") & "...")

'	'	CheckForIllegalCrossThreadCalls = False
'	'	Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

'	'	PerformSendingEmailCallAsync()

'	'	e.Result = True
'	'	If bw.CancellationPending Then e.Cancel = True

'	'End Sub

'	'Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
'	'	Trace.WriteLine(e.ToString)
'	'End Sub

'	'Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'	'	If (e.Error IsNot Nothing) Then
'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Error.Message))
'	'		MessageBox.Show(e.Error.Message)
'	'	Else
'	'		If e.Cancelled = True Then
'	'			m_UtilityUI.ShowErrorDialog("Aktion abgebrochen!")

'	'		Else
'	'			BackgroundWorker1.CancelAsync()
'	'			'        MessageBox.Show(e.Result.ToString())

'	'			_ClsLog.WriteTempLogFile(String.Format("Erfolgreich gesendete Nachrichten: {0}", m_SuccessfullSentData), m_LOGFileName)
'	'			_ClsLog.WriteTempLogFile(String.Format("Fehlerhaft gesendete Nachrichten: {1}{0}", vbNewLine, m_ErrorSentData), m_LOGFileName)
'	'			_ClsLog.WriteTempLogFile(String.Format("***Ende der Versandvorgang: {0}", Now.ToString), m_LOGFileName)

'	'			If e.Result AndAlso File.Exists(m_LOGFileName) Then
'	'				ReadLogFile(m_LOGFileName)

'	'				xtabMain.SelectedTabPage = xtabEMailData
'	'				xtabEMail.SelectedTabPage = xtabMailingLog

'	'			Else
'	'				Dim msg As String = "Der Vorgang wurde abgeschlossen. Leider konnte keinen Bericht erstellt werden."
'	'				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

'	'			End If

'	'			PlaySound(0)
'	'		End If

'	'	End If

'	'End Sub


'	Private Sub ReadLogFile(ByVal strFullFilename As String)
'		Dim objReader As New StreamReader(strFullFilename)
'		Dim sLine As String
'		Dim arrText As New ArrayList()

'		Do
'			sLine = objReader.ReadLine()
'			If Not sLine Is Nothing Then
'				arrText.Add(sLine)
'			End If
'		Loop Until sLine Is Nothing
'		objReader.Close()

'		For Each sLine In arrText
'			txtAllMessage.EditValue &= sLine & vbNewLine
'		Next

'	End Sub

'	'Function ExistDocFile() As Boolean
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	Dim success As Boolean = False
'	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
'	'	Dim sSql As String = "Select Top 1 MANr From OFF_MASelection Where OfNr = @OfNr"

'	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
'	'	Dim param As System.Data.SqlClient.SqlParameter

'	'	Try
'	'		Conn.Open()

'	'		param = cmd.Parameters.AddWithValue("@OFNr", m_CurrentOfferNumber)
'	'		Dim rOFFKDrec As SqlDataReader = cmd.ExecuteReader
'	'		rOFFKDrec.Read()
'	'		success = rOFFKDrec.HasRows

'	'	Catch ex As Exception
'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

'	'	Finally
'	'		cmd.Dispose()

'	'	End Try

'	'	Return success
'	'End Function


'	'Function StoreDataToFs(ByVal lOFNr As Integer) As String()
'	'	Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
'	'	Dim strFullFilename As String() = New String() {""}
'	'	Dim strFiles As String = String.Empty
'	'	Dim BA As Byte()
'	'	Dim sql As String = "Select DocScan, Bezeichnung From OFF_Doc Where "
'	'	sql &= String.Format("OfNr = {0} And MANr = 0", iOfNr)

'	'	Dim i As Integer = 0

'	'	Conn.Open()
'	'	Dim SQLCmd As SqlCommand = New SqlCommand(sql, Conn)
'	'	Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

'	'	'Dim SQLCmd As SqlCommand = New SqlCommand(sql, Conn)
'	'	''Dim SQLCmd_1 As SqlCommand = New SqlCommand(sql, Conn)
'	'	'Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

'	'	Try
'	'		While rOfDoc.Read
'	'			ClsDataDetail.IsAttachedFileInd = True
'	'			Dim strSelectedFile As String = Path.Combine(_ClsProgSetting.GetPersonalFolder, System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString))
'	'			strFiles &= If(strFiles <> String.Empty, ";", "") & strSelectedFile

'	'			Try
'	'				'BA = CType(SQLCmd.ExecuteScalar, Byte())
'	'				BA = CType(rOfDoc("DocScan"), Byte())

'	'				Dim ArraySize As New Integer
'	'				ArraySize = BA.GetUpperBound(0)

'	'				If File.Exists(strSelectedFile) Then File.Delete(strSelectedFile)
'	'				Dim fs As New FileStream(strSelectedFile, FileMode.CreateNew)
'	'				fs.Write(BA, 0, ArraySize + 1)
'	'				fs.Close()
'	'				fs.Dispose()

'	'				i += 1

'	'			Catch ex As Exception
'	'				m_Logger.LogError(ex.ToString)
'	'				_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_1: {0}", ex.Message))

'	'			End Try

'	'		End While
'	'		ReDim strFullFilename(i - 1)
'	'		strFullFilename = strFiles.Split(CChar(";"))

'	'		rOfDoc.Close()

'	'	Catch ex As Exception
'	'		m_Logger.LogError(ex.ToString)
'	'		_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_2: {0}", ex.Message))

'	'	End Try

'	'	Return strFullFilename
'	'End Function

'	Function AllowedToSend() As String
'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		Dim bResult As String = String.Empty
'		Dim msg As String = String.Empty
'		Dim data = SelectedTemplateRecord


'		If m_CurrentOfferNumber.GetValueOrDefault(0) = 0 Then
'			msg = "Error: Bitte wählen Sie eine Offerte aus."
'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, msg))
'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

'			Return msg
'		End If

'		If data Is Nothing OrElse String.IsNullOrWhiteSpace(data.JobNr) Then
'			msg = "Error: Bitte wählen Sie eine Vorlage aus."
'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, msg))
'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

'			Return msg
'		End If

'		If Not m_SendAsStaging Then
'			msg = "Der Vorgang kann mehrere Minuten dauern. Möchten Sie wirklich den Sendvorgang starten?"

'			If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Versand"), MessageBoxDefaultButton.Button1) = True Then
'				bResult = msg
'			Else
'				msg = "user canceled the process."
'				m_Logger.LogWarning(String.Format("{0}.{1}", strMethodeName, msg))

'				Return msg
'			End If

'		End If

'		Return bResult

'	End Function

'	Private Sub OnbtnSendTestEMail_Click(sender As Object, e As EventArgs) Handles btnSendTestEMail.Click
'		SendStagingEMail(Nothing)
'	End Sub

'	Private Sub SendStagingEMail(ByVal customerNumber As Integer?)
'		'Dim strOrgQuery As String = m_GetSearchQuery

'		m_SendAsStaging = True
'		SendMessagesToAssignedlist()

'	End Sub


'	Private Sub OnbtnSendEMail_Click(sender As Object, e As EventArgs) Handles btnSendEMail.Click
'		Dim strMsg As String = String.Empty
'		Dim data = SelectedTemplateRecord

'		m_SendAsStaging = False

'		If m_CurrentOfferNumber.GetValueOrDefault(0) = 0 OrElse (data Is Nothing OrElse String.IsNullOrWhiteSpace(data.JobNr)) Then
'			strMsg = "Eingabe der Vorlage: Ihre Eingabe ist nicht vollständig."
'			m_Logger.LogError(String.Format("{0}", strMsg))

'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg))

'			Return
'		End If
'		SendMessagesToAssignedlist()

'		'SendData2Mail()

'	End Sub

'#Region "Sonstige Funktionen..."

'	'Sub SendData2Mail()
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'	'	Try
'	'		Dim strMessageGuid As String = Guid.NewGuid().ToString
'	'		ClsOfDetails.GetMessageGuid = strMessageGuid

'	'		txtAllMessage.EditValue = String.Empty
'	'		Dim strLogFilename As String = String.Format("{0}{1}.{2}", _ClsProgSetting.GetSpSFiles2DeletePath,
'	'																							ClsOfDetails.GetMessageGuid, "tmp")
'	'		_ClsLog.WriteTempLogFile(String.Format("***Programmstart: {0}", Now.ToString), strLogFilename)
'	'		m_Logger.LogInfo(String.Format("{0}.{1}", strMethodeName, "Gestartet..."))


'	'		BackgroundWorker1.WorkerSupportsCancellation = True
'	'		BackgroundWorker1.RunWorkerAsync()      ' Multithreading starten

'	'	Catch ex As Exception
'	'		MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "CmdMail_0")

'	'	End Try

'	'End Sub


'#End Region

'	Private Sub txtOfNr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueOffer.ButtonClick

'		If m_CurrentOfferNumber.GetValueOrDefault(0) = 0 Then Return
'		If e.Button.Index = 1 Then RunOpenOffForm(m_CurrentOfferNumber)

'	End Sub

'	Sub Ongv_EMailRowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

'		If (e.Clicks = 2) Then

'			Dim column = e.Column
'			Dim dataRow = gvEMail.GetRow(e.RowHandle)
'			If dataRow Is Nothing Then Return
'			Dim viewData = CType(dataRow, CustomerBulkEMailData)

'			Select Case column.Name.ToLower
'				Case "firma1".ToLower
'					If viewData.KDNr > 0 Then OpenCustomerUI(viewData.KDNr)

'				Case "CustomerResponsibleFullname".ToLower
'					If viewData.KDNr > 0 Then OpenCResponsibleUI(viewData.KDNr, viewData.ZHDRecNr)

'				Case Else
'					If viewData.KDNr > 0 Then OpenCustomerUI(viewData.KDNr)

'			End Select


'		End If

'	End Sub

'	Private Sub CboFormat_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CboFormat.QueryPopUp
'		Me.CboFormat.Properties.Items.Clear()

'		'    "PDF", "MHTML", "HTML", "RTF", "XML", "Multi-TIFF", "Text", "XLS"
'		Me.CboFormat.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("PDF"), "PDF"))

'	End Sub

'	Private Sub cboMailTemplate_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboMailTemplate.SelectedIndexChanged
'		SetVisibility4SendButtons()
'	End Sub

'	Sub SetVisibility4SendButtons()

'		If Me.cboMailTemplate.Text.ToLower.Contains("fax") Then
'			Me.btnSendTelefax.Visible = True
'			xtabEMailData.PageVisible = False
'			xtabTelefaxData.PageVisible = True
'			'FillMyDataGrid()

'		Else
'			Me.btnSendTelefax.Visible = False

'			xtabEMailData.PageVisible = True
'			xtabTelefaxData.PageVisible = False

'		End If

'	End Sub

'	'Private Sub OntgsFaxRecipientSelection_Toggled(sender As Object, e As EventArgs) Handles tgsFaxRecipientSelection.Toggled
'	'	FillMyDataGrid()
'	'End Sub

'	Private Sub OntgsMailRecipientSelection_Toggled(sender As Object, e As EventArgs) Handles tgsMailRecipientSelection.Toggled
'		SelDeSelectItems(tgsMailRecipientSelection.EditValue)
'	End Sub

'	Private Sub SelDeSelectItems(ByVal selectItem As Boolean)
'		Dim data As BindingList(Of CustomerBulkEMailData) = grdEMail.DataSource

'		If Not data Is Nothing Then
'			For Each item In data
'				item.Selected = selectItem
'			Next
'		End If

'		gvEMail.RefreshData()

'	End Sub

'	Sub GetQery4ShowInMailGrid()
'		Dim strBeginTrySql As String = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH"
'		Dim strTestSql As String = String.Format("{0} SELECT * {1} FROM _Kundenliste_{2} ",
'															 strBeginTrySql,
'															 "Into #KD_Mailing",
'															 _ClsProgSetting.GetLogedUSNr)
'		strTestSql &= "SELECT KDNr, ZHDRecNr, Firma1, (Case KD_Mail_Mailing "
'		strTestSql &= "When 1 Then '' "
'		strTestSql &= "Else KDeMail "
'		strTestSql &= "End ) As "
'		strTestSql &= "KDeMail, "
'		strTestSql &= "KD_Mail_Mailing, "

'		strTestSql &= "Vorname, Nachname,	"
'		strTestSql &= "(Case ZHD_Mail_Mailing "
'		strTestSql &= "When 1 Then '' "
'		strTestSql &= "Else ZHDeMail "
'		strTestSql &= "End ) As 	"
'		strTestSql &= "ZHDeMail, "
'		strTestSql &= "ZHD_Mail_Mailing "

'		strTestSql &= "From #KD_Mailing Where KDeMail + ZHDeMail <> '' "
'		strTestSql &= "Group By KDNr, ZHDRecNr, Firma1, KDeMail, KD_Mail_Mailing, ZHDeMail, ZHD_Mail_Mailing, Vorname, Nachname "
'		strTestSql &= "Order By Firma1, Vorname, Nachname"

'		Me.Sql2Open4Grid = strTestSql

'	End Sub

'	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

'		Dim m_md As New SPProgUtility.Mandanten.Mandant
'		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
'		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
'		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

'		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
'		Dim translate = clsTransalation.GetTranslationInObject

'		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

'	End Function


'#Region "Helper methodes"

'	Sub OpenCustomerUI(ByVal _iKDNr As Integer)

'		Try
'			Dim hub = MessageService.Instance.Hub
'			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, _iKDNr)
'			hub.Publish(openMng)

'		Catch ex As Exception
'			m_Logger.LogError(ex.ToString)

'		End Try


'	End Sub


'	Sub OpenCResponsibleUI(ByVal _iKDNr As Integer, ByVal _iKDZHDNr As Integer)

'		Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr))

'		If (responsiblePersonsFrom.LoadResponsiblePersonData(_iKDNr, _iKDZHDNr)) Then
'			responsiblePersonsFrom.Show()
'		End If

'	End Sub

'	Private Function LoadStagingEMailAddress() As String
'		Dim result = m_InitializationData.UserData.UserMDeMail
'		If String.IsNullOrWhiteSpace(result) Then result = m_InitializationData.UserData.UsereMail

'		If String.IsNullOrWhiteSpace(result) Then
'			m_Logger.LogError(String.Format("GeteMailFrom: email address is empty."))
'			m_UtilityUI.ShowErrorDialog("Der Absender wurde nicht gefunden! Bitte definieren Sie eine EMail-Adresse in der Benutzerverwaltung.")
'		End If

'		Return result

'	End Function

'	Private Sub OnlueOffer_EditValueChanged(sender As Object, e As EventArgs)

'		m_CurrentOfferNumber = CType(lueOffer.EditValue, Integer)

'	End Sub


'#End Region


'#Region "Helper classes"

'	Private Class RecipientFieldData
'		Public Property RecordValue As RecordValueEnum
'		Public Property RecordLabel As String
'	End Class

'	Private Enum RecordValueEnum
'		NOTDEFINED
'		CUSTOMER
'		RESPONSIBLEPERSON
'		BOTH
'	End Enum

'#End Region



'End Class




