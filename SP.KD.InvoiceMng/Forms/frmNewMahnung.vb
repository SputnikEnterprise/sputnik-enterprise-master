
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
Imports DevExpress.XtraGrid
Imports DevExpress.XtraEditors


Namespace UI


	Public Class frmNewMahnung


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

		Private m_AutomatedUtility As CreateInvoiceDunning


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

		Private m_WhatTODO As List(Of WhatTODO)
		Private m_DunningLevel As List(Of DunningLevel)
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
		Private m_Create3mahnasuntilnotpaid As Boolean

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
		Private ReadOnly Property MwStAnsatz As Decimal
			Get

				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

			End Get

		End Property

		''' <summary>
		''' Gets the reference number to 10 setting.
		''' </summary>
		Private ReadOnly Property Create3mahnasuntilnotpaid As Boolean
			Get

				Dim mdNumber = lueMandant.EditValue
				If mdNumber Is Nothing Then mdNumber = m_InitializationData.MDData.MDNr

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim createDunning As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/create3mahnasuntilnotpaid", FORM_XML_MAIN_KEY)), False)

				Return createDunning.HasValue AndAlso createDunning

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
				Dim grdView = TryCast(grdDunning.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

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
			AddHandler gvDunning.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

			Dim conStr = m_InitializationData.MDData.MDDbConn
			m_CurrentConnectionString = conStr

			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

			' Translate controls.
			TranslateControls()

			Reset()

			m_SuppressUIEvents = False

			AddHandler gvDunning.RowCellClick, AddressOf Ongv_RowCellClick
			AddHandler daeMahndate.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler daeZEUntilDatum.ButtonClick, AddressOf OnDropDownButtonClick

			AddHandler lueDunningDate.ButtonClick, AddressOf OnDunningDateDropDownButtonClick

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

			lueWhatTODO.EditValue = 0
			LoadGridDataForCreatingDunning()

			Return success

		End Function

		Public Function LoadData() As Boolean
			Dim success As Boolean = True


			success = success AndAlso LoadMandantDropDownData()
			lueMandant.EditValue = m_InitializationData.MDData.MDNr

			success = success AndAlso LoadWhatTODODropDown(True)
			success = success AndAlso LoadDunningLevelDropDown()
			success = success AndAlso LoadMandantPaymentReminderData()


			PreselectData()

			Return success

		End Function


#End Region


#Region "Private Methods"


		''' <summary>
		''' Resets the control.
		''' </summary>
		Private Sub Reset()

			m_InvoiceData = Nothing
			m_WhatTODO = Nothing
			m_DunningLevel = Nothing

			daeZEUntilDatum.EditValue = Now.Date
			daeMahndate.EditValue = Now.Date
			lueDunningDate.EditValue = Now.Date

			pnlDeleteMahn.Top = pnlCreateMahn.Top
			pnlDeleteMahn.BorderStyle = BorderStyles.NoBorder
			pnlCreateMahn.BorderStyle = BorderStyles.NoBorder
			lbLstlInfo.Visible = False

			'  Reset drop downs and lists
			ResetMandantDropDown()
			ResetWhatTODODropDown()
			ResetDunningLevelDropDown()
			ResetDunningDateDropDown()

			ResetIndGrid()
			ResetDunningGrid()

		End Sub

		Private Sub ResetAfterMandantChanged()

			m_InvoiceData = Nothing
			m_WhatTODO = Nothing
			m_DunningLevel = Nothing

			daeZEUntilDatum.EditValue = Now.Date
			daeMahndate.EditValue = Now.Date
			lueDunningDate.EditValue = Now.Date

			pnlDeleteMahn.Top = pnlCreateMahn.Top
			pnlDeleteMahn.BorderStyle = BorderStyles.NoBorder
			pnlCreateMahn.BorderStyle = BorderStyles.NoBorder
			lbLstlInfo.Visible = False

			'  Reset drop downs and lists
			ResetDunningDateDropDown()

			ResetIndGrid()
			ResetDunningGrid()

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
			lblMahnstufe.Text = m_Translate.GetSafeTranslationValue(lblMahnstufe.Text)
			lblMahnarten.Text = m_Translate.GetSafeTranslationValue(lblMahnarten.Text)

			lblZahlungberuecksichtigtbis.Text = m_Translate.GetSafeTranslationValue(lblZahlungberuecksichtigtbis.Text)
			lblMahndatum.Text = m_Translate.GetSafeTranslationValue(lblMahndatum.Text)

			tgsSelectionIndividuell.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelectionIndividuell.Properties.OffText)
			tgsSelectionIndividuell.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelectionIndividuell.Properties.OnText)

			lblErstellteRechnungen.Text = m_Translate.GetSafeTranslationValue(lblErstellteRechnungen.Text)
			lblIndividuelleDaten.Text = m_Translate.GetSafeTranslationValue(lblIndividuelleDaten.Text)

			lbLstlInfo.Text = m_Translate.GetSafeTranslationValue(lbLstlInfo.Text)

			bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue(bsiPrintinfo.Caption)
			bbiCreate.Caption = m_Translate.GetSafeTranslationValue(bbiCreate.Caption)
			bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)

		End Sub


