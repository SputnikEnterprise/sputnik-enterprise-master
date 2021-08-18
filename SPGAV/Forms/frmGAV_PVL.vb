
Option Strict Off

Imports System.Reflection.Assembly
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraRichEdit
Imports System.Drawing.Printing
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPGAV.ClsDataDetail
Imports System.ComponentModel

Imports SP.DatabaseAccess.Common

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SPGAV.SPPVLGAVUtilWebService
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Report
Imports SP.Infrastructure

Public Class frmGAV_PVL
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Constants"

	Private Const DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI = "wsSPS_services/SPPVLGAVUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"
	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "private fields"

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
	Private m_ESDatabaseAccess As IESDatabaseAccess
	Private m_ReportDataAccess As IReportDatabaseAccess


	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private bNoPVL As Boolean
	Private bIsQ12Enabled As Boolean
	Private m_CurrDbName As String = String.Empty
	Private PublicationOfGAV As Date

	Private _MAAlter As String = "19"
	Private _KDPLZ As String = "5000"

	Private m_cFAGAnhang1 As Decimal
	Private m_cFANAnhang1 As Decimal

	Private _cWAGAnhang1 As Single
	Private _cWANAnhang1 As Single
	Private _cWAGAnhang1_S As Single
	Private _cWANAnhang1_S As Single
	Private _cWAGAnhang1_J As Single
	Private _cWANAnhang1_J As Single

	Private _cVAGAnhang1 As Single
	Private _cVANAnhang1 As Single
	Private _cVAGAnhang1_S As Single
	Private _cVANAnhang1_S As Single
	Private _cVAGAnhang1_J As Single
	Private _cVANAnhang1_J As Single

	Private bFarWithBVG As Boolean
	Private iFarCalc As Short

	Private aLblControls As New ArrayList()
	Private aLblLODataControls As New ArrayList()
	Private aCboControls As New ArrayList()
	Private aCategoryValuesNr As New List(Of String)

	Private m_LOData As GAVCalculationDTO
	Private liLOData As New List(Of String)
	Private liBauQ12LOData As New List(Of String)

	Private Property _bResorpflichtig As Boolean

	Private m_path As ClsProgPath
	Private m_md As Mandant
	Private m_UtilityUI As UtilityUI

	Private m_AllowedChangeFAR As Boolean

	Private m_SPPVLUtilitiesServiceUrl As String

	Private m_ExistingEmployeeData As EmployeeMasterData
	Private m_ExistingCustomerGAVData As IEnumerable(Of CustomerAssignedGAVGroupData)
	Private m_ExistingESLohnData As IEnumerable(Of ESSalaryData)
	Private m_PVLListData As IEnumerable(Of GAVNameResultDTO)
	Private m_CurrentPVLData As GAVNameResultDTO

	Private m_CategoryData As BindingList(Of GAVCategoryDTO)

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The utility.
	''' </summary>
	Private m_Utility As New Utility
	Private m_PVLArchiveDbName As String
	Private m_PVLSelectedData As String


#End Region


#Region "Public Properties"

	Public Property EmployeeNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property EmploymentNumber As Integer
	Public Property CustomerCanton As String
	Public Property ExistingGAVInfo As String

#End Region


#Region "private properties"

	Private ReadOnly Property SelectedPVLData As GAVNameResultDTO
		Get
			Dim SelectedData = TryCast(lueGruppe0.GetSelectedDataRow(), GAVNameResultDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory0Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue0.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory1Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue1.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory2Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue2.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory3Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue3.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory4Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue4.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory5Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue5.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory6Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue6.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	Private ReadOnly Property SelectedPVLCategory7Data As GAVCategoryValueDTO
		Get
			Dim SelectedData = TryCast(lue7.GetSelectedDataRow(), GAVCategoryValueDTO)

			Return SelectedData
		End Get

	End Property

	''' <summary>
	''' Gets the flexible time from database setting
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Private ReadOnly Property GetflexibletimeFromDatabase As Boolean
		Get

			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

			Dim value As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/getflextimefrommandantdatabase", FORM_XML_MAIN_KEY)), False)

			Return value

		End Get
	End Property

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_md = New Mandant
		m_path = New ClsProgPath
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		Dim m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ReportDataAccess = New ReportDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_PVLArchiveDbName = String.Empty

		Dim domainName = m_InitializationData.MDData.WebserviceDomain
		m_SPPVLUtilitiesServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI)


		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()
		Reset()

		InitialCommonControls()
		CreateExternalLinks()

		m_AllowedChangeFAR = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 264, m_InitializationData.MDData.MDNr)

	End Sub


#End Region


#Region "public properties"

	Public ReadOnly Property GetAssignedPVLDatabase As String
		Get
			Return m_PVLArchiveDbName
		End Get
	End Property

	Public ReadOnly Property GetAssignedPVLData As String
		Get
			Return m_PVLSelectedData
		End Get
	End Property


#End Region

#Region "public methodes"

	Public Sub LoadData()

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		LoadPVLArchiveDbData()
		LoadCantonDropDownData()
		LoadEmployeeData()
		LoadCustomerData()
		If EmploymentNumber > 0 Then LoadEmploymentData()

		m_ExistingCustomerGAVData = m_CustomerDatabaseAccess.LoadAssignedGAVGroupDataOfCustomer(CustomerNumber)

		m_PVLArchiveDbName = luePVLArchiveDb.EditValue

		LoadPVLDropDownData()

		LoadAllowedPVLData()

		'If EmploymentNumber > 0 Then
		'	m_ExistingESLohnData = m_ESDatabaseAccess.LoadESSalaryData(EmploymentNumber)

		'	Dim actualESLohnData = m_ExistingESLohnData.Where(Function(x) x.AktivLODaten = True).ToList()
		'	If Not actualESLohnData Is Nothing AndAlso actualESLohnData.Count > 0 Then
		'		m_PVLListData = m_PVLListData.Where(Function(x) x.gav_number = actualESLohnData(0).GAVNr).ToList()
		'	End If

		'ElseIf CustomerNumber > 0 Then
		'	Dim mainPVLData As New BindingList(Of GAVNameResultDTO)
		'	For Each itm In m_ExistingCustomerGAVData
		'		Dim data = m_PVLListData.Where(Function(x) x.gav_number = itm.GAVNUmber).FirstOrDefault()
		'		If Not data Is Nothing Then
		'			mainPVLData.Add(data)
		'		End If
		'	Next

		'	m_PVLListData = mainPVLData ' m_PVLListData.GroupBy(Function(m) m).Where(Function(g) g.Count() = 1).Select(Function(g) g.Key).ToList
		'End If

		'lueGruppe0.Properties.DataSource = m_PVLListData
		'If m_PVLListData.Count = 1 Then lueGruppe0.EditValue = m_PVLListData(0).gav_number

		m_SuppressUIEvents = suppressUIEventsState

	End Sub


#End Region


	Private Sub InitialCommonControls()

		AddHandler luePVLArchiveDb.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueGruppe0.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lue0.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue2.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue3.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue4.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue5.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue6.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lue7.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is GridLookUpEdit Then
				Dim dateEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub


	Private Function LoadAllowedPVLData() As Boolean
		Dim success As Boolean = True

		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		If EmploymentNumber > 0 Then
			m_ExistingESLohnData = m_ESDatabaseAccess.LoadESSalaryData(EmploymentNumber)

			Dim actualESLohnData = m_ExistingESLohnData.Where(Function(x) x.AktivLODaten = True).ToList()
			If Not actualESLohnData Is Nothing AndAlso actualESLohnData.Count > 0 Then
				m_PVLListData = m_PVLListData.Where(Function(x) x.gav_number = actualESLohnData(0).GAVNr).ToList()
			End If

		ElseIf CustomerNumber > 0 Then
			Dim mainPVLData As New BindingList(Of GAVNameResultDTO)
			For Each itm In m_ExistingCustomerGAVData
				Dim data = m_PVLListData.Where(Function(x) x.gav_number = itm.GAVNUmber).FirstOrDefault()
				If Not data Is Nothing Then
					mainPVLData.Add(data)
				End If
			Next

			m_PVLListData = mainPVLData
		End If

		lueGruppe0.Properties.DataSource = m_PVLListData
		If m_PVLListData.Count = 1 Then lueGruppe0.EditValue = m_PVLListData(0).gav_number

		m_SuppressUIEvents = suppressUIEventsState


		Return success

	End Function

	Private Sub Reset()

		btnTestXML.Visible = m_InitializationData.UserData.UserNr = 1

		ResetAllLBL()
		ResetPVLArchiveDbDropDown()
		ResetCantonDropDown()
		ResetGruppe0DropDown()

		Me.lblSputnikMessage.Text = String.Empty
		Me.lblAchtung.Visible = False
		bNoPVL = IsNoPVL ' If(Val(_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "NotPVL", "0")) = 0, False, True)
		lblAVE.Text = String.Empty

		Me.lblWStd.Text = 0
		Me.lblMStd.Text = 0
		Me.lblJStd.Text = 0

		Me.lblFAG.Text = 0
		Me.lblFAN.Text = 0
		Me.lblPAG.Text = 0
		Me.lblPAN.Text = 0

		Me.lblHeaderFett.AllowHtmlString = True
		If bNoPVL Then
			Me.lblHeaderFett.Text = Me.grpGAVBeruf.Text & String.Format(": <color=red>({0})</color>", m_Translate.GetSafeTranslationValue("Achtung: Kein PVL-GAV"))
		Else
			Me.lblHeaderFett.Text = Me.grpGAVBeruf.Text & String.Format(": <color=blue>({0})</color>", m_Translate.GetSafeTranslationValue("PVL-GAV"))

		End If
		m_PVLSelectedData = String.Empty

	End Sub

	Private Sub ResetPVLArchiveDbDropDown()

		luePVLArchiveDb.Properties.DisplayMember = "DbName"
		luePVLArchiveDb.Properties.ValueMember = "DbName"

		' Reset the grid view
		gvLuePVLArchiveDb.OptionsView.ShowIndicator = False

		gvLuePVLArchiveDb.Columns.Clear()

		Dim columnBranchText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBranchText.Caption = m_Translate.GetSafeTranslationValue("Verfügbare Datenbanken")
		columnBranchText.Name = "DbName"
		columnBranchText.FieldName = "DbName"
		columnBranchText.Visible = True
		gvLuePVLArchiveDb.Columns.Add(columnBranchText)

		luePVLArchiveDb.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePVLArchiveDb.Properties.NullText = String.Empty
		luePVLArchiveDb.Properties.DataSource = Nothing
		luePVLArchiveDb.EditValue = Nothing

	End Sub

	Private Sub ResetCantonDropDown()

		lueCanton.Properties.DisplayMember = "Description"
		lueCanton.Properties.ValueMember = "GetField"

		Dim columns = lueCanton.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
		columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Kanton")))

		lueCanton.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCanton.Properties.SearchMode = SearchMode.AutoComplete
		lueCanton.Properties.AutoSearchColumnIndex = 0
		lueCanton.Properties.NullText = String.Empty
		lueCanton.EditValue = Nothing

	End Sub

	Private Sub ResetGruppe0DropDown()

		lueGruppe0.Properties.DisplayMember = "name_de"
		lueGruppe0.Properties.ValueMember = "gav_number"

		gvGruppe0.OptionsView.ShowIndicator = False
		gvGruppe0.OptionsView.ShowColumnHeaders = True
		gvGruppe0.OptionsView.ShowFooter = False

		gvGruppe0.OptionsView.ShowAutoFilterRow = True
		gvGruppe0.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvGruppe0.Columns.Clear()

		Dim columngav_number As New DevExpress.XtraGrid.Columns.GridColumn()
		columngav_number.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columngav_number.Name = "gav_number"
		columngav_number.FieldName = "gav_number"
		columngav_number.Visible = False
		columngav_number.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvGruppe0.Columns.Add(columngav_number)

		Dim columnname_de As New DevExpress.XtraGrid.Columns.GridColumn()
		columnname_de.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnname_de.Name = "name_de"
		columnname_de.FieldName = "name_de"
		columnname_de.Visible = True
		columnname_de.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		gvGruppe0.Columns.Add(columnname_de)

		lueGruppe0.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGruppe0.Properties.NullText = String.Empty
		lueGruppe0.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the business branches drop down.
	''' </summary>
	Private Sub ResetCategoryDropDown(ByVal i As Integer)

		Dim ctl As New DevExpress.XtraEditors.LookUpEdit
		Select Case i
			Case 0
				ctl = lue0
			Case 1
				ctl = lue1
			Case 2
				ctl = lue2
			Case 3
				ctl = lue3
			Case 4
				ctl = lue4
			Case 5
				ctl = lue5
			Case 6
				ctl = lue6
			Case 7
				ctl = lue7

		End Select

		ctl.Properties.DisplayMember = "Text_De"
		ctl.Properties.ValueMember = "ID_CategoryValue"

		Dim columns = ctl.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Text_De", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

		ctl.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		ctl.Properties.SearchMode = SearchMode.AutoComplete
		ctl.Properties.AutoSearchColumnIndex = 0

		ctl.Properties.NullText = String.Empty
		ctl.EditValue = Nothing

	End Sub

