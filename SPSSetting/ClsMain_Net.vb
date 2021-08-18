
Imports System.IO.File

Public Class ClsMain_Net
  Dim ClsReg As New SPProgUtility.ClsDivReg

  Public Sub New()
    Application.EnableVisualStyles()
  End Sub


#Region "Interne Funktionen"

  'Function GetPersonalFolder() As String
  '  Return ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
  'End Function

  'Public Function GetUserHomePath() As String
  '  Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
  'End Function

  'Public Function GetConnString() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String.Net")
  'End Function

  'Public Function GetDbSelectConnString() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "RootConnStr.Net")
  'End Function

  'Function GetInitPath() As String
  '  Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
  'End Function

  'Public Function GetMDIniFile() As String
  '  Return Me.GetMDPath() & "Programm.dat"
  'End Function

  'Public Function GetMDPath() As String
  '  Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
  'End Function

  'Public Function GetMDMainPath() As String
  '  Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
  'End Function

  'Public Function GetMDTemplatePath() As String
  '  Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
  'End Function

  'Function GetUSProfilePath() As String
  '  Return ClsReg.AddDirSep(Me.GetMDMainPath() & "Profiles")
  'End Function

  'Function GetSkinPath() As String
  '  Return ClsReg.AddDirSep(GetMDTemplatePath()) & "Skins\"
  'End Function

  'Function GetFormDataFile() As String
  '  Return ClsReg.AddDirSep(GetSkinPath()) & "FormData.XML"
  'End Function

  'Function GetSQLDataFile() As String
  '  Return ClsReg.AddDirSep(GetSkinPath()) & "SelectData.XML"
  'End Function

  'Function GetMDData_XMLFile() As String
  '  Return ClsReg.AddDirSep(Me.GetMDPath()) & "Programm.XML"
  'End Function

  'Function GetMSGData_XMLFile() As String
  '  Return ClsReg.AddDirSep(Me.GetInitPath()) & "MsgInfos.XML"
  'End Function

  'Function GetUSFormControlData_XMLFile() As String
  '  Return ClsReg.AddDirSep(Me.GetUSProfilePath()) & "UserFormTemplate_" & Me.GetLogedUSNr & ".XML"
  'End Function

  'Public Function GetMDDocPath() As String
  '  Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
  'End Function

  'Function GetUSFiliale() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
  'End Function

  'Function GetUSLanguage() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
  'End Function

  'Function GetSQLDateFormat() As String
  '  Dim strFormat As String = ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
  '  If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
  '  strFormat = strFormat.Replace("mm", "MM")

  '  Return strFormat
  'End Function


  'Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
  '  Return CStr(IIf(iVersion = 13, "BsB3EQ", "NwOHEQ"))
  'End Function



  'Public Function GetSmtpServer() As String
  '  Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
  'End Function

  'Public Function GetFaxServer() As String
  '  Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
  'End Function

  'Public Function GetDavidServer() As String
  '  Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
  'End Function

  'Function GetMDNr() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
  'End Function

  'Function GetUserName() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname") & " " & _
  '        ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
  'End Function

  'Function GetUserFName() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
  'End Function

  'Function GetUserLName() As String
  '  Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
  'End Function

  'Function GetLogedUSNr() As Integer
  '  Return CInt(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
  'End Function

  'Function GetUserProfileFile() As String
  '  Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
  'End Function

  'Function AllowedExportDoc(ByVal strDocName As String) As Boolean
  '  Return CBool(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
  'End Function

  'Function GetSrvRootPath() As String
  '  Dim strServerInitpath As String = Me.GetInitPath()

  '  Return strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
  'End Function

  'Function GetInitIniFile() As String
  '  Return Me.GetInitPath() & "Programm.dat"
  'End Function

  'Function GetUpdatePath() As String
  '  Return Me.GetSrvRootPath() & "Update\"
  'End Function

  'Function GetErrorLOGPath() As String
  '  Dim strResult As String = Me.GetInitPath() & "Errors\"

  '  If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)

  '  Return strResult
  'End Function

  'Function GetProgLOGPath() As String
  '  Dim strResult As String = Me.GetMDPath() & "LOGs\"

  '  If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)

  '  Return strResult
  'End Function

  'Function GetProgLOGFile() As String
  '  Dim objAssInfo As New ClsAssInfo()
  '  Return Me.GetProgLOGPath() & objAssInfo.Product & ".txt"
  'End Function

  'Function GetErrorLOGFile() As String
  '  Dim objAssInfo As New ClsAssInfo()
  '  Return Me.GetErrorLOGPath() & objAssInfo.Product & ".txt"
  'End Function

  'Function GetMainProgLOGFile() As String
  '  ' Die Datei für Benutzung vom Sputnik-Module
  '  Return Me.GetProgLOGPath() & "_Sputnik_.txt"
  'End Function

#End Region

#Region "Sonstige Funktionen..."

  'Public Function GetCmdFlatStyle() As System.Windows.Forms.FlatStyle
  '  Dim FlatValue As System.Windows.Forms.FlatStyle
  '  Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "CmdFlatStyle").ToString))
  '  Select Case ivalue
  '    Case 0
  '      FlatValue = FlatStyle.System
  '    Case 1
  '      FlatValue = FlatStyle.Flat
  '    Case 2
  '      FlatValue = FlatStyle.Popup
  '    Case 3
  '      FlatValue = FlatStyle.Standard

  '    Case Else
  '      FlatValue = FlatStyle.System
  '  End Select

  '  Return FlatValue
  'End Function

  'Public Function GetDropDownFlatStyle() As System.Windows.Forms.FlatStyle
  '  Dim FlatValue As System.Windows.Forms.FlatStyle
  '  Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "DropDownFlatStyle").ToString))
  '  Select Case ivalue
  '    Case 0
  '      FlatValue = FlatStyle.System
  '    Case 1
  '      FlatValue = FlatStyle.Flat
  '    Case 2
  '      FlatValue = FlatStyle.Popup
  '    Case 3
  '      FlatValue = FlatStyle.Standard

  '    Case Else
  '      FlatValue = FlatStyle.System
  '  End Select

  '  Return FlatValue
  'End Function

  'Public Function GetTextBStyle() As System.Windows.Forms.BorderStyle
  '  Dim FlatValue As System.Windows.Forms.BorderStyle
  '  Dim ivalue As Integer = CInt(Val(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms", "TextBorderStyle").ToString))

  '  Select Case ivalue
  '    Case 0
  '      FlatValue = BorderStyle.Fixed3D
  '    Case 1
  '      FlatValue = BorderStyle.FixedSingle
  '    Case 2
  '      FlatValue = BorderStyle.None

  '    Case Else
  '      FlatValue = BorderStyle.Fixed3D
  '  End Select

  '  Return FlatValue
  'End Function

#End Region

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

End Class
