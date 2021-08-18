
Imports System.Reflection.Assembly
Imports System.ComponentModel
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenIndividuell
Imports DevExpress.XtraGrid.Views.Base
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPProgUtility.ProgPath
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports DevExpress.XtraEditors.Controls
'Imports SPS.SalaryValueCalculation
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Listing

Public Class frmMAGuthaben
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_MandantFormXMLFile As String

	Private m_mandant As Mandant
	Private m_utility As Utilities
	Private m_ProgPath As ClsProgPath

	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_CurrenctGuthabenData As GuthabenData

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_connectionString As String

	Private m_FremdGuthabenData As IEnumerable(Of LOLDataFoRepeatLA4LOBack)
	Private m_currentLOLData As LOLDataFoRepeatLA4LOBack

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False
	Private m_EmployeeNumber As Integer
	'Private m_SalaryCalculationData As SalaryValueCalculator

#End Region


#Region "private consts"

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "public propertes"

	Public Property EmployeeNumber As Integer

#End Region

#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_mandant = New Mandant
		m_UtilityUi = New UtilityUI
		m_utility = New Utilities
		m_ProgPath = New ClsProgPath
		'm_SalaryCalculationData = New SalaryValueCalculator(m_InitializationData)

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		xtabGFremdSystem.PageEnabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 140, m_InitializationData.MDData.MDNr)

		AddHandler gvFremdGuthaben.RowCellClick, AddressOf Ongv_FremdGuthabenRowCellClick
		AddHandler gvFremdGuthaben.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		Reset()

		LoadMandantDropDownData()

		' Suppress UI Events
		Dim suppressUIEventsState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		' Disable Supress UI events 
		m_SuppressUIEvents = suppressUIEventsState


	End Sub

#End Region


