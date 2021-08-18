
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports SP.MA.BankMng
Imports SP.DatabaseAccess.Applicant.DataObjects

Namespace UI

	Public Class ucBankData

#Region "Private Fields"

		''' <summary>
		''' The bank detail form.
		''' </summary>
		Private m_ApplicationDetailForm As frmBank

		''' <summary>
		''' Check edit for default bank symbol.
		''' </summary>
		Private m_CheckEditDedfaultBank As RepositoryItemCheckEdit

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()


			' Important symbol.
			m_CheckEditDedfaultBank = CType(gridApplication.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditDedfaultBank.PictureChecked = My.Resources.Checked
			m_CheckEditDedfaultBank.PictureUnchecked = Nothing
			m_CheckEditDedfaultBank.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined


		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean
			m_SuppressUIEvents = True

			Dim success = True

			m_EmployeeNumber = Nothing
			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				CleanUp()
				success = success AndAlso LoadEmployeeBankData(employeeNumber)

				m_EmployeeNumber = IIf(success, employeeNumber, 0)

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

			m_EmployeeNumber = Nothing

			ResetBankGrid()

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			' Do nothing
			Return True
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
			End If
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()

			If Not m_ApplicationDetailForm Is Nothing AndAlso
			 Not m_ApplicationDetailForm.IsDisposed Then

				Try
					m_ApplicationDetailForm.Close()
					m_ApplicationDetailForm.Dispose()
				Catch
					' Do nothing
				End Try
			End If

		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

		End Sub

		''' <summary>
		''' Resets the application grid.
		''' </summary>
		Private Sub ResetBankGrid()

			' Reset the grid
			gvApplication.OptionsView.ShowIndicator = False

			gvApplication.Columns.Clear()

			Dim columnApplicationLabel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnApplicationLabel.Caption = m_Translate.GetSafeTranslationValue("Stelle als")
			columnApplicationLabel.Name = "ApplicationLabel"
			columnApplicationLabel.FieldName = "ApplicationLabel"
			columnApplicationLabel.Visible = True
			gvApplication.Columns.Add(columnApplicationLabel)

			Dim columnAdvisor As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdvisor.Caption = m_Translate.GetSafeTranslationValue("BeraterIn")
			columnAdvisor.Name = "Advisor"
			columnAdvisor.FieldName = "Advisor"
			columnAdvisor.Visible = True
			gvApplication.Columns.Add(columnAdvisor)

			Dim columnBusinessBranch As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBusinessBranch.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnBusinessBranch.Name = "BusinessBranch"
			columnBusinessBranch.FieldName = "BusinessBranch"
			columnBusinessBranch.Visible = True
			gvApplication.Columns.Add(columnBusinessBranch)

			Dim columnDismissalperiod As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDismissalperiod.Caption = m_Translate.GetSafeTranslationValue("Kündigungsfrist")
			columnDismissalperiod.Name = "Dismissalperiod"
			columnDismissalperiod.FieldName = "Dismissalperiod"
			columnDismissalperiod.Visible = False
			gvApplication.Columns.Add(columnDismissalperiod)

			Dim activeAvailability As New DevExpress.XtraGrid.Columns.GridColumn()
			activeAvailability.Caption = m_Translate.GetSafeTranslationValue("Verfügbarkeit")
			activeAvailability.Name = "Availability"
			activeAvailability.FieldName = "Availability"
			activeAvailability.Visible = False
			gvApplication.Columns.Add(activeAvailability)

			Dim columnVacancyNumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnVacancyNumber.Caption = m_Translate.GetSafeTranslationValue("VacancyNumber")
			columnVacancyNumber.Name = "VacancyNumber"
			columnVacancyNumber.FieldName = "VacancyNumber"
			columnVacancyNumber.Visible = True
			gvApplication.Columns.Add(columnVacancyNumber)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			gvApplication.Columns.Add(columnCreatedOn)

			Dim columnCheckedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCheckedOn.Caption = m_Translate.GetSafeTranslationValue("CheckedOn")
			columnCheckedOn.Name = "CheckedOn"
			columnCheckedOn.FieldName = "CheckedOn"
			columnCheckedOn.Visible = True
			gvApplication.Columns.Add(columnCheckedOn)

			Dim columnCheckedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCheckedFrom.Caption = m_Translate.GetSafeTranslationValue("CheckedFrom")
			columnCheckedFrom.Name = "CheckedFrom"
			columnCheckedFrom.FieldName = "CheckedFrom"
			columnCheckedFrom.Visible = True
			gvApplication.Columns.Add(columnCheckedFrom)

			m_SuppressUIEvents = True
			gridApplication.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub

		''' <summary>
		''' Loads employee bank data.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadEmployeeBankData(ByVal employeeNumber As Integer)

			Dim employeeBankData = m_applicationDataAccess.LoadAssignedApplicantApplications(m_InitializationData.UserData.UserMDGuid, employeeNumber)

			If (employeeBankData Is Nothing) Then
				Return False
			End If

			Dim listDataSource As BindingList(Of ApplicationData) = New BindingList(Of ApplicationData)

      ' Convert the data to view data.
      'For Each appData In employeeBankData

      '	Dim cViewData = New ApplicationData() With {
      '		.ID = appData.ID,
      '		.Advisor = appData.Advisor,
      '		.ApplicantNumber = appData.ApplicantNumber,
      '		.ApplicationNumber = appData.ApplicationNumber,
      '		.Availability = appData.Availability,
      '		.BusinessBranch = appData.BusinessBranch,
      '		.Comment = appData.Comment,
      '		.CreatedFrom = appData.CreatedFrom,
      '		.CreatedOn = appData.CreatedOn,
      '		.CheckedFrom = appData.CheckedFrom,
      '		.CheckedOn = appData.CheckedOn,
      '		.Dismissalperiod = appData.Dismissalperiod,
      '		.VacancyNumber = appData.VacancyNumber,
      '		.ApplicationLabel = appData.ApplicationLabel
      '	}

      '	listDataSource.Add(cViewData)
      'Next

      m_SuppressUIEvents = True
			gridApplication.DataSource = listDataSource
			'gvBank.Columns("DTABCNR").BestFit()
			'gvBank.Columns("Active").BestFit()


			m_SuppressUIEvents = False

			Me.grpApplication.Text = String.Format("{0} ({1} {2})", m_Translate.GetSafeTranslationValue("Bewerbungen"),
																											listDataSource.Count,
																										 m_Translate.GetSafeTranslationValue("Datensätze"))


			Return True

		End Function

		''' <summary>
		''' Handles double click on bank.
		''' </summary>
		Private Sub OnBank_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvApplication.DoubleClick
			Dim selectedRows = gvApplication.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim bankData = CType(gvApplication.GetRow(selectedRows(0)), BankViewData)
				ShowBankDetailForm(m_EmployeeNumber, bankData.RecordNumber)
			End If
		End Sub

		''' <summary>
		''' Handles click on button new bank.
		''' </summary>
		Private Sub OnBtnAddBank_Click(sender As System.Object, e As System.EventArgs) Handles btnAddBank.Click
			If (IsEmployeeDataLoaded) Then
				ShowBankDetailForm(m_EmployeeNumber, Nothing)
			End If
		End Sub

		''' <summary>
		''' Shows the bank management form.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="bankRecordNumber">The bank record number.</param>
		Private Sub ShowBankDetailForm(ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer?)

			If m_ApplicationDetailForm Is Nothing OrElse m_ApplicationDetailForm.IsDisposed Then

				If Not m_ApplicationDetailForm Is Nothing Then
					'First cleanup handlers of old form before new form is created.
					RemoveHandler m_ApplicationDetailForm.FormClosed, AddressOf OnBankFormClosed
					RemoveHandler m_ApplicationDetailForm.BankDataSaved, AddressOf OnBankDataSaved
					RemoveHandler m_ApplicationDetailForm.BankDataDeleted, AddressOf OnBankFormDataDeleted
				End If

				m_ApplicationDetailForm = New frmBank(m_InitializationData)
				AddHandler m_ApplicationDetailForm.FormClosed, AddressOf OnBankFormClosed
				AddHandler m_ApplicationDetailForm.BankDataSaved, AddressOf OnBankDataSaved
				AddHandler m_ApplicationDetailForm.BankDataDeleted, AddressOf OnBankFormDataDeleted
			End If

			m_ApplicationDetailForm.Show()

			If (bankRecordNumber.HasValue) Then
				m_ApplicationDetailForm.LoadBankData(employeeNumber, bankRecordNumber)
			Else
				m_ApplicationDetailForm.NewBank(employeeNumber)
			End If

			m_ApplicationDetailForm.BringToFront()

		End Sub

		''' <summary>
		''' Handles close of bank form.
		''' </summary>
		Private Sub OnBankFormClosed(sender As System.Object, e As System.EventArgs)
			LoadEmployeeBankData(EmployeeNumber)

			Dim bankForm = CType(sender, frmBank)

			If Not bankForm.SelectedBankViewData Is Nothing Then
				FocusBank(m_EmployeeNumber, bankForm.SelectedBankViewData.RecordNumber)
			End If

		End Sub
		''' <summary>
		''' Handles bank form data saved.
		''' </summary>
		Private Sub OnBankDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer)

			LoadEmployeeBankData(m_EmployeeNumber)

			FocusBank(m_EmployeeNumber, bankRecordNumber)

		End Sub

		''' <summary>
		''' Handles bank form data deleted saved.
		''' </summary>
		Private Sub OnBankFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer)
			LoadEmployeeBankData(m_EmployeeNumber)

		End Sub

		''' <summary>
		''' Focuses a bank.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="bankRecordNumber">The bank record number.</param>
		Private Sub FocusBank(ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer)

			Dim listDataSource As BindingList(Of BankViewData) = gridApplication.DataSource

			Dim bankViewData = listDataSource.Where(Function(data) data.EmployeeNumber = employeeNumber AndAlso
																															 data.RecordNumber = bankRecordNumber).FirstOrDefault()

			If Not bankViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(bankViewData)
				Dim rowHandle = gvApplication.GetRowHandle(sourceIndex)
				gvApplication.FocusedRowHandle = rowHandle
			End If

		End Sub

