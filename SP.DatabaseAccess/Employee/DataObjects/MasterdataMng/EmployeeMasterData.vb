Namespace Employee.DataObjects.MasterdataMng

	''' <summary>
	''' Employee master data (Mitarbeiter)
	''' </summary>
	Public Class EmployeeMasterData
		Public Property ID As Integer
		Public Property EmployeeNumber As Integer?
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property PostOfficeBox As String
		Public Property Street As String
		Public Property Postcode As String
		Public Property Latitude As Double?
		Public Property Longitude As Double?
		Public Property Location As String
		Public Property Country As String
		Public Property Language As String
		Public Property Birthdate As DateTime?
		Public Property Gender As String
		Public Property AHV_Nr As String
		Public Property Nationality As String
		Public Property CivilStatus As String
		Public Property Telephone_P As String
		Public Property Telephone2 As String
		Public Property Telephone3 As String
		Public Property Telephone_G As String
		Public Property MobilePhone As String
		Public Property MobilePhone2 As String
		Public Property Homepage As String
		Public Property Email As String
		Public Property Facebook As String
		Public Property LinkedIn As String
		Public Property Xing As String
		Public Property Permission As String
		Public Property PermissionToDate As DateTime?
		Public Property BirthPlace As String
		Public Property CHPartner As Boolean
		Public Property NoSpecialTax As Boolean
		Public Property ValidatePermissionWithTax As Boolean?
		Public Property Q_Steuer As String
		Public Property S_Canton As String
		Public Property ChurchTax As String
		Public Property Residence As Boolean? ' Ansaessigkeit
		Public Property ChildsCount As Short?
		Public Property Profession As String
		Public Property StaysAt As String ' Wohnt_bei
		Public Property Rapports As String

		Public Property V_Hint As String ' V_Hinweis
		Public Property Notice_Employment As String
		Public Property Notice_Report As String
		Public Property Notice_AdvancedPayment As String
		Public Property Notice_Payroll As String

		Public Property CreatedOn As DateTime?
		Public Property ChangedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedFrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?
		Public Property HasImage As Boolean?
		Public Property MABild As Byte()
		Public Property Result As String
		Public Property KST As String
		Public Property InZV As Boolean?
		Public Property KStat1 As String
		Public Property KStat2 As String
		Public Property KontaktHow As String

		Public Property FirstContact As DateTime?
		Public Property LastContact As DateTime?
		Public Property QSTCommunity As String 'QST Gemeinde
		Public Property BusinessBranch As String ' Filiale
		Public Property GAVBez As String
		Public Property CivilState2 As String
		Public Property QLand As String
		Public Property MABusinessBranch As String ' MAFiliale
		Public Property AHV_Nr_New As String
		Public Property MA_Canton As String
		Public Property ANS_OST_Bis As DateTime?
		Public Property Transfered_Guid As String
		Public Property Transfered_User As String
		Public Property Transfered_On As DateTime?
		Public Property Send2WOS As Boolean?
		Public Property SendDataWithEMail As Boolean?
		Public Property WOSGuid As String
		Public Property MA_SMS_Mailing As Boolean?
		Public Property MA_EMail_Mailing As Boolean?
		Public Property ProfessionCode As Integer?
		Public Property MDNr As Integer?
		Public Property ShowAsApplicant As Boolean?
		Public Property ApplicantLifecycle As Integer?
		Public Property ApplicantID As Integer?
		Public Property CVLProfileID As Integer?
		Public Property ForeignCategory As String
		Public Property ZEMISNumber As String
		Public Property TypeOfStay As String
		Public Property EmploymentType As String
		Public Property OtherEmploymentType As String
		Public Property EmployeePartnerRecID As Integer?
		Public Property EmployeeLOHistoryRecID As Integer?
		Public Property TaxCommunityLabel As String
		Public Property TaxCommunityCode As Integer?
		Public Property ExistsHistoryData As Boolean?
		Public Property ExistsOldBackupData As Boolean?


		Public Function Clone() As EmployeeMasterData
			Return DirectCast(Me.MemberwiseClone(), EmployeeMasterData)
		End Function

		Public ReadOnly Property EmployeeFullnameWithComma As String
			Get
				Return String.Format("{1}, {0}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		''' <summary>
		''' employee address CH-5073 Gipf-Oberfrick
		''' </summary>
		''' <returns></returns>
		Public ReadOnly Property EmployeeAddress As String
			Get
				Return String.Format("{0}-{1} {2}", Country, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property EmployeeCompleteAddress As String
			Get
				Return String.Format("{0}, {1}-{2} {3}", Street, Country, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property EmployeeSUVABirthdateAge() As Integer
			Get
				If Birthdate Is Nothing Then Return 0

				Dim today As DateTime = DateTime.Now
				Dim birth As DateTime = New DateTime(Birthdate.GetValueOrDefault(Now).Year, Birthdate.GetValueOrDefault(Now).Month, Birthdate.GetValueOrDefault(Now).Day)
				Dim alter As Integer = today.Year - birth.Year
				If (today.Month < birth.Month) Then
					alter -= 1
				ElseIf (today.Month = birth.Month And today.Day < birth.Day) Then
					alter -= 1
				End If

				Return alter

			End Get
		End Property

		Public ReadOnly Property TaxCommunity As String
			Get
				Return String.Format("{0}-{1}", TaxCommunityCode.GetValueOrDefault(0), TaxCommunityLabel)
			End Get
		End Property

		' this property is just for lazy things:)
		Public ReadOnly Property PermissionLabel As String
			Get
				Select Case String.Format("{0}", Permission)
					Case "A"
						Return "Saisonarbeiterin / Saisonarbeiter"
					Case "B"
						Return "Aufenthalterin / Aufenthalter"
					Case "C"
						Return "Niedergelassene / Niedergelassener"
					Case "04"
						Return "(Ci) Erwerbstätige Ehepartnerin / erwerbstätiger Ehepartner und Kinder von Angehörigen ausländischer Vertretungen oder staatlichen internationalen Organisationen"
					Case "F"
						Return "Vorläufig Aufgenommene / vorläufig Aufgenommener"
					Case "G"
						Return "Grenzgängerin / Grenzgänger"
					Case "L"
						Return "Kurzaufenthalterin / Kurzaufenthalter"
					Case "N"
						Return "Asylsuchende / Asylsuchender"
					Case "09"
						Return "Schutzbedürftige / Schutzbedürftiger"
					Case "10"
						Return "(M) Meldepflichtige / Meldepflichtiger bei ZEMIS (Zentrales Migrationssystem)"

					Case Else
						Return String.Empty

				End Select
			End Get
		End Property


	End Class

	Public Class EmployeeNoticesData
		Public Property ID As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property V_Hint As String
		Public Property Notice_Employment As String
		Public Property Notice_Report As String
		Public Property Notice_AdvancedPayment As String
		Public Property Notice_Payroll As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedFrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedUserNumber As Integer?

	End Class

	Public Class EmployeeBackupBeforeEQuestData
		Public Property EmployeeNumber As Integer?
		Public Property Permission As String
		Public Property CHPartner As Boolean
		Public Property NoSpecialTax As Boolean
		Public Property S_Canton As String
		Public Property QSTCommunity As String
		Public Property Q_Steuer As String
		Public Property ChurchTax As String
		Public Property ChildsCount As Integer
		Public Property Residence As Boolean?
		Public Property ANS_OST_Bis As DateTime?
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property CheckedUserNumber As Integer?

	End Class


	''' <summary>
	''' Founded Propose data.
	''' </summary>
	Public Class EmployeeProposeProperty

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
	Public Class EmployeeOfferProperty

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
	Public Class EmployeeESProperty

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
	Public Class EmployeeReportsProperty

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
	''' Founded AdvancePayment data.
	''' </summary>
	Public Class EmployeeAdvancePaymentProperty

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


	''' <summary>
	''' Founded LO data.
	''' </summary>
	Public Class EmployeePayrollProperty

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
		Public Property IsComplete As Boolean?

	End Class


	''' <summary>
	''' Printed employee Documents.
	''' </summary>
	Public Class EmployeePrintedDocProperty

		Public Property recID As Integer?

		Public Property manr As Integer?

		Public Property docname As String
		Public Property username As String

		Public Property createdon As DateTime?
		Public Property createdfrom As String

		Public Property scandoc As Byte()

	End Class


	''' <summary>
	''' employee NLA Data.
	''' </summary>
	Public Class EmployeeNLAData

		Public Property recID As Integer?
		Public Property employeeNumber As Integer?

		Public Property NLA_LoAusweis As Boolean?
		Public Property NLA_Befoerderung As Boolean?
		Public Property NLA_Kantine As Boolean?

		Public Property NLA_2_3 As String
		Public Property NLA_3_0 As String
		Public Property NLA_4_0 As String
		Public Property NLA_7_0 As String
		Public Property NLA_13_1_2 As String
		Public Property NLA_13_2_3 As String
		Public Property NLA_Nebenleistung_1 As String
		Public Property NLA_Nebenleistung_2 As String
		Public Property NLA_Bemerkung_1 As String
		Public Property NLA_Bemerkung_2 As String

		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String


	End Class

	''' <summary>
	''' employee SAddress Data.
	''' </summary>
	Public Class EmployeeSAddressData

		Public Property ID As Integer?
		Public Property RecNr As Integer?
		Public Property employeeNumber As Integer?

		Public Property ForEmployment As Boolean?
		Public Property ForReport As Boolean?
		Public Property ForPayroll As Boolean?
		Public Property ForAGB As Boolean?
		Public Property ForZV As Boolean?
		Public Property ForNLA As Boolean?
		Public Property ForDivers As Boolean?

		Public Property Lastname As String
		Public Property Firstname As String
		Public Property PostOfficeBox As String
		Public Property StaysAt As String
		Public Property Street As String
		Public Property Country As String
		Public Property Location As String
		Public Property Postcode As String
		Public Property Gender As String
		Public Property ActiveRec As Boolean?

		Public Property Add_Bemerkung As String
		Public Property Add_Res1 As String
		Public Property Add_Res2 As String
		Public Property Add_Res3 As String

		Public Property Createdon As DateTime?
		Public Property Createdfrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

		Public ReadOnly Property DivAddressFullName As String
			Get
				Return String.Format("{0}, {1}", Lastname, Firstname)
			End Get
		End Property

		Public ReadOnly Property EmployeePostcodeLocation As String
			Get
				Return String.Format("{0} {1}", Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property DivAddressFullAddress As String
			Get
				Return String.Format("{0}, {1} {2}", Street, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullnameWithComma As String
			Get
				Return String.Format("{1}, {0}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		''' <summary>
		''' employee address CH-5073 Gipf-Oberfrick
		''' </summary>
		''' <returns></returns>
		Public ReadOnly Property EmployeeAddress As String
			Get
				Return String.Format("{0}-{1} {2}", Country, Postcode, Location)
			End Get
		End Property


	End Class



	Public Class EmployeePartnershipData

		Public Property ID As Integer?
		Public Property AddressNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property ExistingEmployeeNumber As Integer?

		Public Property Gender As String
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property Street As String
		Public Property Country As String
		Public Property City As String
		Public Property Postcode As String
		Public Property PostOfficeBox As String
		Public Property SocialInsuranceNumber As String
		Public Property Birthdate As DateTime?
		Public Property InEmployment As Boolean?
		Public Property ValidFrom As DateTime?
		Public Property ValidTo As DateTime?
		Public Property CreatedOn As DateTime?
		Public Property Createdfrom As String
		Public Property CreatedUserNumber As Integer?
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedUserNumber As Integer?

		Public ReadOnly Property FullName As String
			Get
				Return String.Format("{0}, {1}", Lastname, Firstname)
			End Get
		End Property

		Public ReadOnly Property ValidFromTo As String
			Get
				Return String.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", ValidFrom, ValidTo)
			End Get
		End Property

		Public ReadOnly Property AsEmployee As Boolean
			Get
				Dim value As Boolean = False

				If ExistingEmployeeNumber.GetValueOrDefault(0) > 0 Then value = True

				Return value
			End Get
		End Property

		Public ReadOnly Property Activ As Boolean
			Get
				Dim value As Boolean = False

				If ValidTo Is Nothing Then If ValidFrom <= Now.Date Then value = True
				If ValidTo >= Now.Date AndAlso ValidFrom <= Now.Date Then value = True

				Return value
			End Get
		End Property

	End Class


	''' <summary>
	''' employee Ki-Au Data.
	''' </summary>
	Public Class EmployeeChldData

		'ID, RecNr, MANr, Nachname, Vorname, Gebdat, Geschlecht, LANr, ZulageArt, Bemerkung, VonMonth, VonYear, BisMonth, BisYear, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom
		Public Property recID As Integer?
		Public Property RecNr As Integer?
		Public Property employeeNumber As Integer?

		Public Property childLastname As String
		Public Property childFirstname As String
		Public Property childsex As String

		Public Property childGebDat As Date?
		Public Property laNumber As Decimal?
		Public Property ZulageArt As String
		Public Property vonMonth As Integer?
		Public Property vonYear As Integer?
		Public Property bisMonth As Integer?
		Public Property bisYear As Integer?
		Public Property bemerkung As String

		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

		Public ReadOnly Property ChildFullname As String
			Get
				Return String.Format("{0}, {1}", childLastname, childFirstname)
			End Get
		End Property

		Public ReadOnly Property GenderLabel As String
			Get
				If String.IsNullOrWhiteSpace(childsex) Then Return String.Empty
				If childsex.ToLower = "w" Then Return "weiblich" Else Return "männlich"
			End Get
		End Property

		Public ReadOnly Property ValidFrom As String
			Get
				Return String.Format("{0:F0} - {1:F0}", vonMonth, vonYear)
			End Get
		End Property

		Public ReadOnly Property ValidTill As String
			Get
				Return String.Format("{0:F0} - {1:F0}", bisMonth, bisYear)
			End Get
		End Property

	End Class


#Region "ZV and ARGB data"

	Public Class ZVESData

		Public Property ESNR As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property CustomerName As String
		Public Property Arbzeit As String
		Public Property Arbort As String
		Public Property ES_Als As String
		Public Property ES_Ab As DateTime?
		Public Property ES_Uhr As String
		Public Property ES_Ende As DateTime?
		Public Property Ende As String
		Public Property Bemerk_MA As String
		Public Property Bemerk_KD As String
		Public Property Bemerk_RE As String
		Public Property Bemerk_Lo As String
		Public Property Bemerk_P As String

		Public Property dismissalon As DateTime?
		Public Property dismissalfor As DateTime?
		Public Property dismissalkind As String
		Public Property dismissalreason As String
		Public Property dismissalwho As String

		Public Property Bemerk_1 As String
		Public Property Bemerk_2 As String
		Public Property Bemerk_3 As String
		Public Property Print_KD As Boolean?
		Public Property Print_MA As Boolean?
		Public Property NoListing As Boolean?
		Public Property Einstufung As String
		Public Property ESBranche As String
		Public Property GoesLonger As String
		Public Property KDZHDNr As Integer?
		Public Property MDNr As Integer?


		Public Property GrundLohn As Decimal?
		Public Property StundenLohn As Decimal?
		Public Property FerBasis As Decimal?
		Public Property Ferien As Decimal?
		Public Property FerienProz As Decimal?
		Public Property Feier As Decimal?
		Public Property FeierProz As Decimal?
		Public Property Basis13 As Decimal?
		Public Property Lohn13 As Decimal?
		Public Property Lohn13Proz As Decimal?
		Public Property Tarif As Decimal?
		Public Property MAStdSpesen As Decimal?
		Public Property MATSpesen As Decimal?
		Public Property KDTSpesen As Decimal?
		Public Property MATotal As Decimal?
		Public Property KDTotal As Decimal?
		Public Property GAVNr As Integer?
		Public Property GAVKanton As String
		Public Property GAVGruppe0 As String


		Public ReadOnly Property ESFromTo As String
			Get
				Return String.Format("{0:d} - {1:d}", ES_Ab, ES_Ende)
			End Get
		End Property


		Public ReadOnly Property GAVData As String
			Get
				Return String.Format("{0:F0} - {1}", GAVNr, GAVGruppe0)
			End Get
		End Property

	End Class


	Public Class ZVHourAbsenceData

		Public Property Tag1 As Decimal
		Public Property Tag2 As Decimal
		Public Property Tag3 As Decimal
		Public Property Tag4 As Decimal
		Public Property Tag5 As Decimal
		Public Property Tag6 As Decimal
		Public Property Tag7 As Decimal
		Public Property Tag8 As Decimal
		Public Property Tag9 As Decimal
		Public Property Tag10 As Decimal
		Public Property Tag11 As Decimal
		Public Property Tag12 As Decimal
		Public Property Tag13 As Decimal
		Public Property Tag14 As Decimal
		Public Property Tag15 As Decimal
		Public Property Tag16 As Decimal
		Public Property Tag17 As Decimal
		Public Property Tag18 As Decimal
		Public Property Tag19 As Decimal
		Public Property Tag20 As Decimal
		Public Property Tag21 As Decimal
		Public Property Tag22 As Decimal
		Public Property Tag23 As Decimal
		Public Property Tag24 As Decimal
		Public Property Tag25 As Decimal
		Public Property Tag26 As Decimal
		Public Property Tag27 As Decimal
		Public Property Tag28 As Decimal
		Public Property Tag29 As Decimal
		Public Property Tag30 As Decimal
		Public Property Tag31 As Decimal

		Public Property Fehltag1 As String
		Public Property Fehltag2 As String
		Public Property Fehltag3 As String
		Public Property Fehltag4 As String
		Public Property Fehltag5 As String
		Public Property Fehltag6 As String
		Public Property Fehltag7 As String
		Public Property Fehltag8 As String
		Public Property Fehltag9 As String
		Public Property Fehltag10 As String
		Public Property Fehltag11 As String
		Public Property Fehltag12 As String
		Public Property Fehltag13 As String
		Public Property Fehltag14 As String
		Public Property Fehltag15 As String
		Public Property Fehltag16 As String
		Public Property Fehltag17 As String
		Public Property Fehltag18 As String
		Public Property Fehltag19 As String
		Public Property Fehltag20 As String
		Public Property Fehltag21 As String
		Public Property Fehltag22 As String
		Public Property Fehltag23 As String
		Public Property Fehltag24 As String
		Public Property Fehltag25 As String
		Public Property Fehltag26 As String
		Public Property Fehltag27 As String
		Public Property Fehltag28 As String
		Public Property Fehltag29 As String
		Public Property Fehltag30 As String
		Public Property Fehltag31 As String

		Public ReadOnly Property TotalAmountOfHours() As Decimal
			Get
				Dim total As Decimal = 0

				For i As Integer = 1 To 31
					total += GetWorkingHoursOfDay(i).GetValueOrDefault(0)
				Next

				Return total
			End Get
		End Property

		Public ReadOnly Property GetHourAbsence(ByVal day As Report.DataObjects.Day) As String
			Get
				Dim hour = GetWorkingHoursOfDay(day)
				Dim absence = GetAbsenceDayCodeOfDay(day)

				Return String.Format("{0:F2}{1}", hour.GetValueOrDefault(0), If(String.IsNullOrWhiteSpace(absence), String.Empty, String.Format(" - {0}", absence)))
			End Get
		End Property

		Public Function GetWorkingHoursOfDay(ByVal day As Report.DataObjects.Day) As Decimal?

			Dim columnName As String = String.Format("Tag{0}", Convert.ToInt32(day))
			Dim propertyRef = Me.GetType().GetProperty(columnName)

			Dim dayValue As Decimal? = propertyRef.GetValue(Me, Nothing)

			Return dayValue

		End Function

		Public Function GetAbsenceDayCodeOfDay(ByVal day As Report.DataObjects.Day) As String

			Dim columnName As String = String.Format("Fehltag{0}", Convert.ToInt32(day))
			Dim propertyRef = Me.GetType().GetProperty(columnName)

			Dim absenceValue As String = propertyRef.GetValue(Me, Nothing)

			Return absenceValue

		End Function


	End Class


	Public Class WorkingHourGroupedWithKSTNrData

		Public Property RPNr As Integer
		Public Property KSTNr As Integer
		Public Property KSTBez As String
		Public Property Tag1 As Decimal
		Public Property Tag2 As Decimal
		Public Property Tag3 As Decimal
		Public Property Tag4 As Decimal
		Public Property Tag5 As Decimal
		Public Property Tag6 As Decimal
		Public Property Tag7 As Decimal
		Public Property Tag8 As Decimal
		Public Property Tag9 As Decimal
		Public Property Tag10 As Decimal
		Public Property Tag11 As Decimal
		Public Property Tag12 As Decimal
		Public Property Tag13 As Decimal
		Public Property Tag14 As Decimal
		Public Property Tag15 As Decimal
		Public Property Tag16 As Decimal
		Public Property Tag17 As Decimal
		Public Property Tag18 As Decimal
		Public Property Tag19 As Decimal
		Public Property Tag20 As Decimal
		Public Property Tag21 As Decimal
		Public Property Tag22 As Decimal
		Public Property Tag23 As Decimal
		Public Property Tag24 As Decimal
		Public Property Tag25 As Decimal
		Public Property Tag26 As Decimal
		Public Property Tag27 As Decimal
		Public Property Tag28 As Decimal
		Public Property Tag29 As Decimal
		Public Property Tag30 As Decimal
		Public Property Tag31 As Decimal

		Public ReadOnly Property TotalAmountOfHours() As Decimal
			Get
				Dim total As Decimal = 0

				For i As Integer = 1 To 31
					total += GetWorkingHoursOfDay(i).GetValueOrDefault(0)
				Next

				Return total
			End Get
		End Property

		Public Function GetWorkingHoursOfDay(ByVal day As Report.DataObjects.Day) As Decimal?

			Dim columnName As String = String.Format("Tag{0}", Convert.ToInt32(day))
			Dim propertyRef = Me.GetType().GetProperty(columnName)

			Dim dayValue As Decimal? = propertyRef.GetValue(Me, Nothing)

			Return dayValue

		End Function

	End Class


	Public Class ZVPayrollData

		Public Property LONr As Integer?
		Public Property LANr As Decimal?
		Public Property TotalAnzahl As Decimal?
		Public Property TotalBasis As Decimal?
		Public Property TotalBetrag As Decimal?

		Public Property RPText As String
		Public Property Bruttopflichtig As Boolean?
		Public Property AHVpflichtig As Boolean?


	End Class


	Public Enum WOSZVSENDValue

		PrintWithoutSending
		PrintOtherSendWOS
		PrintAndSend

	End Enum


	Public Class ARGBPayrollData

		Public Property LANr As Decimal?
		Public Property RPText As String
		Public Property Year As Integer?
		Public Property Month1 As Decimal?
		Public Property Month2 As Decimal?
		Public Property Month3 As Decimal?
		Public Property Month4 As Decimal?
		Public Property Month5 As Decimal?
		Public Property Month6 As Decimal?
		Public Property Month7 As Decimal?
		Public Property Month8 As Decimal?
		Public Property Month9 As Decimal?
		Public Property Month10 As Decimal?
		Public Property Month11 As Decimal?
		Public Property Month12 As Decimal?

		Public Property Bruttopflichtig As Boolean?
		Public Property AHVpflichtig As Boolean?
		Public Property ARGB_Verdienst_Unterkunft As Boolean?
		Public Property ARGB_Verdienst_Mahlzeit As Boolean?


		Public ReadOnly Property KumulativAmount() As Decimal
			Get
				Return Month1.GetValueOrDefault(0) + Month2.GetValueOrDefault(0) + Month3.GetValueOrDefault(0) + Month4.GetValueOrDefault(0) + Month5.GetValueOrDefault(0) +
						Month6.GetValueOrDefault(0) + Month7.GetValueOrDefault(0) + Month8.GetValueOrDefault(0) + Month9.GetValueOrDefault(0) + Month10.GetValueOrDefault(0) +
						Month11.GetValueOrDefault(0) + Month12.GetValueOrDefault(0)
			End Get
		End Property
	End Class


	Public Class ARGBAHVPayrollData

		Public Property MonthBefore As Integer
		Public Property AHVAmount As Decimal

	End Class

#End Region


End Namespace