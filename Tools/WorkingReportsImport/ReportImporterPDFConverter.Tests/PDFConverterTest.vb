'------------------------------------
' File: PDFConverterTest.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports ReportImporterCommon.Logging
Imports ReportImporterDBPersistence

<TestClass()>
Public Class PDFConverterTest

#Region "Private Constants"

    Private Const TESTDATA_FOLDER = "PDFConverterTestData"
    Private Const TEST_PDF_FILE = TESTDATA_FOLDER & "\test.pdf"
    Private Const TEST_TEMP_FOLDER = "Temp"
    Private Const TEST_TEMP_FOLDER_DUMMY_FILE = TEST_TEMP_FOLDER & "\dummy.txt"
    Private Const TEST_MANDANT_GUID = "A0BB18D4-84EB-42d7-B366-721ED0E296EC"
    Private Const DATAMATRIX_LIB_X86_FOLDER = "DataMatrixRuntimeLibs\x86"
    Private Const DATAMATRIX_LIB_X86_FILE = DATAMATRIX_LIB_X86_FOLDER & "\SoftekBarcode.dll"
    Private Const DATAMATRIX_LIB_X64_FOLDER = "DataMatrixRuntimeLibs\x64"
    Private Const DATAMATRIX_LIB_X64_FILE = DATAMATRIX_LIB_X64_FOLDER & "\SoftekBarcode.dll"

#End Region

#Region "Public Methods"

    <TestInitialize()>
    Public Sub Startup()
        ' Create e logger delegate and install it into creator.
        Dim loggerMock As New Mock(Of ILogger)
		' No mock setup necessary, since Log does not return nothing.

		'Dim factoryObj As Factory.FilebasedLoggingCreator = Factory.FilebasedLoggingCreator.GetInstance()

		'' Install mock logger
		'Dim factoryAccessor As PrivateObject = New PrivateObject(factoryObj)
		'factoryAccessor.SetProperty("FileLogger", loggerMock.Object)
	End Sub

    <TestMethod()>
    <DeploymentItem(TESTDATA_FOLDER, TESTDATA_FOLDER)>
    <DeploymentItem(TEST_TEMP_FOLDER_DUMMY_FILE, TEST_TEMP_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ConvertPDFReport_ValidPDFIsPassed_PDFCanBeProccessedSuccessfully()

        ' ----------Arrange----------

        Dim pdfConverterSettings As New PDFConverterSettings With {
                                                                   .ProcessedScannedDocumentsFolder = TEST_TEMP_FOLDER,
                                                                   .ScanJobsConnectionString = "dummyConnectionString",
                                                                   .TemporaryFolder = TEST_TEMP_FOLDER}
        Dim pdfConverter As New PDFConverter(pdfConverterSettings)


        ' Mock the database persister.
        Dim dbPersisterMock As New Mock(Of IReportDBPersister)
		dbPersisterMock.Setup(Function(c) c.SaveCompleteFile(It.IsAny(Of ReportDBInformation))).Returns(True)

        ' Install mock database persister.
        Dim pdfConverterPrivateObject As PrivateObject = New PrivateObject(pdfConverter)
        pdfConverterPrivateObject.SetField("reportDBPersister", dbPersisterMock.Object)

        ' ----------Act----------

        pdfConverter.ConvertPDFReport(TEST_PDF_FILE, TEST_MANDANT_GUID)


        ' ----------Assert----------

        ' Assert that the control goes through the hole process and at the end the method for persisting is called.
        dbPersisterMock.Verify(Sub(o As IReportDBPersister) o.SaveData(It.IsAny(Of ReportDBInformation)), Times.Exactly(7))

    End Sub

#End Region

End Class
