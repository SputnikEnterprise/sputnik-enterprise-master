
Imports System.Data.SqlClient
Imports NLog
Imports System.IO

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten


Namespace ProgPath

  Public Class ClsProgPath
    Inherits Utilities

    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Private m_Reg As ClsDivReg



#Region "Fileserver methoden..."

    ''' <summary>
    ''' Server: \\Server\spenterprise$\
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileServerPath() As String 'Implements iPath.IProgPath.GetFileServerPath
      m_Reg = New ClsDivReg
      Return AddDirSep(m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SPSFileServerPath"))
    End Function

    ''' <summary>
    ''' Server: \\Server\spenterprise$\Bin\
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileServerBinPath() As String 'Implements iPath.IProgPath.GetFileServerBinPath
      Return String.Format("{0}Bin\", Me.GetFileServerPath)
    End Function

    ''' <summary>
    ''' Server: \\Server\spenterprise$\Protokoll\
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileServerProtokollPath() As String 'Implements iPath.IProgPath.GetFileServerProtokollPath
      Dim result As String = String.Format("{0}Protokoll\", Me.GetFileServerPath)
      If Not System.IO.Directory.Exists(result) Then System.IO.Directory.CreateDirectory(result)

      Return result
    End Function

    ''' <summary>
    ''' Server: \\Server\spenterprise$\Update\
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileServerUpdatePath() As String 'Implements iPath.IProgPath.GetFileServerUpdatePath
      Return String.Format("{0}Update\", Me.GetFileServerPath)
    End Function

    ''' <summary>
    ''' Server: \\Server\spenterprise$\Bin\Programm.xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileServerXMLFullFilename() As String 'Implements iPath.IProgPath.GetRootIniFullFile
      Return String.Format("{0}Programm.xml", Me.GetFileServerBinPath)
    End Function

    ''' <summary>
    ''' Server - Programm.dat für Datenbankeinstellungen: \\Server\spenterprise$\Bin\translationdata.xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNewTranslationFilename() As String 'Implements iPath.IProgPath.GetNewTranslationFilename
      Dim strServerFilename As String = String.Format("{0}Translationdata.xml", GetFileServerBinPath)

      Try
        Dim strLocalFilename As String = String.Format("{0}{1}", GetSpSTempFolder(), TransFilename)
				If File.Exists(strLocalFilename) AndAlso File.GetLastWriteTime(strLocalFilename) = File.GetLastWriteTime(strServerFilename) Then Return strLocalFilename

				File.Copy(strServerFilename, strLocalFilename, True)

				Return strLocalFilename

			Catch ex As IOException
				Return strServerFilename

			Catch ex As Exception
				logger.Warn(ex.ToString)
				Return strServerFilename

			End Try

      Return String.Format("{0}Translationdata.xml", GetFileServerBinPath)
    End Function

    ''' <summary>
    ''' Server: \\Server\spenterprise$\Protokoll\Translationdata.txt
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNewTranslationLOGFilename() As String 'Implements iPath.IProgPath.GetNewTranslationLOGFilename
      Dim objAssInfo As New ClsAssInfo()
      Return String.Format("{0}Translationdata.txt", Me.GetFileServerProtokollPath())
    End Function

    ''' <summary>
    ''' Server - Programm.dat für Datenbankeinstellungen: \\Server\spenterprise$\Protokoll\SPSErrorLogs.txt
    ''' </summary>
    ''' <returns>mein return</returns>
    ''' <remarks>mein remark</remarks>
    Public Function GetFileServerErrorFilename() As String 'Implements iPath.IProgPath.GetFileServerErrorFilename
      Return String.Format("{0}SPSErrorLogs.txt", Me.GetFileServerProtokollPath)
    End Function

    ''' <summary>
    ''' Server - Programm.dat für Datenbankeinstellungen: \\Server\spenterprise$\Protokoll\SPSLogs.txt
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileServerLogFilename() As String 'Implements iPath.IProgPath.GetFileServerLogFilename
      Return String.Format("{0}SPSLogs.txt", Me.GetFileServerProtokollPath)
    End Function


#End Region

    ''' <summary>
    ''' Formatierung der Datum für SQL-Datenbanken. Ist nicht mehr nötig!!!
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetSQLDateFormat() As String 'Implements iPath.ICommonSetting.GetSQLDateFormat
      Dim dateformat As String = "dd.mm.yyyy"

      Try
        Dim xDoc As XDocument = XDocument.Load(GetFileServerXMLFullFilename)
        Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("sqldateformat")
                                          Select New With {
                                              .dateformat = GetSafeStringFromXElement(exportSetting.Element("dateformat"))
                                                }).FirstOrDefault()

        dateformat = ConfigQuery.dateformat
        If String.IsNullOrEmpty(dateformat) Then dateformat = "dd.mm.yyyy"

      Catch ex As Exception

      End Try

      Return dateformat
    End Function

    ''' <summary>
    ''' Die ScanDb-Daten aus der \\Server\spenterprise$\Bin\Programm.xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetScanDbData() As ScanDbData 'Implements iPath.IProgPath.GetScanDbData
      Dim _ClsProgSetting As New ClsProgSettingPath
      Dim result As New ScanDbData

      Try
        Dim xDoc As XDocument = XDocument.Load(Me.GetFileServerXMLFullFilename)
        Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements("scandb")
                                          Select New With {
                                              .mdconnstr = GetSafeStringFromXElement(exportSetting.Element("connstr")),
                                              .mddbname = GetSafeStringFromXElement(exportSetting.Element("dbname")),
                                              .mddbserver = GetSafeStringFromXElement(exportSetting.Element("dbname"))
                                                }).FirstOrDefault()

        result.ScanDbConn = FromBase64(ConfigQuery.mdconnstr)
        result.ScanDbName = ConfigQuery.mddbname
        result.ScanDbServer = ConfigQuery.mddbserver


      Catch ex As Exception
        logger.Error(ex.ToString)
      End Try

      Return result
    End Function

    ''' <summary>
    ''' Die DbSelect-Daten aus der \\Server\spenterprise$\Bin\Programm.xml
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDbSelectData() As ClsDbSelectData 'Implements iPath.IProgPath.GetDbSelectData
      Dim _ClsProgSetting As New ClsProgSettingPath
      Dim result As New ClsDbSelectData

      Try
        Dim xDoc As XDocument = XDocument.Load(Me.GetFileServerXMLFullFilename)
        Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements("md_0")
                                          Select New With {
                                              .mdconnstr = GetSafeStringFromXElement(exportSetting.Element("connstr")),
                                              .mddbname = GetSafeStringFromXElement(exportSetting.Element("dbname")),
                                              .mddbserver = GetSafeStringFromXElement(exportSetting.Element("dbname"))
                                                }).FirstOrDefault()

        result.MDDbConn = FromBase64(ConfigQuery.mdconnstr)
        result.MDDbName = ConfigQuery.mddbname
        result.MDDbServer = ConfigQuery.mddbserver


      Catch ex As Exception
        logger.Error(ex.ToString)
      End Try

      Return result
    End Function


  End Class


End Namespace
