'------------------------------------
' File: PDFConverter.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports DevExpress.Pdf
'Imports O2S.Components.PDF4NET.PDFFile
Imports ReportImporterCommon.Logging
Imports ReportImporterDBPersistence
Imports ReportImporterCommon.FileHandling
Imports ReportImporterDataMatrixAnalyzer
Imports ReportImporterCommon

''' <summary>
''' Converts pdf reports.
''' </summary>
Public Class PDFConverter
	Implements IPDFConverter

#Region "Private constants"

	Private Const TEMPORARY_FOLDER_ORIGINAL_PDF As String = "ORIGINAL_PDF"
	Private Const TEMPORARY_FOLDER_SPLITTED_PDFS As String = "SPLITTED_PDFS"
	Private Const PDF4NET_SERIALNUMBER As String = "yourserialnumber"
	Private Const PDFVIEW4NET_SERIALNUMBER As String = "yourserialnumber"

#End Region

#Region "Private Fields"

	''' <summary>
	''' The pdf converter settings. 
	''' </summary>
	Private pdfConverterSettings As PDFConverterSettings

	''' <summary>
	''' The database persister.
	''' </summary>
	Private m_ScanDataAccess As IReportDBPersister
	Private m_WorkingReportpath As String

	''' <summary>
	''' The logger object.
	''' </summary>
	Private Shared m_logger As ILogger = New Logger()

	Private m_Pagecount As Integer
	Private m_MandantData As MandantData
	Private dataMatrixInfoIndex As Integer

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	''' <param name="settings">The pdf converter settings.</param>
	Public Sub New(ByVal settings As PDFConverterSettings)
		pdfConverterSettings = settings

		' Create the db persister.
		m_ScanDataAccess = New ReportDBPersister(settings.ScanJobsConnectionString, "DE")
		m_ScanDataAccess.LoadSettingData(pdfConverterSettings.SettingFileValue)
		m_MandantData = New MandantData

	End Sub

#End Region

#Region "Public methods"

	''' <summary>
	''' Converts a (multipage) pdf report.
	''' </summary>
	''' <param name="workingReportPath">The working report path.</param>
	''' <param name="mandantGuid">The mandant guid.</param>
	Public Sub ConvertPDFReport(ByVal workingReportPath As String, ByVal mandantGuid As String) Implements IPDFConverter.ConvertPDFReport

		Dim success As Boolean = True

		pdfConverterSettings.CustomerID = mandantGuid
		m_MandantData = m_ScanDataAccess.LoadMandantData(pdfConverterSettings.CustomerID)

		If m_MandantData Is Nothing OrElse m_MandantData.ID.GetValueOrDefault(0) = 0 Then Return

		m_WorkingReportpath = Directory.GetParent(workingReportPath).ToString

		' Checks if working report does exists.
		If Not (File.Exists(workingReportPath)) Then
			m_logger.LogWarning(String.Format("The file {0} does not exist.", workingReportPath))
			Return
		End If

		' Checks if temporary folder exists.
		If Not Directory.Exists(pdfConverterSettings.TemporaryFolder) Then
			m_logger.LogWarning(String.Format("The configurated temp folder {0} does not exist.", pdfConverterSettings.TemporaryFolder))

			Return
		End If

		' Guid for task.
		Dim pdfConvertGuid As String = Guid.NewGuid.ToString

		' Temporary folders
		Dim workingFolder = Path.Combine(pdfConverterSettings.TemporaryFolder, pdfConvertGuid)

		Dim originalPDFFolder = Path.Combine(workingFolder, TEMPORARY_FOLDER_ORIGINAL_PDF)
		Dim splittedPDFsFolderFullPath = Path.Combine(workingFolder, TEMPORARY_FOLDER_SPLITTED_PDFS)

		m_logger.LogInfo(String.Format("workingFolder: {0} | originalPDFFolder: {1} | splittedPDFsFolderFullPath: {2} | workingReportPath: {3}",
																	 workingFolder, originalPDFFolder, splittedPDFsFolderFullPath, workingReportPath))

		' 1. Create temporary working folders.
		success = CreateTemporaryWorkingFolders(workingFolder, originalPDFFolder, splittedPDFsFolderFullPath)
		If (Not success) Then Return

		' 2. Move working pdf from source to original pdf folder.
		Dim copyToProcessedFolder = MoveOrCopyFileToFolder(workingReportPath, Path.Combine(pdfConverterSettings.ProcessedScannedDocumentsFolder, mandantGuid), True)
		Dim originalPDFFilePath = MoveOrCopyFileToFolder(workingReportPath, originalPDFFolder, False)
		If originalPDFFilePath Is Nothing OrElse String.IsNullOrEmpty(originalPDFFilePath) Then
			m_logger.LogError(String.Format("workingReportPath {0} could not be moved into originalPDFFolder {1} >>> process is terminated!", workingReportPath, originalPDFFolder))

			Return
		End If


		Try

			Dim dataMatrixResults_ = SplitPDFWithDataMatrixCode(originalPDFFilePath, splittedPDFsFolderFullPath, mandantGuid)
			If dataMatrixResults_ Is Nothing OrElse dataMatrixResults_.Count = 0 Then
				m_logger.LogInfo(String.Format("datamatrixcode could not be processed. processing will be aborted. {0}", originalPDFFilePath))

				Return
			End If

			Dim pdfObjectList = dataMatrixResults_


			' 3. Save complete report to database.
			m_Pagecount = dataMatrixResults_.Count
			If Not NotifySystemScanImport(originalPDFFilePath, mandantGuid, pdfConvertGuid) Then
				Return
			End If



			' 5. Analyze pdf files with DataMatrix analyzer.
			Dim dataMatrixResults = AnalyzePDFPagesWithDataMatrixAnalyzer(pdfObjectList, mandantGuid, False, True)
			dataMatrixInfoIndex = 0
			If (dataMatrixResults Is Nothing OrElse dataMatrixResults.Count = 0) Then
				' insert into tbl_attachment
				If Not SaveScanfileAsAttachment(originalPDFFilePath, mandantGuid, pdfConvertGuid) Then Return
				success = success AndAlso AlertForScanImport(originalPDFFilePath, mandantGuid, pdfConvertGuid)

				Return
			End If

			' 6. Store values in database.
			For Each dataMatrixResult In dataMatrixResults

				If dataMatrixResult.DataMatrixInfos.Count <> 0 Then
					If pdfObjectList.Count > 1 Then pdfConvertGuid = Guid.NewGuid.ToString
					Dim pdfFileInfo As FileInfo = New FileInfo(dataMatrixResult.PDFFullPath)
					Dim pdfFilename As String = pdfFileInfo.Name
					Dim code As String = dataMatrixResult.DataMatrixInfos(0).DataMatrixValue

					If (pdfFilename.StartsWith("employee_".ToLower) OrElse pdfFilename.StartsWith("customer_".ToLower)) AndAlso Not code.EndsWith("_999") Then

						' delete first page (stammblatt)!
						Try
							Using pdfDocumentProcessor As New PdfDocumentProcessor()
								pdfDocumentProcessor.LoadDocument(dataMatrixResult.PDFFullPath)
								If pdfDocumentProcessor.Document.Pages.Count > 1 Then
									pdfDocumentProcessor.DeletePage(1)
									pdfDocumentProcessor.SaveDocument(dataMatrixResult.PDFFullPath)
								End If
							End Using
						Catch ex As Exception
							m_logger.LogError(ex.ToString)
						End Try
					End If

					' 3. Save splited file to database.
					success = success AndAlso SaveScanfileAsAttachment(dataMatrixResult.PDFFullPath, mandantGuid, pdfConvertGuid)
					success = success AndAlso SaveReportToDatabase(dataMatrixResult, mandantGuid, pdfConvertGuid, dataMatrixInfoIndex)

					m_logger.LogInfo(String.Format("{0} data is saved >>> filename: {1} >>> value: {2}", mandantGuid, dataMatrixResult.PDFFullPath, dataMatrixResult.DataMatrixInfos(0).DataMatrixValue))

					Trace.WriteLine(String.Format("{0} data is saved >>> filename: {1} >>> value: {2}", mandantGuid, dataMatrixResult.PDFFullPath, dataMatrixResult.DataMatrixInfos(0).DataMatrixValue))
				Else
					m_logger.LogInfo(String.Format("no datamatrixcode was founded in document! {0}", dataMatrixResult.PDFFullPath))
				End If

				If dataMatrixInfoIndex = dataMatrixResult.DataMatrixInfos.Count - 1 Then
					dataMatrixInfoIndex = 0
				Else
					dataMatrixInfoIndex += If(dataMatrixResult.DataMatrixInfos.Count > 1, 1, 0)
				End If


				If Not success Then
					m_logger.LogInfo(String.Format("data could not be saved in database! customer_id: {0} >>> filename: {1} >>> pdfGuid:{2}", mandantGuid, dataMatrixResult.PDFFullPath, pdfConvertGuid))

					success = True
				End If

			Next

			If (Not success) Then
				m_logger.LogError("process was not successfull!")

				Return
			End If


			If Not NotifySystemFinishingScanProcess(originalPDFFilePath, mandantGuid, pdfConvertGuid) Then
				m_logger.LogError("process (NotifySystemFinishingScanProcess) was not successfull!")

				Return
			End If


			' 7. Copy the file to the processed documents folder.
			Try
				success = success AndAlso DeleteSplittedFolder(workingFolder)
				If Not success Then m_logger.LogError(String.Format("customer_id {0} process (DeleteSplittedFolder) was not successfull: {1}", mandantGuid, workingFolder))

			Catch ex As Exception
				m_logger.LogError(ex.ToString)

			End Try

		Catch ex As Exception
			m_logger.LogError(ex.ToString)

		Finally
			Try
				DeleteSplittedFolder(workingFolder)
			Catch ex As Exception
				m_logger.LogError(ex.ToString)

			End Try

		End Try

	End Sub

