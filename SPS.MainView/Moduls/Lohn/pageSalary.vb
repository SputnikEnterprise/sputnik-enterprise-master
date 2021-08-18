
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

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.Infrastructure.Settings
Imports SPS.MainView.ReportSettings
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraSplashScreen


Public Class pageSalary

#Region "private consts"

	Private Const MODUL_NAME = "Lohnbuchhaltung"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Lohnbuchhaltung"

	Private Const MODUL_NAME_SETTING = "salary"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/lol/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/lol/keepfilter"

	Private Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;ourisonline;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline"

#End Region


#Region "private fields"

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private m_Stopwatch As Stopwatch

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

	Private _ClsLOSetting As New ClsLOSetting
	Private _ClsLOPropertiesSetting As New ClsLOPropertySetting

	Private m_GVMainSettingfilename As String
	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_GVLOLSettingfilename As String
	Private m_xmlSettingLOLFilter As String
	Private m_xmlSettingRestoreLOLSetting As String
	Private m_AllowedChangeMandant As Boolean

	Private m_MainViewGridData As SuportedCodes

#End Region



#Region "private Properties"

	Private ReadOnly Property SelectedRowViewData As FoundedSalaryData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedSalaryData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property SelectedRowDetailViewData As FoundedSalaryDetailData
		Get
			Dim grdView = TryCast(grdLOL.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedSalaryDetailData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

	Private ReadOnly Property GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
		Get
			Dim result As New GridData
			If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
			m_path = New ClsProgPath

			Try
				Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
															Where Not (exportSetting.Attribute("ID") Is Nothing) _
																	And exportSetting.Attribute("ID").Value = MODUL_NAME _
																	Select New With {
																						.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
																						.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
																						.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
																						.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
																						.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
																						.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
																						.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
																						.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
																						.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
																						.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
																						.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
																						.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
																							}).FirstOrDefault()



				result.SQLQuery = ConfigQuery.SQLQuery
				result.GridColFieldName = ConfigQuery.GridColFieldName
				result.DisplayMember = ConfigQuery.DisplayMember
				result.GridColCaption = ConfigQuery.GridColCaption
				result.GridColWidth = ConfigQuery.GridColWidth
				result.BackColor = ConfigQuery.BackColor
				result.ForeColor = ConfigQuery.ForeColor

				result.PopupFields = ConfigQuery.PopupFields
				result.PopupCaptions = ConfigQuery.PopupCaptions

				result.CountOfFieldsInHeader = CShort(ConfigQuery.CountOfFieldsInHeader)
				result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
				result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow
				result.IsUserProperty = False

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Return result

		End Get

	End Property

	Private ReadOnly Property GetUserGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
		Get
			Dim result As New GridData
			If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
			m_path = New ClsProgPath

			Try
				Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
				Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
															Where Not (exportSetting.Attribute("ID") Is Nothing) _
																	And exportSetting.Attribute("ID").Value = MODUL_NAME _
																	Select New With {
																						.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
																						.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
																						.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
																						.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
																						.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
																						.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
																						.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
																						.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
																						.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
																						.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
																						.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
																						.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
																							}).FirstOrDefault()


				If Not ConfigQuery Is Nothing Then
					result.SQLQuery = ConfigQuery.SQLQuery
					result.GridColFieldName = ConfigQuery.GridColFieldName
					result.DisplayMember = ConfigQuery.DisplayMember
					result.GridColCaption = ConfigQuery.GridColCaption
					result.GridColWidth = ConfigQuery.GridColWidth
					result.BackColor = ConfigQuery.BackColor
					result.ForeColor = ConfigQuery.ForeColor

					result.PopupFields = ConfigQuery.PopupFields
					result.PopupCaptions = ConfigQuery.PopupCaptions

					Dim scountofitems As Short = Short.TryParse(ConfigQuery.CountOfFieldsInHeader, scountofitems)
					result.CountOfFieldsInHeader = Math.Max(scountofitems, 3)
					result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
					result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow

					result.IsUserProperty = True

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			Return result

		End Get

	End Property


#End Region





