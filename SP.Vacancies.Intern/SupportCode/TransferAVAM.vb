
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports System.Net
Imports System.Threading.Tasks
Imports System.Text
Imports System.IO
Imports Newtonsoft.Json
Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports SP.Infrastructure

Namespace AVAMWebServiceProcess

	Partial Class WebServiceProcess


		Public Async Function AddAVAMAdvertisementToRAV(ByVal customerID As String, ByVal userID As String, ByVal asStaging As Boolean, ByVal vacancyData As VacancyMasterData,
																								ByVal vacancyJobCHData As VacancyInseratJobCHData, ByVal vacancyStmpData As VacancyStmpSettingData,
																								ByVal vacancyStmpLanguageData As List(Of VacancyJobCHLanguageData),
																								ByVal employerData As CustomerMasterData,
																								ByVal jobNumber As Integer?, ByVal language As String) As Task(Of SPAVAMCreationResultData)

			Dim result As New SPAVAMCreationResultData With {.JobroomID = String.Empty, .State = False}
			Dim userFullname As String = "System"

			m_TransmittedSTMPid = String.Empty
			m_ReportingObligation = False

			m_TransmittedSTMPid = String.Empty

			LoadUserDataForAVAM()

			Dim jQueryAddvertismentContent = BuildJasonstring(customerID, userID, vacancyData, vacancyJobCHData, vacancyStmpData, vacancyStmpLanguageData, employerData, jobNumber, language)
			If String.IsNullOrWhiteSpace(jQueryAddvertismentContent.ToString) Then
				m_Logger.LogError("build BuildJasonstring was failed!")

				Return Nothing
			End If

			Dim success As Boolean = True
			Try
				Dim creationReslt As String = String.Empty

				Try
					Dim transferResult As New SPAVAMCreationResultData

					Dim baseUri As Uri = New Uri(m_JobroomURI)

					Dim response As New HttpResponseMessage
					Dim data = Task.Run(Function() webserviceResponse(jQueryAddvertismentContent, baseUri, "Post", m_UserName, m_Password))
					data.Wait()

					response = data.Result
					If response Is Nothing Then
						Return Nothing
					End If

					Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
					creationReslt = myStreamReader.ReadToEnd

					If response.StatusCode > 204 Then
						Dim failedResult = ParseJSonError(creationReslt)

						result.ErrorMessage = failedResult.ErrorMessage


						Return result
					End If
					transferResult = ParseCreateAddvertismentJSonResult(creationReslt)

					'#End If


					If transferResult Is Nothing Then Throw New Exception("Vakanz konnte nicht übermittelt werden.")

					m_TransmittedSTMPid = transferResult.JobroomID

					result.JobroomID = m_TransmittedSTMPid
					result.StellennummerEgov = transferResult.StellennummerEgov
					result.QueryContent = jQueryAddvertismentContent.ToString
					result.ResultContent = transferResult.ResultContent
					result.AVAMRecordState = transferResult.AVAMRecordState
					result.ReportingObligation = transferResult.ReportingObligation.GetValueOrDefault(False)
					result.ReportingObligationEndDate = transferResult.ReportingObligationEndDate

					result.State = Not String.IsNullOrWhiteSpace(m_TransmittedSTMPid)
					result.CreatedOn = Now
					result.CreatedFrom = m_InitializationData.UserData.UserFullName

					If Not result Is Nothing AndAlso result.State.GetValueOrDefault(False) Then success = success AndAlso AddAVAMACreationdvertismentDataToWebservice(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid, vacancyData.VakNr, False, result)

				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
					result = Nothing

				End Try

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString, "AddAVAMAdvertisementToRAV.WebserviceResponse")

				m_TransmittedSTMPid = Nothing
				result = Nothing

			End Try


			Return result

		End Function

		Public Async Function LoadAssignedAdvertisementQueryResultData(ByVal customerID As String, ByVal vacancyNumber As Integer, ByVal v_id As String) As Task(Of SPAVAMCreationResultData)
			Dim result As New SPAVAMCreationResultData With {.JobroomID = String.Empty, .State = False}
			Dim success As Boolean = True

			LoadUserDataForAVAM()

			Try
				Dim baseUri As Uri = New Uri(String.Format(m_JobroomSingleRecordURI, v_id))
				m_Logger.LogInfo(String.Format("sending query to jobroom: JobroomID: {0} | Userfullname: {1} | m_JobroomSingleRecordURI: {2}", v_id, m_InitializationData.UserData.UserFullName, m_JobroomSingleRecordURI))

				Dim response As New HttpResponseMessage
				Dim data = Task.Run(Function() webserviceResponse(Nothing, baseUri, "Get", m_UserName, m_Password))
				data.Wait()

				response = data.Result
				If response Is Nothing Then
					Return Nothing
				End If

				Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
				Dim jsonString = myStreamReader.ReadToEnd

				If response.StatusCode > 204 Then
					Dim failedResult = ParseJSonError(jsonString)
					result.ErrorMessage = failedResult.ErrorMessage

					Return result
				End If

				Dim searchResult = ParseCreateAddvertismentJSonResult(jsonString)

				If searchResult Is Nothing Then Throw New Exception("Vakanz konnte nicht überprüft werden!")

				m_TransmittedSTMPid = searchResult.JobroomID

				result.JobroomID = m_TransmittedSTMPid
				result.QueryContent = String.Format("{0}/{1}", baseUri.ToString, v_id)
				result.ResultContent = searchResult.ResultContent
				result.AVAMRecordState = searchResult.AVAMRecordState
				result.ReportingObligation = searchResult.ReportingObligation.GetValueOrDefault(False)
				result.ReportingObligationEndDate = searchResult.ReportingObligationEndDate

				result.State = Not String.IsNullOrWhiteSpace(m_TransmittedSTMPid)
				result.CreatedOn = Now
				result.CreatedFrom = m_InitializationData.UserData.UserFullName

				If Not result Is Nothing AndAlso result.State.GetValueOrDefault(False) Then success = success AndAlso AddAVAMAdvertismentQueryResultDataToWebservice(customerID, m_InitializationData.UserData.UserGuid, vacancyNumber, result)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try


			Return result

		End Function

		Public Async Function CancelAssignedAdvertisementData(ByVal customerID As String, ByVal vacancyNumber As Integer, ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM, ByVal v_id As String) As Task(Of SPAVAMCreationResultData)
			Dim result As New SPAVAMCreationResultData With {.JobroomID = String.Empty, .State = False}
			Dim success As Boolean = True

			LoadUserDataForAVAM()
			Dim jQueryCancelationContent = BuildCancelationJSONString(reasonEnum)

			Try
				If AsStaging Then m_JobroomSingleRecordURI = STAGING_JOBROOM_CANCEL_RECORDS_URI Else m_JobroomSingleRecordURI = JOBROOM_CANCEL_RECORDS_URI
				Dim baseUri As Uri = New Uri(String.Format(m_JobroomSingleRecordURI, v_id))

				m_Logger.LogInfo(String.Format("CustomerID: {1} | JobroomID: {2} | asStaging: {3} | UserFullname: {4}{0}baseUri: {5} | m_UserName: {6} | m_Password: {7}{0}m_JobroomSingleRecordURI: {8}",
																			 vbNewLine, customerID, v_id, AsStaging, m_InitializationData.UserData.UserFullName, baseUri.ToString, m_UserName, m_Password, m_JobroomSingleRecordURI))

				Dim response As New HttpResponseMessage
				Dim data = Task.Run(Function() webserviceResponse(jQueryCancelationContent, baseUri, "Patch", m_UserName, m_Password))
				data.Wait()

				response = data.Result
				If response Is Nothing Then
					Return Nothing
				End If

				Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
				Dim jsonString = myStreamReader.ReadToEnd

				If response.StatusCode > 204 Then
					Dim failedResult = ParseJSonError(jsonString)
					result.ErrorMessage = failedResult.ErrorMessage

					Return result
				End If

				result = New SPAVAMCreationResultData With {.JobroomID = v_id, .State = True, .ResultContent = jsonString, .QueryContent = jQueryCancelationContent.ToString}

				'Dim searchResult = ParseCreateAddvertismentJSonResult(jsonString)

				'If searchResult Is Nothing Then Throw New Exception("Vakanz konnte nicht übermittelt werden.")

				'm_TransmittedSTMPid = searchResult.JobroomID

				'result.JobroomID = m_TransmittedSTMPid
				'result.QueryContent = String.Format("{0}/{1}", baseUri.ToString, v_id)
				'result.ResultContent = searchResult.ResultContent
				'result.AVAMRecordState = searchResult.AVAMRecordState
				'result.ReportingObligation = searchResult.ReportingObligation.GetValueOrDefault(False)
				'result.ReportingObligationEndDate = searchResult.ReportingObligationEndDate

				'result.State = Not String.IsNullOrWhiteSpace(m_TransmittedSTMPid)
				'result.CreatedOn = Now
				'result.CreatedFrom = m_InitializationData.UserData.UserFullName

				If Not result Is Nothing AndAlso result.State.GetValueOrDefault(False) Then success = success AndAlso AddAVAMAdvertismentQueryResultDataToWebservice(customerID, m_InitializationData.UserData.UserGuid, vacancyNumber, result)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try


			Return result

		End Function


		'Public Function LoadAllJobAdvertisement(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As SPAdvisorData) As Boolean
		'	Dim result As Boolean = True
		'	Dim msgContent As String

		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_UserName) Then
		'		If asStaging Then m_UserName = STAGING_JOBROOM_USER Else m_UserName = JOBROOM_USER

		'	Else
		'		m_UserName = My.Settings.AVAM_UserName
		'	End If
		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_Password) Then
		'		If asStaging Then m_Password = STAGING_JOBROOM_PASSWORD Else m_Password = JOBROOM_PASSWORD
		'	Else
		'		m_Password = My.Settings.AVAM_Password
		'	End If

		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomURI) Then
		'		m_JobroomURI = JOBROOM_URI
		'	Else
		'		m_JobroomURI = My.Settings.AVAM_JobroomURI
		'	End If
		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomAllRecordURI) Then
		'		If asStaging Then m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI Else m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
		'	Else
		'		m_JobroomAllRecordURI = My.Settings.AVAM_JobroomAllRecordURI
		'	End If
		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomSingleRecordURI) Then
		'		If asStaging Then m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI Else m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI
		'	Else
		'		m_JobroomSingleRecordURI = My.Settings.AVAM_JobroomSingleRecordURI
		'	End If

		'	Dim baseUri As Uri = New Uri(String.Format(m_JobroomAllRecordURI, 0, 25))
		'	m_SearchResultData = New JobroomSearchResultData

		'	Try
		'		Dim queryResult = WebserviceGetRequest(baseUri, m_UserName, m_Password)

		'		result = result AndAlso ParseJSonResult(queryResult)

		'		For Each itm In m_SearchResultData.Content
		'			Dim v_id As String = itm.ID

		'			If String.IsNullOrWhiteSpace(itm.ID) Then
		'				msgContent = String.Format("{0} is empty.", v_id)
		'				m_Utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllJobAdvertisement", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'				Continue For
		'			End If

		'			Dim queryData As New SPAVAMJobCreationData
		'			queryData.AVAMRecordState = itm.Status
		'			queryData.JobroomID = itm.ID
		'			queryData.Content = itm.JobContent

		'			queryData.State = Not String.IsNullOrWhiteSpace(v_id)
		'			queryData.ReportingObligation = itm.ReportingObligation.GetValueOrDefault(False)
		'			queryData.reportingObligationEndDate = itm.ReportingObligationEndDate
		'			queryData.ResultContent = queryResult

		'			queryData.SyncDate = Now
		'			queryData.SyncFrom = userData.UserFullname
		'			queryData.CreatedFrom = userData.UserFullname

		'			result = result AndAlso LoadAssignedQueryData(v_id)
		'			If m_QueryResultData Is Nothing OrElse m_QueryResultData.ID.GetValueOrDefault(0) = 0 Then
		'				If Not queryData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(queryData.JobroomID) Then result = result AndAlso AddNotifyDataIntoDatabase(m_customerID, userData.UserGuid, queryData)
		'			Else
		'				If Not queryData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(queryData.JobroomID) Then result = result AndAlso UpdateNotifyDataIntoDatabase(customerID, userData.UserGuid, queryData)
		'			End If

		'		Next


		'	Catch ex As Exception
		'		msgContent = ex.ToString
		'		m_Utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllJobAdvertisement", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'		m_Logger.LogError(ex.ToString)

		'		Return False

		'	End Try

		'	Return True

		'End Function

		'Public Async Function CancelAssignedJobAdvertisementData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As SPAdvisorData, ByVal v_id As String, ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM) As WebServiceResult
		'	Dim result As New WebServiceResult With {.JobResult = False, .JobResultMessage = String.Empty} 'New SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
		'	Dim success As Boolean = True
		'	Dim msgContent As String

		'	m_customerID = customerID
		'	m_UserData = userData

		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_UserName) Then
		'		If asStaging Then m_UserName = STAGING_JOBROOM_USER Else m_UserName = JOBROOM_USER
		'	Else
		'		m_UserName = My.Settings.AVAM_UserName
		'	End If
		'	If String.IsNullOrWhiteSpace(My.Settings.AVAM_Password) Then
		'		If asStaging Then m_Password = STAGING_JOBROOM_PASSWORD Else m_Password = JOBROOM_PASSWORD
		'	Else
		'		m_Password = My.Settings.AVAM_Password
		'	End If

		'	If asStaging Then m_JobroomSingleRecordURI = STAGING_JOBROOM_CANCEL_RECORDS_URI Else m_JobroomSingleRecordURI = JOBROOM_CANCEL_RECORDS_URI

		'	Dim sb As New StringBuilder()
		'	Dim sw As New StringWriter(sb)

		'	Try

		'		Using writer As JsonWriter = New JsonTextWriter(sw)

		'			writer.WriteStartObject()

		'			writer.WritePropertyName("code")
		'			Select Case reasonEnum
		'				Case AVAMAdvertismentCancelReasonENUM.CHANGE_OR_REPOSE
		'					writer.WriteValue("CHANGE_OR_REPOSE")
		'				Case AVAMAdvertismentCancelReasonENUM.NOT_OCCUPIED
		'					writer.WriteValue("NOT_OCCUPIED")
		'				Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_AGENCY
		'					writer.WriteValue("OCCUPIED_AGENCY")
		'				Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_JOBCENTER
		'					writer.WriteValue("OCCUPIED_JOBCENTER")
		'				Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_JOBROOM
		'					writer.WriteValue("OCCUPIED_JOBROOM")
		'				Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_OTHER
		'					writer.WriteValue("OCCUPIED_OTHER")

		'				Case Else
		'					Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "no reason was defined!"}

		'			End Select

		'			writer.WriteEndObject()

		'		End Using

		'	Catch ex As Exception
		'		msgContent = ex.ToString
		'		m_Utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CancelAssignedJobAdvertisementData", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'		Return New WebServiceResult With {.JobResult = False, .JobResultMessage = ex.ToString}
		'	End Try

		'	Try
		'		'v_id = "f5c1c0fb-a537-11e8-9710-005056ac3479"
		'		Dim baseUri As Uri = New Uri(String.Format(m_JobroomSingleRecordURI, v_id))
		'		result = WebservicePATCHResponse(sb.ToString(), baseUri, m_UserName, m_Password)
		'		If Not result.JobResult Then Return result


		'		Dim resultData As New SPAVAMJobCreationData With {.CreatedFrom = userData.UserFullname, .CreatedOn = Now, .ResultContent = "WebservicePATCHResponse: successfull"}
		'		success = success AndAlso AddNotifyDataIntoDatabase(m_customerID, m_UserData.UserGuid, resultData)

		'	Catch ex As Exception
		'		msgContent = ex.ToString
		'		m_Utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CancelAssignedJobAdvertisementData", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'		m_Logger.LogError(ex.ToString)
		'		Return New WebServiceResult With {.JobResult = False, .JobResultMessage = ex.ToString}

		'	End Try

		'	Return result

		'End Function



		Private Sub LoadUserDataForAVAM()

			m_TransmittedSTMPid = String.Empty
			m_ReportingObligation = False

			m_UserName = JOBROOM_USER
			m_Password = JOBROOM_PASSWORD

			m_JobroomURI = JOBROOM_URI
			m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
			m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI

			If AsStaging Then
				m_UserName = STAGING_JOBROOM_USER
				m_Password = STAGING_JOBROOM_PASSWORD

				m_JobroomURI = STAGING_JOBROOM_URI
				m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI
				m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI

			End If

		End Sub

		Private Function AddAVAMACreationdvertismentDataToWebservice(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal notify As Boolean, ByVal avamData As SPAVAMCreationResultData) As Boolean
			Dim result As Boolean = True

			Try
				'm_Logger.LogInfo(String.Format("CustomerID: {1} | userid: {2} | vacancyNumber: {3} | notify: {4}{0}adding advertisment data", vbNewLine, customerID, userid, vacancyNumber, notify))

				Dim webService As New InternVacancyService.SPInternVacanciesSoapClient

