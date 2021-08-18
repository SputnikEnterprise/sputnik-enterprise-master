

Imports System.Net

Imports DevExpress.XtraSplashScreen

Imports DevExpress.XtraGrid.Views.Base
Imports System.IO
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Security.AccessControl
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
'Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.UI
Imports System.Threading
Imports System.Threading.Tasks

Imports SPS.SYS.SrvFTPUpdate.SPUpdateUtilitiesService
Imports SPS.SYS.SrvFTPUpdate.DatabaseAccessBase
Imports SPS.SYS.SrvFTPUpdate.FTPUpdateDatabaseAccess
Imports SPS.SYS.SrvFTPUpdate.FTPUpdateData
'Imports SP.Infrastructure
Imports System.Text.RegularExpressions
Imports DevExpress.XtraEditors
Imports DevExpress.Utils
Imports DevExpress.XtraEditors.Controls

Public Class frmSrvFTPDownload


	Private Delegate Sub StartLoadingData()

#Region "private fieldes"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As Logging.ILogger = New Logging.Logger()

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_UpdateDatabaseAccess As FTPUpdateDatabaseAccess

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	'Private Property m_ExistsUpdateFileresult As FTPUpdateData
	'Private Property m_NeededUpdateFileresult As FTPUpdateData

	Private m_Mandantupdatedata As IEnumerable(Of UpdateMDData)

	Private m_ServerUpdatePath As String
	Private m_DoUpdate As Boolean
	Private m_CustomerID As String

	'''' <summary>
	'''' UI Utility functions.
	'''' </summary>
	'Private m_UtilityUI As UtilityUI
	Private m_Utility As Utility

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_UpdateWebServiceUri As String
	Private m_Timer As System.Timers.Timer
	Dim m_Continue As Boolean

	Private m_TempNETFolder As String
	Private m_MDYearFolder As String
	Private m_TempDocumentFolder As String
	Private m_TempQueryFolder As String
	Private m_TempTemplateFolder As String
	Private m_TempSkinFolder As String
	Private m_TempMDYearFolder As String

	Private m_NewUpdateFilePath As String
	Private m_NETFolder As String
	Private m_DocumentFolder As String
	Private m_QueryFolder As String

	Private m_NetFiles As List(Of String)
	Private m_DocumentFiles As List(Of String)
	Private m_QueryFiles As List(Of String)
	Private m_TemplateFiles As List(Of String)
	Private m_SkinFiles As List(Of String)
	Private m_MDYearFiles As List(Of String)
	Private m_StationData As StationData
	Private m_StartAsDeveloper As Boolean

	Private m_ExistsNewFTPFileVersion As Boolean

#End Region


#Region "private consts"

	Private Const URL_DOWNLOAD_SPS_FILE As String = "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
	Private Const FOLDER_DOWNLOAD_SPS_NET As String = "{0}\NET"
	Private Const FOLDER_DOWNLOAD_SPS_DOCUMENT As String = "{0}\DOCUMENTS"
	Private Const FOLDER_DOWNLOAD_SPS_QUERY As String = "{0}\QUERY"
	Private Const FOLDER_DOWNLOAD_SPS_TEMPLATE As String = "{0}\TEMPLATE"
	Private Const FOLDER_DOWNLOAD_SPS_SKIN As String = "{0}\TEMPLATE\SKINS"
	Private Const FOLDER_DOWNLOAD_SPS_MDYEAR As String = "{0}\{1}"

	Private Const OPTION_HISTORYLLEVELS As Integer = 5

#End Region


#Region "public properties"

	Public Property AUTOSTART As Boolean

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
		m_SkinFiles = New List(Of String)
		m_MDYearFiles = New List(Of String)
		m_UpdateWebServiceUri = "http://asmx.domain.com/wsSPS_services/SPUpdateUtilities.asmx"
		m_StationData = New StationData

		Dim ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(Function(a As IPAddress) Not a.IsIPv6LinkLocal AndAlso Not a.IsIPv6Multicast AndAlso Not a.IsIPv6SiteLocal).First().ToString()
		m_StartAsDeveloper = ipAddress.StartsWith("ipaddress")

		InitializeComponent()

		m_ServerUpdatePath = Path.Combine(Directory.GetCurrentDirectory, "ProgUpdatesetting.sps")

		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Text
		WindowsFormsSettings.AllowAutoFilterConditionChange = DefaultBoolean.False
		m_Mandantupdatedata = New BindingList(Of UpdateMDData)

		Dim connectionString As String = String.Empty ' My.Settings.dbSelectConn
		m_UpdateDatabaseAccess = New FTPUpdateDatabaseAccess(connectionString, Language.German)
		m_Logger.LogDebug(String.Format("m_UpdateDatabaseAccess: {0}", connectionString))


		m_StationData.LocalHostName = GetInternalHostName()
		m_StationData.LocalIPAddress = GetInternalIP()
		m_StationData.LocalDomainName = GetInternalDomainName()
		m_StationData.ExternalIPAddress = GetExternalIP()


		Reset()
		lblDestPath.Visible = m_StartAsDeveloper
		lueDestPath.Visible = m_StartAsDeveloper
		xtabProgModules.PageEnabled = m_StartAsDeveloper

		grdFTPUpdateFiles.AllowDrop = m_StartAsDeveloper
		grdClientUpdateFiles.AllowDrop = m_StartAsDeveloper
		grdUpdates.AllowDrop = m_StartAsDeveloper

		btnLoadData.Enabled = m_Continue
		btnDownloadData.Enabled = m_Continue

		AddHandler Me.gvUpdates.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvNew.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub

#End Region

#Region "private properties"

	''' <summary>
	''' Gets the selected update record to install.
	''' </summary>
	Public ReadOnly Property SelectedUpdateRecord As FTPUpdateData
		Get
			Dim gvRP = TryCast(grdNew.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvNew.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FTPUpdateData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "Public methodes"


	Public Sub ProcessAutoUpdate()
		PerformProcessUpdate()
	End Sub

	Public Sub ProcessAutoLoad()
		m_Logger.LogDebug(String.Format("entring ProcessAutoLoad"))

		LoadData()
		m_Logger.LogDebug(String.Format("finishing LoadData"))
	End Sub


#End Region


	Private Sub PerformProcessUpdate()

		m_Logger.LogDebug(String.Format("entring PerformProcessUpdate"))
		LoadData()
		m_Logger.LogDebug(String.Format("finishing LoadData"))

		StartWithDownload()
		m_Logger.LogDebug(String.Format("finishing StartWithDownload"))

	End Sub

	Private Sub Reset()
		Dim success As Boolean = True

		m_ExistsNewFTPFileVersion = False
		hlNewProgramVersion.Visible = False

		ResetUpdateGrid()
		ReseInstalledGrid()
		ReseNewGrid()
		ResetProgramModulesGrid()
		ResetClientProgramModulesGrid()

		ResetDestPathDropDown()

		success = success AndAlso CreateTemporaryUpdateFolder()


		m_Continue = success

	End Sub


	''' <summary>
	''' Resets the updates overview grid.
	''' </summary>
	Private Sub ResetUpdateGrid()

		' Reset the grid
		gvUpdates.OptionsView.ShowIndicator = False
		gvUpdates.OptionsView.ColumnAutoWidth = True
		gvUpdates.OptionsView.ShowAutoFilterRow = True

		gvUpdates.Columns.Clear()

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESNr.Caption = ("UpdateID")
		columnESNr.Name = "UpdateID"
		columnESNr.FieldName = "UpdateID"
		columnESNr.Visible = False
		gvUpdates.Columns.Add(columnESNr)

		Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerName.Caption = "UpdateFilename"
		columnCustomerName.Name = "UpdateFilename"
		columnCustomerName.FieldName = "UpdateFilename"
		columnCustomerName.Visible = True
		columnCustomerName.Width = 150
		gvUpdates.Columns.Add(columnCustomerName)

		Dim columnESFromTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESFromTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESFromTo.Caption = "FileDestPath"
		columnESFromTo.Name = "FileDestPath"
		columnESFromTo.FieldName = "FileDestPath"
		columnESFromTo.Visible = True
		columnESFromTo.Width = 150
		gvUpdates.Columns.Add(columnESFromTo)

		Dim columnGrundLohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGrundLohn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGrundLohn.Caption = "FileDestVersion"
		columnGrundLohn.Name = "FileDestVersion"
		columnGrundLohn.FieldName = "FileDestVersion"
		columnGrundLohn.Visible = True
		gvUpdates.Columns.Add(columnGrundLohn)

		Dim columnFerienProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFerienProz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFerienProz.Caption = "UpdateFileDate"
		columnFerienProz.Name = "UpdateFileDate"
		columnFerienProz.FieldName = "UpdateFileDate"
		columnFerienProz.Visible = True
		columnFerienProz.Width = 80
		gvUpdates.Columns.Add(columnFerienProz)

		Dim columnFeierProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFeierProz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFeierProz.Caption = "UpdateFileTime"
		columnFeierProz.Name = "UpdateFileTime"
		columnFeierProz.FieldName = "UpdateFileTime"
		columnFeierProz.Visible = True
		columnFeierProz.Width = 80
		gvUpdates.Columns.Add(columnFeierProz)

		Dim columnLohn13Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLohn13Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLohn13Proz.Caption = "UpdateFileSize"
		columnLohn13Proz.Name = "UpdateFileSize"
		columnLohn13Proz.FieldName = "UpdateFileSize"
		columnLohn13Proz.Visible = True
		columnLohn13Proz.Width = 80
		gvUpdates.Columns.Add(columnLohn13Proz)

		Dim columnGAVData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVData.Caption = "File_Guid"
		columnGAVData.Name = "File_Guid"
		columnGAVData.FieldName = "File_Guid"
		columnGAVData.Visible = True
		columnGAVData.Width = 200
		gvUpdates.Columns.Add(columnGAVData)

		Dim columnFileDestinationEnumValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestinationEnumValue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestinationEnumValue.Caption = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Name = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.FieldName = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Visible = True
		columnFileDestinationEnumValue.Width = 200
		gvUpdates.Columns.Add(columnFileDestinationEnumValue)


		grdUpdates.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the installed overview grid.
	''' </summary>
	Private Sub ReseInstalledGrid()

		' Reset the grid
		gvInstalled.OptionsView.ShowIndicator = False
		gvInstalled.OptionsView.ColumnAutoWidth = True
		gvInstalled.OptionsView.ShowAutoFilterRow = True

		gvInstalled.Columns.Clear()

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESNr.Caption = ("UpdateID")
		columnESNr.Name = "UpdateID"
		columnESNr.FieldName = "UpdateID"
		columnESNr.Visible = False
		gvInstalled.Columns.Add(columnESNr)

		Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerName.Caption = "UpdateFilename"
		columnCustomerName.Name = "UpdateFilename"
		columnCustomerName.FieldName = "UpdateFilename"
		columnCustomerName.Visible = True
		columnCustomerName.Width = 150
		gvInstalled.Columns.Add(columnCustomerName)

		Dim columnESFromTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESFromTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESFromTo.Caption = "FileDestPath"
		columnESFromTo.Name = "FileDestPath"
		columnESFromTo.FieldName = "FileDestPath"
		columnESFromTo.Visible = True
		columnESFromTo.Width = 150
		gvInstalled.Columns.Add(columnESFromTo)

		Dim columnGrundLohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGrundLohn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGrundLohn.Caption = "FileDestVersion"
		columnGrundLohn.Name = "FileDestVersion"
		columnGrundLohn.FieldName = "FileDestVersion"
		columnGrundLohn.Visible = True
		gvInstalled.Columns.Add(columnGrundLohn)

		Dim columnFerienProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFerienProz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFerienProz.Caption = "UpdateFileDate"
		columnFerienProz.Name = "UpdateFileDate"
		columnFerienProz.FieldName = "UpdateFileDate"
		columnFerienProz.Visible = True
		columnFerienProz.Width = 80
		gvInstalled.Columns.Add(columnFerienProz)

		Dim columnFeierProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFeierProz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFeierProz.Caption = "UpdateFileTime"
		columnFeierProz.Name = "UpdateFileTime"
		columnFeierProz.FieldName = "UpdateFileTime"
		columnFeierProz.Visible = True
		columnFeierProz.Width = 80
		gvInstalled.Columns.Add(columnFeierProz)

		Dim columnLohn13Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLohn13Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLohn13Proz.Caption = "UpdateFileSize"
		columnLohn13Proz.Name = "UpdateFileSize"
		columnLohn13Proz.FieldName = "UpdateFileSize"
		columnLohn13Proz.Visible = True
		columnLohn13Proz.Width = 80
		gvInstalled.Columns.Add(columnLohn13Proz)

		Dim columnGAVData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVData.Caption = "File_Guid"
		columnGAVData.Name = "File_Guid"
		columnGAVData.FieldName = "File_Guid"
		columnGAVData.Visible = True
		columnGAVData.Width = 200
		gvInstalled.Columns.Add(columnGAVData)

		Dim columnFileDestinationEnumValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestinationEnumValue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestinationEnumValue.Caption = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Name = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.FieldName = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Visible = True
		columnFileDestinationEnumValue.Width = 200
		gvInstalled.Columns.Add(columnFileDestinationEnumValue)


		grdInstalled.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the new overview grid.
	''' </summary>
	Private Sub ReseNewGrid()

		' Reset the grid
		gvNew.OptionsView.ShowIndicator = False
		gvNew.OptionsView.ColumnAutoWidth = True
		gvNew.OptionsView.ShowAutoFilterRow = True

		gvNew.Columns.Clear()

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESNr.Caption = ("UpdateID")
		columnESNr.Name = "UpdateID"
		columnESNr.FieldName = "UpdateID"
		columnESNr.Visible = False
		gvNew.Columns.Add(columnESNr)

		Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerName.Caption = "UpdateFilename"
		columnCustomerName.Name = "UpdateFilename"
		columnCustomerName.FieldName = "UpdateFilename"
		columnCustomerName.Visible = True
		columnCustomerName.Width = 150
		gvNew.Columns.Add(columnCustomerName)

		Dim columnESFromTo As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESFromTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESFromTo.Caption = "FileDestPath"
		columnESFromTo.Name = "FileDestPath"
		columnESFromTo.FieldName = "FileDestPath"
		columnESFromTo.Visible = True
		columnESFromTo.Width = 150
		gvNew.Columns.Add(columnESFromTo)

		Dim columnGrundLohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGrundLohn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGrundLohn.Caption = "FileDestVersion"
		columnGrundLohn.Name = "FileDestVersion"
		columnGrundLohn.FieldName = "FileDestVersion"
		columnGrundLohn.Visible = True
		gvNew.Columns.Add(columnGrundLohn)

		Dim columnFerienProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFerienProz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFerienProz.Caption = "UpdateFileDate"
		columnFerienProz.Name = "UpdateFileDate"
		columnFerienProz.FieldName = "UpdateFileDate"
		columnFerienProz.Visible = True
		columnFerienProz.Width = 80
		gvNew.Columns.Add(columnFerienProz)

		Dim columnFeierProz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFeierProz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFeierProz.Caption = "UpdateFileTime"
		columnFeierProz.Name = "UpdateFileTime"
		columnFeierProz.FieldName = "UpdateFileTime"
		columnFeierProz.Visible = True
		columnFeierProz.Width = 80
		gvNew.Columns.Add(columnFeierProz)

		Dim columnLohn13Proz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLohn13Proz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLohn13Proz.Caption = "UpdateFileSize"
		columnLohn13Proz.Name = "UpdateFileSize"
		columnLohn13Proz.FieldName = "UpdateFileSize"
		columnLohn13Proz.Visible = True
		columnLohn13Proz.Width = 80
		gvNew.Columns.Add(columnLohn13Proz)

		Dim columnGAVData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGAVData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnGAVData.Caption = "File_Guid"
		columnGAVData.Name = "File_Guid"
		columnGAVData.FieldName = "File_Guid"
		columnGAVData.Visible = True
		columnGAVData.Width = 200
		gvNew.Columns.Add(columnGAVData)

		Dim columnFileDestinationEnumValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestinationEnumValue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestinationEnumValue.Caption = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Name = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.FieldName = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Visible = True
		columnFileDestinationEnumValue.Width = 200
		gvNew.Columns.Add(columnFileDestinationEnumValue)


		grdNew.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the program modules overview grid.
	''' </summary>
	Private Sub ResetProgramModulesGrid()

		' Reset the grid
		gvFTPUpdateFiles.OptionsView.ShowIndicator = False
		gvFTPUpdateFiles.OptionsView.ColumnAutoWidth = True
		gvFTPUpdateFiles.OptionsView.ShowAutoFilterRow = True

		gvFTPUpdateFiles.Columns.Clear()

		Dim columnUpdateID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateID.Caption = ("UpdateID")
		columnUpdateID.Name = "UpdateID"
		columnUpdateID.FieldName = "UpdateID"
		columnUpdateID.Visible = True
		gvFTPUpdateFiles.Columns.Add(columnUpdateID)

		Dim columnUpdateFilename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFilename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFilename.Caption = "UpdateFilename"
		columnUpdateFilename.Name = "UpdateFilename"
		columnUpdateFilename.FieldName = "UpdateFilename"
		columnUpdateFilename.Visible = True
		columnUpdateFilename.Width = 150
		gvFTPUpdateFiles.Columns.Add(columnUpdateFilename)

		Dim columnFileDestPath As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestPath.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestPath.Caption = "FileDestPath"
		columnFileDestPath.Name = "FileDestPath"
		columnFileDestPath.FieldName = "FileDestPath"
		columnFileDestPath.Visible = True
		columnFileDestPath.Width = 150
		gvUpdates.Columns.Add(columnFileDestPath)

		Dim columnFileDestVersion As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestVersion.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestVersion.Caption = "FileDestVersion"
		columnFileDestVersion.Name = "FileDestVersion"
		columnFileDestVersion.FieldName = "FileDestVersion"
		columnFileDestVersion.Visible = True
		gvFTPUpdateFiles.Columns.Add(columnFileDestVersion)

		Dim columnUpdateFileDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFileDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFileDate.Caption = "UpdateFileDate"
		columnUpdateFileDate.Name = "UpdateFileDate"
		columnUpdateFileDate.FieldName = "UpdateFileDate"
		columnUpdateFileDate.Visible = True
		columnUpdateFileDate.Width = 80
		gvFTPUpdateFiles.Columns.Add(columnUpdateFileDate)

		Dim columnUpdateFileTime As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFileTime.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFileTime.Caption = "UpdateFileTime"
		columnUpdateFileTime.Name = "UpdateFileTime"
		columnUpdateFileTime.FieldName = "UpdateFileTime"
		columnUpdateFileTime.Visible = True
		columnUpdateFileTime.Width = 80
		gvFTPUpdateFiles.Columns.Add(columnUpdateFileTime)

		Dim columnUpdateFileSize As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFileSize.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFileSize.Caption = "UpdateFileSize"
		columnUpdateFileSize.Name = "UpdateFileSize"
		columnUpdateFileSize.FieldName = "UpdateFileSize"
		columnUpdateFileSize.Visible = True
		columnUpdateFileSize.Width = 80
		gvFTPUpdateFiles.Columns.Add(columnUpdateFileSize)

		Dim columnFile_Guid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFile_Guid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFile_Guid.Caption = "File_Guid"
		columnFile_Guid.Name = "File_Guid"
		columnFile_Guid.FieldName = "File_Guid"
		columnFile_Guid.Visible = True
		columnFile_Guid.Width = 200
		gvFTPUpdateFiles.Columns.Add(columnFile_Guid)

		Dim columnFileDestinationEnumValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestinationEnumValue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestinationEnumValue.Caption = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Name = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.FieldName = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Visible = True
		columnFileDestinationEnumValue.Width = 200
		gvFTPUpdateFiles.Columns.Add(columnFileDestinationEnumValue)


		grdFTPUpdateFiles.DataSource = Nothing

	End Sub

	''' <summary>
	''' Resets the client program modules overview grid.
	''' </summary>
	Private Sub ResetClientProgramModulesGrid()

		' Reset the grid
		gvClientUpdateFiles.OptionsView.ShowIndicator = False
		gvClientUpdateFiles.OptionsView.ColumnAutoWidth = True
		gvClientUpdateFiles.OptionsView.ShowAutoFilterRow = True

		gvClientUpdateFiles.Columns.Clear()

		Dim columnUpdateID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateID.Caption = ("UpdateID")
		columnUpdateID.Name = "UpdateID"
		columnUpdateID.FieldName = "UpdateID"
		columnUpdateID.Visible = True
		gvClientUpdateFiles.Columns.Add(columnUpdateID)

		Dim columnUpdateFilename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFilename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFilename.Caption = "UpdateFilename"
		columnUpdateFilename.Name = "UpdateFilename"
		columnUpdateFilename.FieldName = "UpdateFilename"
		columnUpdateFilename.Visible = True
		columnUpdateFilename.Width = 150
		gvClientUpdateFiles.Columns.Add(columnUpdateFilename)

		Dim columnFileDestPath As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestPath.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestPath.Caption = "FileDestPath"
		columnFileDestPath.Name = "FileDestPath"
		columnFileDestPath.FieldName = "FileDestPath"
		columnFileDestPath.Visible = True
		columnFileDestPath.Width = 150
		gvUpdates.Columns.Add(columnFileDestPath)

		Dim columnFileDestVersion As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestVersion.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestVersion.Caption = "FileDestVersion"
		columnFileDestVersion.Name = "FileDestVersion"
		columnFileDestVersion.FieldName = "FileDestVersion"
		columnFileDestVersion.Visible = True
		gvClientUpdateFiles.Columns.Add(columnFileDestVersion)

		Dim columnUpdateFileDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFileDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFileDate.Caption = "UpdateFileDate"
		columnUpdateFileDate.Name = "UpdateFileDate"
		columnUpdateFileDate.FieldName = "UpdateFileDate"
		columnUpdateFileDate.Visible = True
		columnUpdateFileDate.Width = 80
		gvClientUpdateFiles.Columns.Add(columnUpdateFileDate)

		Dim columnUpdateFileTime As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFileTime.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFileTime.Caption = "UpdateFileTime"
		columnUpdateFileTime.Name = "UpdateFileTime"
		columnUpdateFileTime.FieldName = "UpdateFileTime"
		columnUpdateFileTime.Visible = True
		columnUpdateFileTime.Width = 80
		gvClientUpdateFiles.Columns.Add(columnUpdateFileTime)

		Dim columnUpdateFileSize As New DevExpress.XtraGrid.Columns.GridColumn()
		columnUpdateFileSize.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnUpdateFileSize.Caption = "UpdateFileSize"
		columnUpdateFileSize.Name = "UpdateFileSize"
		columnUpdateFileSize.FieldName = "UpdateFileSize"
		columnUpdateFileSize.Visible = True
		columnUpdateFileSize.Width = 80
		gvClientUpdateFiles.Columns.Add(columnUpdateFileSize)

		Dim columnFile_Guid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFile_Guid.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFile_Guid.Caption = "File_Guid"
		columnFile_Guid.Name = "File_Guid"
		columnFile_Guid.FieldName = "File_Guid"
		columnFile_Guid.Visible = True
		columnFile_Guid.Width = 200
		gvClientUpdateFiles.Columns.Add(columnFile_Guid)

		Dim columnFileDestinationEnumValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileDestinationEnumValue.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileDestinationEnumValue.Caption = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Name = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.FieldName = "FileDestinationEnumValue"
		columnFileDestinationEnumValue.Visible = True
		columnFileDestinationEnumValue.Width = 200
		gvClientUpdateFiles.Columns.Add(columnFileDestinationEnumValue)


		grdClientUpdateFiles.DataSource = Nothing

	End Sub

	Private Sub ResetDestPathDropDown()
		lueDestPath.Properties.DisplayMember = "Description"
		lueDestPath.Properties.ValueMember = "Value"

		lueDestPath.Properties.Columns.Clear()
		lueDestPath.Properties.Columns.Add(New LookUpColumnInfo("Description", 0))
		lueDestPath.EditValue = Nothing
	End Sub

	Private Function LoadDestPathDrowpDownData() As Boolean
		Dim destPathList = New List(Of DestPathData)

		destPathList.Add(New DestPathData With {.Value = 0, .Description = DestPathEnum.NET})
		destPathList.Add(New DestPathData With {.Value = 1, .Description = DestPathEnum.QUERY})
		destPathList.Add(New DestPathData With {.Value = 2, .Description = DestPathEnum.DOCUMENTS})
		destPathList.Add(New DestPathData With {.Value = 3, .Description = DestPathEnum.MDYEAR})
		destPathList.Add(New DestPathData With {.Value = 4, .Description = DestPathEnum.PROFILES})
		destPathList.Add(New DestPathData With {.Value = 5, .Description = DestPathEnum.SKINS})
		destPathList.Add(New DestPathData With {.Value = 6, .Description = DestPathEnum.TEMPLATES})

		lueDestPath.Properties.DataSource = destPathList
		lueDestPath.Properties.DropDownRows = destPathList.Count


		Return True

	End Function

	Private Sub LoadData()
		Dim success As Boolean = True

		m_NetFiles.Clear()
		m_DocumentFiles.Clear()
		m_QueryFiles.Clear()
		m_TemplateFiles.Clear()
		m_SkinFiles.Clear()
		m_MDYearFiles.Clear()

		If Me.InvokeRequired = True Then
			Me.Invoke(New StartLoadingData(AddressOf LoadData))

			Return
		End If
		If m_ExistsNewFTPFileVersion Then Return
		m_Logger.LogDebug(String.Format("entring LoadData"))
		LoadDestPathDrowpDownData()

		grdUpdates.DataSource = Nothing
		grdInstalled.DataSource = Nothing
		grdNew.DataSource = Nothing
		grdFTPUpdateFiles.DataSource = Nothing
		grdClientUpdateFiles.DataSource = Nothing

		Try
			If Not AUTOSTART Then
				SplashScreenManager.CloseForm(False)
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
				SplashScreenManager.Default.SetWaitFormCaption(("Ihre Daten werden abgerufen") & Space(20))
				SplashScreenManager.Default.SetWaitFormDescription("Dies kann einige Sekunden dauern" & "...")
			End If

			success = success AndAlso LoadMandantDataForUpdate()
			m_Logger.LogDebug(String.Format("mandant data is loaded: {0}", success))
			If Not success Then m_Logger.LogDebug(String.Format("update is not possible because no mandant data was founded!!!", success))
			btnDownloadData.Enabled = success

			Dim existData = PerformCurrentlistWebserviceCallAsync()
			grdUpdates.DataSource = existData

			m_Logger.LogDebug(String.Format("current update files: {0}", existData.Count))

			Dim installedData = m_UpdateDatabaseAccess.LoadUploadedData
			grdInstalled.DataSource = installedData
			m_Logger.LogDebug(String.Format("file for installation: {0}", installedData.Count))

			'Dim newData As List(Of FTPUpdateData) = existData.Select(Function(x) x.File_Guid).Except(installedData.Select(Function(g) g.File_Guid))
			Dim listDataSource As BindingList(Of FTPUpdateData) = New BindingList(Of FTPUpdateData)

			For Each result In existData

				Dim newData = installedData.Where(Function(data) data.File_Guid = result.File_Guid).FirstOrDefault

				If newData Is Nothing Then
					listDataSource.Add(result)
				End If

			Next
			m_Logger.LogDebug(String.Format("update files: {0}", listDataSource.Count))

			grdNew.DataSource = listDataSource

			'success = success AndAlso StartWithDownloading()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			If Not AUTOSTART Then SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Private Sub CheckForNewProgramUpdates()
		Dim success As Boolean = True
		Dim ftpModuleFileData = PerformRequiredFTPSetupFilesWebserviceCallAsync()

		m_Logger.LogDebug(String.Format("searching new program version: {0}", ftpModuleFileData.Count))

		grdFTPUpdateFiles.DataSource = ftpModuleFileData
		If Not ftpModuleFileData Is Nothing AndAlso ftpModuleFileData.Count > 0 Then
			Dim updateAssembly As FileInfo = New FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)
			Dim newData = ftpModuleFileData.Where(Function(data) data.FileDestPath = "FTP" AndAlso data.UpdateFilename = updateAssembly.Name).FirstOrDefault
			If Not newData Is Nothing Then
				Dim newFileDate As DateTime = New DateTime(CDate(newData.UpdateFileDate).Year, CDate(newData.UpdateFileDate).Month, CDate(newData.UpdateFileDate).Day, CDate(newData.UpdateFileTime).Hour, CDate(newData.UpdateFileTime).Minute, CDate(newData.UpdateFileTime).Second, 0)

				If newFileDate <> updateAssembly.LastWriteTime Then

					m_ExistsNewFTPFileVersion = True
					m_Logger.LogWarning(String.Format("there is new program file to be updated! newFileDate: {0} | updateAssembly: {1}", newFileDate, updateAssembly.LastWriteTime))
					Dim newMessage As String = "Bitte ersetzen Sie die neuen Dateien aus</br>{0}</br>durch</br>{1}"
					hlNewProgramVersion.Text = String.Format(newMessage, m_NewUpdateFilePath, Directory.GetCurrentDirectory)

					hlNewProgramVersion.Visible = True
					success = success AndAlso SaveProgramModuleFile("FTP", ftpModuleFileData)
					PerformSendingNewUpdateNotificationWebservice()

					Return
				End If
			End If
		End If

		Dim ClientModuleFileData = PerformRequiredClientSetupFilesWebserviceCallAsync()
		grdClientUpdateFiles.DataSource = ClientModuleFileData

		m_Logger.LogDebug(String.Format("finished searching program version: {0}", ClientModuleFileData.Count))

	End Sub

	Private Function LoadMandantDataForUpdate() As Boolean
		Dim success As Boolean = True

		m_Mandantupdatedata = m_UpdateDatabaseAccess.LoadUpdateMandantData
		If m_Mandantupdatedata Is Nothing Then
			m_Logger.LogWarning("could not find any data from mandant table in dbselect database!")

			Return False
		ElseIf m_Mandantupdatedata.Count = 0 Then
			Return False

		End If


		Return (Not m_Mandantupdatedata Is Nothing)

	End Function

	Private Function StartWithDownload() As Boolean
		Dim success As Boolean = True
		Dim newData As BindingList(Of FTPUpdateData) = grdNew.DataSource
		Dim selectedRec = SelectedUpdateRecord
		Dim IsUpdateAllowed As Boolean = True
		Dim downloadIsOk As Boolean = True

		m_NetFiles.Clear()
		m_DocumentFiles.Clear()
		m_QueryFiles.Clear()
		m_TemplateFiles.Clear()
		m_SkinFiles.Clear()
		m_MDYearFiles.Clear()

		If newData Is Nothing OrElse m_ExistsNewFTPFileVersion Then Return False

		If Not AUTOSTART Then
			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(("Ihre Daten werden abgerufen") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription("Dies kann einige Sekunden dauern" & "...")
		End If
		m_Logger.LogDebug(String.Format("entring StartWithDownload"))

		If chkSimulateUpload.Checked Then m_Logger.LogInfo(String.Format("simulating upload..."))
		If Not newData Is Nothing AndAlso newData.Count > 0 Then

			If xTab.SelectedTabPage Is xtabNew Then
				Dim data = PerformDownloadFileWebserviceCallAsync(selectedRec.UpdateID)
				If Not data Is Nothing Then
					downloadIsOk = downloadIsOk AndAlso SaveDownloadFile(data)
				End If

			Else
				For Each itm In newData
					Dim data = PerformDownloadFileWebserviceCallAsync(itm.UpdateID)
					If Not data Is Nothing Then
						downloadIsOk = downloadIsOk AndAlso SaveDownloadFile(data)
					End If
				Next

			End If

		End If

		If (m_Mandantupdatedata Is Nothing OrElse m_Mandantupdatedata.Count = 0) Then
			m_Logger.LogError(String.Format("no md data was founded!"))
			Return False
		End If

		If Not chkSimulateUpload.Checked AndAlso Not (m_DocumentFiles.Count = 0 AndAlso m_QueryFiles.Count = 0 AndAlso m_TemplateFiles.Count = 0 AndAlso m_SkinFiles.Count = 0 AndAlso m_MDYearFiles.Count = 0) Then
			For Each mandant In m_Mandantupdatedata
				m_CustomerID = mandant.Customer_id
				m_Logger.LogInfo(String.Format("customer_id: {0} >>> dbName: {1}", mandant.Customer_id, mandant.DbName))

				IsUpdateAllowed = PerformIsAllowedMandantUpdateWebserviceCallAsync(m_CustomerID)

				If IsUpdateAllowed Then
					If m_DocumentFiles.Count > 0 Then
						m_Logger.LogInfo(String.Format("doing with documents, count: {0}", m_DocumentFiles.Count))
						success = success AndAlso CopyDocumentsToMandantFolder(mandant.MDPath)
					End If
					If m_TemplateFiles.Count > 0 Then
						m_Logger.LogInfo(String.Format("doing with template, count: {0}", m_TemplateFiles.Count))
						success = success AndAlso CopyTemplatesToMandantFolder(mandant.MDPath)
					End If
					If m_SkinFiles.Count > 0 Then
						m_Logger.LogInfo(String.Format("doing with skin, count: {0}", m_SkinFiles.Count))
						success = success AndAlso CopySkinToMandantFolder(mandant.MDPath)
					End If
					If m_MDYearFiles.Count > 0 Then
						m_Logger.LogInfo(String.Format("doing with mdyear, count: {0}", m_MDYearFiles.Count))
						success = success AndAlso CopymdyearToMandantFolder(mandant.MDPath)
					End If


					If m_QueryFiles.Count > 0 Then
						m_Logger.LogInfo(String.Format("doing with query, count: {0}", m_QueryFiles.Count))
						success = success AndAlso UpdateMandantDb(mandant.MDPath, mandant.DbConnectionstr)
					End If


				Else
					m_Logger.LogWarning(String.Format("update is not allowed: {0}", m_CustomerID))

				End If

			Next

		End If

		If success AndAlso IsUpdateAllowed Then
			success = success AndAlso UpdateLocalDatabase()
		End If
		If success Then LoadData()


		Return success

	End Function

	Private Function SaveDownloadFile(ByVal data As FTPUpdateData) As Boolean
		Dim success As Boolean = True
		Dim spsUpdatePath As String = String.Empty

		If data Is Nothing Then Return False
		Dim bytes() = data.FileContent

		Dim tempFileName As String = String.Empty

		Select Case data.FileDestinationEnumValue
			Case FileDestEnum.NET
				tempFileName = Path.Combine(m_TempNETFolder, data.UpdateFilename)
				m_NetFiles.Add(tempFileName)
				spsUpdatePath = m_NETFolder

			Case FileDestEnum.DOCUMENT
				tempFileName = Path.Combine(m_TempDocumentFolder, data.UpdateFilename)
				m_DocumentFiles.Add(tempFileName)
				spsUpdatePath = m_DocumentFolder

			Case FileDestEnum.QUERY
				tempFileName = Path.Combine(m_TempQueryFolder, data.UpdateFilename)
				m_QueryFiles.Add(tempFileName)
				spsUpdatePath = m_QueryFolder

			Case FileDestEnum.TEMPLATE
				tempFileName = Path.Combine(m_TempTemplateFolder, data.UpdateFilename)
				m_TemplateFiles.Add(tempFileName)

			Case FileDestEnum.SKIN
				tempFileName = Path.Combine(m_TempSkinFolder, data.UpdateFilename)
				m_skinFiles.Add(tempFileName)

			Case FileDestEnum.MDYEAR
				tempFileName = Path.Combine(m_TempMDYearFolder, data.UpdateFilename)
				m_MDYearFiles.Add(tempFileName)

			Case Else
				Return False

		End Select

		If Not chkSimulateUpload.Checked Then
			If (Not bytes Is Nothing) Then
				m_Logger.LogDebug(String.Format("downloading file: {0}", tempFileName))

				If Not m_Utility.WriteFileBytes(tempFileName, bytes) Then
					m_Logger.LogError(String.Format("File {0} could not be downloaded!", tempFileName))
					success = False

					Return False
				End If
			End If

			success = success AndAlso ChangeFileAttribute(tempFileName, data)
			If Not success Then Return success

			Try

				If Not String.IsNullOrWhiteSpace(spsUpdatePath) Then
					m_Logger.LogDebug(String.Format("finished with download and copying file: {0} >>> {1}", tempFileName, Path.Combine(spsUpdatePath, data.UpdateFilename)))

					File.Copy(tempFileName, Path.Combine(spsUpdatePath, data.UpdateFilename), True)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("error during copying file! {0}", tempFileName))
				Return False
			End Try
		Else
			m_Logger.LogInfo(String.Format("download is simulated..."))
		End If


		Return success

	End Function

	Private Function SaveProgramModuleFile(ByVal moduleName As String, ByVal data As BindingList(Of FTPUpdateData)) As Boolean
		Dim success As Boolean = True
		Dim existsNewFileVersion As Boolean = False
		Dim tempFileName As String = String.Empty

		If data Is Nothing Then Return False
		Try

			If moduleName = "FTP" Then
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

			Else

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

			Return False
		End Try


		Return success

	End Function

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

	Private Function CopyDocumentsToMandantFolder(ByVal mandantPath As String) As Boolean
		Dim success As Boolean = True
		Dim destPath = Path.Combine(mandantPath, "Documents")

		Try

			' Create the directory if it doesn't exist
			If Not Directory.Exists(destPath) Then Directory.CreateDirectory(destPath)

			For Each document In m_DocumentFiles
				'#If release Then
				If File.Exists(document) Then
					m_Logger.LogDebug(String.Format("copying document file: {0} >>> {1}", document, Path.Combine(destPath, IO.Path.GetFileName(document))))
					File.Copy(document, Path.Combine(destPath, IO.Path.GetFileName(document)), True)
				Else
					m_Logger.LogError(String.Format("file not founded! {0}", document))

				End If
				'#End If

			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("error during coping document: {0} >>> {1}: {2}", mandantPath, destPath, ex.ToString))
			Return False

		End Try


		Return success

	End Function

	Private Function CopyTemplatesToMandantFolder(ByVal mandantPath As String) As Boolean
		Dim success As Boolean = True
		Dim destPath = Path.Combine(mandantPath, "Templates")

		Try

			' Create the directory if it doesn't exist
			If Not Directory.Exists(destPath) Then Directory.CreateDirectory(destPath)

			For Each template In m_TemplateFiles

				'#If release Then
				If File.Exists(template) Then
					m_Logger.LogDebug(String.Format("copying template file: {0} >>> {1}", template, Path.Combine(destPath, IO.Path.GetFileName(template))))
					File.Copy(template, Path.Combine(destPath, IO.Path.GetFileName(template)), True)
				Else
					m_Logger.LogError(String.Format("file not founded! {0}", template))
				End If
				'#End If

			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("error during coping templates: {0} >>> {1}: {2}", mandantPath, destPath, ex.ToString))
			Return False

		End Try


		Return success

	End Function

	Private Function CopySkinToMandantFolder(ByVal mandantPath As String) As Boolean
		Dim success As Boolean = True
		Dim destPath = Path.Combine(mandantPath, "Templates", "Skins")

		Try

			' Create the directory if it doesn't exist
			If Not Directory.Exists(destPath) Then Directory.CreateDirectory(destPath)

			For Each document In m_SkinFiles

				If File.Exists(document) Then
					m_Logger.LogDebug(String.Format("copying skin data file: {0} >>> {1}", document, Path.Combine(destPath, IO.Path.GetFileName(document))))
					File.Copy(document, Path.Combine(destPath, IO.Path.GetFileName(document)), True)
				Else
					m_Logger.LogError(String.Format("file not founded! {0}", document))

				End If

			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("error during coping skin data: {0} >>> {1}: {2}", mandantPath, destPath, ex.ToString))
			Return False

		End Try


		Return success

	End Function

	Private Function CopyMDYearToMandantFolder(ByVal mandantPath As String) As Boolean
		Dim success As Boolean = True
		Dim destPath = Path.Combine(mandantPath, Now.Year)

		Try

			' Create the directory if it doesn't exist
			If Not Directory.Exists(destPath) Then Directory.CreateDirectory(destPath)

			For Each document In m_MDYearFiles

				If File.Exists(document) Then
					m_Logger.LogDebug(String.Format("copying mdyear data file: {0} >>> {1}", document, Path.Combine(destPath, IO.Path.GetFileName(document))))
					File.Copy(document, Path.Combine(destPath, IO.Path.GetFileName(document)), True)
				Else
					m_Logger.LogError(String.Format("file not founded! {0}", document))

				End If

			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("error during coping mdyear data: {0} >>> {1}: {2}", mandantPath, destPath, ex.ToString))
			Return False

		End Try


		Return success

	End Function




	Private Function UpdateMandantDb(ByVal mandantPath As String, ByVal dbConn As String) As Boolean
		Dim success As Boolean = True
		Dim query As String = String.Empty

#If DEBUG Then
		dbConn = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Sputnik TSWAG;Data Source=SPX01\MSSQLSERVER12;Current Language=German;Pooling=false"
#End If

		Try

			For Each query In m_QueryFiles

				If File.Exists(query) Then
					success = success AndAlso ExecuteSQLScript(query, dbConn)
				End If
			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("error during running query file on: {0} | Mandantpath: {1} >>> {2}", query, mandantPath, dbConn))
			Return False

		End Try


		Return success

	End Function

	Private Function ExecuteSQLScript(ByVal queryFile As String, ByVal dbConn As String) As Boolean
		Dim success As Boolean = True

		Try
			m_Logger.LogDebug(String.Format("running query file: {0} >>> {1}", queryFile, dbConn))

			success = success AndAlso m_UpdateDatabaseAccess.ExecuteAssignedSQLScript(queryFile, dbConn)

		Catch ex As Exception
			m_Logger.LogError(String.Format("error during running query file on: {0} >>> {1} | {2}", queryFile, dbConn, ex.ToString))
			Return False

		End Try


		Return success

	End Function

	Private Function UpdateLocalDatabase() As Boolean
		Dim success As Boolean = True
		Dim newData As BindingList(Of FTPUpdateData) = grdNew.DataSource

		If newData Is Nothing Then Return False

		Try

			Dim data As New FTPUpdateData

			For Each netfile In m_NetFiles
				Dim netData = newData.Where(Function(itm) itm.UpdateFilename = Path.GetFileName(netfile) And itm.FileDestinationEnumValue = FileDestEnum.NET).FirstOrDefault
				If Not netData Is Nothing Then
					data = New FTPUpdateData With {.File_Guid = netData.File_Guid, .UpdateFileDate = netData.UpdateFileDate, .UpdateFilename = netData.UpdateFilename,
									.FileDestVersion = netData.FileDestVersion, .UpdateFileSize = netData.UpdateFileSize, .UpdateFileTime = netData.UpdateFileSize}
					success = success AndAlso m_UpdateDatabaseAccess.AddDownloadedData(data)
				End If

			Next

			data = New FTPUpdateData
			For Each doc In m_DocumentFiles
				Dim docData = newData.Where(Function(itm) itm.UpdateFilename = Path.GetFileName(doc) And itm.FileDestinationEnumValue = FileDestEnum.DOCUMENT).FirstOrDefault
				If Not docData Is Nothing Then
					data = New FTPUpdateData With {.File_Guid = docData.File_Guid, .UpdateFileDate = docData.UpdateFileDate, .UpdateFilename = docData.UpdateFilename,
									.FileDestVersion = docData.FileDestVersion, .UpdateFileSize = docData.UpdateFileSize, .UpdateFileTime = docData.UpdateFileSize}
					success = success AndAlso m_UpdateDatabaseAccess.AddDownloadedData(data)
				End If
			Next

			data = New FTPUpdateData
			For Each tpl In m_TemplateFiles
				Dim tplData = newData.Where(Function(itm) itm.UpdateFilename = Path.GetFileName(tpl) And itm.FileDestinationEnumValue = FileDestEnum.TEMPLATE).FirstOrDefault
				If Not tplData Is Nothing Then
					data = New FTPUpdateData With {.File_Guid = tplData.File_Guid, .UpdateFileDate = tplData.UpdateFileDate, .UpdateFilename = tplData.UpdateFilename,
									.FileDestVersion = tplData.FileDestVersion, .UpdateFileSize = tplData.UpdateFileSize, .UpdateFileTime = tplData.UpdateFileSize}
					success = success AndAlso m_UpdateDatabaseAccess.AddDownloadedData(data)
				End If
			Next

			data = New FTPUpdateData
			For Each qry In m_QueryFiles
				Dim tplData = newData.Where(Function(itm) itm.UpdateFilename = Path.GetFileName(qry) And itm.FileDestinationEnumValue = FileDestEnum.QUERY).FirstOrDefault
				If Not tplData Is Nothing Then
					data = New FTPUpdateData With {.File_Guid = tplData.File_Guid, .UpdateFileDate = tplData.UpdateFileDate, .UpdateFilename = tplData.UpdateFilename,
									.FileDestVersion = tplData.FileDestVersion, .UpdateFileSize = tplData.UpdateFileSize, .UpdateFileTime = tplData.UpdateFileSize}
					success = success AndAlso m_UpdateDatabaseAccess.AddDownloadedData(data)
				End If
			Next


		Catch ex As Exception
			m_Logger.LogError(String.Format("error during writing to update table: {0}", ex.ToString))
			success = False
		End Try


		Return success

	End Function

	'''' <summary>
	'''' Search for current list over web service.
	'''' </summary>
	'Private Sub SearchCurrentlistViaWebService()

	'	Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

	'	Task(Of BindingList(Of FTPUpdateData)).Factory.StartNew(Function() PerformCurrentlistWebserviceCallAsync(),
	'																					CancellationToken.None,
	'																					TaskCreationOptions.None,
	'																					TaskScheduler.Default).ContinueWith(Sub(t) FinishCurrentlistWebserviceCallTask(t), CancellationToken.None,
	'																																							TaskContinuationOptions.None, uiSynchronizationContext)
	'End Sub

	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformCurrentlistWebserviceCallAsync() As BindingList(Of FTPUpdateData)

		'Dim listDataSource As BindingList(Of FTPUpdateData) = New BindingList(Of FTPUpdateData)
		Dim data = New BindingList(Of FTPUpdateData)

#If DEBUG Then
		'		Customer_ID = "57EA3F1A-1390-4B96-B9B3-BF98F555BC4F" ' "C942EF9B-A455-49BE-B7FB-5507FCD2F1C0"
#End If

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)
			Dim customerData As SPUpdateUtilitiesService.CustomerMDData = New SPUpdateUtilitiesService.CustomerMDData With {.CustomerID = String.Empty, .LocalIPAddress = m_StationData.LocalIPAddress,
					.ExternalIPAddress = m_StationData.ExternalIPAddress, .LocalHostName = m_StationData.LocalHostName, .LocalDomainName = m_StationData.LocalDomainName}

			Dim searchResult As List(Of FTPUpdateFilesDTO) = Nothing
			' Read data over webservice
			searchResult = webservice.GetFTPUpdateFiles(customerData).ToList

			For Each result In searchResult

				Dim viewData = New FTPUpdateData With {
							.File_Guid = result.File_Guid,
							.FileContent = result.FileContent,
							.FileDestPath = result.FileDestPath,
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

	Private Function PerformRequiredFTPSetupFilesWebserviceCallAsync() As BindingList(Of FTPUpdateData)

		Dim data = New BindingList(Of FTPUpdateData)

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)
			Dim customerData As SPUpdateUtilitiesService.CustomerMDData = New SPUpdateUtilitiesService.CustomerMDData With {.CustomerID = String.Empty, .LocalIPAddress = m_StationData.LocalIPAddress,
					.ExternalIPAddress = m_StationData.ExternalIPAddress, .LocalHostName = m_StationData.LocalHostName, .LocalDomainName = m_StationData.LocalDomainName}

			Dim searchResult As List(Of ModuleFilesDTO) = Nothing
			' Read data over webservice
			searchResult = webservice.GetProgramModuleFilesList(customerData, "FTP").ToList

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

	''' <summary>
	'''  Performs the Download asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformDownloadFileWebserviceCallAsync(ByVal recID As Integer) As FTPUpdateData

		Dim result As FTPUpdateData = Nothing
		If recID = 0 Then Return Nothing

