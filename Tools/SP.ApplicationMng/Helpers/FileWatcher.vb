
Imports System.IO
Imports SP.Infrastructure.Logging

Public Class FileWatcher
	Implements IDisposable

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


#Region "Private fields"

	Private watcher As FileSystemWatcher

	Private Shared fileHandlingAction As Action(Of String)

	Private disposedValue As Boolean

	Private FileListenerSetting As New ReportFileListenerSettings

#End Region

#Region "Constructors"

	''' <summary>
	''' The default constructor.
	''' </summary>
	Public Sub New()
		Me.disposedValue = False
	End Sub

#End Region

#Region "Public methods"

	''' <summary>
	''' Configures the file watcher.
	''' </summary>
	''' <param name="directoryToListen">The directory that will be listened to.</param>
	''' <param name="fileFilter">The file filter, e.g. "*.pdf"</param>
	''' <param name="fileHandlingAction">The callback for handling file listening events.</param>
	''' <returns>True, if it could be initialized, false otherwise.</returns>
	Public Function Configure(ByVal directoryToListen As String, ByVal fileFilter As String, ByVal _FileListenerSettings As ReportFileListenerSettings,
														ByRef fileHandlingAction As Action(Of String)) As Boolean
		Try
			Me.FileListenerSetting = _FileListenerSettings
			' Create a new FileSystemWatcher and set its properties.
			Me.watcher = New FileSystemWatcher()
			Me.watcher.Path = directoryToListen
			Me.watcher.IncludeSubdirectories = True

			' Watch for changes in LastAccess and LastWrite times. 
			Me.watcher.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName)

			' Only watch files which match with the configured file filter.
			Me.watcher.Filter = fileFilter

			' Add event handler for creation of file.
			AddHandler Me.watcher.Created, AddressOf OnCreated

			' Begin watching.
			Me.watcher.EnableRaisingEvents = True

			' Install callback
			FileWatcher.fileHandlingAction = fileHandlingAction

		Catch ex As Exception
			' If an exception arises, the configuration could not be done correctly.
			' So return false.
			m_Logger.LogError(String.Format("{0}", ex.Message))

			' At this position, assume everything is ok.
			' So return true.
			Return False
		End Try

		Return True
	End Function

	' Define the event handlers.
	''' <summary>
	''' The event handling method if a file was created in the listening directory.
	''' </summary>
	''' <param name="source">The event source.</param>
	''' <param name="e">The event parameters.</param>
	Private Shared Sub OnCreated(ByVal source As Object, ByVal e As FileSystemEventArgs)

		FileWatcher.fileHandlingAction(e.FullPath)

	End Sub

	' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
	Public Sub Dispose() Implements IDisposable.Dispose

		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub

#End Region

#Region "Protected methods"

	' IDisposable
	Protected Overridable Sub Dispose(ByVal disposing As Boolean)
		If Not Me.disposedValue Then

			If Me.FileListenerSetting.bNotifyOnDispose Then
				Dim strBody As String = "Der Dienst wird abgeschaltet: {0}{1}{0}Verzeichnis: {2}"
				Dim strSubject As String = "Automatische Nachricht ({0}: {1})"
				strBody = String.Format(strBody, vbNewLine, My.Computer.Name, FileListenerSetting.Folder2Watch)
				strSubject = String.Format(strSubject, My.Application.Info.AssemblyName, My.Computer.Name)
				Try

					m_Logger.LogDebug(String.Format("Sendresult: sending mailnotification to admin: {0}", ""))

				Catch ex As Exception

				End Try

			End If

			If disposing Then
				' Dispose other state on managed objects.
			End If

			' Dispose unmanaged objects
			If Not Me.watcher Is Nothing Then
				Me.watcher.Dispose()
			End If

			' Set to null
			Me.watcher = Nothing

		End If
		Me.disposedValue = True
	End Sub

	Protected Overrides Sub Finalize()

		Dispose(False)
		MyBase.Finalize()
	End Sub
#End Region

End Class


Public Class ReportFileListenerSettings

	Public Property bNotifyOnScan As Boolean
	Public Property bNotifyOnDispose As Boolean
	Public Property Folder2Watch As String
	Public Property ConnStr4ScanDb As String
	Public Property Folder4ProcessedScannedDocuments As String
	Public Property Folder4TemporaryFiles As String

	Public Property SendNotificationTo As String
	Public Property SmtpServer As String
	Public Property SmtpPort As String

	Public Property WorkingForWebService As Boolean?

End Class
