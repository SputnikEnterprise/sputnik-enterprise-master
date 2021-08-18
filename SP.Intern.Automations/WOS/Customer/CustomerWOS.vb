

Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Threading
Imports SP.Internal.Automations.Settings_
Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService
Imports SP.Internal.Automations.SPeCallWebService
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Imports SP.DatabaseAccess.Listing.DataObjects
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
'Imports SP.KD.CPersonMng.UI
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Vacancy
Imports SP.Internal.Automations.Notifying
Imports SP.DatabaseAccess.Propose
Imports System.Text
Imports SPProgUtility.ProgPath

Namespace WOSUtility


	Public Class CustomerExport

#Region "Private Consts"

		Public Const DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSCustomerUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPWOSCustomerUtilities.asmx"
		Public Const DEFAULT_SPUTNIK_NOTIFICATION_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"
		Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPIBANUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"

		Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
		Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_VACANCY_GUID As String = "MD_{0}/Export/Vak_SPUser_ID"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "MD_{0}/Interfaces/webservices/webserviceecall"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

#End Region


#Region "Privte Fields"

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

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
		''' The customer data access object.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Private m_ProposeDatabaseAccess As IProposeDatabaseAccess

		''' <summary>
		''' The vacancy database access.
		''' </summary>
		Protected m_VacancyDatabaseAccess As IVacancyDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' Service Uri of Sputnik bank util webservice.
		''' </summary>
		Private m_CustomerWosUtilWebServiceUri As String
		Private m_NotificationWebServiceUri As String

		'''<summary>
		'''Service Uri of eCall webservice.
		'''</summary>
		Private m_eCallWebServiceUri As String

		Private m_path As ClsProgPath
		Private m_AccountPassword As String

		''' <summary>
		''' The settings manager.
		''' </summary>
		Protected m_SettingsManager As ISettingsManager

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
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

		Private m_CustomerWOSID As String
		Private m_VacancyWOSID As String
		Private m_WarnNotSeenProposesSince As Integer


#End Region



#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
			m_SettingsManager = New SettingsManager
			m_MandantData = New Mandant
			m_path = New ClsProgPath
			m_UtilityUI = New UtilityUI

			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			m_SuppressUIEvents = True

			Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"
			'#If DEBUG Then
			'			domainName = "http://localhost"
			'#End If

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
				m_CustomerWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI)
				m_NotificationWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_WEBSERVICE_URI)
				m_CustomerWOSID = CustomerWOSID
				m_VacancyWOSID = VacancyWOSID

				m_WarnNotSeenProposesSince = DaysForWarningNotSeenProposes

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		End Sub

#End Region


#Region "Public properties"

		Public Property WOSSetting As WOSSendSetting

#End Region


#Region "private properties"

		Private ReadOnly Property CustomerWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

				Return value
			End Get
		End Property

		Private ReadOnly Property VacancyWOSID() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_VACANCY_GUID, m_InitializationData.MDData.MDNr))

				Return value
			End Get
		End Property

		Private ReadOnly Property DaysForWarningNotSeenProposes() As Integer
			Get
				Dim xmlFile As String = m_MandantData.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
				Dim value As Integer = Val(m_path.GetXMLNodeValue(xmlFile, String.Format("{0}/warnnotseenproposessince", FORM_XML_MAIN_KEY)))

				Return value
			End Get
		End Property

#End Region


#Region "Public Methodes"

		Public Function LoadAssignedTransferedCustomerDataFromWOS(ByVal customerID As String, ByVal modulGuid As String, ByVal modulNumber As Integer) As IEnumerable(Of WOSSearchResultData)

			Dim success As Boolean = True
			Dim result As List(Of WOSSearchResultData) = Nothing

#If DEBUG Then
			'm_CustomerWosUtilWebServiceUri = "http://localhost:44721/SPWOSCustomerUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

				Dim searchResult = InternService.LoadAssignedCustomerWOSModul(customerID, m_CustomerWOSID, modulGuid, modulNumber)
				If searchResult Is Nothing Then Return Nothing

				result = New List(Of WOSSearchResultData)

				For Each itm In searchResult
					Dim data = New WOSSearchResultData

					data.DocGuid = itm.DocGuid
					data.CreatedOn = itm.TransferedOn
					data.DocViewedOn = itm.Viewed_On
					data.DocViewResult = itm.ViewedResult
					data.Get_On = itm.Get_On
					data.GetResult = itm.GetResult
					data.CustomerFeedback = itm.CustomerFeedback
					data.CustomerFeedback_On = itm.CustomerFeedback_On

					result.Add(data)
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return result

		End Function

		Public Function NotifyAdvisorForTransferedProposeData(ByVal customerID As String, ByVal modulGuid As String, ByVal modulNumber As Integer, ByVal modulDocArt As Integer) As Boolean
			Dim success As Boolean = True
			Dim notifyObj As New Notifier(m_InitializationData)
			Dim notifyResult As Boolean = True

			Try

				Dim searchResult = LoadAssignedTransferedCustomerDataFromWOSByDocArt(customerID, modulGuid, modulNumber, modulDocArt)
				If searchResult Is Nothing Then
					m_Logger.LogDebug("LoadAssignedTransferedCustomerDataFromWOSByDocArt >>> is nothing!")
					Return False
				End If

				m_Logger.LogWarning(String.Format("m_WarnNotSeenProposesSince: {0}", m_WarnNotSeenProposesSince))
				For Each itm In searchResult
					m_Logger.LogDebug(String.Format("searching for propose: {0}", itm.AssignedDocNumber))
					Dim proposeData = m_ProposeDatabaseAccess.LoadProposeMasterData(itm.AssignedDocNumber)
					If proposeData Is Nothing Then
						m_Logger.LogWarning(String.Format("LoadProposeMasterData: propose: {0} could not be founded!", itm.AssignedDocNumber))

						Continue For
					End If
					Dim userNumbers = New List(Of Integer)(New Integer() {proposeData.Customer_UserNumber, proposeData.Employee_UserNumber})
					Dim body As String = String.Empty
					Dim bodyElements = New List(Of String)

					If m_WarnNotSeenProposesSince > 0 AndAlso itm.DocViewedOn Is Nothing Then
						' itm.NotifyAdvisor is still nothing!!!
						If Now > CDate(itm.CreatedOn).AddDays(m_WarnNotSeenProposesSince) Then
							m_Logger.LogWarning(String.Format("item was created on: {0:G}", itm.CreatedOn))
							bodyElements.Add(String.Format("Der Vorschlag wurde seit {0:G} <b>nicht</b> angeschaut!", itm.CreatedOn))
						End If
					End If

					m_Logger.LogDebug(String.Format("NotifyAdvisor: {0}", itm.NotifyAdvisor))
					'If itm.NotifyAdvisor Then
					If itm.GetResult.GetValueOrDefault(0) <> 0 Then bodyElements.Add(String.Format("Resultat: {0} ({1:G})",
																									   If(itm.GetResult.GetValueOrDefault(0) = 1, "<b>Interessant</b>", "<b>Nicht Interessant</b>"),
																									   itm.Get_On))
						If Not String.IsNullOrWhiteSpace(itm.CustomerFeedback) Then bodyElements.Add(String.Format("Feedback: {0} ({1:G})", itm.CustomerFeedback, itm.CustomerFeedback_On))
						If Not itm.DocViewedOn Is Nothing Then bodyElements.Add(String.Format("Gesehen am: {0:G}", itm.DocViewedOn))
					'End If

					m_Logger.LogDebug(String.Format("bodyElements.Count: {0}", bodyElements.Count))
					If bodyElements.Count > 0 Then
						body = String.Join("<br>", bodyElements)
						notifyResult = notifyObj.AddNewNotifierForProposeResult(m_InitializationData.MDData.MDGuid, proposeData, body, userNumbers)

						notifyResult = notifyResult AndAlso SignAssignedDocNotificationAsDownloaded(customerID, modulGuid, itm.ID)
					Else
						m_Logger.LogWarning(String.Format("will not inesrt todo record!: {0}", bodyElements.Count))

					End If

				Next
				m_Logger.LogDebug(String.Format("finishing todo jobs."))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return False
			End Try


			Return success
		End Function

		Private Function LoadAssignedTransferedCustomerDataFromWOSByDocArt(ByVal customerID As String, ByVal modulGuid As String, ByVal modulNumber As Integer, ByVal modulDocArt As Integer) As IEnumerable(Of WOSSearchResultData)

			Dim success As Boolean = True
			Dim result As List(Of WOSSearchResultData) = Nothing

#If DEBUG Then
			'm_CustomerWosUtilWebServiceUri = "http://localhost:44721/SPWOSCustomerUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

				m_Logger.LogDebug(String.Format("customerID: {0} >>> m_CustomerWOSID: {1} | modulGuid: {2} | modulNumber: {3} | modulDocArt: {4}",
												customerID, m_CustomerWOSID, modulGuid, modulNumber, modulDocArt))
				Dim searchResult = InternService.LoadAssignedCustomerWOSModulByDocArt(customerID, m_CustomerWOSID, modulGuid, modulNumber, modulDocArt)
				If searchResult Is Nothing Then
					m_Logger.LogError(String.Format("LoadAssignedCustomerWOSModulByDocArt.searchResult was nothing!"))
					Return Nothing
				Else
					m_Logger.LogDebug(String.Format("LoadAssignedCustomerWOSModulByDocArt.searchResult {0}!", searchResult.Count))

				End If
				result = New List(Of WOSSearchResultData)

				For Each itm In searchResult
					Dim data = New WOSSearchResultData

					data.ID = itm.ID
					data.AssignedDocNumber = itm.ProposeNr
					data.DocGuid = itm.DocGuid
					data.CreatedOn = itm.TransferedOn
					data.DocViewedOn = itm.Viewed_On
					data.DocViewResult = itm.ViewedResult
					data.Get_On = itm.Get_On
					data.GetResult = itm.GetResult
					data.CustomerFeedback = itm.CustomerFeedback
					data.CustomerFeedback_On = itm.CustomerFeedback_On
					data.NotifyAdvisor = itm.NotifyAdvisor

					If itm.NotifyAdvisor Then
						result.Add(data)
					End If
				Next


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return result

		End Function

		Private Function SignAssignedDocNotificationAsDownloaded(ByVal customerID As String, ByVal modulGuid As String, ByVal recID As Integer) As Boolean

			Dim success As Boolean = True

#If DEBUG Then
			'm_CustomerWosUtilWebServiceUri = "http://localhost:44721/SPWOSCustomerUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)

				success = success AndAlso InternService.UpdateAssignedDocNotificationAsDone(customerID, m_CustomerWOSID, modulGuid, recID)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return success

		End Function

		Public Function TransferCustomerDocumentDataToWOS(ByVal bSendFinal As Boolean) As WOSSendResult

			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}

			If WOSSetting Is Nothing Then
				result.Value = False
				result.Message = "No Properties was defined!"

				Return result
			End If

			Dim customerData = m_CustomerDatabaseAccess.LoadCustomerDataForWOSExport(m_InitializationData.UserData.UserNr,
																																				 WOSSetting.CustomerNumber, WOSSetting.CresponsibleNumber,
																																				 WOSSetting.EmploymentNumber, WOSSetting.ESLohnNumber,
																																				 WOSSetting.ReportNumber, WOSSetting.InvoiceNumber)
			m_Logger.LogDebug(String.Format("wos customerservice starting: m_CustomerWOSID: {0} | UserNr: {1} | CustomerNumber: {2} | CresponsibleNumber: {3} |  EmploymentNumber: {4} | ESLohnNumber: {5} | ReportNumber: {6} | InvoiceNumber: {7}, | ProposeNumber: {8}, | DocumentInfo: {9} | ScanDocName: {10}",
												m_CustomerWOSID,
												m_InitializationData.UserData.UserNr,
												WOSSetting.CustomerNumber,
												WOSSetting.CresponsibleNumber,
												WOSSetting.EmploymentNumber,
												WOSSetting.ESLohnNumber,
												WOSSetting.ReportNumber,
												WOSSetting.InvoiceNumber,
												WOSSetting.ProposeNumber,
												WOSSetting.DocumentInfo,
												WOSSetting.ScanDocName))

			If customerData Is Nothing Then
				result.Value = False
				result.Message = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Möglicherweise der/die KundenberaterIn ist nicht mehr aktiv oder vorhanden.")

				Return result
			End If

			customerData.CustomerWOSID = m_CustomerWOSID
			customerData.AssignedDocumentInfo = WOSSetting.DocumentInfo
			customerData.ScanDocName = WOSSetting.ScanDocName
			customerData.ScanDoc = WOSSetting.ScanDoc
			customerData.ProposeNumber = WOSSetting.ProposeNumber
			customerData.AssignedDocumentGuid = WOSSetting.AssignedDocumentGuid

			Select Case True
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung
					customerData.AssignedDocumentArtName = "Rechnung"

				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Verleihvertrag
					customerData.AssignedDocumentArtName = "Verleihvertrag"

				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
					customerData.AssignedDocumentArtName = "Rapport"

				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Vorschlag
					customerData.AssignedDocumentArtName = "Vorschlag"

				Case Else
					result.Value = False
					result.Message = m_Translate.GetSafeTranslationValue("Kind of document is not valid!")

					Return result

			End Select

			If success AndAlso bSendFinal Then success = success AndAlso PerformTransferCustomerDocumentToWebservice(customerData).Value


			Return result

		End Function

		''' <summary>
		'''  Performs the transfer customer document to wos asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformTransferCustomerDocumentToWebservice(ByVal customerData As CustomerWOSData) As WOSSendResult
			Dim result As WOSSendResult = Nothing

			result = New WOSSendResult
			Dim exportResult As New WOSSendResult
			If customerData.CustomerWOSID.Length <= 10 Then
				exportResult.Message = "Keine Berechtigung!"
				exportResult.Value = False

				Return exportResult
			End If

			Try
#If DEBUG Then
				'm_CustomerWosUtilWebServiceUri = "http://localhost:44721/SPWOSCustomerUtilities.asmx"
#End If
				m_Logger.LogDebug("initial webservice starting...")
				Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)
				m_Logger.LogDebug("initial webservice done...")


				Dim customerWOSData = New SP.Internal.Automations.SPWOSCustomerWebService.CustomerWOSData

				customerWOSData.AssignedDocumentArtName = customerData.AssignedDocumentArtName
				customerWOSData.AssignedDocumentGuid = WOSSetting.AssignedDocumentGuid
				customerWOSData.CustomerWOSID = customerData.CustomerWOSID
				customerWOSData.AssignedDocumentInfo = customerData.AssignedDocumentInfo
				customerWOSData.ScanDocName = customerData.ScanDocName
				customerWOSData.ScanDoc = customerData.ScanDoc

				customerWOSData.CustomerNumber = customerData.CustomerNumber
				customerWOSData.CresponsibleNumber = customerData.CresponsibleNumber
				customerWOSData.EmploymentNumber = customerData.EmploymentNumber
				customerWOSData.EmploymentLineNumber = customerData.EmploymentLineNumber
				customerWOSData.ReportNumber = customerData.ReportNumber
				customerWOSData.InvoiceNumber = customerData.InvoiceNumber
				customerWOSData.ProposeNumber = customerData.ProposeNumber
				customerWOSData.EmployeeNumber = WOSSetting.EmployeeNumber

				customerWOSData.KDTransferedGuid = customerData.KDTransferedGuid
				customerWOSData.ZHDTransferedGuid = customerData.ZHDTransferedGuid

				customerWOSData.CustomerMail = customerData.CustomerMail
				customerWOSData.customername = customerData.customername
				customerWOSData.CustomerStrasse = customerData.CustomerStrasse
				customerWOSData.CustomerOrt = customerData.CustomerOrt
				customerWOSData.CustomerPLZ = customerData.CustomerPLZ
				customerWOSData.CustomerTelefon = customerData.CustomerTelefon
				customerWOSData.CustomerTelefax = customerData.CustomerTelefax
				customerWOSData.CustomerHomepage = customerData.CustomerHomepage
				customerWOSData.DoNotShowContractInWOS = customerData.DoNotShowContractInWOS

				customerWOSData.UserAnrede = customerData.UserAnrede
				customerWOSData.UserVorname = customerData.UserVorname
				customerWOSData.UserName = customerData.UserName
				customerWOSData.UserTelefon = customerData.UserTelefon
				customerWOSData.UserTelefax = customerData.UserTelefax
				customerWOSData.UserMail = customerData.UserMail
				customerWOSData.UserInitial = customerData.UserInitial
				customerWOSData.UserSex = customerData.UserSex
				customerWOSData.UserFiliale = customerData.UserFiliale
				customerWOSData.UserSign = customerData.UserSign
				customerWOSData.UserPicture = customerData.UserPicture
				customerWOSData.LogedUserID = customerData.LogedUserID

				customerWOSData.MDTelefon = customerData.MDTelefon
				customerWOSData.MD_DTelefon = customerData.MD_DTelefon
				customerWOSData.MDTelefax = customerData.MDTelefax
				customerWOSData.MDMail = customerData.MDMail

				customerWOSData.KD_Name = customerData.KD_Name
				customerWOSData.KD_Postfach = customerData.KD_Postfach
				customerWOSData.KD_Strasse = customerData.KD_Strasse
				customerWOSData.KD_PLZ = customerData.KD_PLZ
				customerWOSData.KD_Ort = customerData.KD_Ort
				customerWOSData.KD_Land = customerData.KD_Land
				customerWOSData.KD_Filiale = customerData.KD_Filiale
				customerWOSData.KD_Berater = customerData.KD_Berater
				customerWOSData.KD_Email = customerData.KD_Email
				customerWOSData.KD_AGB_Wos = customerData.KD_AGB_Wos
				customerWOSData.KD_Beruf = customerData.KD_Beruf
				customerWOSData.KD_Branche = customerData.KD_Branche
				customerWOSData.KD_Language = customerData.KD_Language

				customerWOSData.ZHD_Vorname = customerData.ZHD_Vorname
				customerWOSData.ZHD_Nachname = customerData.ZHD_Nachname
				customerWOSData.ZHD_EMail = customerData.ZHD_EMail
				customerWOSData.ZHDSex = customerData.ZHDSex
				customerWOSData.Zhd_BriefAnrede = customerData.Zhd_BriefAnrede
				customerWOSData.Zhd_Berater = customerData.Zhd_Berater
				customerWOSData.Zhd_Beruf = customerData.Zhd_Beruf
				customerWOSData.Zhd_Branche = customerData.Zhd_Branche
				customerWOSData.ZHD_AGB_Wos = customerData.ZHD_AGB_Wos
				customerWOSData.ZHD_GebDat = customerData.ZHD_GebDat
				customerWOSData.SignTransferedDocument = WOSSetting.SignTransferedDocument


				m_Logger.LogDebug("sendig customer wos document data to webservice...")

				'exportResult.Value = InternService.AddCustomerDocumentToWOS(customerWOSData)
				exportResult.Value = InternService.AddAssignedCustomerWOSDocument(m_InitializationData.MDData.MDGuid, customerWOSData)
				exportResult.Message = If(exportResult.Value, "", "Kein bekannter Fehler.")


				result = (exportResult)


			Catch ex As Exception
				result.Value = False
				result.Message = ex.ToString
				m_Logger.LogError(ex.ToString())

			End Try

			Return result

		End Function


		''' <summary>
		'''  Performs wos notification check asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Public Function PerformWOSModulNotificationlistWebservice(ByVal modulNumber As Integer, ByVal number As Integer) As BindingList(Of WOSEMailNotificationData)
			Dim listDataSource As BindingList(Of WOSEMailNotificationData) = New BindingList(Of WOSEMailNotificationData)
			Dim Customer_ID As String = m_CustomerWOSID


#If DEBUG Then
			Customer_ID = m_CustomerWOSID
#End If

			Dim webservice As New SPNotificationWebService.SPNotificationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_NotificationWebServiceUri)

			Try
				' Read data over webservice
				Dim searchDataResult = webservice.GetWOSModulMailNotifications(Customer_ID, modulNumber, number).ToList

				If searchDataResult Is Nothing Then
					m_Logger.LogWarning(String.Format("WOS Document could not be loaded! {0}", Customer_ID))

					Return Nothing
				End If

				For Each result In searchDataResult

					Dim viewData = New WOSEMailNotificationData With {
					.ID = result.ID,
					.CreatedOn = result.CreatedOn,
					.Customer_ID = result.Customer_ID,
					.DocLink = result.DocLink,
					.MailBody = result.MailBody,
					.MailFrom = result.MailFrom,
					.MailSubject = result.MailSubject,
					.MailTo = result.MailTo,
					.RecipientGuid = result.RecipientGuid,
					.Result = result.Result
				}

					listDataSource.Add(viewData)

				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try


			Return listDataSource

		End Function


		''' <summary>
		''' Delete transfered document from WOS
		''' </summary>
		Public Function DeleteTransferedCustomerDocument() As Boolean

			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}

			If WOSSetting Is Nothing Then
				result.Value = False
				result.Message = "No Properties was defined!"

				Return False
			End If

			m_Logger.LogDebug("WOS deleting customer document started...")

