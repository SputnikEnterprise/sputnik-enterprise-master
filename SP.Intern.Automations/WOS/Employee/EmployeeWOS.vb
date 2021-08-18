
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.WOS


Namespace WOSUtility


	Public Class EmployeeExport

#Region "Private Consts"

		Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
		Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"

		Public Const DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSEmployeeUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPWOSEmployeeUtilities.asmx"
		Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPIBANUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "MD_{0}/Interfaces/webservices/webserviceecall"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

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
		''' The wos data access object.
		''' </summary>
		Private m_WOSDatabaseAccess As IWOSDatabaseAccess

		''' <summary>
		''' The Listing data access object.
		''' </summary>
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		''' <summary>
		''' Service Uri of Sputnik bank util webservice.
		''' </summary>
		Private m_EmployeeWosUtilWebServiceUri As String

		'''<summary>
		'''Service Uri of eCall webservice.
		'''</summary>
		Private m_eCallWebServiceUri As String

		Private m_AccountName As String
		Private m_AccountPassword As String

		''' <summary>
		''' The settings manager.
		''' </summary>
		Protected m_SettingsManager As ISettingsManager

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_Utility As Infrastructure.Utility


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

		Private m_EmployeeWOSID As String


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
			m_SettingsManager = New SettingsManager
			m_MandantData = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New Infrastructure.Utility
			m_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

			Dim connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_WOSDatabaseAccess = New WOSDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_SuppressUIEvents = True

			Try
				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

				Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"
				'#If DEBUG Then
				'			domainName = "http://localhost"
				'#End If
				m_EmployeeWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI)
				m_EmployeeWOSID = WOSIDSetting

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try


		End Sub

#End Region


#Region "private properties"

		Private ReadOnly Property WOSIDSetting() As String
			Get
				Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr))

				Return value
			End Get
		End Property


#End Region


#Region "Public properties"

		Public Property WOSSetting As WOSSendSetting

#End Region




#Region "Public Methodes"

		Public Function LoadAvailableTransferedEmployeeDataFromWOS(ByVal customerID As String) As Boolean

			Dim success As Boolean = True
			Dim result As Boolean = True

#If DEBUG Then
			'm_EmployeeWosUtilWebServiceUri = "http://localhost:44721/SPWOSemployeeUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)

				Dim serachResult = InternService.LoadAvailableEmployee(customerID, m_EmployeeWOSID, "", "", "", "")
				m_Logger.LogDebug(String.Format("count of employees: {0}", serachResult.Count))


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return result

		End Function

		Public Function LoadAssignedEmployeeDataFromWOS(ByVal customerID As String, ByVal modulGuid As String, ByVal modulNumber As Integer) As Boolean

			Dim success As Boolean = True
			Dim result As Boolean = True

#If DEBUG Then
			'm_EmployeeWosUtilWebServiceUri = "http://localhost:44721/SPWOSemployeeUtilities.asmx"
