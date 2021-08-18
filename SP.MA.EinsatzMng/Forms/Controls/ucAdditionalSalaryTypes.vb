Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.ES
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports SPS.SalaryValueCalculation.RPLValueCalculation.RPLSalaryTypeValuesCalcParams
Imports SPS.SalaryValueCalculation.RPLValueCalculation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Namespace UI

	Public Class ucAdditionalSalaryTypes

#Region "Private Consts"

		Public Const Basis_Default_Value As Decimal = 0.0
		Public Const Ansatz_Default_Value As Decimal = 0.0
		Public Const Betrag_Default_Value As Decimal = 0.0

#End Region


#Region "Private Fields"

		''' <summary>
		''' The current ES nr.
		''' </summary>
		Private m_CurrentESNr As Integer?

		''' <summary>
		''' The current detail LANr. 
		''' </summary>
		Private m_CurrentDetailESLANr As Integer?

		''' <summary>
		''' The current detail type (customer or employee)
		''' </summary>
		Private m_CurrentDetailType As ESAdditionalSalaryType?

		''' <summary>
		''' The RPL value calculator.
		''' </summary>
		Private m_RPLValueCalculator As RPLValueCalculator

		''' <summary>
		''' LA List.
		''' </summary>
		Private m_LAList As List(Of LAData)

		''' <summary>
		''' ESSalary List.
		''' </summary>
		Private m_ESSalaryList As List(Of ESSalaryData)

		''' <summary>
		''' The year value which the m_LAList hab been loaded (Use to prevent reloading if year did not change).
		''' </summary>
		Private m_YearOfLAList As Integer

		''' <summary>
		'''  Current LA values upper bounds.
		''' </summary>
		Private m_CurrentLAValueUpperBounds As LAValueUpperBounds

		Private m_Mandant As Mandant
		Private m_path As ClsProgPath

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			m_Mandant = New Mandant
			m_path = New ClsProgPath

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_CurrentLAValueUpperBounds = New LAValueUpperBounds
			If Not m_InitializationData Is nothing Then
				m_RPLValueCalculator = New RPLValueCalculator(m_InitializationData)
			End If

			AddHandler lueESSalary.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLAData.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueDayMonthStd.ButtonClick, AddressOf OnDropDown_ButtonClick

		End Sub

#End Region



