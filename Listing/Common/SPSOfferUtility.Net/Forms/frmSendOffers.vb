
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.LOGWriter
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports System.IO

Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen

Imports System.Data.SqlClient
Imports System.Threading

Imports DevExpress.XtraGrid.Columns

Imports SPSOfferUtility_Net.ClsOfDetails
'Imports SPSSendMail
Imports SPProgUtility.ProgPath

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.KD.CPersonMng.UI
Imports SP.DatabaseAccess.Propose
Imports System.Threading.Tasks
Imports DevExpress.XtraBars.Docking2010

Public Class frmOfferSelect



#Region "Private Fields"

	Private Shared m_Logger As ILogger = New Logger()
	Private Shared m_LogWriter As LOGWriter = New LOGWriter()

	Private MyDataReader As SqlDataReader
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_TableSettingDatabaseAccess As ITablesDatabaseAccess
	Private m_ProposeDatabaseAccess As IProposeDatabaseAccess


	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath
	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath


	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_connectionString As String
	Private m_CustomerBulkEMailData As IEnumerable(Of CustomerBulkEMailData)


	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	Private m_ExportedFilename As String
	Private m_SuccessfullSentData As Integer
	Private m_ErrorSentData As Integer


	Private m_EMailPriority As Boolean
	Private m_SendAsStaging As Boolean
	Private m_LOGFileName As String
	Private m_MessageGuid As String

	Private m_RecipientFieldData As List(Of RecipientFieldData)
	Private m_AssignedRecipientFieldData As RecipientFieldData
	Private m_AssignedTemplateData As DocumentData

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_CurrentOfferNumber As Integer?
	Private m_EmailTemplateFilename As String

	Private m_OfferSend As OfferMessages

#End Region


#Region "public properties"

	Public Property m_GetSearchQuery As String

#End Region


#Region "private properties"

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property Sql2Open4Grid As String

	Private ReadOnly Property SelectedTemplateRecord As DocumentData
		Get
			Dim gvData = TryCast(grdTemplates.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvData.GetRow(selectedRows(0)), DocumentData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property LoadTplFilename() As String
		Get
			Dim progUtility = New SPProgUtility.MainUtilities.Utilities
			Dim fileName As String = m_InitializationData.MDData.MandantCurrentXMLFileName
			Dim strQuery As String = String.Empty
			strQuery = String.Format("//Templates/MassenOffer-eMail-Template")
			Dim strMyTplValue As String = _ClsProgSetting.GetXMLValueByQuery(fileName, strQuery, "")

			Try
				If Not String.IsNullOrWhiteSpace(strMyTplValue) Then
					strMyTplValue = Path.Combine(m_InitializationData.MDData.MDTemplatePath, strMyTplValue)
				End If

				If Not File.Exists(strMyTplValue) Then
					m_Logger.LogDebug(String.Format("Template {0} wurde in {1} nicht gefunden.", strMyTplValue, fileName))

					Throw New FileNotFoundException(strQuery)
				End If
				Return strMyTplValue

			Catch ex As FileNotFoundException
				Dim strMsg As String = "Ihre E-Mail-Vorlage wurde nicht gefunden. Das Programm wird beendet.{0}"
				strMsg &= "Bitte versuchen Sie die Vorlage in der Dokumentenverwaltung -> Vorlagen:Massenofferte zu definieren."
				strMsg = String.Format(strMsg, vbNewLine)
				m_UtilityUI.ShowErrorDialog(strMsg)

				m_Logger.LogError(String.Format("{0}.", ex.ToString))
				_ClsLog.WriteTempLogFile(String.Format("***Fehler: EMail-Vorlage wurde nicht gefunden. {0}", ex.ToString), m_LOGFileName)

				Return String.Empty
			End Try

		End Get
	End Property

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting

		m_mandant = New Mandant
		m_path = New ClsProgPath
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_TableSettingDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_OfferSend = New OfferMessages(m_InitializationData)

		m_SuppressUIEvents = True

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_SuppressUIEvents = False

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try


		Reset()
		TranslateControls()

		AddHandler gvTemplates.RowCellClick, AddressOf Ongv_TemplateRowCellClick
		AddHandler gvEMail.RowCellClick, AddressOf Ongv_EMailRowCellClick
		AddHandler lueOffer.EditValueChanged, AddressOf OnlueOffer_EditValueChanged
		AddHandler rgRecipients.EditValueChanged, AddressOf rgRecipients_EditValueChanged

	End Sub


#End Region


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		result = result AndAlso LoadOfferDropDownData()

		' "Vorlage für Mail-Versand", "Vorlage für Fax-Versand", "Sonstige Vorlagen"
		cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Mail-Versand"), "Mail"))
		'cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Fax-Versand"), "Fax"))
		cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Sonstige Vorlagen"), "Sonst"))
		cboMailTemplate.SelectedIndex = 0

		CboFormat.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("PDF"), "PDF"))
		CboFormat.SelectedIndex = 0

		m_EmailTemplateFilename = LoadTplFilename
		cboEmailTemplateFilename.Properties.Items.Add(m_EmailTemplateFilename)
		cboEmailTemplateFilename.EditValue = m_EmailTemplateFilename

		result = result AndAlso LoadDocTemplateDropDownData("Mail")
		result = result AndAlso LoadRecipientFieldData()
		result = result AndAlso LoadEMailData()

		Return result
	End Function

