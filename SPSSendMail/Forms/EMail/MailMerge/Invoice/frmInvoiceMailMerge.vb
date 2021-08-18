
Imports DevExpress.LookAndFeel
Imports DevExpress.Office.Services
Imports DevExpress.Office.Utils
Imports DevExpress.Pdf

Imports DevExpress.Utils

Imports DevExpress.XtraBars
Imports DevExpress.XtraRichEdit

Imports DevExpress.XtraRichEdit.Export
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Applicant.DataObjects

Imports SP.DatabaseAccess.Common

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Applicant

Imports SP.DatabaseAccess.Listing

Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.PayrollMng


Imports SP.DatabaseAccess.Propose
Imports SP.DatabaseAccess.Propose.DataObjects
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.Vacancy
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Text
Imports System.Text.RegularExpressions
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraEditors.ViewInfo
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.Listing.DataObjects.OutgoingEMailData
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Namespace RichEditSendMail

	Public Class frmInvoiceEMailMerge

#Region "private consts"

		Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
		Private Const MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING As String = "MD_{0}/Mailing"
		Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"

		Private Const REGEX_USAnrede As String = "\{(?i)TMPL_VAR name=\'USAnrede\'\}"
		Private Const REGEX_USNachname As String = "\{(?i)TMPL_VAR name=\'USNachname\'\}"
		Private Const REGEX_USVorname As String = "\{(?i)TMPL_VAR name=\'USVorname\'\}"
		Private Const REGEX_USTelefon As String = "\{(?i)TMPL_VAR name=\'USTelefon\'\}"
		Private Const REGEX_USTelefax As String = "\{(?i)TMPL_VAR name=\'USTelefax\'\}"
		Private Const REGEX_USNatel As String = "\{(?i)TMPL_VAR name=\'USNatel\'\}"
		Private Const REGEX_USeMail As String = "\{(?i)TMPL_VAR name=\'USeMail\'\}"
		Private Const REGEX_USMDName As String = "\{(?i)TMPL_VAR name=\'USMDName\'\}"
		Private Const REGEX_USMDName2 As String = "\{(?i)TMPL_VAR name=\'USMDName2\'\}"
		Private Const REGEX_USMDPostfach As String = "\{(?i)TMPL_VAR name=\'USMDPostfach\'\}"
		Private Const REGEX_USMDStrasse As String = "\{(?i)TMPL_VAR name=\'USMDStrasse\'\}"
		Private Const REGEX_USMDOrt As String = "\{(?i)TMPL_VAR name=\'USMDort\'\}"
		Private Const REGEX_USMDPlz As String = "\{(?i)TMPL_VAR name=\'USMDPlz\'\}"
		Private Const REGEX_USMDLand As String = "\{(?i)TMPL_VAR name=\'USMDLand\'\}"
		Private Const REGEX_USMDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDTelefon\'\}"
		Private Const REGEX_USMDTelefax As String = "\{(?i)TMPL_VAR name=\'USMDTelefax\'\}"
		Private Const REGEX_USMDeMail As String = "\{(?i)TMPL_VAR name=\'USMDeMail\'\}"
		Private Const REGEX_USMDHomepage As String = "\{(?i)TMPL_VAR name=\'USMDHomepage\'\}"
		Private Const REGEX_USTitel_1 As String = "\{(?i)TMPL_VAR name=\'USTitel_1\'\}"
		Private Const REGEX_USTitel_2 As String = "\{(?i)TMPL_VAR name=\'USTitel_2\'\}"
		Private Const REGEX_USPostfach As String = "\{(?i)TMPL_VAR name=\'USPostfach\'\}"
		Private Const REGEX_USStrasse As String = "\{(?i)TMPL_VAR name=\'USStrasse\'\}"
		Private Const REGEX_USPLZ As String = "\{(?i)TMPL_VAR name=\'USPLZ\'\}"
		Private Const REGEX_USOrt As String = "\{(?i)TMPL_VAR name=\'USOrt\'\}"
		Private Const REGEX_USLand As String = "\{(?i)TMPL_VAR name=\'USLand\'\}"
		Private Const REGEX_USAbteilung As String = "\{(?i)TMPL_VAR name=\'USAbteilung\'\}"

		Private Const REGEX_EmployeeBriefAnrede As String = "\{(?i)TMPL_VAR name=\'MABriefAnrede\'\}"
		Private Const REGEX_MA_OwnerGuid As String = "\{(?i)TMPL_VAR name=\'MAOwner_Guid\'\}"
		Private Const REGEX_MA_Nachname As String = "\{(?i)TMPL_VAR name=\'MANachname\'\}"
		Private Const REGEX_MA_Vorname As String = "\{(?i)TMPL_VAR name=\'MAVorname\'\}"
		Private Const REGEX_MA_Anrede As String = "\{(?i)TMPL_VAR name=\'MAAnrede\'\}"
		Private Const REGEX_MA_ForAnrede As String = "\{(?i)TMPL_VAR name=\'MAForAnrede\'\}"

		Private Const REGEX_ApplicationLabel As String = "\{(?i)TMPL_VAR name=\'ApplicationLabel\'\}"
		Private Const REGEX_ApplicationCreatedOn As String = "\{(?i)TMPL_VAR name=\'ApplicationCreatedOn\'\}"
		Private Const REGEX_APPLICATION_LABEL As String = "\{(?i)TMPL_VAR name=\'ApplicationLabel\'\}"
		Private Const REGEX_APPLICATION_CREATEDON As String = "\{(?i)TMPL_VAR name=\'ApplicationCreatedOn\'\}"

		Private Const REGEX_Company1 As String = "\{(?i)TMPL_VAR name=\'Company1\'\}"
		Private Const REGEX_Company2 As String = "\{(?i)TMPL_VAR name=\'Company2\'\}"
		Private Const REGEX_Company3 As String = "\{(?i)TMPL_VAR name=\'Company3\'\}"
		Private Const REGEX_CompanyStreet As String = "\{(?i)TMPL_VAR name=\'CompanyStreet\'\}"
		Private Const REGEX_CustomerPostcodeLocation As String = "\{(?i)TMPL_VAR name=\'CustomerPostcodeLocation\'\}"
		Private Const REGEX_VacancyLabel As String = "\{(?i)TMPL_VAR name=\'VacancyLabel\'\}"
		Private Const REGEX_ProposeLabel As String = "\{(?i)TMPL_VAR name=\'ProposeLabel\'\}"

		Private Const REGEX_NumberOfInvoices As String = "\{(?i)TMPL_VAR name=\'numberofinvoices\'\}"
		Private Const REGEX_WhenMoreThanOne As String = "\{(?i)TMPL_VAR name=\'WhenMoreThanOne\'\}"
		Private Const REGEX_InvoiceNumber As String = "\{(?i)TMPL_VAR name=\'invoiceNumber\'\}"
		Private Const REGEX_InvoiceDate As String = "\{(?i)TMPL_VAR name=\'invoiceDate\'\}"
		Private Const REGEX_InvoiceRZHD As String = "\{(?i)TMPL_VAR name=\'invoiceRZHD\'\}"
		Private Const REGEX_InvoiceRName1 As String = "\{(?i)TMPL_VAR name=\'invoiceRName1\'\}"
		Private Const REGEX_InvoiceRName2 As String = "\{(?i)TMPL_VAR name=\'invoiceRName2\'\}"
		Private Const REGEX_InvoiceRName3 As String = "\{(?i)TMPL_VAR name=\'invoiceRName3\'\}"
		Private Const REGEX_InvoiceRStrasse As String = "\{(?i)TMPL_VAR name=\'invoiceRStrasse\'\}"
		Private Const REGEX_InvoiceRPLZ As String = "\{(?i)TMPL_VAR name=\'invoiceRPlz\'\}"
		Private Const REGEX_InvoiceRPostfach As String = "\{(?i)TMPL_VAR name=\'invoiceRPostfach\'\}"
		Private Const REGEX_InvoiceRLand As String = "\{(?i)TMPL_VAR name=\'invoiceRLand\'\}"
		Private Const REGEX_InvoiceAmountTotal As String = "\{(?i)TMPL_VAR name=\'invoiceAmountTotal\'\}"
		Private Const REGEX_InvoiceCurrency As String = "\{(?i)TMPL_VAR name=\'invoiceCurrency\'\}"
		Private Const REGEX_InvoiceDueDate As String = "\{(?i)TMPL_VAR name=\'invoiceDueDate\'\}"
		Private Const REGEX_InvoiceRefFooter As String = "\{(?i)TMPL_VAR name=\'invoiceRefFooter\'\}"
		Private Const REGEX_InvoiceRefline As String = "\{(?i)TMPL_VAR name=\'invoiceRefLine\'\}"
		Private Const REGEX_InvoiceESRKonto As String = "\{(?i)TMPL_VAR name=\'invoiceESRKonto\'\}"
		Private Const REGEX_InvoiceIBAN As String = "\{(?i)TMPL_VAR name=\'invoiceIBAN\'\}"

		Private Const REGEX_NumberOfpayrolls As String = "\{(?i)TMPL_VAR name=\'numberofpayrolls\'\}"

