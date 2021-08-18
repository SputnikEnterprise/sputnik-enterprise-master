
Imports System.ComponentModel
Imports System.Text
Imports DevExpress.XtraEditors
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.UserSkins

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ScanJob
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.ScanJob.DataObjects

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Initialization
Imports System.Threading.Tasks
Imports DevExpress.XtraEditors.Controls
Imports System.Threading
'Imports SP.Main.Notify.SPNotificationWebService
Imports DevExpress.XtraEditors.Repository
Imports System.IO
Imports SP.Internal.Automations

Namespace ScanJobs

	Public Class ScanJobsUtilities

#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
		Private Const DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPScanJobUtility.asmx"

		Private Const MANDANT_XML_SETTING_SPUTNIK_IMPORT_SCANREPORT_BOTH As String = "MD_{0}/Sonstiges/importscanreporttoboth"
		Private Const MANDANT_XML_SETTING_SPUTNIK_IMPORT_SCANREPORT_ZEROAMOUNT As String = "MD_{0}/Sonstiges/importscanreportzeroamount"

		Private Const MANDANT_XML_SETTING_SPUTNIK_SAVE_EMPLOYEE_EMPLOYMENT_SCAN_INTO_WOS As String = "MD_{0}/Sonstiges/saveemployeeemploymentscanintowos"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SAVE_EMPLOYEE_REPORT_SCAN_INTO_WOS As String = "MD_{0}/Sonstiges/saveemployeereportscanintowos"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SAVE_EMPLOYEE_PAYROLL_SCAN_INTO_WOS As String = "MD_{0}/Sonstiges/saveemployeepayrollscanintowos"

		Private Const MANDANT_XML_SETTING_SPUTNIK_SAVE_CUSTOMER_EMPLOYMENT_SCAN_INTO_WOS As String = "MD_{0}/Sonstiges/savecustomeremploymentscanintowos"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SAVE_CUSTOMER_REPORT_SCAN_INTO_WOS As String = "MD_{0}/Sonstiges/savecustomerreportscanintowos"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SAVE_CUSTOMER_INVOICE_SCAN_INTO_WOS As String = "MD_{0}/Sonstiges/savecustomerinvoicescanintowos"

#End Region


