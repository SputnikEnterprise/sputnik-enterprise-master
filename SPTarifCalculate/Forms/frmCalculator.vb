

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports DevExpress.XtraEditors
Imports DevExpress.Utils.Menu
Imports DevExpress.LookAndFeel
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPTarifCalculator.ClsDataDetail
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Common
Imports System.ComponentModel
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Public Class frmCalculator


#Region "Constants"

	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "Private Fields"

	Protected Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_ESDatabaseAccess As IESDatabaseAccess

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	Protected m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	Private _ClsFunc As New ClsDivFunc
	Private PublicationOfGAV As Date

	Private bNoPVL As Boolean
	Private strSputnikMessage As String = String.Empty
	Private _modul As String = String.Empty
	Private m_employeeNumber As Integer
	Private _kdnr As Integer = 0
	Private _esnr As Integer = 0
	Private _kanton As String = "AG"
	Private _ESData As String = String.Empty
	Private _MAAlter As String = "19"
	Private _KDPLZ As String = "5000"

	Private _cFAGAnhang1 As Single
	Private _cFANAnhang1 As Single

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

	Private aLblControls As New ArrayList()
	Private aLblLODataControls As New ArrayList()
	Private aCboControls As New ArrayList()
	Private aCategoryValuesNr As New List(Of String)

	Private liGAVBerurfe As List(Of String)
	Private liGAVCategories As List(Of String)
	Private liLOData As New List(Of String)

	Private m_path As ClsProgPath
	Private m_md As Mandant
	Private m_UtilityUI As New UtilityUI

	Private m_BaseTableData As BaseTable.SPSBaseTables
	Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)

#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		m_md = New Mandant
		m_path = New ClsProgPath
		m_UtilityUI = New UtilityUI

		Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_ESDatabaseAccess = New ESDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitialData.MDData.MDDbConn, m_InitialData.UserData.UserLanguage)

		m_BaseTableData = New SPSBaseTables(ClsDataDetail.m_InitialData)
		m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(ClsDataDetail.m_InitialData.UserData.UserLanguage)

		pd = New Printing.PrintDocument
		bNoPVL = IsNoPVL

		Reset()

		LoadEmployeeDropDownData()
		LoadCustomerDropDownData()

		AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick


	End Sub

#End Region


#Region "Private properties"

	Private Property _bResorpflichtig As Boolean

	Private ReadOnly Property IsNoPVL() As Boolean
		Get

			Dim mandantNumber As Integer = m_InitialData.MDData.MDNr
			Dim companyallowednopvl As Boolean? = m_path.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																												 String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)

			Return If(companyallowednopvl Is Nothing, False, companyallowednopvl)

		End Get
	End Property

