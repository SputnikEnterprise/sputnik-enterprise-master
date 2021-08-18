
Namespace Customer.DataObjects

	Public Class NewCustomerInitData
		Public Property CustomerMandantNumber As Integer
		Public Property CustomerNumber As Integer
		Public Property CustomerNumberOffset As Integer
		Public Property Company1 As String
		Public Property Street As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property CountryCode As String

		Public Property KST As String
		Public Property KDBusinessBranch As String
		Public Property CreditLimit1 As Decimal?
		Public Property CreditLimit2 As Decimal?
		Public Property CreditWarning As Boolean?
		Public Property NoUse As Boolean
		Public Property CurrencyCode As String
		Public Property ReminderCode As String
		Public Property PaymentCondition As String
		Public Property InvoiceOption As String
		Public Property OneInvoicePerMail As Boolean?

		Public Property CreatedFrom As String
		Public Property CreatedUserNumber As Integer?

	End Class


	''' <summary>
	''' Customer master data (Kunde).
	''' </summary>
	Public Class CustomerMasterData
		Public Property CustomerMandantNumber As Integer
		Public Property CustomerNumber As Integer
		Public Property CustomerNumberOffset As Integer
		Public Property WOSGuid As String
		Public Property SolvencyDecisionID As Integer
		Public Property SolvencyInfo As String
		Public Property Company1 As String
		Public Property Company2 As String
		Public Property Company3 As String
		Public Property PostOfficeBox As String
		Public Property Street As String
		Public Property CountryCode As String
		Public Property Postcode As String
		Public Property Latitude As Double?
		Public Property Longitude As Double?
		Public Property Location As String
		Public Property Telephone As String
		Public Property Telefax As String
		Public Property Telefax_Mailing As Boolean
		Public Property EMail As String
		Public Property Email_Mailing As Boolean
		Public Property Hompage As String
		Public Property facebook As String
		Public Property xing As String

		Public Property KST As String
		Public Property KDBusinessBranch As String
		Public Property FirstProperty As Decimal?
		Public Property Language As String
		Public Property HowContact As String
		Public Property CustomerState1 As String
		Public Property CustomerState2 As String
		Public Property NoUse As Boolean
		Public Property NoUseComment As String
		Public Property Comment As String
		Public Property Notice_Employment As String
		Public Property Notice_Report As String
		Public Property Notice_Invoice As String
		Public Property Notice_Payment As String

		Public Property SalaryPerMonth As Decimal?
		Public Property SalaryPerHour As Decimal?
		Public Property Reserve1 As String
		Public Property Reserve2 As String
		Public Property Reserve3 As String
		Public Property Reserve4 As String
		Public Property CreditLimit1 As Decimal?
		Public Property CreditLimit2 As Decimal?
		Public Property CreditLimitsFromDate As DateTime?
		Public Property CreditLimitsToDate As DateTime?
		Public Property OpenInvoiceAmount As Decimal?
		Public Property ReferenceNumber As String
		Public Property KD_UmsMin As Decimal?
		Public Property mwstpflicht As Boolean?
		Public Property NumberOfCopies As Short?
		Public Property ValueAddedTaxNumber As String
		Public Property CreditWarning As Boolean?
		Public Property OPShipment As String
		Public Property NotPrintReports As Boolean?
		Public Property TermsAndConditions_WOS As String
		Public Property sendToWOS As Boolean?
		Public Property OneInvoicePerMail As Boolean?
		Public Property DoNotShowContractInWOS As Boolean?
		Public Property CurrencyCode As String
		Public Property BillTypeCode As String
		Public Property NumberOfEmployees As String
		Public Property CanteenAvailable As Boolean?
		Public Property TransportationOptions As Boolean?
		Public Property InvoiceOption As String
		Public Property ShowHoursInNormal As Boolean?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property Transfered_Guid As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?

		Public Function Clone() As CustomerMasterData
			Return DirectCast(Me.MemberwiseClone(), CustomerMasterData)
		End Function


		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property CustomerCompleteAddress As String
			Get
				Return String.Format("{0}, {1}-{2} {3}", Street, CountryCode, Postcode, Location)
			End Get
		End Property

	End Class

	Public Class CustomerNoticesData
		Public Property ID As Integer?
		Public Property CustomerNumber As Integer?
		Public Property Common As String
		Public Property Notice_Employment As String
		Public Property Notice_Report As String
		Public Property Notice_Invoice As String
		Public Property Notice_Payment As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedFrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?

	End Class


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


	''' <summary>
	''' Founded Propose data.
	''' </summary>
	Public Class CustomerProposeProperty

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


	''' <summary>
	''' Founded Offer data.
	''' </summary>
	Public Class CustomerOfferProperty

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


	''' <summary>
	''' Founded ES data.
	''' </summary>
	Public Class CustomerESProperty

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
		Public Property EmployeeStundenSpesen As Decimal?
		Public Property EmployeeTagesSpesen As Decimal?
		Public Property CustomerTagesSpesen As Decimal?
		Public Property margemitbvg As Decimal?
		Public Property margeohnebvg As Decimal?

		Public Property esals As String
		Public Property actives As Boolean?

		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property zfiliale As String

	End Class


	''' <summary>
	''' Founded Reports data.
	''' </summary>
	Public Class CustomerReportsProperty

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


	''' <summary>
	''' Founded Invoince data.
	''' </summary>
	Public Class CustomerInvoiceProperty

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


	''' <summary>
	''' Founded Payment data.
	''' </summary>
	Public Class CustomerPaymentProperty

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



End Namespace
