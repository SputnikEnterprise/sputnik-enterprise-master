'Imports DevExpress.XtraEditors.Controls
'Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
'Imports DevExpress.XtraEditors
'Imports DevExpress.XtraEditors.Repository
'Imports SP.MA.BankMng

'Namespace UI

'  Public Class ucPageSelectBank

'#Region "Private Fields"

'    Private m_BankData As IEnumerable(Of EmployeeBankData)

'    Private m_CheckEditDedfaultBank As RepositoryItemCheckEdit

'    ''' <summary>
'    ''' The bank detail form.
'    ''' </summary>
'    Private m_BankDetailForm As frmBank

'#End Region

'#Region "Private Properties"

'    ''' <summary>
'    ''' Gets the selected bank.
'    ''' </summary>
'    ''' <returns>The selected bank or nothing.</returns>
'    Public ReadOnly Property SelectedBank As EmployeeBankData
'      Get

'        If lueBank.EditValue Is Nothing Or m_BankData Is Nothing Then
'          Return Nothing
'        End If

'        Dim bankData = m_BankData.Where(Function(data) data.ID = lueBank.EditValue).FirstOrDefault()
'        Return bankData
'      End Get
'    End Property

'#End Region

'#Region "Constructor"

'    ''' <summary>
'    ''' The constructor.
'    ''' </summary>
'    ''' <remarks></remarks>
'    Public Sub New()

'      ' Dieser Aufruf ist für den Designer erforderlich.
'      InitializeComponent()

'      ' Important symbol.
'      m_CheckEditDedfaultBank = New RepositoryItemCheckEdit() ' CType(lueBank.Add("CheckEdit"), RepositoryItemCheckEdit)
'      m_CheckEditDedfaultBank.PictureChecked = My.Resources.Checked
'      m_CheckEditDedfaultBank.PictureUnchecked = Nothing
'      m_CheckEditDedfaultBank.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

'      AddHandler lueBank.ButtonClick, AddressOf OnDropDown_ButtonClick

'    End Sub

'#End Region

'#Region "Public Properties"


'    ''' <summary>
'    ''' Gets the selected bank data.
'    ''' </summary>
'    ''' <returns>Bank data.</returns>
'    Public ReadOnly Property SelectedBankData As InitBankData
'      Get

'        Dim data As New InitBankData With {
'          .BankData = SelectedBank
'        }

'        Return data
'      End Get
'    End Property

'#End Region

'#Region "Public Methods"

'    ''' <summary>
'    ''' Inits the control with configuration information.
'    ''' </summary>
'    '''<param name="initializationClass">The initialization class.</param>
'    '''<param name="translationHelper">The translation helper.</param>
'    Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
'      MyBase.InitWithConfigurationData(initializationClass, translationHelper)
'    End Sub

'    ''' <summary>
'    ''' Activates the page.
'    ''' </summary>
'    ''' <returns>Boolean value indicating success.</returns>
'    Public Overrides Function ActivatePage() As Boolean

'      Dim success As Boolean = True

'      If m_IsFirstPageActivation Then
'        success = success AndAlso LoadBankDropDownData()

'        PreselectData()

'      End If

'      m_IsFirstPageActivation = False

'      Return success
'    End Function

'    ''' <summary>
'    ''' Resets the control.
'    ''' </summary>
'    Public Overrides Sub Reset()

'      m_IsFirstPageActivation = True

'      m_BankData = Nothing

'      '  Reset drop downs and lists

'      ResetBankDropDown()
'      ResetBankDetailGrid()

'      ErrorProvider.Clear()

'    End Sub

'    ''' <summary>
'    ''' Validated data.
'    ''' </summary>
'    Public Overrides Function ValidateData() As Boolean

'      ErrorProvider.Clear()

'      Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

'      Dim isValid As Boolean = True
'			Dim PaymentType As Integer = m_UCMediator.SelectedPaymentData.PaymentType

