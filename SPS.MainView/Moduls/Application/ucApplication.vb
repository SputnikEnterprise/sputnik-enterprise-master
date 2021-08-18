
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.CommonXmlUtility

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects

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

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging


Imports SP.Infrastructure.Settings
Imports SPS.MainView.EmployeeSettings
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports DevExpress.XtraSplashScreen
Imports System.Threading.Tasks
Imports System.Threading

Public Class ucApplication


#Region "private consts"

	Private Const DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"
	Private Const MODUL_NAME = "Applicationmanagement"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}]"
	Private Const UsercontrolCaption As String = "Bewerbermanagement"

	Private Const MODUL_NAME_SETTING = "APPLICATION"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/mainview/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/mainview/{1}/keepfilter"

	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_NR As String = "MD_{0}/Sonstiges/autofilterconditionnr"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AUTOFILTERCONDITION_DATE As String = "MD_{0}/Sonstiges/autofilterconditiondate"

	Private Const DATE_COLUMN_NAME As String = "magebdat;createdon;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;printdate;reminder_1date;reminder_2date;reminder_3date;buchungdate;faelligdate;checkedon;avamreportingdate;avamfrom;avamuntil"
	Private Const INTEGER_COLUMN_NAME As String = "vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;id;vgnr;monat;jahr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"

#End Region


