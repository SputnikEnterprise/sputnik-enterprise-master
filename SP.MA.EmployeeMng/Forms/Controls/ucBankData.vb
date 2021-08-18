
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports SP.MA.BankMng

Namespace UI

  Public Class ucBankData

#Region "Private Fields"

    ''' <summary>
    ''' The bank detail form.
    ''' </summary>
    Private m_BankDetailForm As frmBank

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
      m_CheckEditDedfaultBank = CType(gridBank.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
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

      If Not m_BankDetailForm Is Nothing AndAlso
       Not m_BankDetailForm.IsDisposed Then

        Try
          m_BankDetailForm.Close()
          m_BankDetailForm.Dispose()
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
    ''' Resets the bank grid.
    ''' </summary>
    Private Sub ResetBankGrid()

      ' Reset the grid
      gvBank.OptionsView.ShowIndicator = False

      gvBank.Columns.Clear()

      Dim columnClearningNr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnClearningNr.Caption = m_Translate.GetSafeTranslationValue("Clearing")
      columnClearningNr.Name = "DTABCNR"
      columnClearningNr.FieldName = "DTABCNR"
      columnClearningNr.Visible = True
      gvBank.Columns.Add(columnClearningNr)

      Dim columnBankname As New DevExpress.XtraGrid.Columns.GridColumn()
      columnBankname.Caption = m_Translate.GetSafeTranslationValue("Bank")
      columnBankname.Name = "BankNameForGrid"
      columnBankname.FieldName = "BankNameForGrid"
      columnBankname.Visible = True
      gvBank.Columns.Add(columnBankname)

      Dim columnAccountNumber As New DevExpress.XtraGrid.Columns.GridColumn()
      columnAccountNumber.Caption = m_Translate.GetSafeTranslationValue("Kontonummer")
      columnAccountNumber.Name = "AccountNumber"
      columnAccountNumber.FieldName = "AccountNumber"
      columnAccountNumber.Visible = True
      gvBank.Columns.Add(columnAccountNumber)

      Dim columnIBAN As New DevExpress.XtraGrid.Columns.GridColumn()
      columnIBAN.Caption = m_Translate.GetSafeTranslationValue("IBAN")
      columnIBAN.Name = "IBAN"
      columnIBAN.FieldName = "IBAN"
      columnIBAN.Visible = True
      gvBank.Columns.Add(columnIBAN)

			Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
			columnSwift.Name = "Swift"
			columnSwift.FieldName = "Swift"
			columnSwift.Visible = True
			gvBank.Columns.Add(columnSwift)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = m_Translate.GetSafeTranslationValue("")
      activeColumn.Name = "Active"
      activeColumn.FieldName = "Active"
      activeColumn.Visible = True
      activeColumn.ColumnEdit = m_CheckEditDedfaultBank
      gvBank.Columns.Add(activeColumn)

      m_SuppressUIEvents = True
      gridBank.DataSource = Nothing
      m_SuppressUIEvents = False
    End Sub

    ''' <summary>
    ''' Loads employee bank data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadEmployeeBankData(ByVal employeeNumber As Integer)

      Dim employeeBankData = m_EmployeeDataAccess.LoadEmployeeBanks(employeeNumber)

      If (employeeBankData Is Nothing) Then
        Return False
      End If

      Dim listDataSource As BindingList(Of BankViewData) = New BindingList(Of BankViewData)

      ' Convert the data to view data.
      For Each bankData In employeeBankData

				Dim cViewData = New BankViewData() With {
					.ID = bankData.ID,
					.EmployeeNumber = bankData.EmployeeNumber,
					.RecordNumber = bankData.RecordNumber,
					.BankLOL = (bankData.BnkLOL = True),
					.BankAU = (bankData.BankAU = True),
					.DTABCNR = bankData.DTABCNR,
					.BLZ = bankData.BLZ,
					.BankName = bankData.Bank,
					.BankLocation = bankData.BankLocation,
					.Swift = bankData.Swift,
					.AccountNumber = bankData.AccountNr,
					.IBAN = bankData.IBANNr,
					.Address1 = bankData.DTAAdr1,
					.Address2 = bankData.DTAAdr2,
					.Address3 = bankData.DTAAdr3,
					.Address4 = bankData.DTAAdr4,
					.Active = (bankData.ActiveRec = True),
					.BankZG = (bankData.BankZG = True),
					.zahlart = bankData.zahlart,
					.CreatedOn = bankData.CreatedOn,
					.CreatedFrom = bankData.CreatedFrom,
					.ChangedOn = bankData.ChangedOn,
					.ChangedFrom = bankData.ChangedFrom
				}

        listDataSource.Add(cViewData)
      Next

      m_SuppressUIEvents = True
      gridBank.DataSource = listDataSource
      gvBank.Columns("DTABCNR").BestFit()
			gvBank.Columns("Active").BestFit()

			m_UCMediator.ZahlartCodeHasChanged(employeeNumber)


			m_SuppressUIEvents = False

      Me.grpBank.Text = String.Format("{0} ({1} {2})", m_Translate.GetSafeTranslationValue("Bankdaten"),
                                                      listDataSource.Count,
                                                     m_Translate.GetSafeTranslationValue("Datensätze"))


			Return True

    End Function

    ''' <summary>
    ''' Handles double click on bank.
    ''' </summary>
    Private Sub OnBank_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvBank.DoubleClick
      Dim selectedRows = gvBank.GetSelectedRows()

      If (selectedRows.Count > 0) Then
        Dim bankData = CType(gvBank.GetRow(selectedRows(0)), BankViewData)
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

      If m_BankDetailForm Is Nothing OrElse m_BankDetailForm.IsDisposed Then

        If Not m_BankDetailForm Is Nothing Then
          'First cleanup handlers of old form before new form is created.
          RemoveHandler m_BankDetailForm.FormClosed, AddressOf OnBankFormClosed
          RemoveHandler m_BankDetailForm.BankDataSaved, AddressOf OnBankDataSaved
          RemoveHandler m_BankDetailForm.BankDataDeleted, AddressOf OnBankFormDataDeleted
        End If

        m_BankDetailForm = New frmBank(m_InitializationData)
        AddHandler m_BankDetailForm.FormClosed, AddressOf OnBankFormClosed
        AddHandler m_BankDetailForm.BankDataSaved, AddressOf OnBankDataSaved
        AddHandler m_BankDetailForm.BankDataDeleted, AddressOf OnBankFormDataDeleted
      End If

      m_BankDetailForm.Show()

      If (bankRecordNumber.HasValue) Then
        m_BankDetailForm.LoadBankData(employeeNumber, bankRecordNumber)
      Else
        m_BankDetailForm.NewBank(employeeNumber)
      End If

      m_BankDetailForm.BringToFront()

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

      Dim listDataSource As BindingList(Of BankViewData) = gridBank.DataSource

      Dim bankViewData = listDataSource.Where(Function(data) data.EmployeeNumber = employeeNumber AndAlso
                                                               data.RecordNumber = bankRecordNumber).FirstOrDefault()

      If Not bankViewData Is Nothing Then
        Dim sourceIndex = listDataSource.IndexOf(bankViewData)
        Dim rowHandle = gvBank.GetRowHandle(sourceIndex)
        gvBank.FocusedRowHandle = rowHandle
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