#If DEBUG Then
		'		Customer_ID = "57EA3F1A-1390-4B96-B9B3-BF98F555BC4F" ' "C942EF9B-A455-49BE-B7FB-5507FCD2F1C0"
#End If

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)

			Dim searchResult As FTPUpdateFilesDTO = Nothing
			' Read data over webservice
			searchResult = webservice.GetFTPUpdateFileContent(recID)

			If searchResult Is Nothing Then
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Der gewünschte Datensatz mit der Nummer {0} wurde nicht gefunden.", recID))

				Return Nothing
			End If

			result = New FTPUpdateData With {
							.File_Guid = searchResult.File_Guid,
							.FileContent = searchResult.FileContent,
							.FileDestPath = searchResult.FileDestPath,
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

	''' <summary>
	'''  Performs the Download asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformIsAllowedMandantUpdateWebserviceCallAsync(ByVal customerID As String) As Boolean

		Dim result As Boolean = False
		If customerID = String.Empty Then Return False

#If DEBUG Then
		'		Customer_ID = "57EA3F1A-1390-4B96-B9B3-BF98F555BC4F" ' "C942EF9B-A455-49BE-B7FB-5507FCD2F1C0"
#End If

		Try
			Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)

			Dim customerData As SPUpdateUtilitiesService.CustomerMDData = New SPUpdateUtilitiesService.CustomerMDData With {.CustomerID = customerID, .LocalIPAddress = m_StationData.LocalIPAddress,
					.ExternalIPAddress = m_StationData.ExternalIPAddress, .LocalHostName = m_StationData.LocalHostName, .LocalDomainName = m_StationData.LocalDomainName}

			' Read data over webservice
			Dim searchResult = webservice.IsMandantUpdateAllowed(customerData)

			If Not searchResult Then
				m_Logger.LogWarning(String.Format("mandant is not allowed to be updated: {0}", customerID))

				Return False
			End If

			result = searchResult

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


	Private Sub frmSrvFTPDownload_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
		'm_Timer.Enabled = False
		'm_Timer.Stop()
	End Sub


	Private Sub OnbtnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click
		LoadData()

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub OnbtnDownloadData_Click(sender As Object, e As EventArgs) Handles btnDownloadData.Click
		StartWithDownload()

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub OngrdUpdates_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdUpdates.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		If Not files Is Nothing AndAlso files.Count > 0 Then
			For Each myfile In files
				Dim fileInfo As New FileInfo(myfile)
				Dim pathLabel As String
				'Dim data = TryCast(lueDestPath.GetSelectedDataRow(), DestPathData)
				'If data Is Nothing Then 'pathLabel = "NET" Else pathLabel = data.DestPathLabel

				Select Case fileInfo.Extension.ToLower
						Case ".dll", ".exe"
							pathLabel = "NET"

						Case ".sql"
							pathLabel = "QUERY"

						Case ".lst", ".lsv", ".crd", ".crv"
							pathLabel = "DOCUMENTS"

						Case ".doc", ".docx", ".dot"
							pathLabel = "TEMPLATES"


						Case Else
							pathLabel = "NET"

					End Select

				'Else
				'	pathLabel = data.DestPathLabel

				'End If
				If String.IsNullOrWhiteSpace(pathLabel) Then Continue For

				Dim result As Boolean = PerformUploadReportDropInWebserviceCallAsync(pathLabel, fileInfo)

			Next

			LoadData()
		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OngrdUpdates_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdUpdates.DragEnter
		e.Effect = DragDropEffects.Copy
	End Sub


	Private Sub OngrdFTPUpdateFiles_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdFTPUpdateFiles.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		If Not files Is Nothing AndAlso files.Count > 0 Then
			Dim fileInfo As New FileInfo(files(0))

			Dim result As Boolean = PerformUploadReportDropInWebserviceCallAsync("FTP", fileInfo)
			If result Then
				Dim ftpModuleFileData = PerformRequiredFTPSetupFilesWebserviceCallAsync()
				grdFTPUpdateFiles.DataSource = ftpModuleFileData
			End If

		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OngrdFTPUpdateFiles_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdFTPUpdateFiles.DragEnter
		e.Effect = DragDropEffects.Copy
	End Sub

	Private Sub OngrdClientUpdateFiles_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdClientUpdateFiles.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		If Not files Is Nothing AndAlso files.Count > 0 Then
			Dim fileInfo As New FileInfo(files(0))

			Dim result As Boolean = PerformUploadReportDropInWebserviceCallAsync("ClientUpdate", fileInfo)
			If result Then
				Dim ClientModuleFileData = PerformRequiredClientSetupFilesWebserviceCallAsync()
				grdClientUpdateFiles.DataSource = ClientModuleFileData
			End If

		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OngrdClientUpdateFiles_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdClientUpdateFiles.DragEnter
		e.Effect = DragDropEffects.Copy
	End Sub

	''' <summary>
	'''  Performs Paidlist check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformUploadReportDropInWebserviceCallAsync(ByVal moduleName As String, ByVal newFile As FileInfo) As Boolean

		If newFile Is Nothing Then Return False

		Dim webservice As New SPUpdateUtilitiesService.SPUpdateUtilitiesSoapClient