#End Region

#Region "private fields"


		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Private m_TableDatabaseAccess As ITablesDatabaseAccess
		Private m_ListingDatabaseAccess As IListingDatabaseAccess
		Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess
		Private m_ProposeDatabaseAccess As IProposeDatabaseAccess
		Private m_AppDatabaseAccess As IAppDatabaseAccess

		Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess
		Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_HtmlConverter As SP.Infrastructure.HTMLUtiles.Utilities

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility
		Private m_DateUtility As DateAndTimeUtily

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath

		Private m_CustomerWOSID As String
		Private m_TemplatePath As String

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		Private m_SonstigesSetting As String
		Private m_MailingSetting As String

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connectionString As String
		Private m_SelectedFile2Import As String

		Private m_EmployeeNumber As Integer?
		Private m_ApplicationNumber As Integer?
		Private m_VacancyNumber As Integer?
		Private m_ProposeNumber As Integer?
		Private m_CustomerNumber As Integer?
		Private m_InvoiceNumber As Integer?
		Private m_PayrollNumber As Integer?

		Private m_NumberOfInvoices As Integer?
		Private m_NumberOfPayrolls As Integer?

		Private m_Unprocessed As Image
		Private m_InProcessing As Image
		Private m_Processed As Image
		Private m_Failed As Image

		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Protected m_SuppressUIEvents As Boolean = False
		Private m_SenderEMailData As List(Of String)
		Private m_RecipientEMailData As List(Of String)

		Private m_EmployeeData As EmployeeMasterData
		Private m_EmployeeContactCommData As EmployeeContactComm
		Private m_ApplicationData As ApplicationData
		Private m_VacancyData As VacancyMasterData
		Private m_ProposeData As ProposeMasterData
		Private m_CustomerData As CustomerMasterData
		Private m_InvoiceData As Invoice
		Private m_PayrollData As PayrollDetailData

		Private m_EMailTemplateData As IEnumerable(Of EMailTemplateData)
		Private m_UserData As SP.DatabaseAccess.TableSetting.DataObjects.UserData
		Private m_EmployeeDocumentData As New BindingList(Of EMailAttachmentDocumentData)
		Private m_SortedEmployeeDocumentData As New BindingList(Of EMailAttachmentDocumentData)

		Private m_CurrentInvoiceMailMergeData As New InvoiceEMailMergeViewData
		Private m_AsssignedTemplateFilename As String

		Private m_SendResult As List(Of String)
		Private m_SmtpServer As String
		Private m_MailSender As String

		Private m_RtfContent As RichEditControl

#End Region





#Region "private properties"


#Region "xml data"

		''' <summary>
		''' Gets the email subject for zv.
		''' </summary>
		Private ReadOnly Property EMailSubjectForZV As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Zwischenverdienstformular_Doc-eMail-Betreff", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for argb.
		''' </summary>
		Private ReadOnly Property EMailSubjectForARGB As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Arbeitgeberbescheinigung_Doc-eMail-Betreff", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for payroll.
		''' </summary>
		Private ReadOnly Property EMailSubjectForPayroll As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/payroll-email-subject", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		Private ReadOnly Property EMailSubjectForMorePayrolls As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/morepayroll-email-subject", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for invoice.
		''' </summary>
		Private ReadOnly Property EMailSubjectForInvoice As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/Invoice-eMail-subject", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		Private ReadOnly Property EMailSubjectForMoreInvoices As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/moreinvoices-email-subject", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for applicant ok notification.
		''' </summary>
		Private ReadOnly Property EMailSubjectForApplicantOKNotification As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/applicantoknotification_doc-email-betreff", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for applicant cancel notification.
		''' </summary>
		Private ReadOnly Property EMailSubjectForApplicantCancelNotification As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/applicantcancelnotification_doc-email-betreff", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for application ok notification.
		''' </summary>
		Private ReadOnly Property EMailSubjectForApplicationOKNotification As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/applicationoknotification_doc-email-betreff", m_MailingSetting))

				Return settingValue
			End Get
		End Property

		''' <summary>
		''' Gets the email subject for application cancel notification.
		''' </summary>
		Private ReadOnly Property EMailSubjectForApplicationCancelNotification As String
			Get
				Dim settingValue As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/applicationcancelnotification_doc-email-betreff", m_MailingSetting))

				Return settingValue
			End Get
		End Property

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

