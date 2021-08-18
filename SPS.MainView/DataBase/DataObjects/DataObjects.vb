


Public Class MDData

	Public Property MDName As String
	Public Property MDNr As Integer

End Class

Public Class UpdateProperty
	Public Property UpdateInfo As SPUpdateUtilitiesService.UpdateUtilitiesDTO
	Public Property RecID As Integer?
	Public Property UpdateFilename As String
	Public Property UpdateFileDate As DateTime?
	Public Property Username As String
	Public Property UpdateViewed As DateTime?

End Class


Public Class UserRights

	Public Property bRight101 As Boolean?
	Public Property bRight201 As Boolean?
	Public Property bRight701 As Boolean?
	Public Property bRight801 As Boolean?
	Public Property bRight250 As Boolean?
	Public Property bRight300 As Boolean?
	Public Property bRight349 As Boolean?
	Public Property bRight16 As Boolean?
	Public Property bRight14 As Boolean?
	Public Property bRight15 As Boolean?

	Public Property bRight100100 As Boolean?
	Public Property bRight601 As Boolean?
	Public Property bRight621 As Boolean?
	Public Property bRight603 As Boolean?
	Public Property bRight556 As Boolean?

	Public Property bRight557 As Boolean?
	Public Property bRight606 As Boolean?
	Public Property bRight100105 As Boolean?
	Public Property bRight607 As Boolean?
	Public Property bRight608 As Boolean?
	Public Property bRight610 As Boolean?
	Public Property bRight650 As Boolean?
	Public Property bRight552 As Boolean?

	Public Property bRight666 As Boolean?
	Public Property bRight659 As Boolean?
	Public Property bRight660 As Boolean?
	Public Property bRight654 As Boolean?

	Public Property bRight602 As Boolean?

	Public Property bRight673 As Boolean?

End Class

Public Class UserImageData

	Public Property UsrNr As Integer
	Public Property UserImage As Byte()

End Class


Public Class FoundedEmployeeData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property manr As Integer

	Public Property telefon_p As String
	Public Property natel As String
	Public Property maname As String

	Public Property strasse As String
	Public Property maplzort As String
	Public Property magebdat As Date?
	Public Property beruf As String
	Public Property maalterwithdate As String
	Public Property mabewilligung As String

	Public Property mastatus_1 As String
	Public Property mastatus_2 As String

	Public Property maqualifikation As String
	Public Property maemail As String

	Public Property tempmabild As Boolean
	Public Property md_guid As String
	Public Property zfiliale As String
	Public Property actives As Boolean?
	Public Property noes As Boolean?
	Public Property webexport As Boolean?

	Public Property beraterin As String

End Class


Public Class FoundedCustomerData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property kdnr As Integer

	Public Property firma1 As String
	Public Property kdzhdnr As String

	Public Property strasse As String
	Public Property kdplzort As String
	Public Property fproperty As Integer?
	Public Property howkontakt As String

	Public Property kdstate1 As String
	Public Property kdstate2 As String

	Public Property createdon As Date?
	Public Property createdfrom As String

	Public Property kreditlimiteab As Date?
	Public Property kreditlimitebis As Date?

	Public Property kreditlimite As Decimal?
	Public Property kreditlimite_2 As Decimal?

	Public Property vorname As String
	Public Property nachname As String

	Public Property kdzname As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String
	Public Property kdberater As String

	Public Property ztelefon As String
	Public Property ztelefax As String
	Public Property zemail As String
	Public Property znatel As String

	Public Property actives As Boolean?
	Public Property noes As Boolean?
	Public Property zfiliale As String

	Public Property beraterin As String

End Class

