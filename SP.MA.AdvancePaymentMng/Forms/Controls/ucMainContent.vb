
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Internal.Automations.BaseTable
Imports SP.Internal.Automations
Imports System.ComponentModel

Namespace UI

	Public Class ucMainContent

#Region "Private Fields"
		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The available advisors.
		''' </summary>
		Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

		''' <summary>
		''' The employee master data.
		''' </summary>
		Dim m_EmployeeMasterData As EmployeeMasterData

		''' <summary>
		''' The employee contact comm data.
		''' </summary>
		Dim m_EmployeeContactComData As EmployeeContactComm

		''' <summary>
		''' The employee LO setting data.
		''' </summary>
		Dim m_EmployeeLOSettingsData As SP.DatabaseAccess.Employee.DataObjects.Salary.EmployeeLOSettingsData

		Private m_BaseTableData As BaseTable.SPSBaseTables
		Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisor1.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditPayAt.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePaymentType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueReason.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets or sets the total value in the UI.
		''' </summary>
		Private Property TotaAmountInUI As Decimal
			Get
				Return Convert.ToDecimal(lblTotal.Tag)
			End Get
			Set(value As Decimal)
				lblTotal.Tag = value

				Dim currency = m_UCMediator.ZGData.Currency

				lblTotal.Text = String.Format("{0:N2} {1}", value, currency)
			End Set
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			m_EmployeeDataAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_BaseTableData = New SPSBaseTables(m_InitializationData)
			m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)

		End Sub

		''' <summary>
		''' Loads data of the active advance payment.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadDataOfActiveAdvancePayment() As Boolean

			Dim success As Boolean = True

			If m_UCMediator.ZGData Is Nothing Then
				Return False
			End If

			Dim previousState = SetSuppressUIEventsState(True)

			ErrorProvider.Clear()

			success = success AndAlso LoadDropDownData()

			success = success AndAlso LoadEmployeeData()

			success = success AndAlso FillPropertyGroupData()
			success = success AndAlso FillCommentGroupData()
			success = success AndAlso FillPayoutGroupData()
			success = success AndAlso FillReportVGAndLONumberGroupData()
			success = success AndAlso FillEmployeeGroupData()
			success = success AndAlso LoadAndFillBalanceGroupData()

			SetCurrencyLabels()

			SetSuppressUIEventsState(False)

			Return True
		End Function

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			ErrorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(dateEditPayAt, ErrorProvider, dateEditPayAt.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(luePaymentType, ErrorProvider, luePaymentType.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(lueReason, ErrorProvider, String.IsNullOrWhiteSpace(lueReason.EditValue), errorText)

			Return isValid
		End Function


		''' <summary>
		''' Merges the ZG master data.
		''' </summary>
		''' <param name="zgMasterData">The ZG master data object where the data gets filled into.</param>
		Public Overrides Sub MergeCustomerMasterData(ByVal zgMasterData As ZGMasterData)

			zgMasterData.GebAbzug = chkGebAbzug.Checked
			zgMasterData.Aus_Dat = dateEditPayAt.EditValue
			zgMasterData.LANR = Convert.ToInt32(luePaymentType.EditValue)
			zgMasterData.ZGGRUND = lueReason.EditValue

		End Sub

		''' <summary>
		''' Saves additional data.
		''' </summary>
		Public Overrides Function SaveAdditionalData() As Boolean

			Dim success As Boolean = True

			Dim zgData = m_UCMediator.ZGData

			Dim employee = m_EmployeeDataAccess.LoadEmployeeNoticesData(zgData.MANR)

			If employee Is Nothing Then
				Return False
			End If

			employee.Notice_AdvancedPayment = txtComment.Text

			success = success AndAlso m_EmployeeDataAccess.UpdateEmployeeNoticesData(employee)

			Return success
		End Function

		''' <summary>
		''' Sets readonly state of controls.
		''' </summary>
		''' <param name="isReadonly">Boolean flag indicating if the controls should be readonly.</param>
		Public Overrides Sub SetReadonlyStateOfControls(ByVal isReadonly)

			'txtCommenen is always readonly because it is just a comment txt.

			chkGebAbzug.Properties.ReadOnly = isReadonly
			'dateEditPayAt.Properties.ReadOnly = isReadonly
			'luePaymentType.Properties.ReadOnly = isReadonly
			lueReason.Properties.ReadOnly = isReadonly

			' The other controls are always readonly, so no need to set them here.
		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_Advisors = Nothing
			m_EmployeeMasterData = Nothing
			m_EmployeeContactComData = Nothing
			m_EmployeeLOSettingsData = Nothing

			lueMandant.Properties.ReadOnly = True
			lueAdvisor1.Properties.ReadOnly = True
			lueYear.Properties.ReadOnly = True
			lueMonth.Properties.ReadOnly = True
			chkGebAbzug.Properties.ReadOnly = False
			dateEditPayAt.Properties.ReadOnly = True
			luePaymentType.Properties.ReadOnly = True
			txtAmount.Properties.ReadOnly = True
			lueEmployee.Properties.ReadOnly = True
			lueReason.Properties.ReadOnly = False

			txtComment.Text = String.Empty
			txtComment.Properties.MaxLength = 4000

			chkGebAbzug.Checked = False
			dateEditPayAt.EditValue = Nothing
			txtAmount.EditValue = 0D

			lblVGNrValue.Text = String.Empty
			lblRPNrValue.Text = String.Empty
			lblLONrValue.Text = String.Empty

			lblVGNrValue.Visible = True
			lblRPNrValue.Visible = True
			lblLONrValue.Visible = True

			lblTotal.Text = String.Empty

			lblCurrency.Text = String.Empty

			ResetEmployeeDetailData()

			' ---Reset drop downs, grids and lists---

			ResetMandantDropDown()
			ResetAdvisorDropDown()
			ResetPaymentTypeDropDown()
			ResetYearDropDown()
			ResetMonthDropDown()
			ResetReasonDropDown()
			ResetEmployeeDropDown()

			ErrorProvider.Clear()
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.gpEigenschaften.Text)
			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
			Me.lblAdvisor.Text = m_Translate.GetSafeTranslationValue(Me.lblAdvisor.Text)

			Me.grpBemerkung.Text = m_Translate.GetSafeTranslationValue(Me.grpBemerkung.Text)

			Me.grpAuszahlung.Text = m_Translate.GetSafeTranslationValue(Me.grpAuszahlung.Text)
			Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
			Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
			Me.chkGebAbzug.Text = m_Translate.GetSafeTranslationValue(Me.chkGebAbzug.Text)
			Me.lblZahlungAm.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlungAm.Text)
			Me.lblZahlArt.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlArt.Text)
			Me.lblBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblBetrag.Text)
			Me.lblZahlungsgrund.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlungsgrund.Text)

			Me.grpAdvancePaymentData.Text = m_Translate.GetSafeTranslationValue(Me.grpAdvancePaymentData.Text)
			Me.lblVGNr.Text = m_Translate.GetSafeTranslationValue(Me.lblVGNr.Text)
			Me.lblRpNr.Text = m_Translate.GetSafeTranslationValue(Me.lblRpNr.Text)
			Me.lblLoNr.Text = m_Translate.GetSafeTranslationValue(Me.lblLoNr.Text)

			Me.grpCandidateData.Text = m_Translate.GetSafeTranslationValue(Me.grpCandidateData.Text)
			Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)
			Me.lblGebDatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGebDatum.Text)
			Me.lblAdresseKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresseKandidat.Text)
			Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)
			Me.lblMAStatus.Text = m_Translate.GetSafeTranslationValue(Me.lblMAStatus.Text)
			Me.lblQuellensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblQuellensteuer.Text)
			Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)

			Me.grpGuthaben.Text = m_Translate.GetSafeTranslationValue(Me.grpGuthaben.Text)
			Me.lblGesamtauszahlung.Text = m_Translate.GetSafeTranslationValue(Me.lblGesamtauszahlung.Text)

		End Sub

