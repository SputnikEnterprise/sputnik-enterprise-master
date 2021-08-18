

Imports O2S.Components.PDF4NET.PDFFile

Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraGrid.Columns

Imports SP.RP.ShowScannedDoc.FileHandling
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
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


	'Function GetDbScannedFiles() As DataTable Implements IClsDbRegister.GetDbScannedFiles
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim ds As New DataSet
	'	Dim dt As New DataTable

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim sSql As String = "[Get List of Scanned Reports]"
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sSql, Conn)
	'	cmd.CommandType = CommandType.StoredProcedure

	'	Try
	'		Dim objAdapter As New SqlDataAdapter
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		param = cmd.Parameters.AddWithValue("@MDGuid", m_InitialData.MDData.MDGuid)

	'		objAdapter.SelectCommand = cmd
	'		objAdapter.Fill(ds, "ScannedFiles")

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'	Return ds.Tables(0)
	'End Function

	'Function ListNewScannedFiles(ByVal strSelectedFileGuid As String) As DataTable Implements IClsDbRegister.ListNewScannedFiles
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim dt As New DataTable

	'	Try
	'		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetScanDbConnString)
	'		Conn.Open()

	'		Try
	'			Dim sSql As String = "Select ID, File_ScannedOn, RPNr, KW, (convert(nvarchar(10), RPNr) + '_' + convert(nvarchar(2), KW)) As RPData, "
	'			sSql &= "ImportedFileGuid From [RP.ScannedFileContent] "
	'			sSql &= "Where MDGuid = @MDGuid And [ImportedFileGuid] = @ImportedFileGuid And CheckedOn Is Null Order By RPNr ASC, KW ASC"

	'			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'			cmd.CommandType = Data.CommandType.Text
	'			Dim param As System.Data.SqlClient.SqlParameter
	'			param = cmd.Parameters.AddWithValue("@MDGuid", m_InitialData.MDData.MDGuid)
	'			param = cmd.Parameters.AddWithValue("@ImportedFileGuid", strSelectedFileGuid)
	'			Dim ds As New DataSet

	'			Dim objAdapter As New SqlDataAdapter
	'			objAdapter.SelectCommand = cmd
	'			objAdapter.Fill(ds, "ScannedFileContent")
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
			If success AndAlso (_ClsProgSetting.GetKDWOSGuid.Length > 10 Or _ClsProgSetting.GetMAWOSGuid.Length > 10) Then
				Dim objWosInfo As New ClsWOSInfomation With {.SelectedDocGuid = data.OriginFileGuid}
				Dim clsWOS As New ClsWOS(objWosInfo)

				clsWOS.DeleteDocFromWOS()
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
			Dim clsWOS As New ClsWOS(objWosInfo)

			clsWOS.DeleteDocFromWOS()
		End If

		Dim strNewDocGuid As String = Guid.NewGuid.ToString
		data.OriginFileGuid = strNewDocGuid

		sql = "Declare @oldRPGuid nvarchar(50) = '' "
		sql &= "Select @oldRPGuid = IsNull(RPDoc_Guid, '') FROM dbo.RP_ScanDoc Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr; "

		sql &= "If @oldRPGuid = '' "
		sql &= "begin "
		sql &= "Set @oldRPGuid = NewID() "
		sql &= "End "

		sql &= "Delete [RP_ScanDoc] Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr; "

		sql &= "Insert Into [RP_ScanDoc] "
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

			listOfParams.Add(New SqlClient.SqlParameter("Bezeichnung", String.Format("{0} - {1}", CalendarWeek(data.CalendarWeek, Now.Year),
																																				CalendarWeek(data.CalendarWeek, Now.Year).AddDays(6))))

			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", _ClsProgSetting.GetUserLName))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", m_Utility.ReplaceMissing(data.DocScan, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("RPDoc_Guid", strNewDocGuid))


			success = m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)

			Dim test = LoadAssignedScannedReport(data)
			If test Is Nothing Then Return False



			If (_ClsProgSetting.bAllowedKDDocTransferTo_WS Or _ClsProgSetting.bAllowedMADocTransferTo_WS) AndAlso (data.SendRPToKDWOS Or data.SendRPToMAWOS) Then
				'Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0,
				'																						 .SelectedESNr = data.ESNumber,
				'																						 .SelectedKDNr = data.CustomerNumber,
				'																						 .SelectedMANr = data.EmployeeNumber,
				'																						 .SelectedRPNr = data.SelectedRecordNumber,
				'																						 .SelectedRPLNr = data.SelectedRPLNr,
				'																						 .SelectedDocGuid = strNewDocGuid,
				'																						 .SendRPToKDWOS = data.SendRPToKDWOS,
				'																						 .SendRPToMAWOS = data.SendRPToMAWOS}
				Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0,
																										 .SelectedESNr = data.ESNumber,
																										 .SelectedKDNr = data.CustomerNumber,
																										 .SelectedMANr = data.EmployeeNumber,
																										 .SelectedRPNr = data.SelectedRecordNumber,
																										 .SelectedRPLNr = data.SelectedRPLNr,
																										 .SelectedDocGuid = test.RPDoc_Guid,
																										 .SendRPToKDWOS = data.SendRPToKDWOS,
																										 .SendRPToMAWOS = data.SendRPToMAWOS}
				Dim clsWOS As New ClsWOS(objWosInfo)
				clsWOS.SendKDDoc2WOS()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			success = False
		Finally

		End Try


		Return success

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
			If success AndAlso (_ClsProgSetting.GetKDWOSGuid.Length > 10 AndAlso data.SendToCustomerWOS) Then
				Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0,
																										 .SelectedESNr = data.SP_EinsatzNumber,
																										 .SelectedKDNr = data.SP_CustomerNumber,
																										 .SelectedMANr = data.SP_EmployeeNumber,
																										 .SelectedRPNr = data.SP_RecordNumber,
																										 .SelectedRPLNr = data.SP_RPLNumber,
																										 .SelectedDocGuid = newDocGuid,
																										 .SendRPToKDWOS = data.SendToCustomerWOS,
																										 .SendRPToMAWOS = data.SendToEmployeeWOS}
				Dim clsWOS As New ClsWOS(objWosInfo)
				clsWOS.SendKDDoc2WOS()
			End If


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



	'Function SaveChangedFileIntoDb(ByVal rpInfo As DBInformation) As Boolean Implements IClsDbRegister.SaveChangedFileIntoDb
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim Time_1 As Double = System.Environment.TickCount
	'	Dim strUSName As String = _ClsProgSetting.GetUserName()
	'	Dim Conn As New SqlConnection(ClsDataDetail.GetScanDbConnString)
	'	Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
	'	Dim sSql As String = String.Empty
	'	Dim strResult As String = String.Empty

	'	sSql = "Update [RP.ScannedFileContent] Set RPNr = @RPNr, RPLID = @RPLID, KW = @KW, Monday = @Monday, Sunday = @Sunday, "
	'	sSql &= "Scan_Komplett = @Scan_Komplett, IsValid = @ISValid Where ID = @ID"

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sSql, Conn)
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		If rpInfo.SelectedFileToSave <> String.Empty Then
	'			Try
	'				Dim fileInfo = New FileInfo(rpInfo.SelectedFileToSave)
	'				Dim pdfBytes = fileInfo.ToByteArray()

	'				If pdfBytes Is Nothing Then
	'					Return False
	'				End If

	'				param = cmd.Parameters.AddWithValue("@RPNr", rpInfo.SelectedRecordNumber)
	'				param = cmd.Parameters.AddWithValue("@RPLID", rpInfo.SelectedRPLID)
	'				param = cmd.Parameters.AddWithValue("@KW", rpInfo.CalendarWeek)
	'				param = cmd.Parameters.AddWithValue("@Monday", rpInfo.RPLFrom)
	'				param = cmd.Parameters.AddWithValue("@Sunday", rpInfo.RPLTo)
	'				param = cmd.Parameters.AddWithValue("@Scan_Komplett", pdfBytes)
	'				param = cmd.Parameters.AddWithValue("@ISValid", 1)
	'				param = cmd.Parameters.AddWithValue("@ID", Me.m_reportDBInformation.SelectedFileID)

	'				Conn.Open()
	'				cmd.Connection = Conn
	'				cmd.ExecuteNonQuery()

	'			Catch ex As Exception
	'				m_Logger.LogError(String.Format("{0}.Daten in die Datenbanken schreiben: {1}", strMethodeName, ex.Message))
	'				Return False
	'			Finally
	'				Conn.Close()
	'			End Try

	'		End If

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Dim Time_2 As Double = System.Environment.TickCount
	'	Console.WriteLine("Zeit für SaveChangedFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Return True
	'End Function

	'#Region "Suche nach Rapporten zur Zuordnung der nicht erkannten Rapporte..."

	'	Function GetRPDb4SelectingRP() As DataTable Implements IClsDbRegister.GetRPDb4SelectingRP
	'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'		Dim ds As New DataSet
	'		Dim dt As New DataTable
	'		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
	'		Dim strQuery As String = "[List RPData For Search In RPScanning]"
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
	'		cmd.CommandType = CommandType.StoredProcedure

	'		Try
	'			Dim objAdapter As New SqlDataAdapter

	'			objAdapter.SelectCommand = cmd
	'			objAdapter.Fill(ds, "RPData")

	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'		End Try

	'		Return ds.Tables(0)
	'	End Function

	'	Function GetKWDataInRP(ByVal iRPNr As Integer) As DataTable Implements IClsDbRegister.GetKWDataInRP
	'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'		Dim ds As New DataSet
	'		Dim dt As New DataTable
	'		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
	'		Dim strQuery As String = "[List KWData For Search In RPScanning]"
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
	'		cmd.CommandType = CommandType.StoredProcedure

	'		Try
	'			Dim objAdapter As New SqlDataAdapter
	'			Dim param As System.Data.SqlClient.SqlParameter
	'			param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)

	'			objAdapter.SelectCommand = cmd
	'			objAdapter.Fill(ds, "RPLData")

	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'		End Try
	'		Return ds.Tables(0)
	'	End Function

	'#End Region


	'Function SaveCheckedContentIntoSPDb(ByVal rpInfo As DBInformation) As Boolean Implements IClsDbRegister.SaveCheckedContentIntoSPDb
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim Time_1 As Double = System.Environment.TickCount
	'	Dim strUSName As String = _ClsProgSetting.GetUserName()
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
	'	Dim sSql As String = String.Empty
	'	Dim strResult As String = String.Empty
	'	Dim strNewDocGuid As String = Guid.NewGuid.ToString

	'	sSql = "Insert Into [{0}].Dbo.[RP_ScanDoc] (RecNr, RPNr, RPLNr, MANr, KDNr, ESNr, "
	'	sSql &= "Bezeichnung, Beschreibung, DocScan, ScanExtension, RPDoc_Guid, CreatedOn, CreatedFrom, "
	'	sSql &= "ChangedOn, ChangedFrom) "
	'	sSql &= "Select 1, RPNr, @RPLNr, "
	'	sSql &= "@MANr, "
	'	sSql &= "@KDNr, "
	'	sSql &= "@ESNr, "
	'	sSql &= "(convert(nvarchar(10), Monday,104) + ' - ' + convert(nvarchar(10), sunday,104)) As Bez, "
	'	sSql &= "(convert(nvarchar(10), Monday,104) + ' - ' + convert(nvarchar(10), sunday,104)) As Beschreibung, "
	'	sSql &= "Scan_Komplett, @ScanExtension, @RPDoc_Guid, GetDate(), @CreatedFrom, GetDate(), @ChangedFrom "
	'	sSql &= "From [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Where ID = @ID "
	'	sSql &= "Update [Sputnik ScanJobs].Dbo.[RP.ScannedFileContent] Set CheckedOn = GetDate() Where ID = @ID "

	'	sSql = String.Format(sSql, Conn.Database)

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sSql, Conn)
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		Try

	'			param = cmd.Parameters.AddWithValue("@RPNr", rpInfo.SelectedRecordNumber)
	'			param = cmd.Parameters.AddWithValue("@RPLNr", rpInfo.SelectedRPLNr)
	'			param = cmd.Parameters.AddWithValue("@MANr", rpInfo.EmployeeNumber)
	'			param = cmd.Parameters.AddWithValue("@KDNr", rpInfo.CustomerNumber)
	'			param = cmd.Parameters.AddWithValue("@ESNr", rpInfo.ESNumber)
	'			param = cmd.Parameters.AddWithValue("@ScanExtension", "PDF")
	'			param = cmd.Parameters.AddWithValue("@RPDoc_Guid", strNewDocGuid)
	'			param = cmd.Parameters.AddWithValue("@CreatedFrom", strUSName)
	'			param = cmd.Parameters.AddWithValue("@ChangedFrom", strUSName)
	'			param = cmd.Parameters.AddWithValue("@ID", rpInfo.SelectedFileID)

	'			Conn.Open()
	'			cmd.Connection = Conn
	'			cmd.ExecuteNonQuery()

	'			If rpInfo.SendRPToKDWOS AndAlso _ClsProgSetting.GetKDWOSGuid.Length > 10 Then
	'				Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0, _
	'																										 .SelectedESNr = rpInfo.ESNumber, _
	'																										 .SelectedKDNr = rpInfo.CustomerNumber, _
	'																										 .SelectedMANr = rpInfo.EmployeeNumber, _
	'																										 .SelectedRPNr = rpInfo.SelectedRecordNumber, _
	'																										 .SelectedRPLNr = rpInfo.SelectedRPLNr, _
	'																										 .SelectedDocGuid = strNewDocGuid, _
	'																										 .SendRPToKDWOS = rpInfo.SendRPToKDWOS, _
	'																										 .SendRPToMAWOS = rpInfo.SendRPToMAWOS}
	'				Dim clsWOS As New ClsWOS(objWosInfo)
	'				clsWOS.SendKDDoc2WOS()
	'			End If


	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("{0}.Daten in die Sputnik Datenbanken schreiben: {1}", strMethodeName, ex.Message))
	'			Return False
	'		Finally
	'			Conn.Close()
	'		End Try


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Dim Time_2 As Double = System.Environment.TickCount
	'	Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Return True
	'End Function

	'Function DeleteScannedFileContent(ByVal rpInfo As DBInformation) As String Implements IClsDbRegister.DeleteScannedFileContent
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim Time_1 As Double = System.Environment.TickCount
	'	Dim strUSName As String = _ClsProgSetting.GetUserName()
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
	'	Dim sSql As String = String.Empty
	'	Dim strResult As String = String.Empty

	'	sSql = "[Delete Selected Scanned FileContent]"

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		Conn.Open()
	'		cmd.Connection = Conn

	'		Try
	'			cmd.CommandType = CommandType.StoredProcedure
	'			cmd.CommandText = sSql

	'			param = cmd.Parameters.AddWithValue("@ID", Me.m_reportDBInformation.SelectedFileID)

	'			cmd.Connection = Conn
	'			cmd.ExecuteNonQuery()

	'			cmd.Parameters.Clear()
	'			strResult = "Success..."


	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("{0}.Datensatz löschen: {1}", strMethodeName, ex.Message))
	'			strResult = String.Format("***Error (DeleteImageFromDB_1): {0}", ex.Message)

	'		End Try

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		strResult = String.Format("***Error (DeleteImageFromDB_2): {0}", ex.Message)

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Dim Time_2 As Double = System.Environment.TickCount
	'	Console.WriteLine("Zeit für DeleteImageFromDB: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Return strResult
	'End Function


	'Function GetFileToByte(ByVal filePath As String) As Byte() Implements IClsDbRegister.GetFileToByte
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
	'	Dim reader As BinaryReader = New BinaryReader(stream)

	'	Dim photo() As Byte = Nothing
	'	Try

	'		photo = reader.ReadBytes(CInt(stream.Length))
	'		reader.Close()
	'		stream.Close()

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'	Return photo
	'End Function

	'Function Image2ByteArray(ByVal Bild As Image, _
	'												 ByVal Bildformat As System.Drawing.Imaging.ImageFormat) As Byte()
	'	Dim MS As New IO.MemoryStream
	'	Bild.Save(MS, Bildformat)
	'	MS.Flush()

	'	Return MS.ToArray
	'End Function



