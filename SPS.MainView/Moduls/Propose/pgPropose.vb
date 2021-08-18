
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.SPUserSec.ClsUserSec
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
Imports SPS.MainView.CustomerSettings
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.ModulView
Imports SP.DatabaseAccess.ModulView.DataObjects

Public Class pgPropose

#Region "private consts"

	Private Const MODUL_NAME = "Vorschlagverwaltung"
	Private Const MODUL_NAME_SETTING = "propose"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Vorschlagverwaltung"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/proposeproperties/es/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER As String = "gridsetting/User_{0}/mainview/{1}/proposeproperties/es/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_JOBTERMIN_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/proposeproperties/jobtermin/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_JOBTERMIN_FILTER As String = "gridsetting/User_{0}/mainview/{1}/proposeproperties/jobtermin/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/proposeproperties/contact/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_FILTER As String = "gridsetting/User_{0}/mainview/{1}/proposeproperties/contact/keepfilter"

	Private Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;ourisonline;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline"

#End Region


#Region "private fields"

	''' <summary>
	''' The modulview database access.
	''' </summary>
	Protected m_ModulViewDatabaseAccess As IModulViewDatabaseAccess

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private Property LoadedMDNr As Integer
	Protected m_SettingsManager As ISettingsManager

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

	Dim _iSelectedRecNr As Integer

	Private _ClsPPropertiesSetting As New ClsPPropertySetting
	Dim _PSetting As ClsProposeSetting

	Private m_UitilityUI As UtilityUI


	Private m_GVMainSettingfilename As String
	Private m_GVESSettingfilename As String
	Private m_GVJobTerminSettingfilename As String
	Private m_GVContactSettingfilename As String

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingESFilter As String
	Private m_xmlSettingRestoreESSetting As String

	Private m_xmlSettingJobTerminFilter As String
	Private m_xmlSettingRestoreJobTerminSetting As String

	Private m_xmlSettingContactFilter As String
	Private m_xmlSettingRestoreContactSetting As String
	Private m_AllowedChangeMandant As Boolean
	Private m_MainViewGridData As SuportedCodes


#End Region



#Region "Private property"

	Private ReadOnly Property SelectedRowViewData As FoundedProposeData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedProposeData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region

#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		BarManager1.Form = Me
		LoadedMDNr = ModulConstants.MDData.MDNr
		m_SettingsManager = New SettingsProposeManager

		Me._PSetting = New ClsProposeSetting
		m_md = New Mandant
		m_utility = New Utilities
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_translate = New TranslateValues
		m_UitilityUI = New UtilityUI
		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		dpProperties.Options.ShowCloseButton = False

		Try
			m_communicationHub = MessageService.Instance.Hub

			m_GVMainSettingfilename = String.Format("{0}Propose\{1}{2}.xml", ModulConstants.GridSettingPath, gvMain.Name, ModulConstants.UserData.UserNr)
			m_GVESSettingfilename = String.Format("{0}Propose\{1}{2}.xml", ModulConstants.GridSettingPath, gvES.Name, ModulConstants.UserData.UserNr)
			m_GVJobTerminSettingfilename = String.Format("{0}Propose\{1}{2}.xml", ModulConstants.GridSettingPath, gvInerview.Name, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilename = String.Format("{0}Propose\{1}{2}.xml", ModulConstants.GridSettingPath, gvContact.Name, ModulConstants.UserData.UserNr)


			m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreESSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingESFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreJobTerminSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_JOBTERMIN_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingJobTerminFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_JOBTERMIN_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreContactSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingContactFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


		AddHandler Me.gvMain.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
		AddHandler Me.gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged


		AddHandler Me.gvES.RowCellClick, AddressOf OngvES_RowCellClick
		AddHandler Me.gvES.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvES.ColumnPositionChanged, AddressOf OngvESColumnPositionChanged
		AddHandler Me.gvES.ColumnWidthChanged, AddressOf OngvESColumnPositionChanged

		AddHandler Me.gvInerview.RowCellClick, AddressOf OngvInterview_RowCellClick
		AddHandler Me.gvInerview.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvInerview.ColumnPositionChanged, AddressOf OngvJobTerminColumnPositionChanged
		AddHandler Me.gvInerview.ColumnWidthChanged, AddressOf OngvJobTerminColumnPositionChanged

		AddHandler Me.gvContact.RowCellClick, AddressOf OngvContact_RowCellClick
		AddHandler Me.gvContact.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvContact.ColumnPositionChanged, AddressOf OngvContactColumnPositionChanged
		AddHandler Me.gvContact.ColumnWidthChanged, AddressOf OngvContactColumnPositionChanged

		m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200003)

		ResetMandantenDropDown()
		LoadFoundedMDList()
		cboMD.EditValue = ModulConstants.MDData.MDNr

		m_MainViewGridData = New SuportedCodes
		m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
		m_griddata = m_MainViewGridData.LoadMainGridData

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()

	End Sub

#End Region


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
	'		Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'													Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'															And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME) _
	'															Select New With {
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

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingProposeKeys.SETTING_P_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingProposeKeys.SETTING_P_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingProposeKeys.SETTING_P_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperties.Width = Math.Max(Me.dpProperties.Width, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingProposeKeys.SETTING_P_SCC_NAV_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainNav_1.SplitterPosition = Math.Max(Me.sccMainNav_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingProposeKeys.SETTING_P_SCC_PROPERTY_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainProp_1.SplitterPosition = Math.Max(Me.sccMainProp_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingProposeKeys.SETTING_P_SCC_PROPERTY_1_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainProp_1_1.SplitterPosition = Math.Max(Me.sccMainProp_1_1.SplitterPosition, setting_splitterpos)


		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill

		Me.gvRProperty.OptionsView.ShowIndicator = False
		Me.gvLProperty.OptionsView.ShowIndicator = False

		Me.gvMain.OptionsView.ShowIndicator = False
		Me.gvContact.OptionsView.ShowIndicator = False
		Me.gvES.OptionsView.ShowIndicator = False

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowAutoFilterRow = True


		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties

		AddHandler dpProperties.Resize, AddressOf SaveFormProperties

		AddHandler Me.sccMainNav_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_1_1.SplitterPositionChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		m_SettingsManager.WriteInteger(SettingProposeKeys.SETTING_P_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingProposeKeys.SETTING_P_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingProposeKeys.SETTING_P_DPPROPERTY_WIDTH, Me.dpProperties.Width)

		m_SettingsManager.WriteInteger(SettingProposeKeys.SETTING_P_SCC_NAV_1_SPLITTERPOSION, Me.sccMainNav_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingProposeKeys.SETTING_P_SCC_PROPERTY_1_SPLITTERPOSION, Me.sccMainProp_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingProposeKeys.SETTING_P_SCC_PROPERTY_1_1_SPLITTERPOSION, Me.sccMainProp_1_1.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub

	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		gvMain.Columns.Clear()

		grdLProperty.DataSource = Nothing
		grdRProperty.DataSource = Nothing

		grdES.DataSource = Nothing
		grdInterview.DataSource = Nothing
		grdContact.DataSource = Nothing

		Try
			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdMain.RepositoryItems.Add(repoHTML)

			Dim reproCheckbox = New RepositoryItemCheckEdit
			reproCheckbox.Appearance.Options.UseTextOptions = True
			grdMain.RepositoryItems.Add(reproCheckbox)

			Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
			Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

			For i = 0 To aColCaption.Length - 1

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
				Dim columnName As String = aColFieldName(i).ToLower.Trim

				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = columnCaption
				column.Name = columnName
				column.FieldName = columnName

				If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
					column.DisplayFormat.FormatString = "d"
					column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

				ElseIf DECIMAL_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
					column.AppearanceHeader.Options.UseTextOptions = True
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
					column.DisplayFormat.FormatString = "N2"
					column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

				ElseIf INTEGER_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
					column.AppearanceHeader.Options.UseTextOptions = True
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
					column.DisplayFormat.FormatString = "F0"
					column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

				ElseIf CHECKBOX_COLUMN_NAME.Contains(columnName) Then
					column.ColumnEdit = reproCheckbox
				Else
					column.ColumnEdit = repoHTML
				End If


				column.Visible = If(aColWidth(i) > 0, True, False)
				gvMain.Columns.Add(column)

			Next

			RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMain.DataSource = Nothing

	End Sub


	Private Function LoadFoundedProposeList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.GetDbProposeData4Show(m_griddata.SQLQuery)
		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Exit Function
		End If

		Dim responsiblePersonsGridData = (From person In listOfFoundedData
										  Select New FoundedProposeData With
					 {
						._res = person._res,
						.mdnr = person.mdnr,
						.pnr = person.pnr,
						.manr = person.manr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.vaknr = person.vaknr,
						.honorar = person.honorar,
						.part = person.part,
						.panstellung = person.panstellung,
						.advisor = person.advisor,
						.firma1 = person.firma1,
						.zname = person.zname,
						.maname = person.maname,
						.bezeichnung = person.bezeichnung,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.pstate = person.pstate,
						.vakals = person.vakals,
						.vakcreatedon = person.vakcreatedon,
						.kdemail = person.kdemail,
						.zemail = person.zemail,
						.jchisonline = person.jchisonline,
						.ourisonline = person.ourisonline,
						.ojisonline = person.ojisonline,
						.kdtelefon = person.kdtelefon,
						.kdtelefax = person.kdtelefax,
						.ztelefon = person.ztelefon,
						.ztelefax = person.ztelefax,
						.znatel = person.znatel,
						.zfiliale = person.zfiliale
}).ToList()

		Dim listDataSource As BindingList(Of FoundedProposeData) = New BindingList(Of FoundedProposeData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource

		RefreshMainViewStateBar()
		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Sub gvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs) Handles gvMain.ColumnFilterChanged

		RefreshMainViewStateBar()

	End Sub

	Private Sub pg_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		TranslateControls()

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedProposeList()

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpES.Text = m_translate.GetSafeTranslationValue(Me.grpES.Text)
		Me.grpJobTermin.Text = m_translate.GetSafeTranslationValue(Me.grpJobTermin.Text)
		Me.grpContact.Text = m_translate.GetSafeTranslationValue(Me.grpContact.Text)

		Me.dpProperties.Text = m_translate.GetSafeTranslationValue(Me.dpProperties.Text)

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedProposeData)
				Me._PSetting.SelectedVakNr = viewData.vaknr
				Me._PSetting.SelectedKDNr = viewData.kdnr
				Me._PSetting.SelectedZHDNr = viewData.zhdnr

				Select Case column.Name.ToLower
					Case "ztelefon"
						If viewData.ztelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.ztelefon)
						End If

					Case "znatel"
						If viewData.znatel.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.znatel)
						End If

					Case "kdtelefon"
						If viewData.kdtelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = 0})
							_ClsKD.TelefonCallToCustomer(viewData.kdtelefon)
						End If

					Case "kdzname", "kdzhdnr"
						If viewData.zhdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
							_ClsKD.OpenSelectedCPerson() 'viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "firma1", "kdnr"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "maname", "manr"
						If viewData.manr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "vakals", "vaknr"
						If viewData.vaknr > 0 Then
							Dim _ClsVak As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr,
																																	 .SelectedVakNr = viewData.vaknr,
																																	 .SelectedKDNr = viewData.kdnr,
																																	 .SelectedZHDNr = viewData.zhdnr})
							_ClsVak.OpenSelectedVacancyTiny(viewData.mdnr, ModulConstants.UserData.UserNr)

						End If

					Case Else
						If viewData.pnr > 0 Then
							Dim _ClsVak As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedProposeNr = viewData.pnr})
							_ClsVak.OpenSelectedProposeTiny(viewData.mdnr, ModulConstants.UserData.UserNr)

						End If

				End Select

			End If

		End If

	End Sub


	Private Sub OngvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				If selectedrow.pnr = 0 Then Exit Sub

				Me._PSetting.SelectedProposeNr = selectedrow.pnr
				Me._PSetting.SelectedMANr = selectedrow.manr
				Me._PSetting.SelectedVakNr = selectedrow.vaknr
				Me._PSetting.SelectedKDNr = selectedrow.kdnr
				Me._PSetting.SelectedKDNr = selectedrow.kdnr
				Me._PSetting.SelectedZHDNr = selectedrow.zhdnr
				If Me._PSetting.SelectedZHDNr.HasValue AndAlso Me._PSetting.SelectedZHDNr = 0 Then
					Me._PSetting.SelectedZHDNr = Nothing
				End If

				_ClsPPropertiesSetting.SelectedProposeNr = Me._PSetting.SelectedProposeNr
				_ClsPPropertiesSetting.SelectedVakNr = Me._PSetting.SelectedVakNr
				_ClsPPropertiesSetting.SelectedMANr = selectedrow.manr
				_ClsPPropertiesSetting.SelectedProposeNr = Me._PSetting.SelectedProposeNr


				FillPopupFields(selectedrow)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4Propose()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try


	End Sub

	Private Sub FillPopupFields(ByVal selectedrow As FoundedProposeData)
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
		If m_griddata.CountOfFieldsInHeader.HasValue Then
			iCountofFieldInHeaderInfo = m_griddata.CountOfFieldsInHeader
		End If

		Try

			For j As Integer = 0 To aPoupFields.Length - 1
				Select Case aPoupFields(j).ToLower
					Case "bezeichnung"
						strValue = selectedrow.bezeichnung
					Case "advisor"
						strValue = selectedrow.advisor

					Case "firma1"
						strValue = selectedrow.firma1

					Case "bezeichung"
						strValue = selectedrow.bezeichnung

					Case "createdfrom"
						strValue = selectedrow.createdfrom

					Case "createdon"
						strValue = selectedrow.createdon

					Case "kdemail"
						strValue = selectedrow.kdemail

					Case "honorar"
						strValue = Format(selectedrow.honorar, "n2")
					Case "part"
						strValue = selectedrow.part
					Case "panstellung"
						strValue = selectedrow.panstellung

					Case "kdnr"
						strValue = selectedrow.kdnr

					Case "jchisonline"
						strValue = selectedrow.jchisonline
					Case "ojisonline"
						strValue = selectedrow.ojisonline
					Case "ourisonline"
						strValue = selectedrow.ourisonline

					Case "kdtelefon"
						strValue = selectedrow.kdtelefon
					Case "kdtelefax"
						strValue = selectedrow.kdtelefax
					Case "ztelefon"
						strValue = selectedrow.ztelefon
					Case "ztelefax"
						strValue = selectedrow.ztelefax
					Case "znatel"
						strValue = selectedrow.znatel

					Case "zhdnr"
						strValue = selectedrow.zhdnr
					Case "zname"
						strValue = selectedrow.zname

					Case "ourisonline"
						strValue = selectedrow.ourisonline
					Case "jchisonline"
						strValue = selectedrow.jchisonline
					Case "ojisonline"
						strValue = selectedrow.ojisonline


					Case "zfiliale"
						strValue = selectedrow.zfiliale

					Case "maname"
						strValue = selectedrow.maname
					Case "vakals"
						strValue = selectedrow.vakals

					Case Else
						strValue = "?"


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

	Sub FillProperties4Propose()

		Me._ClsPPropertiesSetting.gES = Me.grdES

		ResetESDetailGrid()
		LoadProposalESDetailList()


		' propose interview
		ResetInterviewDetailGrid()
		LoadProposalInterviewDetailList()

		' propose contact
		ResetContactDetailGrid()
		LoadProposalContactDetailList()



	End Sub


#Region "Einsatz Funktionen..."

	Sub ResetESDetailGrid()

		gvES.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvES.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvES.OptionsView.ShowGroupPanel = False
		gvES.OptionsView.ShowIndicator = False
		gvES.OptionsView.ShowAutoFilterRow = False

		gvES.Columns.Clear()

		Try

			Dim columnmdnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmdnr.Caption = m_translate.GetSafeTranslationValue("MDNr")
			columnmdnr.Name = "mdnr"
			columnmdnr.FieldName = "mdnr"
			columnmdnr.Visible = False
			gvES.Columns.Add(columnmdnr)

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "esnr"
			columnmodulname.FieldName = "esnr"
			columnmodulname.Visible = False
			gvES.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("KDNr")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvES.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvES.Columns.Add(columnZHDNr)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_translate.GetSafeTranslationValue("Periode")
			columnMANr.Name = "periode"
			columnMANr.FieldName = "periode"
			columnMANr.Visible = False
			gvES.Columns.Add(columnMANr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Als")
			columnBezeichnung.Name = "esals"
			columnBezeichnung.FieldName = "esals"
			columnBezeichnung.Visible = True
			gvES.Columns.Add(columnBezeichnung)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvES.Columns.Add(columnEmployeename)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvES.Columns.Add(columncustomername)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvES.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml(gvES.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdES.DataSource = Nothing

	End Sub

	Public Function LoadProposalESDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbProposalESDataForProperties(Me._PSetting.SelectedProposeNr)

		If listOfEmployees Is Nothing Then
			m_UitilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedProposalESDetailData With
					 {.mdnr = person.mdnr,
						.esnr = person.esnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.esals = person.esals,
						.employeename = person.employeename,
						.customername = person.customername,
						.periode = person.periode,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedProposalESDetailData) = New BindingList(Of FoundedProposalESDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdES.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Sub OngvES_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvES.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedCustomerESDetailData)

				If viewData.esnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedESNr = viewData.esnr})
					_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub


#End Region


#Region "Interviews Funktionen..."

	Sub ResetInterviewDetailGrid()

		gvInerview.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvInerview.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvInerview.OptionsView.ShowGroupPanel = False
		gvInerview.OptionsView.ShowIndicator = False
		gvInerview.OptionsView.ShowAutoFilterRow = False

		gvInerview.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "id"
			columnmodulname.FieldName = "id"
			columnmodulname.Visible = False
			gvInerview.Columns.Add(columnmodulname)

			Dim columndestlmnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestlmnr.Caption = m_translate.GetSafeTranslationValue("Datum")
			columndestlmnr.Name = "datum"
			columndestlmnr.FieldName = "datum"
			columndestlmnr.Visible = True
			gvInerview.Columns.Add(columndestlmnr)

			Dim columndestzgnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestzgnr.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columndestzgnr.Name = "employeename"
			columndestzgnr.FieldName = "employeename"
			columndestzgnr.Visible = False
			gvInerview.Columns.Add(columndestzgnr)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_translate.GetSafeTranslationValue("Status")
			columndestesnr.Name = "jstate"
			columndestesnr.FieldName = "jstate"
			columndestesnr.Visible = True
			gvInerview.Columns.Add(columndestesnr)

			Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlanr.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnlanr.Name = "createdon"
			columnlanr.FieldName = "createdon"
			columnlanr.Visible = False
			columnlanr.BestFit()
			gvInerview.Columns.Add(columnlanr)

			Dim columnbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbezeichnung.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnbezeichnung.Name = "createdfrom"
			columnbezeichnung.FieldName = "createdfrom"
			columnbezeichnung.Visible = False
			columnbezeichnung.BestFit()
			gvInerview.Columns.Add(columnbezeichnung)

			RestoreGridLayoutFromXml(gvInerview.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdInterview.DataSource = Nothing

	End Sub


	Public Function LoadProposalInterviewDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbProposalInterviewDataForProperties(Me._PSetting.SelectedProposeNr)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedProposalInterviewDetailData With
					 {.recid = person.recid,
						.recnr = person.recnr,
						.employeenumber = person.employeenumber,
						.employeename = person.employeename,
						.datum = person.datum,
						.jstate = person.jstate,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom
}).ToList()

		Dim listDataSource As BindingList(Of FoundedProposalInterviewDetailData) = New BindingList(Of FoundedProposalInterviewDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdInterview.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvInterview_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvInerview.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedProposalInterviewDetailData)

				If viewData.recid > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.InternviewRecNr = viewData.recnr, .SelectedMANr = viewData.employeenumber})
					_ClsKD.OpenSelectedEmployeeInterview()
				End If

			End If

		End If

	End Sub


#End Region


#Region "Contact Funktionen..."

	Sub ResetContactDetailGrid()

		gvContact.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvContact.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvContact.OptionsView.ShowGroupPanel = False
		gvContact.OptionsView.ShowIndicator = False
		gvContact.OptionsView.ShowAutoFilterRow = False

		gvContact.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "contactnr"
			columnmodulname.FieldName = "contactnr"
			columnmodulname.Visible = False
			gvContact.Columns.Add(columnmodulname)

			Dim columndestlmnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestlmnr.Caption = m_translate.GetSafeTranslationValue("Datum")
			columndestlmnr.Name = "datum"
			columndestlmnr.FieldName = "datum"
			columndestlmnr.Visible = True
			gvContact.Columns.Add(columndestlmnr)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_translate.GetSafeTranslationValue("Art")
			columndestesnr.Name = "art"
			columndestesnr.FieldName = "art"
			columndestesnr.Visible = True
			gvContact.Columns.Add(columndestesnr)

			Dim columnlonr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlonr.Caption = m_translate.GetSafeTranslationValue("KST")
			columnlonr.Name = "kst"
			columnlonr.FieldName = "kst"
			columnlonr.Visible = True
			gvContact.Columns.Add(columnlonr)

			Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlanr.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnlanr.Name = "createdon"
			columnlanr.FieldName = "createdon"
			columnlanr.Visible = False
			columnlanr.BestFit()
			gvContact.Columns.Add(columnlanr)

			Dim columnbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbezeichnung.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnbezeichnung.Name = "createdfrom"
			columnbezeichnung.FieldName = "createdfrom"
			columnbezeichnung.Visible = False
			columnbezeichnung.BestFit()
			gvContact.Columns.Add(columnbezeichnung)

			RestoreGridLayoutFromXml(gvContact.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdContact.DataSource = Nothing

	End Sub


	Public Function LoadProposalContactDetailList() As Boolean
		'Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_ModulViewDatabaseAccess.LoadAssignedProposeContactData(ModulConstants.MDData.MDNr, _PSetting.SelectedProposeNr, Nothing, True)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New ModulViewProposeContactData With
					 {.contactnr = person.contactnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.datum = person.datum,
						.employeename = person.employeename,
						.customername = person.customername,
						.zhdname = person.zhdname,
						.beschreibung = person.beschreibung,
						.art = person.art,
						.kst = person.kst,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom
					 }).ToList()

		Dim listDataSource As BindingList(Of ModulViewProposeContactData) = New BindingList(Of ModulViewProposeContactData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdContact.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvContact_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvContact.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, ModulViewProposeContactData)

				If viewData.contactnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.ContactRecordNumber = viewData.contactnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
					_ClsKD.OpenSelectedCustomerContact()
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
				ModulConstants.MDData = ModulConstants.SelectedMDData(cboMD.EditValue)
				ResetMainGrid()
				LoadFoundedProposeList()
			End If
		End If

		Me.grdMain.Enabled = Not (cboMD.EditValue Is Nothing)
		pccMandant.HidePopup()

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

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()
		LoadFoundedProposeList()

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
	Private Sub OngrpES_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpES.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._PSetting.Data4SelectedPropose = True
			strModul2Open = "ES".ToLower

			Dim frm As New frmProposeDetails(Me._PSetting, strModul2Open)
			frm.Show()
		End If

	End Sub

	Private Sub OngrpJobTermin_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpJobTermin.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._PSetting.Data4SelectedPropose = True
			strModul2Open = "Interview".ToLower

			Dim frm As New frmProposeDetails(Me._PSetting, strModul2Open)
			frm.Show()
		End If

	End Sub

	Private Sub OngrpContact_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpContact.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._PSetting.Data4SelectedPropose = True
			strModul2Open = "Contact".ToLower

			Dim frm As New frmProposeDetails(Me._PSetting, strModul2Open)
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

			Case "gves".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingESFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreESSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVESSettingfilename) Then gvES.RestoreLayoutFromXml(m_GVESSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvES.ActiveFilterCriteria = Nothing

			Case "gvInerview".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingJobTerminFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreJobTerminSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVJobTerminSettingfilename) Then gvInerview.RestoreLayoutFromXml(m_GVJobTerminSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvInerview.ActiveFilterCriteria = Nothing


			Case "gvcontact".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingContactFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreContactSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVContactSettingfilename) Then gvContact.RestoreLayoutFromXml(m_GVContactSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvContact.ActiveFilterCriteria = Nothing


			Case Else

				Exit Sub


		End Select


	End Sub


	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub

	Private Sub OngvESColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvES.SaveLayoutToXml(m_GVESSettingfilename)

	End Sub

	Private Sub OngvJobTerminColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvInerview.SaveLayoutToXml(m_GVJobTerminSettingfilename)

	End Sub

	Private Sub OngvContactColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvContact.SaveLayoutToXml(m_GVContactSettingfilename)

	End Sub


#End Region



#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			Dim _ClsNewPrint As New ClsProposeModuls(Me._PSetting)
			_ClsNewPrint.ShowContextMenu4Print(Me.cmdPrint)
			_ClsNewPrint.ShowContextMenu4New(cmdNew)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = "Keine Daten sind vorhanden"

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


End Class
