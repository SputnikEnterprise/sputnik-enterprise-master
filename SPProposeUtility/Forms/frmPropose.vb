
Imports System.Windows.Forms
Imports System.Reflection.Assembly

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports System.IO
Imports System.ComponentModel
Imports DevExpress.XtraRichEdit.Services
Imports DevExpress.XtraRichEdit.Commands.Internal
Imports DevExpress.XtraRichEdit.Commands

Imports System.IO.File
Imports System.Data
Imports System.Data.SqlClient
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraBars.Alerter
Imports DevExpress.XtraBars
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraNavBar.ViewInfo
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraRichEdit.API.Native

Imports System.Threading
Imports DevExpress.XtraEditors.Controls

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SP.KD.KontaktMng.frmContacts

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP
Imports DevExpress.XtraEditors.Repository
Imports SP.KD.KontaktMng
Imports SP.MA.VorstellungMng.frmJobInterview
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Propose
Imports SP.DatabaseAccess.Propose.DataObjects
Imports DevExpress.XtraBars.ToastNotifications
Imports DevExpress.Utils
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports SP.DatabaseAccess.WOS
Imports DevExpress.Utils.VisualEffects
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports SP.DatabaseAccess.Common.DataObjects

Public Class frmPropose
	Inherits DevExpress.XtraEditors.XtraForm



