Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports System.ComponentModel
Imports SP.Internal.Automations.BaseTable
Imports SP.Internal.Automations
Imports SP.DatabaseAccess.Customer.DataObjects

Namespace UI

	Public Class ucPageSelectCandidateAndCustomer

#Region "Private Constants"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The mandant data.
		''' </summary>
		Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData

		''' <summary>
		''' The employee data.
		''' </summary>
		Private m_SelectedEmployeeData As SP.DatabaseAccess.Employee.DataObjects.MasterdataMng.EmployeeMasterData

		''' <summary>
		''' The customer data.
		''' </summary>
		Private m_SelectedCustomerData As SP.DatabaseAccess.Customer.DataObjects.CustomerMasterData

		''' <summary>
		''' The responsible person data.
		''' </summary>
		Private m_SelectedResponsiblePersonData As SP.DatabaseAccess.Customer.DataObjects.ResponsiblePersonMasterData

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		Private m_BaseTableData As BaseTable.SPSBaseTables
		Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			m_ProgPath = New ClsProgPath
			m_Mandant = New Mandant


			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCustomer.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueZHD.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected candidate and customer data.
		''' </summary>
		''' <returns>Candidate and customer data.</returns>
		Public ReadOnly Property SelectedCandidateAndCustomerData As InitCandidateAndCustomerData
			Get

				Dim data As New InitCandidateAndCustomerData With {
					.MandantData = m_SelectedMandantData,
					.EmployeeData = m_SelectedEmployeeData,
					.CustomerData = m_SelectedCustomerData,
					.ResponsiblePersondata = m_SelectedResponsiblePersonData,
					.EmployeeNoticeEmployement = txtESComment.EditValue,
					.CustomerNoticeEmployement = txtCustomerComment.EditValue
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

			m_BaseTableData = New SPSBaseTables(m_InitializationData)
			m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)

		End Sub

		''' <summary>
		''' Activates the page.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function ActivatePage() As Boolean

			Dim success As Boolean = True

			If m_IsFirstPageActivation Then
				success = success AndAlso LoadMandantDropDownData()

				PreselectData()

			End If

			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_IsFirstPageActivation = True

			ResetEmployeeDetailData()

			'  Reset drop downs and lists

			ResetMandantDropDown()
			ResetEmployeeDropDown()

			ResetCustomerDropDown()
			ResetResponsiblePersonDropDown()
			ResetCustomerDetailData()

			ErrorProvider.Clear()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim isValid As Boolean = True
			Dim isCustomerValid As Boolean = True

			errorText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie einen Mandanten aus.")
			isValid = isValid And SetErrorIfInvalid(lueMandant, ErrorProvider, lueMandant.EditValue Is Nothing, errorText)

			errorText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie einen Kandidaten aus.")
			isValid = isValid And SetErrorIfInvalid(lueEmployee, ErrorProvider, lueEmployee.EditValue Is Nothing, errorText)
			errorText = String.Empty

			isCustomerValid = (Not lueCustomer.EditValue Is Nothing)

			If Not isCustomerValid Then errorText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie einen Kunden aus.")

			If Not lueCustomer.EditValue Is Nothing Then
				Dim customerData = m_UCMediator.CustomerDbAccess.LoadCustomerMasterData(lueCustomer.EditValue, m_ClsProgSetting.GetUSFiliale)
				Dim CreditLimit1 = customerData.CreditLimit1.GetValueOrDefault(0)
				Dim CreditLimit2 = customerData.CreditLimit2.GetValueOrDefault(0)
				Dim CreditLimitToDate = customerData.CreditLimitsToDate
				Dim OpenInvoiceAmount = customerData.OpenInvoiceAmount.GetValueOrDefault(0)
				Dim CreditWarning = customerData.CreditWarning.GetValueOrDefault(False)

				' 1. Creditlimit
				If CreditWarning Then
					If CreditLimit1 > 0 AndAlso OpenInvoiceAmount > CreditLimit1 AndAlso Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 272, m_InitializationData.MDData.MDNr) Then
						errorText &= If(String.IsNullOrWhiteSpace(errorText), "", vbNewLine) & String.Format(m_Translate.GetSafeTranslationValue("1. Kreditlimite wurde erreicht: {0:n2}"), CreditLimit1)
						isCustomerValid = False
					End If

					' 2. Creditlimit
					If CreditLimit2 > 0 AndAlso OpenInvoiceAmount > CreditLimit2 AndAlso Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 273, m_InitializationData.MDData.MDNr) Then
						errorText &= If(String.IsNullOrWhiteSpace(errorText), "", vbNewLine) & String.Format(m_Translate.GetSafeTranslationValue("2. Kreditlimite wurde erreicht: {0:n2}"), CreditLimit2)
						isCustomerValid = False
					End If
					'If CreditLimitToDate.HasValue Then
					'	If CreditLimitToDate < Now.Date Then
					'		errorText &= If(String.IsNullOrWhiteSpace(errorText), "", vbNewLine) & String.Format(m_Translate.GetSafeTranslationValue("Kreditgültigkeit abgelaufen: {0:g}. Bitte neu beantragen."), CreditLimitToDate)
					'	End If
					'End If

				End If

			End If
			isValid = isValid And SetErrorIfInvalid(lueCustomer, ErrorProvider, Not isCustomerValid, errorText)

			Return isValid

		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Group Mandantdaten
			Me.grpMandantData.Text = m_Translate.GetSafeTranslationValue(Me.grpMandantData.Text)
			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)

			' Group Kandidatendaten
			Me.grpCandidateData.Text = m_Translate.GetSafeTranslationValue(Me.grpCandidateData.Text)
			Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)
			Me.lblGebDatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGebDatum.Text)
			Me.lblAdresseKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresseKandidat.Text)
			Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)
			Me.lblMAStatus.Text = m_Translate.GetSafeTranslationValue(Me.lblMAStatus.Text)
			Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)
			Me.lblBemerkung.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung.Text)

			' Group Kundendaten
			Me.grpCustomerData.Text = m_Translate.GetSafeTranslationValue(Me.grpCustomerData.Text)
			Me.lblFirma.Text = m_Translate.GetSafeTranslationValue(Me.lblFirma.Text)
			Me.lblZHD.Text = m_Translate.GetSafeTranslationValue(Me.lblZHD.Text)
			lblCustomerComment.Text = m_Translate.GetSafeTranslationValue(lblCustomerComment.Text)

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
		''' Resets the responsible person drop down.
		''' </summary>
		Private Sub ResetResponsiblePersonDropDown()

			lueZHD.Properties.DisplayMember = "SalutationLastNameFirstName"
			lueZHD.Properties.ValueMember = "ResponsiblePersonRecordNumber"

			gvZHD.OptionsView.ShowIndicator = False
			gvZHD.OptionsView.ShowColumnHeaders = True
			gvZHD.OptionsView.ShowFooter = False
			gvZHD.OptionsView.ShowAutoFilterRow = True
			gvZHD.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvZHD.Columns.Clear()

			Dim columnRecordNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecordNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnRecordNumber.Name = "ResponsiblePersonRecordNumber"
			columnRecordNumber.FieldName = "ResponsiblePersonRecordNumber"
			columnRecordNumber.Visible = False
			gvZHD.Columns.Add(columnRecordNumber)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.Caption = m_Translate.GetSafeTranslationValue("Zuständige Person")
			columnName.Name = "SalutationLastNameFirstName"
			columnName.FieldName = "SalutationLastNameFirstName"
			columnName.Visible = True
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvZHD.Columns.Add(columnName)


			lueZHD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueZHD.Properties.NullText = String.Empty
			lueZHD.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets employee detail data.
		''' </summary>
		Private Sub ResetEmployeeDetailData()

			lblBirthdateValue.Text = String.Empty
			lblEmployeeAddressValue.Text = String.Empty
			lblQualificationValue.Text = String.Empty
			lblMAStateValue.Text = String.Empty
			lblBewilligungValue.Text = String.Empty

			lblBemerkung.Visible = False
			txtESComment.EditValue = String.Empty
			txtESComment.Visible = False

			iconPermissionWarning.Visible = False

		End Sub

		Private Sub ResetCustomerDetailData()

			txtCustomerComment.EditValue = String.Empty
			txtCustomerComment.Visible = False

			lblCustomerComment.Visible = False

		End Sub

		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_UCMediator.CommonDbAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the employee drop down data.
		''' </summary>
		Private Function LoadEmployeeDropDownData() As Boolean

			Dim employeeData = m_UCMediator.ESDbAccess.LoadEmployeeData()

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

			Dim customerData = m_UCMediator.ESDbAccess.LoadCustomerData(m_InitializationData.UserData.UserFiliale)

			If customerData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundendaten konnen nicht geladen werden."))
				Return False
			End If

			lueCustomer.EditValue = Nothing
			lueCustomer.Properties.DataSource = customerData

			Return True

		End Function

		''' <summary>
		''' Load responsible person drop down data.
		''' </summary>
		Private Function LoadResponsiblePersonsDropDownData(ByVal customerNumber As Integer) As Boolean
			Dim responsiblePersonData = m_UCMediator.CustomerDbAccess.LoadResponsiblePersonDataActiv(customerNumber)

			Dim responsiblePersonViewData = Nothing

			If Not responsiblePersonData Is Nothing Then

				responsiblePersonViewData = New List(Of ResponsiblePersonViewData)

				For Each person In responsiblePersonData
					responsiblePersonViewData.Add(New ResponsiblePersonViewData With {
																																					.Lastname = person.Lastname,
																																					.Firstname = person.Firstname,
																																					.TranslatedSalutation = person.TranslatedSalutation,
																																					.ResponsiblePersonRecordNumber = person.RecordNumber,
																																					.ZState1 = person.ZState1,
																																					.ZState2 = person.ZState2
																																					 })
				Next

			End If

			lueZHD.EditValue = Nothing
			lueZHD.Properties.DataSource = responsiblePersonViewData

			Return Not responsiblePersonViewData Is Nothing
		End Function

		''' <summary>
		''' Handles change of mandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If Not lueMandant.EditValue Is Nothing Then

				Dim mandantData = CType(lueMandant.GetSelectedDataRow(), MandantData)

				m_SelectedMandantData = mandantData
				m_UCMediator.HandleChangeOfMandant(m_SelectedMandantData.MandantNumber)

				LoadEmployeeDropDownData()
				LoadCustomerDropDownData()

			Else
				m_SelectedMandantData = Nothing

				lueEmployee.EditValue = Nothing
				lueCustomer.EditValue = Nothing
				lueZHD.EditValue = Nothing

				lueEmployee.Properties.DataSource = Nothing
				lueCustomer.Properties.DataSource = Nothing
				lueZHD.Properties.DataSource = Nothing

			End If

			m_UCMediator.HandleChangeMandantEmployeeOrCustomer()

		End Sub

		''' <summary>
		''' Handles change of employee.
		''' </summary>
		Private Sub OnLueEmployee_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueEmployee.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueEmployee.EditValue Is Nothing Then

				Dim employeeNumber As Integer = lueEmployee.EditValue

				Dim employeeMasterData = m_UCMediator.EmployeeDbAccess.LoadEmployeeMasterData(employeeNumber, False)
				Dim employeeContactCommData As EmployeeContactComm = m_UCMediator.EmployeeDbAccess.LoadEmployeeContactCommData(employeeNumber)

				If employeeMasterData Is Nothing Or
					employeeContactCommData Is Nothing Then

					ResetEmployeeDetailData()
					m_SelectedEmployeeData = Nothing

					If employeeMasterData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeitestammdaten konnten nicht geladen werden."))
					End If

					If employeeContactCommData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter (KontaktKomm) konnten nicht geladen werden."))
					End If

				Else
					m_SelectedEmployeeData = employeeMasterData

					DisplayEmployeeData(employeeMasterData, employeeContactCommData)
				End If

			Else
				ResetEmployeeDetailData()
				m_SelectedEmployeeData = Nothing
			End If

			m_UCMediator.HandleChangeMandantEmployeeOrCustomer()

		End Sub

		''' <summary>
		''' Handles change of customer.
		''' </summary>
		Private Sub OnLueCustomer_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCustomer.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueCustomer.EditValue Is Nothing Then

				Dim customerMasterData = m_UCMediator.CustomerDbAccess.LoadCustomerMasterData(lueCustomer.EditValue, m_ClsProgSetting.GetUSFiliale)

				If customerMasterData Is Nothing Then
					m_UtilityUI.ShowErrorDialog("Kundendaten konnten nicht geladen werden.")
				End If
				ResetCustomerDetailData()

				If customerMasterData.CreditWarning.GetValueOrDefault(False) AndAlso
					(customerMasterData.CreditLimit1 > 0 AndAlso customerMasterData.OpenInvoiceAmount >= customerMasterData.CreditLimit1) Or
					(customerMasterData.CreditLimit2 > 0 AndAlso customerMasterData.OpenInvoiceAmount >= customerMasterData.CreditLimit2) Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Achtung: Kunden-Kreditlimite wurde erreicht oder überschritten.{0}Offener Debitorenbetrag: {1:n2}{0}1. Kunden-Kreditlimite: {2:n2}{0}2. Kunden-Kreditlimite: {3:n2}")
					msg = String.Format(msg, vbNewLine, customerMasterData.OpenInvoiceAmount, customerMasterData.CreditLimit1, customerMasterData.CreditLimit2)

					m_UtilityUI.ShowInfoDialog(msg)
				End If

				m_SelectedCustomerData = customerMasterData
				DisplayCustomerData(customerMasterData)

				LoadResponsiblePersonsDropDownData(lueCustomer.EditValue)

			Else
				ResetCustomerDetailData()
				m_SelectedCustomerData = Nothing
				lueZHD.EditValue = Nothing
				lueZHD.Properties.DataSource = Nothing
			End If

			m_UCMediator.HandleChangeMandantEmployeeOrCustomer()

		End Sub

		''' <summary>
		''' Handles change of ZHD.
		''' </summary>
		Private Sub OnLueZHD_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueZHD.EditValueChanged

			If Not lueCustomer.EditValue Is Nothing AndAlso
				Not lueZHD.EditValue Is Nothing Then

				Dim responsiblePersonData = m_UCMediator.CustomerDbAccess.LoadResponsiblePersonMasterData(lueCustomer.EditValue, lueZHD.EditValue)

				m_SelectedResponsiblePersonData = responsiblePersonData

				If responsiblePersonData Is Nothing Then
					m_UtilityUI.ShowErrorDialog("Daten der zuständigen Person konnten nicht geladen werden.")
				End If

			Else
				m_SelectedResponsiblePersonData = Nothing
			End If

		End Sub

		''' <summary>
		'''  Handles RowStyle event of gvZHD grid view.
		''' </summary>
		Private Sub OngvZHD_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvZHD.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvZHD.GetRow(e.RowHandle), ResponsiblePersonViewData)

				If Not rowData.IsZHDActiv.GetValueOrDefault(True) Then
					e.Appearance.BackColor = Color.LightGray
					e.Appearance.BackColor2 = Color.LightGray
				End If

			End If

		End Sub

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


			' Bewilligung
			Dim employeePermissionCode = employeeMasterData.Permission
			If Not String.IsNullOrWhiteSpace(employeePermissionCode) AndAlso Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
				Dim bewData = m_PermissionData.Where(Function(x) x.Code = employeePermissionCode).FirstOrDefault()
				If Not bewData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewData.Translated_Value) Then employeePermissionCode = String.Format("{0} - {1}", bewData.Code, bewData.Translated_Value)
			End If
			lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", employeePermissionCode, employeeMasterData.PermissionToDate)
			'lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", m_UCMediator.CommonDbAccess.TranslatePermissionCode(employeeMasterData.Permission, m_InitializationData.UserData.UserLanguage), employeeMasterData.PermissionToDate)


			' Bewilligung warn icon
			iconPermissionWarning.Visible = employeeMasterData.PermissionToDate.HasValue AndAlso
																			(employeeMasterData.PermissionToDate.Value.Date < DateTime.Now.Date) AndAlso
																			Not String.IsNullOrWhiteSpace(employeeMasterData.Permission)

			' notice_employment
			' m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
			Dim insertNewline As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_InitializationData.MDData.MDFormXMLFileName, String.Format("{0}/insertnewlineintoemploymentnotice", FORM_XML_MAIN_KEY)), False)
			Dim notice As String = String.Format("{0}", employeeMasterData.Notice_Employment)

			If Not String.IsNullOrWhiteSpace(notice) Then
				If insertNewline.GetValueOrDefault(False) AndAlso Not notice.StartsWith(vbNewLine) Then
					notice = vbNewLine & notice
				End If
				txtESComment.EditValue = String.Format("{0}", notice)
				txtESComment.Visible = True

				lblBemerkung.Visible = True
			End If

			Return True
		End Function

		Private Function DisplayCustomerData(ByVal customerData As CustomerMasterData) As Boolean

			Dim insertNewline As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_InitializationData.MDData.MDFormXMLFileName, String.Format("{0}/insertnewlineintoemploymentnotice", FORM_XML_MAIN_KEY)), False)
			Dim notice As String = String.Format("{0}", customerData.Notice_Employment)

			If Not String.IsNullOrWhiteSpace(notice) Then
				If insertNewline.GetValueOrDefault(False) AndAlso Not notice.StartsWith(vbNewLine) Then
					notice = vbNewLine & notice
				End If
				txtCustomerComment.EditValue = String.Format("{0}", notice)
				txtCustomerComment.Visible = True

				lblCustomerComment.Visible = True
			End If

			Return True
		End Function

		''' <summary>
		''' Handles button click on employee.
		''' </summary>
		Private Sub OnLueEmployee_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueEmployee.ButtonClick

			If lueMandant.EditValue Is Nothing Or lueEmployee.EditValue Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then

				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueEmployee.EditValue)
				hub.Publish(openEmployeeMng)

			End If

		End Sub

		''' <summary>
		''' Handles button click on customer.
		''' </summary>
		Private Sub OnLueCustomer_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueCustomer.ButtonClick

			If lueMandant.EditValue Is Nothing Or lueEmployee.EditValue Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then

				Dim hub = MessageService.Instance.Hub
				Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueCustomer.EditValue)
				hub.Publish(openCustomerMng)

			End If

		End Sub


		''' <summary>
		''' Handles button click on ZHD.
		''' </summary>
		Private Sub OnLueZHD_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueZHD.ButtonClick

			If lueMandant.EditValue Is Nothing Or lueCustomer.EditValue Is Nothing Or lueZHD.EditValue Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then

				Dim hub = MessageService.Instance.Hub
				Dim openResponsiblePersonMng As New OpenResponsiblePersonMngRequest(Me, m_InitializationData.UserData.UserNr, lueMandant.EditValue, lueCustomer.EditValue, lueZHD.EditValue)
				hub.Publish(openResponsiblePersonMng)

			End If

		End Sub

		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			If Not PreselectionData Is Nothing Then

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

				' ---Employee---
				Dim preselectEmployeeSuccesful = False
				If PreselectionData.EmployeeNumber.HasValue AndAlso Not lueEmployee.Properties.DataSource Is Nothing Then

					Dim employeeDataList = CType(lueEmployee.Properties.DataSource, List(Of SP.DatabaseAccess.ES.DataObjects.ESMng.EmployeeData))

					If employeeDataList.Any(Function(employee) employee.EmployeeNumber = PreselectionData.EmployeeNumber) Then
						lueEmployee.EditValue = PreselectionData.EmployeeNumber.Value
						preselectEmployeeSuccesful = True
					End If
				End If

				If PreselectionData.EmployeeNumber.HasValue AndAlso Not preselectEmployeeSuccesful Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Kandidat konnte nicht vorselektiert werden."))
				End If


				' ---Customer---
				Dim preselectCustomerSuccesful = False
				If PreselectionData.CustomerNumber.HasValue AndAlso Not lueCustomer.Properties.DataSource Is Nothing Then

					Dim customerDataList = CType(lueCustomer.Properties.DataSource, List(Of SP.DatabaseAccess.ES.DataObjects.ESMng.CustomerData))

					If customerDataList.Any(Function(customer) customer.CustomerNumber = PreselectionData.CustomerNumber) Then
						lueCustomer.EditValue = PreselectionData.CustomerNumber.Value
						preselectCustomerSuccesful = True
					End If

				End If

				If PreselectionData.CustomerNumber.HasValue AndAlso Not preselectCustomerSuccesful Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Kunde konnte nicht vorselektiert werden."))
				End If


				' ---ZHD---
				Dim preselectZHDSuccesful = False
				If PreselectionData.ResponsiblePersonNumber.HasValue AndAlso Not lueZHD.Properties.DataSource Is Nothing Then

					Dim zhdDataList = CType(lueZHD.Properties.DataSource, List(Of ResponsiblePersonViewData))

					If zhdDataList.Any(Function(zhd) zhd.ResponsiblePersonRecordNumber = PreselectionData.ResponsiblePersonNumber) Then
						lueZHD.EditValue = PreselectionData.ResponsiblePersonNumber.Value
						preselectZHDSuccesful = True
					End If

				End If

				If PreselectionData.ResponsiblePersonNumber.HasValue AndAlso Not preselectZHDSuccesful Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Zuständiger Disponent konnte nicht vorselektiert werden."))
				End If

				m_SuppressUIEvents = supressUIEventState
			Else
				' No preslection data -> use mandant form initialization object.

				' ---Mandant---
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

