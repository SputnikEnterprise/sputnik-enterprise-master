
Imports System.Reflection.Assembly
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports NLog
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings


<CLSCompliant(True)>
Public Class ClsEventLog

	Private Shared logger As Logger = LogManager.GetCurrentClassLogger()
	'Dim _ClsSettingPath As New ClsProgSettingPath

	Private m_Progpath As ClsProgPath
	Private m_common As New CommonSetting

	Public Sub New()

		'default constructor

	End Sub

	Public Sub WriteToLogFile(ByVal msg As String,
								 ByVal strLibrary As String,
								 ByVal strFunctionName As String,
								 ByVal bWriteTime As Boolean,
								 ByVal strFullFilename As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strPathForLOGs As String = String.Empty
		Dim objAssInfo As New ClsAssInfo()
		m_Progpath = New ClsProgPath

		Dim strMyFilename As String = If(String.IsNullOrEmpty(strFullFilename), m_Progpath.GetFileServerLogFilename, strFullFilename)

		Try
			'check and make the directory if necessary; this is set to look in the application
			'folder, you may wish to place the error log in another location depending upon the
			'the user's role and write access to different areas of the file system
			'If Not System.IO.Directory.Exists(strMyFilename) Then
			'  System.IO.Directory.CreateDirectory(strMyFilename)
			'End If

			'check the file
			Dim fs As FileStream = New FileStream(strMyFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite)
			Dim s As StreamWriter = New StreamWriter(fs)
			s.Close()
			fs.Close()

			'log it
			Dim fs1 As FileStream = New FileStream(strMyFilename, FileMode.Append, FileAccess.Write)
			Dim s1 As StreamWriter = New StreamWriter(fs1)
			If msg.Trim.Length > 5 Then
				s1.Write(String.Format("{0} ", DateTimeOffset.Now.ToString("dd.MM.yyyy hh:mm:ss.ffff")))
				s1.Write(String.Format("{0}::{1}{2}{3}{4}", strLibrary, strFunctionName, If(String.IsNullOrWhiteSpace(strFunctionName), "", " -> "), msg, vbNewLine))
			End If

			s1.Close()
			fs1.Close()

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.WriteToEventLog("(WriteToLogFile) Fehler ist aufgetreten... " & ex.Message)

		End Try

	End Sub

	Public Sub WriteErrorLogFile(ByVal msg As String, Optional ByVal strFullFilename As String = "")
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_Progpath = New ClsProgPath

		If strFullFilename = String.Empty Then strFullFilename = m_Progpath.GetFileServerErrorFilename

		Try
			Me.WriteToLogFile(msg, String.Empty, "Error", True, strFullFilename)

		Catch ex As Exception
			Me.WriteToEventLog("(WriteErrorLogFile) Fehler ist aufgetreten... " & ex.Message)

		End Try

	End Sub

	Public Overloads Sub WriteTempLogFile(ByVal msg As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_Progpath = New ClsProgPath

		Dim strFullFilename As String = m_Progpath.GetFileServerLogFilename

		Try
			Me.WriteToLogFile(msg, String.Empty, String.Empty, True, strFullFilename)

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.WriteToEventLog("(WriteTempLogFile) Fehler ist aufgetreten... " & ex.Message)

		End Try

	End Sub

	Public Overloads Sub WriteTempLogFile(ByVal msg As String, ByVal strFullFilename As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_Progpath = New ClsProgPath

		If String.IsNullOrWhiteSpace(strFullFilename) Then strFullFilename = m_Progpath.GetFileServerLogFilename

		Try
			Me.WriteToLogFile(msg, String.Empty, String.Empty, True, strFullFilename)

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.WriteToEventLog("(WriteTempLogFile) Fehler ist aufgetreten... " & ex.Message)

		End Try

	End Sub

	Public Overloads Sub WriteTempLogFile(ByVal msg As String, ByVal strFullFilename As String, ByVal strFunctionName As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_Progpath = New ClsProgPath

		If String.IsNullOrWhiteSpace(strFullFilename) Then strFullFilename = m_Progpath.GetFileServerLogFilename

		Try
			Me.WriteToLogFile(msg, String.Empty, "Temporarylogging", True, strFullFilename)

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.WriteToEventLog("(WriteTempLogFile) Fehler ist aufgetreten... " & ex.Message)

		End Try

	End Sub

	Sub WriteMainLog(ByVal strFuncName As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_common = New CommonSetting
		m_Progpath = New ClsProgPath
		Dim objAssInfo As New ClsAssInfo()
		Dim Filename As String = m_Progpath.GetFileServerLogFilename
		Dim strContent As String = String.Format("({0}){2}{1}{2}{3}\{2}{4}{2}{2}{5}{2}({6})",
												 m_common.GetLogedUserName, String.Empty, vbTab,
												 My.Application.Info.DirectoryPath, objAssInfo.Product,
												 strFuncName,
												 My.Application.Info.Version.Major & "." &
												 My.Application.Info.Version.MajorRevision & "." &
												 My.Application.Info.Version.Minor & "." &
												 My.Application.Info.Version.MinorRevision)

		Try
			Me.WriteToLogFile(strContent, String.Empty, "Mainlogging", True, Filename)

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.WriteToEventLog("(WriteMainLog) Fehler ist aufgetreten... " & ex.Message)

		End Try

	End Sub

	Public Function WriteToEventLog(ByVal entry As String,
					  Optional ByVal appName As String = "Sputnik Enterprise Suite",
					  Optional ByVal eventType As _
					  EventLogEntryType = EventLogEntryType.Information,
					  Optional ByVal logName As String = "SP_Update") As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim objEventLog As New EventLog

		Try

			'Register the Application as an Event Source
			If Not EventLog.SourceExists(appName) Then
				EventLog.CreateEventSource(logName, appName)
			End If

			'log the entry
			objEventLog.Log = appName
			objEventLog.Source = logName
			'objEventLog.Clear()

			objEventLog.WriteEntry(entry)

			Return True

		Catch Ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, Ex.Message))
			Return False

		End Try

	End Function

End Class
