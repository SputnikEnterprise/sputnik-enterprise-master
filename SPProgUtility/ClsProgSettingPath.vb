
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports NLog

Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Public Class ClsProgSettingPath
  Inherits Utilities

  Private m_Progpath As ClsProgPath
  Private m_CommonSetting As New CommonSetting
  Private m_md As New Mandant

  Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

#Region "Interne Funktionen"

  Dim _ClsReg As New ClsDivReg

	ReadOnly Property GetPDFVW_O2SSerial() As String
    Get
      Return m_CommonSetting.GetO2SolutionData.PDFVWSerialnumber

      'Return "yourlicencekey"
    End Get
  End Property

  ReadOnly Property GetPDF_O2SSerial() As String
    Get
      Return m_CommonSetting.GetO2SolutionData.PDFSerialnumber

      'Return "yourlicencekey"
    End Get
  End Property


#Region "Funktionen zur Öffnen der Datenbanken..."

	''' <summary>
	''' Liest aus der Registry die angemeldete Datenbank-Connectionstring aus [Sputnik ...]
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function GetConnString() As String
		If m_md.GetSelectedMDData(0) Is Nothing Then Return String.Empty

		Return m_md.GetSelectedMDData(0).MDDbConn
	End Function

	''' <summary>
	''' Liest aus der Registry die [Sputnik DbSelect] Datenbank-Connectionstring aus
	''' Bitte nicht mehr benutzen!!!
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function GetDbSelectConnString() As String
		Return m_Progpath.GetDbSelectData.MDDbConn
	End Function

	''' Bitte nicht mehr benutzen!!!
	Public Function GetOnlineConnString() As String
    Dim strMyConnection As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\OnlineDbConn", _
                                                          "Online Connection String.Net")
    Return strMyConnection
  End Function

  ''' <summary>
  ''' Listet aus der Registry die [Sputnik ScanJobs] Datenbank-Connectionstring aus
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetConnString4ScanJobs() As String
    Return m_Progpath.GetScanDbData.ScanDbConn

    'Return _ClsReg.GetINIString(Me.GetInitIniFile, "ScanDb", "ConnStr_Net", "")
    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", _
    '                              "Connection String ScanJobs.Net")
  End Function


#End Region


#Region "Verzeichnis-Struktur..."

