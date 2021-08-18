
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.DatabaseAccess.Invoice
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports System.ComponentModel
Imports SP.DatabaseAccess.Report
Imports DevExpress.XtraBars.Docking2010
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraEditors.DXErrorProvider

Namespace UI

	''' <summary>
	''' Edit Invoice Payment.
	''' </summary>
	Public Class frmZEedit

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
		''' Contains the invoice number of the loaded invoice data.
		''' </summary>
		Private m_invoiceData As DataObjects.Invoice

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
		''' The financial Accounts database access.
		''' </summary>
		Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

		Private m_ReportDatabaseAccess As IReportDatabaseAccess

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
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		''' <summary>
		''' Communication support between controls.
		''' </summary>
		Protected m_UCMediator As NewInvoiceUserControlFormMediator

		''' <summary>
		''' The common settings.
		''' </summary>
		Private m_Common As CommonSetting

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

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

		' Other Modul variables 
		Private m_postingAccounts As New Dictionary(Of Integer, String)
		Private m_CostCenters As SP.DatabaseAccess.Common.DataObjects.CostCenters
		Private m_advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)
		Private m_PaymentNumber As Integer?
		Private m_PaymentData As PaymentMasterData

		' Standard Modul Variables
		Private m_Skonto As Integer?
		Private m_Verlust As Integer?
		Private m_Rueckverguetung As Integer?
		Private m_SollKontoStandard As Integer?


		Private m_allowedtoSave As Boolean
		Private m_allowedtoDelete As Boolean


#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="_setting">The settings.</param>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			' Mandantendaten
			m_Mandant = New Mandant
			m_path = New ClsProgPath
			m_Common = New CommonSetting

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn

			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_ReportDatabaseAccess = New ReportDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			ChangeMandant(m_InitializationData.MDData.MDNr)

			'Reset all Controls
			Reset()
			TranslateControls()
			LoadDropDownData()

			' Button Click Handle
			AddHandler daeValutaDate.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler daeBookingDate.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueKonto.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueMandant.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueKst1.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueKst2.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueAdvisor1.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueAdvisor2.ButtonClick, AddressOf OnDropDownButtonClick

		End Sub

#End Region


#Region "Public Properties"

		Public Property CurrentPaymentNumber As Integer?

#End Region


#Region "public methods"

		''' <summary>
		''' Load Invoice and Payments Records
		''' </summary>
		Public Function LoadData() As Boolean
			Dim success As Boolean = True

			Try
				m_PaymentNumber = CurrentPaymentNumber
				success = success AndAlso LoadPaymentData()

				m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_PaymentData.RENR)
				success = success AndAlso DisplayPaymentDetail()

			Catch ex As Exception
				success = False

			End Try

			Return success

		End Function

#End Region