#End Region

		''' <summary>
		''' Gets the selected document.
		''' </summary>
		''' <returns>The selected merge data or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedMergeRecord As InvoiceEMailMergeViewData
			Get
				Dim gvRP = TryCast(grdMergeData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim doc = CType(gvRP.GetRow(selectedRows(0)), InvoiceEMailMergeViewData)

						Return doc
					End If

				End If

				Return Nothing
			End Get

		End Property

		Private ReadOnly Property EMailSender As String
			Get
				Dim value As String = If(String.IsNullOrWhiteSpace(m_InitializationData.UserData.UserMDeMail), m_InitializationData.MDData.MDeMail, m_InitializationData.UserData.UserMDeMail).Trim

				Return value
			End Get
		End Property

#End Region


#Region "public property"

		''' <summary>
		''' Gets or sets the preselection data.
		''' </summary>
		Public Property PreselectionData As PreselectionMailData
		Public Property InvoiceEMailMergeData As List(Of InvoiceEMailMergeData)

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
			m_DateUtility = New DateAndTimeUtily
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			m_HtmlConverter = New SP.Infrastructure.HTMLUtiles.Utilities

			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			InitializeComponent()

			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_TableDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_AppDatabaseAccess = New AppDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_InvoiceDatabaseAccess = New InvoiceDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)


			m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
			m_MailingSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_MAILING_SETTING, m_InitializationData.MDData.MDNr)
			m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
			m_Logger.LogDebug(String.Format("GetSelectedMDDataXMLFilename located in: {0}", m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)))

			m_Unprocessed = My.Resources.Unprocessed
			m_InProcessing = My.Resources.Processing
			m_Processed = My.Resources.Processed
			m_Failed = My.Resources.Failed

			TranslateControls()
			Reset()

			AddHandler Me.gvMergeData.RowCellClick, AddressOf OngvMergeData_RowCellClick

		End Sub

#End Region


#Region "public methodes"


		''' <summary>
		''' Loads data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Public Function LoadData() As Boolean
			Dim success As Boolean = True

			m_SuppressUIEvents = True

			m_SmtpServer = EMailSMTPServer
			m_MailSender = EMailSender
			aceSmtpServer.Text = String.Format("SMTP-Server: {0}", m_SmtpServer)
			aceSender.Text = String.Format("Absender: {0}", m_MailSender)
			aceStaging.Text = String.Format("Testnachricht an: {0}", m_InitializationData.UserData.UserMDeMail)
			aceSendResult.Text = String.Empty


			m_EmployeeNumber = PreselectionData.EmployeeNumber
			m_ApplicationNumber = PreselectionData.ApplicationNumber
			m_CustomerNumber = PreselectionData.CustomerNumber
			m_InvoiceNumber = PreselectionData.InvoiceNumber
			m_PayrollNumber = PreselectionData.PayrollNumber
			m_NumberOfInvoices = PreselectionData.NumberOfInvoices
			m_NumberOfPayrolls = PreselectionData.NumberOfPayrolls

			success = success AndAlso LoadAsignedUserData()
			If m_EmployeeNumber.GetValueOrDefault(0) > 0 Then
				success = success AndAlso LoadEmployeeData()
				success = success AndAlso LoadEmployeeContactCommData()
			End If
			If m_ApplicationNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadApplicationData()
			If m_VacancyNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadVacancyData()
			If m_ProposeNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadProposeData()
			If m_InvoiceNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadInvoiceData()

			If m_CustomerNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadCustomerData()

			LoadModulSettingData()
			If PreselectionData.MailType = MailTypeEnum.MOREINVOICES Then
				LoadInvoiceMailMergeData()
			End If



			m_SuppressUIEvents = False

			Return success

		End Function

		Public Sub DisplayEmployeeData()
			Dim result As Boolean = True

			If m_EmployeeNumber Is Nothing Then Return
			result = result AndAlso LoadEmployeeData()
			result = result AndAlso LoadEmployeeContactCommData()

		End Sub


#End Region

		''' <summary>
		''' Starten von Anwendung.
		''' </summary>
		Private Sub Onfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

			Try
				If My.Settings.iEMailHeight > 0 Then Me.Height = My.Settings.iEMailHeight
				If My.Settings.iEMailWidth > 0 Then Me.Width = My.Settings.iEMailWidth
				If My.Settings.frmEMailLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmEMailLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
			End Try

		End Sub

		Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
			SplashScreenManager.CloseForm(False)

			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.iEMailHeight = Me.Height
				My.Settings.iEMailWidth = Me.Width
				My.Settings.frmEMailLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)


				My.Settings.Save()
			End If

		End Sub


		Private Sub Reset()

			ResetMergeDataGrid()

		End Sub

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.lblFormImage.Text = m_Translate.GetSafeTranslationValue(Me.lblFormImage.Text)
			btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

			Me.grpData.Text = m_Translate.GetSafeTranslationValue(Me.grpData.Text)
			Me.btnSend.Text = m_Translate.GetSafeTranslationValue(Me.btnSend.Text)

			bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)

		End Sub

		Private Sub ResetMergeDataGrid()

			gvMergeData.OptionsView.ShowIndicator = False
			gvMergeData.OptionsView.ShowAutoFilterRow = False
			gvMergeData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvMergeData.OptionsView.ShowFooter = False
			gvMergeData.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False
			gvMergeData.OptionsBehavior.Editable = True

			' Reset the grid
			gvMergeData.Columns.Clear()

			Dim picutureEdit As New RepositoryItemPictureEdit()
			grdMergeData.RepositoryItems.Add(picutureEdit)


			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = " "
			docType.OptionsColumn.AllowEdit = False
			docType.Name = "Processing"
			docType.FieldName = "Processing"
			docType.Visible = True
			'docType.ColumnEdit = New RepositoryItemPictureEdit()
			'docType.UnboundType = DevExpress.Data.UnboundColumnType.Object

			picutureEdit.NullText = " "
			docType.ColumnEdit = picutureEdit
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Integer ' DevExpress.Data.UnboundColumnType.Object
			docType.Width = 20

			gvMergeData.Columns.Add(docType)




			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerNumber.OptionsColumn.AllowEdit = False
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("KD-Nr.")
			columnCustomerNumber.Name = "CustomerNumber"
			columnCustomerNumber.FieldName = "CustomerNumber"
			columnCustomerNumber.Visible = False
			gvMergeData.Columns.Add(columnCustomerNumber)

			Dim columnCompanyname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompanyname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCompanyname.OptionsColumn.AllowEdit = False
			columnCompanyname.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCompanyname.Name = "Companyname"
			columnCompanyname.FieldName = "Companyname"
			columnCompanyname.Visible = True
			gvMergeData.Columns.Add(columnCompanyname)

			Dim columnREEMail As New DevExpress.XtraGrid.Columns.GridColumn()
			columnREEMail.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnREEMail.OptionsColumn.AllowEdit = False
			columnREEMail.Caption = m_Translate.GetSafeTranslationValue("EMail-Adresse")
			columnREEMail.Name = "REEMail"
			columnREEMail.FieldName = "REEMail"
			columnREEMail.Visible = True
			gvMergeData.Columns.Add(columnREEMail)

			Dim columnNumberOfInvoices As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNumberOfInvoices.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNumberOfInvoices.OptionsColumn.AllowEdit = False
			columnNumberOfInvoices.Caption = m_Translate.GetSafeTranslationValue("Anzahl Rechnungen")
			columnNumberOfInvoices.Name = "NumberOfInvoices"
			columnNumberOfInvoices.FieldName = "NumberOfInvoices"
			columnNumberOfInvoices.Visible = True
			gvMergeData.Columns.Add(columnNumberOfInvoices)


			Dim docZipType As New DevExpress.XtraGrid.Columns.GridColumn()
			docZipType.Caption = " "
			docZipType.OptionsColumn.AllowEdit = False
			docZipType.Name = "AttachmentAsZip"
			docZipType.FieldName = "AttachmentAsZip"
			docZipType.Visible = True
			picutureEdit.NullText = " "
			docZipType.ColumnEdit = picutureEdit
			docZipType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			docZipType.Width = 20

			gvMergeData.Columns.Add(docZipType)



			grdMergeData.DataSource = Nothing

		End Sub

		Private Sub gvPrint_MasterRowExpanded(sender As Object, e As CustomMasterRowEventArgs) Handles gvMergeData.MasterRowExpanded
			Dim view As GridView = TryCast(TryCast(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
			'Return
			view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			view.OptionsView.ShowIndicator = False
			view.OptionsBehavior.Editable = False
			view.OptionsView.ShowAutoFilterRow = True
			view.OptionsView.ColumnAutoWidth = False
			view.OptionsView.ShowFooter = True
			view.OptionsView.AllowHtmlDrawGroups = True

			view.Columns.Clear()

			If e.RelationIndex = 0 Then
				Dim columnDateien As New DevExpress.XtraGrid.Columns.GridColumn()
				columnDateien.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				columnDateien.OptionsColumn.AllowEdit = False
				columnDateien.Caption = m_Translate.GetSafeTranslationValue("Dateien")
				columnDateien.Name = "Column"
				columnDateien.FieldName = "Column"
				columnDateien.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
				columnDateien.AppearanceHeader.Options.UseTextOptions = True
				columnDateien.Visible = True
				columnDateien.Width = grdMergeData.Width - 100
				view.Columns.Add(columnDateien)

			ElseIf e.RelationIndex = 1 Then
				Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
				columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				columnLONr.OptionsColumn.AllowEdit = False
				columnLONr.Caption = m_Translate.GetSafeTranslationValue("Rechnung-Nr.")
				columnLONr.Name = "Column"
				columnLONr.FieldName = "Column"
				columnLONr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
				columnLONr.AppearanceHeader.Options.UseTextOptions = True

				columnLONr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
				columnLONr.AppearanceCell.Options.UseTextOptions = True

				columnLONr.Width = 300
				columnLONr.Visible = True
				view.Columns.Add(columnLONr)

			Else
				Return

			End If

			Dim detailView As GridView = CType(CType(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
			RemoveHandler detailView.RowCellClick, AddressOf OngvDetail_RowCellClick
			If (Not (detailView) Is Nothing) Then
				'detailView.ParentView
				AddHandler detailView.RowCellClick, AddressOf OngvDetail_RowCellClick
			End If

		End Sub

		Sub OngvMergeData_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			Try

				If (e.Clicks = 2) Then

					Dim column = e.Column
					Dim data = SelectedMergeRecord
					If data Is Nothing Then Return


					Select Case column.Name.ToLower
						Case "processing"
							If data.ProcessState = ProcessState.Failed Then
								m_UtilityUI.ShowErrorDialog(data.SendState.ValueLable)
							End If

						Case "companyname"
							If data.CustomerNumber.GetValueOrDefault(0) > 0 Then
								Dim hub = MessageService.Instance.Hub
								Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, data.CustomerNumber.GetValueOrDefault(0))
								hub.Publish(openMng)

							End If

						Case "AttachmentAsZip".ToLower
							If data.IndividualAttachments Is Nothing Then
								Process.Start("explorer.exe", "/select," & data.Attachment)
							Else
								Process.Start("explorer.exe", "/select," & data.IndividualAttachments(0))
							End If


						Case Else
							Return

					End Select
				End If

			Catch ex As Exception

			End Try

		End Sub

		Sub OngvDetail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			Try

				If (e.Clicks = 2) Then

					Dim column = e.Column
					Dim detailView As GridView = CType(sender, GridView)
					If (Not (detailView) Is Nothing) Then
						Dim dataRow = detailView.GetRow(e.RowHandle)
						Dim viewData = dataRow
						If Val(dataRow.ToString) = 0 Then
							Process.Start("explorer.exe", "/select," & viewData)

						ElseIf Val(dataRow.ToString) > 0 Then
							Dim hub = MessageService.Instance.Hub
							Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, Val(dataRow.ToString))
							hub.Publish(openMng)

						End If
					End If
				End If

			Catch ex As Exception

			End Try

		End Sub

		Private Sub OngvMergeData_MasterRowGetRelationDisplayCaption(sender As Object, e As MasterRowGetRelationNameEventArgs) Handles gvMergeData.MasterRowGetRelationDisplayCaption

			If e.RelationIndex = 0 Then e.RelationName = "Dateien"
			If e.RelationIndex = 1 Then e.RelationName = "Rechnungen"

		End Sub


#Region "load Database data"

		''' <summary>
		''' Load employee data.
		''' </summary>
		Private Function LoadEmployeeData() As Boolean
			Try
				m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, True)

				If (m_EmployeeData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_EmployeeData Is Nothing
		End Function

		''' <summary>
		''' Load MAKontakt_Komm data.
		''' </summary>
		Private Function LoadEmployeeContactCommData() As Boolean

			Try
				m_EmployeeContactCommData = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_EmployeeNumber)

				If (m_EmployeeContactCommData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten-Kontakt Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_EmployeeContactCommData Is Nothing
		End Function

		''' <summary>
		''' Load customer data.
		''' </summary>
		Private Function LoadCustomerData() As Boolean
			Try
				m_CustomerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CustomerNumber, m_InitializationData.UserData.UserFiliale)

				If (m_CustomerData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kunden Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_CustomerData Is Nothing
		End Function

		''' <summary>
		''' Load user data.
		''' </summary>
		Private Function LoadAsignedUserData() As Boolean

			Try
				Dim data = m_TableDatabaseAccess.LoadUserData(m_InitializationData.UserData.UserNr)
				m_UserData = data(0)

				If (m_UserData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzer Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_UserData Is Nothing
		End Function

		''' <summary>
		''' Load application data.
		''' </summary>
		Private Function LoadApplicationData() As Boolean

			Try
				Dim data = m_EmployeeDatabaseAccess.LoadAssignedEmployeeApplications(m_InitializationData.MDData.MDGuid, m_EmployeeNumber, m_ApplicationNumber, False)

				If (data Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerber Daten konnten nicht geladen werden."))
				End If
				m_ApplicationData = data(0)
				m_VacancyNumber = m_ApplicationData.VacancyNumber

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_EmployeeContactCommData Is Nothing
		End Function

		''' <summary>
		''' Load vacancy data.
		''' </summary>
		Private Function LoadVacancyData() As Boolean

			Try
				m_VacancyData = m_VacancyDatabaseAccess.LoadVacancyMasterData(m_InitializationData.MDData.MDNr, m_VacancyNumber)

				If (m_VacancyData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanzen Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_VacancyData Is Nothing
		End Function

		''' <summary>
		''' Load propose data.
		''' </summary>
		Private Function LoadProposeData() As Boolean

			Try
				m_ProposeData = m_ProposeDatabaseAccess.LoadProposeMasterData(m_ProposeNumber)

				If (m_ProposeData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorschlag Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_ProposeData Is Nothing
		End Function

		Private Function LoadInvoiceData() As Boolean

			Try
				m_InvoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_InvoiceNumber)

				If (m_InvoiceData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnung Daten konnten nicht geladen werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not m_InvoiceData Is Nothing
		End Function

		Private Function LoadInvoiceMailMergeData() As Boolean

			Try
				If InvoiceEMailMergeData Is Nothing OrElse InvoiceEMailMergeData.Count = 0 Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnung Daten konnten nicht geladen werden."))

					Return False
				End If
				'aceSendAsIndividualFiles.Visible = Not InvoiceEMailMergeData(0).IndividualAttachments Is Nothing

				Dim listDataSource As BindingList(Of InvoiceEMailMergeViewData) = New BindingList(Of InvoiceEMailMergeViewData)

				' Convert the data to view data.
				For Each p In InvoiceEMailMergeData

					Dim cViewData = New InvoiceEMailMergeViewData() With {.CustomerNumber = p.CustomerNumber,
						.Attachment = p.Attachment,
						.IndividualAttachments = p.IndividualAttachments,
						.InvoiceNumbers = p.InvoiceNumbers,
						.Companyname = p.Companyname,
						.InvoiceDate = p.InvoiceDate,
						.InvoiceNumber = p.InvoiceNumber,
						.NumberOfInvoices = p.NumberOfInvoices,
						.ProcessState = ProcessState.Unprocessed,
						.REEMail = p.REEMail,
						.SendState = Nothing}

					listDataSource.Add(cViewData)
				Next

				grdMergeData.DataSource = listDataSource
				grdMergeData.ForceInitialize()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return Not InvoiceEMailMergeData Is Nothing
		End Function

#End Region

		Private Sub LoadModulSettingData()

			m_TemplatePath = m_InitializationData.MDData.MDTemplatePath
			Select Case PreselectionData.MailType


				Case MailTypeEnum.PAYROLL, MailTypeEnum.MOREPAYROLLS
					m_TemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Payroll")

				Case MailTypeEnum.INVOICE
					m_TemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice")

				Case MailTypeEnum.MOREINVOICES
					'txt_MailAn.EditValue = String.Empty
					m_TemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice")


				Case Else
					m_UtilityUI.ShowErrorDialog("Sie haben kein Modul ausgewählt!")
					Return

			End Select

			Try
				Dim success As Boolean = LoadSenderData()
				success = success AndAlso LoadEMailTemplateData()

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Private Function LoadSenderData() As Boolean
			Dim result As Boolean = True
			Try
				Dim data As New List(Of String) From {m_InitializationData.UserData.UserMDeMail, m_InitializationData.MDData.MDeMail}
				data.RemoveAll(Function(str) String.IsNullOrEmpty(str))

				m_SenderEMailData = data.GroupBy(Function(m) m).Where(Function(g) g.Count() = 1).Select(Function(g) g.Key).ToList

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return result

		End Function

		''' <summary>
		''' Load template data.
		''' </summary>
		Private Function LoadEMailTemplateData() As Boolean
			Dim eMailType As String = "Mail.{0}.01"

			Select Case PreselectionData.MailType
				Case MailTypeEnum.PAYROLL
					eMailType = String.Format("Mail.{0}.%", "Payroll")
				Case MailTypeEnum.MOREPAYROLLS
					eMailType = String.Format("Mail.{0}.%", "MorePayrolls")

				Case MailTypeEnum.INVOICE
					eMailType = String.Format("Mail.{0}.%", "Invoice")
				Case MailTypeEnum.MOREINVOICES
					eMailType = String.Format("Mail.{0}.%", "MoreInvoices")


				Case Else
					Return False

			End Select

			m_EMailTemplateData = m_ListingDatabaseAccess.LoadTemplateDataToSendEmail(eMailType)
			If (m_EMailTemplateData Is Nothing OrElse m_EMailTemplateData.Count = 0) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("EMail-Vorlage Daten konnten nicht geladen werden."))

				Return False
			End If
			m_AsssignedTemplateFilename = m_EMailTemplateData(0).DocumentName

			Return Not m_EMailTemplateData Is Nothing
		End Function

		Private Sub OnbtnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
			Dim recipientAddress As String = String.Format("{0}", m_InitializationData.UserData.UserMDeMail)
			Dim msg As String = String.Empty
			Dim attachments As List(Of String)

			aceSendResult.Text = String.Empty

			Try
				ResetMergeDataGrid()
				LoadInvoiceMailMergeData()

#If DEBUG Then
				m_MailSender = "user@domain.com"
				recipientAddress = "info@domain.com"
#End If

				m_CurrentInvoiceMailMergeData = SelectedMergeRecord

				If m_CurrentInvoiceMailMergeData.IndividualAttachments Is Nothing Then
					attachments = New List(Of String) From {m_CurrentInvoiceMailMergeData.Attachment}
				Else
					attachments = New List(Of String)(m_CurrentInvoiceMailMergeData.IndividualAttachments)
				End If

				LoadMailSubjectData()
				LoadmyMailTemplate(Path.Combine(m_TemplatePath, m_AsssignedTemplateFilename))

				Dim result = SendMailToWithExchange(m_MailSender, recipientAddress, String.Empty, String.Empty, "TEST-NACHRICHT", m_RtfContent.RtfText, attachments)

				If result.Value Then
					msg = String.Format(m_Translate.GetSafeTranslationValue("Ihre Nachricht wurde erfolgreich gesendet."))
					m_UtilityUI.ShowInfoDialog(msg)

				Else
					msg = String.Format(m_Translate.GetSafeTranslationValue("Fehler in der Übermittlung der Nachrichten.") & "<br>{0}", result.ValueLable)
					bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Fehler in der Übermittlung der Nachrichten.")

					m_UtilityUI.ShowErrorDialog(msg)
				End If

			Catch ex As Exception

			End Try

		End Sub

		Private Sub OnbtnMergeSend_Click(sender As Object, e As EventArgs) Handles btnMergeSend.Click
			Dim result As Boolean = True
			Dim msg As String = m_Translate.GetSafeTranslationValue("Hiermit senden Sie Ihre Nachricht an {0} Adressen.<br>Möchten Sie mit dem Vorgang fortfahren?")
			If Not m_UtilityUI.ShowYesNoDialog(String.Format(msg, InvoiceEMailMergeData.Count), m_Translate.GetSafeTranslationValue("Serien-Versand")) Then Return
			aceSendResult.Text = String.Empty

			ResetMergeDataGrid()
			LoadInvoiceMailMergeData()


			result = result AndAlso BuildAndSendMeragedInvoices()
			If result AndAlso m_SendResult.Count = 0 Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Ihre Nachricht wurde erfolgreich gesendet."))
				m_UtilityUI.ShowInfoDialog(msg)

			Else
				msg = String.Format(m_Translate.GetSafeTranslationValue("Fehler in der Übermittlung der Nachrichten.") & "<br>{0}", String.Join("<br>", m_SendResult))
				m_Logger.LogError(msg)
				bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Fehler in der Übermittlung der Nachrichten.")

				m_UtilityUI.ShowErrorDialog(msg)
			End If

		End Sub

		Private Sub FocusData(ByVal reEMail As String)

			Try
				If Not grdMergeData.DataSource Is Nothing Then

					Dim commonViewData = CType(gvMergeData.DataSource, IEnumerable(Of InvoiceEMailMergeViewData))

					Dim index = commonViewData.ToList().FindIndex(Function(data) data.REEMail = reEMail)

					'm_SuppressUIEvents = True
					Dim rowHandle = gvMergeData.GetRowHandle(index)
					gvMergeData.FocusedRowHandle = rowHandle
					'm_SuppressUIEvents = False

				End If

			Catch ex As Exception
				m_Logger.LogWarning(ex.ToString)
			End Try

		End Sub

		Private Sub OngvMergeData_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvMergeData.CustomUnboundColumnData

			If e.Column.Name = "Processing" Then
				If (e.IsGetData()) Then
					Dim state = CType(e.Row, InvoiceEMailMergeViewData).ProcessState

					Select Case state
						Case ProcessState.Unprocessed
							e.Value = m_Unprocessed
						Case ProcessState.InProcessing
							e.Value = m_InProcessing
						Case ProcessState.Processed
							e.Value = m_Processed
						Case ProcessState.Failed
							e.Value = m_Failed

						Case Else
							e.Value = m_Unprocessed
					End Select
				End If

			ElseIf e.Column.Name = "AttachmentAsZip" Then
				If (e.IsGetData()) Then
					Dim asPDF = True
					Dim data = CType(e.Row, InvoiceEMailMergeViewData)

					If Not data.IndividualAttachments Is Nothing AndAlso data.IndividualAttachments.Count > 0 Then
						asPDF = True
					Else
						asPDF = False
					End If

					Select Case asPDF
						Case False
							e.Value = ImageCollection1.Images(8)


						Case Else
							e.Value = ImageCollection1.Images(9)

					End Select

				End If
			End If

		End Sub

		Private Function LoadMeragedInvoiceTamplateData() As Boolean
			Dim m_TemplateData = New EMailTemplateData

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try
				m_CurrentInvoiceMailMergeData = SelectedMergeRecord

				LoadMailSubjectData()
				LoadmyMailTemplate(Path.Combine(m_TemplatePath, m_AsssignedTemplateFilename))


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))

			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Function

		Private Function BuildAndSendMeragedInvoices() As Boolean
			Dim success As Boolean = True
			Dim numberOfSuccess As Integer = 0
			Dim numberOffailure As Integer = 0
			Dim attachments As List(Of String)

			Dim subject As String

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihr Versand ist gestartet") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")
			m_SendResult = New List(Of String)

			Try
				Dim dataToSend = CType(gvMergeData.DataSource, IEnumerable(Of InvoiceEMailMergeViewData))

				For Each itm In dataToSend
					m_CurrentInvoiceMailMergeData = itm
					FocusData(itm.REEMail)

					If m_CurrentInvoiceMailMergeData.IndividualAttachments Is Nothing Then
						attachments = New List(Of String) From {m_CurrentInvoiceMailMergeData.Attachment}
					Else
						attachments = New List(Of String)(m_CurrentInvoiceMailMergeData.IndividualAttachments)
					End If

					itm.ProcessState = ProcessState.InProcessing
					grdMergeData.RefreshDataSource()

					subject = LoadMailSubjectData()
					LoadmyMailTemplate(Path.Combine(m_TemplatePath, m_AsssignedTemplateFilename))
					m_CustomerNumber = m_CurrentInvoiceMailMergeData.CustomerNumber.GetValueOrDefault(0)

					Dim recipientAddress As String = String.Format("{0}", m_CurrentInvoiceMailMergeData.REEMail)

					Dim sendResult = SendMailToWithExchange(m_MailSender, recipientAddress, String.Empty, String.Empty, subject, m_RtfContent.RtfText, attachments)

					If Not sendResult.Value Then
						m_SendResult.Add(sendResult.ValueLable)
						itm.ProcessState = ProcessState.Failed
						itm.SendState = New MailSendValue With {.Value = False, .ValueLable = sendResult.ValueLable}
						numberOffailure += 1

					Else
						If m_MailSender <> recipientAddress Then
							Dim addContactData = AddCustomerContactData(m_MailSender, recipientAddress, String.Empty, String.Empty, subject, m_HtmlConverter.ConvertToPlainText(m_RtfContent.HtmlText), attachments)
						End If

						numberOfSuccess += 1
						itm.ProcessState = ProcessState.Processed
					End If

					grdMergeData.RefreshDataSource()
				Next

				aceSendResult.Text = String.Format("Erfolgreich: {0}<br>Fehlerhaft: {1}", numberOfSuccess, numberOffailure)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))

			Finally
				SplashScreenManager.CloseForm(False)

			End Try

			Return success
		End Function

		Private Function LoadMailSubjectData() As String
			Dim subject As String = String.Empty

			Select Case PreselectionData.MailType
				Case MailTypeEnum.INVOICE
					subject = EMailSubjectForInvoice
					subject = Regex.Replace(subject, REGEX_Company1, m_InvoiceData.RName1)
					subject = Regex.Replace(subject, REGEX_InvoiceNumber, String.Format("{0:F0}", m_InvoiceData.ReNr))
					subject = Regex.Replace(subject, REGEX_InvoiceDueDate, String.Format("{0:d}", m_InvoiceData.FakDat))

				Case MailTypeEnum.MOREINVOICES
					subject = EMailSubjectForMoreInvoices
					If String.IsNullOrWhiteSpace(subject) Then subject = String.Format("Versand von {0} Rechnungen", m_CurrentInvoiceMailMergeData.NumberOfInvoices)
					subject = Regex.Replace(subject, REGEX_NumberOfInvoices, String.Format("{0:F0}", m_CurrentInvoiceMailMergeData.NumberOfInvoices))

				Case MailTypeEnum.PAYROLL
					subject = EMailSubjectForPayroll

				Case MailTypeEnum.MOREPAYROLLS
					subject = EMailSubjectForMorePayrolls
					If String.IsNullOrWhiteSpace(subject) Then subject = String.Format("Versand von {0} Lohnabrechnungen", m_NumberOfPayrolls)


				Case Else
					Return False

			End Select

			If Not m_EmployeeData Is Nothing Then
				subject = Regex.Replace(subject, REGEX_MA_ForAnrede, If(m_EmployeeData.Gender = "M", "Herrn", "Frau"))
				subject = Regex.Replace(subject, REGEX_MA_Nachname, m_EmployeeData.Lastname)
				subject = Regex.Replace(subject, REGEX_MA_Vorname, m_EmployeeData.Firstname)
			End If

			'Me.Text = String.Format(m_Translate.GetSafeTranslationValue("{0} - Nachricht (HTML)"), subject)

			Return subject
		End Function

		Private Sub LoadmyMailTemplate(ByVal tplFilename As String)

			Try
				If Not File.Exists(tplFilename) AndAlso tplFilename.ToLower.Contains("invoice") Then
					Dim webservice As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

					webservice.PerformDownloadingCommonTemplateFilesOverWebService()
				End If
				If File.Exists(tplFilename) Then
					m_RtfContent = New RichEditControl

					m_RtfContent.BeginUpdate()
					m_RtfContent.LoadDocument(tplFilename)
					m_RtfContent.EndUpdate()

					ParseTemplateFile(m_RtfContent)

				Else
					SplashScreenManager.CloseForm(False)

					Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Die Vorlage-Datei existiert nicht.{0}{1}"), vbNewLine, tplFilename)
					m_UtilityUI.ShowErrorDialog(msg)
				End If

			Catch ex As Exception
				SplashScreenManager.CloseForm(False)

				m_Logger.LogError(ex.StackTrace)
				m_RtfContent.LoadDocument(tplFilename)

			End Try

		End Sub



