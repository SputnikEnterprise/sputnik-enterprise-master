

Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.Internal.Automations.Notifying


Namespace WOSUtility


	Partial Class CustomerExport


#Region "Public Methodes"

		Public Function VerifyVacanciesWithWOS(ByVal customerNumber As Integer, ByVal vacancyNumber As Integer) As Boolean
			Dim result As Boolean = True
			Dim listOfWOSVacanies As New List(Of Integer)
			Dim listOfChangedVacanies As New List(Of Integer)
			Dim listOfWOSVacaniesTObeDeleted As New List(Of Integer)
			Dim notifyObj As New Notifier(m_InitializationData)


			If String.IsNullOrWhiteSpace(m_VacancyWOSID) Then Return False

			' Bimo, they share wosGuid with more Customer_Id
			If m_InitializationData.MDData.MDGuid = "5F703231-6144-47E1-9D37-6EE118D9E79D" Then Return False


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			Dim searchResult = obj.LoadTransferedVacancyDataFromWOS(customerNumber, vacancyNumber)

			If searchResult Is Nothing OrElse searchResult.Count = 0 Then
				m_Logger.LogInfo(String.Format("{0} | no vacancies was founded!", m_InitializationData.MDData.MDGuid))

				Return False
			End If
			m_Logger.LogInfo(String.Format("{0} | count of vacancies: {1}", m_InitializationData.MDData.MDGuid, searchResult.Count))

			For Each vac In searchResult

				Dim vacancyData = m_VacancyDatabaseAccess.LoadVacancyMasterData(m_InitializationData.MDData.MDNr, vac.VakNr)

				If Not vacancyData Is Nothing Then

					If vacancyData.IEExport <> vac.IsOwnePlattformOnline OrElse vacancyData.IsJobsCHOnline <> vac.IsJobsCHOnline OrElse vacancyData.IsOstJobOnline <> vac.IsOstJobOnline Then
						Dim notifyResult As Boolean = True
						Dim logMsg As String = "{1} | internal vacancy will be changed: {2}{0}vacancyData.IEExport: {3}{0}vac.IsOwnePlattformOnline: {4}{0}vac.JobChannelPriority:{5}{0}vacancyData.IsJobsCHOnline: {6}{0}vac.IsJobsCHOnline: {7}{0}vacancyData.IsOstJobOnline: {8}{0}vac.IsOstJobOnline: {9}"
						m_Logger.LogWarning(String.Format(logMsg, vbNewLine, m_InitializationData.MDData.MDGuid, vacancyData.VakNr, vacancyData.IEExport, vac.IsOwnePlattformOnline, vac.JobChannelPriority, vacancyData.IsJobsCHOnline, vac.IsJobsCHOnline, vacancyData.IsOstJobOnline, vac.IsOstJobOnline))

						' set online state as wos
						result = result AndAlso UpdateVacancyOnlineState(vacancyData.VakNr, vacancyData.KDNr, vac.IsOwnePlattformOnline, vac.JobChannelPriority, vac.IsJobsCHOnline, vac.IsOstJobOnline)
#If DEBUG Then
						' set new todo record
						notifyResult = result AndAlso notifyResult AndAlso notifyObj.AddNewNotifierForVacancyChangedOnlineState(m_InitializationData.MDData.MDGuid, vacancyData.VakNr, 1)
#Else
						' set new todo record
						notifyResult = result AndAlso notifyResult AndAlso notifyObj.AddNewNotifierForVacancyChangedOnlineState(m_InitializationData.MDData.MDGuid, vacancyData.VakNr, vacancyData.AdvisorNumber)