#Region "private properties"

		''' <summary>
		''' Reads the payment offset from the settings.
		''' </summary>
		''' <returns>payment offset or zero if it could not be read.</returns>
		Private ReadOnly Property ReadPaymentOffsetFromSettings() As Integer
			Get
				Dim strQuery As String = "//StartNr/Zahlungseingänge"
				Dim r = m_ClsProgSetting.GetUserProfileFile
				Dim paymentNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
				Dim intVal As Integer

				If Integer.TryParse(paymentNumberStartNumberSetting, intVal) Then
					Return intVal
				Else
					Return 0
				End If

			End Get
		End Property

		Private ReadOnly Property LoadAutomatedKontoNumber() As Integer
			Get
				Dim value As Integer = 0
				Dim invoiceYear As Integer = Now.Year
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim mandantNumber = lueMandant.EditValue
				If Not m_invoiceData Is Nothing Then invoiceYear = Year(m_invoiceData.FakDat)

				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", mandantNumber)

				value = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mandantNumber, invoiceYear), String.Format("{0}/_7", FORM_XML_MAIN_KEY)))

				Return value
			End Get
		End Property

		''' <summary>
		''' Read Payment SKonto, DebtLoss, ReImbursement Value in Percent
		''' </summary>
		Private ReadOnly Property GetPaymentPercentage(ByVal dataEnum As ExtraDataEnun) As Decimal

			Get
				Dim intWert As Integer
				Dim percentageValue As Decimal
				Dim fieldValue As Decimal

				If m_invoiceData.BetragInk = 0 Then Return 0

				Select Case dataEnum
					Case ExtraDataEnun.SKonto
						fieldValue = Val(txtSkonto.EditValue)

					Case ExtraDataEnun.DebitLoss
						fieldValue = Val(txtDebtLoss.EditValue)

					Case ExtraDataEnun.Refund
						fieldValue = Val(txtReImbursement.EditValue)

					Case ExtraDataEnun.TotalReduction
						fieldValue = Val(txtTotalDeductions.EditValue)

					Case ExtraDataEnun.OpenAmount
						fieldValue = Val(txtOutstandingValue.EditValue)

				End Select

				intWert = ((fieldValue / m_invoiceData.BetragInk) * 100) * 100
				percentageValue = intWert / 100

				Return percentageValue

			End Get

		End Property

		Private ReadOnly Property SelectedViewData As PaymentExtraData
			Get
				Dim grdView = TryCast(grdInvoicePayments.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim contact = CType(grdView.GetRow(selectedRows(0)), PaymentExtraData)
						Return contact
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


#Region "Reset grids"

		''' <summary>
		''' Reset all controls in this from.
		''' </summary>
		Private Sub Reset()

			' GroupControl 1 - Rechnungsdaten (Nur Anzeige)
			daeInvoiceDate.EditValue = Nothing
			daeDueDate.EditValue = Nothing
			medZahlungsinformation.EditValue = String.Empty
			daeInvoiceDate.Properties.ReadOnly = True
			daeDueDate.Properties.ReadOnly = True
			medZahlungsinformation.Properties.ReadOnly = True

			' GroupControl 3 - Zahlungseingangsdaten (Zur Bearbeitung)
			daeValutaDate.EditValue = Nothing
			daeBookingDate.EditValue = Nothing
			txtBetragZE.EditValue = 0D

			' GroupControl 4 - Kundendaten Zahlung (Nur Anzeige)
			txtCustomer.EditValue = String.Empty
			txtAbteilung.EditValue = String.Empty
			txtStreet.EditValue = String.Empty
			txtPostCode.EditValue = String.Empty
			txtLocation.EditValue = String.Empty
			txtCustomer.Properties.ReadOnly = True
			txtAbteilung.Properties.ReadOnly = True
			txtStreet.Properties.ReadOnly = True
			txtPostCode.Properties.ReadOnly = True
			txtLocation.Properties.ReadOnly = True

			' GroupControl 5 - Payment Details (All Payments Grid - SKonto, DebtLoss, ReImbursement Value from all Payments)
			txtBetragInk.EditValue = 0D
			txtAlreadyPaid.EditValue = 0D
			txtSkonto.EditValue = 0D
			txtDebtLoss.EditValue = 0D
			txtReImbursement.EditValue = 0D
			txtOutstandingValue.EditValue = 0D
			txtTotalDeductions.EditValue = 0D
			txtBetragInk.Properties.ReadOnly = True
			txtAlreadyPaid.Properties.ReadOnly = True
			txtSkonto.Properties.ReadOnly = True
			txtDebtLoss.Properties.ReadOnly = True
			txtReImbursement.Properties.ReadOnly = True
			txtOutstandingValue.Properties.ReadOnly = True
			txtTotalDeductions.Properties.ReadOnly = True

			lblSkontoPercent.Text = String.Empty
			lblDebtLossPercent.Text = String.Empty
			lblRefundPercent.Text = String.Empty
			lblOutstandingPercent.Text = String.Empty
			lblTotalDeductionsPercent.Text = String.Empty

			' Reset all DropDown Controls (GroupControl 2 - Eigenschaften und Merkmale  & GroupControl Zahlungseingang) 
			ResetMandantDropDown()
			ResetKstDropDown()
			ResetAdvisorDropDown()
			ResetKontoDropDown()
			ResetInvoicePaymentGrid()

			m_allowedtoSave = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 501, m_InitializationData.MDData.MDNr)
			m_allowedtoDelete = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 502, m_InitializationData.MDData.MDNr)

			bbiDelete.Enabled = m_allowedtoDelete
			bbiSave.Enabled = m_allowedtoSave
			bbiNewPayment.Enabled = m_allowedtoSave

			DxErrorProvider1.ClearErrors()

		End Sub
		''' <summary>
		''' Resets the Mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			' GroupControl 2 - Mandant (Nur Anzeige)
			lueMandant.EditValue = Nothing
			lueMandant.Properties.ReadOnly = True
			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"
			lueMandant.Properties.Columns.Clear()
			lueMandant.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the Kst1 and Kst2 drop down.
		''' </summary>
		Private Sub ResetKstDropDown()

			' Kostenstelle 1
			lueKst1.EditValue = Nothing
			lueKst1.Properties.ReadOnly = True
			lueKst1.Properties.DisplayMember = "KSTBezeichnung"
			lueKst1.Properties.ValueMember = "KSTName"
			lueKst1.Properties.Columns.Clear()
			lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
			lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))
			lueKst1.EditValue = Nothing

			' Kostenstelle 2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.ReadOnly = True
			lueKst2.Properties.DisplayMember = "KSTBezeichnung"
			lueKst2.Properties.ValueMember = "KSTName"
			lueKst2.Properties.Columns.Clear()
			lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
			lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))
			lueKst2.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the Advisor1 and Advisor2 drop down.
		''' </summary>
		Private Sub ResetAdvisorDropDown()

			' Advisor1
			lueAdvisor1.EditValue = Nothing
			lueAdvisor1.Properties.ReadOnly = True
			lueAdvisor1.Properties.DisplayMember = "UserFullname"
			lueAdvisor1.Properties.ValueMember = "KST"
			lueAdvisor1.Properties.Columns.Clear()
			lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
			lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))
			lueAdvisor1.EditValue = Nothing

			' Advisor2
			lueAdvisor2.EditValue = Nothing
			lueAdvisor2.Properties.ReadOnly = True
			lueAdvisor2.Properties.DisplayMember = "UserFullname"
			lueAdvisor2.Properties.ValueMember = "KST"
			lueAdvisor2.Properties.Columns.Clear()
			lueAdvisor2.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
			lueAdvisor2.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))
			lueAdvisor2.EditValue = Nothing

		End Sub

		''' <summary>
		''' reset Invoice Payments grid
		''' </summary>
		''' <remarks></remarks>
		Private Sub ResetInvoicePaymentGrid()

			' Reset Grid with all incoming payments of the current invoice
			gvInvoicePayments.FocusRectStyle = DrawFocusRectStyle.RowFocus
			gvInvoicePayments.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
			gvInvoicePayments.OptionsView.ShowGroupPanel = False
			gvInvoicePayments.OptionsView.ShowIndicator = False
			gvInvoicePayments.OptionsView.ShowAutoFilterRow = True
			gvInvoicePayments.OptionsView.ShowFooter = True
			gvInvoicePayments.Columns.Clear()

			Try
				Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
				columnmodulname.Caption = m_Translate.GetSafeTranslationValue("Nummer")
				columnmodulname.Name = "ZENr"
				columnmodulname.FieldName = "ZENr"
				columnmodulname.Visible = True
				gvInvoicePayments.Columns.Add(columnmodulname)

				Dim columnFKSollKonto As New DevExpress.XtraGrid.Columns.GridColumn()
				columnFKSollKonto.Caption = m_Translate.GetSafeTranslationValue("Konto")
				columnFKSollKonto.Name = "FKSollKonto"
				columnFKSollKonto.FieldName = "FKSollKonto"
				columnFKSollKonto.Visible = True
				gvInvoicePayments.Columns.Add(columnFKSollKonto)

				Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
				columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Valuta")
				columnFakDat.Name = "ValutaDate"
				columnFakDat.FieldName = "ValutaDate"
				columnFakDat.Visible = True
				gvInvoicePayments.Columns.Add(columnFakDat)

				Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
				columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
				columnAmount.Name = "Amount"
				columnAmount.FieldName = "Amount"
				columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
				columnAmount.AppearanceHeader.Options.UseTextOptions = True
				columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
				columnAmount.DisplayFormat.FormatString = "N2"
				columnAmount.Visible = True
				columnAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
				columnAmount.SummaryItem.DisplayFormat = "{0:n2}"
				gvInvoicePayments.Columns.Add(columnAmount)

				Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
				columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
				columnCreatedOn.Name = "CreatedOn"
				columnCreatedOn.FieldName = "CreatedOn"
				columnCreatedOn.Visible = True
				columnCreatedOn.BestFit()
				gvInvoicePayments.Columns.Add(columnCreatedOn)

				Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
				columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
				columnCreatedFrom.Name = "CreatedFrom"
				columnCreatedFrom.FieldName = "CreatedFrom"
				columnCreatedFrom.Visible = True
				columnCreatedFrom.BestFit()
				gvInvoicePayments.Columns.Add(columnCreatedFrom)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			grdInvoicePayments.DataSource = Nothing

		End Sub

		''' <summary>
		''' Reset the Financial accounts DropDowngrid
		''' </summary>
		''' <remarks></remarks>
		Private Sub ResetKontoDropDown()

			lueKonto.Properties.DisplayMember = "KontoViewData"
			lueKonto.Properties.ValueMember = "KontoNr"

			gvKonto.OptionsView.ShowIndicator = False
			gvKonto.OptionsView.ShowColumnHeaders = True
			gvKonto.OptionsView.ShowFooter = False

			gvKonto.OptionsView.ShowAutoFilterRow = True
			gvKonto.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvKonto.Columns.Clear()

			Dim columnCustomerNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnCustomerNumber.Name = "KontoNr"
			columnCustomerNumber.FieldName = "KontoNr"
			columnCustomerNumber.Visible = True
			columnCustomerNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvKonto.Columns.Add(columnCustomerNumber)

			Dim columnCompany1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany1.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnCompany1.Name = "TranslatedLabel"
			columnCompany1.FieldName = "TranslatedLabel"
			columnCompany1.Visible = True
			columnCompany1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvKonto.Columns.Add(columnCompany1)

			lueKonto.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueKonto.Properties.NullText = String.Empty
			lueKonto.EditValue = Nothing

		End Sub

