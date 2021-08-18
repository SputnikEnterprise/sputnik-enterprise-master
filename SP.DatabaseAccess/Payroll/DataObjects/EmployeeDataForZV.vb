Namespace PayrollMng.DataObjects

  Public Class EmployeeDataForZV

    Public Property MANr As Integer
    Public Property FirstName As String
		Public Property LastName As String
		Public Property ZVPrinted As Boolean?
		Public Property ARGBPrinted As Boolean?

		Public ReadOnly Property EmployeeFullname As String
			Get
				Return (String.Format("{0} {1}", FirstName, LastName))
			End Get
		End Property

		Public ReadOnly Property EmployeeFullnameWithComma As String
			Get
				Return (String.Format("{1}, {0}", FirstName, LastName))
			End Get
		End Property

	End Class

End Namespace
