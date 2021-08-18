
Imports System.Threading.Tasks
Imports System.Threading

Partial Class frmMain


#Region "customer data"


	''' <summary>
	''' Search for customer geo over web service.
	''' </summary>
	Private Sub RefreshCustomerGeoDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnRefreshCustomerGeoData.Enabled = False

		Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingCustomerGeoDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingCustomerGeoDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs customer geo asynchronous.
	''' </summary>
	Private Function PerformUpdatingCustomerGeoDataWebserviceCallAsync() As Boolean

		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
		Dim result = baseTableData.UpdateCustomerGeoData("CH")

		Return result

	End Function

	''' <summary>
	''' Finish customer geo web service call.
	''' </summary>
	Private Sub FinishUpdatingCustomerGeoDataWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					Dim result = t.Result

					If result Then
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Ihre Geo-Koordinaten wurden erfolgreich aktualisiert."))

					Else
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Geo-Koordinaten wurden nicht erfolgreich aktualisiert."))

					End If

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Geo-Koordinaten wurden nicht erfolgreich aktualisiert."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnRefreshCustomerGeoData.Enabled = True

	End Sub


	''' <summary>
	''' Search for customer country over web service.
	''' </summary>
	Private Sub RefreshCustomerCountryDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnRefreshCustomerCountryCodeData.Enabled = False

		Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingCustomerCountryDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingCustomerCountryDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs customer country asynchronous.
	''' </summary>
	Private Function PerformUpdatingCustomerCountryDataWebserviceCallAsync() As Boolean

		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
		Dim result = baseTableData.UpdateCustomerCountryData()

		Return result

	End Function

	''' <summary>
	''' Finish customer country web service call.
	''' </summary>
	Private Sub FinishUpdatingCustomerCountryDataWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					Dim result = t.Result

					If result Then
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Ihre Kunden-Länder wurden erfolgreich aktualisiert."))

					Else
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Kunden-Länder wurden nicht erfolgreich aktualisiert."))

					End If

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Kunden-Länder wurden nicht erfolgreich aktualisiert."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnRefreshCustomerCountryCodeData.Enabled = True

	End Sub


#End Region


End Class
