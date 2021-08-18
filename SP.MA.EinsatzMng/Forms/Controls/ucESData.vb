
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Customer

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.DatabaseAccess.Employee

Namespace UI

	Public Class ucESData

#Region "Private Constants"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The initial es ab date.
		''' </summary>
		Private m_InitialESAb As DateTime?

		''' <summary>
		''' The initial es end date.
		''' </summary>
		Private m_InitialESEnde As DateTime?

		''' <summary>
		''' The current ESNr.
		''' </summary>
		Private m_CurrentESNr As Integer?

		''' <summary>
		''' ES ende text if ES end  is not set.
		''' </summary>
		Private m_ESEndebynull As String

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False

			Try
				m_ProgPath = New ClsProgPath
				m_Mandant = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			AddHandler dateEditStartDate.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditEndDate.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueCurrency.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueSuva.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region

#Region "Public Methods"


		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		End Sub

		''' <summary>
		''' Loads data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadData(ByVal esData As ESMasterData) As Boolean

			Dim success = True

			If Not m_IsIntialControlDataLoaded Then
				success = success AndAlso LoadDropDownData()
				m_IsIntialControlDataLoaded = True
			End If

			m_ESEndebynull = m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/esendebynull", FORM_XML_MAIN_KEY))


			success = success AndAlso LoadBranchDropDownData(esData.CustomerNumber)

			grpESData.Text = String.Format(m_Translate.GetSafeTranslationValue("Einsatz: {0}"), esData.ESNR)
			txtWorktime.Text = esData.Arbzeit
			txtWorkplace.Text = esData.Arbort
			txtReportTo.Text = esData.Melden
			txtEsAls.Text = esData.ES_Als

			Dim suppressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			dateEditStartDate.EditValue = esData.ES_Ab
			dateEditEndDate.EditValue = esData.ES_Ende
			m_SuppressUIEvents = suppressUIEventState

			m_InitialESAb = esData.ES_Ab.Value.Date
			m_InitialESEnde = If(esData.ES_Ende Is Nothing, New DateTime(3999, 12, 31), esData.ES_Ende.Value.Date)

			RecalculateDaysOfES()

			txtTime.Text = esData.ES_Uhr
			txtGoesLonger.Text = esData.GoesLonger
			txtRemarks.Text = esData.Ende

			lueESEinstufung.EditValue = esData.Einstufung
			lueBranch.EditValue = esData.ESBranche
			lueCurrency.EditValue = esData.Currency
			lueSuva.EditValue = esData.SUVA

			chkESBacked.Checked = If(esData.ESVerBacked.HasValue, esData.ESVerBacked.Value, False)
			chkESPrinted.Checked = If(esData.Print_MA.HasValue, esData.Print_MA.Value, False)
			chkDoNotAddInESList.Checked = If(esData.NoListing.HasValue, esData.NoListing.Value, False)
			chkVeleihVertragPrinted.Checked = If(esData.Print_KD.HasValue, esData.Print_KD.Value, False)
			chkVerleihvertragBacked.Checked = If(esData.VerleihBacked.HasValue, esData.VerleihBacked.Value, False)

			chkDoNotAddInESList.Properties.ReadOnly = Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 265, m_InitializationData.MDData.MDNr)
			chkNoPrintReports.Checked = If(esData.PrintNoRP.HasValue, esData.PrintNoRP.Value, False)

			RecalculateDaysOfES()

			m_CurrentESNr = If(success, esData.ESNR, Nothing)

			Return success

		End Function

		''' <summary>
		''' Rereshes Data after ES save.
		''' </summary>
		'''<param name="esData">The ES master data.</param>
		Public Sub RefreshDataAfterESSave(ByVal esData As ESMasterData)
			m_InitialESAb = esData.ES_Ab.Value.Date
			m_InitialESEnde = If(esData.ES_Ende Is Nothing, New DateTime(3999, 12, 31), esData.ES_Ende.Value.Date)
		End Sub

		''' <summary>
		''' Refreshes Verleih- and Einsatzvertrag printed data.
		''' </summary>
		''' <param name="esData">The ES master data.</param>
		Public Sub RefreshVerleihAndEinsatzVertragPrintedData(ByVal esData As ESMasterData)
			chkESPrinted.Checked = If(esData.Print_MA.HasValue, esData.Print_MA.Value, False)
			chkVeleihVertragPrinted.Checked = If(esData.Print_KD.HasValue, esData.Print_KD.Value, False)
		End Sub

		''' <summary>
		''' Merges ES master data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		Public Overrides Sub MergeESMasterData(ByVal esData As ESMasterData)

			esData.Arbzeit = txtWorktime.Text
			esData.Arbort = txtWorkplace.Text
			esData.Melden = txtReportTo.Text
			esData.ES_Als = txtEsAls.Text
			esData.ES_Ab = dateEditStartDate.EditValue
			esData.ES_Ende = dateEditEndDate.EditValue
			esData.ES_Uhr = txtTime.Text
			esData.GoesLonger = txtGoesLonger.Text
			esData.Ende = txtRemarks.Text

			esData.Einstufung = lueESEinstufung.EditValue
			esData.ESBranche = lueBranch.EditValue
			esData.Currency = lueCurrency.EditValue
			esData.SUVA = lueSuva.EditValue

			esData.ESVerBacked = chkESBacked.Checked
			esData.Print_MA = chkESPrinted.Checked
			esData.NoListing = chkDoNotAddInESList.Checked
			esData.Print_KD = chkVeleihVertragPrinted.Checked
			esData.VerleihBacked = chkVerleihvertragBacked.Checked
			esData.PrintNoRP = chkNoPrintReports.Checked

		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_InitialESAb = Nothing
			m_InitialESEnde = Nothing
			m_CurrentESNr = Nothing

			txtWorktime.Text = String.Empty
			txtWorktime.Properties.MaxLength = 255

			txtWorkplace.Text = String.Empty
			txtWorkplace.Properties.MaxLength = 255

			txtReportTo.Text = String.Empty
			txtReportTo.Properties.MaxLength = 255

			txtEsAls.Text = String.Empty
			txtEsAls.Properties.MaxLength = 1000

			dateEditStartDate.EditValue = Nothing

			Dim suppressState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			dateEditEndDate.EditValue = Nothing
			m_SuppressUIEvents = suppressState

			lblDays.Text = String.Empty

			txtTime.Text = String.Empty
			txtTime.Properties.MaxLength = 5

			txtGoesLonger.Text = String.Empty
			txtGoesLonger.Properties.MaxLength = 70

			txtRemarks.Text = String.Empty
			txtRemarks.Properties.MaxLength = 70

			chkESBacked.Checked = False
			chkESPrinted.Checked = False
			chkDoNotAddInESList.Checked = False
			chkVeleihVertragPrinted.Checked = False
			chkVerleihvertragBacked.Checked = False
			chkNoPrintReports.Checked = False

			'  Reset drop downs and lists

			ResetESCategorizationDropDown()
			ResetBranchDropDown()
			ResetCurrencyDropDown()
			ResetSuvaDropDown()

			ErrorProvider.Clear()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			Dim mandantNumber As Integer = m_UCMediator.MandantNumber
			Dim employeeNumber As Integer = m_UCMediator.EmployeeNumber

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorTextInvalidStartOrEndDate As String = m_Translate.GetSafeTranslationValue("Einsatzende muss nach Startdatum liegen.")

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(txtEsAls, ErrorProvider, String.IsNullOrWhiteSpace(txtEsAls.Text), errorText)
			isValid = isValid And SetErrorIfInvalid(dateEditStartDate, ErrorProvider, dateEditStartDate.EditValue Is Nothing, errorText)

			Dim mustESEinstufungBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																									String.Format("{0}/eseinstufungselectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustESBrancheBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																									String.Format("{0}/esbrancheselectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustESTimeBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																									String.Format("{0}/timeselectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			Dim IsDataSelected As Boolean = Not (lueESEinstufung.EditValue Is Nothing AndAlso String.IsNullOrWhiteSpace(lueESEinstufung.EditValue))
			isValid = isValid And SetErrorIfInvalid(lueESEinstufung, ErrorProvider, (mustESEinstufungBeSelected AndAlso (Not IsDataSelected)), errorText)

			IsDataSelected = Not (lueBranch.EditValue Is Nothing AndAlso String.IsNullOrWhiteSpace(lueBranch.EditValue))
			isValid = isValid And SetErrorIfInvalid(lueBranch, ErrorProvider, (mustESBrancheBeSelected AndAlso (Not IsDataSelected)), errorText)

			IsDataSelected = Not (txtTime.EditValue Is Nothing AndAlso String.IsNullOrWhiteSpace(txtTime.EditValue))
			isValid = isValid And SetErrorIfInvalid(txtTime, ErrorProvider, (mustESTimeBeSelected AndAlso (Not IsDataSelected)), errorText)

			If isValid AndAlso Not dateEditStartDate.EditValue Is Nothing AndAlso
						 Not dateEditEndDate.EditValue Is Nothing Then

				Dim startDate As DateTime? = dateEditStartDate.EditValue
				Dim endDate As DateTime? = dateEditEndDate.EditValue

				Dim isEndDateValid As Boolean = (endDate.Value.Date.CompareTo(startDate.Value.Date) >= 0)
				isValid = isValid And SetErrorIfInvalid(dateEditEndDate, ErrorProvider, Not isEndDateValid, errorTextInvalidStartOrEndDate)

			End If

			isValid = isValid And SetErrorIfInvalid(lueCurrency, ErrorProvider, lueCurrency.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(lueSuva, ErrorProvider, lueSuva.EditValue Is Nothing, errorText)

			' -- Check fo conflicting records ---

			Dim esUtilityFunctions As New ESValidationUtility(m_ESDataAccess, m_Translate)

			If isValid Then

				Dim startDate As DateTime = dateEditStartDate.EditValue

				' Check if start date has changed
				If Not startDate.Date = m_InitialESAb.Value.Date Then

					Dim date1 As DateTime
					Dim date2 As DateTime

					' Make sure date1 is always the smaller of start date and initial ES_Ab date 
					If startDate.Date < m_InitialESAb.Value.Date Then
						date1 = startDate.Date
						date2 = m_InitialESAb.Value.Date
					Else
						date1 = m_InitialESAb.Value.Date
						date2 = startDate.Date
					End If

					date2 = date2.AddSeconds(-1) ' Bigger date is always last second of previous day

					isValid = isValid AndAlso esUtilityFunctions.CheckForConflictingLOInPeriod(employeeNumber, mandantNumber, date1, date2)
					isValid = isValid AndAlso esUtilityFunctions.CheckForConflictingRPLInPeriod(m_CurrentESNr, employeeNumber, date1, date2)
					isValid = isValid AndAlso esUtilityFunctions.CheckForConflictingMonthCloseRecordsInPeriod(mandantNumber, date1, date2)

				End If

			End If

			If isValid Then

				Dim endDateIfEmpty = New DateTime(3999, 12, 31) ' Replacement date for empty end date
				Dim endDate As DateTime = If(dateEditEndDate.EditValue Is Nothing, endDateIfEmpty, dateEditEndDate.EditValue)

				If Not endDate.Date = m_InitialESEnde.Value.Date Then

					Dim date1 As DateTime
					Dim date2 As DateTime

					' Make sure date1 is always the smaller of start date and initial ESEnde date 
					If endDate.Date < m_InitialESEnde.Value.Date Then
						date1 = endDate.Date
						date2 = m_InitialESEnde.Value.Date
					Else
						date1 = m_InitialESEnde.Value.Date
						date2 = endDate.Date
					End If

					' If end date is moved for example from 1.10.2013 to 2.10.203 then date1 = 2.10.2013 00:00:00 and date2 = 2.10.2013 23:59:59
					date1 = date1.AddDays(1) ' Next day 
					date2 = date2.AddDays(1).AddSeconds(-1) ' Last second of next day

					isValid = isValid AndAlso esUtilityFunctions.CheckForConflictingLOInPeriod(employeeNumber, mandantNumber, date1, date2)
					isValid = isValid AndAlso esUtilityFunctions.CheckForConflictingRPLInPeriod(m_CurrentESNr, employeeNumber, date1, date2)
					isValid = isValid AndAlso esUtilityFunctions.CheckForConflictingMonthCloseRecordsInPeriod(mandantNumber, date1, date2)

				End If

			End If

			Return isValid

			Return True
		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			'Me.grpESData.Text = m_Translate.GetSafeTranslationValue(Me.grpESData.Text)
			Me.lblArbeitszeit.Text = m_Translate.GetSafeTranslationValue(Me.lblArbeitszeit.Text)
			Me.lblArbeitsort.Text = m_Translate.GetSafeTranslationValue(Me.lblArbeitsort.Text)
			Me.lblMeldenBei.Text = m_Translate.GetSafeTranslationValue(Me.lblMeldenBei.Text)
			Me.lblEinsatzAls.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatzAls.Text)
			Me.lblBescheinigungGueltigBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBescheinigungGueltigBis.Text)
			Me.lblEndetAm.Text = m_Translate.GetSafeTranslationValue(Me.lblEndetAm.Text)
			Me.lblUhrzeit.Text = m_Translate.GetSafeTranslationValue(Me.lblUhrzeit.Text)
			Me.lblVerlaengert.Text = m_Translate.GetSafeTranslationValue(Me.lblVerlaengert.Text, True)
			Me.lblBemerkung.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung.Text, True)

			Me.grpSettings.Text = m_Translate.GetSafeTranslationValue(Me.grpSettings.Text)
			Me.lblEinsatzEinstufung.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatzEinstufung.Text, True)
			Me.lblBranche.Text = m_Translate.GetSafeTranslationValue(Me.lblBranche.Text, True)
			Me.lblWaehrung.Text = m_Translate.GetSafeTranslationValue(Me.lblWaehrung.Text)
			Me.lblSuva.Text = m_Translate.GetSafeTranslationValue(Me.lblSuva.Text)

			Me.chkESPrinted.Text = m_Translate.GetSafeTranslationValue(Me.chkESPrinted.Text)
			Me.chkESBacked.Text = m_Translate.GetSafeTranslationValue(Me.chkESBacked.Text)
			Me.chkDoNotAddInESList.Text = m_Translate.GetSafeTranslationValue(Me.chkDoNotAddInESList.Text)
			Me.chkVeleihVertragPrinted.Text = m_Translate.GetSafeTranslationValue(Me.chkVeleihVertragPrinted.Text)
			Me.chkVerleihvertragBacked.Text = m_Translate.GetSafeTranslationValue(Me.chkVerleihvertragBacked.Text)
			Me.chkNoPrintReports.Text = m_Translate.GetSafeTranslationValue(Me.chkNoPrintReports.Text)

		End Sub

		''' <summary>
		''' Resets the ES categorization drop down.
		''' </summary>
		Private Sub ResetESCategorizationDropDown()

			lueESEinstufung.Properties.DisplayMember = "TranslatedESCategorizationDescription"
			lueESEinstufung.Properties.ValueMember = "Description"

			lueESEinstufung.Properties.Columns.Clear()
			lueESEinstufung.Properties.Columns.Add(New LookUpColumnInfo("TranslatedESCategorizationDescription", 0, String.Empty))

			lueESEinstufung.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueESEinstufung.Properties.SearchMode = SearchMode.AutoComplete
			lueESEinstufung.Properties.AutoSearchColumnIndex = 0

			lueESEinstufung.Properties.NullText = String.Empty
			lueESEinstufung.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the branch drop down.
		''' </summary>
		Private Sub ResetBranchDropDown()

			lueBranch.Properties.DisplayMember = "TranslatedBrancheText"
			lueBranch.Properties.ValueMember = "Branche"

			' Reset the grid view
			gvLueBranch.OptionsView.ShowIndicator = False

			gvLueBranch.Columns.Clear()

			Dim columnBranchText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBranchText.Caption = m_Translate.GetSafeTranslationValue("Branchen") 'String.Empty
			columnBranchText.Name = "TranslatedBrancheText"
			columnBranchText.FieldName = "TranslatedBrancheText"
			columnBranchText.Visible = True
			gvLueBranch.Columns.Add(columnBranchText)

			lueBranch.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueBranch.Properties.NullText = String.Empty
			lueBranch.Properties.DataSource = Nothing
			lueBranch.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the curreny drop down.
		''' </summary>
		Private Sub ResetCurrencyDropDown()

			lueCurrency.Properties.DisplayMember = "Description"
			lueCurrency.Properties.ValueMember = "Code"

			Dim columns = lueCurrency.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Code", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("Description", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueCurrency.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueCurrency.Properties.SearchMode = SearchMode.AutoComplete
			lueCurrency.Properties.AutoSearchColumnIndex = 0

			lueCurrency.Properties.NullText = String.Empty
			lueCurrency.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the Suva drop down.
		''' </summary>
		Private Sub ResetSuvaDropDown()

			lueSuva.Properties.DisplayMember = "DataforView" ' "TranslatedDescription"
			lueSuva.Properties.ValueMember = "GetField"
			lueSuva.Properties.ReadOnly = True

			Dim columns = lueSuva.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("GetField", 0, m_Translate.GetSafeTranslationValue("Code")))
			columns.Add(New LookUpColumnInfo("TranslatedDescription", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			lueSuva.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSuva.Properties.SearchMode = SearchMode.AutoComplete
			lueSuva.Properties.AutoSearchColumnIndex = 0

			lueSuva.Properties.NullText = String.Empty
			lueSuva.EditValue = Nothing

		End Sub

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadESCategorizationDropDownData()
			success = success AndAlso LoadCurrencyDropDownData()
			success = success AndAlso LoadSuvaDropDownData()
			Return success
		End Function

		''' <summary>
		''' Loads the ES categorization drop down data.
		''' </summary>
		Private Function LoadESCategorizationDropDownData() As Boolean
			' Load data
			Dim categorizationData = m_ESDataAccess.LoadESCategorizationData()

			If (categorizationData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatzeinstufungen konnten nicht geladen werden."))
			End If

			lueESEinstufung.Properties.DataSource = categorizationData
			lueESEinstufung.Properties.ForceInitialize()

			Return Not categorizationData Is Nothing
		End Function

		''' <summary>
		''' Loads the branch drop down data.
		''' </summary>
		'''<param name="customerNumber">The customer number.</param>
		Private Function LoadBranchDropDownData(ByVal customerNumber As Integer) As Boolean
			' Load data
			Dim allBranchData As List(Of DatabaseAccess.Common.DataObjects.BranchData) = m_CommonDatabaseAccess.LoadBranchData()
			Dim customerAssignedBranchData = m_CustomerDatabaseAccess.LoadAssignedSectorDataOfCustomer(customerNumber)

			Dim mergedBranchData As List(Of BranchViewData) = Nothing

			If (allBranchData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Branchen konnten nicht geladen werden."))
			End If

			If customerAssignedBranchData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundenbranchen konnten nicht geladen werden."))
			Else

				'--- This section arranges the branches so that the customer assigned ones will be at the top ---

				' Create an empty list that will contain the merged branches of the customer and the ones from the master table
				mergedBranchData = New List(Of BranchViewData)

				For Each customerAssignedBranch In customerAssignedBranchData

					Dim customerBranchCode = customerAssignedBranch.SectorCode

					If customerBranchCode.HasValue AndAlso customerBranchCode <> 0 Then
						' Try to find branch in master data
						Dim extistingBranch = allBranchData.Where(Function(branch) branch.Code = customerBranchCode).FirstOrDefault()

						If Not extistingBranch Is Nothing Then

							' The branch could be found -> add to merged results..
							mergedBranchData.Add(New BranchViewData With {.Branche = extistingBranch.Branche,
																														.TranslatedBrancheText = extistingBranch.TranslatedBrancheText,
																														.IsAssignedToCustomer = True})

							' Remove from all branch data. 
							allBranchData.Remove(extistingBranch)
						Else

							' The branch could not be found. Most likely the sector code is set to 0 in the customer assigned branch data.
							' The data is added manually. Translation is not taken into account!
							mergedBranchData.Add(New BranchViewData With {.Branche = customerAssignedBranch.Description,
																														.TranslatedBrancheText = customerAssignedBranch.Description,
																														.IsAssignedToCustomer = True})

						End If

					End If

				Next

				' Add branches that are left in allBranchData
				For Each branch In allBranchData
					mergedBranchData.Add(New BranchViewData With {.Branche = branch.Branche,
																  .TranslatedBrancheText = branch.TranslatedBrancheText,
																  .IsAssignedToCustomer = False})
				Next
			End If

			lueBranch.Properties.DataSource = mergedBranchData

			Return Not allBranchData Is Nothing
		End Function

		''' <summary>
		''' Loads the currency drop down data.
		''' </summary>
		Private Function LoadCurrencyDropDownData() As Boolean
			Dim currencyData = m_CommonDatabaseAccess.LoadCurrencyData()

			If (currencyData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Währungsdaten konnten nicht geladen werden."))
			End If

			lueCurrency.Properties.DataSource = currencyData
			lueCurrency.Properties.ForceInitialize()

			Return Not currencyData Is Nothing
		End Function

		''' <summary>
		''' Loads the Suva drop down data.
		''' </summary>
		Private Function LoadSuvaDropDownData() As Boolean
			Dim suvaData = m_ESDataAccess.LoadSuvaData()

			If (suvaData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Suva-Daten konnten nicht geladen werden."))
			End If

			lueSuva.Properties.DataSource = suvaData
			lueSuva.Properties.ForceInitialize()

			Return Not suvaData Is Nothing
		End Function

		''' <summary>
		''' Handles click on qualification button.
		''' </summary>
		Private Sub OnTxtQulification_ButtonClick(sender As System.Object, e As System.EventArgs) Handles txtEsAls.ButtonClick
			' Show profession selection dialog.
			Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
			obj.SelectMultirecords = False
			Dim success = True

			Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_UCMediator.EmployeeNumber, False)
			If employeeMasterData Is Nothing Then Return


			success = success AndAlso obj.LoadQualificationData(employeeMasterData.Gender)
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedProfessionsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return

			' Tokenize the result string.
			' Result string has the following format <ProfessionCode>#<ProfessionDescription>
			Dim tokens As String() = selectedProfessionsString.Split("#")

			' It must be an even number of tokens -> otherwhise something is wrong
			If tokens.Count Mod 2 = 0 Then
				txtEsAls.Tag = tokens(0)
				txtEsAls.Text = tokens(1)
			End If

		End Sub

		''' <summary>
		''' Handles change of start date.
		''' </summary>
		Private Sub OnDateEditStartDate_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles dateEditStartDate.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			RecalculateDaysOfES()

			If Not dateEditStartDate.EditValue Is Nothing Then

				Dim dateStart As DateTime? = dateEditStartDate.EditValue
				m_UCMediator.LoadLAData(dateStart.Value.Date.Year)
				m_UCMediator.CheckGAVValidityOfSelectedESSalaryData()
			End If

		End Sub

		''' <summary>
		''' Handles change of end date.
		''' </summary>
		Private Sub OnDateEditEndDate_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles dateEditEndDate.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			RecalculateDaysOfES()
			m_UCMediator.CheckGAVValidityOfSelectedESSalaryData()

		End Sub

		''' <summary>
		'''  Handles RowStyle event of lueBranch grid view.
		''' </summary>
		Private Sub OnGvLueBranch_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvLueBranch.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvLueBranch.GetRow(e.RowHandle), BranchViewData)

				If rowData.IsAssignedToCustomer Then
					e.Appearance.BackColor = Color.Yellow
					e.Appearance.BackColor2 = Color.Yellow
				End If

			End If

		End Sub

		''' <summary>
		''' Recalculates days of ES.
		''' </summary>
		Private Sub RecalculateDaysOfES()

			Dim startDate As DateTime? = dateEditStartDate.EditValue
			Dim endDate As DateTime? = dateEditEndDate.EditValue

			If startDate.HasValue And endDate.HasValue AndAlso
			   endDate.Value.Date.CompareTo(startDate.Value.Date) >= 0 Then
				Dim days As Integer = Math.Max(0, (endDate.Value.Date - startDate.Value.Date).TotalDays + 1)
				lblDays.Text = String.Format("{0} {1}", days, m_Translate.GetSafeTranslationValue("Tage"))
			Else
				lblDays.Text = m_ESEndebynull
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

	End Class

#Region "View helper classes"

	''' <summary>
	''' Branch view data.
	''' </summary>
	Class BranchViewData

		Public Property Branche As String
		Public Property TranslatedBrancheText As String
		Public Property IsAssignedToCustomer

	End Class

#End Region

End Namespace
