
Imports System.Reflection.Assembly

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions

Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common.DataObjects

Public Class frmUmsatz
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsFunc As New ClsDivFunc

	Private Shared frmMyLV As frmListeSearch_LV

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Dim PrintListingThread As Thread
	Private Property SQL4Print As String
	Private Property Conn2Open As New SqlClient.SqlConnection
	Private Property PrintJobNr As String
	Private Property bPrintAsDesign As Boolean
	Private Property bPrintAsGrouped As Boolean

	Private Property TranslatedPage As New List(Of Boolean)

	Private m_mandant As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_SearchCriteria As New SearchCriteria
	Private m_DB1PayrollData As List(Of DB1PayrollData)
	Private m_DB1PayrollData_Staging As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData)
	Private m_DB1InvoiceData As List(Of DB1InvoiceData)

	Private Property IsListForFiliale As Boolean


#Region "Private Constants"

	Private Const frmMyLVName As String = "frmListeSearch_LV"
	Private Const strValueSeprator As String = "#@"

#End Region


#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_mandant = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Reset()
		LoadMandantenDropDown()

		Dim allowedtoShow = AllowedToShowEachEmployee(Now.Year)
		lblEinsatznr.Visible = allowedtoShow
		lueEinsatz.Visible = allowedtoShow
		lblKandidatennr.Visible = allowedtoShow
		lueEmployee.Visible = allowedtoShow
		lblKundennr.Visible = allowedtoShow
		lueCustomer.Visible = allowedtoShow

		chkEmployeeSearch.Visible = m_InitializationData.UserData.UserNr = 1
		chkCustomerSearch.Visible = m_InitializationData.UserData.UserNr = 1
		chkEmploymentSearch.Visible = m_InitializationData.UserData.UserNr = 1

		AddHandler Cbo_BMonth_1.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_BYear_1.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler Cbo_RPKst3.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_Filiale.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler lueEinsatz.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region


#Region "Private properties"

	Private ReadOnly Property GetHwnd() As String
		Get
			Return CStr(Me.Handle)
		End Get
	End Property

	Private ReadOnly Property GetJobID() As String
		Get
			Me.bPrintAsGrouped = IsListForFiliale
			Return String.Format("3.5.{0}", If(IsListForFiliale, "2", "1"))
		End Get
	End Property

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedESRecord As DB1ESData
		Get
			Dim gvRP = CType(lueEinsatz.GetSelectedDataRow(), DB1ESData)
			Return gvRP
		End Get

	End Property

#End Region


#Region "Reset"

	Private Sub Reset()

		ResetMandantenDropDown()

		ResetGridESData()
		ResetGridEmployeeData()
		ResetGridCustomerData()

	End Sub


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

		lueMandant.Properties.ShowFooter = False
		lueMandant.Properties.DropDownRows = 10
		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetGridESData()

		lueEinsatz.Properties.DisplayMember = "ESDataToShow"
		lueEinsatz.Properties.ValueMember = "ESNumber"

		gvES.OptionsView.ShowIndicator = False
		gvES.OptionsView.ShowAutoFilterRow = True
		gvES.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvES.Columns.Clear()

		Dim columnESNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNumber.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Nr.")
		columnESNumber.Name = "ESNumber"
		columnESNumber.FieldName = "ESNumber"
		columnESNumber.Visible = True
		columnESNumber.BestFit()
		gvES.Columns.Add(columnESNumber)

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = False
		columnEmployeeNumber.BestFit()
		gvES.Columns.Add(columnEmployeeNumber)

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = False
		columnCustomerNumber.BestFit()
		gvES.Columns.Add(columnCustomerNumber)

		Dim columnemployeefullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeefullname.Name = "employeefullname"
		columnemployeefullname.FieldName = "employeefullname"
		columnemployeefullname.Visible = True
		columnemployeefullname.BestFit()
		gvES.Columns.Add(columnemployeefullname)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columncustomername.Name = "customername"
		columncustomername.FieldName = "customername"
		columncustomername.Visible = True
		columncustomername.BestFit()
		gvES.Columns.Add(columncustomername)

		Dim columnes_ab As New DevExpress.XtraGrid.Columns.GridColumn()
		columnes_ab.Caption = m_Translate.GetSafeTranslationValue("ES-Ab")
		columnes_ab.Name = "es_ab"
		columnes_ab.FieldName = "es_ab"
		columnes_ab.Visible = True
		columnes_ab.BestFit()
		gvES.Columns.Add(columnes_ab)

		Dim columnes_ende As New DevExpress.XtraGrid.Columns.GridColumn()
		columnes_ende.Caption = m_Translate.GetSafeTranslationValue("ES-Ende")
		columnes_ende.Name = "es_ende"
		columnes_ende.FieldName = "es_ende"
		columnes_ende.Visible = True
		columnes_ende.BestFit()
		gvES.Columns.Add(columnes_ende)


		lueEinsatz.Properties.DataSource = Nothing
		lueEinsatz.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEinsatz.Properties.NullText = String.Empty
		lueEinsatz.EditValue = Nothing

	End Sub

	Private Sub ResetGridEmployeeData()

		lueEmployee.Properties.DisplayMember = "EmployeeDataToShow"
		lueEmployee.Properties.ValueMember = "EmployeeNumber"

		gvEmployee.OptionsView.ShowIndicator = False
		gvEmployee.OptionsView.ShowAutoFilterRow = True
		gvEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvEmployee.Columns.Clear()

		Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnEmployeeNumber.Name = "EmployeeNumber"
		columnEmployeeNumber.FieldName = "EmployeeNumber"
		columnEmployeeNumber.Visible = False
		columnEmployeeNumber.BestFit()
		gvEmployee.Columns.Add(columnEmployeeNumber)

		Dim columnemployeefullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeefullname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnemployeefullname.Name = "employeefullname"
		columnemployeefullname.FieldName = "employeefullname"
		columnemployeefullname.Visible = True
		columnemployeefullname.BestFit()
		gvEmployee.Columns.Add(columnemployeefullname)


		lueEmployee.Properties.DataSource = Nothing
		lueEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployee.Properties.NullText = String.Empty
		lueEmployee.EditValue = Nothing

	End Sub

	Private Sub ResetGridCustomerData()

		lueCustomer.Properties.DisplayMember = "CustomerDataToShow"
		lueCustomer.Properties.ValueMember = "CustomerNumber"

		gvCustomer.OptionsView.ShowIndicator = False
		gvCustomer.OptionsView.ShowAutoFilterRow = True
		gvCustomer.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvCustomer.Columns.Clear()

		Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = False
		columnCustomerNumber.BestFit()
		gvCustomer.Columns.Add(columnCustomerNumber)

		Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomername.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columncustomername.Name = "customername"
		columncustomername.FieldName = "customername"
		columncustomername.Visible = True
		columncustomername.BestFit()
		gvCustomer.Columns.Add(columncustomername)


		lueCustomer.Properties.DataSource = Nothing
		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomer.Properties.NullText = String.Empty
		lueCustomer.EditValue = Nothing

	End Sub

