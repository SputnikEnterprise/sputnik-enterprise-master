
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.EMailJob.DataObjects
Imports System.Text
'Imports System.Transactions


Namespace EMailJob


	Partial Class EMailJobDatabaseAccess


		Inherits DatabaseAccessBase
		Implements IEmailJobDatabaseAccess


		Function LoadEMailSettingData(ByVal eMailData As EMailData) As EMailSettingData Implements IEMailJobDatabaseAccess.LoadEMailSettingData
			Dim result As EMailSettingData = Nothing

			Dim sql As String
			sql = "[Load EMail Setting Data]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("EMailFrom", ReplaceMissing(eMailData.EMailFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailTo", ReplaceMissing(eMailData.EMailTo, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New EMailSettingData

					While reader.Read
						Dim data = New EMailSettingData

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.Customer_ID = SafeGetString(reader, "Customer_ID")
						result.ActivateSSL = SafeGetBoolean(reader, "ActivateSSL", Nothing)
						result.CV_FTPPW = SafeGetString(reader, "CV_FTPPW")
						result.CV_FTPRD = SafeGetString(reader, "CV_FTPRD")
						result.CV_FTPUser = SafeGetString(reader, "CV_FTPUser")
						result.CV_Recipients = SafeGetString(reader, "CV_Recipients")
						result.CV_Senders = SafeGetString(reader, "CV_Senders")
						result.MailPassword = SafeGetString(reader, "MailPassword")
						result.MailUserName = SafeGetString(reader, "MailUserName")
						result.Report_FTPPW = SafeGetString(reader, "Report_FTPPW")
						result.Report_FTPRD = SafeGetString(reader, "Report_FTPRD")
						result.Report_FTPUser = SafeGetString(reader, "Report_FTPUser")
						result.Report_Recipients = SafeGetString(reader, "Report_Recipients")
						result.Report_Senders = SafeGetString(reader, "Report_Senders")
						result.SmtpPort = SafeGetInteger(reader, "SmtpPort", Nothing)
						result.SmtpServer = SafeGetString(reader, "SmtpServer")
						result.TemplateFolder = SafeGetString(reader, "TemplateFolder")

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function LoadEMailSettingForParsingData(ByVal customerID As String, ByVal whatToUpload As EMailSettingData.UploadEnum) As EMailSettingData Implements IEMailJobDatabaseAccess.LoadEMailSettingForParsingData
			Dim result As EMailSettingData = Nothing

			Dim sql As String
			If whatToUpload = EMailSettingData.UploadEnum.CVUpload Then
				sql = "[Load EMail Setting Data For CVParsing]"
			Else
				sql = "[Load EMail Setting Data For ReportParsing]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New EMailSettingData

					While reader.Read
						Dim data = New EMailSettingData

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.Customer_ID = SafeGetString(reader, "Customer_ID")
						result.ActivateSSL = SafeGetBoolean(reader, "ActivateSSL", Nothing)
						result.CV_FTPPW = SafeGetString(reader, "CV_FTPPW")
						result.CV_FTPRD = SafeGetString(reader, "CV_FTPRD")
						result.CV_FTPUser = SafeGetString(reader, "CV_FTPUser")
						result.CV_Recipients = SafeGetString(reader, "CV_Recipients")
						result.CV_Senders = SafeGetString(reader, "CV_Senders")
						result.MailPassword = SafeGetString(reader, "MailPassword")
						result.MailUserName = SafeGetString(reader, "MailUserName")
						result.Report_FTPPW = SafeGetString(reader, "Report_FTPPW")
						result.Report_FTPRD = SafeGetString(reader, "Report_FTPRD")
						result.Report_FTPUser = SafeGetString(reader, "Report_FTPUser")
						result.Report_Recipients = SafeGetString(reader, "Report_Recipients")
						result.Report_Senders = SafeGetString(reader, "Report_Senders")
						result.SmtpPort = SafeGetInteger(reader, "SmtpPort", Nothing)
						result.SmtpServer = SafeGetString(reader, "SmtpServer")
						result.TemplateFolder = SafeGetString(reader, "TemplateFolder")

						Dim modulPriority As Integer
						modulPriority = SafeGetInteger(reader, "PriorityModul", 0)
						result.PriorityModul = CType(modulPriority, EMailSettingData.PriorityModulEnum)

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function LoadEMailUserData(ByVal eMailData As EMailData) As EmailUserData Implements IEMailJobDatabaseAccess.LoadEMailUserData
			Dim result As EmailUserData = Nothing

			Dim sql As String
			sql = "[Load User Data For Received EMail]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("EMailFrom", ReplaceMissing(eMailData.EMailFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMailTo", ReplaceMissing(eMailData.EMailTo, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (reader IsNot Nothing AndAlso reader.Read()) Then

					result = New EmailUserData

					result.Customer_ID = SafeGetString(reader, "Customer_ID")
					result.User_ID = SafeGetString(reader, "User_ID")
					result.UserFName = SafeGetString(reader, "Firstname")
					result.UserLName = SafeGetString(reader, "Lastname")
					result.UserBranchOffice = SafeGetString(reader, "Branchoffice")

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		Function LoadEMailPatternParsingData(ByVal customerID As String) As EMailPatternSettingData Implements IEMailJobDatabaseAccess.LoadEMailPatternParsingData
			Dim result As EMailPatternSettingData = Nothing

			Dim sql As String
			sql = "[Load EMail Pattern For BodyParsing Data]"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New EMailPatternSettingData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.Customer_ID = SafeGetString(reader, "Customer_ID")
						result.CustomerID = SafeGetString(reader, "CustomerID")
						result.Lastname = SafeGetString(reader, "Lastname")
						result.Firstname = SafeGetString(reader, "Firstname")
						result.Gender = SafeGetString(reader, "Gender")
						result.Street = SafeGetString(reader, "Street")

						result.PostofficeBox = SafeGetString(reader, "PostOfficeBox")
						result.Postcode = SafeGetString(reader, "Postcode")
						result.Location = SafeGetString(reader, "Location")
						result.Country = SafeGetString(reader, "Country")
						result.Nationality = SafeGetString(reader, "Nationality")
						result.EMail = SafeGetString(reader, "EMail")
						result.Telephone = SafeGetString(reader, "Telephone")
						result.MobilePhone = SafeGetString(reader, "MobilePhone")
						result.Birthdate = SafeGetString(reader, "Birthdate")
						result.Permission = SafeGetString(reader, "Permission")
						result.Profession = SafeGetString(reader, "Profession")
						result.OtherProfession = SafeGetString(reader, "OtherProfession")
						result.Auto = SafeGetString(reader, "Auto")
						result.Motorcycle = SafeGetString(reader, "Motorcycle")
						result.Bicycle = SafeGetString(reader, "Bicycle")
						result.DrivingLicence1 = SafeGetString(reader, "DrivingLicence1")
						result.DrivingLicence2 = SafeGetString(reader, "DrivingLicence2")
						result.DrivingLicence3 = SafeGetString(reader, "DrivingLicence3")
						result.CivilState = SafeGetString(reader, "CivilState")
						result.Language = SafeGetString(reader, "Language")
						result.LanguageLevel = SafeGetString(reader, "LanguageLevel")
						result.VacancyCustomerID = SafeGetString(reader, "VacancyCustomerID")
						result.VacancyNumber = SafeGetString(reader, "VacancyNumber")
						result.ApplicationLabel = SafeGetString(reader, "ApplicationLabel")
						result.Advisor = SafeGetString(reader, "Advisor")
						result.BusinessBranch = SafeGetString(reader, "BusinessBranch")
						result.Dismissalperiod = SafeGetString(reader, "Dismissalperiod")
						result.Availability = SafeGetString(reader, "Availability")
						result.Comment = SafeGetString(reader, "Comment")
						result.Attachment_CV = SafeGetString(reader, "Attachment_CV")
						result.Attachment_1 = SafeGetString(reader, "Attachment_1")
						result.Attachment_2 = SafeGetString(reader, "Attachment_2")
						result.Attachment_3 = SafeGetString(reader, "Attachment_3")
						result.Attachment_4 = SafeGetString(reader, "Attachment_4")
						result.Attachment_5 = SafeGetString(reader, "Attachment_5")

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing
			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

	End Class


End Namespace
