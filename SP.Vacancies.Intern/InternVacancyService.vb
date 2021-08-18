
Imports System.ComponentModel
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Vacancy
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten

Imports SP.Vacancies.Intern.JobPlatform.Intern
Imports SP.Vacancies.Intern.AVAMWebServiceProcess

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer

Public Class InternVacancyUploader


#Region "Private Consts"

	Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_INTERN_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicewosvacancies"
	Private Const DEFAULT_SPUTNIK_VACANCY_UTIL_WEBSERVICE_URI As String = "wssps_services/SPInternVacancies.asmx"
	Private Const DEFAULT_SPUTNIK_AVAM_UTIL_WEBSERVICE_URI As String = "wssps_AvamServices/SPInternAvam.asmx"

#End Region

#Region "Private Fields"


	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	''' <summary>
	''' Vacancy datbase access.
	''' </summary>
	Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI
	Private m_JobPlattformUtility As WebServiceProcess

	''' <summary>
	''' Service Uri of Sputnik vacancies util webservice.
	''' </summary>
	Private m_VacancyUtilWebServiceUri As String
	'Private m_AvamUtilWebServiceUri As String
	Private m_connectionString As String

	'Private m_SBN2000JobData As BindingList(Of SBN2000GroupHeaderData)

#End Region


#Region "Constructor"

	'Public Sub New(ByVal _setting As InitializeClass)
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		Try
			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			ModulConstants.MDData = _setting.MDData
			ModulConstants.UserData = _setting.UserData
			ModulConstants.ProsonalizedData = _setting.ProsonalizedData
			ModulConstants.TranslationData = _setting.TranslationData

			m_JobPlattformUtility = New WebServiceProcess(m_InitializationData)

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		'Dim domainAvamName As String = "http://avam.domain.com"
		'If Not domainName.ToLower.Contains("localhost".ToLower) Then
		'	domainAvamName = "http://avam.domain.com"
		'Else
		'	domainAvamName = domainName
		'End If

		m_VacancyUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_UTIL_WEBSERVICE_URI)
		'm_AvamUtilWebServiceUri = String.Format("{0}/{1}", domainAvamName, DEFAULT_SPUTNIK_AVAM_UTIL_WEBSERVICE_URI)

	End Sub

#End Region


	''' <summary>
	''' Uploads vacany data.
	''' </summary>
	''' <param name="userGuid">The user guid.</param>
	''' <param name="customerGuid">The customer guid.</param>
	''' <param name="vakNr">The vacany number.</param>
	Public Function UploadVacancies(ByVal userGuid As String, ByVal customerGuid As String, ByVal vakNr As Integer) As UploadResult
		Dim result As UploadResult = Nothing
		Dim internVacancyModel As New Vacancy
		result = New UploadResult
		Dim wosID As String = m_MandantData.GetWOSGuid(m_InitializationData.MDData.MDNr, Now.Year).WOSVacancyGuid

		Dim exportResult As New UploadResult
		If wosID.Length <= 10 Then
			exportResult.message = "Keine Berechtigung!"
			exportResult.value = False

			Return exportResult
		End If

		Try
			'Dim vacancy As New VacancyDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			Dim vacancyData As DataTable = m_VacancyDatabaseAccess.GetVacancyForExportToIntern(customerGuid, vakNr)

			Dim vacancyRowData = vacancyData.Rows(0)
			Dim organisationID As String = GetValueFromdataRow(vacancyRowData, "Bezeichnung")
			Dim isOnline As Boolean = GetValueFromdataRow(vacancyRowData, "IsOnline")

			If internVacancyModel.IsDataValidForXml Then
				Dim InternService As New InternVacancyService.SPInternVacanciesSoapClient

				m_Logger.LogDebug(String.Format("vacancy will be transfered: vacancynumber: {0} >>> IsOnline: {1}", vakNr, isOnline))

				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

				Dim customerUserData = New SP.Vacancies.Intern.InternVacancyService.CustomerUserData With {.User_Firstname = m_InitializationData.UserData.UserFName,
					.User_Lastname = m_InitializationData.UserData.UserLName,
					.User_eMail = m_InitializationData.UserData.UsereMail,
					.User_Telephone = m_InitializationData.UserData.UserTelefon,
					.User_Telefax = m_InitializationData.UserData.UserTelefax,
					.User_Filiale = m_InitializationData.UserData.UserFiliale,
					.User_Salution = m_InitializationData.UserData.UserSalutation,
					.User_Initial = m_InitializationData.UserData.UserKST,
					.Customer_Name = m_InitializationData.UserData.UserMDName,
					.Customer_Street = m_InitializationData.UserData.UserMDStrasse,
					.Customer_Postcode = m_InitializationData.UserData.UserMDPLZ,
					.Customer_City = m_InitializationData.UserData.UserMDOrt,
					.Customer_Country = m_InitializationData.UserData.UserMDLand,
					.Customer_Telephone = m_InitializationData.UserData.UserMDTelefon,
					.Customer_Telefax = m_InitializationData.UserData.UserMDTelefax,
					.Customer_eMail = m_InitializationData.UserData.UserMDeMail,
					.Customer_Homepage = m_InitializationData.UserData.UserMDHomepage
				}
				Dim transferResult = InternService.AddAssignedVacancyForInternalJobplattform(m_InitializationData.MDData.MDGuid, wosID, userGuid, customerUserData, vacancyData)
				If Not transferResult Is Nothing Then
					exportResult.value = transferResult.JobResult
					exportResult.message = transferResult.JobResultMessage
				End If

			Else
				exportResult.value = False
				exportResult.message = internVacancyModel.ValidationErrors
				m_UtilityUI.ShowErrorDialog(internVacancyModel.ValidationErrors)

			End If
			result = (exportResult)


		Catch ex As Exception
			result.message = ex.Message
			m_Logger.LogError(ex.ToString())

		End Try

		Return result

	End Function


