

Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.Infrastructure.UI.UtilityUI
Imports SP.Infrastructure.Settings

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

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPS.MainView.EmployeeSettings

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SP.MA.EmployeeMng
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ModulView.DataObjects
Imports SP.DatabaseAccess.ModulView

Public Class pageKandidaten


	Public Delegate Sub UpdateStatusBarDataHandler(ByVal sender As Object, ByVal recordnumber As Integer)

#Region "Private Consts"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}] | [{1}]"
	Private Const MODUL_NAME_SETTING = "employee"
	Private Const MODUL_NAME As String = "Kandidatenverwaltung"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/es/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/es/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/propose/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/propose/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/contact/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/contact/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/zg/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/zg/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_LO_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/lo/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_LO_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/lo/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/rp/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER As String = "gridsetting/User_{0}/mainview/{1}/{1}properties/rp/keepfilter"

	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NR As String = "MD_{0}/Sonstiges/autofilterconditionnr"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE As String = "MD_{0}/Sonstiges/autofilterconditiondate"

	Private Const DATE_COLUMN_NAME As String = "magebdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate;faelligdate;checkedon;avamreportingdate;avamfrom;avamuntil"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;ourisonline;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline;noes;webeexport"

#End Region


#Region "private fields"

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The modulview database access.
	''' </summary>
	Protected m_ModulViewDatabaseAccess As IModulViewDatabaseAccess

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	Private m_Stopwatch As Stopwatch

	Protected m_SettingsManager As ISettingsManager
	Private m_communicationHub As TinyMessenger.ITinyMessengerHub

	Private m_GVMainSettingfilename As String
	Private m_GVESSettingfilename As String
	Private m_GVProposeSettingfilename As String
	Private m_GVContactSettingfilename As String
	Private m_GVZGSettingfilename As String
	Private m_GVSalarySettingfilename As String
	Private m_GVReportSettingfilename As String

	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Private m_MandantData As Mandant
	Private m_utilitySP As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	'Private m_translate As TranslateValues

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


	Private m_griddata As GridData

	Private _ClsMASetting As New ClsMASetting
	Private _ClsMAPropertiesSetting As New ClsMAPropertySetting

	Private Property LoadedMDNr As Integer


	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingESFilter As String
	Private m_xmlSettingRestoreESSetting As String

	Private m_xmlSettingProposeFilter As String
	Private m_xmlSettingRestoreProposeSetting As String

	Private m_xmlSettingContactFilter As String
	Private m_xmlSettingRestoreContactSetting As String

	Private m_xmlSettingZGFilter As String
	Private m_xmlSettingRestoreZGSetting As String

	Private m_xmlSettingLOFilter As String
	Private m_xmlSettingRestoreLOSetting As String

	Private m_xmlSettingRPFilter As String
	Private m_xmlSettingRestoreRPSetting As String

	Private m_autofilterconditionNumber As AutoFilterCondition
	Private m_autofilterconditionDate As AutoFilterCondition

	Private m_CurrentEmployeeNumber As Integer
	Private m_AllowedChangeMandant As Boolean

	Private m_CheckEditCompleted As RepositoryItemCheckEdit
	Private m_CheckEditYes As RepositoryItemCheckEdit
	Private m_CheckEditExpire As RepositoryItemCheckEdit
	Private m_CheckEditWarning As RepositoryItemCheckEdit
	Private m_CheckEditNotAllowed As RepositoryItemCheckEdit

	Private m_ExistsFilterForBackupBeforeEQuest As Boolean
	Private m_MainViewGridData As SuportedCodes

#End Region


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			m_Stopwatch = New Stopwatch()

			BarManager1.Form = Me
			LoadedMDNr = ModulConstants.MDData.MDNr
			m_SettingsManager = New SettingsESManager

			m_communicationHub = MessageService.Instance.Hub
			m_MandantData = New Mandant
			m_utilitySP = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath
			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_ExistsFilterForBackupBeforeEQuest = False

			dpProperties.Options.ShowCloseButton = False
			dpSProperties.Options.ShowCloseButton = False


			Try
				m_GVMainSettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvMain.Name, ModulConstants.UserData.UserNr)
				m_GVESSettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvES.Name, ModulConstants.UserData.UserNr)
				m_GVProposeSettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvPropose.Name, ModulConstants.UserData.UserNr)
				m_GVContactSettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvContact.Name, ModulConstants.UserData.UserNr)
				m_GVZGSettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvZG.Name, ModulConstants.UserData.UserNr)
				m_GVSalarySettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvLO.Name, ModulConstants.UserData.UserNr)
				m_GVReportSettingfilename = String.Format("{0}Employee\{1}{2}.xml", ModulConstants.GridSettingPath, gvRP.Name, ModulConstants.UserData.UserNr)

				m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreESSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingESFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreProposeSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingProposeFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreContactSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingContactFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreZGSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingZGFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZG_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreLOSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_LO_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingLOFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_LO_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreRPSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingRPFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))
				'm_autofilterconditionNumber = AutoFilterConditionNumber
				'm_autofilterconditionDate = AutoFilterConditionDate

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

			m_CheckEditCompleted = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditCompleted.PictureChecked = My.Resources.bullet_ball_green
			m_CheckEditCompleted.PictureUnchecked = My.Resources.bullet_ball_red
			m_CheckEditCompleted.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_CheckEditYes = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditYes.PictureChecked = My.Resources.bullet_ball_green
			m_CheckEditYes.PictureUnchecked = Nothing
			m_CheckEditYes.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_CheckEditWarning = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditWarning.PictureChecked = ImageCollection1.Images(0)
			m_CheckEditWarning.PictureUnchecked = Nothing
			m_CheckEditWarning.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_CheckEditNotAllowed = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditNotAllowed.PictureChecked = ImageCollection1.Images(2)
			m_CheckEditNotAllowed.PictureUnchecked = Nothing
			m_CheckEditNotAllowed.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_CheckEditExpire = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditExpire.PictureChecked = My.Resources.bullet_ball_yellow
			m_CheckEditExpire.PictureUnchecked = Nothing
			m_CheckEditExpire.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

			AddHandler Me.gvMain.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
			AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
			AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged
			AddHandler Me.gvMain.MouseDown, AddressOf gvMainOnMouseDown

			AddHandler gvMain.FocusedRowChanged, AddressOf OnGvMain_FocusedRowChanged
			AddHandler gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground


			AddHandler Me.gvES.RowCellClick, AddressOf OngvES_RowCellClick
			AddHandler Me.gvES.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvES.ColumnPositionChanged, AddressOf OngvESColumnPositionChanged
			AddHandler Me.gvES.ColumnWidthChanged, AddressOf OngvESColumnPositionChanged

			AddHandler Me.gvPropose.RowCellClick, AddressOf OngvProposal_RowCellClick
			AddHandler Me.gvPropose.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvPropose.ColumnPositionChanged, AddressOf OngvProposeColumnPositionChanged
			AddHandler Me.gvPropose.ColumnWidthChanged, AddressOf OngvProposeColumnPositionChanged

			AddHandler Me.gvRP.RowCellClick, AddressOf OngvRP_RowCellClick
			AddHandler Me.gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvReportColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvReportColumnPositionChanged

			AddHandler Me.gvZG.RowCellClick, AddressOf OngvZG_RowCellClick
			AddHandler Me.gvZG.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvZG.ColumnPositionChanged, AddressOf OngvZGColumnPositionChanged
			AddHandler Me.gvZG.ColumnWidthChanged, AddressOf OngvZGColumnPositionChanged

			AddHandler Me.gvLO.RowCellClick, AddressOf OngvSalary_RowCellClick
			AddHandler Me.gvLO.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvLO.ColumnPositionChanged, AddressOf OngvSalaryColumnPositionChanged
			AddHandler Me.gvLO.ColumnWidthChanged, AddressOf OngvSalaryColumnPositionChanged

			AddHandler Me.gvContact.RowCellClick, AddressOf OngvContact_RowCellClick
			AddHandler Me.gvContact.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvContact.ColumnPositionChanged, AddressOf OngvContactColumnPositionChanged
			AddHandler Me.gvContact.ColumnWidthChanged, AddressOf OngvContactColumnPositionChanged

			m_MainViewGridData = New SuportedCodes
			m_MainViewGridData.ChangeColumnNamesToLowercase = True
			m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
			m_griddata = m_MainViewGridData.LoadMainGridData

			ResetMandantenDropDown()
			LoadFoundedMDList()
			cboMD.EditValue = ModulConstants.MDData.MDNr

			m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200000)
			grpFunction.CustomHeaderButtons(0).Properties.Enabled = m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").CVDropIN AndAlso UserSecValue(678)
			grpFunction.CustomHeaderButtons(1).Properties.Enabled = m_AllowedChangeMandant

			'LoadXMLDataForSelectedModule()
			ResetMainGrid()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try


	End Sub

#End Region


#Region "private Properties"

	Private ReadOnly Property SelectedRowViewData As FoundedEmployeeData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedEmployeeData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

	'Private ReadOnly Property GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Get
	'		Dim result As New GridData
	'		If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'		Dim filename As String = m_MandantData.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr)

	'		m_path = New ClsProgPath
	'		If Not File.Exists(filename) Then
	'			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Einsatellungsdatei für die Auflistung wurde nicht gefunden: {0}"), filename))

	'			Return Nothing
	'		End If

	'		Try
	'			Dim xDoc As XDocument = XDocument.Load(filename)
	'			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'							   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'																And exportSetting.Attribute("ID").Value = "f6c163f6-3dab-4db8-b8dd-a7cd19b7017c"
	'							   Select New With {
	'																					.SQLQuery = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																					.GridColFieldName = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																					.DisplayMember = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																					.GridColCaption = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																					.GridColWidth = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																					.BackColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																					.ForeColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																					.PopupFields = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																					.PopupCaptions = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																					.CountOfFieldsInHeader = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																					.FieldsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																					.CaptionsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																						}).FirstOrDefault()



	'			result.SQLQuery = ConfigQuery.SQLQuery
	'			result.GridColFieldName = ConfigQuery.GridColFieldName
	'			result.DisplayMember = ConfigQuery.DisplayMember
	'			result.GridColCaption = ConfigQuery.GridColCaption
	'			result.GridColWidth = ConfigQuery.GridColWidth
	'			result.BackColor = ConfigQuery.BackColor
	'			result.ForeColor = ConfigQuery.ForeColor

	'			result.PopupFields = ConfigQuery.PopupFields
	'			result.PopupCaptions = ConfigQuery.PopupCaptions

	'			result.CountOfFieldsInHeader = CShort(ConfigQuery.CountOfFieldsInHeader)
	'			result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
	'			result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow
	'			result.IsUserProperty = False

	'		Catch ex As Exception
	'			m_Logger.LogError(ex.ToString)

	'		End Try

	'		Return result

	'	End Get

	'End Property

	'Private ReadOnly Property GetUserGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Get
	'		Dim result As New GridData
	'		If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'		Dim filename As String = m_MandantData.GetSelectedMDUserProfileXMLFilename(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

	'		m_path = New ClsProgPath
	'		If Not File.Exists(filename) Then
	'			m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Benutzer-Einsatellungsdatei für die Auflistung wurde nicht gefunden: {0}"), filename))

	'			Return Nothing
	'		End If

	'		Try
	'			Dim xDoc As XDocument = XDocument.Load(filename)
	'			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'							   Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'																And exportSetting.Attribute("ID").Value = "f6c163f6-3dab-4db8-b8dd-a7cd19b7017c"
	'							   Select New With {
	'																					.SQLQuery = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																					.GridColFieldName = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																					.DisplayMember = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																					.GridColCaption = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																					.GridColWidth = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																					.BackColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																					.ForeColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																					.PopupFields = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																					.PopupCaptions = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																					.CountOfFieldsInHeader = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																					.FieldsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																					.CaptionsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																						}).FirstOrDefault()


	'			If Not ConfigQuery Is Nothing Then
	'				result.SQLQuery = ConfigQuery.SQLQuery
	'				result.GridColFieldName = ConfigQuery.GridColFieldName
	'				result.DisplayMember = ConfigQuery.DisplayMember
	'				result.GridColCaption = ConfigQuery.GridColCaption
	'				result.GridColWidth = ConfigQuery.GridColWidth
	'				result.BackColor = ConfigQuery.BackColor
	'				result.ForeColor = ConfigQuery.ForeColor

	'				result.PopupFields = ConfigQuery.PopupFields
	'				result.PopupCaptions = ConfigQuery.PopupCaptions

	'				Dim scountofitems As Short = Short.TryParse(ConfigQuery.CountOfFieldsInHeader, scountofitems)
	'				result.CountOfFieldsInHeader = Math.Max(scountofitems, 3)
	'				result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow
	'				result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow

	'				result.IsUserProperty = True

	'			End If


	'		Catch ex As Exception
	'			m_Logger.LogError(ex.ToString)

	'		End Try

	'		Return result

	'	End Get

	'End Property


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

		Me.Parent.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue(MODUL_NAME))
		Try
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try
		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_HEADERDETAIL_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderDetail.SplitterPosition = Math.Max(Me.sccHeaderDetail.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)


		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperty.Width = Math.Max(Me.dpProperty.Width, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_PROP_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainProp_1.SplitterPosition = Math.Max(Me.sccMainProp_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_PROP_1_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainProp_1_1.SplitterPosition = Math.Max(Me.sccMainProp_1_1.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_NAV_2_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainNav_2.SplitterPosition = Math.Max(Me.sccMainNav_2.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_PROP_2_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainProp_2.SplitterPosition = Math.Max(Me.sccMainProp_2.SplitterPosition, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75

		Me.DockManager1.ActivePanel = dpProperties
		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.sccHeaderDetail.Dock = DockStyle.Fill

		Me.sccMainNav_1.Dock = DockStyle.Fill
		Me.sccMainProp_1.Dock = DockStyle.Fill
		Me.sccMainNav_2.Dock = DockStyle.Fill
		Me.sccMainProp_2.Dock = DockStyle.Fill

		Me.grdES.Dock = DockStyle.Fill
		Me.grdPropose.Dock = DockStyle.Fill
		Me.grdRP.Dock = DockStyle.Fill
		Me.grdZG.Dock = DockStyle.Fill
		Me.grdLO.Dock = DockStyle.Fill
		Me.grdContact.Dock = DockStyle.Fill

		Me.gvPropose.OptionsView.ShowIndicator = False
		Me.gvES.OptionsView.ShowIndicator = False
		Me.gvRP.OptionsView.ShowIndicator = False
		Me.gvZG.OptionsView.ShowIndicator = False
		Me.gvLO.OptionsView.ShowIndicator = False
		Me.gvContact.OptionsView.ShowIndicator = False

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True


		gvlProperty.OptionsView.ShowIndicator = False
		gvRProperty.OptionsView.ShowIndicator = False


		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderDetail.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler dpProperty.Resize, AddressOf SaveFormProperties

		AddHandler Me.sccMainProp_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_1_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_2.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_2.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler sccMainNav_1.SizeChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_HEADERDETAIL_SPLITTERPOSION, Me.sccHeaderDetail.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_DPPROPERTY_WIDTH, Me.dpProperty.Width)

		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_PROP_1_SPLITTERPOSION, Me.sccMainProp_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_PROP_1_1_SPLITTERPOSION, Me.sccMainProp_1_1.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_NAV_2_SPLITTERPOSION, Me.sccMainNav_2.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_MA_SCC_MAIN_PROP_2_SPLITTERPOSION, Me.sccMainProp_2.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub

	Private Sub frmKandidat_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedEmployeeList(m_ExistsFilterForBackupBeforeEQuest)

	End Sub


	Sub TranslateControls()

		Me.cmdNew.Text = m_Translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_Translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_Translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpES.Text = m_Translate.GetSafeTranslationValue(Me.grpES.Text)
		Me.grpPropose.Text = m_Translate.GetSafeTranslationValue(Me.grpPropose.Text)
		Me.grpRP.Text = m_Translate.GetSafeTranslationValue(Me.grpRP.Text)
		Me.grpZG.Text = m_Translate.GetSafeTranslationValue(Me.grpZG.Text)
		Me.grpLO.Text = m_Translate.GetSafeTranslationValue(Me.grpLO.Text)
		Me.grpContact.Text = m_Translate.GetSafeTranslationValue(Me.grpContact.Text)

		Me.dpProperties.Text = m_Translate.GetSafeTranslationValue(Me.dpProperties.Text)
		Me.dpSProperties.Text = m_Translate.GetSafeTranslationValue(Me.dpSProperties.Text)

	End Sub

	Public Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)

		Return

		gvMain.Columns.Clear()
		grdLProperty.DataSource = Nothing
		grdRProperty.DataSource = Nothing
		grdES.DataSource = Nothing
		grdPropose.DataSource = Nothing
		grdContact.DataSource = Nothing
		grdZG.DataSource = Nothing
		grdLO.DataSource = Nothing
		grdRP.DataSource = Nothing


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
				Dim columnCaption As String = m_Translate.GetSafeTranslationValue(aColCaption(i).Trim)
				Dim columnName As String = aColFieldName(i).ToLower.Trim

				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = columnCaption
				column.Name = columnName
				column.FieldName = columnName

				If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
					If columnName.ToLower = "magebdat" Then
						column.DisplayFormat.FormatString = "dd.MM.yyyy"
					Else
						column.DisplayFormat.FormatString = "G"
					End If
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
				If column.FieldName.ToLower.Contains("NoES".ToLower) Then column.ColumnEdit = m_CheckEditNotAllowed
				If column.FieldName.ToLower.Contains("WebExport".ToLower) Then column.ColumnEdit = m_CheckEditYes

				gvMain.Columns.Add(column)

			Next

			RestoreGridLayoutFromXml(gvMain.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMain.DataSource = Nothing

	End Sub

	Private Sub btnShowBackupDataBeforeEQest_Click(sender As Object, e As EventArgs) Handles btnShowBackupDataBeforeEQest.Click

		Dim btn As DevExpress.XtraEditors.SimpleButton = CType(sender, DevExpress.XtraEditors.SimpleButton)

		If btn.Tag = 0 Then
			btn.Image = ImageCollection1.Images("clearfilter_16x16.png")
			btn.Tag = 1

		Else
			btn.Image = ImageCollection1.Images("quickfilter_16x16.png")
			btn.Tag = 0

		End If
		m_ExistsFilterForBackupBeforeEQuest = btn.Tag = 1

		LoadFoundedEmployeeList(m_ExistsFilterForBackupBeforeEQuest)

	End Sub

	Public Function LoadFoundedEmployeeList(ByVal showBackupHistoryData As Boolean) As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees As IEnumerable(Of FoundedEmployeeData) = Nothing

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")
		m_Stopwatch.Reset()
		m_Stopwatch.Start()

		Try

			listOfEmployees = m_DataAccess.GetDbEmployeeData4Show(m_griddata.SQLQuery, showBackupHistoryData)
			Trace.WriteLine(String.Format("time for employee database: {0}", m_Stopwatch.ElapsedMilliseconds() / 1000))

			m_Stopwatch.Reset()
			m_Stopwatch.Start()

			If listOfEmployees Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog("Fehler in der Auflistung der Daten.")
				Return False
			End If
			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedEmployeeData With
						 {.manr = person.manr,
							._res = person._res,
							.mdnr = person.mdnr,
							.telefon_p = person.telefon_p,
							.natel = person.natel,
							.maname = person.maname,
							.strasse = person.strasse,
							.maplzort = person.maplzort,
							.magebdat = person.magebdat,
							.maalterwithdate = person.maalterwithdate,
							.mabewilligung = person.mabewilligung,
							.mastatus_1 = person.mastatus_1,
							.mastatus_2 = person.mastatus_2,
							.maqualifikation = person.maqualifikation,
							.maemail = person.maemail,
							.beruf = person.beruf,
							.tempmabild = person.tempmabild,
							.md_guid = person.md_guid,
							.actives = person.actives,
							.zfiliale = person.zfiliale,
							.beraterin = person.beraterin
						 }).ToList()

			'Dim listDataSource As BindingList(Of FoundedEmployeeData) = New BindingList(Of FoundedEmployeeData)

			'For Each p In responsiblePersonsGridData
			'	listDataSource.Add(p)
			'Next

			grdMain.DataSource = listOfEmployees
			RestoreGridLayoutFromXml(gvMain.Name.ToLower)

			Trace.WriteLine(String.Format("time for employee listing: {0}", m_Stopwatch.ElapsedMilliseconds() / 1000))

			RefreshMainViewStateBar()
			grpFunction.BackColor = If(String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), Nothing, System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName))

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		Return Not listOfEmployees Is Nothing
	End Function

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)


		pccMandant.HidePopup()
		If (e.Clicks = 2) Then
			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)

			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedEmployeeData)
				Me._ClsMASetting.SelectedMANr = viewData.manr
				Me._ClsMASetting.SelectedMDNr = viewData.mdnr
				m_CurrentEmployeeNumber = viewData.manr

				BuildNewPrintContextMenu()

				Select Case column.Name.ToLower
					Case "telefon_p"
						If viewData.telefon_p.Length > 0 Then
							Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsMA.TelefonCallToEmployee(viewData.telefon_p)
						End If

					Case "natel"
						If viewData.natel.Length > 0 Then
							Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsMA.TelefonCallToEmployee(viewData.natel)
						End If

					Case "maemail"
						If viewData.maemail.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
							_ClsKD.SendEMailToEmployee(viewData.maemail)
						End If


					Case Else
						If viewData.manr > 0 Then
							Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							_ClsMA.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

				End Select

			End If

		End If

	End Sub

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

	Private Sub gvMainOnMouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If e.Button = MouseButtons.Right Then
			ShowContextMenu(sender)
		End If

	End Sub

	Sub ShowContextMenu(sender As Object)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strPFieldName As String = m_griddata.PopupFields
		Dim strPFieldCaption As String = m_griddata.PopupCaptions

		Dim popupMenu1 As New DevExpress.XtraBars.PopupMenu
		Dim mouseposition As New Point(Cursor.Position.X, Cursor.Position.Y)
		Dim itm As New DevExpress.XtraBars.BarButtonItem

		popupMenu1.Manager = Me.BarManager1
		Dim i As Integer
		Dim selectedrow = SelectedRowViewData

		If Not selectedrow Is Nothing Then
			For Each Str As String In strPFieldCaption.Split(CChar(";"))
				Dim strColValue As String = ""
				Try
					Dim data As MenuData = GetCellValue(strPFieldName.Split(CChar(";"))(i).ToLower, selectedrow)
					If Not data Is Nothing Then
						strColValue = data.mnuvalue
					End If

				Catch ex As Exception
					m_Logger.LogWarning(String.Format("{0}.Spaltenwert ermitteln: {1}", strPFieldName.Split(CChar(";"))(i).ToLower, ex.Message))

				End Try

				itm = New DevExpress.XtraBars.BarButtonItem
				itm.Name = strPFieldName.Split(CChar(";"))(i)

				itm.Caption = String.Format("{0}{1}{2}",
																		m_Translate.GetSafeTranslationValue(Str.Replace("__", "")) &
																		If(itm.Name.Contains(CChar("@")), "", ":"), vbTab, strColValue)
				itm.AccessibleDescription = strColValue


				If itm.Name.Contains(CChar("@")) OrElse strColValue.Length > 0 Then
					popupMenu1.AddItem(itm).BeginGroup = Str.StartsWith("__")
					AddHandler itm.ItemClick, AddressOf GetMnuItem

					If strColValue <> String.Empty Then
					Else

					End If

				End If

				i += 1
			Next

		End If
		popupMenu1.ShowPopup(mouseposition)

	End Sub

	Sub GetMnuItem(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strMnuItem As String = e.Item.Name.ToString
		Dim strMnuValue As String = e.Item.AccessibleDescription

		Dim selectedrow = SelectedRowViewData

		If Not selectedrow Is Nothing Then
			If selectedrow.manr = 0 Then Exit Sub

		End If

		Select Case strMnuItem.ToUpper
			Case "MAName".ToUpper
				Me._ClsMASetting.SelectedMANr = selectedrow.manr
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMANr = Me._ClsMASetting.SelectedMANr})
				obj.OpenSelectedEmployeeContact()

			Case "telefon_p".ToUpper
				' TODO:
				If selectedrow.telefon_p.Length > 0 Then
					'Dim _ClsSystem As New Translate4_Net.ClsMain_Net
					'_ClsSystem.RunTapi(selectedrow.telefon_p, selectedrow.manr, 0, 0, 0, 0)
				End If

			Case "natel".ToUpper
				' TODO:
				If selectedrow.natel.Length > 0 Then
					'Dim _ClsSystem As New Translate4_Net.ClsMain_Net
					'_ClsSystem.RunTapi(selectedrow.natel, selectedrow.manr, 0, 0, 0, 0)
				End If

			Case "@MAProperties".ToUpper

				Try
					Dim frmProperty As New frmEmployeesProperties(m_InitializationData, m_Translate, selectedrow.manr, PictureBox1.Image)
					frmProperty.Show()
					frmProperty.BringToFront()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
					ShowErrDetail(ex.Message)

				End Try

		End Select

	End Sub

	Private Function GetCellValue(ByVal strCellName As String, ByVal selectedrow As FoundedEmployeeData) As MenuData

		Select Case strCellName.ToLower
			Case "beraterin"
				Return New MenuData With {.mnuvalue = selectedrow.beraterin}

			Case "maalterwithdate"
				Return New MenuData With {.mnuvalue = selectedrow.maalterwithdate}

			Case "mabewilligung"
				Return New MenuData With {.mnuvalue = selectedrow.mabewilligung}

			Case "maemail"
				Return New MenuData With {.mnuvalue = selectedrow.maemail}

			Case "magebdat"
				Return New MenuData With {.mnuvalue = selectedrow.magebdat}

			Case "maname"
				Return New MenuData With {.mnuvalue = selectedrow.maname}

			Case "manr"
				Return New MenuData With {.mnuvalue = selectedrow.manr}

			Case "maplzort"
				Return New MenuData With {.mnuvalue = selectedrow.maplzort}

			Case "maqualifikation"
				Return New MenuData With {.mnuvalue = selectedrow.maqualifikation}

			Case "mastatus_1"
				Return New MenuData With {.mnuvalue = selectedrow.mastatus_1}

			Case "mastatus_2"
				Return New MenuData With {.mnuvalue = selectedrow.mastatus_2}

			Case "natel"
				Return New MenuData With {.mnuvalue = selectedrow.natel}

			Case "strasse"
				Return New MenuData With {.mnuvalue = selectedrow.strasse}

			Case "telefon_p"
				Return New MenuData With {.mnuvalue = selectedrow.telefon_p}

			Case "zfiliale"
				Return New MenuData With {.mnuvalue = selectedrow.zfiliale}

			Case Else
				Return Nothing

		End Select


	End Function

	Private Sub FillPopupFields(ByVal selectedrow As FoundedEmployeeData) 'As Dictionary(Of String, String)
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
					Case "beraterin"
						strValue = selectedrow.beraterin
					Case "maalterwithdate"
						strValue = selectedrow.maalterwithdate
					Case "mabewilligung"
						strValue = selectedrow.mabewilligung
					Case "maemail"
						strValue = selectedrow.maemail
					Case "magebdat"
						strValue = selectedrow.magebdat
					Case "maname"
						strValue = selectedrow.maname
					Case "manr"
						strValue = selectedrow.manr
					Case "maplzort"
						strValue = selectedrow.maplzort
					Case "maqualifikation"
						strValue = selectedrow.maqualifikation
					Case "mastatus_1"
						strValue = selectedrow.mastatus_1
					Case "mastatus_2"
						strValue = selectedrow.mastatus_2
					Case "natel"
						strValue = selectedrow.natel
					Case "strasse"
						strValue = selectedrow.strasse
					Case "telefon_p"
						strValue = selectedrow.telefon_p
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

					If i > iCountofFieldInHeaderInfo Then
						itemName = aPoupCaption(i)
						itemValue = result.Item(aPoupFields(i))

						Dim propertyItem As New PropertyData With {.ValueName = m_Translate.GetSafeTranslationValue(itemName), .Value = itemValue}
						listOfPropertyRData.Add(propertyItem)

					Else
						itemName = aPoupCaption(i)
						itemValue = result.Item(aPoupFields(i))

						Dim propertyItem As New PropertyData With {.ValueName = m_Translate.GetSafeTranslationValue(itemName), .Value = itemValue}
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


	Private Sub OnGvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_Stopwatch.Reset()
		m_Stopwatch.Start()

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				Me._ClsMASetting.SelectedMANr = selectedrow.manr
				Me._ClsMASetting.SelectedMDNr = selectedrow.mdnr
				m_CurrentEmployeeNumber = selectedrow.manr

				If selectedrow.manr = 0 Then Return

				FillPopupFields(selectedrow)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(ex.Message)

		End Try

		Try
			FillProperties4Candidate()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Navigationsleiste: {0}", ex.ToString))
			ShowErrDetail(ex.Message)

		End Try
		Trace.WriteLine(String.Format("time for employee OnGvMain_FocusedRowChanged: {0}", m_Stopwatch.ElapsedMilliseconds() / 1000))


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

	Public Sub AutosizeImage(ByVal imgstream As Stream, ByVal picBox As PictureBox,
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
					g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy),
																						CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width,
																					imgOrg.Height, GraphicsUnit.Pixel)
					g.Dispose()
				Else
					imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
					imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
					g = Graphics.FromImage(imgShow)
					g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
					g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0,
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
		_ClsMAPropertiesSetting.gES = Me.grdES
		_ClsMAPropertiesSetting.grdLO = Me.grdLO
		_ClsMAPropertiesSetting.grdPropose = Me.grdPropose
		_ClsMAPropertiesSetting.grdRP = Me.grdRP
		_ClsMAPropertiesSetting.grdZG = Me.grdZG
		_ClsMAPropertiesSetting.grdContact = Me.grdContact
		_ClsMAPropertiesSetting.SelectedMANr = Me._ClsMASetting.SelectedMANr

		' employee ES
		ResetESDetailGrid()
		LoadEmployeeESDetailList()

		' employee proposal
		ResetProposalDetailGrid()
		LoadEmployeeProposalDetailList()

		' employee rp
		ResetRPDetailGrid()
		LoadEmployeeRPDetailList()

		' employee zg
		ResetZGDetailGrid()
		LoadEmployeeZGDetailList()


		' employee salary
		ResetSalaryDetailGrid()
		LoadEmployeeSalaryDetailList()

		' employee contact
		ResetContactDetailGrid()
		LoadEmployeeContactDetailList()

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
			columnmdnr.Caption = m_Translate.GetSafeTranslationValue("MDNr")
			columnmdnr.Name = "mdnr"
			columnmdnr.FieldName = "mdnr"
			columnmdnr.Visible = False
			gvES.Columns.Add(columnmdnr)

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "esnr"
			columnmodulname.FieldName = "esnr"
			columnmodulname.Visible = False
			gvES.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("KDNr")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvES.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvES.Columns.Add(columnZHDNr)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnMANr.Name = "periode"
			columnMANr.FieldName = "periode"
			columnMANr.Visible = False
			gvES.Columns.Add(columnMANr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Als")
			columnBezeichnung.Name = "esals"
			columnBezeichnung.FieldName = "esals"
			columnBezeichnung.Visible = True
			gvES.Columns.Add(columnBezeichnung)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvES.Columns.Add(columncustomername)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvES.Columns.Add(columnZFiliale)

			If File.Exists(m_GVESSettingfilename) Then gvES.RestoreLayoutFromXml(m_GVESSettingfilename)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdPropose.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeESDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbEmployeeESDataForProperties(Me._ClsMASetting.SelectedMANr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedEmployeeESDetailData With
					 {.mdnr = person.mdnr,
						.esnr = person.esnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.esals = person.esals,
						.customername = person.customername,
						.periode = person.periode,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedEmployeeESDetailData) = New BindingList(Of FoundedEmployeeESDetailData)

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
				Dim viewData = CType(dataRow, FoundedEmployeeESDetailData)

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

	Sub ResetProposalDetailGrid()
		Dim ColumnID As String = "EmployeeProposal"

		gvPropose.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvPropose.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvPropose.OptionsView.ShowGroupPanel = False
		gvPropose.OptionsView.ShowIndicator = False
		gvPropose.OptionsView.ShowAutoFilterRow = False

		gvPropose.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "pnr"
			columnmodulname.FieldName = "pnr"
			columnmodulname.Visible = False
			gvPropose.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("KDNr")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvPropose.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvPropose.Columns.Add(columnZHDNr)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_Translate.GetSafeTranslationValue("MANr")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvPropose.Columns.Add(columnMANr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.Name = "bezeichnung"
			columnBezeichnung.FieldName = "bezeichnung"
			columnBezeichnung.Visible = True
			gvPropose.Columns.Add(columnBezeichnung)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvPropose.Columns.Add(columncustomername)

			Dim columnZname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZname.Caption = m_Translate.GetSafeTranslationValue("ZHD.-Person")
			columnZname.Name = "zhdname"
			columnZname.FieldName = "zhdname"
			columnZname.Visible = False
			gvPropose.Columns.Add(columnZname)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvPropose.Columns.Add(columnEmployeename)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_Translate.GetSafeTranslationValue("Art")
			columndestesnr.Name = "p_art"
			columndestesnr.FieldName = "p_art"
			columndestesnr.Visible = False
			gvPropose.Columns.Add(columndestesnr)

			Dim columnlonr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlonr.Caption = m_Translate.GetSafeTranslationValue("Status")
			columnlonr.Name = "p_state"
			columnlonr.FieldName = "p_state"
			columnlonr.Visible = True
			gvPropose.Columns.Add(columnlonr)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvPropose.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvPropose.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			columnZFiliale.BestFit()
			gvPropose.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml(gvPropose.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdPropose.DataSource = Nothing

	End Sub


	Public Function LoadEmployeeProposalDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbEmployeeProposalDataForProperties(Me._ClsMASetting.SelectedMANr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Vorschlag-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedEmployeeProposalDetailData With
					 {.mdnr = person.mdnr,
						.pnr = person.pnr,
						.kdnr = person.kdnr,
						.zhdnr = person.zhdnr,
						.manr = person.manr,
						.bezeichnung = person.bezeichnung,
						.employeename = person.employeename,
						.customername = person.customername,
						.zhdname = person.zhdname,
						.p_art = person.p_art,
						.p_state = person.p_state,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedEmployeeProposalDetailData) = New BindingList(Of FoundedEmployeeProposalDetailData)

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
				Dim viewData = CType(dataRow, FoundedEmployeeProposalDetailData)

				If viewData.pnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedProposeNr = viewData.pnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr, .SelectedMANr = viewData.manr})
					_ClsKD.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub


#End Region


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
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "rpnr"
			columnmodulname.FieldName = "rpnr"
			columnmodulname.Visible = True
			gvRP.Columns.Add(columnmodulname)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvRP.Columns.Add(columnBezeichnung)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvRP.Columns.Add(columncustomername)

			Dim columnzfiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnzfiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnzfiliale.Name = "zfiliale"
			columnzfiliale.FieldName = "zfiliale"
			columnzfiliale.Visible = False
			gvRP.Columns.Add(columnzfiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvRP.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvRP.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml(gvRP.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdRP.DataSource = Nothing

	End Sub


	Public Function LoadEmployeeRPDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbEmployeeReportDataForProperties(Me._ClsMASetting.SelectedMANr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedEmployeeReportDetailData With
					 {.mdnr = person.mdnr,
						.rpnr = person.rpnr,
						.manr = person.manr,
						.kdnr = person.kdnr,
						.periode = person.periode,
						.customername = person.customername,
						.createdfrom = person.createdfrom,
						.createdon = person.createdon,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedEmployeeReportDetailData) = New BindingList(Of FoundedEmployeeReportDetailData)

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
				Dim viewData = CType(dataRow, FoundedEmployeeReportDetailData)

				If viewData.rpnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub

#End Region


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
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zgnr"
			columnmodulname.FieldName = "zgnr"
			columnmodulname.Visible = True
			gvZG.Columns.Add(columnmodulname)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_Translate.GetSafeTranslationValue("Auszahlungsart")
			columnLAName.Name = "laname"
			columnLAName.FieldName = "laname"
			columnLAName.Visible = True
			gvZG.Columns.Add(columnLAName)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "betrag"
			columnBetrag.FieldName = "betrag"
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvZG.Columns.Add(columnBetrag)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Zeitperiode")
			columnBezeichnung.Name = "zgperiode"
			columnBezeichnung.FieldName = "zgperiode"
			columnBezeichnung.Visible = True
			gvZG.Columns.Add(columnBezeichnung)

			Dim columnAusDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAusDat.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnAusDat.Name = "aus_dat"
			columnAusDat.FieldName = "aus_dat"
			columnAusDat.Visible = False
			gvZG.Columns.Add(columnAusDat)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columncustomername.Name = "zfiliale"
			columncustomername.FieldName = "zfiliale"
			columncustomername.Visible = False
			gvZG.Columns.Add(columncustomername)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvZG.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
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
		Dim listOfEmployees = m_DataAccess.GetDbEmployeeZGDataForProperties(Me._ClsMASetting.SelectedMANr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Vorschuss-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedEmployeeZGDetailData With
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

		Dim listDataSource As BindingList(Of FoundedEmployeeZGDetailData) = New BindingList(Of FoundedEmployeeZGDetailData)

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
				Dim viewData = CType(dataRow, FoundedEmployeeZGDetailData)

				If viewData.zgnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZGNr = viewData.zgnr, .SelectedMANr = Me._ClsMASetting.SelectedMANr})
					_ClsKD.OpenSelectedAdvancePayment(viewData.mdnr, ModulConstants.UserData.UserNr)
					'_ClsKD.OpenSelectedZG()
				End If

			End If

		End If

	End Sub

#End Region


#Region "LO Funktionen..."

	Sub ResetSalaryDetailGrid()

		gvLO.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvLO.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvLO.OptionsView.ShowGroupPanel = False
		gvLO.OptionsView.ShowIndicator = False
		gvLO.OptionsView.ShowAutoFilterRow = False

		gvLO.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "lonr"
			columnmodulname.FieldName = "lonr"
			columnmodulname.Visible = True
			gvLO.Columns.Add(columnmodulname)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvLO.Columns.Add(columnBezeichnung)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columncustomername.Name = "zfiliale"
			columncustomername.FieldName = "zfiliale"
			columncustomername.Visible = False
			gvLO.Columns.Add(columncustomername)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvLO.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvLO.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml(gvLO.Name.ToLower)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdLO.DataSource = Nothing

	End Sub


	Public Function LoadEmployeeSalaryDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbEmployeeSalaryDataForProperties(Me._ClsMASetting.SelectedMANr)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedEmployeeSalaryDetailData With
					 {.mdnr = person.mdnr,
						.lonr = person.lonr,
						.periode = person.periode,
						.createdfrom = person.createdfrom,
						.createdon = person.createdon,
						.zfiliale = person.zfiliale
					 }).ToList()

		Dim listDataSource As BindingList(Of FoundedEmployeeSalaryDetailData) = New BindingList(Of FoundedEmployeeSalaryDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdLO.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvSalary_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvLO.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedEmployeeSalaryDetailData)

				If viewData.lonr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedLONr = viewData.lonr, .SelectedMANr = _ClsMASetting.SelectedMANr})
					_ClsKD.OpenSelectedLO()
				End If

			End If

		End If

	End Sub

#End Region


#Region "Contact Funktionen..."

	Sub ResetContactDetailGrid()
		Dim ColumnID As String = "EmployeeContact"

		gvContact.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvContact.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvContact.OptionsView.ShowGroupPanel = False
		gvContact.OptionsView.ShowIndicator = False
		gvContact.OptionsView.ShowAutoFilterRow = True

		gvContact.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "contactnr"
			columnmodulname.FieldName = "contactnr"
			columnmodulname.Visible = False
			gvContact.Columns.Add(columnmodulname)

			Dim columndatum As New DevExpress.XtraGrid.Columns.GridColumn()
			columndatum.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columndatum.Name = "datum"
			columndatum.FieldName = "datum"
			columndatum.Visible = True
			gvContact.Columns.Add(columndatum)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Name = "bezeichnung"
			columnBezeichnung.FieldName = "bezeichnung"
			columnBezeichnung.Visible = True
			gvContact.Columns.Add(columnBezeichnung)

			Dim columnBeschreibung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBeschreibung.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
			columnBeschreibung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBeschreibung.Name = "beschreibung"
			columnBeschreibung.FieldName = "beschreibung"
			columnBeschreibung.Visible = False
			gvContact.Columns.Add(columnBeschreibung)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_Translate.GetSafeTranslationValue("Art")
			columndestesnr.Name = "art"
			columndestesnr.FieldName = "art"
			columndestesnr.Visible = True
			gvContact.Columns.Add(columndestesnr)

			Dim columnlonr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlonr.Caption = m_Translate.GetSafeTranslationValue("KST")
			columnlonr.Name = "kst"
			columnlonr.FieldName = "kst"
			columnlonr.Visible = True
			gvContact.Columns.Add(columnlonr)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvContact.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvContact.Columns.Add(columnCreatedFrom)

			grdContact.AllowDrop = True

			RestoreGridLayoutFromXml(gvContact.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdContact.DataSource = Nothing

	End Sub

	Private Function LoadEmployeeContactDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_ModulViewDatabaseAccess.LoadAssignedEmployeeContactData(ModulConstants.MDData.MDNr, _ClsMASetting.SelectedMANr, Nothing, Nothing, Nothing, True, String.Empty)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Kontakt-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New ModulViewEmployeeContactData With
					 {.contactnr = person.contactnr,
						.manr = person.manr,
						.bezeichnung = person.bezeichnung,
						.beschreibung = person.beschreibung,
						.datum = person.datum,
						.art = person.art,
						.createdon = person.createdon,
						.createdfrom = person.createdfrom
					 }).ToList()

		Dim listDataSource As BindingList(Of ModulViewEmployeeContactData) = New BindingList(Of ModulViewEmployeeContactData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdContact.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	''' <summary>
	''' Handles the form dragdrop event.
	''' </summary>
	Private Sub OngrdContact_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdContact.DragDrop
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

		Dim m_data As DataObject = New DataObject()
		Dim title As String = String.Empty
		Dim description As String = String.Empty
		Dim contactType As String = String.Empty
		Dim contactDate As Date = Now.Date
		Dim contactTime As DateTime = Now
		Dim attachedFile As String = String.Empty

		If (Clipboard.GetDataObject().GetDataPresent(DataFormats.Bitmap)) Then Trace.WriteLine("Bitmap")
		If (Clipboard.GetDataObject().GetDataPresent(DataFormats.FileDrop)) Then Trace.WriteLine("fileDrop")

		If e.Data.GetDataPresent("FileGroupDescriptor") Then
			'supports a drop of a Outlook message 
			Dim m_Path As New SPProgUtility.ClsProgSettingPath
			Dim objOL As Object = Nothing
			objOL = CreateObject("Outlook.Application")
			Dim myobj As Object

			For i As Integer = 1 To objOL.ActiveExplorer.Selection.Count
				myobj = objOL.ActiveExplorer.Selection.Item(i)

				'hardcode a destination path for testing
				Dim strFilename As String = myobj.Subject
				Try
					contactType = "Einzelmail"
					title = strFilename
					description = myobj.body
					contactDate = CType(Format(myobj.CreationTime, "d"), Date)
					contactTime = CType(Format(myobj.CreationTime, "t"), DateTime)

				Catch ex As Exception

				End Try

				strFilename = System.Text.RegularExpressions.Regex.Replace(myobj.Subject, "[\\/:*?""<>|\r\n]", "", System.Text.RegularExpressions.RegexOptions.Singleline)
				strFilename &= ".msg"
				Dim strFile As String = IO.Path.Combine(m_Path.GetSpS2DeleteHomeFolder, strFilename)

				myobj.SaveAs(strFile)
				files = New String() {strFile}
			Next

		Else

		End If

		If Not files Is Nothing AndAlso files.Count > 0 Then
			Dim fileInfo As New FileInfo(files(0))
			attachedFile = fileInfo.FullName

			SaveEmployeeContactData(title, description, contactType, contactDate, contactTime, attachedFile)
		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OngrdContact_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdContact.DragEnter
		e.Effect = DragDropEffects.Copy
	End Sub

	Sub OngvContact_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvContact.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, ModulViewEmployeeContactData)

				If viewData.contactnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.ContactRecordNumber = viewData.contactnr, .SelectedMANr = viewData.manr})
					_ClsKD.OpenSelectedEmployeeContact()
				End If

			End If

		End If

	End Sub

	Private Sub SaveEmployeeContactData(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String)

		Dim currentContactRecordNumber As Integer = 0
		Dim currentDocumentID As Integer = 0
		Dim contactData As EmployeeContactData = Nothing
		Dim fileContent = m_Utility.LoadFileBytes(attachedFile)

		Dim dt = DateTime.Now
		contactData = New EmployeeContactData With {.EmployeeNumber = m_CurrentEmployeeNumber,
																																		 .CreatedOn = dt,
																																		 .CreatedFrom = ModulConstants.UserData.UserFullName}

		contactData.EmployeeNumber = m_CurrentEmployeeNumber
		contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
		contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
		contactData.ContactPeriodString = title
		contactData.ContactsString = description
		contactData.ContactImportant = False
		contactData.ContactFinished = False
		contactData.VacancyNumber = Nothing
		contactData.ProposeNr = Nothing
		contactData.ESNr = Nothing
		contactData.CustomerNumber = Nothing

		contactData.ChangedFrom = ModulConstants.UserData.UserFullName
		contactData.ChangedOn = dt
		contactData.UsNr = ModulConstants.UserData.UserNr

		Dim success As Boolean = True

		' Check if the document bytes must be saved.
		If Not (attachedFile Is Nothing) And success Then

			Dim contactDocument As ContactDoc = Nothing


			contactDocument = New ContactDoc() With {.CreatedOn = dt,
																									 .CreatedFrom = ModulConstants.UserData.UserFullName,
																									 .FileBytes = fileContent,
																									 .FileExtension = Path.GetExtension(attachedFile)}
			success = success AndAlso m_EmployeeDatabaseAccess.AddContactDocument(contactDocument)

			If success Then
				currentDocumentID = contactDocument.ID
				contactData.KontaktDocID = currentDocumentID
			End If

		End If

		' Insert contact
		contactData.CreatedUserNumber = ModulConstants.UserData.UserNr
		success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

		If success Then
			currentContactRecordNumber = contactData.RecordNumber
		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Kontaktdaten konnten nicht gespeichert werden."))
		Else

			LoadEmployeeContactDetailList()

			Dim kontakte = New SP.MA.KontaktMng.frmContacts(m_InitializationData)
			If (kontakte.ActivateNewContactDataMode(m_CurrentEmployeeNumber, Nothing, Nothing)) Then
				kontakte.LoadContactData(m_CurrentEmployeeNumber, currentContactRecordNumber, Nothing)

				kontakte.Show()
				kontakte.BringToFront()
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
				LoadFoundedEmployeeList(m_ExistsFilterForBackupBeforeEQuest)
			End If
		End If

		Me.grdMain.Enabled = Not (cboMD.EditValue Is Nothing)
		pccMandant.HidePopup()

	End Sub


