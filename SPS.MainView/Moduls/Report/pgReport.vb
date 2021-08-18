
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.LookAndFeel

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPS.MainView.ReportSettings
Imports SP.Infrastructure.Settings
Imports SPS.MainView.DataBaseAccess
Imports System.ComponentModel
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SPProgUtility.SPTranslation
Imports DevExpress.Utils.VisualEffects

Public Class pgReport


#Region "private consts"

	Private Const MODUL_NAME = "Rapportverwaltung"
	Private Const MODUL_NAME_SETTING = "report"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Rapportverwaltung"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/zg/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/zg/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/reportproperties/re/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/reportproperties/re/keepfilter"

#End Region


#Region "private fields"

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub

	Private m_Stopwatch As Stopwatch

	Private Property LoadedMDNr As Integer

	Protected m_SettingsManager As ISettingsManager

	Private m_GVMainSettingfilename As String
	Private m_GVZGSettingfilename As String
	Private m_GVInvoiceSettingfilename As String

	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()

	Private m_griddata As GridData

	Private _ClsRPPropertiesSetting As New ClsReportPropertySetting
	Dim _RPSetting As ClsReportSetting

	Private Property gvZGDisplayMember As String


	'Private m_UitilityUI As UtilityUI

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingZGFilter As String
	Private m_xmlSettingRestoreZGSetting As String

	Private m_xmlSettingREFilter As String
	Private m_xmlSettingRestoreRESetting As String

	Private m_AllowedChangeMandant As Boolean
	Private m_MainViewGridData As SuportedCodes

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Public Property m_SuppressUIEvents As Boolean

	Private m_Badge As DevExpress.Utils.VisualEffects.Badge
	Private m_DateAndTimeUtility As DateAndTimeUtily

#End Region


#Region "Private Properties"

	Private ReadOnly Property SelectedRowViewData As FoundedReportData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedReportData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "Constructur"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		m_SuppressUIEvents = True

		m_Badge = New Badge
		m_UtilityUI = New UtilityUI
		m_InitializationData = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_Stopwatch = New Stopwatch()

		BarManager1.Form = Me
		LoadedMDNr = ModulConstants.MDData.MDNr
		Me._RPSetting = New ClsReportSetting
		m_SettingsManager = New SettingsReportManager

		m_md = New Mandant
		m_utility = New Utilities
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_translate = New TranslateValues
		m_DateAndTimeUtility = New DateAndTimeUtily

		dpProperties.Options.ShowCloseButton = False

		Try
			m_communicationHub = MessageService.Instance.Hub

			m_GVMainSettingfilename = String.Format("{0}Report\{1}{2}.xml", ModulConstants.GridSettingPath, gvMain.Name, ModulConstants.UserData.UserNr)
			m_GVZGSettingfilename = String.Format("{0}Report\{1}{2}.xml", ModulConstants.GridSettingPath, gvZG.Name, ModulConstants.UserData.UserNr)
			m_GVInvoiceSettingfilename = String.Format("{0}Report\{1}{2}.xml", ModulConstants.GridSettingPath, gvInvoice.Name, ModulConstants.UserData.UserNr)

			m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreZGSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingZGFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreRESetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingREFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200005)

		m_MainViewGridData = New SuportedCodes
		m_MainViewGridData.ChangeColumnNamesToLowercase = True
		m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
		m_griddata = m_MainViewGridData.LoadMainGridData


		'LoadXMLDataForSelectedModule()
		ResetMainGrid()

		ResetMandantenDropDown()
		LoadFoundedMDList()

		cboMD.EditValue = ModulConstants.MDData.MDNr


		AddHandler Me.gvMain.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
		AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged

		AddHandler gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
		AddHandler gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler Me.gvZG.RowCellClick, AddressOf OngvZG_RowCellClick
		AddHandler Me.gvZG.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvZG.ColumnPositionChanged, AddressOf OngvZGColumnPositionChanged
		AddHandler Me.gvZG.ColumnWidthChanged, AddressOf OngvZGColumnPositionChanged

		AddHandler Me.gvInvoice.RowCellClick, AddressOf OngvInvoice_RowCellClick
		AddHandler Me.gvInvoice.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvInvoice.ColumnPositionChanged, AddressOf OngvInvoiceColumnPositionChanged
		AddHandler Me.gvInvoice.ColumnWidthChanged, AddressOf OngvInvoiceColumnPositionChanged

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SizeChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties

		AddHandler dpProperties.Resize, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_1_2.SplitterPositionChanged, AddressOf SaveFormProperties

	End Sub

