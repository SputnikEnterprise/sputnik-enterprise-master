Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports System.ComponentModel
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Namespace UI

	Public Class ucPageSelectMandant

#Region "Private Constants"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "Private Fields"
		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		''' <summary>
		''' The mandant data.
		''' </summary>
		Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData

		''' <summary>
		''' The advisor data.
		''' </summary>
		Private m_SelectedAdvisorData As SP.DatabaseAccess.Common.DataObjects.AdvisorData

		''' <summary>
		''' The employee data.
		''' </summary>
		Private m_SelectedEmployeeData As SP.DatabaseAccess.Employee.DataObjects.MasterdataMng.EmployeeMasterData

		''' <summary>
		''' The employee LO setting data.
		''' </summary>
		Private m_SelectedEmployeeLoSettingData As SP.DatabaseAccess.Employee.DataObjects.Salary.EmployeeLOSettingsData

		''' <summary>
		''' The available advisors.
		''' </summary>
		Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

		Private m_BaseTableData As BaseTable.SPSBaseTables
		Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			Try
				m_ProgPath = New ClsProgPath
				m_Mandant = New Mandant

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisor1.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueEmployee.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected candidate and advisor data.
		''' </summary>
		''' <returns>Candidate and advisor data.</returns>
		Public ReadOnly Property SelectedCandidateAndAdvisorData As InitCandidateAndAdvisorData
			Get

				Dim data As New InitCandidateAndAdvisorData With {
					.MandantData = m_SelectedMandantData,
					.EmployeeData = m_SelectedEmployeeData,
					.EmployeeLOSettingData = m_SelectedEmployeeLoSettingData,
					.AdvisorData = m_SelectedAdvisorData,
					.NoticeAdvancedpayment = txtZGComment.EditValue
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

			m_SelectedAdvisorData = Nothing
			m_SelectedEmployeeData = Nothing
			m_SelectedEmployeeLoSettingData = Nothing
			m_SelectedMandantData = Nothing

			ResetEmployeeDetailData()

			'  Reset drop downs and lists

			ResetMandantDropDown()
			ResetAdvisorDropDown()
			ResetEmployeeDropDown()

			ErrorProvider.Clear()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			ErrorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorTextNoZG As String = m_Translate.GetSafeTranslationValue("Der Kandidat ist für Vorschüsse gesperrt.")
			'Dim errorTextNoZGBecauseOfNoES As String = m_Translate.GetSafeTranslationValue("Der Kandidat hatte keinen Einsatz diesen Monat.")

			Dim isValid As Boolean = True


			isValid = isValid And SetErrorIfInvalid(lueMandant, ErrorProvider, lueMandant.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(lueEmployee, ErrorProvider, lueEmployee.EditValue Is Nothing, errorText)

			' Check NoZG Flag
			If isValid Then

				Dim employeeLOSettingsData As EmployeeLOSettingsData = m_UCMediator.EmployeeDbAccess.LoadEmployeeLOSettings(lueEmployee.EditValue)

				isValid = isValid And SetErrorIfInvalid(lueEmployee, ErrorProvider, Not employeeLOSettingsData Is Nothing AndAlso
																						employeeLOSettingsData.NoZG.HasValue AndAlso employeeLOSettingsData.NoZG, errorTextNoZG)

			End If

			'' Check 358 UserRight (Make advance payment if there is no ES for the selected employee this month)
			'   If isValid Then
			'	Dim isUserAllowedToMakeAnAdvancePaymentIfThereWasNoESForTheSelectedEmployeeAtThisMonth = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 358, m_InitializationData.MDData.MDNr)

			'     Dim numberOfESOfEmployeeThisMonth = m_UCMediator.AdvancePaymentDbAccess.LoadNumberOfESOfEmployeeForMonth(DateTime.Now.Year, DateTime.Now.Month, lueEmployee.EditValue, lueMandant.EditValue)
			'     isValid = isValid And SetErrorIfInvalid(lueEmployee, ErrorProvider, numberOfESOfEmployeeThisMonth.HasValue AndAlso numberOfESOfEmployeeThisMonth = 0 AndAlso Not isUserAllowedToMakeAnAdvancePaymentIfThereWasNoESForTheSelectedEmployeeAtThisMonth, errorTextNoZGBecauseOfNoES)


			'   End If

			Return isValid

		End Function


#End Region

#Region "Translation"


		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Group Eigenschaften
			Me.gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.gpEigenschaften.Text)
			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)

			' Group Kandidatendaten
			Me.grpCandidateData.Text = m_Translate.GetSafeTranslationValue(Me.grpCandidateData.Text)
			Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)
			Me.lblGebDatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGebDatum.Text)
			Me.lblAdresseKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresseKandidat.Text)
			Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)
			Me.lblMAStatus.Text = m_Translate.GetSafeTranslationValue(Me.lblMAStatus.Text)
			Me.lblQuellensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblQuellensteuer.Text)
			Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)
			Me.lblBemerkung.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung.Text)

		End Sub

#End Region

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
			txtZGComment.EditValue = String.Empty
			txtZGComment.Visible = False

			iconPermissionWarning.Visible = False

		End Sub

#End Region

#Region "Load Data"

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
		''' Loads the Advisor1 and Advisor2 drop down data.
		''' </summary>
		Private Sub LoadAdvisorDropDown()
			' Load data
			m_Advisors = m_UCMediator.CommonDbAccess.LoadActivatedAdvisorData()

			' Advisor1
			lueAdvisor1.EditValue = Nothing
			lueAdvisor1.Properties.DataSource = m_Advisors
			lueAdvisor1.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the employee drop down data.
		''' </summary>
		Private Function LoadEmployeeDropDownData() As Boolean

			Dim employeeData = m_UCMediator.AdvancePaymentDbAccess.LoadEmployeeData()

			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnen nicht geladen werden."))
				Return False
			End If

			lueEmployee.EditValue = Nothing
			lueEmployee.Properties.DataSource = employeeData

			Return True

		End Function

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles change of mandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If Not lueMandant.EditValue Is Nothing Then

				Dim mandantData = CType(lueMandant.GetSelectedDataRow(), MandantData)

				m_SelectedMandantData = mandantData
				m_UCMediator.HandleChangeOfMandant(m_SelectedMandantData.MandantNumber)

				LoadAdvisorDropDown()
				LoadEmployeeDropDownData()

			Else
				m_SelectedMandantData = Nothing

				lueAdvisor1.EditValue = Nothing
				lueEmployee.EditValue = Nothing

				lueAdvisor1.Properties.DataSource = Nothing
				lueEmployee.Properties.DataSource = Nothing

			End If

			m_UCMediator.HandleChangeMandantEmployeeOrEmployee()

		End Sub

		''' <summary>
		''' Handles change of advisor.
		''' </summary>
		Private Sub OnLueAdvisor1_EditValueChanged(sender As Object, e As EventArgs) Handles lueAdvisor1.EditValueChanged
			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueAdvisor1.EditValue Is Nothing Then
				m_SelectedAdvisorData = CType(lueAdvisor1.GetSelectedDataRow(), AdvisorData)
			Else
				m_SelectedAdvisorData = Nothing
			End If

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
				Dim employeeLOSettingData As EmployeeLOSettingsData = m_UCMediator.EmployeeDbAccess.LoadEmployeeLOSettings(employeeNumber)

				If employeeMasterData Is Nothing Or
				  employeeContactCommData Is Nothing Or
				  employeeLOSettingData Is Nothing Then

					ResetEmployeeDetailData()
					m_SelectedEmployeeData = Nothing
					m_SelectedEmployeeLoSettingData = Nothing

					If employeeMasterData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeitestammdaten konnten nicht geladen werden."))
					End If

					If employeeContactCommData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterdaten (KontaktKomm) konnten nicht geladen werden."))
					End If

					If employeeLOSettingData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterdaten (LoSettings) konnten nicht geladen werden."))
					End If

				Else
					m_SelectedEmployeeData = employeeMasterData
					m_SelectedEmployeeLoSettingData = employeeLOSettingData

					DisplayEmployeeData(employeeMasterData, employeeContactCommData)
				End If

			Else
				ResetEmployeeDetailData()
				m_SelectedEmployeeData = Nothing
				m_SelectedEmployeeLoSettingData = Nothing
			End If

			m_UCMediator.HandleChangeMandantEmployeeOrEmployee()

		End Sub

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

#Region "Helper Methods"

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
			'lblBewilligungValue.Text = String.Format("({0}) {1:dd.MM.yyyy}", m_UCMediator.CommonDbAccess.TranslatePermissionCode(employeeMasterData.Permission, m_InitializationData.UserData.UserLanguage), employeeMasterData.PermissionToDate)

			' Bewilligung warn icon
			iconPermissionWarning.Visible = employeeMasterData.PermissionToDate.HasValue AndAlso
											(employeeMasterData.PermissionToDate.Value.Date < DateTime.Now.Date) AndAlso
											Not String.IsNullOrWhiteSpace(employeeMasterData.Permission)

			' notice_advancedpayment
			Dim insertNewline As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_InitializationData.MDData.MDFormXMLFileName, String.Format("{0}/insertnewlineintoadvancedpaymentnotice", FORM_XML_MAIN_KEY)), False)
			Dim notice As String = String.Format("{0}", employeeMasterData.Notice_AdvancedPayment)
			If insertNewline.GetValueOrDefault(False) AndAlso Not String.IsNullOrWhiteSpace(notice) AndAlso Not notice.StartsWith(vbNewLine) Then
				notice = vbNewLine & notice
			End If
			txtZGComment.EditValue = String.Format("{0}", notice)
			txtZGComment.Visible = True

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
		''' Selects an advisor and add missing advisor
		''' </summary>
		''' <param name="lueAdvisor">The advisor lookup edit.</param>
		''' <param name="advisorKST">The advisor Kst.</param>
		Private Sub SelectAdvisor(lueAdvisor As LookUpEdit, advisorKST As String)
			Dim advisor = (From a In m_Advisors Where a.KST = advisorKST).FirstOrDefault
			If advisor Is Nothing Then
				'Add missing advisor
				m_Advisors.Add(New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = advisorKST})
			End If
			lueAdvisor.EditValue = advisorKST
		End Sub

#End Region

#Region "Preselection"

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

					Dim employeeDataList = CType(lueEmployee.Properties.DataSource, List(Of SP.DatabaseAccess.AdvancePaymentMng.DataObjects.EmployeeData))

					If employeeDataList.Any(Function(employee) employee.EmployeeNumber = PreselectionData.EmployeeNumber) Then
						lueEmployee.EditValue = PreselectionData.EmployeeNumber.Value
						preselectEmployeeSuccesful = True
					End If
				End If

				If PreselectionData.EmployeeNumber.HasValue AndAlso Not preselectEmployeeSuccesful Then
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Kandidat konnte nicht vorselektiert werden."))
				End If


				Dim selectadvisorkst As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(PreselectionData.MDNr),
																				 String.Format("{0}/selectadvisorkst", FORM_XML_MAIN_KEY)), False)

				Try
					' Advisor MA
					If (Not String.IsNullOrWhiteSpace(PreselectionData.Advisor)) Then
						SelectAdvisor(lueAdvisor1, If(PreselectionData.Advisor Is Nothing, m_InitializationData.UserData.UserKST, PreselectionData.Advisor))
					ElseIf selectadvisorkst Then
						SelectAdvisor(lueAdvisor1, m_InitializationData.UserData.UserKST)
					ElseIf Not m_SelectedEmployeeData Is Nothing Then
						SelectAdvisor(lueAdvisor1, m_SelectedEmployeeData.KST)
					End If
				Catch ex As Exception
					m_Logger.LogError(ex.ToString())
				End Try

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

#End Region

	End Class

End Namespace
