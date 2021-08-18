
'Imports System.IO.File

'Public Class ClsMain_Net

'  Dim ClsReg As New SPProgUtility.ClsDivReg



'#Region "Interne Funktionen"

'  '  Function GetPersonalFolder() As String
'  '    Return ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
'  '  End Function

'  '  Public Function GetUserHomePath() As String
'  '    Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
'  '  End Function

'  '  Public Function GetConnString() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String.Net")
'  '  End Function

'  '  Public Function GetDbSelectConnString() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "RootConnStr.Net")
'  '  End Function

'  '  Function GetInitIniFile() As String
'  '    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath")) & "Programm.dat"
'  '  End Function

'  '  Function GetInitPath() As String
'  '    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
'  '  End Function

'  '  Public Function GetMDIniFile() As String
'  '    Return Me.GetMDPath() & "Programm.dat"
'  '  End Function

'  '  Public Function GetMDPath() As String
'  '    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
'  '  End Function

'  '  Public Function GetMDMainPath() As String
'  '    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
'  '  End Function

'  '  Public Function GetMDTemplatePath() As String
'  '    Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
'  '  End Function

'  '  Function GetSkinPath() As String
'  '    Return ClsReg.AddDirSep(GetMDTemplatePath()) & "Skins\"
'  '  End Function

'  '  Function GetFormDataFile() As String
'  '    Return ClsReg.AddDirSep(GetSkinPath()) & "FormData.XML"
'  '  End Function

'  '  Function GetSQLDataFile() As String
'  '    Return ClsReg.AddDirSep(GetSkinPath()) & "SelectData.XML"
'  '  End Function

'  '  Public Function GetMDDocPath() As String
'  '    Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
'  '  End Function

'  '  Function GetUSFiliale() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
'  '  End Function

'  '  Function GetUSLanguage() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
'  '  End Function

'  '  Function GetSQLDateFormat() As String
'  '    Dim strFormat As String = ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
'  '    If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
'  '    strFormat = strFormat.Replace("mm", "MM")

'  '    Return strFormat
'  '  End Function


'  '  Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
'  '    Return "Asm0EQ"
'  '  End Function



'  '  Public Function GetSmtpServer() As String
'  '    Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
'  '  End Function

'  '  Public Function GetFaxServer() As String
'  '    Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
'  '  End Function

'  '  Public Function GetDavidServer() As String
'  '    Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
'  '  End Function

'  '  Function GetMDNr() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
'  '  End Function

'  '  Function GetUserName() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname") & " " & _
'  '          ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
'  '  End Function

'  '  Function GetUserFName() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
'  '  End Function

'  '  Function GetUserLName() As String
'  '    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
'  '  End Function

'  '  Function GetLogedUSNr() As Integer
'  '    Return CInt(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
'  '  End Function

'  '  Function GetUserProfileFile() As String
'  '    Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
'  '  End Function

'  '  Function AllowedExportDoc(ByVal strJobNr As String) As Boolean
'  '    Dim bResult As Boolean
'  '    Dim _ClsReg As New ClsDivReg
'  '    Dim strUserProfileName As String = Me.GetUserProfileFile()
'  '    Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/Documents/DocName[@ID=" & Chr(34) & strJobNr & Chr(34) & "]/Export"
'  '    'strQuery = "//Control[@Name=" & Chr(34) & "BeraterIn" & Chr(34) & "]/CtlLabel"


'  '    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
'  '    If strBez <> String.Empty Then
'  '      If strBez = CStr(1) Then bResult = True
'  '    End If

'  '    Return bResult
'  '  End Function

'#End Region

'#Region "Setzen von Controls-Eigenschaften..."

'  Public Function GetTextBStyle() As System.Windows.Forms.BorderStyle
'    Dim FlatValue As System.Windows.Forms.BorderStyle
'    Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "TextBorderStyle").ToString))

'    Select Case ivalue
'      Case 0
'        FlatValue = BorderStyle.Fixed3D
'      Case 1
'        FlatValue = BorderStyle.FixedSingle
'      Case 2
'        FlatValue = BorderStyle.None

'      Case Else
'        FlatValue = BorderStyle.Fixed3D
'    End Select

'    Return FlatValue
'  End Function

'#End Region

'#Region "Startfunktionen..."

'  Sub ShowfrmZGSearch()
'    Dim frmTest As New frmZGSearch
'    Dim _ClsDivFunc As New ClsDivFunc

'    '_ClsDivFunc.GetBetragSign()
'    frmTest.Show()

'  End Sub


'  Protected Overrides Sub Finalize()
'    MyBase.Finalize()
'  End Sub

'  Public Sub New(ByVal _setting As ClsSetting)

'    ClsDataDetail.ProgSettingData = _setting

'		ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(ClsDataDetail.m_InitialData.MDData.MDNr)
'		ClsDataDetail.UserData = ClsDataDetail.LogededUSData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.ProgSettingData.LogedUSNr)
'		If _setting.SelectedMDNr = 0 Then ClsDataDetail.m_InitialData.MDData.MDNr = ClsDataDetail.m_InitialData.MDData.MDNr
'		ClsDataDetail.m_InitialData.MDData.MDDbConn = ClsDataDetail.MDData.MDDbConn

'    If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.m_InitialData.UserData.UserNr

'    If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
'      ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
'      ClsDataDetail.TranslationData = ClsDataDetail.Translation
'    Else
'      ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
'    End If

'    Application.EnableVisualStyles()

'  End Sub

'#End Region

'End Class
