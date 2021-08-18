
Option Strict Off

Imports System.Data.SqlClient

Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI

'Imports SP.RP.NotDoneList.ClsDataDetail
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.Report
Imports SP

Public Class frmRPNotDoneSearch
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private consts"

	Private Const PRINTJOBNUMBER As String = "10.8"

#End Region


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility
	Private m_Utility_SP As SPProgUtility.MainUtilities.Utilities

	Private frmMyLV As frmRPListSearch_LV

	''' <summary>
	''' The mandant.
	''' </summary>
	''' <remarks></remarks>
	Private m_Mandant As Mandant

	Private m_path As ClsProgPath

	''' <summary>
	''' The current connection string.
	''' </summary>
	Private m_CurrentConnectionString = String.Empty

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
	''' <summary>
	''' The customer database access.
	''' </summary>
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	''' <summary>
	''' The financial Accounts database access.
	''' </summary>
	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

	Private m_ReportDatabaseAccess As IReportDatabaseAccess

	Private m_SearchCriteria As New SearchCriteria
	Private m_SQL4Print As String
	Private m_PrintAsDesign As Boolean

#End Region





#Region "Constructor..."

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_Utility_SP = New SPProgUtility.MainUtilities.Utilities

		' Mandantendaten
		m_Mandant = New Mandant
		m_path = New ClsProgPath

		m_InitializationData = _setting
		'm__InitialData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn

		m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ReportDatabaseAccess = New ReportDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try

		ResetMandantenDropDown()
		LoadMandantenDropDown()

		LoadEmployeeDropDownData()
		LoadCustomerDropDownData()


		TranslateControls()
		Reset()


		Dim sortdata = LoadSortData()
		CboSort.Properties.DataSource = sortdata

		AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
																					 .Width = 100,
																					 .Caption = "Mandant"})

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
		Dim _ClsFunc As New ClsDbFunc(m_InitializationData)
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)

			m_InitializationData = ChangeMandantData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub


#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblsortierung.Text = m_Translate.GetSafeTranslationValue(Me.lblsortierung.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)

		lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(lblMitarbeiter.Text)
		lblFirma.Text = m_Translate.GetSafeTranslationValue(lblFirma.Text)
		Me.lbl1KST.Text = m_Translate.GetSafeTranslationValue(Me.lbl1KST.Text)
		Me.lbl2KST.Text = m_Translate.GetSafeTranslationValue(Me.lbl2KST.Text)
		Me.lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text)
		Me.lblFiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblFiliale.Text)

		Me.lblAktuellQery.Text = m_Translate.GetSafeTranslationValue(Me.lblAktuellQery.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

	End Sub

	Private Sub Reset()

		ResetSortDropDown()

		ResetEmployeeDropDown()
		ResetCustomerDropDown()

	End Sub


	Private Sub ResetSortDropDown()

		CboSort.Properties.DisplayMember = "BezValue"
		CboSort.Properties.ValueMember = "BezValue"

		CboSort.Properties.NullText = String.Empty
		CboSort.EditValue = Nothing

	End Sub

	Private Sub LoadStaticData()

		If Cbo_Month.EditValue Is Nothing OrElse Cbo_Year.EditValue Is Nothing Then Return
		LoadEmployeeDropDownData()
		LoadCustomerDropDownData()

	End Sub

	''' <summary>
	''' Resets the employee drop down.
	''' </summary>
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
		columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
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

		lueEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueEmployee.Properties.NullText = String.Empty
		lueEmployee.EditValue = Nothing

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
		columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
		columnCustomerNumber.Name = "CustomerNumber"
		columnCustomerNumber.FieldName = "CustomerNumber"
		columnCustomerNumber.Visible = True
		columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
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

		lueCustomer.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCustomer.Properties.NullText = String.Empty
		lueCustomer.EditValue = Nothing

	End Sub

	Private Sub lueEmployee_EditValueChanged(sender As Object, e As EventArgs) Handles lueEmployee.EditValueChanged
		LoadCustomerDropDownData()
	End Sub




