
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

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

Imports System.Runtime.Serialization
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.Infrastructure.Settings
Imports SPS.MainView.DebitorenSettings
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports DevExpress.XtraSplashScreen

Public Class pageFOP


#Region "private consts"

	Private Const MODUL_NAME = "Fremdrechnung"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"

#End Region


#Region "private fields"

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private Property LoadedMDNr As Integer

	Protected m_SettingsManager As ISettingsManager

	Private aColFieldName As String()
	Private aColCaption As String()
	Private aColWidth As String()

	Private aPropertyColFieldName As String()
	Private aPropertyColCaption As String()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()

	Private m_griddata As GridData
	Private m_PropertyGriddata As PropertyGridData

	Private m_Setting As ClsFOPSetting

	Private m_UtilityUI As UtilityUI
	Private m_MainUtility As SP.Infrastructure.Utility

	Private m_GVMainSettingfilename As String
	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String
	Private m_AllowedChangeMandant As Boolean

	Private m_MainViewGridData As SuportedCodes

#End Region

#Region "Private Consts"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Fremdrechnung"

	Private Const MODUL_NAME_SETTING = "FOP"

#End Region


#Region "Private property"


	Private ReadOnly Property SelectedRowViewData As FoundedFOPData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedFOPData)
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
			m_SettingsManager = New SettingsFOPManager
			Me.m_Setting = New ClsFOPSetting

			m_md = New Mandant
			m_utility = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_MainUtility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_translate = New TranslateValues

			Try
				m_communicationHub = MessageService.Instance.Hub

				m_GVMainSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvMain.Name, ModulConstants.UserData.UserNr)

				m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			Catch ex As Exception

			End Try

			m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200010)
			grpFunction.CustomHeaderButtons(0).Properties.Visible = m_AllowedChangeMandant

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

		AddHandler gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
		AddHandler gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
		AddHandler gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
		AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged

		AddHandler gvLProperty.RowCellClick, AddressOf OngvProperty_RowCellClick
		AddHandler gvLProperty.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground


	End Sub

#End Region


