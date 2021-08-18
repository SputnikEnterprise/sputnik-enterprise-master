
Imports System.ComponentModel
Imports SP.DatabaseAccess.Vacancy
Imports SP.Vacancies.Intern.InternVacancyService

Imports Newtonsoft.Json.Linq

Imports Newtonsoft.Json
Imports System.Threading.Tasks
Imports SP.Vacancies.Intern.AVAMWebServiceProcess

Partial Class InternVacancyUploader

#Region "private consts"

	Private Const JOBROOM_USER As String = "username"
	Private Const JOBROOM_PASSWORD As String = "password"
	Private Const JOBROOM_URI As String = "https://api.job-room.ch/jobAdvertisements/v1"
	Private Const JOBROOM_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
	Private Const JOBROOM_SINGLE_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1/{0}"


	Private Const STAGING_JOBROOM_USER As String = "username"
	Private Const STAGING_JOBROOM_PASSWORD As String = "password"
	Private Const STAGING_JOBROOM_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1"
	Private Const STAGING_JOBROOM_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
	Private Const STAGING_JOBROOM_SINGLE_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1/{0}"

#End Region

	Private m_SearchResultData As JobroomSearchResultData

	Private m_TransmittedSTMPid As String

	Private m_APIResponse As String
	Private m_ReportingObligation As Boolean?
	Private m_ReportingObligationEndDate As DateTime?
	Private m_ResultContent As String

	Private m_Staging As Boolean
	Private m_UserName As String
	Private m_Password As String
	Private m_JobroomURI As String
	Private m_JobroomAllRecordURI As String
	Private m_JobroomSingleRecordURI As String


#Region "public properties"

	Public Property CurrentVacancyNumber As Integer
	Public Property StmpJobID As Integer

	Public ReadOnly Property GetNewSTMPVacancyID As String
		Get
			Return m_TransmittedSTMPid
		End Get
	End Property


