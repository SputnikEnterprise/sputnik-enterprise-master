
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.Infrastructure.LOGWriter
Imports System.IO
Imports DevExpress.XtraSplashScreen
Imports System.Net.Mail

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.KD.CPersonMng.UI
Imports SP.DatabaseAccess.Propose
Imports SPProgUtility.Mandanten

Public Class OfferMessages



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

#Region "private fields"

	Private Shared m_Logger As ILogger = New Logger()
	Private Shared m_LogWriter As LOGWriter = New LOGWriter()


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
	Private _ClsLog As SPProgUtility.ClsEventLog


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


	Private m_ExportedFilename As String
	Private m_SuccessfullSentData As Integer
	Private m_WarningSentData As Integer
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

	Private m_SmtpServer As String
	Private m_SmtpPort As String

	Private m_AdvisorEmailAddress As String
	Private m_EmailTemplateFilename As String
	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFile As String
	Private m_MandantUserXMLFile As String

	Private m_MandantSetting As String
	Private m_PayrollSetting As String
	Private m_InvoiceSetting As String
	Private m_MailingSetting As String
	Private m_TemplatesSetting As String

	Private m_TemplateFoler As String

	Private m_CurrentCustomerNumber As Integer?
	Private m_CurrentCResponsibleNumber As Integer?
	Private m_CurrentEMailAddress As String

	Private m_TemplateParser As TemplateFieldParser
	Private m_SendingData As SendingData
	Private m_DocumentData As DocumentData

	Private m_CurrentUserData As UserAndMandantPrintData
	Private m_MailReceiverData As IEnumerable(Of CustomerBulkEMailData)

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant
		_ClsLog = New SPProgUtility.ClsEventLog

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_TableSettingDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_AdvisorEmailAddress = LoadUserEMailAddress

		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)
		If System.IO.File.Exists(m_MandantXMLFile) Then
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If

		m_MailingSetting = String.Format(MANDANT_XML_SETTING_MAILING, m_InitializationData.MDData.MDNr)
		m_TemplatesSetting = String.Format(MANDANT_XML_SETTING_TEMPLATES, m_InitializationData.MDData.MDNr)
		m_SmtpServer = EMailSMTPServer
		m_SmtpPort = EMailSMTPServerPort
		m_MessageGuid = CreateNewMessageGuid

	End Sub


#End Region


