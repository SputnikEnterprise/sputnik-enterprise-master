Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Threading
Imports SP.Internal.Automations.Settings_
Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService
Imports SP.Internal.Automations.SPeCallWebService
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.IO


Public Class frmMessageDetails


#Region "Private Consts"

	Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
	Public Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx"
	'Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "https://soap.ecall.ch/eCall.asmx"

	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

#End Region

#Region "Privte Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_PaymentUtilWebServiceUri As String

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_eCallWebServiceUri As String

	Private m_AccountName As String
	Private m_AccountPassword As String

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility


	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant
	Private m_CustomerID As String


#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_SettingsManager = New SettingsManager
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

			m_AccountName = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, m_InitializationData.MDData.MDNr)))
			m_AccountPassword = m_ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, m_InitializationData.MDData.MDNr)))

			m_eCallWebServiceUri = MANDANT_XML_SETTING_SPUTNIK_ECALL_URI

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)



		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


		Reset()


	End Sub

#End Region

#Region "Public Properties"


#End Region


#Region "Public Methods"

	Public Function LoadData(ByVal Customer_ID As String, ByVal id As Integer) As Boolean
		Dim result As Boolean = True
		Dim data = m_ListingDatabaseAccess.GetAssignedMailData(Customer_ID, id)

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Nachrichten-Details konnten nocht geladen werden."))
			Return False
		End If
		eMail_From.Text = data.email_from
		eMail_To.Text = data.email_to
		eMail_Subject.Text = data.email_subject
		lblAmValue.Text = data.createdon
		lblDurchValue.Text = data.createdfrom

		If (Me.wbHtml.Document Is Nothing) Then
			Me.wbHtml.DocumentText = data.email_body
		Else
			Me.wbHtml.Document.OpenNew(True)
			Me.wbHtml.Document.Write(data.email_body)
		End If

		Try
			m_Logger.LogDebug(String.Format("Customer_ID: {0} | ID: {1} | Message_ID: {2}", Customer_ID, id, data.messageID))
			Dim attachmentData = m_ListingDatabaseAccess.LoadAssignedEMailAttachmentData(Customer_ID, data.messageID)

			If Not attachmentData Is Nothing Then
				lstAttachments.DataSource = attachmentData
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return (Not data Is Nothing)

	End Function


#End Region


#Region "Private Methods"


	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		eMail_From.Text = String.Empty
		eMail_To.Text = String.Empty
		eMail_Subject.Text = String.Empty
		lblAmValue.Text = String.Empty
		lblDurchValue.Text = String.Empty

		lstAttachments.Items.Clear()
		wbHtml.DocumentText = String.Empty

		ResetAttachmentList()

		' Translate controls
		TranslateControls()

	End Sub

	Private Sub ResetAttachmentList()

		' Reset the Listbox
		lstAttachments.DisplayMember = "filename"
		lstAttachments.ValueMember = "ID"

		lstAttachments.Items.Clear()
		lstAttachments.DataSource = Nothing

		m_SuppressUIEvents = False

	End Sub



	''' <summary>
	'''  Translate controls
	''' </summary>
	Private Sub TranslateControls()

		Text = m_Translate.GetSafeTranslationValue(Text)

		lblAn.Text = m_Translate.GetSafeTranslationValue(lblAn.Text)
		lblVon.Text = m_Translate.GetSafeTranslationValue(lblVon.Text)

		grpFilter.Text = m_Translate.GetSafeTranslationValue(grpFilter.Text)

		lblBetreff.Text = m_Translate.GetSafeTranslationValue(lblBetreff.Text)
		lblAm.Text = m_Translate.GetSafeTranslationValue(lblAm.Text)
		lblBenutzer.Text = m_Translate.GetSafeTranslationValue(lblBenutzer.Text)

		lblDateianhang.Text = m_Translate.GetSafeTranslationValue(lblDateianhang.Text)
		lblNachricht.Text = m_Translate.GetSafeTranslationValue(lblNachricht.Text)

	End Sub

	Private Sub lstAttachments_DoubleClick(sender As Object, e As EventArgs) Handles lstAttachments.DoubleClick

		Dim selecteddata = CType(lstAttachments.SelectedItem, ListingMailAttachmentData)
		If Not selecteddata Is Nothing Then
			Dim bytes() = m_ListingDatabaseAccess.LoadAssignedEMailAttachmentFile(selecteddata.customer_id, selecteddata.ID)
			Dim tempFileName = System.IO.Path.GetTempFileName()
			Dim m_CurrentFileExtension = String.Empty

			If selecteddata.filename <> String.Empty Then
				m_CurrentFileExtension = System.IO.Path.GetExtension(selecteddata.filename)
				m_CurrentFileExtension = m_CurrentFileExtension.Replace(".", "")
			End If

			Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, m_CurrentFileExtension)

			If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

				m_Utility.OpenFileWithDefaultProgram(tempFileFinal)


			End If

		End If

	End Sub


#End Region



End Class
