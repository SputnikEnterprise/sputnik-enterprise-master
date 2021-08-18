
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports System.IO

Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins

Imports DevExpress.Data
Imports DevExpress.XtraBars
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils
Imports DevExpress.XtraTab
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository

Imports System.Windows.Forms
Imports System.Drawing
Imports SP.Infrastructure.Logging

Imports System.Threading

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility


Imports SP
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports System.ComponentModel
Imports DevExpress.Pdf
Imports SPProgUtility.CommonXmlUtility

Public Class frmPMail


#Region "private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING As String = "MD_{0}/Mailing"
	Private Const MANDANT_XML_SETTING_WOS_PROPOSE_GUID As String = "MD_{0}/Export/sendproposeattachmenttowos"

#End Region


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private ProposeMailSetting As New ClsMailSetting

	Private _ClsDivFunc As New ClsDivFunc
	Private _Clsreg As New SPProgUtility.ClsDivReg

	Private _regex As New ClsDivFunc
	Private m_xml As New ClsXML

	Private _iMail_ID As Integer = 0

	Private m_PBezeichnung As String = String.Empty
	Private m_Files2Send As String() = {""}

	Private m_liUserData As New List(Of String)
	Private m_liMAData As New List(Of String)
	'Private m_liMADocData As New List(Of String)
	Private m_liKDData As New List(Of String)
	Private m_liTemplate As New List(Of String)

	Private m_EmployeeDocumentData As New BindingList(Of EmployeeDocumentData)
	Private m_SortedEmployeeDocumentData As New BindingList(Of EmployeeDocumentData)

	Private Property m_SelectedFile2Import As String

	Private m_TemplateData As MailTemplateData

	Private m_mandant As New Mandant
	Private m_common As New CommonSetting
	Private m_path As New ClsProgPath

	Private m_SmtpServer As String
	Private m_SmtpPort As Integer
	Private m_EnableSSL As Boolean

	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' Used to copy contacts for employees (Kandidaten)
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI
	Private m_Utility As SP.Infrastructure.Utility
	Private m_MandantSettingsXml As SettingsXml
	Private m_MailingSetting As String
	Private m_AllowedToSendWOS As Boolean


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsMailSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		Me.AllowFormGlass = DevExpress.Utils.DefaultBoolean.False

		InitializeComponent()

		Me.ProposeMailSetting = _setting
		m_InitializationData = New Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData)

		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(ClsDataDetail.MDData.MDDbConn, ClsDataDetail.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(ClsDataDetail.MDData.MDDbConn, ClsDataDetail.UserData.UserLanguage)

		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_TemplateData = New MailTemplateData

		m_Files2Send = ProposeMailSetting.Doc2Send.ToArray

		m_PBezeichnung = _ClsDivFunc.TranslateMailSubject(ProposeMailSetting.ProposeNr2Send)

		ClsDataDetail.GetProposalMANr = ProposeMailSetting.MANr2Send
		ClsDataDetail.GetProposalKDNr = ProposeMailSetting.KDNr2Send
		ClsDataDetail.GetProposalZHDNr = ProposeMailSetting.KDZNr2Send
		ClsDataDetail.GetProposalVakNr = ProposeMailSetting.VakNr2Send

		Try
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle:{1}", strMethodeName, ex.Message))

		End Try
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(ClsDataDetail.MDData.MDNr, Now.Year))
		m_MailingSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING, ClsDataDetail.MDData.MDNr)

		TranslateControls()
		Dim smtpData = GetMailingData(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year)
		m_SmtpServer = smtpData.SMTPServer
		m_SmtpPort = smtpData.SMTPPort
		m_EnableSSL = smtpData.EnableSSL
		If m_EnableSSL = Nothing Then m_EnableSSL = False

		m_AllowedToSendWOS = SendProposeAttachmentToWOS

		Reset()

	End Sub


#End Region


