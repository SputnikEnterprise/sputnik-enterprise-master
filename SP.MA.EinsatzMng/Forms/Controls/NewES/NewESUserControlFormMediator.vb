Imports SP.MA.EinsatzMng.UI.ucPageSelectCandidateAndCustomer
Imports SP.MA.EinsatzMng.UI.ucPageSelectESData
Imports SP.MA.EinsatzMng.UI.ucPageSelectSalaryData
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer

Namespace UI

  ''' <summary>
  ''' Mediator to communicate between user controls (only for new ES wizard).
  ''' </summary>
  Public Class NewESUserControlFormMediator
    Implements INewESUserControlFormMediator
      
#Region "Private Fields"

    Private m_ESFrm As frmNewES
    Private m_ucPageCandidateAndCustomer As ucPageSelectCandidateAndCustomer
    Private m_ucPageSelectESData As ucPageSelectESData
    Private m_ucPageSelectSalaryData As ucPageSelectSalaryData
    Private m_ucPageCreateES As ucPageCreateES

    ''' <summary>
    ''' Thre translation value helper.
    ''' </summary>
    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    Private m_UIUtility As SP.Infrastructure.UI.UtilityUI

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="frmNewES">The new ES form.</param>
    ''' <param name="pageCandidateAndCustomer">The candidate and customer control.</param>
    ''' <param name="pageSelectESData">The ES data control.</param>
    ''' <param name="pageSelectSalaryData">The salary data control.</param>
    ''' <param name="pageCreateES">The create ES control.</param>
    Public Sub New(ByVal frmNewES As frmNewES,
                   ByVal pageCandidateAndCustomer As ucPageSelectCandidateAndCustomer,
                   ByVal pageSelectESData As ucPageSelectESData,
                   ByVal pageSelectSalaryData As ucPageSelectSalaryData,
                   ByVal pageCreateES As ucPageCreateES,
                   ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

      m_UIUtility = New SP.Infrastructure.UI.UtilityUI

      Me.m_ESFrm = frmNewES
      Me.m_ucPageCandidateAndCustomer = pageCandidateAndCustomer
      Me.m_ucPageSelectESData = pageSelectESData
      Me.m_ucPageSelectSalaryData = pageSelectSalaryData
      Me.m_ucPageCreateES = pageCreateES
      Me.m_Translate = translate

      m_ucPageCandidateAndCustomer.UCMediator = Me
      m_ucPageSelectESData.UCMediator = Me
      m_ucPageSelectSalaryData.UCMediator = Me
      m_ucPageCreateES.UCMediator = Me

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    '''  Handles change of mandant, employee or customer.
    ''' </summary>
    Public Sub HandleChangeMandantEmployeeOrCustomer() Implements INewESUserControlFormMediator.HandleChangeMandantEmployeeOrCustomer
      m_ucPageSelectESData.Reset()
      m_ucPageSelectSalaryData.Reset()
      m_ucPageCreateES.Reset()
    End Sub

    ''' <summary>
    ''' Handle finish click
    ''' </summary>
    Public Function HandleFinishClick() As Boolean Implements INewESUserControlFormMediator.HandleFinishClick

      If ValidateData() Then
        m_ucPageCreateES.CreateES()

        Return True
      Else
        m_UIUtility.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Bitte prüfen Sie die Eingaben."))
        Return False
      End If
    End Function

    ''' <summary>
    ''' Handles change of mandant.
    ''' </summary>
    ''' <param name="mdNumber">The mandant number.</param>
    Public Sub HandleChageOfMandant(ByVal mdNumber As Integer) Implements INewESUserControlFormMediator.HandleChangeOfMandant
      m_ESFrm.ChangeMandant(mdNumber)
    End Sub

    ''' <summary>
    ''' Validates all data.
    ''' </summary>
    Public Function ValidateData() As Boolean Implements INewESUserControlFormMediator.ValidateData
      Return m_ESFrm.ValidateData()
    End Function

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected candidate and customer data.
    ''' </summary>
    ''' <returns>Selected candidate and customer data.</returns>
    Public ReadOnly Property SelectedCandidateAndCustomerData As InitCandidateAndCustomerData Implements INewESUserControlFormMediator.SelectedCandidateAndCustomerData
      Get
        Return m_ucPageCandidateAndCustomer.SelectedCandidateAndCustomerData
      End Get
    End Property

    ''' <summary>
    ''' Gets the selected ES data.
    ''' </summary>
    ''' <returns>Selected ES data.</returns>
    Public ReadOnly Property SelectedESData As InitESData Implements INewESUserControlFormMediator.SelectedESData
      Get
        Return m_ucPageSelectESData.SelectedESData
      End Get
    End Property

    ''' <summary>
    ''' Gets the ES salary data data.
    ''' </summary>
    ''' <returns>Selected ES salary data.</returns>
    Public ReadOnly Property SelectedSalaryData As InitESSalaryData Implements INewESUserControlFormMediator.SelectedSalaryData
      Get
        Return m_ucPageSelectSalaryData.SelectedESSalaryData
      End Get
    End Property

    ''' <summary>
    ''' Gets the common db access object.
    ''' </summary>
    Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess Implements INewESUserControlFormMediator.CommonDbAccess
      Get
        Return m_ESFrm.CommonDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the ES db access object.
    ''' </summary>
    Public ReadOnly Property ESDbAccess As IESDatabaseAccess Implements INewESUserControlFormMediator.ESDbAccess
      Get
        Return m_ESFrm.ESDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the employee db access object.
    ''' </summary>
    Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess Implements INewESUserControlFormMediator.EmployeeDbAccess
      Get
        Return m_ESFrm.EmployeeDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the customer db access object.
    ''' </summary> 
    Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess Implements INewESUserControlFormMediator.CustomerDbAccess
      Get
        Return m_ESFrm.CustomerDbAccess
      End Get
    End Property

#End Region

  End Class

End Namespace