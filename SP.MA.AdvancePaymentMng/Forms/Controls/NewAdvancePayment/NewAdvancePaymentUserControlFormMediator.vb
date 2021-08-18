
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.AdvancePaymentMng

Namespace UI

  ''' <summary>
  ''' Mediator to communicate between user controls (only for new advance payment wizard).
  ''' </summary>
  Public Class NewAdvancePaymentUserControlFormMediator
    Implements INewAdvancePaymentUserControlFormMediator

#Region "Private Fields"

    Private m_frmNewAdvancePaymentFrm As frmNewAdvancePayment

    Private m_ucPageSelectMandant As ucPageSelectMandant
    Private m_ucPageSelectBank As ucPageSelectBank
    Private m_ucPageSelectAmountOfPayment As ucPageSelectAmountOfPayment
    Private m_ucPageCreateZG As ucPageCreateZG

    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    Private m_UIUtility As SP.Infrastructure.UI.UtilityUI

#End Region

#Region "Constructor"

    Public Sub New(ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper,
                   ByVal frmNewAdvancePayment As frmNewAdvancePayment,
                   ByVal ucPageSelectManat As ucPageSelectMandant,
                   ByVal ucPageSelectAmountOfPayment As ucPageSelectAmountOfPayment,
                   ByVal ucPageCreateZG As ucPageCreateZG)

      m_UIUtility = New SP.Infrastructure.UI.UtilityUI

      Me.m_frmNewAdvancePaymentFrm = frmNewAdvancePayment
      Me.m_ucPageSelectMandant = ucPageSelectManat
			'Me.m_ucPageSelectBank = ucPageSelectBank
      Me.m_ucPageSelectAmountOfPayment = ucPageSelectAmountOfPayment
      Me.m_ucPageCreateZG = ucPageCreateZG

      Me.m_Translate = translate

      m_ucPageSelectMandant.UCMediator = Me
			'm_ucPageSelectBank.UCMediator = Me
      m_ucPageSelectAmountOfPayment.UCMediator = Me
      m_ucPageCreateZG.UCMediator = Me

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    '''  Handles change of mandant or employee.
    ''' </summary>
    Public Sub HandleChangeMandantOrEmployee() Implements INewAdvancePaymentUserControlFormMediator.HandleChangeMandantEmployeeOrEmployee
			'm_ucPageSelectBank.Reset()
      m_ucPageSelectAmountOfPayment.Reset()
    End Sub

    ''' <summary>
    ''' Handles change of mandant.
    ''' </summary>
    ''' <param name="mdNumber">The mandant number.</param>
    Public Sub HandleChageOfMandant(ByVal mdNumber As Integer) Implements INewAdvancePaymentUserControlFormMediator.HandleChangeOfMandant
      m_frmNewAdvancePaymentFrm.ChangeMandant(mdNumber)
			'm_ucPageSelectBank.Reset()
      m_ucPageSelectAmountOfPayment.Reset()
    End Sub

    ''' <summary>
    ''' Handle finish click
    ''' </summary>
    Public Function HandleFinishClick() As Boolean Implements INewAdvancePaymentUserControlFormMediator.HandleFinishClick

      If ValidateData() Then
        m_ucPageCreateZG.CreateZG()

        Return True
      Else
        m_UIUtility.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Bitte prüfen Sie die Eingaben."))
        Return False
      End If
    End Function


    ''' <summary>
    ''' Validates all data.
    ''' </summary>
    Public Function ValidateData() As Boolean Implements INewAdvancePaymentUserControlFormMediator.ValidateData
      Return m_frmNewAdvancePaymentFrm.ValidateData()
    End Function

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected candidate and advisor data.
    ''' </summary>
    ''' <returns>Candidate and advisor data.</returns>
    Public ReadOnly Property SelectedCandidateAndAdvisorData As InitCandidateAndAdvisorData Implements INewAdvancePaymentUserControlFormMediator.SelectedCandidateAndAdvisorData
      Get
        Return m_ucPageSelectMandant.SelectedCandidateAndAdvisorData
      End Get
    End Property

		' ''' <summary>
		' ''' Gets the selected bank data.
		' ''' </summary>
		' ''' <returns>Bank data.</returns>
		'Public ReadOnly Property SelectedBankData As InitBankData Implements INewAdvancePaymentUserControlFormMediator.SelectedBankData
		'  Get
		'Return m_ucPageSelectBank.SelectedBankData
		'  End Get
		'End Property

    ''' <summary>
    ''' Gets the selected payment data.
    ''' </summary>
    ''' <returns>Payment data.</returns>
    Public ReadOnly Property SelectedPaymentData As InitPaymentData Implements INewAdvancePaymentUserControlFormMediator.SelectedPaymentData
      Get
        Return m_ucPageSelectAmountOfPayment.SelectedPaymentData
      End Get
    End Property

    ''' <summary>
    ''' Gets the common db access object.
    ''' </summary>
    Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess Implements INewAdvancePaymentUserControlFormMediator.CommonDbAccess
      Get
        Return m_frmNewAdvancePaymentFrm.CommonDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the advance payment db access object.
    ''' </summary>
    Public ReadOnly Property AdvancePaymentDbAccess As IAdvancePaymentDatabaseAccess Implements INewAdvancePaymentUserControlFormMediator.AdvancePaymentDbAccess
      Get
        Return m_frmNewAdvancePaymentFrm.AdvancePaymentDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the customer db access object.
    ''' </summary> 
    Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess Implements INewAdvancePaymentUserControlFormMediator.CustomerDbAccess
      Get
        Return m_frmNewAdvancePaymentFrm.CustomerDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the employee db access object.
    ''' </summary> 
    Public ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess Implements INewAdvancePaymentUserControlFormMediator.EmployeeDbAccess
      Get
        Return m_frmNewAdvancePaymentFrm.EmployeeDbAccess
      End Get
    End Property

#End Region

  End Class

End Namespace