#Region "employee contact table"

		Private Function AddCustomerContactData(ByVal strFrom As String, ByVal strTo As String,
												ByVal CCAddress As String, ByVal BCCAddress As String,
												ByVal strSubject As String, ByVal strBody As String,
												ByVal aAttachmentFile As List(Of String)) As Boolean
			Dim result As Boolean = True
			Dim strMessage = "Ihre Nachricht wurde erfolgreich gesendet."

			Try
				Dim eMailData = New ListingMailData With {.AsHTML = True, .createdfrom = m_InitializationData.UserData.UserFullName, .customer_id = m_InitializationData.MDData.MDGuid,
						.email_body = m_RtfContent.HtmlText, .email_from = strFrom, .email_to = strTo,
						.email_subject = strSubject, .KDNr = m_CustomerNumber, .EMail_SMTP = m_SmtpServer}

				result = result AndAlso m_ListingDatabaseAccess.AddEMailLogToContactTable(m_InitializationData.MDData.MDNr, eMailData)

				If result AndAlso aAttachmentFile.Count > 0 Then
					Dim addAttachment As Boolean = True
					For Each fileitm In aAttachmentFile

						Dim eMailAttachmentData = New ListingMailAttachmentData With {.createdfrom = m_InitializationData.UserData.UserFullName,
								.customer_id = m_InitializationData.MDData.MDGuid,
									.email_from = strFrom, .email_to = strTo,
									.messageID = eMailData.messageID,
									.email_subject = strSubject, .filename = fileitm,
									.scanfile = m_Utility.LoadFileBytes(fileitm)}

						' just for now
						'addAttachment = addAttachment AndAlso m_ListingDatabaseAccess.AddEMailAttachmentLogToContactTable(m_InitializationData.MDData.MDNr, eMailAttachmentData)
					Next

				End If

				Dim title As String = String.Empty
				Dim description As String = String.Empty
				Dim contactType As String = String.Empty

				contactType = "Einzelmail"
				description = strBody
				Select Case PreselectionData.MailType
					Case MailTypeEnum.MOREINVOICES
						title = String.Format("{0}", strSubject)

					Case Else
						title = "not defined!"

				End Select

				result = result AndAlso m_CurrentInvoiceMailMergeData.CustomerNumber.GetValueOrDefault(0) > 0 AndAlso SaveCustomerContactData(title, strTo, description, contactType, Nothing)


			Catch ex As Exception
				m_Logger.LogError(String.Format("Saving LOG-Data Into MailDb Database:{0}", ex.ToString))
			End Try

			Return result
		End Function

		Private Function SaveCustomerContactData(ByVal title As String, ByVal strTo As String, ByVal description As String, ByVal contactType As String, ByVal attachedFile As String) As Boolean
			Dim success As Boolean = True

			Dim currentContactRecordNumber As Integer = 0
			Dim currentDocumentID As Integer = 0
			Dim fileContent = m_Utility.LoadFileBytes(attachedFile)

			Try
				Dim dt = DateTime.Now
				Dim contactData = New ResponsiblePersonAssignedContactData With {.CustomerNumber = m_CustomerNumber, .ResponsiblePersonNumber = Nothing, .CreatedOn = dt, .CreatedFrom = m_InitializationData.UserData.UserFullName}

				contactData.CustomerNumber = m_CustomerNumber
				contactData.ContactDate = Now.ToString("G")
				contactData.ResponsiblePersonNumber = Nothing
				contactData.ContactType1 = contactType
				contactData.ContactPeriodString = title
				contactData.ContactsString = description
				contactData.ContactImportant = False
				contactData.ContactFinished = True
				contactData.MANr = Nothing
				contactData.VacancyNumber = Nothing
				contactData.ProposeNr = Nothing
				contactData.ESNr = Nothing

				contactData.ChangedFrom = m_InitializationData.UserData.UserFullName
				contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				contactData.ChangedOn = dt
				contactData.UsNr = m_InitializationData.UserData.UserNr

				' Insert or update contact
				success = success AndAlso m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(contactData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			success = success AndAlso SaveOutgoingEmailsData(strTo)

			Return success
		End Function

		Private Function SaveOutgoingEmailsData(ByVal strTo As String) As Boolean
			Dim success As Boolean = True

			Try
				Dim mailData = New OutgoingEMailData With {.Customer_ID = m_InitializationData.MDData.MDGuid, .ModulNumber = ModulNumberEnum.INVOICE}
				mailData.Receiver = strTo
				mailData.Sender = m_InitializationData.UserData.UserMDeMail
				mailData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				mailData.CreatedFrom = m_InitializationData.UserData.UserFullName

				' Insert outgoing mail data
				For Each Number As Integer In m_CurrentInvoiceMailMergeData.InvoiceNumbers
					mailData.Number = Number
					success = success AndAlso m_ListingDatabaseAccess.AddOutgoingEMailData(m_InitializationData.MDData.MDGuid, mailData)
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function


#End Region


#Region "Helpers"

		Function ParseTemplateFile(ByVal FullFileName As DevExpress.XtraRichEdit.RichEditControl) As String
			Dim ParsedFile As String = FullFileName.RtfText + vbCrLf
			Dim pattern As String = String.Empty

			Try
				'// search templatevars
				pattern = String.Empty
				Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)

				''// replace vars
				Dim myRegEx_0 As Regex = New Regex(pattern)
				Dim result_0 = FullFileName.Document.StartSearch(myRegEx_0)

				If Not m_CurrentInvoiceMailMergeData Is Nothing Then

					myRegEx_0 = New Regex(REGEX_NumberOfInvoices)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_CurrentInvoiceMailMergeData.NumberOfInvoices)
					End While

					myRegEx_0 = New Regex(REGEX_WhenMoreThanOne)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					Dim value As String = String.Empty
					If m_CurrentInvoiceMailMergeData.NumberOfInvoices > 1 Then value = "en"
					While result_0.FindNext()
						result_0.Replace(value)
					End While

				End If


				If Not m_InvoiceData Is Nothing Then

					myRegEx_0 = New Regex(REGEX_NumberOfInvoices)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_NumberOfInvoices)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceNumber)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.ReNr)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRZHD)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RZHD)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRName1)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RName1)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRName2)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RName2)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRName3)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RName3)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRStrasse)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RStrasse)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRPLZ)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RPLZ)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRPostfach)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RPostfach)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceDate)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(String.Format("{0:dd.MM.yyyy}", m_InvoiceData.FakDat))
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceDueDate)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(String.Format("{0:dd.MM.yyyy}", m_InvoiceData.Faellig))
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceRLand)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RLand)
					End While

					myRegEx_0 = New Regex(REGEX_InvoiceAmountTotal)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(String.Format("{0:n2}", m_InvoiceData.BetragInk))
					End While
					myRegEx_0 = New Regex(REGEX_InvoiceCurrency)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.Currency)
					End While
					myRegEx_0 = New Regex(REGEX_InvoiceRefFooter)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RefFootNr)
					End While
					myRegEx_0 = New Regex(REGEX_InvoiceRefline)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.RefNr)
					End While
					myRegEx_0 = New Regex(REGEX_InvoiceESRKonto)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.ESRKonto)
					End While
					myRegEx_0 = New Regex(REGEX_InvoiceIBAN)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_InvoiceData.EsrIBAN1)
					End While

				End If


				If Not m_EmployeeData Is Nothing Then

					' Kandidatendaten ...............................................................................................
					myRegEx_0 = New Regex(REGEX_NumberOfpayrolls)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_NumberOfPayrolls)
					End While

					myRegEx_0 = New Regex(REGEX_MA_Nachname)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_EmployeeData.Lastname)
					End While

					myRegEx_0 = New Regex(REGEX_MA_Vorname)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_EmployeeData.Firstname)
					End While

					myRegEx_0 = New Regex(REGEX_MA_Anrede)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_Translate.GetSafeTranslationValue(If(m_EmployeeData.Gender = "M", "Herr", "Frau")))
					End While

					myRegEx_0 = New Regex(REGEX_MA_ForAnrede)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_Translate.GetSafeTranslationValue(If(m_EmployeeData.Gender = "M", "Herrn", "Frau")))
					End While

					myRegEx_0 = New Regex(REGEX_EmployeeBriefAnrede)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						Dim letterAnrede As String = m_EmployeeContactCommData.BriefAnrede
						If letterAnrede.StartsWith("lieb") Then
							letterAnrede = String.Format("{0} {1}", m_Translate.GetSafeTranslationValue(letterAnrede), m_EmployeeData.Firstname)
						Else
							If Not String.IsNullOrWhiteSpace(letterAnrede) Then
								letterAnrede = String.Format("{0} {1}", m_Translate.GetSafeTranslationValue(letterAnrede), m_EmployeeData.Lastname)
							Else
								Dim employeeSex = "Herr"
								If m_EmployeeData.Gender = "M" Then
									employeeSex = "Herr"
								Else
									employeeSex = "Frau"
								End If
								letterAnrede = m_Translate.GetSafeTranslationValue(String.Format("Sehr geehrte{0}", If(m_EmployeeData.Gender = "M", "r", String.Empty)))

								letterAnrede = String.Format("{0} {1} {2}", letterAnrede, employeeSex, m_EmployeeData.Lastname)
							End If

						End If
						result_0.Replace(letterAnrede)
					End While

					myRegEx_0 = New Regex(REGEX_MA_OwnerGuid)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_EmployeeData.Transfered_Guid)
					End While

				End If


				If Not m_CustomerData Is Nothing Then

					' Customer data ...............................................................................................

					myRegEx_0 = New Regex(REGEX_Company1)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_CustomerData.Company1)
					End While

					myRegEx_0 = New Regex(REGEX_Company2)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_CustomerData.Company2)
					End While

					myRegEx_0 = New Regex(REGEX_Company3)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_CustomerData.Company3)
					End While

					myRegEx_0 = New Regex(REGEX_CompanyStreet)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_CustomerData.Street)
					End While

					myRegEx_0 = New Regex(REGEX_CustomerPostcodeLocation)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_CustomerData.CustomerPostcodeLocation)
					End While

				End If


				' Benutzerdaten ...............................................................................................
				myRegEx_0 = New Regex(REGEX_USAnrede)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserSalutation)
				End While

				myRegEx_0 = New Regex(REGEX_USNachname)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserLName)
				End While

				myRegEx_0 = New Regex(REGEX_USVorname)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserFName)
				End While

				myRegEx_0 = New Regex(REGEX_USTelefon)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserTelefon)
				End While

				myRegEx_0 = New Regex(REGEX_USNatel)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMobile)
				End While

				myRegEx_0 = New Regex(REGEX_USTelefax)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserTelefax)
				End While

				myRegEx_0 = New Regex(REGEX_USeMail)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UsereMail)
				End While

				myRegEx_0 = New Regex(REGEX_USPostfach)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDPostfach)
				End While

				myRegEx_0 = New Regex(REGEX_USStrasse)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDStrasse)
				End While

				myRegEx_0 = New Regex(REGEX_USPLZ)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDPLZ)
				End While

				myRegEx_0 = New Regex(REGEX_USOrt)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDOrt)
				End While

				myRegEx_0 = New Regex(REGEX_USLand)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDLand)
				End While

				myRegEx_0 = New Regex(REGEX_USAbteilung)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserFTitel)
				End While

				myRegEx_0 = New Regex(REGEX_USMDName)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDName)
				End While

				myRegEx_0 = New Regex(REGEX_USMDName2)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDName2)
				End While

				myRegEx_0 = New Regex(REGEX_USMDPostfach)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDPostfach)
				End While

				myRegEx_0 = New Regex(REGEX_USMDStrasse)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDStrasse)
				End While

				myRegEx_0 = New Regex(REGEX_USMDOrt)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDOrt)
				End While

				myRegEx_0 = New Regex(REGEX_USMDPlz)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDPLZ)
				End While

				myRegEx_0 = New Regex(REGEX_USMDLand)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDLand)
				End While

				myRegEx_0 = New Regex(REGEX_USMDTelefon)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDTelefon)
				End While

				myRegEx_0 = New Regex(REGEX_USMDTelefax)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDTelefax)
				End While

				myRegEx_0 = New Regex(REGEX_USMDeMail)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDeMail)
				End While

				myRegEx_0 = New Regex(REGEX_USMDHomepage)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserMDHomepage)
				End While

				myRegEx_0 = New Regex(REGEX_USTitel_1)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserFTitel)
				End While

				myRegEx_0 = New Regex(REGEX_USTitel_2)
				result_0 = FullFileName.Document.StartSearch(myRegEx_0)
				While result_0.FindNext()
					result_0.Replace(m_InitializationData.UserData.UserSTitel)
				End While


			Catch ex As Exception
				'      ParsedFile = String.Empty
				'      MsgBox("Feher: " & ex.Message.Trim & vbCrLf & ParsedFile & vbCrLf & pattern & vbCrLf & line)

			End Try

			Return ParsedFile
		End Function

		Private Function SendMailToWithExchange(ByVal strFrom As String, ByVal strTo As String,
												ByVal CCAddress As String, ByVal BCCAddress As String,
												ByVal strSubject As String, ByVal strBody As String,
												ByVal aAttachmentFile As List(Of String)) As MailSendValue

			Dim result As New MailSendValue With {.Value = True, .ValueLable = String.Empty}
			Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
			Dim mailmsg As New System.Net.Mail.MailMessage
			Dim smtpServer = EMailSMTPServer
			Dim smtpPort As Integer = Val(EMailSMTPServerPort)
			Dim enableSSL As Boolean = EMailEnableSSL
			Dim smtpDelivery As SmtpDeliveryMethod = DeliveryMethod

			Dim strToAdresses = strTo.Split(CChar(";")).ToList()
			Dim strCCAddress = CCAddress.Split(CChar(";")).ToList()
			Dim strBCCAddress = BCCAddress.Split(CChar(";")).ToList()

			Try
				Dim exporter As New RichEditMailMessageExporter(m_RtfContent, mailmsg)
				exporter.Export()

				If smtpPort = 0 Then smtpPort = 25

				Dim strEx_UserName As String = m_UserData.EMail_UserName
				Dim strEx_UserPW As String = m_UserData.EMail_UserPW
				Dim _ClsProgsetting As New SPProgUtility.ClsProgSettingPath

				If Not String.IsNullOrWhiteSpace(strEx_UserName) Then strEx_UserName = _ClsProgsetting.DecryptBase64String(strEx_UserName)
				If Not String.IsNullOrWhiteSpace(strEx_UserPW) Then strEx_UserPW = _ClsProgsetting.DecryptBase64String(strEx_UserPW)

