
Imports System.Data.SqlClient

Imports System.Windows.Forms
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing

Namespace SPSESUtility


	Public Class ClsESFunktionality

#Region "private Consts"

		Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As SP.Infrastructure.Logging.ILogger = New SP.Infrastructure.Logging.Logger()


		Private m_md As Mandant
		Private m_translate As TranslateValues
		Private m_common As CommonSetting
		Private m_ProgPath As ClsProgPath
		Private m_MandantFormXMLFile As String


#Region "Contructor"

		Public Sub New()

			m_md = New Mandant
			m_common = New CommonSetting
			m_ProgPath = New ClsProgPath
			m_translate = New TranslateValues

			ModulConstants.m_InitialData = CreateInitialData(0, 0)
			InitializeObject()

			m_MandantFormXMLFile = m_md.GetSelectedMDFormDataXMLFilename(ModulConstants.m_InitialData.MDData.MDNr)

		End Sub

#End Region

		Public Shared Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Public Shared Sub InitializeObject()

			If ModulConstants.m_InitialData Is Nothing Then
				ModulConstants.m_InitialData = CreateInitialData(0, 0)
			End If

			ModulConstants.MDData = ModulConstants.m_InitialData.MDData ' ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.m_InitialData.UserData ' ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.m_InitialData.ProsonalizedData ' ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.m_InitialData.TranslationData   ' ModulConstants.TranslationValues

		End Sub


		Public Shared Function DeleteSelectedES(ByVal liESNr As List(Of Integer),
											ByVal bShowMsg As Boolean) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success..."
			Dim sSql As String = String.Empty
			InitializeObject()

			Dim m_translate As New TranslateValues

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sMonth As Short = Now.Month
			Dim iYear As Integer = Now.Year
			Dim iMANr As Integer = 0
			Dim iLMID As Integer = 0
			Dim iESNr As Integer = 0
			Dim iKDNr As Integer = 0
			Dim strLOGuid As String = String.Empty
			Dim strMAGuid As String = String.Empty
			Dim strESNr As String = String.Empty

			For i As Integer = 0 To liESNr.Count - 1
				strESNr &= If(strESNr.Length > 0, ",", "") & CStr(liESNr(i))
			Next
			sSql = "Select ES.ESNr, ES.MANr, ES.KDNr From ES "
			sSql &= "Where ES.ESNr In ({0}) Order By ESNr"
			sSql = String.Format(sSql, strESNr)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				cmd.CommandType = Data.CommandType.Text

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				While rFoundedrec.Read
					iESNr = CInt(rFoundedrec("ESNr"))
					iMANr = CInt(rFoundedrec("MANr"))
					iKDNr = CInt(rFoundedrec("KDNr"))

					Dim _setting As New ClsESDataSetting With {.SelectedESNr = iESNr,
															   .SelectedMANr = iMANr,
															   .SelectedKDNr = iKDNr,
															   .ShowMsgBox = bShowMsg}

					strResult = DeleteSelectedESFromDb(_setting)
					If strResult.ToLower.Contains("error") Then Exit While
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				If bShowMsg Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("{0}", ex.Message), m_translate.GetSafeTranslationValue("Einsatz löschen"))
				End If
				Return String.Format("{0}", ex.Message)

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return strResult
		End Function


	End Class


	Public Class EmploymentUtility


