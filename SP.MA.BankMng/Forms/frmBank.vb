
Imports System.Reflection.Assembly

Imports DevExpress.LookAndFeel
Imports SP.MA.BankMng.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Settings
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Logging

Imports SPProgUtility.Mandanten
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
Imports DevExpress.XtraEditors.Repository
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraEditors.Controls
Imports SP.Internal.Automations

''' <summary>
''' Bank management.
''' </summary>
Public Class frmBank

	Public Delegate Sub BankDataSavedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer)
	Public Delegate Sub BankDataDeletedHandler(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer)

#Region "Constants"

	Private Const OPEN_BANK_SEARCH_LEFT_COORDINATE As Integer = 252
	Private Const OPEN_BANK_SEARCH_LEFT_OFFSET_ON_ERROR As Integer = 15

	Public Const MANDANT_XML_SETTING_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceibanutil"
	Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"

	Public Const MANDANT_XML_SETTING_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicebankdatabase"
	Public Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wssps_services/spbankutil.asmx"

#End Region

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' The SPProgUtility object.
	''' </summary>
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' Contains the employee number.
	''' </summary>
	Private m_EmployeeNumber As Integer

	''' <summary>
	''' The record number of selected bank.
	''' </summary>
	Private m_CurrentBankRecordNumber As Integer?

	''' <summary>
	''' Check edit for default bank symbol.
	''' </summary>
	Private m_CheckEditDedfaultBank As RepositoryItemCheckEdit

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_IBanUtilWebServiceUri As String

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_BankUtilWebServiceUri As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

#End Region

#Region "Events"

	Public Event BankDataSaved As BankDataSavedHandler
	Public Event BankDataDeleted As BankDataDeletedHandler

#End Region

#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			' Mandantendaten
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			m_MandantData = New Mandant

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		' Read IBAN Webservice URI from settings
		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
			m_IBanUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

			If String.IsNullOrWhiteSpace(m_IBanUtilWebServiceUri) Then
				m_IBanUtilWebServiceUri = DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_IBanUtilWebServiceUri = DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI
		End Try

		' Read BAnk Webservice URI from settings
		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
			m_BankUtilWebServiceUri = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_BANK_UTIL_WEBSERVICE_URI, m_InitializationData.MDData.MDNr))

			If String.IsNullOrWhiteSpace(m_BankUtilWebServiceUri) Then
				m_BankUtilWebServiceUri = DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_BankUtilWebServiceUri = DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI
		End Try

		gvBank.OptionsView.ShowIndicator = False

		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SettingsManager = New SettingsManager
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		' Important symbol.
		m_CheckEditDedfaultBank = CType(gridBank.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditDedfaultBank.PictureChecked = My.Resources.Checked
		m_CheckEditDedfaultBank.PictureUnchecked = Nothing
		m_CheckEditDedfaultBank.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		' Reset the form
		Reset()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets the selected bank view data.
	''' </summary>
	''' <returns>The selected bank or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedBankViewData As BankViewData
		Get
			Dim grdView = TryCast(gridBank.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim bank = CType(grdView.GetRow(selectedRows(0)), BankViewData)
					Return bank
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the first bank in the list of banks.
	''' </summary>
	''' <returns>First bank in list or nothing.</returns>
	Public ReadOnly Property FirstBankInListOfBanks As BankViewData
		Get
			If gvBank.RowCount > 0 Then

				Dim rowHandle = gvBank.GetVisibleRowHandle(0)
				Return CType(gvBank.GetRow(rowHandle), BankViewData)
			Else
				Return Nothing
			End If

		End Get
	End Property

#End Region

#Region "Public Methods"

	''' <summary>
	''' Loads the bank data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function LoadBankData(ByVal employeeNumber As Integer) As Boolean

		Dim success As Boolean = PrepareFrom(employeeNumber)

		Dim firstBankInList = FirstBankInListOfBanks

		If Not FirstBankInListOfBanks Is Nothing Then
			PresentBankDetailData(firstBankInList)
		Else
			PrepareForNew()
		End If

		Return success

	End Function

	''' <summary>
	''' Loads the bank data of an employee.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="bankRecordNumberToSelect">The bank which should be selected.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function LoadBankData(ByVal employeeNumber As Integer, ByVal bankRecordNumberToSelect As Integer)

		Dim success As Boolean = PrepareFrom(employeeNumber)

		Dim bankToSelect = GetBankViewDataByRecordNumber(bankRecordNumberToSelect)

		If Not bankToSelect Is Nothing Then
			PresentBankDetailData(bankToSelect)
			FocusBank(employeeNumber, bankRecordNumberToSelect)
		Else

			Dim firstBankInList = FirstBankInListOfBanks

			If Not FirstBankInListOfBanks Is Nothing Then
				PresentBankDetailData(firstBankInList)
			Else
				PrepareForNew()
			End If

		End If

		Return success

	End Function

	''' <summary>
	''' Allows to enter a new bank.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Public Function NewBank(ByVal employeeNumber As Integer)

		Dim success As Boolean = PrepareFrom(employeeNumber)

		PrepareForNew()

		Return success
	End Function

#End Region

#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		btnSave.Text = m_Translate.GetSafeTranslationValue(btnSave.Text)
		btnNewBank.Text = m_Translate.GetSafeTranslationValue(btnNewBank.Text)
		btnDeleteBank.Text = m_Translate.GetSafeTranslationValue(btnDeleteBank.Text)
		Me.btnCreateTODO.Text = m_Translate.GetSafeTranslationValue(Me.btnCreateTODO.Text)

		chkForSalary.Text = m_Translate.GetSafeTranslationValue(chkForSalary.Text)
		chkAsforeignBank.Text = m_Translate.GetSafeTranslationValue(chkAsforeignBank.Text)

		lblClearing.Text = m_Translate.GetSafeTranslationValue(lblClearing.Text)
		lblBLZ.Text = m_Translate.GetSafeTranslationValue(lblBLZ.Text)
		lblBankname.Text = m_Translate.GetSafeTranslationValue(lblBankname.Text)
		lblBankOrt.Text = m_Translate.GetSafeTranslationValue(lblBankOrt.Text)
		lblSwift.Text = m_Translate.GetSafeTranslationValue(lblSwift.Text)

		grpreservefelder.Text = m_Translate.GetSafeTranslationValue(grpreservefelder.Text)

		lblKontonummer.Text = m_Translate.GetSafeTranslationValue(lblKontonummer.Text)
		lbliban.Text = m_Translate.GetSafeTranslationValue(lbliban.Text)

		lblInhaber.Text = m_Translate.GetSafeTranslationValue(lblInhaber.Text)
		lbl1Adresse.Text = m_Translate.GetSafeTranslationValue(lbl1Adresse.Text)
		lbl2Adresse.Text = m_Translate.GetSafeTranslationValue(lbl2Adresse.Text)
		lbl3Adresse.Text = m_Translate.GetSafeTranslationValue(lbl3Adresse.Text)

		chkAsStandard.Text = m_Translate.GetSafeTranslationValue(chkAsStandard.Text)
		chkForZG.Text = m_Translate.GetSafeTranslationValue(chkForZG.Text)

		lblerstellt.Text = m_Translate.GetSafeTranslationValue(lblerstellt.Text)
		lblgaendert.Text = m_Translate.GetSafeTranslationValue(lblgaendert.Text)

	End Sub

	''' <summary>
	''' Prepares the form.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function PrepareFrom(ByVal employeeNumber As Integer) As Boolean

		Dim success As Boolean = True

		m_EmployeeNumber = employeeNumber

		' Reset the form
		Reset()

		success = success AndAlso LoadEmployeeMasterData(employeeNumber)
		success = success AndAlso LoadEmployeeBankData(employeeNumber)

		Return success
	End Function

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		m_CurrentBankRecordNumber = Nothing

		If (Not employeePicture.Image Is Nothing) Then
			employeePicture.Image.Dispose()
		End If

		employeePicture.Image = Nothing
		employeePicture.Properties.NullText = m_Translate.GetSafeTranslationValue("Kein Bild vorhanden!")
		employeePicture.Properties.ShowMenu = False
		employeePicture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze

		chkForSalary.Checked = False
		chkAsforeignBank.Checked = False

		txtClearing.Text = String.Empty
		txtClearing.Properties.MaxLength = 5

		txtBLZ.Text = String.Empty
		txtBLZ.Properties.MaxLength = 20

		txtBankname.Text = String.Empty
		txtBankname.Properties.MaxLength = 32

		txBankAddress.Text = String.Empty
		txBankAddress.Properties.MaxLength = 32

		txtSwift.Text = String.Empty
		txtSwift.Properties.MaxLength = 20

		txtKontoNr.Text = String.Empty
		txtKontoNr.Properties.MaxLength = 27

		txtIBAN.Text = String.Empty
		txtIBAN.Properties.MaxLength = 35

		txtOwner.Text = String.Empty
		txtOwner.Properties.MaxLength = 28

		txt1Address.Text = String.Empty
		txt1Address.Properties.MaxLength = 28

		txt2Address.Text = String.Empty
		txt2Address.Properties.MaxLength = 28

		txt3Address.Text = String.Empty
		txt3Address.Properties.MaxLength = 28

		chkAsStandard.Checked = False
		chkForZG.Checked = False

		lblBankCreated.Text = String.Empty
		lblBankChanged.Text = String.Empty

		' ---Reset drop downs, grids and lists---

		ResetBankGrid()

		TranslateControls()

		btnSave.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 116)
		btnDeleteBank.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 116)

		' Clear errors
		ClearErrors()

	End Sub

	''' <summary>
	''' Resets the bank grid.
	''' </summary>
	Private Sub ResetBankGrid()

		' Reset the grid
		gvBank.OptionsView.ShowIndicator = False

		gvBank.Columns.Clear()

		Dim columnClearningNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnClearningNr.Caption = m_Translate.GetSafeTranslationValue("Clearing")
		columnClearningNr.Name = "DTABCNR"
		columnClearningNr.FieldName = "DTABCNR"
		columnClearningNr.Visible = True
		gvBank.Columns.Add(columnClearningNr)

		Dim columnBankname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBankname.Caption = m_Translate.GetSafeTranslationValue("Bank")
		columnBankname.Name = "BankNameForGrid"
		columnBankname.FieldName = "BankNameForGrid"
		columnBankname.Visible = True
		gvBank.Columns.Add(columnBankname)

		Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
		activeColumn.Caption = m_Translate.GetSafeTranslationValue("")
		activeColumn.Name = "Active"
		activeColumn.FieldName = "Active"
		activeColumn.Visible = True
		activeColumn.ColumnEdit = m_CheckEditDedfaultBank
		gvBank.Columns.Add(activeColumn)

		m_SuppressUIEvents = True
		gridBank.DataSource = Nothing
		m_SuppressUIEvents = False
	End Sub

	''' <summary>
	''' Loads the employee master data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function LoadEmployeeMasterData(ByVal employeeNumber As Integer) As Boolean

		Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, True)

		If (employeeMasterData Is Nothing) Then
			Return False
		End If

		Text = String.Format(m_Translate.GetSafeTranslationValue("Bankverbindung für {0} {1}"), employeeMasterData.Lastname, employeeMasterData.Firstname)

		Try
			If Not employeeMasterData.MABild Is Nothing AndAlso employeeMasterData.MABild.Count > 0 Then
				Dim memoryStream As New System.IO.MemoryStream(employeeMasterData.MABild)
				employeePicture.Image = Image.FromStream(memoryStream)
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

		Return True
	End Function

	''' <summary>
	''' Loads employee bank data.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadEmployeeBankData(ByVal employeeNumber As Integer)

		Dim employeeBankData = m_EmployeeDatabaseAccess.LoadEmployeeBanks(employeeNumber)

		If (employeeBankData Is Nothing) Then
			Return False
		End If

		Dim listDataSource As BindingList(Of BankViewData) = New BindingList(Of BankViewData)

		' Convert the data to view data.
		For Each bankData In employeeBankData

			Dim cViewData = New BankViewData() With {
				.ID = bankData.ID,
				.RecordNumber = bankData.RecordNumber,
				.BankLOL = (bankData.BnkLOL = True),
				.BankAU = (bankData.BankAU = True),
				.DTABCNR = bankData.DTABCNR,
				.BLZ = bankData.BLZ,
				.BankName = bankData.Bank,
				.BankLocation = bankData.BankLocation,
				.Swift = bankData.Swift,
				.AccountNumber = bankData.AccountNr,
				.IBAN = bankData.IBANNr,
				.Address1 = bankData.DTAAdr1,
				.Address2 = bankData.DTAAdr2,
				.Address3 = bankData.DTAAdr3,
				.Address4 = bankData.DTAAdr4,
				.Active = (bankData.ActiveRec = True),
				.BankZG = (bankData.BankZG = True),
				.CreatedOn = bankData.CreatedOn,
				.CreatedFrom = bankData.CreatedFrom,
				.ChangedOn = bankData.ChangedOn,
				.ChangedFrom = bankData.ChangedFrom
			}

			listDataSource.Add(cViewData)
		Next

		m_SuppressUIEvents = True
		gridBank.DataSource = listDataSource
		gvBank.Columns("DTABCNR").BestFit()
		gvBank.Columns("Active").BestFit()
		m_SuppressUIEvents = False

		Return True

	End Function

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnFrmBank_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Me.KeyPreview = True
		Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
			Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
			Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
			Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
			If setting_form_mainsplitter > 0 Then Me.sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_form_mainsplitter)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles the form disposed event.
	''' </summary>
	Private Sub OnFrmBank_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccMain.SplitterPosition)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

		' Dispose employee picture
		If (Not employeePicture.Image Is Nothing) Then
			employeePicture.Image.Dispose()
		End If

	End Sub

	''' <summary>
	''' Keypreview for Modul-version
	''' </summary>
	Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 AndAlso m_InitializationData.UserData.UserNr = 1 Then
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

	''' <summary>
	''' Handles focus change of bank row.
	''' </summary>
	Private Sub OnBank_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvBank.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		Dim selectedBank = SelectedBankViewData

		If Not selectedBank Is Nothing Then
			PresentBankDetailData(selectedBank)
		End If

	End Sub

	''' <summary>
	''' Handles double clik on grid.
	''' </summary>
	Private Sub OnGridBank_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridBank.DoubleClick

		Dim selectedBank = SelectedBankViewData

		If Not selectedBank Is Nothing Then
			PresentBankDetailData(selectedBank)
		End If

	End Sub

	''' <summary>
	''' Handles click on new bank data button.
	''' </summary>
	Private Sub OnBtnNewBank_Click(sender As System.Object, e As System.EventArgs) Handles btnNewBank.Click
		PrepareForNew()
	End Sub

	''' <summary>
	''' Handles click on save bank data buton.
	''' </summary>
	Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

		If ValidateBankInputData() Then

			Dim bankData As EmployeeBankData = Nothing

			Dim dt = DateTime.Now
			If Not m_CurrentBankRecordNumber.HasValue Then
				bankData = New EmployeeBankData With {.EmployeeNumber = m_EmployeeNumber,
																							.CreatedOn = dt,
																							.CreatedFrom = m_ClsProgSetting.GetUserName()}
			Else

				Dim bankList = m_EmployeeDatabaseAccess.LoadEmployeeBanks(m_EmployeeNumber, m_CurrentBankRecordNumber)

				If bankList Is Nothing OrElse Not bankList.Count = 1 Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
					Return
				End If

				bankData = bankList(0)
			End If

			bankData.BnkLOL = chkForSalary.Checked
			bankData.BankAU = chkAsforeignBank.Checked

			bankData.DTABCNR = txtClearing.Text
			bankData.BLZ = txtBLZ.Text
			bankData.Bank = txtBankname.Text
			bankData.BankLocation = txBankAddress.Text
			bankData.Swift = txtSwift.Text

			bankData.AccountNr = txtKontoNr.Text
			bankData.IBANNr = txtIBAN.Text

			bankData.DTAAdr1 = txtOwner.Text.Replace("\", "/")
			bankData.DTAAdr2 = txt1Address.Text.Replace("\", "/")
			bankData.DTAAdr3 = txt2Address.Text.Replace("\", "/")
			bankData.DTAAdr4 = txt3Address.Text.Replace("\", "/")

			bankData.ActiveRec = chkAsStandard.Checked
			bankData.BankZG = chkForZG.Checked

			bankData.ChangedFrom = m_ClsProgSetting.GetUserName()
			bankData.ChangedOn = dt

			Dim success As Boolean = True

			' Insert or update document
			If bankData.ID = 0 Then
				success = m_EmployeeDatabaseAccess.AddEmployeeBank(bankData)
				m_CurrentBankRecordNumber = bankData.RecordNumber
			Else
				success = m_EmployeeDatabaseAccess.UpdateEmployeeBank(bankData)
			End If

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Else

				LoadEmployeeBankData(m_EmployeeNumber)
				FocusBank(m_EmployeeNumber, m_CurrentBankRecordNumber)

				PresentBankDetailData(SelectedBankViewData)

				RaiseEvent BankDataSaved(Me, m_EmployeeNumber, m_CurrentBankRecordNumber)

			End If

		End If

	End Sub

	''' <summary>
	''' Handles click on delete bank button.
	''' </summary>
	Private Sub OnBtnDeleteBank_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteBank.Click
		Dim bankViewData = SelectedBankViewData

		If Not bankViewData Is Nothing AndAlso m_CurrentBankRecordNumber.HasValue Then

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																			m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return
			End If

			Dim result = m_EmployeeDatabaseAccess.DeleteEmployeeBank(bankViewData.ID, ConstantValues.ModuleName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

			Select Case result
				Case DeleteEmployeeBankResult.CouldNotDeleteBecauseOfExistingLM
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Bankdaten konnten nicht gelöscht werden, da sie bereits mit einer Lohnabrechnung verbunden sind."))
				Case DeleteEmployeeBankResult.ErrorWhileDelete
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Bankdaten konnten nicht gelöscht werden."))
				Case DeleteEmployeeBankResult.Deleted
					' Reload bank data and select first entry.
					LoadBankData(m_EmployeeNumber)
					Dim firstBank = FirstBankInListOfBanks

					If (firstBank Is Nothing) Then
						PrepareForNew()
					Else
						PresentBankDetailData(firstBank)
					End If

					' if last record was deleted!
					If m_CurrentBankRecordNumber.HasValue Then
						RaiseEvent BankDataDeleted(Me, m_EmployeeNumber, m_CurrentBankRecordNumber)
					End If

			End Select
		End If
	End Sub

	''' <summary>
	''' Handles leave event of IBAN textbox.
	''' </summary>
	Private Sub OnTxtIBANOrClearingOrKontoNr_Leave(sender As System.Object, e As System.EventArgs) Handles txtIBAN.Leave
		DetermineClearingNumberAndAccountNumberFromIBAN()
	End Sub

	Private Sub OntxtSwift_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtSwift.ButtonClick
		If String.IsNullOrWhiteSpace(txtClearing.EditValue) Then Return

		FillBankDataByClearingNumberOverWebService(txtClearing.EditValue, txtBankname.EditValue, txBankAddress.EditValue)

	End Sub

	''' <summary>
	''' Handles leave event Clearing and KonoNr textbox.
	''' </summary>
	Private Sub OnTxtClearingOrKontoNr_Leave(sender As System.Object, e As System.EventArgs) Handles txtClearing.Leave, txtKontoNr.Leave
		DetermineIBANFromClearingNumberAndAccountNumber()

		If Not String.IsNullOrWhiteSpace(txtClearing.EditValue) AndAlso
			String.IsNullOrWhiteSpace(txtBLZ.EditValue) AndAlso
			String.IsNullOrWhiteSpace(txtBankname.EditValue) AndAlso
			String.IsNullOrWhiteSpace(txBankAddress.EditValue) AndAlso
			String.IsNullOrWhiteSpace(txtSwift.EditValue) Then

			FillBankDataByClearingNumberOverWebService(RemoveLeadingZeros(txtClearing.EditValue), txtBankname.EditValue, txBankAddress.EditValue)

		End If

	End Sub

	''' <summary>
	''' Determines IBAN from clearing number and account number.
	''' </summary>
	Private Sub DetermineIBANFromClearingNumberAndAccountNumber()

		If String.IsNullOrWhiteSpace(txtClearing.Text) Or
			String.IsNullOrWhiteSpace(txtKontoNr.Text) Then
			Return
		End If

		Dim result = EncodeIBANOverWebService(txtClearing.Text, txtKontoNr.Text)

		If result Is Nothing Then
			Return
		End If

		If result.Success Then

			If String.IsNullOrWhiteSpace(txtIBAN.Text) Then
				txtIBAN.Text = result.IBAN
				errorProviderBankMng.SetError(txtIBAN, String.Empty)
			ElseIf txtIBAN.Text.Replace(" ", "") <> result.IBAN Then

				If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(String.Format("Die angegebene IBAN {0} stimmt nicht mit der aus der Clearing- und Kontonummer ermittelten IBAN {1} überein.", txtIBAN.Text, result.IBAN)) & vbCrLf &
													 m_Translate.GetSafeTranslationValue("Möchten Sie die aus Clearing- und Kontonummer ermittelte IBAN übernehmen?"))) Then

					txtIBAN.Text = result.IBAN
					errorProviderBankMng.SetError(txtIBAN, String.Empty)
				End If

			End If
		ElseIf Not result.Success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("IBAN konnte nicht aus Clearing- und Kontonummer ermittelt werden." & vbCrLf &
																m_Translate.GetSafeTranslationValue("Bitte prüfen Sie die eingetragene IBAN.")))
		End If

	End Sub

	''' <summary>
	''' Determines clearing number and account number from iban.
	''' </summary>
	Private Sub DetermineClearingNumberAndAccountNumberFromIBAN()
		If String.IsNullOrWhiteSpace(txtIBAN.Text) Then
			Return
		End If

		Dim result = DecodeIBANOverWebService(txtIBAN.EditValue)

		If result Is Nothing Then
			Return
		End If

		If (result.ResultCode = IBANDecodeResultCode.Success And
				Not String.IsNullOrWhiteSpace(result.Landcode)) Then

			Dim clearingNrWithNoLeadingZeros As String = RemoveLeadingZeros(txtClearing.Text)
			Dim resultBankIDWithNoLeadingZeros As String = RemoveLeadingZeros(result.BankID)

			Select Case result.Landcode
				Case "CH", "LI"

					' Check for mismatch of IBAN and clearing nummer textbox
					If String.IsNullOrWhiteSpace(clearingNrWithNoLeadingZeros) Then
						txtClearing.Text = resultBankIDWithNoLeadingZeros
						FillBankDataByClearingNumberOverWebService(resultBankIDWithNoLeadingZeros, txtBankname.EditValue, txBankAddress.EditValue)
					ElseIf (clearingNrWithNoLeadingZeros <> resultBankIDWithNoLeadingZeros) Then

						Dim doesClearingNumberStartWithResultBankID = (clearingNrWithNoLeadingZeros.Length >= 3 And clearingNrWithNoLeadingZeros.StartsWith(resultBankIDWithNoLeadingZeros))

						If (Not doesClearingNumberStartWithResultBankID AndAlso
								m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(String.Format("Die angegebene Clearingnummer {0} stimmt nicht mit der aus der IBAN ermittelten Clearingnummer {1} überein.", clearingNrWithNoLeadingZeros, resultBankIDWithNoLeadingZeros)) & vbCrLf &
																					 m_Translate.GetSafeTranslationValue("Möchten Sie die Clearingnummer aus der IBAN übernehmen?"))) Then

							txtClearing.Text = resultBankIDWithNoLeadingZeros
							FillBankDataByClearingNumberOverWebService(resultBankIDWithNoLeadingZeros, txtBankname.EditValue, txBankAddress.EditValue)
						End If
					End If

				Case Else ' All other countries

					' Check for mismatch of IBAN and BLZ number textbox
					If String.IsNullOrWhiteSpace(txtBLZ.Text) Then
						ClearBankData()
						txtBLZ.Text = result.BankID
					ElseIf RemoveLeadingZeros(txtBLZ.Text) <> RemoveLeadingZeros(result.BankID) Then
						If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(String.Format("Die angegebene BLZ {0} stimmt nicht mit der aus der IBAN ermittelten BLZ {1} überein.", txtBLZ.Text, result.BankID)) & vbCrLf &
																					 m_Translate.GetSafeTranslationValue("Möchten Sie die BLZ aus der IBAN übernehmen?"))) Then
							ClearBankData()
							txtBLZ.Text = result.BankID
						End If
					End If

			End Select

			Dim kontoNrWithNoLeadingZeros = RemoveLeadingZeros(txtKontoNr.Text)
			Dim resultKontoNrWithNoLeadingZeros = String.Empty

			' Sometimes the clearing number is attached to the KontoNr. If that is the case then remove the leading clearing nummer.
			If result.Landcode = "CH" And result.Kontonummer.StartsWith(resultBankIDWithNoLeadingZeros) Then
				resultKontoNrWithNoLeadingZeros = result.Kontonummer.Substring(resultBankIDWithNoLeadingZeros.Length)
			Else
				resultKontoNrWithNoLeadingZeros = RemoveLeadingZeros(result.Kontonummer)
			End If

			' Check for mismatch of IBAN an KontoNr textbox
			If String.IsNullOrWhiteSpace(kontoNrWithNoLeadingZeros) Then
				txtKontoNr.Text = resultKontoNrWithNoLeadingZeros
			ElseIf (kontoNrWithNoLeadingZeros <> resultKontoNrWithNoLeadingZeros) Then

				' The nummers do not match, but also check if result/Kontonummer may starts with BC number, then its ok. (Only for CH AND LI)
				Dim doesBCAndAccountNumberMatchWithCalculatedAccountNummer As Boolean = (result.Landcode = "CH" Or result.Landcode = "LI") And
																																									 (clearingNrWithNoLeadingZeros + kontoNrWithNoLeadingZeros = resultKontoNrWithNoLeadingZeros)

				If (Not doesBCAndAccountNumberMatchWithCalculatedAccountNummer AndAlso
						m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(String.Format("Die angegebene Kontonummer {0} stimmt nicht mit der aus der IBAN ermittelten Kontonummer {1} überein.", kontoNrWithNoLeadingZeros, resultKontoNrWithNoLeadingZeros)) & vbCrLf &
									 m_Translate.GetSafeTranslationValue("Möchten Sie die Kontonummer aus der IBAN übernehmen?"))) Then
					txtKontoNr.Text = resultKontoNrWithNoLeadingZeros
				End If

			End If

		End If
	End Sub


	''' <summary>
	''' Handles click on bank search form.
	''' </summary>
	Private Sub OnBtnOpenBankSearchForm_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenBankSearchForm.Click
		Dim bankSearchForm = New SearchBankData(m_InitializationData)
		bankSearchForm.StartPosition = FormStartPosition.CenterParent
		bankSearchForm.ClearingNumber = txtClearing.Text
		bankSearchForm.ShowDialog()

		If bankSearchForm.DialogResult = DialogResult.OK Then
			Dim bankData = bankSearchForm.SelectedBankData

			' Copy values.

			txtClearing.Text = bankData.ClearingNumber
			txtBankname.Text = bankData.BankName
			txBankAddress.Text = bankData.Location
			txtSwift.Text = bankData.Swift

			DetermineIBANFromClearingNumberAndAccountNumber()
			RemoveHandler txtClearing.Leave, AddressOf OnTxtClearingOrKontoNr_Leave
			txtKontoNr.Focus()
			AddHandler txtClearing.Leave, AddressOf OnTxtClearingOrKontoNr_Leave
		End If

	End Sub

	''' <summary>
	''' Presents bank detail data.
	''' </summary>
	''' <param name="bankViewData">The bank view data.</param>
	Private Sub PresentBankDetailData(ByVal bankViewData As BankViewData)

		If (bankViewData Is Nothing) Then
			Return
		End If

		m_CurrentBankRecordNumber = bankViewData.RecordNumber

		chkForSalary.Checked = bankViewData.BankLOL
		chkAsforeignBank.Checked = bankViewData.BankAU

		txtClearing.Text = bankViewData.DTABCNR

		txtBLZ.Text = bankViewData.BLZ
		txtBankname.Text = bankViewData.BankName
		txBankAddress.Text = bankViewData.BankLocation
		txtSwift.Text = bankViewData.Swift

		txtKontoNr.Text = bankViewData.AccountNumber
		txtIBAN.Text = bankViewData.IBAN

		txtOwner.Text = bankViewData.Address1
		txt1Address.Text = bankViewData.Address2
		txt2Address.Text = bankViewData.Address3
		txt3Address.Text = bankViewData.Address4

		chkAsStandard.Checked = bankViewData.Active
		chkForZG.Checked = bankViewData.BankZG

		lblBankCreated.Text = String.Format("{0:f}, {1}", bankViewData.CreatedOn, bankViewData.CreatedFrom)
		lblBankChanged.Text = String.Format("{0:f}, {1}", bankViewData.ChangedOn, bankViewData.ChangedFrom)

		' Clear errors
		ClearErrors()

	End Sub

	''' <summary>
	''' Prepares for a new bank.
	''' </summary>
	Private Sub PrepareForNew()

		m_CurrentBankRecordNumber = Nothing

		Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_EmployeeNumber)

		chkForSalary.Checked = False
		chkAsforeignBank.Checked = False

		txtClearing.Text = String.Empty
		txtBLZ.Text = String.Empty
		txtBankname.Text = String.Empty
		txBankAddress.Text = String.Empty
		txtSwift.Text = String.Empty

		txtKontoNr.Text = String.Empty
		txtIBAN.Text = String.Empty

		If Not employeeMasterData Is Nothing Then
			txtOwner.Text = String.Format("{0}, {1}", employeeMasterData.Lastname, employeeMasterData.Firstname).ToUpper()
			txt1Address.Text = employeeMasterData.Street.ToUpper()
			txt2Address.Text = String.Format("{0} {1}", employeeMasterData.Postcode, employeeMasterData.Location).ToUpper()
			txt3Address.Text = String.Empty
		Else
			txtOwner.Text = String.Empty
			txt1Address.Text = String.Empty
			txt2Address.Text = String.Empty
			txt3Address.Text = String.Empty
		End If

		chkAsStandard.Checked = False
		chkForZG.Checked = False

		lblBankCreated.Text = "-"
		lblBankChanged.Text = "-"

		' Clear errors
		ClearErrors()

	End Sub

	''' <summary>
	''' Focuses a bank.
	''' </summary>
	''' <param name="employeeNumber">The employee number.</param>
	''' <param name="bankRecordNumber">The bank record number</param>
	Private Sub FocusBank(ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer)

		If Not gridBank.DataSource Is Nothing Then

			Dim bankViewData = CType(gvBank.DataSource, BindingList(Of BankViewData))

			Dim index = bankViewData.ToList().FindIndex(Function(data) data.RecordNumber = bankRecordNumber)

			m_SuppressUIEvents = True
			Dim rowHandle = gvBank.GetRowHandle(index)
			gvBank.FocusedRowHandle = rowHandle
			m_SuppressUIEvents = False
		End If

	End Sub

	''' <summary>
	''' Validates bank data input data.
	''' </summary>
	Private Function ValidateBankInputData() As Boolean

		Dim missingFieldText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
		Dim errorClearingOrBLZMissing As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie eine Clearing-Nummer oder BLZ-Nummer ein.")
		Dim errorAccountNumberOrIBanMissing As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie eine Kontonummer und/oder IBAN-Nummer ein.")

		Dim isValid As Boolean = True

		Dim isClearingNumberEmpty As Boolean = String.IsNullOrEmpty(txtClearing.Text) AndAlso String.IsNullOrEmpty(txtBLZ.Text)
		isValid = isValid And SetErrorIfInvalid(txtClearing, errorProviderBankMng, isClearingNumberEmpty, errorClearingOrBLZMissing)
		isValid = isValid And SetErrorIfInvalid(txtBankname, errorProviderBankMng, String.IsNullOrEmpty(txtBankname.Text), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(txBankAddress, errorProviderBankMng, String.IsNullOrEmpty(txBankAddress.Text), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(txtSwift, errorProviderBankMng, String.IsNullOrEmpty(txtSwift.EditValue), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(txtIBAN, errorProviderBankMng, String.IsNullOrEmpty(txtIBAN.EditValue), errorAccountNumberOrIBanMissing)

		If isClearingNumberEmpty Then
			btnOpenBankSearchForm.Left = OPEN_BANK_SEARCH_LEFT_COORDINATE + OPEN_BANK_SEARCH_LEFT_OFFSET_ON_ERROR
		Else
			btnOpenBankSearchForm.Left = OPEN_BANK_SEARCH_LEFT_COORDINATE
		End If

		Return isValid
	End Function

	''' <summary>
	''' Decodes an IBAN over a web service interface.
	''' </summary>
	''' <param name="iban">The iban number.</param>
	''' <returns>The decode result or nothing in error case.</returns>
	Private Function DecodeIBANOverWebService(ByVal iban As String) As IBANDecodeResultViewData
		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)

		Try
			Dim result = baseTableData.PerformDecodingIBANOverWebService(iban)
			If result Is Nothing Then Return Nothing

			'Dim webservice As New SPIBANUtilWebService.SPIBANUtilSoapClient
			'webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_IBanUtilWebServiceUri)
			'Dim result As SPIBANUtilWebService.IBANDecodeResult = Nothing

			'result = webservice.DecodeIBAN(txtIBAN.Text)

			SetErrorIfInvalid(txtIBAN, errorProviderBankMng, result.ResultCode = IBANDecodeResultCode.InvalidIBAN, m_Translate.GetSafeTranslationValue("Ungültige IBAN Nummer."))

			If result.ResultCode = IBANDecodeResultCode.Failure Then
				Throw New Exception(String.Format("Decode of IBAN {0} failded.", iban))
			End If

			Return result

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("IBAN konnte nicht über Webservice-Schnittstelle in Clearing/Kontonummer zerlegt werden."))

			Return Nothing
		End Try

	End Function

	''' <summary>
	''' Encodes a clearing number and an account number to an IBAN number over a webservice interface.
	''' </summary>
	''' <param name="clearingNumber">The clearing number.</param>
	''' <param name="accountNummber">The account number.</param>
	''' <returns>Returns encode result or nothing in error case.</returns>
	Private Function EncodeIBANOverWebService(ByVal clearingNumber As String, ByVal accountNummber As String) As IBANConvertResultViewData
		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)

		Try
			Dim result = baseTableData.PerformEncodingIBANOverWebService(clearingNumber, accountNummber)
			If result Is Nothing Then Return Nothing

			'	Dim webservice As New SPIBANUtilWebService.SPIBANUtilSoapClient
			'webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_IBanUtilWebServiceUri)
			'Dim result As SPIBANUtilWebService.IBANConvertResult = Nothing

			'Try
			'	result = webservice.EncodeSwissIBAN(clearingNumber, accountNummber)
			Return result

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("IBAN konnte nicht über Webservice-Schnittstelle ermittelt werden."))

			Return Nothing
		End Try

	End Function

	''' <summary>
	''' Reads bank data over web service by clearing number.
	''' </summary>
	Private Sub FillBankDataByClearingNumberOverWebService(ByVal clearingNumber As String, ByVal bankName As String, ByVal bankLocation As String)
		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)

		Try
			Dim searchResult = baseTableData.PerformAssignedBankDataOverWebService(clearingNumber, bankName, bankLocation)

			If Not searchResult Is Nothing Then
				ClearBankData()

				txtClearing.Text = searchResult.ClearingNumber
				txtBankname.Text = searchResult.BankName
				txBankAddress.Text = searchResult.Location
				txtSwift.Text = searchResult.Swift

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht über Webservice abgefragt werden."))
		End Try


	End Sub

	''' <summary>
	''' Validates a control.
	''' </summary>
	''' <param name="control">The control to validate.</param>
	''' <param name="errorProvider">The error providor.</param>
	''' <param name="invalid">Boolean flag if data is invalid.</param>
	''' <param name="errorText">The error text.</param>
	''' <returns>Valid flag</returns>
	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	''' <summary>
	''' Clears the errors.
	''' </summary>
	Private Sub ClearErrors()
		errorProviderBankMng.Clear()
		btnOpenBankSearchForm.Left = OPEN_BANK_SEARCH_LEFT_COORDINATE
	End Sub

	''' <summary>
	''' Clears Bank data.
	''' </summary>
	Private Sub ClearBankData()
		txtClearing.Text = String.Empty
		txtBLZ.Text = String.Empty
		txtBankname.Text = String.Empty
		txBankAddress.Text = String.Empty
		txtSwift.Text = String.Empty
	End Sub

	''' <summary>
	''' Gets bank view data by bank record  number.
	''' </summary>
	''' <param name="recordNumber">The bank record number.</param>
	''' <returns>The bank view data or nothing.</returns>
	''' 
	Private Function GetBankViewDataByRecordNumber(ByVal recordNumber As Integer) As BankViewData

		If gvBank.DataSource Is Nothing Then
			Return Nothing
		End If

		Dim bankViewData = CType(gvBank.DataSource, BindingList(Of BankViewData))

		Dim viewData = bankViewData.Where(Function(data) data.RecordNumber = recordNumber).FirstOrDefault

		Return viewData
	End Function

	''' <summary>
	''' Removes leading Zeros of string.
	''' </summary>
	''' <param name="str">The string.</param>
	''' <returns>String with removed leading zeros.</returns>
	Private Function RemoveLeadingZeros(ByVal str As String) As String

		If String.IsNullOrWhiteSpace(str) Then
			Return String.Empty
		Else
			Return str.Trim().TrimStart("0")
		End If

	End Function

#End Region

#Region "View helper classes"

	''' <summary>
	''' Bank view data.
	''' </summary>
	Class BankViewData
		Public Property ID As Integer
		Public Property EmployeeNumber As Integer?
		Public Property RecordNumber As Integer?
		Public Property BankLOL As Boolean
		Public Property BankAU As Boolean
		Public Property DTABCNR As String
		Public Property BLZ As String
		Public Property BankName As String
		Public Property BankLocation As String
		Public Property Swift As String
		Public Property AccountNumber As String
		Public Property IBAN As String
		Public Property Address1 As String
		Public Property Address2 As String
		Public Property Address3 As String
		Public Property Address4 As String
		Public Property Active As Boolean
		Public Property BankZG As Boolean
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

		''' <summary>
		''' Gets the bank name for grid.
		''' </summary>
		''' <returns>Bank name for grid.</returns>
		Public ReadOnly Property BankNameForGrid As String
			Get

				If Not String.IsNullOrEmpty(BankLocation) Then
					Return String.Format("{0}, {1}", BankName, BankLocation)
				Else
					Return BankName
				End If
			End Get
		End Property

	End Class

#End Region

End Class