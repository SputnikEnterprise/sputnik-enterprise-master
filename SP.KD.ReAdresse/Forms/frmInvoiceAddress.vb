
Imports System.Reflection.Assembly
Imports System.ComponentModel

Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.UI

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.KD.ReAdresse.Settings
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable
Imports System.Text.RegularExpressions
Imports DevExpress.Utils
Imports SP.Infrastructure

Namespace UI

	Public Delegate Sub InvoiceAddressDataSavedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal invoiceAddressRecordNumber As Integer)
	Public Delegate Sub InvoiceAddressDataDeletedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal invoiceAddressRecordNumber As Integer)

	Public Delegate Sub InvoiceKSTDataSavedHandler(ByVal sender As Object, ByVal customerNumber As Integer)
	Public Delegate Sub InvoiceKSTDataDeletedHandler(ByVal sender As Object, ByVal customerNumber As Integer)

	Public Class frmInvoiceAddress


		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"


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
		''' Check edit for active symbol.
		''' </summary>
		Private m_CheckEditActive As RepositoryItemCheckEdit

		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_DataAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Protected m_SettingsManager As ISettingsManager

		''' <summary>
		''' Contains the customer number of the loaded customer.
		''' </summary>
		Private m_CustomerNumber As Integer

		''' <summary>
		''' The current invoice address record number.
		''' </summary>
		Private m_CurrentDetailInvoiceAddressRecordNumber As Integer?

		''' <summary>
		''' The current KST record number
		''' </summary>
		Private m_CurrentDetailKSTlRecordNumber As Integer?

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_Utility As Utility

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		Private m_md As Mandant


#End Region

#Region "Events"

		Public Event InvoiceAddressDataSaved As InvoiceAddressDataSavedHandler
		Public Event InvoiceAddressDataDeleted As InvoiceAddressDataDeletedHandler

		Public Event InvoiceKSTDataSaved As InvoiceKSTDataSavedHandler
		Public Event InvoiceKSTDataDeleted As InvoiceKSTDataDeletedHandler

#End Region

#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()


			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				' Mandantendaten

				m_md = New Mandant
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


			lueCountry.Properties.ShowHeader = False
			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 10

			luePostcode.Properties.ShowHeader = False
			luePostcode.Properties.ShowFooter = False
			luePostcode.Properties.DropDownRows = 10

			luePaymentConditions.Properties.ShowHeader = False
			luePaymentConditions.Properties.ShowFooter = False
			luePaymentConditions.Properties.DropDownRows = 10

			luePaymentReminderCode.Properties.ShowHeader = False
			luePaymentReminderCode.Properties.ShowFooter = False
			luePaymentReminderCode.Properties.DropDownRows = 10

			lueEmploymentPostcode.Properties.ShowHeader = False
			lueEmploymentPostcode.Properties.ShowFooter = False
			lueEmploymentPostcode.Properties.DropDownRows = 10

			lueBKPostcode.Properties.ShowHeader = False
			lueBKPostcode.Properties.ShowFooter = False
			lueBKPostcode.Properties.DropDownRows = 10

			gvExistingInvoiceAddressses.OptionsView.ShowIndicator = False
			gvAssignedKSTs.OptionsView.ShowIndicator = False

			m_DataAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			' Important symbol.
			m_CheckEditActive = CType(gridExistingInvoiceAddresses.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditActive.PictureChecked = My.Resources.Checked
			m_CheckEditActive.PictureUnchecked = Nothing
			m_CheckEditActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePaymentConditions.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePaymentReminderCode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueKSTInvoiceAddressNumber.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueEmploymentPostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueBKPostcode.ButtonClick, AddressOf OnDropDown_ButtonClick

			Reset()
		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected invoice address view data.
		''' </summary>
		''' <returns>The selected invoice address or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedInvoiceAddressViewData As InvoiceAddressViewData
			Get
				Dim grdView = TryCast(gridExistingInvoiceAddresses.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim invoiceAddress = CType(grdView.GetRow(selectedRows(0)), InvoiceAddressViewData)
						Return invoiceAddress
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the selected KST view data.
		''' </summary>
		''' <value>The selected KST view data or nothing if none is selected.</value>
		Private ReadOnly Property SelectedKSTViewData As KSTViewData
			Get
				Dim grdView = TryCast(gridAssignedKSTs.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim kstViewData = CType(grdView.GetRow(selectedRows(0)), KSTViewData)
						Return kstViewData
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region

#Region "Public Methods"


		''' <summary>
		''' Loads customer invoice addresses
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="invoiceAddressNumber">The invoice address record number to select.</param>
		''' <returns>Boolean flag indicating success</returns>
		Public Function LoadCustomerInvoiceAddresses(ByVal customerNumber As Integer, ByVal invoiceAddressNumber? As Integer) As Boolean

			If Not m_IsInitialDataLoaded Then
				LoadDropDownData()
				m_IsInitialDataLoaded = True
			End If
			Return LoadCustomerData(customerNumber, invoiceAddressNumber)

		End Function

#End Region

