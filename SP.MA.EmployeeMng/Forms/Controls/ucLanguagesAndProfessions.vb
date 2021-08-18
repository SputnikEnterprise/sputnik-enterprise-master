
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.Infrastructure.ucListSelectPopup
Imports SP.DatabaseAccess.Employee.DataObjects.MediationMng
Imports System.Linq
Imports SP.DatabaseAccess.Employee.DataObjects.LanguagesAndProfessionsMng

Namespace UI

  Public Class ucLanguagesAndProfessions

#Region "Private Consts"

    Private Const POPUP_DEFAULT_WIDTH As Integer = 300
    Private Const POPUP_DEFAULT_HEIGHT As Integer = 280

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' Jobcandidate language popup data.
    ''' </summary>
    Private m_JobCandidateLanguagePopupData As IEnumerable(Of JobCandidateLanguageData)

    ''' <summary>
    ''' JobCandidate language popup column definitions.
    ''' </summary>
    Private m_JobCandidateLanguagePopupColumns As New List(Of PopupColumDefintion)

#End Region


#Region "Constructor"

    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Register popup row click handlers
      AddHandler ucVerbalLanguagePopup.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucWrittenLanguagePopup.RowClicked, AddressOf OnPopupRowClicked

      ' Register size changed handlers
      AddHandler ucVerbalLanguagePopup.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucWrittenLanguagePopup.PopupSizeChanged, AddressOf OnPopupSizeChanged

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
      m_JobCandidateLanguagePopupColumns.Add(New PopupColumDefintion With {.Name = "TranslatedLanguageText",
                                                                           .Translation = m_Translate.GetSafeTranslationValue("Sprache"),
                                                                           .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})

    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean

      Dim success As Boolean = True

      If (Not IsIntialControlDataLoaded) Then
        ' Load inital data here if needed.
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

      '  Reset drop downs and lists

      lstProfessions.DataSource = Nothing
      lstSector.DataSource = Nothing
      lstVerbalLanguages.DataSource = Nothing
      lstWrittenLanguages.DataSource = Nothing

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
        ' No employee master data (table Mitarbeiter) to merge.
      End If
    End Sub

    ''' <summary>
    '''  Merges the employee contact other data (MASonstiges).
    ''' </summary>
    ''' <param name="employeeOtherData">The employee other data.</param>
    Public Overrides Sub MergeEmployeeOtherData(ByVal employeeOtherData As EmployeeOtherData)
      If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeOtherData.EmployeeNumber) Then
        ' No employee other data (MASonstiges) to merge
      End If
    End Sub

    ''' <summary>
    '''  Merges the employee contact comm data.
    ''' </summary>
    ''' <param name="employeeContactCommData">The employee contact comm data.</param>
    Public Overrides Sub MergeEmployeeContactCommData(ByVal employeeContactCommData As EmployeeContactComm)
      If (IsEmployeeDataLoaded AndAlso m_EmployeeNumber = employeeContactCommData.EmployeeNumber) Then
        ' No employee other data (MA_KontaktKomm) to merge
      End If
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()
      HidePopups()
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.grpberufe.Text = m_Translate.GetSafeTranslationValue(Me.grpberufe.Text)
      Me.grpbranchen.Text = m_Translate.GetSafeTranslationValue(Me.grpbranchen.Text)
      Me.grpVerbalLanguages.Text = m_Translate.GetSafeTranslationValue(Me.grpVerbalLanguages.Text)
      Me.grpWrittenLanguages.Text = m_Translate.GetSafeTranslationValue(Me.grpWrittenLanguages.Text)

    End Sub

    ''' <summary>
    '''  Loads responsible person data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadEmployeeAssignedProfessionData(employeeNumber)
      success = success AndAlso LoadEmployeeAssignedSectorData(employeeNumber)
      success = success AndAlso LoadEmployeeAssignedVerbalLanguageData(employeeNumber)
      success = success AndAlso LoadEmployeeAssignedWrittenLanguageData(employeeNumber)

      Return success
    End Function

    ''' <summary>
    ''' Loads employee assigned profession data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedProfessionData(ByVal employeeNumber As Integer) As Boolean

			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(employeeNumber, False)
			Dim professionData = m_EmployeeDataAccess.LoadEmployeeAssignedProfessionData(employeeNumber, employeeMasterData.Gender)

			If (professionData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugeordnete Berufe konnten nicht geladen werden."))
        Return False
      End If

      lstProfessions.DisplayMember = "TranslatedProfessionText"
      lstProfessions.ValueMember = "ProfessionText"
      lstProfessions.DataSource = professionData

      Return True

    End Function

    ''' <summary>
    ''' Loads employee assigned sector data (Branche).
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedSectorData(ByVal employeeNumber As Integer) As Boolean

      Dim sectorData = m_EmployeeDataAccess.LoadEmployeeAssignedSectorData(employeeNumber)

      If (sectorData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugeordnete Branchen konnten nicht geladen werden."))
        Return False
      End If

      lstSector.DisplayMember = "TranslatedSectorText"
      lstSector.ValueMember = "Description"
      lstSector.DataSource = sectorData

      Return True

    End Function

    ''' <summary>
    ''' Loads employee assigned verbal language data (MA_MSprachen).
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedVerbalLanguageData(ByVal employeeNumber As Integer) As Boolean

      Dim verbalLanguageData = m_EmployeeDataAccess.LoadEmployeeAssignedVerbalLanguageData(employeeNumber)

      If (verbalLanguageData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugeordnete mündliche Sprachen konnten nicht geladen werden."))
        Return False
      End If

      lstVerbalLanguages.DisplayMember = "TranslatedDescriptionText"
      lstVerbalLanguages.ValueMember = "Description"
      lstVerbalLanguages.DataSource = verbalLanguageData

      Return True

    End Function

    ''' <summary>
    ''' Loads employee assigned written language data (MA_MSprachen).
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadEmployeeAssignedWrittenLanguageData(ByVal employeeNumber As Integer) As Boolean

      Dim writtenLanguageData = m_EmployeeDataAccess.LoadEmployeeAssignedWrittenLanguageData(employeeNumber)

      If (writtenLanguageData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zugeordnete schriftliche Sprachen konnten nicht geladen werden."))
        Return False
      End If

      lstWrittenLanguages.DisplayMember = "TranslatedDescriptionText"
      lstWrittenLanguages.ValueMember = "Description"
      lstWrittenLanguages.DataSource = writtenLanguageData

      Return True

    End Function

    ''' <summary>
    ''' Loads job candidate language popup data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadJobCandidateLanguagePopupData() As Boolean

      m_JobCandidateLanguagePopupData = m_EmployeeDataAccess.LoadJobCandidateLanguageData()

      If (m_JobCandidateLanguagePopupData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sprachauswahldaten konnten nicht geladen werden."))
        Return False
      End If

      Return True
    End Function

    ''' <summary>
    ''' Handles click on add job button.
    ''' </summary>
    Private Sub OnBtnAddJobs_Click(sender As System.Object, e As System.EventArgs) Handles btnAddJobs.Click

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			HidePopups()
			Dim separatorChars() As Char = {"|", "#"}

			Dim success = True
			Dim employeeMasterData = m_EmployeeDataAccess.LoadEmployeeMasterData(m_EmployeeNumber, False)

			' Show profession selection dialog.
			Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
			obj.SelectMultirecords = True
			success = success AndAlso obj.LoadQualificationData(employeeMasterData.Gender)
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedProfessionsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return



			'Dim obj As New SPQualicationUtility.ClsMain_Net
			'Dim selectedProfessionsString As String = obj.ShowfrmQualifications(True, employeeMasterData.Gender)

			If Not selectedProfessionsString Is Nothing AndAlso selectedProfessionsString.Contains("#") Then

				' Tokenize the result string.
				' Result string has the following format <ProfessionCode>#<ProfessionDescription>
				Dim tokens As String() = selectedProfessionsString.Split(separatorChars)

				' It must be an even number of tokens -> otherwhise something is wrong
				If tokens.Count Mod 2 = 0 Then

					' Load assinged professions.
					Dim assignedProfessionData = m_EmployeeDataAccess.LoadEmployeeAssignedProfessionData(m_EmployeeNumber, employeeMasterData.Gender)

					If (assignedProfessionData Is Nothing) Then
						success = False
					Else
						For i = 0 To tokens.Count() - 1 Step 2

							Dim professionCodeStr As String = tokens(i)
							Dim professionDescription As String = tokens(i + 1)
							Dim professionCodeInt As Integer = 0

							If Not Integer.TryParse(professionCodeStr, professionCodeInt) Then
								' Parsing of profession code was not possible.
								success = False
							ElseIf Not assignedProfessionData.Any(Function(data) Not data.ProfessionCode Is Nothing AndAlso data.ProfessionCode = professionCodeInt) Then

								' Add to database
								Dim professionToInsert = New EmployeeAssignedProfessionData With {.EmployeeNumber = m_EmployeeNumber, .ProfessionText = professionDescription, .ProfessionCode = professionCodeInt}
								success = success AndAlso m_EmployeeDataAccess.AddEmployeeProfessionAssignment(professionToInsert)

							Else
								' Already assinged -> do nothing
							End If

						Next
					End If
				Else
					' Even number of tokens is invalid.
					success = False
				End If

			End If

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ein oder mehrere Berufszuordnungen konnten nicht hinzugefügt werden."))
			End If

			LoadEmployeeAssignedProfessionData(m_EmployeeNumber)

		End Sub

		''' <summary>
		''' Handles click on add sector.
		''' </summary>
		Private Sub OnBtnAddSector_Click(sender As System.Object, e As System.EventArgs) Handles btnAddSector.Click

			If (Not IsEmployeeDataLoaded) Then
				Return
			End If

			HidePopups()
			Dim separatorChars() As Char = {"|", "#"}

			Dim success = True

			' Show sector selection dialog.
			' Result string has the following format <SectorCode>#<SectorDescription>
			Dim obj As New SPQualicationUtility.frmBranches(m_InitializationData)
			obj.SelectMultirecords = True

			success = success AndAlso obj.LoadBranchesData()
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedSectorsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedSectorsString) Then Return

			If Not selectedSectorsString Is Nothing AndAlso selectedSectorsString.Contains("#") Then

				Dim tokens As String() = selectedSectorsString.Split(separatorChars)

				' It must be an even number of tokens -> otherwhise something is wrong
				If tokens.Count Mod 2 = 0 Then

					' Load assinged sector
					Dim assignedSectorData = m_EmployeeDataAccess.LoadEmployeeAssignedSectorData(m_EmployeeNumber)

					If (assignedSectorData Is Nothing) Then
						success = False
					Else
						For i = 0 To tokens.Count() - 1 Step 2

							Dim sectorStr As String = tokens(i)
							Dim sectorDescription As String = tokens(i + 1)
							Dim sectorCodeInt As Integer = 0

							If Not Integer.TryParse(sectorStr, sectorCodeInt) Then
								' Parsing of profession code was not possible.
								success = False
							ElseIf Not assignedSectorData.Any(Function(data) Not data.SectorCode Is Nothing AndAlso data.SectorCode = sectorCodeInt) Then

								' Add to database
								Dim sectorToInsert = New EmployeeAssignedSectorData With {.EmployeeNumber = m_EmployeeNumber, .Description = sectorDescription, .SectorCode = sectorCodeInt}
								success = success AndAlso m_EmployeeDataAccess.AddEmployeeSectorAssignment(sectorToInsert)
							Else
								' Already assinged -> do nothing
							End If

						Next
					End If
				Else
					' Even number of tokens is invalid.
					success = False
				End If

			End If

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ein oder mehrere Branchenzuordnungen konnten nicht hinzugefügt werden."))
			End If

			LoadEmployeeAssignedSectorData(m_EmployeeNumber)

		End Sub

    ''' <summary>
    ''' Handles keydown event on employee employment type list.
    ''' </summary>
    Private Sub OnLstProfessions_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstProfessions.KeyDown

      If (Not IsEmployeeDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 134, m_InitializationData.MDData.MDNr) Then Return

        Dim selectedProfessionData As EmployeeAssignedProfessionData = TryCast(lstProfessions.SelectedItem, EmployeeAssignedProfessionData)

        If (Not selectedProfessionData Is Nothing) Then

          If Not m_EmployeeDataAccess.DeleteEmployeeProfessionDataAssignment(selectedProfessionData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Berufszuordnung konnte nicht gelöscht werden."))
          End If

          LoadEmployeeAssignedProfessionData(m_EmployeeNumber)

        End If

      End If

    End Sub

    ''' <summary>
    ''' Handles keydown event on employee sector list.
    ''' </summary>
    Private Sub OnLstSector_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstSector.KeyDown

      If (Not IsEmployeeDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 135, m_InitializationData.MDData.MDNr) Then Return

        Dim selectedSectorData As EmployeeAssignedSectorData = TryCast(lstSector.SelectedItem, EmployeeAssignedSectorData)

        If (Not selectedSectorData Is Nothing) Then

          If Not m_EmployeeDataAccess.DeleteEmployeeSectorAssignment(selectedSectorData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Branchenzuordnung konnte nicht gelöscht werden."))
          End If

          LoadEmployeeAssignedSectorData(m_EmployeeNumber)

        End If

      End If
    End Sub

    ''' <summary>
    ''' Handles keydown event on employee verbal languages list.
    ''' </summary>
    Private Sub OnLstVerbalLanguages_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstVerbalLanguages.KeyDown

      If (Not IsEmployeeDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then
				' If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 227, m_InitializationData.MDData.MDNr) Then Exit Sub

        Dim selectedVerbalLanguageData As EmployeeAssignedVerbalLanguageData = TryCast(lstVerbalLanguages.SelectedItem, EmployeeAssignedVerbalLanguageData)

        If (Not selectedVerbalLanguageData Is Nothing) Then

          If Not m_EmployeeDataAccess.DeleteEmployeeVerbalLanguageAssignment(selectedVerbalLanguageData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mündlichesprache konnte nicht gelöscht werden."))
          End If

          LoadEmployeeAssignedVerbalLanguageData(m_EmployeeNumber)

        End If

      End If
    End Sub

    ''' <summary>
    ''' Handles keydown event on employee written languages list.
    ''' </summary>
    Private Sub OnLstWrittenLanguages_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstWrittenLanguages.KeyDown

      If (Not IsEmployeeDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then
				' If Not IsUserActionAllowed(ModulConstants.UserData.UserNr, 227, m_InitializationData.MDData.MDNr) Then Exit Sub

        Dim selectedWrittenlLanguageData As EmployeeAssignedWrittenLanguageData = TryCast(lstWrittenLanguages.SelectedItem, EmployeeAssignedWrittenLanguageData)

        If (Not selectedWrittenlLanguageData Is Nothing) Then

          If Not m_EmployeeDataAccess.DeleteEmployeeWrittenLanguageAssignment(selectedWrittenlLanguageData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Schriftlichesprache konnte nicht gelöscht werden."))
          End If

          LoadEmployeeAssignedWrittenLanguageData(m_EmployeeNumber)

        End If

      End If
    End Sub

    ''' <summary>
    ''' Handles click on add verbal language button.
    ''' </summary>
    Private Sub OnBtnAddVerbalLanguage_Click(sender As System.Object, e As System.EventArgs) Handles btnAddVerbalLanguage.Click
      HidePopups()

      Dim position = Cursor.Position
      If m_JobCandidateLanguagePopupData Is Nothing Then
        LoadJobCandidateLanguagePopupData()
      End If

      If Not m_JobCandidateLanguagePopupData Is Nothing Then

        Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_LANGUAGESANDPROFESSION_VERBALLANGUAGE_SIZE)

        ' Show popup
        ucVerbalLanguagePopup.InitPopup(m_JobCandidateLanguagePopupData, m_JobCandidateLanguagePopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
        ucVerbalLanguagePopup.ShowPopup(position, popupSize)
      End If
    End Sub

    ''' <summary>
    ''' Handles click on add written language button.
    ''' </summary>
    Private Sub OnBtnAddWrittenLanguage_Click(sender As System.Object, e As System.EventArgs) Handles btnAddWrittenLanguage.Click
      HidePopups()

      Dim position = Cursor.Position
      If m_JobCandidateLanguagePopupData Is Nothing Then
        LoadJobCandidateLanguagePopupData()
      End If

      If Not m_JobCandidateLanguagePopupData Is Nothing Then

        Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_LANGUAGESANDPROFESSION_WRITTENLANGUAGE_SIZE)

        ' Show popup
        ucWrittenLanguagePopup.InitPopup(m_JobCandidateLanguagePopupData, m_JobCandidateLanguagePopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
        ucWrittenLanguagePopup.ShowPopup(position, popupSize)
      End If
    End Sub

    ''' <summary>
    ''' Handles click on a row on one of the popups.
    ''' </summary>
    Private Sub OnPopupRowClicked(ByVal sender As Object, ByVal clickedObject As Object)

      Dim success As Boolean = True

      If Object.ReferenceEquals(sender, ucVerbalLanguagePopup) AndAlso
          TypeOf clickedObject Is JobCandidateLanguageData Then
        success = AssignVerbalLanguageDataToEmployee(CType(clickedObject, JobCandidateLanguageData))
      ElseIf Object.ReferenceEquals(sender, ucWrittenLanguagePopup) AndAlso
          TypeOf clickedObject Is JobCandidateLanguageData Then
        success = AssignWrittenLanguageDataToEmployee(CType(clickedObject, JobCandidateLanguageData))
      End If

      HidePopups()

      If Not success Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuordnung konnte nicht duchgeführt werden."))
      End If

    End Sub

    ''' <summary>
    ''' Assign verbal language data to an employee.
    ''' </summary>
    ''' <param name="jobCandidateLanguageData">The language data data to add.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignVerbalLanguageDataToEmployee(ByVal jobCandidateLanguageData As JobCandidateLanguageData) As Boolean

      Dim succcess = True

      ' Load assigned verbal language data.
      Dim assignedVerbalLanguageData = m_EmployeeDataAccess.LoadEmployeeAssignedVerbalLanguageData(m_EmployeeNumber)

      If Not assignedVerbalLanguageData Is Nothing Then

        ' Check if the new verbal language data is not already assigned
        If Not assignedVerbalLanguageData.Any(Function(data) data.Description.ToLower().Trim() = jobCandidateLanguageData.GetField.ToLower().Trim()) Then

          ' Add to database.
          Dim verbalLanguageDataToAssign = New EmployeeAssignedVerbalLanguageData With {.EmployeeNumber = m_EmployeeNumber,
                                                                                      .Description = jobCandidateLanguageData.GetField}
          succcess = m_EmployeeDataAccess.AddEmployeeVerbalLanguageAssignment(verbalLanguageDataToAssign)
        End If
      Else
        succcess = False
      End If

      LoadEmployeeAssignedVerbalLanguageData(m_EmployeeNumber)

      Return succcess

    End Function

    ''' <summary>
    ''' Assign written language data to an employee.
    ''' </summary>
    ''' <param name="jobCandidateLanguageData">The language data data to add.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignWrittenLanguageDataToEmployee(ByVal jobCandidateLanguageData As JobCandidateLanguageData) As Boolean

      Dim succcess = True

      ' Load assigned written language data.
      Dim assignedWrittenLanguageData = m_EmployeeDataAccess.LoadEmployeeAssignedWrittenLanguageData(m_EmployeeNumber)

      If Not assignedWrittenLanguageData Is Nothing Then

        ' Check if the new written language data is not already assigned
        If Not assignedWrittenLanguageData.Any(Function(data) data.Description.ToLower().Trim() = jobCandidateLanguageData.GetField.ToLower().Trim()) Then

          ' Add to database.
          Dim writtenLanguageDataToAssign = New EmployeeAssignedWrittenLanguageData With {.EmployeeNumber = m_EmployeeNumber,
                                                                                      .Description = jobCandidateLanguageData.GetField}
          succcess = m_EmployeeDataAccess.AddEmployeeWrittenLaguageAssignment(writtenLanguageDataToAssign)
        End If
      Else
        succcess = False
      End If

      LoadEmployeeAssignedWrittenLanguageData(m_EmployeeNumber)

      Return succcess

    End Function

    ''' <summary>
    ''' Hides all popups.
    ''' </summary>
    Private Sub HidePopups()
      ucVerbalLanguagePopup.HidePopup()
      ucWrittenLanguagePopup.HidePopup()
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

      If Object.ReferenceEquals(sender, ucVerbalLanguagePopup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_LANGUAGESANDPROFESSION_VERBALLANGUAGE_SIZE
      ElseIf Object.ReferenceEquals(sender, ucWrittenLanguagePopup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_LANGUAGESANDPROFESSION_WRITTENLANGUAGE_SIZE
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
