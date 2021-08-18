
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports SPProgUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.MA.MLohnMng

Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
Imports SP.MA.BankMng
Imports DevExpress.XtraEditors.Repository

Namespace UI

	Public Class ucPageSelectAmountOfPayment

#Region "Private Consts"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
#End Region

#Region "Private Fields"
		Private m_Mandant As Mandant
		Private m_ValidMonth As List(Of Integer)

		Private m_path As ClsProgPath

		''' <summary>
		''' The monthly salary management detail form.
		''' </summary>
		Private m_MonthlySalaryMagementDetailForm As frmMSalary

		Private Property lohnpfaedung8720Amount As Decimal
		Private Property negativ8100Amount As Decimal
		Private Property isAmountexceeded As Boolean

		Private m_BankData As IEnumerable(Of EmployeeBankData)

		Private m_CheckEditDedfaultBank As RepositoryItemCheckEdit

		''' <summary>
		''' The bank detail form.
		''' </summary>
		Private m_BankDetailForm As frmBank

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			m_Mandant = New Mandant
			m_path = New ClsProgPath

			' Important symbol.
			m_CheckEditDedfaultBank = New RepositoryItemCheckEdit()	' CType(lueBank.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditDedfaultBank.PictureChecked = My.Resources.Checked
			m_CheckEditDedfaultBank.PictureUnchecked = Nothing
			m_CheckEditDedfaultBank.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined


			AddHandler lueYear.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueMonth.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditPayAt.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePaymentType.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueReason.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler lueBank.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Private Proberties"

		''' <summary>
		''' Gets the selected bank.
		''' </summary>
		''' <returns>The selected bank or nothing.</returns>
		Public ReadOnly Property SelectedBank As EmployeeBankData
			Get

				If lueBank.EditValue Is Nothing Or m_BankData Is Nothing Then
					Return Nothing
				End If

				Dim bankData = m_BankData.Where(Function(data) data.ID = lueBank.EditValue).FirstOrDefault()
				Return bankData
			End Get
		End Property


		''' <summary>
		''' warning counter if amount is exceeded
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property CountOfWarning As Integer

		''' <summary>
		''' Gets or sets the report hours value in the UI.
		''' </summary>
		Private Property ReportHoursInUI As Decimal
			Get
				Return txtReportHours.EditValue
			End Get
			Set(value As Decimal)
				txtReportHours.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the report hours  precentvalue in the UI.
		''' </summary>
		Private Property ReportHoursPercentInUI As Decimal
			Get
				Return Convert.ToDecimal(lblZGPercentHours.Tag)
			End Get
			Set(value As Decimal)
				lblZGPercentHours.Tag = value
				lblZGPercentHours.Text = String.Format("{0:N2}", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets the report hours sresult value in the UI.
		''' </summary>
		Private Property ReportHoursResultInUI As Decimal
			Get
				Return txtReportHoursResult.EditValue
			End Get
			Set(value As Decimal)
				txtReportHoursResult.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the report total value in the UI.
		''' </summary>
		Private Property ReportTotalInUI As Decimal
			Get
				Return txtReportTotal.EditValue
			End Get
			Set(value As Decimal)
				txtReportTotal.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the total amount of negativlohn result value in the UI.
		''' </summary>
		Private Property NegativLohnTotalResultInUI As Decimal
			Get
				Return txtTotalNegativlohn.EditValue
			End Get
			Set(value As Decimal)
				txtTotalNegativlohn.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the report total precent value in the UI.
		''' </summary>
		Private Property ReportTotalPercentInUI As Decimal
			Get
				Return Convert.ToDecimal(lblZGPercentTotal.Tag)
			End Get
			Set(value As Decimal)
				lblZGPercentTotal.Tag = value
				lblZGPercentTotal.Text = String.Format("{0:N2}", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets the report total  result value in the UI.
		''' </summary>
		Private Property ReportTotalResultInUI As Decimal
			Get
				Return txtReportTotalResult.EditValue
			End Get
			Set(value As Decimal)
				txtReportTotalResult.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the total value in the UI.
		''' </summary>
		Private Property TotaAmountInUI As Decimal
			Get
				Return Convert.ToDecimal(txtTotal.EditValue)
			End Get
			Set(value As Decimal)
				'txtTotal.Tag = value
				txtTotal.EditValue = String.Format("{0:N2}", value)
			End Set
		End Property

		''' <summary>
		''' Gets or sets the possible payout for report hours value in the UI.
		''' </summary>
		Private Property PossiblePayOutForReportHoursInUI As Decimal
			Get
				Return txtPossiblePayOutHours.EditValue
			End Get
			Set(value As Decimal)
				txtPossiblePayOutHours.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the possible payout for report total value in the UI.
		''' </summary>
		Private Property PossiblePayOutForReportTotalInUI As Decimal
			Get
				Return txtPossiblePayOutTotal.EditValue
			End Get
			Set(value As Decimal)
				txtPossiblePayOutTotal.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets the advance payment with fee setting.
		''' </summary>
		Private ReadOnly Property AdvancePaymentWithFeeSetting As Boolean
			Get

				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim advancepaymentwithfee As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																																String.Format("{0}/advancepaymentwithfee", FORM_XML_MAIN_KEY)), False)

				Return advancepaymentwithfee.HasValue AndAlso advancepaymentwithfee
			End Get
		End Property

		''' <summary>
		''' Gets the advance payment with fee setting.
		''' </summary>
		Private ReadOnly Property AdvancePaymentReasonSetting As String
			Get

				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim advancepaymentpaymentreason As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																					String.Format("{0}/advancepaymentpaymentreason", FORM_XML_MAIN_KEY))

				Return advancepaymentpaymentreason
			End Get
		End Property


		''' <summary>
		''' Gets the must payout date be set to today setting.
		''' </summary>
		Private ReadOnly Property MustPayoutDateBeSetToTodaySetting As Boolean
			Get
				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim mustPayoutDayBeSetToToday As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber), String.Format("{0}/setpayoutdatetotodayinadvancepayment", FORM_XML_MAIN_KEY)), True)

				Return mustPayoutDayBeSetToToday.HasValue AndAlso mustPayoutDayBeSetToToday
			End Get
		End Property

		''' <summary>
		''' Gets the advance payment default payment type setting.
		''' </summary>
		Private ReadOnly Property AdvancePaymentDefaultPaymentTypeSetting As String
			Get

				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim advancepaymentDefaultPaymentType As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																								String.Format("{0}/advancepaymentdefaultpaymenttype", FORM_XML_MAIN_KEY))

				Return advancepaymentDefaultPaymentType
			End Get
		End Property

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected payment data.
		''' </summary>
		''' <returns>Payment data.</returns>
		Public ReadOnly Property SelectedPaymentData As InitPaymentData
			Get
				If dateEditPayAt.EditValue Is Nothing Then dateEditPayAt.EditValue = DateTime.Now.Date
				Dim data As New InitPaymentData With {
					.Year = lueYear.EditValue,
					.Month = lueMonth.EditValue,
					.PaymentAtDate = dateEditPayAt.EditValue,
					.PaymentType = Convert.ToInt32(luePaymentType.EditValue),
					.PaymentTypeText = luePaymentType.Text,
					.AmountOfPayment = txtAmount.EditValue,
					.PaymentReason = lueReason.Text,
					.GebAbzug = chkGebAbzug.Checked,
					.AmountExceeded = isAmountexceeded,
					.BankData = SelectedBank
				}

				Return data
			End Get
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
		End Sub

		''' <summary>
		''' Activates the page.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function ActivatePage() As Boolean

			Dim success As Boolean = True

			If m_IsFirstPageActivation Then

				SetVisiblityOfGuthabenValuesLine2()
				SetCurrencyLabels()

				success = success AndAlso LoadDropDownData()

				ResetBankDropDown()
				LoadBankDropDownData()

				PreselectData()

				success = success AndAlso LoadEmployeeBalanceValues()

				ResetNegativeLMDataGrid()
				LoadNegativeLMData()

			End If

			' must be resetet!
			CountOfWarning = 0

			m_SuppressUIEvents = False
			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_IsFirstPageActivation = True
			m_ValidMonth = Nothing

			dateEditPayAt.EditValue = Nothing
			luePaymentType.EditValue = Nothing
			txtAmount.EditValue = 0D
			chkGebAbzug.Checked = False

			txtTotal.Properties.ReadOnly = True
			txtTotalNegativlohn.Properties.ReadOnly = True

			txtReportTotal.Properties.ReadOnly = True
			txtReportHours.Properties.ReadOnly = True
			txtReportHoursResult.Properties.ReadOnly = True
			txtReportTotalResult.Properties.ReadOnly = True

			txtPossiblePayOutHours.Properties.ReadOnly = True
			txtPossiblePayOutTotal.Properties.ReadOnly = True

			ResetGuthabenValues()

			'  Reset drop downs and lists

			ResetYearDropDown()
			ResetMonthDropDown()
			ResetPaymentTypeDropDown()
			ResetReasonDropDown()

			ResetNegativeLMDataGrid()

			ErrorProvider.Clear()
			DxErrorProvider1.ClearErrors()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			Dim FORM_XML_REQUIREDFIEKDS_KEY As String = "UserProfile/advancepayment"

			ErrorProvider.Clear()
			DxErrorProvider1.ClearErrors()

			Dim bWarned As Boolean = False
			Dim mandantNumber As Integer = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
			Dim employeeNumber As Integer = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData.EmployeeNumber
			Dim jahr As Integer = m_UCMediator.SelectedPaymentData.Year
			Dim lp As Integer = m_UCMediator.SelectedPaymentData.Month
			Dim PaymentType As Integer = luePaymentType.EditValue

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorInvalidAmountText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen gültigen Betrag ein.")
			Dim errorNotAllowedToPayMore As String = m_Translate.GetSafeTranslationValue("Maximaler Auszahlungsbetrag ist überschritten.")
			Dim errorNotAllowedToPayMore8900 As String = m_Translate.GetSafeTranslationValue("Maximaler Auszahlungsbetrag für Check-Vorschuss ({0:n2} {1}) ist überschritten.")
			Dim errorNotAllowedToPayMore8930 As String = m_Translate.GetSafeTranslationValue("Maximaler Auszahlungsbetrag für Bar-Vorschuss ({0:n2} {1}) ist überschritten.")
			Dim errorTextPfaendung As String = m_Translate.GetSafeTranslationValue("Der Pfändungsbetrag von {1:c2} darf nicht überschritten werden!{0}Mögliche Auszahlung gemäss Lohnart 8720: {2:c2}")
			Dim errorNotAllowedToPayMoreLohnpfaednung As String = m_Translate.GetSafeTranslationValue("Es ist eine Lohnpfänung in Höhe von ({1:n2} {2}) wurde eingegeben:{0}({3:n2} - ({4:n2} + {5:n2})")
			Dim errorTextNoZGBecauseOfNoES As String = m_Translate.GetSafeTranslationValue("Der Kandidat hatte keinen Einsatz diesen Monat.")
			Dim errorTextNoZGBecauseOfNoActivES As String = m_Translate.GetSafeTranslationValue("Der Kandidat hat keinen aktiven Einsatz.")
			Dim errorTextReportsWithoutLO As String = m_Translate.GetSafeTranslationValue("Es sind Rapporte welche abgerechnet werden sollen. Es sind Lohnabrechnungen welche gemacht werden sollen.{0}")

			Dim MaxAmountAllowedToPay As Decimal = 0
			Dim lohnpfaendungAmount As Decimal = 0
			Dim employeeAmountAllowedToPayMore As Decimal = 0

			Dim isValid As Boolean = True

			isValid = isValid AndAlso SetErrorIfInvalid(lueYear, ErrorProvider, lueYear.EditValue Is Nothing, errorText)
			isValid = isValid AndAlso SetErrorIfInvalid(lueMonth, ErrorProvider, lueMonth.EditValue Is Nothing, errorText)
			If isValid AndAlso Convert.ToDecimal(luePaymentType.EditValue) <> 8920 Then isValid = isValid AndAlso SetErrorIfInvalid(dateEditPayAt, ErrorProvider, dateEditPayAt.EditValue Is Nothing, errorText)
			isValid = isValid AndAlso SetErrorIfInvalid(luePaymentType, ErrorProvider, luePaymentType.EditValue Is Nothing, errorText)
			isValid = isValid AndAlso SetErrorIfInvalid(txtAmount, ErrorProvider, txtAmount.EditValue <= 0, errorInvalidAmountText)
			isValid = isValid AndAlso SetErrorIfInvalid(lueReason, ErrorProvider, String.IsNullOrWhiteSpace(lueReason.EditValue), errorText)
			isValid = isValid AndAlso SetErrorIfInvalid(lueBank, ErrorProvider, PaymentType = 8920 AndAlso lueBank.EditValue Is Nothing, errorText)

			If luePaymentType.EditValue = 8900 Then
				MaxAmountAllowedToPay = m_Utility.ParseToDec(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDUserProfileXMLFilename(mandantNumber, m_InitializationData.UserData.UserNr),
																						String.Format("{0}/maxvalue8900", FORM_XML_REQUIREDFIEKDS_KEY)), False)
				isValid = isValid And SetErrorIfInvalid(txtAmount, ErrorProvider,
																								txtAmount.EditValue > 0 AndAlso MaxAmountAllowedToPay > 0 AndAlso txtAmount.EditValue > MaxAmountAllowedToPay,
																								String.Format(errorNotAllowedToPayMore8900, MaxAmountAllowedToPay, lblCurrency.Text))

			ElseIf luePaymentType.EditValue = 8930 Then
				MaxAmountAllowedToPay = m_Utility.ParseToDec(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDUserProfileXMLFilename(mandantNumber, m_InitializationData.UserData.UserNr),
																						String.Format("{0}/maxvalue8930", FORM_XML_REQUIREDFIEKDS_KEY)), False)
				isValid = isValid And SetErrorIfInvalid(txtAmount, ErrorProvider,
																								txtAmount.EditValue > 0 AndAlso MaxAmountAllowedToPay > 0 AndAlso txtAmount.EditValue > MaxAmountAllowedToPay,
																								String.Format(errorNotAllowedToPayMore8930, MaxAmountAllowedToPay, lblCurrency.Text))
			End If

			Dim isUserAllowedToPayMore = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 356, mandantNumber)

			Dim employeeLOSettingData = m_UCMediator.EmployeeDbAccess.LoadEmployeeLOSettings(employeeNumber)
			If Not employeeLOSettingData Is Nothing AndAlso employeeLOSettingData.Max_NegativSalary > 0 Then

				employeeAmountAllowedToPayMore = employeeLOSettingData.Max_NegativSalary
				isValid = isValid And SetErrorIfInvalid(txtAmount, ErrorProvider, txtAmount.EditValue > 0 AndAlso
																																	(PossiblePayOutForReportHoursInUI + employeeAmountAllowedToPayMore) - (txtAmount.EditValue) < 0 AndAlso
																																	Not isUserAllowedToPayMore, errorNotAllowedToPayMore)
			Else
				isValid = isValid And SetErrorIfInvalid(txtAmount, ErrorProvider, txtAmount.EditValue > 0 AndAlso
																																					(txtAmount.EditValue) > PossiblePayOutForReportHoursInUI AndAlso
																																					Not isUserAllowedToPayMore, errorNotAllowedToPayMore)

			End If

			' message if 8720 exists!!!
			Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
			Dim negativeLMData = m_UCMediator.AdvancePaymentDbAccess.LoadNegativeLMData(employee.EmployeeNumber, lp, jahr)
			Dim lohnpfaednung = negativeLMData.Where(Function(data) data.LANr = 8720).FirstOrDefault()

			If Not lohnpfaednung Is Nothing Then
				If (Math.Abs(TotaAmountInUI) + txtAmount.EditValue) > lohnpfaedung8720Amount Then
					m_UtilityUI.ShowErrorDialog(String.Format(errorTextPfaendung, vbNewLine, lohnpfaedung8720Amount, lohnpfaedung8720Amount - Math.Abs(TotaAmountInUI)))
				End If
			End If

			If isValid AndAlso CountOfWarning = 0 AndAlso isUserAllowedToPayMore AndAlso (txtAmount.EditValue) > PossiblePayOutForReportHoursInUI Then
				isValid = isValid And SetDXWarningIfInvalid(txtAmount, DxErrorProvider1, True, errorNotAllowedToPayMore)
				bWarned = True
			End If
			isAmountexceeded = (txtAmount.EditValue) > m_Utility.SwissCommercialRound(PossiblePayOutForReportHoursInUI)


			' Check 358 UserRight (Make advance payment if there is no ES for the selected employee this month)
			If isValid Then
				Dim isUserAllowedToMakeAnAdvancePaymentIfThereWasNoESForTheSelectedEmployeeAtThisMonth = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 358, m_InitializationData.MDData.MDNr)

				Dim numberOfESOfEmployeeThisMonth = m_UCMediator.AdvancePaymentDbAccess.LoadNumberOfESOfEmployeeForMonth(lueYear.EditValue, lueMonth.EditValue, employeeNumber, mandantNumber)
				isValid = isValid And SetErrorIfInvalid(lueMonth, ErrorProvider, numberOfESOfEmployeeThisMonth.HasValue AndAlso numberOfESOfEmployeeThisMonth = 0 AndAlso Not isUserAllowedToMakeAnAdvancePaymentIfThereWasNoESForTheSelectedEmployeeAtThisMonth, errorTextNoZGBecauseOfNoES)

			End If

			' today active employement?
			Dim esdata = m_UCMediator.EmployeeDbAccess.LoadEmployeeESStateData(employeeNumber)
			If Not esdata Is Nothing Then
				Select Case esdata.EmployeeESStateResult
					Case DatabaseAccess.Employee.EmployeeESStateResult.State_Has_An_Active_ES

					Case Else
						If CountOfWarning = 0 Then
							isValid = isValid And SetDXWarningIfInvalid(lueMonth, DxErrorProvider1, True, errorTextNoZGBecauseOfNoActivES)

							bWarned = True
						End If

				End Select
			End If

			' there is reports from last months which have no LO?
			Dim rpdata = m_UCMediator.EmployeeDbAccess.LoadFoundedRPForEmployeeMng(employeeNumber)
			If Not rpdata Is Nothing Then
				Dim notCreatedPayroll = rpdata.Where(Function(data) data.lonr = 0 AndAlso (data.jahr <= lueYear.EditValue And data.monat < lueMonth.EditValue)).ToList() '.FirstOrDefault
				Dim msg As String
				If notCreatedPayroll.Count > 0 AndAlso CountOfWarning = 0 Then
					msg = String.Format("Folgende Rapporte sind noch nicht abgerechnet:{0}", vbNewLine)

					For Each rec In notCreatedPayroll
						msg &= String.Format(m_Translate.GetSafeTranslationValue("Rapport-Nr.: {0}{1}"), rec.rpnr, vbNewLine)
					Next
					isValid = isValid And SetDXWarningIfInvalid(lueMonth, DxErrorProvider1, Not notCreatedPayroll Is Nothing, String.Format(errorTextReportsWithoutLO, vbNewLine, msg))

					bWarned = True
				End If

			End If


			If bWarned Then CountOfWarning += 1

			Return isValid

		End Function