#Region "Public Properties"

		''' <summary>
		''' Gets the selected ESMALA data.
		''' </summary>
		''' <returns>The selected ESMALA data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedESMALAData As ESEmployeeAndCustomerLAData
			Get
				Dim grdView = TryCast(grdEmployeeSalaryTypeData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim esLAData = CType(grdView.GetRow(selectedRows(0)), ESEmployeeAndCustomerLAData)
						Return esLAData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected ESKDLA data.
		''' </summary>
		''' <returns>The selected ESKDLA data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedESKDLAData As ESEmployeeAndCustomerLAData
			Get
				Dim grdView = TryCast(grdCustomerSalaryTypeData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim esLAData = CType(grdView.GetRow(selectedRows(0)), ESEmployeeAndCustomerLAData)
						Return esLAData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected LA.
		''' </summary>
		''' <returns>The selected LA or nothing.</returns>
		Public ReadOnly Property SelectedLA As LAData
			Get

				If lueLAData.EditValue Is Nothing Then
					Return Nothing
				End If

				Dim laData = m_LAList.Where(Function(data) data.LANr = lueLAData.EditValue).FirstOrDefault()
				Return laData
			End Get
		End Property

		''' <summary>
		''' Gets the selected LA.
		''' </summary>
		''' <returns>The selected LA or nothing.</returns>
		Public ReadOnly Property SelectedESSalaryData As ESSalaryData
			Get

				If lueESSalary.EditValue Is Nothing Then
					Return Nothing
				End If

				Dim salaryData = m_ESSalaryList.Where(Function(data) data.ESNr = m_CurrentESNr And data.ESLohnNr = lueESSalary.EditValue).FirstOrDefault()
				Return salaryData
			End Get
		End Property


#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadData(ByVal esData As ESMasterData) As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadDropDownData(esData.ESNR)
			success = success AndAlso LoadESEmployeeAndCustomerSalaryTypeData(esData.ESNR)

			' ES_Ab should must not be empty, but for safety check it.
			Dim year As Integer = If(esData.ES_Ab.HasValue, esData.ES_Ab.Value.Date.Year, DateTime.Now.Year)

			success = success AndAlso LoadLAData(year)

			m_CurrentESNr = If(success, esData.ESNR, Nothing)

			m_SuppressUIEvents = False

			Return True
		End Function

		''' <summary>
		''' Merges ES master data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		Public Overrides Sub MergeESMasterData(ByVal esData As ESMasterData)
			' Nothing to to here.
		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_CurrentESNr = Nothing
			m_CurrentDetailESLANr = Nothing
			m_CurrentDetailType = Nothing
			m_ESSalaryList = Nothing
			m_LAList = Nothing
			m_YearOfLAList = 0

			lblInfo.Text = String.Empty

			txtBasis.EditValue = Basis_Default_Value
			txtBasis.Properties.MaxLength = 16
			txtBasis.Enabled = True

			txtAnsatz.EditValue = Ansatz_Default_Value
			txtAnsatz.Properties.MaxLength = 16
			txtAnsatz.Enabled = True

			txtBetrag.EditValue = Betrag_Default_Value
			txtBetrag.Properties.MaxLength = 16
			txtBetrag.Enabled = False

			m_CurrentLAValueUpperBounds.Reset()

			btnSave.Enabled = False

			'  Reset drop downs, grid and lists

			ResetESSalaryDropDown()
			ResetLADropDown()
			ResetDayMonthStdDropDown()

			ResetEmployeeSalaryTypesGrid()
			ResetCustomerSalaryTypesGrid()

			errorProviderLAData.Clear()

		End Sub

		''' <summary>
		''' Loads LA data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Function LoadLAData(ByVal year As Integer) As Boolean

			If (Not m_LAList Is Nothing And m_YearOfLAList = year) Then
				' Data is already loaded.
				Return True
			End If

			m_LAList = m_ESDataAccess.LoadLAListForESMng(year)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			lueLAData.Properties.DataSource = m_LAList
			lueLAData.Properties.ForceInitialize()

			If Not lueLAData.EditValue Is Nothing AndAlso
				Not m_LAList.Any(Function(data) data.LANr = lueLAData.EditValue) Then
				lueLAData.EditValue = Nothing
			End If

			m_SuppressUIEvents = suppressUIEventsState

			If m_LAList Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten konnten nicht geladen werden."))
			End If

			m_YearOfLAList = year

			Return Not m_LAList Is Nothing

		End Function

		''' <summary>
		''' Refrehes the data drop down.
		''' </summary>
		Public Sub RefreshData()

			If m_CurrentESNr.HasValue Then

				m_CurrentDetailType = Nothing
				ClearESLADetailControls()

				LoadESEmployeeAndCustomerSalaryTypeData(m_CurrentESNr)
				LoadESSalaryDropDownData(m_CurrentESNr)

				btnSave.Enabled = False
			End If

		End Sub
#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the default MwStValue.
		''' </summary>
		''' <returns>The default MwSt value.</returns>
		Private ReadOnly Property DefaultMwStValue(ByVal mdYear As Integer) As Decimal
			Get

				Dim mdNumber = m_InitializationData.MDData.MDNr

				If mdYear <= Now.Year - 10 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)


				'Dim strQuery As String = "//Debitoren/mwstsatz"
				''Dim r = m_ClsProgSetting.GetUserProfileFile
				'Dim defaultMwSt As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "8")

				'Return Convert.ToDecimal(defaultMwSt)
			End Get
		End Property


#End Region


#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)
			Me.lblKunde.Text = m_Translate.GetSafeTranslationValue(Me.lblKunde.Text)

			Me.lblInfo.Text = String.Empty
			Me.lblDayMonthStd.Text = m_Translate.GetSafeTranslationValue(Me.lblDayMonthStd.Text)
			Me.lblESLohn.Text = m_Translate.GetSafeTranslationValue(Me.lblESLohn.Text)
			Me.lblLohnart.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnart.Text)
			Me.lblBasis.Text = m_Translate.GetSafeTranslationValue(Me.lblBasis.Text)
			Me.lblAnsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblAnsatz.Text)
			Me.lblBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblBetrag.Text)

			Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)

		End Sub

		''' <summary>
		''' Resets the ESSalary drop down data.
		''' </summary>
		Private Sub ResetESSalaryDropDown()

			lueESSalary.Enabled = True

			lueESSalary.Properties.DisplayMember = "FormatString1"
			lueESSalary.Properties.ValueMember = "ESLohnNr"

			Dim columns = lueESSalary.Properties.Columns
			columns.Clear()

			Dim dateColumn As New LookUpColumnInfo("LOVon", 0, m_Translate.GetSafeTranslationValue("Datum"))
			dateColumn.FormatType = DevExpress.Utils.FormatType.DateTime
			dateColumn.FormatString = "dd.MM.yyyy"
			columns.Add(dateColumn)

			Dim grundLohnColumn As New LookUpColumnInfo("GrundLohn", 0, m_Translate.GetSafeTranslationValue("Grundlohn"))
			grundLohnColumn.FormatType = DevExpress.Utils.FormatType.Numeric
			grundLohnColumn.FormatString = "N2"
			grundLohnColumn.Alignment = DevExpress.Utils.HorzAlignment.Far
			columns.Add(grundLohnColumn)

			Dim stdLohnColumn As New LookUpColumnInfo("StundenLohn", 0, m_Translate.GetSafeTranslationValue("Stundenlohn"))
			stdLohnColumn.FormatType = DevExpress.Utils.FormatType.Numeric
			stdLohnColumn.FormatString = "N2"
			stdLohnColumn.Alignment = DevExpress.Utils.HorzAlignment.Far
			columns.Add(stdLohnColumn)

			Dim tarifColumn As New LookUpColumnInfo("Tarif", 0, m_Translate.GetSafeTranslationValue("Tarif"))
			tarifColumn.FormatType = DevExpress.Utils.FormatType.Numeric
			tarifColumn.FormatString = "N2"
			tarifColumn.Alignment = DevExpress.Utils.HorzAlignment.Far
			columns.Add(tarifColumn)


			lueESSalary.Properties.ShowHeader = True
			lueESSalary.Properties.ShowFooter = False
			lueESSalary.Properties.DropDownRows = 5
			lueESSalary.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueESSalary.Properties.SearchMode = SearchMode.AutoComplete
			lueESSalary.Properties.AutoSearchColumnIndex = 0

			lueESSalary.Properties.NullText = String.Empty

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			lueESSalary.EditValue = Nothing
			m_ESSalaryList = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Resets the LA drop down data.
		''' </summary>
		Private Sub ResetLADropDown()

			lueLAData.Enabled = True

			lueLAData.Properties.DisplayMember = "DisplayText"
			lueLAData.Properties.ValueMember = "LANr"

			Dim columns = lueLAData.Properties.Columns
			columns.Clear()

			Dim laNrColumn As New LookUpColumnInfo("LANr", 0)
			laNrColumn.FormatString = "0.###"
			laNrColumn.FormatType = DevExpress.Utils.FormatType.Custom

			columns.Add(laNrColumn)
			columns.Add(New LookUpColumnInfo("LALoText", 0))

			lueLAData.Properties.ShowHeader = False
			lueLAData.Properties.ShowFooter = False
			lueLAData.Properties.DropDownRows = 10
			lueLAData.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueLAData.Properties.SearchMode = SearchMode.AutoComplete
			lueLAData.Properties.AutoSearchColumnIndex = 0

			lueLAData.Properties.NullText = String.Empty

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			lueLAData.EditValue = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Resets the Day, Month, Std drop down data.
		''' </summary>
		Private Sub ResetDayMonthStdDropDown()

			lueDayMonthStd.Properties.DisplayMember = "DisplayText"
			lueDayMonthStd.Properties.ValueMember = "Value"

			Dim columns = lueDayMonthStd.Properties.Columns
			columns.Clear()

			columns.Add(New LookUpColumnInfo("DisplayText", 0))

			lueDayMonthStd.Properties.ShowHeader = False
			lueDayMonthStd.Properties.ShowFooter = False
			lueDayMonthStd.Properties.DropDownRows = 10
			lueDayMonthStd.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueDayMonthStd.Properties.SearchMode = SearchMode.AutoComplete
			lueDayMonthStd.Properties.AutoSearchColumnIndex = 0
			lueDayMonthStd.Properties.NullText = String.Empty

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			lueDayMonthStd.EditValue = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Resets the employee salary types grid
		''' </summary>
		Private Sub ResetEmployeeSalaryTypesGrid()

			' Reset the grid
			gvEmployeeSalaryTypeData.OptionsView.ShowIndicator = False
			gvEmployeeSalaryTypeData.OptionsSelection.EnableAppearanceFocusedRow = False

			gvEmployeeSalaryTypeData.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			gvEmployeeSalaryTypeData.Columns.Add(columnLANr)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "LABez"
			columnDescription.FieldName = "LABez"
			columnDescription.Visible = True
			gvEmployeeSalaryTypeData.Columns.Add(columnDescription)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Basis")
			columnBasis.Name = "Basis"
			columnBasis.FieldName = "Basis"
			columnBasis.Visible = True
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.Visible = True
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N2"
			gvEmployeeSalaryTypeData.Columns.Add(columnBasis)

			Dim columnAnsatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAnsatz.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
			columnAnsatz.Name = "Ansatz"
			columnAnsatz.FieldName = "Ansatz"
			columnAnsatz.Visible = True
			columnAnsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnsatz.AppearanceHeader.Options.UseTextOptions = True
			columnAnsatz.Visible = True
			columnAnsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAnsatz.DisplayFormat.FormatString = "N2"
			gvEmployeeSalaryTypeData.Columns.Add(columnAnsatz)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.Visible = True
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvEmployeeSalaryTypeData.Columns.Add(columnBetrag)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdEmployeeSalaryTypeData.DataSource = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Resets the customer salary types grid
		''' </summary>
		Private Sub ResetCustomerSalaryTypesGrid()

			' Reset the grid
			gvCustomerSalaryTypeData.OptionsView.ShowIndicator = False
			gvCustomerSalaryTypeData.OptionsSelection.EnableAppearanceFocusedRow = False

			gvCustomerSalaryTypeData.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			gvCustomerSalaryTypeData.Columns.Add(columnLANr)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "LABez"
			columnDescription.FieldName = "LABez"
			columnDescription.Visible = True
			gvCustomerSalaryTypeData.Columns.Add(columnDescription)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Basis")
			columnBasis.Name = "Basis"
			columnBasis.FieldName = "Basis"
			columnBasis.Visible = True
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.Visible = True
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N2"
			gvCustomerSalaryTypeData.Columns.Add(columnBasis)

			Dim columnAnsatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAnsatz.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
			columnAnsatz.Name = "Ansatz"
			columnAnsatz.FieldName = "Ansatz"
			columnAnsatz.Visible = True
			columnAnsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnsatz.AppearanceHeader.Options.UseTextOptions = True
			columnAnsatz.Visible = True
			columnAnsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAnsatz.DisplayFormat.FormatString = "N2"
			gvCustomerSalaryTypeData.Columns.Add(columnAnsatz)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.Visible = True
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvCustomerSalaryTypeData.Columns.Add(columnBetrag)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdCustomerSalaryTypeData.DataSource = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Loads the drop down data.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadDropDownData(ByVal esNr As Integer) As Boolean
			Dim success As Boolean = True

			success = success AndAlso LoadESSalaryDropDownData(esNr)
			success = success AndAlso LoadDayMonthStdDropDownData()
			Return success
		End Function


		''' <summary>
		''' Loads ES salary drop down data.
		''' </summary>
		Private Function LoadESSalaryDropDownData(ByVal esNr As Integer) As Boolean

			m_ESSalaryList = m_ESDataAccess.LoadESSalaryData(esNr)

			If m_ESSalaryList Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht geladen werden."))
				Return False
			End If

			lueESSalary.Properties.DataSource = m_ESSalaryList
			lueESSalary.Properties.ForceInitialize()

			Return True
		End Function


		''' <summary>
		''' Loads the day, month, std drop down data.
		''' </summary>
		Private Function LoadDayMonthStdDropDownData() As Boolean
			Dim dayMonthStdData = New List(Of DayMonthStdViewData) From {
			New DayMonthStdViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Stunden"), .Value = 1},
			New DayMonthStdViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Tag"), .Value = 2},
			New DayMonthStdViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Monat"), .Value = 3},
			New DayMonthStdViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Kilometer"), .Value = 4},
			New DayMonthStdViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Woche"), .Value = 5}
			}

			lueDayMonthStd.Properties.DataSource = dayMonthStdData
			lueDayMonthStd.Properties.ForceInitialize()

			Return True
		End Function

		''' <summary>
		''' Loads employee and customer ES salary type data.
		''' </summary>
		''' <param name="esNr">The ES number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadESEmployeeAndCustomerSalaryTypeData(ByVal esNr As Integer) As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadESEmployeeSalaryTypeData(esNr)
			success = success AndAlso LoadESCustomerSalaryTypeData(esNr)

			Return success
		End Function

		''' <summary>
		''' Loads employee ES salary type data.
		''' </summary>
		''' <param name="esNr">The ES number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadESEmployeeSalaryTypeData(ByVal esNr As Integer) As Boolean

			Dim errorMsg = m_Translate.GetSafeTranslationValue("Lohnarten (Kandidat) konnten nicht geladen werden.")
			Dim completeList = New List(Of SP.DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

			Dim nonAssignedData = m_ESDataAccess.LoadESAdditionalSalaryTypeData(esNr, DatabaseAccess.ES.ESAdditionalSalaryType.Employee, Nothing, 0)

			If nonAssignedData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(errorMsg)
				Return False
			End If

			completeList.AddRange(nonAssignedData)

			If Not m_UCMediator.SelectedESSalaryOnSalaryDataTab Is Nothing Then
				Dim dataAssignedToSelectedESLohnNr = m_ESDataAccess.LoadESAdditionalSalaryTypeData(esNr, DatabaseAccess.ES.ESAdditionalSalaryType.Employee, Nothing, m_UCMediator.SelectedESSalaryOnSalaryDataTab.ESLohnNr)

				If dataAssignedToSelectedESLohnNr Is Nothing Then
					m_UtilityUI.ShowErrorDialog(errorMsg)
					Return False
				End If

				completeList.AddRange(dataAssignedToSelectedESLohnNr)
			End If

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdEmployeeSalaryTypeData.DataSource = completeList
			m_SuppressUIEvents = suppressUIEventsState

			Return True
		End Function

		''' <summary>
		''' Loads customer ES salary type data.
		''' </summary>
		''' <param name="esNr">The ES number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadESCustomerSalaryTypeData(ByVal esNr As Integer) As Boolean
			Dim errorMsg = m_Translate.GetSafeTranslationValue("Lohnarten (Kunde) konnten nicht geladen werden.")
			Dim completeList = New List(Of SP.DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

			Dim nonAssignedData = m_ESDataAccess.LoadESAdditionalSalaryTypeData(esNr, DatabaseAccess.ES.ESAdditionalSalaryType.Customer, Nothing, 0)

			If nonAssignedData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(errorMsg)
				Return False
			End If

			completeList.AddRange(nonAssignedData)

			If Not m_UCMediator.SelectedESSalaryOnSalaryDataTab Is Nothing Then
				Dim dataAssignedToSelectedESLohnNr = m_ESDataAccess.LoadESAdditionalSalaryTypeData(esNr, DatabaseAccess.ES.ESAdditionalSalaryType.Customer, Nothing, m_UCMediator.SelectedESSalaryOnSalaryDataTab.ESLohnNr)

				If dataAssignedToSelectedESLohnNr Is Nothing Then
					m_UtilityUI.ShowErrorDialog(errorMsg)
					Return False
				End If

				completeList.AddRange(dataAssignedToSelectedESLohnNr)
			End If

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdCustomerSalaryTypeData.DataSource = completeList
			m_SuppressUIEvents = suppressUIEventsState

			Return True
		End Function


		''' <summary>
		''' Handles edit value changing event of txtAnzahl, txtBasis and txtAnsatz.
		''' </summary>
		Private Sub OnLAValue_EditValueChanging(sender As System.Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtAnsatz.EditValueChanging

			If m_SuppressUIEvents Then
				Return
			End If

			If sender Is txtAnsatz Then
				e.Cancel = Not m_CurrentLAValueUpperBounds.IsAnsatzValueInBoundary(Convert.ToDecimal(e.NewValue))
			End If

		End Sub

		''' <summary>
		''' Handles change of txtAnzahl, txtBasis and txtAnsatz EditValue change.
		''' </summary>
		Private Sub OnLAValue_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles txtAnsatz.EditValueChanged

			If m_SuppressUIEvents = True Then
				Return
			End If

			HandleChangeOfAnzahlBasisOrAnsatzValue()

		End Sub

		''' <summary>
		''' Handles change of Anzahl, Basis or Ansatz value.
		''' </summary>
		Private Sub HandleChangeOfAnzahlBasisOrAnsatzValue()

			CalculateBetrag()

		End Sub

		''' <summary>
		''' Handles change of lueLAData.
		''' </summary>
		Private Sub OnLueLAData_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueLAData.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim la = SelectedLA
			m_RPLValueCalculator = New RPLValueCalculator(m_InitializationData)
			If Not la Is Nothing Then
				Dim selectedESSalaryOnSalaryDataTab = SelectedESSalaryData
				If selectedESSalaryOnSalaryDataTab Is Nothing Then selectedESSalaryOnSalaryDataTab = m_UCMediator.SelectedESSalaryOnSalaryDataTab

				Dim esParameters As New RPLSalaryTypeValuesCalcESParams(selectedESSalaryOnSalaryDataTab.ESNr, selectedESSalaryOnSalaryDataTab.Tarif, selectedESSalaryOnSalaryDataTab.GrundLohn, selectedESSalaryOnSalaryDataTab.MWSTBetrag)
				esParameters.LohnFeierProz = selectedESSalaryOnSalaryDataTab.FeierProz
				esParameters.LohnFerienProz = selectedESSalaryOnSalaryDataTab.FerienProz
				esParameters.Lohn13Proz = selectedESSalaryOnSalaryDataTab.Lohn13Proz
				esParameters.Stundenlohn = selectedESSalaryOnSalaryDataTab.StundenLohn
				Dim laParameters As New RPLSalaryTypeValuesCalcLAParams(la.LANr, la.TypeAnsatz, la.TypeBasis, la.FixAnsatz, la.MABasVar, la.KDBasis, la.MWSTPflichtig)

				If la.TypeAnsatz = 2 Then
					txtAnsatz.EditValue = la.FixAnsatz

					RememberLAUpperBoundValuesBasedOnCurrentLAValues()
				End If


				If (la.MABasVar = "8000" OrElse la.MABasVar = "8001" OrElse la.MABasVar = "8002" OrElse la.MABasVar = "8003") OrElse (la.KDBasis = "8000" OrElse la.KDBasis = "8001" OrElse la.KDBasis = "8002" OrElse la.KDBasis = "8003") Then
					Dim mdYear As Integer
					If selectedESSalaryOnSalaryDataTab.LOVon Is Nothing Then mdYear = Now.Year Else mdYear = Year(selectedESSalaryOnSalaryDataTab.LOVon)
					Dim calculationParameters As New RPLSalaryTypeValuesCalcParams(selectedESSalaryOnSalaryDataTab.EmployeeNumber, DefaultMwStValue(mdYear), esParameters, laParameters)

					Dim result As RPLSalaryTypeValuesCalcuResult = Nothing
					Select Case m_CurrentDetailType
						Case ESAdditionalSalaryType.Employee
							result = m_RPLValueCalculator.CalculateSalaryTypeValuesForEmployee(calculationParameters)

						Case (ESAdditionalSalaryType.Customer)
							result = m_RPLValueCalculator.CalculateSalaryTypeValuesForCustomer(calculationParameters)

						Case Else
							' This case should never happen
							result = New RPLSalaryTypeValuesCalcuResult
					End Select

					txtBasis.EditValue = result.Basis

					RememberLAUpperBoundValuesBasedOnCurrentLAValues()
				Else
					txtBasis.EditValue = la.FixBasis

				End If

			End If

		End Sub

		''' <summary>
		''' Handles click on new ES_MA_LA button.
		''' </summary>
		Private Sub OnBtnAddNewESMALA_Click(sender As System.Object, e As System.EventArgs) Handles btnAddNewESMALA.Click
			m_CurrentDetailESLANr = Nothing
			m_CurrentDetailType = ESAdditionalSalaryType.Employee

			ClearESLADetailControls()
			lblInfo.Text = m_Translate.GetSafeTranslationValue("Bitte geben Sie Daten für den Kandidat ein")

			Dim selectedESSalaryOnSalaryDataTab = m_UCMediator.SelectedESSalaryOnSalaryDataTab
			lueESSalary.EditValue = If(selectedESSalaryOnSalaryDataTab Is Nothing, Nothing, selectedESSalaryOnSalaryDataTab.ESLohnNr)

			btnSave.Enabled = True

		End Sub

		''' <summary>
		''' Handles click on new ES_KD_LA button.
		''' </summary>
		Private Sub OnBtnAddNewESKDLA_Click(sender As System.Object, e As System.EventArgs) Handles btnAddNewESKDLA.Click
			m_CurrentDetailESLANr = Nothing
			m_CurrentDetailType = ESAdditionalSalaryType.Customer

			ClearESLADetailControls()
			lblInfo.Text = m_Translate.GetSafeTranslationValue("Bitte geben Sie Daten für den Kunden ein")

			Dim selectedESSalaryOnSalaryDataTab = m_UCMediator.SelectedESSalaryOnSalaryDataTab
			lueESSalary.EditValue = If(selectedESSalaryOnSalaryDataTab Is Nothing, Nothing, selectedESSalaryOnSalaryDataTab.ESLohnNr)

			btnSave.Enabled = True

		End Sub

		''' <summary>
		''' Handles click on save button.
		''' </summary>
		Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

			If ValidateESLAInputData() Then

				Dim esEmplOrCustLAData As ESEmployeeAndCustomerLAData = Nothing

				If Not m_CurrentDetailESLANr.HasValue Then
					esEmplOrCustLAData = New ESEmployeeAndCustomerLAData

					Select Case m_CurrentDetailType
						Case ESAdditionalSalaryType.Customer
							esEmplOrCustLAData.CustomerNumber = m_UCMediator.CustomerNumber
						Case ESAdditionalSalaryType.Employee
							esEmplOrCustLAData.EmployeeNumber = m_UCMediator.EmployeeNumber
						Case Else
							' Illegal type.
							Return
					End Select

				Else

					Dim esEmplOrCustLADataList = m_ESDataAccess.LoadESAdditionalSalaryTypeData(m_CurrentESNr, m_CurrentDetailType, m_CurrentDetailESLANr)

					If esEmplOrCustLADataList Is Nothing OrElse Not esEmplOrCustLADataList.Count = 1 Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
						Return
					End If

					esEmplOrCustLAData = esEmplOrCustLADataList(0)
				End If

				Dim la = SelectedLA

				esEmplOrCustLAData.ESNr = m_CurrentESNr
				esEmplOrCustLAData.LANr = la.LANr
				esEmplOrCustLAData.LABez = la.LALoText
				esEmplOrCustLAData.Betrag = Convert.ToDouble(txtBetrag.EditValue)
				esEmplOrCustLAData.Ansatz = Convert.ToDouble(txtAnsatz.EditValue)
				esEmplOrCustLAData.Basis = Convert.ToDouble(txtBasis.EditValue)
				esEmplOrCustLAData.Tag = (lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_DAY)
				esEmplOrCustLAData.Monat = (lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_MONTH)
				esEmplOrCustLAData.Std = (lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_STD)
				esEmplOrCustLAData.Kilometer = (lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_KILOMETER)
				esEmplOrCustLAData.Week = (lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_WEEK)

				esEmplOrCustLAData.Vertrag = False
				esEmplOrCustLAData.Result = String.Empty
				esEmplOrCustLAData.ESLohnNr = If(lueESSalary.EditValue Is Nothing, 0, lueESSalary.EditValue)
				' KSTNr and currency are determined automatically.

				Dim success As Boolean = True

				' Insert or update interview
				If esEmplOrCustLAData.ID = 0 Then
					success = m_ESDataAccess.AddNewESAdditionalSalaryTypeData(m_CurrentDetailType, esEmplOrCustLAData, 0)
					m_CurrentDetailESLANr = esEmplOrCustLAData.ESLANr
				Else
					success = m_ESDataAccess.UpdateESAdditionalSalaryTypeData(m_CurrentDetailType, esEmplOrCustLAData)
				End If

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
				Else

					Select Case m_CurrentDetailType
						Case ESAdditionalSalaryType.Customer
							LoadESCustomerSalaryTypeData(m_CurrentESNr)
							FocusESKDLAData(m_CurrentDetailESLANr)
						Case ESAdditionalSalaryType.Employee
							LoadESEmployeeSalaryTypeData(m_CurrentESNr)
							FocusESMALAData(m_CurrentDetailESLANr)
					End Select
					m_UCMediator.SendESDataChangedNotification(m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentESNr)
				End If

			End If

			lblInfo.Text = String.Empty

		End Sub

		''' <summary>
		''' Handles key down on employee or customer salary type data grid.
		''' </summary>
		Private Sub OnGridSalaryTypeData_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdEmployeeSalaryTypeData.KeyDown, grdCustomerSalaryTypeData.KeyDown
			If m_InitializationData.MDData.ClosedMD = 1 Then Return
			Dim type As ESAdditionalSalaryType = ESAdditionalSalaryType.Employee
			If Object.ReferenceEquals(sender, grdEmployeeSalaryTypeData) Then
				type = ESAdditionalSalaryType.Employee
			Else
				type = ESAdditionalSalaryType.Customer
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim esSalaryData = CType(grdView.GetRow(selectedRows(0)), ESEmployeeAndCustomerLAData)

						If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																						m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
							Return
						End If

						Dim result = m_ESDataAccess.DeleteESAdditionalSalaryTypeData(esSalaryData.ID, type, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

						Select Case result
							Case DeleteESSalaryTypeResult.ResultCanNotDeleteBecauseOfRPL
								m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Daten können nich gelöscht werden, da sie bereits mit einem Rapport verbunden sind."))
							Case DeleteESSalaryTypeResult.ResultDeleteError
								m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnte nicht gelöscht werden."))

							Case DeleteESSalaryTypeResult.ResultDeleteOk

								If type = ESAdditionalSalaryType.Employee Then
									LoadESEmployeeSalaryTypeData(m_CurrentESNr)
								Else
									LoadESCustomerSalaryTypeData(m_CurrentESNr)
								End If

								m_CurrentDetailESLANr = Nothing
								ClearESLADetailControls()

								btnSave.Enabled = False

						End Select

					End If
				End If
			End If

		End Sub

		''' <summary>
		''' Handles focus change of employee salary type data row.
		''' </summary>
		Private Sub OngvEmployeeSalaryTypeData_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvEmployeeSalaryTypeData.FocusedRowChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedESMALA = SelectedESMALAData

			If Not selectedESMALA Is Nothing Then
				PresentAdditionalSalaryTypeData(ESAdditionalSalaryType.Employee, selectedESMALA)
			End If

		End Sub

		''' <summary>
		''' Handles row click of employee salary type data row.
		''' </summary>
		Private Sub OngvEmployeeSalaryTypeData_RowClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles gvEmployeeSalaryTypeData.RowClick

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedESMALA = SelectedESMALAData

			If Not selectedESMALA Is Nothing Then
				PresentAdditionalSalaryTypeData(ESAdditionalSalaryType.Employee, selectedESMALA)
			End If

		End Sub

		''' <summary>
		''' Handles row style event of employee salary type data row.
		''' </summary>
		Private Sub OnGvEmployeeSalaryTaypeData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvEmployeeSalaryTypeData.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim rowData = CType(view.GetRow(e.RowHandle), ESEmployeeAndCustomerLAData)

				If (rowData.ESLohnNr = 0) Then
					e.Appearance.BackColor = Color.Yellow
				End If

			End If

		End Sub

		''' <summary>
		''' Handles focus change of customer salary type data row.
		''' </summary>
		Private Sub OnGvCustomerSalaryTypeData_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvCustomerSalaryTypeData.FocusedRowChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedESKDLA = SelectedESKDLAData

			If Not selectedESKDLA Is Nothing Then
				PresentAdditionalSalaryTypeData(ESAdditionalSalaryType.Customer, selectedESKDLA)
			End If

		End Sub

		''' <summary>
		''' Handles row click of customer salary type data row.
		''' </summary>
		Private Sub OnGvCustomerSalaryTypeData_RowClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles gvCustomerSalaryTypeData.RowClick

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedESKDLA = SelectedESKDLAData

			If Not selectedESKDLA Is Nothing Then
				PresentAdditionalSalaryTypeData(ESAdditionalSalaryType.Customer, selectedESKDLA)
			End If

		End Sub

		''' <summary>
		''' Handles row style event of customer salary type data row.
		''' </summary>
		Private Sub OnGvCustomerSalaryTaypeData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvCustomerSalaryTypeData.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim rowData = CType(view.GetRow(e.RowHandle), ESEmployeeAndCustomerLAData)

				If (rowData.ESLohnNr = 0) Then
					e.Appearance.BackColor = Color.Yellow
				End If

			End If

		End Sub

		''' <summary>
		''' Handles leave event of basis value.
		''' </summary>
		Private Sub OnTxtBasis_Leave(sender As System.Object, e As System.EventArgs) Handles txtBasis.Leave
			CalculateBetrag()
		End Sub

		''' <summary>
		''' Handles leave event of ansatz value.
		''' </summary>
		Private Sub OnTxtAnsatz_Leave(sender As System.Object, e As System.EventArgs) Handles txtAnsatz.Leave
			CalculateBetrag()
		End Sub

		''' <summary>
		''' Calculates The betrag value.
		''' </summary>
		Private Sub CalculateBetrag()
			txtBetrag.EditValue = txtBasis.EditValue * txtAnsatz.EditValue / 100D
		End Sub

		''' <summary>
		''' Validates ESLA input data.
		''' </summary>
		Private Function ValidateESLAInputData() As Boolean

			Dim missingFieldText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorTextZeroAmount As String = "Der Betrag 0.00 kann nicht erfasst werden. Bitte kontrollieren Sie Ihre Eingabe."

			Dim isValid As Boolean = True

			isValid = isValid AndAlso m_InitializationData.MDData.ClosedMD = 0
			isValid = isValid And SetErrorIfInvalid(lueLAData, errorProviderLAData, lueLAData.EditValue Is Nothing OrElse lueLAData.EditValue = 0, missingFieldText)
			isValid = isValid And SetErrorIfInvalid(txtBetrag, errorProviderLAData, txtBetrag.EditValue <= 0, errorTextZeroAmount)

			Return isValid
		End Function

		''' <summary>
		''' Clears ES LA details controls
		''' </summary>
		Private Sub ClearESLADetailControls()

			lueESSalary.EditValue = Nothing
			lueLAData.EditValue = Nothing
			lueDayMonthStd.EditValue = Nothing

			txtBasis.EditValue = 0D
			txtAnsatz.EditValue = 0D
			txtBetrag.EditValue = 0D

			lblInfo.Text = String.Empty

			errorProviderLAData.Clear()

		End Sub

		''' <summary>
		''' Presents the additional salary type data.
		''' </summary>
		''' <param name="type">The type.</param>
		''' <param name="salaryTypeData">The additional salary type data.</param>
		Private Sub PresentAdditionalSalaryTypeData(ByVal type As ESAdditionalSalaryType, ByVal salaryTypeData As ESEmployeeAndCustomerLAData)

			m_CurrentDetailType = type
			m_CurrentDetailESLANr = salaryTypeData.ESLANr

			lueESSalary.EditValue = If(salaryTypeData.ESLohnNr = 0, Nothing, salaryTypeData.ESLohnNr)

			If Not m_LAList.Any(Function(data) data.LANr = salaryTypeData.LANr) Then
				Dim laData As New LAData() With {.LANr = salaryTypeData.LANr, .LALoText = salaryTypeData.LABez}

				Dim text = laData.DisplayText
				m_LAList.Add(laData)

			End If

			lueLAData.EditValue = salaryTypeData.LANr

			If salaryTypeData.Tag.HasValue AndAlso salaryTypeData.Tag.Value Then
				lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_DAY
			ElseIf salaryTypeData.Monat.HasValue AndAlso salaryTypeData.Monat.Value Then
				lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_MONTH
			ElseIf salaryTypeData.Std.HasValue AndAlso salaryTypeData.Std.Value Then
				lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_STD
			ElseIf salaryTypeData.Kilometer.HasValue AndAlso salaryTypeData.Kilometer.Value Then
				lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_KILOMETER
			ElseIf salaryTypeData.Week.HasValue AndAlso salaryTypeData.Week.Value Then
				lueDayMonthStd.EditValue = DayMonthStdViewData.VALUE_WEEK
			Else
				lueDayMonthStd.EditValue = Nothing
			End If

			txtBasis.EditValue = Convert.ToDecimal(salaryTypeData.Basis)
			txtAnsatz.EditValue = Convert.ToDecimal(salaryTypeData.Ansatz)
			txtBetrag.EditValue = Convert.ToDouble(salaryTypeData.Betrag)

			lblInfo.Text = String.Empty

			btnSave.Enabled = True
		End Sub

		''' <summary>
		''' Focuses an ES_KD_LA data record.
		''' </summary>
		''' <param name="esLANr">The ESLANr.</param>
		Private Sub FocusESKDLAData(ByVal esLANr As Integer)

			If Not grdCustomerSalaryTypeData.DataSource Is Nothing Then

				Dim esSalaryData = CType(gvCustomerSalaryTypeData.DataSource, List(Of ESEmployeeAndCustomerLAData))

				Dim index = esSalaryData.ToList().FindIndex(Function(data) data.ESLANr = esLANr)

				Dim suppressState = m_SuppressUIEvents
				m_SuppressUIEvents = True
				Dim rowHandle = gvCustomerSalaryTypeData.GetRowHandle(index)
				gvCustomerSalaryTypeData.FocusedRowHandle = rowHandle
				m_SuppressUIEvents = suppressState
			End If

		End Sub

		''' <summary>
		''' Focuses an ES_MA_LA data record.
		''' </summary>
		''' <param name="esLANr">The ESLANr.</param>
		Private Sub FocusESMALAData(ByVal esLANr As Integer)

			If Not grdEmployeeSalaryTypeData.DataSource Is Nothing Then

				Dim esSalaryData = CType(gvEmployeeSalaryTypeData.DataSource, List(Of ESEmployeeAndCustomerLAData))

				Dim index = esSalaryData.ToList().FindIndex(Function(data) data.ESLANr = esLANr)

				Dim suppressState = m_SuppressUIEvents
				m_SuppressUIEvents = True
				Dim rowHandle = gvEmployeeSalaryTypeData.GetRowHandle(index)
				gvEmployeeSalaryTypeData.FocusedRowHandle = rowHandle
				m_SuppressUIEvents = suppressState
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


		''' <summary>
		''' Sets the LA upper bound values based on current LA values.
		''' </summary>
		Private Sub RememberLAUpperBoundValuesBasedOnCurrentLAValues()

			Dim la = SelectedLA

			m_CurrentLAValueUpperBounds.Reset()
			txtAnsatz.ToolTip = String.Empty

			If la Is Nothing Then
				Return
			End If

			' Remember upper bounds
			Dim MIN_VALUE_STR As String = m_Translate.GetSafeTranslationValue("Minimaler")
			Dim MAX_VALUE_STR As String = m_Translate.GetSafeTranslationValue("Maximaler")
			Dim Wert_STR As String = m_Translate.GetSafeTranslationValue("Wert")

			If Not la.AllowMoreAnsatz AndAlso la.TypeAnsatz.HasValue AndAlso la.TypeAnsatz = 2 Then
				m_CurrentLAValueUpperBounds.AnsatzBoundaryValue = txtAnsatz.EditValue
				txtAnsatz.ToolTip = String.Format("{0} {1}: {2}", If(txtAnsatz.EditValue < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, txtAnsatz.EditValue)
			End If

			If Not la.AllowMoreBasis AndAlso la.TypeBasis.HasValue AndAlso la.TypeBasis = 2 Then
				m_CurrentLAValueUpperBounds.BasisBoundaryValue = txtBasis.EditValue
				txtBasis.ToolTip = String.Format("{0} {1}: {2}", If(txtBasis.EditValue < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, txtBasis.EditValue)
			End If

		End Sub


#End Region

#Region "View helper classes"

		''' <summary>
		''' DayMonthStd view data.
		''' </summary>
		Class DayMonthStdViewData

#Region "Public Consts"

			Public Const VALUE_STD As Integer = 1
			Public Const VALUE_DAY As Integer = 2
			Public Const VALUE_MONTH As Integer = 3
			Public Const VALUE_KILOMETER As Integer = 4
			Public Const VALUE_WEEK As Integer = 5


#End Region

#Region "Public Properties"

			Public Property DisplayText As String
			Public Property Value As Integer

			''' <summary>
			''' Gets boolean flag indicating if the value represents a day.
			''' </summary>
			''' <returns>Boolean flag indicating if the value represents a day.</returns>
			Public ReadOnly Property IsDay
				Get
					Return Value = VALUE_DAY
				End Get
			End Property

			''' <summary>
			''' Gets boolean flag indicating if the value represents a month.
			''' </summary>
			''' <returns>Boolean flag indicating if the value represents a month.</returns>
			Public ReadOnly Property IsMonth
				Get
					Return Value = VALUE_MONTH
				End Get
			End Property

			''' <summary>
			''' Gets boolean flag indicating if the value represents a Std.
			''' </summary>
			''' <returns>Boolean flag indicating if the value represents a Std.</returns>
			Public ReadOnly Property IsStd
				Get
					Return Value = VALUE_STD
				End Get
			End Property

			''' <summary>
			''' Gets boolean flag indicating if the value represents a KM.
			''' </summary>
			''' <returns>Boolean flag indicating if the value represents a KM.</returns>
			Public ReadOnly Property IsKM
				Get
					Return Value = VALUE_KILOMETER
				End Get
			End Property

			''' <summary>
			''' Gets boolean flag indicating if the value represents a Week.
			''' </summary>
			''' <returns>Boolean flag indicating if the value represents a Week.</returns>
			Public ReadOnly Property IsWeek
				Get
					Return Value = VALUE_WEEK
				End Get
			End Property

#End Region

		End Class

#End Region


		''' <summary>
		''' LA value bounds
		''' </summary>
		Class LAValueUpperBounds
#Region "Public Properties"

			Public Property AnzahlBoundaryValue As Decimal?
			Public Property BasisBoundaryValue As Decimal?
			Public Property AnsatzBoundaryValue As Decimal?

#End Region

#Region "Public Methods"

			''' <summary>
			''' Resets the values.
			''' </summary>
			Public Sub Reset()
				AnzahlBoundaryValue = Nothing
				BasisBoundaryValue = Nothing
				AnsatzBoundaryValue = Nothing
			End Sub

			''' <summary>
			''' Checks if Anzahl value is in boundary.
			''' </summary>
			''' <param name="anzahl">The Anzahl value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Public Function IsAnzahlValueInBoundary(ByVal anzahl As Decimal) As Boolean
				Return IsValueInBoundary(anzahl, AnzahlBoundaryValue)
			End Function

			''' <summary>
			''' Checks if Basis value is in boundary.
			''' </summary>
			''' <param name="Basis">The Basis value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Public Function IsBasisValueInBoundary(ByVal basis As Decimal) As Boolean
				Return IsValueInBoundary(basis, BasisBoundaryValue)
			End Function

			''' <summary>
			''' Checks if Ansatz value is in boundary.
			''' </summary>
			''' <param name="Ansatz">The Ansatz value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Public Function IsAnsatzValueInBoundary(ByVal ansatz As Decimal) As Boolean
				Return IsValueInBoundary(ansatz, AnsatzBoundaryValue)
			End Function

#End Region

#Region "Private Methods"

			''' <summary>
			''' Checks if a value in in boundary.
			''' </summary>
			''' <param name="value">The value.</param>
			''' <param name="boundaryValue">The boundary value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Private Function IsValueInBoundary(ByVal value As Decimal, ByVal boundaryValue As Decimal?) As Boolean

				Dim inBoundary As Boolean = True

				If boundaryValue.HasValue Then
					If boundaryValue > 0 Then
						inBoundary = value <= boundaryValue
					Else
						inBoundary = value >= boundaryValue
					End If

				End If

				Return inBoundary
			End Function

#End Region

		End Class

	End Class

End Namespace
