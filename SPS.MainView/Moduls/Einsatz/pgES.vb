
Imports SPS.MainView.DataBaseAccess
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
Imports System.ComponentModel

Imports SP.Infrastructure.Settings
Imports SPS.MainView.ESSettings

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Utils.Win
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.Utils.VisualEffects
Imports SPProgUtility.SPTranslation
Imports SP.Infrastructure.DateAndTimeCalculation
Imports DevExpress.XtraEditors.ViewInfo

Public Class pgES


#Region "private consts"

	Private Const MODUL_NAME = "Einsatzverwaltung"
	Private Const MODUL_NAME_SETTING = "employment"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Einsatzverwaltung"


	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/rp/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/rp/keepfilter"

	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NR As String = "MD_{0}/Sonstiges/autofilterconditionnr"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE As String = "MD_{0}/Sonstiges/autofilterconditiondate"

#End Region

#Region "Private Fields"

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private Property LoadedMDNr As Integer

	Protected m_SettingsManager As ISettingsManager

	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Private m_MandantData As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()

	Private m_griddata As GridData

	Private _iSelectedRecNr As Integer

	Private _ClsESPropertiesSetting As New ClsESPropertySetting
	Private _ESSetting As ClsESSetting

	Private Property gvReportDisplayMember As String

	Private m_GVMainSettingfilename As String
	Private m_GVReportSettingfilename As String

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingRPFilter As String
	Private m_xmlSettingRestoreRPSetting As String

	Private m_autofilterconditionNumber As AutoFilterCondition
	Private m_autofilterconditionDate As AutoFilterCondition
	Private m_AllowedChangeMandant As Boolean

	Private m_Badge As DevExpress.Utils.VisualEffects.Badge
	Private m_MainViewGridData As SuportedCodes

	Private m_DateAndTimeUtility As DateAndTimeUtily

#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		m_Badge = New Badge
		m_UtilityUI = New UtilityUI

		m_InitializationData = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		BarManager1.Form = Me
		LoadedMDNr = ModulConstants.MDData.MDNr
		Me._ESSetting = New ClsESSetting
		m_SettingsManager = New SettingsESManager

		m_MandantData = New Mandant
		m_utility = New Utilities
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_translate = New TranslateValues
		m_DateAndTimeUtility = New DateAndTimeUtily

		dpProperties.Options.ShowCloseButton = False

		Try
			m_communicationHub = MessageService.Instance.Hub

			m_GVMainSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvMain.Name, ModulConstants.UserData.UserNr)
			m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)


			m_GVReportSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvRP.Name, ModulConstants.UserData.UserNr)
			m_xmlSettingRestoreRPSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingRPFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
			'm_autofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
			'm_autofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


		AddHandler gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
		AddHandler gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
		AddHandler gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
		AddHandler gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler gvRP.RowCellClick, AddressOf OngvRP_RowCellClick
		AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvReportColumnPositionChanged
		AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvReportColumnPositionChanged
		AddHandler Me.gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground


		grpFunction.CustomHeaderButtons(0).Properties.Visible = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200004)
		m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200004)
		grpFunction.CustomHeaderButtons(0).Properties.Enabled = m_AllowedChangeMandant

		'LoadXMLDataForSelectedModule()
		m_MainViewGridData = New SuportedCodes
		m_MainViewGridData.ChangeColumnNamesToLowercase = True
		m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
		m_griddata = m_MainViewGridData.LoadMainGridData

		ResetGrid()

		ResetMandantenDropDown()
		LoadFoundedMDList()

		cboMD.EditValue = ModulConstants.MDData.MDNr

	End Sub

#End Region


#Region "Private Properties"

	Private ReadOnly Property AutoFilterConditionNumber() As AutoFilterCondition
		Get
			Dim value As String = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NR, ModulConstants.MDData.MDNr))
			Dim result As AutoFilterCondition
			If String.IsNullOrWhiteSpace(value) Then
				result = AutoFilterCondition.Contains
			Else
				result = value
			End If

			Return result

		End Get
	End Property

	Private ReadOnly Property AutoFilterConditionDate() As AutoFilterCondition
		Get
			Dim value As AutoFilterCondition = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE, ModulConstants.MDData.MDNr))
			Dim result As AutoFilterCondition
			If String.IsNullOrWhiteSpace(value) Then
				result = AutoFilterCondition.Contains
			Else
				result = value
			End If

			Return value

		End Get
	End Property

	Private ReadOnly Property SelectedRowViewData As FoundedESData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedESData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


	Private Sub pg_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedESList()

	End Sub


