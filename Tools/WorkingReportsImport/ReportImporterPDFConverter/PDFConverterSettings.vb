'------------------------------------
' File: PDFConverterSettings.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports ReportImporterCommon
''' <summary>
''' Settings used by the pdf converter.
''' </summary>
Public Class PDFConverterSettings

#Region "Properties"

	Public Property CustomerID As String
	Public Property SettingFileValue As ProgramSettings

	''' <summary>
	''' The temporary folder that should be used for pdf processing.
	''' </summary>
	Public Property TemporaryFolder As String

	''' <summary>
	''' The folder where processed documents should be placed into.
	''' </summary>
	Public Property ProcessedScannedDocumentsFolder As String

	''' <summary>
	''' The connection string to the ScanJobs database.
	''' </summary>
	Public Property ScanJobsConnectionString As String

	''' <summary>
	''' Gets or sets the if is for webservice useing.
	''' </summary>
	Public Property WorkingForWebService As Boolean?


#End Region

End Class
