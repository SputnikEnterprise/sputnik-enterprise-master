
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.DateAndTimeCalculation


Imports System.Data.SqlClient
Imports Microsoft.Win32
Imports System.Management
Imports System.Security.Principal
Imports System.Threading


Public Class LogingUtility


#Region "private fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath


#End Region


#Region "Constructor"
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		m_InitializationData = _setting

		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


	End Sub


#End Region


	'Public Function SaveLogingintoWSDatabase() As Boolean
	'	Dim strStationID As String = String.Format("Machinename: {0}|Domainname: {1}|UserName: {2}|MDName: {3}|MDGuid: {4}|{5}|MDCity: {6}|MDTelefon: {7}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 m_InitializationData.MDData.MDName,
	'																						 m_InitializationData.MDData.MDGuid,
	'																						 m_InitializationData.MDData.MDCity,
	'																						 m_InitializationData.MDData.MDTelefon)
	'	Dim result As Boolean = True

	'	Try
	'		Dim strIDString As String = String.Format("{0}", strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SPServiceUtil
	'		Dim strWSValue = wsMyService.SaveUserData2WSDb(strIDString)
	'		result = Not strWSValue.ToLower.Contains("error")


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'		Return False
	'	End Try

	'	Return result
	'End Function


End Class



Module funcGAV_PVL

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsPropSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsEventLog As New SPProgUtility.ClsEventLog



	'Function GetGAVVersionFromWS(ByVal iGAVNr As Integer, ByVal dGAVDate As Date) As List(Of String)
	'   Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'   Dim strResult As New List(Of String)

	'   Dim strMessage As String = String.Empty
	'   Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}", _
	'                                              Environment.MachineName, _
	'                                              Environment.UserDomainName, _
	'                                              Environment.UserName, _
	'                                              _ClsPropSetting.GetSelectedMDData(1))

	'   Try
	'     Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'     Dim wsMyService As New SP_GAV_PVL.SPGAV2012Data
	'     strResult = wsMyService.GetGAVVersionValue(strIDString, iGAVNr).ToList


	'   Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(GetGAVVersionFromWS) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message)

	'	End Try


	'	Return (strResult)
	'End Function

	'Function th_SaveLogingintoWSDatabase() As String
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim t As Thread
	'	t = New Thread(AddressOf SaveLogingintoWSDatabase)
	'	t.Name = "th_SaveLogingintoWSDatabase"
	'	t.Start()
	'	t.IsBackground = True

	'	Dim strWSValue As String = "Erfolg"
	'	'Dim strMessage As String = String.Empty
	'	'Dim strMDData As List(Of String) = _ClsPropSetting.GetSelectedMDData
	'	'Dim strStationID As String = String.Format("Machinename: {0}|Domainname: {1}|UserName: {2}|MDName: {3}|MDGuid: {4}|{5}|Windowskey: {6}", _
	'	'                                           Environment.MachineName, _
	'	'                                           Environment.UserDomainName, _
	'	'                                           Environment.UserName, _
	'	'                                           strMDData(1), _
	'	'                                           strMDData(0), _
	'	'                                           GetHardwareInfo, _
	'	'                                           GetWindowsSerialnumber)

	'	'Try
	'	'  Dim strIDString As String = String.Format("{0}", strStationID)
	'	'  Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SPServiceUtil
	'	'  strWSValue = wsMyService.SaveUserData2WSDb(strIDString)
	'	'  MsgBox(strIDString)

	'	'Catch ex As Exception
	'	'  _ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'	'                               "(SaveLogingintoWSDatabase) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'	'                     ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "SaveLogingintoWSDatabase")

	'	'End Try

	'	Return strWSValue
	'End Function

	'Function SaveLogingintoWSDatabase() As String
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strMessage As String = String.Empty
	'	Dim strMDData As List(Of String) = _ClsPropSetting.GetSelectedMDData
	'	Dim strStationID As String = String.Format("Machinename: {0}|Domainname: {1}|UserName: {2}|MDName: {3}|MDGuid: {4}|{5}|Windowskey: {6}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 strMDData(1),
	'																						 strMDData(0),
	'																						 GetHardwareInfo,
	'																						 GetWindowsSerialnumber)
	'	Dim strWSValue As String = "Erfolg"

	'	Try
	'		Dim strIDString As String = String.Format("{0}", strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SPServiceUtil
	'		strWSValue = wsMyService.SaveUserData2WSDb(strIDString)
	'		'MsgBox(strIDString)

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(SaveLogingintoWSDatabase) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "SaveLogingintoWSDatabase")

	'	End Try

	'	Return strWSValue
	'End Function

	'Function GetCountryList(ByVal strCountryCode As String) As DataSet
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strMessage As String = String.Empty
	'	Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 _ClsPropSetting.GetSelectedMDData(1))
	'	Dim dsWSValue As New DataSet

	'	Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil
	'		dsWSValue = wsMyService.GetCountrylist(strIDString, strCountryCode)


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(GetCountryList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message)

	'	End Try

	'	Return dsWSValue
	'End Function

	Function GetJobList(ByVal strLanguage As String, ByVal strSex As String) As DataSet
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
																							 Environment.MachineName,
																							 Environment.UserDomainName,
																							 Environment.UserName,
																							 _ClsPropSetting.GetSelectedMDData(1))
		Dim dsWSValue As New DataSet

		Try
			Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
			Dim wsMyService As New _wsSPS_Services.SPModulUtil
			dsWSValue = wsMyService.GetJobData(strIDString, strLanguage, strSex)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
																	 "(GetJobList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
												 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetJobList")

		End Try

		Return dsWSValue
	End Function

	Function GetHBBJobList(ByVal strLanguage As String, ByVal strSex As String) As DataSet
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
																							 Environment.MachineName,
																							 Environment.UserDomainName,
																							 Environment.UserName,
																							 _ClsPropSetting.GetSelectedMDData(1))
		Dim dsWSValue As New DataSet

		Try
			Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
			Dim wsMyService As New _wsSPS_Services.SPModulUtil
			dsWSValue = wsMyService.Get_HBBJob_Data(strIDString, strLanguage, strSex)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
																	 "(GetHBBJobList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
												 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetHBBJobList")

		End Try

		Return dsWSValue
	End Function

	Function GetBGBJobList(ByVal strLanguage As String, ByVal strSex As String) As DataSet
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
																							 Environment.MachineName,
																							 Environment.UserDomainName,
																							 Environment.UserName,
																							 _ClsPropSetting.GetSelectedMDData(1))
		Dim dsWSValue As New DataSet

		Try
			Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
			Dim wsMyService As New _wsSPS_Services.SPModulUtil
			dsWSValue = wsMyService.Get_BGBJob_Data(strIDString, strLanguage, strSex)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
																	 "(GetBGBJobList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
												 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetBGBJobList")

		End Try

		Return dsWSValue
	End Function

	Function GetBGBHBBJobList(ByVal strLanguage As String, ByVal strSex As String) As DataSet
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
																							 Environment.MachineName,
																							 Environment.UserDomainName,
																							 Environment.UserName,
																							 _ClsPropSetting.GetSelectedMDData(1))
		Dim dsWSValue As New DataSet

		Try
			Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
			Dim wsMyService As New _wsSPS_Services.SPModulUtil
			dsWSValue = wsMyService.Get_BGBHBBJob_Data(strIDString, strLanguage, strSex)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
																	 "(Get_BGBHBBJob_Data) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
												 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetBGBHBBJobList")

		End Try

		Return dsWSValue
	End Function

	Function GetRegionsList(ByVal strRegionCode As String, ByVal strLanguage As String) As DataSet
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMessage As String = String.Empty
		Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
																							 Environment.MachineName,
																							 Environment.UserDomainName,
																							 Environment.UserName,
																							 _ClsPropSetting.GetSelectedMDData(1))
		Dim dsWSValue As New DataSet

		Try
			Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
			Dim wsMyService As New _wsSPS_Services.SPModulUtil
			dsWSValue = wsMyService.GetRegionsData(strIDString, strLanguage, strRegionCode)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
																	 "(GetRegionsList) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
												 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetRegionsList")

		End Try

		Return dsWSValue
	End Function

