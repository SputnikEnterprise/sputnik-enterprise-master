
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer

Namespace UI

  ''' <summary>
  ''' Mediator to communicate between user controls (only for new employee wizard).
  ''' </summary>
  Public Class NewEmployeeUserControlFormMediator
    Implements INewEmployeeUserControlFormMediator

#Region "Private Fields"

    Private m_NewEmployeeFrm As frmNewEmployee
    Private m_ucPageMandantAndAdvisor As ucPageWelcome
    Private m_ucPageEmployeeBasisData As ucPageEmployeeBasicData
    Private m_ucPageEmployeeAdditionalData1 As ucPageEmployeeAdditionalData1
    Private m_ucPageEmployeeAdditionalData2 As ucPageEmployeeAdditionalData2
    Private m_ucPageCreateEmployee As ucPageCreateEmployee

    ''' <summary>
    ''' Thre translation value helper.
    ''' </summary>
    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    ''' <summary>
    ''' The ui utility.
    ''' </summary>
    Private m_UIUtility As SP.Infrastructure.UI.UtilityUI

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="frmNewEmployee">The new employee form.</param>
    ''' <param name="pageMandantAndAdvisorData">The mandant and advisor data control.</param>
    ''' <param name="pageBasicData">The basic employee data control.</param>
    ''' <param name="pageAdditionalData1">The additional employee data 1 control.</param>
    ''' <param name="pageAdditionalData2">The additional employee data 2 control.</param>
    ''' <param name="pageCreateEmployee">The basic employee data control.</param>
    Public Sub New(ByVal frmNewEmployee As frmNewEmployee,
                   ByVal pageMandantAndAdvisorData As ucPageWelcome,
                   ByVal pageBasicData As ucPageEmployeeBasicData,
                   ByVal pageAdditionalData1 As ucPageEmployeeAdditionalData1,
                   ByVal pageAdditionalData2 As ucPageEmployeeAdditionalData2,
                   ByVal pageCreateEmployee As ucPageCreateEmployee,
                   ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

      m_UIUtility = New SP.Infrastructure.UI.UtilityUI

      Me.m_NewEmployeeFrm = frmNewEmployee
      Me.m_ucPageMandantAndAdvisor = pageMandantAndAdvisorData
      Me.m_ucPageEmployeeBasisData = pageBasicData
      Me.m_ucPageEmployeeAdditionalData1 = pageAdditionalData1
      Me.m_ucPageEmployeeAdditionalData2 = pageAdditionalData2
      Me.m_ucPageCreateEmployee = pageCreateEmployee
      Me.m_Translate = translate

      m_ucPageMandantAndAdvisor.UCMediator = Me
			'm_ucPageEmployeeBasisData.UCMediator = Me
			m_ucPageEmployeeAdditionalData1.UCMediator = Me
      m_ucPageEmployeeAdditionalData2.UCMediator = Me
      m_ucPageCreateEmployee.UCMediator = Me

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Handles change of mandant.
    ''' </summary>
    ''' <param name="mdNumber">The mandant number.</param>
    Public Sub HandleChageOfMandant(ByVal mdNumber As Integer) Implements INewEmployeeUserControlFormMediator.HandleChangeOfMandant
      m_NewEmployeeFrm.ChangeMandant(mdNumber)

			' Reset all other pages 
			'm_ucPageEmployeeBasisData.Reset()
			m_ucPageEmployeeAdditionalData1.Reset()
      m_ucPageEmployeeAdditionalData2.Reset()
      m_ucPageCreateEmployee.Reset()
    End Sub

    ''' <summary>
    ''' CountryCode has changed.
    ''' </summary>
    Public Sub CountryCodeHasChanged() Implements INewEmployeeUserControlFormMediator.CountryCodeHasChanged
      m_ucPageEmployeeAdditionalData2.CountryCodeHasChanged()
    End Sub


    ''' <summary>
    ''' Nationalty has changed.
    ''' </summary>
    Public Sub NationalityHasChanged() Implements INewEmployeeUserControlFormMediator.NationalityHasChanged
      m_ucPageEmployeeAdditionalData2.NationalityHasChanged()
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected manant and advisor data.
    ''' </summary>
    ''' <returns>Selected candidate and customer data.</returns>
    Public ReadOnly Property SelectedMandantAndAdvisorData As InitMandantAndAdvisorData Implements INewEmployeeUserControlFormMediator.SelectedMandantAndAdvisorData
      Get
        Return m_ucPageMandantAndAdvisor.SelectedMandantAndAdvisorData
      End Get
    End Property

		' ''' <summary>
		' ''' Gets the selected employee basic data.
		' ''' </summary>
		' ''' <returns>Selected employee basic data.</returns>
		' Public ReadOnly Property SelectedBasiscData As InitEmployeeBasicData Implements INewEmployeeUserControlFormMediator.SelectedBasiscData
		'   Get
		'	Return m_ucPageEmployeeBasisData.SelectedEmployeeBasicData
		'End Get
		' End Property

		''' <summary>
		''' Gets the selected employee additional data1.
		''' </summary>
		''' <returns>Selected employee additional data1.</returns>
		ReadOnly Property SelectedAdditionalData1 As InitAdditionalEmployeeData1 Implements INewEmployeeUserControlFormMediator.SelectedAdditionalData1
      Get
        Return m_ucPageEmployeeAdditionalData1.SelectedEmployeeAdditionalData1
      End Get
    End Property

    ''' <summary>
    ''' Gets the selected employee additional data1.
    ''' </summary>
    ''' <returns>Selected employee additional data1.</returns>
    ReadOnly Property selectedAdditionalData2 As InitAdditionalEmployeeData2 Implements INewEmployeeUserControlFormMediator.selectedAdditionalData2
      Get
        Return m_ucPageEmployeeAdditionalData2.SelectedEmployeeAdditionalData2
      End Get
    End Property

    ''' <summary>
    ''' Gets the common db access object.
    ''' </summary>
    Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess Implements INewEmployeeUserControlFormMediator.CommonDbAccess
      Get
        Return m_NewEmployeeFrm.CommonDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the employee db access object.
    ''' </summary>
    Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess Implements INewEmployeeUserControlFormMediator.EmployeeDbAccess
      Get
        Return m_NewEmployeeFrm.EmployeeDbAccess
      End Get
    End Property

#End Region

  End Class

End Namespace