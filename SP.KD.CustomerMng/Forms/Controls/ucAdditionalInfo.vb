
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports SP.KD.BAInfo

Imports SPProgUtility.SPUserSec.ClsUserSec


Namespace UI

  ''' <summary>
  ''' Additional info data.
  ''' </summary>
  Public Class ucAdditionalInfo

#Region "Private Fields"
    ''' <summary>
    ''' Check edit for active symbol.
    ''' </summary>
    Private m_CheckEditActive As RepositoryItemCheckEdit

    ''' <summary>
    ''' Credit info detail form.
    ''' </summary>
    Private m_CreditInfoDetailForm As frmBAInfo

    Private m_IsAuthorizedForCode_210 As Boolean = False
    Private m_IsAuthorizedForCode_211 As Boolean = False

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      lueNumberOfEmployees.Properties.ShowHeader = False
      lueNumberOfEmployees.Properties.ShowFooter = False
      lueNumberOfEmployees.Properties.DropDownRows = 10

      lueReserve1.Properties.ShowHeader = False
      lueReserve1.Properties.ShowFooter = False
      lueReserve1.Properties.DropDownRows = 10

      lueReserve2.Properties.ShowHeader = False
      lueReserve2.Properties.ShowFooter = False
      lueReserve2.Properties.DropDownRows = 10

      lueReserve3.Properties.ShowHeader = False
      lueReserve3.Properties.ShowFooter = False
      lueReserve3.Properties.DropDownRows = 10

      lueReserve4.Properties.ShowHeader = False
      lueReserve4.Properties.ShowFooter = False
      lueReserve4.Properties.DropDownRows = 10

      gvCreditInfo.OptionsView.ShowIndicator = False

      ' Important symbol.
      m_CheckEditActive = CType(gridCreditInfo.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
      m_CheckEditActive.PictureChecked = My.Resources.Completed
      m_CheckEditActive.PictureUnchecked = Nothing
      m_CheckEditActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

      AddHandler dateEditFrom.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler dateEditTo.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueNumberOfEmployees.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve1.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve2.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve3.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler lueReserve4.ButtonClick, AddressOf OnDropDown_ButtonClick

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected credit info view data.
    ''' </summary>
    ''' <returns>The selected credit info or nothing if none is selected.</returns>
    Public ReadOnly Property SelectedCreditInfoViewData As CreditInfoViewData
      Get

        If Not IsCustomerDataLoaded Then
          Return Nothing
        End If

        Dim grdView = TryCast(gridCreditInfo.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

        If Not (grdView Is Nothing) Then

          Dim selectedRows = grdView.GetSelectedRows()

          If (selectedRows.Count > 0) Then
            Dim document = CType(grdView.GetRow(selectedRows(0)), CreditInfoViewData)
            Return document
          End If

        End If

        Return Nothing
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

      LoadUserRights()
    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Overrides Function Activate(ByVal customerNumber As Integer?) As Boolean

      Dim success As Boolean = True

      If (Not IsIntialControlDataLoaded) Then
        success = success AndAlso LoadDropDownData()
        IsIntialControlDataLoaded = True
      End If

      If (customerNumber.HasValue) Then
        If (Not IsCustomerDataLoaded) Then
          success = success AndAlso LoadCustomerData(customerNumber)
        ElseIf Not customerNumber = m_CustomerNumber Then
          success = success AndAlso LoadCustomerData(customerNumber)
        End If
      Else
        Reset()
      End If

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

      m_CustomerNumber = Nothing

      ' Resets textboxes, etc.

      spinCreditLimit1.Value = 0
      spinCreditLimit1.Properties.MaxValue = 999999999999
      spinCreditLimit1.Properties.MinValue = -999999999999

      spinCreditLimit2.Value = 0
      spinCreditLimit2.Properties.MaxValue = 999999999999
      spinCreditLimit2.Properties.MinValue = -999999999999

      dateEditFrom.EditValue = Nothing
      dateEditTo.EditValue = Nothing

      txtReferenceNumber.Text = String.Empty
      txtReferenceNumber.Properties.MaxLength = 70

      spinUmsMin.Properties.MinValue = 0
      spinUmsMin.Properties.MaxValue = 100

      chkCreditWarning.Checked = False

      chkCanteenAvailable.Checked = False
      chkTransportOptions.Checked = False

      txtSalaryPerMonth.Text = String.Empty
      txtSalaryPerMonth.Properties.MaxLength = 15
      txtSalaryPerHour.Text = String.Empty
      txtSalaryPerHour.Properties.MaxLength = 15
      chkNoPrintReports.Checked = False

      txtReserve5.Text = String.Empty

      ' Reset drop downs and grids

      ResetNumberOfEmployeestDropDown()
      ResetReserveDropDown(lueReserve1)
      ResetReserveDropDown(lueReserve2)
      ResetReserveDropDown(lueReserve3)
      ResetReserveDropDown(lueReserve4)

      ResetCreditInfoGrid()

      grpbetreibung.Enabled = m_IsAuthorizedForCode_211
      btnAddCreditInfo.Enabled = grpbetreibung.Enabled
      grpkreditlimite.Enabled = m_IsAuthorizedForCode_210

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean
      ' Do nothing
      Return True
    End Function

    ''' <summary>
    ''' Merges the custmer master data.
    ''' </summary>
    ''' <param name="customerMasterData">The customer master data object where the data gets filled into.</param>
    Public Overrides Sub MergeCustomerMasterData(ByVal customerMasterData As CustomerMasterData)
      If (IsCustomerDataLoaded AndAlso
          m_CustomerNumber = customerMasterData.CustomerNumber) Then

        customerMasterData.CreditLimit1 = spinCreditLimit1.Value
        customerMasterData.CreditLimit2 = spinCreditLimit2.Value

        customerMasterData.CreditLimitsFromDate = dateEditFrom.EditValue
        customerMasterData.CreditLimitsToDate = dateEditTo.EditValue

        customerMasterData.ReferenceNumber = txtReferenceNumber.Text
        customerMasterData.KD_UmsMin = spinUmsMin.Value

        customerMasterData.CreditWarning = chkCreditWarning.Checked

        customerMasterData.NumberOfEmployees = lueNumberOfEmployees.EditValue
        customerMasterData.CanteenAvailable = chkCanteenAvailable.Checked
        customerMasterData.TransportationOptions = chkTransportOptions.Checked

        If Not String.IsNullOrEmpty(txtSalaryPerMonth.Text) Then
          customerMasterData.SalaryPerMonth = Decimal.Parse(txtSalaryPerMonth.Text)
        Else
          customerMasterData.SalaryPerMonth = Nothing
        End If

        If Not String.IsNullOrEmpty(txtSalaryPerMonth.Text) Then
          customerMasterData.SalaryPerHour = Decimal.Parse(txtSalaryPerHour.Text)
        Else
          customerMasterData.SalaryPerHour = Nothing
        End If
        customerMasterData.NotPrintReports = chkNoPrintReports.Checked

        customerMasterData.Reserve1 = lueReserve1.EditValue
        customerMasterData.Reserve2 = lueReserve2.EditValue
        customerMasterData.Reserve3 = lueReserve3.EditValue
        customerMasterData.Reserve4 = lueReserve4.EditValue

      End If
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()

      If Not m_CreditInfoDetailForm Is Nothing AndAlso
         Not m_CreditInfoDetailForm.IsDisposed Then

        Try
          m_CreditInfoDetailForm.Close()
          m_CreditInfoDetailForm.Dispose()
        Catch
          ' Do nothing
        End Try
      End If

    End Sub

    ''' <summary>
    ''' Reloads credit infos.
    ''' </summary>
    Public Sub ReloadCreditInfos()

      If IsCustomerDataLoaded Then

        Dim selectedCreditInfoBeforeReload = SelectedCreditInfoViewData

        ' Reload credit infos
        LoadAssignedCustomerCreditInfo(m_CustomerNumber)

        ' Select the previously selected record
        If Not selectedCreditInfoBeforeReload Is Nothing Then
          FocusCreditInfo(m_CustomerNumber, selectedCreditInfoBeforeReload.RecNumber)
        End If

        If Not m_CreditInfoDetailForm Is Nothing AndAlso
            Not m_CreditInfoDetailForm.IsDisposed Then

          ' Also reload credit infos in open credit info form
          m_CreditInfoDetailForm.ReloadCreditInfos()

        End If

      End If
    End Sub

