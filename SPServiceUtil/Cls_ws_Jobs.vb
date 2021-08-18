
'Imports NLog

Public Class Cls_ws_Jobs

	'Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

	Dim _ClsPropSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsEventLog As New SPProgUtility.ClsEventLog

  Function GetJobCHRegionList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetRegionsData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHRegionList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHRegionList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHLanguageList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetLanguageData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHLanguageList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHLanguageList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHLanguageNiveauList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetLanguageNiveauData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHLanguageList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHLanguageList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHBerufGruppeList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetBerufeData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHBerufGruppeList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHBerufGruppeList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHBerufFachbereichList(ByVal strLanguage As String, ByVal iID_Parent As Integer) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetBerufFachbereichData(strIDString, iID_Parent, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHBerufFachbereichList) Fehler bei der Kontrolle der Verbindung zum Server..." & _
                                   vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHBerufFachbereichList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHBerufFachPositionList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetPositionData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHBerufFachPositionList) Fehler bei der Kontrolle der Verbindung zum Server..." & _
                                   vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHBerufFachPositionList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHBranchenList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetBranchenData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHBranchenList) Fehler bei der Kontrolle der Verbindung zum Server..." & _
                                   vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHBranchenList")

    End Try

    Return dsWSValue
  End Function

  Function GetJobCHBildungsNiveauList(ByVal strLanguage As String) As DataSet
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim dsWSValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New _wsSP_JobsCH_Util.SP_JobsCH_Util
      dsWSValue = wsMyService.GetBildungData(strIDString, strLanguage)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetJobCHBildungsNiveauList) Fehler bei der Kontrolle der Verbindung zum Server..." & _
                                   vbCrLf & _
                         ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobCHBildungsNiveauList")

    End Try

    Return dsWSValue
  End Function


	'Function ListRegion(ByVal strLanguage As String) As DataSet
	'  Dim ds As DataSet = GetJobCHRegionList(strLanguage)

	'  Return ds
	'End Function

	Function ListLanguage(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = GetJobCHLanguageList(strLanguage)

    Return ds
  End Function

  Function ListLanguageNiveau(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = GetJobCHLanguageNiveauList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBerufGruppe(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = GetJobCHBerufGruppeList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBerufFachbereich(ByVal strLanguage As String, ByVal iID_Parent As Integer) As DataSet
    Dim ds As DataSet = GetJobCHBerufFachbereichList(strLanguage, iID_Parent)

    Return ds
  End Function

  Function ListJobCHBerufFachPosition(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = GetJobCHBerufFachPositionList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBranchen(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = GetJobCHBranchenList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBildungsNiveau(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = GetJobCHBildungsNiveauList(strLanguage)

    Return ds
  End Function

End Class