Public Class FoundedVacancyData

	Public Property _res As String
	Public Property Customer_ID As String
	Public Property mdnr As Integer
	Public Property vaknr As Integer

	Public Property kdnr As Integer?
	Public Property kdzhdnr As Integer?

	Public Property vakstate As String
	Public Property vak_kanton As String
	Public Property vaklink As String
	Public Property vakkontakt As String
	Public Property vacancygruppe As String
	Public Property vacancyplz As String
	Public Property vacancyort As String
	Public Property titelforsearch As String
	Public Property shortdescription As String

	Public Property firma1 As String
	Public Property bezeichnung As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property changedon As DateTime?
	Public Property changedfrom As String

	Public Property kdzname As String
	Public Property advisor As String

	Public Property kdemail As String
	Public Property zemail As String

	Public Property jchisonline As Boolean?
	Public Property JobsCHExpire As Boolean?
	Public Property OstJobExpire As Boolean?
	Public Property FirstTransferDate As DateTime?
	Public Property ourisonline As Boolean?
	Public Property jobchannelpriority As JobplattformStateEnum ' Boolean?
	Public Property ojisonline As Boolean?

	Public Property AVAMRecordState As String
	Public Property AVAMJobroomID As String
	Public Property AVAMFrom As DateTime?
	Public Property AVAMUntil As DateTime?
	Public Property AVAMReportingDate As DateTime?
	Public Property AVAMReportingObligationEndDate As DateTime?
	Public Property AVAMReportingObligation As Boolean?


	Public Property kdtelefon As String
	Public Property kdtelefax As String

	Public Property ztelefon As String
	Public Property ztelefax As String
	Public Property znatel As String

	Public Property jobchdate As String
	Public Property ostjobchdate As String
	Public Property SyncDate As DateTime?
	Public Property SyncFrom As String
	Public Property zfiliale As String
	Public Property JobsChannelState As JobplattformStateEnum
	Public Property JobsCHWillbeExpireSoon As JobplattformEnum
	Public Property OstJobWillbeExpireSoon As JobplattformEnum

	Public Enum JobplattformEnum
		ONLINE = 1
		EXPIRING = 2
		OFFLINE = 3
	End Enum

	Public Enum JobplattformStateEnum
		ONLINE = 1
		OFFLINE = 3
	End Enum

	Public ReadOnly Property AVAMIsNowOnline As Boolean?

		Get
			If Not AVAMReportingObligationEndDate.HasValue Then
				Return False
			ElseIf AVAMReportingObligationEndDate > Now.Date Then
				Return True

			Else
				Return False
			End If
		End Get

	End Property

	Public ReadOnly Property AVAMStateEnum As AVAMState

		Get
			Dim value As AVAMState = Nothing 'MainView.AVAMState.INSPECTING

			If String.IsNullOrWhiteSpace(AVAMRecordState) Then Return Nothing
			If AVAMRecordState = "INSPECTING" Then value = MainView.AVAMState.INSPECTING
			If AVAMRecordState = "REJECTED" Then value = MainView.AVAMState.REJECTED
			If AVAMRecordState = "PUBLISHED_RESTRICTED" Then value = MainView.AVAMState.PUBLISHED_RESTRICTED
			If AVAMRecordState = "PUBLISHED_PUBLIC" Then value = MainView.AVAMState.PUBLISHED_PUBLIC
			If AVAMRecordState = "CANCELLED" Then value = MainView.AVAMState.CANCELLED
			If AVAMRecordState = "ARCHIVED" Then value = MainView.AVAMState.ARCHIVED


			Return value

		End Get

	End Property


End Class


Public Enum AVAMState

	REJECTED
	INSPECTING
	PUBLISHED_RESTRICTED
	PUBLISHED_PUBLIC
	CANCELLED
	ARCHIVED

End Enum

Public Class FoundedProposeData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property pnr As Integer
	Public Property manr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property vaknr As Integer?

	Public Property honorar As Decimal?
	Public Property part As String
	Public Property panstellung As String

	Public Property advisor As String
	Public Property firma1 As String
	Public Property zname As String

	Public Property maname As String
	Public Property bezeichnung As String

	Public Property createdon As Date?
	Public Property createdfrom As String

	Public Property pstate As String
	Public Property vakals As String
	Public Property vakcreatedon As Date?

	Public Property kdemail As String
	Public Property zemail As String

	Public Property jchisonline As Boolean?
	Public Property ourisonline As Boolean?
	Public Property ojisonline As Boolean?

	Public Property kdtelefon As String
	Public Property kdtelefax As String

	Public Property ztelefon As String
	Public Property ztelefax As String
	Public Property znatel As String

	Public Property zfiliale As String

End Class


'Public Class ApplicationData

'	Public Property _res As String
'	Public Property MDNr As Integer
'	Public Property CustomerMDNr As Integer?
'	Public Property ApplicationAdvisorLastName As String
'	Public Property ApplicationAdvisorFirstName As String
'	Public Property Applicationnumber As Integer?
'	Public Property Applicantnumber As Integer?
'	Public Property Customernumber As Integer?
'	Public Property Vacancynumber As Integer?

