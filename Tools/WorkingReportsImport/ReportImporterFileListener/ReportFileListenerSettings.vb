
Imports ReportImporterCommon

Public Class ReportFileListenerSettings

	Public Property SettingFileValue As ProgramSettings
	Public Property bNotifyOnScan As Boolean
	Public Property bNotifyOnDispose As Boolean
	Public Property Folder2Watch As String
	Public Property ConnStr4ScanDb As String
	Public Property Folder4ProcessedScannedDocuments As String
	Public Property Folder4TemporaryFiles As String

	Public Property SendNotificationTo As String
	Public Property SmtpServer As String
	Public Property SmtpPort As String

	Public Property WorkingForWebService As Boolean?

End Class
