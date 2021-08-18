Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.ES.DataObjects.ESMng

Public Class ActiveReportData

#Region "Public Properties"

  Public Property ReportData As RPMasterData
  Public Property CustomerOfActiveReport As CustomerMasterData
  Public Property EmployeeOfActiveReport As EmployeeMasterData
  Public Property ESDataOfActiveReport As ESMasterData

#End Region

End Class
