

Module FuncWS
  Dim _ClsEventLog As New SPProgUtility.ClsEventLog


#Region "Alte GAV-Funktionen..."

  'Function GetAllGAV_GroupData(ByVal iRunID As Integer, ByVal strGAVKanton As String) As List(Of String)
  '  Dim strMessage As String = String.Empty
  '  Dim strStationID As String = Environment.UserDomainName & "; " & _
  '                              Environment.UserName & "; " & Environment.MachineName
  '  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  '  Dim liGAVValue As New List(Of String)

  '  Try
  '    Dim strIDString As String = _ClsProgSetting.GetUserID("2: ")
  '    Dim wsMyService As New GAVServices_1.ClsGetGAVData
  '    liGAVValue = wsMyService.GetGruppe0ByKanton(strIDString, strGAVKanton).ToList


  '  Catch ex As Exception
  '    Dim _ClsEventLog As New ClsEventLog
  '    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & "(GetGruppe0ByKanton) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
  '                       ex.Message)

  '  End Try

  '  Return liGAVValue
  'End Function

  'Function GetAllGAV_GroupData(ByVal iRunID As Integer, _
  '                           ByVal strGAVKanton As String, _
  '                           ByVal strGAVGruppe0 As String) As List(Of String)
  '  Dim strMessage As String = String.Empty
  '  Dim strStationID As String = Environment.UserDomainName & "; " & _
  '                              Environment.UserName & "; " & Environment.MachineName
  '  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  '  Dim liGAVValue As New List(Of String)

  '  Try
  '    Dim strIDString As String = _ClsProgSetting.GetUserID("2: ")
  '    Dim wsMyService As New GAVServices_1.ClsGetGAVData
  '    liGAVValue = wsMyService.GetGruppe1ByKanton(strIDString, strGAVKanton, strGAVGruppe0.Replace("'", "''")).ToList


  '  Catch ex As Exception
  '    Dim _ClsEventLog As New ClsEventLog
  '    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & "(GetGruppe1ByKanton) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
  '                       ex.Message)

  '  End Try

  '  Return liGAVValue
  'End Function

  'Function GetAllGAV_GroupData(ByVal iRunID As Integer, _
  '                         ByVal strGAVKanton As String, _
  '                         ByVal strGAVGruppe0 As String, _
  '                         ByVal strGAVGruppe1 As String) As List(Of String)
  '  Dim strMessage As String = String.Empty
  '  Dim strStationID As String = Environment.UserDomainName & "; " & _
  '                              Environment.UserName & "; " & Environment.MachineName
  '  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  '  Dim liGAVValue As New List(Of String)

  '  Try
  '    Dim strIDString As String = _ClsProgSetting.GetUserID("2: ")
  '    Dim wsMyService As New GAVServices_1.ClsGetGAVData
  '    liGAVValue = wsMyService.GetGruppe2ByKanton(strIDString, strGAVKanton, strGAVGruppe0.Replace("'", "''"), _
  '                                                strGAVGruppe1.Replace("'", "''")).ToList


  '  Catch ex As Exception
  '    Dim _ClsEventLog As New ClsEventLog
  '    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & "(GetGruppe2ByKanton) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
  '                       ex.Message)

  '  End Try

  '  Return liGAVValue
  'End Function

  'Function GetAllGAV_GroupData(ByVal iRunID As Integer, _
  '                       ByVal strGAVKanton As String, _
  '                       ByVal strGAVGruppe0 As String, _
  '                       ByVal strGAVGruppe1 As String, _
  '                       ByVal strGAVGruppe2 As String) As List(Of String)
  '  Dim strMessage As String = String.Empty
  '  Dim strStationID As String = Environment.UserDomainName & "; " & _
  '                              Environment.UserName & "; " & Environment.MachineName
  '  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  '  Dim liGAVValue As New List(Of String)

  '  Try
  '    Dim strIDString As String = _ClsProgSetting.GetUserID("2: ")
  '    Dim wsMyService As New GAVServices_1.ClsGetGAVData
  '    liGAVValue = wsMyService.GetGruppe3ByKanton(strIDString, strGAVKanton, strGAVGruppe0.Replace("'", "''"), _
  '                                                strGAVGruppe1.Replace("'", "''"), strGAVGruppe2.Replace("'", "''")).ToList


  '  Catch ex As Exception
  '    Dim _ClsEventLog As New ClsEventLog
  '    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & "(GetGruppe3ByKanton) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
  '                       ex.Message)

  '  End Try

  '  Return liGAVValue
  'End Function

  'Function GetAllGAV_GroupData(ByVal iRunID As Integer, _
  '                       ByVal strGAVKanton As String, _
  '                       ByVal strGAVGruppe0 As String, _
  '                       ByVal strGAVGruppe1 As String, _
  '                       ByVal strGAVGruppe2 As String, _
  '                       ByVal strGAVGruppe3 As String) As List(Of String)
  '  Dim strMessage As String = String.Empty
  '  Dim strStationID As String = Environment.UserDomainName & "; " & _
  '                              Environment.UserName & "; " & Environment.MachineName
  '  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  '  Dim liGAVValue As New List(Of String)

  '  Try
  '    Dim strIDString As String = _ClsProgSetting.GetUserID("2: ")
  '    Dim wsMyService As New GAVServices_1.ClsGetGAVData
  '    liGAVValue = wsMyService.GetGAVText(strIDString, strGAVKanton, strGAVGruppe0.Replace("'", "''"), _
  '                                        strGAVGruppe1.Replace("'", "''"), strGAVGruppe2.Replace("'", "''"), _
  '                                        strGAVGruppe3.Replace("'", "''")).ToList


  '  Catch ex As Exception
  '    Dim _ClsEventLog As New ClsEventLog
  '    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & "(GetGAVText) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
  '                       ex.Message)

  '  End Try

  '  Return liGAVValue
  'End Function

  'Function GetAllGAV_GroupData(ByVal iRunID As Integer, _
  '                     ByVal strGAVKanton As String, _
  '                     ByVal strGAVGruppe0 As String, _
  '                     ByVal strGAVGruppe1 As String, _
  '                     ByVal strGAVGruppe2 As String, _
  '                     ByVal strGAVGruppe3 As String, _
  '                     ByVal strGAVText As String) As List(Of String)
  '  Dim strMessage As String = String.Empty
  '  Dim strStationID As String = Environment.UserDomainName & "; " & _
  '                              Environment.UserName & "; " & Environment.MachineName
  '  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  '  Dim liGAVValue As New List(Of String)

  '  Try
  '    Dim strIDString As String = _ClsProgSetting.GetUserID("2: ")
  '    Dim wsMyService As New GAVServices_1.ClsGetGAVData
  '    liGAVValue = wsMyService.GetGAVRecByID(strIDString, strGAVKanton, strGAVGruppe0.Replace("'", "''"), _
  '                                           strGAVGruppe1.Replace("'", "''"), strGAVGruppe2.Replace("'", "''"), _
  '                                           strGAVGruppe3.Replace("'", "''"), strGAVText.Replace("'", "''")).ToList


  '  Catch ex As Exception
  '    Dim _ClsEventLog As New ClsEventLog
  '    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
  '                                 "(GetGAVRecByID) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
  '                                 ex.Message)

  '  End Try

  '  Return liGAVValue
  'End Function

#End Region


End Module