#Region "Private properties"

	Private ReadOnly Property SendProposeAttachmentToWOS() As Boolean
		Get

			If ProposeMailSetting.KDZNr2Send.GetValueOrDefault(0) = 0 Then Return False

			'Dim customerData As CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(ProposeMailSetting.KDNr2Send, m_InitializationData.UserData.UserFiliale)
			'Dim customereMailData As List(Of CustomerAssignedEmailData) = m_CustomerDatabaseAccess.LoadAssignedEmailsOfCustomer(ProposeMailSetting.KDNr2Send)

			'If customerData Is Nothing OrElse customereMailData Is Nothing Then
			'	m_Logger.LogDebug(String.Format("Kundendaten {0} mit Vorschlagnummer {1} wurde nicht gefunden.", ProposeMailSetting.KDNr2Send, ProposeMailSetting.ProposeNr2Send))

			'	Return False
			'End If
			'If Not customerData.sendToWOS OrElse customereMailData.Count = 0 Then
			'	m_Logger.LogDebug(String.Format("Der Kunde {0} ist nicht WOS-pflichtig!", ProposeMailSetting.KDNr2Send))

			'	Return False
			'End If

			Return m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_PROPOSE_GUID, m_InitializationData.MDData.MDNr)), False)

		End Get
	End Property

	''' <summary>
	''' Gets the selected document.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedRecord As EmployeeDocumentData
		Get
			Dim gvRP = TryCast(gridDocuments.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim doc = CType(gvRP.GetRow(selectedRows(0)), EmployeeDocumentData)

					Return doc
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected documentlist to select.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedRecordToSelect As EmployeeDocumentData
		Get
			Dim gvRP = TryCast(grdFileToSelect.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim doc = CType(gvRP.GetRow(selectedRows(0)), EmployeeDocumentData)

					Return doc
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected documentlist to merge.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedRecordToMerge As EmployeeDocumentData
		Get
			Dim gvRP = TryCast(grdSelectedFile.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim doc = CType(gvRP.GetRow(selectedRows(0)), EmployeeDocumentData)

					Return doc
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property EMailEnableSSL As Boolean
		Get
			Dim value As Boolean = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/smtp-enablessl", m_MailingSetting)), False)

			Return value
		End Get
	End Property

#End Region

	Sub TranslateControls()

		Me.Text = String.Format(m_Translate.GetSafeTranslationValue("{0} - Nachricht (HTML)"), m_Translate.GetSafeTranslationValue("Unbenannt"))

		tgsSendToWOS.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSendToWOS.Properties.OffText)
		tgsSendToWOS.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSendToWOS.Properties.OnText)
		tgsEMailPriority.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsEMailPriority.Properties.OffText)
		tgsEMailPriority.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsEMailPriority.Properties.OnText)

		pgrpZwischenablage.Text = m_Translate.GetSafeTranslationValue(Me.pgrpZwischenablage.Text)
		pgrpFont.Text = m_Translate.GetSafeTranslationValue(Me.pgrpFont.Text)
		pgrpMailvorlage.Text = m_Translate.GetSafeTranslationValue(Me.pgrpMailvorlage.Text)
		pgrpFelder.Text = m_Translate.GetSafeTranslationValue(Me.pgrpFelder.Text)
		PasteItem1.Caption = m_Translate.GetSafeTranslationValue(Me.PasteItem1.Caption)
		bbiAnhang.Caption = m_Translate.GetSafeTranslationValue(Me.bbiAnhang.Caption)
		bbiFelder.Caption = m_Translate.GetSafeTranslationValue(Me.bbiFelder.Caption)

		grpVersandfelder.Text = m_Translate.GetSafeTranslationValue(Me.grpVersandfelder.Text)
		btnSend.Text = m_Translate.GetSafeTranslationValue(Me.btnSend.Text)
		btnSender.Text = m_Translate.GetSafeTranslationValue(Me.btnSender.Text)
		btnRecipient.Text = m_Translate.GetSafeTranslationValue(Me.btnRecipient.Text)
		btn_Translate.Caption = m_Translate.GetSafeTranslationValue(Me.btn_Translate.Caption)
		lblCC.Text = m_Translate.GetSafeTranslationValue(Me.lblCC.Text)
		lblBCC.Text = m_Translate.GetSafeTranslationValue(Me.lblBCC.Text)
		lblBetreff.Text = m_Translate.GetSafeTranslationValue(Me.lblBetreff.Text)

		grpAnhaenge.Text = m_Translate.GetSafeTranslationValue(Me.grpAnhaenge.Text)
		grpSendWOS.Text = m_Translate.GetSafeTranslationValue(grpSendWOS.Text)
		grpVersandPriority.Text = m_Translate.GetSafeTranslationValue(grpVersandPriority.Text)

		lblUnsortierte.Text = m_Translate.GetSafeTranslationValue(Me.lblUnsortierte.Text)
		lblSortierte.Text = m_Translate.GetSafeTranslationValue(Me.lblSortierte.Text)
		lblDateiname.Text = m_Translate.GetSafeTranslationValue(Me.lblDateiname.Text)
		sbtnCreateOnePDF.Text = m_Translate.GetSafeTranslationValue(Me.sbtnCreateOnePDF.Text)

		xtabHaupt.Text = m_Translate.GetSafeTranslationValue(Me.xtabHaupt.Text)
		xtabHtml.Text = m_Translate.GetSafeTranslationValue(Me.xtabHtml.Text)
		lblStatus.Caption = m_Translate.GetSafeTranslationValue(Me.lblStatus.Caption)

	End Sub


	Private Sub Reset()

		tgsSendToWOS.EditValue = m_AllowedToSendWOS
		tgsSendToWOS.Enabled = m_AllowedToSendWOS

		ResetDocumentGrid()
		ResetDocumentToSelectGrid()
		ResetSelectedDocumentGrid()

	End Sub

	''' <summary>
	''' Resets the document grid.
	''' </summary>
	Private Sub ResetDocumentGrid()

		gvDocuments.OptionsView.ShowIndicator = False
		gvDocuments.OptionsView.ShowAutoFilterRow = False
		gvDocuments.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvDocuments.OptionsView.ShowColumnHeaders = False
		gvDocuments.OptionsView.ShowFooter = False
		gvDocuments.OptionsView.ShowHorizontalLines = DefaultBoolean.False
		gvDocuments.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvDocuments.OptionsBehavior.Editable = True


		' Reset the grid
		gvDocuments.Columns.Clear()

		Dim columnChecked As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChecked.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnChecked.OptionsColumn.AllowEdit = True
		columnChecked.Caption = m_Translate.GetSafeTranslationValue(" ")
		columnChecked.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnChecked.Name = "Checked"
		columnChecked.FieldName = "Checked"
		columnChecked.Visible = True
		columnChecked.Width = 5
		gvDocuments.Columns.Add(columnChecked)

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.OptionsColumn.AllowEdit = False
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Datei")
		columnDate.Name = "EmployeeFilename"
		columnDate.FieldName = "EmployeeFilename"
		columnDate.Visible = True
		columnDate.Width = 250
		gvDocuments.Columns.Add(columnDate)


		gridDocuments.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the document to select grid.
	''' </summary>
	Private Sub ResetDocumentToSelectGrid()

		gvFileToSelect.OptionsView.ShowIndicator = False
		gvFileToSelect.OptionsView.ShowAutoFilterRow = False
		gvFileToSelect.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvFileToSelect.OptionsView.ShowFooter = False
		gvFileToSelect.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvFileToSelect.OptionsBehavior.Editable = True

		' Reset the grid
		gvFileToSelect.Columns.Clear()

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.OptionsColumn.AllowEdit = False
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Vorhandene Dateien")
		columnDate.Name = "EmployeeFilename"
		columnDate.FieldName = "EmployeeFilename"
		columnDate.Visible = True
		gvFileToSelect.Columns.Add(columnDate)


		grdFileToSelect.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the selected document grid.
	''' </summary>
	Private Sub ResetSelectedDocumentGrid()

		gvSelectedFile.OptionsView.ShowIndicator = False
		gvSelectedFile.OptionsView.ShowAutoFilterRow = False
		gvSelectedFile.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvSelectedFile.OptionsView.ShowFooter = False
		gvSelectedFile.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
		gvSelectedFile.OptionsBehavior.Editable = True

		' Reset the grid
		gvSelectedFile.Columns.Clear()

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.OptionsColumn.AllowEdit = False
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Ausgewählte Dateien")
		columnDate.Name = "EmployeeFilename"
		columnDate.FieldName = "EmployeeFilename"
		columnDate.Visible = True
		gvSelectedFile.Columns.Add(columnDate)


		grdSelectedFile.DataSource = Nothing

	End Sub


	Private Sub frmPMail_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		pcc_1Filename.HidePopup()

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iMailHeight = Me.Height
			My.Settings.iMailWidth = Me.Width
			My.Settings.frmMailLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.DeliveryPriority = tgsEMailPriority.EditValue

			My.Settings.Save()

		End If

		Me.rtfContent.Dispose()
		Me.XtraTabControl1.Dispose()
		Me.RibbonControl.Dispose()

	End Sub

	Private Sub frmPMail_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount

		Try
			Dim SenderData = LoadSenderEMailData(ProposeMailSetting.ProposeNr2Send)
			If Not SenderData Is Nothing AndAlso SenderData.Count > 0 Then
				For Each itm In SenderData
					If Not String.IsNullOrWhiteSpace(itm.UserEMail) Then
						If Not m_liUserData.Contains(itm.UserEMail) Then m_liUserData.Add(itm.UserEMail)
					End If
				Next
			End If

			m_liMAData = GetKandidatenDaten(ProposeMailSetting.MANr2Send)
			'Dim data = GetKandidatenDocDaten(ProposeMailSetting.MANr2Send)
			'If Not data Is Nothing Then
			'	For Each itm In data
			'		If Not String.IsNullOrWhiteSpace(itm.EmployeeDocumentFileName) Then
			'			m_liMADocData.Add(itm.EmployeeDocumentFileName)
			'		End If
			'	Next
			'	'm_liMADocData = GetKandidatenDocDaten(ProposeMailSetting.MANr2Send)
			'End If
			'LoadEmployeeDocumentData()
			LoadFilteredDocumentData()

			m_liKDData = GetKundenDaten(ProposeMailSetting.KDNr2Send, ProposeMailSetting.KDZNr2Send)
		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)
		End Try

		rtfContent.Dock = DockStyle.Fill

		Try
			Dim oFontName = New Font("Calibri", 11, FontStyle.Regular)
			Dim iIndent As Integer = CInt(Val(m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Sonstiges", "LL_IndentSize", "20")))
			Dim ivalue As Integer = CInt(Val(m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Sonstiges", "FontSize", "11")))
			Dim strValue As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Sonstiges", "FontName", "Calibri")

			oFontName = New Font(strValue, ivalue, FontStyle.Regular)
			Me.rtfContent.Font = oFontName
			'Me.bi_Mailtpl.AutoFillWidth = True
			Me.bi_Mailtpl.EditHeight = 300
			'Me.bi_Mailtpl.Width = 400

			Try
				If My.Settings.iMailHeight > 0 Then Me.Height = My.Settings.iMailHeight
				If My.Settings.iMailWidth > 0 Then Me.Width = My.Settings.iMailWidth
				If My.Settings.frmMailLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmMailLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If
				tgsEMailPriority.EditValue = My.Settings.DeliveryPriority

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Formsize.{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Einstellungen für Schriftarten.{1}", strMethodeName, ex.Message))

		End Try

		Me.txt_MailBetreff.Text = m_PBezeichnung
		Me.Text = String.Format(m_Translate.GetSafeTranslationValue("{0} - Nachricht (HTML)"), m_PBezeichnung)

		Me.ChangeFontNameItem1.Caption = String.Empty
		Me.ChangeFontSizeItem1.Caption = String.Empty

		m_Logger.LogInfo(String.Format("{0}. Ladezeit für Form: {1} s.", strMethodeName, ((System.Environment.TickCount - Time_1) / 1000)))

		_tCreatePoupUpMenu()

	End Sub

	Private Function LoadFilteredDocumentData() As Boolean
		Dim success As Boolean = True
		m_EmployeeDocumentData = Nothing
		gridDocuments.DataSource = Nothing

		Dim data = LoadEmployeeDocumentsForProposeAttachment(ProposeMailSetting.MANr2Send)
		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumente konnten nicht geladen werden."))

			Return False
		End If

		Dim listDataSource As BindingList(Of EmployeeDocumentData) = New BindingList(Of EmployeeDocumentData)
		If m_Files2Send.Length > 0 Then
			For i As Integer = 0 To m_Files2Send.Length - 1
				If m_Files2Send(i) <> String.Empty Then
					Dim documentViewData = New EmployeeDocumentData With {.Bezeichnung = Path.GetFileNameWithoutExtension(m_Files2Send(i).ToString),
																																.DocContent = m_Utility.LoadFileBytes(m_Files2Send(i)),
																																.ScanExtension = "PDF",
																																.WatchCategory = False,
																																.RecGuid = Guid.NewGuid.ToString(),
																																.Checked = False}

					If Not String.IsNullOrWhiteSpace(documentViewData.EmployeeFilename) Then listDataSource.Add(documentViewData)

				End If
			Next i
		End If

		For Each document In data
			Dim documentViewData As EmployeeDocumentData = document

			listDataSource.Add(documentViewData)
		Next

		m_EmployeeDocumentData = listDataSource
		gridDocuments.DataSource = listDataSource


		Return (Not m_EmployeeDocumentData Is Nothing)
	End Function

	Private Function LoadDocumentToSelectData() As Boolean

		gridDocuments.DataSource = m_EmployeeDocumentData

		Return True
	End Function

	Sub _tCreatePoupUpMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim MailtplData = ListMailTemplateName()

		Me.RibbonControl.Invoke(Sub()
															Me.rep_MailtplGroup.Items.Clear()
															For Each itm In MailtplData
																Me.rep_MailtplGroup.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem With {.Description = itm.itemBez, .Value = itm.itemValue})
															Next
														End Sub)
		If MailtplData.Count >= 9 Then
			bi_Mailtpl.Width = 550
		ElseIf MailtplData.Count >= 6 Then
			bi_Mailtpl.Width = 400
		Else
			bi_Mailtpl.Width = 300
		End If

		Try
			'SetKDData(liKDData)
			Me.txt_MailAn.Invoke(Sub()
														 SetKDData(m_liKDData)
													 End Sub)
		Catch ex As Exception
			m_Logger.LogError("SetKDData: " & ex.StackTrace)

		End Try

		Try
			'CreateUserPopupMenu(liUserData)
			Me.lblAbsender.Invoke(Sub()
															CreateUserPopupMenu(m_liUserData)
														End Sub)
		Catch ex As Exception
			m_Logger.LogError("CreateUserPopupMenu: " & ex.StackTrace)

		End Try
		Try
			'CreateKDPopupMenu()
			Me.btnRecipient.Invoke(Sub()
															 CreateKDPopupMenu()
														 End Sub)
		Catch ex As Exception
			m_Logger.LogError("CreateKDPopupMenu: " & ex.StackTrace)

		End Try

		CreatetplPopupMenu()

		m_Logger.LogInfo(String.Format("{0}. Ladezeit für Form: {1} s.", strMethodeName, ((System.Environment.TickCount - Time_1) / 1000)))

	End Sub

	Private Sub LoadmyMailTemplate(ByVal strDocName As String)

		Try
			If File.Exists(strDocName) Then
				Me.rtfContent.BeginUpdate()
				Me.rtfContent.LoadDocument(strDocName)
				Me.rtfContent.RtfText = _ClsDivFunc.TranslateSeletedTemplate(Me.rtfContent)
				Me.rtfContent.EndUpdate()

			Else
				Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Die Vorlagedatei existiert nicht.{0}{1}"), vbNewLine, strDocName)
				m_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("Vorlage öffnen"), MessageBoxIcon.Asterisk)

			End If
			tgsSendToWOS.EditValue = rtfContent.RtfText.ToLower.Contains("sponlinedoc/DefaultPage.aspx?".ToLower) AndAlso m_AllowedToSendWOS


		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)
			Me.rtfContent.LoadDocument(strDocName)

		End Try
		Me.WebBrowser1.DocumentText = Me.rtfContent.HtmlText

	End Sub

	Private Sub CreatetplPopupMenu()
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim barmanager As New DevExpress.XtraBars.BarManager
		barmanager.Images = Me.ImageList1
		Dim liTplItems As New List(Of String)(New String() {
																												"{TMPL_VAR name='KDzAnredeform'}",
																												"{TMPL_VAR name='KDzFullAnredeform'}",
																												"{TMPL_VAR name='KDzAnrede'}",
																												"{TMPL_VAR name='KDzVorname'}",
																												"{TMPL_VAR name='KDzNachname'}", _
 _
																												"{TMPL_VAR name='P_MANachname'}",
																												"{TMPL_VAR name='P_MAVorname'}",
																												"{TMPL_VAR name='P_MAGeschlecht'}",
																												"{TMPL_VAR name='P_MAAnrede'}",
																												"{TMPL_VAR name='P_MAGebDat'}",
																												"{TMPL_VAR name='P_MAAlter'}",
																												"{TMPL_VAR name='MA_Res1'}",
																												"{TMPL_VAR name='MA_Res2'}",
																												"{TMPL_VAR name='MA_Res3'}",
																												"{TMPL_VAR name='MA_Res4'}",
																												"{TMPL_VAR name='MA_Res5'}",
																												"{TMPL_VAR name='MA_Res6'}",
																												"{TMPL_VAR name='MA_Res7'}",
																												"{TMPL_VAR name='MA_Res8'}",
																												"{TMPL_VAR name='MA_Res9'}",
																												"{TMPL_VAR name='MA_Res10'}",
																												"{TMPL_VAR name='MA_Res11'}",
																												"{TMPL_VAR name='MA_Res12'}",
																												"{TMPL_VAR name='MA_Res13'}",
																												"{TMPL_VAR name='MA_Res14'}",
																												"{TMPL_VAR name='MA_Res15'}",
																												"{TMPL_VAR name='P_MABeruf'}", _
 _
																												"{TMPL_VAR name='USMDName'}",
																												"{TMPL_VAR name='USMDName2'}",
																												"{TMPL_VAR name='USMDPostfach'}",
																												"{TMPL_VAR name='USMDStrasse'}",
																												"{TMPL_VAR name='USMDort'}",
																												"{TMPL_VAR name='USMDPlz'}",
																												"{TMPL_VAR name='USMDLand'}",
																												"{TMPL_VAR name='USMDTelefon'}",
																												"{TMPL_VAR name='USMDTelefax'}",
																												"{TMPL_VAR name='USMDeMail'}",
																												"{TMPL_VAR name='USMDHomepage'}",
																												"{TMPL_VAR name='USAnrede'}",
																												"{TMPL_VAR name='USNachname'}",
																												"{TMPL_VAR name='USVorname'}",
																												"{TMPL_VAR name='USPostfach'}",
																												"{TMPL_VAR name='USStrasse'}",
																												"{TMPL_VAR name='USPLZ'}",
																												"{TMPL_VAR name='USOrt'}",
																												"{TMPL_VAR name='USLand'}",
																												"{TMPL_VAR name='USTelefon'}",
																												"{TMPL_VAR name='USNatel'}",
																												"{TMPL_VAR name='USTelefax'}",
																												"{TMPL_VAR name='USeMail'}",
																												"{TMPL_VAR name='USTitel_1'}",
																												"{TMPL_VAR name='USTitel_2'}",
																												"{TMPL_VAR name='P_Berater1'}",
																												"{TMPL_VAR name='P_Berater2'}",
																												"{TMPL_VAR name='USAbteilung'}", _
 _
																												"{TMPL_VAR name='P_Nr'}",
																												"{TMPL_VAR name='P_MANr'}",
																												"{TMPL_VAR name='P_KDNr'}",
																												"{TMPL_VAR name='P_KDzNr'}",
																												"{TMPL_VAR name='P_Bez'}",
																												"{TMPL_VAR name='P_KD_Tarif'}",
																												"{TMPL_VAR name='P_ESAls'}",
																												"{TMPL_VAR name='P_ArbBegin'}", _
 _
 _
																												"{TMPL_VAR name='P_Zusatz1'}",
																												"{TMPL_VAR name='P_Zusatz2'}",
																												"{TMPL_VAR name='P_Zusatz3'}",
																												"{TMPL_VAR name='P_Zusatz4'}",
																												"{TMPL_VAR name='P_Zusatz5'}", _
 _
											  "{TMPL_VAR name='ProposeattachmentLink'}"})

		Me.bi_InsertField.DropDownControl = popupMenu
		For i As Integer = 0 To liTplItems.Count - 1
			popupMenu.Manager = barmanager

			Dim itm As New DevExpress.XtraBars.BarButtonItem
			itm = New DevExpress.XtraBars.BarButtonItem
			itm.Caption = liTplItems(i).ToString

			popupMenu.AddItem(itm)

			AddHandler itm.ItemClick, AddressOf InsertTplVarIntoDoc
		Next

		'Throw New System.NotImplementedException()
	End Sub

	Sub InsertTplVarIntoDoc(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.rtfContent.Document.InsertText(Me.rtfContent.Document.CaretPosition, e.Item.Caption)
	End Sub

	Private Sub CreateUserPopupMenu(ByVal liUserData As List(Of String))
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim barmanager As New DevExpress.XtraBars.BarManager
		barmanager.Images = Me.ImageCollection1

		Try
			Me.btnSender.DropDownControl = popupMenu
			For i As Integer = 0 To liUserData.Count - 1
				Dim myValue As String() = liUserData(i).Split(CChar("#"))

				If myValue(0).ToString <> String.Empty Then
					popupMenu.Manager = barmanager

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm = New DevExpress.XtraBars.BarButtonItem

					itm.Caption = liUserData(i).ToString
					If i = 0 Then itm.ImageIndex = 4
					If i = 1 Then itm.ImageIndex = 3

					popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next
			Me.lblAbsender.Text = liUserData(0).ToString
		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)

		End Try

	End Sub

	'Sub SetMAData(ByVal liMADocData As List(Of String))
	'	Return
	'	Try
	'		Me.lstMADoc.Items.Clear()


	'		For i As Integer = 0 To liMADocData.Count - 1
	'			Me.lstMADoc.Items.Add("")
	'			Dim aItem As String() = liMADocData(i).Split(CChar("#"))
	'			Me.lstMADoc.Items.Item(i).Value = String.Format("{0}#{1}", aItem(0), If(aItem.Length > 1, aItem(1), String.Empty))
	'			Me.lstMADoc.Items.Item(i).Description = String.Format("{0}", Path.GetFileName(aItem(0)))
	'		Next

	'		Me.lstMADoc.UnCheckAll()

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.StackTrace)
	'	End Try

	'End Sub

	Sub SetKDData(ByVal liKDData As List(Of String))
		Try
			m_Logger.LogDebug(String.Format("liKDData.Count: {0}", liKDData.Count))

			If liKDData.Count = 0 Then Exit Sub
			Dim myValue As String() = liKDData(0).Split(CChar("#"))

			Me.txt_MailAn.Text = myValue(0).ToString

			If ProposeMailSetting.KDZNr2Send.HasValue Then

				If liKDData.Count > 1 Then
					myValue = liKDData(1).Split(CChar("#"))
					Me.txt_MailAn.Text = myValue(0).ToString
				End If

			End If
		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)

		End Try

	End Sub

	Private Sub CreateKDPopupMenu()
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim barmanager As New DevExpress.XtraBars.BarManager
		barmanager.Images = ImageCollection1
		Me.btnRecipient.DropDownControl = popupMenu

		popupMenu.Manager = barmanager

		Dim itm As New DevExpress.XtraBars.BarButtonItem
		itm = New DevExpress.XtraBars.BarButtonItem

		Try
			' eigene Benutzer
			itm.Caption = Me.lblAbsender.Text
			itm.ImageIndex = 3
			popupMenu.AddItem(itm)
			AddHandler itm.ItemClick, AddressOf GetKDMenuItem

			Dim myValue As String() = m_liKDData(0).Split(CChar("#"))

			Dim strKDMail As String = myValue(0).ToString

			' ZHD-Mail-Adresse
			If Not String.IsNullOrWhiteSpace(Me.txt_MailAn.Text) Then
				itm = New DevExpress.XtraBars.BarButtonItem
				itm.Caption = Me.txt_MailAn.Text
				itm.ImageIndex = 1
				popupMenu.AddItem(itm)
				AddHandler itm.ItemClick, AddressOf GetKDMenuItem
			End If

			' KD-Mail-Adresse
			If Not String.IsNullOrWhiteSpace(strKDMail) Then
				itm = New DevExpress.XtraBars.BarButtonItem
				itm.Caption = strKDMail
				itm.ImageIndex = 2
				popupMenu.AddItem(itm)
				AddHandler itm.ItemClick, AddressOf GetKDMenuItem
			End If

			' MA-Mail-Adresse
			myValue = m_liMAData(0).Split(CChar("#"))
			If Not String.IsNullOrWhiteSpace(myValue(0)) Then
				itm = New DevExpress.XtraBars.BarButtonItem
				itm.Caption = myValue(0)
				itm.ImageIndex = 0
				popupMenu.AddItem(itm)
				AddHandler itm.ItemClick, AddressOf GetKDMenuItem
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.lblAbsender.Text = e.Item.Caption
	End Sub

	Sub GetKDMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.txt_MailAn.Text = e.Item.Caption
	End Sub

	Private Sub btnAbsender_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSender.Click
		btnSender.ShowDropDown()
	End Sub

	'Private Sub lstMADoc_ItemCheck(sender As Object, e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs) Handles lstMADoc.ItemCheck

	'	Dim row = m_EmployeeDocumentData(e.Index)
	'	row.checked = (e.State = CheckState.Checked)

	'End Sub

	Private Sub AddSelectedDocumentData(ByVal filename As String())

		If Not File.Exists(filename(0)) Then Return

		Dim data = m_EmployeeDocumentData
		If filename.Length > 0 Then
			For i As Integer = 0 To filename.Length - 1
				If Str(i) <> String.Empty Then
					Dim documentViewData = New EmployeeDocumentData With {.Bezeichnung = Path.GetFileName(filename(i).ToString),
																																.DocContent = m_Utility.LoadFileBytes(filename(i)), .ScanExtension = "PDF",
																																.RecGuid = Guid.NewGuid.ToString,
																																.Checked = False}

					data.Add(documentViewData)
				End If
			Next i
		End If

		m_EmployeeDocumentData = data
		gridDocuments.DataSource = m_EmployeeDocumentData

	End Sub

	Private Sub RemoveSelectedDocumentData()

		Dim data As EmployeeDocumentData = SelectedRecord ' SelectedDocumentFilterData
		If data Is Nothing Then Return
		m_EmployeeDocumentData.Remove(data) '  Where(Function(data) data.RecGuid = data1.RecGuid And data.EmployeeDocumentFullFileName = data1.EmployeeDocumentFullFileName).FirstOrDefault 

		gridDocuments.DataSource = m_EmployeeDocumentData

	End Sub

	Private Sub lstMADoc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
		If e.KeyCode = Keys.Delete Then
			RemoveSelectedDocumentData()
		End If
	End Sub


	Private Sub lstMADoc_DragEnter(ByVal sender As Object,
																 ByVal e As System.Windows.Forms.DragEventArgs)

		If (e.Data.GetDataPresent(DataFormats.FileDrop) = True) Then
			e.Effect = DragDropEffects.Copy
		End If

	End Sub

	Private Sub lstMADoc_DragDrop(ByVal sender As Object,
																ByVal e As System.Windows.Forms.DragEventArgs)
		Dim str As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop, True), String())
		AddSelectedDocumentData(str)

	End Sub

	Private Sub CreateAnhangPopupMenu(ByVal loc As Point, ByVal itemindex As Integer)
		Dim barmanager As New DevExpress.XtraBars.BarManager

		barmanager.Images = Me.ImageList1
		barmanager.Form = Me

		If Me.gvDocuments.RowCount > 0 Then
			Dim _dXPopupMenu As DevExpress.Utils.Menu.DXPopupMenu = New DevExpress.Utils.Menu.DXPopupMenu()

			_dXPopupMenu.Items.Add(New DevExpress.Utils.Menu.DXMenuItem(m_Translate.GetSafeTranslationValue("Öffnen"), AddressOf GetAngangMenuItem, My.Resources.open))

			_dXPopupMenu.Items.Add(New DevExpress.Utils.Menu.DXMenuItem(m_Translate.GetSafeTranslationValue("Anlage entfernen"),
																																	AddressOf GetAngangMenuItem, My.Resources.Delete_K))

			DevExpress.Utils.Menu.MenuManagerHelper.ShowMenu(_dXPopupMenu, Me.gridDocuments.LookAndFeel, barmanager, gridDocuments, loc)

		End If

	End Sub

	Sub GetAngangMenuItem(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If sender.caption.ToLower.Contains(m_Translate.GetSafeTranslationValue("öffnen")) Then
			Dim data = SelectedRecord

			If data Is Nothing Then

				Return
			End If
			'Dim aFileInfo As String = data.EmployeeFilename ' Me.lstMADoc.Items(Me.lstMADoc.SelectedIndex).Value.ToString.Split(CChar("#"))
			Dim strFilename As String = data.EmployeeFilename ' aFileInfo(0)
			Dim filePath As String = Path.GetDirectoryName(data.EmployeeDocumentFullFileName)
			Dim fullFileName As String = Path.Combine(filePath, strFilename)
			Dim strRecID As String = data.RecGuid ' If(aFileInfo.Length > 1, aFileInfo(1), String.Empty)

			Try
				Dim success As Boolean = False
				success = m_Utility.WriteFileBytes(fullFileName, data.DocContent)

				If success Then
					Process.Start(fullFileName) 'lstMADoc.SelectedItems(0).value.ToString)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

			End Try

		ElseIf sender.caption.ToLower.Contains(m_Translate.GetSafeTranslationValue("entfernen")) Then
			RemoveSelectedDocumentData()

		End If

	End Sub

	Private Sub rep_MailtplGroup_EditValueChanged(sender As Object, e As EventArgs) Handles rep_MailtplGroup.EditValueChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_TemplateData = New MailTemplateData

		Try
			Dim strSelectedValue As String = CType(Me.Ribbon.Manager.ActiveEditor, DevExpress.XtraEditors.RadioGroup).EditValue.ToString
			For Each itm In rep_MailtplGroup.Items
				Dim Test = itm.value

				If Test = strSelectedValue Then
					m_TemplateData.itemBez = itm.ToString
					m_TemplateData.itemValue = strSelectedValue
				End If

			Next

			LoadmyMailTemplate(Path.Combine(m_mandant.GetSelectedMDTemplatePath(ClsDataDetail.ProgSettingData.SelectedMDNr), strSelectedValue))
			Me.rtfContent.Focus()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Function GetMailingData(ByVal iMDNr As Integer, ByVal iYear As Integer) As ClsMailingData 'Implements iPath.ICommonSetting.GetMailingData
		Dim result As New ClsMailingData
		Dim m_utilitiyprog As New SPProgUtility.MainUtilities.Utilities

		Try
			Dim xDoc As XDocument = XDocument.Load(m_mandant.GetSelectedMDDataXMLFilename(iMDNr, iYear))
			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Mailing")
												 Select New With {
																						.SmtpServer = m_utilitiyprog.GetSafeStringFromXElement(exportSetting.Element("SMTP-Server")),
																						.Smtpport = m_utilitiyprog.GetSafeStringFromXElement(exportSetting.Element("SMTP-Port")),
																						.faxDavidserver = m_utilitiyprog.GetSafeStringFromXElement(exportSetting.Element("faxdavidserver")),
																						.faxserver = m_utilitiyprog.GetSafeStringFromXElement(exportSetting.Element("fax-server")),
																						.faxextension = m_utilitiyprog.GetSafeStringFromXElement(exportSetting.Element("fax-extension")),
																						.faxforwarder = m_utilitiyprog.GetSafeStringFromXElement(exportSetting.Element("fax-forwarder"))
																							}).FirstOrDefault()

			result.SMTPServer = ConfigQuery.SmtpServer
			result.SMTPPort = ConfigQuery.Smtpport
			result.EnableSSL = EMailEnableSSL

			result.FaxDavidServer = ConfigQuery.faxDavidserver
			result.FaxServer = ConfigQuery.faxserver
			result.FaxExtension = ConfigQuery.faxextension
			result.FaxForwarder = ConfigQuery.faxforwarder

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result.EnableSSL = False

		End Try

		Return result
	End Function

	Function SendProposeWitheMail(ByVal strBetreff As String,
															ByVal strBody As String,
															ByVal strVon As String,
															ByVal strTo As String,
															ByVal strCC As String,
															ByVal strBcc As String,
															ByVal fileToSend As List(Of String)) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim success As Boolean = True

		If m_SmtpServer Is Nothing OrElse String.IsNullOrWhiteSpace(m_SmtpServer) Then
			m_Logger.LogError("Fehlerhafter oder leerer SMTP-Server!")
		End If

		Try
			Dim regex As New ClsDivFunc
			regex.GetUSData()

			If tgsSendToWOS.EditValue Then
				If fileToSend.Count = 1 Then
					Dim objWOS As New SP.Main.Notify.WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)
					success = success AndAlso objWOS.TransferCustomerProposeDataToWOS(ProposeMailSetting.ProposeNr2Send, File.ReadAllBytes(fileToSend(0)))
				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Es wurde keine Datei für den Versand ausgewählt. Es muss nur eine Datei ausgewählt werden."))

					Return False
				End If
			End If

			success = success AndAlso SendMailToWithExchange(True, strVon, strTo, strCC, strBcc, strBetreff, strBody, tgsEMailPriority.EditValue,
																										 If(tgsSendToWOS.EditValue, Nothing, fileToSend), m_SmtpServer, regex.Exchange_USName, regex.Exchange_USPW)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			success = False

		Finally
			'Finalize()

		End Try

		Return success
	End Function

	Function SendMailToWithExchange(ByVal bIsHtml As Boolean,
																ByVal strFrom As String,
																ByVal strTo As String,
																ByVal strCC As String,
																ByVal strBCC As String,
																ByVal strSubject As String,
																ByVal strBody As String,
																ByVal iPriority As Boolean,
																ByVal aAttachmentFile As List(Of String),
																ByVal strSmtp As String,
																ByVal strUserName As String,
																ByVal strUserPW As String) As Boolean
		Dim success As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
		Dim mailmsg As New System.Net.Mail.MailMessage
		Dim strToAdresses = strTo.Split(CChar(";")).ToList()
		Dim strCCAdresses = strCC.Split(CChar(";")).ToList()
		Dim strbCCAdresses = strBCC.Split(CChar(";")).ToList()
		Dim _Cls As New SPProgUtility.ClsProgSettingPath


		Try
			Dim strEx_UserName As String = String.Empty
			Dim strEx_UserPW As String = String.Empty
			strEx_UserName = strUserName
			strEx_UserPW = strUserPW

#If DEBUG Then
			strSmtp = "smtpserver"
			strEx_UserName = "username"
			strEx_UserPW = "password"
			m_SmtpPort = 587
			m_EnableSSL = True

			strFrom = "info@domain.com"
			strToAdresses = New List(Of String) From {"user@domain.com"}
#End If

			With mailmsg
				.IsBodyHtml = True
				.To.Clear()
				.CC.Clear()
				.Bcc.Clear()
				.ReplyToList.Clear()

				.From = New MailAddress(strFrom)
				For Each toItem In strToAdresses
					If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
				Next
				For Each ccItem In strCCAdresses
					If ccItem.Trim <> String.Empty Then .CC.Add(New MailAddress(ccItem.Trim))
				Next
				For Each bccItem In strbCCAdresses
					If bccItem.Trim <> String.Empty Then .Bcc.Add(New MailAddress(bccItem.Trim))
				Next
				.ReplyToList.Add(.From)

				.Subject = strSubject.Trim()
				.Body = strBody.Trim()
				Try
					' die Imagevariablen umsetzen...
					Dim strImgVar1 As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates", "eMailImageVar1", "")
					Dim strImgValue1 As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates", "eMailImageValue1", "")
					Dim strImgVar2 As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates", "eMailImageVar2", "")
					Dim strImgValue2 As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates", "eMailImageValue2", "")
					Dim strImgVar3 As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates", "eMailImageVar3", "")
					Dim strImgValue3 As String = m_mandant.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates", "eMailImageValue3", "")

					If strImgVar1 <> String.Empty Then .Body = .Body.Replace(strImgVar1, String.Format("<{0}>", strImgValue1))
					If strImgVar2 <> String.Empty Then .Body = .Body.Replace(strImgVar2, String.Format("<{0}>", strImgValue2))
					If strImgVar3 <> String.Empty Then .Body = .Body.Replace(strImgVar3, String.Format("<{0}>", strImgValue3))

				Catch ex As Exception
					m_Logger.LogError(String.Format("GetMDProfilValue.{0}.{1}", strMethodeName, ex.Message))

				End Try

				If iPriority Then
					.Priority = MailPriority.High
				Else
					.Priority = MailPriority.Normal
				End If

				If Not aAttachmentFile Is Nothing AndAlso aAttachmentFile.Count > 0 Then
					For i As Integer = 0 To aAttachmentFile.Count - 1
						If File.Exists(aAttachmentFile(i)) Then

							Dim data As New Attachment(aAttachmentFile(i))
							' Send textMessage as part of the e-mail body.
							.Attachments.Add(data)
							Dim content As Mime.ContentType = data.ContentType
							content.MediaType = Mime.MediaTypeNames.Application.Pdf
							data.NameEncoding = DXEncoding.UTF8NoByteOrderMarks
						End If
					Next
				End If

				If Not aAttachmentFile Is Nothing Then
					If .Attachments.Count = 0 Then
						Dim attachmentResult = m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue("Sie senden ein Email ohne Anhang. Möchten Sie das Email trotzdem senden?")),
																															 m_Translate.GetSafeTranslationValue("Email-Anhang"), MessageBoxDefaultButton.Button2, MessageBoxIcon.Warning)
						m_Logger.LogInfo(String.Format("sending email without attachment: {0}", attachmentResult))
						If attachmentResult = False Then Return False
					End If
				End If

			End With

			m_Logger.LogInfo(String.Format("{0} | {1}", strUserName, strUserPW))

			Try
				If strEx_UserName <> String.Empty Then
					Try
						strEx_UserName = _Cls.DecryptBase64String(strEx_UserName)
						strEx_UserPW = _Cls.DecryptBase64String(strEx_UserPW)

						Dim mailClient As New System.Net.Mail.SmtpClient(strSmtp, m_SmtpPort)
						mailClient.EnableSsl = m_EnableSSL
						mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
						mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network

						m_Logger.LogInfo(String.Format("sending email with authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | smtpServer: {2} | smtpPort: {3}", m_EnableSSL, mailClient.DeliveryMethod.ToString, strSmtp, m_SmtpPort))
						mailClient.Send(mailmsg)
						mailClient.Dispose()

						success = True

					Catch ex As Exception
						Dim strMessage As String = String.Format(m_Translate.GetSafeTranslationValue("sending with authentication.Fehler:{5}To: {0}{5}From: {1}" &
																								"{5}SMTP-Server: {2} ({3}){5}Benutzer: {4} | {6}{5}{7}"),
																								strTo, strFrom, strSmtp, m_SmtpPort, strEx_UserName,
																								vbNewLine, strUserPW, ex.ToString)
						m_Logger.LogError(String.Format("Sendig.{0}.{1}", strMethodeName, strMessage))
						success = False
						m_UtilityUI.ShowErrorDialog(strMessage)

					End Try

				Else
					obj.Port = m_SmtpPort
					obj.EnableSsl = m_EnableSSL
					obj.Host = strSmtp

					Try
						m_Logger.LogInfo(String.Format("sending email with NO authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | smtpServer: {2} | smtpPort: {3}", m_EnableSSL, obj.DeliveryMethod.ToString, strSmtp, m_SmtpPort))
						obj.Send(mailmsg)

						success = True

					Catch ex As Exception
						Dim strMessage As String = String.Format("Fehler: {0}", ex.ToString)
						m_Logger.LogError(String.Format("Sendig.{0}.{1}", strMethodeName, strMessage))
						success = False

						m_UtilityUI.ShowErrorDialog(strMessage)

						Throw New Exception(strMessage)
					End Try

				End If


			Catch ex As Exception
				Dim strMessage As String = String.Format("Fehler: {0}", ex.ToString)
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, strMessage))
				success = False
				m_UtilityUI.ShowErrorDialog(strMessage)

				Throw New Exception(strMessage)

			Finally
				obj.Dispose()
				mailmsg.Attachments.Dispose()
				mailmsg.Dispose()

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			Dim strMessage = String.Format("Fehler: An: {0} From: {1} SMTP-Server: {2} ({3}) | Message: {4}", strTo, strFrom, strSmtp, m_SmtpPort, ex.ToString)
			success = False

			m_UtilityUI.ShowErrorDialog(strMessage)

		End Try

		Return success
	End Function

	Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click

		If Me.txt_MailBetreff.Text.Length <= 1 Or Me.txt_MailAn.Text.Length <= 6 Or Me.rtfContent.Text.Length <= 20 Then
			Dim strMsg As String = String.Format("Leere Felder. Kann nicht Ihre Nachricht senden. {0}", "Empfänger / Betreff / Nachrichtentext")
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg))

			Return

		Else
			Dim strAnhang As New List(Of String)
			Dim liFiles2Send As New List(Of String)
			Dim success As Boolean = True
			'Dim dataExists = gridDocuments.DataSource
			Dim dataExists = CType(gridDocuments.DataSource, BindingList(Of EmployeeDocumentData))
			Dim selectedData = dataExists.Where(Function(data) data.Checked = True).ToList()

			For Each item In selectedData

				Dim row = CType(item, EmployeeDocumentData)
				success = success AndAlso m_Utility.WriteFileBytes(row.EmployeeDocumentFullFileName, row.DocContent)
				If success Then liFiles2Send.Add(row.EmployeeDocumentFullFileName)

			Next
			strAnhang = liFiles2Send

			Dim strResult As Boolean = True
			strResult = strResult AndAlso SendProposeWitheMail(Me.txt_MailBetreff.Text,
																										 Me.rtfContent.HtmlText, Me.lblAbsender.Text,
																										 Me.txt_MailAn.Text, Me.txt_MailCc.Text, Me.txt_MailBcc.Text,
																										 strAnhang)
			If strResult Then
				If Me.txt_MailAn.Text <> Me.lblAbsender.Text Then
					Dim strLogValue As String = CreateLogToMailKontaktDb(ClsDataDetail.GetProposalKDNr,
																	 ClsDataDetail.GetProposalZHDNr,
																	 ClsDataDetail.GetProposalMANr,
																	 strAnhang.ToArray, True, Me.txt_MailAn.Text,
																	 Me.lblAbsender.Text,
																	 Me.txt_MailBetreff.Text,
																	 Me.rtfContent.HtmlText, True)

					If strLogValue.ToLower.Contains("erfolg") Then CreateLogToKontaktDb(ClsDataDetail.GetProposalKDNr,
						ClsDataDetail.GetProposalZHDNr, ClsDataDetail.GetProposalMANr,
						ClsDataDetail.GetProposalNr, ClsDataDetail.GetProposalVakNr, String.Empty)

					If Not strLogValue.ToLower.Contains("erfolg") Then m_UtilityUI.ShowErrorDialog(strLogValue)

				End If
				Dim strMessage = "Ihre Nachricht wurde erfolgreich gesendet."
				If Me.txt_MailAn.Text = Me.lblAbsender.Text Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(strMessage))

				Else
					Dim iBoxResult = True
					If iBoxResult Then Me.Close()
				End If

			Else
				Me.lblStatus.Caption = String.Format(m_Translate.GetSafeTranslationValue("Fehler in der Übermittlung Ihrer Nachricht."))
				m_UtilityUI.ShowErrorDialog(lblStatus.Caption)

			End If

		End If

	End Sub

	Private Sub XtraTabControl1_SelectedPageChanged(ByVal sender As Object,
																									ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) _
																									Handles XtraTabControl1.SelectedPageChanged
		If e.Page.TabControl.SelectedTabPageIndex = 1 Then
			Me.WebBrowser1.DocumentText = Me.rtfContent.HtmlText
		End If
	End Sub

	Private Sub btn_Translate_ItemClick(ByVal sender As Object,
																			ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btn_Translate.ItemClick
		_ClsDivFunc.TranslateSeletedTemplate(Me.rtfContent)
	End Sub

	Private Sub RibbonControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RibbonControl.MouseMove
		Me.lblStatus.Caption = m_Translate.GetSafeTranslationValue("Bereit")
	End Sub

	Private Sub OngvDocuments_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gvDocuments.MouseDown

		If e.Button = MouseButtons.Right Then
			Dim gv As DevExpress.XtraGrid.Views.Grid.GridView = sender
			gv.Focus()
			'Dim itemIndex As Integer = gridDocuments.IndexFromPoint(e.Location)
			'SelectItem(itemIndex)
			CreateAnhangPopupMenu(e.Location, 0) ' itemIndex)
		End If

	End Sub

	Sub SelectItem(ByVal itemindex As Integer)
		'lstMADoc.SelectedIndex = itemindex
	End Sub

	Private Sub biAnhang_ItemClick(ByVal sender As System.Object,
																 ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiAnhang.ItemClick
		Dim openDlg As New OpenFileDialog

		With openDlg
			.Filter = "PDF-Dokumente (*.PDF)|*.PDF"
			.FilterIndex = 1
			openDlg.Multiselect = True
			.InitialDirectory = If(String.IsNullOrEmpty(Me.m_SelectedFile2Import), m_path.GetSpSHomeFolder, Me.m_SelectedFile2Import)
			.Title = m_Translate.GetSafeTranslationValue("Datei einfügen")
			If .ShowDialog() = DialogResult.OK Then
				Dim aFiles As String() = openDlg.FileNames
				AddSelectedDocumentData(aFiles)
				'For i As Integer = 0 To aFiles.Length - 1
				'	Me.lstMADoc.Items.Add("")
				'	Me.lstMADoc.Items(Me.lstMADoc.Items.Count - 1).Value = aFiles(i).ToString
				'	Me.lstMADoc.Items(Me.lstMADoc.Items.Count - 1).Description = Path.GetFileName(aFiles(i).ToString)

				'	Me.lstMADoc.Items(Me.lstMADoc.Items.Count - 1).CheckState = CheckState.Checked
				'	m_liMADocData.Add(String.Format("{0}#{1}", aFiles(i).ToString, ""))
				'Next
				Me.m_SelectedFile2Import = Path.GetDirectoryName(openDlg.FileNames(0))
			End If

		End With

	End Sub

#Region "Kontakteinträge verwalten..."

	Function CreateLogToMailKontaktDb(ByVal lKDNr As Integer, ByVal lZHDNr As Integer?,
															ByVal lMANr As Integer, ByVal strFilename As String(),
															ByVal bSendAsHtml As Boolean, Optional ByVal MailTo As String = "",
															Optional ByVal MailFrom As String = "",
															Optional ByVal MailSubject As String = "",
															Optional ByVal MailBody As String = "",
															Optional ByVal bSendAsTest As Boolean = False) As String
		Dim strResult As String = "Erfolg"
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_Kontakte (KDNr, KDZNr, RecNr, MANr, Message_ID, eMail_To, "
		sMailSql &= "eMail_From, eMail_Subject, eMail_Body, eMail_smtp, AsHtml, AsTelefax, Customer_ID, "
		sMailSql &= "CreatedOn, CreatedFrom) Values (@KDNr, @ZHDNr, @RecNr, @MANr, @Message_ID, @eMailTo, @eMailFrom, "
		sMailSql &= "@eMailsubject, @eMailbody, @eMailSmtp, @SendAsHtml, 0, @Customer_ID, @KontaktDate, @USName) "

		sMailSql = String.Format(sMailSql, GetMailDbName)
		Dim lNewRecNr As Integer

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()
			lNewRecNr = GetNeweMailKontaktNr()
			_iMail_ID = lNewRecNr

			cmd.CommandType = CommandType.Text
			cmd.CommandText = sMailSql

			param = cmd.Parameters.AddWithValue("@KDNr", lKDNr)
			param = cmd.Parameters.AddWithValue("@ZHDNr", If(lZHDNr.HasValue, lZHDNr, 0))
			param = cmd.Parameters.AddWithValue("@RecNr", lNewRecNr)

			param = cmd.Parameters.AddWithValue("@MANr", lMANr)

			param = cmd.Parameters.AddWithValue("@Message_ID", ClsDataDetail.GetProposalNr)
			param = cmd.Parameters.AddWithValue("@eMailTo", MailTo)
			param = cmd.Parameters.AddWithValue("@eMailFrom", MailFrom)
			param = cmd.Parameters.AddWithValue("@eMailsubject", MailSubject)
			param = cmd.Parameters.AddWithValue("@eMailbody", MailBody)
			param = cmd.Parameters.AddWithValue("@eMailSmtp", m_SmtpServer)
			param = cmd.Parameters.AddWithValue("@SendAsHtml", bSendAsHtml)

			param = cmd.Parameters.AddWithValue("@Customer_ID", ClsDataDetail.MDData.MDGuid)
			param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
			param = cmd.Parameters.AddWithValue("@USName", ClsDataDetail.UserData.UserFullName)

			cmd.Connection = Conn
			cmd.ExecuteNonQuery()

			' Binaryfile in die Datenbank
			If strFilename.Length > 0 Then 'And Not ClsDataDetail.IsAttachedFileInd Then
				strResult = InsertBinaryToMailDb(lNewRecNr, strFilename, MailTo, MailFrom, MailSubject)
			End If

		Catch ex As Exception
			strResult = String.Format("***Fehler (CreateLogToMailKontaktDb_0): {0}", ex.Message)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für CreateLogToMailKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Return strResult
	End Function

	Function InsertBinaryToMailDb(ByVal lRecNr As Integer, ByVal strFilename As String(),
												Optional ByVal MailTo As String = "", Optional ByVal MailFrom As String = "",
												Optional ByVal MailSubject As String = "") As String
		Dim strResult As String = "Erfolg"
		Dim Time_1 As Double = System.Environment.TickCount

		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Dim sMailSql As String = "Insert Into [{0}].dbo.Mail_FileScan (RecNr, Message_ID, eMail_To, eMail_From, eMail_Subject, "
		sMailSql &= "ScanFile, Filename, Customer_ID, CreatedOn, CreatedFrom) Values (@RecNr, @Message_ID, @eMailTo, @eMailFrom, "
		sMailSql &= "@eMailsubject, @BinaryFile, @FileName, @Customer_ID, @KontaktDate, @USName)"
		sMailSql = String.Format(sMailSql, GetMailDbName)

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter
		Dim bAllreadyOpen As Boolean = False

		Try
			Conn.Open()
			For i As Integer = 0 To strFilename.Length - 1
				If strFilename(i) <> String.Empty Then
					Dim fi As New System.IO.FileInfo(strFilename(i))
					Dim fs As System.IO.FileStream = fi.OpenRead
					Dim CheckFile As New FileInfo(strFilename(i))

					Dim lBytes As Integer = CInt(fs.Length)
					Dim myImage(lBytes) As Byte

					fs.Read(myImage, 0, lBytes)
					fs.Close()
					fs.Dispose()

					Try
						cmd.CommandType = CommandType.Text
						cmd.CommandText = sMailSql

						param = cmd.Parameters.AddWithValue("@RecNr", lRecNr)
						param = cmd.Parameters.AddWithValue("@Message_ID", ClsDataDetail.GetProposalNr)
						param = cmd.Parameters.AddWithValue("@eMailTo", MailTo)
						param = cmd.Parameters.AddWithValue("@eMailFrom", MailFrom)
						param = cmd.Parameters.AddWithValue("@eMailsubject", MailSubject)
						param = cmd.Parameters.AddWithValue("@BinaryFile", myImage)
						param = cmd.Parameters.AddWithValue("@FileName", CheckFile.Name)
						param = cmd.Parameters.AddWithValue("@Customer_ID", m_mandant.GetMDGuid(ClsDataDetail.ProgSettingData.SelectedMDNr))
						param = cmd.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
						param = cmd.Parameters.AddWithValue("@USName", ClsDataDetail.UserData.UserFullName)

						If Not bAllreadyOpen Then
							cmd.Connection = Conn
							bAllreadyOpen = True
						End If

						cmd.ExecuteNonQuery()
						cmd.Parameters.Clear()

					Catch ex As Exception
						strResult = String.Format("***Fehler (InsertBinaryToMailDb_1): {0}", ex.Message)

					End Try
				End If
			Next

		Catch ex As Exception
			strResult = String.Format("***Fehler (InsertBinaryToMailDb_2): {0}", ex.Message)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für InsertBinaryToMailDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Return strResult
	End Function








#Region "Save ContactData"

	Private Sub CreateLogToKontaktDb(ByVal m_CustomerNumber As Integer, ByVal m_ResponsiblePersonRecordNumber As Integer?, ByVal m_EmployeeNumber As Integer?,
				 ByVal m_ProposeNumber As Integer, ByVal m_VacancyNumber As Integer?,
				 ByVal strProposeBez As String)
		Dim contactData As ResponsiblePersonAssignedContactData = Nothing
		Dim m_CurrentContactRecordNumber As Integer?

		Dim dt = DateTime.Now
		If Not m_CurrentContactRecordNumber.HasValue Then
			contactData = New ResponsiblePersonAssignedContactData With {.CustomerNumber = m_CustomerNumber,
																																		 .ResponsiblePersonNumber = m_ResponsiblePersonRecordNumber,
																																		 .CreatedOn = dt,
																																		 .CreatedFrom = ClsDataDetail.UserData.UserFullName}
		Else

			contactData = m_CustomerDatabaseAccess.LoadAssignedContactDataOfResponsiblePerson(m_CustomerNumber, m_ResponsiblePersonRecordNumber, m_CurrentContactRecordNumber)

			If contactData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht gespeichert werden."))
				Return
			End If

		End If

		contactData.CustomerNumber = m_CustomerNumber
		contactData.ContactDate = Now.ToString("G")
		contactData.ResponsiblePersonNumber = If(Not m_ResponsiblePersonRecordNumber.HasValue, 0, m_ResponsiblePersonRecordNumber.Value)
		contactData.ContactType1 = "Einzelmail"
		contactData.ContactPeriodString = m_Translate.GetSafeTranslationValue("Versand per EMail (Vorschlag)")
		contactData.ContactsString = String.Format(m_Translate.GetSafeTranslationValue("Vorlage: {1} wurde gesendet.{0}{2}"), vbNewLine, m_TemplateData.itemBez, Me.rtfContent.Text)
		contactData.ContactImportant = False
		contactData.ContactFinished = False
		contactData.MANr = m_EmployeeNumber
		contactData.VacancyNumber = m_VacancyNumber
		contactData.ProposeNr = m_ProposeNumber
		contactData.ESNr = Nothing

		contactData.ChangedFrom = ClsDataDetail.UserData.UserFullName
		contactData.ChangedOn = dt
		contactData.UsNr = ClsDataDetail.UserData.UserNr

		Dim isNewContact = (contactData.ID = 0)

		Dim success As Boolean = True


		' Insert or update contact
		If isNewContact Then
			contactData.CreatedUserNumber = ClsDataDetail.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(contactData)

			If success Then
				m_CurrentContactRecordNumber = contactData.RecordNumber
			End If

		Else
			contactData.ChangedUserNumber = ClsDataDetail.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.UpdateResponsiblePersonAssignedContactData(contactData)
		End If


		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht gespeichert werden."))

		Else

			' ---Copy contact---

			If Not m_EmployeeNumber Is Nothing Then
				' The user has selected a list of employees -> copy the contact for all of them.
				success = success AndAlso CopyContactForEmployees(contactData, Nothing, New Integer() {m_EmployeeNumber})

			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht kopiert werden."))
			End If

			' --end of copy contact ---

			' Notifiy system about changed contact
			Dim hub = MessageService.Instance.Hub
			Dim customerContactHasChangedMsg As New CustomerContactDataHasChanged(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, m_CustomerNumber, m_CurrentContactRecordNumber)
			hub.Publish(customerContactHasChangedMsg)

		End If


	End Sub

	''' <summary>
	''' Copies the contact for employees.
	''' </summary>
	''' <param name="contactData">The contact data to copy.</param>
	''' <param name="employeeNumbers">The employee numbers.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function CopyContactForEmployees(ByVal contactData As ResponsiblePersonAssignedContactData,
																					 ByVal contactDocumentID As Integer?,
																					 ByVal employeeNumbers As Integer()) As Boolean

		Dim success As Boolean = True

		Dim dt = DateTime.Now

		' Load dependent employee contact list
		Dim dependentEmployeeContactList = m_CustomerDatabaseAccess.LoadCustomerDependentEmployeeContactData(contactData.ID)

		' Copy contact for each employee.
		For Each employeeNumber In employeeNumbers

			' Create a shallow copy
			Dim copyOfContactData As EmployeeContactData

			Dim employeeNr As Integer = employeeNumber
			Dim dependentEmployeeContact = dependentEmployeeContactList.Where(Function(data) data.EmployeeNumber = employeeNr).FirstOrDefault()

			If dependentEmployeeContact Is Nothing Then
				' Its a new contact
				copyOfContactData = New EmployeeContactData
				copyOfContactData.ID = Nothing
				copyOfContactData.RecordNumber = Nothing
			Else
				' Update existing contact
				copyOfContactData = m_EmployeeDatabaseAccess.LoadEmployeeContact(employeeNumber, dependentEmployeeContact.EmployeeContactRecordNumber)

				If copyOfContactData Is Nothing Then
					Return False
				End If
			End If

			' Overwrite values
			copyOfContactData.EmployeeNumber = employeeNumber
			copyOfContactData.ContactsString = contactData.ContactsString
			copyOfContactData.ContactType1 = contactData.ContactType1
			copyOfContactData.ContactType2 = contactData.ContactType2
			copyOfContactData.ContactDate = contactData.ContactDate
			copyOfContactData.ContactPeriodString = contactData.ContactPeriodString
			copyOfContactData.ContactImportant = contactData.ContactImportant
			copyOfContactData.ContactFinished = contactData.ContactFinished
			copyOfContactData.CreatedOn = dt
			copyOfContactData.CreatedFrom = ClsDataDetail.UserData.UserFullName
			copyOfContactData.ProposeNr = contactData.ProposeNr
			copyOfContactData.VacancyNumber = contactData.VacancyNumber
			copyOfContactData.OfNumber = contactData.OfNumber
			copyOfContactData.Mail_ID = contactData.Mail_ID
			copyOfContactData.TaskRecNr = contactData.TaskRecNr
			copyOfContactData.UsNr = contactData.UsNr
			copyOfContactData.ESNr = contactData.ESNr
			copyOfContactData.CustomerNumber = ClsDataDetail.GetProposalKDNr
			copyOfContactData.CustomerContactRecId = contactData.ID
			copyOfContactData.KontaktDocID = contactData.KontaktDocID

			copyOfContactData.ChangedFrom = ClsDataDetail.UserData.UserFullName
			copyOfContactData.ChangedOn = dt


			' Save the contact

			If copyOfContactData.ID > 0 Then
				copyOfContactData.ChangedUserNumber = ClsDataDetail.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeContact(copyOfContactData)
			Else
				copyOfContactData.CreatedUserNumber = ClsDataDetail.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(copyOfContactData)
			End If

			If Not success Then
				Exit For
			End If

		Next

		Return success
	End Function


