Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.Utils.Win
Imports System.Reflection
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten

Namespace UI

	''' <summary>
	''' Candidate and customer data.
	''' </summary>
	Public Class ucCandidateAndCustomer

#Region "Private Consts"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The employee database access.
		''' </summary>
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The mandant number.
		''' </summary>
		Private m_MandantNumber As Integer

		''' <summary>
		''' The emplyoee number.
		''' </summary>
		Private m_EmployeeNumber As Integer

		''' <summary>
		''' The emplyoee mobile number.
		''' </summary>
		Private m_EmployeeMobileNumber As String
		Private m_EmployeeMobile2Number As String

		''' <summary>
		''' The customer number.
		''' </summary>
		Private m_CustomerNumber As Integer

		''' <summary>
		''' The responsible number.
		''' </summary>
		''' <remarks></remarks>
		Private m_CresponsibleNumber As Integer

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_path As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			m_path = New ClsProgPath
			m_Mandant = New Mandant

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			AddHandler lueZHD.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub
#End Region

#Region "Public Properties"
		''' <summary>
		''' Gets the mandant number.
		''' </summary>
		''' <returns>The mandant number.</returns>
		Public ReadOnly Property MandantNumber As Integer
			Get
				Return m_MandantNumber
			End Get
		End Property

		''' <summary>
		''' Gets the employee number.
		''' </summary>
		''' <returns>The employee number.</returns>
		Public ReadOnly Property EmployeeNumber As Integer
			Get
				Return m_EmployeeNumber
			End Get
		End Property

		''' <summary>
		''' Gets the customer number.
		''' </summary>
		''' <returns>The customer number.</returns>
		Public ReadOnly Property CustomerNumber As Integer
			Get
				Return m_CustomerNumber
			End Get
		End Property

		Public ReadOnly Property CustomerResponsibleNumber As Integer?
			Get
				Return lueZHD.EditValue

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

			m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		End Sub

		''' <summary>
		''' Loads data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadData(ByVal esData As ESMasterData) As Boolean

			Dim success As Boolean = True

			m_SuppressUIEvents = True

			m_MandantNumber = esData.MDNr

			success = success AndAlso LoadDropDownData(esData.CustomerNumber)
			success = success AndAlso LoadEmployeeData(esData)
			success = success AndAlso LoadCustomerData(esData)

			m_SuppressUIEvents = False

			Return success
		End Function

		''' <summary>
		''' Merges ES master data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		Public Overrides Sub MergeESMasterData(ByVal esData As ESMasterData)

			esData.KDZHDNr = lueZHD.EditValue

		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_MandantNumber = 0
			m_EmployeeNumber = 0
			m_CustomerNumber = 0
			m_CresponsibleNumber = 0

			' Emplyoee
			hlnkEmployee.Text = String.Empty
			lblBirthdateValue.Text = String.Empty
			lblEmployeeAddressValue.Text = String.Empty
			lblQualificationValue.Text = String.Empty
			lblMAStateValue.Text = String.Empty

			' Customer
			hlnkCustomer.Text = String.Empty
			lblCustomerStreetValue.Text = String.Empty
			lblCustomerpostcodeValue.Text = String.Empty

			'  Reset drop downs and lists

			ResetResponsiblePersonDropDown()

		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Employee
			Me.grpCandidateData.Text = m_Translate.GetSafeTranslationValue(Me.grpCandidateData.Text)
			Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)
			btnSendSMS.ToolTip = m_Translate.GetSafeTranslationValue("Eine SMS-Nachricht senden") & "..."
			Me.lblGebDatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGebDatum.Text)
			Me.lblAdresseKandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresseKandidat.Text)
			Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)
			Me.lblMAStatus.Text = m_Translate.GetSafeTranslationValue(Me.lblMAStatus.Text)

			' Customer
			Me.grpCustomerData.Text = m_Translate.GetSafeTranslationValue(Me.grpCustomerData.Text)
			Me.lblKunde.Text = m_Translate.GetSafeTranslationValue(lblKunde.Text)
			Me.lblAdresse.Text = m_Translate.GetSafeTranslationValue(lblAdresse.Text)
			Me.lblZHD.Text = m_Translate.GetSafeTranslationValue(lblZHD.Text)

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
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData(ByVal customerNumber As Integer) As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadResponsiblePersonsDropDownData(customerNumber)
			Return success
		End Function

		''' <summary>
		''' Load responsible person drop down data.
		''' </summary>
		Private Function LoadResponsiblePersonsDropDownData(ByVal customerNumber As Integer) As Boolean
			Dim responsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonData(customerNumber)

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

			lueZHD.Properties.DataSource = responsiblePersonViewData

			Return Not responsiblePersonViewData Is Nothing
		End Function

		''' <summary>
		''' Loads employee detail data.
		''' </summary>
		''' <param name="esData">The ES master data.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeData(ByVal esData As ESMasterData) As Boolean
			m_EmployeeNumber = esData.EmployeeNumber

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(esData.EmployeeNumber, False)
			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(esData.EmployeeNumber)

			If employeeMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeitestammdaten konnten nicht geladen werden."))
				Return False
			End If
			grpCandidateData.Text = String.Format(m_Translate.GetSafeTranslationValue("Kandidat: {0}"), employeeMasterData.EmployeeNumber)

			If employeeContactCommData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter (KontaktKom) konnten nicht geladen werden."))
				Return False
			End If

			' Hyperlink
			hlnkEmployee.Text = String.Format("{0} {1}", employeeMasterData.Lastname, employeeMasterData.Firstname)
			Dim allowedEmployeeDetails As Boolean = True
			If Not String.IsNullOrWhiteSpace(m_InitializationData.UserData.UserFiliale) Then
				If Not String.IsNullOrWhiteSpace(employeeMasterData.MABusinessBranch) Then
					If Not employeeMasterData.MABusinessBranch.Contains(m_InitializationData.UserData.UserFiliale) Then
						allowedEmployeeDetails = False
					End If
				End If
			End If
			hlnkEmployee.Enabled = allowedEmployeeDetails


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

			' Show message is permission (Bewilligung) of emplyoee is not valid anymores
			If (employeeMasterData.PermissionToDate.HasValue AndAlso
																			(employeeMasterData.PermissionToDate.Value.Date < DateTime.Now.Date) AndAlso
																			Not employeeMasterData.Permission = "S") Then
				m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("Die Bewilligung des Kandidat ist abgelaufen ") + "({0:dd.MM.yyyy}).", employeeMasterData.PermissionToDate.Value.Date))
			End If

			btnSendSMS.Visible = Not String.IsNullOrWhiteSpace(employeeMasterData.MobilePhone) OrElse Not String.IsNullOrWhiteSpace(employeeMasterData.MobilePhone2)
			If Not String.IsNullOrWhiteSpace(employeeMasterData.MobilePhone) OrElse String.IsNullOrWhiteSpace(employeeMasterData.MobilePhone2) Then
				m_EmployeeMobileNumber = employeeMasterData.MobilePhone
				m_EmployeeMobile2Number = employeeMasterData.MobilePhone2
			End If
			btnNotice_Employment.Visible = Not String.IsNullOrWhiteSpace(employeeMasterData.Notice_Employment)
			txtNotice_Employment.EditValue = employeeMasterData.Notice_Employment


			Return True
		End Function

		''' <summary>
		''' Loads custmer detail data.
		''' </summary>
		''' <param name="esData">The ES master data.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadCustomerData(ByVal esData As ESMasterData) As Boolean
			m_CustomerNumber = esData.CustomerNumber

			Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(esData.CustomerNumber, m_InitializationData.UserData.UserFiliale)

			If customerMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter Detaildaten konnten nicht geladen werden."))
				Return False
			End If

			grpCustomerData.Text = String.Format(m_Translate.GetSafeTranslationValue("Kunde: {0}"), customerMasterData.CustomerNumber)
			hlnkCustomer.Text = String.Format("{0}", customerMasterData.Company1)
			lblCustomerStreetValue.Text = String.Format("{0}", customerMasterData.Street)
			lblCustomerpostcodeValue.Text = String.Format("{0} {1}", customerMasterData.Postcode, customerMasterData.Location)

			If customerMasterData.CreditWarning.GetValueOrDefault(False) AndAlso
				(customerMasterData.CreditLimit1 > 0 AndAlso customerMasterData.OpenInvoiceAmount >= customerMasterData.CreditLimit1) Or
				(customerMasterData.CreditLimit2 > 0 AndAlso customerMasterData.OpenInvoiceAmount >= customerMasterData.CreditLimit2) Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Achtung: Kunden-Kreditlimite wurde erreicht oder überschritten.{0}Offener Debitorenbetrag: {1:n2}{0}1. Kunden-Kreditlimite: {2:n2}{0}2. Kunden-Kreditlimite: {3:n2}")
				msg = String.Format(msg, vbNewLine, customerMasterData.OpenInvoiceAmount, customerMasterData.CreditLimit1, customerMasterData.CreditLimit2)

				m_UtilityUI.ShowInfoDialog(msg)

			End If
			lueZHD.EditValue = esData.KDZHDNr

			Dim allowedCustomerDetails As Boolean = True
			If Not String.IsNullOrWhiteSpace(m_InitializationData.UserData.UserFiliale) Then
				If Not String.IsNullOrWhiteSpace(customerMasterData.KDBusinessBranch) Then
					If Not customerMasterData.KDBusinessBranch.Contains(m_InitializationData.UserData.UserFiliale) Then
						allowedCustomerDetails = False
					End If
				End If
			End If
			hlnkCustomer.Enabled = allowedCustomerDetails

			lueZHD.Properties.Buttons(0).Enabled = allowedCustomerDetails
			lueZHD.Properties.Buttons(2).Enabled = allowedCustomerDetails
			lueZHD.Properties.ReadOnly =not allowedCustomerDetails

			btnCustomerNotice_Employment.Visible = Not String.IsNullOrWhiteSpace(customerMasterData.Notice_Employment)
			txtCustomerNotice_Employment.EditValue = customerMasterData.Notice_Employment

			Return True
		End Function

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
		''' Handles click on employee management hyperlink.
		''' </summary>
		Private Sub OnHlnkEmployee_OpenLink(sender As System.Object, e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles hlnkEmployee.OpenLink

			' Send a request to open a employeeMng form.
			Dim hub = MessageService.Instance.Hub
			Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_EmployeeNumber)
			hub.Publish(openEmployeeMng)

		End Sub

		''' <summary>
		''' Handles click on customer management hyperlink.
		''' </summary>
		Private Sub OnHlnkCustomer_OpenLink(sender As System.Object, e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles hlnkCustomer.OpenLink

			' Send a request to open a customerMng form.
			Dim hub = MessageService.Instance.Hub
			Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CustomerNumber)
			hub.Publish(openCustomerMng)

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

		Private Sub OnbtnSendSMS_Click(sender As Object, e As EventArgs) Handles btnSendSMS.Click
			Dim mobileDevice As Integer = 1
			Dim phonenumber As String = m_EmployeeMobileNumber
			If String.IsNullOrWhiteSpace(phonenumber) Then
				phonenumber = m_EmployeeMobile2Number
				mobileDevice = 2
			End If

			If Not String.IsNullOrWhiteSpace(phonenumber) Then
				OpeneCallSMS(m_InitializationData, phonenumber, m_EmployeeNumber, mobileDevice, Nothing, Nothing)
			End If

		End Sub

		Private Sub OpeneCallSMS(ByVal InitalData As SP.Infrastructure.Initialization.InitializeClass,
														 ByVal number As String, ByVal EmployeeNumber As Integer?, ByVal mobilekind As Integer,
														 ByVal CustomerNumber As Integer?,
														 ByVal ResponiblePersonNumber As Integer?)

			Dim continueSending As Boolean = True
			Dim sql As String = String.Empty

			Try
				Dim setting = New SPS.Export.Listing.Utility.InitializeClass With {.MDData = InitalData.MDData,
																																					 .PersonalizedData = InitalData.ProsonalizedData,
																																					 .TranslationData = InitalData.TranslationData,
																																					 .UserData = InitalData.UserData}
				Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, False)
				If employeeMasterData Is Nothing Then Return
				If employeeMasterData.MA_SMS_Mailing.GetValueOrDefault(False) Then
					Dim msg As String = "Achtung: Sie haben die Funktionalität für den SMS-Versand über Listing ausgeschaltet. Möchten Sie dennoch an den Kandidaten eine SMS-Nachricht senden?"
					continueSending = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("SMS-Versand"))

				End If
				If Not continueSending Then Return

				If EmployeeNumber.HasValue Then
					sql = "Select MA.MANr, MA.Nachname As Nachname, MA.Vorname As Vorname, "
					sql &= "MA.Strasse, MA.Land, MA.PLZ, MA.Ort, "
					'sql &= "( Case MA.MA_SMS_Mailing "
					'sql &= "When 0 Then MA.Natel{0} Else '' End) As Natel, "
					sql &= "MA.Natel{0} As Natel, "
					sql &= "ma.geschlecht AS Geschlecht, "
					sql &= "mak.briefanrede AS Anredeform "
					sql &= "From Mitarbeiter MA "
					sql &= "LEFT JOIN dbo.MAKontakt_Komm mak ON mak.manr = ma.manr "
					sql &= "Where (MA.Natel{0} <> '' And MA.Natel{0} Is Not Null ) "
					sql &= "And MA.MANr = {1}"

					sql = String.Format(sql, If(mobilekind = 1, "", "2"), EmployeeNumber)

					Dim frmSMS2eCall As New SPS.Export.Listing.Utility.frmSMS2eCall(setting, sql, SPS.Export.Listing.Utility.ReceiverType.Employee)
					frmSMS2eCall.LoadData()

					frmSMS2eCall.Show()
					frmSMS2eCall.BringToFront()
				End If


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub


