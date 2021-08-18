Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Vacancy
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten

Imports System.Text
Imports SP.Vacancies.OstJobCH.JobPlatform.OstJob

''' <summary>
''' Upload OstJobCHVacancy
''' </summary>
''' <remarks></remarks>
Public Class OstJobCHVacancyUploader

#Region "Private Consts"

  Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_JOBCH_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicejobchvacancies"
  Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_OSTJOB_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceostjobvacancies"
  Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_SUEDOST_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicesuedostvacancies"
	Private Const DEFAULT_SPUTNIK_VACANCY_OSTJOB_UTIL_WEBSERVICE_URI As String = "wsSPS_Services/SPOstJobsCHVacancies.asmx"
	Private Const DEFAULT_SPUTNIK_VACANCY_JOBSCH_UTIL_WEBSERVICE_URI As String = "wsSPS_Services/SPJobsCHVacancies.asmx"
	Private Const DEFAULT_SPUTNIK_VACANCY_SUEDOSTJOB_UTIL_WEBSERVICE_URI As String = "wsSPS_Services/SPJobsCHVacancies.asmx"

#End Region

#Region "Private Fields"

	''' <summary>
	''' Vacancy datbase access.
	''' </summary>
	Private m_VacancyDatabaseAccess As IVacancyDatabaseAccess

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Protected m_Logger As ILogger = New Logger()


  ''' <summary>
  ''' Settings xml.
  ''' </summary>
  Private m_MandantSettingsXml As SettingsXml

  ''' <summary>
  ''' The mandant.
  ''' </summary>
  Private m_MandantData As Mandant
  Private m_UtilityUI As UtilityUI

  ''' <summary>
  ''' Service Uri of Sputnik vacancies util webservice.
  ''' </summary>
  Private m_VacanciesUtilJobchWebServiceUri As String
  Private m_VacanciesUtilOstjobWebServiceUri As String
  Private m_VacanciesUtilSuedostWebServiceUri As String

  ''' <summary>
  ''' The validation erros.
  ''' </summary>
  Private m_ValidationErrors As StringBuilder = New StringBuilder()

#End Region


#Region "Constructor"

  Public Sub New(ByVal _setting As InitializeClass)

    Try
      m_MandantData = New Mandant
      m_UtilityUI = New UtilityUI


      If _setting.MDData Is Nothing Then
        ModulConstants.MDData = ModulConstants.SelectedMDData(0)
        ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

        ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
        ModulConstants.TranslationData = ModulConstants.TranslationValues

      Else
        ModulConstants.MDData = _setting.MDData
        ModulConstants.UserData = _setting.UserData
        ModulConstants.ProsonalizedData = _setting.ProsonalizedData
        ModulConstants.TranslationData = _setting.TranslationData

      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    End Try

		Dim domainName = ModulConstants.MDData.WebserviceDomain
		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))

			'Try
			'	m_VacanciesUtilJobchWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_JOBCH_WEBSERVICE_URI, ModulConstants.MDData.MDNr))
			'Catch ex As Exception
			'	m_Logger.LogError(ex.ToString)
			'End Try
			'Try
			'	m_VacanciesUtilSuedostWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_SUEDOST_WEBSERVICE_URI, ModulConstants.MDData.MDNr))
			'Catch ex As Exception
			'	m_Logger.LogError(ex.ToString)
			'End Try
			'm_VacanciesUtilOstjobWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_OSTJOB_WEBSERVICE_URI, ModulConstants.MDData.MDNr))

			m_VacanciesUtilJobchWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_JOBSCH_UTIL_WEBSERVICE_URI)
			m_VacanciesUtilOstjobWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_OSTJOB_UTIL_WEBSERVICE_URI)
			m_VacanciesUtilSuedostWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_SUEDOSTJOB_UTIL_WEBSERVICE_URI)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
    End Try

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

    Try
      Dim vacancy As New VacancyDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

      Dim vacancyData As DataTable = vacancy.GetVacancyForExportToOstJobsCH(customerGuid, vakNr)

      Dim vacancyRowData = vacancyData.Rows(0)

      'Validation
      m_ValidationErrors.Clear()
      Dim isValid As Boolean = True

      Dim title = GetValueFromdataRow(vacancyRowData, "Title")
      Dim workplaceCity = GetValueFromdataRow(vacancyRowData, "Workplace_City")

      Dim ostjobCh = GetValueFromdataRow(vacancyRowData, "ostjob_ch")
      Dim westjobAt = GetValueFromdataRow(vacancyRowData, "westjob_at")
      Dim nicejobDe = GetValueFromdataRow(vacancyRowData, "nicejob_de")
      Dim zentraljobCh = GetValueFromdataRow(vacancyRowData, "zentraljob_ch")
      Dim minisite = GetValueFromdataRow(vacancyRowData, "minisite")

			isValid = isValid And Check(Not String.IsNullOrWhiteSpace(title), "Title must have value (Vakanz als auf Hauptseite)")
			isValid = isValid And Check(Not String.IsNullOrWhiteSpace(workplaceCity), "WorkplaceCity must have value (Arbeit-Ort auf Hauptseite)")

			Dim hasPublication As Boolean = ostjobCh Or
        westjobAt Or
        nicejobDe Or
        zentraljobCh Or
        minisite

      isValid = isValid And Check(hasPublication, "At least one publication must have the value 1")
			'isValid = isValid And Check(hasPublication, "At least one publication must have the value 1")

			isValid = isValid And Check(Not String.IsNullOrWhiteSpace(m_VacanciesUtilJobchWebServiceUri), "Web service url could not be found!")

      result = New UploadResult
      Dim exportResult As New UploadResult

      If isValid Then

				Dim ostJobCHService As New OstJobsCHVacancyService.SPOstJobsCHVacanciesSoapClient
				ostJobCHService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacanciesUtilOstjobWebServiceUri)

				exportResult.value = ostJobCHService.SaveOstJobCHVacancy(customerGuid, userGuid, vacancyData)
				exportResult.message = If(exportResult.value, "", "Kein bekannter Fehler.")
			Else
				exportResult.value = False
				exportResult.message = m_ValidationErrors.ToString
			End If


			result = (exportResult)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

    Return result

  End Function


#Region "Helper"

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
  ''' Helper method to catch validation errors.
  ''' </summary>
  ''' <param name="isValid">Boolan flag indicating if validation is valid.</param>
  ''' <param name="stringIfNotValid">String if not valid.</param>
  ''' <returns>isValid value.</returns>
  Private Function Check(ByVal isValid As Boolean, ByVal stringIfNotValid As String) As Boolean

    If Not isValid Then
      m_ValidationErrors.Append(stringIfNotValid)
      m_ValidationErrors.Append(";")
    End If

    Return isValid
  End Function

#End Region


End Class
