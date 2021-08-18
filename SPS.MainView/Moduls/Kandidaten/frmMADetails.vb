
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.UI.UtilityUI
Imports SPS.MainView.DataBaseAccess

Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Views.Base
Imports System.IO
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.ModulView
Imports SP.DatabaseAccess.ModulView.DataObjects
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common

Public Class frmMADetails

#Region "private fields"

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_connectionString As String

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility


	''' <summary>
	''' The modulview database access.
	''' </summary>
	Protected m_ModulViewDatabaseAccess As IModulViewDatabaseAccess

	Private _ClsSetting As New ClsMASetting
	Private Property Modul2Open As String

	Private Property MetroForeColor As System.Drawing.Color
	Private Property MetroBorderColor As System.Drawing.Color
	Private m_translate As TranslateValues

	Private m_GVESSettingfilename As String
	Private m_GVProposeSettingfilename As String
	Private m_GVContactSettingfilename As String
	Private m_GVZGSettingfilename As String
	Private m_GVSalarySettingfilename As String
	Private m_GVReportSettingfilename As String

	Private m_GVESSettingfilenameWithEmployee As String
	Private m_GVProposeSettingfilenameWithEmployee As String
	Private m_GVContactSettingfilenameWithEmployee As String
	Private m_GVZGSettingfilenameWithEmployee As String
	Private m_GVSalarySettingfilenameWithEmployee As String
	Private m_GVReportSettingfilenameWithEmployee As String

	Private Property SearchWithEmployeeNumber As Boolean?

	Private m_Handle As IOverlaySplashScreenHandle
	Protected m_SuppressUIEvents As Boolean = False


#End Region

	Public Sub New(ByVal _setting As ClsMASetting, ByVal _m2Open As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.ProsonalizedData, ModulConstants.MDData, ModulConstants.UserData)
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility

		Me._ClsSetting = _setting
		Me.Modul2Open = _m2Open
		Me._ClsSetting.OpenDetailModul = _m2Open

		m_translate = New TranslateValues

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Default
		WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		Try
			m_connectionString = ModulConstants.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, ModulConstants.UserData.UserLanguage)


			Dim strModulName As String = Me.Modul2Open.ToLower

			m_GVESSettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVZGSettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVSalarySettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVReportSettingfilename = String.Format("{0}Employee\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

			m_GVESSettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVZGSettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVSalarySettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVReportSettingfilenameWithEmployee = String.Format("{0}Employee\Details\{1}_WithEmployee{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		Reset()

		AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvDetail.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvDetail.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvDetail.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler Me.gvDetail.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub


	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		m_SuppressUIEvents = True

		'result = result AndAlso LoadMandantenDropDown()
		result = result AndAlso LoadYearDropDownData()
		result = result AndAlso LoadMonthDropDownData()

		'lueMandant.EditValue = ModulConstants.MDData.MDNr
		lueYear.EditValue = Now.Year
		lueMonth.EditValue = Nothing

		m_SuppressUIEvents = False

		Return result
	End Function


	Private Sub Reset()

		m_SuppressUIEvents = True

		aceAssignedEmployee.Text = String.Format("Für {0}?", _ClsSetting.EmployeeName)
		tgsAssignedEmployee.EditValue = Me._ClsSetting.Data4SelectedMA
		tgsAssignedEmployee.Properties.EditValueChangedFiringMode = EditValueChangedFiringMode.Buffered
		tgsAssignedEmployee.Properties.EditValueChangedDelay = 200

		pnlContactFilter.BorderStyle = BorderStyles.NoBorder
		txtPlainText.EditValue = Nothing
		txtPlainText.Properties.EditValueChangedFiringMode = EditValueChangedFiringMode.Buffered
		txtPlainText.Properties.EditValueChangedDelay = 500

		ResetYearDropDown()
		ResetMonthDropDown()

		m_SuppressUIEvents = False

	End Sub

	'Private Sub ResetMandantDropDown()

	'	lueMandant.Properties.Items.Clear()
	'	lueMandant.Properties.DisplayMember = "MandantName1"
	'	lueMandant.Properties.ValueMember = "MandantNumber"

	'End Sub

	Private Sub ResetYearDropDown()

		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub

	''' <summary>
	''' Resets the month drop down.
	''' </summary>
	Private Sub ResetMonthDropDown()

		lueMonth.Properties.DisplayMember = "Value"
		lueMonth.Properties.ValueMember = "Value"
		lueMonth.Properties.ShowHeader = False

		lueMonth.Properties.Columns.Clear()
		lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_translate.GetSafeTranslationValue("Value")})

		lueMonth.Properties.ShowFooter = False
		lueMonth.Properties.DropDownRows = 12
		lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMonth.Properties.SearchMode = SearchMode.AutoComplete
		lueMonth.Properties.AutoSearchColumnIndex = 0

		lueMonth.Properties.NullText = String.Empty
		lueMonth.EditValue = Nothing
	End Sub


	'Private Function LoadMandantenDropDown() As Boolean
	'	Dim result As Boolean = True
	'	'Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

	'	Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData
	'	lueMandant.Properties.DataSource = Data

	'	Return result
	'End Function

	Private Function LoadYearDropDownData() As Boolean

		Dim success As Boolean = True

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not ModulConstants.MDData Is Nothing Then

			Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(ModulConstants.MDData.MDNr)

			If (yearData Is Nothing) Then
				success = False
				m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
			End If

			If Not yearData Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For Each yearValue In yearData
					wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
				Next

			End If

		End If

		lueYear.EditValue = Nothing
		lueYear.Properties.DataSource = wrappedValues

		lueYear.Properties.ForceInitialize()

		Return success
	End Function

	''' <summary>
	''' Loads the month drop down data.
	''' </summary>
	Private Function LoadMonthDropDownData() As Boolean

		Dim success As Boolean = True

		Dim year = lueYear.EditValue

		Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

		If Not m_InitializationData Is Nothing Then ' and Not year Is Nothing Then

			wrappedValues = New List(Of IntegerValueViewWrapper)
			For i As Integer = 1 To 12
				wrappedValues.Add(New IntegerValueViewWrapper With {.Value = i})
			Next

		End If

		lueMonth.EditValue = Nothing
		lueMonth.Properties.DataSource = wrappedValues
		lueMonth.Properties.ForceInitialize()

		Return success
	End Function

	'Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)

	'	If m_SuppressUIEvents OrElse lueYear.EditValue Is Nothing Then
	'		Return
	'	End If

	'	ResetContactDetailGrid()
	'	LoadCustomerContactDetailList()

	'End Sub

	Private Sub OnlueYear_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueYear.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		ResetContactDetailGrid()
		LoadEmployeeContactDetailList()

	End Sub

	Private Sub OnlueMonth_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMonth.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		ResetContactDetailGrid()
		LoadEmployeeContactDetailList()

	End Sub

	Private Sub txtPlainText_EditValueChanged(sender As Object, e As EventArgs) Handles txtPlainText.EditValueChanged
		ResetContactDetailGrid()
		LoadEmployeeContactDetailList()
	End Sub

	Private Sub txtPlainText_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtPlainText.ButtonClick

		ResetContactDetailGrid()
		LoadEmployeeContactDetailList()

	End Sub

	Private Sub txtPlainText_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPlainText.KeyUp
		If e.KeyCode = Keys.Return Then
			ResetContactDetailGrid()
			LoadEmployeeContactDetailList()
		End If
	End Sub


	Private Sub frmMADetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.SETTING_CANDIDAT_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.SETTING_CANDIDAT_WIDTH = Me.Width
				My.Settings.SETTING_CANDIDAT_HEIGHT = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub frmMADetails_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		OngvColumnPositionChanged(New Object, New System.EventArgs)

	End Sub

	Private Sub frmDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTitle As String = String.Empty

		Try
			If My.Settings.SETTING_CANDIDAT_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_CANDIDAT_HEIGHT)
			If My.Settings.SETTING_CANDIDAT_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_CANDIDAT_WIDTH)
			If My.Settings.SETTING_CANDIDAT_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_CANDIDAT_LOCATION.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

		End Try

		Me.sccMain.Dock = DockStyle.Fill
		Me.bsiRecCount.Caption = TranslateText("Bereit")

		Try
			pnlContactFilter.Visible = False

			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Me.gvDetail.OptionsView.ShowIndicator = False
			Select Case Me.Modul2Open.ToLower
				Case "MAES".ToLower
					strTitle = "Anzeige der Einsätze"
					ResetESDetailGrid()
					LoadEmployeeESDetailList()

				Case "MALO".ToLower
					strTitle = "Anzeige der Lohnabrechnungen"
					ResetSalaryDetailGrid()
					LoadEmployeeSalaryDetailList()

				Case "MARP".ToLower
					strTitle = "Anzeige der Rapporte"
					ResetReportDetailGrid()
					LoadEmployeeReportDetailList()

				Case "MAZG".ToLower
					strTitle = "Anzeige der Auszahlungen"
					ResetZGDetailGrid()
					LoadEmployeeZGDetailList()

				Case "MAPropose".ToLower
					strTitle = "Anzeige der Vorschläge"
					ResetProposeDetailGrid()
					LoadEmployeeProposeDetailList()

				Case "macontact".ToLower
					pnlContactFilter.Visible = True
					strTitle = "Anzeige der Kontakte"
					ResetContactDetailGrid()
					LoadEmployeeContactDetailList()

				Case Else

			End Select
			Me.Text = String.Format(TranslateText(strTitle))
			Me.rlblDetailHeader.Text = String.Format("<b>{0}</b>", TranslateText(strTitle))
			Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

		Catch ex As Exception


		Finally
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Private Sub OntgsAssignedEmployee_EditValueChanged(sender As Object, e As EventArgs) Handles tgsAssignedEmployee.EditValueChanged

		If m_SuppressUIEvents Then Return

		grdDetailrec.BeginUpdate()
		Me.gvDetail.Columns.Clear()
		Me.grdDetailrec.DataSource = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()


			'Me._ClsSetting.Data4SelectedMA = tgsAssignedEmployee.EditValue
			'SearchWithEmployeeNumber = tgsAssignedEmployee.EditValue

			Select Case Me.Modul2Open.ToLower
				Case "maes"
					ResetESDetailGrid()
					LoadEmployeeESDetailList()

				Case "mapropose"
					ResetProposeDetailGrid()
					LoadEmployeeProposeDetailList()

				Case "macontact"
					ResetContactDetailGrid()
					LoadEmployeeContactDetailList()

				Case "mazg"
					ResetZGDetailGrid()
					LoadEmployeeZGDetailList()

				Case "marp"
					ResetReportDetailGrid()
					LoadEmployeeReportDetailList()

				Case "malo"
					ResetSalaryDetailGrid()
					LoadEmployeeSalaryDetailList()


				Case Else

			End Select
			Me.grdDetailrec.EndUpdate()
			Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

		Catch ex As Exception


		Finally
			CloseProgressPanel(m_Handle)

		End Try

	End Sub


#Region "Details for employee ES"

	Sub ResetESDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "esnr"
			columnmodulname.FieldName = "esnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvDetail.Columns.Add(columnMANr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvDetail.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvDetail.Columns.Add(columnZHDNr)

			Dim columnPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPeriode.Caption = m_translate.GetSafeTranslationValue("Periode")
			columnPeriode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPeriode.Name = "periode"
			columnPeriode.FieldName = "periode"
			columnPeriode.Visible = False
			gvDetail.Columns.Add(columnPeriode)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.Caption = m_translate.GetSafeTranslationValue("Als")
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Name = "esals"
			columnESAls.FieldName = "esals"
			columnESAls.Visible = True
			gvDetail.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTarif.Caption = m_translate.GetSafeTranslationValue("Tarif")
			columnTarif.Name = "tarif"
			columnTarif.FieldName = "tarif"
			columnTarif.Visible = False
			columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTarif.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnTarif)

			Dim columnStundenlohn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStundenlohn.Caption = m_translate.GetSafeTranslationValue("Stundenlohn")
			columnStundenlohn.Name = "stundenlohn"
			columnStundenlohn.FieldName = "stundenlohn"
			columnStundenlohn.Visible = False
			columnStundenlohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnStundenlohn.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnStundenlohn)

			Dim columnMargeMitBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeMitBVG.Caption = m_translate.GetSafeTranslationValue("Marge mit BVG")
			columnMargeMitBVG.Name = "margemitbvg"
			columnMargeMitBVG.FieldName = "margemitbvg"
			columnMargeMitBVG.Visible = False
			columnMargeMitBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMargeMitBVG.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnMargeMitBVG)

			Dim columnMargeOhneBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeOhneBVG.Caption = m_translate.GetSafeTranslationValue("Marge ohne BVG")
			columnMargeOhneBVG.Name = "margeohnebvg"
			columnMargeOhneBVG.FieldName = "margeohnebvg"
			columnMargeOhneBVG.Visible = False
			columnMargeOhneBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMargeOhneBVG.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnMargeOhneBVG)

			Dim columnActivES As New DevExpress.XtraGrid.Columns.GridColumn()
			columnActivES.Caption = m_translate.GetSafeTranslationValue("Aktiv?")
			columnActivES.Name = "actives"
			columnActivES.FieldName = "actives"
			columnActivES.Visible = True
			gvDetail.Columns.Add(columnActivES)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeESDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedEmployeeESDetailData) = New BindingList(Of FoundedEmployeeESDetailData)
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedEmployee.EditValue Then
				employeeNumber = Me._ClsSetting.SelectedMANr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbEmployeeESDataForDetails(employeeNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedEmployeeESDetailData With
														   {.mdnr = person.mdnr,
															  .esnr = person.esnr,
															  .manr = person.manr,
															  .kdnr = person.kdnr,
															  .zhdnr = person.zhdnr,
															  .periode = person.periode,
															  .employeename = person.employeename,
															  .customername = person.customername,
															  .esals = person.esals,
															  .tarif = person.tarif,
															  .stundenlohn = person.stundenlohn,
															  .margemitbvg = person.margemitbvg,
															  .margeohnebvg = person.margeohnebvg,
															  .actives = person.actives,
															  .createdfrom = person.createdfrom,
															  .createdon = person.createdon,
															  .zfiliale = person.zfiliale
														   }).ToList()


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for employee Propose"

	Sub ResetProposeDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "pnr"
			columnmodulname.FieldName = "pnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvDetail.Columns.Add(columnMANr)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvDetail.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "zhdnr"
			columnZHDNr.FieldName = "zhdnr"
			columnZHDNr.Visible = False
			gvDetail.Columns.Add(columnZHDNr)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.Caption = m_translate.GetSafeTranslationValue("Bezeichnung")
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvDetail.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.Name = "zhdname"
			columnZHDName.FieldName = "zhdname"
			columnZHDName.Visible = True
			gvDetail.Columns.Add(columnZHDName)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "advisor"
			columnAdvisor.FieldName = "advisor"
			columnAdvisor.Visible = False
			gvDetail.Columns.Add(columnAdvisor)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.Caption = m_translate.GetSafeTranslationValue("Art")
			columnPArt.Name = "p_art"
			columnPArt.FieldName = "p_art"
			columnPArt.Visible = False
			gvDetail.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.Caption = m_translate.GetSafeTranslationValue("Status")
			columnPState.Name = "p_state"
			columnPState.FieldName = "p_state"
			columnPState.Visible = False
			gvDetail.Columns.Add(columnPState)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeProposeDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedEmployeeProposalDetailData) = New BindingList(Of FoundedEmployeeProposalDetailData)
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedEmployee.EditValue Then
				employeeNumber = Me._ClsSetting.SelectedMANr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbEmployeeProposalDataForDetails(employeeNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Vorschlag-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedEmployeeProposalDetailData With
														   {.mdnr = person.mdnr,
															  .pnr = person.pnr,
															  .manr = person.manr,
															  .kdnr = person.kdnr,
															  .zhdnr = person.zhdnr,
															  .employeename = person.employeename,
															  .customername = person.customername,
															  .bezeichnung = person.bezeichnung,
															  .zhdname = person.zhdname,
															  .p_art = person.p_art,
															  .p_state = person.p_state,
															  .advisor = person.advisor,
															  .createdfrom = person.createdfrom,
															  .createdon = person.createdon,
															  .zfiliale = person.zfiliale
														   }).ToList()


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for employee contact"

	Sub ResetContactDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "contactnr"
			columnmodulname.FieldName = "contactnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = True
			gvDetail.Columns.Add(columnKDNr)

			Dim columnDatum As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDatum.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnDatum.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDatum.Name = "datum"
			columnDatum.FieldName = "datum"
			columnDatum.Visible = True
			gvDetail.Columns.Add(columnDatum)

			Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonat.Caption = m_translate.GetSafeTranslationValue("Monat")
			columnMonat.Name = "monat"
			columnMonat.FieldName = "monat"
			columnMonat.Visible = False
			gvDetail.Columns.Add(columnMonat)

			Dim columnJahr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJahr.Caption = m_translate.GetSafeTranslationValue("Jahr")
			columnJahr.Name = "jahr"
			columnJahr.FieldName = "jahr"
			columnJahr.Visible = False
			gvDetail.Columns.Add(columnJahr)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnArt.Caption = m_translate.GetSafeTranslationValue("Art")
			columnArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnArt.Name = "art"
			columnArt.FieldName = "art"
			columnArt.Visible = True
			gvDetail.Columns.Add(columnArt)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Bezeichnung")
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Name = "bezeichnung"
			columnBezeichnung.FieldName = "bezeichnung"
			columnBezeichnung.Visible = True
			gvDetail.Columns.Add(columnBezeichnung)

			Dim columnBeschreibung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBeschreibung.Caption = m_translate.GetSafeTranslationValue("Beschreibung")
			columnBeschreibung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBeschreibung.Name = "beschreibung"
			columnBeschreibung.FieldName = "beschreibung"
			columnBeschreibung.Visible = True
			gvDetail.Columns.Add(columnBeschreibung)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeContactDetailList() As Boolean
		Dim listDataSource As BindingList(Of ModulViewEmployeeContactData) = New BindingList(Of ModulViewEmployeeContactData)
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedEmployee.EditValue Then
				employeeNumber = Me._ClsSetting.SelectedMANr
			End If
			Dim listOfEmployees = m_ModulViewDatabaseAccess.LoadAssignedEmployeeContactData(ModulConstants.MDData.MDNr, employeeNumber, lueYear.EditValue, lueMonth.EditValue, Nothing, Nothing, txtPlainText.EditValue)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Kontakt-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New ModulViewEmployeeContactData With
														   {.contactnr = person.contactnr,
															  .manr = person.manr,
															  .kdnr = person.kdnr,
															  .monat = person.monat,
															  .jahr = person.jahr,
															  .employeename = person.employeename,
															  .customername = person.customername,
															  .bezeichnung = person.bezeichnung,
															  .beschreibung = person.beschreibung,
															  .datum = person.datum,
															  .art = person.art,
															  .createdon = person.createdon,
															  .createdfrom = person.createdfrom
														   }).ToList()


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region



#Region "Details for employee ZG"

	Sub ResetZGDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zgnr"
			columnmodulname.FieldName = "zgnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "manr"
			columnMANr.FieldName = "manr"
			columnMANr.Visible = False
			gvDetail.Columns.Add(columnMANr)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.Caption = m_translate.GetSafeTranslationValue("Lohn-Nr.")
			columnLONr.Name = "lonr"
			columnLONr.FieldName = "lonr"
			columnLONr.Visible = False
			gvDetail.Columns.Add(columnLONr)

			Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPNr.Caption = m_translate.GetSafeTranslationValue("Rapport-Nr.")
			columnRPNr.Name = "rpnr"
			columnRPNr.FieldName = "rpnr"
			columnRPNr.Visible = False
			gvDetail.Columns.Add(columnRPNr)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_translate.GetSafeTranslationValue("Auszahlungsart")
			columnLAName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLAName.Name = "laname"
			columnLAName.FieldName = "laname"
			columnLAName.Visible = True
			gvDetail.Columns.Add(columnLAName)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "betrag"
			columnBetrag.FieldName = "betrag"
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnBetrag)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnZGPeriode As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZGPeriode.Caption = m_translate.GetSafeTranslationValue("Zeitraum")
			columnZGPeriode.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZGPeriode.Name = "zgperiode"
			columnZGPeriode.FieldName = "zgperiode"
			columnZGPeriode.Visible = False
			gvDetail.Columns.Add(columnZGPeriode)

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "lanr"
			columnLANr.FieldName = "lanr"
			columnLANr.Visible = False
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "f3"
			gvDetail.Columns.Add(columnLANr)

			Dim columnAusDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAusDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnAusDat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAusDat.Name = "aus_dat"
			columnAusDat.FieldName = "aus_dat"
			columnAusDat.Visible = False
			gvDetail.Columns.Add(columnAusDat)

			Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonat.Caption = m_translate.GetSafeTranslationValue("Monat")
			columnMonat.Name = "monat"
			columnMonat.FieldName = "monat"
			columnMonat.Visible = False
			gvDetail.Columns.Add(columnMonat)

			Dim columnJahr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJahr.Caption = m_translate.GetSafeTranslationValue("Jahr")
			columnJahr.Name = "jahr"
			columnJahr.FieldName = "jahr"
			columnJahr.Visible = False
			gvDetail.Columns.Add(columnJahr)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeZGDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedEmployeeZGDetailData) = New BindingList(Of FoundedEmployeeZGDetailData)
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedEmployee.EditValue Then
				employeeNumber = Me._ClsSetting.SelectedMANr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbEmployeeZGDataForDetails(employeeNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Vorschuss-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedEmployeeZGDetailData With
														   {.mdnr = person.mdnr,
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


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for employee salary"

	Sub ResetSalaryDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "lonr"
			columnmodulname.FieldName = "lonr"
			columnmodulname.Visible = True
			gvDetail.Columns.Add(columnmodulname)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvDetail.Columns.Add(columnBezeichnung)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = m_translate.GetSafeTranslationValue("Monat")
			columnMonth.Name = "monat"
			columnMonth.FieldName = "monat"
			columnMonth.Visible = False
			gvDetail.Columns.Add(columnMonth)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.Caption = m_translate.GetSafeTranslationValue("Jahr")
			columnYear.Name = "jahr"
			columnYear.FieldName = "jahr"
			columnYear.Visible = False
			gvDetail.Columns.Add(columnYear)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeSalaryDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedEmployeeSalaryDetailData) = New BindingList(Of FoundedEmployeeSalaryDetailData)
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedEmployee.EditValue Then
				employeeNumber = Me._ClsSetting.SelectedMANr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbEmployeeSalaryDataForDetail(employeeNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Lohn-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedEmployeeSalaryDetailData With
														   {.mdnr = person.mdnr,
															  .lonr = person.lonr,
															  .monat = person.monat,
															  .jahr = person.jahr,
															  .manr = person.manr,
															  .periode = person.periode,
															  .employeename = person.employeename,
															  .createdfrom = person.createdfrom,
															  .createdon = person.createdon,
															  .zfiliale = person.zfiliale
														   }).ToList()


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for employee Reports"

	Sub ResetReportDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "rpnr"
			columnmodulname.FieldName = "rpnr"
			columnmodulname.Visible = True
			gvDetail.Columns.Add(columnmodulname)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.Caption = m_translate.GetSafeTranslationValue("Lohn-Nr.")
			columnLONr.Name = "lonr"
			columnLONr.FieldName = "lonr"
			columnLONr.Visible = True
			gvDetail.Columns.Add(columnLONr)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Periode")
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Name = "periode"
			columnBezeichnung.FieldName = "periode"
			columnBezeichnung.Visible = True
			gvDetail.Columns.Add(columnBezeichnung)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.Caption = m_translate.GetSafeTranslationValue("Monat")
			columnMonth.Name = "monat"
			columnMonth.FieldName = "monat"
			columnMonth.Visible = True
			gvDetail.Columns.Add(columnMonth)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.Caption = m_translate.GetSafeTranslationValue("Jahr")
			columnYear.Name = "jahr"
			columnYear.FieldName = "jahr"
			columnYear.Visible = True
			gvDetail.Columns.Add(columnYear)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnIsDone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsDone.Caption = m_translate.GetSafeTranslationValue("Erfasst")
			columnIsDone.Name = "rpdone"
			columnIsDone.FieldName = "rpdone"
			columnIsDone.Visible = True
			gvDetail.Columns.Add(columnIsDone)



			Dim columnFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFiliale.Name = "zfiliale"
			columnFiliale.FieldName = "zfiliale"
			columnFiliale.Visible = False
			gvDetail.Columns.Add(columnFiliale)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadEmployeeReportDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedEmployeeReportDetailData) = New BindingList(Of FoundedEmployeeReportDetailData)
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedEmployee.EditValue Then
				employeeNumber = Me._ClsSetting.SelectedMANr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbEmployeeReportDataForDetails(employeeNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedEmployeeReportDetailData With
														   {.mdnr = person.mdnr,
															  .rpnr = person.rpnr,
															  .lonr = person.lonr,
															  .esnr = person.esnr,
															  .manr = person.manr,
															  .kdnr = person.kdnr,
															  .monat = person.monat,
															  .jahr = person.jahr,
															  .periode = person.periode,
															  .employeename = person.employeename,
															  .customername = person.customername,
															  .rpgav_beruf = person.rpgav_beruf,
															  .rpdone = person.rpdone,
															  .createdon = person.createdon,
															  .createdfrom = person.createdfrom,
															  .zfiliale = person.zfiliale
														   }).ToList()


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region



	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvDetail.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Me.Modul2Open.ToLower
					Case "maes"
						Dim viewData = CType(dataRow, FoundedEmployeeESDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else
								If viewData.esnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedESNr = viewData.esnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select


					Case "mapropose"
						Dim viewData = CType(dataRow, FoundedEmployeeProposalDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case "zhdname"
								If viewData.zhdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
									_ClsKD.OpenSelectedCPerson()
								End If

							Case Else
								If viewData.pnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedProposeNr = viewData.pnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedProposeTiny(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select


					Case "macontact"
						Dim viewData = CType(dataRow, ModulViewEmployeeContactData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
								End If

							Case Else
								If viewData.contactnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.ContactRecordNumber = viewData.contactnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployeeContact()
								End If

						End Select


					Case "malo"
						Dim viewData = CType(dataRow, FoundedEmployeeSalaryDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.lonr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedLONr = viewData.lonr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedLO()
								End If

						End Select


					Case "marp"
						Dim viewData = CType(dataRow, FoundedEmployeeReportDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else
								If viewData.rpnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select


					Case "mazg"
						Dim viewData = CType(dataRow, FoundedEmployeeZGDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.zgnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZGNr = viewData.zgnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedAdvancePayment(viewData.mdnr, ModulConstants.UserData.UserNr)

									'_ClsKD.OpenSelectedZG()
								End If

						End Select


				End Select

			End If

		End If

	End Sub



	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

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


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case Me.Modul2Open
			Case "maes".ToLower
				Try
					If tgsAssignedEmployee.EditValue Then
						If File.Exists(m_GVESSettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVESSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "mapropose".ToLower
				Try
					If tgsAssignedEmployee.EditValue Then
						If File.Exists(m_GVProposeSettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVProposeSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "macontact".ToLower
				Try
					If tgsAssignedEmployee.EditValue Then
						If File.Exists(m_GVContactSettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVContactSettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVContactSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVContactSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case "mazg".ToLower
				Try
					If tgsAssignedEmployee.EditValue Then
						If File.Exists(m_GVZGSettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVZGSettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVZGSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVZGSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case "malo".ToLower
				Try
					If tgsAssignedEmployee.EditValue Then
						If File.Exists(m_GVSalarySettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVSalarySettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVSalarySettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVSalarySettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "maRP".ToLower
				Try
					If tgsAssignedEmployee.EditValue Then
						If File.Exists(m_GVReportSettingfilenameWithEmployee) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilenameWithEmployee)
					Else
						If File.Exists(m_GVReportSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case Else

				Exit Sub


		End Select


	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		If Me.Modul2Open = "maes" Then
			If tgsAssignedEmployee.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVESSettingfilenameWithEmployee)
			Else
				gvDetail.SaveLayoutToXml(m_GVESSettingfilename)

			End If

		ElseIf Me.Modul2Open = "mapropose" Then
			If tgsAssignedEmployee.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVProposeSettingfilenameWithEmployee)
			Else
				gvDetail.SaveLayoutToXml(m_GVProposeSettingfilename)

			End If

		ElseIf Me.Modul2Open = "macontact" Then

			If tgsAssignedEmployee.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVContactSettingfilenameWithEmployee)
			Else
				gvDetail.SaveLayoutToXml(m_GVContactSettingfilename)

			End If

		ElseIf Me.Modul2Open = "mazg" Then
			If tgsAssignedEmployee.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVZGSettingfilenameWithEmployee)
			Else
				gvDetail.SaveLayoutToXml(m_GVZGSettingfilename)

			End If

		ElseIf Me.Modul2Open = "malo" Then

			If tgsAssignedEmployee.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVSalarySettingfilenameWithEmployee)

			Else

				gvDetail.SaveLayoutToXml(m_GVSalarySettingfilename)

			End If

		ElseIf Me.Modul2Open = "marp" Then

			If tgsAssignedEmployee.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVReportSettingfilenameWithEmployee)
			Else
				gvDetail.SaveLayoutToXml(m_GVReportSettingfilename)

			End If

		End If

	End Sub


	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvDetail.RowCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub

	Private Function ShowProgressPanel() As IOverlaySplashScreenHandle
		m_Handle = SplashScreenManager.ShowOverlayForm(Me)
		Return m_Handle
	End Function

	Private Sub CloseProgressPanel(ByVal handle As IOverlaySplashScreenHandle)
		If Not m_Handle Is Nothing Then SplashScreenManager.CloseOverlayForm(m_Handle)
	End Sub

	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is GridLookUpEdit Then
				Dim grdlookupEdit As GridLookUpEdit = CType(sender, GridLookUpEdit)
				grdlookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is ComboBoxEdit Then
				Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
				comboboxEdit.EditValue = Nothing
			End If
		End If
	End Sub


#End Region


#Region "Helper Classes"

	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class

#End Region


End Class