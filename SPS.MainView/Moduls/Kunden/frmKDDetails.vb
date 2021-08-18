
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid

Imports DevExpress.XtraGrid.Views.Base
Imports SPS.MainView.DataBaseAccess
Imports System.ComponentModel
Imports System.IO
Imports SP.DatabaseAccess.ModulView
Imports SP.DatabaseAccess.ModulView.DataObjects
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common

Public Class frmKDDetails

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

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

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private _ClsSetting As New ClsKDSetting
	Private Property Modul2Open As String

	Private m_translate As TranslateValues

	Private m_connectionString As String

	Private m_GVESSettingfilename As String
	Private m_GVvacancySettingfilename As String
	Private m_GVProposeSettingfilename As String
	Private m_GVContactSettingfilename As String
	Private m_GVZESettingfilename As String
	Private m_GVInvoiceSettingfilename As String
	Private m_GVReportSettingfilename As String

	Private m_GVESSettingfilenameWithCustomer As String
	Private m_GVVacancySettingfilenameWithCustomer As String
	Private m_GVProposeSettingfilenameWithCustomer As String
	Private m_GVContactSettingfilenameWithCustomer As String
	Private m_GVZESettingfilenameWithCustomer As String
	Private m_GVInvoiceSettingfilenameWithCustomer As String
	Private m_GVReportSettingfilenameWithCustomer As String

	Private Property SearchWithCustomerNumber As Boolean?
	Protected m_SuppressUIEvents As Boolean = False
	Private m_Handle As IOverlaySplashScreenHandle


	Public Sub New(ByVal _setting As ClsKDSetting, ByVal _m2Open As String)

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

		Try
			m_connectionString = ModulConstants.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, ModulConstants.UserData.UserLanguage)

			Dim strModulName As String = Me.Modul2Open.ToLower

			m_GVESSettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVvacancySettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVZESettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVInvoiceSettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVReportSettingfilename = String.Format("{0}Customer\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

			m_GVESSettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVVacancySettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVZESettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVInvoiceSettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVReportSettingfilenameWithCustomer = String.Format("{0}Customer\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		Reset()

		'RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
		'AddHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged

		AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvDetail.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvDetail.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvDetail.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler Me.gvDetail.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick

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

		aceAssignedCustomer.Text = String.Format("Für {0}?", _ClsSetting.CustomerName)
		tgsAssignedCustomer.EditValue = Me._ClsSetting.Data4SelectedKD
		tgsAssignedCustomer.Properties.EditValueChangedFiringMode = EditValueChangedFiringMode.Buffered
		tgsAssignedCustomer.Properties.EditValueChangedDelay = 200

		pnlContactFileter.BorderStyle = BorderStyles.NoBorder
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
		LoadCustomerContactDetailList()

	End Sub

	Private Sub OnlueMonth_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMonth.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		ResetContactDetailGrid()
		LoadCustomerContactDetailList()

	End Sub

	Private Sub txtPlainText_EditValueChanged(sender As Object, e As EventArgs) Handles txtPlainText.EditValueChanged
		ResetContactDetailGrid()
		LoadCustomerContactDetailList()
	End Sub

	Private Sub txtPlainText_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtPlainText.ButtonClick

		ResetContactDetailGrid()
		LoadCustomerContactDetailList()

	End Sub

	Private Sub txtPlainText_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPlainText.KeyUp
		If e.KeyCode = Keys.Return Then
			ResetContactDetailGrid()
			LoadCustomerContactDetailList()
		End If
	End Sub

	Private Sub OnForm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.SETTING_CUSTOMER_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.SETTING_CUSTOMER_WIDTH = Me.Width
				My.Settings.SETTING_CUSTOMER_HEIGHT = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub OnForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		OngvColumnPositionChanged(New Object, New System.EventArgs)
	End Sub

	Private Sub frmDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTitle As String = String.Empty

		Try
			If My.Settings.SETTING_CUSTOMER_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_CUSTOMER_HEIGHT)
			If My.Settings.SETTING_CUSTOMER_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_CUSTOMER_WIDTH)
			If My.Settings.SETTING_CUSTOMER_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_CUSTOMER_LOCATION.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

		End Try

		Me.sccMain.Dock = DockStyle.Fill
		Me.bsiRecCount.Caption = m_translate.GetSafeTranslationValue("Bereit")

		Try
			pnlContactFileter.Visible = False

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			'chkSelMA.Enabled = True
			Me.gvDetail.OptionsView.ShowIndicator = False
			Select Case Me.Modul2Open.ToLower
				Case "kdES".ToLower
					strTitle = "Anzeige der Einsätze"
					ResetESDetailGrid()
					LoadEmployeeESDetailList()

				Case "kdze".ToLower
					strTitle = "Anzeige der Zahlungseingänge"
					ResetRecipientOfPaymentsDetailGrid()
					LoadCustomerRecipientOfPaymentsDetailList()

				Case "kdRP".ToLower
					strTitle = "Anzeige der Rapporte"
					ResetReportDetailGrid()
					LoadCustomerReportDetailList()

				Case "kdre".ToLower
					strTitle = "Anzeige der Debitoren"
					ResetInvoiceDetailGrid()
					LoadCustomerInvoiceDetailList()

				Case "kdVacancy".ToLower
					strTitle = "Anzeige der Vakanzen"
					ResetVacancyDetailGrid()
					LoadCustomerVacancyDetailList()

				Case "kdPropose".ToLower
					strTitle = "Anzeige der Vorschläge"
					ResetProposeDetailGrid()
					LoadCustomerProposeDetailList()

				Case "kdcontact".ToLower
					pnlContactFileter.Visible = True
					strTitle = "Anzeige der Kontakte"
					ResetContactDetailGrid()
					LoadCustomerContactDetailList()
					'chkSelMA.Enabled = False


				Case Else

			End Select
			Me.Text = String.Format(m_translate.GetSafeTranslationValue(strTitle))
			Me.rlblDetailHeader.Text = String.Format("<b>{0}</b>", m_translate.GetSafeTranslationValue(strTitle))

		Catch ex As Exception


		Finally
			CloseProgressPanel(m_Handle)

		End Try

	End Sub

	Private Sub OntgsAssignedCustomer_EditValueChanged(sender As Object, e As EventArgs) Handles tgsAssignedCustomer.EditValueChanged

		If m_SuppressUIEvents Then Return

		grdDetailrec.BeginUpdate()
		Me.gvDetail.Columns.Clear()
		Me.grdDetailrec.DataSource = Nothing

		'Me._ClsSetting.Data4SelectedKD = Me.chkSelMA.Checked
		'SearchWithCustomerNumber = Me.chkSelMA.Checked

		Try

			Select Case Me.Modul2Open.ToLower

				Case "kdes"
					ResetESDetailGrid()
					LoadEmployeeESDetailList()

				Case "kdvacancy"
					ResetVacancyDetailGrid()
					LoadCustomerVacancyDetailList()

				Case "kdpropose"
					ResetProposeDetailGrid()
					LoadCustomerProposeDetailList()

				Case "kdcontact"
					ResetContactDetailGrid()
					LoadCustomerContactDetailList()

				Case "kdre".ToLower
					ResetInvoiceDetailGrid()
					LoadCustomerInvoiceDetailList()

				Case "kdze".ToLower
					ResetRecipientOfPaymentsDetailGrid()
					LoadCustomerRecipientOfPaymentsDetailList()

				Case "kdrp"
					ResetReportDetailGrid()
					LoadCustomerReportDetailList()

				Case Else

			End Select
			Me.grdDetailrec.EndUpdate()

		Catch ex As Exception


		Finally
			CloseProgressPanel(m_Handle)

		End Try

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvDetail.RowCount)
		OngvColumnPositionChanged(sender, e)

	End Sub

