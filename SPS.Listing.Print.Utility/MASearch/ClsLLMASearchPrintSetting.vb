
Imports SPProgUtility

Public Class ClsLLMASearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property ShowAsDesign As Boolean

  Public Property MANr2Print As Integer
  Public Property ListSortBez As String
  Public Property USSignFileName As String
  Public Property ListFilterBez As List(Of String)

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

  Public Property SelectedLang As String
  Public Property AnzahlCopies As Integer
  Public Property liMANr2Print As List(Of Integer)
  Public Property ListOfExportedFilesMAStamm As New List(Of String)
  Public Property ShowMessageIFNotFounded As Boolean


  Public ReadOnly Property GetExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = _ClsProgSetting.GetSpSMATempPath
      Dim strGuid As String = "SP.MA.PrintUtility"
      Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
      Dim strKeyName As String = "ExportPfad".ToLower
      Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

      Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
      sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetExportFinalFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "FinalMAStammFile.PDF"
      Dim strGuid As String = "SP.MA.PrintUtility"
      Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
      Dim strKeyName As String = String.Format("ExportFinalFileFilename").ToLower
      Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

      Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
      sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetExportFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "MAStammblatt_{0}.PDF"
      Dim strGuid As String = "SP.MA.PrintUtility"
      Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
      Dim strKeyName As String = String.Format("ExportFilename").ToLower
      Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

      Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
      sValue = strBez

      Return sValue
    End Get

  End Property

End Class


Public Class ClsLLMATemplateSetting
	Public Property EmployeeNumbers2Print As List(Of Integer)
	Public Property CustomerNumbers2Print As List(Of Integer)
	Public Property EmploymentNumbers2Print As List(Of Integer)
	Public Property TemplateName As String
	Public Property TemplateJobNumber As String
	Public Property FrmHwnd As String
	Public Property ShowInDesign As Boolean
	Public Property CreateExportFile As Boolean
	Public Property ExportedFiles As List(Of String)

End Class