#Region "Form Eigenschaften..."
	Private Sub frmGAV_PVL_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

		pccExternalLinks.HidePopup()
		If Me.WindowState = FormWindowState.Minimized Then Exit Sub

		My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
		My.Settings.iHeight = Me.Height
		My.Settings.iWidth = Me.Width

		My.Settings.Save()

	End Sub

	Private ReadOnly Property IsNoPVL() As Boolean
		Get

			Dim mandantNumber As Integer = m_InitializationData.MDData.MDNr
			Dim companyallowednopvl As Boolean? = m_path.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																												 String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)

			Return If(companyallowednopvl Is Nothing, False, companyallowednopvl)

		End Get
	End Property

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblPath.Text = m_Translate.GetSafeTranslationValue(Me.lblPath.Text)

		Me.lblDatenQuelle.Text = String.Format(m_Translate.GetSafeTranslationValue("Datenquelle: {0}"), "www.tempdata.ch")
		Me.lblNoGarant.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue("Alle Angaben ohne Gewähr."))
		Me.bsiInfo.Caption = String.Format("Copyright © {0}", DateTime.Now.Year)

		Me.lblKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblKandidat.Text)
		Me.lblKunde.Text = m_Translate.GetSafeTranslationValue(Me.lblKunde.Text)

		Me.lblAchtung.Text = m_Translate.GetSafeTranslationValue(Me.lblAchtung.Text)

		Me.lblEinsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatz.Text)
		Me.lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGeburtsdatum.Text)
		Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)
		Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)

		Me.chkAlter.Text = m_Translate.GetSafeTranslationValue(Me.chkAlter.Text)
		Me.chkKanton.Text = m_Translate.GetSafeTranslationValue(Me.chkKanton.Text)

		Me.tabRechner.Text = m_Translate.GetSafeTranslationValue(Me.tabRechner.Text)
		Me.tabDetails.Text = m_Translate.GetSafeTranslationValue(Me.tabDetails.Text)

		Me.grpGAVBeruf.Text = m_Translate.GetSafeTranslationValue(Me.grpGAVBeruf.Text)
		Me.grpGAVOLDCategory.Text = m_Translate.GetSafeTranslationValue(Me.grpGAVOLDCategory.Text)
		Me.grpGAVCategory.Text = m_Translate.GetSafeTranslationValue(Me.grpGAVCategory.Text)
		Me.grpLODetails.Text = m_Translate.GetSafeTranslationValue(Me.grpLODetails.Text)

		Me.grpD_1.Text = m_Translate.GetSafeTranslationValue(Me.grpD_1.Text)

		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)

		lblgavBeruf.Text = m_Translate.GetSafeTranslationValue(lblgavBeruf.Text)
		lblKanton.Text = m_Translate.GetSafeTranslationValue(lblKanton.Text)
		lblKWStd.Text = m_Translate.GetSafeTranslationValue(lblKWStd.Text)


	End Sub

	Private Sub frmGAV_PVL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			If My.Settings.iHeight > 0 Then Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
			If My.Settings.iWidth > 0 Then Me.Width = Math.Max(My.Settings.iWidth, Me.Width)

			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub LoadEmployeeData()

		m_ExistingEmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, False)

		If m_ExistingEmployeeData Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Kandidatendaten konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		Me.lblQualifikationValue.Text = String.Format("({0}) {1}", m_ExistingEmployeeData.QLand, m_ExistingEmployeeData.Profession)
		Me.lblGebValue.Text = String.Format("({0:d}) {1:f0}", m_ExistingEmployeeData.Birthdate, m_ExistingEmployeeData.EmployeeSUVABirthdateAge)
		Me.lblMANR.Text = String.Format("({0}) {1}", EmployeeNumber, m_ExistingEmployeeData.EmployeeFullname)
		Me.lblBewValue.Text = String.Format("({0}) {1:d}", m_ExistingEmployeeData.Permission, m_ExistingEmployeeData.PermissionToDate)

		_MAAlter = String.Format("{0}", m_ExistingEmployeeData.EmployeeSUVABirthdateAge)

	End Sub

	Private Sub LoadPVLArchiveDbData()

		Dim pvlArchiveDbData = PerformPVLArchiveDbWebservice()

		If pvlArchiveDbData Is Nothing OrElse pvlArchiveDbData.Count = 0 Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Archiv-Datenbanken konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		luePVLArchiveDb.EditValue = Nothing
		luePVLArchiveDb.Properties.DataSource = pvlArchiveDbData
		luePVLArchiveDb.EditValue = pvlArchiveDbData(0).DbName

	End Sub

	Private Sub LoadCustomerData()

		Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(CustomerNumber, False)

		If customerMasterData Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		Me.lblKDNR.Text = String.Format("({0}) {1}", customerMasterData.CustomerNumber, customerMasterData.Company1)
		_KDPLZ = String.Format("{0}", customerMasterData.Postcode)

		Try
			lueCanton.EditValue = CustomerCanton

		Catch ex As Exception
			Dim strMsg As String = "Achtung: Möglicherweise wurde kein Kanton definiert."
			DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg),
																								 m_Translate.GetSafeTranslationValue("GAV-Auswahl"),
																								 MessageBoxButtons.OK,
																								 MessageBoxIcon.Warning)

		End Try

	End Sub

	Private Sub LoadEmploymentData()

		If EmploymentNumber = 0 Then Return
		Dim employmentMasterData = m_ESDatabaseAccess.LoadESMasterData(EmploymentNumber)

		If employmentMasterData Is Nothing Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden.")
			m_UtilityUI.ShowErrorDialog(msg)
			m_Logger.LogError(msg)

			Return
		End If

		Me.lblESNR.Text = String.Format("{0} | {1:d} {2:d}", employmentMasterData.ES_Als, employmentMasterData.ES_Ab, employmentMasterData.ES_Ende)

	End Sub

	Private Function LoadCantonDropDownData() As Boolean

		Dim data = m_CommonDatabaseAccess.LoadCantonData()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kanton Daten konnen nicht geladen werden."))
			Return False
		End If

		lueCanton.EditValue = Nothing
		lueCanton.Properties.DataSource = data

		Return True

	End Function

	Private Function LoadPVLDropDownData() As Boolean

		Dim data = PerformPVLlistWebserviceCallAsync()

		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("PVL-Daten konnen nicht geladen werden."))
			Return False
		End If
		m_PVLListData = data

		lueGruppe0.EditValue = Nothing
		lueGruppe0.Properties.DataSource = data

		Return True

	End Function

	Private Sub OnluePVLArchiveDb_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles luePVLArchiveDb.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Try
			lueGruppe0.EditValue = Nothing
			lueGruppe0.Properties.DataSource = Nothing

			m_PVLArchiveDbName = luePVLArchiveDb.EditValue

			If luePVLArchiveDb.EditValue Is Nothing Then Return
			LoadPVLDropDownData()

			LoadAllowedPVLData()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub OnlueCanton_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueCanton.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Try
			lueGruppe0.EditValue = Nothing
			lueGruppe0.Properties.DataSource = Nothing

			CustomerCanton = lueCanton.EditValue

			If lueCanton.EditValue Is Nothing Then Return
			LoadPVLDropDownData()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub OnlueGruppe0_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueGruppe0.EditValueChanged

		Try
			ResetAllLBL()
			If lueGruppe0.EditValue Is Nothing Then Return

			m_CurrentPVLData = SelectedPVLData

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			LoadSelectedPVLDetailData()
			m_SuppressUIEvents = suppressUIEventsState


		Catch ex As Exception

		End Try

	End Sub

	Private Function PerformPVLArchiveDbWebservice() As BindingList(Of PVLArchiveDBData)

		Dim listDataSource As BindingList(Of PVLArchiveDBData) = New BindingList(Of PVLArchiveDBData)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetPVLArchiveDbData(m_InitializationData.MDData.MDGuid).ToList

		Dim pvlGridData = (From Result In searchResult
											 Select New PVLArchiveDBData With
					 {
						.ID = Result.ID,
						.DbName = Result.DbName,
						.DbConnstring = Result.DbConnstring
					 }).ToList()

		For Each p In pvlGridData
			listDataSource.Add(p)
		Next

		Return listDataSource

	End Function

	Private Function PerformPVLlistWebserviceCallAsync() As BindingList(Of GAVNameResultDTO)

		Dim listDataSource As BindingList(Of GAVNameResultDTO) = New BindingList(Of GAVNameResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetCurrentPVLData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, lueCanton.EditValue, _KDPLZ, m_InitializationData.UserData.UserLanguage).ToList

		Dim pvlGridData = (From Result In searchResult
											 Select New GAVNameResultDTO With
					 {
						.ID_Calculator = Result.ID_Calculator,
						.Version = Result.Version,
						.State = Result.State,
						.validity_start_date = Result.validity_start_date,
						.unia_validity_end = Result.unia_validity_end,
						.unia_validity_start = Result.unia_validity_start,
						.ave_validity_start = Result.ave_validity_start,
						.Created = Result.Created,
						.PVL_Edition = Result.PVL_Edition,
						.schema_version = Result.schema_version,
						.ave = Result.ave,
						.id_meta = Result.id_meta,
						.gav_number = Result.gav_number,
						.name_de = Result.name_de,
						.name_fr = Result.name_fr,
						.name_it = Result.name_it,
						.publication_date = Result.publication_date,
						.stdweek = Result.stdweek,
						.stdmonth = Result.stdmonth,
						.stdyear = Result.stdyear,
						.fag = Result.fag,
						.fan = Result.fan,
						.old_fag = Result.old_fag,
						.old_fan = Result.old_fan,
						.resor_fan = Result.resor_fan,
						.resor_fag = Result.resor_fag,
						.van = Result.van,
						.vag = Result.vag,
						.wan = Result.wan,
						.wag = Result.wag,
						.old_van = Result.old_van,
						.old_vag = Result.old_vag,
						.old_VAN_s = Result.old_VAN_s,
						.old_VAG_s = Result.old_VAG_s,
						.old_VAN_J = Result.old_VAN_J,
						.old_VAG_J = Result.old_VAG_J,
						.old_wan = Result.old_wan,
						.old_wag = Result.old_wag,
						.old_WAN_s = Result.old_WAN_s,
						.old_WAG_s = Result.old_WAG_s,
						.old_WAN_J = Result.old_WAN_J,
						.old_WAG_J = Result.old_WAG_J,
						.GAVKanton = Result.GAVKanton,
						.currdbname = Result.currdbname
					 }).ToList()

		For Each p In pvlGridData

			listDataSource.Add(p)
		Next

		Return listDataSource

	End Function

	Private Function PerformPVLWarningDataWebservice() As GAVNotificationDTO

		Dim listDataSource = New GAVNotificationDTO

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetPVLWarningData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, m_CurrentPVLData.gav_number)

		Return searchResult

	End Function

	Private Function PerformPVLCategoryNamesWebservice() As BindingList(Of GAVCategoryDTO)

		Dim listDataSource As BindingList(Of GAVCategoryDTO) = New BindingList(Of GAVCategoryDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetGAVCategoryLabelData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, m_CurrentPVLData.id_meta, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New GAVCategoryDTO With {
						.ID_Category = result.ID_Category,
						.ID_Calculator = result.ID_Calculator,
						.ID_BaseCategory = result.ID_BaseCategory,
						.name_de = result.name_de,
						.name_fr = result.name_fr,
						.name_it = result.name_it
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	Private Function PerformPVLCategoryValueWebservice(ByVal categoryID As Integer, ByVal baseCategoryValueID As Integer?) As BindingList(Of GAVCategoryValueDTO)

		Dim listDataSource As BindingList(Of GAVCategoryValueDTO) = New BindingList(Of GAVCategoryValueDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetGAVCategoryValuesWithBaseValueData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, categoryID, baseCategoryValueID, m_InitializationData.UserData.UserLanguage).ToList

		'False)		' GetGAVCategoryValuesWithLanguage

		For Each result In searchResult

			Dim viewData = New GAVCategoryValueDTO With {
						.ID_Category = result.ID_Category,
						.ID_CategoryValue = result.ID_CategoryValue,
						.Text_De = result.Text_De
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	Private Function PerformPVLCalculationValueWebservice(ByVal catetoryValues As String) As GAVCalculationDTO

		Dim listDataSource As GAVCalculationDTO = New GAVCalculationDTO

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim Result = webservice.GetGAVCalculationValueData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, catetoryValues)

		Dim viewData = New GAVCalculationDTO With {
						.ID_Calculation = Result.ID_Calculation,
						.ID_Calculator = Result.ID_Calculator,
						.basic_hourly_wage = Result.basic_hourly_wage,
						.vacation_pay = Result.vacation_pay,
						.holiday_compensation = Result.holiday_compensation,
						.compensation_13th_month_salary = Result.compensation_13th_month_salary,
						.number_of_holidays = Result.number_of_holidays,
						.monthly_wage = Result.monthly_wage,
						.number_of_vacation_days = Result.number_of_vacation_days,
						.gross_hourly_wage = Result.gross_hourly_wage,
						.percentage_vacation_pay = Result.percentage_vacation_pay,
						.percentage_holiday_compensation = Result.percentage_holiday_compensation,
						.percentage_13th_month_salary = Result.percentage_13th_month_salary,
						.calculation_vacation_pay = Result.calculation_vacation_pay,
						.calculation_holiday_compensation = Result.calculation_holiday_compensation,
						.calculation_13th_month_salary = Result.calculation_13th_month_salary,
						.has_13th_month_salary_compensation = Result.has_13th_month_salary_compensation,
						.sortPosition = Result.sortPosition,
						.percentage_far_an = Result.percentage_far_an,
						.percentage_far_ag = Result.percentage_far_ag,
						.calculation_far = Result.calculation_far,
						.far_bvg_relevant = Result.far_bvg_relevant,
						.ID_AlternativeText = Result.ID_AlternativeText
			}


		Return viewData

	End Function

	Private Function PerformPVLCriteriasWebservice() As BindingList(Of GAVCriteriasResultDTO)

		Dim listDataSource As BindingList(Of GAVCriteriasResultDTO) = New BindingList(Of GAVCriteriasResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetPVLCriteriasData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, m_CurrentPVLData.id_meta, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New GAVCriteriasResultDTO With {
						.ID_Criterion = result.ID_Criterion,
						.ID_Contract = result.ID_Contract,
						.Element_ID = result.Element_ID,
						.name_de = result.name_de,
						.name_fr = result.name_fr,
						.name_it = result.name_it
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	Private Function PerformPVLCriteriasByIDWebservice(ByVal criteriaID As Integer) As BindingList(Of GAVCriteriaValueResultDTO)

		Dim listDataSource As BindingList(Of GAVCriteriaValueResultDTO) = New BindingList(Of GAVCriteriaValueResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetPVLCriteriaValuesByIDData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, criteriaID, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New GAVCriteriaValueResultDTO With {
						.ID_Criterion = result.ID_Criterion,
						.ID_CriterionValue = result.ID_CriterionValue,
						.txtText = result.txtText,
						.txtTable = result.txtTable
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		ClsDataDetail.strGAVData = String.Empty
		Me.Close()
	End Sub

	'Private Sub OnBtnTestXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestXML.Click
	'	Dim frmNewXML As New frmTempDataPVL(m_InitializationData)

	'	frmNewXML.EmployeeNumber = EmployeeNumber
	'	frmNewXML.CustomerNumber = CustomerNumber
	'	frmNewXML.CustomerCanton = CustomerCanton
	'	frmNewXML.EmploymentNumber = EmploymentNumber
	'	frmNewXML.Staging = True

	'	frmNewXML.LoadData()

	'	frmNewXML.Show()
	'	frmNewXML.BringToFront()

	'End Sub

	Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For i As Integer = 0 To GetExecutingAssembly.GetReferencedAssemblies.Count - 1
				strRAssembly &= String.Format("-->> {1}{0}", vbNewLine, GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	Public Overloads Function ShowDialog() As String
		Try
			'MsgBox(ClsDataDetail.strGAVData.ToArray, MsgBoxStyle.Critical, "ShowDialog")
		Catch ex As Exception

		End Try


		MyBase.ShowDialog()

		Return ClsDataDetail.strGAVData
	End Function


#End Region


#Region "Form drucken..."

	'' create a printing component
	'Private WithEvents pd As PrintDocument
	'Dim WithEvents mPrintDocument As New PrintDocument
	'Dim mPrintBitMap As Bitmap

	'Dim formImage As Bitmap
	'Private Declare Function BitBlt Lib "gdi32.dll" Alias "BitBlt" (
	'			ByVal hdcDest As IntPtr, ByVal nXDest As Integer, ByVal _
	'			nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight _
	'			As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As _
	'			Integer, ByVal nYSrc As Integer, ByVal dwRop As _
	'			System.Int32) As Long

	'Private Const SRCCOPY As Integer = &HCC0020
	'Dim memoryImage As Bitmap

	'' Callback from PrintDocument component to do the actual printing
	'Private Sub pd_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles pd.PrintPage
	'	e.Graphics.DrawImage(formImage, 0, 0)
	'End Sub

	'Private Sub GetFormImage()
	'	Dim g As Graphics = Me.CreateGraphics()
	'	Dim s As Size = Me.Size ' Me.XtraTabControl1.Size
	'	formImage = New Bitmap(s.Width, s.Height, g)
	'	Dim mg As Graphics = Graphics.FromImage(formImage)
	'	Dim dc1 As IntPtr = g.GetHdc
	'	Dim dc2 As IntPtr = mg.GetHdc
	'	' added code to compute and capture the form 
	'	' title bar and borders 
	'	Dim widthDiff As Integer = (Me.Width - Me.ClientRectangle.Width)
	'	Dim heightDiff As Integer = (Me.Height - Me.ClientRectangle.Height)
	'	Dim borderSize As Integer = widthDiff \ 2
	'	Dim heightTitleBar As Integer = heightDiff - borderSize

	'	' Mit Titlebar!!!
	'	' BitBlt(dc2, 0, 0, Me.ClientRectangle.Width + widthDiff, Me.ClientRectangle.Height + heightDiff, dc1, 0 - borderSize, 0 - heightTitleBar, 13369376)

	'	BitBlt(dc2, 0, 10,
	'		 Me.ClientRectangle.Width - widthDiff,
	'		 Me.ClientRectangle.Height - heightDiff, dc1,
	'		 -10 + borderSize, -10 + heightTitleBar, 13369376)

	'	g.ReleaseHdc(dc1)
	'	mg.ReleaseHdc(dc2)

	'End Sub

	Private Sub OnbbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

		Me.lblDatenQuelle.Text = String.Format(m_Translate.GetSafeTranslationValue("Datenquelle: www.tempdata.ch. | Gedruckt: {0} | {1}"), m_InitializationData.UserData.UserFullName, Now.ToString)
		Me.Refresh()


		Dim pf As New PrintForm(Me)
		' pf.PrintPreview()
		' - or-
		pf.Print(True, PrintForm.PrintMode_ENUM.FitToPage, "GAVForm")


		'GetFormImage()
		'pd.DefaultPageSettings.Landscape = True
		'pd.Print()

		Me.lblDatenQuelle.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue("Datenquelle: www.tempdata.ch"))
		Me.Refresh()

	End Sub


#End Region


#Region "Ende der FormPrint..."


#End Region


	'Private Sub Cbo_Gruppe0_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Gruppe0.QueryPopUp

	'	FillPVLBerufe_DS()

	'End Sub

	Sub FillPVLBerufe_DS()
		Dim bResor As Boolean = False

		'Cbo_Gruppe0.Properties.Items.Clear()
		'Cbo_Gruppe0.EditValue = Nothing
		txtStdWeek.EditValue = Format(0, "f2")

		ResetAllLBL()

		'Dim liKDBerufe As List(Of String) = GetKDGAVListe(Me.CustomerNumber)
		'LoadAssignedGAVGroupDataOfCustomer

		' Dataset-Variante...
		Dim ds As New DataSet
		'Dim dt As DataTable
		Dim strCustomer_ID As String = String.Empty

		ClearGAVBerufInfoFields()

		Dim Time_1 As Double = System.Environment.TickCount
		'ds = LoadPVLData(Me.Cbo_Kanton.Text, _KDPLZ)
		'If Not ds Is Nothing Then dt = ds.Tables("PVL_Online")
		'If ds Is Nothing OrElse dt Is Nothing Then
		'	Dim msg As String = m_Translate.GetSafeTranslationValue("Es ist ein Fehler in der Datenbank aufgetreten. Bitte kontaktieren Sie Ihrem Softwarehersteller.")
		'	m_UtilityUI.ShowErrorDialog(msg)

		'	Return
		'End If
		Time_1 = System.Environment.TickCount

		Dim pvlData = PerformPVLlistWebserviceCallAsync()

		'For i As Integer = 0 To dt.Rows.Count - 1
		For Each pvl In pvlData
			Dim bShowGAVBeruf As Boolean = False
			'Dim strGAVNr As String = ClsDataDetail.GetColumnTextStr(pvl, "gav_number", "0")
			Trace.WriteLine(pvl.gav_number)
			Dim strGavBeruf As String = pvl.name_de
			'ClsDataDetail.GetColumnTextStr(dt.Rows(i), String.Format("Name_{0}", "de"), "")

			'Dim strMetaNr As String = pvl.id_meta ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "id_meta", "0")
			'Dim strGAVISAVE As String = pvl.ave '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "ave", "0")
			m_CurrDbName = pvl.currdbname ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "CurrDbName", "?")

			Dim existingGAVData = m_ExistingCustomerGAVData.Where(Function(data) data.GAVNUmber.GetValueOrDefault(0) = pvl.gav_number).FirstOrDefault()
			If Not existingGAVData Is Nothing Then bShowGAVBeruf = True
			If EmploymentNumber > 0 Then
				Dim activeSalaryData = m_ExistingESLohnData.Where(Function(data) data.GAVNr.GetValueOrDefault(0) <> pvl.gav_number).FirstOrDefault()
				If Not activeSalaryData Is Nothing Then bShowGAVBeruf = False
			End If

			'For j As Integer = 0 To liKDBerufe.Count - 1
			'	If liKDBerufe(j) = pvl.gav_number Then
			'		If Me.GetOldGAVInfo.Count > 2 Then
			'			Dim iOLDGAVNr As Integer = GetOldGAVInfo(0).Split(":")(1)
			'			bShowGAVBeruf = iOLDGAVNr = CInt(strGAVNr)
			'		Else
			'			bShowGAVBeruf = True
			'		End If
			'		If EmploymentNumber > 0 Then
			'			Dim activeSalaryData = m_ExistingESLohnData.Where(Function(data) data.GAVNr.GetValueOrDefault(0) <> pvl.gav_number.FirstOrDefault()
			'			If Not activeSalaryData Is Nothing Then bShowGAVBeruf = False
			'		End If


			'		If bShowGAVBeruf Then Exit For
			'	End If
			'Next
			If bNoPVL Then
				If bShowGAVBeruf Then
					'bShowGAVBeruf = pvl.ave And pvl.gav_number<> 815001
					bShowGAVBeruf = pvl.gav_number <> 815001
				End If
				If bShowGAVBeruf Then strGavBeruf = strGavBeruf.Replace("Personalverleih", String.Empty).Trim()
			End If

			If bShowGAVBeruf Then
				Dim strGAVUniaAb As String = pvl.unia_validity_start ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "unia_validity_start", "")
				Dim strGAVUniaEnd As String = pvl.unia_validity_end ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "unia_validity_end", "") ' aGAVValues(5)
				Dim strGAVAveAb As String = pvl.ave_validity_start '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "ave_validity_start", "") '  aGAVValues(6)
				Dim strGAVAveEnd As String = pvl.ave_validity_end '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "ave_validity_end", "") '  aGAVValues(7)
				Dim strGAVPubOn As String = pvl.publication_date '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "publication_date", "") '  aGAVValues(8)
				Dim strGAVValidAb As String = pvl.validity_start_date '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "validity_start_date", "") '  aGAVValues(9)
				Try
					PublicationOfGAV = CType(strGAVPubOn, Date)
				Catch ex As Exception
					PublicationOfGAV = Nothing
				End Try


				Dim strGAVState As String = pvl.State '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "State", "") '  aGAVValues(10)

				Dim strGAVStdWeek As String = pvl.stdweek '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "StdWeek", "") '   aGAVValues(13)
				Dim strGAVStdMonth As String = pvl.stdmonth '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "StdMonth", "") '   aGAVValues(14)
				Dim strGAVStdYear As String = pvl.stdyear '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "StdYear", "") '   aGAVValues(15)

				Dim strGAVFAG As String = pvl.fag ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "FAG", "0")
				Dim strGAVFAN As String = pvl.fan ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "FAN", "0")
				Dim strGAVKanton4FAR As String = pvl.GAVKanton ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "GAVKanton", "")
				bResor = False
				If Not String.IsNullOrWhiteSpace(strGAVKanton4FAR) Then
					If strGAVKanton4FAR.ToUpper.Contains(String.Format("#{0}#", lueCanton.EditValue.ToUpper)) Then
						strGAVFAG = pvl.resor_fag '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "Resor_FAG", "0")
						strGAVFAN = pvl.resor_fan ' ClsDataDetail.GetColumnTextStr(dt.Rows(i), "Resor_FAN", "0")
						bResor = Val(strGAVFAG) + Val(strGAVFAG) > 0 ' True
					End If
				End If

				Dim strGAVWAG As String = pvl.wag '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "WAG", "0")
				Dim strGAVWAN As String = pvl.wan '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "WAN", "0")
				Dim strGAVVAG As String = pvl.vag '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "VAG", "0")
				Dim strGAVVAN As String = pvl.van '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "VAN", "0")

				Dim strGAVFAG_ As String = pvl.old_fag '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_FAG", "0")
				Dim strGAVFAN_ As String = pvl.old_fan '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_FAN", "0")

				Dim strGAVWAG_ As String = pvl.old_wag '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAG", "0")
				Dim strGAVWAN_ As String = pvl.old_wan '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAN", "0")
				Dim strGAVWAG_S As String = pvl.old_WAG_s '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAG_s", "0")
				Dim strGAVWAN_S As String = pvl.old_WAN_s '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAN_s", "0")
				Dim strGAVWAG_J As String = pvl.old_WAG_J '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAG_J", "0")
				Dim strGAVWAN_J As String = pvl.old_WAN_J '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAN_J", "0")
				Dim strGAVVAG_ As String = pvl.old_vag '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAG", "0")
				Dim strGAVVAN_ As String = pvl.old_van '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAN", "0")
				Dim strGAVVAG_S As String = pvl.old_VAG_s '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAG_s", "0")
				Dim strGAVVAN_S As String = pvl.old_VAN_s '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAN_s", "0")
				Dim strGAVVAG_J As String = pvl.old_VAG_J '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAG_J", "0")
				Dim strGAVVAN_J As String = pvl.old_VAN_J '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAN_J", "0")

				Dim strPVLEdition As String = pvl.PVL_Edition '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "PVL_Edition", "0")
				Dim strPVLCreated As String = pvl.Created '  ClsDataDetail.GetColumnTextStr(dt.Rows(i), "Created", "")
				Me.grpGAVBeruf.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue("GAV-Berufe"))
				Trace.WriteLine(String.Format("GAVNr: {0} | GAVBeruf: {1} | Resor: {2}", pvl.gav_number, strGavBeruf, bResor.ToString))

				'With Cbo_Gruppe0
				'	.Properties.Items.Add(New ComboBoxItem(If(strGavBeruf = String.Empty, pvl.id_meta, strGavBeruf),
				'																	pvl.id_meta,
				'																	pvl.gav_number,
				'																	pvl.ave,
				'																	strGAVUniaAb,
				'																	strGAVUniaEnd,
				'																	strGAVAveAb,
				'																	strGAVAveEnd,
				'																	strGAVPubOn,
				'																	strGAVValidAb,
				'																	strGAVState,
				'																	strGAVStdWeek,
				'																	strGAVStdMonth,
				'																	strGAVStdYear,
				'																	strGAVFAG,
				'																	strGAVFAN,
				'																	strGAVWAG,
				'																	strGAVWAN,
				'																	strGAVVAG,
				'																	strGAVVAN,
				'																	strGAVWAG_,
				'																	strGAVWAN_,
				'																	strGAVWAG_S,
				'																	strGAVWAN_S,
				'																	strGAVWAG_J,
				'																	strGAVWAN_J,
				'																	strGAVVAG_,
				'																	strGAVVAN_,
				'																	strGAVVAG_S,
				'																	strGAVVAN_S,
				'																	strGAVVAG_J,
				'																	strGAVVAN_J,
				'																	strGAVFAG_,
				'																	strGAVFAN_,
				'																	bResor.ToString,
				'																	PublicationOfGAV))

				'End With

			End If

		Next
		'Me.Cbo_Gruppe0.Properties.DropDownRows = 20

	End Sub




	Private Sub LoadSelectedPVLDetailData()
		Dim strMetaNr = m_CurrentPVLData.id_meta
		Dim strGAVNr = m_CurrentPVLData.gav_number
		Dim fagAmount As Decimal = 0
		Dim fanAmount As Decimal = 0
		Dim isResor As Boolean = False
		m_CurrDbName = m_CurrentPVLData.currdbname

		If Not String.IsNullOrWhiteSpace(m_CurrentPVLData.GAVKanton) AndAlso m_CurrentPVLData.GAVKanton.ToUpper.Contains(String.Format("#{0}#", lueCanton.EditValue.ToUpper)) Then
			fagAmount = m_CurrentPVLData.resor_fag
			fanAmount = m_CurrentPVLData.resor_fan
			isResor = True
		End If


		chkResor.Visible = False

		Me.lblGAVNr.Text = m_CurrentPVLData.gav_number
		Me.lblMetaNr.Text = m_CurrentPVLData.id_meta
		If m_CurrentPVLData.id_meta = 0 Then
			m_Logger.LogWarning(String.Format("meta number was {0}.", m_CurrentPVLData.id_meta))
			Return
		End If
		Dim maxWorkingHourinWeek As Decimal = Val(m_CurrentPVLData.stdweek)
		Dim newMaxHourinWeek = DetermineMaximalWorkingHoursPerWorkingDay()
		If newMaxHourinWeek = 0 Then newMaxHourinWeek = maxWorkingHourinWeek


		If maxWorkingHourinWeek <> newMaxHourinWeek Then
			Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Achtung: Die Wochenstunden werden individuell aus Mandantenverwaltung genommen!{0}Die neuen Wochenstunden betragen {1:f2} Stunden/Woche."), vbNewLine, newMaxHourinWeek)
			m_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("Maximalen Wochenstunden"))
		End If
		txtStdWeek.EditValue = Format(maxWorkingHourinWeek, "f2")
		txtStdWeek.EditValue = Format(newMaxHourinWeek, "f2")
		Me.lblWStd.Text = m_CurrentPVLData.stdweek
		Me.lblMStd.Text = m_CurrentPVLData.stdmonth
		Me.lblJStd.Text = m_CurrentPVLData.stdyear

		If Not bNoPVL Then
			' ist PVL
			Me.lblFAG.Text = String.Format("{0}", fagAmount)
			Me.lblFAN.Text = String.Format("{0}", fanAmount)
			Me.lblPAG.Text = String.Format("{0}", m_CurrentPVLData.vag)
			Me.lblPAN.Text = String.Format("{0}", m_CurrentPVLData.van)

		Else
			' Ist Inkassopool!
			Me.lblFAG.Text = String.Format("{0}", m_CurrentPVLData.old_fag)
			Me.lblFAN.Text = String.Format("{0}", m_CurrentPVLData.old_fan)
			Me.lblPAG.Text = String.Format("{0}", m_CurrentPVLData.old_wag + m_CurrentPVLData.old_vag)
			Me.lblPAN.Text = String.Format("{0}", m_CurrentPVLData.old_wan + m_CurrentPVLData.old_van)

		End If

		_bResorpflichtig = isResor
		chkResor.Visible = isResor
		chkResor.Enabled = m_AllowedChangeFAR
		chkFAR.Enabled = m_AllowedChangeFAR

		Me._cWAGAnhang1 = m_CurrentPVLData.old_wag
		Me._cWANAnhang1 = m_CurrentPVLData.old_wan
		Me._cWAGAnhang1_S = m_CurrentPVLData.old_WAG_s
		Me._cWANAnhang1_S = m_CurrentPVLData.old_WAN_s
		Me._cWAGAnhang1_J = m_CurrentPVLData.old_WAG_J
		Me._cWANAnhang1_J = m_CurrentPVLData.old_WAN_J

		Me._cVAGAnhang1 = m_CurrentPVLData.old_vag
		Me._cVANAnhang1 = m_CurrentPVLData.old_van
		Me._cVAGAnhang1_S = m_CurrentPVLData.old_VAG_s
		Me._cVANAnhang1_S = m_CurrentPVLData.old_VAN_s
		Me._cVAGAnhang1_J = m_CurrentPVLData.old_VAG_J
		Me._cVANAnhang1_J = m_CurrentPVLData.old_VAN_J

		Me.m_cFAGAnhang1 = m_CurrentPVLData.old_fag
		Me.m_cFANAnhang1 = m_CurrentPVLData.old_fan
		chkFAR.Checked = Val(lblFAG.Text) + Val(lblFAN.Text) > 0

		PublicationOfGAV = m_CurrentPVLData.publication_date
		Me.grpGAVBeruf.Text = String.Format("{0}", m_Translate.GetSafeTranslationValue("GAV-Berufe"))
		ResetCategoryControls()

		' GAV_Infos aufbauen...
		CreateGAVBerufHeaderInfoLblControl()    ' Header
		GetGAV_Details()  ' Details
		Me.btnCopyGAVD_Info.Visible = False

		'GetCategoriesLabel()
		SetCategoriesLabels()
		grpGAVCategory.Height = grpLODetails.Height

		Dim warningData = PerformPVLWarningDataWebservice()
		If Not warningData Is Nothing AndAlso (warningData.gav_number > 0 OrElse Not String.IsNullOrWhiteSpace(warningData.Info)) Then
			Me.lblSputnikMessage.AllowHtmlString = True
			Me.lblSputnikMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None

			Me.lblSputnikMessage.Text = warningData.Info
			m_UtilityUI.ShowInfoDialog(warningData.Info, m_Translate.GetSafeTranslationValue("Achtung"), MessageBoxIcon.Exclamation)
			'ShowMessage()
		End If
		'Me.lblAchtung.Visible = Not String.IsNullOrWhiteSpace(warningData.Info)

	End Sub

	Private Function DetermineMaximalWorkingHoursPerWorkingDay() As Decimal

		Dim value As Decimal? ' = Decimal.MaxValue ' No flexible time
		If GetflexibletimeFromDatabase Then
			value = m_ReportDataAccess.LoadManantTSPLMVWorkingHoursPerWeek(m_CurrentPVLData.gav_number, Now.Year)
		Else
			value = 0
		End If
		'Dim flexibleTimeHelper As New FlexibleTimeHelper(m_InitializationData.MDData.MDNr, m_ReportDataAccess)
		'value = flexibleTimeHelper.DetermineMaximalWorkingHoursPerWorkingDay(m_CurrentPVLData.gav_number, m_CurrentPVLData.stdweek, Now.Year)


		Return value.GetValueOrDefault(0)

	End Function


	'Private Sub test()
	'	Dim cv As ComboBoxItem = DirectCast(Cbo_Gruppe0.SelectedItem, ComboBoxItem)
	'	Dim strMetaNr As String = cv.Value_0
	'	Dim strGAVNr As String = cv.Value_1
	'	Dim strGAVGeltInfo As String = cv.Value_3

	'	chkResor.Visible = False

	'	Me.lblGAVNr.Text = strGAVNr
	'	Me.lblMetaNr.Text = strMetaNr
	'	If Val(Me.lblMetaNr.Text) = 0 Then Exit Sub

	'	txtStdWeek.EditValue = Format(Val(cv.Value_10), "f2")
	'	Me.lblWStd.Text = cv.Value_10
	'	Me.lblMStd.Text = cv.Value_11
	'	Me.lblJStd.Text = cv.Value_12

	'	If Not bNoPVL Then
	'		' ist PVL
	'		Me.lblFAG.Text = String.Format("{0}", Val(cv.Value_13))
	'		Me.lblFAN.Text = String.Format("{0}", Val(cv.Value_14))
	'		Me.lblPAG.Text = String.Format("{0}", Val(cv.Value_17))
	'		Me.lblPAN.Text = String.Format("{0}", Val(cv.Value_18))

	'	Else
	'		' Ist Inkassopool!
	'		Me.lblFAG.Text = String.Format("{0}", Val(cv.Value_31))
	'		Me.lblFAN.Text = String.Format("{0}", Val(cv.Value_32))
	'		Me.lblPAG.Text = String.Format("{0}", Val(cv.Value_19) + Val(cv.Value_25))
	'		Me.lblPAN.Text = String.Format("{0}", Val(cv.Value_20) + Val(cv.Value_26))

	'	End If

	'	Me._bResorpflichtig = CBool(cv.Value_33)
	'	chkResor.Visible = _bResorpflichtig
	'	chkResor.Enabled = m_AllowedChangeFAR
	'	chkFAR.Enabled = m_AllowedChangeFAR


	'	Me._cWAGAnhang1 = cv.Value_19
	'	Me._cWANAnhang1 = cv.Value_20
	'	Me._cWAGAnhang1_S = cv.Value_21
	'	Me._cWANAnhang1_S = cv.Value_22
	'	Me._cWAGAnhang1_J = cv.Value_23
	'	Me._cWANAnhang1_J = cv.Value_24

	'	Me._cVAGAnhang1 = cv.Value_25
	'	Me._cVANAnhang1 = cv.Value_26
	'	Me._cVAGAnhang1_S = cv.Value_27
	'	Me._cVANAnhang1_S = cv.Value_28
	'	Me._cVAGAnhang1_J = cv.Value_29
	'	Me._cVANAnhang1_J = cv.Value_30

	'	Me.m_cFAGAnhang1 = cv.Value_31
	'	Me.m_cFANAnhang1 = cv.Value_32
	'	chkFAR.Checked = Val(lblFAG.Text) + Val(lblFAN.Text) > 0

	'	PublicationOfGAV = cv.Value_34

	'	' GAV_Infos aufbauen...
	'	CreateGAVBerufHeaderInfoLblControl()    ' Header
	'	GetGAV_Details()  ' Details
	'	Me.btnCopyGAVD_Info.Visible = False

	'	GetData4Categories(CInt(Val(Me.lblMetaNr.Text)))

	'	strSputnikMessage = GetPVLWarning(CInt(strGAVNr))
	'	If strSputnikMessage.Trim <> String.Empty Then
	'		Me.lblSputnikMessage.AllowHtmlString = True
	'		Me.lblSputnikMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None

	'		Me.lblSputnikMessage.Text = strSputnikMessage
	'		lblShowMessage_Click(sender, e)
	'	End If
	'	Me.lblAchtung.Visible = strSputnikMessage.Trim <> String.Empty

	'End Sub

	Sub CreateGAVBerufHeaderInfoLblControl()

		Dim aveHeader As String = m_Translate.GetSafeTranslationValue("Allgemeinverbindlicherklärung: {0:d} - {1:d}")
		aveHeader = String.Format(aveHeader, m_CurrentPVLData.ave_validity_start, m_CurrentPVLData.ave_validity_end)
		lblAVEHeader.Text = aveHeader
		lblAVEHeader.Visible = True

		Dim pubHeader = m_Translate.GetSafeTranslationValue("Publikationsdatum: {0:d} - Publikation gültig ab: {1:d}")
		pubHeader = String.Format(pubHeader, m_CurrentPVLData.publication_date, m_CurrentPVLData.validity_start_date)
		lblPublicHeader.Text = pubHeader
		lblPublicHeader.Visible = True

		grpGAVCategory.Text = String.Format(m_Translate.GetSafeTranslationValue("GAV-Kategorien: {0}"), m_CurrentPVLData.gav_number)
		lblAVE.Text = m_Translate.GetSafeTranslationValue(String.Format("{0} AVE", If(m_CurrentPVLData.ave, "Ist", "Kein")))

	End Sub

	Sub GetCategoriesLabel()
		Dim iTop As Integer = 40 ' Me.lbl_Gruppe0.Top + Me.lbl_Gruppe0.Height + 40
		Dim iLeft As Integer = 30 ' Me.lbl_Gruppe0.Left
		Dim iTopCbo As Integer = 50 ' Me.Cbo_Gruppe0.Top + Me.Cbo_Gruppe0.Height + 50
		Dim iLeftCbo As Integer = 150 ' Me.Cbo_Gruppe0.Left + 50
		Dim iOldBaseCategoryNr As Integer = 0

		ResetAllLBL()
		Dim data = PerformPVLCategoryNamesWebservice()
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kategorie Daten konnten geladen werden."))
			Return
		End If
		'liGAVCategories = PerformPVLCategoryNamesWebservice()

		'If liGAVCategories.Count > 0 Then
		For Each cat In data
			'Dim aGAVValues As String() = liGAVCategories(i).Split(CChar("¦"))
			Dim strCategoryName As String = cat.name_de ' aGAVValues(aGAVValues.Length - 1)
			Dim strIDCategory As String = cat.ID_Category '  aGAVValues(0)
			Dim strIDCalculator As String = cat.ID_Calculator '  aGAVValues(1)
			Dim strIDBaseCategory As String = cat.ID_BaseCategory '  aGAVValues(2)
			Dim ctl As New DevExpress.XtraEditors.LabelControl ' Label
			Me.grpGAVOLDCategory.Controls.Add(ctl)

			If Val(strIDBaseCategory) = 0 Then
				ctl.Location = New Point(iLeft, iTop)
				ctl.AutoSize = True
				ctl.Text = strCategoryName
				ctl.Tag = New TextBoxItem(strCategoryName, strIDCategory, strIDCalculator, strIDBaseCategory)
				ctl.Name = "lblCategory_" & strIDCategory & "_" & strIDBaseCategory
				ctl.ForeColor = Color.Black
				ctl.BackColor = Color.Transparent
				ctl.Show()
				CreateCategoryCboControl(cat, iTop - 5, iLeftCbo)

				iOldBaseCategoryNr = 0

			Else
				ctl.AutoSize = True
				ctl.Text = ":.. " & strCategoryName
				ctl.Tag = New TextBoxItem(strCategoryName, strIDCategory, strIDCalculator, strIDBaseCategory)
				ctl.Name = "lblCategory_" & strIDCategory & "_" & strIDBaseCategory
				ctl.ForeColor = Color.Red
				ctl.BackColor = Color.Transparent

				If iOldBaseCategoryNr = 0 Then
					ctl.Location = New Point(iLeft + 20, iTop)
					CreateCategoryCboControl(cat, iTop - 5, iLeftCbo + 50)

					iOldBaseCategoryNr = Val(strIDBaseCategory)

				Else
					If iOldBaseCategoryNr = Val(strIDBaseCategory) Then
						ctl.Location = New Point(iLeft + 20, iTop)
						CreateCategoryCboControl(cat, iTop - 5, iLeftCbo + 50)
					Else
						ctl.Location = New Point(iLeft + 40, iTop)
						CreateCategoryCboControl(cat, iTop - 5, iLeftCbo + 50)
					End If

				End If
				ctl.Show()

			End If
			iTop += 30
			iTopCbo += 40
			aLblControls.Add(ctl.Name)

		Next
		'End If
		Me.grpGAVOLDCategory.Height = iTop
		Me.grpGAVOLDCategory.Visible = True
		'Me.grpLODetails.Top = Me.grpGAVCategory.Top + Me.grpGAVCategory.Height + 10

	End Sub

	Sub CreateCategoryCboControl(ByVal categoryData As GAVCategoryDTO, ByVal iTop As Integer, ByVal iLeft As Integer)
		Dim strCategoryName As String = categoryData.name_de ' liData(liData.Count - 1)
		Dim strIDCategory As String = categoryData.ID_Category ' liData(0)
		Dim strIDCalculator As String = categoryData.ID_Calculator ' liData(1)
		Dim strIDBaseCategory As String = categoryData.ID_BaseCategory ' liData(2)

		Dim ctl As New DevExpress.XtraEditors.ComboBoxEdit
		ctl.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		ctl.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		ctl.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

		Me.grpGAVOLDCategory.Controls.Add(ctl)

		ctl.Location = New Point(iLeft, iTop)
		ctl.Width = grpGAVOLDCategory.Width - ctl.Left - 23
		'If strCategoryName.ToLower.Contains("kanton") Or strCategoryName.ToLower.Contains("alter") Then
		'  ctl.Width = 400 - ctl.Left - 23
		'End If

		ctl.Name = String.Format("CboCategory_{0}_{1}_{2}_", strIDCategory, strIDCalculator, strIDBaseCategory)
		If strCategoryName = m_Translate.GetSafeTranslationValue("Alter") OrElse strCategoryName = "Età" Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		ElseIf strCategoryName = m_Translate.GetSafeTranslationValue("Kanton") Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		ElseIf strCategoryName = m_Translate.GetSafeTranslationValue("Jahr") Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		Else
			ctl.Tag = String.Empty

		End If
		'ctl.Tag = If(strCategoryName.ToLower.Contains("alter"), strCategoryName, If(strCategoryName.ToLower.Contains("kanton"), strCategoryName, ""))
		ctl.ForeColor = Color.Black
		ctl.Show()
		' TODO: 
		MyComboBoxExtensions.ToItem(ctl)

		AddHandler ctl.QueryPopUp, AddressOf ctlCbo_DropDown
		AddHandler ctl.SelectedIndexChanged, AddressOf ctlCbo_SelectedValueChanged
		aCboControls.Add(ctl.Name)

	End Sub

	Function GetCboControl(ByVal strName2Search As String) As Object
		Dim strResult As String = String.Empty

		For i As Integer = 0 To aCboControls.Count - 1
			If aCboControls(i).ToString.ToLower.Contains(strName2Search.ToLower) Then
				Return GetControlbyName(aCboControls(i).ToString)
			End If
		Next

		Return strResult
	End Function

	Private Sub ctlCbo_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim ctlCbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
		ctlCbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		ctlCbo.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		ctlCbo.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

		Dim aCtlInfo As String() = ctlCbo.Name.Split(CChar("_"))
		Dim i As Integer = 0
		Dim iBaseSelectedCategoryValue As Integer = 0
		Dim bNotSearchmore As Boolean = False

		Me.lblCategoryNr.Text = aCtlInfo(1)
		Me.lblCalculatorNr.Text = aCtlInfo(2)
		Me.lblBaseCategoryNr.Text = aCtlInfo(3)
		ctlCbo.Properties.Items.Clear()

		Dim Time_1 As Double = System.Environment.TickCount
		If Val(Me.lblBaseCategoryNr.Text) > 0 Then
			If Val(Me.lblCategoryValueNr.Text) = 0 Then
				Return

			Else
				For i = 0 To aCboControls.Count - 1
					If aCboControls(i).ToString.ToLower = sender.name.ToString.ToLower Then
						Dim ctlBaseCbo As New DevExpress.XtraEditors.ComboBoxEdit
						ctlBaseCbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
						ctlBaseCbo.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
						ctlBaseCbo.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

						Dim strBaseCtlName As String = String.Format("CboCategory_{0}_{1}",
																												Me.lblBaseCategoryNr.Text,
																												Me.lblCalculatorNr.Text)
						' BaseCbo ermitteln...
						ctlBaseCbo = GetCboControl(strBaseCtlName)
						If Not IsNothing(ctlBaseCbo) Then
							For j As Integer = 0 To aCboControls.Count - 1
								' muss der CategoryValue von BaseCbo ermittelt werden...
								If aCboControls(j).ToString.ToLower = ctlBaseCbo.Name.ToLower Then

									If Val(aCategoryValuesNr(j)) = 0 Then
										Return
									Else
										iBaseSelectedCategoryValue = Val(aCategoryValuesNr(j))
										bNotSearchmore = True
										Exit For
									End If

								End If
							Next
							If bNotSearchmore Then Exit For

						End If
					End If
				Next

			End If
		End If

		Dim liGAVCategoryValues As New BindingList(Of GAVCategoryValueDTO)
		If Val(Me.lblBaseCategoryNr.Text) > 0 Then
			liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(Me.lblCategoryNr.Text), iBaseSelectedCategoryValue) ' , True)		' GetGAVCategoryValuesWithBaseValue
		Else
			liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(Me.lblCategoryNr.Text), Nothing) ' 0, False)		' GetGAVCategoryValuesWithLanguage
		End If
		If liGAVCategoryValues Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Weitere Kategorie Daten konnten nicht geladen werden.")

			Return
		End If
		Time_1 = System.Environment.TickCount
		Dim iLengthofString As Integer = 0

		'If liGAVCategoryValues.Count > 0 Then
		ctlCbo.Properties.Items.Add(New ComboBoxItem(String.Empty, 0, 0))
		For Each cat In liGAVCategoryValues
			'Dim aGAVValues As String() = liGAVCategoryValues(i).Split(CChar("¦"))
			Dim strValueName = cat.Text_De ' aGAVValues(aGAVValues.Length - 1)
			Dim strCategoryValueNr As String = cat.ID_CategoryValue ' aGAVValues(0)
			Dim strCategoryNr As String = cat.ID_Category ' aGAVValues(1)
			If (ctlCbo.Tag = m_Translate.GetSafeTranslationValue("Alter") OrElse ctlCbo.Tag = "Età") And chkAlter.CheckState = CheckState.Checked Then
				If Val(strValueName) = Val(_MAAlter) Then
					ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName, strCategoryValueNr, strCategoryNr))
				ElseIf Val(_MAAlter) >= 65 AndAlso Val(strValueName) >= 65 Then
					ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName, strCategoryValueNr, strCategoryNr))
				End If

			ElseIf ctlCbo.Tag = m_Translate.GetSafeTranslationValue("Kanton") And chkKanton.CheckState = CheckState.Checked Then
				If strValueName.ToLower = CustomerCanton.ToLower Or
											strValueName.ToLower.Contains(String.Format("{0}:", CustomerCanton.ToLower)) Then
					ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName, strCategoryValueNr, strCategoryNr))
				End If

			Else
				ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName.Replace("|", " => "), strCategoryValueNr, strCategoryNr))
			End If
			iLengthofString = If(strValueName.Length > iLengthofString, strValueName.Length, iLengthofString)
		Next
		'End If

		Trace.WriteLine(String.Format("Zeitmessung für Füllen der Combobox: {0} s.",
										((System.Environment.TickCount - Time_1) / 1000)))

	End Sub

	Private Sub ctlCbo_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

		'Trace.WriteLine(sender.Tag.value_0)
		Dim ctlCbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
		ctlCbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		ctlCbo.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		ctlCbo.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

		Try
			Dim bNotSearchmore As Boolean = False
			Try
				For i As Integer = 0 To aCboControls.Count - 1
					If aCboControls(i).ToString.ToLower = ctlCbo.Name.ToLower Then
						For j As Integer = i + 1 To aCboControls.Count - 1
							Dim Cboctl2Delete As New DevExpress.XtraEditors.ComboBoxEdit
							Cboctl2Delete.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
							Cboctl2Delete.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
							Cboctl2Delete.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

							Cboctl2Delete = GetControlbyName(aCboControls(j).ToString)
							If Not IsNothing(Cboctl2Delete) Then
								Cboctl2Delete.Text = String.Empty
							End If

						Next
						Exit Try
					End If
				Next


			Catch ex As Exception

			End Try
			aCategoryValuesNr.Clear()


		Catch ex As Exception

		End Try

		Dim cv As ComboBoxItem = DirectCast(ctlCbo.SelectedItem, ComboBoxItem)

		Dim strCategoryValueNr As String = cv.Value_0
		Dim strCategoryNr As String = cv.Value_1

		Me.lblCategoryNr.Text = strCategoryNr
		Me.lblCategoryValueNr.Text = strCategoryValueNr
		Dim bShowLODetails As Boolean = True

		Try
			For i As Integer = 0 To aCboControls.Count - 1
				ctlCbo = GetControlbyName(aCboControls(i).ToString)

				If Not IsNothing(ctlCbo) Then
					If ctlCbo.Text <> String.Empty Then
						Dim cb As ComboBoxItem = DirectCast(ctlCbo.SelectedItem, ComboBoxItem)
						aCategoryValuesNr.Add(cb.Value_0)
					Else
						aCategoryValuesNr.Add("0")
					End If
				End If
			Next

		Catch ex As Exception
			bShowLODetails = False

		End Try
		Dim _clsConvert As New ClsConvert
		Me.lblAllCategoryValueNr.Text = _clsConvert.ConvListObject2String(aCategoryValuesNr)
		If bShowLODetails Then
			bIsQ12Enabled = False ' (m_CurrentPVLData.gav_number = 100001 And Me.lblAllCategoryValueNr.Text.Replace(" ", "").Split(",").Contains("53"))
			If Not bIsQ12Enabled Then liBauQ12LOData.Clear()
			ShowLODataWithCategoryValues(Me.lblAllCategoryValueNr.Text)

			'' beim GAVNumber = 100001 und Category-Value = 53 
			'' soll der Lohn möglicherweise um 12-10% unterschritten werden können
			'If bIsQ12Enabled Then
			'	bIsQ12Enabled = True
			'	Try
			'		Dim chkLOBauQ1_0 As New RadioButton
			'		chkLOBauQ1_0.Text = m_Translate.GetSafeTranslationValue("Keine Reduktion")
			'		chkLOBauQ1_0.Location = New Point(310, 80)
			'		chkLOBauQ1_0.Name = "chkBauQ0"
			'		chkLOBauQ1_0.AutoSize = True
			'		Me.scLODetails.Controls.Add(chkLOBauQ1_0)
			'		aLblLODataControls.Add(chkLOBauQ1_0)

			'		chkLOBauQ1_0.ForeColor = Color.Red
			'		chkLOBauQ1_0.Show()
			'		AddHandler chkLOBauQ1_0.CheckedChanged, AddressOf chkLOBauQ1_2_CheckState

			'		Dim chkLOBauQ1_1 As New RadioButton
			'		chkLOBauQ1_1.Text = m_Translate.GetSafeTranslationValue("Q1-Bau-Facharbeiter im ersten Jahr: 12 %")
			'		chkLOBauQ1_1.Location = New Point(310, 100)
			'		chkLOBauQ1_1.Name = "chkBauQ1"
			'		chkLOBauQ1_1.AutoSize = True
			'		Me.scLODetails.Controls.Add(chkLOBauQ1_1)
			'		aLblLODataControls.Add(chkLOBauQ1_1)

			'		chkLOBauQ1_1.ForeColor = Color.Red
			'		chkLOBauQ1_1.Show()
			'		AddHandler chkLOBauQ1_1.CheckedChanged, AddressOf chkLOBauQ1_2_CheckState

			'		Dim chkLOBauQ1_2 As New RadioButton
			'		chkLOBauQ1_2.Text = m_Translate.GetSafeTranslationValue("Q2-Bau-Facharbeiter im zweiten Jahr: 10 %")
			'		chkLOBauQ1_2.Location = New Point(310, 120)
			'		chkLOBauQ1_2.Name = "chkBauQ2"
			'		chkLOBauQ1_2.AutoSize = True
			'		Me.scLODetails.Controls.Add(chkLOBauQ1_2)
			'		aLblLODataControls.Add(chkLOBauQ1_2)

			'		chkLOBauQ1_2.ForeColor = Color.Red
			'		chkLOBauQ1_2.Show()

			'		AddHandler chkLOBauQ1_2.Click, AddressOf chkLOBauQ1_2_CheckState

			'	Catch ex As Exception

			'	End Try
			'End If

		End If

	End Sub

	Sub chkLOBauQ1_2_CheckState(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim ctlRadio As RadioButton = DirectCast(sender, RadioButton)
		Dim dRabatt As Double = 0

		liBauQ12LOData.Clear()
		If ctlRadio.Name.Contains("Q1") Then
			dRabatt = 12
		ElseIf ctlRadio.Name.Contains("Q2") Then
			dRabatt = 10
		End If

		For i As Integer = 2 To Me.liLOData.Count - 1
			Select Case i
				Case 2, 3, 4, 5, 9
					Dim strbez As String = liLOData(i).Split(CChar("¦"))(0)
					Dim dBetrag As Double = Val(liLOData(i).Split(CChar("¦"))(1))

					liBauQ12LOData.Add(String.Format("{0}¦{1}", strbez, Format(dBetrag - (dBetrag * (dRabatt / 100)), "n")))
					Trace.WriteLine(String.Format("Betrag: {0} | Rabatt: {1} | Abzug: {2} | Endbetrag: {3}",
																				dBetrag, dRabatt, (dBetrag * (dRabatt / 100)), dBetrag - (dBetrag * (dRabatt / 100))))
			End Select
		Next
		Me.scLoBauQ12.Controls.Clear()
		If ctlRadio.Name.Contains("Q0") Then liBauQ12LOData.Clear()
		Me.scLoBauQ12.Visible = liBauQ12LOData.Count > 2
		If liBauQ12LOData.Count <= 2 Then Exit Sub

		Dim iTop As Integer = 130
		Dim iLeft As Integer = 0
		For i As Integer = 0 To liBauQ12LOData.Count - 1
			Dim lblLOBauQ1_0 As New DevExpress.XtraEditors.LabelControl ' Label
			Dim strbez As String = liBauQ12LOData(i).Split(CChar("¦"))(0)
			Dim dBetrag As Double = Val(liBauQ12LOData(i).Split(CChar("¦"))(1))

			lblLOBauQ1_0.Location = New Point(iLeft, iTop)

			lblLOBauQ1_0.Anchor = AnchorStyles.Top
			lblLOBauQ1_0.Size = New Size(35, 15) '.AutoSize = True
			'lblLOBauQ1_0.AutoSize = True

			lblLOBauQ1_0.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			'lblLOBauQ1_0.Font = New Font(lblLOBauQ1_0.Font, FontStyle.Bold)

			lblLOBauQ1_0.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			lblLOBauQ1_0.Name = String.Format("lblBauQ{0}", i)

			lblLOBauQ1_0.Text = Format(dBetrag, "n")
			Me.scLoBauQ12.Controls.Add(lblLOBauQ1_0)
			Trace.WriteLine(lblLOBauQ1_0.Text)

			lblLOBauQ1_0.Show()
			iTop += 25
		Next
		liBauQ12LOData.Add(String.Format("{0} %", dRabatt))

		'Me.scLoBauQ12.Show()
		'Me.scLoBauQ12.Width = 50
		'Me.scLODetails.Width = Me.grpLODetails.Width - Me.scLoBauQ12.Width
		'  Me.scLODetails.Controls.Add(Me.scLoBauQ12)
		'Me.scLoBauQ12.Left = 0

	End Sub

#Region "Funktionen zur Aufbau der Lohndetails..."

	Sub ShowLODataWithCategoryValues(ByVal strCategoryValues As String)

		Me.grpLODetails.Visible = False
		ClearLODataFields()

		m_LOData = PerformPVLCalculationValueWebservice(strCategoryValues)
		If m_LOData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht geladen werden."))

			Return
		End If
		Me.bbiSave.Enabled = m_LOData.ID_Calculation > 0

		CreatelblControl4LO(m_LOData, 30, 10)
		Me.grpLODetails.Visible = True

	End Sub

	Sub CreatelblControl4LO(ByVal data As GAVCalculationDTO, ByVal iTop As Integer, ByVal iLeft As Integer)

		aLblLODataControls.Clear()
		'Me.grpLODetails.Left = Me.grpGAVCategory.Left

		Createlbl4Mindestlohn(data, 10, 10)
		Createlbl4Basislohn(data, 130, 10)

	End Sub

	Sub Createlbl4Mindestlohn(ByVal data As GAVCalculationDTO, ByVal iTop As Integer, ByVal iLeft As Integer)
		If data.ID_Calculation = 0 Then Return

		'Dim aGAVLOData As String() = data(7).Split(CChar("¦"))
		Dim strFieldName As String = "monthly_wage" ' aGAVLOData(0)
		Dim strFieldValue = data.monthly_wage ' aGAVLOData(1)
		Dim left1 = 100
		Dim left2 = 300
		Dim left3 = 200
		Dim iLeftText As Integer = iLeft + left1 '300
		Dim iLeftEinheit As Integer = iLeftText + left2 ' 450
		Dim iLeftBetrag As Integer = iLeftEinheit + 20

		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_7"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Mindesmonatslohn")
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			ctlLOValue.Font = New Font(ctlLOValue.Font, FontStyle.Bold)
			ctlLOValue.Name = "lblLODataValue_7"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_1_7"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If


		iTop += 25

		' Ferientage pro Jahr
		'aGAVLOData = data(8).Split(CChar("¦"))
		strFieldName = "number_of_vacation_days" ' aGAVLOData(0)
		strFieldValue = data.number_of_vacation_days ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_8"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Ferientage pro Jahr")
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_8"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_8"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Tage")
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If


		iTop += 25

		' Feiertage pro Jahr
		'aGAVLOData = data(6).Split(CChar("¦"))
		strFieldName = "number_of_holidays" ' aGAVLOData(0)
		strFieldValue = data.number_of_holidays ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_6"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Feiertage pro Jahr")
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_6"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_6"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Tage")
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If


		iTop += 25

		' 13. Monatslohn
		'aGAVLOData = data(16).Split(CChar("¦"))
		strFieldName = "has_13th_month_salary_compensation" ' aGAVLOData(0)
		strFieldValue = data.has_13th_month_salary_compensation ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_16"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("13. Monatslohn")
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_16"
			ctlLOValue.Text = m_Translate.GetSafeTranslationValue(If(strFieldValue.ToLower.Contains("true"), "Ja", "Nein"))
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		iTop += 25

	End Sub

	Sub Createlbl4Basislohn(ByVal data As GAVCalculationDTO, ByVal iTop As Integer, ByVal iLeft As Integer)
		If data.ID_Calculation = 0 Then Return

		'Dim aGAVLOData As String() = data(2).Split(CChar("¦"))
		Dim strFieldName As String = "basic_hourly_wage" ' aGAVLOData(0)
		Dim strFieldValue As String = data.basic_hourly_wage ' aGAVLOData(1)
		Dim NextLineTop = 40
		Dim calcInfoLineTop = 15
		Dim left1 = 100
		Dim left2 = 300
		Dim left3 = 200
		Dim iLeftText As Integer = iLeft + left1 '300
		Dim iLeftEinheit As Integer = iLeftText + left2 ' 450
		Dim iLeftBetrag As Integer = iLeftEinheit + 20

		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_2"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Basisstundenlohn")
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_2"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			ctlLOValue.Font = New Font(ctlLOValue.Font, FontStyle.Bold)

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_1_2"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += 25

		' Ferienentschädigung Prozentual
		'aGAVLOData = data(3).Split(CChar("¦"))
		strFieldName = "vacation_pay" ' aGAVLOData(0)
		strFieldValue = data.vacation_pay ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_3"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Ferienentschädigung")
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_3"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			'ctlLOValue.Appearance.Options.UseTextOptions = True
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		' Ferienentschädigung Beschreibung Prozentual
		'aGAVLOData = data(10).Split(CChar("¦"))
		strFieldName = "percentage_vacation_pay" ' aGAVLOData(0)
		strFieldValue = data.percentage_vacation_pay ' aGAVLOData(1)
		Dim bWithZusatztext As Boolean = False
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftText - 70, iTop + calcInfoLineTop)
			ctlLOBez.Name = "lblLODataBez_1_10"
			ctlLOBez.Text = Format(Val(strFieldValue) * 100, "n") & " % "
			ctlLOBez.ForeColor = Color.Gray

			'aGAVLOData = data(13).Split(CChar("¦"))
			strFieldName = "calculation_vacation_pay" ' aGAVLOData(0)
			strFieldValue = data.calculation_vacation_pay ' aGAVLOData(1)
			Dim strZusatztext As String = "des "
			If strFieldValue.Trim <> String.Empty Then
				Select Case Val(strFieldValue)
					Case 0
						strZusatztext &= ""
					Case 1
						strZusatztext &= "Basisstundenlohnes"
					Case 2
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung"
					Case 3
						strZusatztext &= "Basisstundenlohnes + Feiertagsentschädigung"
					Case 4
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + Feiertagsentschädigung"
					Case 5
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + 13. Monatslohn"
					Case 6
						strZusatztext &= "Basisstundenlohnes + Feiertagsentschädigung + 13. Monatslohn"
					Case 7
						strZusatztext &= "Basisstundenlohnes + 13. Monatslohn"
					Case 8
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + Feiertagsentschädigung + 13. Monatslohn"
					Case 9
						strZusatztext &= "Kein 13. Monatslohn vorhanden"

				End Select
				ctlLOBez.Text &= strZusatztext
				ctlLOBez.Text = m_Translate.GetSafeTranslationValue(ctlLOBez.Text)
				bWithZusatztext = If(Val(strFieldValue) = 8, True, False)
			End If
			'ctlLOBez.Size = New Size(400, If(bWithZusatztext, 50, 25))
			'ctlLOBez.Size = New Size(left3, 50) ' If(bWithZusatztext, 50, 25))
			ctlLOBez.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			'ctlLOBez.Appearance.Options.UseTextOptions = True
			ctlLOBez.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_3"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += NextLineTop 'If(bWithZusatztext, 55, 25) '25 ' 55 - If(bWithZusatztext, 0, 30)

		' Feiertagsentschädigung prozentual
		'aGAVLOData = data(4).Split(CChar("¦"))
		strFieldName = "holiday_compensation" ' aGAVLOData(0)
		strFieldValue = data.holiday_compensation ' aGAVLOData(1)
		bWithZusatztext = False
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_4"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Feiertagsentschädigung")
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_4"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			'ctlLOValue.Appearance.Options.UseTextOptions = True
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		' Ferienentschädigung Beschreibung Prozentual
		'aGAVLOData = data(11).Split(CChar("¦"))
		strFieldName = "percentage_holiday_compensation" ' aGAVLOData(0)
		strFieldValue = data.percentage_holiday_compensation ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftText - 70, iTop + calcInfoLineTop)
			ctlLOBez.Name = "lblLODataBez_1_11"
			ctlLOBez.Text = Format(Val(strFieldValue) * 100, "n") & " % "
			ctlLOBez.ForeColor = Color.Gray

			'aGAVLOData = data(14).Split(CChar("¦"))
			strFieldName = "calculation_holiday_compensation" ' aGAVLOData(0)
			strFieldValue = data.calculation_holiday_compensation ' aGAVLOData(1)
			Dim strZusatztext As String = "des "
			If Not String.IsNullOrWhiteSpace(strFieldName) Then
				Select Case Val(strFieldValue)
					Case 0
						strZusatztext &= ""
					Case 1
						strZusatztext &= "Basisstundenlohnes"
					Case 2
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung"
					Case 3
						strZusatztext &= "Basisstundenlohnes + Feiertagsentschädigung"
					Case 4
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + Feiertagsentschädigung"
					Case 5
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + 13. Monatslohn"
					Case 6
						strZusatztext &= "Basisstundenlohnes + Feiertagsentschädigung + 13. Monatslohn"
					Case 7
						strZusatztext &= "Basisstundenlohnes + 13. Monatslohn"
					Case 8
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + Feiertagsentschädigung + 13. Monatslohn"
					Case 9
						strZusatztext &= "Kein 13. Monatslohn vorhanden"

				End Select
				ctlLOBez.Text &= strZusatztext
				ctlLOBez.Text = m_Translate.GetSafeTranslationValue(ctlLOBez.Text)
				bWithZusatztext = If(Val(strFieldValue) = 8, True, False)
			End If
			'ctlLOBez.Size = New Size(400, If(bWithZusatztext, 50, 25))
			'ctlLOBez.Size = New Size(left3, 50) ' If(bWithZusatztext, 50, 25))
			'ctlLOBez.Appearance.Options.UseTextOptions = True
			ctlLOBez.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			ctlLOBez.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_11"
			ctlLOBez.Text = "CHF"
			ctlLOBez.AutoSize = True
			ctlLOBez.ForeColor = Color.Gray

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += NextLineTop 'If(bWithZusatztext, 55, 25) '25 ' 55 - If(bWithZusatztext, 0, 30)

		' 13. Monatslohn Prozentual
		'aGAVLOData = data(5).Split(CChar("¦"))
		strFieldName = "compensation_13th_month_salary" ' aGAVLOData(0)
		strFieldValue = data.compensation_13th_month_salary ' aGAVLOData(1)
		bWithZusatztext = False
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_5"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Entschädigung 13. Monatslohn")
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_5"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			'ctlLOValue.Appearance.Options.UseTextOptions = True
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		' 13. Monatslohn Beschreibung Prozentual
		'aGAVLOData = data(12).Split(CChar("¦"))
		strFieldName = "percentage_13th_month_salary" ' aGAVLOData(0)
		strFieldValue = data.percentage_13th_month_salary ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftText - 70, iTop + calcInfoLineTop)
			ctlLOBez.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			ctlLOBez.Name = "lblLODataBez_1_12"
			ctlLOBez.Text = Format(Val(strFieldValue) * 100, "n") & " % "
			ctlLOBez.ForeColor = Color.Gray

			'aGAVLOData = data(15).Split(CChar("¦"))
			strFieldName = "calculation_13th_month_salary" ' aGAVLOData(0)
			strFieldValue = data.calculation_13th_month_salary ' aGAVLOData(1)
			Dim strZusatztext As String = "des "
			If Not String.IsNullOrWhiteSpace(strFieldName) Then
				Select Case Val(strFieldValue)
					Case 0
						strZusatztext &= ""
					Case 1
						strZusatztext &= "Basisstundenlohnes"
					Case 2
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung"
					Case 3
						strZusatztext &= "Basisstundenlohnes + Feiertagsentschädigung"
					Case 4
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + Feiertagsentschädigung"
					Case 5
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + 13. Monatslohn"
					Case 6
						strZusatztext &= "Basisstundenlohnes + Feiertagsentschädigung + 13. Monatslohn"
					Case 7
						strZusatztext &= "Basisstundenlohnes + 13. Monatslohn"
					Case 8
						strZusatztext &= "Basisstundenlohnes + Ferienentschädigung + Feiertagsentschädigung + 13. Monatslohn"
					Case 9
						strZusatztext &= "Kein 13. Monatslohn vorhanden"

				End Select
				ctlLOBez.Text &= strZusatztext
				ctlLOBez.Text = m_Translate.GetSafeTranslationValue(ctlLOBez.Text)
				bWithZusatztext = If(Val(strFieldValue) = 8, True, False)
			End If
			'ctlLOBez.Size = New Size(400, 50)
			'ctlLOBez.Size = New Size(left3, 50)
			'ctlLOBez.Appearance.Options.UseTextOptions = True
			ctlLOBez.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
			ctlLOBez.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_5"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += NextLineTop 'If(bWithZusatztext, 55, 25) '25 ' 55 - If(bWithZusatztext, 0, 30)

		' Stundenlohn
		'aGAVLOData = data(9).Split(CChar("¦"))
		strFieldName = "gross_hourly_wage" ' aGAVLOData(0)
		strFieldValue = data.gross_hourly_wage ' aGAVLOData(1)
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_9"
			ctlLOBez.Text = m_Translate.GetSafeTranslationValue("Mindeststundenlohn mit 13. Monatslohn")
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldValue) Then
			Dim ctlLOValue As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_9"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			'ctlLOValue.Size = New Size(60, 20)
			ctlLOValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			'ctlLOValue.Appearance.Options.UseTextOptions = True
			ctlLOValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

			ctlLOValue.Font = New Font(ctlLOValue.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If Not String.IsNullOrWhiteSpace(strFieldName) Then
			Dim ctlLOBez As New DevExpress.XtraEditors.LabelControl ' Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_9"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += NextLineTop ' 50
		'Me.grpLODetails.Height = Me.tabRechner.Height - Me.grpLODetails.Top - 20  ' iTop

		' wenn Resor darf FAR nicht korrigiert werden. es kann nicht FAR und RESOR in einem Kanton zusammen kommen!!!
		If Not Me._bResorpflichtig Then
			' FAR AN
			'aGAVLOData = data(18).Split(CChar("¦"))
			strFieldName = "percentage_far_an" ' aGAVLOData(0)
			strFieldValue = data.percentage_far_an ' aGAVLOData(1)
			Me.lblFAN.Text = Val(strFieldValue)

			' FAR AG
			'aGAVLOData = data(19).Split(CChar("¦"))
			strFieldName = "percentage_far_ag" ' aGAVLOData(0)
			strFieldValue = data.percentage_far_ag ' aGAVLOData(1)
			If lueCanton.EditValue = "VS" AndAlso m_CurrentPVLData.gav_number = 100001 AndAlso Val(strFieldValue) > 0 Then
				m_Logger.LogWarning(String.Format("GAVNr: {0} | Kanton: {1} >>> FAR-AG is changed to 4.5 %!!! (MDNr: {2}: >>> USNR: {3})",
																					m_CurrentPVLData.gav_number, SelectedKDKanton, m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserFullName))
				strFieldValue = "4.5"
			End If
			Me.lblFAG.Text = Val(strFieldValue)
		End If
		chkFAR.Checked = Val(lblFAG.Text) + Val(lblFAN.Text) > 0

		' FAR Calc
		'aGAVLOData = data(20).Split(CChar("¦"))
		strFieldName = "calculation_far" ' aGAVLOData(0)
		strFieldValue = data.calculation_far ' aGAVLOData(1)
		iFarCalc = Val(strFieldValue)

		' FAR With BVG
		'aGAVLOData = data(21).Split(CChar("¦"))
		strFieldName = "far_bvg_relevant" ' aGAVLOData(0)
		strFieldValue = data.far_bvg_relevant ' aGAVLOData(1)
		bFarWithBVG = CBool(strFieldValue)

	End Sub

#End Region

#Region "Funktionen zur Reseten..."

	Sub ClearGAVBerufInfoFields()

		Try
			Me.scGAVBerufInfo.Controls.Clear()
			chkResor.Visible = False

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try
		Me.scGAVBerufInfo.Refresh()

	End Sub

	Sub ResetAllLBL()

		Try
			Me.grpGAVOLDCategory.Controls.Clear()

		Catch ex As Exception

		End Try

		ClearLODataFields()     ' Lohndaten löschen

		Me.lblCategoryNr.Text = "0"
		Me.lblCalculatorNr.Text = "0"
		Me.lblBaseCategoryNr.Text = "0"
		Me.lblCategoryValueNr.Text = "0"
		Me.lblAllCategoryValueNr.Text = String.Empty
		Me.grpGAVOLDCategory.Visible = False
		Me.grpGAVCategory.Visible = False
		Me.grpLODetails.Visible = False

		Me.Refresh()
		aLblControls.Clear()
		aCboControls.Clear()
		aCategoryValuesNr.Clear()

	End Sub

	Sub ClearLODataFields()
		Dim i As Integer = 0

		Try
			Me.bbiSave.Enabled = False
			Me.scLODetails.Controls.Clear()
			m_LOData = Nothing

			Me.scLoBauQ12.Controls.Clear()
			liBauQ12LOData.Clear()
			Me.scLoBauQ12.Visible = False

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Me.grpLODetails.Refresh()
		Trace.WriteLine(i & ": Controls wurden gelöscht...")
	End Sub

	Sub ClearGAVD_Fields()

		Try
			Me.scD_1.Controls.Clear()
			Me.scD_2.Controls.Clear()
			ClearGAVD_InfoFields()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Me.scD_1.Refresh()
		Me.scD_2.Refresh()
	End Sub

	Sub ClearGAVD_InfoFields()

		Try
			Me.scD_Info.Controls.Clear()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Me.scD_Info.Refresh()
	End Sub

#End Region



	'Private Sub XtraTabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XtraTabControl1.Click

	'	If Me.XtraTabControl1.SelectedTabPage.Name.ToLower = "tabrechner".ToLower AndAlso m_CurrentPVLData.gav_number) > 0 Then


	'	ElseIf Me.XtraTabControl1.SelectedTabPage.Name.ToLower = "tabdetails".ToLower AndAlso m_CurrentPVLData.gav_number) > 0 Then
	'		'GetGAV_Details()

	'	End If

	'End Sub


#Region "Funktionen für GAV_Details..."

	Sub GetGAV_Details()
		Dim iTop As Integer = 10
		Dim iLeft As Integer = 10
		Dim i As Integer = 0

		ClearGAVD_Fields()
		Dim liGAVCriterion = PerformPVLCriteriasWebservice()
		If liGAVCriterion Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Kriterien Daten konnten nicht geladen werden."))

			Return
		End If

		For Each criteria In liGAVCriterion
			'Dim aGAVValues As String() = liGAVCriterion(i).Split(CChar("¦"))
			Dim strIDCritrion As String = criteria.ID_Criterion ' aGAVValues(0)
			Dim strIDContract As String = criteria.ID_Contract '  aGAVValues(1)
			Dim strElementID As String = criteria.Element_ID '  aGAVValues(2)
			Dim strCriterionName As String = criteria.name_de '  aGAVValues(3)
			Dim strCriterionName_fr As String = criteria.name_fr '  aGAVValues(4)
			Dim strCriterionName_it As String = criteria.name_it '  aGAVValues(5)

			If (strCriterionName + strCriterionName_fr + strCriterionName_it).Trim.Length > 0 Then

				Dim ctl As New DevExpress.XtraEditors.LabelControl ' Label

				ctl.Location = New Point(iLeft, iTop)
				ctl.AutoSize = True
				If m_InitializationData.UserData.UserLanguage = "F" Then
					ctl.Text = strCriterionName_fr
				ElseIf m_InitializationData.UserData.UserLanguage = "I" Then
					ctl.Text = strCriterionName_fr
				Else
					ctl.Text = strCriterionName
				End If

				ctl.Cursor = Cursors.Hand

				ctl.Name = "lblCriterion_" & strIDCritrion
				ctl.Tag = strIDCritrion ' strCriterionValue
				ctl.ForeColor = Color.Blue
				ctl.BackColor = Color.Transparent
				ctl.Show()

				AddHandler ctl.Click, AddressOf ctl_click
				If i <= liGAVCriterion.Count / 2 Then
					Me.scD_1.Controls.Add(ctl)
				Else
					Me.scD_2.Controls.Add(ctl)
				End If
				ctl.Show()

				If Val(strElementID) = 40 Then
					'Dim ctltoRemove As Object = GetControlbyName("lblHeaderCriterion_" & strIDCritrion)
					'If Not IsNothing(ctltoRemove) Then
					'  Try
					'    ctltoRemove.remove()
					'  Catch ex As Exception

					'  End Try
					'End If
					'Dim ctlHeader As New DevExpress.XtraEditors.LabelControl ' Label

					'ctl.Location = New Point(iLeft, 20)
					'ctl.AutoSize = True
					'ctl.Text = strCriterionValue

					'ctl.Name = "lblHeaderCriterion_" & strIDCritrion
					'ctl.ForeColor = Color.Black
					'ctl.BackColor = Color.Transparent
					'ctl.Show()

					'Me.scGAVBerufInfo.Controls.Add(ctl)
					'ctl.Show()

				End If
				If i = liGAVCriterion.Count / 2 Then iTop = 10 Else iTop += 20
			End If

		Next

		Me.grpD_1.Visible = True
	End Sub

	Private Sub ctl_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim iCriterionNr As Integer = sender.name.split("_")(1)
		Dim infoLable As String = sender.text
		Dim infoKey As String = sender.tag.ToString

		ClearGAVD_InfoFields()
		Dim ctl As New DevExpress.XtraEditors.LabelControl ' Label
		If Val(iCriterionNr) = 0 Then Exit Sub

		Dim liGAVCriterionValue = PerformPVLCriteriasByIDWebservice(Val(iCriterionNr))

		Try
			Dim ctlRich = New RichEditControl
			ctlRich.Location = New Point(10, 25)
			'ctl.Anchor = AnchorStyles.Top
			ctlRich.AutoSize = False ' True
			ctlRich.Size = New Size(Me.scD_Info.Width + 10, Me.scD_Info.Height + 50)

			Dim liCtlValue = PerformPVLCriteriasByIDWebservice(infoKey)
			Trace.WriteLine(infoKey)
			ctlRich.ReadOnly = False
			Dim strTableValue As String = String.Format("<b>{0}</b></br>", infoLable)
			For Each cr In liCtlValue
				'Dim aCtlValue As String() = liCtlValue(i).Split("¦")
				If Not String.IsNullOrWhiteSpace(cr.txtTable) Then
					strTableValue &= cr.txtText ' aCtlValue(2)
				Else
					strTableValue &= cr.txtText
				End If
			Next

			ctlRich.HtmlText = strTableValue
			ctlRich.Options.HorizontalScrollbar.Visibility = RichEditScrollbarVisibility.Hidden
			ctlRich.Options.VerticalScrollbar.Visibility = RichEditScrollbarVisibility.Hidden
			ctlRich.Options.VerticalRuler.Visibility = RichEditRulerVisibility.Hidden
			ctlRich.Options.HorizontalRuler.Visibility = RichEditRulerVisibility.Hidden

			ctlRich.ReadOnly = True
			ctlRich.Name = "lblDCriterion_"
			ctlRich.ForeColor = Color.Black
			ctlRich.BackColor = Color.Transparent
			ctlRich.Show()

			Me.scD_Info.Controls.Add(ctlRich)
			ctlRich.Dock = DockStyle.Fill
			ctlRich.Show()

		Catch ex As Exception

		End Try

		btnCopyGAVD_Info.Visible = ctl.Text.Trim.Length > 0
		Me.btnCopyGAVD_Info.Top = Me.scD_Info.Top - 10
		Me.btnCopyGAVD_Info.Left = Me.scD_Info.Width - Me.btnCopyGAVD_Info.Width - 0
	End Sub


#End Region



#Region "Funktionen zur Externallinks..."


	Sub CreateExternalLinks()
		Dim iTop As Integer = 40
		Dim iLeft As Integer = 10
		Dim strDomain As String = "http://downloads.domain.com/sps_downloads/PDF/infos/PVL_GAV/"
		Dim liGAVExternalLink As New List(Of String)

		liGAVExternalLink.Add(String.Format(m_Translate.GetSafeTranslationValue("Einleitung|{0}{1}"), strDomain,
																				"Präambel_GAV_Personalverleih.pdf"))
		liGAVExternalLink.Add(String.Format(m_Translate.GetSafeTranslationValue("Was gilt|{0}{1}"), strDomain,
																				"GAV-Leporello_d.pdf"))
		liGAVExternalLink.Add(String.Format(m_Translate.GetSafeTranslationValue("FAQ zum GAV Personalverleih|{0}{1}"), strDomain,
																				"FrequentlyAskedQuestions_V3_veroeffentlicht.pdf"))
		liGAVExternalLink.Add(String.Format(m_Translate.GetSafeTranslationValue("Liste der Lohnzonen|{0}{1}"), strDomain,
																				"Lohnzonen_d_23112011.pdf"))

		liGAVExternalLink.Add(String.Format(m_Translate.GetSafeTranslationValue("Bundesratsbeschluss über ave GAV PVL|{0}{1}"), strDomain,
																				"Bundesratsbeschluss_GAVPersonalverleih_DE.pdf"))
		liGAVExternalLink.Add(String.Format(m_Translate.GetSafeTranslationValue("Gesamtübersicht alle GAV|{0}"),
																		"http://www.tempdata.ch/LohnrechnerStichwortSuchen.aspx"))

		If liGAVExternalLink.Count > 0 Then
			For i As Integer = 0 To liGAVExternalLink.Count - 1
				Dim aGAVValues As String() = liGAVExternalLink(i).Split(CChar("|"))
				Dim strLinkBez As String = aGAVValues(0)
				Dim strLinkWWW As String = aGAVValues(1)
				If strLinkWWW.Trim.Length > 0 Then

					Dim ctl As New DevExpress.XtraEditors.LabelControl ' Label

					ctl.Location = New Point(iLeft, iTop)
					ctl.AutoSize = True
					ctl.Text = strLinkBez
					ctl.Cursor = Cursors.Hand

					ctl.Name = "lblExternal_" & i
					ctl.Tag = strLinkWWW
					ctl.ForeColor = Color.Blue
					ctl.BackColor = Color.Transparent
					ctl.Show()

					Me.scExternalLinks.Controls.Add(ctl)
					AddHandler ctl.Click, AddressOf ctlExternalLinks_click
					iTop += 20
				End If

			Next
		End If

	End Sub

	Private Sub ctlExternalLinks_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim ctl As New DevExpress.XtraEditors.LabelControl ' Label
		Dim strLink = sender.tag

		Process.Start(strLink)

	End Sub

#End Region


#Region "Sonstige Hilfsfunktionen..."

	Function GetControlbyName(ByVal ControlName As String) As Object

		Try

			For Each obj As Control In Me.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next

			For Each obj As Control In Me.tabRechner.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.tabDetails.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.grpGAVOLDCategory.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.grpLODetails.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.scD_Info.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.scD_1.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.scD_2.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.scLODetails.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetControlbyName")

		End Try

		Return Nothing
	End Function

#End Region


	'Private Sub lblRecInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRecInfo.Click
	'	'Dim mouseposition As New Point(Cursor.Position)
	'	Dim rect As Rectangle = Screen.GetWorkingArea(Me)

	'	Dim barmanager As New DevExpress.XtraBars.BarManager
	'	'pcc_1Search.CloseOnLostFocus = True
	'	Me.pccExternalLinks.Manager = barmanager
	'	pccExternalLinks.ShowPopup(New Point(((rect.Width / 2) - (pccExternalLinks.Width / 2)), ((rect.Height / 2) - (pccExternalLinks.Height / 2))))

	'End Sub

	Private Sub btnCopyGAVD_Info_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyGAVD_Info.Click
		Dim ctl As Label = GetControlbyName("lblDCriterion_")

		If Not IsNothing(ctl) Then
			Dim strMessage As String = ctl.Text

			ctl = GetControlbyName("lblDCriterionHeader_")
			If Not IsNothing(ctl) Then strMessage = String.Format("{0}{1}{2}", ctl.Text, vbNewLine, strMessage)

			Clipboard.SetText(strMessage)
		End If

	End Sub


	Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick

		'SavePVLParameters()
		pccGAVInfo.HidePopup()
		SavePVLParameters_2()

	End Sub

	'Private Sub lblShowMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAchtung.Click
	'	ShowMessage()
	'End Sub

	'Private Sub ShowMessage()
	'	Dim rect As Rectangle = Screen.GetWorkingArea(Me)
	'	Dim barmanager As New DevExpress.XtraBars.BarManager
	'	'pcpSputnikMessage.CloseOnLostFocus = True

	'	pccSputnikMessage.Height = 200
	'	pccSputnikMessage.Width = 800
	'	Me.pccSputnikMessage.Manager = barmanager
	'	pccSputnikMessage.ShowPopup(New Point(((rect.Width / 2) - (pccSputnikMessage.Width / 2)), ((rect.Height / 2) - (pccSputnikMessage.Height / 2))))

	'End Sub

	Private Sub OnbbiExternLinks_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExternLinks.ItemClick
		ShowExternalLinks()
	End Sub

	Private Sub ShowExternalLinks()
		'Dim mouseposition As New Point(Me.Left + Me.grpGAVBeruf.Left + lueGruppe0.Left + 20,
		'															 Me.Top + Me.pccExternalLinks.Height - Me.grpGAVBeruf.Top - lueGruppe0.Top + 5)
		Dim rect As Rectangle = Screen.GetWorkingArea(Me)
		Dim barmanager As New DevExpress.XtraBars.BarManager

		pccExternalLinks.Height = 200
		pccExternalLinks.Width = 500
		Me.pccExternalLinks.Manager = barmanager
		pccExternalLinks.ShowPopup(New Point(((rect.Width / 2) - (pccExternalLinks.Width / 2)), ((rect.Height / 2) - (pccExternalLinks.Height / 2))))

	End Sub

	Private Sub OnbbiGAVInfo_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiGAVInfo.ItemClick
		ShowGAVInfo()
	End Sub

	Private Sub ShowGAVInfo()
		Dim rect As Rectangle = Screen.GetWorkingArea(Me)
		Dim barmanager As New DevExpress.XtraBars.BarManager

		pccGAVInfo.HidePopup()
		LoadPVLParameterInfo()

		pccGAVInfo.Height = 250
		pccGAVInfo.Width = 300

		Me.pccGAVInfo.Manager = barmanager
		'pccGAVInfo.ShowPopup(New Point(Screen.PrimaryScreen.WorkingArea.Width - pccGAVInfo.Width, Screen.PrimaryScreen.WorkingArea.Height - pccGAVInfo.Height)) ' New Point(((rect.Width / 2) - (pccGAVInfo.Width / 2)), ((rect.Height / 2) - (pccGAVInfo.Height / 2))))
		pccGAVInfo.ShowPopup(New Point(rect.Width - pccGAVInfo.Width, rect.Height - pccGAVInfo.Height))

		'Me.pccGAVInfo.ShowPopup(New Point(ClientSize.Width \ 2, pccGAVInfo.Height \ 2)) 'mouseposition)

		'Me.pccGAVInfo.ShowPopup(New Point((ClientSize.Width - pccGAVInfo.Width) \ 2, (pccGAVInfo.Height - pccGAVInfo.Height) \ 2)) 'mouseposition)

	End Sub

	Private Sub chkResor_EditValueChanged(sender As Object, e As EventArgs) Handles chkResor.EditValueChanged

		If chkResor.Checked Then
			lblFAG.ForeColor = Color.Black
			lblFAN.ForeColor = Color.Black
		Else
			lblFAG.ForeColor = Color.Red
			lblFAN.ForeColor = Color.Red
		End If

	End Sub




