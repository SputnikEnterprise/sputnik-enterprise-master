
Imports System.Data.SqlClient
Imports SPGAV.ClsDataDetail


Module funcGAV_PVL

	Private _ClsPropSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsEventLog As New SPProgUtility.ClsEventLog


	'Function GetPVLBerufe(ByVal strKanton As String, _
	'                      ByVal strPLZ As String) As List(Of String)
	'  Dim strMessage As String = String.Empty
	'  Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                             Environment.MachineName, _
	'                                             Environment.UserDomainName, _
	'                                             Environment.UserName, _
	'                                             _ClsPropSetting.GetSelectedMDData(1))
	'  Dim liGAVValue As New List(Of String)

	'  Try
	'	Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'    Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'	liGAVValue = wsMyService.GetGruppe0ByKanton(strIDString, strKanton, strPLZ, m_InitialData.UserData.UserLanguage).ToList


	'  Catch ex As Exception
	'	_ClsEventLog.WriteToEventLog(Now.ToString & vbTab & "(GetPVLBerufe) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & ex.ToString())

	'End Try

	'  Return liGAVValue
	'End Function

	Function GetGAVData4_FL(ByVal strKanton As String, ByVal strModulName As String, _
                          ByVal strGAVG0 As String, ByVal strGAVG1 As String, _
                          ByVal strGAVG2 As String, ByVal strGAVG3 As String, _
                          ByVal strGAVText As String) As List(Of String)
    Dim strMessage As String = String.Empty
    Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
                                               Environment.MachineName, _
                                               Environment.UserDomainName, _
                                               Environment.UserName, _
                                               _ClsPropSetting.GetSelectedMDData(1))
    'Dim liGAVValue As New List(Of String)
    Dim liGAVValue_1 As New List(Of String)

    Try
			Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
      'Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
      Dim wsMyService_1 As New GAVWebService.ClsGetGAVData
      'liGAVValue = wsMyService.GetGruppe0ByKanton(strIDString, strKanton, "").ToList
      If strModulName = "Gruppe0" Then
        liGAVValue_1 = wsMyService_1.GetGruppe0ByKanton(strIDString, strKanton).ToList

      ElseIf strModulName = "Gruppe1" Then
        liGAVValue_1 = wsMyService_1.GetGruppe1ByKanton(strIDString, strKanton, strGAVG0).ToList

      ElseIf strModulName = "Gruppe2" Then
        liGAVValue_1 = wsMyService_1.GetGruppe2ByKanton(strIDString, strKanton, strGAVG0, strGAVG1).ToList

      ElseIf strModulName = "Gruppe3" Then
        liGAVValue_1 = wsMyService_1.GetGruppe3ByKanton(strIDString, strKanton, _
                                                        strGAVG0, strGAVG1, strGAVG2).ToList

      ElseIf strModulName = "GAVText" Then
        liGAVValue_1 = wsMyService_1.GetGAVText(strIDString, strKanton, _
                                                        strGAVG0, strGAVG1, strGAVG2, strGAVG3).ToList

      ElseIf strModulName = "GAVResult" Then
        liGAVValue_1 = wsMyService_1.GetGAVRecByID(strIDString, strKanton, _
                                                        strGAVG0, strGAVG1, strGAVG2, strGAVG3, strGAVText).ToList
      End If


    Catch ex As Exception
      liGAVValue_1.Add(ex.Message)
      _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
                                   "(GetGAVData4_FL) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
                         ex.Message)

    End Try

    Return liGAVValue_1
  End Function

	'Function GetPVLBerufe_DS(ByVal strKanton As String, _
	'                    ByVal strPLZ As String) As DataSet
	'  Dim strMessage As String = String.Empty
	'  Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                             Environment.MachineName, _
	'                                             Environment.UserDomainName, _
	'                                             Environment.UserName, _
	'                                             _ClsPropSetting.GetSelectedMDData(1))
	'  Dim liGAVValue As New DataSet

	'  Try
	'	Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'    Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'	liGAVValue = wsMyService.GetGruppe0ByKanton_DS(strIDString, strKanton, strPLZ, m_InitialData.UserData.UserLanguage)


	'  Catch ex As Exception
	'	MsgBox(String.Format("Fehler: {0}{1}{2}", ex.Message, vbNewLine, ex.InnerException), _
	'				 MsgBoxStyle.Critical, "GetPVLBerufe_DS")
	'    _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                 "(GetPVLBerufe_DS) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                       ex.Message)

	'  End Try

	'  Return liGAVValue
	'End Function

	'Function GetPVLAnhang1Berufe() As List(Of String)
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim liGAVValue As New List(Of String)

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'     liGAVValue = wsMyService.GetPVLAnhang1Berufe(strIDString).ToList


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetPVLAnhang1Berufe) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return liGAVValue
	' End Function

	' Function GetPVLWarning(ByVal iGAVNr As Integer) As String
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim strResult As String = String.Empty

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'     strResult = wsMyService.GetPVLBerufWarning(iGAVNr)


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetPVLWarning) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return strResult
	' End Function

	'Function GetPVLCategoryNames(ByVal iMetaNr As Integer) As List(Of String)
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim liGAVValue As New List(Of String)

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'		liGAVValue = wsMyService.GetGAVCategoryNames(strIDString, iMetaNr, m_InitialData.UserData.UserLanguage).ToList


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetPVLCategoryNames) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return liGAVValue
	' End Function

	' Function GetPVLCategoryValues(ByVal iCategoryNr As Integer, _
	'                               ByVal iBaseCategoryValueNr As Integer, _
	'                               ByVal bWithBaseNr As Boolean) As List(Of String)
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim liGAVValue As New List(Of String)

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'     If Not bWithBaseNr Then
	'			liGAVValue = wsMyService.GetGAVCategoryValuesWithLanguage(strIDString, iCategoryNr, m_InitialData.UserData.UserLanguage).ToList

	'     Else
	'			liGAVValue = wsMyService.GetGAVCategoryValuesWithBaseValue(strIDString, iCategoryNr, iBaseCategoryValueNr, m_InitialData.UserData.UserLanguage).ToList

	'		End If


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetPVLCategoryValues) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return liGAVValue
	' End Function

	' Function GetPVLLODataWithCategoryValues(ByVal strCategoryValues As String) As List(Of String)
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim liGAVValue As New List(Of String)

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'     liGAVValue = wsMyService.GetGAVCalculationValue(strIDString, strCategoryValues).ToList


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetPVLLODataWithCategoryValues) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return liGAVValue
	' End Function

	' Function GetPVLCeriterion(ByVal iMetaNr As Integer) As List(Of String)
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim liGAVValue As New List(Of String)

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'		liGAVValue = wsMyService.GetGAVCriterionValue(strIDString, iMetaNr, m_InitialData.UserData.UserLanguage).ToList


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetPVLLODataWithCategoryValues) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return liGAVValue
	' End Function

	' Function GetPVLCeriterionValue(ByVal iCriterionNr As Integer) As List(Of String)
	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))
	'   Dim liGAVValue As New List(Of String)

	'   Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", m_InitialData.UserData.UserMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'		liGAVValue = wsMyService.GetGAVCriterionValueByIDCriterion(strIDString, iCriterionNr, _
	'																															 m_InitialData.UserData.UserLanguage).ToList


	'   Catch ex As Exception
	'     _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetGAVCriterionValueByIDCriterion) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message)

	'   End Try

	'   Return liGAVValue
	' End Function


End Module