#Region "Private Methods"


		''' <summary>
		'''  trannslate controls
		''' </summary>
		''' <remarks></remarks>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.btnDeleteInvoiceAddress.Text = m_Translate.GetSafeTranslationValue(Me.btnDeleteInvoiceAddress.Text)
			Me.btnNewInvoiceAddress.Text = m_Translate.GetSafeTranslationValue(Me.btnNewInvoiceAddress.Text)
			Me.btnSaveInvoiceAddressData.Text = m_Translate.GetSafeTranslationValue(Me.btnSaveInvoiceAddressData.Text)

			Me.btnDeleteKST.Text = m_Translate.GetSafeTranslationValue(Me.btnDeleteKST.Text)
			Me.btnNewKST.Text = m_Translate.GetSafeTranslationValue(Me.btnNewKST.Text)
			Me.btnSaveKST.Text = m_Translate.GetSafeTranslationValue(Me.btnSaveKST.Text)

			Me.grpAddressData.Text = m_Translate.GetSafeTranslationValue(Me.grpAddressData.Text)
			Me.lblfirma.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma.Text)
			Me.lblfirma2.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma2.Text)
			Me.lblfirma3.Text = m_Translate.GetSafeTranslationValue(Me.lblfirma3.Text)
			Me.lblabteilung.Text = m_Translate.GetSafeTranslationValue(Me.lblabteilung.Text)
			Me.lblzhd.Text = m_Translate.GetSafeTranslationValue(Me.lblzhd.Text)
			Me.lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
			Me.lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)
			Me.lblland.Text = m_Translate.GetSafeTranslationValue(Me.lblland.Text)
			Me.lblplz.Text = m_Translate.GetSafeTranslationValue(Me.lblplz.Text)
			Me.lblort.Text = m_Translate.GetSafeTranslationValue(Me.lblort.Text)
			lblEMailAdresse.Text = m_Translate.GetSafeTranslationValue(lblEMailAdresse.Text)

			Me.lblzahlungskondition.Text = m_Translate.GetSafeTranslationValue(Me.lblzahlungskondition.Text)
			Me.lblmahncode.Text = m_Translate.GetSafeTranslationValue(Me.lblmahncode.Text)

			Me.grpKSTData.Text = m_Translate.GetSafeTranslationValue(Me.grpKSTData.Text)
			Me.lblkstbezeichnung.Text = m_Translate.GetSafeTranslationValue(Me.lblkstbezeichnung.Text)
			Me.lblkstAdresse.Text = m_Translate.GetSafeTranslationValue(Me.lblkstAdresse.Text)
			Me.lblkstAnstellungplz.Text = m_Translate.GetSafeTranslationValue(Me.lblkstAnstellungplz.Text)
			Me.lblkstbkplz.Text = m_Translate.GetSafeTranslationValue(Me.lblkstbkplz.Text)
			Me.lblkst1info.Text = m_Translate.GetSafeTranslationValue(Me.lblkst1info.Text)
			Me.lblkst2info.Text = m_Translate.GetSafeTranslationValue(Me.lblkst2info.Text)
			Me.tgsActiveInvoiceAddress.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsActiveInvoiceAddress.Properties.OnText)
			Me.tgsActiveInvoiceAddress.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsActiveInvoiceAddress.Properties.OffText)

			Me.tgsOneInvoicePerMail.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsOneInvoicePerMail.Properties.OnText)
			Me.tgsOneInvoicePerMail.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsOneInvoicePerMail.Properties.OffText)

			Dim sTip_OneInvoice As DevExpress.Utils.SuperToolTip = New SuperToolTip() With {.AllowHtmlText = DefaultBoolean.True}
			Dim ttTitleAsCopyItem1 As ToolTipTitleItem = New ToolTipTitleItem() With {.Text = m_Translate.GetSafeTranslationValue("Versand per Mail")}
			ttTitleAsCopyItem1.ImageOptions.Image = My.Resources.info_16x16 '.Images("info_16x16.png")

			Dim ttAsCopyItem1 As ToolTipItem = New ToolTipItem() With {.Text = m_Translate.GetSafeTranslationValue("Diese Eistellung ist für <b>alle Rechnungsadressen</b> gültig.")}
			Dim ttAsCopySeparatorItem1 As ToolTipSeparatorItem = New ToolTipSeparatorItem()
			sTip_OneInvoice.Items.Add(ttTitleAsCopyItem1)
			sTip_OneInvoice.Items.Add(ttAsCopySeparatorItem1)
			sTip_OneInvoice.Items.Add(ttAsCopyItem1)
			lblMailVersand.SuperTip = sTip_OneInvoice
			tgsOneInvoicePerMail.SuperTip = sTip_OneInvoice

			lblMailVersand.Text = m_Translate.GetSafeTranslationValue(lblMailVersand.Text)
			'lblMailVersand.ToolTip = m_Translate.GetSafeTranslationValue(lblMailVersand.ToolTip)
			Me.tgsSendAsZIP.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSendAsZIP.Properties.OnText)
			Me.tgsSendAsZIP.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSendAsZIP.Properties.OffText)

		End Sub


		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		Private Sub LoadDropDownData()
			LoadCountryDropDownData()
			LoadPostcodeDropDownData()
			LoadPaymetConditionDropDownData()
			LoadPaymetReminderDropDownData()
		End Sub

		''' <summary>
		''' Loads the country drop down data.
		''' </summary>
		Private Sub LoadCountryDropDownData()
			Dim result As Boolean = True
			Dim countryData As IEnumerable(Of CVLBaseTableViewData) = Nothing

			Try
				Dim baseTable = New SPSBaseTables(m_InitializationData)
				baseTable.BaseTableName = "Country"
				countryData = baseTable.PerformCVLBaseTablelistWebserviceCall()

				If (countryData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
				End If

				lueCountry.Properties.DataSource = countryData
				lueCountry.Properties.ForceInitialize()

			Catch ex As Exception

			End Try

			'Dim countryData = m_CommonDatabaseAccess.LoadCountryData()
			'Return Not countryData Is Nothing
		End Sub

		''' <summary>
		''' Loads the postcode drop downdata.
		''' </summary>
		Private Sub LoadPostcodeDropDownData()
			Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

			If (postcodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Postleizahldaten konnten nicht geladen werden.")
			End If

			luePostcode.Properties.DataSource = postcodeData
			luePostcode.Properties.ForceInitialize()

			lueEmploymentPostcode.Properties.DataSource = postcodeData
			lueEmploymentPostcode.Properties.ForceInitialize()

			lueBKPostcode.Properties.DataSource = postcodeData
			lueBKPostcode.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the payment condition drop downdata.
		''' </summary>
		Private Sub LoadPaymetConditionDropDownData()
			Dim paymentConditionData = m_DataAccess.LoadPaymentConditionData()

			If (paymentConditionData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlungskonditionen konnten nicht geladen werden."))
			End If

			luePaymentConditions.Properties.DataSource = paymentConditionData
			luePaymentConditions.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the payment reminder drop downdata.
		''' </summary>
		Private Sub LoadPaymetReminderDropDownData()
			Dim paymentReminderCodeData = m_DataAccess.LoadPaymentReminderCodeData()

			If (paymentReminderCodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Mahnungscodes konnten nicht geladen werden.")
			End If

			Dim listOfPaymentReminderCodeViewData = Nothing
			If Not paymentReminderCodeData Is Nothing Then

				listOfPaymentReminderCodeViewData = New List(Of PaymentReminderViewData)
				For Each paymentReminderCode In paymentReminderCodeData

					Dim paymentReminderCodeViewData = New PaymentReminderViewData With {
							.PaymentReminderCode = paymentReminderCode.GetField,
							.PaymentReminderDataString = String.Format("{0} ({1})-{2}-{3}-{4}",
																							paymentReminderCode.GetField,
																							paymentReminderCode.Reminder1,
																							paymentReminderCode.Reminder2,
																							paymentReminderCode.Reminder3,
																							paymentReminderCode.Reminder4)
							}

					listOfPaymentReminderCodeViewData.Add(paymentReminderCodeViewData)

				Next

			End If

			luePaymentReminderCode.Properties.DataSource = listOfPaymentReminderCodeViewData
			luePaymentReminderCode.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads customer data.
		''' </summary>
		''' <param name="customerNumber">The customer id.</param>
		''' <param name="invoiceAddressNumber">The invoice address record number.</param>
		Private Function LoadCustomerData(ByVal customerNumber As Integer, ByVal invoiceAddressNumber? As Integer) As Boolean

			'Reset()

			Dim success As Boolean = True

			success = LoadAssignedCustomerInvoiceAddresses(customerNumber, invoiceAddressNumber)
			success = success AndAlso LoadInvoiceAddressDetailData(customerNumber, invoiceAddressNumber)

			success = success AndAlso LoadAssignedCustomerKSTs(customerNumber, Nothing)
			success = success AndAlso LoadKSTDetailData(customerNumber, If(SelectedKSTViewData Is Nothing, Nothing, SelectedKSTViewData.RecordNumber))

			m_CustomerNumber = IIf(success, customerNumber, 0)

			If Not success Then
				m_UtilityUI.ShowErrorDialog("Kundendaten konnten nicht geladen werden.")
			End If

			Return success

		End Function

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()

			m_CustomerNumber = 0
			m_CurrentDetailInvoiceAddressRecordNumber = Nothing
			m_CurrentDetailKSTlRecordNumber = Nothing

			txtCompany1.Text = String.Empty
			txtCompany2.Text = String.Empty
			txtCompany3.Text = String.Empty
			txtDeparment.Text = String.Empty
			txtForTheAttentionOf.Text = String.Empty
			txtPostOfficeBox.Text = String.Empty
			txtStreet.Text = String.Empty
			txtLocation.Text = String.Empty
			tgsActiveInvoiceAddress.EditValue = False
			txtKSTDescription.Text = String.Empty
			txtKSTInfo1.Text = String.Empty
			txtKSTInfo2.Text = String.Empty

			txtEMail.Properties.MaxLength = 255
			txtEMail.EditValue = String.Empty
			tgsOneInvoicePerMail.EditValue = False
			tgsSendAsZIP.EditValue = False

			' ---Reset drop downs, grids and lists---

			ResetCountryDropDown()
			ResetPostcodeDropDown()
			ResetPaymentConditionDropDown()
			ResetPaymentReminderCodeDropDown()
			ResetKSTInvoiceNumberDropDown()

			ResetInvoiceAddressGrid()
			ResetKStGrid()

			TranslateControls()

			luePaymentReminderCode.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 206)
			Dim allowedtoDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 209)
			btnDeleteInvoiceAddress.Enabled = allowedtoDelete
			btnDeleteKST.Enabled = allowedtoDelete

			DxErrorProvider1.ClearErrors()

		End Sub


		''' <summary>
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetCountryDropDown()

			lueCountry.Properties.DisplayMember = "Translated_Value"
			lueCountry.Properties.ValueMember = "Code"

			Dim columns = lueCountry.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0))

			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 10
			lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCountry.Properties.SearchMode = SearchMode.AutoComplete
			lueCountry.Properties.AutoSearchColumnIndex = 0

			lueCountry.Properties.NullText = String.Empty
			lueCountry.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the postcode drop down.
		''' </summary>
		Private Sub ResetPostcodeDropDown()

			' Invoice address post code data
			luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
			luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

			luePostcode.Properties.DisplayMember = "Postcode"
			luePostcode.Properties.ValueMember = "Postcode"

			Dim columns = luePostcode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Postcode", 0))
			columns.Add(New LookUpColumnInfo("Location", 0))

			luePostcode.Properties.ShowFooter = False
			luePostcode.Properties.DropDownRows = 10
			luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePostcode.Properties.SearchMode = SearchMode.AutoComplete
			luePostcode.Properties.AutoSearchColumnIndex = 1
			luePostcode.Properties.NullText = String.Empty
			luePostcode.EditValue = Nothing

			' Employment Postcode data
			lueEmploymentPostcode.Properties.SearchMode = SearchMode.OnlyInPopup
			lueEmploymentPostcode.Properties.TextEditStyle = TextEditStyles.Standard

			lueEmploymentPostcode.Properties.DisplayMember = "Postcode"
			lueEmploymentPostcode.Properties.ValueMember = "Postcode"

			columns = lueEmploymentPostcode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Postcode", 0))
			columns.Add(New LookUpColumnInfo("Location", 0))

			lueEmploymentPostcode.Properties.ShowFooter = False
			lueEmploymentPostcode.Properties.DropDownRows = 10
			lueEmploymentPostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueEmploymentPostcode.Properties.SearchMode = SearchMode.AutoComplete
			lueEmploymentPostcode.Properties.AutoSearchColumnIndex = 1
			lueEmploymentPostcode.Properties.NullText = String.Empty
			lueEmploymentPostcode.EditValue = Nothing

			' BK Postcode data
			lueBKPostcode.Properties.SearchMode = SearchMode.OnlyInPopup
			lueBKPostcode.Properties.TextEditStyle = TextEditStyles.Standard

			lueBKPostcode.Properties.DisplayMember = "Postcode"
			lueBKPostcode.Properties.ValueMember = "Postcode"

			columns = lueBKPostcode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Postcode", 0))
			columns.Add(New LookUpColumnInfo("Location", 0))

			lueBKPostcode.Properties.ShowFooter = False
			lueBKPostcode.Properties.DropDownRows = 10
			lueBKPostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueBKPostcode.Properties.SearchMode = SearchMode.AutoComplete
			lueBKPostcode.Properties.AutoSearchColumnIndex = 1
			lueBKPostcode.Properties.NullText = String.Empty
			lueBKPostcode.EditValue = Nothing

		End Sub


		''' <summary>
		''' Resets the payment condition drop down.
		''' </summary>
		Private Sub ResetPaymentConditionDropDown()

			luePaymentConditions.Properties.DisplayMember = "GetField"
			luePaymentConditions.Properties.ValueMember = "GetField"

			Dim columns = luePaymentConditions.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0))

			luePaymentConditions.Properties.ShowFooter = False
			luePaymentConditions.Properties.DropDownRows = 10
			luePaymentConditions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePaymentConditions.Properties.SearchMode = SearchMode.AutoComplete
			luePaymentConditions.Properties.AutoSearchColumnIndex = 0
			luePaymentConditions.Properties.NullText = String.Empty
			luePaymentConditions.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the payment reminder code drop down.
		''' </summary>
		Private Sub ResetPaymentReminderCodeDropDown()

			luePaymentReminderCode.Properties.DisplayMember = "PaymentReminderDataString" ' "PaymentReminderCode"
			luePaymentReminderCode.Properties.ValueMember = "PaymentReminderCode"

			Dim columns = luePaymentReminderCode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("PaymentReminderDataString", 0))

			luePaymentReminderCode.Properties.ShowFooter = False
			luePaymentReminderCode.Properties.DropDownRows = 10
			luePaymentReminderCode.Properties.ShowHeader = False
			luePaymentReminderCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePaymentReminderCode.Properties.SearchMode = SearchMode.AutoComplete
			luePaymentReminderCode.Properties.AutoSearchColumnIndex = 0
			luePaymentReminderCode.Properties.NullText = String.Empty
			luePaymentReminderCode.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the invoice address grid.
		''' </summary>
		Private Sub ResetInvoiceAddressGrid()

			' Reset the grid
			gvExistingInvoiceAddressses.Columns.Clear()
			gvExistingInvoiceAddressses.OptionsView.ShowAutoFilterRow = True

			Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnRecordNumber.Name = "RecordNumber"
			columnRecordNumber.FieldName = "RecordNumber"
			columnRecordNumber.Visible = True
			columnRecordNumber.Width = 30
			gvExistingInvoiceAddressses.Columns.Add(columnRecordNumber)

			Dim columnInvoiceCompany As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoiceCompany.Caption = m_Translate.GetSafeTranslationValue("Firmenname")
			columnInvoiceCompany.Name = "InvoiceCompany"
			columnInvoiceCompany.FieldName = "InvoiceCompany"
			columnInvoiceCompany.Visible = True
			gvExistingInvoiceAddressses.Columns.Add(columnInvoiceCompany)

			Dim columnInvoiceAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoiceAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnInvoiceAddress.Name = "InvoiceAddress"
			columnInvoiceAddress.FieldName = "InvoiceAddress"
			columnInvoiceAddress.Visible = True
			gvExistingInvoiceAddressses.Columns.Add(columnInvoiceAddress)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
			activeColumn.Name = "Active"
			activeColumn.FieldName = "Active"
			activeColumn.Visible = True
			activeColumn.Width = 30
			activeColumn.ColumnEdit = m_CheckEditActive
			gvExistingInvoiceAddressses.Columns.Add(activeColumn)


			gridExistingInvoiceAddresses.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the KST grid.
		''' </summary>
		Private Sub ResetKStGrid()

			' Reset the grid
			gvAssignedKSTs.Columns.Clear()
			gvAssignedKSTs.OptionsView.ShowAutoFilterRow = True

			Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecordNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnRecordNumber.Name = "RecordNumber"
			columnRecordNumber.FieldName = "RecordNumber"
			columnRecordNumber.Visible = True
			columnRecordNumber.Width = 30
			gvAssignedKSTs.Columns.Add(columnRecordNumber)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			gvAssignedKSTs.Columns.Add(columnDescription)

			Dim columnInvoiceRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInvoiceRecordNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnInvoiceRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnInvoiceRecordNumber.Name = "KSTAddressString"
			columnInvoiceRecordNumber.FieldName = "KSTAddressString"
			columnInvoiceRecordNumber.Visible = True
			gvAssignedKSTs.Columns.Add(columnInvoiceRecordNumber)

		End Sub

		''' <summary>
		''' Resets the KST invoice number drop down.
		''' </summary>
		Private Sub ResetKSTInvoiceNumberDropDown()

			lueKSTInvoiceAddressNumber.Properties.DisplayMember = "KSTInvoiceAddressString"
			lueKSTInvoiceAddressNumber.Properties.ValueMember = "RecordNumber"

			Dim columns = lueKSTInvoiceAddressNumber.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("KSTInvoiceAddressString", 0))

			lueKSTInvoiceAddressNumber.Properties.ShowHeader = False
			lueKSTInvoiceAddressNumber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueKSTInvoiceAddressNumber.Properties.SearchMode = SearchMode.AutoComplete
			lueKSTInvoiceAddressNumber.Properties.AutoSearchColumnIndex = 0

			lueKSTInvoiceAddressNumber.Properties.NullText = String.Empty
			lueKSTInvoiceAddressNumber.EditValue = Nothing

		End Sub

		''' <summary>
		''' Loads customer assigned invoice addresses.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="addressRecordNumberToSelect">The record number of the address which should be selected. If this value is nothig then the first address will be selected if possible.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadAssignedCustomerInvoiceAddresses(ByVal customerNumber As Integer, ByVal addressRecordNumberToSelect As Integer?) As Boolean

			Dim assignedInvoiceAddresses = m_DataAccess.LoadAssignedInvoiceAddressDataOfCustomer(customerNumber)

			If assignedInvoiceAddresses Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsadressen konnten nicht geladen werden."))
				Return False
			End If

			Dim listDataSource As BindingList(Of InvoiceAddressViewData) = New BindingList(Of InvoiceAddressViewData)

			' Convert the data to view data.
			For Each address In assignedInvoiceAddresses

				Dim invoiceAddressViewData = New InvoiceAddressViewData() With {
								.Id = address.ID,
								.RecordNumber = address.RecordNumber,
								.InvoiceCompany = address.InvoiceCompany,
								.InvoiceEMailAddress = address.InvoiceEMailAddress,
								.InvoiceSendAsZip = address.InvoiceSendAsZip,
								.InvoiceAddress = String.Format("{0}-{1}, {2}", address.InvoiceCountryCode, address.InvoicePostcode, address.InvoiceLocation),
								.CountryCode = address.InvoiceCountryCode,
								.KSTInvoiceAddressString = String.Format("{0} - {1}, {2}, {3}, {4}, {5}-{6} {7}",
																												 address.RecordNumber,
																												 address.InvoiceCompany,
																												 IIf(String.IsNullOrEmpty(address.InvoiceDepartment), "-", address.InvoiceDepartment),
																												 IIf(String.IsNullOrEmpty(address.InvoiceForTheAttentionOf), "-", address.InvoiceForTheAttentionOf),
																												 address.InvoiceStreet,
																												 address.InvoiceCountryCode,
																												 address.InvoicePostcode,
																												 address.InvoiceLocation),
								.Active = address.Active}

				listDataSource.Add(invoiceAddressViewData)
			Next

			RemoveHandler gvExistingInvoiceAddressses.FocusedRowChanged, AddressOf OnGvExistingInvoiceAddressses_FocusedRowChanged
			gridExistingInvoiceAddresses.DataSource = listDataSource
			lueKSTInvoiceAddressNumber.Properties.DataSource = listDataSource

			If Not addressRecordNumberToSelect Is Nothing Then
				Dim index = assignedInvoiceAddresses.ToList().FindIndex(Function(data) data.RecordNumber = addressRecordNumberToSelect)
				gvExistingInvoiceAddressses.FocusedRowHandle = gvExistingInvoiceAddressses.GetRowHandle(index)
			ElseIf assignedInvoiceAddresses.Count > 0 Then
				gvExistingInvoiceAddressses.FocusedRowHandle = gvExistingInvoiceAddressses.GetVisibleRowHandle(0)
			End If

			AddHandler gvExistingInvoiceAddressses.FocusedRowChanged, AddressOf OnGvExistingInvoiceAddressses_FocusedRowChanged

			Return True

		End Function

		''' <summary>
		''' Loads customer assigned KSTs.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="kstIdToSelect">The id of the KST shich should be selected. If this value is nothig then the first KST will be selected if possible.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadAssignedCustomerKSTs(ByVal customerNumber As Integer, ByVal kstIdToSelect As Integer?) As Boolean

			Dim assignedKSTs = m_DataAccess.LoadAssignedKSTsOfCustomer(customerNumber)

			If assignedKSTs Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstellen konnten nicht geladen werden."))
				Return False
			End If

			Dim listDataSource As BindingList(Of KSTViewData) = New BindingList(Of KSTViewData)

			' Convert the data to view data.
			For Each kst In assignedKSTs

				Dim invoiceAddressViewData = New KSTViewData() With {
								.Id = kst.ID,
								.RecordNumber = kst.RecordNumber,
								.Description = kst.Description,
								.InvoiceRecordNumber = kst.InvoiceAddressRecordNumber,
								.KSTInvoiceAddressString = kst.InvoiceAddressInfo
								}

				listDataSource.Add(invoiceAddressViewData)
			Next

			RemoveHandler gvAssignedKSTs.FocusedRowChanged, AddressOf OnGvAssignedKSTs_FocusedRowChanged
			gridAssignedKSTs.DataSource = listDataSource

			If Not kstIdToSelect Is Nothing Then
				Dim index = assignedKSTs.ToList().FindIndex(Function(data) data.ID = kstIdToSelect)
				gvAssignedKSTs.FocusedRowHandle = gvAssignedKSTs.GetRowHandle(index)
			ElseIf assignedKSTs.Count > 0 Then
				gvAssignedKSTs.FocusedRowHandle = gvAssignedKSTs.GetVisibleRowHandle(0)
			End If

			AddHandler gvAssignedKSTs.FocusedRowChanged, AddressOf OnGvAssignedKSTs_FocusedRowChanged

			Return True

		End Function

		''' <summary>
		''' Loads detail data of an invoice address.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="invoiceRecordNumber">The record number of the invoice address.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadInvoiceAddressDetailData(ByVal customerNumber As Integer, ByVal invoiceRecordNumber As Integer?) As Boolean
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim success As Boolean = True

			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_InitializationData.UserData.UserFiliale)

			If (customerMasterData Is Nothing) Then
				success = False
			End If

			If invoiceRecordNumber.HasValue Then

				Dim invoiceAddress = m_DataAccess.LoadAssignedInvoiceAddressDataOfCustomerByRecordNumber(customerNumber, invoiceRecordNumber)

				If invoiceAddress Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdetaildaten konnten nicht geladen werden."))
					Return False
				End If

				txtCompany1.Text = invoiceAddress.InvoiceCompany
				txtCompany2.Text = invoiceAddress.InvoiceCompany2
				txtCompany3.Text = invoiceAddress.InvoiceCompany3
				txtDeparment.Text = invoiceAddress.InvoiceDepartment
				txtForTheAttentionOf.Text = invoiceAddress.InvoiceForTheAttentionOf
				txtPostOfficeBox.Text = invoiceAddress.InvoicePostOfficeBox
				txtStreet.Text = invoiceAddress.InvoiceStreet

				lueCountry.EditValue = invoiceAddress.InvoiceCountryCode

				' Supress event.
				RemoveHandler luePostcode.EditValueChanged, AddressOf OnLuePostcode_EditValueChanged

				' Add postcode if its not in list of known postcodes.
				Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

				If Not String.IsNullOrEmpty(invoiceAddress.InvoicePostcode) AndAlso
					Not listOfPostcode.Any(Function(postcode) postcode.Postcode = invoiceAddress.InvoicePostcode) Then
					Dim newPostcode As New PostCodeData With {.Postcode = invoiceAddress.InvoicePostcode}
					listOfPostcode.Add(newPostcode)
				End If
				luePostcode.EditValue = invoiceAddress.InvoicePostcode

				AddHandler luePostcode.EditValueChanged, AddressOf OnLuePostcode_EditValueChanged

				txtLocation.Text = invoiceAddress.InvoiceLocation
				txtEMail.Text = invoiceAddress.InvoiceEMailAddress
				tgsOneInvoicePerMail.EditValue = customerMasterData.OneInvoicePerMail
				tgsSendAsZIP.EditValue = invoiceAddress.InvoiceSendAsZip
				luePaymentConditions.EditValue = invoiceAddress.PaymentCondition
				luePaymentReminderCode.EditValue = invoiceAddress.ReminderCode
				tgsActiveInvoiceAddress.EditValue = invoiceAddress.Active

			Else


				txtCompany1.Text = If(customerMasterData Is Nothing, String.Empty, customerMasterData.Company1)
				txtCompany2.Text = String.Empty
				txtCompany3.Text = String.Empty
				txtDeparment.Text = String.Empty
				txtForTheAttentionOf.Text = String.Empty
				txtPostOfficeBox.Text = String.Empty
				txtStreet.Text = If(customerMasterData Is Nothing, String.Empty, customerMasterData.Street)

				lueCountry.EditValue = If(customerMasterData Is Nothing, "CH", customerMasterData.CountryCode)

				' Add postcode if its not in list of known postcodes.
				Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

				If Not String.IsNullOrEmpty(customerMasterData.Postcode) AndAlso
					Not listOfPostcode.Any(Function(postcode) postcode.Postcode = customerMasterData.Postcode) Then
					Dim newPostcode As New PostCodeData With {.Postcode = customerMasterData.Postcode}
					listOfPostcode.Add(newPostcode)
				End If
				luePostcode.EditValue = customerMasterData.Postcode

				txtLocation.Text = customerMasterData.Location
				txtEMail.Text = String.Empty
				tgsOneInvoicePerMail.EditValue = customerMasterData.OneInvoicePerMail
				tgsSendAsZIP.EditValue = True
				luePaymentConditions.EditValue = Nothing

				Dim invoiceremindercode As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_md.GetDefaultMDNr), String.Format("{0}/invoiceremindercode", FORM_XML_MAIN_KEY))
				luePaymentReminderCode.EditValue = If(String.IsNullOrWhiteSpace(invoiceremindercode), "A", invoiceremindercode)
				tgsActiveInvoiceAddress.EditValue = False

			End If

			m_CurrentDetailInvoiceAddressRecordNumber = invoiceRecordNumber

			DxErrorProvider1.ClearErrors()

			Return success
		End Function

		''' <summary>
		''' Loads KST detail data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="kstRecordNumber">The kst record number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadKSTDetailData(ByVal customerNumber As Integer, ByVal kstRecordNumber As Integer?) As Boolean

			If kstRecordNumber.HasValue Then

				Dim kstData = m_DataAccess.LoadAssignedKSTsOfCustomerByRecordNumber(customerNumber, kstRecordNumber)

				If kstData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstelledetaildaten konnen nicht geladen werden."))
					Return False
				End If

				txtKSTDescription.Text = kstData.Description
				lueKSTInvoiceAddressNumber.EditValue = kstData.InvoiceAddressRecordNumber

				' Add postcode if not in list of known postcodes.
				Dim listOfPostcode = CType(lueEmploymentPostcode.Properties.DataSource, List(Of PostCodeData))

				If Not String.IsNullOrEmpty(kstData.EmploymentPostCode) AndAlso
					Not listOfPostcode.Any(Function(postcode) postcode.Postcode = kstData.EmploymentPostCode) Then
					Dim newPostcode As New PostCodeData With {.Postcode = kstData.EmploymentPostCode}
					listOfPostcode.Add(newPostcode)
				End If
				lueEmploymentPostcode.EditValue = kstData.EmploymentPostCode


				' Add postcode if not in list of known postcodes.
				listOfPostcode = CType(lueBKPostcode.Properties.DataSource, List(Of PostCodeData))

				If Not String.IsNullOrEmpty(kstData.BKPostCode) AndAlso
					Not listOfPostcode.Any(Function(postcode) postcode.Postcode = kstData.BKPostCode) Then
					Dim newPostcode As New PostCodeData With {.Postcode = kstData.BKPostCode}
					listOfPostcode.Add(newPostcode)
				End If
				lueBKPostcode.EditValue = kstData.BKPostCode

				txtKSTInfo1.Text = kstData.Info1
				txtKSTInfo2.Text = kstData.Info2
			Else
				txtKSTDescription.Text = String.Empty
				lueKSTInvoiceAddressNumber.EditValue = Nothing
				lueKSTInvoiceAddressNumber.EditValue = Nothing
				lueEmploymentPostcode.EditValue = Nothing
				lueBKPostcode.EditValue = Nothing
				txtKSTInfo1.Text = String.Empty
				txtKSTInfo2.Text = String.Empty

			End If

			m_CurrentDetailKSTlRecordNumber = kstRecordNumber

			' Clear errors
			DxErrorProvider1.ClearErrors()

			Return True
		End Function

		''' <summary>
		''' Handles focus change of invoice address row.
		''' </summary>
		Private Sub OnGvExistingInvoiceAddressses_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvExistingInvoiceAddressses.FocusedRowChanged
			Dim selectedInvoiceAddressViewDataRow = SelectedInvoiceAddressViewData

			If Not selectedInvoiceAddressViewDataRow Is Nothing Then
				LoadInvoiceAddressDetailData(m_CustomerNumber, selectedInvoiceAddressViewDataRow.RecordNumber)
			End If
		End Sub

		''' <summary>
		''' Handles double click on invoice address.
		''' </summary>
		Private Sub OnGvExistingInvoiceAddressses_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvExistingInvoiceAddressses.DoubleClick
			Dim selectedInvoiceAddressViewDataRow = SelectedInvoiceAddressViewData

			If Not selectedInvoiceAddressViewDataRow Is Nothing Then
				LoadInvoiceAddressDetailData(m_CustomerNumber, SelectedInvoiceAddressViewData.RecordNumber)
			End If
		End Sub

		''' <summary>
		''' Handles focus change of KST row.
		''' </summary>
		Private Sub OnGvAssignedKSTs_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
			Dim selectedKSTViewDataRow = SelectedKSTViewData

			If Not selectedKSTViewDataRow Is Nothing Then
				LoadKSTDetailData(m_CustomerNumber, selectedKSTViewDataRow.RecordNumber)
			End If
		End Sub

		''' <summary>
		''' Handles double click on KST row.
		''' </summary>
		Private Sub OnGvAssignedKSTs_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvAssignedKSTs.DoubleClick
			Dim selectedKSTViewDataRow = SelectedKSTViewData

			If Not selectedKSTViewDataRow Is Nothing Then
				LoadKSTDetailData(m_CustomerNumber, selectedKSTViewDataRow.RecordNumber)
			End If
		End Sub

		''' <summary>
		''' Handles new value event on postcode(plz) lookup edit.
		''' </summary>
		Private Sub OnLuePostcode_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles luePostcode.ProcessNewValue, lueBKPostcode.ProcessNewValue, lueEmploymentPostcode.ProcessNewValue

			Dim postcodeedit = CType(sender, LookUpEdit)

			If Not postcodeedit.Properties.DataSource Is Nothing Then

				Dim listOfPostcode = CType(postcodeedit.Properties.DataSource, List(Of PostCodeData))

				Dim newPostcode As New PostCodeData With {.Postcode = e.DisplayValue.ToString()}
				listOfPostcode.Add(newPostcode)

				e.Handled = True
			End If
		End Sub

		''' <summary>
		''' Handles click on save invoice address button.
		''' </summary>
		Private Sub OnBtnSaveInvoiceAddressData_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveInvoiceAddressData.Click

			If ValidateInvoiceAddressInputData() Then
				Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(m_CustomerNumber, m_InitializationData.UserData.UserFiliale)

				Dim invoiceAddress As CustomerAssignedInvoiceAddressData = Nothing
				Dim dt = DateTime.Now
				If Not m_CurrentDetailInvoiceAddressRecordNumber.HasValue Then

					invoiceAddress = New CustomerAssignedInvoiceAddressData With {
							.CustomerNumber = m_CustomerNumber,
							.CreatedOn = dt,
							.CreatedFrom = m_ClsProgSetting.GetUserName()
							}
				Else

					Dim selectedAddress As InvoiceAddressViewData = SelectedInvoiceAddressViewData

					If (selectedAddress Is Nothing) Then
						Return
					End If

					invoiceAddress = m_DataAccess.LoadAssignedInvoiceAddressDataOfCustomerByRecordNumber(m_CustomerNumber, selectedAddress.RecordNumber)

					If invoiceAddress Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
						Return
					End If

				End If

				invoiceAddress.InvoiceCompany = txtCompany1.Text
				invoiceAddress.InvoiceCompany2 = txtCompany2.Text
				invoiceAddress.InvoiceCompany3 = txtCompany3.Text
				invoiceAddress.InvoiceDepartment = txtDeparment.Text
				invoiceAddress.InvoiceForTheAttentionOf = txtForTheAttentionOf.Text
				invoiceAddress.InvoicePostOfficeBox = txtPostOfficeBox.Text
				invoiceAddress.InvoiceStreet = txtStreet.Text
				invoiceAddress.InvoiceCountryCode = lueCountry.EditValue
				invoiceAddress.InvoicePostcode = luePostcode.EditValue
				invoiceAddress.InvoiceLocation = txtLocation.Text
				invoiceAddress.InvoiceEMailAddress = txtEMail.Text
				invoiceAddress.InvoiceSendAsZip = tgsSendAsZIP.EditValue
				invoiceAddress.PaymentCondition = luePaymentConditions.EditValue
				invoiceAddress.ReminderCode = luePaymentReminderCode.EditValue
				invoiceAddress.Active = tgsActiveInvoiceAddress.EditValue
				invoiceAddress.ChangedFrom = m_InitializationData.UserData.UserFullName
				invoiceAddress.ChangedOn = dt

				Dim success As Boolean = True

				customerMasterData.OneInvoicePerMail = tgsOneInvoicePerMail.EditValue
				success = success AndAlso m_DataAccess.UpdateCustomerAssignedEMailDeliveryProperties(customerMasterData)

				If (invoiceAddress.ID = 0) Then
					success = m_DataAccess.AddCustomerInvoiceAddressAssignment(invoiceAddress)
					m_CurrentDetailInvoiceAddressRecordNumber = invoiceAddress.RecordNumber
				Else
					success = m_DataAccess.UpdateCustomerAssignedInvoiceAddress(invoiceAddress)
				End If


				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
				Else
					LoadAssignedCustomerInvoiceAddresses(m_CustomerNumber, invoiceAddress.RecordNumber)

					RaiseEvent InvoiceAddressDataSaved(Me, m_CustomerNumber, m_CurrentDetailInvoiceAddressRecordNumber)
				End If

			End If

		End Sub

		''' <summary>
		''' Handles click on save KST button.
		''' </summary>
		Private Sub OnBtnSaveKST_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveKST.Click

			If ValidateKSTInputData() Then

				Dim kstData As CustomerAssignedKSTData = Nothing

				If Not m_CurrentDetailKSTlRecordNumber.HasValue Then
					kstData = New CustomerAssignedKSTData With {.CustomerNumber = m_CustomerNumber}
				Else

					Dim selectedKST = SelectedKSTViewData

					If (selectedKST Is Nothing) Then
						Return
					End If

					kstData = m_DataAccess.LoadAssignedKSTsOfCustomerByRecordNumber(m_CustomerNumber, selectedKST.RecordNumber)

					If kstData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
						Return
					End If
				End If

				kstData.Description = txtKSTDescription.Text
				kstData.InvoiceAddressRecordNumber = lueKSTInvoiceAddressNumber.EditValue

				kstData.EmploymentPostCode = lueEmploymentPostcode.EditValue
				kstData.BKPostCode = lueBKPostcode.EditValue

				kstData.Info1 = txtKSTInfo1.Text
				kstData.Info2 = txtKSTInfo2.Text

				Dim success As Boolean = True

				If kstData.ID = 0 Then
					success = m_DataAccess.AddCustomerKSTAssignment(kstData)
					m_CurrentDetailKSTlRecordNumber = kstData.RecordNumber
				Else
					success = m_DataAccess.UpdateCustomerAssignedKST(kstData)
				End If

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
				Else
					LoadAssignedCustomerKSTs(m_CustomerNumber, kstData.ID)

					RaiseEvent InvoiceKSTDataSaved(Me, m_CustomerNumber)

				End If

			End If

		End Sub

		''' <summary>
		''' Handles click on delete invoice address button.
		''' </summary>
		Private Sub OnBtnDeleteInvoiceAddress_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteInvoiceAddress.Click
			Dim invoiceAddressData = SelectedInvoiceAddressViewData

			If Not invoiceAddressData Is Nothing AndAlso m_CurrentDetailInvoiceAddressRecordNumber.HasValue Then

				If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																				m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
					Return
				End If

				Dim result = m_DataAccess.DeleteCustomerInvoiceAddressAssignment(invoiceAddressData.Id, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

				Select Case result
					Case DeleteCustomerInvoiceAddressAssignmentResult.CouldNotDeleteOnlyOneRecordLeft
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Es existiert nur ein Datensatz. Bitte erfassen Sie einen neuen Datensatz und dann löschen Sie die gewünschten Daten."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
					Case DeleteCustomerInvoiceAddressAssignmentResult.CouldNotDeleteBecauseOfExistingKST
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnungsadresse kann nich gelöscht werden, da sie bereits mit einer Kostenstelle verbunden ist."))
					Case DeleteCustomerInvoiceAddressAssignmentResult.ErrorWhileDelete
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Rechnungsadresse konnte nicht gelöscht werden."))
					Case DeleteCustomerInvoiceAddressAssignmentResult.Deleted
						LoadAssignedCustomerInvoiceAddresses(m_CustomerNumber, Nothing)
						LoadInvoiceAddressDetailData(m_CustomerNumber, If(SelectedInvoiceAddressViewData Is Nothing, Nothing, SelectedInvoiceAddressViewData.RecordNumber))
						RaiseEvent InvoiceAddressDataDeleted(Me, m_CustomerNumber, invoiceAddressData.Id)
				End Select
			End If

		End Sub

		''' <summary>
		''' Handles click on new invoice address button.
		''' </summary>
		Private Sub OnBtnNewInvoiceAddress_Click(sender As System.Object, e As System.EventArgs) Handles btnNewInvoiceAddress.Click
			LoadInvoiceAddressDetailData(m_CustomerNumber, Nothing)
		End Sub

		''' <summary>
		''' Handles click on new KST button.
		''' </summary>

		Private Sub OnBtnNewKST_Click(sender As System.Object, e As System.EventArgs) Handles btnNewKST.Click
			LoadKSTDetailData(m_CustomerNumber, Nothing)
		End Sub

		''' <summary>
		''' Handles click on delete KST button.
		''' </summary>
		Private Sub OnBtnDeleteKST_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteKST.Click
			Dim kstViewData = SelectedKSTViewData

			If Not kstViewData Is Nothing AndAlso m_CurrentDetailKSTlRecordNumber.HasValue Then
				If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																				m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
					Return
				End If

				Dim result = m_DataAccess.DeleteCustomerKSTAssignment(kstViewData.Id)

				Select Case result
					Case DeleteCustomerKSTAssignmentResult.CouldNotDeleteOnlyOneRecordLeft
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Es existiert nur ein Datensatz. Bitte erfassen Sie einen neuen Datensatz und dann löschen Sie die gewünschten Daten."),
																		 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerKSTAssignmentResult.CouldNotDeleteBecauseOfExistingEsLohn
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Kostenstelle kann nicht gelöscht werden, da sie bereits mit einem Einsatz verbunden ist."))

					Case DeleteCustomerKSTAssignmentResult.CouldNotDeleteBecauseOfExistingRapport
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Kostenstelle kann nicht gelöscht werden, da sie bereits mit einem Rapport verbunden ist."))

					Case DeleteCustomerKSTAssignmentResult.ErrorWhileDelete
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der Datensatz konnte nicht gelöscht werden."), m_Translate.GetSafeTranslationValue("Information"), MessageBoxIcon.Exclamation)

					Case DeleteCustomerInvoiceAddressAssignmentResult.Deleted
						LoadAssignedCustomerKSTs(m_CustomerNumber, Nothing)
						LoadKSTDetailData(m_CustomerNumber, If(SelectedKSTViewData Is Nothing, Nothing, SelectedKSTViewData.RecordNumber))

						RaiseEvent InvoiceKSTDataDeleted(Me, m_CustomerNumber)

				End Select

			End If

		End Sub

		''' <summary>
		''' Handles change of postcode.
		''' </summary>
		Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

			Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)

			If Not postCodeData Is Nothing Then
				txtLocation.Text = postCodeData.Location
			End If

		End Sub


		''' <summary>
		''' Validates address address input.
		''' </summary>
		Private Function ValidateInvoiceAddressInputData() As Boolean

			DxErrorProvider1.ClearErrors()
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorEMailAddress As String = m_Translate.GetSafeTranslationValue("Die eingetragene EMail-Adresse ist nicht richtig.")

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(txtCompany1, DxErrorProvider1, String.IsNullOrEmpty(txtCompany1.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(txtStreet, DxErrorProvider1, String.IsNullOrEmpty(txtStreet.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueCountry, DxErrorProvider1, lueCountry.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(luePostcode, DxErrorProvider1, luePostcode.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(txtLocation, DxErrorProvider1, String.IsNullOrEmpty(txtLocation.Text), errorText)

			Dim eMailPattern As New Regex("\A[a-z0-9!#$%&'*+/=?^_‘{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_‘{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\z")
			txtEMail.EditValue = txtEMail.Text.ToString.ToLower
			'isValid = isValid And SetErrorIfInvalid(txtEMail, DxErrorProvider1, Not String.IsNullOrWhiteSpace(txtEMail.EditValue) AndAlso Not eMailPattern.IsMatch(txtEMail.EditValue), errorEMailAddress)

			isValid = isValid And SetErrorIfInvalid(txtEMail, DxErrorProvider1, Not String.IsNullOrWhiteSpace(txtEMail.EditValue) AndAlso Not m_Utility.IsEMailAddressValid(txtEMail.Text.ToString.ToLower), errorEMailAddress)
			isValid = isValid And SetErrorIfInvalid(luePaymentReminderCode, DxErrorProvider1, luePaymentReminderCode.EditValue Is Nothing, errorText)

			'Dim mailmsg As New System.Net.Mail.MailMessage
			'mailmsg.To.Add(txtEMail.Text.ToString.ToLower)


			Return isValid
		End Function

		''' <summary>
		''' Validates KST input.
		''' </summary>
		Private Function ValidateKSTInputData() As Boolean

			DxErrorProvider1.ClearErrors()
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorNotAllowedText As String = m_Translate.GetSafeTranslationValue("Die 1. Kostenstelle ist ein System-Kostenstelle und daher darf nicht überschrieben werden.")
			Dim errorDopplicatedText As String = m_Translate.GetSafeTranslationValue("Die Kostenstelle existiert bereits!")

			Dim isValid As Boolean = True

			isValid = isValid AndAlso SetErrorIfInvalid(txtKSTDescription, DxErrorProvider1, String.IsNullOrEmpty(txtKSTDescription.Text), errorText)
			isValid = isValid AndAlso SetErrorIfInvalid(lueKSTInvoiceAddressNumber, DxErrorProvider1, lueKSTInvoiceAddressNumber.EditValue Is Nothing, errorText)

			isValid = isValid AndAlso SetErrorIfInvalid(txtKSTDescription, DxErrorProvider1, m_CurrentDetailKSTlRecordNumber.GetValueOrDefault(0) = 1 AndAlso txtKSTDescription.EditValue <> "Standard Kostenstelle", errorNotAllowedText)


			Dim listDataSource As BindingList(Of KSTViewData) = gridAssignedKSTs.DataSource
			isValid = isValid AndAlso SetErrorIfInvalid(txtKSTDescription, DxErrorProvider1, listDataSource Is Nothing, errorDopplicatedText)

			Dim existsKSTViewData
			If m_CurrentDetailKSTlRecordNumber.GetValueOrDefault(0) = 0 Then
				existsKSTViewData = listDataSource.Where(Function(data) data.Description = txtKSTDescription.EditValue).FirstOrDefault()
			Else
				existsKSTViewData = listDataSource.Where(Function(data) data.Description = txtKSTDescription.EditValue AndAlso data.RecordNumber <> m_CurrentDetailKSTlRecordNumber).FirstOrDefault()
			End If

			If Not existsKSTViewData Is Nothing Then
				isValid = isValid AndAlso SetErrorIfInvalid(txtKSTDescription, DxErrorProvider1, Not existsKSTViewData Is Nothing, errorDopplicatedText)
			End If


			Return isValid
		End Function

		''' <summary>
		''' Validates a control
		''' </summary>
		''' <param name="control">The control to validate.</param>
		''' <param name="errorProvider">The error providor.</param>
		''' <param name="invalid">Boolean flag if data is invalid.</param>
		''' <param name="errorText">The error text.</param>
		''' <returns>Valid flag</returns>
		Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight)
			If (invalid) Then
				errorProvider.SetError(control, errorText)
			Else
				errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is LookUpEdit Then
					Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
					lookupEdit.EditValue = Nothing
				End If
			End If
		End Sub

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmInvoiceAddress_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
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
		Private Sub OnFrmInvoiceAddress_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

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

		End Sub

		''' <summary>
		''' Keypreview for Modul-version
		''' </summary>
		Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
			If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
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

#End Region


#Region "View helper classes"

		''' <summary>
		''' Payment reminder data.
		''' </summary>
		Class PaymentReminderViewData
			Public Property PaymentReminderCode As String
			Public Property PaymentReminderDataString As String
		End Class

		''' <summary>
		'''  Invoice address view data.
		''' </summary>
		Class InvoiceAddressViewData
			Public Property Id As Integer
			Public Property RecordNumber As Integer?
			Public Property InvoiceCompany As String
			Public Property InvoiceEMailAddress As String
			Public Property InvoiceSendAsZip As Boolean?
			Public Property InvoiceAddress As String
			Public Property CountryCode As String
			Public Property KSTInvoiceAddressString As String
			Public Property Active As Boolean?

		End Class

		''' <summary>
		'''  KST view data.
		''' </summary>
		Class KSTViewData
			Public Property Id As Integer
			Public Property RecordNumber As Integer?
			Public Property Description As String
			Public Property InvoiceRecordNumber As Integer?

			Public Property KSTInvoiceAddressString As String

			Public ReadOnly Property KSTAddressString As String
				Get
					Return String.Format("{0} - {1}", InvoiceRecordNumber, KSTInvoiceAddressString)
				End Get
			End Property

		End Class

#End Region

	End Class


End Namespace