#Region "Details for customer ES"

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
		Dim listDataSource As BindingList(Of FoundedCustomerESDetailData) = New BindingList(Of FoundedCustomerESDetailData)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbCustomerESDataForDetails(customerNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Einsatz-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedCustomerESDetailData With
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
			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for customer Propose"

	''' <summary>
	''' reset vacancy grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetVacancyDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()



		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "vaknr"
			columnmodulname.FieldName = "vaknr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvDetail.Columns.Add(columnKDNr)

			Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDNr.Caption = m_translate.GetSafeTranslationValue("ZHDNr")
			columnZHDNr.Name = "kdzhdnr"
			columnZHDNr.FieldName = "kdzhdnr"
			columnZHDNr.Visible = False
			gvDetail.Columns.Add(columnZHDNr)

			Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Caption = m_translate.GetSafeTranslationValue("Bezeichnung")
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvDetail.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "firma1"
			columncustomername.FieldName = "firma1"
			columncustomername.Visible = False
			gvDetail.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDName.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.Name = "kdzname"
			columnZHDName.FieldName = "kdzname"
			columnZHDName.Visible = True
			gvDetail.Columns.Add(columnZHDName)

			Dim columnjchisonline As New DevExpress.XtraGrid.Columns.GridColumn()
			columnjchisonline.Caption = m_translate.GetSafeTranslationValue("Jobs.ch")
			columnjchisonline.Name = "jchisonline"
			columnjchisonline.FieldName = "jchisonline"
			columnjchisonline.Visible = False
			gvDetail.Columns.Add(columnjchisonline)

			Dim columnojisonline As New DevExpress.XtraGrid.Columns.GridColumn()
			columnojisonline.Caption = m_translate.GetSafeTranslationValue("ostjob.ch")
			columnojisonline.Name = "ojisonline"
			columnojisonline.FieldName = "ojisonline"
			columnojisonline.Visible = False
			gvDetail.Columns.Add(columnojisonline)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_translate.GetSafeTranslationValue("Eigene Plattform")
			columnAdvisor.Name = "ourisonline"
			columnAdvisor.FieldName = "ourisonline"
			columnAdvisor.Visible = False
			gvDetail.Columns.Add(columnAdvisor)

			Dim columnjobchdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnjobchdate.Caption = m_translate.GetSafeTranslationValue("Jobs.ch Datum")
			columnjobchdate.Name = "jobchdate"
			columnjobchdate.FieldName = "jobchdate"
			columnjobchdate.Visible = False
			gvDetail.Columns.Add(columnjobchdate)

			Dim columnostjobchdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnostjobchdate.Caption = m_translate.GetSafeTranslationValue("ostjob.ch Datum")
			columnostjobchdate.Name = "ostjobchdate"
			columnostjobchdate.FieldName = "ostjobchdate"
			columnostjobchdate.Visible = False
			gvDetail.Columns.Add(columnostjobchdate)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPArt.Caption = m_translate.GetSafeTranslationValue("Kontakt")
			columnPArt.Name = "vakkontakt"
			columnPArt.FieldName = "vakkontakt"
			columnPArt.Visible = False
			gvDetail.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPState.Caption = m_translate.GetSafeTranslationValue("Status")
			columnPState.Name = "vakstate"
			columnPState.FieldName = "vakstate"
			columnPState.Visible = False
			gvDetail.Columns.Add(columnPState)


			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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
			columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESAls.Name = "bezeichung"
			columnESAls.FieldName = "bezeichnung"
			columnESAls.Visible = True
			gvDetail.Columns.Add(columnESAls)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDName.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDName.Name = "zhdname"
			columnZHDName.FieldName = "zhdname"
			columnZHDName.Visible = True
			gvDetail.Columns.Add(columnZHDName)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = False
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_translate.GetSafeTranslationValue("Berater")
			columnAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdvisor.Name = "advisor"
			columnAdvisor.FieldName = "advisor"
			columnAdvisor.Visible = False
			gvDetail.Columns.Add(columnAdvisor)

			Dim columnPArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPArt.Caption = m_translate.GetSafeTranslationValue("Art")
			columnPArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPArt.Name = "p_art"
			columnPArt.FieldName = "p_art"
			columnPArt.Visible = False
			gvDetail.Columns.Add(columnPArt)

			Dim columnPState As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPState.Caption = m_translate.GetSafeTranslationValue("Status")
			columnPState.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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

	Public Function LoadCustomerVacancyDetailList() As Boolean
		Dim listDataSource As BindingList(Of CustomerVacanciesProperty) = New BindingList(Of CustomerVacanciesProperty)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If
			Dim listOfVacancy = m_DataAccess.GetDbCustomerVacancyDataForProperties(customerNumber, False)

			If listOfVacancy Is Nothing Then
				Return False
			End If

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


			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource
			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

	Public Function LoadCustomerProposeDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedCustomerProposalDetailData) = New BindingList(Of FoundedCustomerProposalDetailData)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbCustomerProposalDataForDetails(customerNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Vorschlag-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedCustomerProposalDetailData With
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
			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for customer contact"

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

			Dim columnZHDname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDname.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDname.Name = "zhdname"
			columnZHDname.FieldName = "zhdname"
			columnZHDname.Visible = False
			gvDetail.Columns.Add(columnZHDname)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidaten")
			columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeename.Name = "EmployeeNames"
			columnEmployeename.FieldName = "EmployeeNames"
			columnEmployeename.Visible = True
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnMoreEmployeesContacted As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMoreEmployeesContacted.Caption = m_translate.GetSafeTranslationValue("MoreEmployeesContacted")
			columnMoreEmployeesContacted.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMoreEmployeesContacted.Name = "MoreEmployeesContacted"
			columnMoreEmployeesContacted.FieldName = "MoreEmployeesContacted"
			columnMoreEmployeesContacted.Visible = True
			gvDetail.Columns.Add(columnMoreEmployeesContacted)

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

	Public Function LoadCustomerContactDetailList() As Boolean
		Dim listDataSource As BindingList(Of ModulViewCustomerContactData) = New BindingList(Of ModulViewCustomerContactData)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If


			Dim listOfEmployees = m_ModulViewDatabaseAccess.LoadGetDbCustomerContactData(ModulConstants.MDData.MDNr, customerNumber, lueYear.EditValue, lueMonth.EditValue, Nothing, Nothing, txtPlainText.EditValue)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Kontakt-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New ModulViewCustomerContactData With
																						 {.contactnr = person.contactnr,
																							.kdnr = person.kdnr,
																							.zhdnr = person.zhdnr,
																							.manr = person.manr,
																							.monat = person.monat,
																							.jahr = person.jahr,
																							.MoreEmployeesContacted = person.MoreEmployeesContacted,
																							.EmployeeNumbers = person.EmployeeNumbers,
																							.EmployeeNames = person.EmployeeNames,
																							.customername = person.customername,
																							.zhdname = person.zhdname,
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

			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for customer invoice"

	Sub ResetInvoiceDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "renr"
			columnmodulname.FieldName = "renr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.Name = "firma1"
			columnCustomerName.FieldName = "firma1"
			columnCustomerName.Visible = True
			gvDetail.Columns.Add(columnCustomerName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_translate.GetSafeTranslationValue("Adresse")
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.Name = "plzort"
			columnAddress.FieldName = "plzort"
			columnAddress.Visible = True
			gvDetail.Columns.Add(columnAddress)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnFakDat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFakDat.Name = "fakdate"
			columnFakDat.FieldName = "fakdate"
			columnFakDat.Visible = True
			gvDetail.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "betragink"
			columnBetragInk.FieldName = "betragink"
			columnBetragInk.Visible = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnBetragInk)

			Dim columnBetragOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragOpen.Caption = m_translate.GetSafeTranslationValue("Offener Betrag")
			columnBetragOpen.Name = "betragopen"
			columnBetragOpen.FieldName = "betragopen"
			columnBetragOpen.Visible = True
			columnBetragOpen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragOpen.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnBetragOpen)

			Dim columnIsOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsOpen.Caption = m_translate.GetSafeTranslationValue("Offen?")
			columnIsOpen.Name = "isopen"
			columnIsOpen.FieldName = "isopen"
			columnIsOpen.Visible = True
			gvDetail.Columns.Add(columnIsOpen)

			Dim columnREKst As New DevExpress.XtraGrid.Columns.GridColumn()
			columnREKst.Caption = m_translate.GetSafeTranslationValue("KST")
			columnREKst.Name = "rekst"
			columnREKst.FieldName = "rekst"
			columnREKst.Visible = True
			gvDetail.Columns.Add(columnREKst)

			Dim columnCustomerAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerAdvisor.Caption = m_translate.GetSafeTranslationValue("Kunden-Berater")
			columnCustomerAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerAdvisor.Name = "customeradvisor"
			columnCustomerAdvisor.FieldName = "customeradvisor"
			columnCustomerAdvisor.Visible = True
			gvDetail.Columns.Add(columnCustomerAdvisor)

			Dim columnEmployeeAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeAdvisor.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Berater")
			columnEmployeeAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnEmployeeAdvisor.Name = "employeeadvisor"
			columnEmployeeAdvisor.FieldName = "employeeadvisor"
			columnEmployeeAdvisor.Visible = True
			gvDetail.Columns.Add(columnEmployeeAdvisor)

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

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadCustomerInvoiceDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedCustomerInvoiceDetailData) = New BindingList(Of FoundedCustomerInvoiceDetailData)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbCustomerInvoiceDataForDetails(customerNumber)

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
														  .isopen = person.isopen,
														  .customeradvisor = person.customeradvisor,
														  .employeeadvisor = person.employeeadvisor,
														  .createdon = person.createdon,
														  .createdfrom = person.createdfrom,
														  .zfiliale = person.zfiliale
													   }).ToList()


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource
			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region


