Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common

Namespace UI

  Public Interface INewEmployeeUserControlFormMediator

    Sub HandleChangeOfMandant(ByVal mdNumber As Integer)
    Sub CountryCodeHasChanged()
    Sub NationalityHasChanged()

    ReadOnly Property SelectedMandantAndAdvisorData As InitMandantAndAdvisorData
		'ReadOnly Property SelectedBasiscData As InitEmployeeBasicData
		ReadOnly Property SelectedAdditionalData1 As InitAdditionalEmployeeData1
    ReadOnly Property selectedAdditionalData2 As InitAdditionalEmployeeData2
    ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
    ReadOnly Property EmployeeDbAccess As IEmployeeDatabaseAccess

  End Interface

End Namespace
