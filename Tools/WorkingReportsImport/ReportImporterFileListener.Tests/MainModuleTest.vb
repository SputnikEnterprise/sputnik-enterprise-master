'------------------------------------
' File: MainModuleTest.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports ReportImporterCommon.Logging
Imports ReportImporterPDFConverter

<TestClass()>
Public Class MainModuleTest

#Region "Private Constants"

  Private Const TEST_FILE_WATCHER_FOLDER = "WatchedFolder"
  Private Const TEST_FILE_WATCHER_MANDANT_FOLDER = TEST_FILE_WATCHER_FOLDER & "\A0BB18D4-84EB-42d7-B366-721ED0E296EC"
  Private Const TEST_FILE_WATCHER_EXISING_PDF = TEST_FILE_WATCHER_MANDANT_FOLDER & "\existingPDF.pdf"
  Private Const TEST_FILE_WATCHER_FILE_TO_FILTER = TEST_FILE_WATCHER_MANDANT_FOLDER & "\shouldBeFiltered.txt"

#End Region

#Region "Public Methods"

  <TestInitialize()>
  Public Sub Startup()
    ' Create e logger delegate and install it into creator.
    Dim loggerMock As New Mock(Of ILogger)
		' No mock setup necessary, since Log does not return nothing.

		'Dim factoryObj As Factory.FilebasedLoggingCreator = Factory.FilebasedLoggingCreator.GetInstance()

		' Install mock logger
		'Dim factoryAccessor As PrivateObject = New PrivateObject(factoryObj)
		'  factoryAccessor.SetProperty("FileLogger", loggerMock.Object)
	End Sub


  <TestMethod()>
  <DeploymentItem(TEST_FILE_WATCHER_EXISING_PDF, TEST_FILE_WATCHER_MANDANT_FOLDER)>
  <DeploymentItem(TEST_FILE_WATCHER_FILE_TO_FILTER, TEST_FILE_WATCHER_MANDANT_FOLDER)>
  Public Sub WorkerFunction_MandantFolderHasOneValidPDFFileAndOneFileThatShouldBeFilteredOut_ThePDFConverterIsCalledExactlyOnceForThePDF()

    ' ----------Arrange----------

    ' Mock the pdf converter.
    Dim pdfConverterMock As New Mock(Of IPDFConverter)
    'Dim FileListenerSettings As New ReportImporterFileListener.ReportFileListenerSettings With {.bNotifyOnScan = "", _
    '                                                                                        .bNotifyOnDispose = "", _
    '                                                                                        .Folder2Watch = "", _
    '                                                                                        .ConnStr4ScanDb = "", _
    '                                                                                        .Folder4ProcessedScannedDocuments = "", _
    '                                                                                        .SendNotificationTo = "", _
    '                                                                                        .SmtpPort = "", _
    '                                                                                        .SmtpServer = ""}

    'Dim mainModule As New MainModule(FileListenerSettings)

    '' Install mock pdf converter
    'Dim mailModulePrivateObject As PrivateObject = New PrivateObject(mainModule)
    'mailModulePrivateObject.SetField("pdfConverter", pdfConverterMock.Object)

    '' ----------Act----------

    'mainModule.StartFileListener(TEST_FILE_WATCHER_FOLDER)

    '' Wait a little bit so that the file listener and the proccessing thread can start up.
    'Threading.Thread.Sleep(5000)

    'mainModule.StopFileListener()

    ' ----------Assert----------

    pdfConverterMock.Verify(Sub(o As IPDFConverter) o.ConvertPDFReport(It.IsAny(Of String), It.IsAny(Of String)), Times.Once)

  End Sub

#End Region

End Class