#End If
						m_Logger.LogWarning(String.Format("{0} | vacancy data is changed: {1} <<< {2}", m_InitializationData.MDData.MDGuid, vac.VakNr, result))
						If result Then listOfChangedVacanies.Add(vac.VakNr)
					End If
					If result Then listOfWOSVacanies.Add(vac.VakNr)

				Else
					listOfWOSVacaniesTObeDeleted.Add(vac.VakNr)

				End If

				If Not result Then
					m_Logger.LogWarning(String.Format("{0} | vacancy number {1} job will be canceld.", m_InitializationData.MDData.MDGuid, vac.VakNr))
					Return False
				End If
			Next

			Dim msg As String = String.Empty
			If listOfWOSVacaniesTObeDeleted.Count > 0 OrElse listOfChangedVacanies.Count > 0 Then

				If listOfWOSVacaniesTObeDeleted.Count > 0 Then
					msg = "Ihre Online-Vakanzen werden entfernt, da diese lokal gelöscht sind:<br>{0}<br><br>"
					msg = String.Format(msg, String.Join(", ", listOfWOSVacaniesTObeDeleted))

					m_Logger.LogWarning(String.Format("{0} >>> {1} | vacancy numbers to be deleted from WOS: {2}", m_InitializationData.MDData.MDGuid, m_VacancyWOSID, String.Join(", ", listOfWOSVacaniesTObeDeleted)))
					result = result AndAlso listOfWOSVacaniesTObeDeleted.Count > 0 AndAlso DeleteVacancyDataFromWOS(m_InitializationData.MDData.MDGuid, m_VacancyWOSID, listOfWOSVacaniesTObeDeleted)
				End If

				If listOfChangedVacanies.Count > 0 Then
					msg = "Ihre Vakanzen werden online gesetzt:<br>{0}<br>"
					msg = String.Format(msg, String.Join(", ", listOfChangedVacanies))
				End If

				m_UtilityUI.SendMailNotification("Vacancy was set to online", msg, String.Empty, Nothing)
			End If


			If listOfWOSVacanies.Count > 0 Then
				If result Then m_Logger.LogWarning(String.Format("{0} | vacancy numbers to be deactivated: {1}", m_InitializationData.MDData.MDGuid, String.Join(", ", listOfWOSVacanies)))
				result = result AndAlso listOfWOSVacanies.Count > 0 AndAlso SetAllVacanciesAsOffline(listOfWOSVacanies)
			End If


			Return result
		End Function

		Public Function LoadTransferedVacancyDataFromWOS(ByVal customerNumber As Integer, ByVal vacancyNumber As Integer) As IEnumerable(Of WOSVacancySearchResultData)

			Dim success As Boolean = True
			Dim result As List(Of WOSVacancySearchResultData) = Nothing
			Dim customerID = m_InitializationData.MDData.MDGuid


			Try
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

				Dim searchResult = InternService.LoadTransferedVacancyDataFromWOS(customerID, m_VacancyWOSID, customerNumber, vacancyNumber)
				If searchResult Is Nothing Then Return Nothing

				result = New List(Of WOSVacancySearchResultData)
				For Each itm In searchResult
					Dim data = New WOSVacancySearchResultData

					data.Customer_ID = m_InitializationData.MDData.MDGuid
					data.WOSGuid = itm.Customer_ID
					data.ID = itm.RecID
					data.KDNr = itm.KDNr
					data.VakNr = itm.VakNr
					data.VacancyLable = itm.Bezeichnung
					data.IsJobsCHOnline = itm.IsJobsCHOnline
					data.IsOstJobOnline = itm.IsOstJobOnline
					data.IsOwnePlattformOnline = True
					data.CreatedOn = itm.CreatedOn

					result.Add(data)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return result

		End Function


#End Region


#Region "private mehtodes"

		Private Function DeleteVacancyDataFromWOS(ByVal customerID As String, ByVal customerWOSID As String, ByVal vacancyNumbers As List(Of Integer)) As Boolean

			Dim success As Boolean = True
			If vacancyNumbers Is Nothing OrElse vacancyNumbers.Count = 0 Then Return True

#If DEBUG Then
			'm_CustomerWosUtilWebServiceUri = "http://localhost:44721/SPWOSCustomerUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

				For Each number In vacancyNumbers
					success = success AndAlso number > 0 AndAlso InternService.DeleteAssignedVacancyDataFromWOS(customerID, customerWOSID, number)
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return success

		End Function


		Private Function DeleteAssignedVacancyDataFromWOS(ByVal customerID As String, ByVal customerWOSID As String, ByVal vacancyNumber As Integer) As Boolean

			Dim success As Boolean = True

#If DEBUG Then
			'm_CustomerWosUtilWebServiceUri = "http://localhost:44721/SPWOSCustomerUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

				success = success AndAlso InternService.DeleteAssignedVacancyDataFromWOS(customerID, customerWOSID, vacancyNumber)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return success

		End Function

		Private Function UpdateVacancyOnlineState(ByVal vacancyNumber As Integer, ByVal customerNumber As Integer, ByVal ownerOnline As Boolean, ByVal JobChannelPriority As Boolean?, ByVal jobCHOnline As Boolean, ByVal ostJobOnline As Boolean) As Boolean
			Dim success As Boolean = True

			If vacancyNumber = 0 Then Return False

			Try
				success = success AndAlso m_VacancyDatabaseAccess.UpdateVacancyOnlineData(vacancyNumber, customerNumber, ownerOnline, JobChannelPriority, jobCHOnline, ostJobOnline)

			Catch ex As Exception
				m_Logger.LogError(String.Format("MDGuid: {1} | vacancyNumber: {2} | customerNumber: {3} | ownerOnline: {4} | jobCHOnline: {5} | ostJobOnline: {6}{0}{7}",
																				vbNewLine, m_InitializationData.MDData.MDGuid, vacancyNumber, customerNumber, ownerOnline, jobCHOnline, ostJobOnline, ex.ToString))

				success = False
			End Try

			Return success
		End Function

		Private Function SetAllVacanciesAsOffline(ByVal vacancyNumbers As List(Of Integer)) As Boolean
			Dim success As Boolean = True

			If vacancyNumbers Is Nothing OrElse vacancyNumbers.Count = 0 Then Return True

			Try
				success = success AndAlso m_VacancyDatabaseAccess.UpdateOtherVacanciesAsOffline(vacancyNumbers)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			Return success
		End Function

#End Region


	End Class


End Namespace