#Region "Private properties"

	''' <summary>
	''' Gets the selected fremd guthaben.
	''' </summary>
	''' <returns>The selected customer or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedFremdGuthaben As LOLDataFoRepeatLA4LOBack
		Get
			Dim gvRP = TryCast(grdFremdGuthaben.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim report = CType(gvRP.GetRow(selectedRows(0)), LOLDataFoRepeatLA4LOBack)

					Return report
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region

	Public Sub LoadData()

		m_EmployeeNumber = EmployeeNumber
		LoadSummeryData()
		LoadDetailData()

		LoadLAData(Now.Year)
		LoadFremdGuthaben()

	End Sub

	Private Sub Reset()

		lueLAData.EditValue = Nothing
		txtFerienAmount.EditValue = Nothing

		ResetSummeryGrid()
		ResetDetailGrid()
		ResetFremdGuthabenGrid()
		ResetMandantDropDown()
		ResetLADropDown()

		TranslateControls()

	End Sub

	Private Sub TranslateControls()

		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
		Me.XtraTabPage1.Text = m_Translate.GetSafeTranslationValue(Me.XtraTabPage1.Text)
		Me.XtraTabPage2.Text = m_Translate.GetSafeTranslationValue(Me.XtraTabPage2.Text)
		Me.xtabGFremdSystem.Text = m_Translate.GetSafeTranslationValue(Me.xtabGFremdSystem.Text)

		lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)
		lblFerien.Text = m_Translate.GetSafeTranslationValue(lblFerien.Text)
		lblLohnart.Text = m_Translate.GetSafeTranslationValue(lblLohnart.Text)
		btnSave.Text = m_Translate.GetSafeTranslationValue(btnSave.Text)
		btnNewFremdGuthaben.Text = m_Translate.GetSafeTranslationValue(btnNewFremdGuthaben.Text)

	End Sub

	Private Sub ResetSummeryGrid()

		gvSummery.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvSummery.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvSummery.OptionsView.ShowGroupPanel = False
		gvSummery.OptionsView.ShowIndicator = False
		gvSummery.OptionsView.ShowAutoFilterRow = False


		gvSummery.Columns.Clear()

		Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnBezeichnung.Name = "Bezeichnung"
		columnBezeichnung.FieldName = "Bezeichnung"
		columnBezeichnung.Visible = True
		columnBezeichnung.Width = 400
		gvSummery.Columns.Add(columnBezeichnung)

		Dim columnStdValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStdValue.Caption = m_Translate.GetSafeTranslationValue("Stunden")
		columnStdValue.Name = "StdValue"
		columnStdValue.FieldName = "StdValue"
		columnStdValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnStdValue.AppearanceHeader.Options.UseTextOptions = True
		columnStdValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnStdValue.DisplayFormat.FormatString = "N2"
		columnStdValue.Visible = True
		columnStdValue.BestFit()
		gvSummery.Columns.Add(columnStdValue)

		Dim columnBetragValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBetragValue.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnBetragValue.Name = "BetragValue"
		columnBetragValue.FieldName = "BetragValue"
		columnBetragValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBetragValue.AppearanceHeader.Options.UseTextOptions = True
		columnBetragValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBetragValue.DisplayFormat.FormatString = "N2"
		columnBetragValue.Visible = True
		columnBetragValue.BestFit()
		gvSummery.Columns.Add(columnBetragValue)

		grdSummery.DataSource = Nothing

	End Sub

	Private Sub ResetDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = False

		gvDetail.Columns.Clear()

		Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnBezeichnung.Name = "Bezeichnung"
		columnBezeichnung.FieldName = "Bezeichnung"
		columnBezeichnung.Visible = True
		columnBezeichnung.Width = 400
		gvDetail.Columns.Add(columnBezeichnung)

		Dim columnperiode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnperiode.Caption = m_Translate.GetSafeTranslationValue("Periode")
		columnperiode.Name = "periode"
		columnperiode.FieldName = "periode"
		columnperiode.Visible = True
		columnperiode.BestFit()
		gvDetail.Columns.Add(columnperiode)

		Dim columnBetragValue As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBetragValue.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnBetragValue.Name = "BetragValue"
		columnBetragValue.FieldName = "BetragValue"
		columnBetragValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBetragValue.AppearanceHeader.Options.UseTextOptions = True
		columnBetragValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBetragValue.DisplayFormat.FormatString = "N2"
		columnBetragValue.Visible = True
		columnBetragValue.BestFit()
		gvDetail.Columns.Add(columnBetragValue)

		Dim columnArt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnArt.Caption = m_Translate.GetSafeTranslationValue("Art")
		columnArt.Name = "art"
		columnArt.FieldName = "art"
		columnArt.Visible = True
		columnArt.BestFit()
		gvDetail.Columns.Add(columnArt)

		grdDetail.DataSource = Nothing

	End Sub

	Private Sub ResetFremdGuthabenGrid()

		gvFremdGuthaben.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvFremdGuthaben.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvFremdGuthaben.OptionsView.ShowGroupPanel = False
		gvFremdGuthaben.OptionsView.ShowIndicator = False
		gvFremdGuthaben.OptionsView.ShowAutoFilterRow = False


		gvFremdGuthaben.Columns.Clear()

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart-Nr.")
		columnLANr.Name = "LANr"
		columnLANr.FieldName = "LANr"
		columnLANr.Visible = True
		columnLANr.Width = 100
		columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnLANr.DisplayFormat.FormatString = "0.###"
		gvFremdGuthaben.Columns.Add(columnLANr)

		Dim columnRPText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRPText.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnRPText.Name = "RPText"
		columnRPText.FieldName = "RPText"
		columnRPText.Visible = True
		columnRPText.Width = 300
		gvFremdGuthaben.Columns.Add(columnRPText)

		Dim columnPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPeriode.Caption = m_Translate.GetSafeTranslationValue("Periode")
		columnPeriode.Name = "Periode"
		columnPeriode.FieldName = "Periode"
		columnPeriode.Visible = True
		columnPeriode.Width = 100
		gvFremdGuthaben.Columns.Add(columnPeriode)

		Dim columnm_Btr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnm_Btr.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnm_Btr.Name = "m_Btr"
		columnm_Btr.FieldName = "m_Btr"
		columnm_Btr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnm_Btr.AppearanceHeader.Options.UseTextOptions = True
		columnm_Btr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnm_Btr.DisplayFormat.FormatString = "N5"
		columnm_Btr.Visible = True
		columnm_Btr.Width = 200
		gvFremdGuthaben.Columns.Add(columnm_Btr)


		grdFremdGuthaben.DataSource = Nothing

	End Sub

	Private Sub ResetMandantDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowFooter = False
		lueMandant.Properties.DropDownRows = 10
		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the LA drop down data.
	''' </summary>
	Private Sub ResetLADropDown()

		lueLAData.Enabled = True

		lueLAData.Properties.DisplayMember = "DisplayText"
		lueLAData.Properties.ValueMember = "LANr"

		Dim columns = lueLAData.Properties.Columns
		columns.Clear()

		Dim laNrColumn As New LookUpColumnInfo("LANr", 0)
		laNrColumn.FormatString = "0.###"
		laNrColumn.FormatType = DevExpress.Utils.FormatType.Custom

		columns.Add(laNrColumn)
		columns.Add(New LookUpColumnInfo("LALoText", 0))

		lueLAData.Properties.ShowHeader = False
		lueLAData.Properties.ShowFooter = False
		lueLAData.Properties.DropDownRows = 10
		lueLAData.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLAData.Properties.SearchMode = SearchMode.AutoComplete
		lueLAData.Properties.AutoSearchColumnIndex = 0

		lueLAData.Properties.NullText = String.Empty

		'Dim suppressUIEventsState = m_SuppressUIEvents
		'm_SuppressUIEvents = True
		lueLAData.EditValue = Nothing
		'm_SuppressUIEvents = suppressUIEventsState

	End Sub


#Region "form handling"

	Private Sub frmMAGuthaben_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	Private Sub frmMAGuthaben_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormColor:{1}", strMethodeName, ex.Message))

		End Try

		Try
			If My.Settings.iHeight > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.iHeight)
			If My.Settings.iWidth > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.iWidth)
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Dim strMsg As String = m_Translate.GetSafeTranslationValue("Kandidaten-Guthaben: {0} {1} {2}")
		Dim objGuthaben As New SPS.MA.ZGFunctionality.ClsZGData
		Dim objData As New List(Of SPS.MA.ZGFunctionality.MAInfo)
		objData = SPS.MA.ZGFunctionality.ClsZGData.GetMAInfo(Me.m_EmployeeNumber)
		If objData.Count > 0 Then
			strMsg = String.Format(strMsg,
														m_Translate.GetSafeTranslationValue(objData.Item(0).MAAnrede),
														objData.Item(0).MAVorname,
														objData.Item(0).MANachname)
		End If
		Me.Text = strMsg




	End Sub

#End Region

	Private Sub LoadSummeryData()

		Dim data = CreateTable()
		If data Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Suche nach Guthaben!"))

			Return
		End If
		Dim summeryGridData = (From person In data
							   Select New GuthabenEachEmployeeData With
											  {.Bezeichnung = person.Bezeichnung,
											   .StdValue = person.StdValue,
											   .BetragValue = person.BetragValue
											  }).ToList()

		Dim listDataSource As BindingList(Of GuthabenEachEmployeeData) = New BindingList(Of GuthabenEachEmployeeData)

		For Each p In summeryGridData
			listDataSource.Add(p)
		Next

		grdSummery.DataSource = listDataSource


	End Sub

	Private Sub LoadDetailData()

		Dim tblDetail = GetMAGuthaben4ZGInDetail(Me.m_EmployeeNumber)
		If tblDetail Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Suche nach Guthaben-Detail!"))

			Return
		End If
		Dim detailGridData = (From person In tblDetail
							  Select New DetailGuthabenEachEmployeeData With
											  {.Bezeichnung = person.Bezeichnung,
											   .periode = person.periode,
											   .BetragValue = person.BetragValue,
											   .art = person.art
											  }).ToList()

		Dim listDetailDataSource As BindingList(Of DetailGuthabenEachEmployeeData) = New BindingList(Of DetailGuthabenEachEmployeeData)

		For Each p In detailGridData
			listDetailDataSource.Add(p)
		Next

		grdDetail.DataSource = listDetailDataSource

	End Sub

	Private Sub LoadFremdGuthaben()

		m_FremdGuthabenData = m_PayrollDatabaseAccess.LoadFremdGuthabenData(m_InitializationData.MDData.MDNr, m_EmployeeNumber)

		If (m_FremdGuthabenData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fremdguthaben-Daten konnten nicht geladen werden."))
		End If

		grdFremdGuthaben.DataSource = m_FremdGuthabenData

	End Sub

	''' <summary>
	''' Loads the mandant drop down data.
	''' </summary>
	Private Function LoadMandantDropDownData() As Boolean
		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()

		Return mandantData IsNot Nothing
	End Function

	''' <summary>
	''' Loads LA data.
	''' </summary>
	''' <param name="year">The year.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadLAData(ByVal year As Integer) As Boolean

		Dim m_LAList = m_PayrollDatabaseAccess.LoadLAData(year, Nothing, Nothing)

		'Dim suppressUIEventsState = m_SuppressUIEvents
		'm_SuppressUIEvents = True
		lueLAData.Properties.DataSource = m_LAList
		lueLAData.Properties.ForceInitialize()

		If Not lueLAData.EditValue Is Nothing AndAlso
			Not m_LAList.Any(Function(data) data.LANr = lueLAData.EditValue) Then
			lueLAData.EditValue = Nothing
		End If

		'm_SuppressUIEvents = suppressUIEventsState

		If m_LAList Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten konnten nicht geladen werden."))
		End If

		Return Not m_LAList Is Nothing

	End Function

	Private Sub LoadGuthabenData()
		Dim dbetrag As Decimal = 0
		Dim Anzahl As Decimal = 0
		Dim Betrag As Decimal = 0

		m_CurrenctGuthabenData.Feiertag = GetFeierGuthaben(Me.m_EmployeeNumber, 0)

		dbetrag = GetFeierGuthaben(Me.m_EmployeeNumber, 0)
		m_CurrenctGuthabenData.Ferien = GetFerGuthaben(Me.m_EmployeeNumber, 0)
		m_CurrenctGuthabenData.Lohn13 = Get13Guthaben(Me.m_EmployeeNumber, 0)

		m_CurrenctGuthabenData.FerienInd = GetFerGuthabenIndividuell(Me.m_EmployeeNumber)
		m_CurrenctGuthabenData.FeiertagInd = GetFeierGuthabenIndividuell(Me.m_EmployeeNumber)
		m_CurrenctGuthabenData.Lohn13Ind = Get13GuthabenIndividuell(Me.m_EmployeeNumber)

		m_CurrenctGuthabenData.Darlehen = GetDarlehenGuthaben(m_EmployeeNumber, m_InitializationData.MDData.MDNr)

		'dbetrag = GetAnzGStd(Me.m_EmployeeNumber, Anzahl, Betrag, m_InitializationData.MDData.MDNr)
		Dim data = m_ListingDatabaseAccess.LoadFlexibleWorkingHoursData(m_InitializationData.MDData.MDNr, m_EmployeeNumber)
		If data Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gleitzeit Daten konnten nicht geladen werden."))
		Else
			Anzahl = data.CreditHours
			Betrag = data.CreditAmount
		End If
		m_CurrenctGuthabenData.Gleitzeit.Anzahl = Anzahl
		m_CurrenctGuthabenData.Gleitzeit.Betrag = Betrag


		dbetrag = GetAnzNightStd(Me.m_EmployeeNumber, Anzahl, Betrag, m_InitializationData.MDData.MDNr)
		m_CurrenctGuthabenData.NachtZulage.Anzahl = Anzahl
		m_CurrenctGuthabenData.NachtZulage.Betrag = Betrag


	End Sub

	Private Function CreateTable() As IEnumerable(Of GuthabenEachEmployeeData)
		Dim result As List(Of GuthabenEachEmployeeData) = Nothing

		Dim tbl As New DataTable()
		Dim tbl1 As New DataTable()
		Dim cCutStd As Decimal = 0
		Dim cCutBetrag As Decimal = 0

		tbl.Columns.Add(m_Translate.GetSafeTranslationValue("Bezeichnung"), GetType(String))
		tbl.Columns.Add(m_Translate.GetSafeTranslationValue("Stunden"), GetType(String))
		tbl.Columns.Add(m_Translate.GetSafeTranslationValue("Betrag"), GetType(String)) 'GetType(Decimal))

		result = New List(Of GuthabenEachEmployeeData)
		Dim dbetrag As Decimal = GetFeierGuthaben(lueMandant.EditValue, Me.m_EmployeeNumber, 0)
		Trace.WriteLine(String.Format("Feiertagsentschädigung: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Feiertagsentschädigung")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Feiertagsentschädigung"), "", Format(dbetrag, "n")})
		End If


		dbetrag = GetFerGuthaben(lueMandant.EditValue, Me.m_EmployeeNumber, 0)
		Trace.WriteLine(String.Format("Ferienentschädigung: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Ferienentschädigung")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Ferienentschädigung"), "", Format(dbetrag, "n")})
		End If

		dbetrag = Get13Guthaben(lueMandant.EditValue, Me.m_EmployeeNumber, 0)
		Trace.WriteLine(String.Format("13. Monatslohn: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("13. Monatslohn")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("13. Monatslohn"), "", Format(dbetrag, "n")})
		End If

		'tbl.Rows.Add(New Object() {m_translate.GetSafeTranslationValue("Guthaben aus Individuelle Rückstellungen"), "", Format(0, "n")})
		dbetrag = GetFerGuthabenIndividuell(Me.m_EmployeeNumber)
		Trace.WriteLine(String.Format("Ferienentschädigung Individuell: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Ferienentschädigung Individuell")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Ferienentschädigung Individuell"), "", Format(dbetrag, "n")})
		End If

		dbetrag = GetFeierGuthabenIndividuell(Me.m_EmployeeNumber)
		Trace.WriteLine(String.Format("Feiertagsentschädigung Individuell: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Feiertagsentschädigung Individuell")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Feiertagsentschädigung Individuell"), "", ""})
		End If

		dbetrag = Get13GuthabenIndividuell(Me.m_EmployeeNumber)
		Trace.WriteLine(String.Format("13. Monatslohn Individuell: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("13. Monatslohn Individuell")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("13. Monatslohn Individuell"), "", Format(dbetrag, "n")})
		End If

		dbetrag = GetDarlehenGuthaben(Me.m_EmployeeNumber, lueMandant.EditValue)
		Trace.WriteLine(String.Format("Darlehensschuld: {0:n2}", dbetrag))
		If dbetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Darlehensschuld an Firma")
			data.StdValue = 0
			data.BetragValue = dbetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Darlehensschuld an Firma"), "", Format(dbetrag, "n")})
		End If

		cCutStd = 0
		cCutBetrag = 0
		'dbetrag = GetAnzGStd(Me.m_EmployeeNumber, cCutStd, cCutBetrag, lueMandant.EditValue)

		Dim flexibleWorkingData = m_ListingDatabaseAccess.LoadFlexibleWorkingHoursData(lueMandant.EditValue, m_EmployeeNumber)
		If flexibleWorkingData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gleitzeit Daten konnten nicht geladen werden."))

		Else
			cCutStd = flexibleWorkingData.CreditHours.GetValueOrDefault(0)
			cCutBetrag = flexibleWorkingData.CreditAmount.GetValueOrDefault(0)

			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Gleitzeit")
			data.StdValue = cCutStd
			data.BetragValue = cCutBetrag
			Trace.WriteLine(String.Format("Gleitzeit: {0:n2} | {1:n2} ", cCutStd, cCutBetrag))

			result.Add(data)
			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Gleitzeit"), If(cCutStd = 0, "", Format(cCutStd, "n")), Format(cCutBetrag, "n")})

		End If


		'If cCutStd + cCutBetrag <> 0 Then
		'	Dim data = New GuthabenEachEmployeeData()
		'	data.Bezeichnung = m_Translate.GetSafeTranslationValue("Gleitzeit")
		'	data.StdValue = cCutStd
		'	data.BetragValue = cCutBetrag

		'	result.Add(data)

		'	tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Gleitzeit"), If(cCutStd = 0, "", Format(cCutStd, "n")), Format(cCutBetrag, "n")})
		'End If

		cCutStd = 0
		cCutBetrag = 0
		dbetrag = GetAnzNightStd(Me.m_EmployeeNumber, cCutStd, cCutBetrag, lueMandant.EditValue)
		Trace.WriteLine(String.Format("Nachtzeitzulage: {0:n2} | {1:n2} ", cCutStd, cCutBetrag))
		If cCutStd + cCutBetrag <> 0 Then
			Dim data = New GuthabenEachEmployeeData()
			data.Bezeichnung = m_Translate.GetSafeTranslationValue("Nachtzeitzulage")
			data.StdValue = cCutStd
			data.BetragValue = cCutBetrag

			result.Add(data)

			tbl.Rows.Add(New Object() {m_Translate.GetSafeTranslationValue("Nachtzeitzulage"), If(cCutStd = 0, "", Format(cCutStd, "n")), Format(cCutBetrag, "n")})
		End If

		cCutStd = 0
		cCutBetrag = 0

		Dim objGuthaben As New SPS.MA.ZGFunctionality.ClsZGData
		Dim objData As New List(Of SPS.MA.ZGFunctionality.GuthabenData)
		objData = SPS.MA.ZGFunctionality.ClsZGData.GetMAGuthaben4ZG(Me.m_EmployeeNumber)

		If objData.Count > 0 Then
			Dim strMsg As String = m_Translate.GetSafeTranslationValue("Total Auszahlungen")
			If objData(0).ZGTotalBetrag <> 0 Then
				Dim data = New GuthabenEachEmployeeData()
				data.Bezeichnung = m_Translate.GetSafeTranslationValue("Total Auszahlungen")
				data.StdValue = 0
				data.BetragValue = RoundSPSNumber(objData(0).ZGTotalBetrag, 2)

				result.Add(data)

				tbl.Rows.Add(New Object() {strMsg, "", Format(RoundSPSNumber(objData(0).ZGTotalBetrag, 2), "n")})
			End If


			strMsg = m_Translate.GetSafeTranslationValue("Mögliche Auszahlung 1 (RP.-Stunden - Auszahlungen) {0} %")
			If objData(0).RPTotalStd <> 0 Then
				Dim data = New GuthabenEachEmployeeData()
				data.Bezeichnung = String.Format(m_Translate.GetSafeTranslationValue("Mögliche Auszahlung 1 (RP.-Stunden - Auszahlungen) {0} %"), RoundSPSNumber(objData(0).QSTBetrag, 1))
				data.StdValue = 0
				data.BetragValue = RoundSPSNumber(objData(0).RPTotalStd, 2)

				result.Add(data)

				tbl.Rows.Add(New Object() {String.Format(strMsg, RoundSPSNumber(objData(0).QSTBetrag, 1)), "", Format(RoundSPSNumber(objData(0).RPTotalStd, 2), "n")})
			End If

			strMsg = m_Translate.GetSafeTranslationValue("Mögliche Auszahlung 2 (Alle RP.-Daten - Auszahlungen) {0} %")
			If objData(0).RPTotalBetrag <> 0 Then
				Dim data = New GuthabenEachEmployeeData()
				data.Bezeichnung = String.Format(m_Translate.GetSafeTranslationValue("Mögliche Auszahlung 2 (Alle RP.-Daten - Auszahlungen) {0} %"), RoundSPSNumber(objData(0).QSTBetrag, 1))
				data.StdValue = 0
				data.BetragValue = RoundSPSNumber(objData(0).RPTotalBetrag, 2)

				result.Add(data)

				tbl.Rows.Add(New Object() {String.Format(strMsg, RoundSPSNumber(objData(0).QSTBetrag, 1)), "", Format(RoundSPSNumber(objData(0).RPTotalBetrag, 2), "n")})
			End If

			dbetrag = GetDarlehenGuthaben(Me.m_EmployeeNumber, lueMandant.EditValue)
			strMsg = m_Translate.GetSafeTranslationValue("Total Minuslöhne (Aktuellen Monat)")
			If objData(0).LMNegativLohnBetrag <> 0 Then
				Dim data = New GuthabenEachEmployeeData()
				data.Bezeichnung = m_Translate.GetSafeTranslationValue("Total Minuslöhne (Aktuellen Monat)")
				data.StdValue = 0
				data.BetragValue = RoundSPSNumber(objData(0).LMNegativLohnBetrag, 2)

				result.Add(data)

				tbl.Rows.Add(New Object() {strMsg, "", Format(RoundSPSNumber(objData(0).LMNegativLohnBetrag, 2), "n")})
			End If

		End If

		Return result

		'Return tbl
	End Function


	Private Function GetMAGuthaben4ZGInDetail(ByVal iMANr As Integer) As IEnumerable(Of DetailGuthabenEachEmployeeData)
		Dim result As List(Of DetailGuthabenEachEmployeeData) = Nothing

		'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		'Dim _ClsReg As New SPProgUtility.ClsDivReg
		'Dim strNegativLohn As String = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Vorschusszahlungen", "LANr_4_Allowedpayout")
		Dim strNegativLohn As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/LANr_4_Allowedpayout", FORM_XML_MAIN_KEY))
		If strNegativLohn Is Nothing Then strNegativLohn = String.Empty

		Dim sql As String = "[Get MAGuthaben Data in ZG In Detail]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("MANr", iMANr))
		listOfParams.Add(New SqlClient.SqlParameter("Monat", Now.Month))
		listOfParams.Add(New SqlClient.SqlParameter("Jahr", Now.Year))
		listOfParams.Add(New SqlClient.SqlParameter("LANrList", strNegativLohn))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		If (Not reader Is Nothing) Then

			result = New List(Of DetailGuthabenEachEmployeeData)

			While reader.Read()
				Dim overviewData As New DetailGuthabenEachEmployeeData

				overviewData.Bezeichnung = m_utility.SafeGetString(reader, "Bezeichnung")
				overviewData.periode = m_utility.SafeGetString(reader, "Periode")
				overviewData.BetragValue = m_utility.SafeGetDecimal(reader, "Betrag", Nothing)

				overviewData.art = m_utility.SafeGetString(reader, "Art")


				result.Add(overviewData)

			End While

		End If

		Return result

		'Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
		'Dim strQuery As String = "[Get MAGuthaben Data in ZG In Detail]"
		'Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		'cmd.CommandType = CommandType.StoredProcedure


		'Dim objAdapter As New SqlDataAdapter
		'Dim param As System.Data.SqlClient.SqlParameter

		'param = cmd.Parameters.AddWithValue("@MANr", iMANr)
		'param = cmd.Parameters.AddWithValue("@Monat", Month(Now))
		'param = cmd.Parameters.AddWithValue("@Jahr", Year(Now))
		'param = cmd.Parameters.AddWithValue("@LANrList", strNegativLohn)

		'Try
		'	objAdapter.SelectCommand = cmd
		'	objAdapter.Fill(ds, "ZGGuthaben")


		'Catch ex As Exception
		'	m_Logger.LogError(ex.ToString)
		'	Return ds.Tables(0)

		'Finally
		'	Conn.Close()
		'	Conn.Dispose()

		'End Try

		'Return ds.Tables(0)
	End Function



	''' <summary>
	''' Handles change of mandant.
	''' </summary>
	Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueMandant.EditValue Is Nothing Then

			Dim mandantData = CType(lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)

			'm_SelectedMandantData = mandantData
			m_InitializationData = CreateInitialData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)

			'm_currentLOLData = Nothing
			'grdFremdGuthaben.DataSource = Nothing

			LoadSummeryData()
			LoadDetailData()
			LoadFremdGuthaben()

		Else

			lueLAData.EditValue = Nothing
			lueLAData.Properties.DataSource = Nothing

		End If

	End Sub



	Private Sub OngvSummery_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvSummery.CustomColumnDisplayText

		If e.Column.FieldName = "StdValue" Or e.Column.FieldName = "BetragValue" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	Private Sub OngvDetail_CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs) Handles gvDetail.CustomColumnDisplayText

		If e.Column.FieldName = "BetragValue" Then
			If e.Value = 0 Then e.DisplayText = String.Empty
		End If

	End Sub

	''' <summary>
	''' Handles focus change of fremd guthaben row.
	''' </summary>
	Private Sub OngvFremdGuthaben_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvFremdGuthaben.FocusedRowChanged

		m_currentLOLData = SelectedFremdGuthaben
		PresentMonthlySalaryDetailData(m_currentLOLData)

	End Sub

	Sub Ongv_FremdGuthabenRowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		m_currentLOLData = SelectedFremdGuthaben
		PresentMonthlySalaryDetailData(m_currentLOLData)

	End Sub

	Private Sub OngvFremdGuthaben_KeyUp(sender As Object, e As KeyEventArgs) Handles gvFremdGuthaben.KeyUp
		Dim result As DeleteLOLForCorrectionAssignmentResult

		m_currentLOLData = SelectedFremdGuthaben
		If Not m_currentLOLData Is Nothing Then
			If e.KeyCode = Keys.Delete Then
				If (m_UtilityUi.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																										m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
					Return
				End If

				result = m_PayrollDatabaseAccess.DeleteAssignedFremdGuthaben(m_currentLOLData.ID, "LOL", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)
				Dim msg As String
				Select Case result
					Case DeleteLOLForCorrectionAssignmentResult.ResultCanNotDeleteBecauseMonthIsClosed
						msg = "Der Monat ist bereits abgeschlossen."
					Case DeleteLOLForCorrectionAssignmentResult.ResultDeleteOk
						msg = "Der ausgewählte Datensatz wurde erfolgreich gelöscht."

					Case Else
						msg = "Sonstige undefinierte Fehler."
				End Select

				If Not result = DeleteLOLForCorrectionAssignmentResult.ResultDeleteOk Then
					m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gelöscht werden.{0}{1}"), vbNewLine, msg))
					Return

				Else
					m_UtilityUi.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)

				End If

				LoadFremdGuthaben()

				Dim listDataSource As List(Of LOLDataFoRepeatLA4LOBack) = grdFremdGuthaben.DataSource

				If listDataSource Is Nothing Then
					Return
				End If
				If listDataSource.Count > 0 Then FocusGuthaben(listDataSource(0).ID)

				LoadSummeryData()
				LoadDetailData()

			End If
		End If

	End Sub

	Private Function PresentMonthlySalaryDetailData(ByVal data As LOLDataFoRepeatLA4LOBack) As Boolean
		Dim result As Boolean = True

		If data Is Nothing Then
			lueLAData.EditValue = Nothing
			txtFerienAmount.EditValue = Nothing

			'm_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Selektierte Daten konnten nicht geladen werden."))
			result = False

		Else
			lueLAData.EditValue = data.LANr
			txtFerienAmount.EditValue = data.m_Btr

		End If


		Return result

	End Function

	Private Sub OnbtnNewFremdGuthaben_Click(sender As System.Object, e As System.EventArgs) Handles btnNewFremdGuthaben.Click

		m_currentLOLData = Nothing

		txtFerienAmount.EditValue = Nothing
		txtFerienAmount.EditValue = Nothing

	End Sub

	Private Sub OnbtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
		SaveLOLData()
	End Sub


	Private Function SaveLOLData() As Boolean
		Dim success As Boolean = True

		Dim data = New LOLDataFoRepeatLA4LOBack
		If m_currentLOLData Is Nothing Then

			data.LONr = -1
			data.MANr = m_EmployeeNumber
			data.MDNr = m_InitializationData.MDData.MDNr
			data.LANr = lueLAData.EditValue
			data.RPText = lueLAData.Text

			data.LP = 1
			data.Jahr = 1990
			data.DestESNr = -1
			data.DestRPNr = -1
			data.DestKDNr = -1
			data.m_Anz = 1
			data.m_Ans = 100
			data.m_Bas = Val(txtFerienAmount.EditValue)
			data.m_Btr = Val(txtFerienAmount.EditValue)
			data.DateOfLO = CDate("01.01.1990")

		Else
			data = m_currentLOLData
			data.LANr = lueLAData.EditValue
			data.RPText = lueLAData.Text
			data.m_Btr = Val(txtFerienAmount.EditValue)
			data.m_Bas = Val(txtFerienAmount.EditValue)

		End If

		If m_currentLOLData Is Nothing Then
			success = success AndAlso m_PayrollDatabaseAccess.AddFremdGuthabenIntoLOL(data)
		Else
			success = success AndAlso m_PayrollDatabaseAccess.UpdateFremdGuthabenIntoLOL(data)
		End If

		If Not success Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht gespeichert werden."))

			Return False
		Else
			m_UtilityUi.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

		End If

		LoadFremdGuthaben()
		FocusGuthaben(data.ID)

		LoadSummeryData()
		LoadDetailData()


		Return success

	End Function

	Private Sub FocusGuthaben(ByVal recordNumber As Integer)

		Dim listDataSource As List(Of LOLDataFoRepeatLA4LOBack) = grdFremdGuthaben.DataSource

		If listDataSource Is Nothing Then
			Return
		End If

		Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.ID = recordNumber).FirstOrDefault()

		If Not monthlySalaryViewData Is Nothing Then
			Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
			Dim rowHandle = gvFremdGuthaben.GetRowHandle(sourceIndex)
			gvFremdGuthaben.FocusedRowHandle = rowHandle
		End If



	End Sub

	Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
		Me.Dispose()
	End Sub


	'Private Sub gridView1_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles gvSummery.CustomColumnDisplayText
	'	Dim ciCH As CultureInfo = New CultureInfo("de-ch")

	'	Dim View As ColumnView = sender
	'	If e.Column.FieldName = "Betrag" Then
	'		'Dim currencyType As Integer = View.GetRowCellValue(e.RowHandle, View.Columns("decimalType"))
	'		Dim currencyType As Integer = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("decimalType"))

	'		'Dim dBetrag As Decimal = View.GetRowCellValue(e.RowHandle, View.Columns("Betrag"))
	'		Dim dBetrag As Decimal = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("Betrag"))
	'		If dBetrag <> 0 Then
	'			' Conditional formatting: 
	'			Select Case currencyType
	'				Case 0
	'					e.DisplayText = String.Format(ciCH, "{0:c2}", dBetrag) ' "{0:c}", dBetrag) "#.00;[#.0];Zero"
	'					e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
	'					'Case 1 : e.DisplayText = String.Format(ciEUR, "{0:c}", price)
	'			End Select
	'		Else
	'			e.DisplayText = String.Empty

	'		End If

	'	End If

	'End Sub

	'Private Sub gridView2_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles gvDetail.CustomColumnDisplayText
	'	Dim ciCH As CultureInfo = New CultureInfo("de-ch")

	'	Dim View As ColumnView = sender
	'	If e.Column.FieldName = "Betrag" Then
	'		'Dim currencyType As Integer = View.GetRowCellValue(e.RowHandle, View.Columns("decimalType"))
	'		'Dim dBetrag As Decimal = View.GetRowCellValue(e.RowHandle, View.Columns("Betrag"))
	'		Dim currencyType As Integer = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("decimalType"))
	'		Dim dBetrag As Decimal = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("Betrag"))
	'		If dBetrag <> 0 Then
	'			' Conditional formatting: 
	'			Select Case currencyType
	'				Case 0
	'					e.DisplayText = String.Format(ciCH, "{0:c2}", dBetrag)
	'					e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

	'			End Select
	'		Else
	'			e.DisplayText = String.Empty
	'		End If
	'	ElseIf e.Column.FieldName = "Art" Then
	'		'Dim strValue As String = View.GetRowCellValue(e.RowHandle, View.Columns("Art"))
	'		Dim strValue As String = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("Art"))
	'		'e.DisplayText = String.Format(ciCH, "{0:c2}", strValue)
	'		e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

	'	ElseIf e.Column.FieldName = "Periode" Then
	'		'Dim strValue As String = View.GetRowCellValue(e.RowHandle, View.Columns("Periode"))
	'		Dim strValue As String = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("Periode"))
	'		e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

	'	End If

	'End Sub

	Private Sub frmMAGuthaben_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.Escape Then
			Me.Dispose()
		ElseIf e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next

			strMsg = String.Format(strMsg, vbNewLine, _
														 GetExecutingAssembly().FullName, _
														 GetExecutingAssembly().Location, _
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If

	End Sub

	Function RoundSPSNumber(ByVal value As Decimal, ByVal Digit As Short) As Decimal
		Dim dRValue As Decimal = value
		If Digit = Decimal.Zero Then Digit = 2

		dRValue = Format(CLng(value / 0.05) * 0.05, "0." & (New String("0", Digit)))

		Return dRValue
	End Function



#Region "helpers"

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#End Region



End Class

Public Class MyObject

	Private Property Column_1 As String
	Private Property Column_2 As String

	Public Sub New(ByVal Col1 As String, ByVal Col2 As String)
		Column_1 = Col1
		Column_2 = Col2
	End Sub


End Class


Public Class GuthabenData
	Public Property Ferien As Decimal?
	Public Property Feiertag As Decimal?
	Public Property Lohn13 As Decimal?
	Public Property FerienInd As Decimal?
	Public Property FeiertagInd As Decimal?
	Public Property Lohn13Ind As Decimal?
	Public Property Darlehen As Decimal?

	Public Property NachtZulage As CountAmountData
	Public Property Gleitzeit As CountAmountData

End Class


Public Class CountAmountData
	Public Property Anzahl As Decimal?
	Public Property Betrag As Decimal?

End Class

Public Class GuthabenEachEmployeeData
	Public Property Bezeichnung As String
	Public Property StdValue As Decimal
	Public Property BetragValue As Decimal

End Class

Public Class DetailGuthabenEachEmployeeData
	Public Property Bezeichnung As String
	Public Property periode As String
	Public Property BetragValue As Decimal
	Public Property art As String

End Class
