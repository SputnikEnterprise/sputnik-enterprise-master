
Imports System.Reflection.Assembly
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Imports System.Xml
Imports System.Text
Imports System.Xml.Serialization
Imports System.Xml.XPath

Imports System.Xml.Linq
Imports DevExpress.XtraEditors
Imports DevExpress.Skins

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.Logging
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports DevExpress.XtraEditors.Controls

Public Class frmTemplateDetail
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Constants"


	Private Const MANDANT_XML_SETTING_SONSTIGES_NOTIFICATION As String = "MD_{0}/Sonstiges/notificationintervalperiode"
	Private Const MANDANT_XML_SETTING_SONSTIGES_NOTIFICATION_REPORT As String = "MD_{0}/Sonstiges/notificationintervalperiodeforreport"
	Private Const MANDANT_XML_SETTING_WOS_VACANCY_GUID As String = "MD_{0}/Export/Vak_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_JOBCHANNELPRIORITY As String = "MD_{0}/Export/jobchannelpriority"
	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_VER_GUID As String = "MD_{0}/Export/Ver_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_PROPOSE_GUID As String = "MD_{0}/Export/sendproposeattachmenttowos"

	Private Const MANDANT_XML_SETTING_COCKPIT_EMAIL_TEMPLATE As String = "MD_{0}/Templates/cockpit-email-template"
	Private Const MANDANT_XML_SETTING_COCKPIT_URL As String = "MD_{0}/Templates/cockpit-url"
	Private Const MANDANT_XML_SETTING_COCKPIT_PICTURE As String = "MD_{0}/Templates/cockpit-picture"

	Private Const MANDANT_XML_SETTING_MAILING_URL As String = "MD_{0}/Mailing"

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
	Private Const MANDANT_XML_SETTING_SPUTNIK_LICENCING_URI As String = "MD_{0}/Licencing"


#End Region

#Region "Private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Thre translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _ClsReg As New SPProgUtility.ClsDivReg
	'Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strLinkName As String = ""
	Private strLinkCaption As String = ""

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	Private strMDIniFile As String

	Private m_md As Mandant
	Private m_utilitySP As SPProgUtility.MainUtilities.Utilities
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	''' <summary>
	''' Mandant Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml
	Private m_FormSettingsXml As SettingsXml

	Private Property MD_XML_MAIN_KEY As String

	Private mandantNumber As Integer
	Private userNumber As Integer
	Private m_licencing_uri As String


#End Region


	Public ReadOnly Property GetMDPath() As String
		Get
			Return m_md.GetSelectedMDYearPath(mandantNumber, Now.Year)
		End Get
	End Property

	Public ReadOnly Property GetMDIniFile() As String
		Get
			Return String.Format("{0}{1}", GetMDPath, "Programm.dat")
		End Get
	End Property

	Public ReadOnly Property UserXMLFileName() As String
		Get
			Return m_md.GetSelectedMDUserProfileXMLFilename(mandantNumber, userNumber)
		End Get
	End Property

	Public ReadOnly Property MandantXMLFileName() As String
		Get
			Return m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year)
		End Get
	End Property

	Public ReadOnly Property MDFormXMLFileName() As String
		Get
			Return m_md.GetSelectedMDFormDataXMLFilename(mandantNumber)
		End Get
	End Property




#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()


		m_md = New Mandant
		m_utilitySP = New Utilities
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

		m_InitializationData = _setting ' CreateInitialData(mdnr, usnr)
		mandantNumber = m_InitializationData.MDData.MDNr
		userNumber = m_InitializationData.UserData.UserNr

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		strMDIniFile = GetMDIniFile
		m_licencing_uri = String.Format(MANDANT_XML_SETTING_SPUTNIK_LICENCING_URI, m_InitializationData.MDData.MDNr)

		ModulConstants.MDData = m_InitializationData.MDData ' ModulConstants.SelectedMDData(mandantNumber)
		ModulConstants.UserData = m_InitializationData.UserData ' ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, userNumber)

		ModulConstants.ProsonalizedData = m_InitializationData.ProsonalizedData ' ModulConstants.ProsonalizedValues
		ModulConstants.TranslationData = m_InitializationData.TranslationData ' ModulConstants.TranslationValues

		Me.bsiInfo_1.Caption = ModulConstants.MDData.MDMainPath
		Me.bsiInfo_2.Caption = m_md.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year)
		Me.bsiInfo_3.Caption = m_md.GetSelectedMDFormDataXMLFilename(ModulConstants.MDData.MDNr)
		Me.bsiInfo_4.Caption = m_md.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		Try
			m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year))
			Try
				'm_FormSettingsXml = New SettingsXml(m_md.GetSelectedMDFormDataXMLFilename(mandantNumber))
			Catch ex As Exception

			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		MD_XML_MAIN_KEY = String.Format("MD_{0}/", mandantNumber)

		Me.nvLayout.Visible = False


		Me.pNavbar.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.xscMain.Dock = DockStyle.Fill

		Me.xscMain.Controls.Add(Me.pLizenzen)
		Me.xscMain.Controls.Add(Me.pLColor)
		Me.xscMain.Controls.Add(Me.pMailFax)
		'Me.xscMain.Controls.Add(Me.pFieldBez)
		Me.xscMain.Controls.Add(Me.pJobplattforms)

		Me.xscMain.Controls.Add(Me.pCom_WOS)
		Me.xscMain.Controls.Add(Me.pGlobal)

		'Me.xscMain.Controls.Add(Me.pMetro_0)
		Me.xscMain.Controls.Add(Me.pMail_Tpl)

		Reset()
		LoadDropDownData()


	End Sub