#Region "Private methods"

	'Private Sub LoadXMLDataForSelectedModule()

	'	m_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	If m_griddata.SQLQuery Is Nothing Then
	'		m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	End If

	'	aColFieldName = m_griddata.GridColFieldName.Split(CChar(";"))
	'	aColCaption = m_griddata.GridColCaption.Split(CChar(";"))
	'	aColWidth = m_griddata.GridColWidth.Split(CChar(";"))

	'	aPropertyColFieldName = m_griddata.FieldsInHeaderToShow.Split(CChar(";"))
	'	aPropertyColCaption = m_griddata.CaptionsInHeaderToShow.Split(CChar(";"))

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
		bsiMDData.Caption = String.Format("{0}, {1} | {2}", ModulConstants.MDData.MDName, ModulConstants.MDData.MDCity, ModulConstants.MDData.MDDbName)

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try
		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingFOPKeys.SETTING_FOP_SCC_HEADERINFO_SPLITTERPOSION)
		sccHeaderInfo.SplitterPosition = sccHeaderInfo.Width - 150 ' Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingFOPKeys.SETTING_FOP_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)


		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		sccHeaderInfo.Panel2.MinSize = grpFunction.Width + 10

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		gvLProperty.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvLProperty.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvLProperty.OptionsView.ShowGroupPanel = False
		gvLProperty.OptionsView.ShowIndicator = False
		gvLProperty.OptionsView.ShowAutoFilterRow = False


		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SizeChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccHeaderInfo.SplitterPosition = sccHeaderInfo.Width - 150
		m_SettingsManager.WriteInteger(SettingFOPKeys.SETTING_FOP_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingFOPKeys.SETTING_FOP_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub

#End Region


	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		ResetPropertyGrid()

		Return

		gvMain.Columns.Clear()

		grdLProperty.DataSource = Nothing

		Try
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

				End If


				column.Visible = If(aColWidth(i) > 0, True, False)
				gvMain.Columns.Add(column)

			Next

			RestoreGridLayoutFromXml(gvMain.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMain.DataSource = Nothing
		ResetPropertyGrid()

	End Sub

	Private Sub ResetPropertyGrid()

		gvLProperty.Columns.Clear()

		Try
			For i = 0 To aPropertyColFieldName.Length - 1

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = m_translate.GetSafeTranslationValue(aPropertyColCaption(i).Trim)
				column.Name = aPropertyColFieldName(i).Trim.ToLower
				column.FieldName = aPropertyColFieldName(i).Trim.ToLower
				column.Visible = True

				gvLProperty.Columns.Add(column)

			Next


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdLProperty.DataSource = Nothing

	End Sub


	Private Function LoadFoundedFremdOPList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfCustomerBill = m_DataAccess.GetDbCustomerFOPData4Show(m_griddata.SQLQuery)
		If listOfCustomerBill Is Nothing Then Return False

		Dim responsiblePersonsGridData = (From person In listOfCustomerBill
		Select New FoundedFOPData With
					 {._res = person._res,
						.fopnr = person.fopnr,
						.kdnr = person.kdnr,
						.manr = person.manr,
						.esnr = person.esnr,
						.firma1 = person.firma1,
						.maname = person.maname,
						.kdbranche = person.kdbranche,
						.esab = person.esab,
						.esende = person.esende,
						.krediton = person.krediton,
						.paidon = person.paidon,
						.betragex = person.betragex,
						.betragmwst = person.betragmwst,
						.betragtotal = person.betragtotal,
						.kreditname = person.kreditname,
						.kst3 = person.kst3,
						.esals = person.esals,
						.bemerkung = person.bemerkung,
						.employeeadvisor = person.employeeadvisor,
						.customeradvisor = person.customeradvisor,
						.zfiliale = person.zfiliale,
						.mdnr = person.mdnr,
						.createdon = person.createdon,
		.createdfrom = person.createdfrom
}).ToList()

		Dim listDataSource As BindingList(Of FoundedFOPData) = New BindingList(Of FoundedFOPData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource
		RefreshMainViewStateBar()

		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfCustomerBill Is Nothing
	End Function


	Private Sub frm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		SetFormLayout()

		BuildNewPrintContextMenu()

		LoadFoundedFremdOPList()

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)

	End Sub


	Private Sub FillPopupFields(ByVal selectedrow As FoundedFOPData)
		Dim result As New Dictionary(Of String, String)
		Dim strValue As String = String.Empty

		ResetPropertyGrid()

		Try

			If selectedrow Is Nothing Then
				grdLProperty.Visible = False

				Exit Sub

			Else
				Dim listOfData As New List(Of FoundedFOPData)
				listOfData.Add(selectedrow)
				grdLProperty.DataSource = listOfData
				grdLProperty.Visible = True

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
				Me.m_Setting.SelectedFOPNr = selectedrow.fopnr
				Me.m_Setting.SelectedKDNr = selectedrow.kdnr
				Me.m_Setting.SelectedMANr = selectedrow.manr
				Me.m_Setting.SelectedESNr = selectedrow.esnr

				If selectedrow.fopnr = 0 Then Exit Sub

				FillPopupFields(selectedrow)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

	End Sub

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedFOPData)

				Select Case column.Name.ToLower
					Case "kdnr", "firma1"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "manr", "maname"
						If viewData.manr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case Else
						If viewData.fopnr > 0 Then
							Dim _Clsre As New ClsOpenModul(New ClsSetting With {.SelectedFOPNr = viewData.fopnr})
							'_Clsre.OpenNewFOPForm()
						End If


				End Select

			End If

		End If

	End Sub

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

	Sub OngvProperty_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedFOPData)

				Select Case column.Name.ToLower
					Case "esab", "esals", "esende"
						If viewData.esnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr,
																																	.SelectedESNr = viewData.esnr})
							_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "firma1"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case "maname"
						If viewData.manr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case Else
						If viewData.fopnr > 0 Then
							Dim _Clsre As New ClsOpenModul(New ClsSetting With {.SelectedFOPNr = viewData.fopnr})
							'_Clsre.OpenNewFOPForm()
						End If


				End Select

			End If

		End If

	End Sub


#Region "DropDown Button für New und Print..."

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
				CheckUserProfileDataForNewMandant

				ResetMainGrid()
				LoadFoundedFremdOPList()
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
		LoadFoundedFremdOPList()

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub RefreshMainViewStateBar()

		m_communicationHub.Publish(New RefreshMainViewStatebar(Me, Me.gvMain.RowCount, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
		cmdNew.Enabled = ModulConstants.MDData.ClosedMD = 0

	End Sub

#End Region


#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			Dim _ClsNewPrint As New ClsFOPModuls(Me.m_Setting)
			'_ClsNewPrint.ShowContextMenu4New(cmdNew)


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


			Case Else

				Exit Sub

		End Select

	End Sub

	Private Sub OngvMainColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvMain.SaveLayoutToXml(m_GVMainSettingfilename)

	End Sub


#End Region



End Class


