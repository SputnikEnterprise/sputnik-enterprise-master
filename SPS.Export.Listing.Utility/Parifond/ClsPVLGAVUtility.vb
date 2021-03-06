
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraGrid
Imports SPS.Export.Listing.Utility.SPPVLGAVUtilWebService

Namespace PVLGAV


	Public Class PVLGAVUtility



#Region "Private Consts"

		Private Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
		Public Const DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPPVLGAVUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"

#End Region


#Region "Privte Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The employee data access object.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' Service Uri of Sputnik bank util webservice.
		''' </summary>
		Private m_SPPVLGAVUtilServiceUrl As String

		'''<summary>
		'''Service Uri of eCall webservice.
		'''</summary>
		Private m_eCallWebServiceUri As String


		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI


		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' Settings xml.
		''' </summary>
		Private m_MandantSettingsXml As SettingsXml

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		Private m_CurrentJobID As String


#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_InitializationData = _setting
			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI

			m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

			m_SuppressUIEvents = True

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				'm_SPPVLGAVUtilServiceUrl = DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI

				Dim domainName = m_InitializationData.MDData.WebserviceDomain
				m_SPPVLGAVUtilServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI)


				'm_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

				'If String.IsNullOrWhiteSpace(m_PaymentUtilWebServiceUri) Then
				'	m_PaymentUtilWebServiceUri = DEFAULT_SPUTNIK_PVL_UTIL_WEBSERVICE_URI
				'End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				'm_SPPVLGAVUtilServiceUrl = DEFAULT_SPUTNIK_PVLGAV_UTIL_WEBSERVICE_URI
			End Try

			Reset()


		End Sub

#End Region

		''' <summary>
		'''  Performs the check asynchronous.
		''' </summary>
		Public Function LoadPVLMetaData() As List(Of PVLMetaData)
			Dim listDataSource As List(Of PVLMetaData) = New List(Of PVLMetaData)
			Dim viewData As New PVLMetaData

			Try
				Dim data = PerformWebserviceCallAsync()
				For Each result In data
					viewData = New PVLMetaData With {
						.GAVNumber = result.gav_number,
						.NameDE = result.name_de,
						.NameFR = result.name_fr,
						.NameIT = result.name_it,
						.CurruentDBName = result.currdbname
					}

					listDataSource.Add(viewData)
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Return listDataSource

		End Function

		Private Function PerformWebserviceCallAsync() As BindingList(Of PVLGAVViewData)

			Dim listDataSource As BindingList(Of PVLGAVViewData) = New BindingList(Of PVLGAVViewData)

			Dim webservice As New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLGAVUtilServiceUrl)

			' Read data over webservice
			Dim searchResult As GAVNameResultDTO() = webservice.GetPVLDataforMandant()

			For Each result In searchResult

				Dim viewData = New PVLGAVViewData With {
					.gav_number = result.gav_number,
					.name_de = result.name_de,
					.name_it = result.name_it,
					.name_fr = result.name_fr,
					.stdweek = result.stdweek,
					.stdmonth = result.stdmonth,
					.stdyear = result.stdyear,
					.fag = result.fag,
					.fan = result.fan,
					.vag = result.vag,
					.van = result.van,
					.currdbname = result.currdbname
				}

				listDataSource.Add(viewData)

			Next

			Return listDataSource

		End Function


		Class PVLGAVViewData

			Public Property gav_number As Integer
			Public Property name_de As String
			Public Property name_fr As String
			Public Property name_it As String
			Public Property publication_date As DateTime?
			Public Property schema_version As String

			Public Property stdweek As Decimal?
			Public Property stdmonth As Decimal?
			Public Property stdyear As Decimal?
			Public Property fan As Decimal?
			Public Property fag As Decimal?
			Public Property van As Decimal?
			Public Property vag As Decimal?
			Public Property currdbname As String


			Public ReadOnly Property fan_fag As String
				Get
					Return String.Format("{0:n2} - {1:n2}", fan, fag)
				End Get
			End Property

			Public ReadOnly Property van_vag As String
				Get
					Return String.Format("{0:n2} - {1:n2}", van, vag)
				End Get
			End Property


		End Class

	End Class



	Public Class PVLMetaData
		Public Property GAVNumber As Integer
		Public Property NameDE As String
		Public Property NameFR As String
		Public Property NameIT As String
		Public Property CurruentDBName As String

	End Class


End Namespace