#If DEBUG Then
				'Return exportResult

#End If

				webService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

				m_Logger.LogDebug(String.Format("loging jobroom data to webserver: {0}", avamData.JobroomID))
				Dim data = webService.AddAVAMCreationDataToDatabase(customerID, userid,
															vacancyNumber, avamData.JobroomID,
															avamData.QueryContent, avamData.ResultContent, m_InitializationData.UserData.UserFullName,
															avamData.ReportingObligation.GetValueOrDefault(False), avamData.ReportingObligationEndDate, notify,
															m_InitializationData.UserData.UserLanguage)
				If Not data Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht in die Datenbank geschrieben werden."))
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("loging jobroom data to webwerver error: CustomerID: {1} | userid: {2} | vacancyNumber: {3} | notify: {4}{0}{5}", vbNewLine, customerID, userid, vacancyNumber, notify, ex.ToString))

				result = False

			End Try

			Return result

		End Function

		Private Function AddAVAMAdvertismentQueryResultDataToWebservice(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal avamData As SPAVAMCreationResultData) As Boolean
			Dim result As Boolean = True

			Try
				'm_Logger.LogInfo(String.Format("CustomerID: {1} | userid: {2} | vacancyNumber: {3}{0} veryfing advertisment data", vbNewLine, customerID, userid, vacancyNumber))

				Dim webService As New InternVacancyService.SPInternVacanciesSoapClient

