'------------------------------------
' File: MainModule.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports System.Threading

Imports ReportImporterCommon.StringExtensions
Imports ReportImporterCommon.Notification

Imports ReportImporterPDFConverter
Imports ReportImporterCommon.FileHandling
Imports ReportImporterCommon.Logging
Imports ReportImporterCommon

Public Class MainModule

#Region "Private constants"
	' Regular expression used to test for a valid GUID.
	Private Const GUID_REGULAR_EXPRESSION As String = "^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$"
#End Region

#Region "Private fields"

	''' <summary>
	''' The logger object.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	'''  This is the main service thread.
	''' </summary>
	Private serviceThread As Thread

	''' <summary>
	''' The directory to listen.
	''' </summary>
	Private directoryToListen As String

	''' <summary>
	'''  The filewatchter instance.
	''' </summary>
	''' <remarks></remarks>
	Private fileWatcher As FileWatcher

	''' <summary>
	'''  The pdf converter.
	''' </summary>
	Private pdfConverter As IPDFConverter

	''' <summary>
	''' This field stores whether the service is running.
	''' </summary>
	''' <remarks>
	''' It must be set to false to leave the main service thread, only when the service is stopped.
	''' </remarks>
	Private serviceStarted As Boolean

	''' <summary>
	''' This thread safe queue is filled with jobs from the file system watcher.
	''' </summary>
	''' <remarks>The working thread will dequeue the jobs and proccess them.</remarks>
	Private synchronizedQueueWrapper As Queue

	Private m_SettingFile As ProgramSettings

	Private FileListenerSetting As New ReportFileListenerSettings

#End Region

#Region "Constructor"

	Public Sub New(ByVal _FileListenSetting As ReportFileListenerSettings)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()
		FileListenerSetting = _FileListenSetting
		m_SettingFile = FileListenerSetting.SettingFileValue

		' Settings used by the pdf converter.
		m_Logger.LogInfo(String.Format("ProcessedScannedDocumentsFolder: {0}", _FileListenSetting.Folder4ProcessedScannedDocuments))

		Dim pdfConverterSettings As New PDFConverterSettings With {.TemporaryFolder = _FileListenSetting.Folder4TemporaryFiles,
			.ProcessedScannedDocumentsFolder = _FileListenSetting.Folder4ProcessedScannedDocuments,
			.ScanJobsConnectionString = _FileListenSetting.ConnStr4ScanDb,
			.WorkingForWebService = True,
		.SettingFileValue = _FileListenSetting.SettingFileValue
		}

		' Create the pdf converter.
		Me.pdfConverter = New PDFConverter(pdfConverterSettings)

	End Sub

#End Region

