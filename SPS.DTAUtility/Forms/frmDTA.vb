
Imports System.Reflection
Imports SP.DatabaseAccess

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports DevExpress.LookAndFeel
Imports SPS.DTAUtility.Settings
Imports System.Reflection.Assembly
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.XtraReports.UI
Imports System.ComponentModel.Design
Imports DevExpress.XtraReports.UserDesigner

Imports SPS.Listing.Print.Utility
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports DevExpress.XtraGrid.Views.Base
Imports SP.DatabaseAccess.DTAUtility.DataObjects
Imports DevExpress.XtraEditors.DXErrorProvider
Imports System.Configuration
Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Permissions
Imports System.Security
Imports System.Security.Principal

Namespace UI

	''' <summary>
	''' DTA Form.
	''' </summary>
	''' <remarks></remarks>
	Public Class frmDTA

#Region "Public Shared Methods"

		''' <summary>
		''' Shows a new form instance.
		''' </summary>
		''' <param name="initData"></param>
		''' <param name="showModal"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ShowForm(ByVal initData As SP.Infrastructure.Initialization.InitializeClass, Optional ByVal showModal As Boolean = False) As frmDTA
			Dim form = New frmDTA(initData)
			form.LoadData(initData)
			If showModal Then
				form.ShowDialog()
			Else
				form.Show()
			End If
			Return form
		End Function

#End Region ' Public Shared Methods


#Region "Private Enums"

		Private Enum SelectionListType
			Payments
			PaymentsCreditor
			Jobs
		End Enum

		Private Enum ActionType
			Create
			Print
			Delete
		End Enum

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
		''' The current connection string.
		''' </summary>
		Private m_CurrentConnectionString = String.Empty

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As SP.DatabaseAccess.Common.ICommonDatabaseAccess

		''' <summary>
		''' The DAT utility data access object.
		''' </summary>
		Private m_DTAUtilityDatabaseAccess As SP.DatabaseAccess.DTAUtility.IDTAUtilityDatabaseAccess

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

		Private m_IsDataValid As Boolean = True

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_mandant As Mandant
		Private m_path As ClsProgPath
		Private m_PayrollSetting As String

		Private m_selectionColumnEdit As RepositoryItemCheckEdit

		Private m_mandantDataList As IEnumerable(Of MandantData)

		Private m_transactionDataList As IList(Of TransactionData)
		Private m_transactionDataSelected As TransactionData

		Private m_jobNumberDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData)
		Private m_jobNumberDataSelected As SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData

		Private m_bankDataList As IList(Of SP.DatabaseAccess.DTAUtility.DataObjects.BankData)
		Private m_bankDataSelected As SP.DatabaseAccess.DTAUtility.DataObjects.BankData

		Private m_bankChargesList As IList(Of BankCharges)
		Private m_bankChargesSelected As BankCharges

		Private m_selectionFormatString As String
		Private m_selectionPaymentsFormatString As String
		Private m_selectionPaymentsCreditorFormatString As String
		Private m_selectionJobsFormatString As String
		Private m_SelectedAmount As Decimal

		Private m_selectionListType As SelectionListType?
		Private m_selectionList As IList(Of ISelectionItem)

		Private m_actionType As ActionType?
		''' <summary>
		''' allowed to open lists designmodus?
		''' </summary>
		''' <remarks></remarks>
		Private m_AllowedDesign As Boolean
		''' <summary>
		''' allowed action to create
		''' </summary>
		''' <remarks></remarks>
		Private m_AllowedCreate As Boolean
		Private m_AllowedMandantChange As Boolean
		Private m_AllowedPain001 As Boolean

#End Region  ' Private Fields


#Region "Private Consts"

		Private Const Field_DefaultValues As String = "Forms_Normaly/Field_DefaultValues"
		Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"

		Private Const PRINTJOB_CREDITOR_SWISS As String = "6.2.1"
		Private Const PRINTJOB_DTA As String = "6.2"
		Private Const PRINTJOB_VG As String = "6.5"

#End Region


#Region "Private Properties"

		Private ReadOnly Property GetHwnd() As String
			Get
				Return Me.Handle
			End Get
		End Property

		''' <summary>
		''' should open printform?
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property CheckResetvalueAfterCreatedDTAjob As Integer
			Get

				Dim value As Integer = ParseToInteger(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr),
																																							 String.Format("{0}/resetvalueaftercreateddtajob", Field_DefaultValues)), 0)
				If value > 3 Then value = 3
				If value < 0 Then value = 0

				Return value

			End Get
		End Property

		Private ReadOnly Property MandantAllowedPain001 As Boolean
			Get
				Dim result As Boolean = False
				result = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year),
																																							 String.Format("{0}/mandantallowedwithpain001", m_PayrollSetting)), False)

				Return result

			End Get
		End Property


#End Region



