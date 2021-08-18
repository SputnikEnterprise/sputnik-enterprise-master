'------------------------------------
' File: DataMatrixAnalyzerTests.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports ReportImporterCommon.Logging
Imports ReportImporterDataMatrixAnalyzer
Imports System.IO

<TestClass()>
Public Class DataMatrixAnalyzerTests

#Region "Private Constants"

    Private Const PDF_TESTDATA_FOLDER = "PDF"
    Private Const DATAMATRIX_LIB_X86_FOLDER = "DataMatrixRuntimeLibs\x86"
    Private Const DATAMATRIX_LIB_X86_FILE = DATAMATRIX_LIB_X86_FOLDER & "\SoftekBarcode.dll"
    Private Const DATAMATRIX_LIB_X64_FOLDER = "DataMatrixRuntimeLibs\x64"
    Private Const DATAMATRIX_LIB_X64_FILE = DATAMATRIX_LIB_X64_FOLDER & "\SoftekBarcode.dll"

#End Region

    <TestInitialize()>
    Public Sub Startup()
        ' Create a logger delegate and install it into creator.
        Dim loggerMock As New Mock(Of ILogger)
		' No mock setup necessary, since Log does not return nothing.

		'Dim factoryObj As Factory.FilebasedLoggingCreator = Factory.FilebasedLoggingCreator.GetInstance()

		'' Install mock logger
		'Dim factoryAccessor As PrivateObject = New PrivateObject(factoryObj)
		'factoryAccessor.SetProperty("FileLogger", loggerMock.Object)

	End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCode_DataMatrixCodeIsPresentOnInclindedScan_DataMatrixCanBeDetected()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "inclined.pdf"))

        ' ----------Assert----------
        Assert.IsTrue(dataMatrixResult.CouldPDFFileBeAnalyzed)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos.Count = 1)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos(0).DataMatrixValue = "6666123_12_2010")
    End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCode_DataMatrixCodeIsPresentOn90DegreeScan_DataMatrixCanBeDetected()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "90degree.pdf"))

        ' ----------Assert----------
        Assert.IsTrue(dataMatrixResult.CouldPDFFileBeAnalyzed)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos.Count = 1)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos(0).DataMatrixValue = "12654123_45_2011")
    End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCode_DataMatrixCodeIsPresentOn180DegreeScan_DataMatrixCanBeDetected()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "180degree.pdf"))

        ' ----------Assert----------
        Assert.IsTrue(dataMatrixResult.CouldPDFFileBeAnalyzed)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos.Count = 1)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos(0).DataMatrixValue = "12654123_45_2011")
    End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCodee_DataMatrixCodeIsPresentOn270DegreeScan_DataMatrixCanBeDetected()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "270degree.pdf"))

        ' ----------Assert----------
        Assert.IsTrue(dataMatrixResult.CouldPDFFileBeAnalyzed)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos.Count = 1)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos(0).DataMatrixValue = "12654123_45_2011")
    End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCode_DataMatrixCodeIsNotPresent_EmptyResultIsReturned()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "noDataMatrixCode.pdf"))

        ' ----------Assert----------
        Assert.IsNotNull(dataMatrixResult)
        Assert.IsTrue(dataMatrixResult.CouldPDFFileBeAnalyzed)
        Assert.IsNotNull(dataMatrixResult.DataMatrixInfos)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos.Count = 0)
    End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCodee_PDFFileDoesNotExist_CouldPDFFileBeAnalyzedIsFalse()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "nonExisting.pdf"))

        ' ----------Assert----------
        Assert.IsNotNull(dataMatrixResult)
        Assert.IsFalse(dataMatrixResult.CouldPDFFileBeAnalyzed)
    End Sub

    <TestMethod()>
    <DeploymentItem(PDF_TESTDATA_FOLDER, PDF_TESTDATA_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X86_FILE, DATAMATRIX_LIB_X86_FOLDER)>
    <DeploymentItem(DATAMATRIX_LIB_X64_FILE, DATAMATRIX_LIB_X64_FOLDER)>
    Public Sub ReadDataMatrixCode_DataMatrixCodeIsPresentOnScanWithNoise_DataMatrixCanBeDetectedWithThresholdMode()

        Dim dataMatrixResult As DataMatrixResult = Nothing

        ' ----------Arrange----------
        Dim dataMatrixAnalyzer As New DataMatrixAnalyzer()

        ' ----------Act-------------
        dataMatrixResult = dataMatrixAnalyzer.ReadDataMatrixCode(Path.Combine(PDF_TESTDATA_FOLDER, "noise.pdf"))

        ' ----------Assert----------
        Assert.IsTrue(dataMatrixResult.CouldPDFFileBeAnalyzed)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos.Count = 1)
        Assert.IsTrue(dataMatrixResult.DataMatrixInfos(0).DataMatrixValue = "6666123_12_2010")
    End Sub

End Class
