Imports DevExpress.XtraEditors.Controls

Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.Infrastructure.ucListSelectPopup
Imports SP.DatabaseAccess.Employee.DataObjects.MediationMng
Imports System.Linq

Namespace UI

  ' Mediation data
  Public Class ucMediation

#Region "Private Consts"

    Private Const POPUP_DEFAULT_WIDTH As Integer = 300
    Private Const POPUP_DEFAULT_HEIGHT As Integer = 280

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' Communication popup data.
    ''' </summary>
    Private m_CommunicationPopupData As IEnumerable(Of EmployeeCommunicationData)

    ''' <summary>
    ''' Assessment popup data.
    ''' </summary>
    Private m_AssessmentPopupData As IEnumerable(Of AssessmentData)

    ''' <summary>
    ''' Communication popup column definitions.
    ''' </summary>
    Private m_CommunicationPopupColumns As New List(Of PopupColumDefintion)

    ''' <summary>
    ''' Assessment popup column definitions.
    ''' </summary>
    Private m_AssessmentPopupColumns As New List(Of PopupColumDefintion)


#End Region

#Region "Constructor"

    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Register popup row click handlers
      AddHandler ucPopupCommunicationData.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucPopupAssessmentData.RowClicked, AddressOf OnPopupRowClicked

      ' Register size changed handlers
      AddHandler ucPopupCommunicationData.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucPopupAssessmentData.PopupSizeChanged, AddressOf OnPopupSizeChanged

      AddHandler dateEditESAb.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler dateEditESEnd.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueEmploymentType.ButtonClick, AddressOf OnDropDown_ButtonClick
			'AddHandler lueTermsAndConditions.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueDrivingLicence1.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueDrivingLicence2.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueDrivingLicence3.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueVehicle.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueCarReserve.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve1.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve2.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve3.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve4.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve5.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueDeadline.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueWorkPensum.ButtonClick, AddressOf OnDropDown_ButtonClick

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

      ' Column defintions for popups
      m_CommunicationPopupColumns.Add(New PopupColumDefintion With {.Name = "TranslatedCommunicationText", .Translation = m_Translate.GetSafeTranslationValue("Kommunikation")})
      m_AssessmentPopupColumns.Add(New PopupColumDefintion With {.Name = "TranslatedAssessmentText", .Translation = m_Translate.GetSafeTranslationValue("Beurteilung")})

    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean

      Dim success As Boolean = True

      If (Not IsIntialControlDataLoaded) Then
        success = success AndAlso LoadDropDownData()
        IsIntialControlDataLoaded = True
      End If

      If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
        success = success AndAlso LoadEmployeeData(employeeNumber)
        m_EmployeeNumber = IIf(success, employeeNumber, 0)
      End If

      m_SuppressUIEvents = False

      Return success
    End Function

    ''' <summary>
    ''' Deactivates the control.
    ''' </summary>
    Public Overrides Sub Deactivate()
      HidePopups()
    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      HidePopups()

      m_EmployeeNumber = Nothing

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

      dateEditESAb.EditValue = Nothing
      dateEditESEnd.EditValue = Nothing
			'chkSendEmployeeToWeb.Checked = False

			txtEditAbsence.Text = String.Empty
      txtEditAbsence.Properties.MaxLength = 300

      txtEditNoWorkAs.Text = String.Empty
      txtEditNoWorkAs.Properties.MaxLength = 300

      txtEditTerminatinReason.Text = String.Empty
      txtEditTerminatinReason.Properties.MaxLength = 500

      txtOldSalaryPerYear.Text = String.Empty
      txtOldSalaryPerYear.Properties.MaxLength = 15

      txtOldSalaryPerYear.Text = String.Empty
      txtOldSalaryPerYear.Properties.MaxLength = 15

      txtDesiredSalaryPerMonth.Text = String.Empty
      txtDesiredSalaryPerMonth.Properties.MaxLength = 15

      txtDesiredSalaryPerHour.Text = String.Empty
      txtDesiredSalaryPerHour.Properties.MaxLength = 15

      '  Reset drop downs and lists

      ResetEmploymentTypeDropDown()
			'ResetTermsAndConditionsDropDown()
			ResetDrivingLicenceOneToThreeDropDown()
      ResetVehicleDropDown()
      ResetCarReserveDropDown()
      ResetReserveOneToFourDropDowns()
      ResetReserveFiveDropDown()
      ResetDeadlineDropDown()
      ResetWorkPensumropDown()

      lstEmploymentType.DataSource = Nothing
      lstCommunication.DataSource = Nothing
      lstAssessments.DataSource = Nothing

      m_SuppressUIEvents = suppressUIEventsState

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean

      Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

      Dim isValid As Boolean = True

      ' Add required validation logic here.

      Return isValid

    End Function

    ''' <summary>
    ''' Merges the employee master data.
    ''' </summary>
    ''' <param name="employeeMasterData">The employee master data object where the data gets filled into.</param>
    ''' <param name="forceMerge">Optional flag indicating if the merge should be forced altough no data has been loaded. </param>
    Public Overrides Sub MergeEmployeeMasterData(ByVal employeeMasterData As EmployeeMasterData, Optional forceMerge As Boolean = False)
      If ((IsEmployeeDataLoaded AndAlso
          m_EmployeeNumber = employeeMasterData.EmployeeNumber) Or forceMerge) Then

        ' Not emplyoee master data (table Mitarbeiter) to merge.
      End If
    End Sub

    ''' <summary>
    '''  Merges the employee contact other data (MASonstiges).
    ''' </summary>
    ''' <param name="employeeOtherData">The employee other data.</param>
    Public Overrides Sub MergeEmployeeOtherData(ByVal employeeOtherData As EmployeeOtherData)
      If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeOtherData.EmployeeNumber) Then

        employeeOtherData.DrivingLicence1 = lueDrivingLicence1.EditValue
        employeeOtherData.DrivingLicence2 = lueDrivingLicence2.EditValue
        employeeOtherData.DrivingLicence3 = lueDrivingLicence3.EditValue
        employeeOtherData.Vehicle = lueVehicle.EditValue
        employeeOtherData.AutoReserv = lueCarReserve.EditValue
      End If
    End Sub

    ''' <summary>
    '''  Merges the employee contact comm data.
    ''' </summary>
    ''' <param name="employeeContactCommData">The employee contact comm data.</param>
    Public Overrides Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
      If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeContactCommData.EmployeeNumber) Then

        employeeContactCommData.ESAb = dateEditESAb.EditValue
        employeeContactCommData.ESEnde = dateEditESEnd.EditValue
        employeeContactCommData.Absenzen = txtEditAbsence.Text
				employeeContactCommData.NoWorkAS = txtEditNoWorkAs.Text
				'employeeContactCommData.AGB_WOS = lueTermsAndConditions.EditValue
				'employeeContactCommData.WebExport = chkSendEmployeeToWeb.Checked

				employeeContactCommData.Res1 = lueReserve1.EditValue
				employeeContactCommData.Res2 = lueReserve2.EditValue
        employeeContactCommData.Res3 = lueReserve3.EditValue
        employeeContactCommData.Res4 = lueReserve4.EditValue
        employeeContactCommData.Res5 = lueReserve5.EditValue
        employeeContactCommData.KundFristen = lueDeadline.EditValue
        employeeContactCommData.KundGrund = txtEditTerminatinReason.EditValue
        employeeContactCommData.Arbeitspensum = lueWorkPensum.EditValue

        ' Salary year old
        If Not String.IsNullOrEmpty(txtOldSalaryPerYear.Text) Then
          employeeContactCommData.GehaltAlt = Decimal.Parse(txtOldSalaryPerYear.Text)
        Else
          employeeContactCommData.GehaltAlt = Nothing
        End If

        ' Salary year new
        If Not String.IsNullOrEmpty(txtNewSalaryPerYear.Text) Then
          employeeContactCommData.GehaltNeu = Decimal.Parse(txtNewSalaryPerYear.Text)
        Else
          employeeContactCommData.GehaltNeu = Nothing
        End If

        ' Desired salary per month
        If Not String.IsNullOrEmpty(txtDesiredSalaryPerMonth.Text) Then
          employeeContactCommData.GehaltPerMonth = Decimal.Parse(txtDesiredSalaryPerMonth.Text)
        Else
          employeeContactCommData.GehaltPerMonth = Nothing
        End If

        ' Desired salary per hour
        If Not String.IsNullOrEmpty(txtDesiredSalaryPerHour.Text) Then
          employeeContactCommData.GehaltPerStd = Decimal.Parse(txtDesiredSalaryPerHour.Text)
        Else
          employeeContactCommData.GehaltPerStd = Nothing
        End If

      End If
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()
      HidePopups()
    End Sub