#End Region

#Region "Translation"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Group Auszahlung
			Me.grpAuszahlung.Text = m_Translate.GetSafeTranslationValue(Me.grpAuszahlung.Text)
			Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
			Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
			Me.lblZahlungAm.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlungAm.Text)
			Me.lblZahlArt.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlArt.Text)
			Me.lblBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblBetrag.Text)
			Me.lblZahlungsgrund.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlungsgrund.Text)
			Me.chkGebAbzug.Text = m_Translate.GetSafeTranslationValue(Me.chkGebAbzug.Text)
			Me.lblBank.Text = m_Translate.GetSafeTranslationValue(Me.lblBank.Text)

			' Group Guthaben
			Me.grpGuthaben.Text = m_Translate.GetSafeTranslationValue(Me.grpGuthaben.Text)
			Me.lblGesamtauszahlung.Text = m_Translate.GetSafeTranslationValue(Me.lblGesamtauszahlung.Text)
			Me.lblNegativeLohndaten.Text = String.Format("{0}: (LA 8100, LA 8720)", m_Translate.GetSafeTranslationValue(Me.lblNegativeLohndaten.Text))

			Me.lblRapportStunden.Text = m_Translate.GetSafeTranslationValue(Me.lblRapportStunden.Text)
			Me.lblRapporttotal.Text = m_Translate.GetSafeTranslationValue(Me.lblRapporttotal.Text)
			Me.lblMoeglicheAuszahlung1.Text = m_Translate.GetSafeTranslationValue(Me.lblMoeglicheAuszahlung1.Text)
			Me.lblMoeglicheAuszahlung2.Text = m_Translate.GetSafeTranslationValue(Me.lblMoeglicheAuszahlung2.Text)

			Me.lblNegativelohnliste.Text = m_Translate.GetSafeTranslationValue(Me.lblNegativelohnliste.Text)

		End Sub

