Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Customer.DataObjects

Namespace UI

  ''' <summary>
  ''' Initial candidate and customer data.
  ''' </summary>
  Public Class InitCandidateAndCustomerData

    Public Property MandantData As MandantData
    Public Property EmployeeData As EmployeeMasterData
    Public Property CustomerData As CustomerMasterData
    Public Property ResponsiblePersondata As ResponsiblePersonMasterData
		Public Property EmployeeNoticeEmployement As String
		Public Property CustomerNoticeEmployement As String

	End Class

End Namespace