#Region "Public Methods"

	''' <summary>
	''' Used to start the file system watcher not as Windows Service, but from the ProcessStarter.
	''' </summary>
	''' <remarks>Used for testing withouth windows service.</remarks>
	Public Sub StartFileListener()
		Me.OnStart(Nothing)
	End Sub

	''' <summary>
	''' Used to start the file system watcher not as Windows Service, but from the ProcessStarter.
	''' </summary>
	''' <param name="directoryToListen">The directory to listen which should be used (overrides the My.Settings dictionary setting).</param>
	''' <remarks>Used for testing without a windows service.</remarks>
	Public Sub StartFileListener(ByVal directoryToListen)
		Me.directoryToListen = directoryToListen
		Me.OnStart(Nothing)
	End Sub

	''' <summary>
	''' Stop the file system watcher.
	''' </summary>
	''' <remarks>Used for testing withouth windows service.</remarks>
	Public Sub StopFileListener()
		Me.OnStop()
	End Sub

#End Region

#Region "Protected Methods"

	''' <summary>
	''' Insert Code to start the service.
	''' </summary>
	''' <param name="args">The arguments that can be passed to the service.</param>
	Protected Overrides Sub OnStart(ByVal args() As String)

		' Depending on which StartFileListener Method is called, this field is already set or not.
		If Me.directoryToListen Is Nothing Then
			Me.directoryToListen = FileListenerSetting.Folder2Watch
		Else
			' Use already set directoryToListen
		End If

		Dim queue As New Queue
		synchronizedQueueWrapper = Queue.Synchronized(queue)

		' Configure the environment
		Me.fileWatcher = New FileWatcher()
		If Me.fileWatcher.Configure(Me.directoryToListen, m_SettingFile.ScanFileFilter, FileListenerSetting, AddressOf HandleWorkingReport) Then

			' Create worker thread; this will invoke the WorkerFunction when we start it.
			' Since we use a separate worker thread, the main service thread will return quickly, telling Windows that service has started.
			Dim threadStart As New ThreadStart(AddressOf WorkerFunction)
			serviceThread = New Thread(threadStart)

			' Set flag to indicate worker thread is active
			Me.serviceStarted = True

			Dim directoryToListenInfo As New DirectoryInfo(Me.directoryToListen)

			' Add existing pdf files to the job queue.
			For Each existingReport As FileInfo In directoryToListenInfo.GetFiles(m_SettingFile.ScanFileFilter, System.IO.SearchOption.AllDirectories)
				synchronizedQueueWrapper.Enqueue(existingReport.FullName)
			Next

			' Start the thread
			serviceThread.Start()
		Else
			' The file watcher could not be configured, so by not starting any thread, the service will automatically be stopped.
			m_Logger.LogWarning("The file listener could not be started, since to a configuration error. The service will be stopped.")
			Me.Stop()
		End If
	End Sub

	''' <summary>
	''' Insert Code to start the service.
	''' </summary>
	Protected Overrides Sub OnStop()
		' Code to end service
		If serviceStarted Then
			serviceStarted = False
			serviceThread.Join(New TimeSpan(0, 0, 2))
		End If

		Me.fileWatcher.Dispose()
	End Sub
#End Region