'			isValid = isValid And SetErrorIfInvalid(lueBank, ErrorProvider, PaymentType = 8920 AndAlso lueBank.EditValue Is Nothing, errorText)

'      Return isValid

'    End Function


'#End Region

'#Region "Reset"

'    ''' <summary>
'    ''' Resets the bank drop down data.
'    ''' </summary>
'    Private Sub ResetBankDropDown()

'      lueBank.Properties.DisplayMember = "Bank"
'      lueBank.Properties.ValueMember = "ID"

'      gvBank.OptionsView.ShowIndicator = False
'      gvBank.OptionsView.ShowColumnHeaders = True
'      gvBank.OptionsView.ShowFooter = False
'      gvBank.OptionsView.ShowAutoFilterRow = True
'      gvBank.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

'      gvBank.Columns.Clear()

'      Dim zgBankColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'      zgBankColumn.Caption = m_Translate.GetSafeTranslationValue("Vorschuss")
'      zgBankColumn.Name = "BankZG"
'      zgBankColumn.FieldName = "BankZG"
'      zgBankColumn.Visible = True
'      zgBankColumn.ColumnEdit = m_CheckEditDedfaultBank
'      gvBank.Columns.Add(zgBankColumn)

'      Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'      activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
'      activeColumn.Name = "ActiveRec"
'      activeColumn.FieldName = "ActiveRec"
'      activeColumn.Visible = True
'      activeColumn.ColumnEdit = m_CheckEditDedfaultBank
'      gvBank.Columns.Add(activeColumn)

'			Dim BankAUColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'			BankAUColumn.Caption = m_Translate.GetSafeTranslationValue("Ausland")
'			BankAUColumn.Name = "BankAU"
'			BankAUColumn.FieldName = "BankAU"
'			BankAUColumn.Visible = True
'			BankAUColumn.ColumnEdit = m_CheckEditDedfaultBank
'			gvBank.Columns.Add(BankAUColumn)

'      Dim columnAdditionalText As New DevExpress.XtraGrid.Columns.GridColumn()
'      columnAdditionalText.Caption = m_Translate.GetSafeTranslationValue("Name")
'      columnAdditionalText.Name = "Bank"
'      columnAdditionalText.FieldName = "Bank"
'      columnAdditionalText.Visible = True
'      columnAdditionalText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'      gvBank.Columns.Add(columnAdditionalText)

'			Dim columnIban As New DevExpress.XtraGrid.Columns.GridColumn()
'			columnIban.Caption = m_Translate.GetSafeTranslationValue("IBAN")
'			columnIban.Name = "IBANNr"
'			columnIban.FieldName = "IBANNr"
'			columnIban.Visible = True
'			columnIban.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'			gvBank.Columns.Add(columnIban)

'			Dim columnAccountNr As New DevExpress.XtraGrid.Columns.GridColumn()
'			columnAccountNr.Caption = m_Translate.GetSafeTranslationValue("Konto-Nr.")
'			columnAccountNr.Name = "AccountNr"
'			columnAccountNr.FieldName = "AccountNr"
'			columnAccountNr.Visible = True
'			columnAccountNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
'			gvBank.Columns.Add(columnAccountNr)


'      lueBank.Properties.BestFitMode = BestFitMode.BestFitResizePopup
'      lueBank.Properties.NullText = String.Empty
'      lueBank.EditValue = Nothing
'    End Sub

'    ''' <summary>
'    ''' Resets the Bank detail grid.
'    ''' </summary>
'    Private Sub ResetBankDetailGrid()

'      gvBankDetail.BorderStyle = BorderStyles.NoBorder
'      gvBankDetail.OptionsView.ShowIndicator = False

'      gvBankDetail.OptionsView.ShowColumnHeaders = False
'      gvBankDetail.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
'      gvBankDetail.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
'      gvBankDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
'      gvBankDetail.OptionsSelection.EnableAppearanceFocusedRow = False
'      gvBankDetail.OptionsSelection.EnableAppearanceHideSelection = False

'      ' Reset the grid
'      gvBankDetail.Columns.Clear()