#End Region

#Region "Pirvate Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.grpbetreibung.Text = m_Translate.GetSafeTranslationValue(Me.grpbetreibung.Text)

      Me.grpkreditlimite.Text = m_Translate.GetSafeTranslationValue(Me.grpkreditlimite.Text)

      Me.lbl1Kreditlimite.Text = m_Translate.GetSafeTranslationValue(Me.lbl1Kreditlimite.Text)
      Me.lbl1kreditgueltig.Text = m_Translate.GetSafeTranslationValue(Me.lbl1kreditgueltig.Text)
      Me.lbl2kreditlimite.Text = m_Translate.GetSafeTranslationValue(Me.lbl2kreditlimite.Text)
      Me.lblreferenznummer.Text = m_Translate.GetSafeTranslationValue(Me.lblreferenznummer.Text)
      Me.lblverguetung.Text = m_Translate.GetSafeTranslationValue(Me.lblverguetung.Text)
      Me.chkCreditWarning.Text = m_Translate.GetSafeTranslationValue(Me.chkCreditWarning.Text)

      Me.grpinfoverleih.Text = m_Translate.GetSafeTranslationValue(Me.grpinfoverleih.Text)
      Me.lblbetriebsgroesse.Text = m_Translate.GetSafeTranslationValue(Me.lblbetriebsgroesse.Text)
      Me.chkTransportOptions.Text = m_Translate.GetSafeTranslationValue(Me.chkTransportOptions.Text)
      Me.chkCanteenAvailable.Text = m_Translate.GetSafeTranslationValue(Me.chkCanteenAvailable.Text)
      Me.lbltraifmonat.Text = m_Translate.GetSafeTranslationValue(Me.lbltraifmonat.Text)
      Me.lbltarifstunde.Text = m_Translate.GetSafeTranslationValue(Me.lbltarifstunde.Text)
      Me.chkNoPrintReports.Text = m_Translate.GetSafeTranslationValue(Me.chkNoPrintReports.Text)

      Me.grpreservefelder.Text = m_Translate.GetSafeTranslationValue(Me.grpreservefelder.Text, True)
      Me.lbl1reserve.Text = m_Translate.GetSafeTranslationValue(Me.lbl1reserve.Text, True)
      Me.lbl2reserve.Text = m_Translate.GetSafeTranslationValue(Me.lbl2reserve.Text, True)
      Me.lbl3reserve.Text = m_Translate.GetSafeTranslationValue(Me.lbl3reserve.Text, True)
      Me.lbl4reserve.Text = m_Translate.GetSafeTranslationValue(Me.lbl4reserve.Text, True)
      Me.lbl5reserve.Text = m_Translate.GetSafeTranslationValue(Me.lbl5reserve.Text, True)

    End Sub


    ''' <summary>
    ''' Loads customer data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadCustomerData(ByVal customerNumber As Integer) As Boolean

      Dim success As Boolean = True

      success = LoadCustomerMasterData(customerNumber)
      success = success AndAlso LoadAssignedCustomerCreditInfo(customerNumber)

      m_CustomerNumber = IIf(success, customerNumber, Nothing)

      Return success

    End Function

    ''' <summary>
    ''' Loads user rights.
    ''' </summary>
    Private Sub LoadUserRights()
			m_IsAuthorizedForCode_210 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 210, m_InitializationData.MDData.MDNr)
			m_IsAuthorizedForCode_211 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 211, m_InitializationData.MDData.MDNr)
    End Sub

    ''' <summary>
    ''' Loads the drop down data.
    ''' </summary>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadDropDownData() As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadNumberOfEmployeestDropDownData()
      success = success AndAlso LoadReserveDropDownData(CustomerReserveDataType.Reserve1, lueReserve1)
      success = success AndAlso LoadReserveDropDownData(CustomerReserveDataType.Reserve2, lueReserve2)
      success = success AndAlso LoadReserveDropDownData(CustomerReserveDataType.Reserve3, lueReserve3)
      success = success AndAlso LoadReserveDropDownData(CustomerReserveDataType.Reserve4, lueReserve4)

      Return success
    End Function

    ''' <summary>
    ''' Loads the number of employees drop down data.
    ''' </summary>
    Private Function LoadNumberOfEmployeestDropDownData() As Boolean
      Dim numberOfEmployeesData = m_DataAccess.LoadNumberOfEmployeesData()

      If (numberOfEmployeesData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Anzahl Mitarbeiter-Auswahldaten konnten nicht geladen werden."))
      End If

      lueNumberOfEmployees.Properties.DataSource = numberOfEmployeesData
      lueNumberOfEmployees.Properties.ForceInitialize()

      Return Not numberOfEmployeesData Is Nothing
    End Function

    ''' <summary>
    ''' Loads reserve drop down data.
    ''' </summary>
    ''' <param name="reserveTyp">The reserve type.</param>
    ''' <param name="lueEdit">The lookup edit.</param>
    Private Function LoadReserveDropDownData(ByVal reserveTyp As CustomerReserveDataType, ByVal lueEdit As LookUpEdit) As Boolean
      Dim resesrveData = m_DataAccess.LoadCustomerReserveData(reserveTyp)

      If (resesrveData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Reservedaten{0} konnten nicht geladen werden."), CType(reserveTyp, Integer)))
      End If

      lueEdit.Properties.DataSource = resesrveData
      lueEdit.Properties.ForceInitialize()

      Return Not resesrveData Is Nothing
    End Function

    ''' <summary>
    ''' Resets the OP shipment drop down.
    ''' </summary>
    Private Sub ResetNumberOfEmployeestDropDown()

      lueNumberOfEmployees.Properties.DisplayMember = "NumberOfEmployees"
      lueNumberOfEmployees.Properties.ValueMember = "NumberOfEmployees"

      Dim columns = lueNumberOfEmployees.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("NumberOfEmployees", 0))

      lueNumberOfEmployees.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueNumberOfEmployees.Properties.SearchMode = SearchMode.AutoComplete
      lueNumberOfEmployees.Properties.AutoSearchColumnIndex = 0

      lueNumberOfEmployees.Properties.NullText = String.Empty
      lueNumberOfEmployees.EditValue = Nothing

    End Sub

    ''' <summary>
    ''' Resets the reserve drop down.
    ''' </summary>
    Private Sub ResetReserveDropDown(ByVal lueReserve As LookUpEdit)

      lueReserve.Properties.DisplayMember = "Description"
      lueReserve.Properties.ValueMember = "Description"

      Dim columns = lueReserve.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("Description", 0))

      lueReserve.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueReserve.Properties.SearchMode = SearchMode.AutoComplete
      lueReserve.Properties.AutoSearchColumnIndex = 0

      lueReserve.Properties.NullText = String.Empty
      lueReserve.EditValue = Nothing

    End Sub


    ''' <summary>
    ''' Resets the credit info grid.
    ''' </summary>
    Private Sub ResetCreditInfoGrid()

      ' Reset the grid
      gvCreditInfo.Columns.Clear()

      Dim columnFromDate As New DevExpress.XtraGrid.Columns.GridColumn()
      columnFromDate.Caption = m_Translate.GetSafeTranslationValue("Ab")
      columnFromDate.Name = "FromDate"
      columnFromDate.FieldName = "FromDate"
      columnFromDate.Visible = True
      gvCreditInfo.Columns.Add(columnFromDate)

      Dim columnToDate As New DevExpress.XtraGrid.Columns.GridColumn()
      columnToDate.Caption = m_Translate.GetSafeTranslationValue("Bis")
      columnToDate.Name = "ToDate"
      columnToDate.FieldName = "ToDate"
      columnToDate.Visible = False
      gvCreditInfo.Columns.Add(columnFromDate)

      Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
      columnDescription.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
      columnDescription.Name = "Description"
      columnDescription.FieldName = "Description"
      columnDescription.Visible = True
      gvCreditInfo.Columns.Add(columnDescription)

      Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
      activeColumn.Name = "Active"
      activeColumn.FieldName = "Active"
      activeColumn.Visible = True
      activeColumn.ColumnEdit = m_CheckEditActive
      gvCreditInfo.Columns.Add(activeColumn)

      gridCreditInfo.DataSource = Nothing

    End Sub

    ''' <summary>
    ''' Loads customer master data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadCustomerMasterData(ByVal customerNumber As Integer) As Boolean

      Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_ClsProgSetting.GetUSFiliale)

      If (customerMasterData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
        Return False
      End If

      spinCreditLimit1.Value = customerMasterData.CreditLimit1
      spinCreditLimit2.Value = customerMasterData.CreditLimit2

      dateEditFrom.EditValue = customerMasterData.CreditLimitsFromDate
      dateEditTo.EditValue = customerMasterData.CreditLimitsToDate

      txtReferenceNumber.Text = customerMasterData.ReferenceNumber
      spinUmsMin.Value = customerMasterData.KD_UmsMin

      chkCreditWarning.Checked = customerMasterData.CreditWarning.HasValue AndAlso customerMasterData.CreditWarning.Value = True

      lueNumberOfEmployees.EditValue = customerMasterData.NumberOfEmployees
      chkCanteenAvailable.Checked = customerMasterData.CanteenAvailable.HasValue AndAlso customerMasterData.CanteenAvailable
      chkTransportOptions.Checked = customerMasterData.TransportationOptions.HasValue AndAlso customerMasterData.TransportationOptions

      txtSalaryPerMonth.Text = IIf(customerMasterData.SalaryPerMonth Is Nothing, String.Empty, customerMasterData.SalaryPerMonth)
      txtSalaryPerHour.Text = IIf(customerMasterData.SalaryPerHour Is Nothing, String.Empty, customerMasterData.SalaryPerHour)
      chkNoPrintReports.Checked = customerMasterData.NotPrintReports.HasValue AndAlso customerMasterData.NotPrintReports

      lueReserve1.EditValue = customerMasterData.Reserve1
      lueReserve2.EditValue = customerMasterData.Reserve2
      lueReserve3.EditValue = customerMasterData.Reserve3
      lueReserve4.EditValue = customerMasterData.Reserve4

      Return True
    End Function

    ''' <summary>
    ''' Loads assigned customer creadit info data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadAssignedCustomerCreditInfo(ByVal customerNumber As Integer) As Boolean

      Dim creditInfoData = m_DataAccess.LoadAssignedCreditInfosOfCustomer(customerNumber, Nothing, False)

      If (creditInfoData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Betreibungsauskünfte konnten nicht geladen werden."))
        Return False
      End If

      Dim listDataSource As BindingList(Of CreditInfoViewData) = New BindingList(Of CreditInfoViewData)

      ' Convert the data to view data.
      For Each creditInfoObj In creditInfoData
        Dim openDebitorInvoiceViewData = New CreditInfoViewData() With {
            .CustomerNumber = creditInfoObj.CustomerNumber,
            .RecNumber = creditInfoObj.RecordNumber,
            .FromDate = creditInfoObj.FromDate,
            .ToDate = creditInfoObj.ToDate,
            .Description = creditInfoObj.Description,
            .Active = creditInfoObj.ActiveRec
            }

        listDataSource.Add(openDebitorInvoiceViewData)
      Next

      gridCreditInfo.DataSource = listDataSource

      Return True

    End Function

    ''' <summary>
    ''' Handles double click on documnet.
    ''' </summary>
    Private Sub OnDocument_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvCreditInfo.DoubleClick
      Dim selectedRows = gvCreditInfo.GetSelectedRows()

      If (selectedRows.Count > 0) Then
        Dim creditInfoData = CType(gvCreditInfo.GetRow(selectedRows(0)), CreditInfoViewData)
        ShowCreditInfoDetailForm(m_CustomerNumber, creditInfoData.RecNumber)
      End If
    End Sub

    ''' <summary>
    ''' Handles click on add credit info.
    ''' </summary>
    Private Sub OnBtnAddCreditInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCreditInfo.Click
      If (IsCustomerDataLoaded) Then
        ShowCreditInfoDetailForm(m_CustomerNumber, Nothing)
      End If
    End Sub

    ''' <summary>
    ''' Shows the credit info form.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="creditInfoRecordNumber">The credit info record number to select.</param>
    Private Sub ShowCreditInfoDetailForm(ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer?)

      If m_CreditInfoDetailForm Is Nothing OrElse m_CreditInfoDetailForm.IsDisposed Then

        If Not m_CreditInfoDetailForm Is Nothing Then
          'First cleanup handlers of old form before new form is created.
          RemoveHandler m_CreditInfoDetailForm.FormClosed, AddressOf OnCreditInfoFormClosed
          RemoveHandler m_CreditInfoDetailForm.CreditInfoDataSaved, AddressOf OnCreditInfoFormDocumentDataSaved
          RemoveHandler m_CreditInfoDetailForm.CreditInfoDataDeleted, AddressOf OnCreditInfoFormDocumentDataDeleted
        End If

        m_CreditInfoDetailForm = New frmBAInfo(m_InitializationData)
        AddHandler m_CreditInfoDetailForm.FormClosed, AddressOf OnCreditInfoFormClosed
        AddHandler m_CreditInfoDetailForm.CreditInfoDataSaved, AddressOf OnCreditInfoFormDocumentDataSaved
        AddHandler m_CreditInfoDetailForm.CreditInfoDataDeleted, AddressOf OnCreditInfoFormDocumentDataDeleted
      End If

      m_CreditInfoDetailForm.Show()
      m_CreditInfoDetailForm.LoadCreditInfoData(customerNumber, creditInfoRecordNumber)
      m_CreditInfoDetailForm.BringToFront()

    End Sub

    ''' <summary>
    ''' Handles close of credit info form.
    ''' </summary>
    Private Sub OnCreditInfoFormClosed(sender As System.Object, e As System.EventArgs)
      LoadAssignedCustomerCreditInfo(m_CustomerNumber)

      Dim creditInfoForm = CType(sender, frmBAInfo)

      If Not creditInfoForm.SelectedCreditInfoViewData Is Nothing Then
        FocusCreditInfo(m_CustomerNumber, creditInfoForm.SelectedCreditInfoViewData.RecNr)
      End If

    End Sub

    ''' <summary>
    ''' Handles credit info form data saved.
    ''' </summary>
    Private Sub OnCreditInfoFormDocumentDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer)

      LoadAssignedCustomerCreditInfo(m_CustomerNumber)

      Dim creditInfoForm = CType(sender, frmBAInfo)

      FocusCreditInfo(m_CustomerNumber, creditInfoRecordNumber)

    End Sub

    ''' <summary>
    ''' Handles credit info form data deleted saved.
    ''' </summary>
    Private Sub OnCreditInfoFormDocumentDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer)
      LoadAssignedCustomerCreditInfo(m_CustomerNumber)
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

    ''' <summary>
    ''' Focuses a credit info.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="creditInfoRecordNumber">The credit info record number.</param>
    Private Sub FocusCreditInfo(ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer)

      Dim listDataSource As BindingList(Of CreditInfoViewData) = gridCreditInfo.DataSource

      Dim creditInfoViewData = listDataSource.Where(Function(data) data.CustomerNumber = customerNumber AndAlso data.RecNumber = creditInfoRecordNumber).FirstOrDefault()

      If Not creditInfoViewData Is Nothing Then
        Dim sourceIndex = listDataSource.IndexOf(creditInfoViewData)
        Dim rowHandle = gvCreditInfo.GetRowHandle(sourceIndex)
        gvCreditInfo.FocusedRowHandle = rowHandle
      End If

    End Sub

#End Region

#Region "View helper classes"

    ''' <summary>
    '''  Credit info view data.
    ''' </summary>
    Class CreditInfoViewData
      Public Property CustomerNumber As Integer
      Public Property RecNumber As Integer
      Public Property FromDate As DateTime?
      Public Property ToDate As DateTime?
      Public Property Description As String
      Public Property Active As Boolean?
    End Class

#End Region

  End Class

End Namespace