#If DEBUG Then
			'm_CustomerWosUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPWOSCustomerUtilities.asmx"
#End If
			Dim InternService As New SPWOSCustomerWebService.SPWOSCustomerUtilitiesSoapClient
			InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_CustomerWosUtilWebServiceUri)
			Dim customerWOSData = New SP.Internal.Automations.SPWOSCustomerWebService.CustomerWOSData

			customerWOSData.CustomerWOSID = m_CustomerWOSID
			customerWOSData.AssignedDocumentGuid = If(String.IsNullOrWhiteSpace(WOSSetting.CustomerDocumentGuid), WOSSetting.AssignedDocumentGuid, WOSSetting.CustomerDocumentGuid)

			Select Case True
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung
					customerWOSData.AssignedDocumentArtName = "Rechnung"

				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Verleihvertrag
					customerWOSData.AssignedDocumentArtName = "Verleihvertrag"

				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
					customerWOSData.AssignedDocumentArtName = "Rapport"

				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Vorschlag
					customerWOSData.AssignedDocumentArtName = "Vorschlag"

				Case Else
					result.Value = False
					result.Message = m_Translate.GetSafeTranslationValue("Kind of document is not valid!")

					Return False

			End Select

			result.Value = success AndAlso InternService.DeleteAssignedCustomerDocumentWithGuid(customerWOSData)
			result.Message = If(result.Value, "", "Kein bekannter Fehler.")


			Return success

		End Function


#End Region


#Region "private mehtodes"


#End Region


	End Class


End Namespace