
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports System.ComponentModel
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.DateAndTimeCalculation
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Settings
Imports SP.KD.InvoiceMng.Settings
Imports DevExpress.LookAndFeel
Imports DevExpress.Pdf
Imports System.IO
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.Utils


Namespace UI

	Public Class frmNewAutomatedInvoice

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
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The invoice data access object.
		''' </summary>
		Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		Private m_dateUtility As DateAndTimeUtily

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False


		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False


		''' <summary>
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_AutomatedUtility As CreateAutomatedInvoices


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
		''' The preselection data.
		''' </summary>
		Private m_PreselectionData As PreselectionData

		Private m_Debitorenart As List(Of DebitorenAutomatedArt)
		Private m_BankData As List(Of SP.DatabaseAccess.Invoice.DataObjects.BankData)
		Private m_MahnCodeData As List(Of DatabaseAccess.Customer.DataObjects.PaymentReminderCodeData) = Nothing


		''' <summary>
		''' The posting accounts.
		''' </summary>
		Private m_PostingAccounts As New Dictionary(Of Integer, String)

		Private m_InvoiceNumberOffsetFromSettings As Integer
		Private m_MwStNr As String
		Private m_Currency As String
		Private m_MwStAnsatz As Decimal
		Private m_InvoiceDateAsEndOfReportDate As Boolean
		Private m_ReferenceNumbersTo10Setting As Boolean

		Private m_InvoiceData As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)


#End Region


#Region "Private Properties"

		''' <summary>
		''' Gets the currency setting.
		''' </summary>
		Private ReadOnly Property CurrencySetting As String
			Get

				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

				Dim currencyvalue As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY))

				Return currencyvalue

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt number setting.
		''' </summary>
		Private ReadOnly Property MwStNrSetting As String
			Get
				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstNumber As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mwstnr", FORM_XML_MAIN_KEY))

				Return mwstNumber

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt Ansatz setting.
		''' </summary>
		Private ReadOnly Property MwStAnsatz(ByVal mdYear As Integer) As Decimal
			Get

				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

			End Get

		End Property

		''' <summary>
		''' Gets the reference number to 10 setting.
		''' </summary>
		Private ReadOnly Property ReferenceNumbersTo10Setting As Boolean
			Get

				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim ref10forfactoring As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/ref10forfactoring", FORM_XML_MAIN_KEY)), False)

				Return ref10forfactoring.HasValue AndAlso ref10forfactoring

			End Get

		End Property

		''' <summary>
		''' Gets the currency setting.
		''' </summary>
		Private ReadOnly Property InvoiceDateAsEndOfMonth As Boolean
			Get

				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim setDateToEndofReportMonth As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/setfakdatetoendofreportmonth", FORM_XML_MAIN_KEY)), False)

				Return setDateToEndofReportMonth.HasValue AndAlso setDateToEndofReportMonth

			End Get

		End Property

		''' <summary>
		''' Gets the selected invoice data.
		''' </summary>
		''' <returns>The selected document or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedInvoiceViewData As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Get
				Dim grdView = TryCast(grdInvoices.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim report = CType(grdView.GetRow(selectedRows(0)), SP.DatabaseAccess.Invoice.DataObjects.Invoice)
						Return report
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region



#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="_setting">The settings.</param>
		''' <param name="preselectionData">The preselection data.</param>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal preselectionData As PreselectionData)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				' Mandantendaten
				m_Mandant = New Mandant
				m_path = New ClsProgPath
				m_Common = New CommonSetting

				m_SettingsManager = New SettingsManager
				m_UtilityUI = New UtilityUI
				m_Utility = New Utility
				m_dateUtility = New DateAndTimeUtily

				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
				m_PreselectionData = preselectionData

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()

			AddHandler gvIndData.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			AddHandler gvInvoices.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
			m_SuppressUIEvents = False

			Dim conStr = m_InitializationData.MDData.MDDbConn
			m_CurrentConnectionString = conStr

			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

			' Translate controls.
			TranslateControls()

			Reset()

			AddHandler gvInvoices.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler gvIndData.RowCellClick, AddressOf OngvIndData_RowCellClick

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets or sets the preselection data.
		''' </summary>
		Public Property PreselectionData As PreselectionData

		''' <summary>
		''' Gets the common db access object.
		''' </summary>
		Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
			Get
				Return m_CommonDatabaseAccess
			End Get
		End Property

		''' <summary>
		''' Gets the invoice db access object.
		''' </summary>
		Public ReadOnly Property InvoiceDbAccess As IInvoiceDatabaseAccess
			Get
				Return m_InvoiceDatabaseAccess
			End Get
		End Property

		''' <summary>
		''' Gets the customer db access object.
		''' </summary>
		Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess
			Get
				Return m_CustomerDatabaseAccess
			End Get
		End Property


#End Region

#Region "Public Methods"

		Public Function LoadData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadMandantDropDownData()
			lueMandant.EditValue = m_InitializationData.MDData.MDNr

			success = success AndAlso LoadBankdatenDropDown(True)
			success = success AndAlso LoadDebitorenartDropDown()
			success = success AndAlso LoadMandantPaymentReminderData()
			LoadCreatedInvoices()

			PreselectData()

			' create popup menu
			CreatePrintPopupMenu()


			Return success

		End Function


#End Region


#Region "Private Methods"

		''' <summary>
		''' Resets the control.
		''' </summary>
		Private Sub Reset()

			m_InvoiceData = Nothing
			m_Debitorenart = Nothing
			m_BankData = Nothing

			'  Reset drop downs and lists
			ResetMandantDropDown()
			ResetDebitorenartDropDown()
			ResetBankdatenDropDown()
			ResetInvoiceGrid()

		End Sub

		Private Sub ResetAfterMandantChange()

			m_InvoiceData = Nothing
			m_Debitorenart = Nothing
			m_BankData = Nothing

			'  Reset drop downs and lists
			ResetBankdatenDropDown()
			ResetInvoiceGrid()

		End Sub

		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

			If hasPreselectionData Then

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

				' ---Mandant---
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

					If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

						' Mandant is required
						lueMandant.EditValue = PreselectionData.MDNr

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						m_SuppressUIEvents = supressUIEventState
						Return
					End If

				End If

				m_SuppressUIEvents = supressUIEventState

			Else
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

					If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

						' Mandant is required
						lueMandant.EditValue = m_InitializationData.MDData.MDNr

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						Return
					End If

				End If

			End If


			'bbiPrint.Enabled = False
			'bbiDelete.Enabled = False
			'bbiExport.Enabled = False

		End Sub



		''' <summary>
		''' Translates the controls
		''' </summary>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
			lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
			btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

			lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)
			lblBankdaten.Text = m_Translate.GetSafeTranslationValue(lblBankdaten.Text)
			lblDebitorenart.Text = m_Translate.GetSafeTranslationValue(lblDebitorenart.Text)

			tgsSelectionIndividuell.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelectionIndividuell.Properties.OffText)
			tgsSelectionIndividuell.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelectionIndividuell.Properties.OnText)

			lblErstellteRechnungen.Text = m_Translate.GetSafeTranslationValue(lblErstellteRechnungen.Text)
			lblIndividuelleDaten.Text = m_Translate.GetSafeTranslationValue(lblIndividuelleDaten.Text)

			bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue(bsiPrintinfo.Caption)
			bbiCreate.Caption = m_Translate.GetSafeTranslationValue(bbiCreate.Caption)
			bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)

		End Sub


#Region "Event Handles"

		Private Sub OnfrmNewAutomatedInvoice_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
			SaveFromSettings()
		End Sub


		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmInvoices_Load(sender As Object, e As System.EventArgs) Handles Me.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnFrmInvoices_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

			If Visible Then
				LoadFormSettings()
			End If

		End Sub

		Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Me.Close()
		End Sub


#End Region

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_CREATE_AUTOMATED_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_CREATE_AUTOMATED_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_CREATE_AUTOMATED_LOCATION)
				Dim setting_form_sccMainPos = m_SettingsManager.ReadInteger(SettingKeys.SETTING_SCC_CREATE_AUTOMATED_MAINPOS)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

				If Not String.IsNullOrEmpty(setting_form_location) Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If
				If setting_form_sccMainPos > 0 Then Me.sccMain.SplitterPosition = Math.Max(setting_form_sccMainPos, 10)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Saves the form settings.
		''' </summary>
		Private Sub SaveFromSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_CREATE_AUTOMATED_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_CREATE_AUTOMATED_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_CREATE_AUTOMATED_HEIGHT, Me.Height)

					m_SettingsManager.WriteInteger(SettingKeys.SETTING_SCC_CREATE_AUTOMATED_MAINPOS, Me.sccMain.SplitterPosition)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Resets the Mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"

			lueMandant.Properties.Columns.Clear()
			lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})
			lueMandant.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the Bankdaten drop down.
		''' </summary>
		Private Sub ResetBankdatenDropDown()

			lueBankdaten.Properties.DisplayMember = "BankName"
			lueBankdaten.Properties.ValueMember = "ID" '"BankName"

			lueBankdaten.Properties.Columns.Clear()
			lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("KontoESR2", 0))
			lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("BankName", 0))
			'lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("ID", 0))

			lueBankdaten.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the Debitorenart drop down.
		''' </summary>
		Private Sub ResetDebitorenartDropDown()

			lueDebitorenart.Properties.DisplayMember = "Label"
			lueDebitorenart.Properties.ValueMember = "Value"

			lueDebitorenart.Properties.Columns.Clear()
			lueDebitorenart.Properties.Columns.Add(New LookUpColumnInfo("Value", 0))
			lueDebitorenart.Properties.Columns.Add(New LookUpColumnInfo("Display", 0))

			lueDebitorenart.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the invoice grid.
		''' </summary>
		Private Sub ResetInvoiceGrid()

			gvInvoices.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvInvoices.OptionsView.ShowIndicator = False
			gvInvoices.OptionsBehavior.Editable = True
			gvInvoices.OptionsView.ShowAutoFilterRow = True
			gvInvoices.OptionsView.ColumnAutoWidth = True
			gvInvoices.OptionsView.ShowFooter = True
			gvInvoices.OptionsView.AllowHtmlDrawGroups = True

			gvInvoices.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 50
			gvInvoices.Columns.Add(columnIsSelected)

			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnCustomerNumber.OptionsColumn.AllowEdit = False
			columnCustomerNumber.Name = "ReNr"
			columnCustomerNumber.FieldName = "ReNr"
			columnCustomerNumber.Width = 250
			columnCustomerNumber.Visible = True
			columnCustomerNumber.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			'columnCustomerNumber.to = "Anzahl Datensätze: {0:f2}"
			gvInvoices.Columns.Add(columnCustomerNumber)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany1.OptionsColumn.AllowEdit = False
			columnCompany1.Name = "RName1"
			columnCompany1.FieldName = "RName1"
			columnCompany1.Width = 300
			columnCompany1.Visible = True
			gvInvoices.Columns.Add(columnCompany1)

			Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnStreet.OptionsColumn.AllowEdit = False
			columnStreet.Name = "RStrasse"
			columnStreet.FieldName = "RStrasse"
			columnStreet.Width = 300
			columnStreet.Visible = True
			gvInvoices.Columns.Add(columnStreet)

			Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnPostcodeAndLocation.OptionsColumn.AllowEdit = False
			columnPostcodeAndLocation.Name = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.FieldName = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.Width = 300
			columnPostcodeAndLocation.Visible = True
			gvInvoices.Columns.Add(columnPostcodeAndLocation)

			Dim columninvoiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columninvoiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columninvoiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columninvoiceDate.OptionsColumn.AllowEdit = False
			columninvoiceDate.Name = "FakDat"
			columninvoiceDate.FieldName = "FakDat"
			columninvoiceDate.Width = 300
			columninvoiceDate.Visible = True
			gvInvoices.Columns.Add(columninvoiceDate)


			Dim columnBetragOhne As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragOhne.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBetragOhne.Caption = m_Translate.GetSafeTranslationValue("Betrag exkl. MwSt.")
			columnBetragOhne.OptionsColumn.AllowEdit = False
			columnBetragOhne.Name = "BetragOhne"
			columnBetragOhne.FieldName = "BetragOhne"
			columnBetragOhne.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragOhne.AppearanceHeader.Options.UseTextOptions = True
			columnBetragOhne.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragOhne.DisplayFormat.FormatString = "N2"
			columnBetragOhne.Width = 500
			columnBetragOhne.Visible = False
			columnBetragOhne.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnBetragOhne.SummaryItem.DisplayFormat = "{0:n2}"
			gvInvoices.Columns.Add(columnBetragOhne)

			Dim columnBetragEx As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragEx.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBetragEx.Caption = m_Translate.GetSafeTranslationValue("Betrag exkl. MwSt.")
			columnBetragEx.OptionsColumn.AllowEdit = False
			columnBetragEx.Name = "BetragEx"
			columnBetragEx.FieldName = "BetragEx"
			columnBetragEx.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragEx.AppearanceHeader.Options.UseTextOptions = True
			columnBetragEx.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragEx.DisplayFormat.FormatString = "N2"
			columnBetragEx.Width = 400
			columnBetragEx.Visible = True
			columnBetragEx.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnBetragEx.SummaryItem.DisplayFormat = "{0:n2}"
			gvInvoices.Columns.Add(columnBetragEx)

			Dim columnMWST1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMWST1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMWST1.Caption = m_Translate.GetSafeTranslationValue("MwSt.-Betrag")
			columnMWST1.OptionsColumn.AllowEdit = False
			columnMWST1.Name = "MWST1"
			columnMWST1.FieldName = "MWST1"
			columnMWST1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnMWST1.AppearanceHeader.Options.UseTextOptions = True
			columnMWST1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMWST1.DisplayFormat.FormatString = "n2"
			columnMWST1.Width = 300
			columnMWST1.Visible = True
			columnMWST1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnMWST1.SummaryItem.DisplayFormat = "{0:n2}"
			gvInvoices.Columns.Add(columnMWST1)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag inkl. MwSt.")
			columnBetragInk.OptionsColumn.AllowEdit = False
			columnBetragInk.Name = "BetragInk"
			columnBetragInk.FieldName = "BetragInk"
			columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "n2"
			columnBetragInk.Width = 500
			columnBetragInk.Visible = True
			columnBetragInk.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnBetragInk.SummaryItem.DisplayFormat = "{0:n2}"
			gvInvoices.Columns.Add(columnBetragInk)


			m_SuppressUIEvents = True
			grdInvoices.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets the customer grid.
		''' </summary>
		Private Sub ResetCustomerGrid()

			gvIndData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvIndData.OptionsView.ShowIndicator = False
			gvIndData.OptionsBehavior.Editable = True
			gvIndData.OptionsView.ShowAutoFilterRow = True
			gvIndData.OptionsView.ColumnAutoWidth = True
			gvIndData.OptionsView.ShowFooter = True
			gvIndData.OptionsView.AllowHtmlDrawGroups = True

			gvIndData.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 50
			gvIndData.Columns.Add(columnIsSelected)

			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnCustomerNumber.OptionsColumn.AllowEdit = False
			columnCustomerNumber.Name = "CustomerNumber"
			columnCustomerNumber.FieldName = "CustomerNumber"
			columnCustomerNumber.Width = 200
			columnCustomerNumber.Visible = True
			columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCustomerNumber)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany1.OptionsColumn.AllowEdit = False
			columnCompany1.Name = "Company1"
			columnCompany1.FieldName = "Company1"
			columnCompany1.Width = 300
			columnCompany1.Visible = True
			columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCompany1)

			Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnStreet.OptionsColumn.AllowEdit = False
			columnStreet.Name = "Street"
			columnStreet.FieldName = "Street"
			columnStreet.Width = 200
			columnStreet.Visible = True
			columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnStreet)

			Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnPostcodeAndLocation.OptionsColumn.AllowEdit = False
			columnPostcodeAndLocation.Name = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.FieldName = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.Width = 300
			columnPostcodeAndLocation.Visible = True
			columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnPostcodeAndLocation)

			Dim columnReportLineBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportLineBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnReportLineBetrag.OptionsColumn.AllowEdit = False
			columnReportLineBetrag.Name = "ReportLineBetrag"
			columnReportLineBetrag.FieldName = "ReportLineBetrag"
			columnReportLineBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnReportLineBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnReportLineBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnReportLineBetrag.DisplayFormat.FormatString = "N2"
			columnReportLineBetrag.Width = 150
			columnReportLineBetrag.Visible = True
			columnReportLineBetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnReportLineBetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnReportLineBetrag.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnReportLineBetrag)


			m_SuppressUIEvents = True
			grdIndData.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets the report grid.
		''' </summary>
		Private Sub ResetReportGrid()

			gvIndData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvIndData.OptionsView.ShowIndicator = False
			gvIndData.OptionsBehavior.Editable = True
			gvIndData.OptionsView.ShowAutoFilterRow = True
			gvIndData.OptionsView.ColumnAutoWidth = True
			gvIndData.OptionsView.ShowFooter = True
			gvIndData.OptionsView.AllowHtmlDrawGroups = True

			gvIndData.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 50
			gvIndData.Columns.Add(columnIsSelected)

			Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnRPNr.OptionsColumn.AllowEdit = False
			columnRPNr.Name = "RPNr"
			columnRPNr.FieldName = "RPNr"
			columnRPNr.Width = 200
			columnRPNr.Visible = True
			columnRPNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnRPNr)

			Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullName.OptionsColumn.AllowEdit = False
			columnEmployeeFullName.Name = "EmployeeFullName"
			columnEmployeeFullName.FieldName = "EmployeeFullName"
			columnEmployeeFullName.Width = 300
			columnEmployeeFullName.Visible = True
			columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnEmployeeFullName)

			Dim columnCustomer1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCustomer1.OptionsColumn.AllowEdit = False
			columnCustomer1.Name = "Company1"
			columnCustomer1.FieldName = "Company1"
			columnCustomer1.Width = 300
			columnCustomer1.Visible = True
			columnCustomer1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCustomer1)

			Dim columnReportMonthAndYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportMonthAndYear.Caption = m_Translate.GetSafeTranslationValue("Zeit")
			columnReportMonthAndYear.OptionsColumn.AllowEdit = False
			columnReportMonthAndYear.Name = "ReportMonthAndYear"
			columnReportMonthAndYear.FieldName = "ReportMonthAndYear"
			columnReportMonthAndYear.Visible = False
			columnReportMonthAndYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnReportMonthAndYear)

			Dim columnReportFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportFrom.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnReportFrom.OptionsColumn.AllowEdit = False
			columnReportFrom.Name = "ReportFrom"
			columnReportFrom.FieldName = "ReportFrom"
			columnReportFrom.Width = 250
			columnReportFrom.Visible = True
			columnReportFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnReportFrom)

			Dim columnReportTo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportTo.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnReportTo.OptionsColumn.AllowEdit = False
			columnReportTo.Name = "ReportTo"
			columnReportTo.FieldName = "ReportTo"
			columnReportTo.Width = 250
			columnReportTo.Visible = True
			columnReportTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnReportTo)

			Dim columnReportLineBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportLineBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnReportLineBetrag.OptionsColumn.AllowEdit = False
			columnReportLineBetrag.Name = "ReportLineBetrag"
			columnReportLineBetrag.FieldName = "ReportLineBetrag"
			columnReportLineBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnReportLineBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnReportLineBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnReportLineBetrag.DisplayFormat.FormatString = "N2"
			columnReportLineBetrag.Width = 300
			columnReportLineBetrag.Visible = True
			columnReportLineBetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnReportLineBetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnReportLineBetrag.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnReportLineBetrag)


			m_SuppressUIEvents = True
			grdIndData.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets the report costcenter grid.
		''' </summary>
		Private Sub ResetReportCostcenterGrid()

			gvIndData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvIndData.OptionsView.ShowIndicator = False
			gvIndData.OptionsBehavior.Editable = True
			gvIndData.OptionsView.ShowAutoFilterRow = True
			gvIndData.OptionsView.ColumnAutoWidth = True
			gvIndData.OptionsView.ShowFooter = True
			gvIndData.OptionsView.AllowHtmlDrawGroups = True

			gvIndData.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 50
			gvIndData.Columns.Add(columnIsSelected)

			Dim columnRPNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnRPNr.OptionsColumn.AllowEdit = False
			columnRPNr.Name = "RPNr"
			columnRPNr.FieldName = "RPNr"
			columnRPNr.Width = 200
			columnRPNr.Visible = True
			columnRPNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnRPNr)

			Dim columnKSTBez As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKSTBez.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
			columnKSTBez.OptionsColumn.AllowEdit = False
			columnKSTBez.Name = "KSTBez"
			columnKSTBez.FieldName = "KSTBez"
			columnKSTBez.Width = 300
			columnKSTBez.Visible = True
			columnKSTBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnKSTBez)

			Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullName.OptionsColumn.AllowEdit = False
			columnEmployeeFullName.Name = "EmployeeFullName"
			columnEmployeeFullName.FieldName = "EmployeeFullName"
			columnEmployeeFullName.Width = 300
			columnEmployeeFullName.Visible = True
			columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnEmployeeFullName)

			Dim columnCustomer1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCustomer1.OptionsColumn.AllowEdit = False
			columnCustomer1.Name = "Company1"
			columnCustomer1.FieldName = "Company1"
			columnCustomer1.Width = 300
			columnCustomer1.Visible = True
			columnCustomer1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCustomer1)

			Dim columnReportMonthAndYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportMonthAndYear.Caption = m_Translate.GetSafeTranslationValue("Zeit")
			columnReportMonthAndYear.OptionsColumn.AllowEdit = False
			columnReportMonthAndYear.Name = "ReportMonthAndYear"
			columnReportMonthAndYear.FieldName = "ReportMonthAndYear"
			columnReportMonthAndYear.Visible = False
			columnReportMonthAndYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnReportMonthAndYear)

			Dim columnReportFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportFrom.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnReportFrom.OptionsColumn.AllowEdit = False
			columnReportFrom.Name = "ReportFrom"
			columnReportFrom.FieldName = "ReportFrom"
			columnReportFrom.Width = 250
			columnReportFrom.Visible = True
			columnReportFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnReportFrom)

			Dim columnReportTo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportTo.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnReportTo.OptionsColumn.AllowEdit = False
			columnReportTo.Name = "ReportTo"
			columnReportTo.FieldName = "ReportTo"
			columnReportTo.Width = 250
			columnReportTo.Visible = True
			columnReportTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnReportTo)

			Dim columnReportLineBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportLineBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnReportLineBetrag.OptionsColumn.AllowEdit = False
			columnReportLineBetrag.Name = "ReportLineBetrag"
			columnReportLineBetrag.FieldName = "ReportLineBetrag"
			columnReportLineBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnReportLineBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnReportLineBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnReportLineBetrag.DisplayFormat.FormatString = "N2"
			columnReportLineBetrag.Width = 300
			columnReportLineBetrag.Visible = True
			columnReportLineBetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnReportLineBetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnReportLineBetrag.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnReportLineBetrag)


			m_SuppressUIEvents = True
			grdIndData.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Employment the employment grid.
		''' </summary>
		Private Sub ResetEmploymentGrid()

			gvIndData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvIndData.OptionsView.ShowIndicator = False
			gvIndData.OptionsBehavior.Editable = True
			gvIndData.OptionsView.ShowAutoFilterRow = True
			gvIndData.OptionsView.ColumnAutoWidth = True
			gvIndData.OptionsView.ShowFooter = True
			gvIndData.OptionsView.AllowHtmlDrawGroups = True

			gvIndData.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 50
			gvIndData.Columns.Add(columnIsSelected)

			Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnESNr.OptionsColumn.AllowEdit = False
			columnESNr.Name = "ESNr"
			columnESNr.FieldName = "ESNr"
			columnESNr.Width = 200
			columnESNr.Visible = True
			columnESNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnESNr)

			Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
			columnEmployeeFullName.OptionsColumn.AllowEdit = False
			columnEmployeeFullName.Name = "EmployeeFullName"
			columnEmployeeFullName.FieldName = "EmployeeFullName"
			columnEmployeeFullName.Width = 300
			columnEmployeeFullName.Visible = True
			columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnEmployeeFullName)

			Dim columnCustomer1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
			columnCustomer1.OptionsColumn.AllowEdit = False
			columnCustomer1.Name = "Company1"
			columnCustomer1.FieldName = "Company1"
			columnCustomer1.Width = 300
			columnCustomer1.Visible = True
			columnCustomer1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCustomer1)

			Dim columnES_Ab As New DevExpress.XtraGrid.Columns.GridColumn()
			columnES_Ab.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnES_Ab.OptionsColumn.AllowEdit = False
			columnES_Ab.Name = "ES_Ab"
			columnES_Ab.FieldName = "ES_Ab"
			columnES_Ab.Width = 250
			columnES_Ab.Visible = True
			columnES_Ab.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnES_Ab)

			Dim columnES_Ende As New DevExpress.XtraGrid.Columns.GridColumn()
			columnES_Ende.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnES_Ende.OptionsColumn.AllowEdit = False
			columnES_Ende.Name = "ES_Ende"
			columnES_Ende.FieldName = "ES_Ende"
			columnES_Ende.Width = 250
			columnES_Ende.Visible = True
			columnES_Ende.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnES_Ende)

			Dim columnReportLineBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportLineBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnReportLineBetrag.OptionsColumn.AllowEdit = False
			columnReportLineBetrag.Name = "ReportLineBetrag"
			columnReportLineBetrag.FieldName = "ReportLineBetrag"
			columnReportLineBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnReportLineBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnReportLineBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnReportLineBetrag.DisplayFormat.FormatString = "N2"
			columnReportLineBetrag.Width = 300
			columnReportLineBetrag.Visible = True
			columnReportLineBetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnReportLineBetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnReportLineBetrag.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnReportLineBetrag)


			m_SuppressUIEvents = True
			grdIndData.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets the reportlines grid.
		''' </summary>
		Private Sub ResetReportlineGrid()

			gvIndData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvIndData.OptionsView.ShowIndicator = False
			gvIndData.OptionsBehavior.Editable = True
			gvIndData.OptionsView.ShowAutoFilterRow = True
			gvIndData.OptionsView.ColumnAutoWidth = True
			gvIndData.OptionsView.ShowFooter = True
			gvIndData.OptionsView.AllowHtmlDrawGroups = True

			gvIndData.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Width = 50
			columnIsSelected.Visible = True
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnIsSelected)

			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.OptionsColumn.AllowEdit = False
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnCustomerNumber.Name = "CustomerNumber"
			columnCustomerNumber.FieldName = "CustomerNumber"
			columnCustomerNumber.Width = 200
			columnCustomerNumber.Visible = True
			columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCustomerNumber)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.OptionsColumn.AllowEdit = False
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany1.Name = "Company1"
			columnCompany1.FieldName = "Company1"
			columnCompany1.Width = 300
			columnCompany1.Visible = True
			columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnCompany1)

			Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStreet.OptionsColumn.AllowEdit = False
			columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnStreet.Name = "Street"
			columnStreet.FieldName = "Street"
			columnStreet.Width = 200
			columnStreet.Visible = True
			columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnStreet)

			Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcodeAndLocation.OptionsColumn.AllowEdit = False
			columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnPostcodeAndLocation.Name = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.FieldName = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.Width = 300
			columnPostcodeAndLocation.Visible = True
			columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvIndData.Columns.Add(columnPostcodeAndLocation)

			Dim columnReportLineBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReportLineBetrag.OptionsColumn.AllowEdit = False
			columnReportLineBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnReportLineBetrag.Name = "ReportLineBetrag"
			columnReportLineBetrag.FieldName = "ReportLineBetrag"
			columnReportLineBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnReportLineBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnReportLineBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnReportLineBetrag.DisplayFormat.FormatString = "n2"
			columnReportLineBetrag.Width = 150
			columnReportLineBetrag.Visible = True
			columnReportLineBetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnReportLineBetrag.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnReportLineBetrag.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnReportLineBetrag)


			m_SuppressUIEvents = True
			grdIndData.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub


		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the Bankdaten drop down data.
		''' </summary>
		Private Function LoadBankdatenDropDown(ByVal setDefault As Boolean) As Boolean
			' Load data

			Dim mandantNr = lueMandant.EditValue

			m_BankData = m_InvoiceDatabaseAccess.LoadBankData(mandantNr)

			If (m_BankData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht geladen werden."))
			End If

			lueBankdaten.Properties.DataSource = m_BankData
			lueBankdaten.Properties.ForceInitialize()

			Dim standardBank = (From b In m_BankData Where b.AsStandard = True).FirstOrDefault
			If standardBank Is Nothing Then
				standardBank = (From b In m_BankData Order By b.RecNr).FirstOrDefault
			End If

			If setDefault AndAlso standardBank IsNot Nothing Then
				lueBankdaten.EditValue = standardBank.ID
			End If

			Return m_BankData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the Debitorenart drop down data.
		''' </summary>
		Private Function LoadDebitorenartDropDown() As Boolean
			m_Debitorenart = New List(Of DebitorenAutomatedArt) From {
				New DebitorenAutomatedArt With {.Display = m_Translate.GetSafeTranslationValue("Pro Rapport"), .Value = 1},
				New DebitorenAutomatedArt With {.Display = m_Translate.GetSafeTranslationValue("Pro Einsatz"), .Value = 2},
				New DebitorenAutomatedArt With {.Display = m_Translate.GetSafeTranslationValue("Pro Kunde"), .Value = 3},
				New DebitorenAutomatedArt With {.Display = m_Translate.GetSafeTranslationValue("Pro Rapportkostenstelle"), .Value = 4},
				New DebitorenAutomatedArt With {.Display = m_Translate.GetSafeTranslationValue("Alle Rechnungen automatisch erstellen"), .Value = 5}
			 }

			lueDebitorenart.Properties.DataSource = m_Debitorenart

			lueDebitorenart.Properties.ForceInitialize()
			lueDebitorenart.EditValue = 5

			Return Not m_Debitorenart Is Nothing

		End Function

		''' <summary>
		''' Loads the reminder data.
		''' </summary>
		Private Function LoadMandantPaymentReminderData() As Boolean

			m_MahnCodeData = m_CustomerDatabaseAccess.LoadPaymentReminderCodeData()

			If (m_MahnCodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mahndaten konnten nicht geladen werden."))
			End If


			Return m_MahnCodeData IsNot Nothing
		End Function


		''' <summary>
		''' Handles change of mandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueMandant.EditValue Is Nothing Then

				If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
					ResetAfterMandantChange()
					Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

					Dim clsMandant = m_Mandant.GetSelectedMDData(lueMandant.EditValue)
					Dim logedUserData = m_Mandant.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
					Dim personalizedData = m_InitializationData.ProsonalizedData
					Dim translate = m_InitializationData.TranslationData

					m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

					ChangeMandant(m_InitializationData.MDData.MDNr)
					LoadDataAfterMandantChanged()

				End If

			End If


		End Sub

		''' <summary>
		''' Handles change of debitorenart.
		''' </summary>
		Private Sub OnlueDebitorenart_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueDebitorenart.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueDebitorenart.EditValue Is Nothing Then

				LoadGridDataForCreatingInvoices()

			End If

		End Sub


		Private Sub LoadCreatedInvoices()

			grdInvoices.DataSource = m_InvoiceData

		End Sub


		Private Function LoadGridDataForCreatingInvoices() As Boolean
			Dim result As Boolean = True

			Select Case lueDebitorenart.EditValue
				Case 0, 3
					ResetCustomerGrid()
					result = result AndAlso LoadCustomerGridDataWithFacturaCode()

				Case 1
					ResetReportGrid()
					result = result AndAlso LoadReportGridDataWithFacturaCode()

				Case 2
					ResetEmploymentGrid()
					result = result AndAlso LoadEmploymentGridDataWithFacturaCode()

				Case 4
					ResetReportCostcenterGrid()
					result = result AndAlso LoadReportCostcenterGridDataWithFacturaCode()

				Case 5
					ResetReportlineGrid()
					result = result AndAlso LoadReportlineGridDataWithFacturaCode()


				Case Else
					gvIndData.Columns.Clear()
					Return False

			End Select


			Return result

		End Function



		Private Function LoadCustomerGridDataWithFacturaCode() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadCustomerDataForSearchAutomatedInvoices(lueMandant.EditValue, Nothing)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
											Select New CustomerOverviewAutomatedInvoiceData With {.CustomerNumber = person.CustomerNumber,
																																						.BillTypeCode = person.BillTypeCode,
																																						.Company1 = person.Company1,
																																						.Street = person.Street,
																																						.Postcode = person.Postcode,
																																						.Location = person.Location,
																																						.ReportLineBetrag = person.ReportLineBetrag,
																																						.IsSelected = tgsSelectionIndividuell.EditValue
																																						}).ToList()

			Dim listDataSource As BindingList(Of CustomerOverviewAutomatedInvoiceData) = New BindingList(Of CustomerOverviewAutomatedInvoiceData)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadReportGridDataWithFacturaCode() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadReportDataForSearchAutomatedInvoices(lueMandant.EditValue, Nothing)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
											Select New ReportOverviewAutomatedInvoiceData With {.RPNr = person.RPNr,
																																					.EmployeeNumber = person.EmployeeNumber,
																																					.CustomerNumber = person.CustomerNumber,
																																					.ReportLineBetrag = person.ReportLineBetrag,
																																					.ReportMonth = person.ReportMonth,
																																					.ReportYear = person.ReportYear,
																																					.ReportFrom = person.ReportFrom,
																																					.ReportTo = person.ReportTo,
																																					.Company1 = person.Company1,
																																					.EmployeeFirstname = person.EmployeeFirstname,
																																					.EmployeeLastname = person.EmployeeLastname,
																																					.IsSelected = tgsSelectionIndividuell.EditValue
																																				 }).ToList()

			Dim listDataSource As BindingList(Of ReportOverviewAutomatedInvoiceData) = New BindingList(Of ReportOverviewAutomatedInvoiceData)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadReportCostcenterGridDataWithFacturaCode() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadReportCostcenterDataForSearchAutomatedInvoices(lueMandant.EditValue, Nothing)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapport-Kostenstellendaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
											Select New ReportOverviewAutomatedInvoiceData With {.RPNr = person.RPNr,
																																					.KstNr = person.KstNr,
																																					.KSTBez = person.KSTBez,
																																					.EmployeeNumber = person.EmployeeNumber,
																																					.CustomerNumber = person.CustomerNumber,
																																					.ReportLineBetrag = person.ReportLineBetrag,
																																					.ReportMonth = person.ReportMonth,
																																					.ReportYear = person.ReportYear,
																																					.ReportFrom = person.ReportFrom,
																																					.ReportTo = person.ReportTo,
																																					.Company1 = person.Company1,
																																					.EmployeeFirstname = person.EmployeeFirstname,
																																					.EmployeeLastname = person.EmployeeLastname,
																																					.IsSelected = tgsSelectionIndividuell.EditValue
																																				 }).ToList()

			Dim listDataSource As BindingList(Of ReportOverviewAutomatedInvoiceData) = New BindingList(Of ReportOverviewAutomatedInvoiceData)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadEmploymentGridDataWithFacturaCode() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadEmploymentDataForSearchAutomatedInvoices(lueMandant.EditValue, Nothing)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
											Select New EmploymentOverviewAutomatedInvoiceData With {.ESNr = person.ESNr,
																																							.EmployeeNumber = person.EmployeeNumber,
																																							.CustomerNumber = person.CustomerNumber,
																																							.ReportLineBetrag = person.ReportLineBetrag,
																																							.ES_Ab = person.ES_Ab,
																																							.ES_Ende = person.ES_Ende,
																																							.RPLYear = person.RPLYear,
																																							.Company1 = person.Company1,
																																							.EmployeeFirstname = person.EmployeeFirstname,
																																							.EmployeeLastname = person.EmployeeLastname,
																																							.IsSelected = tgsSelectionIndividuell.EditValue
																																						 }).ToList()

			Dim listDataSource As BindingList(Of EmploymentOverviewAutomatedInvoiceData) = New BindingList(Of EmploymentOverviewAutomatedInvoiceData)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadReportlineGridDataWithFacturaCode() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadReportLineDataForSearchAutomatedInvoices(lueMandant.EditValue, Nothing)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
											Select New ReportLineOverviewAutomatedInvoiceData With {.CustomerNumber = person.CustomerNumber,
																																							.Company1 = person.Company1,
																																							.ReportLineBetrag = person.ReportLineBetrag,
																																							.Street = person.Street,
																																							.Postcode = person.Postcode,
																																							.Location = person.Location,
																																							.BillTypeCode = person.BillTypeCode,
																																							.InvoiceOption = person.InvoiceOption,
																																							.IsSelected = tgsSelectionIndividuell.EditValue
																																						 }).ToList()

			Dim listDataSource As BindingList(Of ReportLineOverviewAutomatedInvoiceData) = New BindingList(Of ReportLineOverviewAutomatedInvoiceData)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function GetSelectedLineForAutoSelectionData() As BindingList(Of ReportLineOverviewAutomatedInvoiceData)

			Dim result As BindingList(Of ReportLineOverviewAutomatedInvoiceData)

			gvIndData.FocusedColumn = gvIndData.VisibleColumns(1)
			grdIndData.RefreshDataSource()
			Dim printList As BindingList(Of ReportLineOverviewAutomatedInvoiceData) = grdIndData.DataSource
			Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

			result = New BindingList(Of ReportLineOverviewAutomatedInvoiceData)

			For Each receiver In sentList
				result.Add(receiver)
			Next


			Return result

		End Function

		Private Function GetSelectedLineForReportSelectionData() As BindingList(Of ReportOverviewAutomatedInvoiceData)

			Select Case lueDebitorenart.EditValue
				Case 1
					Dim result As BindingList(Of ReportOverviewAutomatedInvoiceData)

					gvIndData.FocusedColumn = gvIndData.VisibleColumns(1)
					grdIndData.RefreshDataSource()
					Dim printList As BindingList(Of ReportOverviewAutomatedInvoiceData) = grdIndData.DataSource
					Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

					result = New BindingList(Of ReportOverviewAutomatedInvoiceData)

					For Each receiver In sentList
						result.Add(receiver)
					Next
					Return result


				Case Else

					Return Nothing

			End Select

		End Function

		Private Function GetSelectedLineForEmploymentSelectionData() As BindingList(Of EmploymentOverviewAutomatedInvoiceData)

			Select Case lueDebitorenart.EditValue
				Case 2
					Dim result As BindingList(Of EmploymentOverviewAutomatedInvoiceData)

					gvIndData.FocusedColumn = gvIndData.VisibleColumns(1)
					grdIndData.RefreshDataSource()
					Dim printList As BindingList(Of EmploymentOverviewAutomatedInvoiceData) = grdIndData.DataSource
					Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

					result = New BindingList(Of EmploymentOverviewAutomatedInvoiceData)

					For Each receiver In sentList
						result.Add(receiver)
					Next
					Return result


				Case Else

					Return Nothing

			End Select

		End Function

		Private Function GetSelectedLineForCustomerSelectionData() As BindingList(Of CustomerOverviewAutomatedInvoiceData)

			Select Case lueDebitorenart.EditValue
				Case 3
					Dim result As BindingList(Of CustomerOverviewAutomatedInvoiceData)

					gvIndData.FocusedColumn = gvIndData.VisibleColumns(1)
					grdIndData.RefreshDataSource()
					Dim printList As BindingList(Of CustomerOverviewAutomatedInvoiceData) = grdIndData.DataSource
					Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

					result = New BindingList(Of CustomerOverviewAutomatedInvoiceData)

					For Each receiver In sentList
						result.Add(receiver)
					Next
					Return result


				Case Else

					Return Nothing

			End Select

		End Function

		Private Function GetSelectedLineForReportCostCenterSelectionData() As BindingList(Of ReportOverviewAutomatedInvoiceData)

			Select Case lueDebitorenart.EditValue
				Case 4
					Dim result As BindingList(Of ReportOverviewAutomatedInvoiceData)

					gvIndData.FocusedColumn = gvIndData.VisibleColumns(1)
					grdIndData.RefreshDataSource()
					Dim printList As BindingList(Of ReportOverviewAutomatedInvoiceData) = grdIndData.DataSource
					Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

					result = New BindingList(Of ReportOverviewAutomatedInvoiceData)

					For Each receiver In sentList
						result.Add(receiver)
					Next
					Return result


				Case Else

					Return Nothing

			End Select

		End Function

		Private Function GetSelectedInvoiceData() As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			Dim result As New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			gvInvoices.FocusedColumn = gvInvoices.VisibleColumns(1)
			grdInvoices.RefreshDataSource()
			Dim printList As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = grdInvoices.DataSource
			If Not printList Is Nothing Then
				Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

				result = New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

				For Each receiver In sentList
					result.Add(receiver)
				Next
			End If

			Return result

		End Function


		Private Function CreateIndividuellSelection() As CreationResult
			Dim msg As String = String.Empty
			Dim result As CreationResult = New CreationResult With {.Value = True}
			Dim success As Boolean = True

			m_InvoiceData = New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			Select Case lueDebitorenart.EditValue
				Case 1
					Dim data = GetSelectedLineForReportSelectionData()

					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildInvoiceForIndividuellSelectionWithReportNumberData(itm.RPNr, itm.CustomerNumber)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next

				Case 2
					Dim data = GetSelectedLineForEmploymentSelectionData()

					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildInvoiceForIndividuellSelectionWithEmploymentNumberData(itm.CustomerNumber, itm.ESNr, itm.RPLYear) ' Year(itm.ES_Ab))
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next


				Case 3
					Dim data = GetSelectedLineForCustomerSelectionData()

					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildInvoiceForIndividuellSelectionWithCustomerNumberData(itm.CustomerNumber, 0)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next


				Case 4
					Dim data = GetSelectedLineForReportCostCenterSelectionData()

					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildInvoiceForIndividuellSelectionWithCostcenterNumberData(itm.CustomerNumber, itm.RPNr, itm.KstNr, itm.ReportYear)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next


				Case Else
					gvIndData.Columns.Clear()
					If Not success Then Return New CreationResult With {.Value = False, .Message = "not defined!!!"}

			End Select


			If success Then
				grdInvoices.DataSource = m_InvoiceData
				grdInvoices.RefreshDataSource()
			End If


			SplashScreenManager.CloseForm(False)


			Return result

		End Function

		Private Function CreateAutomatedSelection() As CreationResult
			Dim msg As String = String.Empty
			Dim result As CreationResult = New CreationResult With {.Value = True}
			Dim success As Boolean = True


			m_InvoiceData = New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)
			Dim listData = GetSelectedLineForAutoSelectionData()

			If listData.Count = 0 Then
				msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")

				Return New CreationResult With {.Value = False, .Message = msg}
			End If

			For Each itm In listData
				m_Logger.LogDebug(String.Format("Kundennummer: {0} | Fakturacode: {1}", itm.CustomerNumber, itm.BillTypeCode.ToUpper))
				Select Case True
					Case itm.BillTypeCode.ToUpper = "E"
						success = success AndAlso CreateEmploymentInvoice(itm.CustomerNumber, False, False)

					Case itm.BillTypeCode.ToUpper = "E_M", itm.BillTypeCode.ToUpper = "E_M2"
						success = success AndAlso CreateEmploymentInvoice(itm.CustomerNumber, True, itm.BillTypeCode.ToUpper = "E_M")

					Case itm.BillTypeCode.ToUpper = "K"
						success = success AndAlso CreateCustomerInvoice(itm.CustomerNumber, False, False)

					Case itm.BillTypeCode.ToUpper = "K_M", itm.BillTypeCode.ToUpper = "K_M2"
						success = success AndAlso CreateCustomerInvoice(itm.CustomerNumber, True, itm.BillTypeCode.ToUpper = "K_M")

					Case itm.BillTypeCode.ToUpper = "R"
						success = success AndAlso CreateReportInvoice(itm.CustomerNumber, False)

					Case itm.BillTypeCode.ToUpper = "M"
						success = success AndAlso CreateReportInvoice(itm.CustomerNumber, True)


					Case itm.BillTypeCode.ToUpper = "S"
						success = success AndAlso CreateCostcenterInvoice(itm.CustomerNumber, True, False, False)

					Case itm.BillTypeCode.ToUpper = "S_M"
						success = success AndAlso CreateCostcenterInvoice(itm.CustomerNumber, False, False, True)

					Case itm.BillTypeCode.ToUpper = "S_M2"
						success = success AndAlso CreateCostcenterInvoice(itm.CustomerNumber, False, True, False)

					Case itm.BillTypeCode.ToUpper = "S_R"
						success = success AndAlso CreateCostcenterInvoice(itm.CustomerNumber, True, False, True)

					Case itm.BillTypeCode.ToUpper = "S_A"
						success = success AndAlso CreateCostcenterInvoice(itm.CustomerNumber, False, False, False)



					Case itm.BillTypeCode.ToUpper = "W_E"
						success = success AndAlso CreateWeeklyInvoice(itm.CustomerNumber, True, False, False)
					Case itm.BillTypeCode.ToUpper = "W_K"
						success = success AndAlso CreateWeeklyInvoice(itm.CustomerNumber, False, False, False)
					Case itm.BillTypeCode.ToUpper = "W_R"
						success = success AndAlso CreateWeeklyInvoice(itm.CustomerNumber, False, True, False)

					Case itm.BillTypeCode.ToUpper = "W_S"
						success = success AndAlso CreateWeeklyCostcenterInvoice(itm.CustomerNumber, False, False)
					Case itm.BillTypeCode.ToUpper = "SWR"
						success = success AndAlso CreateWeeklyCostcenterInvoice(itm.CustomerNumber, True, False)
					Case itm.BillTypeCode.ToUpper = "SWR2"
						success = success AndAlso CreateWeeklyCostcenterInvoice(itm.CustomerNumber, True, True)

					Case itm.BillTypeCode.ToUpper = "WSA"
						success = success AndAlso CreateWeeklyCostcenterAddressInvoice(itm.CustomerNumber, False)
					Case itm.BillTypeCode.ToUpper = "RWSA"
						success = success AndAlso CreateWeeklyCostcenterAddressInvoice(itm.CustomerNumber, True)


				End Select

				If success Then
					grdInvoices.DataSource = m_InvoiceData
					grdInvoices.RefreshDataSource()
				End If

			Next

			SplashScreenManager.CloseForm(False)


			Return result

		End Function

		Private Function CreateReportInvoice(ByVal customerNumber As Integer, ByVal closedMonth As Boolean) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadReportDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, closedMonth)
			If data Is Nothing Then
				m_Logger.LogError("Rapportdaten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildNewReportInvoiceData(itm, customerNumber)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function

		Private Function CreateEmploymentInvoice(ByVal customerNumber As Integer, ByVal groupByMonth As Boolean, ByVal closedMonth As Boolean) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadEmploymentDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, groupByMonth, closedMonth)
			If data Is Nothing Then
				m_Logger.LogError("Einsatzdaten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildNewEmploymentInvoiceData(itm, customerNumber, closedMonth)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function

		Private Function CreateCustomerInvoice(ByVal customerNumber As Integer, ByVal groupByMonth As Boolean, ByVal closedMonth As Boolean) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadCustomerDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, groupByMonth, closedMonth)
			If data Is Nothing Then
				m_Logger.LogError("Rappoortdaten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildNewCustomerInvoiceData(itm, customerNumber, closedMonth)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function

		Private Function CreateCostcenterInvoice(ByVal customerNumber As Integer, ByVal groupByReportNumber As Boolean, ByVal groupByMonth As Boolean, ByVal closedMonth As Boolean) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadCostcenterDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, groupByReportNumber, groupByMonth, closedMonth)
			If data Is Nothing Then
				m_Logger.LogError("Kostenstellen-Daten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildNewCostCenterInvoiceData(itm, customerNumber, closedMonth)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function

		Private Function CreateWeeklyInvoice(ByVal customerNumber As Integer, ByVal groupByEmploymentNumber As Boolean?, ByVal groupByReportNumber As Boolean?, ByVal closedMonth As Boolean) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadWeeklyDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, groupByEmploymentNumber, groupByReportNumber, closedMonth)
			If data Is Nothing Then
				m_Logger.LogError("Einsatz-Daten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildWeeklyInvoiceData(itm, customerNumber, closedMonth)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function

		Private Function CreateWeeklyCostcenterInvoice(ByVal customerNumber As Integer, ByVal groupByReportNumber As Boolean, ByVal closedMonth As Boolean) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadWeeklyCostcenterDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, groupByReportNumber, closedMonth)
			If data Is Nothing Then
				m_Logger.LogError("Kostenstellen-Daten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildWeeklyCostCenterInvoiceData(itm, customerNumber, closedMonth)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function


		Private Function CreateWeeklyCostcenterAddressInvoice(ByVal customerNumber As Integer, ByVal groupByReportNumber As Boolean?) As Boolean
			Dim result As Boolean = True

			Dim data = m_InvoiceDatabaseAccess.LoadWeeklyCostcenterAddressDataForCreatingAutomatedInvoices(lueMandant.EditValue, customerNumber, groupByReportNumber)
			If data Is Nothing Then
				m_Logger.LogError("Kostenstellen-Daten konnten nicht gefunden werden.")

				Return False

			End If

			For Each itm In data
				Dim createdInvoiceData = m_AutomatedUtility.BuildWeeklyCostCenterAddressInvoiceData(itm, customerNumber)
				If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
				result = result AndAlso Not createdInvoiceData Is Nothing

				If Not result Then Return False
			Next

			Return Not data Is Nothing

		End Function


#End Region



		Private Sub OntgsSelectionIndividuell_Toggled(sender As Object, e As EventArgs) Handles tgsSelectionIndividuell.Toggled
			SelDeSelectIndividuellItems(tgsSelectionIndividuell.EditValue)
		End Sub

		Private Sub SelDeSelectIndividuellItems(ByVal selectItem As Boolean)
			Dim data = grdIndData.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.IsSelected = selectItem
				Next
			End If

			gvIndData.RefreshData()

		End Sub

		Private Sub OntgsSelectionInvoices_Toggled(sender As Object, e As EventArgs) Handles tgsSelectionInvoices.Toggled
			SelDeSelectInvoiceItems(tgsSelectionInvoices.EditValue)
		End Sub

		Private Sub SelDeSelectInvoiceItems(ByVal selectItem As Boolean)
			Dim data As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = grdInvoices.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.IsSelected = selectItem
				Next
			End If

			gvInvoices.RefreshData()

		End Sub

		Private Sub OnbbiCreate_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCreate.ItemClick

			ResetInvoiceGrid()
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try

				m_AutomatedUtility = New CreateAutomatedInvoices(m_InitializationData)
				m_AutomatedUtility.m_InvoiceDatabaseAccess = m_InvoiceDatabaseAccess
				m_AutomatedUtility.m_CustomerDatabaseAccess = m_CustomerDatabaseAccess
				m_AutomatedUtility.m_MandantNumber = lueMandant.EditValue
				m_AutomatedUtility.m_BankData = m_BankData
				m_AutomatedUtility.SelectedBankNumber = lueBankdaten.EditValue
				m_AutomatedUtility.m_Debitorenart = m_Debitorenart
				m_AutomatedUtility.IsSelected = If(tgsSelectionInvoices.EditValue, True, False)
				m_AutomatedUtility.InitialData()

				Dim result As New CreationResult With {.Message = "is starting...", .Value = False}

				Select Case True
					Case lueDebitorenart.EditValue < 5
						result = CreateIndividuellSelection()
						If result.Value Then
							LoadGridDataForCreatingInvoices()
						End If

					Case lueDebitorenart.EditValue = 5
						result = CreateAutomatedSelection()
						If result.Value Then
							lueDebitorenart.EditValue = 1
							LoadGridDataForCreatingInvoices()
						End If

					Case Else
						Return

				End Select

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			Finally
				SplashScreenManager.CloseForm(False)

			End Try


		End Sub

		Private Sub bbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
			Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl
			popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
		End Sub

		Private Sub CreatePrintPopupMenu()

			Dim bshowMnu As Boolean = True
			Dim popupMenu As New DevExpress.XtraBars.PopupMenu
			Dim liMnu As New List(Of String) From {"Ausgewählte Rechnungen Drucken#PrintRE", _
																						 "Druckmaske öffnen#PrintDetailRE"}
			Try

				bbiPrint.Manager = Me.BarManager1
				Dim allowedEmployeWOS As Boolean = m_mandant.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year)
				BarManager1.ForceInitialize()

				Me.bbiPrint.ActAsDropDown = False
				Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
				Me.bbiPrint.DropDownEnabled = True
				Me.bbiPrint.DropDownControl = popupMenu
				Me.bbiPrint.Enabled = True

				For i As Integer = 0 To liMnu.Count - 1
					Dim myValue As String() = liMnu(i).Split(CChar("#"))

					bshowMnu = myValue(0).ToString <> String.Empty
					If bshowMnu Then
						popupMenu.Manager = BarManager1

						Dim itm As New DevExpress.XtraBars.BarButtonItem
						itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
						itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

						If i = 0 Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
						AddHandler itm.ItemClick, AddressOf GetMenuItem
					End If

				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

			Try
				Select Case e.Item.Name.ToUpper
					Case "PrintRE".ToUpper
						PrintInvoice()

					Case "PrintDetailRE".ToUpper
						LoadPrintInvoiceInDetail()

					Case Else
						Return

				End Select

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		Private Sub PrintInvoice()
			Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			Dim invoiceNumbers = New List(Of Integer)()

			Dim invoiceData = GetSelectedInvoiceData()
			For Each itm In invoiceData
				invoiceNumbers.Add(itm.ReNr)
			Next
			If invoiceNumbers.Count = 0 Then
				Dim msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(msg)
				Return
			End If
			Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = Me.Handle,
																																												.InvoiceNumbers = invoiceNumbers,
																																												.PrintInvoiceAsCopy = False,
																																												.ShowAsDesign = ShowDesign,
																																												.WOSSendValueEnum = SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData.WOSValue.PrintWithoutSending}
			Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
			printUtil.PrintData = _setting
			Dim result = printUtil.PrintInvoice()

			printUtil.Dispose()

		End Sub

		Private Sub LoadPrintInvoiceInDetail()
			Dim invoiceNumbers = New List(Of Integer?)

			Dim invoiceData = GetSelectedInvoiceData()
			For Each itm In invoiceData
				invoiceNumbers.Add(itm.ReNr)
			Next
			If invoiceNumbers.Count = 0 Then
				Dim msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(msg)
				Return
			End If

			Dim frm = New SP.Invoice.PrintUtility.frmInvoicePrint(m_InitializationData)
			Dim preselectionSetting As New SP.Invoice.PrintUtility.PreselectionData With {.MDNr = lueMandant.EditValue, .InvoiceNumbers = invoiceNumbers}
			frm.PreselectionData = preselectionSetting
			frm.PreselectData()

			frm.Show()
			frm.BringToFront()

		End Sub

		Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
			Dim success As Boolean = True
			Dim msg As String = String.Empty
			Dim invoiceNumbers = New List(Of Integer)()

			Dim invoiceData = GetSelectedInvoiceData()
			For Each itm In invoiceData
				invoiceNumbers.Add(itm.ReNr)
			Next
			If invoiceNumbers.Count = 0 Then
				msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(msg)
				Return
			End If
			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie die erstellten Rechnungen wirklich löschen?"),
																m_Translate.GetSafeTranslationValue("Rechnung löschen")) = False) Then
				Return
			End If

			success = m_InvoiceDatabaseAccess.DeleteStraightCreatedInvoices(lueMandant.EditValue, invoiceNumbers.ToArray)
			If Not success Then
				msg = m_Translate.GetSafeTranslationValue("Rechnungen konnten nicht gelöscht werden!")
				m_Logger.LogError(msg)

				m_UtilityUI.ShowErrorDialog(msg)

				Return
			Else
				msg = m_Translate.GetSafeTranslationValue("Rechnungen wurden erfolgreich gelöscht.")

				m_UtilityUI.ShowInfoDialog(msg)

				ResetInvoiceGrid()
				LoadGridDataForCreatingInvoices()

			End If

		End Sub

		Private Sub OnbbiExport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
			Dim ShowDesign As Boolean = False
			Dim invoiceNumbers = New List(Of Integer)()

			Try

				Dim invoiceData = GetSelectedInvoiceData()
				For Each itm In invoiceData
					invoiceNumbers.Add(itm.ReNr)
				Next
				If invoiceNumbers.Count = 0 Then
					Dim msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
					m_UtilityUI.ShowInfoDialog(msg)
					Return
				End If
				SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
				SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
				SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

				Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = Me.Handle,
					.InvoiceNumbers = invoiceNumbers,
					.PrintInvoiceAsCopy = False,
					.ShowAsDesign = ShowDesign,
					.ExportPrintInFiles = True,
					.WOSSendValueEnum = SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData.WOSValue.PrintWithoutSending
				}

				Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
				printUtil.PrintData = _setting
				Dim result = printUtil.PrintInvoice()
				printUtil.Dispose()

				Dim exportPath As String = m_InitializationData.UserData.spTempInvoicePath
				If result.Printresult AndAlso result.JobResultInvoiceData.Count > 0 Then

					Dim newFilename As String = Path.Combine(m_InitializationData.UserData.spTempInvoicePath, String.Format("{0} - {1}.ex", Path.GetRandomFileName, result.JobResultInvoiceData(0).CustomerNumber))
					newFilename = Path.ChangeExtension(newFilename, ".pdf")

					Dim fileList As New List(Of String)
					For Each itm In result.JobResultInvoiceData
						fileList.Add(itm.ExportedFileName)
					Next

					If result.JobResultInvoiceData.Count > 1 Then
						Dim pdfUtility As New SP.Infrastructure.PDFUtilities.Utilities
						Dim success = pdfUtility.MergePdfFiles(fileList.ToArray, newFilename)
					Else
						newFilename = result.JobResultInvoiceData(0).ExportedFileName
					End If

					SplashScreenManager.CloseForm(False)
					Dim msg As String = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich exportiert und zusammengestellt.")
					m_UtilityUI.ShowInfoDialog(msg)

					Process.Start("explorer.exe", "/select," & newFilename)
					'Process.Start(newFilename)

				Else
					Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.") & "<br>{0}<br>{1}",
																					 exportPath, result.PrintresultMessage)
					SplashScreenManager.CloseForm(False)
					m_UtilityUI.ShowErrorDialog(strMsg)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			Finally
				SplashScreenManager.CloseForm(False)

			End Try

		End Sub

		Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvInvoices.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then

					Dim viewData = CType(dataRow, SP.DatabaseAccess.Invoice.DataObjects.Invoice)

					Select Case column.Name.ToLower
						Case "rname1"
							If viewData.KdNr > 0 Then OpenSelectedCustomer(viewData.KdNr)


						Case Else
							If viewData.ReNr > 0 Then
								OpenSelectedInvoice(viewData.ReNr)

							End If

					End Select

				End If

			End If

		End Sub

		Sub OngvIndData_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvIndData.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then

					Select Case lueDebitorenart.EditValue
						Case 1
							Dim viewData = CType(dataRow, ReportOverviewAutomatedInvoiceData)
							If viewData.RPNr > 0 Then OpenSelectedReport(viewData.RPNr)

						Case 2
							Dim viewData = CType(dataRow, EmploymentOverviewAutomatedInvoiceData)
							If viewData.ESNr > 0 Then OpenSelectedES(viewData.ESNr)

						Case 3
							Dim viewData = CType(dataRow, CustomerOverviewAutomatedInvoiceData)
							If viewData.CustomerNumber > 0 Then OpenSelectedCustomer(viewData.CustomerNumber)

						Case 4
							Dim viewData = CType(dataRow, ReportOverviewAutomatedInvoiceData)
							If viewData.RPNr > 0 Then OpenSelectedReport(viewData.RPNr)

						Case Else
							Dim viewData = CType(dataRow, ReportLineOverviewAutomatedInvoiceData)
							If viewData.CustomerNumber > 0 Then OpenSelectedCustomer(viewData.CustomerNumber)

					End Select


				End If

			End If

		End Sub

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

		Private Sub toolTipController1_GetActiveObjectInfo(ByVal sender As Object, ByVal e As DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs) Handles ToolTipController1.GetActiveObjectInfo
			Dim msg = String.Empty

			If e.SelectedControl IsNot grdInvoices Then
				Return
			End If
			Dim info As ToolTipControlInfo = Nothing
			Dim view As GridView = TryCast(grdInvoices.GetViewAt(e.ControlMousePosition), GridView)
			If view Is Nothing Then
				Return
			End If
			Dim hitInfo As GridHitInfo = view.CalcHitInfo(e.ControlMousePosition)
			If hitInfo.HitTest = GridHitTest.Footer AndAlso hitInfo.Column IsNot Nothing AndAlso hitInfo.Column.SummaryItem.SummaryType <> DevExpress.Data.SummaryItemType.None Then
				Dim o As Object = hitInfo.HitTest.ToString() & hitInfo.Column.ToString()
				If hitInfo.Column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count Then
					msg = "Anzahl"
				Else
					msg = "Totalbetrag"
				End If
				Dim s As String = String.Format(msg, hitInfo.Column.ToString())

				info = New ToolTipControlInfo(o, s)
			End If

				If info IsNot Nothing Then e.Info = info


		End Sub