#End Region


	Public Function AddSTMPVacancyToRAV(ByVal customerID As String, ByVal userID As String, ByVal staging As Boolean) As SPAVAMCreationResultData
		Dim result As New SPAVAMCreationResultData With {.JobroomID = String.Empty, .State = False}
		Dim msg As String = String.Empty


		m_TransmittedSTMPid = String.Empty
		m_ReportingObligation = False
		m_Staging = staging

		m_JobPlattformUtility.AsStaging = m_Staging


		Dim vacancyData = m_VacancyDatabaseAccess.LoadVacancyMasterData(m_InitializationData.MDData.MDNr, CurrentVacancyNumber)
		If vacancyData Is Nothing Then
			msg = m_Translate.GetSafeTranslationValue("Vakanzendaten konnten nicht geladen werden.")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Dim vacancyJobCHData = m_VacancyDatabaseAccess.LoadJobCHInseratData(CurrentVacancyNumber)
		If vacancyJobCHData Is Nothing Then
			msg = m_Translate.GetSafeTranslationValue("Vakanz-JobCH-Daten konnten nicht geladen werden.")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Dim vacancyStmpData = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullNameWithComma, m_InitializationData.UserData.UserNr, CurrentVacancyNumber)
		If vacancyStmpData Is Nothing OrElse vacancyStmpData.PublicationStartDate < Now.Date Then
			msg = m_Translate.GetSafeTranslationValue("Vakanz-AVAM-Daten konnten nicht geladen werden. Möglicherweise liegt das Pupblikationsdatum vor dem heutigen Datum.<br>(Einstellungen > (AVAM) Meldepflicht > <b>Erscheinungsdatum</b>)")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Dim vacancyStmpLanguageData = m_VacancyDatabaseAccess.LoadVacancyLanguageData(CurrentVacancyNumber, DataObjects.ExternalPlattforms.AVAM)
		If vacancyStmpLanguageData Is Nothing Then
			msg = m_Translate.GetSafeTranslationValue("Vakanz-Stmp-Sprachdaten konnten nicht geladen werden.")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Dim MDData = m_CommonDatabaseAccess.LoadMandantData(vacancyData.MDNr)
		If MDData Is Nothing Then
			msg = m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden.")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Dim userData = m_CommonDatabaseAccess.LoadAdvisorDataforGivenAdvisor(vacancyData.Berater)
		If userData Is Nothing Then
			msg = m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden.")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Dim employerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(vacancyData.KDNr, String.Empty)
		If employerData Is Nothing Then
			msg = m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden.")
			m_Logger.LogWarning(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing
		End If

		Try

			m_Logger.LogDebug(String.Format("data is now collected and starting with transmitting data to jobroom"))

			Dim data = Task.Run(Function() m_JobPlattformUtility.AddAVAMAdvertisementToRAV(customerID, userID, staging, vacancyData, vacancyJobCHData, vacancyStmpData, vacancyStmpLanguageData,
																												 employerData, vacancyData.VakNr, m_InitializationData.UserData.UserLanguage))
			data.Wait()
			result = data.Result
			m_Logger.LogDebug(String.Format("transmitting data to jobroom is finished"))

			If data.Result Is Nothing OrElse Not data.Result.State.GetValueOrDefault(False) Then
				m_Logger.LogDebug(String.Format("transmitting data to jobroom was not successfull!"))

				msg = "<b>(AVAM)</b>: Ihre Vakanz konnte nicht übermittelt werden."
				If Not data.Result.ErrorMessage Is Nothing Then
					msg &= String.Format("{0}<br>{1}<br><br>{2}", vbNewLine, data.Result.ErrorMessage.Message, data.Result.ErrorMessage.Content)
				End If
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

				Return Nothing
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Error while transmitting data to avam{0}{1}", vbNewLine, ex.ToString))

			Return Nothing

		End Try


		Return result

	End Function

	Public Function LoadAsgingedJobAdvertisement(ByVal customerID As String, ByVal userID As String, ByVal staging As Boolean, ByVal vacancyNumber As Integer, ByVal jobroomID As String) As SPAVAMCreationResultData

		Dim result As New SPAVAMCreationResultData With {.JobroomID = String.Empty, .State = False}

		m_TransmittedSTMPid = String.Empty
		m_ReportingObligation = False
		m_Staging = staging

		m_JobPlattformUtility.AsStaging = m_Staging

		Try
			Dim vacancyData = m_VacancyDatabaseAccess.LoadVacancyMasterData(m_InitializationData.MDData.MDNr, CurrentVacancyNumber)
			If vacancyData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanzendaten konnten nicht geladen werden."))

				Return result
			End If

			m_Logger.LogDebug(String.Format("verifying state of advertisment in jobroom: {0} | vacancynumber: {1} | jobroomID: {2}", customerID, vacancyNumber, jobroomID))

			Dim data = Task.Run(Function() m_JobPlattformUtility.LoadAssignedAdvertisementQueryResultData(customerID, vacancyNumber, jobroomID))
			data.Wait()

			result = data.Result
			m_Logger.LogDebug(String.Format("advertisment is verified: jobroomID: {0}", jobroomID))

			If data.Result Is Nothing OrElse Not data.Result.State.GetValueOrDefault(False) Then
				m_Logger.LogDebug(String.Format("verifying was not successfull: jobroomID: {0}", jobroomID))

				Dim msg As String = "<b>(AVAM)</b>: Ihre Vakanz konnte nicht überprüft werden."
				If Not data Is Nothing AndAlso Not String.IsNullOrWhiteSpace(data.Result.ErrorMessage.Message) Then
					msg &= String.Format("{0}<br>{1}", vbNewLine, data.Result.ErrorMessage.Message)
				End If
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

				Return Nothing
			End If

			result.JobroomID = result.JobroomID

			result.AVAMRecordState = result.AVAMRecordState
			result.CreatedFrom = result.CreatedFrom
			result.CreatedOn = result.CreatedOn
			result.QueryContent = result.QueryContent
			result.ReportingObligation = result.ReportingObligation
			result.ReportingObligationEndDate = result.ReportingObligationEndDate
			result.ResultContent = result.ResultContent
			result.State = result.State
			result.SyncDate = Now
			result.SyncFrom = m_InitializationData.UserData.UserFullName


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(msgContent)

			result = Nothing
		End Try

		Return result

	End Function

	Public Function CancelAssignedJobAdvertisementData(ByVal customerID As String, ByVal userID As String, ByVal staging As Boolean, ByVal vacancyNumber As Integer, ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM, ByVal jobroomID As String) As Boolean
		Dim result As Boolean = True
		Dim success As Boolean = True

		m_Staging = staging
		m_JobPlattformUtility.AsStaging = m_Staging

		Try
			m_Logger.LogDebug(String.Format("transmitting cancelation code to avam..."))

			Dim data = Task.Run(Function() m_JobPlattformUtility.CancelAssignedAdvertisementData(customerID, vacancyNumber, reasonEnum, jobroomID))
			data.Wait()

			Dim testResult = data.Result

			result = testResult.State
			If Not result Then
				If testResult.ErrorMessage Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("<b>Achtung:</b> Die Stelle konnte nicht abgemeldet werden."))

				Else
					m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("<b>Achtung:</b> Die Stelle konnte nicht abgemeldet werden.<br>{0}"), testResult.ErrorMessage.Detail))

				End If


				Return False
			End If
			m_Logger.LogDebug(String.Format("transmitting cancelation code to avam was: {0}", result))


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			Return False
		End Try


		Return result

	End Function

	Private Sub LoadUserDataForAVAM()

		m_TransmittedSTMPid = String.Empty
		m_ReportingObligation = False

		m_UserName = JOBROOM_USER
		m_Password = JOBROOM_PASSWORD

		m_JobroomURI = JOBROOM_URI
		m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
		m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI

		If m_Staging Then
			m_UserName = STAGING_JOBROOM_USER
			m_Password = STAGING_JOBROOM_PASSWORD

			m_JobroomURI = STAGING_JOBROOM_URI
			m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI
			m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI

		End If

	End Sub



