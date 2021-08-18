Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Threading
Imports SP.MA.BankMng.Settings
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SP.Internal.Automations

''' <summary>
''' Search bank data.
''' </summary>
Public Class SearchBankData


#Region "Privte Fields"

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

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' Service Uri of Sputnik bank util webservice.
	''' </summary>
	Private m_BankUtilWebServiceUri As String

	''' <summary>
	''' The settings manager.
	''' </summary>
	Protected m_SettingsManager As ISettingsManager

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
	''' </summary>
	Private m_SuppressUIEvents As Boolean = False

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_SettingsManager = New SettingsManager
		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New Utility

		Try
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		gvBank.OptionsView.ShowIndicator = False

		Reset()

	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets or sets the clearing number.
	''' </summary>
	Public Property ClearingNumber As String
		Get
			Return txtClearingNumber.Text
		End Get
		Set(value As String)
			txtClearingNumber.Text = value
		End Set
	End Property

	''' <summary>
	''' Gets the selected bank.
	''' </summary>
	''' <returns>The selected bank or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedBankData As BankstammViewData
		Get
			Dim grdView = TryCast(gridBank.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim bank = CType(grdView.GetRow(selectedRows(0)), BankstammViewData)
					Return bank
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region

#Region "Private Methods"

	''' <summary>
	'''  Translate controls
	''' </summary>
	Private Sub TranslateControls()

		Text = m_Translate.GetSafeTranslationValue(Text)

		groupDataSource.Properties.Items(0).Description = m_Translate.GetSafeTranslationValue(groupDataSource.Properties.Items(0).Description)
		groupDataSource.Properties.Items(1).Description = m_Translate.GetSafeTranslationValue(groupDataSource.Properties.Items(1).Description)

		lblClearing.Text = m_Translate.GetSafeTranslationValue(lblClearing.Text)
		lblBankname.Text = m_Translate.GetSafeTranslationValue(lblBankname.Text)
		lblBLZ.Text = m_Translate.GetSafeTranslationValue(lblBLZ.Text)
		lblBankOrt.Text = m_Translate.GetSafeTranslationValue(lblBankOrt.Text)
		lblSwift.Text = m_Translate.GetSafeTranslationValue(lblSwift.Text)

		btnSearch.Text = m_Translate.GetSafeTranslationValue(btnSearch.Text)
		btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

		lblSearchInfo.Text = m_Translate.GetSafeTranslationValue(lblSearchInfo.Text)
		lblFoundItems.Text = m_Translate.GetSafeTranslationValue(lblFoundItems.Text)

	End Sub

	''' <summary>
	''' Resets the form.
	''' </summary>
	Private Sub Reset()

		txtClearingNumber.Text = String.Empty
		txtClearingNumber.Properties.MaxLength = 5

		txtBankname.Text = String.Empty
		txtBankname.Properties.MaxLength = 100

		txtPLZ.Text = String.Empty
		txtPLZ.Properties.MaxLength = 50

		txtLocation.Text = String.Empty
		txtLocation.Properties.MaxLength = 100

		txtSwift.Text = String.Empty
		txtSwift.Properties.MaxLength = 50

		lblSearchInfo.Visible = False

		' ---Reset drop downs, grids and lists---

		ResetBankGrid()

		' Translate controls
		TranslateControls()

	End Sub

	''' <summary>
	''' Resets the bank grid.
	''' </summary>
	Private Sub ResetBankGrid()

		' Reset the grid
		gvBank.OptionsView.ShowIndicator = False
		gvBank.OptionsView.ColumnAutoWidth = False

		gvBank.Columns.Clear()

		Dim columnClearningNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnClearningNr.Caption = m_Translate.GetSafeTranslationValue("Clearing")
		columnClearningNr.Name = "ClearingNumber"
		columnClearningNr.FieldName = "ClearingNumber"
		columnClearningNr.Width = 60
		columnClearningNr.Visible = True
		gvBank.Columns.Add(columnClearningNr)

		Dim columnBankname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBankname.Caption = m_Translate.GetSafeTranslationValue("Bank")
		columnBankname.Name = "BankName"
		columnBankname.FieldName = "BankName"
		columnBankname.Width = 270
		columnBankname.Visible = True
		gvBank.Columns.Add(columnBankname)

		Dim columnPostCodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostCodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnPostCodeAndLocation.Name = "PostcodeAndLocation"
		columnPostCodeAndLocation.FieldName = "PostcodeAndLocation"
		columnPostCodeAndLocation.Width = 130
		columnPostCodeAndLocation.Visible = True
		gvBank.Columns.Add(columnPostCodeAndLocation)

		Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
		columnSwift.Name = "Swift"
		columnSwift.FieldName = "Swift"
		columnSwift.Width = 130
		columnSwift.Visible = True
		gvBank.Columns.Add(columnSwift)

		Dim columnTelephone As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelephone.Caption = m_Translate.GetSafeTranslationValue("Telefon")
		columnTelephone.Name = "Telephone"
		columnTelephone.FieldName = "Telephone"
		columnTelephone.Width = 100
		columnTelephone.Visible = True
		gvBank.Columns.Add(columnTelephone)

		Dim columnTelefax As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTelefax.Caption = m_Translate.GetSafeTranslationValue("Telefax")
		columnTelefax.Name = "Telefax"
		columnTelefax.FieldName = "Telefax"
		columnTelefax.Width = 100
		columnTelefax.Visible = True
		gvBank.Columns.Add(columnTelefax)

		Dim columnPostAccount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostAccount.Caption = m_Translate.GetSafeTranslationValue("Post-Konto")
		columnPostAccount.Name = "PostAccount"
		columnPostAccount.FieldName = "PostAccount"
		columnPostAccount.Width = 130
		columnPostAccount.Visible = True
		gvBank.Columns.Add(columnPostAccount)

		m_SuppressUIEvents = True
		gridBank.DataSource = Nothing
		m_SuppressUIEvents = False
	End Sub

	''' <summary>
	''' Handles click on close button.
	''' </summary>
	Private Sub OnBtnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
		DialogResult = DialogResult.None
		Close()
	End Sub

	''' <summary>
	''' Handles the form disposed event.
	''' </summary>
	Private Sub OnSearchBankData_FormClosing(sender As Object, e As System.EventArgs) Handles Me.FormClosing

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_SEARCH_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_SEARCH_WIDTH, Me.Width)
				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_SEARCH_HEIGHT, Me.Height)

				m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_SEARCH_DATASOURCE, groupDataSource.SelectedIndex)

				m_SettingsManager.SaveSettings()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles form load event.
	''' </summary>
	Private Sub OnSearchBankData_Load(sender As Object, e As System.EventArgs) Handles Me.Load

		Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_MandantData.GetDefaultUSNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Dim setting_form_search_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_SEARCH_HEIGHT)
			Dim setting_form_search_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_SEARCH_WIDTH)
			Dim setting_form_search_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_SEARCH_LOCATION)
			Dim setting_form_search_datasource = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_SEARCH_DATASOURCE)

			If setting_form_search_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_search_height)
			If setting_form_search_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_search_width)

			If Not String.IsNullOrEmpty(setting_form_search_location) Then
				Dim aLoc As String() = setting_form_search_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If
			groupDataSource.SelectedIndex = setting_form_search_datasource

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Handles click on search button.
	''' </summary>
	Private Sub OnBtnSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnSearch.Click

		If groupDataSource.SelectedIndex = 0 Then
			SearchViaLocalDatabase()
		Else
			SearchViaWebService()
		End If

	End Sub

	''' <summary>
	''' Search form local data base.
	''' </summary>
	Private Sub SearchViaLocalDatabase()

		Dim searchResult = m_EmployeeDatabaseAccess.SearchBankData(txtClearingNumber.Text, txtBankname.Text, txtPLZ.Text, txtLocation.Text, txtSwift.Text)

		If (searchResult Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Suche ist fehlgeschlagen."))
			Return
		End If

		Dim listDataSource As BindingList(Of BankstammViewData) = New BindingList(Of BankstammViewData)

		For Each result In searchResult

			Dim viewData = New BankstammViewData With {
				.ClearingNumber = result.ClearingNumber,
				.BankName = result.BankName,
				.Postcode = result.Postcode,
				.Location = result.Location,
				.Swift = result.Swift,
				.Telephone = result.Telephone,
				.Telefax = result.Telefax,
				.PostAccount = result.PostAccount
			}

			listDataSource.Add(viewData)

		Next

		m_SuppressUIEvents = True
		gridBank.DataSource = listDataSource
		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Search over web service.
	''' </summary>
	Private Sub SearchViaWebService()

		Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

		btnSearch.Enabled = False
		lblSearchInfo.Visible = True

		Task(Of BindingList(Of BankstammViewData)).Factory.StartNew(Function() PerformWebserviceCallAsync(),
																						CancellationToken.None,
																						TaskCreationOptions.None,
																						TaskScheduler.Default).ContinueWith(Sub(t) FinishWebserviceCallTask(t), CancellationToken.None, TaskContinuationOptions.None, uiSynchronizationContext)
	End Sub


	''' <summary>
	'''  Performs the check asynchronous.
	''' </summary>
	''' <returns>The report response.</returns>
	Private Function PerformWebserviceCallAsync() As BindingList(Of BankstammViewData)
		Dim baseTableData As New BaseTable.SPSBaseTables(m_InitializationData)

		Dim listDataSource As BindingList(Of BankstammViewData) = New BindingList(Of BankstammViewData)

		'Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
		'webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

		'' Read data over webservice
		'Dim searchResult As BankSearchResultDTO() = webservice.GetBankData(txtClearingNumber.Text, txtBankname.Text, txtPLZ.Text, txtLocation.Text, txtSwift.Text)

		Dim searchResult = baseTableData.PerformBankDataOverWebService(txtClearingNumber.EditValue, txtBankname.EditValue, txtPLZ.EditValue, txtLocation.EditValue, txtSwift.EditValue)
		If searchResult Is Nothing Then Return Nothing

		For Each result In searchResult

			Dim viewData = New BankstammViewData With {
				.ClearingNumber = result.ClearingNumber,
				.BankName = result.BankName,
				.Postcode = result.Postcode,
				.Location = result.Location,
				.Swift = result.Swift,
				.Telephone = result.Telephone,
				.Telefax = result.Telefax,
				.PostAccount = result.PostAccount
			}

			listDataSource.Add(viewData)

		Next

		Return listDataSource

	End Function

	''' <summary>
	''' Finish call.
	''' </summary>
	Private Sub FinishWebserviceCallTask(ByVal t As Task(Of BindingList(Of BankstammViewData)))

		Select Case t.Status
			Case TaskStatus.RanToCompletion
				' Webservice call was successful.
				m_SuppressUIEvents = True
				gridBank.DataSource = t.Result
				m_SuppressUIEvents = False

			Case TaskStatus.Faulted
				' Something went wrong -> log error.
				m_Logger.LogError(t.Exception.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))


			Case Else
				' Do nothing
		End Select

		btnSearch.Enabled = True
		lblSearchInfo.Visible = False

	End Sub

	''' <summary>
	''' Handles double click on bank row.
	''' </summary>
	Private Sub OnGvBank_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvBank.DoubleClick
		DialogResult = DialogResult.OK
		Close()
	End Sub

#End Region

#Region "View helper classes"

	'''' <summary>
	'''' Bank search view data.
	'''' </summary>
	'Class BankSearchViewData

	'	Public Property ClearingNr As String
	'	Public Property BankName As String
	'	Public Property Postcode As String
	'	Public Property Location As String
	'	Public Property Swift As String
	'	Public Property Telephone As String
	'	Public Property Telefax As String
	'	Public Property PostAccount As String

	'	Public ReadOnly Property PostcodeAndLocation As String
	'		Get
	'			Return String.Format("{0} {1}", Postcode, Location)
	'		End Get
	'	End Property

	'End Class

#End Region

End Class