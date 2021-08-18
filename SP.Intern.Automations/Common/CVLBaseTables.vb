
Imports System.ComponentModel
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Internal.Automations.SPApplicationWebService
Imports SP.Internal.Automations.SPNotificationWebService


Namespace BaseTable

	Public Class SPSBaseTables


#Region "private consts"

		Private Const DEFAULT_SPUTNIK_TAX_PERMISSION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPEmployeeTaxInfoService.asmx"
		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx"
		Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_Services/SPApplication.asmx"
		Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wssps_services/spbankutil.asmx"
		Private Const DEFAULT_SPUTNIK_VACANCY_UTIL_WEBSERVICE_URI As String = "wssps_services/SPInternVacancies.asmx"
		Private Const DEFAULT_SPUTNIK_STAGING_VACANCY_UTIL_WEBSERVICE_URI As String = "wssps_services/SPInternVacancies.asmx"

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

		Private m_TaxPermissionUtilWebServiceUri As String
		Private m_NotificationUtilWebServiceUri As String
		Private m_ApplicationUtilWebServiceUri As String
		Private m_BankUtilWebServiceUri As String
		Private m_VacancyUtilWebServiceUri As String
		Private m_Staging_VacancyUtilWebServiceUri As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_Utility As Utility

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
		Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Protected m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Protected m_ListingDatabaseAccess As IListingDatabaseAccess

		Private m_AdditionalInfoData As AdditionalInfoLocalViewData
		Private m_LanguageData As BindingList(Of CodeNameViewData)
		Private m_CVLProfileID As Integer?
		Private m_WorkID As Integer?
		Private m_PersonalID As Integer?
		Private m_EducationID As Integer?
		Private m_AdditionalID As Integer?
		Private m_ObjectiveID As Integer?

		Private m_fileGuid As String
		Private m_ResultContent As String
		Private m_customerID As String


#End Region


#Region "Public properties"

		Public Property BaseTableName As String

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

			Dim domainName As String = m_InitializationData.MDData.WebserviceDomain

			m_TaxPermissionUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_TAX_PERMISSION_UTIL_WEBSERVICE_URI)
			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)
			m_BankUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)
			m_VacancyUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_UTIL_WEBSERVICE_URI)
			m_Staging_VacancyUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_STAGING_VACANCY_UTIL_WEBSERVICE_URI)

			m_fileGuid = Guid.NewGuid.ToString

			m_customerID = m_InitializationData.MDData.MDGuid

		End Sub


#End Region

		Public Function UpdateCustomerGeoData(ByVal countryCode As String) As Boolean
			Dim success As Boolean = True

			Dim listDataSource As BindingList(Of LocationGoordinateViewData) = New BindingList(Of LocationGoordinateViewData)

			listDataSource = PerformGeoCoordinationDatalistWebserviceCall(countryCode)
			If listDataSource Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Geo-Koordinaten konnten nicht geladen werden."))

				Return False
			End If
			BaseTableName = "Country"
			Dim countryData = PerformCVLBaseTablelistWebserviceCall()
			If countryData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Länderdaten konnten nicht geladen werden."))

				Return False
			End If


			Dim customerData = m_ListingDatabaseAccess.LoadAllCustomerMasterData()
			If customerData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Mandant: {0} >>> Kundendaten konnten nicht geladen werden.", m_InitializationData.MDData.MDNr))

				Return False
			End If
			customerData = customerData.Where(Function(x) x.CountryCode = countryCode).ToList()

			For Each customer In customerData
				Dim country = countryData.Where(Function(x) x.Code = customer.CountryCode).FirstOrDefault()
				If Not country Is Nothing Then
					Dim geoData = listDataSource.Where(Function(x) x.Postcode = Trim(customer.Postcode) And x.CountryCode = customer.CountryCode).FirstOrDefault()

					If Not geoData Is Nothing Then

						If customer.Latitude.GetValueOrDefault(0) = 0 OrElse customer.Longitude.GetValueOrDefault(0) = 0 Then
							customer.Latitude = geoData.Latitude
							customer.Longitude = geoData.Longitude

							success = success AndAlso m_CustomerDatabaseAccess.UpdateCustomerGeoData(customer)
						End If
					Else
						m_Logger.LogWarning(String.Format("geo data could not be founded! {0}: '{1}' >>> '{2}'", customer.CustomerNumber, customer.Postcode, customer.CountryCode))

						Trace.WriteLine(String.Format("geo data could not be founded! {0}: {2}-{1}", customer.CustomerNumber, customer.Postcode, customer.CountryCode))
					End If

					If Not success Then Exit For
				End If
			Next

			Return success

		End Function


		Public Function UpdateCustomerCountryData() As Boolean
			Dim success As Boolean = True

			BaseTableName = "Country"
			Dim countryData = PerformCVLBaseTablelistWebserviceCall()
			Dim existingCountryData = m_CommonDatabaseAccess.LoadCountryData()
			If countryData Is Nothing OrElse existingCountryData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Länderdaten konnten nicht geladen werden."))

				Return False
			End If

			Dim customerData = m_ListingDatabaseAccess.LoadAllCustomerCountryCodeData()
			If customerData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Mandant: {0} >>> Kundendaten konnten nicht geladen werden.", m_InitializationData.MDData.MDNr))

				Return False
			End If

			For Each customer In customerData
				Dim country = countryData.Where(Function(x) x.Code = customer.CountryCode).FirstOrDefault()
				If country Is Nothing Then
					Dim oldcountry = existingCountryData.Where(Function(x) x.Code = customer.CountryCode).FirstOrDefault()

					If Not oldcountry Is Nothing Then
						country = countryData.Where(Function(x) x.Translated_Value = oldcountry.Name).FirstOrDefault()
						If Not country Is Nothing Then
							success = success AndAlso m_ListingDatabaseAccess.UpdateCustomerCountryData(customer.CountryCode, country.Code)
						End If
					End If

				End If

				If Not success Then Exit For
			Next


			Return success

		End Function

		Public Function PerformCVLBaseTablelistWebserviceCall() As BindingList(Of CVLBaseTableViewData)

			Dim listDataSource As BindingList(Of CVLBaseTableViewData) = New BindingList(Of CVLBaseTableViewData)