#End Region


	Private Sub frmTemplateDetail_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidth = Me.Width
				My.Settings.ifrmHeight = Me.Height


				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmTemplateDetail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			If My.Settings.frmLocation <> String.Empty Then
				Me.Width = Math.Max(My.Settings.ifrmWidth, Me.MinimumSize.Width)
				Me.Height = Math.Max(My.Settings.ifrmHeight, Me.MinimumSize.Height)
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		LoadFormData()
		If ModulConstants.UserData.UserNr <> 1 Then Me.xtabeCall.PageVisible = False
		If ModulConstants.UserData.UserNr <> 1 Then Me.nviLicense.Visible = False

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And ModulConstants.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub


	Private Sub Reset()

		ResetAutoFilterConditionNrDropDown()
		ResetAutoFilterConditionDateDropDown()
		ResetSmtpDeliveryMethodDropDown()

	End Sub

	''' <summary>
	''' Resets the autofilterconditioinNr drop down data.
	''' </summary>
	Private Sub ResetAutoFilterConditionNrDropDown()

		cboautofilterconditionnr.Properties.DisplayMember = "DisplayText"
		cboautofilterconditionnr.Properties.ValueMember = "Value"

		Dim columns = cboautofilterconditionnr.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		cboautofilterconditionnr.Properties.ShowHeader = False
		cboautofilterconditionnr.Properties.ShowFooter = False
		cboautofilterconditionnr.Properties.DropDownRows = 10
		cboautofilterconditionnr.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cboautofilterconditionnr.Properties.SearchMode = SearchMode.AutoComplete
		cboautofilterconditionnr.Properties.AutoSearchColumnIndex = 0
		cboautofilterconditionnr.Properties.NullText = String.Empty

		cboautofilterconditionnr.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the autofilterconditioinDate drop down data.
	''' </summary>
	Private Sub ResetAutoFilterConditionDateDropDown()

		cboAutoFilterConditionDate.Properties.DisplayMember = "DisplayText"
		cboAutoFilterConditionDate.Properties.ValueMember = "Value"

		Dim columns = cboAutoFilterConditionDate.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		cboAutoFilterConditionDate.Properties.ShowHeader = False
		cboAutoFilterConditionDate.Properties.ShowFooter = False
		cboAutoFilterConditionDate.Properties.DropDownRows = 10
		cboAutoFilterConditionDate.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cboAutoFilterConditionDate.Properties.SearchMode = SearchMode.AutoComplete
		cboAutoFilterConditionDate.Properties.AutoSearchColumnIndex = 0
		cboAutoFilterConditionDate.Properties.NullText = String.Empty

		cboAutoFilterConditionDate.EditValue = Nothing

	End Sub

	Private Sub ResetSmtpDeliveryMethodDropDown()

		cboSmtpDeliveryMethod.Properties.DisplayMember = "DisplayText"
		cboSmtpDeliveryMethod.Properties.ValueMember = "Value"

		Dim columns = cboSmtpDeliveryMethod.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		cboSmtpDeliveryMethod.Properties.ShowHeader = False
		cboSmtpDeliveryMethod.Properties.ShowFooter = False
		cboSmtpDeliveryMethod.Properties.DropDownRows = 10
		cboSmtpDeliveryMethod.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cboSmtpDeliveryMethod.Properties.SearchMode = SearchMode.AutoComplete
		cboSmtpDeliveryMethod.Properties.AutoSearchColumnIndex = 0
		cboSmtpDeliveryMethod.Properties.NullText = String.Empty

		cboSmtpDeliveryMethod.EditValue = Nothing

	End Sub


	Private Sub LoadDropDownData()
		LoadAutoFilterConditionNrInDropDownData()
		LoadAutoFilterConditionDateInDropDownData()
		LoadSmtpDeliveryMethodDropDownData()
	End Sub

	''' <summary>
	''' Loads the autofilterconditionNr drop down data.
	''' </summary>
	Private Function LoadAutoFilterConditionNrInDropDownData() As Boolean
		Dim data = New List(Of AutoFilterConditionViewData) From {
			New AutoFilterConditionViewData With {.DisplayText = "Default", .Value = 0},
			New AutoFilterConditionViewData With {.DisplayText = "Like", .Value = 1},
			New AutoFilterConditionViewData With {.DisplayText = "Equal", .Value = 2},
			New AutoFilterConditionViewData With {.DisplayText = "Contains", .Value = 3},
			New AutoFilterConditionViewData With {.DisplayText = "Begins with", .Value = 4}
		}
		cboautofilterconditionnr.Properties.DataSource = data
		cboautofilterconditionnr.Properties.ForceInitialize()

		Return True
	End Function

	''' <summary>
	''' Loads the autofilterconditionDate drop down data.
	''' </summary>
	Private Function LoadAutoFilterConditionDateInDropDownData() As Boolean
		Dim data = New List(Of AutoFilterConditionViewData) From {
			New AutoFilterConditionViewData With {.DisplayText = "Default", .Value = 0},
			New AutoFilterConditionViewData With {.DisplayText = "Like", .Value = 1},
			New AutoFilterConditionViewData With {.DisplayText = "Equal", .Value = 2},
			New AutoFilterConditionViewData With {.DisplayText = "Contains", .Value = 3},
			New AutoFilterConditionViewData With {.DisplayText = "Begins with", .Value = 4}
		}
		cboAutoFilterConditionDate.Properties.DataSource = data
		cboAutoFilterConditionDate.Properties.ForceInitialize()

		Return True
	End Function

	Private Function LoadSmtpDeliveryMethodDropDownData() As Boolean
		Dim data = New List(Of SelectionViewData) From {
			New SelectionViewData With {.DisplayText = "SmtpDeliveryMethod.Network", .Value = 1},
			New SelectionViewData With {.DisplayText = "SmtpDeliveryMethod.PickupDirectoryFromIis", .Value = 2},
			New SelectionViewData With {.DisplayText = "SmtpDeliveryMethod.SpecifiedPickupDirectory", .Value = 3}
		}
		cboSmtpDeliveryMethod.Properties.DataSource = data
		cboSmtpDeliveryMethod.Properties.ForceInitialize()

		Return True
	End Function

	Sub LoadFormData()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strQuery As String = String.Empty
		Dim strXMLProfileName As String = MDFormXMLFileName

		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Try
			Me.txtLPfad_0.Text = _ClsProgSetting.GetSrvRootPath()
			Me.lblLMDPfad.Text = _ClsProgSetting.GetMDPath()
			Me.txtLPfad_1.Text = _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
			Me.txtLPfad_2.Text = _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
			Me.txtLPfad_3.Text = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "PrintFileSaveIn")

			Me.chkLUpdate.Checked = If(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options", "ShowUpdateOnStart") = "0", False, True)

			' Rapporteinstellungen
			Me.chkRPOpenWeek.CheckState = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms",
																								 "RPWeekOpen")))
			Me.chkRPWeekMust.CheckState = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms",
																								 "RPWeekObligation")))

			Me.seAnzTestCheck.Text = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms",
																								 "TempCheckAnz")))
			Me.seAnzTestRP.Text = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms",
																								 "TempRPAnz")))

			Me.cleLNormal_0.EditValue = System.Drawing.Color.FromArgb(CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Colour",
																						 "GotFocusPflichtColor"))))

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.0. {1}", strMethodeName, ex.Message))
			MsgBox(Err.Description, MsgBoxStyle.Critical)

		End Try

		Try
			Dim strUserprofile As String = UserXMLFileName
			strQuery = "//Layouts/Form_DevEx/FormStyle"
			Me.cboLFormStyle.EditValue = GetXMLValueByQuery(strUserprofile, strQuery, String.Empty)
			strQuery = "//Layouts/Form_DevEx/NavbarStyle"
			Me.cboLNavStyle.EditValue = GetXMLValueByQuery(strUserprofile, strQuery, String.Empty)


			Me.txtDavidfaxserver.Text = _ClsReg.GetINIString(strMDIniFile, "Export Db", "DavidServer")
			Me.txtFaxExtension.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "Fax-Extension")
			Me.txtFaxForwarder.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "Fax-Forwarder")

			Try
				Me.txtnotificationintervalperiode.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SONSTIGES_NOTIFICATION, mandantNumber))
			Catch ex As Exception
				Me.txtnotificationintervalperiode.Text = String.Empty
			End Try

			Try
				Me.txtnotificationintervalperiodeforreport.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SONSTIGES_NOTIFICATION_REPORT, mandantNumber))
			Catch ex As Exception
				Me.txtnotificationintervalperiodeforreport.Text = String.Empty
			End Try

			Try
				Me.txt_Vak_ID.EditValue = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_VACANCY_GUID, mandantNumber))
				chkenablevacancywos.Checked = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_VACANCY_GUID, mandantNumber)).Length > 30
				chkJobChannelPriority.Checked = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_JOBCHANNELPRIORITY, mandantNumber)), False)
			Catch ex As Exception
				Me.txt_Vak_ID.Text = String.Empty
				chkenablevacancywos.Checked = False
				chkJobChannelPriority.Checked = False
			End Try

			Try
				Me.txt_MA_ID.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, mandantNumber))
				chkenableemployeewos.Checked = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, mandantNumber)).Length > 30
			Catch ex As Exception
				Me.txt_MA_ID.Text = String.Empty
				chkenableemployeewos.Checked = False
			End Try

			Try
				Me.txt_KD_ID.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, mandantNumber))
				chkenablecustomerwos.Checked = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, mandantNumber)).Length > 30
				Me.txt_Ver_ID.EditValue = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_VER_GUID, mandantNumber))
			Catch ex As Exception
				Me.txt_Ver_ID.Text = String.Empty
				Me.txt_KD_ID.Text = String.Empty
				chkenablecustomerwos.Checked = False
			End Try

			Try
				chksendproposeattachmenttowos.Checked = ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_PROPOSE_GUID, mandantNumber)), False)
			Catch ex As Exception
				chksendproposeattachmenttowos.Checked = False
			End Try
			Try
				Me.txtCockpitWWW.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_COCKPIT_URL, mandantNumber))
			Catch ex As Exception
				Me.txtCockpitWWW.Text = String.Empty
			End Try
			Try
				Me.txtCockpitPicture.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_COCKPIT_PICTURE, mandantNumber))
			Catch ex As Exception
				Me.txtCockpitPicture.Text = String.Empty
			End Try


			Try
				txtSMTP.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/SMTP-Server", mandantNumber))

				Dim deliveryMethode As Integer? = Val(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/smtp-deliverymethode", mandantNumber)))
				If deliveryMethode = 0 Then deliveryMethode = Nothing
				cboSmtpDeliveryMethod.EditValue = deliveryMethode


			Catch ex As Exception
				txtSMTP.Text = String.Empty
				cboSmtpDeliveryMethod.EditValue = Nothing
			End Try
			Try
				Me.txtSMTPPort.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/SMTP-Port", mandantNumber))
			Catch ex As Exception
				Me.txtSMTPPort.Text = String.Empty
			End Try
			Try
				chkSSL.Checked = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/smtp-enablessl", mandantNumber))
			Catch ex As Exception
				chkSSL.Checked = False
			End Try


			Me.lblTemplatePath_1.Text = _ClsProgSetting.GetMDImagesPath

			Me.txtZVDocMailBetreff.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "Zwischenverdienstformular_Doc-eMail-Betreff")
			Me.txtArbgDocMailBetreff.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "Arbeitgeberbescheinigung_Doc-eMail-Betreff")

			Me.txtMADocMailBetreff.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "MADoc-eMail-Betreff")
			Me.txtMADocWWW.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "MADoc-WWW")

			Me.txtKDDocMailBetreff.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "KDDoc-eMail-Betreff")
			Me.txtKDDocWWW.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "KDDoc-WWW")

			Me.txtZHDDocMailBetreff.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "ZHDDoc-eMail-Betreff")
			Me.txtZHDDocWWW.Text = _ClsReg.GetINIString(strMDIniFile, "Mailing", "ZHDDoc-WWW")

			Try
				Me.txtZVDocMailBetreff.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/Zwischenverdienstformular_Doc-eMail-Betreff", mandantNumber))
			Catch ex As Exception
				Me.txtZVDocMailBetreff.Text = String.Empty
			End Try
			Try
				Me.txtArbgDocMailBetreff.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/Arbeitgeberbescheinigung_Doc-eMail-Betreff", mandantNumber))
			Catch ex As Exception
				Me.txtArbgDocMailBetreff.Text = String.Empty
			End Try
			Try
				Me.txtMADocMailBetreff.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/MADoc-eMail-Betreff", mandantNumber))
			Catch ex As Exception
				Me.txtMADocMailBetreff.Text = String.Empty
			End Try
			Try
				Me.txtMADocWWW.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/MADoc-WWW", mandantNumber))
			Catch ex As Exception
				Me.txtMADocWWW.Text = String.Empty
			End Try
			Try
				Me.txtKDDocMailBetreff.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/KDDoc-eMail-Betreff", mandantNumber))
			Catch ex As Exception
				Me.txtKDDocMailBetreff.Text = String.Empty
			End Try
			Try
				Me.txtKDDocWWW.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/KDDoc-WWW", mandantNumber))
			Catch ex As Exception
				Me.txtKDDocWWW.Text = String.Empty
			End Try
			Try
				Me.txtZHDDocMailBetreff.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/ZHDDoc-eMail-Betreff", mandantNumber))
			Catch ex As Exception
				Me.txtZHDDocMailBetreff.Text = String.Empty
			End Try
			Try
				Me.txtZHDDocWWW.Text = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/ZHDDoc-WWW", mandantNumber))
			Catch ex As Exception
				Me.txtZHDDocWWW.Text = String.Empty
			End Try


			Try
				Dim changeownreportforfinishingflag As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "changeownreportforfinishingflag", False))
				If changeownreportforfinishingflag Then Me.chkchangeownreportforfinishingflag.Checked = True
				Dim importscanreporttoboth As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "importscanreporttoboth", False))
				If importscanreporttoboth Then Me.chkimportscanreporttoboth.Checked = True

				Dim importscanreportzeroamount As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "importscanreportzeroamount", False))
				If importscanreportzeroamount Then Me.chkimportscanreportzeroamount.Checked = True

				Dim saveemployeeemploymentscanintowos As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "saveemployeeemploymentscanintowos", False))
				If saveemployeeemploymentscanintowos Then Me.chksaveemployeeemploymentscanintowos.Checked = True
				Dim saveemployeereportscanintowos As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "saveemployeereportscanintowos", False))
				If saveemployeereportscanintowos Then Me.chksaveemployeereportscanintowos.Checked = True
				Dim saveemployeepayrollscanintowos As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "saveemployeepayrollscanintowos", False))
				If saveemployeepayrollscanintowos Then Me.chksaveemployeepayrollscanintowos.Checked = True

				Dim savecustomeremploymentscanintowos As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "savecustomeremploymentscanintowos", False))
				If savecustomeremploymentscanintowos Then Me.chksavecustomeremploymentscanintowos.Checked = True
				Dim savecustomerreportscanintowos As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "savecustomerreportscanintowos", False))
				If savecustomerreportscanintowos Then Me.chksavecustomerreportscanintowos.Checked = True
				Dim savecustomerinvoicescanintowos As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "savecustomerinvoicescanintowos", False))
				If savecustomerinvoicescanintowos Then Me.chksavecustomerinvoicescanintowos.Checked = True

				Dim autofilterconditionnr As Integer = Val(_ClsProgSetting.GetMDProfilValue("Sonstiges", "autofilterconditionnr", 3))
				cboautofilterconditionnr.EditValue = autofilterconditionnr

				Dim autofilterconditiondate As Integer = Val(_ClsProgSetting.GetMDProfilValue("Sonstiges", "autofilterconditiondate", 3))
				cboAutoFilterConditionDate.EditValue = autofilterconditiondate

				Dim allowautofilterconditionchange As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "allowautofilterconditionchange", False))
				If allowautofilterconditionchange Then Me.chkAllowAutoFilterConditionChange.Checked = True


				Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
				If bAllowedtoDebug Then Me.chkGLLDebug.Checked = True

				Dim colorname As String = _ClsProgSetting.GetMDProfilValue("Sonstiges", "mandantcolor", "")
				If Not String.IsNullOrWhiteSpace(colorname) Then Me.ceMandantColor.Color = System.Drawing.Color.FromName(colorname) Else ceMandantColor.EditValue = Nothing

				Dim strValue As String

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecalljobid", "")
				Me.txteCallJobID.Text = strValue

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "faxextension", "")
				Me.txtFaxExtension.Text = strValue
				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "faxforwarder", "")
				Me.txtFaxForwarder.Text = strValue

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallfromtext", "")
				Me.txteCallFromText.Text = strValue

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallheaderid", "")
				Me.txteCallHeaderID.Text = strValue

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallheaderinfo", "")
				Me.txteCallHeaderInfo.Text = strValue

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallsubject", "")
				Me.txteCallSubject.Text = strValue

				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallnotification", "")
				Me.txteCallNotification.Text = strValue
				strValue = _ClsProgSetting.GetMDProfilValue("Mailing", "ecalltoken", "")
				Me.txteCallToken.Text = strValue


				strQuery = "//RPSetting/RPFilesize"
				Me.seRPScanSize.Text = GetXMLValueByQuery(strXMLProfileName, strQuery, 5)


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mailing-Abschnitt lesen. {1}", strMethodeName, ex.Message))

			End Try


			If ModulConstants.UserData.UserNr = 1 Then
				GetLicencedModulValue()
			End If

			GetUserSettingData()
			GetCommonModulsOpenSetting()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.1. {1}", strMethodeName, ex.Message))
			MsgBox(Err.Description, MsgBoxStyle.Critical)
		End Try

		Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.2. {1}", strMethodeName, ex.Message))
			MsgBox(Err.Description, MsgBoxStyle.Critical)

		End Try

	End Sub

	Sub GetUserSettingData()
		Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

		Dim askonexit As Boolean? = ParseToBoolean(m_path.GetXMLNodeValue(UserXMLFileName,
																																				String.Format("{0}/askonexit", FORM_XML_MAIN_KEY)), False)

		Try
			Me.chkaskonexit.Checked = If(askonexit Is Nothing, False, askonexit)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub

	Sub GetLicencedModulValue()

		Dim allowedemployeeweeklypayment As Boolean = m_utilitySP.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year),
																																				String.Format("{0}/allowedemployeeweeklypayment", m_licencing_uri)), False)


		Me.chkSesam.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "sesam"), False)
		Me.chkAbacus.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "abacus"), False)
		Me.chkSwifac.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "swifac"), False)
		Me.chkComatic.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "comatic"), False)
		Me.chkCresus.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "cresus"), False)
		Me.chkKMUFactoring.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "kmufactoring"), False)
		Me.chkCSOPList.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "csoplist"), False)
		Me.chkParifond.Checked = ParseToBoolean(m_md.IsModulLicenseOK(mandantNumber, Now.Year, "parifond"), False)

		chkScanDropIN.Checked = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).ScanDropIN
		chkCVDropIn.Checked = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).CVDropIN
		chkpmsearch.Checked = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).PMSearch
		chkallowedemployeeweeklypayment.Checked = allowedemployeeweeklypayment

		Me.txt_DVRefNo.Text = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).dvrefnr
		Me.txt_DVUSName.Text = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).dvusername
		Me.txt_DVPW.Text = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).dvuserpw
		Me.txt_DVURL.Text = m_md.ModulLicenseKeys(0, Now.Year, MandantXMLFileName).dvurl


	End Sub

	Sub GetCommonModulsOpenSetting()
		Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Layouts"
		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

		Dim openemployeeformmorethanonce As Boolean = ParseToBoolean(m_path.GetXMLNodeValue(MDFormXMLFileName, String.Format("{0}/openemployeeformmorethanonce", FORM_XML_MAIN_KEY)), True)
		Dim opencustomerformmorethanonce As Boolean = ParseToBoolean(m_path.GetXMLNodeValue(MDFormXMLFileName, String.Format("{0}/opencustomerformmorethanonce", FORM_XML_MAIN_KEY)), True)

		Dim openeinsatzformmorethanonce As Boolean = ParseToBoolean(m_path.GetXMLNodeValue(MDFormXMLFileName, String.Format("{0}/openeinsatzformmorethanonce", FORM_XML_MAIN_KEY)), True)
		Dim openreportsformmorethanonce As Boolean = ParseToBoolean(m_path.GetXMLNodeValue(MDFormXMLFileName, String.Format("{0}/openreportsformmorethanonce", FORM_XML_MAIN_KEY)), True)
		Dim openadvancedpaymentformmorethanonce As Boolean = ParseToBoolean(m_path.GetXMLNodeValue(MDFormXMLFileName, String.Format("{0}/openadvancedpaymentformmorethanonce", FORM_XML_MAIN_KEY)), True)
		Dim openinvoiceformmorethanonce As Boolean = ParseToBoolean(m_path.GetXMLNodeValue(MDFormXMLFileName, String.Format("{0}/openinvoiceformmorethanonce", FORM_XML_MAIN_KEY)), True)

		Try
			Me.chkopenemployeeformmorethanonce.Checked = openemployeeformmorethanonce
			Me.chkopencustomerformmorethanonce.Checked = opencustomerformmorethanonce
			Me.chkopeneinsatzformmorethanonce.Checked = openeinsatzformmorethanonce
			Me.chkopenreportsformmorethanonce.Checked = openreportsformmorethanonce
			Me.chkopenadvancedpaymentformmorethanonce.Checked = openadvancedpaymentformmorethanonce
			Me.chkopeninvoiceformmorethanonce.Checked = openinvoiceformmorethanonce


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
		Dim _ClsSaveSetting As New SaveMySettingData(Me, mandantNumber, userNumber)
		Dim result As Boolean = True

		Me.txtLPfad_0.Text &= If(Not Me.txtLPfad_0.Text.EndsWith("\"), "\", "")
		Me.txtLPfad_1.Text &= If(Not Me.txtLPfad_1.Text.EndsWith("\"), "\", "")
		Me.txtLPfad_2.Text &= If(Not Me.txtLPfad_2.Text.EndsWith("\"), "\", "")
		Me.txtLPfad_3.Text &= If(Not Me.txtLPfad_3.Text.EndsWith("\"), "\", "")

		result = result AndAlso _ClsSaveSetting.SaveRPSetting()
		result = result AndAlso _ClsSaveSetting.SaveMDData()

		result = result AndAlso _ClsSaveSetting.SaveUserData()
		result = result AndAlso _ClsSaveSetting.SaveCommonModulsSetting()

		result = result AndAlso _ClsSaveSetting.SaveMyWOSSetting()
		result = result AndAlso _ClsSaveSetting.SaveMyMailFaxSetting()
		result = result AndAlso _ClsSaveSetting.SaveMyMailTemplatesSetting()

		result = result AndAlso XMLWriter()

		If result Then
			m_UtilityUI.ShowInfoDialog("Die Daten wurden gespeichert.")
		Else
			m_UtilityUI.ShowErrorDialog("Die Daten konnten nicht gespeichert werden.")
		End If

	End Sub

	Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
		Me.Dispose()
	End Sub

	Private Function XMLWriter() As Boolean
		Dim result As Boolean = True
		Dim strXMLFile As String = MDFormXMLFileName
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		Try
			xDoc.Load(strXMLFile)

			xNode = xDoc.SelectSingleNode("*//All_FieldsColor")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "All_FieldsColor", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
			End If

			Dim strUserprofile As String = UserXMLFileName
			Dim xEle As XElement = XElement.Load(strUserprofile)
			Dim MyNode = xEle.Elements("Layouts").Elements("Form_DevEx").Elements("FormStyle").ToList()
			If MyNode.Count > 0 Then
				For Each cEle As XElement In MyNode
					cEle.ReplaceNodes(String.Format("{0}", Me.cboLFormStyle.EditValue))
				Next cEle

				MyNode = xEle.Elements("Layouts").Elements("Form_DevEx").Elements("NavbarStyle").ToList()
				If MyNode.Count > 0 Then
					For Each cEle As XElement In MyNode
						cEle.ReplaceNodes(String.Format("{0}", Me.cboLNavStyle.EditValue))
					Next cEle
				End If

			Else
				xEle.AddFirst(New XElement("Layouts", New XElement("Form_DevEx", New XElement("FormStyle", Me.cboLFormStyle.EditValue), New XElement("NavbarStyle", Me.cboLNavStyle.EditValue))))
			End If
			xEle.Save(strUserprofile)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try


		Return result

	End Function

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode,
																	ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function

	Private Sub nvMain_LinkClicked(sender As Object, e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles nvMain.LinkClicked

		Try
			Trace.WriteLine(String.Format("{0} >>> {1}", e.Link.ItemName, e.Link.Caption))
			strLinkName = e.Link.ItemName
			strLinkCaption = e.Link.Caption

			For i As Integer = 0 To Me.nvMain.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange
			Me.xscMain.SuspendLayout()

			Me.pGlobal.Visible = False
			Me.pLizenzen.Visible = False
			Me.pLColor.Visible = False
			Me.pCom_WOS.Visible = False
			'Me.pMetro_0.Visible = False
			Me.pMail_Tpl.Visible = False
			Me.pMailFax.Visible = False
			Me.pJobplattforms.Visible = False

			Select Case strLinkName.ToLower
				Case Me.nviLicense.Name.ToLower
					Me.pLizenzen.Visible = True
					Me.pLizenzen.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pLizenzen.Dock = DockStyle.Fill
					Me.gLizenzen.Dock = DockStyle.Fill

					'Case Me.nviFieldData.Name.ToLower
					'	Me.pFieldBez.Visible = True
					'	Me.pFieldBez.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					'	Me.pFieldBez.Dock = DockStyle.Fill
					'	Me.gFieldbez.Dock = DockStyle.Fill

				Case "nviLFieldColor".ToLower
					Me.pLColor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pLColor.Dock = DockStyle.Fill
					Me.gLColor.Dock = DockStyle.Fill
					Me.pLColor.Visible = True

				Case "nviGlobalSetting".ToLower
					Me.pGlobal.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pGlobal.Dock = DockStyle.Fill
					Me.gGlobal.Dock = DockStyle.Fill
					Me.pGlobal.Visible = True


				Case "nviWOS".ToLower
					Me.pCom_WOS.Visible = True
					Me.pCom_WOS.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pCom_WOS.Dock = DockStyle.Fill
					Me.gWOS.Dock = DockStyle.Fill

				'Case "nviMetro_0".ToLower
				'	Me.pMetro_0.Visible = True
				'	Me.pMetro_0.Dock = DockStyle.Fill

				Case "nviMailTpl".ToLower
					Me.pMail_Tpl.Visible = True
					Me.pMail_Tpl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pMail_Tpl.Dock = DockStyle.Fill
					Me.gMail_Tpl.Dock = DockStyle.Fill

				Case "nviMailFax".ToLower
					Me.pMailFax.Visible = True
					Me.pMailFax.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pMailFax.Dock = DockStyle.Fill
					Me.gMailFax.Dock = DockStyle.Fill

				Case "nviJWinner".ToLower

				Case "nviJCH".ToLower
					ShowJCHDataTab()

					Me.pJobplattforms.Visible = True
					Me.pJobplattforms.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pJobplattforms.Dock = DockStyle.Fill
					Me.gJCH.Dock = DockStyle.Fill

				Case "nviFirmenkonstanten".ToLower
					ShowJCHDataTab()

					Me.pJobplattforms.Visible = True
					Me.pJobplattforms.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
					Me.pJobplattforms.Dock = DockStyle.Fill
					Me.gJCH.Dock = DockStyle.Fill

			End Select


		Catch ex As Exception
			If ModulConstants.UserData.UserNr = 1 Then DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "Linkclicked",
																												MessageBoxButtons.OK, MessageBoxIcon.Error)

		Finally

		End Try
		Me.xscMain.ResumeLayout()

	End Sub

	Sub ShowJCHDataTab()
		Dim _ClsSaveSetting As New SaveMySettingData(Me, mandantNumber, userNumber)

		GetOurJobPlattformLogo()

		GetKDData4JobCH()
		ListKDSubData4JobCH()

		GetKDData4OstJob()

	End Sub


	Sub GetKDData4OstJob()
		Dim sSQL As String = "If Not Exists(Select Top 1 ID From US_JobPlattforms Where Customer_Guid = @Customer_Guid And Jobplattform_Art = 2) "
		sSQL &= "Insert Into US_JobPlattforms (Customer_Guid, Jobplattform_Art) Values "
		sSQL &= "(@Customer_Guid, 2) "

		sSQL &= "Select Top 1 * From US_JobPlattforms Where Customer_Guid = @Customer_Guid And Jobplattform_Art = 2"
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid) ' ModulConstants.MDData.MDGuid)
			Dim rMyrec As SqlDataReader = cmd.ExecuteReader

			rMyrec.Read()
			Me.txtOstJob_OrganisationID.Text = rMyrec("Organisation_ID").ToString

			Me.seOstJobKDAnz.Text = rMyrec("Organisation_Kontingent").ToString
			Me.seOstJobAddDayToDate.Text = rMyrec("DaysToAdd").ToString
			Me.txtOstjob_DirectlinkiFrame.Text = rMyrec("dirctlink_iframe").ToString
			Me.txtOstjob_Bewerberform.Text = rMyrec("Bewerberform").ToString


		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "Benutzerdaten laden")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub GetOurJobPlattformLogo()

		Me.picJCHLogo.Visible = False
		Me.picJCHLogo.EditValue = Nothing
		Dim strLogoFilename As String = StoreSelectedVakLogo2FS(1)
		If strLogoFilename <> String.Empty Then
			Dim myBitmap As New Bitmap(strLogoFilename)
			Me.picJCHLogo.Image = myBitmap

		Else
			Me.picJCHLogo.Image = Me.picJCHLogo.ErrorIcon

		End If
		Me.picJCHLogo.Visible = True


	End Sub

	Sub GetKDData4JobCH()
		Dim sSQL As String = "If Not Exists(Select Top 1 ID From US_JobPlattforms Where Customer_Guid = @Customer_Guid And Jobplattform_Art = 1) "
		sSQL &= "Insert Into US_JobPlattforms (Customer_Guid, Xing_Company_Is_Poc, Jobplattform_Art) Values "
		sSQL &= "(@Customer_Guid, 0, 1) "

		sSQL &= "Select Top 1 * From US_JobPlattforms Where Customer_Guid = @Customer_Guid And Jobplattform_Art = 1"
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn) ' ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid) ' ModulConstants.MDData.MDGuid)
			Dim rMyrec As SqlDataReader = cmd.ExecuteReader

			rMyrec.Read()
			Me.txtJCH_Organisation_ID.Text = rMyrec("Organisation_ID").ToString

			Me.seJCHKDAnz.Text = rMyrec("Organisation_Kontingent").ToString
			Me.seJCHAddDayToDate.Text = rMyrec("DaysToAdd").ToString

			Me.txtJCH_Our_URL.Text = rMyrec("Our_URL").ToString
			Me.txtJCH_Direkt_URL.Text = rMyrec("Direkt_URL").ToString

			Me.txtJCH_Xing_Poster_URL.Text = rMyrec("Xing_Poster_URL").ToString
			Me.txtJCH_Xing_Company_Profile_URL.Text = rMyrec("Xing_Company_Profile_URL").ToString
			Me.chJCH_Xing_Company_Is_Poc.Checked = If(IsDBNull(rMyrec("Xing_Company_Is_Poc")), False, CBool(rMyrec("Xing_Company_Is_Poc")))


		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "Benutzerdaten laden")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListKDSubData4JobCH()
		Dim sSQL As String = "Select * From tblJobAccount Where Customer_Guid = @Customer_Guid Order By Organisation_SubID"
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid) ' ModulConstants.MDData.MDGuid)
			Dim rMyrec As SqlDataReader = cmd.ExecuteReader

			Me.lstJCHKDSubNr.Items.Clear()
			While rMyrec.Read()
				Me.lstJCHKDSubNr.Items.Add(String.Format("{0} - {1}",
																								 rMyrec("Organisation_SubID").ToString,
																								 rMyrec("Organisation_Sub_Kontingent").ToString))
			End While


		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "Sub-Nummern laden")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Private Sub cboLFormStyle_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboLFormStyle.QueryPopUp

		Me.cboLFormStyle.Properties.Items.Clear()
		For Each cnt As DevExpress.Skins.SkinContainer In DevExpress.Skins.SkinManager.Default.Skins
			Me.cboLFormStyle.Properties.Items.Add(cnt.SkinName)
		Next cnt

	End Sub

	Private Sub cboLNavStyle_QueryPopUp(sender As System.Object, e As System.EventArgs) Handles cboLNavStyle.QueryPopUp

		Me.cboLNavStyle.Properties.Items.Clear()
		For i As Integer = 0 To Me.nvMain.AvailableNavBarViews.Count - 1
			cboLNavStyle.Properties.Items.Add(Me.nvMain.AvailableNavBarViews(i).ViewName)
		Next

	End Sub

	Private Sub txtLPfad_3_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtLPfad_3.ButtonClick
		Dim dialog As New FolderBrowserDialog()

		dialog.Description = "Bitte wählen Sie ein Verzeichnis für Export der Datei aus:"
		dialog.ShowNewFolderButton = True
		dialog.SelectedPath = If(Me.txtLPfad_3.Text = String.Empty, m_path.GetSpS2DeleteHomeFolder, Path.GetDirectoryName(Me.txtLPfad_3.Text))
		If dialog.ShowDialog() = DialogResult.OK Then
			Me.txtLPfad_3.Text = String.Format("{0}\", dialog.SelectedPath)
		End If

	End Sub

	Private Sub lbleCallJobID_Click(sender As System.Object, e As System.EventArgs) Handles lbleCallJobID.Click

		Me.txteCallJobID.Text = ModulConstants.MDData.MDGuid

		Me.txteCallFromText.Text = ModulConstants.MDData.MDTelefax

		Me.txteCallHeaderID.Text = String.Format("Header ID {0}", ModulConstants.MDData.MDTelefax)
		Me.txteCallHeaderInfo.Text = String.Format("Header Info {0}", ModulConstants.MDData.MDTelefax)
		Me.txteCallSubject.Text = "FAX Betreff"

		Me.txteCallToken.Text = "MaxRetries;=1"

	End Sub



	Private Sub cmdSaveAccountData_Click(sender As System.Object, e As System.EventArgs) Handles cmdSaveJCHAccount.Click
		Dim _ClsSaveSetting As New SaveMySettingData(Me, mandantNumber, userNumber)

		_ClsSaveSetting.SaveUSData4JobPlattforms()
		GetKDData4JobCH()

	End Sub

	Private Sub cmdSaveOstJobAccount_Click(sender As System.Object, e As System.EventArgs) Handles cmdSaveOstJobAccount.Click
		Dim _ClsSaveSetting As New SaveMySettingData(Me, mandantNumber, userNumber)

		_ClsSaveSetting.SaveUSData4OstJobPlattforms()
		GetKDData4OstJob()

	End Sub

	Private Sub cmdSaveSubNr_Click(sender As System.Object, e As System.EventArgs) Handles cmdSaveSubNr.Click
		If CInt(Val(Me.txtJCH_Organisation_ID.Text)) = 0 Or CInt(Val(Me.seJCHKDAnz.Text)) = 0 Then Exit Sub
		Dim _ClsSaveSetting As New SaveMySettingData(Me, mandantNumber, userNumber)

		_ClsSaveSetting.SaveJCHKDSubData()
		ListKDSubData4JobCH()

	End Sub

	Private Sub lstJCHKDSubNr_Click(sender As Object, e As System.EventArgs) Handles lstJCHKDSubNr.Click
		Dim iIndex As Integer = Me.lstJCHKDSubNr.SelectedIndex
		Me.txtJCHKDSubNr.Text = String.Empty
		Me.seJCHKDSubAnz.Text = 0
		If iIndex = -1 Then Exit Sub

		Dim aSubData As String() = Me.lstJCHKDSubNr.Text.Split(CChar("-"))

		Me.txtJCHKDSubNr.Text = aSubData(0).Trim
		Me.seJCHKDSubAnz.Text = aSubData(1).Trim

	End Sub


	Private Sub lstJCHKDSubNr_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstJCHKDSubNr.SelectedIndexChanged
		'Dim iIndex As Integer = Me.lstJCHKDSubNr.SelectedIndex
		'Me.txtJCHKDSubNr.Text = String.Empty
		'Me.seJCHKDSubAnz.Text = 0
		'If iIndex = -1 Then Exit Sub

		'Dim aSubData As String() = Me.lstJCHKDSubNr.SelectedItem(CChar("-"))

		'Me.txtJCHKDSubNr.Text = aSubData(0)
		'Me.seJCHKDSubAnz.Text = aSubData(1)

	End Sub


	Private Sub cmdJCHKDSubDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdJCHKDSubDelete.Click
		Dim iIndex As Integer = Me.lstJCHKDSubNr.SelectedIndex
		Me.txtJCHKDSubNr.Text = String.Empty
		Me.seJCHKDSubAnz.Text = 0
		If iIndex = -1 Then Exit Sub

		Dim aSubData As String() = Me.lstJCHKDSubNr.Text.Split(CChar("-"))

		Me.txtJCHKDSubNr.Text = aSubData(0).Trim
		Me.seJCHKDSubAnz.Text = aSubData(1).Trim

		Dim _ClsSaveSetting As New SaveMySettingData(Me, mandantNumber, userNumber)

		_ClsSaveSetting.DeleteSelectedjCHKDSubNr(CInt(Val(Me.txtJCHKDSubNr.Text)))
		ListKDSubData4JobCH()

	End Sub


	Private Sub picJCHLogo_click(sender As System.Object, e As System.EventArgs) Handles picJCHLogo.Click
		Dim DlgOpen As New OpenFileDialog

		DlgOpen.FileName = ""
		DlgOpen.ShowDialog()
		If DlgOpen.FileName = "" Then Return
		Try
			Me.picJCHLogo.Image = Nothing

		Catch ex As Exception
			Me.picJCHLogo.Image = Me.picJCHLogo.ErrorIcon

		End Try

		Dim strFilename As String = DlgOpen.FileName
		Try

			Dim myBitmap As New Bitmap(strFilename)
			Me.picJCHLogo.Image = myBitmap
			SaveJCHFileIntoDb(strFilename, Me.picJCHLogo.Image)

		Catch ex As Exception
			Try
				Me.picJCHLogo.Image = Me.picJCHLogo.ErrorIcon

			Catch ex_0 As Exception

			End Try
		End Try

	End Sub

	Function StoreSelectedVakLogo2FS(ByVal iDocArt As Integer) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim sMASql As String = "Select FileContent From MD_Documents Where Customer_Guid = @Customer_Guid" ' And Doc_Art = @DocArt"
		Dim i As Integer = 0

		Conn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sMASql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sMASql, Conn)
		param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)
		'param = cmd.Parameters.AddWithValue("@DocArt", iDocArt)

		Try

			strFullFilename = String.Format("{0}Vak_{1}.PNG", m_path.GetSpS2DeleteHomeFolder,
																			 System.Guid.NewGuid.ToString())

			Try
				Try

					BA = CType(cmd.ExecuteScalar, Byte())

				Catch ex As Exception

				End Try
				If BA Is Nothing Then
					Return String.Empty
				End If

				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()

			Catch ex As Exception
				MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "GetMAPicture")
				strFullFilename = String.Empty

			End Try

		Catch ex As Exception
			strFullFilename = String.Empty

		End Try

		Return strFullFilename
	End Function

	Function SaveJCHFileIntoDb(ByVal strFileToSave As String, ByVal bild As Image) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Conn As New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim sSql As String = String.Empty
		Dim strResult As String = String.Empty

		sSql = "If Not Exists(Select ID From MD_Documents Where Customer_Guid = @Customer_Guid) "
		sSql &= "Insert Into MD_Documents (MDNr, Customer_Guid) Values "
		sSql &= "(@MDNr, @Customer_Guid) "

		sSql &= "Update MD_Documents Set MDNr = @MDNr, Customer_Guid = @Customer_Guid, "
		sSql &= "Filecontent = @BinaryFile, Filebez = @FileBez Where Customer_Guid = @Customer_Guid"

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()
			cmd.Connection = Conn

			If strFileToSave <> String.Empty Then
				Dim myFile() As Byte = Image2ByteArray(bild, Imaging.ImageFormat.Bmp)
				Dim fi As New System.IO.FileInfo(strFileToSave)
				Dim strFileExtension As String = fi.Extension

				Try
					cmd.CommandType = CommandType.Text
					cmd.CommandText = sSql

					param = cmd.Parameters.AddWithValue("@MDNr", ModulConstants.MDData.MDNr)
					param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)
					param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
					param = cmd.Parameters.AddWithValue("@FileBez", TranslateText("jobs.ch"))

					cmd.Connection = Conn
					cmd.ExecuteNonQuery()

					cmd.Parameters.Clear()
					strResult = "Erfolgreich..."


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Daten schreiben. {1}", strMethodeName, ex.Message))
					strResult = String.Format("***Fehler (SaveFileIntoDb_1): {0}", ex.Message)

				End Try
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Datenbank öffnen. {1}", strMethodeName, ex.Message))
			strResult = String.Format("***Fehler (SaveFileIntoDb_2): {0}", ex.Message)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Return strResult
	End Function

	Function SaveOstJobFileIntoDb(ByVal strFileToSave As String, ByVal bild As Image) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Conn As New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim sSql As String = String.Empty
		Dim strResult As String = String.Empty

		sSql = "If Not Exists(Select ID From MD_Documents Where Customer_Guid = @Customer_Guid And Doc_Art = 2) "
		sSql &= "Insert Into MD_Documents (MDNr, Customer_Guid, Doc_Art) Values "
		sSql &= "(@MDNr, @Customer_Guid, 2) "

		sSql &= "Update MD_Documents Set MDNr = @MDNr, Customer_Guid = @Customer_Guid, "
		sSql &= "Filecontent = @BinaryFile, Filebez = @FileBez, Doc_Art = 2 Where Customer_Guid = @Customer_Guid And Doc_Art = 2"

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()
			cmd.Connection = Conn

			If strFileToSave <> String.Empty Then
				Dim myFile() As Byte = Image2ByteArray(bild, Imaging.ImageFormat.Bmp)
				Dim fi As New System.IO.FileInfo(strFileToSave)
				Dim strFileExtension As String = fi.Extension

				Try
					cmd.CommandType = CommandType.Text
					cmd.CommandText = sSql

					param = cmd.Parameters.AddWithValue("@MDNr", ModulConstants.MDData.MDNr)
					param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)
					param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
					param = cmd.Parameters.AddWithValue("@FileBez", TranslateText("Ostjob.ch"))

					cmd.Connection = Conn
					cmd.ExecuteNonQuery()

					cmd.Parameters.Clear()
					strResult = "Erfolgreich..."


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Daten schreiben. {1}", strMethodeName, ex.Message))
					strResult = String.Format("***Fehler (SaveFileIntoDb_1): {0}", ex.Message)

				End Try
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Datenbank öffnen. {1}", strMethodeName, ex.Message))
			strResult = String.Format("***Fehler (SaveFileIntoDb_2): {0}", ex.Message)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Dim Time_2 As Double = System.Environment.TickCount
		Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Return strResult
	End Function

	Function Image2ByteArray(ByVal Bild As Image,
												 ByVal Bildformat As System.Drawing.Imaging.ImageFormat) As Byte()
		Dim MS As New IO.MemoryStream
		Bild.Save(MS, Bildformat)
		MS.Flush()

		Return MS.ToArray
	End Function

	Private Sub cboLFormStyle_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboLFormStyle.SelectedIndexChanged
		Dim comboBox As ComboBoxEdit = sender
		Dim skinName As String = comboBox.Text

		DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = skinName

	End Sub