#End Region


		''' <summary>
		''' Translates the controls
		''' </summary>
		Private Sub TranslateControls()

			Dim advisor1 = lueAdvisor1.EditValue
			Dim advisor2 = lueAdvisor2.EditValue
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)


			lblInvoiceDate.Text = m_Translate.GetSafeTranslationValue(lblInvoiceDate.Text)
			lblDueDate.Text = m_Translate.GetSafeTranslationValue(lblDueDate.Text)
			lblZahlungsinformation.Text = m_Translate.GetSafeTranslationValue(lblZahlungsinformation.Text)

			' GroupControl 2 - Mandant
			grcMandant.Text = m_Translate.GetSafeTranslationValue(grcMandant.Text)
			lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)
			lblKst1.Text = m_Translate.GetSafeTranslationValue(lblKst1.Text)
			lblAdvisor.Text = m_Translate.GetSafeTranslationValue(lblAdvisor.Text)
			lblSelectedAdvisors.Text = advisor1 & "/" & advisor2

			lblValutaDate.Text = m_Translate.GetSafeTranslationValue(lblValutaDate.Text)
			lblKontoZE.Text = m_Translate.GetSafeTranslationValue(lblKontoZE.Text)
			lblBookingDate.Text = m_Translate.GetSafeTranslationValue(lblBookingDate.Text)
			lblBetragZE.Text = m_Translate.GetSafeTranslationValue(lblBetragZE.Text)

			lblCustomer.Text = m_Translate.GetSafeTranslationValue(lblCustomer.Text)
			lblAbteilung.Text = m_Translate.GetSafeTranslationValue(lblAbteilung.Text)
			lblStreet.Text = m_Translate.GetSafeTranslationValue(lblStreet.Text)
			lblCountry.Text = m_Translate.GetSafeTranslationValue(lblCountry.Text)
			lblPostcode.Text = m_Translate.GetSafeTranslationValue(lblPostcode.Text)
			lblLocation.Text = m_Translate.GetSafeTranslationValue(lblLocation.Text)

			grdInvoicePayments.Text = m_Translate.GetSafeTranslationValue(grdInvoicePayments.Text)
			lblBetragInk.Text = m_Translate.GetSafeTranslationValue(lblBetragInk.Text)
			lblAlreadyPaid.Text = m_Translate.GetSafeTranslationValue(lblAlreadyPaid.Text)
			lblSkonto.Text = m_Translate.GetSafeTranslationValue(lblSkonto.Text)
			lblDebtLoss.Text = m_Translate.GetSafeTranslationValue(lblDebtLoss.Text)
			lblReImbursement.Text = m_Translate.GetSafeTranslationValue(lblReImbursement.Text)
			lblOutstandingValue.Text = m_Translate.GetSafeTranslationValue(lblOutstandingValue.Text)
			lblSkontoPercent.Text = m_Translate.GetSafeTranslationValue(lblSkontoPercent.Text)

			lblTotalDeductions.Text = m_Translate.GetSafeTranslationValue(lblTotalDeductions.Text)
			grpInvoice.Text = m_Translate.GetSafeTranslationValue(grpInvoice.Text)
			grpCustomer.Text = m_Translate.GetSafeTranslationValue(grpCustomer.Text)
			grpInvoicePayments.Text = m_Translate.GetSafeTranslationValue(grpInvoicePayments.Text)

			btnAddInvoicePayment.ToolTip = m_Translate.GetSafeTranslationValue("Neue Zahlung für die gleiche Rechnung erstellen.")

			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)
			bsilblCreated.Caption = m_Translate.GetSafeTranslationValue(bsilblCreated.Caption)
			bsiChanged.Caption = m_Translate.GetSafeTranslationValue(bsilblChanged.Caption)
			bbiDelete.Caption = m_Translate.GetSafeTranslationValue(bbiDelete.Caption)

		End Sub

		''' <summary>
		''' Loads all Drop Down data.
		''' </summary>
		Private Sub LoadDropDownData()

			LoadMandantDropDown()
			LoadKst1DropDown()
			LoadKst2DropDown()
			LoadAdvisorDropDown()
			LoadKontoData()

		End Sub

		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantDropDown() As Boolean

			Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				Dim msgText As String = "Mandantendaten konnten nicht geladen werden."
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msgText))

				Return False
			End If

			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing

		End Function

		''' <summary>
		''' Loads the Kst1 drop down data.
		''' </summary>
		Private Sub LoadKst1DropDown()

			m_CostCenters = m_CommonDatabaseAccess.LoadCostCenters()

			' Kostenstelle 1
			lueKst1.Properties.DataSource = m_CostCenters.CostCenter1
			lueKst1.Properties.ForceInitialize()

			' Kostenstelle 2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.DataSource = Nothing
			lueKst2.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the Kst2 drop down data.
		''' </summary>
		Private Sub LoadKst2DropDown()

			If (m_CostCenters Is Nothing) Then
				Return
			End If

			Dim kst1Name = lueKst1.EditValue
			Dim kst2Data = m_CostCenters.GetCostCenter2ForCostCenter1(kst1Name)

			' Kostenstelle 2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.DataSource = kst2Data
			lueKst2.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the Advisor1 and Advisor2 drop down data.
		''' </summary>
		Private Sub LoadAdvisorDropDown()

			' Load data
			m_advisors = m_CommonDatabaseAccess.LoadAllAdvisorsData()

			' Advisor1
			lueAdvisor1.Properties.DataSource = m_advisors
			lueAdvisor1.Properties.ForceInitialize()

			' Advisor2
			lueAdvisor2.Properties.DataSource = m_advisors
			lueAdvisor2.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the Financial Accounts data.
		''' </summary>
		Private Function LoadKontoData() As Boolean
			Dim success As Boolean = True

			Dim listOfData = m_TablesettingDatabaseAccess.LoadFIBUKontenData(m_InitializationData.UserData.UserLanguage)

			If listOfData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Konto Daten konnten nicht geladen werden."))

				Return False
			End If
			lueKonto.Properties.DataSource = listOfData

			Return Not listOfData Is Nothing

		End Function


