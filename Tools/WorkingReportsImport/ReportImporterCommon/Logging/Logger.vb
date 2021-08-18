'------------------------------------
' File: Logger.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports NLog

Namespace Logging

	Public Class Logger
		Implements ILogger

#Region "Private fields"

		'''' <summary>
		'''' Implementation for logger.
		'''' </summary>
		'Private implementation As ILogger

		''' <summary>
		''' The NLog logger.
		''' </summary>
		Private m_Logger As NLog.Logger = LogManager.GetCurrentClassLogger()

#End Region

#Region "Constructor"

		'''' <summary>
		'''' Make Constructor private.
		'''' </summary>
		'''' <remarks>Access logger through getInstance.</remarks>
		'Public Sub New(ByVal loggerImpl As ILogger)
		'	Me.implementation = loggerImpl
		'End Sub

		Public Sub New()
			'Me.implementation = ILogger.LogLevel.InfoLevel
		End Sub

#End Region

#Region "Public methods"

		'''' <summary>
		'''' Writes a message to the console.
		'''' </summary>
		'''' <param name="message">The message text</param>
		'''' <remarks>Delegates it to the implementation.</remarks>
		'Public Sub Log(ByVal message As String, ByVal logLevel As ILogger.LogLevel) Implements ILogger.Log
		'	Me.implementation.Log(message, logLevel)
		'End Sub

		''' <summary>
		''' Log info message
		''' </summary>
		''' <param name="errorMessage"></param>
		''' <remarks></remarks>
		Public Sub LogInfo(ByVal errorMessage As String) Implements ILogger.LogInfo
			m_Logger.Info(errorMessage)
		End Sub

		''' <summary>
		''' Log debug message
		''' </summary>
		''' <param name="errorMessage"></param>
		''' <remarks></remarks>
		Public Sub LogDebug(ByVal errorMessage As String) Implements ILogger.LogDebug
			m_Logger.Debug(errorMessage)
		End Sub

		''' <summary>
		''' Log error message.
		''' </summary>
		''' <param name="errorMessage">The error message.</param>
		Public Sub LogError(ByVal errorMessage As String) Implements ILogger.LogError
			m_Logger.Error(errorMessage)
		End Sub

		''' <summary>
		''' Log warning message.
		''' </summary>
		''' <param name="warningMessage">The warning message.</param>
		Public Sub LogWarning(ByVal warningMessage As String) Implements ILogger.LogWarning
			m_Logger.Warn(warningMessage)
		End Sub

#End Region

	End Class

End Namespace