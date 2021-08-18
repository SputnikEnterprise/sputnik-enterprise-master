
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonXmlUtility
Imports System.IO
Imports System.Collections.Generic
Imports SP.DatabaseAccess.Customer
Imports System.Threading
Imports System.Text.RegularExpressions
Imports SP.Internal.Automations
Imports SP.Internal.Automations.SPCustomerPaymentServicesWebService
Imports System.Net

Public Class ClsFaxStart2

#Region "Private Consts"

	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "https://soap.ecall.ch/eCall.asmx"
	Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"

	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
	Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"
	Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx" ' "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"

	Private Const SPUTNIK_ECALL_FAX As String = "ECALL_FAXCREDIT"

#End Region

#Region "Private Fields"

	Protected m_UtilityUI As UtilityUI
	Private m_Logger As ILogger = New Logger()

	Private m_MandantSettingsXml As SettingsXml

	'''<summary>
	'''The mandant.
	'''</summary>
	Private m_MandantData As Mandant

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_eCallWebServiceUri As String

	'''<summary>
	'''Service Uri of eCall webservice.
	'''</summary>
	Private m_PaymentUtilWebServiceUri As String

	'''<summary>
	'''translate values
	'''</summary>
	''' <remarks></remarks>
	Private m_translate As TranslateValues

	Private m_eCallService As SPeCallWebService.eCallSoapClient

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath

	Private m_AccountName As String
	Private m_AccountPassword As String

	Private m_Attachments As List(Of String)

	Dim m_FromText As String
	Dim m_FaxHeaderID As String
	Dim m_FaxHeaderInfo As String
	Dim m_Subject As String
	Dim m_Notification As String
	Dim m_TokenFields As String
	Dim m_Message As String

#End Region

#Region "Constructor"

	Public Sub New(ByVal _Setting As InitializeClass)

		If _Setting.MDData Is Nothing Then
			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		Else
			ModulConstants.MDData = _Setting.MDData
			ModulConstants.UserData = _Setting.UserData
			ModulConstants.ProsonalizedData = _Setting.ProsonalizedData
			ModulConstants.TranslationData = _Setting.TranslationData

		End If

		m_translate = New TranslateValues
		m_MandantData = New Mandant

		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

		Try

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
			m_AccountName = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, ModulConstants.MDData.MDNr)))
			m_AccountPassword = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, ModulConstants.MDData.MDNr)))

			'm_PaymentUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI, ModulConstants.MDData.MDNr))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		m_eCallWebServiceUri = MANDANT_XML_SETTING_SPUTNIK_ECALL_URI

		Dim domainName = ModulConstants.MDData.WebserviceDomain
		m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)

		Try

			m_eCallService = New SPeCallWebService.eCallSoapClient
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
			m_eCallService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_eCallWebServiceUri)

			ClearAttachments()

			' TODO m_MandantSettingsXml ?
			m_FromText = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallfromtext", String.Empty)
			m_FaxHeaderID = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallheaderid", String.Empty)
			m_FaxHeaderInfo = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallheaderinfo", String.Empty)
			m_Subject = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallsubject", String.Empty)
			m_Notification = _ClsProgSetting.GetMDProfilValue("Mailing", "ecallnotification", String.Empty)
			m_TokenFields = _ClsProgSetting.GetMDProfilValue("Mailing", "ecalltokenfields", String.Empty)
			If String.IsNullOrWhiteSpace(m_TokenFields) Then m_TokenFields = "MaxRetries;=1"
			m_Message = String.Empty

			m_FromText = GetMessage(m_FromText)
			m_FaxHeaderID = GetMessage(m_FaxHeaderID)
			m_FaxHeaderInfo = GetMessage(m_FaxHeaderInfo)

		Catch ex As Exception

		End Try


	End Sub

