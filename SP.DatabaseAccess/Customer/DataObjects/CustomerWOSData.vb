
Namespace Customer.DataObjects

	Public Class CustomerWOSData

		Public Property CustomerWOSID As String

		Public Property CustomerNumber As Integer?
		Public Property CresponsibleNumber As Integer?
		Public Property EmploymentNumber As Integer?
		Public Property EmploymentLineNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property ProposeNumber As Integer?

		Public Property KDTransferedGuid As String
		Public Property ZHDTransferedGuid As String
		Public Property ESDoc_Guid As String
		Public Property RPDoc_Guid As String
		Public Property REDoc_Guid As String
		Public Property CustomerMail As String
		Public Property customername As String
		Public Property CustomerStrasse As String
		Public Property CustomerOrt As String
		Public Property CustomerPLZ As String
		Public Property CustomerTelefon As String
		Public Property CustomerTelefax As String
		Public Property CustomerHomepage As String
		Public Property UserAnrede As String
		Public Property UserVorname As String
		Public Property UserName As String
		Public Property UserTelefon As String
		Public Property UserTelefax As String
		Public Property UserMail As String
		Public Property UserInitial As String
		Public Property UserSex As String
		Public Property UserFiliale As String
		Public Property UserSign As Byte()
		Public Property UserPicture As Byte()
		Public Property LogedUserID As String

		Public Property MDTelefon As String
		Public Property MD_DTelefon As String
		Public Property MDTelefax As String
		Public Property MDMail As String

		Public Property KD_Name As String
		Public Property KD_Postfach As String
		Public Property KD_Strasse As String
		Public Property KD_PLZ As String
		Public Property KD_Ort As String
		Public Property KD_Land As String
		Public Property KD_Filiale As String
		Public Property KD_Berater As String
		Public Property KD_Email As String
		Public Property KD_AGB_Wos As String
		Public Property KD_Beruf As String
		Public Property KD_Branche As String
		Public Property KD_Language As String
		Public Property DoNotShowContractInWOS As Boolean?
		Public Property ZHD_Vorname As String
		Public Property ZHD_Nachname As String
		Public Property ZHD_EMail As String
		Public Property ZHDSex As String
		Public Property Zhd_BriefAnrede As String
		Public Property Zhd_Berater As String
		Public Property Zhd_Beruf As String
		Public Property Zhd_Branche As String
		Public Property ZHD_AGB_Wos As String
		Public Property ZHD_GebDat As DateTime?

		Public Property AssignedDocumentGuid As String
		Public Property AssignedDocumentArtName As String
		Public Property AssignedDocumentInfo As String
		Public Property ScanDoc As Byte()
		Public Property ScanDocName As String

	End Class


End Namespace
