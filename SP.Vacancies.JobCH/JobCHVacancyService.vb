
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Vacancy
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten

Imports SP.Vacancies.JobCH.JobPlatform.JobsCH
Imports System.Text



Public Class JobCHVacancyUploader


#Region "Private Consts"

  Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_JOBCH_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicejobchvacancies"
  Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_OSTJOB_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceostjobvacancies"
  Public Const MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_SUEDOST_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicesuedostvacancies"
	Private Const DEFAULT_SPUTNIK_VACANCY_JOBSCH_UTIL_WEBSERVICE_URI As String = "wsSPS_Services/SPJobsCHVacancies.asmx"

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
	'Private m_VacanciesUtilOstjobWebServiceUri As String
	'Private m_VacanciesUtilSuedostWebServiceUri As String

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

    Try
      m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
			'm_VacanciesUtilJobchWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_JOBCH_WEBSERVICE_URI, ModulConstants.MDData.MDNr))

			Dim domainName = ModulConstants.MDData.WebserviceDomain
			m_VacanciesUtilJobchWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_VACANCY_JOBSCH_UTIL_WEBSERVICE_URI)


			'm_VacanciesUtilOstjobWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_OSTJOB_WEBSERVICE_URI, ModulConstants.MDData.MDNr))
			'm_VacanciesUtilSuedostWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_VACANCIES_UTIL_SUEDOST_WEBSERVICE_URI, ModulConstants.MDData.MDNr))



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

      Dim vacancyData As DataTable = vacancy.GetVacancyForExportToJobsCH(customerGuid, vakNr)

      Dim vacancyRowData = vacancyData.Rows(0)

      Dim organisationID As String = GetValueFromdataRow(vacancyRowData, "Jobs_Organisation_ID")
      Dim inseratID As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "VakNr"))

      Dim vorspann As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Vorspann")
      Dim taetigkeit As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Anforderung")
      Dim anforderung As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Aufgabe")
      Dim wirBieten As String = GetValueFromdataRow(vacancyRowData, "_Jobs_WirBieten")

      Dim beruf As String = GetValueFromdataRow(vacancyRowData, "Bezeichnung")
      Dim plz As String = GetValueFromdataRow(vacancyRowData, "JobPlz")
      Dim ort As String = GetValueFromdataRow(vacancyRowData, "JobOrt")
      Dim kontakt As String = GetValueFromdataRow(vacancyRowData, "UserKontakt")
      Dim email As String = GetValueFromdataRow(vacancyRowData, "UsereMail")

      Dim startDate As String = GetValueFromdataRow(vacancyRowData, "StartDate")
      Dim endDate As String = GetValueFromdataRow(vacancyRowData, "EndDate")
      Dim titel As String = If(GetValueFromdataRow(vacancyRowData, "JobCH_Titel") = String.Empty,
                               GetValueFromdataRow(vacancyRowData, "Bezeichnung"),
                               GetValueFromdataRow(vacancyRowData, "JobCH_Titel"))
      Dim anriss As String = GetValueFromdataRow(vacancyRowData, "JobCH_Anriss")
      Dim firma As String = GetValueFromdataRow(vacancyRowData, "Firma1")
      Dim anstellungsart As String = GetValueFromdataRow(vacancyRowData, "JobCH_Anstellungsart")
      Dim our_URL As String = GetValueFromdataRow(vacancyRowData, "Our_URL")
      Dim anstellungsgrad_von_bis As String = GetValueFromdataRow(vacancyRowData, "JobCH_Anstellungsgrad")
      Dim rubrikID As String = GetValueFromdataRow(vacancyRowData, "JobCH_RubrikID")
      Dim position As String = GetValueFromdataRow(vacancyRowData, "JobCH_Position")
      Dim branche As String = GetValueFromdataRow(vacancyRowData, "JobCH_Branche")
      Dim sprache As String = GetValueFromdataRow(vacancyRowData, "JobCH_Sprache")
      Dim region As String = GetValueFromdataRow(vacancyRowData, "JobCH_RegionData")
      Dim alter_von_bis As String = GetValueFromdataRow(vacancyRowData, "JobCH_Alter_von_Bis")
      Dim sprachkenntniss_Kandidat As String = GetValueFromdataRow(vacancyRowData, "JobCH_SprachKenntniss")
      Dim sprachkenntniss_Niveau As String = GetValueFromdataRow(vacancyRowData, "JobCH_SprachNiveau")
      Dim bildungsniveau As String = GetValueFromdataRow(vacancyRowData, "JobCH_BildungsNiveauData")
      Dim berufserfahrung As String = GetValueFromdataRow(vacancyRowData, "JobCH_Beruferfahrung")
      Dim berufserfahrung_Position As String = GetValueFromdataRow(vacancyRowData, "JobCH_Beruferfahrung_position")
      Dim layout As String = GetValueFromdataRow(vacancyRowData, "Jobs_Layout_ID")
      Dim logo As String = GetValueFromdataRow(vacancyRowData, "Jobs_Logo_ID")
      Dim bewerben_URL As String = GetValueFromdataRow(vacancyRowData, "Bewerben_URL")
      Dim angebot As String = GetValueFromdataRow(vacancyRowData, "Jobs_Angebot_Value")

			Dim Direkt_URL As String = GetValueFromdataRow(vacancyRowData, "Direkt_URL")
			Dim Direkt_URL_Post_Args As String = String.Empty	' GetValueFromdataRow(vacancyRowData, "Direkt_URL_Post_Args")
			Dim xing_Poster_URL As String = GetValueFromdataRow(vacancyRowData, "Xing_Poster_URL")
      Dim xing_Company_Profile_URL As String = GetValueFromdataRow(vacancyRowData, "Xing_Company_Profile_URL")
      Dim xing_Company_Is_Poc As String = If(GetValueFromdataRow(vacancyRowData, "Xing_Company_Is_Poc") = False, "0", "1")


			Dim jobVacancyModel As New Vacancy
      jobVacancyModel.OrganisationsID = organisationID
      jobVacancyModel.InseratID = inseratID
      jobVacancyModel.Vorspann = vorspann
      jobVacancyModel.Beruf = beruf

      Dim textBuffer As New StringBuilder
      If Not taetigkeit Is Nothing Then
        textBuffer.Append(String.Format("{0}</br>", taetigkeit))
      End If

      If Not anforderung Is Nothing Then
        textBuffer.Append(String.Format("{0}</br>", anforderung))
      End If

      If Not wirBieten Is Nothing Then
        textBuffer.Append(String.Format("{0}</br>", wirBieten))
      End If
      jobVacancyModel.Text = textBuffer.ToString()

      Dim anstellungsgradTuple = SplitTwoValueString(anstellungsgrad_von_bis, "#")

      jobVacancyModel.Anstellungsgrad = anstellungsgradTuple.Item1
      jobVacancyModel.Anstellungsgrad_Bis = anstellungsgradTuple.Item2
      jobVacancyModel.Anstellungart = ConvertDbIdFormat1ToJobChIdFormat(anstellungsart)

      jobVacancyModel.RubrikID = ConvertDbIdFormat2ToJobChIdFormat(rubrikID)
      jobVacancyModel.Position = ConvertDbIdFormat2ToJobChIdFormat(position)

      jobVacancyModel.Branche = ConvertDbIdFormat2ToJobChIdFormat(branche)
      jobVacancyModel.Sprache = sprache
      jobVacancyModel.Region = ConvertDbIdFormat2ToJobChIdFormat(region)

      Dim alterTuple = SplitTwoValueString(alter_von_bis, "#")

      jobVacancyModel.Alter_Von = alterTuple.Item1
      jobVacancyModel.Alter_Bis = alterTuple.Item2

      jobVacancyModel.Sprachkenntniss_Kandidat = ConvertDbIdFormat2ToJobChIdFormat(sprachkenntniss_Kandidat, 3) ' Max 3 ids
      jobVacancyModel.Sprachkenntniss_Niveau = ConvertDbIdFormat2ToJobChIdFormat(sprachkenntniss_Niveau, 3) ' Max 3 ids
      jobVacancyModel.Bildungsniveau = ConvertDbIdFormat2ToJobChIdFormat(bildungsniveau, 3) ' Max 3 ids
      jobVacancyModel.Berufserfahrung = ConvertDbIdFormat2ToJobChIdFormat(berufserfahrung, 2) ' Max 2 ids
      jobVacancyModel.Berufserfahrung_Position = ConvertDbIdFormat2ToJobChIdFormat(berufserfahrung_Position, 2) ' Max 2 ids

      jobVacancyModel.Angebot = angebot

			jobVacancyModel.Direkt_URL = String.Format(Direkt_URL, vakNr)
			jobVacancyModel.Direkt_URL_Post_Args = Direkt_URL_Post_Args

			jobVacancyModel.Xing_Poster_URL = xing_Poster_URL
      jobVacancyModel.Xing_Company_Is_Poc = xing_Company_Is_Poc
      jobVacancyModel.Xing_Company_Profile_URL = xing_Company_Profile_URL

      jobVacancyModel.Layout = layout
      jobVacancyModel.Logo = logo
      jobVacancyModel.Bewerben_URL = bewerben_URL

      result = New UploadResult
      Dim exportResult As New UploadResult

			If jobVacancyModel.IsDataValidForXml Then
				Dim jobCHService As New JobsCHVacancyService.SPJobsCHVacanciesSoapClient

				If Not String.IsNullOrWhiteSpace(m_VacanciesUtilJobchWebServiceUri) Then
					jobCHService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacanciesUtilJobchWebServiceUri)

					exportResult.value = jobCHService.SaveJobCHVacancy(customerGuid, userGuid, vacancyData)
					exportResult.message = If(exportResult.value, "", "Kein bekannter Fehler.")

				Else
					exportResult.value = False
					exportResult.message = "Web service url could not be found!"

				End If

			Else
				exportResult.value = False
				exportResult.message = jobVacancyModel.ValidationErrors

			End If
			result = (exportResult)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

    Return result

  End Function


#Region "Helpers"

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