#Region "Funktionen zur Quellensteuerdaten..."

	'Function AllowedQSTData(ByVal strKanton As String, ByVal iYear As Integer,
	'												ByVal iKinder As Integer,
	'												ByVal strGruppe As String, ByVal strKichensteuer As String, ByVal strSex As String) As String
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strMessage As String = String.Empty
	'	Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 _ClsPropSetting.GetSelectedMDData(1))
	'	Dim strWSValue As String = "Erfolg"

	'	Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SP_ServiceUtil.SPServiceUtil

	'		strWSValue = wsMyService.AllowedQstCode(strKanton, iYear,
	'																				iKinder,
	'																				strGruppe, strKichensteuer, strSex)


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(AllowedQSTData) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "AllowedQSTData")

	'	End Try

	'	Return strWSValue
	'End Function

	'Function GetQSTInfo(ByVal strKanton As String, ByVal iYear As Integer,
	'												ByVal cEinkommen As Double, ByVal iKinder As Integer,
	'												ByVal strGruppe As String, ByVal strKichensteuer As String, ByVal strSex As String) As String()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strMessage As String = String.Empty
	'	Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 _ClsPropSetting.GetSelectedMDData(1))
	'	Dim strWSValue As String() = {"Erfolg"}

	'	Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SP_ServiceUtil.SPServiceUtil

	'		strWSValue = wsMyService.GetQstData(strIDString, strKanton, iYear,
	'																				cEinkommen, iKinder,
	'																				strGruppe, strKichensteuer, strSex) '.ToArray


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(GetQSTInfo) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetQSTInfo")

	'	End Try

	'	Return strWSValue
	'End Function

	'Function GetDataQSTCode(ByVal strKanton As String, ByVal iYear As Integer, ByVal strGeschlecht As String) As String()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strMessage As String = String.Empty
	'	Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 _ClsPropSetting.GetSelectedMDData(1))
	'	Dim strWSValue As String() = {"Erfolg"}

	'	Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SP_ServiceUtil.SPServiceUtil

	'		strWSValue = wsMyService.GetQstCodeData(strIDString, strKanton, iYear, strGeschlecht, _ClsPropSetting.GetUSLanguage)
	'		m_Logger.LogInfo(String.Format("{0}|{1}|{2}|{3}|{4}", strIDString, strKanton, iYear, _ClsPropSetting.GetUSLanguage, strWSValue))

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(GetDataQSTCode) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetDataQSTCode")

	'	End Try

	'	Return strWSValue
	'End Function

	'Function GetDataQSTKirchensteuer(ByVal strKanton As String, ByVal iYear As Integer, ByVal strGruppe As String, ByVal strGeschlecht As String) As String()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Dim strMessage As String = String.Empty
	'	Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 _ClsPropSetting.GetSelectedMDData(1))
	'	Dim strWSValue As String() = {"Erfolg"}

	'	Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SP_ServiceUtil.SPServiceUtil

	'		strWSValue = wsMyService.GetQstKirchensteuerData(strIDString, strKanton, iYear, strGruppe, strGeschlecht, _ClsPropSetting.GetUSLanguage)
	'		m_Logger.LogInfo(String.Format("{0}|{1}|{2}|{3}|{4}", strIDString, strKanton, iYear, _ClsPropSetting.GetUSLanguage, strWSValue))


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab &
	'																 "(GetDataQSTKirchensteuer) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf &
	'											 ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetDataQSTKirchensteuer")

	'	End Try

	'	Return strWSValue
	'End Function

	'Function GetDataQSTKinder(ByVal strKanton As String, ByVal iYear As Integer, ByVal strGruppe As String, ByVal strGeschlecht As String) As String()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strMessage As String = String.Empty
	'	Dim strStationID As String = String.Format("{0}: {1}\{2} | {3}",
	'																						 Environment.MachineName,
	'																						 Environment.UserDomainName,
	'																						 Environment.UserName,
	'																						 _ClsPropSetting.GetSelectedMDData(1))
	'	Dim strWSValue As String() = {"Erfolg"}

	'	Try
	'		Dim strIDString As String = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
	'		Dim wsMyService As New _wsSPS_Services.SPModulUtil ' SP_ServiceUtil.SPServiceUtil

	'		strWSValue = wsMyService.GetQstData4Kinder(strIDString, strKanton, iYear, strGruppe, strGeschlecht)
	'		m_Logger.LogInfo(String.Format("{0}|{1}|{2}|{3}", strIDString, strKanton, iYear, strWSValue.ToArray))

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
	'		_ClsEventLog.WriteToEventLog(Now.ToString & vbTab & _
	'                                  "(GetDataQSTKinder) Fehler bei der Kontrolle der Verbindung zum Server..." & vbCrLf & _
	'                        ex.Message, "Sputnik Enterprise Suite", EventLogEntryType.Error, "GetDataQSTKinder")

	'   End Try

	'   Return strWSValue
	' End Function

