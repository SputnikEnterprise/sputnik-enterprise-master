
Option Strict Off

Imports System.IO
Imports NLog

Module FileSystemWatcherProgram

  Dim Eventlog1 As New EventArgs
  Public SampleEventlog As EventLog

  Private Path2Watch As String
  Private WithSubDirectories As Boolean
  Private fsw As FileSystemWatcher
  Private mEvt As WaitForChangedResult

  Sub StartPathWatching()
    fsw = New FileSystemWatcher

    If Not EventLog.Exists("DebSmplg") Then
      EventLog.CreateEventSource("DebSmp", "DebSmplg")
    End If

    SampleEventlog = New EventLog
    SampleEventlog.Log = "DebSmplg"
    SampleEventlog.Source = "DebSmp"





		'    Console.Write("Verzeichnis : ")
		Path2Watch = "<your path>"  ' Console.ReadLine()"
		Console.Write("Unterverzeichnisse einschliessen (True/False): ")
    WithSubDirectories = True ' CType(Console.ReadLine(), Boolean)

    fsw.Path = Path2Watch
    fsw.IncludeSubdirectories = WithSubDirectories

    AddHandler fsw.Created, _
      New FileSystemEventHandler(AddressOf IsCreated)

    AddHandler fsw.Changed, _
      New FileSystemEventHandler(AddressOf IsChanged)

    AddHandler fsw.Deleted, _
      New FileSystemEventHandler(AddressOf IsDeleted)

    AddHandler fsw.Renamed, _
      New RenamedEventHandler(AddressOf IsRenamed)

    Console.WriteLine("Überwachung wird gestartet!")
    Console.WriteLine()

    While True
      mEvt = fsw.WaitForChanged(WatcherChangeTypes.All)
    End While

  End Sub

  Private Sub IsCreated(ByVal source As Object, _
    ByVal evt As FileSystemEventArgs)
    If WithSubDirectories Then
      SampleEventlog.WriteEntry("Update-Info: " & evt.FullPath + " wurde erstellt")
      Console.WriteLine(evt.FullPath + " wurde erstellt")
    Else
      SampleEventlog.WriteEntry("Update-Info: " & evt.Name + " wurde erstellt")
      Console.WriteLine(evt.Name + " wurde erstellt")
    End If
  End Sub

  Private Sub IsChanged(ByVal source As Object, _
    ByVal evt As FileSystemEventArgs)
    If WithSubDirectories Then
      SampleEventlog.WriteEntry("Update-Info: " & evt.FullPath + " wurde verändert")

      Console.WriteLine(evt.FullPath + " wurde verändert")
    Else
      SampleEventlog.WriteEntry("Update-Info: " & evt.Name + " wurde verändert")
      Console.WriteLine(evt.Name + " wurde verändert")
    End If
  End Sub

  Private Sub IsDeleted(ByVal source As Object, _
    ByVal evt As FileSystemEventArgs)
    If WithSubDirectories Then
      SampleEventlog.WriteEntry("Update-Info: " & evt.FullPath + " wurde gelöscht")
      Console.WriteLine(evt.FullPath + " wurde gelöscht")
    Else
      SampleEventlog.WriteEntry("Update-Info: " & evt.Name + " wurde gelöscht")
      Console.WriteLine(evt.Name + " wurde gelöscht")
    End If
  End Sub

  Private Sub IsRenamed(ByVal source As Object, _
    ByVal evt As RenamedEventArgs)
    If WithSubDirectories Then
      SampleEventlog.WriteEntry("Update-Info: " & evt.OldFullPath + " wurde in " + _
      evt.FullPath + " geändert")

      Console.WriteLine(evt.OldFullPath + " wurde in " + _
      evt.FullPath + " geändert")
    Else
      SampleEventlog.WriteEntry("Update-Info: " & evt.OldName + " wurde in " + _
evt.FullPath + " geändert")

      Console.WriteLine(evt.OldName + " wurde in " + _
      evt.Name + " geändert")
    End If
  End Sub

End Module

Module Utilities
	'Private logger As Logger = LogManager.GetCurrentClassLogger()

	Sub RunLVUpdate(ByVal strModulValue As String)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty
    Dim _clsProgsetting As New ClsProgSettingPath

    strTranslationProgName = _clsProgsetting.GetPersonalFolder() & "SPTranslationProg" & _clsProgsetting.GetLogedUSNr()

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("UpdateLVData", strModulValue)

    Catch ex As Exception
			'logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

  End Sub

End Module