#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			m_Stopwatch = New Stopwatch()

			m_communicationHub = MessageService.Instance.Hub

			BarManager1.Form = Me
			LoadedMDNr = ModulConstants.MDData.MDNr
			m_SettingsManager = New SettingsSalaryManager

			m_md = New Mandant
			m_utility = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_translate = New TranslateValues

			Try
				m_communicationHub = MessageService.Instance.Hub

				m_GVMainSettingfilename = String.Format("{0}Salary\{1}{2}.xml", ModulConstants.GridSettingPath, gvMain.Name, ModulConstants.UserData.UserNr)
				m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_GVLOLSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvLOL.Name, ModulConstants.UserData.UserNr)
				m_xmlSettingRestoreLOLSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingLOLFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

			m_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
			If m_griddata.SQLQuery Is Nothing Then
				m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
			End If

			aColCaption = m_griddata.GridColCaption.Split(CChar(";"))
			aColFieldName = m_griddata.GridColFieldName.Split(CChar(";"))
			aColWidth = m_griddata.GridColWidth.Split(CChar(";"))

			AddHandler Me.gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
			AddHandler Me.gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged

			AddHandler Me.gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
			AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
			AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged


			AddHandler Me.gvLOL.RowCellClick, AddressOf OngvLOL_RowCellClick
			AddHandler Me.gvLOL.ColumnPositionChanged, AddressOf OngvLOLColumnPositionChanged
			AddHandler Me.gvLOL.ColumnWidthChanged, AddressOf OngvLOLColumnPositionChanged
			AddHandler Me.gvLOL.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

			m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200007)

			'LoadXMLDataForSelectedModule()
			m_MainViewGridData = New SuportedCodes
			m_MainViewGridData.ChangeColumnNamesToLowercase = True
			m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
			m_griddata = m_MainViewGridData.LoadMainGridData

			ResetGrid()

			ResetMandantenDropDown()
			LoadFoundedMDList()

			cboMD.EditValue = ModulConstants.MDData.MDNr

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


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

	Private Sub SetFormLayout()

		Me.Parent.Text = String.Format("{0}", m_translate.GetSafeTranslationValue(UsercontrolCaption))
		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingSalaryKeys.SETTING_LO_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingSalaryKeys.SETTING_LO_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)
		setting_splitterpos = m_SettingsManager.ReadInteger(SettingSalaryKeys.SETTING_LO_SCC_HEADERDETAIL_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderDetail.SplitterPosition = Math.Max(Me.sccHeaderDetail.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingSalaryKeys.SETTING_LO_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperties.Width = Math.Max(Me.dpProperties.Width, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingSalaryKeys.SETTING_LO_SCC_MAINNAV_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainNav_1.SplitterPosition = Math.Max(Me.sccMainNav_1.SplitterPosition, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75


		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill
		Me.sccHeaderDetail.Dock = DockStyle.Fill

		Me.sccMainNav_1.Dock = DockStyle.Fill
		Me.grdLOL.Dock = DockStyle.Fill

		gvRProperty.OptionsView.ShowIndicator = False
		gvLProperty.OptionsView.ShowIndicator = False

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		gvLOL.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvLOL.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvLOL.OptionsView.ShowGroupPanel = False
		gvLOL.OptionsView.ShowIndicator = False
		gvLOL.OptionsView.ShowAutoFilterRow = True

		Me.gvLOL.OptionsView.ShowIndicator = False

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderDetail.SplitterPositionChanged, AddressOf SaveFormProperties

		AddHandler dpProperties.Resize, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SizeChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingSalaryKeys.SETTING_LO_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingSalaryKeys.SETTING_LO_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingSalaryKeys.SETTING_LO_SCC_HEADERDETAIL_SPLITTERPOSION, Me.sccHeaderDetail.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingSalaryKeys.SETTING_LO_DPPROPERTY_WIDTH, Me.dpProperties.Width)

		m_SettingsManager.WriteInteger(SettingSalaryKeys.SETTING_LO_SCC_MAINNAV_1_SPLITTERPOSION, Me.sccMainNav_1.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub


	Sub ResetGrid()

		gvMain.Columns.Clear()
		grdLProperty.DataSource = Nothing
		grdRProperty.DataSource = Nothing
		grdLOL.DataSource = Nothing

		Try
			ResetMailGrid(aColCaption, gvMain)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMain.DataSource = Nothing

		ResetDetailGrid()

	End Sub

	Sub ResetMailGrid(ByVal caption() As String, ByVal gvGrid As DevExpress.XtraGrid.Views.Grid.GridView)

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		'Return

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

		'	RestoreGridLayoutFromXml(gvGrid.Name.ToLower)

		'Catch ex As Exception
		'	m_Logger.LogError(ex.ToString)
		'End Try

	End Sub



	Private Function LoadFoundedSalaryList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.GetDbSalaryData4Show(m_griddata.SQLQuery)
		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfFoundedData
										  Select New FoundedSalaryData With
											  {.lonr = person.lonr,
											  .manr = person.manr,
											  .mdnr = person.mdnr,
											  .zgnr = person.zgnr,
											  .lmid = person.lmid,
											  .vgnr = person.vgnr,
											  .bruttobetrag = person.bruttobetrag,
											  .zgbetrag = person.zgbetrag,
											  .lmbetrag = person.lmbetrag,
											  .lobetrag = person.lobetrag,
											  .monat = person.monat,
											  .IsComplete = person.IsComplete,
											  .jahr = person.jahr,
											  .loperiode = person.loperiode,
											  .createdon = person.createdon,
											  .createdfrom = person.createdfrom,
											  .employeename = person.employeename,
											  .employeestreet = person.employeestreet,
											  .employeeaddress = person.employeeaddress,
											  .maaddress = person.maaddress,
											  .employeetelefon = person.employeetelefon,
											  .employeemobile = person.employeemobile,
											  .employeeemail = person.employeeemail,
											  .magebdat = person.magebdat,
											  .maalterwithdate = person.maalterwithdate,
											  .mabewilligung = person.mabewilligung,
											  .maqualifikation = person.maqualifikation,
											  .tempmabild = person.tempmabild
											  }).ToList()

		Dim listDataSource As BindingList(Of FoundedSalaryData) = New BindingList(Of FoundedSalaryData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource

		RefreshMainViewStateBar()
		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

	Sub ResetDetailGrid()

		gvLOL.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Modulname")
			columnmodulname.Name = "modulname"
			columnmodulname.FieldName = "modulname"
			columnmodulname.Visible = False
			gvLOL.Columns.Add(columnmodulname)

			Dim columndestrpnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestrpnr.Caption = m_translate.GetSafeTranslationValue("RPNr")
			columndestrpnr.Name = "destrpnr"
			columndestrpnr.FieldName = "destrpnr"
			columndestrpnr.Visible = False
			gvLOL.Columns.Add(columndestrpnr)

			Dim columndestlmnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestlmnr.Caption = m_translate.GetSafeTranslationValue("LMNr")
			columndestlmnr.Name = "destlmnr"
			columndestlmnr.FieldName = "destlmnr"
			columndestlmnr.Visible = False
			gvLOL.Columns.Add(columndestlmnr)

			Dim columndestzgnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestzgnr.Caption = m_translate.GetSafeTranslationValue("ZGNr")
			columndestzgnr.Name = "destzgnr"
			columndestzgnr.FieldName = "destzgnr"
			columndestzgnr.Visible = False
			gvLOL.Columns.Add(columndestzgnr)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_translate.GetSafeTranslationValue("ESNr")
			columndestesnr.Name = "dstesnr"
			columndestesnr.FieldName = "dstesnr"
			columndestesnr.Visible = False
			gvLOL.Columns.Add(columndestesnr)

			Dim columnlonr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlonr.Caption = m_translate.GetSafeTranslationValue("LONr")
			columnlonr.Name = "lonr"
			columnlonr.FieldName = "lonr"
			columnlonr.Visible = False
			gvLOL.Columns.Add(columnlonr)

			Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlanr.Caption = m_translate.GetSafeTranslationValue("LANr")
			columnlanr.Name = "lanr"
			columnlanr.FieldName = "lanr"
			columnlanr.Visible = True
			columnlanr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnlanr.DisplayFormat.FormatString = "f3"
			columnlanr.BestFit()
			gvLOL.Columns.Add(columnlanr)

			Dim columnbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbezeichnung.Caption = m_translate.GetSafeTranslationValue("Bezeichnung")
			columnbezeichnung.Name = "bezeichnung"
			columnbezeichnung.FieldName = "bezeichnung"
			columnbezeichnung.Visible = True
			columnbezeichnung.BestFit()
			gvLOL.Columns.Add(columnbezeichnung)

			Dim columnbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbetrag.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnbetrag.Name = "betrag"
			columnbetrag.FieldName = "betrag"
			columnbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnbetrag.DisplayFormat.FormatString = "N2"
			columnbetrag.Visible = True
			columnbetrag.BestFit()
			gvLOL.Columns.Add(columnbetrag)

			RestoreGridLayoutFromXml(gvLOL.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdLOL.DataSource = Nothing

	End Sub

	Private Function LoadFoundedSalaryDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbSalaryDetailData4ShowInNavigation(Me._ClsLOSetting.SelectedLONr)

		If listOfEmployees Is Nothing Then
			Exit Function
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New FoundedSalaryDetailData With
					 {.modulname = person.modulname,
						.MDNr = person.MDNr,
						.destrpnr = person.destrpnr,
						.destlmnr = person.destlmnr,
						.destzgnr = person.destzgnr,
						.destesnr = person.destesnr,
						.lonr = person.lonr,
						.lanr = person.lanr,
						.bezeichnung = person.bezeichnung,
						.betrag = person.betrag
}).ToList()

		Dim listDataSource As BindingList(Of FoundedSalaryDetailData) = New BindingList(Of FoundedSalaryDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdLOL.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()
		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedSalaryList()

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpLOL.Text = m_translate.GetSafeTranslationValue(Me.grpLOL.Text)

		Me.dpProperties.Text = m_translate.GetSafeTranslationValue(Me.dpProperties.Text)

	End Sub


	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		Dim column = e.Column
		Dim dataRow = gvMain.GetRow(e.RowHandle)
		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, FoundedSalaryData)

			_ClsLOSetting.SelectedMDNr = viewData.mdnr
			_ClsLOSetting.SelectedLONr = viewData.lonr
			_ClsLOSetting.SelectedMANr = viewData.manr

			BuildNewPrintContextMenu()


			If (e.Clicks = 2) Then

				Select Case column.Name.ToLower
					Case "employeename", "employeeaddress", "employeestreet"
						If viewData.manr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "employeetelefon"
						If viewData.employeetelefon > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							_ClsKD.TelefonCallToEmployee(viewData.employeetelefon)
						End If

					Case "employeemobile"
						If viewData.employeemobile > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							_ClsKD.TelefonCallToEmployee(viewData.employeemobile)
						End If

					Case Else
						If viewData.lonr > 0 Then
							Dim _Clsre As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr,
														   .SelectedLONr = viewData.lonr,
														   .SelectedMANr = viewData.manr,
														   .SelectedMonth = viewData.monat,
														   .SelectedYear = viewData.jahr})

							If viewData.monat = 0 AndAlso Not viewData.IsComplete Then

								_Clsre.DeleteAssignedInvalidPayroll(viewData.lonr)
								Return

							End If

							_Clsre.OpenSelectedLO()

						End If


				End Select

			End If

		End If

	End Sub


	Private Function GetCellValue(ByVal strCellName As String, ByVal selectedrow As FoundedSalaryData) As MenuData

		Select Case strCellName.ToLower

			Case "lonr"
				Return New MenuData With {.mnuvalue = selectedrow.lonr}

			Case "manr"
				Return New MenuData With {.mnuvalue = selectedrow.manr}

			Case "zgnr"
				Return New MenuData With {.mnuvalue = selectedrow.zgnr}

			Case "lmid"
				Return New MenuData With {.mnuvalue = selectedrow.lmid}

			Case "vgnr"
				Return New MenuData With {.mnuvalue = selectedrow.vgnr}

			Case "monat"
				Return New MenuData With {.mnuvalue = selectedrow.monat}

			Case "jahr"
				Return New MenuData With {.mnuvalue = selectedrow.jahr}

			Case "loperiode"
				Return New MenuData With {.mnuvalue = selectedrow.loperiode}

			Case "erstelltdurch"
				Return New MenuData With {.mnuvalue = selectedrow.createdfrom}

			Case "erstelltam"
				Return New MenuData With {.mnuvalue = selectedrow.createdon}

			Case "bruttobetrag"
				Return New MenuData With {.mnuvalue = selectedrow.bruttobetrag}

			Case "zgbetrag"
				Return New MenuData With {.mnuvalue = selectedrow.zgbetrag}

			Case "lmbetrag"
				Return New MenuData With {.mnuvalue = selectedrow.lmbetrag}

			Case "lobetrag"
				Return New MenuData With {.mnuvalue = selectedrow.lobetrag}

			Case "maname"
				Return New MenuData With {.mnuvalue = selectedrow.employeename}

			Case "mastrasse"
				Return New MenuData With {.mnuvalue = selectedrow.employeestreet}

			Case "maplzort"
				Return New MenuData With {.mnuvalue = selectedrow.employeeaddress}

			Case "maaddress"
				Return New MenuData With {.mnuvalue = selectedrow.maaddress}

			Case "matelefon"
				Return New MenuData With {.mnuvalue = selectedrow.employeetelefon}

			Case "manatel"
				Return New MenuData With {.mnuvalue = selectedrow.employeemobile}

			Case "maemail"
				Return New MenuData With {.mnuvalue = selectedrow.employeeemail}

			Case "maqualifikation"
				Return New MenuData With {.mnuvalue = selectedrow.maqualifikation}

			Case "magebdat"
				Return New MenuData With {.mnuvalue = selectedrow.magebdat}

			Case "maalterwithdate"
				Return New MenuData With {.mnuvalue = selectedrow.maalterwithdate}

			Case "mabewilligung"
				Return New MenuData With {.mnuvalue = selectedrow.mabewilligung}


			Case Else
				Return Nothing

		End Select


	End Function

	Private Sub FillPopupFields(ByVal selectedrow As FoundedSalaryData)	'As Dictionary(Of String, String)
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
					Case "lonr"
						strValue = selectedrow.lonr

					Case "manr"
						strValue = selectedrow.manr

					Case "zgnr"
						strValue = selectedrow.zgnr

					Case "lmid"
						strValue = selectedrow.lmid

					Case "vgnr"
						strValue = selectedrow.vgnr

					Case "monat"
						strValue = selectedrow.monat

					Case "jahr"
						strValue = selectedrow.jahr

					Case "loperiode"
						strValue = selectedrow.loperiode

					Case "createdfrom"
						strValue = selectedrow.createdfrom

					Case "createdon"
						strValue = selectedrow.createdon

					Case "bruttobetrag"
						strValue = selectedrow.bruttobetrag

					Case "zgbetrag"
						strValue = selectedrow.zgbetrag

					Case "lmbetrag"
						strValue = selectedrow.lmbetrag

					Case "lobetrag"
						strValue = selectedrow.lobetrag

					Case "employeename"
						strValue = selectedrow.employeename

					Case "employeestreet"
						strValue = selectedrow.employeestreet

					Case "employeeaddress"
						strValue = selectedrow.employeeaddress

					Case "maaddress"
						strValue = selectedrow.maaddress

					Case "employeemobile"
						strValue = selectedrow.employeemobile
					Case "employeetelefon"
						strValue = selectedrow.employeetelefon

					Case "employeeemail"
						strValue = selectedrow.employeeemail

					Case "magebdat"
						strValue = selectedrow.magebdat

					Case "maalterwithdate"
						strValue = selectedrow.maalterwithdate

					Case "mabewilligung"
						strValue = selectedrow.mabewilligung

					Case "maqualifikation"
						strValue = selectedrow.maqualifikation

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

			Try
				If selectedrow.tempmabild Then showMATempBild(selectedrow.manr)
				Me.PictureBox1.Visible = selectedrow.tempmabild

			Catch ex As Exception

			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			ShowErrDetail(ex.Message)
			result = Nothing

		End Try

	End Sub


	Private Sub OngvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvMain.FocusedRowChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				Me._ClsLOSetting.SelectedMDNr = selectedrow.mdnr

				Me._ClsLOSetting.SelectedLONr = selectedrow.lonr
				Me._ClsLOSetting.SelectedMANr = selectedrow.manr

				If selectedrow.lonr = 0 Then Exit Sub

				FillPopupFields(selectedrow)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4Candidate()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try


	End Sub


#Region "Kandidatenfoto zeigen..."

	Private Sub showMATempBild(ByVal iMANr As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = "Select MABild From Mitarbeiter Where MANr = @MANr And MABild Is Not Null"
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim BA As Byte() = Nothing
		Dim imgResult As Image = Nothing

		Conn.Open()
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sSql, Conn)
		cmd.CommandType = CommandType.Text
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@MANr", iMANr)

		Try
			Try
				BA = CType(cmd.ExecuteScalar, Byte())
			Catch ex As Exception

			End Try
			If BA Is Nothing Then Throw New Exception(String.Format("Keine Bilddaten wurden gelesen. employeeNumber: {0}", iMANr))

			Dim ArraySize As New Integer
			ArraySize = BA.GetUpperBound(0)

			Dim ms As New MemoryStream(BA)
			Me.PictureBox1.Image = Image.FromStream(ms)
			AutosizeImage(ms, Me.PictureBox1, PictureBoxSizeMode.Normal)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Me.PictureBox1.Image = Nothing

		End Try

	End Sub

	Public Sub AutosizeImage(ByVal imgstream As Stream, ByVal picBox As PictureBox, _
													 Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			If picBox.Width = 0 Or picBox.Height = 0 Then Exit Sub
			picBox.Image = Nothing
			picBox.SizeMode = pSizeMode

			If Not imgstream Is Nothing Then
				Dim imgOrg As Bitmap
				Dim imgShow As Bitmap
				Dim g As Graphics
				Dim divideBy, divideByH, divideByW As Double
				imgOrg = DirectCast(Bitmap.FromStream(imgstream), Bitmap)

				divideByW = imgOrg.Width / picBox.Width
				divideByH = imgOrg.Height / picBox.Height
				If divideByW > 1 Or divideByH > 1 Then
					If divideByW > divideByH Then
						divideBy = divideByW
					Else
						divideBy = divideByH
					End If

					imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
					imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
					g = Graphics.FromImage(imgShow)
					g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
					g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), _
																						CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, _
																					imgOrg.Height, GraphicsUnit.Pixel)
					g.Dispose()
				Else
					imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
					imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
					g = Graphics.FromImage(imgShow)
					g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
					g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, _
											imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
					g.Dispose()
				End If
				imgOrg.Dispose()

				picBox.Image = imgShow
			Else
				picBox.Image = Nothing
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			MsgBox(ex.ToString)
		End Try

	End Sub

#End Region



#Region "Linke Navigationsleiste"

	Sub FillProperties4Candidate()

		LoadFoundedSalaryDetailList()

	End Sub


#Region "Rappote und Monatliche Lohnangaben Funktionen..."

	Sub OngvLOL_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvLOL.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedSalaryDetailData)

				Select Case viewData.modulname.ToLower
					Case "r"
						If viewData.destrpnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.MDNr,
																																	.SelectedRPNr = viewData.destrpnr})
							_ClsKD.OpenSelectedReport(viewData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "l"
						If viewData.destlmnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.MDNr,
																																	.SelectedLMNr = viewData.destlmnr})
							_ClsKD.OpenSelectedEmployeeMSalary()
						End If

					Case "z"
						If viewData.destzgnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.MDNr,
																																	.SelectedZGNr = viewData.destzgnr})
							_ClsKD.OpenSelectedAdvancePayment(viewData.MDNr, ModulConstants.UserData.UserNr)
						End If

				End Select

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
				LoadFoundedSalaryList()
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


		'LoadXMLDataForSelectedModule()
		ResetGrid()
		LoadFoundedSalaryList()

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
	Private Sub grpLOL_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpLOL.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._ClsLOSetting.Data4SelectedMA = True
			strModul2Open = "loES".ToLower

			Dim frm As New frmLODetails(Me._ClsLOSetting, strModul2Open)
			frm.Show()
		End If

	End Sub


#End Region


#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			Dim _ClsNewPrint As New ClsLOModuls(Me._ClsLOSetting)
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

			Case "gvLOL".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLOLFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLOLSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVLOLSettingfilename) Then gvLOL.RestoreLayoutFromXml(m_GVLOLSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvLOL.ActiveFilterCriteria = Nothing

			Case Else

				Exit Sub


		End Select


	End Sub

	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub

	Private Sub OngvLOLColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvLOL.SaveLayoutToXml(m_GVLOLSettingfilename)

	End Sub

#End Region

End Class

