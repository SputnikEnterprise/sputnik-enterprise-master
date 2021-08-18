''------------------------------------
'' File: FileLogger.vb
'' ©2011 Sputnik Informatik GmbH
''------------------------------------

'Imports log4net

'Namespace Logging

'    Friend Class FileLogger
'        Implements ILogger

'#Region "Private fields"

'        ''' <summary>
'        ''' Log4Net logger
'        ''' </summary>
'        Private Shared singeltonLogger As ILog = LogManager.GetLogger("LogFileAppender")

'#End Region

'#Region "Public methods"

'        ''' <summary>
'        ''' Writes a message to the file.
'        ''' </summary>
'        ''' <param name="message">The message text</param>
'        Public Sub Log(ByVal message As String, ByVal logLevel As ILogger.LogLevel) Implements ILogger.Log
'            Select Case logLevel
'                Case ILogger.LogLevel.InfoLevel
'                    FileLogger.singeltonLogger.Info(message)
'                Case ILogger.LogLevel.DebugLevel
'                    FileLogger.singeltonLogger.Debug(message)
'                Case ILogger.LogLevel.ErrorLevel
'                    FileLogger.singeltonLogger.Error(message)
'                Case Else
'                    ' Do noting
'            End Select
'        End Sub

'#End Region

'    End Class

'End Namespace