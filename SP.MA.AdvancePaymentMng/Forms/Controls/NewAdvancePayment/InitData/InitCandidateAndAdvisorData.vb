Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.Salary

Namespace UI

  ''' <summary>
  ''' Initial candidate and advisor data.
  ''' </summary>
  Public Class InitCandidateAndAdvisorData

    Public Property MandantData As MandantData
    Public Property EmployeeData As EmployeeMasterData
    Public Property EmployeeLOSettingData As EmployeeLOSettingsData
    Public Property AdvisorData As AdvisorData
		Public Property NoticeAdvancedpayment As String

	End Class

End Namespace