#End Region

	Public Sub ClearAttachments()
		m_Attachments = New List(Of String)
	End Sub

	Public Sub AddAttachment(ByVal fileName As String)
		If File.Exists(fileName) Then
			Dim fsStream As New FileStream(fileName, FileMode.Open, FileAccess.Read)
			Dim objLeser As New BinaryReader(fsStream)
			Dim bDaten(CInt(fsStream.Length)) As Byte
			Dim attachment As String = Path.GetFileName(fileName) ' As New SPeCallWebService.Attachment

			Try
				objLeser.Read(bDaten, 0, bDaten.Length)
				'attachment.FileContent = bDaten
				'attachment.FileName = Path.GetFileName(fileName)
				If bDaten.Length > 0 Then m_Attachments.Add(attachment) 'attachment)

			Catch ex As Exception
				MsgBox(ex.Message)
			Finally
				If objLeser IsNot Nothing Then objLeser.Close()
				If fsStream IsNot Nothing Then fsStream.Close()
			End Try
		End If
	End Sub

	Public Sub AddAttachments(ByVal files As List(Of String))
		Dim i As Integer = 0
		For Each fileName As String In files
			AddAttachment(fileName)
		Next

	End Sub

	''' <summary>
	''' Send Fax to eCall Service
	''' </summary>
	Public Function SendFax(ByVal fax As FaxReceiver) As FaxReceiver

		Try
			Dim attachments(m_Attachments.Count - 1)
			Dim i As Integer = 0
			For Each attachment In m_Attachments
				attachments(i) = attachment
				i += 1
			Next

			Dim sendDate As String = Now.ToString
			Dim response = m_eCallService.SendFax(m_AccountName, m_AccountPassword, fax.Address, m_Message, fax.JobId, m_FromText, m_FaxHeaderID, m_FaxHeaderInfo, m_Subject, sendDate, attachments, m_Notification, m_TokenFields)

			fax.ResponseCode = response.ResponseCode
			fax.ResponseText = response.ResponseText

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

			fax.ResponseCode = 99999
			fax.ResponseText = ex.ToString()
		End Try

		Return fax

	End Function

	Public Function GetState(ByVal fax As FaxReceiver) As FaxReceiver

		Try
			' As SPeCallWebService.StatusResponse
			Dim response = m_eCallService.GetStateBasic(m_AccountName, m_AccountPassword, fax.JobId, Nothing)
			fax.PointsUsed = response.JobResponse.PointsUsed
			If response.ServiceResponse.ResponseCode <> 0 Then
				fax.ResponseCode = response.ServiceResponse.ResponseCode
				fax.ResponseText = response.ServiceResponse.ResponseText
			Else
				fax.SendState = response.JobResponse.SendState
				fax.ErrorState = response.JobResponse.ErrorState
				fax.FinishDate = response.JobResponse.FinishDate
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			fax.ResponseCode = 99999
			fax.ResponseText = ex.ToString()
		End Try

		Return fax

	End Function

	Public Sub LogPaymentService(ByVal jobId As String, ByVal pointsUsed As Double)

		Try

			Dim spCustomerServias As New SPCustomerPaymentServicesWebService.SPCustomerPaymentServicesSoapClient
			spCustomerServias.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_PaymentUtilWebServiceUri)

			' Log over web servcice 
			Dim success As Boolean = spCustomerServias.LogeCallUsage(ModulConstants.MDData.MDGuid, ModulConstants.UserData.UserGuid, ModulConstants.UserData.UserFullName, SPUTNIK_ECALL_FAX, jobId, pointsUsed.ToString(), DateTime.Now)

			If Not success Then
				m_Logger.LogError(String.Format("Sorry, I could not create log into remote server {0}! I'll try to write into local database!", m_PaymentUtilWebServiceUri))
				' Log over local database.
				LogFailedeCallUsageLogWebserviceCall(jobId)
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.StackTrace)
			' Log over local database.
			LogFailedeCallUsageLogWebserviceCall(jobId)
		End Try

	End Sub

	''' <summary>
	''' Logs a failed eCall Usage logging over webservice.
	''' </summary>
	Private Sub LogFailedeCallUsageLogWebserviceCall(ByVal jobId As String)

		Dim customerDBAccess As New CustomerDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
		Dim conStr As String = m_path.GetDbSelectData().MDDbConn

		Dim success As Boolean = customerDBAccess.LogSolvencyUsage(conStr,
																															 ModulConstants.MDData.MDGuid,
																															 ModulConstants.UserData.UserGuid,
																															 ModulConstants.UserData.UserFullName,
																															 SPUTNIK_ECALL_FAX,
																															 jobId,
																															 DateTime.Now)
		If Not success Then

			' Log in database did not work -> log solvency check usage in log file.
			m_Logger.LogError(String.Format("eCall Fax Usage could not be logged into database. customerGuid = {0}, userGuid = {1}, userName = {2}, CheckType = {3}, serviceDate = {4}, jobId = {5}",
																			ModulConstants.MDData.MDGuid,
																			ModulConstants.UserData.UserGuid,
																			ModulConstants.UserData.UserFullName,
																			SPUTNIK_ECALL_FAX,
																			DateTime.Now))
		End If


	End Sub


#Region "Helpers"

	Private Function GetMessage(ByVal receiver As String) As String
		Dim message As String = receiver

		' Search {@...}
		Dim regex As Regex = New Regex("\{@(\w+)\}", RegexOptions.IgnoreCase Or RegexOptions.Multiline)
		Dim matches As MatchCollection = regex.Matches(message)

		For Each match As Match In matches
			Dim pattern As String = match.Groups(0).Value
			Dim wildcard As String = match.Groups(1).Value

			Select Case wildcard.ToLower
				Case "UserMDName".ToLower
					message = message.Replace(pattern, ModulConstants.UserData.UserMDName)

				Case "UserMDTelefax".ToLower
					message = message.Replace(pattern, ModulConstants.UserData.UserMDTelefax)

				Case "UserTelefax".ToLower
					message = message.Replace(pattern, ModulConstants.UserData.UserTelefax)

			End Select

		Next

		Return message
	End Function

#End Region


End Class
