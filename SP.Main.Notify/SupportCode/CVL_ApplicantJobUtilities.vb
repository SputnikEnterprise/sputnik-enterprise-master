
Imports System.ComponentModel
Imports SP.Main.Notify.SPApplicationWebService


Partial Class ApplicantJobUtilities

	Private m_PersonalCommonData As CVLPersonalDataDTO

	Private Function PerformAssignedPersonalCommonWebservice(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
		Dim result As Boolean = True


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

		' Read data over webservice
		Dim searchResult = webservice.LoadAssignedCVLPersonalViewData(CustomerID, profileID, personalID)
		If searchResult Is Nothing Then
			m_Logger.LogError(String.Format("LoadAssignedCVLPersonalViewData: could not be loaded from webservice! {0} | {1} | {2}", CustomerID, profileID, personalID))

			Return False
		End If

		m_PersonalCommonData = New CVLPersonalDataDTO With {
					.PersonalID = searchResult.PersonalID,
					.DateOfBirth = searchResult.DateOfBirth,
					.DateOfBirthPlace = searchResult.DateOfBirthPlace,
					.FirstName = searchResult.FirstName,
					.Gender = searchResult.Gender,
					.GenderLabel = searchResult.GenderLabel,
					.IsCed = searchResult.IsCed,
					.IsCedLable = searchResult.IsCedLable,
					.LastName = searchResult.LastName,
					.Nationality = searchResult.Nationality,
					.NationalityLable = searchResult.NationalityLable,
					.CivilState = searchResult.CivilState,
					.CivilStateLable = searchResult.CivilStateLable,
					.PersonalPhoto = searchResult.PersonalPhoto,
					.PersonalTitle = searchResult.PersonalTitle,
					.PersonalEMail = searchResult.PersonalEMail,
					.PersonalHomepage = searchResult.PersonalHomepage,
					.PersonalTelephone = searchResult.PersonalTelephone,
					.PersonalTelefax = searchResult.PersonalTelefax,
					.PersonalAddress = searchResult.PersonalAddress
				}


		Return Not (m_PersonalCommonData Is Nothing)

	End Function

	''' <summary>
	'''  Performs loading cvl document data.
	''' </summary>
	Private Function PerformCVLDocumentWebservice(ByVal profileID As Integer?) As BindingList(Of ApplicantDocumentViewData)
		Dim listDataSource As BindingList(Of ApplicantDocumentViewData) = New BindingList(Of ApplicantDocumentViewData)


		Dim webservice As New SPApplicationWebService.SPApplicationSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)
		m_Logger.LogDebug(String.Format("cvl documents (tbl_CVLDocuments) are searching {0} | ProfileID: {1}...", CustomerID, profileID))

		Try
			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedApplicantDocumentFromCVLData(CustomerID, profileID)

			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("Documents could not be loaded from webservice! {0} | {1}", CustomerID, profileID))

				Return listDataSource
			End If

			For Each result In searchResult

				Dim viewData = New ApplicantDocumentViewData With {
						.ID = result.ID,
						.Content = result.DocBinary,
						.DocClass = result.DocClass,
						.FileExtension = result.FileType,
						.Hashvalue = result.FileHashvalue,
						.Title = result.DocClass,
						.Pages = result.Pages,
						.DocXML = result.DocXML,
						.FileSize = result.DocSize,
						.PlainText = result.Plaintext
					}

				listDataSource.Add(viewData)

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


		Return listDataSource

	End Function


End Class