#End Region


#Region "Lookup Edit Load"
	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()

		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.EditValue = Nothing
		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()



		'Dim _ClsFunc As New ClsDbFunc(m_InitializationData)
		'Dim Data = _ClsFunc.LoadMandantenData()

		'lueMandant.Properties.DataSource = Data
		'lueMandant.Properties.ForceInitialize()

		'    Return Not Data Is Nothing
	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

	End Sub


#End Region



#Region "Lb clicks 1. Seite..."

#End Region


#Region "Dropdown Funktionen 1. Seite..."

	Private Sub Cbo_VMonth_1_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_VMonth_1.QueryPopUp
		ListMonth(Cbo_VMonth_1)
	End Sub

	Private Sub Cbo_bMonth_1_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_BMonth_1.QueryPopUp
		ListMonth(Cbo_BMonth_1)
	End Sub

	Private Sub Cbo_VYear_1_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_VYear_1.QueryPopUp
		ListYear(Cbo_VYear_1)
	End Sub

	Private Sub Cbo_bYear_1_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_BYear_1.QueryPopUp
		ListYear(Cbo_BYear_1)
	End Sub

	Private Sub OnCbo_VMonth_1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_VMonth_1.SelectedValueChanged
		Me.Cbo_RPKst3.Properties.Items.Clear()
		LoadGridData()
	End Sub

	Private Sub OnCbo_BMonth_1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_BMonth_1.SelectedValueChanged
		If Not Cbo_BMonth_1.Visible Then Return
		Me.Cbo_RPKst3.Properties.Items.Clear()
		LoadGridData()
	End Sub

	Private Sub OnCbo_VYear_1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_VYear_1.SelectedValueChanged
		Me.Cbo_RPKst3.Properties.Items.Clear()

		Dim allowedtoShow = AllowedToShowEachEmployee(Cbo_VYear_1.EditValue)
		lblEinsatznr.Visible = allowedtoShow
		lueEinsatz.Visible = allowedtoShow
		lblKandidatennr.Visible = allowedtoShow
		lueEmployee.Visible = allowedtoShow
		lblKundennr.Visible = allowedtoShow
		lueCustomer.Visible = allowedtoShow

		If Not allowedtoShow Then
			lueEinsatz.EditValue = Nothing
			lueEmployee.EditValue = Nothing
			lueCustomer.EditValue = Nothing
		End If
		LoadGridData()

	End Sub

	Private Sub OnCbo_BYear_1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_BYear_1.SelectedValueChanged
		If Not Cbo_BYear_1.Visible Then Return

		Me.Cbo_RPKst3.Properties.Items.Clear()
		LoadGridData()
	End Sub

	Private Sub Cbo_KDKanton_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_KDKanton.QueryPopUp
		ListKDKanton(Me.Cbo_KDKanton)
	End Sub

	Private Sub Cbo_BMonth_1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_BMonth_1.TextChanged,
																				Cbo_VMonth_1.TextChanged, Cbo_VYear_1.TextChanged,
																				Cbo_KDKanton.TextChanged, Cbo_Filiale.TextChanged, Cbo_RPKst3.TextChanged,
																				txt_ESGewerbe.TextChanged, txt_KDOrt.TextChanged, txt_MALand.TextChanged,
																				txt_MANationality.TextChanged

		If Not ClsDataDetail.Conn Is Nothing Then
			ClsDataDetail.Conn.Close()
			ClsDataDetail.Conn.Dispose()
		End If

	End Sub


	Private Sub Cbo_RPKst3_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_RPKst3.QueryPopUp
		Dim fmonth As Integer = 0
		Dim lmonth As Integer = 0
		Dim fyear As Integer = 0
		Dim lyear As Integer = 0

		If Cbo_VYear_1.EditValue Is Nothing Then fyear = Now.Year Else fyear = Cbo_VYear_1.EditValue
		If Cbo_BYear_1.EditValue Is Nothing Then lyear = fyear Else lyear = Cbo_BYear_1.EditValue

		If Cbo_VMonth_1.EditValue Is Nothing Then fmonth = Now.Month Else fmonth = Cbo_VMonth_1.EditValue
		If Cbo_BMonth_1.EditValue Is Nothing Then lmonth = fmonth Else lmonth = Cbo_BMonth_1.EditValue

		Dim _search As New SearchCriteria With {.FirstYear = fyear,
																						.LastYear = lyear,
																						.FirstMonth = fmonth,
																						.LastMonth = lmonth}

		ListOPKst(Me.Cbo_RPKst3, Me.lstKST3, _search)

	End Sub

	Private Sub Cbo_Filiale_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Filiale.QueryPopUp
		ListMDFiliale(Me.Cbo_Filiale)
	End Sub

#End Region



	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

