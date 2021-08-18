
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
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ModulView
Imports SP.DatabaseAccess.ModulView.DataObjects

Public Class pageKunden


#Region "private consts"

	Private Const MODUL_NAME = "Kundenverwaltung"
	Private Const MODUL_NAME_SETTING = "customer"

	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Kundenverwaltung"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/es/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/es/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_VACANCY_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/vacancy/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_VACANCY_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/vacancy/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/propose/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/propose/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/contact/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/contact/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/re/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/re/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/ze/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/ze/keepfilter"

	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/rp/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER As String = "gridsetting/User_{0}/mainview/{1}/customerproperties/rp/keepfilter"

	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NR As String = "MD_{0}/Sonstiges/autofilterconditionnr"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE As String = "MD_{0}/Sonstiges/autofilterconditiondate"

	Private Const DATE_COLUMN_NAME As String = "magetdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;ourisonline;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline;noes"

#End Region


#Region "Private Fields"

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The modulview database access.
	''' </summary>
	Protected m_ModulViewDatabaseAccess As IModulViewDatabaseAccess

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The customer data access object.
	''' </summary>
	Private m_CustomerDataAccess As ICustomerDatabaseAccess

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


	Private m_GVMainSettingfilename As String
	Private m_GVESSettingfilename As String
	Private m_GVvacancySettingfilename As String
	Private m_GVProposeSettingfilename As String
	Private m_GVContactSettingfilename As String
	Private m_GVInvoiceSettingfilename As String
	Private m_GVZESettingfilename As String
	Private m_GVReportSettingfilename As String


	Private aColCaption As String()
	Private aColFieldName As String()
	Private aColWidth As String()

	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml


	Private m_MandantData As Mandant
	Private m_utilitySP As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath


	Private m_griddata As GridData

	Private _KDSetting As ClsKDSetting

	Private Property LoadedMDNr As Integer

	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_xmlSettingESFilter As String
	Private m_xmlSettingRestoreESSetting As String

	Private m_xmlSettingVacancyFilter As String
	Private m_xmlSettingRestoreVacancySetting As String

	Private m_xmlSettingProposeFilter As String
	Private m_xmlSettingRestoreProposeSetting As String

	Private m_xmlSettingContactFilter As String
	Private m_xmlSettingRestoreContactSetting As String

	Private m_xmlSettingREFilter As String
	Private m_xmlSettingRestoreRESetting As String

	Private m_xmlSettingZEFilter As String
	Private m_xmlSettingRestoreZESetting As String

	Private m_xmlSettingRPFilter As String
	Private m_xmlSettingRestoreRPSetting As String

	Private m_VacancyPropse As VacancyProposeEnun

	Private m_ShowcustomerinColor As Boolean

	Private m_SuppressUIEvents As Boolean

	Private m_CurrentCustomerNumber As Integer?
	Private m_CurrentResponsiblePersonNumber As Integer?
	Private m_CurrentProposeNumber As Integer?
	Private m_CurrentVacancyNumber As Integer?
	Private m_AllowedChangeMandant As Boolean

	Private m_CheckEditCompleted As RepositoryItemCheckEdit
	Private m_CheckEditExpire As RepositoryItemCheckEdit
	Private m_CheckEditWarning As RepositoryItemCheckEdit
	Private m_CheckEditNotAllowed As RepositoryItemCheckEdit

	Private m_MainViewGridData As SuportedCodes

	Enum VacancyProposeEnun
		VACANCY
		PROPOSE
	End Enum

#End Region



#Region "Private property"

	Private ReadOnly Property ShowCustomerRecordsInColor() As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim mdNumber As Integer = 0
			Dim userNumber As Integer = 0
			mdNumber = ModulConstants.MDData.MDNr
			userNumber = ModulConstants.UserData.UserNr

			Dim UserXMLFileName = m_MandantData.GetSelectedMDUserProfileXMLFilename(mdNumber, userNumber)
			Dim value As Boolean? = StrToBool(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/showcustomerrecordsincolor", FORM_XML_MAIN_KEY)))


			Return value
		End Get

	End Property

	Private ReadOnly Property SelectedRowViewData As FoundedCustomerData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedCustomerData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected vacancy data.
	''' </summary>
	Private ReadOnly Property SelectedVacancyViewData As CustomerVacanciesProperty
		Get
			Dim grdView = TryCast(grdPropose.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), CustomerVacanciesProperty)
					Return viewData
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected propose data.
	''' </summary>
	Private ReadOnly Property SelectedProposeViewData As FoundedCustomerProposalDetailData
		Get
			Dim grdView = TryCast(grdPropose.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim viewData = CType(grdView.GetRow(selectedRows(0)), FoundedCustomerProposalDetailData)
					Return viewData
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

		m_SuppressUIEvents = True

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_CustomerDataAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)


		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try

			m_communicationHub = MessageService.Instance.Hub

			BarManager1.Form = Me
			LoadedMDNr = ModulConstants.MDData.MDNr
			Me._KDSetting = New ClsKDSetting
			m_SettingsManager = New SettingsESManager

			m_MandantData = New Mandant
			m_utilitySP = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_Utility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI

			dpProperties.Options.ShowCloseButton = False
			dpSProperties.Options.ShowCloseButton = False


			Try
				m_GVMainSettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvMain.Name, ModulConstants.UserData.UserNr)
				m_GVESSettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvES.Name, ModulConstants.UserData.UserNr)
				m_GVvacancySettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvPropose.Name, ModulConstants.UserData.UserNr)
				m_GVProposeSettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvPropose.Name, ModulConstants.UserData.UserNr)
				m_GVContactSettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvContact.Name, ModulConstants.UserData.UserNr)
				m_GVInvoiceSettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvInvoice.Name, ModulConstants.UserData.UserNr)
				m_GVZESettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvZE.Name, ModulConstants.UserData.UserNr)
				m_GVReportSettingfilename = String.Format("{0}Customer\{1}{2}.xml", ModulConstants.GridSettingPath, gvRP.Name, ModulConstants.UserData.UserNr)


				m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreESSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingESFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ES_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreVacancySetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_VACANCY_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingVacancyFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_VACANCY_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreProposeSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingProposeFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_PROPOSE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreContactSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingContactFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_CONTACT_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreRESetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingREFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreZESetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingZEFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_ZE_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_xmlSettingRestoreRPSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingRPFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_PROPERTIES_RP_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

			m_CheckEditCompleted = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditCompleted.PictureChecked = My.Resources.bullet_ball_green
			m_CheckEditCompleted.PictureUnchecked = My.Resources.bullet_ball_red
			m_CheckEditCompleted.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

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

			AddHandler Me.gvMain.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler Me.gvMain.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
			AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged


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


			AddHandler Me.gvInvoice.RowCellClick, AddressOf OngvInvoice_RowCellClick
			AddHandler Me.gvInvoice.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvInvoice.ColumnPositionChanged, AddressOf OngvInvoiceColumnPositionChanged
			AddHandler Me.gvInvoice.ColumnWidthChanged, AddressOf OngvInvoiceColumnPositionChanged


			AddHandler Me.gvZE.RowCellClick, AddressOf OngvZE_RowCellClick
			AddHandler Me.gvZE.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvZE.ColumnPositionChanged, AddressOf OngvZEColumnPositionChanged
			AddHandler Me.gvZE.ColumnWidthChanged, AddressOf OngvZEColumnPositionChanged

			AddHandler Me.gvContact.RowCellClick, AddressOf OngvContact_RowCellClick
			AddHandler Me.gvContact.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler Me.gvContact.ColumnPositionChanged, AddressOf OngvContactColumnPositionChanged
			AddHandler Me.gvContact.ColumnWidthChanged, AddressOf OngvContactColumnPositionChanged

			m_VacancyPropse = VacancyProposeEnun.VACANCY
			grpPropose.Text = m_Translate.GetSafeTranslationValue("Vakanzen")

			m_ShowcustomerinColor = ShowCustomerRecordsInColor
			m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200001)

			m_MainViewGridData = New SuportedCodes
			m_MainViewGridData.ChangeColumnNamesToLowercase = True
			m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
			m_griddata = m_MainViewGridData.LoadMainGridData

			ResetMandantenDropDown()
			LoadFoundedMDList()
			cboMD.EditValue = ModulConstants.MDData.MDNr

			'LoadXMLDataForSelectedModule()
			ResetGrid()

			m_SuppressUIEvents = False


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