#End If

			Try
				Dim InternService As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)

				Dim serachResult = InternService.LoadAssignedAvailableEmployeeData(customerID, m_EmployeeWOSID, modulNumber)
				m_Logger.LogDebug(String.Format("employee: {0}", serachResult.Lastname))


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False

			End Try


			Return result

		End Function

		''' <summary>
		''' transfering available employee data to wos. wossetting should not be initialized!
		''' </summary>
		''' <param name="employeeNumber"></param>
		''' <returns></returns>
		Public Function TransferEmployeeDataToWOS(ByVal employeeNumber As Integer) As WOSSendResult
			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}
			Dim msg As String

			Dim employeeMasterData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, True)
			If employeeMasterData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", employeeNumber))

				Return New WOSSendResult With {.Value = False,
					.Message = m_Translate.GetSafeTranslationValue(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", employeeNumber))}
			End If

			Dim employeeGuid = employeeMasterData.Transfered_Guid
			If String.IsNullOrWhiteSpace(employeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				success = success AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(employeeNumber, newGuid)

				If success Then employeeGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.EmployeeGuid = employeeGuid
			_setting.EmployeeNumber = employeeNumber

			Dim employeeData = m_EmployeeDatabaseAccess.LoadAvailableEmployeeDataForWOSExport(employeeNumber, m_InitializationData.UserData.UserNr)
			If employeeData Is Nothing Then
				msg = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Möglicherweise der/die BeraterIn ist nicht mehr aktiv oder vorhanden.")
				m_UtilityUI.ShowErrorDialog(msg)

				result.Value = False
				result.Message = msg

				Return result
			End If

			msg = "wos employeeservice starting: "
			msg &= "m_EmployeeWOSID: {0} | UserNr: {1} | EmployeeNumber: {2}"
			m_Logger.LogDebug(String.Format(msg, m_EmployeeWOSID, m_InitializationData.UserData.UserNr, employeeNumber))

			employeeData.EmployeeWOSID = m_EmployeeWOSID
			WOSSetting = _setting

			If success Then success = success AndAlso PerformTransferAvailableEmployeeToWebService(employeeData, False).Value


			Return result

		End Function

		Public Function TransferEmployeeDataToWOS(ByVal employeeNumber As Integer, ByVal employeeTemplate As List(Of String)) As WOSSendResult
			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}
			Dim msg As String

			Dim employeeMasterData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, True)
			If employeeMasterData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", employeeNumber))

				Return New WOSSendResult With {.Value = False,
					.Message = m_Translate.GetSafeTranslationValue(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", employeeNumber))}
			End If

			Dim employeeGuid = employeeMasterData.Transfered_Guid
			If String.IsNullOrWhiteSpace(employeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				success = success AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(employeeNumber, newGuid)

				If success Then employeeGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.EmployeeGuid = employeeGuid
			_setting.EmployeeNumber = employeeNumber

			Dim employeeData = m_EmployeeDatabaseAccess.LoadAvailableEmployeeDataForWOSExport(employeeNumber, m_InitializationData.UserData.UserNr)
			If employeeData Is Nothing Then
				msg = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Möglicherweise der/die BeraterIn ist nicht mehr aktiv oder vorhanden.")
				m_UtilityUI.ShowErrorDialog(msg)

				result.Value = False
				result.Message = msg

				Return result
			End If

			msg = "wos employeeservice starting: "
			msg &= "m_EmployeeWOSID: {0} | UserNr: {1} | EmployeeNumber: {2}"
			m_Logger.LogDebug(String.Format(msg, m_EmployeeWOSID, m_InitializationData.UserData.UserNr, employeeNumber))

			employeeData.EmployeeWOSID = m_EmployeeWOSID

			If Not employeeTemplate Is Nothing AndAlso employeeTemplate.Count > 0 Then
				employeeData.EmployeeTemplates = New List(Of AvailableEmployeeTemplateData)
				For Each itm In employeeTemplate
					Dim data = New AvailableEmployeeTemplateData
					data.EmployeeNumber = employeeData.EmployeeNumber
					data.ScanDoc = m_Utility.LoadFileBytes(itm)

					employeeData.EmployeeTemplates.Add(data)
				Next
			End If

			'employeeData.EmployeeTemplates = employeeTemplate
			WOSSetting = _setting

			If success Then result = PerformTransferAvailableEmployeeToWebService(employeeData, False)
			success = success AndAlso result.Value


			Return result

		End Function

		''' <summary>
		''' delete assigned available employee data to wos. wossetting should not be initialized!
		''' </summary>
		''' <param name="employeeNumber"></param>
		''' <returns></returns>
		Public Function DeleteTransferedEmployeeDataFromWOS(ByVal employeeNumber As Integer) As WOSSendResult
			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}
			Dim msg As String

			Dim employeeMasterData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, True)
			If employeeMasterData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", employeeNumber))

				Return New WOSSendResult With {.Value = False,
					.Message = m_Translate.GetSafeTranslationValue(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", employeeNumber))}
			End If

			Dim employeeGuid = employeeMasterData.Transfered_Guid
			If String.IsNullOrWhiteSpace(employeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				success = success AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(employeeNumber, newGuid)

				If success Then employeeGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.EmployeeGuid = employeeGuid
			_setting.EmployeeNumber = employeeNumber

			Dim employeeData = m_EmployeeDatabaseAccess.LoadAvailableEmployeeDataForWOSExport(employeeNumber, m_InitializationData.UserData.UserNr)
			If employeeData Is Nothing Then
				msg = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Möglicherweise der/die BeraterIn ist nicht mehr aktiv oder vorhanden.")
				m_UtilityUI.ShowErrorDialog(msg)

				result.Value = False
				result.Message = msg

				Return result
			End If

			msg = "wos employeeservice starting: "
			msg &= "m_EmployeeWOSID: {0} | UserNr: {1} | EmployeeNumber: {2}"
			m_Logger.LogDebug(String.Format(msg, m_EmployeeWOSID, m_InitializationData.UserData.UserNr, employeeNumber))

			employeeData.EmployeeWOSID = m_EmployeeWOSID
			WOSSetting = _setting

			If success Then result = PerformTransferAvailableEmployeeToWebService(employeeData, True)
			success = success AndAlso result.Value


			Return result

		End Function

		''' <summary>
		''' transfering available employee data to wos. wossetting should be initialized!
		''' </summary>
		''' <returns></returns>
		Public Function TransferAvailableEmployeeDataToWOS() As WOSSendResult

			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}
			Dim msg As String

			If WOSSetting Is Nothing Then
				result.Value = False
				result.Message = "No Properties was defined!"

				Return result
			End If

			Dim employeeData = m_EmployeeDatabaseAccess.LoadAvailableEmployeeDataForWOSExport(WOSSetting.EmployeeNumber, m_InitializationData.UserData.UserNr)
			If employeeData Is Nothing Then
				msg = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Möglicherweise der/die BeraterIn ist nicht mehr aktiv oder vorhanden.")
				m_UtilityUI.ShowErrorDialog(msg)

				result.Value = False
				result.Message = msg

				Return result
			End If

			msg = "wos employeeservice starting: "
			msg &= "m_EmployeeWOSID: {0} | UserNr: {1} | EmployeeNumber: {2}"
			m_Logger.LogDebug(String.Format(msg, m_EmployeeWOSID, m_InitializationData.UserData.UserNr, WOSSetting.EmployeeNumber))

			employeeData.EmployeeWOSID = m_EmployeeWOSID

			If success Then success = success AndAlso PerformTransferAvailableEmployeeToWebService(employeeData, False).Value


			Return result

		End Function

		Public Function TransferEmployeeDocumentDataToWOS(ByVal bSendFinal As Boolean) As WOSSendResult

			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}
			Dim msg As String

			If WOSSetting Is Nothing Then
				result.Value = False
				result.Message = "No Properties was defined!"

				Return result
			End If
			Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeDataForWOSExport(m_InitializationData.UserData.UserNr,
																																							 WOSSetting.EmployeeNumber,
																																							 WOSSetting.EmploymentNumber, WOSSetting.ESLohnNumber,
																																							 WOSSetting.ReportNumber, WOSSetting.ReportLineNumber,
																																							 WOSSetting.ReportDocumentNumber,
																																							 WOSSetting.PayrollNumber)
			If employeeData Is Nothing Then
				msg = m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden. Möglicherweise der/die BeraterIn ist nicht mehr aktiv oder vorhanden.")
				m_UtilityUI.ShowErrorDialog(msg)

				result.Value = False
				result.Message = msg

				Return result
			End If

			msg = "wos employeeservice starting: "
			msg &= "m_EmployeeWOSID: {0} | UserNr: {1} | EmployeeNumber: {2} | CustomerNumber: {3} | CresponsibleNumber: {4} |  "
			msg &= "EmploymentNumber: {5} | ESLohnNumber: {6} | ReportNumber: {7} | InvoiceNumber: {8} | PayrollNumber: {9} | "
			msg &= "DocumentInfo: {10} | ScanDocName: {11}"

			m_Logger.LogDebug(String.Format(msg,
												m_EmployeeWOSID,
												m_InitializationData.UserData.UserNr,
												WOSSetting.EmployeeNumber,
												WOSSetting.CustomerNumber,
												WOSSetting.CresponsibleNumber,
												WOSSetting.EmploymentNumber,
												WOSSetting.ESLohnNumber,
												WOSSetting.ReportNumber,
												WOSSetting.InvoiceNumber,
												WOSSetting.PayrollNumber,
												WOSSetting.DocumentInfo,
												WOSSetting.ScanDocName))

			employeeData.EmployeeWOSID = m_EmployeeWOSID
			employeeData.AssignedDocumentInfo = WOSSetting.DocumentInfo
			employeeData.ScanDocName = WOSSetting.ScanDocName
			employeeData.ScanDoc = WOSSetting.ScanDoc
			employeeData.AssignedDocumentGuid = WOSSetting.AssignedDocumentGuid

			Select Case WOSSetting.DocumentArtEnum
				Case WOSSendSetting.DocumentArt.Rechnung
					employeeData.AssignedDocumentArtName = "Rechnung"

				Case WOSSendSetting.DocumentArt.Verleihvertrag
					employeeData.AssignedDocumentArtName = "Verleihvertrag"

				Case WOSSendSetting.DocumentArt.Rapport
					employeeData.AssignedDocumentArtName = "Rapport"

				Case WOSSendSetting.DocumentArt.Einsatzvertrag
					employeeData.AssignedDocumentArtName = "Einsatzvertrag"

				Case WOSSendSetting.DocumentArt.Lohnabrechnung
					employeeData.AssignedDocumentArtName = "Lohnabrechnung"

				Case WOSSendSetting.DocumentArt.Zwischenverdienstformular
					employeeData.AssignedDocumentArtName = "Zwischenverdienstformular"

				Case WOSSendSetting.DocumentArt.Arbeitgeberbescheinigung
					employeeData.AssignedDocumentArtName = "Arbeitgeberbescheinigung"

				Case WOSSendSetting.DocumentArt.Lohnausweis
					employeeData.AssignedDocumentArtName = "Lohnausweis"


				Case Else
					result.Value = False
					result.Message = m_Translate.GetSafeTranslationValue("Kind of document is not valid!")

					Return result

			End Select

			If success AndAlso bSendFinal Then success = success AndAlso PerformTransferDocumentToWebservice(employeeData).Value


			Return result

		End Function

		''' <summary>
		''' Delete transfered document from WOS
		''' </summary>
		Public Function DeleteTransferedEmployeeDocument() As Boolean

			Dim success As Boolean = True
			Dim result As WOSSendResult = New WOSSendResult With {.Value = True}

			If WOSSetting Is Nothing Then
				result.Value = False
				result.Message = "No Properties was defined!"

				Return False
			End If

			m_Logger.LogDebug("WOS deleting customer document started...")

#If DEBUG Then
			'm_EmployeeWosUtilWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPWOSEmployeeUtilities.asmx"
#End If
			Dim InternService As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
			InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)
			Dim EmployeeData = New SP.Internal.Automations.SPWOSEmployeeWebService.EmployeeWOSData

			EmployeeData.EmployeeWOSID = m_EmployeeWOSID
			'EmployeeData.AssignedDocumentGuid = WOSSetting.CustomerDocumentGuid
			EmployeeData.AssignedDocumentGuid = WOSSetting.AssignedDocumentGuid

			Select Case True
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung
					EmployeeData.AssignedDocumentArtName = "Rechnung"
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Verleihvertrag
					EmployeeData.AssignedDocumentArtName = "Verleihvertrag"
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
					EmployeeData.AssignedDocumentArtName = "Rapport"
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Einsatzvertrag
					EmployeeData.AssignedDocumentArtName = "Einsatzvertrag"
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Lohnabrechnung
					EmployeeData.AssignedDocumentArtName = "Lohnabrechnung"
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Zwischenverdienstformular
					EmployeeData.AssignedDocumentArtName = "Zwischenverdienstformular"
				Case WOSSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Arbeitgeberbescheinigung
					EmployeeData.AssignedDocumentArtName = "Arbeitgeberbescheinigung"

				Case Else
					result.Value = False
					result.Message = m_Translate.GetSafeTranslationValue("Kind of document is not valid!")

					Return False

			End Select

			result.Value = success AndAlso InternService.DeleteAssignedEmployeeDocument(m_EmployeeWOSID, WOSSetting.EmployeeNumber, WOSSetting.EmployeeGuid, WOSSetting.AssignedDocumentGuid, EmployeeData.AssignedDocumentArtName, WOSSetting.DocumentInfo)
			result.Message = If(result.Value, "", "Kein bekannter Fehler.")


			Return success

		End Function


#End Region


#Region "private mehtodes"

		''' <summary>
		'''  Performs the transfer customer document to wos asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformTransferAvailableEmployeeToWebService(ByVal availableEmployeeData As AvailableEmployeeWOSData, ByVal justDeleteData As Boolean) As WOSSendResult
			Dim result As WOSSendResult = Nothing

			result = New WOSSendResult
			Dim exportResult As New WOSSendResult
			If availableEmployeeData.EmployeeWOSID.Length <= 10 Then
				exportResult.Message = "Keine Berechtigung!"
				exportResult.Value = False

				Return exportResult
			End If

			Try
#If DEBUG Then
				'm_EmployeeWosUtilWebServiceUri = "http://localhost:44721/SPWOSEmployeeUtilities.asmx"
#End If
				m_Logger.LogDebug("initial webservice starting...")
				Dim InternService As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)
				m_Logger.LogDebug("initial webservice done...")

				Dim wsWOSData = New SP.Internal.Automations.SPWOSEmployeeWebService.AvailableEmployeeNewDTO

				wsWOSData.WOS_ID = availableEmployeeData.EmployeeWOSID
				wsWOSData.Customer_ID = availableEmployeeData.Customer_ID
				wsWOSData.EmployeeNumber = availableEmployeeData.EmployeeNumber
				wsWOSData.EmployeeAdvisor = availableEmployeeData.MA_Berater

				wsWOSData.Advisor_ID = availableEmployeeData.LogedUserID
				wsWOSData.Firstname = availableEmployeeData.MA_Vorname
				wsWOSData.Lastname = availableEmployeeData.MA_Nachname
				wsWOSData.BrunchOffice = availableEmployeeData.MA_Filiale
				wsWOSData.Canton = availableEmployeeData.MA_Canton
				wsWOSData.Postcode = availableEmployeeData.MA_PLZ
				wsWOSData.Location = availableEmployeeData.MA_Ort
				wsWOSData.HowContact = availableEmployeeData.MA_Kontakt
				wsWOSData.FirstState = availableEmployeeData.MA_State1
				wsWOSData.SecondState = availableEmployeeData.MA_State2
				wsWOSData.Qualifications = availableEmployeeData.MA_Beruf
				wsWOSData.JobProzent = availableEmployeeData.JobProzent
				wsWOSData.BirthDay = availableEmployeeData.MA_GebDat

				wsWOSData.Gender = availableEmployeeData.MA_Gender
				wsWOSData.DriverLicenses = availableEmployeeData.MA_FSchein
				wsWOSData.Civilstate = availableEmployeeData.MA_Zivil
				wsWOSData.AvailableMobility = availableEmployeeData.MA_Auto

				wsWOSData.Nationality = availableEmployeeData.MA_Nationality
				wsWOSData.Permit = availableEmployeeData.Permit
				wsWOSData.Salutation = availableEmployeeData.Salutation
				wsWOSData.Transfer_ID = availableEmployeeData.MATransferedGuid
				wsWOSData.Transfer_UserID = m_InitializationData.UserData.UserGuid
				wsWOSData.SpokenLanguages = availableEmployeeData.MA_MSprache
				wsWOSData.WritingLanguages = availableEmployeeData.MA_SSprache
				wsWOSData.Properties = availableEmployeeData.MA_Eigenschaft

				wsWOSData.DesiredWagesOld = availableEmployeeData.DesiredWagesOld
				wsWOSData.DesiredWagesNew = availableEmployeeData.DesiredWagesNew
				wsWOSData.DesiredWagesInMonth = availableEmployeeData.DesiredWagesInMonth
				wsWOSData.DesiredWagesInHour = availableEmployeeData.DesiredWagesInHour


				Dim wsWOSReserveData = New SP.Internal.Automations.SPWOSEmployeeWebService.AvailableEmployeeReserveFields

				'Public Property EmployeeReserveFields As AvailableEmployeeReserveFields
				'Public Property EmployeeApplicationReserveFields As AvailableEmployeeApplicationFields
				'Public Property EmployeeAdvisorData As WOSAdvisorData

				wsWOSReserveData.EmployeeNumber = availableEmployeeData.EmployeeNumber
				wsWOSReserveData.Reserve1 = availableEmployeeData.MA_Res1
				wsWOSReserveData.Reserve2 = availableEmployeeData.MA_Res2
				wsWOSReserveData.Reserve3 = availableEmployeeData.MA_Res3
				wsWOSReserveData.Reserve4 = availableEmployeeData.MA_Res4
				wsWOSReserveData.Reserve5 = availableEmployeeData.MA_Res5

				wsWOSData.EmployeeReserveFields = wsWOSReserveData


				'Public Property EmployeeApplicationReserveFields As AvailableEmployeeApplicationFields
				Dim wsWOSApplicationReserveData = New SP.Internal.Automations.SPWOSEmployeeWebService.AvailableEmployeeApplicationFields

				wsWOSApplicationReserveData.EmployeeNumber = availableEmployeeData.EmployeeNumber
				wsWOSApplicationReserveData.LL_Name = availableEmployeeData.LL_Name
				wsWOSApplicationReserveData.ApplicationReserve0 = availableEmployeeData.Reserve0
				wsWOSApplicationReserveData.ApplicationReserve1 = availableEmployeeData.Reserve1
				wsWOSApplicationReserveData.ApplicationReserve2 = availableEmployeeData.Reserve2
				wsWOSApplicationReserveData.ApplicationReserve3 = availableEmployeeData.Reserve3
				wsWOSApplicationReserveData.ApplicationReserve4 = availableEmployeeData.Reserve4
				wsWOSApplicationReserveData.ApplicationReserve5 = availableEmployeeData.Reserve5
				wsWOSApplicationReserveData.ApplicationReserve6 = availableEmployeeData.Reserve6
				wsWOSApplicationReserveData.ApplicationReserve7 = availableEmployeeData.Reserve7
				wsWOSApplicationReserveData.ApplicationReserve8 = availableEmployeeData.Reserve8
				wsWOSApplicationReserveData.ApplicationReserve9 = availableEmployeeData.Reserve9
				wsWOSApplicationReserveData.ApplicationReserve10 = availableEmployeeData.Reserve10
				wsWOSApplicationReserveData.ApplicationReserve11 = availableEmployeeData.Reserve11
				wsWOSApplicationReserveData.ApplicationReserve12 = availableEmployeeData.Reserve12
				wsWOSApplicationReserveData.ApplicationReserve13 = availableEmployeeData.Reserve13
				wsWOSApplicationReserveData.ApplicationReserve14 = availableEmployeeData.Reserve14
				wsWOSApplicationReserveData.ApplicationReserve15 = availableEmployeeData.Reserve15

				wsWOSApplicationReserveData.ApplicationReserveRtf0 = availableEmployeeData.ReserveRtf0
				wsWOSApplicationReserveData.ApplicationReserveRtf1 = availableEmployeeData.ReserveRtf1
				wsWOSApplicationReserveData.ApplicationReserveRtf2 = availableEmployeeData.ReserveRtf2
				wsWOSApplicationReserveData.ApplicationReserveRtf3 = availableEmployeeData.ReserveRtf3
				wsWOSApplicationReserveData.ApplicationReserveRtf4 = availableEmployeeData.ReserveRtf4
				wsWOSApplicationReserveData.ApplicationReserveRtf5 = availableEmployeeData.ReserveRtf5
				wsWOSApplicationReserveData.ApplicationReserveRtf6 = availableEmployeeData.ReserveRtf6
				wsWOSApplicationReserveData.ApplicationReserveRtf7 = availableEmployeeData.ReserveRtf7
				wsWOSApplicationReserveData.ApplicationReserveRtf8 = availableEmployeeData.ReserveRtf8
				wsWOSApplicationReserveData.ApplicationReserveRtf9 = availableEmployeeData.ReserveRtf9
				wsWOSApplicationReserveData.ApplicationReserveRtf10 = availableEmployeeData.ReserveRtf10
				wsWOSApplicationReserveData.ApplicationReserveRtf11 = availableEmployeeData.ReserveRtf11
				wsWOSApplicationReserveData.ApplicationReserveRtf12 = availableEmployeeData.ReserveRtf12
				wsWOSApplicationReserveData.ApplicationReserveRtf13 = availableEmployeeData.ReserveRtf13
				wsWOSApplicationReserveData.ApplicationReserveRtf14 = availableEmployeeData.ReserveRtf14
				wsWOSApplicationReserveData.ApplicationReserveRtf15 = availableEmployeeData.ReserveRtf15

				wsWOSData.EmployeeApplicationReserveFields = wsWOSApplicationReserveData


				If Not availableEmployeeData.EmployeeTemplates Is Nothing AndAlso availableEmployeeData.EmployeeTemplates.Count > 0 Then
					Dim tpls As List(Of SPWOSEmployeeWebService.AvailableEmployeeTemplateData) = New List(Of SPWOSEmployeeWebService.AvailableEmployeeTemplateData)
					For Each itm In availableEmployeeData.EmployeeTemplates
						Dim doc = New SPWOSEmployeeWebService.AvailableEmployeeTemplateData
						doc.EmployeeNumber = itm.EmployeeNumber
						doc.ID = itm.ID
						doc.ScanDoc = itm.ScanDoc

						wsWOSData.AvailableEmployeeTemplates = doc
						tpls.Add(doc)
					Next

				End If

				m_Logger.LogDebug("sendig employee wos data to webservice...")
				If justDeleteData Then
					Dim webDeleteResult = InternService.RemoveAvailableEmployeeData(m_InitializationData.MDData.MDGuid, wsWOSData.WOS_ID, wsWOSData)
					exportResult.Value = webDeleteResult.JobResult
					exportResult.Message = If(webDeleteResult.JobResult, "", webDeleteResult.JobResultMessage)
				Else
					Dim webResult = InternService.AddAvailableEmployeeData(m_InitializationData.MDData.MDGuid, wsWOSData.WOS_ID, wsWOSData)
					exportResult.Value = webResult.JobResult
					exportResult.Message = If(webResult.JobResult, "", webResult.JobResultMessage)
				End If


				result = (exportResult)

			Catch ex As Exception
				result.Value = False
				result.Message = ex.ToString
				m_Logger.LogError(ex.ToString())

			End Try

			Return result

		End Function

		''' <summary>
		'''  Performs the transfer customer document to wos asynchronous.
		''' </summary>
		''' <returns>The report response.</returns>
		Private Function PerformTransferDocumentToWebservice(ByVal customerData As SP.DatabaseAccess.Employee.DataObjects.DocumentMng.EmployeeWOSData) As WOSSendResult
			Dim result As WOSSendResult = Nothing

			result = New WOSSendResult
			Dim exportResult As New WOSSendResult
			If customerData.EmployeeWOSID.Length <= 10 Then
				exportResult.Message = "Keine Berechtigung!"
				exportResult.Value = False

				Return exportResult
			End If

			Try
#If DEBUG Then
				'm_EmployeeWosUtilWebServiceUri = "http://localhost:44721/SPWOSEmployeeUtilities.asmx"
#End If
				m_Logger.LogDebug("initial webservice starting...")
				Dim InternService As New SPWOSEmployeeWebService.SPWOSEmployeeUtilitiesSoapClient
				InternService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_EmployeeWosUtilWebServiceUri)
				m_Logger.LogDebug("initial webservice done...")


				Dim wsWOSData = New SP.Internal.Automations.SPWOSEmployeeWebService.EmployeeWOSData

				wsWOSData.AssignedDocumentArtName = customerData.AssignedDocumentArtName
				wsWOSData.AssignedDocumentGuid = WOSSetting.AssignedDocumentGuid
				wsWOSData.EmployeeWOSID = customerData.EmployeeWOSID
				wsWOSData.AssignedDocumentInfo = customerData.AssignedDocumentInfo
				wsWOSData.ScanDocName = customerData.ScanDocName
				wsWOSData.ScanDoc = customerData.ScanDoc

				wsWOSData.EmployeeNumber = customerData.EmployeeNumber
				wsWOSData.EmploymentNumber = customerData.EmploymentNumber
				wsWOSData.EmploymentLineNumber = customerData.EmploymentLineNumber
				wsWOSData.ReportNumber = customerData.ReportNumber
				wsWOSData.ReportLineNumber = customerData.ReportLlineNumber
				wsWOSData.ReportDocNumber = customerData.ReportDocNumber
				wsWOSData.PayrollNumber = customerData.PayrollNumber

				wsWOSData.MATransferedGuid = customerData.MATransferedGuid

				wsWOSData.UserAnrede = customerData.UserAnrede
				wsWOSData.UserVorname = customerData.UserVorname
				wsWOSData.UserName = customerData.UserName
				wsWOSData.UserTelefon = customerData.UserTelefon
				wsWOSData.UserTelefax = customerData.UserTelefax
				wsWOSData.UserMail = customerData.UserMail
				wsWOSData.UserInitial = customerData.UserInitial
				wsWOSData.UserSex = customerData.UserSex
				wsWOSData.UserFiliale = customerData.UserFiliale
				wsWOSData.UserSign = customerData.UserSign
				wsWOSData.UserPicture = customerData.UserPicture
				wsWOSData.LogedUserID = customerData.LogedUserID

				wsWOSData.MDTelefon = customerData.MDTelefon
				wsWOSData.MD_DTelefon = customerData.MD_DTelefon
				wsWOSData.MDTelefax = customerData.MDTelefax
				wsWOSData.MDMail = customerData.MDMail

				wsWOSData.MA_Nachname = customerData.MA_Nachname
				wsWOSData.MA_Vorname = customerData.MA_Vorname
				wsWOSData.MA_Postfach = customerData.MA_Postfach
				wsWOSData.MA_Strasse = customerData.MA_Strasse
				wsWOSData.MA_PLZ = customerData.MA_PLZ
				wsWOSData.MA_Ort = customerData.MA_Ort
				wsWOSData.MA_Land = customerData.MA_Land
				wsWOSData.MA_Filiale = customerData.MA_Filiale
				wsWOSData.MA_Berater = customerData.MA_Berater
				wsWOSData.MA_Email = customerData.MA_Email
				wsWOSData.MA_AGB_Wos = customerData.MA_AGB_Wos
				wsWOSData.MA_Beruf = customerData.MA_Beruf
				wsWOSData.MA_Branche = customerData.MA_Branche
				wsWOSData.MA_Language = customerData.MA_Language

				wsWOSData.MA_Gender = customerData.MA_Gender
				wsWOSData.MA_BriefAnrede = customerData.MA_BriefAnrede
				wsWOSData.MA_Berater = customerData.MA_Berater
				wsWOSData.MA_Beruf = customerData.MA_Beruf
				wsWOSData.MA_Branche = customerData.MA_Branche
				wsWOSData.MA_AGB_Wos = customerData.MA_AGB_Wos
				wsWOSData.MA_GebDat = customerData.MA_GebDat

				wsWOSData.MA_FSchein = customerData.MA_FSchein
				wsWOSData.MA_Auto = customerData.MA_Auto
				wsWOSData.MA_Kontakt = customerData.MA_Kontakt
				wsWOSData.MA_State1 = customerData.MA_State1
				wsWOSData.MA_State2 = customerData.MA_State2
				wsWOSData.MA_Eigenschaft = customerData.MA_Eigenschaft
				wsWOSData.MA_SSprache = customerData.MA_SSprache
				wsWOSData.MA_MSprache = customerData.MA_MSprache
				wsWOSData.AHV_Nr = customerData.AHV_Nr
				wsWOSData.MA_Canton = customerData.MA_Canton
				wsWOSData.SignTransferedDocument = WOSSetting.SignTransferedDocument


				m_Logger.LogDebug("sendig customer wos document data to webservice...")

				exportResult.Value = InternService.AddAssignedEmployeeWOSDocument(m_InitializationData.MDData.MDGuid, wsWOSData)
				exportResult.Message = If(exportResult.Value, "", "Kein bekannter Fehler.")


				result = (exportResult)


			Catch ex As Exception
				result.Value = False
				result.Message = ex.ToString
				m_Logger.LogError(ex.ToString())

			End Try

			Return result

		End Function


#End Region


	End Class


End Namespace