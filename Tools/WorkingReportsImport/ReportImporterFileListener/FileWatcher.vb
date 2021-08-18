'------------------------------------
' File: FileWatcher.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

' Imports System.Security.Permissions
Imports System.IO
Imports ReportImporterCommon
Imports ReportImporterCommon.Logging
Imports ReportImporterCommon.Notification


Public Class FileWatcher
	Implements IDisposable

	Private Shared m_logger As ILogger = New Logger()


#Region "Private fields"
	''' <summary>
	''' The filewatcher itself.
	''' </summary>
	''' <remarks>This class hosts the needed event we can listen on for file changes.</remarks>
	Private watcher As FileSystemWatcher

	''' <summary>
	''' Callback to handle the processing of a 
	''' </summary>
	Private Shared fileHandlingAction As Action(Of String)


	''' <summary>
	''' Stores whether the object has been disposed.
	''' </summary>
	''' <remarks></remarks>
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
			m_logger.LogError(String.Format("{0}", ex.ToString))

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
		' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub

#End Region

#Region "Protected methods"

	' IDisposable
	Protected Overridable Sub Dispose(ByVal disposing As Boolean)
		If Not Me.disposedValue Then
			' Send Message to Sputnik-Administrator
			If Me.FileListenerSetting.bNotifyOnDispose Then
				Dim strBody As String = "Der Dienst wird abgeschaltet: {0}{1}{0}Verzeichnis: {2}"
				Dim strSubject As String = "Automatische Nachricht ({0}: {1})"
				strBody = String.Format(strBody, vbNewLine, My.Computer.Name, FileListenerSetting.Folder2Watch)
				strSubject = String.Format(strSubject, My.Application.Info.AssemblyName, My.Computer.Name)
				Try

					Dim m_NotifyUtility As New Notification.Notifying
					m_NotifyUtility.SettingFileData = FileListenerSetting.SettingFileValue

					Dim result = m_NotifyUtility.SendMailToWithExchange("info@sputnik-it.com", FileListenerSetting.SettingFileValue.NotifyEMailToScanJob, "", strSubject, strBody, Nothing)

					m_logger.LogDebug(String.Format("Sendresult: sending mailnotification to admin: {0}", result))

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
		' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
		Dispose(False)
		MyBase.Finalize()
	End Sub
#End Region

End Class