'	Public Property ApplicationLabel As String
'	Public Property ApplicantLastname As String
'	Public Property ApplicantFirstname As String
'	Public Property ApplicantStreet As String
'	Public Property ApplicantPostcode As String
'	Public Property ApplicantLocation As String
'	Public Property ApplicantCountry As String
'	Public Property Birthday As Date?

'	Public Property Customername As String
'	Public Property CustomerStreet As String
'	Public Property CustomerPostcode As String
'	Public Property CustomerLocation As String
'	Public Property CustomerCountry As String

'	Public Property Businessbrunch As String
'	Public Property Dismissalperiod As String
'	Public Property Availability As String
'	Public Property Comment As String

'	Public Property Createdon As Date?
'	Public Property Createdfrom As String
'	Public Property Checkedon As Date?
'	Public Property Checkedfrom As String
'	Public Property ApplicationLifecycle As Integer

'	Public Property zfiliale As String

'	Public Property Image As Image

'	Public ReadOnly Property ApplicationAdvisor
'		Get
'			Return String.Format("{0}, {1}", ApplicationAdvisorLastName, ApplicationAdvisorFirstName)
'		End Get
'	End Property

'	Public ReadOnly Property Applicantfullname
'		Get
'			Return String.Format("{0}, {1}", ApplicantLastname, ApplicantFirstname)
'		End Get
'	End Property

'	Public ReadOnly Property ApplicantAddress
'		Get
'			Return String.Format("{0}-{1} {2}", ApplicantCountry, ApplicantPostcode, ApplicantLocation)
'		End Get
'	End Property

'	Public ReadOnly Property CustomerAddress
'		Get
'			Return String.Format("{0}-{1} {2}", CustomerCountry, CustomerPostcode, CustomerLocation)
'		End Get
'	End Property


'	Public Enum LifecycelEnum

'		APPLICATIONNEW
'		APPLICATIONREJECTED
'		APPLICATIONVIEWED
'		APPLICATIONFORWARDED
'		PROPOSE
'		APPLICATIONCLOSED
'		APPLICATIONSUCCESS

'	End Enum

'End Class



Public Class FoundedEmployeeESDetailData

	Public Property mdnr As Integer
	Public Property esnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property periode As String
	Public Property customername As String
	Public Property employeename As String

	Public Property tarif As Decimal?
	Public Property stundenlohn As Decimal?
	Public Property margemitbvg As Decimal?
	Public Property margeohnebvg As Decimal?

	Public Property esals As String
	Public Property actives As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

'Public Class FoundedEmployeeContactDetailData

'	Public Property contactnr As Integer?
'	Public Property manr As Integer?
'	Public Property kdnr As Integer?

'	Public Property monat As Integer?
'	Public Property jahr As Integer?

'	Public Property customername As String
'	Public Property employeename As String

'	Public Property datum As DateTime?
'	Public Property bezeichnung As String
'	Public Property beschreibung As String

'	Public Property art As String

'	Public Property createdon As DateTime?
'	Public Property createdfrom As String

'End Class

Public Class FoundedEmployeeProposalDetailData

	Public Property mdnr As Integer?
	Public Property pnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property bezeichnung As String

	Public Property customername As String
	Public Property employeename As String
	Public Property zhdname As String

	Public Property p_art As String
	Public Property p_state As String

	Public Property advisor As String
	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedEmployeeSalaryDetailData

	Public Property employeeMDNr As Integer

	Public Property mdnr As Integer
	Public Property lonr As Integer?
	Public Property manr As Integer?

	Public Property monat As Integer?
	Public Property jahr As Integer?

	Public Property periode As String
	Public Property employeename As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedEmployeeZGDetailData

	Public Property employeeMDNr As Integer

	Public Property mdnr As Integer
	Public Property zgnr As Integer
	Public Property lonr As Integer
	Public Property rpnr As Integer?
	Public Property manr As Integer
	Public Property vgnr As Integer

	Public Property monat As Integer
	Public Property jahr As Integer
	Public Property zgperiode As String

	Public Property betrag As Decimal?
	Public Property aus_dat As Date?

	Public Property employeename As String

	Public Property lanr As Decimal?
	Public Property laname As String

	Public Property isout As Boolean
	Public Property isaslo As Boolean

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedEmployeeReportDetailData

	Public Property mdnr As Integer
	Public Property rpnr As Integer?
	Public Property kdnr As Integer?
	Public Property manr As Integer?
	Public Property esnr As Integer?
	Public Property lonr As Integer?

	Public Property monat As Integer?
	Public Property jahr As Integer?

	Public Property periode As String

	Public Property rpgav_beruf As String
	Public Property employeename As String
	Public Property customername As String

	Public Property rpdone As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class



