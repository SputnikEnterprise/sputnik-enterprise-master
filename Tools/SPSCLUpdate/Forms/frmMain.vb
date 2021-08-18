
'Imports SP.Infrastructure
'Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.UI

Imports System.Net.NetworkInformation
Imports System.Net
Imports System.Net.Sockets

'Imports System.Console
'Imports System.Security.AccessControl
'Imports System.Security.Principal

Imports System.IO
Imports System.Text
Imports System.Collections.ObjectModel
Imports System.Data.SqlClient
Imports DevExpress.XtraSplashScreen
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Views.Base
Imports System.Text.RegularExpressions
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports SPSCLUpdate.SPUpdateUtilitiesService

Public Class frmMain


	Private Delegate Sub StartLoadingData()
	'Private Delegate Sub StartDoingUdate()
	Private Delegate Sub StartLogingData(msg As String)

#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As Logging.ILogger = New Logging.Logger()
	'Private Shared m_Logger As Logging.ILogger

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_UpdateDatabaseAccess As FTPUpdateDatabaseAccess

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	Private m_UpdateSettingData As New BindingList(Of UpdateSettingData)

	'Private Property m_NeededUpdateFileresult As FTPUpdateData

	Private m_Mandantupdatedata As IEnumerable(Of UpdateMDData)

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_UpdateWebServiceUri As String
	Private m_Timer As System.Timers.Timer

	'''' <summary>
	'''' UI Utility functions.
	'''' </summary>
	'Private m_UtilityUI As UtilityUI
	Private m_Utility As Utility



	'Membervaribale, die beim Kopieren 'True' wird,
	'wenn der Anwender 'Abbrechen' geklickt hat.
	Private processingJob As Boolean

	'In diesem Stringbuilder wird das Protokoll erstellt.
	Private myLogString As StringBuilder

	'Diese Queue dient zur Speicherung der jeweils 
	'letzten 100 Zeilen des Protokolls.
	Private myVisibleLineQueue As Queue

	'Lässt andere Prozeduren wissen, ob gerade kopiert wird.
	Private myCopyInProgress As Boolean

	Private bNoMessage As Boolean

	'Hält fest, ob - bedingt durch Befehlszeilenargumente - 
	'der Kopiervorgang mit einer durch /autostart:kopierlistendatei.osd
	'angegebenen Datei automatisch gestartet und das Programm anschließend
	'beendet werden soll.
	Private myAutoStartMode As Boolean
	'Der ermittelte Dateiname steht dann hier drin:
	'Passiert beim Einlesen der Kopierlistendatei ein Fehler (eine Ausnahme),
	'wird ein Fehler im Ereignisprotokoll von Windows hinterlegt.
	'Die app.config-Datei wurde entsprechend geändert, damit mit
	'My.Application.Log.WriteException ein Eintrag im Anwendungs-
	'protokoll erfolgen kann.
	Private myAutoStartCopyList As String

	'Hält fest, ob die Anwendung im Silent-Modus gestartet werden soll,
	'bei dem keine Dialoge angezeigt und keine Ausgaben erfolgenden sollen.
	'Der Silent-Modus wird mit /silent initiiert.
	Private mySilentMode As Boolean

	Private m_UpdateSettingFile As String
	Private m_Continue As Boolean

	Private m_NewUpdateFilePath As String
	Private m_TempNETFolder As String
	Private m_TempDocumentFolder As String
	Private m_TempQueryFolder As String
	Private m_TempTemplateFolder As String

	Private m_NETFolder As String
	Private m_DocumentFolder As String
	Private m_QueryFolder As String

	Private m_NetFiles As List(Of String)
	Private m_DocumentFiles As List(Of String)
	Private m_QueryFiles As List(Of String)
	Private m_TemplateFiles As List(Of String)

	Private m_LogData As List(Of EntryLOGData)
	Private m_IsUpdateAllowed As Boolean
	Private m_StationData As StationData

	Private m_ExistsNewFTPFileVersion As Boolean

#End Region


	Public Property AUTOSTART As Boolean


#Region "private consts"

	Private Const URL_UPDATE_SITE As String = "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
	Private Const URL_DOWNLOAD_SPS_FILE As String = "http://downloads.domain.com/sps_downloads/prog/settings/ProgUpdatesetting.txt"
	Private Const SETTING_FILE_NAME As String = "ProgUpdatesetting.sps"

	Private Const FOLDER_DOWNLOAD_SPS_NET As String = "{0}\NET"
	Private Const FOLDER_DOWNLOAD_SPS_DOCUMENT As String = "{0}\DOCUMENTS"
	Private Const FOLDER_DOWNLOAD_SPS_QUERY As String = "{0}\QUERY"
	Private Const FOLDER_DOWNLOAD_SPS_TEMPLATE As String = "{0}\TEMPLATE"
	Private Const OPTION_HISTORYLLEVELS As Integer = 5

#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		'm_UtilityUI = New UtilityUI
		m_Utility = New Utility

		m_NetFiles = New List(Of String)
		m_DocumentFiles = New List(Of String)
		m_QueryFiles = New List(Of String)
		m_TemplateFiles = New List(Of String)
		m_UpdateWebServiceUri = URL_UPDATE_SITE
		m_StationData = New StationData

		InitializeComponent()

		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Text
		WindowsFormsSettings.AllowAutoFilterConditionChange = DefaultBoolean.False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Dim connectionString As String = String.Empty
		m_UpdateDatabaseAccess = New FTPUpdateDatabaseAccess(connectionString, Language.German)

		Reset()

		m_StationData.LocalHostName = GetInternalHostName()
		m_StationData.LocalIPAddress = GetInternalIP()
		m_StationData.LocalDomainName = GetInternalDomainName()
		m_StationData.ExternalIPAddress = GetExternalIP()

		m_LogData = New List(Of EntryLOGData)
		m_LogData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Info", .Message = "LOG started..."})

		CheckForNewProgramUpdates()
		m_Continue = m_Continue AndAlso Not m_ExistsNewFTPFileVersion AndAlso IsStationAllowedForUpdate()



		'		Dim intervalPeriod As Double = Math.Max(My.Settings.SearchInterval, 0)
		'		Dim intervalTime As Double = Math.Min(Math.Max(intervalPeriod, 1), 60) * 60000D
		'#If DEBUG Then
		'		intervalTime = 60000
		'#End If

		'm_Timer = New System.Timers.Timer
		'If intervalPeriod > 0 Then
		'	m_Timer.Interval = intervalTime
		'	AddHandler m_Timer.Elapsed, AddressOf RunTimer

		'	m_Timer.Enabled = m_Continue
		'End If

		' first load data
		m_Logger.LogDebug(String.Format("performing to load setting file: {0}", m_Continue))
		If m_Continue Then PerformLoadingSettingFile()

		btnLoadData.Enabled = m_Continue
		btnDownloadData.Enabled = m_Continue

		AddHandler Me.gvUpdateSetting.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub

#End Region


#Region "Private properties"

	''' <summary>
	''' Gets the selected setting record to install.
	''' </summary>
	Public ReadOnly Property SelectedSettingRecord As UpdateSettingData
		Get
			Dim gvData = TryCast(grdUpdateSetting.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvUpdateSetting.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim data = CType(gvData.GetRow(selectedRows(0)), UpdateSettingData)
					Return data
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "Public methodes"

	Public Sub ProcessUpdate()
		PerformProcessUpdate()
	End Sub

#End Region

	Private Sub Reset()
		Dim success As Boolean = True

		m_ExistsNewFTPFileVersion = False
		hlNewProgramVersion.Visible = False

		ResetSettingGrid()

		success = success AndAlso CreateTemporaryUpdateFolder()


		m_Continue = success

	End Sub

	''' <summary>
	''' Resets the Setting overview grid.
	''' </summary>
	Private Sub ResetSettingGrid()

		' Reset the grid
		gvUpdateSetting.OptionsView.ShowIndicator = False
		gvUpdateSetting.OptionsView.ColumnAutoWidth = True
		gvUpdateSetting.OptionsView.ShowAutoFilterRow = True

		gvUpdateSetting.Columns.Clear()

		Dim columnProgCommand As New DevExpress.XtraGrid.Columns.GridColumn()
		columnProgCommand.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnProgCommand.Caption = "Quellpfad (kopieren von)"
		columnProgCommand.Name = "ProgCommand"
		columnProgCommand.FieldName = "ProgCommand"
		columnProgCommand.Visible = True
		gvUpdateSetting.Columns.Add(columnProgCommand)

		Dim columnSource As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSource.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSource.Caption = "Suchmaske (Dateierweiterung)"
		columnSource.Name = "SourceFolder"
		columnSource.FieldName = "SourceFolder"
		columnSource.Visible = True
		columnSource.Width = 150
		gvUpdateSetting.Columns.Add(columnSource)

		Dim columnSourceFile As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSourceFile.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSourceFile.Caption = "Filename"
		columnSourceFile.Name = "SourceFile"
		columnSourceFile.FieldName = "SourceFile"
		columnSourceFile.Visible = True
		columnSourceFile.Width = 150
		gvUpdateSetting.Columns.Add(columnSourceFile)

		Dim columnDestination As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDestination.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDestination.Caption = "Zielpfad (kopieren nach)"
		columnDestination.Name = "DestinationFolder"
		columnDestination.FieldName = "DestinationFolder"
		columnDestination.Visible = True
		columnDestination.Width = 150
		gvUpdateSetting.Columns.Add(columnDestination)

		Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDescription.Caption = "Dateiinformation"
		columnDescription.Name = "Description"
		columnDescription.FieldName = "Description"
		columnDescription.Visible = True
		gvUpdateSetting.Columns.Add(columnDescription)


		grdUpdateSetting.DataSource = Nothing

	End Sub

	Private Sub RunTimer(sender As Object, e As EventArgs)
		m_Timer.Enabled = False
		m_Timer.Stop()

		PerformLoadingSettingFile()
		If Not m_UpdateSettingData Is Nothing AndAlso m_UpdateSettingData.Count > 0 Then
			PerformProcessUpdate()
		End If

		m_Timer.Start()
		m_Timer.Enabled = True

	End Sub

	Private Sub OnbtnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click
		PerformLoadingSettingFile()

		'SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub OnbtnDownloadData_Click(sender As Object, e As EventArgs) Handles btnDownloadData.Click
		PerformProcessUpdate()

		'SplashScreenManager.CloseForm(False)

	End Sub

	''' <summary>
	'''  Performs the Download asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformIsAllowedStationUpdateWebserviceCall() As Boolean
		Dim result As Boolean = False

#If DEBUG Then
		'		Customer_ID = "57EA3F1A-1390-4B96-B9B3-BF98F555BC4F" ' "C942EF9B-A455-49BE-B7FB-5507FCD2F1C0"
#End If

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)
			Dim stationData As SPUpdateUtilitiesService.StationData = New SPUpdateUtilitiesService.StationData With {.LocalIPAddress = m_StationData.LocalIPAddress,
				.ExternalIPAddress = m_StationData.ExternalIPAddress, .LocalHostName = m_StationData.LocalHostName, .LocalDomainName = m_StationData.LocalDomainName}

			' Read data over webservice
			Dim searchResult = webservice.IsStationUpdateAllowed(stationData)

			If Not searchResult Then
				m_Logger.LogWarning(String.Format("station Is Not allowed To be updated. LocalIPAddress: {0} | ExternalIPAddress: {1} | LocalHostName: {2} | LocalDomainName: {3}",
																					m_StationData.LocalIPAddress, m_StationData.ExternalIPAddress, m_StationData.LocalHostName, m_StationData.LocalDomainName))

				Return False
			End If

			result = searchResult

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return result

	End Function

	Private Sub PerformLoadingSettingFile()
		Dim success As Boolean = True

		m_NetFiles.Clear()
		m_DocumentFiles.Clear()
		m_QueryFiles.Clear()
		m_TemplateFiles.Clear()

		If processingJob Then Return
		processingJob = True
		grdUpdateSetting.DataSource = Nothing
		grdLOG.DataSource = Nothing

		Try

			success = success AndAlso DownloadNewConfigFile()
			m_Logger.LogDebug(String.Format("downloading file {0} was {1}", m_UpdateSettingFile, success))

			If success AndAlso File.Exists(m_UpdateSettingFile) Then LoadCopyEntryList(m_UpdateSettingFile)
			grdUpdateSetting.DataSource = m_UpdateSettingData
			m_Logger.LogDebug(String.Format("{0} jobs was founded.", m_UpdateSettingData.Count))


		Catch ex As Exception
			AddLogData(String.Format("PerformLoadingSettingFile: {0}", ex.ToString))

		Finally
			processingJob = False
		End Try

	End Sub

	Private Sub PerformProcessUpdate() 'As Boolean
		Dim success As Boolean = True

		m_IsUpdateAllowed = m_Continue
		If m_ExistsNewFTPFileVersion Then Return

		m_Logger.LogDebug(String.Format("entring PerformProcessUpdate"))
		If processingJob Then Return 'False
		processingJob = True
		AddLogData(String.Format("entring PerformProcessUpdate"))

#If DEBUG Then

		Dim selectedRec = SelectedSettingRecord
		Try

			If Not selectedRec Is Nothing Then
				If selectedRec.ProgCommand.ToUpper = "Delete".ToUpper Then

					If selectedRec.SourceFile.ToString.ToUpper.Contains("DNCBackup".ToUpper) Then
						success = success AndAlso ProcessFileToDelete(selectedRec)
					End If

				ElseIf selectedRec.ProgCommand.ToUpper = "Copy".ToUpper Then
					If selectedRec.SourceFolder.Contains("<your path>") Then
						Trace.WriteLine(Path.Combine(selectedRec.SourceFolder, selectedRec.SourceFile))
						If Not String.IsNullOrWhiteSpace(selectedRec.SourceFolder) Then success = success AndAlso DoCopyFile(selectedRec)
					End If

				Else

				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			AddLogData(String.Format("PerformProcessUpdate: {0}", ex.ToString))

			Return
		Finally
			processingJob = False
		End Try

		Return

#End If


		Try
			Dim deleteData = m_UpdateSettingData.Where(Function(data) data.ProgCommand.ToUpper = "Delete".ToUpper).ToList()
			m_Logger.LogDebug(String.Format("entring deleting file"))
			For Each result In deleteData

				If Not result Is Nothing Then
					If Not String.IsNullOrWhiteSpace(result.DestinationFolder) Then success = success AndAlso ProcessFileToDelete(result)
				End If

				If Not success Then Throw New Exception(String.Format("{0}", result.CommandLine))
			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			AddLogData(String.Format("PerformProcessUpdate (Delete): {0}", ex.ToString))

			Return 'False
		Finally
			processingJob = False
		End Try

		Try

			Dim copyData = m_UpdateSettingData.Where(Function(data) data.ProgCommand.ToUpper = "Copy".ToUpper).ToList()
			m_Logger.LogDebug(String.Format("entring copying file"))
			For Each result In copyData

				If Not result Is Nothing Then
					If Not String.IsNullOrWhiteSpace(result.SourceFolder) Then success = success AndAlso DoCopyFile(result)
					'End If
				End If

				If Not success Then Throw New Exception(String.Format("{0}", result.CommandLine))
			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			AddLogData(String.Format("PerformProcessUpdate (Copy): {0}", ex.ToString))

			Return 'False
		Finally
			processingJob = False
		End Try

		m_Logger.LogDebug(String.Format("finishing PerformProcessUpdate..."))
		AddLogData(String.Format("finishing PerformProcessUpdate"))

		'SplashScreenManager.CloseForm(False)
		processingJob = False
		'Return success

	End Sub


	Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

		If processingJob Then
			e.Cancel = True

		Else
			'm_Timer.Enabled = False
			'm_Timer.Stop()

		End If

	End Sub

	Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
		'If AUTOSTART Then ProcessUpdate()
		m_Logger.LogDebug(String.Format("AUTOSTART was {0}", AUTOSTART))
		'ProcessUpdate()
	End Sub


	Private Function IsStationAllowedForUpdate() As Boolean
		Dim success As Boolean = True
		Dim stationAdress As String = String.Empty
		Dim allowedStationID As Boolean = PerformIsAllowedStationUpdateWebserviceCall()
		If Not allowedStationID Then Return allowedStationID

		Dim ipData As String = "10.23.223.11, 10.23.223.12, 10.23.223.13, 10.23.223.14, 10.23.192.60"
		Dim ipAddresses = ipData.Split(New String() {",", " ", ";", "|"}, StringSplitOptions.RemoveEmptyEntries)
		Dim blacklist As List(Of String) = ipAddresses.ToList()

		Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
		Dim adapter As NetworkInterface
		For Each adapter In adapters
			Dim properties As IPInterfaceProperties = adapter.GetIPProperties()
			If properties.UnicastAddresses.Count > 0 Then
				For Each unicastadress As UnicastIPAddressInformation In properties.UnicastAddresses
					Dim ip As IPAddress = unicastadress.Address
					If ip.AddressFamily = AddressFamily.InterNetwork Then
						stationAdress &= If(String.IsNullOrWhiteSpace(stationAdress), "", "|") & ip.ToString
					End If
				Next unicastadress
			End If
		Next adapter
		m_Logger.LogDebug(String.Format("{0}", stationAdress))

		For Each itm As String In blacklist
			If stationAdress.Contains(itm) Then
				m_Logger.LogWarning(String.Format("{0} not allowed...", itm))
				AddLogData(String.Format("update is not allowed!!! {0}", itm))
				success = False
				Exit For
			End If
		Next

		Return success

	End Function


	''' <summary>
	''' Lädt die Kopierliste aus einer Textdatei.
	''' </summary>
	''' <param name="copyEntryListPath">Der Ppfad und Dateiname der Kopierlistendatei.</param>
	''' <remarks></remarks>
	Private Sub LoadCopyEntryList(ByVal copyEntryListPath As String)
		Dim locParser As FileIO.TextFieldParser

		m_UpdateSettingData = New BindingList(Of UpdateSettingData)

		Try
			locParser = My.Computer.FileSystem.OpenTextFieldParser(copyEntryListPath, ";")

		Catch ex As Exception
			m_Logger.LogError(String.Format("parsing file: {0} >>> {1}", copyEntryListPath, ex.ToString()))
			AddLogData(String.Format("parsing file: {0} >>> {1}", copyEntryListPath, ex.ToString()))
			Return
		End Try

		Dim strSourcePath As String = String.Empty
		Dim strDestPath As String = String.Empty

		Try
			Dim i As Integer = 0
			Do While Not locParser.EndOfData
				i += 1

				Try
					Dim locFields As String() = locParser.ReadFields
					Dim locCopyListEntry As New UpdateSettingData

					strSourcePath = String.Empty
					strDestPath = String.Empty
					If locFields.Length > 1 Then
						If locFields(0).ToString.Trim <> String.Empty Then

							If locFields(0).ToString.ToUpper = "Delete".ToUpper Then
								locCopyListEntry.ProgCommand = locFields(0).ToString.ToUpper
								locCopyListEntry.SourceFile = locFields(1)

								strDestPath = TranslateString(locFields(2)).ToString
								If strDestPath.Length > 5 Then
									If Not strDestPath.EndsWith("\") Then strDestPath = String.Format("{0}{1}", strDestPath, Path.DirectorySeparatorChar)
									locCopyListEntry.DestinationFolder = strDestPath
									locCopyListEntry.DestinationFile = locFields(1)
									locCopyListEntry.Description = locFields(3)
								End If
								locCopyListEntry.SourceFolder = strDestPath

							Else

								If Not locFields(2).ToString.ToUpper.Contains("$MD$".ToUpper) Then
									strSourcePath = TranslateString(locFields(0)).ToString
									If strSourcePath.Length > 5 Then
										If Not strSourcePath.EndsWith("\") Then strSourcePath = String.Format("{0}{1}", strSourcePath, Path.DirectorySeparatorChar)

										locCopyListEntry.ProgCommand = "Copy".ToUpper
										locCopyListEntry.SourceFolder = strSourcePath
										locCopyListEntry.SourceFile = locFields(1)

										strDestPath = TranslateString(locFields(2)).ToString
										If strDestPath.Length > 5 Then
											If Not strDestPath.EndsWith("\") Then strDestPath = String.Format("{0}{1}", strDestPath, Path.DirectorySeparatorChar)

											locCopyListEntry.DestinationFolder = strDestPath
											locCopyListEntry.DestinationFile = locFields(1)
											locCopyListEntry.Description = locFields(3)

										End If
									End If

								ElseIf locFields(2).ToString.ToUpper.Contains("$MD$".ToUpper) Then
									m_Logger.LogWarning("md will not be update!")
									AddLogData(String.Format("md will not be update: {0}", locFields(2).ToString))
								End If

							End If
						ElseIf locFields(1).ToString.ToUpper.Contains("$MD$=".ToUpper) Then
							UpdateFileVersion = Mid(locFields(1).ToString, "$MD$=".Length + 1)

						ElseIf locFields(1).ToString.ToUpper.Contains("Version=".ToUpper) Then
							UpdateFileVersion = Mid(locFields(1).ToString, "Version=".Length + 1)

						ElseIf locFields(1).ToString.ToUpper.Contains("FileTime=".ToUpper) Then
							UpdateFileVersion = Mid(locFields(1).ToString, "FileTime=".Length + 1)

						ElseIf locFields(1).ToString.ToUpper.Contains("FileDate=".ToUpper) Then
							UpdateFileVersion = Mid(locFields(1).ToString, "FileDate=".Length + 1)

						End If
					End If

					If Not String.IsNullOrWhiteSpace(locCopyListEntry.SourceFolder) AndAlso Directory.Exists(locCopyListEntry.SourceFolder) Then
						locCopyListEntry.CommandID = i
						locCopyListEntry.CommandLine = String.Format("{0} {1} {2}", locCopyListEntry.ProgCommand, locCopyListEntry.SourceFile, locCopyListEntry.DestinationFile)
						m_UpdateSettingData.Add(locCopyListEntry)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					AddLogData(String.Format("{0}", ex.ToString))
				End Try

			Loop

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			AddLogData(String.Format("{0}", ex.ToString))

			Return
		End Try

	End Sub

	Private Function ProcessFileToDelete(ByVal settingFile As UpdateSettingData) As Boolean
		Dim success As Boolean = True

		Try
			For Each myFile In Directory.GetFiles(settingFile.DestinationFolder, settingFile.SourceFile)
				m_Logger.LogInfo(String.Format("deleting file: {0}", myFile))
				AddLogData(String.Format("deleting file: {0}", myFile))
				File.Delete(myFile)
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("deleting file: {0} from {1} | {2}", settingFile.DestinationFolder, settingFile.SourceFile, ex.ToString))
			AddLogData(String.Format("deleting file: {0} from {1} | {2}", settingFile.DestinationFolder, settingFile.SourceFile, ex.ToString))
			'Return False
		End Try


		Return success

	End Function

	Private Function DoCopyFile(ByVal settingFile As UpdateSettingData) As Boolean
		Dim success As Boolean = True
		Dim dataToCopy As New List(Of UpdateCommandData)
		Dim locFiles As ReadOnlyCollection(Of String) = Nothing
		Dim locDoCopy As Boolean = True

		If m_ExistsNewFTPFileVersion Then Return False

		locFiles = My.Computer.FileSystem.GetFiles(settingFile.SourceFolder, FileIO.SearchOption.SearchTopLevelOnly, settingFile.SourceFile)
		m_Logger.LogInfo(String.Format("getting file list to copy: folder: {0} file list: {1}", settingFile.SourceFolder, settingFile.SourceFile))
		AddLogData(String.Format("getting file list to copy: folder: {0} file list: {1}", settingFile.SourceFolder, settingFile.SourceFile))

		For Each locFile As String In locFiles
			Dim locCopyTaskItem As New UpdateCommandData
			locCopyTaskItem.SourceFile = New FileInfo(locFile)

			If (locCopyTaskItem.SourceFile.Attributes AndAlso FileAttributes.Directory) <> FileAttributes.Directory Then
				locCopyTaskItem.SourceFolder = New DirectoryInfo(Path.GetDirectoryName(locFile))
				locCopyTaskItem.DestFolder = New DirectoryInfo(settingFile.DestinationFolder)

				dataToCopy.Add(locCopyTaskItem)
			End If

		Next

		For Each locFileItem In dataToCopy
			Dim locDestPath As DirectoryInfo = Nothing
			Dim locOldDestPath As DirectoryInfo = Nothing
			Dim locDestFile As FileInfo = Nothing

			Try
				locDestPath = locFileItem.DestFolder
				locDestFile = New FileInfo(Path.Combine(locDestPath.ToString, locFileItem.SourceFile.Name))

				If (locFileItem.SourceFile.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden AndAlso My.Settings.Option_CopyHiddenFiles = False Then locDoCopy = False
				If (locFileItem.SourceFile.Attributes And FileAttributes.System) = FileAttributes.System AndAlso My.Settings.Option_CopySystemFiles = False Then locDoCopy = False
				If locDestFile.Exists Then locDoCopy = True

				If locDoCopy Then
					If Not locDestPath.Exists Then locDestPath.Create()

					If locOldDestPath Is Nothing Then locOldDestPath = New DirectoryInfo(locDestPath.ToString)
					If locOldDestPath.FullName <> locDestPath.FullName Then locOldDestPath = New DirectoryInfo(locDestPath.ToString)

					Application.DoEvents()
					m_Logger.LogInfo(String.Format("trying copy file: {0} >>> {1}", locFileItem.SourceFile, locDestFile))
					AddLogData(String.Format("trying copy file: {0} >>> {1}", locFileItem.SourceFile, locDestFile))
					success = success AndAlso CopyFileInternal(locFileItem.SourceFile, locDestFile)

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				AddLogData(String.Format("{0}", ex.ToString))

			End Try

			If Not success Then Return False
		Next


		Return success

	End Function


	Sub SaveNewUpdateVersionToDb()
		Dim sSql As String = "Insert tbl_UpdateInfo (UpdateVersion, UpdateDescription, UpdateDate) Values (@FileVerion, @StInfo, @UpdateDate)"
		Dim _clsSystem As New ClsMain_Net
		Dim strRootConn As String = _clsSystem.GetDbSelectConnString()
		Dim ConnRoot As New SqlConnection(strRootConn)

		Try
			ConnRoot.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnRoot)
			Dim param As System.Data.SqlClient.SqlParameter = New SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@StInfo", Environment.MachineName & " / " & Environment.UserName)
			param = cmd.Parameters.AddWithValue("@FileVerion", UpdateFileVersion)
			param = cmd.Parameters.AddWithValue("@UpdateDate", CDate(Now))

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			AddLogData(String.Format("{0}", ex.ToString))

		Finally
			ConnRoot.Dispose()

		End Try

	End Sub

	''' <summary>
	''' Interne Kopierroutine, die den eigentlichen 'Job' des Kopierens übernimmt.
	''' Achtung: eine Service wird falls vorhanden, entladet, dann kopiert und geladen.
	''' </summary>
	''' <param name="SourceFile"></param>
	''' <param name="DestFile"></param>
	''' <remarks></remarks>
	Private Function CopyFileInternal(ByVal SourceFile As FileInfo, ByVal DestFile As FileInfo) As Boolean
		Dim success As Boolean = True

		Try
			If Not DestFile.Exists Then
				Try
					System.IO.File.Copy(SourceFile.ToString, DestFile.ToString, True)

					If SourceFile.ToString.ToUpper.Contains("\Services".ToUpper) Then
						SetupMyService(DestFile, False)
					ElseIf SourceFile.DirectoryName = "Update" Then
						RegisterComDll(SourceFile, DestFile)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("copying new file: {0} >>> {1} | {2}", SourceFile.ToString, DestFile.ToString, ex.ToString))
					AddLogData(String.Format("copying new file: {0} >>> {1} | {2}", SourceFile.ToString, DestFile.ToString, ex.ToString))
					Return False
				End Try

				Return True
			End If

			If SourceFile.LastWriteTime <> DestFile.LastWriteTime Then
				ManageFileBackupHistory(DestFile)

				Try
					System.IO.File.Copy(SourceFile.ToString, DestFile.ToString, True)
					File.SetCreationTime(DestFile.ToString, File.GetCreationTime(SourceFile.ToString))
					File.SetLastAccessTime(DestFile.ToString, File.GetLastAccessTime(SourceFile.ToString))
					File.SetLastWriteTime(DestFile.ToString, File.GetLastWriteTime(SourceFile.ToString))

					If SourceFile.ToString.ToUpper.Contains("\Services".ToUpper) Then
						SetupMyService(DestFile, True)
					ElseIf SourceFile.DirectoryName = "Update" Then
						RegisterComDll(SourceFile, DestFile)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("copying existing files: {0} >>> {1} | {2}", SourceFile.ToString, DestFile.ToString, ex.ToString))
					AddLogData(String.Format("copying existing files: {0} >>> {1} | {2}", SourceFile.ToString, DestFile.ToString, ex.ToString))
					Return False
				End Try

				Return True

			Else
				m_Logger.LogInfo(String.Format("file copy was not necessary!!! {0} >>> {1}", SourceFile.ToString, DestFile.ToString))
				AddLogData(String.Format("file copy was not necessary!!! {0} >>> {1}", SourceFile.ToString, DestFile.ToString))

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			AddLogData(String.Format("{0}", ex.ToString))

		End Try

		Return success

	End Function


	Sub RegisterComDll(ByVal SourceFile As FileInfo, ByVal DestFile As FileInfo)
		Dim strCommandLine As String = String.Empty

		Try
			If DestFile.ToString.ToUpper.IndexOf(".DLL".ToUpper) > -1 Then
				If SourceFile.ToString.ToUpper.IndexOf("\Net".ToUpper) = -1 Then     ' keine .Net-DLL, dann registrieren
					strCommandLine = Environment.GetFolderPath(Environment.SpecialFolder.System) & "\regsvr32.exe /s " & Chr(34) & DestFile.ToString & Chr(34)

					Try
						Shell(strCommandLine)


					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}", ex.ToString))
						AddLogData(String.Format("setup COM DLL: sourcefilename: {0} >>> desfilename: {1} >>> commandline: {2} | {3}", SourceFile.ToString, DestFile.ToString, strCommandLine, ex.ToString))

					End Try

				End If

			ElseIf DestFile.ToString.ToUpper.IndexOf(".TLB".ToUpper) > -1 Then
				Dim strRegASM As FileInfo ' String.Empty
				Dim strFilename As String() = Split(DestFile.ToString, DestFile.Extension)
				Dim strJustFileName As String = strFilename(0)

				If DestFile.ToString.ToLower.Contains("4_net") Then
					strRegASM = New FileInfo("<your path>\RegAsm.Exe")
				Else
					strRegASM = New FileInfo("<your path>\RegAsm.Exe")
				End If

				If Not strRegASM.Exists Then
					AddLogData(String.Format("DLL file was not founded: {0}", strRegASM.ToString))

					Return
				End If
				strCommandLine = Chr(34) & strRegASM.ToString & Chr(34) & " " & Chr(34) &
												strJustFileName & ".DLL" & Chr(34) & " /tlb:" & Chr(34) &
												strJustFileName & ".TLB" & Chr(34)

				Try
					Dim ctest As String = Shell(strCommandLine).ToString

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					AddLogData(String.Format("setup COM DLL: sourcefilename: {0} >>> desfilename: {1} >>> commandline: {2} | {3}", SourceFile.ToString, DestFile.ToString, strCommandLine, ex.ToString))

				End Try

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			AddLogData(String.Format("setup COM DLL: sourcefilename: {0} >>> desfilename: {1} >>> commandline: {2} | {3}", SourceFile.ToString, DestFile.ToString, strCommandLine, ex.ToString))

		End Try

	End Sub

	Sub SetupMyService(ByVal DestFile As FileInfo, ByVal bUnInstallFile As Boolean)
		Dim strCommandLine As String = String.Empty

		If Not My.Computer.Info.OSFullName.ToUpper.Contains("Server".ToUpper) Then Exit Sub
		If DestFile.ToString.ToUpper.Contains(".config".ToUpper) Then Exit Sub
		If Not DestFile.ToString.ToUpper.Contains("service.exe".ToUpper) Then Exit Sub

		Dim strInstallUtil As FileInfo
		Dim strFilename As String() = Split(DestFile.ToString, DestFile.Extension)
		Dim strJustFileName As String = strFilename(0)

		strInstallUtil = New FileInfo("<your path>\installutil.Exe")

		If Not strInstallUtil.Exists Then
			MessageBox.Show("Fehlende oder Fehlerhafte Dateiangaben: " & strInstallUtil.ToString & vbCrLf &
			"Bitte kontaktieren Sie Ihren Systemadministrator.", "Suche nach Installutil.exe",
			MessageBoxButtons.OK, MessageBoxIcon.Error)
			Exit Sub
		End If
		strCommandLine = Chr(34) & strInstallUtil.ToString & Chr(34) & " " & If(bUnInstallFile, "-u", "") & Chr(34)
		strCommandLine &= DestFile.ToString & Chr(34)

		Try
			Dim ctest As String = Shell(strCommandLine).ToString

		Catch ex As Exception
			m_Logger.LogError(String.Format("installing service: {0} >>> strInstallUtil: {1} | {2}", strCommandLine, strInstallUtil, ex.ToString))
			AddLogData(String.Format("setup service: desfilename: {0}, {1} | {2}", DestFile.ToString, bUnInstallFile, ex.ToString))

		End Try

	End Sub

	Function TranslateString(ByVal myString As String) As String
		Dim strResult As String = myString
		Dim _ClsSystem As New ClsMain_Net

		Dim strServerSputnik As String = _ClsSystem.GetSrvRootPath()
		If Not strServerSputnik.EndsWith("\") Then strServerSputnik = String.Format("{0}\", strServerSputnik)
		Dim strServerSputnikInit As String = _ClsSystem.GetInitPath()
		If Not strServerSputnikInit.EndsWith("\") Then strServerSputnikInit = String.Format("{0}\", strServerSputnikInit)

		Dim strServerSputnikUpdate As String = _ClsSystem.GetSrvUpdatePath()
		If Not strServerSputnikUpdate.EndsWith("\") Then strServerSputnikUpdate = String.Format("{0}\", strServerSputnikUpdate)

		Dim strServerSputnikUpdateBinn As String = Path.Combine(strServerSputnikUpdate, "Binn\")
		Dim strServerSputnikUpdateNet As String = Path.Combine(strServerSputnikUpdate, "Binn\Net\")

		Dim strServerUpdateServices As String = _ClsSystem.GetUpdateServicePath()
		If Not strServerUpdateServices.EndsWith("\") Then strServerUpdateServices = String.Format("{0}\", strServerUpdateServices)

		Dim strLocalSputnik As String = _ClsSystem.GetLocalPath()
		If Not strLocalSputnik.EndsWith("\") Then strLocalSputnik = String.Format("{0}\", strLocalSputnik)

		Dim strLocalBinnSputnik As String = Path.Combine(strLocalSputnik, "Binn\")
		If Not File.Exists(Path.Combine(strLocalBinnSputnik, "SPS.MainView.exe")) Then
			m_Logger.LogError(String.Format("path of local sputnik folder may not be OK!!! {0}", Path.Combine(strLocalBinnSputnik, "SPS.MainView.exe")))

			Return String.Empty
		End If

		Dim strLocalServices As String = Path.Combine(strLocalBinnSputnik, "Services\")


		If String.IsNullOrWhiteSpace(strServerSputnik) OrElse String.IsNullOrWhiteSpace(strServerSputnikInit) OrElse String.IsNullOrWhiteSpace(strServerSputnikUpdate) OrElse String.IsNullOrWhiteSpace(strServerSputnikUpdateNet) OrElse String.IsNullOrWhiteSpace(strServerUpdateServices) Then Return String.Empty
		If String.IsNullOrWhiteSpace(strLocalSputnik) OrElse String.IsNullOrWhiteSpace(strLocalBinnSputnik) OrElse String.IsNullOrWhiteSpace(strLocalServices) Then Return String.Empty


		Try
			If myString.ToUpper.IndexOf("$ServerSputnik$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnik$".ToUpper, strServerSputnik)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikInit$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikInit$".ToUpper, strServerSputnikInit)

			ElseIf myString.ToUpper.IndexOf("$LocalSputnik$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$LocalSputnik$".ToUpper, strLocalSputnik)

			ElseIf myString.ToUpper.IndexOf("$LocalSputnikBinn$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$LocalSputnikBinn$".ToUpper, strLocalBinnSputnik)

			ElseIf myString.ToUpper.IndexOf("$LocalSputnikServices$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$LocalSputnikServices$".ToUpper, strLocalServices)

			ElseIf myString.ToUpper.IndexOf("$ServerUpdateServices$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerUpdateServices$".ToUpper, strServerUpdateServices)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikUpdate$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikUpdate$".ToUpper, strServerSputnikUpdate)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikUpdateBinn$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikUpdateBinn$".ToUpper, strServerSputnikUpdateBinn)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikUpdateNet$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikUpdateNet$".ToUpper, strServerSputnikUpdateNet)

			ElseIf myString.ToUpper.IndexOf("$System32$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$System32$".ToUpper, Environment.GetFolderPath(Environment.SpecialFolder.System) & "\")

			ElseIf myString.ToUpper.IndexOf("$sysWOW64$".ToUpper) > -1 Then
				If Environment.Is64BitOperatingSystem Then
					'If Directory.Exists(System.IO.Path.GetDirectoryName(System.Environment.SystemDirectory) & "\SysWOW64\") Then
					strResult = myString.ToUpper.Replace("$sysWOW64$".ToUpper, Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) & "\")
					'System.IO.Path.GetDirectoryName(System.Environment.SystemDirectory) & "\SysWOW64\")
				Else
					strResult = String.Empty
				End If

			ElseIf myString.ToUpper.IndexOf("$DE$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$DE$".ToUpper, strLocalSputnik & "DE\")

			ElseIf myString.ToUpper.IndexOf("$FR$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$FR$".ToUpper, strLocalSputnik & "FR\")

			ElseIf myString.ToUpper.IndexOf("$IT$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$IT$".ToUpper, strLocalSputnik & "IT\")

			ElseIf myString.ToUpper.IndexOf("$MD$".ToUpper) > -1 Then
				strResult = String.Empty

				'Dim strMDPath As New IO.DirectoryInfo(strServerSputnik)
				'Dim diTarget As DirectoryInfo = New DirectoryInfo(strServerSputnik)
				'Dim aMDList As New List(Of String)

				'' Copy each file into it's new directory.
				'For Each strMDPath In strMDPath.GetDirectories()
				'	Console.WriteLine("Current Directory {0}", strMDPath.FullName)
				'	If strMDPath.Name.ToUpper.Contains("MD".ToUpper) Then
				'		If File.Exists(strMDPath.ToString & "2009\Programm.") Then

				'		End If
				'		aMDList.Add(strMDPath.ToString)
				'	End If
				'Next

				'strResult = myString.ToUpper.Replace("$System32$".ToUpper, Environment.GetFolderPath(Environment.SpecialFolder.System) & "\")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			AddLogData(String.Format("translating setting command: {0} | {1}", myString, ex.ToString))

		End Try

		Return strResult
	End Function

	''' <summary>
	''' Sorgt dafür, dass im Bedarfsfall Backupversionen einer Datei erstellt werden, 
	''' die durch eine neuere Version der Datei überschrieben werden soll.
	''' </summary>
	''' <param name="DestFile"></param>
	''' <remarks></remarks>
	Private Sub ManageFileBackupHistory(ByVal DestFile As FileInfo)
		Dim locFileToProcess As FileInfo

		Try
			locFileToProcess = New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, OPTION_HISTORYLLEVELS.ToString("0")))
			If locFileToProcess.Exists Then
				Try
					locFileToProcess.Delete()
				Catch ex As Exception
					m_Logger.LogError(String.Format("Deleting file: {0} | {1}", locFileToProcess.FullName, ex.ToString))
					AddLogData(String.Format("deleting file: {0} | {1}", locFileToProcess.FullName, ex.ToString))
				End Try
			End If

			For locHistoryCount As Integer = OPTION_HISTORYLLEVELS - 1 To 1 Step -1

				locFileToProcess = New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, locHistoryCount.ToString("0")))
				Dim locFileInfo As New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, (locHistoryCount + 1).ToString("0")))

				If locFileToProcess.Exists Then
					Try
						My.Computer.FileSystem.RenameFile(locFileToProcess.FullName, locFileInfo.Name)

					Catch ex As Exception
						m_Logger.LogError(String.Format("RenameFile: {0} >>> {1} | {2}", locFileToProcess.FullName, locFileInfo.Name, ex.ToString))
						AddLogData(String.Format("RenameFile: {0} >>> {1} | {2}", locFileToProcess.FullName, locFileInfo.Name, ex.ToString))
					End Try
				End If
			Next

			Try
				My.Computer.FileSystem.RenameFile(DestFile.FullName, (New FileInfo(DestFile.ToString & ".DNCBackup1").Name))

			Catch ex As Exception
				m_Logger.LogError(String.Format("Rename old file: {0} >>> {1} | {2}", DestFile.FullName, DestFile.ToString & ".DNCBackup1", ex.ToString))
				AddLogData(String.Format("Rename old file: {0} >>> {1} | {2}", DestFile.FullName, DestFile.ToString & ".DNCBackup1", ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			AddLogData(String.Format("destfilename: {0} | {1}", DestFile.FullName, ex.ToString))

		End Try

	End Sub

	Private Sub CheckForNewProgramUpdates()
		Dim success As Boolean = True
		Dim ftpModuleFileData = PerformRequiredClientSetupFilesWebserviceCallAsync()

		If Not ftpModuleFileData Is Nothing AndAlso ftpModuleFileData.Count > 0 Then
			Dim updateAssembly As FileInfo = New FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)
			Dim newData = ftpModuleFileData.Where(Function(data) data.FileDestPath = "ClientUpdate" AndAlso data.UpdateFilename = updateAssembly.Name).FirstOrDefault
			If Not newData Is Nothing Then
				Dim newFileDate As DateTime = New DateTime(CDate(newData.UpdateFileDate).Year, CDate(newData.UpdateFileDate).Month, CDate(newData.UpdateFileDate).Day, CDate(newData.UpdateFileTime).Hour, CDate(newData.UpdateFileTime).Minute, CDate(newData.UpdateFileTime).Second, 0)

				If newFileDate <> updateAssembly.LastWriteTimeUtc Then
					m_ExistsNewFTPFileVersion = True
					m_Logger.LogWarning("there is new program file to be updated!")
					Dim newMessage As String = "Bitte ersetzen Sie die neuen Dateien aus</br>{0}</br>durch</br>{1}"
					hlNewProgramVersion.Text = String.Format(newMessage, m_NewUpdateFilePath, Directory.GetCurrentDirectory)

					hlNewProgramVersion.Visible = True
					success = success AndAlso SaveProgramModuleFile("ClientUpdate", ftpModuleFileData)
					PerformSendingNewUpdateNotificationWebservice()

					Return
				End If
			End If
		End If

	End Sub

	Private Function SaveProgramModuleFile(ByVal moduleName As String, ByVal data As BindingList(Of FTPUpdateData)) As Boolean
		Dim success As Boolean = True
		Dim existsNewFileVersion As Boolean = False
		Dim tempFileName As String = String.Empty

		If data Is Nothing Then Return False
		Try

			Dim currentPath As String = Directory.GetCurrentDirectory()

			For Each itm In data
					Dim currentFile As FileInfo = New FileInfo(Path.Combine(currentPath, itm.UpdateFilename))
					Dim itmFileDate As DateTime = New DateTime(CDate(itm.UpdateFileDate).Year, CDate(itm.UpdateFileDate).Month, CDate(itm.UpdateFileDate).Day, CDate(itm.UpdateFileTime).Hour, CDate(itm.UpdateFileTime).Minute, CDate(itm.UpdateFileTime).Second, 0)

					existsNewFileVersion = currentFile.LastWriteTimeUtc <> itmFileDate
					If Not existsNewFileVersion Then Continue For

					tempFileName = Path.Combine(m_NewUpdateFilePath, itm.UpdateFilename)

					ManageFileHistory(currentFile)
					Dim remoteFile = PerformDownloadProgramModuleFileWebserviceCallAsync(moduleName, itm.UpdateID)
					If remoteFile Is Nothing Then Continue For

					itm.FileContent = remoteFile.FileContent
					Dim bytes() = itm.FileContent

					If (Not bytes Is Nothing) AndAlso Not m_Utility.WriteFileBytes(tempFileName, bytes) Then
						m_Logger.LogError(String.Format("programmodul file {0} could not be downloaded!", tempFileName))
						success = False

						Return False
					End If
					success = success AndAlso ChangeFileAttribute(tempFileName, itm)
					If success Then
						'File.Copy(tempFileName, Path.Combine(currentPath, itm.UpdateFilename), True)
						'File.Delete(tempFileName)
					End If

					m_Logger.LogWarning(String.Format("update state for programmodul file: {0} >>> {1}", Path.Combine(currentPath, itm.UpdateFilename), success))

					If Not success Then Return False
				Next


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

			Return False
		End Try


		Return success

	End Function

	Private Function PerformRequiredClientSetupFilesWebserviceCallAsync() As BindingList(Of FTPUpdateData)

		Dim data = New BindingList(Of FTPUpdateData)
		Dim updateAssembly As FileInfo = New FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)
			Dim customerData As SPUpdateUtilitiesService.CustomerMDData = New SPUpdateUtilitiesService.CustomerMDData With {.CustomerID = String.Empty, .LocalIPAddress = m_StationData.LocalIPAddress,
				.ExternalIPAddress = m_StationData.ExternalIPAddress, .LocalHostName = m_StationData.LocalHostName, .LocalDomainName = m_StationData.LocalDomainName}

			Dim searchResult As List(Of ModuleFilesDTO) = Nothing
			' Read data over webservice
			searchResult = webservice.GetProgramModuleFilesList(customerData, "ClientUpdate").ToList

			For Each result In searchResult

				Dim viewData = New FTPUpdateData With {
						.File_Guid = result.File_Guid,
						.FileContent = result.FileContent,
						.FileDestPath = result.ModuleName,
						.FileDestVersion = result.FileDestVersion,
						.UpdateFileDate = result.UpdateFileDate,
						.UpdateFilename = result.UpdateFilename,
						.UpdateFileSize = result.UpdateFileSize,
						.UpdateFileTime = result.UpdateFileTime,
						.UpdateID = result.UpdateID
					}

				data.Add(viewData)

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Return data

	End Function

	Private Function PerformDownloadProgramModuleFileWebserviceCallAsync(ByVal moduleName As String, ByVal recID As Integer) As FTPUpdateData

		Dim result As FTPUpdateData = Nothing
		If recID = 0 Then Return Nothing

#If DEBUG Then
		'		Customer_ID = "57EA3F1A-1390-4B96-B9B3-BF98F555BC4F" ' "C942EF9B-A455-49BE-B7FB-5507FCD2F1C0"
#End If

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)

			Dim searchResult As ModuleFilesDTO = Nothing
			' Read data over webservice
			searchResult = webservice.GetProgramModuleFileIDContent(moduleName, recID)

			If searchResult Is Nothing Then
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Der gewünschte (program modules) Datensatz mit der Nummer {0} wurde nicht gefunden.", recID))

				Return Nothing
			End If

			result = New FTPUpdateData With {
						.File_Guid = searchResult.File_Guid,
						.FileContent = searchResult.FileContent,
						.FileDestPath = searchResult.ModuleName,
						.FileDestVersion = searchResult.FileDestVersion,
						.UpdateFileDate = searchResult.UpdateFileDate,
						.UpdateFilename = searchResult.UpdateFilename,
						.UpdateFileSize = searchResult.UpdateFileSize,
						.UpdateFileTime = searchResult.UpdateFileTime,
						.UpdateID = searchResult.UpdateID
					}


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return result

	End Function

	Private Function PerformSendingNewUpdateNotificationWebservice() As Boolean

		Dim result As Boolean = False

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)

			Dim customerData As SPUpdateUtilitiesService.CustomerMDData = New SPUpdateUtilitiesService.CustomerMDData With {.CustomerID = String.Empty, .LocalIPAddress = m_StationData.LocalIPAddress,
				.ExternalIPAddress = m_StationData.ExternalIPAddress, .LocalHostName = m_StationData.LocalHostName, .LocalDomainName = m_StationData.LocalDomainName}

			' Read data over webservice
			Dim searchResult = webservice.SendNewUpdateFileNotificationToSputnik(customerData)

			If Not searchResult Then
				m_Logger.LogWarning(String.Format("update notification was not successfull!"))

				Return False
			End If

			result = searchResult

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Return result

	End Function


#Region "friend properties"

	Friend Property SilentMode() As Boolean
		Get
			Return mySilentMode
		End Get
		Set(ByVal value As Boolean)
			mySilentMode = value
		End Set
	End Property

	Friend Property AutoStartMode() As Boolean
		Get
			Return myAutoStartMode
		End Get
		Set(ByVal value As Boolean)
			myAutoStartMode = value
		End Set
	End Property

	Friend Property AutoStartCopyList() As String
		Get
			Return myAutoStartCopyList
		End Get
		Set(ByVal value As String)
			myAutoStartCopyList = value
		End Set
	End Property

	Private strFileVersion As String
	Property UpdateFileVersion() As String
		Get
			Return strFileVersion
		End Get
		Set(ByVal value As String)
			strFileVersion = value
		End Set
	End Property

	Private strFileTime As String
	Property UpdateFileTime() As String
		Get
			Return strFileTime
		End Get
		Set(ByVal value As String)
			strFileTime = value
		End Set
	End Property

	Private strFileDate As String

	Property UpdateFileDate() As String
		Get
			Return strFileDate
		End Get
		Set(ByVal value As String)
			strFileDate = value
		End Set
	End Property

#End Region






#Region "Helpers"

	Private Function ChangeFileAttribute(ByVal tempFileName As String, ByVal data As FTPUpdateData) As Boolean
		Dim success As Boolean = True

		Try

			Dim dtCreation As DateTime = CType(String.Format("{0:d} {1}", data.UpdateFileDate, data.UpdateFileTime), DateTime)
			dtCreation = New DateTime(dtCreation.Year, dtCreation.Month, dtCreation.Day, dtCreation.Hour, dtCreation.Minute, dtCreation.Second, DateTimeKind.Local)

			File.SetCreationTimeUtc(tempFileName, dtCreation)
			File.SetLastWriteTimeUtc(tempFileName, dtCreation)
			File.SetLastAccessTimeUtc(tempFileName, dtCreation)

		Catch ex As Exception
			Return False

		End Try


		Return success

	End Function


	Private Function DownloadNewConfigFile() As Boolean
		Dim success As Boolean = True

		Dim currentPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) ' Directory.GetCurrentDirectory()
		m_UpdateSettingFile = Path.Combine(currentPath, SETTING_FILE_NAME)
		m_Logger.LogDebug(String.Format("trying to download file from webserver: {0}", m_UpdateSettingFile))

		success = success AndAlso DownloadFile(URL_DOWNLOAD_SPS_FILE, m_UpdateSettingFile)
		If Not success Then
			m_Logger.LogError(String.Format("Die Einstellungsdatei {0} konnte nicht geladen werden.", m_UpdateSettingFile))
			AddLogData(String.Format("setting file could not be downloaded! {0}", m_UpdateSettingFile))
			m_UpdateSettingFile = String.Empty

			Return False
		End If

		Return success

	End Function

	Private Function DownloadFile(ByVal _url As String, ByVal _filename As String) As Boolean

		Try
			If File.Exists(_filename) Then File.Delete(_filename)
		Catch ex As Exception
			m_Logger.LogError(String.Format("deleting existing settingfile: {0} >>> {1}", _filename, ex.ToString))
		End Try

		Try
			Dim _webClient As New System.Net.WebClient
			_webClient.UseDefaultCredentials = True

			_webClient.DownloadFile(_url, _filename)
			Return IO.File.Exists(_filename)

		Catch ex As Exception
			m_Logger.LogError(String.Format("error while downloading file {0} to {1} : {2}", _url, _filename, ex.ToString()))
			AddLogData(String.Format("error while downloading file {0} to {1} : {2}", _url, _filename, ex.ToString()))

			Return False
		End Try

	End Function


	Private Function GetInternalIP() As String
		Dim strHostName As String

		strHostName = System.Net.Dns.GetHostName()

		Dim strIPAddress As String = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(Function(a As IPAddress) Not a.IsIPv6LinkLocal AndAlso Not a.IsIPv6Multicast AndAlso Not a.IsIPv6SiteLocal).First().ToString()

		Return strIPAddress

	End Function

	Private Function GetInternalDomainName() As String
		Dim strHostName As String

		strHostName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()

		Return strHostName

	End Function

	Private Function GetInternalHostName() As String
		Dim strHostName As String

		strHostName = System.Net.Dns.GetHostName()

		Return strHostName

	End Function

	Private Function GetExternalIP() As String
		Dim Response As String = String.Empty
		Dim lol As WebClient = New WebClient()
		Dim sourceURL As String = "http://checkip.dyndns.org/"
		Dim ExternalIP As String = String.Empty

		Try
			ExternalIP = (New WebClient()).DownloadString(sourceURL)

			'<html><head><title>Current IP Check</title></head><body>Current IP Address: 212.120.49.66</body></html>
			ExternalIP = (New Regex("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(ExternalIP)(0).ToString()

			Return ExternalIP

		Catch ex As Exception
			m_Logger.LogError(String.Format("Could not confirm External IP Address from {0} | {1}", sourceURL, ex.ToString))

			'Return String.Empty
		End Try

		Try

			If String.IsNullOrWhiteSpace(ExternalIP) Then
				sourceURL = "https://www.ip-adress.com/"
				Dim str As String = lol.DownloadString(sourceURL)
				Dim pattern As String = "<h2>My IP address is: (.+)</h2>"
				Dim pattern_new As String = "Your IP address is: <strong>(.+)</strong></h1>"
				Dim matches1 As MatchCollection = Regex.Matches(str, pattern_new)
				Dim ip As String = matches1(0).ToString
				ip = ip.Remove(0, 28)
				ip = ip.Replace("</strong></h1>", "")
				ip = ip.Replace("</strong>", "")

				ExternalIP = ip
			End If

			Return ExternalIP

		Catch ex As Exception
			m_Logger.LogError(String.Format("Could not confirm External IP Address from {0} | {1}", sourceURL, ex.ToString))

			Return String.Empty
		End Try

		Return Response

	End Function


	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = "Keine Daten wurden gefunden."

		Try
			s = s

		Catch ex As Exception


		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

	Private Sub AddLogData(ByVal msg As String)
		If String.IsNullOrWhiteSpace(msg) Then Return

		If Me.InvokeRequired = True Then
			Dim d As New StartLogingData(AddressOf AddLogData)
			Me.Invoke(d, New Object() {msg})

			'Me.Invoke(New StartLogingData(AddressOf AddLogData))
		Else

			Dim existData As List(Of EntryLOGData) = CType(grdLOG.DataSource, List(Of EntryLOGData))
			If existData Is Nothing Then
				existData = New List(Of EntryLOGData)
				existData = m_LogData

			Else
				If existData.Count > 50 Then
					grdLOG.DataSource = Nothing
					m_LogData = New List(Of EntryLOGData)
					m_LogData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Info", .Message = "restarting log..."})
					existData = m_LogData

				End If
			End If

			existData.Add(New EntryLOGData With {.LogDate = Now, .LogType = "Station-Update", .Message = msg})
			grdLOG.DataSource = existData
			grdLOG.RefreshDataSource()

		End If

	End Sub

	Private Sub ManageFileHistory(ByVal DestFile As FileInfo)
		Dim locFileToProcess As FileInfo

		Try
			locFileToProcess = New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, OPTION_HISTORYLLEVELS.ToString("0")))
			If locFileToProcess.Exists Then
				Try
					locFileToProcess.Delete()
				Catch ex As Exception
					m_Logger.LogError(String.Format("Deleting file: {0} | {1}", locFileToProcess.FullName, ex.ToString))
				End Try
			End If

			For locHistoryCount As Integer = OPTION_HISTORYLLEVELS - 1 To 1 Step -1

				locFileToProcess = New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, locHistoryCount.ToString("0")))
				Dim locFileInfo As New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, (locHistoryCount + 1).ToString("0")))

				If locFileToProcess.Exists Then
					Try
						My.Computer.FileSystem.RenameFile(locFileToProcess.FullName, locFileInfo.Name)

					Catch ex As Exception
						m_Logger.LogError(String.Format("RenameFile: {0} >>> {1} | {2}", locFileToProcess.FullName, locFileInfo.Name, ex.ToString))
					End Try
				End If
			Next

			Try
				My.Computer.FileSystem.RenameFile(DestFile.FullName, (New FileInfo(DestFile.ToString & ".DNCBackup1").Name))

			Catch ex As Exception
				m_Logger.LogError(String.Format("Rename old file: {0} >>> {1} | {2}", DestFile.FullName, DestFile.ToString & ".DNCBackup1", ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Function CreateTemporaryUpdateFolder() As Boolean
		Dim success As Boolean = True

		Dim tmpPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sputnik\SputnikClientUpdate")


		Try
			m_NewUpdateFilePath = Path.Combine(tmpPath, "ClientUpdateNewFiles")
			m_TempNETFolder = String.Format(FOLDER_DOWNLOAD_SPS_NET, tmpPath)
			m_TempDocumentFolder = String.Format(FOLDER_DOWNLOAD_SPS_DOCUMENT, tmpPath)
			m_TempQueryFolder = String.Format(FOLDER_DOWNLOAD_SPS_QUERY, tmpPath)
			m_TempTemplateFolder = String.Format(FOLDER_DOWNLOAD_SPS_TEMPLATE, tmpPath)

			If Not Directory.Exists(m_NewUpdateFilePath) Then Directory.CreateDirectory(m_NewUpdateFilePath) Else DeleteFilesFromFolder(m_NewUpdateFilePath)
			If Not Directory.Exists(m_TempNETFolder) Then Directory.CreateDirectory(m_TempNETFolder) Else DeleteFilesFromFolder(m_TempNETFolder)
			If Not Directory.Exists(m_TempDocumentFolder) Then Directory.CreateDirectory(m_TempDocumentFolder) Else DeleteFilesFromFolder(m_TempDocumentFolder)
			If Not Directory.Exists(m_TempQueryFolder) Then Directory.CreateDirectory(m_TempQueryFolder) Else DeleteFilesFromFolder(m_TempQueryFolder)
			If Not Directory.Exists(m_TempTemplateFolder) Then Directory.CreateDirectory(m_TempTemplateFolder) Else DeleteFilesFromFolder(m_TempTemplateFolder)

			m_Logger.LogInfo(String.Format("temp folder for downloads: {0}", tmpPath))
			m_Logger.LogInfo(String.Format("net folder on local machine: {0}", m_TempNETFolder))
			m_Logger.LogInfo(String.Format("document folder on local machine: {0}", m_TempDocumentFolder))
			m_Logger.LogInfo(String.Format("query folder on local machine: {0}", m_TempQueryFolder))
			m_Logger.LogInfo(String.Format("template folder on local machine: {0}", m_TempTemplateFolder))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False
		End Try


		Return success

	End Function

	Private Sub DeleteFilesFromFolder(Folder As String)

		If Directory.Exists(Folder) Then

			For Each _file As String In Directory.GetFiles(Folder)
				File.Delete(_file)
			Next
			For Each _folder As String In Directory.GetDirectories(Folder)
				DeleteFilesFromFolder(_folder)
			Next

		End If

	End Sub


#End Region

End Class