#Region "Details for customer ZE"

	Private Sub ResetRecipientOfPaymentsDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "zenr"
			columnmodulname.FieldName = "zenr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnCustomerName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerName.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columnCustomerName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerName.Name = "firma1"
			columnCustomerName.FieldName = "firma1"
			columnCustomerName.Visible = True
			gvDetail.Columns.Add(columnCustomerName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_translate.GetSafeTranslationValue("Adresse")
			columnAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddress.Name = "plzort"
			columnAddress.FieldName = "plzort"
			columnAddress.Visible = True
			gvDetail.Columns.Add(columnAddress)

			Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFakDat.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnFakDat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFakDat.Name = "valutadate"
			columnFakDat.FieldName = "valutadate"
			columnFakDat.Visible = True
			gvDetail.Columns.Add(columnFakDat)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "zebetrag"
			columnBetragInk.FieldName = "zebetrag"
			columnBetragInk.Visible = True
			gvDetail.Columns.Add(columnBetragInk)

			Dim columnCustomerAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerAdvisor.Caption = m_translate.GetSafeTranslationValue("Kunden-Berater")
			columnCustomerAdvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerAdvisor.Name = "customeradvisor"
			columnCustomerAdvisor.FieldName = "customeradvisor"
			columnCustomerAdvisor.Visible = False
			gvDetail.Columns.Add(columnCustomerAdvisor)

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

			Dim columnZFiliale As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZFiliale.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZFiliale.Caption = m_translate.GetSafeTranslationValue("Filiale")
			columnZFiliale.Name = "zfiliale"
			columnZFiliale.FieldName = "zfiliale"
			columnZFiliale.Visible = False
			gvDetail.Columns.Add(columnZFiliale)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Private Function LoadCustomerRecipientOfPaymentsDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedCustomerROPDetailData) = New BindingList(Of FoundedCustomerROPDetailData)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbCustomerRecipientOfPaymentsDataForDetails(customerNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedCustomerROPDetailData With
													   {.mdnr = person.mdnr,
														  .customerMDNr = person.customerMDNr,
														  .customeradvisor = person.customeradvisor,
														  .firma1 = person.firma1,
														  .plzort = person.plzort,
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


			For Each p In responsiblePersonsGridData
				listDataSource.Add(p)
			Next

			grdDetailrec.DataSource = listDataSource
			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Catch ex As Exception

		Finally
			CloseProgressPanel(m_Handle)

		End Try

		Return Not listDataSource Is Nothing
	End Function

#End Region



#Region "Details for customer Reports"

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

	Public Function LoadCustomerReportDetailList() As Boolean
		Dim listDataSource As BindingList(Of FoundedCustomerReportDetailData) = New BindingList(Of FoundedCustomerReportDetailData)
		Dim m_DataAccess As New MainGrid
		Dim customerNumber As Integer? = Nothing

		Try

			CloseProgressPanel(m_Handle)
			ShowProgressPanel()

			If tgsAssignedCustomer.EditValue Then
				customerNumber = Me._ClsSetting.SelectedKDNr
			End If
			Dim listOfEmployees = m_DataAccess.GetDbCustomerReportDataForDetails(customerNumber)

			If listOfEmployees Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Rapport-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim responsiblePersonsGridData = (From person In listOfEmployees
											  Select New FoundedCustomerReportDetailData With
													   {.mdnr = person.mdnr,
														  .employeeMDNr = person.employeeMDNr,
														  .customerMDNr = person.customerMDNr,
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
			Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

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
					Case "kdes"
						Dim viewData = CType(dataRow, FoundedCustomerESDetailData)

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

					Case "kdvacancy"
						Dim viewData = CType(dataRow, CustomerVacanciesProperty)

						Select Case column.Name.ToLower
							Case "firma1"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case "kdzname"
								If viewData.kdzhdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
									_ClsKD.OpenSelectedCPerson()
								End If

							Case Else
								If viewData.vaknr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedVakNr = viewData.vaknr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.kdzhdnr})
									_ClsKD.OpenSelectedVacancyTiny(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select

					Case "kdpropose"
						Dim viewData = CType(dataRow, FoundedCustomerProposalDetailData)

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


					Case "kdcontact"
						Dim viewData = CType(dataRow, ModulViewCustomerContactData)

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
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.ContactRecordNumber = viewData.contactnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomerContact()
								End If

						End Select

					Case "kdrp"
						Dim viewData = CType(dataRow, FoundedCustomerReportDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else
								If viewData.rpnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRPNr = viewData.rpnr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedReport(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select


					Case "kdre"
						Dim viewData = CType(dataRow, FoundedCustomerInvoiceDetailData)

						Select Case column.Name.ToLower
							Case "firma1"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.renr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedInvoice(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select


					Case "kdze"
						Dim viewData = CType(dataRow, FoundedCustomerROPDetailData)

						Select Case column.Name.ToLower
							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedRENr = viewData.renr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.zenr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedZENr = viewData.zenr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedPayment()
								End If

						End Select


				End Select

			End If

		End If

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


	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = m_translate.GetSafeTranslationValue(s)

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
			Case "kdes".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVESSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVESSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "kdvacancy".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVVacancySettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVVacancySettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVvacancySettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVvacancySettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "kdpropose".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVProposeSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVProposeSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "kdcontact".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVContactSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVContactSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVContactSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVContactSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case "kdre".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVReportSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVInvoiceSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVReportSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try



			Case "kdze".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVReportSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVInvoiceSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVReportSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "kdRP".ToLower
				Try
					If tgsAssignedCustomer.EditValue Then
						If File.Exists(m_GVReportSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVReportSettingfilenameWithCustomer)
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

		If Me.Modul2Open = "kdes" Then
			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVESSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVESSettingfilename)

			End If

		ElseIf Me.Modul2Open = "kdvacancy" Then
			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVVacancySettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVvacancySettingfilename)

			End If

		ElseIf Me.Modul2Open = "kdpropose" Then
			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVProposeSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVProposeSettingfilename)

			End If

		ElseIf Me.Modul2Open = "kdcontact" Then

			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVContactSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVContactSettingfilename)

			End If

		ElseIf Me.Modul2Open = "kdze" Then
			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVZESettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVZESettingfilename)

			End If

		ElseIf Me.Modul2Open = "kdre" Then

			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVInvoiceSettingfilenameWithCustomer)

			Else

				gvDetail.SaveLayoutToXml(m_GVInvoiceSettingfilename)

			End If

		ElseIf Me.Modul2Open = "kdrp" Then

			If tgsAssignedCustomer.EditValue Then
				gvDetail.SaveLayoutToXml(m_GVReportSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVReportSettingfilename)

			End If

		End If

	End Sub


	Private Function ShowProgressPanel() As IOverlaySplashScreenHandle
		m_Handle = SplashScreenManager.ShowOverlayForm(Me)
		Return m_Handle
	End Function

	Private Sub CloseProgressPanel(ByVal handle As IOverlaySplashScreenHandle)
		If Not m_Handle Is Nothing Then SplashScreenManager.CloseOverlayForm(m_Handle)
	End Sub



#End Region

#Region "Helper Classes"

	Class IntegerValueViewWrapper
		Public Property Value As Integer
	End Class

#End Region

End Class