
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace EMailJob.DataObjects



	Public Class EmailUserData

		Public Property Customer_ID As String
		Public Property User_ID As String
		Public Property UserNr As Integer
		Public Property UserSalutation As String
		Public Property UserLName As String
		Public Property UserFName As String
		Public Property UserKST As String
		Public Property UserBranchOffice As String
		Public Property UserFTitel As String
		Public Property UserSTitel As String

		Public Property UserMobile As String
		Public Property UsereMail As String
		Public Property UserLanguage As String

		Public Property UserMDTelefon As String
		Public Property UserMDDTelefon As String
		Public Property UserMDTelefax As String
		Public Property UserMDeMail As String
		Public Property UserMDGuid As String

		Public Property UserMDName As String
		Public Property UserMDName2 As String
		Public Property UserMDName3 As String
		Public Property UserMDPostfach As String
		Public Property UserMDStrasse As String
		Public Property UserMDPLZ As String
		Public Property UserMDOrt As String
		Public Property UserMDCanton As String
		Public Property UserMDLand As String
		Public Property UserMDHomepage As String
		Public Property EMail_UserName As String
		Public Property EMail_UserPW As String
		Public Property EMail_SMTP As String
		Public Property Deactivated As Boolean?

		Public ReadOnly Property UserFullnameWithoutComma
			Get
				Return String.Format("{0} {1}", UserFName, UserLName)
			End Get
		End Property

		Public ReadOnly Property UserFullname
			Get
				Return String.Format("{1}, {0}", UserFName, UserLName)
			End Get
		End Property

	End Class

	Public Class EMailSettingData

		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property Report_Senders As String
		Public Property CV_Senders As String
		Public Property Report_Recipients As String
		Public Property CV_Recipients As String
		Public Property Report_FTPUser As String
		Public Property CV_FTPUser As String
		Public Property Report_FTPPW As String
		Public Property CV_FTPPW As String
		Public Property Report_FTPRD As String
		Public Property CV_FTPRD As String
		Public Property MailUserName As String
		Public Property MailPassword As String
		Public Property SmtpServer As String
		Public Property SmtpPort As Integer?
		Public Property ActivateSSL As Boolean?
		Public Property TemplateFolder As String

		Public Property UploadForWhat As UploadEnum
		Public Property PriorityModul As PriorityModulEnum

		Public Enum UploadEnum
			ReportUpload
			CVUpload
		End Enum

		Public Enum PriorityModulEnum
			NOTDEFINED
			MAILTEMPLATE
			CVL
		End Enum

	End Class


	Public Class EMailData
		Public Property ID As Integer?
		Public Property ApplicationID As Integer?
		Public Property EmployeeID As Integer?
		Public Property Customer_ID As String
		Public Property EMailUidl As Integer?
		Public Property EMailFrom As String
		Public Property EMailTo As String
		Public Property EMailSubject As String
		Public Property EMailBody As String
		Public Property EMailPlainTextBody As String
		Public Property HasHtmlBody As Boolean?
		Public Property EMailDate As DateTime?
		Public Property EMailAttachment As List(Of EMailAttachment)
		Public Property EMailMime As String
		Public Property EMailContent As Byte()
		Public Property EMLFilename As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

		Public ReadOnly Property ExistsAttachment() As Boolean
			Get
				Return EMailAttachment.Count > 0
			End Get
		End Property

	End Class


	Public Class EMailAttachment
		Public Property ID As Integer?
		Public Property FK_REID As Integer?
		Public Property DocumentCategoryNumber As Integer?
		Public Property AttachmentSize As Byte()
		Public Property AttachmentName As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class


	Public Class EMailPatternSettingData

		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property CustomerID As String
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property Street As String
		Public Property Location As String
		Public Property Gender As String
		Public Property PostofficeBox As String
		Public Property Postcode As String
		Public Property Country As String
		Public Property Nationality As String
		Public Property EMail As String
		Public Property Telephone As String
		Public Property Birthdate As String
		Public Property Permission As String
		Public Property Profession As String
		Public Property OtherProfession As String
		Public Property MobilePhone As String
		Public Property Auto As String
		Public Property Motorcycle As String
		Public Property Bicycle As String
		Public Property DrivingLicence1 As String
		Public Property DrivingLicence2 As String
		Public Property DrivingLicence3 As String
		Public Property CivilState As String
		Public Property Language As String
		Public Property LanguageLevel As String
		Public Property VacancyCustomerID As String
		Public Property VacancyNumber As String
		Public Property ApplicationLabel As String
		Public Property Advisor As String
		Public Property BusinessBranch As String
		Public Property Dismissalperiod As String
		Public Property Availability As String
		Public Property Comment As String
		Public Property Attachment_CV As String
		Public Property Attachment_1 As String
		Public Property Attachment_2 As String
		Public Property Attachment_3 As String
		Public Property Attachment_4 As String
		Public Property Attachment_5 As String

	End Class


End Namespace