#Region "Private Fields"


	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_ConnectionString As String

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	Private m_ProposeDatabaseAccess As DatabaseAccess.Propose.IProposeDatabaseAccess
	Private m_AppDatabaseAccess As IAppDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' Used to copy contacts for employees (Kandidaten)
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The wos data access object.
	''' </summary>
	Private m_WOSDatabaseAccess As IWOSDatabaseAccess

	Protected m_DataAccess As IDatabaseAccess
	Protected m_Utility As Utility
	Protected m_UtilityUI As UtilityUI
	Private m_mandant As Mandant
	Private m_common As CommonSetting
	Private m_path As ClsProgPath

	Private _ClsSetting As ClsProposeSetting

	Private _ClsFunc As ClsDivFunc


	Private strLinkName As String = String.Empty
	Private strLinkCaption As String = String.Empty
	Private bChangedContent As Boolean

	Private oFontName As Font '= New Font("Calibri", 11, FontStyle.Regular)
	Private MouseX As Integer
	Private MouseY As Integer

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private m_CheckEditImportant As RepositoryItemCheckEdit
	Private m_CheckEditCompleted As RepositoryItemCheckEdit

	Private m_JobInverviewDetailForm As SP.MA.VorstellungMng.frmJobInterview
	Private m_ContactCustomerDetailForm As SP.KD.KontaktMng.frmContacts
	Private m_ContactCustomerFilterSettingsForDetailForm As SP.MA.KontaktMng.ContactFilterSettings
	'Private m_ContactCustomerFilterSettingsForDetailForm As New SP.MA.KontaktMng.ContactFilterSettings

	Private m_ContactEmployeeDetailForm As SP.MA.KontaktMng.frmContacts
	Private m_ContactEmployeeFilterSettingsForDetailForm As SP.MA.KontaktMng.ContactFilterSettings

	Private m_GVEmployeeSettingfilename As String
	Private m_GVCustomerSettingfilename As String
	Private m_GVCResponsibleSettingfilename As String
	Private m_GVVacancySettingfilename As String
	Private m_AllowedDesign As Boolean

	Private m_MandantFormXMLFile As String


	Private m_MandantXMLFile As String
	Private m_StartNumberSetting As String
	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_AllowedDelete As Boolean
	Private m_DeleteButton As NavBarItem
	Private m_CurrentProposedata As ProposeMasterData
	Private m_Badge As DevExpress.Utils.VisualEffects.Badge


	Private m_SuppressUIEvents As Boolean = False


#End Region

#Region "private consts"

	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"
	Private Const MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING As String = "MD_{0}/StartNr"

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _PSetting As ClsProposeSetting)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		'_ClsSetting = New ClsProposeSetting

		m_InitializationData = _setting
		If ClsDataDetail.m_InitialData Is Nothing Then
			ClsDataDetail.m_InitialData = _setting
			ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		End If

		_ClsSetting = _PSetting

		_ClsFunc = New ClsDivFunc
		m_DataAccess = New DBAccess
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_mandant = New Mandant
		m_common = New CommonSetting
		m_path = New ClsProgPath

		m_SuppressUIEvents = True

		InitializeComponent()



		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ProposeDatabaseAccess = New ProposeDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_AppDatabaseAccess = New AppDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_WOSDatabaseAccess = New WOSDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)


		' Der User kann geändert werden.
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		ClsDataDetail.bAllowedTochangeKST = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 133, m_InitializationData.MDData.MDNr)
		m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 805, m_InitializationData.MDData.MDNr)
		m_AllowedDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 803, m_InitializationData.MDData.MDNr)

		Me.sccMain.Dock = DockStyle.Fill
		Me.sccDetails.Dock = DockStyle.Fill

		Me.pcNav.Dock = DockStyle.Fill
		Me.tbVorschlag.Dock = DockStyle.Fill
		Me.pcEditor.Dock = DockStyle.Fill

		Me.KeyPreview = True
		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me.rtfContent.Unit = DevExpress.Office.DocumentUnit.Centimeter ' DevExpress.XtraRichEdit.DocumentUnit.Centimeter
		Me.rtfContent.Document.Sections(0).Page.PaperKind = Printing.PaperKind.A4

		rtfContent.RemoveShortcutKey(Keys.Control, Keys.O)
		rtfContent.RemoveShortcutKey(Keys.Control, Keys.N)
		rtfContent.RemoveShortcutKey(Keys.Control, Keys.S)
		ReplaceRichEditCommandFactoryService(Me.rtfContent)

		CreateMyNavBar()

		m_SuppressUIEvents = False
		Try
			Dim GridSettingPath As String = String.Format("{0}ProposeUtiliy\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
			If Not Directory.Exists(GridSettingPath) Then Directory.CreateDirectory(GridSettingPath)

			If Not Directory.Exists(GridSettingPath & "Proposal\") Then Directory.CreateDirectory(GridSettingPath & "Proposal\")

			m_GVEmployeeSettingfilename = String.Format("{0}Proposal\{1}{2}.xml", GridSettingPath, gvEmployee.Name, m_InitializationData.UserData.UserNr)
			m_GVCustomerSettingfilename = String.Format("{0}Proposal\{1}{2}.xml", GridSettingPath, gvCustomer.Name, m_InitializationData.UserData.UserNr)
			m_GVCResponsibleSettingfilename = String.Format("{0}Proposal\{1}{2}.xml", GridSettingPath, gvlueCresponsible.Name, m_InitializationData.UserData.UserNr)
			m_GVVacancySettingfilename = String.Format("{0}Proposal\{1}{2}.xml", GridSettingPath, gvVacancy.Name, m_InitializationData.UserData.UserNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		Reset()

		LoadDropDownData()
		TranslateControls()

		Dim iIndent As Integer = CInt(Val(m_mandant.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Sonstiges", "LL_IndentSize", "20")))
		Dim ivalue As Integer = CInt(Val(m_mandant.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Sonstiges", "FontSize", "11")))
		Dim strValue As String = m_mandant.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Sonstiges", "FontName", "Calibri")

		oFontName = New Font(strValue, ivalue, FontStyle.Regular)
		Me.rtfContent.Font = CType(oFontName, System.Drawing.Font)
		bChangedContent = False

		AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployee.CloseUp, AddressOf OngvEmployeeColumnPositionChanged
		lueEmployee.Properties.AllowDropDownWhenReadOnly = DefaultBoolean.False

		AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCustomer.CloseUp, AddressOf OngvEmployeeColumnPositionChanged
		lueCustomer.Properties.AllowDropDownWhenReadOnly = DefaultBoolean.False

		AddHandler lueCresponsible.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCresponsible.CloseUp, AddressOf OngvEmployeeColumnPositionChanged
		lueCresponsible.Properties.AllowDropDownWhenReadOnly = DefaultBoolean.False

		AddHandler lueVacancy.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueVacancy.CloseUp, AddressOf OngvEmployeeColumnPositionChanged
		lueVacancy.Properties.AllowDropDownWhenReadOnly = DefaultBoolean.False

		AddHandler rtfContent.DocumentLoaded, AddressOf richEditControl1_DocumentLoaded

	End Sub

#End Region


#Region "Private Properties"

	Private ReadOnly Property ContactString(ByVal ContactFor As Integer) As String
		Get
			If ContactFor = 0 Then
				Return String.Format(m_Translate.GetSafeTranslationValue("Neuer Vorschlag als {0} erfasst"), txt_Bezeichnung.EditValue)

			ElseIf ContactFor = 1 Then
				Return String.Format(m_Translate.GetSafeTranslationValue("Status für Vorschlag {0} wurde geändert."), txt_Bezeichnung.EditValue)

			Else
				Return String.Format(m_Translate.GetSafeTranslationValue("Neuer Vorschlag als {0} erfasst"), txt_Bezeichnung.EditValue)

			End If
		End Get

	End Property

	Private ReadOnly Property DoNotAskForCreatingContact() As Boolean
		Get
			Dim notAskUser As Boolean = m_Utility.StringToBoolean(m_path.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/donotaskforcreatingcustomercontactinpropose", FORM_XML_DEFAULTVALUES_KEY)), False)
			Return notAskUser
		End Get
	End Property

	Private ReadOnly Property DoOpenContactFormForModification() As Boolean
		Get
			Dim openForm As Boolean = m_Utility.StringToBoolean(m_path.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/opencontactforminpropose", FORM_XML_DEFAULTVALUES_KEY)), False)
			Return openForm
		End Get
	End Property


#End Region


#Region "Form inizialisierer..."

	Private Sub ReplaceRichEditCommandFactoryService(ByVal control As DevExpress.XtraRichEdit.RichEditControl)
		Dim service As IRichEditCommandFactoryService = control.GetService(Of IRichEditCommandFactoryService)()

		If service Is Nothing Then Return
		control.RemoveService(GetType(IRichEditCommandFactoryService))
		control.AddService(GetType(IRichEditCommandFactoryService),
											 New CustomCommand.CustomRichEditCommandFactoryService(control, service))

	End Sub

#End Region

	Private Sub Reset()

		lblResult.Text = String.Empty
		lblTransferedOn.Text = String.Empty
		lblViewedOn.Text = String.Empty
		lblCustomerFeedback.Text = String.Empty

		txt_ArbBegin_Date.EditValue = Nothing
		txt_Ab_HBasis.EditValue = 0D
		txt_Ab_HAnsatz.EditValue = 0D
		txt_Ab_HBetrag.EditValue = 0D

		txt_Ab_LBasis.EditValue = 0D
		txt_Ab_LAnzahl.EditValue = 0D
		txt_Ab_LBetrag.EditValue = 0D
		txt_Ab_RePer_Date.EditValue = Nothing

		ResetMandantAndAdvisorDropDown()
		LoadMandantAndAdvisorDropDown()
		ResetDropDown()

	End Sub

	Private Sub ResetMandantAndAdvisorDropDown()

		ResetMandantenDropDown()
		ResetAdvisorDropDown()
		ResetSecAdvisorDropDown()

		ResetInterviewGrid()
		ResetEmployeeContact()
		ResetCustomerContact()

	End Sub

	Private Sub LoadMandantAndAdvisorDropDown()

		LoadMandantenDropDown()
		'LoadAdvisorDropDownData()
		'LoadSecAdvisorDropDownData()

	End Sub

	Public Sub ResetDropDown()

		ResetEmployeeDropDown()
		ResetEmployeeDetailData()

		ResetCustomerDropDown()
		ResetResponsiblePersonDropDown()
		ResetVacancyDropDown()

	End Sub

	Sub LoadDropDownData()

		LoadEmployeeDropDownData()
		LoadCustomerDropDownData()
		LoadcResponsibleDropDownData(0)
		LoadVacancyDropDownData(0)

	End Sub

#Region "DropDown-Data..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData) ' MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		ResetDropDown()
		LoadDropDownData()

	End Sub


	Private Sub ResetEmployeeDropDown()

		lueEmployee.Properties.DisplayMember = "LastnameFirstname"
		lueEmployee.Properties.ValueMember = "EmployeeNumber"

		gvEmployee.OptionsView.ShowIndicator = False
		gvEmployee.OptionsView.ShowColumnHeaders = True
		gvEmployee.OptionsView.ShowFooter = False

		gvEmployee.OptionsView.ShowAutoFilterRow = True
		gvEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvEmployee.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = True
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnEmployeeNumber)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnLastnameFirstname.Name = "LastnameFirstname"
		columnLastnameFirstname.FieldName = "LastnameFirstname"
		columnLastnameFirstname.Visible = True
		columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnLastnameFirstname)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnPostcodeAndLocation)

		Dim columnFState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFState.Caption = m_Translate.GetSafeTranslationValue("MA1Status", True)
		columnFState.Name = "fstate"
		columnFState.FieldName = "fstate"
		columnFState.Visible = False
		columnFState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvEmployee.Columns.Add(columnFState)

		Dim columnDStellen As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDStellen.Caption = m_Translate.GetSafeTranslationValue("Dauerstelle", True)
		columnDStellen.Name = "DStellen"
		columnDStellen.FieldName = "DStellen"
		columnDStellen.Visible = False
		gvEmployee.Columns.Add(columnDStellen)

		Dim columnNoES As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNoES.Caption = m_Translate.GetSafeTranslationValue("Einsatzsperre", True)
		columnNoES.Name = "NoES"
		columnNoES.FieldName = "NoES"
		columnNoES.Visible = False
		gvEmployee.Columns.Add(columnNoES)


		lueEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployee.Properties.NullText = String.Empty
		lueEmployee.EditValue = Nothing

		If File.Exists(m_GVEmployeeSettingfilename) Then gvEmployee.RestoreLayoutFromXml(m_GVEmployeeSettingfilename)

	End Sub

	''' <summary>
	''' Resets the customer drop down.
	''' </summary>
	Private Sub ResetCustomerDropDown()

		lueCustomer.Properties.DisplayMember = "Company1"
		lueCustomer.Properties.ValueMember = "CustomerNumber"

		gvCustomer.OptionsView.ShowIndicator = False
		gvCustomer.OptionsView.ShowColumnHeaders = True
		gvCustomer.OptionsView.ShowFooter = False

		gvCustomer.OptionsView.ShowAutoFilterRow = True
		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCustomer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		gvCustomer.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnPostcodeAndLocation)

		Dim columnFstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFstate.Caption = m_Translate.GetSafeTranslationValue("KD1Status", True)
		columnFstate.Name = "fstate"
		columnFstate.FieldName = "fstate"
		columnFstate.Visible = False
		columnFstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnFstate)

		Dim columnsstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsstate.Caption = m_Translate.GetSafeTranslationValue("KD2Status", True)
		columnsstate.Name = "sstate"
		columnsstate.FieldName = "sstate"
		columnsstate.Visible = False
		columnsstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnsstate)

		Dim columnHowcontact As New DevExpress.XtraGrid.Columns.GridColumn()
		columnHowcontact.Caption = m_Translate.GetSafeTranslationValue("KDKontakt", True)
		columnHowcontact.Name = "howcontact"
		columnHowcontact.FieldName = "howcontact"
		columnHowcontact.Visible = False
		columnHowcontact.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCustomer.Columns.Add(columnHowcontact)

		Dim columnNoES As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNoES.Caption = m_Translate.GetSafeTranslationValue("Einsatzsperre", True)
		columnNoES.Name = "noes"
		columnNoES.FieldName = "noes"
		columnNoES.Visible = False
		gvCustomer.Columns.Add(columnNoES)


		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomer.Properties.NullText = String.Empty
		lueCustomer.EditValue = Nothing

		If File.Exists(m_GVCustomerSettingfilename) Then gvCustomer.RestoreLayoutFromXml(m_GVCustomerSettingfilename)

	End Sub

	''' <summary>
	''' Resets the responsible person drop down.
	''' </summary>
	Private Sub ResetResponsiblePersonDropDown()

		lueCresponsible.Properties.DisplayMember = "LastNameFirstName"
		lueCresponsible.Properties.ValueMember = "RecordNumber"

		gvlueCresponsible.OptionsView.ShowIndicator = False
		gvlueCresponsible.OptionsView.ShowColumnHeaders = True
		gvlueCresponsible.OptionsView.ShowFooter = False

		gvlueCresponsible.OptionsView.ShowAutoFilterRow = True
		gvlueCresponsible.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvlueCresponsible.Columns.Clear()

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "RecordNumber"
		columnRecordNumber.FieldName = "RecordNumber"
		columnRecordNumber.Visible = False
		gvlueCresponsible.Columns.Add(columnRecordNumber)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnName.Name = "LastNameFirstName"
		columnName.FieldName = "LastNameFirstName"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCresponsible.Columns.Add(columnName)

		Dim columnfstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfstate.Caption = m_Translate.GetSafeTranslationValue("KD1Status", True)
		columnfstate.Name = "fstate"
		columnfstate.FieldName = "fstate"
		columnfstate.Visible = True
		columnfstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCresponsible.Columns.Add(columnfstate)

		Dim columnsstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsstate.Caption = m_Translate.GetSafeTranslationValue("KD2Status", True)
		columnsstate.Name = "sstate"
		columnsstate.FieldName = "sstate"
		columnsstate.Visible = False
		columnsstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCresponsible.Columns.Add(columnsstate)

		Dim columnPosition As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPosition.Caption = m_Translate.GetSafeTranslationValue("Position")
		columnPosition.Name = "Position"
		columnPosition.FieldName = "Position"
		columnPosition.Visible = True
		columnPosition.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCresponsible.Columns.Add(columnPosition)

		Dim columnDepartment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDepartment.Caption = m_Translate.GetSafeTranslationValue("Abteilung")
		columnDepartment.Name = "Department"
		columnDepartment.FieldName = "Department"
		columnDepartment.Visible = True
		columnDepartment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCresponsible.Columns.Add(columnDepartment)

		Dim columnContact As New DevExpress.XtraGrid.Columns.GridColumn()
		columnContact.Caption = m_Translate.GetSafeTranslationValue("KDKontakt")
		columnContact.Name = "howcontact"
		columnContact.FieldName = "howcontact"
		columnContact.Visible = True
		columnContact.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvlueCresponsible.Columns.Add(columnContact)


		lueCresponsible.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCresponsible.Properties.NullText = String.Empty
		lueCresponsible.EditValue = Nothing

		If File.Exists(m_GVCResponsibleSettingfilename) Then gvlueCresponsible.RestoreLayoutFromXml(m_GVCResponsibleSettingfilename)

	End Sub

	''' <summary>
	''' Resets the Vacancy drop down.
	''' </summary>
	Private Sub ResetVacancyDropDown()

		lueVacancy.Properties.DisplayMember = "vacancybez"
		lueVacancy.Properties.ValueMember = "vacancynumber"

		gvVacancy.OptionsView.ShowIndicator = False
		gvVacancy.OptionsView.ShowColumnHeaders = True
		gvVacancy.OptionsView.ShowFooter = False

		gvVacancy.OptionsView.ShowAutoFilterRow = True
		gvVacancy.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvVacancy.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "vacancynumber"
		columnCustomerNumber.FieldName = "vacancynumber"
		columnCustomerNumber.Visible = True
		gvVacancy.Columns.Add(columnCustomerNumber)

		Dim columnVBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnVBezeichnung.Name = "vacancybez"
		columnVBezeichnung.FieldName = "vacancybez"
		columnVBezeichnung.Visible = True
		columnVBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvVacancy.Columns.Add(columnVBezeichnung)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "street"
		columnStreet.FieldName = "street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvVacancy.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvVacancy.Columns.Add(columnPostcodeAndLocation)

		Dim columnVState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVState.Caption = m_Translate.GetSafeTranslationValue("Status")
		columnVState.Name = "vacancystate"
		columnVState.FieldName = "vacancystate"
		columnVState.Visible = True
		columnVState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvVacancy.Columns.Add(columnVState)

		Dim columnCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCreatedon.Name = "createdon"
		columnCreatedon.FieldName = "createdon"
		columnCreatedon.Visible = True
		gvVacancy.Columns.Add(columnCreatedon)

		lueVacancy.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueVacancy.Properties.NullText = String.Empty
		lueVacancy.EditValue = Nothing

		If File.Exists(m_GVVacancySettingfilename) Then gvVacancy.RestoreLayoutFromXml(m_GVVacancySettingfilename)

	End Sub


	Private Sub ResetCopyEmployeeDropDown()

		lueCopyEmployee.Properties.DisplayMember = "LastnameFirstname"
		lueCopyEmployee.Properties.ValueMember = "EmployeeNumber"

		gvCopyEmployee.OptionsView.ShowIndicator = False
		gvCopyEmployee.OptionsView.ShowColumnHeaders = True
		gvCopyEmployee.OptionsView.ShowFooter = False

		gvCopyEmployee.OptionsView.ShowAutoFilterRow = True
		gvCopyEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCopyEmployee.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = True
		columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyEmployee.Columns.Add(columnEmployeeNumber)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnLastnameFirstname.Name = "LastnameFirstname"
		columnLastnameFirstname.FieldName = "LastnameFirstname"
		columnLastnameFirstname.Visible = True
		columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyEmployee.Columns.Add(columnLastnameFirstname)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyEmployee.Columns.Add(columnPostcodeAndLocation)

		Dim columnFState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFState.Caption = m_Translate.GetSafeTranslationValue("MA1Status", True)
		columnFState.Name = "fstate"
		columnFState.FieldName = "fstate"
		columnFState.Visible = False
		columnFState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyEmployee.Columns.Add(columnFState)

		Dim columnDStellen As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDStellen.Caption = m_Translate.GetSafeTranslationValue("Dauerstelle", True)
		columnDStellen.Name = "DStellen"
		columnDStellen.FieldName = "DStellen"
		columnDStellen.Visible = False
		gvCopyEmployee.Columns.Add(columnDStellen)

		Dim columnNoES As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNoES.Caption = m_Translate.GetSafeTranslationValue("Einsatzsperre", True)
		columnNoES.Name = "NoES"
		columnNoES.FieldName = "NoES"
		columnNoES.Visible = False
		gvCopyEmployee.Columns.Add(columnNoES)


		lueCopyEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCopyEmployee.Properties.NullText = String.Empty
		lueCopyEmployee.EditValue = Nothing

		If File.Exists(m_GVEmployeeSettingfilename) Then gvCopyEmployee.RestoreLayoutFromXml(m_GVEmployeeSettingfilename)

	End Sub

	''' <summary>
	''' Resets the customer drop down.
	''' </summary>
	Private Sub ResetCopyCustomerDropDown()

		lueCopyCustomer.Properties.DisplayMember = "Company1"
		lueCopyCustomer.Properties.ValueMember = "CustomerNumber"

		gvCopyCustomer.OptionsView.ShowIndicator = False
		gvCopyCustomer.OptionsView.ShowColumnHeaders = True
		gvCopyCustomer.OptionsView.ShowFooter = False

		gvCopyCustomer.OptionsView.ShowAutoFilterRow = True
		gvCopyCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCopyCustomer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		gvCopyCustomer.Columns.Add(columnCustomerNumber)

		Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
		columnCompany1.Name = "Company1"
		columnCompany1.FieldName = "Company1"
		columnCompany1.Visible = True
		columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyCustomer.Columns.Add(columnCompany1)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "Street"
		columnStreet.FieldName = "Street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyCustomer.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyCustomer.Columns.Add(columnPostcodeAndLocation)

		Dim columnFstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFstate.Caption = m_Translate.GetSafeTranslationValue("KD1Status", True)
		columnFstate.Name = "fstate"
		columnFstate.FieldName = "fstate"
		columnFstate.Visible = False
		columnFstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyCustomer.Columns.Add(columnFstate)

		Dim columnsstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsstate.Caption = m_Translate.GetSafeTranslationValue("KD2Status", True)
		columnsstate.Name = "sstate"
		columnsstate.FieldName = "sstate"
		columnsstate.Visible = False
		columnsstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyCustomer.Columns.Add(columnsstate)

		Dim columnHowcontact As New DevExpress.XtraGrid.Columns.GridColumn()
		columnHowcontact.Caption = m_Translate.GetSafeTranslationValue("KDKontakt", True)
		columnHowcontact.Name = "howcontact"
		columnHowcontact.FieldName = "howcontact"
		columnHowcontact.Visible = False
		columnHowcontact.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyCustomer.Columns.Add(columnHowcontact)

		Dim columnNoES As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNoES.Caption = m_Translate.GetSafeTranslationValue("Einsatzsperre", True)
		columnNoES.Name = "noes"
		columnNoES.FieldName = "noes"
		columnNoES.Visible = False
		gvCopyCustomer.Columns.Add(columnNoES)


		lueCopyCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCopyCustomer.Properties.NullText = String.Empty
		lueCopyCustomer.EditValue = Nothing

		If File.Exists(m_GVCustomerSettingfilename) Then gvCopyCustomer.RestoreLayoutFromXml(m_GVCustomerSettingfilename)

	End Sub

	''' <summary>
	''' Resets the responsible person drop down.
	''' </summary>
	Private Sub ResetCopyResponsiblePersonDropDown()

		lueCopyCresponsible.Properties.DisplayMember = "LastNameFirstName"
		lueCopyCresponsible.Properties.ValueMember = "RecordNumber"

		gvCopylueCresponsible.OptionsView.ShowIndicator = False
		gvCopylueCresponsible.OptionsView.ShowColumnHeaders = True
		gvCopylueCresponsible.OptionsView.ShowFooter = False

		gvCopylueCresponsible.OptionsView.ShowAutoFilterRow = True
		gvCopylueCresponsible.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCopylueCresponsible.Columns.Clear()

		Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecordNumber.Name = "RecordNumber"
		columnRecordNumber.FieldName = "RecordNumber"
		columnRecordNumber.Visible = False
		gvCopylueCresponsible.Columns.Add(columnRecordNumber)

		Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
		columnName.Name = "LastNameFirstName"
		columnName.FieldName = "LastNameFirstName"
		columnName.Visible = True
		columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopylueCresponsible.Columns.Add(columnName)

		Dim columnfstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfstate.Caption = m_Translate.GetSafeTranslationValue("KD1Status", True)
		columnfstate.Name = "fstate"
		columnfstate.FieldName = "fstate"
		columnfstate.Visible = True
		columnfstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopylueCresponsible.Columns.Add(columnfstate)

		Dim columnsstate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnsstate.Caption = m_Translate.GetSafeTranslationValue("KD2Status", True)
		columnsstate.Name = "sstate"
		columnsstate.FieldName = "sstate"
		columnsstate.Visible = False
		columnsstate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopylueCresponsible.Columns.Add(columnsstate)

		Dim columnPosition As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPosition.Caption = m_Translate.GetSafeTranslationValue("Position")
		columnPosition.Name = "Position"
		columnPosition.FieldName = "Position"
		columnPosition.Visible = True
		columnPosition.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopylueCresponsible.Columns.Add(columnPosition)

		Dim columnDepartment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDepartment.Caption = m_Translate.GetSafeTranslationValue("Abteilung")
		columnDepartment.Name = "Department"
		columnDepartment.FieldName = "Department"
		columnDepartment.Visible = True
		columnDepartment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopylueCresponsible.Columns.Add(columnDepartment)

		Dim columnContact As New DevExpress.XtraGrid.Columns.GridColumn()
		columnContact.Caption = m_Translate.GetSafeTranslationValue("KDKontakt")
		columnContact.Name = "howcontact"
		columnContact.FieldName = "howcontact"
		columnContact.Visible = True
		columnContact.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopylueCresponsible.Columns.Add(columnContact)


		lueCopyCresponsible.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCopyCresponsible.Properties.NullText = String.Empty
		lueCopyCresponsible.EditValue = Nothing

		If File.Exists(m_GVCResponsibleSettingfilename) Then gvCopylueCresponsible.RestoreLayoutFromXml(m_GVCResponsibleSettingfilename)

	End Sub

	''' <summary>
	''' Resets the Vacancy drop down.
	''' </summary>
	Private Sub ResetCopyVacancyDropDown()

		lueCopyVacancy.Properties.DisplayMember = "vacancybez"
		lueCopyVacancy.Properties.ValueMember = "vacancynumber"

		gvCopyVacancy.OptionsView.ShowIndicator = False
		gvCopyVacancy.OptionsView.ShowColumnHeaders = True
		gvCopyVacancy.OptionsView.ShowFooter = False

		gvCopyVacancy.OptionsView.ShowAutoFilterRow = True
		gvCopyVacancy.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCopyVacancy.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnCustomerNumber.Name = "vacancynumber"
		columnCustomerNumber.FieldName = "vacancynumber"
		columnCustomerNumber.Visible = True
		gvCopyVacancy.Columns.Add(columnCustomerNumber)

		Dim columnVBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnVBezeichnung.Name = "vacancybez"
		columnVBezeichnung.FieldName = "vacancybez"
		columnVBezeichnung.Visible = True
		columnVBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyVacancy.Columns.Add(columnVBezeichnung)

		Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnStreet.Name = "street"
		columnStreet.FieldName = "street"
		columnStreet.Visible = True
		columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyVacancy.Columns.Add(columnStreet)

		Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnPostcodeAndLocation.Name = "PostcodeAndLocation"
		columnPostcodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostcodeAndLocation.Visible = True
		columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyVacancy.Columns.Add(columnPostcodeAndLocation)

		Dim columnVState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVState.Caption = m_Translate.GetSafeTranslationValue("Status")
		columnVState.Name = "vacancystate"
		columnVState.FieldName = "vacancystate"
		columnVState.Visible = True
		columnVState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvCopyVacancy.Columns.Add(columnVState)

		Dim columnCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columnCreatedon.Name = "createdon"
		columnCreatedon.FieldName = "createdon"
		columnCreatedon.Visible = True
		gvCopyVacancy.Columns.Add(columnCreatedon)

		lueCopyVacancy.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCopyVacancy.Properties.NullText = String.Empty
		lueCopyVacancy.EditValue = Nothing

		If File.Exists(m_GVVacancySettingfilename) Then gvCopyVacancy.RestoreLayoutFromXml(m_GVVacancySettingfilename)

	End Sub

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData() As Boolean

		Dim employeeData = m_DataAccess.LoadEmployeeData()

		If employeeData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten können nicht geladen werden."))
			Return False
		End If

		lueEmployee.Properties.DataSource = employeeData

		Return True

	End Function

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim customerData = m_DataAccess.LoadCustomerData()

		If customerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten können nicht geladen werden."))
			Return False
		End If

		lueCustomer.Properties.DataSource = customerData

		Return True

	End Function

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadcResponsibleDropDownData(ByVal customerNumber As Integer) As Boolean

		If customerNumber = 0 Then
			lueCresponsible.EditValue = Nothing
			lueCresponsible.Properties.DataSource = Nothing
			Return True
		End If

		Dim responsiblePersonData = m_DataAccess.LoadResponsiblePersonData(customerNumber)

		If responsiblePersonData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuständige Personen können nicht geladen werden."))
			Return False
		End If

		lueCresponsible.Properties.DataSource = responsiblePersonData

		Return True

	End Function

	''' <summary>
	''' Loads the vacancy drop down data.
	''' </summary>
	Private Function LoadVacancyDropDownData(ByVal customerNumber As Integer) As Boolean
		If customerNumber = 0 Then Return False
		Dim vacancyData = m_DataAccess.LoadVacancyData(customerNumber)

		If vacancyData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanzen konnen nicht geladen werden."))
			Return False
		End If

		lueVacancy.Properties.DataSource = vacancyData

		Return True

	End Function

	Private Function LoadCopycResponsibleDropDownData(ByVal customerNumber As Integer) As Boolean

		If customerNumber = 0 Then
			lueCopyCresponsible.EditValue = Nothing
			lueCopyCresponsible.Properties.DataSource = Nothing

			Return True
		End If

		Dim responsiblePersonData = m_DataAccess.LoadResponsiblePersonData(customerNumber)

		If responsiblePersonData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuständige Personen können nicht geladen werden."))
			Return False
		End If

		lueCopyCresponsible.Properties.DataSource = responsiblePersonData

		Return True

	End Function

	Private Function LoadCopyVacancyDropDownData(ByVal customerNumber As Integer) As Boolean
		If customerNumber = 0 Then Return False
		Dim vacancyData = m_DataAccess.LoadVacancyData(customerNumber)

		If vacancyData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vakanzen konnen nicht geladen werden."))
			Return False
		End If

		lueCopyVacancy.Properties.DataSource = vacancyData

		Return True

	End Function





	''' <summary>
	''' Resets the advisors drop down.
	''' </summary>
	Private Sub ResetAdvisorDropDown()

		lueAdvisor.Properties.DisplayMember = "LastName_FirstName"
		lueAdvisor.Properties.ValueMember = "KST"

		Dim columns = lueAdvisor.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("KST", 0))
		columns.Add(New LookUpColumnInfo("LastName_FirstName", 0, "Name"))

		lueAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueAdvisor.Properties.SearchMode = SearchMode.AutoComplete
		lueAdvisor.Properties.AutoSearchColumnIndex = 1

		lueAdvisor.Properties.NullText = String.Empty
		lueAdvisor.EditValue = Nothing

	End Sub

	Private Function LoadAdvisorDropDownData() As Boolean
		'Dim advisorData = m_DataAccess.LoadAdvisorData()

		Dim advisorData As IEnumerable(Of AdvisorData) '= m_CommonDatabaseAccess.LoadAdvisorData()
		If _ClsSetting.SelectedProposeNr.GetValueOrDefault(0) = 0 Then
			advisorData = m_CommonDatabaseAccess.LoadActivatedAdvisorData()
		Else
			advisorData = m_CommonDatabaseAccess.LoadAllAdvisorsData()
		End If


		Dim advisorViewDataList As New List(Of AdvisorViewData)

		If Not advisorData Is Nothing Then
			For Each advisor In advisorData
				Dim advisorViewData As AdvisorViewData = New AdvisorViewData
				advisorViewData.UserNumber = advisor.UserNumber
				advisorViewData.KST = advisor.KST
				advisorViewData.Salutation = advisor.Salutation
				advisorViewData.FristName = advisor.Firstname
				advisorViewData.LastName = advisor.Lastname

				advisorViewDataList.Add(advisorViewData)
			Next
		Else
			m_UtilityUI.ShowErrorDialog("Beraterdaten konnten nicht geladen werden.")
		End If

		lueAdvisor.Properties.DataSource = advisorViewDataList
		lueAdvisor.Properties.ForceInitialize()

		Return Not advisorData Is Nothing
	End Function

	Private Sub ResetSecAdvisorDropDown()

		lueSecAdvisor.Properties.DisplayMember = "LastName_FirstName"
		lueSecAdvisor.Properties.ValueMember = "KST"

		Dim columns = lueSecAdvisor.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("KST", 0))
		columns.Add(New LookUpColumnInfo("LastName_FirstName", 0, "Name"))

		lueSecAdvisor.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueSecAdvisor.Properties.SearchMode = SearchMode.AutoComplete
		lueSecAdvisor.Properties.AutoSearchColumnIndex = 1

		lueSecAdvisor.Properties.NullText = String.Empty
		lueSecAdvisor.EditValue = Nothing

	End Sub

	Private Function LoadSecAdvisorDropDownData() As Boolean

		lueSecAdvisor.Properties.DataSource = lueAdvisor.Properties.DataSource
		lueSecAdvisor.Properties.ForceInitialize()

		Return (True)
	End Function

	''' <summary>
	''' Handles change of customer.
	''' </summary>
	Private Sub OnLueCustomer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomer.EditValueChanged

		If Not lueCustomer.EditValue Is Nothing Then
			Dim customerNumber As Integer = CInt(Val(lueCustomer.EditValue))
			grpCustomer.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde") & ": {0}", lueCustomer.EditValue)

			ResetResponsiblePersonDropDown()
			LoadcResponsibleDropDownData(customerNumber)

			ResetVacancyDropDown()
			LoadVacancyDropDownData(customerNumber)

		Else

			ResetResponsiblePersonDropDown()
			ResetVacancyDropDown()

		End If

	End Sub

	Private Sub OnlueEmployee_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueEmployee.EditValueChanged

		If Not lueEmployee.EditValue Is Nothing Then

			Dim employeeNumber As Integer = lueEmployee.EditValue
			Dim employeeMasterData = m_DataAccess.LoadEmployeeMasterData(employeeNumber, False)
			Dim employeeContactCommData As EmployeeContactComm = m_DataAccess.LoadEmployeeContactCommData(employeeNumber)

			If employeeMasterData Is Nothing Or
				employeeContactCommData Is Nothing Then

				ResetEmployeeDetailData()

				If employeeMasterData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeitestammdaten konnten nicht geladen werden."))
				End If

				If employeeContactCommData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter (KontaktKomm) konnten nicht geladen werden."))
				End If

			Else

				DisplayEmployeeData(employeeMasterData, employeeContactCommData)
			End If

		Else
			ResetEmployeeDetailData()

		End If

	End Sub

	Private Sub OnlueVacancy_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueVacancy.EditValueChanged

		If Not lueVacancy.EditValue Is Nothing Then
			Dim vacancyNumber As Integer = lueVacancy.EditValue

			Dim vacancyMasterData = m_DataAccess.LoadVacancyMasterData(vacancyNumber)
			If String.IsNullOrWhiteSpace(txt_Bezeichnung.Text) AndAlso Not vacancyMasterData Is Nothing Then
				txt_Bezeichnung.Text = vacancyMasterData.vacancybez
			End If

		End If

	End Sub


