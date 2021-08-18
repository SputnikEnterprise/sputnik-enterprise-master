

Public Class MandantData
	Public Property ID As Integer?
	Public Property Customer_ID As String
	Public Property Customer_Name As String
	Public Property Recipients As String
	Public Property Report_Recipients As String
	Public Property bccAddresses As String
	Public Property MailSender As String
	Public Property MailUserName As String
	Public Property MailPassword As String
	Public Property SmtpServer As String
	Public Property SmtpPort As Integer
	Public Property ActivateSSL As Boolean
	Public Property TemplateFolder As String

End Class


Public Enum Language
	German = 0
	Italian = 1
	French = 2
	English = 3
End Enum


