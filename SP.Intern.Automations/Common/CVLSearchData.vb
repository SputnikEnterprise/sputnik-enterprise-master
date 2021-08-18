
Imports System.ComponentModel
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Internal.Automations.SPApplicationWebService
Imports SP.Internal.Automations.SPNotificationWebService


Namespace CVLUtilityData

	Public Class CVLData


#Region "private consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"
		Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPApplication.asmx" ' "http://localhost/wsSPS_Services/SPApplication.asmx"

#End Region


#Region "public properties"

		Public Property PostCodeCityData As List(Of SearchPostcodeCityViewData)
		Public Property SearchRadius As Integer
    Public Property JobTitelsData As List(Of SearchExperiencesViewData)
    Public Property OperationAreasData As List(Of SearchExperiencesViewData)
    Public Property OperationAreasJoin As JoinENum
		Public Property SkillsData As List(Of SearchExperiencesViewData)
		Public Property SkillsJoin As JoinENum
		Public Property LanguagesData As List(Of SearchLanguageViewData)
		Public Property LanguagesJoin As JoinENum
		Public Property SearchLabel As String
		Public Property SetNotification As Boolean

		Public Enum JoinENum
			UND
			ODER
		End Enum

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

		Private m_NotificationUtilWebServiceUri As String
		Private m_ApplicationUtilWebServiceUri As String

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_customerID As String

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
		Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Protected m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Protected m_ListingDatabaseAccess As IListingDatabaseAccess


		Private m_fileGuid As String
		Private m_ResultContent As String

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_UtilityUI = New UtilityUI
			m_customerID = m_InitializationData.MDData.MDGuid

			m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

			Dim domainName As String = m_InitializationData.MDData.WebserviceDomain

			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI)

		End Sub