#Region "copying data"

	Private Sub OnLueCopyCustomer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCopyCustomer.EditValueChanged

		If Not lueCopyCustomer.EditValue Is Nothing Then
			Dim customerNumber As Integer = CInt(Val(lueCopyCustomer.EditValue))

			ResetResponsiblePersonDropDown()
			LoadCopycResponsibleDropDownData(customerNumber)

			ResetCopyVacancyDropDown()
			LoadCopyVacancyDropDownData(customerNumber)

		Else

			ResetCopyResponsiblePersonDropDown()
			ResetCopyVacancyDropDown()

		End If

	End Sub

	Private Sub OnlueCopyVacancy_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueCopyVacancy.EditValueChanged

		If Not lueCopyVacancy.EditValue Is Nothing Then
			Dim vacancyNumber As Integer = lueCopyVacancy.EditValue

			Dim vacancyMasterData = m_DataAccess.LoadVacancyMasterData(vacancyNumber)
			If String.IsNullOrWhiteSpace(txt_Bezeichnung.Text) AndAlso Not vacancyMasterData Is Nothing Then
				txt_Bezeichnung.Text = vacancyMasterData.vacancybez
			End If

		End If

	End Sub


#End Region


	''' <summary>
	''' Displays employee detail data.
	''' </summary>
	''' <param name="employeeMasterData">The employee master data.</param>
	''' <param name="employeeContactCommData">The employee contact comm data.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function DisplayEmployeeData(ByVal employeeMasterData As EmployeeMasterData, ByVal employeeContactCommData As EmployeeContactComm) As Boolean

		' Birthdate and age
		If employeeMasterData.Birthdate.HasValue Then
			lblBirthdateValue.Text = String.Format("{0:dd.MM.yyyy} ({1})", employeeMasterData.Birthdate.Value, GetAge(employeeMasterData.Birthdate.Value))
		Else
			lblBirthdateValue.Text = "-"
		End If
		grpEmployee.Text = String.Format(m_Translate.GetSafeTranslationValue("Kandidat") & ": {0}", employeeMasterData.EmployeeNumber)
		' Address
		lblEmployeeAddressValue.Text = String.Format("{0}, {1} {2}", employeeMasterData.Street, employeeMasterData.Postcode, employeeMasterData.Location)

		' Qualification
		lblQualificationValue.Text = employeeMasterData.Profession

		' MA State
		lblMAStateValue.Text = If(String.IsNullOrWhiteSpace(employeeContactCommData.KStat1), "-", employeeContactCommData.KStat1)

		Return True
	End Function

	''' <summary>
	''' Resets employee detail data.
	''' </summary>
	Private Sub ResetEmployeeDetailData()

		grpEmployee.Text = String.Format(m_Translate.GetSafeTranslationValue("Kandidat") & ": ?")
		lblBirthdateValue.Text = String.Empty
		lblEmployeeAddressValue.Text = String.Empty
		lblQualificationValue.Text = String.Empty
		lblMAStateValue.Text = String.Empty

	End Sub