Public Class FoundedCustomerESDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer
	Public Property mdnr As Integer
	Public Property esnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property periode As String
	Public Property customername As String
	Public Property employeename As String

	Public Property tarif As Decimal?
	Public Property stundenlohn As Decimal?
	Public Property margemitbvg As Decimal?
	Public Property margeohnebvg As Decimal?

	Public Property esals As String
	Public Property actives As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedCustomerReportDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer
	Public Property mdnr As Integer
	Public Property rpnr As Integer?
	Public Property kdnr As Integer?
	Public Property manr As Integer?
	Public Property esnr As Integer?
	Public Property lonr As Integer?

	Public Property monat As Integer?
	Public Property jahr As Integer?

	Public Property periode As String

	Public Property rpgav_beruf As String
	Public Property employeename As String
	Public Property customername As String

	Public Property rpdone As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

'Public Class FoundedCustomerContactDetailData

'	Public Property contactnr As Integer?
'	Public Property kdnr As Integer?
'	Public Property zhdnr As Integer?
'	Public Property manr As Integer?

'	Public Property monat As Integer?
'	Public Property jahr As Integer?

'	Public Property customername As String
'	Public Property zhdname As String
'	Public Property employeename As String

'	Public Property EmployeeNumbers As String
'	Public Property EmployeeNames As String
'	Public Property MoreEmployeesContacted As Boolean

'	Public Property datum As DateTime?

'	Public Property bezeichnung As String
'	Public Property beschreibung As String
'	Public Property art As String
'	Public Property kst As String

'	Public Property createdon As DateTime?
'	Public Property createdfrom As String

'End Class


''' <summary>
''' customer vacancies in property
''' </summary>
''' <remarks></remarks>
Public Class CustomerVacanciesProperty

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property vaknr As Integer

	Public Property kdnr As Integer?
	Public Property kdzhdnr As Integer?

	Public Property vakstate As String
	Public Property vak_kanton As String
	Public Property vaklink As String
	Public Property vakkontakt As String
	Public Property vacancygruppe As String
	Public Property vacancyplz As String
	Public Property vacancyort As String
	Public Property titelforsearch As String
	Public Property shortdescription As String

	Public Property firma1 As String
	Public Property bezeichnung As String

	Public Property createdon As Date?
	Public Property createdfrom As String

	Public Property kdzname As String
	Public Property advisor As String

	Public Property kdemail As String
	Public Property zemail As String

	Public Property jchisonline As Boolean?
	Public Property ourisonline As Boolean?
	Public Property ojisonline As Boolean?


	Public Property kdtelefon As String
	Public Property kdtelefax As String

	Public Property ztelefon As String
	Public Property ztelefax As String
	Public Property znatel As String

	Public Property jobchdate As String
	Public Property ostjobchdate As String

	Public Property zfiliale As String

End Class


Public Class FoundedCustomerProposalDetailData

	Public Property mdnr As Integer?
	Public Property pnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property bezeichnung As String

	Public Property customername As String
	Public Property employeename As String
	Public Property zhdname As String

	Public Property p_art As String
	Public Property p_state As String

	Public Property advisor As String
	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedCustomerInvoiceDetailData

	Public Property customerMDNr As Integer
	Public Property mdnr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer
	Public Property fbmonth As Integer?

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property printdate As Date?
	Public Property faelligdate As Date?

	Public Property betragink As Decimal?
	Public Property betragex As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property bezahlt As Decimal?
	Public Property offen As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String
	Public Property isopen As Boolean?

	Public Property reart1 As String
	Public Property reart2 As String
	Public Property zahlkond As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedCustomerROPDetailData

	Public Property customerMDNr As Integer
	Public Property mdnr As Integer
	Public Property zenr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property faelligdate As Date?

	Public Property valutadate As Date?
	Public Property buchungdate As Date?

	Public Property betragink As Decimal?
	Public Property zebetrag As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedVacancyESDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property mdnr As Integer
	Public Property esnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property periode As String
	Public Property customername As String
	Public Property employeename As String

	Public Property tarif As Decimal?
	Public Property stundenlohn As Decimal?
	Public Property margemitbvg As Decimal?
	Public Property margeohnebvg As Decimal?

	Public Property esals As String
	Public Property actives As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedVacancyProposalDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property mdnr As Integer?
	Public Property pnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property bezeichnung As String

	Public Property customername As String
	Public Property employeename As String
	Public Property zhdname As String

	Public Property p_art As String
	Public Property p_state As String

	Public Property advisor As String
	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedOfferData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property ofnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property employeename As String
	Public Property bezeichnung As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String

	Public Property offerstate As String

	Public Property customername As String
	Public Property zname As String
	Public Property customeremail As String
	Public Property customeraddress As String
	Public Property customerstreet As String
	Public Property zmobile As String
	Public Property ztelefon As String
	Public Property zemail As String
	Public Property customertelefon As String
	Public Property customertelefax As String

	Public Property advisor As String

	Public Property zfiliale As String

