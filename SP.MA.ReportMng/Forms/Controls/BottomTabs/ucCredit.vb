Imports DevExpress.XtraEditors.Controls
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.Infrastructure
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Listing

Namespace UI

	Public Class ucCredit


#Region "Private Consts"

		Private Const MANDANT_XML_SETTING_SPUTNIK_LICENCING_URI As String = "MD_{0}/Licencing"

#End Region

#Region "Private Fields"

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_path As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_EmployeeDataAccess As IEmployeeDatabaseAccess
		Private m_ListingDatabaseAccess As IListingDatabaseAccess
		Private m_Allowedemployeeweeklypament As Boolean

#End Region


#Region "constructor"

		Public Sub New()

			m_Mandant = New Mandant
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			m_Utility = New Utility

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

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

			m_EmployeeDataAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			Dim m_licencing_uri As String = String.Format(MANDANT_XML_SETTING_SPUTNIK_LICENCING_URI, m_InitializationData.MDData.MDNr)
			m_Allowedemployeeweeklypament = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year),
																																				String.Format("{0}/allowedemployeeweeklypayment", m_licencing_uri)), False)

		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_ReportNumber = Nothing

			Dim previousState = SetSuppressUIEventsState(True)

			' Einstellungen
			chkWeeklyPayment.Enabled = m_Allowedemployeeweeklypament
			chkWeeklyPayment.Checked = False

			' Rückstellung
			chkFerienBack.Checked = False
			chkFeiertagBack.Checked = False
			chkLohn13Back.Checked = False
			chkEnableGleitzeit.Checked = False

			' ---Reset drop downs, grids and lists---

			ResetGuthabenGrid()

			SetSuppressUIEventsState(previousState)

		End Sub

		''' <summary>
		''' Activates the control.
		''' </summary>
		Public Overrides Function Activate() As Boolean

			Dim success As Boolean = True

			If m_UCMediator.ActiveReportData Is Nothing Then
				Return False
			End If

			Dim rpNrToLoad = m_UCMediator.ActiveReportData.ReportData.RPNR

			If (Not IsReportDataLoaded OrElse (Not m_ReportNumber = rpNrToLoad)) Then
				CleanUp()

				success = success AndAlso LoadData()

				m_ReportNumber = IIf(success, rpNrToLoad, 0)

			End If

			Return success
		End Function

		''' <summary>
		''' Saves the tab page data.
		''' </summary>
		Public Overrides Function SaveReportData() As Boolean

			Dim success As Boolean = True

			Dim rpData = m_UCMediator.ActiveReportData

			If ((IsReportDataLoaded AndAlso
				m_ReportNumber = rpData.ReportData.RPNR)) Then

				Dim loSettings = m_EmployeeDataAccess.LoadEmployeeLOSettings(rpData.EmployeeOfActiveReport.EmployeeNumber)

				If loSettings Is Nothing Then
					Return False
				End If

				loSettings.WeeklyPayment = chkWeeklyPayment.Checked
				loSettings.FerienBack = chkFerienBack.Checked
				loSettings.FeierBack = chkFeiertagBack.Checked
				loSettings.Lohn13Back = chkLohn13Back.Checked
				loSettings.MAGleitzeit = chkEnableGleitzeit.Checked

				success = m_EmployeeDataAccess.UpdateEmployeeLOSettings(loSettings)

			End If

			Return success
		End Function

#End Region

