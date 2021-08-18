
Namespace Listing.DataObjects

	''' <summary>
	''' listing data 
	''' </summary>
	Public Class ListingMailData

		Public Property ID As Integer?
		Public Property customer_id As String
		Public Property recNr As Integer?
		Public Property KDNr As Integer?
		Public Property ZHDNr As Integer?
		Public Property MANr As Integer?

		Public Property employeelastname As String
		Public Property employeefirstname As String
		Public Property customername As String
		Public Property cresponsiblesalution As String
		Public Property cresponsiblelastname As String
		Public Property cresponsiblefirstname As String

		Public Property email_to As String
		Public Property email_from As String
		Public Property email_subject As String
		Public Property email_body As String
		Public Property EMail_SMTP As String
		Public Property AsHTML As Boolean?
		Public Property AsTelefax As Boolean?
		Public Property messageID As String

		Public Property createdon As DateTime?
		Public Property createdfrom As String


		Public ReadOnly Property employeeFullname() As String
			Get
				Return String.Format(If(Not String.IsNullOrWhiteSpace(employeelastname), "{0}, {1}", ""), employeelastname, employeefirstname)
			End Get
		End Property

		Public ReadOnly Property responsiblePersonFullname() As String
			Get
				Return String.Format(If(Not String.IsNullOrWhiteSpace(cresponsiblelastname), "{0}, {1}", ""), cresponsiblelastname, cresponsiblefirstname)
			End Get
		End Property

	End Class

	Public Class OutgoingEMailData

		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property ModulNumber As ModulNumberEnum?
		Public Property Number As Integer?

		Public Property Receiver As String
		Public Property Sender As String

		Public Property CreatedOn As DateTime?
		Public Property CreatedUserNumber As Integer?
		Public Property CreatedFrom As String

		Public Enum ModulNumberEnum
			COMMON
			EMPLOYEE
			CUSTOMER
			VACANCY
			PROPOSE
			EMPLOYMENT
			REPORT
			INVOICE
			ADVANCEDPAYMENT
			PAYROLL
		End Enum

	End Class



	Public Class ListingMailAttachmentData

		Public Property ID As Integer?
		Public Property customer_id As String
		Public Property recNr As Integer?

		Public Property messageID As String
		Public Property filename As String
		Public Property scanfile As Byte()

		Public Property email_to As String
		Public Property email_from As String
		Public Property email_subject As String

		Public Property createdon As DateTime?
		Public Property createdfrom As String


	End Class


	Public Class EMailTemplateData
		Public Property ID As Integer
		Public Property DocumentLabel As String
		Public Property DocumentName As String

	End Class

	Public Class CustomerBulkEMailData
		'Public Property Anzrec As Integer
		Public Property KDNr As Integer?
		Public Property ZHDRecNr As Integer?
		Public Property Firma1 As String
		Public Property ReceiverEMail As String
		Public Property Anrede As String
		Public Property Nachname As String
		Public Property Vorname As String
		Public Property AnredeForm As String
		'Public Property ZHDeMail As String
		'Public Property ZHD_Mail_Mailing As Boolean
		Public Property NoMailing As Boolean

		Public Property Strasse As String
		Public Property PLZ As String
		Public Property Postfach As String
		Public Property Ort As String
		Public Property Land As String
		Public Property Telefon As String
		Public Property Telefax As String
		Public Property Telefax_Mailing As Boolean
		Public Property Abteilung As String
		Public Property ModulName As String


		Public Property Selected As Boolean

		Public ReadOnly Property CustomerResponsibleFullname() As String
			Get
				Return (String.Format("{0} {1} {2}", Anrede, Vorname, Nachname))
			End Get
		End Property

	End Class

	Public Class EMailOfferData
		Public Property OfferNumber As Integer
		Public Property OfferLabel As String

	End Class

	Public Class PrintJobData

		Public Property JobNumber As String
		Public Property LLDocName As String
		Public Property LLDocLabel As String
		Public Property LLFontDesent As Integer
		Public Property LLIncPrv As Integer
		Public Property LLParamCheck As Integer
		Public Property LLKonvertName As Integer
		Public Property LLZoomProz As Integer
		Public Property LLCopyCount As Integer
		Public Property LLExportedFilePath As String
		Public Property LLExportedFileName As String
		Public Property LLExportfilter As String
		Public Property LLExporterName As String
		Public Property LLExporterFileName As String
		Public Property LLPrintInDiffColor As Boolean

		Public ReadOnly Property PrintBoxHeader As String
			Get
				Return String.Format("{1}: {2}{0}{3}", vbNewLine, LLDocLabel, JobNumber, LLDocName)
			End Get
		End Property

	End Class

	Public Class UserAndMandantPrintData

		Public Property USNr As Integer
		Public Property USAnrede As String
		Public Property USVorname As String
		Public Property USNachname As String

		Public Property UserPicture As Byte()
		Public Property UserSign As Byte()
		Public Property USPostfach As String
		Public Property USStrasse As String
		Public Property USPLZ As String
		Public Property USLand As String
		Public Property USOrt As String
		Public Property Exchange_USName As String
		Public Property Exchange_USPW As String
		Public Property strUSSignFilename As String
		Public Property USTitel_1 As String
		Public Property USTitel_2 As String
		Public Property USAbteilung As String
		Public Property USeMail As String
		Public Property USTelefon As String
		Public Property USTelefax As String
		Public Property USNatel As String
		Public Property USMDname As String
		Public Property USMDname2 As String
		Public Property USMDname3 As String
		Public Property USMDPostfach As String
		Public Property USMDStrasse As String
		Public Property USMDOrt As String
		Public Property USMDPlz As String
		Public Property USMDLand As String
		Public Property USMDTelefon As String
		Public Property USMDDTelefon As String
		Public Property USMDTelefax As String
		Public Property USMDeMail As String
		Public Property USMDHomepage As String
		Public Property GetTestSearchQuery As String
		Public Property SelectedMAsBerufe As String
		Public Property bvgmaximallohnjahr As Decimal?
		Public Property bvgkoordinationlohnjahr As Decimal?
		Public Property bvgkoordinationlohnstd As Decimal?
		Public Property bvgminmallohnjahr As Decimal?
		Public Property bvgstd As Decimal?
		Public Property bvgwoche As Integer?


	End Class

	Public Class InvoiceData

		Public Property ID As Integer
		Public Property Art As String
		Public Property MDNr As Integer
		Public Property RENr As Integer
		Public Property KDNr As Integer
		Public Property DocGuid As String
		Public Property BetragInk As Decimal?
		Public Property Bezahlt As Decimal?
		Public Property REEmail As String
		Public Property SendAsZip As Boolean?

		Public Property InvoiceDate As Date?
		Public Property Send2WOS As Boolean?
		Public Property PrintWithReport As Boolean?
		Public Property CreditInvoiceAutomated As Boolean?
		Public Property OneInvoicePerMail As Boolean?
		Public Property Language As String
		Public Property NumberOfCopies As Integer
		Public Property RefNr As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property SelectedRec As Boolean?

		Public Property CustomerName As String
		Public Property CustomerGuid As String

	End Class


	Public Class CustomerCreditData
		Public Property CustomerNumber As Integer?
		Public Property FirstCreditlimit As Decimal?
		Public Property SecondCreditlimit As Decimal?

	End Class

	Public Class CustomerOpenAmountData
		Public Property CustomerNumber As Integer?
		Public Property OpenAmount As Decimal?

	End Class

	Public Class InvoiceReportData

		Public Property RPNr As Integer?
		Public Property KSTBez As String
		Public Property LABez As String
		Public Property Anzahl As Decimal?
		Public Property Betrag As Decimal?

	End Class


	Public Class InvoicePrintData

		Public Property Art As String
		Public Property RENr As Integer
		Public Property KDNr As Integer
		Public Property KST As String
		Public Property Fak_Dat As Date?
		Public Property ValutaData As Date?
		Public Property Currency As String

		Public Property BetragOhne As Decimal?
		Public Property BetragEx As Decimal?
		Public Property BetragInk As Decimal?
		Public Property Bezahlt As Decimal?
		Public Property MwSt1 As Decimal?
		Public Property Faellig As Date?
		Public Property R_Name1 As String
		Public Property R_Name2 As String
		Public Property R_Name3 As String
		Public Property R_ZHD As String
		Public Property R_Postfach As String
		Public Property R_Strasse As String
		Public Property R_Abteilung As String

		Public Property MwStProz As Decimal?

		Public Property PLZOrt As String
		Public Property R_Land As String
		Public Property RefNr As String
		Public Property RefFootNr As String
		Public Property ESRID As String
		Public Property ESArt As String
		Public Property ESRKonto As String
		Public Property KontoNr As String
		Public Property btrFr As Integer
		Public Property BtrRP As Integer
		Public Property ZahlKond As String

		Public Property MANr As Integer?
		Public Property ESNr As Integer?
		Public Property KSTNr As Integer?
		Public Property RPNr As Integer?
		Public Property RPLNr As Integer?
		Public Property LANr As Decimal?

		Public Property K_Anzahl As Decimal?
		Public Property K_Ansatz As Decimal?
		Public Property K_Basis As Decimal?
		Public Property K_Betrag As Decimal?
		Public Property MwSt As Decimal?


		Public Property VonDate As Date?
		Public Property BisDate As Date?
		Public Property KstBez As String

		Public Property RPZusatztext As String
		Public Property GAVGruppe0 As String

		Public Property GAV_FAG As Decimal?
		Public Property GAV_VAG As Decimal?
		Public Property GAV_WAG As Decimal?

		Public Property MAName As String
		Public Property OPVersand As String
		Public Property KDFirma1 As String
		Public Property KDFirma2 As String
		Public Property KDFirma3 As String
		Public Property KDPostfach As String
		Public Property KDStrasse As String
		Public Property KDPLZ As String
		Public Property KDOrt As String
		Public Property KDLand As String
		Public Property AnzKopien As Integer?
		Public Property KDMwStNr As String
		Public Property Send2WOS As Boolean?
		Public Property LAOPText As String


		Public Property RPText As String
		Public Property KST_Ort_PLZ As String
		Public Property KST_PK_PLZ As String
		Public Property KST_Res_Info_1 As String
		Public Property KST_Res_Info_2 As String

		Public Property DTAKontoNr As String
		Public Property IBANDTA As String
		Public Property ESRBankName As String
		Public Property ESRBankAdresse As String
		Public Property ESR_Swift As String
		Public Property ESR_IBAN1 As String
		Public Property ESR_IBAN2 As String
		Public Property ESR_IBAN3 As String

		Public Property ESR_BCNr As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property MDEMail As String
		Public Property MDHomepage As String

		Public Property Bemerk_RE As String
		Public Property Bemerk_LO As String
		Public Property Bemerk_P As String
		Public Property Bemerk_1 As String
		Public Property Bemerk_2 As String
		Public Property Bemerk_3 As String

		Public Property Ende As String

		Public Property KDZHDData As String
		Public Property DismissalOn As Date?
		Public Property DismissalFor As Date?
		Public Property DismissalKind As String
		Public Property DismissalWho As String
		Public Property KDZHDNr As Integer?
		Public Property RPVon As Date?
		Public Property RPBis As Date?
		Public Property WithInd As Boolean?

		Public Property IndMWST As Decimal?
		Public Property IndBetragEx As Decimal?
		Public Property BetragTotal As Decimal?
		Public Property RE_HeadText As String
		Public Property RE_Text As String

	End Class


	Public Class ReportForPrintInvoiceData
		Public Property ID As Integer
		Public Property RENr As Integer
		Public Property RPNr As Integer
		Public Property Beschreibung As String

		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property Extension As String
		Public Property DocScan As Byte()
		Public Property RPDoc_Guid As String
		Public Property RPLNr As Integer
		Public Property ESNr As Integer

	End Class


	Public Class RPSearchData

		Public Property MDNr As Integer
		Public Property RPNr As Integer()
		Public Property MANr As Integer()
		Public Property KDNr As Integer()
		Public Property ESNr As Integer()

		Public Property jahr As Integer?
		Public Property monat As Integer?
		Public Property kw1 As Integer?
		Public Property kw2 As Integer?


		Public Property kst1 As String
		Public Property kst2 As String
		Public Property kst3 As String

		Public Property filiale As String


	End Class


	Public Class RPPrintData

		Public Property MDNr As Integer
		Public Property RPNr As Integer?
		Public Property MANr As Integer?
		Public Property KDNr As Integer?
		Public Property ESNr As Integer?

		Public Property monat As Integer?
		Public Property jahr As Integer?

		Public Property von As Date?
		Public Property bis As Date?

		Public Property printedweeks As String


	End Class

	Public Class EmployeeCreditData
		Public Property BackedHours As Decimal?
		Public Property BackedAmount As Decimal?
		Public Property PayedHours As Decimal?
		Public Property PayedAmount As Decimal?

		Public ReadOnly Property CreditHours As Decimal?
			Get
				Return BackedHours.GetValueOrDefault(0) - PayedHours.GetValueOrDefault(0)

			End Get
		End Property

		Public ReadOnly Property CreditAmount As Decimal?
			Get
				Return BackedAmount.GetValueOrDefault(0) - PayedAmount.GetValueOrDefault(0)

			End Get
		End Property

	End Class

	Public Class PayrollSearchData

		Public Enum WOSValue
			WithWOS
			WithoutWOS
			All
		End Enum

		Public Property MDNr As Integer
		Public Property MANr As List(Of Integer)
		Public Property LONr As List(Of Integer)
		Public Property monat As List(Of Integer)
		Public Property jahr As List(Of Integer)

		Public Property mawos As WOSValue

		Public Property sortvalue As Integer
		Public Property GroupByEMail As Boolean?

	End Class

	Public Class PayrollListingSearchData

		Public Property MDNr As Integer
		Public Property MANr As List(Of Integer)
		Public Property LONr As List(Of Integer)
		Public Property monat As List(Of Integer)
		Public Property jahr As List(Of Integer)

		Public Property Canton As String

	End Class


	Public Class PayrollPrintData

		Public Property MDNr As Integer
		Public Property recID As Integer?
		Public Property LONr As Integer?
		Public Property MANr As Integer?

		Public Property monat As Integer?
		Public Property jahr As Integer?
		Public Property Send2WOS As Boolean?
		Public Property SendDataWithEMail As Boolean?
		Public Property SendAsZIP As Boolean?
		Public Property employeeLanguage As String

		Public Property LMID As Integer?
		Public Property lpVGNr As Integer?
		Public Property ZGNumber As Integer?
		Public Property PayrollGuid As String
		Public Property EmployeeGuid As String
		Public Property EmployeeEMail As String

		Public Property createdon As Date?
		Public Property createdfrom As String

		Public Property Gender As String
		Public Property employeefirstname As String
		Public Property employeelastname As String
		Public Property SelectedRec As Boolean?

		Public ReadOnly Property employeeFullname() As String
			Get
				Return String.Format("{0}, {1}", employeelastname, employeefirstname)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullnameWithoutComma() As String
			Get
				Return String.Format("{1} {0}", employeelastname, employeefirstname)
			End Get
		End Property

		Public ReadOnly Property zeitraum() As String
			Get
				Return String.Format("{0} / {1}", monat, jahr)
			End Get
		End Property

	End Class


	Public Class PayrollNLAData
		Public Property MANr As Integer?

		Public Property employeelastname As String
		Public Property employeefirstname As String

		Public Property employeemastrasse As String
		Public Property employeemaplz As String
		Public Property employeemaort As String
		Public Property employeemaland As String
		Public Property employeemaco As String
		Public Property employeeahv_nr As String
		Public Property employeeahv_nr_new As String
		Public Property employeesend2wos As Boolean?
		Public Property employeegeschlecht As String
		Public Property employeemapostfach As String
		Public Property employeelajahr As Integer?

		Public Property Z_1_0 As Decimal?
		Public Property Z_2_1 As Decimal?
		Public Property Z_2_2 As Decimal?
		Public Property Z_2_3 As Decimal?
		Public Property Z_3_0 As Decimal?

		Public Property Z_4_0 As Decimal?
		Public Property Z_5_0 As Decimal?

		Public Property Z_6_0 As Decimal?
		Public Property Z_7_0 As Decimal?
		Public Property Z_8_0 As Decimal?
		Public Property Z_9_0 As Decimal?

		Public Property Z_10_1 As Decimal?
		Public Property Z_10_2 As Decimal?
		Public Property Z_11_0 As Decimal?
		Public Property Z_12_0 As Decimal?

		Public Property Z_13_1_1 As Decimal?
		Public Property Z_13_1_2 As Decimal?
		Public Property Z_13_2_1 As Decimal?
		Public Property Z_13_2_2 As Decimal?
		Public Property Z_13_2_3 As Decimal?
		Public Property Z_13_3_0 As Decimal?

		Public Property NLA_LoAusweis As Boolean?
		Public Property NLA_Befoerderung As Boolean?
		Public Property NLA_Kantine As Boolean?

		Public Property NLA_2_3 As String
		Public Property NLA_3_0 As String
		Public Property NLA_4_0 As String
		Public Property NLA_7_0 As String

		Public Property NLA_Spesen_NotShow As Boolean?
		Public Property NLA_13_1_2 As String
		Public Property NLA_13_2_3 As String

		Public Property NLA_Nebenleistung_1 As String
		Public Property NLA_Nebenleistung_2 As String
		Public Property NLA_Bemerkung_1 As String
		Public Property NLA_Bemerkung_2 As String
		Public Property Grund As String

		Public Property ES_Ab1 As Date?
		Public Property ES_Bis1 As Date?

		Public ReadOnly Property EmployeeFullnameWithoutComma As String
			Get
				Return String.Format("{0} {1}", employeefirstname, employeelastname)
			End Get
		End Property

		Public ReadOnly Property LastnameFirstname As String
			Get
				Return String.Format("{1}, {0}", employeefirstname, employeelastname)
			End Get
		End Property

		Public ReadOnly Property PostcodeCity As String
			Get
				Return String.Format("{0} {1}", employeemaplz, employeemaort)
			End Get
		End Property


	End Class


	Public Class DunningPrintData

		Public Property MDNr As Integer
		Public Property KDNr As Integer

		Public Property SPNr As Integer
		Public Property VerNr As Integer
		Public Property CustomerLanguage As String
		Public Property RName1 As String
		Public Property RStrasse As String
		Public Property RPLZ As String
		Public Property ROrt As String
		Public Property BetragTotal As Decimal?
		Public Property IsSelected As Boolean?

		Public ReadOnly Property RPLZOrt As String
			Get
				Return String.Format("{0} {1}", RPLZ, ROrt)
			End Get
		End Property

		Public ReadOnly Property RAddress As String
			Get
				Return String.Format("{0}, {1} {2}", RStrasse, RPLZ, ROrt)
			End Get
		End Property

	End Class

	Public Class PayrollDetailData

		Public Property MDNr As Integer
		Public Property recID As Integer?
		Public Property LONr As Integer?
		Public Property MANr As Integer?

		Public Property monat As Integer?
		Public Property jahr As Integer?
		Public Property Send2WOS As Boolean?
		Public Property employeeLanguage As String

		Public Property LANr As Decimal?
		Public Property m_Anz As Decimal?
		Public Property m_Bas As Decimal?
		Public Property m_Ans As Decimal?
		Public Property m_btr As Decimal?
		Public Property LALoText As String

		Public Property employeefirstname As String
		Public Property employeelastname As String


		Public ReadOnly Property employeeFullname() As String
			Get
				Return String.Format("{0}, {1}", employeelastname, employeefirstname)
			End Get
		End Property

		Public ReadOnly Property zeitraum() As String
			Get
				Return String.Format("{0} / {1}", monat, jahr)
			End Get
		End Property


	End Class


	Public Class QSTCantonMasterData

		Public Property ID As Integer?
		Public Property RecNr As Integer?
		Public Property MDNr As Integer

		Public Property Address1 As String
		Public Property Address2 As String
		Public Property ZHD As String
		Public Property Canton As String
		Public Property PostOfficeBox As String
		Public Property Street As String
		Public Property Country As String
		Public Property Postcode As String
		Public Property City As String
		Public Property AccountNumber As String
		Public Property Provision As Decimal?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property Communikty As String
		Public Property Comments As String

	End Class


	Public Class QSTLAData

		Public Property MDNr As Integer
		Public Property LANr As Decimal?
		Public Property LALoText As String
		Public Property QSteuer As Boolean?

		Public ReadOnly Property DisplayText As String
			Get
				Return String.Format("{0:0.###} - {1}", LANr, LALoText)
			End Get
		End Property

	End Class

	Public Class CantonData
		Public Property Canton As String
	End Class

	Public Class QSTCommunityData
		Public Property CommunityCode As String
		Public Property CommunityName As String

		Public ReadOnly Property CommunityViewData As String
			Get
				Return String.Format("{0} - {1}", CommunityCode, CommunityName)
			End Get
		End Property

	End Class

	Public Class QSTCodeData
		Public Property QSTCode As String
		Public Property QSTCodeLabel As String

		Public ReadOnly Property QSTCodeViewData As String
			Get
				Return String.Format("{0} - {1}", QSTCode, QSTCodeLabel)
			End Get
		End Property

	End Class

	Public Class QSTPermissionData
		'Public Property PermissioinCode As String
		Public Property PermissionCode As String
		Public Property PermissionCodeLabel As String

		Public ReadOnly Property PermissionCodeViewData As String
			Get
				Return String.Format("{0} - {1}", PermissionCode, PermissionCodeLabel)
			End Get
		End Property

	End Class

	Public Class QSTListingSearchData

		Public Property MDNr As Integer
		Public Property EmployeeNumbers As List(Of Integer?)
		Public Property Year As Integer?
		Public Property MonthFrom As Integer?
		Public Property MonthTo As Integer?
		Public Property LANr As List(Of Decimal?)
		Public Property Canton As String
		Public Property Community As String
		Public Property CountryList As List(Of String)
		Public Property NationaliyList As List(Of String)
		Public Property QSTCode As List(Of String)
		Public Property Permission As List(Of String)

		Public Property XMLListingArt As String
		Public Property CityAsCommunityIFEmpty As Boolean?
		Public Property OupputAsEmptyDeclaretion As Boolean?
		Public Property FirstEmployment As Boolean?
		Public Property HideZeroAmount As Boolean?
		Public Property HideFranz As Boolean?
		Public Property ShowFranz As Boolean?

	End Class

	Public Class SearchRestulOfTaxData

		Public Property MANr As Integer?

		Public Property vonmonat As Integer?
		Public Property bismonat As Integer?
		Public Property jahr As Integer?
		Public Property gebdat As DateTime?

		Public Property ahv_nr As String
		Public Property PermissionCode As String
		Public Property CivilStatus As String
		Public Property CivilState As String
		Public Property ahv_nr_new As String
		Public Property s_kanton As String
		Public Property qstgemeinde As String
		Public Property TaxCommunityLabel As String
		Public Property TaxCommunityCode As Integer?

		Public Property EmploymentType As String
		Public Property OtherEmploymentType As String
		Public Property TypeofStay As String
		Public Property ForeignCategory As String
		Public Property SocialInsuranceNumber As String
		Public Property NumberOfChildren As Integer

		Public Property TaxChurchCode As String
		Public Property PartnerLastName As String
		Public Property PartnerFirstname As String
		Public Property PartnerStreet As String
		Public Property PartnerPostcode As String
		Public Property PartnerCity As String
		Public Property PartnerCountry As String
		Public Property InEmployment As Boolean
		Public Property EmploymentLocation As String
		Public Property EmploymentPostcode As String
		Public Property EmploymentStreet As String
		Public Property EmploymentCommunityCode As Integer

		Public Property geschlecht As String

		Public Property EmployeeFirstname As String
		Public Property EmployeeLastname As String

		Public Property employeename As String
		Public Property employeestreet As String
		Public Property employeepostcode As String
		Public Property employeecity As String
		Public Property employeecountry As String

		Public Property monat As Integer?
		Public Property kinder As Integer?
		Public Property employeelanguage As String
		Public Property m_anz As Decimal?
		Public Property m_bas As Decimal?
		Public Property m_ans As Decimal?
		Public Property m_btr As Decimal?
		Public Property Bruttolohn As Decimal?
		Public Property qstbasis As Decimal?

		Public Property Exception_KTGBetrag_Amount As Decimal?
		Public Property Exception_SuvaBetrag_Amount As Decimal?
		Public Property Exception_KiAuBetrag_Amount As Decimal?
		Public Property Exception_OtherServices_Amount As Decimal?
		Public Property Exception_OtherNotDefined_Amount As Decimal?
		Public Property Exception_OtherServicesLALabel As String
		Public Property Exception_OtherNotDefinedLALabel As String
		Public Property Exception_SPayed_Amount As Decimal?
		Public Property Exception_SBacked_Amount As Decimal?

		Public Property stdanz As Decimal?

		Public Property WorkingHoursWeek As Decimal?
		Public Property WorkingHoursMonth As Decimal?
		Public Property ESLohnNumber As Integer?
		Public Property EmploymentNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property WorkingPensum As String
		Public Property GAVStringInfo As String
		Public Property Dismissalreason As String

		Public Property tarifcode As String
		Public Property workeddays As Integer?

		Public Property esab As DateTime?
		Public Property esende As DateTime?

		Public Property EmployeePartnerRecID As Integer?
		Public Property EmployeeLOHistoryID As Integer?

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return (String.Format("{1}, {0}", EmployeeFirstname, EmployeeLastname))
			End Get
		End Property

		Public ReadOnly Property EmployeeAddress As String
			Get
				Return (String.Format("{0}, {1}-{2} {3}", employeestreet, employeecountry, employeepostcode, employeecity))
			End Get
		End Property

		Public ReadOnly Property VacationDaysCount As Integer
			Get
				Dim result As Integer = 0
				Dim value As List(Of String) = GAVStringInfo.Split("¦").ToList()
				Dim vacationString = value.Where(Function(x As String) x.Contains("FerienJahr:")).FirstOrDefault
				If vacationString Is Nothing Then Return result

				result = Convert.ToInt32(vacationString.Split(":")(1))

				Return result
			End Get
		End Property

		Public ReadOnly Property TypeofStayViewData As String
			Get
				Select Case EmploymentType
					Case "H1"
						Return "DAILY"
					Case "H2"
						Return "WEEKLY"

					Case Else
						Return "CH"
				End Select

			End Get
		End Property

		Public ReadOnly Property EmploymentTypeViewData As String
			Get
				Select Case OtherEmploymentType
					Case "01"
						Return "MAIN_JOB"
					Case "02"
						Return "SIDE_JOB"

					Case Else
						Return String.Empty
				End Select

			End Get
		End Property

		Public ReadOnly Property TaxCivilStatusCode As String
			Get

				Select Case CivilState
					Case "L"
						Return "SINGLE"
					Case "V"
						Return "MARRIED"
					Case "W"
						Return "WIDOWED"
					Case "G"
						Return "DIVORCED"
					Case "T"
						Return "SEPARATED"
					Case "P"
						Return "REGISTERED_PARTNERSHIP"

					Case "D"
						Return "PARTNERSHIP_DISSOLVED_BY_LAW"
					Case "N", "S"
						Return "PARTNERSHIP_DISSOLVED_BY_DEATH"

					Case Else
						Return String.Empty
				End Select

			End Get
		End Property

		Public ReadOnly Property TaxChurchCodeViewData As String
			Get
				If Not String.IsNullOrWhiteSpace(TaxChurchCode) Then
					If TaxChurchCode.ToUpper.EndsWith("Y") Then
						Return "yes"
					Else
						Return "no"
					End If

				Else
					Return String.Empty
				End If

			End Get
		End Property

	End Class



	Public Class ListingESListData

		Public Property ESNr As Integer?
		Public Property KDNr As Integer?
		Public Property MANr As Integer?

		Public Property ahv_nr As String
		Public Property employeefirstname As String
		Public Property employeelastname As String
		Public Property customername As String

		Public Property GebDat As Date?
		Public Property ES_Ab As Date?
		Public Property ES_Ende As Date?
		Public Property MABeruf As String

		Public Property Filiale As String
		Public Property UID As String
		Public Property NogaCode As String

		Public Property KDOrt As String
		Public Property ESOrt As String
		Public Property GAVNumber As Integer?
		Public Property GAVGruppe0 As String
		Public Property ES_Als As String
		Public Property Einstufung As String
		Public Property GAVBezeichnung As String
		Public Property GAVInfo_String As String


		Public Property Grundlohn As Decimal?
		Public Property Ferien As Decimal?
		Public Property FerienProz As Decimal?
		Public Property Feier As Decimal?
		Public Property FeierProz As Decimal?
		Public Property Lohn13 As Decimal?
		Public Property Lohn13Proz As Decimal?
		Public Property Bruttolohn As Decimal?
		Public Property ESSpesen As Decimal?
		Public Property ESStunden As Decimal?


		Public ReadOnly Property employeeFullname() As String
			Get
				Return String.Format("{0}, {1}", employeelastname, employeefirstname)
			End Get
		End Property


	End Class

	Public Class PayrollEvaluationListData

		Public Property LONr As Integer?
		Public Property MANr As Integer?
		Public Property Monat As Integer?
		Public Property Jahr As Integer?

		Public Property WohnOrt As String
		Public Property Employeename As String
		Public Property Country As String
		Public Property S_Kanton As String
		Public Property Zivilstand As String
		Public Property Kirchensteuer As String
		Public Property Q_Steuer As String
		Public Property AnzahlKinder As Integer?
		Public Property WorkedDay As Integer?

		Public Property Bruttolohn As Decimal?
		Public Property AHVBasis As Decimal?
		Public Property AHVLohn As Decimal?
		Public Property AHVFreibetrag As Decimal?
		Public Property NichtAHV As Decimal?
		Public Property ALV1Lohn As Decimal?
		Public Property ALV2Lohn As Decimal?
		Public Property SUVABasis As Decimal?
		Public Property QSTBasis As Decimal?

		Public Property QSTTarif As String
		Public Property ESData As String
		Public Property BVGBegin As Date?
		Public Property BVGEnd As Date?
		Public Property Ansaessigkeit As Boolean?
		Public Property BVGBeginEnd As String
		Public Property CHPartner As Boolean?
		Public Property NoSpecialTax As Boolean?
		Public Property Permission As String

		Public Property PermissionToDate As Date?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

		Public Property WorkedHour As Decimal?
		Public Property AHVBeitrag As Decimal?
		Public Property ALVBeitrag As Decimal?
		Public Property ALV2Beitrag As Decimal?
		Public Property FARTotal As Decimal?
		Public Property ParifondTotal As Decimal?
		Public Property NBUVBeitrag As Decimal?

		Public Property KTGBeitrag As Decimal?
		Public Property BVGTotal As Decimal?
		Public Property QSTTotal As Decimal?
		Public Property MinusLohn As Decimal?
		Public Property _8700 As Decimal?
		Public Property _8720 As Decimal?
		Public Property _8730 As Decimal?
		Public Property ZGTotal As Decimal?
		Public Property ZGTotalFees As Decimal?
		Public Property AuszahlungTotal As Decimal?
		Public Property NegativLohn As Decimal?

	End Class


	Public Class AdvancedpaymentEvaluationListData

		Public Property MDNr As Integer
		Public Property ZGNr As Integer
		Public Property MANr As Integer
		Public Property RPNr As Integer
		Public Property LANr As Integer
		Public Property LONr As Integer
		Public Property VGNr As Integer
		Public Property Grund As String
		Public Property Amount As Decimal?
		Public Property Monat As Integer?
		Public Property Jahr As Integer?
		Public Property Aus_Date As Date?
		Public Property Currency As String
		Public Property GebAbzug As Boolean?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property BankAU As Boolean?
		Public Property DTADate As Date?
		Public Property ClearingNr As Integer?
		Public Property Bankname As String
		Public Property KontoNr As String
		Public Property BankOrt As String
		Public Property Addressline_1 As String
		Public Property Addressline_2 As String
		Public Property Addressline_3 As String
		Public Property Addressline_4 As String
		Public Property IBANNr As String
		Public Property Swift As String
		Public Property BLZNr As String
		Public Property PrintedOn As DateTime?
		Public Property IsCreatedWithLO As Boolean?


		Public Property Firstname As String
		Public Property Lastname As String

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return (String.Format("{0} {1}", Firstname, Lastname))
			End Get
		End Property


	End Class

	Public Class ListingPayrollLohnkontiData

		Public Property MANr As Integer?

		Public Property AHVNumber As String
		Public Property EmployeeFirstname As String
		Public Property Gender As String
		Public Property Nationality As String
		Public Property EmployeeLastname As String
		Public Property GebDat As Date?
		Public Property SatrtOfEmployment As Date?
		Public Property EndOfEmployment As Date?

		Public Property LANr As Decimal?
		Public Property Januar As Decimal?
		Public Property Februar As Decimal?
		Public Property Maerz As Decimal?
		Public Property April As Decimal?
		Public Property Mai As Decimal?
		Public Property Juni As Decimal?
		Public Property Juli As Decimal?
		Public Property August As Decimal?
		Public Property September As Decimal?
		Public Property Oktober As Decimal?
		Public Property November As Decimal?
		Public Property Dezember As Decimal?
		Public Property Kumulativ As Decimal?
		Public Property LALabel As String


		Public ReadOnly Property EmployeeFullname() As String
			Get
				Return String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname)
			End Get
		End Property


	End Class


	Public Class ListingPayrollNLAData

		Public Property IsSelected As Boolean

		Public Property MANr As Integer?

		Public Property employeelastname As String
		Public Property employeefirstname As String

		Public Property employeemastrasse As String
		Public Property employeemaplz As String
		Public Property employeemaort As String
		Public Property employeemaland As String
		Public Property employeemaco As String
		Public Property employeeahv_nr As String
		Public Property employeeahv_nr_new As String
		Public Property employeebirthdate As Date?
		Public Property employeesend2wos As Boolean?
		Public Property employeegeschlecht As String
		Public Property employeemapostfach As String
		Public Property employeelajahr As Integer?


		Public Property Z_1_0 As Decimal?
		Public Property Z_2_1 As Decimal?
		Public Property Z_2_2 As Decimal?
		Public Property Z_2_3 As Decimal?
		Public Property Z_3_0 As Decimal?

		Public Property Z_4_0 As Decimal?
		Public Property Z_5_0 As Decimal?

		Public Property Z_6_0 As Decimal?
		Public Property Z_7_0 As Decimal?
		Public Property Z_8_0 As Decimal?
		Public Property Z_9_0 As Decimal?

		Public Property Z_10_1 As Decimal?
		Public Property Z_10_2 As Decimal?
		Public Property Z_11_0 As Decimal?
		Public Property Z_12_0 As Decimal?


		Public Property Z_13_1_1 As Decimal?
		Public Property Z_13_1_2 As Decimal?
		Public Property Z_13_2_1 As Decimal?
		Public Property Z_13_2_2 As Decimal?
		Public Property Z_13_2_3 As Decimal?
		Public Property Z_13_3_0 As Decimal?


		Public Property NLA_LoAusweis As Boolean?
		Public Property NLA_Befoerderung As Boolean?
		Public Property NLA_Kantine As Boolean?


		Public Property NLA_2_3 As String
		Public Property NLA_3_0 As String
		Public Property NLA_4_0 As String
		Public Property NLA_7_0 As String


		Public Property NLA_Spesen_NotShow As Boolean?
		Public Property NLA_13_1_2 As String
		Public Property NLA_13_2_3 As String


		Public Property NLA_Nebenleistung_1 As String
		Public Property NLA_Nebenleistung_2 As String
		Public Property NLA_Bemerkung_1 As String
		Public Property NLA_Bemerkung_2 As String
		Public Property Grund As String


		Public Property ES_Ab1 As String
		Public Property ES_Bis1 As String


		Public ReadOnly Property LastnameFirstname As String
			Get
				Return String.Format("{0}, {1}", employeelastname, employeefirstname)
			End Get
		End Property

		Public ReadOnly Property PostcodeCity As String
			Get
				Return String.Format("{0} {1}", employeemaplz, employeemaort)
			End Get
		End Property

	End Class


	Public Class HourSearchData

		Public Property MDNr As Integer
		Public Property Numbers As List(Of Integer)
		Public Property Monat As Integer?
		Public Property Jahr As Integer?
		Public Property CustomerFProperty As Decimal?
		Public Property CustomerContact As String
		Public Property CustomerState1 As String
		Public Property CustomerState2 As String

		Public Property EmployeeContact As String
		Public Property EmployeeState1 As String
		Public Property EmployeeState2 As String
		Public Property EmployeeReserve1 As String
		Public Property EmployeeReserve2 As String

		Public Property EmploymentKst1 As String
		Public Property EmploymentKst2 As String
		Public Property EmploymentAdvisor As String
		Public Property EmploymentESCategorize As String
		Public Property EmploymentBranch As String
		Public Property EmploymentPVL As Integer?

		Public Property CalculationType As CalculationTypeEnum

	End Class

	Public Enum HourSearchTypeEnum
		Employee
		Customer
	End Enum

	Public Enum CalculationTypeEnum
		HourCalculation
		AllCalculation
	End Enum

	Public Class CustomertReportHoursData

		Public Property MDNr As Integer
		Public Property CustomerCostcenter As Integer?
		Public Property CustomerNumber As Integer
		Public Property RPKST As String
		Public Property Company1 As String
		Public Property Company2 As String
		Public Property Company3 As String
		Public Property CostcenterName As String
		Public Property PostOfficeBox As String
		Public Property Street As String
		Public Property CountryCode As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property Telephone As String
		Public Property Telefax As String
		Public Property EMail As String
		Public Property FirstProperty As Decimal?
		Public Property TotalHours As Decimal?
		Public Property LANr As Decimal?
		Public Property Amount As Decimal?

		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", Postcode, Location)
			End Get
		End Property

	End Class


	Public Class ReportlineHoursData

		Public Property MDNr As Integer
		Public Property CustomerCostcenter As Integer?
		Public Property CostcenterName As String
		Public Property ReportNumber As Integer
		Public Property Monat As Integer
		Public Property Jahr As Integer
		Public Property LANr As Decimal?
		Public Property LALOText As String
		Public Property VonDate As Date?
		Public Property BisDate As Date?
		Public Property CountHour As Decimal?
		Public Property RPKst1 As String
		Public Property RPKst2 As String
		Public Property RPKst As String
		Public Property ES_Als As String
		Public Property Einstufung As String
		Public Property ESBranche As String
		Public Property Amount As Decimal?

		Public ReadOnly Property ReportMonthYear As String
			Get
				Return String.Format("{0:f0} - {1:f0}", Monat, Jahr)
			End Get
		End Property

		Public ReadOnly Property ReportLineFromTo As String
			Get
				Return String.Format("{0:d} - {1:d}", VonDate, BisDate)
			End Get
		End Property

	End Class

	Public Class CustomerReportLinesPrintData
		Inherits CustomertReportHoursData

		Public Property ReportLineData As List(Of ReportlineHoursData)

	End Class

	Public Class EmployeeReportHoursData

		Public Property MDNr As Integer
		Public Property Employeenumber As Integer
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property PostOfficeBox As String
		Public Property StayAs As String
		Public Property Street As String
		Public Property CountryCode As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property Telephone_P As String
		Public Property Telephone_2 As String
		Public Property Telephone_3 As String
		Public Property Telephone_G As String
		Public Property Mobile As String
		Public Property EMail As String
		Public Property TotalHours As Decimal?

		Public ReadOnly Property EmployeeLastnameFirstname As String
			Get
				Return (String.Format("{0}, {1}", Lastname, Firstname))
			End Get
		End Property

		Public ReadOnly Property EmployeePostcodeLocation As String
			Get
				Return (String.Format("{0}-{1} {2}", CountryCode, Postcode, Location))
			End Get
		End Property

	End Class


	Public Class EmployeeSuvaSearchData

		Public Property MDNr As Integer
		Public Property EmployeeNumbers As List(Of Integer)
		Public Property Monat As Integer?
		Public Property Jahr As Integer?
		Public Property EmploymentPVL As Integer?

	End Class

	Public Class EmployeeARGBSearchData

		Public Property MDNr As Integer
		Public Property EmployeeNumbers As List(Of Integer)
		Public Property DateFrom As Date?
		Public Property DateTo As Date?

	End Class

	Public Class SuvaWeekData

		Public Property CalendarYear As Integer
		Public Property CalendarMonth As Integer
		Public Property CalendarWeek As Integer
		Public Property ReportNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property EmploymentNumber As Integer?

		Public Property MondayDate As DateTime?
		Public Property TuesdayDate As DateTime?
		Public Property WednesdayDate As DateTime?
		Public Property ThursdayDate As DateTime?
		Public Property FridayDate As DateTime?
		Public Property SaturdayDate As DateTime?
		Public Property SundayDate As DateTime?

		Public Property WorkedDaysInWeek As Integer?

		Public Property MondayStd As Decimal?
		Public Property TuesdayStd As Decimal?
		Public Property WednesdayStd As Decimal?
		Public Property ThursdayStd As Decimal?
		Public Property FridayStd As Decimal?
		Public Property SaturdayStd As Decimal?
		Public Property SundayStd As Decimal?

		Public Property MondayAbsence As String
		Public Property TuesdayAbsence As String
		Public Property WednesdayAbsence As String
		Public Property ThursdayAbsence As String
		Public Property FridayAbsence As String
		Public Property SaturdayAbsence As String
		Public Property SundayAbsence As String


		''' <summary>
		''' Gets dayvalue code of a day.
		''' </summary>
		Public Function GetDayValueOfDay(ByVal dayName As DayOfWeek) As String

			Select Case dayName
				Case DayOfWeek.Monday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", MondayStd, MondayAbsence))
					Return String.Format("{0:F2} {1}", MondayStd, MondayAbsence)
				Case DayOfWeek.Tuesday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", TuesdayStd, TuesdayAbsence))
					Return String.Format("{0:F2} {1}", TuesdayStd, TuesdayAbsence)
				Case DayOfWeek.Wednesday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", WednesdayStd, WednesdayAbsence))
					Return String.Format("{0:F2} {1}", WednesdayStd, WednesdayAbsence)
				Case DayOfWeek.Thursday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", ThursdayStd, ThursdayAbsence))
					Return String.Format("{0:F2} {1}", ThursdayStd, ThursdayAbsence)
				Case DayOfWeek.Friday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", FridayStd, FridayAbsence))
					Return String.Format("{0:F2} {1}", FridayStd, FridayAbsence)
				Case DayOfWeek.Saturday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", SaturdayStd, SaturdayAbsence))
					Return String.Format("{0:F2} {1}", SaturdayStd, SaturdayAbsence)
				Case DayOfWeek.Sunday
					Trace.WriteLine(String.Format("GetDayValueOfDay {0:F2} {1}", SundayStd, SundayAbsence))
					Return String.Format("{0:F2} {1}", SundayStd, SundayAbsence)

				Case Else
					Return Nothing

			End Select


		End Function

		''' <summary>
		''' Gets std count of a week.
		''' </summary>
		Public Function GetHourValueOfWeek() As Decimal?
			Dim stdCount As Decimal?

			If MondayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + MondayStd
			If TuesdayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + TuesdayStd
			If WednesdayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + WednesdayStd
			If ThursdayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + ThursdayStd
			If FridayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + FridayStd
			If SaturdayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + SaturdayStd
			If SundayStd.HasValue Then stdCount = stdCount.GetValueOrDefault(0) + SundayStd


			Return stdCount

		End Function

		''' <summary>
		''' Gets day count of a week (without Null days).
		''' </summary>
		Public Function GetDayCountValueOfWeek() As Integer?
			Dim dayCount As Integer

			If (MondayDate.HasValue AndAlso Not String.IsNullOrWhiteSpace(MondayAbsence)) OrElse MondayStd.GetValueOrDefault(0) > 0 Then dayCount += 1

			If (TuesdayDate.HasValue AndAlso Not String.IsNullOrWhiteSpace(TuesdayAbsence)) OrElse TuesdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			If (WednesdayDate.HasValue AndAlso Not String.IsNullOrWhiteSpace(WednesdayAbsence)) OrElse WednesdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			If (ThursdayDate.HasValue AndAlso Not String.IsNullOrWhiteSpace(ThursdayAbsence)) OrElse ThursdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			If (FridayDate.HasValue AndAlso Not String.IsNullOrWhiteSpace(FridayAbsence)) OrElse FridayStd.GetValueOrDefault(0) > 0 Then dayCount += 1

			'If (TuesdayDate.HasValue AndAlso Not TuesdayStd.GetValueOrDefault(0) > 0) OrElse String.IsNullOrWhiteSpace(TuesdayAbsence) Then dayCount += 1
			'If (WednesdayDate.HasValue AndAlso Not WednesdayStd.GetValueOrDefault(0)) > 0 OrElse String.IsNullOrWhiteSpace(WednesdayAbsence) Then dayCount += 1
			'If (ThursdayDate.HasValue AndAlso Not ThursdayStd.GetValueOrDefault(0)) > 0 OrElse String.IsNullOrWhiteSpace(ThursdayAbsence) Then dayCount += 1
			'If (FridayDate.HasValue AndAlso Not FridayStd.GetValueOrDefault(0) > 0) OrElse String.IsNullOrWhiteSpace(FridayAbsence) Then dayCount += 1

			'If MondayDate.HasValue AndAlso MondayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			'If TuesdayDate.HasValue AndAlso TuesdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			'If WednesdayDate.HasValue AndAlso WednesdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			'If ThursdayDate.HasValue AndAlso ThursdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			'If FridayDate.HasValue AndAlso FridayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			If SaturdayDate.HasValue AndAlso SaturdayStd.GetValueOrDefault(0) > 0 Then dayCount += 1
			If SundayDate.HasValue AndAlso SundayStd.GetValueOrDefault(0) > 0 Then dayCount += 1


			Return dayCount

		End Function

		Public Function GetFirstDayOfWeek() As DateTime
			Dim januar4 As New DateTime(CalendarYear, 1, 4)
			Dim weekdayjah4 As Integer = GetDayOfWeek(januar4)
			Dim dateoffirstWeek As DateTime = januar4.AddDays(1 - weekdayjah4)

			Return dateoffirstWeek.AddDays((CalendarWeek - 1) * 7)
		End Function

		Private Function GetDayOfWeek(ByVal myDate As DateTime) As Integer
			Return (myDate.DayOfWeek + 6) Mod 7 + 1
		End Function

	End Class


  Public Class SuvaTableListData

    Public Property MDNr As Integer
    Public Property CalendarYear As Integer
    Public Property CalendarMonth As Integer
    Public Property CalendarWeek As Integer
    Public Property ReportNumber As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property EmploymentNumber As Integer?

    Public Property MondayDate As DateTime?
    Public Property TuesdayDate As DateTime?
    Public Property WednesdayDate As DateTime?
    Public Property ThursdayDate As DateTime?
    Public Property FridayDate As DateTime?
    Public Property SaturdayDate As DateTime?
    Public Property SundayDate As DateTime?

    Public Property Tag1 As String
    Public Property Tag2 As String
    Public Property Tag3 As String
    Public Property Tag4 As String
    Public Property Tag5 As String
    Public Property Tag6 As String
    Public Property Tag7 As String

    Public Property MondayOfWeek As DateTime?
    Public Property SundayOfWeek As DateTime?

    Public Property WorkedDayCount As Integer?
    Public Property WorkedHourCount As Decimal?

  End Class

	Public Class ReportWeeklyPrintData
		Public Property ID As Integer?
		Public Property MDNr As Integer?
		Public Property UserNr As Integer?
		Public Property RPNr As Integer?
		Public Property MANr As Integer?
		Public Property KDNr As Integer?
		Public Property ESNr As Integer?
		Public Property MondayDate As DateTime?
		Public Property TuesdayDate As DateTime?
		Public Property WednesdayDate As DateTime?
		Public Property ThursdayDate As DateTime?
		Public Property FridayDate As DateTime?
		Public Property SaturdayDate As DateTime?
		Public Property SundayDate As DateTime?
		Public Property Month As Integer
		Public Property Week As Integer
		Public Property Year As Integer
		Public Property PrintedWeeks As String
		Public Property PrintedDates As String

	End Class

	Public Class RPPrintWeeklyData
    ' RPPrint Daten
    Public Property ID As Integer?
    Public Property RPNr As Integer?
    Public Property MANr As Integer?
    Public Property KDNr As Integer?
    Public Property ESNr As Integer?
    Public Property Montag As DateTime?
    Public Property Dienstag As DateTime?
    Public Property Mittwoch As DateTime?
    Public Property Donnerstag As DateTime?
    Public Property Freitag As DateTime?
    Public Property Samstag As DateTime?
    Public Property Sonntag As DateTime?
		Public Property Monat As Integer?
		Public Property Woche As Integer?
		Public Property Jahr As Integer?
		Public Property PrintedWeeks As String
    Public Property PrintedDate As String
    Public Property USNr As Integer?

		Public Property CustomerCompany As String
		Public Property KDFirma2 As String
		Public Property KDFirma3 As String
		Public Property CustomerPostoffice As String
		Public Property CustomerStreet As String
		Public Property CustomerPostcode As String
		Public Property CustomerLocation As String
		Public Property CustomerCountry As String
		Public Property CustomerTelephone As String
		Public Property CustomerTelefax As String

		Public Property ESArbOrt As String
		Public Property ESArbZeit As String
		Public Property ESMelden As String
		Public Property ESAls As String
		Public Property ESBemerk_MA As String
		Public Property ESBemerk_1 As String
		Public Property ESBemerk_2 As String
		Public Property ESBemerk_3 As String
		Public Property ESBemerk_LO As String
		Public Property ESBemerk_RE As String
		Public Property ESBemerk_P As String
		Public Property ESSUVA As String
		Public Property ESUhr As String
		Public Property ESEnde As String
		Public Property ES_Ab As Date?
		Public Property ES_Ende As Date?
		Public Property ESLStdLohn As Decimal?
		Public Property ESLTarif As Decimal?
		Public Property ESLMAStdSpesen As Decimal?
		Public Property ESLMATSpesen As Decimal?
		Public Property ESLKDTSpesen As Decimal?
		Public Property ESLGAVGruppe0 As String

		Public Property EmployeeGender As String
		Public Property EmployeeLastname As String
		Public Property EmployeeFirstname As String
		Public Property MACo As String
		Public Property EmployeePostoffice As String
		Public Property EmployeeStreet As String
		Public Property EmployeePostcode As String
		Public Property EmployeeLocation As String
		Public Property EmployeeCountry As String
		Public Property EmployeeTelefon_G As String
		Public Property EmployeeTelefon_P As String
		Public Property EmployeeTelefon_2 As String
		Public Property EmployeeTelefon_3 As String
		Public Property EmployeeMobile As String
		Public Property EmployeeMobile_2 As String


		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", CustomerPostcode, CustomerLocation)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullName As String
			Get
				Return String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname)
			End Get
		End Property

		Public ReadOnly Property EmployeePostcodeLocation As String
			Get
				Return String.Format("{0} {1}", EmployeePostcode, EmployeeLocation)
			End Get
		End Property

	End Class

	Public Class TaxListTableData
		Public Property UserTableName As String
		Public Property MDNr As Integer?
		Public Property USNr As Integer?
		Public Property MANR As Integer?
		Public Property LANR As Decimal?
		Public Property Monat As Integer?
		Public Property LONR As Integer?
		Public Property LALOText As String
		Public Property S_Kanton As String
		Public Property S_Gemeinde As String
		Public Property ESAb As DateTime?
		Public Property ESEnde As DateTime?
		Public Property Nachname As String
		Public Property Vorname As String
		Public Property VonMonat As Integer?
		Public Property BisMonat As Integer?
		Public Property Jahr As Integer?
		Public Property GebDat As DateTime?
		Public Property AHV_Nr As String
		Public Property AHV_Nr_New As String
		Public Property Geschlecht As String
		Public Property MAStrasse As String
		Public Property MAPLZ As String
		Public Property MAOrt As String
		Public Property MAPLZOrt As String
		Public Property MALand As String
		Public Property Zivilstand As String
		Public Property Sprache As String
		Public Property Kinder As Integer?
		Public Property Bewillig As String
		Public Property SelectedKanton As String
		Public Property SelectedGemeinde As String
		Public Property M_Anz As Decimal?
		Public Property M_Bas As Decimal?
		Public Property M_Ans As Decimal?
		Public Property M_Btr As Decimal?
		Public Property Bruttolohn As Decimal?
		Public Property QSTBasis As Decimal?
		Public Property StdAnz As Decimal?
		Public Property TarifCode As String
		Public Property WorkedDays As Integer?
		Public Property ShowStdAnz As Boolean?
		Public Property ESStrasse As String
		Public Property ESPLZ As String
		Public Property ESOrt As String
		Public Property ESKanton As String
		Public Property AssignedESNr As Integer?
		Public Property AssignedESLohnNr As Integer?
		Public Property AssignedRPNr As Integer?
		Public Property GAVInfo As String
		Public Property RPGAVStdWeek As Decimal?
		Public Property RPGAVStdMonth As Decimal?
		Public Property Dismissalreason As String
		Public Property EmployeePartnerRecID As Integer?
		Public Property EmployeeLOHistoryID As Integer?
		Public Property Arbeitspensum As String

	End Class


End Namespace


