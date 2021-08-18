'Imports SP.MA.MLohnMng
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Employee
Imports SP.MA.AdvancePaymentMng.Settings
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee.DataObjects.AdvancedPaymentMng

Namespace UI
  Public Class ucAdvancePaymentslist

#Region "Private Fields"

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

    ''' <summary>
    ''' The employee number.
    ''' </summary>
    Private m_EmployeeNumber As Integer? = Nothing

    Private m_Month As Integer? = Nothing
    Private m_Year As Integer? = Nothing

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
    ''' Activates the control.
    ''' </summary>
    Public Overrides Function Activate() As Boolean

      m_SuppressUIEvents = True

      Dim success As Boolean = True

      If m_UCMediator.ZGData Is Nothing Then
        Return False
      End If

      If Not m_IsIntialControlDataLoaded Then
        Dim settting_filter_onlyCurrentYear As Boolean = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_ADVANCEPAYMENT_FILTER_ONLY_CURRENT_YEAR)
        chkThisYear.Checked = settting_filter_onlyCurrentYear

        m_IsIntialControlDataLoaded = True
      End If

      Dim zgNrToLoad = m_UCMediator.ZGData.ZGNr

      If (Not IsAdvancePaymentDataLoaded OrElse (Not m_ZGNumber = zgNrToLoad)) Then
        CleanUp()

        Dim employeeNumberToLoad = m_UCMediator.ZGData.MANR
        Dim monthToLoad As Integer = m_UCMediator.ZGData.LP
        Dim yearToLoad As Integer = Convert.ToInt32(m_UCMediator.ZGData.JAHR)
        m_Month = IIf(success, monthToLoad, 0)
        m_Year = IIf(success, yearToLoad, 0)
        m_EmployeeNumber = IIf(success, employeeNumberToLoad, 0)
        m_ZGNumber = IIf(success, zgNrToLoad, 0)

        success = success AndAlso LoadAdvancePaymentList(chkThisYear.Checked)

      End If

      FocusEmployeeAdvancePaymentData(zgNrToLoad)

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

      Dim previousState = SetSuppressUIEventsState(True)

      m_ZGNumber = Nothing
      m_EmployeeNumber = Nothing

      ResetAdvancePaymentGrid()

      SetSuppressUIEventsState(previousState)

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean
      ' Do nothing
      Return True
    End Function

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()

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
    ''' Resets the advance payment overview grid.
    ''' </summary>
    Private Sub ResetAdvancePaymentGrid()

      ' Reset the grid
      gvAdvancePayment.OptionsView.ShowIndicator = False
      gvAdvancePayment.OptionsView.ShowAutoFilterRow = True
      gvAdvancePayment.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
      gvAdvancePayment.OptionsView.ShowFooter = True
      gvAdvancePayment.Columns.Clear()

      Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnLANr.Caption = m_Translate.GetSafeTranslationValue("Vorschuss-Nr.")
      columnLANr.Name = "paymentnumber"
      columnLANr.FieldName = "paymentnumber"
      columnLANr.Visible = True
      columnLANr.Width = 35
      'columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      'columnLANr.DisplayFormat.FormatString = "0.###"
      gvAdvancePayment.Columns.Add(columnLANr)

      Dim columnLAName As New DevExpress.XtraGrid.Columns.GridColumn()
      columnLAName.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
      columnLAName.Name = "translatedLAname"
      columnLAName.FieldName = "translatedLAname"
      columnLAName.Visible = True
      columnLAName.Width = 235
      gvAdvancePayment.Columns.Add(columnLAName)

      Dim columnFromDate As New DevExpress.XtraGrid.Columns.GridColumn()
      columnFromDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
      columnFromDate.Name = "paymentdate"
      columnFromDate.FieldName = "paymentdate"
      columnFromDate.Visible = True
      columnFromDate.Width = 70
      gvAdvancePayment.Columns.Add(columnFromDate)

      Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMonth.Caption = m_Translate.GetSafeTranslationValue("Monat")
      columnMonth.Name = "paymentmonth"
      columnMonth.FieldName = "paymentmonth"
      columnMonth.Visible = True
      columnMonth.Width = 70
      gvAdvancePayment.Columns.Add(columnMonth)

      Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
      columnYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
      columnYear.Name = "paymentyear"
      columnYear.FieldName = "paymentyear"
      columnYear.Visible = True
      columnYear.Width = 70
      gvAdvancePayment.Columns.Add(columnYear)

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
      columnAmount.SummaryItem.DisplayFormat = "{0:n2}"
      columnAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
      gvAdvancePayment.Columns.Add(columnAmount)

      Dim columnReason As New DevExpress.XtraGrid.Columns.GridColumn()
      columnReason.Caption = m_Translate.GetSafeTranslationValue("Grund")
      columnReason.Name = "paymentreason"
      columnReason.FieldName = "paymentreason"
      columnReason.Visible = True
      columnReason.Width = 70
      gvAdvancePayment.Columns.Add(columnReason)

      Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
      columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
      columnCreatedOn.Name = "createdon"
      columnCreatedOn.FieldName = "createdon"
      columnCreatedOn.Visible = True
      columnCreatedOn.Width = 70
      gvAdvancePayment.Columns.Add(columnCreatedOn)

      Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
      columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
      columnCreatedFrom.Name = "createdfrom"
      columnCreatedFrom.FieldName = "createdfrom"
      columnCreatedFrom.Visible = True
      columnCreatedFrom.Width = 70
      gvAdvancePayment.Columns.Add(columnCreatedFrom)

    End Sub

    ''' <summary>
    ''' Loads the advance payment overview list.
    ''' </summary>
    ''' <param name="onlyCurrentYear">Boolean value indicating if only data of current year should be shown.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadAdvancePaymentList(ByVal onlyCurrentYear As Boolean) As Boolean
      Dim langUser As String = m_InitializationData.UserData.UserLanguage

      Dim listOfPayments = m_EmployeeDataAccess.LoadEmployeeAdvancedPaymentSettings(m_EmployeeNumber, m_Month, langUser, m_InitializationData.MDData.MDNr, onlyCurrentYear)

      If listOfPayments Is Nothing Then

        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("(Auszahlungen (Vorschüsse) konnten nicht geladen werden."))

        Return False

      End If

      Dim paymentGridData = (From payment In listOfPayments
      Select New EmployeeAdvancedPaymentData With
             {.ID = payment.ID,
              .paymentnumber = payment.paymentnumber,
              .reportnumber = payment.reportnumber,
              .employeenumber = payment.employeenumber,
              .paymentdate = payment.paymentdate,
              .paymentmonth = payment.paymentmonth,
              .paymentyear = payment.paymentyear,
              .Amount = payment.Amount,
              .createdon = payment.createdon,
              .createdfrom = payment.createdfrom,
              .paymentreason = payment.paymentreason,
              .translatedLAname = payment.translatedLAname
             }).ToList()

      Dim listDataSource As BindingList(Of EmployeeAdvancedPaymentData) = New BindingList(Of EmployeeAdvancedPaymentData)

      For Each p In paymentGridData
        listDataSource.Add(p)
      Next

      Dim suppressUIEventsState = m_SuppressUIEvents
      m_SuppressUIEvents = True

      grdAdvancePayment.DataSource = listDataSource
      m_SuppressUIEvents = suppressUIEventsState

      Return True
    End Function

    ''' <summary>
    ''' Handles double click on advance payment.
    ''' </summary>
    Private Sub OnGvAdvancePayment_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gvAdvancePayment.DoubleClick
      Dim selectedRows = gvAdvancePayment.GetSelectedRows()

      If (selectedRows.Count > 0) Then

        If (gvAdvancePayment.GetRow(selectedRows(0)) Is Nothing) Then Return

        Dim advancePaymentData = CType(gvAdvancePayment.GetRow(selectedRows(0)), EmployeeAdvancedPaymentData)
        m_UCMediator.LoadAdvancePaymentData(advancePaymentData.paymentnumber)
      End If
    End Sub

    ''' <summary>
    ''' Handles check changed event on chkThisYear.
    ''' </summary>
    Private Sub OnChkThisYear_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkThisYear.CheckedChanged

      If m_SuppressUIEvents Then
        Return
      End If
      Dim zgData = m_UCMediator.ZGData
      If zgData Is Nothing Then
        Return
      End If

      If m_EmployeeNumber.HasValue Then

        LoadAdvancePaymentList(chkThisYear.Checked)
        FocusEmployeeAdvancePaymentData(zgData.ZGNr)

        Try
          m_SettingsManager.WriteBoolean(SettingKeys.SETTING_ADVANCEPAYMENT_FILTER_ONLY_CURRENT_YEAR, chkThisYear.Checked)
        Catch ex As Exception
          m_Logger.LogError(ex.ToString)
        End Try

      End If

    End Sub

    ''' <summary>
    ''' Focuses employee advance payment data.
    ''' </summary>
    ''' <param name="zgNr">The ZGNr.</param>
    Private Sub FocusEmployeeAdvancePaymentData(ByVal zgNr As Integer)

      If Not grdAdvancePayment.DataSource Is Nothing Then

        Dim employeeAdvancePaymentData = CType(grdAdvancePayment.DataSource, BindingList(Of EmployeeAdvancedPaymentData))

        Dim index = employeeAdvancePaymentData.ToList().FindIndex(Function(data) data.paymentnumber = zgNr)

        Dim previousState = SetSuppressUIEventsState(True)
        Dim rowHandle = gvAdvancePayment.GetRowHandle(index)
        gvAdvancePayment.FocusedRowHandle = rowHandle
        previousState = SetSuppressUIEventsState(previousState)
      End If

    End Sub

#End Region

  End Class


End Namespace
