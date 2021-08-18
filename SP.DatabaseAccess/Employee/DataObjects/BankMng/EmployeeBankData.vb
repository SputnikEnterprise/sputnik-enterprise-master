Namespace Employee.DataObjects.BankMng

    ''' <summary>
    ''' Employee bank data.
    ''' </summary>
    Public Class EmployeeBankData
        Public Property ID As Integer
        Public Property EmployeeNumber As Integer
        Public Property RecordNumber As Short?
        Public Property Bank As String
        Public Property BankLocation As String
        Public Property DTABCNR As String
        Public Property AccountNr As String
        Public Property DTAAdr1 As String
        Public Property DTAAdr2 As String
        Public Property DTAAdr3 As String
        Public Property DTAAdr4 As String
        Public Property ActiveRec As Boolean?
        Public Property Result As String
        Public Property BankZG As Boolean?
        Public Property CreatedOn As DateTime?
        Public Property CreatedFrom As String
        Public Property ChangedOn As DateTime?
        Public Property ChangedFrom As String
        Public Property BankAU As Boolean?
        Public Property IBANNr As String
        Public Property Swift As String
        Public Property BLZ As String
        Public Property LMLAnr As String
		Public Property BnkLOL As Boolean?

		Public Property zahlart As String

	End Class

End Namespace