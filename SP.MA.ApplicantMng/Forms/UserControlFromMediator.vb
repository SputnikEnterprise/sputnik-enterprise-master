
Namespace UI

  ''' <summary>
  ''' Mediator to commuicate between user controls.
  ''' </summary>
  Public Class UserControlFromMediator

#Region "Private Fields"

		Private m_EmployeeFrm As frmApplicant
		Private m_ucCommon As ucCommonData
		Private m_ucSalary As ucSalaryData
		Private m_ucSalary2 As ucSalaryData2
    Private m_ucCVLWorkData As ucCVLWork

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="frmEmployee">The employee form.</param>
    ''' <param name="ucCommon">The common data control.</param>
    ''' <param name="ucSalaryData">The salary data control.</param>
    Public Sub New(ByVal frmEmployee As frmApplicant,
                   ByVal ucCommon As ucCommonData,
                   ByVal ucSalaryData As ucSalaryData,
                   ByVal ucSalaryData2 As ucSalaryData2)

      Me.m_EmployeeFrm = frmEmployee
      Me.m_ucCommon = ucCommon
      'Me.m_ucSalary = ucSalaryData
      'Me.m_ucSalary2 = ucSalaryData2
      'Me.m_ucBankData = ucBankData

      ucCommon.UCMediator = Me
      'ucSalaryData.UCMediator = Me
      'ucSalaryData2.UCMediator = Me
      'ucCVLWorkData.UCMediator = Me

    End Sub


#End Region

#Region "Public Properties"

    ''' <summary>
    ''' CountryCode has changed.
    ''' </summary>
    ''' <param name="employeeNumber">The emplyoee number.</param>
    Public Sub CountryCodeHasChanged(ByVal employeeNumber As Integer)
			'm_ucSalary.CountryCodeHasChanged(employeeNumber)
		End Sub


    ''' <summary>
    ''' Nationalty has changed.
    ''' </summary>
    ''' <param name="employeeNumber">The emplyoee number.</param>
    Public Sub NationalityHasChanged(ByVal employeeNumber As Integer)
			'm_ucSalary.NationalityHasChanged(employeeNumber)
		End Sub

    ''' <summary>
    ''' Gets the country code from UI.
    ''' </summary>
    ''' <param name="employeeNumber">The emplyoee number.</param>
    ''' <returns>The selected country code.</returns>
    Public Function GetCountryCodeFromUi(ByVal employeeNumber As Integer)
      ' Make sure data of the employee is loaded.
      m_ucCommon.Activate(employeeNumber)
      Dim countryCode As String = m_ucCommon.SelectedCountryCode
      Return countryCode
    End Function

    ''' <summary>
    ''' Gets the selected nationality form UI.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>The selected nationality.</returns>
    Public Function GetNationalityFromUi(ByVal employeeNumber As Integer)
      ' Make sure data of employee is loaded.
      m_ucCommon.Activate(employeeNumber)
      Dim nationality As String = m_ucCommon.SelectedNationality
      Return nationality
    End Function

		Public Sub ZahlartCodeHasChanged(ByVal employeeNumber As Integer)
			'm_ucSalary2.ZahlarHasChanged(employeeNumber)
		End Sub

    ''' <summary>
    ''' Report invalid data.
    ''' </summary>
    Public Sub ReportInvalidData()
      m_EmployeeFrm.IsDataValid = False
    End Sub

#End Region

  End Class

End Namespace