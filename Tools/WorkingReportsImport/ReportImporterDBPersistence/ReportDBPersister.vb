
Imports System.Data.SqlClient
Imports System.IO
'Imports ReportImporterCommon.Logging
Imports ReportImporterCommon.FileHandling
Imports ReportImporterCommon.StringExtensions
Imports ReportImporterCommon
Imports SP.Infrastructure.Logging
Imports ReportImporterCommon.CommonXmlUtility

''' <summary>
''' Report persister.
''' </summary>FileInfoExtensionMethods
Public Class ReportDBPersister
	Inherits DatabaseAccessBase
	Implements IReportDBPersister


#Region "Private consts"

	''' <summary>
	''' Maximal database text length.
	''' </summary>
	Public Const MAX_TEXT_LENGTH As Integer = 255

	Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
	Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
	Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
	Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"
	Private Const PROGRAM_XML_SETTING_EMAIL As String = "Settings/EMailSetting"
	Private Const PROGRAM_XML_SETTING_FTP As String = "Settings/FTPSetting"

#End Region


#Region "Private fields"

	''' <summary>
	''' The database connection string.
	''' </summary>
	Private m_ScanJobsConnectionString As String
	Private m_SystemInfoConnectionString As String
	''' <summary>
	''' Name of table that stores the complete report.
	''' </summary>
	Private TableName4PDFOriginFile As String = "[RP.ScannedFiles]"

	''' <summary>
	''' Name of table that stores the individual pages of a pdf.
	''' </summary>
	Private TableName4FileContent As String = "[RP.ScannedFileContent]"


	Private m_CommonConfigFolder As String
	Private m_SputnikFileServer As String
	Private m_ScanParserProgram As String
	Private m_EMailParserProgram As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml
	Private m_ProgSettingsXml As SettingsXml

	Private m_SettingFileName As String
	Private m_SettingFile As ProgramSettings
	Private m_NotifyUtility As Notification.Notifying

#End Region

	Public Property SettingFileValue As SettingFile


#Region "Constructor"

	Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
		MyBase.New(connectionString, translationLanguage)

	End Sub

	''' <summary>
	''' Constructor.
	''' </summary>
	''' <param name="connectionString">The connection string.</param>
	''' <param name="translationLanguage">The translation language.</param>
	''' <remarks></remarks>
	Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
		MyBase.New(connectionString, translationLanguage)
	End Sub


#End Region



#Region "Public methods"

	Public Function LoadSettingData(ByVal settingFileValue As ProgramSettings) As Boolean Implements IReportDBPersister.LoadSettingData
		Dim result As Boolean = True


		m_SettingFile = settingFileValue

		If m_SettingFile Is Nothing Then
			m_logger.LogError(String.Format("settingfile could not be readed!: {0}", m_SettingFile.CurrentSettingFilename))
		Else
			m_ScanJobsConnectionString = m_SettingFile.ConnstringScanjobs
			m_SystemInfoConnectionString = m_SettingFile.ConnstringSysteminfo
		End If
		m_NotifyUtility = New Notification.Notifying
		m_NotifyUtility.SettingFileData = m_SettingFile

		m_logger.LogInfo(String.Format("scanjob connection: {1}{0}systeminfo connection: {2}", vbNewLine, m_ScanJobsConnectionString, m_SystemInfoConnectionString))

		Return result
	End Function

	''' <summary>
	''' Saves complete report (multipage pdf) to database.
	''' </summary>
	''' <param name="reportDBInformation">The report information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Function SaveCompleteFile(ByVal reportDBInformation As ReportDBInformation) As Boolean Implements IReportDBPersister.SaveCompleteFile
		Dim con As New SqlConnection(m_ScanJobsConnectionString)

		' Checks if pdf file is existing.
		If Not (File.Exists(reportDBInformation.PDFFullPath)) Then
			m_logger.LogInfo(String.Format("File not founded: {0}", reportDBInformation.PDFFullPath))

			Return False
		End If
		Dim sql As String

		sql = "INSERT INTO [spScanJobs].Dbo.[tbl_Attachment] (Customer_ID, BusinessBranchNumber, ImportedFileGuid, ScanContent, CreatedOn) "
		sql &= "VALUES (@CustomerID, @BusinessBranchNumber, @ImportedFileGuid, @FileContent, GetDate())"

		Dim cmd As SqlCommand = New SqlCommand(sql, con)

		' Fill the parameter placeholders.
		cmd.Parameters.AddWithValue("@CustomerID", If(String.IsNullOrEmpty(reportDBInformation.MDGuid), DBNull.Value, reportDBInformation.MDGuid.TruncateLength(MAX_TEXT_LENGTH)))
		cmd.Parameters.AddWithValue("@BusinessBranchNumber", ReplaceMissing(reportDBInformation.BusinessBranch, DBNull.Value))
		cmd.Parameters.AddWithValue("@ImportedFileGuid", If(String.IsNullOrEmpty(reportDBInformation.OriginalFileGuid), Guid.NewGuid.ToString, reportDBInformation.OriginalFileGuid.TruncateLength(50)))

		Try
			Dim strNewfilename As String = reportDBInformation.PDFFullPath
			Try
				strNewfilename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & New FileInfo(reportDBInformation.PDFFullPath).Name
				File.Copy(reportDBInformation.PDFFullPath, strNewfilename, True)
			Catch ex As Exception
				m_logger.LogError(String.Format("Error: Filecopy: {0}. {1} >>> {2}", ex.ToString(), reportDBInformation.PDFFullPath, strNewfilename))
			End Try

			Dim fileInfo = New FileInfo(strNewfilename)
			Dim pdfBytes = fileInfo.ToByteArray()

			If pdfBytes Is Nothing Then
				Return False
			End If

			cmd.Parameters.AddWithValue("@FileContent", pdfBytes)

			con.Open()
			' Execute the query
			cmd.ExecuteNonQuery()
			If reportDBInformation.PDFFullPath.ToUpper <> strNewfilename Then
				Try
					File.Delete(strNewfilename)
				Catch ex As Exception
					m_logger.LogError(String.Format("Error: file delete from personal-folder: {0}. {1}", ex.ToString(), strNewfilename))

				End Try

			End If


		Catch ex As Exception
			m_logger.LogError(String.Format("Error: {0}", ex.ToString()))

			Return False
		Finally
			con.Close()
		End Try

		Return True
	End Function

	Function SendNotificationForImportedFiles(ByVal reportDBInformation As ReportDBInformation, ByVal notifyData As NotificationData) As Boolean Implements IReportDBPersister.SendNotificationForImportedFiles
		Dim result As Boolean = True
		Dim sql As String

		sql = "[Add Notify Data]"

		Dim con As New SqlConnection(m_SystemInfoConnectionString)
		Dim cmd As SqlCommand = New SqlCommand(sql, con)
		cmd.CommandType = CommandType.StoredProcedure

		Try
			Dim createdFrom As String = "ReportDbPersister.SendNotificationForImportedFiles"
			Dim header As String = "Scan-Import"
			Dim comments As String = "Ein gesscantes Dokument wurde importiert.{0}Anzahlseiten: {1}{0}"
			comments &= "Dateiname: {2}{0}"
			comments &= "Datei-Kennung: {3}{0}"
			comments = String.Format(comments,
									 vbNewLine,
									 reportDBInformation.Pages,
									 Path.GetFileName(reportDBInformation.PDFFullPath),
									 reportDBInformation.OriginalFileGuid)
			', reportDBInformation.ScanModulType)
			Dim notifyArt As Integer = 5

			If Not String.IsNullOrWhiteSpace(notifyData.CreatedFrom) Then createdFrom = notifyData.CreatedFrom
			If Not String.IsNullOrWhiteSpace(notifyData.NotifyHeader) Then header = notifyData.NotifyHeader
			If Not String.IsNullOrWhiteSpace(notifyData.NotifyComments) Then comments = notifyData.NotifyComments
			If notifyData.NotifyArt.GetValueOrDefault(0) = 0 Then notifyArt = notifyData.NotifyArt

			' Fill the parameter placeholders.
			cmd.Parameters.AddWithValue("CustomerID", ReplaceMissing(notifyData.Customer_ID, DBNull.Value))
			cmd.Parameters.AddWithValue("NotifyHeader", header)
			cmd.Parameters.AddWithValue("NotifyComments", comments)
			cmd.Parameters.AddWithValue("NotifyArt", notifyArt)
			cmd.Parameters.AddWithValue("CreatedFrom", createdFrom)

			con.Open()
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			m_logger.LogError(ex.ToString)
			result = False
		End Try

		Return result
	End Function


	''' <summary>
	''' Saves report information to database.
	''' </summary>
	''' <param name="reportDBInformation">The report information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function SaveData(ByVal reportDBInformation As ReportDBInformation) As Boolean Implements IReportDBPersister.SaveData
		Dim success = True

		If reportDBInformation.ScanModulType = ReportImporterDBPersistence.ReportDBInformation.ScanTypes.reports Then
			success = success AndAlso SaveWebServerReportData(reportDBInformation)

		Else
			success = success AndAlso SaveScanData(reportDBInformation)

		End If


		Return success

	End Function

	''' <summary>
	''' Saves the others scaned files into db (not reports!).
	''' </summary>
	''' <param name="data">The report information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Private Function SaveScanData(ByVal data As ReportDBInformation) As Boolean
		'Dim isNewDb As Boolean = IsConnectionNewDb

		' Check for an valid datamatrix.
		'Dim isDatamatrixOK As Integer? = data.IsDataMatrixCodeValid
		Try

			If Not data.IsDataMatrixCodeValid Then
				' Save the data.
				m_logger.LogError(String.Format("No valid datamatrix: MDGuid: {0} | MatrixCode: {1} | Filename: {2} | RecordNumber: {3} | RecordType:{4}",
																									 data.MDGuid, data.DataMatrixCodeValue, data.PDFFullPath, data.ModulRecordNumber, data.ScanModulType))
				SendNotificationMailForImportedFiles(data)
			End If

			Dim con As New SqlConnection(m_ScanJobsConnectionString)
			Dim sql As String

			sql = "INSERT INTO [spScanJobs].Dbo.[tbl_ScannedFile] ("
			sql &= "Customer_ID, BusinessBranchNumber, FoundedCodeValue, "
			sql &= "ModulNumber, RecordNumber, DocumentCategoryNumber, "
			sql &= "IsValid, ImportedFileGuid, "
			sql &= "CreatedOn, CreatedFrom "
			sql &= ") VALUES ("
			sql &= "@CustomerID, "
			sql &= "@BusinessBranchNumber, "
			sql &= "@FoundedCodeValue, "
			sql &= "@ModulNumber, "
			sql &= "@RecordNumber, "
			sql &= "@DocumentCategoryNumber, "
			sql &= "@IsValid, "
			sql &= "@ImportedFileGuid, "
			sql &= "GetDate(), "
			sql &= "'System'"
			sql &= ") "


			Dim cmd As SqlCommand = New SqlCommand(sql, con)

			' Fill the parameter placeholders
			cmd.Parameters.AddWithValue("@CustomerID", If(String.IsNullOrEmpty(data.MDGuid), DBNull.Value, data.MDGuid.TruncateLength(MAX_TEXT_LENGTH)))
			cmd.Parameters.AddWithValue("@BusinessBranchNumber", ReplaceMissing(data.BusinessBranch, DBNull.Value))
			cmd.Parameters.AddWithValue("@ModulNumber", data.ScanModulType)
			cmd.Parameters.AddWithValue("@RecordNumber", If(Not data.ModulRecordNumber.HasValue, DBNull.Value, data.ModulRecordNumber))
			cmd.Parameters.AddWithValue("@DocumentCategoryNumber", If(Not data.DocumentCategoryNumber.HasValue, DBNull.Value, data.DocumentCategoryNumber))
			cmd.Parameters.AddWithValue("@FoundedCodeValue", If(data.IsDataMatrixCodeValid, data.DataMatrixCodeValue, DBNull.Value))
			cmd.Parameters.AddWithValue("@IsValid", If(data.IsDataMatrixCodeValid, 1, 2))
			cmd.Parameters.AddWithValue("@ImportedFileGuid", If(String.IsNullOrEmpty(data.OriginalFileGuid), Guid.NewGuid.ToString, data.OriginalFileGuid.TruncateLength(50)))


			Try
				con.Open()
				' Execute the query
				cmd.ExecuteNonQuery()

			Catch ex As Exception
				m_logger.LogError(String.Format("Error: {0}", ex.ToString()))
				Return False

			Finally
				con.Close()
			End Try

			Return True


		Catch ex As Exception
			m_logger.LogError(String.Format("Error: {0}", ex.ToString))
			Return False

		End Try

		Return False

	End Function

	Function LoadMandantData(ByVal mdGuid As String) As MandantData Implements IReportDBPersister.LoadMandantData
		Dim result As MandantData = Nothing

		Dim sql As String

		sql = "Select ID"
		sql &= ",Customer_ID"
		sql &= ",Customer_Name"
		sql &= ",Recipients"
		sql &= ",Report_Recipients"
		sql &= ",bccAddresses"
		sql &= ",MailSender"
		sql &= ",MailUserName"
		sql &= ",MailPassword"
		sql &= ",SmtpServer"
		sql &= ",SmtpPort"
		sql &= ",ActivateSSL"
		sql &= ",TemplateFolder "
		sql &= " FROM [spScanJobs].dbo.tbl_Setting "
		sql &= "Where Customer_ID = @mdGuid And (Recipients <> '' And Recipients Is Not Null)"

		Dim con As New SqlConnection(m_ScanJobsConnectionString)
		Dim cmd As SqlCommand = New SqlCommand(sql, con)

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("mdGuid", ReplaceMissing(mdGuid, DBNull.Value)))

		Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)


		Try
			result = New MandantData

			If (Not reader Is Nothing AndAlso reader.Read()) Then

				result.ID = SafeGetInteger(reader, "ID", Nothing)
				result.Customer_ID = SafeGetString(reader, "Customer_ID")
				result.Customer_Name = SafeGetString(reader, "Customer_Name")
				result.Recipients = SafeGetString(reader, "Recipients")
				result.Report_Recipients = SafeGetString(reader, "Report_Recipients")
				result.bccAddresses = SafeGetString(reader, "bccAddresses")
				result.MailSender = SafeGetString(reader, "MailSender")
				result.MailUserName = SafeGetString(reader, "MailUserName")
				result.MailPassword = SafeGetString(reader, "MailPassword")
				result.SmtpServer = SafeGetString(reader, "SmtpServer")
				result.SmtpPort = SafeGetInteger(reader, "SmtpPort", 25)
				result.ActivateSSL = SafeGetBoolean(reader, "ActivateSSL", False)
				result.TemplateFolder = SafeGetString(reader, "TemplateFolder")

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			result = Nothing

		Finally
			CloseReader(reader)
		End Try


		Return result

	End Function

	Private Function SendNotificationMailForImportedFiles(ByVal reportDBInformation As ReportDBInformation) As Boolean
		Dim result As Boolean = True

		Dim mandantData = LoadMandantData(reportDBInformation.MDGuid)
		If mandantData Is Nothing OrElse mandantData.ID.GetValueOrDefault(0) = 0 Then Return result


		Dim strMailSender As String = mandantData.MailSender
		Dim streMailRecipient As String = mandantData.Recipients
		Dim strbcceMail As String = mandantData.bccAddresses

		Dim strMailUsername As String = mandantData.MailUserName
		Dim strMailPW As String = mandantData.MailPassword

		Dim strSMTPServer As String = mandantData.SmtpServer
		Dim strSMTPPort As String = mandantData.SmtpPort

		If Not String.IsNullOrWhiteSpace(streMailRecipient) Then
			Dim strBody = String.Empty
			Dim errorBody As String = "{0}:: {1}: Dokumente wurden eingescannt aber DataMatrix-Code wurden nicht erkannt! Möglicherweise wurde kein DataMatrix-Code geliefert."
			Dim successBody As String = "{0}:: {1}: Dokumente wurden eingescannt."
			If reportDBInformation.IsDataMatrixCodeValid Then
				strBody = String.Format(successBody, Now, mandantData.Customer_Name)
			Else
				strBody = String.Format(errorBody, Now, mandantData.Customer_Name)
			End If

			Dim strSubject = "Dokument-Scanning"
			Try
				Dim filename As String = reportDBInformation.PDFFullPath

				result = m_NotifyUtility.SendMailToWithExchange(strMailSender, streMailRecipient, strbcceMail, strSubject, strBody, New List(Of String) From {filename})
				m_Logger.LogDebug(String.Format("Sendresult: sending mailnotification to user: {0}", result))


			Catch ex As Exception
				m_Logger.LogError(String.Format("error: sending mailnotification to user: {0}", ex.ToString()))

			End Try

		End If

		Return result
	End Function

	''' <summary>
	''' Saves report information to database.
	''' </summary>
	''' <param name="data">The report information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Private Function SaveWebServerReportData(ByVal data As ReportDBInformation) As Boolean

		' Check for an existing report.
		' die gleichen Rapporte dürfen mehrfach gespeichert werden!!!
		Dim isDatamatrixOK As Integer? = data.IsDataMatrixCodeValid
		Try

			If isDatamatrixOK.HasValue Then

				' Save the data.
				Dim con As New SqlConnection(m_ScanJobsConnectionString)

				' Stores report information into the database. 
				Dim sql As String
				sql = "INSERT INTO [spScanJobs].Dbo.[tbl_ScannedFile] ("
				sql &= "Customer_ID, BusinessBranchNumber, FoundedCodeValue, "
				sql &= "ModulNumber, RecordNumber, DocumentCategoryNumber, "
				sql &= "IsValid, ImportedFileGuid, CreatedOn, CreatedFrom "
				sql &= ") VALUES ("
				sql &= "@CustomerID, "
				sql &= "@BusinessBranchNumber, "
				sql &= "@FoundedCodeValue, "
				sql &= "@ModulNumber, "
				sql &= "@RecordNumber, "
				sql &= "@DocumentCategoryNumber, "
				sql &= "@IsValid, "
				sql &= "@ImportedFileGuid, "
				sql &= "GetDate(), "
				sql &= "'System'"
				sql &= "); "

				sql &= "INSERT INTO [spScanJobs].Dbo.[tbl_ScannedReport] ("
				sql &= "Customer_ID, ImportedFileGuid, FoundedCodeValue, "
				sql &= "RecordNumber, "
				sql &= "IsValid, "
				sql &= "ReportYear, "
				sql &= "ReportMonth, "
				sql &= "ReportWeek, "
				sql &= "ReportFirstDay, "
				sql &= "ReportLastDay, "
				sql &= "ReportLineID, "
				sql &= "ScanContent, "
				sql &= "CreatedOn, CreatedFrom "
				sql &= ") VALUES ("
				sql &= "@CustomerID, "
				sql &= "@ImportedFileGuid, "
				sql &= "@FoundedCodeValue, "
				sql &= "@RecordNumber, "
				sql &= "@IsValid, "
				sql &= "@ReportYear, "
				sql &= "@ReportMonth, "
				sql &= "@ReportWeek, "
				sql &= "@ReportFirstDay, "
				sql &= "@ReportLastDay, "
				sql &= "@ReportLineID, "
				sql &= "@ReportScan, "
				sql &= "GetDate(), "
				sql &= "'System'"
				sql &= ") "


				Dim cmd As SqlCommand = New SqlCommand(sql, con)

				' Fill the parameter placeholders
				cmd.Parameters.AddWithValue("@CustomerID", ReplaceMissing(data.MDGuid, DBNull.Value))
				cmd.Parameters.AddWithValue("@BusinessBranchNumber", ReplaceMissing(data.BusinessBranch, DBNull.Value))
				cmd.Parameters.AddWithValue("@ModulNumber", data.ScanModulType)
				cmd.Parameters.AddWithValue("@RecordNumber", ReplaceMissing(data.ModulRecordNumber, DBNull.Value))
				cmd.Parameters.AddWithValue("@DocumentCategoryNumber", ReplaceMissing(data.DocumentCategoryNumber, DBNull.Value))
				cmd.Parameters.AddWithValue("@FoundedCodeValue", ReplaceMissing(data.DataMatrixCodeValue, DBNull.Value))
				cmd.Parameters.AddWithValue("@IsValid", If(data.IsDataMatrixCodeValid, 1, 2))
				cmd.Parameters.AddWithValue("@ImportedFileGuid", ReplaceMissing(data.OriginalFileGuid, Guid.NewGuid.ToString))

				cmd.Parameters.AddWithValue("@ReportYear", ReplaceMissing(data.Year, 0))
				cmd.Parameters.AddWithValue("@ReportMonth", ReplaceMissing(data.Month, 0))
				cmd.Parameters.AddWithValue("@ReportWeek", ReplaceMissing(data.CalendarWeek, 0)) ' If(Not data.CalendarWeek.HasValue, 0, Math.Min(data.CalendarWeek.Value, 53)))
				cmd.Parameters.AddWithValue("@ReportFirstDay", ReplaceMissing(data.FirstRPDay, 0))
				cmd.Parameters.AddWithValue("@ReportLastDay", ReplaceMissing(data.LastRPDay, 0))
				cmd.Parameters.AddWithValue("@ReportLineID", ReplaceMissing(data.RepportLineID, 0))

				Dim fileInfo = New FileInfo(data.PDFFullPath)
				Dim pdfBytes = fileInfo.ToByteArray()

				If pdfBytes Is Nothing Then
					Return False
				End If

				cmd.Parameters.AddWithValue("@ReportScan", pdfBytes)


				Try
					con.Open()
					' Execute the query
					cmd.ExecuteNonQuery()

				Catch ex As Exception
					m_logger.LogError(String.Format("Fehler: {0}", ex.ToString()))
					Return False

				Finally
					con.Close()
				End Try

				Return True

			Else
				m_logger.LogError(String.Format("The report with MDGuid {0}, ReportNr {1} and Calendar Week {2} was already imported.", data.MDGuid, data.ReportNumber, data.CalendarWeek))
			End If

		Catch ex As Exception
			m_logger.LogError(String.Format("Fehler: {0}", ex.ToString))
			Return False

		End Try

		Return False

	End Function

	''' <summary>
	''' Saves report information to database.
	''' </summary>
	''' <param name="data">The report information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Private Function SaveReportData(ByVal data As ReportDBInformation) As Boolean
		'Dim isNewDb As Boolean = IsConnectionNewDb

		' Check for an existing report.
		' die gleichen Rapporte dürfen mehrfach gespeichert werden!!!
		Dim isDatamatrixOK As Integer? = data.IsDataMatrixCodeValid
		Try

			If isDatamatrixOK.HasValue Then

				' Save the data.
				Dim con As New SqlConnection(m_ScanJobsConnectionString)

				' Stores report information into the database. 
				Dim sql As String
				sql = "INSERT INTO {0} (MDName, MDGuid, ModulNumber, RecordNumber, DocumentCategoryNumber, RPNr, RPMonth, KW, Monday, Sunday, "
				sql &= "FoundedCodeValue, Scan_Komplett, IsValid, ImportedFileGuid, File_ScannedOn) "
				sql &= "VALUES (IsNull((Select Top 1 Customer_Name From [tblMySetting] Where RP_Guid = @CustomerID), ''), "
				sql &= "@CustomerID, 3, @RecordNumber, 0, @RecordNumber, @ReportMonth, "
				sql &= "@ReportWeek, @ReportFirstDay, @ReportLastDay, "
				sql &= "@FoundedCodeValue, @ReportScan, @IsValid, @ImportedFileGuid, GetDate()) "

				sql = String.Format(sql, TableName4FileContent)

				Dim cmd As SqlCommand = New SqlCommand(sql, con)

				' Fill the parameter placeholders
				cmd.Parameters.AddWithValue("@CustomerID", If(String.IsNullOrEmpty(data.MDGuid), DBNull.Value, data.MDGuid.TruncateLength(MAX_TEXT_LENGTH)))
				cmd.Parameters.AddWithValue("@RecordNumber", If(Not data.ModulRecordNumber.HasValue, 0, data.ModulRecordNumber))
				cmd.Parameters.AddWithValue("@ReportWeek", If(Not data.CalendarWeek.HasValue, 0, Math.Min(data.CalendarWeek.Value, 53)))

				Dim dMonday As Date = CDate("1.1.1900")
				Dim dSunday As Date = CDate("1.1.1900")
				Dim iYear As Short = If(data.CalendarWeek.HasValue And data.Year.HasValue, data.Year.Value, 0)
				Dim iMonth As Short = If(data.CalendarWeek.HasValue And data.Month.HasValue, data.Month.Value, 0)
				m_logger.LogDebug(String.Format("Values: {0} | {1}", iYear, iMonth))
				If iYear = 0 Then iYear = 1900
				If iMonth = 0 Then iMonth = 1
				Dim dEndofMonth As Date = New DateTime(iYear, iMonth, 1).AddMonths(1).AddDays(-1)

				If data.CalendarWeek.HasValue And data.Year.HasValue Then
					If data.FirstRPDay.HasValue Then
						dMonday = CDate(data.FirstRPDay.Value & "." & data.Month.Value & "." & data.Year.Value)
						m_logger.LogDebug(String.Format("dMonday with value: {0}", dMonday))
					Else
						dMonday = DateUtils.DateOfMonday(CInt(Val(Math.Min(data.CalendarWeek.Value, 53))), data.Year.Value)
						m_logger.LogDebug(String.Format("dMonday with no value: {0}", dMonday))
					End If
					If data.LastRPDay.HasValue Then
						dSunday = CDate(data.LastRPDay.Value & "." & data.Month.Value & "." & data.Year.Value)
						m_logger.LogDebug(String.Format("dSunday with value: {0}", dSunday))
					Else
						dSunday = dMonday.AddDays(6)
						m_logger.LogDebug(String.Format("dSunday with no value: {0}", dSunday))
					End If
					iMonth = Month(dMonday)

					If dMonday < CDate(String.Format("01.{0}.{1}", iMonth, iYear)) Then
						dMonday = CDate(String.Format("01.{0}.{1}", iMonth, iYear))
					End If

					If dSunday > dEndofMonth Then
						dSunday = dEndofMonth
					End If

					cmd.Parameters.AddWithValue("@ReportFirstDay", Format(dMonday, "d"))
					cmd.Parameters.AddWithValue("@ReportLastDay", Format(dSunday, "d"))
				Else
					cmd.Parameters.AddWithValue("@Monday", DBNull.Value)
					cmd.Parameters.AddWithValue("@Sunday", DBNull.Value)
				End If
				cmd.Parameters.AddWithValue("@ReportMonth", iMonth)
				cmd.Parameters.AddWithValue("@FoundedCodeValue", If(data.IsDataMatrixCodeValid, data.DataMatrixCodeValue, DBNull.Value))
				cmd.Parameters.AddWithValue("@IsValid", If(data.IsDataMatrixCodeValid, 1, 2))
				cmd.Parameters.AddWithValue("@ImportedFileGuid", If(String.IsNullOrEmpty(data.OriginalFileGuid), Guid.NewGuid.ToString, data.OriginalFileGuid.TruncateLength(50)))

				Try
					Dim fileInfo = New FileInfo(data.PDFFullPath)
					Dim pdfBytes = fileInfo.ToByteArray()

					If pdfBytes Is Nothing Then
						Return False
					End If

					cmd.Parameters.AddWithValue("@ReportScan", pdfBytes)

					con.Open()
					' Execute the query
					cmd.ExecuteNonQuery()

				Catch ex As Exception
					m_logger.LogError(String.Format("Fehler: {0}", ex.ToString()))
					Return False

				Finally
					con.Close()
				End Try

				Return True

			Else
				m_logger.LogError(String.Format("The report with MDGuid {0}, ReportNr {1} and Calendar Week {2} was already imported.",
																									 data.MDGuid, data.ReportNumber, data.CalendarWeek))
			End If

		Catch ex As Exception
			m_logger.LogError(String.Format("Fehler: {0}", ex.ToString))
			Return False

		End Try

		Return False

	End Function


#End Region

#Region "Protected Methods"

	''' <summary>
	''' Reads number of existing reports.
	''' </summary>
	''' <param name="reportDBInformation">The report information.</param>
	''' <returns>Number of existing reports or nothing on error.</returns>
	Protected Overridable Function ReadNumberOfExistingReports(ByVal reportDBInformation As ReportDBInformation) As Integer?

		Dim con As New SqlConnection(m_ScanJobsConnectionString)

		' Checks if report is already in the database
		Dim sqlCommandText As String = String.Format("SELECT COUNT(*) FROM {0} WHERE MDGuid = @MdGuid AND RPNr = @ReportNumber AND KW = @CalendarWeek", TableName4FileContent)
		Dim command As SqlCommand = New SqlCommand(sqlCommandText, con)

		' Fill the paramter placeholders.
		command.Parameters.AddWithValue("@MdGuid", reportDBInformation.MDGuid)
		command.Parameters.AddWithValue("@ReportNumber", reportDBInformation.ReportNumber)
		command.Parameters.AddWithValue("@CalendarWeek", reportDBInformation.CalendarWeek)

		Dim numberOfExistingReports As Integer? = Nothing

		Try
			con.Open()
			' Execute query.
			numberOfExistingReports = command.ExecuteScalar()
		Catch ex As Exception
			m_logger.LogError(String.Format("Fehler: {0}", ex.ToString))
			'logger.Log(ex.ToString(), ILogger.LogLevel.ErrorLevel)
		Finally
			con.Close()
		End Try

		Return numberOfExistingReports

	End Function