#Region "Form Aktionen..."

	Private Sub frmUmsatz_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		If BackgroundWorker1.IsBusy Then
			BackgroundWorker1.CancelAsync()
		End If
		If Not ClsDataDetail.Conn Is Nothing Then
			ClsDataDetail.Conn.Close()
			ClsDataDetail.Conn.Dispose()
		End If

		FormIsLoaded(frmMyLVName, True)

		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			My.Settings.Save()


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblSortieren.Text = m_Translate.GetSafeTranslationValue(Me.lblSortieren.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)

		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
		Me.lblBeraterIn.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterIn.Text)
		Me.lblFiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblFiliale.Text)

		Me.lblEinsatznr.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatznr.Text)
		Me.lblKandidatennr.Text = m_Translate.GetSafeTranslationValue(Me.lblKandidatennr.Text)
		Me.lblKundennr.Text = m_Translate.GetSafeTranslationValue(Me.lblKundennr.Text)

		Me.grpBranche.Text = m_Translate.GetSafeTranslationValue(Me.grpBranche.Text)
		Me.LibBranche.Text = m_Translate.GetSafeTranslationValue(Me.LibBranche.Text)

		Me.grpKunden.Text = m_Translate.GetSafeTranslationValue(Me.grpKunden.Text)
		Me.lblKanton.Text = m_Translate.GetSafeTranslationValue(Me.lblKanton.Text)
		Me.LibOrt.Text = m_Translate.GetSafeTranslationValue(Me.LibOrt.Text)

		Me.grpKandidaten.Text = m_Translate.GetSafeTranslationValue(Me.grpKandidaten.Text)
		Me.LibNationality.Text = m_Translate.GetSafeTranslationValue(Me.LibNationality.Text)
		Me.LibLand.Text = m_Translate.GetSafeTranslationValue(Me.LibLand.Text)

		Me.lblInfo.Text = m_Translate.GetSafeTranslationValue(Me.lblInfo.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQLQuery.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLQuery.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.beiWorking.Caption = m_Translate.GetSafeTranslationValue(Me.beiWorking.Caption)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

	End Sub

	Private Sub OnFrmLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		TranslateControls()
		SetInitialFields()

		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try
		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If


			Me.CboSort.Properties.Items.AddRange(New Object() {m_Translate.GetSafeTranslationValue("0 - Disponenten"),
																												 m_Translate.GetSafeTranslationValue("1 - Filiale + Disponenten")})

			Dim strSort As String = My.Settings.Listsort
			ClsDataDetail.GetAutoUserNr = _ClsProgSetting.GetLogedUSNr

			Me.CboSort.Text = CStr(IIf(strSort = String.Empty, m_Translate.GetSafeTranslationValue("1 - Filiale + Disponenten"), strSort))
			If m_InitializationData.UserData.UserNr <> 1 Then Me.XtraTabControl1.TabPages.Remove(Me.xtabSQLQuery)


		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.Message))

		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Me.lueMandant.Visible = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

			bbiClearFields.Enabled = True
			bbiPrint.Enabled = False
			bbiExport.Enabled = False

			FillDefaultValues()

			LoadGridData()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Mandantenauswahl anzeigen: {0}", ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

	End Sub

	Private Sub SPBUmsatzTotal_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded(frmMyLVName, False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If

	End Sub

	Sub FillDefaultValues()

		If Now.Day <= 15 Then
			Me.Cbo_VMonth_1.EditValue = If(Now.Month = 1, 12, Now.Month - 1)
			If Now.Month = 1 Then Me.Cbo_VYear_1.EditValue = Now.Year - 1 Else Me.Cbo_VYear_1.EditValue = Now.Year

		Else
			Me.Cbo_VMonth_1.EditValue = Now.Month
			Me.Cbo_VYear_1.EditValue = Now.Year

		End If

	End Sub

#End Region

	Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean,
										ByVal strJobInfo As String, ByVal strQuery As String,
										ByVal bAsGrouped As Boolean)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True

		Dim sSql As String = CStr(IIf(strQuery = String.Empty, Me.txt_1_Query.Text, strQuery))
		If sSql = String.Empty Then
			MsgBox(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "GetData4Print_1")
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)

		Try
			Conn.Open()

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Try
				If Not rKDrec.HasRows Then
					cmd.Dispose()
					rKDrec.Close()

					m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
					Exit Sub
				End If

			Catch ex As Exception
				m_UtilityUi.ShowErrorDialog(ex.ToString)

			End Try

			rKDrec.Read()
			If rKDrec.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo
				Me.bPrintAsGrouped = strJobInfo = "3.5.2"

				StartPrinting()

			End If
			rKDrec.Close()


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

	End Sub

	Sub StartPrinting()
		Dim strFilter As String = String.Empty

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname)

		strFilter &= If(m_SearchCriteria.FirstMonth > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.FirstMonth, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.FirstYear > 0, String.Format(" / {0}", m_SearchCriteria.FirstYear, vbNewLine), String.Empty)
		If Not (m_SearchCriteria.LastYear = m_SearchCriteria.FirstYear And m_SearchCriteria.LastMonth = m_SearchCriteria.FirstMonth) Then
			strFilter &= String.Format(" - {0}", m_SearchCriteria.LastMonth)
			strFilter &= String.Format(" / {0}", m_SearchCriteria.LastYear)

		Else
			strFilter &= vbNewLine

		End If

		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.kst), String.Format("Kostenstelle: {0}, ", m_SearchCriteria.kst), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale), String.Format("Filiale: {0}, ", m_SearchCriteria.filiale), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.kanton), String.Format("Kanton: {0}, ", m_SearchCriteria.kanton), String.Empty)
		strFilter &= If(Not String.IsNullOrWhiteSpace(m_SearchCriteria.esbranche), String.Format("Branche: {0}, ", m_SearchCriteria.esbranche), String.Empty)

		strFilter &= If(m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0, String.Format("Einsatznummer: {0}, ", m_SearchCriteria.esNumber), String.Empty)
		strFilter &= If(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) > 0, String.Format("Kandidatennummer: {0}, ", m_SearchCriteria.employeeNumber), String.Empty)
		strFilter &= If(m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0, String.Format("Kundennummer: {0}", m_SearchCriteria.customerNumber), String.Empty)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLDb1SearchPrintSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr, .DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																									 .frmhwnd = GetHwnd,
																																									 .SQL2Open = Me.SQL4Print,
																																									 .JobNr2Print = Me.PrintJobNr,
																																									 .XMarge = ClsDataDetail.GetXMarge,
																																									 .XMarge_2 = ClsDataDetail.GetXMarge_2,
																																									 ._dTotalTemp = ClsDataDetail.GetTotalTemp,
																																									 ._dTotalInd = ClsDataDetail.GetTotalInd,
																																									 ._dTotalFest = ClsDataDetail.GetTotalFest,
																																									 .Filter_Month_1 = ClsDataDetail.GetFormVars(1).ToString &
										IIf(ClsDataDetail.GetFormVars(2).ToString <> "", " - ", "").ToString & ClsDataDetail.GetFormVars(2).ToString,
																																									 .FltJahr = ClsDataDetail.GetFormVars(1).ToString,
																																									 .VonMonat = ClsDataDetail.GetFormVars(1).ToString,
																																									 .BisMonat = ClsDataDetail.GetFormVars(2).ToString,
																																									 .bAsGrouped = IsListForFiliale,
																																									 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)})}
		Dim obj As New SPS.Listing.Print.Utility.Db1SearchListing.ClsPrintDb1SearchList(_Setting)
		obj.PrintDb1Liste(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub


#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		'Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)


		Try
			'If Not m_DB1PayrollData Is Nothing Then
			'	For Each itm In m_DB1PayrollData
			'		Trace.WriteLine(itm.payrollNumber)
			'		m_Logger.LogInfo("LO" & vbTab & itm.payrollNumber)
			'	Next
			'End If

			'If Not m_DB1InvoiceData Is Nothing Then
			'	For Each itm In m_DB1InvoiceData
			'		Trace.WriteLine(itm.invoiceArt & vbTab & itm.invoicenNumber)
			'		m_Logger.LogInfo(itm.invoiceArt & vbTab & itm.invoicenNumber)
			'	Next
			'End If

#If DEBUG Then
			m_DB1PayrollData_Staging = Nothing

			If Not m_DB1PayrollData_Staging Is Nothing Then
				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(ClsDataDetail.GetSQLQuery(), String.Format("[UmsatzJournal_New_{0}]", _ClsProgSetting.GetLogedUSGuid))
					frmMyLV.LoadData()

					'If Not m_DB1PayrollData Is Nothing Then frmMyLV.LoadPayrollData(m_DB1PayrollData)
					If Not m_DB1PayrollData_Staging Is Nothing Then frmMyLV.LoadPayrollData_Staging(m_DB1PayrollData_Staging)
					If Not m_DB1InvoiceData Is Nothing Then frmMyLV.LoadInvoiceData(m_DB1InvoiceData)

					frmMyLV.Show()
					Me.Select()
					Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																							 frmMyLV.RecCount)
					frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																									frmMyLV.RecCount)

					' Die Buttons Drucken und Export aktivieren
					If frmMyLV.RecCount > 0 Then
						Me.bbiPrint.Enabled = True
						Me.bbiExport.Enabled = True

						CreatePrintPopupMenu()
						CreateExportPopupMenu()

					End If

					Me.txt_1_Query.Text = ClsDataDetail.GetSQLQuery()
					Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

					Me.bbiSearch.Enabled = True
					Me.bbiClearFields.Enabled = True

					PlaySound(0)
				End If

				Return
			End If


