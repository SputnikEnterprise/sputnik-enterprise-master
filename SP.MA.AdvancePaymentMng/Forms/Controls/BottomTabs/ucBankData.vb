Imports SP.DatabaseAccess.AdvancePaymentMng
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.BankMng
Imports SP.DatabaseAccess.Employee
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports SP.MA.BankMng

Namespace UI

  Public Class ucBankData

#Region "Private Fields"

    Private m_BankData As IEnumerable(Of EmployeeBankData)

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

    Private m_CheckEditDedfaultBank As RepositoryItemCheckEdit

    ''' <summary>
    ''' The bank detail form.
    ''' </summary>
    Private m_BankDetailForm As frmBank

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      ' Important symbol.
      m_CheckEditDedfaultBank = New RepositoryItemCheckEdit() ' CType(lueBank.Add("CheckEdit"), RepositoryItemCheckEdit)
      m_CheckEditDedfaultBank.PictureChecked = My.Resources.Checked
      m_CheckEditDedfaultBank.PictureUnchecked = Nothing
      m_CheckEditDedfaultBank.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

      AddHandler lueBank.ButtonClick, AddressOf OnDropDown_ButtonClick

    End Sub

#End Region

#Region "Private Properties"

    ''' <summary>
    ''' Gets the selected bank.
    ''' </summary>
    ''' <returns>The selected bank or nothing.</returns>
    Public ReadOnly Property SelectedBank As EmployeeBankData
      Get

        If lueBank.EditValue Is Nothing Or m_BankData Is Nothing Then
          Return Nothing
        End If

        Dim bankData = m_BankData.Where(Function(data) data.ID = lueBank.EditValue).FirstOrDefault()
        Return bankData
      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Inits the control with configuration information.
    ''' </summary>
    '''<param name="initializationClass">The initialization class.</param>
    '''<param name="translationHelper">The translation helper.</param>
    Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
      MyBase.InitWithConfigurationData(initializationClass, translationHelper)

      m_EmployeeDataAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_ZGNumber = Nothing
      m_BankData = Nothing

      Dim previousState = SetSuppressUIEventsState(True)

      ' ---Reset drop downs, grids and lists---

      ResetBankDropDown()
      ResetBankDetailGrid()

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

      m_SuppressUIEvents = False
      Return success
    End Function

    ''' <summary>
    ''' Merges the ZG master data.
    ''' </summary>
    ''' <param name="zgMasterData">The ZG master data object where the data gets filled into.</param>
    Public Overrides Sub MergeCustomerMasterData(ByVal zgMasterData As ZGMasterData)

      Dim zgData = m_UCMediator.ZGData

      If ((IsAdvancePaymentDataLoaded AndAlso
       m_ZGNumber = zgData.ZGNr)) Then

        Dim bankData = SelectedBank

        If Not bankData Is Nothing Then

          zgMasterData.Bank = bankData.Bank
          zgMasterData.KontoNr = bankData.AccountNr
          zgMasterData.BankOrt = bankData.BankLocation
          zgMasterData.DTAAdr1 = bankData.DTAAdr1
          zgMasterData.DTAAdr2 = bankData.DTAAdr2
          zgMasterData.DTAAdr3 = bankData.DTAAdr3
          zgMasterData.DTAAdr4 = bankData.DTAAdr4
          zgMasterData.BnkAU = bankData.BankAU
          zgMasterData.IBANNr = bankData.IBANNr
          zgMasterData.Swift = bankData.Swift
          zgMasterData.BLZ = bankData.BLZ

        End If

      End If

    End Sub

    ''' <summary>
    ''' Sets readonly state of controls.
    ''' </summary>
    ''' <param name="isReadonly">Boolean flag indicating if the controls should be readonly.</param>
    Public Overrides Sub SetReadonlyStateOfControls(ByVal isReadonly)
			lueBank.Properties.ReadOnly = isReadonly
			btnAddBank.Enabled = Not isReadonly
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.grpBank.Text = m_Translate.GetSafeTranslationValue(Me.grpBank.Text)
      Me.lblBankdaten.Text = m_Translate.GetSafeTranslationValue(Me.lblBankdaten.Text)

    End Sub

    ''' <summary>
    ''' Resets the bank drop down data.
    ''' </summary>
    Private Sub ResetBankDropDown()

      lueBank.Properties.ReadOnly = False
      lueBank.Properties.DisplayMember = "Bank"
      lueBank.Properties.ValueMember = "ID"

      gvBank.OptionsView.ShowIndicator = False
      gvBank.OptionsView.ShowColumnHeaders = True
      gvBank.OptionsView.ShowFooter = False
      gvBank.OptionsView.ShowAutoFilterRow = True
      gvBank.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

      gvBank.Columns.Clear()

			Dim zgBankColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			zgBankColumn.Caption = m_Translate.GetSafeTranslationValue("Vorschuss")
			zgBankColumn.Name = "BankZG"
			zgBankColumn.FieldName = "BankZG"
			zgBankColumn.Visible = True
			zgBankColumn.ColumnEdit = m_CheckEditDedfaultBank
			gvBank.Columns.Add(zgBankColumn)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
			activeColumn.Name = "ActiveRec"
			activeColumn.FieldName = "ActiveRec"
			activeColumn.Visible = True
			activeColumn.ColumnEdit = m_CheckEditDedfaultBank
			gvBank.Columns.Add(activeColumn)

			Dim BankAUColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			BankAUColumn.Caption = m_Translate.GetSafeTranslationValue("Ausland")
			BankAUColumn.Name = "BankAU"
			BankAUColumn.FieldName = "BankAU"
			BankAUColumn.Visible = True
			BankAUColumn.ColumnEdit = m_CheckEditDedfaultBank
			gvBank.Columns.Add(BankAUColumn)

			Dim columnAdditionalText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdditionalText.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnAdditionalText.Name = "Bank"
			columnAdditionalText.FieldName = "Bank"
			columnAdditionalText.Visible = True
			columnAdditionalText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnAdditionalText)

			Dim columnIban As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIban.Caption = m_Translate.GetSafeTranslationValue("IBAN")
			columnIban.Name = "IBANNr"
			columnIban.FieldName = "IBANNr"
			columnIban.Visible = True
			columnIban.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnIban)

			Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
			columnSwift.Name = "Swift"
			columnSwift.FieldName = "Swift"
			columnSwift.Visible = True
			columnSwift.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnSwift)

			Dim columnAccountNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAccountNr.Caption = m_Translate.GetSafeTranslationValue("Konto-Nr.")
			columnAccountNr.Name = "AccountNr"
			columnAccountNr.FieldName = "AccountNr"
			columnAccountNr.Visible = True
			columnAccountNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvBank.Columns.Add(columnAccountNr)

      lueBank.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueBank.Properties.NullText = String.Empty
      lueBank.EditValue = Nothing
    End Sub

    ''' <summary>
    ''' Resets the Bank detail grid.
    ''' </summary>
    Private Sub ResetBankDetailGrid()

      gvBankDetail.BorderStyle = BorderStyles.NoBorder
      gvBankDetail.OptionsView.ShowIndicator = False

      gvBankDetail.OptionsView.ShowColumnHeaders = False
      gvBankDetail.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False
      gvBankDetail.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False
      gvBankDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
      gvBankDetail.OptionsSelection.EnableAppearanceFocusedRow = False
      gvBankDetail.OptionsSelection.EnableAppearanceHideSelection = False
      gvBankDetail.OptionsView.ColumnAutoWidth = False

      ' Reset the grid
      gvBankDetail.Columns.Clear()

      Dim columnDescriptionName As New DevExpress.XtraGrid.Columns.GridColumn()
      columnDescriptionName.Caption = "Description"
      columnDescriptionName.Name = "Description"
      columnDescriptionName.FieldName = "Description"
      columnDescriptionName.Visible = True
      columnDescriptionName.Width = 120
      gvBankDetail.Columns.Add(columnDescriptionName)

      Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
      columnValue.Caption = "Value"
      columnValue.Name = "Value"
      columnValue.FieldName = "Value"
      columnValue.Visible = True
      columnValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
      columnValue.AppearanceHeader.Options.UseTextOptions = True
      columnValue.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
      columnValue.AppearanceCell.Options.UseTextOptions = True
      columnValue.Width = 300
      gvBankDetail.Columns.Add(columnValue)

    End Sub


    ''' <summary>
    ''' Loads the data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadData() As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadBankDropDownData()
      success = success AndAlso LoadBankData()

      Return success

    End Function

    ''' <summary>
    ''' Loads the bank drop down data.
    ''' </summary>
    Private Function LoadBankDropDownData() As Boolean

      Dim employeeNumber = m_UCMediator.ZGData.MANR

      m_BankData = m_EmployeeDataAccess.LoadEmployeeBanks(employeeNumber)

      If (m_BankData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht geladen werden."))
      End If

			lueBank.Visible = m_BankData.Count > 1 AndAlso Not lueBank.Properties.ReadOnly
			lblBankdaten.Visible = m_BankData.Count > 1 AndAlso Not lueBank.Properties.ReadOnly

			lueBank.EditValue = Nothing
			lueBank.Properties.NullText = m_Translate.GetSafeTranslationValue("Wählen Sie eine Bankverbindung aus") & "..."
      lueBank.Properties.DataSource = m_BankData

      Return m_BankData IsNot Nothing
    End Function

    ''' <summary>
    ''' Loads the bank data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadBankData() As Boolean

      Dim zgData = m_UCMediator.ZGData

      Dim bankDetailData As BankDetailViewData = Nothing
      bankDetailData = BankDetailViewData.FromZG(zgData)

      SetBankDetails(bankDetailData)

      Return True
    End Function

#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Handles button click on bank.
    ''' </summary>
    Private Sub OnLueBank_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles lueBank.ButtonClick

      If (e.Button.Index = 2) Then

        OpenEmployeeBank(False)

      End If

    End Sub

    ''' <summary>
    ''' Handles change of lue bank.
    ''' </summary>
    Private Sub OnLueBank_EditValueChanged(sender As Object, e As EventArgs) Handles lueBank.EditValueChanged

      If (m_SuppressUIEvents) Then
        Return
      End If

      Dim selectedBnk = SelectedBank
      Dim bankDetailData As BankDetailViewData = Nothing

      If Not selectedBnk Is Nothing Then
        bankDetailData = BankDetailViewData.FromMABank(selectedBnk)
      End If

      SetBankDetails(bankDetailData)

    End Sub

    ''' <summary>
    ''' Handles click on add bank button.
    ''' </summary>
    Private Sub OnBtnAddBank_Click(sender As Object, e As EventArgs) Handles btnAddBank.Click
      OpenEmployeeBank(True)
    End Sub

		''' <summary>
		''' Handles click on add bank button.
		''' </summary>
		Private Sub OnBtnShowBank_Click(sender As Object, e As EventArgs) Handles btnShowBank.Click
			OpenEmployeeBank(False)
		End Sub

    ''' <summary>
    ''' Handles drop down button clicks.
    ''' </summary>
    Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

      Const ID_OF_DELETE_BUTTON As Int32 = 1

      ' If delete button has been clicked reset the drop down.
      If e.Button.Index = ID_OF_DELETE_BUTTON Then

        If TypeOf sender Is BaseEdit Then
          If CType(sender, BaseEdit).Properties.ReadOnly Then
            ' nothing
          Else
            CType(sender, BaseEdit).EditValue = Nothing
          End If
        End If

      End If
    End Sub

