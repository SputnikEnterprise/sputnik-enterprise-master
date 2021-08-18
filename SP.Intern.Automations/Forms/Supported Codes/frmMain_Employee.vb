
Imports System.Threading.Tasks
Imports System.Threading

Partial Class frmMain

	''' <summary>
	''' Search for employee geo over web service.
	''' </summary>
	Private Sub RefreshEmployeeGeoDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnRefreshEmployeeGeoData.Enabled = False

		Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingEmployeeGeoDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingEmployeeGeoDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs employee geo asynchronous.
	''' </summary>
	Private Function PerformUpdatingEmployeeGeoDataWebserviceCallAsync() As Boolean

		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
		Dim result = baseTableData.UpdateEmployeeGeoData("CH")

		Return result

	End Function

	''' <summary>
	''' Finish employee geo web service call.
	''' </summary>
	Private Sub FinishUpdatingEmployeeGeoDataWebserviceCallTask(ByVal t As Task(Of Boolean))

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

		btnRefreshEmployeeGeoData.Enabled = True

	End Sub


#Region "employee countries"

	''' <summary>
	''' Search for employee country over web service.
	''' </summary>
	Private Sub RefreshEmployeeCountryDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnRefreshEmployeeCountryCodeData.Enabled = False

		Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingEmployeeCountryDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingEmployeeCountryDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs employee country asynchronous.
	''' </summary>
	Private Function PerformUpdatingEmployeeCountryDataWebserviceCallAsync() As Boolean

		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
		Dim result = baseTableData.UpdateEmployeeCountryData()

		Return result

	End Function

	''' <summary>
	''' Finish employee country web service call.
	''' </summary>
	Private Sub FinishUpdatingEmployeeCountryDataWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					Dim result = t.Result

					If result Then
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Ihre Koordinaten-Länder wurden erfolgreich aktualisiert."))

					Else
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Koordinaten-Länder wurden nicht erfolgreich aktualisiert."))

					End If

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Kandidaten-Länder wurden nicht erfolgreich aktualisiert."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnRefreshEmployeeCountryCodeData.Enabled = True

	End Sub


#End Region



#Region "employee tax community"

	''' <summary>
	''' Search for employee geo over web service.
	''' </summary>
	Private Sub RefreshEmployeeTaxCommunityDataViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnRefreshEmployeeGeoData.Enabled = False

		Task(Of Boolean).Factory.StartNew(Function() PerformUpdatingEmployeeTaxCommunityDataWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishUpdatingEmployeeTaxCommunityDataWebserviceCallTask(t), CancellationToken.None,
																																								TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs employee geo asynchronous.
	''' </summary>
	Private Function PerformUpdatingEmployeeTaxCommunityDataWebserviceCallAsync() As Boolean

		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)
		Dim result = baseTableData.UpdateEmployeeTaxCommunityData()

		Return result

	End Function

	''' <summary>
	''' Finish employee geo web service call.
	''' </summary>
	Private Sub FinishUpdatingEmployeeTaxCommunityDataWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try
			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True
					Dim result = t.Result

					If result Then
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Ihre Gemeinde-Daten wurden erfolgreich aktualisiert."))

					Else
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Gemeinde-Daten wurden nicht erfolgreich aktualisiert."))

					End If

					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Gemeinde-Daten wurden nicht erfolgreich aktualisiert."))

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		btnRefreshEmployeeGeoData.Enabled = True

	End Sub

#End Region

End Class