#Region "Grid Methods"

	''' <summary>
	''' Resets the interview grid.
	''' </summary>
	Private Sub ResetInterviewGrid()

		' Reset the grid
		gvInterview.OptionsView.ShowIndicator = False

		gvInterview.Columns.Clear()

		Dim columnAppointmentDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAppointmentDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnAppointmentDate.Name = "AppointmentDate"
		columnAppointmentDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnAppointmentDate.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm"
		columnAppointmentDate.FieldName = "AppointmentDate"
		columnAppointmentDate.Visible = True
		gvInterview.Columns.Add(columnAppointmentDate)

		Dim columnJobTitle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobTitle.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnJobTitle.Name = "JobTitle"
		columnJobTitle.FieldName = "JobTitle"
		columnJobTitle.Visible = True
		gvInterview.Columns.Add(columnJobTitle)

		Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCompany.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCompany.Name = "Company"
		columnCompany.FieldName = "Company"
		columnCompany.Visible = True
		gvInterview.Columns.Add(columnCompany)

		Dim columnJobAppointmentState As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobAppointmentState.Caption = m_Translate.GetSafeTranslationValue("Status")
		columnJobAppointmentState.Name = "JobAppointmentState"
		columnJobAppointmentState.FieldName = "JobAppointmentState"
		columnJobAppointmentState.Visible = True
		gvInterview.Columns.Add(columnJobAppointmentState)

		gridInterview.DataSource = Nothing

	End Sub


	Private Sub ResetEmployeeContact()

		gridViewEmployeeContactData.Columns.Clear()

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnDate.Name = "ContactDate"
		columnDate.FieldName = "ContactDate"
		columnDate.Visible = True
		columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gridViewEmployeeContactData.Columns.Add(columnDate)

		Dim personSubjectColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		personSubjectColumn.Caption = m_Translate.GetSafeTranslationValue("Person / Betreff")
		personSubjectColumn.Name = "Person_Subject"
		personSubjectColumn.FieldName = "Person_Subject"
		personSubjectColumn.Visible = True
		gridViewEmployeeContactData.Columns.Add(personSubjectColumn)

		Dim descriptionColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		descriptionColumn.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
		descriptionColumn.Name = "Description"
		descriptionColumn.FieldName = "Description"
		descriptionColumn.Visible = True
		descriptionColumn.Width = 200
		gridViewEmployeeContactData.Columns.Add(descriptionColumn)

		Dim importantColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		importantColumn.Caption = m_Translate.GetSafeTranslationValue("Wichtig")
		importantColumn.Name = "Important"
		importantColumn.FieldName = "Important"
		importantColumn.Visible = True
		importantColumn.ColumnEdit = m_CheckEditImportant
		gridViewEmployeeContactData.Columns.Add(importantColumn)

		Dim completedColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		completedColumn.Caption = m_Translate.GetSafeTranslationValue("Erledigt")
		completedColumn.Name = "Completed"
		completedColumn.FieldName = "Completed"
		completedColumn.Visible = True
		completedColumn.ColumnEdit = m_CheckEditCompleted
		gridViewEmployeeContactData.Columns.Add(completedColumn)

		Dim kstColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		kstColumn.Caption = m_Translate.GetSafeTranslationValue("Erstellt von")
		kstColumn.Name = "CreatedFrom"
		kstColumn.FieldName = "CreatedFrom"
		kstColumn.Visible = True
		gridViewEmployeeContactData.Columns.Add(kstColumn)

		Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
		docType.Caption = " "
		docType.Name = "docType"
		docType.FieldName = "docType"
		docType.Visible = True

		Dim picutureEdit As New RepositoryItemPictureEdit()
		picutureEdit.NullText = " "
		docType.ColumnEdit = picutureEdit
		docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
		docType.Width = 20
		gridViewEmployeeContactData.Columns.Add(docType)

	End Sub

	Private Sub ResetCustomerContact()

		' Reset the grid
		gridViewCustomerContactData.Columns.Clear()

		Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
		columnDate.Name = "ContactDate"
		columnDate.FieldName = "ContactDate"
		columnDate.Visible = True
		columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		gridViewCustomerContactData.Columns.Add(columnDate)

		Dim personSubjectColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		personSubjectColumn.Caption = m_Translate.GetSafeTranslationValue("Person / Betreff")
		personSubjectColumn.Name = "Person_Subject"
		personSubjectColumn.FieldName = "Person_Subject"
		personSubjectColumn.Visible = True
		gridViewCustomerContactData.Columns.Add(personSubjectColumn)

		Dim descriptionColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		descriptionColumn.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
		descriptionColumn.Name = "Description"
		descriptionColumn.FieldName = "Description"
		descriptionColumn.Visible = True
		descriptionColumn.Width = 200
		gridViewCustomerContactData.Columns.Add(descriptionColumn)

		Dim importantColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		importantColumn.Caption = m_Translate.GetSafeTranslationValue("Wichtig")
		importantColumn.Name = "Important"
		importantColumn.FieldName = "Important"
		importantColumn.Visible = True
		importantColumn.ColumnEdit = m_CheckEditImportant
		gridViewCustomerContactData.Columns.Add(importantColumn)

		Dim completedColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		completedColumn.Caption = m_Translate.GetSafeTranslationValue("Erledigt")
		completedColumn.Name = "Completed"
		completedColumn.FieldName = "Completed"
		completedColumn.Visible = True
		completedColumn.ColumnEdit = m_CheckEditCompleted
		gridViewCustomerContactData.Columns.Add(completedColumn)

		Dim kstColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		kstColumn.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
		kstColumn.Name = "Creator"
		kstColumn.FieldName = "Creator"
		kstColumn.Visible = True
		gridViewCustomerContactData.Columns.Add(kstColumn)

		Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
		docType.Caption = " "
		docType.Name = "docType"
		docType.FieldName = "docType"
		docType.Visible = True
		Dim picutureEdit As New RepositoryItemPictureEdit()
		picutureEdit.NullText = " "
		docType.ColumnEdit = picutureEdit
		docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
		docType.Width = 20
		gridViewCustomerContactData.Columns.Add(docType)

	End Sub


	''' <summary>
	''' Loads employee interview data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeInterviewData(ByVal employeeNumber As Integer)

		Dim employeeJobInterviews = m_EmployeeDatabaseAccess.LoadEmployeeJobInterviewsForPropose(employeeNumber, _ClsSetting.SelectedProposeNr)

		If (employeeJobInterviews Is Nothing) Then
			Return False
		End If

		Dim listDataSource As BindingList(Of JobInterviewViewData) = New BindingList(Of JobInterviewViewData)

		' Convert the data to view data.
		For Each interview In employeeJobInterviews

			Dim viewData = New JobInterviewViewData() With {
				.ID = interview.ID,
				.RecordNumber = interview.RecordNumber,
				.AppointmentDate = interview.AppointmentDate,
				.JobTitle = interview.JobTitle,
				.Company = interview.Company,
				.JobAppointmentState = interview.JobAppointmentState,
				.Location = interview.Location,
				.Telephone = interview.Telephone,
				.Telefax = interview.Telefax,
				.Homepage = interview.Homepage,
				.Email = interview.eMail,
				.Outcome = interview.Outcome,
				.VakNr = interview.VakNr,
				.ProposeNr = interview.ProposeNr,
				.CreatedOn = interview.CreatedOn,
				.CreatedFrom = interview.CreatedFrom,
				.ChangedOn = interview.ChangedOn,
				.ChangedFrom = interview.ChangedFrom,
				.CustomerNumber = interview.CustomerNumber,
				.ResponsiblePersonRecordNumber = interview.ResponsiblePersonNumber
			}

			listDataSource.Add(viewData)
		Next

		gridInterview.DataSource = listDataSource

		Return True

	End Function

	Private Function LoadEmployeeContactData(ByVal employeeNumber As Integer)

		'' Convert check years to a integer array.
		'Dim selectedYears = lstYears.CheckedItems
		Dim filterYears As New List(Of Integer)

		'For Each yar In selectedYears
		filterYears.Add(Now.Year)
		'Next

		' If no years are checked enter a impossible year (-> nothing will be found).
		If (filterYears.Count = 0) Then
			filterYears.Add(-1)
		End If

		Dim yearsArray = filterYears.ToArray()
		Dim contactData = m_EmployeeDatabaseAccess.LoadEmployeeContactOverviewDataForPropose(employeeNumber, _ClsSetting.SelectedProposeNr, False, False, False, False, Nothing) ' yearsArray)

		If (contactData Is Nothing) Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Kontaktdaten konnten nicht geladen werden."))

			Return False
		End If

		Dim listDataSource As BindingList(Of EmployeeContactViewData) = New BindingList(Of EmployeeContactViewData)

		' Convert the data to view data.
		For Each p In contactData

			Dim cViewData = New EmployeeContactViewData() With {
					.ID = p.ID,
					.EmployeeNumber = p.EmployeeNumber,
					.ContactRecorNumber = p.RecNr,
					.ContactDate = p.ContactDate,
					.minContactDate = p.minContactDate,
					.maxContactDate = p.maxContactDate,
					.Person_Subject = p.PersonOrSubject,
					.Description = p.Description,
					.Important = p.IsImportant,
					.Completed = p.IsCompleted,
					.KDKontactRecID = p.KDKontactRecID,
					.CreatedFrom = p.CreatedFrom}

			If p.DocumentID.HasValue Then
				cViewData.PDFImage = My.Resources.DocumentAttach
				cViewData.DocumentId = p.DocumentID
			End If

			listDataSource.Add(cViewData)
		Next

		gridEmployeeContactData.DataSource = listDataSource

		Return True

	End Function

	Private Function LoadCustomerContactData(ByVal customerNumber As Integer)

		'' Convert check years to a integer array.
		'Dim selectedYears = lstYears.CheckedItems
		Dim filterYears As New List(Of Integer)

		'For Each yar In selectedYears
		filterYears.Add(Now.Year)
		'Next

		' If no years are checked enter a impossible year (-> nothing will be found).
		If (filterYears.Count = 0) Then
			filterYears.Add(-1)
		End If

		Dim yearsArray = filterYears.ToArray()
		Dim contactData = m_CustomerDatabaseAccess.LoadCustomerContactTotalDataForPropose(customerNumber, _ClsSetting.SelectedProposeNr, False, False, False, False, Nothing) 'yearsArray)

		If (contactData Is Nothing) Then

			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kunden Kontaktdaten konnten nicht geladen werden."))

			Return False
		End If

		Dim listDataSource As BindingList(Of CustomerContactViewData) = New BindingList(Of CustomerContactViewData)

		' Convert the data to view data.
		For Each p In contactData

			Dim cViewData = New CustomerContactViewData() With {
					.ID = p.ID,
					.CustomerNumber = p.CustomerNumber,
					.ContactRecorNumber = p.RecNr,
					.ContactDate = p.ContactDate,
					.minContactDate = p.minContactDate,
					.maxContactDate = p.maxContactDate,
					.Person_Subject = p.PersonOrSubject,
					.Description = p.Description,
					.Important = p.IsImportant,
					.Completed = p.IsCompleted,
					.Creator = p.Creator}

			If p.DocumentID.HasValue Then
				cViewData.PDFImage = My.Resources.DocumentAttach
				cViewData.DocumentId = p.DocumentID
			End If

			listDataSource.Add(cViewData)
		Next

		gridCustomerContactData.DataSource = listDataSource

		Return True

	End Function


#End Region


#Region "RowEvents"

	''' <summary>
	'''  Handles RowStyle event of gvVacancy grid view.
	''' </summary>
	Private Sub OngvEmployee_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvEmployee.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvEmployee.GetRow(e.RowHandle), EmployeeData)

			If Not String.IsNullOrEmpty(rowData.fstate) AndAlso (rowData.NoES Or rowData.DStellen Or rowData.fstate.Contains("abgemeldet") Or rowData.fstate.Contains("ex-mitarbeiter")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

	''' <summary>
	'''  Handles RowStyle event of gvVacancy grid view.
	''' </summary>
	Private Sub OngvCustomer_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvCustomer.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvCustomer.GetRow(e.RowHandle), CustomerData)

			If Not String.IsNullOrEmpty(rowData.fstate) AndAlso (rowData.noes Or rowData.fstate.Contains("nicht ")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

	''' <summary>
	'''  Handles RowStyle event of gvVacancy grid view.
	''' </summary>
	Private Sub OnGVlueCresponsible_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvlueCresponsible.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvlueCresponsible.GetRow(e.RowHandle), ResponsiblePersonData)

			If Not String.IsNullOrEmpty(rowData.fstate) AndAlso (rowData.fstate.Contains("inaktiv") Or rowData.fstate.Contains("nicht aktiv") Or rowData.fstate.Contains("mehr aktiv")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub


	''' <summary>
	'''  Handles RowStyle event of gvVacancy grid view.
	''' </summary>
	Private Sub OnGvVacancy_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvVacancy.RowStyle

		If e.RowHandle >= 0 Then

			Dim rowData = CType(gvVacancy.GetRow(e.RowHandle), VacancyData)

			If Not rowData.vacancystate Is Nothing AndAlso (rowData.vacancystate.Contains("inaktiv") Or rowData.vacancystate.Contains("nicht aktiv")) Then
				e.Appearance.BackColor = Color.LightGray
				e.Appearance.BackColor2 = Color.LightGray
			End If

		End If

	End Sub

#End Region


#End Region

	Private Sub frmPropose_FormClosing(ByVal sender As Object,
																		 ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

		If Me.WindowState = FormWindowState.Minimized Then Exit Sub

		My.Settings.frmLocation_SPProposeUtility = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
		My.Settings.iHeight_SPProposeUtility = Me.Height
		My.Settings.iWidth_SPProposeUtility = Me.Width
		My.Settings.iMainSplitter = Me.sccMain.SplitterPosition
		My.Settings.iDetailsSplitter = Me.sccDetails.SplitterPosition

		My.Settings.Save()

	End Sub

	Private Sub frmPropose_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp

		If Me.pcEditor.Visible Then
			If e.Control And e.KeyCode = Keys.S Then Me.SaveMyContent()

		Else
			If e.Control And e.KeyCode = Keys.S Then SaveSelectedPropose(True)

		End If

	End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		tgsReportingObligation.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsReportingObligation.Properties.OffText)
		tgsReportingObligation.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsReportingObligation.Properties.OnText)
		lblViewedOn.Text = m_Translate.GetSafeTranslationValue(lblViewedOn.Text)
		lblCustomerFeedback.Text = m_Translate.GetSafeTranslationValue(lblCustomerFeedback.Text)
		lblTransferedOn.Text = m_Translate.GetSafeTranslationValue(lblTransferedOn.Text)

		grpEinstellungmerkmale.Text = m_Translate.GetSafeTranslationValue(grpEinstellungmerkmale.Text)
		grpAllgemein.Text = m_Translate.GetSafeTranslationValue(grpAllgemein.Text)

		lblNummer.Text = m_Translate.GetSafeTranslationValue(lblNummer.Text)
		lblVorschlagals.Text = m_Translate.GetSafeTranslationValue(lblVorschlagals.Text)
		lblBerater1.Text = m_Translate.GetSafeTranslationValue(lblBerater1.Text)
		lblBerater2.Text = m_Translate.GetSafeTranslationValue(lblBerater2.Text)
		lblStatus.Text = m_Translate.GetSafeTranslationValue(lblStatus.Text)
		lblAnstellung.Text = m_Translate.GetSafeTranslationValue(lblAnstellung.Text)
		lblVorschlagart.Text = m_Translate.GetSafeTranslationValue(lblVorschlagart.Text)

		grpEmployee.Text = m_Translate.GetSafeTranslationValue(grpEmployee.Text)
		lblKandidatennummer.Text = m_Translate.GetSafeTranslationValue(lblKandidatennummer.Text)

		lblGebDatum.Text = m_Translate.GetSafeTranslationValue(lblGebDatum.Text)
		lblAdresseKandidat.Text = m_Translate.GetSafeTranslationValue(lblAdresseKandidat.Text)
		lblQualifikation.Text = m_Translate.GetSafeTranslationValue(lblQualifikation.Text)
		lblMAStatus.Text = m_Translate.GetSafeTranslationValue(lblMAStatus.Text)

		grpCustomer.Text = m_Translate.GetSafeTranslationValue(grpCustomer.Text)
		lblKundennummer.Text = m_Translate.GetSafeTranslationValue(lblKundennummer.Text)
		lblZHDName.Text = m_Translate.GetSafeTranslationValue(lblZHDName.Text)
		lblVakanzennummer.Text = m_Translate.GetSafeTranslationValue(lblVakanzennummer.Text)

		tabVorstellung.Text = m_Translate.GetSafeTranslationValue(tabVorstellung.Text)
		tabMAKontakt.Text = m_Translate.GetSafeTranslationValue(tabMAKontakt.Text)
		tabKDKontakt.Text = m_Translate.GetSafeTranslationValue(tabKDKontakt.Text)
		tabAbschluss.Text = m_Translate.GetSafeTranslationValue(tabAbschluss.Text)

		grpVerleih.Text = m_Translate.GetSafeTranslationValue(grpVerleih.Text)
		lblKundentarif.Text = m_Translate.GetSafeTranslationValue(lblKundentarif.Text)
		lblEinsatzbeginn.Text = m_Translate.GetSafeTranslationValue(lblEinsatzbeginn.Text)
		lblArbeitszeit.Text = m_Translate.GetSafeTranslationValue(lblArbeitszeit.Text)
		lblSpesen.Text = m_Translate.GetSafeTranslationValue(lblSpesen.Text)
		lblAnstellungals.Text = m_Translate.GetSafeTranslationValue(lblAnstellungals.Text)

		grpVermittlung.Text = m_Translate.GetSafeTranslationValue(grpVermittlung.Text)
		lblAntrittper.Text = m_Translate.GetSafeTranslationValue(lblAntrittper.Text)
		lblLohn.Text = m_Translate.GetSafeTranslationValue(lblLohn.Text)
		lblHonorar.Text = m_Translate.GetSafeTranslationValue(lblHonorar.Text)
		lblMonat.Text = m_Translate.GetSafeTranslationValue(lblMonat.Text)
		lblVerechnungper.Text = m_Translate.GetSafeTranslationValue(lblVerechnungper.Text)
		lblBemerkung.Text = m_Translate.GetSafeTranslationValue(lblBemerkung.Text)

		bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
		bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsiLblGeaendert.Caption)
		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

	End Sub

	Private Sub frmPropose_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		m_SuppressUIEvents = True

		Try
			Me.lblBezeichnung.Text = m_Translate.GetSafeTranslationValue("Verwaltung von Vorschlägen an Kunden")

			Me.Width = Math.Max(My.Settings.iWidth_SPProposeUtility, Me.Width)
			Me.Height = Math.Max(My.Settings.iHeight_SPProposeUtility, Me.Height)
			If My.Settings.iMainSplitter > 0 Then Me.sccMain.SplitterPosition = My.Settings.iMainSplitter
			If My.Settings.iDetailsSplitter > 0 Then Me.sccDetails.SplitterPosition = My.Settings.iDetailsSplitter

			If My.Settings.frmLocation_SPProposeUtility <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation_SPProposeUtility.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Fensterposition: {0}", ex.ToString))

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("formdesign: {0}", ex.ToString))

		End Try

		' Der User kann geändert werden.
		Me.lueAdvisor.Enabled = ClsDataDetail.bAllowedTochangeKST
		LoadAdvisorDropDownData()
		LoadSecAdvisorDropDownData()

		If Me._ClsSetting.SelectedProposeNr > 0 Then
			DisplayFoundedData(Me._ClsSetting.SelectedProposeNr)

			Me.grpEmployee.Visible = True
			Me.grpCustomer.Visible = True
			Me.tbVorschlag.Visible = True

		Else    ' dann ist der Datensatz neu...
			Me.lueAdvisor.EditValue = String.Format("{0}", m_common.GetLogedUserKst)
			Me.lueSecAdvisor.EditValue = String.Format("{0}", m_common.GetLogedUserKst)

			lueEmployee.EditValue = Me._ClsSetting.SelectedMANr
			grpCustomer.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde") & ": {0}", _ClsSetting.SelectedKDNr)
			lueCustomer.EditValue = Me._ClsSetting.SelectedKDNr
			lueCresponsible.EditValue = Me._ClsSetting.SelectedZHDNr
			lueVacancy.EditValue = Me._ClsSetting.SelectedVakNr

			Me.tbVorschlag.Enabled = False
			Me.pcEditor.Enabled = False

			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr

		End If
		Me._ClsSetting.SelectedProposeNr = CInt(Me.LblRecNr.Text)
		Me._ClsSetting.SelectedMANr = lueEmployee.EditValue
		Me._ClsSetting.SelectedKDNr = lueCustomer.EditValue
		Me._ClsSetting.SelectedZHDNr = lueCresponsible.EditValue
		Me._ClsSetting.SelectedVakNr = lueVacancy.EditValue

		ClsDataDetail.bAllowedWriteMAVorColWidth = False


		m_SuppressUIEvents = False

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub


	Private Sub tbVorschlag_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles tbVorschlag.SelectedPageChanged

		If Me.tbVorschlag.SelectedTabPageIndex = 0 Then
			LoadEmployeeInterviewData(lueEmployee.EditValue)

		ElseIf Me.tbVorschlag.SelectedTabPageIndex = 1 Then
			LoadEmployeeContactData(lueEmployee.EditValue)
			'FillMAKontaktLV(Me.LvMAKontakte, Me._ClsSetting.SelectedProposeNr)

		ElseIf Me.tbVorschlag.SelectedTabPageIndex = 2 Then
			LoadCustomerContactData(lueCustomer.EditValue)
			'FillKDKontaktLV(Me.LvKDKontakte, Me._ClsSetting.SelectedProposeNr)

		ElseIf Me.tbVorschlag.SelectedTabPageIndex = 3 Then

		End If
	End Sub


	Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount

		Me.navBarControl.Items.Clear()
		Try
			navBarControl.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupDatei As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Datei"))
			groupDatei.Name = "gNavDatei"

			' Create an Inbox item and assign an image from the SmallImages list to the item.
			Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
			New_P.Name = "New_Propose"
			New_P.SmallImage = ImageCollection1.Images(0)

			Dim Save_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
			Save_P_Data.Name = "Save_Propose_Data"
			Save_P_Data.SmallImage = ImageCollection1.Images(1)

			Dim Print_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
			Print_P_Data.Name = "Print_Propose_Data"
			Print_P_Data.SmallImage = ImageCollection1.Images(2)
			Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 804)

			Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
			Close_P_Data.Name = "Close_Propose_Form"
			Close_P_Data.SmallImage = ImageCollection1.Images(3)

			Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
			groupDelete.Name = "gNavDelete"
			groupDelete.Appearance.ForeColor = Color.Red
			m_DeleteButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
			m_DeleteButton.Name = "Delete_Propose_Data"
			m_DeleteButton.SmallImage = ImageCollection1.Images(4)
			m_DeleteButton.Appearance.ForeColor = Color.Red

			Dim groupExtras As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
			groupExtras.Name = "gNavExtras"
			Dim Abhängigkeiten_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Abhängigkeiten"))
			Abhängigkeiten_P.Name = "Abhängigkeiten_P"
			Abhängigkeiten_P.SmallImage = ImageCollection1.Images(9)
			Dim P_SendeMail As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("E-Mail versenden"))
			P_SendeMail.Name = "P_SendeMail"
			P_SendeMail.SmallImage = ImageCollection1.Images(10)

			Dim Duplicate_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschlag duplizieren"))
			Duplicate_P_Data.Name = "Duplicate_Propose_Data"
			Duplicate_P_Data.SmallImage = ImageCollection1.Images(14)
			Duplicate_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 802)

			Dim P_MakeES As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsatz erfassen"))
			P_MakeES.Name = "P_MakeES"
			P_MakeES.SmallImage = ImageCollection1.Images(15)
			P_MakeES.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 251)

			navBarControl.BeginUpdate()

			navBarControl.Groups.Add(groupDatei)
			groupDatei.ItemLinks.Add(New_P)
			groupDatei.ItemLinks.Add(Save_P_Data)
			groupDatei.ItemLinks.Add(Print_P_Data)
			groupDatei.ItemLinks.Add(Close_P_Data)
			groupDatei.Expanded = True

			navBarControl.Groups.Add(groupDelete)
			groupDelete.ItemLinks.Add(m_DeleteButton)
			groupDelete.Expanded = False

			navBarControl.Groups.Add(groupExtras)
			groupExtras.ItemLinks.Add(Abhängigkeiten_P)
			groupExtras.ItemLinks.Add(P_SendeMail)


			groupExtras.ItemLinks.Add(Duplicate_P_Data)
			groupExtras.ItemLinks.Add(P_MakeES)
			groupExtras.Expanded = True


			navBarControl.EndUpdate()


			Try
				Time_1 = System.Environment.TickCount
				BuildProposeZusatzFields()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BuildProposeZusatzFields.{1}", strMethodeName, ex.Message))

			End Try

			Try
				Time_1 = System.Environment.TickCount
				BuildLLAbschnitt()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BuildLLAbschnitt.{1}", strMethodeName, ex.Message))

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub BuildProposeZusatzFields()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim liLLAbschnit As New List(Of String)

		Try
			liLLAbschnit = ListDBFieldsName4PZusatz()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.ListDBFieldsName4PZusatz.{1}", strMethodeName, ex.Message))

		End Try

		Dim groupLocal As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Zusätzliche Felder"))
		groupLocal.Name = "gNavZusatzFelder"

		Dim itemMA As New NavBarItem
		navBarControl.BeginUpdate()
		navBarControl.Groups.Add(groupLocal)

		For i As Integer = 0 To liLLAbschnit.Count - 1
			Dim strText As String() = liLLAbschnit(i).ToString.Split(CChar("#"))

			' Create an Inbox item and assign an image from the SmallImages list to the item.
			itemMA = New NavBarItem(m_Translate.GetSafeTranslationValue(strText(2)))
			itemMA.Name = String.Format("P_Zusatz_{0}", strText(1))

			groupLocal.ItemLinks.Add(itemMA)

		Next
		groupLocal.Expanded = False
		navBarControl.EndUpdate()

	End Sub

	Sub BuildLLAbschnitt()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim liLLAbschnit As New List(Of String)

		Try
			liLLAbschnit = ListDBFieldsName()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.liLLAbschnit.{1}", strMethodeName, ex.Message))

		End Try

		Dim groupLocal As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Kandidaten Versandfelder"))
		groupLocal.Name = "gNavMAZusatzFelder"

		Dim itemMA As New NavBarItem
		navBarControl.BeginUpdate()
		navBarControl.Groups.Add(groupLocal)

		For i As Integer = 0 To liLLAbschnit.Count - 1
			Dim strText As String() = liLLAbschnit(i).ToString.Split(CChar("#"))

			' Create an Inbox item and assign an image from the SmallImages list to the item.
			itemMA = New NavBarItem(m_Translate.GetSafeTranslationValue(strText(2)))
			itemMA.Name = String.Format("MA_LL_{0}", strText(1))

			groupLocal.ItemLinks.Add(itemMA)

		Next
		groupLocal.Expanded = False
		navBarControl.EndUpdate()

	End Sub

	Sub ListTemplates(ByVal modulArt As String, ByVal dbFieldName As String)
		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'Dim liLLTemplate As New List(Of String)

		'liLLTemplate = ListLLTemplateName(dbFieldName) ' Mid(Me.strLinkName, 10, Me.strLinkName.Length))

		Dim tplData = m_EmployeeDatabaseAccess.LoadLLZusatzFieldsTemplateData(dbFieldName)
		If tplData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlage Daten konnten nicht geladen werden."))

			Return
		End If


		Me.btnShowTemplate.ClearLinks()

		Dim listDataSource As BindingList(Of LLZusatzFieldsTemplateData) = New BindingList(Of LLZusatzFieldsTemplateData)
		For Each mnu In tplData
			Dim listTemplate As Boolean = True
			Dim templateFilename As String = mnu.FileName
			Dim templateFullfilename As String = FindExtendedTemplateFile(modulArt, templateFilename)
			listTemplate = Not String.IsNullOrWhiteSpace(templateFullfilename)

			If listTemplate Then
				Dim popupMenu As New DevExpress.XtraBars.PopupMenu
				popupMenu.Manager = Me.BarManager1

				Dim itm As New DevExpress.XtraBars.BarButtonItem
				itm.Caption = m_Translate.GetSafeTranslationValue(mnu.Bezeichnung)
				itm.AccessibleName = templateFullfilename

				Me.btnShowTemplate.AddItem(itm)
				AddHandler itm.ItemClick, AddressOf GetTemplateMnu

			Else

				m_Logger.LogWarning(String.Format("extended templates for {0} could not be founded! {1} >>> {2}", modulArt, dbFieldName, templateFullfilename))
			End If

		Next

	End Sub

	Private Function FindExtendedTemplateFile(ByVal modulArt As String, ByVal tplFilename As String) As String
		Dim result As Boolean = True
		Dim additionalPath As String = "employee"
		Dim templatePath As String = m_InitializationData.MDData.MDTemplatePath
		Dim templateFile = Path.Combine(templatePath, tplFilename)

		Select Case modulArt.ToLower
			Case "employee"
				additionalPath = "employee"

			Case "propose"
				additionalPath = "propose"

			Case "cvtemplate"
				additionalPath = "CV Templates"

			Case Else
				Return String.Empty

		End Select

		If Not File.Exists(templateFile) Then
			result = False
			templateFile = Path.Combine(templatePath, additionalPath, tplFilename)

			If File.Exists(templateFile) Then
				result = True
			End If
		End If
		If Not result Then templateFile = String.Empty

		Return templateFile
	End Function

	Private Function ValidateInputData() As Boolean

		Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

		Dim isValid As Boolean = True

		isValid = isValid And SetErrorIfInvalid(lueMandant, ErrorProvider1, String.IsNullOrEmpty(lueMandant.EditValue), errorText)
		isValid = isValid And SetErrorIfInvalid(lueAdvisor, ErrorProvider1, String.IsNullOrEmpty(lueAdvisor.EditValue), errorText)

		isValid = isValid And SetErrorIfInvalid(txt_Bezeichnung, ErrorProvider1, String.IsNullOrEmpty(txt_Bezeichnung.Text), errorText)

		isValid = isValid And SetErrorIfInvalid(lueEmployee, ErrorProvider1, (lueEmployee.EditValue = Nothing), errorText)
		isValid = isValid And SetErrorIfInvalid(lueCustomer, ErrorProvider1, (lueCustomer.EditValue = Nothing), errorText)

		Return isValid
	End Function

	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	Sub SaveSelectedPropose(ByVal showMessage As Boolean)
		Dim _ClsFuncDb As New ClsDbFunc

		Dim bSave As Boolean = ValidateInputData()
		If Not bSave Then Exit Sub Else Me.ErrorProvider1.Clear()

		ClsDataDetail.bAsNew = CInt(Val(Me.LblRecNr.Text)) = 0
		With _ClsFuncDb
			If ClsDataDetail.bAsNew Then ClsDataDetail.bAllowedToChange = True
			.GetMDNr = lueMandant.EditValue
			.GetRecNr = CInt(Val(Me.LblRecNr.Text))

			.GetBezeichnung = Me.txt_Bezeichnung.Text
			.GetMANr = lueEmployee.EditValue
			.GetKDNr = lueCustomer.EditValue
			.GetKDZHDNr = Me.lueCresponsible.EditValue ' CInt(Val(Me.Lib_ZhdNr.Text))
			.GetVakNr = lueVacancy.EditValue ' CInt(Val(Me.txt_VakNr.Text))

			.GetBerater_1 = Me.lueAdvisor.Text
			.GetBerater_2 = lueSecAdvisor.Text ' strKST(0).ToString.Trim

			.GetP_KST_1 = Me.lueAdvisor.EditValue
			.GetP_KST_2 = lueSecAdvisor.EditValue ' strKST(0).ToString.Trim

			.GetP_State = Me.Cbo_Status.Text
			.GetP_Art = Me.Cbo_Art.Text
			.GetKD_Tarif = Me.txt_Tarif.Text
			.GetP_ArbZeit = Me.txt_ArbZeit.Text
			.GetP_Spesen = Me.txt_Spesen.Text

			.GetAnstellung = Me.Cbo_Anstellung.Text
			.GetArbBegin = Me.txt_ArbBegin.Text
			If Not Me.txt_ArbBegin_Date.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(Me.txt_ArbBegin_Date.EditValue) Then
				.GetArbbegin_Date = CDate(Me.txt_ArbBegin_Date.EditValue)
			End If


			.GetAb_AnstellungAls = Me.txt_Ab_Als.Text.Trim
			.GetAb_AntrittPer = Me.txt_Ab_AntrittPer.Text.Trim

			.GetAb_LohnBasis = If(Not IsNumeric(Me.txt_Ab_LBasis.Text), 0, CDbl(Me.txt_Ab_LBasis.Text))
			.GetAb_LohnAnzahl = If(Not IsNumeric(Me.txt_Ab_LAnzahl.Text), 0, CDbl(Me.txt_Ab_LAnzahl.Text))
			.GetAb_LohnBetrag = If(Not IsNumeric(Me.txt_Ab_LBetrag.Text), 0, CDbl(Me.txt_Ab_LBetrag.Text))
			.GetAb_HBasis = If(Not IsNumeric(Me.txt_Ab_HBasis.Text), 0, CDbl(Me.txt_Ab_HBasis.Text))
			.GetAb_HAnsatz = If(Not IsNumeric(Me.txt_Ab_HAnsatz.Text), 0, CDbl(Me.txt_Ab_HAnsatz.Text))
			.GetAb_HBetrag = If(Not IsNumeric(Me.txt_Ab_HBetrag.Text), 0, CDbl(Me.txt_Ab_HBetrag.Text))

			.GetAb_RePer = Me.txt_Ab_RePer.EditValue
			If Not Me.txt_Ab_RePer_Date.EditValue Is Nothing AndAlso Not String.IsNullOrWhiteSpace(Me.txt_Ab_RePer_Date.EditValue) Then
				.GetAb_RePer_Date = Me.txt_Ab_RePer_Date.EditValue
			End If

			.GetAb_Bemerkung = Me.txt_Ab_Bemerkung.Text.Trim
			.ApplicationNumber = _ClsSetting.ApplicationNumber

			Dim result = _ClsFuncDb.SaveDataToProposeDb(CInt(Me.LblRecNr.Text))
			If CInt(Me.LblRecNr.Text) = 0 AndAlso _ClsSetting.ApplicationNumber.GetValueOrDefault(0) > 0 Then
				Dim success As Boolean = True

				Dim appData = m_AppDatabaseAccess.LoadAssignedApplicationDataForMainView(_ClsSetting.ApplicationNumber)
				If Not appData Is Nothing Then
					Select Case appData.ApplicationLifecycle
						Case ApplicationLifecycelEnum.APPLICATIONNEW, ApplicationLifecycelEnum.APPLICATIONVIEWED, ApplicationLifecycelEnum.APPLICATIONFORWARDED
							appData.ApplicationLifecycle = ApplicationLifecycelEnum.PROPOSE
							success = success AndAlso m_AppDatabaseAccess.UpdateMainViewApplicationWithAdvisorData(appData)
					End Select
				End If

			End If
			If CInt(Val(Me.LblRecNr.Text)) = 0 Then
				CreateLogToKontaktDb(lueCustomer.EditValue, lueCresponsible.EditValue, lueEmployee.EditValue, .GetRecNr, .GetVakNr, ContactString(0))

				LoadEmployeeContactData(lueEmployee.EditValue)
				LoadCustomerContactData(lueCustomer.EditValue)

			End If
			Me.LblRecNr.Text = _ClsFuncDb.GetRecNr.ToString

			Me._ClsSetting.SelectedProposeNr = CInt(Me.LblRecNr.Text)
			Me._ClsSetting.SelectedMANr = lueEmployee.EditValue
			Me._ClsSetting.SelectedKDNr = lueCustomer.EditValue
			Me._ClsSetting.SelectedZHDNr = Me.lueCresponsible.EditValue
			Me._ClsSetting.SelectedVakNr = lueVacancy.EditValue ' CInt(Val(Me.txt_VakNr.Text))

			ClsDataDetail.GetProposalNr = Me._ClsSetting.SelectedProposeNr
			ClsDataDetail.GetProposalMANr = Me._ClsSetting.SelectedMANr
			ClsDataDetail.GetProposalKDNr = Me._ClsSetting.SelectedKDNr
			ClsDataDetail.GetProposalZHDNr = Me._ClsSetting.SelectedZHDNr
			ClsDataDetail.GetProposalVakNr = Me._ClsSetting.SelectedVakNr

			If result Then
				If m_InitializationData.UserData.UserNr = 1 Then
					Dim ToastNotificationsManager2 As New ToastNotificationsManager
					ToastNotificationsManager2.HideNotifications(ToastNotificationsManager2.Notifications())
					Dim note As New ToastNotification("88ea4885-79e0-416b-b81c-a3e063877991", Nothing, m_Translate.GetSafeTranslationValue("Daten speichern"),
																					m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."), ToastNotificationSound.Reminder, ToastNotificationTemplate.ImageAndText02)
					ToastNotificationsManager2.Notifications.Add(note)
					ToastNotificationsManager2.ShowNotification(ToastNotificationsManager2.Notifications(0))
				Else
					If showMessage Then m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
				End If

			Else
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
			End If

		End With
		If CInt(Val(Me.LblRecNr.Text)) > 0 Then
			'Me.LblState.Text = "Ihre Daten wurden erfolgreich gespeichert..."
			If ClsDataDetail.bAsNew Then
				lueEmployee.Properties.Buttons(0).Enabled = False
				lueEmployee.Properties.Buttons(1).Enabled = False
				lueEmployee.Properties.ReadOnly = True

				lueCustomer.Properties.Buttons(0).Enabled = False
				lueCustomer.Properties.Buttons(1).Enabled = False
				lueCustomer.Properties.ReadOnly = True

				lueCresponsible.Properties.Buttons(0).Enabled = (lueCresponsible.EditValue Is Nothing)
				lueCresponsible.Properties.Buttons(1).Enabled = (lueCresponsible.EditValue Is Nothing)
				lueCresponsible.Properties.ReadOnly = Not (lueCresponsible.EditValue Is Nothing)

				lueVacancy.Properties.Buttons(0).Enabled = (lueVacancy.EditValue Is Nothing)
				lueVacancy.Properties.Buttons(1).Enabled = (lueVacancy.EditValue Is Nothing)
				lueVacancy.Properties.ReadOnly = Not (lueVacancy.EditValue Is Nothing)

			End If
		End If
		Me.pcEditor.Enabled = True
		Me.tbVorschlag.Enabled = True

	End Sub

	Private Sub OnfpDuplicateData_Load(sender As Object, e As EventArgs) Handles fpDuplicateData.Load

		ResetCopyEmployeeDropDown()

		ResetCopyCustomerDropDown()
		ResetCopyResponsiblePersonDropDown()
		ResetCopyVacancyDropDown()

		LoadProposeDataForCopy()

	End Sub

	Private Sub LoadProposeDataForCopy()

		lueCopyEmployee.Properties.DataSource = lueEmployee.Properties.DataSource
		lueCopyCustomer.Properties.DataSource = lueCustomer.Properties.DataSource
		lueCopyCresponsible.Properties.DataSource = lueCresponsible.Properties.DataSource
		lueCopyVacancy.Properties.DataSource = lueVacancy.Properties.DataSource

		lueCopyEmployee.EditValue = lueEmployee.EditValue
		lueCopyCustomer.EditValue = lueCustomer.EditValue
		lueCopyCresponsible.EditValue = lueCresponsible.EditValue
		lueCopyVacancy.EditValue = lueVacancy.EditValue

	End Sub

	Private Sub fpDuplicateData_ButtonClick(sender As Object, e As FlyoutPanelButtonClickEventArgs) Handles fpDuplicateData.ButtonClick
		Dim tag As Integer = Val(e.Button.Tag)

		Select Case e.Button.Tag
			Case 1
				DuplicateSelectedPropose()
				Return

			Case Else
				Me.Enabled = True

		End Select

		fpDuplicateData.HideBeakForm()

	End Sub

	Private Function DuplicateSelectedPropose() As Boolean
		Dim success As Boolean = True
		Dim newRec As Boolean = True
		Dim userKontakt As String = String.Empty
		Dim userEmail As String = String.Empty

		Dim msg As String
		msg = "Hiermit erstellen Sie eine neue Kopie von Ihren Vorschlagdaten.{0}Sind Sie sicher?"
		If m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine), m_Translate.GetSafeTranslationValue("Daten kopieren?")) = False Then Return False


		Dim oldProposeNumber As Integer = Val(Me.LblRecNr.Text)
		m_CurrentProposedata = m_ProposeDatabaseAccess.LoadProposeMasterData(oldProposeNumber)

		success = success AndAlso ValidateInputData()
		If Not success Then Return False

		Dim proposeNumberOffsetFromSettings As Integer = ReadProposeOffsetFromSettings()
		m_CurrentProposedata.ProposeNumberOffset = proposeNumberOffsetFromSettings

		m_CurrentProposedata.CreatedFrom = m_InitializationData.UserData.UserFullNameWithComma
		m_CurrentProposedata.KDNr = lueCopyCustomer.EditValue
		m_CurrentProposedata.KDZHDNr = lueCopyCresponsible.EditValue
		m_CurrentProposedata.MANr = lueCopyEmployee.EditValue
		m_CurrentProposedata.VakNr = lueCopyVacancy.EditValue

		success = success AndAlso m_ProposeDatabaseAccess.DuplicateProposeData(lueMandant.EditValue, oldProposeNumber, m_CurrentProposedata)

		If success Then
			CreateLogToKontaktDb(lueCopyCustomer.EditValue, lueCopyCresponsible.EditValue, lueCopyEmployee.EditValue, m_CurrentProposedata.ProposeNr, m_CurrentProposedata.VakNr, ContactString(0))

			msg = "Ihre Vorschlagsdaten wurden erfolgreich dupliziert.{0}Bitte kontrollieren Sie Ihre Daten."
			m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine))


			ClsDataDetail.GetProposalNr = m_CurrentProposedata.ProposeNr

			Dim proposeSetting = New ClsProposeSetting With {.SelectedProposeNr = m_CurrentProposedata.ProposeNr, .SelectedMANr = m_CurrentProposedata.MANr, .SelectedKDNr = m_CurrentProposedata.KDNr, .SelectedZHDNr = m_CurrentProposedata.KDZHDNr, .IsAsDuplicated = True}
			Dim frmPropose = New frmPropose(ClsDataDetail.m_InitialData, proposeSetting)

			frmPropose.Show()
			frmPropose.BringToFront()

		Else
			m_CurrentProposedata.ProposeNr = Nothing
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihr Vorschlag konnte nicht dupliziert werden!"))

		End If

		Close()


		Return success

	End Function

	Private Function ReadProposeOffsetFromSettings() As Integer

		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(lueMandant.EditValue, Now.Year)
		m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		m_StartNumberSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_STARTNUMBER_SETTING, lueMandant.EditValue)

		Dim proposeNumberStartNumberSetting As String = Val(m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/proposenumber", m_StartNumberSetting)))

		Dim intVal As Integer
		If Integer.TryParse(proposeNumberStartNumberSetting, intVal) Then
			Return intVal
		Else
			Return 0
		End If

	End Function

	Sub FillMyListing(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal liEntry As List(Of String))

		For i As Integer = 0 To liEntry.Count - 1
			cbo.Properties.Items.Add(liEntry.Item(i).ToString)
		Next

	End Sub

#Region "Cbo Funktionen..."

	Private Sub Cbo_Status_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Status.QueryPopUp
		Dim liResult As New List(Of String)

		If Me.Cbo_Status.Properties.Items.Count > 0 Then Exit Sub
		liResult = ListAllData("Tab_P_State")
		FillMyListing(Me.Cbo_Status, liResult)

	End Sub

	Private Sub Cbo_Art_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Art.QueryPopUp
		Dim liResult As New List(Of String)

		If Me.Cbo_Art.Properties.Items.Count > 0 Then Exit Sub
		liResult = ListAllData("Tab_P_Art")
		FillMyListing(Me.Cbo_Art, liResult)

	End Sub

	Private Sub Cbo_Anstellung_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Anstellung.QueryPopUp
		Dim liResult As New List(Of String)

		If Me.Cbo_Anstellung.Properties.Items.Count > 0 Then Exit Sub
		liResult = ListAllData("Tab_P_Anstellung")
		FillMyListing(Me.Cbo_Anstellung, liResult)

	End Sub

#End Region

	Private Sub txt_Bezeichnung_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_Bezeichnung.ButtonClick
		Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
		obj.SelectMultirecords = False
		Dim success = True

		success = success AndAlso obj.LoadQualificationData("M")
		If Not success Then Return

		obj.ShowDialog()
		Dim selectedProfessionsString = obj.GetSelectedData
		If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return

		Me.txt_Bezeichnung.Text = If(selectedProfessionsString.Contains("#"), selectedProfessionsString.Split("#")(1), selectedProfessionsString)


		'Dim frmTest As New frmSearchRec

		'ClsDataDetail.GetSelectedNumbers = String.Empty
		'ClsDataDetail.GetSelectedBez = String.Empty

		'_ClsFunc.Get4What = "JOB"
		'ClsDataDetail.strButtonValue = "JOB"

		'frmTest.ShowDialog()
		'frmTest.MdiParent = Me.MdiParent

		'Dim m As String
		'm = frmTest.iValue(_ClsFunc.Get4What)
		'Me.txt_Bezeichnung.Text = CStr(ClsDataDetail.GetSelectedBez.Replace("#@", ","))

		'frmTest.Dispose()

	End Sub




#Region "Job Interview methodes"


	''' <summary>
	''' Handles double click on interview.
	''' </summary>
	Private Sub OngvInterview_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvInterview.DoubleClick
		Dim selectedRows = gvInterview.GetSelectedRows()

		If (selectedRows.Count > 0) Then
			Dim InterviewData = CType(gvInterview.GetRow(selectedRows(0)), JobInterviewViewData)
			ShowInterviewDetailForm(lueEmployee.EditValue, InterviewData.RecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles click on add interview.
	''' </summary>
	Private Sub OnBtnAddEmployeeInterview_Click(sender As System.Object, e As System.EventArgs) Handles btnAddEmployeeInterview.Click

		ShowInterviewDetailForm(lueEmployee.EditValue, Nothing)

	End Sub


	''' <summary>
	''' Shows the employee interview detail form.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="InterviewRecordNumber">The interview record number to select.</param>
	Private Sub ShowInterviewDetailForm(ByVal employeeNumber As Integer, ByVal InterviewRecordNumber As Integer?)

		If m_JobInverviewDetailForm Is Nothing Then

			If Not m_JobInverviewDetailForm Is Nothing Then
				'First cleanup handlers of old form before new form is created.
				RemoveHandler m_JobInverviewDetailForm.FormClosed, AddressOf OnEmployeeInterviewFormClosed
				RemoveHandler m_JobInverviewDetailForm.InterviewDataSaved, AddressOf OnEmployeeInterviewFormDataSaved
				RemoveHandler m_JobInverviewDetailForm.InterviewDataDeleted, AddressOf OnEmployeeInterviewFormDataDeleted
			End If

			m_JobInverviewDetailForm = New SP.MA.VorstellungMng.frmJobInterview(m_InitializationData)
			AddHandler m_JobInverviewDetailForm.FormClosed, AddressOf OnEmployeeInterviewFormClosed
			AddHandler m_JobInverviewDetailForm.InterviewDataSaved, AddressOf OnEmployeeInterviewFormDataSaved
			AddHandler m_JobInverviewDetailForm.InterviewDataDeleted, AddressOf OnEmployeeInterviewFormDataDeleted

		End If

		'm_JobInverviewDetailForm.Show()

		If InterviewRecordNumber.HasValue Then
			m_JobInverviewDetailForm.LoadJobInterviewData(lueEmployee.EditValue, InterviewRecordNumber)
		Else
			m_JobInverviewDetailForm.InitDataForNewInteview = New InitalDataForJobInterview With {.InterviewAs = Me.txt_Bezeichnung.Text,
																																								.InteviewDate = New DateTime(Now.Year, Now.Month, Now.Day, 8, 0, 0, 0),
																																								.CustomerNumber = lueCustomer.EditValue,
																																								.ResponsiblePersonNumber = lueCresponsible.EditValue,
																																								.IDState = Nothing,
																																								.Result = String.Empty,
																																								.VakNr = lueVacancy.EditValue,
																																								.ProposeNr = _ClsSetting.SelectedProposeNr
																																							 }

			m_JobInverviewDetailForm.LoadJobInterviewData(lueEmployee.EditValue, InterviewRecordNumber)

		End If

		m_JobInverviewDetailForm.Show()
		m_JobInverviewDetailForm.BringToFront()

	End Sub

	''' <summary>
	''' Handles close of employee contact form.
	''' </summary>
	Private Sub OnEmployeeInterviewFormClosed(sender As System.Object, e As System.EventArgs)

		m_JobInverviewDetailForm = Nothing
		'Dim InterviewForm = CType(sender, SP.MA.VorstellungMng.frmJobInterview)

	End Sub

	''' <summary>
	''' Handles contact form data saved.
	''' </summary>
	Private Sub OnEmployeeInterviewFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal RecordNumber As Integer)

		LoadEmployeeInterviewData(lueEmployee.EditValue)

		Dim InterviewForm = CType(sender, SP.MA.VorstellungMng.frmJobInterview)

		FocusEmployeeInterview(lueEmployee.EditValue, RecordNumber)

	End Sub

	''' <summary>
	''' Handles contact form data deleted saved.
	''' </summary>
	Private Sub OnEmployeeInterviewFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal RecordNumber As Integer)
		LoadEmployeeInterviewData(lueEmployee.EditValue)
	End Sub

	''' <summary>
	''' Focuses a employee contact info.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="RecordNumber">The contact record number.</param>
	Private Sub FocusEmployeeInterview(ByVal employeeNumber As Integer, ByVal RecordNumber As Integer)

		Dim listDataSource As BindingList(Of JobInterviewViewData) = gridInterview.DataSource

		Dim InterviewViewData = listDataSource.Where(Function(data) data.RecordNumber = RecordNumber).FirstOrDefault()

		If Not InterviewViewData Is Nothing Then
			Dim sourceIndex = listDataSource.IndexOf(InterviewViewData)
			Dim rowHandle = gridViewEmployeeContactData.GetRowHandle(sourceIndex)
			gridViewEmployeeContactData.FocusedRowHandle = rowHandle
		End If

	End Sub


#End Region


#Region "Employee Contact methodes"


	''' <summary>
	''' Handles double click on contact.
	''' </summary>
	Private Sub OnEmployeeContact_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridViewEmployeeContactData.DoubleClick
		Dim selectedRows = gridViewEmployeeContactData.GetSelectedRows()

		If (selectedRows.Count > 0) Then
			Dim contactInfoData = CType(gridViewEmployeeContactData.GetRow(selectedRows(0)), EmployeeContactViewData)
			ShowEmployeeContatDetailForm(lueEmployee.EditValue, contactInfoData.ContactRecorNumber)

			'RunOpenMAKontaktForm(contactInfoData.ContactRecorNumber, lueEmployee.EditValue, Nothing, Nothing, Nothing)

		End If

	End Sub

	''' <summary>
	''' Handles click on add contact.
	''' </summary>
	Private Sub OnBtnAddEmployeeContactInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnAddEmployeeContact.Click

		ShowEmployeeContatDetailForm(lueEmployee.EditValue, Nothing)

	End Sub


	''' <summary>
	''' Shows the employee contact detail form.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="conactRecordNumber">The contact record number to select.</param>
	Private Sub ShowEmployeeContatDetailForm(ByVal employeeNumber As Integer, ByVal conactRecordNumber As Integer?)

		If m_ContactEmployeeDetailForm Is Nothing OrElse m_ContactEmployeeDetailForm.IsDisposed Then

			If Not m_ContactEmployeeDetailForm Is Nothing Then
				'First cleanup handlers of old form before new form is created.
				RemoveHandler m_ContactEmployeeDetailForm.FormClosed, AddressOf OnEmployeeContactFormClosed
				RemoveHandler m_ContactEmployeeDetailForm.ContactDataSaved, AddressOf OnEmployeeContactInfoFormDocumentDataSaved
				RemoveHandler m_ContactEmployeeDetailForm.ContactDataDeleted, AddressOf OnEmployeeContactInfoFormDocumentDataDeleted
			End If

			m_ContactEmployeeDetailForm = New SP.MA.KontaktMng.frmContacts(m_InitializationData)
			m_ContactEmployeeFilterSettingsForDetailForm = New SP.MA.KontaktMng.ContactFilterSettings

			AddHandler m_ContactEmployeeDetailForm.FormClosed, AddressOf OnEmployeeContactFormClosed
			AddHandler m_ContactEmployeeDetailForm.ContactDataSaved, AddressOf OnEmployeeContactInfoFormDocumentDataSaved
			AddHandler m_ContactEmployeeDetailForm.ContactDataDeleted, AddressOf OnEmployeeContactInfoFormDocumentDataDeleted
		End If

		m_ContactEmployeeFilterSettingsForDetailForm.ExcluePhone = False
		m_ContactEmployeeFilterSettingsForDetailForm.ExclueMail = False
		m_ContactEmployeeFilterSettingsForDetailForm.ExclueOffered = True
		m_ContactEmployeeFilterSettingsForDetailForm.ExcludeSMS = True
		m_ContactEmployeeFilterSettingsForDetailForm.ClearYears()

		m_ContactEmployeeFilterSettingsForDetailForm.AddYear(Now.Year)

		m_ContactEmployeeDetailForm.Show()

		If conactRecordNumber.HasValue Then
			m_ContactEmployeeDetailForm.LoadContactData(lueEmployee.EditValue, conactRecordNumber, Nothing) ' m_ContactEmployeeFilterSettingsForDetailForm)
		Else
			Dim initalData As New SP.MA.KontaktMng.InitalDataForNewContact With {.StartDateTime = DateTime.Now,
																																					 .ContactTypeBezID = "Telefonisch",
																																					 .customerProposeNumber = _ClsSetting.SelectedProposeNr,
																																					 .customerNumber = lueCustomer.EditValue,
																																					 .customerVacancyNumber = lueVacancy.EditValue,
																																					 .customerESNumber = Nothing}
			m_ContactEmployeeDetailForm.ActivateNewContactDataMode(lueEmployee.EditValue, initalData, Nothing) ' m_ContactEmployeeFilterSettingsForDetailForm)
		End If

		'm_ContactCustomerDetailForm.BringToFront()

	End Sub

	''' <summary>
	''' Handles close of employee contact form.
	''' </summary>
	Private Sub OnEmployeeContactFormClosed(sender As System.Object, e As System.EventArgs)

		Dim contatsForm = CType(sender, SP.MA.KontaktMng.frmContacts)

		If contatsForm.CurrentContactRecordNumber.HasValue Then
			FocusEmployeeContactInfo(lueEmployee.EditValue, contatsForm.CurrentContactRecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles contact form data saved.
	''' </summary>
	Private Sub OnEmployeeContactInfoFormDocumentDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)
		'LoadYearsListData(m_EmployeeNumber)
		LoadEmployeeContactData(lueEmployee.EditValue)

		Dim contatsForm = CType(sender, SP.MA.KontaktMng.frmContacts)

		FocusEmployeeContactInfo(lueEmployee.EditValue, contactRecordNumber)

	End Sub

	''' <summary>
	''' Handles contact form data deleted saved.
	''' </summary>
	Private Sub OnEmployeeContactInfoFormDocumentDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)
		LoadEmployeeContactData(lueEmployee.EditValue)
	End Sub

	''' <summary>
	''' Focuses a employee contact info.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="contactRecordNumber">The contact record number.</param>
	Private Sub FocusEmployeeContactInfo(ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)

		Dim listDataSource As BindingList(Of EmployeeContactViewData) = gridEmployeeContactData.DataSource

		Dim contactViewData = listDataSource.Where(Function(data) data.EmployeeNumber = employeeNumber AndAlso data.ContactRecorNumber = contactRecordNumber).FirstOrDefault()

		If Not contactViewData Is Nothing Then
			Dim sourceIndex = listDataSource.IndexOf(contactViewData)
			Dim rowHandle = gridViewEmployeeContactData.GetRowHandle(sourceIndex)
			gridViewEmployeeContactData.FocusedRowHandle = rowHandle
		End If

	End Sub


#End Region


#Region "Customer Contact methodes"


	''' <summary>
	''' Handles double click on contact.
	''' </summary>
	Private Sub OnCustomerContact_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridViewCustomerContactData.DoubleClick
		Dim selectedRows = gridViewCustomerContactData.GetSelectedRows()

		If (selectedRows.Count > 0) Then
			Dim contactInfoData = CType(gridViewCustomerContactData.GetRow(selectedRows(0)), CustomerContactViewData)
			ShowCustomerContatDetailForm(lueCustomer.EditValue, contactInfoData.ContactRecorNumber)

		End If

	End Sub

	''' <summary>
	''' Handles click on add contact.
	''' </summary>
	Private Sub OnBtnAddCustomerContactInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCustomerContact.Click

		ShowCustomerContatDetailForm(lueCustomer.EditValue, Nothing)

	End Sub


	''' <summary>
	''' Shows the Customer contact detail form.
	''' </summary>
	''' <param name="CustomerNumber">The Customer number.</param>
	''' <param name="conactRecordNumber">The contact record number to select.</param>
	Private Sub ShowCustomerContatDetailForm(ByVal CustomerNumber As Integer, ByVal conactRecordNumber As Integer?)

		If m_ContactCustomerDetailForm Is Nothing OrElse m_ContactCustomerDetailForm.IsDisposed Then

			If Not m_ContactCustomerDetailForm Is Nothing Then
				'First cleanup handlers of old form before new form is created.
				RemoveHandler m_ContactCustomerDetailForm.FormClosed, AddressOf OnCustomerContactFormClosed
				RemoveHandler m_ContactCustomerDetailForm.ContactDataSaved, AddressOf OnCustomerContactInfoFormDocumentDataSaved
				RemoveHandler m_ContactCustomerDetailForm.ContactDataDeleted, AddressOf OnCustomerContactInfoFormDocumentDataDeleted
			End If

			m_ContactCustomerDetailForm = New SP.KD.KontaktMng.frmContacts(m_InitializationData)
			m_ContactCustomerFilterSettingsForDetailForm = New SP.MA.KontaktMng.ContactFilterSettings

			AddHandler m_ContactCustomerDetailForm.FormClosed, AddressOf OnCustomerContactFormClosed
			AddHandler m_ContactCustomerDetailForm.ContactDataSaved, AddressOf OnCustomerContactInfoFormDocumentDataSaved
			AddHandler m_ContactCustomerDetailForm.ContactDataDeleted, AddressOf OnCustomerContactInfoFormDocumentDataDeleted
		End If

		m_ContactCustomerFilterSettingsForDetailForm.ExcluePhone = False
		m_ContactCustomerFilterSettingsForDetailForm.ExclueMail = False
		m_ContactCustomerFilterSettingsForDetailForm.ExclueOffered = True
		m_ContactCustomerFilterSettingsForDetailForm.ExcludeSMS = True
		m_ContactCustomerFilterSettingsForDetailForm.ClearYears()

		m_ContactCustomerFilterSettingsForDetailForm.AddYear(Now.Year)

		m_ContactCustomerDetailForm.Show()

		If conactRecordNumber.HasValue Then
			m_ContactCustomerDetailForm.LoadContactData(lueCustomer.EditValue, lueCresponsible.EditValue, conactRecordNumber, Nothing)  ' m_ContactCustomerFilterSettingsForDetailForm)
		Else
			Dim initalData As New SP.KD.KontaktMng.InitalDataForNewContact With {.StartDateTime = DateTime.Now, .ContactTypeBezID = "Telefonisch",
																																					 .customerESNumber = Nothing, .customerProposeNumber = _ClsSetting.SelectedProposeNr,
																																					 .customerVacancyNumber = lueVacancy.EditValue, .EmployeeCopyList = New List(Of Integer)(New Integer() {lueEmployee.EditValue})}
			m_ContactCustomerDetailForm.ActivateNewContactDataMode(lueCustomer.EditValue, lueCresponsible.EditValue, initalData, Nothing)
		End If

		'm_ContactCustomerDetailForm.BringToFront()

	End Sub

	''' <summary>
	''' Handles close of Customer contact form.
	''' </summary>
	Private Sub OnCustomerContactFormClosed(sender As System.Object, e As System.EventArgs)

		Dim contatsForm = CType(sender, SP.KD.KontaktMng.frmContacts)

		If contatsForm.CurrentContactRecordNumber.HasValue Then
			FocusCustomerContactInfo(lueCustomer.EditValue, contatsForm.CurrentContactRecordNumber)
		End If

	End Sub

	''' <summary>
	''' Handles contact form data saved.
	''' </summary>
	Private Sub OnCustomerContactInfoFormDocumentDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)
		'LoadYearsListData(m_CustomerNumber)
		LoadCustomerContactData(lueCustomer.EditValue)

		Dim contatsForm = CType(sender, SP.KD.KontaktMng.frmContacts)

		FocusCustomerContactInfo(lueCustomer.EditValue, contactRecordNumber)

	End Sub

	''' <summary>
	''' Handles contact form data deleted saved.
	''' </summary>
	Private Sub OnCustomerContactInfoFormDocumentDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)
		LoadCustomerContactData(lueCustomer.EditValue)
	End Sub

	''' <summary>
	''' Focuses a Customer contact info.
	''' </summary>
	''' <param name="CustomerNumber">The Customer number.</param>
	''' <param name="contactRecordNumber">The contact record number.</param>
	Private Sub FocusCustomerContactInfo(ByVal CustomerNumber As Integer, ByVal contactRecordNumber As Integer)

		Dim listDataSource As BindingList(Of CustomerContactViewData) = gridCustomerContactData.DataSource

		Dim contactViewData = listDataSource.Where(Function(data) data.CustomerNumber = CustomerNumber AndAlso data.ContactRecorNumber = contactRecordNumber).FirstOrDefault()

		If Not contactViewData Is Nothing Then
			Dim sourceIndex = listDataSource.IndexOf(contactViewData)
			Dim rowHandle = gridViewCustomerContactData.GetRowHandle(sourceIndex)
			gridViewCustomerContactData.FocusedRowHandle = rowHandle
		End If

	End Sub