#End Region

#Region "Helper Methods"

    ''' <summary>
    ''' Sets the bank details.
    ''' </summary>
    ''' <param name="bankDetail">The bank detail data.</param>
    Private Sub SetBankDetails(ByVal bankDetail As BankDetailViewData)

      Dim listOfDetailData = New List(Of BankDetailPropertyViewData)

      Dim bankNameDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Name")}
      Dim bankLocationDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Ort")}
      Dim bankClearingNumberDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Clearing Nummer")}
      Dim bankAccountNumberDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Kontonummer")}
      Dim bankIBANNumberDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("IBAN")}
			Dim bankSwiftNumberDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Swift")}
			Dim bankForeignDetailData = New BankDetailPropertyViewData With {.Description = m_Translate.GetSafeTranslationValue("Im Ausland")}

			If bankDetail IsNot Nothing AndAlso bankDetail.Bank IsNot Nothing Then

				bankNameDetailData.Value = bankDetail.Bank
				bankLocationDetailData.Value = bankDetail.BankOrt
				bankClearingNumberDetailData.Value = bankDetail.ClearingNr
				bankAccountNumberDetailData.Value = bankDetail.KontoNr
				bankIBANNumberDetailData.Value = bankDetail.IBANNr
				bankSwiftNumberDetailData.Value = bankDetail.swift
				bankForeignDetailData.Value = If(bankDetail.BnkAU.HasValue AndAlso bankDetail.BnkAU, m_Translate.GetSafeTranslationValue("Ja"), m_Translate.GetSafeTranslationValue("Nein"))
			End If

      listOfDetailData.Add(bankNameDetailData)
      listOfDetailData.Add(bankLocationDetailData)
      listOfDetailData.Add(bankClearingNumberDetailData)
      listOfDetailData.Add(bankAccountNumberDetailData)
      listOfDetailData.Add(bankIBANNumberDetailData)
			listOfDetailData.Add(bankSwiftNumberDetailData)
			listOfDetailData.Add(bankForeignDetailData)

			grdBank.DataSource = listOfDetailData

    End Sub

    ''' <summary>
    ''' Shows the bank management form.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="bankRecordNumber">The bank record number.</param>
    Private Sub ShowBankDetailForm(ByVal employeeNumber As Integer, ByVal bankRecordNumber As Integer?)

      m_BankDetailForm = New frmBank(m_InitializationData)

      If (bankRecordNumber.HasValue) Then
        m_BankDetailForm.LoadBankData(employeeNumber, bankRecordNumber)
      Else
        m_BankDetailForm.NewBank(employeeNumber)
      End If

      m_BankDetailForm.ShowDialog()

    End Sub

    ''' <summary>
    ''' Opens the employee bank.
    ''' </summary>
    Private Sub OpenEmployeeBank(ByVal enterNewBank As Boolean)

      If Not IsAdvancePaymentDataLoaded Then
        Return
      End If

      Dim employeeNumber = m_UCMediator.ZGData.MANR
      Dim bank = SelectedBank
      Dim selectedBankRecordNumber As Integer? = Nothing

      If Not bank Is Nothing AndAlso Not enterNewBank Then
        selectedBankRecordNumber = bank.RecordNumber
      End If

      ShowBankDetailForm(employeeNumber, selectedBankRecordNumber)

      LoadBankDropDownData()

      ' Reselect bank than was previously selected.
      If selectedBankRecordNumber.HasValue Then

        Dim bankToReSelect = m_BankData.Where(Function(data) data.RecordNumber.HasValue AndAlso data.RecordNumber = selectedBankRecordNumber).FirstOrDefault()

        If Not bankToReSelect Is Nothing Then
					lueBank.EditValue = bankToReSelect.ID
        End If

      End If

    End Sub

#End Region

#Region "View helper classes"

    ''' <summary>
    ''' Bank detail data.
    ''' </summary>
    Class BankDetailViewData

      Public Property Bank As String
      Public Property BankOrt As String
      Public Property ClearingNr As Integer?
      Public Property KontoNr As String
      Public Property IBANNr As String
			Public Property Swift As String
			Public Property BnkAU As Boolean?

			Public Shared Function FromZG(ByVal zgMasterData As ZGMasterData)

				Dim bankDetailData = New BankDetailViewData With {
					.Bank = zgMasterData.Bank,
					.BankOrt = zgMasterData.BankOrt,
					.ClearingNr = zgMasterData.ClearingNr,
					.KontoNr = zgMasterData.KontoNr,
					.IBANNr = zgMasterData.IBANNr,
					.Swift = zgMasterData.Swift,
					.BnkAU = zgMasterData.BnkAU
				}

				Return bankDetailData
      End Function

      Public Shared Function FromMABank(ByVal maBankData As EmployeeBankData)

				Dim bankDetailData = New BankDetailViewData With {
					.Bank = maBankData.Bank,
					.BankOrt = maBankData.BankLocation,
					.ClearingNr = maBankData.DTABCNR,
					.KontoNr = maBankData.AccountNr,
					.IBANNr = maBankData.IBANNr,
					.Swift = maBankData.Swift,
					.BnkAU = maBankData.BankAU
				}

				Return bankDetailData
      End Function

    End Class

    ''' <summary>
    ''' Bankdetail list item view data.
    ''' </summary>
    Class BankDetailPropertyViewData
      Public Property Description As String
      Public Property Value As String
    End Class

#End Region

  End Class

End Namespace
