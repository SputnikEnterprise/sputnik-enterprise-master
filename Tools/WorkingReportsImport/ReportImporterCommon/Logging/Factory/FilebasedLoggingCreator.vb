''------------------------------------
'' File: FilebasedLoggingFactory.vb
'' This implements the Creator.
'' (Singleton implementation)
'' ©2011 Sputnik Informatik GmbH
''------------------------------------

'Imports ReportImporterCommon.Logging
'Imports System.Runtime.CompilerServices

'Namespace Logging.Factory

'    Public Class FilebasedLoggingCreator
'        Implements ILoggingCreator

'#Region "Private fields"

'        ''' <summary>
'        ''' The singleton instance
'        ''' </summary>
'        ''' <remarks></remarks>
'        Private Shared singleton As ILoggingCreator

'        ''' <summary>
'        ''' Hold one single instance of FileLogger
'        ''' </summary>
'        Private Shared singleFileLogger As ILogger
'#End Region


'#Region "Private properties"

'        Private Property FileLogger As ILogger
'            Get
'                If FilebasedLoggingCreator.singleFileLogger Is Nothing Then
'                    FilebasedLoggingCreator.singleFileLogger = New Logger(New FileLogger())
'                End If
'                Return FilebasedLoggingCreator.singleFileLogger
'            End Get

'            Set(ByVal value As ILogger)
'                If Not FilebasedLoggingCreator.singleFileLogger Is Nothing Then
'                    FilebasedLoggingCreator.singleFileLogger = Nothing
'                End If
'                FilebasedLoggingCreator.singleFileLogger = New Logger(value)
'            End Set
'        End Property
'#End Region

'#Region "Constructor"

'        ''' <summary>
'        ''' Make Constructor private, because of Singleton pattern.
'        ''' </summary>
'        ''' <remarks>Access logger through GetInstance.</remarks>
'        Private Sub New()
'        End Sub

'#End Region

'#Region "Public methods"

'        ''' <summary>
'        ''' Factory method that creates a filebased ILogger object.
'        ''' </summary>
'        ''' <returns>A filebased ILogger object</returns>
'        Public Function FactoryMethod() As ILogger Implements ILoggingCreator.FactoryMethod
'            Return Me.FileLogger
'        End Function

'        ''' <summary>
'        ''' Singleton implementation for logging creator.
'        ''' </summary>
'        ''' <returns>A FileLoggerCreator instance.</returns>        
'        Public Shared Function GetInstance() As ILoggingCreator
'            If IsNothing(FilebasedLoggingCreator.singleton) Then
'                FilebasedLoggingCreator.singleton = New FilebasedLoggingCreator()
'            End If
'            Return FilebasedLoggingCreator.singleton
'        End Function

'#End Region

'    End Class


'End Namespace