#Region "Testfunktionen..."

	'Function _ListRPScanDb4Print() As Boolean Implements IClsDbRegister._ListRPScanDb4Print
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strCreatedFilename As New List(Of String)
	'	Dim bTest As Boolean = True

	'	Try
	'		Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
	'		Conn.Open()

	'		Try
	'			Dim sSql As String = "Select ID, DocScan From RP_ScanDoc "
	'			sSql &= "Where Month(Createdon) Between 1 And 7 And Year(CreatedOn) = 2012 "
	'			sSql &= "Order By RPNr"

	'			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'			cmd.CommandType = Data.CommandType.Text
	'			'Dim param As System.Data.SqlClient.SqlParameter
	'			'param = cmd.Parameters.AddWithValue("@MDGuid", m_InitialData.MDData.MDGuid)
	'			'param = cmd.Parameters.AddWithValue("@ImportedFileGuid", strSelectedFileGuid)
	'			Dim rScannrec As SqlDataReader = cmd.ExecuteReader
	'			While rScannrec.Read
	'				m_reportDBInformation.SelectedFileID = rScannrec("ID").ToString
	'				Dim strFileName As String = Me._CreatePDFFileFromSP2FS()
	'				If File.Exists(strFileName) Then
	'					strCreatedFilename.Add(strFileName)
	'				End If
	'			End While
	'			If strCreatedFilename.Count > 0 Then
	'				Dim strResult As String = Me.Merg2PDFFiles(String.Format("{0}{1}.pdf", _ClsProgSetting.GetSpSRPTempPath, "_AllRP"), strCreatedFilename.ToArray)
	'			End If

	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("1. {0}.{1}", strMethodeName, ex.Message))

	'		End Try

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'	Return bTest
	'End Function

	'Function _CreatePDFFileFromSP2FS() As String Implements IClsDbRegister._CreatePDFFileFromSP2FS
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim dResult As Boolean = False
	'	Dim strFullFilename As String = String.Empty
	'	Dim strFiles As String = String.Empty
	'	Dim BA As Byte() = Nothing
	'	Dim sSql As String = "Select DocScan, ID, RPNr From RP_ScanDoc "
	'	sSql &= "Where ID = {0}"
	'	sSql = String.Format(sSql, m_reportDBInformation.SelectedFileID)
	'	Dim i As Integer = 0

	'	Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
	'	Conn.Open()
	'	Dim SQLCmd As SqlCommand = New SqlCommand(sSql, Conn)
	'	Dim SQLCmd_1 As SqlCommand = New SqlCommand(sSql, Conn)

	'	Try


	'		Try
	'			strFullFilename = String.Format("{0}ScannedReport_{1}.pdf", _ClsProgSetting.GetSpSRPTempPath, _
	'																			 m_reportDBInformation.SelectedFileID)
	'			Try
	'				Try
	'					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
	'				Catch ex As Exception

	'				End Try
	'				If BA Is Nothing Then Return dResult

	'				Dim ArraySize As New Integer
	'				ArraySize = BA.GetUpperBound(0)

	'				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
	'				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
	'				fs.Write(BA, 0, ArraySize + 1)
	'				fs.Close()
	'				fs.Dispose()


	'			Catch ex As Exception
	'				m_Logger.LogError(String.Format("1. {0}.{1}", strMethodeName, ex.Message))
	'				MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "StoreSelectedScannedFile2FS")
	'				strFullFilename = String.Empty

	'			End Try

	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("2. {0}.{1}", strMethodeName, ex.Message))

	'		End Try
	'		If File.Exists(strFullFilename) Then Return strFullFilename

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		strFullFilename = String.Empty

	'	End Try

	'	Return strFullFilename
	'End Function


	'Function StoreExistsScannedFileFromLocalDb2FS(ByVal rpInfo As DBInformation) As Dictionary(Of String, String) Implements IClsDbRegister.StoreExistsScannedFileFromLocalDb2FS
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim dResult As New Dictionary(Of String, String)
	'	'If reportDBInformation.SelectedFileID = 0 Then Return dResult
	'	Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)

	'	Dim strFullFilename As String = String.Empty
	'	Dim strFiles As String = String.Empty
	'	Dim BA As Byte() = Nothing
	'	Dim sScanSql As String = "Select Top 1 DocScan, IsNull(RPNr, 0) As RPNr, IsNull(RPDoc_Guid, '') As RPDoc_Guid, "
	'	sScanSql &= "IsNull(ESNr,0) As ESNr, IsNull(MANr, 0) As MANr, IsNull(KDNr, 0) As KDNr, IsNull(ScanExtension, 'PDF') As ScanExtension "
	'	sScanSql &= "From [RP_ScanDoc] Where "
	'	sScanSql &= "RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr"

	'	Dim i As Integer = 0
	'	Dim strNewGuid As String = Guid.NewGuid.ToString

	'	Conn.Open()
	'	'Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, ScanConn)
	'	Dim SQLCmd_1 As SqlCommand = New SqlCommand(sScanSql, Conn)
	'	Dim param As System.Data.SqlClient.SqlParameter
	'	param = SQLCmd_1.Parameters.AddWithValue("@RPNr", rpInfo.SelectedRecordNumber)
	'	param = SQLCmd_1.Parameters.AddWithValue("@RPLNr", rpInfo.SelectedRPLNr)
	'	param = SQLCmd_1.Parameters.AddWithValue("@MANr", rpInfo.EmployeeNumber)
	'	param = SQLCmd_1.Parameters.AddWithValue("@KDNr", rpInfo.CustomerNumber)
	'	Dim strExtension As String = "PDF"
	'	Try
	'		Dim rScannrec As SqlDataReader = SQLCmd_1.ExecuteReader
	'		While rScannrec.Read
	'			dResult.Add("RPDoc_Guid".ToUpper, rScannrec("RPDoc_Guid").ToString)
	'			strExtension = rScannrec("ScanExtension").ToString

	'			Try
	'				BA = CType(rScannrec("DocScan"), Byte()) 'SQLCmd_1.ExecuteScalar, Byte())
	'			Catch ex As Exception
	'				m_Logger.LogError(String.Format("1. {0}.{1}", strMethodeName, ex.Message))

	'			End Try

	'		End While

	'		If BA Is Nothing Then dResult.Clear() : Return dResult
	'		strFullFilename = String.Format("{0}ScannedReport_{1}.{2}", _
	'																		_ClsProgSetting.GetSpSRPTempPath, strNewGuid, _
	'																		strExtension)

	'		Dim ArraySize As New Integer
	'		ArraySize = BA.GetUpperBound(0)

	'		If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
	'		Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
	'		fs.Write(BA, 0, ArraySize + 1)
	'		fs.Close()
	'		fs.Dispose()


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "StoreExistsScannedFileFromLocalDb2FS")
	'		strFullFilename = String.Empty

	'	End Try
	'	dResult.Add("FileName".ToUpper, strFullFilename)

	'	Return dResult
	'End Function

	'Function SaveSelectedFileIntoLocalDb(ByVal rpInfo As DBInformation) As Boolean Implements IClsDbRegister.SaveSelectedFileIntoLocalDb
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	If (_ClsProgSetting.GetKDWOSGuid.Length > 10 Or _ClsProgSetting.GetMAWOSGuid.Length > 10) AndAlso Not String.IsNullOrEmpty(rpInfo.OriginFileGuid) Then
	'		Dim objWosInfo As New ClsWOSInfomation With {.SelectedDocGuid = rpInfo.OriginFileGuid}
	'		Dim clsWOS As New ClsWOS(objWosInfo)

	'		clsWOS.DeleteDocFromWOS()
	'	End If

	'	Dim Time_1 As Double = System.Environment.TickCount
	'	Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim sSql As String = String.Empty
	'	Dim strResult As String = String.Empty
	'	Dim strNewDocGuid As String = Guid.NewGuid.ToString
	'	rpInfo.OriginFileGuid = strNewDocGuid

	'	sSql = "Delete [RP_ScanDoc] Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr; "
	'	sSql &= "Insert Into [RP_ScanDoc] (RPNr, MANr, RecNr, Bezeichnung, Beschreibung, CreatedOn, CreatedFrom, DocScan, "
	'	sSql &= "ScanExtension, KDNr, ESNr, RPLNr, RPDoc_Guid) Values ("
	'	sSql &= "@RPNr, @MANr, 1, "
	'	sSql &= "@Bezeichnung, @Bezeichnung, GetDate(), @CreatedFrom, @DocScan, 'PDF', "
	'	sSql &= "@KDNr, "
	'	sSql &= "@ESNr, @RPLNr, @RPDoc_Guid)"

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sSql, Conn)
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		If File.Exists(rpInfo.SelectedFileToSave) Then
	'			Try
	'				Dim fileInfo = New FileInfo(rpInfo.SelectedFileToSave)
	'				Dim pdfBytes = fileInfo.ToByteArray()

	'				If pdfBytes Is Nothing Then
	'					Return False
	'				End If

	'				param = cmd.Parameters.AddWithValue("@RPNr", rpInfo.SelectedRecordNumber)
	'				param = cmd.Parameters.AddWithValue("@MANr", rpInfo.SelectedMANr)
	'				param = cmd.Parameters.AddWithValue("@Bezeichnung", String.Format("{0} - {1}", CalendarWeek(rpInfo.CalendarWeek, Now.Year), _
	'																																					CalendarWeek(rpInfo.CalendarWeek, Now.Year).AddDays(6)))
	'				param = cmd.Parameters.AddWithValue("@CreatedFrom", _ClsProgSetting.GetUserLName)
	'				param = cmd.Parameters.AddWithValue("@DocScan", pdfBytes)
	'				param = cmd.Parameters.AddWithValue("@KDNr", rpInfo.SelectedKDNr)
	'				param = cmd.Parameters.AddWithValue("@ESNr", rpInfo.SelectedESNr)
	'				param = cmd.Parameters.AddWithValue("@RPLNr", rpInfo.SelectedRPLNr)
	'				param = cmd.Parameters.AddWithValue("@RPDoc_Guid", strNewDocGuid)

	'				Conn.Open()
	'				cmd.Connection = Conn
	'				cmd.ExecuteNonQuery()

	'				If (_ClsProgSetting.bAllowedKDDocTransferTo_WS Or _ClsProgSetting.bAllowedMADocTransferTo_WS) AndAlso (rpInfo.SendRPToKDWOS Or rpInfo.SendRPToMAWOS) Then
	'					Dim objWosInfo As New ClsWOSInfomation With {.SelectedESLohnNr = 0, _
	'																											 .SelectedESNr = rpInfo.SelectedESNr, _
	'																											 .SelectedKDNr = rpInfo.SelectedKDNr, _
	'																											 .SelectedMANr = rpInfo.SelectedMANr, _
	'																											 .SelectedRPNr = rpInfo.SelectedRecordNumber, _
	'																											 .SelectedRPLNr = rpInfo.SelectedRPLNr, _
	'																											 .SelectedDocGuid = strNewDocGuid, _
	'																											 .SendRPToKDWOS = rpInfo.SendRPToKDWOS, _
	'																											 .SendRPToMAWOS = rpInfo.SendRPToMAWOS}
	'					Dim clsWOS As New ClsWOS(objWosInfo)
	'					clsWOS.SendKDDoc2WOS()
	'				End If

	'			Catch ex As Exception
	'				m_Logger.LogError(String.Format("1. {0}.{1}", strMethodeName, ex.Message))
	'				Return False
	'			Finally
	'				Conn.Close()
	'			End Try

	'		End If

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Dim Time_2 As Double = System.Environment.TickCount
	'	Console.WriteLine("Zeit für SaveChangedFileIntoLocalDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Return True
	'End Function

	'Function DeleteExistsFileContentFromLocalDb(ByVal rpInfo As DBInformation) As Boolean Implements IClsDbRegister.DeleteExistsFileContentFromLocalDb
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim Time_1 As Double = System.Environment.TickCount
	'	Dim strUSName As String = _ClsProgSetting.GetUserName()
	'	Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
	'	Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
	'	Dim sSql As String = String.Empty
	'	Dim strResult As String = String.Empty

	'	sSql = "Delete [RP_ScanDoc] "
	'	sSql &= "Where RPNr = @RPNr And RPLNr = @RPLNr And MANr = @MANr And KDNr = @KDNr And ESNr = @ESNr"

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	Try
	'		Conn.Open()
	'		cmd.Connection = Conn

	'		Try
	'			cmd.CommandType = CommandType.Text
	'			cmd.CommandText = sSql

	'			param = cmd.Parameters.AddWithValue("@RPNr", rpInfo.SelectedRecordNumber)
	'			param = cmd.Parameters.AddWithValue("@RPLNr", rpInfo.SelectedRPLNr)
	'			param = cmd.Parameters.AddWithValue("@MANr", rpInfo.SelectedMANr)
	'			param = cmd.Parameters.AddWithValue("@KDNr", rpInfo.SelectedKDNr)
	'			param = cmd.Parameters.AddWithValue("@ESNr", rpInfo.SelectedESNr)

	'			cmd.Connection = Conn
	'			cmd.ExecuteNonQuery()

	'			cmd.Parameters.Clear()
	'			strResult = "Erfolgreich..."

	'			If _ClsProgSetting.GetKDWOSGuid.Length > 10 Or _ClsProgSetting.GetMAWOSGuid.Length > 10 Then
	'				Dim objWosInfo As New ClsWOSInfomation With {.SelectedDocGuid = rpInfo.OriginFileGuid}
	'				Dim clsWOS As New ClsWOS(objWosInfo)

	'				strResult = clsWOS.DeleteDocFromWOS()
	'			End If


	'		Catch ex As Exception
	'			m_Logger.LogError(String.Format("1. {0}.{1}", strMethodeName, ex.Message))

	'		End Try

	'	Catch ex As Exception
	'		strResult = String.Format("***Fehler (DeleteExistsFileContentFromLocalDb_2): {0}", ex.Message)
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'	Dim Time_2 As Double = System.Environment.TickCount
	'	Console.WriteLine("Zeit für DeleteExistsFileContentFromLocalDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'	Return strResult
	'End Function


	Public ReadOnly Property GetPDF4Net_O2SSerial() As String
		Get
			Return _ClsProgSetting.GetPDF_O2SSerial	' "PDF4NET-MT735-CUBQB-6D8HV-I82RS-BO1VB"
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



