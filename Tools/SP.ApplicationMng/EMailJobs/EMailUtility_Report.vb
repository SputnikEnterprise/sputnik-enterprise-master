
Imports DevExpress.XtraSplashScreen

Namespace ChilKatUtility


	Partial Class EMailUtility


		Public Function PrepareChilKatReportLogin() As Boolean
			Dim success As Boolean = True

			'm_Logger.LogDebug("preparing chilkat login!")

			'If imap Is Nothing Then PrepareChilKatLogin()
			'If imap.IsConnected Then imap.Disconnect()

			success = success AndAlso imap.Login(REPORT_SCAN_EMAIL_USER, REPORT_SCAN_EMAIL_PASSWORD)
			If Not success Then

				SplashScreenManager.CloseForm(False)

				m_Logger.LogError(String.Format("unable to login in imap: {0} > {1}", REPORT_SCAN_EMAIL_USER, REPORT_SCAN_EMAIL_PASSWORD))
				m_Logger.LogError(String.Format("unable to login in imap!", imap.LastErrorText))
				'm_UtilityUI.ShowErrorDialog(imap.LastErrorText)
				Return False
			End If
			'm_Logger.LogDebug("finishing chilkat login!")


			Return success

		End Function


	End Class


End Namespace