#Region "Reset"

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

		''' <summary>
		''' Resets the Advisor1 and Advisor2 drop down.
		''' </summary>
		Private Sub ResetAdvisorDropDown()

			'Advisor1
			lueAdvisor1.Properties.DisplayMember = "UserFullname"
			lueAdvisor1.Properties.ValueMember = "KST"

			lueAdvisor1.Properties.Columns.Clear()
			lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
			lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))

			lueAdvisor1.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the year drop down.
		''' </summary>
		Private Sub ResetYearDropDown()

			lueYear.Properties.DisplayMember = "Value"
			lueYear.Properties.ValueMember = "Value"
			lueYear.Properties.ShowHeader = False

			lueYear.Properties.Columns.Clear()
			lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

			lueYear.Properties.ShowFooter = False
			lueYear.Properties.DropDownRows = 10
			lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueYear.Properties.SearchMode = SearchMode.AutoComplete
			lueYear.Properties.AutoSearchColumnIndex = 0

			lueYear.Properties.NullText = String.Empty
			lueYear.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the month drop down.
		''' </summary>
		Private Sub ResetMonthDropDown()

			lueMonth.Properties.DisplayMember = "Value"
			lueMonth.Properties.ValueMember = "Value"
			lueMonth.Properties.ShowHeader = False

			lueMonth.Properties.Columns.Clear()
			lueMonth.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Value")})

			lueMonth.Properties.ShowFooter = False
			lueMonth.Properties.DropDownRows = 10
			lueMonth.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMonth.Properties.SearchMode = SearchMode.AutoComplete
			lueMonth.Properties.AutoSearchColumnIndex = 0

			lueMonth.Properties.NullText = String.Empty
			lueMonth.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the paymenttype drop down.
		''' </summary>
		Private Sub ResetPaymentTypeDropDown()

			luePaymentType.Properties.DisplayMember = "LAText"
			luePaymentType.Properties.ValueMember = "LANr"
			luePaymentType.Properties.ShowHeader = False

			luePaymentType.Properties.Columns.Clear()
			luePaymentType.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "LAText",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("LAText")})

			luePaymentType.Properties.ShowFooter = False
			luePaymentType.Properties.DropDownRows = 10
			luePaymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePaymentType.Properties.SearchMode = SearchMode.AutoComplete
			luePaymentType.Properties.AutoSearchColumnIndex = 0

			luePaymentType.Properties.NullText = String.Empty
			luePaymentType.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the reason info drop down data.
		''' </summary>
		Private Sub ResetReasonDropDown()

			lueReason.Properties.DisplayMember = "ReasonText"
			lueReason.Properties.ValueMember = "ReasonText"
			lueReason.Properties.TextEditStyle = TextEditStyles.Standard
			lueReason.Properties.ReadOnly = False
			lueReason.Properties.MaxLength = 50

			gvReason.OptionsView.ShowIndicator = False
			gvReason.OptionsView.ShowColumnHeaders = False
			gvReason.OptionsView.ShowFooter = False
			gvReason.OptionsView.ShowAutoFilterRow = True
			gvReason.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			gvReason.Columns.Clear()

			Dim columnAdditionalText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdditionalText.Caption = m_Translate.GetSafeTranslationValue("ReasonText")
			columnAdditionalText.Name = "ReasonText"
			columnAdditionalText.FieldName = "ReasonText"
			columnAdditionalText.Visible = True
			columnAdditionalText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvReason.Columns.Add(columnAdditionalText)

			lueReason.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueReason.Properties.NullText = String.Empty
			lueReason.EditValue = Nothing
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
		''' Resets employee detail data.
		''' </summary>
		Private Sub ResetEmployeeDetailData()

			lblBirthdateValue.Text = String.Empty
			lblEmployeeAddressValue.Text = String.Empty
			lblQualificationValue.Text = String.Empty
			lblMAStateValue.Text = String.Empty
			lblQuellensteuerValue.Text = String.Empty
			lblBewilligungValue.Text = String.Empty

			iconPermissionWarning.Visible = False

		End Sub

#End Region

#Region "Load data"


		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadDropDownData() As Boolean

			Dim success As Boolean = True

			If Not m_IsIntialControlDataLoaded Then

				success = success AndAlso LoadMandantDropDownData()
				success = success AndAlso LoadAdvisorDropDown()
				success = success AndAlso LoadPaymentTypeDropDownData()

				m_IsIntialControlDataLoaded = True
			End If

			success = success AndAlso LoadPaymentReasonInfoDropdownData()

			Return success

		End Function

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
		''' Loads the Advisor1 and Advisor2 drop down data.
		''' </summary>
		Private Function LoadAdvisorDropDown() As Boolean
			' Load data
			m_Advisors = m_CommonDatabaseAccess.LoadAllAdvisorsData()

			' Advisor1
			lueAdvisor1.EditValue = Nothing
			lueAdvisor1.Properties.DataSource = m_Advisors
			lueAdvisor1.Properties.ForceInitialize()

			Return Not m_Advisors Is Nothing
		End Function

		''' <summary>
		''' Loads payment type drop down data.
		''' </summary>
		Private Function LoadPaymentTypeDropDownData() As Boolean

			Dim laData = m_AdvancePaymentDbAcccess.LoadLAData()

			If (laData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlarten konnten nicht geladen werden."))
			End If

			luePaymentType.Properties.DataSource = laData
			luePaymentType.Properties.ForceInitialize()

			Return laData IsNot Nothing
		End Function

		''' <summary>
		''' Loads payment reason info drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadPaymentReasonInfoDropdownData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			Dim paymentReasonTexts = m_AdvancePaymentDbAcccess.LoadPaymentReasonTexts(zgData.MANR, zgData.MDNr)

			If (paymentReasonTexts Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlungsgrundtexte konnten nicht geladen werden."))
			End If

			lueReason.Properties.DataSource = paymentReasonTexts

			Return Not paymentReasonTexts Is Nothing

		End Function

		''' <summary>
		''' Loads the employee data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			m_EmployeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(zgData.MANR)
			m_EmployeeContactComData = m_EmployeeDataAccess.LoadEmployeeContactCommData(zgData.MANR)
			m_EmployeeLOSettingsData = m_EmployeeDataAccess.LoadEmployeeLOSettings(zgData.MANR)

			If m_EmployeeMasterData Is Nothing Or
				m_EmployeeContactComData Is Nothing Or
				m_EmployeeLOSettingsData Is Nothing Then
				ResetEmployeeDetailData()
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterdaten konnten nciht geladen werden."))

				Return False
			End If

			Return True
		End Function

		''' <summary>
		''' Fills the property group data. 
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function FillPropertyGroupData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			lueMandant.EditValue = zgData.MDNr
			lueAdvisor1.EditValue = zgData.DTAAdr1

			Return True
		End Function

		''' <summary>
		''' Fills the comment group data.
		''' </summary>
		''' <returns>Boolean flag indiciating success.</returns>
		Private Function FillCommentGroupData() As Boolean

			txtComment.Text = m_EmployeeMasterData.Notice_AdvancedPayment

			Return True

		End Function

		''' <summary>
		''' Fills the payout group data (Auszahlung).
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function FillPayoutGroupData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			' Year
			Dim yearList As New List(Of IntegerValueViewWrapper)
			yearList.Add(New IntegerValueViewWrapper With {.Value = zgData.JAHR})
			lueYear.Properties.DataSource = yearList
			lueYear.Properties.ForceInitialize()
			lueYear.EditValue = Convert.ToInt32(zgData.JAHR)

			' Month
			Dim monthList As New List(Of IntegerValueViewWrapper)
			monthList.Add(New IntegerValueViewWrapper With {.Value = zgData.LP})
			lueMonth.Properties.DataSource = monthList
			lueMonth.Properties.ForceInitialize()
			lueMonth.EditValue = Convert.ToInt32(zgData.LP)

			' Gebührenauszahlung
			chkGebAbzug.Checked = zgData.GebAbzug.HasValue AndAlso zgData.GebAbzug

			' Zahlung am
			dateEditPayAt.EditValue = zgData.Aus_Dat

			' Zahlungsart
			luePaymentType.EditValue = Convert.ToDecimal(zgData.LANR)
			dateEditPayAt.Visible = Convert.ToDecimal(zgData.LANR) <> 8920
			lblZahlungAm.Visible = Convert.ToDecimal(zgData.LANR) <> 8920

			' Betrag
			txtAmount.EditValue = zgData.Betrag

			' Zahlungsgrund
			SetPaymentReasonInfoText(zgData.ZGGRUND)

			Return True
		End Function

		''' <summary>
		''' Fills RP, VG and LO number data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function FillReportVGAndLONumberGroupData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			lblVGNrValue.Text = String.Format(m_Translate.GetSafeTranslationValue("{0} wurde überwiesen am: {1:G}"), zgData.VGNR, zgData.DTADate)
			lblRPNrValue.Text = zgData.RPNR
			lblLONrValue.Text = zgData.LONR

			lblVGNrValue.Visible = zgData.VGNR.HasValue AndAlso zgData.VGNR > 0
			lblRPNrValue.Visible = zgData.RPNR.HasValue AndAlso zgData.RPNR > 0
			lblLONrValue.Visible = zgData.LONR.HasValue AndAlso zgData.LONR > 0

			grpAdvancePaymentData.Text = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Vorschuss"), zgData.ZGNr)
			grpCandidateData.Text = String.Format("{0}: {1}", m_Translate.GetSafeTranslationValue("Kandidat"), zgData.MANR)

			Return True
		End Function

		''' <summary>
		''' Fills employee group data. 
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function FillEmployeeGroupData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			Dim employeeListData = New List(Of EmployeeData)

			Dim employeeData As New EmployeeData With {.EmployeeNumber = m_EmployeeMasterData.EmployeeNumber,
																								 .Firstname = m_EmployeeMasterData.Firstname,
																								 .LastName = m_EmployeeMasterData.Lastname,
																								 .Location = m_EmployeeMasterData.Location,
																								 .Postcode = m_EmployeeMasterData.Postcode}

			employeeListData.Add(employeeData)
			lueEmployee.Properties.DataSource = employeeListData
			lueEmployee.ForceInitialize()
			lueEmployee.EditValue = zgData.MANR

			DisplayEmployeeData(m_EmployeeMasterData, m_EmployeeContactComData)

			Return True
		End Function

		''' <summary>
		''' Loads and fills the balance group data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadAndFillBalanceGroupData() As Boolean

			Dim zgData = m_UCMediator.ZGData

			Dim values = m_AdvancePaymentDbAcccess.LoadGuthabenValuesForAdvancePayment(zgData.MDNr, zgData.MANR, zgData.LP, zgData.JAHR)

			If values Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Guthabenwerte konnte nicht geladen werden."))
				Return False
			End If

			TotaAmountInUI = values.Total

			Return True
		End Function

#End Region

#Region "Event handling"

		''' <summary>
		''' Handles click on RP number link.
		''' </summary>
		Private Sub OnLblRPNrValue_Click(sender As Object, e As EventArgs) Handles lblRPNrValue.Click

			Dim zgData = m_UCMediator.ZGData

			If Not zgData Is Nothing AndAlso zgData.RPNR > 0 Then

				Dim hub = MessageService.Instance.Hub
				Dim openreportMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, zgData.RPNR)
				hub.Publish(openreportMng)
			End If

		End Sub

		''' <summary>
		''' Handles click on LO number link.
		''' </summary>
		Private Sub OnLblLONrValue_OpenLink(sender As Object, e As OpenLinkEventArgs) Handles lblLONrValue.OpenLink

			Dim zgData = m_UCMediator.ZGData

			If Not zgData Is Nothing AndAlso zgData.LONR > 0 Then
				Dim allowedUserNr As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554, zgData.MDNr)

				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 553, zgData.MDNr) And Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554, zgData.MDNr) Then m_Logger.LogWarning("No rights...") : Exit Sub

				Try
					Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.SelectedMDNr = zgData.MDNr,
																																		 .SelectedMANr = New List(Of Integer)(New Integer() {zgData.MANR}),
																																		 .SelectedLONr = New List(Of Integer)(New Integer() {zgData.LONR}),
																																		 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
																																		 .SelectedYear = New List(Of Integer)(New Integer() {0}),
																																		 .SearchAutomatic = True}
					'Dim obj As New SP.LO.PrintUtility.ClsMain_Net(m_InitializationData, _settring)
					'obj.ShowfrmLO4Details()
					Dim obj As New SP.LO.PrintUtility.frmLOPrint(m_InitializationData)
					obj.LOSetting = _settring

					obj.Show()
					obj.BringToFront()


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.Message))

				End Try

			End If

		End Sub

		''' <summary>
		''' Handles button click on employee.
		''' </summary>
		Private Sub OnLueEmployee_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueEmployee.ButtonClick

			Dim zgData = m_UCMediator.ZGData

			If zgData Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then

				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, zgData.MANR)
				hub.Publish(openEmployeeMng)

			End If

		End Sub

		''' <summary>
		''' Handles new value event on reason (ZGGrund) lookup edit.
		''' </summary>
		Private Sub OnLueReason_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueReason.ProcessNewValue

			If Not lueReason.Properties.DataSource Is Nothing Then

				Dim reasonInfoTexts = CType(lueReason.Properties.DataSource, List(Of PaymentReasonData))

				Dim newReasonlInfoText As New PaymentReasonData With {.ReasonText = e.DisplayValue.ToString()}
				reasonInfoTexts.Add(newReasonlInfoText)

				e.Handled = True
			End If
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

