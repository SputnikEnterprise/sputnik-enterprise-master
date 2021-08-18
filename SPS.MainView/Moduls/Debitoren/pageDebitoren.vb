
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

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.Infrastructure.Settings
Imports SPS.MainView.DebitorenSettings
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports DevExpress.XtraSplashScreen

Public Class pageDebitoren


#Region "private fields"

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private Property LoadedMDNr As Integer

	Protected m_SettingsManager As ISettingsManager

	Private LiGridProperty As New Dictionary(Of String, String)
	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()
	Private m_UitilityUI As UtilityUI

	Private m_griddata As GridData

	Private m_Setting As ClsRESetting
	Private m_PropertiesSetting As New ClsREPropertySetting

	Private m_GVMainSettingfilename As String
	Private m_GVReportSettingfilename As String
	Private m_GVZESettingfilename As String

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingRPFilter As String
	Private m_xmlSettingRestoreRPSetting As String

	Private m_xmlSettingZEFilter As String
	Private m_xmlSettingRestoreZESetting As String

	Private m_InvoiceData As FoundedCustomerBillData

	Private m_ClsNewPrint As ClsREModuls
	Private m_AllowedChangeMandant As Boolean
	Private m_MainViewGridData As SuportedCodes


#End Region


#Region "private consts"

	Private Const MODUL_NAME = "Debitorenverwaltung"
	Private Const MODUL_NAME_SETTING = "Invoice"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Debitorenbuchhaltung"

	Private Const GV_RP_DISPLAYMEMBER = "Rapport-Nr."
	Private Const GV_ZE_DISPLAYMEMBER = "Zahlung-Nr."

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/rp/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/rp/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/ze/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/ze/keepfilter"

	'Private Const DATE_COLUMN_NAME As String = "magebdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate;faelligdate;checkedon;avamreportingdate;avamfrom;avamuntil"
	'Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	'Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	'Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;ourisonline;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline"

#End Region





#Region "Private property"

	Private Property gvESDisplayMember As String
	Private Property gvRPDisplayMember As String
	Private Property gvProposeDisplayMember As String
	Private Property gvREDisplayMember As String
	Private Property gvZEDisplayMember As String
	Private Property gvContactDisplayMember As String


	Private ReadOnly Property SelectedRowViewData As FoundedCustomerBillData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedCustomerBillData)
					Return contact
				End If

			End If

			Return Nothing
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
			BarManager1.Form = Me
			LoadedMDNr = ModulConstants.MDData.MDNr
			Me.m_Setting = New ClsRESetting
			m_SettingsManager = New SettingsREManager

			m_md = New Mandant
			m_utility = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_translate = New TranslateValues
			m_UitilityUI = New UtilityUI

			Try
				m_communicationHub = MessageService.Instance.Hub

				m_GVMainSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvMain.Name, ModulConstants.UserData.UserNr)
				m_GVReportSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvRP.Name, ModulConstants.UserData.UserNr)

				m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreRPSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingRPFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreZESetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingZEFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try


			dpProperties.Options.ShowCloseButton = False

			AddHandler gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
			AddHandler gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
			AddHandler gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
			AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
			AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged

			AddHandler gvRP.RowCellClick, AddressOf OngvRP_RowCellClick
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvReportColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvReportColumnPositionChanged
			AddHandler Me.gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

			AddHandler gvZE.RowCellClick, AddressOf OngvZE_RowCellClick
			AddHandler Me.gvZE.ColumnPositionChanged, AddressOf OngvZEColumnPositionChanged
			AddHandler Me.gvZE.ColumnWidthChanged, AddressOf OngvZEColumnPositionChanged
			AddHandler Me.gvZE.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

			m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200009)
			grpFunction.CustomHeaderButtons(0).Properties.Enabled = m_AllowedChangeMandant

			m_MainViewGridData = New SuportedCodes
			m_MainViewGridData.ChangeColumnNamesToLowercase = True
			m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
			m_griddata = m_MainViewGridData.LoadMainGridData

			ResetMainGrid()

			ResetMandantenDropDown()
			LoadFoundedMDList()

			cboMD.EditValue = ModulConstants.MDData.MDNr

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

#End Region


