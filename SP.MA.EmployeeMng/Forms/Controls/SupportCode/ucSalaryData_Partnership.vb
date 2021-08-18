
Imports System.ComponentModel
Imports System.Globalization
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Namespace UI

	Partial Class ucSalaryData


		Private Function LoadPartnerDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadCountryDropDownData()
			success = success AndAlso LoadPostcodeDropDownData()

			success = success AndAlso LoadGenderDropDownData()

			Return success
		End Function


#Region "reseting controls"

		Private Sub ResetPartnershipGrid()

			gvPartnership.OptionsView.ShowIndicator = False
			gvPartnership.OptionsView.ShowAutoFilterRow = False
			gvPartnership.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvPartnership.OptionsView.ShowFooter = False
			gvPartnership.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvPartnership.Columns.Clear()

			Dim columnFullName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFullName.OptionsColumn.AllowEdit = False
			columnFullName.Caption = m_Translate.GetSafeTranslationValue("Name, Vorname")
			columnFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnFullName.Name = "FullName"
			columnFullName.FieldName = "FullName"
			columnFullName.Visible = True
			columnFullName.Width = 200
			gvPartnership.Columns.Add(columnFullName)

			Dim columnBirthdate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBirthdate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBirthdate.OptionsColumn.AllowEdit = False
			columnBirthdate.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
			columnBirthdate.Name = "Birthdate"
			columnBirthdate.FieldName = "Birthdate"
			columnBirthdate.Width = 50
			columnBirthdate.Visible = True
			gvPartnership.Columns.Add(columnBirthdate)

			Dim columnValidFromTo As New DevExpress.XtraGrid.Columns.GridColumn()
			columnValidFromTo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnValidFromTo.OptionsColumn.AllowEdit = False
			columnValidFromTo.Caption = m_Translate.GetSafeTranslationValue("Gültig")
			columnValidFromTo.Name = "ValidFromTo"
			columnValidFromTo.FieldName = "ValidFromTo"
			columnValidFromTo.Visible = True
			columnValidFromTo.Width = 50
			gvPartnership.Columns.Add(columnValidFromTo)

			Dim columnAsEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAsEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAsEmployee.OptionsColumn.AllowEdit = False
			columnAsEmployee.Caption = m_Translate.GetSafeTranslationValue("Als KandidatIn")
			columnAsEmployee.Name = "AsEmployee"
			columnAsEmployee.FieldName = "AsEmployee"
			columnAsEmployee.Visible = True
			columnAsEmployee.Width = 50
			gvPartnership.Columns.Add(columnAsEmployee)

			Dim columnActiv As New DevExpress.XtraGrid.Columns.GridColumn()
			columnActiv.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnActiv.OptionsColumn.AllowEdit = False
			columnActiv.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
			columnActiv.Name = "Activ"
			columnActiv.FieldName = "Activ"
			columnActiv.Visible = True
			columnActiv.Width = 50
			gvPartnership.Columns.Add(columnActiv)

			Dim columnInEmployment As New DevExpress.XtraGrid.Columns.GridColumn()
			columnInEmployment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnInEmployment.OptionsColumn.AllowEdit = False
			columnInEmployment.Caption = m_Translate.GetSafeTranslationValue("Erwerbstätig")
			columnInEmployment.Name = "InEmployment"
			columnInEmployment.FieldName = "InEmployment"
			columnInEmployment.Visible = True
			columnInEmployment.Width = 50
			gvPartnership.Columns.Add(columnInEmployment)


			grdPartnership.DataSource = Nothing

		End Sub

		Private Sub ResetEmployeeDropDown()

			lueExistingEmployee.Properties.DisplayMember = "EmployeeFullnameWithComma"
			lueExistingEmployee.Properties.ValueMember = "EmployeeNumber"

			gvExistingEmployee.OptionsView.ShowIndicator = False
			gvExistingEmployee.OptionsView.ShowColumnHeaders = True
			gvExistingEmployee.OptionsView.ShowFooter = False

			gvExistingEmployee.OptionsView.ShowAutoFilterRow = True
			gvExistingEmployee.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvExistingEmployee.Columns.Clear()

			Dim columnEmployeeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeeNumber.Caption = m_Translate.GetSafeTranslationValue("Nr")
			columnEmployeeNumber.Name = "EmployeeNumber"
			columnEmployeeNumber.FieldName = "EmployeeNumber"
			columnEmployeeNumber.Visible = True
			columnEmployeeNumber.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvExistingEmployee.Columns.Add(columnEmployeeNumber)

			Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnLastnameFirstname.Name = "EmployeeFullnameWithComma"
			columnLastnameFirstname.FieldName = "EmployeeFullnameWithComma"
			columnLastnameFirstname.Visible = True
			columnLastnameFirstname.Width = 200
			gvExistingEmployee.Columns.Add(columnLastnameFirstname)

			Dim columnPostcodeAndLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnPostcodeAndLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnPostcodeAndLocation.Caption = m_Translate.GetSafeTranslationValue("Ardresse")
			columnPostcodeAndLocation.Name = "EmployeeAddress"
			columnPostcodeAndLocation.FieldName = "EmployeeAddress"
			columnPostcodeAndLocation.Visible = True
			columnPostcodeAndLocation.Width = 300
			gvExistingEmployee.Columns.Add(columnPostcodeAndLocation)

			lueExistingEmployee.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueExistingEmployee.Properties.NullText = String.Empty
			lueExistingEmployee.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetCountryDropDown()

			lueCountry.Properties.ShowHeader = False
			lueCountry.Properties.ShowFooter = False
			lueCountry.Properties.DropDownRows = 20
			lueCountry.Properties.DisplayMember = "Translated_Value"
			lueCountry.Properties.ValueMember = "Code"

			Dim columns = lueCountry.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Land")))

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

			luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
			luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

			luePostcode.Properties.DisplayMember = "Postcode"
			luePostcode.Properties.ValueMember = "Postcode"

			Dim columns = luePostcode.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Postcode", 0, m_Translate.GetSafeTranslationValue("PLZ")))
			columns.Add(New LookUpColumnInfo("Location", 0, m_Translate.GetSafeTranslationValue("Ort")))

			luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			luePostcode.Properties.SearchMode = SearchMode.AutoComplete
			luePostcode.Properties.AutoSearchColumnIndex = 1
			luePostcode.Properties.NullText = String.Empty
			luePostcode.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the gender drop down.
		''' </summary>
		Private Sub ResetGenderDropDown()

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


#End Region


#Region "loading partnership data"

		Private Function LoadExistingEmployeeDropDownData(ByVal employeeNumber As Integer) As Boolean

			Dim employeeData = m_EmployeeDataAccess.LoadExistingEmployeeForSelectingPartnershipData(employeeNumber)

			If employeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidatendaten konnen nicht geladen werden."))
				Return False
			End If

			lueExistingEmployee.EditValue = Nothing
			lueExistingEmployee.Properties.DataSource = employeeData


			Return True

		End Function

		Private Function LoadCountryDropDownData() As Boolean
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
				m_Logger.LogError(String.Format("lueCountry: {0}", ex.ToString))

			End Try

			Return Not countryData Is Nothing
		End Function

		''' <summary>
		''' Loads the postcode drop downdata.
		''' </summary>
		Private Function LoadPostcodeDropDownData() As Boolean
			Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

			If (postcodeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
			End If

			luePostcode.Properties.DataSource = postcodeData
			luePostcode.Properties.ForceInitialize()

			Return Not postcodeData Is Nothing
		End Function

		''' <summary>
		''' Loads gender drop down data.
		''' </summary>
		Private Function LoadGenderDropDownData() As Boolean
			Dim genderData = m_CommonDatabaseAccess.LoadGenderData()

			If (genderData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Geschlechtsdaten konnten nicht geladen werden."))
			End If

			lueGender.Properties.DataSource = genderData
			lueGender.Properties.ForceInitialize()

			Return Not genderData Is Nothing
		End Function


#End Region



		Private Function LoadPartnershipData(ByVal employeeNumber As Integer) As Boolean

			Dim success As Boolean = True
			Dim listDataSource As BindingList(Of EmployeePartnershipData) = New BindingList(Of EmployeePartnershipData)

			Try
				Dim employeePartnerData = m_EmployeeDataAccess.LoadEmployeePartnershipData(employeeNumber)

				If (employeePartnerData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Partner Daten konnten nicht geladen werden."))

					Return False
				End If

				Dim gridData = (From person In employeePartnerData
								Select New EmployeePartnershipData With {.ID = person.ID,
									.AddressNumber = person.AddressNumber,
									.EmployeeNumber = person.EmployeeNumber,
									.ExistingEmployeeNumber = person.ExistingEmployeeNumber,
									.Gender = person.Gender,
									.Lastname = person.Lastname,
									.Firstname = person.Firstname,
									.Street = person.Street,
									.PostOfficeBox = person.PostOfficeBox,
									.Postcode = person.Postcode,
									.City = person.City,
									.Country = person.Country,
									.SocialInsuranceNumber = person.SocialInsuranceNumber,
									.Birthdate = person.Birthdate,
									.InEmployment = person.InEmployment,
									.ValidFrom = person.ValidFrom,
									.ValidTo = person.ValidTo,
									.Createdfrom = person.Createdfrom,
									.CreatedUserNumber = person.CreatedUserNumber,
									.CreatedOn = person.CreatedOn,
									.ChangedFrom = person.ChangedFrom,
									.ChangedUserNumber = person.ChangedUserNumber,
									.ChangedOn = person.ChangedOn
									}).ToList()


				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = True

				For Each p In gridData
					listDataSource.Add(p)
				Next
				grdPartnership.DataSource = listDataSource


				m_SuppressUIEvents = supressUIEventState


			Catch ex As Exception
				m_Logger.LogError(Err.ToString)

				Return False
			End Try

			Return Not listDataSource Is Nothing

		End Function

		Private Function SelectedPartnershipRecord() As EmployeePartnershipData
			Dim gvAssigned = TryCast(grdPartnership.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvAssigned Is Nothing) Then
				Dim selectedRows = gvAssigned.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvAssigned.GetRow(selectedRows(0)), EmployeePartnershipData)

					Return employee
				End If

			End If

			Return Nothing

		End Function

		Private Sub btnAddPartner_Click(sender As Object, e As EventArgs) Handles btnAddPartner.Click

			m_PartnershipID = Nothing
			LoadAdornerPertnershipForm(m_PartnershipID)

		End Sub

		Private Sub OngvPartnership_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvPartnership.DoubleClick
			Dim selectedData = SelectedPartnershipRecord()

			If Not selectedData Is Nothing AndAlso (selectedData.ID.GetValueOrDefault(0) > 0) Then
				m_PartnershipID = selectedData.ID
			Else
				m_PartnershipID = Nothing
			End If
			LoadAdornerPertnershipForm(m_PartnershipID)

		End Sub

		Private Sub OnfpPartnerDetailData_Hidden(sender As Object, e As FlyoutPanelEventArgs) Handles fpPartnerDetailData.Hidden
			grpBewilligung.Visible = True
			Me.Focus()
			'Me.Enabled = True
		End Sub

		Private Sub fpPartnerDetailData_Shown(sender As Object, e As FlyoutPanelEventArgs) Handles fpPartnerDetailData.Shown
			'Me.Enabled = False
			grpBewilligung.Visible = False
			fpPartnerDetailData.OptionsButtonPanel.Buttons(3).Properties.Enabled = m_PartnershipID.HasValue

			For Each button In fpPartnerDetailData.OptionsButtonPanel.Buttons
				button.caption = m_Translate.GetSafeTranslationValue(button.caption)
			Next


		End Sub

		Private Sub LoadAdornerPertnershipForm(ByVal recID As Integer?)

			Try
				AdornerUIManager1.Elements.Remove(m_PartnershipBadge)
				AdornerUIManager1.Hide()
				ResetBeakControls()

				fpPartnerDetailData.OwnerControl = grdPartnership
				fpPartnerDetailData.OptionsBeakPanel.CloseOnOuterClick = False
				fpPartnerDetailData.OptionsBeakPanel.AnimationType = Win.PopupToolWindowAnimation.Fade
				fpPartnerDetailData.OptionsBeakPanel.BeakLocation = BeakPanelBeakLocation.Right

				fpPartnerDetailData.ShowBeakForm()

				If m_PartnershipID.GetValueOrDefault(0) > 0 Then
					LoadAssignedPartnershipData(m_PartnershipID)
				Else
					ResetExistingEmployeeDetailData()
				End If

				m_SuppressUIEvents = False


			Catch ex As Exception
				m_Logger.LogError(Err.ToString)

			End Try

		End Sub

		Private Sub ResetBeakControls()
			pnlExistingEmployee.Enabled = True

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			lueExistingEmployee.EditValue = Nothing

			lueGender.EditValue = Nothing
			txtLastname.EditValue = String.Empty
			txtFirstname.EditValue = String.Empty
			txtStreet.EditValue = String.Empty
			lueCountry.EditValue = Nothing
			luePostcode.EditValue = Nothing
			txtCity.EditValue = String.Empty
			txtSocialInsuranceNumber.EditValue = String.Empty
			dateEditBirthday.EditValue = Nothing
			chkInEmployment.Checked = False
			dateEditValidFrom.EditValue = Nothing
			dateEditValidTo.EditValue = Nothing

			pnlExistingEmployee.Enabled = True


			m_SuppressUIEvents = m_SuppressUIEvents

		End Sub

		Private Sub OnfpPartnerDetailData_ButtonClick(sender As Object, e As FlyoutPanelButtonClickEventArgs) Handles fpPartnerDetailData.ButtonClick
			Dim result As Boolean = True

			Try
				Dim tag As Integer = Val(e.Button.Tag)

				Select Case e.Button.Tag
					Case 0
						fpPartnerDetailData.HideBeakForm(True)
						Me.BringToFront()
						Me.Focus()

					Case 1
						' save data
						result = result AndAlso SaveAssignedPartnershipData(m_PartnershipID)

					Case 2
						' new data, blank form
						m_PartnershipID = Nothing
						ResetExistingEmployeeDetailData()
						Return

					Case 3
						' delete form data
						result = result AndAlso m_PartnershipID.HasValue AndAlso DeleteAssignedPartnershipData(m_PartnershipID)


					Case Else
						Return

				End Select

				If (tag = 1 OrElse tag = 3) AndAlso result Then
					result = result AndAlso LoadPartnershipData(m_EmployeeNumber)

					If tag = 1 AndAlso result Then FocusPartnershipData(m_PartnershipID)
				End If

			Catch ex As Exception
				m_Logger.LogError(Err.ToString)

				result = False
			End Try

		End Sub

		Private Function LoadAssignedPartnershipData(ByVal recID As Integer) As Boolean
			Dim result As Boolean = True

			Try

				Dim partnershipData = m_EmployeeDataAccess.LoadEmployeeAssignedPartnershipData(recID)
				If (partnershipData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Partner Daten konnten nicht geladen werden."))

					Return False
				End If

				Dim suppressUIEventsState = m_SuppressUIEvents
				m_SuppressUIEvents = True
				lueExistingEmployee.EditValue = partnershipData.ExistingEmployeeNumber

				lueGender.EditValue = partnershipData.Gender
				txtLastname.EditValue = partnershipData.Lastname
				txtFirstname.EditValue = partnershipData.Firstname
				txtStreet.EditValue = partnershipData.Street
				lueCountry.EditValue = partnershipData.Country
				luePostcode.EditValue = partnershipData.Postcode
				txtCity.EditValue = partnershipData.City
				txtSocialInsuranceNumber.EditValue = partnershipData.SocialInsuranceNumber
				dateEditBirthday.EditValue = partnershipData.Birthdate
				chkInEmployment.Checked = partnershipData.InEmployment.GetValueOrDefault(False)
				dateEditValidFrom.EditValue = partnershipData.ValidFrom
				dateEditValidTo.EditValue = partnershipData.ValidTo

				pnlExistingEmployee.Enabled = Not partnershipData.AsEmployee

				m_SuppressUIEvents = m_SuppressUIEvents


			Catch ex As Exception
				m_Logger.LogError(Err.ToString)

				result = False
			End Try


			Return result

		End Function

		Private Function SaveAssignedPartnershipData(ByVal recID As Integer?) As Boolean
			Dim result As Boolean = True
			Dim recordAsNew As Boolean = True
			Dim expenddt As Date
			Dim dateFormatstring As String = "dd.MM.yyyy"
			Dim culture As CultureInfo = New CultureInfo("de-DE")
			Dim msg As String
			Dim partnershipData = New EmployeePartnershipData

			Try

				If recID.GetValueOrDefault(0) > 0 Then partnershipData = m_EmployeeDataAccess.LoadEmployeeAssignedPartnershipData(recID.GetValueOrDefault(0))
				If (partnershipData Is Nothing) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten Partner Daten konnten nicht geladen werden."))

					Return False
				End If
				recordAsNew = partnershipData.ID.GetValueOrDefault(0) = 0
				partnershipData.EmployeeNumber = m_EmployeeNumber
				partnershipData.ExistingEmployeeNumber = lueExistingEmployee.EditValue

				partnershipData.Gender = lueGender.EditValue
				partnershipData.Lastname = txtLastname.EditValue
				partnershipData.Firstname = txtFirstname.EditValue
				partnershipData.Street = txtStreet.EditValue
				partnershipData.Country = lueCountry.EditValue
				partnershipData.Postcode = luePostcode.EditValue
				partnershipData.City = txtCity.EditValue
				partnershipData.SocialInsuranceNumber = txtSocialInsuranceNumber.EditValue


				If String.IsNullOrWhiteSpace(dateEditBirthday.Text) Then
					dateEditBirthday.EditValue = Nothing
				Else
					dateEditBirthday.EditValue = Convert.ToDateTime(dateEditBirthday.Text, culture)
				End If

				If Not Date.TryParseExact(dateEditBirthday.EditValue, dateFormatstring, System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None, expenddt) Then
					partnershipData.Birthdate = Nothing
					msg = m_Translate.GetSafeTranslationValue("Achtung: Das Datum für Geburtstag ist ungültig und wird entfernt. Bitte ändern Sie das Datum.<br>{0}")
					If Not dateEditBirthday.EditValue Is Nothing Then m_UtilityUI.ShowErrorDialog(String.Format(msg, dateEditBirthday.EditValue), m_Translate.GetSafeTranslationValue("Falsche Format"))
				Else
					partnershipData.Birthdate = expenddt
				End If
				If (chkInEmployment.Checked = True And lueCode.EditValue = "B") Then
					chkInEmployment.Checked = False
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Erwerbstätigkeit wird geändert!"))
				ElseIf (chkInEmployment.Checked = False And lueCode.EditValue = "C") Then
					chkInEmployment.Checked = True
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Erwerbstätigkeit wird geändert!"))
				End If

				partnershipData.InEmployment = chkInEmployment.Checked



				If String.IsNullOrWhiteSpace(dateEditValidFrom.Text) Then
					dateEditValidFrom.EditValue = Nothing
				Else
					dateEditValidFrom.EditValue = Convert.ToDateTime(dateEditValidFrom.Text, culture)
				End If
				If Not Date.TryParseExact(dateEditValidFrom.EditValue, dateFormatstring, System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None, expenddt) Then
					partnershipData.ValidFrom = Nothing
					msg = m_Translate.GetSafeTranslationValue("Achtung: Das Datum für Gültig-Ab ist ungültig und wird entfernt. Bitte ändern Sie das Datum.<br>{0}")
					If Not dateEditValidFrom.EditValue Is Nothing Then m_UtilityUI.ShowErrorDialog(String.Format(msg, dateEditValidFrom.EditValue), m_Translate.GetSafeTranslationValue("Falsche Format"))
				Else
					partnershipData.ValidFrom = expenddt
				End If

				If String.IsNullOrWhiteSpace(dateEditValidTo.Text) Then
					dateEditValidTo.EditValue = Nothing
				Else
					dateEditValidTo.EditValue = Convert.ToDateTime(dateEditValidTo.Text, culture)
				End If
				If Not Date.TryParseExact(dateEditValidTo.EditValue, dateFormatstring, System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None, expenddt) Then
					partnershipData.ValidTo = Nothing
					msg = m_Translate.GetSafeTranslationValue("Achtung: Das Datum für Gültig-Bis ist ungültig und wird entfernt. Bitte ändern Sie das Datum.<br>{0}")
					If Not dateEditValidTo.EditValue Is Nothing Then m_UtilityUI.ShowErrorDialog(String.Format(msg, dateEditValidTo.EditValue), m_Translate.GetSafeTranslationValue("Falsche Format"))
				Else
					partnershipData.ValidTo = expenddt
				End If


				If recordAsNew Then
					partnershipData.Createdfrom = m_InitializationData.UserData.UserFullName
					partnershipData.CreatedUserNumber = m_InitializationData.UserData.UserNr
					result = result AndAlso m_EmployeeDataAccess.AddEmployeePartnershipData(m_EmployeeNumber, partnershipData)
				Else
					partnershipData.ChangedFrom = m_InitializationData.UserData.UserFullName
					partnershipData.ChangedUserNumber = m_InitializationData.UserData.UserNr
					result = result AndAlso m_EmployeeDataAccess.UpdateEmployeePartnershipData(m_EmployeeNumber, partnershipData)
				End If

				m_PartnershipID = partnershipData.ID
				pnlExistingEmployee.Enabled = Not partnershipData.AsEmployee


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = False

			End Try


			Return result

		End Function

		Private Function DeleteAssignedPartnershipData(ByVal recID As Integer) As Boolean
			Dim result = DeleteEmployeeResult.Deleted

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																											m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return DeleteEmployeeResult.ErrorWhileDelete
			End If

			Try

				Dim partnershipData = m_EmployeeDataAccess.DeleteEmployeePartnershipData(recID, "Partnership", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)

				Dim msg As String = String.Empty

				Select Case result
					Case DeleteEmployeeResult.CouldNotDeleteBecauseOfExistingLO
						msg = "Die ausgewählten Partnerdaten sind in den Lohnabrechnungen enthalten."

					Case DeleteEmployeeResult.ErrorWhileDelete
						msg = "Die Daten konnten nicht gelöscht werden."

					Case DeleteEmployeeResult.Deleted
						msg = "Der ausgewählte Partner wurde erfolgreich gelöscht."
						m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)
						Return result

				End Select
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(String.Format(msg & "{0}Bitte löschen Sie alle abhängigen Datensätze bevor Sie den Datensatz endgültig löschen.", vbNewLine)),
																	 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				result = DeleteEmployeeResult.ErrorWhileDelete

			End Try

			Return result = DeleteEmployeeResult.Deleted

		End Function

		Private Sub OnlueExistingEmployee_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueExistingEmployee.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueExistingEmployee.EditValue Is Nothing Then

				Dim employeeNumber As Integer = lueExistingEmployee.EditValue

				Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(employeeNumber, False)
				Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDataAccess.LoadEmployeeContactCommData(employeeNumber)

				If employeeMasterData Is Nothing OrElse employeeContactCommData Is Nothing Then

					ResetExistingEmployeeDetailData()

					If employeeMasterData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeitestammdaten konnten nicht geladen werden."))
					End If

					If employeeContactCommData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter (KontaktKomm) konnten nicht geladen werden."))
					End If

				Else

					DisplayEmployeeAsPartinershipData(employeeMasterData, employeeContactCommData)
				End If

			Else
				ResetExistingEmployeeDetailData()

			End If


		End Sub

		Private Sub OnlueExistingEmployee_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueExistingEmployee.ButtonClick

			If lueExistingEmployee.EditValue Is Nothing Then
				Return
			End If

			If (e.Button.Index = 2) Then
				Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(lueExistingEmployee.EditValue, False)

				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, employeeMasterData.MDNr, lueExistingEmployee.EditValue)
				hub.Publish(openEmployeeMng)

			End If

		End Sub

		Private Sub ResetExistingEmployeeDetailData()

			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(EmployeeNumber, False)

			lueGender.EditValue = If(employeeMasterData.Gender = "M", "W", "M")
			txtLastname.EditValue = employeeMasterData.Lastname
			txtFirstname.EditValue = Nothing
			txtStreet.EditValue = employeeMasterData.Street
			lueCountry.EditValue = employeeMasterData.Country
			luePostcode.EditValue = employeeMasterData.Postcode
			txtCity.EditValue = employeeMasterData.Location
			txtSocialInsuranceNumber.EditValue = Nothing
			dateEditBirthday.EditValue = Nothing
			chkInEmployment.Checked = If(lueCode.EditValue = "C", True, False)

			dateEditValidFrom.EditValue = Nothing
			dateEditValidTo.EditValue = Nothing

		End Sub

		Private Function DisplayEmployeeAsPartinershipData(ByVal employeeMasterData As EmployeeMasterData, ByVal employeeContactCommData As EmployeeContactComm) As Boolean

			lueGender.EditValue = employeeMasterData.Gender
			txtLastname.EditValue = employeeMasterData.Lastname
			txtFirstname.EditValue = employeeMasterData.Firstname
			txtStreet.EditValue = employeeMasterData.Street
			lueCountry.EditValue = employeeMasterData.Country
			luePostcode.EditValue = employeeMasterData.Postcode
			txtCity.EditValue = employeeMasterData.Location
			txtSocialInsuranceNumber.EditValue = employeeMasterData.AHV_Nr_New
			dateEditBirthday.EditValue = employeeMasterData.Birthdate
			chkInEmployment.Checked = False

			dateEditValidFrom.EditValue = Nothing
			dateEditValidTo.EditValue = Nothing

			Return True
		End Function

		Private Sub FocusPartnershipData(ByVal recID As Integer)

			Dim listDataSource As BindingList(Of EmployeePartnershipData) = grdPartnership.DataSource
			If listDataSource Is Nothing Then Return

			Dim partnershipViewData = listDataSource.Where(Function(data) data.ID = recID).FirstOrDefault()

			If Not partnershipViewData Is Nothing Then
				Dim suppressUIEventsState = m_SuppressUIEvents
				m_SuppressUIEvents = True
				Dim sourceIndex = listDataSource.IndexOf(partnershipViewData)
				Dim rowHandle = gvPartnership.GetRowHandle(sourceIndex)
				gvPartnership.FocusedRowHandle = rowHandle

				m_SuppressUIEvents = suppressUIEventsState
			End If

		End Sub




	End Class

End Namespace