#End Region


#Region "Private methods"

	'Sub LoadXMLDataForSelectedModule()

	'	m_griddata = GetUserGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	If m_griddata.SQLQuery Is Nothing Then
	'		m_griddata = GetGridPropertiesFromXML(ModulConstants.MDData.MDNr)
	'	End If

	'	aColCaption = m_griddata.GridColCaption.Split(CChar(";"))
	'	aColFieldName = m_griddata.GridColFieldName.Split(CChar(";"))
	'	aColWidth = m_griddata.GridColWidth.Split(CChar(";"))

	'End Sub

	Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs)

		m_ShowcustomerinColor = ShowCustomerRecordsInColor
		'LoadXMLDataForSelectedModule()
		ResetGrid()
		LoadFoundedCustomerList()

	End Sub



	'Private Function GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_MandantData.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'											 Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'													 And exportSetting.Attribute("ID").Value = MODUL_NAME
	'											 Select New With {
	'																 .SQLQuery = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																 .GridColFieldName = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																 .DisplayMember = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																 .GridColCaption = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																 .GridColWidth = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																 .BackColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																 .ForeColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																 .PopupFields = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																 .PopupCaptions = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																 .CountOfFieldsInHeader = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																 .FieldsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																 .CaptionsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																	 }).FirstOrDefault()



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
	'											 Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'													 And exportSetting.Attribute("ID").Value = MODUL_NAME
	'											 Select New With {
	'																 .SQLQuery = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																 .GridColFieldName = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																 .DisplayMember = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																 .GridColCaption = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																 .GridColWidth = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																 .BackColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																 .ForeColor = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																 .PopupFields = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																 .PopupCaptions = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																 .CountOfFieldsInHeader = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																 .FieldsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																 .CaptionsInHeaderToShow = m_utilitySP.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
	'																	 }).FirstOrDefault()


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

		Me.Parent.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue(UsercontrolCaption))
		Try
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Me.DockManager1.ActivePanel = dpProperties
		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMainProp_1.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill
		Me.scHeaderDetail1.Dock = DockStyle.Fill
		Me.scHeaderDetail2.Dock = DockStyle.Fill

		Me.gvRProperty.OptionsView.ShowIndicator = False
		Me.gvLProperty.OptionsView.ShowIndicator = False

		Me.gvMain.OptionsView.ShowIndicator = False
		Me.gvPropose.OptionsView.ShowIndicator = False
		Me.gvES.OptionsView.ShowIndicator = False
		Me.gvRP.OptionsView.ShowIndicator = False
		Me.gvInvoice.OptionsView.ShowIndicator = False
		Me.gvZE.OptionsView.ShowIndicator = False
		Me.gvContact.OptionsView.ShowIndicator = False

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties

		AddHandler dpProperty.Resize, AddressOf SaveFormProperties

		AddHandler Me.sccMainProp_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_1_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_2.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainProp_2_1.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccMainNav_1.SizeChanged, AddressOf SaveFormProperties


	End Sub

