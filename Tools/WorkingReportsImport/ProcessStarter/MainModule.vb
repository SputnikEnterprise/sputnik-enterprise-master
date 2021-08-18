'------------------------------------
' File: MainModule.vb
' Date: 16.11.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports ReportImporterPDFConverter
Imports ReportImporterDataMatrixAnalyzer
Imports System.IO
Imports ReportImporterCommon

Module MainModule


	Private m_SettingFile As ProgramSettings

	''' <summary>
	''' Application entry point.
	''' </summary>
	''' <param name="args">The args.</param>
	Sub Main(ByVal args As String())

		Dim settingObj = New ReportImporterCommon.SettingFile()
		m_SettingFile = settingObj.ReadSettingFile
		If m_SettingFile Is Nothing Then
			Console.Out.WriteLine(String.Format("settingfile could not be readed!: {0}", m_SettingFile.CurrentSettingFilename))
		End If

		Console.Out.WriteLine(String.Format("settingfile: {1}{0}scanjob connection: {2}{0}systeminfo connection: {3}",
											vbNewLine, m_SettingFile.CurrentSettingFilename, m_SettingFile.ConnstringScanjobs, m_SettingFile.ConnstringSysteminfo))

		If args.Count > 0 Then

			' Read the command
			Dim command As String = args(0)

			Select Case command
				Case "CONVERT_PDF"
					ConvertPDF(args)
				Case "ANALYZE_PDF"
					AnalyzePDF(args)
				Case "START_FILESYSTEMWATCHER"
					StartFileSystemWatcher()
			End Select

		Else
			' Describe usage of tool:
			Console.Out.WriteLine("Usage of ProcessStarter:")
			Console.Out.WriteLine("ProcessStarter [CONVERT_PDF|ANALYZE_PDF|START_FILESYSTEMWATCHER] [Parameter] ")
			Console.Out.WriteLine("ProcessStarter CONVERT_PDF <pdfPath> <mdguid> ")
			Console.Out.WriteLine("ProcessStarter ANALYZE_PDF <pdfPath>")
			Console.Out.WriteLine("ProcessStarter START_FILESYSTEMWATCHER")

		End If
	End Sub

	''' <summary>
	''' Convert a pdf.
	''' </summary>
	''' <param name="args">The args</param>
	Private Sub ConvertPDF(ByVal args As String())

		If (args.Count = 3) Then

			Dim pdfPath As String = args(1)
			Dim mdGuid As String = args(2)

			' Settings used by the pdf converter.
			Dim pdfConverterSettings As New PDFConverterSettings With {
				.TemporaryFolder = My.Settings.TemporaryFolder,
				.ProcessedScannedDocumentsFolder = My.Settings.ProcessedScannedDocuments,
				.ScanJobsConnectionString = My.Settings.ScanJobsConnectionString,
				.WorkingForWebService = My.Settings.WorkingForWebService,
				.SettingFileValue = m_SettingFile,
				.CustomerID = mdGuid
			}

			Dim pdfConverter As New PDFConverter(pdfConverterSettings)
			pdfConverter.ConvertPDFReport(pdfPath, mdGuid)

		End If

	End Sub

	''' <summary>
	''' Analyze a PDF.
	''' </summary>
	''' <param name="args">The args.</param>
	Private Sub AnalyzePDF(ByVal args As String())

		If (args.Count = 2) Then

			Dim pdfPath As String = args(1)
			Dim mandantGuid As String = Directory.GetParent(pdfPath).Name
			Dim mandantFolder As String = Directory.GetParent(pdfPath).ToString

			Dim analyzer As New DataMatrixAnalyzer(mandantFolder, mandantGuid)
			analyzer.SettingFileValue = m_SettingFile
			Dim result As DataMatrixResult = Nothing

			result = analyzer.ReadDataMatrixCode(pdfPath)

			' Print found DataMatrix codes.
			If Not (result Is Nothing) Then
				Console.WriteLine(String.Format("Number of found DataMatrix codes: {0}", result.DataMatrixInfos.Count))

				For i As Integer = 0 To result.DataMatrixInfos.Count - 1
					Console.WriteLine(String.Format("Value of #{0}: {1}", (i + 1), result.DataMatrixInfos(i).DataMatrixValue))
				Next

			End If

		End If

	End Sub

	''' <summary>
	''' Start file system watcher.
	''' </summary>
	Private Sub StartFileSystemWatcher()

		Dim FileListenerSettings As New ReportImporterFileListener.ReportFileListenerSettings With {
			.bNotifyOnDispose = True,
			.Folder2Watch = m_SettingFile.ScanDirectoryToListen,
			.ConnStr4ScanDb = m_SettingFile.ConnstringScanjobs,
			.Folder4ProcessedScannedDocuments = m_SettingFile.ProcessedScannedDocuments,
			.SendNotificationTo = m_SettingFile.NotifyEMailToScanJob,
			.SmtpPort = m_SettingFile.SmtpPort,
			.SmtpServer = m_SettingFile.SmtpNotificationServer,
			.Folder4TemporaryFiles = m_SettingFile.TemporaryFolder,
			.WorkingForWebService = True,
		.SettingFileValue = m_SettingFile
		}

		Dim mainModule As New ReportImporterFileListener.MainModule(FileListenerSettings)
		mainModule.StartFileListener()

		' Waits for a ke event
		Console.WriteLine("Press any key to stop listener.")
		Console.Read()

		'If key read, stop.
		mainModule.StopFileListener()

	End Sub

End Module