#Region "Private methods"

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpRP.Text = m_translate.GetSafeTranslationValue(Me.grpRP.Text)

		Me.dpProperties.Text = m_translate.GetSafeTranslationValue(Me.dpProperties.Text)

	End Sub

	'Sub LoadXMLDataForSelectedModule()

	'	m_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	If m_griddata.SQLQuery Is Nothing Then
	'		m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	End If

	'	aColCaption = m_griddata.GridColCaption.Split(CChar(";"))
	'	aColFieldName = m_griddata.GridColFieldName.Split(CChar(";"))
	'	aColWidth = m_griddata.GridColWidth.Split(CChar(";"))

	'End Sub

	'Private Function GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_MandantData.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'														 And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'						   Select New With {
	'																	 .SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																	 .GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																	 .DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																	 .GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																	 .GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																	 .BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																	 .ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																	 .PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																	 .PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																	 .CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																	 .FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																	 .CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																		 }).FirstOrDefault()



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
	'		Dim xDoc As XDocument = XDocument.Load(m_MandantData.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'														 And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'						   Select New With {
	'																	 .SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																	 .GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																	 .DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																	 .GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																	 .GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																	 .BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																	 .ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																	 .PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																	 .PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																	 .CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																	 .FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																	 .CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																		 }).FirstOrDefault()


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

	Private Sub SetFormLayout()

		Me.Parent.Text = String.Format("{0}", m_translate.GetSafeTranslationValue(UsercontrolCaption))
		Try
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingESKeys.SETTING_ES_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingESKeys.SETTING_ES_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingESKeys.SETTING_ES_SCC_PROPERTY_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainNav_1.SplitterPosition = Math.Max(Me.sccMainNav_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingESKeys.SETTING_ES_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperties.Width = Math.Max(Me.dpProperties.Width, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75

		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill

		Me.gvRProperty.OptionsView.ShowIndicator = False
		Me.gvlProperty.OptionsView.ShowIndicator = False

		Me.gvMain.OptionsView.ShowIndicator = False
		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		Me.gvRP.OptionsView.ShowIndicator = False

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SizeChanged, AddressOf SaveFormProperties
		AddHandler dpProperties.Resize, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingESKeys.SETTING_ES_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingESKeys.SETTING_ES_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingESKeys.SETTING_ES_SCC_PROPERTY_SPLITTERPOSION, Me.sccMainNav_1.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingESKeys.SETTING_ES_DPPROPERTY_WIDTH, Me.dpProperties.Width)

		m_SettingsManager.SaveSettings()

	End Sub



#End Region



	Private Sub ResetGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

	End Sub

	'gvMain.Columns.Clear()
	'grdLProperty.DataSource = Nothing
	'grdRProperty.DataSource = Nothing
	'grdRP.DataSource = Nothing


	'Try
	'	Dim repoHTML = New RepositoryItemHypertextLabel
	'	repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
	'	repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
	'	repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
	'	repoHTML.Appearance.Options.UseTextOptions = True
	'	grdMain.RepositoryItems.Add(repoHTML)

	'	Dim reproCheckbox = New RepositoryItemCheckEdit
	'	reproCheckbox.Appearance.Options.UseTextOptions = True
	'	grdMain.RepositoryItems.Add(reproCheckbox)

	'	Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
	'	Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

	'	For i = 0 To aColCaption.Length - 1

	'		Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
	'		Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
	'		Dim columnName As String = aColFieldName(i).ToLower.Trim

	'		column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'		column.Caption = columnCaption
	'		column.Name = columnName
	'		column.FieldName = columnName

	'		If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
	'			column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
	'			column.DisplayFormat.FormatString = "d"
	'			column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

	'		ElseIf DECIMAL_COLUMN_NAME.ToLower.Contains(columnName) Then
	'			column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'			column.AppearanceHeader.Options.UseTextOptions = True
	'			column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'			column.DisplayFormat.FormatString = "N2"
	'			column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

	'		ElseIf INTEGER_COLUMN_NAME.ToLower.Contains(columnName) Then
	'			column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'			column.AppearanceHeader.Options.UseTextOptions = True
	'			column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'			column.DisplayFormat.FormatString = "F0"
	'			column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

	'		ElseIf CHECKBOX_COLUMN_NAME.Contains(columnName) Then
	'			column.ColumnEdit = reproCheckbox
	'		Else
	'			column.ColumnEdit = repoHTML

	'		End If


	'		column.Visible = If(aColWidth(i) > 0, True, False)
	'		gvMain.Columns.Add(column)

	'	Next

	'	RestoreGridLayoutFromXml(gvMain.Name.ToLower)


	'Catch ex As Exception
	'	m_Logger.LogError(ex.ToString)
	'End Try

	'grdMain.DataSource = Nothing



	Private Function LoadFoundedESList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.GetDbESData4Show(m_griddata.SQLQuery)

		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Return False
		End If

		Dim mergedESData As BindingList(Of FoundedESData) = New BindingList(Of FoundedESData)

		For Each p In listOfFoundedData
			mergedESData.Add(p)
		Next

		Dim calculatedData = LoadPVLNotificationData(mergedESData)

		Dim listDataSource As BindingList(Of FoundedESData) = New BindingList(Of FoundedESData)

		For Each p In calculatedData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource
		RefreshMainViewStateBar()

		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)
		grpFunction.Appearance.Options.UseBackColor = True

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Function LoadPVLNotificationData(ByVal esData As BindingList(Of FoundedESData)) As BindingList(Of FoundedESData)
		Dim result As List(Of FoundedESData) = Nothing

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

			For Each assigedES In esData
				Dim esEnde = If(assigedES.es_ende.HasValue, assigedES.es_ende.Value.Date, New DateTime(3999, 12, 31))
				Dim esLohnBisDate As DateTime = If(assigedES.lobis.HasValue, assigedES.lobis.Value.Date, esEnde)

				If assigedES.esnr = 22872 Then
					Trace.WriteLine(String.Format("{0}-{1} >>> {2}", assigedES.es_als, esEnde, assigedES.lovon))
				End If

				Dim assignedNewsData = m_tempDataMergedNews.Where(Function(x) x.ContractNumber = assigedES.gavnumber And x.PublicationDate >= assigedES.lovon And x.PublicationDate <= esLohnBisDate And x.PublicationDate <= esEnde).FirstOrDefault
				If assignedNewsData Is Nothing Then Continue For


				Dim gavUpdateDate As DateTime = assignedNewsData.PublicationDate

				If esLohnBisDate < gavUpdateDate OrElse esEnde < gavUpdateDate Then Continue For

				' Check if the GAVNr has between updated between ESLohnVon and ESLohnBis.
				If gavUpdateDate >= assigedES.lovon AndAlso gavUpdateDate <= esLohnBisDate Then

					assigedES.gavstate = False
					assigedES.gavnotification = String.Format("{0:G}:<br>{1}", gavUpdateDate, assignedNewsData.Content)

				Else
					assigedES.gavstate = True
					assigedES.gavnotification = String.Empty

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

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
		Dim column = e.Column

		pccMandant.HidePopup()
		AdornerUIManager1.Elements.Remove(m_Badge)
		AdornerUIManager1.Hide()

		Dim viewData = SelectedRowViewData
		If viewData Is Nothing Then Return

		_ESSetting.SelectedMDNr = viewData.mdnr
		_ESSetting.SelectedESNr = viewData.esnr
		_ESSetting.SelectedKDNr = viewData.kdnr
		_ESSetting.SelectedZHDNr = viewData.zhdnr
		_ESSetting.SelectedMANr = viewData.manr
		_ESSetting.PrintNoRP = viewData.PrintNoRP

		_ESSetting.SelectedStartMonth = viewData.StartMonth
		_ESSetting.SelectedStartYear = viewData.StartYear

		_ESSetting.SelectedEndMonth = viewData.EndMonth
		_ESSetting.SelectedEndYear = viewData.EndYear

		BuildNewPrintContextMenu()


		If (e.Clicks = 1) Then
			If column.Name.ToLower = "gavstate" Then
				If Not viewData.gavstate Then
					Dim infoText = String.Format("{0}", viewData.gavnotification)
					m_UtilityUI.ShowStandardBadgeNotification(grdMain, m_Badge, AdornerUIManager1,
																					   m_translate.GetSafeTranslationValue(infoText),
																					   ContentAlignment.TopCenter, TargetElementRegion.Default, BadgePaintStyle.Critical)
				End If
			End If
		End If

		If (e.Clicks = 2) Then