#End Region


	Private Sub ResetGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)

		Return

		Dim ColumnID As String = "CustomerMainGrid"

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		gvMain.Columns.Clear()

		grdLProperty.DataSource = Nothing
		grdRProperty.DataSource = Nothing
		grdES.DataSource = Nothing
		grdPropose.DataSource = Nothing
		grdContact.DataSource = Nothing
		grdInvoice.DataSource = Nothing
		grdZE.DataSource = Nothing
		grdRP.DataSource = Nothing

		Dim filter As CriteriaOperator
		filter = gvMain.ActiveFilterCriteria


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
				If column.FieldName.ToLower.Contains("NoES".ToLower) Then column.ColumnEdit = m_CheckEditNotAllowed

				gvMain.Columns.Add(column)

			Next

			RestoreGridLayoutFromXml(gvMain.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdMain.DataSource = Nothing

	End Sub


	Private Function LoadFoundedCustomerList() As Boolean
		Dim m_DataAccess As New MainGrid

		Dim listOfFoundedData = m_DataAccess.GetDbCustomerData4Show(m_griddata.SQLQuery)
		If listOfFoundedData Is Nothing Then
			SplashScreenManager.CloseForm(False)
			ShowErrDetail(m_Translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Exit Function
		End If

		Dim responsiblePersonsGridData = (From person In listOfFoundedData
										  Select New FoundedCustomerData With
																						 {.kdnr = person.kdnr,
																							._res = person._res,
																							.mdnr = person.mdnr,
																							.firma1 = person.firma1,
																							.fproperty = person.fproperty,
																							.howkontakt = person.howkontakt,
																							.kdstate1 = person.kdstate1,
																							.kdstate2 = person.kdstate2,
																							.kdberater = person.kdberater,
																							.kdemail = person.kdemail,
																							.kdplzort = person.kdplzort,
																							.kdtelefax = person.kdtelefax,
																							.kdtelefon = person.kdtelefon,
																							.ztelefax = person.ztelefax,
																							.ztelefon = person.ztelefon,
																							.znatel = person.znatel,
																							.zemail = person.zemail,
																							.kdzhdnr = person.kdzhdnr,
																							.kdzname = person.kdzname,
																							.kreditlimite = person.kreditlimite,
																							.kreditlimite_2 = person.kreditlimite_2,
																							.kreditlimiteab = person.kreditlimiteab,
																							.kreditlimitebis = person.kreditlimitebis,
																							.nachname = person.nachname,
																							.strasse = person.strasse,
																							.vorname = person.vorname,
																							.createdon = person.createdon,
																							.createdfrom = person.createdfrom,
																							.actives = person.actives,
																							.noes = person.noes,
																							.zfiliale = person.zfiliale,
																							.beraterin = person.beraterin
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedCustomerData) = New BindingList(Of FoundedCustomerData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdMain.DataSource = listDataSource
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		RefreshMainViewStateBar()
		grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)

		Return Not listOfFoundedData Is Nothing
	End Function

	Private Sub gvMain_ColumnFilterChanged(sender As Object, e As System.EventArgs) Handles gvMain.ColumnFilterChanged

		RefreshMainViewStateBar()

	End Sub


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

			If String.IsNullOrWhiteSpace(title) Then title = "Fileimport"
			If String.IsNullOrWhiteSpace(description) Then description = String.Format("Fileimport: {0}", fileInfo.FullName)
			If String.IsNullOrWhiteSpace(contactType) Then contactType = "Schriftlich"

			SaveCustomerContactData(title, description, contactType, contactDate, contactTime, attachedFile)
		End If

	End Sub

	''' <summary>
	''' Handles the form drag enter event.
	''' </summary>
	Private Sub OnFrmContacts_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles grdContact.DragEnter
		Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
		e.Effect = DragDropEffects.Copy
	End Sub

	Private Sub SaveCustomerContactData(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String)

		Dim currentContactRecordNumber As Integer = 0
		Dim currentDocumentID As Integer = 0

		Dim fileContent = m_Utility.LoadFileBytes(attachedFile)

		Dim contactData As ResponsiblePersonAssignedContactData = Nothing

		Dim dt = DateTime.Now
		contactData = New ResponsiblePersonAssignedContactData With {.CustomerNumber = m_CurrentCustomerNumber, .ResponsiblePersonNumber = m_CurrentResponsiblePersonNumber, .CreatedOn = dt, .CreatedFrom = m_InitializationData.UserData.UserFullName}

		contactData.CustomerNumber = m_CurrentCustomerNumber
		contactData.ResponsiblePersonNumber = If(m_CurrentResponsiblePersonNumber.GetValueOrDefault(0) = 0, Nothing, m_CurrentResponsiblePersonNumber)
		contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
		contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
		contactData.ContactPeriodString = title
		contactData.ContactsString = description
		contactData.ContactImportant = False
		contactData.ContactFinished = False
		contactData.MANr = 0
		contactData.VacancyNumber = Nothing
		contactData.ProposeNr = Nothing
		contactData.ESNr = Nothing

		contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
		contactData.UsNr = m_InitializationData.UserData.UserNr

		Dim isNewContact = (contactData.ID = 0)

		Dim success As Boolean = True

		' Check if the document bytes must be saved.
		If Not (fileContent Is Nothing) And success Then

			Dim contactDocument As ContactDoc = Nothing

			contactDocument = New ContactDoc() With {.CreatedOn = dt,
																									 .CreatedFrom = m_InitializationData.UserData.UserFullName,
																									 .FileBytes = fileContent,
																									 .FileExtension = Path.GetExtension(attachedFile)}
			success = success AndAlso m_CustomerDataAccess.AddContactDocument(contactDocument)

			If success Then
				currentDocumentID = contactDocument.ID
				contactData.KontaktDocID = currentDocumentID
			End If

		End If

		' Insert or update contact
		If isNewContact Then
			success = success AndAlso m_CustomerDataAccess.AddResponsiblePersonContactAssignment(contactData)

			If success Then
				currentContactRecordNumber = contactData.RecordNumber
			End If

		End If

		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kunden Kontaktdaten konnten nicht gespeichert werden."))
		Else

			LoadCustomerContactData()


			Dim kontakte = New SP.KD.KontaktMng.frmContacts(m_InitializationData)
			If (kontakte.ActivateNewContactDataMode(m_CurrentCustomerNumber, If(m_CurrentResponsiblePersonNumber.GetValueOrDefault(0) = 0, Nothing, m_CurrentResponsiblePersonNumber), Nothing, Nothing)) Then
				kontakte.LoadContactData(m_CurrentCustomerNumber, If(m_CurrentResponsiblePersonNumber.GetValueOrDefault(0) = 0, Nothing, m_CurrentResponsiblePersonNumber), currentContactRecordNumber, Nothing)

				kontakte.Show()
				kontakte.BringToFront()
			End If

		End If

	End Sub



	Private Sub frm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_SuppressUIEvents = True

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_SCC_HEADERINFO_SPLITTERPOSION)
		If setting_splitterpos > 0 Then Me.sccHeaderInfo.SplitterPosition = Math.Max(Me.sccHeaderInfo.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_DPPROPERTY_WIDTH)
		If setting_splitterpos > 0 Then Me.dpProperty.Width = Math.Max(Me.dpProperty.Width, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainProp_1.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_1_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainProp_1_1.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_NAV_2_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainNav_2.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		setting_splitterpos = m_SettingsManager.ReadInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_2_1_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMainProp_2_1.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)
		sccMainNav_1.SplitterPosition = 75

		Dim setting_vacancypropose = m_SettingsManager.ReadBoolean(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_SHOWVACANCY_DEFAULT)
		If setting_vacancypropose Then m_VacancyPropse = VacancyProposeEnun.VACANCY Else m_VacancyPropse = VacancyProposeEnun.PROPOSE

		If m_VacancyPropse = VacancyProposeEnun.VACANCY Then
			grpPropose.Text = m_Translate.GetSafeTranslationValue("Vakanzen")
		Else
			grpPropose.Text = m_Translate.GetSafeTranslationValue("Vorschläge")
		End If


		TranslateControls()

		SetFormLayout()
		BuildNewPrintContextMenu()

		LoadFoundedCustomerList()

		m_SuppressUIEvents = False

	End Sub

	Sub TranslateControls()

		Me.cmdNew.Text = m_Translate.GetSafeTranslationValue(Me.cmdNew.Text)
		Me.cmdPrint.Text = m_Translate.GetSafeTranslationValue(Me.cmdPrint.Text)

		Me.grpFunction.Text = m_Translate.GetSafeTranslationValue(Me.grpFunction.Text)
		Me.grpES.Text = m_Translate.GetSafeTranslationValue(Me.grpES.Text)
		Me.grpPropose.Text = m_Translate.GetSafeTranslationValue(Me.grpPropose.Text)

		For Each itm In grpPropose.CustomHeaderButtons
			itm = CType(itm, ButtonsPanelControl.GroupBoxButton)
			If itm.groupindex = 0 OrElse itm.groupindex = 1 Then
				itm.caption = m_Translate.GetSafeTranslationValue(itm.caption)
			End If
		Next

		Me.grpContact.Text = m_Translate.GetSafeTranslationValue(Me.grpContact.Text)

		Me.grpRE.Text = m_Translate.GetSafeTranslationValue(Me.grpRE.Text)
		Me.grpZE.Text = m_Translate.GetSafeTranslationValue(Me.grpZE.Text)
		Me.grpRP.Text = m_Translate.GetSafeTranslationValue(Me.grpRP.Text)

		Me.dpProperties.Text = m_Translate.GetSafeTranslationValue(Me.dpProperties.Text)
		Me.dpSProperties.Text = m_Translate.GetSafeTranslationValue(Me.dpSProperties.Text)

	End Sub


	Sub SaveFormProperties()

		If m_SuppressUIEvents Then Return


		sccMainNav_1.SplitterPosition = 75
		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_SCC_HEADERINFO_SPLITTERPOSION, Me.sccHeaderInfo.SplitterPosition)

		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_DPPROPERTY_WIDTH, Me.dpProperty.Width)

		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_1_SPLITTERPOSION, Me.sccMainProp_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_1_1_SPLITTERPOSION, Me.sccMainProp_1_1.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_NAV_2_SPLITTERPOSION, Me.sccMainNav_2.SplitterPosition)
		m_SettingsManager.WriteInteger(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_2_1_SPLITTERPOSION, Me.sccMainProp_2_1.SplitterPosition)

		m_SettingsManager.WriteBoolean(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_SHOWVACANCY_DEFAULT, If(m_VacancyPropse = VacancyProposeEnun.VACANCY, True, False))

		m_SettingsManager.SaveSettings()

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedCustomerData)

				m_CurrentCustomerNumber = viewData.kdnr
				m_CurrentResponsiblePersonNumber = viewData.kdzhdnr

				Me._KDSetting.SelectedKDNr = viewData.kdnr
				Me._KDSetting.SelectedKDzNr = viewData.kdzhdnr

				'BuildNewPrintContextMenu()

				Select Case column.Name.ToLower
					Case "ztelefon"
						If viewData.ztelefon.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
							_ClsKD.TelefonCallToCustomer(viewData.ztelefon)
						End If

					Case "kdemail"
						If viewData.kdemail.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
							_ClsKD.SendEMailToCustomer(viewData.kdemail)
						End If

					Case "zemail"
						If viewData.zemail.Length > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
							_ClsKD.SendEMailTocResponsible(viewData.zemail)
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
							_ClsKD.OpenSelectedCPerson()
						End If

					Case Else
						If viewData.kdnr > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
							_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
						End If

				End Select

			End If

		End If

	End Sub


	Private Sub gvMainOnMouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
		Trace.WriteLine("GridView1_MouseDown")

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
					strColValue = GetCellValue(strPFieldName.Split(CChar(";"))(i).ToLower, selectedrow).mnuvalue

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

	Private Function GetCellValue(ByVal strCellName As String, ByVal selectedrow As FoundedCustomerData) As MenuData

		Select Case strCellName.ToLower
			Case "beraterin"
				Return New MenuData With {.mnuvalue = selectedrow.beraterin}

			Case "firma1"
				Return New MenuData With {.mnuvalue = selectedrow.firma1}

			Case "fproperty"
				Return New MenuData With {.mnuvalue = selectedrow.fproperty}

			Case "howkontakt"
				Return New MenuData With {.mnuvalue = selectedrow.howkontakt}

			Case "kdberater"
				Return New MenuData With {.mnuvalue = selectedrow.kdberater}

			Case "kdemail"
				Return New MenuData With {.mnuvalue = selectedrow.kdemail}

			Case "kdnr"
				Return New MenuData With {.mnuvalue = selectedrow.kdnr}

			Case "kdplzort"
				Return New MenuData With {.mnuvalue = selectedrow.kdplzort}

			Case "kdstate1"
				Return New MenuData With {.mnuvalue = selectedrow.kdstate1}

			Case "kdtelefax"
				Return New MenuData With {.mnuvalue = selectedrow.kdtelefax}

			Case "kdtelefon"
				Return New MenuData With {.mnuvalue = selectedrow.kdtelefon}

			Case "kdzhdnr"
				Return New MenuData With {.mnuvalue = selectedrow.kdzhdnr}

			Case "kdzname"
				Return New MenuData With {.mnuvalue = selectedrow.kdzname}

			Case "kreditlimite"
				Return New MenuData With {.mnuvalue = selectedrow.kreditlimite}

			Case "kreditlimite_2"
				Return New MenuData With {.mnuvalue = selectedrow.kreditlimite_2}

			Case "kreditlimiteab"
				Return New MenuData With {.mnuvalue = selectedrow.kreditlimiteab}

			Case "kreditlimitebis"
				Return New MenuData With {.mnuvalue = selectedrow.kreditlimitebis}

			Case "nachname"
				Return New MenuData With {.mnuvalue = selectedrow.nachname}

			Case "strasse"
				Return New MenuData With {.mnuvalue = selectedrow.strasse}

			Case "vorname"
				Return New MenuData With {.mnuvalue = selectedrow.vorname}


			Case Else
				Return Nothing

		End Select

	End Function

	Sub GetMnuItem(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuItem As String = e.Item.Name.ToString
		Dim strMnuValue As String = e.Item.AccessibleDescription

		Select Case strMnuItem.ToUpper
			Case "KDName".ToUpper
				RunExtraFunctions4Listing(strMnuItem)

			'Case "Telefon".ToUpper
			'	Dim _ClsKD As New ClsKDModuls(Me._KDSetting)
			'	_ClsKD.ShowCallForm(strMnuValue)

			Case "@KDProperties".ToUpper
				RunExtraFunctions4Listing(strMnuItem)

		End Select

	End Sub

	Sub RunExtraFunctions4Listing(ByVal strFuncname As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Select Case strFuncname.ToUpper
			Case "KDName".ToUpper
				Dim _ClsPropose As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._KDSetting.SelectedKDNr})
				_ClsPropose.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

		End Select

	End Sub


	Private Sub FillPopupFields(ByVal selectedrow As FoundedCustomerData)
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
					Case "beraterin"
						strValue = selectedrow.beraterin
					Case "zfiliale"
						strValue = selectedrow.zfiliale

					Case "firma1"
						strValue = selectedrow.firma1

					Case "fproperty"
						strValue = selectedrow.fproperty

					Case "howkontakt"
						strValue = selectedrow.howkontakt

					Case "createdon"
						strValue = Format(selectedrow.createdon, "D")
					Case "createdfrom"
						strValue = selectedrow.createdfrom

					Case "kdberater"
						strValue = selectedrow.kdberater

					Case "kdemail"
						strValue = selectedrow.kdemail

					Case "kdnr"
						strValue = selectedrow.kdnr

					Case "kdplzort"
						strValue = selectedrow.kdplzort

					Case "kdstate1"
						strValue = selectedrow.kdstate1
					Case "kdstate2"
						strValue = selectedrow.kdstate2

					Case "kdtelefax"
						strValue = selectedrow.kdtelefax

					Case "kdtelefon"
						strValue = selectedrow.kdtelefon

					Case "kdzhdnr"
						strValue = selectedrow.kdzhdnr

					Case "kdzname"
						strValue = selectedrow.kdzname

					Case "kreditlimite"
						strValue = selectedrow.kreditlimite

					Case "kreditlimite_2"
						strValue = selectedrow.kreditlimite_2

					Case "kreditlimiteab"
						strValue = selectedrow.kreditlimiteab

					Case "kreditlimitebis"
						strValue = selectedrow.kreditlimitebis

					Case "nachname"
						strValue = selectedrow.nachname

					Case "strasse"
						strValue = selectedrow.strasse

					Case "vorname"
						strValue = selectedrow.vorname

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


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			ShowErrDetail(ex.Message)
			result = Nothing

		End Try

	End Sub

	Private Sub OngvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvMain.FocusedRowChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		If cmdNew.IsOpened Then cmdNew.HideDropDown()
		If cmdPrint.IsOpened Then cmdPrint.HideDropDown()

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				m_CurrentCustomerNumber = selectedrow.kdnr
				m_CurrentResponsiblePersonNumber = selectedrow.kdzhdnr

				Me._KDSetting.SelectedKDNr = selectedrow.kdnr
				If selectedrow.kdnr = 0 Then Return
				Me._KDSetting.SelectedKDzNr = selectedrow.kdzhdnr

				FillPopupFields(selectedrow)

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

	''' <summary>
	'''  Handles RowStyle event of main grid view.
	''' </summary>
	Private Sub OngvMain_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvMain.RowStyle

		If Not m_ShowcustomerinColor Then Return
		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvMain.GetRow(e.RowHandle), FoundedCustomerData)

			If rowData.fproperty > 0 Then
				e.Appearance.ForeColor = ColorTranslator.FromWin32(rowData.fproperty)
			End If

		End If

	End Sub




#Region "linke Navigationsleist"


	Sub FillProperties4Customer()

		' customer ES
		ResetESDetailGrid()
		LoadEmployeeESDetailList()

		If m_VacancyPropse = VacancyProposeEnun.VACANCY Then
			' customervacancy
			ResetVacancyDetailGrid()
			LoadCustomerVacancyDetailList()

		Else
			' customerproposal
			ResetProposalDetailGrid()
			LoadCustomerProposalDetailList()

		End If

		' employee rp
		ResetRPDetailGrid()
		LoadEmployeeRPDetailList()

		' customer Invoice
		ResetInvoiceDetailGrid()
		LoadCustomerInvoiceDetailList()

		' customer RecipientOfPayments
		ResetRecipientOfPaymentsDetailGrid()
		LoadCustomerRecipientOfPaymentsDetailList()

		' customerContact
		ResetContactDetailGrid()
		LoadCustomerContactData()

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
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columncustomername.Name = "employeename"
			columncustomername.FieldName = "employeename"
			columncustomername.Visible = True
			gvES.Columns.Add(columncustomername)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvES.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml(gvES.Name.ToLower)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdPropose.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeESDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbCustomerESDataForProperties(Me._KDSetting.SelectedKDNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedCustomerESDetailData With
													   {.mdnr = person.mdnr,
														  .esnr = person.esnr,
														  .kdnr = person.kdnr,
														  .zhdnr = person.zhdnr,
														  .esals = person.esals,
														  .employeename = person.employeename,
														  .periode = person.periode,
														  .zfiliale = person.zfiliale
													   }).ToList()

		Dim listDataSource As BindingList(Of FoundedCustomerESDetailData) = New BindingList(Of FoundedCustomerESDetailData)

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

	''' <summary>
	''' reset vacancy grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetVacancyDetailGrid()

		gvPropose.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvPropose.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvPropose.OptionsView.ShowGroupPanel = False
		gvPropose.OptionsView.ShowIndicator = False
		gvPropose.OptionsView.ShowAutoFilterRow = False

		gvPropose.Columns.Clear()


		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "vaknr"
			columnmodulname.FieldName = "vaknr"
			columnmodulname.Visible = False
			gvPropose.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvPropose.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_Translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "kdzhdnr"
			columnZHDNr.FieldName = "kdzhdnr"
			columnZHDNr.Visible = False
			gvPropose.Columns.Add(columnZHDNr)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvPropose.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "firma1"
			columncustomername.FieldName = "firma1"
			columncustomername.Visible = False
			gvPropose.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.Name = "kdzname"
			columnZHDName.FieldName = "kdzname"
			columnZHDName.Visible = True
			gvPropose.Columns.Add(columnZHDName)

			Dim columnjchisonline As New DevExpress.XtraGrid.Columns.GridColumn()
			columnjchisonline.Caption = m_Translate.GetSafeTranslationValue("Jobs.ch")
			columnjchisonline.Name = "jchisonline"
			columnjchisonline.FieldName = "jchisonline"
			columnjchisonline.Visible = False
			gvPropose.Columns.Add(columnjchisonline)

			Dim columnojisonline As New DevExpress.XtraGrid.Columns.GridColumn()
			columnojisonline.Caption = m_Translate.GetSafeTranslationValue("ostjob.ch")
			columnojisonline.Name = "ojisonline"
			columnojisonline.FieldName = "ojisonline"
			columnojisonline.Visible = False
			gvPropose.Columns.Add(columnojisonline)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Eigene Plattform")
			columnAdvisor.Name = "ourisonline"
			columnAdvisor.FieldName = "ourisonline"
			columnAdvisor.Visible = False
			gvPropose.Columns.Add(columnAdvisor)

			Dim columnjobchdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnjobchdate.Caption = m_Translate.GetSafeTranslationValue("Jobs.ch Datum")
			columnjobchdate.Name = "jobchdate"
			columnjobchdate.FieldName = "jobchdate"
			columnjobchdate.Visible = False
			gvPropose.Columns.Add(columnjobchdate)

			Dim columnostjobchdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnostjobchdate.Caption = m_Translate.GetSafeTranslationValue("ostjob.ch Datum")
			columnostjobchdate.Name = "ostjobchdate"
			columnostjobchdate.FieldName = "ostjobchdate"
			columnostjobchdate.Visible = False
			gvPropose.Columns.Add(columnostjobchdate)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPArt.Caption = m_Translate.GetSafeTranslationValue("Kontakt")
			columnPArt.Name = "vakkontakt"
			columnPArt.FieldName = "vakkontakt"
			columnPArt.Visible = False
			gvPropose.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPState.Caption = m_Translate.GetSafeTranslationValue("Status")
			columnPState.Name = "vakstate"
			columnPState.FieldName = "vakstate"
			columnPState.Visible = False
			gvPropose.Columns.Add(columnPState)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvPropose.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = True
			columnCreatedOn.BestFit()
			gvPropose.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvPropose.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml("gvvacancy")


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdPropose.DataSource = Nothing

	End Sub

	Sub ResetProposalDetailGrid()
		Dim ColumnID As String = "CustomerProposal"

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
			columnEmployeename.Visible = True
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

	Public Function LoadCustomerVacancyDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfVacancy = m_DataAccess.GetDbCustomerVacancyDataForProperties(Me._KDSetting.SelectedKDNr, True)

		Dim reportGridData = (From report In listOfVacancy
							  Select New CustomerVacanciesProperty With
{.mdnr = report.mdnr,
  .vaknr = report.vaknr,
  .kdnr = report.kdnr,
  .kdzhdnr = report.kdzhdnr,
  .vakstate = report.vakstate,
  .vak_kanton = report.vak_kanton,
  .vaklink = report.vaklink,
  .vakkontakt = report.vakkontakt,
  .vacancygruppe = report.vacancygruppe,
  .vacancyplz = report.vacancyplz,
  .vacancyort = report.vacancyort,
  .titelforsearch = report.titelforsearch,
  .shortdescription = report.shortdescription,
  .firma1 = report.firma1,
  .bezeichnung = report.bezeichnung,
  .createdon = report.createdon,
  .createdfrom = report.createdfrom,
  .kdzname = report.kdzname,
  .advisor = report.advisor,
  .kdemail = report.kdemail,
  .zemail = report.zemail,
  .jchisonline = report.jchisonline,
  .ojisonline = report.ojisonline,
  .ourisonline = report.ourisonline,
  .kdtelefon = report.kdtelefon,
  .kdtelefax = report.kdtelefax,
  .ztelefon = report.ztelefon,
  .ztelefax = report.ztelefax,
  .znatel = report.znatel,
  .jobchdate = report.jobchdate,
  .ostjobchdate = report.ostjobchdate,
  .zfiliale = report.zfiliale
}).ToList()

		Dim listDataSource As BindingList(Of CustomerVacanciesProperty) = New BindingList(Of CustomerVacanciesProperty)

		For Each p In reportGridData
			listDataSource.Add(p)
		Next

		grdPropose.DataSource = listDataSource

		Return Not listOfVacancy Is Nothing
	End Function

	Public Function LoadCustomerProposalDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbCustomerProposalDataForProperties(Me._KDSetting.SelectedKDNr)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedCustomerProposalDetailData With
{.pnr = person.pnr,
.mdnr = person.mdnr,
.kdnr = person.kdnr,
.zhdnr = person.zhdnr,
.manr = person.manr,
.bezeichnung = person.bezeichnung,
.employeename = person.employeename,
.zhdname = person.zhdname,
.p_art = person.p_art,
.p_state = person.p_state,
.createdon = person.createdon,
.createdfrom = person.createdfrom,
.zfiliale = person.zfiliale
}).ToList()

		Dim listDataSource As BindingList(Of FoundedCustomerProposalDetailData) = New BindingList(Of FoundedCustomerProposalDetailData)

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

				If m_VacancyPropse = VacancyProposeEnun.PROPOSE Then

					Dim proposeViewData = SelectedProposeViewData '  CType(dataRow, FoundedCustomerProposalDetailData)
					If proposeViewData.pnr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = proposeViewData.mdnr, .SelectedProposeNr = proposeViewData.pnr,
																																.SelectedMANr = proposeViewData.manr, .SelectedKDNr = proposeViewData.kdnr, .SelectedZHDNr = proposeViewData.zhdnr})
						_ClsKD.OpenSelectedProposeTiny(proposeViewData.mdnr, ModulConstants.UserData.UserNr)
					End If

				Else
					Dim proposeViewData = SelectedVacancyViewData

					If proposeViewData.vaknr > 0 Then
						Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = proposeViewData.mdnr, .SelectedVakNr = proposeViewData.vaknr,
																																.SelectedKDNr = proposeViewData.kdnr, .SelectedZHDNr = 0})
						_ClsKD.OpenSelectedVacancyTiny(proposeViewData.mdnr, ModulConstants.UserData.UserNr)
					End If

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

			Dim columnemployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnemployeename.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnemployeename.Name = "employeename"
			columnemployeename.FieldName = "employeename"
			columnemployeename.Visible = False
			columnemployeename.BestFit()
			gvRP.Columns.Add(columnemployeename)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
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
		Dim listOfEmployees = m_DataAccess.GetDbCustomerReportDataForProperties(Me._KDSetting.SelectedKDNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedCustomerReportDetailData With
																						 {.mdnr = person.mdnr,
																							.rpnr = person.rpnr,
																							.kdnr = person.kdnr,
																							.manr = person.manr,
																							.periode = person.periode,
																							.employeename = person.employeename,
																							.createdfrom = person.createdfrom,
																							.createdon = person.createdon,
																							.zfiliale = person.zfiliale
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedCustomerReportDetailData) = New BindingList(Of FoundedCustomerReportDetailData)

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
				Dim viewData = CType(dataRow, FoundedCustomerReportDetailData)

				If viewData.rpnr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
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
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "renr"
			columnmodulname.FieldName = "renr"
			columnmodulname.Visible = False
			gvInvoice.Columns.Add(columnmodulname)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "fakdate"
			columnFakDat.FieldName = "fakdate"
			columnFakDat.Visible = True
			gvInvoice.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "betragink"
			columnBetragInk.FieldName = "betragink"
			columnBetragInk.Visible = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvInvoice.Columns.Add(columnBetragInk)

			Dim columnREKst As New DevExpress.XtraGrid.Columns.GridColumn()
			columnREKst.Caption = m_Translate.GetSafeTranslationValue("KST")
			columnREKst.Name = "rekst"
			columnREKst.FieldName = "rekst"
			columnREKst.Visible = True
			gvInvoice.Columns.Add(columnREKst)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvInvoice.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvInvoice.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
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


	Public Function LoadCustomerInvoiceDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbCustomerInvoiceDataForProperties(Me._KDSetting.SelectedKDNr)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler in der Debitoren-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedCustomerInvoiceDetailData With
													   {.mdnr = person.mdnr,
														  .customerMDNr = person.customerMDNr,
														  .renr = person.renr,
														  .kdnr = person.kdnr,
														  .firma1 = person.firma1,
														  .plzort = person.plzort,
														  .fakdate = person.fakdate,
														  .betragink = person.betragink,
														  .betragopen = person.betragopen,
														  .rekst = person.rekst,
														  .customeradvisor = person.customeradvisor,
														  .employeeadvisor = person.employeeadvisor,
														  .createdon = person.createdon,
														  .createdfrom = person.createdfrom,
														  .zfiliale = person.zfiliale
													   }).ToList()

		Dim listDataSource As BindingList(Of FoundedCustomerInvoiceDetailData) = New BindingList(Of FoundedCustomerInvoiceDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdInvoice.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Sub OngvInvoice_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If Not ModulConstants.UserSecValue(14) Then Return

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvInvoice.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedCustomerInvoiceDetailData)

				If viewData.renr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedInvoice(viewData.mdnr, ModulConstants.UserData.UserNr)
				End If

			End If

		End If

	End Sub