#End Region



	Function CreateLogToKDKontaktDb() As String
		Dim strResult As String = "Erfolg"
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim strKontaktSql As String = _ClsDivFunc.GetSQLString4KontaktDb(True)

		Dim lNewRecNr As Integer
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter
		Try
			Conn.Open()
			cmd.CommandType = CommandType.Text
			cmd.CommandText = strKontaktSql

			lNewRecNr = GetNewKontaktNr(ClsDataDetail.GetProposalKDNr,
																	ClsDataDetail.GetProposalZHDNr,
																	0)

			Dim rKontaktrec As New SqlDataAdapter()

			param = cmd.Parameters.AddWithValue("@KDNr", ClsDataDetail.GetProposalKDNr)
			param = cmd.Parameters.AddWithValue("@KDZNr", If(ClsDataDetail.GetProposalZHDNr.HasValue, ClsDataDetail.GetProposalZHDNr, 0))

			param = cmd.Parameters.AddWithValue("@KontaktDate", Format(Now, "g"))
			param = cmd.Parameters.AddWithValue("@Kontakte", String.Format(m_Translate.GetSafeTranslationValue("Vorlage: {0} wurde gesendet."), m_TemplateData.itemBez))
			param = cmd.Parameters.AddWithValue("@RecNr", lNewRecNr)

			param = cmd.Parameters.AddWithValue("@KontaktType1", "Einzelmail")
			param = cmd.Parameters.AddWithValue("@KontaktType2", "1")
			param = cmd.Parameters.AddWithValue("@KontaktDauer", m_Translate.GetSafeTranslationValue("Versand per EMail (Vorschlag)"))
			param = cmd.Parameters.AddWithValue("@KontaktWichtig", "1")
			param = cmd.Parameters.AddWithValue("@KontaktErledigt", "0")

			param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetProposalMANr)
			param = cmd.Parameters.AddWithValue("@ProposeNr", ClsDataDetail.GetProposalNr)
			param = cmd.Parameters.AddWithValue("@VakNr", If(ClsDataDetail.GetProposalVakNr.HasValue, ClsDataDetail.GetProposalVakNr, 0))
			param = cmd.Parameters.AddWithValue("@OfNr", 0)
			param = cmd.Parameters.AddWithValue("@Mail_ID", _iMail_ID)

			param = cmd.Parameters.AddWithValue("@CreatedOn", Format(Now, "g"))
			param = cmd.Parameters.AddWithValue("@CreatedFrom", ClsDataDetail.UserData.UserFullName)

			cmd.Connection = Conn
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			strResult = String.Format("***Fehler (CreateLogToKDKontaktDb_0): {0}", ex.Message)

		End Try

		Conn.Close()

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für CreateLogToKDKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Return strResult
	End Function

	Function CreateLogToMAKontaktDb() As String
		Dim strResult As String = "Erfolg"
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim strKontaktSql As String = _ClsDivFunc.GetSQLString4KontaktDb(False)

		Dim lNewRecNr As Integer
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter
		Try
			Conn.Open()
			cmd.CommandType = CommandType.Text
			cmd.CommandText = strKontaktSql

			lNewRecNr = GetNewKontaktNr(0, 0, ClsDataDetail.GetProposalMANr)

			param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetProposalMANr)

			param = cmd.Parameters.AddWithValue("@KontaktDate", Format(Now, "g"))
			param = cmd.Parameters.AddWithValue("@Kontakte", m_Translate.GetSafeTranslationValue("Vorschlag wurde gesendet."))
			param = cmd.Parameters.AddWithValue("@RecNr", lNewRecNr)

			param = cmd.Parameters.AddWithValue("@KontaktType1", "Information")
			param = cmd.Parameters.AddWithValue("@KontaktType2", "1")
			param = cmd.Parameters.AddWithValue("@KontaktDauer", m_Translate.GetSafeTranslationValue("Versand per EMail"))
			param = cmd.Parameters.AddWithValue("@KontaktWichtig", "1")
			param = cmd.Parameters.AddWithValue("@KontaktErledigt", "0")

			param = cmd.Parameters.AddWithValue("@ProposeNr", ClsDataDetail.GetProposalNr)
			param = cmd.Parameters.AddWithValue("@VakNr", If(ClsDataDetail.GetProposalVakNr.HasValue, ClsDataDetail.GetProposalVakNr, 0))
			param = cmd.Parameters.AddWithValue("@OfNr", 0)
			param = cmd.Parameters.AddWithValue("@Mail_ID", _iMail_ID)

			param = cmd.Parameters.AddWithValue("@CreatedOn", Format(Now, "g"))
			param = cmd.Parameters.AddWithValue("@CreatedFrom", ClsDataDetail.UserData.UserFullName)


			cmd.Connection = Conn
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			strResult = String.Format("***Fehler (CreateLogToMAKontaktDb_0): {0}", ex.Message)

		End Try
		Conn.Close()

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für CreateLogToMAKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Return strResult
	End Function

	Function GetMailDbName() As String
		Dim strDatabaseName As String = "Sputnik_MailDb"
		'strDatabaseName = _Clsreg.GetINIString(_ClsProgSetting.GetMDIniFile(), "Mailing", "Mail-Database", strDatabaseName)

		Return strDatabaseName
	End Function

	Function GetNeweMailKontaktNr() As Integer
		Dim lRecNr As Integer = 1
		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Conn.Open()

		Try
			Dim sSql As String = "Select Top 1 RecNr From [{0}].dbo.Mail_Kontakte Order By RecNr Desc"
			sSql = String.Format(sSql, GetMailDbName)
			Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
			Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader

			rTemprec.Read()
			If rTemprec.HasRows Then
				lRecNr = CInt(rTemprec("RecNr").ToString) + 1
			Else
				lRecNr = 1
			End If
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()

		End Try

		Return lRecNr
	End Function