#If DEBUG Then
			Dim frmNewXML As New SPGAV.UI.frmTempDataPVL(m_InitializationData)

			frmNewXML.EmployeeNumber = viewData.manr
			frmNewXML.CustomerNumber = viewData.kdnr
			frmNewXML.CustomerCanton = "AG"
			frmNewXML.EmploymentNumber = viewData.esnr
			frmNewXML.Staging = Not (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Dim success = frmNewXML.LoadData()
			If success Then
				frmNewXML.Show()
				frmNewXML.BringToFront()
			End If

			'Return
#End If


			Select Case column.Name.ToLower

					Case "kdnr", "customername"
						If viewData.kdnr > 0 Then
							If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserFiliale) Then
								If Not String.IsNullOrWhiteSpace(viewData.customerbusinessbranch) Then
									If Not viewData.customerbusinessbranch.Contains(ModulConstants.UserData.UserFiliale) Then
										m_UtilityUI.ShowStandardBadgeNotification(grdMain, m_Badge, AdornerUIManager1,
																					   m_translate.GetSafeTranslationValue("Sie dürfen keine Details abrufen!"),
																					   ContentAlignment.TopCenter, TargetElementRegion.Default, BadgePaintStyle.Critical)
										Return
									End If
								End If
							End If

							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)

						End If

					Case "zname", "zhdnr"
						If viewData.zhdnr > 0 Then
							If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserFiliale) Then
								If Not String.IsNullOrWhiteSpace(viewData.customerbusinessbranch) Then
									If Not viewData.customerbusinessbranch.Contains(ModulConstants.UserData.UserFiliale) Then
										m_UtilityUI.ShowStandardBadgeNotification(grdMain, m_Badge, AdornerUIManager1,
																					   m_translate.GetSafeTranslationValue("Sie dürfen keine Details abrufen!"),
																					   ContentAlignment.TopCenter, TargetElementRegion.Default, BadgePaintStyle.Critical)
										Return
									End If
								End If
							End If

							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.OpenSelectedCPerson()

						End If

					Case "customertelefon"
						If viewData.customertelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = 0})
							_ClsKD.TelefonCallToCustomer(viewData.customertelefon)
						End If
					Case "zmobile"
						If viewData.zmobile.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.zmobile)
						End If
					Case "ztelefon"
						If viewData.ztelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.ztelefon)
						End If


					Case "manr", "employeename"
						If viewData.manr > 0 Then
							If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserFiliale) Then
								If Not String.IsNullOrWhiteSpace(viewData.employeebusinessbranch) Then
									If Not viewData.employeebusinessbranch.Contains(ModulConstants.UserData.UserFiliale) Then
										m_UtilityUI.ShowStandardBadgeNotification(grdMain, m_Badge, AdornerUIManager1,
																					   m_translate.GetSafeTranslationValue("Sie dürfen keine Details abrufen!"),
																					   ContentAlignment.TopCenter, TargetElementRegion.Default, BadgePaintStyle.Critical)

										Return
									End If
								End If
							End If

							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "employeetelefon"
						If viewData.employeetelfon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr, .SelectedKDNr = 0, .SelectedZHDNr = 0})
							_ClsKD.TelefonCallToCustomer(viewData.employeetelfon)
						End If
					Case "employeemobile"
						If viewData.employeemobile.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr, .SelectedKDNr = 0, .SelectedZHDNr = 0})
							_ClsKD.TelefonCallToCustomer(viewData.employeemobile)
						End If

					Case "proposenr", "proposeals", "proposestatus"
						If viewData.proposenr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr, .SelectedProposeNr = viewData.proposenr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.OpenSelectedProposeTiny(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case Else
						If viewData.esnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedESNr = viewData.esnr})
							_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

				End Select

			End If


	End Sub

	Sub GetMnuItem(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMnuItem As String = e.Item.Name.ToString
		Dim strMnuValue As String = e.Item.AccessibleDescription

		Select Case strMnuItem.ToUpper
			Case "firma1".ToUpper
				Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._ESSetting.SelectedKDNr})
				_ClsES.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "kdzname".ToUpper
				Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._ESSetting.SelectedKDNr,
																																	 .SelectedZHDNr = Me._ESSetting.SelectedZHDNr})
				_ClsES.OpenSelectedCPerson() 'ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "maname".ToUpper
				Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._ESSetting.SelectedMANr})
				_ClsES.OpenSelectedEmployee(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "vakanz als".ToUpper
				Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedVakNr = Me._ESSetting.SelectedVakNr})
				_ClsES.OpenSelectedVacancyTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "Propose".ToUpper
				Dim _ClsES As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																															.SelectedProposeNr = Me._ESSetting.SelectedProposeNr})
				_ClsES.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)


			Case Else
				Exit Sub

		End Select

	End Sub

	Private Sub FillPopupFields(ByVal selectedrow As FoundedESData)
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

					Case "stundenlohn"
						strValue = selectedrow.stundenlohn
					Case "tarif"
						strValue = selectedrow.tarif
					Case "bruttomarge"
						strValue = selectedrow.bruttomarge

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
				If selectedrow.esnr = 0 Then Exit Sub

				Me._ESSetting.SelectedMDNr = selectedrow.mdnr

				Me._ESSetting.SelectedESNr = selectedrow.esnr

				Me._ESSetting.SelectedMANr = selectedrow.manr
				Me._ESSetting.SelectedKDNr = selectedrow.kdnr

				FillPopupFields(selectedrow)

			Else

				grdLProperty.DataSource = Nothing
				grdRProperty.DataSource = Nothing
				grdRP.DataSource = Nothing

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4ES()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

	End Sub


	Sub FillProperties4ES()

		ResetRPDetailGrid()
		LoadEmployeeRPDetailList()

	End Sub



