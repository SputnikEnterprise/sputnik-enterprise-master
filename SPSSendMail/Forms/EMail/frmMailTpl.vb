
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

Namespace RichEditSendMail

	Public Class frmMailTpl


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


#End Region


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



#Region "private properties"


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

		''' <summary>
		''' Gets the selected document.
		''' </summary>
		''' <returns>The selected document or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedRecord As EMailAttachmentDocumentData
			Get
				Dim gvRP = TryCast(gridDocuments.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim doc = CType(gvRP.GetRow(selectedRows(0)), EMailAttachmentDocumentData)

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
		Private ReadOnly Property SelectedRecordToSelect As EMailAttachmentDocumentData
			Get
				Dim gvRP = TryCast(grdFileToSelect.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvRP Is Nothing) Then

					Dim selectedRows = gvRP.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim doc = CType(gvRP.GetRow(selectedRows(0)), EMailAttachmentDocumentData)

						Return doc
					End If

				End If

				Return Nothing
			End Get

		End Property

		'''' <summary>
		'''' Gets the selected documentlist to merge.
		'''' </summary>
		'''' <returns>The selected document or nothing if none is selected.</returns>
		'Private ReadOnly Property SelectedRecordToMerge As EMailAttachmentDocumentData
		'	Get
		'		Dim gvRP = TryCast(grdSelectedFile.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

		'		If Not (gvRP Is Nothing) Then

		'			Dim selectedRows = gvRP.GetSelectedRows()

		'			If (selectedRows.Count > 0) Then
		'				Dim doc = CType(gvRP.GetRow(selectedRows(0)), EMailAttachmentDocumentData)

		'				Return doc
		'			End If

		'		End If

		'		Return Nothing
		'	End Get

		'End Property

#End Region


#Region "public property"

		''' <summary>
		''' Gets or sets the preselection data.
		''' </summary>
		Public Property PreselectionData As PreselectionMailData
		Public Property InvoiceMailMergeData As InvoiceEMailMergeData

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

			TranslateControls()
			Reset()


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
			If m_CustomerNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadCustomerData()

			If m_ApplicationNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadApplicationData()
			If m_VacancyNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadVacancyData()
			If m_ProposeNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadProposeData()
			If m_InvoiceNumber.GetValueOrDefault(0) > 0 Then success = success AndAlso LoadInvoiceData()


			lblAbsender.Text = If(String.IsNullOrWhiteSpace(m_InitializationData.UserData.UserMDeMail), m_InitializationData.MDData.MDeMail, m_InitializationData.UserData.UserMDeMail).Trim

			LoadModulSettingData()

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

			ResetDocumentGrid()
			ResetDocumentToSelectGrid()
			ResetSelectedDocumentGrid()

		End Sub

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = String.Format(m_Translate.GetSafeTranslationValue("{0} - Nachricht (HTML)"), m_Translate.GetSafeTranslationValue("Unbenannt"))

			Me.pgrpZwischenablage.Text = m_Translate.GetSafeTranslationValue(Me.pgrpZwischenablage.Text)
			Me.pgrpFont.Text = m_Translate.GetSafeTranslationValue(Me.pgrpFont.Text)
			Me.pgrpMailvorlage.Text = m_Translate.GetSafeTranslationValue(Me.pgrpMailvorlage.Text)
			Me.pgrpFelder.Text = m_Translate.GetSafeTranslationValue(Me.pgrpFelder.Text)
			Me.PasteItem1.Caption = m_Translate.GetSafeTranslationValue(Me.PasteItem1.Caption)
			Me.bbiAnhang.Caption = m_Translate.GetSafeTranslationValue(Me.bbiAnhang.Caption)
			Me.bbiFelder.Caption = m_Translate.GetSafeTranslationValue(Me.bbiFelder.Caption)

			Me.grpVersandfelder.Text = m_Translate.GetSafeTranslationValue(Me.grpVersandfelder.Text)
			Me.btnSend.Text = m_Translate.GetSafeTranslationValue(Me.btnSend.Text)
			Me.btnSender.Text = m_Translate.GetSafeTranslationValue(Me.btnSender.Text)
			Me.btnRecipient.Text = m_Translate.GetSafeTranslationValue(Me.btnRecipient.Text)
			Me.btn_Translate.Caption = m_Translate.GetSafeTranslationValue(Me.btn_Translate.Caption)
			Me.lblCC.Text = m_Translate.GetSafeTranslationValue(Me.lblCC.Text)
			Me.lblBCC.Text = m_Translate.GetSafeTranslationValue(Me.lblBCC.Text)
			Me.lblBetreff.Text = m_Translate.GetSafeTranslationValue(Me.lblBetreff.Text)

			Me.grpAnhaenge.Text = m_Translate.GetSafeTranslationValue(Me.grpAnhaenge.Text)

			Me.lblUnsortierte.Text = m_Translate.GetSafeTranslationValue(Me.lblUnsortierte.Text)
			Me.lblSortierte.Text = m_Translate.GetSafeTranslationValue(Me.lblSortierte.Text)
			Me.lblDateiname.Text = m_Translate.GetSafeTranslationValue(Me.lblDateiname.Text)
			Me.sbtnCreateOnePDF.Text = m_Translate.GetSafeTranslationValue(Me.sbtnCreateOnePDF.Text)

			Me.xtabHaupt.Text = m_Translate.GetSafeTranslationValue(Me.xtabHaupt.Text)
			Me.lblStatus.Caption = m_Translate.GetSafeTranslationValue(Me.lblStatus.Caption)


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
			columnDate.Name = "DocumentLabel"
			columnDate.FieldName = "DocumentLabel"
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
			columnDate.Name = "DocumentLabel"
			columnDate.FieldName = "DocumentLabel"
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
			columnDate.Name = "DocumentLabel"
			columnDate.FieldName = "DocumentLabel"
			columnDate.Visible = True
			gvSelectedFile.Columns.Add(columnDate)


			grdSelectedFile.DataSource = Nothing

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

#End Region

		Private Sub LoadModulSettingData()

			m_TemplatePath = m_InitializationData.MDData.MDTemplatePath
			Select Case PreselectionData.MailType
				Case MailTypeEnum.PAYROLL, MailTypeEnum.ApplicantOKNotification, MailTypeEnum.ApplicationOKNotification, MailTypeEnum.ApplicationCancelNotification
					If Not String.IsNullOrWhiteSpace(m_EmployeeData.Email) Then txt_MailAn.EditValue = String.Format("{0}", m_EmployeeData.Email).Trim

				Case MailTypeEnum.INVOICE
					txt_MailAn.EditValue = String.Format("{0}", m_InvoiceData.ReMail).Trim
					m_TemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice")

				Case MailTypeEnum.ZV, MailTypeEnum.ARGB
					txt_MailAn.EditValue = If(String.IsNullOrWhiteSpace(m_EmployeeContactCommData.ZVeMail), String.Format("{0}", m_EmployeeData.Email), String.Format("{0}", m_EmployeeContactCommData.ZVeMail)).Trim

				Case MailTypeEnum.EMPLOYEECOMMON
					m_TemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Common")
					If Not Directory.Exists(m_TemplatePath) Then Directory.CreateDirectory(m_TemplatePath)

					txt_MailAn.EditValue = String.Format("{0}", m_EmployeeData.Email).Trim

				Case MailTypeEnum.CUSTOMERCOMMON
					m_TemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Common")
					txt_MailAn.EditValue = String.Format("{0}", m_CustomerData.EMail).Trim


				Case Else
					txt_MailAn.EditValue = String.Format("{0}", m_EmployeeData.Email).Trim

			End Select
			Try
				If Not Directory.Exists(m_TemplatePath) Then Directory.CreateDirectory(m_TemplatePath)
			Catch ex As Exception
				m_TemplatePath = m_InitializationData.MDData.MDTemplatePath

			End Try


			Try
				Dim success As Boolean = LoadSenderData()
				success = success AndAlso LoadRecipientData()
				If success Then LoadEMailAttachmentData()
				success = success AndAlso LoadEMailTemplateData()

				CreateSenderPopupMenu()
				CreateRecipientPopupMenu()

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return
			End Try

		End Sub

		''' <summary>
		''' Load template data.
		''' </summary>
		Private Function LoadEMailTemplateData() As Boolean
			Dim eMailType As String = "Mail.{0}.01"

			Me.rep_MailtplGroup.Items.Clear()
			Select Case PreselectionData.MailType
				Case MailTypeEnum.ARGB
					eMailType = String.Format("Mail.{0}.%", "ARGB")

				Case MailTypeEnum.ZV
					eMailType = String.Format("Mail.{0}.%", "ZV")

				Case MailTypeEnum.PAYROLL
					eMailType = String.Format("Mail.{0}.%", "Payroll")

				Case MailTypeEnum.INVOICE
					eMailType = String.Format("Mail.{0}.%", "Invoice")
				Case MailTypeEnum.MOREINVOICES
					eMailType = String.Format("Mail.{0}.%", "MoreInvoices")

				Case MailTypeEnum.ApplicantOKNotification
					eMailType = String.Format("Mail.{0}.%", "ApplicantOKNotification")

				Case MailTypeEnum.ApplicantCancelNotification
					eMailType = String.Format("Mail.{0}.%", "ApplicantCancelNotification")

				Case MailTypeEnum.ApplicationOKNotification
					eMailType = String.Format("Mail.{0}.%", "ApplicationOKNotification")

				Case MailTypeEnum.ApplicationCancelNotification
					eMailType = String.Format("Mail.{0}.%", "ApplicationCancelNotification")

				Case MailTypeEnum.EMPLOYEECOMMON
					eMailType = String.Format("Mail.{0}.%", "EmployeeCommon")

				Case MailTypeEnum.CUSTOMERCOMMON
					eMailType = String.Format("Mail.{0}.%", "CustomerCommon")

				Case MailTypeEnum.NOTDEFINED
					eMailType = String.Format("Mail.{0}.%", "Common")


				Case Else
					Return False

			End Select

			m_EMailTemplateData = m_ListingDatabaseAccess.LoadTemplateDataToSendEmail(eMailType)

			If (m_EMailTemplateData Is Nothing OrElse m_EMailTemplateData.Count = 0) Then
				pgrpMailvorlage.Visible = False
				If PreselectionData.MailType = MailTypeEnum.NOTDEFINED Then Return False
				m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("EMail-Vorlagedaten ({0}) konnten nicht geladen werden."), eMailType), m_Translate.GetSafeTranslationValue("Fehlende Vorlage"), MessageBoxIcon.Asterisk)

				Return False
			End If

			For Each itm In m_EMailTemplateData
				Me.rep_MailtplGroup.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem With {.Description = itm.DocumentLabel, .Value = itm.DocumentName})
			Next

			Return Not m_EMailTemplateData Is Nothing
		End Function

		Private Function LoadEMailAttachmentData() As Boolean
			Dim result As Boolean = True
			Dim listDataSource As BindingList(Of EMailAttachmentDocumentData) = New BindingList(Of EMailAttachmentDocumentData)

			Try
				If PreselectionData.PDFFilesToSend Is Nothing OrElse PreselectionData.PDFFilesToSend.Count = 0 Then Return False
				Dim attachment = PreselectionData.PDFFilesToSend

				For Each itm In attachment
					If itm <> String.Empty Then
						Dim documentViewData = New EMailAttachmentDocumentData With {.DocumentLabel = Path.GetFileNameWithoutExtension(itm),
							.DocumentFilename = itm,
																																	.DocContent = m_Utility.LoadFileBytes(itm),
																																	.ScanExtension = Path.GetExtension(itm),
																																	.Checked = True}

						listDataSource.Add(documentViewData)

					End If

				Next

				m_EmployeeDocumentData = listDataSource
				gridDocuments.DataSource = listDataSource

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try


			Return Not listDataSource Is Nothing

		End Function

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

		Private Function LoadRecipientData() As Boolean
			Dim result As Boolean = True
			Try
				Dim data As New List(Of String)
				Select Case PreselectionData.MailType
					Case MailTypeEnum.ARGB
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.ZV
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.PAYROLL
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.INVOICE
						data = New List(Of String) From {m_InvoiceData.ReMail, m_CustomerData.EMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.ApplicantOKNotification
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.ApplicantCancelNotification
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.ApplicationOKNotification
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.ApplicationCancelNotification
						data = New List(Of String) From {m_EmployeeData.Email, m_EmployeeContactCommData.ZVeMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.EMPLOYEECOMMON
						data = New List(Of String) From {m_EmployeeData.Email, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.CUSTOMERCOMMON
						data = New List(Of String) From {m_CustomerData.EMail, m_InitializationData.UserData.UserMDeMail}

					Case MailTypeEnum.NOTDEFINED
						If Not m_CustomerData Is Nothing Then data = New List(Of String) From {m_CustomerData.EMail, m_InitializationData.UserData.UserMDeMail}
						If Not m_EmployeeData Is Nothing Then data = New List(Of String) From {m_EmployeeData.Email, m_InitializationData.UserData.UserMDeMail}


					Case Else
						Return False

				End Select

				data.RemoveAll(Function(str) String.IsNullOrEmpty(str))
				m_RecipientEMailData = data.GroupBy(Function(m) m).Where(Function(g) g.Count() = 1).Select(Function(g) g.Key).ToList

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return result

		End Function

		Private Sub CreateSenderPopupMenu()
			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			Dim barmanager As New DevExpress.XtraBars.BarManager
			barmanager.Images = ImageCollection1
			Me.btnSender.DropDownControl = popupMenu

			popupMenu.Manager = barmanager

			Dim itm As New DevExpress.XtraBars.BarButtonItem
			itm = New DevExpress.XtraBars.BarButtonItem

			Try

				For Each address In m_SenderEMailData
					itm = New DevExpress.XtraBars.BarButtonItem With {.Caption = address, .ImageIndex = 3}
					popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetMenuItem

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.StackTrace)

			End Try

		End Sub

		Private Sub CreateRecipientPopupMenu()
			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			Dim barmanager As New DevExpress.XtraBars.BarManager
			barmanager.Images = ImageCollection1
			Me.btnRecipient.DropDownControl = popupMenu

			popupMenu.Manager = barmanager

			Dim itm As New DevExpress.XtraBars.BarButtonItem
			itm = New DevExpress.XtraBars.BarButtonItem

			Try
				For Each address In m_RecipientEMailData
					itm = New DevExpress.XtraBars.BarButtonItem With {.Caption = address, .ImageIndex = 3}
					popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetKDMenuItem

				Next

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

		Private Sub OnbtnSender_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSender.Click
			btnSender.ShowDropDown()
		End Sub

		Private Sub OnbtnRecipient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecipient.Click
			btnRecipient.ShowDropDown()
		End Sub

		Private Sub rep_MailtplGroup_EditValueChanged(sender As Object, e As EventArgs) Handles rep_MailtplGroup.EditValueChanged
			Dim m_TemplateData = New EMailTemplateData

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try
				Dim selectedTplValue = CType(sender, DevExpress.XtraEditors.RadioGroup).EditValue

				LoadmyMailTemplate(Path.Combine(m_TemplatePath, selectedTplValue))
				Me.rtfContent.Focus()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))

			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Sub

		Private Sub OnbtnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
			Dim senderAddress As String = lblAbsender.Text
			Dim recipientAddress As String = String.Format("{0}", txt_MailAn.EditValue)
			Dim copyAddress As String = String.Format("{0}", txt_MailCc.EditValue)
			Dim blindAddress As String = String.Format("{0}", txt_MailBcc.EditValue)
			Dim smtpServer = EMailSMTPServer



			If String.IsNullOrWhiteSpace(senderAddress) Then senderAddress = m_InitializationData.UserData.UserMDeMail
			If String.IsNullOrWhiteSpace(recipientAddress) Then recipientAddress = m_InitializationData.UserData.UsereMail
			If String.IsNullOrWhiteSpace(txt_MailSubject.EditValue) Then
				txt_MailSubject.EditValue = "Versand neue Nachricht"
			End If

			Dim data = CType(gridDocuments.DataSource, BindingList(Of EMailAttachmentDocumentData))
			Dim attachments As New List(Of String)
			If Not data Is Nothing AndAlso data.Count > 0 Then
				Dim selectedAttachments = data.Where(Function(x) x.Checked = True).ToList

				For Each itm In selectedAttachments
					If File.Exists(itm.DocumentFilename) Then attachments.Add(itm.DocumentFilename)
				Next
			End If

			Dim result = SendMailToWithExchange(senderAddress, recipientAddress, copyAddress, blindAddress, txt_MailSubject.EditValue, rtfContent.RtfText, attachments)

			If Not result.Value Then
				Me.lblStatus.Caption = String.Format(m_Translate.GetSafeTranslationValue("Fehler in der Übermittlung der Nachricht: {0}"), result.ValueLable)
				m_UtilityUI.ShowErrorDialog(lblStatus.Caption)

				Return
			End If

			Dim strMessage = "Ihre Nachricht wurde erfolgreich gesendet."
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(strMessage))
			If senderAddress = recipientAddress Then Return

			Try
				Dim success As Boolean = True
				Dim title As String = String.Empty

				Select Case PreselectionData.MailType
					Case MailTypeEnum.ZV
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Zwischenverdienst"), txt_MailSubject.EditValue)
					Case MailTypeEnum.ARGB
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Arbeitgeberbescheinigung"), txt_MailSubject.EditValue)
					Case MailTypeEnum.PAYROLL
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Lohnabrechnung"), txt_MailSubject.EditValue)
					Case MailTypeEnum.MOREPAYROLLS
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Versand von mehreren Lohnabrechnungen"), txt_MailSubject.EditValue)

					Case MailTypeEnum.EMPLOYEECOMMON, MailTypeEnum.CUSTOMERCOMMON
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Versand von Unterlagen"), txt_MailSubject.EditValue)

					Case MailTypeEnum.INVOICE
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Rechnung"), txt_MailSubject.EditValue)
					Case MailTypeEnum.MOREINVOICES
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Versand von mehreren Rechnungen"), txt_MailSubject.EditValue)

					Case MailTypeEnum.ApplicantOKNotification
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Bewerber-Zusage"), txt_MailSubject.EditValue)
					Case MailTypeEnum.ApplicantCancelNotification
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Bewerber-Absage"), txt_MailSubject.EditValue)

						Dim appData As New MainViewApplicationData With {.EmployeeID = m_EmployeeNumber}
						If Not appData Is Nothing Then
							appData.ApplicationLifecycle = ApplicationLifecycelEnum.APPLICATIONREJECTED
							appData.CheckedFrom = m_InitializationData.UserData.UserFullName
							success = success AndAlso m_AppDatabaseAccess.UpdateAllApplicantApplicationFlagData(appData)
						End If

					Case MailTypeEnum.ApplicationOKNotification
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Bewerbung-Einladung"), txt_MailSubject.EditValue)

						Dim appData = m_AppDatabaseAccess.LoadAssignedApplicationDataForMainView(m_ApplicationNumber)
						If Not appData Is Nothing Then
							appData.ApplicationLifecycle = ApplicationLifecycelEnum.APPLICATIONSUCCESS
							appData.CheckedFrom = m_InitializationData.UserData.UserFullName
							success = success AndAlso m_AppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData(appData)
						End If

					Case MailTypeEnum.ApplicationCancelNotification
						title = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Bewerbung-Absage"), txt_MailSubject.EditValue)

						Dim appData = m_AppDatabaseAccess.LoadAssignedApplicationDataForMainView(m_ApplicationNumber)
						If Not appData Is Nothing Then
							appData.ApplicationLifecycle = ApplicationLifecycelEnum.APPLICATIONREJECTED
							appData.CheckedFrom = m_InitializationData.UserData.UserFullName
							success = success AndAlso m_AppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData(appData)
						End If

					Case MailTypeEnum.NOTDEFINED
						Me.Close()
						'Return


					Case Else
						title = "not defined!"
						Return

				End Select
				If Not success Then Return


				' handle contact
				Try
					Dim contactAddResult As Boolean = True
					Dim eMailData = New ListingMailData With {.AsHTML = True, .createdfrom = m_InitializationData.UserData.UserFullName, .customer_id = m_InitializationData.MDData.MDGuid,
								.email_body = rtfContent.HtmlText, .email_from = senderAddress, .email_to = recipientAddress,
								.email_subject = txt_MailSubject.EditValue, .MANr = m_EmployeeNumber, .KDNr = m_CustomerNumber, .EMail_SMTP = smtpServer}

					Dim description As String = String.Empty
					Dim contactType As String = String.Empty

					contactType = "Einzelmail"
					description = rtfContent.Text

					If m_EmployeeNumber.GetValueOrDefault(0) > 0 Then
						contactAddResult = contactAddResult AndAlso SaveEmployeeContactData(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)

					ElseIf m_CustomerNumber.GetValueOrDefault(0) > 0 Then
						contactAddResult = contactAddResult AndAlso SaveCustomerContactData(title, recipientAddress, description, contactType, Nothing)

					End If
					contactAddResult = contactAddResult AndAlso m_ListingDatabaseAccess.AddEMailLogToContactTable(m_InitializationData.MDData.MDNr, eMailData)

					If contactAddResult AndAlso Not attachments Is Nothing AndAlso attachments.Count > 0 Then

						For Each fileitm In attachments

							Dim eMailAttachmentData = New ListingMailAttachmentData With {.createdfrom = m_InitializationData.UserData.UserFullName,
											.customer_id = m_InitializationData.MDData.MDGuid,
												.email_from = senderAddress, .email_to = recipientAddress,
												.messageID = eMailData.messageID,
												.email_subject = txt_MailSubject.EditValue, .filename = fileitm,
												.scanfile = m_Utility.LoadFileBytes(fileitm)}

							contactAddResult = contactAddResult AndAlso m_ListingDatabaseAccess.AddEMailAttachmentLogToContactTable(m_InitializationData.MDData.MDNr, eMailAttachmentData)
						Next

					End If
				Catch ex As Exception

				End Try


			Catch ex As Exception
				m_Logger.LogError(String.Format("Saving LOG-Data Into MailDb Database:{0}", ex.ToString))
			End Try

			Dim iBoxResult = True
			If iBoxResult Then Me.Close()

		End Sub

		Private Sub LoadmyMailTemplate(ByVal tplFilename As String)

			Try
				If Not File.Exists(tplFilename) AndAlso tplFilename.ToLower.Contains("invoice") Then
					Dim webservice As New SP.Internal.Automations.BaseTable.SPSBaseTables(m_InitializationData)

					webservice.PerformDownloadingCommonTemplateFilesOverWebService()
				End If

				If File.Exists(tplFilename) Then
					Me.rtfContent.BeginUpdate()
					Me.rtfContent.LoadDocument(tplFilename)
					'Me.rtfContent.RtfText = _ClsDivFunc.TranslateSeletedTemplate(Me.rtfContent)
					Me.rtfContent.EndUpdate()
					ParseTemplateFile(rtfContent)

				Else
					SplashScreenManager.CloseForm(False)

					Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Die Vorlage-Datei existiert nicht.{0}{1}"), vbNewLine, tplFilename)
					m_UtilityUI.ShowErrorDialog(msg)
				End If

				Dim subject As String = String.Empty

				Select Case PreselectionData.MailType

					Case MailTypeEnum.ZV
						subject = EMailSubjectForZV
					Case MailTypeEnum.ARGB
						subject = EMailSubjectForARGB
					Case MailTypeEnum.PAYROLL
						subject = EMailSubjectForPayroll

					Case MailTypeEnum.INVOICE
						subject = EMailSubjectForInvoice
						subject = Regex.Replace(subject, REGEX_Company1, m_InvoiceData.RName1)
						subject = Regex.Replace(subject, REGEX_InvoiceNumber, String.Format("{0:F0}", m_InvoiceData.ReNr))
						subject = Regex.Replace(subject, REGEX_InvoiceDueDate, String.Format("{0:d}", m_InvoiceData.FakDat))

					Case MailTypeEnum.MOREINVOICES
						subject = EMailSubjectForMoreInvoices
						If String.IsNullOrWhiteSpace(subject) Then subject = String.Format("Versand von {0} Rechnungen", m_NumberOfInvoices)

					Case MailTypeEnum.PAYROLL
						subject = EMailSubjectForPayroll
					Case MailTypeEnum.MOREPAYROLLS
						subject = EMailSubjectForMorePayrolls
						If String.IsNullOrWhiteSpace(subject) Then subject = String.Format("Versand von {0} Lohnabrechnungen", m_NumberOfPayrolls)

					Case MailTypeEnum.ApplicantOKNotification
						subject = EMailSubjectForApplicantOKNotification

					Case MailTypeEnum.ApplicantCancelNotification
						subject = EMailSubjectForApplicantCancelNotification

					Case MailTypeEnum.ApplicationOKNotification
						subject = EMailSubjectForApplicationOKNotification

					Case MailTypeEnum.ApplicationCancelNotification
						subject = EMailSubjectForApplicationCancelNotification

					Case MailTypeEnum.EMPLOYEECOMMON, MailTypeEnum.CUSTOMERCOMMON
						If String.IsNullOrWhiteSpace(subject) Then subject = String.Format("Versand von Unterlagen")

					Case Else

				End Select

				If Not m_EmployeeData Is Nothing Then
					subject = Regex.Replace(subject, REGEX_MA_ForAnrede, If(m_EmployeeData.Gender = "M", "Herrn", "Frau"))
					subject = Regex.Replace(subject, REGEX_MA_Nachname, m_EmployeeData.Lastname)
					subject = Regex.Replace(subject, REGEX_MA_Vorname, m_EmployeeData.Firstname)
				End If

				If Not m_ApplicationData Is Nothing Then
					subject = Regex.Replace(subject, REGEX_APPLICATION_LABEL, If(String.IsNullOrWhiteSpace(m_ApplicationData.ApplicationLabel), REGEX_APPLICATION_LABEL, m_ApplicationData.ApplicationLabel))
					subject = Regex.Replace(subject, REGEX_APPLICATION_CREATEDON, String.Format("{0:d}", m_ApplicationData.CreatedOn))
				End If

				txt_MailSubject.EditValue = subject
				Me.Text = String.Format(m_Translate.GetSafeTranslationValue("{0} - Nachricht (HTML)"), subject)


			Catch ex As Exception
				SplashScreenManager.CloseForm(False)

				m_Logger.LogError(ex.StackTrace)
				Me.rtfContent.LoadDocument(tplFilename)

			End Try

		End Sub

		Private Sub OngvDocuments_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gvDocuments.MouseDown

			If e.Button = MouseButtons.Right Then
				Dim gv As DevExpress.XtraGrid.Views.Grid.GridView = sender
				gv.Focus()
				CreateAnhangPopupMenu(e.Location, 0)
			End If

		End Sub

		Private Sub OnbbiAnhang_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiAnhang.ItemClick
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

					Me.m_SelectedFile2Import = Path.GetDirectoryName(openDlg.FileNames(0))
				End If

			End With

		End Sub

		Private Sub grpAnhaenge_CustomButtonClick(sender As Object, e As Docking2010.BaseButtonEventArgs) Handles grpAnhaenge.CustomButtonClick

			If e.Button.Properties.GroupIndex = 0 Then
				LoadEMailAttachmentData()
			ElseIf e.Button.Properties.GroupIndex = 1 Then
				pcc_1Filename.HidePopup()
				ShowMergeFiles()
			End If

		End Sub

		Private Sub OngridDocuments_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles gridDocuments.DragEnter

			If (e.Data.GetDataPresent(DataFormats.FileDrop) = True) Then
				e.Effect = DragDropEffects.Copy
			End If

		End Sub

		Private Sub OngridDocuments_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles gridDocuments.DragDrop
			Dim str As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop, True), String())
			AddSelectedDocumentData(str)

		End Sub

		Private Sub sbtnSend2Sortedclst_Click(sender As System.Object, e As System.EventArgs) Handles btnSend2SortedFiles.Click

			Dim selectedDocument = SelectedRecordToSelect

			If m_SortedEmployeeDocumentData Is Nothing Then
				If Not selectedDocument Is Nothing Then
					m_SortedEmployeeDocumentData.Add(selectedDocument)
				End If

			Else
				If Not selectedDocument Is Nothing AndAlso Not m_SortedEmployeeDocumentData.Any(Function(data) data.DocumentFilename = selectedDocument.DocumentFilename) Then
					m_SortedEmployeeDocumentData.Add(selectedDocument)
				End If

			End If
			grdSelectedFile.DataSource = m_SortedEmployeeDocumentData

			Dim existSortedData = grdSelectedFile.DataSource
			Dim listDataSource As BindingList(Of EMailAttachmentDocumentData) = New BindingList(Of EMailAttachmentDocumentData)
			Dim selectdata = CType(grdFileToSelect.DataSource, BindingList(Of EMailAttachmentDocumentData))
			For Each itm In selectdata

				If Not m_SortedEmployeeDocumentData.Any(Function(data) data.DocumentFilename = itm.DocumentFilename) Then

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

		Private Sub OnsbtnCreateOnePDF_Click(sender As System.Object, e As System.EventArgs) Handles sbtnCreateOnePDF.Click
			If m_SortedEmployeeDocumentData.Count = 0 Then Exit Sub
			Dim success As Boolean = True

			Dim liFiles2Merge As New List(Of String)
			Dim finalFielname As String = Me.beFilename2Zip.Text.Replace(":", String.Empty).Replace("*", String.Empty).Replace("?", String.Empty).Replace("/", String.Empty).Replace("<", String.Empty).Replace(">", String.Empty).Replace("|", String.Empty)

			If String.IsNullOrWhiteSpace(finalFielname) Then
				Dim strZipfile2Send As String = String.Format(m_Translate.GetSafeTranslationValue("Unterlagen von {0} {1}.{2}"), m_EmployeeData.Firstname, m_EmployeeData.Lastname, "PDF")
				strZipfile2Send = strZipfile2Send.Replace(":", String.Empty).Replace("*", String.Empty).Replace("?", String.Empty).Replace("/", String.Empty).Replace("<", String.Empty).Replace(">", String.Empty).Replace("|", String.Empty)

				finalFielname = strZipfile2Send
			End If

			Dim tmpFilename = Path.GetTempFileName
			For Each itm In m_SortedEmployeeDocumentData
				success = success AndAlso m_Utility.WriteFileBytes(tmpFilename, itm.DocContent)
				If success Then liFiles2Merge.Add(itm.DocumentFilename)
			Next

			If liFiles2Merge Is Nothing OrElse liFiles2Merge.Count = 0 OrElse liFiles2Merge(0) = String.Empty Then Return
			Dim strTempFielname As String = String.Empty
			Try
				strTempFielname = String.Format("{0}{1}{2}",
																			 If(finalFielname.Contains("\"), String.Empty, m_path.GetSpSMAHomeFolder),
																			 finalFielname,
																			 If(finalFielname.ToLower.EndsWith(".pdf"), String.Empty, ".PDF"))
				If File.Exists(strTempFielname) Then File.Delete(strTempFielname)

			Catch ex As Exception
				pcc_1Filename.HidePopup()

			End Try

			Dim strMessage As String = "Die Datei wurde erfolgreich erstellt und wird automatisch als Mail-Anhang versendet."
			Try

				Dim pdfDocument As New PdfDocumentProcessor()
				Dim fileName As String = liFiles2Merge(0)
				pdfDocument.LoadDocument(fileName)
				For i As Integer = 1 To liFiles2Merge.Count - 1
					pdfDocument.AppendDocument(liFiles2Merge(i))
					pdfDocument.SaveDocument(fileName)
				Next
				pdfDocument.CloseDocument()

				File.Move(fileName, strTempFielname)
				Dim data = New BindingList(Of EMailAttachmentDocumentData)
				data.Add(New EMailAttachmentDocumentData With {.DocumentLabel = Path.GetFileNameWithoutExtension(strTempFielname),
					.DocumentFilename = strTempFielname,
																									.DocContent = m_Utility.LoadFileBytes(strTempFielname),
																									.ScanExtension = "PDF",
																									.Checked = True})
				gridDocuments.DataSource = data

				pcc_1Filename.HidePopup()
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(strMessage))

			Catch ex As Exception

				pcc_1Filename.HidePopup()

				strMessage = String.Format("Fehler: Möglicherweise sind die Dateien fehlerhaft!{0}{1}", vbNewLine, ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMessage))

			End Try

			Me.beFilename2Zip.Text = finalFielname

		End Sub

		Private Sub ShowMergeFiles()
			Dim barmgm As New BarManager

			Dim strZipfile2Send As String = String.Format(m_Translate.GetSafeTranslationValue("Unterlagen von {0} {1}.{2}"), m_EmployeeData.Firstname, m_EmployeeData.Lastname, ".PDF")

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
			m_SortedEmployeeDocumentData.Clear()

			Dim data = gridDocuments.DataSource
			grdFileToSelect.DataSource = data

		End Sub

		Private Sub AddSelectedDocumentData(ByVal filename As String())

			If Not File.Exists(filename(0)) Then Return

			Dim data = m_EmployeeDocumentData
			If filename.Length > 0 Then
				For i As Integer = 0 To filename.Length - 1
					If Str(i) <> String.Empty Then
						Dim documentViewData = New EMailAttachmentDocumentData With {.DocumentLabel = Path.GetFileNameWithoutExtension(filename(i)),
							.DocumentFilename = filename(i),
							.DocContent = m_Utility.LoadFileBytes(filename(i)),
							.ScanExtension = Path.GetExtension(filename(i)),
							.Checked = False}

						data.Add(documentViewData)
					End If
				Next i
			End If

			m_EmployeeDocumentData = data
			gridDocuments.DataSource = m_EmployeeDocumentData

		End Sub

		Private Sub RemoveSelectedDocumentData()

			Dim data As EMailAttachmentDocumentData = SelectedRecord
			If data Is Nothing Then Return
			m_EmployeeDocumentData.Remove(data)

			gridDocuments.DataSource = m_EmployeeDocumentData

		End Sub


		Private Sub CreateAnhangPopupMenu(ByVal loc As Point, ByVal itemindex As Integer)
			Dim barmanager As New DevExpress.XtraBars.BarManager

			barmanager.Images = Me.ImageList1
			barmanager.Form = Me

			If Me.gvDocuments.RowCount > 0 Then
				Dim _dXPopupMenu As DevExpress.Utils.Menu.DXPopupMenu = New DevExpress.Utils.Menu.DXPopupMenu()

				_dXPopupMenu.Items.Add(New DevExpress.Utils.Menu.DXMenuItem(m_Translate.GetSafeTranslationValue("Öffnen"), AddressOf GetAngangMenuItem, My.Resources.open_16x16))
				_dXPopupMenu.Items.Add(New DevExpress.Utils.Menu.DXMenuItem(m_Translate.GetSafeTranslationValue("Anlage entfernen"), AddressOf GetAngangMenuItem, My.Resources.delete_16x16))

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
				Dim aFileInfo As String = data.DocumentLabel
				Dim strFilename As String = data.DocumentFilename

				Try
					Dim success As Boolean = False
					success = m_Utility.WriteFileBytes(data.DocumentFilename, data.DocContent)

					If success Then
						Process.Start(data.DocumentFilename)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

				End Try

			ElseIf sender.caption.ToLower.Contains(m_Translate.GetSafeTranslationValue("entfernen")) Then
				RemoveSelectedDocumentData()

			End If

		End Sub


