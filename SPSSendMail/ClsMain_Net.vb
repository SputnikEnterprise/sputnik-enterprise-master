
Imports DevExpress.XtraSplashScreen
Imports System.Data.SqlClient
Imports System.IO
Imports SP.Infrastructure.Logging
Imports SPProgUtility
Imports SP.Infrastructure.UI
Imports System.Collections.Generic
Imports System.Net.Mail
Imports System.Net

Public Class ClsMain_Net

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath ' SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Private m_mandant As SPProgUtility.Mandanten.Mandant

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_Logger As ILogger = New Logger()


	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFile As String
	Private m_MandantUserXMLFile As String

	Private m_MandantSetting As String
	Private m_PayrollSetting As String
	Private m_InvoiceSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_MailingSetting As String
	Private m_TemplatesSetting As String
	Private m_UtilityUI As UtilityUI

	Private m_SMTPServer As String
	Private m_SMTPServerPort As String
	Private m_UserEMailAddress As String

#Region "private consts"

	Private Const MANDANT_XML_SETTING_MAILING As String = "MD_{0}/Mailing"
	Private Const MANDANT_XML_SETTING_TEMPLATES As String = "MD_{0}/Templates"


	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_VER_GUID As String = "MD_{0}/Export/Ver_SPUser_ID"

	Private Const MANDANT_XML_SETTING_COCKPIT_EMAIL_TEMPLATE As String = "MD_{0}/Templates/cockpit-email-template"
	Private Const MANDANT_XML_SETTING_COCKPIT_URL As String = "MD_{0}/Templates/cockpit-url"
	Private Const MANDANT_XML_SETTING_COCKPIT_PICTURE As String = "MD_{0}/Templates/cockpit-picture"


	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"

	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"
	Private Const FORM_XML_PRINT_PAYROLL_KEY As String = "Forms_Normaly/Lohnbuchhaltung"

	Private Const FORM_XML_REQUIREDFIELDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

	Public Sub New()

		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath
		m_mandant = New SPProgUtility.Mandanten.Mandant
		m_UtilityUI = New UtilityUI

		m_InitializationData = CreateInitialData(m_mandant.GetDefaultMDNr, m_mandant.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = m_InitializationData
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_Translate = ClsDataDetail.m_Translate

		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)
		If System.IO.File.Exists(m_MandantXMLFile) Then
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		m_MailingSetting = String.Format(MANDANT_XML_SETTING_MAILING, m_InitializationData.MDData.MDNr)
		m_TemplatesSetting = String.Format(MANDANT_XML_SETTING_TEMPLATES, m_InitializationData.MDData.MDNr)

		m_SMTPServer = EMailSMTPServer
		m_SMTPServerPort = EMailSMTPServerPort

	End Sub

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_Translate = ClsDataDetail.m_Translate

		m_UtilityUI = New UtilityUI

		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath
		m_mandant = New SPProgUtility.Mandanten.Mandant


	End Sub

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)
		If System.IO.File.Exists(m_MandantXMLFile) Then
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		m_MailingSetting = String.Format(MANDANT_XML_SETTING_MAILING, m_InitializationData.MDData.MDNr)
		m_TemplatesSetting = String.Format(MANDANT_XML_SETTING_TEMPLATES, m_InitializationData.MDData.MDNr)
		m_SMTPServer = EMailSMTPServer
		m_SMTPServerPort = EMailSMTPServerPort
		m_UserEMailAddress = LoadUserEMailAddress

		Dim logFilename As String = String.Format("{0}.{1}", ProcessGuid, "tmp")
		ClsDataDetail.GetTempLogFile = Path.Combine(m_InitializationData.UserData.spAllowedPath, logFilename)

		m_Logger.LogDebug(String.Format("ClsDataDetail.GetTempLogFile: {0}", ClsDataDetail.GetTempLogFile))
		_ClsLog.WriteTempLogFile(String.Format("Total Datensätze: {1}.{0}{2} Versand wird gestartet:{0}Von der Adresse: {3} über Smtp-Server: {4} | Port: {5}{0}",
											   vbNewLine, SendigRecordCount, m_Translate.GetSafeTranslationValue(If(SendAsStagingMessage, "Test", "Endgültiger")),
											   m_UserEMailAddress, m_SMTPServer, m_SMTPServerPort), ClsDataDetail.GetTempLogFile)



		Return result

	End Function