#End Region


	Private Function DeleteSelectedPropose() As Boolean
		Dim success As Boolean = True
		Dim msg As String

		msg = m_Translate.GetSafeTranslationValue("Der Datensatz wird gelöscht. Möchten Sie wirklich diesen Datensatz löschen?")
		If m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False Then Return False

		success = success AndAlso m_ProposeDatabaseAccess.DeleteProposeData(m_CurrentProposedata.ProposeNr, m_InitializationData.UserData.UserNr)

		If success Then
			msg = "Der Datensatz wurde erfolgreich gelöscht."
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))

		Else
			msg = "Die Vakanz konnte nicht gelöscht werden."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

		End If

		Return success
	End Function

	Private Sub tbVorschlag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbVorschlag.Click

		If Me.tbVorschlag.SelectedTabPage.Name.ToLower = "tabVorstellung".ToLower AndAlso
											CInt(Val(Me.LblRecNr.Text)) > 0 Then
			LoadEmployeeInterviewData(lueEmployee.EditValue)

		ElseIf Me.tbVorschlag.SelectedTabPage.Name.ToLower = "tabmakontakt".ToLower AndAlso
											CInt(Val(Me.LblRecNr.Text)) > 0 Then

			LoadEmployeeContactData(lueEmployee.EditValue)

		ElseIf Me.tbVorschlag.SelectedTabPage.Name.ToLower = "tabkdkontakt".ToLower AndAlso
											CInt(Val(Me.LblRecNr.Text)) > 0 Then
			LoadCustomerContactData(lueCustomer.EditValue)

		End If

	End Sub