#Region "Lokale Verzeichnisse ermitteln..."

  ' Without Backslash!!!
  ''' Bitte nicht mehr benutzen!!!
  Public Function GetUserHomePath() As String
    Return GetMyDocumentsPath

    'Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
  End Function

  ''' <summary>
  ''' Benutzerverzeichnis "\"
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetPersonalFolder() As String
    Return GetMyDocumentsPathWithBackSlash

    'Return AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
  End Function

  ''' <summary>
  ''' Verzeichnis für Lokale Dateien vom Sputnik (Me.GetPersonalFolder + "Sputnik\")
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSHomePath() As String
    Return GetSpSHomeFolder

    'Dim strValue As String = Me.GetPersonalFolder & "Sputnik\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' ("Sputnik\Temp\")
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSTempPath() As String
    Return GetSpSTempFolder

    'Dim strValue As String = Me.GetSpSHomePath & "Temp\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' Die Dateien in diesem Verzeichnis können jeder zeitputnkt gelöscht werden... 
  ''' ("Me.GetPersonalFolder + Sputnik\Allowed2Delete\")
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSFiles2DeletePath() As String
    Return GetSpS2DeleteHomeFolder

    'Dim strValue As String = Me.GetSpSHomePath & "Allowed2Delete\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' Die Dateien in diesem Verzeichnis können jeder zeitputnkt gelöscht werden... 
  ''' ("Me.GetPersonalFolder + Sputnik\Allowed2Delete\Bilder\")
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSBildFiles2DeletePath() As String
    Return GetSpSPictureHomeFolder

    'Dim strValue As String = Me.GetSpSFiles2DeletePath & "Bilder\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' Pfad für Lokale Drucker (P) Einstellungsdateien. Wenn nicht existiert dann wird der Pfad angelegt.
  ''' (Me.GetPersonalFolder +  "Sputnik\Printerfiles\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSPrinterPath() As String
    Return GetSpSPrinterHomeFolder

    'Dim strValue As String = Me.GetSpSHomePath & "Printerfiles\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\Kandidat\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSMATempPath() As String
    Return GetSpSMAHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "Kandidat\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\Kunde\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSKDTempPath() As String
    Return GetSpSKDHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "Kunde\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\Offer\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSOfferTempPath() As String
    Return GetSpSOfferHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "Offer\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\RP\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSRPTempPath() As String
    Return GetSpSRPHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "RP\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\RE\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSRETempPath() As String
    Return GetSpSREHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "RE\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\LO\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSLOTempPath() As String
    Return GetSpSLOHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "LO\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\NLA\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSNLATempPath() As String
    Return GetSpSNLAHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "NLA\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' (Me.GetPersonalFolder +  "Sputnik\Temp\ES\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetSpSESTempPath() As String
    Return GetSpSESHomeFolder

    'Dim strValue As String = Me.GetSpSTempPath & "ES\"
    'If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

    'Return strValue
  End Function

  ''' <summary>
  ''' ist gemäss Einstellungen->lokale Einstellungen(PrintFileSaveIn)
  ''' Wenn leer ist dann nimmt den (Me.GetPersonalFolder +  "Sputnik\Printerfiles\)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetPrinterPath() As String
    Return GetPrinterHomeFolder

    'Dim strValue As String = AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", _
    '                                            "PrintFileSaveIn"))
    'If strValue = String.Empty Then strValue = Me.GetSpSPrinterPath

    'Return strValue
  End Function


#End Region


  ''' <summary>
  ''' Server - Vorlagenverzeichnis des Mandanten: \\Server\spenterprise$\MDxx\Jahr\Templates\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetMDTemplatePath() As String
    Return m_md.GetSelectedMDTemplatePath(0)
  End Function

  ''' <summary>
  ''' Server - Programm.dat für Datenbankeinstellungen: \\Server\spenterprise$\Bin\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetInitPath() As String
    Return m_Progpath.GetFileServerBinPath
  End Function

  ''' <summary>
  ''' Server - Einstellungsdatei für Mandanten inklusiv Jahr: \\Server\spenterprise$\MDxx\Jahr\Programm.dat
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetMDIniFile() As String
    Return String.Format("{0}{1}", Me.GetMDPath, "Programm.dat")
  End Function

  ''' <summary>
  ''' Lokal - Verzeichnis für lokale Sputnikinstallation: C:\Programme\Sputnik Enterprise Suite\Binn\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetLocalBinnPath() As String
    Return AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", _
                                                    "ProgUpperPath")) & "Binn\"
  End Function

  ''' <summary>
  '''  Lokal - Verzeichnis für lokale Sputnik-Installation: C:\Programme\Sputnik Enterprise Suite\Moduls\
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetLocalModulPath() As String
    Return AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", _
                                                    "ProgUpperPath")) & "Moduls\"
  End Function

  ''' <summary>
  ''' Server - Programm.dat für Datenbankeinstellungen: \\Server\spenterprise$\Bin\Programm.dat
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetInitIniFile() As String
    Return AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath")) & "Programm.dat"
  End Function

  ''' <summary>
  ''' Server - Mandanten-Jahresverzeichnis: \\Server\spenterprise$\MDxx\Jahr\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetMDPath() As String
    Return m_md.GetSelectedMDYearPath(0, Now.Year)
  End Function

  ''' <summary>
  ''' Server - Haupt-Mandantenverzeichnis : \\Server\spenterprise$\MDxx\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetMDMainPath() As String
    Return m_md.GetSelectedMDData(0).MDMainPath

    'Return AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
  End Function

  ''' <summary>
  ''' Server - Dokumentenverzeichnis von Mandanten : \\Server\spenterprise$\MDxx\Documents\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetMDDocPath() As String
    Return m_md.GetSelectedMDDocPath(0)

    'Return String.Format("{0}{1}", Me.GetMDMainPath, AddDirSep(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath")))
  End Function

  ''' <summary>
  ''' Server - Hauptverzeichnis auf dem Server : \\Server\spenterprise$\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetSrvRootPath() As String
    Return m_Progpath.GetFileServerPath

    'Dim strServerInitpath As String = Me.GetInitPath()
    'Return strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
  End Function

  ''' <summary>
  ''' Server - Verzeichnis für Update : \\Server\spenterprise$\Update\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUpdatePath() As String
    Return m_Progpath.GetFileServerUpdatePath

    'Return Me.GetSrvRootPath() & "Update\"
  End Function

  ''' <summary>
  ''' Server - Verzeichnis für LOG-Dateien in der Mandantenverzeichnis: \\Server\spenterprise$\Protokoll\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetProgLOGPath() As String
    Return m_Progpath.GetFileServerProtokollPath

    'Dim strResult As String = Me.GetMDPath() & "LOGs\"
    'If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)
    'Return strResult
  End Function

  ''' <summary>
  ''' Server - Errorverzeichnis in Mandantenverzeichnis : \\Server\spenterprise$\Protokoll\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetErrorLOGPath() As String
    Return m_Progpath.GetFileServerProtokollPath

    'Dim strResult As String = String.Format("{0}{1}\", Me.GetProgLOGPath(), "Errors")
    'If Not System.IO.Directory.Exists(strResult) Then System.IO.Directory.CreateDirectory(strResult)
    'Return strResult
  End Function

  ''' <summary>
  ''' Server - Prog-Logfile : \\Server\spenterprise$\Protokoll\SPSLogs.txt
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetProgLOGFile() As String
    Return m_Progpath.GetFileServerLogFilename

    'Dim objAssInfo As New ClsAssInfo()
    'Return String.Format("{0}PROG_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  ''' <summary>
  ''' Server - Log-File : \\Server\spenterprise$\Protokoll\SPSErrorLogs.txt
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetErrorLOGFile() As String
    Return m_Progpath.GetFileServerErrorFilename

    'Dim objAssInfo As New ClsAssInfo()
    'Return String.Format("{0}Err_{1}.txt", Me.GetErrorLOGPath(), objAssInfo.Product)
  End Function

  ''' <summary>
  ''' Server - Logfile : \\Server\spenterprise$\MDxx\Jahr\LOGs\{0}Translation_{1}.txt
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetTranslationLOGFile() As String
    Return m_Progpath.GetNewTranslationLOGFilename

    'Dim objAssInfo As New ClsAssInfo()
    'Return String.Format("{0}Translation_Values.txt", Me.GetProgLOGPath())
  End Function

  ''' <summary>
  ''' Server - Logfile : \\Server\spenterprise$\Protokoll\SPSLogs.txt
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetMainProgLOGFile() As String
    Return Me.GetProgLOGFile

    'Dim objAssInfo As New ClsAssInfo()
    'Return String.Format("{0}SP_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  ''' <summary>
  ''' Server - Logfile : \\Server\spenterprise$\Protokoll\SPSLogs.txt
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetProzessLOGFile() As String
    Return Me.GetProgLOGFile

    'Dim objAssInfo As New ClsAssInfo()
    'Return String.Format("{0}Proc_{1}.txt", Me.GetProgLOGPath(), objAssInfo.Product)
  End Function

  ''' <summary>
  ''' Server - Skin-Verzeichnis in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Templates\Skins\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetSkinPath() As String
    Return m_md.GetSelectedMDSkinPath(0)

    'Return String.Format("{0}{1}\", Me.GetMDTemplatePath, "Skins")
  End Function

  ''' <summary>
  ''' Server - Image-Verzeichnis in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Templates\Images\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetMDImagesPath() As String
    Return m_md.GetSelectedMDImagePath(0)

    'Return String.Format("{0}{1}\", Me.GetMDTemplatePath, "images")
  End Function

  ''' <summary>
  ''' Server - FormData.XML Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Templates\Skins\FormData.xml
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetFormDataFile() As String
    Return m_md.GetSelectedMDFormDataXMLFilename(0)

    'Return String.Format("{0}{1}", Me.GetSkinPath, "FormData.XML")
  End Function

  ''' <summary>
  ''' Server - TranslationData.XML Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Templates\Skins\TranslationData.xml
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetTranslateDataFile() As String
    Return m_Progpath.GetNewTranslationFilename
  End Function

  ''' <summary>
  ''' Server - SelectData.XML Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Templates\Skins\SelectData.xml
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetSQLDataFile() As String
    Return m_md.GetSelectedMDSQLDataXMLFilename(0)

    'Return String.Format("{0}{1}", Me.GetSkinPath, "SelectData.XML")
  End Function

  ''' <summary>
  ''' Server - Haupübersicht - MainView.XML Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Templates\Skins\MainView.xml
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetMainViewSettingFile() As String
    Return m_md.GetSelectedMDMainViewXMLFilename(0)

    'Return String.Format("{0}{1}", Me.GetSkinPath, "MainView.XML")
  End Function

  ''' <summary>
  ''' Server - Programm.XML Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Jahr\Programm.xml
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetMDData_XMLFile() As String
    Return m_md.GetSelectedMDDataXMLFilename(0, Now.Year)

    'Return String.Format("{0}{1}", Me.GetMDPath(), "Programm.XML")
  End Function

  ''' <summary>
  ''' Server - MsgInfos.XML Datei: \\Server\spenterprise$\Bin\MsgInfos.xml
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetMSGData_XMLFile() As String
    Return String.Format("{0}{1}", Me.GetInitPath(), "MsgInfos.XML")
  End Function

  ''' <summary>
  ''' Server - UserProfile Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Profiles\
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserProfilePath() As String
    Return m_md.GetSelectedMDUserProfilePath(0)

    'Return String.Format("{0}Profiles\", Me.GetMDMainPath())
  End Function

  ''' <summary>
  ''' Server - UserProfile{UserNr}.XML Datei in Mandantenverzeichnis : \\Server\spenterprise$\MDxx\Profiles\UserProfile{UserNr}.XML
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserProfileFile() As String
    Return m_md.GetSelectedMDUserProfileXMLFilename(0, Me.GetLogedUSNr)

    'Return String.Format("{0}UserProfile{1}.XML", Me.GetUserProfilePath(), Me.GetLogedUSNr())
  End Function


#End Region



  ''' Bitte nicht mehr benutzen!!!
  Function GetFile4ZeuginisDeckblatt() As String
    Return m_md.GetPath4Templates(0, Now.Year).Path4ZeugnisDeckblatt

    'Return GetMDProfilValue("Templates", "ZeugnisDeckblatt", "")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetFile4AGBDeckblatt() As String
    Return m_md.GetPath4Templates(0, Now.Year).Path4AGBDeckblatt

    'Return GetMDProfilValue("Templates", "AGBDeckblatt", "")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetFile4AGBTemp() As String
    Return m_md.GetPath4Templates(0, Now.Year).Path4AGBTemp

    'Return GetMDProfilValue("Templates", "AGB4Temp", "")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetFile4AGBFest() As String
    Return m_md.GetPath4Templates(0, Now.Year).Path4AGBFest

    'Return GetMDProfilValue("Templates", "AGB4Fest", "")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetUSFiliale() As String
    Return m_CommonSetting.GetLogedUserFiliale

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetUSKst() As String
    Return m_CommonSetting.GetLogedUserKst

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USKSt")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetSQLDateFormat() As String
    Return m_Progpath.GetSQLDateFormat

    'Dim strFormat As String = _ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
    'If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
    'Return strFormat
  End Function


	''' Bitte nicht mehr benutzen!!!
	Public Function GetSmtpServer() As String
    Return m_md.GetMailingData(0, Now.Year).SMTPServer

    'Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Public Function GetFaxServer() As String
    Return m_md.GetMailingData(0, Now.Year).FaxServer

    'Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Public Function GetDavidServer() As String
    Return m_md.GetMailingData(0, Now.Year).FaxDavidServer

    'Return _ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetMDNr() As String
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
  End Function

  ''' <summary>
  ''' Benutzervorname und Nachname
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserName() As String
    Return m_CommonSetting.GetLogedUserName

    'Return String.Format("{0} {1}", Me.GetUserFName, GetUserLName())
  End Function

  ''' <summary>
  ''' Benutzernachname + , + Benutzervorname
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserNameWithComma() As String
    Return m_CommonSetting.GetLogedUserNameWithComma

    'Return String.Format("{0}, {1}", Me.GetUserLName, Me.GetUserFName)
  End Function

  ''' <summary>
  ''' Benutzer EMail-Adresse aus der Registry
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserMail() As String
    Return m_CommonSetting.GetLogedUserMail

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "spUsereMail")
  End Function

  ''' <summary>
  ''' Benutzervorname aus der Registry
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserFName() As String
    Return m_CommonSetting.GetLogedUserFirstName

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
  End Function

  ''' <summary>
  ''' Benutzernachname aus der Registry
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUserLName() As String
    Return m_CommonSetting.GetLogedUserLastName

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
  End Function

  ''' <summary>
  ''' Angemeldete Benutzernummer aus der Registry
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetLogedUSNr() As Integer
    Return m_CommonSetting.GetLogedUserNr

    'Return CInt(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
  End Function

  ''' <summary>
  ''' Die Guid-Nummer der jeweiligen Benutzer. Wird aus Registry gelesen ("\Sputnik Suite\ProgOptions", "MyGuid")
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetLogedUSGuid() As String
    Return m_CommonSetting.GetLogedUserGuid

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MyGuid").ToString
  End Function

  ''' <summary>
  ''' Benutzer Sprache aus der Registry
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetUSLanguage() As String
    Return m_CommonSetting.GetLogedUserLanguage

    'Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
  End Function

  ''' <summary>
  ''' Mandantendaten in Form einer List (of String)
  ''' 0 = CType(rMDrec("Customer_ID"), String)
  ''' 1 = CType(rMDrec("MD_Name1"), String)
  ''' 2 = CType(rMDrec("MD_Name2"), String)
  ''' 3 = CType(rMDrec("Strasse"), String)
  ''' 4 = CType(rMDrec("PLZ"), String)
  ''' 5 = CType(rMDrec("Ort"), String)
  ''' 6 = CType(rMDrec("Land"), String)
  ''' 7 = CType(rMDrec("Telefon"), String)
  ''' 8 = CType(rMDrec("Telefax"), String)
  ''' 9 = CType(rMDrec("EMail"), String)
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetSelectedMDData() As List(Of String)
    Return m_md.GetSelectedMDDataInList(0)

    ''Dim _ClsLog As New ClsEventLog
    ''Dim Conn As New SqlConnection(Me.GetConnString)
    'Dim result As New List(Of String)

    ''Dim bCreateFile As Boolean = False
    ''Dim sOffDocSql As String = "Select Customer_ID, MD_Name1, MD_Name2, Strasse, PLZ, Ort, Land, "
    ''sOffDocSql &= "Telefon, Telefax, eMail From Mandanten Where Jahr = @MDJahr"

    ''Dim i As Integer = 0

    ''Conn.Open()
    ''Dim SQLCmd As SqlCommand = New SqlCommand(sOffDocSql, Conn)
    ''Dim param As System.Data.SqlClient.SqlParameter
    'Dim MDData = GetMDData4SelectedMD(0, Now.Year)

    'Try
    '  'param = SQLCmd.Parameters.AddWithValue("@MDJahr", Now.Year)
    '  'Dim rMDrec As SqlDataReader = SQLCmd.ExecuteReader
    '  'While rMDrec.Read
    '  Try
    '    result.Add(MDData.MDGuid)
    '    result.Add(MDData.MDName)
    '    result.Add(MDData.MDName_2)
    '    result.Add(MDData.MDStreet)
    '    result.Add(MDData.MDPostcode)
    '    result.Add(MDData.MDCity)
    '    result.Add(MDData.MDCountry)
    '    result.Add(MDData.MDTelefon)
    '    result.Add(MDData.MDTelefax)
    '    result.Add(MDData.MDeMail)
    '    result.Add(MDData.MDGuid)


    '    'strResult.Add(CType(rMDrec("Customer_ID"), String))
    '    'strResult.Add(CType(rMDrec("MD_Name1"), String))
    '    'strResult.Add(CType(rMDrec("MD_Name2"), String))
    '    'strResult.Add(CType(rMDrec("Strasse"), String))
    '    'strResult.Add(CType(rMDrec("PLZ"), String))
    '    'strResult.Add(CType(rMDrec("Ort"), String))
    '    'strResult.Add(CType(rMDrec("Land"), String))
    '    'strResult.Add(CType(rMDrec("Telefon"), String))
    '    'strResult.Add(CType(rMDrec("Telefax"), String))
    '    'strResult.Add(CType(rMDrec("eMail"), String))
    '    'strResult.Add(CType(rMDrec("Customer_ID"), String))

    '  Catch ex As Exception
    '    logger.Error(ex.ToString)
    '    '_ClsLog.WriteTempLogFile(String.Format("***GetSelectedMDGuid_1: {0}", ex.Message))

    '  End Try

    '  'End While
    '  'rMDrec.Close()
    '  'If strResult.Count = 0 Then strResult.Add("Kein Schlüssel vorhanden!!!")


    'Catch ex As Exception
    '  logger.Error(ex.ToString)
    '  '_ClsLog.WriteTempLogFile(String.Format("***GetSelectedMDGuid_2: {0}", ex.Message))

    'End Try

    'Return result
  End Function

  ''' <summary>
  ''' Gibt den Mandanten-Guid aus der Array aus
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetMDGuid() As String
    Return m_md.GetMDData4SelectedMD(0, Now.Year).MDGuid

    'Return (Me.GetSelectedMDData(0))
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetMDName() As List(Of String)
    Return (Me.GetSelectedMDData)
  End Function

#Region "WOS-Lizenzen..."

  ''' Bitte nicht mehr benutzen!!!
  Function GetMAWOSGuid() As String
    Return m_md.GetWOSGuid(0, Now.Year).WOSEmployeeGuid

    'Return GetMDProfilValue("Export", "MA_SPUser_ID", "")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetKDWOSGuid() As String
    Return m_md.GetWOSGuid(0, Now.Year).WOSCustomerGuid

    'Return GetMDProfilValue("Export", "KD_SPUser_ID", "")
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function GetVakWOSGuid() As String
    Return m_md.GetWOSGuid(0, Now.Year).WOSVacancyGuid

    'Return GetMDProfilValue("Export", "Vak_SPUser_ID", "")
  End Function


  ''' Bitte nicht mehr benutzen!!!
  Function bAllowedMADocTransferTo_WS() As Boolean
    Return m_md.AllowedExportEmployee2WOS(0, Now.Year)

    'Return Me.GetMAWOSGuid.ToString.Length > 31
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function bAllowedKDDocTransferTo_WS() As Boolean
    Return m_md.AllowedExportCustomer2WOS(0, Now.Year)

    'Return Me.GetKDWOSGuid.ToString.Length > 31
  End Function

  ''' Bitte nicht mehr benutzen!!!
  Function bAllowedVakDocTransferTo_WS() As Boolean
    Return m_md.AllowedExportVacancy2WOS(0, Now.Year)

    'Return Me.GetVakWOSGuid.ToString.Length > 31
  End Function

#End Region

  ''' Bitte nicht mehr benutzen!!!
  Function IsProcessRunning(ByVal sProcessName As String) As Boolean
    Return (System.Diagnostics.Process.GetProcessesByName(sProcessName).Length > 0)
  End Function

  ''' <summary>
  ''' überprüft ob ein Druckjob epxortieren darf. wird eher in den Listen- und Dokumenten-Druck abgefragt
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <param name="strJobNr"></param>
  ''' <returns></returns>
  ''' <remarks>Veraltet! Bitte SPUserSec.ClsUserSec.AllowedExportDoc(strJobNr) benutzen.</remarks>
  Function AllowedExportDoc(ByVal strJobNr As String) As Boolean

		Return SPProgUtility.SPUserSec.ClsUserSec.IsUserAllowed4DocExport(strJobNr) ' bResult
  End Function

	''' <summary>
	''' Rechte für Moduls werden registriert. 
	''' Bitte nicht mehr benutzen!!!
	''' </summary>
	''' <param name="strFieldname"></param>
	''' <returns>Boolean (True | False)</returns>
	''' <example>IsModulLicenceOK("255")</example>
	''' <remarks>
	''' Veraltet! Bitte SPUserSec.ClsUserSec.IsModulLicenceOK(strfieldname) benutzen.
	''' </remarks>
	Function IsModulLicenceOK(ByVal strFieldname As String) As Boolean
		Return CBool(m_md.IsModulLicenseOK(0, Now.Year, strFieldname))

	End Function

	''' <summary>
	''' sucht nach angegebenen Wert in der Benutzer.XML-Datei.
	''' Bitte nicht mehr benutzen!!!
	''' </summary>
	''' <param name="strFieldName"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetUSProfilValue(ByVal strFieldName As String) As String
		Return m_md.GetLogedUSProfilValue(0, strFieldName, m_CommonSetting.GetLogedUserNr)

	End Function

	''' Bitte nicht mehr benutzen!!!
	Function GetMDProfilValue(ByVal strSectionName As String, ByVal strFieldName As String, ByVal strValuebyNull As String) As String
		Return m_md.GetSelectedMDProfilValue(0, Now.Year, strSectionName, strFieldName, strValuebyNull)

	End Function

	''' Bitte nicht mehr benutzen!!!
	Overridable Function GetCtlNameValue(ByVal strCtlName As String, ByVal strValuebyNull As String) As String
		Return m_md.GetSelectedCtlNameValue(0, strCtlName, strValuebyNull, m_CommonSetting.GetLogedUserLanguage)

	End Function

	''' Bitte nicht mehr benutzen!!!
	Function GetMainViewXMLValue(ByVal strQuery As String, ByVal strValuebyNull As String) As String
		Return m_md.GetSelectedCtlNameValue(0, strQuery, strValuebyNull, m_CommonSetting.GetLogedUserLanguage)

	End Function

	''' <summary>
	''' Gibt die XML-Value einer Datei und Query aus...
	''' Bitte nicht mehr benutzen!!!
	''' </summary>
	''' <param name="strFilename"></param>
	''' <param name="strQuery"></param>
	''' <param name="strValuebyNull"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetXMLValueByQuery(ByVal strFilename As String,
							  ByVal strQuery As String,
							  ByVal strValuebyNull As String) As String
		Return GetXMLValueByQueryWithFilename(strFilename, strQuery, strValuebyNull)

	End Function


	''' <summary>
	''' übersetzt ein(en) beliebiges(n) Wort/Satz via xml-Datei
	''' Bitte nicht mehr benutzen!!!
	''' </summary>
	''' <param name="strText"></param>
	''' <returns></returns>
	''' <remarks>Veraltet! Bitte SPTranslation.ClsTranslation.TranslateText(strText) benutzen.</remarks>
	Function TranslateText(ByVal strText As String) As String

    Dim strOriginText As String = strText
    Dim strResult As String = strText
    Dim strUSLang As String = Me.GetUSLanguage()
    Dim strFilename As String = String.Format("{0}{1}", GetSpSTempPath(), TransFilename)

    If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
    Dim strQuery As String = "//Control[@Name=" & Chr(34) & strText & Chr(34) & "]/CtlLabel" & strUSLang
    strResult = GetXMLNodeValue(strFilename, strQuery)

    If strResult = String.Empty Then strResult = strText

    If Me.GetLogedUSNr = 1 Then
      Dim _clsEventlog As New ClsEventLog
      _clsEventlog.WriteToLogFile(String.Format("{0}{1}{0} >>> {0}{2}{0}", Chr(34), strOriginText, strResult), _
                                  String.Empty, "TranslateText", True, Me.GetTranslationLOGFile())
    End If

    Return strResult
  End Function

  ''' <summary>
  ''' Übersetzt den selektierten Text in bestimmten Forms
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <param name="strText"></param>
  ''' <returns></returns>
  ''' <remarks>Veraltet! Bitte SPTranslation.ClsTranslation.TranslateText(strText, strCallerInfo) benutzen.</remarks>
  Function TranslateText(ByVal strText As String, ByVal strCallerInfo As String) As String
    Dim strOriginText As String = strText
    Dim strResult As String = strText
    Dim strUSLang As String = Me.GetUSLanguage()
    Dim strFilename As String = Me.GetTranslateDataFile()
    Dim strLocalFilename As String = If(Me.GetLogedUSNr = 1, GetTranslateDataFile, String.Format("{0}{1}", Me.GetSpSTempPath(), _
                                                   FileIO.FileSystem.GetName(Me.GetTranslateDataFile())))

    Try
      If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
      Dim strQuery As String = "//Control[@Name=" & Chr(34) & strText & Chr(34) & "]/CtlLabel" & strUSLang
      strResult = GetXMLNodeValue(strLocalFilename, strQuery)

      If strResult = String.Empty Then strResult = strText

      If Me.GetLogedUSNr = 1 Then
        Dim _clsEventlog As New ClsEventLog
        _clsEventlog.WriteToLogFile(String.Format("{0}{1}{0} >>> {0}{2}{0}", Chr(34), strOriginText, strResult), _
                                    strCallerInfo, String.Empty, True, Me.GetTranslationLOGFile())
      End If

    Catch ex As Exception
      Dim _clsEventlog As New ClsEventLog
      _clsEventlog.WriteToEventLog("(TranslateText) Fehler ist aufgetreten... " & ex.Message)

    End Try

    Return strResult
  End Function

  ''' <summary>
  ''' Übersetzt eine Text in gewünschte Sprache
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <param name="strText">Der zuübersetzende Text</param>
  ''' <param name="strLang">Die zuübersetzende Sprache: D, I, F</param>
  ''' <returns></returns>
  ''' <seealso>TranslateText</seealso>
  ''' <example>translatetextinto("Bereit", "I")</example>
  ''' <remarks>Veraltet! Bitte SPTranslation.ClsTranslation.TranslateTextInto(strText, strLang) benutzen.</remarks>
  Function TranslateTextInto(ByVal strText As String, ByVal strLang As String) As String
    Dim strOriginText As String = strText
    Dim strResult As String = strText
    Dim strFilename As String = Me.GetTranslateDataFile()
    Dim strLocalFilename As String = If(Me.GetLogedUSNr = 1, GetTranslateDataFile, String.Format("{0}{1}", Me.GetSpSTempPath(), _
                                                   FileIO.FileSystem.GetName(Me.GetTranslateDataFile())))

    If strLang = "D" Then strLang = String.Empty
    If strLang <> String.Empty Then strLang = "_" & strLang
    Dim strQuery As String = "//Control[@Name=" & Chr(34) & strText & Chr(34) & "]/CtlLabel" & strLang
    strResult = GetXMLNodeValue(strLocalFilename, strQuery)

    If strResult = String.Empty Then strResult = strText

    If Me.GetLogedUSNr = 1 Then
      Dim _clsEventlog As New ClsEventLog
      _clsEventlog.WriteToLogFile(String.Format("{0}{1}{0} >>> {0}{2}{0}", Chr(34), strOriginText, strResult), _
                                  String.Empty, "TranslateText", True, Me.GetTranslationLOGFile())
    End If

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
  ''' Seriennummer für topTapi3
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetTapi3SN() As String
    Return m_CommonSetting.GetTapi3Data.TapiSerialnumber

    'Return "yourlicencekey"
  End Function

  ''' <summary>
  ''' Lizenzkey für topTapi3
  ''' Bitte nicht mehr benutzen!!!
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function GetTapi3Key() As String
    Return m_CommonSetting.GetTapi3Data.TapiLicenseKey

    'Return "yourlicencekey"
  End Function
#End Region


#Region "Encryption und Decryption..."

  Function EncryptString(ByVal strText2Encrypt As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Function DecryptString(ByVal strEncryptedText As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Function DecryptBase64String(ByVal myString As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
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

	End Sub

  Public Sub New()
    m_Progpath = New ClsProgPath
  End Sub

End Class