#Region "Helpers"

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
		Dim result As Integer
		If (Not Integer.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

#End Region




	Private Sub ceMandantColor_ButtonClick(sender As Object, e As EventArgs) Handles ceMandantColor.ButtonClick
		Me.ceMandantColor.EditValue = Nothing
	End Sub


#Region "helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

#End Region


End Class



Public Class SaveMySettingData


#Region "private consts"

	Private Const MANDANT_XML_SETTING_SONSTIGES As String = "MD_{0}/Sonstiges/notificationintervalperiode"
	Private Const MANDANT_XML_SETTING_SONSTIGES_NOTIFICATION_REPORT As String = "MD_{0}/Sonstiges/notificationintervalperiodeforreport"
	Private Const MANDANT_XML_SETTING_WOS_VACANCY_GUID As String = "MD_{0}/Export/Vak_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_JOBCHANNELPRIORITY As String = "MD_{0}/Export/jobchannelpriority"
	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_VER_GUID As String = "MD_{0}/Export/Ver_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_PROPOSE_GUID As String = "MD_{0}/Export/sendproposeattachmenttowos"

	Private Const MANDANT_XML_SETTING_COCKPIT_EMAIL_TEMPLATE As String = "MD_{0}/Templates/cockpit-email-template"
	Private Const MANDANT_XML_SETTING_COCKPIT_URL As String = "MD_{0}/Templates/cockpit-url"
	Private Const MANDANT_XML_SETTING_WOS_COCKPIT_PICTURE As String = "MD_{0}/Templates/cockpit-picture"

	Private Const MANDANT_XML_SETTING_MAILING_URL As String = "MD_{0}/Mailing"

#End Region


#Region "Constants"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Thre translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private strMDIniFile As String

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	Private m_MandantSettingsXml As SettingsXml

	Private frm As frmTemplateDetail
	Private m_VacancyGuid As String
	Private m_EmployeeGuid As String
	Private m_CustomerGuid As String

	Private mandantNumber As Integer
	Private userNumber As Integer

#End Region


#Region "public properties"

	Public ReadOnly Property GetMDPath() As String
		Get
			Return m_md.GetSelectedMDYearPath(mandantNumber, Now.Year)
		End Get
	End Property

	Public ReadOnly Property GetMDIniFile() As String
		Get
			Return String.Format("{0}{1}", GetMDPath, "Programm.dat")
		End Get
	End Property

	Public ReadOnly Property UserXMLFileName() As String
		Get
			Return m_md.GetSelectedMDUserProfileXMLFilename(mandantNumber, userNumber)
		End Get
	End Property

	Public ReadOnly Property MandantXMLFileName() As String
		Get
			Return m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year)
		End Get
	End Property

	Public ReadOnly Property MDFormXMLFileName() As String
		Get
			Return m_md.GetSelectedMDFormDataXMLFilename(mandantNumber)
		End Get
	End Property

