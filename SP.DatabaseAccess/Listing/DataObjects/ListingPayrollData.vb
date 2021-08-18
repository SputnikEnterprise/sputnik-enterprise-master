
Namespace Listing.DataObjects


	Public Class DB1CalculationData
		Public Property KST As String
		Public Property AdvisorName As String
		Public Property PayrollListData As List(Of DB1PayrollKSTDetailData)
		Public Property AGDetailData As List(Of DB1PayrollAGAnteilData)

		Public ReadOnly Property KSTViewData As String
			Get
				Return String.Format("{0} ({1})", KST, AdvisorName)
			End Get
		End Property

	End Class

	Public Class DB1PayrollKSTDetailData
		Public Property DataType As DB1DataRecordType
		Public Property DataDetails As List(Of DB1PayrollData)
		Public Property DataTypeAmount As Decimal?


		Public ReadOnly Property DataTypeLabel As String
			Get
				Select Case DataType
					Case DB1DataRecordType.BRUTTOLOHN
						Return "Bruttohn"
					Case DB1DataRecordType.AGANTEIL
						Return "AG-Anteile"
					Case DB1DataRecordType.FEIERAUS
						Return "Auszahlung Feiertag"
					Case DB1DataRecordType.AHVLOHN
						Return "AHV-Lohn"
					Case DB1DataRecordType.FEIERBACKED
						Return "Rückstellung Feiertag"
					Case DB1DataRecordType.FERIENAUS
						Return "Auszahlung Ferien"
					Case DB1DataRecordType.FERIENBACKED
						Return "Rückstellung Ferien"
					Case DB1DataRecordType.FREMDLEISTUNGLOHN
						Return "Fremdleistungen"

					Case DB1DataRecordType.GLEITZEITAUS
						Return "Auszahlung Gleitzeit"
					Case DB1DataRecordType.GLEITZEITBACK
						Return "Rückstellung Gleitzeit"
					Case DB1DataRecordType.LO13AUS
						Return "Auszahlung 13. Lohn"
					Case DB1DataRecordType.LO13BACK
						Return "Rückstellung 13. Lohn"

					Case DB1DataRecordType.TotalBetragAus
						Return "Total Auszahlung"
					Case DB1DataRecordType.TotalBetragBack
						Return "Total Rückstellung"
					Case DB1DataRecordType.XMARGE
						Return "Admin.-Kosten"
					Case DB1DataRecordType.AGBEITRAG
						Return "AG-Beiträge"


					Case Else
						Return "Not defined!"

				End Select

			End Get
		End Property

	End Class

	Public Class DB1PayrollData
		Public Property PayrollNumber As Integer?
		Public Property ReportNumber As Integer?
		Public Property LANumber As Decimal?
		Public Property Amount As Decimal?

	End Class


	Public Enum DB1DataRecordType

		BRUTTOLOHN
		AHVLOHN

		FERIENBACKED
		FEIERBACKED
		LO13BACK
		GLEITZEITBACK

		FERIENAUS
		FEIERAUS
		LO13AUS
		GLEITZEITAUS

		TotalBetragBack
		TotalBetragAus
		FREMDLEISTUNGLOHN

		AGBEITRAG
		XMARGE
		AGANTEIL

	End Enum


	Public Class DB1PayrollAGAnteilData

		Public Property PayrollNumber As Integer?
		Public Property AHVAnteil As Decimal?
		Public Property AHVAmountEachEmployee As Decimal?
		Public Property AGAnteil As Decimal?
		Public Property AGAmountEachEmployee As Decimal?
		Public Property AGBVGAmountEachEmployee As Decimal?
		Public Property AGKSTProcent As Decimal?
		Public Property AGEOAmount As Decimal?

	End Class


	''' <summary>
	''' Result of payroll (payroll) deletion.
	''' </summary>
	''' <remarks></remarks>
	Public Enum DeletePayrollResult
		Deleted = 2
		CouldNotDeleteBecauseOfExistingPropose = 4

		CouldNotDeleteBecauseOfExistingES = 10
		CouldNotDeleteBecauseOfExistingRP = 11
		CouldNotDeleteBecauseOfExistingZG = 12

		CouldNotDeleteBecauseOfExistingLM = 13
		CouldNotDeleteBecauseOfExistingLO = 14

		ErrorWhileDelete = 20
	End Enum



End Namespace