#Region "Private Metods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Guthaben
			Me.grpguthaben.Text = m_Translate.GetSafeTranslationValue(Me.grpguthaben.Text)

			' Einstellungen
			grpEinstellungen.Text = m_Translate.GetSafeTranslationValue(grpEinstellungen.Text)
			chkWeeklyPayment.Text = m_Translate.GetSafeTranslationValue(chkWeeklyPayment.Text)

			' Rückstellungen
			grpRuekstellungen.Text = m_Translate.GetSafeTranslationValue(grpRuekstellungen.Text)
			chkFerienBack.Text = m_Translate.GetSafeTranslationValue(Me.chkFerienBack.Text)
			chkFeiertagBack.Text = m_Translate.GetSafeTranslationValue(Me.chkFeiertagBack.Text)
			chkLohn13Back.Text = m_Translate.GetSafeTranslationValue(Me.chkLohn13Back.Text)
			chkEnableGleitzeit.Text = m_Translate.GetSafeTranslationValue(Me.chkEnableGleitzeit.Text)

		End Sub

		''' <summary>
		''' Resets the Guthaben grid.
		''' </summary>
		Private Sub ResetGuthabenGrid()

			gvGuthaben.BorderStyle = BorderStyles.NoBorder
			gvGuthaben.OptionsView.ShowIndicator = False

			gvGuthaben.OptionsView.ShowColumnHeaders = False
			gvGuthaben.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
			gvGuthaben.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
			gvGuthaben.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
			gvGuthaben.OptionsSelection.EnableAppearanceFocusedRow = False
			gvGuthaben.OptionsSelection.EnableAppearanceHideSelection = False


			' Reset the grid
			gvGuthaben.Columns.Clear()

			Dim columnValueName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnValueName.Caption = "ValueName"
			columnValueName.Name = "ValueName"
			columnValueName.FieldName = "ValueName"
			columnValueName.Visible = True
			gvGuthaben.Columns.Add(columnValueName)

			Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
			columnValue.Caption = "Value"
			columnValue.Name = "Value"
			columnValue.FieldName = "Value"
			columnValue.Visible = True
			columnValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnValue.DisplayFormat.FormatString = "N"
			columnValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnValue.AppearanceHeader.Options.UseTextOptions = True
			columnValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnValue.AppearanceCell.Options.UseTextOptions = True
			gvGuthaben.Columns.Add(columnValue)

			Dim columnUnit As New DevExpress.XtraGrid.Columns.GridColumn()
			columnUnit.Caption = "Unit"
			columnUnit.Name = "Unit"
			columnUnit.FieldName = "Unit"
			columnUnit.Visible = True
			columnUnit.Width = 40
			gvGuthaben.Columns.Add(columnUnit)

		End Sub

		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadData() As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadCreditData()
			success = success AndAlso LoadRetractionData()

			Return success

		End Function

		''' <summary>
		''' Loads the credit data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCreditData() As Boolean

			Dim employeeNumber As Integer = m_UCMediator.ActiveReportData.EmployeeOfActiveReport.EmployeeNumber

			Dim success As Boolean = True
			Dim currency As String = String.Empty

			Dim employeeLOSetting = m_EmployeeDataAccess.LoadEmployeeLOSettingForMonthlySalaryMng(employeeNumber)

			If Not employeeLOSetting Is Nothing Then
				currency = employeeLOSetting.Currency
			Else
				success = False
			End If

			Dim listOfGuthabenViewData As New List(Of GuthabenViewData)

			Dim value As Decimal? = Nothing

			' Ferien
			value = GetFerGuthaben(employeeNumber, 0)
			If value > 0 Then
				Dim ferienGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Ferien"), .Value = value, .Unit = currency}
				listOfGuthabenViewData.Add(ferienGuthaben)
			End If

			' Feiertag
			value = GetFeierGuthaben(employeeNumber, 0)
			If value > 0 Then
				Dim feiertagsGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Feiertag"), .Value = value, .Unit = currency}
				listOfGuthabenViewData.Add(feiertagsGuthaben)
			End If

			' 13. Lohn
			value = Get13Guthaben(employeeNumber, 0)
			If value > 0 Then
				Dim lohn13Guthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("13. Lohn"), .Value = value, .Unit = currency}
				listOfGuthabenViewData.Add(lohn13Guthaben)
			End If

			' Darlehen 
			value = GetDarlehenGuthaben(employeeNumber, m_InitializationData.MDData.MDNr)
			If value > 0 Then
				Dim darlehnGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Darlehen"), .Value = value, .Unit = currency}
				listOfGuthabenViewData.Add(darlehnGuthaben)
			End If

			' Gleitzeit
			'GetAnzGStd(employeeNumber, cRestStd, cRestBetrag, m_InitializationData.MDData.MDNr)
			Dim data = m_ListingDatabaseAccess.LoadFlexibleWorkingHoursData(m_InitializationData.MDData.MDNr, employeeNumber)
			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Gleitzeit Daten konnten nicht geladen werden.")

			Else
				If data.CreditHours.GetValueOrDefault(0) <> 0 AndAlso data.CreditAmount.GetValueOrDefault(0) <> 0 Then
					Dim cRestStd As Decimal = data.CreditHours.GetValueOrDefault(0)
					Dim cRestBetrag As Decimal = data.CreditAmount.GetValueOrDefault(0)

					Dim gleitzeitStdGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Gleitzeit"), .Value = cRestStd, .Unit = m_Translate.GetSafeTranslationValue("Std")}
					listOfGuthabenViewData.Add(gleitzeitStdGuthaben)

					Dim gleitzeitBetragGuthaben As New GuthabenViewData With {.ValueName = String.Empty, .Value = cRestBetrag, .Unit = currency}
					listOfGuthabenViewData.Add(gleitzeitBetragGuthaben)
				End If

			End If

			' Nachtzulagen (LANr = 290)
			Dim cNachtStd As Decimal = 0
			Dim cNachtBetrag As Decimal = 0

			GetAnzNightStd(employeeNumber, cNachtStd, cNachtBetrag, m_InitializationData.MDData.MDNr)

			If cNachtStd > 0 Then

				Dim nachzulageStdGuthaben As New GuthabenViewData With {.ValueName = m_Translate.GetSafeTranslationValue("Nachtzulage"), .Value = cNachtStd, .Unit = m_Translate.GetSafeTranslationValue("Std")}
				listOfGuthabenViewData.Add(nachzulageStdGuthaben)

				Dim nachtzulageBetragGuthaben As New GuthabenViewData With {.ValueName = String.Empty, .Value = cNachtBetrag, .Unit = currency}
				listOfGuthabenViewData.Add(nachtzulageBetragGuthaben)

			End If

			' Apply data to gridview
			Dim previousState = SetSuppressUIEventsState(True)
			grdGuthaben.DataSource = listOfGuthabenViewData
			SetSuppressUIEventsState(previousState)

			Return success
		End Function

		''' <summary>
		''' Loads the retraction (Rueckstellung) data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadRetractionData() As Boolean

			Dim employeeNumber As Integer = m_UCMediator.ActiveReportData.EmployeeOfActiveReport.EmployeeNumber

			Dim employeeLOSettingsData As DataObjects.Salary.EmployeeLOSettingsData = m_EmployeeDataAccess.LoadEmployeeLOSettings(employeeNumber)

			If (employeeLOSettingsData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Lohneinststellungsdaten konnten nicht geladen werden."))
				Return False
			End If

			chkWeeklyPayment.Checked = employeeLOSettingsData.WeeklyPayment.GetValueOrDefault(False)
			chkFerienBack.Checked = employeeLOSettingsData.FerienBack.HasValue AndAlso employeeLOSettingsData.FerienBack = True
			chkFeiertagBack.Checked = employeeLOSettingsData.FeierBack.HasValue AndAlso employeeLOSettingsData.FeierBack = True
			chkLohn13Back.Checked = employeeLOSettingsData.Lohn13Back.HasValue AndAlso employeeLOSettingsData.Lohn13Back = True
			chkEnableGleitzeit.Checked = employeeLOSettingsData.MAGleitzeit.HasValue AndAlso employeeLOSettingsData.MAGleitzeit = True

			Return True

		End Function

#End Region

#Region "View helper classes"

		''' <summary>
		''' Guthaben list item view data.
		''' </summary>
		Class GuthabenViewData
			Public Property ValueName As String
			Public Property Value As Decimal
			Public Property Unit As String
		End Class

#End Region

	End Class

End Namespace