#Region "Private methods"

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

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingREKeys.SETTING_RE_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingREKeys.SETTING_RE_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingREKeys.SETTING_RE_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperties.Width = Math.Max(Me.dpProperties.Width, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75


		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMainNav.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill

		Me.gvMain.OptionsView.ShowIndicator = False
		Me.gvRP.OptionsView.ShowIndicator = False
		Me.gvZE.OptionsView.ShowIndicator = False

		gvlProperty.OptionsView.ShowColumnHeaders = False
		gvlProperty.OptionsView.ShowIndicator = False
		gvrProperty.OptionsView.ShowColumnHeaders = False
		gvrProperty.OptionsView.ShowIndicator = False

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler dpProperties.Resize, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SizeChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingREKeys.SETTING_RE_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingREKeys.SETTING_RE_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingREKeys.SETTING_RE_DPPROPERTY_WIDTH, Me.dpProperties.Width)

		m_SettingsManager.SaveSettings()

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

	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

	End Sub

	'	gvlProperty.Columns.Clear()
	'	gvrProperty.Columns.Clear()
	'	gvMain.Columns.Clear()

	'	grdLProperty.DataSource = Nothing
	'	grdRProperty.DataSource = Nothing
	'	grdRP.DataSource = Nothing
	'	grdZE.DataSource = Nothing

	'	Try
	'		Dim repoHTML = New RepositoryItemHypertextLabel
	'		repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
	'		repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
	'		repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
	'		repoHTML.Appearance.Options.UseTextOptions = True
	'		grdMain.RepositoryItems.Add(repoHTML)

	'		Dim reproCheckbox = New RepositoryItemCheckEdit
	'		reproCheckbox.Appearance.Options.UseTextOptions = True
	'		grdMain.RepositoryItems.Add(reproCheckbox)

	'		Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
	'		Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

	'		For i = 0 To aColCaption.Length - 1

	'			Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
	'			Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
	'			Dim columnName As String = aColFieldName(i).ToLower.Trim

	'			column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'			column.Caption = columnCaption
	'			column.Name = columnName
	'			column.FieldName = columnName

	'			If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
	'				column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
	'				If columnName.ToLower = "faelligdate" OrElse columnName.ToLower = "fakdate" Then
	'					column.DisplayFormat.FormatString = "dd.MM.yyyy"
	'				Else
	'					column.DisplayFormat.FormatString = "G"
	'				End If
	'				column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

	'			ElseIf DECIMAL_COLUMN_NAME.ToLower.Contains(columnName) Then
	'				column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'				column.AppearanceHeader.Options.UseTextOptions = True
	'				column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'				column.DisplayFormat.FormatString = "N2"
	'				column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

	'			ElseIf INTEGER_COLUMN_NAME.ToLower.Contains(columnName) Then
	'				column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'				column.AppearanceHeader.Options.UseTextOptions = True
	'				column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'				column.DisplayFormat.FormatString = "F0"
	'				column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

	'			ElseIf CHECKBOX_COLUMN_NAME.Contains(columnName) Then
	'				column.ColumnEdit = reproCheckbox
	'			Else
	'				column.ColumnEdit = repoHTML
	'			End If


	'			column.Visible = If(aColWidth(i) > 0, True, False)
	'			gvMain.Columns.Add(column)

	'		Next

	'		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'	End Try

	'	grdMain.DataSource = Nothing

	'End Sub


	Private Function LoadFoundedInvoicesList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfCustomerBill = m_DataAccess.GetDbCustomerBillsData4Show(m_griddata.SQLQuery)
		If listOfCustomerBill Is Nothing Then Return False

		Dim responsiblePersonsGridData = (From person In listOfCustomerBill
										  Select New FoundedCustomerBillData With
					 {.mdnr = person.mdnr,
					 .customermdnr = person.customermdnr,
						.renr = person.renr,
						.kdnr = person.kdnr,
						._res = person._res,
						.firma1 = person.firma1,
						.firma2 = person.firma2,
						.firma3 = person.firma3,
						.abteilung = person.abteilung,
						.postfach = person.postfach,
						.zhd = person.zhd,
						.plz = person.plz,
						.ort = person.ort,
						.plzort = person.plzort,
						.einstufung = person.einstufung,
						.branche = person.branche,
						.betragex = person.betragex,
						.betragink = person.betragink,
						.betragmwst = person.betragmwst,
						.bezahlt = person.bezahlt,
						.mwstproz = person.mwstproz,
						.betragopen = person.betragopen,
						.fbmonth = person.fbmonth,
						.fakdate = person.fakdate,
						.faelligdate = person.faelligdate,
						.employeeadvisor = person.employeeadvisor,
						.customeradvisor = person.customeradvisor,
						.kdemail = person.kdemail,
						.kdtelefax = person.kdtelefax,
						.kdtelefon = person.kdtelefon,
						.rekst1 = person.rekst1,
						.rekst2 = person.rekst2,
						.rekst = person.rekst,
						.reart1 = person.reart1,
						.reart2 = person.reart2,
						.printdate = person.printdate,
						.strasse = person.strasse,
						.zfiliale = person.zfiliale,
						.zahlkond = person.zahlkond,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedCustomerBillData) = New BindingList(Of FoundedCustomerBillData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource
		RefreshMainViewStateBar()

		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfCustomerBill Is Nothing
	End Function

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

	Private Sub frm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		SetFormLayout()

		BuildNewPrintContextMenu()

		LoadFoundedInvoicesList()

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpRP.Text = m_translate.GetSafeTranslationValue(Me.grpRP.Text)
		Me.grpZE.Text = m_translate.GetSafeTranslationValue(Me.grpZE.Text)

		Me.dpProperties.Text = m_translate.GetSafeTranslationValue(Me.dpProperties.Text)

	End Sub

	Private Function GetCellValue(ByVal strCellName As String, ByVal selectedrow As FoundedCustomerBillData) As MenuData

		Select Case strCellName.ToLower
			Case "renr"
				Return New MenuData With {.mnuvalue = selectedrow.renr}
			Case "firma1"
				Return New MenuData With {.mnuvalue = selectedrow.firma1}

			Case "customeradvisor"
				Return New MenuData With {.mnuvalue = selectedrow.customeradvisor}

			Case "employeeadvisor"
				Return New MenuData With {.mnuvalue = selectedrow.employeeadvisor}

			Case "kdtelefax"
				Return New MenuData With {.mnuvalue = selectedrow.kdtelefax}

			Case "kdtelefon"
				Return New MenuData With {.mnuvalue = selectedrow.kdtelefon}

			Case "kdemail"
				Return New MenuData With {.mnuvalue = selectedrow.kdemail}

			Case "kdnr"
				Return New MenuData With {.mnuvalue = selectedrow.kdnr}

			Case "plzort"
				Return New MenuData With {.mnuvalue = selectedrow.plzort}


			Case "strasse"
				Return New MenuData With {.mnuvalue = selectedrow.strasse}

			Case "betragink"
				Return New MenuData With {.mnuvalue = selectedrow.betragink}
			Case "betragex"
				Return New MenuData With {.mnuvalue = selectedrow.betragex}
			Case "betragmwst"
				Return New MenuData With {.mnuvalue = selectedrow.betragmwst}
			Case "bezahlt"
				Return New MenuData With {.mnuvalue = selectedrow.bezahlt}
			Case "mwstproz"
				Return New MenuData With {.mnuvalue = selectedrow.mwstproz}
			Case "betragopen"
				Return New MenuData With {.mnuvalue = selectedrow.betragopen}

			Case "fakdate"
				Return New MenuData With {.mnuvalue = selectedrow.fakdate}
			Case "printdate"
				Return New MenuData With {.mnuvalue = selectedrow.printdate}
			Case "faelligdate"
				Return New MenuData With {.mnuvalue = selectedrow.faelligdate}

			Case "createdon"
				Return New MenuData With {.mnuvalue = selectedrow.createdon}
			Case "createdfrom"
				Return New MenuData With {.mnuvalue = selectedrow.createdfrom}



			Case Else
				Return Nothing

		End Select

	End Function


	Private Sub FillPopupFields(ByVal selectedrow As FoundedCustomerBillData)
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
					Case "firma1"
						strValue = selectedrow.firma1
					Case "firma2"
						strValue = selectedrow.firma2
					Case "firma3"
						strValue = selectedrow.firma3
					Case "abteilung"
						strValue = selectedrow.abteilung

					Case "kdnr"
						strValue = selectedrow.kdnr

					Case "strasse"
						strValue = selectedrow.strasse
					Case "plzort"
						strValue = selectedrow.plzort

					Case "kdtelefon"
						strValue = selectedrow.kdtelefon
					Case "kdtelefax"
						strValue = selectedrow.kdtelefax
					Case "kdemail"
						strValue = selectedrow.kdemail


					Case "betragink"
						strValue = Format(If(selectedrow.betragink Is Nothing, 0, selectedrow.betragink), "N2")
					Case "betragex"
						strValue = Format(If(selectedrow.betragex Is Nothing, 0, selectedrow.betragex), "N2")
					Case "betragmwst"
						strValue = Format(If(selectedrow.betragmwst Is Nothing, 0, selectedrow.betragmwst), "N2")
					Case "bezahlt"
						strValue = Format(If(selectedrow.bezahlt Is Nothing, 0, selectedrow.bezahlt), "N2")
					Case "mwstproz"
						strValue = Format(If(selectedrow.mwstproz Is Nothing, 0, selectedrow.mwstproz), "N2")
					Case "betragopen"
						strValue = Format(If(selectedrow.betragopen Is Nothing, 0, selectedrow.betragopen), "N2")

					Case "faelligdate"
						strValue = selectedrow.faelligdate
					Case "fakdate"
						strValue = selectedrow.fakdate
					Case "printdate"
						strValue = If(selectedrow.betragopen Is Nothing, "", selectedrow.printdate)

					Case "employeeadvisor"
						strValue = selectedrow.employeeadvisor
					Case "customeradvisor"
						strValue = selectedrow.customeradvisor


					Case "createdon"
						strValue = selectedrow.createdon
					Case "createdfrom"
						strValue = selectedrow.createdfrom

				End Select

				result.Add(aPoupFields(j), strValue)
			Next

			Dim itemName As String
			Dim itemValue As String

			For i As Integer = 0 To result.Count - 1
				If Not String.IsNullOrEmpty(aPoupCaption(i)) And Not String.IsNullOrEmpty(result.Item(aPoupFields(i))) Then

					If i > iCountofFieldInHeaderInfo Then
						itemName = aPoupCaption(i)
						itemValue = If(IsDate(result.Item(aPoupFields(i))),
																									Format(CDate(result.Item(aPoupFields(i))), "D"),
																									result.Item(aPoupFields(i)))
						Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
						listOfPropertyRData.Add(propertyItem)

					Else
						itemName = aPoupCaption(i)
						itemValue = If(IsDate(result.Item(aPoupFields(i))),
																									Format(CDate(result.Item(aPoupFields(i))), "D"),
																									result.Item(aPoupFields(i)))
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
			m_InvoiceData = SelectedRowViewData

			m_ClsNewPrint = New ClsREModuls(Me.m_Setting, m_InvoiceData)
			BuildNewPrintContextMenu()

			If Not m_InvoiceData Is Nothing Then
				Me.m_Setting.SelectedMDNr = m_InvoiceData.mdnr
				Me.m_Setting.SelectedRENr = m_InvoiceData.renr
				Me.m_Setting.SelectedKDNr = m_InvoiceData.kdnr
				If m_InvoiceData.renr = 0 Then Exit Sub
				'Me._KDSetting.SelectedKDzNr = selectedrow.kdzhdnr

				FillPopupFields(m_InvoiceData)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4Customer()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

	End Sub

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedCustomerBillData)

				Select Case column.Name.ToLower
					Case "kdnr", "firma1", "firma2"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customermdnr, .SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(viewData.customermdnr, ModulConstants.UserData.UserNr)
						End If

					Case Else
						If viewData.renr > 0 Then
							Dim _Clsre As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRENr = viewData.renr})
							_Clsre.OpenSelectedInvoice(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If


				End Select

			End If

		End If

	End Sub




#Region "linke Navigationsleist"


	Sub FillProperties4Customer()
		m_PropertiesSetting.grdRP = Me.grdRP
		m_PropertiesSetting.grdZE = Me.grdZE
		m_PropertiesSetting.SelectedRENr = Me.m_Setting.SelectedRENr
		m_PropertiesSetting.SelectedKDNr = Me.m_Setting.SelectedKDNr


		ResetRPDetailGrid()
		LoadInvoiceRPDetailList()


		'Dim _ClsPRERP As New REProperties.ClsRERP(m_PropertiesSetting)
		'Me.gvZEDisplayMember = "Rapport-Nr."
		'_ClsPRERP.LoadRERPData()

		'Dim _ClsPKDZE As New REProperties.ClsREZE(m_PropertiesSetting)
		'Me.gvZEDisplayMember = "Zahlung-Nr."
		'_ClsPKDZE.FillKDOpenZE()

		ResetZEGrid()
		LoadInvoiceRecipientOfPaymentsDetailList()


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

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvRP.Columns.Add(columnEmployeename)

			Dim columnCustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomername.Caption = m_translate.GetSafeTranslationValue("Firma")
			columnCustomername.Name = "customername"
			columnCustomername.FieldName = "customername"
			columnCustomername.Visible = False
			gvRP.Columns.Add(columnCustomername)

			Dim columnCustomeramount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomeramount.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnCustomeramount.Name = "customeramount"
			columnCustomeramount.FieldName = "customeramount"
			columnCustomeramount.Visible = True
			columnCustomeramount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnCustomeramount.DisplayFormat.FormatString = "N2"
			gvRP.Columns.Add(columnCustomeramount)

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


	Public Function LoadInvoiceRPDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbInvoiceReportDataForProperties(Me.m_Setting.SelectedRENr)

		If listOfEmployees Is Nothing Then
			m_UitilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedInvoiceReportDetailData With
					 {.mdnr = person.mdnr,
						.employeeMDNr = person.employeeMDNr,
						.customerMDNr = person.customerMDNr,
						.rpnr = person.rpnr,
						.kdnr = person.kdnr,
						.manr = person.manr,
						.employeename = person.employeename,
						.customername = person.customername,
						.periode = person.periode,
						.customeramount = person.customeramount,
						.createdfrom = person.createdfrom,
						.createdon = person.createdon,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedInvoiceReportDetailData) = New BindingList(Of FoundedInvoiceReportDetailData)

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
				Dim viewData = CType(dataRow, FoundedInvoiceReportDetailData)

				If viewData.rpnr.HasValue Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub

#End Region



#Region "ZE Funktionen..."

	Sub ResetZEGrid()

		gvZE.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvZE.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvZE.OptionsView.ShowGroupPanel = False
		gvZE.OptionsView.ShowIndicator = False
		gvZE.OptionsView.ShowAutoFilterRow = False

		gvZE.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zenr"
			columnmodulname.FieldName = "zenr"
			columnmodulname.Visible = False
			gvZE.Columns.Add(columnmodulname)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "valutadate"
			columnFakDat.FieldName = "valutadate"
			columnFakDat.Visible = True
			gvZE.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "zebetrag"
			columnBetragInk.FieldName = "zebetrag"
			columnBetragInk.Visible = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvZE.Columns.Add(columnBetragInk)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvZE.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvZE.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvZE.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml(gvZE.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdZE.DataSource = Nothing

	End Sub

	Public Function LoadInvoiceRecipientOfPaymentsDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbInvoiceRecipientOfPaymentsDataForProperties(Me.m_Setting.SelectedRENr)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedInvoiceROPDetailData With
					 {.mdnr = person.mdnr,
						.customerMDNr = person.customerMDNr,
						.zenr = person.zenr,
						.renr = person.renr,
						.kdnr = person.kdnr,
						.valutadate = person.valutadate,
						.zebetrag = person.zebetrag,
						.rekst = person.rekst,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedInvoiceROPDetailData) = New BindingList(Of FoundedInvoiceROPDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdZE.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Sub OngvZE_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvZE.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedInvoiceROPDetailData)

				If viewData.zenr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZENr = viewData.zenr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedPayment()
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
				LoadFoundedInvoicesList()
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

	End Sub

	Private Sub RefreshData()
		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()
		LoadFoundedInvoicesList()

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
	Private Sub OngrpRP_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpRP.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me.m_Setting.Data4SelectedKD = True
			strModul2Open = "RP".ToLower

			Dim frm As New frmREDetails(Me.m_Setting, strModul2Open)
			frm.Show()

		End If

	End Sub

	Private Sub OngrpZE_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpZE.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me.m_Setting.Data4SelectedKD = True
			strModul2Open = "ZE".ToLower

			Dim frm As New frmREDetails(Me.m_Setting, strModul2Open)
			frm.Show()

		End If

	End Sub


#End Region


#Region "GridSetting"

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

			Case "gvZE".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingZEFilter, False), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreZESetting, False), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVZESettingfilename) Then gvZE.RestoreLayoutFromXml(m_GVZESettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvZE.ActiveFilterCriteria = Nothing


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

	Private Sub OngvZEColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvZE.SaveLayoutToXml(m_GVZESettingfilename)

	End Sub


#End Region


#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			'If m_InvoiceData Is Nothing Then Return

			'If m_ClsNewPrint Is Nothing Then Return
			cmdPrint.HideDropDown()

			If m_ClsNewPrint Is Nothing Then
				m_ClsNewPrint = New ClsREModuls(Me.m_Setting, m_InvoiceData)
			End If
			If Not m_InvoiceData Is Nothing Then m_ClsNewPrint.ShowContextMenu4Print(BarManager1, Me.cmdPrint)
			m_ClsNewPrint.ShowContextMenu4New(BarManager1, cmdNew)


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



