
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
Imports DevExpress.XtraEditors.ViewInfo

Public Class pgVakanzen


#Region "private consts"

	Private Const MODUL_NAME = "Vakanzenverwaltung"
	Private Const MODUL_NAME_SETTING = "vacancy"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Vakanzenverwaltung"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/es/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/es/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/propose/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/propose/keepfilter"

	'Private Const DATE_COLUMN_NAME As String = "magebdat;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;reminder_1date;reminder_2date;reminder_3date;buchungdate;checkedon;avamreportingdate;avamfrom"
	'Private Const DATE_COLUMN_NAME_LONG As String = "createdon;datum;printdate;checkedon;avamreportingdate;avamuntil;changedon"
	'Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	'Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	'Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline"

#End Region


#Region "private fields"

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub

	Private m_DataAccess As MainGrid
	Private Property LoadedMDNr As Integer
	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Private aPropertyColFieldName As String()
	Private aPropertyColCaption As String()

	Protected m_SettingsManager As ISettingsManager

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues

	Private Shared m_Logger As ILogger = New Logger()
	Private m_griddata As GridData

	Private _iSelectedRecNr As Integer

	Private _ClsVakPropertiesSetting As New ClsVakPropertySetting
	Private _VakSetting As ClsVakSetting

	Private Property gvESDisplayMember As String
	Private Property gvProposeDisplayMember As String

	Private m_UtilityUI As UtilityUI

	Private m_GVMainSettingfilename As String
	Private m_GVESSettingfilename As String
	Private m_GVProposeSettingfilename As String

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingESFilter As String
	Private m_xmlSettingRestoreESSetting As String

	Private m_xmlSettingProposeFilter As String
	Private m_xmlSettingRestoreProposeSetting As String
	Private m_AllowedChangeMandant As Boolean

	'Private m_CheckEditCompleted As RepositoryItemCheckEdit
	'Private m_CheckEditExpire As RepositoryItemCheckEdit
	'Private m_CheckEditWarning As RepositoryItemCheckEdit
	'Private m_CheckEditNotAllowed As RepositoryItemCheckEdit

	Private m_MainViewGridData As SuportedCodes


#End Region


#Region "Private property"

	Private ReadOnly Property SelectedRowViewData As FoundedVacancyData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedVacancyData)
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

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUI = New UtilityUI
		m_common = New CommonSetting
		m_path = New ClsProgPath
		m_DataAccess = New MainGrid
		m_translate = New TranslateValues

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		BarManager1.Form = Me
		LoadedMDNr = ModulConstants.MDData.MDNr
		Me._VakSetting = New ClsVakSetting
		m_SettingsManager = New SettingsVacancyManager
		dpProperties.Options.ShowCloseButton = False

		Try
			m_communicationHub = MessageService.Instance.Hub

			m_GVMainSettingfilename = String.Format("{0}Vacancy\{1}{2}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvMain.Name, ModulConstants.UserData.UserNr)
			m_GVESSettingfilename = String.Format("{0}Vacancy\{1}{2}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvES.Name, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilename = String.Format("{0}Vacancy\{1}{2}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvPropose.Name, ModulConstants.UserData.UserNr)

			m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreESSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingESFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

			m_xmlSettingRestoreProposeSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingProposeFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		'm_CheckEditCompleted = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		'm_CheckEditCompleted.PictureChecked = My.Resources.bullet_ball_green
		'm_CheckEditCompleted.PictureUnchecked = My.Resources.bullet_ball_red
		'm_CheckEditCompleted.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		'm_CheckEditWarning = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		'm_CheckEditWarning.PictureChecked = ImageCollection1.Images(0)
		'm_CheckEditWarning.PictureUnchecked = Nothing
		'm_CheckEditWarning.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		'm_CheckEditNotAllowed = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		'm_CheckEditNotAllowed.PictureChecked = ImageCollection1.Images(2)
		'm_CheckEditNotAllowed.PictureUnchecked = Nothing
		'm_CheckEditNotAllowed.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		'm_CheckEditExpire = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		'm_CheckEditExpire.PictureChecked = My.Resources.bullet_ball_yellow
		'm_CheckEditExpire.PictureUnchecked = Nothing
		'm_CheckEditExpire.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined


		'LoadXMLDataForSelectedModule()

		m_MainViewGridData = New SuportedCodes
		m_MainViewGridData.ChangeColumnNamesToLowercase = False
		m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
		m_griddata = m_MainViewGridData.LoadMainGridData

		Reset()
		TranslateControls()


		AddHandler Me.gvMain.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
		AddHandler Me.gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
		AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged

		AddHandler Me.gvPropose.RowCellClick, AddressOf OngvProposal_RowCellClick
		AddHandler Me.gvPropose.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvPropose.ColumnPositionChanged, AddressOf OngvProposeColumnPositionChanged
		AddHandler Me.gvPropose.ColumnWidthChanged, AddressOf OngvProposeColumnPositionChanged

		AddHandler Me.gvES.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler Me.gvES.ColumnPositionChanged, AddressOf OngvESColumnPositionChanged
		AddHandler Me.gvES.ColumnWidthChanged, AddressOf OngvESColumnPositionChanged

		m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200002)

		LoadFoundedMDList()
		cboMD.EditValue = ModulConstants.MDData.MDNr

		'LoadJobplattformsData()



	End Sub