#Region "private fields"

	Private m_communicationHub As TinyMessenger.ITinyMessengerHub
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connString As String

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_AppDatabaseAccess As IAppDatabaseAccess

	''' <summary>
	''' Service Uri of Sputnik notification util webservice.
	''' </summary>
	Private m_ApplicationUtilWebServiceUri As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	Private Property LoadedMDNr As Integer

	Protected m_SettingsManager As ISettingsManager

	Private aColFieldName As String()
	Private aColCaption As String()
	Private aColWidth As String()

	Private aPropertyColFieldName As String()
	Private aPropertyColCaption As String()

	Private m_MandantData As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()

	Private m_griddata As GridData
	Private m_PropertyGriddata As PropertyGridData

	Private m_UtilityUI As UtilityUI
	Private m_MainUtility As SP.Infrastructure.Utility

	Private m_GVMainSettingfilename As String
	Private m_xmlSettingMainFilter As String
	Private m_xmlSettingRestoreMainSetting As String

	Private m_CurrentApplicationData As BindingList(Of MainViewApplicationData)

	Private m_CurrentApplicationID As Integer
	Private m_CurrentEmployeeID As Integer?
	Private m_CurrentVacancyNumber As Integer?
	Private m_CurrentApplicationLabel As String
	Private m_AllowedChangeMandant As Boolean

	Private m_MainViewGridData As SuportedCodes

#End Region



#Region "Private property"

	Private ReadOnly Property SelectedRowViewData As MainViewApplicationData
		Get
			Dim grdView = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), MainViewApplicationData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

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


#End Region


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
			WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

			BarManager1.Form = Me
			LoadedMDNr = ModulConstants.MDData.MDNr
			m_SettingsManager = New SettingsZEManager

			m_MandantData = New Mandant
			m_utility = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_MainUtility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_translate = New TranslateValues

			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																			ModulConstants.ProsonalizedData,
																																			ModulConstants.MDData,
																																			ModulConstants.UserData)

			m_connString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)
			m_AppDatabaseAccess = New AppDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)

			Try
				m_communicationHub = MessageService.Instance.Hub

				m_GVMainSettingfilename = String.Format("{0}{1}\{2}{3}.xml", ModulConstants.GridSettingPath, MODUL_NAME_SETTING, gvMain.Name, ModulConstants.UserData.UserNr)

				m_xmlSettingRestoreMainSetting = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_RESTORE, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)
				m_xmlSettingMainFilter = String.Format(USER_XML_SETTING_SPUTNIK_MAINVIEW_GRIDSETTING_FILTER, ModulConstants.UserData.UserNr, MODUL_NAME_SETTING)

				m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))

			Catch ex As Exception

			End Try
			Dim allowedCVDropIn As Boolean = m_MandantData.ModulLicenseKeys(ModulConstants.MDData.MDNr, Now.Year, "").CVDropIN AndAlso UserSecValue(678)
			m_AllowedChangeMandant = IsUserActionAllowed(ModulConstants.UserData.UserNr, 200003)
			grpFunction.CustomHeaderButtons(2).Properties.Enabled = m_AllowedChangeMandant

			grpFunction.CustomHeaderButtons(0).Properties.Enabled = allowedCVDropIn ' ModulConstants.UserData.UserNr = 1
			grpFunction.CustomHeaderButtons(1).Properties.Enabled = allowedCVDropIn

			m_ApplicationUtilWebServiceUri = DEFAULT_SPUTNIK_APPLICATION_UTIL_WEBSERVICE_URI

			AddHandler gvMain.RowCellClick, AddressOf OngvMain_RowCellClick
			AddHandler gvMain.FocusedRowChanged, AddressOf OngvMain_FocusedRowChanged
			AddHandler gvMain.ColumnFilterChanged, AddressOf OnGVMain_ColumnFilterChanged
			AddHandler Me.gvMain.ColumnPositionChanged, AddressOf OngvMainColumnPositionChanged
			AddHandler Me.gvMain.ColumnWidthChanged, AddressOf OngvMainColumnPositionChanged


			m_MainViewGridData = New SuportedCodes
			m_MainViewGridData.ChangeColumnNamesToLowercase = False
			m_MainViewGridData.LoadMainModulGridPropertiesFromXML(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr, MODUL_NAME)
			m_griddata = m_MainViewGridData.LoadMainGridData

			ResetMainGrid()

			ResetMandantenDropDown()
			LoadFoundedMDList()

			cboMD.EditValue = ModulConstants.MDData.MDNr
			'SetFormLayout()

			TranslateControls()

			AddHandler rgApplicationAdvisor.SelectedIndexChanged, AddressOf ChangeApplicationAdvisor
			AddHandler rgApplicationLifeCycel.SelectedIndexChanged, AddressOf ChangeApplicationAdvisor


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

	'	If Not m_griddata.GridColFieldName Is Nothing Then
	'		aColFieldName = m_griddata.GridColFieldName.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "").Split(CChar(";"))
	'		aColCaption = m_griddata.GridColCaption.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "").Split(CChar(";"))
	'		aColWidth = m_griddata.GridColWidth.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "").Split(CChar(";"))

	'		aPropertyColFieldName = m_griddata.FieldsInHeaderToShow.Split(CChar(";"))
	'		aPropertyColCaption = m_griddata.CaptionsInHeaderToShow.Split(CChar(";"))
	'	End If

	'End Sub


#Region "Private methods"

	Sub TranslateControls()

		Me.grpFunction.Text = m_translate.GetSafeTranslationValue(Me.grpFunction.Text)

	End Sub


	'Private Function GetGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
	'	Dim result As New GridData
	'	If iMDNr = 0 Then iMDNr = ModulConstants.MDData.MDNr
	'	m_path = New ClsProgPath

	'	Try
	'		Dim xDoc As XDocument = XDocument.Load(m_MandantData.GetSelectedMDMainViewXMLFilename(ModulConstants.MDData.MDNr))
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
	'											 Where Not (exportSetting.Attribute("ID") Is Nothing) _
	'													 And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'											 Select New With {
	'																 .SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																 .GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																 .DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																 .GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																 .GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																 .BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																 .ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																 .PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																 .PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																 .CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																 .FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																 .CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
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
	'													 And exportSetting.Attribute("ID").Value = String.Format("{0}", MODUL_NAME)
	'											 Select New With {
	'																 .SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
	'																 .GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
	'																 .DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
	'																 .GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
	'																 .GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
	'																 .BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
	'																 .ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
	'																 .PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
	'																 .PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
	'																 .CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
	'																 .FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
	'																 .CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
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

	Private Sub frm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetFormLayout()

		LoadData()

	End Sub

	Private Sub SetFormLayout()

		Me.Parent.Text = String.Format("{0}", m_translate.GetSafeTranslationValue(UsercontrolCaption))
		bsiMDData.Caption = String.Format("{0}, {1} | {2}", ModulConstants.MDData.MDName, ModulConstants.MDData.MDCity, ModulConstants.MDData.MDDbName)

		Try
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Dim setting_splitterpos = m_SettingsManager.ReadInteger(SettingMAKeys.SETTING_APPLICATION_SCC_MAIN_SPLITTERPOSION)
		If setting_splitterpos > 0 Then sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_splitterpos)

		Me.pcHeader.Dock = DockStyle.Fill
		Me.sccMain.Dock = DockStyle.Fill
		Me.sccHeaderInfo.Dock = DockStyle.Fill

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		AddHandler Me.sccMain.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SplitterPositionChanged, AddressOf SaveFormProperties
		AddHandler Me.sccHeaderInfo.SizeChanged, AddressOf SaveFormProperties

	End Sub

	Sub SaveFormProperties()

		sccHeaderInfo.SplitterPosition = sccHeaderInfo.Width - 150
		m_SettingsManager.WriteInteger(SettingMAKeys.SETTING_APPLICATION_SCC_MAIN_SPLITTERPOSION, Me.sccMain.SplitterPosition)

		m_SettingsManager.SaveSettings()

	End Sub


#End Region


	Private Sub ResetMainGrid()

		m_MainViewGridData.ResetMainGrid(gvMain, grdMain, MODUL_NAME)
		RestoreGridLayoutFromXml(gvMain.Name.ToLower)

		Return

		gvMain.Columns.Clear()

		Try
			Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
			Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

			For i = 0 To aColCaption.Length - 1

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				Dim columnCaption As String = m_translate.GetSafeTranslationValue(aColCaption(i).Trim)
				Dim columnName As String = aColFieldName(i).Trim

				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = columnCaption
				column.Name = columnName
				column.FieldName = columnName

				If DATE_COLUMN_NAME.ToLower.Contains(columnName) Then
					column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
					If columnName.ToLower = "Birthdate".ToLower Then
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


	Private Function LoadData() As Boolean
		Dim m_DataAccess As New MainGrid

		SearchApplicationlist()

		Return True
	End Function

	''' <summary>
	''' Search for notification.
	''' </summary>
	Private Sub SearchApplicationlist()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		m_CurrentApplicationData = Nothing
		grdMain.DataSource = m_CurrentApplicationData

		Task(Of BindingList(Of MainViewApplicationData)).Factory.StartNew(Function() PerformApplicationlistCallAsync(),
																							CancellationToken.None,
																							TaskCreationOptions.None,
																							TaskScheduler.Default).ContinueWith(Sub(t) FinishApplicationCallTask(t), CancellationToken.None,
																																									TaskContinuationOptions.None, uiSynchronizationContext)

	End Sub

	''' <summary>
	'''  Performs notification check asynchronous.
	''' </summary>
	Private Function PerformApplicationlistCallAsync() As BindingList(Of MainViewApplicationData)
		Dim m_DataAccess As New MainGrid

		Dim listDataSource As BindingList(Of MainViewApplicationData) = New BindingList(Of MainViewApplicationData)
		Dim Customer_ID As String = ModulConstants.MDData.MDGuid
		Dim userName As String = String.Empty
		Dim mdNumber As Integer = ModulConstants.MDData.MDNr

		' Read data over webservice
		Dim searchResult = m_DataAccess.LoadApplicationData(m_griddata.SQLQuery)
		If searchResult Is Nothing Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(String.Format("Bewerbungen konnten nicht geladen werden."))

			Return Nothing
		End If

		For Each result In searchResult

			Dim viewData = New MainViewApplicationData With {.MDNr = mdNumber,
																							.ID = result.ID,
																							.Customer_ID = result.Customer_ID,
																							.EmployeeID = result.EmployeeID,
																							.ApplicantLastname = result.ApplicantLastname,
																							.ApplicantFirstname = result.ApplicantFirstname,
																							.ApplicantStreet = result.ApplicantStreet,
																							.ApplicantPostcode = result.ApplicantPostcode,
																							.ApplicantLocation = result.ApplicantLocation,
																							.ApplicantCountry = result.ApplicantCountry,
																							.Birthdate = result.Birthdate,
																							.ApplicationLifecycle = result.ApplicationLifecycle,
																							.CreatedOn = result.CreatedOn,
																							.CreatedFrom = result.CreatedFrom,
																							.CheckedOn = result.CheckedOn,
																							.CheckedFrom = result.CheckedFrom,
																							.ApplicationLabel = result.ApplicationLabel,
																							.VacancyNumber = result.VacancyNumber,
																							.Advisor = result.Advisor,
																							.ApplicationAdvisorLastName = result.ApplicationAdvisorLastName,
																							.ApplicationAdvisorFirstName = result.ApplicationAdvisorFirstName,
																							.BusinessBranch = result.BusinessBranch,
																							.Dismissalperiod = result.Dismissalperiod,
																							.Availability = result.Availability,
																							.Comment = result.Comment,
																							.zfiliale = result.BusinessBranch,
																							.ShowAsApplicant = result.ShowAsApplicant
																						 }

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	''' <summary>
	''' Finish application call.
	''' </summary>
	Private Sub FinishApplicationCallTask(ByVal t As Task(Of BindingList(Of MainViewApplicationData)))

		Try

			Select Case t.Status
				Case TaskStatus.RanToCompletion
					' Webservice call was successful.
					'm_SuppressUIEvents = True
					m_CurrentApplicationData = t.Result
					grdMain.DataSource = m_CurrentApplicationData
					RunSelectedFilterOnRecords()

					RefreshMainViewStateBar()
					grpFunction.BackColor = If(Not String.IsNullOrWhiteSpace(ModulConstants.MDData.MandantColorName), System.Drawing.Color.FromName(ModulConstants.MDData.MandantColorName), Nothing)


					'm_SuppressUIEvents = False

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





	Private Sub OngvMain_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)

		Try
			Dim data = SelectedRowViewData

			If Not data Is Nothing Then
				m_CurrentEmployeeID = data.EmployeeID
				m_CurrentApplicationLabel = data.ApplicationLabel
				m_CurrentApplicationID = data.ID
				m_CurrentVacancyNumber = data.VacancyNumber

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			ShowErrDetail(ex.ToString)

		End Try

	End Sub

	Sub OngvMain_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		pccMandant.HidePopup()
		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvMain.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, MainViewApplicationData)

				Select Case column.Name.ToLower
					Case "customernumber", "customername"
						If viewData.Customernumber > 0 Then
							Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.MDNr, .SelectedKDNr = viewData.Customernumber})
							_ClsKD.OpenSelectedCustomer(viewData.MDNr, ModulConstants.UserData.UserNr)
						End If

					Case "ApplicantFullname".ToLower, "applicantnumber".ToLower
						If viewData.EmployeeID > 0 Then
							Dim _ClsMA As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.MDNr, .SelectedMANr = viewData.EmployeeID})
							If viewData.ShowAsApplicant.GetValueOrDefault(False) Then
								_ClsMA.OpenSelectedApplicant(viewData.MDNr, ModulConstants.UserData.UserNr)
							Else
								_ClsMA.OpenSelectedEmployee(viewData.MDNr, ModulConstants.UserData.UserNr)
							End If
						End If

					Case Else
						If viewData.ID > 0 Then

							Select Case viewData.ApplicationLifecycle
								Case ApplicationLifecycelEnum.APPLICATIONNEW
									viewData.ApplicationLifecycle = ApplicationLifecycelEnum.APPLICATIONVIEWED
									Dim success As Boolean = m_AppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData(viewData)
							End Select

							Dim frm = New frmApplicationDetail(m_InitializationData, viewData)
							frm.Show()
							frm.BringToFront()


						End If

				End Select

			End If

		End If

	End Sub

	Private Sub OnGVMain_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		RefreshMainViewStateBar()

	End Sub

	Private Sub OngvMain_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvMain.CustomUnboundColumnData

		If e.Column.Name = "docType" Then
			If (e.IsGetData()) Then
				'e.Value = CType(e.Row, MainViewApplicationData).image
			End If
		End If
	End Sub

