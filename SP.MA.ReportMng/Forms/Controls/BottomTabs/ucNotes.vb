Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee

Namespace UI

  Public Class ucNotes

#Region "Pirvate Fields"

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

    ''' <summary>
    ''' The customer database access.
    ''' </summary>
    Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

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
      m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_ReportNumber = Nothing

      Dim previousState = SetSuppressUIEventsState(True)

      txtCommentEmployee.Text = String.Empty
      txtCommentCustomer.Text = String.Empty
      txtCommentCustomer.Properties.MaxLength = 1000

      SetSuppressUIEventsState(previousState)

    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    Public Overrides Function Activate() As Boolean

      Dim success As Boolean = True

      If m_UCMediator.ActiveReportData Is Nothing Then
        Return False
      End If

      Dim rpNrToLoad = m_UCMediator.ActiveReportData.ReportData.RPNR

      If (Not IsReportDataLoaded OrElse (Not m_ReportNumber = rpNrToLoad)) Then
        CleanUp()

        success = success AndAlso LoadData()

        m_ReportNumber = IIf(success, rpNrToLoad, 0)

      End If

      Return success
    End Function

    ''' <summary>
    ''' Saves the tab page data.
    ''' </summary>
    Public Overrides Function SaveReportData() As Boolean

      Dim success As Boolean = True

      Dim rpData = m_UCMediator.ActiveReportData

      If ((IsReportDataLoaded AndAlso
        m_ReportNumber = rpData.ReportData.RPNR)) Then

				Dim customer = m_CustomerDatabaseAccess.LoadCustomerNoticesData(rpData.CustomerOfActiveReport.CustomerNumber)
				Dim employee = m_EmployeeDataAccess.LoadEmployeeNoticesData(rpData.EmployeeOfActiveReport.EmployeeNumber)

				If customer Is Nothing Or
          employee Is Nothing Then
          Return False
        End If

				customer.Notice_Report = txtCommentCustomer.Text
				employee.Notice_Report = txtCommentEmployee.Text

				success = success AndAlso m_CustomerDatabaseAccess.UpdateCustomerNoticesData(customer)
				success = success AndAlso m_EmployeeDataAccess.UpdateEmployeeNoticesData(employee)

			End If

      Return success
    End Function

#End Region

#Region "Private Metods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.grpEmployee.Text = m_Translate.GetSafeTranslationValue(Me.grpEmployee.Text)
      Me.grpCustomer.Text = m_Translate.GetSafeTranslationValue(Me.grpCustomer.Text)

    End Sub

    ''' <summary>
    ''' Loads the data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadData() As Boolean

      Dim success As Boolean = True

      Dim employee = m_UCMediator.ActiveReportData.EmployeeOfActiveReport
      Dim customer = m_UCMediator.ActiveReportData.CustomerOfActiveReport

			txtCommentEmployee.Text = employee.Notice_Report
			txtCommentCustomer.Text = customer.Notice_Report

			Return success

    End Function

#End Region

  End Class

End Namespace
