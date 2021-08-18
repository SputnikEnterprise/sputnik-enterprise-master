
Imports ReportImporterCommon


''' <summary>
''' Interface for a report database persister.
''' </summary>
Public Interface IReportDBPersister

	Function LoadSettingData(ByVal settingFileValue As ProgramSettings) As Boolean

	''' <summary>
	''' Saves the complete report (multipage PDF) to database.
	''' </summary>
	''' <param name="reportDBInformation">The database information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Function SaveCompleteFile(reportDBInformation As ReportDBInformation) As Boolean

	''' <summary>
	''' Saves report information to database.
	''' </summary>
	''' <param name="reportDBInformation">The report information.</param>
	''' <returns>Boolean truth value indicating success.</returns>
	Function SaveData(ByVal reportDBInformation As ReportDBInformation) As Boolean

	Function SendNotificationForImportedFiles(ByVal reportDBInformation As ReportDBInformation, ByVal notifyData As NotificationData) As Boolean

	Function LoadMandantData(ByVal mdGuid As String) As MandantData

End Interface