#Region "DropDown Button für New und Print..."


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
				LoadData()
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

		' Care: Groupindex is from right to left!!!
		Select Case e.Button.Properties.GroupIndex
			Case 0
				RefreshData()

			Case 1
				pccMandant.SuspendLayout()
				Me.pccMandant.Manager = New DevExpress.XtraBars.BarManager
				pccMandant.ShowCloseButton = True
				pccMandant.ShowSizeGrip = True

				pccMandant.ShowPopup(Cursor.Position)
				pccMandant.ResumeLayout()

			Case 2
				Dim frmCVLSearch = New frmCVLSearch(m_InitializationData)

				frmCVLSearch.Show()
				frmCVLSearch.BringToFront()

			Case 3
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

			m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData,
																																						ModulConstants.ProsonalizedData,
																																						ModulConstants.MDData,
																																						ModulConstants.UserData)
			m_connString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)
			m_AppDatabaseAccess = New AppDatabaseAccess(m_connString, m_InitializationData.UserData.UserLanguage)

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
		LoadData()

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub RefreshMainViewStateBar()

		m_communicationHub.Publish(New RefreshMainViewStatebar(Me, Me.gvMain.RowCount, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))

	End Sub

#End Region


#Region "Helpers"

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

	Private Sub ChangeApplicationAdvisor(sender As Object, e As EventArgs)
		RunSelectedFilterOnRecords()
	End Sub

	Private Sub RunSelectedFilterOnRecords()

		Dim existData As BindingList(Of MainViewApplicationData) = m_CurrentApplicationData
		Dim newData As New List(Of MainViewApplicationData)

		Try
			' advisor property
			Select Case rgApplicationAdvisor.SelectedIndex
				Case 0
					' alle Bewerbungen
					newData = existData.ToList

				Case 1
					' nur meine Bewerbungen
					newData = existData.Where(Function(x) x.Advisor = m_InitializationData.UserData.UserGuid).ToList
					'grdMain.DataSource = data

				Case 2
					' spontan Bewerbungen
					newData = existData.Where(Function(x) x.Advisor Is Nothing).ToList
					'grdMain.DataSource = Data

				Case Else
					newData = existData.Where(Function(x) x.Advisor = m_InitializationData.UserData.UserGuid).ToList
					'grdMain.DataSource = data

			End Select

			' Application Lifecycel
			Select Case rgApplicationLifeCycel.SelectedIndex
				Case 0
					' alle
					'grdMain.DataSource = existData
				Case 1
					' neue
					newData = newData.Where(Function(x) x.ApplicationLifecycle = 0 Or x.ApplicationLifecycle = 3).ToList
							'grdMain.DataSource = data

				Case 2
					' in Bearbeitung
					newData = newData.Where(Function(x) x.ApplicationLifecycle = 2 Or x.ApplicationLifecycle = 3 Or x.ApplicationLifecycle = 4).ToList

				Case 3
					' Abgeschlossene
					newData = newData.Where(Function(x) x.ApplicationLifecycle = 1 Or x.ApplicationLifecycle = 5 Or x.ApplicationLifecycle = 6).ToList

			End Select

			grdMain.DataSource = newData
			grdMain.RefreshDataSource()


		Catch ex As Exception

		End Try

	End Sub


#End Region


#End Region


End Class
