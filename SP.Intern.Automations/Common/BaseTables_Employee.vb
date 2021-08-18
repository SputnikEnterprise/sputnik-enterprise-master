
Imports System.ComponentModel

Namespace BaseTable

	Partial Class SPSBaseTables


		Public Function PerformTaxInfoDataOverWebService(ByVal canton As String, ByVal year As Integer, ByVal language As String) As TaxData
			Dim resultData = New TaxData

			Try
				Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
				webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

				Dim result = webservice.LoadTaxInfoData(m_InitializationData.MDData.MDGuid, canton, year)
				If Not result Is Nothing Then
					Dim taxItem As New List(Of TaxDataItem)

					For Each itm In result.Data
						Dim data As New TaxDataItem

						data.Gruppe = itm.Gruppe
						data.Kanton = itm.Kanton
						data.Kinder = itm.Kinder
						data.Kirchensteuer = itm.Kirchensteuer
						data.Gruppe = itm.Gruppe

						taxItem.Add(data)

					Next

					resultData.Data = taxItem
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


			Return resultData
		End Function

		Public Function PerformTaxCodeDataOverWebService(ByVal language As String) As BindingList(Of TaxCodeData)
			Dim listDataSource As BindingList(Of TaxCodeData) = New BindingList(Of TaxCodeData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadTaxCodeData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("tax codes was NOT passed: language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New TaxCodeData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try


			Return listDataSource

		End Function

		Public Function PerformTaxChurchCodeDataOverWebService(ByVal language As String) As BindingList(Of TaxChurchCodeData)
			Dim listDataSource As BindingList(Of TaxChurchCodeData) = New BindingList(Of TaxChurchCodeData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadTaxChurchCodeData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("tax church codes was NOT passed: language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New TaxChurchCodeData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try


			Return listDataSource

		End Function

		Public Function PerformTaxNumberOfChildernDataOverWebService(ByVal language As String, ByVal canton As String, ByVal code As String, ByVal church As String) As BindingList(Of Integer)
			Dim listDataSource As BindingList(Of Integer) = New BindingList(Of Integer)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadTaxNumberOfChilderData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage, canton, code, church)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("tax children number was NOT passed: language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult
					listDataSource.Add(result)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try


			Return listDataSource

		End Function

		Public Function PerformCommunityDataOverWebService(ByVal canton As String, ByVal language As String) As BindingList(Of CommunityData)
			Dim listDataSource As BindingList(Of CommunityData) = New BindingList(Of CommunityData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadCommunityData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, canton, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("communities could not be downloaded: canton: {0} >>> language: {1}", canton, language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New CommunityData With {.Canton = result.Canton,
						.BFSNumber = result.BFSNumber, .Translated_Value = result.Translated_Value}

					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try


			Return listDataSource

		End Function

		Public Function PerformEmploymentTypeDataOverWebService(ByVal language As String) As BindingList(Of EmploymentTypeData)
			Dim listDataSource As BindingList(Of EmploymentTypeData) = New BindingList(Of EmploymentTypeData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadEmploymentType(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("EmploymentType was NOT passed: language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New EmploymentTypeData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try


			Return listDataSource

		End Function

		Public Function PerformOtherEmploymentTypeDataOverWebService(ByVal language As String) As BindingList(Of EmploymentTypeData)
			Dim listDataSource As BindingList(Of EmploymentTypeData) = New BindingList(Of EmploymentTypeData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadOtherEmploymentType(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("EmploymentType was NOT passed: language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New EmploymentTypeData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Weitere Beschäftigungsarten konnten nicht geladen werden."))
			End Try


			Return listDataSource

		End Function

		Public Function PerformTypeOfStayDataOverWebService(ByVal language As String) As BindingList(Of TypeOfStayData)
			Dim listDataSource As BindingList(Of TypeOfStayData) = New BindingList(Of TypeOfStayData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadTypeOfStay(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("TypeofStay was NOT passed: language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New TypeOfStayData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Aufenthaltsarten konnten nicht geladen werden."))
			End Try


			Return listDataSource

		End Function

		Public Function PerformPermissionDataOverWebService(ByVal language As String) As BindingList(Of PermissionData)
			Dim listDataSource As BindingList(Of PermissionData) = New BindingList(Of PermissionData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadPermission(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("permissiondata was NOT passed. language: {0}", language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New PermissionData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value, .Code = result.Code, .RecNr = result.RecNr}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewilligungen konnten nicht geladen werden."))
			End Try


			Return listDataSource

		End Function

		Public Function PerformForeignCategoryDataOverWebService(ByVal code As String, ByVal language As String) As BindingList(Of PermissionData)
			Dim listDataSource As BindingList(Of PermissionData) = New BindingList(Of PermissionData)

			If String.IsNullOrWhiteSpace(language) Then language = "DE"

			Dim webservice As New EmployeeTaxInfoWebService.SPEmployeeTaxInfoServiceSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_TaxPermissionUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult = webservice.LoadForeignCategory(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, code, m_InitializationData.UserData.UserLanguage)
				If searchResult Is Nothing Then
					m_Logger.LogWarning(String.Format("ForeignCategory was NOT passed. code: {0} | language: {1}", code, language))

					Return Nothing
				End If

				For Each result In searchResult

					Dim viewData = New PermissionData With {.ID = result.ID, .Rec_Value = result.Rec_Value, .Translated_Value = result.Translated_Value, .Code = result.Code, .RecNr = result.RecNr}


					listDataSource.Add(viewData)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewilligungskategorien konnten nicht geladen werden."))
			End Try


			Return listDataSource

		End Function



	End Class

End Namespace