#If DEBUG Then
			'm_NotificationUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
#End If

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			Try
				' Read data over webservice
				Dim searchDataResult = webservice.GetCVLBaseData(m_InitializationData.MDData.MDGuid, BaseTableName, m_InitializationData.UserData.UserLanguage) '.ToList
				If searchDataResult Is Nothing Then
					m_Logger.LogWarning(String.Format("table base data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Dim existingCountryData = m_CommonDatabaseAccess.LoadCountryData()
					For Each searchResult In existingCountryData
						Dim viewData = New CVLBaseTableViewData With {
						.ID = searchResult.ID,
						.Code = searchResult.Code,
						.Translated_Value = searchResult.Name
					}

						listDataSource.Add(viewData)

					Next

					Return listDataSource
				End If

				For Each result In searchDataResult

					Dim viewData = New CVLBaseTableViewData With {
						.ID = result.ID,
						.Code = result.Code,
						.Translated_Value = result.Translated_Value
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0} >>> {1}", m_NotificationUtilWebServiceUri, ex.ToString))

				listDataSource = Nothing

				Return Nothing
			End Try

			Return listDataSource

		End Function

		Public Function PerformGeoDataWebservice(ByVal countryCode As String, ByVal postCode As String) As GeoCoordinateDataViewData
			Dim result As New GeoCoordinateDataViewData

			If String.IsNullOrWhiteSpace(postCode) Then Return result

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPApplication.asmx"
#End If

			Dim ws = New SPApplicationWebService.SPApplicationSoapClient
			ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = ws.LoadGeoCoordinationPostcodeData(m_InitializationData.MDData.MDGuid, countryCode, postCode)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("geo data could not be loaded from webservice! {0} | {1}", m_InitializationData.MDData.MDGuid, postCode))

				Return Nothing
			End If

			result.ID = searchResult.ID
			result.AdminCode1 = searchResult.AdminCode1
			result.AdminName1 = searchResult.AdminName1
			result.AdminCode2 = searchResult.AdminCode2
			result.AdminName2 = searchResult.AdminName2
			result.AdminCode3 = searchResult.AdminCode3
			result.AdminName3 = searchResult.AdminName3
			result.Latitude = searchResult.Latitude
			result.Longitude = searchResult.Longitude
			result.PlaceName = searchResult.PlaceName
			result.Postcode = searchResult.Postcode
			result.CountryCode = searchResult.CountryCode
			result.Accuracy = searchResult.Accuracy


			Return result

		End Function


		Public Function PerformQualificationDataWebservice(ByVal gender As String, ByVal language As String, ByVal qualificationModul As String) As IEnumerable(Of QualificationData)
			Dim listDataSource As BindingList(Of QualificationData) = New BindingList(Of QualificationData)

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPApplication.asmx"
#End If

			Dim ws = New SPNotificationWebService.SPNotificationSoapClient
			ws.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = ws.LoadQualificationData(m_InitializationData.MDData.MDGuid, gender, language, qualificationModul)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("qualification data could not be loaded from webservice! {0} | {1} >>> {2}", m_InitializationData.MDData.MDGuid, gender, language))

				Return Nothing
			End If

			For Each itm In searchResult

				Dim viewData = New QualificationData With {
						.Code = itm.Code,
						.MP = itm.MeldePflichtig,
						.TranslatedValue = itm.TranslatedValue
					}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function