#End Region



	''' <summary>
	''' Klick auf einzelne Detailbuttons für die Anzeige der Daten
	''' </summary>
	Private Sub OngrpES_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpES.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_ClsMASetting.EmployeeName = data.maname
			Me._ClsMASetting.Data4SelectedMA = True
			strModul2Open = "MAES".ToLower

			Dim frm As New frmMADetails(Me._ClsMASetting, strModul2Open)
			frm.Show()

		End If

	End Sub

	Private Sub OngrpPropose_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpPropose.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_ClsMASetting.EmployeeName = data.maname
			Me._ClsMASetting.Data4SelectedMA = True
			strModul2Open = "MAPropose".ToLower

			Dim frm As New frmMADetails(Me._ClsMASetting, strModul2Open)
			frm.Show()

		End If

	End Sub

	Private Sub OngrpContact_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpContact.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_ClsMASetting.EmployeeName = data.maname
			Me._ClsMASetting.Data4SelectedMA = True
			strModul2Open = "macontact".ToLower

			Dim frm As New frmMADetails(Me._ClsMASetting, strModul2Open)
			frm.LoadData()

			frm.Show()
			frm.BringToFront()

		End If

	End Sub


	Private Sub OngrpZG_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpZG.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_ClsMASetting.EmployeeName = data.maname
			Me._ClsMASetting.Data4SelectedMA = True
			strModul2Open = "MAZG".ToLower

			Dim frm As New frmMADetails(Me._ClsMASetting, strModul2Open)
			frm.Show()

		End If

	End Sub

	Private Sub OngrpLO_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpLO.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_ClsMASetting.EmployeeName = data.maname
			Me._ClsMASetting.Data4SelectedMA = True
			strModul2Open = "MALO".ToLower

			Dim frm As New frmMADetails(Me._ClsMASetting, strModul2Open)
			frm.Show()

		End If

	End Sub

	Private Sub OngrpRP_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpRP.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_ClsMASetting.EmployeeName = data.maname
			Me._ClsMASetting.Data4SelectedMA = True
			strModul2Open = "MARP".ToLower

			Dim frm As New frmMADetails(Me._ClsMASetting, strModul2Open)
			frm.Show()

		End If

	End Sub


#End Region


#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()

		Try
			' build contextmenu
			Dim _ClsNewPrint As New ClsMAModuls(Me._ClsMASetting)
			_ClsNewPrint.ShowContextMenu4Print(BarManager1, Me.cmdPrint)
			_ClsNewPrint.ShowContextMenu4New(cmdNew)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = TranslateText(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub


#Region "GridSettings"

	Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case GridName.ToLower
			Case "gvmain".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingMainFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreMainSetting, False), True)

				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVMainSettingfilename) Then gvMain.RestoreLayoutFromXml(m_GVMainSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvMain.ActiveFilterCriteria = Nothing

			Case "gves".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingESFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreESSetting, False), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVESSettingfilename) Then gvES.RestoreLayoutFromXml(m_GVESSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvES.ActiveFilterCriteria = Nothing

			Case "gvpropose".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingProposeFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreProposeSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVProposeSettingfilename) Then gvPropose.RestoreLayoutFromXml(m_GVProposeSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvPropose.ActiveFilterCriteria = Nothing


			Case "gvcontact".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingContactFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreContactSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVContactSettingfilename) Then gvContact.RestoreLayoutFromXml(m_GVContactSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvContact.ActiveFilterCriteria = Nothing



			Case "gvzg".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingZGFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreZGSetting, False), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVZGSettingfilename) Then gvZG.RestoreLayoutFromXml(m_GVZGSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvZG.ActiveFilterCriteria = Nothing


			Case "gvlo".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingLOFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreLOSetting, False), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVSalarySettingfilename) Then gvLO.RestoreLayoutFromXml(m_GVSalarySettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvLO.ActiveFilterCriteria = Nothing

			Case "gvRP".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRPFilter, False), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRPSetting, False), True)
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

	Private Sub OngvESColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvES.SaveLayoutToXml(m_GVESSettingfilename)

	End Sub

	Private Sub OngvProposeColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvPropose.SaveLayoutToXml(m_GVProposeSettingfilename)

	End Sub

	Private Sub OngvContactColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvContact.SaveLayoutToXml(m_GVContactSettingfilename)

	End Sub

	Private Sub OngvSalaryColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvLO.SaveLayoutToXml(m_GVSalarySettingfilename)

	End Sub

	Private Sub OngvReportColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVReportSettingfilename)

	End Sub

	Private Sub OngvZGColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvZG.SaveLayoutToXml(m_GVZGSettingfilename)

	End Sub



#End Region


#End Region


	Private Function LoadFoundedMDList() As Boolean
		cboMD.Properties.DataSource = ListOfMandantData

		Return Not ListOfMandantData Is Nothing
	End Function

	''' <summary>
	''' Zeigt pcc-Container für Mandantenauswahl
	''' </summary>
	Private Sub OngrpFunction_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpFunction.CustomButtonClick

		Select Case e.Button.Properties.GroupIndex
			Case 0
				RefreshData()

			Case 1

				pccMandant.SuspendLayout()
				pccMandant.Manager = New DevExpress.XtraBars.BarManager
				pccMandant.ShowCloseButton = True
				pccMandant.ShowSizeGrip = True

				pccMandant.ShowPopup(Cursor.Position)
				pccMandant.ResumeLayout()

			Case 2
				Dim frmQSearch = New frmEmployeeQuickSearch(m_InitializationData)
				frmQSearch.LoadData()

				frmQSearch.Show()
				frmQSearch.BringToFront()

			Case Else
				Return

		End Select

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
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																						ModulConstants.ProsonalizedData,
																																						ModulConstants.MDData,
																																						ModulConstants.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		'LoadXMLDataForSelectedModule()
		ResetMainGrid()
		LoadFoundedEmployeeList(m_ExistsFilterForBackupBeforeEQuest)

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub RefreshMainViewStateBar()

		m_communicationHub.Publish(New RefreshMainViewStatebar(Me, Me.gvMain.RowCount, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
		cmdNew.Enabled = ModulConstants.MDData.ClosedMD = 0

	End Sub


#Region "helpers"

	''' <summary>
	''' Combines date and time.
	''' </summary>
	''' <param name="dateComponent">The date component.</param>
	''' <param name="timeComponent">The time component (date is ignored)</param>
	''' <returns>Combined date and time</returns>
	Private Function CombineDateAndTime(ByVal dateComponent As DateTime?, ByVal timeComponent As DateTime?) As DateTime?

		If Not dateComponent.HasValue Then
			Return Nothing
		End If

		If Not timeComponent.HasValue Then
			Return dateComponent.Value.Date
		End If

		Dim timeSpan As TimeSpan = timeComponent.Value - timeComponent.Value.Date
		Dim dateAndTime = dateComponent.Value.Date.Add(timeSpan)

		Return dateAndTime
	End Function



#End Region


End Class

