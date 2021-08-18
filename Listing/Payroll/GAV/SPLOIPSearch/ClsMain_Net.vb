
'Imports System.IO.File

'Public Class ClsMain_Net
'  Public Shared frmTest As frmLOIPSearch
'  Dim _ClsReg As New SPProgUtility.ClsDivReg

'  '#Region "Interne Funktionen"

'  '  Function GetPersonalFolder() As String
'  '    Return _ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
'  '  End Function

'  '  Public Function GetUserHomePath() As String
'  '    Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
'  '  End Function

'  '  Public Function GetConnString() As String
'  '    Dim strMyConnection As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", _
'  '                                                          "Connection String.Net")
'  '    If Not strMyConnection.ToUpper.Contains("MultipleActiveResultSets=") Then
'  '      strMyConnection &= ";MultipleActiveResultSets="
'  '      Dim strQuery As String = "//SPLOIPSearch/SPLOIPSearch/DiffSetting[@ID=" & Chr(34) & Me.GetAppGuidValue() & Chr(34) & "]/MARS"

'  '      Dim strBez As String = _ClsReg.GetXMLNodeValue(Me.GetSQLDataFile(), strQuery)
'  '      If strBez <> String.Empty Then
'  '        strMyConnection &= strBez

'  '      Else
'  '        strMyConnection &= "True"
'  '      End If
'  '    End If

'  '    If Not strMyConnection.ToUpper.Contains("Pooling=") Then
'  '      strMyConnection &= ";Pooling="
'  '      Dim strQuery As String = "//SPLOIPSearch/SPLOIPSearch/DiffSetting[@ID=" & Chr(34) & Me.GetAppGuidValue() & Chr(34) & "]/Pooling"

'  '      Dim strBez As String = _ClsReg.GetXMLNodeValue(Me.GetSQLDataFile(), strQuery)
'  '      If strBez <> String.Empty Then
'  '        strMyConnection &= strBez

'  '      Else
'  '        strMyConnection &= "true"
'  '      End If
'  '    End If

'  '    Return strMyConnection
'  '  End Function

'  '  Public Function GetDbSelectConnString() As String
'  '    Dim MDSection As String = "Mandant0000"
'  '    Dim _clsEventlog As New ClsEventLog
'  '    Dim CnnStr As String = String.Empty

'  '    CnnStr = _ClsReg.GetINIString(Me.GetInitIniFile(), MDSection, "ConnStr")
'  '    '_clsEventlog.WriteToEventLog(Now.ToString & vbTab & "GetDbSelectConnString_0: " & CnnStr)

'  '    If CnnStr.ToUpper.Contains("Provider=SQLOLEDB.1;".ToUpper) Then
'  '      Dim strNewCnnStr As String = _ClsReg.GetINIString(Me.GetInitIniFile(), MDSection, "ConnStr_Net")
'  '      '_clsEventlog.WriteToEventLog(Now.ToString & vbTab & "GetDbSelectConnString_1: " & strNewCnnStr)
'  '      If strNewCnnStr.Trim = String.Empty Then
'  '        CnnStr = Mid(CnnStr, Len("Provider=SQLOLEDB.1;") + 1)
'  '      Else
'  '        CnnStr = strNewCnnStr
'  '      End If
'  '    End If
'  '    '_clsEventlog.WriteToEventLog(Now.ToString & vbTab & "GetDbSelectConnString_2: " & vbCrLf & CnnStr)

'  '    Return CnnStr
'  '  End Function

'  '  Function GetInitPath() As String
'  '    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
'  '  End Function

'  '  Function GetSrvRootPath() As String
'  '    Dim strServerInitpath As String = Me.GetInitPath()

'  '    Return strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
'  '  End Function

'  '  Function GetInitIniFile() As String
'  '    Return Me.GetInitPath() & "Programm.dat"
'  '  End Function

'  '  Function GetUpdatePath() As String
'  '    Return Me.GetSrvRootPath() & "Update\"
'  '  End Function

'  '  Function GetErrorLOGPath() As String
'  '    Dim strResult As String = Me.GetInitPath() & "Errors\"

'  '    If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)

'  '    Return strResult
'  '  End Function

'  '  Function GetProgLOGPath() As String
'  '    Dim strResult As String = Me.GetMDPath() & "LOGs\"

'  '    If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)

'  '    Return strResult
'  '  End Function

'  '  Function GetProgLOGFile() As String
'  '    Dim objAssInfo As New ClsAssInfo()
'  '    Return Me.GetProgLOGPath() & objAssInfo.Product & ".txt"
'  '  End Function

'  '  Function GetErrorLOGFile() As String
'  '    Dim objAssInfo As New ClsAssInfo()
'  '    Return Me.GetErrorLOGPath() & objAssInfo.Product & ".txt"
'  '  End Function

