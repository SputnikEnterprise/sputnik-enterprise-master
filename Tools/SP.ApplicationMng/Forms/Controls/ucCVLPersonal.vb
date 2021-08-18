
Imports SP.DatabaseAccess.CVLizer.DataObjects
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports System.IO
Imports SP.ApplicationMng.SPApplicationWebService
Imports SP.Main.Notify.SPApplicationWebService

Namespace CVLizer.UI


	Public Class ucCVLPersonal


#Region "Private Consts"

		'Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

#End Region


#Region "private fields"

		'''' <summary>
		'''' The Initialization data.
		'''' </summary>
		'Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		'''' <summary>
		'''' Boolean flag indicating if form is initializing.
		'''' </summary>
		'Protected m_SuppressUIEvents As Boolean = False

		'''' <summary>
		'''' The logger.
		'''' </summary>
		'Private Shared m_Logger As ILogger = New Logger()

		'''' <summary>
		'''' The translation value helper.
		'''' </summary>
		'Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


		'''' <summary>
		'''' The common data access object.
		'''' </summary>
		'Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		'Private m_AppDatabaseAccess As IAppDatabaseAccess

		'''' <summary>
		'''' The cv data access object.
		'''' </summary>
		'Private m_AppCVDatabaseAccess As IAppCvDatabaseAccess

		'''' <summary>
		'''' The cv data access object.
		'''' </summary>
		'Private m_CVLDatabaseAccess As ICVLizerDatabaseAccess


		Private m_applicantDb As String
		Private m_customerID As String

		Private m_CVLizerXMLData As CVLizerXMLData
		Private m_CurrentFileExtension As String


		Private m_EducationPhaseData As IEnumerable(Of EducationPhaseViewData)
		Private m_CVLProfileID As Integer
		Private m_PersonalID As Integer
		Private m_PhaseID As Integer

		'''' <summary>
		'''' Service Uri of Sputnik notification util webservice.
		'''' </summary>
		'Private m_ApplicationUtilWebServiceUri As String

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' List of user controls.
		''' </summary>
		Private m_connectionString As String

		'''' <summary>
		'''' UI Utility functions.
		'''' </summary>
		'Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath

		Private m_ProfileData As CVLizerProfileDataDTO
		Private m_PersonalCommonData As CVLPersonalDataDTO
		Private m_PersonalAddressData As CVLAddressDTO


#End Region


#Region "constructor"

		Public Sub New() 'ByVal _setting As InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			'm_InitializationData = _setting
			m_mandant = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			'm_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)


			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			Reset()

#If DEBUG Then
			'm_customerID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If


		End Sub


#End Region


#Region "private property"


#End Region


#Region "public Methodes"

		Public Function LoadAssignedPersonalData(ByVal customerID As String, ByVal cvlProfileID As Integer?, ByVal cvlPersonalID As Integer?) As Boolean
			Dim result As Boolean = True

			m_customerID = customerID

#If DEBUG Then
			'm_customerID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If

			m_CVLProfileID = cvlProfileID.GetValueOrDefault(0)
			m_PersonalID = cvlPersonalID.GetValueOrDefault(0)

			result = result AndAlso PerformAssignedProfileWebservice(cvlProfileID)
			result = result AndAlso DisplayAssignedPersonalData(cvlPersonalID.GetValueOrDefault(0))

			Return True

		End Function