End Class


Public Class FoundedESData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property gavnumber As Integer
	Public Property esnr As Integer
	Public Property manr As Integer
	Public Property kdnr As Integer
	Public Property zhdnr As Integer?
	Public Property vaknr As Integer
	Public Property proposenr As Integer?

	Public Property eskst1 As String
	Public Property eskst2 As String
	Public Property eskst As String
	Public Property employeeadvisor As String
	Public Property employeebusinessbranch As String
	Public Property customeradvisor As String
	Public Property customerbusinessbranch As String
	Public Property esadvisor As String

	Public Property employeename As String
	Public Property employeeemail As String
	Public Property employeemobile As String
	Public Property employeeaddress As String
	Public Property employeestreet As String
	Public Property employeetelfon As String

	Public Property customername As String
	Public Property zname As String
	Public Property customeremail As String
	Public Property customermobile As String
	Public Property customeraddress As String
	Public Property customerstreet As String
	Public Property zmobile As String
	Public Property ztelefon As String
	Public Property zemail As String
	Public Property customertelefon As String

	Public Property es_als As String
	Public Property es_ab As Date?
	Public Property es_ende As Date?

	Public Property kreditlimite As Decimal?
	Public Property kreditlimite_2 As Decimal?
	Public Property kreditlimiteab As Date?
	Public Property kreditlimitebis As Date?
	Public Property kreditwarnung As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property lobis As DateTime?
	Public Property lovon As DateTime?

	Public Property tarif As Decimal?
	Public Property grundlohn As Decimal?
	Public Property stundenlohn As Decimal?
	Public Property ferienproz As Decimal?
	Public Property feierproz As Decimal?
	Public Property lohn13proz As Decimal?
	Public Property mastdspesen As Decimal?
	Public Property matspesen As Decimal?
	Public Property kdtspesen As Decimal?
	Public Property bruttomarge As Decimal?

	Public Property gavkanton As String
	Public Property gavbezeichnung As String
	Public Property gavfar As String
	Public Property actives As Boolean?

	Public Property proposeals As String
	Public Property proposecreatedon As Date?
	Public Property proposestatus As String

	Public Property zfiliale As String

	Public Property beraterin As String
	Public Property PrintNoRP As Boolean?
	Public Property gavstate As Boolean
	Public Property gavnotification As String


	Public ReadOnly Property StartMonth As Integer?
		Get
			Return Month(es_ab)
		End Get
	End Property

	Public ReadOnly Property StartYear As Integer?
		Get
			Return Year(es_ab)
		End Get
	End Property

	Public ReadOnly Property EndMonth As Integer?
		Get
			If es_ende.HasValue Then Return Month(es_ende)
			Return 12
		End Get
	End Property

	Public ReadOnly Property EndYear As Integer?
		Get
			If es_ende.HasValue Then Return Year(es_ende)
			Return Nothing
		End Get
	End Property

End Class


Public Class FoundedReportData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property esMDNr As Integer
	Public Property loMDNr As Integer

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property rpnr As Integer?
	Public Property lonr As Integer?

	Public Property esnr As Integer
	Public Property manr As Integer
	Public Property kdnr As Integer

	Public Property rpmonat As Integer
	Public Property rpjahr As Integer

	Public Property rpperiode As String
	Public Property es_als As String
	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zhdnr As Integer?
	Public Property vaknr As Integer?

	Public Property customername As String
	Public Property customerstreet As String
	Public Property customeraddress As String
	Public Property customeremail As String
	Public Property customertelefon As String
	Public Property customertelefax As String

	Public Property zname As String
	Public Property zemail As String
	Public Property ztelefon As String
	Public Property zmobile As String

	Public Property gavkanton As String
	Public Property rpgav_beruf As String
	Public Property gavbezeichnung As String
	Public Property gavfar As String
	Public Property gavparifond As String

	Public Property rpdone As Boolean?
	Public Property PrintNoRP As Boolean?

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeeaddress As String
	Public Property employeetelfon As String
	Public Property employeemobile As String
	Public Property employeeemail As String

	Public Property rpkst1 As String
	Public Property rpkst2 As String
	Public Property rpkst As String
	Public Property employeeadvisor As String
	Public Property customeradvisor As String
	Public Property rpadvisor As String

	Public Property es_ab As Date?
	Public Property es_ende As Date?

	Public Property zfiliale As String

	Public Property gavnumber As Integer
	Public Property von As DateTime?
	Public Property bis As DateTime?
	Public Property lovon As DateTime?

	Public Property gavstate As Boolean
	Public Property gavnotification As String