#Region "load stmp data"



	Public Function LoadSTMP2020AllJobTitileData(ByVal year As Integer) As BindingList(Of STMPJobViewData)
		Dim result = New BindingList(Of STMPJobViewData)

		result = PerformLoadingAVAMAfter2019listWebserviceCall(year)

		Return result

	End Function

	Public Function LoadSTMPMappingData(ByVal oldSBNNumber As Integer?) As BindingList(Of STMPMappingViewData)
		Dim result = New BindingList(Of STMPMappingViewData)

		result = PerformLoadingSTMPMappinglistWebserviceCall(oldSBNNumber)

		If oldSBNNumber.GetValueOrDefault(0) > 0 Then
			Dim data = result.Where(Function(x) x.OLD_AVAMNumber = oldSBNNumber).FirstOrDefault

			If Not data Is Nothing Then
				Dim searchResult = New BindingList(Of STMPMappingViewData)

				Dim viewData = New STMPMappingViewData With {
				.OLD_AVAMNumber = data.OLD_AVAMNumber,
				.New_AVAMNumber = data.New_AVAMNumber,
				.OLD_Bez_Translated = data.OLD_Bez_Translated,
				.New_Bez_Translated = data.New_Bez_Translated
			}

				searchResult.Add(viewData)

				result = searchResult
			End If

		End If

		Return result

	End Function

	Public Function LoadSTMPAllJobTitileData() As BindingList(Of STMPJobViewData)
		Dim result = New BindingList(Of STMPJobViewData)

		result = PerformLoadingSTMPlistWebserviceCall()

		Return result

	End Function

#End Region


