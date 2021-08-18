
Imports SP.DatabaseAccess.Listing.DataObjects


Partial Class ClsExportStart

	''' <summary>
	''' Lohnkonti-Jährliche Liste for export
	''' </summary>
	Function ExportCSVFromLOKontiListing(ByVal data As IEnumerable(Of ListingPayrollLohnkontiData)) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "LOKontiliste.csv"
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

		If String.IsNullOrWhiteSpace(strBez) OrElse strBez.ToLower.Contains("vorname") OrElse strBez.ToLower.Contains("esbegin") Then
			strFieldNameByEmpty = "MANr{0}EmployeeLastname{0}EmployeeFirstname{0}AHVNumber{0}SatrtOfEmployment{0}EndOfEmployment{0}"
			strFieldNameByEmpty &= "LANr{0}Bezeichnung{0}Januar{0}Febrauar{0}March{0}April{0}Mai{0}Juni{0}Juli{0}August{0}September{0}Oktober{0}November{0}Dezember{0}Kumulativ{0}"

		Else
			strFieldNameByEmpty = strBez

		End If
		m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

		Dim _clsKDSearchSetting As New ClsCSVSettings With {.SQL2Open = _ClsSetting.SQL2Open,
																												.ModulName = _ClsSetting.ModulName,
																												.DbConnString2Open = _ClsSetting.DbConnString2Open,
																												.ExportFileName = strExportFilename,
																												.FieldIn = strFieldInByEmpty,
																												.FieldSeprator = strFieldSepratorByEmpty,
																												.SQLFields = strFieldNameByEmpty}
		Dim frm As New frmCSV(m_InitializationData)
		frm.ExportSetting = _clsKDSearchSetting
		frm.LohnKontiData = data
		frm.ChkIgnorEmptyRecords.Enabled = False
		frm.chkRecordsAsHeader.Enabled = True

		frm.Show()
		frm.BringToFront()

		Return strResult
	End Function

	''' <summary>
	''' Guthaben-liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromLOGUListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "GUliste.csv"
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

			strFieldNameByEmpty = "MANr{0}MANachname{0}MAVorname{0}G500{0}G600{0}G700{0}G529{0}G629{0}G729{0}GDar{0}GGTime{0}GGTotal{0}"

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
	''' Jährliche Lohnrekapitulation-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromJLORekapListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "JLORekapListe.csv"
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

			strFieldNameByEmpty = "LANR{0}Bezeichnung{0}Januar{0}Februar{0}März{0}April{0}Mai{0}Juni{0}Juli{0}August{0}September{0}Oktober{0}November{0}Dezember{0}Kumulativ{0}Jahr{0}"

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
	''' KIGA-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromKigaListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "KigaListe.csv"
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

			strFieldNameByEmpty = "jahr{0}anzsmaennner{0}anzsfrauen{0}anzamaenner{0}anzafrauen{0}totalsmaenner{0}totalsfrauen{0}totalamaenner{0}totalafrauen{0}"

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
	''' AHV-Lohnbescheinigungsliste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromJAHVListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "JAHVListe.csv"
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

			' id_1, MANr, maname, ablp, bislp, jahr, _7100, _7120, _7220, _7240, ahvnr, ahvgebdat, mageschlecht
			strFieldNameByEmpty = "id_1{0}MANr{0}maname{0}ablp{0}bislp{0}jahr{0}_7100{0}_7120{0}_7220{0}_7240{0}ahvnr{0}ahvgebdat{0}mageschlecht{0}"

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
	''' UVG-Liste for export
	''' </summary>
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromUVGListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "UVGListe.csv"
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

			'manr, monatvon, monatbis, nachname, vorname, Geschlecht, ahv_nr_new, gebdat, bruttolohn, suvabasis, suvalohn
			strFieldNameByEmpty = "manr{0}monatvon{0}monatbis{0}nachname{0}vorname{0}Geschlecht{0}ahv_nr_new{0}gebdat{0}bruttolohn{0}suvabasis{0}suvalohn{0}"

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
	''' <param name="strSQL2ShowFields"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function ExportCSVFromFrancListing(ByVal strSQL2ShowFields As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim strQuery As String = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName",
																			 Chr(34), _ClsSetting.ModulName)
		Dim strBez As String = _ClsReg.GetXMLNodeValue(m_UserProfileFile, strQuery)
		If strBez <> String.Empty Then
			strExportFilename = strBez
		Else
			strExportFilename = _ClsProgSetting.GetSpSFiles2DeletePath & "FrancListe.csv"
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

			'MANr, Nachname, Vorname, BLohn, magebdat, mageschlecht, eintritt, austritt
			strFieldNameByEmpty = "MANr{0}Nachname{0}Vorname{0}BLohn{0}magebdat{0}mageschlecht{0}eintritt{0}austritt{0}"

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
