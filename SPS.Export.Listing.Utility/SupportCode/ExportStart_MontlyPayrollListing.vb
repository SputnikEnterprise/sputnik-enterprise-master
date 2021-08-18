

Partial Class ClsExportStart

	''' <summary>
	''' AN-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOAGListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LOAGliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "MANR{0}Jahr{0}LP{0}LANR{0}LALOText{0}M_Bas{0}M_Anz{0}M_Ans{0}M_Btr{0}GAV_Kanton{0}GAV_Beruf{0}GAV_Gruppe1{0}Nachname{0}Vorname{0}"
			strFieldNameByEmpty &= "AHV_Nr_New{0}MALand{0}PLZ{0}Ort{0}Strasse{0}GebDat{0}PLZ{0}Ort{0}"

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
	''' lo ag rekap-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOAGRekapListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LOAG-Rekapliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else
			strFieldNameByEmpty = "LANr{0}Bezeichnung{0}Jahr{0}LP{0}LANR{0}LALOText{0}Betrag{0}Kumulativ{0}HKonto{0}SKonto{0}"

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
	''' buchungsbeleg für fibu to csv-format
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOFibuListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "Fibuliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else
			strFieldNameByEmpty = "LANr{0}LALOText{0}HKonto{0}SKonto{0}Vorzeichen_2{0}Vorzeichen_3{0}Totalbetrag{0}"

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
	''' bruttolohn journal to csv-format
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromBLJListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "BLJliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}Monatvon{0}MonatBis{0}BLohn{0}AHVBas{0}SUVABas{0}"

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
	''' Fremdleistung-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOFremdleistungListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "Fremdleistungliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "LANr{0}LALOText{0}Betrag{0}"

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
	''' Kinder- Ausbildungszulagen Jährliche Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOYFakListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LOYFakliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "MANr{0}Jahr{0}USNr{0}_3600{0}_3602{0}_3650{0}_3700{0}_3750{0}_3800{0}_3850{0}_3900{0}_3900_1{0}_3901{0}_3901_1{0}MDNr{0}MANachname{0}MAVorname{0}Zivilstand{0}GebDat{0}AHV_Nr{0}AHV_Nr_New{0}"

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
	''' Stunden-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOStdListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LOStdListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "LONr{0}MANr{0}Vorname{0}Nachname{0}LANr{0}LP{0}Jahr{0}m_btr{0}gav_kanton{0}gav_beruf{0}"

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
	''' Französische Grenzgänger-Liste for export
	''' </summary>
	Function ExportCSVFromMFakListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "MFakListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			'Select LOL.LONr, LOL.MANr, LOL.LP, LOL.LANr, LOL.m_Anz, LOL.m_Bas, LOL.m_Ans, LOL.m_Btr, LOL.Jahr, LOL.S_Kanton, LOL.RPText, MA.Nachname As MANachname, MA.Vorname As MAVorname, (Select Count(*) From MA_KIAddress Where MANr = MA.MANr) As MAKIAnz From LOL Left Join Mitarbeiter MA On LOL.MANr = MA.MANr  Where  LOL.Lanr In (3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901) And LOL.LP like 4 And LOL.Jahr In (2014) And  LOL.m_Btr <> 0  Order By MA.Nachname ASC, MA.Vorname ASC, LOL.LP ASC
			strFieldNameByEmpty = "MANr{0}LP{0}LANr{0}RPText{0}m_Btr{0}Jahr{0}S_Kanton{0}MAKIAnz{0}MANachname{0}MAVorname{0}"

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
	''' AN-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOANListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LOANliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "MANR{0}Jahr{0}LP{0}LANR{0}LALOText{0}M_Bas{0}M_Anz{0}M_Ans{0}M_Btr{0}GAV_Kanton{0}GAV_Beruf{0}GAV_Gruppe1{0}Nachname{0}Vorname{0}"
			strFieldNameByEmpty &= "AHV_Nr_New{0}MALand{0}PLZ{0}Ort{0}Strasse{0}GebDat{0}PLZ{0}Ort{0}"

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
	''' lo rekap-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLORekapListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LORekapliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else
			strFieldNameByEmpty = "LANr{0}Bezeichnung{0}Jahr{0}LP{0}LANR{0}LALOText{0}Betrag{0}Kumulativ{0}HKonto{0}SKonto{0}"

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
	''' lo ktg-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromKTGListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "KTGliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "MANR{0}Jahr{0}LP{0}LANR{0}LALOText{0}M_Bas{0}M_Anz{0}M_Ans{0}M_Btr{0}GAV_Kanton{0}GAV_Beruf{0}GAV_Gruppe1{0}Nachname{0}Vorname{0}"
			strFieldNameByEmpty &= "AHV_Nr_New{0}MALand{0}PLZ{0}Ort{0}Strasse{0}GebDat{0}PLZ{0}Ort{0}"

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
	''' qstliste for export to csv
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromQSTListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "QSTliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If Not String.IsNullOrWhiteSpace(strBez) AndAlso Not strBez.ToUpper.Contains("LALOText".ToUpper) Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}GebDat{0}AHV_Nr_New{0}MAStrasse{0}MAPLZ{0}MAOrt{0}MALand{0}ESAb{0}ESEnde{0}StdAnz{0}S_Kanton{0}M_Anz{0}M_Bas{0}M_Ans{0}M_Btr{0}QSTBasis{0}Bruttolohn{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open,
			.ModulName = _ClsSetting.ModulName,
			.DbConnString2Open = _ClsSetting.DbConnString2Open,
			.ExportFileName = strExportFilename,
			.FieldIn = strFieldInByEmpty,
			.FieldSeprator = strFieldSepratorByEmpty,
			.SQLFields = strFieldNameByEmpty,
			.SQL4FieldShow = strSQL2ShowFields
		}

		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.ExportModul = frmCSV.EnumModul.SEARCHRESULTTAXDATA

		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	''' <summary>
	''' bvgliste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromBVGListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "BVGliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez
		Else
			strFieldNameByEmpty = "MANr{0}LANr{0}LALOText{0}M_Anz{0}M_Bas{0}M_Ans{0}M_Btr{0}Nachname{0}Vorname{0}GebDat{0}"
			strFieldNameByEmpty &= "GebDat{0}AHV_Nr_New{0}MAStrasse{0}MAPLZ{0}MAOrt{0}MALand{0}Zivilstand{0}BVGStd{0}"
			strFieldNameByEmpty &= "AHVLohn{0}BVGEin{0}BVGAus{0}"

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







End Class
