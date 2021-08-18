
Imports System.Data.SqlClient
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors

Imports System.IO
Imports SP.Infrastructure.Logging
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility

Public Class ClsDeleteRPData


#Region "private consts"

	Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

#End Region

#Region "Private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Protected m_UtilityUI As UtilityUI

	Protected m_Utility As SP.Infrastructure.Utility


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_EmployeeDataAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_CustomerDataAccess As ICustomerDatabaseAccess


	Private m_MandantData As Mandant

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml
	Private m_EmployeeWOSID As String
	Private m_CustomerWOSID As String

	Private m_EmployeeTransferedGuid As String
	Private m_CustomerTransferedGuid As String
	Private m_ReportTransferedGuid As String
	Private m_EmployeeNumber As Integer
	Private m_CustomerNumber As Integer
	Private m_EmploymentNumber As Integer
	Private m_ReportNumber As Integer

#End Region


#Region "constructor"

	Public Sub New()

		If ModulConstants.m_InitialData Is Nothing Then ModulConstants.m_InitialData = CreateInitialData(0, 0)

		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_InitializationData = ModulConstants.m_InitialData
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantData = New Mandant
		m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
		m_EmployeeWOSID = EmployeeWOSID
		m_CustomerWOSID = CustomerWOSID


		Dim conStr = m_InitializationData.MDData.MDDbConn
		m_EmployeeDataAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDataAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

	End Sub

#End Region

#Region "private properties"

	Private ReadOnly Property EmployeeWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property

	Private ReadOnly Property CustomerWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property

