
Namespace Listing.DataObjects


	Public Class InvoiceDunningPrintData
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

		Public Property SPText As String
		Public Property OPBetragEx As Decimal?
		Public Property OPBetragInk As Decimal?
		Public Property SPBetrag As Decimal?
		Public Property SPMwStProz As Decimal?
		Public Property SPBetragTotal As Decimal?
		Public Property SPBezahlt As Boolean?
		Public Property SPDate As Date?

		Public Property SPRefNr As String
		Public Property SPRefFootNr As String
		Public Property SPESRID As String
		Public Property SPESRKonto As String
		Public Property SPMwStNr As String
		Public Property SPKontoNr As String
		Public Property SPBtrFr As Integer
		Public Property SPBtrRr As Integer


		Public Property IsSelected As Boolean?

		Public ReadOnly Property CustomerPostcodeLocation As String
			Get
				Return String.Format("{0} {1}", RPLZ, ROrt)
			End Get
		End Property

	End Class


End Namespace