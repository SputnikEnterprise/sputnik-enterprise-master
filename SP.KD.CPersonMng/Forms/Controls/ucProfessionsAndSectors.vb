
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects

Namespace UI

  ''' <summary>
  ''' Professions and sectors.
  ''' </summary>
  Public Class ucProfessionsAndSectors

#Region "Concturctor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()


      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function Activate(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean
      m_SuppressUIEvents = True
      Dim success As Boolean = True

      If (recordNumber.HasValue) Then
        If (Not IsResponsiblePersonDataLoaded) Then
          success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
        ElseIf Not customerNumber = m_CustomerNumber Or
               Not m_RecordNumber = recordNumber Then
          success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
        End If
      Else
        Reset()
      End If
      m_SuppressUIEvents = False

      Return success
    End Function

    ''' <summary>
    ''' Deactivates the control.
    ''' </summary>
    Public Overrides Sub Deactivate()
      ' Do nothing
    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_CustomerNumber = 0
      m_RecordNumber = Nothing

      lstProfessions.DataSource = Nothing
      lstSector.DataSource = Nothing

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean
      ' Do nothing
      Return True
    End Function

    ''' <summary>
    ''' Merges the responsible person master data.
    ''' </summary>
    ''' <param name="responsiblePersonMasterData">The responsible person master data object where the data gets filled into.</param>
    Public Overrides Sub MergeResponsiblePersonMasterData(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData, Optional forceMerge As Boolean = False)
      ' Do nothing
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()
      ' Do nothing
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Trannslate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.grpBerufe.Text = m_translate.GetSafeTranslationValue(Me.grpBerufe.Text)
      Me.grpBranchen.Text = m_translate.GetSafeTranslationValue(Me.grpBranchen.Text)

    End Sub

    ''' <summary>
    '''  Loads responsible person data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadResponsiblePersonData(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadAssignedProfessionData(customerNumber, recordNumber)
      success = success AndAlso LoadAssignedSectorData(customerNumber, recordNumber)

      m_CustomerNumber = IIf(success, customerNumber, 0)
      m_RecordNumber = IIf(success, recordNumber, Nothing)

      Return success

    End Function

    ''' <summary>
    ''' Loads assigned profession data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadAssignedProfessionData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As Boolean

      Dim professionData = m_DataAccess.LoadAssignedProfessionDataOfResponsiblePerson(customerNumber, responsiblePersonRecordNumber)

      If (professionData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Zugeordnete Berufe konnten nicht geladen werden."))
        Return False
      End If

      lstProfessions.DisplayMember = "Description"
      lstProfessions.ValueMember = "ID"
      lstProfessions.DataSource = professionData

      Return True

    End Function

    ''' <summary>
    ''' Loads assigned sector data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadAssignedSectorData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As Boolean

      Dim professionData = m_DataAccess.LoadAssignedSectorDataOfResponsiblePerson(customerNumber, responsiblePersonRecordNumber)

      If (professionData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Zugeordnete Branchen konnten nicht geladen werden."))
        Return False
      End If

      lstSector.DisplayMember = "Description"
      lstSector.ValueMember = "ID"
      lstSector.DataSource = professionData

      Return True

    End Function

    ''' <summary>
    ''' Handles click on add profession button.
    ''' </summary>
    Private Sub OnBtnAddProfession_Click(sender As System.Object, e As System.EventArgs) Handles btnAddJobs.Click

			If (Not IsResponsiblePersonDataLoaded) Then
				Return
			End If

			Dim separatorChars() As Char = {"|", "#"}

			Dim success = True

			' Show profession selection dialog.
			Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
			obj.SelectMultirecords = True

			success = success AndAlso obj.LoadQualificationData("M")
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedProfessionsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return

			If Not selectedProfessionsString Is Nothing AndAlso selectedProfessionsString.Contains("#") Then

				' Tokenize the result string.
				' Result string has the following format <ProfessionCode>#<ProfessionDescription>
				Dim tokens As String() = selectedProfessionsString.Split(separatorChars)

				' It must be an even number of tokens -> otherwhise something is wrong
				If tokens.Count Mod 2 = 0 Then

					' Load assinged professions.
					Dim assignedProfessionData = m_DataAccess.LoadAssignedProfessionDataOfResponsiblePerson(m_CustomerNumber, m_RecordNumber)

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
							ElseIf Not assignedProfessionData.Any(Function(data) Not data.ProfessionCodeInteger Is Nothing AndAlso data.ProfessionCodeInteger = professionCodeInt) Then

								' Add to database
								Dim professionToInsert = New ResponsiblePersonAssignedProfessionData With {.CustomerNumber = m_CustomerNumber,
																							   .ResponsiblePersonRecordNumber = m_RecordNumber,
																							   .ProfessionCodeString = Nothing,
																							   .Description = professionDescription,
																							   .ProfessionCodeInteger = professionCodeInt}
								success = success AndAlso m_DataAccess.AddResponsiblePersonProfessionAssignment(professionToInsert)

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

			LoadAssignedProfessionData(m_CustomerNumber, m_RecordNumber)

		End Sub

		''' <summary>
		''' Handles click on add sector button.
		''' </summary>
		Private Sub OnBtnAddSector_Click(sender As System.Object, e As System.EventArgs) Handles btnAddSector.Click

			If (Not IsResponsiblePersonDataLoaded) Then
				Return
			End If

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
					Dim assignedSectorData = m_DataAccess.LoadAssignedSectorDataOfResponsiblePerson(m_CustomerNumber, m_RecordNumber)

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
								Dim sectorToInsert = New ResponsiblePersonAssignedSectorData With {.CustomerNumber = m_CustomerNumber,
																					   .ResponsiblePersonRecordNumber = m_RecordNumber,
																					   .Description = sectorDescription,
																					   .SectorCode = sectorCodeInt}
								success = success AndAlso m_DataAccess.AddResponsiblePersonSectorAssignment(sectorToInsert)
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

			LoadAssignedSectorData(m_CustomerNumber, m_RecordNumber)

		End Sub

		''' <summary>
		''' Handles keydown event on responsible person professions list.
		''' </summary>
		Private Sub OnLstProfessions_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstProfessions.KeyDown

      If (Not IsResponsiblePersonDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then

        Dim selectedProfessionData As ResponsiblePersonAssignedProfessionData = TryCast(lstProfessions.SelectedItem, ResponsiblePersonAssignedProfessionData)

        If (Not selectedProfessionData Is Nothing) Then

          If Not m_DataAccess.DeleteResponsiblePersonProfessionDataAssignment(selectedProfessionData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Berufszuordnung konnte nicht gelöscht werden."))
          End If

          LoadAssignedProfessionData(m_CustomerNumber, m_RecordNumber)

        End If

      End If
    End Sub


    ''' <summary>
    ''' Handles keydown event on responsible person sector list.
    ''' </summary>
    Private Sub OnLstSector_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstSector.KeyDown

      If (Not IsResponsiblePersonDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then

        Dim selectedSectorData As ResponsiblePersonAssignedSectorData = TryCast(lstSector.SelectedItem, ResponsiblePersonAssignedSectorData)

        If (Not selectedSectorData Is Nothing) Then

          If Not m_DataAccess.DeleteResponsiblePersonSectorDataAssigment(selectedSectorData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Branchenzuordnung konnte nicht gelöscht werden."))
          End If

          LoadAssignedSectorData(m_CustomerNumber, m_RecordNumber)

        End If

      End If
    End Sub

#End Region

  End Class

End Namespace