#End Region



#Region "ZE Funktionen..."

	Sub ResetRecipientOfPaymentsDetailGrid()

		gvZE.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvZE.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvZE.OptionsView.ShowGroupPanel = False
		gvZE.OptionsView.ShowIndicator = False
		gvZE.OptionsView.ShowAutoFilterRow = False

		gvZE.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zenr"
			columnmodulname.FieldName = "zenr"
			columnmodulname.Visible = False
			gvZE.Columns.Add(columnmodulname)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnFakDat.Name = "valutadate"
			columnFakDat.FieldName = "valutadate"
			columnFakDat.Visible = True
			gvZE.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "zebetrag"
			columnBetragInk.FieldName = "zebetrag"
			columnBetragInk.Visible = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvZE.Columns.Add(columnBetragInk)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvZE.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvZE.Columns.Add(columnCreatedFrom)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_Translate.GetSafeTranslationValue("Filiale")
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

	Public Function LoadCustomerRecipientOfPaymentsDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbCustomerRecipientOfPaymentsDataForProperties(Me._KDSetting.SelectedKDNr)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedCustomerROPDetailData With
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

		Dim listDataSource As BindingList(Of FoundedCustomerROPDetailData) = New BindingList(Of FoundedCustomerROPDetailData)

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
				Dim viewData = CType(dataRow, FoundedCustomerROPDetailData)

				If viewData.zenr > 0 Then
					Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZENr = viewData.zenr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
					_ClsKD.OpenSelectedPayment()
				End If

			End If

		End If

	End Sub


