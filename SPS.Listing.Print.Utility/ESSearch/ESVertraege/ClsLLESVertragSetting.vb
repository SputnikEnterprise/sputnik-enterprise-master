
Imports SPProgUtility
Imports SPProgUtility.Mandanten

Public Class ClsLLESVertragSetting

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

	Public Property ExportFilename As String
	Public Property ExportFinalFilename As String
	Public Property ExportPath As String

	Public Property AnzahlCopies As Short

	Public Property ListOfExportedFilesESVertrag As New List(Of String)
	Public Property ListOfExportedFilesVerleih As New List(Of String)

	' Mehrere Datensätze...
	Public Property liESNr2Print As New List(Of Integer)
	Public Property LiESLohnNr2Print As New List(Of Integer)

	Public Property liMANr2Print As New List(Of Integer)
	Public Property liKDNr2Print As New List(Of Integer)
	Public Property liKDZHDNr2Print As New List(Of Integer)

	Public Property LiMALang As New List(Of String)
	Public Property LiKDLang As New List(Of String)

	Public Property liESSend2WOS As New List(Of Boolean)

	Public Property liSendESMAData2WOS As New List(Of Boolean)
	Public Property liSendESKDData2WOS As New List(Of Boolean)

	' Einzelne Datensätze...
	Public Property SelectedMonth As Byte
	Public Property SelectedYear As Short
	Public Property SelectedMANr2Print As Integer
	Public Property SelectedKDNr2Print As Integer
	Public Property SelectedKDZHDNr2Print As Integer
	Public Property SelectedESNr2Print As Integer
	Public Property SelectedESLohnNr2Print As Integer
	Public Property SelectedMALang As String
	Public Property SelectedKDLang As String
	Public Property SelectedLang As String

	Public Property ESCreatedOn As Date
	Public Property ESCreatedFrom As String

	Public Property Is4Export As Boolean              ' Exportiert nur in PDF
	Public Property SendMAData2WOS As Boolean           ' Sendet einzelne Daten zum WOS
	Public Property SendKDData2WOS As Boolean           ' Sendet einzelne Daten zum WOS
	Public Property SendAndPrintData2WOS As Boolean   ' Sendet und Druckt die Daten zum WOS

	Public Property IsPrintAsVerleih As Boolean       ' Ist der Ausdruck als Verleihvertrag

	'Public ReadOnly Property GetESUnterzeichner() As String
	'	Get
	'		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'		Dim m_mandant As New Mandant

	'		Dim sValue As Boolean = True

	'		Dim strQuery As String = String.Format("//ExportSetting[@Name={0}SP.ES.PrintUtility{0}]/esunterzeichner_esvertrag", Chr(34))
	'		Dim strBez As String = _ClsProgSetting.GetXMLNodeValue(m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr), strQuery)
	'		sValue = StrToBool(strBez)




	'		'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'		'Dim sValue As String = "1"
	'		'Dim strGuid As String = "SP.ES.PrintUtility"
	'		'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
	'		'Dim strKeyName As String = "ESUnterzeichner_ESVertrag".ToLower
	'		'Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

	'		'Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
	'		'sValue = strBez

	'		Return sValue
	'	End Get

	'End Property

	Public ReadOnly Property GetExportPfad() As String
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

	Public ReadOnly Property GetExportFinalFilename(ByVal bVerleih As Boolean) As String
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

	Public ReadOnly Property GetExportFilename(ByVal bVerleih As Boolean) As String
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

	'Private Function StrToBool(ByVal str As String) As Boolean

	'	Dim result As Boolean = False

	'	If String.IsNullOrWhiteSpace(str) Then
	'		Return False
	'	ElseIf str = "1" Then
	'		Return True

	'	End If

	'	Boolean.TryParse(str, result)

	'	Return result
	'End Function

End Class