#End Region

#Region "Private methdos"

	''' <summary>
	''' Creates the temporary working folders.
	''' </summary>
	''' <param name="workingFolder">The working folder root.</param>
	''' <param name="originalPDFFolderFullPath">The original pdf folder.</param>
	''' <param name="splittedPDFsFolderFullPath">The splitted pdfs folder.</param>
	''' <returns>Boolean truth indicating if operation was succesfull.</returns>
	Private Function CreateTemporaryWorkingFolders(ByVal workingFolder As String, ByVal originalPDFFolderFullPath As String, ByVal splittedPDFsFolderFullPath As String) As Boolean
		Try
			Directory.CreateDirectory(workingFolder)
			Directory.CreateDirectory(originalPDFFolderFullPath)
			Directory.CreateDirectory(splittedPDFsFolderFullPath)

		Catch ex As Exception
			m_logger.LogError(ex.ToString)
			Return False
		End Try

		Return True
	End Function

	''' <summary>
	''' Moves the source pdf to the original pdf folder.
	''' </summary>
	''' <param name="filePath">The file full path.</param>
	''' <param name="destinationFolderPath">The destination folder full path.</param>
	''' <param name="doCopy">True for copying, false for moving.</param>
	''' <returns>Path to the moved or copied destination file or Nothing if this did not work.</returns>
	Private Function MoveOrCopyFileToFolder(ByVal filePath As String, ByVal destinationFolderPath As String, ByVal doCopy As Boolean) As String

		Dim newFilePath As String

		Try
			Dim fileInfo As FileInfo = New FileInfo(filePath)
			If Not Directory.Exists(destinationFolderPath) Then
				Try
					Directory.CreateDirectory(destinationFolderPath)
				Catch ex As Exception

				End Try
			End If
			newFilePath = Path.Combine(destinationFolderPath, fileInfo.Name)

			Dim newFileInfo As New FileInfo(newFilePath)

			' Delete an existing file with the same name before move or copy operation.
			If (newFileInfo.Exists) Then
				newFileInfo.Delete()
			End If

			If (doCopy) Then
				' Copy the file.
				File.Copy(filePath, newFilePath, True)
			Else
				' Move the file.
				File.Move(filePath, newFilePath)
			End If

		Catch ex As Exception
			m_logger.LogError(ex.ToString)

			Return Nothing
		End Try

		Return newFilePath

	End Function

	Private Function SplitPDFWithDataMatrixCode(ByVal pdfFileFullPath As String, ByVal splittedPDFsFolderFullPath As String, ByVal mandantGuid As String) As List(Of PDFObject)
		Dim result As List(Of PDFObject) = New List(Of PDFObject)

		Try
			Dim pdfObjectList = New List(Of PDFObject)
			pdfObjectList.Add(New PDFObject With {.PDFFilePath = pdfFileFullPath})
			Dim dataMatrixResult = AnalyzePDFPagesWithDataMatrixAnalyzer(pdfObjectList, mandantGuid, True, True)
			If dataMatrixResult Is Nothing OrElse dataMatrixResult.Count = 0 Then Return Nothing

			Dim directoryInfo As DirectoryInfo = New DirectoryInfo(splittedPDFsFolderFullPath)

			If directoryInfo.GetFiles().Count <> dataMatrixResult(0).DataMatrixInfos.Count AndAlso dataMatrixResult(0).DataMatrixInfos.Count > 0 Then
				Dim tmpCodeValue = New List(Of String)
				For Each code In dataMatrixResult(0).DataMatrixInfos
					tmpCodeValue.Add(code.DataMatrixValue)
				Next

				For Each fileInfo In directoryInfo.GetFiles()
					Dim tmpPDFList = New List(Of PDFObject)
					tmpPDFList.Add(New PDFObject With {.PDFFilePath = fileInfo.FullName})
					Dim tmpdataMatrixResult = AnalyzePDFPagesWithDataMatrixAnalyzer(tmpPDFList, mandantGuid, False, True)

					For Each code In tmpdataMatrixResult(0).DataMatrixInfos
						If code.DataMatrixValue.ToString.ToLower.Split(CChar("_")).Count < 3 Then Continue For

						Dim newPDFFile = Path.Combine(fileInfo.DirectoryName, String.Format("{0}_{1}.PDF", Path.GetFileNameWithoutExtension(fileInfo.FullName), Path.GetRandomFileName))
						File.Copy(fileInfo.FullName, newPDFFile, True)
						m_logger.LogWarning(String.Format("newfile: {0} >>> {1}", newPDFFile, code.DataMatrixValue.ToString))

						result.Add(New PDFObject With {.PDFFilePath = newPDFFile})
					Next

				Next
				If result.Count > 0 Then
					m_logger.LogInfo(String.Format("{0} files are splited and created.", result.Count))

					Return result
				End If


			ElseIf dataMatrixResult(0).DataMatrixInfos.Count = 0 Then
				Return result
			End If

			directoryInfo = New DirectoryInfo(splittedPDFsFolderFullPath)
			For Each fileInfo In directoryInfo.GetFiles()
				result.Add(New PDFObject With {.PDFFilePath = fileInfo.FullName})
			Next
			m_logger.LogInfo(String.Format("{0} files are splited and created.", result.Count))


		Catch ex As Exception
			m_logger.LogError(ex.ToString)

			Return Nothing
		End Try


		Return result

	End Function


	''' <summary>
	''' Analyze pdf pages with a DataMatrix analyzer.
	''' </summary>
	''' <param name="pdfObjects">The list of pdf objects.</param>
	''' <param name="mandantGuid">The mandant guid.</param>
	''' <returns>List of DataMatrixResult objects or Nothing if something fails.</returns>
	Private Function AnalyzePDFPagesWithDataMatrixAnalyzer(ByVal pdfObjects As List(Of PDFObject), ByVal mandantGuid As String, ByVal doSplit As Boolean, ByVal notifyAdmin As Boolean) As List(Of DataMatrixResult)
		Dim dataMatrixAnalayzer As New DataMatrixAnalyzer(m_WorkingReportpath, mandantGuid)
		dataMatrixAnalayzer.SettingFileValue = pdfConverterSettings.SettingFileValue
		Dim dataMatrixResults As List(Of DataMatrixResult) = New List(Of DataMatrixResult)
		dataMatrixAnalayzer.SplitPDFFileWithBarCode = doSplit

		Try

			For Each pdfObject In pdfObjects

				' Analyze the pdf file with automatic threshold detection.
				Dim dataMatrixResult = dataMatrixAnalayzer.ReadDataMatrixCode(pdfObject.PDFFilePath)

				' Check if PDF file could be analyzed.
				If (Not dataMatrixResult.CouldPDFFileBeAnalyzed) Then

					m_logger.LogWarning(String.Format("PDF file {0} could not be analyzed.", pdfObject.PDFFilePath))

					If notifyAdmin Then
						Try
							Dim mailFrom = "info@sputnik-it.com"
							Dim mailTo = m_MandantData.Report_Recipients
							Dim bCC = m_MandantData.bccAddresses
							Dim strSubject As String = "Automatische Nachricht über Sputnik DOC-Scan"
							Dim strBody As String = "<b>Mandant:</b> {0}<br><b>Name:</b> {1}<br>Die im Anhang befindliche PDF-Datei konnte nicht gelesen werden."
							strBody = String.Format(strBody, mandantGuid, m_MandantData.Customer_Name)

							Dim m_NotifyUtility As New Notification.Notifying
							m_NotifyUtility.SettingFileData = pdfConverterSettings.SettingFileValue
							If String.IsNullOrWhiteSpace(mailTo) Then mailTo = pdfConverterSettings.SettingFileValue.NotifyEMailToScanJob

							Dim result = m_NotifyUtility.SendMailToWithExchange(mailFrom, mailTo, bCC, strSubject, strBody, New List(Of String) From {pdfObject.PDFFilePath})
							m_logger.LogDebug(String.Format("notification was sent. {0} >>> {1}", result, pdfObject.PDFFilePath))

						Catch ex As Exception
							m_logger.LogError(String.Format("SendMailToWithExchange: {0}", ex.ToString))

						End Try
					End If


					dataMatrixResults.Add(dataMatrixResult)

					Continue For
				End If

				' Log if not exactly one DataMatrix code has been found.
				If (dataMatrixResult.DataMatrixInfos.Count = 0 OrElse dataMatrixResult.DataMatrixInfos.Count = 1) Then
					m_logger.LogDebug(String.Format("{0} Daten in DataMatrix codes wurden gefunden. {1}", dataMatrixResult.DataMatrixInfos.Count, pdfObject.PDFFilePath))

				ElseIf (dataMatrixResult.DataMatrixInfos.Count > 1) Then
					m_logger.LogDebug(String.Format("Mehrere DataMatrix codes wurden gefunden. Möglicherweise sind mehrere Dokumente in einem eingescannt worden. Anzahl DataMatrix codes: {0} -> {1}",
																								dataMatrixResult.DataMatrixInfos.Count, pdfObject.PDFFilePath))
				End If

				dataMatrixResults.Add(dataMatrixResult)

			Next


		Catch ex As Exception
			m_logger.LogError(ex.ToString)

			Return Nothing

		End Try

		Return dataMatrixResults

	End Function

	''' <summary>
	''' Saves the complete pdf report to the databse.
	''' </summary>
	''' <param name="strPDFFullPath">The full pdf path.</param>
	''' <param name="mdGuid">The mandant guid.</param>
	''' <param name="strFileGuid">The file guid.</param>
	''' <returns>Boolean truth value indicating success</returns>
	Private Function SaveScanfileAsAttachment(ByVal strPDFFullPath As String, ByVal mdGuid As String, ByVal strFileGuid As String) As Boolean

		' Create a new report db information object
		Dim result As Boolean = True
		Dim reportDBInformation As New ReportImporterDBPersistence.ReportDBInformation()
		reportDBInformation.MDGuid = mdGuid
		reportDBInformation.PDFFullPath = strPDFFullPath
		reportDBInformation.OriginalFileGuid = strFileGuid
		reportDBInformation.WorkingForWebService = True ' pdfConverterSettings.WorkingForWebService

#If Not DEBUG Then
		' insert into tbl_attachment
		result = result AndAlso m_ScanDataAccess.SaveCompleteFile(reportDBInformation)
#End If

		Return result

	End Function

	''' <summary>
	''' Saves a report to the database.
	''' </summary>
	''' <param name="dataMatrixResult">The dataMatrix result.</param>
	''' <param name="mdGuid">The mandant guid.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Private Function SaveReportToDatabase(ByVal dataMatrixResult As DataMatrixResult, ByVal mdGuid As String, ByVal pdfConvertGuid As String, ByVal dataMatrixInfoIndex As Integer) As Boolean

		'1. Create a new report db information object.
		Dim result As Boolean = True
		Dim reportDBInformation As ReportDBInformation = New ReportDBInformation
		reportDBInformation.MDGuid = mdGuid
		reportDBInformation.PDFFullPath = dataMatrixResult.PDFFullPath
		reportDBInformation.OriginalFileGuid = pdfConvertGuid
		reportDBInformation.WorkingForWebService = pdfConverterSettings.WorkingForWebService


		If dataMatrixResult.DataMatrixInfos.Count = 1 Then
			reportDBInformation.DataMatrixCodeValue = dataMatrixResult.DataMatrixInfos(0).DataMatrixValue

		Else
			m_logger.LogWarning(String.Format("dataMatrixInfoIndex is given {0}, count of dataMatrixResult: {1}", dataMatrixInfoIndex, dataMatrixResult.DataMatrixInfos.Count))

			Try
				If dataMatrixInfoIndex > 0 Then File.Copy(reportDBInformation.PDFFullPath, Path.Combine(pdfConverterSettings.ProcessedScannedDocumentsFolder, "SuspectedFiles", Path.GetRandomFileName & ".PDF"), True)
			Catch ex As Exception
				m_logger.LogWarning(String.Format("more datamatrix code was founded {0}, suspectedfile: {1}", dataMatrixInfoIndex, Path.Combine(pdfConverterSettings.ProcessedScannedDocumentsFolder, "SuspectedFiles", Path.GetRandomFileName & ".PDF")))
			End Try

			Try
				reportDBInformation.DataMatrixCodeValue = dataMatrixResult.DataMatrixInfos(dataMatrixInfoIndex).DataMatrixValue
			Catch ex As Exception
				m_logger.LogWarning(String.Format("dataMatrixInfoIndex is given {0}, value: {1}", dataMatrixInfoIndex, dataMatrixResult.DataMatrixInfos(0).DataMatrixValue))
				reportDBInformation.DataMatrixCodeValue = dataMatrixResult.DataMatrixInfos(0).DataMatrixValue
			End Try

		End If

		'2. Save the report to the database.