#End Region

#Region "View helper classes"

		''' <summary>
		''' Bank view data.
		''' </summary>
		Class BankViewData
			Public Property ID As Integer
			Public Property EmployeeNumber As Integer?
			Public Property RecordNumber As Integer?
			Public Property BankLOL As Boolean
			Public Property BankAU As Boolean
			Public Property DTABCNR As String
			Public Property BLZ As String
			Public Property BankName As String
			Public Property BankLocation As String
			Public Property Swift As String
			Public Property AccountNumber As String
			Public Property IBAN As String
			Public Property Address1 As String
			Public Property Address2 As String
			Public Property Address3 As String
			Public Property Address4 As String
			Public Property Active As Boolean
			Public Property BankZG As Boolean
			Public Property zahlart As String
			Public Property CreatedOn As DateTime?
			Public Property CreatedFrom As String
			Public Property ChangedOn As DateTime?
			Public Property ChangedFrom As String

			''' <summary>
			''' Gets the bank name for grid.
			''' </summary>
			''' <returns>Bank name for grid.</returns>
			Public ReadOnly Property BankNameForGrid As String
				Get

					If Not String.IsNullOrEmpty(BankLocation) Then
						Return String.Format("{0}, {1}", BankName, BankLocation)
					Else
						Return BankName
					End If
				End Get
			End Property

		End Class


#End Region


	End Class

End Namespace
