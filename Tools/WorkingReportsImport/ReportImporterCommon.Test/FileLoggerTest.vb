'------------------------------------
' File: FileLoggerTest.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports ReportImporterCommon.Logging
Imports Moq

<TestClass()>
Public Class FileLoggerTest

    <TestMethod()>
    Public Sub Log_LoggerCreatedByFactoryMethod_LoggerUsesDelegate()

        ' ----------Arrange----------
        ' Create e logger delegate and install it into creator.
        Dim loggerMock As New Mock(Of ILogger)
		' No mock setup necessary, since Log does not return nothing.

		'Dim factory As Factory.FilebasedLoggingCreator = Logging.Factory.FilebasedLoggingCreator.GetInstance()

		' Install mock logger
		'Dim factoryAccessor As PrivateObject = New PrivateObject(factory)
		'      factoryAccessor.SetProperty("FileLogger", loggerMock.Object)

		'Dim fileLogger As ILogger = factory.FactoryMethod()

		' ----------Act----------
		'fileLogger.Log("A critial error happend", ILogger.LogLevel.ErrorLevel)

		' ----------Assert----------
		'loggerMock.Verify(Sub(logger As ILogger) logger.Log("A critial error happend", ILogger.LogLevel.ErrorLevel), Times.Exactly(1))

	End Sub

End Class