#End Region


	Private Sub pg_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_SuppressUIEvents = True

		TranslateControls()

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedReportList()

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpZG.Text = m_translate.GetSafeTranslationValue(Me.grpZG.Text)
		Me.grpRE.Text = m_translate.GetSafeTranslationValue(Me.grpRE.Text)

		Me.dpProperties.Text = m_translate.GetSafeTranslationValue(Me.dpProperties.Text)

	End Sub


	Private Sub SetFormLayout()

		Me.Parent.Text = String.Format("{0}", m_translate.GetSafeTranslationValue(UsercontrolCaption))
		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try
		m_SuppressUIEvents = True
		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingReportKeys.SETTING_RP_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingReportKeys.SETTING_RP_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingReportKeys.SETTING_RP_SCC_MAINNAV_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainNav_1.SplitterPosition = Math.Max(Me.sccMainNav_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingReportKeys.SETTING_RP_SCC_MAINPROP_1_2_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainProp_1_2.SplitterPosition = Math.Max(Me.sccMainProp_1_2.SplitterPosition, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75

		Me.gvZG.OptionsView.ShowIndicator = False
		Me.gvInvoice.OptionsView.ShowIndicator = False

		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill

		Me.gvRProperty.OptionsView.ShowIndicator = False
		Me.gvLProperty.OptionsView.ShowIndicator = False

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		m_SuppressUIEvents = False

	End Sub

	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

	End Sub


	Private Function LoadFoundedReportList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.GetDbRepportData4Show(m_griddata.SQLQuery)

		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Return False
		End If


		Dim mergedESData As BindingList(Of FoundedReportData) = New BindingList(Of FoundedReportData)
		For Each p In listOfFoundedData
			mergedESData.Add(p)
		Next

		Dim calculatedData = LoadPVLNotificationData(mergedESData)

		Dim listDataSource As BindingList(Of FoundedReportData) = New BindingList(Of FoundedReportData)

		For Each p In calculatedData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource

		RefreshMainViewStateBar()
		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Function LoadPVLNotificationData(ByVal esData As BindingList(Of FoundedReportData)) As BindingList(Of FoundedReportData)
		Dim result As List(Of FoundedReportData) = Nothing

		Try

			Dim newsData = New SPGAV.UI.frmPublicationNews(m_InitializationData)
			Dim m_tempDataMergedNews = newsData.LoadMergedNewsData(False)

			If (m_tempDataMergedNews Is Nothing) Then
				'm_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Die GAV-News konnten nicht geladen werden."))
				For Each assigedES In esData
					assigedES.gavstate = Nothing
				Next
				Return esData
			End If

			For Each rp In esData
				Dim esEnde = If(rp.es_ende.HasValue, rp.es_ende.Value.Date, New DateTime(3999, 12, 31))

				Dim assignedNewsData = m_tempDataMergedNews.Where(Function(x) x.ContractNumber = rp.gavnumber And x.PublicationDate >= rp.lovon And x.PublicationDate <= rp.bis And x.PublicationDate <= esEnde).FirstOrDefault
				If assignedNewsData Is Nothing Then Continue For


				Dim gavUpdateDate As DateTime = assignedNewsData.PublicationDate
				If rp.rpnr = 56987 Then
					Trace.WriteLine(String.Format("{0}-{1} >>> {2}", rp.von, rp.bis, rp.lovon))
				End If

				If rp.bis < gavUpdateDate OrElse esEnde < gavUpdateDate Then Continue For

				' Check if the GAVNr has between updated between ESLohnVon and ESLohnBis.
				If gavUpdateDate >= rp.lovon AndAlso gavUpdateDate <= rp.bis Then

					rp.gavstate = False
					rp.gavnotification = String.Format("{0:G}:<br>{1}", gavUpdateDate, assignedNewsData.Content)

				Else
					rp.gavstate = True
					rp.gavnotification = String.Empty

				End If

			Next

			grdMain.DataSource = esData
			RefreshMainViewStateBar()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try


		Return esData
	End Function


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
		Dim column = e.Column

		pccMandant.HidePopup()
		AdornerUIManager1.Elements.Remove(m_Badge)
		AdornerUIManager1.Hide()

		Dim dataRow = gvMain.GetRow(e.RowHandle)
		If dataRow Is Nothing Then Return
		Dim viewData = CType(dataRow, FoundedReportData)
		If viewData Is Nothing Then Return

		_RPSetting.SelectedMDNr = viewData.mdnr
		_RPSetting.SelectedMANr = viewData.manr
		_RPSetting.SelectedKDNr = viewData.kdnr
		_RPSetting.SelectedESNr = viewData.esnr
		_RPSetting.SelectedRPNr = viewData.rpnr
		_RPSetting.SelectedMonth = viewData.rpmonat
		_RPSetting.SelectedYear = viewData.rpjahr
		_RPSetting.PrintNoRP = viewData.PrintNoRP

		BuildNewPrintContextMenu()

		If (e.Clicks = 1) Then
			If column.Name.ToLower = "gavstate" Then
				If Not viewData.gavstate Then
					Dim infoText = String.Format("{0}", viewData.gavnotification)
					m_UtilityUI.ShowStandardBadgeNotification(grdMain, m_Badge, AdornerUIManager1,
																					   m_translate.GetSafeTranslationValue(infoText),
																					   ContentAlignment.TopCenter, TargetElementRegion.Default, BadgePaintStyle.Critical)
					'm_UtilityUI.ShowOKDialog(Me, infoText, m_translate.GetSafeTranslationValue("GAV Anpassung"), MessageBoxIcon.Asterisk)
				End If
			End If
		End If


		If (e.Clicks = 2) Then
			Select Case column.Name.ToLower
				Case "customername", "kdnr"
					If viewData.kdnr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
						_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
					End If

				Case "zname", "zhdnr"
					If viewData.zhdnr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr,
																																	.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
						_ClsKD.OpenSelectedCPerson()
					End If

				Case "customertelefon"
					If viewData.customertelefon.Length > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr,
																																	.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = 0})
						_ClsKD.TelefonCallToCustomer(viewData.customertelefon)
					End If
				Case "zmobile"
					If viewData.zmobile.Length > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr,
																																	.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
						_ClsKD.TelefonCallToCustomer(viewData.zmobile)
					End If
				Case "ztelefon"
					If viewData.ztelefon.Length > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr,
																																	.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
						_ClsKD.TelefonCallToCustomer(viewData.ztelefon)
					End If


				Case "employeename", "manr"
					If viewData.manr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.manr})
						_ClsKD.OpenSelectedEmployee(viewData.employeeMDNr, ModulConstants.UserData.UserNr)
					End If


				Case "employeetelefon"
					If viewData.employeetelfon.Length > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr,
																																	.SelectedMANr = viewData.manr, .SelectedKDNr = 0, .SelectedZHDNr = 0})
						_ClsKD.TelefonCallToCustomer(viewData.employeetelfon)
					End If
				Case "employeemobile"
					If viewData.employeemobile.Length > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr,
																																	.SelectedMANr = viewData.manr, .SelectedKDNr = 0, .SelectedZHDNr = 0})
						_ClsKD.TelefonCallToCustomer(viewData.employeemobile)
					End If

				Case "esnr", "es_als"
					If viewData.esnr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.esMDNr,
																																	.SelectedESNr = viewData.esnr})
						_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
					End If

				Case "lonr"
					If viewData.lonr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.loMDNr,
																																	.SelectedLONr = viewData.lonr,
																																	.SelectedMANr = viewData.manr})
						_ClsKD.OpenSelectedLO()
					End If


				Case Else
					If viewData.rpnr > 0 Then
						Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
						_ClsMA.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
					End If

			End Select

		End If

	End Sub

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