#Region "Private Methods"

		''' <summary>
		''' Loads Payment Data from DataBase.
		''' </summary>
		Private Function LoadPaymentData() As Boolean
			Dim success As Boolean = True

			m_PaymentData = m_InvoiceDatabaseAccess.LoadPaymentData(m_InitializationData.MDData.MDNr, m_PaymentNumber)

			If m_PaymentData Is Nothing Then
				Dim msgText As String = "Zahlungseingang Daten konnten nicht geladen werden."
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msgText))

				Return False
			End If

			Return Not (m_PaymentData Is Nothing)

		End Function

		Private Function LoadPaymentExtraPercentageData() As PaymentExtraPercentageData
			Dim result As New PaymentExtraPercentageData

			result.SKonto = GetPaymentPercentage(ExtraDataEnun.SKonto)
			result.DebitLoss = GetPaymentPercentage(ExtraDataEnun.DebitLoss)
			result.Refund = GetPaymentPercentage(ExtraDataEnun.Refund)
			result.TotalReduction = GetPaymentPercentage(ExtraDataEnun.TotalReduction)
			result.OpenAmount = GetPaymentPercentage(ExtraDataEnun.OpenAmount)


			Return result

		End Function

		Private Function IsMonthClosed() As Boolean
			Dim result As Boolean = True
			If m_PaymentData Is Nothing Then Return False

			Dim monthClosedData = m_ReportDatabaseAccess.LoadMonthCloseData(CType(lueMandant.EditValue, Integer), Month(m_PaymentData.VDate), Year(m_PaymentData.VDate))

			result = Not monthClosedData Is Nothing

			Return result

		End Function

		''' <summary>
		''' Display the Payment Data into Form.Controls
		''' </summary>
		Private Function DisplayPaymentDetail() As Boolean

			Dim strText As String = ""
			Dim success As Boolean = True
			Dim PaymentYear As Integer
			Dim sKontoAmount As Decimal = 0D
			Dim debtLossAmount As Decimal = 0D
			Dim refundAmount As Decimal = 0D

			Dim msgText1 As String = m_Translate.GetSafeTranslationValue("Zahlungseingang Daten konnten nicht geladen werden.")
			' Stop -- ToDo  (Text Übersetzung prüfen und für das Programm nachführen)
			Dim msgText2 As String = m_Translate.GetSafeTranslationValue("Der Datensatz ist verbucht und kann nicht geändert werden.")

			If m_PaymentData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(msgText1)

				Return False
			End If

			Try
				' load Financial Accounting Accounts based on posting date
				success = success AndAlso DisplayMandantDetails()
				success = success AndAlso DisplayInvoiceDetails()

				PaymentYear = Year(m_PaymentData.VDate.GetValueOrDefault(Now.Date))
				LoadPostingAccounts(PaymentYear)

				Dim isAssignedMonthClosed As Boolean = IsMonthClosed()
				btnAddInvoicePayment.Enabled = m_invoiceData.OpenAmount.GetValueOrDefault(0) > 0
				bbiDelete.Enabled = m_allowedtoDelete AndAlso Not isAssignedMonthClosed
				bbiSave.Enabled = m_allowedtoSave AndAlso Not isAssignedMonthClosed

				daeValutaDate.EditValue = m_PaymentData.VDate
				daeBookingDate.EditValue = m_PaymentData.BDate
				lueKonto.EditValue = m_PaymentData.FKSOLL
				txtBetragZE.EditValue = m_PaymentData.Amount

				grdInvoicePayments.DataSource = m_PaymentData.PaymentExtraAmounts                          ' load all Payments done in this Invoice into Grid
				FocusInvoicePayment(m_PaymentData.ZENr)

				For Each itm In m_PaymentData.PaymentExtraAmounts
					If itm.FKSollKonto = m_Skonto Then
						sKontoAmount += itm.Amount
					ElseIf itm.FKSollKonto = m_Verlust Then
						debtLossAmount += itm.Amount
					ElseIf itm.FKSollKonto = m_Rueckverguetung Then
						refundAmount += itm.Amount
					End If
				Next

				txtSkonto.EditValue = sKontoAmount
				txtDebtLoss.EditValue = debtLossAmount
				txtReImbursement.EditValue = refundAmount
				txtTotalDeductions.EditValue = sKontoAmount + debtLossAmount + refundAmount

				Dim reductionData = LoadPaymentExtraPercentageData()
				lblSkontoPercent.Text = If(reductionData.SKonto = 0, String.Empty, String.Format("{0:n2} %", reductionData.SKonto))
				lblDebtLossPercent.Text = If(reductionData.DebitLoss = 0, String.Empty, String.Format("{0:n2} %", reductionData.DebitLoss))
				lblRefundPercent.Text = If(reductionData.Refund = 0, String.Empty, String.Format("{0:n2} %", reductionData.Refund))
				lblTotalDeductionsPercent.Text = If(reductionData.TotalReduction = 0, String.Empty, String.Format("{0:n2} %", reductionData.TotalReduction))
				lblOutstandingPercent.Text = If(reductionData.OpenAmount = 0, String.Empty, String.Format("{0:n2} %", reductionData.OpenAmount))

				bsiCreated.Caption = String.Format("{0:f}, {1}", m_PaymentData.CreatedOn, m_PaymentData.CreatedFrom)
				bsiChanged.Caption = String.Format("{0:f}, {1}", m_PaymentData.ChangedOn, m_PaymentData.ChangedFrom)

			Catch ex As Exception
				Return False
			End Try


			Return success

		End Function

		Private Function DisplayMandantDetails() As Boolean
			Dim success As Boolean = True

			Try

				lueMandant.EditValue = m_PaymentData.MDNr
				lueKst1.EditValue = m_PaymentData.REKST1
				lueKst2.EditValue = m_PaymentData.REKST2
				Dim advisors = m_invoiceData.KST.Split({"/"c})
				If advisors.Length = 1 Then
					SelectAdvisor(lueAdvisor1, advisors(0))
					SelectAdvisor(lueAdvisor2, advisors(0))
				ElseIf advisors.Length = 2 Then
					SelectAdvisor(lueAdvisor1, advisors(0))
					SelectAdvisor(lueAdvisor2, advisors(1))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return success

		End Function

		Private Function DisplayInvoiceDetails() As Boolean
			Dim success As Boolean = True

			Try
				grpInvoice.Text = String.Format(m_Translate.GetSafeTranslationValue("Rechnungsdaten: {0}"), m_PaymentData.RENR)
				daeInvoiceDate.EditValue = m_invoiceData.FakDat
				daeDueDate.EditValue = m_invoiceData.Faellig
				medZahlungsinformation.EditValue = m_invoiceData.ZEInfo
				txtBetragInk.EditValue = m_invoiceData.BetragInk
				txtAlreadyPaid.EditValue = m_invoiceData.Bezahlt
				txtOutstandingValue.EditValue = m_invoiceData.OpenAmount

				grpCustomer.Text = String.Format(m_Translate.GetSafeTranslationValue("Rechnungsempfänger: {0}"), m_PaymentData.KDNR)
				txtCustomer.EditValue = m_invoiceData.RName1
				If Not String.IsNullOrWhiteSpace(m_invoiceData.RName2) Then
					txtCustomer.EditValue = String.Format("{0} / {1}", m_invoiceData.RName1, m_invoiceData.RName2)
				End If
				txtAbteilung.EditValue = m_invoiceData.RZHD
				txtStreet.EditValue = m_invoiceData.RStrasse
				txtPostCode.EditValue = m_invoiceData.RPLZ
				txtLocation.EditValue = m_invoiceData.ROrt
				txtCountry.EditValue = m_invoiceData.RLand

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Return False
			End Try

			Return success
		End Function

		''' <summary>
		''' Selects an advisor and add missing advisor
		''' </summary>
		Private Sub SelectAdvisor(lueAdvisor As LookUpEdit, advisorKST As String)

			Dim advisor = (From a In m_advisors Where a.KST = advisorKST).FirstOrDefault
			If advisor Is Nothing Then
				'Add missing advisor
				m_advisors.Add(New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = advisorKST})
			End If
			lueAdvisor.EditValue = advisorKST

		End Sub

		''' <summary>
		''' Set the focus on the current payment receipt
		''' </summary>
		Private Sub FocusInvoicePayment(ByVal paymentNumber As Integer)
			If Not grdInvoicePayments.DataSource Is Nothing Then

				Dim paymentViewData = CType(gvInvoicePayments.DataSource, List(Of PaymentExtraData))
				Dim index = paymentViewData.ToList().FindIndex(Function(data) data.ZENr = paymentNumber)

				m_SuppressUIEvents = True
				Dim rowHandle = gvInvoicePayments.GetRowHandle(index)
				gvInvoicePayments.FocusedRowHandle = rowHandle
				m_SuppressUIEvents = False

			End If

		End Sub

