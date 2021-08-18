
Namespace Employee.DataObjects.DocumentMng

	''' <summary>
	''' Employee document category data (MA_LLDoc).
	''' </summary>
	Public Class EmployeeDocumentData

		Public Property ID As Integer
		Public Property DocumentRecordNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property DocPath As String
		'Name -> Bezeichnung
		Public Property Name As String
		Public Property Description As String
		Public Property ScanExtension As String
		Public Property FileFullPath As String
		Public Property USNr As Integer?
		Public Property CategorieNumber As Integer?
		Public Property Pages As Integer?
		Public Property FileSize As Integer?
		Public Property PlainText As String
		Public Property DocXML As String
		Public Property FileHashvalue As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

		Public Property CategoryName As String
	End Class

	Public Class EmployeeCVData

		Public Property ID As Integer
		Public Property EmployeeNumber As Integer?

		'Name -> Bezeichnung
		Public Property Name As String
		Public Property Description As String
		Public Property ScanExtension As String
		Public Property FileFullPath As String
		Public Property USNr As Integer?
		Public Property ReserveRTFContent As String
		Public Property ReserveTextContent As String
		Public Property DocumentContent As Byte()
		Public Property PDFContent As Byte()
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

	End Class

	Public Class EmployeeWOSData

		Public Property EmployeeWOSID As String

		Public Property EmployeeNumber As Integer?
		Public Property EmploymentNumber As Integer?
		Public Property EmploymentLineNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property ReportLlineNumber As Integer?
		Public Property ReportDocNumber As Integer?
		Public Property PayrollNumber As Integer?

		Public Property MATransferedGuid As String
		Public Property ESDoc_Guid As String
		Public Property RPDoc_Guid As String
		Public Property PayrollDoc_Guid As String
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

		Public Property MA_Nachname As String
		Public Property MA_Vorname As String
		Public Property MA_Postfach As String
		Public Property MA_Strasse As String
		Public Property MA_PLZ As String
		Public Property MA_Ort As String
		Public Property MA_Land As String
		Public Property MA_Filiale As String
		Public Property MA_Berater As String
		Public Property MA_Email As String
		Public Property MA_AGB_Wos As String
		Public Property MA_Beruf As String
		Public Property MA_Branche As String
		Public Property MA_Language As String
		Public Property MA_GebDat As DateTime?
		Public Property MA_Gender As String
		Public Property MA_BriefAnrede As String
		Public Property MA_Zivil As String
		Public Property MA_Nationality As String
		Public Property MA_FSchein As String
		Public Property MA_Auto As String
		Public Property MA_Kontakt As String
		Public Property MA_State1 As String
		Public Property MA_State2 As String
		Public Property MA_Eigenschaft As String
		Public Property MA_SSprache As String
		Public Property MA_MSprache As String

		Public Property AHV_Nr As String
		Public Property MA_Canton As String

		Public Property AssignedDocumentGuid As String
		Public Property AssignedDocumentArtName As String
		Public Property AssignedDocumentInfo As String
		Public Property ScanDoc As Byte()
		Public Property ScanDocName As String

	End Class

	Public Class AvailableEmployeeWOSData

		Public Property Customer_ID As String
		Public Property EmployeeWOSID As String
		Public Property EmployeeNumber As Integer?
		Public Property MATransferedGuid As String
		Public Property LogedUserID As String
		Public Property MA_Nachname As String
		Public Property MA_Vorname As String
		Public Property MA_Postfach As String
		Public Property MA_Strasse As String
		Public Property MA_PLZ As String
		Public Property MA_Ort As String
		Public Property MA_Land As String
		Public Property MA_Filiale As String
		Public Property MA_Berater As String
		Public Property MA_Email As String
		Public Property MA_AGB_Wos As String
		Public Property MA_Beruf As String
		Public Property MA_Branche As String
		Public Property JobProzent As String
		Public Property Permit As String
		Public Property MA_Language As String
		Public Property MA_GebDat As DateTime?
		Public Property MA_Gender As String
		Public Property Salutation As String
		Public Property MA_Zivil As String
		Public Property MA_Nationality As String
		Public Property MA_FSchein As String
		Public Property MA_Auto As String
		Public Property MA_Kontakt As String
		Public Property MA_State1 As String
		Public Property MA_State2 As String
		Public Property MA_Eigenschaft As String
		Public Property MA_SSprache As String
		Public Property MA_MSprache As String
		Public Property MA_Canton As String

		Public Property MA_Res1 As String
		Public Property MA_Res2 As String
		Public Property MA_Res3 As String
		Public Property MA_Res4 As String
		Public Property MA_Res5 As String
		Public Property LL_Name As String
		Public Property Reserve0 As String
		Public Property Reserve1 As String
		Public Property Reserve2 As String
		Public Property Reserve3 As String
		Public Property Reserve4 As String
		Public Property Reserve5 As String
		Public Property Reserve6 As String
		Public Property Reserve7 As String
		Public Property Reserve8 As String
		Public Property Reserve9 As String
		Public Property Reserve10 As String
		Public Property Reserve11 As String
		Public Property Reserve12 As String
		Public Property Reserve13 As String
		Public Property Reserve14 As String
		Public Property Reserve15 As String
		Public Property ReserveRtf0 As String
		Public Property ReserveRtf1 As String
		Public Property ReserveRtf2 As String
		Public Property ReserveRtf3 As String
		Public Property ReserveRtf4 As String
		Public Property ReserveRtf5 As String
		Public Property ReserveRtf6 As String
		Public Property ReserveRtf7 As String
		Public Property ReserveRtf8 As String
		Public Property ReserveRtf9 As String
		Public Property ReserveRtf10 As String
		Public Property ReserveRtf11 As String
		Public Property ReserveRtf12 As String
		Public Property ReserveRtf13 As String
		Public Property ReserveRtf14 As String
		Public Property ReserveRtf15 As String
		Public Property EmployeeTemplates As List(Of AvailableEmployeeTemplateData)

		Public Property DesiredWagesOld As Decimal?
		Public Property DesiredWagesNew As Decimal?
		Public Property DesiredWagesInMonth As Decimal?
		Public Property DesiredWagesInHour As Decimal?


	End Class

	Public Class AvailableEmployeeTemplateData
		Public Property ID As Integer?
		Public Property EmployeeNumber As Integer
		Public Property ScanDoc As Byte()

	End Class

	Public Class LLZusatzFieldsData

		Public Property ID As Integer
		Public Property RecNr As Integer
		Public Property GroupNr As Integer
		Public Property DbFieldName As String
		Public Property ShowInMAVersand As Boolean
		Public Property ShowInProposeNavBar As Boolean
		Public Property Bezeichnung As String
		Public Property FileName As String
		Public Property ModulName As String

	End Class

	Public Class LLZusatzFieldsTemplateData

			Public Property ID As Integer
			Public Property RecNr As Integer
			Public Property DbFieldName As String
			Public Property Bezeichnung As String
			Public Property FileName As String


		End Class


End Namespace