#End Region


#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.cmdClose.Text = m_Translate.GetSafeTranslationValue(Me.cmdClose.Text)

		grpOffer.Text = m_Translate.GetSafeTranslationValue(grpOffer.Text)
		Me.lblOfNr.Text = m_Translate.GetSafeTranslationValue(Me.lblOfNr.Text)

		grpAttachmentTemplate.Text = m_Translate.GetSafeTranslationValue(grpAttachmentTemplate.Text)
		Me.lblVersandmethode.Text = m_Translate.GetSafeTranslationValue(Me.lblVersandmethode.Text)
		Me.lblVorhandeneOfferte.Text = m_Translate.GetSafeTranslationValue(Me.lblVorhandeneOfferte.Text)
		Me.lblExportformat.Text = m_Translate.GetSafeTranslationValue(Me.lblExportformat.Text)

		xtabEMailData.Text = m_Translate.GetSafeTranslationValue(xtabEMailData.Text)
		xtabTelefaxData.Text = m_Translate.GetSafeTranslationValue(xtabTelefaxData.Text)
		xtabMainMailDetailData.Text = m_Translate.GetSafeTranslationValue(xtabMainMailDetailData.Text)
		xtabMailingLog.Text = m_Translate.GetSafeTranslationValue(xtabMailingLog.Text)

		CmdExport.Text = m_Translate.GetSafeTranslationValue(Me.CmdExport.Text)
		btnSendTelefax.Text = m_Translate.GetSafeTranslationValue(Me.btnSendTelefax.Text)

		grpMailSetting.Text = m_Translate.GetSafeTranslationValue(grpMailSetting.Text)
		Me.CmdPrint.Text = m_Translate.GetSafeTranslationValue(Me.CmdPrint.Text)
		lblPrioritaet.Text = m_Translate.GetSafeTranslationValue(lblPrioritaet.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)

	End Sub


	Private Sub Reset()

		m_CurrentOfferNumber = Nothing
		lueOffer.EditValue = Nothing

		rtfSendLog.Unit = DevExpress.Office.DocumentUnit.Centimeter
		rtfSendLog.Document.Sections(0).Page.PaperKind = Printing.PaperKind.A4
		rtfSendLog.Text = String.Empty
		rtfSendLog.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden

		rtfSendLog.Font = New Drawing.Font("Calibri", 10, FontStyle.Regular)
		rtfSendLog.Document.DefaultCharacterProperties.FontName = "Calibri"
		rtfSendLog.Document.DefaultCharacterProperties.FontSize = 10
		rtfSendLog.ReadOnly = True

		rtfSendLog.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple
		rtfSendLog.Views.SimpleView.Padding = New DevExpress.Portable.PortablePadding(0) ' New DevExpress.Portable.PortablePadding(0)
		rtfSendLog.Document.Sections(0).Margins.Left = 0
		rtfSendLog.Document.Sections(0).Margins.Right = 0
		rtfSendLog.Document.Sections(0).Margins.Top = 0
		rtfSendLog.Document.Sections(0).Margins.Bottom = 0

		ResetTemplateGridData()
		ResetEMailGridData()
		ResetOfferDropDown()

	End Sub

	Private Sub ResetTemplateGridData()

		gvTemplates.OptionsView.ShowIndicator = False
		gvTemplates.OptionsView.ShowAutoFilterRow = True
		gvTemplates.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvTemplates.OptionsView.ShowFooter = False
		gvTemplates.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvTemplates.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.OptionsColumn.AllowEdit = False
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Job-Nr.")
		columnMANr.Name = "JobNr"
		columnMANr.FieldName = "JobNr"
		columnMANr.Visible = False
		gvTemplates.Columns.Add(columnMANr)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.OptionsColumn.AllowEdit = False
		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columncustomername.Name = "Bezeichnung"
		columncustomername.FieldName = "Bezeichnung"
		columncustomername.BestFit()
		columncustomername.Visible = True
		gvTemplates.Columns.Add(columncustomername)


		grdTemplates.DataSource = Nothing


	End Sub

	Private Sub ResetOfferDropDown()

		lueOffer.Properties.DisplayMember = "OfferLabel"
		lueOffer.Properties.ValueMember = "OfferNumber"

		gvOffer.OptionsView.ShowIndicator = False
		gvOffer.OptionsView.ShowColumnHeaders = True
		gvOffer.OptionsView.ShowFooter = False

		gvOffer.OptionsView.ShowAutoFilterRow = True
		gvOffer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvOffer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "OfferNumber"
		columnCustomerNumber.FieldName = "OfferNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvOffer.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnCompany1.Name = "OfferLabel"
		columnCompany1.FieldName = "OfferLabel"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvOffer.Columns.Add(columnCompany1)

		lueOffer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueOffer.Properties.NullText = String.Empty
		lueOffer.EditValue = Nothing

	End Sub

	Private Sub ResetEMailGridData()

		gvEMail.OptionsView.ShowIndicator = False
		gvEMail.OptionsView.ShowAutoFilterRow = True
		gvEMail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEMail.OptionsView.ShowFooter = False
		gvEMail.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvEMail.Columns.Clear()

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.OptionsColumn.AllowEdit = False
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnMANr.Name = "KDNr"
		columnMANr.FieldName = "KDNr"
		columnMANr.Visible = False
		gvEMail.Columns.Add(columnMANr)

		Dim columnzNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnzNr.OptionsColumn.AllowEdit = False
		columnzNr.Caption = m_Translate.GetSafeTranslationValue("ZHD-Nr.")
		columnzNr.Name = "ZHDRecNr"
		columnzNr.FieldName = "ZHDRecNr"
		columnzNr.Visible = False
		gvEMail.Columns.Add(columnzNr)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.OptionsColumn.AllowEdit = False
		columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columncustomername.Name = "Firma1"
		columncustomername.FieldName = "Firma1"
		columncustomername.BestFit()
		columncustomername.Visible = True
		gvEMail.Columns.Add(columncustomername)

		Dim columncustomername2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername2.OptionsColumn.AllowEdit = False
		columncustomername2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername2.Caption = m_Translate.GetSafeTranslationValue("E-Mail")
		columncustomername2.Name = "ReceiverEMail"
		columncustomername2.FieldName = "ReceiverEMail"
		columncustomername2.BestFit()
		columncustomername2.Visible = True
		gvEMail.Columns.Add(columncustomername2)

		Dim columncustomername3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername3.OptionsColumn.AllowEdit = False
		columncustomername3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomername3.Caption = m_Translate.GetSafeTranslationValue("Mailing")
		columncustomername3.Name = "NoMailing"
		columncustomername3.FieldName = "NoMailing"
		columncustomername3.BestFit()
		columncustomername3.Visible = False
		gvEMail.Columns.Add(columncustomername3)

		Dim columncustomerstreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomerstreet.OptionsColumn.AllowEdit = False
		columncustomerstreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomerstreet.Caption = m_Translate.GetSafeTranslationValue("Zusändige Person")
		columncustomerstreet.Name = "CustomerResponsibleFullname"
		columncustomerstreet.FieldName = "CustomerResponsibleFullname"
		columncustomerstreet.BestFit()
		columncustomerstreet.Visible = True
		gvEMail.Columns.Add(columncustomerstreet)

		'Dim columnEcustomeradress As New DevExpress.XtraGrid.Columns.GridColumn()
		'columnEcustomeradress.OptionsColumn.AllowEdit = False
		'columnEcustomeradress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		'columnEcustomeradress.Caption = m_Translate.GetSafeTranslationValue("ZHD.-E-Mail")
		'columnEcustomeradress.Name = "ZHDeMail"
		'columnEcustomeradress.FieldName = "ZHDeMail"
		'columnEcustomeradress.BestFit()
		'columnEcustomeradress.Visible = True
		'gvEMail.Columns.Add(columnEcustomeradress)

		'Dim columncustomertelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		'columncustomertelefon.OptionsColumn.AllowEdit = False
		'columncustomertelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		'columncustomertelefon.Caption = m_Translate.GetSafeTranslationValue("ZHD.-Mailing")
		'columncustomertelefon.Name = "ZHD_Mail_Mailing"
		'columncustomertelefon.FieldName = "ZHD_Mail_Mailing"
		'columncustomertelefon.BestFit()
		'columncustomertelefon.Visible = True
		'gvEMail.Columns.Add(columncustomertelefon)

		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSelectedRec.Caption = m_Translate.GetSafeTranslationValue("")
		columnSelectedRec.Name = "Selected"
		columnSelectedRec.FieldName = "Selected"
		columnSelectedRec.BestFit()
		columnSelectedRec.Visible = True
		gvEMail.Columns.Add(columnSelectedRec)


		grdEMail.DataSource = Nothing


	End Sub



	'''' <summary>
	'''' Resets the customer drop down.
	'''' </summary>
	'Private Sub ResetCustomerDropDown()

	'	lueCustomer.Properties.DisplayMember = "Company1"
	'	lueCustomer.Properties.ValueMember = "CustomerNumber"

	'	gvCustomer.OptionsView.ShowIndicator = False
	'	gvCustomer.OptionsView.ShowColumnHeaders = True
	'	gvCustomer.OptionsView.ShowFooter = False

	'	gvCustomer.OptionsView.ShowAutoFilterRow = True
	'	gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
	'	gvCustomer.Columns.Clear()

	'	Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
	'	columnCustomerNumber.Name = "CustomerNumber"
	'	columnCustomerNumber.FieldName = "CustomerNumber"
	'	columnCustomerNumber.Visible = True
	'	columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'	gvCustomer.Columns.Add(columnCustomerNumber)

	'	Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
	'	columnCompany1.Name = "Company1"
	'	columnCompany1.FieldName = "Company1"
	'	columnCompany1.Visible = True
	'	columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'	gvCustomer.Columns.Add(columnCompany1)

	'	Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
	'	columnStreet.Name = "Street"
	'	columnStreet.FieldName = "Street"
	'	columnStreet.Visible = True
	'	columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'	gvCustomer.Columns.Add(columnStreet)

	'	Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
	'	columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
	'	columnPostcodeAndLocation.Name = "PostcodeAndLocation"
	'	columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
	'	columnPostcodeAndLocation.Visible = True
	'	columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'	gvCustomer.Columns.Add(columnPostcodeAndLocation)

	'	lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
	'	lueCustomer.Properties.NullText = String.Empty
	'	lueCustomer.EditValue = Nothing

	'End Sub

#End Region


	Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Close()
			Me.Dispose()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub frmOfferSelect_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

		Try
			My.Settings.DeliveryPriority = tgsEMailPriority.EditValue

			My.Settings.Save()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub frmOfferSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		tgsEMailPriority.EditValue = My.Settings.DeliveryPriority
	End Sub


	Private Function LoadDocTemplateDropDownData(ByVal bTemplateArt As String) As Boolean

		grdTemplates.DataSource = Nothing
		Dim offerData = m_TableSettingDatabaseAccess.LoadTemplateDataForSendBulkCustomer(bTemplateArt)

		If offerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlage Daten konnen nicht geladen werden."))
			Return False
		End If

		Dim templateGridData = (From person In offerData
								Select New DocumentData With {
												  .JobNr = person.JobNr,
												  .Bezeichnung = person.Bezeichnung
												  }).ToList()

		Dim listDataSource As BindingList(Of DocumentData) = New BindingList(Of DocumentData)

		For Each p In templateGridData
			listDataSource.Add(p)
		Next
		grdTemplates.DataSource = listDataSource


		Return Not listDataSource Is Nothing

	End Function

	Private Function LoadRecipientFieldData() As Boolean
		Dim result As Boolean = True

		m_RecipientFieldData = New List(Of RecipientFieldData)
		m_RecipientFieldData.Add(New RecipientFieldData With {.RecordValue = RecordValueEnum.CUSTOMER, .RecordLabel = m_Translate.GetSafeTranslationValue("(KD) NUR Kunden")})
		m_RecipientFieldData.Add(New RecipientFieldData With {.RecordValue = RecordValueEnum.RESPONSIBLEPERSON, .RecordLabel = m_Translate.GetSafeTranslationValue("(ZHD) NUR Zuständige Personen")})
		m_RecipientFieldData.Add(New RecipientFieldData With {.RecordValue = RecordValueEnum.BOTH, .RecordLabel = m_Translate.GetSafeTranslationValue("(BOTH) Kunden und Zuständige Personen")})

		rgRecipients.Properties.Items.Clear()
		For Each itm In m_RecipientFieldData
			rgRecipients.Properties.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem With {.Description = itm.RecordLabel, .Value = itm.RecordValue})
		Next

		Dim lastRecipient = CType(3, RecordValueEnum)
		rgRecipients.EditValue = lastRecipient


		Return result
	End Function

	Private Function LoadOfferDropDownData() As Boolean

		Dim offerData = m_ListingDatabaseAccess.LoadOfferDataToSendEmail(m_InitializationData.UserData.UserFiliale)
		If offerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Offerte Daten konnen nicht geladen werden."))
			Return False
		End If

		lueOffer.EditValue = Nothing
		lueOffer.Properties.DataSource = offerData


		Return Not offerData Is Nothing

	End Function

	Private Function LoadEMailData() As Boolean
		Dim modulName As String = ""

		If rgRecipients.EditValue = 1 Then modulName = "KD"
		If rgRecipients.EditValue = 2 Then modulName = "ZHD"

		m_CustomerBulkEMailData = m_ListingDatabaseAccess.LoadCustomerDataToSendBulkEmail(m_InitializationData.UserData.UserNr, modulName)
		If m_CustomerBulkEMailData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("E-Mail Versanddaten konnten nicht geladen werden."))

			Return False
		End If

		Dim gridData = (From person In m_CustomerBulkEMailData
						Select New CustomerBulkEMailData With {
												  .KDNr = person.KDNr,
												  .ZHDRecNr = person.ZHDRecNr,
												  .Firma1 = person.Firma1,
												  .ReceiverEMail = person.ReceiverEMail,
												  .NoMailing = person.NoMailing,
												  .Anrede = person.Anrede,
												  .Nachname = person.Nachname,
												  .Vorname = person.Vorname,
												  .AnredeForm = person.AnredeForm,
												  .Strasse = person.Strasse,
												  .Postfach = person.Postfach,
												  .PLZ = person.PLZ,
												  .Ort = person.Ort,
												  .Land = person.Land,
												  .Telefon = person.Telefon,
												  .Telefax = person.Telefax,
												  .Telefax_Mailing = person.Telefax_Mailing,
												  .Abteilung = person.Abteilung,
												  .ModulName = person.ModulName,
												  .Selected = True
												  }).ToList()

		Dim listDataSource As BindingList(Of CustomerBulkEMailData) = New BindingList(Of CustomerBulkEMailData)

		For Each p In gridData
			listDataSource.Add(p)
		Next
		grdEMail.DataSource = listDataSource

		bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Return Not listDataSource Is Nothing
	End Function

	Sub Ongv_TemplateRowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
		m_AssignedTemplateData = SelectedTemplateRecord
	End Sub

	Private Sub cboMailTemplate_EditValueChanged(sender As Object, e As System.EventArgs) Handles cboMailTemplate.EditValueChanged
		Dim cv As ComboValue = DirectCast(Me.cboMailTemplate.SelectedItem, ComboValue)

		LoadDocTemplateDropDownData(cv.ComboValue)

	End Sub

	Private Sub cboMailTemplate_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboMailTemplate.QueryPopUp
		Me.cboMailTemplate.Properties.Items.Clear()

		Me.cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Mail-Versand"), "Mail"))
		'Me.cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Vorlage für Fax-Versand"), "Fax"))
		Me.cboMailTemplate.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("Sonstige Vorlagen"), "Sonstige"))

	End Sub

	Private Function GetSelectedRecipientFieldData() As RecipientFieldData
		Dim result As New RecipientFieldData

		m_AssignedRecipientFieldData = New RecipientFieldData

		For Each itm In rgRecipients.Properties.Items

			If itm.value = rgRecipients.EditValue Then
				m_AssignedRecipientFieldData.RecordLabel = itm.ToString
				m_AssignedRecipientFieldData.RecordValue = itm.value

				Exit For
			End If

		Next
		result = m_AssignedRecipientFieldData

		Return result
	End Function

	Sub StartPrinting(ByVal bForExport As Boolean)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True
		Dim tplData = SelectedTemplateRecord
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Try
			If tplData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Vorlage ausgewählt."))

				Return
			End If
			If grdEMail.DataSource Is Nothing Then Return
			Dim CustomerNumber As Integer = 0
			Dim CResponsibleNumber As Integer = 0

			Dim data = CType(grdEMail.DataSource, IEnumerable(Of CustomerBulkEMailData))
			data = (From r In data Where r.Selected = True).ToList()
			If data Is Nothing OrElse data.Count = 0 Then
				data = CType(grdEMail.DataSource, IEnumerable(Of CustomerBulkEMailData))
				If data Is Nothing Then Return
			End If
			GetSelectedRecipientFieldData()

			CustomerNumber = data(0).KDNr
			CResponsibleNumber = data(0).ZHDRecNr

			Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = m_InitializationData, .offerNumber = m_CurrentOfferNumber.GetValueOrDefault(0),
				.JobNr2Print = tplData.JobNr, .ShowAsExport = bForExport, .ShowAsDesgin = ShowDesign}

			_setting.customerNumber = CustomerNumber
			_setting.cresponsibleNumber = CResponsibleNumber

			Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
			Dim strOfferblattFileName As String = printTemplate.PrintOfferTemplate()

			If bForExport Then Process.Start(strOfferblattFileName)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


		Return

	End Sub

	Private Sub CmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdPrint.Click
		StartPrinting(False)
	End Sub

	Private Sub CmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdExport.Click
		StartPrinting(True)
	End Sub

	'Private Sub CmdMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	SendData2Mail()
	'End Sub


#Region "Multitreading..."

	Private Sub SendMessagesToAssignedlist()

		If Not AllowedToSend() Then Return
		btnSendEMail.Enabled = False
		btnSendTestEMail.Enabled = False
		rtfSendLog.HtmlText = String.Empty
		m_EMailPriority = tgsEMailPriority.EditValue

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		Task(Of Boolean).Factory.StartNew(Function() PerformSendingEmailCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishCurrentlistWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	Private Function PerformSendingEmailCallAsync() As Boolean

		Dim data = CType(grdEMail.DataSource, IEnumerable(Of CustomerBulkEMailData))
		data = (From r In data Where r.Selected = True).ToList()
		If data Is Nothing OrElse data.Count = 0 Then
			If Not m_SendAsStaging Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Datensätze ausgewählt. Zum Versand müssen Sie Daten auswählen."))

				Return False
			End If

			data = CType(grdEMail.DataSource, IEnumerable(Of CustomerBulkEMailData))
			If data Is Nothing Then Return False
		End If

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Ihr Versand wird gestartet") & "...")

		m_Logger.LogInfo(String.Format("mailing ist started..."))

		If Not Directory.Exists(m_InitializationData.UserData.spAllowedPath) Then Directory.CreateDirectory(m_InitializationData.UserData.spAllowedPath)
		m_LOGFileName = Path.Combine(m_InitializationData.UserData.spAllowedPath, Path.GetRandomFileName)
		m_LogWriter.CurrentLogFileName = m_LOGFileName

		m_OfferSend.CommonLOGFilename = m_LOGFileName
		m_OfferSend.CurrentofferNumber = m_CurrentOfferNumber
		m_OfferSend.MailTemplateFilename = m_EmailTemplateFilename
		m_OfferSend.MailReceiverData = data
		m_OfferSend.SelectedRecipientFieldData = GetSelectedRecipientFieldData()
		m_OfferSend.SelectedTemplateData = SelectedTemplateRecord
		m_OfferSend.SendMessagesAsStaging = m_SendAsStaging
		m_OfferSend.DeliverEMailWithHighPriority = m_EMailPriority

		Dim result = m_OfferSend.SendAssignedOfferToSelectedCustomers()


		Return result
	End Function

	Private Sub FinishCurrentlistWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try
			Select Case t.Status
				Case TaskStatus.RanToCompletion
					m_LogWriter.WriteTempLogFile(String.Format("<b>Ende der Versandvorgang</b>"), m_LOGFileName)
					If File.Exists(m_LOGFileName) Then
						rtfSendLog.LoadDocument(m_LOGFileName, DevExpress.XtraRichEdit.DocumentFormat.Html)

						Me.BringToFront()

						xtabMain.SelectedTabPage = xtabEMailData
						xtabEMail.SelectedTabPage = xtabMailingLog

					Else
						SplashScreenManager.CloseForm(False)

						Dim msg As String = "Der Vorgang wurde abgeschlossen. Leider konnte keinen Bericht erstellt werden."
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

					End If

					PlaySound(0)

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					SplashScreenManager.CloseForm(False)
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Case Else
					' Do nothing
			End Select
		Catch ex As Exception
			SplashScreenManager.CloseForm(False)
			m_Logger.LogError(ex.ToString)

		End Try

		SplashScreenManager.CloseForm(False)

		btnSendEMail.Enabled = True
		btnSendTestEMail.Enabled = True
		Me.BringToFront()

	End Sub

#End Region


	'Private Sub BackgroundWorker2_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	m_SuccessfullSentData = 0
	'	m_ErrorSentData = 0

	'	SplashScreenManager.CloseForm(False)
	'	SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
	'	SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Ihr Versand wird gestartet") & "...")

	'	CheckForIllegalCrossThreadCalls = False
	'	Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

	'	PerformSendingEmailCallAsync()

	'	e.Result = True
	'	If bw.CancellationPending Then e.Cancel = True

	'End Sub

	'Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
	'	Trace.WriteLine(e.ToString)
	'End Sub

	'Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	If (e.Error IsNot Nothing) Then
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Error.Message))
	'		MessageBox.Show(e.Error.Message)
	'	Else
	'		If e.Cancelled = True Then
	'			m_UtilityUI.ShowErrorDialog("Aktion abgebrochen!")

	'		Else
	'			BackgroundWorker1.CancelAsync()
	'			'        MessageBox.Show(e.Result.ToString())

	'			_ClsLog.WriteTempLogFile(String.Format("Erfolgreich gesendete Nachrichten: {0}", m_SuccessfullSentData), m_LOGFileName)
	'			_ClsLog.WriteTempLogFile(String.Format("Fehlerhaft gesendete Nachrichten: {1}{0}", vbNewLine, m_ErrorSentData), m_LOGFileName)
	'			_ClsLog.WriteTempLogFile(String.Format("***Ende der Versandvorgang: {0}", Now.ToString), m_LOGFileName)

	'			If e.Result AndAlso File.Exists(m_LOGFileName) Then
	'				ReadLogFile(m_LOGFileName)

	'				xtabMain.SelectedTabPage = xtabEMailData
	'				xtabEMail.SelectedTabPage = xtabMailingLog

	'			Else
	'				Dim msg As String = "Der Vorgang wurde abgeschlossen. Leider konnte keinen Bericht erstellt werden."
	'				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

	'			End If

	'			PlaySound(0)
	'		End If

	'	End If

	'End Sub


	'Private Sub ReadLogFile(ByVal strFullFilename As String)
	'	Dim objReader As New StreamReader(strFullFilename)
	'	Dim sLine As String
	'	Dim arrText As New ArrayList()

	'	Do
	'		sLine = objReader.ReadLine()
	'		If Not sLine Is Nothing Then
	'			arrText.Add(sLine)
	'		End If
	'	Loop Until sLine Is Nothing
	'	objReader.Close()

	'	For Each sLine In arrText
	'		txtAllMessage.EditValue &= sLine & vbNewLine
	'	Next

	'End Sub

	'Function ExistDocFile() As Boolean
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim success As Boolean = False
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	Dim sSql As String = "Select Top 1 MANr From OFF_MASelection Where OfNr = @OfNr"

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		Conn.Open()

	'		param = cmd.Parameters.AddWithValue("@OFNr", m_CurrentOfferNumber)
	'		Dim rOFFKDrec As SqlDataReader = cmd.ExecuteReader
	'		rOFFKDrec.Read()
	'		success = rOFFKDrec.HasRows

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	Finally
	'		cmd.Dispose()

	'	End Try

	'	Return success
	'End Function


	'Function StoreDataToFs(ByVal lOFNr As Integer) As String()
	'	Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
	'	Dim strFullFilename As String() = New String() {""}
	'	Dim strFiles As String = String.Empty
	'	Dim BA As Byte()
	'	Dim sql As String = "Select DocScan, Bezeichnung From OFF_Doc Where "
	'	sql &= String.Format("OfNr = {0} And MANr = 0", iOfNr)

	'	Dim i As Integer = 0

	'	Conn.Open()
	'	Dim SQLCmd As SqlCommand = New SqlCommand(sql, Conn)
	'	Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

	'	'Dim SQLCmd As SqlCommand = New SqlCommand(sql, Conn)
	'	''Dim SQLCmd_1 As SqlCommand = New SqlCommand(sql, Conn)
	'	'Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

	'	Try
	'		While rOfDoc.Read
	'			ClsDataDetail.IsAttachedFileInd = True
	'			Dim strSelectedFile As String = Path.Combine(_ClsProgSetting.GetPersonalFolder, System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString))
	'			strFiles &= If(strFiles <> String.Empty, ";", "") & strSelectedFile

	'			Try
	'				'BA = CType(SQLCmd.ExecuteScalar, Byte())
	'				BA = CType(rOfDoc("DocScan"), Byte())

	'				Dim ArraySize As New Integer
	'				ArraySize = BA.GetUpperBound(0)

	'				If File.Exists(strSelectedFile) Then File.Delete(strSelectedFile)
	'				Dim fs As New FileStream(strSelectedFile, FileMode.CreateNew)
	'				fs.Write(BA, 0, ArraySize + 1)
	'				fs.Close()
	'				fs.Dispose()

	'				i += 1

	'			Catch ex As Exception
	'				m_Logger.LogError(ex.ToString)
	'				_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_1: {0}", ex.Message))

	'			End Try

	'		End While
	'		ReDim strFullFilename(i - 1)
	'		strFullFilename = strFiles.Split(CChar(";"))

	'		rOfDoc.Close()

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'		_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_2: {0}", ex.Message))

	'	End Try

	'	Return strFullFilename
	'End Function

	Function AllowedToSend() As Boolean
		Dim result As Boolean = True
		Dim msg As String = String.Empty
		Dim data = SelectedTemplateRecord

		If m_CurrentOfferNumber.GetValueOrDefault(0) = 0 Then
			msg = "Bitte wählen Sie eine Offerte aus."
			m_Logger.LogError(String.Format("{0}", msg))
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			Return False
		End If

		If data Is Nothing OrElse String.IsNullOrWhiteSpace(data.JobNr) Then
			msg = "Bitte wählen Sie eine Vorlage aus."
			m_Logger.LogError(String.Format("{0}", msg))
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			Return False
		End If

		If Not m_SendAsStaging Then
			msg = "Der Vorgang kann mehrere Minuten dauern. Möchten Sie wirklich den Sendvorgang für Versand von {0} EMails starten?"
			If m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue(msg), m_CustomerBulkEMailData.Count),
										   m_Translate.GetSafeTranslationValue("Versand"), MessageBoxDefaultButton.Button1) Then
				result = True

			Else
				msg = "user canceled the process."
				m_Logger.LogWarning(String.Format("{0}", msg))

				Return False
			End If

		End If

		Return result

	End Function

	Private Sub OnbtnSendTestEMail_Click(sender As Object, e As EventArgs) Handles btnSendTestEMail.Click
		SendStagingEMail(Nothing)
	End Sub

	Private Sub SendStagingEMail(ByVal customerNumber As Integer?)

		m_SendAsStaging = True

		SendMessagesToAssignedlist()

	End Sub


	Private Sub OnbtnSendEMail_Click(sender As Object, e As EventArgs) Handles btnSendEMail.Click
		Dim strMsg As String = String.Empty
		Dim data = SelectedTemplateRecord

		m_SendAsStaging = False

		If m_CurrentOfferNumber.GetValueOrDefault(0) = 0 OrElse (data Is Nothing OrElse String.IsNullOrWhiteSpace(data.JobNr)) Then
			strMsg = "Eingabe der Vorlage: Ihre Eingabe ist nicht vollständig."
			m_Logger.LogError(String.Format("{0}", strMsg))

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg))

			Return
		End If
		SendMessagesToAssignedlist()

	End Sub

#Region "Sonstige Funktionen..."

	'Sub SendData2Mail()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Try
	'		Dim strMessageGuid As String = Guid.NewGuid().ToString
	'		ClsOfDetails.GetMessageGuid = strMessageGuid

	'		txtAllMessage.EditValue = String.Empty
	'		Dim strLogFilename As String = String.Format("{0}{1}.{2}", _ClsProgSetting.GetSpSFiles2DeletePath,
	'																							ClsOfDetails.GetMessageGuid, "tmp")
	'		_ClsLog.WriteTempLogFile(String.Format("***Programmstart: {0}", Now.ToString), strLogFilename)
	'		m_Logger.LogInfo(String.Format("{0}.{1}", strMethodeName, "Gestartet..."))


	'		BackgroundWorker1.WorkerSupportsCancellation = True
	'		BackgroundWorker1.RunWorkerAsync()      ' Multithreading starten

	'	Catch ex As Exception
	'		MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "CmdMail_0")

	'	End Try

	'End Sub


#End Region

	Private Sub txtOfNr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueOffer.ButtonClick

		If m_CurrentOfferNumber.GetValueOrDefault(0) = 0 Then Return
		If e.Button.Index = 1 Then RunOpenOffForm(m_CurrentOfferNumber)

	End Sub

	Sub Ongv_EMailRowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvEMail.GetRow(e.RowHandle)
			If dataRow Is Nothing Then Return
			Dim viewData = CType(dataRow, CustomerBulkEMailData)

			Select Case column.Name.ToLower
				Case "firma1".ToLower
					If viewData.KDNr > 0 Then OpenCustomerUI(viewData.KDNr)

				Case "CustomerResponsibleFullname".ToLower
					If viewData.ZHDRecNr > 0 Then OpenCResponsibleUI(viewData.KDNr, viewData.ZHDRecNr)

				Case Else
					If viewData.KDNr > 0 Then OpenCustomerUI(viewData.KDNr)

			End Select


		End If

	End Sub

	Private Sub CboFormat_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CboFormat.QueryPopUp
		Me.CboFormat.Properties.Items.Clear()

		'    "PDF", "MHTML", "HTML", "RTF", "XML", "Multi-TIFF", "Text", "XLS"
		Me.CboFormat.Properties.Items.Add(New ComboValue(m_Translate.GetSafeTranslationValue("PDF"), "PDF"))

	End Sub

	Private Sub cboMailTemplate_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboMailTemplate.SelectedIndexChanged
		SetVisibility4SendButtons()
	End Sub

	Sub SetVisibility4SendButtons()

		xtabMain.Enabled = cboMailTemplate.Text.ToLower.Contains("mail")

	End Sub

	'Private Sub OntgsFaxRecipientSelection_Toggled(sender As Object, e As EventArgs) Handles tgsFaxRecipientSelection.Toggled
	'	FillMyDataGrid()
	'End Sub

	Private Sub OntgsMailRecipientSelection_Toggled(sender As Object, e As EventArgs) Handles tgsMailRecipientSelection.Toggled
		SelDeSelectItems(tgsMailRecipientSelection.EditValue)
	End Sub

	Private Sub SelDeSelectItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of CustomerBulkEMailData) = grdEMail.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.Selected = selectItem
			Next
		End If

		gvEMail.RefreshData()

	End Sub

	Sub GetQery4ShowInMailGrid()
		Dim strBeginTrySql As String = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH"
		Dim strTestSql As String = String.Format("{0} SELECT * {1} FROM _Kundenliste_{2} ",
															 strBeginTrySql,
															 "Into #KD_Mailing",
															 _ClsProgSetting.GetLogedUSNr)
		strTestSql &= "SELECT KDNr, ZHDRecNr, Firma1, (Case KD_Mail_Mailing "
		strTestSql &= "When 1 Then '' "
		strTestSql &= "Else KDeMail "
		strTestSql &= "End ) As "
		strTestSql &= "KDeMail, "
		strTestSql &= "KD_Mail_Mailing, "

		strTestSql &= "Vorname, Nachname,	"
		strTestSql &= "(Case ZHD_Mail_Mailing "
		strTestSql &= "When 1 Then '' "
		strTestSql &= "Else ZHDeMail "
		strTestSql &= "End ) As 	"
		strTestSql &= "ZHDeMail, "
		strTestSql &= "ZHD_Mail_Mailing "

		strTestSql &= "From #KD_Mailing Where KDeMail + ZHDeMail <> '' "
		strTestSql &= "Group By KDNr, ZHDRecNr, Firma1, KDeMail, KD_Mail_Mailing, ZHDeMail, ZHD_Mail_Mailing, Vorname, Nachname "
		strTestSql &= "Order By Firma1, Vorname, Nachname"

		Me.Sql2Open4Grid = strTestSql

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#Region "Helper methodes"

	Sub OpenCustomerUI(ByVal _iKDNr As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, _iKDNr)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub


	Sub OpenCResponsibleUI(ByVal _iKDNr As Integer, ByVal _iKDZHDNr As Integer)

		Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr))

		If (responsiblePersonsFrom.LoadResponsiblePersonData(_iKDNr, _iKDZHDNr)) Then
			responsiblePersonsFrom.Show()
		End If

	End Sub


	Private Sub OnlueOffer_EditValueChanged(sender As Object, e As EventArgs)

		m_CurrentOfferNumber = CType(lueOffer.EditValue, Integer)

	End Sub

	Private Sub grpSendLog_CustomButtonClick(sender As Object, e As BaseButtonEventArgs) Handles grpSendLog.CustomButtonClick

		Try

			If File.Exists(m_LOGFileName) Then
				Dim subject As String = "Versand Ergebnis"
				Dim body As String = String.Format("<b>Versand Ergebnis:</b><br>{0}<br>{1}<br>{2}", m_InitializationData.UserData.UserMDName, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UsereMail)
				Dim newLOGFile = Path.ChangeExtension(m_LOGFileName, "txt")
				File.Copy(m_LOGFileName, newLOGFile)
				Dim sendResult = m_UtilityUI.SendMailNotification(subject, body, m_InitializationData.UserData.UsereMail, New List(Of String) From {newLOGFile})
				File.Delete(newLOGFile)

				If sendResult Then
					m_UtilityUI.ShowInfoDialog(String.Format("Ihre LOG-Datei wurde an {0} gemailt.", m_InitializationData.UserData.UsereMail))
				End If

			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub rgRecipients_EditValueChanged(sender As Object, e As EventArgs)
		If m_SuppressUIEvents Then Return

		LoadEMailData()
	End Sub

	Private Sub cboEmailTemplateFilename_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEmailTemplateFilename.SelectedIndexChanged

	End Sub

	Private Sub cboEmailTemplateFilename_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles cboEmailTemplateFilename.ButtonClick
		If e.Button.Index = 1 Then
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Achtung: Durch Fehlerhafte Eingabe in der Vorlage, können die Mails nicht korrekt versendet werden!"), m_Translate.GetSafeTranslationValue("Vorlage anpassen"), MessageBoxIcon.Warning)
			Dim oldTemplatefile = cboEmailTemplateFilename.EditValue
			Dim newFilename = Path.GetFileNameWithoutExtension(oldTemplatefile) & "_" & m_InitializationData.UserData.UserKST & "_" & Path.GetRandomFileName

			Dim backupPath As String = Path.Combine(Path.GetDirectoryName(oldTemplatefile), "Backups")
			Try
				If Not Directory.Exists(backupPath) Then Directory.CreateDirectory(backupPath)

			Catch ex As Exception
				backupPath = Path.GetDirectoryName(oldTemplatefile)
			End Try

			oldTemplatefile = Path.Combine(backupPath, newFilename)

			File.Copy(cboEmailTemplateFilename.EditValue, oldTemplatefile)
			Process.Start(cboEmailTemplateFilename.EditValue)
		End If
	End Sub


#End Region


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



End Class