#End Region


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
		''' Resets the what to do drop down.
		''' </summary>
		Private Sub ResetWhatTODODropDown()

			lueWhatTODO.Properties.DisplayMember = "Label"
			lueWhatTODO.Properties.ValueMember = "Value"

			lueWhatTODO.Properties.Columns.Clear()
			'lueWhatTODO.Properties.Columns.Add(New LookUpColumnInfo("Value", 0))
			lueWhatTODO.Properties.Columns.Add(New LookUpColumnInfo("Display", 0))

			lueWhatTODO.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the dunning level drop down.
		''' </summary>
		Private Sub ResetDunningLevelDropDown()

			lueDunningLevel.Properties.DisplayMember = "Label"
			lueDunningLevel.Properties.ValueMember = "Value"

			lueDunningLevel.Properties.Columns.Clear()
			'lueDunningLevel.Properties.Columns.Add(New LookUpColumnInfo("Value", 0))
			lueDunningLevel.Properties.Columns.Add(New LookUpColumnInfo("Display", 0))

			lueDunningLevel.EditValue = Nothing

		End Sub


		''' <summary>
		''' Resets the dunning date drop down.
		''' </summary>
		Private Sub ResetDunningDateDropDown()

			lueDunningDate.Properties.DisplayMember = "DunningDate"
			lueDunningDate.Properties.ValueMember = "DunningDate"

			lueDunningDate.Properties.Columns.Clear()
			lueDunningDate.Properties.Columns.Add(New LookUpColumnInfo("DunningDate", "", 0, DevExpress.Utils.FormatType.DateTime, "d", True, HorzAlignment.Default))
			lueDunningDate.Properties.Columns.Add(New LookUpColumnInfo("DunningCount", 0))

			lueDunningLevel.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the invoice grid.
		''' </summary>
		Private Sub ResetIndGrid()

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

			Dim columnReNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnReNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnReNr.OptionsColumn.AllowEdit = False
			columnReNr.Name = "ReNr"
			columnReNr.FieldName = "ReNr"
			columnReNr.Width = 250
			columnReNr.Visible = True
			columnReNr.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			'columnCustomerNumber.to = "Anzahl Datensätze: {0:f2}"
			gvIndData.Columns.Add(columnReNr)

			Dim columnKdNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKdNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnKdNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKdNr.OptionsColumn.AllowEdit = False
			columnKdNr.Name = "KdNr"
			columnKdNr.FieldName = "KdNr"
			columnKdNr.Width = 250
			columnKdNr.Visible = False
			columnKdNr.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count
			'columnKdNrr.to = "Anzahl Datensätze: {0:f2}"
			gvIndData.Columns.Add(columnKdNr)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany1.OptionsColumn.AllowEdit = False
			columnCompany1.Name = "RName1"
			columnCompany1.FieldName = "RName1"
			columnCompany1.Width = 300
			columnCompany1.Visible = True
			gvIndData.Columns.Add(columnCompany1)

			Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnStreet.OptionsColumn.AllowEdit = False
			columnStreet.Name = "RStrasse"
			columnStreet.FieldName = "RStrasse"
			columnStreet.Width = 300
			columnStreet.Visible = False
			gvIndData.Columns.Add(columnStreet)

			Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnPostcodeAndLocation.OptionsColumn.AllowEdit = False
			columnPostcodeAndLocation.Name = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.FieldName = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.Width = 300
			columnPostcodeAndLocation.Visible = True
			gvIndData.Columns.Add(columnPostcodeAndLocation)

			Dim columninvoiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columninvoiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columninvoiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columninvoiceDate.OptionsColumn.AllowEdit = False
			columninvoiceDate.Name = "FakDat"
			columninvoiceDate.FieldName = "FakDat"
			columninvoiceDate.Width = 300
			columninvoiceDate.Visible = True
			gvIndData.Columns.Add(columninvoiceDate)

			Dim columnFaellig As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFaellig.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFaellig.Caption = m_Translate.GetSafeTranslationValue("Fällig am")
			columnFaellig.OptionsColumn.AllowEdit = False
			columnFaellig.Name = "Faellig"
			columnFaellig.FieldName = "Faellig"
			columnFaellig.Width = 300
			columnFaellig.Visible = True
			gvIndData.Columns.Add(columnFaellig)

			Dim columnMA0 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMA0.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMA0.Caption = m_Translate.GetSafeTranslationValue("Kontoauszug")
			columnMA0.OptionsColumn.AllowEdit = False
			columnMA0.Name = "MA0"
			columnMA0.FieldName = "MA0"
			columnMA0.Width = 300
			columnMA0.Visible = If(lueWhatTODO.EditValue = 4 AndAlso lueDunningLevel.EditValue = 0, True, False)
			gvIndData.Columns.Add(columnMA0)

			Dim columnMA1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMA1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMA1.Caption = m_Translate.GetSafeTranslationValue("1. Mahnung")
			columnMA1.OptionsColumn.AllowEdit = False
			columnMA1.Name = "MA1"
			columnMA1.FieldName = "MA1"
			columnMA1.Width = 300
			columnMA1.Visible = If(lueWhatTODO.EditValue = 4 AndAlso lueDunningLevel.EditValue = 1, True, False)
			gvIndData.Columns.Add(columnMA1)

			Dim columnMA2 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMA2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMA2.Caption = m_Translate.GetSafeTranslationValue("2. Mahnung")
			columnMA2.OptionsColumn.AllowEdit = False
			columnMA2.Name = "MA2"
			columnMA2.FieldName = "MA2"
			columnMA2.Width = 300
			columnMA2.Visible = If(lueWhatTODO.EditValue = 4 AndAlso lueDunningLevel.EditValue = 2, True, False)
			gvIndData.Columns.Add(columnMA2)

			Dim columnMA3 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMA3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMA3.Caption = m_Translate.GetSafeTranslationValue("Inkasso")
			columnMA3.OptionsColumn.AllowEdit = False
			columnMA3.Name = "MA3"
			columnMA3.FieldName = "MA3"
			columnMA3.Width = 300
			columnMA3.Visible = If(lueWhatTODO.EditValue = 4 AndAlso lueDunningLevel.EditValue = 3, True, False)
			gvIndData.Columns.Add(columnMA3)


			Dim columnBetragOhne As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragOhne.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBetragOhne.Caption = m_Translate.GetSafeTranslationValue("Betrag ohne MwSt.")
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
			gvIndData.Columns.Add(columnBetragOhne)

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
			gvIndData.Columns.Add(columnBetragEx)

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
			columnMWST1.Visible = False
			columnMWST1.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnMWST1.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnMWST1)

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
			gvIndData.Columns.Add(columnBetragInk)

			Dim columnBezahlt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezahlt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezahlt.Caption = m_Translate.GetSafeTranslationValue("Bezahlt")
			columnBezahlt.OptionsColumn.AllowEdit = False
			columnBezahlt.Name = "Bezahlt"
			columnBezahlt.FieldName = "Bezahlt"
			columnBezahlt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBezahlt.AppearanceHeader.Options.UseTextOptions = True
			columnBezahlt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBezahlt.DisplayFormat.FormatString = "n2"
			columnBezahlt.Width = 500
			columnBezahlt.Visible = True
			columnBezahlt.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			columnBezahlt.SummaryItem.DisplayFormat = "{0:n2}"
			gvIndData.Columns.Add(columnBezahlt)



			gvIndData.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways
			' Create and setup the first summary item.
			Dim grpMA0 As GridGroupSummaryItem = New GridGroupSummaryItem()
			grpMA0.FieldName = "MA0"
			grpMA0.SummaryType = DevExpress.Data.SummaryItemType.None
			'grpMA0.DisplayFormat = m_Translate.GetSafeTranslationValue("Anzahl") & " = {0:F0}"
			gvIndData.GroupSummary.Add(grpMA0)


			' Create and setup the first summary item.
			Dim item As GridGroupSummaryItem = New GridGroupSummaryItem()
			item.FieldName = "RName1"
			item.SummaryType = DevExpress.Data.SummaryItemType.Count
			item.DisplayFormat = m_Translate.GetSafeTranslationValue("Anzahl {0:F0}")
			gvIndData.GroupSummary.Add(item)




			' Create and setup the second summary item.
			Dim item1 As GridGroupSummaryItem = New GridGroupSummaryItem()
			item1.FieldName = "BetragInk"
			item1.SummaryType = DevExpress.Data.SummaryItemType.Sum
			item1.DisplayFormat = m_Translate.GetSafeTranslationValue("Total {0:c2}")
			item1.ShowInGroupColumnFooter = gvIndData.Columns("BetragInk")
			gvIndData.GroupSummary.Add(item1)
			gvIndData.GroupFormat = "{0}: {1}"


			m_SuppressUIEvents = True
			grdIndData.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Resets the dunning grid.
		''' </summary>
		Private Sub ResetDunningGrid()

			gvDunning.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvDunning.OptionsView.ShowIndicator = False
			gvDunning.OptionsBehavior.Editable = True
			gvDunning.OptionsView.ShowAutoFilterRow = True
			gvDunning.OptionsView.ColumnAutoWidth = True
			gvDunning.OptionsView.ShowFooter = True
			gvDunning.OptionsView.AllowHtmlDrawGroups = True

			gvDunning.Columns.Clear()

			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 50
			gvDunning.Columns.Add(columnIsSelected)

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
			gvDunning.Columns.Add(columnCustomerNumber)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany1.OptionsColumn.AllowEdit = False
			columnCompany1.Name = "RName1"
			columnCompany1.FieldName = "RName1"
			columnCompany1.Width = 300
			columnCompany1.Visible = True
			gvDunning.Columns.Add(columnCompany1)

			Dim columnStreet As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStreet.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnStreet.Caption = m_Translate.GetSafeTranslationValue("Strasse")
			columnCustomerNumber.OptionsColumn.AllowEdit = False
			columnStreet.Name = "RStrasse"
			columnStreet.FieldName = "RStrasse"
			columnStreet.Width = 300
			columnStreet.Visible = True
			gvDunning.Columns.Add(columnStreet)

			Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnPostcodeAndLocation.OptionsColumn.AllowEdit = False
			columnPostcodeAndLocation.Name = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.FieldName = "CustomerPostcodeLocation"
			columnPostcodeAndLocation.Width = 300
			columnPostcodeAndLocation.Visible = True
			gvDunning.Columns.Add(columnPostcodeAndLocation)

			Dim columninvoiceDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columninvoiceDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columninvoiceDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columninvoiceDate.OptionsColumn.AllowEdit = False
			columninvoiceDate.Name = "FakDat"
			columninvoiceDate.FieldName = "FakDat"
			columninvoiceDate.Width = 300
			columninvoiceDate.Visible = True
			gvDunning.Columns.Add(columninvoiceDate)


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
			gvDunning.Columns.Add(columnBetragOhne)

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
			gvDunning.Columns.Add(columnBetragEx)

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
			gvDunning.Columns.Add(columnMWST1)

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
			gvDunning.Columns.Add(columnBetragInk)


			m_SuppressUIEvents = True
			grdDunning.DataSource = Nothing
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
		''' Loads the what to do drop down data.
		''' </summary>
		Private Function LoadWhatTODODropDown(ByVal setDefault As Boolean) As Boolean
			' Load data

			m_WhatTODO = New List(Of WhatTODO) From {
				New WhatTODO With {.Display = m_Translate.GetSafeTranslationValue("Kontoauszug erstellen"), .Value = 0},
				New WhatTODO With {.Display = m_Translate.GetSafeTranslationValue("1. Mahnung erstellen"), .Value = 1},
				New WhatTODO With {.Display = m_Translate.GetSafeTranslationValue("2. Mahnung erstellen"), .Value = 2},
				New WhatTODO With {.Display = m_Translate.GetSafeTranslationValue("3. Mahnung erstellen"), .Value = 3},
				New WhatTODO With {.Display = m_Translate.GetSafeTranslationValue("Eine Mahnstufe zurücksetzen"), .Value = 4}
			 }

			lueWhatTODO.Properties.DataSource = m_WhatTODO

			lueWhatTODO.Properties.ForceInitialize()
			lueWhatTODO.EditValue = 0

			Return Not m_WhatTODO Is Nothing

		End Function

		''' <summary>
		''' Loads the dunning level drop down data.
		''' </summary>
		Private Function LoadDunningLevelDropDown() As Boolean

			m_DunningLevel = New List(Of DunningLevel) From {
				New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("Kontoauszug"), .Value = 0},
				New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("1. Mahnstufe"), .Value = 1},
				New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("2. Mahnstufe"), .Value = 2},
				New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("3. Mahnstufe"), .Value = 3}
			 }

			lueDunningLevel.Properties.DataSource = m_DunningLevel

			lueDunningLevel.Properties.ForceInitialize()

			Return Not m_DunningLevel Is Nothing

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
					ResetAfterMandantChanged()
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
		''' Handles change of lueWhatTODO.
		''' </summary>
		Private Sub OnlueWhatTODO_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueWhatTODO.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueWhatTODO.EditValue Is Nothing Then
				LoadGridDataForCreatingDunning()
			End If

		End Sub


		''' <summary>
		''' Loads the Bankdaten drop down data.
		''' </summary>
		Private Function LoadDunningdateDropDown() As Boolean
			' Load data
			If lueDunningLevel.EditValue Is Nothing Then Return True
			Dim mandantNr = lueMandant.EditValue

			Dim data = m_InvoiceDatabaseAccess.LoadDunningDateData(mandantNr, lueDunningLevel.EditValue)

			If (data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mahndatums konnten nicht geladen werden."))
			End If

			lueDunningDate.Properties.DataSource = data
			lueDunningDate.Properties.ForceInitialize()

			If Not data Is Nothing AndAlso data.Count > 0 Then lueDunningDate.EditValue = data(0).DunningDate


			Return data IsNot Nothing
		End Function

		''' <summary>
		''' Handles change of lueDunningLevel.
		''' </summary>
		Private Sub OnlueDunningLevel_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueDunningLevel.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueDunningLevel.EditValue Is Nothing Then
				ResetIndGrid()
				'ResetDunningDateDropDown()
				LoadDunningdateDropDown()
				LoadInvoiceGridDataForDeletingDunningLevel(lueDunningDate.EditValue)
			End If

		End Sub

		''' <summary>
		''' Handles change of lueDunningLevel.
		''' </summary>
		Private Sub OnlueDunningDate_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueDunningDate.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueDunningLevel.EditValue Is Nothing AndAlso Not lueDunningDate.EditValue Is Nothing Then
				'				ResetIndGrid()
				LoadInvoiceGridDataForDeletingDunningLevel(lueDunningDate.EditValue)
			End If

		End Sub

		Private Sub daeZEUntilDatum_EditValueChanged(sender As Object, e As EventArgs) Handles daeZEUntilDatum.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			LoadGridDataForCreatingDunning()

		End Sub

		Private Function LoadGridDataForCreatingDunning() As Boolean
			Dim result As Boolean = True

			lbLstlInfo.Visible = False
			pnlCreateMahn.Visible = False
			pnlDeleteMahn.Visible = False
			bbiCreate.Caption = m_Translate.GetSafeTranslationValue("Erstellen")
			bbiDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
			ResetIndGrid()

			Select Case lueWhatTODO.EditValue
				Case 0
					result = result AndAlso LoadInvoiceGridDataForKontoauszug()
					pnlCreateMahn.Visible = True

				Case 1
					result = result AndAlso LoadInvoiceGridDataForFirstDunning()
					pnlCreateMahn.Visible = True

				Case 2
					result = result AndAlso LoadInvoiceGridDataForSecondDunning()
					pnlCreateMahn.Visible = True

				Case 3
					result = result AndAlso LoadInvoiceGridDataForThirdDunning()
					pnlCreateMahn.Visible = True


				Case 4
					LoadInvoiceGridDataForDeletingDunningLevel(lueDunningDate.EditValue)
					pnlDeleteMahn.Visible = True
					lbLstlInfo.Visible = True
					bbiCreate.Caption = m_Translate.GetSafeTranslationValue("Zurücksetzen")


				Case Else
					Return False

			End Select


			Return result

		End Function



		Private Function LoadInvoiceGridDataForKontoauszug() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadInvoiceDataForCreatingKontoauszug(lueMandant.EditValue, daeZEUntilDatum.EditValue)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
							Select New SP.DatabaseAccess.Invoice.DataObjects.Invoice With {.ReNr = person.ReNr,
																							 .KdNr = person.KdNr,
																							 .RName1 = person.RName1,
																							 .Mahncode = person.Mahncode,
																							 .RStrasse = person.RStrasse,
																							 .RPLZ = person.RPLZ,
																							 .ROrt = person.ROrt,
																							 .ReMail = person.ReMail,
																							 .SendAsZip = person.SendAsZip,
																							 .BetragOhne = person.BetragOhne,
																							 .BetragEx = person.BetragEx,
																							 .BetragInk = person.BetragInk,
																							 .MWSTProz = person.MWSTProz,
																							 .MWST1 = person.MWST1,
																							 .Bezahlt = person.Bezahlt,
																							 .FakDat = person.FakDat,
																							 .Faellig = person.Faellig,
																							 .MA0 = person.MA0,
																							 .MA1 = person.MA1,
																							 .MA2 = person.MA2,
																							 .MA3 = person.MA3,
																							 .ZEBis0 = person.ZEBis0,
																							 .ZEBis1 = person.ZEBis1,
																							 .ZEBis2 = person.ZEBis2,
																							 .ZEBis3 = person.ZEBis3,
																							 .IsSelected = tgsSelectionIndividuell.EditValue
																							}).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = New BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadInvoiceGridDataForFirstDunning() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadInvoiceDataForCreatingFirstDunning(lueMandant.EditValue, daeZEUntilDatum.EditValue)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
							Select New SP.DatabaseAccess.Invoice.DataObjects.Invoice With {.ReNr = person.ReNr,
																							 .KdNr = person.KdNr,
																							 .RName1 = person.RName1,
																							 .Mahncode = person.Mahncode,
																							 .RStrasse = person.RStrasse,
																							 .RPLZ = person.RPLZ,
																							 .ROrt = person.ROrt,
																							 .ReMail = person.ReMail,
																							 .SendAsZip = person.SendAsZip,
																							 .BetragOhne = person.BetragOhne,
																							 .BetragEx = person.BetragEx,
																							 .BetragInk = person.BetragInk,
																							 .MWSTProz = person.MWSTProz,
																							 .MWST1 = person.MWST1,
																							 .Bezahlt = person.Bezahlt,
																							 .FakDat = person.FakDat,
																							 .Faellig = person.Faellig,
																							 .MA0 = person.MA0,
																							 .MA1 = person.MA1,
																							 .MA2 = person.MA2,
																							 .MA3 = person.MA3,
																							 .ZEBis0 = person.ZEBis0,
																							 .ZEBis1 = person.ZEBis1,
																							 .ZEBis2 = person.ZEBis2,
																							 .ZEBis3 = person.ZEBis3,
																							 .IsSelected = tgsSelectionIndividuell.EditValue
																							}).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = New BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			For Each p In gridData
				Dim data = (From b In m_MahnCodeData Where b.GetField = p.Mahncode).FirstOrDefault
				If DateAdd("d", CInt(data.Reminder2), p.ZEBis0) < daeZEUntilDatum.EditValue Then
					listDataSource.Add(p)
				End If

			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadInvoiceGridDataForSecondDunning() As Boolean
			Dim result As Boolean = True

			Dim listOfData = m_InvoiceDatabaseAccess.LoadInvoiceDataForCreatingSecondDunning(lueMandant.EditValue, daeZEUntilDatum.EditValue)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
							Select New SP.DatabaseAccess.Invoice.DataObjects.Invoice With {.ReNr = person.ReNr,
																							 .KdNr = person.KdNr,
																							 .RName1 = person.RName1,
																							 .Mahncode = person.Mahncode,
																							 .RStrasse = person.RStrasse,
																							 .RPLZ = person.RPLZ,
																							 .ROrt = person.ROrt,
																							 .ReMail = person.ReMail,
																							 .SendAsZip = person.SendAsZip,
																							 .BetragOhne = person.BetragOhne,
																							 .BetragEx = person.BetragEx,
																							 .BetragInk = person.BetragInk,
																							 .MWSTProz = person.MWSTProz,
																							 .MWST1 = person.MWST1,
																							 .Bezahlt = person.Bezahlt,
																							 .FakDat = person.FakDat,
																							 .Faellig = person.Faellig,
																							 .MA0 = person.MA0,
																							 .MA1 = person.MA1,
																							 .MA2 = person.MA2,
																							 .MA3 = person.MA3,
																							 .ZEBis0 = person.ZEBis0,
																							 .ZEBis1 = person.ZEBis1,
																							 .ZEBis2 = person.ZEBis2,
																							 .ZEBis3 = person.ZEBis3,
																							 .IsSelected = tgsSelectionIndividuell.EditValue
																							}).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = New BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			For Each p In gridData
				Dim data = (From b In m_MahnCodeData Where b.GetField = p.Mahncode).FirstOrDefault
				If DateAdd("d", CInt(data.Reminder3), p.ZEBis1) < daeZEUntilDatum.EditValue Then
					listDataSource.Add(p)
				End If
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadInvoiceGridDataForThirdDunning() As Boolean
			Dim result As Boolean = True
			Dim createAgin = m_Create3mahnasuntilnotpaid

			Dim listOfData = m_InvoiceDatabaseAccess.LoadInvoiceDataForCreatingThirdDunning(lueMandant.EditValue, daeZEUntilDatum.EditValue, createAgin)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
							Select New SP.DatabaseAccess.Invoice.DataObjects.Invoice With {.ReNr = person.ReNr,
																							 .KdNr = person.KdNr,
																							 .RName1 = person.RName1,
																							 .Mahncode = person.Mahncode,
																							 .RStrasse = person.RStrasse,
																							 .RPLZ = person.RPLZ,
																							 .ROrt = person.ROrt,
																							 .ReMail = person.ReMail,
																							 .SendAsZip = person.SendAsZip,
																							 .BetragOhne = person.BetragOhne,
																							 .BetragEx = person.BetragEx,
																							 .BetragInk = person.BetragInk,
																							 .MWSTProz = person.MWSTProz,
																							 .MWST1 = person.MWST1,
																							 .Bezahlt = person.Bezahlt,
																							 .FakDat = person.FakDat,
																							 .Faellig = person.Faellig,
																							 .MA0 = person.MA0,
																							 .MA1 = person.MA1,
																							 .MA2 = person.MA2,
																							 .MA3 = person.MA3,
																							 .ZEBis0 = person.ZEBis0,
																							 .ZEBis1 = person.ZEBis1,
																							 .ZEBis2 = person.ZEBis2,
																							 .ZEBis3 = person.ZEBis3,
																							 .IsSelected = tgsSelectionIndividuell.EditValue
																							}).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = New BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			For Each p In gridData
				Dim data = (From b In m_MahnCodeData Where b.GetField = p.Mahncode).FirstOrDefault
				If DateAdd("d", CInt(data.Reminder4), p.ZEBis2) < daeZEUntilDatum.EditValue Then
					listDataSource.Add(p)
				End If
			Next

			grdIndData.DataSource = listDataSource


			Return Not listOfData Is Nothing

		End Function

		Private Function LoadInvoiceGridDataForDeletingDunningLevel(ByVal dunningDate As Date?) As Boolean
			Dim result As Boolean = True

			If lueDunningLevel.EditValue Is Nothing Then Return result
			Dim listOfData = m_InvoiceDatabaseAccess.LoadDunningDatesData(lueMandant.EditValue, lueDunningLevel.EditValue, dunningDate)

			If (listOfData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In listOfData
							Select New SP.DatabaseAccess.Invoice.DataObjects.Invoice With {.ReNr = person.ReNr,
																							 .KdNr = person.KdNr,
																							 .RName1 = person.RName1,
																							 .Mahncode = person.Mahncode,
																							 .RStrasse = person.RStrasse,
																							 .RPLZ = person.RPLZ,
																							 .ROrt = person.ROrt,
																							 .ReMail = person.ReMail,
																							 .SendAsZip = person.SendAsZip,
																							 .BetragOhne = person.BetragOhne,
																							 .BetragEx = person.BetragEx,
																							 .BetragInk = person.BetragInk,
																							 .MWST1 = person.MWST1,
																							 .Bezahlt = person.Bezahlt,
																							 .FakDat = person.FakDat,
																							 .Faellig = person.Faellig,
																							 .MA0 = person.MA0,
																							 .MA1 = person.MA1,
																							 .MA2 = person.MA2,
																							 .MA3 = person.MA3,
																							 .IsSelected = tgsSelectionIndividuell.EditValue
																							}).ToList()

			Dim listDataSource As BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = New BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			For Each p In gridData
				listDataSource.Add(p)
			Next

			grdIndData.DataSource = listDataSource
			bbiCreate.Enabled = listDataSource.Count > 0
			bbiDelete.Enabled = listDataSource.Count > 0
			bbiPrint.Enabled = listDataSource.Count > 0

			Return Not listOfData Is Nothing

		End Function


		Private Function GetSelectedInvoiceData() As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			Dim result As New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			gvIndData.FocusedColumn = gvIndData.VisibleColumns(1)
			grdIndData.RefreshDataSource()
			Dim printList As BindingList(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = grdIndData.DataSource
			If Not printList Is Nothing Then
				Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

				result = New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

				For Each receiver In sentList
					result.Add(receiver)
				Next
			End If

			Return result

		End Function

		Private Function GetSelectedDunningData() As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			Dim result As New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			gvDunning.FocusedColumn = gvDunning.VisibleColumns(1)
			grdDunning.RefreshDataSource()
			Dim printList As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = grdDunning.DataSource
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
			Dim invoiceNumbers = New List(Of Integer)()

			m_InvoiceData = New List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice)

			Dim data = GetSelectedInvoiceData()
			For Each itm In data
				invoiceNumbers.Add(itm.ReNr)
			Next
			If invoiceNumbers.Count = 0 Then
				msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(msg)
				Return New CreationResult With {.Value = False}
			End If

			Select Case lueWhatTODO.EditValue
				Case 0
					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildKontoauszugData(itm, daeZEUntilDatum.EditValue, daeMahndate.EditValue)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}
					Next

				Case 1
					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildFirstDunningData(itm)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next

				Case 2
					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildSecondDunningData(itm)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next

				Case 3
					For Each itm In data
						Dim createdInvoiceData = m_AutomatedUtility.BuildThirdDunningData(itm)
						If Not createdInvoiceData Is Nothing Then m_InvoiceData.Add(createdInvoiceData)
						success = success AndAlso Not createdInvoiceData Is Nothing

						If Not success Then Return New CreationResult With {.Value = False}

					Next


				Case 4
					For Each itm In data
						success = m_InvoiceDatabaseAccess.DeleteStraightCreatedDunning(lueMandant.EditValue, New Integer() {itm.ReNr}, lueDunningLevel.EditValue)
						If Not success Then Return New CreationResult With {.Value = False}

					Next


				Case Else
					gvIndData.Columns.Clear()
					If Not success Then Return New CreationResult With {.Value = False, .Message = "not defined!!!"}

			End Select

			success = success AndAlso LoadGridDataForCreatingDunning()
			If success AndAlso lueWhatTODO.EditValue <> 4 Then
				grdDunning.DataSource = m_InvoiceData
				grdDunning.RefreshDataSource()
				bbiDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
			End If


			SplashScreenManager.CloseForm(False)


			Return result

		End Function


		Private Sub OntgsSelectionIndividuell_Toggled(sender As Object, e As EventArgs) Handles tgsSelectionIndividuell.Toggled
			SelDeSelectIndividuellItems(tgsSelectionIndividuell.EditValue)
		End Sub

		Private Sub OntgsSelectionInvoices_Toggled(sender As Object, e As EventArgs) Handles tgsSelectionInvoices.Toggled
			SelDeSelectInvoiceItems(tgsSelectionInvoices.EditValue)
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

		Private Sub SelDeSelectInvoiceItems(ByVal selectItem As Boolean)
			Dim data As List(Of SP.DatabaseAccess.Invoice.DataObjects.Invoice) = grdDunning.DataSource

			If Not data Is Nothing Then
				For Each item In data
					item.IsSelected = selectItem
				Next
			End If

			gvDunning.RefreshData()

		End Sub

		Private Sub OnbbiCreate_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCreate.ItemClick

			ResetDunningGrid()
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Try

				m_AutomatedUtility = New CreateInvoiceDunning(m_InitializationData)
				m_AutomatedUtility.m_InvoiceDatabaseAccess = m_InvoiceDatabaseAccess
				m_AutomatedUtility.m_CustomerDatabaseAccess = m_CustomerDatabaseAccess
				m_AutomatedUtility.MandantNumber = lueMandant.EditValue
				m_AutomatedUtility.DunningLevel = lueDunningLevel.EditValue
				m_AutomatedUtility.ZEUntil = daeZEUntilDatum.EditValue
				m_AutomatedUtility.DunningDate = daeMahndate.EditValue
				m_AutomatedUtility.DunningData = m_MahnCodeData
				m_AutomatedUtility.IsSelected = If(tgsSelectionInvoices.EditValue, True, False)

				m_AutomatedUtility.InitialData()

				Dim result As New CreationResult With {.Message = "is starting...", .Value = False}
				result = CreateIndividuellSelection()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			Finally
				SplashScreenManager.CloseForm(False)

			End Try


		End Sub

		Private Sub bbiPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

			Dim frm = New SP.Invoice.PrintUtility.frmDunningPrint(m_InitializationData)
			Dim preselectionSetting As New SP.Invoice.PrintUtility.PreselectionDunningData With {.MDNr = m_InitializationData.MDData.MDNr, .DunningLevel = lueWhatTODO.EditValue, .DunningDate = daeMahndate.EditValue}
			frm.PreselectionData = preselectionSetting
			frm.LoadData()

			frm.Show()
			frm.BringToFront()

		End Sub

		Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
			Dim success As Boolean = True
			Dim msg As String = String.Empty
			Dim invoiceNumbers = New List(Of Integer)()

			Dim invoiceData = GetSelectedDunningData()
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

			success = m_InvoiceDatabaseAccess.DeleteStraightCreatedDunning(lueMandant.EditValue, invoiceNumbers.ToArray, lueWhatTODO.EditValue)
			If Not success Then
				msg = m_Translate.GetSafeTranslationValue("Mahndaten konnten nicht zurück gesetzt werden!")
				m_Logger.LogError(msg)

				m_UtilityUI.ShowErrorDialog(msg)

				Return
			Else
				msg = m_Translate.GetSafeTranslationValue("Mahndaten wurden erfolgreich zurück gesetzt.")

				m_UtilityUI.ShowInfoDialog(msg)

				ResetDunningGrid()
				LoadGridDataForCreatingDunning()

			End If

		End Sub

		Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvDunning.GetRow(e.RowHandle)
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

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

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
		''' Handles drop down button clicks for luedunningDate.
		''' </summary>
		Private Sub OnDunningDateDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

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
				LoadInvoiceGridDataForDeletingDunningLevel(Nothing)
			End If

		End Sub


#Region "Helpers"


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