#Region "Private Methods"

		''' <summary>
		''' Constructor (Private to force use of shared sub ShowForm).
		''' </summary>
		''' <param name="initData"></param>
		''' <remarks></remarks>
		Private Sub New(ByVal initData As SP.Infrastructure.Initialization.InitializeClass)
			Try
				' Mandantendaten
				m_mandant = New Mandant
				m_path = New ClsProgPath

				m_InitializationData = initData
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(initData.TranslationData, initData.ProsonalizedData)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True

			InitializeComponent()

			m_SuppressUIEvents = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			Me.CreateDatabaseAccessObjects(m_InitializationData.MDData.MDNr)

			m_SettingsManager = New SettingsManager
			m_UtilityUI = New UtilityUI
			m_Utility = New Utility

			' Translate controls.
			TranslateControls()

			m_PayrollSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING, m_InitializationData.MDData.MDNr)
			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 566, m_InitializationData.MDData.MDNr)
			m_AllowedCreate = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 562, m_InitializationData.MDData.MDNr)
			m_AllowedMandantChange = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)

			Reset()

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueTransactionType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueBankData.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueBankCharges.ButtonClick, AddressOf OnDropDown_ButtonClick

			m_AllowedPain001 = MandantAllowedPain001
			Me.LoadTransactionDataDropDownData(0)
			Me.LoadBankChargesDrowpDownData()

			LoadFormSettings()

			Me.LoadData(initData)
		End Sub

		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <returns>True wenn erfolgreich</returns>
		Private Function LoadData(ByVal initData As SP.Infrastructure.Initialization.InitializeClass) As Boolean
			Dim success As Boolean = True

			' Mandanten laden
			success = LoadMandantDropDownData()

			' Mandant auswählen
			Me.SetMandant(initData.MDData.MDNr)

			' Transaktionsart auswählen
			Me.SetTransactionDefault()

			Dim dt As New DateTime(Now.Year, Now.Month, Now.Day)

			If Now.Hour > 11 Then
				dt = dt.AddDays(1)
			Else
				dt = dt.AddDays(0)
			End If
			If dt.DayOfWeek = DayOfWeek.Saturday Then
				dt = dt.AddDays(2)
			ElseIf dt.DayOfWeek = DayOfWeek.Sunday Then
				dt = dt.AddDays(1)
			End If
			dteTransactionDate.EditValue = dt

			txtFilePath.EditValue = m_SettingsManager.ReadString(SettingKeys.SETTING_DTAFile_LOCATION)
			If String.IsNullOrWhiteSpace(txtFilePath.EditValue) Then txtFilePath.Text = String.Format("C:\Path\Test {0:yyyy-MM-dd HH.mm.ss}.dta", DateTime.Now)

			Return success

		End Function


#Region "Reset Form"

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			' Reset grids, drop downs and lists, etc.
			ResetMandantDropDown()
			ResetBankDataDropDown()
			ResetTransactionDataDropDown()
			ResetBankChargesDropDown()
			ResetSelectionListGrid()

			Me.lueMandant.Visible = m_AllowedMandantChange
			Me.lblMandant.Visible = m_AllowedMandantChange


			DxErrorProvider1.ClearErrors()

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

			lueMandant.Properties.ShowFooter = False
			lueMandant.Properties.DropDownRows = 10
			lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMandant.Properties.SearchMode = SearchMode.AutoComplete
			lueMandant.Properties.AutoSearchColumnIndex = 0

			lueMandant.Properties.NullText = String.Empty
			lueMandant.EditValue = Nothing

		End Sub

		Private Sub ResetTransactionDataDropDown()
			lueTransactionType.Properties.DisplayMember = "Description"

			lueTransactionType.Properties.Columns.Clear()
			lueTransactionType.Properties.Columns.Add(New LookUpColumnInfo("Description", 0))
			lueTransactionType.EditValue = Nothing
		End Sub

		Private Sub ResetBankDataDropDown()
			lueBankData.Properties.DisplayMember = "DisplayName"
			'lueBankData.Properties.ValueMember = "ID"

			lueBankData.Properties.Columns.Clear()
			lueBankData.Properties.BestFitMode = BestFitMode.None
			'lueBankData.Properties.Columns.Add(New LookUpColumnInfo("ID", 20))
			lueBankData.Properties.Columns.Add(New LookUpColumnInfo("DisplayName", lueBankData.Width - 40))

			lueBankData.EditValue = Nothing
		End Sub

		Private Sub ResetBankChargesDropDown()
			lueBankCharges.Properties.DisplayMember = "Description"

			lueBankCharges.Properties.Columns.Clear()
			lueBankCharges.Properties.Columns.Add(New LookUpColumnInfo("Description", 0))
			lueBankCharges.EditValue = Nothing
		End Sub

		Private Sub ResetSelectionListGrid()
			' Selection Editor.
			If m_selectionColumnEdit Is Nothing Then
				m_selectionColumnEdit = New RepositoryItemCheckEdit()
				m_selectionColumnEdit.EditValueChangedFiringMode = EditValueChangedFiringMode.Default

				grdSelectionList.RepositoryItems.Clear()
				grdSelectionList.RepositoryItems.Add(m_selectionColumnEdit)
			End If

			' Gridview Options
			gvSelectionList.OptionsView.ShowIndicator = False
			gvSelectionList.OptionsView.ShowAutoFilterRow = True
			gvSelectionList.OptionsView.ColumnAutoWidth = False
			gvSelectionList.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvSelectionList.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll
			gvSelectionList.OptionsView.ShowFooter = True
			gvSelectionList.OptionsBehavior.Editable = True
			gvSelectionList.OptionsBehavior.AutoUpdateTotalSummary = True
			gvSelectionList.OptionsBehavior.SummariesIgnoreNullValues = True

			gvSelectionList.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True


		End Sub


#End Region  ' Reset Form


#Region "Load Data"

		Private Function LoadMandantDropDownData() As Boolean
			Dim success As Boolean = True

			m_mandantDataList = m_CommonDatabaseAccess.LoadCompaniesListData() 'LoadMandantAllowedListData()
			If (m_mandantDataList Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				success = False
			End If

			lueMandant.Properties.DataSource = m_mandantDataList
			lueMandant.Properties.DropDownRows = If(m_mandantDataList Is Nothing, 0, Math.Min(m_mandantDataList.Count, 20))
			lueMandant.Properties.ForceInitialize()

			Return success
		End Function

		Private Function LoadTransactionDataDropDownData(ByVal defaultValue As Integer?) As Boolean
			m_transactionDataList = TransactionData.GetList(m_Translate)

			'chkPain001.Checked = m_AllowedPain001
			chkPain001.Enabled = True ' m_AllowedPain001

			lueTransactionType.Properties.DataSource = m_transactionDataList
			lueTransactionType.Properties.DropDownRows = m_transactionDataList.Count

			lueTransactionType.EditValue = m_transactionDataList(defaultValue.GetValueOrDefault(0))
			lueTransactionType.Properties.ForceInitialize()

			Return True
		End Function

		Private Function LoadBankChargesDrowpDownData() As Boolean
			m_bankChargesList = New List(Of BankCharges)
			m_bankChargesList.Add(New BankCharges With {.Value = 0, .Description = m_Translate.GetSafeTranslationValue("(OUR) Alle Spesen zu Lasten Auftraggeber")})
			m_bankChargesList.Add(New BankCharges With {.Value = 1, .Description = m_Translate.GetSafeTranslationValue("(BEN) Alle Spesen zu Lasten Begünstigte")})
			m_bankChargesList.Add(New BankCharges With {.Value = 2, .Description = m_Translate.GetSafeTranslationValue("(SHA) Spesen-Teilung")})

			lueBankCharges.Properties.DataSource = m_bankChargesList
			lueBankCharges.Properties.DropDownRows = m_bankChargesList.Count

			Dim defaultbankchargeto As Integer = ParseToInteger(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr),
																																								 String.Format("{0}/defaultbankchargeto", Field_DefaultValues)), 1)
			If defaultbankchargeto > 2 Then defaultbankchargeto = 2
			If defaultbankchargeto < 0 Then defaultbankchargeto = 0

			lueBankCharges.EditValue = m_bankChargesList(defaultbankchargeto)
			lueBankCharges.Properties.ForceInitialize()

			Return True
		End Function

		Private Function LoadBankDataDropDownData() As Boolean
			Dim success As Boolean = True

			If lueMandant.EditValue IsNot Nothing Then
				m_bankDataList = m_DTAUtilityDatabaseAccess.LoadBankData(lueMandant.EditValue)
				Dim notAllowedRec = m_bankDataList.Where(Function(m) m.Swift = "" OrElse m.IBANNr = "").ToList()
				If Not notAllowedRec Is Nothing AndAlso notAllowedRec.Count > 0 Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Achtung: Es sind Mandanten-Bank Daten welche noch korrigiert werden müssen."),
																		 m_Translate.GetSafeTranslationValue("Fehlerhafte Bankdaten"), MessageBoxIcon.Hand)
				End If
				If Not m_bankDataList Is Nothing Then
					m_bankDataList = m_bankDataList.Where(Function(m) m.Swift <> "" And m.IBANNr <> "").ToList()
				End If
				success = Not (m_bankDataList Is Nothing OrElse m_bankDataList.Count = 0)
			End If

			lueBankData.EditValue = Nothing
			lueBankData.Properties.DataSource = m_bankDataList
			lueBankData.Properties.DropDownRows = If(m_bankDataList Is Nothing, 0, Math.Min(m_bankDataList.Count, 20))
			If m_bankDataList IsNot Nothing AndAlso m_bankDataList.Count > 0 Then
				lueBankData.EditValue = m_bankDataList(0)
			End If
			lueBankData.Properties.ForceInitialize()

			Return success
		End Function

		Private Sub LoadSelectionList()

			tgsSelection.EditValue = False
			If m_selectionListType.HasValue Then
				Select Case m_selectionListType
					Case frmDTA.SelectionListType.Payments
						Me.LoadSelectionListWithPayments()

					Case frmDTA.SelectionListType.PaymentsCreditor
						Me.LoadSelectionListWithPaymentsCreditor()

					Case frmDTA.SelectionListType.Jobs
						Me.LoadSelectionListWithJobs()
						Me.SetSelectionCount()

						Return

				End Select
			End If
			SelDeSelectItems(True)
			tgsSelection.EditValue = True
			'Me.SetSelectionCount()

		End Sub

		Private Sub LoadSelectionListWithPayments()
			Dim selectionList As IList(Of ISelectionItem) = Nothing
			Dim selectionBindingList As BindingList(Of SelectionItemPayment) = Nothing
			Dim noIBANDataCount As Integer = 0
			Dim emplyeeMessage As String = String.Empty

			DxErrorProvider1.ClearErrors()


			m_SelectedAmount = 0D

			If lueMandant.EditValue IsNot Nothing AndAlso m_transactionDataSelected IsNot Nothing Then
				Dim zgaType As SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.ZgTypeEnum?
				Select Case m_transactionDataSelected.Type
					Case TransactionData.TransactionType.CreateDtaFileSwiss
						zgaType = SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.ZgTypeEnum.CreateDtaFileSwiss

					Case TransactionData.TransactionType.CreateDtaFileForeign
						zgaType = SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.ZgTypeEnum.CreateDtaFileForeign

					Case TransactionData.TransactionType.CreatePaymentJob
						zgaType = SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.ZgTypeEnum.CreateVg


					Case Else
						zgaType = Nothing
				End Select

				If zgaType IsNot Nothing Then
					Dim zgaDataList = m_DTAUtilityDatabaseAccess.LoadZGADataForDTAFileCreation(zgaType, lueMandant.EditValue)
					Dim allowedAdding As Boolean = True
					If Not zgaDataList Is Nothing Then
						selectionList = New List(Of ISelectionItem)
						selectionBindingList = New BindingList(Of SelectionItemPayment)
						For Each zgaData In zgaDataList
							Dim selectionItem = New SelectionItemPayment(zgaData)
							If chkPain001.Checked Then
								If String.IsNullOrWhiteSpace(zgaData.IBANNr) OrElse String.IsNullOrWhiteSpace(zgaData.Swift) Then
									emplyeeMessage &= String.Format("{0}Kandidatennummer: {1}, Auszahlungsnummer: {2}, Betrag: {3:n2} >>> IBAN: {4}, Swift: {5}", vbNewLine, zgaData.MANr, zgaData.ZGNr, zgaData.Betrag, zgaData.IBANNr, zgaData.Swift)
									allowedAdding = False
									noIBANDataCount += 1
								Else
									allowedAdding = True
								End If
							Else
								allowedAdding = True
							End If

							If allowedAdding Then
								selectionList.Add(selectionItem)
								selectionBindingList.Add(selectionItem)
							End If

						Next
					End If
				End If
			End If

			m_selectionList = selectionList
			m_selectionFormatString = m_selectionPaymentsFormatString
			grdSelectionList.DataSource = selectionBindingList

			If noIBANDataCount > 0 Then
				Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Fehlende IBAN-Nummer oder Swift-Angaben bei Bankverbindungen.{0}Anzahl ignorierte Datensätze: {1}{0}{0}{2}"), vbNewLine, noIBANDataCount, emplyeeMessage)
				m_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("Pain.001"), MessageBoxIcon.Warning)
			End If

		End Sub

		Private Sub LoadSelectionListWithPaymentsCreditor()
			Dim selectionList As IList(Of ISelectionItem) = Nothing
			Dim selectionBindingList As BindingList(Of SelectionItemPaymentCreditor) = Nothing
			Dim noIBANDataCount As Integer = 0
			Dim emplyeeMessage As String = String.Empty

			DxErrorProvider1.ClearErrors()


			m_SelectedAmount = 0D

			If lueMandant.EditValue IsNot Nothing AndAlso m_transactionDataSelected IsNot Nothing Then
				Select Case m_transactionDataSelected.Type
					Case TransactionData.TransactionType.CreateDtaFileCreditorSwiss
						Dim lolDataList = m_DTAUtilityDatabaseAccess.LoadLolDataForDtaFileCreation(lueMandant.EditValue)
						Dim allowedAdding As Boolean = True

						If Not lolDataList Is Nothing Then
							selectionList = New List(Of ISelectionItem)
							selectionBindingList = New BindingList(Of SelectionItemPaymentCreditor)
							For Each lolData In lolDataList
								Dim selectionItem = New SelectionItemPaymentCreditor(lolData)

								If chkPain001.Checked Then
									If String.IsNullOrWhiteSpace(lolData.IBANNr) OrElse String.IsNullOrWhiteSpace(lolData.Swift) Then
										emplyeeMessage &= String.Format("{0}Kandidatennummer: {1}, Lohnartennummer: {2:F5}, Betrag: {3:n2} >>> IBAN: {4}, Swift: {5}", vbNewLine, lolData.MANr, lolData.LANr, lolData.Betrag, lolData.IBANNr, lolData.Swift)
										allowedAdding = False
										noIBANDataCount += 1
									Else
										allowedAdding = True
									End If
								Else
									allowedAdding = True
								End If

								If allowedAdding Then
									selectionList.Add(selectionItem)
									selectionBindingList.Add(selectionItem)
								End If

							Next
						End If
				End Select
			End If

			m_selectionList = selectionList
			m_selectionFormatString = m_selectionPaymentsCreditorFormatString
			grdSelectionList.DataSource = selectionBindingList

			If noIBANDataCount > 0 Then
				Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Fehlende IBAN-Nummer oder Swift-Angaben bei Bankverbindungen.{0}Anzahl ignorierte Datensätze: {1}{0}{0}{2}"), vbNewLine, noIBANDataCount, emplyeeMessage)
				m_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("Pain.001"), MessageBoxIcon.Warning)
			End If

		End Sub

		Private Sub LoadSelectionListWithJobs()
			Dim selectionList As IList(Of ISelectionItem) = Nothing
			Dim selectionBindingList As BindingList(Of SelectionItemJob) = Nothing

			m_SelectedAmount = 0D

			If lueMandant.EditValue IsNot Nothing AndAlso m_transactionDataSelected IsNot Nothing Then

				Dim dtaJobType As SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.DtaJobTypeEnum?
				Select Case m_transactionDataSelected.Type
					Case TransactionData.TransactionType.PrintJob, TransactionData.TransactionType.DeleteJob
						dtaJobType = SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.DtaJobTypeEnum.Dta

					Case TransactionData.TransactionType.PrintCreditorJob, TransactionData.TransactionType.DeleteCreditorJob
						dtaJobType = SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess.DtaJobTypeEnum.DtaLol

					Case Else
						dtaJobType = Nothing

				End Select

				If dtaJobType IsNot Nothing Then
					Dim jobDataList = m_DTAUtilityDatabaseAccess.LoadJobNumberData(dtaJobType.Value, lueMandant.EditValue)
					If Not jobDataList Is Nothing Then
						selectionList = New List(Of ISelectionItem)
						selectionBindingList = New BindingList(Of SelectionItemJob)
						For Each lolData In jobDataList
							Dim selectionItem = New SelectionItemJob(lolData)
							selectionList.Add(selectionItem)
							selectionBindingList.Add(selectionItem)
						Next
					End If
				End If
			End If

			m_selectionList = selectionList
			m_selectionFormatString = m_selectionJobsFormatString
			grdSelectionList.DataSource = selectionBindingList
		End Sub

#End Region  ' Load Data


#Region "Set Data"

		Private Sub SetMandant(mdNr As Integer)
			Dim mandantData As MandantData = m_mandantDataList.Where(Function(md) md.MandantNumber = mdNr).FirstOrDefault()
			lueMandant.EditValue = mandantData.MandantNumber
		End Sub

		Private Sub SetTransactionDefault()
			If m_transactionDataList IsNot Nothing AndAlso m_transactionDataList.Count > 0 Then
				lueTransactionType.EditValue = m_transactionDataList(0)
			End If
		End Sub

		Private Sub SetSelectionList(ByRef selectionListType As SelectionListType?)
			Dim selectionListTypeChanged As Boolean = False
			If Not selectionListType.HasValue Then
				If (m_selectionListType.HasValue) Then
					selectionListTypeChanged = True
				End If
			Else
				If (Not m_selectionListType.HasValue OrElse m_selectionListType.Value <> selectionListType.Value) Then
					selectionListTypeChanged = True
				End If
			End If
			If selectionListTypeChanged Then
				grdSelectionList.SuspendLayout()

				' Alle Spalten neu definieren
				gvSelectionList.Columns.Clear()

				Dim fieldName As String

				fieldName = "IsSelected"
				Dim colIsSelected As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
				colIsSelected.Caption = m_Translate.GetSafeTranslationValue("Auswahl")
				colIsSelected.Name = "colIsSelected"
				colIsSelected.FieldName = fieldName
				colIsSelected.Visible = True
				colIsSelected.ColumnEdit = m_selectionColumnEdit
				colIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
				colIsSelected.OptionsColumn.ReadOnly = False
				colIsSelected.OptionsColumn.AllowEdit = True
				colIsSelected.Width = 60

				Select Case selectionListType
					Case frmDTA.SelectionListType.Payments,
						frmDTA.SelectionListType.PaymentsCreditor
						fieldName = "Candidate"
						Dim colCandidate As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colCandidate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colCandidate.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
						colCandidate.Name = "colCandidate"
						colCandidate.FieldName = fieldName
						colCandidate.OptionsColumn.ReadOnly = True
						colCandidate.OptionsColumn.AllowEdit = False
						colCandidate.Visible = True
						colCandidate.Width = 120

						fieldName = "Amount"
						Dim colAmount As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
						colAmount.Name = "colAmount"
						colAmount.FieldName = fieldName
						colAmount.OptionsColumn.ReadOnly = True
						colAmount.OptionsColumn.AllowEdit = False
						colAmount.Visible = True
						colAmount.Width = 80
						colAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
						colAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
						colAmount.DisplayFormat.FormatString = "n2"
						colAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
						colAmount.SummaryItem.DisplayFormat = "{0:n2}"

				End Select

				Select Case selectionListType
					Case frmDTA.SelectionListType.Payments
						fieldName = "Bank"
						Dim colBank As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colBank.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colBank.Caption = m_Translate.GetSafeTranslationValue("Bank")
						colBank.Name = "colBank"
						colBank.FieldName = fieldName
						colBank.OptionsColumn.ReadOnly = True
						colBank.OptionsColumn.AllowEdit = False
						colBank.Visible = True
						colBank.Width = 120

						fieldName = "LedgerNo"
						Dim colLedgerNo As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colLedgerNo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colLedgerNo.Caption = m_Translate.GetSafeTranslationValue("Konto")
						colLedgerNo.Name = "colLedgerNo"
						colLedgerNo.FieldName = fieldName
						colLedgerNo.OptionsColumn.ReadOnly = True
						colLedgerNo.OptionsColumn.AllowEdit = False
						colLedgerNo.Visible = False
						colLedgerNo.Width = 120

						fieldName = "IBANNr"
						Dim colIBANNr As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colIBANNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colIBANNr.Caption = m_Translate.GetSafeTranslationValue("IBAN")
						colIBANNr.Name = "colIBANNr"
						colIBANNr.FieldName = fieldName
						colIBANNr.OptionsColumn.ReadOnly = True
						colIBANNr.OptionsColumn.AllowEdit = False
						colIBANNr.Visible = True
						colIBANNr.Width = 100

						fieldName = "Swift"
						Dim colSwift As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colSwift.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
						colSwift.Name = "colSwift"
						colSwift.FieldName = fieldName
						colSwift.OptionsColumn.ReadOnly = True
						colSwift.OptionsColumn.AllowEdit = False
						colSwift.Visible = True
						colSwift.Width = 100

					Case frmDTA.SelectionListType.PaymentsCreditor
						fieldName = "Recipient"
						Dim colRecipient As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colRecipient.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colRecipient.Caption = m_Translate.GetSafeTranslationValue("Empfänger")
						colRecipient.Name = "colRecipient"
						colRecipient.FieldName = fieldName
						colRecipient.OptionsColumn.ReadOnly = True
						colRecipient.OptionsColumn.AllowEdit = False
						colRecipient.Visible = True
						colRecipient.Width = 120

						fieldName = "SalaryType"
						Dim colSalaryType As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colSalaryType.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colSalaryType.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
						colSalaryType.Name = "colSalaryType"
						colSalaryType.FieldName = fieldName
						colSalaryType.OptionsColumn.ReadOnly = True
						colSalaryType.OptionsColumn.AllowEdit = False
						colSalaryType.Visible = True
						colSalaryType.Width = 120

						fieldName = "SalaryNo"
						Dim colSalaryNo As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colSalaryNo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colSalaryNo.Caption = m_Translate.GetSafeTranslationValue("LoNr")
						colSalaryNo.Name = "colSalaryNo"
						colSalaryNo.FieldName = fieldName
						colSalaryNo.OptionsColumn.ReadOnly = True
						colSalaryNo.OptionsColumn.AllowEdit = False
						colSalaryNo.Visible = True
						colSalaryNo.Width = 120

				End Select

				Select Case selectionListType
					Case frmDTA.SelectionListType.Payments,
						frmDTA.SelectionListType.PaymentsCreditor
						fieldName = "PaymentReason"
						Dim colPaymentReason As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colPaymentReason.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colPaymentReason.Caption = m_Translate.GetSafeTranslationValue("Zahlungsgrund")
						colPaymentReason.Name = "colPaymentReason"
						colPaymentReason.FieldName = fieldName
						colPaymentReason.OptionsColumn.ReadOnly = True
						colPaymentReason.OptionsColumn.AllowEdit = False
						colPaymentReason.Visible = True
						colPaymentReason.Width = 120

						fieldName = "PaymentPeriod"
						Dim colPaymentPeriod As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colPaymentPeriod.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colPaymentPeriod.Caption = m_Translate.GetSafeTranslationValue("Zeitraum")
						colPaymentPeriod.Name = "colPaymentPeriod"
						colPaymentPeriod.FieldName = fieldName
						colPaymentPeriod.OptionsColumn.ReadOnly = True
						colPaymentPeriod.OptionsColumn.AllowEdit = False
						colPaymentPeriod.Visible = True
						colPaymentPeriod.Width = 60

					Case frmDTA.SelectionListType.Jobs
						fieldName = "JobNo"
						Dim colJobNo As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colJobNo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colJobNo.Caption = m_Translate.GetSafeTranslationValue("Auftrag")
						colJobNo.Name = "colJobNo"
						colJobNo.FieldName = fieldName
						colJobNo.OptionsColumn.ReadOnly = True
						colJobNo.OptionsColumn.AllowEdit = False
						colJobNo.Visible = True
						colJobNo.Width = 80

						fieldName = "JobDate"
						Dim colJobDate As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colJobDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colJobDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
						colJobDate.Name = "colJobDate"
						colJobDate.FieldName = fieldName
						colJobDate.OptionsColumn.ReadOnly = True
						colJobDate.OptionsColumn.AllowEdit = False
						colJobDate.Visible = True
						colJobDate.Width = 100

						fieldName = "AmountTotal"
						Dim colAmountTotal As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colAmountTotal.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colAmountTotal.Caption = m_Translate.GetSafeTranslationValue("Totalbetrag")
						colAmountTotal.Name = "colAmountTotal"
						colAmountTotal.FieldName = fieldName
						colAmountTotal.OptionsColumn.ReadOnly = True
						colAmountTotal.OptionsColumn.AllowEdit = False
						colAmountTotal.Visible = True
						colAmountTotal.Width = 80
						colAmountTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
						colAmountTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
						colAmountTotal.DisplayFormat.FormatString = "n2"
						colAmountTotal.SummaryItem.DisplayFormat = "{0:n2}"
						colAmountTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum

						fieldName = "PaymentCount"
						Dim colPaymentCount As DevExpress.XtraGrid.Columns.GridColumn = gvSelectionList.Columns.AddField(fieldName)
						colPaymentCount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
						colPaymentCount.Caption = m_Translate.GetSafeTranslationValue("Anzahl Zahlungen")
						colPaymentCount.Name = "colPaymentCount"
						colPaymentCount.FieldName = fieldName
						colPaymentCount.OptionsColumn.ReadOnly = True
						colPaymentCount.OptionsColumn.AllowEdit = False
						colPaymentCount.Visible = True
						colPaymentCount.Width = 120
						colPaymentCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
						colPaymentCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
						colPaymentCount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum

				End Select

				m_selectionListType = selectionListType
				grdSelectionList.ResumeLayout()
			End If

		End Sub

		Private Sub SetSelectionCount()
			'Dim selectionList As IList(Of ISelectionItem) = Nothing
			'Dim selectionBindingList As BindingList(Of SelectionItemPaymentCreditor) = Nothing

			If m_transactionDataSelected Is Nothing Then Return
			m_SelectedAmount = 0D

			If m_selectionList IsNot Nothing Then

				Dim totalCount As Integer = m_selectionList.Count
				Dim selectionCount As Integer = (
					From si In m_selectionList
					Where si.IsSelected
					Select si.IsSelected
					).Count()

				Select Case m_transactionDataSelected.Type
					Case TransactionData.TransactionType.CreateDtaFileCreditorSwiss
						For Each si In m_selectionList
							If si.IsSelected Then
								Dim Selecteddata As SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation = si.DataObject
								m_SelectedAmount += Math.Abs(Selecteddata.Betrag.GetValueOrDefault(0))

							End If

						Next

					Case TransactionData.TransactionType.CreateDtaFileSwiss, TransactionData.TransactionType.CreateDtaFileForeign, TransactionData.TransactionType.CreatePaymentJob
						For Each si In m_selectionList
							If si.IsSelected Then
								Dim Selecteddata As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData = si.DataObject
								m_SelectedAmount += Math.Abs(Selecteddata.Betrag.GetValueOrDefault(0))

							End If

						Next

					Case TransactionData.TransactionType.DeleteJob, TransactionData.TransactionType.DeleteCreditorJob, TransactionData.TransactionType.PrintCreditorJob, TransactionData.TransactionType.PrintJob
						For Each si In m_selectionList
							If si.IsSelected Then
								Dim Selecteddata As SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData = si.DataObject
								m_SelectedAmount += Math.Abs(Selecteddata.TotalBetrag.GetValueOrDefault(0))

							End If

						Next

				End Select

				bsiSelectedEntriesAmount.Caption = Format(m_SelectedAmount, "n2")
				bsiAllEntriesCount.Caption = totalCount
				bsiSelectedEntriesCount.Caption = selectionCount

				bbiAction.Enabled = m_AllowedCreate AndAlso selectionCount > 0

			End If

		End Sub

		Private Sub SetActionType(ByRef actionType As ActionType?)
			Dim actionTypeChanged As Boolean = False
			If Not actionType.HasValue Then
				If (m_actionType.HasValue) Then
					actionTypeChanged = True
				End If
			Else
				If (Not m_actionType.HasValue OrElse m_actionType.Value <> actionType.Value) Then
					actionTypeChanged = True
				End If
			End If
			If actionTypeChanged Then
				Select Case actionType
					Case frmDTA.ActionType.Create
						bbiAction.Caption = m_Translate.GetSafeTranslationValue("Erstellen")
						bbiAction.Glyph = My.Resources.exportfile_16x16

					Case frmDTA.ActionType.Print
						bbiAction.Caption = m_Translate.GetSafeTranslationValue("Drucken")
						bbiAction.Glyph = My.Resources.print_16x16

					Case frmDTA.ActionType.Delete
						bbiAction.Caption = m_Translate.GetSafeTranslationValue("Löschen")
						bbiAction.Glyph = My.Resources.deletelist2_16x16

				End Select
				m_actionType = actionType
			End If
		End Sub

#End Region  ' Set Data


#Region "Event Handlers"

		Private Sub OnLueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

			If lueMandant.EditValue IsNot Nothing Then

				If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
					Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

					Dim clsMandant = m_mandant.GetSelectedMDData(lueMandant.EditValue)
					Dim logedUserData = m_mandant.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
					Dim personalizedData = m_InitializationData.ProsonalizedData
					Dim translate = m_InitializationData.TranslationData

					m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

					'm_AllowedPain001 = MandantAllowedPain001
				End If
				'chkPain001.Checked = m_AllowedPain001
				'chkPain001.Enabled = m_AllowedPain001

				Me.CreateDatabaseAccessObjects(lueMandant.EditValue)
				Me.LoadBankDataDropDownData()
				Me.LoadSelectionList()
			End If

		End Sub

		Private Sub OnLueTransactionType_EditValueChanged(sender As Object, e As EventArgs) Handles lueTransactionType.EditValueChanged
			m_transactionDataSelected = TryCast(lueTransactionType.EditValue, TransactionData)

			Dim usesBankData = TransactionData.UsesBankData(m_transactionDataSelected)
			Dim usesBankCharges = TransactionData.UsesBankCharges(m_transactionDataSelected)
			Dim usesSalaryOptions = TransactionData.UsesSalaryOptions(m_transactionDataSelected)
			Dim usesFileSelection = TransactionData.UsesFileSelection(m_transactionDataSelected)

			' UI Elemente abhängig von der gewählten Übertragungsart verbergen
			lblTransactionDate.Visible = usesBankData
			dteTransactionDate.Visible = usesBankData
			lblBankData.Visible = usesBankData
			lueBankData.Visible = usesBankData
			chkPain001.Visible = usesFileSelection
			chkMarkAsSalary.Visible = usesSalaryOptions
			lblBankCharges.Visible = usesBankCharges
			lueBankCharges.Visible = usesBankCharges
			lblFilePath.Visible = usesFileSelection
			txtFilePath.Visible = usesFileSelection

			' Auswahl Grid abhängig von der gewählten Übertragungsart füllen
			Dim selectionListType = TransactionData.GetSelectionListType(m_transactionDataSelected)
			Me.SetSelectionList(selectionListType)
			Me.LoadSelectionList()

			' Aktionen abhängig von der gewählten Übertragungsart setzen
			Dim actionType = TransactionData.GetActionType(m_transactionDataSelected)
			Me.SetActionType(actionType)
		End Sub

		Private Sub OnLueBankData_EditValueChanged(sender As Object, e As EventArgs) Handles lueBankData.EditValueChanged
			m_bankDataSelected = TryCast(lueBankData.EditValue, SP.DatabaseAccess.DTAUtility.DataObjects.BankData)
		End Sub

		Private Sub OnLueBankCharges_EditValueChanged(sender As Object, e As EventArgs) Handles lueBankCharges.EditValueChanged
			m_bankChargesSelected = TryCast(lueBankCharges.EditValue, BankCharges)
		End Sub

		Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtFilePath.ButtonClick
			Dim dlg As New FolderBrowserDialog With {.Description = m_Translate.GetSafeTranslationValue("Wählen Sie das Verzeichnis für DTA-Datei"),
																							 .ShowNewFolderButton = True, .SelectedPath = If(String.IsNullOrWhiteSpace(txtFilePath.EditValue),
																																																System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
																																																System.IO.Path.GetDirectoryName(txtFilePath.EditValue))}
			If dlg.ShowDialog() = DialogResult.OK Then
				txtFilePath.EditValue = dlg.SelectedPath
				txtFilePath.SelectionStart = Len(txtFilePath.EditValue)
			End If

		End Sub

		Private Sub OnGvSelectionList_CellValueChanging(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gvSelectionList.CellValueChanging
			' Hack: So wird eine Änderung in der Checkbox sofort an die Datenquelle zurückgegeben
			gvSelectionList.SetFocusedRowCellValue(gvSelectionList.FocusedColumn, e.Value)
			Me.SetSelectionCount()
		End Sub

		Private Sub OnBtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Me.Close()
		End Sub

		Private Sub OnbbiAction_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiAction.ItemClick
			Dim printJobKey As String = PRINTJOB_DTA
			Dim showCreatedXMLFolder As Boolean = False

			If Not ValidateData() Then Return

			bbiAction.Enabled = False
			If lueMandant.EditValue IsNot Nothing AndAlso m_transactionDataSelected IsNot Nothing Then
				' Pfad prüfen falls benötigt
				If TransactionData.UsesFileSelection(m_transactionDataSelected) Then
					Dim isFilePathValid As Boolean
					Try
						Dim fileInfo = New System.IO.FileInfo(txtFilePath.Text)
						isFilePathValid = System.IO.Directory.Exists(fileInfo.DirectoryName)
						isFilePathValid = isFilePathValid AndAlso m_Utility.IsDirectoryAccessible(fileInfo.DirectoryName)
						isFilePathValid = True

					Catch ex As Exception
						isFilePathValid = False
					End Try

					If Not isFilePathValid Then
						m_UtilityUI.ShowInfoDialog(Me, String.Format(m_Translate.GetSafeTranslationValue("Der Pfad '{0}' ist ungültig."), txtFilePath.Text))
						Return

					End If
					showCreatedXMLFolder = isFilePathValid
				End If

				' Selektion überprüfen
				Dim idArray As Integer() = Nothing
				If m_selectionList IsNot Nothing Then
					idArray = (
							From item In m_selectionList
							Where item.IsSelected
							Select item.Id
							).ToArray()
				End If
				If idArray Is Nothing OrElse idArray.Count = 0 Then
					' Keine Selektierten Ids: der Button müsste deaktiviert sein
					m_UtilityUI.ShowErrorDialog(Me, m_Translate.GetSafeTranslationValue("Es ist kein Eintrag selektiert."))
					Return
				End If

				' Gewählte Aktionen ausführen
				Dim success As Boolean = False
				Dim newDTANr As Integer = 0
				Dim newDTANrpuffer As Integer() = Nothing
				Dim message As String = String.Empty

				Dim dtaDate As DateTime? = dteTransactionDate.EditValue
				Dim fileName As String = txtFilePath.Text

				Select Case m_transactionDataSelected.Type
					Case TransactionData.TransactionType.CreateDtaFileSwiss,
							 TransactionData.TransactionType.CreateDtaFileForeign,
							 TransactionData.TransactionType.CreateDtaFileCreditorSwiss,
							 TransactionData.TransactionType.CreatePaymentJob
						' DTA File erstellen
						Dim zgDataListSelected As IList(Of ZgData) = Nothing
						Dim lolDataListSelected_New As New BindingList(Of ZgData) '= Nothing
						Dim lolDataListSelected As IList(Of LolDataForDtaFileCreation) = Nothing

						If m_selectionList IsNot Nothing Then
							zgDataListSelected = (
									From item In m_selectionList
									Where item.IsSelected And TypeOf item.DataObject Is SP.DatabaseAccess.DTAUtility.DataObjects.ZgData
									Select DirectCast(item.DataObject, SP.DatabaseAccess.DTAUtility.DataObjects.ZgData)).ToList()

							lolDataListSelected = (From item In m_selectionList
																		 Where item.IsSelected And TypeOf item.DataObject Is SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation
																		 Select DirectCast(item.DataObject, SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation)).ToList()
							For Each itm In lolDataListSelected
								Dim newItm = New ZgData
								newItm.Bank = itm.BANK
								newItm.BankOrt = itm.BankOrt
								newItm.Betrag = itm.Betrag
								newItm.DTAAdr1 = itm.DTAADR1
								newItm.DTAAdr2 = itm.DTAADR2
								newItm.DTAAdr3 = itm.DTAADR3
								newItm.DTAAdr4 = itm.DTAADR4
								newItm.IBANNr = itm.IBANNr
								newItm.Jahr = itm.Jahr
								newItm.KontoNr = itm.KONTONR
								newItm.LONr = itm.LONr
								newItm.LP = itm.LP
								newItm.MANr = itm.MANr
								newItm.Nachname = itm.Nachname
								newItm.EmployeeCountry = itm.EmployeeCountry
								newItm.Swift = itm.Swift
								newItm.Vorname = itm.Vorname
								If String.IsNullOrWhiteSpace(itm.ZGGrund) Then newItm.ZGGrund = itm.RPText Else newItm.ZGGrund = itm.ZGGrund
								newItm.ZGNr = itm.LOLID

								lolDataListSelected_New.Add(newItm)
							Next

						End If

						Dim dtaFileGenerator As New DTAFileGenerator(m_DTAUtilityDatabaseAccess, m_InitializationData) ' m_Translate)
						If m_transactionDataSelected.Type = TransactionData.TransactionType.CreateDtaFileCreditorSwiss AndAlso chkPain001.Checked Then
							dtaFileGenerator.PaymentListData = lolDataListSelected_New
						Else
							dtaFileGenerator.PaymentListData = zgDataListSelected
						End If

						dtaFileGenerator.PaymentDate = dteTransactionDate.EditValue
						dtaFileGenerator.BankIDNumber = m_bankDataSelected.ID

						Select Case m_transactionDataSelected.Type
							Case TransactionData.TransactionType.CreateDtaFileSwiss
								If chkPain001.Checked Then
									dtaFileGenerator.ChrgBrENum = DTAFileGenerator.Charger.SHAR
									newDTANr = dtaFileGenerator.BuildXMLPain001FileSwiss(DateTime.Now.Year, chkMarkAsSalary.Checked, txtFilePath.Text)
								Else
									newDTANr = dtaFileGenerator.BuildNewDtaFileSwiss(zgDataListSelected, dteTransactionDate.EditValue, lueMandant.EditValue, DateTime.Now.Year, m_bankDataSelected.ID, chkMarkAsSalary.Checked, txtFilePath.Text)
								End If

							Case TransactionData.TransactionType.CreateDtaFileForeign
								If chkPain001.Checked Then
									dtaFileGenerator.ChrgBrENum = m_bankChargesSelected.Value
									newDTANr = dtaFileGenerator.BuildXMLPain001FileForeign(DateTime.Now.Year, chkMarkAsSalary.Checked, txtFilePath.Text)
								Else
									newDTANr = dtaFileGenerator.BuildNewDtaFileForeign(zgDataListSelected, dteTransactionDate.EditValue, lueMandant.EditValue, DateTime.Now.Year, m_bankDataSelected.ID, chkMarkAsSalary.Checked, m_bankChargesSelected.Value, txtFilePath.Text)
								End If

							Case TransactionData.TransactionType.CreateDtaFileCreditorSwiss
								If chkPain001.Checked Then
									newDTANr = dtaFileGenerator.BuildXMLPain001FileCreditorSwiss(DateTime.Now.Year, chkMarkAsSalary.Checked, txtFilePath.Text)
								Else
									newDTANr = dtaFileGenerator.BuildDtaFileCreditorSwiss(lolDataListSelected, dteTransactionDate.EditValue, lueMandant.EditValue, DateTime.Now.Year, m_bankDataSelected.ID, txtFilePath.Text)
								End If

							Case TransactionData.TransactionType.CreatePaymentJob
								newDTANr = dtaFileGenerator.BuildPaymentOrder(zgDataListSelected, dteTransactionDate.EditValue)

						End Select
						success = newDTANr > 0
						newDTANrpuffer = New Integer() {newDTANr}

						If success Then
							If m_transactionDataSelected.Type <> TransactionData.TransactionType.CreatePaymentJob Then
								txtFilePath.EditValue = dtaFileGenerator.CreatedPaymentFile
								fileName = txtFilePath.Text
							End If

							bbiAction.Enabled = True
								message = m_Translate.GetSafeTranslationValue("Die DTA Datei wurde erfolgreich erstellt.")
								If showCreatedXMLFolder Then
									Dim test As Integer
									Try
										test = Shell(String.Format("explorer /select, {0}", txtFilePath.Text), AppWinStyle.NormalFocus)
									Catch ex As Exception
										m_Logger.LogError(ex.ToString)
									End Try
								End If

							Else
								message = m_Translate.GetSafeTranslationValue("Beim Erstellen der DTA Datei ist ein Fehler aufgetreten.")
						End If


					Case TransactionData.TransactionType.DeleteJob, TransactionData.TransactionType.DeleteCreditorJob
						' Aufträge löschen
						message = m_Translate.GetSafeTranslationValue("Wollen Sie die augewählten Aufträge wirklich löschen?")
						Dim result = m_UtilityUI.ShowYesNoDialog(Me, message, m_Translate.GetSafeTranslationValue("Aufträge löschen"))

						If result Then
							Select Case m_transactionDataSelected.Type
								Case TransactionData.TransactionType.DeleteJob
									success = m_DTAUtilityDatabaseAccess.SetZgOrderDeleted(idArray)
								Case TransactionData.TransactionType.DeleteCreditorJob
									success = m_DTAUtilityDatabaseAccess.SetLolOrderDeleted(idArray)

							End Select
							If success Then
								message = m_Translate.GetSafeTranslationValue("Die ausgewählten Aufträge wurden gelöscht.") & Environment.NewLine _
										& m_Translate.GetSafeTranslationValue("Alle Auszahlungen werden auf die nächste Liste kommen.")

							Else
								message = m_Translate.GetSafeTranslationValue("Beim Löschen der Aufträge ist ein Fehler aufgetreten.")
							End If
						Else
							Return
						End If

					Case TransactionData.TransactionType.PrintJob, TransactionData.TransactionType.PrintCreditorJob
						' Aufträge drucken
						Dim result = True
						fileName = String.Empty

						Dim selectedRows = GetSelectedDataForPrint()
						If Not selectedRows Is Nothing Then
							If selectedRows.Count > 1 Then
								dtaDate = Nothing
							Else
								dtaDate = selectedRows(0).JobDate
							End If
						End If

						If result Then
							newDTANrpuffer = idArray
							success = True

						Else
							Return
						End If

					Case Else
						' Nicht unterstützt
						m_UtilityUI.ShowErrorDialog(Me, String.Format(m_Translate.GetSafeTranslationValue("Die Übertragungsart '{0}' ist nicht implementiert."), m_transactionDataSelected.Description))
						Return

				End Select

				' Erfolgsmeldung anzeigen
				If success Then
					m_UtilityUI.ShowInfoDialog(Me, message)

					Try
						If Not (m_transactionDataSelected.Type = TransactionData.TransactionType.DeleteCreditorJob OrElse m_transactionDataSelected.Type = TransactionData.TransactionType.DeleteJob) Then

							If m_transactionDataSelected.Type = TransactionData.TransactionType.CreateDtaFileCreditorSwiss OrElse m_transactionDataSelected.Type = TransactionData.TransactionType.PrintCreditorJob Then printJobKey = PRINTJOB_CREDITOR_SWISS
							If m_transactionDataSelected.Type = TransactionData.TransactionType.CreatePaymentJob Then printJobKey = PRINTJOB_VG

							PrintDTAList(newDTANrpuffer, fileName, dtaDate, printJobKey)

							If m_transactionDataSelected.Type <> TransactionData.TransactionType.PrintCreditorJob AndAlso m_transactionDataSelected.Type <> TransactionData.TransactionType.PrintJob Then
								lueTransactionType.EditValue = m_transactionDataList(CheckResetvalueAfterCreatedDTAjob)
							End If

						End If

					Catch ex As TypeLoadException
						m_Logger.LogError(ex.TypeName)
					Catch ex As Exception
						m_Logger.LogError(ex.ToString)
					End Try

				Else
					m_UtilityUI.ShowErrorDialog(Me, message)
				End If

				Me.LoadSelectionList()
			End If

		End Sub

		Private Function GetSelectedDataForPrint() As BindingList(Of SelectionItemJob)

			Dim result As BindingList(Of SelectionItemJob)

			gvSelectionList.FocusedColumn = gvSelectionList.VisibleColumns(1)
			grdSelectionList.RefreshDataSource()
			Dim printList As BindingList(Of SelectionItemJob) = grdSelectionList.DataSource
			Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

			result = New BindingList(Of SelectionItemJob)

			For Each receiver In sentList
				result.Add(receiver)
			Next


			Return result

		End Function

		Function ValidateData() As Boolean

			DxErrorProvider1.ClearErrors()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			Dim m_MDOk = lueMandant.EditValue Is Nothing
			isValid = isValid And SetErrorIfInvalid(lueMandant, DxErrorProvider1, lueMandant.EditValue Is Nothing OrElse lueMandant.EditValue = 0, errorText)
			Select Case m_transactionDataSelected.Type
				Case TransactionData.TransactionType.CreateDtaFileSwiss,
							 TransactionData.TransactionType.CreateDtaFileForeign,
							 TransactionData.TransactionType.CreateDtaFileCreditorSwiss,
							 TransactionData.TransactionType.CreatePaymentJob
					isValid = isValid And SetErrorIfInvalid(lueBankData, DxErrorProvider1, m_bankDataSelected Is Nothing, errorText)

				Case Else

			End Select


			Return isValid

		End Function

		Protected Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText)
				'Else
				'  errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		Protected Function SetDXWarningIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				'errorProvider.SetErrorType(control, ErrorType.Warning)
				errorProvider.SetError(control, errorText, ErrorType.Warning)
				'Else
				'  errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		Private Function IsAssignedDirectoryAccessible(ByVal pathName As String) As Boolean
			Dim result As Boolean = True

			Dim tmpFilename = Path.GetRandomFileName()
			Dim testFile As String = Path.Combine(pathName, tmpFilename)
			Try
				Dim fileExists As Boolean = File.Exists(testFile)
				Using sw As New StreamWriter(File.Open(testFile, FileMode.OpenOrCreate))
					sw.WriteLine(
							IIf(fileExists,
									"file was already created " & DateTime.Now,
									"file is now created"))
				End Using
				File.Delete(testFile)

			Catch ex As Exception
				Return False

			End Try

			Return result
		End Function

		Private Sub OntgsSelection_Click(sender As Object, e As EventArgs) Handles tgsSelection.Click

			SelDeSelectItems(Not tgsSelection.EditValue)
			'If Not tgsSelection.EditValue Then
			'	For Each item As ISelectionItem In m_selectionList
			'		item.IsSelected = True
			'	Next

			'Else
			'	For Each item As ISelectionItem In m_selectionList
			'		item.IsSelected = False
			'		'gvSelectionList.RefreshData()
			'	Next
			'End If

			'gvSelectionList.RefreshData()
			'Me.SetSelectionCount()

		End Sub

		Private Sub SelDeSelectItems(ByVal selectAll As Boolean)

			If Not m_selectionList Is Nothing Then
				For Each item As ISelectionItem In m_selectionList
					item.IsSelected = selectAll
				Next
			End If


			'If Not tgsSelection.EditValue Then
			'	For Each item As ISelectionItem In m_selectionList
			'		item.IsSelected = True
			'	Next

			'Else
			'	For Each item As ISelectionItem In m_selectionList
			'		item.IsSelected = False
			'		'gvSelectionList.RefreshData()
			'	Next
			'End If

			gvSelectionList.RefreshData()
			Me.SetSelectionCount()

		End Sub

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnForm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged
			If Visible Then
				'LoadFormSettings()
			End If
		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
			CleanupAndHideForm()
			e.Cancel = True
		End Sub

		''' <summary>
		''' Keypreview for Modul-version
		''' </summary>
		Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
			If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
				Dim frm As New frmLibraryInfo(m_InitializationData)
				frm.LoadAssemblyData()

				frm.Show()
				frm.BringToFront()
			End If
		End Sub


		Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvSelectionList.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvSelectionList.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then

					Select Case m_transactionDataSelected.Type
						Case TransactionData.TransactionType.CreateDtaFileSwiss, TransactionData.TransactionType.CreateDtaFileForeign
							Dim viewDataZG = CType(dataRow, SelectionItemPayment).Data

							Select Case column.Name.ToLower
								Case "MANr".ToLower, "colRecipient".ToLower, "colCandidate".ToLower
									If viewDataZG.MANr.HasValue Then OpenSelectedEmployee(viewDataZG.MANr)

								Case Else
									If viewDataZG.ZGNr.HasValue Then OpenSelectedAdvancePayment(viewDataZG.ZGNr)

							End Select

						Case TransactionData.TransactionType.CreateDtaFileCreditorSwiss, TransactionData.TransactionType.CreatePaymentJob
							Dim viewDataLO = CType(dataRow, SelectionItemPaymentCreditor).Data

							Select Case column.Name.ToLower
								Case "MANr".ToLower, "colRecipient".ToLower
									If viewDataLO.MANr.HasValue Then OpenSelectedEmployee(viewDataLO.MANr)

								Case Else
									If viewDataLO.LONr.HasValue Then OpenSelectedSalaryPayment(viewDataLO.LONr, viewDataLO.MANr)

							End Select

						Case Else
							Return

					End Select

				End If

			End If

		End Sub

		Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs) Handles gvSelectionList.CustomDrawEmptyForeground
			Dim s As String = "Keine Daten sind vorhanden"

			Try
				s = m_Translate.GetSafeTranslationValue(s)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
			Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
			e.Graphics.DrawString(s, font, Brushes.Black, r)

		End Sub

		Private Sub chkPain001_CheckedChanged(sender As Object, e As EventArgs) Handles chkPain001.CheckedChanged
			'LoadSelectionListWithPayments()
			LoadSelectionList()
		End Sub