#Region "Private fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The scanjob data access object.
		''' </summary>
		Private m_ScanJobDatabaseAccess As IScanJobDatabaseAccess

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI


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
		Private m_connectionString As String

		''' <summary>
		''' Service Uri of Sputnik notification util webservice.
		''' </summary>
		Private m_NotificationUtilWebServiceUri As String

		''' <summary>
		''' Service Uri of Sputnik scan job util webservice.
		''' </summary>
		Private m_ScanjobUtilWebServiceUri As String

		Private m_ScanData As List(Of ScanViewData)
		Private m_ImportedScanData As BindingList(Of SP.DatabaseAccess.ScanJob.DataObjects.ScanJobData)


		'Private m_ As New BindingList(Of NotifyData)
		Private m_ExitApplication As Boolean
		Private m_NotFoundedReport As Integer

		Private m_SavescantoBoth As Boolean
		Private m_SavescanwithZeroAmount As Boolean
		Private m_CustomerReportData As ScanViewData

		Private m_SaveEmployeeEmploymentScanToWOS As Boolean
		Private m_SaveEmployeeReportScanToWOS As Boolean
		Private m_SaveEmployeePayrollScanToWOS As Boolean

		Private m_SaveCustomerEmploymentScanToWOS As Boolean
		Private m_SaveCustomerReportScanToWOS As Boolean
		Private m_SaveCustomerInvoiceScanToWOS As Boolean


#End Region


#Region "public property"

		Public Property CustomerID As String

		Public ReadOnly Property ImportedScanJobData() As BindingList(Of ScanJobData)
			Get
				Return m_ImportedScanData
			End Get
		End Property


#End Region


#Region "Constructor"

		Sub New(ByVal _setting As InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ScanJobDatabaseAccess = New ScanJobDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

			m_SavescantoBoth = SaveReportEmployeeAndCustomerBoth
			m_SavescanwithZeroAmount = SaveReportWithZeroAmount

			m_SaveEmployeeEmploymentScanToWOS = SaveEmployeeEmploymentScanIntoWOS
			m_SaveEmployeeReportScanToWOS = SaveEmployeeReportScanIntoWOS
			m_SaveEmployeePayrollScanToWOS = SaveEmployeePayrollScanIntoWOS

			m_SaveCustomerEmploymentScanToWOS = SaveCustomerEmploymentScanIntoWOS
			m_SaveCustomerReportScanToWOS = SaveCustomerReportScanIntoWOS
			m_SaveCustomerInvoiceScanToWOS = SaveCustomerInvoiceScanIntoWOS

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_ScanjobUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_SCANJOB_UTIL_WEBSERVICE_URI)

		End Sub

#End Region


#Region "Public Methodes"

		Public Function LoadNotCheckedScanJobData() As Boolean
			Dim success As Boolean = True
			m_ImportedScanData = Nothing

			SearchNotCheckedScanJoblistViaWebService()


			Return success

		End Function


#End Region


#Region "private properties"

		Private ReadOnly Property SaveReportEmployeeAndCustomerBoth() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_IMPORT_SCANREPORT_BOTH, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveReportWithZeroAmount() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_IMPORT_SCANREPORT_ZEROAMOUNT, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveEmployeeEmploymentScanIntoWOS() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SAVE_EMPLOYEE_EMPLOYMENT_SCAN_INTO_WOS, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveEmployeeReportScanIntoWOS() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SAVE_EMPLOYEE_REPORT_SCAN_INTO_WOS, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveEmployeePayrollScanIntoWOS() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SAVE_EMPLOYEE_PAYROLL_SCAN_INTO_WOS, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveCustomerEmploymentScanIntoWOS() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SAVE_CUSTOMER_EMPLOYMENT_SCAN_INTO_WOS, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveCustomerReportScanIntoWOS() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SAVE_CUSTOMER_REPORT_SCAN_INTO_WOS, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property

		Private ReadOnly Property SaveCustomerInvoiceScanIntoWOS() As Boolean
			Get
				Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SAVE_CUSTOMER_INVOICE_SCAN_INTO_WOS, m_InitializationData.MDData.MDNr))
				Dim result As Boolean?
				If String.IsNullOrWhiteSpace(value) Then
					result = Nothing
				Else
					result = CBool(value)
				End If

				Return result.HasValue AndAlso result

			End Get
		End Property


#End Region


#Region "Private Methodes"

		''' <summary>
		''' Search for scanjobs over web service.
		''' </summary>
		Private Sub SearchNotCheckedScanJoblistViaWebService()

			Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

			m_ImportedScanData = New BindingList(Of SP.DatabaseAccess.ScanJob.DataObjects.ScanJobData)
			m_ScanData = Nothing

			m_SuppressUIEvents = True
			Dim data = PerformScanJoblistWebserviceCallAsync()
			m_ScanData = data.ToList
			ValidateScancJobs()
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		'''  Performs Paidlist check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformScanJoblistWebserviceCallAsync() As BindingList(Of ScanViewData)

			Dim listDataSource As BindingList(Of ScanViewData) = New BindingList(Of ScanViewData)

#If DEBUG Then
			'm_NotificationUtilWebServiceUri = "http://localhost:44721/SPNotification.asmx"
			'CustomerID = "273B4FB4-80A2-4C80-8F48-EE84DE25E013"
#End If

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", CustomerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", CustomerID))

			' Read data over webservice
			Dim searchResult = webservice.GetScanJobsNotifications(CustomerID, True).ToList

			For Each result In searchResult

				Dim viewData = New ScanViewData With {
					.ID = result.ID,
					.Customer_ID = result.Customer_ID,
					.ModulNumber = result.ModulNumber,
					.DocumentCategoryNumber = result.DocumentCategoryNumber,
					.FoundedCodeValue = result.FoundedCodeValue,
					.ImportedFileGuid = result.ImportedFileGuid,
					.IsValid = result.IsValid,
					.CreatedOn = result.CreatedOn,
					.CheckedOn = result.CheckedOn
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		''' <summary>
		''' Finish Paidlist web service call.
		''' </summary>
		Private Sub FinishScanJoblistWebserviceCallTask(ByVal t As Task(Of BindingList(Of ScanViewData)))

			Try

				Select Case t.Status
					Case TaskStatus.RanToCompletion
						' Webservice call was successful.
						m_SuppressUIEvents = True

						m_ScanData = t.Result.ToList
						ValidateScancJobs()

						m_SuppressUIEvents = False

					Case TaskStatus.Faulted
						' Something went wrong -> log error.
						m_Logger.LogError(t.Exception.ToString())

					Case Else
						' Do nothing
				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


		End Sub


		''' <summary>
		'''  Performs assigned scanjob check.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformAssignedScanJoblistWebservice(ByVal scanID As String) As ScanViewData

			Dim listDataSource = New ScanViewData


			Dim webservice As New SPScanJobWebService.SPScanJobUtilitySoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ScanjobUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.GetAssignedScanJob(CustomerID, scanID)


			listDataSource.ID = searchResult.ID
			listDataSource.Customer_ID = searchResult.Customer_ID
			listDataSource.FoundedCodeValue = searchResult.FoundedCodeValue
			listDataSource.ModulNumber = searchResult.ModulNumber
			listDataSource.RecordNumber = searchResult.RecordNumber
			listDataSource.DocumentCategoryNumber = searchResult.DocumentCategoryNumber
			listDataSource.IsValid = searchResult.IsValid
			listDataSource.ReportYear = searchResult.ReportYear
			listDataSource.ReportMonth = searchResult.ReportMonth
			listDataSource.ReportWeek = searchResult.ReportWeek
			listDataSource.ReportFirstDay = searchResult.ReportFirstDay
			listDataSource.ReportLastDay = searchResult.ReportLastDay
			listDataSource.ReportLineID = searchResult.ReportLineID
			listDataSource.ScanContent = searchResult.ScanContent
			listDataSource.ImportedFileGuid = searchResult.ImportedFileGuid
			listDataSource.CreatedOn = searchResult.CreatedOn
			listDataSource.CreatedFrom = searchResult.CreatedFrom
			listDataSource.CheckedOn = searchResult.CheckedOn
			listDataSource.CheckedFrom = searchResult.CheckedFrom


			Return listDataSource

		End Function

		Private Function PerformUpdateAssignedScanJobWebservice(ByVal scanID As String) As Boolean

			Dim success As Boolean = True

			Dim webservice As New SPScanJobWebService.SPScanJobUtilitySoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ScanjobUtilWebServiceUri)

			' Read data over webservice
			success = success AndAlso webservice.UpdateAssignedScanJobs(CustomerID, scanID, m_InitializationData.UserData.UserFullName)


			Return success

		End Function

		Private Function PerformSendingNotificationScanJobWebservice(ByVal notFoundedReportQuantity As Integer) As Boolean

			Dim success As Boolean = True


			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			success = success AndAlso webservice.SendReportNotificationsWithEMail(CustomerID, notFoundedReportQuantity)


			Return success

		End Function

