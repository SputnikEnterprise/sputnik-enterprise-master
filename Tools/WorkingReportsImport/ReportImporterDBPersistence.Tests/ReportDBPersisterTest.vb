'------------------------------------
' File: ReportDBPersisterTest.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports ReportImporterCommon.Logging

<TestClass()>
Public Class ReportDBPersisterTest

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
    Public Sub SaveData_WorkingReportAlreadyExistsInDB_SaveDataReturnsFalse()

        ' ----------Arrange----------

        ' Simulate one existing report (second parameter = 1)
        Dim testableReportDBPersister As New TestableReportDBPersister("dummyConnectionString", 1)
        Dim returnValue As Boolean = True

        Dim reportDBInformation As New ReportDBInformation()
        reportDBInformation.DataMatrixCodeValue = "12345678_01_2012"

        ' ----------Act----------
        returnValue = testableReportDBPersister.SaveData(reportDBInformation)

        ' ----------Assert----------
        Assert.IsFalse(returnValue)

    End Sub

#End Region

End Class