End Class

Public Class FoundedReportZGDetailData

	Public Property employeeMDNr As Integer

	Public Property mdnr As Integer
	Public Property zgnr As Integer
	Public Property lonr As Integer
	Public Property rpnr As Integer?
	Public Property manr As Integer
	Public Property vgnr As Integer

	Public Property monat As Integer
	Public Property jahr As Integer
	Public Property zgperiode As String

	Public Property betrag As Decimal?
	Public Property aus_dat As Date?

	Public Property employeename As String

	Public Property lanr As Decimal?
	Public Property laname As String

	Public Property isout As Boolean
	Public Property isaslo As Boolean

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedReportInvoiceDetailData

	Public Property customerMDNr As Integer

	Public Property mdnr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer
	Public Property fbmonth As Integer?

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property printdate As Date?
	Public Property faelligdate As Date?

	Public Property betragink As Decimal?
	Public Property betragex As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?
	Public Property isopen As Boolean?

	Public Property bezahlt As Decimal?
	Public Property offen As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String
	Public Property zahlkond As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class




Public Class FoundedVacanciesProposalDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property mdnr As Integer?
	Public Property pnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property bezeichnung As String

	Public Property customername As String
	Public Property employeename As String
	Public Property zhdname As String

	Public Property p_art As String
	Public Property p_state As String

	Public Property advisor As String
	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedProposalESDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property mdnr As Integer
	Public Property esnr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?
	Public Property manr As Integer?

	Public Property periode As String
	Public Property customername As String
	Public Property zhdname As String
	Public Property employeename As String

	Public Property tarif As Decimal?
	Public Property stundenlohn As Decimal?
	Public Property margemitbvg As Decimal?
	Public Property margeohnebvg As Decimal?

	Public Property esals As String
	Public Property actives As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class

Public Class FoundedProposalInterviewDetailData

	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property recid As Integer?
	Public Property recnr As Integer?
	Public Property employeenumber As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?

	Public Property datum As DateTime?
	Public Property jobtitel As String
	Public Property employeename As String
	Public Property zhdname As String
	Public Property customername As String

	Public Property jstate As String

	Public Property kst As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String

End Class

'Public Class FoundedProposalContactDetailData

'	Public Property employeeMDNr As Integer
'	Public Property customerMDNr As Integer

'	Public Property contactnr As Integer?
'	Public Property kdnr As Integer?
'	Public Property zhdnr As Integer?
'	Public Property manr As Integer?

'	Public Property monat As Integer?
'	Public Property jahr As Integer?

'	Public Property customername As String
'	Public Property zhdname As String
'	Public Property employeename As String

'	Public Property datum As String

'	Public Property bezeichnung As String
'	Public Property beschreibung As String
'	Public Property art As String
'	Public Property kst As String

'	Public Property createdon As DateTime?
'	Public Property createdfrom As String

'End Class



Public Class FoundedZGData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property zgnr As Integer
	Public Property lonr As Integer
	Public Property rpnr As Integer?
	Public Property manr As Integer
	Public Property vgnr As Integer

	Public Property monat As Integer
	Public Property jahr As Integer
	Public Property zgperiode As String

	Public Property createdon As Date?
	Public Property createdfrom As String

	Public Property lanr As Decimal?
	Public Property betrag As Decimal?
	Public Property aus_dat As Date?

  Public Property EmployeeFirstname As String
  Public Property EmployeeLastname As String
  Public Property Employeestreet As String
  Public Property Employeepostcode As String
  Public Property Employeelocation As String
  Public Property employeetelefon As String
  Public Property employeemobile As String
	Public Property employeeemail As String

	Public Property laname As String
	Public Property zfiliale As String

	Public Property isout As Boolean
	Public Property isaslo As Boolean

	Public ReadOnly Property EmployeeFullname
		Get
			Return (String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname))
		End Get
	End Property

	Public Property employeename As String
	'Public ReadOnly Property employeename
	'	Get
	'		Return (String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname))
	'	End Get
	'End Property

	Public ReadOnly Property employeeaddress As String
    Get
      Return (String.Format("{0}; {1} {2}", Employeestreet, Employeepostcode, Employeelocation))
    End Get
  End Property