#End Region


#Region "Helpers"

	''' <summary>
	''' Returns a string or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetString(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a boolean or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetBoolean(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an integer or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetInt32(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an short integer or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetShort(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Short?) As Short?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetInt16(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an byte or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetByte(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Byte?) As Byte?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetByte(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a decimal or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetDecimal(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Decimal?) As Decimal?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDecimal(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If
	End Function

	Private Function CreateTemporaryWorkingFolders(ByVal settingFile As ProgramSettings) As Boolean

		Try
			If Not String.IsNullOrWhiteSpace(settingFile.CVLFolderTOWatch) AndAlso Not Directory.Exists(settingFile.CVLFolderTOWatch) Then Directory.CreateDirectory(settingFile.CVLFolderTOWatch)
			If Not String.IsNullOrWhiteSpace(settingFile.CVLFolderTOArchive) AndAlso Not Directory.Exists(settingFile.CVLFolderTOArchive) Then Directory.CreateDirectory(settingFile.CVLFolderTOArchive)
			If Not String.IsNullOrWhiteSpace(settingFile.CVLXMLFolder) AndAlso Not Directory.Exists(settingFile.CVLXMLFolder) Then Directory.CreateDirectory(settingFile.CVLXMLFolder)
			If Not String.IsNullOrWhiteSpace(settingFile.TemporaryFolder) AndAlso Not Directory.Exists(settingFile.TemporaryFolder) Then Directory.CreateDirectory(settingFile.TemporaryFolder)

		Catch ex As Exception
			m_logger.LogError(ex.ToString)
			Return False
		End Try

		Return True
	End Function

#End Region


End Class