#Region "loading profilmatcher saved searches"


#End Region



#Region "Helpers"

	''' <summary>
	''' Convets the db id format1 to the jobsCH id format.
	''' </summary>
	''' <param name="dbIdString">Format is id#id#id ...</param>
	''' <returns>JobCh id string :id:id:id: ...</returns>
	Private Function ConvertDbIdFormat1ToJobChIdFormat(ByVal dbIdString As String) As String

		If String.IsNullOrEmpty(dbIdString) Then
			Return dbIdString
		End If

		Dim tokens As String() = dbIdString.Split("#")

		Dim stringBuilder As New System.Text.StringBuilder

		For Each token In tokens

			stringBuilder.Append(":")
			stringBuilder.Append(token)
		Next

		stringBuilder.Append(":")

		Return stringBuilder.ToString()
	End Function

	''' <summary>
	''' Convets the db id format2 to the jobsCH id format.
	''' </summary>
	''' <param name="dbIdString">Format is text|id#text|id#text|id ...</param>
	''' <returns>JobCh id string :id:id:id: ...</returns>
	Private Function ConvertDbIdFormat2ToJobChIdFormat(ByVal dbIdString As String, Optional ByVal skiptAfterEntries As Integer? = Nothing) As String

		' Check for empty string.
		If String.IsNullOrEmpty(dbIdString) Then
			Return dbIdString
		End If

		Dim ids As New List(Of String)

		' Split by '#' symbol.
		Dim textAndIdTokens As String() = dbIdString.Split("#")

		For Each textAndIdToken In textAndIdTokens

			' Split by pipe
			Dim token As String() = textAndIdToken.Split("|")

			If token.Count = 2 Then

				ids.Add(token(1))
			Else
				Throw New Exception(String.Format("Invalid format {0}.", dbIdString))
			End If

		Next

		If ids.Count = 0 Then
			Return String.Empty
		Else

			' Convert ids to job ch format
			Dim stringBuilder As New System.Text.StringBuilder

			Dim count As Integer = Math.Min(ids.Count, If(skiptAfterEntries.HasValue, skiptAfterEntries.Value, Integer.MaxValue))

			For i As Integer = 0 To count - 1

				Dim id As String = ids(i)

				stringBuilder.Append(":")
				stringBuilder.Append(id)
			Next

			stringBuilder.Append(":")

			Return stringBuilder.ToString()

		End If

	End Function

	''' <summary>
	''' Splits a two value string.
	''' </summary>
	''' <param name="str">The string.</param>
	''' <param name="delimeter">The delimeter.</param>
	''' <returns>Tuple with values.</returns>
	Private Function SplitTwoValueString(ByVal str As String, ByVal delimeter As String) As Tuple(Of Integer?, Integer?)

		Dim value1 As Integer? = Nothing
		Dim value2 As Integer? = Nothing

		If Not String.IsNullOrEmpty(str) Then

			Dim tokens As String() = str.Trim().Split(delimeter)

			If tokens.Count = 2 Then
				value1 = Integer.Parse(tokens(0))
				value2 = Integer.Parse(tokens(1))
			End If

		End If

		Return New Tuple(Of Integer?, Integer?)(value1, value2)

	End Function


	''' <summary>
	''' Gets a value from a data row.
	''' </summary>
	''' <param name="dataRow">The data row.</param>
	''' <param name="column">The column.</param>
	''' <returns>The value or nothing.</returns>
	Private Function GetValueFromdataRow(ByVal dataRow As DataRow, ByVal column As String) As Object

		If Not dataRow.IsNull(column) Then
			Dim value As Object = dataRow(column)
			Return value
		End If

		Return Nothing
	End Function

	''' <summary>
	''' Parsets a nullable integer.
	''' </summary>
	''' <param name="str">The string.</param>
	''' <returns>Nullable integer value or nothing.</returns>
	Private Function ParseNullableInt(ByVal str As String) As Integer?

		If String.IsNullOrEmpty(str) Then
			Return Nothing
		End If

		Return Integer.Parse(str)

	End Function

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If
	End Function

#End Region



End Class

Public Class STMPJobViewData

	Public Property ID As Integer?
	Public Property GroupNumber As Integer?
	Public Property TitleNumber As Integer?
	Public Property Bez_DE As String
	Public Property Bez_FR As String
	Public Property Bez_IT As String
	Public Property Bez_Translated As String
	Public Property Group_DE As String
	Public Property Group_FR As String
	Public Property Group_IT As String
	Public Property Group_Translated As String
	Public Property Notifiable As Boolean
	Public Property Occupation2020 As Integer

	Public ReadOnly Property DataLabel As String
		Get
			Return String.Format("{0} >>> {1}", Group_Translated, Bez_Translated)
		End Get
	End Property


End Class


Public Class STMPMappingViewData

	Public Property OLD_AVAMNumber As Integer?
	Public Property New_AVAMNumber As Integer?
	Public Property OLD_Bez_Translated As String
	Public Property New_Bez_Translated As String

End Class