#If DEBUG Then
				'Return exportResult

#End If

				webService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_VacancyUtilWebServiceUri)

				m_Logger.LogDebug(String.Format("loging jobroom query result to webserver: {0}", avamData.JobroomID))
				Dim data = webService.AddAVAMQueryResultDataToDatabase(customerID, userid,
															vacancyNumber, avamData.JobroomID,
															avamData.QueryContent, avamData.ResultContent, m_InitializationData.UserData.UserFullName,
															avamData.ReportingObligation.GetValueOrDefault(False), avamData.ReportingObligationEndDate,
															m_InitializationData.UserData.UserLanguage)
				If Not data Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht in die Datenbank geschrieben werden."))
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("loging jobroom query result data to webwerver error: CustomerID: {1} | userid: {2} | vacancyNumber: {3}{0}{4}", vbNewLine, customerID, userid, vacancyNumber, ex.ToString))

				result = False

			End Try

			Return result

		End Function

		Private Function BuildJasonstring(ByVal customerID As String, ByVal userID As String, ByVal vacancyData As VacancyMasterData,
																										ByVal vacancyJobCHData As VacancyInseratJobCHData, ByVal vacancyStmpData As VacancyStmpSettingData,
																										ByVal vacancyStmpLanguageData As List(Of VacancyJobCHLanguageData),
																										ByVal employerData As CustomerMasterData,
																										ByVal jobNumber As Integer?, ByVal language As String) As StringBuilder
			Dim msgContent = "building jason string is started..."
			Dim result As New SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
			'Dim htmlToMarkdown As New Html2Markdown.Converter
			Dim userFullname As String = "System"

			Try
				If m_InitializationData.UserData Is Nothing Then
					msgContent = "userData was null"
					Throw New Exception(msgContent)
				End If
				userFullname = m_InitializationData.UserData.UserFullName

				If vacancyData Is Nothing Then
					msgContent = "vacancyData was null"
					Throw New Exception(msgContent)
				End If
				If vacancyJobCHData Is Nothing Then
					msgContent = "vacancyJobCHData was null"
					Throw New Exception(msgContent)
				End If
				If vacancyStmpData Is Nothing Then
					msgContent = "vacancyStmpData was null"
					Throw New Exception(msgContent)
				End If
				If m_InitializationData.MDData Is Nothing Then
					msgContent = "MDData was null"
					Throw New Exception(msgContent)
				End If
				If employerData Is Nothing Then
					msgContent = "employerData was null"
					Throw New Exception(msgContent)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString, "AddAVAMAdvertisementToRAV.GettingObjects")

				Return Nothing

			End Try

			Dim sb As New StringBuilder()
			Dim sw As New StringWriter(sb)

			Try

				Using writer As JsonWriter = New JsonTextWriter(sw)

					writer.WriteStartObject()

					'writer.WritePropertyName("externalUrl")
					'writer.WriteValue("externalUrl_Value")
					'writer.WritePropertyName("externalReference")
					'writer.WriteValue("externalReference_Value")
					writer.WritePropertyName("reportToAvam")
					If vacancyStmpData.ReportToAvam.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If

					If vacancyStmpData.NumberOfJobs.GetValueOrDefault(1) > 1 Then
						writer.WritePropertyName("numberOfJobs")
						writer.WriteValue(vacancyStmpData.NumberOfJobs.GetValueOrDefault(1))
					End If

					' contact
					' Provide an administrative contact (e. g. an HR employee); this contact is used for email notifications concerning the reporting obligation
					writer.WritePropertyName("contact")
					writer.WriteStartObject()

					writer.WritePropertyName("languageIsoCode")
					writer.WriteValue("de")
					writer.WritePropertyName("salutation")
					If m_InitializationData.UserData.UserSalutation = "Herr" Then
						writer.WriteValue("MR")
					Else
						writer.WriteValue("MS")
					End If
					writer.WritePropertyName("firstName")
					writer.WriteValue(m_InitializationData.UserData.UserFName)
					writer.WritePropertyName("lastName")
					writer.WriteValue(m_InitializationData.UserData.UserLName)
					writer.WritePropertyName("phone")
					writer.WriteValue(FormatPhoneNumber(m_InitializationData.UserData.UserMDTelefon))
					writer.WritePropertyName("email")
					writer.WriteValue(m_InitializationData.UserData.UserMDeMail)

					writer.WriteEndObject()


					'' jobDescriptions
					' The text of the job advertisement; may be multilingual
					writer.WritePropertyName("jobDescriptions")
					writer.WriteStartArray()
					writer.WriteStartObject()

					writer.WritePropertyName("languageIsoCode")
					writer.WriteValue("de")
					writer.WritePropertyName("title")
					writer.WriteValue(vacancyData.Bezeichnung)
					writer.WritePropertyName("description")

					Dim descriptionValue As String = String.Empty
					Dim taetigkeit = vacancyJobCHData.Aufgabe
					Dim anforderung = vacancyJobCHData.Anforderung

					If String.IsNullOrWhiteSpace(taetigkeit) AndAlso String.IsNullOrWhiteSpace(anforderung) Then
						taetigkeit = "<b>Tätigkeit:</b><br>Keine Angaben!"
						anforderung = "<b>Anforderung:<b><br>Keine Angaben!"

						descriptionValue = String.Format("{0}<br>{1}", taetigkeit, anforderung)

					Else
						If Not String.IsNullOrWhiteSpace(taetigkeit) Then If Not taetigkeit.Contains("Tätigkeit:") Then taetigkeit = String.Format("<b>Tätigkeit:</b><br>{0}", taetigkeit)
						If Not String.IsNullOrWhiteSpace(anforderung) Then If Not anforderung.Contains("Anforderung:") Then anforderung = String.Format("<b>Anforderung:</b><br>{0}", anforderung)

						descriptionValue = String.Format("{0}<br>{1}", taetigkeit, anforderung)

					End If
					Dim htmlutilities As New HTMLUtiles.Utilities
					writer.WriteValue(htmlutilities.ConvertHtmlToMarkDown(descriptionValue))

					writer.WriteEndObject()
					writer.WriteEndArray()

					'' company
					' The company that handles the recruitment. This information is published.
					writer.WritePropertyName("company")
					writer.WriteStartObject()

					writer.WritePropertyName("name")
					writer.WriteValue(m_InitializationData.MDData.MDName)

					Dim streetName As String = String.Empty
					Dim houseNumber As String = String.Empty

					If Not String.IsNullOrWhiteSpace(m_InitializationData.UserData.UserMDStrasse) Then
						Dim adressString = m_InitializationData.UserData.UserMDStrasse
						Dim i As Integer = 0
						Dim streetData = adressString.Split(" ")

						houseNumber = streetData(streetData.Length - 1)
						For Each itm In streetData
							If i < streetData.Length - 1 Then
								streetName = String.Format("{0}{1}{2}", streetName, If(String.IsNullOrWhiteSpace(streetName), "", " "), itm)
							End If
							i += 1
						Next

					End If

					writer.WritePropertyName("street")
					writer.WriteValue(streetName)

					writer.WritePropertyName("houseNumber")
					writer.WriteValue(houseNumber)

					'writer.WritePropertyName("postOfficeBoxNumber")
					'writer.WriteValue("postOfficeBoxNumber")
					'writer.WritePropertyName("postOfficeBoxPostalCode")
					'writer.WriteValue("postOfficeBoxPostalCode")
					'writer.WritePropertyName("postOfficeBoxCity")
					'writer.WriteValue("postOfficeBoxCity")

					writer.WritePropertyName("postalCode")
					writer.WriteValue(m_InitializationData.UserData.UserMDPLZ)

					writer.WritePropertyName("city")
					writer.WriteValue(m_InitializationData.UserData.UserMDOrt)

					writer.WritePropertyName("countryIsoCode")
					writer.WriteValue("CH")

					writer.WritePropertyName("website")
					writer.WriteValue(m_InitializationData.MDData.MDHomepage)

					writer.WritePropertyName("phone")
					writer.WriteValue(FormatPhoneNumber(m_InitializationData.UserData.UserMDTelefon))

					writer.WritePropertyName("email")
					writer.WriteValue(m_InitializationData.UserData.UserMDeMail)

					writer.WritePropertyName("surrogate")
					If vacancyStmpData.Surrogate.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If

					writer.WriteEndObject()

					'' employer
					' Must be provided if the company handling the recruitment is not the actual employer; will not be published.
					writer.WritePropertyName("employer")
					writer.WriteStartObject()

					writer.WritePropertyName("name")
					writer.WriteValue(employerData.Company1)
					writer.WritePropertyName("postalCode")
					writer.WriteValue(employerData.Postcode)
					writer.WritePropertyName("city")
					writer.WriteValue(employerData.Location)
					writer.WritePropertyName("countryIsoCode")
					writer.WriteValue(employerData.CountryCode)

					writer.WriteEndObject()

					'' employment
					' Employment metadata
					writer.WritePropertyName("employment")
					writer.WriteStartObject()

					'If vacancyStmpData.StartDate.HasValue Then
					'	writer.WritePropertyName("startDate")
					'	writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.StartDate.GetValueOrDefault(Now)))
					'End If
					'If vacancyStmpData.EndDate.HasValue Then
					'	writer.WritePropertyName("endDate")
					'	writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.EndDate.GetValueOrDefault(Now)))
					'End If

					writer.WritePropertyName("shortEmployment")
					If vacancyStmpData.ShortEmployment.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WritePropertyName("immediately")
					If vacancyStmpData.Immediately.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WritePropertyName("permanent")
					If vacancyStmpData.Permanent.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If

					Dim jobProztent = vacancyData.JobProzent
					If String.IsNullOrWhiteSpace(jobProztent) Then
						jobProztent = "100#100"
					End If
					Dim minProzent = jobProztent.Split(CChar("#"))(0)
					Dim maxProzent = jobProztent.Split(CChar("#"))(1)
					minProzent = Math.Max(10, Val(minProzent))
					maxProzent = Math.Max(10, Val(maxProzent))

					writer.WritePropertyName("workloadPercentageMax")
					writer.WriteValue(CStr(maxProzent))
					writer.WritePropertyName("workloadPercentageMin")
					writer.WriteValue(CStr(minProzent))

					'workForms
					If Not vacancyStmpData Is Nothing AndAlso (vacancyStmpData.Sunday_and_Holidays.GetValueOrDefault(False) OrElse vacancyStmpData.Shift_Work.GetValueOrDefault(False) OrElse vacancyStmpData.Night_Work.GetValueOrDefault(False) OrElse
						vacancyStmpData.Home_Work.GetValueOrDefault(False)) Then
						writer.WritePropertyName("workForms")
						writer.WriteStartArray()

						Dim langName As String
						If vacancyStmpData.Sunday_and_Holidays.GetValueOrDefault(False) Then
							langName = "SUNDAY_AND_HOLIDAYS"
							writer.WriteValue(langName)
						End If
						If vacancyStmpData.Shift_Work.GetValueOrDefault(False) Then
							langName = "SHIFT_WORK"
							writer.WriteValue(langName)
						End If
						If vacancyStmpData.Night_Work.GetValueOrDefault(False) Then
							langName = "NIGHT_WORK"
							writer.WriteValue(langName)
						End If
						If vacancyStmpData.Home_Work.GetValueOrDefault(False) Then
							langName = "HOME_WORK"
							writer.WriteValue(langName)
						End If

						writer.WriteEnd()

					End If


					writer.WriteEndObject()

					'' location
					' The work location
					writer.WritePropertyName("location")
					writer.WriteStartObject()

					'writer.WritePropertyName("remarks")
					'writer.WriteValue("remarks")
					writer.WritePropertyName("postalCode")
					writer.WriteValue(vacancyData.JobPLZ)
					writer.WritePropertyName("city")
					writer.WriteValue(vacancyData.JobOrt)
					writer.WritePropertyName("countryIsoCode")
					writer.WriteValue("CH")

					writer.WriteEndObject()

					'' occupation
					' The ad must be coded to an occupation according ot the AVAM occupation list; this determines the reporting obligation.
					writer.WritePropertyName("occupation")
					writer.WriteStartObject()

					writer.WritePropertyName("avamOccupationCode")
					writer.WriteValue(CStr(vacancyData.SBNNumber))

					Dim experienceValue As String = String.Empty
					If vacancyStmpData.Less_One_Year.GetValueOrDefault(False) Then
						experienceValue = "LESS_THAN_1_YEAR"
					ElseIf vacancyStmpData.More_One_Year.GetValueOrDefault(False) Then
						experienceValue = "MORE_THAN_1_YEAR"
					ElseIf vacancyStmpData.More_Three_Years.GetValueOrDefault(False) Then
						experienceValue = "MORE_THAN_3_YEARS"
					End If
					If Not String.IsNullOrWhiteSpace(experienceValue) Then
						writer.WritePropertyName("workExperience")
						writer.WriteValue(experienceValue)
					End If

					If vacancyStmpData.EducationCode.GetValueOrDefault(0) > 0 Then
						writer.WritePropertyName("educationCode")
						writer.WriteValue(vacancyStmpData.EducationCode)
					End If

					writer.WriteEndObject()

					'' languageSkills
					If Not vacancyStmpLanguageData Is Nothing AndAlso vacancyStmpLanguageData.Count > 0 Then
						writer.WritePropertyName("languageSkills")
						writer.WriteStartArray()

						' TODO: if not exists, must be "de" and "NONE"
						For Each lang As VacancyJobCHLanguageData In vacancyStmpLanguageData
							writer.WriteStartObject()
							Dim langName As String = "de"
							Dim langNiveau As String = "BASIC"

							Select Case lang.Bezeichnung_Value.GetValueOrDefault(0)
								Case 1
									langName = "de"
								Case 2
									langName = "fr"
								Case 4
									langName = "it"
								Case 3
									langName = "en"
							End Select

							writer.WritePropertyName("languageIsoCode")
							writer.WriteValue(langName)

							Select Case lang.LanguageNiveau_Value.GetValueOrDefault(0)
								Case 1
									langNiveau = "NONE"
								Case 2
									langNiveau = "BASIC"
								Case 3
									langNiveau = "INTERMEDIATE"
								Case 4
									langNiveau = "PROFICIENT"
							End Select

							writer.WritePropertyName("spokenLevel")
							writer.WriteValue(langNiveau)
							writer.WritePropertyName("writtenLevel")
							writer.WriteValue(langNiveau)

							writer.WriteEndObject()
						Next
						writer.WriteEndArray()

					End If


					'' applyChannel
					' Provide at least one channel for applications.
					writer.WritePropertyName("applyChannel")
					writer.WriteStartObject()

					writer.WritePropertyName("mailAddress")
					writer.WriteValue(m_InitializationData.UserData.UserMDeMail)
					writer.WritePropertyName("phoneNumber")
					writer.WriteValue(FormatPhoneNumber(m_InitializationData.UserData.UserMDTelefon))

					writer.WriteEndObject()

					'' publicContact
					' Provide a public contact if you want to give applicants the opportunity to ask questions about the job.
					writer.WritePropertyName("publicContact")
					writer.WriteStartObject()

					writer.WritePropertyName("salutation")
					If m_InitializationData.UserData.UserSalutation = "Herr" Then
						writer.WriteValue("MR")
					Else
						writer.WriteValue("MS")
					End If
					writer.WritePropertyName("firstName")
					writer.WriteValue(m_InitializationData.UserData.UserFName)
					writer.WritePropertyName("lastName")
					writer.WriteValue(m_InitializationData.UserData.UserLName)
					writer.WritePropertyName("phone")
					writer.WriteValue(FormatPhoneNumber(m_InitializationData.UserData.UserMDTelefon))
					writer.WritePropertyName("email")
					writer.WriteValue(m_InitializationData.UserData.UserMDeMail)

					writer.WriteEndObject()

					'' publication
					' If the ad falls under the reporting obligation, the ad will be restricted for five business days.
					' After that period, the ad will be published In the Job-Room Public area If the publicDisplay flag Is Set, otherwise Not.
					writer.WritePropertyName("publication")
					writer.WriteStartObject()

					writer.WritePropertyName("startDate")
					writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.PublicationStartDate.GetValueOrDefault(Now)))
					'If vacancyStmpData.EndDate.HasValue Then
					'	writer.WritePropertyName("endDate")
					'	writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.EndDate.GetValueOrDefault(Now)))
					'End If
					writer.WritePropertyName("euresDisplay")
					If vacancyStmpData.EuresDisplay.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WritePropertyName("publicDisplay")
					If vacancyStmpData.PublicDisplay.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WriteEndObject()

					writer.WriteEndObject()

				End Using

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString, "AddAVAMAdvertisementToRAV.StringWriter")

				m_TransmittedSTMPid = Nothing
				Return Nothing

			End Try


			Return sb

		End Function

		Private Function BuildCancelationJSONString(ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM) As StringBuilder
			Dim sb As New StringBuilder()
			Dim sw As New StringWriter(sb)

			Try
				Using writer As JsonWriter = New JsonTextWriter(sw)

					writer.WriteStartObject()

					writer.WritePropertyName("code")
					Select Case reasonEnum
						Case AVAMAdvertismentCancelReasonENUM.CHANGE_OR_REPOSE
							writer.WriteValue("CHANGE_OR_REPOSE")
						Case AVAMAdvertismentCancelReasonENUM.NOT_OCCUPIED
							writer.WriteValue("NOT_OCCUPIED")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_AGENCY
							writer.WriteValue("OCCUPIED_AGENCY")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_JOBCENTER
							writer.WriteValue("OCCUPIED_JOBCENTER")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_JOBROOM
							writer.WriteValue("OCCUPIED_JOBROOM")

						Case Else
							writer.WriteValue("OCCUPIED_OTHER")

					End Select

					writer.WriteEndObject()

				End Using

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(ex.ToString, "BuildCancelationJSONString.StringWriter")

				Return Nothing

			End Try


			Return sb

		End Function

#Region "helpers"

		Private Function FormatPhoneNumber(ByRef phoneNumber As String) As String
			Dim result As String = phoneNumber
			Dim existsCountryCode As Boolean = False

			If String.IsNullOrWhiteSpace(phoneNumber) Then Return result
			result = result.Replace(" ", "")
			result = result.Replace("(0)", "")

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

#End Region


	End Class


End Namespace
