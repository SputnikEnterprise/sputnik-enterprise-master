
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.IO
Imports System.ComponentModel
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class ClsExportStart

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsSetting As New ClsCSVSettings
	Private m_UserProfileFile As String




#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsCSVSettings)

		ClsMainSetting.ProgSettingData = _Setting

		If ClsMainSetting.ProgSettingData.TranslationItems Is Nothing Then
			ClsMainSetting.ProsonalizedData = ClsMainSetting.ProsonalizedName
			ClsMainSetting.TranslationData = ClsMainSetting.Translation
		Else
			ClsMainSetting.TranslationData = ClsMainSetting.ProgSettingData.TranslationItems
		End If
		If _Setting.SelectedMDNr = 0 Then ClsMainSetting.ProgSettingData.SelectedMDNr = ClsMainSetting.SelectedMDData.MDNr
		_ClsSetting = _Setting

		m_InitializationData = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
		m_UserProfileFile = _ClsProgSetting.GetUserProfileFile

	End Sub

#End Region


	Function ExportCSVFromKDSearchListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "ExportKundenliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "KDNr{0}Firma1{0}KDFirma2{0}KDFirma3{0}KDPLZ{0}KDOrt{0}KDPostfach{0}KDLand{0}"
			strFieldNameByEmpty &= "KDTelefon{0}KDStrasse{0}KDTelefax{0}KDStrasse{0}"
			strFieldNameByEmpty &= "KDAllFiliale{0}Anrede{0}Nachname{0}Vorname{0}AnredeForm{0}ZHDAbt{0}ZHDPos{0}ZHDBerater"
		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	Function ExportCSVFromMASearchListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "ExportKandidatenliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "MANr{0}MANachname{0}MAVorname{0}MAPLZ{0}MAOrt{0}MALand{0}MATelefon3{0}MAStrasse{0}MACo{0}"
			strFieldNameByEmpty &= "MATelefonG{0}MATelefonP{0}MANatel{0}MA_SMS_Mailing{0}MAGebDat{0}MAGeschlecht{0}MABewillig{0}"
			strFieldNameByEmpty &= "MABew_Bis{0}MAeMail"
		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	Function ExportCSVFromESKDSearchListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, "ExportKundenliste.csv")
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez.ToLower.Contains("KDZNachname".ToLower) Then strBez = String.Empty

		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "KDNr{0}ESKDZHDNr{0}Firma1{0}Nachname{0}Vorname{0}ZNatel{0}ZeMail{0}Anrede{0}AnredeForm{0}KDPostfach{0}KDStrasse{0}KDLand{0}KDPLZ{0}KDOrt"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult

	End Function

	Function ExportCSVFromESMASearchListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "ExportEinsatzliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez.ToLower.Contains("AHV_Nr_New".ToLower) Then strBez = String.Empty

		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}Natel{0}eMail{0}Geschlecht{0}MACo{0}MAPostfach{0}Anredeform{0}"
			strFieldNameByEmpty &= "MAStrasse{0}MALand{0}MAPLZ{0}MAOrt"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	Function ExportCSVFromESSearchListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "ExportEinsatzliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "ESNr{0}MANr{0}KDNr{0}ES_Ab{0}ES_Ende{0}ESBranche{0}"
			strFieldNameByEmpty &= "Firma1{0}KDStrasse{0}KDPostfach{0}KDPLZ{0}KDOrt{0}KDLand{0}KDTelefon{0}KDTelefax{0}"
			strFieldNameByEmpty &= "MANr{0}Nachname{0}Vorname{0}MAStrasse{0}MAPostfach{0}MAPLZ{0}MAOrt{0}MALand{0}"
			strFieldNameByEmpty &= "Telefon_G{0}Telefon_P{0}MANatel{0}GebDat{0}AHV_Nr_New{0}Geschlecht{0}MABeruf{0}ES_Als{0}"
			strFieldNameByEmpty &= "Bewillig{0}Bew_Bis{0}MAeMail{0}KDZNachname{0}KDZVorname{0}KDZAnrede{0}KDZAnredeForm"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	''' <summary>
	''' Debitorenliste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromOPListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "OPListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "RENr{0}Art{0}Fak_Dat{0}BetragOhne{0}BetragEx{0}BetragInk{0}Mwst1{0}Bezahlt{0}FKSoll{0}FKHaben1{0}Faellig{0}ma0{0}ma1{0}ma2{0}ma3{0}"
			strFieldNameByEmpty &= "R_Name1{0}R_Name2{0}R_Name3{0}R_ZHD{0}R_Strasse{0}R_Land{0}R_Ort{0}CreatedOn{0}CreatedFrom{0}ES_Einstufung{0}KDBranche{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	Function ExportCSVFromZEForKDMailing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "ZEListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "ZENr{0}RENr{0}KDNr{0}fak_dat{0}v_date{0}b_date{0}InvoiceCreatedOn{0}currency{0}betrag{0}mwst-betrag{0}mwst{0}createdon{0}r_name1{0}r_zhd{0}r_strasse{0}r_plz{0}r_ort{0}r_land{0}zahlkond{0}faellig{0}mwstproz{0}kst{0}reart{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open,
																												.ModulName = _ClsSetting.ModulName,
																												.DbConnString2Open = _ClsSetting.DbConnString2Open,
																												.ExportFileName = strExportFilename,
																												.FieldIn = strFieldInByEmpty,
																												.FieldSeprator = strFieldSepratorByEmpty,
																												.SQLFields = strFieldNameByEmpty,
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	''' <summary>
	''' Kundenumsatzliste gruppiert für Versand der Mailing KD_CSV
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromKDUmsatzForKDMailing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "KDUmsatzGroupedListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "KDNr{0}Firma1{0}Postfach{0}KDeMail{0}KDStrasse{0}KDLand{0}KDPLZ{0}KDOrt{0}fBetragInk{0}sBetragInk{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	''' <summary>
	''' Kundenumsatzliste CSV
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromKDUmsatzListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "KDUmsatzListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function


	''' <summary>
	''' Liste nicht erfassten Rapporte for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromRPNotFoundedSearchListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "ListeNichtErfassteRapporte.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			'RP.RPNr{0}RP.MANr{0}RP.ESNr{0}RP.KDNr{0}RP.Monat{0}RP.Jahr{0}RP.Result{0}RP.PrintedWeeks{0}RP.Von{0}RP.Bis{0}RP.MDNr{0}MA.Nachname AS MANachname{0}MA.Vorname AS MAVorname{0}KD.Firma1{0}KD.Ort AS KSOrt{0}ES.ES_Ab{0}ES.ES_Ende{0}ES.ES_Als{0}RPDayDb.Day1{0}RPDayDb.Day2{0}RPDayDb.Day3{0}RPDayDb.Day4{0}RPDayDb.Day5{0}RPDayDb.Day6{0}RPDayDb.Day7{0}RPDayDb.Day8{0}RPDayDb.Day9{0}RPDayDb.Day10{0}RPDayDb.Day11{0}RPDayDb.Day12{0}RPDayDb.Day13{0}RPDayDb.Day14{0}RPDayDb.Day15{0}RPDayDb.Day16{0}RPDayDb.Day17{0}RPDayDb.Day18{0}RPDayDb.Day19{0}RPDayDb.Day20{0}RPDayDb.Day21{0}RPDayDb.Day22{0}RPDayDb.Day23{0}RPDayDb.Day24{0}RPDayDb.Day25{0}RPDayDb.Day26{0}RPDayDb.Day27{0}RPDayDb.Day28{0}RPDayDb.Day29{0}RPDayDb.Day30{0}RPDayDb.Day31{0}RPDayDb.WeekNr
			strFieldNameByEmpty = "RPNr{0}MANr{0}ESNr{0}KDNr{0}Monat{0}Jahr{0}Result{0}PrintedWeeks{0}Von{0}Bis{0}MDNr{0}MANachname{0}MAVorname{0}Firma1{0}KSOrt{0}ES_Ab{0}ES_Ende{0}ES_Als{0}Day1{0}Day2{0}Day3{0}Day4{0}Day5{0}Day6{0}Day7{0}Day8{0}Day9{0}Day10{0}Day11{0}Day12{0}Day13{0}Day14{0}Day15{0}Day16{0}Day17{0}Day18{0}Day19{0}Day20{0}Day21{0}Day22{0}Day23{0}Day24{0}Day25{0}Day26{0}Day27{0}Day28{0}Day29{0}Day30{0}Day31{0}WeekNr"


		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function







	''' <summary>
	''' DB1-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromDB1Listing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", _
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "DB1Liste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", _
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "KST3_1{0}KST3Bez{0}USFiliale{0}Monat{0}Jahr{0}_tempumsatz{0}_indUmsatz{0}_festumsatz{0}Bruttolohn{0}ahvlohn{0}agbetrag{0}fremdleistung{0}_lohnaufwand_1{0}_lohnaufwand_2{0}_marge{0}_bgtemp{0}_bgind{0}_bgfest{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function



	''' <summary>
	''' calll history
	''' </summary>
	Function ExportCSVFromCallhistoryListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)

		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "Callhistory.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "Berater{0}USName{0}USVorname{0}USNachname{0}Telefoniert¨An{0}Info{0}Zeitpunkt{0}KDNr{0}ZHDNr{0}MANr{0}Kandidatenname{0}Firmenname{0}Zuständige Person{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty, _
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function



#Region "Export für Abacus..."

	Sub ShowAbacusForm()
		Dim frm As New frmAbacus(_ClsSetting, m_InitializationData)

		frm.Show()
		frm.BringToFront()

	End Sub

	Sub ShowCresusForm()
		Dim frm As New frmCresus(_ClsSetting, m_InitializationData)

		frm.Show()
		frm.BringToFront()

	End Sub

#End Region


#Region "Export für Sesam..."

	Sub ShowSesamForm()
		Dim frm As New frmSesam(_ClsSetting, m_InitializationData)

		frm.Show()
		frm.BringToFront()

	End Sub

#End Region




	''' <summary>
	''' customer and hour list
	''' </summary>
	Function ExportCSVFromCustomerHourData(ByVal data As BindingList(Of CustomertReportHoursData)) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "Kunden Stundenliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "CustomerNumber{0}Company1{0}Company2{0}Company3{0}PostOfficeBox{0}Street{0}"
			strFieldNameByEmpty &= "CountryCode{0}Postcode{0}Location{0}Telephone{0}Telefax{0}EMail{0}TotalHours{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.CusotmerHourData = data.ToList()
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	''' <summary>
	''' emplyoee hour list
	''' </summary>
	Function ExportCSVFromEmployeeHourData(ByVal data As BindingList(Of EmployeeReportHoursData)) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "Kandidaten Stundenliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "MDNr{0}Employeenumber{0}Lastname{0}Firstname{0}Street{0}PostOfficeBox{0}Street{0}"
			strFieldNameByEmpty &= "CountryCode{0}Postcode{0}Location{0}Telephone_P{0}Telephone_2{0}Telephone_3{0}Telephone_G{0}Mobile{0}EMail{0}TotalHours{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open, _
																												.ModulName = _ClsSetting.ModulName, _
																												.DbConnString2Open = _ClsSetting.DbConnString2Open, _
																												.ExportFileName = strExportFilename, _
																												.FieldIn = strFieldInByEmpty, _
																												.FieldSeprator = strFieldSepratorByEmpty, _
																												.SQLFields = strFieldNameByEmpty}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.EmployeeHourData = data.ToList()
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function



	Sub ShowFax2eCallForm()
		Dim frm As New frmDoc2eCall(_ClsSetting)

		frm.Show()
		frm.BringToFront()

	End Sub


	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


End Class