#End Region


#Region "Contructor"

	Public Sub New(ByVal frmTemp As Form, ByVal mdnr As Integer, ByVal usnr As Integer)

		Me.frm = frmTemp
		mandantNumber = mdnr
		userNumber = usnr

		m_md = New Mandant
		m_utility = New Utilities
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

		strMDIniFile = GetMDIniFile

		Try
			m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

#End Region


	Public Function SaveMyMailFaxSetting() As Boolean
		Dim result As Boolean = True

		Try
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "SMTP-Server", frm.txtSMTP.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "SMTP-Port", Val(frm.txtSMTPPort.Text))
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "smtp-enablessl", If(frm.chkSSL.Checked, 1, 0))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try


		Return result

	End Function

	Public Function SaveMyWOSSetting() As Boolean
		Dim result As Boolean = True


		Try
			_ClsReg.SetINIString(strMDIniFile, "Export", "Vak_SPUser_ID", frm.txt_Vak_ID.Text)
			_ClsReg.SetINIString(strMDIniFile, "Export", "MA_SPUser_ID", frm.txt_MA_ID.Text)
			_ClsReg.SetINIString(strMDIniFile, "Export", "KD_SPUser_ID", frm.txt_KD_ID.Text)
			_ClsReg.SetINIString(strMDIniFile, "Export", "Ver_SPUser_ID", frm.txt_Ver_ID.Text)

			_ClsReg.SetINIString(strMDIniFile, "Mailing", "Cockpit-WWW", frm.txtCockpitWWW.Text)
			_ClsReg.SetINIString(strMDIniFile, "Templates", "Cockpit-Picture", frm.txtCockpitPicture.Text)


			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_SONSTIGES, mandantNumber), Math.Min(Math.Max(Val(frm.txtnotificationintervalperiode.Text), 1), 60))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_SONSTIGES_NOTIFICATION_REPORT, mandantNumber), Val(frm.txtnotificationintervalperiodeforreport.Text))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_VACANCY_GUID, mandantNumber), frm.txt_Vak_ID.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_JOBCHANNELPRIORITY, mandantNumber), If(frm.chkJobChannelPriority.Checked, "true", "false"))
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, mandantNumber), frm.txt_MA_ID.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, mandantNumber), frm.txt_KD_ID.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_VER_GUID, mandantNumber), frm.txt_Ver_ID.Text)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_PROPOSE_GUID, mandantNumber), If(frm.chksendproposeattachmenttowos.Checked, "true", "false"))

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_COCKPIT_URL, mandantNumber), frm.txtCockpitWWW.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_WOS_COCKPIT_PICTURE, mandantNumber), frm.txtCockpitPicture.Text)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try


		Return result

	End Function

	Public Function SaveMyMailTemplatesSetting() As Boolean
		Dim result As Boolean = True


		Try
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "Zwischenverdienstformular_Doc-eMail-Betreff",
												 frm.txtZVDocMailBetreff.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "Arbeitgeberbescheinigung_Doc-eMail-Betreff",
												 frm.txtArbgDocMailBetreff.Text)

			_ClsReg.SetINIString(strMDIniFile, "Mailing", "MADoc-eMail-Betreff", frm.txtMADocMailBetreff.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "MADoc-WWW", frm.txtMADocWWW.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "KDDoc-eMail-Betreff", frm.txtKDDocMailBetreff.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "KDDoc-WWW", frm.txtKDDocWWW.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "ZHDDoc-eMail-Betreff", frm.txtZHDDocMailBetreff.Text)
			_ClsReg.SetINIString(strMDIniFile, "Mailing", "ZHDDoc-WWW", frm.txtZHDDocWWW.Text)


			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/Zwischenverdienstformular_Doc-eMail-Betreff", mandantNumber), frm.txtZVDocMailBetreff.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/Arbeitgeberbescheinigung_Doc-eMail-Betreff", mandantNumber), frm.txtArbgDocMailBetreff.Text)

			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/MADoc-eMail-Betreff", mandantNumber), frm.txtMADocMailBetreff.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/MADoc-WWW", mandantNumber), frm.txtMADocWWW.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/KDDoc-eMail-Betreff", mandantNumber), frm.txtKDDocMailBetreff.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/KDDoc-WWW", mandantNumber), frm.txtKDDocWWW.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/ZHDDoc-eMail-Betreff", mandantNumber), frm.txtZHDDocMailBetreff.Text)
			m_MandantSettingsXml.AddOrUpdateSetting(String.Format(MANDANT_XML_SETTING_MAILING_URL & "/ZHDDoc-WWW", mandantNumber), frm.txtZHDDocWWW.Text)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try


		Return result

	End Function

	Public Function SaveRPSetting() As Boolean

		Dim result As Boolean = True

		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath", String.Format("{0}Bin\", frm.txtLPfad_0.Text))

		Try
			If Directory.Exists(String.Format("{0}{1}", m_md.GetSelectedMDData(mandantNumber).MDMainPath, frm.txtLPfad_1.Text)) Then
				_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath", frm.txtLPfad_1.Text)
			Else
				Throw New FileNotFoundException

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowOKDialog(frm, String.Format("Dokumenten-Pfad: Pfadangaben speichern.<br>{0}", String.Format("{0}{1}", m_md.GetSelectedMDData(mandantNumber).MDMainPath, frm.txtLPfad_1.Text)),
									 "Fehlerhaftes Verzeichniss", MessageBoxIcon.Stop)
			result = False
		End Try

		Try

			If Directory.Exists(String.Format("{0}{1}", m_md.GetSelectedMDData(mandantNumber).MDMainPath, frm.txtLPfad_2.Text)) Then
				_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath", frm.txtLPfad_2.Text)
			Else
				Throw New FileNotFoundException

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowOKDialog(frm, String.Format("Vorlagen-Pfad: Pfadangaben speichern.<br>{0}", String.Format("{0}{1}", m_md.GetSelectedMDData(mandantNumber).MDMainPath, frm.txtLPfad_2.Text)),
									 "Fehlerhaftes Verzeichniss", MessageBoxIcon.Stop)
			result = False

		End Try

		Try
			If frm.txtLPfad_3.Text.Contains("%") Then frm.txtLPfad_3.EditValue = TranslatePath(frm.txtLPfad_3.EditValue)

			If Directory.Exists(String.Format("{0}", frm.txtLPfad_3.Text)) Then
				_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "PrintFileSaveIn", frm.txtLPfad_3.EditValue)
			Else
				Throw New FileNotFoundException

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowOKDialog(frm, String.Format("Temporäre Dateien: Pfadangaben speichern.<br>{0}", String.Format("{0}{1}", m_md.GetSelectedMDData(mandantNumber).MDMainPath, frm.txtLPfad_3.Text)),
									 "Fehlerhaftes Verzeichniss", MessageBoxIcon.Stop)
			result = False

		End Try

		Try
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options", "ShowUpdateOnStart", If(frm.chkLUpdate.Checked, 1, 0))

			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "RPWeekOpen", If(frm.chkRPOpenWeek.CheckState = CheckState.Checked, 1, 0))
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "RPWeekObligation", If(frm.chkRPWeekMust.CheckState = CheckState.Checked, 1, 0))

			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "TempCheckAnz", If(Val(frm.seAnzTestCheck.Text) = 0, 5, Val(frm.seAnzTestCheck.Text)))
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "TempRPAnz", If(Val(frm.seAnzTestRP.Text) = 0, 5, Val(frm.seAnzTestRP.Text)))

			Dim strXMLFile As String = MDFormXMLFileName
			Dim xDoc As XmlDocument = New XmlDocument()
			Dim xNode As XmlNode
			Dim xElmntFamily As XmlElement = Nothing

			xDoc.Load(strXMLFile)

			xNode = xDoc.SelectSingleNode("*//RPSetting")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "RPSetting", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				' Open RPWeek
				If xElmntFamily.SelectSingleNode("OpenRPWeekDropDown") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("OpenRPWeekDropDown"))
				InsertTextNode(xDoc, xElmntFamily, "OpenRPWeekDropDown", frm.chkRPOpenWeek.CheckState)
				' Must RPWeek
				If xElmntFamily.SelectSingleNode("RPWeekDropDownMust") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("RPWeekDropDownMust"))
				InsertTextNode(xDoc, xElmntFamily, "RPWeekDropDownMust", frm.chkRPWeekMust.CheckState)

				' Anz CheckTest
				If xElmntFamily.SelectSingleNode("CheckTestCount") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("CheckTestCount"))
				InsertTextNode(xDoc, xElmntFamily, "CheckTestCount", frm.seAnzTestCheck.Text)
				' Anz RPTest
				If xElmntFamily.SelectSingleNode("RPTestCount") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("RPTestCount"))
				InsertTextNode(xDoc, xElmntFamily, "RPTestCount", CInt(Val(frm.seAnzTestRP.Text)))
				' RPFilesize
				If xElmntFamily.SelectSingleNode("RPFilesize") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("RPFilesize"))
				InsertTextNode(xDoc, xElmntFamily, "RPFilesize", CInt(Val(frm.seRPScanSize.Text)))
			End If
			xDoc.Save(strXMLFile)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False

		End Try


		Return result

	End Function

	Private Function TranslatePath(ByVal pathToTranslate As String) As String
		Dim result As String = pathToTranslate

		Try
			If result.ToUpper.Contains("%userprofile%".ToUpper) Then
				result = result.Replace("%userprofile%", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))

			ElseIf result.ToUpper.Contains("%appdata%".ToUpper) Then
				result = result.Replace("%appdata%", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)))

			End If
			If Not Directory.Exists(result) Then Directory.CreateDirectory(result)

		Catch ex As Exception
			m_Logger.LogError(String.Format("path could not be translated: {0}", pathToTranslate))

			Return pathToTranslate

		End Try


		Return result

	End Function

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode,
															ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function

	Sub SaveUSData4JobPlattforms()
		If Not Allowed2SaveJCHKDData(CInt(Val(frm.seJCHKDAnz.Text))) Then Exit Sub

		Dim sSQL As String = "Update US_JobPlattforms Set Organisation_ID = @Organisation_ID, Organisation_Kontingent = @Organisation_Kontingent, "
		sSQL &= "Our_URL = @Our_URL, Direkt_URL = @Direkt_URL, "
		sSQL &= "Xing_Company_Profile_URL = @Xing_Company_Profile_URL, Xing_Poster_URL = @Xing_Poster_URL, "
		sSQL &= "Xing_Company_Is_Poc = @Xing_Company_Is_Poc, DaysToAdd = @DaysToAdd, [Jobplattform_Art] = 1 "
		sSQL &= "Where Customer_Guid = @Customer_Guid And Jobplattform_Art = 1"

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Organisation_ID", CInt(Val(frm.txtJCH_Organisation_ID.Text)))
			param = cmd.Parameters.AddWithValue("@Organisation_Kontingent", CInt(Val(frm.seJCHKDAnz.Text)))
			param = cmd.Parameters.AddWithValue("@DaysToAdd", CInt(Val(frm.seJCHAddDayToDate.Text)))

			param = cmd.Parameters.AddWithValue("@Our_URL", frm.txtJCH_Our_URL.Text)
			param = cmd.Parameters.AddWithValue("@Direkt_URL", frm.txtJCH_Direkt_URL.Text)

			param = cmd.Parameters.AddWithValue("@Xing_Company_Profile_URL", frm.txtJCH_Xing_Company_Profile_URL.Text)
			param = cmd.Parameters.AddWithValue("@Xing_Poster_URL", frm.txtJCH_Xing_Poster_URL.Text)
			param = cmd.Parameters.AddWithValue("@Xing_Company_Is_Poc", CBool(frm.chJCH_Xing_Company_Is_Poc.Checked))

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)

			cmd.ExecuteNonQuery()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Kundendaten für Job-Plattform speichern")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub SaveJCHKDSubData()
		If Not Allowed2SaveJCHKDSubData(CInt(Val(frm.seJCHKDSubAnz.Text))) Then Exit Sub

		Dim sSQL As String = "Delete tblJobAccount Where Customer_Guid = @Customer_Guid And Organisation_ID = @Organisation_ID "
		sSQL &= "And Organisation_SubID = @Organisation_SubID "
		sSQL &= "Insert Into tblJobAccount (Customer_Guid, Organisation_ID, Organisation_SubID, Organisation_Sub_Kontingent) "
		sSQL &= "Values (@Customer_Guid, @Organisation_ID, @Organisation_SubID, @Organisation_Sub_Kontingent) "

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Organisation_ID", CInt(Val(frm.txtJCH_Organisation_ID.Text)))
			param = cmd.Parameters.AddWithValue("@Organisation_SubID", CInt(Val(frm.txtJCHKDSubNr.Text)))
			param = cmd.Parameters.AddWithValue("@Organisation_Sub_Kontingent", CInt(Val(frm.seJCHKDSubAnz.Text)))

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)

			cmd.ExecuteNonQuery()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Sub-Nummer speichern")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function Allowed2SaveJCHKDData(ByVal iNewKontingent As Integer) As Boolean
		Dim bResult As Boolean = False
		Dim sSQL As String = "[Get Vakanz Kontingent Data]"

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)
			param = cmd.Parameters.AddWithValue("@Organisation_ID", CInt(Val(frm.txtJCH_Organisation_ID.Text)))

			Dim rMyrec As SqlDataReader = cmd.ExecuteReader

			rMyrec.Read()
			Dim iTotalKontingent As Integer = rMyrec("TotalKontingent").ToString
			Dim iSubKontingent As Integer = rMyrec("SubKontingent").ToString

			If iNewKontingent < iSubKontingent Then
				Dim strMsg As String = "Die Platz-Anzahl darf nicht gespeichert werden. Teil-Plätze: {0}"
				strMsg = String.Format(strMsg, iSubKontingent)
				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Anzhal Plätze speichern",
																									 MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Else
				bResult = True
			End If

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Anzahl Plätze speichern")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return bResult
	End Function

	Function Allowed2SaveJCHKDSubData(ByVal iNewKontingent As Integer) As Boolean
		Dim bResult As Boolean = False
		Dim sSQL As String = "[Get Vakanz SubKontingent Data]"

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)
			param = cmd.Parameters.AddWithValue("@Organisation_ID", CInt(Val(frm.txtJCH_Organisation_ID.Text)))
			param = cmd.Parameters.AddWithValue("@Organisation_SubID", CInt(Val(frm.txtJCHKDSubNr.Text)))

			Dim rMyrec As SqlDataReader = cmd.ExecuteReader

			rMyrec.Read()
			Dim iTotalKontingent As Integer = rMyrec("TotalKontingent").ToString
			Dim iSubKontingent As Integer = rMyrec("SubKontingent").ToString

			If iTotalKontingent < iSubKontingent + iNewKontingent Then
				Dim strMsg As String = "Die Kontingent-Anzahl darf nicht gespeichert werden. Gesamt-Platz: {0}"
				strMsg = String.Format(strMsg, iTotalKontingent)
				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Anzahl Teil-Plätze speichern",
																									 MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			Else
				bResult = True
			End If

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Anzahl Teil-Plätze speichern")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return bResult
	End Function

	Sub DeleteSelectedjCHKDSubNr(ByVal SubNr As Integer)
		Dim sSQL As String = "Delete tblJobAccount Where Customer_Guid = @Customer_Guid And Organisation_ID = @Organisation_ID And Organisation_SubID = @Organisation_SubID"

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Organisation_ID", CInt(Val(frm.txtJCH_Organisation_ID.Text)))
			param = cmd.Parameters.AddWithValue("@Organisation_SubID", SubNr)

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)

			cmd.ExecuteNonQuery()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Sub-Nummer löschen")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub SaveUSData4OstJobPlattforms()
		If Not Allowed2SaveJCHKDData(CInt(Val(frm.seJCHKDAnz.Text))) Then Exit Sub

		Dim sSQL As String = "Update US_JobPlattforms Set Organisation_ID = @Organisation_ID, Organisation_Kontingent = @Organisation_Kontingent, "
		sSQL &= "DaysToAdd = @DaysToAdd, [Jobplattform_Art] = 2, "
		sSQL &= "dirctlink_iframe = @dirctlink_iframe, Bewerberform = @Bewerberform "
		sSQL &= "Where Customer_Guid = @Customer_Guid And Jobplattform_Art = 2"

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Organisation_ID", CInt(Val(frm.txtOstJob_OrganisationID.Text)))
			param = cmd.Parameters.AddWithValue("@Organisation_Kontingent", CInt(Val(frm.seOstJobKDAnz.Text)))
			param = cmd.Parameters.AddWithValue("@DaysToAdd", CInt(Val(frm.seOstJobAddDayToDate.Text)))
			param = cmd.Parameters.AddWithValue("@dirctlink_iframe", frm.txtOstjob_DirectlinkiFrame.Text)
			param = cmd.Parameters.AddWithValue("@Bewerberform", frm.txtOstjob_Bewerberform.Text)

			param = cmd.Parameters.AddWithValue("@Customer_Guid", ModulConstants.MDData.MDGuid)

			cmd.ExecuteNonQuery()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Kundendaten für OstJob-Plattform speichern")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub


	Public Function SaveMDData() As Boolean
		Dim result As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strXMLFile As String = MandantXMLFileName
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		xDoc.Load(strXMLFile)

		Try
			xNode = xDoc.SelectSingleNode("*//Sonstiges")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Sonstiges", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				If xElmntFamily.SelectSingleNode("changeownreportforfinishingflag") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("changeownreportforfinishingflag"))
				InsertTextNode(xDoc, xElmntFamily, "changeownreportforfinishingflag", If(frm.chkchangeownreportforfinishingflag.Checked, "true", "false"))
				If xElmntFamily.SelectSingleNode("importscanreporttoboth") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("importscanreporttoboth"))
				InsertTextNode(xDoc, xElmntFamily, "importscanreporttoboth", If(frm.chkimportscanreporttoboth.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("importscanreportzeroamount") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("importscanreportzeroamount"))
				InsertTextNode(xDoc, xElmntFamily, "importscanreportzeroamount", If(frm.chkimportscanreportzeroamount.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("saveemployeeemploymentscanintowos") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("saveemployeeemploymentscanintowos"))
				InsertTextNode(xDoc, xElmntFamily, "saveemployeeemploymentscanintowos", If(frm.chksaveemployeeemploymentscanintowos.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("saveemployeereportscanintowos") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("saveemployeereportscanintowos"))
				InsertTextNode(xDoc, xElmntFamily, "saveemployeereportscanintowos", If(frm.chksaveemployeereportscanintowos.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("saveemployeepayrollscanintowos") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("saveemployeepayrollscanintowos"))
				InsertTextNode(xDoc, xElmntFamily, "saveemployeepayrollscanintowos", If(frm.chksaveemployeepayrollscanintowos.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("savecustomeremploymentscanintowos") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("savecustomeremploymentscanintowos"))
				InsertTextNode(xDoc, xElmntFamily, "savecustomeremploymentscanintowos", If(frm.chksavecustomeremploymentscanintowos.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("savecustomerreportscanintowos") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("savecustomerreportscanintowos"))
				InsertTextNode(xDoc, xElmntFamily, "savecustomerreportscanintowos", If(frm.chksavecustomerreportscanintowos.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("savecustomerinvoicescanintowos") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("savecustomerinvoicescanintowos"))
				InsertTextNode(xDoc, xElmntFamily, "savecustomerinvoicescanintowos", If(frm.chksavecustomerinvoicescanintowos.Checked, "true", "false"))

				If xElmntFamily.SelectSingleNode("autofilterconditionnr") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("autofilterconditionnr"))
				InsertTextNode(xDoc, xElmntFamily, "autofilterconditionnr", If(String.IsNullOrWhiteSpace(frm.cboautofilterconditionnr.EditValue), "0", frm.cboautofilterconditionnr.EditValue))

				If xElmntFamily.SelectSingleNode("autofilterconditiondate") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("autofilterconditiondate"))
				InsertTextNode(xDoc, xElmntFamily, "autofilterconditiondate", If(String.IsNullOrWhiteSpace(frm.cboAutoFilterConditionDate.EditValue), "0", frm.cboAutoFilterConditionDate.EditValue))
				If xElmntFamily.SelectSingleNode("allowautofilterconditionchange") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("allowautofilterconditionchange"))
				InsertTextNode(xDoc, xElmntFamily, "allowautofilterconditionchange", If(frm.chkAllowAutoFilterConditionChange.Checked, "true", "false"))



				If xElmntFamily.SelectSingleNode("EnableLLDebug") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("EnableLLDebug"))
				InsertTextNode(xDoc, xElmntFamily, "EnableLLDebug", If(frm.chkGLLDebug.Checked, 1, 0))

				If xElmntFamily.SelectSingleNode("mandantcolor") IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("mandantcolor"))
				Dim mandantcolor As String = frm.ceMandantColor.Color.Name
				If frm.ceMandantColor.EditValue = Nothing Then
					mandantcolor = String.Empty
				End If
				InsertTextNode(xDoc, xElmntFamily, "mandantcolor", mandantcolor)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.LLDebug-Abschnitt speichern. {1}", strMethodeName, ex.Message))
			result = False
		End Try

		Try
			xNode = xDoc.SelectSingleNode("*//Mailing")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Mailing", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				Dim strNodename As String = String.Empty

				Try
					strNodename = "SMTP-Server"
					If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
				 xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
					InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txtSMTP.Text)

					strNodename = "smtp-deliverymethode"
					If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
					InsertTextNode(xDoc, xElmntFamily, strNodename, frm.cboSmtpDeliveryMethod.EditValue)

					strNodename = "SMTP-Port"
					If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
					InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txtSMTPPort.Text)

					strNodename = "smtp-enablessl"
					If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
					InsertTextNode(xDoc, xElmntFamily, strNodename, frm.chkSSL.Checked)

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.SMTP-Abschnitt speichern. {1}", strMethodeName, ex.Message))

				End Try

				Try

					strNodename = "Fax-Forwarder".ToLower
					If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
				 xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
					InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txtFaxForwarder.Text)

					strNodename = "davidserver".ToLower
					If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
				 xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
					InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txtDavidfaxserver.Text)
					_ClsReg.SetINIString(strMDIniFile, "Export Db", "DavidServer", frm.txtDavidfaxserver.Text)

					' extenstion
					If xElmntFamily.SelectSingleNode("faxextension") IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("faxextension"))
					InsertTextNode(xDoc, xElmntFamily, "faxextension", frm.txtFaxExtension.Text)
					' Forwarder
					If xElmntFamily.SelectSingleNode("faxforwarder") IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("faxforwarder"))
					InsertTextNode(xDoc, xElmntFamily, "faxforwarder", frm.txtFaxForwarder.Text)

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.faxextension-Abschnitt speichern. {1}", strMethodeName, ex.Message))
					result = False

				End Try

				Try
					If ModulConstants.UserData.UserNr = 1 Then
						Dim _clsProg As New SPProgUtility.ClsProgSettingPath
						Try
							' ecalljobid
							strNodename = "ecalljobid"
							If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
										xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
							InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallJobID.Text)

						Catch ex As Exception
							m_Logger.LogError(String.Format("{0}.faxuser und pw-Abschnitt speichern. {1}", strMethodeName, ex.Message))

						End Try

						' fromtext
						strNodename = "ecallfromtext"
						If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
									xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
						InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallFromText.Text)
						' headerid
						strNodename = "ecallheaderid"
						If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
									xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
						InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallHeaderID.Text)
						' headerinfo
						strNodename = "ecallheaderinfo"
						If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
									xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
						InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallHeaderInfo.Text)
						' subject
						strNodename = "ecallsubject"
						If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
									xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
						InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallSubject.Text)

						' Notification
						strNodename = "ecallnotification"
						If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
									xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
						InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallNotification.Text)
						' token
						strNodename = "ecalltoken"
						If xElmntFamily.SelectSingleNode(strNodename) IsNot Nothing Then _
									xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNodename))
						InsertTextNode(xDoc, xElmntFamily, strNodename, frm.txteCallToken.Text)

					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Userdata-Abschnitt speichern. {1}", strMethodeName, ex.Message))
					result = False

				End Try

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Mailing-Abschnitt speichern. {1}", strMethodeName, ex.Message))
			result = False

		End Try


		If ModulConstants.UserData.UserNr = 1 Then
			Try
				xNode = xDoc.SelectSingleNode("*//Licencing")
				If xNode Is Nothing Then
					xNode = xDoc.CreateNode(XmlNodeType.Element, "Licencing", "")
					xDoc.DocumentElement.AppendChild(xNode)
				End If
				If xNode IsNot Nothing Then
					If TypeOf xNode Is XmlElement Then
						xElmntFamily = CType(xNode, XmlElement)
					End If
					' Sesam
					Dim strKey As String = "sesam"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkSesam.Checked, "+{172195F3-8C5A-41df-9018-6D3527CFD807}+", ""))
					' Abacus
					strKey = "abacus"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkAbacus.Checked, "+{B2705597-5217-4778-96F6-2998CCEF0598}+", ""))
					' Swifac
					strKey = "swifac"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkSwifac.Checked, "+{AC1200CE-AE49-4f40-A28E-EA5891449595}+", ""))

					' Comatic
					strKey = "comatic"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkComatic.Checked, "+{4401D7E1-D512-420d-8822-2106456B33C0}+", ""))

					' Comatic
					strKey = "cresus"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkCresus.Checked, "+{8BCF3299-F2C9-4EA6-873C-CD2E9B7317A4}+", ""))

					' KMUFactoring
					strKey = "kmufactoring"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkKMUFactoring.Checked, "+{799327A2-E69D-4021-AF83-072CA0468AAE}+", ""))
					' CSOPListe
					strKey = "csoplist"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkCSOPList.Checked, "+{982E690D-39E4-4D8C-9028-01F2714E3A49}+", ""))
					' parifond
					strKey = "parifond"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkParifond.Checked, "+{9E9ACBD2-37FE-4632-902A-F7252B04DCE8}+", ""))

					' Scan DropIN
					strKey = "scandropin"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkScanDropIN.Checked, "true", "false"))

					' CV DropIN
					strKey = "cvdropin"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkCVDropIn.Checked, "true", "false"))

					' profilmatcher
					strKey = "pmsearch"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkpmsearch.Checked, "true", "false"))

					' allowedemployeeweeklypayment
					strKey = "allowedemployeeweeklypayment"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkallowedemployeeweeklypayment.Checked, "true", "false"))

					' Deltavista
					' Delta-Vista RefNr
					strKey = "customermng_deltavistaSolvencyCheckReferenceNumber"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, m_utility.ToBase64(frm.txt_DVRefNo.Text))

					' Delta-Vista UserName
					strKey = "customermng_deltavistaWebServiceUserName"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, m_utility.ToBase64(frm.txt_DVUSName.Text))

					' Delta-Vista PW
					strKey = "customermng_deltavistaWebServicePassword"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, m_utility.ToBase64(frm.txt_DVPW.Text))

					' Delta-Vista URL
					strKey = "customermng_deltavistaWebServiceUrl"
					If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then _
								xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
					InsertTextNode(xDoc, xElmntFamily, strKey, m_utility.ToBase64(frm.txt_DVURL.Text))


				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Licencing-Abschnitt speichern. {1}", strMethodeName, ex.Message))
				result = False

			End Try

		End If

		Try
			xDoc.Save(strXMLFile)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.File speichern. {1}", strMethodeName, ex.Message))
			result = False

		End Try


		Return result

	End Function

	Function AddOrUpdateFieldLabelNode(ByVal xDoc As XmlDocument, ByVal strGuid As String, ByVal strMainKey As String, ByVal KeyValue As String) As Boolean
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing
		Dim strKeyName As String = String.Empty
		strKeyName = "CtlLabel"

		xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
		If xNode Is Nothing Then
			Dim newNode As Xml.XmlElement = xDoc.CreateElement("Control")

			newNode.SetAttribute("Name", strGuid)
			xDoc.DocumentElement.AppendChild(newNode)
			xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
		End If

		If xNode IsNot Nothing Then
			If TypeOf xNode Is XmlElement Then
				xElmntFamily = CType(xNode, XmlElement)
			End If
		End If

		If TypeOf xNode Is XmlElement Then xElmntFamily = CType(xNode, XmlElement)
		If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
		'InsertTextNode(xDoc, xElmntFamily, strKeyName, (frm.txt_Telefonprivat.Text))

		With xElmntFamily
			.SetAttribute("Name", String.Format("{0}", strGuid))
			.AppendChild(xDoc.CreateElement("CtlLabel")).InnerText = KeyValue
		End With


		Return True
	End Function

	Public Function SaveUserData() As Boolean
		Dim result As Boolean = True
		Dim strXMLFile As String = UserXMLFileName
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		Try
			xDoc.Load(strXMLFile)
			xNode = xDoc.SelectSingleNode("*//programsetting")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "programsetting", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				Dim strKey As String = String.Empty

				strKey = "askonexit"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkaskonexit.Checked, "true", "false"))

			End If
			xDoc.Save(strXMLFile)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try


		Return result

	End Function

	Public Function SaveCommonModulsSetting() As Boolean
		Dim result As Boolean = True
		Dim strXMLFile As String = MDFormXMLFileName
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing


		Try
			xDoc.Load(strXMLFile)

			xNode = xDoc.SelectSingleNode("*//Layouts")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Layouts", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				Dim strKey As String = String.Empty

				strKey = "openemployeeformmorethanonce"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkopenemployeeformmorethanonce.Checked, "true", "false"))

				strKey = "opencustomerformmorethanonce"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkopencustomerformmorethanonce.Checked, "true", "false"))

				strKey = "openeinsatzformmorethanonce"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkopeneinsatzformmorethanonce.Checked, "true", "false"))

				strKey = "openreportsformmorethanonce"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkopenreportsformmorethanonce.Checked, "true", "false"))

				strKey = "openadvancedpaymentformmorethanonce"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkopenadvancedpaymentformmorethanonce.Checked, "true", "false"))

				strKey = "openinvoiceformmorethanonce"
				If xElmntFamily.SelectSingleNode(strKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKey))
				InsertTextNode(xDoc, xElmntFamily, strKey, If(frm.chkopeninvoiceformmorethanonce.Checked, "true", "false"))


				xDoc.Save(strXMLFile)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try


		Return result

	End Function



End Class



''' <summary>
''' autofiltercondition view data.
''' </summary>
Public Class AutoFilterConditionViewData

	Public Property DisplayText As String
	Public Property Value As Integer

End Class

Public Class SelectionViewData

	Public Property Value As Integer
	Public Property DisplayText As String

End Class