#If Not DEBUG Then
		' insert into tbl_scannedFile and/or tbl_ScannedReport
	result = result AndAlso m_ScanDataAccess.SaveData(reportDBInformation)
#End If


		Return result

	End Function

	Private Function NotifySystemScanImport(ByVal strPDFFullPath As String, ByVal mdGuid As String, ByVal pdfConvertGuid As String) As Boolean
		Dim result As Boolean = True

		Dim dbInfoData As ReportDBInformation = New ReportDBInformation
		Dim notifyData = New NotificationData
		dbInfoData.MDGuid = mdGuid
		dbInfoData.PDFFullPath = strPDFFullPath
		dbInfoData.OriginalFileGuid = pdfConvertGuid
		dbInfoData.WorkingForWebService = pdfConverterSettings.WorkingForWebService
		dbInfoData.Pages = m_Pagecount


		Dim header As String = "Scan-Job: Scan-Import"
		Dim comments As String = "Ein gesscantes Dokument wurde importiert.{0}Anzahlseiten: {1}{0}"
		comments &= "Dateiname: {2}{0}"
		comments &= "Scan-Kennung: {3}{0}"
		comments &= "Scan-Type: {4}{0}"
		comments = String.Format(comments, vbNewLine, m_Pagecount, Path.GetFileName(dbInfoData.PDFFullPath), dbInfoData.OriginalFileGuid, dbInfoData.ScanModulType)

		notifyData.Customer_ID = mdGuid
		notifyData.NotifyArt = 5
		notifyData.NotifyComments = comments
		notifyData.NotifyHeader = header
		notifyData.CreatedFrom = "NotifySystemScanImport"

		result = result AndAlso m_ScanDataAccess.SendNotificationForImportedFiles(dbInfoData, notifyData)

		Return result

	End Function

	Private Function NotifySystemFinishingScanProcess(ByVal strPDFFullPath As String, ByVal mdGuid As String, ByVal pdfConvertGuid As String) As Boolean
		Dim result As Boolean = True

		Dim dbInfoData As ReportDBInformation = New ReportDBInformation
		Dim notifyData = New NotificationData
		dbInfoData.MDGuid = mdGuid
		dbInfoData.PDFFullPath = strPDFFullPath
		dbInfoData.OriginalFileGuid = pdfConvertGuid
		dbInfoData.WorkingForWebService = pdfConverterSettings.WorkingForWebService
		dbInfoData.Pages = m_Pagecount

		Dim header As String = "Scan-Job: Scan-Import abgeschlossen"
		Dim comments As String = "Anzahl verarbeitete Dokumente: {0}"
		comments = String.Format(comments, dataMatrixInfoIndex)

		notifyData.Customer_ID = mdGuid
		notifyData.NotifyArt = 5
		notifyData.NotifyComments = comments
		notifyData.NotifyHeader = header
		notifyData.CreatedFrom = "PDFConverter.NotifySystemFinishingScanProcess"

		result = result AndAlso m_ScanDataAccess.SendNotificationForImportedFiles(dbInfoData, notifyData)

		Return result

	End Function

	Private Function AlertForScanImport(ByVal strPDFFullPath As String, ByVal mdGuid As String, ByVal pdfConvertGuid As String) As Boolean
		Dim result As Boolean = True

		Dim dbInfoData As ReportDBInformation = New ReportDBInformation
		Dim notifyData = New NotificationData
		dbInfoData.MDGuid = mdGuid
		dbInfoData.PDFFullPath = strPDFFullPath
		dbInfoData.OriginalFileGuid = pdfConvertGuid
		dbInfoData.WorkingForWebService = pdfConverterSettings.WorkingForWebService
		dbInfoData.Pages = m_Pagecount

		Dim header As String = "Scan-Job: Scan-Error"
		Dim comments As String = "Ein gescanntest Dokument kann nicht analysiert werden.{0}Anzahlseiten: {1}{0}"
		comments &= "Dateiname: {2}{0}"
		comments &= "Scan-Kennung: {3}{0}"
		comments &= "Scan-Type: {4}{0}"
		comments = String.Format(comments, vbNewLine, dbInfoData.Pages, Path.GetFileName(dbInfoData.PDFFullPath), dbInfoData.OriginalFileGuid, dbInfoData.ScanModulType)

		notifyData.Customer_ID = mdGuid
		notifyData.NotifyArt = 99
		notifyData.NotifyComments = comments
		notifyData.NotifyHeader = header
		notifyData.CreatedFrom = "PDFConverter.AlertForScanImport"

		result = m_ScanDataAccess.SendNotificationForImportedFiles(dbInfoData, notifyData)

		Return result

	End Function

	''' <summary>
	''' Moves a pdf file to the processed scanned documents folder.
	''' </summary>
	''' <param name="originalPDFFilePath">The original pdf file path.</param>
	''' <param name="scannedDocumentsFolderPath">The scanned documents folder.</param>
	''' <param name="mandantGuid">The mandant guid.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function MovePDFToProcessedScannedDocuments(ByVal originalPDFFilePath As String, ByVal scannedDocumentsFolderPath As String, ByVal mandantGuid As String) As Boolean

		If Not Directory.Exists(scannedDocumentsFolderPath) Then
			m_logger.LogWarning(String.Format("The configurated scanned documents folder {0} does not exist.", scannedDocumentsFolderPath))
			Return False
		End If

		Dim mandantSubFolderPath As String = Path.Combine(scannedDocumentsFolderPath, mandantGuid)
		Dim success As Boolean = True

		Try

			If Not Directory.Exists(mandantSubFolderPath) Then
				' Create the mandant sub directory if it does not exist.
				Directory.CreateDirectory(mandantSubFolderPath)
			End If

			Dim originalPDFFileInfo As New FileInfo(originalPDFFilePath)

			If (originalPDFFileInfo.Exists) Then
				' Copy the pdf to the mandant sub directory
				Dim newFilePath As String = MoveOrCopyFileToFolder(originalPDFFilePath, mandantSubFolderPath, False)

				If newFilePath Is Nothing Then
					success = False
				End If

			Else
				m_logger.LogWarning(String.Format("The pdf file {0} does not exist.", originalPDFFilePath))
				success = False
			End If

		Catch ex As Exception
			m_logger.LogError(ex.ToString)
			success = False
		End Try

		Return success

	End Function

	Private Function DeleteSplittedFolder(ByVal workingFolderPath As String) As Boolean
		Dim success As Boolean = True

		Try
			If Directory.Exists(workingFolderPath) Then
				For Each _file As String In Directory.GetFiles(workingFolderPath)
					File.Delete(_file)
				Next
				For Each _folder As String In Directory.GetDirectories(workingFolderPath)

					DeleteSplittedFolder(_folder)
				Next
				System.IO.Directory.Delete(workingFolderPath, True)
			End If

		Catch ex As Exception
			m_logger.LogError(String.Format("Error: can not remove folder {0} | {1}.", workingFolderPath, ex.ToString))
			success = False

		End Try


		Return success

	End Function

#End Region

End Class