#Region "contact table"

		Private Function SaveEmployeeContactData(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String) As Boolean
			Dim success As Boolean = True

			Dim currentContactRecordNumber As Integer = 0
			Dim currentDocumentID As Integer = 0
			Dim contactData As EmployeeContactData = Nothing
			Dim fileContent = m_Utility.LoadFileBytes(attachedFile)

			Dim dt = DateTime.Now
			contactData = New EmployeeContactData With {.EmployeeNumber = m_EmployeeNumber, .CreatedOn = dt, .CreatedFrom = m_InitializationData.UserData.UserFullName}

			contactData.EmployeeNumber = m_EmployeeNumber
			contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
			contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
			contactData.ContactPeriodString = title
			contactData.ContactsString = description
			contactData.ContactImportant = False
			contactData.ContactFinished = False
			contactData.VacancyNumber = Nothing
			contactData.ProposeNr = Nothing
			contactData.ESNr = Nothing
			contactData.CustomerNumber = Nothing

			contactData.ChangedFrom = m_InitializationData.UserData.UserFullName
			contactData.ChangedOn = dt
			contactData.UsNr = m_InitializationData.UserData.UserNr

			' Check if the document bytes must be saved.
			If Not (attachedFile Is Nothing) And success Then

				Dim contactDocument As SP.DatabaseAccess.Employee.DataObjects.ContactMng.ContactDoc = Nothing
				contactDocument = New SP.DatabaseAccess.Employee.DataObjects.ContactMng.ContactDoc() With {.CreatedOn = dt,
																									 .CreatedFrom = m_InitializationData.UserData.UserFullName,
																									 .FileBytes = fileContent,
																									 .FileExtension = Path.GetExtension(attachedFile)}
				success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

				If success Then
					currentDocumentID = contactDocument.ID
					contactData.KontaktDocID = currentDocumentID
				End If

			End If

			' Insert contact
			contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

			If success Then
				currentContactRecordNumber = contactData.RecordNumber
			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Kontaktdaten konnten nicht gespeichert werden."))
			Else


			End If


			Return success
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

			'success = success AndAlso SaveOutgoingEmailsData(strTo)

			Return success
		End Function

		'Private Function SaveOutgoingEmailsData(ByVal strTo As String) As Boolean
		'	Dim success As Boolean = True

		'	Try
		'		Dim mailData = New OutgoingEMailData With {.Customer_ID = m_InitializationData.MDData.MDGuid, .ModulNumber = ModulNumberEnum.INVOICE}
		'		mailData.Receiver = strTo
		'		mailData.Sender = m_InitializationData.UserData.UserMDeMail
		'		mailData.CreatedUserNumber = m_InitializationData.UserData.UserNr
		'		mailData.CreatedFrom = m_InitializationData.UserData.UserFullName

		'		' Insert outgoing mail data
		'		For Each Number As Integer In m_CurrentInvoiceMailMergeData.InvoiceNumbers
		'			mailData.Number = Number
		'			success = success AndAlso m_ListingDatabaseAccess.AddOutgoingEMailData(m_InitializationData.MDData.MDGuid, mailData)
		'		Next

		'	Catch ex As Exception
		'		m_Logger.LogError(ex.ToString)

		'		success = False
		'	End Try

		'	Return success
		'End Function


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


				If Not m_VacancyData Is Nothing Then

					' vacancy ...............................................................................................
					myRegEx_0 = New Regex(REGEX_VacancyLabel)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_VacancyData.Bezeichnung)
					End While
				End If

				If Not m_ProposeData Is Nothing Then

					' vacancy ...............................................................................................
					myRegEx_0 = New Regex(REGEX_ProposeLabel)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(m_ProposeData.Bezeichnung)
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

				If Not m_ApplicationData Is Nothing Then

					' application data ...............................................................................................

					myRegEx_0 = New Regex(REGEX_ApplicationLabel)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(String.Format("{0}", m_ApplicationData.ApplicationLabel))
					End While

					myRegEx_0 = New Regex(REGEX_ApplicationCreatedOn)
					result_0 = FullFileName.Document.StartSearch(myRegEx_0)
					While result_0.FindNext()
						result_0.Replace(String.Format("{0:d}", m_ApplicationData.CreatedOn))
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


				'ParsedFile = SetSyntax(ParsedFile)


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
				Dim exporter As New RichEditMailMessageExporter(rtfContent, mailmsg)
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


#End Region


#Region "Helper classes"

		Private Class MailSendValue
			Public Property Value As Boolean
			Public Property ValueLable As String

		End Class

		Private Sub grpAnhaenge_Paint(sender As Object, e As PaintEventArgs) Handles grpAnhaenge.Paint

		End Sub


#End Region

	End Class





End Namespace