#Region "read xml setting file"

	'Private Function GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'															And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'						   Select New With {
	'																				.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																				.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																				.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																				.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																				.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																				.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																				.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																				.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																				.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																				.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																				.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																				.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																					}).FirstOrDefault()



	'		result.SQLQuery = ConfigQuery.SQLQuery
	'		result.GridColFieldName = ConfigQuery.GridColFieldName
	'		result.DisplayMember = ConfigQuery.DisplayMember
	'		result.GridColCaption = ConfigQuery.GridColCaption
	'		result.GridColWidth = ConfigQuery.GridColWidth
	'		result.BackColor = ConfigQuery.BackColor
	'		result.ForeColor = ConfigQuery.ForeColor

	'		result.PopupFields = ConfigQuery.PopupFields
	'		result.PopupCaptions = ConfigQuery.PopupCaptions

	'		result.CountOfFieldsInHeader = CShort(ConfigQuery.CountOfFieldsInHeader)
	'		result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
	'		result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow
	'		result.IsUserProperty = False

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return result
	'End Function

	'Private Function GetUserGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'															And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'						   Select New With {
	'																				.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																				.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																				.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																				.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																				.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																				.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																				.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																				.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																				.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																				.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																				.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																				.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																					}).FirstOrDefault()


	'		If Not ConfigQuery Is Nothing Then
	'			result.SQLQuery = ConfigQuery.SQLQuery
	'			result.GridColFieldName = ConfigQuery.GridColFieldName
	'			result.DisplayMember = ConfigQuery.DisplayMember
	'			result.GridColCaption = ConfigQuery.GridColCaption
	'			result.GridColWidth = ConfigQuery.GridColWidth
	'			result.BackColor = ConfigQuery.BackColor
	'			result.ForeColor = ConfigQuery.ForeColor

	'			result.PopupFields = ConfigQuery.PopupFields
	'			result.PopupCaptions = ConfigQuery.PopupCaptions

	'			Dim scountofitems As Short = Short.TryParse(ConfigQuery.CountOfFieldsInHeader, scountofitems)
	'			result.CountOfFieldsInHeader = Math.Max(scountofitems, 3)
	'			result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
	'			result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow

	'			result.IsUserProperty = True

	'		End If


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return result
	'End Function