#Region "test methodes"

	Private Function SavePVLParameters() As Boolean
		Dim result As Boolean = True

		Dim strData2Save As String = String.Empty
		Dim strCategoryLbl As String = String.Empty
		Dim msgNotResor As String = String.Empty
		Dim msgResor As String = String.Empty
		Dim bGetLOData As Boolean = True
		Dim liEmptyFields As New List(Of String)
		Dim bBauQ12 As Boolean = liBauQ12LOData.Count > 3

		m_PVLSelectedData = String.Empty
		If chkFAR.Enabled AndAlso Val(lblFAG.Text) + Val(lblFAN.Text) > 0 Then
			If Not chkFAR.Checked Then
				msgNotResor = m_Translate.GetSafeTranslationValue("Sie haben die FAR-pflicht ausgeschaltet! Sind Sie sicher?")
				Dim answer As Boolean = False
				If m_AllowedChangeFAR Then
					answer = m_UtilityUI.ShowYesNoDialog(msgNotResor, m_Translate.GetSafeTranslationValue("FAR-pflicht"), MessageBoxDefaultButton.Button2)
				Else
					answer = True
				End If
				If answer Then
					m_Logger.LogInfo(String.Format("FAR-pflicht was deactivated: GAVNr: {0} >>> User: {1}", m_CurrentPVLData.gav_number, m_InitializationData.UserData.UserFullName))
					lblFAG.Text = 0
					lblFAN.Text = 0
					m_cFAGAnhang1 = 0
					m_cFANAnhang1 = 0
				End If

			End If
		End If
		Dim fagBeitrag As Decimal = CType(lblFAG.Text, Decimal)
		Dim fanBeitrag As Decimal = CType(lblFAN.Text, Decimal)
		Dim _cFAGAnhang1beitrag As Decimal = CType(m_cFAGAnhang1, Decimal)
		Dim _cFAnAnhang1beitrag As Decimal = CType(m_cFANAnhang1, Decimal)
		Dim maximalKWStd As Decimal = CType(m_CurrentPVLData.stdweek, Decimal)

		pccExternalLinks.HidePopup()
		If chkResor.Visible Then
			If Val(lblFAG.Text) + Val(lblFAN.Text) <> 0 Then
				msgNotResor = m_Translate.GetSafeTranslationValue("Sie haben die RESOR-pflicht ausgeschaltet! Sind Sie sicher?")
				msgResor = m_Translate.GetSafeTranslationValue("Sie haben die RESOR-pflicht eingeschaltet! Sind Sie sicher?")
				Dim answer As Boolean = False
				If m_AllowedChangeFAR Then
					answer = m_UtilityUI.ShowYesNoDialog(If(chkResor.Checked, msgResor, msgNotResor), m_Translate.GetSafeTranslationValue("RESEOR-pflicht"), MessageBoxDefaultButton.Button2)
				Else
					answer = True
				End If
				m_Logger.LogInfo(String.Format("FAR-RESOR '({0})': GAVNr: {1} >>> User: {2}", If(chkResor.Checked, msgResor, msgNotResor), m_CurrentPVLData.gav_number, m_InitializationData.UserData.UserFullName))

				If answer Then
					If Not chkResor.Checked Then
						fagBeitrag = 0
						fanBeitrag = 0
						_cFAGAnhang1beitrag = 0
						_cFAnAnhang1beitrag = 0
					End If
				Else
					If chkResor.Checked Then
						fagBeitrag = 0
						fanBeitrag = 0
						_cFAGAnhang1beitrag = 0
						_cFAnAnhang1beitrag = 0
					End If
				End If

				m_Logger.LogWarning(String.Format("Der Benutzer {0} hat die RESOR-pflicht wie folgt entschieden: {1}: {2}",
																					m_InitializationData.UserData.UserFullName,
																					If(chkResor.Checked, msgResor, msgNotResor),
																					answer))
			End If

		End If

		If Val(txtStdWeek.Text) <> m_CurrentPVLData.stdweek Then

			Dim msg As String = m_Translate.GetSafeTranslationValue("Sie haben die maximalen Wochenstunden von {0:f2} in {1:f2} geändert! Sind Sie sicher?")
			msg = String.Format(msg, m_CurrentPVLData.stdweek, Val(txtStdWeek.EditValue))

			If m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Maximalen Wochenstunden"), MessageBoxDefaultButton.Button2) Then
				m_Logger.LogWarning(String.Format("Der Benutzer {0} hat die maximalen Wochenstunden von {1:f2} in {2:f2} verändert!",
																					m_InitializationData.UserData.UserFullName,
																					m_CurrentPVLData.stdweek,
																					Val(txtStdWeek.Text)))

				maximalKWStd = CType(Val(txtStdWeek.EditValue), Decimal)

			End If

		End If

		Try

			If bBauQ12 Then
				Dim strMessage As String = "Sie versuchen den vorgeschlagenen Mindestlohn zu unterschreiten!{0}{0}"
				strMessage &= "Art. 43.2 LMV sagt lediglich, dass in Ausnahmefällen bei Lehrabgänger der Mindestlohn{0}im ersten (12%) und zweiten Jahr (10%) der Mindestlohn von {1} CHF unterschritten werden kann.{0}Ausnahmefälle sind Ausnahmefälle und müssen begründbar sein!!!{0}"
				strMessage &= "{0}Bitte kontrollieren Sie die Angaben in der Einsatzverwaltung. Sind Sie sicher?"
				DevExpress.XtraEditors.XtraMessageBox.AllowCustomLookAndFeel = True
				Dim iResult As MsgBoxResult = DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue(strMessage),
																																															 vbNewLine,
																																															 Format(Val(liLOData(2).Split("¦")(1)), "n")),
																						 m_Translate.GetSafeTranslationValue("GAV-Daten unterschreiten"),
																							MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,
																							MessageBoxDefaultButton.Button2)
				bBauQ12 = iResult = MsgBoxResult.Yes
				If iResult = MsgBoxResult.Cancel Then Return False
			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		strData2Save = "GAVNr:{0}¦MetaNr:{1}¦CalNr:{2}¦CatNr:{3}¦CatBaseNr:{4}¦CatValueNr:{5}¦LONr:{6}¦Kanton:{7}¦Beruf:{8}¦"
		Try

			For i As Integer = 0 To Me.aLblControls.Count - 1
				Dim ctl As DevExpress.XtraEditors.LabelControl = GetControlbyName(aLblControls(i))
				Dim ctlCbo As DevExpress.XtraEditors.ComboBoxEdit = GetControlbyName(aCboControls(i))
				ctlCbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
				ctlCbo.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
				ctlCbo.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

				Dim strCboText As String = String.Empty
				If Not IsNothing(ctl) Then
					If Not IsNothing(ctlCbo) Then
						strCboText = ctlCbo.Text
					End If
					If strCboText.Trim = String.Empty Then
						liEmptyFields.Add(String.Format("{0}", ctl.Text))
					End If
					strCategoryLbl &= String.Format("{0}:{1}¦", ctl.Text, strCboText)
					If i = 0 Then
						If strCboText.ToLower.Contains("Chemisch-pharmazeutische".ToLower) Then
							bGetLOData = False
						End If
					End If

				End If
			Next
			Dim gavEinstufung As String = strCategoryLbl.Replace("¦", "#").Replace(":.. ", String.Empty).Replace(":..", String.Empty)
			If m_CurrentPVLData.gav_number = 815001 Then
				gavEinstufung = gavEinstufung.Replace(":alle anderen Branchen (Achtung: Lohn- und Arbeitszeitbestimmungen der allgemeinverbindlich erklärten und der im Anhang 1 aufgeführten GAV haben Vorrang, vgl. <a href='http://www.tempdata.ch/LohnrechnerSuchen.aspx/' target='_blank'>hier</a>.)", ":alle anderen Branchen")
				gavEinstufung = gavEinstufung.Replace(":autres branches (attention: les dispositions concernant le salaire et le temps de travail des CCT étendues ou listées en annexe 1 sont prioritaires, cf. vgl. <a href='http://www.tempdata.ch/LohnrechnerSuchen.aspx/' target='_blank'>ici</a>)", ":autres branches")
				gavEinstufung = gavEinstufung.Replace(":altri rami (attenzione: le disposizioni del salario e la durata di lavoro sono dichiarate dall 'obbligatorietà generale e di cui all'appendice 1 della replica CCL hanno la precedenza, cfr. vgl. <a href='http://www.tempdata.ch/LohnrechnerSuchen.aspx/' target='_blank'>qui</a>)", ":altri rami")
			End If
			For i As Integer = Me.aLblControls.Count + 8 To 18
				strCategoryLbl &= String.Format("Res_{0}:{1}¦", i, If(i = 18, gavEinstufung, String.Empty))
			Next
			strCategoryLbl &= String.Format("PublicationDate:{0}¦", Format(PublicationOfGAV, "d"))

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		strData2Save &= String.Format("{0}", strCategoryLbl)
		strData2Save &= "Monatslohn:{9}¦FeiertagJahr:{10}¦FierienJahr:{11}¦13.Lohn:{12}¦{13}"
		strData2Save &= "BasisLohn:{14}¦FerienBetrag:{15}¦FerienProz:{16}¦FeierBetrag:{17}¦FeierProz:{18}¦"
		strData2Save &= "13.Betrag:{19}¦13.Proz:{20}¦CalcFerien:{21}¦CalcFeier:{22}¦Calc13:{23}¦"
		strData2Save &= "StdLohn:{24}¦FARAN:{25}¦FARAG:{26}¦VAN:{27}¦VAG:{28}¦"
		strData2Save &= "StdWeek:{29}¦StdMonth:{30}¦StdYear:{31}¦IsPVL:{32}¦"

		strData2Save &= "_WAG:{33}¦_WAN:{34}¦_WAG_S:{35}¦_WAN_S:{36}¦_WAG_J:{37}¦_WAN_J:{38}¦"
		strData2Save &= "_VAG:{39}¦_VAN:{40}¦_VAG_S:{41}¦_VAN_S:{42}¦_VAG_J:{43}¦_VAN_J:{44}¦"
		strData2Save &= "_FAG:{45}¦_FAN:{46}¦BauQ12:{47}¦iFANCalc:{48}¦bFANWithBVG:{49}¦"

		Dim str13Lohn As String = If(bBauQ12, liBauQ12LOData(3).Split("¦")(1), liLOData(5).Split("¦")(1))   ' 13. Betrag
		Dim str13Proz As String = If(CInt(str13Lohn) = 0 And m_CurrentPVLData.gav_number <> 815001, "0", liLOData(12).Split("¦")(1))

		Try
			strData2Save = String.Format(strData2Save,
																	 m_CurrentPVLData.gav_number,
																 m_CurrentPVLData.id_meta,
																 Me.lblCalculatorNr.Text,
																 Me.lblCategoryNr.Text,
																 Me.lblBaseCategoryNr.Text,
																 Me.lblCategoryValueNr.Text,
																 Me.lblAllCategoryValueNr.Text,
																 lueCanton.EditValue,
																 lueGruppe0.Text,
 _
																 liLOData(7).Split("¦")(1),
																 liLOData(8).Split("¦")(1),
																 liLOData(6).Split("¦")(1),
																 liLOData(16).Split("¦")(1),
																 String.Empty,
 _
																 If(bGetLOData, If(bBauQ12, liBauQ12LOData(0).Split("¦")(1), liLOData(2).Split("¦")(1)), "1"),
																 If(bBauQ12, liBauQ12LOData(1).Split("¦")(1), liLOData(3).Split("¦")(1)),
																 liLOData(10).Split("¦")(1),
																 If(bBauQ12, liBauQ12LOData(2).Split("¦")(1), liLOData(4).Split("¦")(1)),
																 liLOData(11).Split("¦")(1),
 _
																 str13Lohn,
																 str13Proz,
																 liLOData(13).Split("¦")(1),
																 liLOData(14).Split("¦")(1),
																 liLOData(15).Split("¦")(1),
 _
																 If(bGetLOData, If(bBauQ12, liBauQ12LOData(4).Split("¦")(1), liLOData(9).Split("¦")(1)), "1"),
																 fanBeitrag,
																 fagBeitrag,
																 "0.7",
																 "0.3",
 _
																 maximalKWStd,
																 Me.lblMStd.Text,
																 Me.lblJStd.Text,
																 If(m_CurrentPVLData.gav_number = 815001, 1, 0),
 _
																 Me._cWAGAnhang1,
																 Me._cWANAnhang1,
																 Me._cWAGAnhang1_S,
																 Me._cWANAnhang1_S,
																 Me._cWAGAnhang1_J,
																 Me._cWANAnhang1_J,
 _
																 Me._cVAGAnhang1,
																 Me._cVANAnhang1,
																 Me._cVAGAnhang1_S,
																 Me._cVANAnhang1_S,
																 Me._cVAGAnhang1_J,
																 Me._cVANAnhang1_J,
 _
																 _cFAGAnhang1beitrag,
																 _cFAnAnhang1beitrag,
																 If(bBauQ12, String.Format("{0}|{1}|{2}|{3}|{4}|{5}", liLOData(2).Split("¦")(1),
																													 liLOData(3),
																													 liLOData(4),
																													 liLOData(5),
																													 liLOData(9),
																													 liBauQ12LOData(5)), "0"),
																											 iFarCalc,
																											 bFarWithBVG)

			If liEmptyFields.Count > 0 Then
				Dim strMessage As String = "Folgende Felder sind noch leer:{0}{1}{0}Möchten Sie Ihre Daten übernehmen?{0}"
				Dim strFieldName As String = String.Empty

				For i As Integer = 0 To liEmptyFields.Count - 1
					strFieldName &= liEmptyFields(i) & vbNewLine
				Next

				Dim iResult As MsgBoxResult = DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue(strMessage),
																																															 vbNewLine,
																																															 strFieldName),
																						 m_Translate.GetSafeTranslationValue("GAV-Daten übernehmen"),
																							MessageBoxButtons.YesNo, MessageBoxIcon.Question,
																							MessageBoxDefaultButton.Button2)

				'Dim iResult As MsgBoxResult = MsgBox(String.Format(strMessage, vbNewLine, strFieldName), _
				'                                     MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, _
				'                                     "GAV-Daten übernehmen")
				If iResult <> MsgBoxResult.Yes Then
					Return False
				End If
			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			strData2Save = String.Empty

		End Try

		ClsDataDetail.strGAVData = strData2Save '.Split(CChar("¦"))
		m_PVLSelectedData = strData2Save

		Me.Close()


		Return result

	End Function

	Private Function SavePVLParameters_2() As Boolean
		Dim result As Boolean = True

		Dim strData2Save As String = String.Empty
		Dim strCategoryLbl As String = String.Empty
		Dim msgNotResor As String = String.Empty
		Dim msgResor As String = String.Empty
		Dim bGetLOData As Boolean = True
		Dim liEmptyFields As New List(Of String)
		Dim bBauQ12 As Boolean = liBauQ12LOData.Count > 3
		Dim loDetail = New List(Of String)

		m_PVLSelectedData = String.Empty
		If chkFAR.Enabled AndAlso Val(lblFAG.Text) + Val(lblFAN.Text) > 0 Then
			If Not chkFAR.Checked Then
				msgNotResor = m_Translate.GetSafeTranslationValue("Sie haben die FAR-pflicht ausgeschaltet! Sind Sie sicher?")
				Dim answer As Boolean = False
				If m_AllowedChangeFAR Then
					answer = m_UtilityUI.ShowYesNoDialog(msgNotResor, m_Translate.GetSafeTranslationValue("FAR-pflicht"), MessageBoxDefaultButton.Button2)
				Else
					answer = True
				End If
				If answer Then
					m_Logger.LogInfo(String.Format("FAR-pflicht was deactivated: GAVNr: {0} >>> User: {1}", m_CurrentPVLData.gav_number, m_InitializationData.UserData.UserFullName))
					lblFAG.Text = 0
					lblFAN.Text = 0
					m_cFAGAnhang1 = 0
					m_cFANAnhang1 = 0
				End If

			End If
		End If
		Dim fagBeitrag As Decimal = CType(lblFAG.Text, Decimal)
		Dim fanBeitrag As Decimal = CType(lblFAN.Text, Decimal)
		'If fagBeitrag > 0 AndAlso fagBeitrag < 0.1 Then fagBeitrag = fagBeitrag * 100
		'If fanBeitrag > 0 AndAlso fanBeitrag < 0.1 Then fanBeitrag = fanBeitrag * 100

		Dim _cFAGAnhang1beitrag As Decimal = CType(m_cFAGAnhang1, Decimal)
		Dim _cFAnAnhang1beitrag As Decimal = CType(m_cFANAnhang1, Decimal)
		Dim maximalKWStd As Decimal = CType(m_CurrentPVLData.stdweek, Decimal)

		pccExternalLinks.HidePopup()
		If chkResor.Visible Then
			If Val(lblFAG.Text) + Val(lblFAN.Text) <> 0 Then
				msgNotResor = m_Translate.GetSafeTranslationValue("Sie haben die RESOR-pflicht ausgeschaltet! Sind Sie sicher?")
				msgResor = m_Translate.GetSafeTranslationValue("Sie haben die RESOR-pflicht eingeschaltet! Sind Sie sicher?")
				Dim answer As Boolean = False
				If m_AllowedChangeFAR Then
					answer = m_UtilityUI.ShowYesNoDialog(If(chkResor.Checked, msgResor, msgNotResor), m_Translate.GetSafeTranslationValue("RESEOR-pflicht"), MessageBoxDefaultButton.Button2)
				Else
					answer = True
				End If
				m_Logger.LogInfo(String.Format("FAR-RESOR '({0})': GAVNr: {1} >>> User: {2}", If(chkResor.Checked, msgResor, msgNotResor), m_CurrentPVLData.gav_number, m_InitializationData.UserData.UserFullName))

				If answer Then
					If Not chkResor.Checked Then
						fagBeitrag = 0
						fanBeitrag = 0
						_cFAGAnhang1beitrag = 0
						_cFAnAnhang1beitrag = 0
					End If
				Else
					If chkResor.Checked Then
						fagBeitrag = 0
						fanBeitrag = 0
						_cFAGAnhang1beitrag = 0
						_cFAnAnhang1beitrag = 0
					End If
				End If

				m_Logger.LogWarning(String.Format("Der Benutzer {0} hat die RESOR-pflicht wie folgt entschieden: {1}: {2}",
																					m_InitializationData.UserData.UserFullName,
																					If(chkResor.Checked, msgResor, msgNotResor),
																					answer))
			End If

		End If

		If Val(txtStdWeek.Text) <> m_CurrentPVLData.stdweek Then

			Dim msg As String = m_Translate.GetSafeTranslationValue("Sie haben die maximalen Wochenstunden von {0:f2} in {1:f2} geändert! Sind Sie sicher?")
			msg = String.Format(msg, m_CurrentPVLData.stdweek, Val(txtStdWeek.EditValue))

			If m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Maximalen Wochenstunde"), MessageBoxDefaultButton.Button2) Then
				m_Logger.LogWarning(String.Format("Der Benutzer {0} hat die maximalen Wochenstunden von {1:f2} in {2:f2} verändert!",
																					m_InitializationData.UserData.UserFullName,
																					m_CurrentPVLData.stdweek,
																					Val(txtStdWeek.Text)))

				maximalKWStd = CType(Val(txtStdWeek.EditValue), Decimal)

			End If

		End If

		strData2Save = "GAVNr:{0}¦MetaNr:{1}¦CalNr:{2}¦CatNr:{3}¦CatBaseNr:{4}¦CatValueNr:{5}¦LONr:{6}¦Kanton:{7}¦Beruf:{8}¦"
		strData2Save = String.Format(strData2Save,
									 m_CurrentPVLData.gav_number,
									 m_CurrentPVLData.id_meta,
									 0,
									 0,
									 0,
									 0,
									 GetSalaryParameterString,
									 lueCanton.EditValue,
									 lueGruppe0.Text
									 )

		Try
			Dim j As Integer = 0
			For Each cat In m_CategoryData
				Dim strCboText As String = String.Empty
				Dim ctl As DevExpress.XtraEditors.LabelControl
				Dim ctlValue As DevExpress.XtraEditors.LabelControl
				Dim lue As DevExpress.XtraEditors.LookUpEdit

				Select Case j
					Case 0
						ctl = lblCat0
						ctlValue = lblCatValue0
						lue = lue0
					Case 1
						ctl = lblCat1
						ctlValue = lblCatValue1
						lue = lue1
					Case 2
						ctl = lblCat2
						ctlValue = lblCatValue2
						lue = lue2
					Case 3
						ctl = lblCat3
						ctlValue = lblCatValue3
						lue = lue3
					Case 4
						ctl = lblCat4
						ctlValue = lblCatValue4
						lue = lue4
					Case 5
						ctl = lblCat5
						ctlValue = lblCatValue5
						lue = lue5
					Case 6
						ctl = lblCat6
						ctlValue = lblCatValue6
						lue = lue6
					Case 7
						ctl = lblCat7
						ctlValue = lblCatValue7
						lue = lue7

					Case Else
						Return False

				End Select
				If lue.EditValue Is Nothing Then
					liEmptyFields.Add(ctl.Text)
				End If
				strCboText = lue.Text
				strCategoryLbl &= String.Format("{0}:{1}¦", ctl.Text.Split("("c)(0).Trim, strCboText)
				If strCboText.ToLower.Contains("Chemisch-pharmazeutische".ToLower) Then bGetLOData = False


				j += 1
			Next
			strCategoryLbl = strCategoryLbl.Replace(", vgl. <a href='http://www.tempdata.ch/LohnrechnerSuchen.aspx/' target='_blank'>hier</a>", "")
			strCategoryLbl = strCategoryLbl.Replace(", cfr. <a href='http://www.tempdata.ch/LohnrechnerSuchen.aspx/' target='_blank'>qui</a>", "")
			strCategoryLbl = strCategoryLbl.Replace(", cf. <a href='http://www.tempdata.ch/LohnrechnerSuchen.aspx/' target='_blank'>ici</a>", "")

			Dim gavEinstufung As String = strCategoryLbl.Replace("¦", "#").Replace(":.. ", String.Empty).Replace(":..", String.Empty)
			If m_CurrentPVLData.gav_number = 815001 Then
				gavEinstufung = gavEinstufung.Replace(":alle anderen Branchen (Achtung: Lohn- und Arbeitszeitbestimmungen der allgemeinverbindlich erklärten und der im Anhang 1 aufgeführten GAV haben Vorrang.)", ":alle anderen Branchen")
				gavEinstufung = gavEinstufung.Replace(":autres branches (attention: les dispositions concernant le salaire et le temps de travail des CCT étendues ou listées en annexe 1 sont prioritaires.", ":autres branches")
				gavEinstufung = gavEinstufung.Replace(":altri rami (attenzione: le disposizioni del salario e la durata di lavoro dei CCL dichiarati d’obbligatorietà generale e di cui all'appendice 1 della replica CCL hanno la precedenza.", ":altri rami")

			End If

			For i As Integer = m_CategoryData.Count + 8 To 18
				strCategoryLbl &= String.Format("Res_{0}:{1}¦", i, If(i = 18, gavEinstufung, String.Empty))
			Next
			strCategoryLbl &= String.Format("PublicationDate:{0:d}¦", PublicationOfGAV)



			Dim keyValuePairs = strCategoryLbl.Split("¦")

			Dim tuples As New List(Of Tuple(Of String, String))
			For Each keyValue In keyValuePairs

				Dim tuple As Tuple(Of String, String) = SplitKeyValue(keyValue)
				If Not tuple Is Nothing Then tuples.Add(tuple)
			Next

			Trace.WriteLine(String.Format("strCategoryLbl: {0}", strCategoryLbl))


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		strData2Save &= String.Format("{0}", strCategoryLbl)

		Try
			Dim loString As String = "Monatslohn:{0}¦FeiertagJahr:{1}¦FerienJahr:{2}¦13.Lohn:{3}¦"
			loDetail.Add(String.Format(loString,
																 m_LOData.monthly_wage,
																 m_LOData.number_of_holidays,
																 m_LOData.number_of_vacation_days,
																 m_LOData.has_13th_month_salary_compensation))

			loString = "BasisLohn:{0}¦FerienBetrag:{1}¦FerienProz:{2}¦FeierBetrag:{3}¦FeierProz:{4}¦"
			loDetail.Add(String.Format(loString,
																 If(bGetLOData, m_LOData.basic_hourly_wage, "1"),
																 m_LOData.vacation_pay,
																 m_LOData.percentage_vacation_pay,
																 m_LOData.holiday_compensation,
																 m_LOData.percentage_holiday_compensation))
			'If (m_LOData.holiday_compensation = 0 AndAlso m_CurrentPVLData.gav_number <> 815001, 0, m_LOData.percentage_holiday_compensation)))

			loString = "13.Betrag:{0}¦13.Proz:{1}¦CalcFerien:{2}¦CalcFeier:{3}¦Calc13:{4}¦"
			loDetail.Add(String.Format(loString,
																 m_LOData.compensation_13th_month_salary,
																 m_LOData.percentage_13th_month_salary,
																 m_LOData.calculation_vacation_pay,
																 m_LOData.calculation_holiday_compensation,
																 m_LOData.calculation_13th_month_salary))

			loString = "StdLohn:{0}¦FARAN:{1}¦FARAG:{2}¦VAN:{3}¦VAG:{4}¦"
			loDetail.Add(String.Format(loString,
																 If(bGetLOData, m_LOData.gross_hourly_wage, "1"),
																 fanBeitrag,
																 fagBeitrag,
																 "0.7",
																 "0.3"))

			loString = "StdWeek:{0}¦StdMonth:{1}¦StdYear:{2}¦IsPVL:{3}¦"
			loDetail.Add(String.Format(loString,
																 maximalKWStd,
																 Me.lblMStd.Text,
																 Me.lblJStd.Text,
																 If(m_CurrentPVLData.gav_number = 815001, 1, 0)))

			loString = "_WAG:{0}¦_WAN:{1}¦_WAG_S:{2}¦_WAN_S:{3}¦_WAG_J:{4}¦_WAN_J:{5}¦"
			loDetail.Add(String.Format(loString,
																 Me._cWAGAnhang1,
																 Me._cWANAnhang1,
																 Me._cWAGAnhang1_S,
																 Me._cWANAnhang1_S,
																 Me._cWAGAnhang1_J,
																 Me._cWANAnhang1_J))

			loString = "_VAG:{0}¦_VAN:{1}¦_VAG_S:{2}¦_VAN_S:{3}¦_VAG_J:{4}¦_VAN_J:{5}¦"
			loDetail.Add(String.Format(loString,
																 Me._cVAGAnhang1,
																 Me._cVANAnhang1,
																 Me._cVAGAnhang1_S,
																 Me._cVANAnhang1_S,
																 Me._cVAGAnhang1_J,
																 Me._cVANAnhang1_J))

			loString = "_FAG:{0}¦_FAN:{1}¦BauQ12:{2}¦iFANCalc:{3}¦bFANWithBVG:{4}¦"
			loDetail.Add(String.Format(loString,
																 _cFAGAnhang1beitrag,
																 _cFAnAnhang1beitrag,
																 "0",
																 iFarCalc,
																 bFarWithBVG))

			If liEmptyFields.Count > 0 Then
				Dim strMessage As String = "Folgende Felder sind noch leer:{0}{1}{0}Möchten Sie Ihre Daten übernehmen?{0}"
				Dim strFieldName As String = String.Empty

				For i As Integer = 0 To liEmptyFields.Count - 1
					strFieldName &= liEmptyFields(i) & vbNewLine
				Next

				Dim iResult As MsgBoxResult = DevExpress.XtraEditors.XtraMessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue(strMessage),
																																															 vbNewLine,
																																															 strFieldName),
																						 m_Translate.GetSafeTranslationValue("GAV-Daten übernehmen"),
																							MessageBoxButtons.YesNo, MessageBoxIcon.Question,
																							MessageBoxDefaultButton.Button2)

				If iResult <> MsgBoxResult.Yes Then
					Return False
				End If
			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			strData2Save = String.Empty

		End Try

		strData2Save &= String.Join("", loDetail.ToList())
		ClsDataDetail.strGAVData = strData2Save '.Split(CChar("¦"))

		m_PVLSelectedData = strData2Save

		Me.Close()


		Return result

	End Function

	Private Function SplitKeyValue(ByVal keyValuePair As String) As Tuple(Of String, String)

		Dim result As Tuple(Of String, String) = Nothing

		If String.IsNullOrWhiteSpace(keyValuePair) Then
			Return Nothing
		End If

		Dim indexOfFirstColon = keyValuePair.IndexOf(":"c)
		If indexOfFirstColon < 0 Then Return result

		Dim key As String = keyValuePair.Substring(0, indexOfFirstColon)
		Dim value As String = keyValuePair.Substring(indexOfFirstColon + 1, keyValuePair.Length - (indexOfFirstColon + 1))
		If value.IndexOf("|") > 0 Then
			indexOfFirstColon = value.IndexOf("|")
			value = value.Substring(0, indexOfFirstColon)
		End If

		result = New Tuple(Of String, String)(key, value)

		Return result
	End Function

	Sub SetCategoriesLabels()
		Dim iTop As Integer = 40 ' Me.lbl_Gruppe0.Top + Me.lbl_Gruppe0.Height + 40
		Dim iLeft As Integer = 30 ' Me.lbl_Gruppe0.Left
		Dim iTopCbo As Integer = 50 ' Me.Cbo_Gruppe0.Top + Me.Cbo_Gruppe0.Height + 50
		Dim iLeftCbo As Integer = 150 ' Me.Cbo_Gruppe0.Left + 50
		Dim iOldBaseCategoryNr As Integer = 0
		Dim i As Integer = 0
		Dim orglabelLeft As Integer = lblCat0.Left
		Dim orgLueLeft As Integer = lue0.Left
		Dim orgLueWidth As Integer = lue0.Width
		Dim hasBaseCat As Boolean = False
		Dim hasSubBaseCat As Boolean = False
		grpGAVCategory.Visible = False

		For i = 0 To 7
			ResetCategoryDropDown(i)
		Next
		i = 0
		RemoveHandler lue0.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue1.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue2.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue3.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue4.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue5.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue6.EditValueChanged, AddressOf lueCategory_EditValueChanged
		RemoveHandler lue7.EditValueChanged, AddressOf lueCategory_EditValueChanged

		ResetCategoryControls()
		Dim data = PerformPVLCategoryNamesWebservice()
		m_CategoryData = data
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kategorie Daten konnten geladen werden."))
			Return
		End If

		Try

			For Each cat In data
				Dim strCategoryName As String = cat.name_de ' aGAVValues(aGAVValues.Length - 1)
				Dim strIDCategory As String = cat.ID_Category '  aGAVValues(0)
				Dim strIDCalculator As String = cat.ID_Calculator '  aGAVValues(1)
				Dim strIDBaseCategory As String = cat.ID_BaseCategory '  aGAVValues(2)

				Dim ctl As DevExpress.XtraEditors.LabelControl
				Dim ctlValue As DevExpress.XtraEditors.LabelControl
				Dim lue As DevExpress.XtraEditors.LookUpEdit
				Select Case i
					Case 0
						ctl = lblCat0
						ctlValue = lblCatValue0
						lue = lue0
					Case 1
						ctl = lblCat1
						ctlValue = lblCatValue1
						lue = lue1
					Case 2
						ctl = lblCat2
						ctlValue = lblCatValue2
						lue = lue2
					Case 3
						ctl = lblCat3
						ctlValue = lblCatValue3
						lue = lue3
					Case 4
						ctl = lblCat4
						ctlValue = lblCatValue4
						lue = lue4
					Case 5
						ctl = lblCat5
						ctlValue = lblCatValue5
						lue = lue5
					Case 6
						ctl = lblCat6
						ctlValue = lblCatValue6
						lue = lue6
					Case 7
						ctl = lblCat7
						ctlValue = lblCatValue7
						lue = lue7

					Case Else
						m_Logger.LogWarning(String.Format("SetCategoriesLabels: index is not registered: {0} >>> gav_number: {1} more fields are available!!!", i, m_CurrentPVLData.gav_number))
						Continue For

				End Select

				ctl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
				ctl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
				ctl.Tag = New TextBoxItem(strCategoryName, strIDCategory, strIDCalculator, strIDBaseCategory)
				ctl.BackColor = Color.Transparent

				lue.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
				lue.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

				If Val(strIDBaseCategory) = 0 Then
					ctl.Text = String.Format("{0} ({1})", strCategoryName, strIDCategory)
					ctl.ForeColor = Color.Black

					iOldBaseCategoryNr = 0

					ctl.Left = orglabelLeft
					lue.Left = orgLueLeft
					lue.Width = orgLueWidth

				Else
					ctl.Text = String.Format(":.. {0} ({1}-{2})", strCategoryName, strIDCategory, strIDBaseCategory)
					ctl.ForeColor = Color.Red
					hasBaseCat = True

					If iOldBaseCategoryNr = 0 Then
						iOldBaseCategoryNr = Val(strIDBaseCategory)

						hasSubBaseCat = False

					Else
						hasSubBaseCat = iOldBaseCategoryNr <> Val(strIDBaseCategory)

					End If
					ctl.Left = orglabelLeft + If(hasSubBaseCat, 20, 0) + 20
					lue.Left = orgLueLeft + If(hasSubBaseCat, 20, 0) + 20
					lue.Width = orgLueWidth - If(hasSubBaseCat, 20, 0) - 20

				End If
				hasBaseCat = False
				hasSubBaseCat = False

				Dim liGAVCategoryValues As New BindingList(Of GAVCategoryValueDTO)
				If Val(strIDBaseCategory) = 0 Then
					liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(strIDCategory), Nothing)
				Else
					ReadPreviousCategoryValue(i - 1)

				End If
				If liGAVCategoryValues Is Nothing Then
					m_UtilityUI.ShowErrorDialog("Weitere Kategorie Daten konnten nicht geladen werden.")

					Return
				End If
				'ResetCategoryDropDown(i)
				AddHandler lue.EditValueChanged, AddressOf lueCategory_EditValueChanged
				If lue.Properties.DataSource Is Nothing Then
					lue.Properties.DataSource = liGAVCategoryValues
				Else
					liGAVCategoryValues = lue.Properties.DataSource
				End If

				SetSpecialCategoryValues(cat, liGAVCategoryValues, ctl, ctlValue, lue)


				i += 1

			Next
			ReadSelectedCategoryValues()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Building Categories: {0}", ex.ToString))
		End Try

		Me.grpGAVCategory.Visible = True

	End Sub

	Private Sub SetSpecialCategoryValues(ByVal cat As GAVCategoryDTO, ByVal liGAVCategoryValues As BindingList(Of GAVCategoryValueDTO), ByVal ctl As LabelControl, ByVal ctlvalue As LabelControl, ByVal lue As LookUpEdit)
		Dim prepareValue As Boolean = False
		Dim selectValue As GAVCategoryValueDTO = Nothing

		Try

			If Not liGAVCategoryValues Is Nothing AndAlso liGAVCategoryValues.Count > 0 Then
				If cat.name_de = "Alter" OrElse cat.name_de = "Età" OrElse cat.name_de = "Âge" OrElse cat.name_de = "age" Then
					prepareValue = True
					Dim maxAge As Integer = Math.Min(m_ExistingEmployeeData.EmployeeSUVABirthdateAge, 65)
					If liGAVCategoryValues.Count > 0 Then
						selectValue = liGAVCategoryValues.Where(Function(rec) rec.Text_De = maxAge).FirstOrDefault()
					End If

				ElseIf cat.name_de = "Jahr" OrElse cat.name_de = "Anno" OrElse cat.name_de = "Année" Then
					prepareValue = True
					Dim maxYear As Integer = Math.Min(Now.Year, Val(liGAVCategoryValues(liGAVCategoryValues.Count - 1).Text_De))
					If liGAVCategoryValues.Count > 0 Then
						selectValue = liGAVCategoryValues.Where(Function(rec) rec.Text_De = maxYear).FirstOrDefault()
					End If

				ElseIf cat.name_de = "Kanton" OrElse cat.name_de = "canton" OrElse cat.name_de = "Cantone" OrElse cat.name_de = "KantonRegion" OrElse cat.name_de = "Canton - région" OrElse cat.name_de = "Cantone - regione" Then
					prepareValue = True
					If liGAVCategoryValues.Count > 0 Then
						selectValue = liGAVCategoryValues.Where(Function(rec) rec.Text_De = lueCanton.EditValue).FirstOrDefault()

					End If
				Else
					prepareValue = False

				End If
				If prepareValue AndAlso Not selectValue Is Nothing Then
					lue.EditValue = selectValue.ID_CategoryValue
				End If

			End If

		Catch ex As Exception

		End Try

		ctl.Visible = True
		ctlvalue.Visible = True
		lue.Visible = True
		lue.Enabled = liGAVCategoryValues.Count > 0

	End Sub

	Private Sub ResetCategoryControls()

		lblCat0.Text = String.Empty
		lue0.Properties.DataSource = Nothing
		lblCat0.Visible = False
		lblCatValue0.Visible = False
		lue0.Visible = False

		lblCat1.Text = String.Empty
		lue1.Properties.DataSource = Nothing
		lblCat1.Visible = False
		lblCatValue1.Visible = False
		lue1.Visible = False

		lblCat2.Text = String.Empty
		lue2.Properties.DataSource = Nothing
		lblCat2.Visible = False
		lblCatValue2.Visible = False
		lue2.Visible = False

		lblCat3.Text = String.Empty
		lue3.Properties.DataSource = Nothing
		lblCat3.Visible = False
		lblCatValue3.Visible = False
		lue3.Visible = False

		lblCat4.Text = String.Empty
		lue4.Properties.DataSource = Nothing
		lblCat4.Visible = False
		lblCatValue4.Visible = False
		lue4.Visible = False

		lblCat5.Text = String.Empty
		lue5.Properties.DataSource = Nothing
		lblCat5.Visible = False
		lblCatValue5.Visible = False
		lue5.Visible = False

		lblCat6.Text = String.Empty
		lue6.Properties.DataSource = Nothing
		lblCat6.Visible = False
		lblCatValue6.Visible = False
		lue6.Visible = False

		lblCat7.Text = String.Empty
		lue7.Properties.DataSource = Nothing
		lblCat7.Visible = False
		lblCatValue7.Visible = False
		lue7.Visible = False

	End Sub

	Sub CreateNewCategoryLookupEdit(ByVal categoryData As GAVCategoryDTO, ByVal i As Integer, ByVal iTop As Integer, ByVal iLeft As Integer)
		Dim strCategoryName As String = categoryData.name_de ' liData(liData.Count - 1)
		Dim strIDCategory As String = categoryData.ID_Category ' liData(0)
		Dim strIDCalculator As String = categoryData.ID_Calculator ' liData(1)
		Dim strIDBaseCategory As String = categoryData.ID_BaseCategory ' liData(2)

		Dim ctl As New DevExpress.XtraEditors.LookUpEdit
		Select Case i
			Case 0
				ctl = lue0
			Case 1
				ctl = lue1
			Case 2
				ctl = lue2
			Case 3
				ctl = lue3
			Case 4
				ctl = lue4
			Case 5
				ctl = lue5
			Case 6
				ctl = lue6
			Case 7
				ctl = lue7

		End Select

		ctl.Properties.DataSource = categoryData
		ctl.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
		ctl.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
		ctl.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

		ctl.Name = String.Format("CboCategory_{0}_{1}_{2}_", strIDCategory, strIDCalculator, strIDBaseCategory)
		If strCategoryName = m_Translate.GetSafeTranslationValue("Alter") OrElse strCategoryName = "Età" Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		ElseIf strCategoryName = m_Translate.GetSafeTranslationValue("Kanton") Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		ElseIf strCategoryName = m_Translate.GetSafeTranslationValue("Jahr") Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		Else
			ctl.Tag = String.Empty

		End If

		ctl.ForeColor = Color.Black

		'AddHandler ctl.QueryPopUp, AddressOf ctlCbo_DropDown
		'AddHandler ctl.EditValueChanged, AddressOf lueCategory_EditValueChanged
		'aCboControls.Add(ctl.Name)

	End Sub

	Private Sub lueCategory_EditValueChanged(sender As System.Object, e As System.EventArgs)
		Dim catNumber As Integer = 0

		If m_SuppressUIEvents Then
			Trace.WriteLine("ignoring changes on editvalue...")
			Return
		End If

		Select Case sender.name.ToString.ToLower
			Case "lue0"
				catNumber = 0

			Case "lue1"
				catNumber = 1

			Case "lue2"
				catNumber = 2

			Case "lue3"
				catNumber = 3

			Case "lue4"
				catNumber = 4

			Case "lue5"
				catNumber = 5

			Case "lue6"
				catNumber = 6

			Case "lue7"
				catNumber = 7

			Case Else
				Return

		End Select

		ReadPreviousCategoryValue(catNumber)

	End Sub

	Private Sub ReadPreviousCategoryValue(ByVal previousCatNumber As Integer)

		Dim dataCategory0 = SelectedPVLCategory0Data
		Dim dataCategory1 = SelectedPVLCategory1Data
		Dim dataCategory2 = SelectedPVLCategory2Data
		Dim dataCategory3 = SelectedPVLCategory3Data
		Dim dataCategory4 = SelectedPVLCategory4Data
		Dim dataCategory5 = SelectedPVLCategory5Data
		Dim dataCategory6 = SelectedPVLCategory6Data
		Dim dataCategory7 = SelectedPVLCategory7Data
		Dim liGAVCategoryValues As New BindingList(Of GAVCategoryValueDTO)

		Try

			Trace.WriteLine(String.Format("sender.name.ToString.ToLower: lue{0}", previousCatNumber))

			Select Case previousCatNumber
				Case 0
					If m_CategoryData.Count < 2 Then Exit Select
					If m_CategoryData(1).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(1).ID_Category), lue0.EditValue) ' dataCategory0.ID_CategoryValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(1).ID_Category), Nothing)
					End If
					lue1.Properties.DataSource = liGAVCategoryValues
					lue1.Enabled = liGAVCategoryValues.Count > 0

				Case 1
					If m_CategoryData.Count < 3 Then Exit Select
					If m_CategoryData(2).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(2).ID_Category), lue1.EditValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(2).ID_Category), Nothing)
					End If
					lue2.Properties.DataSource = liGAVCategoryValues
					lue2.Enabled = liGAVCategoryValues.Count > 0

				Case 2
					If m_CategoryData.Count < 4 Then Exit Select
					If m_CategoryData(3).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(3).ID_Category), lue2.EditValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(3).ID_Category), Nothing)
					End If
					lue3.Properties.DataSource = liGAVCategoryValues
					lue3.Enabled = liGAVCategoryValues.Count > 0

				Case 3
					If m_CategoryData.Count <= 4 Then Exit Select
					If m_CategoryData(4).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(4).ID_Category), lue3.EditValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(4).ID_Category), Nothing)
					End If
					lue4.Properties.DataSource = liGAVCategoryValues
					lue4.Enabled = liGAVCategoryValues.Count > 0

				Case 4
					If m_CategoryData.Count <= 5 Then Exit Select
					If m_CategoryData(5).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(5).ID_Category), lue4.EditValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(5).ID_Category), Nothing)
					End If
					lue5.Properties.DataSource = liGAVCategoryValues
					lue5.Enabled = liGAVCategoryValues.Count > 0

				Case 5
					If m_CategoryData.Count <= 6 Then Exit Select
					If m_CategoryData(6).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(6).ID_Category), lue5.EditValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(6).ID_Category), Nothing)
					End If
					lue6.Properties.DataSource = liGAVCategoryValues
					lue6.Enabled = liGAVCategoryValues.Count > 0

				Case 6
					If m_CategoryData.Count <= 7 Then Exit Select
					If m_CategoryData(7).ID_BaseCategory > 0 Then
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(7).ID_Category), lue6.EditValue)
					Else
						liGAVCategoryValues = PerformPVLCategoryValueWebservice(Val(m_CategoryData(7).ID_Category), Nothing)
					End If
					lue7.Properties.DataSource = liGAVCategoryValues
					lue7.Enabled = liGAVCategoryValues.Count > 0

				Case Else
					'Return

			End Select

			ReadSelectedCategoryValues()

			ShowLODataWithCategoryValues(String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
																								 Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue), Val(lue3.EditValue),
																								 Val(lue4.EditValue), Val(lue5.EditValue), Val(lue6.EditValue), Val(lue7.EditValue)))

		Catch ex As Exception

		End Try

	End Sub

	Private Sub ReadSelectedCategoryValues()

		lblCatValue0.Text = lue0.EditValue
		lblCatValue1.Text = lue1.EditValue
		lblCatValue2.Text = lue2.EditValue
		lblCatValue3.Text = lue3.EditValue
		lblCatValue4.Text = lue4.EditValue
		lblCatValue5.Text = lue5.EditValue
		lblCatValue6.Text = lue6.EditValue
		lblCatValue7.Text = lue7.EditValue

		lblAllCategoryValueNr.Text = GetSalaryParameterString()

	End Sub

	Private Function GetSalaryParameterString() As String
		Dim result As String = String.Empty

		Select Case m_CategoryData.Count
			Case 1
				result = String.Format("{0}", Val(lue0.EditValue))
			Case 2
				result = String.Format("{0}, {1}", Val(lue0.EditValue), Val(lue1.EditValue))
			Case 3
				result = String.Format("{0}, {1}, {2}", Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue))
			Case 4
				result = String.Format("{0}, {1}, {2}, {3}", Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue), Val(lue3.EditValue))
			Case 5
				result = String.Format("{0}, {1}, {2}, {3}, {4}", Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue), Val(lue3.EditValue), Val(lue4.EditValue))
			Case 6
				result = String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
									   Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue), Val(lue3.EditValue),
									   Val(lue4.EditValue), Val(lue5.EditValue))
			Case 7
				result = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
									   Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue), Val(lue3.EditValue),
									   Val(lue4.EditValue), Val(lue5.EditValue), Val(lue6.EditValue))
			Case 8
				result = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
									   Val(lue0.EditValue), Val(lue1.EditValue), Val(lue2.EditValue), Val(lue3.EditValue),
									   Val(lue4.EditValue), Val(lue5.EditValue), Val(lue6.EditValue), Val(lue7.EditValue))

		End Select


		Return result

	End Function

	Private Sub LoadPVLParameterInfo()

		lblGAVNr.Text = m_CurrentPVLData.gav_number
		lblMetaNr.Text = m_CurrentPVLData.id_meta
		lblAllCategoryValueNr.Text = GetSalaryParameterString()

		lblWStd.Text = m_CurrentPVLData.stdweek
		lblMStd.Text = m_CurrentPVLData.stdmonth
		lblJStd.Text = m_CurrentPVLData.stdyear

	End Sub

	Private Sub tabRechner_Paint(sender As Object, e As PaintEventArgs) Handles tabRechner.Paint

	End Sub

	Private Sub scD_1_Click(sender As Object, e As EventArgs) Handles scD_1.Click

	End Sub


#End Region

	Private Class PVLArchiveDBData
		Public Property ID As Integer
		Public Property DbName As String
		Public Property DbConnstring As String

	End Class


End Class