#Region "View helper classes"

		''' <summary>
		''' Responsible person view data.
		''' </summary>
		Class ResponsiblePersonViewData

			Public Property Lastname As String
			Public Property Firstname As String
			Public Property TranslatedSalutation As String
			Public Property ResponsiblePersonRecordNumber As Integer?

			Public Property ZState1 As String
			Public Property ZState2 As String

			Public ReadOnly Property IsZHDActiv As Boolean?
				Get
					Dim isZActiv As Boolean = True
					Dim state1 As String = If(String.IsNullOrWhiteSpace(ZState1), String.Empty, ZState1.ToLower)
					Dim state2 As String = If(String.IsNullOrWhiteSpace(ZState2), String.Empty, ZState2.ToLower)

					isZActiv = Not (state1.Contains("inaktiv") OrElse state1.Contains("mehr aktiv") OrElse state2.Contains("inaktiv") OrElse state2.Contains("mehr aktiv"))
					Return isZActiv
				End Get
			End Property

			Public ReadOnly Property SalutationLastNameFirstName
				Get
					Return String.Format("{0} {1} {2}", TranslatedSalutation, Lastname, Firstname)
				End Get
			End Property
		End Class

		Private Sub grpCustomerData_Click(sender As Object, e As EventArgs) Handles grpCustomerData.Click

		End Sub

#End Region


	End Class

End Namespace