#End Region

	Private Sub sbtnSend2Sortedclst_Click(sender As System.Object, e As System.EventArgs) Handles btnSend2SortedFiles.Click

		Dim selectedDocument = SelectedRecordToSelect ' lstAllFiles.SelectedItem

		If m_SortedEmployeeDocumentData Is Nothing Then
			If Not selectedDocument Is Nothing Then
				m_SortedEmployeeDocumentData.Add(selectedDocument)
			End If

		Else
			If Not selectedDocument Is Nothing AndAlso Not m_SortedEmployeeDocumentData.Any(Function(data) data.RecGuid = selectedDocument.RecGuid) Then
				m_SortedEmployeeDocumentData.Add(selectedDocument)
			End If

		End If
		grdSelectedFile.DataSource = m_SortedEmployeeDocumentData



		Dim existSortedData = grdSelectedFile.DataSource
		Dim listDataSource As BindingList(Of EmployeeDocumentData) = New BindingList(Of EmployeeDocumentData)
		For Each itm In grdFileToSelect.DataSource

			If Not m_SortedEmployeeDocumentData.Any(Function(data) data.RecGuid = itm.recguid) Then

				listDataSource.Add(itm)

			End If
		Next

		grdFileToSelect.DataSource = listDataSource
		Me.sbtnCreateOnePDF.Enabled = m_SortedEmployeeDocumentData.Count > 0

	End Sub

	Private Sub sbtnLoadclst_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadFilesToSelect.Click

		grdSelectedFile.DataSource = Nothing
		grdFileToSelect.DataSource = Nothing
		m_SortedEmployeeDocumentData.Clear()

		grdFileToSelect.DataSource = m_EmployeeDocumentData

		Me.sbtnCreateOnePDF.Enabled = False

	End Sub

	Private Sub sbtnCreateOnePDF_Click(sender As System.Object, e As System.EventArgs) Handles sbtnCreateOnePDF.Click
		If m_SortedEmployeeDocumentData.Count = 0 Then Exit Sub
		Dim success As Boolean = True
		Dim liFiles2Merge As New List(Of String)
		Dim finalFielname As String = Me.beFilename2Zip.Text.Replace(":", "").Replace("*", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("|", "")

		If String.IsNullOrWhiteSpace(finalFielname) Then
			Dim aMAName As String() = m_liMAData(0).Split(CChar("#"))
			Dim strZipfile2Send As String = String.Format(m_Translate.GetSafeTranslationValue("Unterlagen von {0} {1}.{2}"), aMAName(1), aMAName(2), "PDF")
			finalFielname = strZipfile2Send
		End If

		For Each itm In m_SortedEmployeeDocumentData
			success = success AndAlso m_Utility.WriteFileBytes(itm.EmployeeDocumentFullFileName, itm.DocContent)
			If success Then liFiles2Merge.Add(itm.EmployeeDocumentFullFileName)
		Next

		If liFiles2Merge.Count = 0 OrElse liFiles2Merge(0) = String.Empty Then Return
		Dim strTempFielname As String = String.Empty
		Try
			strTempFielname = String.Format("{0}{1}{2}",
																			 If(finalFielname.Contains("\"), "", m_path.GetSpSMAHomeFolder),
																			 finalFielname,
																			 If(finalFielname.ToLower.EndsWith(".pdf"), "", ".PDF"))
			If File.Exists(strTempFielname) Then File.Delete(strTempFielname)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Die Datei konnte nicht gelöscht werden.{0}{1}{0}Möglicherweise haben Sie keine Rechte für Dateioperationen.{0}{2}"), vbNewLine, strTempFielname, ex.ToString))
			m_Logger.LogError(String.Format("{0}", ex.ToString))

			Return
		End Try

		pcc_1Filename.HidePopup()
		If liFiles2Merge.Count > 1 Then
			Dim continueWithMerge As Boolean = True
			Try
				Dim pdfDocument As New PdfDocumentProcessor()
				pdfDocument.LoadDocument(liFiles2Merge(0).ToString)

				For i As Integer = 1 To liFiles2Merge.Count - 1
					pdfDocument.AppendDocument(liFiles2Merge(i))
					pdfDocument.SaveDocument(liFiles2Merge(0).ToString)
				Next
				pdfDocument.CloseDocument()
				File.Copy(liFiles2Merge(0).ToString, strTempFielname, True)
				File.Delete(liFiles2Merge(0).ToString)

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Die Dateien können nicht zusammengefügt werden.{0}{1}"), vbNewLine, ex.ToString))

				m_Logger.LogError(String.Format("{0}", ex.ToString))
				continueWithMerge = False

			End Try

			If continueWithMerge Then
				Dim strMessage As String = "Die Datei wurde erfolgreich erstellt und wird automatisch als Mail-Anhang versendet."
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(strMessage))

				Dim data = New BindingList(Of EmployeeDocumentData)
				data.Add(New EmployeeDocumentData With {.Bezeichnung = Path.GetFileNameWithoutExtension(strTempFielname),
																								.DocContent = m_Utility.LoadFileBytes(strTempFielname),
																								.ScanExtension = "PDF",
																								.RecGuid = Guid.NewGuid.ToString,
																								.Checked = True})
				gridDocuments.DataSource = data

			Else
				Dim strMessage As String = String.Format("Fehler: Möglicherweise sind die Dateien fehlerhaft!")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMessage))

			End If

		Else
			Dim strMessage As String = "Sie müssen mehr als eine Datei auswählen."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMessage))
		End If

		Me.beFilename2Zip.Text = finalFielname

	End Sub

	Private Sub ShowMergeFiles()
		Dim barmgm As New BarManager

		Dim aMAName As String() = m_liMAData(0).Split(CChar("#"))
		Dim strZipfile2Send As String = String.Format(m_Translate.GetSafeTranslationValue("Unterlagen von {0} {1}.{2}"), aMAName(1), aMAName(2), ".PDF")

		Me.beFilename2Zip.Text = String.Empty

		LoadAllFileForSelect()

		Me.pcc_1Filename.Manager = barmgm
		Me.pcc_1Filename.ShowPopup(Control.MousePosition)
		Me.sbtnCreateOnePDF.Enabled = Me.gvSelectedFile.RowCount > 0

	End Sub

	Private Sub LoadAllFileForSelect()

		ResetDocumentToSelectGrid()
		'ResetAllFileTestLST()
		ResetSelectedDocumentGrid()

		Dim data = gridDocuments.DataSource
		grdFileToSelect.DataSource = data

	End Sub

	Private Sub grpAnhaenge_CustomButtonClick(sender As Object, e As Docking2010.BaseButtonEventArgs) Handles grpAnhaenge.CustomButtonClick

		m_SortedEmployeeDocumentData.Clear()

		If e.Button.Properties.GroupIndex = 0 Then
			LoadFilteredDocumentData()
		ElseIf e.Button.Properties.GroupIndex = 1 Then
			pcc_1Filename.HidePopup()
			ShowMergeFiles()
		End If

	End Sub

	Private Sub frmPMail_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

	End Sub


#Region "Helpers"


#End Region


End Class
