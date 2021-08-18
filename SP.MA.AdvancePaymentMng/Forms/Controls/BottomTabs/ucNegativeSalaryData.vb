Imports SP.MA.MLohnMng
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Namespace UI

  Public Class ucNegativeSalaryData

#Region "Private Fields"

    ''' <summary>
    ''' The monthly salary management detail form.
    ''' </summary>
    Private m_MonthlySalaryMagementDetailForm As frmMSalary

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_ZGNumber = Nothing

      Dim previousState = SetSuppressUIEventsState(True)

      ' ---Reset drop downs, grids and lists---

      ResetNegativeLMDataGrid()

      SetSuppressUIEventsState(previousState)

    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    Public Overrides Function Activate() As Boolean

      Dim success As Boolean = True

      If m_UCMediator.ZGData Is Nothing Then
        Return False
      End If

      Dim zgNrToLoad = m_UCMediator.ZGData.ZGNr

      If (Not IsAdvancePaymentDataLoaded OrElse (Not m_ZGNumber = zgNrToLoad)) Then
        CleanUp()

        success = success AndAlso LoadData()

        m_ZGNumber = IIf(success, zgNrToLoad, 0)

      End If

      Return success
    End Function

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

#Region "Private Metods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      ' Bank
      Me.grpMonthlySalary.Text = m_Translate.GetSafeTranslationValue(Me.grpMonthlySalary.Text)

    End Sub

    ''' <summary>
    ''' Resets the negative LM data grid.
    ''' </summary>
    Private Sub ResetNegativeLMDataGrid()

      ' Reset the grid
      gvMSalary.OptionsView.ShowIndicator = False
      gvMSalary.OptionsView.ShowAutoFilterRow = True
      gvMSalary.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
      gvMSalary.Columns.Clear()

      Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
      columnLANr.Name = "LANr"
      columnLANr.FieldName = "LANr"
      columnLANr.Visible = True
      columnLANr.Width = 70
      columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnLANr.DisplayFormat.FormatString = "0.###"
      gvMSalary.Columns.Add(columnLANr)

      Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
      columnLAName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
      columnLAName.Name = "LAName"
      columnLAName.FieldName = "LAName"
      columnLAName.Visible = True
      columnLAName.Width = 70
      gvMSalary.Columns.Add(columnLAName)

      Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
      columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
      columnAmount.Name = "M_Btr"
      columnAmount.FieldName = "M_Btr"
      columnAmount.Visible = True
      columnAmount.Width = 70
      columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnAmount.DisplayFormat.FormatString = "N"
      columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnAmount.AppearanceHeader.Options.UseTextOptions = True
      columnAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnAmount.AppearanceCell.Options.UseTextOptions = True
      gvMSalary.Columns.Add(columnAmount)

      Dim columnMonatJahrVon As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMonatJahrVon.Caption = m_Translate.GetSafeTranslationValue("Von")
      columnMonatJahrVon.Name = "MonatJahrVon"
      columnMonatJahrVon.FieldName = "MonatJahrVon"
      columnMonatJahrVon.Visible = True
      columnMonatJahrVon.Width = 70
      gvMSalary.Columns.Add(columnMonatJahrVon)

      Dim columnMonatJahrBis As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMonatJahrBis.Caption = m_Translate.GetSafeTranslationValue("Bis")
      columnMonatJahrBis.Name = "MonatJahrBis"
      columnMonatJahrBis.FieldName = "MonatJahrBis"
      columnMonatJahrBis.Visible = True
      columnMonatJahrBis.Width = 70
      gvMSalary.Columns.Add(columnMonatJahrBis)

      Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
      columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Datum")
      columnCreatedOn.Name = "CreatedOn"
      columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
      columnCreatedOn.DisplayFormat.FormatString = "dd.MM.yyyy HH:mm"
      columnCreatedOn.FieldName = "CreatedOn"
      columnCreatedOn.Visible = True
      gvMSalary.Columns.Add(columnCreatedOn)

      Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
      columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt von")
      columnCreatedFrom.Name = "CreatedFrom"
      columnCreatedFrom.FieldName = "CreatedFrom"
      columnCreatedFrom.Visible = True
      columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
      gvMSalary.Columns.Add(columnCreatedFrom)

    End Sub

    ''' <summary>
    ''' Loads the data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadData() As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadNegativeLMData()

      Return success

    End Function

    ''' <summary>
    ''' Loads the negative LM data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadNegativeLMData() As Boolean

      Dim zgData = m_UCMediator.ZGData

      Dim negativeLMData = m_AdvancePaymentDbAcccess.LoadNegativeLMData(zgData.MANR, zgData.LP, zgData.JAHR)

      If negativeLMData Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Negative Lohndaten konnten nicht geladen werden."))
      End If

      grdMSalary.DataSource = negativeLMData

      Return True
    End Function


    ''' <summary>
    ''' Handles double click on monthly salary.
    ''' </summary>
    Private Sub OnMonthlySalary_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvMSalary.DoubleClick
      Dim selectedRows = gvMSalary.GetSelectedRows()

      If (selectedRows.Count > 0) Then

        If gvMSalary.GetRow(selectedRows(0)) Is Nothing Then Return

        Dim monthlySalaryData = CType(gvMSalary.GetRow(selectedRows(0)), NegativeLMData)
        ShowMonthlySalaryDetailForm(m_UCMediator.ZGData.MANR, monthlySalaryData.LMNR)
      End If
    End Sub

    ''' <summary>
    ''' Handles click on button new monthly salary.
    ''' </summary>
    Private Sub OnBtnAddMonthlySalary_Click(sender As System.Object, e As System.EventArgs) Handles btnAddMonthlySalary.Click
      If (IsAdvancePaymentDataLoaded) Then
        ShowMonthlySalaryDetailForm(m_UCMediator.ZGData.MANR, Nothing)
      End If
    End Sub

    ''' <summary>
    ''' Handles close of monthly salary form.
    ''' </summary>
    Private Sub OnMonthlySalaryFormClosed(sender As System.Object, e As System.EventArgs)
      LoadData()

      Dim monthlySalaryForm = CType(sender, frmMSalary)

      If Not monthlySalaryForm.SelectedMonthlySalary Is Nothing Then
        FocusMonthlySalary(m_UCMediator.ZGData.MANR, monthlySalaryForm.SelectedMonthlySalary.LMNr)
      End If

    End Sub

    ''' <summary>
    ''' Handles monthly salary form data saved.
    ''' </summary>
    Private Sub OnMonthlySalaryFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)

      LoadData()

      FocusMonthlySalary(m_UCMediator.ZGData.MANR, lmnr)

    End Sub

    ''' <summary>
    ''' Handles monthly salary form data deleted saved.
    ''' </summary>
    Private Sub OnMonthlySalaryFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)
      LoadData()
    End Sub

    ''' <summary>
    ''' Focuses a monthly salary.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="lmNr">The lmNr record number.</param>
    Private Sub FocusMonthlySalary(ByVal employeeNumber As Integer, ByVal lmNr As Integer)

      Dim listDataSource As List(Of NegativeLMData) = grdMSalary.DataSource

      Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.MANR = employeeNumber AndAlso
                                                               data.LMNR = lmNr).FirstOrDefault()

      If Not monthlySalaryViewData Is Nothing Then
        Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
        Dim rowHandle = gvMSalary.GetRowHandle(sourceIndex)
        gvMSalary.FocusedRowHandle = rowHandle
      End If

    End Sub

#End Region

#Region "Helper Methods"

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

        m_MonthlySalaryMagementDetailForm = New frmMSalary(m_InitializationData)
        AddHandler m_MonthlySalaryMagementDetailForm.FormClosed, AddressOf OnMonthlySalaryFormClosed
        AddHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataSaved, AddressOf OnMonthlySalaryFormDataSaved
        AddHandler m_MonthlySalaryMagementDetailForm.MonthlySalaryDataDeleted, AddressOf OnMonthlySalaryFormDataDeleted
      End If

      m_MonthlySalaryMagementDetailForm.Show()

      If lmNr.HasValue Then
        m_MonthlySalaryMagementDetailForm.LoadData(employeeNumber, lmNr, True)
      Else
        m_MonthlySalaryMagementDetailForm.NewMonthlySalary(employeeNumber, True)
      End If

      m_MonthlySalaryMagementDetailForm.BringToFront()

    End Sub

#End Region


#Region "View helper classes"

    ''' <summary>
    ''' Bankdetail list item view data.
    ''' </summary>
    Class BankDetailViewData
      Public Property Description As String
      Public Property Value As String
    End Class

#End Region

  End Class

End Namespace