#End Region

#Region "Reset"

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
			lueMonth.Properties.DropDownRows = 12
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
		''' Resets the Guthaben values.
		''' </summary>
		Private Sub ResetGuthabenValues()

			TotaAmountInUI = 0D

			lohnpfaedung8720Amount = 0D
			negativ8100Amount = 0D

			ReportHoursInUI = 0D
			ReportTotalInUI = 0D

			ReportHoursPercentInUI = 0D
			ReportTotalPercentInUI = 0D

			RecalculateValues()

		End Sub

		''' <summary>
		''' Resets the bank drop down data.
		''' </summary>
		Private Sub ResetBankDropDown()

			lueBank.Properties.DisplayMember = "Bank"
			lueBank.Properties.ValueMember = "ID"

			gvBank.OptionsView.ShowIndicator = False
			gvBank.OptionsView.ShowColumnHeaders = True
			gvBank.OptionsView.ShowFooter = False
			gvBank.OptionsView.ShowAutoFilterRow = True
			gvBank.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			gvBank.Columns.Clear()

			Dim zgBankColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			zgBankColumn.Caption = m_Translate.GetSafeTranslationValue("Vorschuss")
			zgBankColumn.Name = "BankZG"
			zgBankColumn.FieldName = "BankZG"
			zgBankColumn.Visible = True
			zgBankColumn.ColumnEdit = m_CheckEditDedfaultBank
			gvBank.Columns.Add(zgBankColumn)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
			activeColumn.Name = "ActiveRec"
			activeColumn.FieldName = "ActiveRec"
			activeColumn.Visible = True
			activeColumn.ColumnEdit = m_CheckEditDedfaultBank
			gvBank.Columns.Add(activeColumn)

			Dim BankAUColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			BankAUColumn.Caption = m_Translate.GetSafeTranslationValue("Ausland")
			BankAUColumn.Name = "BankAU"
			BankAUColumn.FieldName = "BankAU"
			BankAUColumn.Visible = True
			BankAUColumn.ColumnEdit = m_CheckEditDedfaultBank
			gvBank.Columns.Add(BankAUColumn)

			Dim columnAdditionalText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdditionalText.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnAdditionalText.Name = "Bank"
			columnAdditionalText.FieldName = "Bank"
			columnAdditionalText.Visible = True
			columnAdditionalText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnAdditionalText)

			Dim columnIban As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIban.Caption = m_Translate.GetSafeTranslationValue("IBAN")
			columnIban.Name = "IBANNr"
			columnIban.FieldName = "IBANNr"
			columnIban.Visible = True
			columnIban.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnIban)

			Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
			columnSwift.Name = "Swift"
			columnSwift.FieldName = "Swift"
			columnSwift.Visible = True
			columnSwift.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnSwift)


			lueBank.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueBank.Properties.NullText = String.Empty
			lueBank.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the negative LM data grid.
		''' </summary>
		Private Sub ResetNegativeLMDataGrid()

			' Reset the grid
			gvMSalary.OptionsView.ShowIndicator = False
			gvMSalary.OptionsView.ShowAutoFilterRow = False
			gvMSalary.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvMSalary.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.Width = 70
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			gvMSalary.Columns.Add(columnLANr)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnLAName.Name = "LAName"
			columnLAName.FieldName = "LAName"
			columnLAName.Visible = True
			columnLAName.Width = 70
			gvMSalary.Columns.Add(columnLAName)

			Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnAmount.Name = "M_Btr"
			columnAmount.FieldName = "M_Btr"
			columnAmount.Visible = True
			columnAmount.Width = 70
			columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAmount.DisplayFormat.FormatString = "N"
			columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAmount.AppearanceHeader.Options.UseTextOptions = True
			columnAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAmount.AppearanceCell.Options.UseTextOptions = True
			gvMSalary.Columns.Add(columnAmount)

			Dim columnMonatJahrVon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonatJahrVon.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnMonatJahrVon.Name = "MonatJahrVon"
			columnMonatJahrVon.FieldName = "MonatJahrVon"
			columnMonatJahrVon.Visible = True
			columnMonatJahrVon.Width = 70
			gvMSalary.Columns.Add(columnMonatJahrVon)

			Dim columnMonatJahrBis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonatJahrBis.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnMonatJahrBis.Name = "MonatJahrBis"
			columnMonatJahrBis.FieldName = "MonatJahrBis"
			columnMonatJahrBis.Visible = True
			columnMonatJahrBis.Width = 70
			gvMSalary.Columns.Add(columnMonatJahrBis)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			gvMSalary.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt von")
			columnCreatedFrom.Name = "CreatedFrom"
			columnCreatedFrom.FieldName = "CreatedFrom"
			columnCreatedFrom.Visible = True
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvMSalary.Columns.Add(columnCreatedFrom)

		End Sub


