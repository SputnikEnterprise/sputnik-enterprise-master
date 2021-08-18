
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Initialization
Imports System.Threading.Tasks
Imports SPProgUtility.ProgPath

Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.LanguagesAndProfessionsMng
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SP.Internal.Automations

Public Class ApplicantJobUtilities


#Region "Private Consts"

	Private Const DEFAULT_SALUTATION_CODE_MALE = "Herr"
	Private Const DEFAULT_SALUTATION_CODE_FEMALE = "Frau"

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
	Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

	Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
	Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx"

	Private Const MANDANT_XML_SETTING_SPUTNIK_IMPORT_SCANREPORT_BOTH As String = "MD_{0}/Sonstiges/importscanreporttoboth"
	Private Const MANDANT_XML_SETTING_SPUTNIK_IMPORT_SCANREPORT_ZEROAMOUNT As String = "MD_{0}/Sonstiges/importscanreportzeroamount"

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
	''' The application data access object.
	''' </summary>
	Private m_AppDatabaseAccess As IAppDatabaseAccess

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
	''' Utility functions.
	''' </summary>
	Private m_Utility As Infrastructure.Utility

	''' <summary>
	''' The path.
	''' </summary>
	Private m_path As ClsProgPath

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The mandant data.
	''' </summary>
	Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData

	''' <summary>
	''' The advisor data.
	''' </summary>
	Private m_SelectedAdvisorData As SP.DatabaseAccess.Common.DataObjects.AdvisorData

	''' <summary>
	''' The advisors.
	''' </summary>
	Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

	''' <summary>
	''' The countries.
	''' </summary>
	Private m_Countries As List(Of DatabaseAccess.Common.DataObjects.CountryData)

	''' <summary>
	''' The permission.
	''' </summary>
	Private m_Permission As List(Of DatabaseAccess.Common.DataObjects.PermissionData)

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
	''' Service Uri of Sputnik application util webservice.
	''' </summary>
	Private m_ApplicationUtilWebServiceUri As String

	''' <summary>
	''' new applicant number.
	''' </summary>
	Private m_currentApplicantNumber As Integer?

	Private m_ApplicationData As List(Of ApplicationViewData)
	Private m_ApplicantData As List(Of ApplicantData)
	Private m_ApplicationDocumentData As List(Of ApplicantDocumentViewData)

	Private m_ImportedApplicantData As BindingList(Of ApplicantData)

	Private m_NotFoundedReport As Integer
	Private m_CurrentApplicationNumber As Integer?
	Private m_VacancyNumber As Integer?


#End Region


#Region "public property"

	Public Property CustomerID As String

	Public ReadOnly Property ImportedApplicantData As BindingList(Of ApplicantData)
		Get
			Return m_ImportedApplicantData
		End Get
	End Property


#End Region


#Region "Constructor"

	Sub New(ByVal _setting As InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New Infrastructure.Utility
		m_path = New ClsProgPath

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_AppDatabaseAccess = New AppDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
		m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)

		m_SelectedMandantData = New SP.DatabaseAccess.Common.DataObjects.MandantData With {.MandantCanton = m_InitializationData.MDData.MDCanton,
																								.MandantDbConnection = m_InitializationData.MDData.MDDbConn,
																								.MandantGuid = m_InitializationData.MDData.MDGuid,
																								.MandantName1 = m_InitializationData.MDData.MDName,
																								.MandantName2 = m_InitializationData.MDData.MDName_2,
																								.MandantNumber = m_InitializationData.MDData.MDNr}

		m_Advisors = m_CommonDatabaseAccess.LoadAdvisorData()
		m_Countries = m_CommonDatabaseAccess.LoadCountryData()
		m_Permission = m_CommonDatabaseAccess.LoadPermissionData()

	End Sub

#End Region


#Region "Public Methodes"

	Public Function LoadNotCheckedApplicantJobData() As Boolean
		Dim success As Boolean = True
		m_ImportedApplicantData = Nothing

		SearchNotCheckedApplicantlistViaWebService()


		Return success

	End Function

	Public Function LoadNotCheckedApplicationJobData() As Boolean
		Dim success As Boolean = True
		m_ImportedApplicantData = Nothing

		SearchNotCheckedApplicationListViaWebService()


		Return success

	End Function


#End Region


#Region "private properties"

	Private ReadOnly Property ReadEmployeeOffsetFromSettings() As Integer
		Get
			Dim strQuery As String = "//StartNr/Mitarbeiter"
			Dim r = m_ClsProgSetting.GetUserProfileFile
			Dim employeeNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
			Dim intVal As Integer

			If Integer.TryParse(employeeNumberStartNumberSetting, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Get
	End Property


#End Region


#Region "load just applications"

	Private Sub SearchNotCheckedApplicationListViaWebService()
		Dim success As Boolean = True
		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		m_ImportedApplicantData = New BindingList(Of ApplicantData)
		m_ApplicationData = Nothing

		m_SuppressUIEvents = True
		Dim data = PerformNotDoneApplicationJobWebserviceCallAsync()
		m_ApplicationData = data.ToList
		If Not data Is Nothing AndAlso data.Count > 0 Then

			Dim notCheckedApplicationData = m_ApplicationData.ToList()

			If Not notCheckedApplicationData Is Nothing Then
				For Each itm In notCheckedApplicationData
					success = success AndAlso ImportApplicationJobData(itm.FK_ApplicantID)
				Next
			End If

			If m_NotFoundedReport > 0 Then
				'PerformSendingNotificationScanJobWebservice(m_NotFoundedReport)
			End If


		End If
		m_SuppressUIEvents = False

	End Sub


#End Region

#Region "Private Methodes"

	''' <summary>
	''' Search for application jobs over web service.
	''' </summary>
	Private Sub SearchNotCheckedApplicantlistViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		m_ImportedApplicantData = New BindingList(Of ApplicantData)
		m_ApplicationData = Nothing

		m_SuppressUIEvents = True
		Dim data = PerformApplicantJoblistWebserviceCallAsync()
		m_ApplicantData = data.ToList
		If Not data Is Nothing AndAlso data.Count > 0 Then ValidateData()
		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	'''  Performs application downloads from webserver.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformApplicantJoblistWebserviceCallAsync() As BindingList(Of ApplicantData)

		Dim listDataSource As BindingList(Of ApplicantData) = New BindingList(Of ApplicantData)

#If DEBUG Then
		'm_ApplicationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPApplication.asmx"
#End If

		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", CustomerID))
		Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", CustomerID))

		' Read data over webservice
		Dim searchResult = webservice.GetApplicantNotifications(CustomerID).ToList

		For Each result In searchResult

			Dim viewData = New ApplicantData With {
				.ID = result.ID,
				.Customer_ID = result.Customer_ID,
				.Auto = result.Auto,
				.Bicycle = result.Bicycle,
				.Birthdate = result.Birthdate,
				.CivilState = result.CivilState,
				.Country = result.Country,
				.DrivingLicence1 = result.DrivingLicence1,
				.DrivingLicence2 = result.DrivingLicence2,
				.DrivingLicence3 = result.DrivingLicence3,
				.EMail = result.EMail,
				.Firstname = result.Firstname,
				.Gender = result.Gender,
				.Language = result.Language,
				.LanguageLevel = result.LanguageLevel,
				.Lastname = result.Lastname,
				.Location = result.Location,
				.MobilePhone = result.MobilePhone,
				.Motorcycle = result.Motorcycle,
				.Nationality = result.Nationality,
				.Permission = result.Permission,
				.Postcode = result.Postcode,
				.PostOfficeBox = result.PostOfficeBox,
				.Profession = result.Profession,
				.Street = result.Street,
				.Telephone = result.Telephone,
				.CreatedOn = result.CreatedOn,
				.CreatedFrom = result.CreatedFrom,
				.ApplicantLifecycle = result.ApplicantLifecycle,
				.CVLProfileID = result.CVLProfileID
			}

			listDataSource.Add(viewData)
			m_Logger.LogDebug(String.Format("adding applicants into job queue >>> customerID. {0} | CVLProfileID: {1} | Fullname: {2}", CustomerID, viewData.CVLProfileID, viewData.ApplicantFullname))

		Next
		m_Logger.LogDebug(String.Format("number of applicants in job queue: {0}", listDataSource.Count))


		Return listDataSource

	End Function


	Private Function PerformNotDoneApplicationJobWebserviceCallAsync() As BindingList(Of ApplicationViewData)

		Dim listDataSource As BindingList(Of ApplicationViewData) = New BindingList(Of ApplicationViewData)


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		Dim searchResult = webservice.GetAssignedApplicationsForApplicant(CustomerID, 0).ToList

		For Each result In searchResult

			Dim viewData = New ApplicationViewData With {
				.Customer_ID = result.Customer_ID,
				.ID = result.ID,
				.FK_ApplicantID = result.FK_ApplicantID,
				.ApplicationLabel = result.ApplicationLabel,
				.Advisor = result.Advisor,
				.Availability = result.Availability,
				.BusinessBranch = result.BusinessBranch,
				.Comment = result.Comment,
				.Dismissalperiod = result.Dismissalperiod,
				.VacancyNumber = result.VacancyNumber,
				.CreatedOn = result.CreatedOn,
				.CreatedFrom = result.CreatedFrom,
				.CheckedOn = result.CheckedOn,
				.CheckedFrom = result.CheckedFrom,
				.ApplicationLifecycle = result.ApplicationLifecycle
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	'''  Performs application data for assigned applicant.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformAssignedApplicationJoblistWebservice(ByVal applicantID As Integer) As BindingList(Of ApplicationViewData)

		Dim listDataSource As BindingList(Of ApplicationViewData) = New BindingList(Of ApplicationViewData)

		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		Dim searchResult = webservice.GetAssignedApplicationsForApplicant(CustomerID, applicantID).ToList

		For Each result In searchResult

			Dim viewData = New ApplicationViewData With {
				.Customer_ID = result.Customer_ID,
				.ID = result.ID,
				.FK_ApplicantID = result.FK_ApplicantID,
				.ApplicationLabel = result.ApplicationLabel,
				.Advisor = result.Advisor,
				.Availability = result.Availability,
				.BusinessBranch = result.BusinessBranch,
				.Comment = result.Comment,
				.Dismissalperiod = result.Dismissalperiod,
				.VacancyNumber = result.VacancyNumber,
				.CreatedOn = result.CreatedOn,
				.CreatedFrom = result.CreatedFrom,
				.CheckedOn = result.CheckedOn,
				.CheckedFrom = result.CheckedFrom,
				.ApplicationLifecycle = result.ApplicationLifecycle
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	'''  Performs document data for assigned applicant.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformAssignedDocumentJoblistWebservice(ByVal applicantID As Integer) As BindingList(Of ApplicantDocumentViewData)

		Dim listDataSource As BindingList(Of ApplicantDocumentViewData) = New BindingList(Of ApplicantDocumentViewData)

		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		Try

			' Read data over webservice
			Dim searchResult = webservice.GetAssignedDocumentsForApplicant(CustomerID, applicantID).ToList

			For Each result In searchResult

				Dim viewData = New ApplicantDocumentViewData With {
					.ID = result.ID,
					.FK_ApplicantID = result.FK_ApplicantID,
					.Content = result.Content,
					.Type = result.Type,
					.FileExtension = result.FileExtension,
					.Flag = result.Flag,
					.Hashvalue = result.Hashvalue,
					.Title = result.Title,
					.CreatedOn = result.CreatedOn,
					.CreatedFrom = result.CreatedFrom,
					.CheckedOn = result.CheckedOn,
					.CheckedFrom = result.CheckedFrom
				}

				listDataSource.Add(viewData)

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


		Return listDataSource

	End Function

	''' <summary>
	'''  Performs applicant picture data for assigned applicant.
	''' </summary>
	Private Function PerformLoadAssignedApplicantPictureWebservice(ByVal cvlProfileID As Integer) As ApplicantDocumentViewData

		Dim listDataSource As ApplicantDocumentViewData = New ApplicantDocumentViewData

		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		Try

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedApplicantPictureFromCVLData(CustomerID, cvlProfileID)
			If Not searchResult Is Nothing AndAlso Not searchResult.ID Is Nothing Then
				listDataSource = New ApplicantDocumentViewData With {
						.ID = searchResult.ID,
						.Content = searchResult.DocBinary,
						.FileExtension = searchResult.FileType,
						.Hashvalue = searchResult.FileHashvalue
					}

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			listDataSource = Nothing

		End Try

		Return listDataSource

	End Function

	Private Function PerformLoadAssignedApplicationEMaliWebservice(ByVal applicationID As Integer) As EMailData

		Dim listDataSource As EMailData = New EMailData

		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		Try

			' Read data over webservice
			Dim searchResult = webservice.LoadApplicationEMailData(CustomerID, applicationID, False)
			If Not searchResult Is Nothing AndAlso searchResult.ApplicationID.GetValueOrDefault(0) > 0 Then
				listDataSource = New EMailData With {
						.ID = searchResult.ID,
						.Customer_ID = searchResult.Customer_ID,
						.EMailSubject = searchResult.EMailSubject,
						.EMailUidl = searchResult.EMailUidl,
						.EMailFrom = searchResult.EMailFrom,
						.EMailTo = searchResult.EMailTo,
						.HasHtmlBody = searchResult.HasHtmlBody,
						.EMailPlainTextBody = searchResult.EMailPlainTextBody,
						.EMailBody = searchResult.EMailBody,
						.CreatedOn = searchResult.CreatedOn,
						.CreatedFrom = searchResult.CreatedFrom,
						.EMailMime = searchResult.EMailMime,
						.EMailContent = searchResult.EMailContent
					}

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			listDataSource = Nothing

		End Try

		Return listDataSource

	End Function

	Private Function PerformUpdateAssignedApplicationJobWebservice(ByVal recordID As Integer, ByVal destApplicationNumber As Integer?, ByVal destApplicantNumber As Integer?) As Boolean

		Dim success As Boolean = True


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		success = success AndAlso webservice.UpdateAssignedApplicationAsChecked(CustomerID, recordID, destApplicationNumber, destApplicantNumber, True, m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName)


		Return success

	End Function

	Private Function PerformUpdateAssignedApplicantJobWebservice(ByVal recordID As Integer, ByVal destApplicantNumber As Integer?) As Boolean

		Dim success As Boolean = True


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		m_Logger.LogInfo(String.Format("CustomerID: {0} | destApplicantNumber: {1} will be checked as done.", CustomerID, destApplicantNumber))
		success = success AndAlso webservice.UpdateAssignedApplicantAsChecked(CustomerID, recordID, destApplicantNumber, True, m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName)


		Return success

	End Function

	Private Function PerformUpdateAssignedDocumentJobWebservice(ByVal recordID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer) As Boolean

		Dim success As Boolean = True


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		success = success AndAlso webservice.UpdateAssignedDocumentAsChecked(CustomerID, recordID, destDocumentID, destApplicationNumber, destApplicantNumber, True, m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName)


		Return success

	End Function

	Private Function PerformUpdateAllDataforAssignedApplicantWebservice(ByVal applicantID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer) As Boolean

		Dim success As Boolean = True


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		success = success AndAlso webservice.UpdateAllDataForAssignedApplicantAsChecked(CustomerID, applicantID, destDocumentID, destApplicationNumber, destApplicantNumber, True, m_InitializationData.UserData.UserGuid, m_InitializationData.UserData.UserFullName)


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


	Private Function ValidateData() As Boolean
		Dim success As Boolean = True
		If m_ApplicationData Is Nothing AndAlso m_ApplicantData Is Nothing Then Return False
		m_NotFoundedReport = 0

#If DEBUG Then
		'Return
#End If

		Dim notCheckedApplicantData = m_ApplicantData.ToList()

		If Not notCheckedApplicantData Is Nothing Then
			For Each itm In notCheckedApplicantData
				success = success AndAlso ValidateAssignedApplicantJob(itm)
			Next
		End If

		If m_NotFoundedReport > 0 Then
			'PerformSendingNotificationScanJobWebservice(m_NotFoundedReport)
		End If

	End Function

	Private Function ValidateAssignedApplicantJob(ByVal applicantData As ApplicantData)
		Dim success As Boolean = True

		success = success AndAlso ImportApplicantJobData(applicantData.ID)
		m_Logger.LogInfo(String.Format("CustomerID: {0} | Importing ImportApplicantJobData(applicantData.ID): ({1}) = {2} >>> is finished", CustomerID, applicantData.ID, success))

		success = PerformUpdateAssignedApplicantJobWebservice(applicantData.ID, m_currentApplicantNumber)
		m_Logger.LogInfo(String.Format("CustomerID: {0} | Applicant on remote done: {1} | m_currentApplicantNumber: {2} >>> is finished", CustomerID, success, m_currentApplicantNumber))

		success = True

		Return success

	End Function

	Private Function ImportApplicantJobData(ByVal applicantNumber As Integer) As Boolean
		Dim success As Boolean = True
		Dim createNewData As Boolean = True

		m_currentApplicantNumber = Nothing
		m_VacancyNumber = Nothing
		m_CurrentApplicationNumber = Nothing

		Dim applicantData = (From a In m_ApplicantData Where a.ID = applicantNumber And (a.Firstname <> String.Empty And a.Firstname <> String.Empty)).FirstOrDefault
		Dim mdnr = m_InitializationData.MDData.MDNr
		Dim xmlFile As String = m_MandantData.GetSelectedMDFormDataXMLFilename(mdnr)

		If applicantData Is Nothing Then
			' should be true because of webservice checkedOn update!!!
			m_Logger.LogError(String.Format("MDNr: {0} | Applicant: ({1}) is nothing!", mdnr, applicantNumber))

			Return True
		End If

		Try
			m_Logger.LogInfo(String.Format("MDNr: {0} | Applicant: ({1}) {2} | CVLProfileID: {3} >>> is verifying...", mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.CVLProfileID))
			If applicantData.CVLProfileID.GetValueOrDefault(0) = 0 Then
				Dim body As String = String.Format("CustomerID: ({1}) {2}{0}applicantData.ID: {3}{0}applicantData.ApplicantFullname: {4}{0}applicantData.CVLProfileID: {5}",
												   "<br>", CustomerID, mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.CVLProfileID)
				m_UtilityUI.SendMailNotification(String.Format("applicantData.CVLProfileID is {0}", applicantData.CVLProfileID), body, String.Empty, Nothing)
				m_Logger.LogWarning(String.Format("MDNr: {0} | Applicant: ({1}) {2} | CVLProfileID: {3} >>> exiting process!", mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.CVLProfileID))

				Return False
			End If
			Dim countryCode As String
			Dim country = (From a In m_Countries Where a.Code = applicantData.Country).FirstOrDefault
			If country Is Nothing Then
				countryCode = String.Empty
				m_Logger.LogWarning(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> country is empty! {3}", mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.Country))
			Else
				countryCode = country.Code
			End If

			Dim nationality = (From a In m_Countries Where a.Code = applicantData.Nationality).FirstOrDefault
			Dim nationalityCode As String
			If nationality Is Nothing Then
				nationalityCode = String.Empty
				m_Logger.LogWarning(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> nationality is empty! {3}", mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.Nationality))
			Else
				nationalityCode = nationality.Code
			End If

			Dim permissionCode As String
			Dim permission = (From a In m_Permission Where a.TranslatedPermission = applicantData.Permission).FirstOrDefault
			If permission Is Nothing Then
				permissionCode = Nothing
			Else
				permissionCode = permission.RecValue
			End If

			Dim currencyvalue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY)), "CHF")
			Dim mainlanguagevalue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/mainlanguagevalue", FORM_XML_MAIN_KEY)), "deutsch")

			Dim employeezahlart As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeezahlart", FORM_XML_MAIN_KEY)), "K")
			Dim employeebvgcode As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeebvgcode", FORM_XML_MAIN_KEY)), "9")
			Dim employeerahmenarbeitsvertrag As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeerahmenarbeitsvertrag", FORM_XML_MAIN_KEY)), True)
			Dim employeeferienback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeeferienback", FORM_XML_MAIN_KEY)), False)
			Dim employeefeiertagback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeefeiertagback", FORM_XML_MAIN_KEY)), False)
			Dim employee13lohnback As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employee13lohnback", FORM_XML_MAIN_KEY)), False)
			Dim employeenolo As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeenolo", FORM_XML_MAIN_KEY)), False)
			Dim employeenozg As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeenozg", FORM_XML_MAIN_KEY)), False)
			Dim emplyoeefstate As String = m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/emplyoeefstate", FORM_XML_MAIN_KEY))
			Dim employeesstate As String = m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeesstate", FORM_XML_MAIN_KEY))
			Dim employeecontact As String = m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeecontact", FORM_XML_MAIN_KEY))
			Dim employeesecsuvacode As String = m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/employeesecsuvacode", FORM_XML_MAIN_KEY))

			Dim searchlastAndFirstnameforDuplicateemployee As Boolean = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/searchlastandfirstnameforduplicateemployee", FORM_XML_MAIN_KEY)), False)

			Dim employeeNumberOffsetFromSettings As Integer = ReadEmployeeOffsetFromSettings()

			Dim sCanton As String = String.Empty
			Dim postCode As String = String.Empty
			Dim applicantWithCVLData As Boolean = True

			If countryCode.ToUpper <> "CH" Then
				sCanton = m_InitializationData.MDData.MDCanton
				postCode = applicantData.Postcode

			ElseIf countryCode.ToUpper = "CH" Then
				postCode = Format(Val(applicantData.Postcode), "0")
				applicantData.Postcode = postCode

				sCanton = m_CommonDatabaseAccess.LoadCantonByPostCode(postCode)
				If String.IsNullOrEmpty(sCanton) Then
					sCanton = m_InitializationData.MDData.MDCanton
				End If

			End If
			postCode = postCode.Substring(0, Math.Min(postCode.Length, 10))
			Dim civilState As String = applicantData.CivilState
			m_Logger.LogInfo(String.Format("civilState: {0}", civilState))

			If applicantData.CVLProfileID.GetValueOrDefault(0) > 0 Then
				Dim loadCVLData = PerformAssignedPersonalCommonWebservice(applicantData.CVLProfileID, Nothing)
				If Not loadCVLData OrElse m_PersonalCommonData.PersonalID.GetValueOrDefault(0) = 0 Then
					applicantWithCVLData = False
					m_Logger.LogWarning(String.Format("MDNr: {0} | Applicant: ({1}) {2} | CVLProfileID: {3} | PersonalID: {4} >>> is without cvl data!",
													  mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.CVLProfileID, m_PersonalCommonData.PersonalID.GetValueOrDefault(0)))

					'Return False
				End If

				If Not String.IsNullOrWhiteSpace(m_PersonalCommonData.CivilState) Then If Val(civilState) = 0 Then civilState = GetCivilstate()
			End If

			m_Logger.LogInfo(String.Format("MDNr: {0} | Applicant: ({1}) {2} | CVLProfileID: {3} >>> is ready for import...", mdnr, applicantData.ID, applicantData.ApplicantFullname, applicantData.CVLProfileID))

			Dim newEmployeeInitData As New NewEmployeeInitData With {
			.KST = Nothing,
			.Lastname = applicantData.Lastname,
			.Firstname = applicantData.Firstname,
			.Street = applicantData.Street,
			.CountryCode = countryCode,
			.Postcode = postCode,
			.Latitude = applicantData.Latitude,
			.Longitude = applicantData.Longitude,
			.Location = applicantData.Location,
			.Gender = If(applicantData.Gender = "f", "W", "M"),
			.Nationality = nationalityCode,
			.Civilstate = civilState,
			.Birthdate = applicantData.Birthdate,
			.Language = mainlanguagevalue,
			.DStellen = False,
			.NoES = False,
			.Stat1 = emplyoeefstate,
			.Stat2 = employeesstate,
			.Contact = employeecontact,
			.ProfessionCode = 0,
			.Profession = applicantData.Profession,
			.QLand = Nothing,
			.Permission = applicantData.Permission,
			.PermissionToDate = Nothing,
			.BirthPlace = String.Empty,
			.S_Canton = sCanton,
			.Residence = Nothing,
			.ANS_QST_Bis = Nothing,
			.Q_Steuer = "0",
			.ChurchTax = Nothing,
			.ValidatePermissionWithTax = True,
			.ChildsCount = 0,
			.QSTCommunity = Nothing,
			.RahmenCheck = False,
			.NoZG = employeenozg,
			.NoLO = employeenolo,
			.Currency = currencyvalue,
			.Zahlart = employeezahlart,
			.BVGCode = employeebvgcode,
			.SecSuvaCode = employeesecsuvacode,
			.FerienBack = employeeferienback,
			.FeiertagBack = employeefeiertagback,
			.L13Back = employee13lohnback,
			.ShowAsApplicant = True,
			.EmployeeNumberOffset = employeeNumberOffsetFromSettings,
			.MDNr = m_InitializationData.MDData.MDNr,
			.UserKST = m_InitializationData.UserData.UserKST,
			.CreatedFrom = m_InitializationData.UserData.UserFullName,
			.CreatedUserNumber = m_InitializationData.UserData.UserNr
		}


			m_Logger.LogInfo(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> is check for duplicate", mdnr, applicantData.ID, applicantData.ApplicantFullname))
			Dim existEmployee = ExistAssignedEmployeeData(applicantData, searchlastAndFirstnameforDuplicateemployee)
			If Not existEmployee Is Nothing AndAlso existEmployee.EmployeeNumber > 0 Then 'AndAlso existEmployee.MDNr = m_InitializationData.MDData.MDNr Then
				m_Logger.LogWarning(String.Format("employee exists allready and will be updated! existsEmployeenumber: {0} >>> applicantID: {1}", existEmployee.EmployeeNumber, applicantData.ID))
				m_currentApplicantNumber = existEmployee.EmployeeNumber

				success = success AndAlso UpdateExistingEmployeeWithApplicantData(applicantData, existEmployee.EmployeeNumber)
				createNewData = False

			Else

				m_Logger.LogInfo(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> will be added...", mdnr, applicantData.ID, applicantData.ApplicantFullname))
				success = success AndAlso m_EmployeeDatabaseAccess.AddNewEmployee(newEmployeeInitData)
				m_currentApplicantNumber = newEmployeeInitData.IdNewEmployee
				m_Logger.LogInfo(String.Format("(success: {4}) - MDNr: {0} | Applicant: ({1}) {2} >>> is saved with new number: {3} | CVLProfileID: {5}",
											   mdnr, applicantData.ID, applicantData.ApplicantFullname, m_currentApplicantNumber, success, applicantData.CVLProfileID))

				If Not success Then
					m_Logger.LogWarning(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> job is terminating...", mdnr, applicantData.ID, applicantData.ApplicantFullname))

					Return False
				End If


				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> processing language...", applicantData.ID, applicantData.ApplicantFullname))
				For Each itm In applicantData.Language.Split(New String() {",", ";", "#", "/"}, StringSplitOptions.None)
					Dim writtenLanguageDataToAssign = New EmployeeAssignedWrittenLanguageData With {.EmployeeNumber = m_currentApplicantNumber, .Description = itm}
					Dim result As Boolean = True
					If Not String.IsNullOrWhiteSpace(itm) Then
						result = result AndAlso m_EmployeeDatabaseAccess.AddEmployeeWrittenLaguageAssignment(writtenLanguageDataToAssign)
					End If
				Next
				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> processing profession...", applicantData.ID, applicantData.ApplicantFullname))
				For Each itm In applicantData.Profession.Split(New String() {",", ";", "#"}, StringSplitOptions.None)
					Dim professionToInsert = New EmployeeAssignedProfessionData With {.EmployeeNumber = m_currentApplicantNumber, .ProfessionCode = 0, .ProfessionText = itm}
					Dim result As Boolean = True
					If Not String.IsNullOrWhiteSpace(itm) Then
						result = result AndAlso m_EmployeeDatabaseAccess.AddEmployeeProfessionAssignment(professionToInsert)
					End If
				Next

				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> loading employee master data...", applicantData.ID, applicantData.ApplicantFullname))
				Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_currentApplicantNumber)
				Dim salutionData = m_CommonDatabaseAccess.LoadSalutationData()
				Dim letterSalutation As String = String.Empty

				Dim salution As String = DEFAULT_SALUTATION_CODE_MALE
				Select Case employeeData.Gender.ToUpper()
					Case "M"
						salution = DEFAULT_SALUTATION_CODE_MALE
					Case "W"
						salution = DEFAULT_SALUTATION_CODE_FEMALE
					Case Else
						' Do nothing

				End Select
				Dim assignedSalutionData = salutionData.Where(Function(data) data.Salutation = salution).FirstOrDefault()
				If Not assignedSalutionData Is Nothing Then
					letterSalutation = assignedSalutionData.LetterForm
				End If
				Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_currentApplicantNumber)
				employeeContactCommData.AnredeForm = salution
				employeeContactCommData.BriefAnrede = letterSalutation

				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> updating employee comm data...", applicantData.ID, applicantData.ApplicantFullname))
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeConactCommData(employeeContactCommData)

				employeeData.Telephone_P = applicantData.Telephone
				employeeData.MobilePhone = applicantData.MobilePhone
				employeeData.Email = applicantData.EMail
				employeeData.MA_Canton = sCanton
				employeeData.CVLProfileID = applicantData.CVLProfileID
				employeeData.ApplicantID = applicantData.ID
				employeeData.ApplicantLifecycle = applicantData.ApplicantLifecycle
				employeeData.ValidatePermissionWithTax = True

				If applicantData.CVLProfileID.GetValueOrDefault(0) > 0 AndAlso applicantWithCVLData Then
					Try

						m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> loading applicant picture...", applicantData.ID, applicantData.ApplicantFullname))
						Dim employeePicture = PerformLoadAssignedApplicantPictureWebservice(applicantData.CVLProfileID)
						If Not employeePicture Is Nothing AndAlso employeePicture.ID > 0 Then
							employeeData.MABild = employeePicture.Content
							Dim pictureSaved = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeePictureByteData(m_currentApplicantNumber, employeePicture.Content)
						End If

					Catch ex As Exception
						m_Logger.LogError(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> could not insert applicant picture!{3}", mdnr, applicantData.ID, applicantData.ApplicantFullname, ex.ToString))

					End Try
				End If
				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> updating employee master data...", applicantData.ID, applicantData.ApplicantFullname))
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(employeeData)
				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} | CVLProfileID: {2} >>> is saved: {3}.", applicantData.ID, applicantData.ApplicantFullname, employeeData.CVLProfileID, success))

				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> application will be added...", applicantData.ID, applicantData.ApplicantFullname))
				success = success AndAlso ImportApplicationJobData(applicantData.ID)

				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> mail attachments will be added...", applicantData.ID, applicantData.ApplicantFullname))
				success = success AndAlso ImportDocumentJobData(applicantData.ID)

				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> cvl documents will be added...", applicantData.ID, applicantData.ApplicantFullname))
				success = success AndAlso ImportCVLDocumentData(applicantData.CVLProfileID)


				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> contact data will be added...", applicantData.ID, applicantData.ApplicantFullname))
				Dim title As String = String.Empty
				Dim description As String = String.Empty
				Dim contactType As String = String.Empty

				title = String.Format("{0}", m_Translate.GetSafeTranslationValue("Bewerbung-Eingang"))
				contactType = "Einzelmail"
				description = String.Format("{0}{1}", title, If(Not applicantWithCVLData, " (OHNE Unterlagen!)", ""))
				If success AndAlso m_currentApplicantNumber.GetValueOrDefault(0) > 0 Then AddNewEmployeeContact(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)
				m_Logger.LogInfo(String.Format("Applicant: ({0}) {1} >>> saving contact data: {2}", applicantData.ID, applicantData.ApplicantFullname, success))

				applicantData.ApplicantNumber = m_currentApplicantNumber
				applicantData.CreatedOn = applicantData.CreatedOn

				If Not success Then
					Dim result As Boolean = True
					m_Logger.LogInfo(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> could not be successfully saved and will be deleted.", mdnr, applicantData.ID, applicantData.ApplicantFullname))

					result = result AndAlso m_EmployeeDatabaseAccess.DeleteEmployee(m_currentApplicantNumber, "MA", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)
				End If

				'End If

			End If

			m_Logger.LogInfo(String.Format("MDNr: {0} | Applicant: ({1}) {2} >>> process is finished...", mdnr, applicantData.ID, applicantData.ApplicantFullname))
			m_ImportedApplicantData.Add(applicantData)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1} >>> {2}", applicantData.ID, applicantData.ApplicantFullname, ex.ToString))
			Dim result As Boolean = True
			result = result AndAlso m_EmployeeDatabaseAccess.DeleteEmployee(m_currentApplicantNumber, "MA", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)
			success = False

		End Try


		Return success

	End Function

	Private Function GetCivilstate() As String
		Dim stateLbl As String = String.Empty

		Try
			stateLbl = m_PersonalCommonData.CivilState
			If String.IsNullOrWhiteSpace(stateLbl) Then Return stateLbl

			If stateLbl.Split(","c).Length > 0 Then
				stateLbl = stateLbl.Split(","c)(0)
			End If

			Select Case stateLbl.ToLower
				Case "p"
					stateLbl = "P"
				Case "s"
					stateLbl = "L"
				Case "d"
					stateLbl = "G"
				Case "w"
					stateLbl = "W"
				Case "m"
					stateLbl = "V"

				Case Else

			End Select

		Catch ex As Exception
			m_Logger.LogError(String.Format("GetCivilstate::PersonalID: {0} | CivilState: {1} >>> {2}", m_PersonalCommonData.PersonalID, m_PersonalCommonData.CivilState, ex.ToString))

		End Try

		Return stateLbl

	End Function

	Private Function ExistAssignedEmployeeData(ByVal applicantData As ApplicantData, ByVal searchlastAndFirstnameforDuplicateemployee As Boolean) As ExistingEmployeeSearchData
		Dim result = New ExistingEmployeeSearchData

		m_Logger.LogDebug(String.Format("searchlastAndFirstnameforDuplicateemployee: {0}", searchlastAndFirstnameforDuplicateemployee))

		If String.IsNullOrWhiteSpace(applicantData.EMail) OrElse (applicantData.Birthdate Is Nothing) Then
			m_Logger.LogDebug(String.Format("New Value for searchlastAndFirstnameforDuplicateemployee setting to TRUE: eMail: {0} | Birthdate: {1}", applicantData.EMail, applicantData.Birthdate))
			searchlastAndFirstnameforDuplicateemployee = True
		End If

		Dim querySetting As ExistingEmployeeSearchData
		If searchlastAndFirstnameforDuplicateemployee Then
			querySetting = New ExistingEmployeeSearchData With {.Lastname = applicantData.Lastname, .Firstname = applicantData.Firstname, .Email = applicantData.EMail, .Birthdate = applicantData.Birthdate}
		Else
			querySetting = New ExistingEmployeeSearchData With {.Email = applicantData.EMail, .Birthdate = applicantData.Birthdate}
		End If
		m_Logger.LogDebug(String.Format("querySetting: Lastname: {0} | Firstname: {1} | Email: {2} | Birthdate: {3}", querySetting.Lastname, querySetting.Firstname, querySetting.Email, querySetting.Birthdate))
		Dim existingEmployeeData As IEnumerable(Of ExistingEmployeeSearchData) = m_EmployeeDatabaseAccess.LoadAssignedEmployeesBySearchCriteria(querySetting)

		result = existingEmployeeData(0)

		Return result

	End Function

	Private Function UpdateExistingEmployeeWithApplicantData(ByVal applicantData As ApplicantData, ByVal existingEmployeeNumber As Integer) As Boolean
		Dim success As Boolean = True

		Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(existingEmployeeNumber)
		If employeeData Is Nothing Then Return False

		employeeData.CVLProfileID = applicantData.CVLProfileID
		employeeData.ApplicantID = applicantData.ID

		m_Logger.LogWarning(String.Format("employee was allready exists! Now updating employeemasterdata: existingEmployeeNumber: {0} | CVLProfileID: {1} >>> ID: {2}",
																			existingEmployeeNumber, applicantData.CVLProfileID, applicantData.ID))
		success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeMasterData(employeeData)

		m_Logger.LogWarning(String.Format("employee is saved successfull. now inserting application data. profileID: {0} >>> applicantID: {1}", applicantData.CVLProfileID, applicantData.ID))

		success = success AndAlso ImportApplicationJobData(applicantData.ID)
		success = success AndAlso ImportDocumentJobData(applicantData.ID)
		success = success AndAlso ImportCVLDocumentData(applicantData.CVLProfileID)

		applicantData.ApplicantNumber = existingEmployeeNumber

		Try
			' create new contact record
			Dim title As String = String.Empty
			Dim description As String = String.Empty
			Dim contactType As String = String.Empty

			title = String.Format("{0}", m_Translate.GetSafeTranslationValue("Bewerbung-Eingang"))
			contactType = "Einzelmail"
			description = title
			If success AndAlso existingEmployeeNumber > 0 Then AddNewEmployeeContact(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)

		Catch ex As Exception
			m_Logger.LogError(String.Format("contact could not be imsertetd! applicantData.ID: {0} >>> existingEmployeeNumber: {1} | {2}", applicantData.ID, existingEmployeeNumber, ex.ToString))
		End Try


		Return success

	End Function

	Private Sub AddNewEmployeeContact(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String)

		If m_CurrentApplicationNumber.GetValueOrDefault(0) = 0 Then Return
		Dim currentContactRecordNumber As Integer = 0
		Dim currentDocumentID As Integer = 0
		Dim contactData As EmployeeContactData = Nothing
		Dim fileContent = m_Utility.LoadFileBytes(attachedFile)
		Dim advisorFullname As String = "System"
		Dim extension As String = ".msg"

		If fileContent Is Nothing Then
			Dim mailData = PerformLoadAssignedApplicationEMaliWebservice(m_CurrentApplicationNumber.GetValueOrDefault(0))
			If Not mailData Is Nothing AndAlso Not mailData.EMailContent Is Nothing Then
				fileContent = mailData.EMailContent
				extension = ".eml"
			End If
		End If
		Dim dt = DateTime.Now
		contactData = New EmployeeContactData With {.EmployeeNumber = m_currentApplicantNumber, .CreatedOn = dt, .CreatedFrom = advisorFullname}

		contactData.EmployeeNumber = m_currentApplicantNumber
		contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
		contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
		contactData.ContactPeriodString = title
		contactData.ContactsString = description
		contactData.ContactImportant = False
		contactData.ContactFinished = False
		contactData.VacancyNumber = m_VacancyNumber
		contactData.ProposeNr = Nothing
		contactData.ESNr = Nothing
		contactData.CustomerNumber = Nothing

		contactData.ChangedFrom = advisorFullname
		contactData.ChangedOn = dt
		contactData.UsNr = 1

		Dim success As Boolean = True

		' Check if the document bytes must be saved.
		If Not (fileContent Is Nothing) And success Then

			Dim contactDocument As ContactDoc = Nothing

			contactDocument = New ContactDoc() With {.CreatedOn = dt,
																										 .CreatedFrom = advisorFullname,
																										 .FileBytes = fileContent,
																										 .FileExtension = extension}
			success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

			If success Then
				currentDocumentID = contactDocument.ID
				contactData.KontaktDocID = currentDocumentID
			End If

		End If

		' Insert contact
		contactData.CreatedUserNumber = 1
		success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

		If success Then
			currentContactRecordNumber = contactData.RecordNumber
		End If

		If Not success Then
			m_Logger.LogError("add new contact was not successfull!")
		End If

	End Sub

	Private Function ImportApplicationJobData(ByVal applicantID As Integer) As Boolean
		Dim success As Boolean = True

		m_Logger.LogDebug(String.Format("importing application data. applicantID: {0}", applicantID))

		Dim assigneddata = PerformAssignedApplicationJoblistWebservice(applicantID)
		Dim availableBusinessBranches = m_CommonDatabaseAccess.LoadBusinessBranchsData()

		For Each itm In assigneddata
			Dim successData = New ApplicationData
			successData.Customer_ID = itm.Customer_ID
			successData.ApplicationID = itm.ID
			successData.Availability = itm.Availability
			successData.CheckedFrom = itm.CheckedFrom
			successData.CheckedOn = itm.CheckedOn
			successData.Comment = itm.Comment
			successData.CreatedFrom = itm.CreatedFrom
			successData.CreatedOn = itm.CreatedOn
			successData.Dismissalperiod = itm.Dismissalperiod
			successData.VacancyNumber = itm.VacancyNumber
			successData.ApplicationLabel = itm.ApplicationLabel
			successData.ApplicationLifecycle = itm.ApplicationLifecycle
			successData.EmployeeID = m_currentApplicantNumber
			m_VacancyNumber = itm.VacancyNumber
			m_CurrentApplicationNumber = itm.ID

			If itm.CheckedOn Is Nothing Then
				If Not String.IsNullOrWhiteSpace(itm.BusinessBranch) Then
					Dim assingedBusinessBranch = availableBusinessBranches.Where(Function(data) data.Name.ToLower().Trim() = itm.BusinessBranch.ToLower().Trim()).FirstOrDefault()
					If Not assingedBusinessBranch Is Nothing Then
						successData.BusinessBranch = assingedBusinessBranch.Name
					End If

				End If
				If Not String.IsNullOrWhiteSpace(itm.Advisor) Then
					Dim advisor = (From a In m_Advisors Where a.UserGuid = itm.Advisor).FirstOrDefault
					If advisor Is Nothing Then
						advisor = New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = m_InitializationData.UserData.UserKST}
					End If
					successData.Advisor = advisor.UserGuid
				End If

				success = success AndAlso m_AppDatabaseAccess.AddApplicationToClientDb(successData)
				If Not success Then
					Dim msg = String.Format("application could not be saved. RecordID: {0} >>> {1}", itm.ID, itm.ApplicationLabel)
					m_Logger.LogWarning(msg)
				Else
					m_Logger.LogDebug(String.Format("application saved successfull. ApplicationID: {0} >>> {1}", itm.ID, itm.ApplicationLabel))
				End If

				success = success AndAlso PerformUpdateAssignedApplicationJobWebservice(itm.ID, successData.ID, m_currentApplicantNumber)

			End If

		Next


		Return success

	End Function

	Private Function ImportDocumentJobData(ByVal applicantID As Integer) As Boolean
		Dim success As Boolean = True

		Try
			m_Logger.LogDebug(String.Format("importing tbl_applicant_Document job data: ApplicationID: {0}", applicantID))

			Dim assigneddata = PerformAssignedDocumentJoblistWebservice(applicantID)

			For Each itm In assigneddata
				Dim originalRecordID As Integer = itm.ID
				Dim successData = New ApplicantDocumentData
				successData.ID = itm.ID
				successData.DocClass = itm.DocClass
				successData.FK_ApplicantID = m_currentApplicantNumber
				successData.Title = itm.Title
				successData.Content = itm.Content
				successData.Flag = itm.Flag
				successData.FileExtension = itm.FileExtension
				successData.Type = itm.Type
				successData.HashValue = itm.Hashvalue
				successData.Category = 0

				Dim docLabel As String = successData.Title

				If Not String.IsNullOrWhiteSpace(docLabel) Then

					If docLabel.ToLower.Contains("lebenslauf") OrElse docLabel.ToLower.Contains("cv ") OrElse docLabel.ToLower.Contains(" cv ") OrElse docLabel.ToLower.Contains("cv-") OrElse docLabel.ToLower.Contains("cv_") OrElse docLabel.ToLower = "cv" OrElse docLabel.ToLower.Contains("curriculum") Then
						docLabel = m_Translate.GetSafeTranslationValue("Lebenslauf")
						successData.Category = 201

					ElseIf docLabel.ToLower.Contains("recommendation") Then
						docLabel = m_Translate.GetSafeTranslationValue("Zeugnis")
						successData.Category = 202

					ElseIf docLabel.ToLower.Contains("schoolreport") Then
						docLabel = m_Translate.GetSafeTranslationValue("Schulzeugnis")
						successData.Category = 202

					ElseIf docLabel.ToLower.Contains("application") Then
						docLabel = m_Translate.GetSafeTranslationValue("Bewerbungsunterlage")
						successData.Category = 202

					ElseIf docLabel.ToLower.Contains("unclassified") Then
						docLabel = m_Translate.GetSafeTranslationValue("Nicht definierte Unterlage")
						successData.Category = 0

					ElseIf docLabel.ToLower.Contains("certificate") Then
						docLabel = "Zertifikat"
						successData.Category = 251

					Else
						successData.Category = 0
					End If

				Else
					docLabel = m_Translate.GetSafeTranslationValue("Nicht definierte Unterlage")
					successData.Category = 0

				End If

				successData.Title = docLabel
				success = success AndAlso m_AppDatabaseAccess.AddApplicantDocumentToEmployee(successData)
				If Not success Then
					Dim msg = String.Format("tbl_applicant_Document could NOT be saved. RecordID: {0}", originalRecordID)
					m_Logger.LogWarning(msg)
				Else
					m_Logger.LogDebug(String.Format("tbl_applicant_Document is saved successfull. RecordID: {0}", originalRecordID))
				End If
				' TODO
				'success = success AndAlso PerformUpdateAssignedDocumentJobWebservice(originalRecordID, successData.ID, successData.ApplicationNumber, m_currentApplicantNumber)

				'End If

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		End Try


		Return success

	End Function

	Private Function ImportCVLDocumentData(ByVal cvlProfileID As Integer) As Boolean
		Dim success As Boolean = True

		Try
			m_Logger.LogDebug(String.Format("importing cvl Document job data. cvlProfileID: {0}", cvlProfileID))

			Dim assigneddata = PerformCVLDocumentWebservice(cvlProfileID)

			For Each itm In assigneddata
				Dim originalRecordID As Integer = itm.ID
				Dim successData = New ApplicantDocumentData
				successData.ID = itm.ID
				successData.FK_ApplicantID = m_currentApplicantNumber
				successData.DocClass = itm.DocClass
				successData.Title = itm.Title
				successData.Content = itm.Content
				successData.FileExtension = itm.FileExtension
				successData.HashValue = itm.Hashvalue
				successData.DocXML = itm.DocXML
				successData.PlainText = itm.PlainText
				successData.Pages = itm.Pages
				successData.FileSize = itm.FileSize

				If Not String.IsNullOrWhiteSpace(successData.DocClass) Then
					If successData.DocClass.ToLower.Contains("cv") OrElse successData.DocClass.ToLower = "cv" Then
						successData.Title = m_Translate.GetSafeTranslationValue("Lebenslauf")
						successData.Category = 201

					ElseIf successData.DocClass.ToLower.Contains("recommendation") Then
						successData.Title = m_Translate.GetSafeTranslationValue("Zeugnis")
						successData.Category = 202

					ElseIf successData.DocClass.ToLower.Contains("schoolreport") Then
						successData.Title = m_Translate.GetSafeTranslationValue("Schulzeugnis")
						successData.Category = 202

					ElseIf successData.DocClass.ToLower.Contains("application") Then
						successData.Title = m_Translate.GetSafeTranslationValue("Bewerbungsunterlage")
						successData.Category = 202

					ElseIf successData.DocClass.ToLower.Contains("unclassified") Then
						successData.Title = m_Translate.GetSafeTranslationValue("Nicht definierte Unterlage")
						successData.Category = 0

					ElseIf successData.DocClass.ToLower.Contains("certificate") Then
						successData.Title = "Zertifikat"
						successData.Category = 251

					Else
						successData.Title = m_Translate.GetSafeTranslationValue("Nicht definierte Unterlage")
						successData.Category = 0

					End If

				End If


				success = success AndAlso m_AppDatabaseAccess.AddApplicantDocumentToEmployee(successData)
				If Not success Then
					Dim msg = String.Format("cvl document could NOT be saved. RecordID: {0}", originalRecordID)
					m_Logger.LogWarning(msg)
				Else
					m_Logger.LogDebug(String.Format("cvl document is saved successfull. RecordID: {0}", originalRecordID))
				End If
				' TODO
				'success = success AndAlso PerformUpdateAssignedDocumentJobWebservice(originalRecordID, successData.ID, successData.ApplicationNumber, m_currentApplicantNumber)

				'End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("customerID: {0} >>> cvlProfileID: {1} | {2}", cvlProfileID, m_InitializationData.MDData.MDGuid, ex.ToString))
			Return True

		End Try


		Return success

	End Function


#Region "helpers"

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


#Region "Helper class"

	''' <summary>
	''' notificaton search view data (tbl_Notify).
	''' </summary>
	Private Class ApplicationViewData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property FK_ApplicantID As Integer?
		Public Property VacancyNumber As Integer?
		Public Property ApplicationLabel As String
		Public Property BusinessBranch As String
		Public Property Advisor As String
		Public Property Dismissalperiod As String
		Public Property Availability As String
		Public Property Comment As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property ApplicationLifecycle As Integer?

	End Class


	Private Class ApplicantDocumentViewData

		Public Property ID As Integer
		Public Property FK_ApplicantID As Integer?
		Public Property DocClass As String
		Public Property Type As Integer?
		Public Property Flag As Integer?
		Public Property Title As String
		Public Property FileExtension As String
		Public Property Content As Byte()
		Public Property Pages As Integer?
		Public Property FileSize As Integer?
		Public Property Hashvalue As String
		Public Property PlainText As String
		Public Property DocXML As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

	End Class


#End Region


End Class