#End Region


#Region "Event Handles"

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

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

#End Region

#Region "helpers"

		''' <summary>
		''' Loads the Posting Accounts from XML
		''' </summary>
		''' <param name="year">The Year for the PostingAccount</param>
		Private Sub LoadPostingAccounts(ByVal year As Integer?)

			Dim intSkontoPos As Short
			Dim intVerlustPos As Short
			Dim intRueckvergPos As Short

			If Not year.HasValue Then year = m_InitializationData.MDData.MDYear

			m_postingAccounts = New Dictionary(Of Integer, String)

			Try
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", m_InitializationData.MDData.MDNr)
				For i As Integer = 1 To 38
					Dim strValue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(m_Mandant.GetDefaultMDNr, year), String.Format("{0}/_{1}", FORM_XML_MAIN_KEY, i)), "")
					m_postingAccounts.Add(i, strValue)
				Next

				Select Case m_PaymentData.REArt
					Case "A"
						If m_PaymentData.InvoiceWithTax Then
							intSkontoPos = 10
						Else
							intSkontoPos = 12
						End If
						intVerlustPos = 22
						intRueckvergPos = 24

					Case "I"
						If m_PaymentData.InvoiceWithTax Then
							intSkontoPos = 11
						Else
							intSkontoPos = 13
						End If
						intVerlustPos = 26
						intRueckvergPos = 28

					Case "F"
						intSkontoPos = 20 + m_PaymentData.InvoiceWithTax
						intVerlustPos = 30
						intRueckvergPos = 32

				End Select

				intVerlustPos += m_PaymentData.InvoiceWithTax
				intRueckvergPos += m_PaymentData.InvoiceWithTax
				m_Skonto = m_postingAccounts(intSkontoPos)
				m_Verlust = m_postingAccounts(intVerlustPos)
				m_Rueckverguetung = m_postingAccounts(intRueckvergPos)
				m_SollKontoStandard = m_postingAccounts(7)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		''' <summary>
		''' Changes the mandant number
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		Private Sub ChangeMandant(ByVal mdNr As Integer)

			Dim conStr = m_Mandant.GetSelectedMDData(mdNr).MDDbConn

			If Not m_CurrentConnectionString = conStr Then
				m_CurrentConnectionString = conStr
				m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
				m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
			End If

		End Sub

		''''' <summary>
		''''' Sets the "SaveButton" to Enable in case of changes
		''''' </summary>
		'Private Sub EnableDisableSaveButton()

		'	bbiSave.Enabled = Not m_PaymentIsLocked And CheckIfFieldsChanged(False)
		'	If Not bbiSave.Enabled Then
		'		bbiSave.Enabled = m_PaymentData.ZENr Is Nothing
		'	End If

		'End Sub

		''''' <summary>
		''''' Check the changed PaymentValue changed
		''''' </summary>
		'Private Sub txtBetragZE_LostFocus(sender As Object, e As EventArgs) Handles txtBetragZE.LostFocus
		'	CheckPaymentAmount(False)
		'End Sub

		'''' <summary>
		'''' Compare Valuta Date and Booking Date
		'''' </summary>
		'Private Sub CheckBookingDate()

		'	If daeValutaDate.EditValue > daeBookingDate.EditValue OrElse daeValutaDate.EditValue = Nothing Then
		'		daeBookingDate.EditValue = daeValutaDate.EditValue
		'	End If

		'	EnableDisableSaveButton()

		'End Sub

		'''' <summary>
		'''' Checks if fields have changed
		'''' </summary>
		'''' <param name = "blnSetAllChangeableFields" >True = Read changeable fields (Reset) / False = Check if a field has changed</param>
		'Private Function CheckIfFieldsChanged(blnSetAllChangeableFields As Boolean) As Boolean
		'	Dim strChangeableFields As String = String.Empty

		'	'If m_PaymentData.ZENr Is Nothing Then
		'	'  Return False
		'	'End if
		'	If blnSetAllChangeableFields Then
		'		strChangeableFields = String.Format("{0:dd.MM.yyy}|{1:dd.MM.yyy}|{2}|{3:n2}", daeValutaDate.EditValue, daeBookingDate.EditValue, lueKonto.EditValue, Val(txtBetragZE.EditValue))
		'		Return True
		'	Else
		'		Return strChangeableFields <> String.Format("{0:dd.MM.yyy}|{1:dd.MM.yyy}|{2}|{3:n2}", daeValutaDate.EditValue, daeBookingDate.EditValue, lueKonto.EditValue, Val(txtBetragZE.EditValue))
		'	End If

		'End Function

		'''' <summary>
		'''' Check the Valuta Date and Booking Date
		'''' </summary>
		'Private Sub daeBookingDate_LostFocus(sender As Object, e As EventArgs) Handles daeBookingDate.LostFocus
		'	CheckBookingDate()
		'End Sub

		'''' <summary>
		'''' Check the Valuta Date and Booking Date
		'''' </summary>
		'Private Sub daeValutaDate_LostFocus(sender As Object, e As EventArgs) Handles daeValutaDate.LostFocus
		'	CheckBookingDate()
		'End Sub

		''' <summary>
		''' Save current data entry
		''' </summary>
		Private Sub bbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
			Dim success As Boolean = True
			Dim msgSavingErrorText As String = m_Translate.GetSafeTranslationValue("Die Daten können nicht gespeichert werden.")

			If Not ValidateData() Then Return
			Try
				If m_PaymentData.ZENr Is Nothing Then
					success = success AndAlso SaveNewPaymentWithAssignedInvoice()

				Else
					success = success AndAlso SaveAssignedPayment()

				End If
				success = success AndAlso DisplayPaymentDetail()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

		End Sub

		Private Sub OnbbiNewPayment_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNewPayment.ItemClick
			ShowNewPaymentWizard()
		End Sub
		Private Function SaveNewPaymentWithAssignedInvoice() As Boolean
			Dim success As Boolean = True
			Dim initData As New NewPaymentInitData
			Dim offsetNumber = ReadPaymentOffsetFromSettings

			Try

				initData.PaymentNumberOffset = offsetNumber
				initData.MDNr = m_invoiceData.MDNr
				initData.RENR = m_invoiceData.ReNr
				initData.KDNR = m_invoiceData.KdNr
				initData.VDate = daeValutaDate.EditValue
				initData.BDate = daeBookingDate.EditValue
				initData.Currency = m_invoiceData.Currency
				initData.Amount = txtBetragZE.EditValue
				initData.FKSOLL = lueKonto.EditValue
				initData.FKHABEN = m_invoiceData.FKSoll
				initData.CreatedFrom = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_InvoiceDatabaseAccess.AddNewPayment(initData)
				If Not success Then Return False

				m_PaymentNumber = initData.NewPaymentNr

			Catch ex As Exception
				success = False
			End Try

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))

			Else
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

				success = success AndAlso LoadPaymentData()
				m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_PaymentData.RENR)
			End If


			Return success

		End Function

		''' <summary>
		''' Save current Payment data entry
		''' </summary>
		Private Function SaveAssignedPayment() As Boolean

			Dim success As Boolean = True

			' is done allready!
			'If Not ValidateData() Then Return False
			Try

				m_PaymentData.Amount = txtBetragZE.EditValue
				m_PaymentData.BDate = daeBookingDate.EditValue
				m_PaymentData.VDate = daeValutaDate.EditValue
				m_PaymentData.FKSOLL = lueKonto.EditValue
				m_PaymentData.ChangedOn = DateTime.Now
				m_PaymentData.ChangedFrom = m_InitializationData.UserData.UserFullName
				success = success AndAlso m_InvoiceDatabaseAccess.UpdatePaymentMasterData(m_PaymentData.ZENr, m_PaymentData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))

			Else
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

				success = success AndAlso LoadPaymentData()
				m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_PaymentData.RENR)

			End If
			bbiSave.Enabled = Not success


			Return success

		End Function

		''' <summary>
		''' Check if the Month is Closed 
		''' </summary>
		Private Function ValidateData() As Boolean

			' Clear errors
			DxErrorProvider1.ClearErrors()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorClosedMonthText As String = m_Translate.GetSafeTranslationValue("Der Monat wurde bereits abgeschlossen.")
			Dim errorInvalidAmountText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen gültigen Betrag ein. Der Betrag darf den offenen Rechnungsbetrag nicht übersteigen!")
			Dim errorInvalidDateText As String = m_Translate.GetSafeTranslationValue("Das ausgewählte Datum ist nicht gültig.")

			Dim payedAmountWithoutAssignedPayment As Decimal = 0
			Dim isValid As Boolean = True

			isValid = isValid And SetDXErrorIfInvalid(lueKonto, DxErrorProvider1, lueKonto.EditValue Is Nothing, errorText)
			isValid = isValid And SetDXErrorIfInvalid(daeValutaDate, DxErrorProvider1, IsMonthClosed, errorClosedMonthText)

			If m_PaymentData.ZENr Is Nothing Then
				payedAmountWithoutAssignedPayment = m_invoiceData.OpenAmount
			Else
				payedAmountWithoutAssignedPayment = m_invoiceData.OpenAmount + m_PaymentData.Amount
			End If

			isValid = isValid And SetDXErrorIfInvalid(txtBetragZE, DxErrorProvider1,
																								txtBetragZE.EditValue Is Nothing OrElse txtBetragZE.EditValue = 0 OrElse txtBetragZE.EditValue > payedAmountWithoutAssignedPayment,
																								errorInvalidAmountText)

			isValid = isValid And SetDXErrorIfInvalid(daeValutaDate, DxErrorProvider1, daeValutaDate.EditValue Is Nothing OrElse daeValutaDate.EditValue > daeBookingDate.EditValue, errorInvalidDateText)
			isValid = isValid And SetDXErrorIfInvalid(daeBookingDate, DxErrorProvider1, daeBookingDate.EditValue Is Nothing OrElse daeValutaDate.EditValue > daeBookingDate.EditValue, errorInvalidDateText)


			Return isValid

		End Function

		'''' <summary>
		'''' Delete Payment Record.
		'''' </summary>
		Private Sub bbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

			Dim msgText As String = m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?")
			Dim msgHeader As String = m_Translate.GetSafeTranslationValue("Datensatz löschen")

			If m_PaymentData Is Nothing OrElse m_PaymentData.ZENr Is Nothing Then Return
			If Not (m_UtilityUI.ShowYesNoDialog(msgText, msgHeader, MessageBoxDefaultButton.Button2)) Then Return

			Dim result = m_InvoiceDatabaseAccess.DeleteAssingedPaymentData(lueMandant.EditValue, m_PaymentData.ZENr, "ZE", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)

			Select Case result
				Case DeleteREResult.ResultCanNotDeleteBecauseMonthIsClosed
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Zahlung kann nicht gelöscht werden, da der Monat bereits abgeschlossen ist."),
																																											 msgHeader, MessageBoxIcon.Exclamation)

				Case DeleteREResult.ResultDeleteError
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Zahlung konnte nicht gelöscht werden."))

				Case DeleteREResult.ResultDeleteOk
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Zahlung wurde gelöscht."))
					Me.Close()

			End Select

		End Sub

		Private Sub btnAddInvoicePayment_Click(sender As Object, e As EventArgs) Handles btnAddInvoicePayment.Click

			Dim success As Boolean = True

			' Clear errors
			DxErrorProvider1.ClearErrors()

			m_PaymentData.ZENr = Nothing

			daeValutaDate.EditValue = Now.Date
			daeBookingDate.EditValue = Now.Date
			lueKonto.EditValue = LoadAutomatedKontoNumber
			txtBetragZE.EditValue = 0D

			txtBetragZE.Focus()

		End Sub

		'''' <summary>
		'''' Show billing information in a separate window
		'''' </summary>
		Private Sub grpInvoice_CustomButtonClick(sender As Object, e As BaseButtonEventArgs) Handles grpInvoice.CustomButtonClick

			If m_PaymentData Is Nothing Then Return

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_PaymentData.MDNr, m_PaymentData.RENR)
			hub.Publish(openMng)

		End Sub

		'''' <summary>
		'''' Show customer data in a separate window
		'''' </summary>
		Private Sub grpKundendaten_CustomButtonClick(sender As Object, e As BaseButtonEventArgs) Handles grpCustomer.CustomButtonClick

			If m_PaymentData IsNot Nothing Then
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_PaymentData.MDNr, m_PaymentData.KDNR)
				hub.Publish(openMng)
			End If

		End Sub

		'''' <summary>
		'''' Display the selected incoming payment
		'''' </summary>
		Private Sub grdInovicePayments_DoubleClick(sender As Object, e As EventArgs) Handles grdInvoicePayments.DoubleClick
			Dim success As Boolean = True

			' Clear errors
			DxErrorProvider1.ClearErrors()

			Dim data = SelectedViewData
			If data Is Nothing Then Return

			m_PaymentNumber = data.ZENr
			success = success AndAlso LoadPaymentData()

			If Not success Then
				' must exit program!
				Me.Close()

				Return
			End If

			m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_PaymentData.RENR)
			success = success AndAlso DisplayPaymentDetail()

		End Sub

		''' <summary>
		''' Shows new ES wizard.
		''' </summary>
		Private Sub ShowNewPaymentWizard()
			If m_InitializationData.MDData.ClosedMD = 1 Then Return

			Dim frm As SP.KD.InvoiceMng.UI.frmNewZahlungsEingang = New frmNewZahlungsEingang(m_InitializationData, Nothing)

			frm.Show()
			frm.BringToFront()

			Me.Close()

		End Sub

#End Region

#Region "Helpers"

		Protected Function SetDXErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText, ErrorType.Critical)
			End If

			Return Not invalid

		End Function

		Protected Function SetDXWarningIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText, ErrorType.Warning)
			End If

			Return Not invalid

		End Function


		Private Enum ExtraDataEnun
			SKonto
			DebitLoss
			Refund
			OpenAmount
			TotalReduction
		End Enum


#End Region


		Private Class PaymentExtraPercentageData
			Public Property SKonto As Decimal?
			Public Property DebitLoss As Decimal?
			Public Property Refund As Decimal?
			Public Property OpenAmount As Decimal?
			Public Property TotalReduction As Decimal?

		End Class

	End Class

End Namespace
