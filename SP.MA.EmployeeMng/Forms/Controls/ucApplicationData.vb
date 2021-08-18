
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.MA.EmployeeMng.Settings
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Applicant.DataObjects
Imports System.ComponentModel

Namespace UI

	Public Class ucApplicationData

#Region "Private Fields"

		''' <summary>
		''' The monthly salary management detail form.
		''' </summary>
		Private m_MonthlySalaryMagementDetailForm As frmApplicationDetail

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

#End Region

#Region "Constructor"

		Public Sub New()

			m_Mandant = New Mandant

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

		End Sub

#End Region


#Region "readonly properties"

		''' <summary>
		''' Gets the selected document.
		''' </summary>
		''' <returns>The selected employee or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedRecord As ApplicationData
			Get
				Dim gv = TryCast(grdApplication.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gv Is Nothing) Then

					Dim selectedRows = gv.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(gv.GetRow(selectedRows(0)), ApplicationData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region


#Region "Public Methods"

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function Activate(ByVal employeeNumber As Integer) As Boolean
			m_SuppressUIEvents = True

			If Not m_IsIntialControlDataLoaded Then
				Dim settting_filter_onlyCurrentYear As Boolean = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_MONTHLYSALARY_FILTER_ONLY_CURRENT_YEAR)
				chkAsActive.Checked = settting_filter_onlyCurrentYear

				m_IsIntialControlDataLoaded = True
			End If

			Dim success = True

			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				CleanUp()
				m_customerID = m_InitializationData.MDData.MDGuid
				success = success AndAlso LoadMonthlySalaryList(employeeNumber, chkAsActive.Checked)

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

			ResetApplicationOverviewGrid()

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

			If Not m_MonthlySalaryMagementDetailForm Is Nothing AndAlso
				 Not m_MonthlySalaryMagementDetailForm.IsDisposed Then

				Try
					m_MonthlySalaryMagementDetailForm.Close()
					m_MonthlySalaryMagementDetailForm.Dispose()
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

			Me.chkAsActive.Text = m_Translate.GetSafeTranslationValue(Me.chkAsActive.Text)

		End Sub

		''' <summary>
		''' Resets the application overview grid.
		''' </summary>
		Private Sub ResetApplicationOverviewGrid()

			' Reset the grid
			gvApplication.OptionsView.ShowIndicator = False
			gvApplication.OptionsView.ShowColumnHeaders = True
			gvApplication.OptionsView.ShowFooter = False

			gvApplication.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnLANr.Name = "ID"
			columnLANr.FieldName = "ID"
			columnLANr.Visible = False
			columnLANr.Width = 35
			gvApplication.Columns.Add(columnLANr)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnLAName.Name = "ApplicationLabel"
			columnLAName.FieldName = "ApplicationLabel"
			columnLAName.Visible = True
			columnLAName.Width = 150
			gvApplication.Columns.Add(columnLAName)

			Dim columnFromDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFromDate.Caption = m_Translate.GetSafeTranslationValue("Vak.-Nr.")
			columnFromDate.Name = "VacancyNumber"
			columnFromDate.FieldName = "VacancyNumber"
			columnFromDate.Visible = False
			columnFromDate.Width = 50
			gvApplication.Columns.Add(columnFromDate)

			Dim columnToDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnToDate.Caption = m_Translate.GetSafeTranslationValue("Berater")
			columnToDate.Name = "AdvisorFullname"
			columnToDate.FieldName = "AdvisorFullname"
			columnToDate.Visible = True
			columnToDate.Width = 50
			gvApplication.Columns.Add(columnToDate)

			Dim columnCount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCount.Caption = m_Translate.GetSafeTranslationValue("Filiale")
			columnCount.Name = "BusinessBranch"
			columnCount.FieldName = "BusinessBranch"
			columnCount.Visible = True
			columnCount.Width = 50
			gvApplication.Columns.Add(columnCount)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Kündigungsfristen")
			columnBasis.Name = "Dismissalperiod"
			columnBasis.FieldName = "Dismissalperiod"
			columnBasis.Visible = False
			columnBasis.Width = 70
			gvApplication.Columns.Add(columnBasis)

			Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAmount.Caption = m_Translate.GetSafeTranslationValue("Verfügbarkeit")
			columnAmount.Name = "Availability"
			columnAmount.FieldName = "Availability"
			columnAmount.Visible = False
			columnAmount.Width = 70
			gvApplication.Columns.Add(columnAmount)

			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = m_Translate.GetSafeTranslationValue("Status")
			docType.Name = "ApplicationLifecycleLabel"
			docType.FieldName = "ApplicationLifecycleLabel"
			docType.Visible = True
			docType.Width = 70
			gvApplication.Columns.Add(docType)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("CreatedOn")
			columnCreatedOn.Name = "CreatedOn"
			columnCreatedOn.FieldName = "CreatedOn"
			columnCreatedOn.Visible = True
			columnCreatedOn.Width = 70
			gvApplication.Columns.Add(columnCreatedOn)


			grdApplication.DataSource = Nothing

		End Sub

		''' <summary>
		''' Loads the monthly salary overview list.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="onlyActiveRecords">Boolean value indicating if only data of current year should be shown.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadMonthlySalaryList(ByVal employeeNumber As Integer, ByVal onlyActiveRecords As Boolean) As Boolean
			Dim salaryOverviewList = m_EmployeeDataAccess.LoadAssignedEmployeeApplications(String.Empty, employeeNumber, 0, onlyActiveRecords)

			If salaryOverviewList Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bewerbungen konnten nicht geladen werden."))
				Return False
			End If

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			Dim listOfMonthlySalaryViewData As New List(Of ApplicationData)

			Dim gridData = (From person In salaryOverviewList
											Select New ApplicationData With {
													.Advisor = person.Advisor,
													.ApplicationID = person.ApplicationID,
													.ApplicationLabel = person.ApplicationLabel,
													.ApplicationLifecycle = person.ApplicationLifecycle,
													.Availability = person.Availability,
													.BusinessBranch = person.BusinessBranch,
													.CheckedFrom = person.CheckedFrom,
													.CheckedOn = person.CheckedOn,
													.Comment = person.Comment,
													.CreatedFrom = person.CreatedFrom,
													.CreatedOn = person.CreatedOn,
													.Customer_ID = person.Customer_ID,
													.Dismissalperiod = person.Dismissalperiod,
													.EmployeeID = person.EmployeeID,
													.FK_ApplicantID = person.FK_ApplicantID,
													.VacancyNumber = person.VacancyNumber,
													.CustomerNumber = person.CustomerNumber
													}).ToList()

			Dim listDataSource As BindingList(Of ApplicationData) = New BindingList(Of ApplicationData)

			For Each p In salaryOverviewList
				listDataSource.Add(p)
			Next

			grdApplication.DataSource = listDataSource

			m_SuppressUIEvents = suppressUIEventsState


			Return Not (listDataSource Is Nothing)

		End Function

		''' <summary>
		''' Handles double click on monthly salary.
		''' </summary>
		Private Sub OngvApplication_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvApplication.DoubleClick
			Dim selectedRows = gvApplication.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim appData = CType(gvApplication.GetRow(selectedRows(0)), ApplicationData)
				ShowMonthlySalaryDetailForm(m_EmployeeNumber, appData.ID)
			End If
		End Sub


		''' <summary>
		''' Handles check changed event on chkThisYear.
		''' </summary>
		Private Sub OnChkThisYear_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAsActive.CheckedChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If IsEmployeeDataLoaded Then

				LoadMonthlySalaryList(m_EmployeeNumber, chkAsActive.Checked)

				Try
					m_SettingsManager.WriteBoolean(SettingKeys.SETTING_MONTHLYSALARY_FILTER_ONLY_CURRENT_YEAR, chkAsActive.Checked)
				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
				End Try

			End If

		End Sub

		''' <summary>
		''' Shows the monthly salary management form.
		''' </summary>
		''' <param name="lmNr">The monthly salary record number.</param>
		''' <param name="employeeNumber">The employee number.</param>
		Private Sub ShowMonthlySalaryDetailForm(ByVal employeeNumber As Integer, ByVal lmNr As Integer?)

			If m_MonthlySalaryMagementDetailForm Is Nothing OrElse m_MonthlySalaryMagementDetailForm.IsDisposed Then

				If Not m_MonthlySalaryMagementDetailForm Is Nothing Then
					'First cleanup handlers of old form before new form is created.
					RemoveHandler m_MonthlySalaryMagementDetailForm.FormClosed, AddressOf OnMonthlySalaryFormClosed
					RemoveHandler m_MonthlySalaryMagementDetailForm.ApplicationDataSaved, AddressOf OnMonthlySalaryFormDataSaved
				End If
				Dim m_init As SP.Infrastructure.Initialization.InitializeClass
				Dim MandantData = m_Mandant.GetSelectedMDData(m_Mandant.GetDefaultMDNr)
				m_init = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, MandantData, m_InitializationData.UserData)

				Dim data = SelectedRecord
				If data Is Nothing Then Return

				'm_InitializationData()
				m_MonthlySalaryMagementDetailForm = New frmApplicationDetail(m_init, data)
				m_MonthlySalaryMagementDetailForm.LoadData()
				m_MonthlySalaryMagementDetailForm.Show()
				m_MonthlySalaryMagementDetailForm.BringToFront()

				AddHandler m_MonthlySalaryMagementDetailForm.FormClosed, AddressOf OnMonthlySalaryFormClosed
				AddHandler m_MonthlySalaryMagementDetailForm.ApplicationDataSaved, AddressOf OnMonthlySalaryFormDataSaved
			End If

			m_MonthlySalaryMagementDetailForm.Show()

			m_MonthlySalaryMagementDetailForm.LoadData()
			m_MonthlySalaryMagementDetailForm.BringToFront()

		End Sub

		''' <summary>
		''' Handles close of monthly salary form.
		''' </summary>
		Private Sub OnMonthlySalaryFormClosed(sender As System.Object, e As System.EventArgs)
			LoadMonthlySalaryList(EmployeeNumber, chkAsActive.Checked)

			Dim monthlySalaryForm = CType(sender, frmApplicationDetail)

			'If Not monthlySalaryForm.SelectedMonthlySalary Is Nothing Then
			'     FocusMonthlySalary(m_EmployeeNumber, monthlySalaryForm.SelectedMonthlySalary.LMNr)
			'   End If

		End Sub

		''' <summary>
		''' Handles monthly salary form data saved.
		''' </summary>
		Private Sub OnMonthlySalaryFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal appID As Integer)

			LoadMonthlySalaryList(m_EmployeeNumber, chkAsActive.Checked)

			FocusMonthlySalary(m_EmployeeNumber, appID)

		End Sub

		''' <summary>
		''' Handles monthly salary form data deleted saved.
		''' </summary>
		Private Sub OnMonthlySalaryFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal appID As Integer)
			LoadMonthlySalaryList(m_EmployeeNumber, chkAsActive.Checked)
		End Sub

		''' <summary>
		''' Focuses a monthly salary.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="appID">The lmNr record number.</param>
		Private Sub FocusMonthlySalary(ByVal employeeNumber As Integer, ByVal appID As Integer)

			' TODO: must be focused!
			'Dim listDataSource As List(Of ApplicationData) = CType(grdExistsMSalary.DataSource, List(Of ApplicationData))

			'Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.EmployeeID = employeeNumber AndAlso
			'																													 data.ApplicationID = appID).FirstOrDefault()

			'If Not monthlySalaryViewData Is Nothing Then
			'	Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
			'	Dim rowHandle = gvExistsMSalary.GetRowHandle(sourceIndex)
			'	gvExistsMSalary.FocusedRowHandle = rowHandle
			'End If

		End Sub

#End Region


	End Class

End Namespace