#End Region


		Public Overrides Sub Reset()

			ResetPersonalInformationFields()

		End Sub

		Private Sub ResetPersonalInformationFields()

			peProfilePicture.Image = Nothing

			txtApplicantLastName.EditValue = Nothing
			txtApplicantFirstname.EditValue = Nothing
			cboGender.EditValue = Nothing
			txtApplicantStreet.EditValue = Nothing
			txtApplicantPostofficeBox.EditValue = Nothing
			txtApplicantPostcode.EditValue = Nothing
			txtApplicantLocation.EditValue = Nothing
			txtApplicantCountry.EditValue = Nothing
			txtApplicantNationality.EditValue = Nothing

			lstEMails.Items.Clear()
			lstHomepages.Items.Clear()
			lstTelefon.Items.Clear()
			lstTelefax.Items.Clear()
			lstTiltles.Items.Clear()

			txtApplicantBirthDate.EditValue = Nothing
			lblAge.Text = "?"
			txtApplicantCivilstate.EditValue = Nothing

			txtApplicantCreatedOn.EditValue = Nothing

		End Sub


		Private Function DisplayAssignedPersonalData(ByVal personalID As Integer) As Boolean
			Dim success As Boolean = True

			ResetPersonalInformationFields()
			Try

				success = success AndAlso LoadAssignedPersonalViewData(m_CVLProfileID, personalID)
				success = success AndAlso DisplayAssignedPersonalPhotoData()
				If Not success Then Return success

				m_PersonalAddressData = m_PersonalCommonData.PersonalAddress
				txtApplicantLastName.EditValue = m_PersonalCommonData.LastName
				txtApplicantFirstname.EditValue = m_PersonalCommonData.FirstName
				cboGender.EditValue = m_PersonalCommonData.GenderLabel
				If Not m_PersonalAddressData Is Nothing Then
					txtApplicantStreet.EditValue = m_PersonalAddressData.Street
					txtApplicantPostcode.EditValue = m_PersonalAddressData.Postcode
					txtApplicantLocation.EditValue = m_PersonalAddressData.City
					txtApplicantCountry.EditValue = m_PersonalAddressData.CountryLable
				End If

				txtApplicantNationality.EditValue = m_PersonalCommonData.NationalityLable
				txtApplicantBirthDate.EditValue = String.Format("{0:d}", m_PersonalCommonData.DateOfBirth)

				If Not m_PersonalCommonData.DateOfBirth Is Nothing Then
					Dim birthDate = m_PersonalCommonData.DateOfBirth.GetValueOrDefault(Now)
					Dim years As Integer = DateTime.Now.Year - birthDate.Year

					birthDate = birthDate.AddYears(years)
					If (DateTime.Today.CompareTo(birthDate) < 0) Then years = years - 1

					lblAge.Text = String.Format("{0}", years)

				Else
					lblAge.Text = "?"

				End If
				txtApplicantCivilstate.EditValue = m_PersonalCommonData.CivilStateLable


				'Dim viewdata = New List(Of PersonalListViewData)
				Dim viewdata = m_PersonalCommonData.PersonalTitle ' m_CVLDatabaseAccess.LoadAssignedCVLPersonalTitleViewData(m_CVLProfileID, personalID)
				If Not viewdata Is Nothing AndAlso viewdata.Count > 0 Then
					For Each itm In viewdata
						lstTiltles.Items.Add(itm.Lable)
					Next
				End If

				'viewdata = m_CVLDatabaseAccess.LoadAssignedCVLPersonalEMailViewData(m_CVLProfileID, personalID)
				viewdata = m_PersonalCommonData.PersonalEMail
				If Not viewdata Is Nothing AndAlso viewdata.Count > 0 Then
					For Each itm In viewdata
						lstEMails.Items.Add(itm.Lable)
					Next
				End If

				'viewdata = m_CVLDatabaseAccess.LoadAssignedCVLPersonalHomepageViewData(m_CVLProfileID, personalID)
				viewdata = m_PersonalCommonData.PersonalHomepage
				If Not viewdata Is Nothing AndAlso viewdata.Count > 0 Then
					For Each itm In viewdata
						lstHomepages.Items.Add(itm.Lable)
					Next
				End If

				'viewdata = m_CVLDatabaseAccess.LoadAssignedCVLPersonalTelefonNumberViewData(m_CVLProfileID, personalID)
				viewdata = m_PersonalCommonData.PersonalTelephone
				If Not viewdata Is Nothing AndAlso viewdata.Count > 0 Then
					For Each itm In viewdata
						lstTelefon.Items.Add(itm.Lable)
					Next
				End If

				'viewdata = m_CVLDatabaseAccess.LoadAssignedCVLPersonalTelefaxNumberViewData(m_CVLProfileID, personalID)
				viewdata = m_PersonalCommonData.PersonalTelefax
				If Not viewdata Is Nothing AndAlso viewdata.Count > 0 Then
					For Each itm In viewdata
						lstTelefax.Items.Add(itm.Lable)
					Next
				End If

				txtApplicantCreatedOn.EditValue = m_ProfileData.CreatedOn

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return False

			End Try

			Return success

		End Function

		''' <summary>
		'''  Performs loading cvl profile data.
		''' </summary>
		Private Function PerformAssignedProfileWebservice(ByVal profileID As Integer?) As Boolean
			Dim result As Boolean = True

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", m_customerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedCVLProfileViewData(m_customerID, profileID)

			m_ProfileData = New CVLizerProfileDataDTO With {
					.ProfileID = searchResult.ProfileID,
					.Customer_ID = searchResult.Customer_ID,
					.PersonalID = searchResult.PersonalID,
					.WorkID = searchResult.WorkID,
					.EducationID = searchResult.EducationID,
					.AdditionalID = searchResult.AdditionalID,
					.ObjectiveID = searchResult.ObjectiveID,
					.CreatedOn = searchResult.CreatedOn,
					.CreatedFrom = searchResult.CreatedFrom,
					.FirstName = searchResult.FirstName,
					.LastName = searchResult.LastName
				}


			Return Not (m_ProfileData Is Nothing)

		End Function

		'Private Function LoadAssignedProfileData(ByVal profileID As Integer?) As Boolean
		'	Dim success As Boolean = True

		'	m_ProfileData = m_CVLDatabaseAccess.LoadAssignedCVLProfileViewData(m_customerID, profileID)


		'	Return success AndAlso Not m_ProfileData Is Nothing

		'End Function

		Private Function LoadAssignedPersonalViewData(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
			Dim success As Boolean = True

			Try

				success = success AndAlso PerformAssignedPersonalCommonWebservice(profileID, personalID)
				'success = success AndAlso PerformAssignedPersonalAddressWebservice(profileID, personalID)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				Return False
			End Try


			Return success

		End Function

		''' <summary>
		'''  Performs loading cvl personal data.
		''' </summary>
		Private Function PerformAssignedPersonalCommonWebservice(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
			Dim result As Boolean = True

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", m_customerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			Dim searchResult = webservice.LoadAssignedCVLPersonalViewData(m_customerID, profileID, personalID)
			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("LoadAssignedCVLPersonalViewData: could not be loaded from webservice! {0} | {1} | {2}", m_customerID, profileID, personalID))

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


		'Private Function LoadAssignedPersonalCommonViewData(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
		'	Dim success As Boolean = True

		'	m_PersonalCommonData = m_CVLDatabaseAccess.LoadAssignedCVLPersonalViewData(profileID, personalID)


		'	Return success AndAlso Not m_PersonalCommonData Is Nothing

		'End Function

		'Private Function LoadAssignedPersonalAddressViewData(ByVal profileID As Integer?, ByVal personalID As Integer?) As Boolean
		'	Dim success As Boolean = True

		'	m_PersonalAddressData = m_CVLDatabaseAccess.LoadAssignedCVLPersonalAddressViewData(profileID, personalID)


		'	Return success AndAlso Not m_PersonalAddressData Is Nothing

		'End Function

		Private Function DisplayAssignedPersonalPhotoData() As Boolean
			Dim success As Boolean = True

			If m_PersonalCommonData.PersonalPhoto Is Nothing Then Return success
			peProfilePicture.Image = Image.FromStream(New MemoryStream(m_PersonalCommonData.PersonalPhoto))

			Return success

		End Function


