Namespace Invoice.DataObjects

  ''' <summary>
  ''' The Invoice (TABLE [dbo].[RE])
  ''' </summary>
  Public Class Invoice
    Public Property Id As Integer?
    Public Property ReNr As Integer?
    Public Property KdNr As Integer?
    Public Property Art As String
    Public Property KST As String
		Public Property Lp As Integer?
    Public Property FakDat As DateTime?
    Public Property Currency As String
    Public Property BetragOhne As Decimal?
    Public Property BetragEx As Decimal?
		Public Property BetragInk As Decimal?
    Public Property MWST1 As Decimal?
    Public Property MWSTProz As Decimal?
    Public Property Bezahlt As Decimal?
    Public ReadOnly Property IsPayed As Boolean
      Get
        Return Bezahlt.HasValue AndAlso Bezahlt > 0
      End Get
    End Property
    Public Property SKonto As Decimal?
    Public Property Verlust As Decimal?
    Public Property FSKonto As Decimal?
    Public Property FVerlust As Decimal?
    Public Property Faellig As DateTime?
    Public Property Mahncode As String
    Public Property SPNr As Integer?
    Public Property VerNr As Integer?
    Public Property MA0 As DateTime?
    Public Property MA1 As DateTime?
    Public Property MA2 As DateTime?
    Public Property MA3 As DateTime?
    Public Property Storno As Boolean
    Public Property Gebucht As Boolean
    Public Property FBMonat As Int16?
    Public Property FBDat As DateTime?
    Public Property FKSoll As Integer?
    Public Property FKHaben0 As Integer?
    Public Property FKHaben1 As Integer?
    Public Property RName1 As String
    Public Property RName2 As String
		Public Property RName3 As String
		Public Property CustomerLanguage As String

    Public Property RZHD As String
    Public Property RPostfach As String
    Public Property RStrasse As String
    Public Property RLand As String
    Public Property RPLZ As String
    Public Property ROrt As String
		Public Property ReMail As String
		Public Property SendAsZip As Boolean?
		Public Property Zahlkond As String
		Public Property Result As String
    Public Property RefNr As String
    Public Property RefFootNr As String
    Public Property ESRArt As String
    Public Property ESRID As String
    Public Property ESRKonto As String
    Public Property MWSTNr As String
    Public Property KontoNr As String
    Public Property BtrFr As Integer?
    Public Property btrRp As String
    Public Property REKST1 As String
    Public Property REKST2 As String
    Public Property PrintedDate As String
    Public Property GebuchtAm As DateTime?
    Public Property ZEInfo As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property RAbteilung As String
    Public Property Ma3RepeatNr As Integer?
    Public Property EsEinstufung As String
    Public Property KDBranche As String
    Public Property DTAName As String
    Public Property DTAPLZOrt As String
    Public Property ESRBankName As String
    Public Property ESRBankAdresse As String
    Public Property DTAKonto As String
    Public Property IBANDTA As String
    Public Property IBANVG As String
    Public Property EsrSwift As String
    Public Property EsrIBAN1 As String
    Public Property EsrIBAN2 As String
    Public Property EsrIBAN3 As String
    Public Property EsrBcNr As String
    Public Property ProposeNr As Integer?
    Public Property Art2 As String
    Public Property MahnStopUntil As DateTime?
    Public Property REDoc_Guid As String
    Public Property Transfered_User As String
    Public Property Transfered_On As String
    Public Property ZEBis0 As DateTime?
    Public Property ZEBis1 As DateTime?
    Public Property ZEBis2 As DateTime?
    Public Property ZEBis3 As DateTime?
    Public Property MDNr As Integer?
		Public Property ESRBankID As Integer?
		Public Property PaymentExtraAmounts As List(Of PaymentExtraData)

		Public Property IsSelected As Boolean?

		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", RPLZ, ROrt)
			End Get
		End Property

		Public ReadOnly Property InvoiceViewData As String
			Get
				Return String.Format("{0} >>> {1:dd.MM.yyyy} - {2:n2} - {3:n2}", RName1, FakDat, BetragInk.GetValueOrDefault(0), OpenAmount)
			End Get
		End Property

		Public ReadOnly Property OpenAmount As Decimal?
			Get
				Return BetragInk.GetValueOrDefault(0) - Bezahlt.GetValueOrDefault(0)
			End Get
		End Property

	End Class


	Public Class NewPaymentInitData

		Public Property MDNr As Integer
		Public Property RENR As Integer
		Public Property KDNR As Integer
		Public Property VDate As DateTime?
		Public Property BDate As DateTime?
		Public Property Currency As String
		Public Property Amount As Decimal?
    Public Property FKSOLL As Integer?

    Public Property FKHABEN As Integer?
    Public Property CreatedFrom As String

		Public Property IdNewPayment As Integer?
		Public Property NewPaymentNr As Integer?
    Public Property PaymentNumberOffset As Integer
    Public Property NewPaymentID As Integer


  End Class

	Public Class PaymentMasterData

		Public Property ID As Integer
		Public Property MDNr As Integer?
		Public Property ZENr As Integer?
		Public Property RENR As Integer?
		Public Property KDNR As Integer?
		Public Property FakDate As DateTime?
		Public Property VDate As DateTime?
    Public Property BDate As DateTime?
    Public Property Currency As String
    Public Property InvoiceAmount As Decimal?
    Public Property Amount As Decimal?
    Public Property MWSTAmount As Decimal?
    Public Property InvoiceTaxPercent As Decimal?

    Public Property VD As String
    Public Property VT As String
    Public Property VL As String
    Public Property FBMONAT As Integer?
    Public Property FBDAT As DateTime?
    Public Property FKSOLL As Integer?
    Public Property FKHABEN As Integer?
    Public Property Storniert As Boolean?
    Public Property MWST As Decimal?
    Public Property DiskInfo As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property ZeInfo As String
    Public Property KST As String
    Public Property REKST1 As String
    Public Property REKST2 As String
    Public Property REArt As String
    Public Property REArt2 As String
		Public Property PaymentExtraAmounts As List(Of PaymentExtraData)


		Public ReadOnly Property PaymentRestAmount As Decimal?
      Get
        Return InvoiceAmount - Amount
      End Get
    End Property

    Public ReadOnly Property PaymentRestAmountPercent As Decimal?
			Get
        Dim intWert As Integer
        If InvoiceAmount = 0 Then
          'Divsion by '0' abfangen
          Return 0
        Else
          intWert = (100 - ((Amount / InvoiceAmount) * 100)) * 100
          Return intWert / 100
        End If

      End Get
		End Property

    Public ReadOnly Property InvoiceWithTax As Boolean?
      Get
        Return (InvoiceTaxPercent.GetValueOrDefault(0) <> 0)
      End Get
    End Property

  End Class


	Public Class PaymentExtraData

		Public Property ZENr As Integer?
		Public Property FKSollKonto As Integer?
		Public Property Amount As Decimal?
		Public Property ValutaDate As DateTime?
		Public Property BookingDate As DateTime?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class



	' Marker interface
	Public Interface IInvoiceRow
  End Interface

  ''' <summary>
  ''' The Invoice (TABLE [dbo].[RE_Ind])
  ''' </summary>
  Public Class InvoiceIndividual
    Implements IInvoiceRow
    Public Property Id As Integer?
    Public Property RENr As Integer?
    Public Property KDNr As Integer?
    Public Property BetragTotal As Decimal?
    Public Property MWST1 As Decimal?
    Public Property BetragEx As Decimal?
    Public Property Currency As String
    Public Property Monat As Int16?
    Public Property Jahr As String
    Public Property ReText As String
    Public Property RecNr As Integer
    Public Property ReHeadText As String

  End Class

  ''' <summary>
  ''' The Invoice (TABLE [dbo].[RPL])
  ''' </summary>
  Public Class InvoiceRPL
    Implements IInvoiceRow
    Public Property RPNr As Integer?
    Public Property MANr As Integer?
    Public Property ESNr As Integer?
    Public Property LANr As Decimal?
    Public Property VonDate As DateTime?
    Public Property BisDate As DateTime?
    Public Property KAnzahl As Decimal?
    Public Property KBasis As Decimal?
    Public Property KAnsatz As Decimal?
    Public Property KBetrag As Decimal?
    Public Property RPLNr As Integer?
    Public Property KSTNr As Integer?
    Public Property kstname As String
    Public Property MWST As Decimal?
    Public Property LAOpText As String
    Public Property RPTId As Integer?
    Public Property RPText As String
  End Class


  ''' <summary>
  ''' Conflicted Month clsoe data.
  ''' </summary>
  Public Class ConflictedMonthCloseData

		Public Property Month As Integer?
    Public Property Year As Integer?

  End Class


  Public Class InvoicePaymentProperty

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
	''' Customer data (Kunde).
	''' </summary>
	Public Class CustomerOverviewAutomatedInvoiceData
		Public Property CustomerMandantNumber As Integer
		Public Property CustomerNumber As Integer
		Public Property ReportLineBetrag As Decimal?
		Public Property Company1 As String
		Public Property Street As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property BillTypeCode As String
		Public Property InvoiceOption As String
		Public Property IsSelected As Boolean?

		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", Postcode, Location)
			End Get
		End Property

	End Class


	Public Class ReportOverviewAutomatedInvoiceData

		Public Property RPNr As Integer
		Public Property ReportLineBetrag As Decimal?

		Public Property KstNr As Integer?
		Public Property KSTBez As String
		Public Property KSTAddNr As Integer?
		Public Property ESNr As Integer
		Public Property EmployeeNumber As Integer
		Public Property CustomerNumber As Integer
		Public Property EmployeeLastname As String
		Public Property EmployeeFirstname As String
		Public Property Company1 As String
		Public Property ReportFrom As DateTime?
		Public Property ReportTo As DateTime?
		Public Property ReportMonth As Integer
		Public Property ReportYear As Integer
		Public Property ReportlineWeekFrom As Integer
		Public Property Erfasst As Boolean
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property IsSelected As Boolean?

		Public ReadOnly Property EmployeeFullName As String
			Get

				If String.IsNullOrWhiteSpace(EmployeeFirstname) Then
					Return EmployeeLastname
				Else
					Return String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname)
				End If

			End Get
		End Property

		Public ReadOnly Property ReportMonthAndYear As String
			Get
				Return String.Format("{0:00}, {1:0000}", ReportMonth, ReportYear)
			End Get
		End Property

	End Class



	Public Class EmploymentOverviewAutomatedInvoiceData

		Public Property ESNr As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property ReportLineBetrag As Decimal?
		Public Property ES_Als As String
		Public Property ES_Ab As DateTime?
		Public Property ES_Ende As DateTime?
		Public Property RPLYear As Integer?
		Public Property KDZHDNr As Integer?
		Public Property MDNr As Integer?
		Public Property EmployeeFirstname As String
		Public Property EmployeeLastname As String
		Public Property Company1 As String

		Public Property IsSelected As Boolean?

		Public ReadOnly Property EmployeeFullName As String
			Get

				If String.IsNullOrWhiteSpace(EmployeeFirstname) Then
					Return EmployeeLastname
				Else
					Return String.Format("{0}, {1}", EmployeeLastname, EmployeeFirstname)
				End If

			End Get
		End Property



	End Class



	Public Class ReportLineOverviewAutomatedInvoiceData

		Public Property CustomerNumber As Integer
		Public Property Company1 As String
		Public Property Street As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property BillTypeCode As String
		Public Property InvoiceOption As String
		Public Property ReportLineBetrag As Decimal?
		Public Property IsSelected As Boolean?

		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", Postcode, Location)
			End Get
		End Property

	End Class


	Public Class ReportLineCreatingAutomatedInvoiceData

		Public Property RPNr As Integer
		Public Property RPLNr As Integer
		Public Property ESNr As Integer
		Public Property CustomerNumber As Integer
		Public Property ReportLineBetrag As Decimal?
		Public Property MwSt As Decimal?
		Public Property ReportMonth As Integer
		Public Property ReportYear As Integer
		Public Property ES_Einstufung As String
		Public Property KDBranche As String
		Public Property RPKst1 As String
		Public Property RPKst2 As String
		Public Property RPKst As String
		Public Property BetragInkMwStTotal As Decimal?
		Public Property BetragOhneMwStTotal As Decimal?
		Public Property RPLID As String
		Public Property IsSelected As Boolean?


	End Class


	Public Class DunningAndArrearsData

		Public Property MDNr As Integer
		Public Property RENr As Integer
		Public Property KDNr As Integer
		Public Property FKSoll As Integer
		Public Property FKHaben0 As Integer
		Public Property FKHaben1 As Integer
		Public Property MwStProz As Decimal?
		Public Property SPNumber As Integer
		Public Property SPDate As Date

		Public Property BetragInk As Decimal?
		Public Property BetragEx As Decimal?
		Public Property DunningAmount As Decimal?
		Public Property SP_BetragTotal As Decimal?
		Public Property SP_Bezahlt As Decimal?

		Public Property SP_Text As String
		Public Property MwStNr As String
		Public Property ESRArt As String
		Public Property ESRID As String
		Public Property ESRKonto As String
		Public Property KontoNr As String


	End Class


	''' <summary>
	''' dunning date.
	''' </summary>
	Public Class DunningDateData

		Public Property DunningCount As Integer?
		Public Property DunningDate As Date?

	End Class


End Namespace
