

Public Class ClsWSFunctions

  Dim _ClsPropSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsEventLog As New SPProgUtility.ClsEventLog
  Dim _ClsService As New SPServiceUtil.Cls_ws_Jobs

	Function ListLanguage(ByVal strLanguage As String) As DataSet
		Dim ds As DataSet = _ClsService.GetJobCHLanguageList(strLanguage)

		Return ds
	End Function

	Function ListLanguageNiveau(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = _ClsService.GetJobCHLanguageNiveauList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBerufGruppe(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = _ClsService.GetJobCHBerufGruppeList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBerufFachbereich(ByVal strLanguage As String, ByVal iID_Parent As Integer) As DataSet
    Dim ds As DataSet = _ClsService.GetJobCHBerufFachbereichList(strLanguage, iID_Parent)

    Return ds
  End Function

  Function ListJobCHBerufFachPosition(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = _ClsService.GetJobCHBerufFachPositionList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBranchen(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = _ClsService.GetJobCHBranchenList(strLanguage)

    Return ds
  End Function

  Function ListJobCHBildungsNiveau(ByVal strLanguage As String) As DataSet
    Dim ds As DataSet = _ClsService.GetJobCHBildungsNiveauList(strLanguage)

    Return ds
  End Function


End Class