#Region "Bank data"

		Public Function PerformBankDataOverWebService(ByVal clearingNumber As String, ByVal bankName As String, ByVal bankPostcode As String, ByVal bankLocation As String, ByVal swift As String) As BindingList(Of BankstammViewData)
			Dim listDataSource As BindingList(Of BankstammViewData) = New BindingList(Of BankstammViewData)

			If String.IsNullOrWhiteSpace(clearingNumber) Then Return listDataSource

			Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

			' Read data over webservice
			Try
				clearingNumber = RemoveLeadingZeros(clearingNumber)
				Dim searchResult = webservice.LoadBankData(m_InitializationData.MDData.MDGuid, clearingNumber, bankName, bankPostcode, bankLocation, swift)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("bank data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New BankstammViewData With {
					  .BankName = result.BankName,
					  .ClearingNumber = result.ClearingNumber,
					  .Location = result.Location,
					  .PostAccount = result.PostAccount,
					  .Postcode = result.Postcode,
					  .Swift = result.Swift,
					  .Telefax = result.Telefax,
					  .Telephone = result.Telephone
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource

		End Function

		Public Function PerformAssignedBankDataOverWebService(ByVal clearingNumber As String, ByVal bankName As String, ByVal bankLocation As String) As BankstammViewData
			Dim result As New BankstammViewData

			If String.IsNullOrWhiteSpace(clearingNumber) Then Return result

			Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

			' Read data over webservice
			Try
				clearingNumber = RemoveLeadingZeros(clearingNumber)
				Dim searchResult = webservice.LoadAssignedBankData(m_InitializationData.MDData.MDGuid, clearingNumber, bankName, bankLocation)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("bank data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				If Not searchResult Is Nothing Then

					result.BankName = searchResult.BankName
					result.ClearingNumber = searchResult.ClearingNumber
					result.Location = searchResult.Location
					result.PostAccount = searchResult.PostAccount
					result.Postcode = searchResult.Postcode
					result.Swift = searchResult.Swift
					result.Telefax = searchResult.Telefax
					result.Telephone = searchResult.Telephone

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return result

		End Function

#End Region


#Region "iban data"

		Public Function PerformEncodingIBANOverWebService(ByVal clearingNumber As String, ByVal accountNummber As String) As IBANConvertResultViewData
			Dim result As New IBANConvertResultViewData

			Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

			Try
				Dim searchResult = webservice.EncodeSwissIBAN(m_InitializationData.MDData.MDGuid, clearingNumber, accountNummber)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("IBAN konnte nicht über Webservice-Schnittstelle ermittelt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("iban data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				result.IBAN = searchResult.IBAN
				result.PC = searchResult.PC
				result.ResultCode = searchResult.ResultCode
				result.Success = searchResult.Success


			Catch ex As Exception
				Dim msg As String = String.Format("MDGuid: {0} | m_BankUtilWebServiceUri: {1} | clearingNumber: {2} | accountNummber: {3}", m_BankUtilWebServiceUri, m_InitializationData.MDData.MDGuid, clearingNumber, accountNummber)
				m_Logger.LogError(String.Format("{1}{0}{2}", vbNewLine, msg, ex.ToString))
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("IBAN konnte nicht über Webservice-Schnittstelle ermittelt werden."))
			End Try

			Return result
		End Function

		Public Function PerformDecodingIBANOverWebService(ByVal iban As String) As IBANDecodeResultViewData
			Dim result As New IBANDecodeResultViewData

			Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

			Try
				Dim searchResult = webservice.DecodeIBAN(m_InitializationData.MDData.MDGuid, iban)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("IBAN konnte nicht über Webservice-Schnittstelle in Clearing/Kontonummer zerlegt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("iban data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				result.BankID = searchResult.BankID
				result.Kontonummer = searchResult.Kontonummer
				result.Landcode = searchResult.Landcode
				result.ResultCode = searchResult.ResultCode


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("IBAN konnte nicht über Webservice-Schnittstelle in Clearing/Kontonummer zerlegt werden."))
			End Try

			Return result

		End Function

		Public Function LoadIBANLibraryVersionOverWebService() As IBANVersionResultViewData
			Dim result As New IBANVersionResultViewData

			Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

			Try
				Dim searchResult = webservice.IBANEncodeDLLVersionInfo(m_InitializationData.MDData.MDGuid)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("IBAN-Version konnte nicht geladen werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("iban version data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				result.MajorVersion = searchResult.MajorVersion
				result.MinorVersion = searchResult.MinorVersion
				result.ValidUntil = searchResult.ValidUntil


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("IBAN-Version konnte nicht geladen werden."))
			End Try

			Return result

		End Function

#End Region


#Region "vacancies"

		Function GetJobCHOccupationList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHBerufeData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Beruf Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch occupation data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruf Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHOccupationExperienceList(ByVal parentID As Integer?, ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHFachbereichData(m_InitializationData.MDData.MDGuid, parentID.GetValueOrDefault(0), m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Beruferfahrung Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch occupation experience data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beruferfahrung Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHPositionList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHPositionData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Position Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch position data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Position Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHBildungsniveauList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHBildungNiveauData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Bildungsniveau Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch bildungsniveau data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bildungsniveau Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHBrunchesList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHBranchenData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Branchen Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch brunches data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Branchen Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHLanguageNameList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHLanguageData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Sprachdaten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch language name data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprachdaten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHLanguageNiveauList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHLanguageNiveauData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Sprachniveau Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch language niveau data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprachniveau Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetJobCHRegionList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadJobCHRegionData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Region Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("jobs.ch region data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID_Parent = result.ID_Parent,
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Region Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function

		Function GetAVAMEducationList(ByVal language As String) As BindingList(Of VacancyJobCHPeripheryViewData)
			Dim listDataSource As BindingList(Of VacancyJobCHPeripheryViewData) = New BindingList(Of VacancyJobCHPeripheryViewData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_Staging_VacancyUtilWebServiceUri)
			'Dim webservice As New InternStagingVacancyService.SPInternVacanciesSoapClient
			'webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_Staging_VacancyUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadAVAMEducationData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("(AVAM) Bildung Daten konnten nicht über Webservice abgefragt werden.")
					m_UtilityUI.ShowErrorDialog(msg)
					m_Logger.LogWarning(String.Format("AVAM education data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New VacancyJobCHPeripheryViewData With {
					  .ID = result.ID,
					  .RecNr = result.RecNr,
					  .TranslatedLabel = result.TranslatedLabel
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bildungsniveau Daten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return listDataSource
		End Function



#End Region


#Region "Coordinations"

		Private Function PerformGeoCoordinationDatalistWebserviceCall(ByVal countryCode As String) As BindingList(Of LocationGoordinateViewData)

			Dim listDataSource As BindingList(Of LocationGoordinateViewData) = New BindingList(Of LocationGoordinateViewData)

#If DEBUG Then
			m_ApplicationUtilWebServiceUri = "http://localhost/wsSPS_Services/SPApplication.asmx"
#End If

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Try
				' Read data over webservice
				Dim searchDataResult = webservice.LoadGeoCoordinationCountryData(m_InitializationData.MDData.MDGuid, countryCode).ToList
				If searchDataResult Is Nothing Then
					m_Logger.LogWarning(String.Format("geo coordinate data could not be loaded! {0}", m_InitializationData.MDData.MDGuid))
					Return Nothing
				End If

				For Each result In searchDataResult

					Dim viewData = New LocationGoordinateViewData With {
						.ID = result.ID,
						.Accuracy = result.Accuracy,
						.AdminCode1 = result.AdminCode1,
						.AdminCode2 = result.AdminCode2,
						.AdminCode3 = result.AdminCode3,
						.AdminName1 = result.AdminName1,
						.AdminName2 = result.AdminName2,
						.AdminName3 = result.AdminName3,
						.CountryCode = result.CountryCode,
						.Latitude = result.Latitude,
						.Longitude = result.Longitude,
						.PlaceName = result.PlaceName,
						.Postcode = result.Postcode
					}

					listDataSource.Add(viewData)

				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try

			Return listDataSource

		End Function

#End Region


		''' <summary>
		''' Removes leading Zeros of string.
		''' </summary>
		''' <param name="str">The string.</param>
		''' <returns>String with removed leading zeros.</returns>
		Private Function RemoveLeadingZeros(ByVal str As String) As String

			If String.IsNullOrWhiteSpace(str) Then
				Return String.Empty
			Else
				Return str.Trim().TrimStart("0")
			End If

		End Function


	End Class


End Namespace
