Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.ComponentModel
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Namespace UI

	Public Class ucPageEmployeeBasicData


#Region "Private Consts"
		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
#End Region


#Region "Private Fields"

		''' <summary>
		''' The mandant data.
		''' </summary>
		Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData

		''' <summary>
		''' The advisor data.
		''' </summary>
		Private m_SelectedAdvisorData As SP.DatabaseAccess.Common.DataObjects.AdvisorData

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_MandantData As Mandant

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			Try
				m_ProgPath = New ClsProgPath
				m_MandantData = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			AddHandler txtLastName.TextChanged, AddressOf OnControl_TextChanged
			AddHandler txtFirstname.TextChanged, AddressOf OnControl_TextChanged
			AddHandler txtStreet.TextChanged, AddressOf OnControl_TextChanged
			AddHandler txtLocation.TextChanged, AddressOf OnControl_TextChanged
			AddHandler lueCountry.TextChanged, AddressOf OnControl_TextChanged
			AddHandler luePostcode.TextChanged, AddressOf OnControl_TextChanged

			AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueGender.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueNationality.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCivilstate.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditBirthday.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLanguage.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected employee basic data. data.
		''' </summary>
		''' <returns>Employee basic data.</returns>
		Public ReadOnly Property SelectedEmployeeBasicData As InitEmployeeBasicData
			Get

				Dim data As New InitEmployeeBasicData With {
					.Lastname = txtLastName.Text,
					.Firstname = txtFirstname.Text,
					.Street = txtStreet.Text,
					.CountryCode = lueCountry.EditValue,
					.PostCode = luePostcode.EditValue,
					.Location = txtLocation.Text,
					.Gender = lueGender.EditValue,
					.Nationality = lueNationality.EditValue,
					.CivilState = lueCivilstate.EditValue,
					.Birthdate = dateEditBirthday.EditValue,
					.Language = lueLanguage.EditValue
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
				success = success AndAlso LoadDropDownData()

			End If

			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()
			m_IsFirstPageActivation = True

			Dim supressUIState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			txtLastName.Text = String.Empty
			txtLastName.Properties.MaxLength = 255

			txtFirstname.Text = String.Empty
			txtFirstname.Properties.MaxLength = 255

			txtStreet.Text = String.Empty
			txtStreet.Properties.MaxLength = 70

			txtLocation.Text = String.Empty
			txtLocation.Properties.MaxLength = 70

			dateEditBirthday.EditValue = Nothing
			lblAge.Text = String.Empty

			m_SuppressUIEvents = supressUIState

			ResetCountryDropDown()
			ResetPostcodeDropDown()

			ResetGenderDropDown()
			ResetNationalityDropDown()
			ResetCivilStateDropDown()
			ResetLanguageDropDown()

			ResetGridExistingEmployees()

		End Sub

		''' <summary>
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetCountryDropDown()

			lueCountry.Properties.ShowHeader = False
			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 10

			lueCountry.Properties.DisplayMember = "Translated_Value"
			lueCountry.Properties.ValueMember = "Code"

			Dim columns = lueCountry.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Land")))

			lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCountry.Properties.SearchMode = SearchMode.AutoComplete
			lueCountry.Properties.AutoSearchColumnIndex = 0
			lueCountry.Properties.NullText = String.Empty

		End Sub

		''' <summary>
		''' Resets the postcode drop down.
		''' </summary>
		Private Sub ResetPostcodeDropDown()

			luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
			luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

			luePostcode.Properties.ShowHeader = False
			luePostcode.Properties.ShowFooter = False
			luePostcode.Properties.DropDownRows = 10

			luePostcode.Properties.DisplayMember = "Postcode"
			luePostcode.Properties.ValueMember = "Postcode"

			Dim columns = luePostcode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Postcode", 0))
			columns.Add(New LookUpColumnInfo("Location", 0))

			luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePostcode.Properties.SearchMode = SearchMode.AutoComplete
			luePostcode.Properties.AutoSearchColumnIndex = 1
			luePostcode.Properties.NullText = String.Empty
			luePostcode.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the gender drop down.
		''' </summary>
		Public Sub ResetGenderDropDown()

			lueGender.Properties.ShowHeader = False
			lueGender.Properties.ShowFooter = False
			lueGender.Properties.DropDownRows = 10

			lueGender.Properties.DisplayMember = "TranslatedGender"
			lueGender.Properties.ValueMember = "RecValue"

			Dim columns = lueGender.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedGender", 0, ("Geschlecht")))

			lueGender.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueGender.Properties.SearchMode = SearchMode.AutoComplete
			lueGender.Properties.AutoSearchColumnIndex = 0

			lueGender.Properties.NullText = String.Empty
			lueGender.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the nationality drop down.
		''' </summary>
		Private Sub ResetNationalityDropDown()

			lueNationality.Properties.ShowHeader = False
			lueNationality.Properties.ShowFooter = False
			lueNationality.Properties.DropDownRows = 50
			lueNationality.Properties.DisplayMember = "Translated_Value"
			lueNationality.Properties.ValueMember = "Code"

			Dim columns = lueNationality.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Land")))

			lueNationality.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueNationality.Properties.SearchMode = SearchMode.AutoComplete
			lueNationality.Properties.AutoSearchColumnIndex = 0

			lueNationality.Properties.NullText = String.Empty
			lueNationality.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the civil state down.
		''' </summary>
		Private Sub ResetCivilStateDropDown()

			lueCivilstate.Properties.DisplayMember = "TranslatedCivilState"
			lueCivilstate.Properties.ValueMember = "GetField"

			Dim columns = lueCivilstate.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))
			columns.Add(New LookUpColumnInfo("TranslatedCivilState", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueCivilstate.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCivilstate.Properties.SearchMode = SearchMode.AutoComplete
			lueCivilstate.Properties.AutoSearchColumnIndex = 1
			lueCivilstate.Properties.NullText = String.Empty
			lueCivilstate.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the language drop down.
		''' </summary>
		Public Sub ResetLanguageDropDown()

			lueLanguage.Properties.ShowHeader = False
			lueLanguage.Properties.ShowFooter = False
			lueLanguage.Properties.DropDownRows = 10

			lueLanguage.Properties.DisplayMember = "TranslatedDescription"
			lueLanguage.Properties.ValueMember = "Description"

			Dim columns = lueLanguage.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedDescription", 0, m_Translate.GetSafeTranslationValue("Sprache")))

			lueLanguage.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueLanguage.Properties.SearchMode = SearchMode.AutoComplete
			lueLanguage.Properties.AutoSearchColumnIndex = 0

			lueLanguage.Properties.NullText = String.Empty
			lueLanguage.EditValue = Nothing
		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			errorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(txtLastName, errorProvider, String.IsNullOrEmpty(txtLastName.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(txtFirstname, errorProvider, String.IsNullOrEmpty(txtFirstname.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(txtStreet, errorProvider, String.IsNullOrEmpty(txtStreet.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueCountry, errorProvider, lueCountry.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(luePostcode, errorProvider, luePostcode.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(txtLocation, errorProvider, String.IsNullOrEmpty(txtLocation.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(lueGender, errorProvider, lueGender.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(lueNationality, errorProvider, lueNationality.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(lueCivilstate, errorProvider, lueCivilstate.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(dateEditBirthday, errorProvider, dateEditBirthday.EditValue Is Nothing Or Not DateTime.TryParse(dateEditBirthday.EditValue, Nothing), errorText)
			isValid = isValid And SetErrorIfInvalid(lueLanguage, errorProvider, lueLanguage.EditValue Is Nothing, errorText)

			Return isValid

		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Group basic data.
			Me.grpBasicData.Text = m_Translate.GetSafeTranslationValue(Me.grpBasicData.Text)

			Me.lblNachname.Text = m_Translate.GetSafeTranslationValue(Me.lblNachname.Text)
			Me.lblVorname.Text = m_Translate.GetSafeTranslationValue(Me.lblVorname.Text)
			Me.lblStrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblStrasse.Text)
			Me.lblland.Text = m_Translate.GetSafeTranslationValue(Me.lblland.Text)
			Me.lblplz.Text = m_Translate.GetSafeTranslationValue(Me.lblplz.Text)
			Me.lblort.Text = m_Translate.GetSafeTranslationValue(Me.lblort.Text)
			Me.lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(Me.lblGeschlecht.Text)
			Me.lblNationalitaet.Text = m_Translate.GetSafeTranslationValue(Me.lblNationalitaet.Text)
			Me.lblZivilstand.Text = m_Translate.GetSafeTranslationValue(Me.lblZivilstand.Text)
			Me.lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGeburtsdatum.Text)
			Me.lblSprache.Text = m_Translate.GetSafeTranslationValue(Me.lblSprache.Text)

			' Group existing employee data.
			Me.grpExistingEmployees.TabIndex = m_Translate.GetSafeTranslationValue(Me.grpExistingEmployees.TabIndex)

		End Sub

		''' <summary>
		''' Resets the existing employees grid.
		''' </summary>
		Private Sub ResetGridExistingEmployees()

			' Reset the grid

			gvExistingEmployees.OptionsView.ShowIndicator = False

			gvExistingEmployees.Columns.Clear()

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnName.Name = "LastnameFirstname"
			columnName.FieldName = "LastnameFirstname"
			columnName.Visible = True
			gvExistingEmployees.Columns.Add(columnName)

			Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnAddress.Name = "Address"
			columnAddress.FieldName = "Address"
			columnAddress.Visible = True
			gvExistingEmployees.Columns.Add(columnAddress)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnAdvisor.Name = "employeeadvisor"
			columnAdvisor.FieldName = "employeeadvisor"
			columnAdvisor.Visible = True
			gvExistingEmployees.Columns.Add(columnAdvisor)

			grdExistingEmployees.DataSource = Nothing

		End Sub

		''' <summary>
		''' Loads drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadCountryAndNationalityDropDownData()
			success = success AndAlso LoadPostcodeDropDownData()
			success = success AndAlso LoadGenderDropDownData()
			success = success AndAlso LoadCivilStateDropDownData()
			success = success AndAlso LoadLanguageDropDownData()

			Return success
		End Function

		''' <summary>
		''' Loads country and nationality drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCountryAndNationalityDropDownData() As Boolean

			Dim mdnr = m_UCMediator.SelectedMandantAndAdvisorData.MandantData.MandantNumber
			Dim xmlFilename As String = m_MandantData.GetSelectedMDFormDataXMLFilename(mdnr)
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
				lueCountry.EditValue = "CH"

				lueNationality.Properties.DataSource = countryData
				lueNationality.Properties.ForceInitialize()
				Dim employeenationality As String = m_ProgPath.GetXMLNodeValue(xmlFilename, String.Format("{0}/employeenationality", FORM_XML_MAIN_KEY))

				lueNationality.EditValue = employeenationality '  "CH"

			Catch ex As Exception
				m_Logger.LogError(String.Format("lueCountry: {0}", ex.ToString))

			End Try

			'Dim countryData = m_UCMediator.CommonDbAccess.LoadCountryData()

			'If countryData Is Nothing Then
			'	m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
			'End If

			'lueCountry.Properties.DataSource = countryData
			'lueCountry.Properties.ForceInitialize()
			'lueCountry.EditValue = "CH"

			'lueNationality.Properties.DataSource = countryData
			'lueNationality.Properties.ForceInitialize()

			'Dim employeenationality As String = m_ProgPath.GetXMLNodeValue(xmlFilename, String.Format("{0}/employeenationality", FORM_XML_MAIN_KEY))

			'lueNationality.EditValue = employeenationality '  "CH"

			Return Not countryData Is Nothing
		End Function

		''' <summary>
		''' Loads post code drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadPostcodeDropDownData() As Boolean

			Dim postcodeData = m_UCMediator.CommonDbAccess.LoadPostcodeData()

			If postcodeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleitzahlen konnten nicht geladen werden."))
			End If

			luePostcode.Properties.DataSource = postcodeData
			luePostcode.Properties.DropDownRows = Math.Min(20, postcodeData.Count)

			luePostcode.Properties.ForceInitialize()

			Return Not postcodeData Is Nothing
		End Function

		''' <summary>
		''' Loads gender data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadGenderDropDownData() As Boolean

			Dim genderData = m_UCMediator.CommonDbAccess.LoadGenderData()

			If genderData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Geschlechtsbezeichnungen konnten nicht geladen werden."))
			End If

			lueGender.Properties.DataSource = genderData
			lueGender.Properties.DropDownRows = Math.Min(20, genderData.Count)

			lueGender.Properties.ForceInitialize()

			Return Not genderData Is Nothing
		End Function

		''' <summary>
		''' Loads civilstate drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCivilStateDropDownData() As Boolean

			Dim civilStateData = m_UCMediator.CommonDbAccess.LoadCivilStateData()

			If civilStateData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zivistatusbezeichnungen konnten nicht geladen werden."))
			End If

			lueCivilstate.Properties.DataSource = civilStateData
			lueCivilstate.Properties.DropDownRows = Math.Min(20, civilStateData.Count)

			lueCivilstate.Properties.ForceInitialize()

			Return Not civilStateData Is Nothing
		End Function

		''' <summary>
		''' Loads language drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadLanguageDropDownData() As Boolean

			Dim languageData = m_UCMediator.CommonDbAccess.LoadLanguageData()

			If languageData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprachbezeichnungen konnten nicht geladen werden."))
			End If

			lueLanguage.Properties.DataSource = languageData
			lueLanguage.Properties.DropDownRows = Math.Min(20, languageData.Count)

			lueLanguage.Properties.ForceInitialize()

			Return Not languageData Is Nothing
		End Function


		''' <summary>
		''' Searches for existing employees.
		''' </summary>
		''' <param name="lastName">The last name.</param>
		''' <param name="firstName">The first name.</param>
		''' <param name="street">The street.</param>
		''' <param name="postCode">The post code.</param>
		''' <param name="location">The location.</param>
		''' <param name="countryCode">The country code.</param>
		Private Sub SearchExistingEmployeeData(ByVal lastName As String,
																					 ByVal firstName As String,
																					 ByVal street As String,
																					 ByVal postCode As String,
																					 ByVal location As String,
																					 ByVal countryCode As String)

			Dim existingEmployeeData As IEnumerable(Of ExistingEmployeeSearchData) = m_UCMediator.EmployeeDbAccess.LoadExistingEmployeesBySearchCriteria(lastName, firstName, street, postCode, location, countryCode)

			Dim existingEmployeesGridData = (From employeeData In existingEmployeeData
																			 Select New ExistingEmployeeGridViewItem With
																							{.Lastname = employeeData.Lastname,
																							 .Firstname = employeeData.Firstname,
																							 .Street = employeeData.Street,
																							 .CountryCode = employeeData.CountryCode,
																							 .Postcode = employeeData.Postcode,
																							 .Location = employeeData.Location,
																							 .employeekst = employeeData.employeeKST,
																							 .employeeadvisor = employeeData.employeeAdvisor}).ToList()


			Dim listDataSource As BindingList(Of ExistingEmployeeGridViewItem) = New BindingList(Of ExistingEmployeeGridViewItem)

			For Each employeerGridData In existingEmployeesGridData

				listDataSource.Add(employeerGridData)

			Next

			grdExistingEmployees.DataSource = listDataSource

		End Sub

		''' <summary>
		''' Handles change of country code.
		''' </summary>
		Private Sub OnLueCountry_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueCountry.EditValueChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			m_UCMediator.CountryCodeHasChanged()

		End Sub

		''' <summary>
		''' Handles change of nationality.
		''' </summary>
		Private Sub OnLueNationality_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueNationality.EditValueChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			m_UCMediator.NationalityHasChanged()

		End Sub

		''' <summary>
		''' Handles change of postcode.
		''' </summary>
		Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

			Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)
			If lueCountry Is Nothing OrElse lueCountry.EditValue <> "CH" Then
				txtLocation.Text = Nothing

				Return
			End If
			If Not postCodeData Is Nothing Then
				txtLocation.Text = postCodeData.Location
			End If

		End Sub

		''' <summary>
		''' Handles new value event on postcode(plz) lookup edit.
		''' </summary>
		Private Sub OnLuePostcode_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles luePostcode.ProcessNewValue

			If Not luePostcode.Properties.DataSource Is Nothing AndAlso Not String.IsNullOrWhiteSpace(e.DisplayValue.ToString()) Then

				Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

				Dim newPostcode As New PostCodeData With {.Postcode = e.DisplayValue.ToString()}
				listOfPostcode.Add(newPostcode)

				e.Handled = True
			End If
		End Sub

		''' <summary>
		''' Handles leave event of birthdate control.
		''' </summary>
		Private Sub OnDateEditBirthday_Leave(sender As System.Object, e As System.EventArgs) Handles dateEditBirthday.Leave
			RecalculateAge()
		End Sub

		''' <summary>
		''' Handles controls lost focus event.
		''' </summary>
		Private Sub OnControl_TextChanged(sender As System.Object, e As System.EventArgs)

			If m_SuppressUIEvents Then
				Return
			End If

			SearchExistingEmployeeData(txtLastName.Text, txtFirstname.Text, txtStreet.Text, luePostcode.EditValue, txtLocation.Text, String.Empty) 'lueCountry.EditValue)

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

		''' <summary>
		''' Recalculates the age.
		''' </summary>
		Private Sub RecalculateAge()
			If dateEditBirthday.EditValue Is Nothing Then
				lblAge.Text = String.Empty
			Else
				lblAge.Text = GetAge(dateEditBirthday.EditValue)
			End If

		End Sub

		''' <summary>
		''' Gets the age in years.
		''' </summary>
		''' <param name="birthDate">The birthdate.</param>
		''' <returns>Age in years.</returns>
		Private Function GetAge(ByVal birthDate As DateTime)

			' Get year diff
			Dim years As Integer = DateTime.Now.Year - birthDate.Year

			birthDate = birthDate.AddYears(years)

			' Subtract another year if its a day before the the birth day
			If (DateTime.Today.CompareTo(birthDate) < 0) Then
				years = years - 1
			End If

			Return years

		End Function

#End Region

#Region "View helper classes"

		''' <summary>
		''' View data for existing employees.
		''' </summary>
		Private Class ExistingEmployeeGridViewItem

			Public Property Lastname As String
			Public Property Firstname As String

			Public Property Street As String
			Public Property CountryCode As String
			Public Property Postcode As String
			Public Property Location As String

			Public Property employeekst As String
			Public Property employeeadvisor As String


			Public ReadOnly Property LastnameFirstname
				Get
					Return String.Format("{0} {1}", Lastname, Firstname)
				End Get
			End Property

			Public ReadOnly Property Address As String
				Get
					Return String.Format("{0} {1}-{2}, {3}", Street, CountryCode, Postcode, Location)
				End Get
			End Property

		End Class

#End Region

	End Class

End Namespace
