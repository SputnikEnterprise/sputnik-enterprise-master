
Imports SPProgUtility

Public Class ClsLLESTemplateSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property bAsDesign As Boolean
  Public Property JobNr2Print As String
  Public Property SQL2Open As String
  Public Property USSignFileName As String

  ' Einzelne Datensätze...
  Public Property SelectedMonth As Byte
  Public Property SelectedYear As Short
  Public Property SelectedMANr2Print As Integer
  Public Property SelectedKDNr2Print As Integer
  Public Property SelectedKDZHDNr2Print As Integer
  Public Property SelectedESNr2Print As Integer
  Public Property SelectedESLohnNr2Print As Integer


	'Public ReadOnly Property GetESUnterzeichner() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'    Dim sValue As String = _ClsProgSetting.GetSpSESTempPath
	'    Dim strGuid As String = "SP.ES.PrintUtility"
	'    Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
	'    Dim strKeyName As String = "ESUnterzeichner_ESVertrag".ToLower
	'    Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

	'    Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
	'    sValue = strBez

	'    Return sValue
	'  End Get

	'End Property

	Public ReadOnly Property GetExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = _ClsProgSetting.GetSpSESTempPath
      Dim strGuid As String = "SP.ES.PrintUtility"
      Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
      Dim strKeyName As String = "ExportPfad".ToLower
      Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

      Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
      sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetExportFinalFilename(ByVal bVerleih As Boolean) As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = String.Format("FinalES{0}File.PDF", If(bVerleih, "Verleih", "Vertrag"))
      Dim strGuid As String = "SP.ES.PrintUtility"
      Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
      Dim strKeyName As String = String.Format("ExportFinalFileFilename_{0}", If(bVerleih, "Verleih", "ESVertrag")).ToLower
      Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

      Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
      sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetExportFilename(ByVal bVerleih As Boolean) As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "ES" & If(bVerleih, "Verleih", "Vertrag") & "_{0}_{1}.PDF"
      Dim strGuid As String = "SP.ES.PrintUtility"
      Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
      Dim strKeyName As String = String.Format("ExportFilename_{0}", If(bVerleih, "Verleih", "ESVertrag")).ToLower
      Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

      Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
      sValue = strBez

      Return sValue
    End Get

  End Property

End Class
