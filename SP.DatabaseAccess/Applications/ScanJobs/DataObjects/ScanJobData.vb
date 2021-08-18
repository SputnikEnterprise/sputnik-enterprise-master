
Imports SP.DatabaseAccess.ScanJob.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace ScanJob.DataObjects

	Public Class ScanJobData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property FoundedCodeValue As String
		Public Property ModulNumber As ScannModulEnum
		Public Property RecordNumber As Integer
		Public Property DocumentCategoryNumber As Integer
		Public Property IsValid As Boolean

		Public Property ReportYear As Integer?
		Public Property ReportMonth As Integer?
		Public Property ReportWeek As Integer?
		Public Property ReportFirstDay As Integer?
		Public Property ReportLastDay As Integer?
		Public Property ReportLineID As Integer?

		Public Property ScanContent As Byte()
		Public Property ImportedFileGuid As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

		Public Property ImportedDocID As Integer?
		Public Property ImportedDocRecNr As Integer?
		Public Property ImportedDocGuid As String

		Public Enum ScannModulEnum
			Employee
			Customer
			Employment
			Report
			Invoice
			Payroll
			NotDefined
		End Enum

	End Class


	Public Class ReportLineData

		Public Property RecID As Integer?
		Public Property LANr As Decimal?
		Public Property ReportNumber As Integer?
		Public Property ReportLineNumber As Integer?
		Public Property ReportLineFrom As Date?
		Public Property ReportLineTo As Date?

	End Class


End Namespace
