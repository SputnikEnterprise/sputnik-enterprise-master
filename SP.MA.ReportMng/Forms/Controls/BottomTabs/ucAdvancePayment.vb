Imports SP.MA.MLohnMng
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Employee
Imports SP.MA.ReportMng.Settings
Imports System.ComponentModel
Imports SP.DatabaseAccess.Employee.DataObjects.AdvancedPaymentMng
Imports SP.MA.AdvancePaymentMng.UI
Imports SPProgUtility.SPUserSec.ClsUserSec


Namespace UI

  Public Class ucAdvancePayment

#Region "Private Fields"

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

    ' ''' <summary>
    ' ''' The advance payment management detail form.
    ' ''' </summary>
		Private m_AdvancePaymentDetailForm As frmNewAdvancePayment

    ''' <summary>
    ''' The employee number.
    ''' </summary>
    Private m_EmployeeNumber As Integer? = Nothing

    Private m_Month As Integer? = Nothing
    Private m_Year As Integer? = Nothing
    Private vb6Object As Object = Nothing

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

      If m_UCMediator.ActiveReportData Is Nothing Then
        Return False
      End If

      If Not m_IsIntialControlDataLoaded Then
        Dim settting_filter_onlyCurrentYear As Boolean = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_ADVANCEPAYMENT_FILTER_ONLY_CURRENT_YEAR)
        chkThisYear.Checked = settting_filter_onlyCurrentYear

        m_IsIntialControlDataLoaded = True
      End If

      Dim rpNrToLoad = m_UCMediator.ActiveReportData.ReportData.RPNR

      If (Not IsReportDataLoaded OrElse (Not m_ReportNumber = rpNrToLoad)) Then
        CleanUp()

        Dim employeeNumberToLoad = m_UCMediator.ActiveReportData.EmployeeOfActiveReport.EmployeeNumber
        Dim monthToLoad As Integer = m_UCMediator.ActiveReportData.ReportData.Monat
        Dim yearToLoad As Integer = m_UCMediator.ActiveReportData.ReportData.Jahr
        m_Month = IIf(success, monthToLoad, 0)
        m_Year = IIf(success, yearToLoad, 0)
        m_EmployeeNumber = IIf(success, employeeNumberToLoad, 0)
        m_ReportNumber = IIf(success, rpNrToLoad, 0)

        success = success AndAlso LoadAdvancePaymentList(chkThisYear.Checked)

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

      Dim previousState = SetSuppressUIEventsState(True)

      m_ReportNumber = Nothing
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

			If Not m_AdvancePaymentDetailForm Is Nothing AndAlso Not m_AdvancePaymentDetailForm.IsDisposed Then

				Try
					vb6Object = Nothing
					m_AdvancePaymentDetailForm.Dispose()
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

			chkThisYear.Text = m_Translate.GetSafeTranslationValue(chkThisYear.Text)
			grpAdvancePayment.Text = m_Translate.GetSafeTranslationValue(grpAdvancePayment.Text)

		End Sub

    ''' <summary>
    ''' Resets the advance payment overview grid.
    ''' </summary>
    Private Sub ResetAdvancePaymentGrid()

      ' Reset the grid
      gvAdvancePayment.OptionsView.ShowIndicator = False
      gvAdvancePayment.OptionsView.ShowAutoFilterRow = True
      gvAdvancePayment.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
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

      Dim paymentGridData = (From report In listOfPayments
      Select New EmployeeAdvancedPaymentData With
             {.ID = report.ID,
              .paymentnumber = report.paymentnumber,
              .reportnumber = report.reportnumber,
              .employeenumber = report.employeenumber,
              .paymentdate = report.paymentdate,
              .paymentmonth = report.paymentmonth,
              .paymentyear = report.paymentyear,
              .Amount = report.Amount,
              .createdon = report.createdon,
              .createdfrom = report.createdfrom,
              .paymentreason = report.paymentreason,
              .translatedLAname = report.translatedLAname
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
        ShowAdvancePaymentDetailFrom(m_EmployeeNumber, advancePaymentData.paymentnumber)
      End If
    End Sub

    ''' <summary>
    ''' Handles click on button new advance payment.
    ''' </summary>
    Private Sub OnBtnAddAdvancePayment_Click(sender As System.Object, e As System.EventArgs) Handles btnAddAdvancePayment.Click
      If (m_EmployeeNumber.HasValue) Then
        ShowAdvancePaymentDetailFrom(m_EmployeeNumber, Nothing)
      End If
    End Sub

    ''' <summary>
    ''' Handles check changed event on chkThisYear.
    ''' </summary>
    Private Sub OnChkThisYear_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkThisYear.CheckedChanged

      If m_SuppressUIEvents Then
        Return
      End If

      If m_EmployeeNumber.HasValue Then

        LoadAdvancePaymentList(chkThisYear.Checked)

        Try
          m_SettingsManager.WriteBoolean(SettingKeys.SETTING_ADVANCEPAYMENT_FILTER_ONLY_CURRENT_YEAR, chkThisYear.Checked)
        Catch ex As Exception
          m_Logger.LogError(ex.ToString)
        End Try

      End If

    End Sub

    ''' <summary>
    ''' Shows the advance payment management form.
    ''' </summary>
    ''' <param name="zgNr">The payment record number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    Private Sub ShowAdvancePaymentDetailFrom(ByVal employeeNumber As Integer, ByVal zgNr As Integer?)

			'Dim strTranslationProgName As String = String.Empty
			'Dim _ClsReg As New SPProgUtility.ClsDivReg


			'Try
			'  _ClsReg.SetRegKeyValue("Software\yourregistrykeyname\Sputnik Suite\ProgOptions", "ZGNr", If(zgNr.HasValue, zgNr, 0))

			'  If vb6Object Is Nothing Then
			'    vb6Object = CreateObject("SPSModulsView.ClsMain")
			'  End If
			'  'oMyProg = CreateObject("SPSModulsView.ClsMain")
			'  If Not zgNr.HasValue OrElse zgNr = 0 Then
			'    If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 350, m_InitializationData.MDData.MDNr) Then Exit Sub
			'    vb6Object.TranslateProg4Net("CreateZGWithCandidateAndReport", employeeNumber,
			'                              m_ReportNumber,
			'                              m_Month,
			'                              m_Year)

			'  Else
			'    vb6Object.TranslateProg4Net("ZGUtility.ClsMain", zgNr, "2")

			'  End If


			'Catch ex As Exception
			'  m_Logger.LogError(ex.Message)
			'  m_UtilityUI.ShowErrorDialog(String.Format("Fehler in der Applikation: {1}", ex.Message))

			'End Try

			If zgNr.HasValue Then
				Dim frmAdvancePayments As SP.MA.AdvancePaymentMng.UI.frmAdvancePayments = New frmAdvancePayments(m_InitializationData)

				If (frmAdvancePayments.LoadAdvancePaymentData(zgNr)) Then
					frmAdvancePayments.Show()
					frmAdvancePayments.BringToFront()
				End If

			Else
				Dim yearToLoad As Integer = m_UCMediator.ActiveReportData.ReportData.Jahr
				Dim monthToLoad As Integer = m_UCMediator.ActiveReportData.ReportData.Monat
				Dim lonr As Integer? = m_UCMediator.ActiveReportData.ReportData.LONr
				If lonr.HasValue AndAlso lonr > 0 Then
					Dim msg As String = m_Translate.GetSafeTranslationValue("Für diesen Rapport können Sie keinen Vorschuss erfassen, da Sie bereits die Lohnabrechnung erstellt haben!")
					m_UtilityUI.ShowInfoDialog(msg)

					Return
				End If
				Dim preselection = New SP.MA.AdvancePaymentMng.PreselectionData With {.MDNr = m_InitializationData.MDData.MDNr,
																																	.EmployeeNumber = m_EmployeeNumber,
																																	.Advisor = m_InitializationData.UserData.UserKST,
																																	.RPNr = m_ReportNumber,
																																							.reportyear = yearToLoad,
																																							.reportmonth = monthToLoad
																																 }

				'Dim frmNewAdvancePayment As SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment = New SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment(m_InitializationData, preselection)
				'frmNewAdvancePayment.Show()
				'frmNewAdvancePayment.BringToFront()

				m_AdvancePaymentDetailForm = New SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment(m_InitializationData, preselection)
				AddHandler m_AdvancePaymentDetailForm.FormClosed, AddressOf OnAdvancePaymentFormClosed
				m_AdvancePaymentDetailForm.Show()
				m_AdvancePaymentDetailForm.BringToFront()

			End If

			'm_AdvancePaymentDetailForm.Show()
			'm_AdvancePaymentDetailForm.BringToFront()

		End Sub

    ''' <summary>
    ''' Handles close of advance payment form.
    ''' </summary>
    Private Sub OnAdvancePaymentFormClosed(sender As System.Object, e As System.EventArgs)
      LoadAdvancePaymentList(chkThisYear.Checked)

			'' Andreas fragen!!!
			'If Not m_AdvancePaymentDetailForm Is Nothing AndAlso Not m_AdvancePaymentDetailForm.IsDisposed Then
			'	m_AdvancePaymentDetailForm.Dispose()
			'End If

			Dim advancePaymentForm = CType(sender, frmNewAdvancePayment)

			If advancePaymentForm.NewAdvancePaymentNumber.HasValue AndAlso advancePaymentForm.NewAdvancePaymentNumber > 0 Then
				FocusAdvancePayment(m_EmployeeNumber, advancePaymentForm.NewAdvancePaymentNumber)
			End If

		End Sub

    ''' <summary>
    ''' Handles advance payment form data saved.
    ''' </summary>
		Private Sub OnAdvancePaymentFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal zgnr As Integer)

			LoadAdvancePaymentList(chkThisYear.Checked)

			FocusAdvancePayment(m_EmployeeNumber, zgnr)

		End Sub

    ''' <summary>
    ''' Handles advance payment form data deleted saved.
    ''' </summary>
    Private Sub OnAdvancePaymentFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal lmnr As Integer)
      LoadAdvancePaymentList(chkThisYear.Checked)
    End Sub

    ''' <summary>
    ''' Focuses an advance payment.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="zgNr">The zgNr record number.</param>
    Private Sub FocusAdvancePayment(ByVal employeeNumber As Integer, ByVal zgNr As Integer)

			Dim listDataSource As BindingList(Of EmployeeAdvancedPaymentData) = grdAdvancePayment.DataSource

      Dim advancePaymentViewData = listDataSource.Where(Function(data) data.employeenumber = employeeNumber AndAlso
                                                               data.paymentnumber = zgNr).FirstOrDefault()

      If Not advancePaymentViewData Is Nothing Then
        Dim sourceIndex = listDataSource.IndexOf(advancePaymentViewData)
        Dim rowHandle = gvAdvancePayment.GetRowHandle(sourceIndex)
        gvAdvancePayment.FocusedRowHandle = rowHandle
      End If

    End Sub

#End Region

#Region "View helper classes"

    ' ''' <summary>
    ' ''' Advance payment list item view data.
    ' ''' </summary>
    'Class AdvancePaymentListItemViewData

    '  Public Property EmployeeNumber As Integer
    '  Public Property LMNr As Integer?

    'End Class

#End Region


  End Class

End Namespace
