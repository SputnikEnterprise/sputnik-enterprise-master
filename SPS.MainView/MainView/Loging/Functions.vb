
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.Infrastructure.UI.UtilityUI
Imports SP.Infrastructure.Settings

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports System.Data.SqlClient

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPS.MainView.EmployeeSettings

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports Microsoft.Win32


Module Functions

	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_utilityUI As New UtilityUI
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As New TranslateValues

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private Const SPSRegKey As String = "Software\yourregistrykeyname"
	Private Const LocalUpdateArg As String = ""


	Private ReadOnly Property GetLocalServicePath() As String
		Get
			Dim strLocalpath As String = _ClsProgSetting.GetLocalBinnPath()
			strLocalpath &= "Services\"
			Return strLocalpath
		End Get
	End Property


	Function LoadStartSettings() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim SerNumber As String
		Dim ProgName As String
		Dim UserName As String
		Dim OldrootIniFullName As String
		Dim strEncryptedLicence As String
		Dim bOpenUpdateProg As Boolean

		Try

			SrvBinPath = _ClsProgSetting.GetInitPath()
			SrvSettingFullFileName = _ClsProgSetting.GetInitIniFile()

			' wenn die Einstellungen weg sind, dann muss es resetet werden...
			If Not System.IO.File.Exists(SrvSettingFullFileName) Then
				If Not ResetRegistrySetting() Then Return False
			End If

			SrvSettingFullFileName = _ClsProgSetting.GetInitIniFile()


			Try
				If Not System.IO.File.Exists(SrvSettingFullFileName) Then
					Dim strMessage As String = "Leider konnte ich Ihre Datei für die Einstellungen nicht finden. "
					strMessage &= "Bitte geben Sie den Pfad ein:{0}{0}"
					strMessage &= "Mögliche Eingaben:{0}"
					strMessage &= "\\Servername{0}"
					strMessage &= "\\Servername\Spenterprise$\{0}"
					strMessage &= "\\Servername\spenterprise$\Bin\Programm.dat"
					strMessage = String.Format(strMessage, vbNewLine)
					strMessage = m_translate.GetSafeTranslationValue(strMessage)

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
						Dim msg = String.Format("Die Progammeinstellungsdatei wurde nicht gefunden.{0}Das Programm wird abgebrochen.{0}{1}", vbLf, SrvSettingFullFileName)
						m_Logger.LogError(msg)
						m_utilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue(msg))

						Return False

					Else
						Dim CheckFile As New FileInfo(OldrootIniFullName)
						SrvBinPath = AddDirSep(CheckFile.DirectoryName.ToString)
						_ClsReg.SetRegKeyValue(SPSRegKey & "\Sputnik Suite\Path", "InitPath", SrvBinPath)

					End If
				End If
				Dim strFileServerPath As String = Path.GetDirectoryName(Mid(SrvBinPath, 1, Len(SrvBinPath) - 1))
				_ClsReg.SetRegKeyValue(SPSRegKey & "\Sputnik Suite\Path", "SPSFileServerPath", AddDirSep(strFileServerPath))

			Catch ex As Exception
				Dim msg = String.Format("Loadstartsettings (1. Phase) | {0}", ex.ToString)
				m_Logger.LogError(msg)
				m_utilityUI.ShowErrorDialog(msg)

			End Try

			SQLDateformat = _ClsProgSetting.GetSQLDateFormat()
			If String.IsNullOrWhiteSpace(SQLDateformat) Then SQLDateformat = "dd.MM.yyyy"

			SerNumber = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "S/N")
			ProgName = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "ProgName")
			UserName = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "UserName")
			Dim sLizCount As Short = CShort(Val(_ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "UserCount")))
			strEncryptedLicence = EncryptMyString(UCase(UserName) & CStr(sLizCount) & strExtraPass, strEncryptionKey)

			If SerNumber <> strEncryptedLicence Then
				Dim msg As String = String.Format(m_translate.GetSafeTranslationValue("(Lizenz):{0}Das Programm ist noch nicht registriert.{0}Bitte kontaktieren Sie Ihren Systemadministrator."), vbNewLine)
				m_Logger.LogError(msg)
				m_utilityUI.ShowErrorDialog(msg)

				Return False

			End If
			Try
				Dim value = _ClsReg.GetRegKeyValue("HKLM\Software\Microsoft\Windows\CurrentVersion\Policies" & "\System", "EnableLUA")
				If String.IsNullOrWhiteSpace(value) Then
					bOpenUpdateProg = False
				Else
					bOpenUpdateProg = Not _ClsReg.GetRegKeyValue("HKLM\Software\Microsoft\Windows\CurrentVersion\Policies" & "\System", "EnableLUA")
				End If
				If Not Environment.UserName.ToUpper.Contains("Fardin".ToUpper) Then
					bOpenUpdateProg = True
				Else
					bOpenUpdateProg = CBool(_ClsReg.GetRegKeyValue(RegStr_Net & "\Options", "ShowUpdateOnStart"))
				End If
				m_Logger.LogInfo(String.Format("Domainname: {0}", Environment.UserDomainName.ToLower))
				If Environment.UserDomainName.ToLower = "zeda" OrElse Environment.UserDomainName.ToLower = "da" OrElse Environment.UserDomainName.ToLower = "work" Then
					Dim reg As Microsoft.Win32.RegistryKey
					reg = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System", True)
					Dim s = reg.GetValue("EnableLUA", "")
					If s = "1" Then bOpenUpdateProg = False
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Fehler beim Lesen von Policies!"))

			End Try

			' Einstellungen speichern (für Pfad-Verlust!)
			Call SaveDataFromRegistry()
            'If bOpenUpdateProg Then RunProgUpdate()


        Catch ex As Exception
			m_utilityUI.ShowErrorDialog(ex.ToString)
			m_Logger.LogError(ex.ToString)

			Return False

		End Try

		Return True

	End Function



	Sub RunProgUpdate()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim _clsreg As New SPProgUtility.ClsDivReg
		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		SrvBinPath = _ClsProgSetting.GetInitPath()
		SrvSettingFullFileName = _ClsProgSetting.GetInitIniFile()

		Try
			' Updateprogramme
			Dim strUpdateFilename As String = Mid("SPSCLUpdate.exe", 1, Len("SPSCLUpdate.exe") - 4)

			If IsProcessRunning(strUpdateFilename) Then TerminateSelectedProcess(strUpdateFilename)

			' Update installieren
			If File.Exists(Path.Combine(_ClsProgSetting.GetLocalBinnPath(), strUpdateFilename & ".exe")) Then
				Dim startInfo As New ProcessStartInfo(Path.Combine(_ClsProgSetting.GetLocalBinnPath(), strUpdateFilename & ".exe"))
				startInfo.Arguments = If(LocalUpdateArg = String.Empty, "/SILENT /AUTOSTART", LocalUpdateArg)
				Process.Start(startInfo)
			End If


		Catch ex As Exception
			m_Logger.LogError("Update could not be started!")

		End Try

	End Sub

	Sub TerminateSelectedProcess(ByVal strProgFullname As String)
		'Dim p As Process = Process.GetProcessesByName(strProgFullname)(0)
		'p.Kill()
		Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName(strProgFullname)

		For Each p As Process In pProcess
			p.Kill()
		Next

	End Sub

	' Die Lokale Daten zur Sicherheit noch einmal sichern
	Private Function SaveDataFromRegistry() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim result As Boolean = True
		Dim printerPath As String
		Dim strAmtkennzahl As String
		Dim strDocPath As String
		Dim iStartTerminplaner As Short

		Try
			' Serverpath
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Path", "InitPath", AddDirSep(SrvBinPath))

			' Verzeichnis für Druckerdateien

			printerPath = _ClsReg.GetRegKeyValue(RegStr_Net & "\Path", "PrintFileSaveIn")
			If String.IsNullOrWhiteSpace(printerPath) OrElse Not Directory.Exists(printerPath) Then printerPath = _ClsProgSetting.GetUserHomePath()
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Path", "PrintFileSaveIn", AddDirSep(printerPath))
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Path", "WordTempPathToSave", AddDirSep(printerPath))
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "WordTempPathToSave", AddDirSep(printerPath))

			' Amtskennziffer fürs Telefonieren...
			strAmtkennzahl = _ClsReg.GetRegKeyValue(RegStr_Net & "\Options", "AmtsZiffer")
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Options", "AmtsZiffer", strAmtkennzahl)

			' Verzeichnis für Dokumente (Lokal!!!)
			strDocPath = _ClsReg.GetRegKeyValue(RegStr_Net & "\Path", "TemplatePath")
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Path", "TemplatePath", AddDirSep(strDocPath))

			' Terminliste anzeigen (Lokal!!!)
			iStartTerminplaner = Val(_ClsReg.GetRegKeyValue(RegStr_Net & "\Options", "ShowAgendaOnStart"))
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Options", "ShowAgendaOnStart", Str(iStartTerminplaner))

			' Updateprogramm starten(Lokal!!!)
			_ClsReg.SetRegKeyValue(strSaveRegKey & "\Options", "ShowUpdateOnStart", "1")


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			MsgBox(Err.Description, MsgBoxStyle.Critical, "SaveDataFromRegistry")
			result = False

		End Try

		Return result
	End Function

	' Die Lokale Daten Reseten...
	Private Function ResetRegistrySetting() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim result As Boolean = True
		Dim strPrinterPath As String
		Dim strAmtkennzahl As String
		Dim strDocPath As String
		Dim iStartTerminplaner As Short
		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Try
			' Serverpath
			SrvBinPath = AddDirSep(_ClsReg.GetRegKeyValue(strSaveRegKey & "\Path", "InitPath"))
			If SrvBinPath.Length > 1 Then _ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "InitPath", SrvBinPath)

			strPrinterPath = _ClsReg.GetRegKeyValue(strSaveRegKey & "\Path", "PrintFileSaveIn")
			If strPrinterPath.Trim.Length = 0 Then strPrinterPath = _ClsProgSetting.GetUserHomePath
			' Verzeichnis für Druckerdateien
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "PrintFileSaveIn", AddDirSep(strPrinterPath))
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "WordTempPathToSave", AddDirSep(strPrinterPath))

			' Amtskennziffer fürs Telefonieren...
			strAmtkennzahl = _ClsReg.GetRegKeyValue(strSaveRegKey & "\Options", "AmtsZiffer")
			If strAmtkennzahl.Length > 0 Then _ClsReg.SetRegKeyValue(RegStr_Net & "\Options", "AmtsZiffer", strAmtkennzahl)

			' Verzeichnis für Dokumente (Lokal!!!)
			strDocPath = _ClsReg.GetRegKeyValue(strSaveRegKey & "\Path", "TemplatePath")
			If strDocPath.Length > 1 Then _ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "TemplatePath", AddDirSep(strDocPath))

			' Terminliste anzeigen (Lokal!!!)
			iStartTerminplaner = Val(_ClsReg.GetRegKeyValue(strSaveRegKey & "\Options", "ShowAgendaOnStart"))
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options", "ShowAgendaOnStart", Str(iStartTerminplaner))

			' Updateprogramm starten(Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options", "ShowUpdateOnStart", "1")

			Return result


		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "ResetRegistrySetting")
			Return False

		End Try

	End Function

	Sub CreateRegistryWithNewSetting_1()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
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
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "InitPath", "")

			strPrinterPath = _ClsProgSetting.GetUserHomePath()
			' Verzeichnis für Druckerdateien
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "PrintFileSaveIn", strPrinterPath)

			' Amtskennziffer fürs Telefonieren...
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options", "AmtsZiffer", "")

			' Verzeichnis für Dokumente (Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "TemplatePath", "Templates\")
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "DocPath", "Documents\")

			' Terminliste anzeigen (Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options", "ShowAgendaOnStart", "0")

			' Updateprogramm starten(Lokal!!!)
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Options", "ShowUpdateOnStart", "1")

			' Optionen 
			_ClsReg.SetRegKeyValue(RegStr_Net & "\Path", "USLanguage", "")

		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "CreateRegistryWithNewSetting_1")

		End Try

	End Sub

	Sub WriteError(ByRef iErrNum As Short,
								 ByRef sDesc As String,
								 ByRef sSource As String,
								 ByRef SDate As String,
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

		Dim CnnStr As String = String.Empty
		Dim sSql As String = String.Empty
		Dim oldString As Boolean = False

		Try

			CnnStr = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "ConnStr_Net")
			If String.IsNullOrWhiteSpace(CnnStr) Then
				CnnStr = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "ConnStr")
				oldString = True
			End If
			m_Logger.LogDebug(String.Format("1. MDSection: {0}|CnnStr: {1}", MDSection, CnnStr))

			If oldString Then
				CnnStr = Mid(CnnStr, Len("Provider=SQLOLEDB.1;") + 1)
				_ClsReg.SetINIString(SrvSettingFullFileName, MDSection, "ConnStr_Net", CnnStr)
			End If
			If CnnStr.Length > 10 Then
				If MDSection <> "Mandant0000" Then _ClsReg.SetINIString(SrvSettingFullFileName, MDSection, "_Net", "1")
			End If


			m_Logger.LogDebug(String.Format("3. MDSection: {0}|CnnStr: {1}", MDSection, CnnStr))
			If Not String.IsNullOrWhiteSpace(CnnStr) Then
				ConnTemp = New SqlConnection(CnnStr)
				ConnTemp.Open()

			Else
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_translate.GetSafeTranslationValue("Für den Mandant {0} existieren keine Angaben in {1} zur Verbindung an Ihre Datenbank.{2}" & _
							 "Bitte kontaktieren Sie Ihren Systemadministrator."), MDSection, SrvSettingFullFileName, vbNewLine),
					 m_translate.GetSafeTranslationValue("Fehlende Datenbankverbindung"), MessageBoxButtons.OK, MessageBoxIcon.Error)

				Return Nothing
			End If

			If ConnTemp.State = ConnectionState.Closed Then
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_translate.GetSafeTranslationValue("Sie sind nicht berechtigt mit diesem Programm zu arbeiten.{0}" &
							 "Bitte kontaktieren Sie Ihrem Systemadministrator."), vbNewLine),
							 m_translate.GetSafeTranslationValue("Berechtigung nicht erteilt"), MessageBoxButtons.OK, MessageBoxIcon.Error)

				Return Nothing
			End If
			Return ConnTemp

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			DevExpress.XtraEditors.XtraMessageBox.Show(Err.Description, "GetDataBase", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return Nothing

		End Try

	End Function

	Function GetOSVersion() As String
		Dim strResult As String = My.Computer.Info.OSFullName

		Return strResult
	End Function


#Region "Funktionen zur Übersetzung der Daten..."

	'Public Function GetSafeTranslationValue(ByVal dicKey As String) As String

	'  Try
	'    If ClsPublicData.TranslationData.ContainsKey(dicKey) Then
	'      Return ClsPublicData.TranslationData.Item(dicKey).LogedUserLanguage

	'    Else
	'      Return dicKey

	'    End If

	'  Catch ex As Exception
	'    Return dicKey

	'  End Try

	'End Function

#End Region

	Function AddDirSep(ByVal strPathName As String) As String

		m_path = New ClsProgPath
		Return m_path.AddDirSep(strPathName)

	End Function

End Module


Public Class ClsWriteDataToXML

	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private doc As New Xml.XmlDocument
	Private xmlfile As String
	Private _ClsSetting As New ClsLogingMDData



#Region "Constructor"

	Public Sub New(ByVal _setting As ClsLogingMDData)
		Me._ClsSetting = _setting

		m_md = New Mandant
		m_utility = New Utilities
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_translate = New TranslateValues

	End Sub

#End Region



End Class
