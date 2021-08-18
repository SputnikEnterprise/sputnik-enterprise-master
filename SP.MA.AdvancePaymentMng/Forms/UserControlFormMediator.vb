Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Namespace UI

  ''' <summary>
  ''' Mediator to commuicate between user controls and the parent form.
  ''' </summary>
  Public Class UserControlFormMediator

#Region "Private Fields"

    Private m_frmAdvancePayment As frmAdvancePayments
    Private m_ucMainContent As ucMainContent
    Private m_ucNegativeSalaryData As ucNegativeSalaryData
    Private m_ucBankData As ucBankData
    Private m_ucAdvancePaymentsList As ucAdvancePaymentslist

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="frmAdvancePayment">The advance payment form.</param>
    ''' <param name="mainContent">The main content.</param>
    ''' <param name="negativeSalaryData">The negative salary data.</param>
    ''' <param name="bankData">The bank data.</param>
    ''' <param name="advancePaymentslist">The advance payments list.</param>
    Public Sub New(ByVal frmAdvancePayment As frmAdvancePayments,
                   ByVal mainContent As ucMainContent,
                   ByVal negativeSalaryData As ucNegativeSalaryData,
                   ByVal bankData As ucBankData,
                   ByVal advancePaymentsList As ucAdvancePaymentslist)

      Me.m_frmAdvancePayment = frmAdvancePayment
      Me.m_ucMainContent = mainContent
      Me.m_ucNegativeSalaryData = negativeSalaryData
      Me.m_ucBankData = bankData
      Me.m_ucAdvancePaymentsList = advancePaymentsList


      Me.m_ucMainContent.UCMediator = Me
      Me.m_ucNegativeSalaryData.UCMediator = Me
      Me.m_ucBankData.UCMediator = Me
      Me.m_ucAdvancePaymentsList.UCMediator = Me
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets the ZG data.
    ''' </summary>
    ''' <returns>The ZG data.</returns>
    Public ReadOnly Property ZGData As ZGMasterData
      Get
        Return m_frmAdvancePayment.ZGData
      End Get

    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Loads the advance payment data.
    ''' </summary>
    ''' <param name="zgNumber">The zg number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function LoadAdvancePaymentData(ByVal zgNumber As Integer)
      Return m_frmAdvancePayment.LoadAdvancePaymentData(zgNumber)
    End Function

#End Region

  End Class

End Namespace