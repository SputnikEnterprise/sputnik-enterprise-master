Imports System.ComponentModel
Imports System.IO

Namespace BaseTable


	Partial Class SPSBaseTables

		Private Const SERVICENAME As String = "DOWNLOADSETTINGFILE"

		Private m_AllowedServicetoUse As Boolean

		Public Function PerformDownloadingCommonSettingFilesOverWebService() As Boolean
			Dim resultData As Boolean = True

			Try
				m_AllowedServicetoUse = IsCustomerServiceAllowed(SERVICENAME)
				If Not m_AllowedServicetoUse Then Throw New Exception(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

				Dim result = webservice.LoadMainViewSettingFile(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid, "MainView.xml")
				If Not result Is Nothing Then

					Dim existsFile = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Skins", "MainView.xml")
					Dim newFileDatetime = CType(String.Format("{0}", result.FileDate), DateTime)
					Dim existsFileDatetime = CType(String.Format("{0}", File.GetLastWriteTime(existsFile).ToLocalTime), DateTime)

					If existsFileDatetime <> newFileDatetime Then
						If m_Utility.WriteFileBytes(existsFile, result.FileContent) Then
							resultData = resultData AndAlso ChangeFileAttribute(existsFile, result.FileDate)
						End If
					End If

				End If

				Dim resultTranslationFile = webservice.LoadTranslateionSettingFile(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid, "TranslationData.xml")
				If Not resultTranslationFile Is Nothing Then

					Dim ServerPath As String = String.Empty
					If m_InitializationData.MDData.MDRootPath.EndsWith("\") Then ServerPath = Directory.GetParent(m_InitializationData.MDData.MDRootPath.Remove(m_InitializationData.MDData.MDRootPath.Length - 1)).FullName

					Dim existsFile = Path.Combine(ServerPath, "Bin", "TranslationData.xml")
					Dim newFileDatetime = CType(String.Format("{0}", resultTranslationFile.FileDate), DateTime)
					Dim existsFileDatetime = CType(String.Format("{0}", File.GetLastWriteTime(existsFile).ToLocalTime), DateTime)

					If existsFileDatetime <> newFileDatetime Then
						If m_Utility.WriteFileBytes(existsFile, resultTranslationFile.FileContent) Then
							resultData = resultData AndAlso ChangeFileAttribute(existsFile, resultTranslationFile.FileDate)
						End If
					End If

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				resultData = False
			End Try


			Return resultData
		End Function

		Public Function PerformDownloadingCommonTemplateFilesOverWebService() As Boolean
			Dim resultData As Boolean = True

			Try
				m_AllowedServicetoUse = IsCustomerServiceAllowed(SERVICENAME)
				If Not m_AllowedServicetoUse Then Throw New Exception(String.Format("{0} modul is not allowed to use!!!", SERVICENAME))

				If Not Directory.Exists(Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice")) Then Directory.CreateDirectory(Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice"))
				If Not Directory.Exists(Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Payroll")) Then Directory.CreateDirectory(Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Payroll"))

				Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationUtilWebServiceUri)

				Dim result = webservice.LoadMailTemplateFile(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid, "invoice.template")
				If Not result Is Nothing Then

					Dim existsFile = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice", "MailTempl_Invoice_01.docx")
					Dim newFileDatetime = CType(String.Format("{0}", result.FileDate), DateTime)
					Dim existsFileDatetime = CType(String.Format("{0}", File.GetLastWriteTime(existsFile).ToLocalTime), DateTime)

					If existsFileDatetime <> newFileDatetime Then
						If m_Utility.WriteFileBytes(existsFile, result.FileContent) Then
							resultData = resultData AndAlso ChangeFileAttribute(existsFile, result.FileDate)
						End If
					End If

				End If

				result = webservice.LoadMailTemplateFile(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid, "moreinvoices.template")
				If Not result Is Nothing Then

					Dim existsFile = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Invoice", "MailTempl_MoreInvoices_01.docx")
					Dim newFileDatetime = CType(String.Format("{0}", result.FileDate), DateTime)
					Dim existsFileDatetime = CType(String.Format("{0}", File.GetLastWriteTime(existsFile).ToLocalTime), DateTime)

					If existsFileDatetime <> newFileDatetime Then
						If m_Utility.WriteFileBytes(existsFile, result.FileContent) Then
							resultData = resultData AndAlso ChangeFileAttribute(existsFile, result.FileDate)
						End If
					End If

				End If

				result = webservice.LoadMailTemplateFile(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserGuid, "morepayrolls.template")
				If Not result Is Nothing Then

					Dim existsFile = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "Mail", "Payroll", "MailTempl_MorePayrolls_01.docx")
					Dim newFileDatetime = CType(String.Format("{0}", result.FileDate), DateTime)
					Dim existsFileDatetime = CType(String.Format("{0}", File.GetLastWriteTime(existsFile).ToLocalTime), DateTime)

					If existsFileDatetime <> newFileDatetime Then
						If m_Utility.WriteFileBytes(existsFile, result.FileContent) Then
							resultData = resultData AndAlso ChangeFileAttribute(existsFile, result.FileDate)
						End If
					End If

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				resultData = False
			End Try


			Return resultData
		End Function

		Private Function ChangeFileAttribute(ByVal tempFileName As String, ByVal fileDate As DateTime?) As Boolean
			Dim success As Boolean = True

			Try

				Dim dtCreation As DateTime = CType(String.Format("{0}", fileDate), DateTime)
				dtCreation = New DateTime(dtCreation.Year, dtCreation.Month, dtCreation.Day, dtCreation.Hour, dtCreation.Minute, dtCreation.Second, DateTimeKind.Local)

				File.SetCreationTime(tempFileName, dtCreation)
				File.SetLastWriteTime(tempFileName, dtCreation)
				File.SetLastAccessTime(tempFileName, dtCreation)

			Catch ex As Exception
				Return False

			End Try


			Return success

		End Function

		Private Function IsCustomerServiceAllowed(ByVal serviceName As String) As Boolean
			Dim providerObj As New ProviderData(m_InitializationData)
			Dim result = providerObj.IsCustomerAllowedToUseServiceData(m_InitializationData.MDData.MDGuid, serviceName)

			Return result

		End Function

	End Class

End Namespace