#End Region

	Function DeleteSelectedRPFromDb(ByVal _setting As ClsRPDataSetting) As String
		Dim result As Boolean = True
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		'Dim Conn As SqlConnection = New SqlConnection(ModulConstants.m_InitialData.MDData.MDDbConn)
		Dim strGeschlecht As String = String.Empty
		Dim strMAAnrede As String = String.Empty
		Dim strNachname As String = String.Empty
		Dim strVorname As String = String.Empty

		Dim _iRPNr As Integer = _setting.SelectedRPNr
		Dim _iESNr As Integer = _setting.SelectedESNr
		Dim _iMANr As Integer = _setting.SelectedMANr
		Dim _iKDNr As Integer = _setting.SelectedKDNr

		m_EmployeeNumber = _setting.SelectedMANr
		m_CustomerNumber = _setting.SelectedKDNr
		m_EmploymentNumber = _setting.SelectedESNr
		m_ReportNumber = _setting.SelectedRPNr

		If _iKDNr = 0 Or _iMANr = 0 Or _iRPNr = 0 Then Throw New Exception("Keine Rapporte wurde gefunden.")

		Dim employeeData = m_EmployeeDataAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
		Dim customerData = m_CustomerDataAccess.LoadCustomerMasterData(m_CustomerNumber, "%%")
		If employeeData Is Nothing Then Throw New Exception("Error: Der Kandidat wurde nicht gefunden...")

		strGeschlecht = employeeData.Gender ' "Geschlecht") 
		strNachname = employeeData.Lastname ' rFoundedrec("Nachname")
		strVorname = employeeData.Firstname ' rFoundedrec("Vorname") 
		m_EmployeeTransferedGuid = employeeData.Transfered_Guid ' rFoundedrec("Transfered_Guid")
		m_CustomerTransferedGuid = customerData.Transfered_Guid

		strMAAnrede = String.Format(m_Translate.GetSafeTranslationValue(If(UCase(strGeschlecht = "M"), "Herr", "Frau")) & " {0} {1}", strVorname, strNachname)

		Dim strMsg As String = "Mit diesem Vorgang löschen Sie den ausgewählten Rapport.{0}<b>Rapportnummer: {1}</b>{0}<b>Einsatznummer: {2}</b>{0}<b>KandidatIn: {3}</b>{0}{0}"
		strMsg &= "Möchten Sie wirklich mit dem Vorgang fortfahren?"
		strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, _iRPNr, _iESNr, strMAAnrede)
		If _setting.ShowMsgBox Then
			If m_UtilityUI.ShowYesNoDialog(strMsg, m_Translate.GetSafeTranslationValue("Rapport löschen")) = False Then
				Throw New Exception("Der Vorgang wurde abgebrochen.")
			End If
		End If


		Try
			Dim rFrec As SqlClient.SqlDataReader = GetSelectedRPData4DeletingRec(_setting)
			rFrec.Read()
			If rFrec.HasRows Then
				Dim strLONr As String = If(CInt(rFrec("LONr")) = 0, String.Empty, CStr(rFrec("LONr")))
				Dim strRENr As String = rFrec("RENr")
				m_ReportTransferedGuid = rFrec("RPDocGuid")
				strMsg = String.Empty

				If Not String.IsNullOrWhiteSpace(strLONr) Then strMsg = String.Format("Es sind folgende Lohnabrechnung mit Rapport verknüpft:{0}{1}", vbNewLine, strLONr)
				If Not String.IsNullOrWhiteSpace(strRENr) Then strMsg = String.Format("Es sind folgende Rechnungen mit Rapport verknüpft:{0}{1}", vbNewLine, strRENr)


				If String.IsNullOrWhiteSpace(strMsg) Then
					m_Logger.LogDebug(String.Format("m_ReportTransferedGuid: {0}", m_ReportTransferedGuid))

					If Not String.IsNullOrWhiteSpace(m_ReportTransferedGuid) AndAlso Not String.IsNullOrWhiteSpace(m_CustomerWOSID & m_CustomerWOSID) Then
						Try
							'result = result AndAlso DeleteEmployeeReportDocumentFromWOS(m_ReportNumber, m_ReportTransferedGuid)
							'result = result AndAlso DeleteCustomerReportDocumentFromWOS(m_ReportNumber, m_ReportTransferedGuid)
							For Each itm In m_ReportTransferedGuid.Split(","c)
								m_Logger.LogDebug(String.Format("DeleteEmployeeReportDocumentFromWOS: {0}", itm))
								result = result AndAlso DeleteEmployeeReportDocumentFromWOS(m_ReportNumber, itm)

								m_Logger.LogDebug(String.Format("DeleteCustomerReportDocumentFromWOS: {0}", itm))
								result = result AndAlso DeleteCustomerReportDocumentFromWOS(m_ReportNumber, itm)
							Next

						Catch ex As Exception
							m_Logger.LogError(String.Format("{0}.Datensätze im WOS löschen. {1}", strMethodeName, ex.ToString))

						End Try
					End If

					Try
						strResult = DeleteSelectedRPRec(_setting)

						If _setting.ShowMsgBox Then
							strMsg = String.Format("Ihre Daten wurden erfolgreich gelöscht.")
							XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Rapportdaten löschen"),
																	System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
																	System.Windows.Forms.MessageBoxDefaultButton.Button1)

						End If
						strResult = "Success"

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datensätze löschen. {1}", strMethodeName, ex.ToString))

					End Try

				Else
					strMsg &= String.Format("{0}Der Vorgang wird abgebrochen.", vbNewLine)
					XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Rapportdaten löschen"),
															System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
															System.Windows.Forms.MessageBoxDefaultButton.Button1)

				End If

			Else
				Throw New Exception(m_Translate.GetSafeTranslationValue("Die allgemeine Daten wurden nicht gefunden. Der Vorgang wird abgebrochen."))

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Rapportdetails auflisten. {1}", strMethodeName, ex.ToString))

		End Try

		Return strResult
	End Function

	Function GetSelectedRPData4DeletingRec(ByVal _setting As ClsRPDataSetting) As SqlClient.SqlDataReader
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim rFrec As SqlClient.SqlDataReader
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim sSql As String = "[Get RPData 4 Delete Selected RP]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = New System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", _setting.SelectedRPNr)

			cmd.CommandType = Data.CommandType.StoredProcedure
			rFrec = cmd.ExecuteReader


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			rFrec = Nothing

		End Try

		Return rFrec
	End Function

	Function DeleteSelectedRPRec(ByVal _setting As ClsRPDataSetting) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim sSql As String = "[Delete Selected RP Data In All Table]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = New System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", _setting.SelectedRPNr)
			param = cmd.Parameters.AddWithValue("@MANr", _setting.SelectedMANr)
			param = cmd.Parameters.AddWithValue("@KDNr", _setting.SelectedKDNr)
			param = cmd.Parameters.AddWithValue("@UserName", String.Format("{0}", ModulConstants.m_InitialData.UserData.UserFullName))

			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			strResult = String.Format("Error: {0}", ex.ToString)

		End Try

		Return strResult
	End Function

	Private Function DeleteEmployeeReportDocumentFromWOS(ByVal reportNumber As Integer, ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(ModulConstants.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.EmployeeNumber = m_EmployeeNumber
		wosSetting.EmployeeGuid = m_EmployeeTransferedGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = reportNumber
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", reportNumber)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedEmployeeDocument()


		Return result

	End Function

	Private Function DeleteCustomerReportDocumentFromWOS(ByVal reportNumber As Integer, ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(ModulConstants.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.CustomerNumber = m_CustomerNumber
		wosSetting.CustomerGuid = m_CustomerTransferedGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = reportNumber
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", reportNumber)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedCustomerDocument()


		Return result

	End Function


	'Function SaveFileIntoDb(ByVal strFile2Save As String) As String
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	'Dim strUSName As String = _ClsProgSetting.GetUserName()
	'	Dim Conn As New SqlConnection(ModulConstants.m_InitialData.MDData.MDDbConn)
	'	'Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
	'	Dim sSql As String = String.Empty
	'	Dim strResult As String = "Success..."

	'	sSql = "Update DeleteInfo Set DeletedDoc = @BinaryFile Where ID = (Select Top 1 [ID] From DeleteInfo Order By ID DESC)"
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		Conn.Open()
	'		cmd.Connection = Conn

	'		If strFile2Save <> String.Empty Then
	'			Dim myFile() As Byte = GetFileToByte(strFile2Save)
	'			Dim fi As New System.IO.FileInfo(strFile2Save)
	'			Dim strFileExtension As String = fi.Extension

	'			Try
	'				cmd.CommandType = CommandType.Text
	'				cmd.CommandText = sSql
	'				param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)

	'				cmd.Connection = Conn
	'				cmd.ExecuteNonQuery()
	'				cmd.Parameters.Clear()

	'			Catch ex As Exception
	'				strResult = String.Format("Error: {0}", ex.ToString)
	'				m_Logger.LogError(String.Format("{0}.Datei in die Datenbank schreiben. {1}", strMethodeName, ex.ToString))

	'			End Try
	'		End If

	'	Catch ex As Exception
	'		strResult = String.Format("Error: {0}", ex.ToString)
	'		m_Logger.LogError(String.Format("{0}.Datei in die Datenbank schreiben. {1}", strMethodeName, ex.ToString))

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Return strResult
	'End Function

	'Function GetFileToByte(ByVal filePath As String) As Byte()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
	'	Dim reader As BinaryReader = New BinaryReader(stream)

	'	Dim photo() As Byte = Nothing
	'	Try

	'		photo = reader.ReadBytes(CInt(stream.Length))
	'		reader.Close()
	'		stream.Close()

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

	'	End Try

	'	Return photo
	'End Function


#Region "helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

#End Region



End Class
