
Imports System.IO.File
Imports System.Data.SqlClient

Imports SP.Infrastructure.Logging
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports System.Linq
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.WOS
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.Propose
Imports SP.DatabaseAccess.Vacancy

Namespace WOSDataTransfer



	Public Class SendScanJobTOWOS

#Region "Private Consts"

		Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
		Public Const DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPWOSCustomerUtilities.asmx"
		Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"
		Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_VACANCY_GUID As String = "MD_{0}/Export/Vak_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_SENDEMPLOYEEDATA_WOS_AFTER_PROPOSE As String = "MD_{0}/Export/sendemployeedatatowosaftersendingpropose"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "MD_{0}/Interfaces/webservices/webserviceecall"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

#End Region


#Region "private fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		'Private m_ClsProgSetting As SPProgUtility.ClsProgSettingPath
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The payroll data access object.
		''' </summary>
		Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

		''' <summary>
		''' The customer data access object.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The wos data access object.
		''' </summary>
		Private m_WOSDatabaseAccess As IWOSDatabaseAccess

		''' <summary>
		''' The report data access object.
		''' </summary>
		Private m_RPDatabaseAccess As IReportDatabaseAccess

		''' <summary>
		''' The invocie data access object.
		''' </summary>
		Private m_REDatabaseAccess As IInvoiceDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The vacancy database access.
		''' </summary>
		Protected m_VacancyDatabaseAccess As IVacancyDatabaseAccess

		''' <summary>
		''' The employment data access object.
		''' </summary>
		Private m_ESDatabaseAccess As IESDatabaseAccess

		''' <summary>
		''' The propose data access object.
		''' </summary>
		Private m_ProposeDatabaseAccess As IProposeDatabaseAccess

		''' <summary>
		''' The payroll data access object.
		''' </summary>
		Private m_LODatabaseAccess As IPayrollDatabaseAccess

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility
		Private m_utilitySP As SPProgUtility.MainUtilities.Utilities

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		''' <summary>
		''' Service Uri of Sputnik bank util webservice.
		''' </summary>
		Private m_CustomerWosUtilWebServiceUri As String
		Private m_connectionString As String

		Private m_EmployeeWOSID As String
		Private m_CustomerWOSID As String
		Private m_VacancyWOSID As String

		Private m_EmployeeGuid As String
		Private m_CustomerGuid As String
		Private m_ZHDGuid As String
		Private m_ReportGuid As String
		Private m_EmployeeEmploymentGuid As String
		Private m_CustomerEmploymentGuid As String
		Private m_DocumentGuid As String
		Private m_CurrentEmployeeNumber As Integer?
		Private m_CurrentCustomerNumber As Integer?
		Private m_CurrentESNumber As Integer?
		Private m_CurrentReportNumber As Integer?
		Private m_SendEmployeeAfterPropose As Boolean


#End Region


#Region "constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_utilitySP = New SPProgUtility.MainUtilities.Utilities

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_WOSDatabaseAccess = New WOSDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_RPDatabaseAccess = New ReportDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_REDatabaseAccess = New InvoiceDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

				m_CustomerWosUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

				If String.IsNullOrWhiteSpace(m_CustomerWosUtilWebServiceUri) Then
					m_CustomerWosUtilWebServiceUri = DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI
				End If

				m_EmployeeWOSID = EmployeeWOSID
				m_CustomerWOSID = CustomerWOSID
				m_VacancyWOSID = VacancyWOSID
				m_sendEmployeeAfterPropose = SendEmployeeDataToWOSAfterSendingPropose

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_CustomerWosUtilWebServiceUri = DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI
			End Try

		End Sub


#End Region

#Region "private properties"

		Private ReadOnly Property EmployeeWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr))

				Return value
			End Get
		End Property

		Private ReadOnly Property CustomerWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

				Return value
			End Get
		End Property

		Private ReadOnly Property VacancyWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_VACANCY_GUID, m_InitializationData.MDData.MDNr))

				Return value
			End Get
		End Property

		Private ReadOnly Property SendEmployeeDataToWOSAfterSendingPropose() As Boolean
			Get
				Dim value = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_SENDEMPLOYEEDATA_WOS_AFTER_PROPOSE, m_InitializationData.MDData.MDNr)), False)

				Return value
			End Get
		End Property

#End Region


	End Class


End Namespace