#Region "public properties"

	Public Property ProcessGuid As String
	Public Property SendigRecordCount As Integer
	Public Property SendAsStagingMessage As Boolean

#End Region


	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		'Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant As ClsMDData = m_mandant.GetSelectedMDData(iMDNr)
		Dim logedUserData As ClsUserData = m_mandant.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData As System.Collections.Generic.Dictionary(Of String, SPProgUtility.ClsProsonalizedData) = m_mandant.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate As System.Collections.Generic.Dictionary(Of String, SPProgUtility.ClsTranslationData) = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#Region "private properties"

	''' <summary>
	''' Gets the email smtp server.
	''' </summary>
	Private ReadOnly Property EMailSMTPServer As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SMTP-Server", m_MailingSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the email smtp server port.
	''' </summary>
	Private ReadOnly Property EMailSMTPServerPort As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/SMTP-Port", m_MailingSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the email MADoc-eMail-Betreff.
	''' </summary>
	Private ReadOnly Property EmployeeWOSDocSubject As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/MADoc-eMail-Betreff", m_MailingSetting))

			Return settingValue
		End Get
	End Property

	''' <summary>
	''' Gets the email KDDoc-eMail-Betreff.
	''' </summary>
	Private ReadOnly Property CustomerWOSDocSubject As String
		Get
			Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/KDDoc-eMail-Betreff", m_MailingSetting))

			Return settingValue
		End Get
	End Property

	Private ReadOnly Property LoadUserEMailAddress() As String
		Get
			Dim result = m_InitializationData.UserData.UserMDeMail
			If String.IsNullOrWhiteSpace(result) Then result = m_InitializationData.UserData.UsereMail

			If String.IsNullOrWhiteSpace(result) Then
				m_Logger.LogError(String.Format("LoadUserEMailAddress: email address is empty."))
				m_UtilityUI.ShowErrorDialog("Der Absender wurde nicht gefunden! Bitte definieren Sie eine EMail-Adresse in der Benutzerverwaltung.")
			End If

			Return result
		End Get
	End Property

