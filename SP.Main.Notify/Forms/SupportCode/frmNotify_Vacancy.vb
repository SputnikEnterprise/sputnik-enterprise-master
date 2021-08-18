

Imports SP.Main.Notify.WOSDataTransfer

Namespace UI


	Partial Class frmNotify

		Private Function LoadAutomatedVacancyJobData() As Boolean
			Dim success As Boolean = True
			grpSearch.Text = String.Format("Last search: {0}", Now)

			If m_MasterMandantData Is Nothing OrElse m_MasterMandantData.Count = 0 Then Return success

			Try

				For Each md In m_MasterMandantData
					m_CurrentMDNumber = md.MDNr
					m_CurrentMDString = md.DbConnectionstr
					m_CurrentMDGuid = md.Customer_id
					If txt_MDGuidForVacancyCheck.EditValue <> m_CurrentMDGuid Then Continue For
					Dim initSuccess As Boolean = InitialAssignedMandantData()

					If initSuccess Then
						If Not String.IsNullOrWhiteSpace(m_OriginalCustomerID) Then
							m_Logger.LogDebug(String.Format("LoadAutomatedVacancyJobData: m_CurrentMDNumber {0} is looking for vacancy...", m_CurrentMDNumber))

							PerformVacancyListWebserviceCallAsync()

						Else
							m_Logger.LogDebug(String.Format("LoadAutomatedVacancyJobData: m_CurrentMDNumber {0} ===>>> m_OriginalCustomerID is empty!", m_CurrentMDNumber))

						End If
					End If

				Next

				If Not m_currentImportResultData Is Nothing AndAlso m_currentImportResultData.Count > 0 Then XtraTabControl1.SelectedTabPage = xtabDocScan

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			Finally

			End Try

			Return success

		End Function

		Private Function PerformVacancyListWebserviceCallAsync() As Boolean
			Dim success As Boolean = True
			m_WosObj = New SendScanJobTOWOS(m_InitializationData)

			success = success AndAlso m_WosObj.VerifyVacanciesWithWOS

		End Function


	End Class


End Namespace