#End Region



	Sub SaveFormProperties()

		If m_RPSuppressUIEvents Then Return
		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_DPPROPERTY_WIDTH, Me.dpProperties.Width)

		m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_SCC_MAINNAV_1_SPLITTERPOSION, Me.sccMainNav_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingReportKeys.SETTING_RP_SCC_MAINPROP_1_2_SPLITTERPOSION, Me.sccMainProp_1_2.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub

	Private Sub FillPopupFields(ByVal selectedrow As FoundedReportData)
		Dim result As New Dictionary(Of String, String)
		Dim strValue As String = String.Empty

		Dim listOfPropertyLData As New List(Of PropertyData)
		Dim listOfPropertyRData As New List(Of PropertyData)
		grdLProperty.DataSource = Nothing
		grdRProperty.DataSource = Nothing

		Dim strPFieldName As String = m_griddata.FieldsInHeaderToShow
		Dim strPFieldCaption As String = m_griddata.CaptionsInHeaderToShow
		Dim aPoupFields As String() = strPFieldName.Split(CChar(";"))
		Dim aPoupCaption As String() = strPFieldCaption.Split(CChar(";"))
		Dim iCountofFieldInHeaderInfo As Short = 3
		If Not m_griddata.CountOfFieldsInHeader.HasValue Then
			iCountofFieldInHeaderInfo = m_griddata.CountOfFieldsInHeader
		End If

		Try

			For j As Integer = 0 To aPoupFields.Length - 1
				Select Case aPoupFields(j).ToLower
					Case "customername"
						strValue = selectedrow.customername
					Case "zname"
						strValue = selectedrow.zname
					Case "employeename"
						strValue = selectedrow.employeename
					Case "es_als"
						strValue = selectedrow.es_als

					Case "kdnr"
						strValue = selectedrow.kdnr

					Case "createdon"
						strValue = selectedrow.createdon
					Case "gavbezeichnung"
						strValue = selectedrow.gavbezeichnung

					Case "employeeadvisor"
						strValue = selectedrow.employeeadvisor
					Case "customeradvisor"
						strValue = selectedrow.customeradvisor

				End Select

				result.Add(aPoupFields(j), strValue)
			Next

			Dim itemName As String
			Dim itemValue As String

			For i As Integer = 0 To result.Count - 1
				If Not String.IsNullOrEmpty(aPoupCaption(i)) And Not String.IsNullOrEmpty(result.Item(aPoupFields(i))) Then

					If i > iCountofFieldInHeaderInfo Then
						itemName = aPoupCaption(i)
						itemValue = result.Item(aPoupFields(i))

						Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
						listOfPropertyRData.Add(propertyItem)

					Else
						itemName = aPoupCaption(i)
						itemValue = result.Item(aPoupFields(i))

						Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
						listOfPropertyLData.Add(propertyItem)

					End If

				End If
			Next

			If Not (listOfPropertyLData Is Nothing) Then
				grdLProperty.DataSource = listOfPropertyLData
				grdLProperty.Visible = True

			Else
				grdLProperty.Visible = False

			End If

			If Not (listOfPropertyRData Is Nothing) Then
				grdRProperty.DataSource = listOfPropertyRData
				grdRProperty.Visible = True

			Else
				grdRProperty.Visible = False

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			ShowErrDetail(ex.Message)
			result = Nothing

		End Try

	End Sub

	Private Sub OngvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				If selectedrow.rpnr = 0 Then Exit Sub

				Me._RPSetting.SelectedMDNr = selectedrow.mdnr
				Me._RPSetting.SelectedRPNr = selectedrow.rpnr

				Me._RPSetting.SelectedMANr = selectedrow.manr
				Me._RPSetting.SelectedKDNr = selectedrow.kdnr
				Me._RPSetting.SelectedESNr = selectedrow.esnr

				Me._RPSetting.SelectedMonth = selectedrow.rpmonat
				Me._RPSetting.SelectedYear = selectedrow.rpjahr
				Me._RPSetting.SelectedLONr = selectedrow.lonr

				FillPopupFields(selectedrow)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4RP()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub FillProperties4RP()

		ResetZGDetailGrid()
		LoadEmployeeZGDetailList()

		ResetInvoiceDetailGrid()
		LoadReportInvoiceDetailList()

	End Sub