#Region "Helpers"

		''' <summary>
		''' Changes the mandant nr.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		Private Sub ChangeMandant(ByVal mdNr As Integer?)

			If mdNr Is Nothing Then mdNr = m_InitializationData.MDData.MDNr
			Dim conStr = m_Mandant.GetSelectedMDData(mdNr).MDDbConn

			If Not m_CurrentConnectionString = conStr Then

				m_CurrentConnectionString = conStr

				m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			End If

		End Sub

		Private Function LoadDataAfterMandantChanged() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadBankdatenDropDown(True)
			LoadGridDataForCreatingInvoices()


			Return success

		End Function

		Private Sub OpenSelectedEmployee(ByVal employeenumber As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, employeenumber)
			hub.Publish(openMng)

		End Sub

		Private Sub OpenSelectedCustomer(ByVal customernumber As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, customernumber)
			hub.Publish(openMng)

		End Sub

		''' <summary>
		''' Handles focus change of es row.
		''' </summary>
		Private Sub OpenSelectedES(ByVal esnumber As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, esnumber)
			hub.Publish(openMng)

		End Sub

		''' <summary>
		''' Handles focus change of report row.
		''' </summary>
		Private Sub OpenSelectedReport(ByVal reportNumber As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, reportNumber)
			hub.Publish(openMng)

		End Sub

		''' <summary>
		''' Handles focus change of invoice row.
		''' </summary>
		Private Sub OpenSelectedInvoice(ByVal invoiceNumber As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, invoiceNumber)
			hub.Publish(openMng)

		End Sub


		Private Class CreationResult

			Public Property Value As Boolean?
			Public Property Message As String

		End Class


#End Region


	End Class

End Namespace