#End Region


		''' <summary>
		''' Loads the postcode and city data.
		''' </summary>
		Public Function LoadCVLPostcodeCityData() As IEnumerable(Of SearchPostcodeCityViewData)

			Dim listDataSource As BindingList(Of SearchPostcodeCityViewData) = New BindingList(Of SearchPostcodeCityViewData)

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadPostcodeCityViewData(m_customerID).ToList

			For Each result In searchResult

				Dim viewData = New SearchPostcodeCityViewData With {
					.Customer_ID = result.Customer_ID,
					.Postcode = result.Postcode,
					.City = result.City
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		''' <summary>
		''' Loads the job group data.
		''' </summary>
		Public Function LoadJobGroupsData() As IEnumerable(Of SearchExperiencesViewData)

			Dim listDataSource As BindingList(Of SearchExperiencesViewData) = New BindingList(Of SearchExperiencesViewData)


			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", CustomerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadJobGroupsViewData(m_customerID).ToList

			For Each result In searchResult

				Dim viewData = New SearchExperiencesViewData With {
					.Customer_ID = result.Customer_ID,
					.Code = result.Code,
					.ExperienceLabel = result.ExperienceLabel
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		Public Function LoadDistancesData() As List(Of Integer)
			Dim list = New List(Of Integer)

			list.Add(20)
			list.Add(30)
			list.Add(40)
			list.Add(50)
			list.Add(75)
			list.Add(100)


			Return (list)

		End Function

		''' <summary>
		''' Loads the language data.
		''' </summary>
		Public Function LoadLanguageData() As IEnumerable(Of SearchLanguageViewData)

			Dim listDataSource As BindingList(Of SearchLanguageViewData) = New BindingList(Of SearchLanguageViewData)

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", CustomerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadLanguageViewData(m_customerID).ToList

			For Each result In searchResult

				Dim viewData = New SearchLanguageViewData With {
					.Customer_ID = result.Customer_ID,
					.LanguageCode = result.LanguageCode,
					.LanguageLabel = result.LanguageLabel
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		''' <summary>
		''' Loads the compentence drop down data.
		''' </summary>
		Public Function LoadExperienceData() As IEnumerable(Of SearchExperiencesViewData)
			Dim listDataSource As BindingList(Of SearchExperiencesViewData) = New BindingList(Of SearchExperiencesViewData)


			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", CustomerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadExperiencesViewData(m_customerID, 0).ToList

			For Each result In searchResult

				Dim viewData = New SearchExperiencesViewData With {
					.Customer_ID = result.Customer_ID,
					.Code = result.Code,
					.ExperienceLabel = result.ExperienceLabel
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		''' <summary>
		''' Loads the search history data.
		''' </summary>
		Public Function LoadCVLSearchHistoryData() As IEnumerable(Of CVLSearchHistoryViewData)
			Dim listDataSource As BindingList(Of CVLSearchHistoryViewData) = New BindingList(Of CVLSearchHistoryViewData)


			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.LoadCVLSearchHistoryData(m_customerID, m_InitializationData.UserData.UserGuid).ToList

			For Each result In searchResult

				Dim viewData = New CVLSearchHistoryViewData With {
					.Customer_ID = result.Customer_ID,
					.User_ID = result.User_ID,
					.ID = result.ID,
					.QueryName = result.QueryName,
					.QueryContent = result.QueryContent,
					.QueryResultContent = result.QueryResultContent,
					.Notify = result.Notify,
					.CreatedFrom = result.CreatedFrom,
					.CreatedOn = result.CreatedOn,
					.ResultCount = result.ResultCount
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		''' <summary>
		''' Loads the search history result data.
		''' </summary>
		Public Function LoadAssignedCVLSearchHistoryResultData(ByVal searchID As Integer) As IEnumerable(Of CVLSearchResultViewData)
			Dim listDataSource As BindingList(Of CVLSearchResultViewData) = New BindingList(Of CVLSearchResultViewData)


			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedCVLSearchHistoryResultData(m_customerID, searchID).ToList

			For Each result In searchResult

				Dim viewData = New CVLSearchResultViewData With {
					.Customer_ID = result.Customer_ID,
					.CountryCode = result.CountryCode,
					.CreatedOn = result.CreatedOn,
					.DateOfBirth = result.DateOfBirth,
					.EmployeeAge = result.EmployeeAge,
					.CVLProfileID = result.CVLProfileID,
					.EmployeeID = result.EmployeeID,
					.Firstname = result.Firstname,
					.Lastname = result.Lastname,
					.Location = result.Location,
					.PersonalID = result.PersonalID,
					.Postcode = result.Postcode,
					.JobTitel = result.JobTitel,
					.Street = result.Street
				}

				listDataSource.Add(viewData)

			Next


			Return listDataSource

		End Function

		Public Function LoadSearchResultData() As IEnumerable(Of CVLSearchResultViewData)
			Dim listDataSource As BindingList(Of CVLSearchResultViewData) = New BindingList(Of CVLSearchResultViewData)



			Dim postCodeAndCityData = New SPApplicationWebService.PostcodeCityViewDataDTO
			Dim redius As Integer = 0
      Dim jobTitels = New List(Of SPApplicationWebService.ExperiencesViewDataDTO)
      Dim operationAreas = New List(Of SPApplicationWebService.ExperiencesViewDataDTO)
      Dim skills = New List(Of SPApplicationWebService.ExperiencesViewDataDTO)
			Dim languages = New List(Of SPApplicationWebService.LanguageViewDataDTO)

			If Not PostCodeCityData Is Nothing AndAlso PostCodeCityData.Count > 0 Then
				'Dim data = CType(luePostcodeCity.EditValue, SearchPostcodeCityViewData)
				postCodeAndCityData.Customer_ID = m_customerID
				postCodeAndCityData.Postcode = PostCodeCityData(0).Postcode
				redius = SearchRadius
			End If
      If Not JobTitelsData Is Nothing AndAlso JobTitelsData.Count > 0 Then
        For Each itm In JobTitelsData
          Dim data = New SPApplicationWebService.ExperiencesViewDataDTO
          data.Customer_ID = m_customerID
          data.Code = itm.Code
          data.ExperienceLabel = itm.ExperienceLabel

          jobTitels.Add(data)
        Next
      End If

      If Not OperationAreasData Is Nothing AndAlso OperationAreasData.Count > 0 Then
				For Each itm In OperationAreasData
					Dim data = New SPApplicationWebService.ExperiencesViewDataDTO
					data.Customer_ID = m_customerID
					data.Code = itm.Code
					data.ExperienceLabel = itm.ExperienceLabel

					operationAreas.Add(data)
				Next
			End If
			If Not SkillsData Is Nothing AndAlso SkillsData.Count > 0 Then
				For Each itm In SkillsData
					Dim data = New SPApplicationWebService.ExperiencesViewDataDTO
					data.Customer_ID = m_customerID
					data.Code = itm.Code
					data.ExperienceLabel = itm.ExperienceLabel

					skills.Add(data)
				Next
			End If
			If Not LanguagesData Is Nothing AndAlso LanguagesData.Count > 0 Then
				For Each itm In LanguagesData
					Dim data = New SPApplicationWebService.LanguageViewDataDTO
					data.Customer_ID = m_customerID
					data.LanguageCode = itm.LanguageCode
					data.LanguageLabel = itm.LanguageLabel

					languages.Add(data)
				Next
			End If

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			Try

				Dim searchResult = webservice.LoadSearchResultAndSaveCriteriaViewData(m_customerID, m_InitializationData.UserData.UserGuid,
																			  postCodeAndCityData, redius, jobTitels.ToArray(), operationAreas.ToArray(), OperationAreasJoin,
																			  skills.ToArray(), SkillsJoin,
																			  languages.ToArray(), LanguagesJoin, SearchLabel, SetNotification).ToList

				If searchResult Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Such-Anfrage konnte nicht bearbeitet werden."))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New CVLSearchResultViewData With {
					.Customer_ID = result.Customer_ID,
					.CountryCode = result.CountryCode,
					.CreatedOn = result.CreatedOn,
					.DateOfBirth = result.DateOfBirth,
					.EmployeeAge = result.EmployeeAge,
					.CVLProfileID = result.CVLProfileID,
					.EmployeeID = result.EmployeeID,
					.Firstname = result.Firstname,
					.Lastname = result.Lastname,
					.Location = result.Location,
					.PersonalID = result.PersonalID,
					.Postcode = result.Postcode,
					.JobTitel = result.JobTitel,
					.Street = result.Street
				}

					listDataSource.Add(viewData)

				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))

				listDataSource = Nothing

			End Try


			Return listDataSource

		End Function

		''' <summary>
		''' delete assigned search history notifier data.
		''' </summary>
		Public Function UpdateAssignedCVLSearchHistoryNotifierData(ByVal searchID As Integer) As Boolean
			Dim success As Boolean = True


			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			success = success AndAlso webservice.UpdateAssignedCVLSearchHistoryNotifierStateData(m_customerID, searchID)


			Return success

		End Function

		''' <summary>
		''' delete assigned search history data.
		''' </summary>
		Public Function DeleteAssignedCVLSearchHistoryData(ByVal searchID As Integer) As Boolean
			Dim success As Boolean = True

			Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			' Read data over webservice
			success = success AndAlso webservice.DeleteAssignedCVLSearchHistoryData(m_customerID, searchID)


			Return success

		End Function


	End Class


	Public Class SearchPostcodeCityViewData

		Public Property Customer_ID As String
		Public Property Postcode As String
		Public Property City As String

		Public ReadOnly Property Addresse As String
			Get
				Return (String.Format("{0} {1}", Postcode, City))
			End Get
		End Property

	End Class

	Public Class SearchExperiencesViewData

		Public Property Customer_ID As String
		Public Property Code As String
		Public Property ExperienceLabel As String

	End Class

	Public Class SearchLanguageViewData

		Public Property Customer_ID As String
		Public Property LanguageCode As String
		Public Property LanguageLabel As String

	End Class

	Public Class CVLSearchHistoryViewData

		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property User_ID As String
		Public Property QueryName As String
		Public Property QueryContent As String
		Public Property QueryResultContent As String
		Public Property Notify As Boolean
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ResultCount As Integer?

		Public ReadOnly Property QueryNameWithCount As String
			Get
				Return String.Format("{0} ({1})", QueryName, ResultCount)
			End Get
		End Property
	End Class

	Public Class CVLSearchResultViewData
		Public Property Customer_ID As String
		Public Property CVLProfileID As Integer?
		Public Property PersonalID As Integer?
		Public Property EmployeeID As Integer?
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property Postcode As String
		Public Property Street As String
		Public Property Location As String
		Public Property CountryCode As String
		Public Property CreatedOn As DateTime?
		Public Property DateOfBirth As DateTime?
		Public Property JobTitel As String
		Public Property EmployeeAge As Integer?

		Public ReadOnly Property ApplicantFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property ApplicantFullnameWithComma As String
			Get
				Return String.Format("{1}, {0}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property ApplicantPostcodeLocation As String
			Get
				Return String.Format("{0}-{1} {2}", CountryCode, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property ApplicantAddress As String
			Get
				Return String.Format("{0}, {0}-{1} {2}", Street, CountryCode, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property CreatedMonth As Integer
			Get
				Return Month(CreatedOn)
			End Get
		End Property

		Public ReadOnly Property CreatedYear As Integer
			Get
				Return Year(CreatedOn)
			End Get
		End Property

	End Class


End Namespace