#Region "notice"


		Private Sub OnbtnNotice_Employment_Click(sender As Object, e As EventArgs) Handles btnNotice_Employment.Click
			Dim insertNewline As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/insertnewlineintoemploymentnotice", FORM_XML_MAIN_KEY)), False)

			If insertNewline.GetValueOrDefault(False) AndAlso Not txtNotice_Employment.EditValue.ToString.StartsWith(vbNewLine) Then
				txtNotice_Employment.EditValue = vbNewLine & txtNotice_Employment.EditValue
			End If
			txtNotice_Employment.ShowPopup()

		End Sub

		Private Sub memoExEdit1_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles txtNotice_Employment.Popup
			Dim form As MemoExPopupForm = TryCast((TryCast(sender, IPopupControl)).PopupWindow, MemoExPopupForm)

			form.OkButton.Text = m_Translate.GetSafeTranslationValue("Speichern")

			RemoveHandler form.OkButton.Click, AddressOf edit_EditValueChangedOK
			AddHandler form.OkButton.Click, AddressOf edit_EditValueChangedOK

			Dim DropDownWindow As Control = CType(sender, IPopupControl).PopupWindow
			Dim FI As FieldInfo = GetType(MemoExPopupForm).GetField("memo", BindingFlags.NonPublic Or BindingFlags.Instance)
			Dim Edit As MemoEdit = CType(FI.GetValue(DropDownWindow), MemoEdit)
			Edit.SelectionStart = 0 'Edit.Text.Length
			Edit.SelectionLength = 0

		End Sub

		Private Sub edit_EditValueChangedOK(sender As Object, e As EventArgs)
			SaveEmploymentNoticesData()
		End Sub

		Private Sub SaveEmploymentNoticesData()

			Dim success As Boolean = True

			Try

				Dim employee = m_EmployeeDatabaseAccess.LoadEmployeeNoticesData(m_EmployeeNumber)
				employee.Notice_Employment = txtNotice_Employment.EditValue
				employee.ChangedFrom = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeNoticesData(employee)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub



		Private Sub OnbtnCustomerNotice_Employment_Click(sender As Object, e As EventArgs) Handles btnCustomerNotice_Employment.Click
			Dim insertNewline As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/insertnewlineintoemploymentnotice", FORM_XML_MAIN_KEY)), False)

			If insertNewline.GetValueOrDefault(False) AndAlso Not txtCustomerNotice_Employment.EditValue.ToString.StartsWith(vbNewLine) Then
				txtCustomerNotice_Employment.EditValue = vbNewLine & txtCustomerNotice_Employment.EditValue
			End If
			txtCustomerNotice_Employment.ShowPopup()

		End Sub

		Private Sub OnMemoExEdit1_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles txtCustomerNotice_Employment.Popup
			Dim form As MemoExPopupForm = TryCast((TryCast(sender, IPopupControl)).PopupWindow, MemoExPopupForm)

			form.OkButton.Text = m_Translate.GetSafeTranslationValue("Speichern")

			RemoveHandler form.OkButton.Click, AddressOf edit_CustomerEditValueChangedOK
			AddHandler form.OkButton.Click, AddressOf edit_CustomerEditValueChangedOK

			Dim DropDownWindow As Control = CType(sender, IPopupControl).PopupWindow
			Dim FI As FieldInfo = GetType(MemoExPopupForm).GetField("memo", BindingFlags.NonPublic Or BindingFlags.Instance)
			Dim Edit As MemoEdit = CType(FI.GetValue(DropDownWindow), MemoEdit)
			Edit.SelectionStart = 0
			Edit.SelectionLength = 0

		End Sub

		Private Sub edit_CustomerEditValueChangedOK(sender As Object, e As EventArgs)
			SaveCustomerNoticesData()
		End Sub

		Private Sub SaveCustomerNoticesData()

			Dim success As Boolean = True

			Try

				Dim customer = m_CustomerDatabaseAccess.LoadCustomerNoticesData(m_CustomerNumber)
				customer.Notice_Employment = txtCustomerNotice_Employment.EditValue
				customer.ChangedFrom = m_InitializationData.UserData.UserFullName

				success = success AndAlso m_CustomerDatabaseAccess.UpdateCustomerNoticesData(customer)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub


#End Region







		Private Sub lueZHD_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueZHD.ButtonClick
			If lueZHD.EditValue Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then

				Dim hub = MessageService.Instance.Hub
				Dim openResponsiblePersonMng As New OpenResponsiblePersonMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CustomerNumber, lueZHD.EditValue)
				hub.Publish(openResponsiblePersonMng)

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




#End Region


	End Class

End Namespace