
Public Interface IClsDbRegister

	'Function GetKWDataInRP(ByVal iRPNr As Integer) As DataTable

	' Function GetRPDb4SelectingRP() As DataTable

	'Function DeleteExistsFileContentFromLocalDb(ByVal rpInfo As DBInformation) As Boolean
	Function DeleteAssignedReport(ByVal rpInfo As DBInformation) As Boolean
	Function UpdateAssignedReport(ByVal rpInfo As DBInformation) As Boolean
	Function UpdateFileContentData(ByVal rpInfo As DBInformation) As Boolean


	Function LoadReportData() As IEnumerable(Of ReportData)
	Function LoadReportLineData(ByVal reportNumber As Integer?) As IEnumerable(Of ReportLineData)


	'Function StoreExistsScannedFileFromLocalDb2FS(ByVal rpInfo As DBInformation) As Dictionary(Of String, String)

	'Function SaveSelectedFileIntoLocalDb(ByVal rpInfo As DBInformation) As Boolean

	Function AddAssignedEmployeeContentIntoFinalTable(ByVal data As AssignedDataToImport) As Boolean
	'Function AddAssignedCustomerContentIntoFinalTable(ByVal data As AssignedDataToImport) As Boolean
	Function AddAssignedReportContentIntoFinalTable(ByVal data As AssignedDataToImport) As Boolean

	'Function SaveCheckedContentIntoSPDb(ByVal rpInfo As DBInformation) As Boolean

	Function LoadAssignedReportScanContentWithForImport(ByVal strSelectedFileGuid As String) As IEnumerable(Of AssignedDataToImport)
	Function LoadAssignedScanContentWithForImport() As IEnumerable(Of AssignedDataToImport)

	'Function _ListRPScanDb4Print() As Boolean

	'Function _CreatePDFFileFromSP2FS() As String

	'Function ListAllRPContent4StoreIntoSPDb(ByVal strSelectedFileGuid As String) As DataTable

	'Function SaveChangedFileIntoDb(ByVal test As DBInformation) As Boolean

	'Function GetDbScannedFiles() As DataTable

	'Function ListNewScannedFiles(ByVal strSelectedFileGuid As String) As DataTable

	Function LoadScannedData() As IEnumerable(Of ScanedData)

	Function LoadScannedContentData(ByVal strSelectedFileGuid As String) As IEnumerable(Of ScanedContentData)
	Function LoadAssignedScannedContentData(ByVal recID As Integer) As ScanedContentData
	Function LoadAssignedScannedReport(ByVal data As DBInformation) As AssignedContentData

  Function StoreSelectedScannedFile2FS() As Dictionary(Of String, String)

	Function DeleteAssignedFileContent(ByVal recID As Integer) As Boolean
  'Function GetFileToByte(ByVal filePath As String) As Byte()


End Interface




Public Class PreselectionData

	Public Property EmployeeNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property ESNumber As Integer?
	Public Property ReportNumber As Integer?
	Public Property ReportLineNumber As Integer?
	Public Property CalendarWeek As Integer?

End Class



Public Class DBInformation


	Public Property SelectedFileID As Integer?
	Public Property OriginFileGuid As String

	Public Property EmployeeNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property ESNumber As Integer?

	Public Property SelectedRecordNumber As Integer?
	Public Property SelectedCategoryNumber As Integer?
	Public Property SelectedRPLNr As Integer?
	Public Property SelectedRPLID As Integer?

	Public Property CalendarWeek As Integer?

	Public Property DocScan As Byte()

	Public Property RPLFrom As Date?
	Public Property RPLTo As Date?

	Public Property SendRPToKDWOS As Boolean?
	Public Property SendRPToMAWOS As Boolean?


End Class


Public Class AssignedContentData

	'DocScan, IsNull(RPNr, 0) As RPNr, IsNull(RPDoc_Guid, '') As RPDoc_Guid, "
	'		sScanSql &= "IsNull(ESNr,0) As ESNr, IsNull(MANr, 0) As MANr, IsNull(KDNr, 0) As KDNr, IsNull(ScanExtension, 'PDF') As ScanExtension "
	Public Property ID As Integer?
	Public Property RPNr As Integer?
	Public Property ESNr As Integer?
	Public Property MANr As Integer?
	Public Property KDNr As Integer?
	Public Property RPDoc_Guid As String
	Public Property ScanExtension As String
	Public Property DocScan As Byte()

End Class

Public Class ScanedContentData

	Public Property ID As Integer?
	Public Property ModulNumber As Integer?
	Public Property RecordNumber As Integer?
	Public Property RPLID As Integer?
	Public Property DocumentCategoryNumber As Integer?
	Public Property File_ScannedOn As DateTime?
	Public Property CalendarWeek As Integer?
	Public Property RPMonth As Integer?
	Public Property ImportedFileGuild As String
	Public Property FoundedCodeValue As String

	Public Property Monday As DateTime?
	Public Property Sunday As DateTime?
	Public Property CheckedOn As DateTime?
	Public Property Scan_Komplett As Byte()
	Public Property IsValid As Boolean?

	Public ReadOnly Property DataToShow() As String
		Get
			Dim nameToShow As String = String.Empty
			Select Case ModulNumber
				Case 0
					nameToShow = String.Format("{0}_{1}", RecordNumber, DocumentCategoryNumber)

				Case 1
					nameToShow = String.Format("{0}_{1}", RecordNumber, DocumentCategoryNumber)

				Case 2

				Case 3
					nameToShow = String.Format("{0}_{1}", RecordNumber, CalendarWeek)

			End Select

			Return nameToShow
		End Get

	End Property

End Class


Public Class ScanedData

	Public Property ID As Integer?
	Public Property File_ScannedOn As String
	Public Property ImportedFileGuild As String

End Class


Public Class ReportData

	Public Property ID As Integer?
	Public Property ReportNumber As Integer?
	Public Property Monat As Integer?
	Public Property Jahr As Integer?
	Public Property Von As Date?
	Public Property Bis As Date?

	Public Property EmployeeName As String
	Public Property CustomerName As String

End Class

Public Class ReportLineData

	Public Property ID As Integer?
	Public Property LANr As decimal?
	Public Property RPLNr As Integer?
	Public Property VonDate As Date?
	Public Property BisDate As Date?

End Class

Public Class AssignedDataToImport

  Public Property ID As Integer?

  Public Property File_ScannedOn As DateTime?
  Public Property RecordNumber As Integer?
  Public Property CalendarWeek As Integer?
  Public Property LAOPText As String
  Public Property Period As String
  Public Property ImportedFileGuild As String

  Public Property SP_RecordNumber As Integer?
  Public Property SP_Period As String
  Public Property SP_RPLNumber As Integer?

  Public Property SP_EmployeeNumber As Integer?
  Public Property SP_CustomerNumber As Integer?
  Public Property SP_EinsatzNumber As Integer?

  Public Property RecipientName As String

  Public Property ModulNumber As Integer?
  Public Property DocumentCategoryNumber As Integer?

  Public Property SendToCustomerWOS As Boolean?
  Public Property SendToEmployeeWOS As Boolean?

  Public Property IsSelected As Boolean?


End Class