'      Dim columnDescriptionName As New DevExpress.XtraGrid.Columns.GridColumn()
'      columnDescriptionName.Caption = "Description"
'      columnDescriptionName.Name = "Description"
'      columnDescriptionName.FieldName = "Description"
'      columnDescriptionName.Visible = True
'      columnDescriptionName.Width = 150
'      gvBankDetail.Columns.Add(columnDescriptionName)

'      Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
'      columnValue.Caption = "Value"
'      columnValue.Name = "Value"
'      columnValue.FieldName = "Value"
'      columnValue.Visible = True
'      columnValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
'      columnValue.AppearanceHeader.Options.UseTextOptions = True
'      columnValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
'      columnValue.AppearanceCell.Options.UseTextOptions = True
'      columnValue.Width = 300
'      gvBankDetail.Columns.Add(columnValue)

'    End Sub

'#End Region

'#Region "Load Data"

'    ''' <summary>
'    ''' Loads the bank drop down data.
'    ''' </summary>
'    Private Function LoadBankDropDownData() As Boolean

'      Dim selectedEmployee = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData

'      m_BankData = m_UCMediator.EmployeeDbAccess.LoadEmployeeBanks(selectedEmployee.EmployeeNumber)

'      If (m_BankData Is Nothing) Then
'        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht geladen werden."))
'      End If

'      lueBank.EditValue = Nothing
'      lueBank.Properties.DataSource = m_BankData

'      Return m_BankData IsNot Nothing
'    End Function

'#End Region

'#Region "Event Handlers"

'    ''' <summary>
'    ''' Handles change of bank.
'    ''' </summary>
'    Private Sub OnLueBank_EditValueChanged(sender As Object, e As EventArgs) Handles lueBank.EditValueChanged

'      If (m_SuppressUIEvents) Then
'        Return
'      End If

'      SetBankDetails(SelectedBank)

'    End Sub

'    ''' <summary>
'    ''' Handles button click on bank.
'    ''' </summary>
'    Private Sub OnLueBank_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueBank.ButtonClick

'      If (e.Button.Index = 2) Then
'        Dim employeeData = m_UCMediator.SelectedCandidateAndAdvisorData.EmployeeData
'        Dim bank = SelectedBank
'        Dim selectedBankRecordNumber As Integer? = Nothing

'        If Not bank Is Nothing Then
'          selectedBankRecordNumber = bank.RecordNumber
'        End If

'        ShowBankDetailForm(employeeData.EmployeeNumber, selectedBankRecordNumber)

'        LoadBankDropDownData()

'        ' Reselect bank than was previously selected.
'        If selectedBankRecordNumber.HasValue Then

'          Dim bankToReSelect = m_BankData.Where(Function(data) data.RecordNumber.HasValue AndAlso data.RecordNumber = selectedBankRecordNumber).FirstOrDefault()

'          If Not bankToReSelect Is Nothing Then
'            lueBank.EditValue = bankToReSelect.ID
'          End If

'        End If

'      End If

'    End Sub

'    ''' <summary>
'    ''' Handles drop down button clicks.
'    ''' </summary>
'    Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

'      Const ID_OF_DELETE_BUTTON As Int32 = 1

'      ' If delete button has been clicked reset the drop down.
'      If e.Button.Index = ID_OF_DELETE_BUTTON Then

'        If TypeOf sender Is BaseEdit Then
'          If CType(sender, BaseEdit).Properties.ReadOnly Then
'            ' nothing
'          Else
'            CType(sender, BaseEdit).EditValue = Nothing
'          End If
'        End If

'      End If
'    End Sub

'#End Region

'#Region "Helper Methods"

'    ''' <summary>
'    ''' Sets the bank details.
'    ''' </summary>
'    ''' <param name="bank">The bank.</param>
'    Private Sub SetBankDetails(ByVal bank As EmployeeBankData)

'      Dim listOfDetailData = New List(Of BankDetailViewData)