#End Region


	Private Sub Reset()

		m_employeeNumber = 0
		Me.LblInfo_0.Text = String.Empty
		Me.LblInfo_1.Text = String.Empty

		Me.LblInfo_5.Text = String.Empty
		Me.LblInfo_6.Text = String.Empty

		Me.LblInfo_7.Text = String.Empty
		Me.LblInfo_8.Text = String.Empty
		Me.lblCurrentDate.Text = Now.ToUniversalTime

		Me.Op_CHF.Checked = True
		CalcAGBeitrag(Me.LblInfo_7.Text)
		Me.LblMargeProz.Text = String.Format("{0:n4} %", 0)

		Me.bbiGAVInfo.Enabled = False
		lblBewilligungValue.Text = String.Empty
		lblBewilligungValue.Appearance.Options.UseImage = False
		Cbo_Kanton.Text = m_InitialData.MDData.MDCanton
		cbo_Gruppe0.Properties.DropDownRows = 30

		ResetEmployeeDropDown()
		ResetCustomerDropDown()

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


	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadEmployeeDropDownData() As Boolean

		Dim employeeData = m_ESDatabaseAccess.LoadEmployeeData()

		If employeeData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnen nicht geladen werden."))
			Return False
		End If

		lueEmployee.EditValue = Nothing
		lueEmployee.Properties.DataSource = employeeData

		Return True

	End Function

	''' <summary>
	''' Loads the employee drop down data.
	''' </summary>
	Private Function LoadCustomerDropDownData() As Boolean

		Dim customerData = m_ESDatabaseAccess.LoadCustomerData(m_InitialData.UserData.UserFiliale)

		If customerData Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))
			Return False
		End If

		lueCustomer.EditValue = Nothing
		lueCustomer.Properties.DataSource = customerData

		Return True

	End Function

	''' <summary>
	''' Handles change of employee.
	''' </summary>
	Private Sub OnLueEmployee_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueEmployee.EditValueChanged
		Dim employeeGender = String.Empty

		lblBewilligungValue.Text = String.Empty
		lblBewilligungValue.Appearance.Options.UseImage = False
		m_employeeNumber = 0
		_MAAlter = 0

		If Not lueEmployee.EditValue Is Nothing Then

			m_employeeNumber = lueEmployee.EditValue

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_employeeNumber, False)
			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_employeeNumber)

			If employeeMasterData Is Nothing Or employeeContactCommData Is Nothing Then
				employeeGender = employeeMasterData.Gender

				If employeeMasterData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeitestammdaten konnten nicht geladen werden."))
				End If

				If employeeContactCommData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter (KontaktKomm) konnten nicht geladen werden."))
				End If

			Else

				Dim employeePermissionCode = employeeMasterData.Permission
				If Not String.IsNullOrWhiteSpace(employeePermissionCode) AndAlso Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
					Dim bewData = m_PermissionData.Where(Function(x) x.Code = employeePermissionCode).FirstOrDefault()
					If Not bewData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewData.Translated_Value) Then employeePermissionCode = String.Format("{0} - {1}", bewData.Code, bewData.Translated_Value)
				End If
				lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", employeePermissionCode, employeeMasterData.PermissionToDate)
				'lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", m_CommonDatabaseAccess.TranslatePermissionCode(employeeMasterData.Permission, m_InitialData.UserData.UserLanguage), employeeMasterData.PermissionToDate)

				' Bewilligung warn icon
				lblBewilligungValue.Appearance.Options.UseImage = If(employeeMasterData.PermissionToDate.HasValue AndAlso
																				(employeeMasterData.PermissionToDate.Value.Date < DateTime.Now.Date) AndAlso
																				Not String.IsNullOrWhiteSpace(employeeMasterData.Permission), True, False)

				_MAAlter = GetAge(employeeMasterData.Birthdate)

			End If

		Else
			lblBewilligungValue.Text = String.Empty
		End If
		CalcAGBeitrag(employeeGender)
		CalculateTarif()

	End Sub

	''' <summary>
	''' Handles change of employee.
	''' </summary>
	Private Sub OnlueCustomer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomer.EditValueChanged

		_kdnr = 0
		ResetAllData()
		If Not lueCustomer.EditValue Is Nothing Then

			Dim customerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(lueCustomer.EditValue, m_InitialData.UserData.UserFiliale)

			If customerData Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Kundendaten konnten nicht geladen werden.")
			End If

			If customerData.CreditWarning.GetValueOrDefault(False) AndAlso
				(customerData.CreditLimit1 > 0 AndAlso customerData.OpenInvoiceAmount >= customerData.CreditLimit1) Or
				(customerData.CreditLimit2 > 0 AndAlso customerData.OpenInvoiceAmount >= customerData.CreditLimit2) Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Achtung: Kunden-Kreditlimite wurde erreicht oder überschritten.{0}Offener Debitorenbetrag: {1:n2}{0}1. Kunden-Kreditlimite: {2:n2}{0}2. Kunden-Kreditlimite: {3:n2}")
				msg = String.Format(msg, vbNewLine, customerData.OpenInvoiceAmount, customerData.CreditLimit1, customerData.CreditLimit2)

				m_UtilityUI.ShowInfoDialog(msg)
			End If

			Dim mdCanton As String = m_md.GetMDData4SelectedMD(m_InitialData.MDData.MDNr, DateTime.Now.Year).MDCanton
			Dim cantonFromDB = m_CommonDatabaseAccess.LoadCantonByPostCode(customerData.Postcode)

			Dim canton As String = If(Not String.IsNullOrWhiteSpace(cantonFromDB), cantonFromDB, mdCanton)
			Cbo_Kanton.Text = canton
			_kdnr = customerData.CustomerNumber

		Else

			Cbo_Kanton.Text = m_InitialData.MDData.MDCanton

		End If

	End Sub

	''' <summary>
	''' Handles button click on employee.
	''' </summary>
	Private Sub OnLueEmployee_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueEmployee.ButtonClick

		If (e.Button.Index = 2) Then

			Dim hub = MessageService.Instance.Hub
			Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, lueEmployee.EditValue)
			hub.Publish(openEmployeeMng)

		End If

	End Sub

	''' <summary>
	''' Handles button click on customer.
	''' </summary>
	Private Sub OnLueCustomer_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueCustomer.ButtonClick

		If (e.Button.Index = 2) Then

			Dim hub = MessageService.Instance.Hub
			Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, lueCustomer.EditValue)
			hub.Publish(openCustomerMng)

		End If

	End Sub


#Region " Form drucken "

	' create a printing component
	Private WithEvents pd As Printing.PrintDocument
	' storage for form image
	Dim formImage As Bitmap
	Private Declare Function BitBlt Lib "gdi32.dll" Alias "BitBlt" (
				ByVal hdcDest As IntPtr, ByVal nXDest As Integer, ByVal _
				nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight _
				As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As _
				Integer, ByVal nYSrc As Integer, ByVal dwRop As _
				System.Int32) As Long

	Private Const SRCCOPY As Integer = &HCC0020
	Dim memoryImage As Bitmap

	' Callback from PrintDocument component to do the actual printing
	Private Sub pd_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles pd.PrintPage
		e.Graphics.DrawImage(formImage, 10, 10)
	End Sub

	Private Sub GetFormImage()
		Dim g As Graphics = Me.CreateGraphics()
		Dim s As Size = Me.Size
		formImage = New Bitmap(s.Width, s.Height, g)
		Dim mg As Graphics = Graphics.FromImage(formImage)
		Dim dc1 As IntPtr = g.GetHdc
		Dim dc2 As IntPtr = mg.GetHdc
		' added code to compute and capture the form 
		' title bar and borders 
		Dim widthDiff As Integer = (Me.Width - Me.ClientRectangle.Width)
		Dim heightDiff As Integer = (Me.Height - Me.ClientRectangle.Height)
		Dim borderSize As Integer = widthDiff \ 2
		Dim heightTitleBar As Integer = heightDiff - borderSize

		' Mit Titlebar!!!
		' BitBlt(dc2, 0, 0, Me.ClientRectangle.Width + widthDiff, Me.ClientRectangle.Height + heightDiff, dc1, 0 - borderSize, 0 - heightTitleBar, 13369376)

		BitBlt(dc2, 0, 0,
			 Me.ClientRectangle.Width - widthDiff,
			 Me.ClientRectangle.Height - heightDiff, dc1,
			 0 + borderSize, 0 + heightTitleBar, 13369376)

		g.ReleaseHdc(dc1)
		mg.ReleaseHdc(dc2)

	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

		lblCurrentDate.Text = Now.ToUniversalTime

		GetFormImage()
		pd.Print()

	End Sub

#End Region


#Region "Funktionen für Form..."

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmCalculator_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

		Try
			If Me.WindowState = FormWindowState.Minimized Then Exit Sub

			My.Settings.frmLocation = String.Empty
			My.Settings.iLeft = Me.Top
			My.Settings.iTop = Me.Left
			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmCalculator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Me.LblState_1.Width = Me.Label1.Left

	End Sub


#End Region



#Region "Funktionen für GAV-Auswahl..."

	Private Sub Cbo_Gruppe0_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_Gruppe0.QueryPopUp

		Me._kanton = Me.Cbo_Kanton.Text

		FillPVLBerufe_DS()

	End Sub

	Sub FillPVLBerufe_DS()
		Dim bResor As Boolean = False

		cbo_Gruppe0.Properties.Items.Clear()
		ResetAllLBL()

		Dim liKDBerufe As List(Of String) = GetKDGAVListe(Me._kdnr)

		' Dataset-Variante...
		Dim ds As New DataSet
		Dim dt As DataTable
		Dim strCustomer_ID As String = String.Empty

		ClearGAVBerufInfoFields()

		Dim Time_1 As Double = System.Environment.TickCount
		ds = GetPVLBerufe_DS(Me.Cbo_Kanton.Text, _KDPLZ)
		dt = ds.Tables("PVL_Online")
		If IsNothing(dt) Then
			m_UtilityUI.ShowErrorDialog("Es ist ein Fehler in der Datenbank aufgetretten. Bitte kontaktieren Sie Ihrem Softwarehersteller.")

			Return
		End If
		Time_1 = System.Environment.TickCount
		Dim bShowGAVBeruf As Boolean = If(CInt(Me._kdnr) = 0, True, False)
		Dim iLengthofString As Integer = 0

		For i As Integer = 0 To dt.Rows.Count - 1
			Dim strGAVNr As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "gav_number", "0")
			Dim strGavBeruf As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "Name_de", "")
			Dim strMetaNr As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "id_meta", "0")
			Dim strGAVISAVE As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "ave", "0")

			For j As Integer = 0 To liKDBerufe.Count - 1
				If liKDBerufe(j) = strGAVNr Then
					bShowGAVBeruf = True
					Exit For
				End If
			Next
			If bNoPVL Then
				If bShowGAVBeruf Then
					bShowGAVBeruf = CBool(strGAVISAVE) And CInt(strGAVNr) <> 815001
				End If
				If bShowGAVBeruf Then strGavBeruf = strGavBeruf.Replace("Personalverleih", String.Empty).Trim()
			End If

			If bShowGAVBeruf Then
				Dim strGAVUniaAb As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "unia_validity_start", "")
				Dim strGAVUniaEnd As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "unia_validity_end", "")   ' aGAVValues(5)
				Dim strGAVAveAb As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "ave_validity_start", "") '  aGAVValues(6)
				Dim strGAVAveEnd As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "ave_validity_end", "") '  aGAVValues(7)
				Dim strGAVPubOn As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "publication_date", "") '  aGAVValues(8)
				Dim strGAVValidAb As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "validity_start_date", "") '  aGAVValues(9)
				Dim strGAVState As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "State", "") '  aGAVValues(10)

				Try
					PublicationOfGAV = CType(strGAVPubOn, Date)
				Catch ex As Exception
					PublicationOfGAV = Nothing
				End Try

				Dim strGAVStdWeek As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "StdWeek", "") '   aGAVValues(13)
				Dim strGAVStdMonth As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "StdMonth", "")   '   aGAVValues(14)
				Dim strGAVStdYear As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "StdYear", "") '   aGAVValues(15)

				Dim strGAVFAG As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "FAG", "0")
				Dim strGAVFAN As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "FAN", "0")
				Dim strGAVWAG As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "WAG", "0")
				Dim strGAVWAN As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "WAN", "0")
				Dim strGAVVAG As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "VAG", "0")
				Dim strGAVVAN As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "VAN", "0")

				Dim strGAVFAG_ As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_FAG", "0")
				Dim strGAVFAN_ As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_FAN", "0")

				Dim strGAVKanton4FAR As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "GAVKanton", "")
				bResor = False
				If Not String.IsNullOrWhiteSpace(strGAVKanton4FAR) Then
					'If Val(strGAVFAG) + Val(strGAVFAN) = 0 Then
					If strGAVKanton4FAR.ToUpper.Contains(String.Format("#{0}#", Me.Cbo_Kanton.Text.ToUpper)) Then
						strGAVFAG = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "Resor_FAG", "0")
						strGAVFAN = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "Resor_FAN", "0")
						bResor = Val(strGAVFAG) + Val(strGAVFAG) > 0 ' True
						'End If
					End If
				End If

				Dim strGAVWAG_ As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAG", "0")
				Dim strGAVWAN_ As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAN", "0")
				Dim strGAVWAG_S As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAG_s", "0")
				Dim strGAVWAN_S As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAN_s", "0")
				Dim strGAVWAG_J As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAG_J", "0")
				Dim strGAVWAN_J As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_WAN_J", "0")
				Dim strGAVVAG_ As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAG", "0")
				Dim strGAVVAN_ As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAN", "0")
				Dim strGAVVAG_S As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAG_s", "0")
				Dim strGAVVAN_S As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAN_s", "0")
				Dim strGAVVAG_J As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAG_J", "0")
				Dim strGAVVAN_J As String = ClsDataDetail.GetColumnTextStr(dt.Rows(i), "_VAN_J", "0")

				With cbo_Gruppe0
					.Properties.Items.Add(New ComboBoxItem(If(strGavBeruf = String.Empty, strMetaNr, strGavBeruf),
																					strMetaNr,
																					strGAVNr,
																					strGAVISAVE,
																					strGAVUniaAb,
																					strGAVUniaEnd,
																					strGAVAveAb,
																					strGAVAveEnd,
																					strGAVPubOn,
																					strGAVValidAb,
																					strGAVState,
																					strGAVStdWeek,
																					strGAVStdMonth,
																					strGAVStdYear,
																					strGAVFAG,
																					strGAVFAN,
																					strGAVWAG,
																					strGAVWAN,
																					strGAVVAG,
																					strGAVVAN,
																					strGAVWAG_,
																					strGAVWAN_,
																					strGAVWAG_S,
																					strGAVWAN_S,
																					strGAVWAG_J,
																					strGAVWAN_J,
																					strGAVVAG_,
																					strGAVVAN_,
																					strGAVVAG_S,
																					strGAVVAN_S,
																					strGAVVAG_J,
																					strGAVVAN_J,
																					strGAVFAG_,
																					strGAVFAN_,
																					bResor.ToString,
																					PublicationOfGAV))

				End With

			End If

			bShowGAVBeruf = If(CInt(Me._kdnr) = 0, True, False)
		Next

	End Sub

	Private Sub Cbo_Gruppe0_SelectedIndexChanged(ByVal sender As System.Object,
																							 ByVal e As System.EventArgs) Handles cbo_Gruppe0.SelectedIndexChanged

		Dim cv As ComboBoxItem = DirectCast(cbo_Gruppe0.SelectedItem, ComboBoxItem)
		Dim strMetaNr As String = cv.Value_0
		Dim strGAVNr As String = cv.Value_1
		Dim strGAVGeltInfo As String = cv.Value_3

		Me.lblGAVNr.Text = strGAVNr
		Me.lblMetaNr.Text = strMetaNr
		If Val(Me.lblMetaNr.Text) = 0 Then bbiGAVInfo.Enabled = False : Return
		bbiGAVInfo.Enabled = True

		Me.lblJStd.Text = String.Format("{0:F0} | {1:F0} | {2:n0}",
																		Val(cv.Value_10),
																		Val(cv.Value_11),
																		Val(cv.Value_12))

		If Not bNoPVL Then
			' ist PVL
			Me.lblFAG.Text = String.Format("{0}", Val(cv.Value_13))
			Me.lblFAN.Text = String.Format("{0}", Val(cv.Value_14))
			Me.lblPAG.Text = String.Format("{0}", Val(cv.Value_17))
			Me.lblPAN.Text = String.Format("{0}", Val(cv.Value_18))

		Else
			' Ist Inkassopool!
			Me.lblFAG.Text = String.Format("{0}", Val(cv.Value_31))
			Me.lblFAN.Text = String.Format("{0}", Val(cv.Value_32))
			Me.lblPAG.Text = String.Format("{0}", Val(cv.Value_19) + Val(cv.Value_25))
			Me.lblPAN.Text = String.Format("{0}", Val(cv.Value_20) + Val(cv.Value_26))

		End If
		Me._bResorpflichtig = CBool(cv.Value_33)

		Me._cWAGAnhang1 = cv.Value_19
		Me._cWANAnhang1 = cv.Value_20
		Me._cWAGAnhang1_S = cv.Value_21
		Me._cWANAnhang1_S = cv.Value_22
		Me._cWAGAnhang1_J = cv.Value_23
		Me._cWANAnhang1_J = cv.Value_24

		Me._cVAGAnhang1 = cv.Value_25
		Me._cVANAnhang1 = cv.Value_26
		Me._cVAGAnhang1_S = cv.Value_27
		Me._cVANAnhang1_S = cv.Value_28
		Me._cVAGAnhang1_J = cv.Value_29
		Me._cVANAnhang1_J = cv.Value_30

		Me._cFAGAnhang1 = cv.Value_31
		Me._cFANAnhang1 = cv.Value_32

		PublicationOfGAV = cv.Value_34

		' GAV_Infos aufbauen...
		'CreateGAVBerufHeaderInfoLblControl()    ' Header
		GetData4Categories(CInt(Val(Me.lblMetaNr.Text)))

		strSputnikMessage = GetPVLWarning(CInt(strGAVNr))
		If strSputnikMessage.Trim <> String.Empty Then
			Me.lblSputnikMessage.Text = strSputnikMessage
			lblShowMessage_Click(sender, e)
		End If
		Me.lblShowMessage.Visible = strSputnikMessage.Trim <> String.Empty

	End Sub

#End Region

	' Schreibt auf ob der GAV Ave oder kein Ave ist
	Sub ShowAVEOrNot()
		Dim ctl As New Label
		Dim cv As ComboBoxItem = DirectCast(cbo_Gruppe0.SelectedItem, ComboBoxItem)

		Me.grpGAVBeruf.Controls.Add(ctl)

		'ctl.Location = New Point(iLeft, 40)
		ctl.Anchor = AnchorStyles.Top
		ctl.Size = New Size(Me.grpGAVBeruf.Width - 10, 23)
		ctl.TextAlign = ContentAlignment.MiddleRight
		ctl.Text = String.Format("{0} AVE", If(cv.Value_2.ToLower.Contains("true"), "Ist", "Kein"))
		ctl.Name = "lblGAV_Ave"
		ctl.ForeColor = If(cv.Value_2.ToLower.Contains("true"), Color.Black, Color.Red)
		ctl.BackColor = Color.Transparent
		ctl.Font = New Font(ctl.Font, If(cv.Value_2.ToLower.Contains("true"), FontStyle.Regular, FontStyle.Bold))
		ctl.Show()

	End Sub

	Sub GetData4Categories(ByVal iMetaNr As Integer)
		Dim iTop As Integer = 10 ' Me.lbl_Gruppe0.Top + Me.lbl_Gruppe0.Height + 40
		Dim iLeft As Integer = 30   ' Me.lbl_Gruppe0.Left
		Dim iTopCbo As Integer = 30 ' Me.Cbo_Gruppe0.Top + Me.Cbo_Gruppe0.Height + 50
		Dim iLeftCbo As Integer = 150   ' Me.Cbo_Gruppe0.Left + 50
		Dim iOldBaseCategoryNr As Integer = 0

		ResetAllLBL()
		liGAVCategories = GetPVLCategoryNames(CInt(Val(Me.lblMetaNr.Text)))
		ShowAVEOrNot()

		If liGAVCategories.Count > 0 Then
			For i As Integer = 0 To liGAVCategories.Count - 1
				Dim aGAVValues As String() = liGAVCategories(i).Split(CChar("¦"))
				Dim strCategoryName As String = aGAVValues(aGAVValues.Length - 1)
				Dim strIDCategory As String = aGAVValues(0)
				Dim strIDCalculator As String = aGAVValues(1)
				Dim strIDBaseCategory As String = aGAVValues(2)
				Dim ctl As New Label
				Me.scGAVCategory.Controls.Add(ctl)

				If Val(strIDBaseCategory) = 0 Then
					ctl.Location = New Point(iLeft, iTop)
					ctl.AutoSize = True
					ctl.Text = strCategoryName
					ctl.Tag = New TextBoxItem(strCategoryName,
																										strIDCategory, strIDCalculator, strIDBaseCategory)
					ctl.Name = "lblCategory_" & strIDCategory & "_" & strIDBaseCategory
					ctl.ForeColor = Color.Black
					ctl.BackColor = Color.Transparent
					ctl.Show()
					CreateCategoryCboControl(aGAVValues.ToList, iTop - 5, iLeftCbo)

					iOldBaseCategoryNr = 0

				Else
					ctl.AutoSize = True
					ctl.Text = ":.. " & strCategoryName
					ctl.Tag = New TextBoxItem(strCategoryName,
																										strIDCategory, strIDCalculator, strIDBaseCategory)
					ctl.Name = "lblCategory_" & strIDCategory & "_" & strIDBaseCategory
					ctl.ForeColor = Color.Red
					ctl.BackColor = Color.Transparent

					If iOldBaseCategoryNr = 0 Then
						ctl.Location = New Point(iLeft + 20, iTop)
						CreateCategoryCboControl(aGAVValues.ToList, iTop - 5, iLeftCbo + 50)

						iOldBaseCategoryNr = Val(strIDBaseCategory)

					Else
						If iOldBaseCategoryNr = Val(strIDBaseCategory) Then
							ctl.Location = New Point(iLeft + 20, iTop)
							CreateCategoryCboControl(aGAVValues.ToList, iTop - 5, iLeftCbo + 50)
						Else
							ctl.Location = New Point(iLeft + 40, iTop)
							CreateCategoryCboControl(aGAVValues.ToList, iTop - 5, iLeftCbo + 50)
						End If

					End If
					ctl.Show()

				End If
				iTop += 25
				iTopCbo += 35
				aLblControls.Add(ctl.Name)

			Next
		End If
		Me.scGAVCategory.Height = iTop
		Me.scGAVCategory.Visible = True
		Me.grpLOInfo.Top = Me.scGAVCategory.Top + Me.scGAVCategory.Height + 10

	End Sub

	Sub CreateCategoryCboControl(ByVal liData As List(Of String), ByVal iTop As Integer, ByVal iLeft As Integer)
		Dim strCategoryName As String = liData(liData.Count - 1)
		Dim strIDCategory As String = liData(0)
		Dim strIDCalculator As String = liData(1)
		Dim strIDBaseCategory As String = liData(2)

		Dim ctl As New ComboBoxEdit
		Me.scGAVCategory.Controls.Add(ctl)

		ctl.Location = New Point(iLeft, iTop)
		ctl.Width = scGAVCategory.Width - ctl.Left - 23
		ctl.Name = String.Format("CboCategory_{0}_{1}_{2}_", strIDCategory, strIDCalculator, strIDBaseCategory)
		If strCategoryName.ToLower.Contains("alter") Then
			ctl.Tag = String.Format("{0}", strCategoryName)
		ElseIf strCategoryName.ToLower.Contains("kanton") Then
			ctl.Tag = String.Format("{0}", strCategoryName)
		ElseIf strCategoryName.ToLower = "jahr" Then
			ctl.Tag = String.Format("{0}", strCategoryName)

		Else
			ctl.Tag = String.Empty

		End If
		'ctl.Tag = If(strCategoryName.ToLower.Contains("alter"), strCategoryName, _
		'             If(strCategoryName.ToLower.Contains("kanton"), strCategoryName, ""))
		ctl.ForeColor = Color.Black
		ctl.Show()
		MyComboBoxExtensions.ToItem(ctl)

		AddHandler ctl.Properties.QueryPopUp, AddressOf ctlCbo_DropDown
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
		Dim ctlCbo As ComboBoxEdit = DirectCast(sender, ComboBoxEdit)
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
				Exit Sub

			Else
				For i = 0 To aCboControls.Count - 1
					If aCboControls(i).ToString.ToLower = sender.name.ToString.ToLower Then
						Dim ctlBaseCbo As New ComboBoxEdit
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
										Exit Sub
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

		Dim liGAVCategoryValues As New List(Of String)
		If Val(Me.lblBaseCategoryNr.Text) > 0 Then
			liGAVCategoryValues = GetPVLCategoryValues(Val(Me.lblCategoryNr.Text), iBaseSelectedCategoryValue, True)
		Else
			liGAVCategoryValues = GetPVLCategoryValues(Val(Me.lblCategoryNr.Text), 0, False)
		End If
		Trace.WriteLine(String.Format("Zeitmessung für suchen der Webservice-Daten: {0} s.",
										((System.Environment.TickCount - Time_1) / 1000)))

		Time_1 = System.Environment.TickCount
		Dim iLengthofString As Integer = 0
		Dim bCheckedMAAlter As Boolean = Me.chkListBox.Items(0).CheckState = CheckState.Checked
		Dim bCheckedKanton As Boolean = Me.chkListBox.Items(1).CheckState = CheckState.Checked

		If Me.m_employeeNumber = 0 Then bCheckedMAAlter = False
		If Me._kdnr = 0 Then bCheckedKanton = False

		If liGAVCategoryValues.Count > 0 Then
			ctlCbo.Properties.Items.Add(New ComboBoxItem(String.Empty, 0, 0))
			For i = 0 To liGAVCategoryValues.Count - 1
				Dim aGAVValues As String() = liGAVCategoryValues(i).Split(CChar("¦"))
				Dim strValueName = aGAVValues(aGAVValues.Length - 1)
				Dim strCategoryValueNr As String = aGAVValues(0)
				Dim strCategoryNr As String = aGAVValues(1)
				If ctlCbo.Tag.ToLower.Contains("alter") And bCheckedMAAlter Then
					'  CheckEdit1.CheckState = CheckState.Checked Then
					If Val(strValueName) = Val(_MAAlter) Then
						ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName, strCategoryValueNr, strCategoryNr))
					End If

				ElseIf ctlCbo.Tag.ToLower.Contains("kanton") And bCheckedKanton Then
					If strValueName.ToLower = _kanton.ToLower Or
											strValueName.ToLower.Contains(String.Format("{0}:", _kanton.ToLower)) Then
						ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName, strCategoryValueNr, strCategoryNr))
					End If

				Else
					ctlCbo.Properties.Items.Add(New ComboBoxItem(strValueName.Replace("|", " => "), strCategoryValueNr, strCategoryNr))
				End If
				iLengthofString = If(strValueName.Length > iLengthofString, strValueName.Length, iLengthofString)
			Next


		End If

	End Sub

	Private Sub ctlCbo_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

		'Trace.WriteLine(sender.Tag.value_0)
		Dim ctlCbo = DirectCast(sender, ComboBoxEdit)
		Try
			Dim bNotSearchmore As Boolean = False
			Try
				For i As Integer = 0 To aCboControls.Count - 1
					If aCboControls(i).ToString.ToLower = ctlCbo.Name.ToLower Then
						For j As Integer = i + 1 To aCboControls.Count - 1
							Dim Cboctl2Delete As New ComboBoxEdit
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

		Dim strCategoryValueNr As String = ctlCbo.ToItem.Value_0
		Dim strCategoryNr As String = ctlCbo.ToItem.Value_1

		Me.lblCategoryNr.Text = strCategoryNr
		Me.lblCategoryValueNr.Text = strCategoryValueNr
		Dim bShowLODetails As Boolean = True

		Try
			For i As Integer = 0 To aCboControls.Count - 1
				ctlCbo = GetControlbyName(aCboControls(i).ToString)

				If Not IsNothing(ctlCbo) Then
					If ctlCbo.Text <> String.Empty Then
						aCategoryValuesNr.Add(ctlCbo.ToItem.Value_0)
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
			ShowLODataWithCategoryValues(Me.lblAllCategoryValueNr.Text)
		End If

	End Sub


#Region "Funktionen zur Aufbau der Lohndetails..."

	Sub ShowLODataWithCategoryValues(ByVal strCategoryValues As String)

		'Me.scLODetails.Visible = False
		ClearLODataFields()
		liLOData = GetPVLLODataWithCategoryValues(strCategoryValues)

		CreatelblControl4LO(liLOData, 10, 10)
		'    Me.scLODetails.Visible = True

	End Sub

	Sub CreatelblControl4LO(ByVal liData As List(Of String), ByVal iTop As Integer, ByVal iLeft As Integer)

		aLblLODataControls.Clear()
		Me.scLODetails.Left = Me.scGAVCategory.Left
		Createlbl4Basislohn(liData, 10, 10)

		'Me.scLODetails.Visible = False
		' Daten für Info anzeigen...
		ShowGAVLOData()

	End Sub

	Sub Createlbl4Mindestlohn(ByVal liData As List(Of String), ByVal iTop As Integer, ByVal iLeft As Integer)
		If liData.Count = 0 Then Exit Sub

		Dim aGAVLOData As String() = liData(7).Split(CChar("¦"))
		Dim strFieldName As String = aGAVLOData(0)
		Dim strFieldValue As String = aGAVLOData(1)
		Dim iLeftText As Integer = iLeft + 300
		Dim iLeftEinheit As Integer = iLeftText + 450
		Dim iLeftBetrag As Integer = iLeftEinheit + 30



		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_7"
			ctlLOBez.Text = "Mindesmonatslohn"
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_7"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight
			ctlLOValue.Font = New Font(ctlLOValue.Font, FontStyle.Bold)

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

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
		aGAVLOData = liData(8).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_8"
			ctlLOBez.Text = "Ferientage pro Jahr"
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_8"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_8"
			ctlLOBez.Text = "Tage"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += 25

		' Feiertage pro Jahr
		aGAVLOData = liData(6).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_6"
			ctlLOBez.Text = "Feiertage pro Jahr"
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_6"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_6"
			ctlLOBez.Text = "Tage"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += 25

		' 13. Monatslohn
		aGAVLOData = liData(16).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_16"
			ctlLOBez.Text = "13. Monatslohn"
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_16"
			ctlLOValue.Text = If(strFieldValue.ToLower.Contains("true"), "Ja", "Nein")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		iTop += 25

	End Sub

	Sub Createlbl4Basislohn(ByVal liData As List(Of String), ByVal iTop As Integer, ByVal iLeft As Integer)
		If liData.Count = 0 Then Exit Sub

		Dim aGAVLOData As String() = liData(2).Split(CChar("¦"))
		Dim strFieldName As String = aGAVLOData(0)
		Dim strFieldValue As String = aGAVLOData(1)
		Dim iLeftText As Integer = iLeft + 300
		Dim iLeftEinheit As Integer = iLeftText + 30 '+ 450
		Dim iLeftBetrag As Integer = iLeftEinheit + 30

		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_2"
			ctlLOBez.Text = "Basisstundenlohn"
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_2"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight
			ctlLOValue.Font = New Font(ctlLOValue.Font, FontStyle.Bold)

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

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
		aGAVLOData = liData(3).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_3"
			ctlLOBez.Text = "Ferienentschädigung"
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_3"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_3"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += 25

		' Feiertagsentschädigung prozentual
		aGAVLOData = liData(4).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		'bWithZusatztext = False
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_4"
			ctlLOBez.Text = "Feiertagsentschädigung"
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_4"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_11"
			ctlLOBez.Text = "CHF"
			ctlLOBez.AutoSize = True
			ctlLOBez.ForeColor = Color.Gray

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += 25 '25 ' 55 - If(bWithZusatztext, 0, 30)

		' 13. Monatslohn Prozentual
		aGAVLOData = liData(5).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		'bWithZusatztext = False
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_5"
			ctlLOBez.Text = "Entschädigung 13. Monatslohn"
			ctlLOBez.AutoSize = True

			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_5"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight

			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeftEinheit, iTop)
			ctlLOBez.Name = "lblLODataBez_2_5"
			ctlLOBez.Text = "CHF"
			ctlLOBez.ForeColor = Color.Gray
			ctlLOBez.AutoSize = True
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		iTop += 25

		' Stundenlohn
		aGAVLOData = liData(9).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

			ctlLOBez.Location = New Point(iLeft, iTop)
			ctlLOBez.Name = "lblLODataBez_9"
			ctlLOBez.Text = "Mindeststundenlohn mit 13. Monatslohn"
			ctlLOBez.AutoSize = True
			ctlLOBez.Font = New Font(ctlLOBez.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOBez)
			ctlLOBez.Show()
			aLblLODataControls.Add(ctlLOBez.Name)
		End If
		If strFieldValue <> String.Empty Then
			Dim ctlLOValue As New Label

			ctlLOValue.Location = New Point(iLeftBetrag, iTop)
			ctlLOValue.Name = "lblLODataValue_9"
			ctlLOValue.Text = Format(Val(strFieldValue), "n")
			ctlLOValue.Size = New Size(100, 20)
			ctlLOValue.TextAlign = ContentAlignment.TopRight

			ctlLOValue.Font = New Font(ctlLOValue.Font, FontStyle.Bold)
			Me.scLODetails.Controls.Add(ctlLOValue)
			ctlLOValue.Show()
			aLblLODataControls.Add(ctlLOValue.Name)
		End If
		If strFieldName <> String.Empty Then
			Dim ctlLOBez As New Label

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
		iTop += 25
		Me.scLODetails.Height = Me.grpGAVBeruf.Height - Me.scLODetails.Top - 10  ' iTop

		' wenn Resor darf FAR nicht korrigiert werden. es kann nicht FAR und RESOR in einem Kanton zusammen kommen!!!
		If Not Me._bResorpflichtig Then
			' FAR AN
			aGAVLOData = liData(18).Split(CChar("¦"))
			strFieldName = aGAVLOData(0)
			strFieldValue = aGAVLOData(1)
			Me.lblFAN.Text = Val(strFieldValue)

			' FAR AG
			aGAVLOData = liData(19).Split(CChar("¦"))
			strFieldName = aGAVLOData(0)
			strFieldValue = aGAVLOData(1)
			Me.lblFAG.Text = Val(strFieldValue)
		End If

		' FAR Calc
		aGAVLOData = liData(20).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)

		' FAR With BVG
		aGAVLOData = liData(21).Split(CChar("¦"))
		strFieldName = aGAVLOData(0)
		strFieldValue = aGAVLOData(1)


	End Sub

#End Region

	Function GetControlbyName(ByVal ControlName As String) As Object

		Try
			For Each obj As Control In Me.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next

			For Each obj As Control In Me.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next

			For Each obj As Control In Me.scGAVCategory.Controls
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
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return Nothing

	End Function


#Region "Funktionen zur Reseten..."

	Sub ClearGAVBerufInfoFields()

		Try
			Me.scGAVCategory.Controls.Clear()
			Me.LblInfo_0.Text = String.Empty
			Me.LblInfo_1.Text = String.Empty


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try
		'Me.scGAVBerufInfo.Refresh()

	End Sub

	Sub ResetAllLBL()

		Try
			ClsDataDetail.GetGAVAGBeitrag = 0
			Me.scGAVCategory.Controls.Clear()
			Me.LblInfo_0.Text = String.Empty
			Me.LblInfo_1.Text = String.Empty
			Me.LblInfo_5.Text = String.Empty
			Me.LblInfo_6.Text = String.Empty

		Catch ex As Exception

		End Try

		ClearLODataFields()         ' Lohndaten löschen

		Me.lblCategoryNr.Text = "0"
		Me.lblCalculatorNr.Text = "0"
		Me.lblBaseCategoryNr.Text = "0"
		Me.lblCategoryValueNr.Text = "0"
		Me.lblAllCategoryValueNr.Text = String.Empty
		'Me.scGAVBerufInfo.Visible = False
		Me.scLODetails.Visible = False

		Me.Refresh()
		aLblControls.Clear()
		aCboControls.Clear()
		aCategoryValuesNr.Clear()

	End Sub

	Sub ClearLODataFields()
		Dim i As Integer = 0

		Try
			Me.scLODetails.Controls.Clear()
			liLOData.Clear()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try
		Me.scLODetails.Refresh()

	End Sub

	Sub ClearGAVD_Fields()

		Try
			'Me.scD_1.Controls.Clear()
			'Me.scD_2.Controls.Clear()
			ClearGAVD_InfoFields()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		'Me.scD_1.Refresh()
		'Me.scD_2.Refresh()
	End Sub

	Sub ClearGAVD_InfoFields()

		Try
			'Me.scD_Info.Controls.Clear()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		'Me.scD_Info.Refresh()
	End Sub

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnbbiClear_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		ResetAllData()

		lueEmployee.EditValue = Nothing
		lueCustomer.EditValue = Nothing

	End Sub

	Private Sub ResetAllData()

		Me.LblState_1.Text = "Bereit"
		cbo_Gruppe0.Properties.Items.Clear()
		cbo_Gruppe0.EditValue = Nothing

		Cbo_Kanton.Text = m_InitialData.MDData.MDCanton

		ClsDataDetail.GetGAVAGBeitrag = 0
		If liLOData.Count > 0 Then ClsDataDetail.GetLiGAVData.Clear()

		ResetAllTabEntries()
		Me.txt_Header.Focus()

		ResetGAVFields()

	End Sub

	Sub ResetGAVFields()

		Me.txt_CHF_Lohn.Enabled = True
		Me.txt_Faktor_Lohn.Enabled = True

		Me.txt_CHF_Lohn.Text = "0.00"
		Me.txt_CHF_AG.Text = "0.00"
		Me.txt_CHF_Marge.Text = "0.00"
		Me.txt_CHF_Tarif.Text = "0.00"

		Me.txt_Faktor_Lohn.Text = "0.00"
		Me.txt_Faktor_Faktor.Text = "0.00"
		Me.txt_Faktor_Tarif.Text = "0.00"

		ResetAllLBL()

		If ClsDataDetail.GetLiAGData.Count <> 0 Then
			Me.txt_CHF_AG.Text = Format(CDec(ClsDataDetail.GetLiAGData.Item(12)), "n")

			If Format(CDec(ClsDataDetail.GetMDAGBeitrag + ClsDataDetail.GetGAVAGBeitrag), "n") >
															Format(CDec(ClsDataDetail.GetLiAGData.Item(12)), "n") Then
				Me.txt_CHF_AG.ForeColor = Color.DarkRed
				Me.Label7.ForeColor = Color.DarkRed
				Me.txt_CHF_AG.Text = Format(ClsDataDetail.GetMDAGBeitrag + ClsDataDetail.GetGAVAGBeitrag, "n")

			Else
				Me.txt_CHF_AG.ForeColor = Color.Black
				Me.Label7.ForeColor = Color.Black

			End If
		End If

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each cControl As Control In Me.Controls
			ResetControl(cControl)
		Next
	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		If TypeOf (con) Is TextBox Then
			Dim tb As TextBox = con
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is myCbo Then
			Dim cbo As myCbo = con
			'Die Sortierung darf nicht zurückgesetzt werden
			If cbo.Name = "CboSort" Then Exit Sub
			' Alle Felder auf Unchecked setzen
			If cbo.Items.Count > 0 Then cbo.CheckAll(CheckState.Unchecked)

			cbo.Items.Clear()
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is ComboBoxEdit Then
			Dim cbo As ComboBoxEdit = con
			cbo.EditValue = Nothing

		ElseIf TypeOf (con) Is GroupBox Then
			Dim grp As Control = con
			For Each con2 In grp.Controls
				ResetControl(con2)
			Next

		ElseIf TypeOf (con) Is ListBox Then
			Dim lst As ListBox = con
			lst.Items.Clear()

		ElseIf TypeOf (con) Is TextBox Then
			Dim txt As ListBox = con
			txt.Text = String.Empty

		End If
	End Sub

#End Region




	Sub CalcAGBeitrag(ByVal strMASex As String)

		Try
			If strMASex = String.Empty Then strMASex = "M"
			FuncLv.GetAGBeitragData(Me.m_employeeNumber)
			ClsDataDetail.GetMDAGBeitrag = CDec(ClsDataDetail.GetLiAGData.Item(0)) +
																				CDec(ClsDataDetail.GetLiAGData.Item(1)) +
																				CDec(ClsDataDetail.GetLiAGData.Item(2)) +
																				CDec(ClsDataDetail.GetLiAGData.Item(4)) +
																				CDec(ClsDataDetail.GetLiAGData.Item(5)) +
																				CDec(ClsDataDetail.GetLiAGData.Item(13)) +
																				CDec(ClsDataDetail.GetLiAGData.Item(If(strMASex = "M", 8, 6)))

			Me.LblInfo_3.Text = String.Format("AHV:{0}ALV:{0}SUVA_A:{0}FAK:{0}Verwaltungskosten für Sozialleistungen:{0}KTG_A:{0}BVG:{0}{0}Total 1 %:", vbNewLine)
			Me.LblInfo_4.Text = String.Format("{0:n4}{1}{2:n4}{1}{3:n4}{1}{4:n4}{1}{5:n4}{1}{6:n4}{1}{7:n4}{1}{1}{8:n4}",
																				CDec(ClsDataDetail.GetLiAGData.Item(0)),
																				vbNewLine,
																				CDec(ClsDataDetail.GetLiAGData.Item(1)),
																				CDec(ClsDataDetail.GetLiAGData.Item(2)),
																				CDec(ClsDataDetail.GetLiAGData.Item(4)),
																				CDec(ClsDataDetail.GetLiAGData.Item(5)),
																				CDec(ClsDataDetail.GetLiAGData.Item(If(strMASex = "M", 8, 6))),
																				CDec(ClsDataDetail.GetLiAGData.Item(13)),
																				ClsDataDetail.GetMDAGBeitrag)

			Me.txt_CHF_AG.Text = Format(If(CDec(ClsDataDetail.GetLiAGData.Item(12)) = 0,
																		 ClsDataDetail.GetMDAGBeitrag, CDec(ClsDataDetail.GetLiAGData.Item(12))), "n")
			CalcGAVAGBeitrag()


		Catch ex As Exception
			Me.LblInfo_3.Text = String.Empty
			Me.LblInfo_4.Text = String.Empty

		End Try


	End Sub

	Sub CalcGAVAGBeitrag()

		Try
			If liLOData.Count > 0 Then
				Dim far As Decimal = CDec(Val(If(bNoPVL, Me._cFAGAnhang1, Me.lblFAG.Text)))
				Dim wag As Decimal = CDec(Val(If(bNoPVL, Me._cWAGAnhang1, 0)))
				Dim vag As Decimal = CDec(Val(If(bNoPVL, Me._cVAGAnhang1, 0.3)))
				Dim agBeitrag As Decimal = CDec(ClsDataDetail.GetMDAGBeitrag + ClsDataDetail.GetGAVAGBeitrag)

				ClsDataDetail.GetGAVAGBeitrag = CDec(Val(If(bNoPVL, Me._cFAGAnhang1, Me.lblFAG.Text))) +
																				CDec(Val(If(bNoPVL, Me._cWAGAnhang1, 0))) +
																				CDec(Val(If(bNoPVL, Me._cVAGAnhang1, 0.3)))
				If wag = 0 Then
					If far = 0 Then
						Me.LblInfo_5.Text = String.Format("Vollzugskosten:{0}{0}Total:", vbNewLine)
						Me.LblInfo_6.Text = String.Format("{0:n4}{1}{2:n4}",
																							vag,
																							vbNewLine,
																							agBeitrag)
					Else
						Me.LblInfo_5.Text = String.Format("FAR:{0}Vollzugskosten:{0}{0}Total:", vbNewLine)
						Me.LblInfo_6.Text = String.Format("{0:n4}{1}{2:n4}{1}{3:n4}{1}",
																							far,
																							vbNewLine,
																							vag,
																							agBeitrag)

					End If
				Else
					Me.LblInfo_5.Text = String.Format("FAR:{0}Weiterbildung:{0}Vollzugskosten:{0}{0}Total:", vbNewLine)
					Me.LblInfo_6.Text = String.Format("{0:n4}{1}{2:n4}{1}{3:n4}{1}{1}{4:n4}",
																						far,
																						vbNewLine,
																						wag,
																						vag,
																						agBeitrag)
				End If

			Else
				Me.LblInfo_5.Text = String.Empty
				Me.LblInfo_6.Text = String.Empty

			End If
			Me.txt_CHF_AG.Text = Format(ClsDataDetail.GetMDAGBeitrag + ClsDataDetail.GetGAVAGBeitrag, "n")
			Me.txt_CHF_AG.ForeColor = Color.Black
			Me.Label7.ForeColor = Color.Black

			If ClsDataDetail.GetLiAGData.Count > 0 Then
				If Format(CDec(ClsDataDetail.GetMDAGBeitrag + ClsDataDetail.GetGAVAGBeitrag), "n") > Format(CDec(ClsDataDetail.GetLiAGData.Item(12)), "n") Then
					Me.txt_CHF_AG.ForeColor = Color.DarkRed
					Me.Label7.ForeColor = Color.DarkRed
				End If
			End If

		Catch ex As Exception
			Me.LblInfo_5.Text = String.Empty
			Me.LblInfo_6.Text = String.Empty
			ClsDataDetail.GetGAVAGBeitrag = 0


		End Try

	End Sub

#Region "Funktionen zur Menüaufbau..."

	Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		CalculateTarif()
	End Sub


#End Region


	Private Sub Op_CHF_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Op_CHF.MouseClick
		Me.grpFaktor.Visible = False
		Me.grpMarge.Visible = True
	End Sub

	Private Sub Op_Faktor_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Op_Faktor.MouseDown
		Me.grpFaktor.Location = Me.grpMarge.Location
		Me.grpFaktor.Visible = True
		Me.grpMarge.Visible = False
		Me.txt_Faktor_Faktor.Text = Format(If(CDec(Val(Me.txt_Faktor_Faktor.Text)) = 0, 1, CDec(Val(Me.txt_Faktor_Faktor.Text))), "n")
	End Sub

	Sub CalculateTarif()
		Dim bAsMarge As Boolean = Me.Op_CHF.Checked
		Dim dTarif As Double = 0

		If bAsMarge Then
			CalculateCustomerTarif()
			dTarif = Val(Me.txt_CHF_Lohn.Text) + Val(Me.txt_CHF_Marge.Text) + Val(((Val(Me.txt_CHF_AG.Text) * Val(Me.txt_CHF_Lohn.Text)) / 100))
			Me.txt_CHF_Tarif.Text = Format(dTarif, "n")

			Dim margenProz As Decimal = 0
			If Val(Me.txt_CHF_Tarif.Text) <> 0 Then
				margenProz = Val(Me.txt_CHF_Marge.Text) / Val(Me.txt_CHF_Tarif.Text) * 100
			Else
				margenProz = 0
			End If
			Me.LblMargeProz.Text = String.Format("{0:n4} %", margenProz)

		Else
			If Me.txt_Faktor_Tarif.Focused Then
				CalculateCustomerFaktor()
			End If
			dTarif = Val(Me.txt_Faktor_Lohn.Text) * Val(Me.txt_Faktor_Faktor.Text)
			Me.txt_Faktor_Tarif.Text = Format(dTarif, "n")

		End If

	End Sub

	Private Sub txt_CHF_AG_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_CHF_AG.KeyPress

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Private Sub txt_CHF_Lohn_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CHF_Lohn.LostFocus

		Me.txt_CHF_Lohn.Text = Format(CDec(Val(Me.txt_CHF_Lohn.Text)), "n")
		Me.txt_CHF_AG.Text = Format(CDec(Val(Me.txt_CHF_AG.Text)), "n")
		Me.txt_CHF_Marge.Text = Format(CDec(Val(Me.txt_CHF_Marge.Text)), "n")
		Me.txt_CHF_Tarif.Text = Format(CDec(Val(Me.txt_CHF_Tarif.Text)), "n")

		CalculateTarif()

		If Format(CDec(ClsDataDetail.GetMDAGBeitrag + ClsDataDetail.GetGAVAGBeitrag), "n") > Format(CDec(Val(Me.txt_CHF_AG.Text)), "n") Then
			Me.Label7.ForeColor = Color.DarkRed
			Me.txt_CHF_AG.ForeColor = Color.DarkRed
		Else
			Me.Label7.ForeColor = Color.Black
			Me.txt_CHF_AG.ForeColor = Color.Black
		End If

	End Sub

	Private Sub txt_CHF_Marge_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CHF_Marge.Leave
		CalculateTarif()
	End Sub


	Private Sub txt_Faktor_Tarif_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Faktor_Tarif.Leave
		CalculateCustomerFaktor()
	End Sub

	Private Sub CalculateCustomerFaktor()
		Dim dTarif As Decimal = CDec(Val(Me.txt_Faktor_Tarif.Text))
		Dim dFaktor As Decimal = CDec(Val(Me.txt_Faktor_Faktor.Text))
		Dim dLohn As Decimal = CDec(Val(Me.txt_Faktor_Lohn.Text))

		If dFaktor = 0 Then dFaktor = 1

		If CDec(dLohn) = 0 Then
			Me.txt_Faktor_Lohn.Text = Format(CDec((dTarif / dFaktor)), "n")

		Else
			Me.txt_Faktor_Faktor.Text = Format(CDec(dTarif / dLohn), "n")

		End If
		Me.txt_Faktor_Tarif.Text = Format(CDec(Val(Me.txt_Faktor_Tarif.Text)), "n")

	End Sub

	Private Sub txt_Faktor_Faktor_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Faktor_Faktor.Leave
		CalculateTarif()
	End Sub

	Private Sub txt_Faktor_Lohn_LostFocus(ByVal sender As Object, _
																				ByVal e As System.EventArgs) Handles txt_Faktor_Lohn.LostFocus, txt_Faktor_Tarif.LostFocus

		Me.txt_Faktor_Lohn.Text = Format(CDec(Val(Me.txt_Faktor_Lohn.Text)), "n")
		Me.txt_Faktor_Faktor.Text = Format(CDec(Val(Me.txt_Faktor_Faktor.Text)), "n")
		Me.txt_Faktor_Tarif.Text = Format(CDec(Val(Me.txt_Faktor_Tarif.Text)), "n")

		CalculateTarif()

	End Sub

	Private Sub txt_CHF_Lohn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_CHF_Lohn.TextChanged
		CalculateTarif()
	End Sub

	Private Sub txt_Faktor_Faktor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Faktor_Faktor.TextChanged

		If Val(Me.txt_CHF_Marge.Text) < 0 Then
			Me.txt_CHF_Marge.ForeColor = Color.DarkRed
			Me.Label10.ForeColor = Color.DarkRed
		Else
			Me.txt_CHF_Marge.ForeColor = Color.Black
			Me.Label10.ForeColor = Color.Black
		End If

		If Val(Me.txt_Faktor_Faktor.Text) < 0 Then
			Me.txt_Faktor_Faktor.ForeColor = Color.DarkRed
			Me.Label31.ForeColor = Color.DarkRed
		Else
			Me.txt_Faktor_Faktor.ForeColor = Color.Black
			Me.Label31.ForeColor = Color.Black
		End If

	End Sub

#Region "Funktionen zur SelectedIndexChanged..."

	Private Sub Cbo_Kanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.DropDown
		GetGAVKantone(Me.Cbo_Kanton)
	End Sub

	Private Sub Cbo_Kanton_SelectedIndexChanged(ByVal sender As System.Object, _
																							ByVal e As System.EventArgs) Handles Cbo_Kanton.SelectedIndexChanged
		Try
			Me._kanton = Me.Cbo_Kanton.Text
			cbo_Gruppe0.Properties.Items.Clear()
			ResetAllLBL()

		Catch ex As Exception

		End Try

	End Sub

	Sub ShowGAVLOData()
		Dim strData2Save As String = String.Empty
		Dim strCategoryLbl As String = String.Empty
		Dim bGetLOData As Boolean = True
		Dim liEmptyFields As New List(Of String)

		strData2Save = "GAVNr:{0}¦MetaNr:{1}¦CalNr:{2}¦CatNr:{3}¦CatBaseNr:{4}¦CatValueNr:{5}¦LONr:{6}¦Kanton:{7}¦Beruf:{8}¦"

		For i As Integer = 0 To Me.aLblControls.Count - 1
			Dim ctl As Label = GetControlbyName(aLblControls(i))
			Dim ctlCbo As ComboBoxEdit = GetControlbyName(aCboControls(i))
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
		For i As Integer = Me.aLblControls.Count + 8 To 19
			strCategoryLbl &= String.Format("Res_{0}:{1}¦", i, String.Empty)
		Next

		strData2Save &= String.Format("{0}", strCategoryLbl)
		strData2Save &= "Monatslohn:{9}¦FeiertagJahr:{10}¦FierienJahr:{11}¦13.Lohn:{12}¦{13}"
		strData2Save &= "BasisLohn:{14}¦FerienBetrag:{15}¦FerienProz:{16}¦FeierBetrag:{17}¦FeierProz:{18}¦"
		strData2Save &= "13.Betrag:{19}¦13.Proz:{20}¦CalcFerien:{21}¦CalcFeier:{22}¦Calc13:{23}¦"
		strData2Save &= "StdLohn:{24}¦FARAN:{25}¦FARAG:{26}¦VAN:{27}¦VAG:{28}¦"
		strData2Save &= "StdWeek:{29}¦StdMonth:{30}¦StdYear:{31}¦IsPVL:{32}¦"

		strData2Save &= "_WAG:{33}¦_WAN:{34}¦_WAG_S:{35}¦_WAN_S:{36}¦_WAG_J:{37}¦_WAN_J:{38}¦"
		strData2Save &= "_VAG:{39}¦_VAN:{40}¦_VAG_S:{41}¦_VAN_S:{42}¦_VAG_J:{43}¦_VAN_J:{44}¦"
		strData2Save &= "_FAG:{45}¦_FAN:{46}¦"

		strData2Save = String.Format(strData2Save, Me.lblGAVNr.Text, _
															 Me.lblMetaNr.Text, _
															 Me.lblCalculatorNr.Text, _
															 Me.lblCategoryNr.Text, _
															 Me.lblBaseCategoryNr.Text, _
															 Me.lblCategoryValueNr.Text, _
															 Me.lblAllCategoryValueNr.Text, _
															 Me.Cbo_Kanton.Text, _
															 Me.cbo_Gruppe0.Text, _
 _
															 liLOData(7).Split("¦")(1), _
															 liLOData(8).Split("¦")(1), _
															 liLOData(6).Split("¦")(1), _
															 liLOData(16).Split("¦")(1), String.Empty, _
 _
															 If(bGetLOData, liLOData(2).Split("¦")(1), "1"), _
															 liLOData(3).Split("¦")(1), _
															 liLOData(10).Split("¦")(1), _
															 liLOData(4).Split("¦")(1), _
															 liLOData(11).Split("¦")(1), _
 _
															 liLOData(5).Split("¦")(1), _
															 liLOData(12).Split("¦")(1), _
															 liLOData(13).Split("¦")(1), _
															 liLOData(14).Split("¦")(1), _
															 liLOData(15).Split("¦")(1), _
 _
															 If(bGetLOData, liLOData(9).Split("¦")(1), "1"), _
															 Me.lblFAN.Text, _
															 Me.lblFAG.Text, _
															 "0.7", _
															 "0.3", _
 _
															 Me.lblWStd.Text, _
															 Me.lblMStd.Text, _
															 Me.lblJStd.Text, _
															 If(Val(Me.lblGAVNr.Text) = 815001, 1, 0), _
 _
															 Me._cWAGAnhang1, _
															 Me._cWANAnhang1, _
															 Me._cWAGAnhang1_S, _
															 Me._cWANAnhang1_S, _
															 Me._cWAGAnhang1_J, _
															 Me._cWANAnhang1_J, _
 _
															 Me._cVAGAnhang1, _
															 Me._cVANAnhang1, _
															 Me._cVAGAnhang1_S, _
															 Me._cVANAnhang1_S, _
															 Me._cVAGAnhang1_J, _
															 Me._cVANAnhang1_J, _
 _
															 Me._cFAGAnhang1, _
															 Me._cFANAnhang1)

		ClsDataDetail.strGAVData = strData2Save

		Me.LblInfo_0.Text = "Grundlohn:{0}"
		Me.LblInfo_0.Text &= "Feiertag:{0}"
		Me.LblInfo_0.Text &= "Ferien:{0}"
		Me.LblInfo_0.Text &= "13. Lohn:{0}"
		Me.LblInfo_0.Text &= "Stundenlohn:{0}"
		Me.LblInfo_0.Text &= "{0}SOLL-Stunden:{0}"
		Me.LblInfo_0.Text &= "FAR AN-AG:{0}"
		Me.LblInfo_0.Text &= String.Format("{0}{1}", If(bNoPVL, "Weiterbildung AN-AG:", String.Empty), "{0}")
		Me.LblInfo_0.Text &= "Vollzugskosten AN-AG:{0}"
		Me.LblInfo_0.Text = String.Format(Me.LblInfo_0.Text, vbNewLine)

		Me.LblInfo_1.Text = String.Format("{0}{1}({2} %)  {3}{1}({4} %)  {5}{1}({6} %)  {7}{1}{8}{1}{9}{1}{10}{1}" & _
																			"{11} - {12}{1}{13}  {14}{1}{15} - {16}", _
																		 Format(CDec(Val(liLOData(2).Split("¦")(1))), "n"), _
																			vbNewLine, _
																			Format(CDec(Val(liLOData(11).Split("¦")(1))) * 100, "n"), _
																			Format(CDec(Val(liLOData(4).Split("¦")(1))), "n"), _
																			Format(CDec(Val(liLOData(10).Split("¦")(1))) * 100, "n"), _
																			Format(CDec(Val(liLOData(3).Split("¦")(1))), "n"), _
																			Format(CDec(Val(liLOData(12).Split("¦")(1))) * 100, "n"), _
																			Format(CDec(Val(liLOData(5).Split("¦")(1))), "n"), _
																			Format(CDec(Val(If(bGetLOData, liLOData(9).Split("¦")(1), "1"))), "n"), _
 _
																			String.Empty, _
																			Format(CDec(Val(Me.lblWStd.Text)), "n"), _
 _
																			Format(CDec(Val(If(bNoPVL, Me._cFANAnhang1, Me.lblFAN.Text))), "n"), _
																			Format(CDec(If(bNoPVL, Me._cFAGAnhang1, Me.lblFAG.Text)), "n"), _
																			If(bNoPVL, Format(If(CDec(Val(If(bNoPVL, Me._cWANAnhang1, 0))) = 0, _
																								CDec(Val(If(bNoPVL, Me._cWAGAnhang1_S, 0))), _
																								CDec(Val(If(bNoPVL, Me._cWANAnhang1, 0)))), "n") + "-", String.Empty), _
																			If(bNoPVL, Format(If(CDec(Val(If(bNoPVL, Me._cWAGAnhang1, 0))) = 0, _
																								CDec(Val(If(bNoPVL, Me._cWAGAnhang1_S, 0))), _
																								CDec(Val(If(bNoPVL, Me._cWAGAnhang1, 0)))), "n"), String.Empty), _
																			Format(CDec(Val(If(bNoPVL, Me._cVANAnhang1, 0.7))), "n"), _
																			Format(CDec(Val(If(bNoPVL, Me._cVAGAnhang1, 0.3))), "n"))

		' AG-Beiträge für GAV-Beruf ausgeben...
		CalcGAVAGBeitrag()
		Me.txt_CHF_Lohn.Text = Format(CDec(Val(If(bGetLOData, liLOData(9).Split("¦")(1), "1"))), "n")
		Me.txt_Faktor_Lohn.Text = Format(CDec(Val(If(bGetLOData, liLOData(9).Split("¦")(1), "1"))), "n")

	End Sub

#End Region

	Private Sub txt_CHF_Lohn_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txt_CHF_Lohn.Validating

		If liLOData.Count > 0 Then
			If CDec(Val(Me.txt_CHF_Lohn.Text)) < CDec(Val(liLOData(9).Split("¦")(1))) Then
				Me.txt_CHF_Lohn.Text = Format(CDec(Val(liLOData(9).Split("¦")(1))), "n")
			End If
		End If

	End Sub

	Private Sub txt_Faktor_Lohn_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txt_Faktor_Lohn.Validating

		If liLOData.Count > 0 Then
			If CDec(Val(Me.txt_Faktor_Lohn.Text)) < CDec(Val(liLOData(9).Split("¦")(1))) Then
				Me.txt_Faktor_Lohn.Text = Format(CDec(Val(liLOData(9).Split("¦")(1))), "n")
			End If
		End If

	End Sub

	Private Sub txt_CHF_Tarif_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_CHF_Tarif.Leave
		CalculateCustomerTarif()
	End Sub

	Private Sub CalculateCustomerTarif()
		Dim dTarif As Decimal = CDec(Val(Me.txt_CHF_Tarif.Text))
		Dim dMarge As Decimal = CDec(Val(Me.txt_CHF_Marge.Text))
		Dim dAG As Decimal = CDec(Val(Me.txt_CHF_AG.Text))
		Dim dLohn As Decimal = CDec(Val(Me.txt_CHF_Lohn.Text))

		If CDec(Val(Me.txt_CHF_Lohn.Text)) = 0 Then
			Me.txt_CHF_Lohn.Text = Format(CDec(((dTarif - dMarge) * 100) / (100 + dAG)), "n")

		Else
			Me.txt_CHF_Marge.Text = Format(CDec(dTarif - dLohn - (dLohn * (dAG / 100))), "n")

		End If
		Me.txt_CHF_Tarif.Text = Format(CDec(Val(Me.txt_CHF_Tarif.Text)), "n")

		Dim margenProz As Decimal = 0
		If Val(Me.txt_CHF_Tarif.Text) <> 0 Then
			margenProz = Val(Me.txt_CHF_Marge.Text) / Val(Me.txt_CHF_Tarif.Text) * 100
		Else
			margenProz = 0
		End If
		Me.LblMargeProz.Text = String.Format("{0:n4} %", margenProz)

	End Sub

	Private Sub txt_CHF_AG_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txt_CHF_AG.Validating

		If ClsDataDetail.GetLiAGData.Count > 0 Then
			If CDec(Val(Me.txt_CHF_AG.Text)) < CDec(ClsDataDetail.GetLiAGData.Item(12)) Then
				Me.txt_CHF_AG.Text = Format(CDec(ClsDataDetail.GetLiAGData.Item(12)), "n")
			End If
		End If

	End Sub

	Private Sub pd_QueryPageSettings(ByVal sender As Object, ByVal e As System.Drawing.Printing.QueryPageSettingsEventArgs) Handles pd.QueryPageSettings

		e.PageSettings.Landscape = True

	End Sub

	Private Sub lblShowMessage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblShowMessage.Click
		Dim mouseposition As New Point(Me.Left + Me.grpGAVBeruf.Left + 20, pcpSputnikMessage.Top + Me.grpGAVBeruf.Top + Me.Top + 50)

		Dim barmanager As New DevExpress.XtraBars.BarManager
		'pcpSputnikMessage.CloseOnLostFocus = True
		Me.pcpSputnikMessage.Manager = barmanager
		Me.pcpSputnikMessage.ShowPopup(mouseposition)

	End Sub

	Private Sub lblRecInfo_Click(sender As System.Object, e As System.EventArgs) Handles lblRecInfo.Click
		'Dim mouseposition As New Point(Cursor.Position)
		Dim mouseposition As New Point(Me.Left + Me.grpGAVBeruf.Width + 20, pcc_1Search.Top + Me.grpGAVBeruf.Top + Me.Top + 50)

		Dim barmanager As New DevExpress.XtraBars.BarManager
		'pcc_1Search.CloseOnLostFocus = True
		Me.pcc_1Search.Manager = barmanager
		Me.pcc_1Search.ShowPopup(mouseposition)

	End Sub

	Private Sub OnbbiGAVInfo_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiGAVInfo.ItemClick
		Dim GAVObj As New SPGAV.ClsMain_Net
		If CInt(Val(Me.lblMetaNr.Text)) > 0 Then
			GAVObj.ShowfrmPVLD_Info(CInt(Val(Me.lblMetaNr.Text)))

		Else
			m_UtilityUI.ShowErrorDialog("Keine gültige GAV-Daten ausgewählt.")

		End If

	End Sub


#Region "Helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


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

#End Region


End Class
