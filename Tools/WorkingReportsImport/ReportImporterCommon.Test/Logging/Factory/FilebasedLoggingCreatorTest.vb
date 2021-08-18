'------------------------------------
' File: FilebasedLoggingCreatorTest.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports ReportImporterCommon.Logging

<TestClass()>
Public Class FilebasedLoggingCreatorTest

    <TestMethod()>
    Public Sub FactoryMethod_CreateFileLogger_CreatesInstanceOfFileLogger()
		' ----------Arrange----------
		'Dim creator As Factory.ILoggingCreator = Factory.FilebasedLoggingCreator.GetInstance()

		'' ----------Act----------
		'Dim logger As ILogger = creator.FactoryMethod()

		'' ----------Assert----------
		'' Logger is of correct dynamic type
		'Assert.IsTrue(TypeOf (logger) Is Logger)

	End Sub

    <TestMethod()>
    Public Sub FactoryMethod_IsCalledMoreTimes_SameInstanceIsReturned()
		' ----------Arrange----------
		'Dim creator As Factory.ILoggingCreator = Factory.FilebasedLoggingCreator.GetInstance()

		'' ----------Act----------
		'Dim logger1 As ILogger = creator.FactoryMethod()
		'Dim logger2 As ILogger = creator.FactoryMethod()

		'' ----------Assert----------
		'Assert.IsTrue(logger1.Equals(logger2))
	End Sub

End Class