#Region "loading stmp data"


	''' <summary>
	'''  Performs loading stmp data.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformLoadingSTMPlistWebserviceCall() As BindingList(Of STMPJobViewData)

		Dim listDataSource = New BindingList(Of STMPJobViewData)


		Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

		Dim searchResult As List(Of STMPJobData) = Nothing

		Try
			' Read data over webservice
			searchResult = webservice.LoadSTMPAllJobTitleData(ModulConstants.MDData.MDGuid, ModulConstants.UserData.UserGuid, 0, ModulConstants.UserData.UserLanguage).ToList

			If searchResult Is Nothing Then
				m_Logger.LogWarning(String.Format("loading stmp result data could not be successfull! {0}", ModulConstants.MDData.MDGuid))
				Return Nothing
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		If searchResult Is Nothing Then Return Nothing

		For Each result In searchResult

			Dim viewData = New STMPJobViewData With {
				.TitleNumber = result.TitleNumber,
				.Bez_Translated = result.Bez_Translated,
				.Group_Translated = result.Group_Translated
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	Private Function PerformLoadingAVAMAfter2019listWebserviceCall(ByVal year As Integer) As BindingList(Of STMPJobViewData)

		Dim listDataSource = New BindingList(Of STMPJobViewData)


		Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

		Dim search2020Result As List(Of STMPJobData) = Nothing

		Try
			' Read data over webservice
			If year = 2020 Then
				search2020Result = webservice.LoadOccupation2020Data(ModulConstants.MDData.MDGuid, ModulConstants.UserData.UserGuid, 0, ModulConstants.UserData.UserLanguage).ToList

			ElseIf year = 2021 Then
				search2020Result = webservice.LoadOccupation2021Data(ModulConstants.MDData.MDGuid, ModulConstants.UserData.UserGuid, 0, ModulConstants.UserData.UserLanguage).ToList

			Else
				Return Nothing

			End If

			If search2020Result Is Nothing Then
				m_Logger.LogWarning(String.Format("loading stmp result data could not be successfull! {0}", ModulConstants.MDData.MDGuid))
				Return Nothing
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		If search2020Result Is Nothing Then Return Nothing

		For Each result In search2020Result

			Dim viewData = New STMPJobViewData With {
				.TitleNumber = result.TitleNumber,
				.Bez_Translated = result.Bez_Translated,
				.Group_Translated = result.Group_Translated,
				.Notifiable = result.Notifiable,
				.Occupation2020 = 2020
			}

			listDataSource.Add(viewData)

		Next

		Return listDataSource

	End Function

	Private Function PerformLoadingSTMPMappinglistWebserviceCall(ByVal oldSBNNumber As Integer?) As BindingList(Of STMPMappingViewData)

		Dim listDataSource = New BindingList(Of STMPMappingViewData)


		Dim webservice As New InternVacancyService.SPInternVacanciesSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

		Dim searchResult As List(Of STMPMappingData) = Nothing

		Try
			' Read data over webservice
			searchResult = webservice.LoadOccupationMappingData(ModulConstants.MDData.MDGuid, ModulConstants.UserData.UserGuid, 0, ModulConstants.UserData.UserLanguage).ToList

			If searchResult Is Nothing Then
				m_Logger.LogWarning(String.Format("loading stmp result data could not be successfull! {0}", ModulConstants.MDData.MDGuid))
				Return Nothing
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		If searchResult Is Nothing Then Return Nothing

		For Each result In searchResult

			Dim viewData = New STMPMappingViewData With {
				.OLD_AVAMNumber = result.OLD_AVAMNumber,
				.New_AVAMNumber = result.New_AVAMNumber,
				.OLD_Bez_Translated = result.OLD_Bez_Translated,
				.New_Bez_Translated = result.New_Bez_Translated
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

#End Region



#Region "helpers"

	Private Function FormatPhoneNumber(ByRef phoneNumber As String) As String
		Dim result As String = phoneNumber
		Dim existsCountryCode As Boolean = False

		If String.IsNullOrWhiteSpace(phoneNumber) Then Return result
		result = result.Replace(" ", "")


		If result.StartsWith("00") OrElse result.StartsWith("++") Then
			result = String.Format("+{0}", result.Substring(2, Len(result) - 2))
			existsCountryCode = True
		End If
		If result.StartsWith("+") Then
			existsCountryCode = True
		End If

		If result.StartsWith("0") Then
			result = String.Format("{0}", result.Substring(1, Len(result) - 1))
		End If
		If Not existsCountryCode Then result = String.Format("+41{0}", result)


		Return result
	End Function

	'Private Function MapObjectToVacancyMasterData(ByVal vacancyData As SP.DatabaseAccess.Vacancy.DataObjects.VacancyMasterData) As VacancyMasterData
	'	Dim result = New VacancyMasterData

	'	result.Anforderung = vacancyData.Anforderung
	'	result.Anstellung = vacancyData.Anstellung
	'	result.Ausbildung = vacancyData.Ausbildung
	'	result.Beginn = vacancyData.Beginn
	'	result.Bemerkung = vacancyData.Bemerkung
	'	result.Berater = vacancyData.Berater
	'	result.Bezeichnung = vacancyData.Bezeichnung
	'	result.ChangedFrom = vacancyData.ChangedFrom
	'	result.ChangedOn = vacancyData.ChangedOn
	'	result.CreatedFrom = vacancyData.CreatedFrom
	'	result.CreatedOn = vacancyData.CreatedOn
	'	result.Customer_Guid = vacancyData.Customer_Guid
	'	result.Dauer = vacancyData.Dauer
	'	result.EDVKennt = vacancyData.EDVKennt
	'	result.ExistLink = vacancyData.ExistLink
	'	result.Filiale = vacancyData.Filiale
	'	result.Gruppe = vacancyData.Gruppe
	'	result.ID = vacancyData.ID
	'	result.IEExport = vacancyData.IEExport
	'	result.JobOrt = vacancyData.JobOrt
	'	result.JobPLZ = vacancyData.JobPLZ
	'	result.JobProzent = vacancyData.JobProzent
	'	result.Jobtime = vacancyData.Jobtime
	'	result.KDBeschreibung = vacancyData.KDBeschreibung
	'	result.KDBietet = vacancyData.KDBietet
	'	result.KDNr = vacancyData.KDNr
	'	result.KDZHDNr = vacancyData.KDZHDNr
	'	result.MAAge = vacancyData.MAAge
	'	result.MAAuto = vacancyData.MAAuto
	'	result.MAFSchein = vacancyData.MAFSchein
	'	result.MALohn = vacancyData.MALohn
	'	result.MANationality = vacancyData.MANationality
	'	result.MASex = vacancyData.MASex
	'	result.MAZivil = vacancyData.MAZivil
	'	result.MDNr = vacancyData.MDNr
	'	result.Reserve1 = vacancyData.Reserve1
	'	result.Reserve2 = vacancyData.Reserve2
	'	result.Reserve3 = vacancyData.Reserve3
	'	result.SBeschreibung = vacancyData.SBeschreibung
	'	result.SBNNumber = vacancyData.SBNNumber
	'	result.SBNPublicationDate = vacancyData.SBNPublicationDate
	'	result.Result = vacancyData.Result
	'	result.SBNPublicationFrom = vacancyData.SBNPublicationFrom
	'	result.SBNPublicationState = vacancyData.SBNPublicationState
	'	result.ShortDescription = vacancyData.ShortDescription
	'	result.SKennt = vacancyData.SKennt
	'	result.Slogan = vacancyData.Slogan
	'	result.SubGroup = vacancyData.SubGroup
	'	result.Taetigkeit = vacancyData.Taetigkeit
	'	result.TitelForSearch = vacancyData.TitelForSearch
	'	result.Transfered_Guid = vacancyData.Transfered_Guid
	'	result.Transfered_On = vacancyData.Transfered_On
	'	result.Transfered_User = vacancyData.Transfered_User
	'	result.UserEMail = vacancyData.UserEMail
	'	result.UserKontakt = vacancyData.UserKontakt
	'	result.VacancyNumberOffset = vacancyData.VacancyNumberOffset
	'	result.VakKontakt = vacancyData.VakKontakt
	'	result.VakKontakt_Value = vacancyData.VakKontakt_Value
	'	result.VakLink = vacancyData.VakLink
	'	result.VakNr = vacancyData.VakNr
	'	result.VakState = vacancyData.VakState
	'	result.VakState_Value = vacancyData.VakState_Value
	'	result.Vak_Kanton = vacancyData.Vak_Kanton
	'	result.Vak_Region = vacancyData.Vak_Region
	'	result.Weiterbildung = vacancyData.Weiterbildung


	'	Return result

	'End Function

	'Private Function MapObjectToVacancyInseratJobCHData(ByVal vacancyData As SP.DatabaseAccess.Vacancy.DataObjects.VacancyInseratJobCHData) As VacancyInseratJobCHData
	'	Dim result = New VacancyInseratJobCHData

	'	result.Anforderung = vacancyData.Anforderung
	'	result.Aufgabe = vacancyData.Aufgabe
	'	result.Bezeichnung = vacancyData.Bezeichnung
	'	result.Vorspann = vacancyData.Vorspann
	'	result.Wirbieten = vacancyData.Wirbieten


	'	Return result

	'End Function

	'Private Function MapObjectToVacancyStmpSettingData(ByVal vacancyData As SP.DatabaseAccess.Vacancy.DataObjects.VacancyStmpSettingData) As VacancyStmpSettingData
	'	Dim result = New VacancyStmpSettingData

	'	result.EducationCode = vacancyData.EducationCode
	'	result.StartDate = vacancyData.PublicationStartDate
	'	result.EndDate = vacancyData.PublicationEndDate
	'	result.NumberOfJobs = vacancyData.NumberOfJobs
	'	result.EuresDisplay = vacancyData.EuresDisplay
	'	result.Home_Work = vacancyData.Home_Work
	'	result.ID = vacancyData.ID
	'	result.Immediately = vacancyData.Immediately
	'	result.IsOnline = vacancyData.IsOnline
	'	result.JobroomID = vacancyData.JobroomID
	'	result.Less_One_Year = vacancyData.Less_One_Year
	'	result.More_One_Year = vacancyData.More_One_Year
	'	result.More_Three_Years = vacancyData.More_Three_Years
	'	result.Night_Work = vacancyData.Night_Work
	'	result.Permanent = vacancyData.Permanent
	'	result.PublicDisplay = vacancyData.PublicDisplay
	'	result.ReportingDate = vacancyData.ReportingDate
	'	result.ReportingFrom = vacancyData.ReportingFrom
	'	result.ReportingObligation = vacancyData.ReportingObligation
	'	result.ReportingObligationEndDate = vacancyData.ReportingObligationEndDate
	'	result.ReportToAvam = vacancyData.ReportToAvam
	'	result.Shift_Work = vacancyData.Shift_Work
	'	result.ShortEmployment = vacancyData.ShortEmployment


	'	result.Sunday_and_Holidays = vacancyData.Sunday_and_Holidays
	'	result.Surrogate = vacancyData.Surrogate
	'	result.VakNr = vacancyData.VakNr


	'	Return result

	'End Function

	'Private Function MapObjectToMandantData(ByVal vacancyData As SP.DatabaseAccess.Common.DataObjects.MandantData) As MandantData
	'	Dim result = New MandantData

	'	result.CustomerID = vacancyData.MandantGuid
	'	result.EMail = vacancyData.EMail
	'	result.Homepage = vacancyData.Homepage
	'	result.ID = vacancyData.ID
	'	result.Location = vacancyData.Location
	'	result.MandantCanton = vacancyData.MandantCanton
	'	result.MandantDbConnection = vacancyData.MandantDbConnection
	'	result.MandantName1 = vacancyData.MandantName1
	'	result.MandantName2 = vacancyData.MandantName2
	'	result.MandantNumber = vacancyData.MandantNumber
	'	result.Postcode = vacancyData.Postcode
	'	result.Street = vacancyData.Street
	'	result.Telefax = vacancyData.Telefax
	'	result.Telephon = vacancyData.Telephon


	'	Return result

	'End Function

	'Private Function MapObjectToAdvisorData(ByVal vacancyData As SP.DatabaseAccess.Common.DataObjects.AdvisorData) As InternAvamService.AdvisorData
	'	Dim result = New InternAvamService.AdvisorData

	'	result.Firstname = vacancyData.Firstname
	'	result.KST = vacancyData.KST
	'	result.KST1 = vacancyData.KST1
	'	result.KST2 = vacancyData.KST2
	'	result.Lastname = vacancyData.Lastname
	'	result.MDCanton = vacancyData.MDCanton
	'	result.Salutation = vacancyData.Salutation
	'	result.UserBusinessBranch = vacancyData.UserBusinessBranch
	'	result.UsereMail = vacancyData.UsereMail
	'	result.UserFiliale = vacancyData.UserFiliale
	'	result.UserFTitel = vacancyData.UserFTitel
	'	result.UserGuid = vacancyData.UserGuid
	'	result.UserLanguage = vacancyData.UserLanguage
	'	result.UserLoginname = vacancyData.UserLoginname
	'	result.UserLoginPassword = vacancyData.UserLoginPassword
	'	result.UserMDDTelefon = vacancyData.UserMDDTelefon
	'	result.UserMDeMail = vacancyData.UserMDeMail
	'	result.UserMDGuid = vacancyData.UserMDGuid
	'	result.UserMDHomepage = vacancyData.UserMDHomepage
	'	result.UserMDLand = vacancyData.UserMDLand
	'	result.UserMDName = vacancyData.UserMDName
	'	result.UserMDName2 = vacancyData.UserMDName2
	'	result.UserMDName3 = vacancyData.UserMDName3
	'	result.UserMDNr = vacancyData.UserMDNr
	'	result.UserMDOrt = vacancyData.UserMDOrt
	'	result.UserMDPLZ = vacancyData.UserMDPLZ
	'	result.UserMDPostfach = vacancyData.UserMDPostfach
	'	result.UserMDStrasse = vacancyData.UserMDStrasse
	'	result.UserMDTelefax = vacancyData.UserMDTelefax
	'	result.UserMDTelefon = vacancyData.UserMDTelefon
	'	result.UserMobile = vacancyData.UserMobile
	'	result.UserNumber = vacancyData.UserNumber
	'	result.UserSTitel = vacancyData.UserSTitel
	'	result.UserTelefax = vacancyData.UserTelefax
	'	result.UserTelefon = vacancyData.UserTelefon


	'	Return result

	'End Function

	'Private Function MapObjectToCustomerMasterData(ByVal vacancyData As SP.DatabaseAccess.Customer.DataObjects.CustomerMasterData) As InternAvamService.CustomerMasterData
	'	Dim result = New InternAvamService.CustomerMasterData

	'	result.BillTypeCode = vacancyData.BillTypeCode
	'	result.CanteenAvailable = vacancyData.CanteenAvailable
	'	result.ChangedFrom = vacancyData.ChangedFrom
	'	result.ChangedOn = vacancyData.ChangedOn
	'	result.Comment = vacancyData.Comment
	'	result.Company1 = vacancyData.Company1
	'	result.Company2 = vacancyData.Company2
	'	result.Company3 = vacancyData.Company3
	'	result.CountryCode = vacancyData.CountryCode
	'	result.CreatedFrom = vacancyData.CreatedFrom
	'	result.CreatedOn = vacancyData.CreatedOn
	'	result.CreditLimit1 = vacancyData.CreditLimit1
	'	result.CreditLimit2 = vacancyData.CreditLimit2
	'	result.CreditLimitsFromDate = vacancyData.CreditLimitsFromDate
	'	result.CreditLimitsToDate = vacancyData.CreditLimitsToDate
	'	result.CreditWarning = vacancyData.CreditWarning
	'	result.CurrencyCode = vacancyData.CurrencyCode
	'	result.CustomerMandantNumber = vacancyData.CustomerMandantNumber
	'	result.CustomerNumber = vacancyData.CustomerNumber
	'	result.CustomerState1 = vacancyData.CustomerState1
	'	result.CustomerState2 = vacancyData.CustomerState2
	'	result.DoNotShowContractInWOS = vacancyData.DoNotShowContractInWOS
	'	result.EMail = vacancyData.EMail
	'	result.Email_Mailing = vacancyData.Email_Mailing
	'	result.facebook = vacancyData.facebook
	'	result.FirstProperty = vacancyData.FirstProperty
	'	result.Hompage = vacancyData.Hompage
	'	result.HowContact = vacancyData.HowContact
	'	result.InvoiceOption = vacancyData.InvoiceOption
	'	result.KD_UmsMin = vacancyData.KD_UmsMin
	'	result.KST = vacancyData.KST
	'	result.Language = vacancyData.Language
	'	result.Latitude = vacancyData.Latitude
	'	result.Location = vacancyData.Location
	'	result.Longitude = vacancyData.Longitude
	'	result.mwstpflicht = vacancyData.mwstpflicht
	'	result.NotPrintReports = vacancyData.NotPrintReports
	'	result.NoUse = vacancyData.NoUse
	'	result.NoUseComment = vacancyData.NoUseComment
	'	result.NumberOfCopies = vacancyData.NumberOfCopies
	'	result.NumberOfEmployees = vacancyData.NumberOfEmployees
	'	result.OpenInvoiceAmount = vacancyData.OpenInvoiceAmount
	'	result.OPShipment = vacancyData.OPShipment
	'	result.Postcode = vacancyData.Postcode
	'	result.PostOfficeBox = vacancyData.PostOfficeBox
	'	result.ReferenceNumber = vacancyData.ReferenceNumber
	'	result.Reserve1 = vacancyData.Reserve1
	'	result.Reserve2 = vacancyData.Reserve2
	'	result.Reserve3 = vacancyData.Reserve3
	'	result.Reserve4 = vacancyData.Reserve4
	'	result.SalaryPerHour = vacancyData.SalaryPerHour
	'	result.SalaryPerMonth = vacancyData.SalaryPerMonth
	'	result.sendToWOS = vacancyData.sendToWOS
	'	result.ShowHoursInNormal = vacancyData.ShowHoursInNormal
	'	result.SolvencyDecisionID = vacancyData.SolvencyDecisionID
	'	result.SolvencyInfo = vacancyData.SolvencyInfo
	'	result.Street = vacancyData.Street
	'	result.Telefax = vacancyData.Telefax
	'	result.Telefax_Mailing = vacancyData.Telefax_Mailing
	'	result.Telephone = vacancyData.Telephone
	'	result.TermsAndConditions_WOS = vacancyData.TermsAndConditions_WOS
	'	result.Transfered_Guid = vacancyData.Transfered_Guid
	'	result.TransportationOptions = vacancyData.TransportationOptions
	'	result.ValueAddedTaxNumber = vacancyData.ValueAddedTaxNumber
	'	result.WOSGuid = vacancyData.WOSGuid
	'	result.xing = vacancyData.xing


	'	Return result

	'End Function

	'Private Function MapObjectToVacancyJobCHLanguageData(ByVal vacancyData As List(Of SP.DatabaseAccess.Vacancy.DataObjects.VacancyJobCHLanguageData)) As List(Of InternAvamService.VacancyJobCHLanguageData)
	'	Dim result = New List(Of InternAvamService.VacancyJobCHLanguageData)

	'	For Each lang In vacancyData
	'		Dim data As New InternAvamService.VacancyJobCHLanguageData

	'		data.Bezeichnung = lang.Bezeichnung
	'		data.Bezeichnung_Value = lang.Bezeichnung_Value
	'		data.ID = lang.ID
	'		data.LanguageNiveau = lang.LanguageNiveau
	'		data.LanguageNiveau_Value = lang.LanguageNiveau_Value
	'		data.VakNr = lang.VakNr

	'		result.Add(data)

	'	Next


	'	Return result

	'End Function


#End Region


End Class


Public Class ParsJobroomJSONData

	Private m_XMLfilename As String
	Private m_JSONContent As String


	Public Sub New(ByVal jsonContent As String)
		m_JSONContent = jsonContent
	End Sub

	Public Function LoadProfilMatcherResultData() As JobroomSearchResultData
		Dim result As JobroomSearchResultData = Nothing

		Dim jsonResulttodict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(m_JSONContent)
		Dim firstItem = jsonResulttodict.Item("id")

		Dim json As JObject = JObject.Parse(m_JSONContent)


		For Each rec In json.SelectToken("content")
			Dim content = JsonConvert.DeserializeObject(Of JobroomSearchResultData)(rec)

		Next
		Dim reader = JsonConvert.DeserializeObject(Of JobroomSearchResultData)(m_JSONContent)
		result = New JobroomSearchResultData

		result = reader


		Return result

	End Function



#Region "helpers"

	Private Function GetSafeStringFromXElement(ByVal xelment As XElement) As String

		If xelment Is Nothing Then
			Return String.Empty
		Else

			Return xelment.Value
		End If

	End Function

	Private Function GetSafeDateFromXElement(ByVal xelment As XElement) As Date?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CDate(xelment.Value)
		End If

	End Function

	Private Function GetSafeBooleanFromXElement(ByVal xelment As XElement) As Boolean?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CBool(xelment.Value)
		End If

	End Function

	Private Function GetSafeIntegerFromXElement(ByVal xelment As XElement) As Integer?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CInt(xelment.Value)
		End If

	End Function

	Private Function GetSafeLongFromXElement(ByVal xelment As XElement) As Long?

		If xelment Is Nothing Then
			Return Nothing
		Else

			Return CLng(xelment.Value)
		End If

	End Function

	Private Function GetSafeByteFromXElement(ByVal xelment As XElement) As Byte()

		If xelment Is Nothing Then
			Return Nothing
		Else

			'Dim utf8 As Encoding = Encoding.UTF8()
			Dim bytes As Byte() = Convert.FromBase64String(xelment.Value) ' utf8.GetBytes(xelment.Value)


			Return (bytes)
		End If

	End Function

#End Region



End Class






Public Class SearchResponseData
	Public Property HttpState As String
	Public Property APIResponse As String
	Public Property APIResult As String

End Class


Public Class JobroomSearchResultData
	Public Property Content As List(Of JobroomData)
	Public Property TotalElements As Integer?
	Public Property TotalPages As Integer?
	Public Property CurrentPage As Integer?
	Public Property CurrentSize As Integer?
	Public Property First As Boolean?
	Public Property Last As Boolean?
	Public Property ErrorMessage As ErrorData

End Class


Public Class JobroomData
	Public Property ID As String
	Public Property Status As String
	Public Property SourceSystem As String
	Public Property ExternalReference As String
	Public Property StellennummerEgov As String
	Public Property StellennummerAvam As String
	Public Property Fingerprint As String
	Public Property ReportingObligation As Boolean?
	Public Property ReportingObligationEndDate As Date?
	Public Property ReportToAvam As Boolean?
	Public Property JobCenterCode As String
	Public Property ApprovalDate As Date?
	Public Property RejectionDate As Date?
	Public Property RejectionCode As String
	Public Property RejectionReason As String
	Public Property CancellationDate As Date?
	Public Property CancellationCode As String
	Public Property JobContent As JobContentData
	Public Property Publication As PublicationData

End Class


Public Class JobContentData
	Public Property ExternalUrl As String
	Public Property JobDescriptions As List(Of JobDescriptionData)
	Public Property Company As CompanyData
	Public Property Employment As EmploymentData
	Public Property Location As LocationData
	Public Property Occupations As List(Of OccupationsData)
	Public Property LanguageSkills As List(Of LanguageSkillsData)
	Public Property ApplyChannel As ApplyChannelData
	Public Property PublicContact As PublicContactData

End Class

Public Class JobDescriptionData
	Public Property LanguageIsoCode As String
	Public Property Title As String
	Public Property Description As String

End Class

Public Class CompanyData
	Public Property Name As String
	Public Property Street As String
	Public Property HouseNumber As String
	Public Property PostalCode As String
	Public Property City As String
	Public Property CountryIsoCode As String
	Public Property PostOfficeBoxNumber As String
	Public Property PostOfficeBoxPostalCode As String
	Public Property PostOfficeBoxCity As String
	Public Property Phone As String
	Public Property Email As String
	Public Property Website As String
	Public Property Surrogate As Boolean?

End Class

Public Class EmploymentData
	Public Property StartDate As Date?
	Public Property EndDate As Date?
	Public Property ShortEmployment As Boolean?
	Public Property Immediately As Boolean?
	Public Property Permanent As Boolean?
	Public Property WorkloadPercentageMin As String
	Public Property WorkloadPercentageMax As String
	Public Property WorkForms As List(Of WorkFormsData)

End Class

Public Class LocationData
	Public Property Remarks As String
	Public Property City As String
	Public Property PostalCode As String
	Public Property CommunalCode As String
	Public Property RegionCode As String
	Public Property CantonCode As String
	Public Property CountryIsoCode As String
	Public Property Coordinates As String

End Class

Public Class WorkFormsData
	Public Property Value As String

End Class

Public Class OccupationsData
	Public Property AvamOccupationCode As Integer?
	Public Property WorkExperience As String
	Public Property EducationCode As String

End Class

Public Class LanguageSkillsData
	Public Property LanguageIsoCode As String
	Public Property SpokenLevel As String
	Public Property WrittenLevel As String

End Class

Public Class ApplyChannelData
	Public Property MailAddress As String
	Public Property EmailAddress As String
	Public Property PhoneNumber As String
	Public Property FormUrl As String
	Public Property AdditionalInfo As String

End Class

Public Class PublicContactData
	Public Property Salutation As String
	Public Property FirstName As String
	Public Property LastName As String
	Public Property Phone As String
	Public Property Email As String

End Class

Public Class PublicationData
	Public Property StartDate As Date?
	Public Property EndDate As Date?
	Public Property EuresAnonymous As Boolean?
	Public Property PublicDisplay As Boolean?
	Public Property PublicAnonymous As Boolean?
	Public Property RestrictedDisplay As Boolean?
	Public Property RestrictedAnonymous As Boolean?

End Class




Public Class JobCreationData
	Public Property AVAMRecordState As String
	Public Property JobroomID As String
	Public Property ReportingObligation As Boolean?
	Public Property reportingObligationEndDate As DateTime?
	Public Property State As Boolean?
	Public Property Content As JobContentData
	Public Property ErrorMessage As ErrorData

	Public Property SyncDate As DateTime?
	Public Property SyncFrom As String


	Public ReadOnly Property AVAMStateEnum As AVAMState

		Get
			Dim value As AVAMState = AVAMState.INSPECTING

			If String.IsNullOrWhiteSpace(AVAMRecordState) OrElse value = "INSPECTING" Then value = AVAMState.INSPECTING
			If AVAMRecordState = "REJECTED" Then value = AVAMState.REJECTED
			If AVAMRecordState = "PUBLISHED_RESTRICTED" Then value = AVAMState.PUBLISHED_RESTRICTED
			If AVAMRecordState = "PUBLISHED_PUBLIC" Then value = AVAMState.PUBLISHED_PUBLIC
			If AVAMRecordState = "CANCELLED" Then value = AVAMState.CANCELLED
			If AVAMRecordState = "ARCHIVED" Then value = AVAMState.ARCHIVED


			Return value

		End Get

	End Property

End Class

Public Enum AVAMState

	INSPECTING
	REJECTED
	PUBLISHED_RESTRICTED
	PUBLISHED_PUBLIC
	CANCELLED
	ARCHIVED

End Enum

Public Enum AVAMAdvertismentCancelReason

	OCCUPIED_JOBCENTER
	OCCUPIED_AGENCY
	OCCUPIED_JOBROOM
	OCCUPIED_OTHER
	NOT_OCCUPIED
	CHANGE_OR_REPOSE

End Enum

Public Class ErrorData
	Public Property Content As String
	Public Property Title As String
	Public Property Status As String
	Public Property Message As String
	Public Property Detail As String

End Class