#End Region


		Private Sub ValidateScancJobs()
			Dim success As Boolean = True
			If m_ScanData Is Nothing Then Return
			m_NotFoundedReport = 0


			Dim notCheckedScanData = (From b In m_ScanData Where b.FoundedCodeValue.Length > 5 And b.ImportedFileGuid.Length > 20 And Not b.CheckedOn.HasValue).ToList()

			If Not notCheckedScanData Is Nothing Then
				For Each itm In notCheckedScanData
					success = success AndAlso ValidateAssignedScanJob(itm)
				Next
			End If

			If m_NotFoundedReport > 0 Then
				PerformSendingNotificationScanJobWebservice(m_NotFoundedReport)
			End If

		End Sub

		Private Function ValidateAssignedScanJob(ByVal scanData As ScanViewData)
			Dim success As Boolean = True

			Select Case scanData.ModulNumber

				Case ScanViewData.ScannModulEnum.Employee
					success = success AndAlso ImportEmployeeScanJobData(scanData.ImportedFileGuid)

				Case ScanViewData.ScannModulEnum.Customer
					success = success AndAlso ImportCustomerScanJobData(scanData.ImportedFileGuid)

				Case ScanViewData.ScannModulEnum.Employment
					If scanData.DocumentCategoryNumber = 211 Then
						success = success AndAlso ImportEmploymentEmployeeScanJobData(scanData.ImportedFileGuid)
					ElseIf scanData.DocumentCategoryNumber = 107 Then
						success = success AndAlso ImportEmploymentCustomerScanJobData(scanData.ImportedFileGuid)
					End If

				Case ScanViewData.ScannModulEnum.Report
					m_CustomerReportData = New ScanViewData
					success = success AndAlso ImportCustomerReportScanJobData(scanData.ImportedFileGuid)
					If m_SavescantoBoth AndAlso Not m_CustomerReportData Is Nothing Then
						success = success AndAlso ImportDuplicatedEmployeeReportScanJobData(m_CustomerReportData)
					End If

				Case ScanViewData.ScannModulEnum.Invoice
					success = success AndAlso ImportInvoiceCustomerScanJobData(scanData.ImportedFileGuid)

				Case ScanViewData.ScannModulEnum.Payroll
					success = success AndAlso ImportPayrollEmployeeScanJobData(scanData.ImportedFileGuid)


				Case Else


			End Select

			Return success

		End Function