#Region "private Consts"

		Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "private fields"

		Private m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_connectionString As String

		''' <summary>
		''' The common data access object.
		''' </summary>
		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Private m_ListingDatabaseAccess As IListingDatabaseAccess

		Private m_mandant As SPProgUtility.Mandanten.Mandant

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility
		Private m_UtilityUI As UtilityUI

		Private m_common As CommonSetting
		Private m_ProgPath As ClsProgPath
		Private m_MandantFormXMLFile As String


		Private m_Showmessagebox As Boolean?
		Private m_EmploymentNumber As Integer?
		Private m_EmployeeNumber As Integer?
		Private m_CustomerNumber As Integer?


#End Region


#Region "Contructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_mandant = New SPProgUtility.Mandanten.Mandant

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(ModulConstants.m_InitialData.MDData.MDNr)

		End Sub

#End Region


#Region "public methods"

		Public Function DeleteSelectedES(ByVal liESNr As List(Of Integer), ByVal bShowMsg As Boolean) As Boolean
			Dim result As Boolean = True
			Dim sSql As String = String.Empty

			Dim sMonth As Short = Now.Month
			Dim iYear As Integer = Now.Year
			Dim iMANr As Integer = 0
			Dim iLMID As Integer = 0
			Dim iESNr As Integer = 0
			Dim iKDNr As Integer = 0
			Dim strLOGuid As String = String.Empty
			Dim strMAGuid As String = String.Empty
			Dim strESNr As String = String.Empty

			Dim data = m_ListingDatabaseAccess.LoadAssignedEmploymentsData(m_InitializationData.MDData.MDNr, liESNr)
			If data Is Nothing OrElse data.Count = 0 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Einsatzdaten konnten nicht geladen werden."))

				Return False
			End If
			m_Showmessagebox = bShowMsg

			Try
				For Each itm In data
					m_EmploymentNumber = itm.ESNR
					m_EmployeeNumber = itm.EmployeeNumber
					m_CustomerNumber = itm.CustomerNumber

					'Dim _setting As New ClsESDataSetting With {.SelectedESNr = m_EmploymentNumber,
					'								 .SelectedMANr = m_EmployeeNumber,
					'								 .SelectedKDNr = m_CustomerNumber,
					'								 .ShowMsgBox = bShowMsg}

					'result = DeleteSelectedESFromDb()
					If Not result Then Return False
				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				If bShowMsg Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("{0}", ex.Message), m_Translate.GetSafeTranslationValue("Einsatz löschen"))
				End If
				Return False

			Finally

			End Try

			Return result
		End Function

#End Region


#Region "private methods"

		'		Private Function DeleteSelectedESFromDb() As Boolean
		'			Dim result As Boolean = True
		'			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		'			Dim strGeschlecht As String = String.Empty
		'			Dim strMAAnrede As String = String.Empty
		'			Dim strNachname As String = String.Empty
		'			Dim strVorname As String = String.Empty

		'			'Dim _iESNr As Integer = _setting.SelectedESNr
		'			'Dim _iMANr As Integer = _setting.SelectedMANr
		'			'Dim _iKDNr As Integer = _setting.SelectedKDNr

		'			If m_CustomerNumber = 0 OrElse m_EmployeeNumber = 0 OrElse m_EmploymentNumber = 0 Then Throw New Exception("Keine Einsätze wurde gefunden.")
		'			Dim employeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)
		'			If employeData Is Nothing Then
		'				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kaniddaten Daten konnten nicht geladen werden."))

		'				Return False
		'			End If

		'			strGeschlecht = employeData.Gender
		'			strNachname = employeData.Lastname
		'			strVorname = employeData.Firstname
		'			strMAAnrede = String.Format(m_Translate.GetSafeTranslationValue(If(UCase(strGeschlecht = "M"), "Herr", "Frau")) & " {0}", employeData.EmployeeFullname)


		'			Dim strMsg As String = "Mit diesem Vorgang löschen Sie den ausgewählten Einsatz.{0}Einsatznummer: {1}{0}KandidatIn: {2}{0}{0}"
		'			strMsg &= "Möchten Sie wirklich mit dem Vorgang fortfahren?"
		'			strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, _iESNr, strMAAnrede)
		'			If m_Showmessagebox Then
		'				If m_UtilityUI.ShowYesNoDialog(strMsg, m_Translate.GetSafeTranslationValue("Einsatz löschen"), MessageBoxDefaultButton.Button1, MessageBoxIcon.Question) = False Then
		'					Throw New Exception(String.Format("Error: {0}", m_Translate.GetSafeTranslationValue("Der Vorgang wurde abgebrochen.")))
		'				End If
		'			End If

		'			Try
		'				Dim dependData = m_ListingDatabaseAccess.LoadAssignedEmploymentDependentData(m_InitializationData.MDData.MDNr, m_EmploymentNumber)

		'				If dependData Is Nothing  Then
		'					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler bei der Suche nach Einsatz-Abhängigkeiten."))

		'					Return False
		'				End If

		'				For Each itm In dependData

		'					Dim strLONr As String = itm.LONr
		'					Dim strRENr As String = itm.RENr
		'					Dim strLANr As String = itm.LANr
		'					Dim strESVertragDocGuid As String = itm.ESDoc_Guid
		'					Dim strVerleihDocGuid As String = itm.VerleihDoc_Guid
		'					Dim strRPDocGuid As String = itm.RPDoc_Guid

		'					strMsg = String.Empty

		'					If strLONr <> String.Empty Then
		'						strMsg = String.Format("Lohnabrechnung: {0}", strLONr)
		'					End If
		'					If strRENr <> String.Empty Then
		'						strMsg &= String.Format("{0}Rechnungen: {1}", If(strMsg = "", "", vbNewLine), strRENr)
		'					End If
		'					If strLANr <> String.Empty Then
		'						strMsg &= String.Format("{0}Monatliche Lohnangaben: {1}", If(strMsg = "", "", vbNewLine), strLANr)
		'					End If
		'					If strMsg <> String.Empty Then
		'						strMsg = String.Format("Mit Ihrem Einsatz sind folgende Daten verknüpft:{0}{1}", vbNewLine, strMsg)
		'					End If


		'					If strMsg = String.Empty Then
		'						Try
		'							If IsUserActionAllowed(ModulConstants.m_InitialData.UserData.UserNr, 302, ModulConstants.m_InitialData.MDData.MDNr) Then
		'								result = DeleteSelectedRPWithESNr(New List(Of Integer)(New Integer() {_setting.SelectedESNr}), bShowMsg:=False)
		'							Else
		'								strMsg = "Bitte stellen Sie sicher, dass Sie ausreichende Benutzerrechte besitzen, um Rapporte zu löschen."
		'								XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Einsatzdaten mit Rapporte löschen"),
		'										System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
		'										System.Windows.Forms.MessageBoxDefaultButton.Button1)
		'								Throw New Exception("error: " & m_Translate.GetSafeTranslationValue(strMsg))
		'							End If

		'						Catch ex As Exception
		'							m_Logger.LogError(String.Format("Einsatzrapporte löschen. {0}", ex.ToString))
		'							Return ex.ToString

		'						End Try
		'						Dim guidArray = New String() {strESVertragDocGuid, strVerleihDocGuid, strRPDocGuid}.ToArray()
		'						Dim strDocGuid As String = String.Join(",", guidArray.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
		'						If Not String.IsNullOrWhiteSpace(strDocGuid) AndAlso (_ClsProgSetting.bAllowedMADocTransferTo_WS() OrElse _ClsProgSetting.bAllowedKDDocTransferTo_WS()) Then
		'							Try
		'								ESUtility.ESSetting = _setting

		'								ESUtility.ESTemplateGuid = strESVertragDocGuid
		'								ESUtility.ESVerleihTemplateGuid = strVerleihDocGuid
		'								ESUtility.ReportScanGuids = strRPDocGuid.Split(","c).ToList

		'								ESUtility.DeleteDocFrom_WS(_iMANr, _iKDNr, _iESNr)

		'							Catch ex As Exception
		'								m_Logger.LogError(String.Format("Datensätze im WOS löschen. {0}", ex.ToString))

		'							End Try
		'						End If


		'						Try
		'							result = DeleteSelectedESRec(_setting)
		'							If m_Showmessagebox Then
		'								strMsg = String.Format("Ihre Daten wurden erfolgreich gelöscht.")
		'								XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Einsatzdaten löschen"),
		'																	System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
		'																	System.Windows.Forms.MessageBoxDefaultButton.Button1)
		'							End If

		'						Catch ex As Exception
		'							m_Logger.LogError(String.Format("Datensätze löschen. {0}", ex.ToString))

		'						End Try

		'					Else
		'						strMsg &= String.Format("{0}{0}Der Vorgang wird abgebrochen.", vbNewLine)
		'						XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Einsatzdaten löschen"),
		'															System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
		'															System.Windows.Forms.MessageBoxDefaultButton.Button1)
		'						Throw New Exception("error: " & m_Translate.GetSafeTranslationValue(strMsg))

		'					End If

		'				Next

		'				'Else
		'				'	strMsg = "Die allgemeinen Daten wurden nicht gefunden. Der Vorgang wird abgebrochen."
		'				'	Throw New Exception("error: " & m_Translate.GetSafeTranslationValue(strMsg))

		'				'End If

		'			Catch ex As Exception
		'				strMsg = String.Format("{0}", ex.Message)
		'				m_Logger.LogError(String.Format("Einsatzdetails auflisten. {0}", ex.ToString))
		'				result = strMsg
		'			End Try

		'			Return result
		'		End Function

		'		Private Function GetSelectedESData4DeletingRec() As SqlClient.SqlDataReader

		'			Try
		'				Conn.Open()

		'				Dim sSql As String = "[Get ESData 4 Delete Selected ES]"
		'				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		'				Dim param As System.Data.SqlClient.SqlParameter

		'				param = New System.Data.SqlClient.SqlParameter
		'				param = cmd.Parameters.AddWithValue("@ESNr", _setting.SelectedESNr)

		'				cmd.CommandType = Data.CommandType.StoredProcedure
		'				rFrec = cmd.ExecuteReader


		'			Catch ex As Exception
		'				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		'				rFrec = Nothing

		'			End Try

		'			Return rFrec
		'		End Function

		'		Function DeleteSelectedESRec(ByVal _setting As ClsESDataSetting) As String
		'			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'			Dim strResult As String = "Success..."
		'			Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

		'			Try
		'				Conn.Open()

		'				Dim sSql As String = "[Delete Selected ES Data In All Table]"
		'				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		'				Dim param As System.Data.SqlClient.SqlParameter

		'				param = New System.Data.SqlClient.SqlParameter
		'				param = cmd.Parameters.AddWithValue("@ESNr", _setting.SelectedESNr)
		'				param = cmd.Parameters.AddWithValue("@MANr", _setting.SelectedMANr)
		'				param = cmd.Parameters.AddWithValue("@KDNr", _setting.SelectedKDNr)
		'				param = cmd.Parameters.AddWithValue("@UserName", String.Format("{0} {1}", _ClsProgSetting.GetUserFName, _ClsProgSetting.GetUserLName))

		'				cmd.CommandType = Data.CommandType.StoredProcedure
		'				cmd.ExecuteNonQuery()

		'			Catch ex As Exception
		'				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		'				strResult = String.Format("Error: {0}", ex.Message)

		'			End Try

		'			Return strResult
		'		End Function

		'		Private Function SaveFileIntoDb(ByVal strFile2Save As String) As String
		'			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'			Dim strUSName As String = _ClsProgSetting.GetUserName()
		'			Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
		'			Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
		'			Dim sSql As String = String.Empty
		'			Dim strResult As String = "Success..."

		'			sSql = "Update DeleteInfo Set DeletedDoc = @BinaryFile Where ID = (Select Top 1 [ID] From DeleteInfo Order By ID DESC)"
		'			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		'			Dim param As System.Data.SqlClient.SqlParameter

		'			Try
		'				Conn.Open()
		'				cmd.Connection = Conn

		'				If strFile2Save <> String.Empty Then
		'					Dim myFile() As Byte = GetFileToByte(strFile2Save)
		'					Dim fi As New System.IO.FileInfo(strFile2Save)
		'					Dim strFileExtension As String = fi.Extension

		'					Try
		'						cmd.CommandType = CommandType.Text
		'						cmd.CommandText = sSql
		'						param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)

		'						cmd.Connection = Conn
		'						cmd.ExecuteNonQuery()
		'						cmd.Parameters.Clear()

		'					Catch ex As Exception
		'						strResult = String.Format("Error: {0}", ex.Message)
		'						m_Logger.LogError(String.Format("{0}.Datei in die Datenbank schreiben. {1}", strMethodeName, ex.Message))

		'					End Try
		'				End If

		'			Catch ex As Exception
		'				strResult = String.Format("Error: {0}", ex.Message)
		'				m_Logger.LogError(String.Format("{0}.Datei in die Datenbank schreiben. {1}", strMethodeName, ex.Message))

		'			Finally
		'				cmd.Dispose()
		'				Conn.Close()

		'			End Try

		'			Return strResult
		'		End Function

		'		Function GetFileToByte(ByVal filePath As String) As Byte()
		'			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'			Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
		'			Dim reader As BinaryReader = New BinaryReader(stream)

		'			Dim photo() As Byte = Nothing
		'			Try

		'				photo = reader.ReadBytes(CInt(stream.Length))
		'				reader.Close()
		'				stream.Close()

		'			Catch ex As Exception
		'				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		'			End Try

		'			Return photo
		'		End Function

#End Region


	End Class

End Namespace


