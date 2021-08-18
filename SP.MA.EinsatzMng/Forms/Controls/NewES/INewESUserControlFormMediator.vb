Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common

Namespace UI

  Public Interface INewESUserControlFormMediator

    Sub HandleChangeMandantEmployeeOrCustomer()
    Function HandleFinishClick() As Boolean
    Sub HandleChangeOfMandant(ByVal mdNumber As Integer)
    Function ValidateData() As Boolean
    ReadOnly Property SelectedSalaryData As InitESSalaryData
    ReadOnly Property SelectedESData As InitESData
    ReadOnly Property SelectedCandidateAndCustomerData As InitCandidateAndCustomerData
    ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
    ReadOnly Property ESDbAccess As IESDatabaseAccess
    ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess
    ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess

  End Interface

End Namespace
