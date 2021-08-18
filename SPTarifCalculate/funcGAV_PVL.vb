
Imports System.Data.SqlClient

Module funcGAV_PVL

  Dim _ClsPropSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsEventLog As New SPProgUtility.ClsEventLog

  Function GetPVLBerufe(ByVal strKanton As String, _
                        ByVal strPLZ As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      liGAVValue = wsMyService.GetGruppe0ByKanton(strIDString, strKanton, strPLZ, _ClsPropSetting.GetUSLanguage).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe0ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetPVLBerufe_DS(ByVal strKanton As String, _
                      ByVal strPLZ As String) As DataSet
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      liGAVValue = wsMyService.GetGruppe0ByKanton_DS(strIDString, strKanton, strPLZ, _ClsPropSetting.GetUSLanguage)


    Catch ex As Exception
      MsgBox(String.Format("Fehler: {0}{1}{2}", ex.Message, vbNewLine, ex.InnerException), _
             MsgBoxStyle.Critical, "GetPVLBerufe_DS")
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe0ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetPVLAnhang1Berufe() As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      liGAVValue = wsMyService.GetPVLAnhang1Berufe(strIDString).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe0ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetPVLWarning(ByVal iGAVNr As Integer) As String
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim strResult As String = String.Empty

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      strResult = wsMyService.GetPVLBerufWarning(iGAVNr)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGruppe0ByKantonData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return strResult
  End Function

  Function GetPVLCategoryNames_DS(ByVal iMetaNr As Integer) As DataSet
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New DataSet

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      'liGAVValue = wsMyService.GetGAVCategoryNames_ds(strIDString, iMetaNr)


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetPVLCategoryNames) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function


  Function GetPVLCategoryNames(ByVal iMetaNr As Integer) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      liGAVValue = wsMyService.GetGAVCategoryNames(strIDString, iMetaNr, _ClsPropSetting.GetUSLanguage).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetPVLCategoryNames) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetPVLCategoryValues(ByVal iCategoryNr As Integer, _
                                ByVal iBaseCategoryValueNr As Integer, _
                                ByVal bWithBaseNr As Boolean) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      If Not bWithBaseNr Then
        liGAVValue = wsMyService.GetGAVCategoryValues(strIDString, iCategoryNr).ToList

      Else
        liGAVValue = wsMyService.GetGAVCategoryValuesWithBaseValue(strIDString, iCategoryNr, iBaseCategoryValueNr, _ClsPropSetting.GetUSLanguage).ToList

      End If


    Catch ex As Exception
			_ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
																	 "(GetPVLCategoryValues) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & ex.ToString)

    End Try

    Return liGAVValue
  End Function

  Function GetPVLLODataWithCategoryValues(ByVal strCategoryValues As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      liGAVValue = wsMyService.GetGAVCalculationValue(strIDString, strCategoryValues).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetPVLLODataWithCategoryValues) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function

  Function GetPVLCeriterion(ByVal iMetaNr As Integer) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    Dim liGAVValue As New List(Of String)

    Try
      Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
      Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      liGAVValue = wsMyService.GetGAVCriterionValue(strIDString, iMetaNr, ClsDataDetail.SelectedLanguage).ToList


    Catch ex As Exception
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetPVLLODataWithCategoryValues) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue
  End Function


End Module

