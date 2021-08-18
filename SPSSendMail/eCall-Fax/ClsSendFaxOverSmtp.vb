
Imports System.Xml.Linq

Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports System.Collections.Generic
Imports System.IO
Imports DevExpress.Utils
Imports System.Text.RegularExpressions

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.CommonXmlUtility
Imports SP.Internal.Automations.SPeCallWebService
Imports SP.Internal.Automations
Imports System.Net

Namespace Fax2eCall

	<Obsolete("This class is deprecated.")>
	Public Class ClsSendFaxOverSmtp


#Region "Private Fields"

		Private m_Logger As ILogger = New Logger()

		Private m_xml As New ClsXML
		Private m_md As Mandant
		Private m_path As ClsProgPath
		Private m_common As CommonSetting

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

		Private m_AccountName As String
		Private m_AccountPassword As String

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _FaxSetting As New ClsFaxSetting


		Private m_eCallService As SPeCallWebService.eCallSoapClient

#End Region


#Region "Private Consts"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "MD_{0}/Interfaces/webservices/webserviceecall"
		Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

		Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-contact-subject"
		Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY As String = "MD_{0}/Templates/ customer-contact-body"

		Public Const SPUTNIK_ECALL_SMS As String = "ECALL_SMSCREDIT"

#End Region


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsFaxSetting)
			Me._FaxSetting = _Setting

			m_MandantData = New Mandant

			Try

				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
				m_AccountName = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, ModulConstants.MDData.MDNr)))
				m_AccountPassword = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, ModulConstants.MDData.MDNr)))

				m_eCallWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_URI, ModulConstants.MDData.MDNr))
				m_PaymentUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI, ModulConstants.MDData.MDNr))

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			m_eCallService = New SPeCallWebService.eCallSoapClient
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
			m_eCallService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_eCallWebServiceUri)

		End Sub


#End Region

		Private ReadOnly Property GetAbsender4Faxversand() As String
			Get
				Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
				Dim strValue As String = _ClsProgSetting.GetMDProfilValue("Mailing", "faxforwarder", _ClsProgSetting.GetSelectedMDData(9))

				Return strValue
			End Get
		End Property

		Private ReadOnly Property GetFaxExtension() As String
			Get
				Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
				Dim strValue As String = _ClsProgSetting.GetMDProfilValue("Mailing", "faxextension", String.Empty)

				Return strValue
			End Get
		End Property

		'Private Function AddAttachments(ByVal liFiles As List(Of String)) As SPeCallWebService.Attachment()

		'	Dim aAttachments(liFiles.Count - 1) As SPeCallWebService.Attachment
		'	Dim strVollerName As String

		'	Dim iListe As Integer
		'	For iListe = 0 To liFiles.Count - 1
		'		strVollerName = liFiles(iListe).ToString
		'		If File.Exists(strVollerName) Then
		'			Dim fsStream As New FileStream(strVollerName, FileMode.Open, FileAccess.Read)
		'			Dim objLeser As New BinaryReader(fsStream)
		'			Dim bDaten(CInt(fsStream.Length)) As Byte
		'			aAttachments(iListe) = New SPeCallWebService.Attachment

		'			Try
		'				objLeser.Read(bDaten, 0, bDaten.Length)
		'				aAttachments(iListe).FileContent = bDaten
		'				aAttachments(iListe).FileName = Path.GetFileName(strVollerName)

		'			Catch ex As Exception
		'				MsgBox(ex.Message)

		'			Finally
		'				If Not (objLeser Is Nothing) Then objLeser.Close()
		'				If Not (fsStream Is Nothing) Then fsStream.Close()

		'			End Try
		'		End If
		'	Next

		'	Return aAttachments
		'End Function


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

	Public Class ClseCallState

#Region "Private Fields"

		Private m_Logger As ILogger = New Logger()

		Private m_xml As New ClsXML
		Private m_md As Mandant
		Private m_path As ClsProgPath
		Private m_common As CommonSetting

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

		Private m_AccountName As String
		Private m_AccountPassword As String


		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _FaxSetting As New ClsFaxSetting

		Private m_eCallService As SPeCallWebService.eCallSoapClient

#End Region


#Region "Private Consts"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "MD_{0}/Interfaces/webservices/webserviceecall"
		Public Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"

		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
		Public Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"

		Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_SUBJECT As String = "MD_{0}/Templates/customer-contact-subject"
		Private Const MANDANT_XML_SETTING_CUSTOMER_CONTACT_BODY As String = "MD_{0}/Templates/ customer-contact-body"

		Public Const SPUTNIK_ECALL_SMS As String = "ECALL_SMSCREDIT"

