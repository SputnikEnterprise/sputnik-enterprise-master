
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports DevExpress.Pdf
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


Public Class frmPDFFormData


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


	Private Sub txtCompanies_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtPDFFields.ButtonClick

		If e.Button.Index = 0 Then
			If OpenPDFFile() Then
				LoadPDFFields()
			Else
				txtPDFFields.EditValue = String.Empty
			End If

		Else
			LoadPDFFields()

		End If

	End Sub

	Private Function OpenPDFFile() As Boolean
		Dim result As Boolean = True

		Dim openDlg = New OpenFileDialog
		With openDlg
			.Filter =
			"PDF-Dokumente (*.pdf)|*.pdf"
			.FilterIndex = 3
			.InitialDirectory = If(String.IsNullOrWhiteSpace(txtPDFFields.EditValue), m_ClsProgSetting.GetUserHomePath, Path.GetDirectoryName(txtPDFFields.EditValue))
			.Title = m_Translate.GetSafeTranslationValue("Dokument öffnen")
			.FileName = String.Empty

			If .ShowDialog() <> DialogResult.OK Then Return False

			txtPDFFields.Text = openDlg.FileName
			PdfViewer1.LoadDocument(.FileName)

		End With

		Return result
	End Function

	Private Sub LoadPDFFields()

		Dim result As New List(Of String)
		Dim pdfFilename = txtPDFFields.EditValue

		txtPDFFormResult.EditValue = String.Empty
		If Not File.Exists(pdfFilename) Then Return

		Using documentProcessor As New PdfDocumentProcessor()
			documentProcessor.LoadDocument(pdfFilename)

			' Get names of interactive form fields.
			Dim formData As PdfFormData = documentProcessor.GetFormData()
			Dim names As IList(Of String) = formData.GetFieldNames()

			' Show the field names in the rich text box.
			Dim strings(names.Count - 1) As String
			names.CopyTo(strings, 0)

			result = (strings.ToList())

			result = strings.ToList()
		End Using

		txtPDFFormResult.EditValue = String.Join(vbNewLine, result)

	End Sub


End Class