#Region "Datensatz neu..."

	Sub NewProposeData()

		ResetAllTabEntries()
		Me.lueMandant.Enabled = True
		Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr

		Me._ClsSetting.SelectedProposeNr = 0
		Me._ClsSetting.SelectedKDNr = 0
		Me._ClsSetting.SelectedZHDNr = 0
		Me._ClsSetting.SelectedVakNr = 0
		Me._ClsSetting.SelectedMANr = 0

		Me.tbVorschlag.Enabled = False
		Me.pcEditor.Enabled = False

		Me.LblRecNr.Text = "0"
		Me.LblRecID.Text = "0"

		Me.lblBerater2.Visible = False
		Me.lueSecAdvisor.Visible = False

		Me.lueAdvisor.Enabled = ClsDataDetail.bAllowedTochangeKST
		Me.lueSecAdvisor.Enabled = ClsDataDetail.bAllowedTochangeKST
		Me.lueAdvisor.EditValue = String.Format("{0}", m_common.GetLogedUserKst)
		Me.lueSecAdvisor.EditValue = String.Format("{0}", m_common.GetLogedUserKst)

		lueEmployee.Properties.Buttons(0).Enabled = True
		lueEmployee.Properties.Buttons(1).Enabled = True
		lueEmployee.Properties.ReadOnly = False

		lueCustomer.Properties.Buttons(0).Enabled = True
		lueCustomer.Properties.Buttons(1).Enabled = True
		lueCustomer.Properties.ReadOnly = False

		lueCresponsible.Properties.Buttons(0).Enabled = True
		lueCresponsible.Properties.Buttons(1).Enabled = True
		lueCresponsible.Properties.ReadOnly = False

		lueVacancy.Properties.Buttons(0).Enabled = True
		lueVacancy.Properties.Buttons(1).Enabled = True
		lueVacancy.Properties.ReadOnly = False


		'Me.Lib_MAName.Text = String.Empty
		'Me.Lib_KDName.Text = String.Empty
		'Me.Lib_ZhdNr.Text = String.Empty
		'Me.Lib_VakBez.Text = String.Empty

		Me.tbVorschlag.TabIndex = 0
		ResetResponsiblePersonDropDown()
		LoadcResponsibleDropDownData(lueCustomer.EditValue)

		Me.txt_Bezeichnung.Focus()

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()

		For Each ctrls As Control In Me.Controls
			ResetControl(ctrls)
		Next

		ResetDropDown()
		LoadDropDownData()

		grpEmployee.Text = String.Format(m_Translate.GetSafeTranslationValue("Kandidat") & ": ?")
		grpCustomer.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde") & ": ?")

		Me.pcEditor.Enabled = False
		Me.tbVorschlag.Enabled = False

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)

		If TypeOf (con) Is System.Windows.Forms.TextBox Then
			Dim tb As System.Windows.Forms.TextBox = CType(con, System.Windows.Forms.TextBox)
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.MemoExEdit Then
			Dim tb As DevExpress.XtraEditors.MemoExEdit = CType(con, DevExpress.XtraEditors.MemoExEdit)
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CalcEdit Then
			Dim tb As DevExpress.XtraEditors.CalcEdit = CType(con, DevExpress.XtraEditors.CalcEdit)
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
			Dim tb As DevExpress.XtraEditors.DateEdit = CType(con, DevExpress.XtraEditors.DateEdit)
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraRichEdit.RichEditControl Then
			Dim tb As DevExpress.XtraRichEdit.RichEditControl = CType(con, DevExpress.XtraRichEdit.RichEditControl)
			tb.RtfText = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
			cbo.Properties.Items.Clear()
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is ComboBox Then
			Dim cbo As ComboBox = CType(con, ComboBox)
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is GroupBox Then
			Dim grp As Control = con
			For Each con2 As Control In grp.Controls
				ResetControl(con2)
			Next

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.GroupControl Then
			Dim grp As Control = con
			For Each con2 As Control In grp.Controls
				ResetControl(con2)
			Next

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.PanelControl Then
			Dim grp As Control = con
			For Each con2 As Control In grp.Controls
				ResetControl(con2)
			Next

		ElseIf TypeOf (con) Is ListBox Then
			Dim lst As ListBox = CType(con, ListBox)
			lst.Items.Clear()

		ElseIf TypeOf (con) Is ListView Then
			Dim lv As ListView = CType(con, ListView)
			lv.Items.Clear()

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

	End Sub