#End Region


#Region "reset"

	Private Sub Reset()

		ResetMandantenDropDown()

		grdES.DataSource = Nothing
		grdPropose.DataSource = Nothing

		ResetJobplattformsGrid()
		ResetMainGrid()

	End Sub

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		cboMD.Properties.DisplayMember = "MDName"
		cboMD.Properties.ValueMember = "MDNr"

		Dim columns = cboMD.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName", .Width = 100, .Caption = "Mandant"})

		cboMD.Properties.ShowHeader = False
		cboMD.Properties.ShowFooter = False

		cboMD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		cboMD.Properties.SearchMode = SearchMode.AutoComplete
		cboMD.Properties.AutoSearchColumnIndex = 0

		cboMD.Properties.NullText = String.Empty
		cboMD.EditValue = Nothing

	End Sub

	Private Sub ResetJobplattformsGrid()
		' Reset the grid
		gvJobplattforms.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvJobplattforms.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvJobplattforms.OptionsView.ShowGroupPanel = False
		gvJobplattforms.OptionsView.ShowIndicator = False
		gvJobplattforms.OptionsView.ShowAutoFilterRow = False
		gvJobplattforms.OptionsView.ShowColumnHeaders = True

		gvJobplattforms.Columns.Clear()


		Dim columnMD_Name1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMD_Name1.Caption = m_translate.GetSafeTranslationValue("Mandant")
		columnMD_Name1.Name = "MD_Name1"
		columnMD_Name1.FieldName = "MD_Name1"
		columnMD_Name1.Visible = True
		columnMD_Name1.Width = 200
		gvJobplattforms.Columns.Add(columnMD_Name1)

		Dim columnJobplattformLabel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobplattformLabel.Caption = m_translate.GetSafeTranslationValue("Plattform")
		columnJobplattformLabel.Name = "JobplattformLabel"
		columnJobplattformLabel.FieldName = "JobplattformLabel"
		columnJobplattformLabel.Visible = True
		columnJobplattformLabel.Width = 100
		gvJobplattforms.Columns.Add(columnJobplattformLabel)

		Dim columnTranferedJobs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTranferedJobs.Caption = m_translate.GetSafeTranslationValue("Online")
		columnTranferedJobs.Name = "TranferedJobs"
		columnTranferedJobs.FieldName = "TranferedJobs"

		columnTranferedJobs.AppearanceHeader.Options.UseTextOptions = True
		columnTranferedJobs.AppearanceCell.Options.UseTextOptions = True
		columnTranferedJobs.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnTranferedJobs.DisplayFormat.FormatString = "F0"
		columnTranferedJobs.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnTranferedJobs.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

		columnTranferedJobs.Visible = True
		columnTranferedJobs.Width = 100
		gvJobplattforms.Columns.Add(columnTranferedJobs)

		Dim columnTotalAllowedJobsSlot As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTotalAllowedJobsSlot.Caption = m_translate.GetSafeTranslationValue("Total Slots")
		columnTotalAllowedJobsSlot.Name = "TotalAllowedJobsSlot"
		columnTotalAllowedJobsSlot.FieldName = "TotalAllowedJobsSlot"

		columnTotalAllowedJobsSlot.AppearanceHeader.Options.UseTextOptions = True
		columnTotalAllowedJobsSlot.AppearanceCell.Options.UseTextOptions = True
		columnTotalAllowedJobsSlot.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnTotalAllowedJobsSlot.DisplayFormat.FormatString = "F0"
		columnTotalAllowedJobsSlot.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnTotalAllowedJobsSlot.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

		columnTotalAllowedJobsSlot.Visible = True
		columnTotalAllowedJobsSlot.Width = 100
		gvJobplattforms.Columns.Add(columnTotalAllowedJobsSlot)

		Dim columnTotalOpenJobs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTotalOpenJobs.Caption = m_translate.GetSafeTranslationValue("Offene Slots")
		columnTotalOpenJobs.Name = "TotalOpenJobs"
		columnTotalOpenJobs.FieldName = "TotalOpenJobs"
		columnTotalOpenJobs.Visible = True
		columnTotalOpenJobs.Width = 100
		gvJobplattforms.Columns.Add(columnTotalOpenJobs)

		Dim columnTotalSoonExpireJobs As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTotalSoonExpireJobs.Caption = m_translate.GetSafeTranslationValue("Ablauf (heute oder morgen)")
		columnTotalSoonExpireJobs.Name = "TotalSoonExpireJobs"
		columnTotalSoonExpireJobs.FieldName = "TotalSoonExpireJobs"
		columnTotalSoonExpireJobs.Visible = True
		columnTotalSoonExpireJobs.Width = 100
		gvJobplattforms.Columns.Add(columnTotalSoonExpireJobs)


		grdJobplattforms.DataSource = Nothing


	End Sub

	Private Sub ResetMainGrid()

		'm_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, "Vakanzenverwaltung")
		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

	End Sub

	'gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
	'gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
	'gvMain.OptionsView.ShowGroupPanel = False
	'gvMain.OptionsView.ShowIndicator = False
	'gvMain.OptionsView.ShowAutoFilterRow = True

	'gvMain.Columns.Clear()

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

	'	'Dim riButton = New RepositoryItemButtonEdit
	'	'riButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
	'	'Dim EB = New EditorButton
	'	'EB.Kind = ButtonPredefines.Search
	'	'EB.Caption = "mein Test"
	'	'riButton.Buttons.Clear()
	'	'riButton.Buttons.Add(EB)
	'	''AddHandler riButton.Click, AddressOf btnSelectItemDesignation_Click

	'	Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
	'	Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

	'	For i = 0 To aColCaption.Length - 1

	'		Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
	'		Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
	'		'Dim columnName As String = aColFieldName(i).ToLower.Trim
	'		Dim columnName As String = aColFieldName(i).Trim

	'		column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
	'		column.Caption = columnCaption
	'		column.Name = columnName
	'		column.FieldName = columnName

	'		If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
	'			column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
	'			column.DisplayFormat.FormatString = "d"
	'			column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

	'		ElseIf DATE_COLUMN_NAME_LONG.ToLower.Contains(columnName) Then
	'			column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
	'				column.DisplayFormat.FormatString = "g"
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


	'			ElseIf columnName = "ourisonline" Then
	'				Trace.WriteLine(String.Format("{0} >>> {1}", columnName, columnCaption))
	'				column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
	'				column.ColumnEdit = reproCheckbox
	'				column.AppearanceHeader.Options.UseTextOptions = True

	'			ElseIf columnName = "JobsCHWillbeExpireSoon" OrElse columnName = "OstJobWillbeExpireSoon" OrElse column.FieldName.ToLower.Contains("jobchannelpriority") Then
	'				Trace.WriteLine(String.Format("{0} >>> {1}", columnName, columnCaption))
	'				column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
	'				column.AppearanceHeader.Options.UseTextOptions = True
	'				column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
	'				column.DisplayFormat.FormatString = "F0"
	'				column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

	'			Else
	'				column.ColumnEdit = repoHTML
	'			column.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways

	'		End If

	'		'If i = 9 Then Trace.WriteLine(String.Format("{0} >>> {1}", columnName, columnCaption))
	'		column.Visible = If(aColWidth(i) > 0, True, False)

	'		If column.FieldName.ToLower.Contains("online") AndAlso Not column.FieldName.ToLower.Contains("AVAMIsNowOnline".ToLower) Then column.ColumnEdit = m_CheckEditCompleted
	'		If column.FieldName.ToLower.Contains("AVAMIsNowOnline".ToLower) Then column.ColumnEdit = m_CheckEditNotAllowed
	'		If column.FieldName.ToLower.Contains("AVAMReportingObligation".ToLower) AndAlso Not column.FieldName.ToLower.Contains("AVAMIsNowOnline".ToLower) Then column.ColumnEdit = m_CheckEditWarning
	'		If column.FieldName.ToLower.Contains("JobsCHExpire".ToLower) OrElse column.FieldName.ToLower.Contains("OstJobExpire".ToLower) Then column.ColumnEdit = m_CheckEditExpire


	'		gvMain.Columns.Add(column)

	'	Next

	'	RestoreGridLayoutFromXml(gvMain.Name.ToLower)

	'Catch ex As Exception
	'	m_Logger.LogError(ex.ToString)
	'End Try

	'grdMain.DataSource = Nothing


	Sub ResetProposalDetailGrid()

		gvPropose.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvPropose.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvPropose.OptionsView.ShowGroupPanel = False
		gvPropose.OptionsView.ShowIndicator = False
		gvPropose.OptionsView.ShowAutoFilterRow = False


		gvPropose.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "pnr"
			columnmodulname.FieldName = "pnr"
			columnmodulname.Visible = False
			gvPropose.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("KDNr")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvPropose.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvPropose.Columns.Add(columnZHDNr)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_translate.GetSafeTranslationValue("MANr")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvPropose.Columns.Add(columnMANr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "bezeichnung"
			columnBezeichnung.FieldName = "bezeichnung"
			columnBezeichnung.Visible = True
			gvPropose.Columns.Add(columnBezeichnung)

			Dim columnZname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZname.Caption = m_translate.GetSafeTranslationValue("ZHD.-Person")
			columnZname.Name = "zhdname"
			columnZname.FieldName = "zhdname"
			columnZname.Visible = False
			gvPropose.Columns.Add(columnZname)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvPropose.Columns.Add(columnEmployeename)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_translate.GetSafeTranslationValue("Art")
			columndestesnr.Name = "p_art"
			columndestesnr.FieldName = "p_art"
			columndestesnr.Visible = True
			gvPropose.Columns.Add(columndestesnr)

			Dim columnlonr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlonr.Caption = m_translate.GetSafeTranslationValue("Status")
			columnlonr.Name = "p_state"
			columnlonr.FieldName = "p_state"
			columnlonr.Visible = True
			gvPropose.Columns.Add(columnlonr)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "advisor"
			columnAdvisor.FieldName = "advisor"
			columnAdvisor.Visible = False
			gvPropose.Columns.Add(columnAdvisor)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvPropose.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvPropose.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvPropose.Columns.Add(columnZFiliale)


			RestoreGridLayoutFromXml(gvPropose.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdPropose.DataSource = Nothing

	End Sub

	Private Sub ResetESDetailGrid()

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

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columncustomername.Name = "employeename"
			columncustomername.FieldName = "employeename"
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


#End Region

	Sub TranslateControls()

		Me.cmdNew.Text = m_translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpPropose.Text = m_translate.GetSafeTranslationValue(Me.grpPropose.Text)
		Me.grpES.Text = m_translate.GetSafeTranslationValue(Me.grpES.Text)

		Me.dpProperties.Text = m_translate.GetSafeTranslationValue(Me.dpProperties.Text)

	End Sub


	'Sub LoadXMLDataForSelectedModule()

	'	m_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	If m_griddata.SQLQuery Is Nothing Then
	'		m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	End If

	'	aColFieldName = m_griddata.GridColFieldName.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "").Split(CChar(";"))
	'	aColCaption = m_griddata.GridColCaption.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "").Split(CChar(";"))
	'	aColWidth = m_griddata.GridColWidth.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "").Split(CChar(";"))

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
	'						   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'															And exportSetting.Attribute("ID").Value = MODUL_NAME
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

	Private Function GetUserGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
		Dim result As New GridData
		If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
		m_path = New ClsProgPath

		Try
			Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr))
			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
							   Where Not (exportSetting.Attribute("ID") Is Nothing) _
																And exportSetting.Attribute("ID").Value = MODUL_NAME
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
	End Function

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

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperties.Width = Math.Max(Me.dpProperties.Width, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_SCC_NAV_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainNav_1.SplitterPosition = Math.Max(Me.sccMainNav_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingVacancyKeys.SETTING_VAK_SCC_PROPERTY_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccMainProp_1.SplitterPosition = Math.Max(Me.sccMainProp_1.SplitterPosition, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75


		'pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill

		Me.gvLProperty.OptionsView.ShowIndicator = False


		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties

		AddHandler dpProperties.Resize, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SizeChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_1.SplitterPositionChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingVacancyKeys.SETTING_VAK_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingVacancyKeys.SETTING_VAK_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingVacancyKeys.SETTING_VAK_DPPROPERTY_WIDTH, Me.dpProperties.Width)

		m_SettingsManager.WriteInteger(SettingVacancyKeys.SETTING_VAK_SCC_NAV_1_SPLITTERPOSION, Me.sccMainNav_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingVacancyKeys.SETTING_VAK_SCC_PROPERTY_1_SPLITTERPOSION, Me.sccMainProp_1.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub

	Private Function LoadFoundedVacanciesList() As Boolean

		Dim listOfFoundedData = m_DataAccess.GetDbVacancyData4Show(m_griddata.SQLQuery)
		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Return False
		End If

		Dim dataList = (From person In listOfFoundedData
						Select New FoundedVacancyData With
					 {
						._res = person._res,
						.Customer_ID = person.Customer_ID,
						.mdnr = person.mdnr,
						.vaknr = person.vaknr,
						.kdnr = person.kdnr,
						.kdzhdnr = person.kdzhdnr,
						.vakstate = person.vakstate,
						.vak_kanton = person.vak_kanton,
						.vaklink = person.vaklink,
						.vakkontakt = person.vakkontakt,
						.vacancygruppe = person.vacancygruppe,
						.vacancyplz = person.vacancyplz,
						.vacancyort = person.vacancyort,
						.titelforsearch = person.titelforsearch,
						.shortdescription = person.shortdescription,
						.firma1 = person.firma1,
						.bezeichnung = person.bezeichnung,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.changedon = person.changedon,
						.changedfrom = person.changedfrom,
						.kdzname = person.kdzname,
						.advisor = person.advisor,
						.kdemail = person.kdemail,
						.zemail = person.zemail,
						.jchisonline = person.jchisonline,
						.ojisonline = person.ojisonline,
						.JobsCHExpire = person.JobsCHExpire,
						.OstJobExpire = person.OstJobExpire,
						.ourisonline = person.ourisonline,
						.jobchannelpriority = person.jobchannelpriority,
						.JobsCHWillbeExpireSoon = person.JobsCHWillbeExpireSoon,
						.OstJobWillbeExpireSoon = person.OstJobWillbeExpireSoon,
						.AVAMRecordState = person.AVAMRecordState,
						.AVAMJobroomID = person.AVAMJobroomID,
						.AVAMFrom = person.AVAMFrom,
						.AVAMUntil = person.AVAMUntil,
						.AVAMReportingDate = person.AVAMReportingDate,
						.AVAMReportingObligation = person.AVAMReportingObligation,
						.AVAMReportingObligationEndDate = person.AVAMReportingObligationEndDate,
						.kdtelefon = person.kdtelefon,
						.kdtelefax = person.kdtelefax,
						.ztelefon = person.ztelefon,
						.ztelefax = person.ztelefax,
						.znatel = person.znatel,
						.jobchdate = person.jobchdate,
						.ostjobchdate = person.ostjobchdate,
						.zfiliale = person.zfiliale
					}).ToList()

		Dim listDataSource As BindingList(Of FoundedVacancyData) = New BindingList(Of FoundedVacancyData)

		For Each p In dataList
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource

		RefreshMainViewStateBar()

		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Function LoadJobplattformsData() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.LoadJobplattformData()
		If listOfFoundedData Is Nothing Then
			ShowErrDetail(m_translate.GetSafeTranslationValue("Jobplattform Daten konnten nicht geladen werden."))

			Return False
		End If

		Dim listData = (From person In listOfFoundedData
						Select New JobPlattformsInfoData With
					 {
						.Customer_ID = person.Customer_ID,
						.MD_Name1 = person.MD_Name1,
						.JobplattformLabel = person.JobplattformLabel,
						.TranferedJobs = person.TranferedJobs,
						.TotalAllowedJobsSlot = person.TotalAllowedJobsSlot,
						.TotalSoonExpireJobs = person.TotalSoonExpireJobs
					}).ToList()

		Dim listDataSource As BindingList(Of JobPlattformsInfoData) = New BindingList(Of JobPlattformsInfoData)

		For Each p In listData
			listDataSource.Add(p)
		Next

		grdJobplattforms.DataSource = listDataSource


		Return Not listDataSource Is Nothing
	End Function

	Private Sub OngvJobplattforms_CustomDrawCell(sender As Object, e As RowCellCustomDrawEventArgs) Handles gvJobplattforms.CustomDrawCell
		Dim warningImage As Image = My.Resources.bullet_ball_yellow ' Image.("c:\warningImage.png")

		If e.Column.FieldName = "TotalSoonExpireJobs" OrElse e.Column.FieldName = "TotalOpenJobs" Then
			e.DefaultDraw()

			If Convert.ToInt32(e.CellValue) > 0 Then
				If e.Column.FieldName = "TotalOpenJobs" Then
					warningImage = My.Resources.bullet_ball_red
				ElseIf e.Column.FieldName = "TotalSoonExpireJobs" Then
					warningImage = My.Resources.bullet_ball_yellow
				Else
					Return

				End If

				Dim cellInfo As GridCellInfo = CType(e.Cell, GridCellInfo)
					Dim viewInfo As TextEditViewInfo = CType(cellInfo.ViewInfo, TextEditViewInfo)
					Dim imageRect As Rectangle = viewInfo.ContextImageBounds
					imageRect.X = e.Bounds.X + e.Bounds.Width / 2 - imageRect.Width / 2
					imageRect.Y = e.Bounds.Y + e.Bounds.Height / 2 - imageRect.Height / 2

					e.Cache.DrawImage(warningImage, imageRect.X, e.Bounds.Y)
				End If
			End If
	End Sub

	Private Sub OngvJobplattforms_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvJobplattforms.CustomColumnDisplayText

		If e.Column.FieldName = "TranferedJobs" OrElse e.Column.FieldName = "TotalAllowedJobsSlot" OrElse e.Column.FieldName = "TotalSoonExpireJobs" OrElse e.Column.FieldName = "TotalOpenJobs" Then
			If Val(e.Value) = 0 Then e.DisplayText = String.Empty

			'ElseIf e.Column.FieldName = "jobchannelpriority" Then
			'	If Val(e.Value) = 0 Then e.DisplayText = String.Empty

		End If

	End Sub

	Private Sub FocusJobplattformData()

		If Not grdJobplattforms.DataSource Is Nothing Then

			Dim todoData = CType(gvJobplattforms.DataSource, BindingList(Of JobPlattformsInfoData))
			Dim selectedVacancy = SelectedRowViewData

			Dim index = todoData.ToList().FindIndex(Function(data) data.Customer_ID = selectedVacancy.Customer_ID)

			Dim rowHandle = gvJobplattforms.GetRowHandle(index)
			gvJobplattforms.FocusedRowHandle = rowHandle
		End If

	End Sub

	Private Sub OngvMain_CustomDrawCell(sender As Object, e As RowCellCustomDrawEventArgs) Handles gvMain.CustomDrawCell
		Dim changeDrawCell As Boolean = True
		'Return

		Try
			If e.Column.FieldName.ToLower = "ourisonline".ToLower Then
				Dim warningImage As Image = My.Resources.bullet_ball_yellow
				If e.CellValue Is Nothing Then Return
			End If

			If e.Column.FieldName.ToLower = "JobsCHWillbeExpireSoon".ToLower OrElse e.Column.FieldName.ToLower = "OstJobWillbeExpireSoon".ToLower Then
				Dim warningImage As Image = My.Resources.bullet_ball_yellow
				If e.CellValue Is Nothing Then Return

				e.DefaultDraw()
				Dim myValue As FoundedVacancyData.JobplattformEnum
				If Val(e.CellValue) = 0 Then
					If e.CellValue = "ONLINE" Then myValue = FoundedVacancyData.JobplattformEnum.ONLINE
					If e.CellValue = "EXPIRING" Then myValue = FoundedVacancyData.JobplattformEnum.EXPIRING
				Else
					myValue = CType(e.CellValue, FoundedVacancyData.JobplattformEnum)
				End If

				Dim cellInfo As GridCellInfo = TryCast(e.Cell, GridCellInfo)
				Dim info As TextEditViewInfo = TryCast(cellInfo.ViewInfo, TextEditViewInfo)
				Dim textEdit As New RepositoryItemTextEdit()

				If (myValue) = FoundedVacancyData.JobplattformEnum.ONLINE Then
					textEdit.ContextImageOptions.Image = My.Resources.bullet_ball_green
					warningImage = My.Resources.bullet_ball_green

				ElseIf myValue = FoundedVacancyData.JobplattformEnum.EXPIRING Then
					textEdit.ContextImageOptions.Image = My.Resources.bullet_ball_yellow
					warningImage = My.Resources.bullet_ball_yellow

				Else
					changeDrawCell = False
				End If

				If changeDrawCell Then
					Dim viewInfo As TextEditViewInfo = CType(cellInfo.ViewInfo, TextEditViewInfo)
					Dim imageRect As Rectangle = viewInfo.ContextImageBounds
					imageRect.X = e.Bounds.X + e.Bounds.Width / 2 - imageRect.Width / 2
					imageRect.Y = e.Bounds.Y + e.Bounds.Height / 2 - imageRect.Height / 2

					'e.Appearance.DrawString(e.Cache, String.Empty, e.Bounds)
					e.Cache.DrawImage(warningImage, imageRect.X, e.Bounds.Y)
					'e.Cache.DrawImage(warningImage, e.Bounds.Location)
				End If

			ElseIf e.Column.FieldName.ToLower = "jobchannelpriority".ToLower Then
				Dim warningImage As Image = My.Resources.bullet_ball_yellow
				If e.CellValue Is Nothing Then Return

				e.DefaultDraw()
				Dim myValue As FoundedVacancyData.JobplattformStateEnum
				If Val(e.CellValue) = 0 Then
					If e.CellValue = "ONLINE" Then
						myValue = FoundedVacancyData.JobplattformStateEnum.ONLINE
					Else
						myValue = FoundedVacancyData.JobplattformStateEnum.OFFLINE
					End If
				Else
					myValue = CType(e.CellValue, FoundedVacancyData.JobplattformStateEnum)
				End If

				Dim cellInfo As GridCellInfo = TryCast(e.Cell, GridCellInfo)
				Dim info As TextEditViewInfo = TryCast(cellInfo.ViewInfo, TextEditViewInfo)
				Dim textEdit As New RepositoryItemTextEdit()

				If (myValue) = FoundedVacancyData.JobplattformStateEnum.ONLINE Then
					textEdit.ContextImageOptions.Image = My.Resources.bullet_ball_green
					warningImage = My.Resources.bullet_ball_green

				Else
					changeDrawCell = False
				End If

				If changeDrawCell Then
					Dim viewInfo As TextEditViewInfo = CType(cellInfo.ViewInfo, TextEditViewInfo)
					Dim imageRect As Rectangle = viewInfo.ContextImageBounds
					imageRect.X = e.Bounds.X + e.Bounds.Width / 2 - imageRect.Width / 2
					imageRect.Y = e.Bounds.Y + e.Bounds.Height / 2 - imageRect.Height / 2

					'e.Appearance.DrawString(e.Cache, String.Empty, e.Bounds)
					e.Cache.DrawImage(warningImage, imageRect.X, e.Bounds.Y)
					'e.Cache.DrawImage(warningImage, e.Bounds.Location)
				End If

			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub OngvMain_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvMain.CustomColumnDisplayText

		If e.Column.FieldName.ToLower = "JobsCHWillbeExpireSoon".ToLower OrElse e.Column.FieldName.ToLower = "OstJobWillbeExpireSoon".ToLower Then
			e.DisplayText = String.Empty

		ElseIf e.Column.FieldName.ToLower = "jobchannelpriority".ToLower Then
			e.DisplayText = String.Empty

		End If

	End Sub

	Private Sub gvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs) Handles gvMain.ColumnFilterChanged

		RefreshMainViewStateBar()

	End Sub

	Private Sub pgVakanzen_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadJobplattformsData()
		LoadFoundedVacanciesList()

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedVacancyData)
				Me._VakSetting.SelectedMDNr = viewData.mdnr
				Me._VakSetting.SelectedVakNr = viewData.vaknr
				Me._VakSetting.SelectedKDNr = viewData.kdnr
				Me._VakSetting.SelectedKDzNr = viewData.kdzhdnr

				Select Case column.Name.ToLower
					Case "ztelefon"
						If viewData.ztelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.ztelefon)
						End If

					Case "znatel"
						If viewData.znatel.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.znatel)
						End If

					Case "kdtelefon"
						If viewData.kdtelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = 0})
							_ClsKD.TelefonCallToCustomer(viewData.kdtelefon)
						End If

					Case "kdzname", "kdzhdnr"
						If viewData.kdzhdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
							_ClsKD.OpenSelectedCPerson() 'ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "firma1", "kdnr"
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

					Case Else
						If viewData.vaknr > 0 Then
							Dim _ClsVak As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = _VakSetting.SelectedMDNr,
																							.SelectedVakNr = Me._VakSetting.SelectedVakNr,
																							.SelectedKDNr = Me._VakSetting.SelectedKDNr,
																							.SelectedZHDNr = Me._VakSetting.SelectedKDzNr})
							_ClsVak.OpenSelectedVacancyTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

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
				Me._VakSetting.SelectedVakNr = selectedrow.vaknr
				Me._VakSetting.SelectedKDNr = selectedrow.kdnr
				If selectedrow.kdnr = 0 Then Exit Sub
				_VakSetting.SelectedKDzNr = selectedrow.kdzhdnr

				FocusJobplattformData()

				FillPopupFields(selectedrow)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4Vacancies()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navigationsleiste. {1}", strMethodeName, ex.Message))
			ShowErrDetail(ex.Message)

		End Try


	End Sub


	Private Sub FillPopupFields(ByVal selectedrow As FoundedVacancyData)
		Dim result As New Dictionary(Of String, String)
		Dim strValue As String = String.Empty

		Dim listOfPropertyLData As New List(Of PropertyData)
		Dim listOfPropertyRData As New List(Of PropertyData)
		grdLProperty.DataSource = Nothing

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
					Case "advisor"
						strValue = selectedrow.advisor

					Case "firma1"
						strValue = selectedrow.firma1

					Case "bezeichnung"
						strValue = selectedrow.bezeichnung

					Case "createdfrom"
						strValue = selectedrow.createdfrom

					Case "createdon"
						strValue = selectedrow.createdon

					Case "kdemail"
						strValue = selectedrow.kdemail

					Case "kdnr"
						strValue = selectedrow.kdnr

					Case "jchisonline"
						strValue = selectedrow.jchisonline
					Case "ojisonline"
						strValue = selectedrow.ojisonline
					'Case "ourisonline"
					'	strValue = selectedrow.ourisonline
					Case "jobchannelpriority"
						strValue = selectedrow.jobchannelpriority

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

					Case "kdzhdnr"
						strValue = selectedrow.kdzhdnr
					Case "kdzname"
						strValue = selectedrow.kdzname

					Case "titelforsearch"
						strValue = selectedrow.titelforsearch
					Case "shortdescription"
						strValue = selectedrow.shortdescription

					Case "vacancygruppe"
						strValue = selectedrow.vacancygruppe
					Case "vacancyort"
						strValue = selectedrow.vacancyort
					Case "vacancyplz"
						strValue = selectedrow.vacancyplz

					Case "vak_kanton"
						strValue = selectedrow.vak_kanton
					Case "vakkontakt"
						strValue = selectedrow.vakkontakt
					Case "vaklink"
						strValue = selectedrow.vaklink
					Case "vakstate"
						strValue = selectedrow.vakstate

					Case "jobchdate"
						strValue = selectedrow.jobchdate
					Case "ostjobchdate"
						strValue = selectedrow.ostjobchdate

					Case "zfiliale"
						strValue = selectedrow.zfiliale


					Case Else
						strValue = "?"

				End Select

				result.Add(aPoupFields(j), strValue)
			Next

			Dim itemName As String
			Dim itemValue As String

			For i As Integer = 0 To result.Count - 1
				If Not String.IsNullOrEmpty(aPoupCaption(i)) And Not String.IsNullOrEmpty(result.Item(aPoupFields(i))) Then

					'If i > iCountofFieldInHeaderInfo Then
					'itemName = aPoupCaption(i)
					'	itemValue = result.Item(aPoupFields(i))

					'	Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
					'	listOfPropertyRData.Add(propertyItem)

					'Else
					itemName = aPoupCaption(i)
					itemValue = result.Item(aPoupFields(i))

					Dim propertyItem As New PropertyData With {.ValueName = m_translate.GetSafeTranslationValue(itemName), .Value = itemValue}
					listOfPropertyLData.Add(propertyItem)

					'End If

				End If
			Next

			If Not (listOfPropertyLData Is Nothing) Then
				grdLProperty.DataSource = listOfPropertyLData
				grdLProperty.Visible = True

			Else
				grdLProperty.Visible = False

			End If

			'If Not (listOfPropertyRData Is Nothing) Then
			'	grdRProperty.DataSource = listOfPropertyRData
			'	grdRProperty.Visible = True

			'Else
			'	grdRProperty.Visible = False

			'End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			ShowErrDetail(ex.Message)
			result = Nothing

		End Try

	End Sub

	Sub FillProperties4Vacancies()

		Me._ClsVakPropertiesSetting.gES = Me.grdES
		_ClsVakPropertiesSetting.gPropose = Me.grdPropose
		_ClsVakPropertiesSetting.SelectedVakNr = Me._VakSetting.SelectedVakNr

		ResetESDetailGrid()
		LoadVacanciesESDetailList()

		' Vacancies proposal
		ResetProposalDetailGrid()
		LoadVacanciesProposalDetailList()

	End Sub