End Class


Public Class FoundedSalaryData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property lonr As Integer
	Public Property manr As Integer
	Public Property zgnr As Integer?
	Public Property lmid As Integer?
	Public Property vgnr As Integer?

	Public Property monat As Integer?
	Public Property jahr As Integer?
	Public Property loperiode As String

	Public Property createdon As DateTime?
	Public Property createdfrom As String

	Public Property bruttobetrag As Decimal?
	Public Property zgbetrag As Decimal?
	Public Property lmbetrag As Decimal?
	Public Property lobetrag As Decimal?

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeeaddress As String
	Public Property maaddress As String
	Public Property employeetelefon As String
	Public Property employeemobile As String
	Public Property employeeemail As String

	'Public Property magebdat As Date?
	Public Property magebdat As String
	Public Property maalterwithdate As String

	Public Property mabewilligung As String
	Public Property maqualifikation As String
	Public Property IsComplete As Boolean?

	Public Property tempmabild As Boolean

End Class

Public Class FoundedSalaryDetailData

	Public Property MDNr As Integer

	Public Property modulname As String
	Public Property destrpnr As Integer
	Public Property destlmnr As Integer
	Public Property destzgnr As Integer
	Public Property destesnr As Integer?
	Public Property lonr As Integer?

	Public Property bezeichnung As String
	Public Property lanr As Decimal?

	Public Property betrag As Decimal?

End Class


Public Class FoundedCustomerBillData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property customermdnr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer
	Public Property fbmonth As Integer?

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property printdate As Date?
	Public Property faelligdate As Date?

	Public Property betragink As Decimal?
	Public Property betragex As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property bezahlt As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String
	Public Property zahlkond As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedCustomerROPData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property customermdnr As Integer
	Public Property zenr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property faelligdate As Date?

	Public Property valutadate As Date?
	Public Property buchungdate As Date?

	Public Property betragink As Decimal?
	Public Property zebetrag As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String

	Public Property zfiliale As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String

End Class


Public Class FoundedCustomerCreditBillData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer
	Public Property fbmonth As Integer?

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property printdate As Date?
	Public Property faelligdate As Date?
	Public Property buchungdate As Date?
	Public Property isdown As Boolean?

	Public Property betragink As Decimal?
	Public Property betragex As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property bezahlt As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String
	Public Property zahlkond As String

	Public Property zfiliale As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String

End Class


Public Class FoundedCustomerreminderData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer
	Public Property fbmonth As Integer?

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property printdate As Date?
	Public Property faelligdate As Date?

	Public Property reminder_0date As Date?
	Public Property reminder_1date As Date?
	Public Property reminder_2date As Date?
	Public Property reminder_3date As Date?

	Public Property betragink As Decimal?
	Public Property betragex As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property bezahlt As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String
	Public Property zahlkond As String

	Public Property zfiliale As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String

End Class



Public Class FoundedFOPData

	Public Property _res As String
	Public Property mdnr As Integer
	Public Property fopnr As Integer
	Public Property kdnr As Integer?
	Public Property manr As Integer?
	Public Property esnr As Integer?

	Public Property firma1 As String
	Public Property maname As String

	Public Property kdbranche As String
	Public Property kreditnr As Integer?
	Public Property krediton As Date?
	Public Property paidon As Date?

	Public Property esals As String
	Public Property esab As Date?
	Public Property esende As Date?

	Public Property betragex As Decimal?
	Public Property betragmwst As Decimal?
	Public Property betragtotal As Decimal?

	Public Property kreditname As String
	Public Property kst3 As String
	Public Property bemerkung As String

	Public Property zfiliale As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String

End Class


