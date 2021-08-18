
Namespace WOSUtility.DataObjects

	Public Class WOSEMailNotificationData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property MailFrom As String
		Public Property MailTo As String
		Public Property Result As String
		Public Property MailSubject As String
		Public Property MailBody As String
		Public Property DocLink As String
		Public Property RecipientGuid As String

		Public Property CreatedOn As DateTime?

	End Class

	Public Class WOSSearchResultData
		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property WOSGuid As String
		Public Property AssignedDocNumber As Integer
		Public Property EmploymentNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property CRepesponsibleNumber As Integer?
		Public Property ProposeNr As Integer?
		Public Property DocGuid As String
		Public Property CreatedOn As DateTime?
		Public Property DocViewedOn As DateTime?
		Public Property DocViewResult As Integer?
		Public Property Get_On As DateTime?
		Public Property GetResult As Integer?
		Public Property CustomerFeedback_On As DateTime?
		Public Property CustomerFeedback As String
		Public Property NotifyAdvisor As Boolean

	End Class

	Public Class WOSVacancySearchResultData
		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property WOSGuid As String
		Public Property KDNr As Integer?
		Public Property VakNr As Integer?
		Public Property VacancyLable As String
		Public Property CreatedOn As DateTime?
		Public Property IsOwnePlattformOnline As Boolean?
		Public Property JobChannelPriority As Boolean?
		Public Property IsJobsCHOnline As Boolean?
		Public Property IsOstJobOnline As Boolean?

	End Class

	Public Class WOSSendSetting

		Public Enum DocumentArt

			Arbeitgeberbescheinigung
			Zwischenverdienstformular
			Einsatzvertrag
			Rapport
			Lohnabrechnung
			Lohnausweis

			Verleihvertrag
			Rechnung
			Vorschlag

		End Enum

		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property CresponsibleNumber As Integer?
		Public Property EmploymentNumber As Integer?
		Public Property ProposeNumber As Integer?
		Public Property ESLohnNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property ReportLineNumber As Integer?
		Public Property ReportDocumentNumber As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property PayrollNumber As Integer?

		Public Property EmployeeGuid As String
		Public Property CustomerGuid As String
		Public Property CresponsibleGuid As String
		Public Property CustomerDocumentGuid As String

		Public Property AssignedDocumentGuid As String
		Public Property EmploymentGuid As String
		Public Property ReportGuid As String
		Public Property InvoiceGuid As String
		Public Property ScanDoc As Byte()
		Public Property ScanDocName As String

		Public Property DocumentInfo As String
		Public Property DocumentArtEnum As DocumentArt

		Public Property SignTransferedDocument As Boolean?

	End Class


	Public Class WOSSendResult

		Public Property Value As Boolean
		Public Property Message As String


	End Class

End Namespace