#Region "Einsatz Funktionen..."

	Public Function LoadVacanciesESDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbCustomerESDataForProperties(Me._VakSetting.SelectedVakNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedVacancyESDetailData With
					 {.mdnr = person.mdnr,
						.esnr = person.esnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.esals = person.esals,
						.employeename = person.employeename,
						.periode = person.periode,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedVacancyESDetailData) = New BindingList(Of FoundedVacancyESDetailData)

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
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr,
																															.SelectedESNr = viewData.esnr})
					_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub


#End Region


#Region "Propose Funktionen..."



	Public Function LoadVacanciesProposalDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbVacanciesProposalDataForProperties(Me._VakSetting.SelectedVakNr)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedVacanciesProposalDetailData With
					 {.mdnr = person.mdnr,
						.employeeMDNr = person.employeeMDNr,
						.customerMDNr = person.customerMDNr,
						.pnr = person.pnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.bezeichnung = person.bezeichnung,
						.manr = person.manr,
						.employeename = person.employeename,
						.zhdname = person.zhdname,
						.p_art = person.p_art,
						.p_state = person.p_state,
						.advisor = person.advisor,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedVacanciesProposalDetailData) = New BindingList(Of FoundedVacanciesProposalDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdPropose.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvProposal_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPropose.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedVacanciesProposalDetailData)

				If viewData.pnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedProposeNr = viewData.pnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
					_ClsKD.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
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


	' Mandantendaten...
	Private Sub OnCboMD_EditValueChanged(sender As Object, e As System.EventArgs) Handles cboMD.EditValueChanged
		Dim SelectedData As MDData = TryCast(Me.cboMD.GetSelectedDataRow(), MDData)
		Dim OldMDNr = ModulConstants.MDData.MDNr

		If Not SelectedData Is Nothing Then

			ModulConstants.MDData.MDNr = cboMD.EditValue
			If OldMDNr <> SelectedData.MDNr Then
				ModulConstants.MDData = ModulConstants.SelectedMDData(cboMD.EditValue)
				ResetMainGrid()
				LoadFoundedVacanciesList()
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

		ResetJobplattformsGrid()
		LoadJobplattformsData()

		ResetMainGrid()
		LoadFoundedVacanciesList()


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

			Me._VakSetting.Data4SelectedVak = True
			strModul2Open = "ES".ToLower

			Dim frm As New frmVakDetails(Me._VakSetting, strModul2Open)
			frm.Show()
		End If

	End Sub

	Private Sub OngrpPropose_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpPropose.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Me._VakSetting.Data4SelectedVak = True
			strModul2Open = "Propose".ToLower

			Dim frm As New frmVakDetails(Me._VakSetting, strModul2Open)
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

			Case "gvpropose".ToLower
				Try
					keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingProposeFilter), False)
					restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreProposeSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVProposeSettingfilename) Then gvPropose.RestoreLayoutFromXml(m_GVProposeSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvPropose.ActiveFilterCriteria = Nothing


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

	Private Sub OngvProposeColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvPropose.SaveLayoutToXml(m_GVProposeSettingfilename)

	End Sub

#End Region



#Region "Helpers"


	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			Dim _ClsNewPrint As New ClsVacanciesModuls(Me._VakSetting)
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