Public Class FoundedTODOData

	Public Property id As String
	Public Property recnr As Integer?
	Public Property mdnr As Integer

	Public Property advisor As String
	Public Property usnr As Integer?
	Public Property manr As Integer?
	Public Property kdnr As Integer?
	Public Property zhdnr As Integer?

	Public Property proposenr As Integer?
	Public Property vaknr As Integer?

	Public Property esnr As Integer?
	Public Property rpnr As Integer?

	Public Property lmnr As Integer?
	Public Property renr As Integer?
	Public Property zenr As Integer?

	Public Property subject As String
	Public Property body As String

	Public Property EmployeeLastname As String
	Public Property EmployeeFirstname As String
	Public Property Customername As String
	Public Property ZLastname As String
	Public Property ZFirstname As String
	Public Property ProposeLabel As String

	Public Property es_als As String
	Public Property es_ab As Date?
	Public Property es_ende As Date?

	Public Property importand As Boolean?
	Public Property done As Boolean?

	Public Property schedulebegins As Date?
	Public Property scheduleends As Date?
	Public Property schedulerememberin As Date?
	Public Property scheduleremember As Date?

	Public Property createdon As Date?
	Public Property createdfrom As String


	Public ReadOnly Property EmployeeFullname As String
		Get
			If manr.GetValueOrDefault(0) = 0 Then
				Return String.Empty
			Else
				Return String.Format("{1}, {0}", EmployeeFirstname, EmployeeLastname)
			End If
		End Get
	End Property

	Public ReadOnly Property ResponsibleFullname As String
		Get
			If zhdnr.GetValueOrDefault(0) = 0 Then
				Return String.Empty
			Else
				Return String.Format("{1}, {0}", ZFirstname, ZLastname)
			End If
		End Get
	End Property

End Class


Public Class FoundedESReportDetailData

	Public Property mdnr As Integer
	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property rpnr As Integer?
	Public Property kdnr As Integer?
	Public Property manr As Integer?
	Public Property esnr As Integer?
	Public Property lonr As Integer?

	Public Property monat As Integer?
	Public Property jahr As Integer?

	Public Property periode As String

	Public Property rpgav_beruf As String
	Public Property employeename As String
	Public Property customername As String

	Public Property rpdone As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedInvoiceReportDetailData

	Public Property mdnr As Integer
	Public Property employeeMDNr As Integer
	Public Property customerMDNr As Integer

	Public Property rpnr As Integer?
	Public Property kdnr As Integer?
	Public Property manr As Integer?
	Public Property esnr As Integer?
	Public Property lonr As Integer?

	Public Property monat As Integer?
	Public Property jahr As Integer?

	Public Property periode As String
	Public Property customeramount As Decimal?

	Public Property rpgav_beruf As String
	Public Property employeename As String
	Public Property customername As String

	Public Property rpdone As Boolean?

	Public Property createdon As DateTime?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


Public Class FoundedInvoiceROPDetailData

	Public Property customerMDNr As Integer
	Public Property mdnr As Integer
	Public Property zenr As Integer
	Public Property renr As Integer
	Public Property kdnr As Integer

	Public Property firma1 As String
	Public Property firma2 As String
	Public Property firma3 As String
	Public Property abteilung As String
	Public Property zhd As String

	Public Property postfach As String
	Public Property strasse As String
	Public Property plz As String
	Public Property ort As String
	Public Property plzort As String

	Public Property einstufung As String
	Public Property branche As String

	Public Property kdtelefon As String
	Public Property kdtelefax As String
	Public Property kdemail As String

	Public Property fakdate As Date?
	Public Property faelligdate As Date?

	Public Property valutadate As Date?
	Public Property buchungdate As Date?

	Public Property betragink As Decimal?
	Public Property zebetrag As Decimal?
	Public Property betragmwst As Decimal?
	Public Property mwstproz As Decimal?
	Public Property betragopen As Decimal?

	Public Property rekst1 As String
	Public Property rekst2 As String
	Public Property rekst As String

	Public Property reart1 As String
	Public Property reart2 As String

	Public Property employeeadvisor As String
	Public Property customeradvisor As String

	Public Property createdon As Date?
	Public Property createdfrom As String
	Public Property zfiliale As String

End Class


''' <summary>
''' Context menu data.
''' </summary>
Public Class ContextMenuForPrint
	Public Property MnuName As String
	Public Property MnuCaption As String
	Public Property MnuGrouped As Boolean?

End Class

''' <summary>
''' Context menu data.
''' </summary>
Public Class ContextMenuForNew
	Public Property MnuName As String
	Public Property MnuCaption As String
	Public Property MnuGrouped As Boolean?

End Class


''' <summary>
''' payroll print data.
''' </summary>
Public Class PayrollPrintData
	Public Property language As String

End Class


