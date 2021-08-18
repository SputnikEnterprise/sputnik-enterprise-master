
Imports SP.DatabaseAccess.EMailJob.DataObjects


Namespace EMailJob

	Public Interface IEMailJobDatabaseAccess

		Function LoadEMailSettingData(ByVal eMailData As EMailData) As EMailSettingData
		Function LoadEMailSettingForParsingData(ByVal customerID As String, ByVal whatToUpload As EMailSettingData.UploadEnum) As EMailSettingData
		Function LoadEMailUserData(ByVal eMailData As EMailData) As EmailUserData

		Function AddEMailJob(ByVal eMailData As EMailData, ByVal applicationID As Integer?) As Boolean
		Function AddEMailAttachmentJob(ByVal eMailID As Integer, ByVal attachmentData As EMailAttachment) As Boolean

		Function LoadAssigendApplicationEMailData(ByVal customerID As String, ByVal applicationId As Integer?, ByVal searchDate As Date?) As EMailData
		Function LoadExistingAssigendEMailData(ByVal eMailData As EMailData) As EMailData
		Function LoadEMailPatternParsingData(ByVal customerID As String) As EMailPatternSettingData

	End Interface

End Namespace
