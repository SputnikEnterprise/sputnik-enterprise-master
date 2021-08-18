

Imports O2S.Components.PDF4NET.PDFFile

Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraGrid.Columns

Imports SP.RP.ShowScannedDoc.FileHandling
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.RP.ShowScannedDoc.ClsDataDetail


Public Class ClsDbFunc
  Implements IClsDbRegister

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsLog As New SPProgUtility.ClsEventLog


	Private m_reportDBInformation As New DBInformation

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SPProgUtility.MainUtilities.Utilities

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


#Region "Contructor"

	Public Sub New()

		m_Utility = New SPProgUtility.MainUtilities.Utilities
		m_UtilityUI = New UtilityUI

	End Sub

	Public Sub New(ByVal strOriginFileGuid As String, ByVal strSelectedFileID As Integer)

		m_reportDBInformation.OriginFileGuid = strOriginFileGuid
		m_reportDBInformation.SelectedFileID = strSelectedFileID

	End Sub


#End Region


	Function LoadScannedData() As IEnumerable(Of ScanedData) Implements IClsDbRegister.LoadScannedData

		Dim result As List(Of ScanedData) = Nothing

		Dim sql As String
		sql = "[Get List of Scanned Reports]"


		Try
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDGuid", m_InitialData.MDData.MDGuid))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			If (Not reader Is Nothing) Then
				result = New List(Of ScanedData)
				Dim firstLine As Boolean = True

				While reader.Read

					Dim data = New ScanedData()

					If firstLine Then
						If m_Utility.SafeGetInteger(reader, "CountOfEmployeeDoc", 0) > 0 Then
							result.Add(New ScanedData With {.File_ScannedOn = String.Format(m_Translate.GetSafeTranslationValue("Kandidatendokumente ({0})"),
																																							m_Utility.SafeGetInteger(reader, "CountOfEmployeeDoc", 0)), .ImportedFileGuild = "0"})
						End If
						If m_Utility.SafeGetInteger(reader, "CountOfCustomerDoc", 0) > 0 Then
							result.Add(New ScanedData With {.File_ScannedOn = String.Format(m_Translate.GetSafeTranslationValue("Kundendokumente ({0})"),
																																	m_Utility.SafeGetInteger(reader, "CountOfCustomerDoc", 0)), .ImportedFileGuild = "1"})
						End If

						firstLine = False

					End If
					data.ID = m_Utility.SafeGetInteger(reader, "ID", 0)
					data.File_ScannedOn = m_Utility.SafeGetString(reader, "File_ScannedOn")
					data.ImportedFileGuild = m_Utility.SafeGetString(reader, "ImportedFileGuid")

					result.Add(data)


				End While

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try

		Return result

	End Function

	Function LoadScannedContentData(ByVal strSelectedFileGuid As String) As IEnumerable(Of ScanedContentData) Implements IClsDbRegister.LoadScannedContentData

		Dim result As List(Of ScanedContentData) = Nothing


		Try
			Dim sql As String
			sql = "Select "
			sql &= "ID"
			sql &= ",File_ScannedOn"
			sql &= ",RecordNumber"
			sql &= ",KW"
			sql &= ",ImportedFileGuid"
			sql &= ",ModulNumber"
			sql &= ",DocumentCategoryNumber"
			sql &= ",RPLID "
			sql &= "From  [Sputnik ScanJobs].[Dbo].[RP.ScannedFileContent] "
			sql &= "Where MDGuid = @MDGuid "

			sql &= "And CheckedOn Is Null "
			If strSelectedFileGuid = "0" Then
				sql &= "And ModulNumber = 0 "
			ElseIf strSelectedFileGuid = "1" Then
				sql &= "And ModulNumber = 1 "
			Else
				sql &= "And [ImportedFileGuid] = @ImportedFileGuid "

			End If

			sql &= "Order By RecordNumber ASC, KW ASC"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDGuid", m_InitialData.MDData.MDGuid))
			listOfParams.Add(New SqlClient.SqlParameter("ImportedFileGuid", strSelectedFileGuid))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.GetScanDbConnString, sql, listOfParams)


			If (Not reader Is Nothing) Then
				result = New List(Of ScanedContentData)

				While reader.Read

					Dim data = New ScanedContentData()

					data.ID = m_Utility.SafeGetInteger(reader, "ID", 0)
					data.File_ScannedOn = m_Utility.SafeGetDateTime(reader, "File_ScannedOn", Nothing)
					data.ImportedFileGuild = m_Utility.SafeGetString(reader, "ImportedFileGuid")
					data.CalendarWeek = m_Utility.SafeGetInteger(reader, "KW", 0)
					data.RecordNumber = m_Utility.SafeGetInteger(reader, "RecordNumber", 0)
					data.RPLID = m_Utility.SafeGetInteger(reader, "RPLID", 0)
					data.DocumentCategoryNumber = m_Utility.SafeGetInteger(reader, "DocumentCategoryNumber", 0)
					data.ModulNumber = m_Utility.SafeGetInteger(reader, "ModulNumber", 0)

					result.Add(data)

				End While

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try


		Return result

	End Function

	Function LoadAssignedScannedContentData(ByVal recID As Integer) As ScanedContentData Implements IClsDbRegister.LoadAssignedScannedContentData

		Dim result As ScanedContentData = Nothing

		Try
			Dim sql As String
			sql = "Select "
			sql &= "ID"
			sql &= ",File_ScannedOn"
			sql &= ",RecordNumber"
			sql &= ",KW"
			sql &= ",RPLID"
			sql &= ",Monday"
			sql &= ",Sunday"
			sql &= ",FoundedCodeValue"
			sql &= ",Scan_Komplett"
			sql &= ",IsValid"
			sql &= ",CheckedOn"
			sql &= ",RPMonth"
			sql &= ",ImportedFileGuid"
			sql &= ",ModulNumber"
			sql &= ",DocumentCategoryNumber "

			sql &= "From  [Sputnik ScanJobs].[Dbo].[RP.ScannedFileContent] "
			sql &= "Where ID = @recID "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recID", recID))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.GetScanDbConnString, sql, listOfParams)


			result = New ScanedContentData
			If (Not reader Is Nothing AndAlso reader.Read()) Then

				result.ID = m_Utility.SafeGetInteger(reader, "ID", 0)
				result.File_ScannedOn = m_Utility.SafeGetDateTime(reader, "File_ScannedOn", Nothing)
				result.ImportedFileGuild = m_Utility.SafeGetString(reader, "ImportedFileGuid")

				result.CalendarWeek = m_Utility.SafeGetInteger(reader, "KW", 0)
				result.Monday = m_Utility.SafeGetDateTime(reader, "Monday", Nothing)
				result.Sunday = m_Utility.SafeGetDateTime(reader, "Sunday", Nothing)
				result.Scan_Komplett = m_Utility.SafeGetByteArray(reader, "Scan_Komplett")
				result.CheckedOn = m_Utility.SafeGetDateTime(reader, "CheckedOn", Nothing)
				result.RPMonth = m_Utility.SafeGetInteger(reader, "RPMonth", 0)
				result.IsValid = m_Utility.SafeGetBoolean(reader, "IsValid", False)

				result.RecordNumber = m_Utility.SafeGetInteger(reader, "RecordNumber", 0)
				result.ModulNumber = m_Utility.SafeGetInteger(reader, "ModulNumber", 0)
				result.RPLID = m_Utility.SafeGetInteger(reader, "RPLID", 0)
				result.DocumentCategoryNumber = m_Utility.SafeGetInteger(reader, "DocumentCategoryNumber", 0)
				result.FoundedCodeValue = m_Utility.SafeGetString(reader, "FoundedCodeValue")

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try


		Return result

	End Function


	Function LoadAssignedScannedReport(ByVal data As DBInformation) As AssignedContentData Implements IClsDbRegister.LoadAssignedScannedReport
		Dim result As AssignedContentData = Nothing


		Dim sql As String = "Select Top 1 ID, DocScan, IsNull(RPNr, 0) As RPNr, IsNull(RPDoc_Guid, '') As RPDoc_Guid, "
		sql &= "IsNull(ESNr,0) As ESNr, IsNull(MANr, 0) As MANr, IsNull(KDNr, 0) As KDNr, IsNull(ScanExtension, 'PDF') As ScanExtension "
		sql &= "From [RP_ScanDoc] Where "
		sql &= "RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("RPNr", data.SelectedRecordNumber))
		listOfParams.Add(New SqlClient.SqlParameter("RPLNr", data.SelectedRPLNr))
		listOfParams.Add(New SqlClient.SqlParameter("MANr", data.EmployeeNumber))
		listOfParams.Add(New SqlClient.SqlParameter("KDNr", data.CustomerNumber))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams)

		Try

			result = New AssignedContentData
			If (Not reader Is Nothing AndAlso reader.Read()) Then

				result.ID = m_Utility.SafeGetInteger(reader, "ID", 0)
				result.DocScan = m_Utility.SafeGetByteArray(reader, "DocScan")

				result.RPNr = m_Utility.SafeGetInteger(reader, "RPNr", 0)
				result.ESNr = m_Utility.SafeGetInteger(reader, "ESNr", 0)
				result.MANr = m_Utility.SafeGetInteger(reader, "MANr", 0)
				result.KDNr = m_Utility.SafeGetInteger(reader, "KDNr", 0)

				result.RPDoc_Guid = m_Utility.SafeGetString(reader, "RPDoc_Guid")
				result.ScanExtension = m_Utility.SafeGetString(reader, "ScanExtension")

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try

		Return result

	End Function


	Function DeleteAssignedReport(ByVal data As DBInformation) As Boolean Implements IClsDbRegister.DeleteAssignedReport
		Dim success As Boolean = True

		Dim sql As String
		sql = "Delete [RP_ScanDoc] "
		sql &= "Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("RPNr", Data.SelectedRecordNumber))
		listOfParams.Add(New SqlClient.SqlParameter("RPLNr", Data.SelectedRPLNr))
		listOfParams.Add(New SqlClient.SqlParameter("MANr", data.EmployeeNumber))
		listOfParams.Add(New SqlClient.SqlParameter("KDNr", data.CustomerNumber))
		listOfParams.Add(New SqlClient.SqlParameter("ESNr", data.ESNumber))

		success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)
		Try
			If success AndAlso (_ClsProgSetting.GetKDWOSGuid.Length > 10 OrElse _ClsProgSetting.GetMAWOSGuid.Length > 10) Then
				Dim objWosInfo As New ClsWOSInfomation With {.SelectedDocGuid = data.OriginFileGuid}
				objWosInfo.SelectedMANr = data.EmployeeNumber
				objWosInfo.SelectedKDNr = data.CustomerNumber
				objWosInfo.SelectedMAGuid = data.EmployeeGuid
				objWosInfo.SelectedKDGuid = data.CustomerGuid
				objWosInfo.SelectedRPNr = data.SelectedRecordNumber
				objWosInfo.SelectedDocGuid = data.OriginFileGuid

				DeleteEmployeeReportDocumentFromWOS(objWosInfo)
				DeleteCustomerReportDocumentFromWOS(objWosInfo)
				'Dim clsWOS As New ClsWOS(objWosInfo)
				'clsWOS.DeleteDocFromWOS()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			success = False

		End Try

		Return success

	End Function

	Function UpdateAssignedReport(ByVal data As DBInformation) As Boolean Implements IClsDbRegister.UpdateAssignedReport
		Dim success As Boolean = True
		Dim sql As String

		If (_ClsProgSetting.GetKDWOSGuid.Length > 10 OrElse _ClsProgSetting.GetMAWOSGuid.Length > 10) AndAlso Not String.IsNullOrEmpty(data.OriginFileGuid) Then
			Dim objWosInfo As New ClsWOSInfomation With {.SelectedDocGuid = data.OriginFileGuid}
			objWosInfo.SelectedMANr = data.EmployeeNumber
			objWosInfo.SelectedKDNr = data.CustomerNumber
			objWosInfo.SelectedMAGuid = data.EmployeeGuid
			objWosInfo.SelectedKDGuid = data.CustomerGuid
			objWosInfo.SelectedRPNr = data.SelectedRecordNumber
			objWosInfo.SelectedDocGuid = data.OriginFileGuid

			DeleteEmployeeReportDocumentFromWOS(objWosInfo)
			DeleteCustomerReportDocumentFromWOS(objWosInfo)
		End If

		Dim strNewDocGuid As String = Guid.NewGuid.ToString
		data.OriginFileGuid = strNewDocGuid

		sql = "Declare @oldRPGuid nvarchar(50) = '' "
		sql &= "Select @oldRPGuid = IsNull(RPDoc_Guid, '') FROM dbo.RP_ScanDoc Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr; "

		sql &= "If @oldRPGuid = '' "
		sql &= "begin "
		sql &= "Set @oldRPGuid = NewID() "
		sql &= "End "

		sql &= "Delete dbo.[RP_ScanDoc] Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr; "

		sql &= "Insert Into dbo.[RP_ScanDoc] "
		sql &= "(RPNr, "
		sql &= "MANr, "
		sql &= "RecNr, "
		sql &= "Bezeichnung, "
		sql &= "Beschreibung, "
		sql &= "CreatedOn, "
		sql &= "CreatedFrom, "
		sql &= "DocScan, "
		sql &= "ScanExtension, "
		sql &= "KDNr, "
		sql &= "ESNr, "
		sql &= "RPLNr, "
		sql &= "RPDoc_Guid"

		sql &= ") Values ("

		sql &= "@RPNr, "
		sql &= "@MANr, "
		sql &= "1, "
		sql &= "@Bezeichnung, "
		sql &= "@Bezeichnung, "
		sql &= "GetDate(), "
		sql &= "@CreatedFrom, "
		sql &= "@DocScan, "
		sql &= "'PDF', "
		sql &= "@KDNr, "
		sql &= "@ESNr, "
		sql &= "@RPLNr, "
		sql &= "@oldRPGuid"
		sql &= ")"


		Try

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", m_Utility.ReplaceMissing(data.SelectedRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", m_Utility.ReplaceMissing(data.SelectedRPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", m_Utility.ReplaceMissing(data.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", m_Utility.ReplaceMissing(data.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", m_Utility.ReplaceMissing(data.ESNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", String.Format("{0} - {1}", CalendarWeek(data.CalendarWeek, Now.Year), CalendarWeek(data.CalendarWeek, Now.Year).AddDays(6))))

			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", _ClsProgSetting.GetUserLName))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", m_Utility.ReplaceMissing(data.DocScan, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("RPDoc_Guid", strNewDocGuid))


			success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)




			'If (_ClsProgSetting.bAllowedKDDocTransferTo_WS Or _ClsProgSetting.bAllowedMADocTransferTo_WS) AndAlso (data.SendRPToKDWOS Or data.SendRPToMAWOS) Then

			'	Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0,
			'		.SelectedESNr = data.ESNumber,
			'		.SelectedKDNr = data.CustomerNumber,
			'		.SelectedMANr = data.EmployeeNumber,
			'		.SelectedRPNr = data.SelectedRecordNumber,
			'		.SelectedRPLNr = data.SelectedRPLNr,
			'		.SelectedDocGuid = test.RPDoc_Guid,
			'		.SendRPToKDWOS = data.SendRPToKDWOS,
			'		.SendRPToMAWOS = data.SendRPToMAWOS}

			'	Dim clsWOS As New ClsWOS(objWosInfo)
			'	clsWOS.SendKDDoc2WOS()
			'End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			success = False
		Finally

		End Try


		Return success

	End Function

	Private Function DeleteEmployeeReportDocumentFromWOS(ByVal wosInfo As ClsWOSInfomation) As Boolean
		Dim result As Boolean = True
		If String.IsNullOrWhiteSpace(wosInfo.SelectedDocGuid) Then Return True
		Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(ClsDataDetail.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		m_Logger.LogDebug(String.Format("DeleteEmployeeReportDocumentFromWOS: SelectedMANr: {0} | SelectedMAGuid: {1} | SelectedRPNr: {2} | SelectedDocGuid: {3}", wosInfo.SelectedMANr, wosInfo.SelectedMAGuid, wosInfo.SelectedRPNr, wosInfo.SelectedDocGuid))

		wosSetting.EmployeeNumber = wosInfo.SelectedMANr ' m_CurrentEmployeeNumber
		wosSetting.EmployeeGuid = wosInfo.SelectedMAGuid '  m_CurrentEmployeeGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = wosInfo.SelectedRPNr ' m_CurrentPayrollNumber
		wosSetting.AssignedDocumentGuid = wosInfo.SelectedDocGuid ' m_CurrentPayrollGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", wosInfo.SelectedRPNr)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedEmployeeDocument()


		Return result

	End Function

	Private Function DeleteCustomerReportDocumentFromWOS(ByVal wosInfo As ClsWOSInfomation) As Boolean
		Dim result As Boolean = True
		If String.IsNullOrWhiteSpace(wosInfo.SelectedDocGuid) Then Return True
		Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(ClsDataDetail.m_InitialData)
		Dim wosSetting = New WOSSendSetting

		m_Logger.LogDebug(String.Format("DeleteCustomerReportDocumentFromWOS: SelectedKDNr: {0} | SelectedKDGuid: {1} | SelectedRPNr: {2} | SelectedDocGuid: {3}", wosInfo.SelectedKDNr, wosInfo.SelectedKDGuid, wosInfo.SelectedRPNr, wosInfo.SelectedDocGuid))

		wosSetting.CustomerNumber = wosInfo.SelectedKDNr
		wosSetting.CustomerGuid = wosInfo.SelectedKDGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = wosInfo.SelectedRPNr
		wosSetting.AssignedDocumentGuid = wosInfo.SelectedDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", wosInfo.SelectedRPNr)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedCustomerDocument()


		Return result

	End Function


	Function UpdateFileContentData(ByVal data As DBInformation) As Boolean Implements IClsDbRegister.UpdateFileContentData
		Dim success As Boolean = True

		Dim sql As String
		sql = "Update [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Set "
		sql &= "RecordNumber = @RecordNumber"
		sql &= ",DocumentCategoryNumber = @DocumentCategoryNumber"
		sql &= ",RPLID = @RPLID"
		sql &= ",KW = @calendarWeek"
		sql &= ",Monday = @Monday"
		sql &= ",Sunday = @Sunday"
		sql &= ",Scan_Komplett = @Scan_Komplett"
		sql &= ",IsValid = @ISValid"

		sql &= " Where ID = @ID"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("ID", m_Utility.ReplaceMissing(data.SelectedFileID, DBNull.Value)))

		listOfParams.Add(New SqlClient.SqlParameter("RPLID", m_Utility.ReplaceMissing(data.SelectedRPLID, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("calendarWeek", m_Utility.ReplaceMissing(data.CalendarWeek, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Monday", m_Utility.ReplaceMissing(data.RPLFrom, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Sunday", m_Utility.ReplaceMissing(data.RPLTo, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("ISValid", 1))

		listOfParams.Add(New SqlClient.SqlParameter("RecordNumber", data.SelectedRecordNumber))
		listOfParams.Add(New SqlClient.SqlParameter("DocumentCategoryNumber", m_Utility.ReplaceMissing(data.SelectedCategoryNumber, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("Scan_Komplett", m_Utility.ReplaceMissing(data.DocScan, DBNull.Value)))

		Try
			success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)
		Catch ex As Exception

		End Try



		Return success

	End Function


	Function LoadReportData() As IEnumerable(Of ReportData) Implements IClsDbRegister.LoadReportData
		Dim result As List(Of ReportData) = Nothing
		Try
			Dim sql As String
			sql = "[List RPData For Search In RPScanning]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			If (Not reader Is Nothing) Then
				result = New List(Of ReportData)

				While reader.Read

					Dim data = New ReportData()

					data.ReportNumber = m_Utility.SafeGetInteger(reader, "RPNr", 0)
					data.EmployeeName = m_Utility.SafeGetString(reader, "MAName")
					data.CustomerName = m_Utility.SafeGetString(reader, "Firma1")
					data.Von = m_Utility.SafeGetDateTime(reader, "Von", Nothing)
					data.Bis = m_Utility.SafeGetDateTime(reader, "Bis", Nothing)

					result.Add(data)

				End While

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try


		Return result

	End Function

	Function LoadReportLineData(ByVal reportNumber As Integer?) As IEnumerable(Of ReportLineData) Implements IClsDbRegister.LoadReportLineData
		Dim result As List(Of ReportLineData) = Nothing
		Try
			Dim sql As String
			sql = "[List KWData For Search In RPScanning]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", reportNumber))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			If (Not reader Is Nothing) Then
				result = New List(Of ReportLineData)

				While reader.Read

					Dim data = New ReportLineData()

					data.ID = m_Utility.SafeGetInteger(reader, "RPLID", 0)
					data.RPLNr = m_Utility.SafeGetInteger(reader, "RPLNr", Nothing)
					data.LANr = m_Utility.SafeGetDecimal(reader, "lanr", Nothing)
					data.VonDate = m_Utility.SafeGetDateTime(reader, "VonDate", Nothing)
					data.BisDate = m_Utility.SafeGetDateTime(reader, "BisDate", Nothing)

					result.Add(data)

				End While

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try


		Return result

	End Function

	Function LoadAssignedScanContentWithForImport() As IEnumerable(Of AssignedDataToImport) Implements IClsDbRegister.LoadAssignedScanContentWithForImport

		Dim result As List(Of AssignedDataToImport) = Nothing

		Try
			Dim sql As String
			sql = "[Get Match List Of Scanned Document With SpS]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDGuid", m_InitialData.MDData.MDGuid))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			If (Not reader Is Nothing) Then
				result = New List(Of AssignedDataToImport)

				While reader.Read

					Dim data = New AssignedDataToImport()

					data.ID = m_Utility.SafeGetInteger(reader, "ID", 0)
					data.File_ScannedOn = m_Utility.SafeGetDateTime(reader, "File_ScannedOn", Nothing)
					data.RecordNumber = m_Utility.SafeGetInteger(reader, "RecordNumber", Nothing)
					data.DocumentCategoryNumber = m_Utility.SafeGetInteger(reader, "DocumentCategoryNumber", Nothing)
					data.RecipientName = m_Utility.SafeGetString(reader, "RecipientName")

					data.ModulNumber = m_Utility.SafeGetInteger(reader, "ModulNumber", Nothing)
					data.IsSelected = True

					result.Add(data)

				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try


		Return result

	End Function

	Function LoadAssignedReportScanContentWithForImport(ByVal fileGuid As String) As IEnumerable(Of AssignedDataToImport) Implements IClsDbRegister.LoadAssignedReportScanContentWithForImport

		Dim result As List(Of AssignedDataToImport) = Nothing

		Try
			Dim sql As String
			sql = "[Get Match List Of Scanned RP With SpS]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDGuid", m_InitialData.MDData.MDGuid))
			listOfParams.Add(New SqlClient.SqlParameter("ImportedFileGuid", fileGuid))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			If (Not reader Is Nothing) Then
				result = New List(Of AssignedDataToImport)

				While reader.Read

					Dim data = New AssignedDataToImport()

					data.ID = m_Utility.SafeGetInteger(reader, "ID", 0)
					data.File_ScannedOn = m_Utility.SafeGetDateTime(reader, "File_ScannedOn", Nothing)

					data.RecordNumber = m_Utility.SafeGetInteger(reader, "RPNr", Nothing)
					data.CalendarWeek = m_Utility.SafeGetInteger(reader, "KW", Nothing)
					data.laoptext = m_Utility.SafeGetString(reader, "LAOpText")
					data.Period = m_Utility.SafeGetString(reader, "Von-Bis")
					data.ImportedFileGuild = m_Utility.SafeGetString(reader, "ImportedFileGuid")

					data.SP_RecordNumber = m_Utility.SafeGetInteger(reader, "sputnik Rapportnummer", Nothing)
					data.SP_Period = m_Utility.SafeGetString(reader, "sputnik von-bis")
					data.SP_RPLNumber = m_Utility.SafeGetInteger(reader, "SPRPLNr", Nothing)

					data.SP_EmployeeNumber = m_Utility.SafeGetInteger(reader, "SPMANr", Nothing)
					data.SP_CustomerNumber = m_Utility.SafeGetInteger(reader, "SPKDNr", Nothing)
					data.SP_EinsatzNumber = m_Utility.SafeGetInteger(reader, "SPESNr", Nothing)
					data.RecipientName = m_Utility.SafeGetString(reader, "Firma1")

					data.IsSelected = True
					data.ModulNumber = 3

					result.Add(data)

				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = Nothing

		End Try


		Return result

	End Function


	Function DeleteAssignedFileContent(ByVal recID As Integer) As Boolean Implements IClsDbRegister.DeleteAssignedFileContent
		Dim success As Boolean = True

		Dim sql As String
		sql = "[Delete Selected Scanned FileContent]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("ID", recID))

		success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure, False)

		Return success
	End Function

	Function AddAssignedEmployeeContentIntoFinalTable(ByVal data As AssignedDataToImport) As Boolean Implements IClsDbRegister.AddAssignedEmployeeContentIntoFinalTable
		Dim success As Boolean = True
		Dim newDocGuid As String = Guid.NewGuid.ToString

		Dim sql As String
		sql = "Insert Into [MA_LLDoc] ("
		sql &= "RecNr, "
		sql &= "MANr, "
		sql &= "Bezeichnung, "
		sql &= "Beschreibung, "
		sql &= "DocScan, "
		sql &= "ScanExtension, "
		sql &= "USNr, "
		sql &= "Categorie_Nr, "
		sql &= "CreatedOn, "
		sql &= "CreatedFrom "

		sql &= ") "

		sql &= "Select "
		sql &= "IsNull( (Select Top 1 RecNr From [MA_LLDoc] Where MANr = @MANr Order By RecNr DESC), 0) + 1, "
		sql &= "@MANr, "
		sql &= "FoundedCodeValue, "
		sql &= "FoundedCodeValue, "
		sql &= "Scan_Komplett, "
		sql &= "'PDF', "
		sql &= "@USNr, "
		sql &= "@Categorie_Nr, "
		sql &= "GetDate(), "
		sql &= "@CreatedFrom "
		sql &= "From [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Where ID = @ID; "

		sql &= "Update [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Set CheckedOn = GetDate() Where ID = @ID "

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("ID", data.ID))

		listOfParams.Add(New SqlClient.SqlParameter("MANr", data.RecordNumber))
		listOfParams.Add(New SqlClient.SqlParameter("USNr", m_InitialData.UserData.UserNr))
		listOfParams.Add(New SqlClient.SqlParameter("Categorie_Nr", data.DocumentCategoryNumber))

		listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", m_InitialData.UserData.UserFullName))

		success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)


		Return success

	End Function

	Function AddAssignedReportContentIntoFinalTable(ByVal data As AssignedDataToImport) As Boolean Implements IClsDbRegister.AddAssignedReportContentIntoFinalTable
		Dim success As Boolean = True
		Dim newDocGuid As String = Guid.NewGuid.ToString

		Dim sql As String
		sql = "Insert Into [RP_ScanDoc] ("
		sql &= "RecNr, "
		sql &= "RPNr, "
		sql &= "RPLNr, "
		sql &= "MANr, "
		sql &= "KDNr, "
		sql &= "ESNr, "
		sql &= "Bezeichnung, "
		sql &= "Beschreibung, "
		sql &= "DocScan, "
		sql &= "ScanExtension, "
		sql &= "RPDoc_Guid, "
		sql &= "CreatedOn, "
		sql &= "CreatedFrom "

		sql &= ") "

		sql &= "Select "
		sql &= "IsNull( (Select Top 1 RecNr From [RP_ScanDoc] Where RPNr = @recordNumber Order By RecNr DESC), 0) + 1, "
		sql &= "RPNr, "
		sql &= "@RPLNr, "
		sql &= "@MANr, "
		sql &= "@KDNr, "
		sql &= "@ESNr, "
		sql &= "(convert(nvarchar(10), Monday,104) + ' - ' + convert(nvarchar(10), sunday,104)) As Bez, "
		sql &= "(convert(nvarchar(10), Monday,104) + ' - ' + convert(nvarchar(10), sunday,104)) As Beschreibung, "
		sql &= "Scan_Komplett, "
		sql &= "'PDF', "
		sql &= "@RPDoc_Guid, "
		sql &= "GetDate(), "
		sql &= "@CreatedFrom "
		sql &= "From [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Where ID = @ID; "

		sql &= "Update [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Set CheckedOn = GetDate() Where ID = @ID "

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("ID", data.ID))

		listOfParams.Add(New SqlClient.SqlParameter("recordNumber", data.SP_RecordNumber))
		listOfParams.Add(New SqlClient.SqlParameter("RPLNr", data.SP_RPLNumber))
		listOfParams.Add(New SqlClient.SqlParameter("MANr", data.SP_EmployeeNumber))
		listOfParams.Add(New SqlClient.SqlParameter("KDNr", data.SP_CustomerNumber))
		listOfParams.Add(New SqlClient.SqlParameter("ESNr", data.SP_EinsatzNumber))
		listOfParams.Add(New SqlClient.SqlParameter("RPDoc_Guid", newDocGuid))

		listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", m_InitialData.UserData.UserFullName))

		success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)

		Try
			'If success AndAlso (_ClsProgSetting.GetKDWOSGuid.Length > 10 AndAlso data.SendToCustomerWOS) Then
			'	Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0,
			'																							 .SelectedESNr = data.SP_EinsatzNumber,
			'																							 .SelectedKDNr = data.SP_CustomerNumber,
			'																							 .SelectedMANr = data.SP_EmployeeNumber,
			'																							 .SelectedRPNr = data.SP_RecordNumber,
			'																							 .SelectedRPLNr = data.SP_RPLNumber,
			'																							 .SelectedDocGuid = newDocGuid,
			'																							 .SendRPToKDWOS = data.SendToCustomerWOS,
			'																							 .SendRPToMAWOS = data.SendToEmployeeWOS}
			'	Dim clsWOS As New ClsWOS(objWosInfo)
			'	clsWOS.SendKDDoc2WOS()
			'End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			success = False

		Finally

		End Try


		Return success

	End Function



	'Function ListAllRPContent4StoreIntoSPDb(ByVal strSelectedFileGuid As String) _
	'					As DataTable Implements IClsDbRegister.ListAllRPContent4StoreIntoSPDb
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim dt As New DataTable

	'	Try
	'		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetScanDbConnString)
	'		Conn.Open()

	'		Try
	'			Dim sSql As String = "Select ID, File_ScannedOn, RPNr, KW, "
	'			sSql &= "ImportedFileGuid From [RP.ScannedFileContent] "
	'			sSql &= "Where MDGuid = @MDGuid And [ImportedFileGuid] = @ImportedFileGuid And "
	'			sSql &= "(RPNr > 0 and RPNr Is not Null) And (KW > 0 and KW Is Not Null) Order By RPNr, KW"

	'			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'			cmd.CommandType = Data.CommandType.Text
	'			Dim param As System.Data.SqlClient.SqlParameter
	'			param = cmd.Parameters.AddWithValue("@MDGuid", m_InitialData.MDData.MDGuid)
	'			param = cmd.Parameters.AddWithValue("@ImportedFileGuid", strSelectedFileGuid)
	'			Dim ds As New DataSet

	'			Dim objAdapter As New SqlDataAdapter
	'			objAdapter.SelectCommand = cmd
	'			objAdapter.Fill(ds, "ScannedFileContent4Check")
	'			dt = ds.Tables(0)

	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("1. {0}.{1}", strMethodeName, ex.Message))

	'		End Try

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		'strResult = String.Format("Error: {0}", ex.InnerException)

	'	End Try

	'	Return dt
	'End Function


	Function StoreSelectedScannedFile2FS() As Dictionary(Of String, String) Implements IClsDbRegister.StoreSelectedScannedFile2FS
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim dResult As New Dictionary(Of String, String)
		If m_reportDBInformation.SelectedFileID = 0 Then Return dResult
		Dim ScanConn As New SqlConnection(ClsDataDetail.GetScanDbConnString)

		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim sMASql As String = "Select Scan_Komplett, IsNull(KW, 0) As KW, IsNull(RPNr, 0) As RPNr, IsNull(RPLID,0) As RPLID, "
		sMASql &= "IsNull(ESNr,0) As ESNr, IsNull(MANr, 0) As MANr, IsNull(KDNr, 0) As KDNr, Monday, Sunday, "
		sMASql &= "IsNull(IsValid, 2) As IsValid From  [Sputnik ScanJobs].[Dbo].[RP.ScannedFileContent] Where "
		sMASql &= String.Format("ID = {0} ", m_reportDBInformation.SelectedFileID)

		Dim i As Integer = 0

		ScanConn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, ScanConn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sMASql, ScanConn)

		Try

			strFullFilename = String.Format("{0}ScannedReport_{1}_{2}.pdf", _ClsProgSetting.GetSpSRPTempPath, _
																			 m_reportDBInformation.OriginFileGuid, m_reportDBInformation.SelectedFileID)
			dResult.Add("FileName".ToUpper, strFullFilename)
			Try
				Dim rScannrec As SqlDataReader = SQLCmd.ExecuteReader
				While rScannrec.Read
					dResult.Add("RPNr".ToUpper, rScannrec("RPNr").ToString)
					dResult.Add("KW".ToUpper, rScannrec("KW").ToString)
					dResult.Add("RPLID".ToUpper, rScannrec("RPLID").ToString)
					dResult.Add("StartOfWeek".ToUpper, rScannrec("Monday").ToString)
					dResult.Add("EndOfWeek".ToUpper, rScannrec("Sunday").ToString)

					dResult.Add("ESNr".ToUpper, rScannrec("ESNr").ToString)
					dResult.Add("MANr".ToUpper, rScannrec("MANr").ToString)
					dResult.Add("KDNr".ToUpper, rScannrec("KDNr").ToString)
					dResult.Add("IsValid".ToUpper, rScannrec("IsValid").ToString)

				End While
				rScannrec.Close()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.PDF-Dokument in die Datenbank übernehmen: {1}", strMethodeName, ex.Message))

			End Try
			If File.Exists(strFullFilename) Then Return dResult

			Try
				Try
					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
				Catch ex As Exception

				End Try
				If BA Is Nothing Then Return dResult

				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Datei in Filesystem schreiben: {1}", strMethodeName, ex.Message))
				_ClsLog.WriteToEventLog(String.Format("***StoreSelectedScannedFile2FS: {0}", ex.Message))
				MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "StoreSelectedScannedFile2FS")
				strFullFilename = String.Empty

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			_ClsLog.WriteToEventLog(String.Format("***StoreSelectedScannedFile2FS: {0}", ex.Message))
			strFullFilename = String.Empty

		End Try

		Return dResult
	End Function



#Region "Testfunktionen..."

	Public ReadOnly Property GetPDF4Net_O2SSerial() As String
		Get
			Return _ClsProgSetting.GetPDF_O2SSerial ' "yourlicencekey"
		End Get
	End Property

	Function Merg2PDFFiles(ByVal strFinalFile As String, ByVal aPDFFilename() As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Erfolgreich"

		Try
			PDFFile.SerialNumber = Me.GetPDF4Net_O2SSerial
			PDFFile.MergeFiles(strFinalFile, aPDFFilename, True)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			strResult = String.Format("Fehler: {0}", ex.Message)

		End Try


		Return strResult
	End Function

#End Region

End Class