#Region "private property"

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

	Private ReadOnly Property EMailEnableSSL As Boolean
		Get
			Dim value As Boolean = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/smtp-enablessl", m_MailingSetting)), False)

			Return value
		End Get
	End Property

	Private ReadOnly Property DeliveryMethod As SmtpDeliveryMethod
		Get
			Dim value As New SmtpDeliveryMethod

			Dim deliveryMethode As Integer? = Val(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/smtp-deliverymethode", m_MailingSetting)))
			If deliveryMethode = 0 Then Return Nothing
			If deliveryMethode = 1 Then Return SmtpDeliveryMethod.Network
			If deliveryMethode = 2 Then Return SmtpDeliveryMethod.PickupDirectoryFromIis
			If deliveryMethode = 3 Then Return SmtpDeliveryMethod.SpecifiedPickupDirectory


			Return value
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


	Private ReadOnly Property CreateNewMessageGuid As String
		Get
			Return Guid.NewGuid.ToString()
		End Get
	End Property

#End Region


#Region "Public Properties"

	Public Property CommonLOGFilename As String
	Public Property CurrentofferNumber As Integer
	Public Property MailTemplateFilename As String
	Public Property MailReceiverData As IEnumerable(Of CustomerBulkEMailData)
	Public Property SelectedRecipientFieldData As RecipientFieldData
	Public Property SelectedTemplateData As DocumentData
	Public Property SendMessagesAsStaging As Boolean
	Public Property DeliverEMailWithHighPriority As Boolean

	Public ReadOnly Property GetSendingLogFileName As String
		Get
			Return m_LOGFileName
		End Get
	End Property


#End Region

#Region "Public Methodes"

	Public Function SendAssignedOfferToSelectedCustomers() As Boolean
		Dim result As Boolean = True

		m_SuccessfullSentData = 0
		m_ErrorSentData = 0
		m_WarningSentData = 0

		m_LOGFileName = CommonLOGFilename
		m_LogWriter.CurrentLogFileName = m_LOGFileName

		m_AdvisorEmailAddress = LoadUserEMailAddress
		m_EmailTemplateFilename = MailTemplateFilename
		m_CurrentOfferNumber = CurrentofferNumber
		m_MailReceiverData = MailReceiverData
		m_AssignedRecipientFieldData = SelectedRecipientFieldData
		m_DocumentData = SelectedTemplateData
		m_SendAsStaging = SendMessagesAsStaging
		m_EMailPriority = DeliverEMailWithHighPriority


		m_MessageGuid = CreateNewMessageGuid
		result = result AndAlso LoadAssignedOfferAdvisorData()
		result = result AndAlso PerformSendingEmailCallAsync()

		m_LogWriter.WriteTempLogFile(String.Format("Erfolgreich gesendete Nachrichten: {0}", m_SuccessfullSentData), m_LOGFileName)
		m_LogWriter.WriteTempLogFile(String.Format("Duplikat-Nachrichten: {1}{0}<br>", vbNewLine, m_WarningSentData), m_LOGFileName)
		m_LogWriter.WriteTempLogFile(String.Format("Fehlerhaft gesendete Nachrichten: {1}{0}<br>", vbNewLine, m_ErrorSentData), m_LOGFileName)

		Return result
	End Function

#End Region


	Private Function LoadAssignedOfferAdvisorData() As Boolean

		m_CurrentUserData = m_ListingDatabaseAccess.LoadUserInformationData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
		If m_CurrentUserData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzer Daten konnten nicht geladen werden."))

			Return False
		End If

		Return Not m_CurrentUserData Is Nothing
	End Function

	Private Function LoadStagingEMailAddress() As String
		Dim result = m_InitializationData.UserData.UserMDeMail
		If String.IsNullOrWhiteSpace(result) Then result = m_InitializationData.UserData.UsereMail

		If String.IsNullOrWhiteSpace(result) Then
			m_Logger.LogError(String.Format("GeteMailFrom: email address is empty."))
			m_UtilityUI.ShowErrorDialog("Der Absender wurde nicht gefunden! Bitte definieren Sie eine EMail-Adresse in der Benutzerverwaltung.")
		End If

		Return result

	End Function

	Private Function PerformSendingEmailCallAsync() As Boolean
		Dim result As Boolean = True
		Dim strToField As String = ""

		Dim strOldReceiver As String = ";"
		Dim bVersandResult As Boolean = True
		Dim strExportedFilename As String = String.Empty
		Dim stagingEMailAddress As String = LoadStagingEMailAddress()

		m_CurrentCustomerNumber = Nothing
		m_CurrentCResponsibleNumber = Nothing

		result = result AndAlso ValidateData()
		If Not result Then Return False

		m_TemplateFoler = m_InitializationData.MDData.MDTemplatePath
		m_TemplateParser = New TemplateFieldParser(m_InitializationData)
		m_SendingData = New SendingData With {.OffersNumber = m_CurrentOfferNumber, .AdvisorNumber = m_InitializationData.UserData.UserNr}
		m_LogWriter.WriteTempLogFile(String.Format("<b>Programmstart:</b>: {0} ({1}) > {2}<br>Offerte: {3} an: {4}<br>",
												   m_InitializationData.MDData.MDName, m_InitializationData.MDData.MDCity, m_InitializationData.UserData.UserFullName,
												   m_CurrentOfferNumber, SelectedRecipientFieldData.RecordLabel),
									 m_LOGFileName)


		' In der Offerte sind Kandidaten vorhanden
		Dim existsEmployee As Boolean?
		existsEmployee = m_ProposeDatabaseAccess.CheckIfEmployeeInOfferExists(m_InitializationData.MDData.MDNr, m_CurrentOfferNumber)
		m_Logger.LogDebug(String.Format("Offertennummer: {0} | bExistDocFile: {1}", m_CurrentOfferNumber, existsEmployee.GetValueOrDefault(False)))
		If existsEmployee Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Ihre Kandidaten Daten konnten nicht geladen werden.")

			Return False
		End If
		result = result AndAlso LoadAssignedOfferAdvisorData()

		m_CurrentCustomerNumber = 0
		m_CurrentCResponsibleNumber = 0
		m_TemplateParser.CurrentUserData = m_CurrentUserData


		Try
			strToField = m_AssignedRecipientFieldData.RecordValue

			Dim i As Integer = 0

			For Each customer In m_MailReceiverData
				i += 1

				Dim modulName As String = customer.ModulName
				m_ExportedFilename = String.Empty
				m_CurrentCustomerNumber = customer.KDNr
				m_CurrentCResponsibleNumber = customer.ZHDRecNr
				Dim assignedReceiverName As String = customer.Firma1

				Dim assignedEMail As String = String.Format("{0}", customer.ReceiverEMail).Replace(" ", "").Replace(vbNewLine, "").Replace(vbLf, "")
				If m_SendAsStaging Then assignedEMail = stagingEMailAddress
				If Not m_Utility.IsEMailAddressValid(assignedEMail) OrElse String.IsNullOrEmpty(assignedEMail) Then
					m_Logger.LogWarning(String.Format("email address ({0}) might be wrong or empty!", assignedEMail))

					Continue For
				End If
				Try
					m_LogWriter.WriteTempLogFile(String.Format("<b>Starte den Versand für {0} an {1} ({2})</b>", modulName, assignedReceiverName, assignedEMail), m_LOGFileName)

					result = result AndAlso Not strOldReceiver.ToLower.Contains(assignedEMail.ToLower)
					If Not result Then m_LogWriter.WriteTempLogFile(String.Format("<b>Warnung:</b> Gleiche Adresse in dieser Sendung gefunden. {0}: {1} >>> {2}", modulName, assignedReceiverName, assignedEMail), m_LOGFileName)
					If Not result Then Throw New Exception("jumping: same address in same send job")

					If existsEmployee Then result = result AndAlso existsEmployee AndAlso IsPDFTemplateCreated(m_CurrentCustomerNumber, m_CurrentCResponsibleNumber)
					If Not result Then Throw New Exception(String.Format("jumping: existsEmployee-IsPDFTemplateCreated: {0}", assignedEMail.ToLower))

					m_CurrentEMailAddress = assignedEMail
					m_SendingData.CustomerNumber = m_CurrentCustomerNumber
					m_SendingData.CResponsibleNumber = m_CurrentCResponsibleNumber

					m_SendingData.SendCreatedTemplateAttachments = Not String.IsNullOrWhiteSpace(m_ExportedFilename) AndAlso File.Exists(m_ExportedFilename)
					If m_SendingData.SendCreatedTemplateAttachments Then m_SendingData.SendCreatedTemplateAttachmentFilename = m_ExportedFilename
					m_SendingData.SendScannedAttachments = String.IsNullOrWhiteSpace(m_ExportedFilename) OrElse Not File.Exists(m_ExportedFilename)
					m_SendingData.SendScannedAttachmentFilename = String.Empty

					m_Logger.LogDebug(String.Format("StartWithMailing wird gestartet..."))
					result = result AndAlso LoadOffersDataForSendingMail()

					strOldReceiver &= assignedEMail & ";"

					If Not result Then Throw New Exception("jumping: LoadOffersDataForSendingMail")
					If result Then m_LogWriter.WriteTempLogFile(String.Format("<b>Versand: OK</b>"), m_LOGFileName)

					m_SuccessfullSentData += If(result, 1, 0)

				Catch ex_1 As Exception
					m_Logger.LogError(String.Format("{0}", ex_1.ToString))
					If ex_1.ToString.Contains("Warnung:") Then
						m_WarningSentData += 1
					Else
						m_ErrorSentData += 1
					End If

				End Try


				result = True

				If m_SendAsStaging Then Exit For
			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_LogWriter.WriteTempLogFile(String.Format("***DoWork_0: {0}", ex.Message & vbNewLine & "KDNr: " & m_CurrentCustomerNumber & vbCrLf & m_CurrentCResponsibleNumber & vbCrLf), m_LOGFileName)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		Return result
	End Function

	Function ValidateData() As Boolean

		Dim result As Boolean = True

		If String.IsNullOrWhiteSpace(m_AdvisorEmailAddress) Then
			m_Logger.LogDebug(String.Format("Leere Adresse..."))

			m_UtilityUI.ShowErrorDialog(String.Format("Sie haben keine gültigen Absender für EMail-Versand definiert.{0}Das Programm wird beendet.", vbNewLine))
			Return False
		End If

		If String.IsNullOrWhiteSpace(m_SmtpServer) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Sie haben keinen gültigen SMTP-Server für EMail-Versand definiert.{0}Das Programm wird beendet.", vbNewLine))

			Return False
		End If
		If m_AssignedRecipientFieldData Is Nothing OrElse m_AssignedRecipientFieldData.RecordValue = RecordValueEnum.NOTDEFINED Then
			m_LogWriter.WriteTempLogFile(String.Format("<color=red><b>Fehler:</b> Keine Empfänger-Art wurde gewählt.{0}An welche Adresse soll die Nachricht versendet werden?</color>", vbNewLine), m_LOGFileName)

			Return False
		End If

		Return result
	End Function

	Private Function IsPDFTemplateCreated(ByVal customerNumber As Integer, ByVal responsibleNumber As Integer) As Boolean
		Dim result As Boolean = True
		Dim tplData = m_DocumentData

		If tplData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Vorlage ausgewählt."))

			Return False
		End If

		Dim _setting As New SPS.Listing.Print.Utility.ClsLLOfferSearchPrintSetting With {.m_initData = m_InitializationData,
			.offerNumber = m_CurrentOfferNumber,
			.customerNumber = customerNumber, .cresponsibleNumber = responsibleNumber,
			.JobNr2Print = tplData.JobNr, .ShowAsExport = True, .ShowAsDesgin = False}

		Dim printTemplate As New SPS.Listing.Print.Utility.OfferSearchListing.ClsPrintOfferSearchList(_setting)
		m_ExportedFilename = printTemplate.PrintOfferTemplate()

		m_Logger.LogDebug(String.Format("m_ExportedFilename: {0}", m_ExportedFilename))
		result = Not String.IsNullOrWhiteSpace(m_ExportedFilename) AndAlso File.Exists(m_ExportedFilename)
		If Not result Then m_LogWriter.WriteTempLogFile(String.Format("<color=red><b>Fehler:</b> Vorlage konnte nicht erstellt werden. KDNr: {0} >>> ZHDNr: {1}<color>", customerNumber, responsibleNumber), m_LOGFileName)


		Return result

	End Function


End Class

Public Class SendingData

	Public Property OffersNumber As Integer?
	Public Property AdvisorNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property CResponsibleNumber As Integer?

	Public Property EMailSubject As String
	Public Property EMailTo As String
	Public Property EMailFrom As String

	Public Property SendScannedAttachments As Boolean?
	Public Property SendCreatedTemplateAttachments As Boolean?
	Public Property SendScannedAttachmentFilename As String
	Public Property SendCreatedTemplateAttachmentFilename As String


End Class

#Region "Helper classes"

Public Class RecipientFieldData
	Public Property RecordValue As RecordValueEnum
	Public Property RecordLabel As String
End Class

Public Enum RecordValueEnum
	NOTDEFINED
	CUSTOMER
	RESPONSIBLEPERSON
	BOTH
End Enum

#End Region
