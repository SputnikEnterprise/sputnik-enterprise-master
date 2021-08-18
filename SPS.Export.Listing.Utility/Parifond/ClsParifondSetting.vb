
Imports SPProgUtility

Public Class ClsParifondSetting

  Public Property DbConnString2Open As String
  Public Property SQL2Open As String
  Public Property ExportTablename As String

  Public Property FirstMonth As Integer?
  Public Property FirstYear As Integer?
  Public Property LastMonth As Integer?
  Public Property LastYear As Integer?


  Public Property MergePDFFiles As Boolean

  Public Property SelectedPVL As List(Of String)
  Public Property SelectedPVLKanton As String
  Public Property SelectedVonDate As Date
  Public Property SelectedBisDate As Date

  Public Property liESNr2Print As New List(Of Integer)
  Public Property liMANr2Print As New List(Of Integer)
  Public Property liKDNr2Print As New List(Of Integer)
  Public Property liKDZHDNr2Print As New List(Of Integer)

  Public Property liRPNr2Print As New List(Of Integer)
  Public Property liLONr2Print As New List(Of Integer)


  Public Property SelectedData2WOS As New List(Of Boolean)
  Public Property SelectedMALang As New List(Of String)
  Public Property SelectedKDLang As New List(Of String)

  Public Property SelectedMAData2WOS As New List(Of Boolean)
  Public Property SelectedKDData2WOS As New List(Of Boolean)

  Public Property liCreatedFinalFile As New List(Of String)
  Public Property liCreatedError As New List(Of String)


  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

	Public ReadOnly Property SelectedPeriodeTime As String
		Get
			Return String.Format("{0:d}-{1:d}", SelectedVonDate, SelectedBisDate)
		End Get
	End Property


#Region "Lo-Pfad..."

	Public ReadOnly Property GetLOExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sValue As String = _ClsProgSetting.GetSpSLOTempPath

			'Dim strGuid As String = "SP.LO.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportPfad".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetLOExportFinalFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "FinalFile.PDF"
			'Dim strGuid As String = "SP.LO.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportFinalFileFilename".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetLOExportFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "LO_{0}_{1}.PDF"
			'Dim strGuid As String = "SP.LO.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportFilename".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property


#End Region


#Region "Rapport-Pfad..."

  Public ReadOnly Property GetRPExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = _ClsProgSetting.GetSpSRPTempPath
			'Dim strGuid As String = "SP.RPContent.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportPfad".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetRPExportFinalFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "FinalRPCFile.PDF"
			'Dim strGuid As String = "SP.RPContent.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportFinalFileFilename".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetRPExportFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "RPCFileName_{0}.PDF"
			'Dim strGuid As String = "SP.RPContent.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportFilename".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

#End Region


#Region "Einsatz-Pfad..."

  Public ReadOnly Property GetESExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = _ClsProgSetting.GetSpSESTempPath
			'Dim strGuid As String = "SP.ES.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportPfad".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetESExportFinalFilename(ByVal bVerleih As Boolean) As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = String.Format("FinalES{0}File.PDF", If(bVerleih, "Verleih", "Vertrag"))
			'Dim strGuid As String = "SP.ES.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = String.Format("ExportFinalFileFilename_{0}", If(bVerleih, "Verleih", "ESVertrag")).ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetESExportFilename(ByVal bVerleih As Boolean) As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "ES" & If(bVerleih, "Verleih", "Vertrag") & "_{0}_{1}.PDF"
			'Dim strGuid As String = "SP.ES.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = String.Format("ExportFilename_{0}", If(bVerleih, "Verleih", "ESVertrag")).ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

#End Region


#Region "Kunden-Pfad..."

  Public ReadOnly Property GetKDExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = _ClsProgSetting.GetSpSKDTempPath
			'Dim strGuid As String = "SP.KD.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportPfad".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetKDExportFinalFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "FinalKDStammFile.PDF"
			'Dim strGuid As String = "SP.KD.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = String.Format("ExportFinalFileFilename").ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetKDExportFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "KDStammblatt_{0}.PDF"
			'Dim strGuid As String = "SP.KD.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = String.Format("ExportFilename").ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

#End Region


#Region "Kandidaten-Pfad..."

  Public ReadOnly Property GetMAExportPfad() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = _ClsProgSetting.GetSpSMATempPath
			'Dim strGuid As String = "SP.MA.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportPfad".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetMAExportFinalFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "FinalMAStammFile.PDF"
			'Dim strGuid As String = "SP.MA.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = String.Format("ExportFinalFileFilename").ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property

  Public ReadOnly Property GetMAExportFilename() As String
    Get
      Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
      Dim sValue As String = "MAStammblatt_{0}.PDF"
			'Dim strGuid As String = "SP.MA.PrintUtility"
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = String.Format("ExportFilename").ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			'sValue = strBez

      Return sValue
    End Get

  End Property


#End Region

End Class


Public Class FoundedRPData

  Public Property RPNr As Integer
  Public Property MANr As Integer
  Public Property KDNr As Integer
  Public Property ESNr As Integer

  Public Property employeename As String
  Public Property customername As String

  Public Property monthyear As String
  Public Property esperiode As String
  Public Property es_als As String
  Public Property rpperiode As String
  Public Property weeknumbers As String


End Class