'      Dim bankNameDetailData = New BankDetailViewData With {.Description = m_Translate.GetSafeTranslationValue("Name")}
'      Dim bankLocationDetailData = New BankDetailViewData With {.Description = m_Translate.GetSafeTranslationValue("Ort")}
'      Dim bankClearingNumberDetailData = New BankDetailViewData With {.Description = m_Translate.GetSafeTranslationValue("Clearing Nummer")}
'      Dim bankAccountNumberDetailData = New BankDetailViewData With {.Description = m_Translate.GetSafeTranslationValue("Kontonummer")}
'      Dim bankIBANNumberDetailData = New BankDetailViewData With {.Description = m_Translate.GetSafeTranslationValue("IBAN")}
'      Dim bankForeignDetailData = New BankDetailViewData With {.Description = m_Translate.GetSafeTranslationValue("Im Ausland")}

'      If bank IsNot Nothing Then

'        bankNameDetailData.Value = bank.Bank
'        bankLocationDetailData.Value = bank.BankLocation
'        bankClearingNumberDetailData.Value = bank.DTABCNR
'        bankAccountNumberDetailData.Value = bank.AccountNr
'        bankIBANNumberDetailData.Value = bank.IBANNr
'        bankForeignDetailData.Value = If(bank.BankAU.HasValue AndAlso bank.BankAU, m_Translate.GetSafeTranslationValue("Ja"), m_Translate.GetSafeTranslationValue("Nein"))
'      End If

'      listOfDetailData.Add(bankNameDetailData)
'      listOfDetailData.Add(bankLocationDetailData)
'      listOfDetailData.Add(bankClearingNumberDetailData)
'      listOfDetailData.Add(bankAccountNumberDetailData)
'      listOfDetailData.Add(bankIBANNumberDetailData)
'      listOfDetailData.Add(bankForeignDetailData)

'      grdBankDetail.DataSource = listOfDetailData

'    End Sub

'    ''' <summary>
'    ''' Shows the bank management form.
'    ''' </summary>
'    ''' <param name="employeeNumber">The employee number.</param>
'    ''' <param name="bankRecordNumber">The bank record number.</param>
'    Private Sub ShowBankDetailForm(ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer?)

'      m_BankDetailForm = New frmBank(m_InitializationData)

'      If (bankRecordNumber.HasValue) Then
'        m_BankDetailForm.LoadBankData(employeeNumber, bankRecordNumber)
'      Else
'        m_BankDetailForm.NewBank(employeeNumber)
'      End If

'      m_BankDetailForm.ShowDialog()

'    End Sub

'#End Region

'#Region "Preselection"

'    ''' <summary>
'    ''' Preselects data.
'    ''' </summary>
'    Private Sub PreselectData()

'      Dim supressUIEventState = m_SuppressUIEvents
'      m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

'      If m_BankData Is Nothing Then
'        lueBank.EditValue = Nothing
'        Return
'      End If

'      Dim idBankToSelect As Integer?

'      '  ZG Bank
'      Dim zgBank = m_BankData.Where(Function(data) data.BankZG.HasValue AndAlso data.BankZG).FirstOrDefault
'      If zgBank IsNot Nothing Then
'        idBankToSelect = zgBank.ID
'      End If

'      If Not idBankToSelect.HasValue Then

'        ' Use active bank if no bank is checked as ZG.
'        Dim activeBank = m_BankData.Where(Function(data) data.ActiveRec.HasValue AndAlso data.ActiveRec).FirstOrDefault

'        If activeBank IsNot Nothing Then
'          idBankToSelect = activeBank.ID
'        End If

'      End If

'      lueBank.EditValue = idBankToSelect

'      m_SuppressUIEvents = supressUIEventState

'    End Sub


'#End Region

'#Region "View helper classes"

'    ''' <summary>
'    ''' Bankdetail list item view data.
'    ''' </summary>
'    Class BankDetailViewData
'      Public Property Description As String
'      Public Property Value As String
'    End Class

'#End Region

'  End Class

'End Namespace