#Region "Reports Funktionen..."

	Sub ResetRPDetailGrid()

		gvRP.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvRP.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvRP.OptionsView.ShowGroupPanel = False
		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = False

		gvRP.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "rpnr"
			columnmodulname.FieldName = "rpnr"
			columnmodulname.Visible = True
			gvRP.Columns.Add(columnmodulname)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvRP.Columns.Add(columnBezeichnung)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvRP.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvRP.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvRP.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml(gvRP.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdRP.DataSource = Nothing

	End Sub


	Public Function LoadEmployeeRPDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbEinsatzReportDataForProperties(Me._ESSetting.SelectedESNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedESReportDetailData With
																							 {.mdnr = person.mdnr,
																								.employeeMDNr = person.employeeMDNr,
																								.customerMDNr = person.customerMDNr,
																								.rpnr = person.rpnr,
																								.kdnr = person.kdnr,
																								.manr = person.manr,
																								.periode = person.periode,
																								.createdfrom = person.createdfrom,
																								.createdon = person.createdon,
																								.zfiliale = person.zfiliale
																							 }).ToList()

		Dim listDataSource As BindingList(Of FoundedESReportDetailData) = New BindingList(Of FoundedESReportDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvRP_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedESReportDetailData)

				If viewData.rpnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
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

				ResetGrid()
				LoadFoundedESList()
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

	End Sub

	Private Sub RefreshData()
		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		m_InitializationData = CreateInitialData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
		ResetGrid()
		LoadFoundedESList()

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub RefreshMainViewStateBar()

		m_communicationHub.Publish(New RefreshMainViewStatebar(Me, Me.gvMain.RowCount, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
		cmdNew.Enabled = ModulConstants.MDData.ClosedMD = 0

	End Sub

#End Region



#Region "Grid Setting"

	Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case GridName.ToLower
			Case "gvmain".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingMainFilter, False), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreMainSetting, False), True)

				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVMainSettingfilename) Then gvMain.RestoreLayoutFromXml(m_GVMainSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvMain.ActiveFilterCriteria = Nothing


			Case "gvRP".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRPFilter, False), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRPSetting, False), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVReportSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVReportSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing


			Case Else

				Exit Sub


		End Select


	End Sub

	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub

	Private Sub OngvReportColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVReportSettingfilename)

	End Sub


