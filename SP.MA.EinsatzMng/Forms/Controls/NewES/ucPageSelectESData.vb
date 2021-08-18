Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure

Namespace UI

  Public Class ucPageSelectESData

#Region "Private Constants"

    Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"
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
    ''' The advisors.
    ''' </summary>
    Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

    ''' <summary>
    ''' The cost centers.
    ''' </summary>
    Private m_CostCenters As SP.DatabaseAccess.Common.DataObjects.CostCenters

    ''' <summary>
    ''' ES ende text if ES end  is not set.
    ''' </summary>
    Private m_ESEndebynull As String

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
        m_Mandant = New Mandant

      Catch ex As Exception
        m_Logger.LogError(ex.ToString)
      End Try

      AddHandler dateEditStartDate.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler dateEditEndDate.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueSuva.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueKst1.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueKst2.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueAdvisorMA.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueAdvisorKD.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueSubscriber.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueESEinstufung.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueBranch.ButtonClick, AddressOf OnDropDown_ButtonClick
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected ES data.
    ''' </summary>
    ''' <returns>ES data.</returns>
    Public ReadOnly Property SelectedESData As InitESData
      Get

        Dim time = CType(timeEdit.EditValue, DateTime).TimeOfDay

        Dim data As New InitESData With {
          .ESAls = txtEsAls.Text,
          .ESStartDate = dateEditStartDate.EditValue,
          .ESEndDate = dateEditEndDate.EditValue,
          .Uhrzeit = String.Format("{0:00}:{1:00}", time.Hours, time.Minutes),
          .SUVA = lueSuva.EditValue,
          .Kst1 = lueKst1.EditValue,
          .Kst2 = lueKst2.EditValue,
          .MA_KD_Berater = lblAdvisorCombinedText.Text,
          .Unterzeichner = lueSubscriber.EditValue,
          .ESEinstufung = lueESEinstufung.EditValue,
          .Branche = lueBranch.EditValue,
          .VakNr = If(Not PreselectionData Is Nothing AndAlso PreselectionData.VAKNr.HasValue, PreselectionData.VAKNr, 0),
          .PNr = If(Not PreselectionData Is Nothing AndAlso PreselectionData.PNR.HasValue, PreselectionData.PNR, 0)
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

        Dim mdNumber As Integer = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber

        m_ESEndebynull = m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/esendebynull", FORM_XML_MAIN_KEY))

        LoadDropDownData()

        success = success AndAlso LoadBranchDropDownData(m_UCMediator.SelectedCandidateAndCustomerData.CustomerData.CustomerNumber)

        PreselectData()

      End If

      m_SuppressUIEvents = False
      m_IsFirstPageActivation = False

      Return success
    End Function

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_IsFirstPageActivation = True

      m_Advisors = Nothing
      m_ESEndebynull = Nothing
      m_CostCenters = Nothing

      txtEsAls.Text = String.Empty
      txtEsAls.Properties.MaxLength = 1000

      dateEditStartDate.EditValue = Nothing
      dateEditEndDate.EditValue = Nothing

      Dim dt = DateTime.Now.Date ' Date part is not relevant here
      timeEdit.EditValue = New DateTime(dt.Year, dt.Month, dt.Day, 7, 0, 0, 0)

      lblDays.Text = String.Empty

      lblAdvisorCombinedText.Text = String.Empty

      '  Reset drop downs and lists

      ResetSuvaDropDown()
      ResetKstDropDown()
      ResetCustomerAdvisorDropDown()
      ResetEmployeeAdvisorDropDown()
      ResetSubscriberDropDown()
      ResetBranchDropDown()
      ResetESCategorizationDropDown()

      ErrorProvider.Clear()

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean

      Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorDeaktivatedAdvisorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein. Möglicherweise wurde der / die BeraterIn in der Benutzerverwaltung deaktiviert!")
			Dim errorTextInvalidStartOrEndDate As String = m_Translate.GetSafeTranslationValue("Einsatzende muss nach Startdatum liegen.")

			Dim employeeNumber As Integer = m_UCMediator.SelectedCandidateAndCustomerData.EmployeeData.EmployeeNumber
      Dim mandantNumber As Integer = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber

      Dim mustESKST1BeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
                                                     String.Format("{0}/kst1selectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
      Dim mustESKST2BeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
                                                           String.Format("{0}/kst2selectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
      Dim mustESEinstufungBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
                                                           String.Format("{0}/eseinstufungselectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
      Dim mustESBrancheBeSelected As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
                                                           String.Format("{0}/esbrancheselectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)

      Dim isValid As Boolean = True

      isValid = isValid And SetErrorIfInvalid(txtEsAls, ErrorProvider, String.IsNullOrWhiteSpace(txtEsAls.Text), errorText)
      isValid = isValid And SetErrorIfInvalid(dateEditStartDate, ErrorProvider, dateEditStartDate.EditValue Is Nothing, errorText)
      isValid = isValid And SetErrorIfInvalid(timeEdit, ErrorProvider, timeEdit.EditValue Is Nothing, errorText)

      isValid = isValid And SetErrorIfInvalid(lueSuva, ErrorProvider, lueSuva.EditValue Is Nothing, errorText)

      ' Check end date lies after start date
      If isValid AndAlso Not dateEditStartDate.EditValue Is Nothing AndAlso
                   Not dateEditEndDate.EditValue Is Nothing Then

        Dim startDateToCheck As DateTime? = dateEditStartDate.EditValue
        Dim endDateToCheck As DateTime? = dateEditEndDate.EditValue

        Dim isEndDateValid As Boolean = (endDateToCheck.Value.Date.CompareTo(startDateToCheck.Value.Date) >= 0)
        isValid = isValid And SetErrorIfInvalid(dateEditEndDate, ErrorProvider, Not isEndDateValid, errorTextInvalidStartOrEndDate)

      End If

      ' KST1, KST2

      If mustESKST1BeSelected And Not lueKst1.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueKst1, ErrorProvider, String.IsNullOrEmpty(lueKst1.EditValue), errorText)
      End If
      If mustESKST2BeSelected And Not lueKst2.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueKst2, ErrorProvider, String.IsNullOrEmpty(lueKst2.EditValue), errorText)
      End If
			isValid = isValid And SetErrorIfInvalid(lueAdvisorMA, ErrorProvider, lueAdvisorMA.EditValue Is Nothing OrElse lueAdvisorMA.Text.Trim = ",", If(lueAdvisorMA.EditValue Is Nothing, errorText, errorDeaktivatedAdvisorText))
			isValid = isValid And SetErrorIfInvalid(lueAdvisorKD, ErrorProvider, lueAdvisorKD.EditValue Is Nothing OrElse lueAdvisorKD.Text.Trim = ",", If(lueAdvisorKD.EditValue Is Nothing, errorText, errorDeaktivatedAdvisorText))
			isValid = isValid And SetErrorIfInvalid(lueSubscriber, ErrorProvider, lueSubscriber.EditValue Is Nothing OrElse lueSubscriber.Text.Trim = ",", If(lueSubscriber.EditValue Is Nothing, errorText, errorDeaktivatedAdvisorText))


			' branches and Einstufung

			If mustESEinstufungBeSelected And Not lueESEinstufung.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueESEinstufung, ErrorProvider, String.IsNullOrEmpty(lueESEinstufung.EditValue), errorText)
      End If
      If mustESBrancheBeSelected And Not lueBranch.Properties.DataSource Is Nothing Then
        isValid = isValid And SetErrorIfInvalid(lueBranch, ErrorProvider, String.IsNullOrEmpty(lueBranch.EditValue), errorText)
      End If


      ' Check for conflicting records.

      Dim uiUtilityFunctions As New ESValidationUtility(m_UCMediator.ESDbAccess, m_Translate)

      Dim startDate = dateEditStartDate.EditValue
      Dim endDateIfEmpty = New DateTime(3999, 12, 31)
      Dim endDate As DateTime = If(dateEditEndDate.EditValue Is Nothing, endDateIfEmpty, dateEditEndDate.EditValue)
      endDate = endDate.Date.AddDays(1).AddSeconds(-1) ' Last second of day

      ' Check for conflicting LO records.
      If isValid Then
        isValid = isValid And uiUtilityFunctions.CheckForConflictingLOInPeriod(employeeNumber, mandantNumber, startDate, endDate)
      End If

      ' Check for conflicting MonthClose records.
      If isValid Then
        isValid = isValid And uiUtilityFunctions.CheckForConflictingMonthCloseRecordsInPeriod(mandantNumber, startDate, endDate)
      End If

      Return isValid

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      ' Group Einsatzdaten
      Me.grpESData.Text = m_Translate.GetSafeTranslationValue(Me.grpESData.Text)
			Me.lblEinsatzAls.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatzAls.Text)
			Me.lblBeginntAm.Text = m_Translate.GetSafeTranslationValue(Me.lblBeginntAm.Text)
      Me.lblEndetAm.Text = m_Translate.GetSafeTranslationValue(Me.lblEndetAm.Text)
      Me.lblUhrzeit.Text = m_Translate.GetSafeTranslationValue(Me.lblUhrzeit.Text)
      Me.lblSuva.Text = m_Translate.GetSafeTranslationValue(Me.lblSuva.Text)

      ' Group Kostenteilung
      Me.grpKostenteilungData.Text = m_Translate.GetSafeTranslationValue(Me.grpKostenteilungData.Text)
      Me.lblKST1.Text = m_Translate.GetSafeTranslationValue(Me.lblKST1.Text, True)
      Me.lblKST2.Text = m_Translate.GetSafeTranslationValue(Me.lblKST2.Text, True)
			Me.lblBeraterIn.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterIn.Text, True)
      Me.lblBeraterMA.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterMA.Text)
      Me.lblBeraterKD.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterKD.Text)
      Me.lblUnterzeichner.Text = m_Translate.GetSafeTranslationValue(Me.lblUnterzeichner.Text)

      Me.grpEinstellungen.Text = m_Translate.GetSafeTranslationValue(Me.grpEinstellungen.Text, True)
      Me.lblEinsatzEinstufung.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatzEinstufung.Text, True)
      Me.lblBranche.Text = m_Translate.GetSafeTranslationValue(Me.lblBranche.Text, True)

    End Sub

    ''' <summary>
    ''' Resets the Suva drop down.
    ''' </summary>
    Private Sub ResetSuvaDropDown()

      lueSuva.Properties.DisplayMember = "TranslatedDescription"
      lueSuva.Properties.ValueMember = "GetField"

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
    ''' Resets the Kst1 and Kst2 drop down.
    ''' </summary>
    Private Sub ResetKstDropDown()
      'Kst1
      lueKst1.Properties.DisplayMember = "KSTBezeichnung"
      lueKst1.Properties.ValueMember = "KSTName"

      lueKst1.Properties.Columns.Clear()
      lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
      lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

      lueKst1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueKst1.Properties.SearchMode = SearchMode.AutoComplete
      lueKst1.Properties.AutoSearchColumnIndex = 1
      lueKst1.Properties.NullText = String.Empty
      lueKst1.EditValue = Nothing

      'Kst2
      lueKst2.Properties.DisplayMember = "KSTBezeichnung"
      lueKst2.Properties.ValueMember = "KSTName"

      lueKst2.Properties.Columns.Clear()
      lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
      lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

      lueKst2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueKst2.Properties.SearchMode = SearchMode.AutoComplete
      lueKst2.Properties.AutoSearchColumnIndex = 1
      lueKst2.Properties.NullText = String.Empty
      lueKst2.EditValue = Nothing
    End Sub

    ''' <summary>
    ''' Resets the customer advisors drop down.
    ''' </summary>
    Private Sub ResetCustomerAdvisorDropDown()

      lueAdvisorKD.Properties.DropDownRows = 20

      lueAdvisorKD.Properties.DisplayMember = "UserFullname"
      lueAdvisorKD.Properties.ValueMember = "KST"

      Dim columns = lueAdvisorKD.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("KST", 0))
      columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

      lueAdvisorKD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueAdvisorKD.Properties.SearchMode = SearchMode.AutoComplete
      lueAdvisorKD.Properties.AutoSearchColumnIndex = 1

      lueAdvisorKD.Properties.NullText = String.Empty
      lueAdvisorKD.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Resets the employee advisors drop down.
    ''' </summary>
    Private Sub ResetEmployeeAdvisorDropDown()

      lueAdvisorMA.Properties.DropDownRows = 20

      lueAdvisorMA.Properties.DisplayMember = "UserFullname"
      lueAdvisorMA.Properties.ValueMember = "KST"

      Dim columns = lueAdvisorMA.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("KST", 0))
      columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

      lueAdvisorMA.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueAdvisorMA.Properties.SearchMode = SearchMode.AutoComplete
      lueAdvisorMA.Properties.AutoSearchColumnIndex = 1

      lueAdvisorMA.Properties.NullText = String.Empty
      lueAdvisorMA.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Resets the subscriber drop down.
    ''' </summary>
    Private Sub ResetSubscriberDropDown()

      lueSubscriber.Properties.DropDownRows = 20

      lueSubscriber.Properties.DisplayMember = "UserFullnameReversedWithoutComma"
      lueSubscriber.Properties.ValueMember = "UserFullnameReversedWithoutComma"

      Dim columns = lueSubscriber.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("UserFullnameReversedWithoutComma", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

      lueSubscriber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueSubscriber.Properties.SearchMode = SearchMode.AutoComplete
      lueSubscriber.Properties.AutoSearchColumnIndex = 1

      lueSubscriber.Properties.NullText = String.Empty
      lueSubscriber.EditValue = Nothing

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
    ''' Loads drop down data.
    ''' </summary>
    Private Function LoadDropDownData() As Boolean

      Dim success As Boolean = True
      success = success AndAlso LoadSuvaDropDownData()
      success = success AndAlso LoadKst1DropDownData()

      success = success AndAlso LoadAdvisorDropDownData()
      success = success AndAlso LoadESCategorizationDropDownData()

      Return success
    End Function

    ''' <summary>
    ''' Loads the Suva drop down data.
    ''' </summary>
    Private Function LoadSuvaDropDownData() As Boolean
      Dim suvaData = m_UCMediator.ESDbAccess.LoadSuvaData()

      If (suvaData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Suva-Daten konnten nicht geladen werden."))
      End If

      lueSuva.Properties.DataSource = suvaData
      lueSuva.Properties.ForceInitialize()

      Return Not suvaData Is Nothing
    End Function

    ''' <summary>
    ''' Loads the Kst1 data.
    ''' </summary>
    Private Function LoadKst1DropDownData() As Boolean
      ' Load data
      m_CostCenters = m_UCMediator.CommonDbAccess.LoadCostCenters()

      If m_CostCenters Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstellen konnten nicht geladen werden."))
        Return False
      End If

      ' Kst1
      lueKst1.EditValue = Nothing
      lueKst1.Properties.DataSource = m_CostCenters.CostCenter1
      lueKst1.Properties.ForceInitialize()

      ' Kst2
      lueKst2.EditValue = Nothing
      lueKst2.Properties.DataSource = Nothing
      lueKst2.Properties.ForceInitialize()

      Return True

    End Function

    ''' <summary>
    ''' Loads the Kst2 drop down data.
    ''' </summary>
    Private Sub LoadKst2DropDown()

      If (m_CostCenters Is Nothing) Then
        Return
      End If

      Dim kst1Name = lueKst1.EditValue
      Dim kst2Data = m_CostCenters.GetCostCenter2ForCostCenter1(kst1Name)

      ' Kst2
      lueKst2.EditValue = Nothing
      lueKst2.Properties.DataSource = kst2Data
      lueKst2.Properties.ForceInitialize()

    End Sub

    ''' <summary>
    ''' Loads the advisor drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadAdvisorDropDownData() As Boolean

			m_Advisors = m_UCMediator.CommonDbAccess.LoadActivatedAdvisorData()

			If m_Advisors Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
      End If

      ' Customer advisor
      lueAdvisorKD.Properties.DataSource = m_Advisors
      lueAdvisorKD.Properties.ForceInitialize()

      ' Employee advisor
      lueAdvisorMA.Properties.DataSource = m_Advisors
      lueAdvisorMA.Properties.ForceInitialize()

      ' Subscriber
      lueSubscriber.Properties.DataSource = m_Advisors
      lueSubscriber.Properties.ForceInitialize()

      Return Not m_Advisors Is Nothing
    End Function

		''' <summary>
		''' Loads the Kst1 and Kst2 values for given User.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorKst12Data() As Boolean

			Dim m_selectedAdvisor = lueAdvisorMA.EditValue
			If m_selectedAdvisor Is Nothing Then
				lueKst1.EditValue = Nothing
				lueKst2.EditValue = Nothing
				lueKst1.Properties.DataSource = Nothing
				lueKst2.Properties.DataSource = Nothing

				Return False
			End If
      Dim givenAdvisors = m_UCMediator.CommonDbAccess.LoadAdvisorDataforGivenAdvisor(m_selectedAdvisor)

      If givenAdvisors Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die gesuchten Beraterdaten konnten nicht geladen werden."))
      End If

      LoadKst1DropDownData()
      LoadKst2DropDown()

      lueKst1.EditValue = givenAdvisors.KST1
      lueKst2.EditValue = givenAdvisors.KST2

      Return Not givenAdvisors Is Nothing
		End Function

    ''' <summary>
    ''' Loads the branch drop down data.
    ''' </summary>
    '''<param name="customerNumber">The customer number.</param>
    Private Function LoadBranchDropDownData(ByVal customerNumber As Integer) As Boolean
      ' Load data
      Dim allBranchData As List(Of DatabaseAccess.Common.DataObjects.BranchData) = m_UCMediator.CommonDbAccess.LoadBranchData()
      Dim customerAssignedBranchData = m_UCMediator.CustomerDbAccess.LoadAssignedSectorDataOfCustomer(customerNumber)

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

							' The branch could be found -> add to merged results.
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
    ''' Loads the ES categorization drop down data.
    ''' </summary>
    Private Function LoadESCategorizationDropDownData() As Boolean
      ' Load data
      Dim categorizationData = m_UCMediator.ESDbAccess.LoadESCategorizationData()

      If (categorizationData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatzeinstufungen konnten nicht geladen werden."))
      End If

      lueESEinstufung.Properties.DataSource = categorizationData
      lueESEinstufung.Properties.ForceInitialize()

      Return Not categorizationData Is Nothing
    End Function

		''' <summary>
		''' Handles click on qualification button.
		''' </summary>
		Private Sub OnTxtQulification_ButtonClick(sender As System.Object, e As System.EventArgs) Handles txtEsAls.ButtonClick
			' Show profession selection dialog.
			Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
			obj.SelectMultirecords = False
			Dim success = True

			Dim selectedEmployee = m_UCMediator.SelectedCandidateAndCustomerData.EmployeeData

			success = success AndAlso obj.LoadQualificationData(selectedEmployee.Gender)
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

    End Sub

    ''' <summary>
    ''' Handles change of end date.
    ''' </summary>
    Private Sub OnDateEditEndDate_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles dateEditEndDate.EditValueChanged

      If m_SuppressUIEvents Then
        Return
      End If

      RecalculateDaysOfES()

    End Sub

    ''' <summary>
    ''' Handles change of KST1.
    ''' </summary>
    Private Sub OnLueKst1_EditValueChanged(sender As Object, e As EventArgs) Handles lueKst1.EditValueChanged

      If m_SuppressUIEvents Then
        Return
      End If

      LoadKst2DropDown()
    End Sub

    ''' <summary>
    ''' Handles change of customer or employee advisor.
    ''' </summary>
    Private Sub OnlueBeraterInEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdvisorKD.EditValueChanged, lueAdvisorMA.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim advisorKD = lueAdvisorKD.EditValue
			Dim advisorMA = lueAdvisorMA.EditValue

			LoadAdvisorKst12Data()

			If String.IsNullOrWhiteSpace(advisorKD) Then
				lblAdvisorCombinedText.Text = advisorMA
			ElseIf String.IsNullOrWhiteSpace(advisorMA) Then
				lblAdvisorCombinedText.Text = advisorKD
			ElseIf advisorKD = advisorMA Then
				lblAdvisorCombinedText.Text = advisorKD
			Else
				lblAdvisorCombinedText.Text = advisorMA + "/" + advisorKD
			End If

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
    ''' Preselects data.
    ''' </summary>
    Private Sub PreselectData()

      Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

      ' Einsatz als 
      txtEsAls.Text = If(hasPreselectionData AndAlso Not String.IsNullOrWhiteSpace(PreselectionData.ESAls),
                         PreselectionData.ESAls,
                         m_UCMediator.SelectedCandidateAndCustomerData.EmployeeData.Profession)

      ' Start date
      dateEditStartDate.EditValue = If(hasPreselectionData AndAlso PreselectionData.ESAb.HasValue,
                                       PreselectionData.ESAb,
                                       DateTime.Now.Date)

      Dim mdNr As Integer = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber

      ' Suva
      Try

        Dim suvaValue As String = m_Utility.ParseToString(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
                                                                     String.Format("{0}/essuvacode", FORM_XML_MAIN_KEY)), Nothing)

        lueSuva.EditValue = suvaValue

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

      ' Set default cost center
      SelectKst1(m_InitializationData.UserData.UserKST_1)
      lueKst2.EditValue = m_InitializationData.UserData.UserKST_2

      Dim selectadvisorkst As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNr),
                                                                               String.Format("{0}/selectadvisorkst", FORM_XML_MAIN_KEY)), False)


      Try
        ' Advisor MA
        If (hasPreselectionData AndAlso Not String.IsNullOrWhiteSpace(PreselectionData.BeraterMA)) Then
          SelectAdvisor(lueAdvisorMA, If(PreselectionData.BeraterMA Is Nothing, m_InitializationData.UserData.UserKST, PreselectionData.BeraterMA))
        ElseIf selectadvisorkst Then
          SelectAdvisor(lueAdvisorMA, m_InitializationData.UserData.UserKST)
        Else
          Dim selectedEmployee = m_UCMediator.SelectedCandidateAndCustomerData.EmployeeData
          SelectAdvisor(lueAdvisorMA, selectedEmployee.KST)
        End If
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())

      End Try

      Try
        ' Advisor KD
        If hasPreselectionData AndAlso Not String.IsNullOrWhiteSpace(PreselectionData.BeraterKD) Then
          SelectAdvisor(lueAdvisorKD, If(PreselectionData.BeraterKD Is Nothing, m_InitializationData.UserData.UserKST, PreselectionData.BeraterKD))
        ElseIf selectadvisorkst Then
          SelectAdvisor(lueAdvisorKD, m_InitializationData.UserData.UserKST)
        Else
          Dim selectedCustomer = m_UCMediator.SelectedCandidateAndCustomerData.CustomerData
          SelectAdvisor(lueAdvisorKD, selectedCustomer.KST)
        End If
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())

      End Try

      ' Unterzeichner 
      lueSubscriber.EditValue = String.Format("{0} {1}", m_InitializationData.UserData.UserFName, m_InitializationData.UserData.UserLName)

    End Sub

    ''' <summary>
    ''' Selects the Kst1.
    ''' </summary>
    ''' <param name="kst1">The kst1</param>
    Private Sub SelectKst1(ByVal kst1 As String)

      Dim suppressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = True

      lueKst1.EditValue = kst1
      LoadKst2DropDown()

      m_SuppressUIEvents = suppressUIEventState

    End Sub

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
				If m_ESEndebynull Is Nothing Then
					lblDays.Text = String.Empty
				Else
					lblDays.Text = m_Translate.GetSafeTranslationValue(m_ESEndebynull)
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

      Public ReadOnly Property SalutationLastNameFirstName
        Get
          Return String.Format("{0} {1} {2}", TranslatedSalutation, Lastname, Firstname)
        End Get
      End Property
    End Class



    ''' <summary>
    ''' Branch view data.
    ''' </summary>
    Class BranchViewData

      Public Property Branche As String
      Public Property TranslatedBrancheText As String
      Public Property IsAssignedToCustomer

    End Class

#End Region

    Private Sub OnTxtQulification_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtEsAls.ButtonClick

    End Sub

    Private Sub lblKST2_Click(sender As System.Object, e As System.EventArgs) Handles lblKST2.Click

    End Sub
  End Class

End Namespace
