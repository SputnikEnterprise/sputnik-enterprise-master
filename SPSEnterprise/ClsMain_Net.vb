
Imports SPProgUtility.MainUtilities
Imports System.IO.File
Imports System.IO

Public Class ClsMain_Net

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private m_util As New Utilities

	Sub New()
		Dim strRegCheck As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite", "Prog_Version")

		Dim strValue As String = Environment.CurrentDirectory
		If strValue.ToUpper.EndsWith("Binn".ToUpper) Then strValue = Mid(strValue, 1, Len(strValue) - 5)
		Dim strRegFullfilname As String = AddDirSep(strValue) & "sps_0.reg"

		If System.IO.File.Exists(strRegFullfilname) Or strRegCheck = String.Empty Then
			Me.CreateRegistryWithNewSetting()
		End If

	End Sub

#Region "Interne Funktionen"

	Function GetPrinterPath() As String
		Dim strValue As String = AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "PrintFileSaveIn"))
		If String.IsNullOrWhiteSpace(strValue) OrElse strValue.ToUpper = "%userprofile%".ToUpper Then
			strValue = Me.GetUserHomePath
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "PrintFileSaveIn", strValue)
		End If

		Return strValue
	End Function

	Private Sub CreateRegistryWithNewSetting()
		Dim _clsReg As New ClsDivReg

		Dim strValue As String = Environment.CurrentDirectory
		If strValue.ToUpper.EndsWith("Binn".ToUpper) Then strValue = Mid(strValue, 1, Len(strValue) - 5)
		Dim strRegFullfilname As String = AddDirSep(strValue) & "sps_0.reg"
		strValue = strRegFullfilname

		Try

			If System.IO.File.Exists(strValue) Then

				strValue = """" & strValue & """"
				strValue = "/s " & strValue

				Dim startInfo As New ProcessStartInfo("regedit.exe")
				startInfo.Arguments = strValue
				startInfo.UseShellExecute = False

				Process.Start(startInfo)

				Exit Sub

			End If

		Catch ex As Exception
			MsgBox(ex.Message & vbCrLf & ex.GetBaseException.ToString)
		End Try


		Try
			_clsReg.SetRegKeyValue(RegStr_Net, "Prog_Version", "000-000.000")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Coordination", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\LoadedForms", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\LVG-Property", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\SearchFields", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Tabs", "_Standard", "0")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "BackColor", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Blau1", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Blau2", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Blau3", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Blau4", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "BorderColor", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "ClickColor", "0")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\GotFocusNormalColor", "BackColor", "16777215")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "GotFocusPflichtColor", "16770790")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Grün1", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Grün2", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Grün3", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Grün4", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "LostFocusNormalColor", "16777215")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "LightBorderColor", "8421504")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "LostFocusPflichtColor", "16770790")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "LostFocusNormalColor", "16777215")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "MouseoverColor", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Rot1", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Rot2", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Rot3", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Rot4", "0")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "ShowXPNoSmart", "1")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "WindowsXPButtons", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "UseColorInMainForm", "1")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color1", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color2", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color3", "32896")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color4", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color5", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color6", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color7", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Forms\Colour", "Color8", "0")


			_clsReg.SetRegKeyValue(RegStr_Net & "\Options\DbForList", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Options\TapiDevice", "_Standard", "0")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Options", "LastModulSelL", "Kandidatenverwaltung")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Options", "TempCheckAnz", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Options", "Sprache", "Deutsch")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Options", "TempRPAnz", "5")

			_clsReg.SetRegKeyValue(RegStr_Net & "\Options", "SaveDbGridColWidth", "1")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Options\DbSelections", "Kandidatenverwaltung", "Show_MA_Data")

			_clsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "_Standard", "0")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Save", "_Standard", "0")

			' Serverpath
			_clsReg.SetRegKeyValue(RegStr_Net & "\Path", My.Resources.str272, "")

			' Verzeichnis für Druckerdateien
			_clsReg.SetRegKeyValue(RegStr_Net & "\Path", "PrintFileSaveIn", Me.GetUserHomePath())

			' Verzeichnis für Dokumente (Lokal!!!)
			_clsReg.SetRegKeyValue(RegStr_Net & "\Path", "TemplatePath", "Templates\")
			_clsReg.SetRegKeyValue(RegStr_Net & "\Path", "DocPath", "Documents\")

			' Terminliste anzeigen (Lokal!!!)
			_clsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str218, "0")

			' Updateprogramm starten(Lokal!!!)
			_clsReg.SetRegKeyValue(RegStr_Net & My.Resources.str251, My.Resources.str219, "1")

			strValue = Environment.CurrentDirectory
			If strValue.ToUpper.EndsWith("Binn".ToUpper) Then strValue = Mid(strValue, 1, Len(strValue) - 5)
			strValue = AddDirSep(strValue)
			_clsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath", strValue)
			_clsReg.SetRegKeyValue(RegStr_Net & "\ProgOptions", "USLanguage", "")

		Catch ex As Exception
			MsgBox(Err.Description, MsgBoxStyle.Critical, "CreateRegistryWithNewSetting")

		End Try

	End Sub

	Public Function GetUserHomePath() As String
		Return AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
	End Function

	Public Function GetConnString() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String.Net")
	End Function

	Public Function GetDbSelectConnString() As String
		Dim MDSection As String = "Mandant0000"
		Dim CnnStr As String = ""
		CnnStr = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "ConnStr")
		If CnnStr.Contains("Provider=SQLOLEDB.1;") Then
			Dim strNewCnnStr As String = _ClsReg.GetINIString(SrvSettingFullFileName, MDSection, "ConnStr_Net")
			If strNewCnnStr = String.Empty Then
				CnnStr = Mid(CnnStr, Len("Provider=SQLOLEDB.1;") + 1)
			Else
				CnnStr = strNewCnnStr
			End If
		End If

		Return CnnStr 'ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "RootConnStr.Net")
	End Function

	Function GetInitPath() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath")
	End Function

	Function GetSrvRootPath() As String
		Dim strServerInitpath As String = GetInitPath()
		Return strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
	End Function

	Function GetInitIniFile() As String
		Return GetInitPath() & "Programm.dat"
	End Function

	Function GetTranslationFullFilename() As String
		Return GetInitPath() & "Translationdata.xml"
	End Function

	Function GetUpdatePath() As String
		Return GetSrvRootPath() & "Update\"
	End Function

	Function GetErrorPath() As String
		Return GetInitPath() & "Errors\"
	End Function

	Function GetLocalPath() As String
		Dim p As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
		Dim localPath As String = New Uri(p).LocalPath
		localPath = Directory.GetParent(localPath).FullName

		'Dim localPath As String = AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath"))

		'If String.IsNullOrWhiteSpace(localPath) OrElse Not Directory.Exists(localPath) Then
		'	Dim p As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
		'	localPath = New Uri(p).LocalPath ' Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).FullName

		'	localPath = Directory.GetParent(localPath).FullName
		'End If

		Return localPath
	End Function

	Function GetLocalInitPath() As String
		Dim p As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
		Dim localINIPath As String = New Uri(p).LocalPath


		'Dim localINIPath As String = Me.GetLocalPath()
		'localINIPath = Path.Combine(localINIPath, "Binn\")

		'If String.IsNullOrWhiteSpace(localINIPath) OrElse Not Directory.Exists(localINIPath) Then
		'	Dim p As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
		'	localINIPath = New Uri(p).LocalPath

		'	localINIPath = Directory.GetParent(localINIPath).FullName
		'End If

		Return localINIPath
	End Function


	Public Function GetMDPath() As String
		Return AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
	End Function

	Public Function GetMDIniFile() As String
		Return Path.Combine(Me.GetMDPath(), "Programm.dat")
	End Function

	Public Function GetMDMainPath() As String
		Return AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
	End Function

	Public Function GetMDTemplatePath() As String
		Dim value As String = Path.Combine(Me.GetMDMainPath(), AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath")))
		If Not value.EndsWith("\") Then value &= "\"

		Return value

	End Function

	Public Function GetMDDocPath() As String
		Dim value As String = Path.Combine(Me.GetMDMainPath(), AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath")))
		If Not value.EndsWith("\") Then value &= "\"

		Return value

	End Function


	Function GetUserProfileFile() As String
		Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
	End Function

	Function GetSkinPath() As String
		Return Me.GetMDTemplatePath() & "Skins\"
	End Function

	Function GetFormDataFile() As String
		Return Me.GetSkinPath() & "FormData.XML"
	End Function

	Function GetSQLDataFile() As String
		Return Me.GetSkinPath() & "SelectData.XML"
	End Function

	Function GetMDData_XMLFile() As String
		Return Me.GetMDPath() & "Programm.XML"
	End Function

	Function GetMSGData_XMLFile() As String
		Return Me.GetInitPath() & "MsgInfos.XML"
	End Function


	Function GetUSFiliale() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
	End Function

	Function GetUSLanguage() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
	End Function

	Function GetSQLDateFormat() As String
		Dim strFormat As String = _ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
		If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
		strFormat = strFormat.Replace("mm", "MM")

		Return strFormat
	End Function



	Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
		Return CStr(IIf(iVersion = 13, "BsB3EQ", "NwOHEQ"))
	End Function

	Public Function GetSmtpServer() As String
		Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
	End Function

	Public Function GetFaxServer() As String
		Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
	End Function

	Public Function GetDavidServer() As String
		Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
	End Function

	Function GetMDNr() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
	End Function

	Function GetUserName() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname") & " " &
					_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
	End Function

	Function GetUserFName() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
	End Function

	Function GetUserLName() As String
		Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
	End Function

	Function GetLogedUSNr() As Integer
		Return CInt(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
	End Function

#End Region


#Region "XML-Funktionen..."

	Public Function GetMDDataFromXML(ByVal strFieldName As String) As String
		Dim _ClsReg As New ClsDivReg
		Dim strUserProfileName As String = Me.GetMDData_XMLFile()
		Dim strQuery As String = "//MD_" & Me.GetMDNr & "/Sonstiges/strFieldName" 'AusgNummer"
		Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

		Return strBez
	End Function

	Public Function GetMessageFromXML(ByVal strMSGID As String) As String
		Dim _ClsReg As New ClsDivReg
		Dim strUserProfileName As String = Me.GetMSGData_XMLFile()
		Dim strQuery As String = "//Messages/MSGID/" & strMSGID
		Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

		Return strBez
	End Function

	Function AllowedExportDoc(ByVal strJobNr As String) As Boolean
		Dim bResult As Boolean
		Dim _ClsReg As New ClsDivReg
		Dim strUserProfileName As String = Me.GetUserProfileFile()
		Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/Documents/DocName[@ID=" & Chr(34) & strJobNr & Chr(34) & "]/Export"
		'strQuery = "//Control[@Name=" & Chr(34) & "BeraterIn" & Chr(34) & "]/CtlLabel"


		Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
		If strBez <> String.Empty Then
			If strBez = CStr(1) Then bResult = True
		End If

		Return bResult
	End Function

#End Region


#Region "Control-Layouts..."

	Public Function GetCmdFlatStyle() As System.Windows.Forms.FlatStyle
		Dim FlatValue As System.Windows.Forms.FlatStyle
		Dim ivalue As Integer = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "CmdFlatStyle").ToString))
		Select Case ivalue
			Case 0
				FlatValue = FlatStyle.System
			Case 1
				FlatValue = FlatStyle.Flat
			Case 2
				FlatValue = FlatStyle.Popup
			Case 3
				FlatValue = FlatStyle.Standard

			Case Else
				FlatValue = FlatStyle.System
		End Select

		Return FlatValue
	End Function

	Public Function GetDropDownFlatStyle() As System.Windows.Forms.FlatStyle
		Dim FlatValue As System.Windows.Forms.FlatStyle
		Dim ivalue As Integer = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "DropDownFlatStyle").ToString))
		Select Case ivalue
			Case 0
				FlatValue = FlatStyle.System
			Case 1
				FlatValue = FlatStyle.Flat
			Case 2
				FlatValue = FlatStyle.Popup
			Case 3
				FlatValue = FlatStyle.Standard

			Case Else
				FlatValue = FlatStyle.System
		End Select

		Return FlatValue
	End Function

	Public Function GetTextBStyle() As System.Windows.Forms.BorderStyle
		Dim FlatValue As System.Windows.Forms.BorderStyle
		Dim ivalue As Integer = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "TextBorderStyle").ToString))

		Select Case ivalue
			Case 0
				FlatValue = BorderStyle.Fixed3D
			Case 1
				FlatValue = BorderStyle.FixedSingle
			Case 2
				FlatValue = BorderStyle.None

			Case Else
				FlatValue = BorderStyle.Fixed3D
		End Select

		Return FlatValue
	End Function

#End Region

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

End Class
