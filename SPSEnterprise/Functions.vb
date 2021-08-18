
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports SPProgUtility.MainUtilities
Imports NLog
Imports System.Xml
Imports System.Xml.Linq
Imports System.Text
Imports System.Collections.Generic

Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports Microsoft.Win32


Module Functions

	Private logger As Logger = LogManager.GetCurrentClassLogger()

	Dim _ClsReg As New SPProgUtility.ClsDivReg
	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Dim _ClsSystem As New ClsMain_Net
	Dim m_utility As New Utilities

	Dim strConnString As String = _ClsSystem.GetConnString()


	Private Const OLD_SQL_CONNECTION_STRING = "Provider=SQLOLEDB.1;"
	Private Const DBSELECT_MDREFERENCE = "Mandant0000"
	Private Const LOCAL_UPDATE_ARG As String = ""


	Function LoadStartSettings() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim _clsSystem As New ClsMain_Net
		Dim SerNumber As String
		Dim ProgName As String
		Dim UserName As String
		Dim OldrootIniFullName As String
		Dim strEncryptedLicence As String

		Try
			' die Version des laufenden System ermitteln. Mehrfach Start auf normaler Station ist nicht erlaubt.
			Dim strOSVersion As String = GetOSVersion()
			If Not strOSVersion.ToUpper.Contains("Server".ToUpper) Then
				If IsProcessRunning("spsModulsview") Then

					If Not IsProcessRunning("sps.mainview") Then
						TerminateSelectedProcess("spsModulsview")
					Else
						Dim msg = String.Format(GetSafeTranslationValue("Das ausgewählte Programm {0} ist bereits aktiv!"), "Hauptübersicht")
						DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Programmstart", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

						Return False

					End If

				End If

			End If

		Catch ex As Exception
			logger.Error(ex.ToString)
		End Try

		Try
			SrvBinPath = _clsSystem.GetInitPath()
			SrvSettingFullFileName = _clsSystem.GetInitIniFile()


			' wenn die Einstellungen weg sind, dann muss es resetet werden...
			If Not System.IO.File.Exists(SrvSettingFullFileName) Then Call ResetRegistrySetting()
			SrvSettingFullFileName = _clsSystem.GetInitIniFile()


			Try
				If Not System.IO.File.Exists(SrvSettingFullFileName) Then
					Dim strMessage As String = "Leider konnte ich Ihre Datei für die Einstellungen nicht finden. "
					strMessage &= "Bitte geben Sie den Pfad ein:{0}{0}"
					strMessage &= "Mögliche Eingaben:{0}"
					strMessage &= "\\Servername{0}"
					strMessage &= "\\Servername\Spenterprise$\{0}"
					strMessage &= "\\Servername\spenterprise$\Bin\Programm.dat"
					strMessage = String.Format(strMessage, vbNewLine)
					strMessage = GetSafeTranslationValue(strMessage)

					OldrootIniFullName = InputBox(strMessage, My.Application.Info.ProductName, SrvSettingFullFileName)
					If OldrootIniFullName.ToUpper.Contains("Programm.dat".ToUpper) Then
						SrvSettingFullFileName = OldrootIniFullName

					ElseIf Not OldrootIniFullName.ToUpper.Contains("Spenterprise".ToUpper) Then
						SrvSettingFullFileName = AddDirSep(OldrootIniFullName) & "Spenterprise$\Bin\Programm.dat"
						OldrootIniFullName = SrvSettingFullFileName

					Else
						SrvSettingFullFileName = AddDirSep(OldrootIniFullName) & "Bin\Programm.dat"
						OldrootIniFullName = SrvSettingFullFileName

					End If

					If Not System.IO.File.Exists(SrvSettingFullFileName) Then
						Dim msg = String.Format(GetSafeTranslationValue("Die Progammeinstellungsdatei wurde nicht gefunden.{0}Das Programm wird abgebrochen.{0}{1}"), vbNewLine, SrvSettingFullFileName)
						DevExpress.XtraEditors.XtraMessageBox.Show(msg, "LoadStartSettings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

						Return False

					Else
						Dim CheckFile As New FileInfo(OldrootIniFullName)
						SrvBinPath = AddDirSep(CheckFile.DirectoryName.ToString)
						_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath", SrvBinPath)

					End If
				End If
				Dim strFileServerPath As String = Path.GetDirectoryName(Mid(SrvBinPath, 1, Len(SrvBinPath) - 1))
				_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SPSFileServerPath", AddDirSep(strFileServerPath))

			Catch ex As Exception
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Serverpfad konnte nicht gesetzt werden: {0}", ex.ToString), "LoadStartSettings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

			End Try

			SQLDateformat = _clsSystem.GetSQLDateFormat()

			SerNumber = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", My.Resources.str422)
			ProgName = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", My.Resources.str424)
			UserName = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", My.Resources.str425)
			Dim sLizCount As Short = CShort(_ClsReg.GetINIString(SrvSettingFullFileName, "Customer", My.Resources.str423))
			strEncryptedLicence = EncryptMyString(UCase(UserName) & CStr(sLizCount) & strExtraPass, strEncryptionKey)

			If SerNumber <> strEncryptedLicence Then
				Dim msg = String.Format(GetSafeTranslationValue("Das Programm ist noch nicht registriert.{0}Bitte kontaktieren Sie Ihren Systemadministrator."), vbNewLine)
				DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Lizenzangaben", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

				Return False

			End If
			' Einstellungen noch speichern (für Pfad-Verlust!)
			Call SaveDataFromRegistry()


		Catch ex As Exception
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "LoadStartSettings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

			Return False

		End Try


		Return True

	End Function

	Sub TerminateSelectedProcess(ByVal strProgFullname As String)

		Try
			Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName(strProgFullname)
			For Each p As Process In pProcess
				p.Kill()
			Next

		Catch ex As Exception

		End Try

	End Sub

	' Die Lokale Daten zur Sicherheit noch einmal sichern
	Sub SaveDataFromRegistry()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim printerPath As String
		Dim strAmtkennzahl As String
		Dim strDocPath As String
		Dim iStartTerminplaner As Short
		Dim _clsSystem As New ClsMain_Net

		Try
			' Serverpath
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str272, AddDirSep(SrvBinPath))

			' Verzeichnis für Druckerdateien
			printerPath = _ClsReg.GetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str145)
			If String.IsNullOrWhiteSpace(printerPath) OrElse Not Directory.Exists(printerPath) Then printerPath = _clsSystem.GetUserHomePath()
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str145, AddDirSep(printerPath))
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str500, AddDirSep(printerPath))
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str500, AddDirSep(printerPath))






			' Amtskennziffer fürs Telefonieren...
			strAmtkennzahl = _ClsReg.GetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str460)
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str251, My.Resources.str460, strAmtkennzahl)

			' Verzeichnis für Dokumente (Lokal!!!)
			strDocPath = _ClsReg.GetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str436)
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str436, AddDirSep(strDocPath))

			' Terminliste anzeigen (Lokal!!!)
			iStartTerminplaner = Val(_ClsReg.GetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str218))
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str251, My.Resources.str218, Str(iStartTerminplaner))

			' Updateprogramm starten(Lokal!!!)
			_ClsReg.SetRegKeyValue(strSaveRegKey & My.Resources.str251, My.Resources.str219, "1")


		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "SaveDataFromRegistry")

		End Try

	End Sub

	' Die Lokale Daten Reseten...
	Sub ResetRegistrySetting()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strPrinterPath As String
		Dim strAmtkennzahl As String
		Dim strDocPath As String
		Dim iStartTerminplaner As Short

		Try
			' Serverpath
			SrvBinPath = AddDirSep(_ClsReg.GetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str272))
			If SrvBinPath.Length > 1 Then _ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str272, SrvBinPath)

			strPrinterPath = _ClsReg.GetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str145)
			If strPrinterPath.Trim.Length = 0 Then strPrinterPath = _ClsSystem.GetUserHomePath
			' Verzeichnis für Druckerdateien
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str145, AddDirSep(strPrinterPath))
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str500, AddDirSep(strPrinterPath))

			' Amtskennziffer fürs Telefonieren...
			strAmtkennzahl = _ClsReg.GetRegKeyValue(strSaveRegKey & My.Resources.str251, My.Resources.str460)
			If strAmtkennzahl.Length > 0 Then _ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str460, strAmtkennzahl)

			' Verzeichnis für Dokumente (Lokal!!!)
			strDocPath = _ClsReg.GetRegKeyValue(strSaveRegKey & My.Resources.str256, My.Resources.str436)
			If strDocPath.Length > 1 Then _ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str436, AddDirSep(strDocPath))

			' Terminliste anzeigen (Lokal!!!)
			iStartTerminplaner = Val(_ClsReg.GetRegKeyValue(strSaveRegKey & My.Resources.str251, My.Resources.str218))
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str218, Str(iStartTerminplaner))

			' Updateprogramm starten(Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str219, "1")

		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "ResetRegistrySetting")

		End Try

	End Sub

	Sub CreateRegistryWithNewSetting_1()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strPrinterPath As String

		Try
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Forms\Coordination", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Forms\LoadedForms", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Forms\LVG-Property", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Forms\SearchFields", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Forms\Tabs", "_Standard", "0")

			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbForList", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options\TapiDevice", "_Standard", "0")

			_ClsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "_Standard", "0")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Save", "_Standard", "0")

			' Serverpath
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str272, "")

			strPrinterPath = _ClsSystem.GetUserHomePath()
			' Verzeichnis für Druckerdateien
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str145, strPrinterPath)

			' Amtskennziffer fürs Telefonieren...
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str460, "")

			' Verzeichnis für Dokumente (Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str436, "Templates\")
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, My.Resources.str276, "Documents\")

			' Terminliste anzeigen (Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str218, "0")

			' Updateprogramm starten(Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str219, "1")

			' Optionen 
			_ClsReg.SetRegKeyValue(RegStr_Net & My.Resources.str256, "USLanguage", "")

		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "CreateRegistryWithNewSetting_1")

		End Try

	End Sub

	Sub WriteError(ByRef iErrNum As Short, _
								 ByRef sDesc As String, _
								 ByRef sSource As String, _
								 ByRef SDate As String, _
								 ByRef sPath As String)
		'// Writes all errors to err.log
		Dim F As Short

		F = FreeFile()
		FileOpen(F, sPath, OpenMode.Append)
		PrintLine(F, "Error Number: " & iErrNum)
		PrintLine(F, "Description: " & sDesc)
		PrintLine(F, "Source: " & sSource)
		PrintLine(F, "Date: " & SDate)
		PrintLine(F, "")
		FileClose(F)

	End Sub

	Function GetDatabase(ByRef MDSection As String) As SqlConnection
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim ConnTemp As SqlConnection
		Dim existsNetConnstring As Boolean = False
		Dim msg As String = String.Empty
		Dim CnnStr As String = String.Empty
		Dim sSql As String = String.Empty

		Try
			logger.Debug(String.Format("MDSection: {0}", MDSection))

			CnnStr = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "_Net")
			existsNetConnstring = Val(_ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "_Net")) = 1
			If Not existsNetConnstring Then
				CnnStr = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "ConnStr")

				CnnStr = CnnStr.Replace(OLD_SQL_CONNECTION_STRING, String.Empty)
				_ClsReg.SetINIString(SrvSettingFullFileName, MDSection, "ConnStr_Net", CnnStr)
				_ClsReg.SetINIString(SrvSettingFullFileName, MDSection, "_Net", "1")

			Else
				CnnStr = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "ConnStr_Net")
			End If

			If String.IsNullOrWhiteSpace(CnnStr) Then
				msg = "Für den Mandant {0} existieren keine Angaben in {1} zur Verbindung an Ihre Datenbank.{2}"
				msg &= "Bitte kontaktieren Sie Ihren Systemadministrator."
				msg = String.Format(GetSafeTranslationValue(msg), MDSection, SrvSettingFullFileName, vbNewLine)

				DevExpress.XtraEditors.XtraMessageBox.Show(msg, GetSafeTranslationValue("Fehlende Datenbankverbindung"), MessageBoxButtons.OK, MessageBoxIcon.Error)

				Return Nothing

			Else
				ConnTemp = New SqlConnection(CnnStr)
				ConnTemp.Open()

			End If

			If ConnTemp Is Nothing OrElse ConnTemp.State = ConnectionState.Closed Then
				msg = "Sie sind nicht berechtigt mit diesem Programm zu arbeiten.{0}Bitte kontaktieren Sie Ihrem Systemadministrator."
				msg = String.Format(GetSafeTranslationValue(msg), vbNewLine)

				DevExpress.XtraEditors.XtraMessageBox.Show(msg, GetSafeTranslationValue("Berechtigung nicht erteilt"), MessageBoxButtons.OK, MessageBoxIcon.Error)

				Return Nothing
			End If
			Return ConnTemp

		Catch ex As Exception
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.ToString))
			DevExpress.XtraEditors.XtraMessageBox.Show(Err.Description, "GetDataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)

			Return Nothing

		End Try

	End Function

	Function GetOSVersion() As String
		Dim strResult As String = My.Computer.Info.OSFullName

		Return strResult
	End Function

	Function GetDbNewConn(ByVal strMDSection As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strRootConn As String = _ClsSystem.GetDbSelectConnString()
		Dim ConnRoot As New SqlConnection(strRootConn)

		Dim strSqlQuery As String = "Select Top 1 con.ConnStr From [Sputnik DbSelect].Dbo.SpConnStr con "
		strSqlQuery &= "Where con.ForWhat = '" & strMDSection & "' Order By ID Desc"

		Try
			ConnRoot.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, ConnRoot)
			Dim rConnrec As SqlDataReader = cmd.ExecuteReader					 ' SpConnStr-Datenbank

			While rConnrec.Read
				strResult = rConnrec("ConnStr").ToString
				strResult = Decrypt(strResult)
				If Not strResult.ToUpper.Contains("Current language=".ToUpper) Then
					strResult &= ";Current language=German"
				End If

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			ConnRoot.Dispose()

		End Try

		Return strResult
	End Function

	Sub SetDbNewConn(ByVal strConnstr As String, ByVal strMDSection As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = "Delete [Sputnik DbSelect].Dbo.SpConnStr  Where ForWhat = @ForWhat; Insert [Sputnik DbSelect].Dbo.SpConnStr (ForWhat, ConnStr) Values (@ForWhat, @ConnStr)"
		Dim strRootConn As String = _ClsSystem.GetDbSelectConnString()
		Dim ConnRoot As New SqlConnection(strRootConn)

		Try
			ConnRoot.Open()
			strConnstr = Encrypt(strConnstr)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnRoot)
			Dim param As System.Data.SqlClient.SqlParameter = New SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@ForWhat", strMDSection)
			param = cmd.Parameters.AddWithValue("@ConnStr", strConnstr)
			' ändern...
			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			ConnRoot.Dispose()

		End Try

	End Sub


#Region "Funktionen zur Übersetzung der Daten..."

	Public Function GetSafeTranslationValue(ByVal dicKey As String) As String

		Try
			If ClsPublicData.TranslationData.ContainsKey(dicKey) Then
				Return ClsPublicData.TranslationData.Item(dicKey).LogedUserLanguage

			Else
				Return dicKey

			End If

		Catch ex As Exception
			Return dicKey

		End Try

	End Function

#End Region

	Function AddDirSep(ByVal strPathName As String) As String

		Const gstrSEP_URLDIR As String = "/"											' Separator for dividing directories in URL addresses.
		Const gstrSEP_DIR As String = "\"                                                   ' Directory separator character

		If Right(Trim(strPathName), Len(gstrSEP_URLDIR)) <> gstrSEP_URLDIR And Right(Trim(strPathName), Len(gstrSEP_DIR)) <> gstrSEP_DIR Then
			strPathName = RTrim$(strPathName) & gstrSEP_DIR
		End If
		AddDirSep = strPathName

	End Function


	Public Sub StartMainView()

		Dim startInfo As New ProcessStartInfo

		Dim Prog2Start As String = Path.Combine(My.Application.Info.DirectoryPath, "SPS.MainView.exe")
		startInfo.FileName = Prog2Start
		startInfo.Arguments = ""
		startInfo.UseShellExecute = False
		Process.Start(startInfo)

	End Sub

End Module


Public Class ClsWriteDataToXML

	Private logger As Logger = LogManager.GetCurrentClassLogger()
	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private doc As New Xml.XmlDocument
	Private xmlfile As String
	Private _ClsSetting As New ClsMDData


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsMDData)
		Me._ClsSetting = _setting
	End Sub

#End Region



End Class
