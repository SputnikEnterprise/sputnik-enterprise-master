
Module FuncWS

  Dim _ClsEventLog As New SPProgUtility.ClsEventLog

  Function GetGruppe0inAllCantonData() As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGruppe0inAllCanton(strIDString).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe0inAllCantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetGAVTextData(ByVal kanton As String, ByVal gavGruppe0 As String, _
                           ByVal gavGruppe1 As String, ByVal gavGruppe2 As String, _
                           ByVal gavGruppe3 As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGAVText(strIDString, kanton, gavGruppe0, gavGruppe1, gavGruppe2, gavGruppe3).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGAVTextData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetGruppe0ByKantonData(ByVal kanton As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGruppe0ByKanton(strIDString, kanton).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe0ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetGruppe1ByKantonData(ByVal kanton As String, ByVal gavGruppe0 As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGruppe1ByKanton(strIDString, kanton, gavGruppe0).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe1ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetGruppe2ByKantonData(ByVal kanton As String, ByVal gavGruppe0 As String, _
                           ByVal gavGruppe1 As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGruppe2ByKanton(strIDString, kanton, gavGruppe0, gavGruppe1).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe2ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetGruppe3ByKantonData(ByVal kanton As String, ByVal gavGruppe0 As String, _
                           ByVal gavGruppe1 As String, ByVal gavGruppe2 As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGruppe3ByKanton(strIDString, kanton, gavGruppe0, gavGruppe1, gavGruppe2).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe3ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetGAVRecByIDData(ByVal kanton As String, ByVal gavGruppe0 As String, _
                          ByVal gavGruppe1 As String, ByVal gavGruppe2 As String, _
                          ByVal gavGruppe3 As String, ByVal gavText As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = Environment.UserDomainName & "; " & _
                                Environment.UserName & "; " & Environment.MachineName
    Dim _clsSystem As New ClsMain_Net
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = _clsSystem.GetUserID("2: ")
      Dim wsMyService As New GAVWebService.ClsGetGAVData
      liGAVValue = wsMyService.GetGAVRecByID(strIDString, kanton, gavGruppe0, gavGruppe1, gavGruppe2, gavGruppe3, gavText).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGAVRecByIDData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function
End Module
