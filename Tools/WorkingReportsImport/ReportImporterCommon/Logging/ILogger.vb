'------------------------------------
' File: ILogger.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Runtime.CompilerServices

'<Assembly: InternalsVisibleTo("ReportImporterCommon.Tests")> 

Namespace Logging

	''' <summary>
	''' Interface for logger implementations
	''' </summary>
	Public Interface ILogger

		'	''' <summary>
		'	''' The log level
		'	''' </summary>
		'	Enum LogLevel
		'		''' <summary>
		'		''' Info level
		'		''' </summary>
		'		''' <remarks>Use this for logging useful informations.</remarks>
		'		InfoLevel

		'		''' <summary>
		'		''' Debug level
		'		''' </summary>
		'		''' <remarks>Use this for debugging purposese.</remarks>
		'		DebugLevel

		'		''' <summary>
		'		''' Error level
		'		''' </summary>
		'		''' <remarks>Use this to log errors.</remarks>
		'		ErrorLevel
		'	End Enum


		'''' <summary>
		'''' Writes an log message.
		'''' </summary>
		'''' <param name="message">The message text</param>
		'''' <param name="logLevel">The log level</param>
		'Sub Log(ByVal message As String, ByVal logLevel As LogLevel)

		Sub LogError(ByVal errorMessage As String)
		Sub LogWarning(ByVal warningMessage As String)

		Sub LogInfo(errorMessage As String)
		Sub LogDebug(errorMessage As String)

	End Interface


End Namespace