#End Region  ' Event Handlers


#Region "Helper Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()
			Me.lblHeader.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader.Text)
			Me.lblHeaderDescription.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderDescription.Text)
			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
			Me.lblUerberweisungsArt.Text = m_Translate.GetSafeTranslationValue(Me.lblUerberweisungsArt.Text)
			Me.lblTransactionDate.Text = m_Translate.GetSafeTranslationValue(Me.lblTransactionDate.Text)
			Me.lblBankData.Text = m_Translate.GetSafeTranslationValue(Me.lblBankData.Text)
			Me.lblFilePath.Text = m_Translate.GetSafeTranslationValue(Me.lblFilePath.Text)
			Me.lblBankCharges.Text = m_Translate.GetSafeTranslationValue(Me.lblBankCharges.Text)
			Me.lblSelectionList.Text = m_Translate.GetSafeTranslationValue(Me.lblSelectionList.Text)

			Me.bsiLblAllEntries.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblAllEntries.Caption)
			Me.bsiLblSelectedEntries.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblSelectedEntries.Caption)

			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.bbiAction.Caption = m_Translate.GetSafeTranslationValue(Me.bbiAction.Caption)
			Me.tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(Me.tgsSelection.Properties.OffText)
			Me.tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(Me.tgsSelection.Properties.OnText)

			m_selectionPaymentsFormatString = m_Translate.GetSafeTranslationValue("Zahlungen zur Überweisung ({0} von {1})")
			m_selectionPaymentsCreditorFormatString = m_Translate.GetSafeTranslationValue("Kreditoren Zahlungen zur Überweisung ({0} von {1})")
			m_selectionJobsFormatString = m_Translate.GetSafeTranslationValue("Aufträge ({0} von {1})")
		End Sub


		''' <summary>
		''' Creates database access objects by mandant number.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		Private Sub CreateDatabaseAccessObjects(mdNr As Integer?)
			If mdNr.HasValue Then
				Dim conStr = m_mandant.GetSelectedMDData(mdNr.Value).MDDbConn
				If m_CurrentConnectionString <> conStr Then
					m_CurrentConnectionString = conStr
					m_DTAUtilityDatabaseAccess = New SP.DatabaseAccess.DTAUtility.DTAUtilityDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
					m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
				End If
			End If
		End Sub

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

				If Not String.IsNullOrEmpty(setting_form_location) Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If
				chkPain001.Checked = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_DTA_PAIN001)
				If Date.Now >= New Date(2018, 7, 1) Then
					chkPain001.Checked = True
				End If
				Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
				m_Logger.LogInfo(String.Format("Local user config path: {0}", config.FilePath))

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
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
					m_SettingsManager.WriteBoolean(SettingKeys.SETTING_DTA_PAIN001, chkPain001.Checked)

					m_SettingsManager.WriteString(SettingKeys.SETTING_DTAFile_LOCATION, Me.txtFilePath.EditValue)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub


		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFromSettings()

			Me.Hide()
			'Me.Reset() 'Clear all data.

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

		Private Function PrintDTAList(ByVal dtaNumbers As Integer(), ByVal dtaFilename As String, ByVal dtaFileDate As DateTime?, ByVal printJobKey As String) As Boolean
			Dim success As Boolean = True
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
			Dim dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing) = m_DTAUtilityDatabaseAccess.LoadZGADataForDTAList(lueMandant.EditValue, dtaNumbers)

			Try
				Dim bankIDNumber As Integer?
				If Not m_bankDataSelected Is Nothing Then bankIDNumber = m_bankDataSelected.ID

				Dim _Setting As New ClsLLDTAPrintSetting With {.m_initData = m_InitializationData, .m_Translate = m_Translate, .m_dtaDataList = dtaDataList, .JobNr2Print = printJobKey,
																											 .ShowAsDesgin = ShowDesign,
																											 .frmhwnd = GetHwnd,
																											 .dtaNumber = dtaNumbers,
																											 .DTAfiledate = dtaFileDate,
																											 .DTAFileName = dtaFilename,
																											 .MandantBankIDNumber = bankIDNumber,
																											 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", String.Empty)})}

				Dim o2Open As New ZGSearchListing.ClsPrintZGSearchList(New ClsLLZGSearchPrintSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr, .SelectedMDYear = m_InitializationData.MDData.MDYear,
																	   .DbConnString2Open = m_InitializationData.MDData.MDDbConn, .SQL2Open = m_InitializationData.MDData.MDDbConn, .JobNr2Print = printJobKey, .frmhwnd = GetHwnd})
				o2Open.PrintDTAList(_Setting)

				success = True


			Catch ex As Exception
				Return False

			End Try

			Return success

		End Function


		Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, Employeenumber)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

		Sub OpenSelectedAdvancePayment(ByVal advancePaymentNumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenAdvancePaymentMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, advancePaymentNumber)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

		Sub OpenSelectedSalaryPayment(ByVal salaryPaymentNumber As Integer, ByVal Employeenumber As Integer)

			Try
				Dim liLONr As New List(Of Integer)
				Dim mdnr As Integer = m_InitializationData.MDData.MDNr

				Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.SelectedMANr = New List(Of Integer)(New Integer() {Employeenumber}),
																																		 .SelectedLONr = New List(Of Integer)(New Integer() {salaryPaymentNumber}),
																																		 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
																																		 .SelectedYear = New List(Of Integer)(New Integer() {0}),
																																		 .SearchAutomatic = True,
																																		 .SelectedMDNr = mdnr,
																																		 .LogedUSNr = m_InitializationData.UserData.UserNr}
				'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(m_InitializationData, _settring)
				'obj.ShowfrmLO4Details()

				Dim obj As New SP.LO.PrintUtility.frmLOPrint(m_InitializationData)
				obj.LOSetting = _settring

				obj.Show()
				obj.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub


#End Region  ' Helper Methods


#End Region  ' Private Methods


#Region "Private Helper Classes"

		Private Class TransactionData

			Public Enum TransactionType
				CreateDtaFileSwiss
				CreateDtaFileForeign
				CreateDtaFileCreditorSwiss
				CreatePaymentJob
				PrintJob
				PrintCreditorJob
				DeleteJob
				DeleteCreditorJob
			End Enum

			Public Shared Function GetList(translate As SP.Infrastructure.Initialization.TranslateValuesHelper) As IList(Of TransactionData)
				Dim list = New List(Of TransactionData)

				list.Add(New TransactionData With {
					.Type = TransactionType.CreateDtaFileSwiss,
					.Description = translate.GetSafeTranslationValue("DTA-Datei erstellen (Schweiz)")})
				list.Add(New TransactionData With {
					.Type = TransactionType.CreateDtaFileForeign,
					.Description = translate.GetSafeTranslationValue("DTA-Datei erstellen (Ausland)")})
				list.Add(New TransactionData With {
					.Type = TransactionType.CreateDtaFileCreditorSwiss,
					.Description = translate.GetSafeTranslationValue("DTA-Datei für Kreditoren erstellen (Schweiz)")})
				list.Add(New TransactionData With {
					.Type = TransactionType.CreatePaymentJob,
					.Description = translate.GetSafeTranslationValue("Vergütungsauftrag erstellen")})
				list.Add(New TransactionData With {
					.Type = TransactionType.PrintJob,
					.Description = translate.GetSafeTranslationValue("Einen ausgeführten Auftrag drucken (NUR Ausdruck)")})
				list.Add(New TransactionData With {
					.Type = TransactionType.PrintCreditorJob,
					.Description = translate.GetSafeTranslationValue("Einen ausgeführten Kreditoren-Auftrag drucken (NUR Ausdruck)")})
				list.Add(New TransactionData With {
					.Type = TransactionType.DeleteJob,
					.Description = translate.GetSafeTranslationValue("Einen bestehenden Auftrag löschen")})
				list.Add(New TransactionData With {
					.Type = TransactionType.DeleteCreditorJob,
					.Description = translate.GetSafeTranslationValue("Einen bestehenden Kreditoren-Auftrag löschen")})
				Return list
			End Function

			Public Shared Function UsesBankData(transactionData As TransactionData) As Boolean
				If transactionData IsNot Nothing Then
					Select Case transactionData.Type
						Case TransactionType.CreateDtaFileSwiss,
								 TransactionType.CreateDtaFileForeign,
							TransactionType.CreateDtaFileCreditorSwiss,
							TransactionType.CreatePaymentJob
							Return True
					End Select
				End If
				Return False
			End Function

			Public Shared Function UsesBankCharges(transactionData As TransactionData) As Boolean
				If transactionData IsNot Nothing Then
					Select Case transactionData.Type
						Case TransactionType.CreateDtaFileForeign
							Return True
					End Select
				End If
				Return False
			End Function

			Public Shared Function UsesSalaryOptions(transactionData As TransactionData) As Boolean
				If transactionData IsNot Nothing Then
					Select Case transactionData.Type
						Case TransactionType.CreateDtaFileSwiss,
								 TransactionType.CreateDtaFileForeign
							Return True
					End Select
				End If
				Return False
			End Function

			Public Shared Function UsesFileSelection(transactionData As TransactionData) As Boolean
				If transactionData IsNot Nothing Then
					Select Case transactionData.Type
						Case TransactionType.CreateDtaFileSwiss,
									TransactionType.CreateDtaFileForeign,
									TransactionType.CreateDtaFileCreditorSwiss
							Return True
					End Select
				End If
				Return False
			End Function

			Public Shared Function GetActionType(transactionData As TransactionData) As ActionType?
				If transactionData IsNot Nothing Then
					Select Case transactionData.Type
						Case TransactionType.CreateDtaFileSwiss,
							TransactionType.CreateDtaFileForeign,
							TransactionType.CreatePaymentJob,
							TransactionType.CreateDtaFileCreditorSwiss
							Return ActionType.Create

						Case TransactionType.PrintJob,
							TransactionType.PrintCreditorJob
							Return ActionType.Print

						Case TransactionType.DeleteJob,
							TransactionType.DeleteCreditorJob
							Return ActionType.Delete

					End Select
				End If
				Return Nothing
			End Function

			Public Shared Function GetSelectionListType(transactionData As TransactionData) As SelectionListType?
				If transactionData IsNot Nothing Then
					Select Case transactionData.Type
						Case TransactionType.CreateDtaFileSwiss,
							TransactionType.CreateDtaFileForeign,
							TransactionType.CreatePaymentJob

							Return SelectionListType.Payments

						Case TransactionType.CreateDtaFileCreditorSwiss
							Return SelectionListType.PaymentsCreditor

						Case TransactionType.PrintJob,
							TransactionType.PrintCreditorJob,
							TransactionType.DeleteJob,
							TransactionType.DeleteCreditorJob

							Return SelectionListType.Jobs

					End Select
				End If

				Return Nothing
			End Function

			Public Property Type As TransactionType
			Public Property Description As String

		End Class

		Private Class BankCharges

			Public Property Value As Integer
			Public Property Description As String

		End Class

		Private Interface ISelectionItem
			Property IsSelected As Boolean
			ReadOnly Property Id As Integer
			ReadOnly Property DataObject As Object
		End Interface

		''' <summary>
		''' Item Klasse für Zahlungsauswahl-Liste.
		''' </summary>
		''' <remarks></remarks>
		Private Class SelectionItemPayment
			Implements ISelectionItem

			Public Sub New(data As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData)
				m_data = data
			End Sub

			Public Property IsSelected As Boolean Implements ISelectionItem.IsSelected
				Get
					Return m_isSelected
				End Get
				Set(value As Boolean)
					m_isSelected = value
				End Set
			End Property

			Public ReadOnly Property Id As Integer Implements ISelectionItem.Id
				Get
					Return m_data.ZGNr.GetValueOrDefault(0)
				End Get
			End Property

			Public ReadOnly Property DataObject As Object Implements ISelectionItem.DataObject
				Get
					Return m_data
				End Get
			End Property

			Public ReadOnly Property Data As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData
				Get
					Return m_data
				End Get
			End Property

			Public ReadOnly Property Candidate As String
				Get
					Return m_data.DTAAdr1
				End Get
			End Property

			Public ReadOnly Property Amount As Decimal
				Get
					Return -m_data.Betrag
				End Get
			End Property

			Public ReadOnly Property Bank As String
				Get
					Return m_data.Bank
				End Get
			End Property

			Public ReadOnly Property LedgerNo As String
				Get
					Return m_data.KontoNr
				End Get
			End Property

			Public ReadOnly Property PaymentReason As String
				Get
					Return m_data.ZGGrund
				End Get
			End Property

			Public ReadOnly Property IBANNr As String
				Get
					Return m_data.IBANNr
				End Get
			End Property

			Public ReadOnly Property Swift As String
				Get
					Return m_data.Swift
				End Get
			End Property

			Public ReadOnly Property PaymentPeriod As String
				Get
					Return String.Format("{0:00} / {1}", m_data.LP.GetValueOrDefault(0), m_data.Jahr)
				End Get
			End Property

			Private m_data As SP.DatabaseAccess.DTAUtility.DataObjects.ZgData
			Private m_isSelected As Boolean

		End Class

		''' <summary>
		''' Item Klasse für Kreditoren Zahlungsauswahl-Liste.
		''' </summary>
		''' <remarks></remarks>
		Private Class SelectionItemPaymentCreditor
			Implements ISelectionItem

			Public Sub New(data As SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation)
				m_data = data
			End Sub

			Public Property IsSelected As Boolean Implements ISelectionItem.IsSelected
				Get
					Return m_isSelected
				End Get
				Set(value As Boolean)
					m_isSelected = value
				End Set
			End Property

			Public ReadOnly Property Id As Integer Implements ISelectionItem.Id
				Get
					Return m_data.LOLID
				End Get
			End Property

			Public ReadOnly Property DataObject As Object Implements ISelectionItem.DataObject
				Get
					Return m_data
				End Get
			End Property

			Public ReadOnly Property Data As SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation
				Get
					Return m_data
				End Get
			End Property

			Public ReadOnly Property Candidate As String
				Get
					Return m_data.Nachname.Trim() + ", " + m_data.Vorname.Trim()
				End Get
			End Property

			Public ReadOnly Property Amount As Decimal
				Get
					Return m_data.Betrag.GetValueOrDefault(0D)
				End Get
			End Property

			Public ReadOnly Property Recipient As String
				Get
					Return m_data.DTAADR1
				End Get
			End Property

			Public ReadOnly Property SalaryType As String
				Get
					Return m_data.RPText
				End Get
			End Property

			Public ReadOnly Property SalaryNo As String
				Get
					Return If(m_data.LONr.HasValue, m_data.LONr.Value.ToString(), String.Empty)
				End Get
			End Property

			Public ReadOnly Property PaymentReason As String
				Get
					Return m_data.ZGGrund
				End Get
			End Property

			Public ReadOnly Property IBANNr As String
				Get
					Return m_data.IBANNr
				End Get
			End Property

			Public ReadOnly Property Swift As String
				Get
					Return m_data.Swift
				End Get
			End Property

			Public ReadOnly Property PaymentPeriod As String
				Get
					Return String.Format("{0:00} / {1}", m_data.LP.GetValueOrDefault(0), m_data.Jahr)
				End Get
			End Property

			Private m_data As SP.DatabaseAccess.DTAUtility.DataObjects.LolDataForDtaFileCreation
			Private m_isSelected As Boolean

		End Class

		''' <summary>
		''' Item Klasse für Auftragsauswahl-Liste.
		''' </summary>
		''' <remarks></remarks>
		Private Class SelectionItemJob
			Implements ISelectionItem

			Public Sub New(data As SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData)
				m_data = data
			End Sub

			Public Property IsSelected As Boolean Implements ISelectionItem.IsSelected
				Get
					Return m_isSelected
				End Get
				Set(value As Boolean)
					m_isSelected = value
				End Set
			End Property

			Public ReadOnly Property Id As Integer Implements ISelectionItem.Id
				Get
					Return m_data.VGNr.GetValueOrDefault(0)
				End Get
			End Property

			Public ReadOnly Property DataObject As Object Implements ISelectionItem.DataObject
				Get
					Return m_data
				End Get
			End Property

			Public ReadOnly Property Data As SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData
				Get
					Return m_data
				End Get
			End Property

			Public ReadOnly Property JobNo As String
				Get
					Return If(m_data.VGNr.HasValue, String.Format("{0:000000}", m_data.VGNr.Value), String.Empty)
				End Get
			End Property

			Public ReadOnly Property JobDate As String
				Get
					Return If(m_data.DTADate.HasValue, String.Format("{0:dd.MM.yyyy}", m_data.DTADate.Value), String.Empty)
				End Get
			End Property

			Public ReadOnly Property AmountTotal As Decimal?
				Get
					Return -m_data.TotalBetrag
				End Get
			End Property

			Public ReadOnly Property PaymentCount As Integer?
				Get
					Return m_data.RecAnzahl
				End Get
			End Property

			Private m_data As SP.DatabaseAccess.DTAUtility.DataObjects.JobNumberData
			Private m_isSelected As Boolean

		End Class

#End Region  'Private Helper Classes

		Private Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
			Dim result As Integer
			If (Not Integer.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

	End Class

End Namespace