#End Region


#Region "Helper Funktionen..."

	Function GetHardwareInfo() As String
    Dim strNetworkMacAdress As String = "MAC-Adresse: {0}"
    Dim strDiskID As String = "DiskID: {0}"
    Dim strProcessorID As String = "ProcessID: {0}"
    Dim strBiosSerialNumber As String = "BiosSerialnumber: {0}"
    Dim strValues As String = String.Empty

    ' Netzwerk MAC-Adresse
    Try

      Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
      Dim moc As ManagementObjectCollection = mc.GetInstances()
      Dim strMyMACAddress As String = ""
      For Each mo As ManagementObject In moc
        If (strMyMACAddress.Equals(String.Empty)) Then
          If CBool(mo("IPEnabled")) Then strMyMACAddress = mo("MacAddress").ToString()
          mo.Dispose()
        End If
        strMyMACAddress = strMyMACAddress.Replace(":", String.Empty)
      Next
      strNetworkMacAdress = String.Format(strNetworkMacAdress, strMyMACAddress)

    Catch ex As Exception
      strNetworkMacAdress = String.Format(strNetworkMacAdress, "Not defined!")
    End Try

    ' Lokaler Harddisk ID
    Try
      Dim strMyDiskID As String = "Not defined!"
      Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", "C"))
      disk.Get()
      strMyDiskID = disk("VolumeSerialNumber").ToString()

      strDiskID = String.Format(strDiskID, strMyDiskID)

    Catch ex As Exception
      strDiskID = String.Format(strDiskID, "Not defined!")

    End Try

    ' Processor ID
    Try
      Dim query As New SelectQuery("Win32_processor")
      Dim search As New ManagementObjectSearcher(query)
      Dim info As ManagementObject
      Dim strMyProcID As String = "Not defined!"

      For Each info In search.Get()
        strMyProcID = info("processorId").ToString()
      Next

      strProcessorID = String.Format(strProcessorID, strMyProcID)

    Catch ex As Exception
      strProcessorID = String.Format(strProcessorID, "Not defined!")

    End Try

    ' Bios Serialnumber
    Try
      Dim objWMIService As Object
      Dim objItems As Object
      Dim objItem As Object
      Dim strBios As String = String.Empty
      Dim searcher As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_BIOS")

      For Each queryObj As ManagementObject In searcher.Get()
        Console.WriteLine("Win32_BIOS instance")
        strBios = CStr(queryObj("SerialNumber"))
      Next

      strBiosSerialNumber = String.Format(strBiosSerialNumber, strBios)

      objWMIService = Nothing
      objItems = Nothing
      objItem = Nothing

    Catch ex As Exception
      strBiosSerialNumber = String.Format(strBiosSerialNumber, "Not defined!")

    End Try

    strValues = String.Format("{0}|{1}|{2}|{3}", strNetworkMacAdress, strDiskID, strProcessorID, strBiosSerialNumber)

    Return strValues
  End Function

  Sub Test()
    Dim irc As IdentityReferenceCollection
    Dim ir As IdentityReference
    irc = WindowsIdentity.GetCurrent().Groups
    Dim strGroupName As String

    For Each ir In irc
      Dim mktGroup As IdentityReference = ir.Translate(GetType(NTAccount))
      MsgBox(mktGroup.Value)
      Debug.WriteLine(mktGroup.Value)
      strGroupName = mktGroup.Value.ToString

    Next

  End Sub

	Function GetBiosArchitecture(ByVal Computer As String) As String
		Dim strValue As String = "Bios no Serialnumber"
		Dim objWMIService As Object
		Dim objItems As Object
		Dim objItem As Object

		Try
			' Windows 64/32 Bit
			'objWMIService = GetObject("winmgmts:\\" & Computer & "\root\CIMV2")
			'objItems = objWMIService.ExecQuery( _
			'    "SELECT * FROM Win32_OperatingSystem")
			'For Each objItem In objItems
			'  strValue = objItem.OSArchitecture
			'Next


			Dim searcher As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_BIOS")

			For Each queryObj As ManagementObject In searcher.Get()
				Console.WriteLine("Win32_BIOS instance")
				strValue = CStr(queryObj("SerialNumber"))
			Next

			objWMIService = Nothing
			objItems = Nothing
			objItem = Nothing

		Catch ex As Exception
			strValue = "Bios no Serialnumber"
		End Try

		Return strValue
	End Function

  Private Function GetWindowsSerialnumber() As String
    Dim BaseKey As RegistryKey
    Dim SubKey As RegistryKey
    Dim strKey As String = "No WindowsKey defined..."

    If Environment.Is64BitOperatingSystem = True Then
      BaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
      SubKey = BaseKey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
    Else
      SubKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
    End If

    If Not SubKey Is Nothing Then
      Try
        strKey = String.Empty
        Dim rpk As Byte() = DirectCast(SubKey.GetValue("DigitalProductId", New _
          Byte(-1) {}), Byte())

        Const iRPKOffset As Integer = 52
        Const strPossibleChars As String = "BCDFGHJKMPQRTVWXY2346789"
        Dim i As Integer = 28

        Do
          Dim lAccu As Long = 0
          Dim j As Integer = 14

          Do

            lAccu *= 256
            lAccu += Convert.ToInt64(rpk(iRPKOffset + j))
            rpk(iRPKOffset + j) = Convert.ToByte(Convert.ToInt64(Math.Floor(CSng(lAccu) / 24.0F)) And Convert.ToInt64(255))
            lAccu = lAccu Mod 24

            j -= 1
          Loop While j >= 0

          i -= 1
          strKey = strPossibleChars(CInt(lAccu)).ToString() & strKey
          If (0 = ((29 - i) Mod 6)) AndAlso (-1 <> i) Then
            i -= 1
            strKey = "-" & strKey
          End If
        Loop While i >= 0

      Catch ex As Exception

        strKey = ex.ToString
      End Try

    End If

    Return strKey
  End Function

#End Region

End Module