#End Region


#Region "Contact Funktionen..."

	Sub ResetContactDetailGrid()
		Dim ColumnID As String = "CustomerContact"

		gvContact.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvContact.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvContact.OptionsView.ShowGroupPanel = False
		gvContact.OptionsView.ShowIndicator = False
		gvContact.OptionsView.ShowAutoFilterRow = False

		gvContact.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "contactnr"
			columnmodulname.FieldName = "contactnr"
			columnmodulname.Visible = False
			gvContact.Columns.Add(columnmodulname)

			Dim columndestlmnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestlmnr.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columndestlmnr.Name = "datum"
			columndestlmnr.FieldName = "datum"
			columndestlmnr.Visible = True
			gvContact.Columns.Add(columndestlmnr)

			Dim columndestzgnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestzgnr.Caption = m_Translate.GetSafeTranslationValue("ZHD.-Person")
			columndestzgnr.Name = "zhdname"
			columndestzgnr.FieldName = "zhdname"
			columndestzgnr.Visible = True
			gvContact.Columns.Add(columndestzgnr)

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
			columndestesnr.Visible = False
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


	Public Function LoadCustomerContactData() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_ModulViewDatabaseAccess.LoadGetDbCustomerContactData(ModulConstants.MDData.MDNr, m_CurrentCustomerNumber, Nothing, Nothing, Nothing, True, String.Empty)
		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))

			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New ModulViewCustomerContactData With
													   {.contactnr = person.contactnr,
														  .kdnr = person.kdnr,
														  .zhdnr = person.zhdnr,
														  .datum = person.datum,
														  .employeename = person.employeename,
														  .customername = person.customername,
														  .zhdname = person.zhdname,
														  .bezeichnung = person.bezeichnung,
														  .beschreibung = person.beschreibung,
														  .art = person.art,
														  .kst = person.kst,
														  .createdon = person.createdon,
														  .createdfrom = person.createdfrom
													   }).ToList()

		Dim listDataSource As BindingList(Of ModulViewCustomerContactData) = New BindingList(Of ModulViewCustomerContactData)

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
				Dim viewData = CType(dataRow, ModulViewCustomerContactData)

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
	Private Sub cmdPrint_Click(sender As System.Object, e As System.EventArgs) Handles cmdPrint.Click
		If cmdPrint.IsOpened Then cmdPrint.HideDropDown()
		If cmdNew.IsOpened Then cmdNew.HideDropDown()
		Me.cmdPrint.ShowDropDown()
	End Sub

	''' <summary>
	''' shows Contexmenu for New-button
	''' </summary>
	Private Sub cmdNew_Click(sender As System.Object, e As System.EventArgs) Handles cmdNew.Click
		If cmdPrint.IsOpened Then cmdPrint.HideDropDown()
		If cmdNew.IsOpened Then cmdNew.HideDropDown()
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
				ResetGrid()
				LoadFoundedCustomerList()
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
	Private Sub OngrpFunction_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpFunction.CustomButtonClick

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
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																						ModulConstants.ProsonalizedData,
																																						ModulConstants.MDData,
																																						ModulConstants.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_CustomerDataAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		ResetGrid()
		LoadFoundedCustomerList()

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

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_KDSetting.CustomerName = data.firma1
			Me._KDSetting.Data4SelectedKD = True
			strModul2Open = "kdES".ToLower

			Dim frm As New frmKDDetails(Me._KDSetting, strModul2Open)
			frm.Show()
			frm.BringToFront()

		End If

	End Sub

	Private Sub OngrpPropose_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpPropose.CustomButtonClick

		LoadAssignedVacancyProposalProperty(e.Button.Properties.GroupIndex)

		If e.Button.Properties.GroupIndex <> 2 Then
			m_SettingsManager.WriteBoolean(SettingKDKeys.SETTING_KD_SCC_MAIN_PROP_SHOWVACANCY_DEFAULT, If(m_VacancyPropse = VacancyProposeEnun.VACANCY, True, False))
			m_SettingsManager.SaveSettings()
		End If

	End Sub

	Private Sub LoadAssignedVacancyProposalProperty(ByVal buttonIndex As Integer)

		If buttonIndex = 0 Then
			m_VacancyPropse = VacancyProposeEnun.VACANCY
			grpPropose.Text = m_Translate.GetSafeTranslationValue("Vakanzen")
			ResetVacancyDetailGrid()
			LoadCustomerVacancyDetailList()

		ElseIf buttonIndex = 1 Then
			grpPropose.Text = m_Translate.GetSafeTranslationValue("Vorschläge")
			m_VacancyPropse = VacancyProposeEnun.PROPOSE
			ResetProposalDetailGrid()
			LoadCustomerProposalDetailList()

		ElseIf buttonIndex = 2 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_KDSetting.CustomerName = data.firma1
			Me._KDSetting.Data4SelectedKD = True
			If m_VacancyPropse = VacancyProposeEnun.VACANCY Then
				strModul2Open = "kdVacancy".ToLower
			Else
				strModul2Open = "kdPropose".ToLower
			End If

			Dim frm As New frmKDDetails(Me._KDSetting, strModul2Open)
			frm.Show()
			frm.BringToFront()

		End If

	End Sub

	Private Sub OngrpContact_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpContact.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_KDSetting.CustomerName = data.firma1
			Me._KDSetting.Data4SelectedKD = True
			strModul2Open = "kdcontact".ToLower

			Dim frm As New frmKDDetails(Me._KDSetting, strModul2Open)
			frm.LoadData()

			frm.Show()
			frm.BringToFront()

		End If

	End Sub

	Private Sub OngrpRE_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpRE.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_KDSetting.CustomerName = data.firma1
			Me._KDSetting.Data4SelectedKD = True
			strModul2Open = "kdre".ToLower

			Dim frm As New frmKDDetails(Me._KDSetting, strModul2Open)
			frm.Show()
			frm.BringToFront()

		End If

	End Sub

	Private Sub OngrpZE_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpZE.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_KDSetting.CustomerName = data.firma1
			Me._KDSetting.Data4SelectedKD = True
			strModul2Open = "kdze".ToLower

			Dim frm As New frmKDDetails(Me._KDSetting, strModul2Open)
			frm.Show()
			frm.BringToFront()

		End If

	End Sub

	Private Sub OngrpRP_CustomButtonClick(sender As Object, e As DevExpress.XtraBars.Docking2010.BaseButtonEventArgs) Handles grpRP.CustomButtonClick

		If e.Button.Properties.GroupIndex = 0 Then
			Dim strModul2Open As String = String.Empty

			Dim data = SelectedRowViewData
			If data Is Nothing Then Return

			_KDSetting.CustomerName = data.firma1
			Me._KDSetting.Data4SelectedKD = True
			strModul2Open = "kdRP".ToLower

			Dim frm As New frmKDDetails(Me._KDSetting, strModul2Open)
			frm.Show()
			frm.BringToFront()

		End If

	End Sub


#End Region


#Region "Helpers"

	Private Sub BuildNewPrintContextMenu()
		Dim m_DataAccess As New MainGrid

		Try
			' build contextmenu
			Dim mnuData = m_DataAccess.LoadContextMenu4PrintCustomerData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Return
			End If

			'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			cmdPrint.DropDownControl = PopupMenu1
			PopupMenu1.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem

				itm.Caption = (mnuData(i).MnuCaption)
				itm.Name = mnuData(i).MnuName

				If Not mnuData(i).MnuGrouped Then
					PopupMenu1.AddItem(itm)
				Else
					PopupMenu1.AddItem(itm).BeginGroup = True
				End If
				AddHandler itm.ItemClick, AddressOf GetMnuItem4Print
			Next

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Try
			cmdNew.DropDownControl = PopupMenu2
			' build contextmenu
			Dim mnuData = m_DataAccess.LoadContextMenu4NewCustomerData
			If (mnuData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

				Exit Sub
			End If

			'Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			PopupMenu2.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1
				itm = New DevExpress.XtraBars.BarButtonItem

				itm.Caption = (mnuData(i).MnuCaption)
				itm.Name = mnuData(i).MnuName
				If Not mnuData(i).MnuGrouped Then
					PopupMenu2.AddItem(itm)
				Else
					PopupMenu2.AddItem(itm).BeginGroup = True
				End If
				AddHandler itm.ItemClick, AddressOf GetMnuItem4New
			Next

			' show contextmenu
			'If Me.Visible Then PopupMenu2.ShowPopup(New Point(cmdNew.Location))


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


	End Sub

	''' <summary>
	''' wertet die contextmenu vom printbutton
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Sub GetMnuItem4Print(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuName As String = e.Item.Name.ToLower

		Select Case strMnuName
			Case "kdstammblatt".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._KDSetting.SelectedKDNr})
				obj.PrintKDStammblatt()

			Case Else
				Exit Sub

		End Select

	End Sub

	''' <summary>
	''' wertet die contextmenu vom newbutton
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Sub GetMnuItem4New(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMnuName As String = e.Item.Name.ToLower

		Select Case strMnuName
			Case "kdNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = 0})
				obj.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "zhdNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = Me._KDSetting.SelectedKDNr,
																												 .SelectedZHDNr = Nothing})
				obj.OpenSelectedCPerson() 'ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "VacancyNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																												 .SelectedVakNr = 0,
																												 .SelectedKDNr = Me._KDSetting.SelectedKDNr,
																												 .SelectedZHDNr = Me._KDSetting.SelectedKDzNr,
																												 .SelectedMANr = 0})
				obj.OpenSelectedVacancyTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ProposeNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																												 .SelectedProposeNr = 0,
																												 .SelectedKDNr = Me._KDSetting.SelectedKDNr,
																												 .SelectedZHDNr = Me._KDSetting.SelectedKDzNr,
																												 .SelectedMANr = Nothing,
																												 .SelectedVakNr = Nothing})
				obj.OpenSelectedProposeTiny(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ESNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedESNr = Nothing, .SelectedMANr = Nothing,
																												 .SelectedKDNr = Me._KDSetting.SelectedKDNr, .SelectedZHDNr = Me._KDSetting.SelectedKDzNr,
																												 .SelectedVakNr = Nothing, .SelectedProposeNr = Nothing})
				obj.CreateNewES(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "reNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedRENr = 0,
																												 .SelectedKDNr = Me._KDSetting.SelectedKDNr})
				obj.OpenSelectedInvoice(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			Case "ContactNew".ToLower
				Dim obj As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr,
																												 .SelectedKDNr = Me._KDSetting.SelectedKDNr,
																												 .SelectedZHDNr = Me._KDSetting.SelectedKDzNr})
				obj.OpenSelectedCustomerContactForNewEntry()

			Case "DocScan".ToLower
				Dim _ClsLO As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = ModulConstants.MDData.MDNr})
				_ClsLO.OpenAutoRPScan()


			Case Else
				Exit Sub

		End Select

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
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingMainFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreMainSetting), True)

				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVMainSettingfilename) Then gvMain.RestoreLayoutFromXml(m_GVMainSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvMain.ActiveFilterCriteria = Nothing

			Case "gves".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingESFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreESSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVESSettingfilename) Then gvES.RestoreLayoutFromXml(m_GVESSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvES.ActiveFilterCriteria = Nothing

			Case "gvvacancy".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingVacancyFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreVacancySetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVvacancySettingfilename) Then gvPropose.RestoreLayoutFromXml(m_GVvacancySettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvPropose.ActiveFilterCriteria = Nothing


			Case "gvpropose".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingProposeFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreProposeSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVProposeSettingfilename) Then gvPropose.RestoreLayoutFromXml(m_GVProposeSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvPropose.ActiveFilterCriteria = Nothing


			Case "gvcontact".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingContactFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreContactSetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVContactSettingfilename) Then gvContact.RestoreLayoutFromXml(m_GVContactSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvContact.ActiveFilterCriteria = Nothing



			Case "gvInvoice".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingREFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRESetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVInvoiceSettingfilename) Then gvInvoice.RestoreLayoutFromXml(m_GVInvoiceSettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvInvoice.ActiveFilterCriteria = Nothing


			Case "gvZE".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingZEFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreZESetting), True)
				Catch ex As Exception

				End Try

				If restoreLayout AndAlso File.Exists(m_GVZESettingfilename) Then gvZE.RestoreLayoutFromXml(m_GVZESettingfilename)
				If restoreLayout AndAlso Not keepFilter Then gvZE.ActiveFilterCriteria = Nothing

			Case "gvRP".ToLower
				Try
					keepFilter = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRPFilter), False)
					restoreLayout = m_utilitySP.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRPSetting), True)
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

	Private Sub OngvInvoiceColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvInvoice.SaveLayoutToXml(m_GVInvoiceSettingfilename)

	End Sub

	Private Sub OngvReportColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVReportSettingfilename)

	End Sub

	Private Sub OngvZEColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvZE.SaveLayoutToXml(m_GVZESettingfilename)

	End Sub

	Private Function StrToBool(ByVal str As String) As Boolean

		Dim result As Boolean = False

		If String.IsNullOrWhiteSpace(str) Then
			Return False
		ElseIf str = "1" Then
			Return True

		End If

		Boolean.TryParse(str, result)

		Return result
	End Function


#End Region

#End Region



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

