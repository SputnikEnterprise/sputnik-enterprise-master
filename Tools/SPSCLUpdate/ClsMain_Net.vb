
Imports System.IO
Imports System.IO.File

Public Class ClsMain_Net

	Dim ClsReg As New ClsDivReg

#Region "Interne Funktionen"

	Function GetAppGuidValue() As String
		Return "ADF910B5-E512-4792-9FA8-E9B4A2721CBA"
	End Function

	Function GetUserHomePath() As String
		Return ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
	End Function

	Public Function GetConnString() As String
		Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String.Net")
	End Function

	Public Function GetDbSelectConnString() As String
		Dim MDSection As String = "Mandant0000"
		Dim CnnStr As String = ClsReg.GetINIString(Me.GetInitIniFile(), MDSection, "ConnStr")
		If CnnStr.Contains("Provider=SQLOLEDB.1;") Then
			Dim strNewCnnStr As String = ClsReg.GetINIString(Me.GetInitIniFile(), MDSection, "ConnStr_Net")
			If strNewCnnStr = String.Empty Then
				CnnStr = Mid(CnnStr, Len("Provider=SQLOLEDB.1;") + 1)
			Else
				CnnStr = strNewCnnStr
			End If
		End If

		Return CnnStr 'ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "RootConnStr.Net")
	End Function

	Function GetUserID(ByVal strIDNr As String) As String
		Dim strResult As String = String.Empty
		Dim srRead As System.IO.StreamReader
		Dim strSettingUrl As String = "http://www.domain.com/sputnik/update/inern/error_1.php"

		Try
			' make a Web request
			Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(strSettingUrl)
			Dim resp As System.Net.WebResponse = req.GetResponse
			Dim Str As System.IO.Stream = resp.GetResponseStream
			srRead = New System.IO.StreamReader(Str, System.Text.Encoding.Default)
			' read all the text 
			Dim strFileContent As String = srRead.ReadToEnd

			'  Close Stream and StreamReader when done
			srRead.Close()
			Str.Close()

			Dim aMyString As String() = Split(strFileContent, vbCrLf)
			Dim i As Integer
			For Each strline As String In aMyString
				i += i
				If strline.Contains(strIDNr) Then
					Dim strMyCode As String() = Split(strline, strIDNr)
					Trace.WriteLine(i & ": " & strline)
					strResult = strMyCode(1).ToString
				End If
			Next


		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "SetUserID")

		End Try

		Return strResult
	End Function

	''' <summary>
	''' \\Server\spenterprise$
	''' </summary>
	''' <returns></returns>
	Function GetSrvRootPath() As String
		'Dim strServerInitpath As String = GetInitPath()
		Return My.Settings.SPSEnterpriseFolder 'strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
	End Function

	''' <summary>
	''' \\Server\spenterprise$\bin
	''' </summary>
	''' <returns></returns>
	Function GetInitPath() As String
		Return Path.Combine(GetSrvRootPath, "Bin") 'ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
	End Function

	''' <summary>
	''' \\Server\spenterprise$\bin\ProgUpdatesetting.sps
	''' </summary>
	''' <returns></returns>
	Function GetUpdateListFile() As String
		Return Path.Combine(My.Settings.SPSEnterpriseFolder, "Bin\ProgUpdatesetting.sps") 'ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
	End Function

	''' <summary>
	''' \\Server\spenterprise$\bin\Programm.dat
	''' </summary>
	''' <returns></returns>
	Function GetInitIniFile() As String
		Return Path.Combine(GetInitPath, "Programm.dat")
	End Function

	''' <summary>
	''' \\Server\spenterprise$\Update
	''' </summary>
	''' <returns></returns>
	Function GetSrvUpdatePath() As String
		Return Path.Combine(GetSrvRootPath, "Update")
	End Function

	''' <summary>
	''' \\Server\spenterprise$\Update\Binn\Services
	''' </summary>
	''' <returns></returns>
	Function GetUpdateServicePath() As String
		Return Path.Combine(Me.GetSrvUpdatePath, "Binn\Services")
	End Function

	Function GetLocalPath() As String
		Dim progPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
		Dim sputnikPath As String = Path.Combine(progPath, "Sputnik Enterprise Suite")
		If Not Directory.Exists(sputnikPath) Then
			'Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "@SPCLUpdate")
		End If
		Dim strLocalpath As String = If(String.IsNullOrWhiteSpace(My.Settings.ProgUpperPath), sputnikPath, My.Settings.ProgUpperPath)
		Return strLocalpath
	End Function

	Function GetLocalServicePath() As String
		Return Path.Combine(GetLocalPath, "Binn\Services")
	End Function

	'Public Function GetMDIniFile() As String
	'	Return Me.GetMDPath() & "Programm.dat"
	'End Function

	'Public Function GetMDPath() As String
	'	Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
	'End Function

	'Public Function GetMDMainPath() As String
	'	Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
	'End Function

	'Public Function GetMDTemplatePath() As String
	'	Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
	'End Function

	'Function GetSkinPath() As String
	'	Return ClsReg.AddDirSep(GetMDTemplatePath()) & "Skins\"
	'End Function

	'Function GetFormDataFile() As String
	'	Return ClsReg.AddDirSep(GetSkinPath()) & "FormData.XML"
	'End Function

	'Function GetSQLDataFile() As String
	'	Return ClsReg.AddDirSep(GetSkinPath()) & "SelectData.XML"
	'End Function

	'Function GetMDData_XMLFile() As String
	'	Return ClsReg.AddDirSep(Me.GetMDPath()) & "Programm.XML"
	'End Function

	'Function GetMSGData_XMLFile() As String
	'	Return ClsReg.AddDirSep(Me.GetInitPath()) & "MsgInfos.XML"
	'End Function

	'Public Function GetMDDocPath() As String
	'	Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
	'End Function

	'Function GetUSFiliale() As String
	'	Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
	'End Function

	'Function GetUSLanguage() As String
	'	Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
	'End Function

	'Function GetSQLDateFormat() As String
	'	Dim strFormat As String = ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
	'	If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
	'	strFormat = strFormat.Replace("mm", "MM")

	'	Return strFormat
	'End Function


	'Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
	'	Return CStr(IIf(iVersion = 13, "BsB3EQ", "NwOHEQ"))
	'End Function



	'Public Function GetSmtpServer() As String
	'	Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
	'End Function

	'Public Function GetFaxServer() As String
	'	Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
	'End Function

	'Public Function GetDavidServer() As String
	'	Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
	'End Function

	'Function GetMDNr() As String
	'	Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
	'End Function

	Function GetUserName() As String
		Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname") & " " &
					ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
	End Function

	Function GetUserFName() As String
		Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
	End Function

	Function GetUserLName() As String
		Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
	End Function

	Function GetLogedUSNr() As Integer
		Return CInt(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
	End Function

	'Function GetUserProfileFile() As String
	'	Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
	'End Function

	'Function GetFirstDayOfMonth(ByVal DateTime As Date) As Date
	'	Return CDate("1." & Month(DateTime) & "." & Year(DateTime))
	'End Function

	'Function GetLastDayOfMonth(ByVal DateTime As Date) As Date
	'	Return DateAdd(DateInterval.Day, -1, CDate("01." & CStr(DateTime.Month + 1) & "." & CStr(DateTime.Year.ToString)))
	'End Function

	'Public Function GetMDDataFromXML(ByVal strFieldName As String) As String
	'	Dim _ClsReg As New ClsDivReg
	'	Dim strUserProfileName As String = Me.GetMDData_XMLFile()
	'	Dim strQuery As String = "//MD_" & Me.GetMDNr & "/Sonstiges/strFieldName" 'AusgNummer"
	'	Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

	'	Return strBez
	'End Function

	'Public Function GetMessageFromXML(ByVal strMSGID As String) As String
	'	Dim _ClsReg As New ClsDivReg
	'	Dim strUserProfileName As String = Me.GetMSGData_XMLFile()
	'	Dim strQuery As String = "//Messages/MSGID/" & strMSGID
	'	Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

	'	Return strBez
	'End Function

	'Function AllowedExportDoc(ByVal strJobNr As String) As Boolean
	'	Dim bResult As Boolean
	'	Dim _ClsReg As New ClsDivReg
	'	Dim strUserProfileName As String = Me.GetUserProfileFile()
	'	Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/Documents/DocName[@ID=" & Chr(34) & strJobNr & Chr(34) & "]/Export"
	'	'strQuery = "//Control[@Name=" & Chr(34) & "BeraterIn" & Chr(34) & "]/CtlLabel"


	'	Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
	'	If strBez <> String.Empty Then
	'		If strBez = CStr(1) Then bResult = True
	'	End If

	'	Return bResult
	'End Function

#End Region

#Region "Setzen von Controls-Eigenschaften..."

	'Public Function GetCmdFlatStyle() As System.Windows.Forms.FlatStyle
	'	Dim FlatValue As System.Windows.Forms.FlatStyle
	'	Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "CmdFlatStyle").ToString))
	'	Select Case ivalue
	'		Case 0
	'			FlatValue = FlatStyle.System
	'		Case 1
	'			FlatValue = FlatStyle.Flat
	'		Case 2
	'			FlatValue = FlatStyle.Popup
	'		Case 3
	'			FlatValue = FlatStyle.Standard

	'		Case Else
	'			FlatValue = FlatStyle.System
	'	End Select

	'	Return FlatValue
	'End Function

	'Public Function GetDropDownFlatStyle() As System.Windows.Forms.FlatStyle
	'	Dim FlatValue As System.Windows.Forms.FlatStyle
	'	Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "DropDownFlatStyle").ToString))
	'	Select Case ivalue
	'		Case 0
	'			FlatValue = FlatStyle.System
	'		Case 1
	'			FlatValue = FlatStyle.Flat
	'		Case 2
	'			FlatValue = FlatStyle.Popup
	'		Case 3
	'			FlatValue = FlatStyle.Standard

	'		Case Else
	'			FlatValue = FlatStyle.System
	'	End Select

	'	Return FlatValue
	'End Function

	'Public Function GetTextBStyle() As System.Windows.Forms.BorderStyle
	'	Dim FlatValue As System.Windows.Forms.BorderStyle
	'	Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "TextBorderStyle").ToString))

	'	Select Case ivalue
	'		Case 0
	'			FlatValue = BorderStyle.Fixed3D
	'		Case 1
	'			FlatValue = BorderStyle.FixedSingle
	'		Case 2
	'			FlatValue = BorderStyle.None

	'		Case Else
	'			FlatValue = BorderStyle.Fixed3D
	'	End Select

	'	Return FlatValue
	'End Function

#End Region

#Region "Startfunktionen..."

	Sub ShowfrmUpdate()
		Dim frmTest As New frmMain

		frmTest.Show()

	End Sub


	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	Public Sub New()

		Application.EnableVisualStyles()

	End Sub

#End Region

End Class
