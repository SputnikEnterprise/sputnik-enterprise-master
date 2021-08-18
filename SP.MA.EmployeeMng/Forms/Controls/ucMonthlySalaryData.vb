Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors.Repository
Imports SP.MA.MLohnMng
Imports SP.MA.EmployeeMng.Settings
Imports SPProgUtility.Mandanten

Namespace UI

	Public Class ucMonthlySalaryData

#Region "Private Fields"

		''' <summary>
		''' The monthly salary management detail form.
		''' </summary>
		Private m_MonthlySalaryMagementDetailForm As frmMSalary

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
				chkThisYear.Checked = settting_filter_onlyCurrentYear

				m_IsIntialControlDataLoaded = True
			End If

			Dim success = True

			If (Not IsEmployeeDataLoaded OrElse (Not m_EmployeeNumber = employeeNumber)) Then
				CleanUp()
				success = success AndAlso LoadMonthlySalaryList(employeeNumber, chkThisYear.Checked)

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

			ResetMonthlySalaryOverviewGrid()

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

			Me.chkThisYear.Text = m_Translate.GetSafeTranslationValue(Me.chkThisYear.Text)

		End Sub

		''' <summary>
		''' Resets the monthly salary overview grid.
		''' </summary>
		Private Sub ResetMonthlySalaryOverviewGrid()

			' Reset the grid
			gvExistsMSalary.OptionsView.ShowIndicator = False
			gvExistsMSalary.OptionsView.ShowColumnHeaders = False
			gvExistsMSalary.OptionsView.ShowFooter = False

			gvExistsMSalary.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("Lohnart-Nr.")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.Width = 35
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.#####"
			gvExistsMSalary.Columns.Add(columnLANr)

			Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnLAName.Name = "LAName"
			columnLAName.FieldName = "LAName"
			columnLAName.Visible = True
			columnLAName.Width = 235
			gvExistsMSalary.Columns.Add(columnLAName)

			Dim columnFromDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFromDate.Caption = m_Translate.GetSafeTranslationValue("Von")
			columnFromDate.Name = "FromDate"
			columnFromDate.FieldName = "FromDate"
			columnFromDate.Visible = True
			columnFromDate.Width = 70
			gvExistsMSalary.Columns.Add(columnFromDate)

			Dim columnToDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnToDate.Caption = m_Translate.GetSafeTranslationValue("Bis")
			columnToDate.Name = "ToDate"
			columnToDate.FieldName = "ToDate"
			columnToDate.Visible = True
			columnToDate.Width = 70
			gvExistsMSalary.Columns.Add(columnToDate)

			Dim columnCount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCount.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
			columnCount.Name = "Count"
			columnCount.FieldName = "Count"
			columnCount.Visible = True
			columnCount.Width = 70
			columnCount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnCount.DisplayFormat.FormatString = "N"
			columnCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnCount.AppearanceHeader.Options.UseTextOptions = True
			columnCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnCount.AppearanceCell.Options.UseTextOptions = True
			gvExistsMSalary.Columns.Add(columnCount)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = ("Basis")
			columnBasis.Name = "Basis"
			columnBasis.FieldName = "Basis"
			columnBasis.Visible = True
			columnBasis.Width = 70
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N"
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceCell.Options.UseTextOptions = True
			gvExistsMSalary.Columns.Add(columnBasis)

			Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnAmount.Name = "Amount"
			columnAmount.FieldName = "Amount"
			columnAmount.Visible = True
			columnAmount.Width = 70
			columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAmount.DisplayFormat.FormatString = "N"
			columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAmount.AppearanceHeader.Options.UseTextOptions = True
			columnAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAmount.AppearanceCell.Options.UseTextOptions = True
			gvExistsMSalary.Columns.Add(columnAmount)

			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = " "
			docType.Name = "docType"
			docType.FieldName = "docType"
			docType.Visible = True
			Dim picutureEdit As New RepositoryItemPictureEdit()
			picutureEdit.NullText = " "
			docType.ColumnEdit = picutureEdit
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			docType.Width = 20
			gvExistsMSalary.Columns.Add(docType)

		End Sub

		''' <summary>
		''' Loads the monthly salary overview list.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="onlyCurrentYear">Boolean value indicating if only data of current year should be shown.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadMonthlySalaryList(ByVal employeeNumber As Integer, ByVal onlyCurrentYear As Boolean) As Boolean
			Dim salaryOverviewList = m_EmployeeDataAccess.LoadMonthlySalaryOverviewListForMonthlySalaryMng(employeeNumber, 0, onlyCurrentYear)

			If salaryOverviewList Is Nothing Then

				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnangaben konnten nicht geladen werden."))

				Return False

			End If

			Dim listOfMonthlySalaryViewData As New List(Of MonthlySalaryListItemViewData)

			For Each element In salaryOverviewList

				Dim viewData As New MonthlySalaryListItemViewData
				viewData.EmployeeNumber = employeeNumber
				viewData.LMNr = element.LMNr
				viewData.ESNr = element.ESNr
				viewData.LANr = element.LANr
				viewData.LAName = element.LAName
				viewData.FromDate = String.Format("{0} / {1}", element.LP_From, element.Year_From)
				viewData.LP_From = element.LP_From
				Integer.TryParse(element.Year_From, viewData.YearFrom)
				viewData.ToDate = String.Format("{0} / {1}", element.LP_To, element.Year_To)
				viewData.LP_To = element.LP_To
				Integer.TryParse(element.Year_To, viewData.YearTo)

				viewData.Count = element.M_Anz
				viewData.Ans = element.M_Ans
				viewData.Basis = element.M_Bas
				viewData.Amount = element.M_BTR
				viewData.LAIndBez = element.LAIndBez
				viewData.Canton = element.Canton
				viewData.ZGGrund = element.ZGGrund
				viewData.BankNr = element.BankNr
				viewData.CreatedFrom = element.CreatedFrom
				viewData.CreatedOn = element.CreatedOn
				viewData.ChangedFrom = element.ChangedFrom
				viewData.ChangedOn = element.ChangedOn
				viewData.Sign = If(element.Sign Is Nothing, String.Empty, element.Sign)
				viewData.NumberOfExistingLOLRecords = element.NumberOfExistingLOLRecords

				If element.NumberOfExistingLMDocRecords > 0 Then
					viewData.PDFImage = My.Resources.pdf
				End If

				listOfMonthlySalaryViewData.Add(viewData)

			Next

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdExistsMSalary.DataSource = listOfMonthlySalaryViewData
			m_SuppressUIEvents = suppressUIEventsState

			Me.grpMonthlySalary.Text = String.Format("{0} ({1} {2})", m_Translate.GetSafeTranslationValue("Monatliche Lohnangaben"),
																									listOfMonthlySalaryViewData.Count,
																									m_Translate.GetSafeTranslationValue("Datensätze"))

			Return True
		End Function

		''' <summary>
		''' Handles double click on monthly salary.
		''' </summary>
		Private Sub OnMonthlySalary_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvExistsMSalary.DoubleClick
			Dim selectedRows = gvExistsMSalary.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim monthlySalaryData = CType(gvExistsMSalary.GetRow(selectedRows(0)), MonthlySalaryListItemViewData)
				ShowMonthlySalaryDetailForm(m_EmployeeNumber, monthlySalaryData.LMNr)
			End If
		End Sub

		''' <summary>
		''' Handles click on button new monthly salary.
		''' </summary>
		Private Sub OnBtnAddDocument_Click(sender As System.Object, e As System.EventArgs) Handles btnAddDocument.Click
			If (IsEmployeeDataLoaded) Then
				ShowMonthlySalaryDetailForm(m_EmployeeNumber, Nothing)
			End If
		End Sub

		''' <summary>
		''' Handles check changed event on chkThisYear.
		''' </summary>
		Private Sub OnChkThisYear_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkThisYear.CheckedChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If IsEmployeeDataLoaded Then

				LoadMonthlySalaryList(m_EmployeeNumber, chkThisYear.Checked)

				Try
					m_SettingsManager.WriteBoolean(SettingKeys.SETTING_MONTHLYSALARY_FILTER_ONLY_CURRENT_YEAR, chkThisYear.Checked)
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
					RemoveHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataSaved, AddressOf OnMonthlySalaryFormDataSaved
					RemoveHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataDeleted, AddressOf OnMonthlySalaryFormDataDeleted
				End If
				Dim m_init As SP.Infrastructure.Initialization.InitializeClass
				Dim MandantData = m_Mandant.GetSelectedMDData(m_Mandant.GetDefaultMDNr)
				m_init = New SP.Infrastructure.Initialization.InitializeClass(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData, MandantData, m_InitializationData.UserData)

				'm_InitializationData()
				m_MonthlySalaryMagementDetailForm = New frmMSalary(m_init)
				AddHandler m_MonthlySalaryMagementDetailForm.FormClosed, AddressOf OnMonthlySalaryFormClosed
				AddHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataSaved, AddressOf OnMonthlySalaryFormDataSaved
				AddHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataDeleted, AddressOf OnMonthlySalaryFormDataDeleted
			End If

			m_MonthlySalaryMagementDetailForm.Show()
			m_MonthlySalaryMagementDetailForm.IsFilterMonthlySalaryOnlyOfCurrentYearEnabled = chkThisYear.Checked

			If lmNr.HasValue Then
				m_MonthlySalaryMagementDetailForm.LoadData(employeeNumber, lmNr, True)
			Else
				m_MonthlySalaryMagementDetailForm.NewMonthlySalary(employeeNumber, True)
			End If

			m_MonthlySalaryMagementDetailForm.BringToFront()

		End Sub

		''' <summary>
		''' Handles close of monthly salary form.
		''' </summary>
		Private Sub OnMonthlySalaryFormClosed(sender As System.Object, e As System.EventArgs)
			LoadMonthlySalaryList(EmployeeNumber, chkThisYear.Checked)

			Dim monthlySalaryForm = CType(sender, frmMSalary)

			If Not monthlySalaryForm.SelectedMonthlySalary Is Nothing Then
				FocusMonthlySalary(m_EmployeeNumber, monthlySalaryForm.SelectedMonthlySalary.LMNr)
			End If

		End Sub

		''' <summary>
		''' Handles monthly salary form data saved.
		''' </summary>
		Private Sub OnMonthlySalaryFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)

			LoadMonthlySalaryList(m_EmployeeNumber, chkThisYear.Checked)

			FocusMonthlySalary(m_EmployeeNumber, lmnr)

		End Sub

		''' <summary>
		''' Handles monthly salary form data deleted saved.
		''' </summary>
		Private Sub OnMonthlySalaryFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)
			LoadMonthlySalaryList(m_EmployeeNumber, chkThisYear.Checked)
		End Sub

		''' <summary>
		''' Focuses a monthly salary.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <param name="lmNr">The lmNr record number.</param>
		Private Sub FocusMonthlySalary(ByVal employeeNumber As Integer, ByVal lmNr As Integer)

			Dim listDataSource As List(Of MonthlySalaryListItemViewData) = grdExistsMSalary.DataSource

			Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.EmployeeNumber = employeeNumber AndAlso
																															 data.LMNr = lmNr).FirstOrDefault()

			If Not monthlySalaryViewData Is Nothing Then
				Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
				Dim rowHandle = gvExistsMSalary.GetRowHandle(sourceIndex)
				gvExistsMSalary.FocusedRowHandle = rowHandle
			End If

		End Sub

#End Region

#Region "View helper classes"

		''' <summary>
		''' Monthly salary list item view data.
		''' </summary>
		Class MonthlySalaryListItemViewData

			Public Property EmployeeNumber As Integer
			Public Property LMNr As Integer?
			Public Property ESNr As Integer?
			Public Property LANr As Decimal?
			Public Property LAName As String
			Public Property FromDate As String
			Public Property ToDate As String
			Public Property LP_From As Integer?
			Public Property YearFrom As Integer
			Public Property LP_To As Integer?
			Public Property YearTo As Integer
			Public Property Count As Decimal?
			Public Property Ans As Decimal?
			Public Property Basis As Decimal?
			Public Property Amount As Decimal?
			Public Property LAIndBez As String
			Public Property Canton As String
			Public Property ZGGrund As String
			Public Property BankNr As Integer?
			Public Property CreatedFrom As String
			Public Property CreatedOn As DateTime?
			Public Property ChangedFrom As String
			Public Property ChangedOn As DateTime?
			Public Property Sign As String
			Public Property NumberOfExistingLOLRecords As Integer
			Public Property PDFImage As Image

		End Class

#End Region

	End Class

End Namespace
