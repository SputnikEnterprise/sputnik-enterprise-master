
Imports SPProgUtility

Public Class ClsRPCSetting

  Public Property DbConnString2Open As String
  Public Property SelectedQuery2Open As String

  Public Property SelectedYear As New List(Of Integer)
  Public Property SelectedMonth As New List(Of Short)

  Public Property FoundedRPNr As New List(Of Integer)
  Public Property ShowMessage As Boolean
  Public Property SelectedRPNr As New List(Of Integer)

  Public Property SelectedKanton As String
  Public Property SelectedPVLBez As String

  Public Property MetroForeColor As System.Drawing.Color
  Public Property MetroBorderColor As System.Drawing.Color
  Public Property PrintCreatedPDFFile As Boolean

  Public Property PrintFerFeiertage As Boolean
  Public Property lv2Fill As DevComponents.DotNetBar.Controls.ListViewEx


  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)


	'Public ReadOnly Property GetExportPfad() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'    Dim sValue As String = _ClsProgSetting.GetSpSRPTempPath
	'    Dim strGuid As String = "SP.RPContent.PrintUtility"
	'    Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
	'    Dim strKeyName As String = "ExportPfad".ToLower
	'    Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

	'    Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
	'    sValue = strBez

	'    Return sValue
	'  End Get

	'End Property

	'Public ReadOnly Property GetExportFinalFilename() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'    Dim sValue As String = "FinalRPCFile.PDF"
	'    Dim strGuid As String = "SP.RPContent.PrintUtility"
	'    Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
	'    Dim strKeyName As String = "ExportFinalFileFilename".ToLower
	'    Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

	'    Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
	'    sValue = strBez

	'    Return sValue
	'  End Get

	'End Property

	'Public ReadOnly Property GetExportFilename() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'    Dim sValue As String = "RPCFileName_{0}.PDF"
	'    Dim strGuid As String = "SP.RPContent.PrintUtility"
	'    Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
	'    Dim strKeyName As String = "ExportFilename".ToLower
	'    Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

	'    Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
	'    sValue = strBez

	'    Return sValue
	'  End Get

	'End Property

End Class


Public Class MandantenData

  Public Property MDNr As Integer
  Public Property MDName As String
  Public Property MDGuid As String
  Public Property MDConnStr As String

End Class


Public Class FoundedRPData

  Public Property RPNr As Integer

  Public Property employeename As String
  Public Property customername As String
  Public Property gavberuf As String

  Public Property es_als As String
  Public Property rpperiode As String
  

End Class
