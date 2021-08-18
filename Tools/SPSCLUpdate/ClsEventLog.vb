
'Imports System.Diagnostics
'Imports System.IO

'<CLSCompliant(True)> _
'Public Class ClsEventLog

'  Public Sub New()

'    'default constructor

'  End Sub

'  Public Sub WriteToErrorLog(ByVal msg As String, ByVal stkTrace As String, ByVal title As String)
'    Dim _ClsSystem As New ClsMain_Net
'    Dim strPathForErrors As String = _ClsSystem.GetSrvRootPath() & "Errors\"

'    Try
'      'check and make the directory if necessary; this is set to look in the application
'      'folder, you may wish to place the error log in another location depending upon the
'      'the user's role and write access to different areas of the file system
'      If Not System.IO.Directory.Exists(strPathForErrors) Then
'        System.IO.Directory.CreateDirectory(strPathForErrors)
'      End If

'      'check the file
'      Dim fs As FileStream = New FileStream(strPathForErrors & "\errlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
'      Dim s As StreamWriter = New StreamWriter(fs)
'      s.Close()
'      fs.Close()

'      'log it
'      Dim fs1 As FileStream = New FileStream(strPathForErrors & "\errlog.txt", FileMode.Append, FileAccess.Write)
'      Dim s1 As StreamWriter = New StreamWriter(fs1)
'      s1.Write("Title: " & title & vbCrLf)
'      s1.Write("Message: " & msg & vbCrLf)
'      s1.Write("StackTrace: " & stkTrace & vbCrLf)
'      s1.Write("Date/Time: " & DateTime.Now.ToString() & vbCrLf)
'      s1.Write("===========================================================================================" & vbCrLf)
'      s1.Close()
'      fs1.Close()

'    Catch ex As Exception
'      Me.WriteToEventLog("Fehler ist aufgetreten... " & ex.Message)

'    End Try

'  End Sub

'  Sub SaveTextToFile(ByVal strData As String, ByVal FullPath As String)
'    '    Dim Contents As String
'    Dim bAns As Boolean = False
'    Dim objReader As StreamWriter

'    Try
'      objReader = New StreamWriter(FullPath, False)
'      objReader.Write(strData)
'      objReader.Close()
'      bAns = True

'    Catch Ex As Exception
'      MsgBox(Ex.Message)

'    End Try

'  End Sub

'  Sub SendFileWithWebServer(ByVal iRunID As Integer, ByVal FullFilename As String)
'    Dim strMessage As String = String.Empty
'    Dim strStationID As String = Environment.UserDomainName & "; " & _
'                                Environment.UserName & "; " & Environment.MachineName
'    Dim _clsSystem As New ClsMain_Net

'    Try
'      Dim strIDString As String = _clsSystem.GetUserID("1: ")
'      Dim wsMyService As New SPUpdateService_1.Service1 ' spWebService_1.Service1SoapClient
'      If FullFilename <> String.Empty Then
'        Dim objFileStream As System.IO.FileStream       'Holds the Stream of the File
'        Dim oFileInfo As System.IO.FileInfo = New System.IO.FileInfo(FullFilename)
'        'Open the file and break it down into a file stream
'        objFileStream = System.IO.File.Open(FullFilename, IO.FileMode.Open, IO.FileAccess.Read)

'        'declare the byte array of the file
'        Dim objFileByte(CInt(objFileStream.Length - 1)) As Byte

'        'break the file into bytes and place into the byte object
'        objFileStream.Read(objFileByte, 0, CInt(objFileStream.Length))

'        objFileStream.Close()
'        If Not File.Exists(FullFilename) Then
'          Me.WriteToEventLog(Now.ToString & vbTab & "Datei wurde nicht gefunden..." & vbCrLf & FullFilename)
'        End If
'        Dim strFile As String = System.IO.Path.GetFileName(FullFilename)
'        wsMyService.UploadFile(strIDString, strStationID, strFile, objFileByte, strMessage)

'        If strMessage <> String.Empty Then MsgBox(strMessage.ToString, MsgBoxStyle.Exclamation, "SendFileWithWebServer")

'      End If


'    Catch ex As Exception
'      Me.WriteToEventLog(Now.ToString & vbTab & "Fehler bei der Kontrolle der LOG-Dateien..." & vbCrLf & _
'                         ex.Message)

'    End Try


'  End Sub

'  Public Function WriteToEventLog(ByVal entry As String, _
'                  Optional ByVal appName As String = "username", _
'                  Optional ByVal eventType As  _
'                  EventLogEntryType = EventLogEntryType.Information, _
'                  Optional ByVal logName As String = "SP_Update") As Boolean

'    Dim objEventLog As New EventLog

'    Try

'      'Try
'      '  EventLog.DeleteEventSource(logName)
'      '  EventLog.Delete(appName)

'      'Catch ex As Exception

'      'End Try

'      'Register the Application as an Event Source
'      If Not EventLog.SourceExists(appName) Then
'        EventLog.CreateEventSource(logName, appName)
'      End If

'      'log the entry
'      objEventLog.Log = appName
'      objEventLog.Source = logName
'      'objEventLog.Clear()

'      objEventLog.WriteEntry(entry)

'      Return True

'    Catch Ex As Exception

'      Return False

'    End Try

'  End Function

'End Class
