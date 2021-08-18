Namespace Employee.DataObjects.MonthlySalary

	''' <summary>
	''' Monthly salary overview data.
	''' </summary>
	Public Class MonthlySalaryOverviewData

		Public Property MDNr As Integer?
		Public Property LMNr As Integer?
		Public Property ESNr As Decimal?
		Public Property LANr As Decimal?
		Public Property LAName As String
		Public Property LP_From As Integer?
		Public Property Year_From As String
		Public Property LP_To As Integer?
		Public Property Year_To As String
		Public Property M_Anz As Decimal?
		Public Property M_Ans As Decimal?
		Public Property M_Bas As Decimal?
		Public Property M_BTR As Decimal?
		Public Property LAIndBez As String
		Public Property Canton As String
		Public Property ZGGrund As String
		Public Property BankNr As Integer?
		Public Property Sign As String
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property NumberOfExistingLOLRecords As Integer
		Public Property NumberOfExistingLMDocRecords As Integer
	End Class

End Namespace