#End If


			m_SearchCriteria = GetSearchKrieteria()
			If m_SearchCriteria Is Nothing Then Return

			If Not IsNothing(ClsDataDetail.Conn) Then ClsDataDetail.Conn.Dispose()

			'' KST3-Daten in Lst füllen...
			FillFoundedKstBez(Me.lstKST3, m_SearchCriteria) '.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastYear)
			If Me.lstKST3.Items.Count = 0 Then
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Es existieren keine Daten für Kostenstellen! Bitte ändern Sie Ihre Suchkrieterien."))

				Return
			End If


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try

		If ClsDataDetail.GetFormVars(5).ToString <> String.Empty Then
			Me.lstKST3.BeginUpdate()
			For i As Integer = Me.lstKST3.Items.Count - 1 To 0 Step -1
				Trace.WriteLine(String.Format("KST: {0}", Me.lstKST3.Items(i).ToString))

				If Not Me.lstKST3.Items(i).ToString.ToUpper.Contains(ClsDataDetail.GetFormVars(5).ToString.Trim.ToUpper) Then
					Me.lstKST3.Items.RemoveAt(i)
				End If
			Next
			Me.lstKST3.EndUpdate()
		End If

		Try
			Me.txt_SQLQuery.Text = String.Empty
			Me.txt_1_Query.Text = String.Empty

			ClsDataDetail.strAllKDNr = String.Empty
			FormIsLoaded(frmMyLVName, True)

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."


			' Daten suchen...
			GetMyQueryString()


		Catch ex As Exception
			MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "btnSearch_Click")

		Finally

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		result.listname = m_Translate.GetSafeTranslationValue("DB1 Liste")
		result.mandantenname = lueMandant.Text

		If Me.Cbo_VMonth_1.EditValue Is Nothing Then Me.Cbo_VMonth_1.EditValue = Now.Month
		If Me.Cbo_VYear_1.EditValue Is Nothing Then Me.Cbo_VYear_1.EditValue = Now.Year


		Dim strFMonth As Integer = Val(Cbo_VMonth_1.EditValue)
		Dim strLMonth As Integer? = Val(Cbo_BMonth_1.EditValue)
		Dim strVYear As Integer = Val(Cbo_VYear_1.EditValue)
		Dim strBYear As Integer? = Val(Cbo_BYear_1.EditValue)

		If strLMonth.GetValueOrDefault(0) = 0 Then strLMonth = strFMonth
		If strBYear.GetValueOrDefault(0) = 0 Then strBYear = strVYear

		If CDate("01." & strFMonth & "." & strVYear) > CDate("01." & strLMonth & "." & strBYear) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der ausgewählte Zeitraum ist ungültig. Bitte ändern Sie Ihre Auswahl."))
			Return Nothing
		End If

		ClsDataDetail.GetKstFullName = Me.Cbo_RPKst3.Text
		Dim strKst As String = String.Empty
		If Not String.IsNullOrWhiteSpace(Me.Cbo_RPKst3.Text) Then
			Dim aUSData As String() = Me.Cbo_RPKst3.Text.Split(CChar("("))
			strKst = aUSData(1).Replace(")", "").ToUpper
		End If


		ClsDataDetail.GetFormVars.Clear()
		ClsDataDetail.GetFormVars.Add(Me.CboSort.Text)
		ClsDataDetail.GetFormVars.Add(strFMonth)
		ClsDataDetail.GetFormVars.Add(strLMonth)
		ClsDataDetail.GetFormVars.Add(strVYear)
		ClsDataDetail.GetFormVars.Add(strBYear)

		ClsDataDetail.GetFormVars.Add(strKst)
		ClsDataDetail.GetFormVars.Add(Me.Cbo_Filiale.Text)
		ClsDataDetail.GetFormVars.Add(Me.txt_ESGewerbe.Text)
		ClsDataDetail.GetFormVars.Add(Me.Cbo_KDKanton.Text)
		ClsDataDetail.GetFormVars.Add(Me.txt_KDOrt.Text)
		ClsDataDetail.GetFormVars.Add(Me.txt_MANationality.Text)
		ClsDataDetail.GetFormVars.Add(Me.txt_MALand.Text)

		result.FirstMonth = strFMonth
		result.FirstYear = strVYear

		result.LastMonth = strLMonth
		result.LastYear = strBYear

		result.filiale = CStr(Cbo_Filiale.EditValue)
		result.kst = strKst
		result.sortvalue = Me.CboSort.Text
		result.esbranche = txt_ESGewerbe.Text
		result.kanton = Cbo_KDKanton.Text
		result.customercity = txt_KDOrt.Text
		result.employeenationality = txt_MANationality.Text
		result.employeecountry = txt_MALand.Text

		result.esNumber = CType(lueEinsatz.EditValue, Integer)
		result.employeeNumber = CType(lueEmployee.EditValue, Integer)
		result.customerNumber = CType(lueCustomer.EditValue, Integer)

		Return result

	End Function

	Function GetMyQueryString() As Boolean

		Me.bbiSearch.Enabled = False
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try

		Return True
	End Function

	Private Sub bbiClearFields_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClearFields.ItemClick

		FormIsLoaded(frmMyLVName, True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Cbo_RPKst3.EditValue = String.Empty
		Cbo_Filiale.EditValue = String.Empty

		lueEinsatz.EditValue = Nothing
		lueEmployee.EditValue = Nothing
		lueCustomer.EditValue = Nothing
		LoadGridData()

		txt_ESGewerbe.EditValue = String.Empty
		Cbo_KDKanton.EditValue = String.Empty
		txt_KDOrt.EditValue = String.Empty

		txt_MALand.EditValue = String.Empty
		txt_MANationality.EditValue = String.Empty

		txt_SQLQuery.EditValue = String.Empty
		txt_1_Query.EditValue = String.Empty

		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False
		If Not IsNothing(ClsDataDetail.Conn) Then ClsDataDetail.Conn.Dispose()
		ClsDataDetail.GetFormVars.Clear()

	End Sub

#End Region


#Region "Multitreading..."

	Private Sub OnBackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		Dim _ClsDb As New ClsDbFunc(m_InitializationData)
		Dim _ClsQuery As New ClsGetSQLString(m_SearchCriteria)
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		_ClsDb.SearchCriterias = m_SearchCriteria

		CheckForIllegalCrossThreadCalls = False
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		_ClsDb.LbKST3 = Me.lstKST3

		Try
			If chkCustomerSearch.Checked Then
				If IsNothing(ClsDataDetail.Conn) OrElse ClsDataDetail.Conn.State <> ConnectionState.Open Then
					_ClsDb.CalcDataForJournal_Test(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastYear)

				End If
			Else
				If IsNothing(ClsDataDetail.Conn) Then
					_ClsDb.CalcDataForJournal(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastYear)
				Else
					If ClsDataDetail.Conn.State <> ConnectionState.Open Then
						_ClsDb.CalcDataForJournal(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastYear)
					End If
				End If

			End If


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try

		Try
			' SQL_Query mit Order Klausel...
			If chkCustomerSearch.Checked Then
				Dim result As Boolean = _ClsQuery.LoadJournalDataWithCustomerForOutput(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear)
				ClsDataDetail.GetSQLQuery() = String.Format("Select * From [UmsatzJournal_New_{0}] UmJ ", _ClsProgSetting.GetLogedUSGuid)

			Else
				ClsDataDetail.GetSQLQuery() = _ClsQuery.GetUJournalQueryForOutput(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear)
			End If

			Dim _ClsJournal As New ClsJournal(m_SearchCriteria)
			_ClsJournal.CallculateAllFields()
			_ClsDb.CreateMySP4FilialProz()

			m_DB1PayrollData = _ClsDb.GetPayrollData
			m_DB1PayrollData_Staging = _ClsDb.GetPayrollData_Staging


			m_DB1InvoiceData = _ClsDb.GetInvoiceData


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try

		e.Result = True
		If bw.CancellationPending Then e.Cancel = True
		Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			m_UtilityUi.ShowErrorDialog(e.Error.ToString)
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorgang wurde abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()
				'        MessageBox.Show(e.Result.ToString())

				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(ClsDataDetail.GetSQLQuery(), String.Format("[UmsatzJournal_New_{0}]", _ClsProgSetting.GetLogedUSGuid))
					frmMyLV.LoadData()

					'If Not m_DB1PayrollData Is Nothing Then frmMyLV.LoadPayrollData(m_DB1PayrollData)
					If Not m_DB1PayrollData_Staging Is Nothing Then frmMyLV.LoadPayrollData_Staging(m_DB1PayrollData_Staging)
					If Not m_DB1InvoiceData Is Nothing Then frmMyLV.LoadInvoiceData(m_DB1InvoiceData)

					frmMyLV.Show()
					Me.Select()
					Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																							 frmMyLV.RecCount)
					frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																									frmMyLV.RecCount)

					' Die Buttons Drucken und Export aktivieren
					If frmMyLV.RecCount > 0 Then
						Me.bbiPrint.Enabled = True
						Me.bbiExport.Enabled = True

						CreatePrintPopupMenu()
						CreateExportPopupMenu()

					End If

					Me.txt_1_Query.Text = ClsDataDetail.GetSQLQuery()
					Me.beiWorking.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

					Me.bbiSearch.Enabled = True
					Me.bbiClearFields.Enabled = True

					PlaySound(0)
				End If
			End If

		End If


	End Sub