#End Region


	Function OpenEmailClient(ByVal strTemplateFileName As String, _
																	 ByVal strEMailAdress As String, _
																	 ByVal strMailSubject As String, _
																	 ByVal strFilestoSend As String) As Boolean
		Dim _ClsFunc As New ClsDivFunc

		If Not File.Exists(strTemplateFileName) Then Return False

		Dim strBodyText As String = "&body="
		Dim strBodyTextContent As String = _ClsFunc.ParseTemplateFile(strTemplateFileName)
		If strBodyTextContent = String.Empty Then Return False

		strBodyText &= strBodyTextContent

		'string builder used for concatination
		Dim MsgBuilder As New System.Text.StringBuilder
		MsgBuilder.Append("mailto:" & strEMailAdress)
		MsgBuilder.Append("&subject=" & strMailSubject)
		MsgBuilder.Append(strBodyText)
		MsgBuilder.Append("&attachment=" & strFilestoSend & "")

		ExecuteFile(MsgBuilder.ToString)

		Return True
	End Function

	Private Function ExecuteFile(ByVal FileName As String) As Boolean
		Dim myProcess As New Process

		myProcess.StartInfo.FileName = FileName
		myProcess.StartInfo.UseShellExecute = True
		myProcess.StartInfo.RedirectStandardOutput = False
		myProcess.Start()
		myProcess.Dispose()

	End Function

	Function StartWithMailing(ByVal iOfferNr As Integer, ByVal iKDTempNr As Integer, ByVal iKDZTempNr As Integer, ByVal bSendAsTest As Boolean, ByVal streMailField As String,
														ByVal strGuid As String, Optional ByVal strFileToSend As String = "") As Boolean


		Dim streMailFrom As String = m_UserEMailAddress
		Dim bResult As Boolean
		Dim mdXMLFilename As String = m_InitializationData.MDData.MandantCurrentXMLFileName

		ClsDataDetail.CheckForMailSent = Not bSendAsTest

		ClsDataDetail.GetOffNr = iOfferNr
		ClsDataDetail.GetKDNr = iKDTempNr
		ClsDataDetail.GetZHDNr = iKDZTempNr
		ClsDataDetail.GetMANr = 0
		ClsDataDetail.GeteMailFieldToSend = streMailField
		ClsDataDetail.SendAsHtml = True
		ClsDataDetail.GetMessageGuid = strGuid

		ClsDataDetail.IsAttachedFileInd = False

		ClsDataDetail.SendWithMADoks = strFileToSend <> String.Empty
		ClsDataDetail.SendWithPDoks = strFileToSend = String.Empty
		ClsDataDetail.GeteMailFrom = streMailFrom

		If streMailFrom = String.Empty Then
			m_Logger.LogDebug(String.Format("Leere Adresse..."))

			m_UtilityUI.ShowErrorDialog(String.Format("Sie haben keine gültigen Absender für EMail-Versand definiert.{0}Das Programm wird beendet.", vbNewLine))
			Return False
		End If

		Dim strQuery As String = String.Empty
		strQuery = String.Format("//Templates/MassenOffer-eMail-Template")
		Dim strMyTplValue As String = ClsDataDetail.GetXMLValueByQuery(mdXMLFilename, strQuery, "")
		Try
			If strMyTplValue <> String.Empty Then
				strMyTplValue = String.Format("{0}{1}", _ClsProgSetting.GetMDTemplatePath, strMyTplValue)
			End If
			If Not File.Exists(strMyTplValue) Then
				m_Logger.LogDebug(String.Format("Template {0} wurde in {1} nicht gefunden.", strMyTplValue, mdXMLFilename))
				Throw New FileNotFoundException(strQuery)
			End If

		Catch ex As FileNotFoundException
			Dim strMsg As String = "Ihre E-Mail-Vorlage wurde nicht gefunden. Das Programm wird beendet.{0}"
			strMsg &= "Bitte versuchen Sie die Vorlage in der Dokumentenverwaltung -> Vorlagen:Massenofferte zu definieren."
			strMsg = String.Format(strMsg, vbNewLine)
			m_UtilityUI.ShowErrorDialog(strMsg)

			m_Logger.LogError(String.Format("{0}.", ex.ToString))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler: {0}", ex.ToString), ClsDataDetail.GetTempLogFile)

			Return False
		End Try
		ClsDataDetail.GetMailTemplateFilename = strMyTplValue
		m_Logger.LogDebug(String.Format("ClsDataDetail.GetMailTemplateFilename: {0}", ClsDataDetail.GetMailTemplateFilename))

		If String.IsNullOrWhiteSpace(m_SMTPServer) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Sie haben keinen gültigen SMTP-Server für EMail-Versand definiert.{0}Das Programm wird beendet.", vbNewLine))
			Return False
		End If


		Try
			ClsDataDetail.GetAttachmentFile = strFileToSend.Split(CChar(";"))
			m_Logger.LogDebug(String.Format("strFileToSend: {0}", strFileToSend))
			bResult = OpenConnection(bSendAsTest)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			Finalize()

		End Try


		Return bResult
	End Function




	<Obsolete("This method is deprecated.")>
	Function SendMADoceMailWithTemplate(ByVal iMANr As Integer) As String

		Dim strResult As String = String.Empty
		Return strResult
	End Function

	<Obsolete("This method is deprecated.")>
	Function SendMADoc2eMailWithTemplate(ByVal iMANr As Integer, ByVal strFileName2Send As String, ByVal strDocArt As String) As String

		Dim strResult As String = String.Empty
		Return strResult
	End Function

	<Obsolete("This method is deprecated.")>
	Function SendKDDoceMailWithTemplate(ByVal iKDNr As Integer) As String

		Dim strResult As String = String.Empty
		Return strResult
	End Function

	<Obsolete("This method is deprecated.")>
	Function SendZHDDoceMailWithTemplate(ByVal iKDNr As Integer, ByVal iZHDNr As Integer) As String

		Dim strResult As String = String.Empty
		Return strResult
	End Function

	''' <summary>
	''' dient zum Versenden vom einzelnen Offerten an Kunden / Zuständige Personen
	''' </summary>
	''' <param name="iOfferNr"></param>
	''' <param name="iMATempNr"></param>
	''' <param name="iKDTempNr"></param>
	''' <param name="iKDZTempNr"></param>
	''' <param name="bSendAsTest"></param>
	''' <param name="bSendAsHTML"></param>
	''' <param name="bWithMADoks"></param>
	''' <param name="bWithPresentation"></param>
	''' <param name="streMailField"></param>
	''' <param name="strGuid"></param>
	''' <param name="strFromAddress"></param>
	''' <param name="strTemplateFilename"></param>
	''' <param name="strFileToSend"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function SendOfferWithMail(ByVal iOfferNr As Integer, _
													ByVal iMATempNr As Integer, _
													ByVal iKDTempNr As Integer, _
													ByVal iKDZTempNr As Integer, _
													ByVal bSendAsTest As Boolean, _
													ByVal bSendAsHTML As Boolean, _
													ByVal bWithMADoks As Boolean, _
													ByVal bWithPresentation As Boolean, _
													ByVal streMailField As String, _
													ByVal strGuid As String, _
													Optional ByVal strFromAddress As String = "", _
													Optional ByVal strTemplateFilename As String = "", _
													Optional ByVal strFileToSend As String = "") As Boolean
		Dim frmTest As New frmSendOffer(m_InitializationData)
		'Dim strSmtp As String = _ClsProgSetting.GetSmtpServer()

		Dim streMailFrom As String = If(strFromAddress = String.Empty, GeteMailFrom(), strFromAddress)
		Dim bResult As Boolean
		Dim strQuery As String = String.Empty
		m_Logger.LogDebug("SendOfferWithMail: ist gestartet...")

		If String.IsNullOrWhiteSpace(m_SMTPServer) Then
			If Not LoadData() Then Return False

			'm_MailingSetting = String.Format(MANDANT_XML_SETTING_MAILING, m_InitializationData.MDData.MDNr)
			'm_TemplatesSetting = String.Format(MANDANT_XML_SETTING_TEMPLATES, m_InitializationData.MDData.MDNr)
			'm_SMTPServer = EMailSMTPServer
			'm_SMTPServerPort = EMailSMTPServerPort
			'm_UserEMailAddress = LoadUserEMailAddress

			If String.IsNullOrWhiteSpace(m_SMTPServer) Then
				m_Logger.LogWarning("Empty SMTP-Server!!!")
				MsgBox(String.Format("Sie haben keinen gültigen SMTP-Server für EMail-Versand definiert.{0}" &
													 "Das Programm wird beendet.", vbLf),
						MsgBoxStyle.Critical, "E-Mail-Versand")
				Return False
			End If

		End If
		If streMailFrom = String.Empty Then
			m_Logger.LogWarning("Empty MailAddress!!!")
			MsgBox(String.Format("Sie haben keine gültigen Absender für EMail-Versand definiert.{0} Das Programm wird beendet.", vbLf), _
						MsgBoxStyle.Critical, "E-Mail-Versand")
			Return False
		End If
		ClsDataDetail.GeteMailFrom = streMailFrom

		ClsDataDetail.GetOffNr = iOfferNr
		ClsDataDetail.GetKDNr = iKDTempNr
		ClsDataDetail.GetZHDNr = iKDZTempNr
		ClsDataDetail.GetMANr = iMATempNr
		ClsDataDetail.GeteMailFieldToSend = streMailField
		ClsDataDetail.GetMessageGuid = strGuid
		ClsDataDetail.GetTempLogFile = String.Format("{0}{1}.{2}", _ClsProgSetting.GetSpSFiles2DeletePath, strGuid, "tmp")

		ClsDataDetail.IsAttachedFileInd = True
		ClsDataDetail.SendWithMADoks = bWithMADoks
		ClsDataDetail.SendWithPDoks = bWithPresentation
		ClsDataDetail.SendAsHtml = True
		If strTemplateFilename = String.Empty Or Not File.Exists(strTemplateFilename) Then
			strQuery = String.Format("//Templates/eMail-Template")
			ClsDataDetail.GetMailTemplateFilename = ClsDataDetail.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, _
																																							 strQuery, "")
		Else
			ClsDataDetail.GetMailTemplateFilename = strTemplateFilename
		End If

		Try
			'Dim o2Open As New SPSOfferUtility_Net.ClsMain_Net

			m_Logger.LogDebug("Created...")
			'Dim strjobnr As String = "15.1.1"
			Dim strjobnr = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/OffMail_tplDocNr", m_TemplatesSetting))
			If Not String.IsNullOrWhiteSpace(strjobnr) Then
				Dim m_init As SP.Infrastructure.Initialization.InitializeClass = CreateInitialData(0, 0)

				Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = m_init,
																																												 .customerNumber = ClsDataDetail.GetKDNr,
																																												 .cresponsibleNumber = ClsDataDetail.GetZHDNr,
																																												 .JobNr2Print = strjobnr,
																																												 .offerNumber = iOfferNr,
																																												 .ShowAsExport = True}
				'.ClsPrintOffers(m_init, iOfferNr, Nothing, ClsDataDetail.GetKDNr, ClsDataDetail.GetZHDNr, True, strjobnr)
				Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
				Dim strOfferblattFileName As String = printTemplate.PrintOfferTemplate()

				'			Dim strOfferblattFileName As String = CStr(o2Open.PrintLLDocToFile(iOfferNr, 0, ClsDataDetail.GetKDNr, ClsDataDetail.GetZHDNr, False, True, strjobnr))

				If _ClsProgSetting.GetLogedUSNr = 1 Then m_Logger.LogInfo(String.Format("Filename: {0}", strOfferblattFileName))
				strFileToSend = strOfferblattFileName

				frmTest.chkOffblatt.Checked = My.Settings.bWithOffblatt

			Else
				frmTest.chkOffblatt.Checked = False
				frmTest.chkOffblatt.Enabled = False

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try
		ClsDataDetail.GetAttachmentFile = strFileToSend.Split(CChar(";"))

		Try
			frmTest.chkOffPBlatt.Checked = My.Settings.bWithOffPBlatt
			frmTest.chkOffMABlatt.Checked = My.Settings.bWithOffMABlatt

			frmTest.Show()
			frmTest.BringToFront()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			MsgBox(Err.Description, MsgBoxStyle.Critical, "SendOfferWithMail")

		Finally
			Finalize()

		End Try

		Return bResult

	End Function

	Function StartAllMailing(ByVal iOfferNr As Integer, ByVal bSendAsTest As Boolean, Optional ByVal strFileToSend As String = "") As Boolean

		ClsDataDetail.CheckForMailSent = Not bSendAsTest

		ClsDataDetail.GetOffNr = iOfferNr
		ClsDataDetail.GetMailTemplateFilename = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile(), "Mailing", "eMail-Template")

		ClsDataDetail.GetAttachmentFile = strFileToSend.Split(CChar(";"))
		OpenConnection(bSendAsTest)

		Return True
	End Function

	Function OpenMailingList(ByVal iCommArt As Short, ByVal strGuid As String) As Boolean
		Dim frm As New frmMessageList

		Try
			ClsDataDetail.GetMessageGuid = strGuid
			frm.Show()
			frm.cboMessageArt.SelectedIndex = iCommArt

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenMailingList")

		End Try

		Return True
	End Function

	Function IsMessageAlreadySent(ByVal streMailTo As String, _
																ByVal strSubject As String, _
																ByVal strBody As String, _
																ByVal iKDNr As Integer, _
																ByVal strGuid As String, _
																ByVal bSendAsTest As Boolean) As Boolean

		Return IsMyMessageAlreadySent(streMailTo, strSubject, strBody, iKDNr, strGuid, bSendAsTest)
	End Function


	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub




#Region "Helper methodes"


#End Region

#Region "Helper classes"

	Private Class MailSendValue
		Public Property Value As Boolean
		Public Property ValueLable As String

	End Class

#End Region


End Class