#Region "employee import"


		Private Function ImportEmployeeScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				success = success AndAlso m_ScanJobDatabaseAccess.AddEmployeeScanJobDocument(successData)
				If Not success Then
					Dim msg = m_Translate.GetSafeTranslationValue("Die Kandidaten-Dokumente konnten nicht gespeichert werden.")
					m_Logger.LogWarning(msg)

				End If
				m_Logger.LogDebug(String.Format("Kandidatendaten wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))

			Else
				m_Logger.LogDebug(String.Format("Kandidatendaten wurden nicht importiert!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				assigneddata.IsValid = False

			End If
			success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportEmploymentEmployeeScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber
			successData.IsValid = assigneddata.IsValid

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				success = success AndAlso m_ScanJobDatabaseAccess.AddEmploymentEmployeeScanJobDocument(successData)
				If Not success Then
					Dim msg = m_Translate.GetSafeTranslationValue("Die Kandidaten-Dokumente (Einsatzvertrag) konnten nicht gespeichert werden.")
					m_Logger.LogWarning(msg)

				End If
				m_Logger.LogDebug(String.Format("Kandidatendaten (Einsatzverträge) wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))

			Else
				m_Logger.LogDebug(String.Format("Kandidatendaten (Einsatzverträge) wurden nicht importiert!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				assigneddata.IsValid = False

			End If
			success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
			Dim wosResult As Boolean = success AndAlso m_SaveEmployeeEmploymentScanToWOS AndAlso SendAssignedEmploymentDocumentToWOS(assigneddata.ScanContent, assigneddata.RecordNumber)
			If wosResult Then
				m_Logger.LogInfo(String.Format("Dokument (Einsatzvertrag) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			Else
				m_Logger.LogInfo(String.Format("Dokument (Einsatzvertrag) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			End If

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportEmployeeReportScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)
			If assigneddata Is Nothing Then Return False
			If String.IsNullOrWhiteSpace(assigneddata.Customer_ID) Then Return True

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			successData.ReportFirstDay = assigneddata.ReportFirstDay
			successData.ReportLastDay = assigneddata.ReportLastDay
			successData.ReportMonth = assigneddata.ReportMonth
			successData.ReportWeek = assigneddata.ReportWeek
			successData.ReportYear = assigneddata.ReportYear

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				Dim assignedReportLinedata = m_ScanJobDatabaseAccess.LoadEmployeeReportLineDataForScanJobDocument(m_InitializationData.MDData.MDNr, m_SavescanwithZeroAmount, successData)

				If assignedReportLinedata.Count = 0 Then
					' report was not founded!
					success = success AndAlso m_ScanJobDatabaseAccess.AddReportScanDocumentIntoScanJobDb(successData)
					m_NotFoundedReport += 1

					success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
				Else

					For Each record In assignedReportLinedata
						success = success AndAlso m_ScanJobDatabaseAccess.AddReportScanJobDocument(successData, record)
						If Not success Then
							Dim msg = m_Translate.GetSafeTranslationValue("Die Kandidaten Rapport-Dokumente konnten nicht gespeichert werden.")
							m_Logger.LogWarning(msg)
						Else
							m_Logger.LogDebug(String.Format("Kandidaten Rapporte wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						End If

						success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
						Dim wosResult As Boolean = success AndAlso m_SaveEmployeeReportScanToWOS AndAlso SendAssignedEmployeeReportToWOS(assigneddata.ScanContent, assigneddata.ModulNumber, record.ReportLineNumber)
						If wosResult Then
							m_Logger.LogInfo(String.Format("Dokument (Rapport) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						Else
							m_Logger.LogInfo(String.Format("Dokument (Rapport) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						End If

					Next

				End If

			Else
				success = False
				assigneddata.IsValid = False

			End If

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportDuplicatedEmployeeReportScanJobData(ByVal scandata As ScanViewData) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = m_CustomerReportData
			If assigneddata Is Nothing Then Return False
			If String.IsNullOrWhiteSpace(assigneddata.Customer_ID) Then Return True

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			successData.ReportFirstDay = assigneddata.ReportFirstDay
			successData.ReportLastDay = assigneddata.ReportLastDay
			successData.ReportLineID = assigneddata.ReportLineID
			successData.ReportMonth = assigneddata.ReportMonth
			successData.ReportWeek = assigneddata.ReportWeek
			successData.ReportYear = assigneddata.ReportYear

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				Dim assignedReportLinedata = m_ScanJobDatabaseAccess.LoadEmployeeReportLineDataForScanJobDocument(m_InitializationData.MDData.MDNr, m_SavescanwithZeroAmount, successData)

				If assignedReportLinedata.Count > 0 Then
					For Each record In assignedReportLinedata
						success = success AndAlso m_ScanJobDatabaseAccess.AddReportScanJobDocument(successData, record)
						If Not success Then
							Dim msg = m_Translate.GetSafeTranslationValue("Die Kandidaten Rapport-Dokumente konnten nicht gespeichert werden.")
							m_Logger.LogWarning(msg)
						Else
							m_Logger.LogDebug(String.Format("Kandidaten Rapporte wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						End If

						Dim wosResult As Boolean = success AndAlso m_SaveEmployeeReportScanToWOS AndAlso SendAssignedEmployeeReportToWOS(assigneddata.ScanContent, assigneddata.RecordNumber, record.ReportLineNumber)
						If wosResult Then
							m_Logger.LogInfo(String.Format("Dokument (Rapport) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						Else
							m_Logger.LogInfo(String.Format("Dokument (Rapport) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						End If

					Next
				End If

			Else
				success = False
				assigneddata.IsValid = False

			End If

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportPayrollEmployeeScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				success = success AndAlso m_ScanJobDatabaseAccess.AddPayrollEmployeeScanJobDocument(successData)
				If Not success Then
					Dim msg = m_Translate.GetSafeTranslationValue("Die Kandidaten-Dokumente (Lohnabrechnung) konnten nicht gespeichert werden.")
					m_Logger.LogWarning(msg)

				End If
				m_Logger.LogDebug(String.Format("Kandidatendaten (Lohnabrechnungen) wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))

			Else
				m_Logger.LogDebug(String.Format("Kandidatendaten (Lohnabrechnungen) wurden nicht importiert!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				assigneddata.IsValid = False

			End If
			success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
			Dim wosResult As Boolean = success AndAlso m_SaveEmployeePayrollScanToWOS AndAlso SendAssignedPayrollDocumentToWOS(assigneddata.ScanContent, assigneddata.RecordNumber)
			If wosResult Then
				m_Logger.LogInfo(String.Format("Dokument (Lohnabrechnung) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			Else
				m_Logger.LogInfo(String.Format("Dokument (Lohnabrechnung) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			End If

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function


#Region "employee wos import"

		Private Function SendAssignedEmploymentDocumentToWOS(ByVal scanContent As Byte(), ByVal esNumber As Integer) As Boolean
			Dim result As Boolean = True
			Dim obj As New WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)

			Try
				If (scanContent Is Nothing) Then
					Dim strMsg As String = "Es wurden keine Daten zum Versand erstellt."

					Return False

				End If
				result = result AndAlso obj.TransferEmployeeEmploymentDataToWOS(esNumber, scanContent)


			Catch ex As Exception
				m_Logger.LogError(String.Format("wos transfere failied: {0}", ex.ToString))
				result = False

			End Try

			Return result
		End Function

		Private Function SendAssignedEmployeeReportToWOS(ByVal scanContent As Byte(), ByVal rpNumber As Integer, ByVal reportLineNumber As Integer?) As Boolean
			Dim result As Boolean = True
			Dim obj As New WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)

			Try
				If (scanContent Is Nothing) Then
					Dim strMsg As String = "Es wurden keine Daten zum Versand erstellt."

					Return False

				End If
				result = result AndAlso obj.TransferEmployeeReportDataToWOS(rpNumber, reportLineNumber, scanContent)


			Catch ex As Exception
				m_Logger.LogError(String.Format("wos transfere failied: {0}", ex.ToString))
				result = False

			End Try

			Return result
		End Function

		Private Function SendAssignedPayrollDocumentToWOS(ByVal scanContent As Byte(), ByVal loNumber As Integer) As Boolean
			Dim result As Boolean = True
			Dim obj As New WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)

			Try
				If (scanContent Is Nothing) Then
					Dim strMsg As String = "Es wurden keine Daten zum Versand erstellt."

					Return False

				End If
				result = result AndAlso obj.TransferEmployeePayrollDataToWOS(loNumber, scanContent)


			Catch ex As Exception
				m_Logger.LogError(String.Format("wos transfere failied: {0}", ex.ToString))
				result = False

			End Try

			Return result
		End Function

#End Region


#End Region



#Region "customer import"

		Private Function ImportCustomerScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				success = success AndAlso m_ScanJobDatabaseAccess.AddCustomerScanJobDocument(successData)

				If Not success Then
					Dim msg = m_Translate.GetSafeTranslationValue("Die Kunden-Dokumente konnten nicht gespeichert werden.")
					m_Logger.LogWarning(msg)
				Else
					m_Logger.LogDebug(String.Format("Kundendaten wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				End If

			Else
				m_Logger.LogDebug(String.Format("Kundendaten wurden nicht importiert (Scanfehler)!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				assigneddata.IsValid = False

			End If
			success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportEmploymentCustomerScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				success = success AndAlso m_ScanJobDatabaseAccess.AddEmploymentCustomerScanJobDocument(successData)

				If Not success Then
					Dim msg = m_Translate.GetSafeTranslationValue("Die Kunden-Dokumente (Verleihverträge) konnten nicht gespeichert werden.")
					m_Logger.LogWarning(msg)
				Else
					m_Logger.LogDebug(String.Format("Kundendaten (Verleihverträge) wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				End If

			Else
				m_Logger.LogDebug(String.Format("Kundendaten (Verleihverträge) wurden nicht importiert (Scanfehler)!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				assigneddata.IsValid = False

			End If
			success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
			Dim wosResult As Boolean = success AndAlso m_SaveCustomerEmploymentScanToWOS AndAlso SendAssignedCustomerEmploymentDocumentToWOS(assigneddata.ScanContent, assigneddata.RecordNumber)
			If wosResult Then
				m_Logger.LogInfo(String.Format("Dokument (Einsatzvertrag) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			Else
				m_Logger.LogInfo(String.Format("Dokument (Einsatzvertrag) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			End If


			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportCustomerReportScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)
			m_CustomerReportData = assigneddata

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			successData.ReportFirstDay = assigneddata.ReportFirstDay
			successData.ReportLastDay = assigneddata.ReportLastDay
			successData.ReportLineID = assigneddata.ReportLineID
			successData.ReportMonth = assigneddata.ReportMonth
			successData.ReportWeek = assigneddata.ReportWeek
			successData.ReportYear = assigneddata.ReportYear


			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				Dim assignedReportLinedata = m_ScanJobDatabaseAccess.LoadCustomerReportLineDataForScanJobDocument(m_InitializationData.MDData.MDNr, m_SavescanwithZeroAmount, successData)

				If assignedReportLinedata.Count = 0 Then
					' report was not founded!
					success = success AndAlso m_ScanJobDatabaseAccess.AddReportScanDocumentIntoScanJobDb(successData)
					m_NotFoundedReport += 1

					success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
				Else

					For Each record In assignedReportLinedata
						success = success AndAlso m_ScanJobDatabaseAccess.AddReportScanJobDocument(successData, record)
						If Not success Then
							Dim msg = m_Translate.GetSafeTranslationValue("Die Kunden Rapport-Dokumente konnten nicht gespeichert werden.")
							m_Logger.LogWarning(msg)
						Else
							m_Logger.LogDebug(String.Format("Kunden Rapporte wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						End If
						success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)

						Dim wosResult As Boolean = success AndAlso m_SaveCustomerReportScanToWOS AndAlso SendAssignedCustomerReportToWOS(assigneddata.ScanContent, assigneddata.RecordNumber, record.ReportLineNumber)
						If wosResult Then
							m_Logger.LogInfo(String.Format("Kundendokument (Rapport) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						Else
							m_Logger.LogInfo(String.Format("Kundendokument (Rapport) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
						End If

					Next

				End If

			Else
				success = False
				assigneddata.IsValid = False

			End If

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function

		Private Function ImportInvoiceCustomerScanJobData(ByVal scanID As String) As Boolean
			Dim success As Boolean = True

			Dim assigneddata = PerformAssignedScanJoblistWebservice(scanID)

			Dim successData = New ScanJobData
			successData.ID = assigneddata.ID
			successData.Customer_ID = assigneddata.Customer_ID
			successData.ScanContent = assigneddata.ScanContent
			successData.ImportedFileGuid = assigneddata.ImportedFileGuid
			successData.ModulNumber = assigneddata.ModulNumber
			successData.RecordNumber = assigneddata.RecordNumber
			successData.FoundedCodeValue = assigneddata.FoundedCodeValue
			successData.DocumentCategoryNumber = assigneddata.DocumentCategoryNumber

			If assigneddata.RecordNumber > 0 AndAlso Not assigneddata.ScanContent Is Nothing Then
				success = success AndAlso m_ScanJobDatabaseAccess.AddInvoiceCustomerScanJobDocument(successData)

				If Not success Then
					Dim msg = m_Translate.GetSafeTranslationValue("Die Kunden-Dokumente (Rechnungen) konnten nicht gespeichert werden.")
					m_Logger.LogWarning(msg)
				Else
					m_Logger.LogDebug(String.Format("Kundendaten (Rechnungen) wurden importiert: RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				End If

			Else
				m_Logger.LogDebug(String.Format("Kundendaten (Rechnungen) wurden nicht importiert (Scanfehler)!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
				assigneddata.IsValid = False

			End If
			success = success AndAlso PerformUpdateAssignedScanJobWebservice(scanID)
			Dim wosResult As Boolean = success AndAlso m_SaveCustomerInvoiceScanToWOS AndAlso SendAssignedCustomerInvoiceToWOS(assigneddata.ScanContent, assigneddata.RecordNumber)
			If wosResult Then
				m_Logger.LogInfo(String.Format("Dokument (Rechnung) wurde an WOS übermittelt. RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			Else
				m_Logger.LogInfo(String.Format("Dokument (Rechnung) konnte NICHT an WOS übermittelt werden!!! RecordNumber: {0} | FoundedCodeValue: {1}", assigneddata.RecordNumber, assigneddata.FoundedCodeValue))
			End If

			successData.IsValid = assigneddata.IsValid AndAlso success
			m_ImportedScanData.Add(successData)


			Return success

		End Function


#Region "customer wos export"

		Private Function SendAssignedCustomerEmploymentDocumentToWOS(ByVal scanContent As Byte(), ByVal esNumber As Integer) As Boolean
			Dim result As Boolean = True
			Dim obj As New WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)

			Try
				If (scanContent Is Nothing) Then
					Dim strMsg As String = "Es wurden keine Daten zum Versand erstellt."

					Return False

				End If
				result = result AndAlso obj.TransferCustomerEmploymentDataToWOS(esNumber, scanContent)


			Catch ex As Exception
				m_Logger.LogError(String.Format("wos transfere failied: {0}", ex.ToString))
				result = False

			End Try

			Return result
		End Function

		Private Function SendAssignedCustomerReportToWOS(ByVal scanContent As Byte(), ByVal rpNumber As Integer, ByVal reportLineNumber As Integer?) As Boolean
			Dim result As Boolean = True
			Dim obj As New WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)

			Try
				If (scanContent Is Nothing) Then
					Dim strMsg As String = "Es wurden keine Daten zum Versand erstellt."

					Return False

				End If
				result = result AndAlso obj.TransferCustomerReportDataToWOS(rpNumber, reportLineNumber, scanContent)


			Catch ex As Exception
				m_Logger.LogError(String.Format("wos transfere failied: {0}", ex.ToString))
				result = False

			End Try

			Return result
		End Function

		Private Function SendAssignedCustomerInvoiceToWOS(ByVal scanContent As Byte(), ByVal reNumber As Integer) As Boolean
			Dim result As Boolean = True
			Dim obj As New WOSDataTransfer.SendScanJobTOWOS(m_InitializationData)

			Try
				If (scanContent Is Nothing) Then
					Dim strMsg As String = "Es wurden keine Daten zum Versand erstellt."

					Return False

				End If
				result = result AndAlso obj.TransferCustomerInvoiceDataToWOS(reNumber, scanContent)


			Catch ex As Exception
				m_Logger.LogError(String.Format("wos transfere failied: {0}", ex.ToString))
				result = False

			End Try

			Return result
		End Function


#End Region

#End Region





#Region "Helpers"

		''' <summary>
		''' notificaton search view data (tbl_Notify).
		''' </summary>
		Private Class ScanViewData

			Public Property ID As Integer
			Public Property Customer_ID As String
			Public Property FoundedCodeValue As String
			Public Property ModulNumber As ScannModulEnum
			Public Property RecordNumber As Integer
			Public Property DocumentCategoryNumber As Integer?
			Public Property IsValid As Boolean?

			Public Property ReportYear As Integer?
			Public Property ReportMonth As Integer?
			Public Property ReportWeek As Integer?
			Public Property ReportFirstDay As Integer?
			Public Property ReportLastDay As Integer?
			Public Property ReportLineID As Integer?

			Public Property ScanContent As Byte()
			Public Property ImportedFileGuid As String
			Public Property CreatedOn As DateTime?
			Public Property CreatedFrom As String
			Public Property CheckedOn As DateTime?
			Public Property CheckedFrom As String

			Public Enum ScannModulEnum
				Employee
				Customer
				Employment
				Report
				Invoice
				Payroll
				NotDefined
			End Enum


			Public ReadOnly Property WhoCreated_FullData
				Get
					Return String.Format("{0}, {1}", CreatedOn, CreatedFrom)
				End Get
			End Property

			Public ReadOnly Property WhoChecked_FullData
				Get
					Return String.Format("{0}, {1}", CheckedOn, CheckedFrom)
				End Get
			End Property


		End Class


#End Region


	End Class

End Namespace