#End Region

		Function GeteCallNotification(ByVal strAdress As String) As String
			Dim strResult As String = "Success"

			'm_eCallService = New SPeCallWebService.eCallSoapClient
			'Dim txtAccountName As String = _ClsProgSetting.DecryptString(_ClsProgSetting.GetMDProfilValue("Mailing", "faxusername", String.Empty))
			'Dim txtAccountPassword As String = _ClsProgSetting.DecryptString(_ClsProgSetting.GetMDProfilValue("Mailing", "faxuserpw", String.Empty))
			Dim txtJobID As String = _ClsProgSetting.GetMDProfilValue("Mailing", "ecalljobid", _ClsProgSetting.GetSelectedMDData(0)).Replace("-", "")
			txtJobID = String.Format("{0}|{1}", txtJobID, Me._FaxSetting.VersandGuid)

			Try
				Dim rStateMsg As StatusResponse

				rStateMsg = m_eCallService.GetStateBasic(m_AccountName, m_AccountPassword, txtJobID, strAdress)
				Trace.WriteLine(String.Format(TranslateText("Vor While-Schleife: Adresse: {0} | Meldung: {1}"), rStateMsg.JobResponse.Address, rStateMsg.JobResponse.SendState))
				While rStateMsg.JobResponse.SendState <> 41 Or rStateMsg.JobResponse.SendState <> 42
					rStateMsg = m_eCallService.GetStateBasic(m_AccountName, m_AccountPassword, txtJobID, strAdress)
					Trace.WriteLine(String.Format(TranslateText("Adresse: {0} | Meldung: {1}"), rStateMsg.JobResponse.Address, rStateMsg.JobResponse.SendState))
					If rStateMsg.JobResponse.SendState = 41 Then
						Exit While
					ElseIf rStateMsg.JobResponse.SendState = 42 Then
						Dim iResult As Integer = CInt(rStateMsg.JobResponse.ErrorState)
						Select Case iResult
							Case 6000, 6002, 6005, 6014, 6016, 6017
								Dim strErrMsg As String = ""
								If iResult = 6000 Then strErrMsg = "Unbekannter Fehler"
								If iResult = 6002 Then strErrMsg = "Fehler beim Konvertieren der Dokumente"
								If iResult = 6005 Then strErrMsg = "Fehler: Nichts zum Senden in diesem Auftrag. Kann keine JobID erstellen"
								If iResult = 6014 Then strErrMsg = "Verbindung vom Sender abgebrochen"
								If iResult = 6016 Then strErrMsg = "Dateiformat nicht unterstütz"
								If iResult = 6017 Then strErrMsg = "Keinen Dateizugriff"

								strResult = String.Format(TranslateText("Error: Adresse: {0} | Meldung: ({1}): {2} = {3}"),
																					rStateMsg.JobResponse.Address, rStateMsg.JobResponse.SendState,
																					rStateMsg.JobResponse.ErrorState, strErrMsg)
							Case Else
								strResult = String.Format(TranslateText("Success: Adresse: {0} | Meldung: ({1}): {2}"),
																					rStateMsg.JobResponse.Address, rStateMsg.JobResponse.SendState,
																					rStateMsg.JobResponse.ErrorState)

						End Select
						Return strResult

					ElseIf rStateMsg.JobResponse.SendState = 201 Then

					End If
				End While

			Catch ex As Exception
				Return String.Format("Error: {0}", ex.Message)

			End Try
			'Try
			'  sAntwort = objService.GetStateBasic(txtAccountName, txtAccountPassword, Me._FaxSetting.VersandGuid, String.Empty)
			'  strResult = "lblResponseCode: {0} | lblREsponseText: {1} | lblJobID: {2} | lblAddress: {3} | lblJobType: {4} | lblSendState: {5} | lblPointsUsed: {6} | lblFinishedDate: {7}"
			'  strResult = String.Format(strResult,
			'                            sAntwort.ServiceResponse.ResponseCode,
			'                            sAntwort.ServiceResponse.ResponseText,
			'                            sAntwort.JobResponse.JobID,
			'                            sAntwort.JobResponse.Address,
			'                            sAntwort.JobResponse.JobType,
			'                            sAntwort.JobResponse.SendState,
			'                            sAntwort.JobResponse.PointsUsed,
			'                            sAntwort.JobResponse.FinishDate.ToString)

			'  '     lblErrorState.Text = sAntwort.JobResponse.ErrorState

			'Catch ex As Exception
			'  MsgBox(ex.Message)
			'End Try

			Return strResult
		End Function

		Public Sub New(ByVal _Setting As ClsFaxSetting)
			Me._FaxSetting = _Setting

			m_MandantData = New Mandant

			Try

				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
				m_AccountName = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME, ModulConstants.MDData.MDNr)))
				m_AccountPassword = _ClsProgSetting.DecryptString(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW, ModulConstants.MDData.MDNr)))

				m_eCallWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_ECALL_URI, ModulConstants.MDData.MDNr))
				m_PaymentUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI, ModulConstants.MDData.MDNr))

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			m_eCallService = New SPeCallWebService.eCallSoapClient
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
			m_eCallService.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_eCallWebServiceUri)

		End Sub

	End Class

End Namespace