#End Region

#Region "Privte Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      ' Group Verfügbarkeint
      Me.grpVerfuegbarkeit.Text = m_Translate.GetSafeTranslationValue(Me.grpVerfuegbarkeit.Text)
      Me.lblVerfuegbarkeit.Text = m_Translate.GetSafeTranslationValue(Me.lblVerfuegbarkeit.Text)
      Me.lblAbsenzen.Text = m_Translate.GetSafeTranslationValue(Me.lblAbsenzen.Text)
      Me.lblNichtEinsetzen.Text = m_Translate.GetSafeTranslationValue(Me.lblNichtEinsetzen.Text)
			'Me.chkSendEmployeeToWeb.Text = m_Translate.GetSafeTranslationValue(Me.chkSendEmployeeToWeb.Text)
			'Me.lblAGBFuerWOS.Text = m_Translate.GetSafeTranslationValue(Me.lblAGBFuerWOS.Text)

			' Group Anstellungsart
			Me.grpAnstellungsart.Text = m_Translate.GetSafeTranslationValue(Me.grpAnstellungsart.Text, True)

      ' Group Kommunikation
			Me.grpKommunikation.Text = m_Translate.GetSafeTranslationValue(Me.grpKommunikation.Text, True)

			' Group Eigenschaften
			Me.grpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.grpEigenschaften.Text, True)

      ' Group Führerschein
      Me.grpFuehrerschein.Text = m_Translate.GetSafeTranslationValue(Me.grpFuehrerschein.Text)
      Me.lblFuehrerschein1.Text = m_Translate.GetSafeTranslationValue(Me.lblFuehrerschein1.Text)
      Me.lblFuehrerschein2.Text = m_Translate.GetSafeTranslationValue(Me.lblFuehrerschein2.Text)
      Me.lblFuehrerschein3.Text = m_Translate.GetSafeTranslationValue(Me.lblFuehrerschein3.Text)
      Me.lblFahrzeug.Text = m_Translate.GetSafeTranslationValue(Me.lblFahrzeug.Text)
      Me.lblResAuto.Text = m_Translate.GetSafeTranslationValue(Me.lblResAuto.Text)

      ' Group Reservefelder
			Me.grpReservefelder.Text = m_Translate.GetSafeTranslationValue(Me.grpReservefelder.Text, True)
			Me.lblReserve1.Text = m_Translate.GetSafeTranslationValue(Me.lblReserve1.Text, True)
      Me.lblReserve2.Text = m_Translate.GetSafeTranslationValue(Me.lblReserve2.Text, True)
      Me.lblReserve3.Text = m_Translate.GetSafeTranslationValue(Me.lblReserve3.Text, True)
      Me.lblReserve4.Text = m_Translate.GetSafeTranslationValue(Me.lblReserve4.Text, True)
      Me.lblReserve5.Text = m_Translate.GetSafeTranslationValue(Me.lblReserve5.Text, True)

      ' Group Kündigung
      Me.grpKuendigung.Text = m_Translate.GetSafeTranslationValue(Me.grpKuendigung.Text)
      Me.lblFristen.Text = m_Translate.GetSafeTranslationValue(Me.lblFristen.Text)
      Me.lblKuendigungsGrund.Text = m_Translate.GetSafeTranslationValue(Me.lblKuendigungsGrund.Text)
      Me.lblArbeitspensum.Text = m_Translate.GetSafeTranslationValue(Me.lblArbeitspensum.Text)
      Me.lblGehaltJahrAlt.Text = m_Translate.GetSafeTranslationValue(Me.lblGehaltJahrAlt.Text)
      Me.lblGehaltJahrNeu.Text = m_Translate.GetSafeTranslationValue(Me.lblGehaltJahrNeu.Text)
      Me.lblWunschlohnProMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblWunschlohnProMonat.Text)
      Me.lblWunschlohnProStunde.Text = m_Translate.GetSafeTranslationValue(Me.lblWunschlohnProStunde.Text)

    End Sub

    ''' <summary>
    ''' Resets employment type drop down data.
    ''' </summary>
    Private Sub ResetEmploymentTypeDropDown()

      lueEmploymentType.Properties.ShowHeader = False
      lueEmploymentType.Properties.ShowFooter = False
      lueEmploymentType.Properties.DisplayMember = "TranslatedEmploymentType"
      lueEmploymentType.Properties.ValueMember = "Description"

      Dim columns = lueEmploymentType.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("TranslatedEmploymentType", 0))

      lueEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
      lueEmploymentType.Properties.AutoSearchColumnIndex = 0

      lueEmploymentType.Properties.NullText = String.Empty
      lueEmploymentType.EditValue = Nothing

    End Sub

		'''' <summary>
		'''' Resets the terms and conditions drop down.
		'''' </summary>
		'Private Sub ResetTermsAndConditionsDropDown()

		'  lueTermsAndConditions.Properties.ShowHeader = False
		'  lueTermsAndConditions.Properties.ShowFooter = False
		'  lueTermsAndConditions.Properties.DropDownRows = 10

		'  lueTermsAndConditions.Properties.DisplayMember = "TranslatedTermsAndConditions"
		'  lueTermsAndConditions.Properties.ValueMember = "Description"

		'  Dim columns = lueTermsAndConditions.Properties.Columns
		'  columns.Clear()
		'  columns.Add(New LookUpColumnInfo("TranslatedTermsAndConditions", 0))

		'  lueTermsAndConditions.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		'  lueTermsAndConditions.Properties.SearchMode = SearchMode.AutoComplete
		'  lueTermsAndConditions.Properties.AutoSearchColumnIndex = 0

		'  lueTermsAndConditions.Properties.NullText = String.Empty
		'  lueTermsAndConditions.EditValue = Nothing

		'End Sub

		''' <summary>
		''' Resets the driving licence drop down data.
		''' </summary>
		Private Sub ResetDrivingLicenceOneToThreeDropDown()

      Dim controls As New List(Of LookUpEdit)
      controls.Add(lueDrivingLicence1)
      controls.Add(lueDrivingLicence2)
      controls.Add(lueDrivingLicence3)

      For Each ctrl In controls

				ctrl.Properties.ShowHeader = True
        ctrl.Properties.ShowFooter = False
        ctrl.Properties.DropDownRows = 20
				ctrl.Properties.DisplayMember = "ValueToShow"
        ctrl.Properties.ValueMember = "RecValue"

        Dim columns = ctrl.Properties.Columns
        columns.Clear()
				columns.Add(New LookUpColumnInfo("RecValue", 20, String.Empty))
				columns.Add(New LookUpColumnInfo("TranslatedDrivingLicenceText", 0, m_Translate.GetSafeTranslationValue("Führerschein")))

        ctrl.Properties.BestFitMode = BestFitMode.BestFitResizePopup
        ctrl.Properties.SearchMode = SearchMode.AutoComplete
        ctrl.Properties.AutoSearchColumnIndex = 0

        ctrl.Properties.NullText = String.Empty
        ctrl.EditValue = Nothing

      Next

    End Sub

    ''' <summary>
    ''' Resets reserve vehicle drop down data.
    ''' </summary>
    Private Sub ResetVehicleDropDown()

      lueVehicle.Properties.ShowHeader = False
      lueVehicle.Properties.ShowFooter = False
      lueVehicle.Properties.DropDownRows = 20
      lueVehicle.Properties.DisplayMember = "TranslatedVehicleText"
      lueVehicle.Properties.ValueMember = "RecValue"

      Dim columns = lueVehicle.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("RecValue", 0, String.Empty))
      columns.Add(New LookUpColumnInfo("TranslatedVehicleText", 0, m_Translate.GetSafeTranslationValue("TranslatedVehicleText")))

      lueVehicle.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueVehicle.Properties.SearchMode = SearchMode.AutoComplete
      lueVehicle.Properties.AutoSearchColumnIndex = 0

      lueVehicle.Properties.NullText = String.Empty
      lueVehicle.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Resets the car reserve drop down data.
    ''' </summary>
    Private Sub ResetCarReserveDropDown()

      lueCarReserve.Properties.ShowHeader = False
      lueCarReserve.Properties.ShowFooter = False
      lueCarReserve.Properties.DropDownRows = 20

      lueCarReserve.Properties.DisplayMember = "TranslatedReserveText"
      lueCarReserve.Properties.ValueMember = "Description"

      Dim columns = lueCarReserve.Properties.Columns
      columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedReserveText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

      lueCarReserve.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueCarReserve.Properties.SearchMode = SearchMode.AutoComplete
      lueCarReserve.Properties.AutoSearchColumnIndex = 0

      lueCarReserve.Properties.NullText = String.Empty
      lueCarReserve.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Resets the reserve 1-4 drop downs
    ''' </summary>
    Private Sub ResetReserveOneToFourDropDowns()

      Dim controls As New List(Of LookUpEdit)
      controls.Add(lueReserve1)
      controls.Add(lueReserve2)
      controls.Add(lueReserve3)
      controls.Add(lueReserve4)

      For Each ctrl In controls
        ctrl.Properties.SearchMode = SearchMode.OnlyInPopup
        ctrl.Properties.TextEditStyle = TextEditStyles.Standard
        ctrl.Properties.ShowHeader = False
        ctrl.Properties.ShowFooter = False
        ctrl.Properties.DropDownRows = 20
        ctrl.Properties.DisplayMember = "TranslatedReserveText"
        ctrl.Properties.ValueMember = "Description"

        Dim columns = ctrl.Properties.Columns
        columns.Clear()
				columns.Add(New LookUpColumnInfo("TranslatedReserveText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

        ctrl.Properties.BestFitMode = BestFitMode.BestFitResizePopup
        ctrl.Properties.SearchMode = SearchMode.AutoComplete
        ctrl.Properties.AutoSearchColumnIndex = 0

        ctrl.Properties.NullText = String.Empty
        ctrl.EditValue = Nothing
      Next

    End Sub

    ''' <summary>
    ''' Resets reserve five drop down data.
    ''' </summary>
    Private Sub ResetReserveFiveDropDown()

      ' Reserve 5 allows to enter data, so set some additonal properties
      lueReserve5.Properties.SearchMode = SearchMode.OnlyInPopup
      lueReserve5.Properties.TextEditStyle = TextEditStyles.Standard

      lueReserve5.Properties.ShowHeader = False
      lueReserve5.Properties.ShowFooter = False
      lueReserve5.Properties.DropDownRows = 20
      lueReserve5.Properties.DisplayMember = "Reserve5"
      lueReserve5.Properties.ValueMember = "Reserve5"

      Dim columns = lueReserve5.Properties.Columns
      columns.Clear()
			columns.Add(New LookUpColumnInfo("Reserve5", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

      lueReserve5.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueReserve5.Properties.SearchMode = SearchMode.AutoComplete
      lueReserve5.Properties.AutoSearchColumnIndex = 0

      lueReserve5.Properties.NullText = String.Empty
      lueReserve5.EditValue = Nothing
    End Sub

    ''' <summary>
    ''' Resets the deadline drop down data.
    ''' </summary>
    Private Sub ResetDeadlineDropDown()

      lueDeadline.Properties.ShowHeader = False
      lueDeadline.Properties.ShowFooter = False
      lueDeadline.Properties.DropDownRows = 20

      lueDeadline.Properties.DisplayMember = "TranslatedDeadlineText"
      lueDeadline.Properties.ValueMember = "GetField"

      Dim columns = lueDeadline.Properties.Columns
      columns.Clear()
			columns.Add(New LookUpColumnInfo("TranslatedDeadlineText", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

      lueDeadline.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueDeadline.Properties.SearchMode = SearchMode.AutoComplete
      lueDeadline.Properties.AutoSearchColumnIndex = 0

      lueDeadline.Properties.NullText = String.Empty
      lueDeadline.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Resets the work pensum drop down data.
    ''' </summary>
    Private Sub ResetWorkPensumropDown()

      lueWorkPensum.Properties.ShowHeader = False
      lueWorkPensum.Properties.ShowFooter = False
      lueWorkPensum.Properties.DropDownRows = 20

      lueWorkPensum.Properties.DisplayMember = "GetField"
      lueWorkPensum.Properties.ValueMember = "GetField"

      Dim columns = lueWorkPensum.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("GetField", 0, String.Empty))

      lueWorkPensum.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueWorkPensum.Properties.SearchMode = SearchMode.AutoComplete
      lueWorkPensum.Properties.AutoSearchColumnIndex = 0

      lueWorkPensum.Properties.NullText = String.Empty
      lueWorkPensum.EditValue = Nothing

    End Sub

    ''' <summary>
    '''  Loads responsible person data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadEmployeeOtherData(employeeNumber)
      success = success AndAlso LoadEmployeeContactCommData(employeeNumber)
      success = success AndAlso LoadEmployeeAssignedEmploymentTypeData(employeeNumber)
      success = success AndAlso LoadEmployeeAssignedCommunicationData(employeeNumber)
      success = success AndAlso LoadEmployeeAssignedAssessmentData(employeeNumber)

      Return success
    End Function

    ''' <summary>
    ''' Loads employee assigned employment type data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedEmploymentTypeData(ByVal employeeNumber As Integer) As Boolean

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

      Dim assignedEmploymentTypeData = m_EmployeeDataAccess.LoadEmployeeAssignedEmploymentTypeData(employeeNumber)

      If (assignedEmploymentTypeData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugewiesene Anstellungsarten konnten nicht geladen werden."))
        Return False
      End If

      lstEmploymentType.DisplayMember = "TranslatedEmploymentType"
      lstEmploymentType.ValueMember = "Description"
      lstEmploymentType.DataSource = assignedEmploymentTypeData

      m_SuppressUIEvents = suppressUIEventsState

      Return True

    End Function

    ''' <summary>
    ''' Loads employee assigned communication data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedCommunicationData(ByVal employeeNumber As Integer) As Boolean

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

      Dim assignedEmploymentTypeData = m_EmployeeDataAccess.LoadEmployeeAssignedCommunicationData(employeeNumber)

      If (assignedEmploymentTypeData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugewiesene Kommunikationsarten konnten nicht geladen werden."))
        Return False
      End If

      lstCommunication.DisplayMember = "TranslatedCommunicationText"
      lstCommunication.ValueMember = "Description"
      lstCommunication.DataSource = assignedEmploymentTypeData

      m_SuppressUIEvents = suppressUIEventsState

      Return True

    End Function

    ''' <summary>
    ''' Loads employee assigned assessment data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedAssessmentData(ByVal employeeNumber As Integer) As Boolean

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

      Dim assignedAssessmentData = m_EmployeeDataAccess.LoadEmployeeAssignedAssessmentData(employeeNumber)

      If (assignedAssessmentData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugewiesene Beurteilungen konnten nicht geladen werden."))
        Return False
      End If

      lstAssessments.DisplayMember = "TranslatedDescription"
      lstAssessments.ValueMember = "Description"
      lstAssessments.DataSource = assignedAssessmentData

      m_SuppressUIEvents = suppressUIEventsState

      Return True

    End Function

    ''' <summary>
    ''' Loads the employee other data (MASonstiges).
    ''' </summary>
    ''' <param name="enmployeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeOtherData(ByVal enmployeeNumber As Integer) As Boolean

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

      Dim employeeOtherData As EmployeeOtherData = m_EmployeeDataAccess.LoadEmployeeOtherData(enmployeeNumber)

      If (employeeOtherData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sonstige Mitarbeiterdaten konnten nicht geladen werden."))
        Return False
      End If

      lueDrivingLicence1.EditValue = employeeOtherData.DrivingLicence1
      lueDrivingLicence2.EditValue = employeeOtherData.DrivingLicence2
      lueDrivingLicence3.EditValue = employeeOtherData.DrivingLicence3
      lueVehicle.EditValue = employeeOtherData.Vehicle
      lueCarReserve.EditValue = employeeOtherData.AutoReserv

      m_SuppressUIEvents = suppressUIEventsState

      Return True
    End Function

		''' <summary>
		''' Loads the employee contact comm data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeContactCommData(ByVal employeeNumber As Integer) As Boolean

			Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDataAccess.LoadEmployeeContactCommData(employeeNumber)

			If (employeeContactCommData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
        Return False
      End If

      dateEditESAb.EditValue = employeeContactCommData.ESAb
      dateEditESEnd.EditValue = employeeContactCommData.ESEnde
      txtEditAbsence.Text = employeeContactCommData.Absenzen
      txtEditNoWorkAs.Text = employeeContactCommData.NoWorkAS
			'lueTermsAndConditions.EditValue = employeeContactCommData.AGB_WOS
			'chkSendEmployeeToWeb.Checked = employeeContactCommData.WebExport.HasValue AndAlso employeeContactCommData.WebExport = True

			SetReserveValue1To4(lueReserve1, employeeContactCommData.Res1)
      SetReserveValue1To4(lueReserve2, employeeContactCommData.Res2)
      SetReserveValue1To4(lueReserve3, employeeContactCommData.Res3)
      SetReserveValue1To4(lueReserve4, employeeContactCommData.Res4)
      lueReserve5.EditValue = employeeContactCommData.Res5
      lueDeadline.EditValue = employeeContactCommData.KundFristen
      txtEditTerminatinReason.Text = employeeContactCommData.KundGrund
      lueWorkPensum.EditValue = employeeContactCommData.Arbeitspensum

      txtOldSalaryPerYear.Text = IIf(employeeContactCommData.GehaltAlt Is Nothing, String.Empty, employeeContactCommData.GehaltAlt)
      txtNewSalaryPerYear.Text = IIf(employeeContactCommData.GehaltNeu Is Nothing, String.Empty, employeeContactCommData.GehaltNeu)
      txtDesiredSalaryPerMonth.Text = IIf(employeeContactCommData.GehaltPerMonth Is Nothing, String.Empty, employeeContactCommData.GehaltPerMonth)
      txtDesiredSalaryPerHour.Text = IIf(employeeContactCommData.GehaltPerStd Is Nothing, String.Empty, employeeContactCommData.GehaltPerStd)

      m_SuppressUIEvents = suppressUIEventsState

      Return True
    End Function

    ''' <summary>
    ''' Sets the reserve 1 to 4 values.
    ''' </summary>
    ''' <param name="ctrl">The reserve lookup edit control.</param>
    ''' <param name="value">The value.</param>
    Private Sub SetReserveValue1To4(ByVal ctrl As LookUpEdit, ByVal value As String)

      If Not String.IsNullOrWhiteSpace(value) And Not ctrl.Properties.DataSource Is Nothing Then
        Dim contactReserveList = CType(ctrl.Properties.DataSource, List(Of ContactReserveData))

        If Not contactReserveList.Any(Function(data) data.Description = value) Then
          Dim newReserveData As New ContactReserveData With {.Description = value, .TranslatedReserveText = value}
          contactReserveList.Add(newReserveData)
        End If

      End If

      ctrl.EditValue = value

    End Sub

    ''' <summary>
    ''' Loads the drop down data.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadDropDownData() As Boolean
      Dim success As Boolean = True

      success = success AndAlso LoadEmployeeEmploymentTypeDropDownData()
			'success = success AndAlso LoadTermsAndConditionsDropDownData()
			success = success AndAlso LoadDrivingLicenceOneToThreeDropDownData()
      success = success AndAlso LoadVehicleDropDownData()
      success = success AndAlso LoadCarDropDownData()
      success = success AndAlso LoadReserveOneToFourDropDownData()
      success = success AndAlso LoadReserveFiveDropDownData()
      success = success AndAlso LoadDeadlineDropDownData()
      success = success AndAlso LoadWorkPensumDropDownData()

      Return success
    End Function

    ''' <summary>
    ''' Loads employee employment type drop down data.
    ''' </summary>
    ''' <returns>Boolean falg indicating success.</returns>
    Private Function LoadEmployeeEmploymentTypeDropDownData() As Boolean
      Dim employmentTypeData = m_EmployeeDataAccess.LoadEmployeeEmployementTypeData

      If (employmentTypeData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog("Anstellungsartenauswahl konnte nicht geladen werden.")
      End If

      lueEmploymentType.Properties.DataSource = employmentTypeData
      lueEmploymentType.Properties.ForceInitialize()

      Return Not employmentTypeData Is Nothing
    End Function

		'''' <summary>
		'''' Loads the terms and conditions drop down data.
		'''' </summary>
		'Private Function LoadTermsAndConditionsDropDownData()
		'  Dim termsAndConditionsData = m_CommonDatabaseAccess.LoadTermsAndConditionsData()

		'  If (termsAndConditionsData Is Nothing) Then
		'    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("AGB-Daten konnten nicht geladen werden."))
		'  End If

		'  lueTermsAndConditions.Properties.DataSource = termsAndConditionsData
		'  lueTermsAndConditions.Properties.ForceInitialize()

		'  Return Not termsAndConditionsData Is Nothing
		'End Function

		''' <summary>
		''' Loads the driving licence drop down data (1-3).
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadDrivingLicenceOneToThreeDropDownData() As Boolean
      Dim drivingLicenceData = m_EmployeeDataAccess.LoadDrivingLicenceData

      If (drivingLicenceData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog("Führerscheindaten konnten nicht geladen werden.")
      End If

      lueDrivingLicence1.Properties.DataSource = drivingLicenceData
      lueDrivingLicence1.Properties.ForceInitialize()

      lueDrivingLicence2.Properties.DataSource = drivingLicenceData
      lueDrivingLicence2.Properties.ForceInitialize()

      lueDrivingLicence3.Properties.DataSource = drivingLicenceData
      lueDrivingLicence3.Properties.ForceInitialize()

      Return Not drivingLicenceData Is Nothing
    End Function

    ''' <summary>
    ''' Loads vehicle drop down data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadVehicleDropDownData() As Boolean

      Dim vehicleData = m_EmployeeDataAccess.LoadVehicleData

      If (vehicleData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog("Fahrzeugdaten konnten nicht geladen werden.")
      End If

      lueVehicle.Properties.DataSource = vehicleData
      lueVehicle.Properties.ForceInitialize()

      Return Not vehicleData Is Nothing

    End Function

    ''' <summary>
    ''' Loads car drop down data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadCarDropDownData() As Boolean

      Dim carReserveData = m_EmployeeDataAccess.LoadCarReserveData

      If (carReserveData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog("Autoreservedaten konnten nicht geladen werden.")
      End If

      lueCarReserve.Properties.DataSource = carReserveData
      lueCarReserve.Properties.ForceInitialize()

      Return Not carReserveData Is Nothing

    End Function

    ''' <summary>
    ''' Loads the reserve one to four drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadReserveOneToFourDropDownData() As Boolean
      Dim reserve1Data = m_EmployeeDataAccess.LoadContactReserveData(EmployeeContactReserveType.Reserve1)
      Dim reserve2Data = m_EmployeeDataAccess.LoadContactReserveData(EmployeeContactReserveType.Reserve2)
      Dim reserve3Data = m_EmployeeDataAccess.LoadContactReserveData(EmployeeContactReserveType.Reserve3)
      Dim reserve4Data = m_EmployeeDataAccess.LoadContactReserveData(EmployeeContactReserveType.Reserve4)

      Dim success As Boolean = True
      If (reserve1Data Is Nothing Or
          reserve2Data Is Nothing Or
          reserve3Data Is Nothing Or
          reserve4Data Is Nothing) Then
        success = False
        m_UtilityUI.ShowErrorDialog("Reservedaten (1-4) konnten nicht vollständig geladen werden.")
      End If

      lueReserve1.Properties.DataSource = reserve1Data
      lueReserve1.Properties.ForceInitialize()

      lueReserve2.Properties.DataSource = reserve2Data
      lueReserve2.Properties.ForceInitialize()

      lueReserve3.Properties.DataSource = reserve3Data
      lueReserve3.Properties.ForceInitialize()

			lueReserve4.Properties.DataSource = reserve4Data
			lueReserve4.Properties.ForceInitialize()

			Return success
		End Function

		''' <summary>
		''' Loads the reserve five drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadReserveFiveDropDownData() As Boolean
			Dim reserve5Data = m_EmployeeDataAccess.LoadEmployeeContactCommReserve5Data

			If (reserve5Data Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Reservedaten (5) konnten nicht geladen werden.")
			End If

			lueReserve5.Properties.DataSource = reserve5Data
			lueReserve5.Properties.ForceInitialize()

			Return Not reserve5Data Is Nothing
		End Function

		''' <summary>
		''' Loads deadline drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadDeadlineDropDownData() As Boolean
			Dim deadlineData = m_EmployeeDataAccess.LoadDeadLineData

			If (deadlineData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Fristauswahldaten konnten nicht geladen werden.")
			End If

			lueDeadline.Properties.DataSource = deadlineData
			lueDeadline.Properties.ForceInitialize()

			Return Not deadlineData Is Nothing
		End Function


		''' <summary>
		''' Loads work pensum drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadWorkPensumDropDownData() As Boolean
			Dim workPensumData = m_EmployeeDataAccess.LoadWorkPensumData

			If (workPensumData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Arbeitspensumauswahldaten konnten nicht geladen werden.")
			End If

			lueWorkPensum.Properties.DataSource = workPensumData
			lueWorkPensum.Properties.ForceInitialize()

			Return Not workPensumData Is Nothing
		End Function

		''' <summary>
		''' Loads communication popup data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCommunicationPopupData() As Boolean

			m_CommunicationPopupData = m_EmployeeDataAccess.LoadEmployeeCommunicationData()

			If (m_CommunicationPopupData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kommunikationsauswahldaten konnten nicht geladen werden."))
				Return False
			End If

			Return True
		End Function

		''' <summary>
		''' Loads assessment popup data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssessmentPopupData() As Boolean

			m_AssessmentPopupData = m_EmployeeDataAccess.LoadAssessmentData()

			If (m_AssessmentPopupData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beurteilungsauswahldaten konnten nicht geladen werden."))
				Return False
			End If

			Return True
		End Function

		''' <summary>
		''' Handles click on button add communication data.
		''' </summary>
		Private Sub OnBtnAddCommunicationData_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCommunicationData.Click

			HidePopups()

			Dim position = Cursor.Position
			If m_CommunicationPopupData Is Nothing Then
				LoadCommunicationPopupData()
			End If

			If Not m_CommunicationPopupData Is Nothing Then

				Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_MEDIATION_COMMUNICATION_SIZE)

				' Show popup
				ucPopupCommunicationData.InitPopup(m_CommunicationPopupData, m_CommunicationPopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
				ucPopupCommunicationData.ShowPopup(position, popupSize)
			End If

		End Sub

		''' <summary>
		''' Handles click on button add assesment data.
		''' </summary>
		Private Sub OnBtnAddAssessmentsDatanData_Click(sender As System.Object, e As System.EventArgs) Handles btnAddAssessmentsData.Click

			HidePopups()

			Dim position = Cursor.Position
			If m_AssessmentPopupData Is Nothing Then
				LoadAssessmentPopupData()
			End If

			If Not m_AssessmentPopupData Is Nothing Then

				Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_MEDIATION_ASSESSMENT_SIZE)

				' Show popup
				ucPopupAssessmentData.InitPopup(m_AssessmentPopupData, m_AssessmentPopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
				ucPopupAssessmentData.ShowPopup(position, popupSize)
			End If

		End Sub

		''' <summary>
		''' Handles employment type value change.
		''' </summary>
		Private Sub OnLueEmploymentType_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueEmploymentType.EditValueChanged

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			Dim success = True

			Dim selectedEmploymentTypeData As EmployeeEmployementType = TryCast(lueEmploymentType.GetSelectedDataRow(), EmployeeEmployementType)

			If (Not selectedEmploymentTypeData Is Nothing) Then

				' Load the already assigned employment types.
				Dim employeeAssignedEmploymentTypes = m_EmployeeDataAccess.LoadEmployeeAssignedEmploymentTypeData(m_EmployeeNumber)

				' Check if the employment type is already assigned.
				If (employeeAssignedEmploymentTypes Is Nothing) Then
					' Data could not be loaded
					success = False
				ElseIf Not employeeAssignedEmploymentTypes.Any(Function(data) data.Description.ToLower().Trim() = selectedEmploymentTypeData.Description.ToLower().Trim()) Then

					' Add to database
					Dim employmentTypeAssignementToInsert = New EmployeeAssignedEmploymentTypeData With {.EmployeeNumber = m_EmployeeNumber, .Description = selectedEmploymentTypeData.Description}

					success = m_EmployeeDataAccess.AddEmployeeEmploymentTypeAssignment(employmentTypeAssignementToInsert)
				Else
					' Already assinged -> do nothing
				End If

			Else
				success = False
			End If

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog("Anstellungsart konnte nicht hinzugefügt werden.")
			End If

			' Reload assigned employment types.
			LoadEmployeeAssignedEmploymentTypeData(m_EmployeeNumber)

		End Sub

		''' <summary>
		''' Handles keydown event on employee employment type list.
		''' </summary>
		Private Sub OnLstEmploymentType_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstEmploymentType.KeyDown

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim employmentTypeData As EmployeeAssignedEmploymentTypeData = TryCast(lstEmploymentType.SelectedItem, EmployeeAssignedEmploymentTypeData)

				If (Not employmentTypeData Is Nothing) Then

					If Not m_EmployeeDataAccess.DeleteEmployeeEmploymentTypeAssignment(employmentTypeData.ID) Then
						m_UtilityUI.ShowErrorDialog("Anstellungsart konnte nicht gelöscht werden.")
					End If

					LoadEmployeeAssignedEmploymentTypeData(m_EmployeeNumber)

				End If

			End If

		End Sub

		''' <summary>
		''' Handles keydown event on employee communication.
		''' </summary>
		Private Sub OnLstCommunication_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstCommunication.KeyDown

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim communicationData As EmployeeAssignedCommunicationData = TryCast(lstCommunication.SelectedItem, EmployeeAssignedCommunicationData)

				If (Not communicationData Is Nothing) Then

					If Not m_EmployeeDataAccess.DeleteEmployeeCommunicationAssignment(communicationData.ID) Then
						m_UtilityUI.ShowErrorDialog("Kommunikationsart konnte nicht gelöscht werden.")
					End If

					LoadEmployeeAssignedCommunicationData(m_EmployeeNumber)

				End If

			End If

		End Sub

		''' <summary>
		''' Handles keydown event on employee assessment.
		''' </summary>
		Private Sub OnLstAssessment_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstAssessments.KeyDown

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim assessmentData As EmployeeAssignedAssessmentData = TryCast(lstAssessments.SelectedItem, EmployeeAssignedAssessmentData)

				If (Not assessmentData Is Nothing) Then

					If Not m_EmployeeDataAccess.DeleteEmployeeAssessmentAssignment(assessmentData.ID) Then
						m_UtilityUI.ShowErrorDialog("Beurteilung konnte nicht gelöscht werden.")
					End If

					LoadEmployeeAssignedAssessmentData(m_EmployeeNumber)

				End If

			End If

		End Sub

		''' <summary>
		''' Handles new value event on reserve 1 to 4 lookup edit.
		''' </summary>
    Private Sub OnLueReserve1To4_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueReserve1.ProcessNewValue,
                                                                                                                                                 lueReserve2.ProcessNewValue,
                                                                                                                                                 lueReserve3.ProcessNewValue,
                                                                                                                                                 lueReserve4.ProcessNewValue
      Dim ctrl As LookUpEdit = sender

      If Not ctrl.Properties.DataSource Is Nothing AndAlso
        Not String.IsNullOrEmpty(e.DisplayValue.ToString()) Then

        Dim listOfReserveData = CType(ctrl.Properties.DataSource, List(Of ContactReserveData))

        Dim newReserveData As New ContactReserveData With {.Description = e.DisplayValue.ToString(), .TranslatedReserveText = e.DisplayValue.ToString()}
        listOfReserveData.Add(newReserveData)

        e.Handled = True
      End If
    End Sub

    ''' <summary>
    ''' Handles new value event on reserve 5 lookup edit.
    ''' </summary>
    Private Sub OnLueReserve5_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueReserve5.ProcessNewValue

      If Not lueReserve5.Properties.DataSource Is Nothing AndAlso
        Not String.IsNullOrEmpty(e.DisplayValue.ToString()) Then

        Dim listOfEMployeeConact5Reserve = CType(lueReserve5.Properties.DataSource, List(Of ContactReserve5Data))

        Dim newReserve5Value As New ContactReserve5Data With {.Reserve5 = e.DisplayValue.ToString()}
        listOfEMployeeConact5Reserve.Add(newReserve5Value)

        e.Handled = True
      End If
    End Sub

    ''' <summary>
    ''' Handles click on a row on one of the popups.
    ''' </summary>
    Private Sub OnPopupRowClicked(ByVal sender As Object, ByVal clickedObject As Object)

      Dim success As Boolean = True

      If Object.ReferenceEquals(sender, ucPopupCommunicationData) AndAlso
          TypeOf clickedObject Is EmployeeCommunicationData Then
        success = AssignCommuncationDataToEmployee(CType(clickedObject, EmployeeCommunicationData))
      ElseIf Object.ReferenceEquals(sender, ucPopupAssessmentData) AndAlso
          TypeOf clickedObject Is AssessmentData Then
        success = AssignssesmentDataToEmployee(CType(clickedObject, AssessmentData))
      End If

      HidePopups()

      If Not success Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuordnung konnte nicht duchgeführt werden."))
      End If

    End Sub

    ''' <summary>
    ''' Assign communication data to an employee.
    ''' </summary>
    ''' <param name="communicationDataToAdd">The communication data to add.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignCommuncationDataToEmployee(ByVal communicationDataToAdd As EmployeeCommunicationData) As Boolean

      Dim succcess = True

      ' Load assigned communication data.
      Dim assignedCommunicationData = m_EmployeeDataAccess.LoadEmployeeAssignedCommunicationData(m_EmployeeNumber)

      If Not assignedCommunicationData Is Nothing Then

        ' Check if the new communication data is not already assigned
        If Not assignedCommunicationData.Any(Function(data) data.Description.ToLower().Trim() = communicationDataToAdd.Description.ToLower().Trim()) Then

          ' Add to database.
          Dim communicationDataToAssign = New EmployeeAssignedCommunicationData With {.EmployeeNumber = m_EmployeeNumber,
                                                                                      .Description = communicationDataToAdd.Description}
          succcess = m_EmployeeDataAccess.AddEmployeeCommunicationAssignment(communicationDataToAssign)
        End If
      Else
        succcess = False
      End If

      LoadEmployeeAssignedCommunicationData(m_EmployeeNumber)

      Return succcess

    End Function

    ''' <summary>
    ''' Assign assessment data to an employee.
    ''' </summary>
    ''' <param name="assessmentDataToAdd">The assessment data to add.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignssesmentDataToEmployee(ByVal assessmentDataToAdd As AssessmentData) As Boolean

      Dim succcess = True

      ' Load assigned assessment data.
      Dim assignedAssessmentData = m_EmployeeDataAccess.LoadEmployeeAssignedAssessmentData(m_EmployeeNumber)

      If Not assignedAssessmentData Is Nothing Then

        ' Check if the new assessment data is not already assigned
        If Not assignedAssessmentData.Any(Function(data) data.Description.ToLower().Trim() = assessmentDataToAdd.Description.ToLower().Trim()) Then

          ' Add to database.
          Dim assessmentDataToAssign = New EmployeeAssignedAssessmentData With {.EmployeeNumber = m_EmployeeNumber,
                                                                                  .Description = assessmentDataToAdd.Description}
          succcess = m_EmployeeDataAccess.AddEmployeeAssessmentAssignment(assessmentDataToAssign)
        End If
      Else
        succcess = False
      End If

      LoadEmployeeAssignedAssessmentData(m_EmployeeNumber)

      Return succcess

    End Function

    ''' <summary>
    ''' Handles drop down button clicks.
    ''' </summary>
    Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

      Const ID_OF_DELETE_BUTTON As Int32 = 1

      ' If delete button has been clicked reset the drop down.
      If e.Button.Index = ID_OF_DELETE_BUTTON Then

        If TypeOf sender Is LookUpEdit Then
          Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
          lookupEdit.EditValue = Nothing
        ElseIf TypeOf sender Is DateEdit Then
          Dim dateEdit As DateEdit = CType(sender, DateEdit)
          dateEdit.EditValue = Nothing
        End If
      End If
    End Sub

    ''' <summary>
    ''' Hides all popups.
    ''' </summary>
    Private Sub HidePopups()
      ucPopupCommunicationData.HidePopup()
      ucPopupAssessmentData.HidePopup()
    End Sub

    ''' <summary>
    ''' Reads a popup size setting.
    ''' </summary>
    ''' <param name="settingKey">The settings key.</param>
    ''' <returns>The size setting.</returns>
    Public Function ReadPopupSizeSetting(ByRef settingKey As String) As Size

      ' Load width/height setting
      Dim popupSizeSetting As String = String.Empty
      Dim popupSize As Size
      popupSize.Width = POPUP_DEFAULT_WIDTH
      popupSize.Height = POPUP_DEFAULT_HEIGHT

      Try
        popupSizeSetting = m_SettingsManager.ReadString(settingKey)

        If Not String.IsNullOrEmpty(popupSizeSetting) Then
          Dim arrSize As String() = popupSizeSetting.Split(CChar(";"))
          popupSize.Width = arrSize(0)
          popupSize.Height = arrSize(1)
        End If
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

      Return popupSize
    End Function

    ''' <summary>
    ''' Handles size changs of popups.
    ''' </summary>
    Private Sub OnPopupSizeChanged(ByVal sender As Object, ByVal newWidth As Integer, ByVal newHeight As Integer)
      Dim setting As String = String.Format("{0};{1}", newWidth, newHeight)
      Dim settingKey As String = String.Empty

      If Object.ReferenceEquals(sender, ucPopupCommunicationData) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_MEDIATION_COMMUNICATION_SIZE
      ElseIf Object.ReferenceEquals(sender, ucPopupAssessmentData) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_MEDIATION_ASSESSMENT_SIZE
      End If

      Try
        If Not String.IsNullOrEmpty(settingKey) Then
          m_SettingsManager.WriteString(settingKey, setting)
          m_SettingsManager.SaveSettings()
        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

    End Sub

#End Region

  End Class

End Namespace