#End Region

#Region "Load Data"

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		Private Function LoadDropDownData() As Boolean

			Dim success As Boolean = True

			success = success And LoadMandantYearDropDownData()
			success = success And LoadPaymentTypeDropDownData()
			success = success And LoadPaymentReasonInfoDropdownData()

			success = success AndAlso LoadNegativeLMData()

			Return success

		End Function

		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantYearDropDownData() As Boolean

			Dim mandant = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData
			Dim yearData = m_UCMediator.CommonDbAccess.LoadMandantYears(mandant.MandantNumber)

			If (yearData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
			End If

			Dim wrappedValues As List(Of IntegerValueViewWrapper) = Nothing

			If Not yearData Is Nothing Then
				wrappedValues = New List(Of IntegerValueViewWrapper)

				For Each yearValue In yearData
					wrappedValues.Add(New IntegerValueViewWrapper With {.Value = yearValue})
				Next

			End If

			lueYear.Properties.DataSource = wrappedValues
			lueYear.Properties.ForceInitialize()

			Return yearData IsNot Nothing
		End Function

		''' <summary>
		''' Loads month drop down data.
		''' </summary>
		Private Function LoadMonthDropDownData() As Boolean

			Dim monthList As New List(Of IntegerValueViewWrapper)

			If Not lueYear.EditValue Is Nothing Then

				Dim year = lueYear.EditValue
				Dim employeeNumber = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData.EmployeeNumber
				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber

				Dim invalidMonthList = m_UCMediator.AdvancePaymentDbAccess.LoadInvalidMonthForAdvancePayment(year, employeeNumber, mandantNumber)

				If Not invalidMonthList Is Nothing Then

					For i As Integer = 1 To 12

						If Not invalidMonthList.Contains(i) Then
							monthList.Add(New IntegerValueViewWrapper With {.Value = i})
						End If

					Next
				End If

			End If

			m_ValidMonth = monthList.Select(Function(data) data.Value).ToList()
			lueMonth.EditValue = Nothing
			lueMonth.Properties.DataSource = monthList
			lueMonth.Properties.ForceInitialize()

			Return True
		End Function

		''' <summary>
		''' Loads payment type drop down data.
		''' </summary>
		Private Function LoadPaymentTypeDropDownData() As Boolean

			Dim laData = m_UCMediator.AdvancePaymentDbAccess.LoadLAData

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

			Dim employeeNumber = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData.EmployeeNumber
			Dim mdNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber

			Dim paymentReasonTexts = m_UCMediator.AdvancePaymentDbAccess.LoadPaymentReasonTexts(employeeNumber, mdNumber)

			If (paymentReasonTexts Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlungsgrundtexte konnten nicht geladen werden."))
			End If

			lueReason.Properties.DataSource = paymentReasonTexts

			Return Not paymentReasonTexts Is Nothing

		End Function

		''' <summary>
		''' Loads the bank drop down data.
		''' </summary>
		Private Function LoadBankDropDownData() As Boolean

			Dim selectedEmployee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData

			m_BankData = m_UCMediator.EmployeeDbAccess.LoadEmployeeBanks(selectedEmployee.EmployeeNumber, Nothing, True)

			If (m_BankData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht geladen werden."))
			End If

			lueBank.EditValue = Nothing
			lueBank.Properties.DataSource = m_BankData

			Return m_BankData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the employee balance values.
		''' </summary>
		''' <returns>Boolean value indicating success</returns>
		Private Function LoadEmployeeBalanceValues() As Boolean

			Dim year = lueYear.EditValue
			Dim month = lueMonth.EditValue

			If month Is Nothing Or year Is Nothing Then
				ResetGuthabenValues()
				Return True
			End If

			Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
			Dim mandant = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData
			Dim qSteuer As Boolean = (Not String.IsNullOrWhiteSpace(employee.Q_Steuer) AndAlso Not employee.Q_Steuer = "0")

			Dim dbAccess = m_UCMediator.AdvancePaymentDbAccess

			Dim values = dbAccess.LoadGuthabenValuesForAdvancePayment(mandant.MandantNumber, employee.EmployeeNumber, month, year)

			If values Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Guthabenwerte konnte nicht geladen werden."))
				Return False
			End If

			TotaAmountInUI = m_Utility.SwissCommercialRound(values.Total)

			ReportHoursInUI = m_Utility.SwissCommercialRound(values.ReportHours)
			ReportTotalInUI = m_Utility.SwissCommercialRound(values.ReportTotal)

			Dim percentValue = If(qSteuer, values.Q_ZHLG, values.N_ZHLG)

			ReportHoursPercentInUI = percentValue
			ReportTotalPercentInUI = percentValue

			RecalculateValues()

			Return True
		End Function

		''' <summary>
		''' Recalculate values.
		''' </summary>
		Private Sub RecalculateValues()
			Dim year = lueYear.EditValue
			Dim month = lueMonth.EditValue

			If month Is Nothing Or year Is Nothing Then
				Return
			End If

			ReportHoursResultInUI = m_Utility.SwissCommercialRound(ReportHoursInUI * ReportHoursPercentInUI / 100.0)
			ReportTotalResultInUI = m_Utility.SwissCommercialRound(ReportTotalInUI * ReportTotalPercentInUI / 100.0)


			' Lohnpfändung übersteigend überprüfen!
			Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
			Dim negativeLMData = m_UCMediator.AdvancePaymentDbAccess.LoadNegativeLMData(employee.EmployeeNumber, month, year)
			Dim lohnpfaednung = negativeLMData.Where(Function(data) data.LANr = 8720).FirstOrDefault()
			Dim negativLMTotalAmount = 0D

			'If Not lohnpfaednung Is Nothing Then lohnpfaedung8720Amount = If(ReportHoursResultInUI >= lohnpfaednung.M_Btr, lohnpfaednung.M_Btr, 0)
			If Not lohnpfaednung Is Nothing Then lohnpfaedung8720Amount = lohnpfaednung.M_Btr

			Dim negativPayroll = negativeLMData.Where(Function(data) data.LANr = 8100).FirstOrDefault()
			If Not negativPayroll Is Nothing Then negativ8100Amount = negativPayroll.M_Btr
			negativLMTotalAmount = lohnpfaedung8720Amount + Math.Abs(negativ8100Amount)

			NegativLohnTotalResultInUI = m_Utility.SwissCommercialRound(negativLMTotalAmount)


			PossiblePayOutForReportHoursInUI = m_Utility.SwissCommercialRound(ReportHoursResultInUI) - Math.Abs(TotaAmountInUI) - Math.Abs(negativ8100Amount)	'negativLMTotalAmount
			PossiblePayOutForReportTotalInUI = m_Utility.SwissCommercialRound(ReportTotalResultInUI) - Math.Abs(TotaAmountInUI) - Math.Abs(negativ8100Amount)	'negativLMTotalAmount

			' build ToolTip for Amount
			BuildToolTipOnTotalAmount()

		End Sub

		''' <summary>
		''' Loads the negative LM data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadNegativeLMData() As Boolean

			Dim year = lueYear.EditValue
			Dim month = lueMonth.EditValue

			If month Is Nothing Or year Is Nothing Then
				Return True
			End If

			Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
			Dim mandant = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData
			Dim qSteuer As Boolean = (Not String.IsNullOrWhiteSpace(employee.Q_Steuer) AndAlso Not employee.Q_Steuer = "0")

			Dim dbAccess = m_UCMediator.AdvancePaymentDbAccess
			Dim negativeLMData = dbAccess.LoadNegativeLMData(employee.EmployeeNumber, month, year)

			If negativeLMData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Negative Lohndaten konnten nicht geladen werden."))
			End If

			grdMSalary.DataSource = negativeLMData

			Return True
		End Function

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles change of year.
		''' </summary>
		Private Sub OnLueYear_EditValueChanged(sender As Object, e As EventArgs) Handles lueYear.EditValueChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			LoadMonthDropDownData()
			LoadEmployeeBalanceValues()

			ResetNegativeLMDataGrid()
			LoadNegativeLMData()

		End Sub

		''' <summary>
		''' Handles change of month.
		''' </summary>
		Private Sub OnLueMonth_EditValueChanged(sender As Object, e As EventArgs) Handles lueMonth.EditValueChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			LoadEmployeeBalanceValues()

			ResetNegativeLMDataGrid()
			LoadNegativeLMData()

		End Sub

		Private Sub luePaymentType_EditValueChanged(sender As Object, e As EventArgs) Handles luePaymentType.EditValueChanged

			If (m_SuppressUIEvents) Then Return

			If luePaymentType.EditValue Is Nothing Then Return
			dateEditPayAt.Visible = Convert.ToDecimal(luePaymentType.EditValue) <> 8920
			lblZahlungAm.Visible = Convert.ToDecimal(luePaymentType.EditValue) <> 8920

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
		''' Handles totoal informations on click
		''' </summary>
		''' <remarks></remarks>
		Private Sub BuildToolTipOnTotalAmount()

			Dim msgTotalAmountInUI = String.Format(m_Translate.GetSafeTranslationValue("Total geleistete Zahlungen: {0:n2}"), (TotaAmountInUI))

			Dim msgPossiblePayOutForReportHoursInUI = String.Format(m_Translate.GetSafeTranslationValue("Total Rapportstunden: {0:n2}"), (ReportHoursResultInUI))
			Dim msgPossiblePayOutForReportTotalInUI = String.Format(m_Translate.GetSafeTranslationValue("Total Rapportdaten: {0:n2}"), (ReportTotalResultInUI))

			Dim msglohnpfaendungAmount = String.Format(m_Translate.GetSafeTranslationValue("Lohnpfändung übersteigend: {0:n2}"), lohnpfaedung8720Amount)
			Dim msgnegativPayrollAmount = String.Format(m_Translate.GetSafeTranslationValue("Minuslohnvortrag: {0:n2}"), Math.Abs(negativ8100Amount))

			lblMoeglicheAuszahlung1.ToolTip = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", vbNewLine, msgTotalAmountInUI, msgPossiblePayOutForReportHoursInUI, msgPossiblePayOutForReportTotalInUI, msglohnpfaendungAmount, msgnegativPayrollAmount)
			lblMoeglicheAuszahlung2.ToolTip = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", vbNewLine, msgTotalAmountInUI, msgPossiblePayOutForReportHoursInUI, msgPossiblePayOutForReportTotalInUI, msglohnpfaendungAmount, msgnegativPayrollAmount)

		End Sub

		''' <summary>
		''' Handles button click on bank.
		''' </summary>
		Private Sub OnLueBank_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueBank.ButtonClick

			If (e.Button.Index = 2) Then
				Dim employeeData = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
				Dim bank = SelectedBank
				Dim selectedBankRecordNumber As Integer? = Nothing

				If Not bank Is Nothing Then
					selectedBankRecordNumber = bank.RecordNumber
				End If

				LoadBankDropDownData()

				' Reselect bank than was previously selected.
				If selectedBankRecordNumber.HasValue Then

					Dim bankToReSelect = m_BankData.Where(Function(data) data.RecordNumber.HasValue AndAlso data.RecordNumber = selectedBankRecordNumber).FirstOrDefault()

					If Not bankToReSelect Is Nothing Then
						lueBank.EditValue = bankToReSelect.ID
					End If

				End If

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

		Sub OngvMSalary_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvMSalary.RowCellClick

			If (e.Clicks = 2) Then

				Dim selectedRows = gvMSalary.GetSelectedRows()

				If (selectedRows.Count > 0) Then

					If gvMSalary.GetRow(selectedRows(0)) Is Nothing Then Return

					Dim monthlySalaryData = CType(gvMSalary.GetRow(selectedRows(0)), NegativeLMData)

					Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
					ShowMonthlySalaryDetailForm(employee.EmployeeNumber, monthlySalaryData.LMNR)
				End If

			End If

		End Sub

#End Region

#Region "Helper Methods"

		''' <summary>
		''' Sets the visibility of the guthaben values line2.
		''' </summary>
		Private Sub SetVisiblityOfGuthabenValuesLine2()

			Dim isUserAllowedToSeeSecondGuthabenValuesLine2 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 357, m_InitializationData.MDData.MDNr)
			pnlGuthabenValuesLine2.Visible = isUserAllowedToSeeSecondGuthabenValuesLine2

		End Sub

		''' <summary>
		''' Sets the currency labels.
		''' </summary>
		Private Sub SetCurrencyLabels()

			Dim currency = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeLOSettingData.Currency

			currency = If(String.IsNullOrWhiteSpace(currency), "CHF", currency)

			lblCurrency.Text = currency
			lblCurrencyLine1.Text = currency
			lblCurrencyLine1_2.Text = currency
			lblCurrencyLine1_3.Text = currency
			lblCurrencynegativelohn.Text = currency
			lblCurrencyAuszahlung1.Text = currency

			lblCurrencyLine2.Text = currency
			lblCurrencyLine2_2.Text = currency
			lblCurrencyAuszahlung2.Text = currency

		End Sub

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
		''' Handles close of monthly salary form.
		''' </summary>
		Private Sub OnMonthlySalaryFormClosed(sender As System.Object, e As System.EventArgs)
			LoadNegativeLMData()

			Dim monthlySalaryForm = CType(sender, frmMSalary)

			If Not monthlySalaryForm.SelectedMonthlySalary Is Nothing Then
				Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData

				FocusMonthlySalary(employee.EmployeeNumber, monthlySalaryForm.SelectedMonthlySalary.LMNr)
			End If

		End Sub

		''' <summary>
		''' Handles monthly salary form data saved.
		''' </summary>
		Private Sub OnMonthlySalaryFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)

			LoadNegativeLMData()

			Dim employee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
			FocusMonthlySalary(employee.EmployeeNumber, lmnr)

		End Sub

		''' <summary>
		''' Handles monthly salary form data deleted saved.
		''' </summary>
		Private Sub OnMonthlySalaryFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)
			LoadNegativeLMData()
		End Sub

		''' <summary>
		''' Focuses a monthly salary.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="lmNr">The lmNr record number.</param>
		Private Sub FocusMonthlySalary(ByVal employeeNumber As Integer, ByVal lmNr As Integer)

			Dim listDataSource As List(Of NegativeLMData) = grdMSalary.DataSource

			Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.MANR = employeeNumber AndAlso
																															 data.LMNR = lmNr).FirstOrDefault()

			If Not monthlySalaryViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
				Dim rowHandle = gvMSalary.GetRowHandle(sourceIndex)
				gvMSalary.FocusedRowHandle = rowHandle
			End If

		End Sub

		''' <summary>
		''' Shows the monthly salary management form.
		''' </summary>
		''' <param name="lmNr">The monthly salary record number.</param>
		''' <param name="employeeNumber">The employee number.</param>
		Private Sub ShowMonthlySalaryDetailForm(ByVal employeeNumber As Integer, ByVal lmNr As Integer?)

			If m_MonthlySalaryMagementDetailForm Is Nothing OrElse m_MonthlySalaryMagementDetailForm.IsDisposed Then

				If Not m_MonthlySalaryMagementDetailForm Is Nothing Then
					'First cleanup handlers of old form before new form is created.
					RemoveHandler m_MonthlySalaryMagementDetailForm.FormClosed, AddressOf OnMonthlySalaryFormClosed
					RemoveHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataSaved, AddressOf OnMonthlySalaryFormDataSaved
					RemoveHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataDeleted, AddressOf OnMonthlySalaryFormDataDeleted
				End If

				m_MonthlySalaryMagementDetailForm = New frmMSalary(m_InitializationData)
				AddHandler m_MonthlySalaryMagementDetailForm.FormClosed, AddressOf OnMonthlySalaryFormClosed
				AddHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataSaved, AddressOf OnMonthlySalaryFormDataSaved
				AddHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataDeleted, AddressOf OnMonthlySalaryFormDataDeleted
			End If

			m_MonthlySalaryMagementDetailForm.Show()

			If lmNr.HasValue Then
				m_MonthlySalaryMagementDetailForm.LoadData(employeeNumber, lmNr, True)
			Else
				m_MonthlySalaryMagementDetailForm.NewMonthlySalary(employeeNumber, True)
			End If

			m_MonthlySalaryMagementDetailForm.BringToFront()

		End Sub


#End Region


#Region "Preselection"

		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim supressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim hasPreselectionData = PreselectionData IsNot Nothing

			If hasPreselectionData AndAlso PreselectionData.reportyear.HasValue Then
				lueYear.EditValue = PreselectionData.reportyear
			Else
				lueYear.EditValue = DateTime.Now.Year
			End If

			LoadMonthDropDownData()

			If hasPreselectionData AndAlso PreselectionData.reportmonth.HasValue Then
				lueMonth.EditValue = PreselectionData.reportmonth
			Else
				If m_ValidMonth IsNot Nothing AndAlso m_ValidMonth.Contains(DateTime.Now.Month) Then
					lueMonth.EditValue = DateTime.Now.Date.Month
				End If
			End If


			SetPaymentReasonInfoText(AdvancePaymentReasonSetting)
			chkGebAbzug.Checked = AdvancePaymentWithFeeSetting

			If MustPayoutDateBeSetToTodaySetting Then
				dateEditPayAt.EditValue = DateTime.Now.Date
			End If

			Dim defaultPaymentTypeSetting = AdvancePaymentDefaultPaymentTypeSetting
			If hasPreselectionData AndAlso PreselectionData.LANr.HasValue AndAlso PreselectionData.LANr > 0 Then
				luePaymentType.EditValue = Convert.ToDecimal(PreselectionData.LANr.Value)
			ElseIf Not String.IsNullOrWhiteSpace(defaultPaymentTypeSetting) Then
				luePaymentType.EditValue = Convert.ToDecimal(defaultPaymentTypeSetting)
			Else
				luePaymentType.EditValue = Convert.ToDecimal(8920)
			End If
			dateEditPayAt.Visible = Convert.ToDecimal(luePaymentType.EditValue) <> 8920
			lblZahlungAm.Visible = Convert.ToDecimal(luePaymentType.EditValue) <> 8920

			' preselect bankdata
			If m_BankData Is Nothing Then
				lueBank.EditValue = Nothing
				Return
			End If

			Dim idBankToSelect As Integer?

			'  ZG Bank
			Dim zgBank = m_BankData.Where(Function(data) data.BankZG.HasValue AndAlso data.BankZG).FirstOrDefault
			If zgBank IsNot Nothing Then
				idBankToSelect = zgBank.ID
			End If

			If Not idBankToSelect.HasValue Then

				' Use active bank if no bank is checked as ZG.
				Dim activeBank = m_BankData.Where(Function(data) data.ActiveRec.HasValue AndAlso data.ActiveRec).FirstOrDefault

				If activeBank IsNot Nothing Then
					idBankToSelect = activeBank.ID
				End If

			End If

			lueBank.EditValue = idBankToSelect

			m_SuppressUIEvents = supressUIEventState

		End Sub

#End Region


#Region "View helper classes"

		''' <summary>
		''' Shows the bank management form.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="bankRecordNumber">The bank record number.</param>
		Private Sub ShowBankDetailForm(ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer?)

			m_BankDetailForm = New frmBank(m_InitializationData)

			If (bankRecordNumber.HasValue) Then
				m_BankDetailForm.LoadBankData(employeeNumber, bankRecordNumber)
			Else
				m_BankDetailForm.NewBank(employeeNumber)
			End If

			m_BankDetailForm.ShowDialog()

		End Sub


		''' <summary>
		''' Wraps an integer value.
		''' </summary>
		Class IntegerValueViewWrapper
			Public Property Value As Integer
		End Class

#End Region


	End Class

End Namespace