#Region "Methods"

	''' <summary>
	''' This is the main loop in the service.
	''' </summary>
	Private Sub WorkerFunction()

		Dim proccedFileNumber As Integer = 0
		Dim loopCount As Integer = 0

		' Start an endless loop; loop will abort only when "serviceStarted" flag = false
		While serviceStarted

			If Me.synchronizedQueueWrapper.Count > 0 Then

				Dim reportPath As String = synchronizedQueueWrapper.Dequeue

				Thread.Sleep(New TimeSpan(0, 0, 10))

				Dim reportFileInfo As New FileInfo(reportPath)

				If reportFileInfo.IsFileUsedByAnotherProcess() Then
					Me.synchronizedQueueWrapper.Enqueue(reportPath)
					m_Logger.LogInfo(String.Format("The file {0} is in used...", reportFileInfo.FullName))
					Thread.Sleep(New TimeSpan(0, 0, 10))

					loopCount += 1
					If loopCount >= 10 Then
						Dim strBody As String = "file in USE!!! {0}{1}{0}file: {2}"
						Dim strSubject As String = "Automatische Nachricht ({0}: {1})"
						strBody = String.Format(strBody, vbNewLine, My.Computer.Name, reportFileInfo.FullName)
						strSubject = String.Format(strSubject, My.Application.Info.AssemblyName, My.Computer.Name)

						If loopCount = 10 Then
							Try

								Dim m_NotifyUtility As New Notification.Notifying
								m_NotifyUtility.SettingFileData = FileListenerSetting.SettingFileValue

								Dim result = m_NotifyUtility.SendMailToWithExchange("info@sputnik-it.com", FileListenerSetting.SettingFileValue.NotifyEMailToScanJob, "", strSubject, strBody, Nothing)

								m_Logger.LogDebug(String.Format("Sendresult: sending mailnotification to admin: {0}", result))
								loopCount = 0

								Thread.Sleep(New TimeSpan(0, 0, 10))
							Catch ex As Exception
								m_Logger.LogWarning(String.Format("error during sending mail for file in use! {0}", reportFileInfo.FullName))

								Thread.Sleep(New TimeSpan(0, 0, 10))

							End Try
						End If

					End If

					Continue While
				End If

				If Me.IsValidReportFile(reportFileInfo) Then

					proccedFileNumber += 1
					m_Logger.LogInfo(String.Format("Jobnumber {0} with file {1} will now be processing...", proccedFileNumber, reportFileInfo.FullName))

					' The dictionary name of the report file is used as the mandant guid.
					Dim mandantGuid As String = reportFileInfo.Directory.Name

					'Do something with the working report
					m_Logger.LogInfo(String.Format("Pass working report {0} to Converter!", reportPath))

					pdfConverter.ConvertPDFReport(reportPath, mandantGuid)

					m_Logger.LogInfo(String.Format("Jobnumber {0} for file {1} is now finished!", proccedFileNumber, reportPath))

				Else
					m_Logger.LogInfo(String.Format("The file {0} is invalid and will not be processed.", reportFileInfo.FullName))
				End If
				m_Logger.LogInfo(String.Format("waiting for new job..."))

			End If

			If serviceStarted Then
				Thread.Sleep(New TimeSpan(0, 0, 1))
			End If

		End While

		' Time to end the thread
		Thread.CurrentThread.Abort()
	End Sub

	''' <summary>
	''' Checks if a file is a valid report file.
	''' 
	''' A valid file must fulfill the following requirements:
	''' - The file filter must be set (eg. *.pdf)
	''' - The file must be in a direct sub folder (a mandant folder) of the import folder.
	''' - The name of the mandant folder must be a valid GUID.
	''' </summary>
	''' <param name="fileInfo">The file to check.</param>
	''' <returns>Boolean truth value indicating if the file is valid.</returns>
	Private Function IsValidReportFile(ByVal fileInfo As FileInfo) As Boolean

		If Not fileInfo.Exists Then Return False

		Dim fileFilter As String = m_SettingFile.ScanFileFilter
		If (String.IsNullOrEmpty(fileFilter)) Then
			m_Logger.LogWarning(String.Format("The file filter {0} is not valid.", fileFilter))
			Return False
		End If

		Dim valid = True
		Dim importFolderDirectoryInfo As New DirectoryInfo(Me.directoryToListen)

		' Checks that the file was placed in a child directory (mandant directory) of the import folder.
		If Not fileInfo.Directory.Parent.FullName = importFolderDirectoryInfo.FullName Then

			m_Logger.LogWarning(String.Format("The file {0} was not placed in a mandant sub directory. Please put the file into a mandant sub directory.", fileInfo.FullName))
			valid = False
		Else

			' Checks that the mandant directory name is a valid guid.
			Dim guidResult As Guid
			If Not Guid.TryParse(fileInfo.Directory.Name, guidResult) Then
				m_Logger.LogWarning(String.Format("The name of the directory {0} is not a valid GUID.", fileInfo.Directory.Name))
				Return False
				End If

			' Checks that the file extension is correct.
			If Not fileInfo.Extension.ToLower() = fileFilter.Substring(1) Then
					m_Logger.LogWarning(String.Format("The file {0} does not match with the file filter {1}", fileInfo.FullName, fileFilter))
					valid = False
				End If

			End If

			Return valid
	End Function

	''' <summary>
	''' Callback method for the file listener.
	''' </summary>
	''' <param name="reportPath">The path to the report file.</param>
	''' <remarks>As soon as a file is dropped in the folder, this callback method is called.</remarks>
	Private Sub HandleWorkingReport(ByVal reportPath As String)

		' Queue the job.
		Me.synchronizedQueueWrapper.Enqueue(reportPath)

	End Sub

#End Region

End Class
