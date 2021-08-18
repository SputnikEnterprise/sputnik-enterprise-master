Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.DatabaseAccess.Common.DataObjects

Namespace UI

  Public Interface INewAdvancePaymentUserControlFormMediator

    Sub HandleChangeMandantEmployeeOrEmployee()

    Sub HandleChangeOfMandant(ByVal mdNumber As Integer)
    Function HandleFinishClick() As Boolean
    Function ValidateData() As Boolean

    ReadOnly Property SelectedCandidateAndAdvisorData As InitCandidateAndAdvisorData

		'ReadOnly Property SelectedBankData As InitBankData

		ReadOnly Property SelectedPaymentData As InitPaymentData
    ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
    ReadOnly Property AdvancePaymentDbAccess As IAdvancePaymentDatabaseAccess
    ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess
    ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess

  End Interface

End Namespace