#Region "Dropdown Funktionen 1. Seite..."

	Function LoadSortData() As IEnumerable(Of SortData)
		Dim result As List(Of SortData) = Nothing

		'0 - Rapportnummer
		'1 - Rapportdatum
		'2 - Kandidatennummer
		'3 - Kandidatenname
		'4 - Kundennummer
		'5 - Kundenname
		result = New List(Of SortData)

		result.Add(New SortData With {.BezValue = String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Rapportnummer"))})
		result.Add(New SortData With {.BezValue = String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Rapportdatum"))})
		result.Add(New SortData With {.BezValue = String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennummer"))})
		result.Add(New SortData With {.BezValue = String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Kandidatenname"))})
		result.Add(New SortData With {.BezValue = String.Format("4 - {0}", m_Translate.GetSafeTranslationValue("Kundennummer"))})
		result.Add(New SortData With {.BezValue = String.Format("5 - {0}", m_Translate.GetSafeTranslationValue("Kundenname"))})

		Return result

	End Function


	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData() As Boolean

		Dim _ClsFunc As New ClsDbFunc(m_InitializationData)
		Dim firstMonth As Integer = Now.Month
		Dim firstYear As Integer = Now.Year
		If Not Cbo_Month.EditValue Is Nothing Then firstMonth = Cbo_Month.EditValue
		If Not Cbo_Year.EditValue Is Nothing Then firstYear = Cbo_Year.EditValue

		Dim employeeData = _ClsFunc.LoadEmployeeData(firstMonth, firstYear)

		If employeeData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnen nicht geladen werden."))
			Return False
		End If

		lueEmployee.EditValue = Nothing
		lueEmployee.Properties.DataSource = employeeData

		Return True

	End Function

	''' <summary>
	''' Loads the customer drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim _ClsFunc As New ClsDbFunc(m_InitializationData)
		Dim firstMonth As Integer = Now.Month
		Dim firstYear As Integer = Now.Year
		If Not Cbo_Month.EditValue Is Nothing Then firstMonth = Cbo_Month.EditValue
		If Not Cbo_Year.EditValue Is Nothing Then firstYear = Cbo_Year.EditValue

		Dim customerData = _ClsFunc.LoadCustomerData(firstMonth, firstYear, lueEmployee.EditValue)

		If customerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))
			Return False
		End If

		lueCustomer.EditValue = Nothing
		lueCustomer.Properties.DataSource = customerData

		Return True

	End Function


	Private Sub Cbo_KST1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST1.QueryPopUp
		ListREKst1(Me.Cbo_KST1, Cbo_Month.EditValue, Cbo_Year.EditValue)
	End Sub

	Private Sub Cbo_KST2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST2.QueryPopUp
		ListREKst2(Me.Cbo_KST2, Cbo_Month.EditValue, Cbo_Year.EditValue)
	End Sub

	Private Sub Cbo_Berater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		ListBerater(Me.Cbo_Berater, Cbo_Month.EditValue, Cbo_Year.EditValue)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		ListRPFiliale(Me.Cbo_Filiale, Cbo_Month.EditValue, Cbo_Year.EditValue, Cbo_Berater.EditValue)
	End Sub

	Private Sub Cbo_Month_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.QueryPopUp
		ListRPMonth(Cbo_Month)
	End Sub

	Private Sub Cbo_Year_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.QueryPopUp
		ListRPYear(Cbo_Year)
	End Sub

	Private Sub OnCbo_Month_SelectedValueChanged(sender As Object, e As EventArgs) Handles Cbo_Month.SelectedValueChanged
		LoadStaticData()
	End Sub

	Private Sub OnCbo_Year_SelectedValueChanged(sender As Object, e As EventArgs) Handles Cbo_Year.SelectedValueChanged
		LoadStaticData()
	End Sub



	Sub ListRPFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?, ByVal berater As String)

		Try
			Dim sql As String
			sql = "[List All UserBranches For Listing In Not Done Reports]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_Utility_SP.ReplaceMissing(m_InitializationData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("kst", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_Utility_SP.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_Utility_SP.ReplaceMissing(jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_Utility_SP.SafeGetString(reader, "Filiale"))

			End While

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		Finally


		End Try

	End Sub

	Sub ListREKst1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?)

		Try

			Dim sql As String
			sql = "SELECT RP.RPKst1 As Bezeichnung From RP "
			sql &= "WHERE RP.RPKst2 Is Not Null And RP.RPKst2 <> '' "
			sql &= "And RP.MDNr = @MDNr "
			sql &= "And RP.Erfasst <> 1 "
			sql &= "And (@Monat = 0 Or RP.Monat = @Monat) "
			sql &= "And (@Jahr = 0 Or RP.Jahr = @Jahr) "

			sql &= "Group By RP.RPKst1 "
			sql &= "Order By RP.RPKst1"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_Utility_SP.ReplaceMissing(m_InitializationData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_Utility_SP.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_Utility_SP.ReplaceMissing(jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)


			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_Utility_SP.SafeGetString(reader, "Bezeichnung"))

			End While

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		End Try

	End Sub

	Sub ListREKst2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?)

		Try

			Dim sql As String
			sql = "SELECT RP.RPKst2 As Bezeichnung From RP "
			sql &= "WHERE RP.RPKst2 Is Not Null And RP.RPKst2 <> '' "
			sql &= "And RP.MDNr = @MDNr "
			sql &= "And RP.Erfasst <> 1 "
			sql &= "And (@Monat = 0 Or RP.Monat = @Monat) "
			sql &= "And (@Jahr = 0 Or RP.Jahr = @Jahr) "

			sql &= "Group By RP.RPKst2 "
			sql &= "Order By RP.RPKst2"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_Utility_SP.ReplaceMissing(m_InitializationData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_Utility_SP.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_Utility_SP.ReplaceMissing(jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)


			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_Utility_SP.SafeGetString(reader, "Bezeichnung"))

			End While

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		End Try

	End Sub

	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?)

		Try

			Dim sql As String
			sql = "SELECT MA.KST, (US.Nachname + ', ' + US.Vorname) As USName From RP "
			sql &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
			sql &= "Left Join Benutzer US On MA.KST = US.KST "
			sql &= "WHERE MA.KST <> '' and (US.Nachname Is Not Null Or US.Nachname <> '') "
			sql &= "And RP.MDNr = @MDNr "
			sql &= "And RP.Erfasst <> 1 "
			sql &= "And (@Monat = 0 Or RP.Monat = @Monat) "
			sql &= "And (@Jahr = 0 Or RP.Jahr = @Jahr) "
			sql &= "And (@Filiale = '' Or USFiliale = @Filiale) "

			sql &= "Group By MA.KST, "
			sql &= "US.Nachname + ', ' + US.Vorname "
			sql &= "Order By MA.KST, (US.Nachname + ', ' + US.Vorname)"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_Utility_SP.ReplaceMissing(m_InitializationData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_Utility_SP.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_Utility_SP.ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", m_Utility_SP.ReplaceMissing(m_InitializationData.UserData.UserFiliale, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(New ComboValue(m_Utility_SP.SafeGetString(reader, "USName"), m_Utility_SP.SafeGetString(reader, "KST")))
			End While
			cbo.Properties.DropDownRows = 20


		Catch e As Exception
			m_Logger.LogError(e.ToString())

		End Try

	End Sub

	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal filiale As String)
		Dim strSqlQuery As String = "[List AllBerater For Search In RPList]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@filiale", filiale)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rESrec.Read
				cbo.Properties.Items.Add(New ComboValue(rESrec("USName").ToString, rESrec("KST").ToString))

			End While
			cbo.Properties.DropDownRows = 15

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListRPMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetRPLP]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_Utility_SP.SafeGetInteger(reader, "Monat", 0))

				i += 1
			End While
			cbo.Properties.DropDownRows = 12

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListRPYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetRPYear]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_Utility_SP.SafeGetInteger(reader, "Jahr", 0))

				i += 1
			End While
			cbo.Properties.DropDownRows = 10

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub


	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rPLZrec.Read
				strPLZResult += rPLZrec(strFieldName).ToString & ","

			End While

			If strPLZResult.Length > 1 Then
				strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
				strPLZResult = Replace(strPLZResult, ",", "','")
			Else
				strPLZResult = String.Empty
			End If

		Catch e As Exception
			strPLZResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function