#Region "Helpers Class"

		Private Class PersonalLocalViewData

			Public Property PersonalID As Integer?
			Public Property FirstName As String
			Public Property LastName As String
			Public Property Gender As String
			Public Property GenderLabel As String
			Public Property IsCed As String
			Public Property IsCedLable As String
			Public Property DateOfBirth As Date?
			Public Property DateOfBirthPlace As String
			Public Property Nationality As String
			Public Property NationalityLable As String
			Public Property CivilState As String
			Public Property CivilStateLable As String

			Public Property PersonalPhoto As Byte()
			Public Property ISValid As Boolean?

			Public Property PersonalTitle As List(Of PersonalLocalListViewData)
			Public Property PersonalEMail As List(Of PersonalLocalListViewData)
			Public Property PersonalHomepage As List(Of PersonalLocalListViewData)
			Public Property PersonalTelephone As List(Of PersonalLocalListViewData)
			Public Property PersonalTelefax As List(Of PersonalLocalListViewData)

			Public Property PersonalAddress As AddressLocalViewData

			Public ReadOnly Property Fullname As String
				Get
					Return String.Format("{0} {1}", FirstName, LastName)
				End Get
			End Property


			Public ReadOnly Property DateOfBirthYear As Integer?
				Get
					Return Year(DateOfBirth)
				End Get
			End Property


		End Class

		Private Class AddressLocalViewData
			Public Property ID As Integer?
			Public Property Street As String
			Public Property Postcode As String
			Public Property City As String
			Public Property Country As String
			Public Property CountryLable As String
			Public Property State As String

			Public ReadOnly Property AddressLable As String
				Get
					Return String.Format("{0} {1}-{2} {3}", Street, Country, Postcode, City)
				End Get
			End Property

		End Class

		Private Class PersonalLocalListViewData
			Public Property ID As Integer?
			Public Property PersonalID As Integer?
			Public Property Lable As String

		End Class

#End Region

	End Class

End Namespace