#End Region

	Private Sub txt_Ab_LBasis_TextChanged(ByVal sender As System.Object,
																				ByVal e As System.EventArgs) Handles txt_Ab_LAnzahl.TextChanged,
																				txt_Ab_LBasis.TextChanged
		'Dim dBas As Double = If(IsNumeric(Me.txt_Ab_LBasis.Text), CDbl(Me.txt_Ab_LBasis.Text), 0)
		'Dim dAnz As Double = If(IsNumeric(Me.txt_Ab_LAnzahl.Text), CDbl(Me.txt_Ab_LAnzahl.Text), 0)

		Dim dBas As Double = 0
		Double.TryParse(txt_Ab_LBasis.EditValue, dBas)
		Dim dAnz As Double = 0
		Double.TryParse(txt_Ab_LAnzahl.EditValue, dAnz)

		txt_Ab_LBetrag.EditValue = Format(dBas * dAnz, "n")

	End Sub

	Private Sub txt_Ab_HBasis_TextChanged(ByVal sender As System.Object,
																				ByVal e As System.EventArgs) Handles txt_Ab_HAnsatz.TextChanged,
																				txt_Ab_HBasis.TextChanged
		Dim dBas As Double = 0
		Double.TryParse(txt_Ab_HBasis.EditValue, dBas)
		Dim dAns As Double = 0
		Double.TryParse(txt_Ab_HAnsatz.EditValue, dAns)

		txt_Ab_HBetrag.EditValue = Format(dBas * dAns / 100, "n")

	End Sub

	Private Sub Cbo_Status_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Status.SelectedIndexChanged
		Dim bInsertIntoKontaktDb As System.Windows.Forms.DialogResult = DialogResult.Yes

		If CInt(Me.LblRecNr.Text) = 0 Then Exit Sub
		If Not DoNotAskForCreatingContact Then
			Dim strMsg = "Möchten Sie einen neuen Eintrag in der Kontaktdatenbank aufnehmen?"
			bInsertIntoKontaktDb = DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg),
																								 m_Translate.GetSafeTranslationValue("Statusänderung"),
																								 MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		End If

		If bInsertIntoKontaktDb = DialogResult.Yes Then
			Dim _ClsFuncDb As New ClsDbFunc
			With _ClsFuncDb
				.GetRecNr = CInt(Val(Me.LblRecNr.Text))
				.GetMANr = lueEmployee.EditValue
				.GetKDNr = lueCustomer.EditValue
				.GetKDZHDNr = lueCresponsible.EditValue
				.GetVakNr = lueVacancy.EditValue

				CreateLogToKontaktDb(lueCustomer.EditValue, lueCresponsible.EditValue, lueEmployee.EditValue, .GetRecNr, lueVacancy.EditValue, ContactString(1))

				LoadEmployeeContactData(lueEmployee.EditValue)
				LoadCustomerContactData(lueCustomer.EditValue)

				If DoOpenContactFormForModification Then
					Dim contactData = CType(gridCustomerContactData.DataSource, BindingList(Of CustomerContactViewData))
					If Not contactData Is Nothing AndAlso contactData.Count > 0 Then
						ShowCustomerContatDetailForm(lueCustomer.EditValue, contactData(0).ContactRecorNumber)
					End If
				End If

			End With

		End If

	End Sub

	Sub DisplayFoundedData(ByVal iRecNr As Integer)
		Dim i As Integer = 0

		m_CurrentProposedata = m_ProposeDatabaseAccess.LoadProposeMasterData(iRecNr)
		If m_CurrentProposedata Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorschlag Daten konnten nicht geladen werden."))
			Return
		End If

		Try
			LoadtransferData()

			lueMandant.EditValue = m_CurrentProposedata.MDNr
			lueMandant.Enabled = m_InitializationData.MDData.MultiMD = 1
			m_AllowedDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 803, lueMandant.EditValue)

			lueAdvisor.EditValue = m_CurrentProposedata.MA_Kst
			lueSecAdvisor.EditValue = m_CurrentProposedata.KD_Kst

			LblRecID.Text = m_CurrentProposedata.ID
			LblRecNr.Text = m_CurrentProposedata.ProposeNr
			txt_Bezeichnung.Text = m_CurrentProposedata.Bezeichnung

			Cbo_Status.Text = m_CurrentProposedata.P_State
			Cbo_Art.Text = m_CurrentProposedata.P_Art

			lueEmployee.EditValue = m_CurrentProposedata.MANr
			txt_ArbBegin.Text = m_CurrentProposedata.P_ArbBegin

			txt_ArbBegin_Date.EditValue = m_CurrentProposedata.P_ArbBegin_Date

			lueCustomer.EditValue = m_CurrentProposedata.KDNr

			lueCresponsible.EditValue = m_CurrentProposedata.KDZHDNr
			lueVacancy.EditValue = m_CurrentProposedata.VakNr

			txt_Tarif.Text = m_CurrentProposedata.KD_Tarif
			txt_Spesen.Text = m_CurrentProposedata.P_Spesen
			txt_ArbZeit.Text = m_CurrentProposedata.P_ArbZeit

			Cbo_Anstellung.Text = m_CurrentProposedata.P_Anstellung

			txt_Ab_Als.Text = m_CurrentProposedata.Ab_AnstellungAls
			txt_Ab_AntrittPer.Text = m_CurrentProposedata.Ab_AntrittPer
			txt_Ab_LBasis.Text = m_CurrentProposedata.Ab_LohnBas
			txt_Ab_LAnzahl.Text = m_CurrentProposedata.Ab_LohnAnz
			txt_Ab_LBetrag.Text = m_CurrentProposedata.Ab_LohnBetrag
			txt_Ab_HBasis.Text = m_CurrentProposedata.Ab_HBas
			txt_Ab_HAnsatz.Text = m_CurrentProposedata.Ab_HAns
			txt_Ab_HBetrag.Text = m_CurrentProposedata.Ab_HBetrag

			txt_Ab_RePer.Text = m_CurrentProposedata.Ab_REPer
			txt_Ab_RePer_Date.EditValue = m_CurrentProposedata.Ab_RePer_Date
			txt_Ab_Bemerkung.Text = m_CurrentProposedata.Ab_Bemerkung

			bsiCreated.Caption = String.Format(" {0:f}, {1}", m_CurrentProposedata.CreatedOn, m_CurrentProposedata.CreatedFrom)
			bsiChanged.Caption = String.Format(" {0:f}, {1}", m_CurrentProposedata.ChangedOn, m_CurrentProposedata.ChangedFrom)

			grpEmployee.Text = String.Format(m_Translate.GetSafeTranslationValue("Kandidat") & ": {0}", lueEmployee.EditValue)
			grpCustomer.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde") & ": {0}", lueCustomer.EditValue)

			LoadEmployeeInterviewData(lueEmployee.EditValue)
			LoadEmployeeContactData(lueEmployee.EditValue)
			LoadCustomerContactData(lueCustomer.EditValue)

			lueAdvisor.Enabled = ClsDataDetail.bAllowedToChange And ClsDataDetail.bAllowedTochangeKST
			lueSecAdvisor.Enabled = ClsDataDetail.bAllowedToChange And ClsDataDetail.bAllowedTochangeKST

			lueEmployee.Properties.Buttons(0).Enabled = ClsDataDetail.bAllowedToChange
			lueEmployee.Properties.Buttons(1).Enabled = ClsDataDetail.bAllowedToChange
			lueEmployee.Properties.ReadOnly = Not ClsDataDetail.bAllowedToChange


			lueCustomer.Properties.Buttons(0).Enabled = ClsDataDetail.bAllowedToChange
			lueCustomer.Properties.Buttons(1).Enabled = ClsDataDetail.bAllowedToChange
			lueCustomer.Properties.ReadOnly = Not ClsDataDetail.bAllowedToChange

			lueCresponsible.Properties.Buttons(0).Enabled = ClsDataDetail.bAllowedToChange And (Not lueCresponsible.EditValue Is Nothing)
			lueCresponsible.Properties.Buttons(1).Enabled = ClsDataDetail.bAllowedToChange And (Not lueCresponsible.EditValue Is Nothing)
			lueCresponsible.Properties.ReadOnly = Not ClsDataDetail.bAllowedToChange

			lueVacancy.Properties.Buttons(0).Enabled = ClsDataDetail.bAllowedToChange And (Not lueVacancy.EditValue Is Nothing)
			lueVacancy.Properties.Buttons(1).Enabled = ClsDataDetail.bAllowedToChange And (Not lueVacancy.EditValue Is Nothing)
			lueVacancy.Properties.ReadOnly = Not ClsDataDetail.bAllowedToChange

			For Each ctl As Control In Controls
				ErrorProvider1.SetError(ctl, String.Empty)
			Next

			ClsDataDetail.GetProposalNr = CInt(LblRecNr.Text)
			ClsDataDetail.GetProposalMANr = lueEmployee.EditValue
			ClsDataDetail.GetProposalKDNr = lueCustomer.EditValue
			ClsDataDetail.GetProposalZHDNr = lueCresponsible.EditValue
			ClsDataDetail.GetProposalVakNr = lueVacancy.EditValue

			m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(lueMandant.EditValue)
			ClsDataDetail.strUSSignFilename = String.Empty

			If _ClsSetting.IsAsDuplicated.GetValueOrDefault(False) Then m_UtilityUI.ShowStandardBadgeNotification(grpAllgemein, m_Badge, AdornerUIManager1,
																												  m_Translate.GetSafeTranslationValue("Der Datensatz wurde erfolgreich dupliziert."),
																												  ContentAlignment.TopCenter, TargetElementRegion.Default,
																												  BadgePaintStyle.Information)
			m_AllowedDelete = m_AllowedDelete AndAlso (Not pnlWOS.Visible)

			If navBarControl.Groups.Count > 0 Then
				Dim Group As NavBarGroup = navBarControl.Groups(1)
				Dim Item As NavBarItem = Group.ItemLinks(0).Item
				Item.Enabled = m_AllowedDelete
			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			Return
		End Try

	End Sub

	Private Sub LoadtransferData()

		If m_CurrentProposedata Is Nothing Then
			pnlWOS.Visible = False

			Return
		End If

		Dim docGuid As String = String.Format("{0}", m_CurrentProposedata.Doc_Guid)
		pnlWOS.Visible = Not String.IsNullOrWhiteSpace(docGuid)
		If Not docGuid Is Nothing AndAlso docGuid.Length > 20 Then
			Dim transferObj = New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			Dim transferSearchData = transferObj.LoadAssignedTransferedCustomerDataFromWOS(m_InitializationData.MDData.MDGuid, m_CurrentProposedata.Doc_Guid, m_CurrentProposedata.ProposeNr)
			lblViewedOn.Appearance.ForeColor = Color.Black
			If transferSearchData Is Nothing Then Return
			Dim transferData = transferSearchData(0)
			If transferData Is Nothing Then Return

			pnlWOS.Visible = (Not transferData Is Nothing) AndAlso (transferData.DocGuid = m_CurrentProposedata.Doc_Guid)
			tgsReportingObligation.EditValue = (transferData.DocGuid = m_CurrentProposedata.Doc_Guid)

			If Not transferData Is Nothing OrElse String.IsNullOrWhiteSpace(transferData.DocGuid) Then
				lblViewedOn.Text = If(Not transferData.DocViewedOn.HasValue, "nicht gesehen!", transferData.DocViewedOn)
				If Not transferData.DocViewedOn.HasValue Then lblViewedOn.Appearance.ForeColor = Color.Red
				lblCustomerFeedback.Text = If(Not transferData.GetResult.HasValue, "?", transferData.GetResult)
				lblTransferedOn.Text = If(Not transferData.CreatedOn.HasValue, String.Empty, transferData.CreatedOn)


				lblFeedback.Visible = transferData.CustomerFeedback_On.HasValue
				lblCustomerFeedback.Visible = transferData.CustomerFeedback_On.HasValue
				lblFeedback.Text = String.Format(m_Translate.GetSafeTranslationValue("Feedback ({0:G})"), transferData.CustomerFeedback_On)
				lblCustomerFeedback.Text = transferData.CustomerFeedback


				If transferData.GetResult.HasValue Then

					If transferData.GetResult.GetValueOrDefault(0) = 2 Then
						lblResult.Text = String.Format("{0}<br>({1:G})", m_Translate.GetSafeTranslationValue("Nicht interessant"), transferData.Get_On)
					ElseIf transferData.GetResult.GetValueOrDefault(0) = 1 Then
						lblResult.Text = String.Format("{0}<br>({1:G})", m_Translate.GetSafeTranslationValue("Interessant"), transferData.Get_On)
					Else
						lblResult.Text = "?"
					End If

				End If
			End If
		End If
		tgsReportingObligation.Enabled = tgsReportingObligation.EditValue

	End Sub

	Private Sub tgsReportingObligation_EditValueChanged(sender As Object, e As EventArgs) Handles tgsReportingObligation.EditValueChanged

		If m_SuppressUIEvents Then Return
		m_SuppressUIEvents = True
		If Not tgsReportingObligation.EditValue Then
			Dim msg As String = "Achtung: hiermit entfernen Sie den Vorschlag aus WOS-Plattform. Möchten Sie wirklich mit dem Vorgang fortfahren?"
			If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Vorschlag vom WOS löschen")) = False Then
				tgsReportingObligation.EditValue = True
				pnlWOS.Visible = True
				m_SuppressUIEvents = False

				Return
			End If

			Dim transferObj = New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			Dim setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting With {.DocumentArtEnum = SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting.DocumentArt.Vorschlag, .AssignedDocumentGuid = m_CurrentProposedata.Doc_Guid}
			transferObj.WOSSetting = setting
			Dim transferData = transferObj.DeleteTransferedCustomerDocument()

			transferData = transferData AndAlso m_WOSDatabaseAccess.UpdateProposeGuidData(m_CurrentProposedata.ProposeNr, String.Empty)
			If transferData Then
				m_CurrentProposedata = m_ProposeDatabaseAccess.LoadProposeMasterData(m_CurrentProposedata.ProposeNr)
				pnlWOS.Visible = False
			End If

			m_AllowedDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 803, lueMandant.EditValue)
			m_AllowedDelete = m_AllowedDelete AndAlso (Not pnlWOS.Visible)

			If navBarControl.Groups.Count > 0 Then
				Dim Group As NavBarGroup = navBarControl.Groups(1)
				Dim Item As NavBarItem = Group.ItemLinks(0).Item
				Item.Enabled = m_AllowedDelete
			End If

		End If

		m_SuppressUIEvents = False

	End Sub

	Sub GetMenuItems4Print()

		Dim mnuData = m_ProposeDatabaseAccess.LoadContextMenu4PrintData
		If (mnuData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Inhalte konnten nicht geladen werden."))

			Return
		End If

		Try

			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			popupMenu.Manager = Me.BarManager1

			Dim itm As New DevExpress.XtraBars.BarButtonItem

			For i As Integer = 0 To mnuData.Count - 1

				itm = New DevExpress.XtraBars.BarButtonItem
				Dim strMnuBez As String = m_Translate.GetSafeTranslationValue(mnuData(i).MnuCaption)

				Dim bAsGroup As Boolean = strMnuBez.StartsWith("-")

				itm.Caption = strMnuBez.Replace("-", "")
				itm.Name = mnuData(i).MnuName

				If bAsGroup Then
					popupMenu.AddItem(itm).BeginGroup = True
				Else
					popupMenu.AddItem(itm)
				End If
				AddHandler itm.ItemClick, AddressOf PrintProposeTemplates

			Next
			Dim mouseposition As New Point(Me.grpAllgemein.Left + Me.Left - Me.navBarControl.Left, Cursor.Position.Y)
			popupMenu.ShowPopup(mouseposition)

		Catch e As Exception
			m_UtilityUI.ShowErrorDialog(String.Format("{0}", e.ToString))

		End Try

	End Sub

	Sub PrintProposeTemplates(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMenuName As String = e.Item.Name.ToString

		Me.PrintJobNr = strMenuName
		Dim _ClsDb As New ClsDbFunc
		Me.SQL4Print = _ClsDb.GetSQLString4Print(0)
		StartPrinting()

	End Sub

	Sub StartPrinting()
		Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLProposeSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																			 .SQL2Open = Me.SQL4Print,
																																			 .JobNr2Print = Me.PrintJobNr, .SelectedMDNr = m_InitializationData.MDData.MDNr, .LogedUSNr = m_InitializationData.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.ProposeSearchListing.ClsPrintProposeSearchList(_Setting)

		obj.PrintProposeTpl_1(ShowDesign, String.Empty, CInt(Val(Me.LblRecNr.Text)), False, False)
	End Sub

	Sub GetTemplateMnu(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Dim strMenuName As String = e.Item.AccessibleName.ToString
		If strMenuName = String.Empty Then Return
		Dim strTemplateFilename As String = strMenuName

		Dim strNewTemplateFile As String = m_path.GetSpS2DeleteHomeFolder & String.Format("tpl_P_{0}_.Doc", Me.strLinkName)
		File.Copy(strTemplateFilename, strNewTemplateFile, True)
		Me.rtfContent.LoadDocument(strNewTemplateFile, DevExpress.XtraRichEdit.DocumentFormat.Doc)

		Try
			Me.rtfContent.RtfText = TranslateSeletedTemplate(Me.rtfContent)

			Try
				Dim strToSearch As String = "#USSign"
				Dim bWithUSSign As Boolean = Me.rtfContent.Text.ToLower.Contains(strToSearch.ToLower)
				Dim strFilename As String = If(bWithUSSign, GetUSSign(m_common.GetLogedUserNr), String.Empty)
				If bWithUSSign AndAlso File.Exists(strFilename) Then
					Dim img As Image = Image.FromFile(strFilename)
					Dim searchResult As ISearchResult = Me.rtfContent.Document.StartSearch(strToSearch, SearchOptions.WholeWord, SearchDirection.Forward, rtfContent.Document.Range)

					rtfContent.Document.BeginUpdate()
					Do While searchResult.FindNext()
						searchResult.Replace(String.Empty)
						'Me.rtfContent.Document.InsertImage(searchResult.CurrentResult.Start, img)
						rtfContent.Document.Images.Insert(searchResult.CurrentResult.Start, img)
					Loop
					rtfContent.Document.EndUpdate()

				Else
					Me.rtfContent.RtfText = Me.rtfContent.RtfText.Replace(strToSearch, String.Empty)

				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Sub SaveLV_ColumnInfo(ByVal lv As ListView, ByVal iCol4What As Integer)
		Dim strColInfo As String = String.Empty
		Dim strColInfo_1 As String = String.Empty
		Dim strColAlign As String = String.Empty

		For i As Integer = 0 To lv.Columns.Count - 1
			If lv.Columns.Item(i).TextAlign = HorizontalAlignment.Center Then
				strColAlign = "2"

			ElseIf lv.Columns.Item(i).TextAlign = HorizontalAlignment.Right Then
				strColAlign = "1"
			Else
				strColAlign = "0"
			End If
			strColInfo &= CStr(IIf(strColInfo = String.Empty, "", ";")) & (lv.Columns.Item(i).Width) & "-" & strColAlign

		Next

		Try

			Select Case iCol4What
				Case 1    ' MA
					My.Settings.LV_1_Size_SPProposeUtility = strColInfo

				Case 2    ' KD
					My.Settings.LV_2_Size_SPProposeUtility = strColInfo

				Case 3    ' Job
					My.Settings.LV_3_Size_SPProposeUtility = strColInfo

				Case 4    ' Vak
					My.Settings.LV_4_Size_SPProposeUtility = strColInfo

				Case 5    ' MAVorstellung
					My.Settings.LV_5_Size_SPProposeUtility = strColInfo

				Case 6    ' MAKontakt
					My.Settings.LV_6_Size_SPProposeUtility = strColInfo

				Case 7    ' KDKontakt
					My.Settings.LV_7_Size_SPProposeUtility = strColInfo

			End Select
			My.Settings.Save()

		Catch ex As Exception
			'_ClsErrException.MessageBoxShowWarning("SaveLV_ColumnInfo", ex, "Die Einstellungen konnten nicht gespeichert werden.")

		End Try

	End Sub

	Sub CloseForm()

		Try
			Try

				My.Settings.iHeight_SPProposeUtility = Me.Height
				My.Settings.iWidth_SPProposeUtility = Me.Width
				My.Settings.frmLocation_SPProposeUtility = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

				My.Settings.Save()

			Catch ex As Exception

			End Try

			Me.Close()
			Me.Dispose()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txt_Tarif_TextChanged(ByVal sender As Object,
																		ByVal e As System.EventArgs) Handles txt_Tarif.TextChanged,
																		txt_Spesen.TextChanged, txt_Ab_Bemerkung.TextChanged

		Me.txt_Tarif.Properties.ShowIcon = Not Me.txt_Tarif.Text.Length > 0
		Me.txt_Spesen.Properties.ShowIcon = Not Me.txt_Spesen.Text.Length > 0
		Me.txt_Ab_Bemerkung.Properties.ShowIcon = Not Me.txt_Ab_Bemerkung.Text.Length > 0

		Me.txt_Tarif.Properties.PopupFormSize = New Size(Me.txt_Tarif.Width, 0)
		Me.txt_Spesen.Properties.PopupFormSize = New Size(Me.txt_Spesen.Width, 0)
		Me.txt_Ab_Bemerkung.Properties.PopupFormSize = New Size(Me.txt_Ab_Bemerkung.Width, 0)

	End Sub

	Private Sub nbMain_GroupExpanded(ByVal sender As Object,
																	 ByVal e As DevExpress.XtraNavBar.NavBarGroupEventArgs) Handles navBarControl.GroupExpanded
		Dim strGroupLinkname As String = e.Group.Name

		Try
			Select Case strGroupLinkname.ToLower
				Case "gNavDatei".ToLower
				Case "gNavDelete".ToLower
					Me.navBarControl.Groups.Item("gNavMAZusatzFelder").Expanded = False
				Case "gNavSearch".ToLower
					Me.navBarControl.Groups.Item("gNavMAZusatzFelder").Expanded = False
				Case "gNavExtras".ToLower
					Me.navBarControl.Groups.Item("gNavMAZusatzFelder").Expanded = False
				Case "gNavzusatzfelder".ToLower
					Me.navBarControl.Groups.Item("gNavDelete").Expanded = False
					Me.navBarControl.Groups.Item("gNavSearch").Expanded = False
				Case "gNavMAZusatzFelder".ToLower
					Me.navBarControl.Groups.Item("gNavDelete").Expanded = False
					Me.navBarControl.Groups.Item("gNavSearch").Expanded = False

			End Select

		Catch ex As Exception

		End Try
		Trace.WriteLine(sender.ToString)

	End Sub

	Private Sub nbMain_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navBarControl.LinkClicked

		Me.rtfContent.RtfText = String.Empty
		Try
			Trace.WriteLine(String.Format("{0} >>> {1}", e.Link.ItemName, e.Link.Caption))
			strLinkName = e.Link.ItemName
			strLinkCaption = e.Link.Caption

			For i As Integer = 0 To Me.navBarControl.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			If Not strLinkName.ToLower.Contains("save") And
							Not strLinkName.ToLower.Contains("delete") And
							Not strLinkName.ToLower.Contains("print") And
							Not strLinkName.ToLower.Contains("new") Then e.Link.Item.Appearance.ForeColor = Color.Orange
			Me.pcEditor.Left = Me.grpAllgemein.Left

			Select Case strLinkName.ToLower
				Case "New_Propose".ToLower
					NewProposeData()

				Case "Save_Propose_Data".ToLower
					SaveSelectedPropose(True)

				Case "Print_Propose_Data".ToLower
					SaveSelectedPropose(False)
					If CInt(Val(Me.LblRecNr.Text)) > 0 Then GetMenuItems4Print()

				Case "Close_Propose_Form".ToLower
					CloseForm()

				Case "delete_propose_Data".ToLower
					If m_CurrentProposedata.ProposeNr.GetValueOrDefault(0) > 0 Then
						If DeleteSelectedPropose() Then CloseForm()
					End If

				Case "Abhängigkeiten_P".ToLower
					If CInt(Val(Me.LblRecNr.Text)) > 0 Then
						Me.tbVorschlag.Visible = True
						Me.pcEditor.Visible = False
					End If

				Case "P_SendeMail".ToLower
					SaveSelectedPropose(False)
					If CInt(Val(Me.LblRecNr.Text)) > 0 Then
						OpenEMailForm(Me.LblRecNr.Text, lueEmployee.EditValue, lueCustomer.EditValue, lueCresponsible.EditValue,
													m_mandant.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Templates", "PMail_tplDocNr", ""))
					End If
					Return

				Case "Duplicate_Propose_Data".ToLower
					AdornerUIManager1.Elements.Remove(m_Badge)
					AdornerUIManager1.Hide()

					Me.Enabled = False
					fpDuplicateData.OwnerControl = grpAllgemein
					fpDuplicateData.OptionsBeakPanel.CloseOnOuterClick = False
					fpDuplicateData.OptionsBeakPanel.AnimationType = Win.PopupToolWindowAnimation.Fade
					fpDuplicateData.OptionsBeakPanel.BeakLocation = BeakPanelBeakLocation.Top

					fpDuplicateData.ShowBeakForm()

					Return

				Case "P_MakeES".ToLower
					SaveSelectedPropose(False)
					If CInt(Val(Me.LblRecNr.Text)) > 0 Then
						OpenESForm(ClsDataDetail.GetProposalMANr,
											 ClsDataDetail.GetProposalKDNr,
											 ClsDataDetail.GetProposalZHDNr,
											 lueVacancy.EditValue)
					End If


				Case Else
					If CBool(CStr(strLinkName.ToLower.StartsWith("MA_LL_".ToLower))) Then
						Me.pcEditor.Visible = True
						Me.tbVorschlag.Visible = False
						ListTemplates("employee", Mid(strLinkName, 7, strLinkName.Length))

						DisplaySelectedMALLDbField(Mid(strLinkName, 7, strLinkName.Length))
						bChangedContent = False

					ElseIf CBool(CStr(strLinkName.ToLower.StartsWith("P_Zusatz_".ToLower))) Then
						Me.pcEditor.Visible = True
						Me.tbVorschlag.Visible = False

						ListTemplates("propose", Mid(strLinkName, 10, strLinkName.Length))
						DisplaySelectedProposeDbField(Mid(strLinkName, 10, strLinkName.Length))
						Me.rtfContent.SaveDocument(String.Format("{0}test.rtf", m_path.GetSpS2DeleteHomeFolder), DevExpress.XtraRichEdit.DocumentFormat.Rtf)
						bChangedContent = False

					End If


			End Select
			If Not Me.tbVorschlag.Visible And Not Me.pcEditor.Visible Then Me.tbVorschlag.Visible = True


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

	End Sub

	Private Sub richEditControl1_DocumentLoaded(ByVal sender As Object, ByVal e As EventArgs)

		rtfContent.Font = CType(oFontName, System.Drawing.Font)
		rtfContent.Document.DefaultCharacterProperties.FontName = oFontName.Name
		rtfContent.Document.DefaultCharacterProperties.FontSize = oFontName.Size

	End Sub

	Sub DisplaySelectedMALLDbField(ByVal strFieldName As String)
		Dim strFieldValue As String = String.Empty

		If strFieldName <> String.Empty Then strFieldValue = GetMALLDbFieldValue(strFieldName, lueEmployee.EditValue)
		If strFieldValue.ToLower.Contains("{\rtf1\") Then
			Me.rtfContent.RtfText = strFieldValue
		Else
			Me.rtfContent.Text = strFieldValue
		End If

	End Sub

	Sub SaveSelectedMALLDbField(ByVal strFieldName As String)
		Dim strFieldValue As String = Me.rtfContent.RtfText

		If strFieldName <> String.Empty Then strFieldValue = SaveMALLDbFieldValue(strFieldName,
																																					Me.rtfContent.RtfText,
																																					Me.rtfContent.Text,
																																					lueEmployee.EditValue)
		If strFieldName.ToLower.Contains(m_Translate.GetSafeTranslationValue("fehler")) Then
			DevExpress.XtraEditors.XtraMessageBox.Show(Err.GetException.ToString,
																								 String.Format("SaveSelectedMALLDbField ()", strFieldName),
																									MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

	End Sub

	Sub DisplaySelectedProposeDbField(ByVal strFieldName As String)
		Dim strFieldValue As String = String.Empty

		If strFieldName <> String.Empty Then strFieldValue = GetMAProposeDbFieldValue(strFieldName, CInt(Val(Me.LblRecNr.Text)))
		If strFieldValue.ToLower.Contains("{\rtf1\") Then
			Me.rtfContent.RtfText = strFieldValue
		Else
			Me.rtfContent.Text = strFieldValue
		End If

	End Sub

	Sub SaveSelectedMAProposeDbField(ByVal strFieldName As String)
		Dim strFieldValue As String = Me.rtfContent.RtfText

		If strFieldName <> String.Empty Then strFieldValue = SaveMAProposeDbFieldValue(strFieldName,
																																					Me.rtfContent.RtfText,
																																					Me.rtfContent.Text,
																																					CInt(Val(Me.LblRecNr.Text)))
		If strFieldValue.ToLower.Contains(m_Translate.GetSafeTranslationValue("fehler")) Then
			DevExpress.XtraEditors.XtraMessageBox.Show(Err.GetException.ToString & vbNewLine & strFieldValue,
																								 "SaveSelectedMAProposeDbField",
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

	End Sub

	Private Sub navBarControl_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles navBarControl.MouseMove
		' calculating hit information by the current mouse pointer position
		Dim HitInfo As NavBarHitInfo = navBarControl.CalcHitInfo(New Point(e.X, e.Y))
		MouseX = e.X
		MouseY = e.Y
		If HitInfo.InLink Then
			Dim Link As NavBarItemLink = HitInfo.Link
			'strLinkName = HitInfo.Link.ItemName
			'strLinkCaption = HitInfo.Link.Caption

			'Trace.WriteLine(String.Format("{0} >>> {1}", HitInfo.Link.ItemName, HitInfo.Link.Caption))
			' perform operations on the link here
			' ...
		End If

	End Sub

	Private Sub SaveMyContent()

		If CBool(CStr(strLinkName.ToLower.StartsWith("ma_ll_"))) Then
			Me.SaveSelectedMALLDbField(Me.strLinkName)

		ElseIf CBool(CStr(strLinkName.ToLower.StartsWith("p_zusatz"))) Then
			Me.SaveSelectedMAProposeDbField(Mid(Me.strLinkName, 10, 20))

		Else
			Exit Sub

		End If

	End Sub

	Private Sub FileSaveItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles FileSaveItem1.ItemClick
		SaveMyContent()

	End Sub

	Sub ReloadDropDownData()

		ResetResponsiblePersonDropDown()
		LoadcResponsibleDropDownData(lueCustomer.EditValue)

	End Sub

	Private Sub rtfContent_RtfTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtfContent.RtfTextChanged
		Me.rtfContent.Modified = True
		Me.FileSaveItem1.Enabled = True
		bChangedContent = True
	End Sub


#Region "View helper classes"

	''' <summary>
	''' Advisor view data.
	''' </summary>
	Private Class AdvisorViewData

		Public Property UserNumber As Integer
		Public Property KST As String
		Public Property Salutation As String
		Public Property FristName As String
		Public Property LastName As String

		Public ReadOnly Property LastName_FirstName As String
			Get
				Return LastName & ", " & FristName
			End Get
		End Property

		Public ReadOnly Property Fullname As String
			Get
				Return String.Format("{0} {1} {2}", Salutation, FristName, LastName)
			End Get
		End Property

	End Class


	Private Class ZHDViewData

		Public Property zNumber As Integer
		Public Property zSalutation As String
		Public Property zFristName As String
		Public Property zLastName As String

		Public ReadOnly Property zFullname As String
			Get
				Return String.Format("{0} {1} {2}", zSalutation, zFristName, zLastName)
			End Get
		End Property

	End Class


	''' <summary>
	''' Responsible person view data.
	''' </summary>
	Class ResponsiblePersonViewData

		Public Property Lastname As String
		Public Property Firstname As String
		Public Property TranslatedSalutation As String
		Public Property ResponsiblePersonRecordNumber As Integer?

		Public ReadOnly Property SalutationLastNameFirstName
			Get
				Return String.Format("{0} {1} {2}", TranslatedSalutation, Lastname, Firstname)
			End Get
		End Property
	End Class


#Region "GridSettings"

	Private Sub OngvEmployeeColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvEmployee.SaveLayoutToXml(m_GVEmployeeSettingfilename)

	End Sub

	Private Sub OngvCustomerColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvCustomer.SaveLayoutToXml(m_GVCustomerSettingfilename)

	End Sub

	Private Sub OngvCResponsibleColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvlueCresponsible.SaveLayoutToXml(m_GVCResponsibleSettingfilename)

	End Sub

	Private Sub OngvVacancyColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvVacancy.SaveLayoutToXml(m_GVVacancySettingfilename)

	End Sub

#End Region


#End Region





	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1
		Const ID_OF_OPEN_BUTTON As Int32 = 2

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is DevExpress.XtraEditors.BaseEdit Then
				If CType(sender, DevExpress.XtraEditors.BaseEdit).Properties.ReadOnly Then
					' nothing
				Else
					CType(sender, DevExpress.XtraEditors.BaseEdit).EditValue = Nothing
				End If
			End If

		ElseIf e.Button.Index = ID_OF_OPEN_BUTTON Then

			If sender.name.tolower.contains("lueemployee") Then
				OpenMAForm(Me, lueEmployee.EditValue)

			ElseIf sender.name.tolower.contains("luecustomer") Then
				OpenKDForm(Me, lueCustomer.EditValue)

			ElseIf sender.name.tolower.contains("luecresponsible") Then
				If Me.lueCresponsible.EditValue Is Nothing Then Exit Sub
				OpenKDZHDForm(lueCustomer.EditValue, lueCresponsible.EditValue)

			ElseIf sender.name.tolower.contains("luevacancy") Then
				OpenVakForm(lueVacancy.EditValue)

			End If

		End If


	End Sub

	''' <summary>
	''' Gets the age in years.
	''' </summary>
	''' <param name="birthDate">The birthdate.</param>
	''' <returns>Age in years.</returns>
	Private Function GetAge(ByVal birthDate As DateTime) As Integer

		' Get year diff
		Dim years As Integer = DateTime.Now.Year - birthDate.Year

		birthDate = birthDate.AddYears(years)

		' Subtract another year if its a day before the the birth day
		If (DateTime.Today.CompareTo(birthDate) < 0) Then
			years = years - 1
		End If

		Return years

	End Function




#Region "Save ContactData"

	Private Sub CreateLogToKontaktDb(ByVal m_CustomerNumber As Integer, ByVal m_ResponsiblePersonRecordNumber As Integer?, ByVal m_EmployeeNumber As Integer?,
				 ByVal m_ProposeNumber As Integer, ByVal m_VacancyNumber As Integer?,
				 ByVal strProposeBez As String)
		Dim contactData As ResponsiblePersonAssignedContactData = Nothing
		Dim m_CurrentContactRecordNumber As Integer?
		Dim m_UtilityUI As New SP.Infrastructure.UI.UtilityUI

		Dim dt = DateTime.Now
		If Not m_CurrentContactRecordNumber.HasValue Then
			contactData = New ResponsiblePersonAssignedContactData With {.CustomerNumber = m_CustomerNumber,
																																		 .ResponsiblePersonNumber = m_ResponsiblePersonRecordNumber,
																																		 .CreatedOn = dt,
																																		 .CreatedFrom = m_InitializationData.UserData.UserFullName}
		Else

			contactData = m_CustomerDatabaseAccess.LoadAssignedContactDataOfResponsiblePerson(m_CustomerNumber, m_ResponsiblePersonRecordNumber, m_CurrentContactRecordNumber)

			If contactData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht gespeichert werden."))
				Return
			End If

		End If

		contactData.CustomerNumber = m_CustomerNumber
		contactData.ContactDate = Now.ToString("G")
		contactData.ResponsiblePersonNumber = If(Not m_ResponsiblePersonRecordNumber.HasValue, 0, m_ResponsiblePersonRecordNumber.Value)
		contactData.ContactType1 = "Information"
		contactData.ContactPeriodString = strProposeBez
		contactData.ContactsString = strProposeBez
		contactData.ContactImportant = False
		contactData.ContactFinished = False
		contactData.MANr = m_EmployeeNumber
		contactData.VacancyNumber = m_VacancyNumber
		contactData.ProposeNr = m_ProposeNumber
		contactData.ESNr = Nothing

		contactData.ChangedFrom = m_InitializationData.UserData.UserFullName
		contactData.ChangedOn = dt
		contactData.UsNr = m_InitializationData.UserData.UserNr

		Dim isNewContact = (contactData.ID = 0)

		Dim success As Boolean = True


		' Insert or update contact
		If isNewContact Then
			contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(contactData)

			If success Then
				m_CurrentContactRecordNumber = contactData.RecordNumber
			End If

		Else
			contactData.ChangedUserNumber = m_InitializationData.UserData.UserNr
			success = success AndAlso m_CustomerDatabaseAccess.UpdateResponsiblePersonAssignedContactData(contactData)
		End If


		If Not success Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht gespeichert werden."))

		Else

			' ---Copy contact---

			If Not m_EmployeeNumber Is Nothing Then
				' The user has selected a list of employees -> copy the contact for all of them.
				success = success AndAlso CopyContactForEmployees(contactData, Nothing, New Integer() {m_EmployeeNumber})

			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht kopiert werden."))
			End If

			' --end of copy contact ---

			' Notifiy system about changed contact
			Dim hub = MessageService.Instance.Hub
			Dim customerContactHasChangedMsg As New CustomerContactDataHasChanged(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CustomerNumber, m_CurrentContactRecordNumber)
			hub.Publish(customerContactHasChangedMsg)

		End If


	End Sub

	''' <summary>
	''' Copies the contact for employees.
	''' </summary>
	''' <param name="contactData">The contact data to copy.</param>
	''' <param name="employeeNumbers">The employee numbers.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function CopyContactForEmployees(ByVal contactData As ResponsiblePersonAssignedContactData,
																					 ByVal contactDocumentID As Integer?,
																					 ByVal employeeNumbers As Integer()) As Boolean

		Dim success As Boolean = True

		Dim dt = DateTime.Now

		' Load dependent employee contact list
		Dim dependentEmployeeContactList = m_CustomerDatabaseAccess.LoadCustomerDependentEmployeeContactData(contactData.ID)

		' Copy contact for each employee.
		For Each employeeNumber In employeeNumbers

			' Create a shallow copy
			Dim copyOfContactData As DatabaseAccess.Employee.DataObjects.ContactMng.EmployeeContactData

			Dim employeeNr As Integer = employeeNumber
			Dim dependentEmployeeContact = dependentEmployeeContactList.Where(Function(data) data.EmployeeNumber = employeeNr).FirstOrDefault()

			If dependentEmployeeContact Is Nothing Then
				' Its a new contact
				copyOfContactData = New DatabaseAccess.Employee.DataObjects.ContactMng.EmployeeContactData
				copyOfContactData.ID = Nothing
				copyOfContactData.RecordNumber = Nothing
			Else
				' Update existing contact
				copyOfContactData = m_EmployeeDatabaseAccess.LoadEmployeeContact(employeeNumber, dependentEmployeeContact.EmployeeContactRecordNumber)

				If copyOfContactData Is Nothing Then
					Return False
				End If
			End If

			' Overwrite values
			copyOfContactData.EmployeeNumber = employeeNumber
			copyOfContactData.ContactsString = contactData.ContactsString
			copyOfContactData.ContactType1 = contactData.ContactType1
			copyOfContactData.ContactType2 = contactData.ContactType2
			copyOfContactData.ContactDate = contactData.ContactDate
			copyOfContactData.ContactPeriodString = contactData.ContactPeriodString
			copyOfContactData.ContactImportant = contactData.ContactImportant
			copyOfContactData.ContactFinished = contactData.ContactFinished
			copyOfContactData.CreatedOn = dt
			copyOfContactData.CreatedFrom = m_InitializationData.UserData.UserFullName
			copyOfContactData.ProposeNr = contactData.ProposeNr
			copyOfContactData.VacancyNumber = contactData.VacancyNumber
			copyOfContactData.OfNumber = contactData.OfNumber
			copyOfContactData.Mail_ID = contactData.Mail_ID
			copyOfContactData.TaskRecNr = contactData.TaskRecNr
			copyOfContactData.UsNr = contactData.UsNr
			copyOfContactData.ESNr = contactData.ESNr
			copyOfContactData.CustomerNumber = lueCustomer.EditValue
			copyOfContactData.CustomerContactRecId = contactData.ID
			copyOfContactData.KontaktDocID = contactData.KontaktDocID

			copyOfContactData.ChangedFrom = m_InitializationData.UserData.UserFullName
			copyOfContactData.ChangedOn = dt


			' Save the contact

			If copyOfContactData.ID > 0 Then
				copyOfContactData.ChangedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeContact(copyOfContactData)
			Else
				copyOfContactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(copyOfContactData)
			End If

			If Not success Then
				Exit For
			End If

		Next

		Return success
	End Function


#End Region







#Region "View helper classes"

	''' <summary>
	''' Job interview view data.
	''' </summary>
	Class JobInterviewViewData

		Public Property ID As Integer
		Public Property RecordNumber As Integer?
		Public Property AppointmentDate As DateTime?
		Public Property JobTitle As String
		Public Property Company As String
		Public Property JobAppointmentState As String
		Public Property Location As String
		Public Property Telephone As String
		Public Property Telefax As String
		Public Property Homepage As String
		Public Property Email As String
		Public Property Outcome As String
		Public Property VakNr As Integer?
		Public Property ProposeNr As Integer?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property CustomerNumber As Integer?
		Public Property ResponsiblePersonRecordNumber As Integer?

	End Class


	''' <summary>
	'''  Employee Contact view data.
	''' </summary>
	Class EmployeeContactViewData
		Public Property ID As Integer
		Public Property EmployeeNumber As Integer
		Public Property ContactRecorNumber As Integer
		Public Property ContactDate As DateTime?
		Public Property minContactDate As DateTime?
		Public Property maxContactDate As DateTime?
		Public Property Person_Subject As String
		Public Property Description As String
		Public Property Important As Boolean?
		Public Property Completed As Boolean?
		Public Property CreatedFrom As String
		Public Property PDFImage As Image
		Public Property DocumentId As Integer?
		Public Property KDKontactRecID As Integer?
	End Class


	''' <summary>
	'''  Contact view data.
	''' </summary>
	Class CustomerContactViewData
		Public Property ID As Integer
		Public Property CustomerNumber As Integer
		Public Property ContactRecorNumber As Integer
		Public Property ContactDate As DateTime?
		Public Property minContactDate As DateTime?
		Public Property maxContactDate As DateTime?
		Public Property Person_Subject As String
		Public Property Description As String
		Public Property Important As Boolean?
		Public Property Completed As Boolean?
		Public Property Creator As String
		Public Property PDFImage As Image
		Public Property DocumentId As Integer?
	End Class


#End Region


	Function LvVorstellung() As ListView
		Throw New NotImplementedException
	End Function

End Class

