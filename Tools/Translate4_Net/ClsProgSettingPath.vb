
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Public Class ClsProgSettingPath

#Region "Interne Funktionen"

  Dim _ClsReg As New ClsDivReg
  Dim strInitialString2Encrypt As String = "%3/NJA98ASJAG7S634JAHZH&\GHK21&74575m823w6467KS#J578iH4HS61648@567Qq635766A836=58)73425(JKJASDKJ?238ASuD852sKJKJASDJK5ADSAKJ34WKJASJKAW4598lKJSDnFKJs"

  ' Without Backslash!!!
  Public Function GetUserHomePath() As String
    Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
  End Function

  ''' <summary>
  ''' With Backslash "\"
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetPersonalFolder() As String
    Return _ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
  End Function

  ''' <summary>
  ''' Verzeichnis für Lokale Dateien vom Sputnik (Me.GetPersonalFolder + "Sputnik\")
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSHomePath() As String
    Dim strValue As String = Me.GetPersonalFolder & "Sputnik\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' ("Sputnik\Temp\")
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSTempPath() As String
    Dim strValue As String = Me.GetSpSHomePath & "Temp\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' Die Dateien in diesem Verzeichnis können jeder zeitputnkt gelöscht werden... 
  ''' ("Me.GetPersonalFolder + Sputnik\Allowed2Delete\")
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSFiles2DeletePath() As String
    Dim strValue As String = Me.GetSpSHomePath & "Allowed2Delete\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' Die Dateien in diesem Verzeichnis können jeder zeitputnkt gelöscht werden... 
  ''' ("Me.GetPersonalFolder + Sputnik\Allowed2Delete\Bilder\")
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSBildFiles2DeletePath() As String
    Dim strValue As String = Me.GetSpSFiles2DeletePath & "Bilder\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' Pfad für Lokale Drucker (P) Einstellungsdateien. Wenn nicht existiert dann wird der Pfad angelegt.
  ''' (Me.GetPersonalFolder +  "Sputnik\Printerfiles\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSPrinterPath() As String
    Dim strValue As String = Me.GetSpSHomePath & "Printerfiles\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\Kandidat\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSMATempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "Kandidat\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\Kunde\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSKDTempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "Kunde\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\Offer\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSOfferTempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "Offer\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\RP\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSRPTempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "RP\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\RE\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSRETempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "RE\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\LO\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSLOTempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "LO\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\NLA\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSNLATempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "NLA\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\ES\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSESTempPath() As String
    Dim strValue As String = Me.GetSpSTempPath & "ES\"
    If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    Return strValue
  End Function


  ''' <summary>
  ''' ist gemäss Einstellungen->lokale Einstellungen(PrintFileSaveIn)
  ''' Wenn leer ist dann nimmt den (Me.GetPersonalFolder +  "Sputnik\Printerfiles\)
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetPrinterPath() As String
    Dim strValue As String = _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", _
                                                "PrintFileSaveIn"))
    If strValue = String.Empty Then strValue = Me.GetSpSPrinterPath

    Return strValue
  End Function

  ''' <summary>
  ''' Liest aus der Registry die angemeldete Datenbank-Connectionstring aus
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetConnString() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String.Net")
  End Function

  Public Function GetConnString_() As String
    Return DecryptString128Bit(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String._Net"))
  End Function

  ''' <summary>
  ''' Liest aus der Registry die [Sputnik DbSelect] Datenbank-Connectionstring aus
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetDbSelectConnString() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "RootConnStr.Net")
  End Function

  ' Noch zu machen...
  Public Function GetOnlineConnString() As String
    Dim strMyConnection As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\OnlineDbConn", _
                                                          "Online Connection String.Net")
    Return strMyConnection
  End Function

  Public Function GetMDTemplatePath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath")) & _
          _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
  End Function

  Function GetInitPath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
  End Function

  Public Function GetMDIniFile() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath")) & "Programm.dat"
  End Function

  Public Function GetLocalBinnPath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", _
                                                    "ProgUpperPath")) & "Binn\"
  End Function

  Public Function GetLocalModulPath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", _
                                                    "ProgUpperPath")) & "Moduls\"
  End Function

  Function GetInitIniFile() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath")) & "Programm.dat"
  End Function

  Public Function GetMDPath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
  End Function

  Public Function GetMDMainPath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
  End Function

  Public Function GetMDDocPath() As String
    Return _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath")) & _
          _ClsReg.AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
  End Function

  Function GetFile4ZeuginisDeckblatt() As String
    Return GetMDProfilValue("Templates", "ZeugnisDeckblatt", "")
  End Function

  Function GetFile4AGBDeckblatt() As String
    Return GetMDProfilValue("Templates", "AGBDeckblatt", "")
  End Function

  Function GetFile4AGBTemp() As String
    Return GetMDProfilValue("Templates", "AGB4Temp", "")
  End Function

  Function GetFile4AGBFest() As String
    Return GetMDProfilValue("Templates", "AGB4Fest", "")
  End Function

  Function GetUSFiliale() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
  End Function

  Function GetUSKst() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USKSt")
  End Function

  Function GetSQLDateFormat() As String
    Dim strFormat As String = _ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
    If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"

    Return strFormat
  End Function

  Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
    Dim strValue As String = String.Empty

    If iVersion = 13 Then strValue = "BsB3EQ"
    If iVersion = 14 Then strValue = "NwOHEQ"

    If iVersion = 15 Then strValue = "40mWEQ"
    If iVersion = 16 Then strValue = "9qylEQ"
    If iVersion = 17 Then strValue = "Asm0EQ"

    Return strValue
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
    Return Me.GetUserFName & " " & GetUserLName()
  End Function

  Function GetUserNameWithComma() As String
    Return Me.GetUserLName & ", " & GetUserFName()
  End Function

  Function GetUserMail() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "spUsereMail")
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

  Function GetUSLanguage() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
  End Function

  Function GetSrvRootPath() As String
    Dim strServerInitpath As String = Me.GetInitPath()

    Return strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
  End Function

  Function GetUpdatePath() As String
    Return Me.GetSrvRootPath() & "Update\"
  End Function

  Function GetProgLOGPath() As String
    Dim strResult As String = Me.GetMDPath() & "LOGs\"
    If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)

    Return strResult
  End Function

  Function GetErrorLOGPath() As String
    Dim strResult As String = Me.GetProgLOGPath() & "Errors\"
    If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)

    Return strResult
  End Function

  Function GetProgLOGFile() As String
    Dim objAssInfo As New ClsAssInfo()
    Return String.Format("{0}PROG_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  Function GetErrorLOGFile() As String
    Dim objAssInfo As New ClsAssInfo()
    Return String.Format("{0}Err_{1}.txt", Me.GetErrorLOGPath(), objAssInfo.Product)
  End Function

  Function GetTranslationLOGFile() As String
    Dim objAssInfo As New ClsAssInfo()
    Return String.Format("{0}Translation_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  Function GetMainProgLOGFile() As String
    Dim objAssInfo As New ClsAssInfo()
    Return String.Format("{0}SP_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  Function GetProzessLOGFile() As String
    Dim objAssInfo As New ClsAssInfo()
    Return String.Format("{0}Proc_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  'Function GetMDGuid() As String
  '  Return (_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MD_Guid").ToString)
  'End Function

  Function GetSkinPath() As String
    Return _ClsReg.AddDirSep(GetMDTemplatePath()) & "Skins\"
  End Function

  Function GetMDImagesPath() As String
    Return _ClsReg.AddDirSep(GetMDTemplatePath()) & "images\"
  End Function

  Function GetFormDataFile() As String
    Return _ClsReg.AddDirSep(GetSkinPath()) & "FormData.XML"
  End Function

  Function GetTranslateDataFile() As String
    Return _ClsReg.AddDirSep(GetSkinPath()) & "TranslationData.XML"
  End Function

  Function GetSQLDataFile() As String
    Return _ClsReg.AddDirSep(GetSkinPath()) & "SelectData.XML"
  End Function

  Function GetMainViewSettingFile() As String
    Return _ClsReg.AddDirSep(GetSkinPath()) & "MainView.XML"
  End Function

  Function GetMDData_XMLFile() As String
    Return _ClsReg.AddDirSep(Me.GetMDPath()) & "Programm.XML"
  End Function

  Function GetMSGData_XMLFile() As String
    Return _ClsReg.AddDirSep(Me.GetInitPath()) & "MsgInfos.XML"
  End Function

  Function GetSelectedMDData() As List(Of String)
    Dim _ClsLog As New ClsEventLog
    Dim Conn As New SqlConnection(Me.GetConnString)
    Dim strResult As New List(Of String)

    Dim bCreateFile As Boolean = False
    Dim sOffDocSql As String = "Select Customer_ID, MD_Name1, MD_Name2, Strasse, PLZ, Ort, Land, "
    sOffDocSql &= "Telefon, Telefax, eMail From Mandanten Where Jahr = @MDJahr"

    Dim i As Integer = 0

    Conn.Open()
    Dim SQLCmd As SqlCommand = New SqlCommand(sOffDocSql, Conn)
    Dim param As System.Data.SqlClient.SqlParameter

    Try
      param = SQLCmd.Parameters.AddWithValue("@MDJahr", Now.Year)
      Dim rMDrec As SqlDataReader = SQLCmd.ExecuteReader
      While rMDrec.Read
        Try
          strResult.Add(CType(rMDrec("Customer_ID"), String))
          strResult.Add(CType(rMDrec("MD_Name1"), String))
          strResult.Add(CType(rMDrec("MD_Name2"), String))
          strResult.Add(CType(rMDrec("Strasse"), String))
          strResult.Add(CType(rMDrec("PLZ"), String))
          strResult.Add(CType(rMDrec("Ort"), String))
          strResult.Add(CType(rMDrec("Land"), String))
          strResult.Add(CType(rMDrec("Telefon"), String))
          strResult.Add(CType(rMDrec("Telefax"), String))
          strResult.Add(CType(rMDrec("eMail"), String))

        Catch ex As Exception
          _ClsLog.WriteTempLogFile(String.Format("***GetSelectedMDGuid_1: {0}", ex.Message))

        End Try

      End While
      rMDrec.Close()
      If strResult.Count = 0 Then strResult.Add("Kein Schlüssel vorhanden!!!")


    Catch ex As Exception
      _ClsLog.WriteTempLogFile(String.Format("***GetSelectedMDGuid_2: {0}", ex.Message))

    End Try

    Return strResult
  End Function

  Function GetMDGuid() As String
    Return (Me.GetSelectedMDData(0))
  End Function

  Function GetMDName() As List(Of String)
    Return (Me.GetSelectedMDData)
  End Function

#Region "WOS-Lizenzen..."

  Function GetMAWOSGuid() As String
    Return GetMDProfilValue("Export", "MA_SPUser_ID", "")
  End Function

  Function GetKDWOSGuid() As String
    Return GetMDProfilValue("Export", "KD_SPUser_ID", "")
  End Function

  Function GetVakWOSGuid() As String
    Return GetMDProfilValue("Export", "Vak_SPUser_ID", "")
  End Function


  Function bAllowedMADocTransferTo_WS() As Boolean
    Return Me.GetMAWOSGuid.ToString.Length > 31
  End Function

  Function bAllowedKDDocTransferTo_WS() As Boolean
    Return Me.GetKDWOSGuid.ToString.Length > 31
  End Function

  Function bAllowedVakDocTransferTo_WS() As Boolean
    Return Me.GetVakWOSGuid.ToString.Length > 31
  End Function

#End Region

  Function IsProcessRunning(ByVal sProcessName As String) As Boolean
    Return (System.Diagnostics.Process.GetProcessesByName(sProcessName).Length > 0)
  End Function

  ''' <summary>
  ''' überprüft ob ein Druckjob epxortieren darf. wird eher in den Listen- und Dokumenten-Druck abgefragt
  ''' </summary>
  ''' <param name="strJobNr"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function AllowedExportDoc(ByVal strJobNr As String) As Boolean
    Dim bResult As Boolean
    Dim _ClsReg As New ClsDivReg
    Dim strUserProfileName As String = Me.GetUserProfileFile()
    Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/Documents/DocName[@ID=" & _
                              Chr(34) & strJobNr & Chr(34) & "]/Export"

    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
    If strBez <> String.Empty Then
      If strBez = CStr(1) Then bResult = True
    End If

    Return bResult
  End Function

  ''' <summary>
  ''' sucht nach angegebenen Wert in der Benutzer.XML-Datei.
  ''' </summary>
  ''' <param name="strFieldName"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUSProfilValue(ByVal strFieldName As String) As String
    Dim bResult As String = String.Empty
    Dim strUserProfileName As String = Me.GetUserProfileFile()
    Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/USSetting/SettingName[@ID=" & Chr(34) & _
                              strFieldName & Chr(34) & "]/USValue"
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

    Return strBez
  End Function

  Function GetMDProfilValue(ByVal strSectionName As String, ByVal strFieldName As String, ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strMDProfileName As String = Me.GetMDData_XMLFile()
    Dim strQuery As String = "//{0}/{1}"

    strQuery = String.Format(strQuery, strSectionName, strFieldName)
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strMDProfileName, strQuery)
    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  Function GetCtlNameValue(ByVal strCtlName As String, ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strMDProfileName As String = Me.GetFormDataFile()
    '    Dim strQuery As String = "//Control[@Name=" & Chr(34) & strCtlName & Chr(34) & "]/CtlLabel" & Me.GetUSLanguage  
    Dim strQuery As String = "//Control[@Name={0}{1}{0}]/CtlLabel{2}" '& Me.GetUSLanguage

    strQuery = String.Format(strQuery, Chr(34), strCtlName, Me.GetUSLanguage)
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strMDProfileName, strQuery)
    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  Function GetMainViewXMLValue(ByVal strQuery As String, ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strMDProfileName As String = Me.GetMainViewSettingFile()
    'Dim strQuery As String = "//Modul[@ID={0}{1}{0}]/CtlLabel{2}"

    'strQuery = String.Format(strQuery, Chr(34), strCtlName, Me.GetUSLanguage)
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strMDProfileName, strQuery)
    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

    ''' <summary>
    ''' übersetzt ein(en) beliebiges(n) Wort/Satz via xml-Datei
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
  Function TranslateText(ByVal strText As String) As String
    Dim strResult As String = strText
    Dim strUSLang As String = Me.GetUSLanguage()
    Dim strFilename As String = Me.GetTranslateDataFile()
    Dim strLocalFilename As String = String.Format("{0}{1}", Me.GetSpSTempPath(), _
                                                   FileIO.FileSystem.GetName(Me.GetTranslateDataFile()))
    Try
      If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
      Dim strQuery As String = "//Control[@Name=" & Chr(34) & strText & Chr(34) & "]/CtlLabel" & strUSLang
      'strResult = _ClsReg.GetXMLNodeValue(strFilename, strQuery)
      strResult = _ClsReg.GetXMLNodeValue(strLocalFilename, strQuery)

      If strResult = String.Empty Then
        strResult = strText
      End If

    Catch ex As Exception
      Dim _clsEventlog As New ClsEventLog
      _clsEventlog.WriteToLogFile("*** Err: " & strText, String.Empty, "TranslateText", True, Me.GetTranslationLOGFile())

    End Try

    Return strResult
  End Function

  ''' <summary>
  ''' Übersetzt den selektierten Text in bestimmten Forms
  ''' </summary>
  ''' <param name="strText"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function TranslateText(ByVal strText As String, ByVal bWithLoging As Boolean) As String
    Dim strResult As String = strText
    Dim strUSLang As String = Me.GetUSLanguage()
    Dim strFilename As String = Me.GetTranslateDataFile()
    Dim strLocalFilename As String = String.Format("{0}{1}", _
                                                   Me.GetSpSTempPath(), _
                                                   FileIO.FileSystem.GetName(Me.GetTranslateDataFile()))

    Try
      If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
      Dim strQuery As String = "//Control[@Name=" & Chr(34) & strText & Chr(34) & "]/CtlLabel" & strUSLang
      'strResult = _ClsReg.GetXMLNodeValue(strFilename, strQuery)
      strResult = _ClsReg.GetXMLNodeValue(strLocalFilename, strQuery)

      If strResult = String.Empty Then
        strResult = strText
      End If
      If bWithLoging Then
        Dim _clsEventlog As New ClsEventLog
        _clsEventlog.WriteToLogFile(strText, String.Empty, "TranslateText", True, Me.GetTranslationLOGFile())
      End If

    Catch ex As Exception
      Dim _clsEventlog As New ClsEventLog
      _clsEventlog.WriteToLogFile("*** Err: " & strText, String.Empty, "TranslateText", True, Me.GetTranslationLOGFile())

    End Try

    Return strResult
  End Function

  ''' <summary>
  '''  beendet das ausgewählte Programm
  ''' </summary>
  ''' <param name="strProgFullname"></param>
  ''' <remarks></remarks>
  Sub TerminateSelectedProcess(ByVal strProgFullname As String)
    Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName(strProgFullname)

    For Each p As Process In pProcess
      p.Kill()
    Next

  End Sub

  ''' <summary>
  ''' gibt das Fullfilename für Benutzerprofile aus
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserProfileFile() As String
    Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
  End Function

  ''' <summary>
  ''' Seriennummer für topTapi3
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetTapi3SN() As String
    Return "20-038-888-28"
  End Function

  ''' <summary>
  ''' Lizenzkey für topTapi3
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetTapi3Key() As String
    Return "BLQNELWWUIZV"
  End Function
#End Region


#Region "Encryption und Decryption..."

  Function EncryptString(ByVal strText2Encrypt As String) As String
    Dim strValue As String = strText2Encrypt
    Dim rd As New RijndaelManaged
    Dim md5 As New MD5CryptoServiceProvider
    Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(strInitialString2Encrypt))

    md5.Clear()
    rd.Key = key
    rd.GenerateIV()

    Dim iv() As Byte = rd.IV
    Dim ms As New MemoryStream

    ms.Write(iv, 0, iv.Length)

    Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
    Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(strText2Encrypt)

    cs.Write(data, 0, data.Length)
    cs.FlushFinalBlock()

    Dim encdata() As Byte = ms.ToArray()
    strValue = Convert.ToBase64String(encdata)
    cs.Close()
    rd.Clear()
    strText2Encrypt = String.Empty

    Return strValue
  End Function

  Function DecryptString(ByVal strEncryptedText As String) As String
    Dim strValue As String = strEncryptedText
    Dim rd As New RijndaelManaged
    Dim rijndaelIvLength As Integer = 16
    Dim md5 As New MD5CryptoServiceProvider
    Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes(strInitialString2Encrypt))

    md5.Clear()

    Dim encdata() As Byte = Convert.FromBase64String(strValue)
    Dim ms As New MemoryStream(encdata)
    Dim iv(15) As Byte

    ms.Read(iv, 0, rijndaelIvLength)
    rd.IV = iv
    rd.Key = key

    Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)

    Dim data(CInt(ms.Length - rijndaelIvLength)) As Byte
    Dim i As Integer = cs.Read(data, 0, data.Length)

    strValue = System.Text.Encoding.UTF8.GetString(data, 0, i)
    cs.Close()
    rd.Clear()

    Return strValue
  End Function


  Function EncryptString128Bit(ByVal strString4Encryption As String) As String
    Dim vstrEncryptionKey As String = strInitialString2Encrypt
    Dim bytValue() As Byte
    Dim bytKey() As Byte
    Dim bytEncoded() As Byte = Nothing
    Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
    Dim intLength As Integer
    Dim intRemaining As Integer
    Dim objMemoryStream As New MemoryStream()
    Dim objCryptoStream As CryptoStream
    Dim objRijndaelManaged As RijndaelManaged


    '   **********************************************************************
    '   ******  Strip any null character from string to be encrypted    ******
    '   **********************************************************************

    strString4Encryption = StripNullCharacters(strString4Encryption)

    '   **********************************************************************
    '   ******  Value must be within ASCII range (i.e., no DBCS chars)  ******
    '   **********************************************************************

    bytValue = Encoding.ASCII.GetBytes(strString4Encryption.ToCharArray)
    intLength = Len(vstrEncryptionKey)

    '   ********************************************************************
    '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
    '   ******   If it is longer than 32 bytes it will be truncated.  ******
    '   ******   If it is shorter than 32 bytes it will be padded     ******
    '   ******   with upper-case Xs.                                  ****** 
    '   ********************************************************************

    If intLength >= 32 Then
      vstrEncryptionKey = Strings.Left(vstrEncryptionKey, 32)
    Else
      intLength = Len(vstrEncryptionKey)
      intRemaining = 32 - intLength
      vstrEncryptionKey = vstrEncryptionKey & Strings.StrDup(intRemaining, "X")
    End If

    bytKey = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray)
    objRijndaelManaged = New RijndaelManaged()

    '   ***********************************************************************
    '   ******  Create the encryptor and write value to it after it is   ******
    '   ******  converted into a byte array                              ******
    '   ***********************************************************************

    Try

      objCryptoStream = New CryptoStream(objMemoryStream, _
        objRijndaelManaged.CreateEncryptor(bytKey, bytIV), _
        CryptoStreamMode.Write)
      objCryptoStream.Write(bytValue, 0, bytValue.Length)

      objCryptoStream.FlushFinalBlock()

      bytEncoded = objMemoryStream.ToArray
      objMemoryStream.Close()
      objCryptoStream.Close()
    Catch

    End Try

    '   ***********************************************************************
    '   ******   Return encryptes value (converted from  byte Array to   ******
    '   ******   a base64 string).  Base64 is MIME encoding)             ******
    '   ***********************************************************************

    Return Convert.ToBase64String(bytEncoded)

  End Function

  Public Function DecryptString128Bit(ByVal strString4Decryption As String) As String
    Dim vstrDecryptionKey As String = strInitialString2Encrypt
    Dim bytDataToBeDecrypted() As Byte
    Dim bytTemp() As Byte
    Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
    Dim objRijndaelManaged As New RijndaelManaged()
    Dim objMemoryStream As MemoryStream
    Dim objCryptoStream As CryptoStream
    Dim bytDecryptionKey() As Byte

    Dim intLength As Integer
    Dim intRemaining As Integer
    'Dim intCtr As Integer
    Dim strReturnString As String = String.Empty
    'Dim achrCharacterArray() As Char
    'Dim intIndex As Integer

    '   *****************************************************************
    '   ******   Convert base64 encrypted value to byte array      ******
    '   *****************************************************************

    bytDataToBeDecrypted = Convert.FromBase64String(strString4Decryption)

    '   ********************************************************************
    '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
    '   ******   If it is longer than 32 bytes it will be truncated.  ******
    '   ******   If it is shorter than 32 bytes it will be padded     ******
    '   ******   with upper-case Xs.                                  ****** 
    '   ********************************************************************

    intLength = Len(vstrDecryptionKey)

    If intLength >= 32 Then
      vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32)
    Else
      intLength = Len(vstrDecryptionKey)
      intRemaining = 32 - intLength
      vstrDecryptionKey = vstrDecryptionKey & Strings.StrDup(intRemaining, "X")
    End If

    bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray)
    ReDim bytTemp(bytDataToBeDecrypted.Length)
    objMemoryStream = New MemoryStream(bytDataToBeDecrypted)

    '   ***********************************************************************
    '   ******  Create the decryptor and write value to it after it is   ******
    '   ******  converted into a byte array                              ******
    '   ***********************************************************************

    Try

      objCryptoStream = New CryptoStream(objMemoryStream, _
         objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), _
         CryptoStreamMode.Read)

      objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

      objCryptoStream.FlushFinalBlock()
      objMemoryStream.Close()
      objCryptoStream.Close()

    Catch

    End Try

    Return StripNullCharacters(Encoding.ASCII.GetString(bytTemp))
  End Function

  Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
    Dim intPosition As Integer
    Dim strStringWithOutNulls As String

    intPosition = 1
    strStringWithOutNulls = vstrStringWithNulls

    Do While intPosition > 0
      intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)

      If intPosition > 0 Then
        strStringWithOutNulls = Left$(strStringWithOutNulls, intPosition - 1) & _
                          Right$(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
      End If

      If intPosition > strStringWithOutNulls.Length Then
        Exit Do
      End If
    Loop

    Return strStringWithOutNulls
  End Function



#End Region


  Sub CreateSPSDirectories()

    Me.GetSpSTempPath()
    Me.GetSpSPrinterPath()
    Me.GetSpSFiles2DeletePath()
    Me.GetSpSBildFiles2DeletePath()

    Me.GetSpSMATempPath()
    Me.GetSpSKDTempPath()
    Me.GetSpSOfferTempPath()

    Me.GetSpSESTempPath()
    Me.GetSpSRPTempPath()
    Me.GetSpSRETempPath()

    Me.GetSpSLOTempPath()
    Me.GetSpSNLATempPath()

    Try
      Dim strFilename As String = Me.GetTranslateDataFile()
      Dim strLocalFilename As String = String.Format("{0}{1}", _
                                                     Me.GetSpSTempPath(), _
                                                     FileIO.FileSystem.GetName(Me.GetTranslateDataFile()))

      File.Copy(strFilename, strLocalFilename, True)

    Catch ex As Exception

    End Try

  End Sub

  Public Sub New()

  End Sub
End Class