#End Region




	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmZGSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmRPListSearch_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width
				My.Settings.frm_Sort = CboSort.EditValue

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub


	Private Sub OnFormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSort As String = My.Settings.frm_Sort
		Me.CboSort.EditValue = strSort

		If String.IsNullOrWhiteSpace(strSort) Then
			Dim sortData = CboSort.Properties.DataSource
			If Not sortData Is Nothing Then

				For Each itm As SortData In sortData
					strSort = itm.BezValue

					Exit For
				Next

			End If
		End If
		Me.CboSort.EditValue = strSort

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		SetBeraterValues()
		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Dim showMDSelection As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
			Me.lueMandant.Visible = showMDSelection
			Me.lblMDName.Visible = showMDSelection

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

		Me.Cbo_Year.EditValue = Now.Year
		Me.Cbo_Month.EditValue = If(Now.Day > 10, Now.Month, Now.Month - 1)

		Me.xtabSQLAbfrage.PageVisible = m_InitializationData.UserData.UserNr = 1

		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	Sub SetBeraterValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_common As New CommonSetting

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 672, m_InitializationData.MDData.MDNr) Then
			Try
				Me.Cbo_Filiale.Enabled = False
				Me.Cbo_KST1.Enabled = False
				Me.Cbo_KST2.Enabled = False
				Dim strUSTitle As String = GetUSTitle(m_InitializationData.UserData.UserNr)

				Me.Cbo_Berater.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
				Me.Cbo_Filiale.Text = m_common.GetLogedUserFiliale
				ListBerater(Me.Cbo_Berater, Me.Cbo_Filiale.Text)


				For Each item As CheckedListBoxItem In Me.Cbo_Berater.Properties.Items
					Dim cv As ComboValue = DirectCast(item.Value, ComboValue)
					Dim strKst As String = cv.ComboValue.Trim
					Dim strUserName As String = cv.Text.Trim
					If strUserName.ToLower.Contains(String.Format("{0}, {1}", m_common.GetLogedUserLastName, m_common.GetLogedUserFirstName).ToLower) Then
						item.CheckState = CheckState.Checked
						Me.Cbo_Berater.Text = cv.Text
						Exit For
					End If

				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BenutzerTitel auflisten: {1}", strMethodeName, ex.Message))
			End Try

		End If

	End Sub

	'''' <summary>
	'''' Daten fürs Drucken bereit stellen.
	'''' </summary>
	'''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	'''' <param name="bForExport">ob die Liste für Export ist.</param>
	'''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	'''' <remarks></remarks>
	'Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'	Dim iKDNr As Integer = 0
	'	Dim iKDZNr As Integer = 0
	'	Dim bResult As Boolean = True
	'	Dim bWithKD As Boolean = True

	'	Dim sSql As String = ClsDataDetail.GetSQLQuery()
	'	If sSql = String.Empty Then
	'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"))

	'		Return
	'	End If

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'	Try
	'		Conn.Open()

	'		Dim rZGrec As SqlDataReader = cmd.ExecuteReader
	'		Try
	'			If Not rZGrec.HasRows Then
	'				cmd.Dispose()
	'				rZGrec.Close()

	'				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
	'				Return
	'			End If

	'		Catch ex As Exception
	'			m_UtilityUI.ShowErrorDialog(ex.ToString)

	'			Return
	'		End Try

	'		rZGrec.Read()
	'		If rZGrec.HasRows Then
	'			Me.m_SQL4Print = sSql

	'			StartPrinting()

	'		End If
	'		rZGrec.Close()

	'	Catch ex As Exception
	'		m_UtilityUI.ShowErrorDialog(ex.ToString)

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try

	'End Sub

	Sub StartPrinting()
		Dim bShowDesign As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 614, m_InitializationData.MDData.MDNr)
		Dim ShowDesign As Boolean = bShowDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLRPNotFinishedPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																											 .bAsDesign = ShowDesign,
																																											 .SQL2Open = Me.m_SQL4Print,
																																											 .JobNr2Print = PRINTJOBNUMBER, .frmhwnd = GetHwnd,
																																											 .ListSortBez = ClsDataDetail.GetSortBez,
																																											 .ListFilterBez = New List(Of String)(New String() _
																																																														{ClsDataDetail.GetFilterBez,
																																																														 ClsDataDetail.GetFilterBez2,
																																																														 ClsDataDetail.GetFilterBez3,
																																																														 ClsDataDetail.GetFilterBez4}),
																																											 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																											 .LogedUSNr = m_InitializationData.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.RPNotFinishedListing.ClsPrintRPNotFinishedList(_Setting)

		obj.PrintRPNotFinishedList()

	End Sub

#Region "Funktionen zur Menüaufbau..."


	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick

		Try
			m_SearchCriteria = GetSearchKrieteria()

			Me.txt_SQL_1.Text = String.Empty
			ClsDataDetail.GetSQLQuery() = String.Empty
			ClsDataDetail.GetSQLSortString() = String.Empty

			FormIsLoaded("frmRPListSearch_LV", True)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			GetMyQueryString()


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim vonMonth As Integer? = Now.Month
		Dim vonYear As Integer? = Now.Year

		If Not Cbo_Month.EditValue Is Nothing Then vonMonth = Val(Cbo_Month.EditValue)
		If Not Cbo_Year.EditValue Is Nothing Then vonYear = Val(Cbo_Year.EditValue)

		result.SortIn = CboSort.EditValue
		result.mandantenname = lueMandant.EditValue

		result.EmployeeNumber = lueEmployee.EditValue
		result.CustomerNumber = lueCustomer.EditValue

		result.FromMonth = vonMonth
		result.FromYear = vonYear

		result.kst1 = Cbo_KST1.EditValue
		result.kst2 = Cbo_KST2.EditValue
		Dim berater As String = String.Empty

		For Each item As CheckedListBoxItem In Cbo_Berater.Properties.Items
			If item.CheckState = CheckState.Checked Then
				Dim cv = DirectCast(item.Value, ComboValue)
				Dim strKst As String = cv.ComboValue.Trim.Replace("'", "").Replace("*", "%")
				berater &= If(String.IsNullOrWhiteSpace(berater), "", ",") & strKst
			End If

		Next

		result.Berater = berater ' Cbo_Berater.EditValue
		result.filiale = Cbo_Filiale.EditValue

		Return result

	End Function

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty
		Dim _ClsDb As New ClsDbFunc(m_InitializationData)

		If ClsDataDetail.GetSQLQuery() = String.Empty Then
			Me.bbiSearch.Enabled = False

			'_ClsDb.GetJobNr4Print(Me)
			sSql1Query = _ClsDb.GetStartSQLString()    ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, m_SearchCriteria)    ' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If

			ClsDataDetail.GetSQLQuery = sSql1Query + sSql2Query & " Order By RP.RPNr"
			strSort = _ClsDb.GetSortString(m_SearchCriteria)     ' Sort Klausel
			ClsDataDetail.GetSQLSortString = strSort

			Me.txt_SQL_1.Text = ClsDataDetail.GetSQLQuery


			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		End If

		Return True

	End Function

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		FormIsLoaded("frmRPListSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		' Cbo leeren...
		Cbo_Month.EditValue = Nothing
		Cbo_Year.EditValue = Nothing
		Cbo_KST1.EditValue = Nothing
		Cbo_KST2.EditValue = Nothing
		Cbo_Berater.EditValue = Nothing
		Cbo_Berater.Properties.Items.Clear()
		Cbo_Filiale.EditValue = Nothing

		Cbo_Year.EditValue = Now.Year
		Cbo_Month.EditValue = If(Now.Day > 10, Now.Month, Now.Month - 1)

		Reset()

		txt_SQL_1.EditValue = String.Empty
		txt_SQL_2.EditValue = String.Empty

		'For Each cControl As Control In Me.Controls
		'	If TypeOf (cControl) Is DevExpress.XtraEditors.ComboBoxEdit Then
		'		Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = cControl
		'		cbo.EditValue = Nothing
		'		cbo.Properties.Items.Clear()

		'	ElseIf TypeOf (cControl) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
		'		Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = cControl
		'		cbo.EditValue = Nothing
		'		cbo.Properties.Items.Clear()

		'	ElseIf TypeOf (cControl) Is DevExpress.XtraEditors.MemoEdit Then
		'		Dim cbo As DevExpress.XtraEditors.MemoEdit = cControl
		'		cbo.Text = String.Empty
		'	End If

		'Next

		ClsDataDetail.GetSQLQuery() = String.Empty
		ClsDataDetail.GetSQLSortString() = String.Empty

	End Sub

#End Region



	'Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

	'	For Each Ctrl As Control In Ctrls
	'		AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
	'		'      AddHandler Ctrl.Click, AddressOf ClickEvents
	'	Next

	'End Sub

	'Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)	' System.EventArgs)
	'	'   ToDo  Auswertung und Klick-Aktion ausführen
	'	'If sender Is TextBox1 Then

	'	Try
	'		If e.KeyChar = Chr(13) Then
	'			SendKeys.Send("{tab}")
	'			e.Handled = True
	'		End If

	'	Catch ex As Exception
	'		MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
	'	End Try

	'	'End If
	'End Sub

	Private Sub frmZGSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmRPListSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
		Dim _ClsDb As New ClsDbFunc(m_InitializationData)
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty


		CheckForIllegalCrossThreadCalls = False
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		_ClsDb.BuildRPDayDb(ClsDataDetail.GetSQLQuery) 'sSql1Query + sSql2Query & " Order By RP.RPNr")
		sSql1Query = _ClsDb.GetStartSQLString_2()    ' 2. String für Drucken (die Whereklausel kommt nicht mehr.
		ClsDataDetail.GetSQLQuery() = sSql1Query + ClsDataDetail.GetSQLSortString()

		e.Result = True
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			m_UtilityUI.ShowErrorDialog(e.Error.ToString)
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Vorgang wurde abgebrochen."))

			Else
				BackgroundWorker1.CancelAsync()

				If Not FormIsLoaded("frmRPListSearch_LV", True) Then
					frmMyLV = New frmRPListSearch_LV(m_InitializationData, ClsDataDetail.GetSQLQuery(), Me.Location.X, Me.Location.Y, Me.Height)

					frmMyLV.Show()
					Me.Select()

					Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																						 frmMyLV.RecCount)

				End If
				Me.bbiSearch.Enabled = True
				Me.txt_SQL_2.Text = ClsDataDetail.GetSQLQuery()

				If frmMyLV.gvRP.RowCount > 0 Then
					Me.bbiPrint.Enabled = True
					Me.bbiExport.Enabled = True

					'CreatePrintPopupMenu()
					CreateExportPopupMenu()
				End If

			End If
		End If

	End Sub

#End Region


#Region "Contextmenü für Print und Export..."

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

		Me.m_SQL4Print = String.Empty
		Me.m_PrintAsDesign = False

		m_PrintAsDesign = False
		m_SQL4Print = ClsDataDetail.GetSQLQuery()

		StartPrinting()

		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl
		'popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {If(IsUserActionAllowed(m_InitializationData.UserData.UserNr, 604, m_InitializationData.MDData.MDNr), "Liste drucken#mnuRPListPrint", "")}

	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))
	'			bshowMnu = myValue(0).ToString <> String.Empty

	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

	'				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try
	'End Sub

	'Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

	'	Me.m_SQL4Print = String.Empty
	'	Me.m_PrintAsDesign = False

	'	Select Case e.Item.Name.ToUpper
	'		Case "mnuRPListPrint".ToUpper
	'			m_PrintAsDesign = False
	'			m_SQL4Print = ClsDataDetail.GetSQLQuery()
	'			StartPrinting()

	'			GetData4Print(False, False, ClsDataDetail.GetModulToPrint())

	'		Case "PrintDesign".ToUpper
	'			GetData4Print(True, False, ClsDataDetail.GetModulToPrint())


	'		Case Else
	'			Exit Sub

	'	End Select

	'End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Alle Daten in CSV- / TXT exportieren...#CSV", "Alle Daten für eCall™-SMS-Versand exportieren......#eCall-SMS"}

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
				bshowMnu = True
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = ClsDataDetail.GetSQLQuery()

		Select Case UCase(e.Item.Name.ToUpper)

			Case UCase("eCall-SMS")
				Dim sql As String
				sql = "Select MA.MANr, MA.Nachname, MA.Vorname, "
				sql &= "	( "
				sql &= "Case MA.MA_SMS_Mailing "
				sql &= "When 0 Then MA.Natel Else '' End) As Natel, "
				sql &= "MA.Geschlecht, "
				sql &= "mak.Briefanrede AS Anredeform, "
				sql &= "MA.Strasse, MA.Land, MA.Plz, MA.Ort "
				sql &= "From RPDayDb R "
				sql &= "LEFT JOIN dbo.RP ON RP.RPNr = R.RPNr "
				sql &= "LEFT JOIN dbo.Mitarbeiter MA ON MA.manr = RP.manr "
				sql &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = RP.manr "
				sql &= "Where USNr = {0} And (MA.Natel <> '' And MA.Natel Is Not Null ) And MA.MA_SMS_Mailing <> 1 "
				sql &= "Order by MA.Nachname, MA.Vorname"

				strSQL = String.Format(sql, m_InitializationData.UserData.UserNr)

				Call RuneCallSMSModul(strSQL)


			Case UCase("TXT"), UCase("CSV")
				StartExportESModul()

		End Select

	End Sub

	Sub StartExportESModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																					 .SQL2Open = ClsDataDetail.GetSQLQuery(),
																																					 .ModulName = "RPNotfinishedSearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromRPNotFoundedSearchListing(ClsDataDetail.GetSQLQuery())

	End Sub

	Sub RuneCallSMSModul(ByVal strTempSQL As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = m_InitializationData.MDData,
																																				 .PersonalizedData = m_InitializationData.ProsonalizedData,
																																				 .TranslationData = m_InitializationData.TranslationData,
																																				 .UserData = m_InitializationData.UserData}

			Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, strTempSQL, SPS.Export.Listing.Utility.ReceiverType.Employee)
			frmSMS2eCall.LoadData()

			frmSMS2eCall.Show()


		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			m_UtilityUI.ShowErrorDialog(e.Message)

		End Try

	End Sub

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
				End If
			End If

		End If
	End Sub

	Private Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlauden
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function




End Class