#End Region



	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim bAllowedList As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 608, m_InitializationData.MDData.MDNr)
		Dim bAllowedDesignList As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 618, m_InitializationData.MDData.MDNr)
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {If(bAllowedList, "DB1-Liste drucken#PrintList", ""),
																					 "Filialstatistiken drucken#PrintListForFiliale",
																					 "Entwurfsansicht#PrintDesign", "Entwurfsansicht der Filialstatistiken#printdesignForFiliale"}
		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				If myValue(1).ToString.ToLower = ("PrintDesign".ToLower) Then bshowMnu = bAllowedDesignList Else bshowMnu = Not String.IsNullOrWhiteSpace(myValue(0))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower = ("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			'm_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim _ClsQuery As New ClsGetSQLString(m_SearchCriteria)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintList".ToUpper
					IsListForFiliale = False
					Me.bPrintAsDesign = False
					SQL4Print = Me.txt_1_Query.Text

				Case "PrintListForFiliale".ToUpper
					IsListForFiliale = True
					Me.bPrintAsDesign = False
					Me.SQL4Print = _ClsQuery.GetUJournalQueryForGroupedOutput(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear)

				Case "printdesign".ToUpper
					IsListForFiliale = False
					Me.bPrintAsDesign = True
					SQL4Print = Me.txt_1_Query.Text

				Case "printdesignForFiliale".ToUpper
					IsListForFiliale = True
					Me.bPrintAsDesign = True
					Me.SQL4Print = _ClsQuery.GetUJournalQueryForGroupedOutput(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear)

				Case Else
					Exit Sub

			End Select
			Me.PrintJobNr = GetJobID()
			StartPrinting()


		Catch ex As Exception
			'm_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiExport.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV"}
		Try
			bbiExport.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = myValue(1).ToString

					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.txt_SQLQuery.Text

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				StartExportModul()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_1_Query.Text,
																																			 .ModulName = "Db1ListTOCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromDB1Listing(Me.txt_1_Query.Text)

	End Sub


#Region "Buttonclicks..."

	Private Sub OnBranche_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_ESGewerbe.ButtonClick
		Dim frmTest As New frmSearchRec(3)

		_ClsFunc.Get4What = "ESGewerbe"
		ClsDataDetail.strButtonValue = "ESGewerbe"
		ClsDataDetail.Get4What = "ESGewerbe"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMyValue(_ClsFunc.Get4What)
		Me.txt_ESGewerbe.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

	Private Sub OnOrt_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_KDOrt.ButtonClick
		Dim frmTest As New frmSearchRec(2)

		_ClsFunc.Get4What = "MAOrt"
		ClsDataDetail.strButtonValue = "MAOrt"
		ClsDataDetail.Get4What = "MAOrt"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMyValue(_ClsFunc.Get4What)
		Me.txt_KDOrt.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

	Private Sub OnNationality_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANationality.ButtonClick
		Dim frmTest As New frmSearchRec(0)

		_ClsFunc.Get4What = "MANationality"
		ClsDataDetail.strButtonValue = "MANationality"
		ClsDataDetail.Get4What = "MANationality"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMyValue(_ClsFunc.Get4What)
		Me.txt_MANationality.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

	Private Sub OnLand_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MALand.ButtonClick
		Dim frmTest As New frmSearchRec(1)

		_ClsFunc.Get4What = "MALand"
		ClsDataDetail.strButtonValue = "MALand"
		ClsDataDetail.Get4What = "MALand"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMyValue(_ClsFunc.Get4What)
		Me.txt_MALand.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

#End Region


#Region "LblHeader_..."

	'Private Sub LblHeader_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  Dim bIstOpen As Boolean
	'  bIstOpen = Me.p_1.Height > 0

	'  If Not bIstOpen Then
	'    For iPHeight As Integer = Me.P_1.Height To Me.Cbo_BYear_1.Top + Me.Cbo_BYear_1.Height Step 5
	'      P_1.Height = iPHeight
	'      P_1.Refresh()
	'      'RefreshAllPanels()
	'      '        Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_1.Height = Me.Cbo_BYear_1.Top + Me.Cbo_BYear_1.Height
	'    Me.Cbo_BMonth_1.Focus()

	'  Else
	'    For iPHeight As Integer = Me.P_1.Height To Me.Cbo_BYear_1.Top + Me.Cbo_BYear_1.Height Step -5
	'      P_1.Height = iPHeight
	'      P_1.Refresh()
	'      Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.p_1.Height = 0

	'    Me.CboSort.Focus()
	'  End If
	'  'RefreshAllPanels()

	'End Sub

	'Private Sub LblHeader_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  Dim bIstOpen As Boolean
	'  bIstOpen = Me.p_2.Height > 0

	'  If Not bIstOpen Then
	'    For iPHeight As Integer = Me.P_2.Height To Me.Cbo_Filiale.Top + Me.Cbo_Filiale.Height Step 5
	'      P_2.Height = iPHeight
	'      P_2.Refresh()
	'      RefreshAllPanels()
	'      '       Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_2.Height = Me.Cbo_Filiale.Top + Me.Cbo_Filiale.Height
	'    Me.Cbo_RPKst3.Focus()

	'  Else
	'    For iPHeight As Integer = Me.P_2.Height To Me.Cbo_Filiale.Top + Me.Cbo_Filiale.Height Step -5
	'      P_2.Height = iPHeight
	'      P_2.Refresh()
	'      Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.p_2.Height = 0

	'    Me.CboSort.Focus()
	'  End If
	'  RefreshAllPanels()

	'End Sub

	'Private Sub LblHeader_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  Dim bIstOpen As Boolean
	'  bIstOpen = Me.P_3.Height > 0

	'  If Not bIstOpen Then
	'    For iPHeight As Integer = Me.P_3.Height To Me.txt_ESGewerbe.Top + Me.txt_ESGewerbe.Height Step 5
	'      P_3.Height = iPHeight
	'      P_3.Refresh()
	'      RefreshAllPanels()
	'      'Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_3.Height = Me.txt_ESGewerbe.Top + Me.txt_ESGewerbe.Height
	'    Me.txt_ESGewerbe.Focus()

	'  Else
	'    For iPHeight As Integer = Me.P_3.Height To Me.txt_ESGewerbe.Top + Me.txt_ESGewerbe.Height Step 5
	'      P_3.Height = iPHeight
	'      P_3.Refresh()
	'      Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_3.Height = 0

	'    Me.CboSort.Focus()
	'  End If
	'  RefreshAllPanels()

	'End Sub

	'Private Sub LblHeader_4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  Dim bIstOpen As Boolean
	'  bIstOpen = Me.p_4.Height > 0

	'  If Not bIstOpen Then
	'    For iPHeight As Integer = Me.P_4.Height To Me.txt_KDOrt.Top + Me.txt_KDOrt.Height Step 5
	'      P_4.Height = iPHeight
	'      P_4.Refresh()
	'      RefreshAllPanels()
	'      '      Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_4.Height = Me.txt_KDOrt.Top + Me.txt_KDOrt.Height
	'    Me.Cbo_KDKanton.Focus()

	'  Else
	'    For iPHeight As Integer = Me.P_4.Height To Me.txt_KDOrt.Top + Me.txt_KDOrt.Height Step 5
	'      P_4.Height = iPHeight
	'      P_4.Refresh()
	'      Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.p_4.Height = 0

	'    Me.CboSort.Focus()
	'  End If
	'  RefreshAllPanels()

	'End Sub

	'Private Sub LblHeader_5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  Dim bIstOpen As Boolean
	'  bIstOpen = Me.P_5.Height > 0

	'  If Not bIstOpen Then
	'    For iPHeight As Integer = Me.P_5.Height To Me.Label5.Top + Me.Label5.Height Step 5
	'      P_5.Height = iPHeight
	'      P_5.Refresh()
	'      'Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_5.Height = Me.Label5.Top + Me.Label5.Height
	'    Me.txt_MANationality.Focus()

	'  Else
	'    For iPHeight As Integer = Me.P_5.Height To Me.Label5.Top + Me.Label5.Height Step 5
	'      P_5.Height = iPHeight
	'      P_5.Refresh()
	'      Threading.Thread.Sleep(iInterval)
	'      System.Windows.Forms.Application.DoEvents()
	'    Next
	'    Me.P_5.Height = 0

	'    Me.CboSort.Focus()
	'  End If
	'  '    RefreshAllPanels()

	'End Sub

	'Sub RefreshAllPanels()

	'  Me.tbSearch.Refresh()

	'  Me.LblHeader_3.Top = Me.P_2.Top + Me.P_2.Height + 20
	'  Me.P_3.Top = Me.LblHeader_3.Top + 20

	'  Me.LblHeader_4.Top = Me.P_3.Top + Me.P_3.Height + 20
	'  Me.p_4.Top = Me.LblHeader_4.Top + 20

	'  Me.LblHeader_5.Top = Me.P_4.Top + Me.P_4.Height + 20
	'  Me.P_5.Top = Me.LblHeader_5.Top + 20

	'End Sub

#End Region



	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is BaseEdit Then
				If CType(sender, BaseEdit).Properties.ReadOnly Then
					' nothing
				Else
					CType(sender, BaseEdit).EditValue = Nothing

					lueEmployee.Properties.Buttons(0).Enabled = True
					lueEmployee.Properties.Buttons(1).Enabled = True
					lueCustomer.Properties.Buttons(0).Enabled = True
					lueCustomer.Properties.Buttons(1).Enabled = True

				End If
			End If

		End If
	End Sub

	''' <summary>
	''' Handles button click on einsatz.
	''' </summary>
	Private Sub OnLueEinsatz_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueEinsatz.ButtonClick

		If lueEinsatz.EditValue Is Nothing Then
			Return
		End If

		If (e.Button.Index = 2) Then

			Dim hub = MessageService.Instance.Hub
			Dim openEinsatzMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueEinsatz.EditValue)
			hub.Publish(openEinsatzMng)

		End If

	End Sub

	''' <summary>
	''' Handles button click on employee.
	''' </summary>
	Private Sub OnLueEmployee_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueEmployee.ButtonClick

		If lueEmployee.EditValue Is Nothing Then
			Return
		End If

		If (e.Button.Index = 2) Then

			Dim hub = MessageService.Instance.Hub
			Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueEmployee.EditValue)
			hub.Publish(openEmployeeMng)

		End If

	End Sub

	''' <summary>
	''' Handles button click on customer.
	''' </summary>
	Private Sub OnLueCustomer_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueCustomer.ButtonClick

		If lueMandant.EditValue Is Nothing Or lueCustomer.EditValue Is Nothing Then
			Return
		End If

		If (e.Button.Index = 2) Then

			Dim hub = MessageService.Instance.Hub
			Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueCustomer.EditValue)
			hub.Publish(openCustomerMng)

		End If

	End Sub

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_BMonth_1.Visible = Me.SwitchButton1.Value
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.Cbo_BYear_1.Visible = Me.SwitchButton2.Value
	End Sub

	Private Sub frmUmsatz_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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



	Private Sub LoadGridData()
		Dim jahr As Integer?
		Dim lastYear As Integer?

		If Not Cbo_VYear_1.EditValue Is Nothing Then
			jahr = CType(Cbo_VYear_1.EditValue, Integer)
		End If
		If Cbo_BYear_1.Visible AndAlso Not Cbo_BYear_1.EditValue Is Nothing Then
			lastYear = CType(Cbo_BYear_1.EditValue, Integer)
		End If

		Dim vonMonat As Integer?
		If Not Cbo_VMonth_1.EditValue Is Nothing Then
			vonMonat = CType(Cbo_VMonth_1.EditValue, Integer)
		End If
		Dim bisMonat As Integer?
		If Cbo_BMonth_1.Visible AndAlso Not Cbo_BMonth_1.EditValue Is Nothing Then
			bisMonat = CType(Cbo_BMonth_1.EditValue, Integer)
		End If

		jahr = jahr.GetValueOrDefault(Now.Year)
		lastYear = lastYear.GetValueOrDefault(jahr)
		vonMonat = vonMonat.GetValueOrDefault(Now.Month)
		bisMonat = bisMonat.GetValueOrDefault(vonMonat)

		Dim data As ClsGetSQLString = Nothing
		data = New ClsGetSQLString(Nothing)
		Dim ESData = data.ListESData4DB1(vonMonat, bisMonat, jahr, lastYear)

		If Not ESData Is Nothing Then

			Dim reportGridData = (From report In ESData
								  Select New DB1ESData With
											   {.ESNumber = report.ESNumber,
												  .customername = report.customername,
												  .CustomerNumber = report.CustomerNumber,
												  .employeefullname = report.employeefullname,
												  .EmployeeNumber = report.EmployeeNumber,
												  .es_ab = report.es_ab,
												  .es_ende = report.es_ende
											   }).ToList()

			Dim listDataSource As BindingList(Of DB1ESData) = New BindingList(Of DB1ESData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			lueEinsatz.Properties.DataSource = listDataSource

		End If

		Dim EmployeeData = data.ListEmployeeData4DB1(vonMonat, bisMonat, jahr, lastYear)

		If Not EmployeeData Is Nothing Then

			Dim reportGridData = (From report In EmployeeData
								  Select New DB1EmployeeData With
											   {.ESNumber = report.ESNumber,
												  .customername = report.customername,
												  .CustomerNumber = report.CustomerNumber,
												  .employeefullname = report.employeefullname,
												  .EmployeeNumber = report.EmployeeNumber,
												  .es_ab = report.es_ab,
												  .es_ende = report.es_ende
											   }).ToList()

			Dim listDataSource As BindingList(Of DB1EmployeeData) = New BindingList(Of DB1EmployeeData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			lueEmployee.Properties.DataSource = listDataSource

		End If

		Dim CustomerData = data.ListCustomerData4DB1(vonMonat, bisMonat, jahr, lastYear)

		If Not CustomerData Is Nothing Then

			Dim reportGridData = (From report In CustomerData
								  Select New DB1CustomerData With
											   {.ESNumber = report.ESNumber,
												  .customername = report.customername,
												  .CustomerNumber = report.CustomerNumber,
												  .employeefullname = report.employeefullname,
												  .EmployeeNumber = report.EmployeeNumber,
												  .es_ab = report.es_ab,
												  .es_ende = report.es_ende
											   }).ToList()

			Dim listDataSource As BindingList(Of DB1CustomerData) = New BindingList(Of DB1CustomerData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			'Dim listDataSourceData = listDataSource.GroupBy(Function(p) p.CustomerNumber)

			lueCustomer.Properties.DataSource = listDataSource

		End If


	End Sub

	Private Sub cbo_ESNr_EditValueChanged(sender As Object, e As EventArgs) Handles lueEinsatz.EditValueChanged

		If Not lueEinsatz.EditValue Is Nothing Then

			Dim data = SelectedESRecord
			If Not data Is Nothing Then
				lueEmployee.EditValue = data.EmployeeNumber

				lueEmployee.Properties.Buttons(0).Enabled = False
				lueEmployee.Properties.Buttons(1).Enabled = False

				lueCustomer.EditValue = data.CustomerNumber

				lueCustomer.Properties.Buttons(0).Enabled = False
				lueCustomer.Properties.Buttons(1).Enabled = False


				Dim bshow = AllowedToShowEachEmployee(Year(data.es_ab))
				If bshow Then
					Cbo_VYear_1.EditValue = Year(data.es_ab)
					Cbo_BYear_1.EditValue = If(data.es_ende.HasValue, Year(data.es_ende), Now.Year)

					Cbo_VMonth_1.EditValue = Month(data.es_ab)
					Cbo_BMonth_1.EditValue = If(data.es_ende.HasValue, Month(data.es_ende), Now.Month)
					SwitchButton1.Value = True
					SwitchButton2.Value = True

					Cbo_BYear_1.Visible = True
					Cbo_BMonth_1.Visible = True

				Else
					m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Für den gewählten Einsatz kann nicht für die gesamte Einsatzdauer eine DB1-Liste ausgegeben werden (Nicht einheitliche Rückstellungsmethode)."))

				End If

			End If

		End If

	End Sub


End Class
