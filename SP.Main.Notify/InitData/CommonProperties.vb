
Public Class EmployeeScanJobData

	Public Property ID As Integer
	Public Property Customer_ID As String
	Public Property RecordNumber As Integer
	Public Property DocumentCategoryNumber As Integer
	Public Property IsValid As Boolean

	Public Property ScanContent As Byte()
	Public Property ImportedFileGuid As String
	Public Property CreatedOn As DateTime?
	Public Property CreatedFrom As String
	Public Property CheckedOn As DateTime?
	Public Property CheckedFrom As String


End Class
