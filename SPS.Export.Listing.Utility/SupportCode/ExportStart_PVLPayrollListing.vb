


Partial Class ClsExportStart


	''' <summary>
	''' FAR-Lohnbescheinigungsliste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromFARListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "FarListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			' MANr, Nachname, Vorname, Gebdat, AHV_Nr, Ablp, BisLP, anzahlstd, Lohnsumme 
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}Gebdat{0}AHV_Nr{0}Ablp{0}BisLP{0}anzahlstd{0}Lohnsumme{0}"

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
	''' Inkassopool-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromInkassopoolListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "InkassopoolListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			'AnzahlStd, BeitragLohnAG, BeitragLohnAN, Lohnsumme, GAV_VAG_S, GAV_VAN_S, GAV_WAG_S, GAV_WAN_S, GAV_VAG, GAV_VAN, GAV_WAG, GAV_WAN, Pauschale, MANr, Nachname, Vorname, Jahrgang, GebDat, AHV_Nr 
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}GebDat{0}AHV_Nr_New{0}AnzahlStd{0}BeitragLohnAN{0}BeitragLohnAG{0}Lohnsumme{0}EinsatzAls{0}Ablp{0}BisLp{0}"
			strFieldNameByEmpty &= "GAV_VAG_S{0}GAV_VAN_S{0}GAV_WAG_S{0}GAV_WAN_S{0}GAV_VAG{0}GAV_VAN{0}GAV_WAG{0}GAV_WAN{0}Pauschale{0}"
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
	''' PVL-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromPVLListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "PVLListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			' MANr, Nachname, Vorname, GebDat, AHV_Nr_New, AnzahlStd, BeitragLohnAN, BeitragLohnAG, Lohnsumme, EinsatzAls, Ablp, BisLp
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}GebDat{0}AHV_Nr_New{0}AnzahlStd{0}BeitragLohnAN{0}BeitragLohnAG{0}Lohnsumme{0}EinsatzAls{0}Ablp{0}BisLp{0}"

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
	''' GEFAK-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromGEFAKListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "GEFAKListe.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			' AnzahlStd, BeitragLohnAN, Lohnsumme, EinsatzAls, GAV_VAG_S, GAV_VAN_S, GAV_WAG_S, GAV_WAN_S, GAV_VAG, GAV_VAN, GAV_WAG, GAV_WAN, Pauschale, MANr, Nachname, Vorname
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}AnzahlStd{0}BeitragLohnAN{0}Lohnsumme{0}EinsatzAls{0}Ablp{0}BisLp{0}"
			strFieldNameByEmpty &= "GAV_VAG_S{0}GAV_VAN_S{0}GAV_WAG_S{0}GAV_WAN_S{0}GAV_VAG{0}GAV_VAN{0}GAV_WAG{0}GAV_WAN{0}Pauschale{0}"
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
	''' Schreiner-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromGAVSchreinerListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "Schreinerliste.csv"
		End If
		Dim strFieldSepratorByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldSepratorByEmpty = strBez
		Else
			strFieldSepratorByEmpty = ";"
		End If
		Dim strFieldInByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldInByEmpty = strBez
		Else
			strFieldInByEmpty = String.Empty
		End If

		Dim strFieldNameByEmpty As String = String.Empty
		strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields",
																			 Chr(34), _ClsSetting.ModulName)
		strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetUserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strFieldNameByEmpty = strBez

		Else

			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}GebDat{0}AHV_Nr_New{0}AnzahlStd{0}BeitragLohnAN{0}BeitragLohnAG{0}Lohnsumme{0}EinsatzAls{0}Ablp{0}BisLp{0}"
			strFieldNameByEmpty &= "VonMonat{0}BisMonat{0}Jahr{0}Jahrgang{0}Strasse{0}PLZ{0}Ort{0}GAV_VAG_S{0}GAV_WAG_S{0}GAV_VAG{0}GAV_WAG{0}GAV_VAN_S{0}"
			strFieldNameByEmpty &= "GAV_WAN_S{0}GAV_VAN{0}GAV_WAN{0}Pauschale{0}GAV_Name{0}GAV_ZHD{0}GAV_Postfach{0}GAV_Strasse{0}GAV_PLZ{0}GAV_Ort{0}GAV_Bank{0}GAV_BankPLZOrt{0}"
			strFieldNameByEmpty &= "GAV_Bankkonto{0}GAV_IBAN{0}GAVBeruf{0}GAVKanton{0}"

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open,
																												.ModulName = _ClsSetting.ModulName,
																												.DbConnString2Open = _ClsSetting.DbConnString2Open,
																												.ExportFileName = strExportFilename,
																												.FieldIn = strFieldInByEmpty,
																												.FieldSeprator = strFieldSepratorByEmpty,
																												.SQLFields = strFieldNameByEmpty,
																												.SQL4FieldShow = strSQL2ShowFields}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsSearchSetting
		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

End Class