#End Region

#Region "Helper methods"

		''' <summary>
		''' Displays employee detail data.
		''' </summary>
		''' <param name="employeeMasterData">The employee master data.</param>
		''' <param name="employeeContactCommData">The employee contact comm data.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function DisplayEmployeeData(ByVal employeeMasterData As EmployeeMasterData, ByVal employeeContactCommData As EmployeeContactComm) As Boolean

			' Birthdate and age
			If employeeMasterData.Birthdate.HasValue Then
				lblBirthdateValue.Text = String.Format("{0:dd.MM.yyyy} ({1})", employeeMasterData.Birthdate.Value, GetAge(employeeMasterData.Birthdate.Value))
			Else
				lblBirthdateValue.Text = "-"
			End If

			' Address
			lblEmployeeAddressValue.Text = String.Format("{0}, {1} {2}", employeeMasterData.Street, employeeMasterData.Postcode, employeeMasterData.Location)

			' Qualification
			lblQualificationValue.Text = employeeMasterData.Profession

			' MA State
			lblMAStateValue.Text = If(String.IsNullOrWhiteSpace(employeeContactCommData.KStat1), "-", employeeContactCommData.KStat1)

			' Quellensteuer
			lblQuellensteuerValue.Text = If(String.IsNullOrWhiteSpace(employeeMasterData.Q_Steuer) OrElse employeeMasterData.Q_Steuer = "0",
											m_Translate.GetSafeTranslationValue("Nein"), m_Translate.GetSafeTranslationValue("Ja"))

			' Bewilligung
			Dim employeePermissionCode = employeeMasterData.Permission
			If Not String.IsNullOrWhiteSpace(employeePermissionCode) AndAlso Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
				Dim bewData = m_PermissionData.Where(Function(x) x.Code = employeePermissionCode).FirstOrDefault()
				If Not bewData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewData.Translated_Value) Then employeePermissionCode = String.Format("{0} - {1}", bewData.Code, bewData.Translated_Value)
			End If
			lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", employeePermissionCode, employeeMasterData.PermissionToDate)

			'lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", m_CommonDatabaseAccess.TranslatePermissionCode(employeeMasterData.Permission, m_InitializationData.UserData.UserLanguage), employeeMasterData.PermissionToDate)

			' Bewilligung warn icon
			iconPermissionWarning.Visible = employeeMasterData.PermissionToDate.HasValue AndAlso
									  (employeeMasterData.PermissionToDate.Value.Date < DateTime.Now.Date) AndAlso
									  Not String.IsNullOrWhiteSpace(employeeMasterData.Permission)

			Return True
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

		''' <summary>
		''' Sets the payment reason info text.
		''' </summary>
		''' <param name="reasonInfoText">The reason info text.</param>
		Private Sub SetPaymentReasonInfoText(ByVal reasonInfoText As String)

			If Not String.IsNullOrWhiteSpace(reasonInfoText) And Not lueReason.Properties.DataSource Is Nothing Then
				Dim paymentReasonTexts = CType(lueReason.Properties.DataSource, List(Of PaymentReasonData))

				If Not paymentReasonTexts.Any(Function(data) data.ReasonText = reasonInfoText) Then
					Dim newReasonInfoText As New PaymentReasonData With {.ReasonText = reasonInfoText}
					paymentReasonTexts.Add(newReasonInfoText)
				End If

			End If

			lueReason.EditValue = reasonInfoText
		End Sub

		''' <summary>
		''' Sets the currency labels.
		''' </summary>
		Private Sub SetCurrencyLabels()

			Dim currency = m_EmployeeLOSettingsData.Currency

			currency = If(String.IsNullOrWhiteSpace(currency), "CHF", currency)

			lblCurrency.Text = currency

		End Sub

#End Region

#End Region

#Region "View helper classes"

		''' <summary>
		''' Wraps an integer value.
		''' </summary>
		Class IntegerValueViewWrapper

			Public Property Value As Integer

		End Class

#End Region

	End Class

End Namespace