#If DEBUG Then
				smtpServer = "smtpserver"
				strEx_UserName = "username"
				strEx_UserPW = "password"
				smtpPort = 587
				enableSSL = True

				strFrom = "info@domain.com"
				strToAdresses = New List(Of String) From {"user@domain.com"}
#End If

				With mailmsg
					.IsBodyHtml = True
					.To.Clear()
					.From = New MailAddress(strFrom)

					For Each toItem In strToAdresses
						If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
					Next

					.CC.Clear()
					For Each ccItem In strCCAddress
						If Not String.IsNullOrWhiteSpace(ccItem) Then .CC.Add(New MailAddress(ccItem.Trim))
					Next

					.Bcc.Clear()
					For Each bccItem In strBCCAddress
						If Not String.IsNullOrWhiteSpace(bccItem) Then .Bcc.Add(New MailAddress(bccItem.Trim))
					Next

					.ReplyToList.Clear()
					.ReplyToList.Add(.From)

					.Subject = strSubject.Trim()
					.Body = strBody.Trim()

					.Priority = Net.Mail.MailPriority.High
					If Not aAttachmentFile Is Nothing AndAlso aAttachmentFile.Count > 0 Then
						For Each itm In aAttachmentFile
							If File.Exists(itm) Then
								.Attachments.Add(New System.Net.Mail.Attachment(itm))
							End If
						Next
					End If

					Dim askMissingAttachment As Boolean = True
					Select Case PreselectionData.MailType
						Case MailTypeEnum.ApplicantOKNotification
							askMissingAttachment = False
						Case MailTypeEnum.ApplicantCancelNotification
							askMissingAttachment = False
						Case MailTypeEnum.ApplicationOKNotification
							askMissingAttachment = False
						Case MailTypeEnum.ApplicationCancelNotification
							askMissingAttachment = False

						Case MailTypeEnum.INVOICE, MailTypeEnum.MOREINVOICES
							askMissingAttachment = True

						Case MailTypeEnum.PAYROLL, MailTypeEnum.MOREPAYROLLS
							askMissingAttachment = True

					End Select

					If askMissingAttachment AndAlso .Attachments.Count = 0 Then
						Dim attachmentResult = m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue("Sie senden ein Email ohne Anhang. Möchten Sie das Email trotzdem senden?")),
																															 m_Translate.GetSafeTranslationValue("Email-Anhang"))
						m_Logger.LogInfo(String.Format("sendig email without attachment: {0}", attachmentResult))
						If attachmentResult = False Then Return New MailSendValue With {.Value = False, .ValueLable = "Fehlende Datei"}
					End If

				End With

				Try
					If Not String.IsNullOrWhiteSpace(strEx_UserName) Then
						Dim mailClient As New System.Net.Mail.SmtpClient(smtpServer, smtpPort)
						mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
						mailClient.EnableSsl = enableSSL
						If smtpDelivery = Nothing Then mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network Else mailClient.DeliveryMethod = smtpDelivery
						m_Logger.LogInfo(String.Format("sending email with authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | smtpServer: {2} | smtpPort: {3}", enableSSL, mailClient.DeliveryMethod.ToString, smtpServer, smtpPort))

						mailClient.Send(mailmsg)
						mailClient.Dispose()

					Else

						obj.Port = smtpPort
						obj.EnableSsl = enableSSL
						If smtpDelivery = Nothing Then obj.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network Else obj.DeliveryMethod = smtpDelivery
						obj.Host = smtpServer

						m_Logger.LogInfo(String.Format("sending email with NO authentication>>>EnableSsl: {0} | mailClient.DeliveryMethod: {1} | smtpServer: {2} | smtpPort: {3}", enableSSL, obj.DeliveryMethod.ToString, smtpServer, smtpPort))

						obj.Send(mailmsg)

					End If


					result.Value = True
					m_Logger.LogDebug(String.Format("SendMailToWithExchange: {0}", result.Value))

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.", ex.ToString()))
					result.Value = False
					result.ValueLable = String.Format("(Eror: SendMailToWithExchange) Fehler_1: AN: {0} From: {1} Message: {2}", strTo, strFrom, ex.ToString())

				Finally
					obj.Dispose()
					If Not mailmsg.Attachments Is Nothing Then mailmsg.Attachments.Dispose()
					mailmsg.Dispose()

				End Try
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.", ex.Message))
				result.Value = False
				result.ValueLable = String.Format("(Eror: SendMailToWithExchange) Fehler_2: AN: {0} From: {1} Message: {2}", strTo, strFrom, ex.ToString)

			Finally
				obj.Dispose()
				mailmsg.Dispose()

			End Try

			Return result
		End Function

		''' <summary>
		''' Combines date and time.
		''' </summary>
		''' <param name="dateComponent">The date component.</param>
		''' <param name="timeComponent">The time component (date is ignored)</param>
		''' <returns>Combined date and time</returns>
		Private Function CombineDateAndTime(ByVal dateComponent As DateTime?, ByVal timeComponent As DateTime?) As DateTime?

			If Not dateComponent.HasValue Then
				Return Nothing
			End If

			If Not timeComponent.HasValue Then
				Return dateComponent.Value.Date
			End If

			Dim timeSpan As TimeSpan = timeComponent.Value - timeComponent.Value.Date
			Dim dateAndTime = dateComponent.Value.Date.Add(timeSpan)

			Return dateAndTime
		End Function

		Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Me.Close()
		End Sub


#End Region



		Private Class InvoiceEMailMergeViewData
			Inherits InvoiceEMailMergeData

			Public Property ProcessState As ProcessState
			Public Property SendState As MailSendValue

		End Class

	End Class

End Namespace