#If DEBUG Then
		'm_UpdateWebServiceUri = "http://localhost:44721/SPUpdateUtilities.asmx"
#End If

		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_UpdateWebServiceUri)
		Dim fileToSend As FTPUpdateFilesDTO = New SPUpdateUtilitiesService.FTPUpdateFilesDTO With {.FileContent = m_Utility.LoadFileBytes(newFile.FullName), .FileDestPath = moduleName, .FileDestVersion = String.Empty,
				.File_Guid = Guid.NewGuid.ToString(), .UpdateFileDate = File.GetLastWriteTime(newFile.FullName).ToShortDateString(), .UpdateFilename = newFile.Name, .UpdateFileSize = newFile.Length,
				.UpdateFileTime = File.GetLastWriteTime(newFile.FullName).ToLongTimeString()}
		' Read data over webservice
		Dim success = webservice.UploadNewUpdateFile(fileToSend)


		Return success

	End Function

	''' <summary>
	''' Finish uploadlist web service call.
	''' </summary>
	Private Sub FinishUploadReportDropInWebserviceCallTask(ByVal t As Task(Of Boolean))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					m_SuppressUIEvents = True


					m_SuppressUIEvents = False

				Case TaskStatus.Faulted
					' Something went wrong -> log error.
					m_Logger.LogError(t.Exception.ToString())

				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub

	Private Sub hlNewProgramVersion_Click(sender As Object, e As EventArgs) Handles hlNewProgramVersion.Click
		Process.Start(m_NewUpdateFilePath)
		Process.Start(Directory.GetCurrentDirectory)
	End Sub



#Region "Helpers"

	Private Function GetInternalIP() As String
		Dim strHostName As String
		'Dim strIPAddress As String

		strHostName = System.Net.Dns.GetHostName()

		'strIPAddress = Dns.GetHostEntry(strHostName).AddressList(0).ToString()
		Dim strIPAddress As String = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(Function(a As IPAddress) Not a.IsIPv6LinkLocal AndAlso Not a.IsIPv6Multicast AndAlso Not a.IsIPv6SiteLocal).First().ToString()

		Return strIPAddress

	End Function

	Private Function GetInternalHostName() As String
		Dim strHostName As String

		strHostName = System.Net.Dns.GetHostName()

		Return strHostName

	End Function

	Private Function GetInternalDomainName() As String
		Dim strHostName As String

		strHostName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()

		Return strHostName

	End Function

	Private Function GetExternalIP() As String
		Dim Response As String = String.Empty
		Dim lol As WebClient = New WebClient()

		Try
			Dim ExternalIP As String = (New WebClient()).DownloadString("http://checkip.dyndns.org/")

			'<html><head><title>Current IP Check</title></head><body>Current IP Address: 212.120.49.66</body></html>
			ExternalIP = (New Regex("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(ExternalIP)(0).ToString()

			If String.IsNullOrWhiteSpace(ExternalIP) Then
				Dim str As String = lol.DownloadString("https://www.ip-adress.com/")
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
			m_Logger.LogError(String.Format("Could not confirm External IP Address from {0} | {1}", "http://checkip.dyndns.org/", ex.ToString))

			Return String.Empty
		End Try

		Return Response

	End Function

	Private Function CreateTemporaryUpdateFolder() As Boolean
		Dim success As Boolean = True

		Dim tmpPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sputnik\SputnikFTPUpdate")
		Dim serverUpdatePath As String = Path.Combine(My.Settings.SPSEnterpriseFolder, "Update")

		m_Logger.LogInfo(String.Format("temp folder for downloads: {0}", tmpPath))
		m_Logger.LogInfo(String.Format("update folder on SPEnterprise$: {0}", serverUpdatePath))

		Try
			m_NewUpdateFilePath = Path.Combine(tmpPath, "FTPNewFiles")

			m_TempNETFolder = String.Format(FOLDER_DOWNLOAD_SPS_NET, tmpPath)
			m_NETFolder = Path.Combine(serverUpdatePath, "Binn\Net")

			m_TempDocumentFolder = String.Format(FOLDER_DOWNLOAD_SPS_DOCUMENT, tmpPath)
			m_DocumentFolder = Path.Combine(serverUpdatePath, "Binn\Documents")

			m_TempQueryFolder = String.Format(FOLDER_DOWNLOAD_SPS_QUERY, tmpPath)
			m_QueryFolder = Path.Combine(serverUpdatePath, "Binn\Query")

			m_TempTemplateFolder = String.Format(FOLDER_DOWNLOAD_SPS_TEMPLATE, tmpPath)
			m_TempSkinFolder = String.Format(FOLDER_DOWNLOAD_SPS_SKIN, tmpPath)
			m_TempmdyearFolder = String.Format(FOLDER_DOWNLOAD_SPS_MDYEAR, tmpPath, Now.Year)


			m_Logger.LogInfo(String.Format("net folder on local machine: {0}", m_TempNETFolder))
			m_Logger.LogInfo(String.Format("net folder on SPEnterprise$\update: {0}", m_NETFolder))

			m_Logger.LogInfo(String.Format("document folder on local machine: {0}", m_TempDocumentFolder))
			m_Logger.LogInfo(String.Format("document folder on SPEnterprise$\update: {0}", m_DocumentFolder))

			m_Logger.LogInfo(String.Format("query folder on local machine: {0}", m_TempQueryFolder))
			m_Logger.LogInfo(String.Format("query folder on SPEnterprise$\update: {0}", m_QueryFolder))

			m_Logger.LogInfo(String.Format("template folder on local machine: {0}", m_TempTemplateFolder))
			m_Logger.LogInfo(String.Format("skin folder on local machine: {0}", m_TempSkinFolder))
			m_Logger.LogInfo(String.Format("mdyear folder on local machine: {0}", m_TempMDYearFolder))


			If Not Directory.Exists(m_NewUpdateFilePath) Then Directory.CreateDirectory(m_NewUpdateFilePath) Else DeleteFilesFromFolder(m_NewUpdateFilePath)
			If Not Directory.Exists(m_TempNETFolder) Then Directory.CreateDirectory(m_TempNETFolder) Else DeleteFilesFromFolder(m_TempNETFolder)
			If Not Directory.Exists(m_NETFolder) Then Directory.CreateDirectory(m_NETFolder)

			If Not Directory.Exists(m_TempDocumentFolder) Then Directory.CreateDirectory(m_TempDocumentFolder) Else DeleteFilesFromFolder(m_TempDocumentFolder)
			If Not Directory.Exists(m_DocumentFolder) Then Directory.CreateDirectory(m_DocumentFolder)

			If Not Directory.Exists(m_TempQueryFolder) Then Directory.CreateDirectory(m_TempQueryFolder) Else DeleteFilesFromFolder(m_TempQueryFolder)
			If Not Directory.Exists(m_QueryFolder) Then Directory.CreateDirectory(m_QueryFolder)

			If Not Directory.Exists(m_TempTemplateFolder) Then Directory.CreateDirectory(m_TempTemplateFolder) Else DeleteFilesFromFolder(m_TempTemplateFolder)
			If Not Directory.Exists(m_TempSkinFolder) Then Directory.CreateDirectory(m_TempSkinFolder) Else DeleteFilesFromFolder(m_TempSkinFolder)
			If Not Directory.Exists(m_TempMDYearFolder) Then Directory.CreateDirectory(m_TempMDYearFolder) Else DeleteFilesFromFolder(m_TempMDYearFolder)

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

	Private Function ParseToDouble(ByVal stringvalue As String, ByVal value As Double?) As Double
		Dim result As Double
		If (Not Double.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
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


#End Region

	Private Class DestPathData

		Public Property Value As Integer
		Public Property Description As DestPathEnum

		Public ReadOnly Property DestPathLabel As String
			Get
				Dim value As String = String.Empty

				If Description = DestPathEnum.NET Then value = "NET"
				If Description = DestPathEnum.QUERY Then value = "QUERY"
				If Description = DestPathEnum.DOCUMENTS Then value = "DOCUMENTS"
				If Description = DestPathEnum.TEMPLATES Then value = "TEMPLATES"
				If Description = DestPathEnum.MDYEAR Then value = "MDYEAR"
				If Description = DestPathEnum.PROFILES Then value = "PROFILES"
				If Description = DestPathEnum.SKINS Then value = "SKINS"


				Return value
			End Get
		End Property

	End Class


	Private Enum DestPathEnum
		NET
		QUERY
		DOCUMENTS
		TEMPLATES
		MDYEAR
		PROFILES
		SKINS
	End Enum


End Class
