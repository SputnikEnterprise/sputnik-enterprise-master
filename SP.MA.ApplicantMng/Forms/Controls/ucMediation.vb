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

      ' Register size changed handlers
			AddHandler lueDrivingLicence1.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueDrivingLicence2.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueDrivingLicence3.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueVehicle.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueCarReserve.ButtonClick, AddressOf OnDropDown_ButtonClick
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
		End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

			m_EmployeeNumber = Nothing

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

			txtEditTerminatinReason.Text = String.Empty
      txtEditTerminatinReason.Properties.MaxLength = 500


      '  Reset drop downs and lists

			ResetDrivingLicenceOneToThreeDropDown()
      ResetVehicleDropDown()
      ResetCarReserveDropDown()
			ResetDeadlineDropDown()
      ResetWorkPensumropDown()


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

				employeeContactCommData.KundFristen = lueDeadline.EditValue
        employeeContactCommData.KundGrund = txtEditTerminatinReason.EditValue
        employeeContactCommData.Arbeitspensum = lueWorkPensum.EditValue

        ' Salary year old

      End If
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()

		End Sub

#End Region

#Region "Privte Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()


      ' Group Führerschein
      Me.grpFuehrerschein.Text = m_Translate.GetSafeTranslationValue(Me.grpFuehrerschein.Text)
      Me.lblFuehrerschein1.Text = m_Translate.GetSafeTranslationValue(Me.lblFuehrerschein1.Text)
      Me.lblFuehrerschein2.Text = m_Translate.GetSafeTranslationValue(Me.lblFuehrerschein2.Text)
      Me.lblFuehrerschein3.Text = m_Translate.GetSafeTranslationValue(Me.lblFuehrerschein3.Text)
      Me.lblFahrzeug.Text = m_Translate.GetSafeTranslationValue(Me.lblFahrzeug.Text)
      Me.lblResAuto.Text = m_Translate.GetSafeTranslationValue(Me.lblResAuto.Text)


      ' Group Kündigung
      Me.grpKuendigung.Text = m_Translate.GetSafeTranslationValue(Me.grpKuendigung.Text)
      Me.lblFristen.Text = m_Translate.GetSafeTranslationValue(Me.lblFristen.Text)
      Me.lblKuendigungsGrund.Text = m_Translate.GetSafeTranslationValue(Me.lblKuendigungsGrund.Text)
      Me.lblArbeitspensum.Text = m_Translate.GetSafeTranslationValue(Me.lblArbeitspensum.Text)

    End Sub

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

      Return success
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
    ''' <param name="enmployeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeContactCommData(ByVal enmployeeNumber As Integer) As Boolean

      Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
      m_SuppressUIEvents = True

      Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDataAccess.LoadEmployeeContactCommData(enmployeeNumber)

      If (employeeContactCommData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiter-Kontaktdaten konnten nicht geladen werden."))
        Return False
      End If

			lueDeadline.EditValue = employeeContactCommData.KundFristen
      txtEditTerminatinReason.Text = employeeContactCommData.KundGrund
      lueWorkPensum.EditValue = employeeContactCommData.Arbeitspensum


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

			success = success AndAlso LoadDrivingLicenceOneToThreeDropDownData()
      success = success AndAlso LoadVehicleDropDownData()
      success = success AndAlso LoadCarDropDownData()
			success = success AndAlso LoadDeadlineDropDownData()
      success = success AndAlso LoadWorkPensumDropDownData()

      Return success
    End Function

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
		''' Handles new value event on reserve 1 to 4 lookup edit.
		''' </summary>
		Private Sub OnLueReserve1To4_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs)
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

#End Region

  End Class

End Namespace