#Region "ZG Funktionen..."

	Sub ResetZGDetailGrid()

		gvZG.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvZG.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvZG.OptionsView.ShowGroupPanel = False
		gvZG.OptionsView.ShowIndicator = False
		gvZG.OptionsView.ShowAutoFilterRow = False

		gvZG.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zgnr"
			columnmodulname.FieldName = "zgnr"
			columnmodulname.Visible = True
			gvZG.Columns.Add(columnmodulname)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_translate.GetSafeTranslationValue("Auszahlungsart")
			columnLAName.Name = "laname"
			columnLAName.FieldName = "laname"
			columnLAName.Visible = True
			gvZG.Columns.Add(columnLAName)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "betrag"
			columnBetrag.FieldName = "betrag"
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvZG.Columns.Add(columnBetrag)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Zeitperiode")
			columnBezeichnung.Name = "zgperiode"
			columnBezeichnung.FieldName = "zgperiode"
			columnBezeichnung.Visible = True
			gvZG.Columns.Add(columnBezeichnung)

			Dim columnAusDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAusDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnAusDat.Name = "aus_dat"
			columnAusDat.FieldName = "aus_dat"
			columnAusDat.Visible = False
			gvZG.Columns.Add(columnAusDat)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columncustomername.Name = "zfiliale"
			columncustomername.FieldName = "zfiliale"
			columncustomername.Visible = False
			gvZG.Columns.Add(columncustomername)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvZG.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvZG.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml(gvZG.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdZG.DataSource = Nothing

	End Sub


	Public Function LoadEmployeeZGDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbReportZGDataForProperties(Me._RPSetting.SelectedRPNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Vorschuss-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedReportZGDetailData With
					 {.mdnr = person.mdnr,
						.employeeMDNr = person.employeeMDNr,
						.zgnr = person.zgnr,
						.rpnr = person.rpnr,
						.manr = person.manr,
						.vgnr = person.vgnr,
						.lonr = person.lonr,
						.monat = person.monat,
						.jahr = person.jahr,
						.betrag = person.betrag,
						.employeename = person.employeename,
						.zgperiode = person.zgperiode,
						.aus_dat = person.aus_dat,
						.lanr = person.lanr,
						.laname = person.laname,
						.isaslo = person.isaslo,
						.isout = person.isout,
						.createdfrom = person.createdfrom,
						.createdon = person.createdon,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedReportZGDetailData) = New BindingList(Of FoundedReportZGDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdZG.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvZG_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvZG.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedReportZGDetailData)

				If viewData.zgnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZGNr = viewData.zgnr, .SelectedMANr = Me._RPSetting.SelectedMANr})
					_ClsKD.OpenSelectedAdvancePayment(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub

#End Region


#Region "Invoice Funktionen..."

	Sub ResetInvoiceDetailGrid()

		gvInvoice.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvInvoice.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvInvoice.OptionsView.ShowGroupPanel = False
		gvInvoice.OptionsView.ShowIndicator = False
		gvInvoice.OptionsView.ShowAutoFilterRow = False

		gvInvoice.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "renr"
			columnmodulname.FieldName = "renr"
			columnmodulname.Visible = False
			gvInvoice.Columns.Add(columnmodulname)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "fakdate"
			columnFakDat.FieldName = "fakdate"
			columnFakDat.Visible = True
			gvInvoice.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "betragink"
			columnBetragInk.FieldName = "betragink"
			columnBetragInk.Visible = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvInvoice.Columns.Add(columnBetragInk)

			Dim columnREKst As New DevExpress.XtraGrid.Columns.GridColumn()
			columnREKst.Caption = m_translate.GetSafeTranslationValue("KST")
			columnREKst.Name = "rekst"
			columnREKst.FieldName = "rekst"
			columnREKst.Visible = True
			gvInvoice.Columns.Add(columnREKst)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvInvoice.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvInvoice.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvInvoice.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml(gvInvoice.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdInvoice.DataSource = Nothing

	End Sub


	Public Function LoadReportInvoiceDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbReportInvoiceDataForProperties(Me._RPSetting.SelectedRPNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Debitoren-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedReportInvoiceDetailData With
					 {.mdnr = person.mdnr,
						.customerMDNr = person.customerMDNr,
						.renr = person.renr,
						.kdnr = person.kdnr,
						.fakdate = person.fakdate,
						.betragink = person.betragink,
						.rekst = person.rekst,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedReportInvoiceDetailData) = New BindingList(Of FoundedReportInvoiceDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdInvoice.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvInvoice_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvInvoice.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedReportInvoiceDetailData)

				If viewData.renr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedInvoice(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub


#End Region


#Region "DropDown Button für New und Print..."


	''' <summary>
	''' shows contextmenu for print-button
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub cmdPrint_Click(sender As System.Object, e As System.EventArgs) Handles cmdPrint.Click
		Me.cmdPrint.ShowDropDown()
	End Sub

	''' <summary>
	''' shows Contexmenu for New-button
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub cmdNew_Click(sender As System.Object, e As System.EventArgs) Handles cmdNew.Click
		Me.cmdNew.ShowDropDown()
	End Sub


#End Region


#Region "Mandantendaten auswählen..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		cboMD.Properties.DisplayMember = "MDName"
		cboMD.Properties.ValueMember = "MDNr"

		Dim columns = cboMD.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
																					 .Width = 100,
																					 .Caption = "Mandant"})

		cboMD.Properties.ShowHeader = False
		cboMD.Properties.ShowFooter = False

		cboMD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cboMD.Properties.SearchMode = SearchMode.AutoComplete
		cboMD.Properties.AutoSearchColumnIndex = 0

		cboMD.Properties.NullText = String.Empty
		cboMD.EditValue = Nothing

	End Sub

	' Mandantendaten...
	Private Sub OnCboMD_EditValueChanged(sender As Object, e As System.EventArgs) Handles cboMD.EditValueChanged
		Dim SelectedData As MDData = TryCast(Me.cboMD.GetSelectedDataRow(), MDData)
		Dim OldMDNr = ModulConstants.MDData.MDNr

		If Not SelectedData Is Nothing Then

			ModulConstants.MDData.MDNr = cboMD.EditValue
			If OldMDNr <> SelectedData.MDNr Then
				CheckUserProfileDataForNewMandant()

				ResetMainGrid()
				LoadFoundedReportList()
			End If
		End If

		Me.grdMain.Enabled = Not (cboMD.EditValue Is Nothing)
		pccMandant.HidePopup()

	End Sub

	Private Sub CheckUserProfileDataForNewMandant()

		Try
			Dim oldUserProfilePathfilename = Path.Combine(ModulConstants.MDData.MDUserProfilesPath, String.Format("UserProfile{0}.xml", ModulConstants.UserData.UserNr))
			ModulConstants.MDData = ModulConstants.SelectedMDData(cboMD.EditValue)
			Dim newUserProfilePathfilename = Path.Combine(ModulConstants.MDData.MDUserProfilesPath, String.Format("UserProfile{0}.xml", ModulConstants.UserData.UserNr))
			If Not File.Exists(newUserProfilePathfilename) AndAlso File.Exists(oldUserProfilePathfilename) Then
				File.Copy(oldUserProfilePathfilename, newUserProfilePathfilename)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Function LoadFoundedMDList() As Boolean
		cboMD.Properties.DataSource = ListOfMandantData

		Return Not ListOfMandantData Is Nothing
	End Function

	''' <summary>
	''' Zeigt pcc-Container für Mandantenauswahl
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub grpFunction_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpFunction.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then

			pccMandant.SuspendLayout()
			Me.pccMandant.Manager = New DevExpress.XtraBars.BarManager
			pccMandant.ShowCloseButton = True
			pccMandant.ShowSizeGrip = True

			pccMandant.ShowPopup(Cursor.Position)
			pccMandant.ResumeLayout()

		Else
			RefreshData()

		End If

	End Sub

	Private Sub OnpageVisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

		pccMandant.HidePopup()
		If Me.Visible Then RefreshMainViewStateBar()
		If LoadedMDNr <> ModulConstants.MDData.MDNr Then
			LoadedMDNr = ModulConstants.MDData.MDNr
			ResetMandantenDropDown()
			Dim supportedData = New SuportedCodes
			ListOfMandantData = supportedData.LoadFoundedMDList()

			LoadFoundedMDList()
			cboMD.EditValue = LoadedMDNr

			RefreshData()

		End If
		m_SuppressUIEvents = If(Me.Visible, False, True)

	End Sub

	Private Sub RefreshData()
		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()
		LoadFoundedReportList()

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub RefreshMainViewStateBar()

		m_communicationHub.Publish(New RefreshMainViewStatebar(Me, Me.gvMain.RowCount, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
		cmdNew.Enabled = ModulConstants.MDData.ClosedMD = 0

	End Sub

#End Region









	''' <summary>
	''' Klick auf einzelne Detailbuttons für die Anzeige der Daten
	''' </summary>
	Private Sub OngrpZG_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpZG.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._RPSetting.Data4SelectedRP = True
			strModul2Open = "zg".ToLower

			Dim frm As New frmReportDetails(Me._RPSetting, strModul2Open)
			frm.Show()
		End If

	End Sub

	Private Sub OngrpRE_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpRE.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._RPSetting.Data4SelectedRP = True
			strModul2Open = "re".ToLower

			Dim frm As New frmReportDetails(Me._RPSetting, strModul2Open)
			frm.Show()
		End If

	End Sub


#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case GridName.ToLower
			Case "gvmain".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingMainFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreMainSetting), True)

				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVMainSettingfilename) Then gvMain.RestoreLayoutFromXml(m_GVMainSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvMain.ActiveFilterCriteria = Nothing


			Case "gvzg".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingZGFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreZGSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVZGSettingfilename) Then gvZG.RestoreLayoutFromXml(m_GVZGSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvZG.ActiveFilterCriteria = Nothing

			Case "gvInvoice".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingREFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRESetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVInvoiceSettingfilename) Then gvInvoice.RestoreLayoutFromXml(m_GVInvoiceSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvInvoice.ActiveFilterCriteria = Nothing


			Case Else

				Exit Sub


		End Select


	End Sub


	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub

	Private Sub OngvZGColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvZG.SaveLayoutToXml(m_GVZGSettingfilename)

	End Sub

	Private Sub OngvInvoiceColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvInvoice.SaveLayoutToXml(m_GVInvoiceSettingfilename)

	End Sub


#End Region



#Region "Helpers"

	'Private Sub BuildNewPrintContextMenu()

	'	Try
	'		' build contextmenu
	'		Dim _ClsNewPrint As New ClsReportModuls(Me._RPSetting)
	'		_ClsNewPrint.ShowContextMenu4Print(Me.cmdPrint)
	'		_ClsNewPrint.ShowContextMenu4New(cmdNew)


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'	End Try

	'End Sub


	Private Sub BuildNewPrintContextMenu()
		Dim m_DataAccess As New MainGrid
		Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
		Dim popupMenu2 As New DevExpress.XtraBars.PopupMenu

		Try
			' build contextmenu
			Dim mnuData = m_DataAccess.LoadContextMenu4PrintReportData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Return
			End If

			'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu1.ClearLinks()
			popupMenu2.ClearLinks()

			cmdPrint.DropDownControl = popupMenu1
			popupMenu1.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				Dim AddmnuItem As Boolean = True
				itm = New DevExpress.XtraBars.BarButtonItem

				itm.Caption = (mnuData(i).MnuCaption)
				itm.Name = mnuData(i).MnuName

				If _RPSetting.PrintNoRP.GetValueOrDefault(False) AndAlso (mnuData(i).MnuName = "RPPrintMontly" OrElse mnuData(i).MnuName = "RPPrint") Then AddmnuItem = False

				If AddmnuItem Then

					If Not mnuData(i).MnuGrouped Then
						popupMenu1.AddItem(itm)
					Else
						popupMenu1.AddItem(itm).BeginGroup = True
					End If

					AddHandler itm.ItemClick, AddressOf GetMnuItem4Print

				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Try
			cmdNew.DropDownControl = popupMenu2
			' build contextmenu
			Dim mnuData = m_DataAccess.LoadContextMenu4NewReportData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Exit Sub
			End If

			'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu2.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem

				itm.Caption = (mnuData(i).MnuCaption)
				itm.Name = mnuData(i).MnuName
				If itm.Name.ToLower = "RPNew".ToLower Then
					If Not ModulConstants.UserSecValue(301) Then Continue For
				ElseIf itm.Name.ToLower = "zgNew".ToLower Then
					If Not ModulConstants.UserSecValue(350) Then Continue For
				ElseIf itm.Name.ToLower = "LONew".ToLower Then
					If Not ModulConstants.UserSecValue(550) Then Continue For
				ElseIf itm.Name.ToLower = "DTANew".ToLower Then
					If Not ModulConstants.UserSecValue(562) Then Continue For
				End If

				If Not mnuData(i).MnuGrouped Then
					popupMenu2.AddItem(itm)
				Else
					popupMenu2.AddItem(itm).BeginGroup = True
				End If
				AddHandler itm.ItemClick, AddressOf GetMnuItem4New
			Next


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


	End Sub


	Sub GetMnuItem4Print(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuName As String = e.Item.Name.ToLower
		Dim newVersion As Boolean = True ' My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.AltKeyDown


		Select Case strMnuName
			Case "RPPrint".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
											.SelectedKDNr = Me._RPSetting.SelectedKDNr,
											.SelectedESNr = Me._RPSetting.SelectedESNr,
											.SelectedRPNr = Me._RPSetting.SelectedRPNr,
											.SelectedMonth = _RPSetting.SelectedMonth,
											.SelectedYear = _RPSetting.SelectedYear})


				obj.PrintAssignedReport()


			Case "RPPrintMontly".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedRPNr = Me._RPSetting.SelectedRPNr, .SelectedESNr = Me._RPSetting.SelectedESNr, .SelectedMonth = _RPSetting.SelectedMonth, .SelectedYear = _RPSetting.SelectedYear})
				obj.PrintAssignedReport()


			Case "SuvaStd".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr})
				obj.PrintSuvaStdListe4SelectedEmployee()

			Case "ZV".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
														 .SelectedMonth = Me._RPSetting.SelectedMonth,
														 .SelectedYear = Me._RPSetting.SelectedYear})
				obj.PrintZV4SelectedEmployee()

			Case "Arg".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr})
				obj.PrintARG4SelectedEmployee()


			Case "LOPrint".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
														 .SelectedLONr = Me._RPSetting.SelectedLONr})
				If Me._RPSetting.SelectedLONr = 0 Then
					obj.PrintMALO()
				Else
					obj.PrinSelectedtMAPayroll()
				End If

			Case "LOPrintMore".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedLONr = 0})
				obj.PrintMALO()

			Case "LSTNotDoneRP".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedLONr = Me._RPSetting.SelectedLONr})
				obj.PrintNotFinshedReportList()

			Case "PrintRPContentIntoPDF".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedLONr = Me._RPSetting.SelectedLONr})
				obj.PrintRPDataIntoPDF()

			Case "CompareRPDaten".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedLONr = Me._RPSetting.SelectedLONr})
				obj.PrintRPCompareData()

			Case "StdControl".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedLONr = Me._RPSetting.SelectedLONr})
				obj.PrintRPStdData()

			Case "10.0.2".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._RPSetting.SelectedKDNr,
																												 .SelectedYear = Me._RPSetting.SelectedYear})
				obj.PrintStdData()

			Case "ListDowntimeData".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._RPSetting.SelectedKDNr,
																												 .SelectedYear = Me._RPSetting.SelectedYear})
				obj.PrintKAEListingData()


			Case Else
				Exit Sub

		End Select

	End Sub

	Sub GetMnuItem4New(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuName As String = e.Item.Name.ToLower

		Select Case strMnuName
			Case "RPNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._RPSetting.SelectedMDNr,
														 .SelectedRPNr = Nothing,
														 .SelectedMonth = Me._RPSetting.SelectedMonth,
														 .SelectedYear = Me._RPSetting.SelectedYear})
				obj.OpenReportFormForNewRP()

			Case "ZGNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._RPSetting.SelectedMDNr,
																												 .SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedZGNr = Nothing,
																												 .SelectedRPNr = Me._RPSetting.SelectedRPNr})
				obj.OpenSelectedAdvancePayment(Me._RPSetting.SelectedMDNr, ModulConstants.UserData.UserNr)

			Case "LONew".ToLower
				Dim _ClsLO As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._RPSetting.SelectedMDNr,
															.SelectedMonth = Me._RPSetting.SelectedMonth,
															.SelectedYear = Me._RPSetting.SelectedYear})
				_ClsLO.OpenPayrollForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


			Case "DTANew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._RPSetting.SelectedMDNr,
																												 .SelectedMANr = Me._RPSetting.SelectedMANr,
																												 .SelectedZGNr = 0})
				obj.OpenNewDTAForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "AutoRPScan".ToLower, "DocScan".ToLower
				Dim _ClsLO As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._RPSetting.SelectedMDNr,
																														.SelectedMonth = Me._RPSetting.SelectedMonth,
																														.SelectedYear = Me._RPSetting.SelectedYear})
				_ClsLO.OpenAutoRPScan()


			Case Else
				Exit Sub

		End Select

	End Sub



	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = TranslateText(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = ModulConstants.MDData
		Dim logedUserData = ModulConstants.UserData
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

#End Region



End Class