#End Region


	''' <summary>
	''' Klick auf einzelne Detailbuttons für die Anzeige der Daten
	''' </summary>
	Private Sub OngrpRP_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpRP.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._ESSetting.Data4SelectedES = True
			strModul2Open = "RP".ToLower

			Dim frm As New frmESDetails(Me._ESSetting, strModul2Open)
			frm.Show()
		End If

	End Sub


#Region "Helpers"

	'Private Sub BuildNewPrintContextMenu()

	'	Try
	'		' build contextmenu
	'		Dim _ClsNewPrint As New ClsESModuls(Me._ESSetting)
	'		_ClsNewPrint.ShowContextMenu4Print(Me.cmdPrint)
	'		_ClsNewPrint.ShowContextMenu4New(cmdNew)


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'	End Try

	'End Sub

	Private Sub BuildNewPrintContextMenu()
		Dim m_DataAccess As New MainGrid

		Try
			' build contextmenu
			Dim mnuData = m_DataAccess.LoadContextMenu4PrintEmploymentData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Return
			End If

			'Dim popupMenu As New DevExpress.XtraBars.PopupMenu

			PopupMenu1 = New DevExpress.XtraBars.PopupMenu
			PopupMenu1.Manager = Me.BarManager1



			'PopupMenu1.ClearLinks()
			'PopupMenu2.ClearLinks()
			cmdPrint.DropDownControl = PopupMenu1
			PopupMenu1.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				Dim AddmnuItem As Boolean = True
				itm = New DevExpress.XtraBars.BarButtonItem

				itm.Caption = (mnuData(i).MnuCaption)
				itm.Name = mnuData(i).MnuName

				If _ESSetting.PrintNoRP.GetValueOrDefault(False) AndAlso (mnuData(i).MnuName = "RPPrintMontly" OrElse mnuData(i).MnuName = "RPPrint") Then AddmnuItem = False


				If itm.Name.ToLower = "ZV".ToLower Then
					If Not ModulConstants.UserSecValue(108) Then Continue For
				ElseIf itm.Name.ToLower = "ARG".ToLower Then
					If Not ModulConstants.UserSecValue(110) Then Continue For
				ElseIf itm.Name.ToLower = "RPPrint".ToLower OrElse itm.Name.ToLower = "RPPrintMontly".ToLower Then
					If Not ModulConstants.UserSecValue(303) Then Continue For

				ElseIf itm.Name.ToLower = "AllESVertrag".ToLower Then
					If Not ModulConstants.UserSecValue(253) AndAlso Not ModulConstants.UserSecValue(255) Then Continue For
				ElseIf itm.Name.ToLower = "ESVertrag".ToLower Then
					If Not ModulConstants.UserSecValue(253) Then Continue For
				ElseIf itm.Name.ToLower = "VerleihVertrag".ToLower Then
					If Not ModulConstants.UserSecValue(255) Then Continue For
				End If

				If AddmnuItem Then

					If Not mnuData(i).MnuGrouped Then
						PopupMenu1.AddItem(itm)
					Else
						PopupMenu1.AddItem(itm).BeginGroup = True
					End If

					AddHandler itm.ItemClick, AddressOf GetMnuItem4Print

				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Try

			PopupMenu2 = New DevExpress.XtraBars.PopupMenu
			PopupMenu2.Manager = Me.BarManager1

			cmdNew.DropDownControl = PopupMenu2
			' build contextmenu
			Dim mnuData = m_DataAccess.LoadContextMenu4NewEmploymentData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Exit Sub
			End If

			'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			'PopupMenu2.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem

				itm.Caption = (mnuData(i).MnuCaption)
				itm.Name = mnuData(i).MnuName

				If itm.Name.ToLower = "ESNew".ToLower Then
					If Not ModulConstants.UserSecValue(251) Then Continue For
				ElseIf itm.Name.ToLower = "zgNew".ToLower Then
					If Not ModulConstants.UserSecValue(350) Then Continue For
				End If

				If Not mnuData(i).MnuGrouped Then
					PopupMenu2.AddItem(itm)
				Else
					PopupMenu2.AddItem(itm).BeginGroup = True
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
			Case "AllESVertrag".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = _ESSetting.SelectedMDNr,
																 .SelectedESNr = Me._ESSetting.SelectedESNr})
				obj.PrintESVertrag_(False, True, False)

			Case "ESVertrag".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ESSetting.SelectedMDNr,
																 .SelectedESNr = Me._ESSetting.SelectedESNr})
				obj.PrintESVertrag_(False, False, False)

			Case "VerleihVertrag".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ESSetting.SelectedMDNr,
																 .SelectedESNr = Me._ESSetting.SelectedESNr})
				obj.PrintESVertrag_(True, False, False)

			Case "ZV".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ESSetting.SelectedMDNr,
																 .SelectedESNr = Me._ESSetting.SelectedESNr,
																 .SelectedMANr = _ESSetting.SelectedMANr})
				obj.PrintZV4SelectedEmployee()

			Case "ARG".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ESSetting.SelectedMDNr,
																 .SelectedESNr = Me._ESSetting.SelectedESNr,
																 .SelectedMANr = _ESSetting.SelectedMANr})
				obj.PrintARG4SelectedEmployee()

			Case "AllEmployeeForgottenZVARGB".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ESSetting.SelectedMDNr})
				obj.PrintEmployeeForgottenZVARGB()

			Case "RPPrint".ToLower
				Dim year As Integer = Now.Year
				Dim month As Integer = Now.Month

				If _ESSetting.SelectedEndMonth.HasValue Then
					If _ESSetting.SelectedEndMonth <= month Then
						month = _ESSetting.SelectedEndMonth
					Else
						If _ESSetting.SelectedStartMonth > month Then month = _ESSetting.SelectedStartMonth
					End If
				End If

				If _ESSetting.SelectedEndYear.HasValue Then
					If _ESSetting.SelectedEndYear <= year Then
						year = _ESSetting.SelectedEndYear
					Else
						If _ESSetting.SelectedStartYear > year Then year = _ESSetting.SelectedStartYear
					End If
				End If

				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Me._ESSetting.SelectedMDNr,
																 .SelectedESNr = Me._ESSetting.SelectedESNr,
																 .SelectedMANr = Me._ESSetting.SelectedMANr,
																 .SelectedKDNr = Me._ESSetting.SelectedKDNr,
																 .SelectedMonth = month,
																 .SelectedYear = year})

				obj.PrintAssignedReport()


			Case Else
				Exit Sub

		End Select

	End Sub

	Sub GetMnuItem4New(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuName As String = e.Item.Name.ToLower

		Select Case strMnuName
			Case "ESNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = Nothing,
																 .SelectedESNr = Nothing,
																 .SelectedMANr = Nothing,
																 .SelectedKDNr = Nothing, .SelectedZHDNr = Nothing,
																 .SelectedVakNr = Nothing, .SelectedProposeNr = Nothing})
				obj.OpenNewESForm(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "RPNew".ToLower


			Case "zgNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = _ESSetting.SelectedMDNr,
																														 .SelectedMANr = _ESSetting.SelectedMANr,
																														 .SelectedZGNr = Nothing,
																														 .SelectedRPNr = Nothing})
				obj.OpenSelectedAdvancePayment(Me._ESSetting.SelectedMDNr, ModulConstants.UserData.UserNr)


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


#End Region


	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = ModulConstants.MDData
		Dim logedUserData = ModulConstants.UserData
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

End Class