'  '  Function GetMainProgLOGFile() As String
'  '    ' Die Datei für Benutzung vom Sputnik-Module
'  '    Return Me.GetProgLOGPath() & "_Sputnik_.txt"
'  '  End Function

'  '  Function GetLocalPath() As String
'  '    Dim strLocalpath As String = _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath"))

'  '    Return strLocalpath
'  '  End Function

'  '  Function GetLocalInitPath() As String
'  '    Dim strLocalInitpath As String = Me.GetLocalPath()
'  '    strLocalInitpath &= "Binn\"
'  '    Return strLocalInitpath
'  '  End Function

'  '  Function GetLocalServicePath() As String
'  '    Dim strLocalpath As String = Me.GetLocalInitPath()
'  '    strLocalpath &= "Services\"

'  '    Return strLocalpath
'  '  End Function

'  '  Public Function GetMDIniFile() As String
'  '    Return Me.GetMDPath() & "Programm.dat"
'  '  End Function

'  '  Public Function GetMDPath() As String
'  '    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
'  '  End Function

'  '  Public Function GetMDMainPath() As String
'  '    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
'  '  End Function

'  '  Public Function GetMDTemplatePath() As String
'  '    Return Me.GetMDMainPath() & _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
'  '  End Function

'  '  Function GetSkinPath() As String
'  '    Return _ClsReg.AddDirSep(GetMDTemplatePath()) & "Skins\"
'  '  End Function

'  '  Function GetFormDataFile() As String
'  '    Return _ClsReg.AddDirSep(GetSkinPath()) & "FormData.XML"
'  '  End Function

'  '  Function GetSQLDataFile() As String
'  '    Return _ClsReg.AddDirSep(GetSkinPath()) & "SelectData.XML"
'  '  End Function

'  '  Function GetMDData_XMLFile() As String
'  '    Return _ClsReg.AddDirSep(Me.GetMDPath()) & "Programm.XML"
'  '  End Function

'  '  Function GetMSGData_XMLFile() As String
'  '    Return _ClsReg.AddDirSep(Me.GetInitPath()) & "MsgInfos.XML"
'  '  End Function

'  '  Public Function GetMDDocPath() As String
'  '    Return Me.GetMDMainPath() & _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
'  '  End Function

'  '  Function GetUSFiliale() As String
'  '    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
'  '  End Function

'  '  Function GetUSLanguage() As String
'  '    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
'  '  End Function

'  '  Function GetSQLDateFormat() As String
'  '    Dim strFormat As String = _ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
'  '    If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
'  '    strFormat = strFormat.Replace("mm", "MM")

'  '    Return strFormat
'  '  End Function

'  '  Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
'  '    Return CStr(IIf(iVersion = 15, "40mWEQ", "NwOHEQ"))
'  '  End Function

'  '  Public Function GetSmtpServer() As String
'  '    Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
'  '  End Function

'  '  Public Function GetFaxServer() As String
'  '    Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
'  '  End Function

'  '  Public Function GetDavidServer() As String
'  '    Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
'  '  End Function

'  '  Function GetMDNr() As String
'  '    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
'  '  End Function

'  '  Function GetUserName() As String
'  '    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname") & " " & _
'  '          _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
'  '  End Function

'  '  Function GetUserFName() As String
'  '    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
'  '  End Function

'  '  Function GetUserLName() As String
'  '    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
'  '  End Function

'  '  Function GetLogedUSNr() As Integer
'  '    Return CInt(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
'  '  End Function

'  '  Function GetUserProfileFile() As String
'  '    Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
'  '  End Function

'  '  Function AllowedExportDoc(ByVal strJobNr As String) As Boolean
'  '    Dim bResult As Boolean
'  '    Dim _ClsReg As New SPProgUtility.ClsDivReg
'  '    Dim strUserProfileName As String = Me.GetUserProfileFile()
'  '    Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/Documents/DocName[@ID=" & Chr(34) & strJobNr & Chr(34) & "]/Export"
'  '    'strQuery = "//Control[@Name=" & Chr(34) & "BeraterIn" & Chr(34) & "]/CtlLabel"


'  '    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
'  '    If strBez <> String.Empty Then
'  '      If strBez = CStr(1) Then bResult = True
'  '    End If

'  '    Return bResult
'  '  End Function


'  '#End Region


'  Protected Overrides Sub Finalize()
'    MyBase.Finalize()
'  End Sub

'  Public Sub New()

'    Application.EnableVisualStyles()

'  End Sub

'  Sub ShowfrmLOIPSearch()
'    Dim _ClsELog As New SPProgUtility.ClsEventLog

'    _ClsELog.WriteMainLog("ShowfrmLOIPSearch")

'    frmTest = New frmLOIPSearch
'    frmTest.Show()

'  End Sub

'End Class
